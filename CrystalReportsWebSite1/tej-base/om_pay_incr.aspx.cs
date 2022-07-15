using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_pay_incr : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, SQuery2, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", mq2, PDateRange;
    DataTable dt, dt2, dt3, dt4, dt5; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
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
    string frm_tabname, frm_tabname1, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
    double c1 = 0; double c2 = 0; double c3 = 0; double c4 = 0; double c5 = 0; double c6 = 0; double c7 = 0; double c8 = 0; double c9 = 0; double c10 = 0; double c11 = 0; double c12 = 0; double c13 = 0;
    double c14 = 0; double c15 = 0; double c16 = 0; double c17 = 0; double c18 = 0; double c19 = 0; double c20 = 0;
    double currsal = 0; double ctcpf = 0; double incr1 = 0; double currctc = 0;
    double pfcal = 0; string pflimt = ""; double pf = 0;
    //double double_val2, double_val1;
    fgenDB fgen = new fgenDB(); string mq3 = "";

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
                doc_addl.Value = "1";
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            //dtEarning = new DataTable();
            //dtEarning = fgen.getdata(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename,trim(er) as er from selmas where grade='" + txtlbl4.Text.Trim() + "' order by morder");
            setColHeadings();
            set_Val();
            typePopup = "Y";
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
        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        gridhead.Visible = false;
        Div4.Visible = false;
        Div3.Visible = false;
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false;
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
        div1.Visible = false; div2.Visible = false;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlbl4.Enabled = true;
        btnlbl7.Enabled = true;
        btnprint.Disabled = true; btnlist.Disabled = true;
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
        lblheader.Text = "Pay Increment";
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "PAYINCR";
        frm_tabname1 = "EMPMAS";
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

            case "TICODE":
                break;

            case "EMP":
                SQuery = "select trim(grade)||trim(empcode) as fstr,name as emp_name,empcode as emp_code,fhname as father_name,desg as desig,deptt as department,pfcut,(case when nvl(mnthinc,0)=1 then 'Y' else 'N' end) as pflmt,old_empc from empmas where branchcd='" + frm_mbr + "' and grade='" + doc_addl.Value.Trim() + "' and dtjoin<=to_DatE('" + txtvchdate.Text + "','dd/mm/yyyy') and substr(trim(tfr_stat),1,8)<>'TRANSFER' and TRIM(nvl(leaving_dt,'-'))='-' order by emp_code";
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
            case "Print_E":
                SQuery = "select trim(mthnum) as fstr,mthname ,mthnum from mths order by mthsno";
                break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                {
                    SQuery = "select distinct branchcd||trim(vchnum)||trim(vchdate)||trim(grade)||trim(empcode) as fstr,trim(vchnum) as entry_no,trim(vchdate) as Entry_date,grade,trim(empcode) as emp_code,name from " + frm_tabname + "  where branchcd='" + frm_mbr + "' AND grade='" + txtlbl4.Text.Trim() + "' AND length(trim(EMPIMG))<2 order by entry_no desc";
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
        int month = fgen.make_int(doc_vty.Value) + 1; int year;
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
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and to_date(vchdate,'dd/mm/yyyy') " + DateRange + "", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = Convert.ToDateTime(mon_end_dt).AddDays(-1).ToString("dd/MM/yyyy");
        txtlbl2.Text = frm_uname;
        txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl5.Text = "-";
        txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtefffrom.Text = Convert.ToDateTime(mon_end_dt).AddDays(-1).ToString("dd/MM/yyyy");

        int weekly_off = CountSundays(year, Convert.ToInt32(vty));
        int days = Convert.ToInt32(txtvchdate.Text.Substring(0, 2)) - weekly_off;
        txtlbl8.Text = days.ToString();
        txtlbl8.Text = txtvchdate.Text.Substring(0, 2);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();
        SQuery = "SELECT GRADE,EMPCODE,NAME,FHNAME,DESG,DEPTT,PFCUT,DEPTT_TEXT,DESG_TEXT,(CASE WHEN NVL(MNTHINC,0)=1 THEN 'Y' ELSE 'N' END) AS PFLMT,ER1,ER2,ER3,ER4,ER5,ER6,ER7,ER8,ER9,ER10,ER11,ER12,ER13,ER14,ER15,ER16,ER17,ER18,ER19,ER20 FROM EMPMAS WHERE substr(trim(tfr_stat),1,8)<>'TRANSFER' and TRIM(nvl(leaving_dt,'-'))='-' and branchcd='" + frm_mbr + "' and trim(grade)||trim(empcode)='" + col1.Trim() + "'";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        create_tab();
        sg1_dr = null;
        if (dt.Rows.Count > 0)
        {
            txtlbl4.Text = dt.Rows[0]["empcode"].ToString().Trim();
            txtlbl4a.Text = dt.Rows[0]["grade"].ToString().Trim();
            txtlbl7.Text = dt.Rows[0]["NAME"].ToString().Trim();
            txtFather.Text = dt.Rows[0]["FHNAME"].ToString().Trim();
            txtDesCode.Text = dt.Rows[0]["DESG"].ToString().Trim();
            txtDeptCode.Text = dt.Rows[0]["DEPTT"].ToString().Trim();
            txtPF.Text = dt.Rows[0]["PFCUT"].ToString().Trim();
            txtDepartment.Text = dt.Rows[0]["DEPTT_TEXT"].ToString().Trim();
            txtDesignation.Text = dt.Rows[0]["DESG_TEXT"].ToString().Trim();
            txtPFLmt.Text = dt.Rows[0]["PFLMT"].ToString().Trim();
            txtrate.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select rate from wb_selmast where grade='" + dt.Rows[0]["grade"].ToString().Trim() + "' and ed_fld='DED1'", "rate");

            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_SrNo"] = 1;
            sg1_dr["sg1_t21"] = fgen.seek_iname(frm_qstr, frm_cocd, "select rate from wb_selmast where grade='" + dt.Rows[0]["grade"].ToString().Trim() + "' and ed_fld='DED1'", "rate");
            sg1_dr["sg1_t1"] = dt.Rows[0]["ER1"].ToString().Trim();
            sg1_dr["sg1_t2"] = dt.Rows[0]["ER2"].ToString().Trim();
            sg1_dr["sg1_t3"] = dt.Rows[0]["ER3"].ToString().Trim();
            sg1_dr["sg1_t4"] = dt.Rows[0]["ER4"].ToString().Trim();
            sg1_dr["sg1_t5"] = dt.Rows[0]["ER5"].ToString().Trim();
            sg1_dr["sg1_t6"] = dt.Rows[0]["ER6"].ToString().Trim();
            sg1_dr["sg1_t7"] = dt.Rows[0]["ER7"].ToString().Trim();
            sg1_dr["sg1_t8"] = dt.Rows[0]["ER8"].ToString().Trim();
            sg1_dr["sg1_t9"] = dt.Rows[0]["ER9"].ToString().Trim();
            sg1_dr["sg1_t10"] = dt.Rows[0]["ER10"].ToString().Trim();
            sg1_dr["sg1_t11"] = dt.Rows[0]["ER11"].ToString().Trim();
            sg1_dr["sg1_t12"] = dt.Rows[0]["ER12"].ToString().Trim();
            sg1_dr["sg1_t13"] = dt.Rows[0]["ER13"].ToString().Trim();
            sg1_dr["sg1_t14"] = dt.Rows[0]["ER14"].ToString().Trim();
            sg1_dr["sg1_t15"] = dt.Rows[0]["ER15"].ToString().Trim();
            sg1_dr["sg1_t16"] = dt.Rows[0]["ER16"].ToString().Trim();
            sg1_dr["sg1_t17"] = dt.Rows[0]["ER17"].ToString().Trim();
            sg1_dr["sg1_t18"] = dt.Rows[0]["ER18"].ToString().Trim();
            sg1_dr["sg1_t19"] = dt.Rows[0]["ER19"].ToString().Trim();
            sg1_dr["sg1_t20"] = dt.Rows[0]["ER20"].ToString().Trim();
            sg1_dt.Rows.Add(sg1_dr);
            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            setColHeadings();
            ViewState["sg1"] = sg1_dt;
        }
        sg2_dt = new DataTable();
        create_tab2();
        sg2_add_blankrows();
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        setColHeadings();
        ViewState["sg2"] = sg3_dt;
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
        Div4.Visible = true;
        Div3.Visible = true;
        gridhead.Visible = true;
        Cal();
        Cal2();
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

        int dhd1 = fgen.ChkDate(txtapplfrm.Text.ToString());
        if (dhd1 == 0) { fgen.msg("-", "AMSG", "Please Select last date of Month in Applicable Date"); return; }
        //SQuery = "";
        //dt = new DataTable();
        //SQuery = "SELECT * FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND GRADE='" + txtlbl4.Text.Trim() + "' AND VCHDATE BETWEEN TO_DATE('" + txtvchdate.Text + "','DD/MM/YYYY') AND TO_DATE('" + txtlbl4a.Text + "','DD/MM/YYYY')";
        //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

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
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        SQuery = "SELECT NAME ,VCHDATE,INC_APP_DT,EMPCODE,GRADE,ER1,ER2,ER3,ER4,ER5,ER6,ER7,ER8,ER9,ER10,ER11,ER12,ENT_DT FROM PAYINCR WHERE BRANCHCD='" + frm_mbr + "' ORDER BY GRADE";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("Increment Checklist", frm_qstr);
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("Select Grade", frm_qstr);
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
        //frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_vty = "10";
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {

                string mq0 = "";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                dt = new DataTable();
                SQuery = "select * from " + frm_tabname + " where branchcd||trim(vchnum)||trim(vchdate)||trim(grade)||trim(empcode)='" + mq0 + "'";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt2 = new DataTable();
                SQuery1 = "select * from " + frm_tabname1 + " where branchcd='" + frm_mbr + "' and trim(grade)||trim(empcode)='" + mq0.Substring(18, 8) + "'";
                dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                if (dt.Rows.Count > 0)
                {
                    c1 = fgen.make_double(dt2.Rows[0]["ER1"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER1"].ToString().Trim());
                    c2 = fgen.make_double(dt2.Rows[0]["ER2"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER2"].ToString().Trim());
                    c3 = fgen.make_double(dt2.Rows[0]["ER3"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER3"].ToString().Trim());
                    c4 = fgen.make_double(dt2.Rows[0]["ER4"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER4"].ToString().Trim());
                    c5 = fgen.make_double(dt2.Rows[0]["ER5"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER5"].ToString().Trim());
                    c6 = fgen.make_double(dt2.Rows[0]["ER6"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER6"].ToString().Trim());
                    c7 = fgen.make_double(dt2.Rows[0]["ER7"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER7"].ToString().Trim());
                    c8 = fgen.make_double(dt2.Rows[0]["ER8"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER8"].ToString().Trim());
                    c9 = fgen.make_double(dt2.Rows[0]["ER9"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER9"].ToString().Trim());
                    c10 = fgen.make_double(dt2.Rows[0]["ER10"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER10"].ToString().Trim());
                    c11 = fgen.make_double(dt2.Rows[0]["ER11"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER11"].ToString().Trim());
                    c12 = fgen.make_double(dt2.Rows[0]["ER12"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER12"].ToString().Trim());
                    c13 = fgen.make_double(dt2.Rows[0]["ER13"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER13"].ToString().Trim());
                    c14 = fgen.make_double(dt2.Rows[0]["ER14"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER14"].ToString().Trim());
                    c15 = fgen.make_double(dt2.Rows[0]["ER15"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER15"].ToString().Trim());
                    c16 = fgen.make_double(dt2.Rows[0]["ER16"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER16"].ToString().Trim());
                    c17 = fgen.make_double(dt2.Rows[0]["ER17"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER17"].ToString().Trim());
                    c18 = fgen.make_double(dt2.Rows[0]["ER18"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER18"].ToString().Trim());
                    c19 = fgen.make_double(dt2.Rows[0]["ER19"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER19"].ToString().Trim());
                    c20 = fgen.make_double(dt2.Rows[0]["ER20"].ToString().Trim()) - fgen.make_double(dt.Rows[0]["ER20"].ToString().Trim());
                    mq2 = "";
                    mq2 = "UPDATE " + frm_tabname1 + " SET ER1='" + c1 + "',ER2='" + c2 + "',ER3='" + c3 + "',ER4='" + c4 + "',ER5='" + c5 + "',ER6='" + c6 + "',ER7='" + c7 + "',ER8='" + c8 + "',ER9='" + c9 + "',ER10='" + c10 + "',ER11='" + c11 + "',ER12='" + c12 + "',ER13='" + c13 + "',ER14='" + c14 + "',ER15='" + c15 + "',ER16='" + c17 + "',ER18='" + c18 + "',ER19='" + c19 + "',ER20='" + c20 + "' WHERE BRANCHCD='" + frm_mbr + "' AND GRADE='" + dt.Rows[0]["GRADE"].ToString().Trim() + "' AND EMPCODE='" + dt.Rows[0]["EMPCODE"].ToString().Trim() + "' ";
                }
                //UPDATE EMPMAS TABLE FIELD FIRST
                if (dt.Rows[0]["empimg"].ToString().Trim().Length > 3)
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, mq2);
                }
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where branchcd||trim(vchnum)||trim(vchdate)||trim(grade)||trim(empcode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
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
                    doc_addl.Value = col1;
                    hffield.Value = "New_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Month", frm_qstr);
                    //newCase(col1);
                    break;

                case "New_E":
                    doc_vty.Value = col1;                    
                    if (Convert.ToInt32(col1) > 3 && Convert.ToInt32(col1) <= 12)
                    {

                    }
                    else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                    int days = DateTime.DaysInMonth(fgen.make_int(frm_myear), fgen.make_int(col1));
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and to_date(vchdate,'dd/mm/yyyy') " + DateRange + "", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = days + "/" + col1 + "/" + frm_myear;
                    SQuery = "SELECT MAX(TO_CHAR(DATE_,'YYYYMMDD')) AS DATE_ FROM PAY WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND DATE_ " + DateRange + "";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["DATE_"].ToString().Trim().Length == 8)
                        {
                            DateTime date1 = DateTime.ParseExact(dt.Rows[0]["DATE_"].ToString().Trim(), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                            mq2 = date1.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        }
                        if (Convert.ToDateTime(mq2) > Convert.ToDateTime(txtvchdate.Text))
                        {
                            fgen.msg("-", "AMSG", "Salary Already Made Entered UPto " + mq2 + "  '13' Saving Changes Will Not Be Allowed");
                            return;
                        }
                    }
                    hffield.Value = "EMP";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Employee", frm_qstr);
                    break;

                case "EMP":
                    newCase(doc_vty.Value);
                    Fetch_Col_Earn();
                    Fetch_Col_Earn2();
                    div1.Visible = true;
                    div2.Visible = true;
                    gridhead.Visible = true;
                    Div4.Visible = true;
                    Div3.Visible = true;
                    Cal();
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
                    frm_vty = "10";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                    txtlbl4.Text = col1;
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Month", frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                    #region Edit Start

                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select * from " + frm_tabname + " where branchcd||trim(vchnum)||trim(vchdate)||trim(grade)||trim(empcode)='" + col1 + "' ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl2.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[0]["ent_dt"].ToString().Trim();
                        txtefffrom.Text = Convert.ToDateTime(dt.Rows[0]["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtapplfrm.Text = Convert.ToDateTime(dt.Rows[0]["INC_APP_DT"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4.Text = dt.Rows[0]["EMPCODE"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["GRADE"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["NAME"].ToString().Trim();
                        txtFather.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT FHNAME,DESG,DEPTT FROM EMPMAS WHERE BRANCHCD='" + frm_mbr + "' AND GRADE='" + dt.Rows[0]["GRADE"].ToString().Trim() + "' AND EMPCODE='" + dt.Rows[0]["EMPCODE"].ToString().Trim() + "'", "FHNAME");
                        txtDesCode.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT FHNAME,DESG,DEPTT FROM EMPMAS WHERE BRANCHCD='" + frm_mbr + "' AND GRADE='" + dt.Rows[0]["GRADE"].ToString().Trim() + "' AND EMPCODE='" + dt.Rows[0]["EMPCODE"].ToString().Trim() + "'", "DESG");
                        txtDesignation.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT FHNAME,DESG,DEPTT,DESG_TEXT FROM EMPMAS WHERE BRANCHCD='" + frm_mbr + "' AND GRADE='" + dt.Rows[0]["GRADE"].ToString().Trim() + "' AND EMPCODE='" + dt.Rows[0]["EMPCODE"].ToString().Trim() + "'", "DESG_TEXT");
                        txtDeptCode.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT FHNAME,DESG,DEPTT,DEPTT_TEXT,DESG_TEXT FROM EMPMAS WHERE BRANCHCD='" + frm_mbr + "' AND GRADE='" + dt.Rows[0]["GRADE"].ToString().Trim() + "' AND EMPCODE='" + dt.Rows[0]["EMPCODE"].ToString().Trim() + "'", "DEPTT");
                        txtDepartment.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT FHNAME,DESG,DEPTT,DEPTT_TEXT,DESG_TEXT FROM EMPMAS WHERE BRANCHCD='" + frm_mbr + "' AND GRADE='" + dt.Rows[0]["GRADE"].ToString().Trim() + "' AND EMPCODE='" + dt.Rows[0]["EMPCODE"].ToString().Trim() + "'", "DEPTT_TEXT");

                        txtPF.Text = dt.Rows[0]["PF"].ToString().Trim();
                        txtPFLmt.Text = dt.Rows[0]["PFCUT"].ToString().Trim();
                        txtcurrsal.Text = dt.Rows[0]["JOIN_SAL"].ToString().Trim();
                        txtctcpf.Text = dt.Rows[0]["INC_AMT1"].ToString().Trim();
                        txtincr.Text = dt.Rows[0]["INCREMNT"].ToString().Trim();
                        txtcurrctc.Text = dt.Rows[0]["CURR_CTC"].ToString().Trim();
                        create_tab2();
                        sg2_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_t1"] = dt.Rows[0]["ER1"].ToString().Trim();
                            sg2_dr["sg2_t2"] = dt.Rows[0]["ER2"].ToString().Trim();
                            sg2_dr["sg2_t3"] = dt.Rows[0]["ER3"].ToString().Trim();
                            sg2_dr["sg2_t4"] = dt.Rows[0]["ER4"].ToString().Trim();
                            sg2_dr["sg2_t5"] = dt.Rows[0]["ER5"].ToString().Trim();
                            sg2_dr["sg2_t6"] = dt.Rows[0]["ER6"].ToString().Trim();
                            sg2_dr["sg2_t7"] = dt.Rows[0]["ER7"].ToString().Trim();
                            sg2_dr["sg2_t8"] = dt.Rows[0]["ER8"].ToString().Trim();
                            sg2_dr["sg2_t9"] = dt.Rows[0]["ER9"].ToString().Trim();
                            sg2_dr["sg2_t10"] = dt.Rows[0]["ER10"].ToString().Trim();
                            sg2_dr["sg2_t11"] = dt.Rows[0]["ER11"].ToString().Trim();
                            sg2_dr["sg2_t12"] = dt.Rows[0]["ER12"].ToString().Trim();
                            sg2_dr["sg2_t13"] = dt.Rows[0]["ER13"].ToString().Trim();
                            sg2_dr["sg2_t14"] = dt.Rows[0]["ER14"].ToString().Trim();
                            sg2_dr["sg2_t15"] = dt.Rows[0]["ER15"].ToString().Trim();
                            sg2_dr["sg2_t16"] = dt.Rows[i]["ER16"].ToString().Trim();
                            sg2_dr["sg2_t17"] = dt.Rows[i]["ER17"].ToString().Trim();
                            sg2_dr["sg2_t18"] = dt.Rows[i]["ER18"].ToString().Trim();
                            sg2_dr["sg2_t19"] = dt.Rows[i]["ER19"].ToString().Trim();
                            sg2_dr["sg2_t20"] = dt.Rows[i]["ER20"].ToString().Trim();
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        dt.Dispose(); sg2_dt.Dispose();

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
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";
                            sg1_dr["sg1_f1"] = "-";
                            mq3 = dt.Rows[i]["PAYLINK"].ToString().Trim();
                            z = 1;
                            for (int j = 0; j < mq3.Split('~').Length; j++)
                            {
                                sg1_dr["sg1_t" + z + ""] = mq3.Split('~')[j];
                                z++;
                            }
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        Fetch_Col_Earn();
                        Fetch_Col_Earn2();
                        div1.Visible = true;
                        div2.Visible = true;
                        gridhead.Visible = true;
                        Div4.Visible = true;
                        Div3.Visible = true;
                        Cal(); Cal2();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", txtlbl4.Text.Trim());
                    if (Convert.ToInt32(col1) > 3 && Convert.ToInt32(col1) <= 12)
                    {

                    }
                    else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", col1 + "/" + frm_myear);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", col2); // month name
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

                case "List_E":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", txtlbl4.Text.Trim());
                    if (Convert.ToInt32(col1) > 3 && Convert.ToInt32(col1) <= 12)
                    {

                    }
                    else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", col1 + "/" + frm_myear);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", col2); // month name
                    frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F85150"); // ATTENDANCE REGISTER IS ALREADY MERGE ON THIS ID THATS' WHY IT IS HARD CODED
                    fgen.fin_pay_reps(frm_qstr);
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
            SQuery = "SELECT a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode as cust_code,c.aname as cust_name,a.icode as item_code,b.iname as item_name,a.enqno as jobno,to_char(A.enqdt,'dd/mm/yyyy') as job_dt from costestimate a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + " order by entry_no";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------

            //string last_entdt;
            //checks
            //if (edmode.Value == "Y")
            //{
            //}
            //else
            //{
            //    last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  ", "ldt");
            //    if (last_entdt == "0")
            //    { }
            //    else
            //    {
            //        if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
            //        {
            //            Checked_ok = "N";
            //            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
            //        }
            //    }
            //}

            //last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            //if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            //{
            //    Checked_ok = "N";
            //    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            //}
            //-----------------------------
            i = 0;
            hffield.Value = "";

            setColHeadings();

            #region Number Gen and Save to Table
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "N")
            {
                btnsave.Disabled = false;
                Div3.Visible = true;
                Div4.Visible = true;
                gridhead.Visible = true;

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
                        // save_fun2();
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
                            save_it = "Y";
                            if (save_it == "Y")
                            {
                                i = 0;
                                do
                                {
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and to_date(vchdate,'dd/mm/yyyy') " + DateRange + " ", 6, "vch");
                                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                    if (i > 20)
                                    {
                                        fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and to_date(vchdate,'dd/mm/yyyy') " + DateRange + " ", 6, "vch");
                                        pk_error = "N";
                                        i = 0;
                                    }
                                    i++;
                                }
                                while (pk_error == "Y");

                                //string doc_is_ok = "";
                                //double dte;
                                //dte = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ") as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and to_date(vchdate,'dd/mm/yyyy') " + DateRange + "", "vch"));
                                //frm_vnum = (dte + 1).ToString();
                                ////frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                //doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                //if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        //save_fun3();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                            //cmd_query = "update " + frm_tabname1 + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            //fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        //  save_fun2();

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
                        // fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
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
        sg2_dt.Columns.Add(new DataColumn("sg2_f1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f6", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f7", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f8", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f9", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f10", typeof(string)));

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
        sg2_dt.Columns.Add(new DataColumn("sg2_t11", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t12", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t13", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t14", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t15", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t16", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t17", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t18", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t19", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t20", typeof(string)));

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
        sg2_dr["sg2_f1"] = "-";
        sg2_dr["sg2_f2"] = "-";
        sg2_dr["sg2_f3"] = "-";
        sg2_dr["sg2_f4"] = "-";
        sg2_dr["sg2_f5"] = "-";
        sg2_dr["sg2_f5"] = "-";
        sg2_dr["sg2_f6"] = "-";
        sg2_dr["sg2_f7"] = "-";
        sg2_dr["sg2_f8"] = "-";
        sg2_dr["sg2_f9"] = "-";
        sg2_dr["sg2_f10"] = "-";
        sg2_dr["sg2_t1"] = "0";
        sg2_dr["sg2_t2"] = "0";
        sg2_dr["sg2_t3"] = "0";
        sg2_dr["sg2_t4"] = "0";
        sg2_dr["sg2_t5"] = "0";
        sg2_dr["sg2_t6"] = "0";
        sg2_dr["sg2_t7"] = "0";
        sg2_dr["sg2_t8"] = "0";
        sg2_dr["sg2_t9"] = "0";
        sg2_dr["sg2_t10"] = "0";
        sg2_dr["sg2_t11"] = "0";
        sg2_dr["sg2_t12"] = "0";
        sg2_dr["sg2_t13"] = "0";
        sg2_dr["sg2_t14"] = "0";
        sg2_dr["sg2_t15"] = "0";
        sg2_dr["sg2_t16"] = "0";
        sg2_dr["sg2_t17"] = "0";
        sg2_dr["sg2_t18"] = "0";
        sg2_dr["sg2_t19"] = "0";
        sg2_dr["sg2_t20"] = "0";

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
            //sg1.HeaderRow.Cells[37].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER1'", "ename");
            //sg1.HeaderRow.Cells[38].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER2'", "ename");
            //sg1.HeaderRow.Cells[39].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER3'", "ename");
            //sg1.HeaderRow.Cells[40].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER4'", "ename");
            //sg1.HeaderRow.Cells[41].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER5'", "ename");
            //sg1.HeaderRow.Cells[42].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER6'", "ename");
            //sg1.HeaderRow.Cells[43].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER7'", "ename");
            //sg1.HeaderRow.Cells[44].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER8'", "ename");
            //sg1.HeaderRow.Cells[45].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER9'", "ename");
            //sg1.HeaderRow.Cells[46].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER10'", "ename");

            //sg1.HeaderRow.Cells[37].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER1'", "ename");
            //sg1.HeaderRow.Cells[38].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER2'", "ename");
            //sg1.HeaderRow.Cells[39].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER3'", "ename");
            //sg1.HeaderRow.Cells[40].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER4'", "ename");
            //sg1.HeaderRow.Cells[41].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER5'", "ename");
            //sg1.HeaderRow.Cells[42].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER6'", "ename");
            //sg1.HeaderRow.Cells[43].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER7'", "ename");
            //sg1.HeaderRow.Cells[44].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER8'", "ename");
            //sg1.HeaderRow.Cells[45].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER9'", "ename");
            //sg1.HeaderRow.Cells[46].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER10'", "ename");
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
        //dt2 = new DataTable();
        //dt2 = fgen.getdata(frm_qstr, frm_cocd, "select branchcd,trim(grade) as grade,trim(empcode) as empcode,pfcut,esicut,cutvpf,er1,er2,er3,er4,er5,er6,er7,er8,er9,er10,er11,er12,er13,er14,er15,er16,er17,er18,er19,er20,ded1,ded2,ded3,ded4,ded5,ded6,ded7,ded8,ded9,ded10,ded11,ded12,ded13,ded14,ded15,ded16,ded17,ded18,ded19,ded20 from empmas where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' order by empcode");
        //double PFlimit = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='H11'", "params"));
        for (i = 0; i < sg2.Rows.Count; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum.Trim().ToUpper();
            oporow["VCHDATE"] = txtvchdate.Text.Trim().ToUpper();
            //oporow["SRNO"] = i + 1;
            oporow["EMPCODE"] = txtlbl4.Text.ToString().Trim().ToUpper();
            oporow["GRADE"] = txtlbl4a.Text.ToString().Trim().ToUpper();
            oporow["INC_APP_DT"] = Convert.ToDateTime(txtapplfrm.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");

            oporow["ER1"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim().ToUpper());
            oporow["ER2"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim().ToUpper());
            oporow["ER3"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim().ToUpper());
            oporow["ER4"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim().ToUpper());
            oporow["ER5"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim().ToUpper());
            oporow["ER6"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim().ToUpper());
            oporow["ER7"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim().ToUpper());
            oporow["ER8"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim().ToUpper());
            oporow["ER9"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim().ToUpper());
            oporow["ER10"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim().ToUpper());
            oporow["ER11"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t11")).Text.Trim().ToUpper());
            oporow["ER12"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t12")).Text.Trim().ToUpper());
            oporow["ER13"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t13")).Text.Trim().ToUpper());
            oporow["ER14"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t14")).Text.Trim().ToUpper());
            oporow["ER15"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t15")).Text.Trim().ToUpper());
            oporow["ER16"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t16")).Text.Trim().ToUpper());
            oporow["ER17"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t17")).Text.Trim().ToUpper());
            oporow["ER18"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t18")).Text.Trim().ToUpper());
            oporow["ER19"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t19")).Text.Trim().ToUpper());
            oporow["ER20"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t20")).Text.Trim().ToUpper());

            oporow["NAME"] = txtlbl7.Text.Trim().ToUpper();
            oporow["INCREMNT"] = fgen.make_double(txtincr.Text.Trim().ToString());
            oporow["CURR_CTC"] = fgen.make_double(txtcurrctc.Text.Trim().ToUpper());
            oporow["INC_AMT1"] = fgen.make_double(txtctcpf.Text.Trim().ToUpper());
            oporow["JOIN_SAL"] = fgen.make_double(txtcurrsal.Text.Trim().ToUpper());

            oporow["SEX"] = "-";
            oporow["FHNAME"] = "-";
            oporow["DEPTT"] = "-";
            oporow["DEPTT2"] = 0;
            oporow["DEPTT1"] = 0;
            oporow["DESG"] = "-";
            oporow["DESCGRD"] = "-";
            oporow["SECTION_"] = "-";
            oporow["TRADE"] = "-";
            oporow["INDST"] = "-";
            oporow["SCALE"] = "-";
            oporow["EL"] = 0;
            oporow["CL"] = 0;
            oporow["SL"] = 0;

            oporow["MNTHINC"] = 0;
            oporow["PF"] = txtPF.Text.Trim().ToUpper();
            oporow["PFCUT"] = txtPFLmt.Text.Trim().ToUpper();
            oporow["CUTVPF"] = "-";
            oporow["FPF"] = 0;
            oporow["ADVANCE"] = 0;
            oporow["ADVBAL"] = 0;
            oporow["ESI"] = "-";
            oporow["ESICUT"] = "-";
            oporow["INSURABL"] = "-";
            oporow["ITAX"] = 0;
            oporow["PFLOAN"] = 0;
            oporow["PFLOANBL"] = 0;
            oporow["PFNO"] = "-";
            oporow["PFNOMINEE"] = "-";
            oporow["FPFNO"] = "-";
            oporow["FPFNOMINEE"] = "-";
            oporow["ESINO"] = "-";
            oporow["ESINOMINEE"] = "-";
            oporow["BANK"] = "-";
            oporow["BNKACNO"] = "-";
            oporow["DED1"] = 0;
            oporow["DED2"] = 0;
            oporow["DED3"] = 0;
            oporow["DED4"] = 0;
            oporow["DED5"] = 0;
            oporow["DED6"] = 0;
            oporow["DED7"] = 0;
            oporow["DED8"] = 0;
            oporow["DED9"] = 0;
            oporow["DED10"] = 0;
            oporow["DED11"] = 0;
            oporow["DED12"] = 0;
            oporow["DED13"] = 0;
            oporow["DED14"] = 0;
            oporow["DED15"] = 0;
            oporow["DED16"] = 0;
            oporow["DED17"] = 0;
            oporow["DED18"] = 0;
            oporow["DED19"] = 0;
            oporow["DED20"] = 0;
            oporow["LMCOINS"] = 0;
            oporow["OTHER"] = 0;
            oporow["SAV1"] = 0;
            oporow["SAV2"] = 0;
            oporow["SAV3"] = 0;
            oporow["SAV4"] = 0;
            oporow["SAV5"] = 0;
            oporow["SAVINGS"] = 0;
            oporow["LTA"] = 0;
            oporow["BONUS"] = 0;
            oporow["COINS"] = 0;
            oporow["GENERATED"] = 0;
            oporow["LTA"] = 0;
            oporow["STATUS"] = "I";
            oporow["MCAT"] = "-";
            oporow["ADDR1"] = "-";
            oporow["ADDR2"] = "-";
            oporow["CITY"] = "-";

            oporow["STATE"] = "-";
            oporow["COUNTRY"] = "-";
            oporow["PIN"] = "-";
            oporow["PHONE"] = "-";
            oporow["MOBILE"] = "-";
            oporow["EMAIL"] = "-";
            oporow["PADDR1"] = "-";
            oporow["PADDR2"] = "-";
            oporow["PCITY"] = "-";
            oporow["PSTATE"] = "-";
            oporow["PCOUNTRY"] = "-";
            oporow["PPIN"] = "-";
            oporow["PPHONE"] = "-";
            oporow["VPF_RATE"] = 0;
            oporow["MARRIED"] = "-";
            oporow["WRKHOUR"] = 0;
            oporow["DEPTT_TEXT"] = "-";
            oporow["DESG_TEXT"] = "-";
            oporow["LEAVING_DT"] = "-";
            oporow["LEAVING_WHY"] = "-";
            oporow["ESI_DISP"] = "-";
            oporow["TFR_STAT"] = "-";
            oporow["CARDNO"] = "-";
            oporow["MED"] = 0;
            oporow["BNP"] = 0;
            oporow["VEHI"] = 0;
            oporow["REIMGEN"] = 0;
            oporow["REIMTEL"] = 0;
            oporow["INC_DT1"] = "-";
            oporow["OTH2"] = 0;
            oporow["OTH3"] = 0;
            oporow["QUALIFIC"] = "-";
            oporow["BLOODGRP"] = "-";
            oporow["SHIFT_TYPE"] = "-";
            oporow["SHIFT_CODE"] = "-";
            oporow["EMPIMG"] = "-";
            oporow["CONF_DT"] = "-";
            oporow["PYMT_BY"] = "-";
            oporow["BON_RATE"] = 0;
            oporow["OLD_EMPC"] = "-";
            oporow["CUTWF"] = "-";
            oporow["CUT_WF"] = "-";
            oporow["WPW"] = "-";
            oporow["CHILD_CNT"] = 0;
            oporow["DEDCANT"] = "-";
            oporow["DEDGRAT"] = "-";
            oporow["NEW_PFRULE"] = "-";
            oporow["MLEAVE"] = 0;
            oporow["INCR_M"] = "-";
            oporow["UINNO"] = "-";
            oporow["EDT_DTL"] = "-";
            oporow["IFSC_CODE"] = "-";
            oporow["EMP_TYPE"] = "-";
            oporow["QTR_QPI"] = 0;
            oporow["CURR_BR"] = "-";
            oporow["ADHARNO"] = "-";
            oporow["SP_RELASHN"] = "-";
            oporow["BNKCID"] = "-";
            oporow["ATN_ALLOW"] = "-";
            oporow["WLEVEL3PW"] = "-";
            oporow["WLEVEL3PW"] = "-";
            oporow["EMER_NAME"] = "-";
            oporow["EMER_RELA"] = "-";
            oporow["EMER_CONT"] = "-";
            oporow["BANKW_NAME"] = "-";
            oporow["PAYLINK"] = fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t1")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t2")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t3")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t4")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t5")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t6")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t7")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t8")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t9")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t10")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t11")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t12")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t13")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t14")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t15")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t16")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t17")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t18")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t19")).Text.Trim().ToUpper()) + "~" + fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t20")).Text.Trim().ToUpper());
            if (edmode.Value == "Y")
            {
                oporow["ent_by"] = ViewState["entby"].ToString();
                oporow["ent_dt"] = ViewState["entdt"].ToString();
                oporow["APPR_BY"] = "-";
                oporow["APP_BY"] = "-";
                oporow["APP_DT"] = "-";
                //oporow["edt_by"] = frm_uname;
                //oporow["edt_dt"] = vardate;
            }
            else
            {
                oporow["ent_by"] = frm_uname;
                oporow["ent_dt"] = vardate;
                oporow["APPR_BY"] = "-";
                oporow["APP_BY"] = "-";
                oporow["APP_DT"] = "-";
                //oporow["edt_by"] = "-";
                //oporow["edt_dt"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (i = 0; i < sg1.Rows.Count; i++)
        {
            oporow2 = oDS2.Tables[0].NewRow();
            oporow2["branchcd"] = frm_mbr;
            oporow2["vchnum"] = frm_vnum.Trim().ToUpper();
            oporow2["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow2["EMPCODE"] = txtlbl4.Text.Trim().ToUpper();
            oporow["GRADE"] = txtlbl4a.Text.Trim().ToUpper();
            oporow2["ER1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t1")).Text.Trim().ToUpper());
            oporow2["ER2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t2")).Text.Trim().ToUpper());
            oporow2["ER3"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t3")).Text.Trim().ToUpper());
            oporow2["ER4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t4")).Text.Trim().ToUpper());
            oporow2["ER5"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t5")).Text.Trim().ToUpper());
            oporow2["ER6"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t6")).Text.Trim().ToUpper());
            oporow2["ER7"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t7")).Text.Trim().ToUpper());
            oporow2["ER8"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t8")).Text.Trim().ToUpper());
            oporow2["ER9"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t9")).Text.Trim().ToUpper());
            oporow2["ER10"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t10")).Text.Trim().ToUpper());
            oporow2["ER11"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t11")).Text.Trim().ToUpper());
            oporow2["ER12"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t12")).Text.Trim().ToUpper());
            oporow2["ER13"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t13")).Text.Trim().ToUpper());
            oporow2["ER14"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t14")).Text.Trim().ToUpper());
            oporow2["ER15"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t15")).Text.Trim().ToUpper());
            oporow2["ER16"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t16")).Text.Trim().ToUpper());
            oporow2["ER17"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t17")).Text.Trim().ToUpper());
            oporow2["ER18"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t18")).Text.Trim().ToUpper());
            oporow2["ER19"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t19")).Text.Trim().ToUpper());
            oporow2["ER20"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg2.Rows[0].FindControl("sg2_t20")).Text.Trim().ToUpper());
            oDS2.Tables[0].Rows.Add(oporow2);
            mq2 = "UPDATE " + frm_tabname1 + " SET ER1='" + oporow2["ER1"] + "',ER2='" + oporow2["ER2"] + "',ER3='" + oporow2["ER3"] + "',ER4='" + oporow2["ER4"] + "',ER5='" + oporow2["ER5"] + "',ER6='" + oporow2["ER6"] + "',ER7='" + oporow2["ER7"] + "',ER8='" + oporow2["ER8"] + "',ER9='" + oporow2["ER9"] + "',ER10='" + oporow2["ER10"] + "',ER11='" + oporow2["ER11"] + "',ER12='" + oporow2["ER12"] + "',ER13='" + oporow2["ER13"] + "',ER14='" + oporow2["ER14"] + "',ER15='" + oporow2["ER15"] + "',ER16='" + oporow2["ER16"] + "',ER17='" + oporow2["ER17"] + "',ER18='" + oporow2["ER18"] + "',ER19='" + oporow2["ER19"] + "',ER20='" + oporow2["ER20"] + "' WHERE BRANCHCD='" + oporow2["branchcd"] + "' AND EMPCODE='" + oporow2["EMPCODE"] + "' AND GRADE='" + oporow["GRADE"] + "'";
            fgen.execute_cmd(frm_qstr, frm_cocd, mq2);

        }
    }
    //------------------------------------------------------------------------------------
    void save_fun3()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        //dt2 = new DataTable();
        //dt2 = fgen.getdata(frm_qstr, frm_cocd, "select branchcd,trim(grade) as grade,trim(empcode) as empcode,pfcut,esicut,cutvpf,er1,er2,er3,er4,er5,er6,er7,er8,er9,er10,er11,er12,er13,er14,er15,er16,er17,er18,er19,er20,ded1,ded2,ded3,ded4,ded5,ded6,ded7,ded8,ded9,ded10,ded11,ded12,ded13,ded14,ded15,ded16,ded17,ded18,ded19,ded20 from empmas where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' order by empcode");
        //double PFlimit = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='H11'", "params"));
        for (i = 0; i < sg1.Rows.Count; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum.Trim().ToUpper();
            oporow["VCHDATE"] = txtvchdate.Text.Trim().ToUpper();
            //oporow["SRNO"] = i + 1;
            oporow["EMPCODE"] = txtlbl4.Text.ToString().Trim().ToUpper();
            oporow["GRADE"] = txtlbl4a.Text.ToString().Trim().ToUpper();

            oporow["ER1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper());
            oporow["ER2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper());
            oporow["ER3"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper());
            oporow["ER4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper());
            oporow["ER5"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper());
            oporow["ER6"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper());
            oporow["ER7"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper());
            oporow["ER8"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper());
            oporow["ER9"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper());
            oporow["ER10"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().ToUpper());
            oporow["ER11"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim().ToUpper());
            oporow["ER12"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim().ToUpper());
            oporow["ER13"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim().ToUpper());
            oporow["ER14"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim().ToUpper());
            oporow["ER15"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim().ToUpper());
            oporow["ER16"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().ToUpper());
            oporow["ER17"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim().ToUpper());
            oporow["ER18"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim().ToUpper());
            oporow["ER19"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim().ToUpper());
            oporow["ER20"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim().ToUpper());

            oporow["NAME"] = txtlbl7.Text.Trim().ToUpper();
            oporow["INCREMNT"] = fgen.make_double(txtincr.Text.Trim().ToString());
            oporow["CURR_CTC"] = fgen.make_double(txtcurrctc.Text.Trim().ToUpper());
            oporow["INC_AMT1"] = fgen.make_double(txtctcpf.Text.Trim().ToUpper());
            oporow["INC_APP_DT"] = Convert.ToDateTime(txtapplfrm.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");

            oporow["SEX"] = "-";
            oporow["FHNAME"] = "-";
            oporow["DEPTT"] = "-";
            oporow["DEPTT2"] = 0;
            oporow["DEPTT1"] = 0;
            oporow["DESG"] = "-";
            oporow["DESCGRD"] = "-";
            oporow["SECTION_"] = "-";
            oporow["TRADE"] = "-";
            oporow["INDST"] = "-";
            oporow["SCALE"] = "-";
            oporow["EL"] = 0;
            oporow["CL"] = 0;
            oporow["SL"] = 0;

            oporow["MNTHINC"] = 0;
            oporow["PF"] = txtPF.Text.Trim().ToUpper();
            oporow["PFCUT"] = txtPFLmt.Text.Trim().ToUpper();
            oporow["CUTVPF"] = "-";
            oporow["FPF"] = 0;
            oporow["ADVANCE"] = 0;
            oporow["ADVBAL"] = 0;
            oporow["ESI"] = "-";
            oporow["ESICUT"] = "-";
            oporow["INSURABL"] = "-";
            oporow["ITAX"] = 0;
            oporow["PFLOAN"] = 0;
            oporow["PFLOANBL"] = 0;
            oporow["PFNO"] = "-";
            oporow["PFNOMINEE"] = "-";
            oporow["FPFNO"] = "-";
            oporow["FPFNOMINEE"] = "-";
            oporow["ESINO"] = "-";
            oporow["ESINOMINEE"] = "-";
            oporow["BANK"] = "-";
            oporow["BNKACNO"] = "-";
            oporow["DED1"] = 0;
            oporow["DED2"] = 0;
            oporow["DED3"] = 0;
            oporow["DED4"] = 0;
            oporow["DED5"] = 0;
            oporow["DED6"] = 0;
            oporow["DED7"] = 0;
            oporow["DED8"] = 0;
            oporow["DED9"] = 0;
            oporow["DED10"] = 0;
            oporow["DED11"] = 0;
            oporow["DED12"] = 0;
            oporow["DED13"] = 0;
            oporow["DED14"] = 0;
            oporow["DED15"] = 0;
            oporow["DED16"] = 0;
            oporow["DED17"] = 0;
            oporow["DED18"] = 0;
            oporow["DED19"] = 0;
            oporow["DED20"] = 0;
            oporow["LMCOINS"] = 0;
            oporow["OTHER"] = 0;
            oporow["SAV1"] = 0;
            oporow["SAV2"] = 0;
            oporow["SAV3"] = 0;
            oporow["SAV4"] = 0;
            oporow["SAV5"] = 0;
            oporow["SAVINGS"] = 0;
            oporow["LTA"] = 0;
            oporow["BONUS"] = 0;
            oporow["COINS"] = 0;
            oporow["GENERATED"] = 0;
            oporow["LTA"] = 0;
            oporow["STATUS"] = "E";
            oporow["MCAT"] = "-";
            oporow["ADDR1"] = "-";
            oporow["ADDR2"] = "-";
            oporow["CITY"] = "-";

            oporow["STATE"] = "-";
            oporow["COUNTRY"] = "-";
            oporow["PIN"] = "-";
            oporow["PHONE"] = "-";
            oporow["MOBILE"] = "-";
            oporow["EMAIL"] = "-";
            oporow["PADDR1"] = "-";
            oporow["PADDR2"] = "-";
            oporow["PCITY"] = "-";
            oporow["PSTATE"] = "-";
            oporow["PCOUNTRY"] = "-";
            oporow["PPIN"] = "-";
            oporow["PPHONE"] = "-";
            oporow["VPF_RATE"] = 0;
            oporow["MARRIED"] = "-";
            oporow["WRKHOUR"] = 0;
            oporow["DEPTT_TEXT"] = "-";
            oporow["DESG_TEXT"] = "-";
            oporow["LEAVING_DT"] = "-";
            oporow["LEAVING_WHY"] = "-";
            oporow["ESI_DISP"] = "-";
            oporow["TFR_STAT"] = "-";
            oporow["CARDNO"] = "-";
            oporow["MED"] = 0;
            oporow["BNP"] = 0;
            oporow["VEHI"] = 0;
            oporow["REIMGEN"] = 0;
            oporow["REIMTEL"] = 0;
            oporow["INC_DT1"] = "-";
            oporow["OTH2"] = 0;
            oporow["OTH3"] = 0;
            oporow["QUALIFIC"] = "-";
            oporow["BLOODGRP"] = "-";
            oporow["SHIFT_TYPE"] = "-";
            oporow["SHIFT_CODE"] = "-";
            oporow["EMPIMG"] = "-";
            oporow["CONF_DT"] = "-";
            oporow["APP_BY"] = "-";
            oporow["APP_DT"] = "-";
            oporow["APPR_BY"] = "-";
            oporow["PYMT_BY"] = "-";
            oporow["BON_RATE"] = 0;
            oporow["OLD_EMPC"] = "-";
            oporow["CUTWF"] = "-";
            oporow["CUT_WF"] = "-";
            oporow["WPW"] = "-";
            oporow["JOIN_SAL"] = 0;
            oporow["CHILD_CNT"] = 0;
            oporow["DEDCANT"] = "-";
            oporow["DEDGRAT"] = "-";
            oporow["NEW_PFRULE"] = "-";
            oporow["MLEAVE"] = 0;
            oporow["INCR_M"] = "-";
            oporow["UINNO"] = "-";
            oporow["EDT_DTL"] = "-";
            oporow["IFSC_CODE"] = "-";
            oporow["EMP_TYPE"] = "-";
            oporow["QTR_QPI"] = 0;
            oporow["CURR_BR"] = "-";
            oporow["ADHARNO"] = "-";
            oporow["SP_RELASHN"] = "-";
            oporow["BNKCID"] = "-";
            oporow["ATN_ALLOW"] = "-";
            oporow["WLEVEL3PW"] = "-";
            oporow["WLEVEL3PW"] = "-";

            oporow["EMER_NAME"] = "-";
            oporow["EMER_RELA"] = "-";
            oporow["EMER_CONT"] = "-";
            oporow["BANKW_NAME"] = "-";

            if (edmode.Value == "Y")
            {
                oporow["ent_by"] = ViewState["entby"].ToString();
                oporow["ent_dt"] = ViewState["entdt"].ToString();
                //oporow["edt_by"] = frm_uname;
                //oporow["edt_dt"] = vardate;
            }
            else
            {
                oporow["ent_by"] = frm_uname;
                oporow["ent_dt"] = vardate;
                //oporow["edt_by"] = "-";
                //oporow["edt_dt"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);
        }
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
        SQuery = "select type1 as fstr,name as grade_name,Type1 as Grade_Code from type where id='I' and substr(type1,1,1)<'2' order by grade_code";
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
    public void Fetch_Col_Earn()
    {
        dt2 = new DataTable();
        SQuery = "select  er as col ,ename as coloumns from selmas where grade='" + txtlbl4a.Text.Trim() + "' order by grade, morder";
        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg1_dr = sg1_dt.NewRow();
        z = 17;
        for (int d = 0; d < dt2.Rows.Count; d++)
        {
            sg1.HeaderRow.Cells[z].Text = dt2.Rows[d]["coloumns"].ToString().Trim().Replace(" ", "_");
            z++;
        }
        if (sg1.Rows.Count > 0)
        {
            //sg1.HeaderRow.Cells[20].Text = "LTA";
            //sg1.HeaderRow.Cells[21].Text = "MEDICAL";
            //sg1.HeaderRow.Cells[22].Text = "BNP";
            //sg1.HeaderRow.Cells[23].Text = "VEHI.";
            //sg1.HeaderRow.Cells[24].Text = "REIMB_GEN";
            //sg1.HeaderRow.Cells[25].Text = "REIMB_TEL";
        }
    }
    //------------------------------------------------------------------------------------
    public void Fetch_Col_Earn2()
    {
        dt2 = new DataTable();
        SQuery = "select  er as col ,ename as coloumns from selmas where grade='" + txtlbl4a.Text.Trim() + "' order by grade, morder";
        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg2_dt = new DataTable();
        sg2_dr = sg2_dt.NewRow();
        z = 13;
        for (int d = 0; d < dt2.Rows.Count; d++)
        {

            sg2.HeaderRow.Cells[z].Text = dt2.Rows[d]["coloumns"].ToString().Trim().Replace(" ", "_");
            z++;
        }
        //sg2_add_blankrows();

    }
    //------------------------------------------------------------------------------------
    public void Cal()
    {
        double currsal = 0;
        for (int sg1r = 0; sg1r < sg1.Rows.Count; sg1r++)
        {
            c1 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t1")).Text.Trim());
            c2 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t2")).Text.Trim());
            c3 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t3")).Text.Trim());
            c4 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t4")).Text.Trim());
            c5 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t5")).Text.Trim());
            c6 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t6")).Text.Trim());
            c7 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t7")).Text.Trim());
            c8 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t8")).Text.Trim());
            c9 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t9")).Text.Trim());
            c10 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t10")).Text.Trim());
            c11 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t11")).Text.Trim());
            c12 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t12")).Text.Trim());
            c13 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t13")).Text.Trim());
            c14 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t14")).Text.Trim());
            c15 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t15")).Text.Trim());
            c16 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t16")).Text.Trim());
            c17 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t17")).Text.Trim());
            c18 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t18")).Text.Trim());
            c19 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t19")).Text.Trim());
            c20 = fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t20")).Text.Trim());

            currsal = (c1 * 1) + (c2 * 1) + (c3 * 1) + (c4 * 1) + (c5 * 1) + (c6 * 1) + (c7 * 1) + (c8 * 1) + (c9 * 1) + (c10 * 1) + (c11 * 1) + (c12 * 1) + (c13 * 1) + (c14 * 1) + (c15 * 1) + (c16 * 1) + (c17 * 1) + (c18 * 1) + (c19 * 1) + (c20 * 1);
            txtcurrsal.Text = currsal.ToString();
        }
    }
    //------------------------------------------------------------------------------------
    public void Cal2()
    {
        double c21 = 0; double c22 = 0; double c23 = 0; double c24 = 0; double c25 = 0; double c26 = 0; double c27 = 0; double c28 = 0; double c29 = 0; double c30 = 0; double c31 = 0; double c32 = 0; double c33 = 0; double c34 = 0;
        double c35 = 0; double c36 = 0; double c37 = 0; double c38 = 0; double c39 = 0; double c40 = 0; double incr = 0;
        for (int sg2r = 0; sg2r < sg2.Rows.Count; sg2r++)
        {
            c21 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t1")).Text.Trim());
            c22 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t2")).Text.Trim());
            c23 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t3")).Text.Trim());
            c24 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t4")).Text.Trim());
            c25 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t5")).Text.Trim());
            c26 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t6")).Text.Trim());
            c27 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t7")).Text.Trim());
            c28 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t8")).Text.Trim());
            c29 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t9")).Text.Trim());
            c30 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t10")).Text.Trim());
            c31 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t11")).Text.Trim());
            c32 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t12")).Text.Trim());
            c33 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t13")).Text.Trim());
            c34 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t14")).Text.Trim());
            c35 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t15")).Text.Trim());
            c36 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t16")).Text.Trim());
            c37 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t17")).Text.Trim());
            c38 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t18")).Text.Trim());
            c39 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t19")).Text.Trim());
            c40 = fgen.make_double(((TextBox)sg2.Rows[sg2r].FindControl("sg2_t20")).Text.Trim());
            incr = (c21 * 1) + (c22 * 1) + (c23 * 1) + (c24 * 1) + (c25 * 1) + (c26 * 1) + (c27 * 1) + (c28 * 1) + (c29 * 1) + (c30 * 1) + (c31 * 1) + (c32 * 1) + (c33 * 1) + (c34 * 1) + (c35 * 1) + (c36 * 1) + (c37 * 1) + (c38 * 1) + (c39 * 1) + (c40 * 1);
            txtincr.Text = incr.ToString();
        }

        DataTable dtSelmas = new DataTable();
        SQuery2 = "select ED_FLD,RATE,EMPR_RATE,MAX_LMT from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4a.Text.Trim() + "'";
        dtSelmas = fgen.getdata(frm_qstr, frm_cocd, SQuery2);
        double PF_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "RATE")) / 100;
        double PF_Empr_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "EMPR_RATE")) / 100;
        double PF_Limit = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "MAX_LMT"));
        double PFTOT = fgen.make_double(txtcurrsal.Text.Trim());
        pfcal = fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t1")).Text.Trim());
        pflimt = txtPF.Text.Trim().ToString().ToUpper();
        if (pflimt.ToString() == "Y")
        {
            if (PFTOT > PF_Limit)
            {
                pf = Math.Round(PF_Limit * PF_Empr_Rate, 2);
            }
            else
            {
                pf = Math.Round(PFTOT * PF_Empr_Rate, 2);
            }
            txtctcpf.Text = pf.ToString();
        }
        else
        {
            pf = 0;
            txtctcpf.Text = pf.ToString();
        }
        currsal = fgen.make_double(txtcurrsal.Text.Trim());
        ctcpf = fgen.make_double(txtctcpf.Text.Trim());
        incr1 = fgen.make_double(txtincr.Text.Trim());
        currctc += (currsal * 1) + (ctcpf * 1) + (incr1 * 1);
        txtcurrctc.Text = currctc.ToString();
    }
    protected void cal_ctc_Click(object sender, EventArgs e)
    {
        Cal();
        Cal2();

        div1.Visible = true;
        div2.Visible = true;
        gridhead.Visible = true;
        Div4.Visible = true;
        Div3.Visible = true;
    }
}