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

public partial class om_multi_empmas : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, nVty = "", typePopup = "N";
    DataTable dt, dt2, dt3, dt4; string mq0;
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
            typePopup = "Y";
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
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnlist.Disabled = false; btnhelp.Disabled = false;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = true; btnvalidate.Disabled = true; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; btnlist.Disabled = true; btnhelp.Disabled = true;
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
        doc_nf.Value = "grade";
        doc_df.Value = "empcode";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "EMPMAS";
        lblheader.Text = "All Employees Upload";
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "ZZ");
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
                SQuery = "select acode,aname as customer,acode as code from famst where trim(Acode) like '2%' order by acode";
                break;
            case "New":
            case "List":
            case "Edit":
            case "Del":
            case "Print":
            case "ExpEx":
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
            // if want to ask popup at the time of new            
            //hffield.Value = "New";
            //make_qry_4_popup();
            //fgen.Fn_open_sseek("-", frm_qstr);

            // else comment upper code

            //frm_vnum = fgen.next_no(frm_qstr, frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");

        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_df.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and GRADE='" + col1 + "'", 6, "vch");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnupload.Focus();
    }
    //-----------------------------------------------------------------------------------
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
        #region checkexistitemname
        #endregion

        #region MAXLENGTH

        //string maxlength = "";
        //foreach (DataRow drn in dtn.Rows)
        //{
        //    //chkname = fgen.seek_iname(frm_qstr, frm_cocd, "select iname from item where iname='" + drn["item_name"].ToString().Trim() + "' ", "iname");

        //    if (drn["A/B/CCLASS"].ToString().Length>5)
        //    {
        //        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This A/B/CCLASS  must be in 5 in length ");
        //        return;
        //    }
        //    if (drn["primary_unit"].ToString().Length > 10 || drn["secondary_unit"].ToString().Length > 8)
        //    {
        //        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This primary  unit  must be in 10 in length and secondary unit  must be in 8 in length ");
        //        return;
        //    }
        //    if (drn["critical_item"].ToString().Length > 2)
        //    {
        //        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This critical item input  must be in 2 in length and in (Y/N) format ");
        //        return;
        //    }
        //    if (drn["gross_wt"].ToString().Length > 9)
        //    {
        //        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This gross weight  must be in 9 in length ");
        //        return;
        //    }

        //    if (drn["net_wt"].ToString().Length > 9)
        //    {
        //        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This net weight  must be in 17 in length  only");
        //        return;
        //    }



        //}

        #endregion

        string crFound = "N";
        #region
        //if (txtAname.Value.ToString().ToUpper().Contains("MARUTI"))
        //{
        //    if (dt2.Rows.Count > 0)
        //    {
        //        if (dt2.Rows[0]["num10"].ToString() == "0" && dt.Rows.Count > 0)
        //        {
        //            dtn = new DataTable();
        //            dtn = (DataTable)ViewState["dtn"];
        //            foreach (DataRow drn in dtn.Rows)
        //            {
        //                if (fgen.make_double(drn["col9"].ToString().Trim()) > 0) crFound = "Y";
        //            }
        //            if (crFound == "Y")
        //            {
        //                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", These Batch is already exist!!'13'Please Upload only Credit Entries");
        //                return;
        //            }
        //        }
        //    }
        //}
        //else if (dt.Rows.Count > 0)
        //{
        //    Session["send_dt"] = dt;
        //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
        //    fgen.Fn_open_rptlevel("These Batch No Already Exist!!'13'Please delete first befor uploading.", frm_qstr);
        //    return;
        //}

        #endregion
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
        FileUpload1.Enabled = false;
        clearctrl();
        enablectrl();
        setColHeadings();

    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Grade", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + DateRange + " and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
        fgen.Fn_FillChart(frm_cocd, frm_qstr, "Testing Chart", "line", "Main Heading", "Sub Heading", SQuery, "");
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
                    //frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_df.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND GRADE='" + frm_vty + "' ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    disablectrl();
                    fgen.EnableForm(this.Controls);
                    // Popup asking for Copy from Older Data
                    //fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
                    //hffield.Value = "NEW_E";
                    #endregion
                    break;
                case "ExpEx":
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);


                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    DataTable dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, "SELECT 'SRNO' AS SRNO, 'GRADE' AS GRADE,'EMPNAME' AS EMPNAME,'GENDER' AS GENDER,'FHNAME' AS FHNAME,'DESIGNATION' AS DESIGNATION,'DEPARTMENT' AS DEPARTMENT,'PHONE' AS PHONE ,'DATE_OF_BIRTH' AS DATE_OF_BIRTH,'WORKING_HOUR' AS  WORKING_HOUR, 'PAN_NUMBER' AS PAN_NUMBER,'ADHAR_NO' AS  ADHAR_NO,'EMAIL_ID' AS EMAIL_ID,'ADDR1' AS ADDR1,'ADDR2' AS ADDR2, 'STATE' AS STATE,'COUNTRY' AS COUNTRY,'PINCODE' AS PINCODE, 'JOINING_DATE'AS JOINING_DATE,'CUTPF(Y/N)' AS CUTPF_Y_N,'CUTESI(Y/N)'AS CUTESI_Y_N,'LEAVING_DATE' AS LEAVING_DATE,'CURRENT_CTC' AS CURRENT_CTC FROM DUAL");
                    DataTable dt2 = new DataTable();
                    //dt2 = fgen.getdata(frm_qstr, frm_cocd, "select er as col ,ename as columns from selmas where grade='" + frm_vty + "' order by grade, morder");

                    //for (int k = 0; k < dt2.Rows.Count; k++)
                    //{
                    //    dt1.Columns.Add("" + dt2.Rows[k]["columns"].ToString().Trim().Replace(".", "*").Replace("-", "ER" + k + "") + "", typeof(string));
                    //    //dt1.Rows.Add("" + dt2.Rows[k]["columns"].ToString().Trim().Replace(".", "*").Replace("-", "ER" + k + "") + "", typeof(double));
                    //    dt1.Rows[0]["" + dt2.Rows[k]["columns"].ToString().Trim().Replace(".", "*").Replace("-", "ER" + k + "") + ""] = dt2.Rows[k]["columns"].ToString().Trim().Replace(".", "*").Replace("-", "ER" + k + "");
                    //}

                    //dt2 = fgen.getdata(frm_qstr, frm_cocd, "select ded as col,dname as columns from selmas where grade='" + frm_vty + "' order by grade, morder");
                    //for (int j = 0; j < dt2.Rows.Count; j++)
                    //{
                    //    dt1.Columns.Add("" + dt2.Rows[j]["columns"].ToString().Trim().Replace(".", "*").Replace("-", "DED" + j + "") + "", typeof(string));
                    //    dt1.Rows[0]["" + dt2.Rows[j]["columns"].ToString().Trim().Replace(".", "*").Replace("-", "DED" + j + "") + ""] = dt2.Rows[j]["columns"].ToString().Trim().Replace(".", "*").Replace("-", "DED" + j + "");
                    //}

                    dt2 = fgen.getdata(frm_qstr, frm_cocd, "select ed_fld as col ,ed_name as columns from wb_selmast where grade='" + frm_vty + "' and type='10' and ed_fld like 'ER%' order by morder");

                    for (int k = 0; k < dt2.Rows.Count; k++)
                    {
                        dt1.Columns.Add("" + dt2.Rows[k]["columns"].ToString().Trim().Replace(".", "*").Replace("-", dt2.Rows[k]["col"].ToString().Trim()) + "", typeof(string));
                        dt1.Rows[0]["" + dt2.Rows[k]["columns"].ToString().Trim().Replace(".", "*").Replace("-", dt2.Rows[k]["col"].ToString().Trim()) + ""] = dt2.Rows[k]["columns"].ToString().Trim().Replace(".", "*").Replace("-", dt2.Rows[k]["col"].ToString().Trim());
                    }

                    dt2 = fgen.getdata(frm_qstr, frm_cocd, "select ed_fld as col ,ed_name as columns from wb_selmast where grade='" + frm_vty + "' and type='10' and ed_fld like 'DED%' order by morder");
                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        dt1.Columns.Add("" + dt2.Rows[j]["columns"].ToString().Trim().Replace(".", "*").Replace("-", dt2.Rows[j]["col"].ToString().Trim()) + "", typeof(string));
                        dt1.Rows[0]["" + dt2.Rows[j]["columns"].ToString().Trim().Replace(".", "*").Replace("-", dt2.Rows[j]["col"].ToString().Trim()) + ""] = dt2.Rows[j]["columns"].ToString().Trim().Replace(".", "*").Replace("-", dt2.Rows[j]["col"].ToString().Trim());
                    }
                    if (dt1.Rows.Count > 0)
                        //fgen.exp_to_excel(dt1, "ms-excel", "xls", frm_cocd + "_" + DateTime.Now.ToString().Trim());
                        //else fgen.msg("-", "AMSG", "No Data to Export");
                        // dt1.Dispose();
                        Session["send_dt"] = dt1;
                    fgen.Fn_open_rptlevel("Download The Excel Format and don't change the columns positions", frm_qstr);
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
                    txtRcode.Value = col1;
                    Text2.Value = col2;
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

            SQuery = "Select distinct * from " + frm_tabname + " where branchcd='" + frm_mbr + "' and trim(grade)='" + frm_vty + "'";
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
                last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and " + doc_df.Value + " " + DateRange + " ", "ldt");
                if (last_entdt == "0")
                { }
                else
                {
                    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                    }
                }
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

                            if (save_it == "Y")
                            {
                                i = 0;
                                do
                                {
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                                    //frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where ICODE='" + doc_df.Value + "'", 8, "vch");

                                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                    if (i > 20)
                                    {
                                        fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
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
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
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
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        dt2 = new DataTable(); dt3 = new DataTable();
        dt2 = fgen.getdata(frm_qstr, frm_cocd, "select  ed_fld as col ,ed_name as columns from wb_selmast where grade='" + frm_vty + "' and type='10' and ed_fld like 'ER%' order by morder");
        dt3 = fgen.getdata(frm_qstr, frm_cocd, "select ed_fld as col ,ed_name as columns from wb_selmast where grade='" + frm_vty + "' and type='10' and ed_fld like 'DED%' order by morder");
        DataTable dtW = (DataTable)ViewState["dtn"];
        if (dtW != null)
        {
            DataView dvW = new DataView(dtW);
            dvW.Sort = "empname";
            dtW = new DataTable();
            dtW = dvW.ToTable();
            foreach (DataRow gr1 in dtW.Rows)
            {
                oporow = oDS.Tables[0].NewRow();
                //string chk_code;

                string empcode = fgen.seek_iname(frm_qstr, frm_cocd, "select lpad(trim(max(empcode)+1),6,'0') as existcd from  EMPMAS where branchcd='" + frm_mbr + "' and substr(empcode,1,2)='" + gr1["grade"].ToString().Trim() + "'", "existcd");
                //chk_code = fgen.seek_iname(frm_qstr, frm_cocd, "select max(icode) as existcd from item where branchcd!='DD' and substr(icode,1,4)='" + gr1["subgp"] + "' and length(Trim(icode))>4  ", "existcd");
                if (empcode == "0")
                {
                    oporow["empcode"] = gr1["grade"].ToString().Trim() + "0001";
                }
                else
                {
                    empcode = fgen.seek_iname(frm_qstr, frm_cocd, "select lpad(trim(max(empcode)+1),6,'0') as existcd from EMPMAS where branchcd='" + frm_mbr + "' and substr(empcode,1,2)='" + gr1["grade"].ToString().Trim() + "'", "existcd");
                    oporow["empcode"] = empcode;
                }
                oporow["BRANCHCD"] = frm_mbr;
                //CHANGED CODE FOR GRID
                oporow["GRADE"] = gr1["grade"].ToString().ToUpper().Trim();
                oporow["name"] = gr1["empname"].ToString().ToUpper().Trim();
                oporow["wlevel3pw"] = "-";
                oporow["pymt_by"] = gr1["PYMT_BY"].ToString().ToUpper().Trim();
                oporow["OLD_EMPC"] = gr1["OLD_EMPC"].ToString().ToUpper().Trim();
                oporow["cardno"] = gr1["cardno"].ToString().ToUpper().Trim();

                //oporow["sex"] = gr1["gender"].ToString().ToUpper().Trim();
                //oporow["fhname"] = gr1["fhname"].ToString().ToUpper().Trim();
                //oporow["desg_text"] = gr1["designation"].ToString().ToUpper().Trim();
                //oporow["deptt"] = gr1["department"].ToString().ToUpper().Trim();
                //oporow["phone"] = gr1["phone"].ToString().ToUpper().Trim();
                //oporow["d_o_b"] = Convert.ToDateTime(gr1["date_of_birth"].ToString().ToUpper().Trim()).ToString("dd/MM/yyyy");
                //oporow["wrkhour"] = gr1["working_hour"].ToString().ToUpper().Trim();
                //oporow["trade"] = gr1["pan_number"].ToString().ToUpper().Trim();
                //oporow["adharno"] = gr1["adhar_no"].ToString().ToUpper().Trim();
                //oporow["email"] = gr1["email_id"].ToString().ToUpper().Trim();
                //oporow["addr1"] = gr1["addr1"].ToString().ToUpper().Trim();
                //oporow["addr2"] = gr1["addr2"].ToString().ToUpper().Trim();
                //oporow["state"] = gr1["state"].ToString().ToUpper().Trim();
                oporow["sex"] = "M";
                oporow["fhname"] = '-';
                oporow["desg_text"] = gr1["desg_text"].ToString().ToUpper().Trim();
                oporow["desg"] = gr1["designation"].ToString().ToUpper().Trim();
                oporow["deptt"] = gr1["desg_text"].ToString().ToUpper().Trim();
                oporow["phone"] = "-";
                oporow["d_o_b"] = Convert.ToDateTime(gr1["date_of_birth"].ToString().ToUpper().Trim()).ToString("dd/MM/yyyy");
                oporow["wrkhour"] = 0;
                oporow["trade"] = "-";
                oporow["adharno"] = "-";
                oporow["email"] = "-";
                oporow["addr1"] = "-";
                oporow["addr2"] = "-";
                oporow["state"] = "-";

                oporow["country"] = gr1["country"].ToString().ToUpper().Trim();
                oporow["pin"] = "-";
                oporow["dtjoin"] = Convert.ToDateTime(gr1["joining_date"].ToString().ToUpper().Trim()).ToString("dd/MM/yyyy");
                oporow["cutvpf"] = "N";
                oporow["esicut"] = "N";
                oporow["leaving_dt"] = "-";
                oporow["curr_ctc"] = fgen.make_double(gr1["current_ctc"].ToString().ToUpper().Trim());
                oporow["erpecode"] = frm_vty + empcode;
                //changed code
                oporow["Section_"] = "Y";
                oporow["scale"] = "-";
                oporow["indst"] = "-";
                oporow["descgrd"] = "-";
                oporow["el"] = 0;
                oporow["cl"] = 0;
                oporow["sl"] = 0;
                oporow["mnthinc"] = 0;
                oporow["insurabl"] = "-";
                oporow["pfno"] = "-";
                oporow["pfnominee"] = "-";
                oporow["fpfno"] = "-";
                oporow["fpfnominee"] = "-";
                oporow["esino"] = "-";
                oporow["esinominee"] = "-";
                oporow["bnkacno"] = gr1["bnkacno"].ToString().ToUpper().Trim();
                oporow["other"] = 0;
                oporow["lta"] = 0;
                oporow["coins"] = 0;
                oporow["generated"] = 0;
                oporow["status"] = "-";
                oporow["mcat"] = "-";
                oporow["city"] = "-";
                oporow["leaving_dt"] = "-";
                oporow["leaving_why"] = "-";
                oporow["esi_disp"] = "-";
                oporow["med"] = 0;
                oporow["bnp"] = 0;
                oporow["vehi"] = 0;
                oporow["reimgen"] = 0;
                oporow["reimtel"] = 0;
                oporow["oth2"] = 0;
                oporow["oth3"] = 0;
                oporow["qualific"] = "-";
                oporow["bloodgrp"] = "-";
                oporow["shift_type"] = "-";
                oporow["shift_code"] = "-";
                oporow["empimg"] = "-";
                oporow["conf_dt"] = "-";
                oporow["app_dt"] = "-";
                oporow["appr_by"] = "-";
                oporow["pymt_by"] = "-";
                oporow["op_coff"] = 0;
                oporow["bon_rate"] = 0;
                // oporow["old_empc"] = "-";
                oporow["cut_wf"] = "N";
                oporow["skillset"] = "-";
                oporow["op_mach"] = "-";
                oporow["join_sal"] = 0;
                oporow["child_cnt"] = 0;
                oporow["emp_type"] = "-";
                oporow["ifsc_code"] = "-";
                oporow["uinno"] = "-";
                oporow["mleave"] = 0;
                oporow["new_pfrule"] = "N";
                oporow["dedgrat"] = "N";
                oporow["dedcant"] = "N";
                oporow["qtr_qpi"] = 0;
                oporow["curr_br"] = "-";
                oporow["adharno"] = "-";
                oporow["sp_relashn"] = "-";
                for (int i = 1; i <= dt2.Rows.Count; i++)
                {
                    //oporow["ER" + i + ""] = fgen.make_double(gr1["" + dt2.Rows[i - 1]["columns"].ToString().Trim().Replace(".", "*").Replace("-", "ER" + Convert.ToInt16(i - 1) + "") + ""].ToString().Trim());
                    oporow["ER" + i + ""] = fgen.make_double(gr1["" + dt2.Rows[i - 1]["columns"].ToString().Trim().Replace(".", "*").Replace("-", dt2.Rows[i - 1]["col"].ToString().Trim() + "") + ""].ToString().Trim());
                }
                for (int i = 1; i <= dt3.Rows.Count; i++)
                {
                    //oporow["DED" + i + ""] = fgen.make_double(gr1["" + dt3.Rows[i - 1]["columns"].ToString().Trim().Replace(".", "*").Replace("-", "DED" + Convert.ToInt16(i - 1) + "") + ""].ToString().Trim());
                    oporow["DED" + i + ""] = fgen.make_double(gr1["" + dt3.Rows[i - 1]["columns"].ToString().Trim().Replace(".", "*").Replace("-", dt3.Rows[i - 1]["col"].ToString().Trim() + "") + ""].ToString().Trim());
                }

                if (edmode.Value == "Y")
                {
                    oporow["ent_by"] = ViewState["entby"].ToString();
                    oporow["eNt_dt"] = ViewState["entdt"].ToString();
                }
                else
                {
                    oporow["eNt_by"] = frm_uname;
                    oporow["eNt_dt"] = vardate;
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
    //------------------------------------------------------------------------------------
    void save_fun3()
    {

    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        SQuery = "select type1 as fstr,name as grade_name,Type1 as Grade_Code from type where id='I' and type1 like '0%' order by grade_code";
        //SQuery = "SELECT 'ED' AS FSTR,'Record Efforts Done' as NAME,'ED' AS CODE FROM dual";
    }
    //------------------------------------------------------------------------------------   
    protected void btnupload_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
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
                dtn.Columns.Add("GRADE", typeof(string));
                //dtn.Columns.Add("EMPCODE", typeof(string));
                dtn.Columns.Add("OLD_EMPC", typeof(string));
                dtn.Columns.Add("EMPNAME", typeof(string));
                // dtn.Columns.Add("GENDER", typeof(string));
                // dtn.Columns.Add("FHNAME", typeof(string));
                dtn.Columns.Add("DESG_TEXT", typeof(string));
                dtn.Columns.Add("DESIGNATION", typeof(string));
                // dtn.Columns.Add("DEPARTMENT", typeof(string));
                // dtn.Columns.Add("PHONE", typeof(string));
                dtn.Columns.Add("DATE_OF_BIRTH", typeof(string));
                dtn.Columns.Add("JOINING_DATE", typeof(string));
                ///dtn.Columns.Add("WORKING_HOUR", typeof(string));
                // /dtn.Columns.Add("PAN_NUMBER", typeof(string));
                ///dtn.Columns.Add("ADHAR_NO", typeof(string));
                ///dtn.Columns.Add("EMAIL_ID", typeof(string));
                /// dtn.Columns.Add("ADDR1", typeof(string));
                /// dtn.Columns.Add("ADDR2", typeof(string));
                /// dtn.Columns.Add("STATE", typeof(string));
                dtn.Columns.Add("COUNTRY", typeof(string));
                /// dtn.Columns.Add("PINCODE", typeof(string));
                // dtn.Columns.Add("CUTPF(Y/N)", typeof(string));
                // dtn.Columns.Add("CUTESI(Y/N)", typeof(string));
                //dtn.Columns.Add("LEAVING_DATE", typeof(string));
                //dtn.Columns.Add("ER1", typeof(string));//downwards all er column ae being made
                dtn.Columns.Add("PYMT_BY", typeof(string));
                dtn.Columns.Add("CURRENT_CTC", typeof(string));
                dtn.Columns.Add("CARDNO", typeof(string));
                dtn.Columns.Add("IFSC_CODE", typeof(string));
                dtn.Columns.Add("BNKACNO", typeof(string));
                //dtn.Columns.Add("ERATE2", typeof(string));
                //dtn.Columns.Add("CONVENYANCE", typeof(string));
                //dtn.Columns.Add("OTHER", typeof(string));

                DataTable dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, "select ed_fld as col ,ed_name as columns from wb_selmast where grade='" + frm_vty + "' and type='10' and ed_fld like 'ER%' order by morder");

                for (int k = 0; k < dt2.Rows.Count; k++)
                {
                    dtn.Columns.Add("" + dt2.Rows[k]["columns"].ToString().Trim().Replace(".", "*").Replace("-", dt2.Rows[k]["col"].ToString().Trim()) + "", typeof(double));
                }

                dt2 = fgen.getdata(frm_qstr, frm_cocd, "select ed_fld as col ,ed_name as columns from wb_selmast where grade='" + frm_vty + "' and type='10' and ed_fld like 'DED%' order by morder");
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    dtn.Columns.Add("" + dt2.Rows[j]["columns"].ToString().Trim().Replace(".", "*").Replace("-", dt2.Rows[j]["col"].ToString().Trim()) + "", typeof(double));
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
                    fgen.msg("-", "AMSG", " Please put exact number of columns as prescribed");
                    return;
                }
                dtn.Columns.Add("Duplicate", typeof(string));
                dtn.Columns.Add("ReasonOfFailure", typeof(string));
                dtn.Columns.Add("dtsrno", typeof(int)); // for development point of view     
                int count = 1, count1 = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr[1].ToString().Trim().Length >= 1)
                    {
                        drn = dtn.NewRow();
                        drn["srno"] = count;
                        drn["grade"] = fgen.padlc(Convert.ToInt32(dr[1].ToString().Trim()), 2);
                        //drn["empcode"] = fgen.padlc(Convert.ToInt32(dr[2].ToString().Trim()),6);
                        drn["OLD_EMPC"] = dr[2].ToString().Trim().Replace("””", "").Replace("’’", "~");
                        drn["empname"] = dr[3].ToString().Trim();
                        drn["desg_text"] = dr[4].ToString().Trim();
                        drn["designation"] = dr[5].ToString().Trim();
                        //drn[""] = dr[5].ToString().Trim();
                        //drn["department"] = dr[6].ToString().Trim();
                        //drn["phone"] = dr[7].ToString().Trim();
                        drn["date_of_birth"] = dr[6].ToString().Trim();
                        drn["joining_date"] = dr[7].ToString().Trim();
                        drn["country"] = dr[8].ToString().Trim();
                        drn["current_ctc"] = dr[9].ToString().Trim();
                        drn["pymt_by"] = dr[10].ToString().Trim();
                        drn["cardno"] = dr[11].ToString().Trim();
                        drn["ifsc_code"] = dr[12].ToString().Trim();
                        drn["bnkacno"] = dr[13].ToString().Trim();
                        //drn["state"] = dr[15].ToString().Trim();
                        //drn["country"] = dr[16].ToString().Trim();
                        //drn["pincode"] = dr[17].ToString().Trim();
                        //drn[""] = dr[18].ToString().Trim();
                        //drn["cutpf(y/n)"] = dr[19].ToString().Trim();
                        //drn["cutesi(y/n)"] = dr[20].ToString().Trim();
                        //drn["leaving_date"] = dr[21].ToString().Trim();
                        //drn["current_ctc"] = dr[22].ToString().Trim();
                        // columns are fixed upto 22.so for dynamic columns loop is start from 23.
                        for (int m = 14; m < dtn.Columns.Count - 3; m++)
                        {
                            drn["" + dtn.Columns[m].ColumnName.ToString().Trim().ToUpper() + ""] = fgen.make_double(dr[m].ToString().Trim());
                        }

                        drn["dtsrno"] = count1;
                        count++;
                        count1++;
                        dtn.Rows.Add(drn);
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Please put grade value in th excel sheet in every row.");
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
    //------------------------------------------------------------------------------------
    protected void btnAcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Supplier ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnRcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TRCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Leadger ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnvalidate_ServerClick(object sender, EventArgs e)
    {
        DataTable dtn = new DataTable();
        dtn = (DataTable)ViewState["dtn"];
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);
        DataView view = new DataView(dtn);
        DataTable distinctValues = view.ToTable(true, "OLD_EMPC");
        //checking duplicate values in dataview
        foreach (DataRow dr1 in distinctValues.Rows)
        {
            DataView view2 = new DataView(dtn, "OLD_EMPC='" + dr1["OLD_EMPC"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
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
                    dtn.Rows[Convert.ToInt32(dt2.Rows[l]["dtsrno"].ToString())]["duplicate"] = dt2.Rows[l]["phone"].ToString() + " " + "is Duplicate";
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
        //SQuery = "select name as grade_name,trim(Type1) as Code from type where id='I' and type1 like '0%' order by code";
        //dt = new DataTable();
        //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        //#region checkexistitemname
        //for (int i1 = 0; i1 < dtn.Rows.Count; i1++)
        //{
        //    if (dtn.Rows[i1]["grade"].ToString().Length < 1)
        //    {
        //        flag = 1;
        //        app += "Grade must be Entered";
        //        req = req + 1;
        //    }
        //    if (dtn.Rows[i1]["grade"].ToString().Length > 2)
        //    {
        //        flag = 1;
        //        app += "Grade must be in 2 in length.Length is exceeding.";
        //        req = req + 1;
        //    }
        //    dt2 = new DataTable();
        //    mq0 = "select branchcd,grade,trim(empcode) as code,name from " + frm_tabname + " where branchcd='" + frm_mbr + "' and grade='" + dtn.Rows[i1]["grade"].ToString().Trim() + "' and name='" + dtn.Rows[i1]["EMPNAME"].ToString().Trim() + "' and fhname='" + dtn.Rows[i1]["FHNAME"].ToString() + "'";
        //    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
        //    if (dt2.Rows.Count > 0)
        //    {
        //        flag = 1;
        //        app += "It seems It is a Duplicate Entry!! Same Details are entered on Emp. Code :  " + dt2.Rows[0]["code"].ToString().Trim() + "";
        //        req = req + 1;
        //        //fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,  " + " It seems it is a Duplicate Entry.'13' Same Details are entered on Emp. Code " + dt.Rows[0]["code"].ToString().Trim());
        //        //return;
        //    }
        //    if (dt.Rows.Count > 0)
        //    {
        //        mq0 = fgen.seek_iname_dt(dt, "code='" + dtn.Rows[i1]["grade"].ToString().Trim() + "'", "code");
        //    }
        //    if (mq0.Length == 1)
        //    {
        //        flag = 1;
        //        app += "This Grade Does not Exist in Grade Master";
        //        req = req + 1;
        //    }
        //    if (dtn.Rows[i1]["gender"].ToString().Length > 1)
        //    {
        //        flag = 1;
        //        app += "This Gender must be Specified in 1 character only in F/M";
        //        req = req + 1;
        //    }
        //    if (dtn.Rows[i1]["phone"].ToString().Length > 12 || dtn.Rows[i1]["pincode"].ToString().Length > 6)
        //    {
        //        flag = 1;
        //        app += "This Phone number must be in 12 in length and Pincode must be in 6 in length.Length is exceeding.";
        //        req = req + 1;
        //    }
        //    if (dtn.Rows[i1]["working_hour"].ToString().Length > 3)
        //    {
        //        flag = 1;
        //        app += "This Working Hour input must be in 3 in length.";
        //        req = req + 1;
        //    }
        //    if (dtn.Rows[i1]["cutesi(y/n)"].ToString().Length > 1)
        //    {
        //        flag = 1;
        //        app += "This CutESI input must be in (Y/N) format";
        //        req = req + 1;
        //    }
        //    if (dtn.Rows[i1]["cutpf(y/n)"].ToString().Length > 1)
        //    {
        //        flag = 1;
        //        app += "This CutPF input  must be in (Y/N) format";
        //        req = req + 1;
        //    }
        //    if (dtn.Rows[i1]["empname"].ToString().Length > 50)
        //    {
        //        flag = 1;
        //        app += "This Employee Name must be in 50 characters only.";
        //        req = req + 1;
        //    }
        //    int dhd = fgen.ChkDate(dtn.Rows[i1]["date_of_birth"].ToString().Trim());
        //    if (dtn.Rows[i1]["date_of_birth"].ToString().Trim().Length != 10 && dhd == 0)
        //    {
        //        flag = 1;
        //        app += "Date of birth should be in DD/MM/YYYY format.";
        //        req = req + 1;
        //    }
        //    #region Validation of Pan card
        //    if ((dtn.Rows[i1]["pan_number"].ToString().Trim().Length > 3))
        //    {
        //        if ((dtn.Rows[i1]["pan_number"].ToString().Trim().Length < 10))
        //        {
        //            //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. is a 10 Digit Number");
        //            flag = 1;
        //            app += "PAN No.must be in 10 characters";
        //            req = req + 1;
        //        }

        //        if (dtn.Rows[i1]["pan_number"].ToString().Trim().Length == 10)
        //        {
        //            char[] str = dtn.Rows[i1]["pan_number"].ToString().Trim().Substring(0, 5).ToCharArray();

        //            for (int i2 = 0; i2 < 5; i2++)
        //            {
        //                if (str[i2] >= 65 && str[i2] <= 90)
        //                {
        //                }
        //                else
        //                {
        //                    //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. is Not Correct (Digit 1-5 has to be An Alphabet)");
        //                    flag = 1;
        //                    app += "Format of PAN should be AAAAANNNNA(A-alpha,N-numeric)";

        //                    req = req + 1;

        //                }
        //            }
        //            char[] str1 = dtn.Rows[i1]["pan_number"].ToString().Trim().Substring(5, 4).ToCharArray();
        //            for (int i3 = 0; i3 < 4; i3++)
        //            {
        //                if (str1[i3] >= 48 && str1[i3] <= 57)
        //                {
        //                }
        //                else
        //                {
        //                    //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. is Not Correct (Digit 6-9 has to be A Number)");
        //                    flag = 1;
        //                    app += "Format of PAN should be AAAAANNNNA(A-alpha,N-numeric)";

        //                    req = req + 1;
        //                }
        //            }

        //            char[] str2 = dtn.Rows[i1]["pan_number"].ToString().Trim().Substring(9, 1).ToCharArray();
        //            for (int i4 = 0; i4 < 1; i4++)
        //            {
        //                if (str2[i4] >= 65 && str2[i4] <= 90)
        //                {
        //                }
        //                else
        //                {
        //                    //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. is Not Correct (Digit 10 has to be An Alphabet)");
        //                    flag = 1;
        //                    app += "Format of PAN should be AAAAANNNNA(A-alpha,N-numeric)";
        //                    req = req + 1;
        //                }
        //            }
        //        }
        //    }

        //    #endregion
        //    if (app != "")
        //    {
        //        dtn.Rows[i1]["reasonoffailure"] = app;
        //        app = "";
        //    }
        //}
        //#endregion

        ViewState["dtn"] = dtn;
        dt = new DataTable();
        DataTable dtn1 = new DataTable();
        dtn1 = (DataTable)ViewState["dtn"];
        dt = dtn1.Copy();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dt, "", "ContentPlaceHolder1_datadiv").ToString(), false);

        if ((req > 0) || (flag == 1))
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This form is not validated successfully .Please download the excel file(See last two columns of excel file.) ");
            if (dtn.Rows.Count > 0)
            {
                dtn.Columns.Remove("dtsrno");
            }
            btnexptoexl.Visible = true;
            btnvalidate.Disabled = true;
            return;
        }
        if (flag == 0)
        {
            btnsave.Disabled = false;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This form is validated successfully");
            btnvalidate.Disabled = true;
            return;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhelp_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "ExpEx";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Grade", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnexptoexl_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
        DataTable dt1 = new DataTable();
        dt1 = (DataTable)ViewState["dtn"];
        if (dt1.Rows.Count > 0)
        {
            Session["send_dt"] = dt1;
            fgen.Fn_open_rptlevel("List of Errors", frm_qstr);
        }
    }
    //------------------------------------------------------------------------------------
}