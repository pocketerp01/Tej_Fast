﻿using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Drawing;
using System.Collections;

public partial class om_BT_BD_BF : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", xStartDt = "", Enable = "", mq0, mq1;
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
        if (sg1.Rows.Count <= 0) return;
        for (int sR = 0; sR < sg1.Columns.Count; sR++)
        {
            string orig_name;
            double tb_Colm;
            tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
            orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();

            for (int K = 0; K < sg1.Rows.Count; K++)
            {
                #region hide hidden columns
                for (int i = 0; i < 10; i++)
                {
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[K].Cells[i].CssClass = "hidden";
                }
                #endregion
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));
            }
            orig_name = orig_name.ToUpper();
            if (sR == tb_Colm)
            {
                // hidding column
                if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
                {
                    sg1.Columns[sR].Visible = false;
                }
                // Setting Heading Name
                sg1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
                // Setting Col Width
                string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
                if (fgen.make_double(mcol_width) > 0)
                {
                    sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
                    sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                }
            }
        }
        // to hide and show to tab panel
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnprint.Disabled = false;
        create_tab();
        create_tab2();
        create_tab3();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();
        btnlbl4.Enabled = false;
        btnlbl7.Enabled = false;
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; btnprint.Disabled = true;
        btnlbl4.Enabled = true;
        btnlbl7.Enabled = true;
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
        frm_tabname = "inspvch";
        lblheader.Text = "Dim. And Visual Exam. Rep. For BT/BD/BF";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "84");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
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

            case "WO":
                SQuery = "Select Distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.org_invno)||trim(a.work_ordno)||trim(a.icode)||trim(a.cdrgno) as fstr,b.aname as Customer,a.Pordno,a.org_invno as WO_NO,a.acode,a.work_ordno as project,a.icode,i.iname,a.cdrgno as so_line_no,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.ordno,to_char(a.orddt,'yyyymmdd') as vdd from Somas a,famst b,item i where trim(a.acodE)=trim(b.acodE) and trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='4' and length(trim(nvl(a.app_by,'-')))> 1 and length(trim(nvl(a.org_invno,'-')))> 1 order by vdd desc,a.ordno desc";
                SQuery = "Select Distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.org_invno)||trim(a.work_ordno)||trim(a.icode)||trim(a.cdrgno) as fstr,b.aname as Customer,a.Pordno,a.org_invno as WO_NO,a.acode,a.work_ordno as project,a.icode,i.iname,a.cdrgno as so_line_no,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.ordno,to_char(a.orddt,'yyyymmdd') as vdd from Somas a,famst b,item i where trim(a.acodE)=trim(b.acodE) and trim(a.icode)=trim(i.icode) and a.branchcd!='DD' and substr(a.type,1,1)='4' and a.type!='44' and length(trim(nvl(a.app_by,'-')))> 1 and length(trim(nvl(a.org_invno,'-')))> 1 order by vdd desc,a.ordno desc";
                break;

            case "TAG":
                SQuery = "Select trim(icode)||trim(invno)||trim(desc_) as fstr,invno as Wo_No,desc_ as Tag_no,IQTYIN AS Qty,to_char(vchdate,'dd/mm/yyyy') as Entry_Date,Ent_by,icode,finvno from ivoucher where branchcd='" + frm_mbr + "' and type='15' and invno='" + txtlbl4.Text.Trim() + "' and icode='" + txtIcode.Text + "' and upper(finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' order by desc_";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + ((TextBox)gr.FindControl("sg1_t2")).Text.Trim() + "'";
                    else col1 = "'" + ((TextBox)gr.FindControl("sg1_t2")).Text.Trim() + "'";
                }
                if (col1.Length <= 0) col1 = "'-'";
                //SQuery = "Select trim(icode)||trim(invno)||REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') as fstr,invno as Wo_No,desc_ as Tag_no,IQTYIN AS Qty,to_char(vchdate,'dd/mm/yyyy') as Entry_Date,Ent_by,icode,finvno from ivoucher where branchcd='" + frm_mbr + "' and type='15' and invno='" + txtlbl4.Text.Trim() + "' and icode='" + txtIcode.Text + "' and upper(finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') NOT IN (" + col1 + ") order by desc_";
                //SQuery = "Select trim(icode)||trim(invno)||REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') as fstr,invno as Wo_No,desc_ as Tag_no,IQTYIN AS Qty,to_char(vchdate,'dd/mm/yyyy') as Entry_Date,Ent_by,icode,finvno from ivoucherp where branchcd='" + frm_mbr + "' and type='15' and invno='" + txtlbl4.Text.Trim() + "' and upper(finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') NOT IN (" + col1 + ") order by desc_";
                SQuery = "Select trim(icode)||trim(invno)||REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') as fstr,invno as Wo_No,desc_ as Tag_no,IQTYIN AS Qty,to_char(vchdate,'dd/mm/yyyy') as Entry_Date,Ent_by,icode,finvno from ivoucherp where /*branchcd='" + frm_mbr + "' and*/ type='15' and invno='" + txtlbl4.Text.Trim() + "' AND REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') NOT IN (" + col1 + ") order by desc_";
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                //pop3
                // to avoid repeat of item
                col1 = "";
                if (btnval != "SG3_ROW_ADD" && btnval != "SG3_ROW_ADD_E")
                {
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[13].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                    }
                }
                if (col1.Length <= 0) col1 = "'-'";
                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(Trim(icode))>4 and icode like '9%' and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname  ";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "Print_E":
                SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.cpartno as wo_no,a.acode as code,f.aname as customer,a.type,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst f WHERE trim(a.acode)=trim(f.acode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' AND a.VCHDATE  " + DateRange + " ORDER BY vdd desc,entry_no DESC";
                break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.cpartno as wo_no,a.acode as code,f.aname as customer,a.type,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst f WHERE trim(a.acode)=trim(f.acode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' AND a.VCHDATE  " + DateRange + " ORDER BY vdd desc,entry_no DESC";
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
            set_Val();
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
            FillDropDown();
            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
            // else comment upper code

            //frm_vnum = fgen.next_no(frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
            //txtvchnum.Text = frm_vnum;
            //txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            //disablectrl();
            //fgen.EnableForm(this.Controls);
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
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' and vchdate " + DateRange + " AND TYPE='" + frm_vty + "' ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();
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
            fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus();
            return;
        }
        if (txtlbl4.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill " + lbl4.Text);
            txtlbl4.Focus();
            return;
        }
        if (txtlbl4a.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill PO Line Item No.");
            txtlbl4a.Focus();
            return;
        }
        if (txtlbl7.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Customer");
            txtlbl7.Focus();
            return;
        }
        if (txtWoLine.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill WO Line Item No.");
            txtWoLine.Focus();
            return;
        }
        if (txtDrawing.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Drawing No & Rev No.");
            txtDrawing.Focus();
            return;
        }
        if (txtMaterial.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Material / Stock Code");
            txtDrawing.Focus();
            return;
        }
        if (txtlbl2.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill PO No.");
            txtlbl2.Focus();
            return;
        }
        if (txtProject.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Project");
            txtProject.Focus();
            return;
        }
        if (dd_Desg.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Fill Design Std");
            dd_Desg.Focus();
            return;
        }
        if (txtFlange.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Flange Std");
            txtFlange.Focus();
            return;
        }
        if (txtItem.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Select Product");
            txtItem.Focus();
            return;
        }

        if (txtModel.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Valve Model");
            txtModel.Focus();
            return;
        }
        if (txtRating.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Valve Rating");
            txtRating.Focus();
            return;
        }
        if (dd_Dimension.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Above Dimensions Of The Valves Checked And Found Satisfactory '13' (Tab 2)");
            dd_Dimension.Focus();
            return;
        }

        if (dd_Tested.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Visual Inspection Done And Found Satisfactory '13' (Tab 2)");
            dd_Tested.Focus();
            return;
        }

        if (dd_Visual.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select The Above Tested Products Are In Compliance With Customer Statutory And Regulatory Requirements '13' (Tab 2)");
            dd_Visual.Focus();
            return;
        }
        if (sg1.Rows.Count <= 3)
        {
            fgen.msg("-", "AMSG", "Please Select Tag");
            return;
        }
        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            if (i > 1)
            {
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim() == "-")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Tag No. At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
                    return;
                }
            }
            //if (((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() == "-")
            //{
            //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Client Tag No. At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
            //    return;
            //}
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

        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();

        ViewState["sg1"] = null;
        setColHeadings();
        BlankDropDown();
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
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
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
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(6, 10), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                //fgen.save_info(frm_qstr,frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0,6),vardate, frm_uname, frm_vty, lblheader.Text.Trim() + "Delete");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6) + "");
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

                case "COPY_OLD":
                    #region Copy from Old Temp
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,b.text from " + frm_tabname + " a left outer join FIN_MSYS b on trim(a.frm_name)=trim(b.id) where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl4.Text = dt.Rows[i]["frm_name"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["text"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[i]["frm_header"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["ent_id"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["text"].ToString().Trim();
                            sg1_dr["sg1_h3"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";
                            sg1_dr["sg1_f1"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["text"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_t1"] = dt.Rows[i]["OBJ_NAME"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["OBJ_CAPTION"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["OBJ_WIDTH"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["OBJ_VISIBLE"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["col_no"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["obj_maxlen"].ToString().Trim();
                            sg1_dr["sg1_t7"] = "";
                            if (frm_tabname.ToUpper() == "SYS_CONFIG")
                            {
                                sg1_dr["sg1_t7"] = dt.Rows[i]["OBJ_READONLY"].ToString().Trim();
                            }
                            sg1_dr["sg1_t8"] = "";
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1_add_blankrows();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                    }
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
                    FillDropDown();
                    SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt,f.aname from " + frm_tabname + " a,famst f where trim(a.acode)=trim(f.acode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl2.Text = dt.Rows[0]["TITLE"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["ACODE"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["ANAME"].ToString().Trim();
                        txtWoLine.Text = dt.Rows[0]["BTCHNO"].ToString().Trim();
                        txtwoLno.Text = dt.Rows[0]["col5"].ToString().Trim();
                        txtIcode.Text = dt.Rows[0]["ICODE"].ToString().Trim();
                        txtlbl4.Text = dt.Rows[0]["CPARTNO"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["COL1"].ToString().Trim();
                        txtOrder.Text = dt.Rows[0]["GRADE"].ToString().Trim();
                        dd_Desg.SelectedItem.Text = dt.Rows[0]["COL3"].ToString().Trim();
                        txtFlange.Text = dt.Rows[0]["MATL"].ToString().Trim();
                        txtDrawing.Text = dt.Rows[0]["COL4"].ToString().Trim();
                        txtRating.Text = dt.Rows[0]["WONO"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["COL1"].ToString().Trim();
                        txtProject.Text = dt.Rows[0]["COL6"].ToString().Trim();
                        txtModel.Text = dt.Rows[0]["COL2"].ToString().Trim();
                        txtItem.Text = dt.Rows[0]["OBSV20"].ToString().Trim();
                        dd_Dimension.SelectedItem.Text = dt.Rows[0]["OBSV25"].ToString().Trim();
                        dd_Tested.SelectedItem.Text = dt.Rows[0]["OBSV26"].ToString().Trim();
                        dd_Visual.SelectedItem.Text = dt.Rows[0]["OBSV27"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["LINKFILE"].ToString().Trim();
                        txtMaterial.Text = dt.Rows[0]["OBSV15"].ToString().Trim();
                        txtClient.Text = dt.Rows[0]["OBSV24"].ToString().Trim();
                        create_tab();
                        sg1_dr = null;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = "-";
                            sg1_dr["sg1_h2"] = "-";
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";
                            sg1_dr["sg1_f1"] = "-";
                            sg1_dr["sg1_f2"] = dt.Rows[i]["DTR1"].ToString().Trim();
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_f6"] = dt.Rows[i]["CUSTREF"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["OBSV1"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["OBSV2"].ToString().Trim();
                            //sg1_dr["sg1_t3"] = dt.Rows[i]["OBSV3"].ToString().Trim();
                            sg1_dr["sg1_t3"] = "-";
                            sg1_dr["sg1_t4"] = dt.Rows[i]["OBSV4"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["OBSV5"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["OBSV6"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["OBSV7"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["OBSV8"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["OBSV9"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["OBSV10"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["OBSV11"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["OBSV12"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["OBSV13"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["OBSV14"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["EQUIP_ID"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["FINISH"].ToString().Trim();
                            sg1_dr["sg1_t17"] = dt.Rows[i]["OBSV17"].ToString().Trim();
                            sg1_dr["sg1_t18"] = dt.Rows[i]["OBSV18"].ToString().Trim();
                            sg1_dr["sg1_t19"] = dt.Rows[i]["OBSV22"].ToString().Trim();
                            sg1_dr["sg1_t20"] = dt.Rows[i]["OBSV19"].ToString().Trim();
                            sg1_dr["sg1_t21"] = dt.Rows[i]["OBSV21"].ToString().Trim();
                            txtClientTag.Text = dt.Rows[i]["OBSV3"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        btnlbl4.Enabled = false;
                        edmode.Value = "Y";
                        //if (sg1.Rows.Count > 0)
                        //{
                        //    sg1.Rows[0].Cells[10].Enabled = false;
                        //    sg1.Rows[0].Cells[11].Enabled = false;
                        //    sg1.Rows[0].BackColor = Color.Khaki;
                        //}
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_qa_reps(frm_qstr);
                    break;

                case "TACODE":
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

                case "WO":
                    if (col1.Length <= 0) return;
                    SQuery = "Select Distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr,a.branchcd,a.work_ordno,b.aname as Customer,a.Pordno,a.org_invno as WO_NO,a.acode,a.icode,a.type,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.ordno,a.weight,a.cdrgno,to_char(a.orddt,'yyyymmdd') as vdd from Somas a,famst b where trim(a.acodE)=trim(b.acodE) and a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.org_invno)||trim(a.work_ordno)||trim(a.icode)||trim(a.cdrgno)='" + col1 + "' order by vdd desc,a.ordno desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        mq0 = "select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,COL11 AS FACING,COL12 AS FLANGE_STD,COL22 AS DESIGN_STD,COL4 AS VALVE_MODEL,COL3 AS RATING,SRNO FROM SCRATCH WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='WO' AND COL26='" + dt.Rows[0]["CDRGNO"].ToString().Trim() + "' AND COL27='" + dt.Rows[0]["WO_NO"].ToString().Trim() + "' AND UPPER(COL28)='" + dt.Rows[0]["TYPE"].ToString().Trim() + "/" + dt.Rows[0]["ORDNO"].ToString().Trim() + " DT." + dt.Rows[0]["ORDDT"].ToString().Trim() + "' AND ICODE='" + dt.Rows[0]["ICODE"].ToString().Trim() + "'";
                        mq0 = "select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,COL11 AS FACING,COL12 AS FLANGE_STD,COL22 AS DESIGN_STD,COL4 AS VALVE_MODEL,COL3 AS RATING,SRNO FROM SCRATCH WHERE BRANCHCD='" + dt.Rows[0]["BRANCHCD"].ToString().Trim() + "' AND TYPE='WO' AND COL26='" + dt.Rows[0]["CDRGNO"].ToString().Trim() + "' AND COL27='" + dt.Rows[0]["WO_NO"].ToString().Trim() + "' AND UPPER(COL28)='" + dt.Rows[0]["TYPE"].ToString().Trim() + "/" + dt.Rows[0]["ORDNO"].ToString().Trim() + " DT." + dt.Rows[0]["ORDDT"].ToString().Trim() + "' AND ICODE='" + dt.Rows[0]["ICODE"].ToString().Trim() + "'";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        if (dt2.Rows.Count > 0)
                        {
                            txtwoLno.Text = dt2.Rows[0]["srno"].ToString().Trim();
                            //txtDesg.Text = dt2.Rows[0]["DESIGN_STD"].ToString().Trim(); // IT IS CHANGED TO DROP DOWN LIST                     
                            if (dt.Rows[0]["WO_NO"].ToString().Trim().Substring(3, 2) == "BF")
                            {
                                txtFlange.Text = ""; // AS PER CLIENT,WORK ORDER'S GRID SEQUENCE IS MATCHING FOR ALL EXCEPT BF, SO USER WILL TYPE MANUALLY IN BF
                            }
                            else
                            {
                                txtFlange.Text = dt2.Rows[0]["FLANGE_STD"].ToString().Trim();
                            }
                            txtModel.Text = dt2.Rows[0]["VALVE_MODEL"].ToString().Trim();
                            txtRating.Text = dt2.Rows[0]["RATING"].ToString().Trim();
                        }
                        txtlbl4.Text = dt.Rows[0]["wo_no"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["weight"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["customer"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[0]["Pordno"].ToString().Trim();
                        txtProject.Text = dt.Rows[0]["work_ordno"].ToString().Trim();
                        txtOrder.Text = dt.Rows[0]["fstr"].ToString().Trim();
                        txtIcode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtWoLine.Text = dt.Rows[0]["cdrgno"].ToString().Trim();
                        FillGrid();
                        if (dt.Rows[0]["WO_NO"].ToString().Trim().Substring(3, 2) == "DP")
                        {
                            txtItem.Text = "DUAL PLATE CHECK VALVE";
                        }
                        else if (dt.Rows[0]["WO_NO"].ToString().Trim().Substring(3, 2) == "BF")
                        {
                            txtItem.Text = "BUTTERFLY CONCENTRIC VALVE";
                        }
                        else if (dt.Rows[0]["WO_NO"].ToString().Trim().Substring(3, 2) == "BD")
                        {
                            txtItem.Text = "BUTTERFLY DOUBLE OFFSET VALVE";
                        }
                        else if (dt.Rows[0]["WO_NO"].ToString().Trim().Substring(3, 2) == "BT")
                        {
                            txtItem.Text = "BUTTERFLY TRIPLE OFFSET VALVE";
                        }
                        else if (dt.Rows[0]["WO_NO"].ToString().Trim().Substring(3, 2) == "BV")
                        {
                            txtItem.Text = "BALANCING VALVE";
                        }
                    }
                    txtDrawing.Focus();
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,b.weight from ivoucher a,item i,somas b where trim(a.icode)=trim(i.icode) and upper(trim(a.finvno))=trim(b.type)||'/'||trim(b.ordno)||' DT.'||to_char(b.orddt,'dd/mm/yyyy') and trim(a.icode)=trim(b.icode) and trim(a.invno)=trim(b.org_invno) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                    //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                    SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and /*a.branchcd='" + frm_mbr + "' and*/ a.type='15' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    mq0 = "select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,COL11 AS FACING,COL12 AS FLANGE_STD,COL22 AS DESIGN_STD,COL4 AS VALVE_MODEL,COL3 AS RATING,COL2 AS SIZE_MM,COL25 AS CLIENT_TAG FROM SCRATCH WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='WO' AND COL26='" + txtWoLine.Text.Trim() + "' AND COL27='" + txtlbl4.Text.Trim() + "' AND UPPER(COL28)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND ICODE='" + txtIcode.Text.Trim() + "'";
                    mq0 = "select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,COL11 AS FACING,COL12 AS FLANGE_STD,COL22 AS DESIGN_STD,COL4 AS VALVE_MODEL,COL3 AS RATING,COL2 AS SIZE_MM,COL25 AS CLIENT_TAG FROM SCRATCH WHERE BRANCHCD='" + txtOrder.Text.Trim().Substring(0, 2) + "' AND TYPE='WO' AND COL26='" + txtWoLine.Text.Trim() + "' AND COL27='" + txtlbl4.Text.Trim() + "' AND UPPER(COL28)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND ICODE='" + txtIcode.Text.Trim() + "'";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in Hidden Field
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[d]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["finvno"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = dt.Rows[d]["TAG_NO"].ToString().Trim();
                        if (dt2.Rows.Count > 0)
                        {
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = dt2.Rows[0]["SIZE_MM"].ToString().Trim();
                            //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = dt2.Rows[0]["CLIENT_TAG"].ToString().Trim(); // AS PER USER THEY WILL TYPE IT MANUALLY
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t19")).Text = dt2.Rows[0]["FACING"].ToString().Trim();
                        }
                    }
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    if (ViewState["sg1"] != null)
                    {
                        dt = new DataTable();
                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = Convert.ToInt32(sg1.Rows[i].Cells[12].Text);
                            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.ToString();
                            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.ToString();
                            sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.ToString();
                            sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.ToString();
                            sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.ToString();
                            sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.ToString();
                            sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.ToString();
                            sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.ToString();
                            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.ToString();
                            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.ToString();
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.ToString();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.ToString();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.ToString();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.ToString();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.ToString();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.ToString();
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                            sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                            sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                            sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();
                            sg1_dr["sg1_t19"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                            sg1_dr["sg1_t20"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim();
                            sg1_dr["sg1_t21"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,b.weight from ivoucher a,item i,somas b where trim(a.icode)=trim(i.icode) and upper(trim(a.finvno))=trim(b.type)||'/'||trim(b.ordno)||' DT.'||to_char(b.orddt,'dd/mm/yyyy') and trim(a.icode)=trim(b.icode) and trim(a.invno)=trim(b.org_invno) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                        //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                        SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and /*a.branchcd='" + frm_mbr + "' and*/ a.type='15' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') IN (" + col1.Trim() + ") order by Tag_no";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        mq0 = "select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,COL11 AS FACING,COL12 AS FLANGE_STD,COL22 AS DESIGN_STD,COL4 AS VALVE_MODEL,COL3 AS RATING,COL2 AS SIZE_MM,COL25 AS CLIENT_TAG FROM SCRATCH WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='WO' AND COL26='" + txtWoLine.Text.Trim() + "' AND COL27='" + txtlbl4.Text.Trim() + "' AND UPPER(COL28)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND ICODE='" + txtIcode.Text.Trim() + "'";
                        mq0 = "select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,COL11 AS FACING,COL12 AS FLANGE_STD,COL22 AS DESIGN_STD,COL4 AS VALVE_MODEL,COL3 AS RATING,COL2 AS SIZE_MM,COL25 AS CLIENT_TAG FROM SCRATCH WHERE BRANCHCD='" + txtOrder.Text.Trim().Substring(0, 2) + "' AND TYPE='WO' AND COL26='" + txtWoLine.Text.Trim() + "' AND COL27='" + txtlbl4.Text.Trim() + "' AND UPPER(COL28)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND ICODE='" + txtIcode.Text.Trim() + "'";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = "-";
                            sg1_dr["sg1_h2"] = "-";
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";
                            sg1_dr["sg1_f1"] = "-";
                            sg1_dr["sg1_f2"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_f6"] = dt.Rows[d]["finvno"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[d]["tag_no"].ToString().Trim();
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = "";
                            sg1_dr["sg1_t5"] = "";
                            sg1_dr["sg1_t6"] = "";
                            sg1_dr["sg1_t7"] = "";
                            sg1_dr["sg1_t8"] = "";
                            sg1_dr["sg1_t9"] = "";
                            sg1_dr["sg1_t10"] = "";
                            sg1_dr["sg1_t11"] = "";
                            sg1_dr["sg1_t12"] = "";
                            sg1_dr["sg1_t13"] = "";
                            sg1_dr["sg1_t14"] = "";
                            sg1_dr["sg1_t15"] = "";
                            sg1_dr["sg1_t16"] = "";
                            sg1_dr["sg1_t17"] = "";
                            sg1_dr["sg1_t18"] = "";
                            if (dt2.Rows.Count > 0)
                            {
                                sg1_dr["sg1_t1"] = dt2.Rows[0]["SIZE_MM"].ToString().Trim();
                                //sg1_dr["sg1_t3"] = dt2.Rows[0]["CLIENT_TAG"].ToString().Trim(); // AS PER USER THEY WILL TYPE IT MANUALLY
                                sg1_dr["sg1_t19"] = dt2.Rows[0]["FACING"].ToString().Trim();
                            }
                            sg1_dr["sg1_t20"] = "";
                            sg1_dr["sg1_t21"] = "";
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    setColHeadings();
                    //if (sg1.Rows.Count > 0)
                    //{
                    //    sg1.Rows[0].Cells[10].Enabled = false;
                    //    sg1.Rows[0].Cells[11].Enabled = false;
                    //    sg1.Rows[0].BackColor = Color.Khaki;
                    //}
                    #endregion
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
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = (i + 1);
                            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.Trim();
                            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.Trim();
                            sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.Trim();
                            sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.Trim();
                            sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.Trim();
                            sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.Trim();
                            sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.Trim();
                            sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.Trim();
                            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.Trim();
                            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.Trim();
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.Trim();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.Trim();
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                            sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                            sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                            sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();
                            sg1_dr["sg1_t19"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                            sg1_dr["sg1_t20"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim();
                            sg1_dr["sg1_t21"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        for (i = 0; i < sg1.Rows.Count; i++)
                        {
                            //sg1.Rows[0].Cells[10].Enabled = false;
                            //sg1.Rows[0].Cells[11].Enabled = false;
                            //sg1.Rows[0].BackColor = Color.Khaki;
                            sg1.Rows[i].Cells[12].Text = (i + 1).ToString();
                        }
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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "sELECT distinct trim(a.Vchnum) as Sheet_No,to_char(a.Vchdate,'dd/mm/yyyy') as Sheet_Dt,a.Job_no,a.Job_Dt,a.icode,i.iname,i.unit,a.a1 as qty_issue,a.a2 as no_of_cuts,a.a3 as cut_sheet,a.a4 as rej_sheet,a.mchcode as machine_code,a.mcstart as start_time,mcstop as stop_time,a.num1 as ok_sheet,a.num2 as total_cutsheet,a.Ent_by,a.Ent_Dt,to_char(a.vchdate,'yyyymmdd') as vdd FROM " + frm_tabname + " a,item i WHERE trim(a.icode)=trim(i.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' AND a.VCHDATE  " + PrdRange + " ORDER BY vdd DESC,Sheet_No DESC";
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
                last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  ", "ldt");
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
                            //for (i = 0; i < sg1.Rows.Count - 0; i++)
                            //{
                            //    if (((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().Length > 1)
                            //    {
                            //        save_it = "Y";
                            //    }
                            //}
                            for (i = 0; i < sg1.Rows.Count - 1; i++)
                            {
                                save_it = "Y";
                            }
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
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); BlankDropDown();
                    }
                    catch (Exception ex)
                    {
                        fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N"; btnsave.Disabled = false;
                    }
            #endregion
                }
            }
        }
    }
    //------------------------------------------------------------------------------------
    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        // Hidden Field
        sg1_dt.Columns.Add(new DataColumn("sg1_h1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h10", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(Int32)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t10", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t11", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t13", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t14", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t16", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t20", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t21", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t4", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void create_tab3()
    {
        sg3_dt = new DataTable();
        sg3_dr = null;
        sg3_dt.Columns.Add(new DataColumn("sg3_SrNo", typeof(Int32)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t3", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t4", typeof(string)));

    }
    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        if (sg1_dt != null)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_h1"] = "-";
            sg1_dr["sg1_h2"] = "-";
            sg1_dr["sg1_h3"] = "-";
            sg1_dr["sg1_h4"] = "-";
            sg1_dr["sg1_h5"] = "-";
            sg1_dr["sg1_h6"] = "-";
            sg1_dr["sg1_h7"] = "-";
            sg1_dr["sg1_h8"] = "-";
            sg1_dr["sg1_h9"] = "-";
            sg1_dr["sg1_h10"] = "-";
            sg1_dr["sg1_SrNo"] = sg1_dt.Rows.Count + 1;
            sg1_dr["sg1_f1"] = "-";
            sg1_dr["sg1_f2"] = "-";
            sg1_dr["sg1_f3"] = "-";
            sg1_dr["sg1_f4"] = "-";
            sg1_dr["sg1_f5"] = "-";
            sg1_dr["sg1_f6"] = "-";
            sg1_dr["sg1_t1"] = "-";
            sg1_dr["sg1_t2"] = "-";
            sg1_dr["sg1_t3"] = "-";
            sg1_dr["sg1_t4"] = "-";
            sg1_dr["sg1_t5"] = "-";
            sg1_dr["sg1_t6"] = "-";
            sg1_dr["sg1_t7"] = "-";
            sg1_dr["sg1_t8"] = "-";
            sg1_dr["sg1_t9"] = "-";
            sg1_dr["sg1_t10"] = "-";
            sg1_dr["sg1_t11"] = "-";
            sg1_dr["sg1_t12"] = "-";
            sg1_dr["sg1_t13"] = "-";
            sg1_dr["sg1_t14"] = "-";
            sg1_dr["sg1_t15"] = "-";
            sg1_dr["sg1_t16"] = "-";
            sg1_dr["sg1_t17"] = "-";
            sg1_dr["sg1_t18"] = "-";
            sg1_dr["sg1_t19"] = "-";
            sg1_dr["sg1_t20"] = "-";
            sg1_dr["sg1_t21"] = "-";
            sg1_dt.Rows.Add(sg1_dr);
        }
    }
    //------------------------------------------------------------------------------------
    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();
        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
        sg2_dr["sg2_t1"] = "-";
        sg2_dr["sg2_t2"] = "-";
        sg2_dt.Rows.Add(sg2_dr);
    }
    //------------------------------------------------------------------------------------
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
    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg1r = 0; sg1r < sg1.Rows.Count; sg1r++)
            {
                for (int j = 0; j < sg1.Columns.Count; j++)
                {
                    sg1.Rows[sg1r].Cells[j].ToolTip = sg1.Rows[sg1r].Cells[j].Text;
                    if (sg1.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
                    {
                        sg1.Rows[sg1r].Cells[j].Text = sg1.Rows[sg1r].Cells[j].Text.Substring(0, 35);
                    }
                    if (sg1r == 0)
                    {
                        sg1.Rows[sg1r].Cells[j].Enabled = false;
                        sg1.Rows[sg1r].Cells[21].Enabled = true;
                        sg1.Rows[sg1r].Cells[38].Enabled = true;

                        sg1.Rows[sg1r].Cells[32].Enabled = true;
                        sg1.Rows[sg1r].Cells[33].Enabled = true;
                        sg1.Rows[sg1r].Cells[34].Enabled = true;
                        sg1.Rows[sg1r].Cells[35].Enabled = true;
                        sg1.Rows[sg1r].BackColor = Color.Khaki;
                    }
                    if (sg1r == 1)
                    {
                        sg1.Rows[1].Cells[10].Enabled = false;
                        sg1.Rows[1].Cells[11].Enabled = false;
                        ((TextBox)sg1.Rows[1].FindControl("sg1_t1")).Text = "REQUIRED DIMENSIONS & FEATURE";
                        ((TextBox)sg1.Rows[1].FindControl("sg1_t1")).Enabled = false;
                        ((TextBox)sg1.Rows[1].FindControl("sg1_t19")).ReadOnly = false;
                    }
                }
            }
            //sg1.HeaderRow.Cells[18].Style["display"] = "none";
            //e.Row.Cells[18].Style["display"] = "none";
            sg1.Columns[10].HeaderStyle.Width = 30;
            sg1.Columns[11].HeaderStyle.Width = 30;
            sg1.Columns[12].HeaderStyle.Width = 50;
            sg1.Columns[13].HeaderStyle.Width = 0;
            sg1.Columns[15].HeaderStyle.Width = 0;
            sg1.Columns[16].HeaderStyle.Width = 0;
            sg1.Columns[17].HeaderStyle.Width = 0;
            sg1.Columns[18].HeaderStyle.Width = 0;
            //sg1.Columns[19].HeaderStyle.Width = 125;
            //sg1.Columns[20].HeaderStyle.Width = 335;
            sg1.Columns[21].HeaderStyle.Width = 0;
            //sg1.Columns[22].HeaderStyle.Width = 335;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG1_RMV":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Tag From The List");
                }
                break;

            case "SG1_ROW_ADD":
                if (txtlbl4.Text.Trim().Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Select Work Order");
                    return;
                }
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Tag", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Tag", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "WO";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Result", frm_qstr);
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
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            //if (((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().Length > 1)
            //{
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum.Trim().ToUpper();
            oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow["SRNO"] = i + 1;
            oporow["TITLE"] = txtlbl2.Text.Trim().ToUpper();
            oporow["BTCHNO"] = txtWoLine.Text.Trim().ToUpper();
            oporow["ACODE"] = txtlbl7.Text.Trim().ToUpper();
            oporow["ICODE"] = txtIcode.Text.Trim().ToUpper();
            oporow["CPARTNO"] = txtlbl4.Text.Trim().ToUpper();
            oporow["GRADE"] = txtOrder.Text.Trim().ToUpper();
            oporow["COL1"] = txtlbl4a.Text.Trim().ToUpper();
            oporow["COL2"] = txtModel.Text.Trim().ToUpper();
            oporow["COL3"] = dd_Desg.SelectedItem.Text.Trim().ToUpper();
            oporow["COL4"] = txtDrawing.Text.Trim().ToUpper();
            oporow["COL5"] = txtwoLno.Text.Trim().ToUpper();
            oporow["COL6"] = txtProject.Text.Trim().ToUpper();
            oporow["WONO"] = txtRating.Text.Trim().ToUpper();
            oporow["MATL"] = txtFlange.Text.Trim().ToUpper();
            oporow["MRRNUM"] = "-";
            oporow["MRRDATE"] = "-";
            oporow["BTCHDT"] = "-";
            oporow["RESULT"] = "-";
            oporow["OBSV1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
            oporow["OBSV2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper();
            //oporow["OBSV3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper();
            oporow["OBSV3"] = txtClientTag.Text.Trim().ToUpper();
            oporow["OBSV4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper();
            oporow["OBSV5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper();
            oporow["OBSV6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper();
            oporow["OBSV7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper();
            oporow["OBSV8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper();
            oporow["OBSV9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper();
            oporow["OBSV10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().ToUpper();
            oporow["OBSV11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim().ToUpper();
            oporow["OBSV12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim().ToUpper();
            oporow["OBSV13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim().ToUpper();
            oporow["OBSV14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim().ToUpper();
            oporow["OBSV15"] = txtMaterial.Text.Trim().ToUpper();
            oporow["OBSV16"] = "-";
            oporow["OBSV17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim().ToUpper();
            oporow["OBSV18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim().ToUpper();
            oporow["OBSV19"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim().ToUpper();
            oporow["CONTPLAN"] = "-";
            oporow["SAMPQTY"] = 0;
            oporow["EQUIP_ID"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim().ToUpper();
            oporow["FINISH"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().ToUpper();
            oporow["OMAX"] = "-";
            oporow["OMIN"] = "-";
            oporow["EXPDATE"] = "-";
            oporow["OBSV20"] = txtItem.Text.Trim().ToUpper();
            oporow["OBSV21"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim().ToUpper();
            oporow["OBSV22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim().ToUpper();
            oporow["OBSV23"] = "-";
            oporow["OBSV24"] = txtClient.Text.Trim().ToUpper();
            oporow["OBSV25"] = dd_Dimension.SelectedItem.Text.Trim().ToUpper();
            oporow["OBSV26"] = dd_Tested.SelectedItem.Text.Trim().ToUpper();
            oporow["OBSV27"] = dd_Visual.SelectedItem.Text.Trim().ToUpper();
            if (txtrmk.Text.Trim().Length > 200)
            {
                oporow["LINKFILE"] = txtrmk.Text.Trim().ToUpper().Substring(0, 199);
            }
            else
            {
                oporow["LINKFILE"] = txtrmk.Text.Trim().ToUpper();
            }
            oporow["FIGURE_NO"] = "-";
            oporow["CUSTREF"] = sg1.Rows[i].Cells[18].Text.Trim().ToUpper();
            oporow["OBSV28"] = "-";
            oporow["OBSV29"] = "-";
            oporow["APP_BY"] = "-";
            oporow["APP_DT"] = vardate;
            oporow["OBSV30"] = "-";
            oporow["OBSV31"] = "-";
            oporow["REJQTY"] = 0;
            oporow["NUM1"] = 0;
            oporow["NUM2"] = 0;
            oporow["DTR1"] = sg1.Rows[i].Cells[14].Text.Trim().ToUpper();
            oporow["DTT1"] = 0;
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
            //}
        }
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "84");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------
    protected void FillDropDown()
    {
        txtClient.Text = "CLIENT TAG NO. / ITEM CODE";
        SQuery = "SELECT 'PLEASE SELECT' AS FSTR FROM DUAL UNION ALL SELECT 'YES' AS FSTR FROM DUAL UNION ALL SELECT 'NO' AS FSTR FROM DUAL";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        dd_Dimension.DataSource = dt;
        dd_Dimension.DataTextField = "fstr";
        dd_Dimension.DataValueField = "fstr";
        dd_Dimension.DataBind();

        dd_Visual.DataSource = dt;
        dd_Visual.DataTextField = "fstr";
        dd_Visual.DataValueField = "fstr";
        dd_Visual.DataBind();

        dd_Tested.DataSource = dt;
        dd_Tested.DataTextField = "fstr";
        dd_Tested.DataValueField = "fstr";
        dd_Tested.DataBind();

        SQuery = "SELECT 'PLEASE SELECT' AS FSTR FROM DUAL UNION ALL SELECT 'API 609' AS FSTR FROM DUAL UNION ALL SELECT 'API-594' AS FSTR FROM DUAL UNION ALL SELECT 'API-6D' AS FSTR FROM DUAL";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        dd_Desg.DataSource = dt;
        dd_Desg.DataTextField = "fstr";
        dd_Desg.DataValueField = "fstr";
        dd_Desg.DataBind();
    }
    //------------------------------------------------------------------------------------
    protected void BlankDropDown()
    {   
        dd_Dimension.Items.Clear();
        dd_Visual.Items.Clear();
        dd_Tested.Items.Clear();
        dd_Desg.Items.Clear();
    }
    //------------------------------------------------------------------------------------
    private void FillGrid()
    {
        create_tab();
        //dt2 = new DataTable();
        //SQuery = "SELECT 'SIZE(MM)' AS HEADING FROM DUAL UNION ALL SELECT 'VALVE TAG NO.' AS HEADING FROM DUAL UNION ALL SELECT 'CLIENT TAG NO.' AS HEADING FROM DUAL UNION ALL SELECT 'A' AS HEADING FROM DUAL UNION ALL SELECT 'B' AS HEADING FROM DUAL UNION ALL SELECT 'C' AS HEADING FROM DUAL UNION ALL SELECT 'ØD' AS HEADING FROM DUAL UNION ALL SELECT 'E' AS HEADING FROM DUAL UNION ALL SELECT 'ØF' AS HEADING FROM DUAL UNION ALL SELECT 'G' AS HEADING FROM DUAL UNION ALL SELECT 'I' AS HEADING FROM DUAL UNION ALL SELECT 'J' AS HEADING FROM DUAL UNION ALL SELECT 'PCD' AS HEADING FROM DUAL UNION ALL SELECT 'NO.OF TAPPED HOLES MARKED $' AS HEADING FROM DUAL UNION ALL SELECT 'SIZE X DEPTH OF TAPPING MARKED $' AS HEADING FROM DUAL UNION ALL SELECT 'NO.OF THRU HOLES /  NO.OF TAPPED HOLES' AS HEADING FROM DUAL UNION ALL SELECT 'DIA. OF THRU/ TAPPED HOLES' AS HEADING FROM DUAL UNION ALL SELECT 'TOTAL NO. OF HOLES' AS HEADING FROM DUAL UNION ALL SELECT 'FACING / FINISH' AS HEADING FROM DUAL UNION ALL SELECT '-' AS HEADING FROM DUAL UNION ALL SELECT 'REMARKS' AS HEADING FROM DUAL";
        //dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        //z = 1;
        //sg1_dr = sg1_dt.NewRow();
        //for (int i = 0; i < dt2.Rows.Count; i++)
        //{
        //    sg1_dr["sg1_srno"] = 1;
        //    sg1_dr["sg1_t" + z] = dt2.Rows[i]["heading"].ToString().Trim();
        //    z++;
        //}
        ArrayList GridHeading = new ArrayList();
        GridHeading.Add("SIZE(MM)");
        GridHeading.Add("VALVE TAG NO.");
        GridHeading.Add("CLIENT TAG NO.");
        GridHeading.Add("A");
        GridHeading.Add("B");
        GridHeading.Add("C");
        GridHeading.Add("ØD");
        GridHeading.Add("E");
        GridHeading.Add("ØF");
        GridHeading.Add("G");
        GridHeading.Add("I");
        GridHeading.Add("J");
        GridHeading.Add("PCD");
        GridHeading.Add("NO.OF TAPPED HOLES MARKED $");
        GridHeading.Add("SIZE X DEPTH OF TAPPING MARKED $");
        GridHeading.Add("NO.OF THRU HOLES /  NO.OF TAPPED HOLES");
        GridHeading.Add("DIA. OF THRU/ TAPPED HOLES");
        GridHeading.Add("TOTAL NO. OF HOLES");
        GridHeading.Add("FACING / FINISH");
        GridHeading.Add("");
        GridHeading.Add("REMARKS");
        z = 1;
        sg1_dr = sg1_dt.NewRow();
        for (int i = 0; i < GridHeading.Count; i++)
        {
            sg1_dr["sg1_srno"] = 1;
            sg1_dr["sg1_f2"] = "-";
            sg1_dr["sg1_f6"] = "-";
            sg1_dr["sg1_t" + z] = GridHeading[i].ToString().Trim();
            z++;
        }
        sg1_dt.Rows.Add(sg1_dr);
        sg1_add_blankrows();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        //if (sg1.Rows.Count > 0)
        //{
        //    sg1.Rows[0].Cells[10].Enabled = false;
        //    sg1.Rows[0].Cells[11].Enabled = false;
        //    sg1.Rows[0].BackColor = Color.Khaki;
        //}
    }
    //------------------------------------------------------------------------------------
}