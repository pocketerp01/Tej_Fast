using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_mpe : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", xStartDt = "", Enable = "";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it, mq0;
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
            btnlist.Visible = false;
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

                ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");

            }
            orig_name = orig_name.ToUpper();
            //if (sg1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_NAME"))
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
        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnprint.Disabled = false;
        create_tab();
        sg1_add_blankrows();
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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "inspvch";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "79");
        lblheader.Text = "Magnetic Particle Examination";
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

            case "WONO":
                //SQuery = "Select Distinct a.branchcd||a.type||a.ordno||to_char(a.orddt,'dd/mm/yyyy')||trim(a.org_invno)||trim(a.icode) as fstr, trim(b.aname) as Customer,trim(a.Pordno) as Pordno,trim(a.org_invno) as WO_NO,trim(a.acode) as acode,to_char(a.orddt,'dd/mm/yyyy') as orddt,trim(a.ordno) as ordno from Somas a, famst b where trim(a.acodE)=trim(b.acodE) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='4' and length(trim(nvl(a.app_by,'-')))> 1 and length(trim(nvl(a.org_invno,'-')))> 1 order by ordno desc";            
                SQuery = "Select Distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.org_invno)||trim(a.work_ordno)||trim(a.icode)||trim(a.cdrgno) as fstr,b.aname as Customer,a.Pordno,a.org_invno as WO_NO,a.acode,a.work_ordno as project,a.icode,i.iname,a.cdrgno as so_line_no,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.ordno,to_char(a.orddt,'yyyymmdd') as vdd from Somas a,famst b,item i where trim(a.acodE)=trim(b.acodE) and trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='4' and length(trim(nvl(a.app_by,'-')))> 1 and length(trim(nvl(a.org_invno,'-')))> 1 order by vdd desc,a.ordno desc";
                SQuery = "Select Distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.org_invno)||trim(a.work_ordno)||trim(a.icode)||trim(a.cdrgno) as fstr,b.aname as Customer,a.Pordno,a.org_invno as WO_NO,a.acode,a.work_ordno as project,a.icode,i.iname,a.cdrgno as so_line_no,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.ordno,to_char(a.orddt,'yyyymmdd') as vdd from Somas a,famst b,item i where trim(a.acodE)=trim(b.acodE) and trim(a.icode)=trim(i.icode) and a.branchcd!='DD' and substr(a.type,1,1)='4' and a.type!='44' and length(trim(nvl(a.app_by,'-')))> 1 and length(trim(nvl(a.org_invno,'-')))> 1 order by vdd desc,a.ordno desc";
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
                //SQuery = "Select icode as fstr, Ciname,Cpartno,icode,cdrgno from somas where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + txtgrade.Text.Trim() + "' order by Srno";
                //SQuery = "Select trim(icode)||trim(invno)||REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') as fstr,invno as Wo_No,desc_ as Tag_no,IQTYIN AS Qty,to_char(vchdate,'dd/mm/yyyy') as Entry_Date,Ent_by,icode,finvno from ivoucher where branchcd='" + frm_mbr + "' and type='15' and invno='" + txtlbl4.Text.Trim() + "' and icode='" + txtIcode.Text + "' and upper(finvno)='" + txtgrade.Text.Trim().Substring(2, 2) + "/" + txtgrade.Text.Trim().Substring(4, 6) + " DT." + txtgrade.Text.Trim().Substring(10, 10) + "' AND REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') NOT IN (" + col1 + ") order by desc_";
                //SQuery = "Select trim(icode)||trim(invno)||REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') as fstr,invno as Wo_No,desc_ as Tag_no,IQTYIN AS Qty,to_char(vchdate,'dd/mm/yyyy') as Entry_Date,Ent_by,icode,finvno from ivoucherp where branchcd='" + frm_mbr + "' and type='15' and invno='" + txtlbl4.Text.Trim() + "' and upper(finvno)='" + txtgrade.Text.Trim().Substring(2, 2) + "/" + txtgrade.Text.Trim().Substring(4, 6) + " DT." + txtgrade.Text.Trim().Substring(10, 10) + "' AND REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') NOT IN (" + col1 + ") order by desc_";
                SQuery = "Select trim(icode)||trim(invno)||REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') as fstr,invno as Wo_No,desc_ as Tag_no,IQTYIN AS Qty,to_char(vchdate,'dd/mm/yyyy') as Entry_Date,Ent_by,icode,finvno from ivoucherp where /*branchcd='" + frm_mbr + "' and*/ type='15' and invno='" + txtlbl4.Text.Trim() + "' AND REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') NOT IN (" + col1 + ") order by desc_";
                break;

            case "SG1_ROW_ADD1":
            case "SG1_ROW_ADD_E1":
                string stage = "0";
                stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text;
                SQuery = "";
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
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "SELECT distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,A.TITLE AS PONO,A.CPARTNO AS WO_NO,A.ACODE AS CUSTOMER_CODE,TRIM(B.ANAME) AS CUSTOMER FROM " + frm_tabname + " A, FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE  " + DateRange + " ORDER BY A.VCHNUM DESC";
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
        DDBind();
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            frm_vty = "79";
            lbl1a.Text = frm_vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);

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
        txttstDate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtprocref.Text = "QAD-W-08 REV.00 ISSUE-02";
        txtfootnote.Text = "1) AS PER MESC-SPE 77/302,NDT WILL BE PERFORMED BY PERSONNEL NDT LEVEL II QUALIFICATION,WHERE REQUIRED INCLUDING SHELL EFA";
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();
        // Popup asking for Copy from Older Data
        //fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        //hffield.Value = "NEW_E";
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
        //if (txtPoLine.Text == "-")
        //{
        //    fgen.msg("-", "AMSG", "Please Fill PO Line Item No.");
        //    txtPoLine.Focus();
        //    return;
        //}
        if (txtsurf_temp.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill " + lbl6.Text);
            txtsurf_temp.Focus();
            return;
        }
        if (ddsurfprep.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Surface Prep");
            ddsurfprep.Focus();
            return;
        }
        if (ddstgtest.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Test Stage");
            ddstgtest.Focus();
            return;
        }
        if (txtproject.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Project");
            txtproject.Focus();
            return;
        }
        if (ddmagparti.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Type of Mag Particles");
            ddmagparti.Focus();
            return;
        }

        if (ddmagntech.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Mag Technique");
            ddmagntech.Focus();
            return;
        }

        if (ddmthodpoder.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Method Of Powder");
            ddmthodpoder.Focus();
            return;
        }

        if (ddsurfprep2.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Surface Preparation");
            ddsurfprep2.Focus();
            return;
        }

        if (ddmagcheck.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Mag Adequacy Check");
            ddmagcheck.Focus();
            return;
        }

        if (ddacptstand.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Acceptance Standard");
            ddacptstand.Focus();
            return;
        }

        if (txtItem.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Product");
            txtItem.Focus();
            return;
        }

        if (dddemag.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Demagnetization");
            dddemag.Focus();
            return;
        }

        if (ddpostclean.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Post_Cleaning");
            ddpostclean.Focus();
            return;
        }

        if (ddlift_pwer.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Lifting Power Of Yoke");
            ddlift_pwer.Focus();
            return;
        }

        if (txtComponent.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill Component");
            txtComponent.Focus();
            return;
        }

        if (txtprocref.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill " + Label7.Text);
            txtprocref.Focus();
            return;
        }

        if (txtmatlspec.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill " + lbl102.Text);
            txtmatlspec.Focus();
            return;
        }

        if (ddmagntype.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Magn Current Type");
            ddmagntype.Focus();
            return;
        }

        if (txtValve_Size_Rating.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill Valve Size & Rating");
            txtValve_Size_Rating.Focus();
            return;
        }

        if (txtlightequip.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill " + lbl12.Text);
            txtlightequip.Focus();
            return;
        }

        if (txtjbthick.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill " + Label6.Text);
            txtjbthick.Focus();
            return;
        }

        if (txtfootnote.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill " + Label11.Text);
            txtfootnote.Focus();
            return;
        }

        if (sg1.Rows.Count <= 1)
        {
            fgen.msg("-", "AMSG", "Please Select Tag");
            return;
        }

        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            //if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() == "-")
            //{
            //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill PO Line Item No. At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
            //    return;
            //}
            if (((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim() == "-")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Tag No. At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
                return;
            }

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() == "-")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Heat No. At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
                return;
            }

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim() == "-")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Job Deccription At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
                return;
            }

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim() == "-")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Record Of Indication At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
                return;
            }

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim() == "-")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Grid Interpretation At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
                return;
            }

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim() == "-")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Grid Remarks At Line No." + sg1.Rows[i].Cells[12].Text.Trim());
                return;
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
        DDClear();
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
                        //txtlbl4a.Text = dt.Rows[i]["text"].ToString().Trim();
                        //txtlbl2.Text = dt.Rows[i]["frm_header"].ToString().Trim();
                        //txtlbl7.Text = dt.Rows[0]["ent_id"].ToString().Trim();
                        //txtlbl7a.Text = dt.Rows[0]["ent_by"].ToString().Trim();
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
                    SQuery = "Select a.* ,trim(c.aname) as aname from " + frm_tabname + " a ,famst c where trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    DDBind();
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                        txtgrade.Text = dt.Rows[0]["grade"].ToString().Trim();
                        txtpono.Text = dt.Rows[0]["title"].ToString().Trim();
                        txtwoline.Text = dt.Rows[0]["BTCHNO"].ToString().Trim();
                        txtwolno.Text = dt.Rows[0]["OBSV6"].ToString().Trim();
                        txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtaname.Text = dt.Rows[0]["ANAME"].ToString().Trim();
                        txtIcode.Text = dt.Rows[0]["ICODE"].ToString().Trim();
                        ////div 2
                        txttstDate.Text = dt.Rows[0]["DOC_DT"].ToString().Trim();
                        txtsurf_temp.Text = dt.Rows[0]["matl"].ToString().Trim();
                        ddsurfprep.Text = dt.Rows[0]["WONO"].ToString().Trim();
                        ddstgtest.Text = dt.Rows[0]["obsv2"].ToString().Trim();
                        txtproject.Text = dt.Rows[0]["OBSV15"].ToString().Trim();
                        ddmagparti.Text = dt.Rows[0]["omax"].ToString().Trim();
                        ////div 3
                        ddmagntech.Text = dt.Rows[0]["col3"].ToString().Trim();
                        ddmthodpoder.Text = dt.Rows[0]["col4"].ToString().Trim();
                        ddsurfprep2.Text = dt.Rows[0]["OBSV22"].ToString().Trim();
                        ddmagcheck.Text = dt.Rows[0]["OBSV21"].ToString().Trim();
                        txtprocref.Text = dt.Rows[0]["OBSV23"].ToString().Trim();
                        txtmatlspec.Text = dt.Rows[0]["col2"].ToString().Trim();
                        ddmagntype.Text = dt.Rows[0]["omin"].ToString().Trim();
                        ////div 4
                        ddacptstand.Text = dt.Rows[0]["OBSV16"].ToString().Trim();
                        txtItem.Text = dt.Rows[0]["OBSV17"].ToString().Trim();
                        dddemag.Text = dt.Rows[0]["OBSV18"].ToString().Trim();
                        ddpostclean.Text = dt.Rows[0]["OBSV19"].ToString().Trim();
                        txtlightequip.Text = dt.Rows[0]["col6"].ToString().Trim();
                        txtjbthick.Text = dt.Rows[0]["OBSV20"].ToString().Trim();
                        ddlift_pwer.Text = dt.Rows[0]["col5"].ToString().Trim();
                        ////div 5
                        txtfootnote.Text = dt.Rows[0]["LINKFILE"].ToString().Trim();
                        txtPoLine.Text = dt.Rows[0]["obsv24"].ToString().Trim();
                        txtValve_Size_Rating.Text = dt.Rows[0]["FOOTNOTE"].ToString().Trim();
                        txtComponent.Text = dt.Rows[0]["OBSV26"].ToString().Trim();
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
                            sg1_dr["sg1_t1"] = dt.Rows[i]["obsv7"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["obsv1"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["obsv8"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["col1"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["obsv3"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["obsv4"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["obsv5"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t2")).Focus();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        btnlbl4.Enabled = false;
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_qa_reps(frm_qstr);
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

                case "WONO":
                    if (col1.Length <= 0) return;
                    SQuery = "Select Distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr,a.branchcd,a.work_ordno,b.aname as Customer,a.Pordno,a.org_invno as WO_NO,a.acode,a.icode,a.type,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.ordno,a.weight,a.cdrgno,to_char(a.orddt,'yyyymmdd') as vdd from Somas a,famst b where trim(a.acodE)=trim(b.acodE) and a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.org_invno)||trim(a.work_ordno)||trim(a.icode)||trim(a.cdrgno)='" + col1 + "' order by vdd desc,a.ordno desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        mq0 = "select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,SRNO FROM SCRATCH WHERE BRANCHCD='" + frm_mbr + "' AND TYPE ='WO' AND COL26='" + dt.Rows[0]["CDRGNO"].ToString().Trim() + "' AND COL27='" + dt.Rows[0]["WO_NO"].ToString().Trim() + "' AND UPPER(COL28)='" + dt.Rows[0]["TYPE"].ToString().Trim() + "/" + dt.Rows[0]["ORDNO"].ToString().Trim() + " DT." + dt.Rows[0]["ORDDT"].ToString().Trim() + "' AND ICODE='" + dt.Rows[0]["ICODE"].ToString().Trim() + "'";
                        mq0 = "select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,SRNO,COL3 AS RATING,COL2 AS SIZE_MM FROM SCRATCH WHERE BRANCHCD='" + dt.Rows[0]["BRANCHCD"].ToString().Trim() + "' AND TYPE ='WO' AND COL26='" + dt.Rows[0]["CDRGNO"].ToString().Trim() + "' AND COL27='" + dt.Rows[0]["WO_NO"].ToString().Trim() + "' AND UPPER(COL28)='" + dt.Rows[0]["TYPE"].ToString().Trim() + "/" + dt.Rows[0]["ORDNO"].ToString().Trim() + " DT." + dt.Rows[0]["ORDDT"].ToString().Trim() + "' AND ICODE='" + dt.Rows[0]["ICODE"].ToString().Trim() + "'";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        if (dt2.Rows.Count > 0)
                        {
                            txtwolno.Text = dt2.Rows[0]["SRNO"].ToString().Trim();
                            txtValve_Size_Rating.Text = dt2.Rows[0]["SIZE_MM"].ToString().Trim() + "-" + dt2.Rows[0]["RATING"].ToString().Trim();
                        }
                        txtlbl4.Text = dt.Rows[0]["wo_no"].ToString().Trim();
                        txtwoline.Text = dt.Rows[0]["cdrgno"].ToString().Trim();
                        txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtaname.Text = dt.Rows[0]["customer"].ToString().Trim();
                        txtpono.Text = dt.Rows[0]["Pordno"].ToString().Trim();
                        txtproject.Text = dt.Rows[0]["work_ordno"].ToString().Trim();
                        txtgrade.Text = dt.Rows[0]["fstr"].ToString().Trim();
                        txtIcode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtPoLine.Text = dt.Rows[0]["weight"].ToString().Trim();
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
                    txtsurf_temp.Focus();
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,b.weight from ivoucher a,item i,somas b where trim(a.icode)=trim(i.icode) and upper(trim(a.finvno))=trim(b.type)||'/'||trim(b.ordno)||' DT.'||to_char(b.orddt,'dd/mm/yyyy') and trim(a.icode)=trim(b.icode) and trim(a.invno)=trim(b.org_invno) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtgrade.Text.Trim().Substring(2, 2) + "/" + txtgrade.Text.Trim().Substring(4, 6) + " DT." + txtgrade.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                    //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtgrade.Text.Trim().Substring(2, 2) + "/" + txtgrade.Text.Trim().Substring(4, 6) + " DT." + txtgrade.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                    SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and /*a.branchcd='" + frm_mbr + "' and*/ a.type='15' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                    mq0 = "select distinct weight from somas where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + txtgrade.Text + "' and icode='" + txtIcode.Text.Trim() + "' and org_invno='" + txtlbl4.Text.Trim() + "' and cdrgno='" + txtwoline.Text.Trim() + "'";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    if (col1.Length <= 0) return;
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in Hidden Field 
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[d]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["finvno"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[9].Text = "-";
                        if (dt2.Rows.Count > 0)
                        {
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = dt2.Rows[0]["weight"].ToString().Trim();
                        }
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = dt.Rows[d]["Tag_no"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = dt.Rows[d]["INAME"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t5")).Text = "NRI";
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t6")).Text = "ACCEPTED";
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t7")).Text = "OK";
                    }
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,b.weight from ivoucher a,item i,somas b where trim(a.icode)=trim(i.icode) and upper(trim(a.finvno))=trim(b.type)||'/'||trim(b.ordno)||' DT.'||to_char(b.orddt,'dd/mm/yyyy') and trim(a.icode)=trim(b.icode) and trim(a.invno)=trim(b.org_invno) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtgrade.Text.Trim().Substring(2, 2) + "/" + txtgrade.Text.Trim().Substring(4, 6) + " DT." + txtgrade.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                        //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtgrade.Text.Trim().Substring(2, 2) + "/" + txtgrade.Text.Trim().Substring(4, 6) + " DT." + txtgrade.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                        SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and /*a.branchcd='" + frm_mbr + "' and*/ a.type='15' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') IN (" + col1.Trim() + ") order by Tag_no";
                        mq0 = "select distinct weight from somas where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + txtgrade.Text + "' and icode='" + txtIcode.Text.Trim() + "' and org_invno='" + txtlbl4.Text.Trim() + "' and cdrgno='" + txtwoline.Text.Trim() + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
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
                            if (dt2.Rows.Count > 0)
                            {
                                sg1_dr["sg1_t1"] = dt2.Rows[0]["weight"].ToString().Trim();
                            }
                            sg1_dr["sg1_t2"] = dt.Rows[d]["Tag_no"].ToString().Trim();
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_t5"] = "NRI";
                            sg1_dr["sg1_t6"] = "ACCEPTED";
                            sg1_dr["sg1_t7"] = "OK";
                            sg1_dr["sg1_t8"] = "";
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        for (i = 0; i < sg1.Rows.Count; i++)
                        {
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
        frm_vty = "85";
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
                            //for (i = 0; i < sg1.Rows.Count - 1; i++)
                            //{
                            //    if (sg1.Rows[i].Cells[14].Text.Trim().Length > 1)
                            //    {
                            save_it = "Y";
                            //    }
                            //}

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
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); DDClear();
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
                }
            }
            //sg1.HeaderRow.Cells[18].Style["display"] = "none";
            //e.Row.Cells[18].Style["display"] = "none";
            //sg1.HeaderRow.Cells[19].Style["display"] = "none";
            //e.Row.Cells[19].Style["display"] = "none";
            sg1.Columns[10].HeaderStyle.Width = 30;
            sg1.Columns[11].HeaderStyle.Width = 30;
            sg1.Columns[12].HeaderStyle.Width = 50;
            sg1.Columns[13].HeaderStyle.Width = 0;
            sg1.Columns[14].HeaderStyle.Width = 80;
            sg1.Columns[15].HeaderStyle.Width = 0;
            sg1.Columns[16].HeaderStyle.Width = 0;
            sg1.Columns[17].HeaderStyle.Width = 0;
            sg1.Columns[18].HeaderStyle.Width = 0;
            sg1.Columns[19].HeaderStyle.Width = 0;
            sg1.Columns[20].HeaderStyle.Width = 300;
            sg1.Columns[21].HeaderStyle.Width = 200;
            sg1.Columns[22].HeaderStyle.Width = 200;
            sg1.Columns[23].HeaderStyle.Width = 200;
            sg1.Columns[24].HeaderStyle.Width = 200;
            sg1.Columns[25].HeaderStyle.Width = 200;
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
        hffield.Value = "WONO";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TYPE";
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
            //if (sg1.Rows[i].Cells[14].Text.Trim().Length > 1)
            //{
                //save data into the inspvch table of type=79
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;  //div 1
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum.Trim().ToUpper();
                oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
                oporow["SRNO"] = i + 1;
                oporow["cpartno"] = txtlbl4.Text.Trim().ToUpper();//wono
                oporow["title"] = txtpono.Text.Trim().ToUpper();//pono
                oporow["ICODE"] = txtIcode.Text.Trim();

                //oporow["MRRNUM"] = txtwoline.Text.Trim().ToUpper();//wo item line no
                // oporow["OBSV6"] = txtwoline.Text.Trim().ToUpper();//wo item line no
                oporow["OBSV6"] = txtwolno.Text.Trim().ToUpper();
                oporow["acode"] = txtacode.Text.Trim().ToUpper(); // customer code
                oporow["grade"] = txtgrade.Text.Trim().ToUpper();
                //div 2
                oporow["DOC_DT"] = txttstDate.Text.Trim().ToUpper();//testing date
                oporow["matl"] = txtsurf_temp.Text.Trim().ToUpper();//surf Temp
                oporow["WONO"] = ddsurfprep.SelectedItem.Text.Trim().ToUpper();//dd surface prep
                oporow["obsv2"] = ddstgtest.SelectedItem.Text.Trim().ToUpper(); //dd test stage
                oporow["OBSV15"] = txtproject.Text.Trim().ToUpper();// project
                oporow["omax"] = ddmagparti.SelectedItem.Text.Trim().ToUpper();//dd type of mag particles
                //div 3
                oporow["col3"] = ddmagntech.SelectedItem.Text.Trim().ToUpper(); // dd magn technique
                oporow["col4"] = ddmthodpoder.SelectedItem.Text.Trim().ToUpper();// DD method of powder
                oporow["OBSV22"] = ddsurfprep2.SelectedItem.Text.Trim().ToUpper();//dd surface preparation
                oporow["OBSV21"] = ddmagcheck.SelectedItem.Text.Trim().ToUpper();//dd  mag adequacy check
                oporow["OBSV23"] = txtprocref.Text.Trim().ToUpper();// procedure ref
                oporow["col2"] = txtmatlspec.Text.Trim().ToUpper();//material specfication
                oporow["omin"] = ddmagntype.SelectedItem.Text.Trim().ToUpper();//dd magnetic current type
                //div 4
                oporow["OBSV16"] = ddacptstand.SelectedItem.Text.Trim().ToUpper();//dd acceptance standard
                oporow["OBSV17"] = txtItem.Text.Trim().ToUpper(); // dd item
                oporow["OBSV18"] = dddemag.SelectedItem.Text.Trim().ToUpper();// dd demagnetization
                oporow["OBSV19"] = ddpostclean.SelectedItem.Text.Trim().ToUpper(); // dd post cleaning
                oporow["col6"] = txtlightequip.Text.Trim().ToUpper(); //lighting equipment
                oporow["OBSV20"] = txtjbthick.Text.Trim().ToUpper(); // job thicknes
                oporow["col5"] = ddlift_pwer.SelectedItem.Text.Trim().ToUpper(); //  dd lifting power of yoke
                //div 5
                oporow["LINKFILE"] = txtfootnote.Text.Trim().ToUpper(); // footer notes

                //Grid
                oporow["BTCHNO"] = txtwoline.Text.Trim().ToUpper();//wo item line no
                oporow["obsv7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper(); // po.so.no
                oporow["obsv1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper(); // identification tag no
                oporow["obsv8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper(); // identification heat no
                oporow["col1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper(); // job description
                oporow["obsv3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper(); // record of indication
                oporow["obsv4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper(); // interpretation
                oporow["obsv5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper(); // remarks               
                oporow["DTR1"] = sg1.Rows[i].Cells[14].Text.Trim().ToUpper();
                oporow["CUSTREF"] = sg1.Rows[i].Cells[18].Text.Trim().ToUpper();
                oporow["OBSV24"] = txtPoLine.Text.Trim().ToUpper();
                oporow["FOOTNOTE"] = txtValve_Size_Rating.Text.Trim().ToUpper();
                oporow["OBSV26"] = txtComponent.Text.Trim().ToUpper();
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
                    oporow["edt_dt"] = vardate;
                }
                oDS.Tables[0].Rows.Add(oporow);
           // }
        }
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "79");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------
    protected void txt_TextChanged(object sender, EventArgs e)
    {
    }
    //------------------------------------------------------------------------------------
    protected void btnProdReport_Click(object sender, EventArgs e)
    {
        hffield.Value = "ProdRep";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    public void DDClear()
    {
        ddacptstand.Items.Clear();
        ddmthodpoder.Items.Clear();
        ddmagntech.Items.Clear();
        ddsurfprep.Items.Clear();
        ddstgtest.Items.Clear();
        ddsurfprep2.Items.Clear();
        ddmagcheck.Items.Clear();
        dddemag.Items.Clear();
        ddpostclean.Items.Clear();
        ddmagparti.Items.Clear();
        ddlift_pwer.Items.Clear();
        ddmagntype.Items.Clear();
    }
    //------------------------------------------------------------------------------------
    public void DDBind()
    {
        DDClear();
        ddsurfprep.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        ddsurfprep.Items.Add(new System.Web.UI.WebControls.ListItem("AS CAST", "AS CAST"));
        ddsurfprep.Items.Add(new System.Web.UI.WebControls.ListItem("AS MACHINED", "AS MACHINED"));

        ddacptstand.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        ddacptstand.Items.Add(new System.Web.UI.WebControls.ListItem("ASME B16.34 APDX II", "ASME B16.34 APDX II"));
        ddacptstand.Items.Add(new System.Web.UI.WebControls.ListItem("ASME SEC 8 DIV.I APPENDIX VI", "ASME SEC 8 DIV.I APPENDIX VI"));

        ddmthodpoder.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        ddmthodpoder.Items.Add(new System.Web.UI.WebControls.ListItem("DRY POWDER METHOD", "DRY POWDER METHOD"));
        ddmthodpoder.Items.Add(new System.Web.UI.WebControls.ListItem("LIQUID METHOD", "LIQUID METHOD"));

        ddmagntech.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        ddmagntech.Items.Add(new System.Web.UI.WebControls.ListItem("ELECTROMAGNETIC YOKE", "ELECTROMAGNETIC YOKE"));
        ddmagntech.Items.Add(new System.Web.UI.WebControls.ListItem("PRODE", "PRODE"));

        ddstgtest.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        ddstgtest.Items.Add(new System.Web.UI.WebControls.ListItem("AS CAST", "AS CAST"));
        ddstgtest.Items.Add(new System.Web.UI.WebControls.ListItem("AFTER MACHINING", "AFTER MACHINING"));
        ddstgtest.Items.Add(new System.Web.UI.WebControls.ListItem("AFTER OVERLAY", "AFTER OVERLAY"));
        ddstgtest.Items.Add(new System.Web.UI.WebControls.ListItem("AS WELD", "AS WELD"));

        ddsurfprep2.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        ddsurfprep2.Items.Add(new System.Web.UI.WebControls.ListItem("AS CAST", "AS CAST"));
        ddsurfprep2.Items.Add(new System.Web.UI.WebControls.ListItem("AS MACHINED", "AS MACHINED"));
        ddsurfprep2.Items.Add(new System.Web.UI.WebControls.ListItem("AS WELDED", "AS WELDED"));

        ddmagcheck.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        ddmagcheck.Items.Add(new System.Web.UI.WebControls.ListItem("PIE SHAPED INDICATOR", "PIE SHAPED INDICATOR"));
        ddmagcheck.Items.Add(new System.Web.UI.WebControls.ListItem("ARTIFICIAL FLAW SHIMS", "ARTIFICIAL FLAW SHIMS"));

        dddemag.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        dddemag.Items.Add(new System.Web.UI.WebControls.ListItem("REQUIRED", "REQUIRED"));
        dddemag.Items.Add(new System.Web.UI.WebControls.ListItem("NOT REQUIRED", "NOT REQUIRED"));

        ddpostclean.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        ddpostclean.Items.Add(new System.Web.UI.WebControls.ListItem("DONE", "DONE"));
        ddpostclean.Items.Add(new System.Web.UI.WebControls.ListItem("NOT DONE", "NOT DONE"));

        ddmagparti.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        ddmagparti.Items.Add(new System.Web.UI.WebControls.ListItem("NON-FLUORESCENT", "NON-FLUORESCENT"));
        ddmagparti.Items.Add(new System.Web.UI.WebControls.ListItem("FLUORESCENT WET", "FLUORESCENT WET"));
        ddmagparti.Items.Add(new System.Web.UI.WebControls.ListItem("FLUORESCENT DRY", "FLUORESCENT DRY"));

        ddlift_pwer.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        ddlift_pwer.Items.Add(new System.Web.UI.WebControls.ListItem("4.5 KG IN A.C MODE", "4.5 KG IN A.C MODE"));
        ddlift_pwer.Items.Add(new System.Web.UI.WebControls.ListItem("18.1 KG IN H.W.D.C. MODE", "18.1 KG IN H.W.D.C. MODE"));

        ddmagntype.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        ddmagntype.Items.Add(new System.Web.UI.WebControls.ListItem("A.C", "A.C"));
        ddmagntype.Items.Add(new System.Web.UI.WebControls.ListItem("H.W.D.C.", "H.W.D.C."));
    }
    //------------------------------------------------------------------------------------
    protected void FillGrid()
    {
        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
    }
    //------------------------------------------------------------------------------------
}