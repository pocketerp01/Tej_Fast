﻿using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public partial class fUpl : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4;
    DataRow oporow, oporow1, oporow2; DataSet oDS, oDS1, oDS2;
    int i = 0, z = 0;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
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
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");
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
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
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
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "SCRATCH2";

        lblheader.Text = "MARUTI INV UPLOADING";

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
                SQuery = "select acode,aname as customer,acode as code from famst where trim(Acode) like '2%' order by acode";
                break;
            case "DNCN":
                SQuery = "SELECT TYPE1,NAME AS REASON,TYPE1 AS CODE FROM TYPE WHERE ID='$' ORDER BY TYPE1";
                break;
            case "GSTCLASS":
                SQuery = "SELECT TYPE1,NAME AS REASON,TYPE1 AS CODE FROM TYPE WHERE ID='}' ORDER BY TYPE1";
                break;
            case "New":
            case "List":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD" || btnval == "LIST_E")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.col33 as batchno,A.COL35 AS BATCH_DATE,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
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
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            //hffield.Value = "New";
            //make_qry_4_popup();
            //fgen.Fn_open_sseek("-", frm_qstr);

            // else comment upper code

            //frm_vnum = fgen.next_no(frm_qstr, frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
            frm_vty = "ZZ";
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
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
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }
        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only"); txtvchdate.Focus(); return; }

        if (txtacode.Value.Trim().Length < 2)
        { fgen.msg("-", "AMSG", "Please Select Customer Code!!"); txtvchdate.Focus(); return; }

        DataTable dtn = new DataTable();
        dtn = (DataTable)ViewState["dtn"];
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);

        DataView dv = new DataView(dtn);
        dtn = new DataTable();
        dtn = dv.ToTable(true, "BATCHNO");
        dt = new DataTable();
        dt.Columns.Add("ENTRY_NO", typeof(string));
        dt.Columns.Add("ENTRY_DT", typeof(string));
        dt.Columns.Add("BRANCH", typeof(string));
        dt.Columns.Add("Batchno", typeof(string));
        DataRow dr = null;
        foreach (DataRow drn in dtn.Rows)
        {
            dt2 = new DataTable();
            dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT distinct vchnum,vchdate,branchcd,COL33,min(num10) as num10 FROM SCRATCH2 WHERE BRANCHCD='" + frm_mbr + "' and COL33='" + drn["batchno"].ToString().Trim() + "' group by vchnum,vchdate,branchcd,COL33 order by vchnum desc");
            if (dt2.Rows.Count > 0)
            {
                dr = dt.NewRow();
                dr["entry_no"] = dt2.Rows[0]["vchnum"].ToString().Trim();
                dr["entry_dt"] = dt2.Rows[0]["vchdate"].ToString().Trim();
                dr["branch"] = dt2.Rows[0]["branchcd"].ToString().Trim();
                dr["batchno"] = dt2.Rows[0]["col33"].ToString().Trim();
                dt.Rows.Add(dr);
            }
        }
        string crFound = "N";
        //if (txtAname.Value.ToString().ToUpper().Contains("MARUTI"))
        //{
        if (dt2.Rows.Count > 0)
        {
            if (dt2.Rows[0]["num10"].ToString() == "0" && dt.Rows.Count > 0)
            {
                dtn = new DataTable();
                dtn = (DataTable)ViewState["dtn"];
                foreach (DataRow drn in dtn.Rows)
                {
                    if (fgen.make_double(drn["col9"].ToString().Trim()) > 0) crFound = "Y";
                }
                if (crFound == "Y")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", These Batch is already exist!!'13'Please Upload only Credit Entries");
                    return;
                }
            }
        }
        //}
        //else if (dt.Rows.Count > 0)
        //{
        //    Session["send_dt"] = dt;
        //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
        //    fgen.Fn_open_rptlevel("These Batch No Already Exist!!'13'Please delete first befor uploading.", frm_qstr);
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
        //if (txtAname.Value.ToString().ToUpper().Contains("MARUTI"))
        //{
        hfCNote.Value = "N";
        if (fgen.make_double(frm_ulvl) <= 2)
        {
            hffield.Value = "SAVE";
            fgen.msg("-", "CMSG", "Do You want to Make Credit Note too!!'13'(Select No for Debit Note Only)");
        }
        else fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        //}
        //else fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
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
        enablectrl();
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "LIST_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);

        //hffield.Value = "List";
        //fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        //SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + DateRange + " and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
        //fgen.Fn_FillChart(frm_cocd, frm_qstr, "Testing Chart", "line", "Main Heading", "Sub Heading", SQuery, "");
        hffield.Value = "Print_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
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
                /*
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')||trim(a.COL33)='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "'");
                // Deleing data from Sr Ctrl Table
               // BY VIPIN SIR fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "' ");
                // BY VIPIN SIR  fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.acode)='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "' and a.type in ('" + frm_vty + "','58','59') ");
                // BY VIPIN SIR fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivoucher a where trim(a.location)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "' and a.type in ('58','59') AND VCHDATE " + DateRange + "");
                // BY VIPIN SIR fgen.execute_cmd(frm_qstr, frm_cocd, "delete from voucher a where trim(a.GSTVCH_NO)='W" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "' and a.type in ('58','59') AND VCHDATE " + DateRange + "");

                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.acode)='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "' and a.type in ('58','59') ");
                // Deleing data from Ivoucher Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivoucher a where trim(a.location)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "' and a.type in ('58','59') AND VCHDATE " + DateRange + " AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3") + "'");
                // Deleing data from voucher Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from voucher a where trim(a.GSTVCH_NO)='W" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "' and a.type in ('58','59') AND VCHDATE " + DateRange + " AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3") + "'");
                // Saving Deleting History
                */

                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')||trim(a.COL33)='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "'");
                // Deleing data from Sr Ctrl Table                
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.acode)='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "' and a.type in ('" + frm_vty + "','58','59') ");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'  ");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (select DISTINCT a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr from IVOUCHER A WHERE A.BTCHNO='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "')");
                // Deleing data from voucher Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from voucher a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (select DISTINCT a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr from IVOUCHER A WHERE A.BTCHNO='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "')");
                // Deleing data from Ivoucher Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivoucher a where A.BTCHNO='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' AND A.TYPE IN ('58','59') AND A.BRANCHCD='" + frm_mbr + "'");

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
                case "LIST_E":
                    {
                        /*
                        #region BY VIPIN SIR
                        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                        todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                        
                        //corr
                        SQuery = "select DISTINCT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,a.DIFF AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,a.TOTAL AS TOTAL,A.COL29 AS HSCODE,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code from (SELECT distinct a.acode,a.vchdate,a.icode,A.COL33,A.COL34 ,A.COL35 ,A.COL2 ,A.COL1 ,A.COL3 ,A.COL12 ,A.COL22 ,A.COL13 ,A.COL14 ,A.COL16 ,A.COL26 ,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 ,A.COL18 ,A.COL19 ,A.COL20 ,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 FROM SCRATCH2 A  WHERE a.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and a.num10>0 ) a, ivoucher b where TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(b.location)||to_char(b.vchdate,'dd/mm/yyyy') and b.type in ('58','59') order by a.col33";

                        SQuery = "select DISTINCT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,a.DIFF AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,a.TOTAL AS TOTAL,A.COL29 AS HSCODE,a.col46 as vch_no,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS VCH_DT from (SELECT distinct a.acode,a.vchdate,a.icode,A.COL33,A.COL34 ,A.COL35 ,A.COL2 ,A.COL1 ,A.COL3 ,A.COL12 ,A.COL22 ,A.COL13 ,A.COL14 ,A.COL16 ,A.COL26 ,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 ,A.COL18 ,A.COL19 ,A.COL20 ,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 FROM SCRATCH2 A  WHERE a.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and a.num10>0 ) a order by a.col33";
                        SQuery = "select DISTINCT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,a.DIFF AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,a.TOTAL AS TOTAL,A.COL29 AS HSCODE,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code from (SELECT distinct a.acode,a.vchdate,a.icode,A.COL33,A.COL34 ,A.COL35 ,A.COL2 ,A.COL1 ,A.COL3 ,A.COL12 ,A.COL22 ,A.COL13 ,A.COL14 ,A.COL16 ,A.COL26 ,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 ,A.COL18 ,A.COL19 ,A.COL20 ,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 FROM SCRATCH2 A  WHERE a.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and a.num10>0 ) a, ivoucher b where TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||TRIM(A.COL14)=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(b.location)||to_char(b.vchdate,'dd/mm/yyyy')||TRIM(B.IQTY_CHL) and b.type in ('58','59') order by a.col33";

                        SQuery = "SELECT distinct A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,'------' as dr_note,'------' as cr_note,'----------' AS VCH_DT,'--' as vch_type,'--' as b_code,TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||A.COL14||A.COL17||(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16))||A.COL3 as fstr FROM SCRATCH2 A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + DateRange + " and a.num10>0 and a.col33='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "") + "' ORDER BY A.COL33";
                        /*
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
                        hffield.Value = "-";
                        return;
                        
                        DataTable dtList = new DataTable();
                        dtList = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        dt = new DataTable();
                        dt2 = new DataTable();

                        if (dtList.Rows.Count <= 0) return;

                        DataView dv = new DataView(dtList);
                        dt = dv.ToTable(true, "BATCH_NO");
                        col3 = "'-'";
                        foreach (DataRow dr in dt.Rows)
                        {
                            col3 += ",'" + dr["batch_no"] + "'";
                        }

                         //FROM HERE IT IS TAKING DEBIT/CREDIT NOTE NO AND COW
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT distinct branchcd||'~'||type||'~'||trim(vchnum)||'~'||to_char(Vchdate,'dd/mm/yyyy') as Vch,type,branchcd,TRIM(ACODE)||TRIM(ICODE)||TRIM(INVNO)||TO_CHAR(INVDATE,'DD/MM/YYYY')||trim(location)||to_char(vchdate,'dd/mm/yyyy')||iqty_chl||iamount||irate||finvno||trim(genum) fstr,invno FROM IVOUCHER WHERE BRANCHCD!='DD' AND TYPE in ('58','59') AND VCHDATE " + DateRange + " AND STORE='N' and location in (" + col3 + ") ORDER BY vch");

                        if (dt2.Rows.Count <= 0) return;

                        dt = new DataTable();
                        dt.Columns.Add("fstr", typeof(string));
                        dt.Columns.Add("col1", typeof(string));
                        oporow = null;
                        string mhd = "";
                        col1 = "";
                        foreach (DataRow dr in dtList.Rows)
                        {
                            col2 = dr["invno"].ToString().Trim();
                            i = 0;
                            do
                            {
                                if (i > 10) break;
                                mhd = fgen.seek_iname_dt(dt2, "fstr='" + dr["fstr"].ToString().Trim() + "' and vch<>'" + col1 + "'", "vch");
                                col1 = fgen.seek_iname_dt(dt, "col1='" + mhd + "'", "col1");
                                i++;
                            }
                            while (mhd == col1 && col1.Length > 2);

                            {
                                dv = new DataView(dt2, "fstr='" + dr["fstr"].ToString().Trim() + "' and vch<>'" + col1 + "' and invno='" + col2 + "'", "vch", DataViewRowState.CurrentRows);
                                dt4 = new DataTable();
                                dt4 = dv.ToTable(true, "fstr", "vch");
                                foreach (DataRow dr4 in dt4.Rows)
                                {
                                    mhd = dr4["vch"].ToString().Trim();
                                    col1 = fgen.seek_iname_dt(dt, "col1='" + mhd + "'", "col1");
                                    if (col1 != mhd) break;
                                }
                            }

                            {
                                col1 = mhd;
                                if (mhd.Contains("~"))
                                {
                                    dr["b_code"] = mhd.Split('~')[0].ToString().Trim();
                                    dr["vch_type"] = mhd.Split('~')[1].ToString().Trim();
                                    if (mhd.Split('~')[1].ToString().Trim() == "58") dr["cr_note"] = mhd.Split('~')[2].ToString().Trim();
                                    else dr["dr_note"] = mhd.Split('~')[2].ToString().Trim();
                                    dr["vch_dt"] = mhd.Split('~')[3].ToString().Trim();

                                    oporow = dt.NewRow();
                                    oporow["fstr"] = dr["fstr"].ToString().Trim();
                                    oporow["col1"] = col1;
                                    dt.Rows.Add(oporow);
                                }
                            }
                        }
                        #endregion
                        */
                        #region BY MADHVI ON 30 APR 2018
                        SQuery = "SELECT distinct A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,(CASE WHEN B.TYPE='59' THEN B.VCHNUM END) as dr_note,(CASE WHEN B.TYPE='58' THEN B.VCHNUM END) AS CR_NOTE,TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS VCH_DT,B.TYPE as vch_type,B.BRANCHCD as b_code,TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||A.COL14||A.COL17||(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16))||A.COL3||trim(a.reason) as fstr,a.reason FROM SCRATCH2 A,IVOUCHER B WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.reason)||to_char(a.vchdate,'dd/mm/yyyy')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(B.REVIS_NO)||to_char(b.vchdate,'dd/mm/yyyy') AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND trim(a.vchnum)||to_char(A.VCHDATE,'dd/mm/yyyy') ='" + col1 + "' and a.num10>0  AND B.TYPE IN ('58','59') ORDER BY A.REASON";                        
                        if (frm_cocd == "YTEC") SQuery = "SELECT distinct A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,(CASE WHEN B.TYPE='59' THEN b.branchcd||b.type||'-'||trim(B.VCHNUM) END) as dr_note,(CASE WHEN B.TYPE='58' THEN b.branchcd||b.type||'-'||trim(B.VCHNUM) END) AS CR_NOTE,TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS VCH_DT,B.TYPE as vch_type,B.BRANCHCD as b_code,TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||A.COL14||A.COL17||(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16))||A.COL3||trim(a.reason) as fstr,a.reason FROM SCRATCH2 A,IVOUCHER B WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.reason)||trim(a.branchcd)||to_char(a.vchdate,'dd/mm/yyyy')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(B.REVIS_NO)||trim(b.branchcd)||to_char(b.vchdate,'dd/mm/yyyy') AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND trim(a.vchnum)||to_char(A.VCHDATE,'dd/mm/yyyy') ='" + col1 + "' and a.num10>0  AND B.TYPE IN ('58','59') ORDER BY A.REASON";
                        DataTable dtList = new DataTable();
                        dtList = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        #endregion
                        if (dtList.Rows.Count > 0)
                        {
                            dtList.Columns.Remove("fstr");
                            dtList.Columns.Remove("reason");
                        }
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                        Session["send_dt"] = dtList;
                        fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim(), frm_qstr);
                        hffield.Value = "-";
                    }
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
                    string repname = "mUpl";
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";

                    SQuery = "SELECT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,c.aname,c.addr1,c.addr2,c.addr3,C.EMAIL FROM SCRATCH2 A,ivoucher B,famst c WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY') and trim(A.acode)=trim(c.acode) AND A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') and a.num10>0 ORDER BY A.COL33";
                    SQuery = "SELECT distinct A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,b.branchcd||b.type||'-'||trim(B.VCHNUM) as vch_no,TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS VCH_DT,B.TYPE as vch_type,B.BRANCHCD as b_code,TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||A.COL14||A.COL17||(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16))||A.COL3||trim(a.reason) as fstr,a.reason,c.aname,c.addr1,c.addr2,c.addr3,C.EMAIL,B.VCHNUM FROM SCRATCH2 A,IVOUCHER B,FAMST C WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.reason)||trim(a.branchcd)||to_char(a.vchdate,'dd/mm/yyyy')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(B.REVIS_NO)||trim(b.branchcd)||to_char(b.vchdate,'dd/mm/yyyy') AND TRIM(a.ACODE)=TRIM(C.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') and a.num10>0  AND B.TYPE IN ('58','59') ORDER BY B.VCHNUM,A.REASON";
                    //if(frm_cocd == "YTEC") SQuery = "SELECT distinct A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,trim(B.VCHNUM) as vch_no,TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS VCH_DT,B.TYPE as vch_type,B.BRANCHCD as b_code,TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||A.COL14||A.COL17||(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16))||A.COL3||trim(a.reason) as fstr,a.reason,c.aname,c.addr1,c.addr2,c.addr3,C.EMAIL FROM SCRATCH2 A,IVOUCHER B,FAMST C WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.reason)||trim(a.branchcd)||to_char(a.vchdate,'dd/mm/yyyy')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(B.REVIS_NO)||trim(b.branchcd)||to_char(b.vchdate,'dd/mm/yyyy') AND TRIM(a.ACODE)=TRIM(C.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') and a.num10>0  AND B.TYPE IN ('58','59') ORDER BY A.REASON";
                    if (frm_cocd == "YTEC") repname = "mUplYTC";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "mUpl", repname);
                    break;
                case "TACODE":
                    txtacode.Value = col1;
                    txtAname.Value = col2;
                    break;
                case "TRCODE":
                    txtRcode.Value = col1;
                    Text2.Value = col2;
                    break;
                case "DNCN":
                    txtDnCnCode.Value = col1;
                    txtDnCnName.Value = col2;
                    btnGstClass.Focus();
                    break;
                case "GSTCLASS":
                    txtGstClassCode.Value = col1;
                    txtGstClassName.Value = col2;
                    txtGstClassName.Focus();
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
            SQuery = "SELECT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE FROM SCRATCH2 A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + DateRange + " and a.num10>0 ORDER BY A.COL33";
            SQuery = "SELECT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code FROM SCRATCH2 A,ivoucher B WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(b.location)||to_char(b.vchdate,'dd/mm/yyyy') AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and b.type in ('58','59') and a.num10>0 ORDER BY A.COL33";

            //SQuery = "SELECT distinct A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code FROM SCRATCH2 A,ivoucher B WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.col14)||trim(a.col3)||a.branchcd=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(b.location)||to_char(b.vchdate,'dd/mm/yyyy')||b.iqty_chl||trim(b.finvno)||b.branchcd AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND a.vchdate " + PrdRange + " and b.type in ('58','59') and a.num10>0 order by a.col33 ";
            //corr
            SQuery = "select DISTINCT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,a.DIFF AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,a.TOTAL AS TOTAL,A.COL29 AS HSCODE,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code from (SELECT distinct a.acode,a.vchdate,a.icode,A.COL33,A.COL34 ,A.COL35 ,A.COL2 ,A.COL1 ,A.COL3 ,A.COL12 ,A.COL22 ,A.COL13 ,A.COL14 ,A.COL16 ,A.COL26 ,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 ,A.COL18 ,A.COL19 ,A.COL20 ,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 FROM SCRATCH2 A  WHERE a.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and a.num10>0 ) a, ivoucher b where TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(b.location)||to_char(b.vchdate,'dd/mm/yyyy') and b.type in ('58','59') order by a.col33";

            //SQuery = "select DISTINCT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,a.DIFF AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,a.TOTAL AS TOTAL,A.COL29 AS HSCODE,a.col46 as vch_no,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS VCH_DT from (SELECT distinct a.acode,a.vchdate,a.icode,A.COL33,A.COL34 ,A.COL35 ,A.COL2 ,A.COL1 ,A.COL3 ,A.COL12 ,A.COL22 ,A.COL13 ,A.COL14 ,A.COL16 ,A.COL26 ,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 ,A.COL18 ,A.COL19 ,A.COL20 ,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 FROM SCRATCH2 A  WHERE a.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and a.num10>0 ) a order by a.col33";
            SQuery = "select DISTINCT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,a.DIFF AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,a.TOTAL AS TOTAL,A.COL29 AS HSCODE,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code from (SELECT distinct a.acode,a.vchdate,a.icode,A.COL33,A.COL34 ,A.COL35 ,A.COL2 ,A.COL1 ,A.COL3 ,A.COL12 ,A.COL22 ,A.COL13 ,A.COL14 ,A.COL16 ,A.COL26 ,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 ,A.COL18 ,A.COL19 ,A.COL20 ,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 FROM SCRATCH2 A  WHERE a.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and a.num10>0 ) a, ivoucher b where TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||TRIM(A.COL14)=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(b.location)||to_char(b.vchdate,'dd/mm/yyyy')||TRIM(B.IQTY_CHL) and b.type in ('58','59') order by a.col33";

            SQuery = "SELECT distinct A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,'------' as dr_note,'------' as cr_note,'----------' AS VCH_DT,'--' as vch_type,'--' as b_code,TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||A.COL14||A.COL17||(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16))||A.COL3 as fstr FROM SCRATCH2 A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and a.num10>0 ORDER BY A.COL33";


            //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            //fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            //hffield.Value = "-";
            //return;

            DataTable dtList = new DataTable();
            dtList = fgen.getdata(frm_qstr, frm_cocd, SQuery);

            dt = new DataTable();
            dt2 = new DataTable();

            if (dtList.Rows.Count <= 0) return;

            DataView dv = new DataView(dtList);
            dt = dv.ToTable(true, "BATCH_NO");
            col3 = "'-'";
            foreach (DataRow dr in dt.Rows)
            {
                col3 += ",'" + dr["batch_no"] + "'";
            }

            dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT distinct branchcd||'~'||type||'~'||trim(vchnum)||'~'||to_char(Vchdate,'dd/mm/yyyy') as Vch,type,branchcd,TRIM(ACODE)||TRIM(ICODE)||TRIM(INVNO)||TO_CHAR(INVDATE,'DD/MM/YYYY')||trim(location)||to_char(vchdate,'dd/mm/yyyy')||iqty_chl||iamount||irate||finvno fstr FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('58','59') AND VCHDATE " + DateRange + " AND STORE='N' and location in (" + col3 + ") ORDER BY vch");

            dt = new DataTable();
            dt.Columns.Add("fstr", typeof(string));
            dt.Columns.Add("col1", typeof(string));
            oporow = null;
            string mhd = "";
            col1 = "";
            foreach (DataRow dr in dtList.Rows)
            {
                col2 = dr["invno"].ToString().Trim();
                do
                {
                    mhd = fgen.seek_iname_dt(dt2, "fstr='" + dr["fstr"].ToString().Trim() + "' and vch<>'" + col1 + "'", "vch");
                    col1 = fgen.seek_iname_dt(dt, "col1='" + mhd + "'", "col1");
                }
                while (mhd == col1 && col1.Length > 2);

                {
                    col1 = mhd;
                    if (mhd.Contains("~"))
                    {
                        dr["b_code"] = mhd.Split('~')[0].ToString().Trim();
                        dr["vch_type"] = mhd.Split('~')[1].ToString().Trim();
                        if (mhd.Split('~')[1].ToString().Trim() == "58") dr["cr_note"] = mhd.Split('~')[2].ToString().Trim();
                        else dr["dr_note"] = mhd.Split('~')[2].ToString().Trim();
                        dr["vch_dt"] = mhd.Split('~')[3].ToString().Trim();

                        oporow = dt.NewRow();
                        oporow["fstr"] = dr["fstr"].ToString().Trim();
                        oporow["col1"] = col1;
                        dt.Rows.Add(oporow);
                    }
                }
            }
            if (dtList.Rows.Count > 0) dtList.Columns.Remove("fstr");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
            Session["send_dt"] = dtList;
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
            if (col1 == "Y" && Checked_ok == "Y")
            {
                try
                {
                    oDS = new DataSet();
                    oporow = null;
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                    // This is for checking that, is it ready to save the data
                    frm_vnum = "000000";
                    save_fun();
                    //save_fun3();

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
                        save_it = "N";
                        save_it = "Y";

                        if (save_it == "Y")
                        {
                            i = 0;
                            do
                            {
                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
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

                    save_fun();
                    ViewState["refNo"] = frm_vnum;

                    if (edmode.Value == "Y")
                    {
                        cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                    }
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                    save_fun2();

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
                            //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
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
            #endregion
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
            dvW.Sort = "icode,col2";
            dtW = new DataTable();
            dtW = dvW.ToTable();

            foreach (DataRow gr1 in dtW.Rows)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();
                oporow["ICODE"] = gr1["icode"].ToString().Trim();
                oporow["ACODE"] = txtacode.Value.Trim();

                for (int K = 1; K < 36; K++)
                {
                    oporow["COL" + K] = gr1[K].ToString().Trim();
                }

                if (hfCNote.Value == "Y") oporow["NUM10"] = 1;
                else
                {
                    if (fgen.make_double(gr1["col9"].ToString().Trim()) > 0) oporow["NUM10"] = 1;
                    else oporow["NUM10"] = 0;
                }

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
                oporow["reason"] = gr1["dtsrno"].ToString().Trim(); // ADD ON 30 APR 2018 BY MADHVI
                oDS.Tables[0].Rows.Add(oporow);
            }
        }
    }

    void save_fun2()
    {
        string sal_code = "", par_code = "", tax_code = "", tax_code2 = "", schg_code = "", iopr = "", nVty = "";
        string taxCal = "Y";

        if (frm_cocd == "SVPL*") taxCal = "N";

        double dVal = 0; double dVal1 = 0; double dVal2 = 0; double qty = 0;
        DataTable dtSale = new DataTable();
        dtSale = fgen.getdata(frm_qstr, frm_cocd, "SELECT distinct branchcd,TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY') AS FSTR FROM SALE WHERE BRANCHCD!='DD' AND TYPE LIKE '4%' AND VCHDATE BETWEEN TO_DATE('01/04/2016','DD/MM/YYYY') AND TO_DATE('" + frm_CDT2 + "','DD/MM/YYYY') order by fstr ");
        DataTable dtW = (DataTable)ViewState["dtn"];
        if (dtW != null)
        {
            DataView dvW = new DataView(dtW);
            dvW.Sort = "icode,col2";
            dtW = new DataTable();
            dtW = dvW.ToTable();

            int l = 1;
            string mhd = "";
            string saveTo = "Y";
            foreach (DataRow drw in dtW.Rows)
            {
                if (hfCNote.Value == "N")
                {
                    if (fgen.make_double(drw["col9"].ToString().Trim()) < 0) saveTo = "N";
                    else saveTo = "Y";
                }
                if (saveTo == "Y")
                {
                    #region Complete Save Function
                    mhd = fgen.seek_iname_dt(dtSale, "fstr='" + fgen.padlc(Convert.ToInt32(drw["COL4"].ToString().Trim()), 6) + Convert.ToDateTime(drw["COL14"].ToString().Trim()).ToString("dd/MM/yyyy") + "'", "branchcd");
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

                        if (fgen.make_double(drw["col9"].ToString().Trim()) > 0) nVty = "59";
                        else nVty = "58";

                        oporow1["TYPE"] = nVty;

                        i = 0;
                        do
                        {
                            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum)+" + i + " as vch from IVOUCHER where branchcd='" + branchcd + "' and type='" + nVty + "' and VCHDATE " + DateRange + "", 6, "vch");
                            pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + branchcd + nVty + frm_vnum + frm_CDT1, branchcd, nVty, frm_vnum, txtvchdate.Text.Trim(), drw["batchno"].ToString().Trim(), frm_uname);
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
                        string batchNo = drw["batchno"].ToString().Trim();

                        oporow1["LOCATION"] = drw["batchno"].ToString().Trim();

                        oporow1["vchnum"] = frm_vnum;
                        oporow1["vchdate"] = txtvchdate.Text.Trim();

                        oporow1["ACODE"] = txtacode.Value.Trim();
                        oporow1["VCODE"] = txtacode.Value.ToString().Trim();
                        oporow1["ICODE"] = drw["icode"].ToString().Trim();

                        oporow1["REC_ISS"] = "C";

                        oporow1["IQTYIN"] = 0;
                        oporow1["IQTYOUT"] = 0;

                        oporow1["IQTY_CHL"] = drw["col7"].ToString().Trim();
                        qty = fgen.make_double(drw["col7"].ToString().Trim());
                        oporow1["PURPOSE"] = drw["iname"].ToString().Trim();

                        invRmrk = "Supplementary Invoice Against Batch No. :" + drw["Batchno"].ToString().Trim();
                        if (frm_cocd != "PPAP") invRmrk = "Supplementary Invoice Against Batch No. :" + drw["Batchno"].ToString().Trim() + " , Serial No. : " + drw["srno"].ToString().Trim();
                        oporow1["NARATION"] = invRmrk;

                        oporow1["finvno"] = drw["PONO"].ToString().Trim();
                        oporow1["PODATE"] = Convert.ToDateTime(drw["PODT"].ToString().Trim()).ToString("dd/MM/yyyy");

                        oporow1["INVNO"] = fgen.padlc(Convert.ToInt32(drw["COL4"].ToString().Trim()), 6);
                        oporow1["INVDATE"] = Convert.ToDateTime(drw["COL14"].ToString().Trim()).ToString("dd/MM/yyyy");

                        oporow1["revis_no"] = drw["dtsrno"].ToString().Trim(); // ADD ON 30 APR 2018 BY MADHVI
                        oporow1["UNIT"] = "NOS";

                        double Rate = fgen.make_double(drw["col18"].ToString().Trim()) - fgen.make_double(drw["col8"].ToString().Trim());
                        if (Rate < 0) Rate = -1 * Rate;
                        oporow1["IRATE"] = Rate;

                        //OLD RATE + " ~ " + NEW RATE
                        oporow1["PNAME"] = fgen.make_double(drw["col8"].ToString().Trim(), 2) + "~" + fgen.make_double(drw["col18"].ToString().Trim(), 2);

                        dVal = Math.Round(fgen.make_double(drw["col7"].ToString().Trim()) * (fgen.make_double(drw["col18"].ToString().Trim()) - fgen.make_double(drw["col8"].ToString().Trim())), 2);
                        if (dVal < 0) dVal = -1 * dVal;
                        oporow1["IAMOUNT"] = dVal;

                        oporow1["NO_CASES"] = drw["COL21"].ToString().Trim();
                        oporow1["EXC_57F4"] = drw["COL21"].ToString().Trim();

                        if (fgen.make_double(drw["IGST"].ToString().Trim()) > 0)
                        {
                            oporow1["IOPR"] = "IG";
                            iopr = "IG";

                            if (taxCal == "N")
                            {
                                oporow1["EXC_RATE"] = 0;
                                oporow1["EXC_AMT"] = 0;
                            }
                            else
                            {
                                oporow1["EXC_RATE"] = drw["IGST"].ToString().Trim();
                                dVal1 = Math.Round(dVal * (fgen.make_double(drw["IGST"].ToString().Trim()) / 100), 2);
                                oporow1["EXC_AMT"] = Math.Round(dVal1, 2);
                            }
                        }
                        else
                        {
                            iopr = "CG";
                            oporow1["IOPR"] = "CG";

                            if (taxCal == "N")
                            {
                                oporow1["EXC_RATE"] = 0;
                                oporow1["EXC_AMT"] = 0;

                                oporow1["CESS_PERCENT"] = 0;
                                oporow1["CESS_PU"] = 0;
                            }
                            else
                            {
                                oporow1["EXC_RATE"] = drw["CGST"].ToString().Trim();
                                dVal1 = Math.Round(dVal * (fgen.make_double(drw["CGST"].ToString().Trim()) / 100), 2);
                                oporow1["EXC_AMT"] = Math.Round(dVal1, 2);

                                oporow1["CESS_PERCENT"] = drw["SGST"].ToString().Trim();
                                dVal2 = Math.Round(dVal * (fgen.make_double(drw["SGST"].ToString().Trim()) / 100), 2);
                                oporow1["CESS_PU"] = Math.Round(dVal2, 2);
                            }
                        }


                        oporow1["STORE"] = "N";
                        oporow1["MORDER"] = 1;
                        oporow1["SPEXC_RATE"] = dVal;
                        oporow1["SPEXC_AMT"] = dVal + dVal1 + dVal2;

                        oporow1["btchno"] = frm_mbr + ViewState["refNo"].ToString() + txtvchdate.Text.Trim();

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

                        oporow1["RCODE"] = sal_code;

                        oporow1["MATTYPE"] = txtGstClassCode.Value;
                        oporow1["POTYPE"] = txtDnCnCode.Value;

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

                        string vinvno = fgen.padlc(Convert.ToInt32(drw["COL4"].ToString().Trim()), 6);
                        string vinvdt = Convert.ToDateTime(drw["COL14"].ToString().Trim()).ToString("dd/MM/yyyy");

                        #region Voucher Saving
                        batchNo = "W" + batchNo;

                        if (taxCal == "N")
                        {
                            if (nVty == "58")
                            {
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, sal_code, par_code, dVal, 0, vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, par_code, sal_code, 0, fgen.make_double(dVal + dVal1 + dVal2, 2), vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);
                            }
                            else
                            {
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, sal_code, fgen.make_double(dVal + dVal1 + dVal2, 2), 0, vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, sal_code, par_code, 0, dVal, vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);
                            }
                        }
                        else
                        {
                            if (nVty == "58")
                            {
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, sal_code, par_code, dVal, 0, vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);

                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 2, tax_code, par_code, dVal1, 0, vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);
                                if (tax_code2.Length > 0)
                                {
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 3, tax_code2, par_code, dVal2, 0, vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);
                                }
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, par_code, sal_code, 0, fgen.make_double(dVal + dVal1 + dVal2, 2), vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);
                            }
                            else
                            {
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, sal_code, fgen.make_double(dVal + dVal1 + dVal2, 2), 0, vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, sal_code, par_code, 0, dVal, vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 51, tax_code, par_code, 0, dVal1, vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);

                                if (tax_code2.Length > 0)
                                {
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 52, tax_code2, par_code, 0, dVal2, vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);
                                }
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
        if (txtacode.Value.Trim().Length > 2)
        {
            if (FileUpload1.HasFile)
            {
                ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".txt";
                FileUpload1.SaveAs(filesavepath);
                string[] readText = File.ReadAllLines(filesavepath);

                DataTable dtn = new DataTable();
                dtn.Columns.Add("ICODE", typeof(string));
                dtn.Columns.Add("Iname", typeof(string));
                dtn.Columns.Add("CPARTNO", typeof(string));
                dtn.Columns.Add("PONO", typeof(string));
                dtn.Columns.Add("OLDPONO", typeof(string));
                dtn.Columns.Add("PODT", typeof(string));
                dtn.Columns.Add("CGST", typeof(string));
                dtn.Columns.Add("SGST", typeof(string));
                dtn.Columns.Add("IGST", typeof(string));

                string icode = "", iname = "", cpartno = "", HSCODE = "", cgst = "", sgst = "", igst = "", pono = "", oldpono = "", podt = "", batchno = "", srno = "", batchdt = "";
                string sno = "", billno = "", pricdt = "", _57f2 = "", srvno = "", shp = "", acp = "", old_rate = "", bas_amt = "", cgstrate = "", sgstrate = "", igstrate = "", oldrate = "", billdt = "", srvDate = "", cum_shp = "", cum_acp = "", newRate = "", oldBas = "", Flg = ""; long dtsrno = 1;// ADD ON 30 APR 2018 BY MADHVI
                string[] u1 = null;
                string[] u2 = null;

                for (int j = 1; j < 25; j++)
                {
                    dtn.Columns.Add("col" + j, typeof(string));
                }

                dtn.Columns.Add("BATCHNO", typeof(string));
                dtn.Columns.Add("SRNO", typeof(string));
                dtn.Columns.Add("BATCHDT", typeof(string));
                dtn.Columns.Add("dtsrno", typeof(string)); // ADD ON 30 APR 2018 BY MADHVI
                DataRow drn;
                string toRead = "N";
                foreach (string s in readText)
                {
                    if (s.Contains("Total For Item :"))
                    {
                        toRead = "N";
                    }
                    if (toRead == "Y")
                    {
                        if (s.Contains("------------")) { }
                        else
                        {
                            if (1 == 2)
                            {
                                #region valueFill
                                sno = s.Substring(5, 3);
                                billno = s.Substring(8, 22);
                                pricdt = s.Substring(30, 12);
                                _57f2 = s.Substring(42, 17);
                                srvno = s.Substring(59, 11);
                                shp = s.Substring(70, 5);
                                acp = s.Substring(75, 5);
                                old_rate = s.Substring(80, 7);
                                bas_amt = s.Substring(87, 10);
                                cgstrate = s.Substring(97, 8);
                                sgstrate = s.Substring(104, 8);
                                igstrate = s.Substring(114, 8);
                                oldrate = s.Substring(126, 8);
                                billdt = s.Substring(134, 11);
                                srvDate = s.Substring(145, 12);
                                cum_shp = s.Substring(157, 5);
                                cum_acp = s.Substring(163, 6);
                                newRate = s.Substring(169, 10);
                                oldBas = s.Substring(179, 6);
                                Flg = s.Substring(185, 2);
                                #endregion
                            }

                            string[] r1 = s.Split(' ');
                            int v = 0;
                            #region valueFill
                            foreach (string res in r1)
                            {
                                if (res.Length >= 1)
                                {
                                    if (v == 1) sno = res;
                                    if (v == 2) billno = res;
                                    if (v == 3) pricdt = res;
                                    try
                                    {
                                        if (v == 4) _57f2 = fgen.Right(billno, 6);
                                    }
                                    catch
                                    {
                                        fgen.msg("-", "AMSG", "This Invoice No. " + billno + " is not Correct!!");
                                        return;
                                    }
                                    if (v == 5) srvno = res;
                                    if (v == 6) shp = res;
                                    if (v == 7) acp = res;
                                    if (v == 8) old_rate = res;
                                    if (v == 9) bas_amt = res;
                                    if (v == 10) cgstrate = res;
                                    if (v == 11) sgstrate = res;
                                    if (v == 12) igstrate = res;
                                    if (v == 13) oldrate = res;
                                    if (v == 14) billdt = res;
                                    if (v == 15) srvDate = res;
                                    if (v == 16) cum_shp = res;
                                    if (v == 17) cum_acp = res;
                                    if (v == 18) newRate = res;
                                    if (v == 19) oldBas = res;
                                    if (v == 20) Flg = res;
                                    v++;
                                }
                            }
                            v = 0;
                            #endregion

                            if (fgen.make_double(sno) >= 1)
                            {
                                #region adding to table
                                drn = dtn.NewRow();
                                drn["icode"] = icode;
                                drn["iname"] = iname;
                                drn["cpartno"] = cpartno;
                                drn["PONO"] = pono;
                                drn["OLDPONO"] = oldpono;
                                drn["PODT"] = podt;
                                drn["CGST"] = cgst;
                                drn["SGST"] = sgst;
                                drn["IGST"] = igst;
                                drn["COL1"] = sno;
                                drn["COL2"] = billno;
                                drn["COL3"] = pricdt;
                                drn["COL4"] = _57f2;
                                drn["COL5"] = srvno;
                                drn["COL6"] = shp;
                                drn["COL7"] = acp;
                                drn["COL8"] = old_rate;
                                drn["COL9"] = bas_amt;
                                drn["COL10"] = cgstrate;
                                drn["COL11"] = sgstrate;
                                drn["COL12"] = igstrate;
                                drn["COL13"] = oldrate;
                                drn["COL14"] = billdt;
                                drn["COL15"] = srvDate;
                                drn["COL16"] = cum_shp;
                                drn["COL17"] = cum_acp;
                                drn["COL18"] = newRate;
                                drn["COL19"] = oldBas;
                                drn["COL20"] = Flg;
                                drn["COL21"] = HSCODE;
                                drn["BATCHNO"] = batchno;
                                drn["SRNO"] = srno;
                                drn["BATCHDT"] = batchdt;
                                drn["dtsrno"] = fgen.padlc(dtsrno, 6);// ADD ON 30 APR 2018 BY MADHVI
                                dtn.Rows.Add(drn);
                                dtsrno++;// ADD ON 30 APR 2018 BY MADHVI
                                #endregion
                                sno = "";
                            }
                        }
                    }
                    if (s.Contains("BATCH NO  :"))
                    {
                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "BATCH NO  :");
                        u2 = Regex.Split(u1[1], "Buyer ");
                        batchno = u2[0].ToString();
                    }
                    if (s.Contains("BATCH DATE:"))
                    {
                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "BATCH DATE:");
                        batchdt = u1[1].ToString();
                    }
                    if (s.Contains("SERIAL NO :"))
                    {
                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "SERIAL NO :");
                        srno = u1[1].ToString();
                    }
                    if (s.Contains("Part No. :"))
                    {
                        #region filling header
                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "Part No. :");
                        u2 = Regex.Split(u1[1], "PO.No");
                        cpartno = u2[0].ToString();
                        if (cpartno.Trim() == "84681M68P00")
                        {

                        }
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "sELECT TRIM(ICODE) AS ICODE,INAME,CPARTNO,HSCODE FROM item where upper(trim(CPARTNO))='" + cpartno.Trim().ToUpper() + "'");
                        if (dt.Rows.Count > 0)
                        {
                            icode = dt.Rows[0]["icode"].ToString().Trim();
                            iname = dt.Rows[0]["iname"].ToString().Trim();
                            HSCODE = dt.Rows[0]["HSCODE"].ToString().Trim();
                            dt.Dispose();
                        }

                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "PO.No");
                        u2 = Regex.Split(u1[1], "Old");
                        pono = u2[0].ToString();

                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "Old PO.No");
                        u2 = Regex.Split(u1[1], "PO Date:");
                        oldpono = u2[0].ToString();

                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "PO Date:");
                        u2 = Regex.Split(u1[1], "CGST");
                        podt = u2[0].ToString();

                        u1 = null;
                        u1 = Regex.Split(s, "CGST :");
                        cgst = u1[1].Split('%')[0].ToString();

                        u1 = null;
                        u1 = Regex.Split(s, "SGST :");
                        sgst = u1[1].Split('%')[0].ToString();

                        u1 = null;
                        u1 = Regex.Split(s, "IGST :");
                        igst = u1[1].Split('%')[0].ToString();
                        #endregion
                    }
                    if (s.Contains("S.N. BillNo"))
                    {
                        toRead = "Y";
                    }
                }

                ViewState["dtn"] = dtn;
                // ADD ON 01 MAY 2018 BY MADHVI FOR CHECKING WHETHER INVOICES ARE PRESENT IN THE DATABASE OR NOT
                SQuery = "SELECT distinct branchcd,TRIM(VCHNUM) as vchnum,TO_cHAR(VCHDATE,'DD/MM/YYYY') AS vchdate FROM SALE WHERE BRANCHCD!='DD' AND TYPE LIKE '4%' AND VCHDATE BETWEEN TO_DATE('01/04/2016','DD/MM/YYYY') AND TO_DATE('" + frm_CDT2 + "','DD/MM/YYYY') order by vchnum";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("invno", typeof(string));
                dt1.Columns.Add("dt", typeof(string));
                string mq0 = "";
                DataRow dr = null;
                DataView view = new DataView(dtn);
                dt2 = new DataTable();
                dt2 = view.ToTable(true, "col4", "col14");
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    mq0 = fgen.seek_iname_dt(dt, "vchnum='" + fgen.padlc(Convert.ToInt32(dt2.Rows[i]["COL4"].ToString().Trim()), 6) + "' and vchdate='" + Convert.ToDateTime(dt2.Rows[i]["COL14"].ToString().Trim()).ToString("dd/MM/yyyy") + "'", "vchnum");
                    if (mq0 == "0")
                    {
                        dr = dt1.NewRow();
                        dr["invno"] = dt2.Rows[i]["col4"].ToString();
                        dr["dt"] = dt2.Rows[i]["col14"].ToString();
                        dt1.Rows.Add(dr);
                    }
                }
                if (dt1.Rows.Count > 0)
                {
                    Session["send_dt"] = dt1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
                    fgen.Fn_open_rptlevel("These Invoices are not in the Database.", frm_qstr);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall2", "Alert('Total Rows Uploaded " + dtn.Rows.Count + "!!')", false);
                    fgen.msg("-", "AMSG", "Total Rows Uploaded " + dtn.Rows.Count + "!!");
                }
            }
        }
        else
        {
            fgen.msg("-", "AMSG", "Please Select Customer First!!");
        }
    }

    protected void btnAcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Supplier ", frm_qstr);
    }
    protected void btnDNCN_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DNCN";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select D/N C/N Reaosn", frm_qstr);
    }
    protected void btnGstClass_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "GSTCLASS";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select GST Class", frm_qstr);
    }
    protected void btnRcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TRCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Leadger ", frm_qstr);
    }
}