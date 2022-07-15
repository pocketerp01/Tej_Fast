using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_mrr_entry : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, col4, col5, col6, col7, col8, col9, col10, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;
    string mq0 = "", prt_rav = "", chk_indust = "", ps_rt = "";
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;
    double db1 = 0, db2, db3, db4;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    fgenDB fgen = new fgenDB();
    double totamt = 0, tot_qty = 0;

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
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
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
                for (int i = 0; i < 8; i++)
                {
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[K].Cells[i].CssClass = "hidden";
                }
                #endregion
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

                ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Add("autocomplete", "off");
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
                    //sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
                    sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
                    sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                }
            }
        }

        txtlbl25.Attributes.Add("readonly", "readonly");
        txtlbl27.Attributes.Add("readonly", "readonly");
        txtlbl29.Attributes.Add("readonly", "readonly");
        txtlbl31.Attributes.Add("readonly", "readonly");

        // to hide and show to tab panel
        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();
        sg4_add_blankrows();
        btnlbl4.Enabled = false;
        btnlbl7.Enabled = false;
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
        sg4.DataSource = sg4_dt; sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();
        btnprint.Disabled = false; btnlist.Disabled = false; btnCal.Disabled = true;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlbl4.Enabled = true;
        btnlbl7.Enabled = true;
        btnprint.Disabled = true; btnlist.Disabled = true; btnCal.Disabled = false;
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
        doc_vty.Value = "LC";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = "LC"; frm_tabname = "scratch"; lbl1a.Text = frm_vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
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

            case "TACODE":
                SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, a.vchnum as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,b.aname as Supplier,a.acode as Supp_Code,a.type,a.invno,a.invdate from ivoucher a , famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " AND a.type like '" + frm_vty + "%' and a.store<>'R' and (Trim(a.acodE),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy')) not in (select trim(acode),trim(col9),trim(col10) from scratch where branchcd='" + frm_mbr + "' and type='LC' and vchdate " + DateRange + ") order by a.vchdate desc,a.vchnum desc";
                break;
            case "TICODE":
                SQuery = "SELECT distinct a.Acode as FStr,a.Aname as Customer,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.Staten from famst a where  length(trim(nvl(a.deac_by,'-'))) <2 and substr(a.acode,1,2) in ('02','05','06','14','15') order by a.Aname ";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
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
                SQuery = "select a.Fstr,max(a.Iname)as Item_Name,a.ERP_code,max(a.Cpartno)as Part_no,max(a.Irate) As Irate,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as PO_No,a.Fstr as SO_link,max(a.cdisc) as CDisc,max(a.iexc_Addl) as iexc_Addl,max(a.sd) as frt_pu,max(a.ipack) as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,pordno,(Case when length(trim(nvl(desc9,'-')))>1 then desc9 else ciname end) as Iname,trim(Icode) as ERP_code,Cpartno,Irate,Qtyord,0 as Soldqty,nvl(Cdisc,0) as Cdisc,nvl(iexc_addl,0) as Iexc_Addl,nvl(sd,0) as sd ,nvl(ipack,0) as ipack from somas where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "' and trim(icat)!='Y' and trim(app_by)!='-'  union all SELECT to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(Icode) as fstr,null as pordno,null as Iname,trim(Icode) as ERP_code,null as Cpartno,0 as Irate,0 as Qtyord,iqtyout as Soldqty,0 as Cdisc,0 as iexc_Addl,0 as sd,0 as ipack  from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "')a,item b where trim(a.erp_code)=trim(B.icode)  group by a.fstr,a.ERP_code,b.unit,b.hscode having (case when sum(a.Qtyord)>0 then sum(a.Qtyord)-sum(a.Soldqty) else max(a.irate) end)>0 order by Item_Name,a.fstr";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);
                break;

            case "New":
                Type_Sel_query();
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as lc_sheet_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Dated,a.type,a.col9 as mrr_no,a.col10 as mrr_Dt,col11 as Supplier,a.icode as erpcode,a.col1 as Invno,a.remarks,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " order by vdd desc,a." + doc_nf.Value + " desc";
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
            hffield.Value = "New";
            make_qry_4_popup();
            fgen.Fn_open_sseek("select MRR Type", frm_qstr);
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
            hffield.Value = "Edit_E";
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
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }
        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only"); txtvchdate.Focus(); return; }

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        if (sg1.Rows.Count > 0)
        {
            // REASON BEHIND CHECKING FOR ONLY ONE ROW IS IF USER ENTER TAX THEN IT WILL COME AUTOMATICALLY IN ALL ROWS. THEREFORE, NO NEED TO CHECK ON ALL ROWS
            if (fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t6")).Text) == 0 && fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t8")).Text) == 0)
            {
                fgen.msg("-", "SMSG", "You Have Not Filled Any Amount For Tax!!'13' Are You Sure, You Want To Save!!");
            }
            else
            {
                fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
            }
        }
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
        sg2_dt = new DataTable();
        sg3_dt = new DataTable();
        sg4_dt = new DataTable();

        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();

        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();

        sg2_add_blankrows();
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();

        sg3_add_blankrows();
        sg3.DataSource = sg3_dt;
        sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();

        sg4_add_blankrows();
        sg4.DataSource = sg4_dt;
        sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();

        ViewState["sg1"] = null;
        ViewState["sg2"] = null;
        ViewState["sg3"] = null;
        ViewState["sg4"] = null;
        Session["Filled"] = null;
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
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
            if (CP_BTN.Trim().Substring(0, 3) == "BTN" || CP_BTN.Trim().Substring(0, 3) == "SG1" || CP_BTN.Trim().Substring(0, 3) == "SG2" || CP_BTN.Trim().Substring(0, 3) == "SG3" || CP_BTN.Trim().Substring(0, 3) == "SG4")
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
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
                    #region
                    if (col1 == "") return;
                    doc_qty.Value = col1.Trim();
                    Multi_MRR.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select enable_yn from STOCK where id='M206'", "enable_yn");
                    if (Multi_MRR.Value == "Y")
                    {
                        hffield.Value = "TACODE_MULTIMRR";
                        SQuery = "SELECT distinct trim(a.Acode) as FStr,a.Aname as Account_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.ent_by,a.edt_by from famst a,ivoucher b where trim(a.acode)=trim(b.acode) and b.branchcd='" + frm_mbr + "' and b.type like '0%' order by Account_Name";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_sseek("Select Supplier", frm_qstr);
                    }
                    else
                    {
                        hffield.Value = "TACODE";
                        SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, a.vchnum as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,b.aname as Supplier,a.acode as Supp_Code,a.type,a.invno,a.invdate,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a , famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " AND a.type='" + col1 + "' and a.store<>'R' and (Trim(a.acodE),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy')) not in (select trim(acode),trim(col9),trim(col10) from scratch where branchcd='" + frm_mbr + "' and type='LC' and vchdate " + DateRange + ") order by vdd desc,mrr_no desc";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_sseek("Select MRR Entry", frm_qstr);
                    }
                    break;
                    #endregion

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
                    hffield.Value = btnval + "_E";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,c.Aname,c.gst_no,nvl(b.Iname,'-') As Iname,nvl(b.unit,'-') as Unit from " + frm_tabname + " a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        txtvchnum.Text = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl2.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        txtlbl3.Text = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl5.Text = dt.Rows[0]["eDt_by"].ToString().Trim();
                        txtlbl6.Text = Convert.ToDateTime(dt.Rows[0]["edt_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl7.Text = dt.Rows[0]["Acode"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["col11"].ToString().Trim();
                        txtlbl4.Text = dt.Rows[0]["COL9"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["COL10"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["naration"].ToString().Trim();
                        txtlbl8.Text = dt.Rows[0]["col27"].ToString().Trim();
                        doc_qty.Value = dt.Rows[0]["REMARKS"].ToString().Trim();
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
                            sg1_dr["sg1_h9"] = dt.Rows[i]["col18"].ToString().Trim();// MRR RATE
                            sg1_dr["sg1_h10"] = dt.Rows[i]["col26"].ToString().Trim();// LANDED COST PER UNIT
                            sg1_dr["sg1_f1"] = dt.Rows[i]["num7"].ToString().Trim();// FC RATE
                            sg1_dr["sg1_f2"] = dt.Rows[i]["col10"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["col9"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["acode"].ToString().Trim();
                            sg1_dr["sg1_f6"] = dt.Rows[i]["unit"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["COL1"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["COL2"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["COL3"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["COL4"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["COL5"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["COL6"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["COL7"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["COL8"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["COL14"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["COL15"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["COL16"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["COL19"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["COL20"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["COL21"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["COL22"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["COL17"].ToString().Trim();
                            sg1_dr["sg1_t17"] = dt.Rows[i]["NUM10"].ToString().Trim();
                            sg1_dr["sg1_t18"] = dt.Rows[i]["NUM8"].ToString().Trim();
                            sg1_dr["sg1_t19"] = dt.Rows[i]["NUM9"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                            totamt += fgen.make_double(dt.Rows[i]["COL5"].ToString().Trim());
                            tot_qty += fgen.make_double(dt.Rows[i]["COL3"].ToString().Trim());
                        }
                        txtTotQty.Text = tot_qty.ToString();
                        txtTotAmt.Text = totamt.ToString();
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
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    col2 = "F1002";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", col2);
                    fgen.fin_invn_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    hffield.Value = "TACODE_E";
                    doc_addl.Value = col1;
                    if (doc_qty.Value == "07")
                    {
                        if (Multi_MRR.Value == "Y")
                        {
                            col2 = fgen.seek_iname(frm_qstr, frm_cocd, "select cavity from ivoucher where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') in (" + doc_addl.Value + ") and store<>'R' order by vchnum desc", "cavity");
                        }
                        else
                        {
                            col2 = fgen.seek_iname(frm_qstr, frm_cocd, "select cavity from ivoucher where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') ='" + doc_addl.Value + "' and store<>'R'", "cavity");
                        }
                        fgen.Fn_ValueBox("Enter The Currency Rate!! Conv.Rate Entered At MRR Screen is " + col2, frm_qstr);
                    }
                    else
                    {
                        btnhideF_Click(sender, e);
                    }
                    break;

                case "TACODE_MULTIMRR":
                    if (col1.Length <= 0)
                    {
                        fgen.msg("-", frm_qstr, "Please Select Supplier'13'Entry Only Allowed After Selecting Supplier Only");
                        return;
                    }
                    hffield.Value = "TACODE";
                    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, b.aname as Supplier,a.vchnum as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,a.acode as Supp_Code,a.type,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.finvno,to_char(a.vchdate,'yyyy/mm/dd') as vdd from ivoucher a , famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " AND a.type='" + doc_qty.Value + "' and a.store<>'R' and (Trim(a.acodE),a.vchnum,to_char(a.vchdate,'dd/mm/yyyy')) not in (select trim(acode),trim(col9),trim(col10) from scratch where branchcd='" + frm_mbr + "' and type='LC' and vchdate " + DateRange + ") and a.acode='" + col1 + "' order by vdd desc,mrr_no desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_mseek("Select MRR Entry", frm_qstr);
                    break;

                case "TACODE_E":
                    #region Grid Filling After MRR Selection
                    lbl1a.Text = frm_vty;
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' and type='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    txtlbl2.Text = frm_uname;
                    txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    txtlbl5.Text = "-";
                    txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    if (doc_qty.Value == "07")
                    {
                        txtlbl8.Text = col1;
                    }
                    else
                    {
                        txtlbl8.Text = "1";
                    }
                    if (Multi_MRR.Value == "Y")
                    {
                        SQuery = "Select a.POTYPE,a.PONUM,a.PODATE,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,a.acode,' ' as naration,a.iqty_wt,a.iqtyin,a.ipack,a.irate,a.iamount,a.type,a.CAVITY,f.aname,i.iname,i.unit,a.srno from ivoucher a,famst f,item i where trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + doc_addl.Value + ") and a.store<>'R' order by a.vchnum,a.srno";
                    }
                    else
                    {
                        SQuery = "Select a.POTYPE,a.PONUM,a.PODATE,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,a.acode,' ' as naration,a.iqty_wt,a.iqtyin,a.ipack,a.irate,a.iamount,a.type,a.CAVITY,f.aname,i.iname,i.unit,a.srno from ivoucher a,famst f,item i where trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') ='" + doc_addl.Value + "' and a.store<>'R' order by a.vchnum,a.srno";
                    }
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl4.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["vchdate"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["aname"].ToString().Trim();
                        create_tab();
                        dt2 = new DataTable();
                        sg1_dr = null;

                        prt_rav = fgen.seek_iname(frm_qstr, frm_cocd, "select lpad(trim(upper(opt_param)),2,'0') as opt from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W1000'", "opt");
                        
                        if (prt_rav == "06" || prt_rav == "05")
                        {
                            prt_rav = "Y";
                        }
                        else
                        {
                            prt_rav = "N";
                        }
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
                            sg1_dr["sg1_srno"] = i + 1;
                            if (prt_rav == "Y")
                            {
                                sg1_dr["sg1_h9"] = fgen.make_double(dt.Rows[i]["ipack"].ToString().Trim()) * fgen.make_double(txtlbl8.Text);
                            }
                            else
                            {
                                sg1_dr["sg1_h9"] = fgen.make_double(dt.Rows[i]["irate"].ToString().Trim()) * fgen.make_double(txtlbl8.Text);
                            }
                            sg1_dr["sg1_h10"] = "0";
                            txtlbl9.Text = (fgen.make_double(dt.Rows[i]["irate"].ToString().Trim()) * fgen.make_double(txtlbl8.Text)).ToString();
                            sg1_dr["sg1_f1"] = dt.Rows[i]["irate"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["vchdate"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["vchnum"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["acode"].ToString().Trim();
                            sg1_dr["sg1_f6"] = dt.Rows[i]["unit"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["invno"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["invdate"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["iqtyin"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["iname"].ToString().Trim();
                            string d = "select col4,num14,num15,col29 from scratch where branchcd='" + frm_mbr + "' and type='VC' and trim(AcodE)='" + dt.Rows[i]["acode"].ToString().Trim() + "' and trim(Col2)='" + dt.Rows[i]["iqtyin"].ToString().Trim() + "' and trim(icodE)='" + dt.Rows[i]["icode"].ToString().Trim() + "' and trim(col27)='" + dt.Rows[i]["vchnum"].ToString().Trim() + dt.Rows[i]["vchdate"].ToString().Trim() + "'";
                            dt2 = fgen.getdata(frm_qstr, frm_cocd, "select col4,num14,num15,col29 from scratch where branchcd='" + frm_mbr + "' and type='VC' and trim(AcodE)='" + dt.Rows[i]["acode"].ToString().Trim() + "' and trim(Col2)='" + dt.Rows[i]["iqtyin"].ToString().Trim() + "' and trim(icodE)='" + dt.Rows[i]["icode"].ToString().Trim() + "' and trim(col27)='" + dt.Rows[i]["vchnum"].ToString().Trim() + dt.Rows[i]["vchdate"].ToString().Trim() + "'");
                            if (dt2.Rows.Count > 0)
                            {
                                if (frm_cocd != "PRPL")
                                {
                                    sg1_dr["sg1_h9"] = fgen.make_double(dt2.Rows[0]["col4"].ToString().Trim());
                                    ps_rt = fgen.make_double(dt2.Rows[0]["col4"].ToString().Trim()).ToString();
                                    // COMMENTED BECUASE IN MAIN Tejaxo T6,T7,T8 BELONGS TO E.DUTY,CESS,CST BUT IN WEB THESE THREE BELONGS TO CSGST,IGST,CGST
                                    //sg1_dr["sg1_t6"] = fgen.make_double(dt2.Rows[i]["num14"].ToString().Trim());
                                    //sg1_dr["sg1_t7"] = fgen.make_double(dt2.Rows[i]["num15"].ToString().Trim());
                                    //if (frm_cocd == "YTEC" && doc_qty.Value == "07")
                                    //{

                                    //}
                                    //else
                                    //{
                                    //    sg1_dr["sg1_t8"] = fgen.make_double(dt2.Rows[i]["col29"].ToString().Trim());
                                    //}
                                }
                            }
                            db1 = Math.Round(fgen.make_double(dt.Rows[i]["iamount"].ToString().Trim()), 0);
                            db2 = fgen.make_double(dt.Rows[i]["irate"].ToString().Trim());
                            db3 = fgen.make_double(dt.Rows[i]["iqtyin"].ToString().Trim());
                            db4 = Math.Round(db2 * db3, 0);
                            if (fgen.make_double(dt.Rows[i]["cavity"].ToString().Trim()) != 1 && fgen.make_double(dt.Rows[i]["cavity"].ToString().Trim()) > 0 && frm_cocd != "YTEC")
                            {
                                if ((db1 == db4) || fgen.make_double(dt.Rows[i]["cavity"].ToString().Trim()) == 0)
                                {
                                    sg1_dr["sg1_t5"] = (Math.Round(fgen.make_double(dt.Rows[i]["iamount"].ToString().Trim()) * fgen.make_double(txtlbl8.Text), 2)).ToString();
                                }
                                else
                                {
                                    sg1_dr["sg1_t5"] = (Math.Round((fgen.make_double(dt.Rows[i]["iamount"].ToString().Trim()) / fgen.make_double(dt.Rows[i]["cavity"].ToString().Trim())) * fgen.make_double(txtlbl8.Text), 2)).ToString();
                                    sg1_dr["sg1_h9"] = fgen.make_double(dt.Rows[i]["irate"].ToString().Trim());
                                }
                            }
                            else
                            {
                                if (fgen.make_double(ps_rt) > 0)
                                {
                                    sg1_dr["sg1_t5"] = (Math.Round(fgen.make_double(dt.Rows[i]["iqtyin"].ToString().Trim()) * fgen.make_double(ps_rt), 2)).ToString();
                                }
                                else
                                {
                                    sg1_dr["sg1_t5"] = (Math.Round(fgen.make_double(dt.Rows[i]["iamount"].ToString().Trim()) * fgen.make_double(txtlbl8.Text), 2)).ToString();
                                }
                            }
                            //sg1_dr["sg1_t5"] = (Math.Round(fgen.make_double(dt.Rows[i]["iqtyin"].ToString().Trim()) * fgen.make_double(txtlbl9.Text), 2)).ToString();
                            sg1_dr["sg1_t8"] = "";
                            sg1_dr["sg1_t9"] = "";
                            sg1_dr["sg1_t10"] = "";
                            sg1_dr["sg1_t11"] = "";
                            sg1_dr["sg1_t12"] = "";
                            sg1_dr["sg1_t13"] = "";
                            sg1_dr["sg1_t14"] = "";
                            sg1_dr["sg1_t15"] = "";
                            sg1_dr["sg1_t16"] = "";
                            sg1_dr["sg1_t17"] = dt.Rows[i]["iqty_wt"].ToString().Trim();
                            sg1_dr["sg1_t18"] = "";
                            sg1_dr["sg1_t19"] = "";
                            sg1_dr["sg1_t20"] = "";
                            sg1_dr["sg1_t21"] = "";
                            sg1_dr["sg1_t22"] = "";
                            sg1_dr["sg1_t23"] = "";
                            sg1_dr["sg1_t24"] = "";
                            sg1_dt.Rows.Add(sg1_dr);
                            ps_rt = "0";
                        }
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        Fn_ValueBox_Multiple("Please Enter Values", frm_qstr);
                        hffield.Value = "CAL";
                    }
                    #endregion
                    break;

                case "CAL":
                    Cal();
                    break;

                case "BTN_10":
                    if (col1.Length <= 0) return;
                    txtlbl10.Text = col2;
                    btnlbl11.Focus();
                    break;
                case "BTN_11":
                    if (col1.Length <= 0) return;
                    txtlbl11.Text = col2;
                    btnlbl12.Focus();
                    break;
                case "BTN_12":
                    if (col1.Length <= 0) return;
                    txtlbl12.Text = col2;
                    btnlbl13.Focus();
                    break;
                case "BTN_13":
                    if (col1.Length <= 0) return;
                    txtlbl13.Text = col2;
                    btnlbl14.Focus();
                    break;
                case "BTN_14":
                    if (col1.Length <= 0) return;
                    txtlbl14.Text = col2;
                    btnlbl15.Focus();
                    break;
                case "BTN_15":
                    if (col1.Length <= 0) return;
                    txtlbl15.Text = col2;
                    btnlbl16.Focus();
                    break;
                case "BTN_16":
                    if (col1.Length <= 0) return;
                    txtlbl16.Text = col2;
                    btnlbl17.Focus();
                    break;
                case "BTN_17":
                    if (col1.Length <= 0) return;
                    txtlbl17.Text = col2;
                    btnlbl18.Focus();
                    break;
                case "BTN_18":
                    if (col1.Length <= 0) return;
                    txtlbl18.Text = col2;
                    break;

                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    txtlbl2.Focus();
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
                            sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                            sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["sg1_h2"].ToString();
                            sg1_dr["sg1_h3"] = dt.Rows[i]["sg1_h3"].ToString();
                            sg1_dr["sg1_h4"] = dt.Rows[i]["sg1_h4"].ToString();
                            sg1_dr["sg1_h5"] = dt.Rows[i]["sg1_h5"].ToString();
                            sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                            sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                            sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                            sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                            sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();

                            sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
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

                        dt = new DataTable();


                        String pop_qry;

                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");
                        if (col1.Trim().Length == 8) SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                        else SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in (" + col1 + ")";

                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_h3"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PACKSIZE FROM ITEM WHERE TRIM(ICODe)='" + dt.Rows[d]["icode"].ToString().Trim() + "'", "PACKSIZE");
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";

                            sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["po_no"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = "";
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = dt.Rows[d]["Balance_Qty"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[d]["Irate"].ToString().Trim();

                            sg1_dr["sg1_t9"] = "";
                            sg1_dr["sg1_t10"] = "-";
                            sg1_dr["sg1_t11"] = dt.Rows[d]["iexc_Addl"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[d]["frt_pu"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[d]["pkchg_pu"].ToString().Trim();

                            string mpo_Dt;
                            mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(9, 6);
                            sg1_dr["sg1_t14"] = mpo_Dt;
                            sg1_dr["sg1_t15"] = "";
                            mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(6, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(4, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 4);
                            sg1_dr["sg1_t16"] = fgen.make_def_Date(mpo_Dt, vardate);


                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    //dt.Dispose(); 
                    sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    break;
                case "SG2_ROW_ADD":
                    if (col1.Length < 2) return;
                    #region for gridview 2
                    if (col1.Length <= 0) return;
                    if (ViewState["sg2"] != null)
                    {
                        dt = new DataTable();
                        sg2_dt = new DataTable();
                        dt = (DataTable)ViewState["sg2"];
                        z = dt.Rows.Count - 1;
                        sg2_dt = dt.Clone();
                        sg2_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = Convert.ToInt32(dt.Rows[i]["sg2_srno"].ToString());
                            sg2_dr["sg2_h1"] = dt.Rows[i]["sg2_h1"].ToString();
                            sg2_dr["sg2_h2"] = dt.Rows[i]["sg2_h2"].ToString();
                            sg2_dr["sg2_h3"] = dt.Rows[i]["sg2_h3"].ToString();
                            sg2_dr["sg2_h4"] = dt.Rows[i]["sg2_h4"].ToString();
                            sg2_dr["sg2_h5"] = dt.Rows[i]["sg2_h5"].ToString();

                            sg2_dr["sg2_f1"] = dt.Rows[i]["sg2_f1"].ToString();
                            sg2_dr["sg2_f2"] = dt.Rows[i]["sg2_f2"].ToString();
                            sg2_dr["sg2_f3"] = dt.Rows[i]["sg2_f3"].ToString();
                            sg2_dr["sg2_f4"] = dt.Rows[i]["sg2_f4"].ToString();
                            sg2_dr["sg2_f5"] = dt.Rows[i]["sg2_f5"].ToString();

                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                            sg2_dr["sg2_t3"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim();
                            sg2_dr["sg2_t4"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim();
                            sg2_dr["sg2_t5"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim();
                            sg2_dr["sg2_t6"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim();
                            sg2_dr["sg2_t7"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim();
                            sg2_dr["sg2_t8"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim();
                            sg2_dr["sg2_t9"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim();
                            sg2_dr["sg2_t10"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim();

                            sg2_dt.Rows.Add(sg2_dr);
                        }

                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_h1"] = col1;
                            sg2_dr["sg2_h2"] = col2;
                            sg2_dr["sg2_h3"] = "-";
                            sg2_dr["sg2_h4"] = "-";
                            sg2_dr["sg2_h5"] = "-";

                            sg2_dr["sg2_f1"] = col1;
                            sg2_dr["sg2_f2"] = col2;
                            sg2_dr["sg2_f3"] = "-";
                            sg2_dr["sg2_f4"] = "-";
                            sg2_dr["sg2_f5"] = "-";

                            sg2_dr["sg2_t1"] = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(kclreelno) AS VCH FROM REELVCH WHERE BRANCHCD='" + frm_mbr + "' AND TYPE like '0%' AND VCHDATE " + DateRange + " ", 6, "VCH");
                            sg2_dr["sg2_t2"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                            sg2_dr["sg2_t3"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

                            sg2_dt.Rows.Add(sg2_dr);
                        }
                    }
                    sg2_add_blankrows();

                    ViewState["sg2"] = sg2_dt;
                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    dt.Dispose(); sg2_dt.Dispose();
                    ((TextBox)sg2.Rows[z].FindControl("sg2_t1")).Focus();
                    #endregion
                    setColHeadings();
                    break;
                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    if (edmode.Value == "Y")
                    {
                        //return;
                    }

                    //********* Saving in Hidden Field
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    //********* Saving in GridView Value
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

                    setColHeadings();
                    break;
                case "SG3_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    if (ViewState["sg3"] != null)
                    {
                        dt = new DataTable();
                        sg3_dt = new DataTable();
                        dt = (DataTable)ViewState["sg3"];
                        z = dt.Rows.Count - 1;
                        sg3_dt = dt.Clone();
                        sg3_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = Convert.ToInt32(dt.Rows[i]["sg3_srno"].ToString());
                            sg3_dr["sg3_f1"] = dt.Rows[i]["sg3_f1"].ToString();
                            sg3_dr["sg3_f2"] = dt.Rows[i]["sg3_f2"].ToString();
                            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();
                            sg3_dt.Rows.Add(sg3_dr);
                        }

                        dt = new DataTable();
                        if (col1.Length > 8) SQuery = "select * from item where trim(icode) in (" + col1 + ")";
                        else SQuery = "select * from item where trim(icode)='" + col1 + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;

                            sg3_dr["sg3_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg3_dr["sg3_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg3_dr["sg3_t1"] = "";
                            sg3_dr["sg3_t2"] = "";
                            sg3_dr["sg3_t3"] = "";
                            sg3_dr["sg3_t4"] = "";
                            sg3_dt.Rows.Add(sg3_dr);
                        }
                    }
                    sg3_add_blankrows();

                    ViewState["sg3"] = sg3_dt;
                    sg3.DataSource = sg3_dt;
                    sg3.DataBind();
                    dt.Dispose(); sg3_dt.Dispose();
                    ((TextBox)sg3.Rows[z].FindControl("sg3_t1")).Focus();
                    #endregion
                    break;

                case "SG2_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg2_dt = new DataTable();
                        dt = (DataTable)ViewState["sg2"];
                        z = dt.Rows.Count - 1;
                        sg2_dt = dt.Clone();
                        sg2_dr = null;
                        i = 0;
                        for (i = 0; i < sg2.Rows.Count - 1; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = (i + 1);

                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();


                            sg2_dt.Rows.Add(sg2_dr);
                        }

                        sg2_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg2_add_blankrows();

                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                    }
                    #endregion
                    setColHeadings();
                    break;
                case "SG4_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg4_dt = new DataTable();
                        dt = (DataTable)ViewState["sg4"];
                        z = dt.Rows.Count - 1;
                        sg4_dt = dt.Clone();
                        sg4_dr = null;
                        i = 0;
                        for (i = 0; i < sg4.Rows.Count - 1; i++)
                        {
                            sg4_dr = sg4_dt.NewRow();
                            sg4_dr["sg4_srno"] = (i + 1);

                            sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                            sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();


                            sg4_dt.Rows.Add(sg4_dr);
                        }

                        sg4_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg4_add_blankrows();

                        ViewState["sg4"] = sg4_dt;
                        sg4.DataSource = sg4_dt;
                        sg4.DataBind();
                    }
                    #endregion
                    setColHeadings();
                    break;

                case "SG3_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg3_dt = new DataTable();
                        dt = (DataTable)ViewState["sg3"];
                        z = dt.Rows.Count - 1;
                        sg3_dt = dt.Clone();
                        sg3_dr = null;
                        i = 0;
                        for (i = 0; i < sg3.Rows.Count - 1; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = (i + 1);
                            sg3_dr["sg3_f1"] = sg3.Rows[i].Cells[3].Text.Trim();
                            sg3_dr["sg3_f2"] = sg3.Rows[i].Cells[4].Text.Trim();

                            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();

                            sg3_dt.Rows.Add(sg3_dr);
                        }

                        sg3_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg3_add_blankrows();

                        ViewState["sg3"] = sg3_dt;
                        sg3.DataSource = sg3_dt;
                        sg3.DataBind();
                    }
                    #endregion
                    setColHeadings();
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
                        for (i = 0; i < sg1.Rows.Count - 1; i++)
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

                        if (edmode.Value == "Y")
                        {
                            //sg1_dr["sg1_f1"] = "*" + sg1.Rows[-1 + Convert.ToInt32(hf1.Value.Trim())].Cells[13].Text.Trim();

                            sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        }
                        else
                        {
                            sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        }

                        sg1_add_blankrows();

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
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "SELECT a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as dated,a.col9 as mrr_no,a.col10 as mrr_dt,a.remarks as mrr_type,a.col27 as currency,a.acode as cust_code,a.col11 as supplier,a.icode as item_code,b.iname as item_name,a.col3 as qty_rcvd,a.col5 as tot_amt,a.col26 as landed_cost_per_unit,a.COL17 as total_landed_cost,to_char(A.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + " order by vdd desc,entry_no desc,a.srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel(lblheader.Text + " Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------
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
                            frm_vnum = txtvchnum.Text.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            save_it = "Y";
                            //for (i = 0; i < sg1.Rows.Count - 0; i++)
                            //{
                            //    if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2)
                            //    {
                            //        save_it = "Y";
                            //    }
                            //}

                            if (save_it == "Y")
                            {
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                            txtvchnum.Text = frm_vnum;
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();

                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }

                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); setColHeadings();
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
        sg1_dt.Columns.Add(new DataColumn("sg1_t22", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t23", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t24", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        // Hidden Field
        sg2_dt.Columns.Add(new DataColumn("sg2_h1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t6", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t7", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t8", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t9", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t10", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void create_tab3()
    {
        sg3_dt = new DataTable();
        sg3_dr = null;
        // Hidden Field
        sg3_dt.Columns.Add(new DataColumn("sg3_SrNo", typeof(Int32)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t3", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t4", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void create_tab4()
    {
        sg4_dt = new DataTable();
        sg4_dr = null;
        // Hidden Field
        sg4_dt.Columns.Add(new DataColumn("sg4_SrNo", typeof(Int32)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t1", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t2", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t3", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t4", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        if (sg1_dt == null) create_tab();
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
        sg1_dr["sg1_t22"] = "-";
        sg1_dr["sg1_t23"] = "-";
        sg1_dr["sg1_t24"] = "-";
        sg1_dt.Rows.Add(sg1_dr);
    }
    //------------------------------------------------------------------------------------
    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();
        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
        sg2_dr["sg2_h1"] = "-";
        sg2_dr["sg2_h2"] = "-";
        sg2_dr["sg2_h3"] = "-";
        sg2_dr["sg2_h4"] = "-";
        sg2_dr["sg2_h5"] = "-";
        sg2_dr["sg2_f1"] = "-";
        sg2_dr["sg2_f2"] = "-";
        sg2_dr["sg2_f3"] = "-";
        sg2_dr["sg2_f4"] = "-";
        sg2_dr["sg2_f5"] = "-";
        sg2_dr["sg2_t1"] = "-";
        sg2_dr["sg2_t2"] = "-";
        sg2_dr["sg2_t3"] = "-";
        sg2_dr["sg2_t4"] = "-";
        sg2_dr["sg2_t5"] = "-";
        sg2_dr["sg2_t6"] = "-";
        sg2_dr["sg2_t7"] = "-";
        sg2_dr["sg2_t8"] = "-";
        sg2_dr["sg2_t9"] = "-";
        sg2_dr["sg2_t10"] = "-";
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
    public void sg4_add_blankrows()
    {
        sg4_dr = sg4_dt.NewRow();
        sg4_dr["sg4_SrNo"] = sg4_dt.Rows.Count + 1;
        sg4_dr["sg4_t1"] = "-";
        sg4_dr["sg4_t2"] = "-";
        sg4_dt.Rows.Add(sg4_dr);
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
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "SG1_ROW_TAX":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_TAX";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                break;
            case "SG1_ROW_DT":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_DT";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

                    fgen.Fn_open_dtbox("Select Date", frm_qstr);

                }
                break;

            case "SG1_ROW_ADD":
                string gate_link = fgen.seek_iname(frm_qstr, frm_cocd, "select enable_yn from controls where id='M52'", "enable_yn");
                if (gate_link == "Y")
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);

                    dt = new DataTable();
                    sg1_dt = new DataTable();
                    dt = (DataTable)ViewState["sg1"];
                    z = dt.Rows.Count - 1;
                    sg1_dt = dt.Clone();
                    sg1_dr = null;
                    i = 0;
                    for (i = 0; i < sg1.Rows.Count; i++)
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
                        sg1_dr["sg1_t21"] = "1";

                        sg1_dt.Rows.Add(sg1_dr);
                    }

                    sg1_dr = sg1_dt.NewRow();
                    sg1_dr["sg1_srno"] = (i + 1);
                    i = index;
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
                    sg1_dr["sg1_t21"] = (i + 2);

                    sg1_dt.Rows.Add(sg1_dr);
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    setColHeadings();
                    set_Val();
                }
                else
                {
                    if (index < sg1.Rows.Count - 1)
                    {
                        hf1.Value = index.ToString();
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                        //----------------------------
                        hffield.Value = "SG1_ROW_ADD_E";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("Select Item", frm_qstr);
                    }
                    else
                    {
                        hffield.Value = "SG1_ROW_ADD";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                        make_qry_4_popup();
                        fgen.Fn_open_mseek("Select Item", frm_qstr);
                    }
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg2.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG2_RMV":
                if (index < sg2.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG2_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "SG2_ROW_ADD":
                hffield.Value = "SG2_ROW_ADD";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

                col1 = "";
                foreach (GridViewRow gr1 in sg1.Rows)
                {
                    if (col1.Length > 0) col1 += ",'" + gr1.Cells[13].Text.Trim().ToString() + "'";
                    else col1 = "'" + gr1.Cells[13].Text.Trim().ToString() + "'";
                }

                SQuery = "SELECT TRIM(ICODe) AS FSTR,INAME AS PRODUCT,ICODE AS ERPCODE,OPRATE1 AS SIZE_,OPRATE3 AS GSM,UNIT FROM ITEM WHERE TRIM(ICODE) IN (" + col1 + ") ORDER BY ICODE ";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek("Select Item", frm_qstr);
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg3.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG3_RMV":
                if (index < sg3.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG3_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "SG3_ROW_ADD":
                if (index < sg3.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG3_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG3_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Item", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg4.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "sg4_RMV":
                if (index < sg4.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "sg4_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "sg4_ROW_ADD":
                dt = new DataTable();
                sg4_dt = new DataTable();
                dt = (DataTable)ViewState["sg4"];
                z = dt.Rows.Count - 1;
                sg4_dt = dt.Clone();
                sg4_dr = null;
                i = 0;
                for (i = 0; i < sg4.Rows.Count; i++)
                {
                    sg4_dr = sg4_dt.NewRow();
                    sg4_dr["sg4_srno"] = (i + 1);
                    sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                    sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
                    sg4_dt.Rows.Add(sg4_dr);
                }
                sg4_add_blankrows();
                ViewState["sg4"] = sg4_dt;
                sg4.DataSource = sg4_dt;
                sg4.DataBind();
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl10_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_10";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl10.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl11_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_11";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl10.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl12_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_12";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_13";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_14";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_15";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl15.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_16";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl16.Text, frm_qstr);
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
        hffield.Value = "BTN_19";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = lbl1a.Text.Substring(0, 2);
            oporow["vchnum"] = frm_vnum.Trim();
            oporow["vchdate"] = txtvchdate.Text.Trim();
            oporow["ACODE"] = txtlbl7.Text.Trim().ToUpper();
            oporow["ICODE"] = sg1.Rows[i].Cells[16].Text.Trim().ToUpper();
            oporow["SRNO"] = i + 1;
            oporow["COL1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
            oporow["COL2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper();
            oporow["COL3"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper());
            oporow["COL4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper();
            oporow["COL5"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper());
            oporow["COL6"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper());
            oporow["COL7"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper());
            oporow["COL8"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper());
            oporow["COL9"] = sg1.Rows[i].Cells[15].Text.Trim().ToUpper();
            oporow["COL10"] = sg1.Rows[i].Cells[14].Text.Trim().ToUpper();
            oporow["COL11"] = txtlbl7a.Text.Trim().ToUpper();
            oporow["COL12"] = "-";
            oporow["COL13"] = "-";
            oporow["COL14"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper());
            oporow["REMARKS"] = doc_qty.Value;
            oporow["DOCDATE"] = vardate;
            oporow["COL15"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().ToUpper());
            oporow["COL16"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim().ToUpper());
            oporow["COL17"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().ToUpper());
            oporow["COL18"] = Math.Round(fgen.make_double(sg1.Rows[i].Cells[8].Text.Trim().ToUpper()), 2);
            oporow["COL19"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim().ToUpper());
            oporow["COL20"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim().ToUpper());
            oporow["COL21"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim().ToUpper());
            oporow["COL22"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim().ToUpper());
            oporow["COL23"] = "-";
            oporow["COL24"] = "-";
            oporow["COL25"] = "-";
            oporow["COL26"] = fgen.make_double(sg1.Rows[i].Cells[9].Text.Trim().ToUpper());
            oporow["COL27"] = fgen.make_double(txtlbl8.Text.Trim().ToUpper());
            oporow["COL28"] = "-";
            oporow["COL29"] = "-";
            oporow["COL30"] = "-";
            oporow["COL31"] = "-";
            oporow["COL32"] = "-";
            oporow["COL33"] = "-";
            oporow["COL34"] = "-";
            oporow["COL35"] = "-";
            oporow["COL36"] = "-";
            oporow["COL37"] = "-";
            oporow["COL38"] = "-";
            oporow["COL39"] = "-";
            oporow["COL40"] = "-";
            oporow["COL41"] = "-";
            oporow["COL42"] = "-";
            oporow["COL43"] = "-";
            oporow["COL44"] = "-";
            oporow["COL45"] = "-";
            oporow["COL46"] = "-";
            oporow["COL47"] = "-";
            oporow["NUM1"] = 0;
            oporow["NUM2"] = 0;
            oporow["NUM3"] = 0;
            oporow["NUM4"] = 0;
            oporow["NUM5"] = 0;
            oporow["NUM6"] = 0;
            oporow["NUM7"] = Math.Round(fgen.make_double(sg1.Rows[i].Cells[13].Text.Trim().ToUpper()), 2);
            oporow["NUM8"] = Math.Round(fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim().ToUpper()), 2);
            oporow["NUM9"] = Math.Round(fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim().ToUpper()), 2);
            oporow["NUM10"] = Math.Round(fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim().ToUpper()), 2);
            if (txtrmk.Text.Trim().Length > 125)
            {
                oporow["NARATION"] = txtrmk.Text.Trim().Substring(0, 124).ToUpper();
            }
            else
            {
                oporow["NARATION"] = txtrmk.Text.Trim().ToUpper();
            }
            oporow["INVNO"] = "-";
            oporow["INVDATE"] = vardate;
            oporow["CHK_BY"] = "-";
            oporow["CHK_DT"] = vardate;
            oporow["NUM11"] = 0;
            oporow["NUM12"] = 0;
            oporow["NUM13"] = 0;
            oporow["NUM14"] = 0;
            oporow["NUM15"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim().ToUpper());
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
            if (frm_vnum != "000000")
            {
                double lc_amt_1 = 0;
                

                prt_rav = fgen.seek_iname(frm_qstr, frm_cocd, "select lpad(trim(upper(opt_param)),2,'0') as opt from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W1000'", "opt");

                if (prt_rav == "06" || prt_rav == "05")
                {
                    prt_rav = "Y";
                }
                else
                {
                    prt_rav = "N";
                }
                if (prt_rav == "Y")
                {
                    if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim().ToUpper()) > 0 && sg1.Rows[i].Cells[16].Text.Trim().ToUpper().Substring(0, 2) == "02")
                    {
                        lc_amt_1 = Math.Round(fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().ToUpper()) / fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim().ToUpper()), 3);
                    }
                    else
                    {
                        lc_amt_1 = Math.Round(fgen.make_double(sg1.Rows[i].Cells[9].Text.Trim()), 3);
                    }

                    if (Multi_MRR.Value == "Y")
                    {
                        SQuery = "update ivoucher set ichgs=" + Math.Round(fgen.make_double(sg1.Rows[i].Cells[9].Text.Trim()), 3) + ",ipack=" + lc_amt_1 + " where branchcd='" + frm_mbr + "' and trim(icode)='" + sg1.Rows[i].Cells[16].Text.Trim().ToUpper() + "' and vchnum='" + sg1.Rows[i].Cells[15].Text.Trim().ToUpper() + "' and vchdate=to_date('" + sg1.Rows[i].Cells[14].Text.Trim().ToUpper() + "','dd/mm/yyyy') and type='" + doc_qty.Value.Trim() + "'";

                    }
                    else
                    {
                        SQuery = "update ivoucher set ichgs=" + Math.Round(fgen.make_double(sg1.Rows[i].Cells[9].Text.Trim()), 3) + ",ipack=" + lc_amt_1 + " where branchcd='" + frm_mbr + "' and trim(icode)='" + sg1.Rows[i].Cells[16].Text.Trim().ToUpper() + "' and vchnum='" + txtlbl4.Text.Trim() + "' and vchdate=to_date('" + txtlbl4a.Text.Trim() + "','dd/mm/yyyy') and type='" + doc_qty.Value.Trim() + "'";
                    }
                }
                else
                {
                    SQuery = "update ivoucher set ichgs=" + Math.Round(fgen.make_double(sg1.Rows[i].Cells[9].Text.Trim()), 3) + " where branchcd='" + frm_mbr + "' and trim(icode)='" + sg1.Rows[i].Cells[16].Text.Trim().ToUpper() + "' and vchnum='" + txtlbl4.Text.Trim() + "' and vchdate=to_date('" + txtlbl4a.Text.Trim() + "','dd/mm/yyyy') and type='" + doc_qty.Value.Trim() + "'";
                }
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
            }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {

    }
    //------------------------------------------------------------------------------------
    void save_fun3()
    {

    }
    //------------------------------------------------------------------------------------
    void save_fun4()
    {

    }
    //------------------------------------------------------------------------------------
    void save_fun5()
    {

    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE FROM type where id='M' and type1 like '0%' and length(trim(type1))=2 order by type1";
    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }
    //------------------------------------------------------------------------------------
    protected void Cal()
    {
        z = 0;
        double CGST = 0, IGST = 0, SGST = 0, freight = 0, other = 0, sad = 0, landing_chg = 0, lc_tt = 0, clring = 0, srv = 0, lcost = 0;
        totamt = 0; tot_qty = 0;
        col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL1").ToString().Trim();
        col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").ToString().Trim();
        col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL3").ToString().Trim();
        col4 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL4").ToString().Trim();
        col5 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL5").ToString().Trim();
        col6 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL6").ToString().Trim();
        col7 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL7").ToString().Trim();
        col8 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL8").ToString().Trim();
        col9 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL9").ToString().Trim();
        col10 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL10").ToString().Trim();
        for (int i = 0; i < sg1.Rows.Count - 0; i++)
        {
            totamt += fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
            tot_qty += fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
            Session["Filled"] = "Y";
        }
        for (z = 0; z < sg1.Rows.Count - 0; z++)
        {
            CGST = Math.Round(((fgen.make_double(((TextBox)sg1.Rows[z].FindControl("sg1_t5")).Text) / totamt) * fgen.make_double(col1)), 2);
            IGST = Math.Round(((fgen.make_double(((TextBox)sg1.Rows[z].FindControl("sg1_t5")).Text) / totamt) * fgen.make_double(col3)), 2);
            //SGST = Math.Round(((fgen.make_double(((TextBox)sg1.Rows[z].FindControl("sg1_t5")).Text) / totamt) * fgen.make_double(col3)), 2);
            SGST = CGST;
            string Party_GST = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(unit) as unit from ivoucher where branchcd='" + frm_mbr + "' and type='" + doc_qty.Value + "' and vchnum='" + sg1.Rows[i].Cells[15].Text.Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + sg1.Rows[i].Cells[14].Text.Trim() + "'", "unit");
            if (Party_GST == "IG")
            {
                if (IGST == 0 && CGST != 0)
                {
                    fgen.msg("-", "AMSG", "CGST/SGST Is Not Applicable On Supplier " + txtlbl7a.Text);
                    return;
                }
            }
            else
            {
                if (CGST == 0 && IGST != 0)
                {
                    fgen.msg("-", "AMSG", "IGST Is Not Applicable On Supplier " + txtlbl7a.Text);
                    return;
                }
            }
            freight = Math.Round(((fgen.make_double(((TextBox)sg1.Rows[z].FindControl("sg1_t5")).Text) / totamt) * fgen.make_double(col4)), 2);
            other = Math.Round(((fgen.make_double(((TextBox)sg1.Rows[z].FindControl("sg1_t5")).Text) / totamt) * fgen.make_double(col5)), 2);
            sad = Math.Round(((fgen.make_double(((TextBox)sg1.Rows[z].FindControl("sg1_t5")).Text) / totamt) * fgen.make_double(col6)), 2);
            landing_chg = Math.Round(((fgen.make_double(((TextBox)sg1.Rows[z].FindControl("sg1_t5")).Text) / totamt) * fgen.make_double(col7)), 2);
            lc_tt = Math.Round(((fgen.make_double(((TextBox)sg1.Rows[z].FindControl("sg1_t5")).Text) / totamt) * fgen.make_double(col8)), 2);
            clring = Math.Round(((fgen.make_double(((TextBox)sg1.Rows[z].FindControl("sg1_t5")).Text) / totamt) * fgen.make_double(col9)), 2);
            srv = Math.Round(((fgen.make_double(((TextBox)sg1.Rows[z].FindControl("sg1_t5")).Text) / totamt) * fgen.make_double(col10)), 2);
            ((TextBox)sg1.Rows[z].FindControl("sg1_t6")).Text = CGST.ToString().Replace("NaN", "0").Replace("Infinity", "0");
            ((TextBox)sg1.Rows[z].FindControl("sg1_t7")).Text = SGST.ToString().Replace("NaN", "0").Replace("Infinity", "0");
            ((TextBox)sg1.Rows[z].FindControl("sg1_t8")).Text = IGST.ToString().Replace("NaN", "0").Replace("Infinity", "0");
            ((TextBox)sg1.Rows[z].FindControl("sg1_t9")).Text = freight.ToString().Replace("NaN", "0").Replace("Infinity", "0");
            ((TextBox)sg1.Rows[z].FindControl("sg1_t10")).Text = other.ToString().Replace("NaN", "0").Replace("Infinity", "0");
            ((TextBox)sg1.Rows[z].FindControl("sg1_t11")).Text = sad.ToString().Replace("NaN", "0").Replace("Infinity", "0");
            ((TextBox)sg1.Rows[z].FindControl("sg1_t12")).Text = landing_chg.ToString().Replace("NaN", "0").Replace("Infinity", "0");
            ((TextBox)sg1.Rows[z].FindControl("sg1_t13")).Text = lc_tt.ToString().Replace("NaN", "0").Replace("Infinity", "0");
            ((TextBox)sg1.Rows[z].FindControl("sg1_t14")).Text = clring.ToString().Replace("NaN", "0").Replace("Infinity", "0");
            ((TextBox)sg1.Rows[z].FindControl("sg1_t15")).Text = srv.ToString().Replace("NaN", "0").Replace("Infinity", "0");
            lcost = Math.Round(fgen.make_double(((TextBox)sg1.Rows[z].FindControl("sg1_t5")).Text) + (CGST + IGST + SGST + freight + other + sad + landing_chg + lc_tt + clring + srv), 2);            
            ((TextBox)sg1.Rows[z].FindControl("sg1_t16")).Text = Math.Round(lcost, 3).ToString().Replace("NaN", "0").Replace("Infinity", "0");
            sg1.Rows[z].Cells[9].Text = Math.Round(lcost / fgen.make_double(((TextBox)sg1.Rows[z].FindControl("sg1_t3")).Text), 3).ToString().Replace("NaN", "0").Replace("Infinity", "0");
        }
        txtTotQty.Text = tot_qty.ToString();
        txtTotAmt.Text = totamt.ToString();
    }
    //------------------------------------------------------------------------------------
    public void Fn_ValueBox_Multiple(string titl, string QR_str)
    {
        if (HttpContext.Current.CurrentHandler is Page)
        {
            string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/ival_multiple.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle1('" + fil_loc + "?STR=" + QR_str + "','410px','420px','" + titl + "');", true);
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnCal_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "CAL";
        Fn_ValueBox_Multiple("Please Enter Values", frm_qstr);
        //btnhideF_Click(sender, e);
    }
    //------------------------------------------------------------------------------------
}