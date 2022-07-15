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

public partial class om_multi_account_upt : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, nVty = "";
    DataTable dt, dt2, dt3, dt4;
    DataRow oporow, oporow1, oporow2; DataSet oDS, oDS1, oDS2;
    int i = 0, z = 0,flag=0;

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

    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = true;  btndel.Disabled = true;
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
        doc_nf.Value = "acode";
        doc_df.Value = "acode";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "famst";

        lblheader.Text = "Account Multi Updating";

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
                SQuery = "SELECT  type1 as fstr, type1 as ledger_code, name as NAME FROM TYPE WHERE ID='Z' order by fstr";
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
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        FileUpload1.Enabled = true;
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            //hffield.Value = "New";
            //make_qry_4_popup();
            //fgen.Fn_open_sseek("-", frm_qstr);

            // else comment upper code

            //frm_vnum = fgen.next_no(frm_qstr, frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
           // frm_vty = "ZZ";
           // frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
           // txtvchnum.Text = frm_vnum;
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
       
        //DataView view = new DataView(dtn);
        //DataTable distinctValues = view.ToTable(true,"item_name");
        //if (distinctValues.Rows.Count != dtn.Rows.Count)

        //{

        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You cannot insert duplicate items. Please update it ");
        //    return;
        //}



        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        hfCNote.Value = "Y";
        if (txtAname.Value.ToString().ToUpper().Contains("MARUTI"))
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
                case "TACODE":
                    txtacode.Value = col1;
                    txtAname.Value = col2;
                    break;
                case "TRCODE":

                    SQuery = "SELECT GRP as LEDGRP,ACODE as PARTYCODE ,ANAME as ACCT_NAME ,PNAME AS ALIAS_NAME,GIRNO AS PAN_NO,GST_NO AS GST_NO,ADDR1,ADDR2,ADDR3,ADDR4,STATEN AS STATE_NAME,COUNTRY,CIN_NO,EMAIL AS EMAILID,PERSON AS CONTACT_PERSON,GSTPERSON AS COM_ACNT,MOBILE FROM FAMST WHERE GRP IN ("+col1+")";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt.Columns.Add("SRNO", typeof(int)).SetOrdinal(0);
                   // dt.Columns.Add("Duplicate", typeof(string));
                    //dt.Columns.Add("ReasonOfFailure", typeof(string));
                   // dt.Columns.Add("dtsrno", typeof(int)); // for development point of view
                    if (dt.Rows.Count > 0) fgen.exp_to_excel(dt, "ms-excel", "xls", frm_cocd + "_" + DateTime.Now.ToString().Trim());
                    else fgen.msg("-", "AMSG", "No Data to Export");
                    dt.Dispose();
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    //fgen.Fn_open_rptlevel("Report", frm_qstr);

                 
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
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "SELECT a.vchnum as entryno,to_char(a.vchdate,'dd/mm/yyyy') as entrydt,a.col1 as invno,a.col2 as invdt,b.aname as customer,c.cpartno as partno,c.iname as part_name,a.col6 as qty_sold,a.col7 as old_rate,a.col8 as rev_rate,a.col9 as diff,a.col10 as diffval,a.col11 as pono,a.col12 as podt,a.col13 check_sheet_no FROM SCRATCH2 A,famst b,item c WHERE trim(a.acode)=trim(B.acode) and trim(A.icode)=trim(c.icode) and A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + DateRange + " ORDER BY A.COL33";

            SQuery = "select DISTINCT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL11 AS INVNO,A.COL12 AS INV_DATE,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,a.DIFF AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,a.TOTAL AS TOTAL,A.COL29 AS HSCODE,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code from (SELECT distinct a.acode,a.vchdate,a.icode,A.COL33,A.COL34 ,A.COL35 ,A.COL2 ,A.COL1 ,A.COL3 ,A.COL11 ,A.COL12 ,A.COL13 ,A.COL14 ,A.COL16 ,A.COL26 ,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 ,A.COL18 ,A.COL19 ,A.COL20 ,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 FROM SCRATCH2 A  WHERE a.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and a.num10>0 ) a, ivoucher b where TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL11)||TO_CHAR(TO_DATE(A.COL12,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(b.location)||to_char(b.vchdate,'dd/mm/yyyy') and b.type in ('58','59') order by a.col33";
            SQuery = "SELECT distinct A.COL33 AS BATCH_NO,A.COL4 AS PART_NO,A.COL5 AS PART_NAME,A.COL11 AS PO_NO,A.COL1 AS INVNO,A.COL2 AS INV_DATE ,A.COL6 AS QTY,A.COL7 AS OLD_RATE,A.COL8 AS NEW_RATE,(TO_NUMBER(A.COL7)-TO_NUMBER(A.COL8)) AS DIFF,a.col13 check_sheet_no,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code FROM SCRATCH2 A,ivoucher B WHERE a.BRANCHCD||TRIM(a.ACODE)||TRIM(A.ICODE)||TRIM(A.COL1)||TRIM(A.COL33)=B.BRANCHCD||TRIM(B.ACODE)||TRIM(B.ICODe)||trim(b.invno)||TRIM(B.LOCATION) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND a.vchdate " + PrdRange + " and b.type in ('58','59') and a.num10>0 order by a.col33 ";
            SQuery = "SELECT distinct a.acode,c.aname as customer,A.COL4 AS PART_NO,A.COL5 AS PART_NAME,A.COL1 AS INVNO,A.COL2 AS INV_DATE ,A.COL6 AS QTY,A.COL8 AS NEW_RATE,(TO_NUMBER(A.COL7)-TO_NUMBER(A.COL8)) AS DIFF,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code FROM SCRATCH2 A,ivoucher B,famst c WHERE a.BRANCHCD||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)||trim(a.col1)||trim(A.col2)||a.col6=trim(b.btchno)||trim(B.acode)||trim(b.icode)||trim(b.invno)||to_char(b.invdate,'dd/mm/yyyy')||trim(b.iqty_chl) and trim(a.acode)=trim(C.acodE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND a.vchdate " + PrdRange + " and b.type in ('58','59') order by a.col1 ";
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
                //last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and " + doc_df.Value + " " + DateRange + " ", "ldt");
                //if (last_entdt == "0")
                //{ }
                //else
                //{
                //    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                //    {
                //        Checked_ok = "N";
                //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                //    }
                //}
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

                            //if (save_it == "Y")
                            //{
                            //    i = 0;
                            //    do
                            //    {
                            //        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                            //        //frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where ICODE='" + doc_df.Value + "'", 8, "vch");

                            //        pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                            //        if (i > 20)
                            //        {
                            //            fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                            //            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                            //            pk_error = "N";
                            //            i = 0;
                            //        }
                            //        i++;
                            //    }
                            //    while (pk_error == "Y");
                            //}
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        ViewState["refNo"] = frm_vnum;
                        save_fun();

                        if (edmode.Value == "Y")
                        {
                            //cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            //fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        //fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        //save_fun2();

                        if (edmode.Value == "Y")
                        {
                            //fgen.msg("-", "AMSG", "Data Updated Successfully");
                            //cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            //fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
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
                    acnat = gr1["LEDGRP"].ToString().Trim();
                    switch (acnat)
                    {
                        case "05":
                        case "06":
                        case "16":
                        case "02":
                            chk_code = fgen.seek_iname(frm_qstr, frm_cocd, "select max(substr(acode,4,3)) as existcd from famst where branchcd!='DD' and substr(acode,3,1)='" + gr1["ACCT_NAME"].ToString().Trim().Substring(0, 1) + "' and trim(nvl(GRP,'-'))='" + gr1["LEDGRP"].ToString().Trim() + "' ", "existcd");
                            break;
                        default:
                            chk_code = fgen.seek_iname(frm_qstr, frm_cocd, "select max(substr(acode,3,4)) as existcd from famst where branchcd!='DD' and trim(nvl(GRP,'-'))='" + gr1["LEDGRP"].ToString().Trim() + "' ", "existcd");
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
                        if (acnat == "05" || acnat == "06" || acnat == "16" ||acnat == "02")
                        {
                            chk_code = fgen.seek_iname(frm_qstr, frm_cocd, "select lpad(trim(max(substr(acode,4,3))+1),3,'0') as existcd from famst where branchcd!='DD' and substr(acode,3,1)='" + gr1["ACCT_NAME"].ToString().Trim().Substring(0, 1) + "' and trim(nvl(GRP,'-'))='" + gr1["LEDGRP"].ToString().Trim() + "' ", "existcd");
                            oporow["acode"] = acnat + gr1["ACCT_NAME"].ToString().Trim().Substring(0, 1) + chk_code;
                        }
                        else
                        {
                            chk_code = fgen.seek_iname(frm_qstr, frm_cocd, "select lpad(trim(max(substr(acode,3,4))+1),4,'0') as existcd from famst where branchcd!='DD' and trim(nvl(GRP,'-'))='" + gr1["LEDGRP"].ToString().Trim() + "' ", "existcd");
                            oporow["acode"] = acnat + chk_code;
                        }

                    }
                }
                oporow["BRANCHCD"] = frm_mbr;
           
                //CHANGED CODE FOR GRID
              
                oporow["grp"] = gr1["LEDGRP"].ToString().Trim();
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
                oporow["rateint"] =0;
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
                oporow["hr_ml"] ="-";
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

                        oporow1["ACODE"] = txtacode.Value.Trim();
                        oporow1["VCODE"] = txtacode.Value.ToString().Trim();
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
                        par_code = txtacode.Value.Trim();
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
        string sal_code = "", par_code = "", tax_code = "", tax_code2 = "", schg_code = "", iopr = "";
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
            #region Complete Save Function
            DataView dv = new DataView(dtW, "", "invno,invdt", DataViewRowState.CurrentRows);
            dt = new DataTable();
            dt = dv.ToTable(true, "invno", "invdt");
            foreach (DataRow dr in dt.Rows)
            {
                dt2 = new DataTable();
                dv = new DataView(dtW, "invno='" + dr["invno"].ToString().Trim() + "' and invdt='" + dr["invdt"].ToString().Trim() + "'", "icode", DataViewRowState.CurrentRows);
                dt3 = new DataTable();
                dt3 = dv.ToTable();

                oDS1 = new DataSet();
                oporow1 = null;
                oDS1 = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");
                string newVnum = "Y";
                string branchcd = mhd;
                string invRmrk = "";
                string batchNo = "";
                double dValTot = 0;
                double dVal1Tot = 0;
                double dVal2Tot = 0;
                foreach (DataRow drw in dt3.Rows)
                {
                    saveTo = "Y";
                    if (saveTo == "Y")
                    {
                        mhd = fgen.seek_iname_dt(dtSale, "fstr='" + fgen.padlc(Convert.ToInt32(drw["invno"].ToString().Trim()), 6) + Convert.ToDateTime(drw["invdt"].ToString().Trim()).ToString("dd/MM/yyyy") + "'", "branchcd");
                        if (mhd != "0")
                        {
                            branchcd = mhd;
                            invRmrk = "";
                            dVal = 0;
                            dVal1 = 0;
                            dVal2 = 0;

                            //*******************

                            oporow1 = oDS1.Tables[0].NewRow();
                            oporow1["BRANCHCD"] = branchcd;

                            if (fgen.make_double(drw["rrate"].ToString().Trim()) > 0) nVty = "59";
                            else nVty = "58";
                            //nVty = "59";

                            oporow1["TYPE"] = nVty;

                            if (newVnum == "Y")
                            {
                                i = 0;
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, "IVOUCHER", "VCHNUM", "VCHDATE", branchcd, nVty, txtvchdate.Text.Trim(), frm_uname, frm_formID);
                                newVnum = "N";
                            }

                            batchNo = drw["pono"].ToString().Trim();

                            oporow1["LOCATION"] = batchNo;

                            oporow1["vchnum"] = frm_vnum;
                            oporow1["vchdate"] = txtvchdate.Text.Trim();

                            oporow1["ACODE"] = txtacode.Value.Trim();
                            oporow1["VCODE"] = txtacode.Value.ToString().Trim();
                            oporow1["ICODE"] = drw["icode"].ToString().Trim();

                            oporow1["REC_ISS"] = "C";

                            oporow1["IQTYIN"] = 0;
                            oporow1["IQTYOUT"] = 0;

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

                            dValTot += dVal;

                            oporow1["NO_CASES"] = drw["hscode"].ToString().Trim();
                            oporow1["EXC_57F4"] = drw["CPARTNO"].ToString().Trim();

                            if (fgen.make_double(drw["IGST"].ToString().Trim()) > 0)
                            {
                                oporow1["IOPR"] = "IG";
                                iopr = "IG";

                                oporow1["EXC_RATE"] = drw["IGST"].ToString().Trim();
                                dVal1 = Math.Round(dVal * (fgen.make_double(drw["IGST"].ToString().Trim()) / 100), 2);

                                dVal1Tot += dVal1;
                                oporow1["EXC_AMT"] = Math.Round(dVal1, 2);
                            }
                            else
                            {
                                iopr = "CG";
                                oporow1["IOPR"] = "CG";
                                oporow1["EXC_RATE"] = drw["CGST"].ToString().Trim();
                                dVal1 = Math.Round(dVal * (fgen.make_double(drw["CGST"].ToString().Trim()) / 100), 2);

                                dVal1Tot += dVal1;
                                oporow1["EXC_AMT"] = Math.Round(dVal1, 2);

                                oporow1["CESS_PERCENT"] = drw["SGST"].ToString().Trim();
                                dVal2 = Math.Round(dVal * (fgen.make_double(drw["SGST"].ToString().Trim()) / 100), 2);

                                dVal2Tot += dVal2;
                                oporow1["CESS_PU"] = Math.Round(dVal2, 2);
                            }


                            oporow1["STORE"] = "N";
                            oporow1["MORDER"] = 1;
                            oporow1["SPEXC_RATE"] = dVal;
                            oporow1["SPEXC_AMT"] = dVal + dVal1 + dVal2;

                            oporow1["RCODE"] = sal_code;
                            oporow1["MATTYPE"] = "12";

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

                            l++;
                        }
                    }
                }
                fgen.save_data(frm_qstr, frm_cocd, oDS1, "IVOUCHER");
                //*******************
                par_code = txtacode.Value.Trim();                
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

                if (txtRcode.Value.Trim().Length > 2) sal_code = txtRcode.Value.Trim();

                //***********************
                #region Voucher Saving
                batchNo = "W" + batchNo;

                if (nVty == "58")
                {
                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, sal_code, par_code, dVal1Tot, 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");
                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 2, tax_code, par_code, dVal1Tot, 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");

                    if (tax_code2.Length > 0)
                    {
                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 3, tax_code2, par_code, dVal2Tot, 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");
                    }
                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, par_code, sal_code, 0, fgen.make_double(dValTot + dVal1Tot + dVal2Tot, 2), frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");
                }
                else
                {
                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, sal_code, fgen.make_double(dValTot + dVal1Tot + dVal2Tot, 2), 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");
                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, sal_code, par_code, 0, dValTot, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");
                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 51, tax_code, par_code, 0, dVal1Tot, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");

                    if (tax_code2.Length > 0)
                    {
                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 52, tax_code2, par_code, 0, dVal2Tot, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, "VOUCHER");
                    }
                }
                #endregion

                newVnum = "Y";
            }
            #endregion
        }
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
                    
                   
                    DataTable dtn = new DataTable();
                    dtn.Columns.Add("SRNO", typeof(int));
                    dtn.Columns.Add("LEDGRP", typeof(string));
                    dtn.Columns.Add("PARTYCODE", typeof(string));
                    dtn.Columns.Add("ACCT_NAME", typeof(string));
                    dtn.Columns.Add("ALIAS_NAME", typeof(string));
                    dtn.Columns.Add("PAN_NO", typeof(string));
                    dtn.Columns.Add("GST_NO", typeof(string));
                    dtn.Columns.Add("ADDR1", typeof(string));
                    dtn.Columns.Add("ADDR2", typeof(string));
                    dtn.Columns.Add("ADDR3", typeof(string));
                    dtn.Columns.Add("ADDR4", typeof(string));
                    dtn.Columns.Add("STATE_NAME", typeof(string));
                    dtn.Columns.Add("COUNTRY", typeof(string));
                    dtn.Columns.Add("CIN_NO", typeof(string));
                    dtn.Columns.Add("EMAILID", typeof(string));
                    dtn.Columns.Add("CONTACT_PERSON", typeof(string));
                    dtn.Columns.Add("COM_ACNT", typeof(string));
                    dtn.Columns.Add("MOBILE", typeof(string));
                    
                    DataRow drn = null;

                    // for checking data headers , excel file must contain same column
                    if (dt.Columns.Count == dtn.Columns.Count)

                    {
                        for (int j = 0; j < dtn.Columns.Count;j++)
                        {
                           if (dtn.Columns[j].ColumnName.ToString().Trim().ToUpper()!= dt.Columns[j].ColumnName.ToString().Trim().ToUpper())
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
                    string mq1 = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[1].ToString().Trim().Length <= 2)
                        {
                            drn = dtn.NewRow();
                            drn["srno"] = count;
                            drn["ledgRp"] = fgen.padlc(Convert.ToInt32(dr[1].ToString().Trim()), 2);
                            mq1 = fgen.padlc(Convert.ToInt32(dr[1].ToString().Trim()), 2);
                            if (mq1 == "02" || mq1 == "06" || mq1 == "05" || mq1 == "16")
                            {

                                drn["partycode"] = dr[2].ToString().Trim();

                            }
                            else {

                                drn["partycode"] = fgen.padlc(Convert.ToInt32(dr[2].ToString().Trim()), 6);
                            }
                            drn["acct_name"] = dr[3].ToString().Trim().Replace("””", "").Replace("’’", "~");

                            drn["alias_name"] = dr[4].ToString().Trim().Replace("””", "").Replace("’’", "~");
                            drn["pan_no"] = dr[5].ToString().Trim();
                            drn["gst_no"] = dr[6].ToString().Trim();

                            drn["addr1"] = dr[7].ToString().Trim();
                            drn["addr2"] = dr[8].ToString().Trim();
                            drn["addr3"] = dr[9].ToString().Trim();
                            drn["addr4"] = dr[10].ToString().Trim();
                            drn["state_name"] =dr[11].ToString().Trim();

                            drn["country"] = dr[12].ToString().Trim();
                            drn["cin_no"] = dr[13].ToString().Trim();
                            drn["emailid"] = dr[14].ToString().Trim();

                            drn["contact_person"] = dr[15].ToString().Trim();

                            drn["com_acnt"] = dr[16].ToString().Trim();
                            drn["mobile"] = dr[17].ToString().Trim();
                            drn["dtsrno"] = count1;

                            count++;
                            count1++;
                            dtn.Rows.Add(drn);
                        }

                        else {

                            fgen.msg("-", "AMSG", "Please put only 2 digit code in ledger grp");
                            return;
                        }
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
    protected void btnvalidate_ServerClick(object sender, EventArgs e)
    {

        //if (!FileUpload1.HasFile)
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please choose file first in order to validate");
        //    return;
        //}



        DataTable dtn = new DataTable();
        dtn = (DataTable)ViewState["dtn"];
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);
        DataView view = new DataView(dtn);
        DataTable distinctValues = view.ToTable(true, "acct_name");

        //checking duplicate values in dataview
        foreach (DataRow dr1 in distinctValues.Rows)
        {
            DataView view2 = new DataView(dtn, "acct_name='" + dr1["acct_name"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
            dt2 = new DataTable();
            dt2 = view2.ToTable();
            if (dt2.Rows.Count == 1)
            {

            }
            else
            {
                for (int l = 0; l < dt2.Rows.Count; l++)
                {
                    flag = 1;
                    dtn.Rows[Convert.ToInt32(dt2.Rows[l]["dtsrno"].ToString())]["duplicate"] = dt2.Rows[l]["acct_name"].ToString() + " " + "is Duplicate";
                }
            }
        }
        int req = 0, i = 0;
        dt = new DataTable();
        DataRow dr = null;
        string app = "";

        //if (distinctValues.Rows.Count != dtn.Rows.Count)
        //{


        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You cannot insert duplicate items. Please update it(ITEM_NAME)");
        //    i++;
        //    flag = 1;
        //    return;

        //}
        #region checkexistitemname

        string chkname1 = "";
        for (int i1 = 0; i1 < dtn.Rows.Count; i1++)
        {

            chkname1 = fgen.seek_iname(frm_qstr, frm_cocd, "select acode from famst where acode='" + dtn.Rows[i1]["PARTYCODE"].ToString().Trim() + "'", "acode");
            if (chkname1 == "0")
            {
                //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This party name " + chkname1 + "  already exist. Please update it ");
                flag = 1;
                app = "The party code " + chkname1 + " is not found in  database";

                req = req + 1;

            }


            if (dtn.Rows[i1]["com_acnt"].ToString().Length > 1)
            {
                //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Composition Account must be in Y/N format  ");
                flag = 1;


                app += "Composition Account must be Y/N ";

                req = req + 1;
            }

            if (dtn.Rows[i1]["addr1"].ToString().Length > 150)
            {
                //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This addr2 must be in 60 in length ");
                flag = 1;
                app += "This addr1 can be upto 150 characters";

                req = req + 1;
            }

            if (dtn.Rows[i1]["addr2"].ToString().Length > 60)
            {
                //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This addr2 must be in 60 in length ");
                flag = 1;
                app += "Addr2 can be upto 60 characters";

                req = req + 1;
            }
            if (dtn.Rows[i1]["addr3"].ToString().Length > 60 || dtn.Rows[i1]["addr4"].ToString().Length > 60)
            {
                //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This primary  unit  must be in 10 in length and secondary unit  must be in 8 in length ");
                flag = 1;
                app += "The addr3 and addr4 can be upto 60 characters";

                req = req + 1;
            }
            if (dtn.Rows[i1]["contact_person"].ToString().Length > 50)
            {
                //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This critical item input  must be in 2 in length and in (Y/N) format ");
                flag = 1;
                app += "Contact person can be upto 50 characters";

                req = req + 1;
            }
            if (dtn.Rows[i1]["mobile"].ToString().Length > 12)
            {
                //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This gross weight  must be in 9 in length ");
                flag = 1;
                app += "Mobile number can be upto 12 characters ";

                req = req + 1;
            }

            if (dtn.Rows[i1]["cin_no"].ToString().Length > 21)
            {
                //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This net weight  must be in 17 in length  only");
                flag = 1;
                app += "Cinno  can be upto 21 characters ";

                req = req + 1;
            }

            if (dtn.Rows[i1]["ledgRp"].ToString().Trim() == "05" || dtn.Rows[i1]["ledgRp"].ToString().Trim() == "06" || dtn.Rows[i1]["ledgRp"].ToString().Trim() == "16" || dtn.Rows[i1]["ledgrp"].ToString().Trim() == "02")
            {
                if ((dtn.Rows[i1]["pan_no"].ToString().Length < 3) || (dtn.Rows[i1]["pan_no"].ToString() == "-"))
                {
                    //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This net weight  must be in 17 in length  only");
                    flag = 1;
                    app += "Please enter PAN NO. or N/A";

                    req = req + 1;
                }

                if ((dtn.Rows[i1]["gst_no"].ToString().Length < 3) || (dtn.Rows[i1]["gst_no"].ToString() == "-"))
                {
                    //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This net weight  must be in 17 in length only");
                    flag = 1;
                    app += "Please enter GST NO. or N/A";
                    req = req + 1;
                }
            }

            #region validation of pan card



            if ((dtn.Rows[i1]["pan_no"].ToString().Trim().Length > 3))
            {
                if ((dtn.Rows[i1]["pan_no"].ToString().Trim().Length < 10))
                {
                    //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. is a 10 Digit Number");
                    flag = 1;
                    app += "PAN No.must be in 10 characters";

                    req = req + 1;



                }

                if (dtn.Rows[i1]["pan_no"].ToString().Trim().Length == 10)
                {
                    char[] str = dtn.Rows[i1]["pan_no"].ToString().Trim().Substring(0, 5).ToCharArray();

                    for (int i2 = 0; i2 < 5; i2++)
                    {
                        if (str[i2] >= 65 && str[i2] <= 90)
                        {


                        }
                        else
                        {
                            //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. is Not Correct (Digit 1-5 has to be An Alphabet)");
                            flag = 1;
                            app += "Format of PAN should be AAAAANNNNA(A-alpha,N-numeric)";

                            req = req + 1;

                        }
                    }
                    char[] str1 = dtn.Rows[i1]["pan_no"].ToString().Trim().Substring(5, 4).ToCharArray();
                    for (int i3 = 0; i3 < 4; i3++)
                    {
                        if (str1[i3] >= 48 && str1[i3] <= 57)
                        {


                        }
                        else
                        {

                            //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. is Not Correct (Digit 6-9 has to be A Number)");
                            flag = 1;
                            app += "Format of PAN should be AAAAANNNNA(A-alpha,N-numeric)";

                            req = req + 1;
                        }
                    }

                    char[] str2 = dtn.Rows[i1]["pan_no"].ToString().Trim().Substring(9, 1).ToCharArray();
                    for (int i4 = 0; i4 < 1; i4++)
                    {
                        if (str2[i4] >= 65 && str2[i4] <= 90)
                        {


                        }
                        else
                        {

                            //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. is Not Correct (Digit 10 has to be An Alphabet)");
                            flag = 1;
                            app += "Format of PAN should be AAAAANNNNA(A-alpha,N-numeric)";

                            req = req + 1;
                        }
                    }

                }

            }


            #endregion

            #region validation of gst
            if ((dtn.Rows[i1]["gst_no"].ToString().Trim().Length > 3))
            {
                if ((dtn.Rows[i1]["gst_no"].ToString().Trim().Length < 15))
                {
                    //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "GST No. is a 15 Digit Number");
                    flag = 1;
                    app += "GST No.must be in 15 characters";

                    req = req + 1;
                }

                if (dtn.Rows[i1]["gst_no"].ToString().Trim().Length == 15)
                {
                    char[] str = dtn.Rows[i1]["gst_no"].ToString().Trim().Substring(0, 2).ToCharArray();

                    for (int i2 = 0; i2 < 2; i2++)
                    {
                        if (str[i2] >= 48 && str[i2] <= 57)
                        {


                        }
                        else
                        {
                            //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "GST No. is Not Correct (Digit 1-2 has to be Numeric)");
                            flag = 1;
                            app += "GST No. is Not Correct (Digit 1-2 has to be Numeric)";

                            req = req + 1;
                        }
                    }

                    char[] str1 = dtn.Rows[i1]["gst_no"].ToString().Trim().Substring(12, 3).ToCharArray();

                    for (int i2 = 0; i2 > 2 && i2 < 13; i2++)
                    {
                        if (str1[i2] >= 48 || str1[i2] <= 57 || str1[i2] >= 65 || str1[i2] <= 90)
                        {


                        }
                        else
                        {
                            //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "GSTNO. has to Contain Alphabets / Numeric Values only");
                            flag = 1;
                            app += "Invalid GST number";

                            req = req + 1;
                        }
                    }

                }

            }
            #endregion

            #region validation of gst +pan card
            if ((dtn.Rows[i1]["gst_no"].ToString().Trim().Length == 15) && (dtn.Rows[i1]["pan_no"].ToString().Trim().Length == 10))
            {
                if ((dtn.Rows[i1]["gst_no"].ToString().Trim().Substring(2, 10) != dtn.Rows[i1]["pan_no"].ToString()))
                {
                    //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "GST/PANNo. is Not Matching");

                    flag = 1;
                    app += "GST and PAN is Not Matching";

                    req = req + 1;

                }

            }

            #endregion

            if (app != "")
            {
                dtn.Rows[i1]["reasonoffailure"] = app;
                app = "";
            }

        }


        #endregion
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

        if ((req > 0) || (flag == 1))
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This form is not validated successfully .Please download the excel file(See last two columns of excel file.) and while reuploading delete the last two columns ");
            btnexptoexl.Visible = true;
            btnvalidate.Disabled = true;
            return;

        }

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
          //fgen.exp_to_excel(dt1, "ms-excel", "xls", frm_cocd + "_" + DateTime.Now.ToString().Trim());
          //else fgen.msg("-", "AMSG", "No Data to Export");
          // dt1.Dispose();
          Session["send_dt"] = dt1;
      fgen.Fn_open_rptlevel("", frm_qstr);
  }

    public void update_data()
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
                SQuery = "update "+ frm_tabname +" set aname='" + gr1["acct_name"].ToString().Trim() + "',pname='" + gr1["alias_name"].ToString().Trim() + "', girno='" + gr1["pan_no"].ToString().Trim()+"',";
                SQuery += "gst_no='" + gr1["gst_no"].ToString().Trim() + "', addr1='" + gr1["addr1"].ToString().Trim() + "', addr2='" + gr1["addr2"].ToString().Trim() + "',";
                SQuery += "staten='" + gr1["state_name"].ToString().Trim() + "',addr3='" + gr1["addr3"].ToString().Trim() + "', addr4='" + gr1["addr4"].ToString().Trim() + "',";
                SQuery += "country='" + gr1["country"].ToString().Trim() + "',cin_no='" + gr1["cin_no"].ToString().Trim() + "', email='" + gr1["emailid"].ToString().Trim() + "',";
                SQuery += "person='" + gr1["contact_person"].ToString().Trim() + "',gstperson='" + gr1["COM_ACNT"].ToString().Trim() + "', Mobile='" + gr1["Mobile"].ToString().Trim() + "',edt_by='"+frm_uname+"',edt_dt =to_date('" + Convert.ToDateTime(vardate).ToString("dd/MM/yyyy") + "','dd/MM/yyyy')";
                SQuery += "where acode='" + gr1["partycode"].ToString().Trim() +"'";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
           
            }
            fgen.msg("-", "AMSG", "DATA is updated successfully");
            fgen.DisableForm(this.Controls);
            enablectrl();
        }
    
    }

    protected void btnupdate_ServerClick(object sender, EventArgs e)
    {
        update_data();
    }
    protected void btnexport_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "TRCODE";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select Ledger ", frm_qstr);
    }
}