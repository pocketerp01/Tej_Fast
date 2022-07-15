using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_ASN_PO : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", xStartDt = "", Enable = "";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable dtCol = new DataTable();

    string Checked_ok;
    string save_it, mq0, mq1, mq2, mq3, mq4;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    //double double_val2, double_val1;
    double db1, db2, db3, db4, db5, db6;
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
            btnprint.Visible = false;
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
        btnlbl4.Enabled = true; btnlbl7.Enabled = true;
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
        frm_tabname = "WB_ASN_DTL";
        //CREATE TABLE WB_ASN_DTL (BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM VARCHAR(6),VCHDATE DATE,SRNO NUMBER(5),PONUM VARCHAR(10),PODATE DATE,ICODE VARCHAR2(8),ACODE VARCHAR2(6),UNIT VARCHAR2(8),IQTYIN NUMBER(12,2),IQTYOUT NUMBER(12,2),GENUM VARCHAR2(8),GEDATE DATE,INVNO VARCHAR2(8),INVDATE DATE,TPT_NAMES VARCHAR2(80),MOB_NO VARCHAR2(12),DELV_dT DATE,MODE_TPT VARCHAR2(80),NARATION VARCHAR2(150),ENT_BY VARCHAR2(20),ENT_DT DATE,EDT_BY VARCHAR2(20),EDT_DT DATE)
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "AS");
        lblheader.Text = "ASN Details";
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

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[13].Text.ToString() + "'";
                    else col1 = "'" + gr.Cells[13].Text.ToString() + "'";
                }
                if (col1.Length <= 0) col1 = "'-'";
                //SQuery = "select a.po_num,a.po_date,a.item_code ,b.iname as item_name,a.unit,sum(a.qty) as qty from (select trim(ordno) as po_num,to_char(orddt,'dd/mm/yyyy') as po_date,trim(icode) as item_code,unit,qtyord as qty from pending_po_vu where branchcd='" + frm_mbr + "' and orddt " + PrdRange + " and acode='06C091' union all select trim(ponum) as po_num,to_char(podate,'dd/mm/yyyy') as po_date,trim(icode) as item_code,unit,0 as qty from wb_asn_dtl where branchcd='" + frm_mbr + "' and acode='06C091' and vchdate " + PrdRange + " ) a, item b where trim(a.item_code)=trim(b.icode) group by a.po_num,a.po_date,a.item_code ,b.iname ,a.unit having sum(a.qty)>0 order by po_num";
                SQuery = "select trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode) as fstr,trim(ordno) as po_num,to_char(orddt,'dd/mm/yyyy') as po_date,trim(icode) as item_code,trim(iname) as item_name,qtyord,unit from WBVU_pending_PO where branchcd='" + frm_mbr + "' and orddt " + PrdRange + " order by ordno";
                SQuery = "select trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode) as fstr,trim(a.ordno) as po_num,to_char(a.orddt,'dd/mm/yyyy') as po_date,trim(a.icode) as item_code,trim(b.iname) as item_name,a.qtyord,b.unit from WBVU_pending_PO A inner join item b on trim(b.icode)=trim(a.icode) where a.branchcd='" + frm_mbr + "' and a.orddt " + PrdRange + " order by a.ordno";
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
                SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.licno,to_char(a.licdt,'dd/mm/yyyy') as licence_dt,a.hscode,b.name as item_desc,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,typegrp b WHERE trim(a.hscode)=trim(b.acref) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' AND a.VCHDATE  " + DateRange + " ORDER BY vdd desc,entry_no DESC";
                break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "SELECT distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,trim(a.acode) as Vendor_code,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,to_char(a.vchdate,'yyyymmdd') as vdd FROM " + frm_tabname + " A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE  " + DateRange + " ORDER BY vdd desc,A.VCHNUM DESC";
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
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + "", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtent_by.Text = frm_uname;
        txtent_dt.Text = vardate;
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();
        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        hffield.Value = "TACODE";
        fgen.Fn_open_prddmp1("-", frm_qstr);
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
        cal();
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

        for (i = 0; i < sg1.Rows.Count; i++)
        {
            if (((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToString().Length <= 1 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()) > 0)
            {
                fgen.msg("-", "AMSG", "Please Fill Invoice No and Invoice Date At Line No. " + sg1.Rows[i].Cells[12].Text.Trim() + "");
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
        //DDClear();
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
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' AND FINPKFLD LIKE '" + frm_tabname + "%'");
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
                    SQuery = "Select a.*,b.iname,trim(c.aname) as aname from " + frm_tabname + " a , item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtent_by.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        txtent_dt.Text = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
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
                            sg1_dr["sg1_f1"] = dt.Rows[i]["ponum"].ToString().Trim();
                            sg1_dr["sg1_f2"] = Convert.ToDateTime(dt.Rows[i]["podate"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_f3"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["acode"].ToString().Trim();
                            sg1_dr["sg1_f6"] = dt.Rows[i]["aname"].ToString().Trim();
                            sg1_dr["sg1_f7"] = dt.Rows[i]["unit"].ToString().Trim();
                            sg1_dr["sg1_f8"] = dt.Rows[i]["iqtyin"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["iqtyout"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["genum"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["gedate"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["invno"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["invdate"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["tpt_names"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["mob_no"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["delv_dt"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["MODE_TPT"].ToString().Trim();
                            sg1_dr["sg1_t10"] = "";
                            sg1_dr["sg1_t11"] = "";
                            sg1_dr["sg1_t12"] = "";
                            sg1_dr["sg1_t13"] = "";
                            sg1_dr["sg1_t14"] = "";
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
                    fgen.fin_esales_reps(frm_qstr);
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

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    SQuery = "select trim(ordno) as po_no,to_char(orddt,'dd/mm/yyyy') as po_date,trim(icode) as item_code,trim(iname) as item_name,qtyord,unit,acode ,aname from WBVU_pending_PO where branchcd='" + frm_mbr + "' and trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode) in (" + col1 + ") order by ordno";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (col1.Length <= 0) return;
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in Hidden Field 
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[d]["po_no"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[d]["po_date"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[d]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[d]["item_name"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[d]["acode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[d]["aname"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[d]["unit"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[d]["qtyord"].ToString().Trim();
                    }
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
                    dt = new DataTable();
                    if (col1.Length <= 0) return;
                    if (ViewState["sg1"] != null)
                    {

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
                            sg1_dr["sg1_f7"] = sg1.Rows[i].Cells[19].Text.ToString();
                            sg1_dr["sg1_f8"] = sg1.Rows[i].Cells[20].Text.ToString();
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        dt = new DataTable();
                        //SQuery = "select trim(ordno) as po_no,to_char(orddt,'dd/mm/yyyy') as po_date,trim(icode) as item_code,trim(iname) as item_name,qtyord,unit,acode ,aname from WBVU_pending_PO where branchcd='" + frm_mbr + "' and trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(icode) in (" + col1 + ") order by ordno";
                        SQuery = "select trim(a.ordno) as po_no,to_char(a.orddt,'dd/mm/yyyy') as po_date,trim(a.icode) as item_code,trim(b.iname) as item_name,a.qtyord,b.unit,a.acode from WBVU_pending_PO A inner join item b on trim(b.icode)=trim(a.icode) where a.branchcd='" + frm_mbr + "' and trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.icode) in (" + col1 + ") order by ordno";
                        string mq1;
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
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
                            sg1_dr["sg1_f1"] = dt.Rows[d]["po_no"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["po_date"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["item_code"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["item_name"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["acode"].ToString().Trim();
                            sg1_dr["sg1_f6"] = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where trim(acode)='" + dt.Rows[d]["acode"].ToString().Trim() + "'", "aname");
                            sg1_dr["sg1_f7"] = dt.Rows[d]["unit"].ToString().Trim();
                            sg1_dr["sg1_f8"] = dt.Rows[d]["qtyord"].ToString().Trim();

                            sg1_dr["sg1_t1"] = "";
                            sg1_dr["sg1_t2"] = "";
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
                    //DDBind();
                    cal();
                    //foreach (GridViewRow gr in sg1.Rows)
                    //{
                    //    string hf = ((HiddenField)gr.FindControl("cmd")).Value;
                    //    if (hf != "" && hf != "-")
                    //    {
                    //        ((DropDownList)gr.FindControl("sg1_t11")).Items.FindByText(hf).Selected = true;
                    //    }

                    //    hf = ((HiddenField)gr.FindControl("cmd1")).Value;
                    //    if (hf != "" && hf != "-")
                    //    {
                    //        ((DropDownList)gr.FindControl("sg1_t13")).Items.FindByText(hf).Selected = true;
                    //    }
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
                        foreach (GridViewRow gr in sg1.Rows)
                        {
                            string hf = ((HiddenField)gr.FindControl("cmd")).Value;
                            if (hf != "" && hf != "-")
                            {
                                ((DropDownList)gr.FindControl("sg1_t11")).Items.FindByText(hf).Selected = true;
                            }

                            hf = ((HiddenField)gr.FindControl("cmd1")).Value;
                            if (hf != "" && hf != "-")
                            {
                                ((DropDownList)gr.FindControl("sg1_t13")).Items.FindByText(hf).Selected = true;
                            }

                        }
                    }
                    #endregion
                    setColHeadings();
                    //DDBind();
                    cal();
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "SELECT TRIM(A.VCHNUM) AS ENTRY_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRY_DATE ,A.ICODE AS ITEM_CODE,TRIM(B.INAME) AS ITEM_NAME,TRIM(A.PONUM) AS PO_NO,TO_CHAR(A.PODATE,'DD/MM/YYYY') AS PO_DATE,A.IQTYIN AS QTY_NOS,A.IQTYOUT AS QTY_OUT,A.GENUM,TO_CHAR(A.GEDATE,'DD/MM/YYYY') AS GEDATE,A.INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,A.TPT_NAMES AS TRUCK_NO,A.MOB_NO AS MOBILE_NO,TO_CHAR(A.DELV_DT,'DD/MM/YYYY') AS DELIVERY_DATE,A.MODE_TPT AS MODE_OF_TPT ,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM " + frm_tabname + " A , ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + PrdRange + "  ORDER BY VDD DESC,A.VCHNUM DESC,A.SRNO";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "TACODE")
        {
            mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_HF1");
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            if (hf2.Value == "SG1_ROW_ADD_E")
            {
                hffield.Value = "SG1_ROW_ADD_E";
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Purchase Order", frm_qstr);
            }
            else
            {
                hffield.Value = "SG1_ROW_ADD";
                make_qry_4_popup();
                fgen.Fn_open_mseek("Select Purchase Orders", frm_qstr);
            }
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
            //-------------------------------------------------------
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
                            send_mail(frm_cocd, frm_formID, col1);
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "akshay@pocketdriver.in", "", "", "Hello", "Test Body");
                                send_mail(frm_cocd, frm_formID, col1);
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                        //DDClear();
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
        sg1_dt.Columns.Add(new DataColumn("sg1_f7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f8", typeof(string)));
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
            sg1_dr["sg1_f7"] = "-";
            sg1_dr["sg1_f8"] = "-";

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
            sg1.HeaderRow.Cells[10].Enabled = false;
            e.Row.Cells[10].Enabled = false;
            sg1.HeaderRow.Cells[11].Enabled = false;
            e.Row.Cells[11].Enabled = false;
            sg1.Columns[10].HeaderStyle.Width = 30;
            sg1.Columns[11].HeaderStyle.Width = 30;
            sg1.Columns[12].HeaderStyle.Width = 50;
            sg1.Columns[13].HeaderStyle.Width = 100;
            sg1.Columns[14].HeaderStyle.Width = 120;
            sg1.Columns[15].HeaderStyle.Width = 100;
            sg1.Columns[16].HeaderStyle.Width = 230;
            sg1.Columns[17].HeaderStyle.Width = 100;
            sg1.Columns[18].HeaderStyle.Width = 220;
            sg1.Columns[19].HeaderStyle.Width = 90; //t1
            sg1.Columns[20].HeaderStyle.Width = 90;
            sg1.Columns[21].HeaderStyle.Width = 100;
            sg1.Columns[22].HeaderStyle.Width = 100;
            sg1.Columns[23].HeaderStyle.Width = 100;
            sg1.Columns[24].HeaderStyle.Width = 100;
            sg1.Columns[25].HeaderStyle.Width = 130;
            sg1.Columns[26].HeaderStyle.Width = 130;
            sg1.Columns[27].HeaderStyle.Width = 130;
            sg1.Columns[28].HeaderStyle.Width = 130;
            sg1.Columns[29].HeaderStyle.Width = 130;
            sg1.Columns[30].HeaderStyle.Width = 130;
            sg1.Columns[31].HeaderStyle.Width = 130;
            sg1.Columns[32].HeaderStyle.Width = 180;
            sg1.Columns[33].HeaderStyle.Width = 180;
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

            case "SG1_ROW_ADD":
                if (txtlbl4.Text.Trim().Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Select Inovice First");
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
                    fgen.Fn_open_sseek("Select Bill Of Entry", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Bill Of Entries", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "LICNO";
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

        for (i = 0; i < sg1.Rows.Count; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;  //div 1
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum.Trim().ToUpper();
                oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
                oporow["SRNO"] = i + 1;

                oporow["ponum"] = sg1.Rows[i].Cells[13].Text.Trim().ToUpper();
                oporow["podate"] = sg1.Rows[i].Cells[14].Text.Trim().ToUpper();
                oporow["icode"] = sg1.Rows[i].Cells[15].Text.Trim().ToUpper();
                oporow["acode"] = sg1.Rows[i].Cells[17].Text.Trim().ToUpper();

                oporow["unit"] = sg1.Rows[i].Cells[19].Text.Trim().ToUpper();
                oporow["IQTYIN"] = fgen.make_double(sg1.Rows[i].Cells[20].Text.Trim().ToUpper());
                oporow["IQTYOUT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper());
                oporow["genum"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper();
                //oporow["gedate"] =Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
                oporow["gedate"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim(), vardate);
                oporow["INVNO"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper();
                //oporow["INVdate"] = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
                oporow["INVdate"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim(), vardate);
                oporow["tpt_names"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper();
                oporow["mob_no"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper());
                //oporow["delv_dt"] = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
                oporow["delv_dt"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim(), vardate);
                oporow["MODE_TPT"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper();

                if (txtrmk.Text.Trim().Length > 150)
                {
                    oporow["NARATION"] = txtrmk.Text.Trim().ToUpper().Substring(0, 149);
                }
                else
                {
                    oporow["NARATION"] = txtrmk.Text.Trim().ToUpper();
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
                    oporow["edt_dt"] = vardate;
                }
                oDS.Tables[0].Rows.Add(oporow);
            }
        }
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "AS");
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
    public void cal()
    {
        double coltot = 0;
        for (int sg1r = 0; sg1r < sg1.Rows.Count - 1; sg1r++)
        {
            coltot = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t10")).Text.Trim()) + fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t12")).Text.Trim());
            ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t14")).Text = coltot.ToString();
        }
    }
    //------------------------------------------------------------------------------------
    protected void btninv_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "INV";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Invoice", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    public void send_mail(string cocd, string formID, string appr_Status)
    {
        string emailTo = "", emailCC = "", emailSubj = "";
        System.Text.StringBuilder stb = new System.Text.StringBuilder();


        //string username = ((TextBox)sg1.Rows[Convert.ToInt32(info)].FindControl("txtreason")).Text;
        string username = frm_uname;
        stb.Append("<html><body>");
        //stb.Append("Dear " + username.Split('~')[0].ToString() + ", <br><br>");
        stb.Append("<b>Dear " + fgenCO.chk_co(cocd) + ",</b> <br><br>");
        stb.Append("Entry No. <b>" + frm_vnum.Trim() + "</b>, Dated : <b>" + txtvchdate.Text.Trim() + "</b>  has been Shipped to you by <b>" + frm_uname + " </b> with following item detailed.<br><br>");
        //stb.Append("Item No. : " + sg1.Rows[i].Cells[15].Text.Trim().ToUpper() + "<br>");
        //stb.Append("Product : " + sg1.Rows[i].Cells[15].Text.Trim().ToUpper() + "<br><br>");
        stb.Append("<table>");
        stb.Append("<tr style='color: #FFFFFF; background-color: #0099FF; font-weight: 700; font-family: Arial, Helvetica, sans-serif'>" + "<table border=2><tr><td><b>Invoice No.</b></td><td><b>Invoice Date</b></td><td><b>Item Code</b></td><td><b>Item Name</b></td><td><b>Unit</b></td><td><b>Qty_Sent</b></td><td><b>GRN No.</b></td><td><b>GRN Date</b></td><td><b>Truck No.</b></td><td><b>Mobile No.</b></td><td><b>Delivery Date</b></td></tr>");

        // stb.Append("Respected Sir/Mam,<br/><br/>");
        //stb.Append("Please find SOP By Passed Report Item Wise , the report is based on data position as on " + mq0 + " , " + DateTime.Now.ToShortTimeString().ToString() + "<br/><br/>");                        

        for (int info = 0; info < sg1.Rows.Count - 1; info++)
        {
            stb.Append("<tr>");

            stb.Append("<td>");
            stb.Append(((TextBox)sg1.Rows[Convert.ToInt32(info)].Cells[24].FindControl("sg1_t4")).Text);
            stb.Append("</td>");

            stb.Append("<td>");
            stb.Append(((TextBox)sg1.Rows[Convert.ToInt32(info)].Cells[25].FindControl("sg1_t5")).Text);
            stb.Append("</td>");

            stb.Append("<td>");
            stb.Append(sg1.Rows[Convert.ToInt32(info)].Cells[15].Text.Trim());
            stb.Append("</td>");

            stb.Append("<td>");
            stb.Append(sg1.Rows[Convert.ToInt32(info)].Cells[16].Text.Trim());
            stb.Append("</td>");

            stb.Append("<td>");
            stb.Append(sg1.Rows[Convert.ToInt32(info)].Cells[19].Text.Trim());
            stb.Append("</td>");

            stb.Append("<td>");
            stb.Append(((TextBox)sg1.Rows[Convert.ToInt32(info)].Cells[21].FindControl("sg1_t1")).Text);
            stb.Append("</td>");

            stb.Append("<td>");
            stb.Append(((TextBox)sg1.Rows[Convert.ToInt32(info)].Cells[22].FindControl("sg1_t2")).Text);
            stb.Append("</td>");

            stb.Append("<td>");
            stb.Append(((TextBox)sg1.Rows[Convert.ToInt32(info)].Cells[23].FindControl("sg1_t3")).Text);
            stb.Append("</td>");

            stb.Append("<td>");
            stb.Append(((TextBox)sg1.Rows[Convert.ToInt32(info)].Cells[26].FindControl("sg1_t6")).Text);
            stb.Append("</td>");

            stb.Append("<td>");
            stb.Append(((TextBox)sg1.Rows[Convert.ToInt32(info)].Cells[27].FindControl("sg1_t7")).Text);
            stb.Append("</td>");

            stb.Append("<td>");
            stb.Append(((TextBox)sg1.Rows[Convert.ToInt32(info)].Cells[28].FindControl("sg1_t8")).Text);
            stb.Append("</td>");
            stb.Append("</tr>");
        }

        stb.Append("</table><br/><br/>");
        stb.Append("Thanks & Regards, <br>");
        stb.Append(username.Split('~')[0].ToString() + "<br><br>");
        stb.Append("<b>Note: Please respond to concerned BUYER only as this is the system generated E-Mail. Buyer Name given in the pending details.</b><br>");
        emailSubj = " ASN Master Details";
        stb.Append("</body></html>");
        emailTo = "akshay@pocketdriver.in";
        emailCC = "";

        //emailTo = fgen.seek_iname(frm_qstr, cocd, "SELECT NVL(ACREF,'-') AS ACREF FROM TYPEGRP WHERE ID='SE' AND TRIM(NAME)||'~'||TRIM(TYPE1)='" + username.Trim() + "'", "ACREF");
        //emailCC = fgen.seek_iname(frm_qstr, cocd, "SELECT NVL(EMAILID,'-') AS ACREF FROM EVAS WHERE (USERNAME)='" + frm_uname.Trim() + "'", "ACREF");

        if (stb.ToString().Length > 2 && emailTo.Length > 2)
            fgen.send_mail(cocd, "Tejaxo ERP", emailTo, emailCC, "", emailSubj, stb.ToString());
    }

    //------------------------------------------

}