using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_pay_h : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, SQuery2, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", mq0, mq1, mq2, PDateRange;
    DataTable dt, dt2, dt3, dt4, dt5; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id, Attn_Click = "";
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName, frm_vnum1;
    string frm_tabname, frm_tabname1, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
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
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
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
            typePopup = "Y";
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
                for (int i = 0; i < 4; i++)
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
        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false; btnAttn.Enabled = false;
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
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = true; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlbl4.Enabled = true;
        btnlbl7.Enabled = true;
        btnprint.Disabled = true; btnlist.Disabled = true; btnAttn.Enabled = true;
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
        lblheader.Text = "Pay Data In Hrs";
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "wbpayh";
        frm_tabname1 = "pay";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL8", frm_tabname1);
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

            case "SHFTCODE":
                break;

            case "DEPTTCODE":
                break;

            case "TICODE":
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":

                break;

            case "SG1_ROW_ADD1":
            case "SG1_ROW_ADD_E1":
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
            case "List":
                Type_Sel_query();
                break;

            case "New_E":
            case "List_E":
                SQuery = "select trim(mthnum) as fstr,mthname ,mthnum from mths order by mthsno";
                break;

            //case "Print_E":
            //    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,A.vchnum,to_Char(a.vchdate,'dd/mm/yyyy') as dated,A.type,a.ent_by,a.branchcd,a.grade,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " A where grade='" + txtlbl4.Text.Trim() + "' and branchcd='" + frm_mbr + "' and vchdate " + DateRange + " AND LENGTH(tRIM(nvl(A.LEAVING_dT,'-')))<5 and trim(nvl(a.deptt1,'-'))!='ONEBY' order by vdd desc,A.vchnum desc";
            //    break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD" || btnval == "Print_E")
                {
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,A.vchnum,to_Char(a.vchdate,'dd/mm/yyyy') as dated,A.type,a.ent_by,a.branchcd,a.grade as grade_code,t.name as grade,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " A,type t where trim(a.grade)=trim(t.type1) and t.id='I' and a.grade='" + txtlbl4.Text.Trim() + "' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + "  order by vdd desc,A.vchnum desc";
                }
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
            frm_vty = "10";
            lbl1a.Text = frm_vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Grade", frm_qstr);
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
        frm_vty = "10";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        lbl1a.Text = frm_vty;
        string mon_end_dt = "";
        int month = fgen.make_int(col1) + 1; int year;
        switch (vty)
        {
            case "12":
                mon_end_dt = "01/01/" + (fgen.make_int(frm_myear) + 1).ToString();
                year = fgen.make_int(frm_myear) + 1;
                break;

            case "01":
            case "02":
            case "03":
                mon_end_dt = "01/" + fgen.padlc(month, 2) + "/" + (fgen.make_int(frm_myear) + 1).ToString();
                year = fgen.make_int(frm_myear) + 1;
                break;

            default:
                mon_end_dt = "01/" + fgen.padlc(month, 2) + "/" + frm_myear;
                year = fgen.make_int(frm_myear);
                break;
        }
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + "", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = Convert.ToDateTime(mon_end_dt).AddDays(-1).ToString("dd/MM/yyyy");
        txtlbl2.Text = frm_uname;
        txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl5.Text = "-";
        txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

        int weekly_off = CountSundays(year, Convert.ToInt32(vty));
        int days = Convert.ToInt32(txtvchdate.Text.Substring(0, 2)) - weekly_off;
        txtlbl8.Text = days.ToString();
        txtlbl8.Text = txtvchdate.Text.Substring(0, 2);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();
        SQuery = "select nvl(a.deptt_text,'') as deptt_Text,to_char(a.dtjoin,'dd/mm/yyyy') as dtjoin,nvl(a.appr_by,'-') as appr_by,nvl(a.erpecode,'XXXXXX') as erpecode,nvl(a.empcode,'XXXXXX') as empcode,nvl(a.name,'-') as Name,nvl(a.cardno,'-') as cardno,a.old_empc,nvl(a.fhname,'-') as fhname,A.WRKHOUR,nvl(a.deptt,'-') as deptt,nvl(a.desg,'-') as desg,b.dramt,(b.dramt-b.cramt) as advbal,nvl(a.ded4,0) as tds,nvl(a.ded7,0) as ded7,nvl(a.ded8,0) as ded8,nvl(a.ded9,0) as ded9,a.leaving_Dt,a.ded12,a.curr_ctc,A.ER1+A.ER2+A.ER3+A.ER4+A.ER5+A.ER6+A.ER7+A.ER8+A.ER9+A.ER10+A.ER11+A.ER12+A.ER13+A.ER14+A.ER15+A.ER16+A.ER17+A.ER18+A.ER19+A.ER20 AS earnings,a.fixed_amt from empmas a left outer join advstat b on trim(a.empcode)=trim(b.empcode) and trim(a.grade)=trim(b.grade) and trim(a.branchcd)=trim(b.branchcd) where substr(trim(a.tfr_stat),1,8)<>'TRANSFER' and TRIM(nvl(a.leaving_dt,'-'))='-' and  a.branchcd='" + frm_mbr + "' and a.grade='" + txtlbl4.Text.Trim() + "' and a.dtjoin<=to_DatE('" + txtvchdate.Text + "','dd/mm/yyyy') and substr(nvl(trim(appr_by),'-'),1,3)='[A]' order by a.empcode";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        create_tab();
        sg1_dr = null;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f1"] = dt.Rows[i]["WRKHOUR"].ToString().Trim();
            sg1_dr["sg1_h5"] = dt.Rows[i]["fixed_amt"].ToString().Trim();
            sg1_dr["sg1_h6"] = dt.Rows[i]["advbal"].ToString().Trim();
            sg1_dr["sg1_h7"] = txtvchdate.Text.Substring(0, 2);
            sg1_dr["sg1_h8"] = dt.Rows[i]["empcode"].ToString().Trim();
            sg1_dr["sg1_h9"] = dt.Rows[i]["Name"].ToString().Trim();
            sg1_dr["sg1_h10"] = dt.Rows[i]["fhname"].ToString().Trim();
            sg1_dr["sg1_SrNo"] = i + 1;
            sg1_dt.Rows.Add(sg1_dr);
        }
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        //Cal();
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
            fgen.Fn_open_sseek("Select Grade", frm_qstr);
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
        Cal();
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }
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
            fgen.Fn_open_sseek("Select Grade", frm_qstr);
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
        create_tab();
        create_tab2();
        create_tab3();
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
        ViewState["sg1"] = null;
        ViewState["sg2"] = null;
        ViewState["sg3"] = null;
        setColHeadings();
        doc_addl.Value = "";
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Grade", frm_qstr);
        //fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Grade", frm_qstr);
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
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Pay Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname1 + " a where a.branchcd||a.type||trim(a.mastvch)='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + "Delete");
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
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

            switch (btnval)
            {
                case "New":
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    hffield.Value = "New_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Month", frm_qstr);
                    break;

                case "New_E":
                    if (Convert.ToInt32(col1) > 3 && Convert.ToInt32(col1) <= 12)
                    {

                    }
                    else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                    mq0 = "select distinct vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate from wbpayh where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and to_char(vchdate,'mmyyyy')='" + col1 + frm_myear + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    if (dt.Rows.Count > 0)
                    {
                        fgen.msg("-", "AMSG", "Data Already Entered For Month " + col1 + "/" + frm_myear + "");
                        return;
                    }
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
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
                    lbl1a.Text = "10";
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry No to Delete", frm_qstr);
                    break;

                case "Edit":
                    if (col1 == "") return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
                    lbl1a.Text = "10";
                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry to Edit", frm_qstr);
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
                    txtlbl4.Text = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
                    lbl1a.Text = "10";
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry to Print", frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt,b.name,b.fhname from " + frm_tabname + " a,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl2.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[0]["pent_Dt"].ToString().Trim();
                        txtlbl8.Text = txtvchdate.Text.Substring(0, 2);
                        txtvchnum1.Text = dt.Rows[0]["payno"].ToString().Trim();
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
                            sg1_dr["sg1_h5"] = dt.Rows[i]["actual_rate"].ToString().Trim();
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = dt.Rows[i]["totdays"].ToString().Trim();
                            sg1_dr["sg1_h8"] = dt.Rows[i]["empcode"].ToString().Trim();
                            sg1_dr["sg1_h9"] = dt.Rows[i]["name"].ToString().Trim();
                            sg1_dr["sg1_h10"] = dt.Rows[i]["fhname"].ToString().Trim();
                            sg1_dr["sg1_f1"] = dt.Rows[i]["wrkhrs"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["sunday"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["pay_hrs"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["pay_sal"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["ot"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["days_"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["attn"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["fooding"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["prev_mth_add"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["spl_add"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["tot_add"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["ded_2d"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["late"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["fine"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["sleep"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["oth_ded"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["advance"].ToString().Trim();
                            sg1_dr["sg1_t17"] = dt.Rows[i]["prev_mth_sub"].ToString().Trim();
                            sg1_dr["sg1_t18"] = dt.Rows[i]["spl_sub"].ToString().Trim();
                            sg1_dr["sg1_t19"] = dt.Rows[i]["tot_ded"].ToString().Trim();
                            sg1_dr["sg1_t20"] = dt.Rows[i]["gross"].ToString().Trim();
                            sg1_dr["sg1_t21"] = dt.Rows[i]["ot_hrs"].ToString().Trim();
                            sg1_dr["sg1_t22"] = dt.Rows[i]["pr_hrs"].ToString().Trim();
                            sg1_dr["sg1_t23"] = dt.Rows[i]["fooding_hrs"].ToString().Trim();
                            sg1_dr["sg1_t24"] = dt.Rows[i]["tot_2d_hrs"].ToString().Trim();
                            sg1_dr["sg1_t25"] = dt.Rows[i]["tot_late_hrs"].ToString().Trim();
                            sg1_dr["sg1_t26"] = dt.Rows[i]["tot_fine_hrs"].ToString().Trim();
                            sg1_dr["sg1_t27"] = dt.Rows[i]["tot_sleep_hrs"].ToString().Trim();
                            sg1_dr["sg1_t28"] = dt.Rows[i]["tot_other_ded_hrs"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
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
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", txtlbl4.Text.Trim());
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", col1);
                    frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_pay_reps(frm_qstr);
                    break;

                case "List":
                    if (col1 == "") return;
                    frm_vty = "10";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                    txtlbl4.Text = col1;
                    hffield.Value = "List_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Month", frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
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

                case "TICODE":
                    break;

                case "SHFTCODE":
                    if (col1.Length <= 0) return;
                    break;

                case "DEPTTCODE":
                    if (col1.Length <= 0) return;
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

                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[14].Text.ToString();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[15].Text.ToString();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[16].Text.ToString();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[17].Text.ToString();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[18].Text.ToString();
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
                            sg1_dr["sg1_t22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim();
                            sg1_dr["sg1_t23"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim();
                            sg1_dr["sg1_t24"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text.Trim();
                            sg1_dr["sg1_t25"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text.Trim();
                            sg1_dr["sg1_t26"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text.Trim();
                            sg1_dr["sg1_t27"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t27")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        PDateRange = "between to_date('" + frm_CDT1 + "','dd/mm/yyyy')-200  and to_date('" + frm_CDT2 + "','dd/mm/yyyy')";
                        dt = new DataTable();
                        // SQuery = "select distinct a.vchnum as fstr,B.INAME as Item_Name,b.cpartno as Part_No,a.icode as ERP_Code,A.vchnum AS Job_No,to_char(A.vchdate,'dd/mm/yyyy') as Dated,A.QTY as Job_Qty,a.acode,a.status,a.JSTATUS from costestimate A,ITEM B  WHERE trim(A.ICODE)=trim(B.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='30' and trim(nvl(a.app_by,'-'))<>'-' and nvl(a.status,'N')<>'Y' and nvl(a.jstatus,'N')<>'Y' and trim(nvl(a.enqno,'N'))<>'Y' and b.pageno=1 and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' and A.SRNO=0  order by Dated desc ,A.vchnum desc"; ;
                        SQuery = "select trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy') as fstr, b.iname as Item_Name,b.cpartno as Part_No,a.erp_Code,a.job_no,to_char(a.Dated,'dd/mm/yyyy') as dated,sum(a.Job_Qty) as Job_qty,a.acode,sum(a.prodn) as Prodn_qty,MAX(A.PRODDT) AS PROD_DT from (select a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,0 as prodn,a.acode,null as proddt from costestimate A WHERE  a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + PDateRange + " and A.SRNO=0 AND trim(nvl(a.status,'N'))<>'Y' union all select a.icode as ERP_Code,A.enqno AS Job_No,A.enqdt as Dated,0 as Job_Qty,to_number(a.col4) as prodn,a.acode,A.VCHDATE from costestimate A WHERE a.branchcd='" + frm_mbr + "' and a.type='60' and a.vchdate  " + PDateRange + " )a, item b where  trim(A.erp_Code)=trim(B.icode) and trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy')='" + col1 + "' group by b.iname,b.cpartno,a.erp_Code,a.job_no,a.dated,a.acode having sum(a.Job_Qty)-sum(a.prodn)>0 order by b.iname";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";
                            sg1_dr["sg1_f1"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[d]["erp_code"].ToString().Trim() + "'", "CL");
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = dt.Rows[d]["Job_No"].ToString().Trim();
                            sg1_dr["sg1_f4"] = Convert.ToDateTime(dt.Rows[d]["dated"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_t1"] = dt.Rows[d]["ERP_Code"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[d]["Item_Name"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[d]["Part_No"].ToString().Trim();
                            string acode = fgen.seek_iname(frm_qstr, frm_cocd, "select * from (select acode,max(irate) as irt from somas where branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and icat<>'Y' and trim(icode)='" + dt.Rows[d]["ERP_Code"].ToString().Trim() + "' group by acode) order by irt desc", "acode");
                            sg1_dr["sg1_t6"] = fgen.seek_iname(frm_qstr, frm_cocd, "select * from (select acode,max(irate) as irt from somas where branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and icat<>'Y' and trim(icode)='" + dt.Rows[d]["ERP_Code"].ToString().Trim() + "' group by acode) order by irt desc", "irt");
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t4")).Focus();
                    #endregion
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD_E":
                    dt = new DataTable();
                    PDateRange = "between to_date('" + frm_CDT1 + "','dd/mm/yyyy')-200 and to_date('" + frm_CDT2 + "','dd/mm/yyyy')";
                    SQuery = "select trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy') as fstr,a.ERP_Code, b.iname as Item_Name,b.cpartno,a.erp_Code as Part_No,a.job_no,to_char(a.Dated,'dd/mm/yyyy') as dated,sum(a.Job_Qty) as Job_qty,a.acode,sum(a.prodn) as Prodn_qty,MAX(A.PRODDT) AS PROD_DT from (select a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,0 as prodn,a.acode,null as proddt from costestimate A WHERE  a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + PDateRange + " and A.SRNO=0 AND trim(nvl(a.status,'N'))<>'Y' union all select a.icode as ERP_Code,A.enqno AS Job_No,A.enqdt as Dated,0 as Job_Qty,to_number(a.col4) as prodn,a.acode,A.VCHDATE from costestimate A WHERE a.branchcd='" + frm_mbr + "' and a.type='60' and a.vchdate  " + PDateRange + " )a, item b where  trim(A.erp_Code)=trim(B.icode) and trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy')='" + col1 + "' group by b.iname,b.cpartno,a.erp_Code,a.job_no,a.dated,a.acode having sum(a.Job_Qty)-sum(a.prodn)>0 order by b.iname";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[0]["Job_No"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[0]["dated"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = dt.Rows[0]["ERP_Code"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = dt.Rows[0]["Item_Name"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = dt.Rows[0]["Part_No"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t6")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select * from (select acode,max(irate) as irt from somas where branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and icat<>'Y' and trim(icode)='" + dt.Rows[0]["ERP_Code"].ToString().Trim() + "' group by acode) order by irt desc", "irt");
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[0]["erp_code"].ToString().Trim() + "'", "CL");
                        setColHeadings();
                    }
                    break;

                case "SG1_ROW_ADD1":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    //if (ViewState["sg1"] != null)
                    //{
                    //    dt = new DataTable();
                    //    sg1_dt = new DataTable();
                    //    dt = (DataTable)ViewState["sg1"];
                    //    z = dt.Rows.Count - 1;
                    //    sg1_dt = dt.Clone();
                    //    sg1_dr = null;
                    //    for (i = 0; i < dt.Rows.Count - 1; i++)
                    //    {
                    //        sg1_dr = sg1_dt.NewRow();
                    //        sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                    //        sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
                    //        sg1_dr["sg1_h2"] = dt.Rows[i]["sg1_h2"].ToString();
                    //        sg1_dr["sg1_h3"] = dt.Rows[i]["sg1_h3"].ToString();
                    //        sg1_dr["sg1_h4"] = dt.Rows[i]["sg1_h4"].ToString();
                    //        sg1_dr["sg1_h5"] = dt.Rows[i]["sg1_h5"].ToString();
                    //        sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                    //        sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                    //        sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                    //        sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                    //        sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();

                    //        sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                    //        sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                    //        sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                    //        sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                    //        sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                    //        sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                    //        sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                    //        sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                    //        sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                    //        sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                    //        sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                    //        sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                    //        sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                    //        sg1_dt.Rows.Add(sg1_dr);
                    //}
                    string stage = "0";
                    stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text;
                    dt = new DataTable();
                    //if (col1.Length > 6) SQuery = "select * from evas where trim(userid) in (" + col1 + ")";
                    //else SQuery = "select * from evas where trim(userid)='" + col1 + "'";
                    SQuery = "select distinct b.Iname as iname,a.Icode as iCode,b.Cpartno,a.vchnum,a.qty,to_char(a.vchdate,'dd/mm/yyyy')as vchdate,trim(a.Icode)||'.'||trim(a.vchnum) as fstr,a.col17 from costestimate a, item b,itwstage c where trim(a.icode)=trim(b.icode) and a.type='30' and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.status='N' and c.stagec='" + stage + "' and trim(a.Icode)||'.'||trim(a.vchnum) in (" + col1 + ") order by trim(a.Icode)||'.'||trim(a.vchnum)";
                    //SQuery = "select  a.type1 as fstr,A.NAME,A.type1,B.CNT AS ITEMS from type A,(select DISTINCT stagec,count(icode) AS CNT from itwstage  GROUP BY STAGEC) B where A.id='K' AND A.TYPE1=B.STAGEC and a.type1 in("+col1 +") order by A.TYPE1";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        sg1.Rows[d].Cells[18].Text = dt.Rows[d]["cpartno"].ToString().Trim();

                        //sg1_dr["sg1_f6"] = dt.Rows[d]["cpartno"].ToString().Trim();
                        sg1.Rows[d].Cells[17].Text = dt.Rows[d]["vchnum"].ToString().Trim();
                        sg1.Rows[d].Cells[16].Text = dt.Rows[d]["vchdate"].ToString().Trim();

                        ((TextBox)sg1.Rows[d].FindControl("sg1_t3")).Text = dt.Rows[d]["iname"].ToString().Trim();
                        sg1.Rows[d].Cells[18].Text = dt.Rows[d]["Cpartno"].ToString().Trim();
                        sg1.Rows[d].Cells[22].Width = 70;
                        // sg1_dr["sg1_t2"] = "";
                        //sg1_dr["sg1_t3"] = "";
                        ((TextBox)sg1.Rows[d].FindControl("sg1_t6")).Text = dt.Rows[d]["qty"].ToString().Trim();
                        ((TextBox)sg1.Rows[d].FindControl("sg1_t21")).Text = dt.Rows[d]["iCode"].ToString().Trim();
                        ((TextBox)sg1.Rows[d].FindControl("sg1_t4")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select rate from type where id='K' and type1='" + stage + "'", "rate");
                        ((TextBox)sg1.Rows[d].FindControl("sg1_t5")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select excrate from type where id='K' and type1='" + stage + "'", "excrate");


                        string stagename = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STAGEC FROM ITWSTAGE WHERE SRNO>(SELECT SRNO FROM ITWSTAGE WHERE ICODE='90020488' AND STAGEC='" + stage + "' AND ROWNUM<=1)AND ROWNUM<=1 ORDER BY SRNO", "stagec");

                        ((TextBox)sg1.Rows[d].FindControl("sg1_t36")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='K' and type1='" + stagename + " '", "name");
                        //sg1_dr["sg1_t4"] = dt.Rows[d]["Type1"].ToString().Trim();
                        //sg1_dr["sg1_t5"] = "";
                        //sg1_dr["sg1_t7"] = "";
                        // ((TextBox)sg1.Rows[d].FindControl("sg1_t1")).Text = dt.Rows[d]["qty"].ToString().Trim();
                        //sg1_dr["sg1_t8"] = "";
                        //sg1_dr["sg1_t9"] = "";
                        //sg1_dr["sg1_t10"] = "";
                        //  sg1_dr["sg1_t11"] = dt.Rows[d]["icode"].ToString().Trim();
                        //sg1_dr["sg1_t12"] = "";
                        //sg1_dr["sg1_t13"] = "";
                        //sg1_dr["sg1_t14"] = "";
                        //sg1_dr["sg1_t15"] = "";
                        //sg1_dr["sg1_t16"] = "";

                        // sg1_dt.Rows.Add(sg1_dr);
                    }

                    //sg1_add_blankrows();

                    //ViewState["sg1"] = sg1_dt;
                    //sg1.DataSource = sg1_dt;
                    //sg1.DataBind();
                    //dt.Dispose(); sg1_dt.Dispose();
                    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
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
                            sg1_dr["sg1_srno"] = sg1.Rows[i].Cells[13].Text.Trim();
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[14].Text.Trim();
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[17].Text.Trim();
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
                            sg1_dr["sg1_t22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim();
                            sg1_dr["sg1_t23"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim();
                            sg1_dr["sg1_t24"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text.Trim();
                            sg1_dr["sg1_t25"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text.Trim();
                            sg1_dr["sg1_t26"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text.Trim();
                            sg1_dr["sg1_t27"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t27")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        setColHeadings();
                    }
                    #endregion
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = "10";
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
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

                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        frm_vnum1 = "000000";
                        save_fun();
                        save_fun2();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);

                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnum.Text.Trim();
                            frm_vnum1 = txtvchnum1.Text.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            save_it = "Y";
                            if (save_it == "Y")
                            {
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                frm_vnum1 = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname1, doc_nf.Value, "date_", frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        save_fun2();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "update " + frm_tabname1 + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(date_,'dd/mm/yyyy')='" + frm_mbr + frm_vty + txtvchnum1.Text.Trim() + txtvchdate.Text + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, frm_tabname1);

                        save_fun3();

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "delete from " + frm_tabname1 + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(date_,'dd/mm/yyyy')='DD" + frm_vty + txtvchnum1.Text.Trim() + txtvchdate.Text + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); doc_addl.Value = "";
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
        sg1_dt.Columns.Add(new DataColumn("sg1_h11", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h13", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h14", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h16", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h20", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h21", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h22", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h23", typeof(string)));
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
        sg1_dt.Columns.Add(new DataColumn("sg1_t25", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t26", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t27", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t28", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t29", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t30", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t31", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        // Hidden Field
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
    public void sg1_add_blankrows()
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
        sg1_dr["sg1_t22"] = "-";
        sg1_dr["sg1_t23"] = "-";
        sg1_dr["sg1_t24"] = "-";
        sg1_dr["sg1_t25"] = "-";
        sg1_dr["sg1_t26"] = "-";
        sg1_dr["sg1_t27"] = "-";
        sg1_dr["sg1_t28"] = "-";
        sg1_dr["sg1_t29"] = "-";
        sg1_dr["sg1_t30"] = "-";
        sg1_dr["sg1_t31"] = "-";
        sg1_dt.Rows.Add(sg1_dr);
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
            sg1.Columns[4].HeaderStyle.Width = 80;
            sg1.Columns[6].HeaderStyle.Width = 70;
            sg1.Columns[7].HeaderStyle.Width = 70;
            sg1.Columns[8].HeaderStyle.Width = 150;
            sg1.Columns[9].HeaderStyle.Width = 150;
            sg1.Columns[12].HeaderStyle.Width = 50;
            sg1.Columns[13].HeaderStyle.Width = 60;
            sg1.Columns[18].HeaderStyle.Width = 120;
            sg1.Columns[21].HeaderStyle.Width = 120;
            sg1.Columns[22].HeaderStyle.Width = 160;
            sg1.Columns[24].HeaderStyle.Width = 120;
            sg1.Columns[32].HeaderStyle.Width = 120;
            sg1.Columns[36].HeaderStyle.Width = 120;
            sg1.Columns[41].HeaderStyle.Width = 120;
            sg1.Columns[43].HeaderStyle.Width = 70;
            sg1.Columns[44].HeaderStyle.Width = 150;
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
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                    //fgen.Fn_open_mseek("Select Item", frm_qstr);
                }
                break;

            case "SG1_ROW_ADD1":
                if (sg1.Rows[Convert.ToInt32(index)].Cells[13].Text.Trim().Length > 1)
                {
                    hf1.Value = index.ToString();
                    hffield.Value = "SG1_ROW_ADD1";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select item", frm_qstr);
                }
                else
                {
                    fgen.msg("-", "AMSG", "Please Select Stage First!!");
                    return;
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
                dt = new DataTable();
                sg2_dt = new DataTable();
                dt = (DataTable)ViewState["sg2"];
                z = dt.Rows.Count - 1;
                sg2_dt = dt.Clone();
                sg2_dr = null;
                i = 0;
                for (i = 0; i < sg2.Rows.Count; i++)
                {
                    sg2_dr = sg2_dt.NewRow();
                    sg2_dr["sg2_srno"] = (i + 1);
                    sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                    sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                    sg2_dt.Rows.Add(sg2_dr);
                }
                sg2_add_blankrows();
                ViewState["sg2"] = sg2_dt;
                sg2.DataSource = sg2_dt;
                sg2.DataBind();
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
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SHFTCODE";
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
        hffield.Value = "DEPTTCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg1.Rows.Count; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum.Trim().ToUpper();
            oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow["SRNO"] = i + 1;
            oporow["grade"] = txtlbl4.Text.Trim().ToUpper();
            oporow["empcode"] = sg1.Rows[i].Cells[7].Text.Trim().ToUpper();
            oporow["totdays"] = txtlbl8.Text.Trim();
            oporow["sunday"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper());
            oporow["pay_hrs"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper());
            oporow["pay_sal"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper());
            oporow["ot"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper());
            oporow["days_"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper());
            oporow["attn"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper());
            oporow["fooding"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper());
            oporow["prev_mth_add"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper());
            oporow["spl_add"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper());
            oporow["tot_add"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().ToUpper());
            oporow["ded_2d"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim().ToUpper());
            oporow["late"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim().ToUpper());
            oporow["fine"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim().ToUpper());
            oporow["sleep"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim().ToUpper());
            oporow["oth_ded"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim().ToUpper());
            oporow["advance"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().ToUpper());
            oporow["prev_mth_sub"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim().ToUpper());
            oporow["spl_sub"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim().ToUpper());
            oporow["tot_ded"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim().ToUpper());
            oporow["gross"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim().ToUpper());
            oporow["payno"] = frm_vnum1; // pay table vchnum and vchdate;
            oporow["ot_hrs"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim().ToUpper());
            oporow["pr_hrs"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim().ToUpper());
            oporow["fooding_hrs"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim().ToUpper());
            oporow["tot_2d_hrs"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text.Trim().ToUpper());
            oporow["tot_late_hrs"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text.Trim().ToUpper());
            oporow["tot_fine_hrs"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text.Trim().ToUpper());
            oporow["tot_sleep_hrs"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t27")).Text.Trim().ToUpper());
            oporow["tot_other_ded_hrs"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t28")).Text.Trim().ToUpper());
            oporow["actual_rate"] = fgen.make_double(sg1.Rows[i].Cells[4].Text.Trim().ToUpper());
            oporow["wrkhrs"] = fgen.make_double(sg1.Rows[i].Cells[13].Text.Trim().ToUpper());
            if (edmode.Value == "Y")
            {
                oporow["ent_by"] = ViewState["entby"].ToString();
                oporow["ent_dt"] = ViewState["entdt"].ToString();
                oporow["edt_by"] = frm_uname;
                oporow["edt_dt"] = vardate;
            }
            else
            {
                oporow["ent_by"] = frm_uname;
                oporow["ent_dt"] = vardate;
                oporow["edt_by"] = "-";
                oporow["edt_dt"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        dt2 = new DataTable();
        mq0 = "select branchcd,trim(grade) as grade,trim(empcode) as empcode,pfcut,esicut,cutvpf,er1,er2,er3,er4,er5,er6,er7,er8,er9,er10,er11,er12,er13,er14,er15,er16,er17,er18,er19,er20,ded1,ded2,ded3,ded4,ded5,ded6,ded7,ded8,ded9,ded10,ded11,ded12,ded13,ded14,ded15,ded16,ded17,ded18,ded19,ded20,leaving_dt,erpecode,fixed_amt,deptt,d_o_b,er1+er2+er3+er4+er5+er6+er7+er8+er9+er10+er11+er12+er13+er14+er15+er16+er17+er18+er19+er20 as earnings from empmas where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' order by empcode";
        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

        double Gross = 0, Sal_FixedAmt = 0, Gross_Earn = 0;
        double PF_Limit = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "SELECT MAX_LMT FROM WB_SELMAST WHERE branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and ed_fld='DED1' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/ order by morder", "MAX_LMT"));
        string selvch = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and ed_fld='DED1' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/", "fstr");
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            oporow2 = oDS2.Tables[0].NewRow();
            oporow2["BRANCHCD"] = frm_mbr;
            oporow2["TYPE"] = frm_vty;
            oporow2["vchnum"] = frm_vnum1.Trim().ToUpper();
            oporow2["date_"] = txtvchdate.Text.Trim().ToUpper();
            oporow2["SRNO"] = i + 1;
            oporow2["grade"] = txtlbl4.Text.Trim().ToUpper();
            oporow2["empcode"] = sg1.Rows[i].Cells[7].Text.Trim().ToUpper();

            Gross = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim().ToUpper());
            Sal_FixedAmt = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "fixed_amt"));

            if (Gross > Sal_FixedAmt)
            {
                Gross_Earn = Sal_FixedAmt;
            }
            else
            {
                Gross_Earn = Gross;
            }
            oporow2["totdays"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_h7")).Text.Trim().ToUpper());
            if (Sal_FixedAmt == 0)
            {
                Sal_FixedAmt = 1;
            }
            oporow2["WORKDAYS"] = (Gross_Earn / Sal_FixedAmt) * fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_h7")).Text.Trim().ToUpper());

            //EARNINGS
            if (Gross_Earn <= 20500)
            {
                oporow2["ER1"] = Math.Round(Gross_Earn * 0.8, 2);
                oporow2["ER2"] = Math.Round(Gross_Earn - fgen.make_double(oporow2["ER1"].ToString()), 2);
            }
            else
            {
                oporow2["ER1"] = Math.Round(Gross_Earn * 0.7, 2);
                oporow2["ER2"] = Math.Round(fgen.make_double(oporow2["ER1"].ToString()) * 0.3, 2);
            }
            oporow2["ER3"] = Math.Round(Gross_Earn - fgen.make_double(oporow2["ER1"].ToString()) - fgen.make_double(oporow2["ER2"].ToString()), 2);
            oporow2["TOTERN"] = fgen.make_double(oporow2["ER1"].ToString()) + fgen.make_double(oporow2["ER2"].ToString()) + fgen.make_double(oporow2["ER3"].ToString());
            //------------
            oporow2["DEPTT"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "DEPTT");
            oporow2["DEPTT2"] = "-";
            oporow2["DEPTT1"] = "-";
            oporow2["DESG"] = "-";
            oporow2["DESCGRD"] = "-";
            oporow2["SECTION_"] = "-";
            oporow2["TRADE"] = "N";
            oporow2["ESINO"] = "-";
            oporow2["PRESENT"] = 0;
            oporow2["SHL"] = 0;
            oporow2["MNTS"] = 0;
            oporow2["CL"] = 0;
            oporow2["EL"] = 0;
            oporow2["SL"] = 0;
            oporow2["LWP"] = 0;
            oporow2["ESIL"] = 0;
            oporow2["ABSENT"] = 0;
            oporow2["LATE"] = 0;
            oporow2["HOURS"] = 0;
            oporow2["OT"] = 0;
            oporow2["NIGHTS"] = 0;
            oporow2["CPL"] = 0;
            oporow2["GWA"] = (Gross_Earn / Sal_FixedAmt) * fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_h7")).Text.Trim().ToUpper());
            oporow2["GWARR"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er1");
            oporow2["SD"] = "-";
            oporow2["GWAMNT"] = fgen.make_double(oporow2["ER1"].ToString());
            oporow2["PRDINC"] = 0;
            oporow2["ESIGW"] = 0;
            oporow2["PFCUT"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "pfcut");
            oporow2["ESICUT"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "esicut");
            oporow2["PFNO"] = "-";

            oporow2["ERATE1"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er1");
            oporow2["ERATE2"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er2");
            oporow2["ERATE3"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er3");
            oporow2["ERATE4"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er4");
            oporow2["ERATE5"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er5");
            oporow2["ERATE6"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er6");
            oporow2["ERATE7"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er7");
            oporow2["ERATE8"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er8");
            oporow2["ERATE9"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er9");
            oporow2["ERATE10"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er10");
            oporow2["ERATE11"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er11");
            oporow2["ERATE12"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er12");
            oporow2["ERATE13"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er13");
            oporow2["ERATE14"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er14");
            oporow2["ERATE15"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er15");
            oporow2["ERATE16"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er16");
            oporow2["ERATE17"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er17");
            oporow2["ERATE18"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er18");
            oporow2["ERATE19"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er19");
            oporow2["ERATE20"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er20");

            oporow2["DRATE1"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded1");
            oporow2["DRATE2"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded2");
            oporow2["DRATE3"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded3");
            oporow2["DRATE4"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded4");
            oporow2["DRATE5"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded5");
            oporow2["DRATE6"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded6");
            oporow2["DRATE7"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded7");
            oporow2["DRATE8"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded8");
            oporow2["DRATE9"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded9");
            oporow2["DRATE10"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded10");
            oporow2["DRATE11"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded11");
            oporow2["DRATE12"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded12");
            oporow2["DRATE13"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded13");
            oporow2["DRATE14"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded14");
            oporow2["DRATE15"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded15");
            oporow2["DRATE16"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded16");
            oporow2["DRATE17"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded17");
            oporow2["DRATE18"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded18");
            oporow2["DRATE19"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded19");
            oporow2["DRATE20"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded20");

            oporow2["ER4"] = 0;
            oporow2["ER5"] = 0;
            oporow2["ER6"] = 0;
            oporow2["ER7"] = 0;
            oporow2["ER8"] = 0;
            oporow2["ER9"] = 0;
            oporow2["ER10"] = 0;
            oporow2["ER11"] = 0;
            oporow2["ER12"] = 0;
            oporow2["ER13"] = 0;
            oporow2["ER14"] = 0;
            oporow2["ER15"] = 0;
            oporow2["ER16"] = 0;
            oporow2["ER17"] = 0;
            oporow2["ER18"] = 0;
            oporow2["ER19"] = 0;
            oporow2["ER20"] = 0;
            oporow2["AR1"] = 0;
            oporow2["AR2"] = 0;
            oporow2["AR3"] = 0;
            oporow2["AR4"] = 0;
            oporow2["AR5"] = 0;
            oporow2["AR6"] = 0;
            oporow2["AR7"] = 0;
            oporow2["AR8"] = 0;
            oporow2["AR9"] = 0;
            oporow2["AR10"] = 0;
            oporow2["AR11"] = 0;
            oporow2["AR12"] = 0;
            oporow2["AR13"] = 0;
            oporow2["AR14"] = 0;
            oporow2["AR15"] = 0;
            oporow2["AR16"] = 0;
            oporow2["AR17"] = 0;
            oporow2["AR18"] = 0;
            oporow2["AR19"] = 0;
            oporow2["AR20"] = 0;
            oporow2["DED1"] = 0;
            oporow2["DED2"] = 0;
            oporow2["DED3"] = 0;
            oporow2["DED4"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded4");
            oporow2["DED5"] = 0;
            oporow2["DED6"] = 0;
            oporow2["DED7"] = 0;
            oporow2["DED8"] = 0;
            oporow2["DED9"] = 0;
            oporow2["DED10"] = 0;
            oporow2["DED11"] = 0;
            oporow2["DED12"] = 0;
            oporow2["DED13"] = 0;
            oporow2["DED14"] = 0;
            oporow2["DED15"] = 0;
            oporow2["DED16"] = 0;
            oporow2["DED17"] = 0;
            oporow2["DED18"] = 0;
            oporow2["DED19"] = 0;
            oporow2["DED20"] = 0;
            oporow2["TOTDED"] = 0; //SAVING THROUGH SAVE_FUN3()
            oporow2["NETSLRY"] = 0; //SAVING THROUGH SAVE_FUN3()
            oporow2["COINS"] = 0;
            oporow2["ATINC"] = 0;
            oporow2["ESI"] = 0;
            oporow2["CEPF"] = 0;
            oporow2["STATUS"] = "-";
            oporow2["OFFDAYS"] = 0;
            oporow2["CUTVPF"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "cutvpf");
            oporow2["TOTSAL"] = fgen.make_double(oporow2["ERATE1"].ToString()) + fgen.make_double(oporow2["ERATE2"].ToString()) + fgen.make_double(oporow2["ERATE3"].ToString()) + fgen.make_double(oporow2["ERATE4"].ToString()) + fgen.make_double(oporow2["ERATE5"].ToString()) + fgen.make_double(oporow2["ERATE6"].ToString()) + fgen.make_double(oporow2["ERATE7"].ToString()) + fgen.make_double(oporow2["ERATE8"].ToString()) + fgen.make_double(oporow2["ERATE9"].ToString()) + fgen.make_double(oporow2["ERATE10"].ToString()) + fgen.make_double(oporow2["ERATE11"].ToString()) + fgen.make_double(oporow2["ERATE12"].ToString()) + fgen.make_double(oporow2["ERATE13"].ToString()) + fgen.make_double(oporow2["ERATE14"].ToString()) + fgen.make_double(oporow2["ERATE15"].ToString()) + fgen.make_double(oporow2["ERATE16"].ToString()) + fgen.make_double(oporow2["ERATE17"].ToString()) + fgen.make_double(oporow2["ERATE18"].ToString()) + fgen.make_double(oporow2["ERATE19"].ToString()) + fgen.make_double(oporow2["ERATE20"].ToString());
            oporow2["ADVANCE"] = 0;
            oporow2["WRKHRS"] = fgen.make_double(sg1.Rows[i].Cells[13].Text.Trim().ToUpper());
            oporow2["TDS"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ded4");
            oporow2["VONC"] = 0;
            oporow2["VERO"] = 0;
            oporow2["REFR"] = 0;
            oporow2["OTH2"] = fgen.make_double(oporow2["er2"].ToString());
            oporow2["OTH3"] = fgen.make_double(oporow2["er3"].ToString());
            oporow2["ROTH2"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er2");
            oporow2["ROTH3"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "er3");
            oporow2["LEAVING_DT"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "LEAVING_DT");
            oporow2["SALHOLD"] = "-";
            oporow2["SALPAID"] = "-";
            oporow2["LEAVING_WHY"] = "-";
            oporow2["ERPECODE"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "ERPECODE");
            oporow2["ADDLESI"] = 0;
            oporow2["BON_RATE"] = 0;
            oporow2["CALC_PF_WG"] = 0;
            oporow2["PADVBAL"] = 0;
            oporow2["COFF"] = 0;
            oporow2["BON_ELIG"] = 0;
            oporow2["AR21"] = 0;
            oporow2["TNUM"] = 0;
            oporow2["TNUM2"] = 0;
            oporow2["LOAN_DED"] = 0;
            oporow2["TMJ"] = 0;
            oporow2["TML"] = "N";
            oporow2["LTA"] = 0;
            oporow2["AR22"] = 0;
            oporow2["MLEV"] = 0;
            oporow2["LMT_PF"] = PF_Limit;
            oporow2["LEAVING_TXT"] = "-";
            oporow2["NPDAYS"] = 0;
            oporow2["BONMINWG"] = 0;
            oporow2["ESI_WAGE"] = 0;//to be asked
            oporow2["MASTVCH"] = frm_vnum + txtvchdate.Text; // WBPAYH TABLE'S VCHNUM,VCHDATE
            mq0 = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "d_o_b");
            oporow2["AGE"] = fgen.make_double(txtvchdate.Text.Substring(6, 4)) - fgen.make_double(mq0.Substring(6, 4));
            oporow2["SELVCH"] = selvch;
            oporow2["ent_by"] = frm_uname;
            oporow2["ent_date"] = vardate;
            oDS2.Tables[0].Rows.Add(oporow2);
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun3()
    {
        string PF_ER = "", ESI_ER = "", WF_ER = "", PF_YN = "", ESI_YN = "", Apply_15000_PFLimit = ""; double Tot_Earnings = 0, Tot_Ded = 0, Net_Sal = 0;
        double PFWage = 0, WFWage = 0, DED1 = 0, DED3 = 0, DED6 = 0;
        double PF_AMT_CS = 0, PF_RT_CS = 0, PF_RT_ES = 0, PF_SAL = 0, ESI_SAL = 0, ESI_RT_CS = 0, ESI_RT_ES = 0, ESI_AMT_CS = 0, WF_AMT_CS = 0, WF_RT_CS = 0, WF_RT_ES = 0;

        #region
        dt2 = new DataTable();
        dt2 = fgen.getdata(frm_qstr, frm_cocd, "select trim(empcode) as empcode,mnthinc from empmas where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text + "'");

        DataTable dtPF_ER = new DataTable();
        dtPF_ER = fgen.getdata(frm_qstr, frm_cocd, "select rtrim(xmlagg(xmlelement(e,replace(TRIM(ED_FLD) ,'-',null)||'+')).extract('//text()').extract('//text()'),'+') as earnings from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and pf_yn='Y' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/");

        DataTable dtSelmas = new DataTable();
        dtSelmas = fgen.getdata(frm_qstr, frm_cocd, "SELECT RATE,PF_DIV,ED_FLD,ED_NAME,ESI_DIV,WF_DIV,EMPR_RATE,MAX_LMT FROM WB_SELMAST WHERE branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and ed_fld like 'DED%' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/ order by morder");

        if (dtPF_ER.Rows.Count > 0)
        {
            PF_ER = dtPF_ER.Rows[0]["earnings"].ToString().Trim();
            if (PF_ER == "")
            {
                PF_ER = "0";
            }
        }
        else
        {
            PF_ER = "0";
        }
        string PF_Div = fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "PF_DIV");
        if (PF_Div == "PLEASE SELECT")
        {

        }
        double PF_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "RATE")) / 100;
        double PF_Empr_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "EMPR_RATE")) / 100;
        double PF_Limit = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "MAX_LMT"));
        string PF_Formula = "round(((" + PF_ER + ")/" + PF_Div + ")*WORKDAYS,2) as DED1";

        DataTable dtESI_ER = new DataTable();
        dtESI_ER = fgen.getdata(frm_qstr, frm_cocd, "select rtrim(xmlagg(xmlelement(e,replace(TRIM(ED_FLD) ,'-',null)||'+')).extract('//text()').extract('//text()'),'+') as earnings from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and esi_yn='Y' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/");

        if (dtESI_ER.Rows.Count > 0)
        {
            ESI_ER = dtESI_ER.Rows[0]["earnings"].ToString().Trim();
            if (ESI_ER == "")
            {
                ESI_ER = "0";
            }
        }
        else
        {
            ESI_ER = "0";
        }
        string ESI_Div = fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED3'", "ESI_DIV");
        double ESI_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED3'", "RATE")) / 100;
        double ESI_Empr_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED3'", "EMPR_RATE")) / 100;
        string ESI_Formula = "round(((" + ESI_ER + ")/" + ESI_Div + ")*WORKDAYS,2) as DED3";

        DataTable dtWF_ER = new DataTable();
        dtWF_ER = fgen.getdata(frm_qstr, frm_cocd, "select rtrim(xmlagg(xmlelement(e,replace(TRIM(ED_FLD) ,'-',null)||'+')).extract('//text()').extract('//text()'),'+') as earnings from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and wf_yn='Y' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/");
        if (dtWF_ER.Rows.Count > 0)
        {
            WF_ER = dtWF_ER.Rows[0]["earnings"].ToString().Trim();
            if (WF_ER == "")
            {
                WF_ER = "0";
            }
        }
        else
        {
            WF_ER = "0";
        }
        string WF_Div = fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED6'", "WF_DIV");
        double WF_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED6'", "RATE")) / 100;
        double WF_Empr_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED6'", "EMPR_RATE"));
        double WF_Limit = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED6'", "MAX_LMT"));
        string WF_Formula = "round(((" + WF_ER + ")/" + WF_Div + ")*WORKDAYS,2) as DED6";
        #endregion

        SQuery1 = "";
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            SQuery = "select " + PF_Formula + "," + ESI_Formula + "," + WF_Formula + ",trim(empcode) as empcode,pfcut,esicut,cutvpf,TOTERN from pay where branchcd='" + frm_mbr + "' and type='10' and grade='" + txtlbl4.Text.Trim() + "' and vchnum='" + frm_vnum1 + "' and to_char(date_,'dd/mm/yyyy')='" + txtvchdate.Text + "' and empcode='" + sg1.Rows[i].Cells[7].Text.Trim() + "'";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            Tot_Ded = 0; Net_Sal = 0; DED1 = 0; DED3 = 0; DED6 = 0; PF_YN = ""; ESI_YN = ""; Tot_Earnings = 0; PFWage = 0; WFWage = 0;
            PF_AMT_CS = 0; PF_RT_CS = 0; PF_RT_ES = 0; PF_SAL = 0; ESI_SAL = 0; ESI_RT_CS = 0; ESI_RT_ES = 0; ESI_AMT_CS = 0; WF_AMT_CS = 0; WF_RT_CS = 0; WF_RT_ES = 0;
            if (dt.Rows.Count > 0)
            {
                DED6 = fgen.make_double(dt.Rows[0]["ded6"].ToString().Trim());
                PF_YN = dt.Rows[0]["pfcut"].ToString().Trim();
                ESI_YN = dt.Rows[0]["esicut"].ToString().Trim();
                Tot_Earnings = fgen.make_double(dt.Rows[0]["totern"].ToString().Trim());
            }
            #region PF
            if (PF_YN == "Y")
            {
                Apply_15000_PFLimit = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim() + "'", "mnthinc");
                if (Apply_15000_PFLimit == "1")
                {
                    if (Tot_Earnings > PF_Limit)
                    {
                        PFWage = PF_Limit;
                    }
                    else
                    {
                        PFWage = Tot_Earnings;
                    }
                }
                else
                {
                    PFWage = Tot_Earnings;
                }
                PF_RT_ES = PF_Rate * 100;
                PF_RT_CS = PF_Empr_Rate * 100;
                PF_SAL = PFWage;
                DED1 = Math.Round(PFWage * PF_Rate, 2);
                PF_AMT_CS = Math.Round(PFWage * PF_Empr_Rate, 2);
            }
            #endregion

            #region ESI
            if (ESI_YN == "Y")
            {
                ESI_RT_ES = ESI_Rate * 100;
                ESI_RT_CS = ESI_Empr_Rate * 100;
                if (dt.Rows.Count > 0)
                {
                    ESI_SAL = fgen.make_double(dt.Rows[0]["ded3"].ToString().Trim());
                }
                ESI_AMT_CS = Math.Round(ESI_SAL * ESI_Empr_Rate, 2);
                DED3 = Math.Round(ESI_SAL * ESI_Rate, 2);
            }
            #endregion

            #region WF
            WF_RT_CS = WF_Empr_Rate;// HERE WF_Empr_Rate DENOTES THE MULTIPLYING FACTOR
            WF_RT_ES = WF_Rate * 100;
            DED6 = DED6 * WF_Rate;
            if (DED6 > WF_Limit)
            {
                DED6 = WF_Limit;
                WFWage = WF_Limit * WF_Rate;
            }
            else
            {
                WFWage = (DED6 * 100) / WF_Rate;
            }
            WF_AMT_CS = Math.Round(DED6 * WF_RT_CS, 2);
            #endregion

            Tot_Ded = DED1 + DED3 + DED6;
            Net_Sal = Tot_Earnings - Tot_Ded;
            SQuery1 = "update pay set ded1=" + DED1 + ",ded3=" + DED3 + ",ded6=" + DED6 + ",totded=" + Tot_Ded + ",netslry=" + Net_Sal + ",ESI_RT_ES = " + ESI_RT_ES + ",ESI_RT_CS =" + ESI_RT_CS + ",ESI_SAL=" + ESI_SAL + ",ESI_AMT_CS=" + ESI_AMT_CS + ",PF_AMT_CS=" + PF_AMT_CS + ",PF_RT_CS=" + PF_RT_CS + ", PF_RT_ES=" + PF_RT_ES + ",PF_SAL=" + PF_SAL + ",WF_AMT_CS=" + WF_AMT_CS + ", WF_RT_CS=" + WF_RT_CS + ", WF_RT_ES=" + WF_RT_ES + ",WF_SAL=" + WFWage.ToString().Replace("NaN", "0") + " where branchcd='" + frm_mbr + "' and type='10' and grade='" + txtlbl4.Text + "' and vchnum='" + frm_vnum1 + "' and to_char(date_,'dd/mm/yyyy')='" + txtvchdate.Text + "' and empcode='" + sg1.Rows[i].Cells[7].Text.Trim() + "'";
            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery1);
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun4()
    {

    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
        SQuery = "select type1 as fstr,name as grade_name,Type1 as Grade_Code from type where id='I' and type1 like '0%' order by grade_code";
    }
    //------------------------------------------------------------------------------------
    static int CountSundays(int year, int month)
    {
        var firstDay = new DateTime(year, month, 1);
        var day29 = firstDay.AddDays(28);
        var day30 = firstDay.AddDays(29);
        var day31 = firstDay.AddDays(30);
        if ((day29.Month == month && day29.DayOfWeek == DayOfWeek.Sunday) || (day30.Month == month && day30.DayOfWeek == DayOfWeek.Sunday) || (day31.Month == month && day31.DayOfWeek == DayOfWeek.Sunday))
        {
            return 5;
        }
        else
        {
            return 4;
        }
    }
    //------------------------------------------------------------------------------------
    public void Cal()
    {
        double payablehrs = 0, payablesal = 0, tot_ot_amt = 0, days2_5_amt = 0, att_bonus = 0, fooding = 0, prev_mon_add = 0, spl_add = 0, tot_add = 0, ded_2d = 0, tot_late_coming = 0, tot_fine = 0, tot_sleeping = 0, tot_other_ded = 0;
        double advance = 0, prev_mon_sub = 0, spl_sub = 0, tot_ded = 0, gross = 0, sunday = 0, wrkhr = 0, ctc = 0, leave_2_5_days = 0, presenthrs = 0;
        double attn_fooding, attn_2d = 0, attn_late = 0, attn_fine = 0, attn_sleeping = 0, attn_tot_ded = 0, ot_hrs = 0;

        if (doc_addl.Value == "Y")
        {
            dt = new DataTable();
            SQuery = "select sum((hrwrk*60)+minwrk) as presenthrs,trim(empcode) as empcode,sum(dt1) as fooding,sum(dt2) as d2_ded,round(sum(dt3/60),2) as late,sum(dt4) as fine,round(sum(dt5/60),2) as sleeping,round(sum(tot_ded/60),2) as tot_ded,sum(sunday_pay) as sunday_pay,sum(tot_ot) as tot_ot from attn where branchcd='" + frm_mbr + "' and type='10' and grade='" + txtlbl4.Text.Trim() + "' and to_char(vchdate,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "' group by empcode";
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

            for (int i = 0; i < sg1.Rows.Count; i++)
            {
                sunday = fgen.make_double(fgen.seek_iname_dt(dt, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim() + "'", "sunday_pay"));
                ot_hrs = fgen.make_double(fgen.seek_iname_dt(dt, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim() + "'", "tot_ot"));
                wrkhr = fgen.make_double(sg1.Rows[i].Cells[13].Text.Trim());
                ctc = fgen.make_double(sg1.Rows[i].Cells[4].Text.Trim());
                leave_2_5_days = wrkhr * 2.5;

                presenthrs = fgen.make_double(fgen.seek_iname_dt(dt, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim() + "'", "presenthrs"));
                attn_fooding = fgen.make_double(fgen.seek_iname_dt(dt, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim() + "'", "fooding"));
                attn_2d = fgen.make_double(fgen.seek_iname_dt(dt, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim() + "'", "d2_ded"));
                attn_late = fgen.make_double(fgen.seek_iname_dt(dt, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim() + "'", "late"));
                attn_fine = fgen.make_double(fgen.seek_iname_dt(dt, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim() + "'", "fine"));
                attn_sleeping = fgen.make_double(fgen.seek_iname_dt(dt, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim() + "'", "sleeping"));
                attn_tot_ded = fgen.make_double(fgen.seek_iname_dt(dt, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim() + "'", "tot_ded"));

                payablehrs = (sunday * wrkhr) + (presenthrs / 60);
                payablesal = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * wrkhr)) * payablehrs;
                tot_ot_amt = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * 10)) * ot_hrs;//to be multilied by tot ot hrs
                days2_5_amt = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * wrkhr)) * leave_2_5_days;
                att_bonus = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t6"))).Text.Trim());
                prev_mon_add = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t8"))).Text.Trim());
                spl_add = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t9"))).Text.Trim());
                fooding = attn_fooding;
                tot_add = tot_ot_amt + days2_5_amt + att_bonus + fooding + prev_mon_add + spl_add;

                ded_2d = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * wrkhr)) * attn_2d;//to be mutltiplied by tot 2d ded hrs
                tot_late_coming = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * wrkhr)) * attn_late;// to be multiplied by tot late coming hrs
                tot_sleeping = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * wrkhr)) * attn_sleeping;// to be multiplied by tot late coming hrs
                tot_other_ded = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * wrkhr)) * attn_tot_ded;// to be multiplied by tot other ded
                tot_fine = attn_fine;
                advance = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t16"))).Text.Trim());
                prev_mon_sub = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t17"))).Text.Trim());
                spl_sub = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t18"))).Text.Trim());
                tot_ded = ded_2d + tot_fine + tot_late_coming + tot_other_ded + tot_sleeping + advance + prev_mon_sub + spl_sub;
                gross = Math.Round((payablesal + tot_add - tot_ded), 0);
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t2"))).Text = Math.Round(payablehrs, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t3"))).Text = Math.Round(payablesal, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t4"))).Text = Math.Round(tot_ot_amt, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t5"))).Text = Math.Round(days2_5_amt, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t7"))).Text = Math.Round(attn_fooding, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t10"))).Text = Math.Round(tot_add, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t11"))).Text = Math.Round(ded_2d, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t12"))).Text = Math.Round(tot_late_coming, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t13"))).Text = Math.Round(tot_fine, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t14"))).Text = Math.Round(tot_sleeping, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t15"))).Text = Math.Round(tot_other_ded, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t19"))).Text = Math.Round(tot_ded, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t20"))).Text = gross.ToString();

                ((TextBox)(sg1.Rows[i].FindControl("sg1_t1"))).Text = Math.Round(sunday, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t21"))).Text = Math.Round(ot_hrs, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t22"))).Text = Math.Round(presenthrs / 60, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t23"))).Text = Math.Round(attn_fooding, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t24"))).Text = Math.Round(attn_2d, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t25"))).Text = Math.Round(attn_late, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t26"))).Text = Math.Round(attn_fine, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t27"))).Text = Math.Round(attn_sleeping, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t28"))).Text = Math.Round(attn_tot_ded, 2).ToString();
            }
        }
        else
        {
            for (int i = 0; i < sg1.Rows.Count; i++)
            {
                sunday = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t1"))).Text.Trim());
                ot_hrs = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t21"))).Text.Trim());
                wrkhr = fgen.make_double(sg1.Rows[i].Cells[13].Text.Trim());
                ctc = fgen.make_double(sg1.Rows[i].Cells[4].Text.Trim());
                leave_2_5_days = wrkhr * 2.5;
                presenthrs = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t22"))).Text.Trim());
                attn_fooding = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t23"))).Text.Trim());
                attn_2d = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t24"))).Text.Trim());
                attn_late = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t25"))).Text.Trim());
                attn_fine = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t26"))).Text.Trim());
                attn_sleeping = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t27"))).Text.Trim());
                attn_tot_ded = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t28"))).Text.Trim());
                payablehrs = (sunday * wrkhr) + (presenthrs);
                payablesal = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * wrkhr)) * payablehrs;
                tot_ot_amt = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * 10)) * ot_hrs;//to be multilied by tot ot hrs
                days2_5_amt = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * wrkhr)) * leave_2_5_days;
                att_bonus = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t6"))).Text.Trim());
                prev_mon_add = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t8"))).Text.Trim());
                spl_add = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t9"))).Text.Trim());
                fooding = attn_fooding;
                tot_add = tot_ot_amt + days2_5_amt + att_bonus + fooding + prev_mon_add + spl_add;

                ded_2d = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * wrkhr)) * attn_2d;//to be mutltiplied by tot 2d ded hrs
                tot_late_coming = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * wrkhr)) * attn_late;// to be multiplied by tot late coming hrs
                tot_sleeping = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * wrkhr)) * attn_sleeping;// to be multiplied by tot sleeping hrs
                tot_other_ded = (ctc / (fgen.make_double(txtlbl8.Text.Trim()) * wrkhr)) * attn_tot_ded;// to be multiplied by tot other ded
                tot_fine = attn_fine;
                advance = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t16"))).Text.Trim());
                prev_mon_sub = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t17"))).Text.Trim());
                spl_sub = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t18"))).Text.Trim());
                tot_ded = ded_2d + tot_fine + tot_late_coming + tot_other_ded + tot_sleeping + advance + prev_mon_sub + spl_sub;
                gross = Math.Round((payablesal + tot_add - tot_ded), 0);
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t2"))).Text = Math.Round(payablehrs, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t3"))).Text = Math.Round(payablesal, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t4"))).Text = Math.Round(tot_ot_amt, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t5"))).Text = Math.Round(days2_5_amt, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t7"))).Text = Math.Round(attn_fooding, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t10"))).Text = Math.Round(tot_add, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t11"))).Text = Math.Round(ded_2d, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t12"))).Text = Math.Round(tot_late_coming, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t13"))).Text = Math.Round(tot_fine, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t14"))).Text = Math.Round(tot_sleeping, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t15"))).Text = Math.Round(tot_other_ded, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t19"))).Text = Math.Round(tot_ded, 2).ToString();
                ((TextBox)(sg1.Rows[i].FindControl("sg1_t20"))).Text = gross.ToString();
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnCal_ServerClick(object sender, EventArgs e)
    {
        Cal();
        btnsave.Disabled = false;
    }
    //------------------------------------------------------------------------------------
    protected void btnAttn_Click(object sender, EventArgs e)
    {
        doc_addl.Value = "Y";
        Cal();
        btnsave.Disabled = false;
    }
    //------------------------------------------------------------------------------------
    //SQuery = "create table wbpayh (branchcd char(2) default '-',type char(2) default '-',vchnum varchar2(6) default '-',vchdate date default sysdate,grade char(2) default '-',empcode varchar2(6) default '-',totdays number(10,2) default 0,srno number(5) default 0 ,sunday number(10,2) default 0,pr_hrs number(10,2) default 0,ot_hrs number(10,2) default 0,fooding_hrs  number(10,2) default 0,tot_2d_hrs  number(10,2) default 0,tot_late_hrs  number(10,2) default 0,tot_fine_hrs  number(10,2) default 0,tot_sleep_hrs  number(10,2) default 0,tot_other_ded_hrs  number(10,2) default 0,pay_hrs number(10,2) default 0,pay_sal number(10,2) default 0,ot number(10,2) default 0,days_ number(10,2) default 0,attn number(10,2) default 0,fooding number(10,2) default 0,prev_mth_add number(10,2) default 0,spl_add number(10,2) default 0,tot_add number(10,2) default 0,ded_2d number(10,2) default 0,late number(10,2) default 0,fine number(10,2) default 0,sleep number(10,2) default 0,oth_ded number(10,2) default 0,advance number(10,2) default 0,prev_mth_sub number(10,2) default 0,spl_sub number(10,2) default 0,tot_ded number(10,2) default 0,gross number(10,2) default 0,payno varchar2(16) default '-',actual_rate number(15,5) default 0,wrkhrs number(7,2) default 0 ,ent_by varchar2(15),ent_dt date,edt_by varchar2(15),edt_dt date)";
    //fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

    //ALTER TABLE PAY ADD ESI_SAL NUMBER(15,5) DEFAULT 0;
    //ALTER TABLE PAY ADD ESI_AMT_CS NUMBER(15,5) DEFAULT 0;

    //ALTER TABLE PAY ADD PF_SAL NUMBER(15,5) DEFAULT 0;
    //ALTER TABLE PAY ADD PF_RT_CS NUMBER(15,5) DEFAULT 0;
    //ALTER TABLE PAY ADD PF_AMT_CS NUMBER(15,5) DEFAULT 0;
    //ALTER TABLE PAY ADD PF_RT_ES NUMBER(15,5) DEFAULT 0;

    //ALTER TABLE PAY ADD WF_SAL NUMBER(15,5) DEFAULT 0;
    //ALTER TABLE PAY ADD WF_RT_CS NUMBER(15,5) DEFAULT 0;
    //ALTER TABLE PAY ADD WF_AMT_CS NUMBER(15,5) DEFAULT 0;
    //ALTER TABLE PAY ADD WF_RT_ES NUMBER(15,5) DEFAULT 0;

    //ALTER TABLE PAY ADD MASTVCH VARCHAR2(16) DEFAULT '-';

    //ALTER TABLE PAY ADD AGE NUMBER(5,2) DEFAULT 0;

    //ALTER TABLE PAY ADD SELVCH VARCHAR2(16) DEFAULT '-';
}