using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_inc_ded : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "N",vcntry;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;
    String pop_qry;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;
    string mq0 = "", mq1 = "";
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName, cmd_query;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    //double double_val2, double_val1;
    int srno = 1;
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
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
                txtddpf.Visible = false;
                txtddesi.Visible = false;
                txtddwf.Visible = false;
                //txtddern.Visible = false;
                txtOTErn.Visible = false;
                txtOTErn2.Visible = false;
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
                for (int i = 0; i < 10; i++)
                {
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[K].Cells[i].CssClass = "hidden";
                }
                #endregion
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

                ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");
                // ((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");
                //((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");

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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        create_tab();
        create_tab3();
        sg1_add_blankrows();
        sg3_add_blankrows();
        //btnlbl7.Enabled = false;
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        //btnlbl7.Enabled = true;
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
        doc_nf.Value = "VCHNUM";
        doc_df.Value = "VCHDATE";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        frm_tabname = "WB_SELMAST";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        typePopup = "N";
        lblheader.Text = "Income & Deduction Heads";
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
            case "BTN_20":
                break;
            case "BTN_21":
                break;
            case "BTN_22":
                break;
            case "BTN_23":
                break;
            case "TICODE":
                col1 = "";
                SQuery = "SELECT DAY AS FSTR, DAY2 AS DAY_NAME,DAY FROM (select  '01' as day,'MONDAY' as day2 from dual union all select  '02' as day,'TUESDAY' as day2 from dual union all select  '03' as day,'WEDNESDAY' as day2 from dual union all select  '04' as day,'THRUSDAY' as day2 from dual union all select  '05' as day,'FRIDAY' as day2 from dual union all select  '06' as day,'SATURDAY' as day2 from dual union all select  '07' as day,'SUNDAY' as day2 from dual)";
                break;
            case "New":
            case "GRADE":
                col1 = "";
                SQuery = "select type1 as fstr,name as grade_name,Type1 as Grade_Code from type where id='I' and substr(type1,1,1)<'2' order by grade_code";
                break;
            case "TACODE":
                //pop1
                col1 = "";
                SQuery = "";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[13].Text.Trim() + "'";
                    else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                }
                if (col1.Length <= 0) col1 = "'-'";

                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg3.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[3].Text.Trim() + "'";
                    else col1 = "'" + gr.Cells[3].Text.Trim() + "'";
                }
                if (col1.Length <= 0) col1 = "'-'";
                SQuery = "select trim(acref)as fstr , trim(name) as HS_name, trim(acref) as hs_code from typegrp where BRANCHCD!='DD' AND id='T1' and length(acref)>4 and trim(acref) not in (" + col1 + ") order by acref";
                break;

            //case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct a.grade||trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as entry_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as entry_Dt,a.grade as code,t.name as grade,to_char(a.eff_from,'dd/mm/yyyy') as eff_from,to_char(a.eff_to,'dd/mm/yyyy') as eff_upto,(case when nvl(icat,'-')!='Y' then 'Open' else 'Close' end) as status,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a,type t wheRE trim(a.grade)=trim(t.type1) and t.id='I' and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' order by vdd desc,entry_no";
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
        DDBind();
        typePopup = "Y";
        btngrade.Enabled = false;
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";
            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Grade", frm_qstr);
            }
            btngrade.Focus();

            // else comment upper code

            //frm_vnum = fgen.next_no(frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH" );
            //txtvchnum.Value = frm_vnum;
            //txtvchdate.Value = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            //disablectrl();
            //fgen.EnableForm(this.Controls);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        string mhd;
        if (col1 == "") return;
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        //lbl1a.Text = col1;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        disablectrl();
        fgen.EnableForm(this.Controls);
        ((ImageButton)sg1.Rows[z].FindControl("sg1_btnadd")).Focus();

        //btnlbl7.Focus();
        sg3_dt = new DataTable();
        create_tab3();
        for (int i = 0; i < 10; i++)
        {
            sg3_dr = sg3_dt.NewRow();
            //sg3_dr["sg3_h1"] = "-";
            //sg3_dr["sg3_h2"] = "-";
            //sg3_dr["sg3_h3"] = "-";
            //sg3_dr["sg3_h4"] = "-";
            //sg3_dr["sg3_h5"] = "-";
            //sg3_dr["sg3_h6"] = "-";
            //sg3_dr["sg3_h7"] = "-";
            //sg3_dr["sg3_h8"] = "-";
            //sg3_dr["sg3_h9"] = "-";
            //sg3_dr["sg3_h10"] = "-";
            sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
            sg3_dr["sg3_f1"] = "-";
            sg3_dr["sg3_f2"] = "-";
            int db5 = sg3_dt.Rows.Count + 1;
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select upper(trim(countrynm)) as cntry from type where id='B' and trim(type1)='" + frm_mbr + "' ", "cntry");
            if (mhd == "" || mhd == "0" || mhd == "-")
            { fgen.msg("Alert", "", "Please check country in Branch master."); }
            else
            {
                if (mhd == "INDIA")  vcntry = "1"; 
                switch (i)
                {
                    case 0:
                        sg3_dr["sg3_f1"] = "DED" + db5;
                        sg3_dr["sg3_t1"] = (vcntry == "1") ? "PF" : "Gosi";
                       break;
                    case 1:
                        sg3_dr["sg3_f1"] = "DED" + db5;
                        sg3_dr["sg3_t1"] = (vcntry == "1") ? "VPF" : "-";
                        break;
                    case 2:
                        sg3_dr["sg3_f1"] = "DED" + db5;
                        sg3_dr["sg3_t1"] =  (vcntry == "1") ? "ESI" : "-";
                        break;
                    case 3:
                        sg3_dr["sg3_f1"] = "DED" + db5;
                        sg3_dr["sg3_t1"] = (vcntry == "1") ? "TDS" : "-";
                        break;
                    case 4:
                        sg3_dr["sg3_f1"] = "DED" + db5;
                        sg3_dr["sg3_t1"] = "ADV";
                        break;
                    case 5:
                        sg3_dr["sg3_f1"] = "DED" + db5;
                        sg3_dr["sg3_t1"] = (vcntry == "1") ? "WF" : "-";
                        break;
                    case 6:
                        sg3_dr["sg3_f1"] = "DED" + db5;
                        sg3_dr["sg3_t1"] = "";
                        break;
                    case 7:
                        sg3_dr["sg3_f1"] = "DED" + db5;
                        sg3_dr["sg3_t1"] = "";
                        break;
                    case 8:
                        sg3_dr["sg3_f1"] = "DED" + db5;
                        sg3_dr["sg3_t1"] = "";
                        break;
                    case 9:
                        sg3_dr["sg3_f1"] = "DED" + db5;
                        sg3_dr["sg3_t1"] = (vcntry == "1") ? "PT" : "-";
                        break;
                }
            }
            sg3_dt.Rows.Add(sg3_dr);
        }
        sg3_add_blankrows();
        sg3.DataSource = sg3_dt;
        sg3.DataBind();
        setColHeadings();
        ViewState["sg3"] = sg3_dt;
        sg1_dt = new DataTable();
        create_tab();
        for (int i = 0; i < 9; i++)
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
            int db6 = sg1_dt.Rows.Count + 1;
            switch (i)
            {
                case 0:
                    sg1_dr["sg1_f1"] = "ER" + db6;
                    sg1_dr["sg1_t1"] = "BASIC";
                    break;
                default:
                    sg1_dr["sg1_f1"] = "ER" + db6;
                    sg1_dr["sg1_t1"] = "";
                    break;
            }
            sg1_dt.Rows.Add(sg1_dr);
        }
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        //txtlbl4.Focus();
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
        if (dhd == 0) { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        dhd = fgen.ChkDate(txtlbl4.Text.ToString());
        if (dhd == 0) { fgen.msg("-", "AMSG", "Please Select Valid Effective From Date"); txtlbl4.Focus(); return; }

        dhd = fgen.ChkDate(txtlbl4a.Text.ToString());
        if (dhd == 0) { fgen.msg("-", "AMSG", "Please Select Valid Effective To Date"); txtlbl4a.Focus(); return; }

        mq0 = txtlbl4.Text;
        //mq1 = txtlbl4a.Text;

        if (mq0.Substring(0, 2) != "01")
        {
            fgen.msg("-", "AMSG", "Effective From Date Can Be The First Date Of The Month"); return;
        }
        //int days = DateTime.DaysInMonth(fgen.make_int(mq0.Substring(6, 4)), fgen.make_int(mq0.Substring(3, 2)));
        //if (mq1.Substring(0, 2) != days.ToString())
        //{
        //    fgen.msg("-", "AMSG", "Effective To Date Can Be The Last Date Of The Month"); return;
        //}

        if (Convert.ToDateTime(mq0) > Convert.ToDateTime(txtlbl4a.Text))
        {
            fgen.msg("-", "AMSG", "Effective From Date Cannot Be Greater Than Effective To Date"); return;
        }

        SQuery = "select to_char(last_day(to_date('" + txtlbl4a.Text + "','dd/mm/yyyy')),'dd/mm/yyyy') as lastdate from dual";
        mq1 = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "lastdate");
        if (mq1 != txtlbl4a.Text)
        {
            fgen.msg("-", "AMSG", "Effective To Date Can Be The Last Date Of The Month"); return;
        }

        // DONE ON THE INSTRUCTIONS OF MAYURI MAM----------------------
        //if (DDPF.SelectedItem.Text == "PLEASE SELECT")
        //{
        //    fgen.msg("-", "AMSG", "Please Select PF PDay"); return;
        //}

        //if (DDESI.SelectedItem.Text == "PLEASE SELECT")
        //{
        //    fgen.msg("-", "AMSG", "Please Select ESI PDay"); return;
        //}

        //if (DDWF.SelectedItem.Text == "PLEASE SELECT")
        //{
        //    fgen.msg("-", "AMSG", "Please Select WF PDay"); return;
        //}
        //--------------------------------------

        //if (ddern.SelectedItem.Text == "PLEASE SELECT")
        //{
        //    fgen.msg("-", "AMSG", "Please Select Earning to be Proportionate"); return;
        //}

        if (txtlbl5.Text.Trim().ToUpper() == "-" || txtlbl5.Text.Trim().ToUpper() == "" || txtlbl5.Text.Trim().ToUpper() == "0")
        {
            fgen.msg("-", "AMSG", "Please Fill Max_Working_Hrs/Day"); txtlbl5.Focus();
            return;
        }
        SQuery = "";
        if (edmode.Value != "Y")
        {
            dt = new DataTable();
            SQuery = "SELECT * FROM WB_SELMAST WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND GRADE='" + txtgrade.Text.Trim() + "' AND ICAT='N' AND VCHDATE BETWEEN TO_DATE('" + txtvchdate.Text + "','DD/MM/YYYY') AND TO_DATE('" + txtlbl4a.Text + "','DD/MM/YYYY') ";
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

            if (dt.Rows.Count > 0)
            {
                fgen.msg("-", "AMSG", "On this Grade an Entry is Already Activated. First Deactivate that Entry!!"); return;
            }
        }
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper().Length > 1)
            {
                if (((DropDownList)sg1.Rows[i].FindControl("ddern")).SelectedItem.Text.Trim() == "PLEASE SELECT")
                {
                    fgen.msg("-", "AMSG", "Please Select ER Proportionate At Line No. " + sg1.Rows[i].Cells[12].Text.Trim()); return;
                }
            }
        }
        for (int i = 0; i < sg3.Rows.Count; i++)
        {
            if (((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim().ToUpper().Length > 1)
            {
                if (((DropDownList)sg3.Rows[i].FindControl("sg3_t6")).SelectedItem.Text.Trim() == "PLEASE SELECT")
                {
                    fgen.msg("-", "AMSG", "Please Select DED Proportionate At Line No. " + sg3.Rows[i].Cells[2].Text.Trim()); return;
                }
            }
        }
        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
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
        sg2_dt = new DataTable();
        sg3_dt = new DataTable();
        sg4_dt = new DataTable();
        create_tab();
        create_tab3();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg3_add_blankrows();
        sg3.DataSource = sg3_dt;
        sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
        ViewState["sg1"] = null;
        ViewState["sg3"] = null;
        setColHeadings();
        DDClear();
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
        hffield.Value = "Print";
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where A.TYPE='10' AND a.branchcd||a.GRADE||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.TYPE)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' AND FINPKFLD LIKE '" + frm_tabname + "%'");
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
                    dt = new DataTable();
                    string mq1 = "select distinct vchnum,grade from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + col1 + "' and nvl(icat,'-')!='Y'";
                    dt = fgen.getdata(frm_qstr, frm_cocd, mq1);
                    if (dt.Rows.Count > 0)
                    {
                        fgen.msg("-", "AMSG", "Already have an Entry on this Grade Please Edit '13'  Entry No: " + dt.Rows[0]["vchnum"].ToString().Trim() + "");
                    }
                    else
                    {
                        fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
                        hffield.Value = "NEW_E";
                        //txtstatus.Text = "N";                       
                        txtgrade.Text = col1;
                        txtgradenm.Text = col2;
                        //newCase(frm_vty);
                    }
                    //-------------------------------------------
                    #endregion
                    break;
                case "NEW_E":
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        hffield.Value = "COPY_OLD";
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("-", frm_qstr);
                    }
                    else
                    {
                        txtstatus.Text = "N";
                        txtgrade.Text = col1;
                        txtgradenm.Text = col2;
                        newCase(frm_vty);
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
                    //lbl1a.Text = col1;
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
                    fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;

                case "COPY_OLD":
                    #region Copy from Old Temp
                    if (col1 == "") return;
                    clearctrl();
                    DDBind();
                    frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    //lbl1a.Text = col1;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

                    SQuery = "Select a.* from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.grade||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + col1 + "' ORDER BY a.morder";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt2 = new DataTable();
                    mq0 = "select name ,Type1  from type where id='I' and type1 like '0%' order by TYPE1";
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    if (dt.Rows.Count > 0)
                    {
                        //txtgrade.Text = dt.Rows[i]["GRADE"].ToString().Trim();
                        //txtgradenm.Text = fgen.seek_iname_dt(dt2, "type1=" + dt.Rows[i]["GRADE"].ToString().Trim() + "", "NAME");
                        txtlbl4a.Text = Convert.ToDateTime(dt.Rows[i]["EFF_TO"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4.Text = Convert.ToDateTime(dt.Rows[i]["EFF_FROM"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl7.Text = dt.Rows[i]["WEEK_OFF"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[i]["DAYN"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[i]["MINHRS"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["FST_START"].ToString().Trim();
                        txtlbl5.Text = dt.Rows[i]["MAXHRS"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["FST_END"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[i]["MINHRS"].ToString().Trim();
                        txtunit.Text = dt.Rows[i]["SSFT_START"].ToString().Trim();
                        txtfc.Text = dt.Rows[i]["SSFT_END"].ToString().Trim();
                        //txtstatus.Text = dt.Rows[i]["icat"].ToString().Trim();
                        txtstatus.Text = "N";
                        txtovertm.Text = dt.Rows[i]["OT_DAYS"].ToString().Trim();
                        txtovertm2.Text = dt.Rows[i]["OT_DAYS2"].ToString().Trim();
                        //ddOT.SelectedItem.Text = dt.Rows[i]["OT_DIV"].ToString().Trim();
                        if (dt.Rows[i]["PF_DIV"].ToString() == "TOTDAYS" || dt.Rows[i]["PF_DIV"].ToString() == "WORKDAYS" || dt.Rows[i]["PF_DIV"].ToString() == "PLEASE SELECT") { DDPF.SelectedItem.Text = dt.Rows[i]["PF_DIV"].ToString().Trim(); }
                        else { txtddpf.Text = dt.Rows[i]["PF_DIV"].ToString(); DDPF.SelectedItem.Text = "OTHER"; txtddpf.Visible = true; }

                        if (dt.Rows[i]["ESI_DIV"].ToString() == "TOTDAYS" || dt.Rows[i]["ESI_DIV"].ToString() == "WORKDAYS" || dt.Rows[i]["ESI_DIV"].ToString() == "PLEASE SELECT") { DDESI.SelectedItem.Text = dt.Rows[i]["ESI_DIV"].ToString().Trim(); }
                        else { txtddesi.Text = dt.Rows[i]["ESI_DIV"].ToString(); DDESI.SelectedItem.Text = "OTHER"; txtddesi.Visible = true; }

                        if (dt.Rows[i]["WF_DIV"].ToString() == "TOTDAYS" || dt.Rows[i]["WF_DIV"].ToString() == "WORKDAYS" || dt.Rows[i]["WF_DIV"].ToString() == "PLEASE SELECT") { DDWF.SelectedItem.Text = dt.Rows[i]["WF_DIV"].ToString().Trim(); }
                        else { txtddwf.Text = dt.Rows[i]["WF_DIV"].ToString(); DDWF.SelectedItem.Text = "OTHER"; txtddwf.Visible = true; }

                        if (dt.Rows[i]["OT_DIV"].ToString() == "TOTDAYS" || dt.Rows[i]["OT_DIV"].ToString() == "WORKDAYS" || dt.Rows[i]["OT_DIV"].ToString() == "PLEASE SELECT") { ddOT.SelectedItem.Text = dt.Rows[i]["OT_DIV"].ToString().Trim(); }
                        else { txtOTErn.Text = dt.Rows[i]["OT_DIV"].ToString(); ddOT.SelectedItem.Text = "OTHER"; txtOTErn.Visible = true; }

                        if (dt.Rows[i]["OT_DIV2"].ToString() == "TOTDAYS" || dt.Rows[i]["OT_DIV2"].ToString() == "WORKDAYS" || dt.Rows[i]["OT_DIV2"].ToString() == "PLEASE SELECT") { ddOT2.SelectedItem.Text = dt.Rows[i]["OT_DIV2"].ToString().Trim(); }
                        else { txtOTErn2.Text = dt.Rows[i]["OT_DIV2"].ToString(); ddOT2.SelectedItem.Text = "OTHER"; txtOTErn2.Visible = true; }

                        create_tab();
                        sg1_dr = null;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["ED_FLD"].ToString().Trim().Substring(0, 1) == "E")
                            {
                                sg1_dr = sg1_dt.NewRow();
                                sg1_dr["sg1_srno"] = srno;
                                sg1_dr["sg1_h1"] = "-";
                                sg1_dr["sg1_h2"] = "-";
                                sg1_dr["sg1_h3"] = "-";
                                sg1_dr["sg1_h4"] = "-";
                                sg1_dr["sg1_h5"] = "-";
                                sg1_dr["sg1_h6"] = "-";
                                sg1_dr["sg1_f1"] = dt.Rows[i]["ED_FLD"].ToString().Trim();
                                sg1_dr["sg1_f2"] = "";
                                sg1_dr["sg1_t1"] = dt.Rows[i]["ED_NAME"].ToString().Trim();
                                sg1_dr["sg1_t2"] = dt.Rows[i]["PF_YN"].ToString().Trim();
                                sg1_dr["sg1_t5"] = dt.Rows[i]["VPF_YN"].ToString().Trim();
                                sg1_dr["sg1_t6"] = dt.Rows[i]["ESI_YN"].ToString().Trim();
                                sg1_dr["sg1_t8"] = dt.Rows[i]["WF_YN"].ToString().Trim();
                                sg1_dr["sg1_t9"] = dt.Rows[i]["PT_YN"].ToString().Trim();
                                sg1_dr["sg1_t11"] = dt.Rows[i]["OT_YN"].ToString().Trim();
                                sg1_dr["sg1_t12"] = dt.Rows[i]["EL_YN"].ToString().Trim();
                                if (dt.Rows[i]["ERN_DIV"].ToString() == "TOTDAYS" || dt.Rows[i]["ERN_DIV"].ToString() == "WORKDAYS" || dt.Rows[i]["ERN_DIV"].ToString() == "PLEASE SELECT")
                                {
                                    sg1_dr["sg1_t13"] = dt.Rows[i]["ERN_DIV"].ToString().Trim();
                                    sg1_dr["txtddern"] = "-";
                                }
                                else
                                {
                                    sg1_dr["txtddern"] = dt.Rows[i]["ERN_DIV"].ToString();
                                    sg1_dr["sg1_t13"] = "OTHER";
                                }
                                sg1_dt.Rows.Add(sg1_dr);
                                srno++;
                            }
                        }
                        //create_tab();
                        //sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        create_tab3();
                        sg3_dr = null;
                        int k = 1;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["ED_FLD"].ToString().Trim().Substring(0, 1) == "D")
                            {
                                sg3_dr = sg3_dt.NewRow();
                                sg3_dr["sg3_SrNo"] = k;
                                sg3_dr["sg3_f1"] = dt.Rows[i]["ED_FLD"].ToString().Trim();
                                sg3_dr["sg3_f2"] = "";
                                sg3_dr["sg3_t1"] = dt.Rows[i]["ED_NAME"].ToString().Trim();
                                sg3_dr["sg3_t3"] = dt.Rows[i]["RATE"].ToString().Trim();
                                sg3_dr["sg3_t4"] = fgen.make_double(dt.Rows[i]["EMPR_RATE"].ToString().Trim());
                                sg3_dr["sg3_t5"] = fgen.make_double(dt.Rows[i]["MAX_LMT"].ToString().Trim());
                                if (dt.Rows[i]["DED_DIV"].ToString() == "TOTDAYS" || dt.Rows[i]["DED_DIV"].ToString() == "WORKDAYS" || dt.Rows[i]["DED_DIV"].ToString() == "PLEASE SELECT" || dt.Rows[i]["DED_DIV"].ToString() == "N.A.")
                                {
                                    sg3_dr["sg3_t6"] = dt.Rows[i]["DED_DIV"].ToString().Trim();
                                    sg3_dr["sg3_t7"] = "-";
                                }
                                else
                                {
                                    sg3_dr["sg3_t7"] = dt.Rows[i]["DED_DIV"].ToString();
                                    sg3_dr["sg3_t6"] = "OTHER";
                                }
                                sg3_dt.Rows.Add(sg3_dr);
                                k++;
                            }
                        }
                        //------------------------
                        k = 0;
                       // sg3_add_blankrows();

                        ViewState["sg3"] = sg3_dt;
                        sg3.DataSource = sg3_dt;
                        sg3.DataBind();
                        dt.Dispose();
                        sg3_dt.Dispose();
                        //((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Focus();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();

                        foreach (GridViewRow gr in sg1.Rows)
                        {
                            string hf = ((HiddenField)gr.FindControl("cmd2")).Value;
                            string hf2 = ((HiddenField)gr.FindControl("cmd3")).Value;
                            string hf3 = ((HiddenField)gr.FindControl("cmd4")).Value;
                            string hf4 = ((HiddenField)gr.FindControl("cmd5")).Value;
                            string hf5 = ((HiddenField)gr.FindControl("cmd6")).Value;
                            string hf6 = ((HiddenField)gr.FindControl("cmd8")).Value;
                            string hf7 = ((HiddenField)gr.FindControl("cmd9")).Value;
                            string hf8 = ((HiddenField)gr.FindControl("cmd11")).Value;
                            string hf9 = ((HiddenField)gr.FindControl("cmd12")).Value;
                            string hf10 = ((HiddenField)gr.FindControl("cmd13")).Value;

                            if (hf == "Y") { ((CheckBox)gr.FindControl("sg1_t2")).Checked = true; }
                            if (hf2 == "Y") { ((CheckBox)gr.FindControl("sg1_t3")).Checked = true; }
                            if (hf3 == "Y") { ((CheckBox)gr.FindControl("sg1_t4")).Checked = true; }
                            if (hf4 == "Y") { ((CheckBox)gr.FindControl("sg1_t5")).Checked = true; }
                            if (hf5 == "Y") { ((CheckBox)gr.FindControl("sg1_t6")).Checked = true; }
                            if (hf6 == "Y") { ((CheckBox)gr.FindControl("sg1_t8")).Checked = true; }
                            if (hf7 == "Y") { ((CheckBox)gr.FindControl("sg1_t9")).Checked = true; }
                            if (hf8 == "Y") { ((CheckBox)gr.FindControl("sg1_t11")).Checked = true; }
                            if (hf9 == "Y") { ((CheckBox)gr.FindControl("sg1_t12")).Checked = true; }
                            if (hf10 == "OTHER") { ((TextBox)gr.FindControl("txtddern")).Visible = true; }
                            else { ((TextBox)gr.FindControl("txtddern")).Visible = false; }
                            if (hf10 != "" && hf10 != "-")
                            {
                                ((DropDownList)gr.FindControl("ddern")).Items.FindByText(hf10).Selected = true;
                            }
                        }

                        foreach (GridViewRow gr in sg3.Rows)
                        {
                            string hf15 = ((HiddenField)gr.FindControl("cmd15")).Value;
                            if (hf15 == "OTHER") { ((TextBox)gr.FindControl("sg3_t7")).Visible = true; }
                            else { ((TextBox)gr.FindControl("sg3_t7")).Visible = false; }
                            if (hf15 != "" && hf15 != "-")
                            {
                                ((DropDownList)gr.FindControl("sg3_t6")).Items.FindByText(hf15).Selected = true;
                            }
                        }
                    }
                    #endregion
                    break;

                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    DDBind();
                    string mv_col;
                    mv_col = frm_mbr + frm_vty + col1;
                    ViewState["fstr"] = col1;
                    SQuery = "Select a.* from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.grade||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + col1 + "' ORDER BY a.morder";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt2 = new DataTable();
                    mq0 = "select name ,Type1  from type where id='I' and type1 like '0%' order by TYPE1";
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        txtvchnum.Text = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtgrade.Text = dt.Rows[i]["GRADE"].ToString().Trim();
                        txtgradenm.Text = fgen.seek_iname_dt(dt2, "type1=" + dt.Rows[i]["GRADE"].ToString().Trim() + "", "NAME");
                        txtlbl4a.Text = Convert.ToDateTime(dt.Rows[i]["EFF_TO"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4.Text = Convert.ToDateTime(dt.Rows[i]["EFF_FROM"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl7.Text = dt.Rows[i]["WEEK_OFF"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[i]["DAYN"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[i]["MINHRS"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["FST_START"].ToString().Trim();
                        txtlbl5.Text = dt.Rows[i]["MAXHRS"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["FST_END"].ToString().Trim();
                        txtunit.Text = dt.Rows[i]["SSFT_START"].ToString().Trim();
                        txtfc.Text = dt.Rows[i]["SSFT_END"].ToString().Trim();
                        txtstatus.Text = dt.Rows[i]["icat"].ToString().Trim();
                        txtovertm.Text = dt.Rows[i]["OT_DAYS"].ToString().Trim();
                        txtovertm2.Text = dt.Rows[i]["OT_DAYS2"].ToString().Trim();
                        //ddOT.SelectedItem.Text = dt.Rows[i]["OT_DIV"].ToString().Trim();
                        if (dt.Rows[i]["PF_DIV"].ToString() == "TOTDAYS" || dt.Rows[i]["PF_DIV"].ToString() == "WORKDAYS" || dt.Rows[i]["PF_DIV"].ToString() == "PLEASE SELECT") { DDPF.SelectedItem.Text = dt.Rows[i]["PF_DIV"].ToString().Trim(); }
                        else { txtddpf.Text = dt.Rows[i]["PF_DIV"].ToString(); DDPF.SelectedItem.Text = "OTHER"; txtddpf.Visible = true; }

                        if (dt.Rows[i]["ESI_DIV"].ToString() == "TOTDAYS" || dt.Rows[i]["ESI_DIV"].ToString() == "WORKDAYS" || dt.Rows[i]["ESI_DIV"].ToString() == "PLEASE SELECT") { DDESI.SelectedItem.Text = dt.Rows[i]["ESI_DIV"].ToString().Trim(); }
                        else { txtddesi.Text = dt.Rows[i]["ESI_DIV"].ToString(); DDESI.SelectedItem.Text = "OTHER"; txtddesi.Visible = true; }

                        if (dt.Rows[i]["WF_DIV"].ToString() == "TOTDAYS" || dt.Rows[i]["WF_DIV"].ToString() == "WORKDAYS" || dt.Rows[i]["WF_DIV"].ToString() == "PLEASE SELECT") { DDWF.SelectedItem.Text = dt.Rows[i]["WF_DIV"].ToString().Trim(); }
                        else { txtddwf.Text = dt.Rows[i]["WF_DIV"].ToString(); DDWF.SelectedItem.Text = "OTHER"; txtddwf.Visible = true; }

                        //if (dt.Rows[i]["ERN_DIV"].ToString() == "TOTDAYS" || dt.Rows[i]["ERN_DIV"].ToString() == "WORKDAYS" || dt.Rows[i]["ERN_DIV"].ToString() == "PLEASE SELECT") { ddern.SelectedItem.Text = dt.Rows[i]["ERN_DIV"].ToString().Trim(); }
                        //else { txtddern.Text = dt.Rows[i]["ERN_DIV"].ToString(); ddern.SelectedItem.Text = "OTHER"; txtddern.Visible = true; }

                        if (dt.Rows[i]["OT_DIV"].ToString() == "TOTDAYS" || dt.Rows[i]["OT_DIV"].ToString() == "WORKDAYS" || dt.Rows[i]["OT_DIV"].ToString() == "PLEASE SELECT") { ddOT.SelectedItem.Text = dt.Rows[i]["OT_DIV"].ToString().Trim(); }
                        else { txtOTErn.Text = dt.Rows[i]["OT_DIV"].ToString(); ddOT.SelectedItem.Text = "OTHER"; txtOTErn.Visible = true; }

                        if (dt.Rows[i]["OT_DIV2"].ToString() == "TOTDAYS" || dt.Rows[i]["OT_DIV2"].ToString() == "WORKDAYS" || dt.Rows[i]["OT_DIV2"].ToString() == "PLEASE SELECT") { ddOT2.SelectedItem.Text = dt.Rows[i]["OT_DIV2"].ToString().Trim(); }
                        else { txtOTErn2.Text = dt.Rows[i]["OT_DIV2"].ToString(); ddOT2.SelectedItem.Text = "OTHER"; txtOTErn2.Visible = true; }

                        //DDESI.SelectedItem.Text = dt.Rows[i]["ESI_DIV"].ToString().Trim(); 
                        //DDWF.SelectedItem.Text = dt.Rows[i]["WF_DIV"].ToString().Trim();

                        create_tab();
                        sg1_dr = null;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["ED_FLD"].ToString().Trim().Substring(0, 1) == "E")
                            {
                                sg1_dr = sg1_dt.NewRow();
                                sg1_dr["sg1_srno"] = srno;
                                sg1_dr["sg1_h1"] = "-";
                                sg1_dr["sg1_h2"] = "-";
                                sg1_dr["sg1_h3"] = "-";
                                sg1_dr["sg1_h4"] = "-";
                                sg1_dr["sg1_h5"] = "-";
                                sg1_dr["sg1_h6"] = "-";
                                sg1_dr["sg1_f1"] = dt.Rows[i]["ED_FLD"].ToString().Trim();
                                sg1_dr["sg1_f2"] = "";
                                sg1_dr["sg1_t1"] = dt.Rows[i]["ED_NAME"].ToString().Trim();
                                sg1_dr["sg1_t2"] = dt.Rows[i]["PF_YN"].ToString().Trim();
                                sg1_dr["sg1_t5"] = dt.Rows[i]["VPF_YN"].ToString().Trim();
                                sg1_dr["sg1_t6"] = dt.Rows[i]["ESI_YN"].ToString().Trim();
                                sg1_dr["sg1_t8"] = dt.Rows[i]["WF_YN"].ToString().Trim();
                                sg1_dr["sg1_t9"] = dt.Rows[i]["PT_YN"].ToString().Trim();
                                sg1_dr["sg1_t11"] = dt.Rows[i]["OT_YN"].ToString().Trim();
                                sg1_dr["sg1_t12"] = dt.Rows[i]["EL_YN"].ToString().Trim();
                                sg1_dr["sg1_t14"] = dt.Rows[i]["OT2_YN"].ToString().Trim();
                                if (dt.Rows[i]["ERN_DIV"].ToString() == "TOTDAYS" || dt.Rows[i]["ERN_DIV"].ToString() == "WORKDAYS" || dt.Rows[i]["ERN_DIV"].ToString() == "PLEASE SELECT")
                                {
                                    sg1_dr["sg1_t13"] = dt.Rows[i]["ERN_DIV"].ToString().Trim();
                                    sg1_dr["txtddern"] = "-";
                                }
                                else
                                {
                                    sg1_dr["txtddern"] = dt.Rows[i]["ERN_DIV"].ToString(); 
                                    sg1_dr["sg1_t13"] = "OTHER";
                                }
                                sg1_dt.Rows.Add(sg1_dr);
                                srno++;
                            }
                        }

                        //create_tab();
                        //sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        create_tab3();
                        sg3_dr = null;
                        int k = 1;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["ED_FLD"].ToString().Trim().Substring(0, 1) == "D")
                            {
                                sg3_dr = sg3_dt.NewRow();
                                sg3_dr["sg3_SrNo"] = k;
                                sg3_dr["sg3_f1"] = dt.Rows[i]["ED_FLD"].ToString().Trim();
                                sg3_dr["sg3_f2"] = "";
                                sg3_dr["sg3_t1"] = dt.Rows[i]["ED_NAME"].ToString().Trim();
                                //sg3_dr["sg3_t2"] = dt.Rows[i]["DFORM"].ToString().Trim();
                                sg3_dr["sg3_t3"] = fgen.make_double(dt.Rows[i]["RATE"].ToString().Trim());
                                sg3_dr["sg3_t4"] = fgen.make_double(dt.Rows[i]["EMPR_RATE"].ToString().Trim());
                                sg3_dr["sg3_t5"] = fgen.make_double(dt.Rows[i]["MAX_LMT"].ToString().Trim());
                                if (dt.Rows[i]["DED_DIV"].ToString() == "TOTDAYS" || dt.Rows[i]["DED_DIV"].ToString() == "WORKDAYS" || dt.Rows[i]["DED_DIV"].ToString() == "PLEASE SELECT" || dt.Rows[i]["DED_DIV"].ToString() == "N.A.")
                                {
                                    sg3_dr["sg3_t6"] = dt.Rows[i]["DED_DIV"].ToString().Trim();
                                    sg3_dr["sg3_t7"] = "-";
                                }
                                else
                                {
                                    sg3_dr["sg3_t7"] = dt.Rows[i]["DED_DIV"].ToString();
                                    sg3_dr["sg3_t6"] = "OTHER";
                                }
                                sg3_dt.Rows.Add(sg3_dr);
                                k++;
                            }
                        }
                        //------------------------
                        k = 0;
                        //sg3_add_blankrows();
                        ViewState["sg3"] = sg3_dt;
                        sg3.DataSource = sg3_dt;
                        sg3.DataBind();
                        dt.Dispose();
                        sg3_dt.Dispose();
                        //((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Focus();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                        foreach (GridViewRow gr in sg1.Rows)
                        {
                            string hf = ((HiddenField)gr.FindControl("cmd2")).Value;
                            string hf2 = ((HiddenField)gr.FindControl("cmd3")).Value;
                            string hf3 = ((HiddenField)gr.FindControl("cmd4")).Value;
                            string hf4 = ((HiddenField)gr.FindControl("cmd5")).Value;
                            string hf5 = ((HiddenField)gr.FindControl("cmd6")).Value;
                            string hf6 = ((HiddenField)gr.FindControl("cmd8")).Value;
                            string hf7 = ((HiddenField)gr.FindControl("cmd9")).Value;
                            string hf8 = ((HiddenField)gr.FindControl("cmd11")).Value;
                            string hf9 = ((HiddenField)gr.FindControl("cmd12")).Value;
                            string hf10 = ((HiddenField)gr.FindControl("cmd13")).Value;
                            string hf11 = ((HiddenField)gr.FindControl("cmd14")).Value;

                            if (hf == "Y") { ((CheckBox)gr.FindControl("sg1_t2")).Checked = true; }
                            if (hf2 == "Y") { ((CheckBox)gr.FindControl("sg1_t3")).Checked = true; }
                            if (hf3 == "Y") { ((CheckBox)gr.FindControl("sg1_t4")).Checked = true; }
                            if (hf4 == "Y") { ((CheckBox)gr.FindControl("sg1_t5")).Checked = true; }
                            if (hf5 == "Y") { ((CheckBox)gr.FindControl("sg1_t6")).Checked = true; }
                            if (hf6 == "Y") { ((CheckBox)gr.FindControl("sg1_t8")).Checked = true; }
                            if (hf7 == "Y") { ((CheckBox)gr.FindControl("sg1_t9")).Checked = true; }
                            if (hf8 == "Y") { ((CheckBox)gr.FindControl("sg1_t11")).Checked = true; }
                            if (hf9 == "Y") { ((CheckBox)gr.FindControl("sg1_t12")).Checked = true; }
                            if (hf10 == "OTHER") { ((TextBox)gr.FindControl("txtddern")).Visible = true; }
                            else { ((TextBox)gr.FindControl("txtddern")).Visible = false; }
                            if (hf10 != "" && hf10 != "-")
                            {
                                ((DropDownList)gr.FindControl("ddern")).Items.FindByText(hf10).Selected = true;
                            }
                            if (hf11 == "Y") { ((CheckBox)gr.FindControl("sg1_t14")).Checked = true; }
                        }

                        foreach (GridViewRow gr in sg3.Rows)
                        {
                            string hf15 = ((HiddenField)gr.FindControl("cmd15")).Value;
                            if (hf15 == "OTHER") { ((TextBox)gr.FindControl("sg3_t7")).Visible = true; }
                            else { ((TextBox)gr.FindControl("sg3_t7")).Visible = false; }
                            if (hf15 != "" && hf15 != "-")
                            {
                                ((DropDownList)gr.FindControl("sg3_t6")).Items.FindByText(hf15).Selected = true;
                            }
                        }
                    }
                    #endregion
                    break;

                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_invn_reps(frm_qstr);
                    break;
                //-----------------------------
                case "UNIT":
                    if (col1.Length <= 0) return;
                    txtunit.Text = col2;
                    //btnfc.Focus();
                    break;

                case "GRADE":
                    if (col1.Length <= 0) return;
                    txtgrade.Text = col1;
                    txtgradenm.Text = col2;
                    txtlbl4.Focus();
                    break;

                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    //txtlbl2.Focus();
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return; dt = new DataTable();
                    if (ViewState["sg1"] != null)
                    {
                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
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
                            sg1_dr["sg1_t2"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t2")).Checked;
                            sg1_dr["sg1_t3"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t3")).Checked;
                            sg1_dr["sg1_t4"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t4")).Checked;
                            sg1_dr["sg1_t5"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t5")).Checked;
                            sg1_dr["sg1_t6"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t6")).Checked;
                            ////sg1_dr["sg1_t7"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t8")).Checked;
                            sg1_dr["sg1_t9"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t9")).Checked;
                            ////sg1_dr["sg1_t10"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                            sg1_dr["sg1_t11"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t11")).Checked;
                            sg1_dr["sg1_t12"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t12")).Checked;
                            sg1_dr["sg1_t13"] = ((DropDownList)sg1.Rows[i].FindControl("ddern")).SelectedItem.Text.Trim();
                            sg1_dr["txtddern"] = ((TextBox)sg1.Rows[i].FindControl("txtddern")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t14")).Checked;
                            //sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            //sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            //sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                            //sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            //sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    if (sg1_dt.Rows.Count >= 13)
                    {
                        return;
                    }
                    else { sg1_add_blankrows(); }
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();

                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        string hf = ((HiddenField)gr.FindControl("cmd2")).Value;
                        string hf2 = ((HiddenField)gr.FindControl("cmd3")).Value;
                        string hf3 = ((HiddenField)gr.FindControl("cmd4")).Value;
                        string hf4 = ((HiddenField)gr.FindControl("cmd5")).Value;
                        string hf5 = ((HiddenField)gr.FindControl("cmd6")).Value;
                        string hf6 = ((HiddenField)gr.FindControl("cmd8")).Value;
                        string hf7 = ((HiddenField)gr.FindControl("cmd9")).Value;
                        string hf8 = ((HiddenField)gr.FindControl("cmd11")).Value;
                        string hf9 = ((HiddenField)gr.FindControl("cmd12")).Value;
                        string hf10 = ((HiddenField)gr.FindControl("cmd13")).Value;
                        string hf11 = ((HiddenField)gr.FindControl("cmd14")).Value;

                        if (hf == "True") { ((CheckBox)gr.FindControl("sg1_t2")).Checked = true; }
                        if (hf2 == "True") { ((CheckBox)gr.FindControl("sg1_t3")).Checked = true; }
                        if (hf3 == "True") { ((CheckBox)gr.FindControl("sg1_t4")).Checked = true; }
                        if (hf4 == "True") { ((CheckBox)gr.FindControl("sg1_t5")).Checked = true; }
                        if (hf5 == "True") { ((CheckBox)gr.FindControl("sg1_t6")).Checked = true; }
                        if (hf6 == "True") { ((CheckBox)gr.FindControl("sg1_t8")).Checked = true; }
                        if (hf7 == "True") { ((CheckBox)gr.FindControl("sg1_t9")).Checked = true; }
                        if (hf8 == "True") { ((CheckBox)gr.FindControl("sg1_t11")).Checked = true; }
                        if (hf9 == "True") { ((CheckBox)gr.FindControl("sg1_t12")).Checked = true; }
                        if (hf10 == "OTHER") { ((TextBox)gr.FindControl("txtddern")).Visible = true; }
                        else { ((TextBox)gr.FindControl("txtddern")).Visible = false; }
                        if (hf10 != "" && hf10 != "-")
                        {
                            ((DropDownList)gr.FindControl("ddern")).Items.FindByText(hf10).Selected = true;
                        }
                        if (hf11 == "True") { ((CheckBox)gr.FindControl("sg1_t14")).Checked = true; }
                    }
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    if (edmode.Value == "Y")
                    {
                        //return;
                    }
                    SQuery = "select trim(name) as HS_name, trim(acref) as hs_code from typegrp where trim(acref) = '" + col1 + "' order by acref";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        //********* Saving in GridView Value
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[0]["hs_code"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[0]["HS_name"].ToString().Trim();

                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = "";
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = "";
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = "";
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = "";
                    }
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
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = Convert.ToInt32(dt.Rows[i]["sg3_srno"].ToString());
                            sg3_dr["sg3_f1"] = dt.Rows[i]["sg3_f1"].ToString();
                            sg3_dr["sg3_f2"] = dt.Rows[i]["sg3_f2"].ToString();
                            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();
                            sg3_dr["sg3_t6"] = ((DropDownList)sg3.Rows[i].FindControl("sg3_t6")).SelectedItem.Text.Trim();
                            sg3_dr["sg3_t7"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t7")).Text.Trim();
                            sg3_dt.Rows.Add(sg3_dr);
                        }                        
                    }
                    if (sg3_dt.Rows.Count >= 13)
                    {
                        return;
                    }
                    else { sg3_add_blankrows(); }                   
                    ViewState["sg3"] = sg3_dt;
                    sg3.DataSource = sg3_dt;
                    sg3.DataBind();
                    dt.Dispose(); sg3_dt.Dispose();
                    ((TextBox)sg3.Rows[z].FindControl("sg3_t1")).Focus();
                    foreach (GridViewRow gr in sg3.Rows)
                    {
                        string hf15 = ((HiddenField)gr.FindControl("cmd15")).Value;
                        if (hf15 == "OTHER") { ((TextBox)gr.FindControl("sg3_t7")).Visible = true; }
                        else { ((TextBox)gr.FindControl("sg3_t7")).Visible = false; }
                        if (hf15 != "" && hf15 != "-")
                        {
                            ((DropDownList)gr.FindControl("sg3_t6")).Items.FindByText(hf15).Selected = true;
                        }
                    }
                    #endregion
                    break;

                case "SG3_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    if (edmode.Value == "Y")
                    {
                        //return;
                    }
                    SQuery = "select trim(name) as HS_name, trim(acref) as hs_code from typegrp where trim(acref) = '" + col1 + "' order by acref";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        //********* Saving in GridView Value
                        sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[0]["hs_code"].ToString().Trim();
                        sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[0]["HS_name"].ToString().Trim();

                        ((TextBox)sg3.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg3_t1")).Text = "";
                        ((TextBox)sg3.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg3_t2")).Text = "";
                        ((TextBox)sg3.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg3_t3")).Text = "";
                        ((TextBox)sg3.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg3_t4")).Text = "";
                    }
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
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = (i + 1);
                            sg3_dr["sg3_f1"] = sg3.Rows[i].Cells[3].Text.Trim();
                            sg3_dr["sg3_f2"] = sg3.Rows[i].Cells[4].Text.Trim();
                            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();
                            sg3_dr["sg3_t6"] = ((DropDownList)sg3.Rows[i].FindControl("sg3_t6")).SelectedItem.Text.Trim();
                            sg3_dr["sg3_t7"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t7")).Text.Trim();
                            sg3_dt.Rows.Add(sg3_dr);
                        }
                        sg3_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg3_add_blankrows();
                        ViewState["sg3"] = sg3_dt;
                        sg3.DataSource = sg3_dt;
                        sg3.DataBind();
                        i = 1;
                        foreach (GridViewRow gr in sg3.Rows)
                        {
                            string hf15 = ((HiddenField)gr.FindControl("cmd15")).Value;
                            if (hf15 == "OTHER") { ((TextBox)gr.FindControl("sg3_t7")).Visible = true; }
                            else { ((TextBox)gr.FindControl("sg3_t7")).Visible = false; }
                            if (hf15 != "" && hf15 != "-")
                            {
                                ((DropDownList)gr.FindControl("sg3_t6")).Items.FindByText(hf15).Selected = true;
                            }
                            gr.Cells[2].Text = (i + 1).ToString();
                            i++;
                        }
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
                        for (i = 0; i < dt.Rows.Count; i++)
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
                            sg1_dr["sg1_t2"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t2")).Checked;
                            sg1_dr["sg1_t3"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t3")).Checked;
                            sg1_dr["sg1_t4"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t4")).Checked;
                            sg1_dr["sg1_t5"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t5")).Checked;
                            sg1_dr["sg1_t6"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t6")).Checked;
                            //sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t8")).Checked;
                            sg1_dr["sg1_t9"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t9")).Checked;
                            //sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                            sg1_dr["sg1_t11"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t11")).Checked;
                            sg1_dr["sg1_t12"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t12")).Checked;
                            sg1_dr["sg1_t13"] = ((DropDownList)sg1.Rows[i].FindControl("ddern")).SelectedItem.Text.Trim();
                            sg1_dr["txtddern"] = ((TextBox)sg1.Rows[i].FindControl("txtddern")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((CheckBox)sg1.Rows[i].FindControl("sg1_t14")).Checked;
                            //sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            //sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            //sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                            //sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            //sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                       // sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        i = 0;
                        foreach (GridViewRow gr in sg1.Rows)
                        {
                            gr.Cells[12].Text = (i + 1).ToString();
                            string hf = ((HiddenField)gr.FindControl("cmd2")).Value;
                            string hf2 = ((HiddenField)gr.FindControl("cmd3")).Value;
                            string hf3 = ((HiddenField)gr.FindControl("cmd4")).Value;
                            string hf4 = ((HiddenField)gr.FindControl("cmd5")).Value;
                            string hf5 = ((HiddenField)gr.FindControl("cmd6")).Value;
                            string hf6 = ((HiddenField)gr.FindControl("cmd8")).Value;
                            string hf7 = ((HiddenField)gr.FindControl("cmd9")).Value;
                            string hf8 = ((HiddenField)gr.FindControl("cmd11")).Value;
                            string hf9 = ((HiddenField)gr.FindControl("cmd12")).Value;
                            string hf10 = ((HiddenField)gr.FindControl("cmd13")).Value;
                            string hf11 = ((HiddenField)gr.FindControl("cmd14")).Value;

                            if (hf == "True") { ((CheckBox)gr.FindControl("sg1_t2")).Checked = true; }
                            if (hf2 == "True") { ((CheckBox)gr.FindControl("sg1_t3")).Checked = true; }
                            if (hf3 == "True") { ((CheckBox)gr.FindControl("sg1_t4")).Checked = true; }
                            if (hf4 == "True") { ((CheckBox)gr.FindControl("sg1_t5")).Checked = true; }
                            if (hf5 == "True") { ((CheckBox)gr.FindControl("sg1_t6")).Checked = true; }
                            if (hf6 == "True") { ((CheckBox)gr.FindControl("sg1_t8")).Checked = true; }
                            if (hf7 == "True") { ((CheckBox)gr.FindControl("sg1_t9")).Checked = true; }
                            if (hf8 == "True") { ((CheckBox)gr.FindControl("sg1_t11")).Checked = true; }
                            if (hf9 == "True") { ((CheckBox)gr.FindControl("sg1_t12")).Checked = true; }
                            if (hf10 == "OTHER") { ((TextBox)gr.FindControl("txtddern")).Visible = true; }
                            else { ((TextBox)gr.FindControl("txtddern")).Visible = false; }
                            if (hf10 != "" && hf10 != "-")
                            {
                                ((DropDownList)gr.FindControl("ddern")).Items.FindByText(hf10).Selected = true;
                            }
                            if (hf11 == "True") { ((CheckBox)gr.FindControl("sg1_t14")).Checked = true; }
                            i++;
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
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "SELECT trim(a.vchnum) as ENTRY_No ,to_char(a.vchdate,'dd/mm/yyyy') as ENTRY_dAtE,A.GRADE,B.NAME AS GRADE_NAME,A.ED_FLD,A.ED_NAME,to_char(A.EFF_FROM,'DD/MM/YYYY') AS EFFECTIVE_FROM,to_char(A.EFF_TO,'DD/MM/YYYY') AS EFFECTIVE_UPTO,A.PF_YN ,A.VPF_YN,A.WF_YN,A.PT_YN,A.OT_YN,A.EL_YN,NVL(A.EMPR_RATE,0) AS EMPR_RATE,A.RATE AS EMP_RATE,A.MAX_LMT,to_char(a.vchdate,'yyyymmdd') as vdd FROM " + frm_tabname + " a,TYPE B where TRIM(A.GRADE)=TRIM(B.TYPE1) AND a.branchcd='" + frm_mbr + "' and a.type='10' AND B.ID='I' AND B.TYPE1 LIKE '0%' and a.vchdate " + PrdRange + " order by vdd desc ,trim(a.vchnum) desc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Advance Licence Master  For the Period " + fromdt + " to " + todt, frm_qstr);
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------           

            string last_entdt;
            //checks
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + " ", "ldt");
            if (last_entdt == "0") { }
            else if (edmode.Value != "Y")
            {
                if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
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
                        save_fun2();
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
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
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
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||grade||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||grade||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
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
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); DDClear();
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
        sg1_dt.Columns.Add(new DataColumn("txtddern", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t18", typeof(string)));

    }
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
        sg3_dt.Columns.Add(new DataColumn("sg3_t5", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t6", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t7", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t8", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t9", typeof(string)));

    }
    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        if (sg1_dt == null) return;
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
        int db3 = sg1_dt.Rows.Count + 1;
        sg1_dr["sg1_f1"] = "ER" + db3;
        sg1_dr["sg1_f2"] = "-";
        sg1_dr["sg1_f3"] = "-";
        sg1_dr["sg1_f4"] = "-";
        sg1_dr["sg1_f5"] = "-";
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
        sg1_dr["sg1_t16"] = "";
        sg1_dr["txtddern"] = "-";
        //sg1_dr["sg1_t18"] = "-";
        sg1_dt.Rows.Add(sg1_dr);
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
        sg3_dr["sg3_t5"] = "-";
        sg3_dr["sg3_t6"] = "-";
        sg3_dr["sg3_t7"] = "-";
        sg3_dr["sg3_t8"] = "-";
        sg3_dr["sg3_t9"] = "-";
        int db4 = sg3_dt.Rows.Count + 1;
        sg3_dr["sg3_f1"] = "DED" + db4;
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
                if (sg1r < 1)
                {
                    ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t1")).ReadOnly = true;
                    sg1.Rows[sg1r].Cells[10].Enabled = false;
                    sg1.Rows[sg1r].Cells[11].Enabled = false;
                }
            }
            sg1.Columns[10].HeaderStyle.Width = 40;
            sg1.Columns[11].HeaderStyle.Width = 40;
            sg1.Columns[12].HeaderStyle.Width = 60;
            //sg1.Columns[13].HeaderStyle.Width = 100;
            //sg1.Columns[14].HeaderStyle.Width = 80;
            //sg1.Columns[15].HeaderStyle.Width = 80;
            //sg1.Columns[16].HeaderStyle.Width = 80;
            //sg1.Columns[17].HeaderStyle.Width = 80;
            sg1.Columns[18].HeaderStyle.Width = 120;
            //sg1.Columns[19].HeaderStyle.Width = 120;
            //sg1.Columns[20].HeaderStyle.Width = 120;
            //sg1.Columns[21].HeaderStyle.Width = 120;
            //sg1.Columns[22].HeaderStyle.Width = 120;
            //sg1.Columns[23].HeaderStyle.Width = 120;
            //sg1.Columns[24].HeaderStyle.Width = 120;
            //sg1.Columns[25].HeaderStyle.Width = 120;
            //sg1.Columns[26].HeaderStyle.Width = 120;
            //sg1.Columns[27].HeaderStyle.Width = 120;
            //sg1.Columns[28].HeaderStyle.Width = 120;
            sg1.Columns[30].HeaderStyle.Width = 100;
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
                if (index < sg1.Rows.Count)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This From The List");
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
                    // make_qry_4_popup();
                    // fgen.Fn_open_sseek("Select HSN Code", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    btnhideF_Click(sender, e);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg3r = 0; sg3r < sg3.Rows.Count; sg3r++)
            {
                for (int j = 0; j < sg3.Columns.Count; j++)
                {
                    sg3.Rows[sg3r].Cells[j].ToolTip = sg3.Rows[sg3r].Cells[j].Text;
                    if (sg3.Rows[sg3r].Cells[j].Text.Trim().Length > 35)
                    {
                        sg3.Rows[sg3r].Cells[j].Text = sg3.Rows[sg3r].Cells[j].Text.Substring(0, 35);
                    }
                }
                if (sg3r < 10)
                {
                    ((TextBox)sg3.Rows[sg3r].FindControl("sg3_t1")).ReadOnly = true;
                    sg3.Rows[sg3r].Cells[0].Enabled = false;
                    sg3.Rows[sg3r].Cells[1].Enabled = false;
                }
                //if (sg3r == 3) { ((TextBox)sg3.Rows[sg3r].FindControl("sg3_t3")).ReadOnly = true; }
                //if (sg3r == 4) { ((TextBox)sg3.Rows[sg3r].FindControl("sg3_t3")).ReadOnly = true; }
                if (sg3r == 6)
                {
                    ((TextBox)sg3.Rows[sg3r].FindControl("sg3_t1")).ReadOnly = false;
                    sg3.Rows[sg3r].Cells[0].Enabled = true;
                    sg3.Rows[sg3r].Cells[1].Enabled = true;
                }
                if (sg3r == 7)
                {
                    ((TextBox)sg3.Rows[sg3r].FindControl("sg3_t1")).ReadOnly = false;
                    sg3.Rows[sg3r].Cells[0].Enabled = true;
                    sg3.Rows[sg3r].Cells[1].Enabled = true;
                }
                if (sg3r == 8)
                {
                    ((TextBox)sg3.Rows[sg3r].FindControl("sg3_t1")).ReadOnly = false;
                    sg3.Rows[sg3r].Cells[0].Enabled = true;
                    sg3.Rows[sg3r].Cells[1].Enabled = true;
                }
                if (sg3r == 9)
                {
                    ((TextBox)sg3.Rows[sg3r].FindControl("sg3_t1")).ReadOnly = true;
                    sg3.Rows[sg3r].Cells[0].Enabled = true;
                }
            }
            sg3.Columns[0].HeaderStyle.Width = 40;
            sg3.Columns[1].HeaderStyle.Width = 40;
            sg3.Columns[2].HeaderStyle.Width = 60;
            sg3.Columns[3].HeaderStyle.Width = 100;
            sg3.Columns[5].HeaderStyle.Width = 150;
            sg3.Columns[6].HeaderStyle.Width = 220;
            sg3.Columns[7].HeaderStyle.Width = 100;
            sg3.Columns[8].HeaderStyle.Width = 150;
            sg3.Columns[9].HeaderStyle.Width = 150;
            sg3.Columns[10].HeaderStyle.Width = 150;
            sg3.Columns[11].HeaderStyle.Width = 100;
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
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This From The List");
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
                    //make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG3_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    btnhideF_Click(sender, e);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------   
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
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
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Weekly Off Day ", frm_qstr);
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
            //if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().Length > 1)
            //{
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["MORDER"] = i + 1;
            oporow["vchnum"] = frm_vnum.Trim();
            oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow["GRADE"] = txtgrade.Text.Trim().ToUpper();
            oporow["EFF_FROM"] = Convert.ToDateTime(txtlbl4.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
            oporow["EFF_TO"] = Convert.ToDateTime(txtlbl4a.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
            oporow["WEEK_OFF"] = txtlbl7.Text.Trim().ToUpper();
            oporow["DAYN"] = txtlbl7a.Text.Trim().ToUpper();
            oporow["MINHRS"] = fgen.make_double(txtlbl2.Text.Trim().ToUpper());
            oporow["MAXHRS"] = fgen.make_double(txtlbl5.Text.Trim().ToUpper());
            oporow["FST_START"] = txtlbl3.Text.Trim().ToUpper();
            oporow["SSFT_START"] = txtunit.Text.Trim().ToUpper();
            oporow["FST_END"] = txtlbl6.Text.Trim().ToUpper();
            oporow["SSFT_END"] = txtfc.Text.Trim().ToUpper();
            oporow["ICAT"] = txtstatus.Text.Trim().ToUpper();
            oporow["ED_NAME"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
            oporow["ED_FLD"] = sg1.Rows[i].Cells[13].Text.Trim().ToUpper();
            oporow["PF_YN"] = (((CheckBox)sg1.Rows[i].FindControl("sg1_t2")).Checked == true) ? "Y" : "N";
            oporow["VPF_YN"] = (((CheckBox)sg1.Rows[i].FindControl("sg1_t5")).Checked == true) ? "Y" : "N";
            oporow["ESI_YN"] = (((CheckBox)sg1.Rows[i].FindControl("sg1_t6")).Checked == true) ? "Y" : "N";
            oporow["WF_YN"] = (((CheckBox)sg1.Rows[i].FindControl("sg1_t8")).Checked == true) ? "Y" : "N";
            oporow["PT_YN"] = (((CheckBox)sg1.Rows[i].FindControl("sg1_t9")).Checked == true) ? "Y" : "N";
            oporow["OT_YN"] = (((CheckBox)sg1.Rows[i].FindControl("sg1_t11")).Checked == true) ? "Y" : "N";
            oporow["EL_YN"] = (((CheckBox)sg1.Rows[i].FindControl("sg1_t12")).Checked == true) ? "Y" : "N";
            oporow["OT2_YN"] = (((CheckBox)sg1.Rows[i].FindControl("sg1_t14")).Checked == true) ? "Y" : "N";
            oporow["OT_DAYS"] = fgen.make_double(txtovertm.Text.Trim().ToUpper());
            //oporow["OT_DIV"] = ddOT.SelectedItem.Text.Trim().ToUpper();
            oporow["OT_DIV"] = (ddOT.SelectedItem.Text.Trim() == "OTHER") ? txtOTErn.Text.Trim() : ddOT.SelectedItem.Text.Trim().ToUpper();
            oporow["PF_DIV"] = (DDPF.SelectedItem.Text.Trim() == "OTHER") ? txtddpf.Text.Trim() : DDPF.SelectedItem.Text.Trim().ToUpper();
            oporow["ESI_DIV"] = (DDESI.SelectedItem.Text.Trim() == "OTHER") ? txtddesi.Text.Trim() : DDESI.SelectedItem.Text.Trim().ToUpper();
            oporow["WF_DIV"] = (DDWF.SelectedItem.Text.Trim() == "OTHER") ? txtddwf.Text.Trim() : DDWF.SelectedItem.Text.Trim().ToUpper();

            // oporow["ERN_DIV"] = (ddern.SelectedItem.Text.Trim() == "OTHER") ? txtddern.Text.Trim() : ddern.SelectedItem.Text.Trim().ToUpper();
            oporow["ERN_DIV"] = (((DropDownList)sg1.Rows[i].FindControl("ddern")).SelectedItem.Text.Trim() == "OTHER") ? ((TextBox)sg1.Rows[i].FindControl("txtddern")).Text : ((DropDownList)sg1.Rows[i].FindControl("ddern")).SelectedItem.Text.Trim();

            oporow["OT_DAYS2"] = fgen.make_double(txtovertm2.Text.Trim().ToUpper());
            oporow["OT_DIV2"] = (ddOT2.SelectedItem.Text.Trim() == "OTHER") ? txtOTErn2.Text.Trim() : ddOT2.SelectedItem.Text.Trim().ToUpper();
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
            // }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        for (i = 0; i < sg3.Rows.Count; i++)
        {
            //if (((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim().Length > 1)
            //{
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["MORDER"] = i + 1;
            oporow["vchnum"] = frm_vnum.Trim();
            oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow["GRADE"] = txtgrade.Text.Trim().ToUpper();
            oporow["EFF_FROM"] = Convert.ToDateTime(txtlbl4.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
            oporow["EFF_TO"] = Convert.ToDateTime(txtlbl4a.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
            oporow["WEEK_OFF"] = txtlbl7.Text.Trim().ToUpper();
            oporow["DAYN"] = txtlbl7a.Text.Trim().ToUpper();
            oporow["MINHRS"] = fgen.make_double(txtlbl2.Text.Trim().ToUpper());
            oporow["MAXHRS"] = fgen.make_double(txtlbl5.Text.Trim().ToUpper());
            oporow["FST_START"] = txtlbl3.Text.Trim().ToUpper();
            oporow["SSFT_START"] = txtunit.Text.Trim().ToUpper();
            oporow["FST_END"] = txtlbl6.Text.Trim().ToUpper();
            oporow["SSFT_END"] = txtfc.Text.Trim().ToUpper();
            oporow["PF_YN"] = "-";
            oporow["VPF_YN"] = "-";
            oporow["ESI_YN"] = "-";
            oporow["WF_YN"] = "-";
            oporow["PT_YN"] = "-";
            oporow["OT_YN"] = "-";
            oporow["OT2_YN"] = "-";
            oporow["EL_YN"] = "-";
            oporow["ICAT"] = txtstatus.Text.Trim().ToUpper();
            oporow["ED_FLD"] = sg3.Rows[i].Cells[3].Text.Trim().ToUpper();
            oporow["ED_NAME"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim().ToUpper();
            //oporow["DFORM"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim().ToUpper();
            oporow["RATE"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim().ToUpper());
            oporow["EMPR_RATE"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim().ToUpper());
            oporow["MAX_LMT"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t5")).Text.Trim().ToUpper());
            oporow["OT_DAYS"] = fgen.make_double(txtovertm.Text.Trim().ToUpper());
            //oporow["OT_DIV"] = ddOT.SelectedItem.Text.Trim().ToUpper();
            oporow["OT_DIV"] = (ddOT.SelectedItem.Text.Trim() == "OTHER") ? txtOTErn.Text.Trim() : ddOT.SelectedItem.Text.Trim().ToUpper();
            oporow["PF_DIV"] = (DDPF.SelectedItem.Text.Trim() == "OTHER") ? txtddpf.Text.Trim() : DDPF.SelectedItem.Text.Trim().ToUpper();
            oporow["ESI_DIV"] = (DDESI.SelectedItem.Text.Trim() == "OTHER") ? txtddesi.Text.Trim() : DDESI.SelectedItem.Text.Trim().ToUpper();
            oporow["WF_DIV"] = (DDWF.SelectedItem.Text.Trim() == "OTHER") ? txtddwf.Text.Trim() : DDWF.SelectedItem.Text.Trim().ToUpper();

            //oporow["ERN_DIV"] = (ddern.SelectedItem.Text.Trim() == "OTHER") ? txtddern.Text.Trim() : ddern.SelectedItem.Text.Trim().ToUpper();
            oporow["ERN_DIV"] = "-";
            oporow["OT_DAYS2"] = fgen.make_double(txtovertm2.Text.Trim().ToUpper());
            oporow["DED_DIV"] = (((DropDownList)sg3.Rows[i].FindControl("sg3_t6")).SelectedItem.Text.Trim() == "OTHER") ? ((TextBox)sg3.Rows[i].FindControl("sg3_t7")).Text : ((DropDownList)sg3.Rows[i].FindControl("sg3_t6")).SelectedItem.Text.Trim();

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
    }
    //------------------------------------------------------------------------------------
    protected void btnunit_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "UNIT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Import Unit ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnfc_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "IMPFC";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Import Currency ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btngrade_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "GRADE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Grade", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    public void DDClear()
    {
        ddOT.Items.Clear();
        DDPF.Items.Clear();
        DDESI.Items.Clear();
        DDWF.Items.Clear();
      //  ddern.Items.Clear();
        ddOT2.Items.Clear();
    }
    //------------------------------------------------------------------------------------
    public void DDBind()
    {
        DDClear();
        ddOT.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        ddOT.Items.Add(new System.Web.UI.WebControls.ListItem("WORKDAYS", "WORKDAYS"));
        ddOT.Items.Add(new System.Web.UI.WebControls.ListItem("TOTDAYS", "TOTDAYS"));
        ddOT.Items.Add(new System.Web.UI.WebControls.ListItem("OTHER", "OTHER"));
        // dd_Item.Items.Add(new System.Web.UI.WebControls.ListItem("N/A", "N/A"));

        DDPF.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        DDPF.Items.Add(new System.Web.UI.WebControls.ListItem("WORKDAYS", "WORKDAYS"));
        DDPF.Items.Add(new System.Web.UI.WebControls.ListItem("TOTDAYS", "TOTDAYS"));
        DDPF.Items.Add(new System.Web.UI.WebControls.ListItem("OTHER", "OTHER"));
        //DDPF.Items.Add(new System.Web.UI.WebControls.ListItem("N/A", "N/A"));

        DDESI.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        DDESI.Items.Add(new System.Web.UI.WebControls.ListItem("WORKDAYS", "WORKDAYS"));
        DDESI.Items.Add(new System.Web.UI.WebControls.ListItem("TOTDAYS", "TOTDAYS"));
        DDESI.Items.Add(new System.Web.UI.WebControls.ListItem("OTHER", "OTHER"));
        //DDESI.Items.Add(new System.Web.UI.WebControls.ListItem("N/A", "N/A"));

        DDWF.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        DDWF.Items.Add(new System.Web.UI.WebControls.ListItem("WORKDAYS", "WORKDAYS"));
        DDWF.Items.Add(new System.Web.UI.WebControls.ListItem("TOTDAYS", "TOTDAYS"));
        DDWF.Items.Add(new System.Web.UI.WebControls.ListItem("OTHER", "OTHER"));

        //ddern.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        //ddern.Items.Add(new System.Web.UI.WebControls.ListItem("WORKDAYS", "WORKDAYS"));
        //ddern.Items.Add(new System.Web.UI.WebControls.ListItem("TOTDAYS", "TOTDAYS"));
        //ddern.Items.Add(new System.Web.UI.WebControls.ListItem("OTHER", "OTHER"));

        ddOT2.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        ddOT2.Items.Add(new System.Web.UI.WebControls.ListItem("WORKDAYS", "WORKDAYS"));
        ddOT2.Items.Add(new System.Web.UI.WebControls.ListItem("TOTDAYS", "TOTDAYS"));
        ddOT2.Items.Add(new System.Web.UI.WebControls.ListItem("OTHER", "OTHER"));
    }
    //------------------------------------------------------------------------------------
    protected void DDPF_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDPF.SelectedItem.Text == "OTHER")
        {
            txtddpf.Visible = true;
        }
        else { txtddpf.Visible = false; }
    }
    //------------------------------------------------------------------------------------
    protected void DDWF_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDWF.SelectedItem.Text == "OTHER")
        {
            txtddwf.Visible = true;
        }
        else { txtddwf.Visible = false; }
    }
    //------------------------------------------------------------------------------------
    protected void DDESI_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDESI.SelectedItem.Text == "OTHER")
        {
            txtddesi.Visible = true;
        }
        else { txtddesi.Visible = false; }
    }
    //------------------------------------------------------------------------------------
    protected void ddern_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddern.SelectedItem.Text == "OTHER")
        //{
        //    txtddern.Visible = true;
        //}
        //else { txtddern.Visible = false; }        
        DropDownList ddl = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddl.NamingContainer;
        int rowIndex = row.RowIndex;
        if (ddl.SelectedItem.Text == "OTHER")
        {
            sg1.Rows[rowIndex].FindControl("txtddern").Visible = true;
        }
        else
        {
            sg1.Rows[rowIndex].FindControl("txtddern").Visible = false;
        }
    }
    //------------------------------------------------------------------------------------
    protected void ddOT_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddOT.SelectedItem.Text == "OTHER")
        {
            txtOTErn.Visible = true;
        }
        else { txtOTErn.Visible = false; }
    }
    //------------------------------------------------------------------------------------
    protected void ddOT2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddOT2.SelectedItem.Text == "OTHER")
        {
            txtOTErn2.Visible = true;
        }
        else { txtOTErn2.Visible = false; }
    }
    //------------------------------------------------------------------------------------
    protected void sg3_t6_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddl.NamingContainer;
        int rowIndex = row.RowIndex;
        if (ddl.SelectedItem.Text == "OTHER")
        {
            sg3.Rows[rowIndex].FindControl("sg3_t7").Visible = true;
        }
        else
        {
            sg3.Rows[rowIndex].FindControl("sg3_t7").Visible = false;
        }
    }
    //------------------------------------------------------------------------------------
}