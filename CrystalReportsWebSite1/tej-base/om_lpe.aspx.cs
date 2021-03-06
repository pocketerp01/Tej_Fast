using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_lpe : System.Web.UI.Page
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
        lblheader.Text = "Liquid Penetrant Examination";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "81");
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
                SQuery = "Select Distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.org_invno)||trim(a.work_ordno)||trim(a.icode)||trim(a.cdrgno) as fstr,a.branchcd,b.aname as Customer,a.Pordno,a.org_invno as WO_NO,a.acode,a.work_ordno as project,a.icode,i.iname,a.cdrgno as so_line_no,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.ordno,to_char(a.orddt,'yyyymmdd') as vdd from Somas a,famst b,item i where trim(a.acodE)=trim(b.acodE) and trim(a.icode)=trim(i.icode) and a.branchcd!='DD' and substr(a.type,1,1)='4' and a.type!='44' and length(trim(nvl(a.app_by,'-')))> 1 and length(trim(nvl(a.org_invno,'-')))> 1 order by vdd desc,a.ordno desc";
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
                //REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') USED FOR REPLACING ENTER KEY EFFECT
                //SQuery = "Select trim(icode)||trim(invno)||REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') as fstr,invno as Wo_No,desc_ as Tag_no,IQTYIN AS Qty,to_char(vchdate,'dd/mm/yyyy') as Entry_Date,Ent_by,icode,finvno from ivoucher where branchcd='" + frm_mbr + "' and type='15' and invno='" + txtlbl4.Text.Trim() + "' and icode='" + txtIcode.Text + "' and upper(finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') NOT IN (" + col1 + ") order by desc_";
                //SQuery = "Select trim(icode)||trim(invno)||REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') as fstr,invno as Wo_No,desc_ as Tag_no,IQTYIN AS Qty,to_char(vchdate,'dd/mm/yyyy') as Entry_Date,Ent_by,icode,finvno from ivoucherp where branchcd='" + frm_mbr + "' and type='15' and invno='" + txtlbl4.Text.Trim() + "' and upper(finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') NOT IN (" + col1 + ") order by desc_";
                SQuery = "Select trim(icode)||trim(invno)||REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') as fstr,invno as Wo_No,desc_ as Tag_no,IQTYIN AS Qty,to_char(vchdate,'dd/mm/yyyy') as Entry_Date,Ent_by,icode,finvno from ivoucherp where /*branchcd='" + frm_mbr + "' and*/ type='15' and invno='" + txtlbl4.Text.Trim() + "' /*AND REPLACE(REPLACE(DESC_, CHR(13),''), CHR(10),'') NOT IN (" + col1 + ")*/ order by desc_";
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
        //if (txtWoLineNo.Text == "-")
        //{
        //    fgen.msg("-", "AMSG", "Please Fill WO Line Item No.");
        //    txtWoLineNo.Focus();
        //    return;
        //}
        if (txtPOLine.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill PO Line Item No.");
            txtPOLine.Focus();
            return;
        }
        if (txtlbl7.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Customer");
            txtlbl7.Focus();
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
        if (dd_Test_Stage.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Test Stage");
            dd_Test_Stage.Focus();
            return;
        }

        if (dd_Area_Tested.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Area Tested");
            txtPenetrate_Time.Focus();
            return;
        }

        if (txtItem.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Product");
            txtItem.Focus();
            return;
        }

        if (txtMaterial.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Material");
            txtMaterial.Focus();
            return;
        }

        if (txtTest_Temp.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Test Temprature");
            txtTest_Temp.Focus();
            return;
        }

        if (txtPenetrant_Brand.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Penetrant Brand");
            txtPenetrant_Brand.Focus();
            return;
        }

        if (txtPenetrant_Batch.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Penetrant Batch");
            txtPenetrant_Batch.Focus();
            return;
        }

        if (txtLighting_Equip.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Lighting Equip");
            txtLighting_Equip.Focus();
            return;
        }

        if (txtComp_Thick.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Component Thickness");
            txtComp_Thick.Focus();
            return;
        }

        if (txtWeld_Details.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Weld Details");
            txtWeld_Details.Focus();
            return;
        }

        if (txtPenetrant_Type.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Type Of Penetrant");
            txtPenetrant_Type.Focus();
            return;
        }

        if (txtTechnique.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Type Of Technique");
            txtTechnique.Focus();
            return;
        }

        if (txtValve_Size_Rating.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Valve Size & Rating");
            txtValve_Size_Rating.Focus();
            return;
        }

        if (txtSol_Brand.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Solvent Brand");
            txtSol_Brand.Focus();
            return;
        }

        if (txtSol_Batch_No.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Solvent Batch No");
            txtSol_Batch_No.Focus();
            return;
        }

        if (txtDev_Brand.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Developer_Brand");
            txtDev_Brand.Focus();
            return;
        }

        if (txtDev_Batch.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Developer Batch");
            txtDev_Batch.Focus();
            return;
        }

        if (txtProcedure_Ref.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Procedure Ref");
            txtProcedure_Ref.Focus();
            return;
        }

        if (txtAcceptance_Std.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Acceptance Std");
            txtAcceptance_Std.Focus();
            return;
        }

        if (txtDev_Type.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Type Of Developer");
            txtDev_Type.Focus();
            return;
        }

        if (txtTest_Dt.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Testing Date");
            txtTest_Dt.Focus();
            return;
        }

        if (txtPenetrate_Time.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Penetrate Time");
            txtPenetrate_Time.Focus();
            return;
        }

        if (txtDev_Time.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Development Time");
            txtDev_Time.Focus();
            return;
        }

        if (txtComponent.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill Component");
            txtComponent.Focus();
            return;
        }

        if (dd_Surface.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Surface Cleaned With Cleaner And Wiped Off With Lint Free Cloth '13' (Tab 2)");
            dd_Surface.Focus();
            return;
        }

        if (dd_Dried.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Surface Dried & Cleaner Evaporated '13' (Tab 2)");
            dd_Dried.Focus();
            return;
        }

        if (dd_Pene_App.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Penetrant Application '13' (Tab 2)");
            dd_Pene_App.Focus();
            return;
        }

        if (dd_Excess.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Excess Penetrant Removal By Wiping The Surface First With Dry, Clean, White & Lint Free Cloth Then With Moistened Cloth '13' (Tab 2)");
            dd_Excess.Focus();
            return;
        }

        if (dd_Drying.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Drying Time Interval Between Penetrant Removal And Developer Application Within 7 Minutes '13' (Tab 2)");
            dd_Drying.Focus();
            return;
        }

        if (dd_Dev_App.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Developer Application '13' (Tab 2)");
            dd_Dev_App.Focus();
            return;
        }

        if (txtEvaluation.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Fill  Evaluation Time (7 Minutes To 60 Minutes) '13' (Tab 2)");
            txtEvaluation.Focus();
            return;
        }

        if (dd_Post.SelectedItem.Text == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Please Select Post Cleaning '13' (Tab 2)");
            dd_Post.Focus();
            return;
        }

        //if (txtrmk.Text == "-")
        //{
        //    fgen.msg("-", "AMSG", "Please Fill Remarks");
        //    return;
        //}

        if (sg1.Rows.Count <= 1)
        {
            fgen.msg("-", "AMSG", "Please Select Tag");
            return;
        }

        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            //if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() == "-")
            //{
            //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill PO SL No. At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
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

            //if (((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim() == "-")
            //{
            //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Job Description At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
            //    return;
            //}

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim() == "-")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Size Of Indication At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
                return;
            }

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim() == "-")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Interpretation At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
                return;
            }

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim() == "-")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Grid Remarks At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
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
                        txtIcode.Text = dt.Rows[0]["ICODE"].ToString().Trim();
                        txtlbl4.Text = dt.Rows[0]["CPARTNO"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["BTCHNO"].ToString().Trim();
                        txtWoLineNo.Text = dt.Rows[0]["obsv8"].ToString().Trim();
                        txtOrder.Text = dt.Rows[0]["GRADE"].ToString().Trim();
                        txtMaterial.Text = dt.Rows[0]["COL2"].ToString().Trim();
                        txtPenetrant_Brand.Text = dt.Rows[0]["COL3"].ToString().Trim();
                        txtPenetrant_Batch.Text = dt.Rows[0]["COL4"].ToString().Trim();
                        txtDev_Brand.Text = dt.Rows[0]["COL5"].ToString().Trim();
                        txtDev_Batch.Text = dt.Rows[0]["COL6"].ToString().Trim();
                        dd_Test_Stage.SelectedItem.Text = dt.Rows[0]["OBSV2"].ToString().Trim();
                        txtProject.Text = dt.Rows[0]["OBSV9"].ToString().Trim();
                        txtComp_Thick.Text = dt.Rows[0]["OBSV10"].ToString().Trim();
                        txtProcedure_Ref.Text = dt.Rows[0]["OBSV11"].ToString().Trim();
                        dd_Area_Tested.SelectedItem.Text = dt.Rows[0]["OBSV12"].ToString().Trim();
                        txtWeld_Details.Text = dt.Rows[0]["OBSV13"].ToString().Trim();
                        txtPenetrant_Type.Text = dt.Rows[0]["OBSV14"].ToString().Trim();
                        txtDev_Type.Text = dt.Rows[0]["OBSV15"].ToString().Trim();
                        txtTest_Temp.Text = dt.Rows[0]["WONO"].ToString().Trim();
                        txtLighting_Equip.Text = dt.Rows[0]["MATL"].ToString().Trim();
                        txtSol_Brand.Text = dt.Rows[0]["OMAX"].ToString().Trim();
                        txtSol_Batch_No.Text = dt.Rows[0]["OMIN"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["LINKFILE"].ToString().Trim();
                        txtTechnique.Text = dt.Rows[0]["MFGDATE"].ToString().Trim();
                        txtPenetrate_Time.Text = dt.Rows[0]["OBSV16"].ToString().Trim();
                        txtDev_Time.Text = dt.Rows[0]["OBSV17"].ToString().Trim();
                        txtAcceptance_Std.Text = dt.Rows[0]["OBSV19"].ToString().Trim();
                        txtItem.Text = dt.Rows[0]["OBSV20"].ToString().Trim();
                        dd_Surface.SelectedItem.Text = dt.Rows[0]["OBSV21"].ToString().Trim();
                        dd_Dried.SelectedItem.Text = dt.Rows[0]["OBSV22"].ToString().Trim();
                        dd_Pene_App.SelectedItem.Text = dt.Rows[0]["OBSV23"].ToString().Trim();
                        dd_Excess.SelectedItem.Text = dt.Rows[0]["OBSV24"].ToString().Trim();
                        dd_Drying.SelectedItem.Text = dt.Rows[0]["OBSV25"].ToString().Trim();
                        dd_Dev_App.SelectedItem.Text = dt.Rows[0]["OBSV26"].ToString().Trim();
                        txtEvaluation.Text = dt.Rows[0]["OBSV27"].ToString().Trim();
                        dd_Post.SelectedItem.Text = dt.Rows[0]["OBSV28"].ToString().Trim();
                        txtTest_Dt.Text = Convert.ToDateTime(dt.Rows[0]["DOC_DT"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtPOLine.Text = dt.Rows[0]["OBSV7"].ToString().Trim();
                        txtValve_Size_Rating.Text = dt.Rows[0]["FOOTNOTE"].ToString().Trim();
                        txtComponent.Text = dt.Rows[0]["OBSV30"].ToString().Trim();
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
                            //sg1_dr["sg1_t1"] = dt.Rows[i]["OBSV7"].ToString().Trim();
                            sg1_dr["sg1_t1"] = "-";
                            sg1_dr["sg1_t2"] = dt.Rows[i]["OBSV1"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["OBSV6"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["COL1"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["OBSV3"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["OBSV4"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["OBSV5"].ToString().Trim();
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
                        mq0 = "select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,SRNO FROM SCRATCH WHERE BRANCHCD='" + frm_mbr + "' AND TYPE = 'WO' AND COL26='" + dt.Rows[0]["CDRGNO"].ToString().Trim() + "' AND COL27='" + dt.Rows[0]["WO_NO"].ToString().Trim() + "' AND UPPER(COL28)='" + dt.Rows[0]["TYPE"].ToString().Trim() + "/" + dt.Rows[0]["ORDNO"].ToString().Trim() + " DT." + dt.Rows[0]["ORDDT"].ToString().Trim() + "' AND ICODE='" + dt.Rows[0]["ICODE"].ToString().Trim() + "'";
                        mq0 = "select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,SRNO,COL3 AS RATING,COL2 AS SIZE_MM FROM SCRATCH WHERE BRANCHCD='" + dt.Rows[0]["BRANCHCD"].ToString().Trim() + "' AND TYPE = 'WO' AND COL26='" + dt.Rows[0]["CDRGNO"].ToString().Trim() + "' AND COL27='" + dt.Rows[0]["WO_NO"].ToString().Trim() + "' AND UPPER(COL28)='" + dt.Rows[0]["TYPE"].ToString().Trim() + "/" + dt.Rows[0]["ORDNO"].ToString().Trim() + " DT." + dt.Rows[0]["ORDDT"].ToString().Trim() + "' AND ICODE='" + dt.Rows[0]["ICODE"].ToString().Trim() + "'";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        if (dt2.Rows.Count > 0)
                        {
                            txtWoLineNo.Text = dt2.Rows[0]["srno"].ToString().Trim();
                            txtValve_Size_Rating.Text = dt2.Rows[0]["SIZE_MM"].ToString().Trim() + "-" + dt2.Rows[0]["RATING"].ToString().Trim();
                        }
                        txtlbl4.Text = dt.Rows[0]["wo_no"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["cdrgno"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["customer"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[0]["Pordno"].ToString().Trim();
                        txtProject.Text = dt.Rows[0]["work_ordno"].ToString().Trim();
                        txtOrder.Text = dt.Rows[0]["fstr"].ToString().Trim();
                        txtIcode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtPOLine.Text = dt.Rows[0]["weight"].ToString().Trim();
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
                    dd_Test_Stage.Focus();
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,b.weight from ivoucher a,item i,somas b where trim(a.icode)=trim(i.icode) and upper(trim(a.finvno))=trim(b.type)||'/'||trim(b.ordno)||' DT.'||to_char(b.orddt,'dd/mm/yyyy') and trim(a.icode)=trim(b.icode) and trim(a.invno)=trim(b.org_invno) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                    //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,'-' as weight from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                    SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,'-' as weight from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and /*a.branchcd='" + frm_mbr + "' and*/ a.type='15' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    //mq0 = "select distinct weight from somas where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + txtOrder.Text.Trim() + "' and icode='" + txtIcode.Text.Trim() + "' and org_invno='" + txtlbl4.Text.Trim() + "' and cdrgno='" + txtlbl4a.Text.Trim() + "'";
                    //dt2 = new DataTable();
                    //dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in Hidden Field 
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[d]["ICODE"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["finvno"].ToString().Trim();
                        //if (dt2.Rows.Count > 0)
                        //{
                        //    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = dt2.Rows[0]["weight"].ToString().Trim();
                        //}
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = dt.Rows[d]["TAG_NO"].ToString().Trim();
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        //SQuery = "Select a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='15' and trim(a.icode)||trim(a.invno)||trim(a.desc_) ='" + col1.Trim() + "'  order by Tag_no";
                        //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,b.weight from ivoucher a,item i,somas b where trim(a.icode)=trim(i.icode) and upper(trim(a.finvno))=trim(b.type)||'/'||trim(b.ordno)||' DT.'||to_char(b.orddt,'dd/mm/yyyy') and trim(a.icode)=trim(b.icode) and trim(a.invno)=trim(b.org_invno) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                        //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,'-' as weight from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                        SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,'-' as weight from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and /*a.branchcd='" + frm_mbr + "' and*/ a.type='15' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') IN (" + col1.Trim() + ") order by Tag_no";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        //mq0 = "select distinct weight from somas where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + txtOrder.Text.Trim() + "' and icode='" + txtIcode.Text.Trim() + "' and org_invno='" + txtlbl4.Text.Trim() + "' and cdrgno='" + txtlbl4a.Text.Trim() + "'";
                        //dt2 = new DataTable();
                        //dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0); // PO SL NO IS GIVEN ON TOP INSTEAD OF GRID ON 12 SEPT 2019
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
                            //if (dt2.Rows.Count > 0)
                            //{
                            //    sg1_dr["sg1_t1"] = dt2.Rows[0]["weight"].ToString().Trim();
                            //}
                            sg1_dr["sg1_t1"] = "-";
                            sg1_dr["sg1_t2"] = dt.Rows[d]["tag_no"].ToString().Trim();
                            sg1_dr["sg1_t3"] = "-";
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
                            for (i = 0; i < sg1.Rows.Count - 0; i++)
                            {
                                if (sg1.Rows[i].Cells[14].Text.Trim().Length > 1)
                                {
                                    save_it = "Y";
                                }
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
            //sg1.Columns[18].HeaderStyle.Width = 200;
            //sg1.Columns[19].HeaderStyle.Width = 150;
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
            if (sg1.Rows[i].Cells[14].Text.Trim().Length > 1)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum.Trim().ToUpper();
                oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
                oporow["SRNO"] = i + 1;
                oporow["TITLE"] = txtlbl2.Text.Trim().ToUpper();
                oporow["BTCHNO"] = txtlbl4a.Text.Trim().ToUpper();
                oporow["ACODE"] = txtlbl7.Text.Trim().ToUpper();
                oporow["ICODE"] = txtIcode.Text.Trim().ToUpper();
                oporow["CPARTNO"] = txtlbl4.Text.Trim().ToUpper();
                oporow["GRADE"] = txtOrder.Text.Trim().ToUpper();
                oporow["COL1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper();
                oporow["COL2"] = txtMaterial.Text.Trim().ToUpper();
                oporow["COL3"] = txtPenetrant_Brand.Text.Trim().ToUpper();
                oporow["COL4"] = txtPenetrant_Batch.Text.Trim().ToUpper();
                oporow["COL5"] = txtDev_Brand.Text.Trim().ToUpper();
                oporow["COL6"] = txtDev_Batch.Text.Trim().ToUpper();
                oporow["MRRNUM"] = "-";
                oporow["MRRDATE"] = "-";
                oporow["BTCHDT"] = "-";
                oporow["RESULT"] = "-";
                oporow["OBSV1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper();
                oporow["OBSV2"] = dd_Test_Stage.SelectedItem.Text.Trim().ToUpper();
                oporow["OBSV3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper();
                oporow["OBSV4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper();
                oporow["OBSV5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper();
                oporow["OBSV6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper();
                //oporow["OBSV7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
                oporow["OBSV7"] = txtPOLine.Text.Trim().ToUpper();
                oporow["OBSV8"] = txtWoLineNo.Text.Trim().ToUpper();
                oporow["OBSV9"] = txtProject.Text.Trim().ToUpper();
                oporow["OBSV10"] = txtComp_Thick.Text.Trim().ToUpper();
                oporow["OBSV11"] = txtProcedure_Ref.Text.Trim().ToUpper();
                oporow["OBSV12"] = dd_Area_Tested.SelectedItem.Text.Trim().ToUpper();
                oporow["OBSV13"] = txtWeld_Details.Text.Trim().ToUpper();
                oporow["OBSV14"] = txtPenetrant_Type.Text.Trim().ToUpper();
                oporow["OBSV15"] = txtDev_Type.Text.Trim().ToUpper();
                oporow["CONTPLAN"] = "-";
                oporow["SAMPQTY"] = 0;
                oporow["WONO"] = txtTest_Temp.Text.Trim().ToUpper();
                oporow["MATL"] = txtLighting_Equip.Text.Trim().ToUpper();
                oporow["FINISH"] = "-";
                oporow["OMAX"] = txtSol_Brand.Text.Trim().ToUpper();
                oporow["OMIN"] = txtSol_Batch_No.Text.Trim().ToUpper();
                if (txtrmk.Text.Trim().Length > 200)
                {
                    oporow["LINKFILE"] = txtrmk.Text.Trim().ToUpper().Substring(0, 199);
                }
                else
                {
                    oporow["LINKFILE"] = txtrmk.Text.Trim().ToUpper();
                }
                oporow["MFGDATE"] = txtTechnique.Text.Trim().ToUpper();
                oporow["EXPDATE"] = "-";
                oporow["OBSV16"] = txtPenetrate_Time.Text.Trim().ToUpper();
                oporow["OBSV17"] = txtDev_Time.Text.Trim().ToUpper();
                oporow["OBSV18"] = "-";
                oporow["OBSV19"] = txtAcceptance_Std.Text.Trim().ToUpper();
                oporow["OBSV20"] = txtItem.Text.Trim().ToUpper();
                oporow["OBSV21"] = dd_Surface.SelectedItem.Text.Trim().ToUpper();
                oporow["OBSV22"] = dd_Dried.SelectedItem.Text.Trim().ToUpper();
                oporow["OBSV23"] = dd_Pene_App.SelectedItem.Text.Trim().ToUpper();
                oporow["OBSV24"] = dd_Excess.SelectedItem.Text.Trim().ToUpper();
                oporow["OBSV25"] = dd_Drying.SelectedItem.Text.Trim().ToUpper();
                oporow["OBSV26"] = dd_Dev_App.SelectedItem.Text.Trim().ToUpper();
                oporow["OBSV27"] = txtEvaluation.Text.Trim().ToUpper();
                oporow["FIGURE_NO"] = "-";
                oporow["CUSTREF"] = sg1.Rows[i].Cells[18].Text.Trim().ToUpper();
                oporow["OBSV28"] = dd_Post.SelectedItem.Text.Trim().ToUpper();
                oporow["OBSV29"] = "-";
                oporow["APP_BY"] = "-";
                oporow["APP_DT"] = vardate;
                oporow["OBSV30"] = txtComponent.Text.Trim().ToUpper();
                oporow["OBSV31"] = "-";
                oporow["REJQTY"] = 0;
                oporow["DOC_DT"] = txtTest_Dt.Text.Trim().ToUpper();
                oporow["NUM1"] = 0;
                oporow["NUM2"] = 0;
                oporow["DTR1"] = sg1.Rows[i].Cells[14].Text.Trim().ToUpper();
                oporow["DTT1"] = 0;
                oporow["EQUIP_ID"] = "";
                oporow["FOOTNOTE"] = txtValve_Size_Rating.Text.Trim().ToUpper();
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
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "81");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------
    protected void FillDropDown()
    {
        SQuery = "SELECT 'PLEASE SELECT' AS FSTR FROM DUAL UNION ALL SELECT 'CASTED' AS FSTR FROM DUAL UNION ALL SELECT 'MACHINED' AS FSTR FROM DUAL UNION ALL SELECT 'WELDED' AS FSTR FROM DUAL";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        dd_Area_Tested.DataSource = dt;
        dd_Area_Tested.DataTextField = "fstr";
        dd_Area_Tested.DataValueField = "fstr";
        dd_Area_Tested.DataBind();

        SQuery = "SELECT 'PLEASE SELECT' AS FSTR FROM DUAL UNION ALL SELECT 'AS CAST' AS FSTR FROM DUAL UNION ALL SELECT 'AFTER MACHINING' AS FSTR FROM DUAL UNION ALL SELECT 'AFTER OVERLAY' AS FSTR FROM DUAL UNION ALL SELECT 'AS WELD' AS FSTR FROM DUAL";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        dd_Test_Stage.DataSource = dt;
        dd_Test_Stage.DataTextField = "fstr";
        dd_Test_Stage.DataValueField = "fstr";
        dd_Test_Stage.DataBind();

        txtTechnique.Text = "COLOUR CONTRAST PENETRANT TECHNIQUE";
        txtPenetrant_Type.Text = "SOLVENT REMOVABLE";
        txtDev_Type.Text = "WET NON AQUEOUS";
        txtProcedure_Ref.Text = "QAD-W-07 REV.00 ISSUE-02";
        txtAcceptance_Std.Text = "ASME SEC.VIII DIV-1 APP.-8,PARA 8.4/ASME B16.34/CLIENT SPEC";
        txtPenetrant_Brand.Text = "MAGNAFLUX";
        txtSol_Brand.Text = "MAGNAFLUX";
        txtDev_Brand.Text = "MAGNAFLUX";
        txtLighting_Equip.Text = "100 WATT BULB AT 250 MM DISTANCE";

        SQuery = "SELECT 'YES' AS FSTR FROM DUAL UNION ALL SELECT 'NO' AS FSTR FROM DUAL";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        dd_Surface.DataSource = dt;
        dd_Surface.DataTextField = "fstr";
        dd_Surface.DataValueField = "fstr";
        dd_Surface.DataBind();

        dd_Dried.DataSource = dt;
        dd_Dried.DataTextField = "fstr";
        dd_Dried.DataValueField = "fstr";
        dd_Dried.DataBind();

        dd_Excess.DataSource = dt;
        dd_Excess.DataTextField = "fstr";
        dd_Excess.DataValueField = "fstr";
        dd_Excess.DataBind();

        dd_Drying.DataSource = dt;
        dd_Drying.DataTextField = "fstr";
        dd_Drying.DataValueField = "fstr";
        dd_Drying.DataBind();

        SQuery = "SELECT 'BY SPRAYING' AS FSTR FROM DUAL UNION ALL SELECT 'BY DIPPING' AS FSTR FROM DUAL";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        dd_Pene_App.DataSource = dt;
        dd_Pene_App.DataTextField = "fstr";
        dd_Pene_App.DataValueField = "fstr";
        dd_Pene_App.DataBind();

        dd_Dev_App.DataSource = dt;
        dd_Dev_App.DataTextField = "fstr";
        dd_Dev_App.DataValueField = "fstr";
        dd_Dev_App.DataBind();

        SQuery = "SELECT 'DONE' AS FSTR FROM DUAL UNION ALL SELECT 'NOT DONE' AS FSTR FROM DUAL";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        dd_Post.DataSource = dt;
        dd_Post.DataTextField = "fstr";
        dd_Post.DataValueField = "fstr";
        dd_Post.DataBind();
    }
    //------------------------------------------------------------------------------------
    protected void BlankDropDown()
    {
        dd_Area_Tested.Items.Clear();
        dd_Dev_App.Items.Clear();
        dd_Dried.Items.Clear();
        dd_Drying.Items.Clear();
        dd_Excess.Items.Clear();
        dd_Pene_App.Items.Clear();
        dd_Post.Items.Clear();
        dd_Surface.Items.Clear();
        dd_Test_Stage.Items.Clear();
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