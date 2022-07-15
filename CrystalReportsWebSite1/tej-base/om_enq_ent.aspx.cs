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

public partial class om_enq_ent : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, col7, vardate, fromdt, todt, next_year, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;
    DataTable dtCol = new DataTable();
    DataTable sg1_dt; DataRow sg1_dr;
    string Checked_ok;
    string save_it;
    string html_body = "";
    string Prg_Id, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
    string mq0, mq1, mq2, mq3, mq4, mq5, mq6;
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
        btnlist.Disabled = false; btnprint.Disabled = false; btnacode.Enabled = false; btnitem.Enabled = false;
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
        btnacode.Enabled = true; btnitem.Enabled = true;
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
        doc_nf.Value = "ordno";
        doc_df.Value = "orddt";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "WB_SORFQ";
        lblheader.Text = "Enquiry Entry";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        switch (Prg_Id)
        {
            case "F47310": // FOR Enquiry Entry
                lblheader.Text = "Enquiry Entry";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "ER");
                ecntextbox.Visible = false;
                ecnrmk.Visible = false;
                prior.Visible = false;
                Div2.Visible = false;
                break;

            case "F47313": // FOR ECN Entry
                lblheader.Text = "ECN (Eng. Change Notification)";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "EC");
                enqentbox.Visible = false;
                enqentbox2.Visible = false;
                enqentbox3.Visible = false;
                lbltxtrmk.Text = "Foundry Remarks";
                break;
        }
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
            case "New":
                Type_Sel_query();
                break;

            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "ITEM":
                SQuery = "select trim(icode) as fstr,trim(icode) as item_code,trim(iname) as item_name ,unit,trim(cpartno) as part_no,trim(ciname) as ciname from item where length(trim(icode))>4 and substr(icode,1,2)>='7' order by item_name";
                break;

            case "ACODE":
                SQuery = "select trim(acode) as fstr,acode as Customer_code, trim(aname) as Customer_name, trim(addr1) as address1,trim(addr2) as address2 from famst where substr(trim(acode),1,2)='16' order by Customer_name";
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "SELECT distinct trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr, trim(a.ordno) as RFQ_no,TO_CHAR(a.orddt,'DD/MM/YYYY') as enquiry_dt,a.ACODE AS code,b.aname as party,trim(a.icode) as item_code,c.iname,to_char(a.orddt,'yyyymmdd') as vdd FROM " + frm_tabname + " a,famst b,item c WHERE trim(A.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.type='" + frm_vty + "' ORDER BY VDD DESC,TRIM(a.ORDNO) DESC";
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
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        frm_vty = vty;
        lbl1a.Text = vty;
        string mq0 = "";
        mq0 = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND type='" + frm_vty + "'";
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, mq0, 6, "VCH");
        txtvchnum.Value = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        disablectrl(); btnitem.Enabled = true; btnacode.Focus();
        fgen.EnableForm(this.Controls);
        create_tab();
        for (int i = 0; i < 3; i++)
        {
            sg1_dr = sg1_dt.NewRow();
            if (i == 0)
            {
                sg1_dr["sg1_t1"] = "2D DRAWING";
            }
            if (i == 1)
            {
                sg1_dr["sg1_t1"] = "STANDARDS";
            }
            if (i == 2)
            {
                sg1_dr["sg1_t1"] = "CUSTOMER SPECIFICATION";
            }
            sg1_dr["sg1_t3"] = "-";
            sg1_dr["sg1_t4"] = "-";
            sg1_dt.Rows.Add(sg1_dr);
        }
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        ViewState["sg1"] = sg1_dt;
        // Popup asking for Copy from Older Data
        fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        hffield.Value = "NEW_E";
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " To Edit", frm_qstr);
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
        if (txtacode.Value.Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Customer"); btnacode.Focus(); return;
        }
        if (txticode.Value.Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Item"); btnitem.Focus(); return;
        }
        if (frm_formID == "F47310")
        {
            if (txtsopdate.Text.Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Select SOP Date"); txtsopdate.Focus(); return;
            }
            if (txt_lead_time.Value.Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Select Lead Time Development"); txt_lead_time.Focus(); return;
            }
        }
        else if (frm_formID == "F47313")
        {
            if (txtTrgtDt.Text.Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Select Target Date for Implementation"); txtTrgtDt.Focus(); return;
            }
        }
        if (sg1.Rows.Count < 1)
        {
            fgen.msg("-", "AMSG", "Please Select Atleast One Attachment");
            return;
        }
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            if (((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).SelectedItem.Text.Trim() == "PLEASE SELECT")
            {
                fgen.msg("-", "AMSG", "Please Select Either Yes Or No For " + ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim());
                return;
            }
            if (((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).SelectedItem.Text.Trim() == "YES")
            {
                if (sg1.Rows[i].Cells[5].Text.Trim().Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Add Attachment For " + ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim());
                    return;
                }
            }
            if (((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).SelectedItem.Text.Trim() == "NO")
            {
                if (sg1.Rows[i].Cells[5].Text.Trim().Length > 1)
                {
                    fgen.msg("-", "AMSG", "For " + ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() + " ,Attchment Is Added.'13' But 'No' Is Selected");
                    return;
                }
            }
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " To Delete", frm_qstr);
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
        sg1.DataBind();
        ViewState["sg1"] = null;
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
                mq0 = "select nvl(trim(test),'-') as test from " + frm_tabname + " where  branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "test");
                if (mq1 == "0")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                    // Deleing data from WSr Ctrl Table
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                    // Saving Deleting History
                    fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Type =" + frm_vty + " Deleted");
                    fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                    clearctrl(); fgen.ResetForm(this.Controls);
                }
                else
                {
                    fgen.msg("-", "AMSG", "Either Respond Foundry Or Machine Shop Foundry Is Done.'13' Entry Cannot Be Deleted.");
                    clearctrl(); fgen.ResetForm(this.Controls);
                }
            }
        }
        else if (hffield.Value == "NEW_E")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                hffield.Value = "COPY_OLD";
                make_qry_4_popup();
                fgen.Fn_open_sseek(lblheader.Text + " For Copy", frm_qstr);
            }
            else
            {
                btnacode.Focus();
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

                case "COPY_OLD":
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    switch (Prg_Id)
                    {
                        case "F47310":
                            #region Enquiry Entry
                            SQuery = "select a.*,trim(b.aname) as aname,trim(c.iname) as item_name,c.cpartno as partno from " + frm_tabname + " a, famst b, item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') ='" + col1 + "' order by a.srno";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                i = 0;
                                txtacode.Value = dt.Rows[0]["acode"].ToString().Trim();
                                txtsuppname.Value = dt.Rows[0]["aname"].ToString().Trim();
                                txticode.Value = dt.Rows[0]["icode"].ToString().Trim();
                                txtitmname.Value = dt.Rows[0]["item_name"].ToString().Trim();
                                txtupl_sup.Value = dt.Rows[0]["partno"].ToString().Trim();
                                txtvolpyr.Value = dt.Rows[0]["qtyord"].ToString().Trim();
                                txtsopdate.Text = Convert.ToDateTime(dt.Rows[0]["del_date"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txttarget.Text = dt.Rows[0]["qtysupp"].ToString().Trim();
                                txttrgtwgt.Text = dt.Rows[0]["qtybal"].ToString().Trim();
                                txt_lead_time.Value = dt.Rows[0]["DELV_TERM"].ToString().Trim();
                                txt_cast.Value = dt.Rows[0]["PACKING"].ToString().Trim();
                                txt_ann_bus.Value = dt.Rows[0]["WK1"].ToString().Trim();
                                txtlocn.Value = dt.Rows[0]["TERM"].ToString().Trim();
                                txt_paymt_term.Value = dt.Rows[0]["PAYMENT"].ToString().Trim();
                                txt_req_rtrn.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                                txt_req_sent.Text = dt.Rows[0]["PBASIS"].ToString().Trim();
                                txtrmk.Text = dt.Rows[0]["DESC_"].ToString().Trim();
                                //txtAttch.Text = dt.Rows[0]["atch2"].ToString().Trim();
                                //txtAttchPath.Text = dt.Rows[0]["atch3"].ToString().Trim();
                            }
                            #endregion
                            break;

                        case "F47313":
                            #region ECN
                            SQuery = "select distinct a.*,b.aname,c.iname,c.cpartno from " + frm_tabname + " A,FAMST B,ITEM C WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')='" + col1 + "' order by a.srno";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                i = 0;
                                txtacode.Value = dt.Rows[0]["acode"].ToString().Trim();
                                txticode.Value = dt.Rows[0]["icode"].ToString().Trim();
                                txtitmname.Value = dt.Rows[0]["iname"].ToString().Trim();
                                txtsuppname.Value = dt.Rows[0]["aname"].ToString().Trim();
                                txtupl_sup.Value = dt.Rows[0]["cpartno"].ToString().Trim();
                                txtPriority.Value = dt.Rows[0]["MODE_TPT"].ToString().Trim();
                                txtpymtterm.Value = dt.Rows[0]["payment"].ToString().Trim();
                                txtTrgtDt.Text = Convert.ToDateTime(dt.Rows[0]["DEL_date"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtPrice.Text = dt.Rows[0]["OTHAMT1"].ToString().Trim();
                                txtTrgtWt.Value = dt.Rows[0]["qtybal"].ToString().Trim();
                                txtLdtime.Value = dt.Rows[0]["othac1"].ToString().Trim();
                                txtCompWt.Value = dt.Rows[0]["qtysupp"].ToString().Trim();
                                txtFdyToolCost.Value = dt.Rows[0]["OTCOST1"].ToString().Trim();
                                txtMchToolcost.Value = dt.Rows[0]["IRATE"].ToString().Trim();
                                TxtCastPrice.Value = dt.Rows[0]["OTCOST2"].ToString().Trim();
                                txtToolcost.Value = dt.Rows[0]["OTCOST3"].ToString().Trim();
                                txtShopMchPrice.Value = dt.Rows[0]["qtyord"].ToString().Trim();
                                txtrmk.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                                txtrmk2.Text = dt.Rows[0]["DESC_"].ToString().Trim();
                                txtDrawingRev.Value = dt.Rows[0]["WK1"].ToString().Trim();
                                //txtAttch.Text = dt.Rows[0]["atch2"].ToString().Trim();
                                //txtAttchPath.Text = dt.Rows[0]["atch3"].ToString().Trim();
                            }
                            #endregion
                            break;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        //create_tab();
                        //sg1_dr = null;
                        //for (i = 0; i < dt.Rows.Count; i++)
                        //{
                        //    sg1_dr = sg1_dt.NewRow();
                        //    sg1_dr["sg1_t1"] = dt.Rows[i]["kindattn"].ToString().Trim();
                        //    sg1_dr["sg1_t2"] = dt.Rows[i]["st31no"].ToString().Trim();
                        //    sg1_dr["sg1_t3"] = dt.Rows[i]["atch2"].ToString().Trim();
                        //    sg1_dr["sg1_t4"] = dt.Rows[i]["atch3"].ToString().Trim();
                        //    sg1_dt.Rows.Add(sg1_dr);
                        //}
                        //sg1.DataSource = sg1_dt;
                        //sg1.DataBind();
                        //ViewState["sg1"] = sg1_dt;
                        //foreach (GridViewRow gr in sg1.Rows)
                        //{
                        //    string hf = ((HiddenField)gr.FindControl("cmd1")).Value;
                        //    if (hf != "" && hf != "-")
                        //    {
                        //        ((DropDownList)gr.FindControl("sg1_t2")).Items.FindByText(hf).Selected = true;
                        //    }
                        //}
                        txtTotChild_RF.Value = dt.Rows[0]["PDISC"].ToString().Trim();
                        txtTotChild_MC.Value = dt.Rows[0]["PEXC"].ToString().Trim();
                        disablectrl(); btnitem.Enabled = true; btnacode.Focus();
                        fgen.EnableForm(this.Controls);
                    }
                    break;

                case "ACODE":
                    if (col1 == "") return;
                    SQuery = "select trim(acode) as acode, trim(aname) as aname , trim(addr1)||trim(addr2)||trim(addr3)||trim(addr4) as delv_loc,payment from famst where trim(acode)='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtacode.Value = dt.Rows[0]["acode"].ToString().Trim();
                        txtsuppname.Value = dt.Rows[0]["aname"].ToString().Trim();
                        txtlocn.Value = dt.Rows[0]["delv_loc"].ToString().Trim();
                        txt_paymt_term.Value = dt.Rows[0]["payment"].ToString().Trim() + " - Days";
                        txtpymtterm.Value = dt.Rows[0]["payment"].ToString().Trim();
                    }
                    btnitem.Focus();
                    break;

                case "ITEM":
                    if (col1 == "") return;
                    SQuery = "select trim(icode) as item_code,trim(iname) as item_name ,trim(cpartno) as part_no,wt_net from item where trim(icode) ='" + col1 + "' ";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txticode.Value = dt.Rows[0]["item_code"].ToString().Trim();
                        txtitmname.Value = dt.Rows[0]["item_name"].ToString().Trim();
                        txtupl_sup.Value = dt.Rows[0]["part_no"].ToString().Trim();

                        mq3 = "select count(trim(ibcode)) as totchild from itemosp where trim(icode)='" + col1 + "'";
                        txtTotChild_RF.Value = fgen.seek_iname(frm_qstr, frm_cocd, mq3, "totchild");
                        txtTotChild_MC.Value = fgen.seek_iname(frm_qstr, frm_cocd, mq3, "totchild");
                    }
                    if (frm_formID == "F47310")
                    {
                        txtvolpyr.Focus();
                    }
                    else if (frm_formID == "F47313")
                    {
                        txtPriority.Focus();
                        if (dt.Rows.Count > 0)
                        {
                            txtCompWt.Value = dt.Rows[0]["wt_net"].ToString().Trim();
                        }
                        mq0 = "select orddt,trim(icode) as icode,irate from somas where branchcd='" + frm_mbr + "' and type like '4%' and icode='" + txticode.Value + "' AND orddt>(SYSDATE-500) order by orddt desc";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);//90060001
                        if (dt2.Rows.Count > 0)
                        {
                            txtPrice.Text = dt2.Rows[0]["irate"].ToString().Trim();
                        }
                        mq1 = "select distinct qtysupp,branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,orddt from wb_sorfq where branchcd='" + frm_mbr + "' and type='RF' and icode='" + txticode.Value + "' order by orddt desc";
                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, mq1);
                        if (dt3.Rows.Count > 0)
                        {
                            txtFdyToolCost.Value = dt3.Rows[0]["qtysupp"].ToString().Trim();
                        }
                        mq2 = "select distinct qtyord,branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,orddt from wb_sorfq where branchcd='" + frm_mbr + "' and type='MC' and icode='" + txticode.Value + "' order by orddt desc";
                        dt4 = new DataTable();
                        dt4 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                        if (dt4.Rows.Count > 0)
                        {
                            txtMchToolcost.Value = dt4.Rows[0]["qtyord"].ToString().Trim();
                        }
                    }
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
                    switch (Prg_Id)
                    {
                        case "F47310": // Enquiry Entry
                            #region Enquiry Entry
                            SQuery = "select a.*,trim(b.aname) as aname,trim(c.iname) as item_name,c.cpartno as partno from " + frm_tabname + " a, famst b, item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') ='" + col1 + "' order by a.srno";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                i = 0;
                                ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                                ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                                txtvchnum.Value = dt.Rows[0]["ordno"].ToString().Trim();
                                txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["orddt"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtacode.Value = dt.Rows[0]["acode"].ToString().Trim();
                                txtsuppname.Value = dt.Rows[0]["aname"].ToString().Trim();
                                txticode.Value = dt.Rows[0]["icode"].ToString().Trim();
                                txtitmname.Value = dt.Rows[0]["item_name"].ToString().Trim();
                                txtupl_sup.Value = dt.Rows[0]["partno"].ToString().Trim();
                                //txtdrawing.Value = dt.Rows[0]["DOC_THR"].ToString().Trim();
                                txtvolpyr.Value = dt.Rows[0]["qtyord"].ToString().Trim();
                                txtsopdate.Text = Convert.ToDateTime(dt.Rows[0]["del_date"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txttarget.Text = dt.Rows[0]["qtysupp"].ToString().Trim();
                                txttrgtwgt.Text = dt.Rows[0]["qtybal"].ToString().Trim();
                                txt_lead_time.Value = dt.Rows[0]["DELV_TERM"].ToString().Trim();
                                txt_cast.Value = dt.Rows[0]["PACKING"].ToString().Trim();
                                txt_ann_bus.Value = dt.Rows[0]["WK1"].ToString().Trim();
                                txtlocn.Value = dt.Rows[0]["TERM"].ToString().Trim();
                                txt_paymt_term.Value = dt.Rows[0]["PAYMENT"].ToString().Trim();
                                txt_req_rtrn.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                                txt_req_sent.Text = dt.Rows[0]["PBASIS"].ToString().Trim();
                                txtrmk.Text = dt.Rows[0]["DESC_"].ToString().Trim();
                                //txtAttch.Text = dt.Rows[0]["atch2"].ToString().Trim();
                                //txtAttchPath.Text = dt.Rows[0]["atch3"].ToString().Trim();
                                txtTest.Text = dt.Rows[0]["TEST"].ToString().Trim();
                                txtMC_Flag.Text = dt.Rows[0]["PR_NO"].ToString().Trim();
                                txtTotChild_RF.Value = dt.Rows[0]["PDISC"].ToString().Trim();
                                txtTotChild_MC.Value = dt.Rows[0]["PEXC"].ToString().Trim();
                                fgen.EnableForm(this.Controls);
                                disablectrl();
                                setColHeadings();
                                edmode.Value = "Y";
                                btnacode.Enabled = false;
                                btnitem.Enabled = false;
                            }
                            #endregion
                            break;

                        case "F47313": // ECN
                            #region ECN
                            SQuery = "select distinct a.*,b.aname,c.iname,c.cpartno from " + frm_tabname + " A,FAMST B,ITEM C WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')='" + col1 + "' order by a.srno";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                i = 0;
                                ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                                ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                                txtvchnum.Value = dt.Rows[0]["ordno"].ToString().Trim();
                                txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["orddt"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtacode.Value = dt.Rows[0]["acode"].ToString().Trim();
                                txticode.Value = dt.Rows[0]["icode"].ToString().Trim();
                                txtitmname.Value = dt.Rows[0]["iname"].ToString().Trim();
                                txtsuppname.Value = dt.Rows[0]["aname"].ToString().Trim();
                                txtupl_sup.Value = dt.Rows[0]["cpartno"].ToString().Trim();
                                txtPriority.Value = dt.Rows[0]["MODE_TPT"].ToString().Trim();
                                txtpymtterm.Value = dt.Rows[0]["payment"].ToString().Trim();
                                txtTrgtDt.Text = Convert.ToDateTime(dt.Rows[0]["DEL_date"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtPrice.Text = dt.Rows[0]["OTHAMT1"].ToString().Trim();
                                txtTrgtWt.Value = dt.Rows[0]["qtybal"].ToString().Trim();
                                txtLdtime.Value = dt.Rows[0]["othac1"].ToString().Trim();
                                txtCompWt.Value = dt.Rows[0]["qtysupp"].ToString().Trim();
                                txtFdyToolCost.Value = dt.Rows[0]["OTCOST1"].ToString().Trim();
                                txtMchToolcost.Value = dt.Rows[0]["IRATE"].ToString().Trim();
                                TxtCastPrice.Value = dt.Rows[0]["OTCOST2"].ToString().Trim();
                                txtToolcost.Value = dt.Rows[0]["OTCOST3"].ToString().Trim();
                                txtShopMchPrice.Value = dt.Rows[0]["qtyord"].ToString().Trim();
                                txtrmk.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                                txtrmk2.Text = dt.Rows[0]["DESC_"].ToString().Trim();
                                txtDrawingRev.Value = dt.Rows[0]["WK1"].ToString().Trim();
                                //txtAttch.Text = dt.Rows[0]["atch2"].ToString().Trim();
                                //txtAttchPath.Text = dt.Rows[0]["atch3"].ToString().Trim();
                                txtTest.Text = dt.Rows[0]["TEST"].ToString().Trim();
                                txtMC_Flag.Text = dt.Rows[0]["PR_NO"].ToString().Trim();
                                txtTotChild_RF.Value = dt.Rows[0]["PDISC"].ToString().Trim();
                                txtTotChild_MC.Value = dt.Rows[0]["PEXC"].ToString().Trim();
                                fgen.EnableForm(this.Controls);
                                disablectrl();
                                setColHeadings();
                                edmode.Value = "Y";
                                txtvchdate.Enabled = false;
                                btnitem.Enabled = false;
                                btnacode.Enabled = false;
                            }
                            #endregion
                            break;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["kindattn"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["st31no"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["atch2"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["atch3"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        ViewState["sg1"] = sg1_dt;
                        foreach (GridViewRow gr in sg1.Rows)
                        {
                            string hf = ((HiddenField)gr.FindControl("cmd1")).Value;
                            if (hf != "" && hf != "-")
                            {
                                ((DropDownList)gr.FindControl("sg1_t2")).Items.FindByText(hf).Selected = true;
                            }
                        }
                        dt.Dispose(); sg1_dt.Dispose();
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

                case "SG1_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        i = 0;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = sg1.Rows[i].Cells[5].Text.Trim();
                            sg1_dr["sg1_t4"] = sg1.Rows[i].Cells[6].Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                    }
                    #endregion
                    setColHeadings();
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
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            if (Prg_Id == "F47310")
            {
                SQuery = "select trim(a.ordno) as entry_no , to_char(a.orddt,'dd/mm/yyyy') as entry_date, a.acode as customer_code,trim(b.aname) as Customer_name,a.icode as item_code, trim(c.iname) as item_name,c.cpartno as drg_cpartno,a.qtyord as volume_per_year,to_char(a.del_date,'dd/mm/yyyy') as sop,a.qtysupp as target_price,a.qtybal as target_casting_weight,a.delv_term as lead_time_for_development,a.packing as as_cast_fully_finished,a.wk1 as annual_business,a.term as delivery_location,a.payment as payment_term ,a.pbasis as priority,a.remark as other,a.atch2 as file_name,a.atch3 as filepath,a.kindattn as drawing_type,a.st31no as yes_no,to_char(a.orddt,'yyyymmdd') as vdd from " + frm_tabname + " a , famst b,item c  where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and  a.type= '" + frm_vty + "' and a.orddt " + PrdRange + " ORDER BY VDD DESC,Entry_No DESC,A.SRNO";
            }
            else if (Prg_Id == "F47313")
            {
                SQuery = "select a.ordno as Entry_No,to_char(a.orddt,'dd/mm/yyyy') as Entry_Date,trim(a.acode) as cust_code,b.aname as customer,trim(a.icode) as item_code,c.iname as item_name,c.cpartno,a.mode_tpt as priority,a.payment as Payment_termS_Days,to_char(a.del_date,'dd/mm/yyyy') as target_date,nvl(a.otcost2,0)  as new_Casting_price,nvl(a.qtyord,0) as New_shop_machining_price,nvl(a.qtysupp,0) as existing_comp_wt , a.remark ,a.othac1 as leat_Time,a.othamt1 as current_price ,a.otcost1 as existing_fdy_tool_cost,a.atch2 as file_name,a.atch3 as filepath,a.kindattn as drawing_type,a.st31no as yes_no,to_char(a.orddt,'yyyymmdd') as vdd from " + frm_tabname + " A,FAMST B,ITEM C WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' And a.orddt " + PrdRange + " ORDER BY VDD DESC,Entry_No DESC,A.SRNO";
            }
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            string last_entdt;
            //checks
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and id='" + lbl1a.Text + "'  ", "ldt");
            if (last_entdt == "0" || edmode.Value == "Y")
            {
            }
            else
            {
                if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + ",Please Check !!");
                }
            }
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + " ,Please Check !!");
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
                        if (Prg_Id == "F47310")
                        {
                            save_fun();
                        }
                        else if (Prg_Id == "F47313") { save_fun2(); }

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
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        if (Prg_Id == "F47310")
                        {
                            save_fun();
                        }
                        else if (Prg_Id == "F47313") { save_fun2(); }

                        if (edmode.Value == "Y")
                        {
                            string mycmd = "";
                            mycmd = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/MM/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mycmd);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            mq5 = "update " + frm_tabname + " set test='" + txtTest.Text.Trim() + "' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mq5);
                            mq5 = "update " + frm_tabname + " set pr_no='" + txtMC_Flag.Text.Trim() + "' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mq5);
                        }
                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Value + " Updated Successfully");
                            string mycmd2 = "";
                            mycmd2 = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mycmd2);
                        }
                        else
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
                        #region Mail
                        if (edmode.Value == "")
                        {
                            System.Text.StringBuilder msb = new System.Text.StringBuilder();
                            msb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
                            msb.Append("Dear Sir/Mam,<br/><br/>");
                            string sub = "";

                            //if (edmode.Value == "Y")
                            //{
                            //    sub = "A " + lblheader.Text + " has been Edited.";
                            //    msb.Append("A " + lblheader.Text + " has been <b>Edited</b> and details are as follows:  <br/><br/>");
                            //}
                            //else
                            //{
                            sub = "A New " + lblheader.Text + " has been Entered.";
                            msb.Append("A <b>New</b> " + lblheader.Text + " has been entered and details are as follows:  <br/><br/>");
                            //}
                            msb.Append("<table border=1 cellspacing=2 cellpadding=2 style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; color: #474646'>");
                            //if (edmode.Value == "Y")
                            //{
                            //    msb.Append("<tr style='color: #FFFFFF; background-color: #0099FF; font-weight: 700; font-family: Arial, Helvetica, sans-serif'><td><b>RFQ No.</b></td><td><b>Date</b></td><td><b>Customer</b></td><td><b>Item</b></td><td><b>Ent_By</b></td><td><b>Ent_Dt</b></td><td><b>Edt_By</b></td><td><b>Edt_Dt</b></td></tr>");
                            //}
                            //else
                            //{
                            msb.Append("<tr style='color: #FFFFFF; background-color: #0099FF; font-weight: 700; font-family: Arial, Helvetica, sans-serif'><td><b>RFQ No.</b></td><td><b>Date</b></td><td><b>Customer</b></td><td><b>Item</b></td><td><b>Ent_By</b></td><td><b>Ent_Dt</b></td></tr>");
                            //}
                            msb.Append("<td>");
                            msb.Append(txtvchnum.Value);
                            msb.Append("</td>");
                            msb.Append("<td>");
                            msb.Append(txtvchdate.Text);
                            msb.Append("</td>");
                            msb.Append("<td>");
                            msb.Append(txtsuppname.Value + " (" + txtacode.Value + ")");
                            msb.Append("</td>");
                            msb.Append("<td>");
                            msb.Append(txtitmname.Value + " (" + txticode.Value + ")");
                            msb.Append("</td>");
                            //==========ADD ENT BY AND EDT BY
                            SQuery = "select  DISTINCT ENT_BY,TO_CHAR(ENT_DT,'DD/MM/YYYY') AS ENT_DT,EDT_BY,TO_CHAR(EDT_DT,'DD/MM/YYYY') AS EDT_dT  from WB_SORFQ where branchcd='" + frm_mbr + "'  and type='" + frm_vty + "' and ordno='" + frm_vnum + "' and to_char(orddt,'dd/mm/yyyy')='" + txtvchdate.Text + "'";
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            msb.Append("<td>");
                            msb.Append(dt.Rows[0]["ENT_BY"].ToString().Trim());
                            msb.Append("</td>");
                            msb.Append("<td>");
                            msb.Append(dt.Rows[0]["ENT_dt"].ToString().Trim());
                            msb.Append("</td>");
                            //if (edmode.Value == "Y")
                            //{
                            //    msb.Append("<td>");
                            //    msb.Append(dt.Rows[0]["EdT_BY"].ToString().Trim());
                            //    msb.Append("</td>");
                            //    msb.Append("<td>");
                            //    msb.Append(dt.Rows[0]["EdT_dt"].ToString().Trim());
                            //    msb.Append("</td>");
                            //}
                            msb.Append("</tr>");
                            msb.Append("</table><br/><br/>");
                            msb.Append("<br>===========================================================<br>");
                            msb.Append("<br>This Report is Auto generated from the Tejaxo ERP.");
                            msb.Append("<br>The above details are to be best of information and data available to the ERP system.");
                            msb.Append("<br>Errors or Omissions if any are regretted.");
                            msb.Append("Thanks and Regards,<br/>");
                            msb.Append("" + fgenCO.chk_co(frm_cocd) + "");
                            msb.Append("</body></html>");
                            //=========================================
                            SQuery = "select  branchcd,type,vchnum,vchdate,rcode,ecode as userid,srno from WB_mail_mgr where rcode='" + frm_formID + "' order by srno";
                            dt3 = new DataTable(); dt4 = new DataTable();
                            dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);//MAIL CONFIG
                            SQuery = "select distinct userid,username,erpdeptt,emailid from evas order by userid";
                            dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //EVAS
                            mq2 = "";
                            mq2 = fgen.seek_iname(frm_qstr, frm_cocd, "select emailid  from evas WHERE upper(trim(USERNAME))='" + frm_uname + "'", "emailid");//LOGIN USER ID
                            for (i = 0; i < dt3.Rows.Count; i++)
                            {
                                mq1 = "";
                                mq1 = fgen.seek_iname_dt(dt4, "userid='" + dt3.Rows[i]["userid"].ToString().Trim() + "'", "emailid");
                                fgen.send_mail(frm_cocd, "Tejaxo ERP", mq1, "", "", sub, msb.ToString());
                            }
                            fgen.send_mail(frm_cocd, "Tejaxo ERP", mq2, "", "", sub, msb.ToString());
                        }
                        #endregion
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); sg1.DataSource = null; sg1.DataBind(); ViewState["sg1"] = null;
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
    //------------------------------------------------------------------------------------
    protected void btnlbl11_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl12_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl19_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl20_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl21_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl22_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl23_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["ORDNO"] = frm_vnum.Trim().ToUpper();
            oporow["ORDDT"] = txtvchdate.Text.Trim().ToUpper();
            oporow["acode"] = txtacode.Value.ToUpper();
            oporow["ICODE"] = txticode.Value.ToUpper();
            oporow["QTYORD"] = fgen.make_double(txtvolpyr.Value.ToUpper());
            //oporow["DOC_THR"] = txtdrawing.Value.Trim().ToUpper();
            oporow["DEL_DATE"] = txtsopdate.Text.Trim().ToUpper();
            oporow["QTYSUPP"] = fgen.make_double(txttarget.Text.ToUpper());
            oporow["QTYBAL"] = fgen.make_double(txttrgtwgt.Text.ToUpper());
            oporow["DELV_TERM"] = txt_lead_time.Value.Trim().ToUpper();
            oporow["PACKING"] = txt_cast.Value.Trim().ToUpper();
            oporow["WK1"] = fgen.make_double(txt_ann_bus.Value.Trim().ToUpper());
            oporow["PAYMENT"] = txt_paymt_term.Value.ToUpper();
            oporow["TERM"] = txtlocn.Value.ToUpper();
            oporow["PBASIS"] = txt_req_sent.Text.ToUpper();
            oporow["REMARK"] = txt_req_rtrn.Text.ToUpper();
            oporow["DESC_"] = txtrmk.Text.ToUpper();
            oporow["UNIT"] = "-";
            oporow["OTCOST2"] = "0";
            oporow["PDISC"] = fgen.make_double(txtTotChild_RF.Value.Trim().ToUpper());// CHILD COUNT FOR RESPOND FOUNDRY
            oporow["PEXC"] = fgen.make_double(txtTotChild_MC.Value.Trim().ToUpper());// CHILD COUNT FOR MACHINE FOUNDRY
            oporow["PTAX"] = "0";
            oporow["OTCOST3"] = "0";
            oporow["PSIZE"] = "-";
            oporow["PORDNO"] = "-";
            oporow["PORDDT"] = vardate;
            oporow["INVNO"] = "-";
            oporow["INVDATE"] = vardate;
            oporow["DELIVERY"] = "0";
            oporow["DEL_MTH"] = "0";
            oporow["DEL_WK"] = "0";
            oporow["INST"] = "-";
            oporow["DOC_THR"] = "-";
            oporow["REFDATE"] = vardate;
            oporow["MODE_TPT"] = "-";
            oporow["TR_INSUR"] = "-";
            oporow["DESP_TO"] = "-";
            oporow["FREIGHT"] = "-";
            oporow["BANK"] = "-";
            oporow["STAX"] = "-";
            oporow["EXC"] = "-";
            oporow["IOPR"] = "-";
            oporow["PR_NO"] = "-";
            oporow["AMD_NO"] = "-";
            oporow["DEL_SCH"] = "-";
            oporow["TAX"] = "-";
            oporow["WK2"] = "0";
            oporow["WK3"] = "0";
            oporow["WK4"] = "0";
            oporow["VEND_WT"] = "0";
            oporow["APP_BY"] = "-";
            oporow["APP_DT"] = vardate;
            oporow["ISSUE_NO"] = "0";
            oporow["SRNO"] = i + 1;
            oporow["ATCH2"] = sg1.Rows[i].Cells[5].Text.Trim();
            oporow["ATCH3"] = sg1.Rows[i].Cells[6].Text.Trim();
            oporow["KINDATTN"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
            oporow["ST31NO"] = ((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).SelectedItem.Text.Trim().ToUpper();
            oporow["BILLCODE"] = "-";
            oporow["PREFSOURCE"] = "-";
            //if (txtAttch.Text.Length > 1)
            //{
            //    oporow["atch2"] = txtAttch.Text.Trim();
            //    oporow["atch3"] = txtAttchPath.Text.Trim();
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
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["ORDNO"] = frm_vnum.Trim().ToUpper();
            oporow["orddt"] = fgen.make_def_Date(txtvchdate.Text.Trim(), vardate);
            oporow["ICODE"] = txticode.Value.ToUpper();
            //oporow["unit"] = fgen.seek_iname(frm_qstr, frm_cocd, "select distinct unit from item where trim(icode)='" + txticode.Value.Trim() + "'", "unit");
            oporow["unit"] = "-";
            oporow["ACODE"] = txtacode.Value.Trim().ToUpper();
            oporow["INVNO"] = "-";
            oporow["INVDATE"] = vardate;
            oporow["MODE_TPT"] = txtPriority.Value.Trim().ToUpper();//Priority
            oporow["del_date"] = txtTrgtDt.Text.Trim().ToUpper();//target implement date
            oporow["OTHAMT1"] = fgen.make_double(txtPrice.Text.Trim().ToUpper());//current price
            oporow["qtybal"] = fgen.make_double(txtTrgtWt.Value.Trim().ToUpper()); //TARGET COSTING WEIGHT
            oporow["othac1"] = txtLdtime.Value.Trim().ToUpper();//LEAD TIME
            oporow["qtysupp"] = fgen.make_double(txtCompWt.Value.Trim().ToUpper());//existing component weight
            oporow["OTCOST1"] = fgen.make_double(txtFdyToolCost.Value.Trim().ToUpper()); //existing fdy tool cost
            oporow["IRATE"] = fgen.make_double(txtMchToolcost.Value.Trim().ToUpper());//exisiting m/c tool cost
            oporow["OTCOST2"] = fgen.make_double(TxtCastPrice.Value.Trim().ToUpper()); //new casting price
            oporow["OTCOST3"] = fgen.make_double(txtToolcost.Value.Trim().ToUpper()); //new fdy tooling cost
            oporow["qtyord"] = fgen.make_double(txtShopMchPrice.Value.Trim().ToUpper());//new m/c shop machining price
            oporow["payment"] = txtpymtterm.Value.Trim().ToUpper();
            //not null
            oporow["TR_INSUR"] = "-";
            oporow["PDISC"] = fgen.make_double(txtTotChild_RF.Value.Trim().ToUpper());// CHILD COUNT FOR RESPOND FOUNDRY
            oporow["PEXC"] = fgen.make_double(txtTotChild_MC.Value.Trim().ToUpper());// CHILD COUNT FOR MACHINE FOUNDRY
            oporow["PTAX"] = 0;
            oporow["PSIZE"] = "-";
            oporow["pordno"] = "-";
            oporow["porddt"] = vardate;
            oporow["DELIVERY"] = 0;
            oporow["del_mth"] = 0;
            oporow["del_wk"] = 0;
            oporow["EFFDATE"] = vardate;
            oporow["delv_term"] = "-";
            oporow["term"] = "-";
            oporow["inst"] = "-";
            oporow["refdate"] = vardate;
            oporow["desp_to"] = "-";
            oporow["freight"] = "-";
            oporow["doc_thr"] = "-";
            oporow["packing"] = "-";
            oporow["bank"] = "-";
            oporow["desc_"] = txtrmk2.Text.Trim().ToUpper();
            oporow["stax"] = "-";
            oporow["exc"] = "-";
            oporow["iopr"] = "-";
            oporow["pr_no"] = "-";
            oporow["amd_no"] = "-";
            oporow["del_sch"] = "-";
            oporow["tax"] = "-";
            oporow["wk1"] = fgen.make_double(txtDrawingRev.Value.Trim());
            oporow["wk2"] = 0;
            oporow["wk3"] = 0;
            oporow["wk4"] = 0;
            oporow["vend_wt"] = 0;
            oporow["app_by"] = "-";
            oporow["App_dt"] = vardate;
            oporow["issue_no"] = 0;
            oporow["REMARK"] = txtrmk.Text.Trim().ToUpper();
            //if (txtAttch.Text.Length > 1)
            //{
            //    oporow["atch2"] = txtAttch.Text.Trim();
            //    oporow["atch3"] = txtAttchPath.Text.Trim();
            //}
            oporow["SRNO"] = i + 1;
            oporow["ATCH2"] = sg1.Rows[i].Cells[5].Text.Trim();
            oporow["ATCH3"] = sg1.Rows[i].Cells[6].Text.Trim();
            oporow["KINDATTN"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
            oporow["ST31NO"] = ((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).SelectedItem.Text.Trim().ToUpper();
            oporow["BILLCODE"] = "-";
            oporow["PREFSOURCE"] = "-";
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
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
    }
    //------------------------------------------------------------------------------------
    protected void btnitem_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ITEM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Item", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnacode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnAtt_Click(object sender, EventArgs e)
    {
        string filepath = @"c:\TEJ_ERP\UPLOAD\";   //Server.MapPath("~/tej-base/UPLOAD/");
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            txtAttch.Text = Attch.FileName;
            string fileName = txtvchnum.Value.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
            filepath = filepath + fileName;
            txtAttchPath.Text = filepath;
            txtAttch.Text = Attch.FileName;
            Attch.PostedFile.SaveAs(filepath);
            Attch.PostedFile.SaveAs(Server.MapPath("~/tej-base/Upload/") + fileName);
            lblUpload.Text = filepath;
        }
        else
        {
            lblUpload.Text = "";
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnView1_Click(object sender, ImageClickEventArgs e)
    {
        lblUpload.Text = txtAttchPath.Text;

        string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
    }
    //------------------------------------------------------------------------------------
    protected void btnDwnld1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            lblUpload.Text = txtAttchPath.Text;
            string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));

            Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
            Session["FileName"] = txtAttch.Text;
            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");
        }
        catch { }
    }
    //------------------------------------------------------------------------------------
    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        // Hidden Field
        sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t6", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            sg1.Columns[0].HeaderStyle.Width = 50;
            sg1.Columns[1].HeaderStyle.Width = 100;
            sg1.Columns[2].HeaderStyle.Width = 50;
            sg1.Columns[3].HeaderStyle.Width = 200;
            sg1.Columns[4].HeaderStyle.Width = 200;
            sg1.Columns[5].HeaderStyle.Width = 200;
            sg1.Columns[6].HeaderStyle.Width = 300;
            sg1.Columns[7].HeaderStyle.Width = 200;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = 0;
        if (var == "SG1_UPLD")
        {
            rowIndex = ((GridViewRow)((Button)e.CommandSource).NamingContainer).RowIndex;
        }
        else
        {
            rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        }
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);
        string filePath = "";
        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG1_RMV":
                filePath = sg1.Rows[index].Cells[6].Text.ToUpper();
                if (filePath.Length > 1)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    string secFilePath = Server.MapPath("~/tej-base/") + sg1.Rows[index].Cells[6].Text.Substring(sg1.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"), sg1.Rows[index].Cells[6].Text.ToUpper().Length - sg1.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"));
                    if (File.Exists(secFilePath))
                    {
                        File.Delete(secFilePath);
                    }
                }
                sg1.Rows[index].Cells[5].Text = "-"; ;
                sg1.Rows[index].Cells[6].Text = "-";
                break;

            case "SG1_DWN":
                filePath = sg1.Rows[index].Cells[6].Text.ToUpper();
                if (filePath.Length > 1)
                {
                    Response.ContentType = ContentType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                    Response.WriteFile(filePath);
                    Response.End();
                }
                break;

            case "SG1_VIEW":
                if (sg1.Rows[index].Cells[6].Text.Trim().Length > 1)
                {
                    filePath = sg1.Rows[index].Cells[6].Text.Substring(sg1.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"), sg1.Rows[index].Cells[6].Text.ToUpper().Length - sg1.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
                }
                break;

            case "SG1_UPLD":
                string UploadedFile = ((FileUpload)sg1.Rows[index].FindControl("FileUpload1")).FileName;
                string filepath = @"c:\TEJ_ERP\UPLOAD\";
                string fileName = txtvchnum.Value.Trim() + fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY") + frm_CDT1.Replace(@"/", "_") + "~" + UploadedFile.Replace("&", "").Replace("%", "_");
                filepath = filepath + fileName;
                ((FileUpload)sg1.Rows[index].FindControl("FileUpload1")).PostedFile.SaveAs(filepath);
                ((FileUpload)sg1.Rows[index].FindControl("FileUpload1")).PostedFile.SaveAs(Server.MapPath("~/tej-base/Upload/") + fileName);
                sg1.Rows[index].Cells[5].Text = UploadedFile;
                sg1.Rows[index].Cells[6].Text = filepath;
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnUpload_Click(object sender, EventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
}