using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;

public partial class om_emp_mas67 : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "N";
    DataTable dt, dt1, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0; string mq0, mq1, mq2, mq3;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok; string grade;
    string save_it;
    string html_body = "";
    string Prg_Id, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
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
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
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
            if (lblUpload.Text.Length > 1)
            {
                btnView1.Visible = true;
                btnDwnld1.Visible = true;
            }
            typePopup = "Y";
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
        #region hide hidden columns
        sg1.Columns[0].Visible = false;
        sg1.Columns[1].Visible = false;
        sg1.Columns[2].Visible = false;
        sg1.Columns[3].Visible = false;
        sg1.Columns[4].Visible = false;
        sg1.Columns[5].Visible = false;
        sg1.Columns[6].Visible = false;
        sg1.Columns[7].Visible = false;
        sg1.Columns[8].Visible = false;
        sg1.Columns[9].Visible = false;
        #endregion
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
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));
                //((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("readonly", "readonly");
                //((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("readonly", "readonly");
                //((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("readonly", "readonly");
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

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
        if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
        {
            tab7.Visible = true;
        }
        else
        {
            tab7.Visible = false;
        }
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false;
        btnprint.Disabled = false;
        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();
        sg1_add_blankrows();
        sg3_add_blankrows();
        sg4_add_blankrows();
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg4.DataSource = sg4_dt; sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
        ImageButton_2.Enabled = false; ImageButton1.Enabled = false; ImageButton12.Enabled = false; ImageButton13.Enabled = false;
        ImageButton19.Enabled = false; ImageButton2.Enabled = false; ImageButton20.Enabled = false; ImageButton21.Enabled = false;
        ImageButton22.Enabled = false; ImageButton23.Enabled = false; ImageButton24.Enabled = false; ImageButton25.Enabled = false; ImageButton26.Enabled = false;
        ImageButton27.Enabled = false; btnPresentsame.Enabled = false; btnPermanentsame.Enabled = false; btnCTC.Enabled = false; imgBranch.Enabled = false;
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
        ImageButton_2.Enabled = true; ImageButton1.Enabled = true; ImageButton12.Enabled = true; ImageButton13.Enabled = true;
        ImageButton19.Enabled = true; ImageButton2.Enabled = true; ImageButton20.Enabled = true; ImageButton21.Enabled = true;
        ImageButton22.Enabled = true; ImageButton23.Enabled = true; ImageButton24.Enabled = true; ImageButton25.Enabled = true; ImageButton26.Enabled = true;
        ImageButton27.Enabled = true; btnPresentsame.Enabled = true; btnPermanentsame.Enabled = true; btnCTC.Enabled = true; imgBranch.Enabled = true;
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
        lblheader.Text = "Employee Master";
        doc_nf.Value = "empcode";
        doc_df.Value = "empcode";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "EMPMAS";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        if (frm_cocd == "HGLO")
        {
            HGLO1.Visible = true;
            HGLO2.Visible = true;
            HGLO3.Visible = true;
            OTAfter.Visible = true;
            txt_OTAfter.Visible = true;
            imgBranch.Visible = false;
            HGLO5.Visible = true;
            HGLO6.Visible = true;
        }
        else
        {
            HGLO1.Visible = false;
            HGLO2.Visible = false;
            HGLO3.Visible = false;
            OTAfter.Visible = false;
            txt_OTAfter.Visible = false;
            HGLO5.Visible = false;
            HGLO6.Visible = false;
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
            case "ACNBUT":
                SQuery = "select trim(type1) as fstr,type1 as code,name as Department from typegrp where id='HT' order by code";
                break;

            case "MGRBUT"://section wise
                SQuery = "select trim(type1) as fstr,Type1 as Code,name as Section from typegrp where id='SM' order by code";
                break;

            case "SCHBUT":
                SQuery = "select trim(type1) as fstr ,type1 as code,name as Designation from typegrp where id='HD' order by code";
                break;

            case "DISTBUT"://reports to
                SQuery = "Select trim(Userid) as fstr,Username,Userid,trim(substr(deptt,4,20)) as Deptt from evas order by Username";
                break;

            case "STATBUT":
            case "STATBUT1":
                SQuery = "select trim(type1) as fstr ,name as State_Name ,type1 as code from type where id='{' order by Name";
                break;

            case "ZONEBUT":
                SQuery = "Select distinct trim(scale) as fstr,scale from EMPMAS where branchcd!='DD' order by Scale";
                break;

            case "CTRYBUT":
            case "CTRYBUT1":
                SQuery = "select trim(type1) as fstr,name as Country,type1 as code from typegrp where branchcd!='DD' and id='CN' order by name ";
                break;

            case "CONTBUT":
                SQuery = "select trim(type1) as fstr,name as Dispensary,Type1 as Code from typegrp where id='DS' order by Dispensary";
                break;

            case "PMODEBUT":
                SQuery = "select 'CHEQUE' as fstr,'CHEQUE' as choice,'-' as dash from dual union all select 'B/TFR' as fstr,'B/TFR' as choice,'-' as dash from dual union all select 'CASH' as fstr,'CASH' as choice ,'-' as dash from dual";
                break;
            case "ACCOUNTBUT":
                SQuery = "select 'SALARY' as fstr,'SALARY' as choice,'-' as dash from dual union all select 'WAGES' as fstr,'WAGES' as choice,'-' as dash from dual";
                break;

            case "EMPTYPEBUT":
                SQuery = "Select Name,type1 as Code from typegrp where id='KL' order by Acref";
                break;

            case "SKILLBUT":
                SQuery = "Select trim(type1) as fstr, Name,type1 as Code from typegrp where id='KL' order by Acref";
                break;

            case "MBUT":
                SQuery = "Select distinct Op_mach as Name,Op_mach as Code,BRANCHCD AS BRANCHCODE from EMPMAS where branchcd!='DD' order by Op_mach";
                break;

            case "ACODEBUT":
                SQuery = "Select trim(Acode) as fstr,Aname as Name,Acode as Code from famst where substr(acode,1,2)='14'";
                break;

            case "EMPBUT":
                SQuery = "select 'DIRECT' as fstr,'DIRECT' as choice,'-' as dash from dual union all select 'INDIRECT' as fstr,'INDIRECT' as choice,'-' as dash from dual";
                break;

            case "BRANCH":
                SQuery = "SELECT TRIM(TYPE1) AS FSTR,NAME AS BRANCH,TYPE1 AS CODE FROM TYPE WHERE ID='B' ORDER BY CODE";
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":

                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":

                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
            case "List":
                Type_Sel_query();
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                SQuery = "select trim(branchcd)||trim(grade)||trim(empcode) as fstr,name as emp_name,empcode as emp_code,fhname as father_name,desg_text as desig,deptt_text as department,ent_by,ent_dt,conf_dt,cardno,old_empc from " + frm_tabname + "  where  branchcd='" + frm_mbr + "' AND GRADE='" + frm_vty + "' order by emp_code";
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
            if (typePopup == "N")
            {
                newCase(frm_vty);
            }
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Grade", frm_qstr);
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        frm_vty = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(substr(empcode,3,6)) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND grade='" + col1 + "' ", 4, "VCH");
        txt_empcode.Value = col1 + frm_vnum;
        txt_joining.Text = vardate;
        txt_dob.Text = vardate;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        disablectrl();
        fgen.EnableForm(this.Controls);
        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        sg3_dt = new DataTable();
        create_tab3();
        sg3_add_blankrows();
        sg3.DataSource = sg3_dt;
        sg3.DataBind();
        setColHeadings();
        ViewState["sg3"] = sg3_dt;
        setColHeadings();
        create_tab4();
        sg4_add_blankrows();
        ViewState["sg4"] = sg4_dt;
        sg4.DataSource = sg4_dt;
        sg4.DataBind();
        sg4_dt.Dispose();
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
        Cal_CTC();
        fgen.fill_dash(this.Controls);
        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;
        if (txt_empname.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Employee Name";
        }
        if (txt_gender.Value.Trim().Length < 0 || txt_gender.Value.Trim().ToString() == "-")
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Gender (Please Check On Tab 1)";
        }
        if (frm_cocd != "SPPI" && frm_cocd != "HPPI")
        {
            if (txt_cutesi.Value.Trim().Length < 0 || txt_cutesi.Value.Trim().ToString() == "-")
            {
                reqd_nc = reqd_nc + 1;
                reqd_flds = reqd_flds + " / " + "ESI Flag (Please Check On Tab 3)";
            }

            if (txt_cutpf.Value.Trim().Length < 0 || txt_cutpf.Value.Trim().ToString() == "-")
            {
                reqd_nc = reqd_nc + 1;
                reqd_flds = reqd_flds + " / " + "PF Flag (Please Check On Tab 3)";
            }
        }
        if (txt_dob.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Date of Birth (Please Check On Tab 1)";
        }

        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        if (edmode.Value == "")
        {
            dt = new DataTable();
            mq0 = "select branchcd,grade,trim(empcode) as code,name from " + frm_tabname + " where branchcd='" + frm_mbr + "' and grade='" + frm_vty + "' and name='" + txt_empname.Value.Trim().ToUpper() + "' and fhname='" + txt_fhname.Value.Trim().ToUpper() + "'";
            dt = fgen.getdata(frm_qstr, frm_cocd, mq0);
            if (dt.Rows.Count > 0)
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,  " + " It seems it is a Duplicate Entry.'13' Same Details are entered on Emp. Code " + dt.Rows[0]["code"].ToString().Trim());
                return;
            }
            if (txt_cardno.Value != "-")
            {
                dt2 = new DataTable();
                mq2 = "select branchcd,grade,trim(empcode) as code,name,cardno from " + frm_tabname + " where cardno='" + txt_cardno.Value.Trim() + "' ";
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                if (dt2.Rows.Count > 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + " Card No. You Entered Is Already Saved on Employee ID : " + dt2.Rows[0]["code"].ToString().Trim() + "'13'(Please Check On Tab 3)");
                    return;
                }
            }
        }
        if (txt_joining.Text.Trim() == txt_dob.Text.Trim())
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,  " + " Date of Birth can not be Same as Joining Date");
            return;
        }
        if (Convert.ToDateTime(txt_joining.Text.Trim()) < Convert.ToDateTime(txt_dob.Text.Trim()))
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,  " + " Date of Joining can not be Less Than Date of Birth");
            return;
        }
        if ((Convert.ToDateTime(txt_joining.Text.Trim()) - Convert.ToDateTime(txt_dob.Text.Trim())).Days < 18)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,  " + " Please check Age as Employee age is Less Than 18 Yrs");
            return;
        }
        if (frm_cocd != "SPPI" && frm_cocd != "HPPI")
        {
            if (txt_cutesi.Value.ToUpper() != "Y" && txt_cutesi.Value.ToUpper() != "N")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,  " + " Please Put Either 'Y' or 'N' in ESI Flag (Tab 3)");
                return;
            }

            if (txt_cutpf.Value.ToUpper() != "Y" && txt_cutpf.Value.ToUpper() != "N")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,  " + " Please Put Either 'Y' or 'N' in PF Flag (Tab 3)");
                return;
            }

            if (txt_cutvpf.Value != "-")
            {
                if (txt_cutvpf.Value.ToUpper() != "Y" && txt_cutvpf.Value.ToUpper() != "N")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,  " + " Please Put Either 'Y' or 'N' in VPF Flag (Tab 3)");
                    return;
                }
            }
        }

        #region Checking Valid Dates
        int dhd = 0;
        if (txt_lastincrementdt.Value.Trim() != "-")
        {
            dhd = fgen.ChkDate(txt_lastincrementdt.Value.ToString());
            if (dhd == 0)
            {
                fgen.msg("-", "AMSG", "Please Fill a Valid Last Increment Date (Please Check On Tab 1)"); txt_lastincrementdt.Focus();
                return;
            }
        }
        if (txt_confir.Value.Trim() != "-")
        {
            dhd = fgen.ChkDate(txt_confir.Value.ToString());
            if (dhd == 0)
            {
                fgen.msg("-", "AMSG", "Please Fill a Valid Confirmation Date (Please Check On Tab 1)"); txt_confir.Focus();
                return;
            }
        }
        if (txt_leavingdate.Value.Trim() != "-")
        {
            //dhd = fgen.ChkDate(txt_leavingdate.Value.ToString());
            //if (dhd == 0)
            //{
            //    fgen.msg("-", "AMSG", "Please Fill a Valid Leaving Date (Please Check On Tab 3)"); txt_leavingdate.Focus();
            //    return;
            //}
        }
        if (txt_marriagedt.Text.Trim() != "-")
        {
            dhd = fgen.ChkDate(txt_marriagedt.Text.ToString());
            if (dhd == 0)
            {
                fgen.msg("-", "AMSG", "Please Fill a Valid Marriage Date (Please Check On Tab 4)"); txt_marriagedt.Focus();
                return;
            }
        }
        #endregion

        #region Validation of Pan Card
        if (frm_cocd != "SPPI" && frm_cocd != "HPPI")
        {
            if (txt_panno.Value.Trim() != "-")
            {
                if (txt_panno.Value.Trim().Length < 10)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,  " + " Please Put Valid Pan No. (Please Check On Tab 1)");
                    return;
                }
            }
            if ((txt_panno.Value.Trim().Length == 10))
            {
                char[] str = txt_panno.Value.Trim().ToUpper().Substring(0, 5).ToCharArray();
                for (int i = 0; i < 5; i++)
                {
                    if (str[i] >= 65 && str[i] <= 90)
                    {
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. Is Not Appropriate (Please Check On Tab 1)");
                        txt_panno.Focus();
                        return;
                    }
                }
                char[] str1 = txt_panno.Value.Trim().ToUpper().Substring(5, 4).ToCharArray();
                for (int i = 0; i < 4; i++)
                {
                    if (str1[i] >= 48 && str1[i] <= 57)
                    {
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. Is Not Appropriate (Please Check On Tab 1)");
                        txt_panno.Focus();
                        return;
                    }
                }

                char[] str2 = txt_panno.Value.Trim().ToUpper().Substring(9, 1).ToCharArray();
                for (int i = 0; i < 1; i++)
                {
                    if (str2[i] >= 65 && str2[i] <= 90)
                    {
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. Is Not Appropriate (Please Check On Tab 1)");
                        txt_panno.Focus();
                        return;
                    }
                }
            }
        }
        #endregion

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
        sg3_dt = new DataTable();
        sg4_dt = new DataTable();
        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();
        sg1_add_blankrows();
        sg1.DataSource = null;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg3_add_blankrows();
        sg4_add_blankrows();
        sg4.DataSource = null;
        sg4.DataBind();
        sg3.DataSource = null;
        sg3.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();
        ViewState["sg1"] = null;
        ViewState["sg3"] = null;
        ViewState["sg4"] = null;
        setColHeadings(); chk.Checked = false; rdbGender.SelectedValue = "M";
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        set_Val();
        hffield.Value = "List";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Grade", frm_qstr);
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||trim(a.grade)||trim(a.empcode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wb_empmas_dtl a where a.branchcd||trim(a.grade)||trim(a.empcode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                }
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), System.DateTime.Now.Date.ToString("dd/MM/yyyy"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3") + "");
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
                    if (col1 == "") return;
                    txt_Category.Value = col1;
                    txt_CatgName.Value = col2;
                    newCase(col1);
                    Fetch_Col_Earn();
                    Fetch_Col_Ded();
                    txt_empname.Focus();
                    break;

                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Delete", frm_qstr);
                    break;

                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Edit", frm_qstr);
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
                    doc_addl.Value = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "" || col1 == "-") return;
                    clearctrl();
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_GRADE", col1.Substring(2, 2));
                    string SQuery1 = "Select a.*,t.name as gradename from " + frm_tabname + " a,type t where trim(a.grade)=trim(t.type1) and t.id='I' and trim(a.branchcd)||trim(a.grade)||trim(a.empcode) ='" + col1 + "'";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        txt_Category.Value = dt.Rows[i]["grade"].ToString().Trim();
                        txt_CatgName.Value = dt.Rows[i]["gradename"].ToString().Trim();
                        txt_fhname.Value = dt.Rows[i]["fhname"].ToString().Trim();
                        txt_empcode.Value = dt.Rows[i]["EmpCode"].ToString().Trim();
                        txt_empname.Value = dt.Rows[i]["Name"].ToString().Trim();
                        txt_gender.Value = dt.Rows[i]["sex"].ToString().Trim();
                        if (txt_gender.Value == "M")
                        {
                            rdbGender.SelectedValue = "M";
                        }
                        else
                        {
                            rdbGender.SelectedValue = "F";
                        }
                        txtdepcode.Value = dt.Rows[i]["deptt"].ToString().Trim();
                        Txt_descode.Value = dt.Rows[i]["desg"].ToString().Trim();
                        txt_panno.Value = dt.Rows[i]["trade"].ToString().Trim();
                        txt_add1.Value = dt.Rows[i]["ADDR1"].ToString().Trim();
                        txt_add2.Value = dt.Rows[i]["addr2"].ToString().Trim();
                        txt_city.Value = dt.Rows[i]["city"].ToString().Trim();
                        txt_state.Value = dt.Rows[i]["state"].ToString().Trim();
                        txt_country.Value = dt.Rows[i]["country"].ToString().Trim();
                        txt_pincode.Value = dt.Rows[i]["pin"].ToString().Trim();
                        txt_offnumber.Value = dt.Rows[i]["phone"].ToString().Trim();
                        txt_tele.Value = dt.Rows[i]["phone"].ToString().Trim();
                        txt_mobile.Value = dt.Rows[i]["mobile"].ToString().Trim();
                        txt_emailid.Value = dt.Rows[i]["email"].ToString().Trim();
                        txt_add3.Value = dt.Rows[i]["paddr1"].ToString().Trim();
                        txt_add4.Value = dt.Rows[i]["paddr2"].ToString().Trim();
                        txt_city1.Value = dt.Rows[i]["pcity"].ToString().Trim();
                        txt_state1.Value = dt.Rows[i]["pstate"].ToString().Trim();
                        txt_country1.Value = dt.Rows[i]["pcountry"].ToString().Trim();
                        txt_pincode1.Value = dt.Rows[i]["ppin"].ToString().Trim();
                        txt_tele1.Value = dt.Rows[i]["pphone"].ToString().Trim();
                        txt_cutwf.Value = dt.Rows[i]["cut_wf"].ToString().Trim();
                        txt_maternal.Value = dt.Rows[i]["maternity"].ToString().Trim();
                        if (dt.Rows[i]["dtjoin"].ToString().Trim().Length > 2)
                        {
                            txt_joining.Text = Convert.ToDateTime(dt.Rows[i]["dtjoin"].ToString().Trim()).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            txt_joining.Text = "";
                        }
                        txt_leavingdate.Value = dt.Rows[i]["leaving_dt"].ToString().Trim();
                        txt_scale.Value = dt.Rows[i]["scale"].ToString().Trim();
                        txt_earned.Value = dt.Rows[i]["el"].ToString().Trim();
                        txt_casual.Value = dt.Rows[i]["cl"].ToString().Trim();
                        txt_sick.Value = dt.Rows[i]["sl"].ToString().Trim();
                        txt_lastincrementdt.Value = dt.Rows[i]["inc_dt1"].ToString().Trim();
                        txt_cutpf.Value = dt.Rows[i]["pfcut"].ToString().Trim();
                        txt_cutvpf.Value = dt.Rows[i]["cutvpf"].ToString().Trim();
                        txt_cutesi.Value = dt.Rows[i]["esicut"].ToString().Trim();
                        txt_pfno.Value = dt.Rows[i]["pfno"].ToString().Trim();
                        txt_reportsto.Value = dt.Rows[i]["pfnominee"].ToString().Trim();
                        txt_section.Value = dt.Rows[i]["fpfnominee"].ToString().Trim();
                        txt_esi.Value = dt.Rows[i]["esino"].ToString().Trim();
                        txt_esidisp.Value = dt.Rows[i]["esi_disp"].ToString().Trim();
                        txt_bankname.Value = dt.Rows[i]["Bank"].ToString().Trim();
                        txt_bankers.Value = dt.Rows[i]["bnkacno"].ToString().Trim();
                        if (dt.Rows[i]["d_o_m"].ToString().Trim().Length > 2)
                        {
                            chk.Checked = true;
                            txt_marriagedt.Text = Convert.ToDateTime(dt.Rows[i]["d_o_m"].ToString().Trim()).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            chk.Checked = false;
                            txt_marriagedt.Text = "";
                        }
                        if (dt.Rows[i]["d_o_b"].ToString().Trim().Length > 2)
                        {
                            txt_dob.Text = Convert.ToDateTime(dt.Rows[i]["d_o_b"].ToString().Trim()).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            txt_dob.Text = "";
                        }
                        txt_wrkinghor.Value = dt.Rows[i]["wrkhour"].ToString().Trim();
                        txt_departname.Value = dt.Rows[i]["deptt_text"].ToString().Trim();
                        txtrepcode.Value = dt.Rows[i]["esinominee"].ToString().Trim();
                        txt_designation.Value = dt.Rows[i]["desg_text"].ToString().Trim();
                        txt_reason.Value = dt.Rows[i]["leaving_why"].ToString().Trim();
                        txt_cardno.Value = dt.Rows[i]["cardno"].ToString().Trim();
                        txt_quali.Value = dt.Rows[i]["qualific"].ToString().Trim();
                        txt_bloodgrp.Value = dt.Rows[i]["bloodgrp"].ToString().Trim();
                        txt_confir.Value = dt.Rows[i]["conf_dt"].ToString().Trim();
                        txt_coff.Value = dt.Rows[i]["op_coff"].ToString().Trim();
                        txt_skills.Value = dt.Rows[i]["skillset"].ToString().Trim();
                        txt_joiningsal.Value = dt.Rows[i]["join_sal"].ToString().Trim();
                        txt_currctc.Value = dt.Rows[i]["curr_ctc"].ToString().Trim();
                        txt_child.Value = dt.Rows[i]["child_cnt"].ToString().Trim();
                        txt_emptype.Value = dt.Rows[i]["emp_type"].ToString().Trim();
                        txt_ifsccode.Value = dt.Rows[i]["ifsc_code"].ToString().Trim();
                        txt_adharno.Value = dt.Rows[i]["adharno"].ToString().Trim();
                        txt_probationmths.Value = dt.Rows[i]["deptt2"].ToString().Trim();
                        txt_oldcode.Value = dt.Rows[i]["old_empc"].ToString().Trim();
                        txt_uanno.Value = dt.Rows[i]["uinno"].ToString().Trim();
                        txt_erp.Value = dt.Rows[i]["erpecode"].ToString().Trim();
                        txt_m_c.Value = dt.Rows[i]["op_mach"].ToString().Trim();
                        txt_exp.Value = dt.Rows[i]["deptt1"].ToString().Trim();
                        txt_accounts.Value = dt.Rows[i]["status"].ToString().Trim();
                        txt_status.Value = dt.Rows[i]["tfr_stat"].ToString().Trim();
                        txt_accountscode.Value = dt.Rows[i]["fpfno"].ToString().Trim();
                        txt_accounterp.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where acode='" + txt_accountscode.Value + "'", "aname");
                        if (txt_accounterp.Value == "0")
                        {
                            txt_accounterp.Value = "-";
                        }
                        txt_Relation.Value = dt.Rows[i]["sp_relashn"].ToString().Trim();
                        txt_mop.Value = dt.Rows[i]["pymt_by"].ToString().Trim();
                        if (dt.Rows[i]["mnthinc"].ToString().Trim() == "1")
                        {
                            txt_PFLimit.Value = "Y";
                        }
                        else
                        {
                            txt_PFLimit.Value = "N";
                        }
                        txt_App.Value = dt.Rows[i]["appr_by"].ToString().Trim();
                        if (dt.Rows[i]["app_dt"].ToString().Trim().Length > 2)
                        {
                            txt_Appdt.Value = Convert.ToDateTime(dt.Rows[i]["app_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        }
                        lblUpload.Text = dt.Rows[i]["empimg"].ToString().Trim();
                        if (frm_cocd == "HGLO")
                        {
                            txt_Mname.Value = dt.Rows[i]["mname"].ToString().Trim();
                            txt_Emergency.Value = dt.Rows[i]["emergency"].ToString().Trim();
                            txt_List.Value = dt.Rows[i]["list"].ToString().Trim();
                            txt_Plant.Value = dt.Rows[i]["plant"].ToString().Trim();
                            txt_Branch.Value = dt.Rows[i]["branch"].ToString().Trim();
                            txt_FixedAmt.Value = dt.Rows[i]["fixed_amt"].ToString().Trim();
                            txt_Bonus.Value = dt.Rows[i]["hbonus"].ToString().Trim();
                            txt_Days.Value = dt.Rows[i]["hdays"].ToString().Trim();
                            txt_OTAfter.Value = dt.Rows[i]["otafter"].ToString().Trim();
                        }
                        if (dt.Rows[i]["branch_act"].ToString().Trim().Length > 1)
                        {
                            txt_Branch.Value = dt.Rows[i]["branch_act"].ToString().Trim();
                        }
                        create_tab4();
                        sg4_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg4_dr = sg4_dt.NewRow();
                            sg4_dr["sg4_t1"] = dt.Rows[i]["er1"].ToString().Trim();
                            sg4_dr["sg4_t2"] = dt.Rows[i]["er2"].ToString().Trim();
                            sg4_dr["sg4_t3"] = dt.Rows[i]["er3"].ToString().Trim();
                            sg4_dr["sg4_t4"] = dt.Rows[i]["er4"].ToString().Trim();
                            sg4_dr["sg4_t5"] = dt.Rows[i]["er5"].ToString().Trim();
                            sg4_dr["sg4_t6"] = dt.Rows[i]["er6"].ToString().Trim();
                            sg4_dr["sg4_t7"] = dt.Rows[i]["er7"].ToString().Trim();
                            sg4_dr["sg4_t8"] = dt.Rows[i]["er8"].ToString().Trim();
                            sg4_dr["sg4_t9"] = dt.Rows[i]["er9"].ToString().Trim();
                            sg4_dr["sg4_t10"] = dt.Rows[i]["er10"].ToString().Trim();
                            sg4_dr["sg4_t11"] = dt.Rows[i]["er11"].ToString().Trim();
                            sg4_dr["sg4_t12"] = dt.Rows[i]["er12"].ToString().Trim();
                            sg4_dr["sg4_t13"] = dt.Rows[i]["er13"].ToString().Trim();
                            sg4_dr["sg4_t14"] = dt.Rows[i]["er14"].ToString().Trim();
                            sg4_dr["sg4_t15"] = dt.Rows[i]["er15"].ToString().Trim();
                            sg4_dr["sg4_t16"] = dt.Rows[i]["er16"].ToString().Trim();
                            sg4_dr["sg4_t17"] = dt.Rows[i]["er17"].ToString().Trim();
                            sg4_dr["sg4_t18"] = dt.Rows[i]["er18"].ToString().Trim();
                            sg4_dr["sg4_t19"] = dt.Rows[i]["er19"].ToString().Trim();
                            sg4_dr["sg4_t20"] = dt.Rows[i]["er20"].ToString().Trim();
                            sg4_dr["sg4_t21"] = dt.Rows[i]["lta"].ToString().Trim();
                            sg4_dr["sg4_t22"] = dt.Rows[i]["med"].ToString().Trim();
                            sg4_dr["sg4_t23"] = dt.Rows[i]["bnp"].ToString().Trim();
                            sg4_dr["sg4_t24"] = dt.Rows[i]["vehi"].ToString().Trim();
                            sg4_dr["sg4_t25"] = dt.Rows[i]["reimgen"].ToString().Trim();
                            sg4_dr["sg4_t26"] = dt.Rows[i]["reimtel"].ToString().Trim();
                            sg4_dt.Rows.Add(sg4_dr);
                        }
                        create_tab3();

                        sg3_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            //sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;
                            sg3_dr["sg3_t1"] = dt.Rows[i]["ded1"].ToString().Trim();
                            sg3_dr["sg3_t2"] = dt.Rows[i]["ded2"].ToString().Trim();
                            sg3_dr["sg3_t3"] = dt.Rows[i]["ded3"].ToString().Trim();
                            sg3_dr["sg3_t4"] = dt.Rows[i]["ded4"].ToString().Trim();
                            sg3_dr["sg3_t5"] = dt.Rows[i]["ded5"].ToString().Trim();
                            sg3_dr["sg3_t6"] = dt.Rows[i]["ded6"].ToString().Trim();
                            sg3_dr["sg3_t7"] = dt.Rows[i]["ded7"].ToString().Trim();
                            sg3_dr["sg3_t8"] = dt.Rows[i]["ded8"].ToString().Trim();
                            sg3_dr["sg3_t9"] = dt.Rows[i]["ded9"].ToString().Trim();
                            sg3_dr["sg3_t10"] = dt.Rows[i]["ded10"].ToString().Trim();
                            sg3_dr["sg3_t11"] = dt.Rows[i]["ded11"].ToString().Trim();
                            sg3_dr["sg3_t12"] = dt.Rows[i]["ded12"].ToString().Trim();
                            sg3_dr["sg3_t13"] = dt.Rows[i]["ded13"].ToString().Trim();
                            sg3_dr["sg3_t14"] = dt.Rows[i]["ded14"].ToString().Trim();
                            sg3_dr["sg3_t15"] = dt.Rows[i]["ded15"].ToString().Trim();
                            sg3_dr["sg3_t16"] = dt.Rows[i]["ded16"].ToString().Trim();
                            sg3_dr["sg3_t17"] = dt.Rows[i]["ded17"].ToString().Trim();
                            sg3_dr["sg3_t18"] = dt.Rows[i]["ded18"].ToString().Trim();
                            sg3_dr["sg3_t19"] = dt.Rows[i]["ded19"].ToString().Trim();
                            sg3_dr["sg3_t20"] = dt.Rows[i]["ded20"].ToString().Trim();
                            sg3_dt.Rows.Add(sg3_dr);
                        }
                        ViewState["sg4"] = sg4_dt;
                        edmode.Value = "Y";
                        sg4.DataSource = sg4_dt;
                        sg4.DataBind();
                        Fetch_Col_Earn();
                        dt.Dispose();
                        sg4_dt.Dispose();
                        ViewState["sg3"] = sg3_dt;
                        sg3.DataSource = sg3_dt;
                        sg3.DataBind();
                        Fetch_Col_Ded();
                        dt.Dispose();
                        sg3_dt.Dispose();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        if (lblUpload.Text.Length > 1)
                        {
                            if (lblUpload.Text.Contains("~"))
                            {
                                txtAttch.Text = lblUpload.Text.Split('~')[1].ToString();
                            }
                            btnDwnld1.Visible = true;
                            btnView1.Visible = true;
                        }
                        if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
                        {
                            create_tab();
                            SQuery1 = "select * from wb_empmas_dtl where trim(branchcd)||trim(grade)||trim(empcode) ='" + col1 + "'";
                            dt1 = new DataTable();
                            dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                            for (i = 0; i < dt1.Rows.Count; i++)
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
                                sg1_dr["sg1_t1"] = dt1.Rows[i]["document_type"].ToString().Trim();
                                sg1_dr["sg1_t2"] = dt1.Rows[i]["doc_no"].ToString().Trim();
                                sg1_dr["sg1_t3"] = dt1.Rows[i]["issue_dt"].ToString().Trim();
                                sg1_dr["sg1_t4"] = dt1.Rows[i]["expiry_dt"].ToString().Trim();
                                sg1_dr["sg1_t5"] = dt1.Rows[i]["iss_from"].ToString().Trim();
                                sg1_dr["sg1_t6"] = dt1.Rows[i]["remarks"].ToString().Trim();
                                sg1_dr["sg1_t9"] = dt1.Rows[i]["filename"].ToString().Trim();
                                sg1_dr["sg1_t10"] = dt1.Rows[i]["filepath"].ToString().Trim();
                                sg1_dt.Rows.Add(sg1_dr);
                            }
                            if (dt1.Rows.Count <= 0)
                            {
                                sg1_add_blankrows();
                            }
                            ViewState["sg1"] = sg1_dt;
                            sg1.DataSource = sg1_dt;
                            sg1.DataBind();
                            setColHeadings();
                        }
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", doc_addl.Value.Trim());// grade
                    frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_pay_reps(frm_qstr);
                    break;

                case "List":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "List_E";
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

                case "ACNBUT":
                    if (col1.Length <= 0) return;
                    txt_departname.Value = col3.Trim();
                    txtdepcode.Value = col1.Trim();
                    txt_Emergency.Focus();
                    break;

                case "MGRBUT":
                    if (col1.Length <= 0) return;
                    txt_section.Value = col3;
                    ImageButton21.Focus();
                    break;

                case "SCHBUT":
                    if (col1.Length <= 0) return;
                    txt_designation.Value = col3.Trim();
                    Txt_descode.Value = col1.Trim();
                    ImageButton19.Focus();
                    break;

                case "DISTBUT":
                    if (col1.Length <= 0) return;
                    txtrepcode.Value = col1.Trim();
                    txt_reportsto.Value = col2.Trim();
                    txt_bloodgrp.Focus();
                    break;

                case "STATBUT":
                    if (col1.Length <= 0) return;
                    txt_state.Value = col2.Trim();
                    ImageButton13.Focus();
                    break;

                case "STATBUT1":
                    if (col1.Length <= 0) return;
                    txt_state1.Value = col2.Trim();
                    btnCountry1.Focus();
                    break;

                case "ZONEBUT":
                    if (col1.Length <= 0) return;
                    txt_scale.Value = col1.Trim();
                    txt_reason.Focus();
                    break;

                case "EMPBUT":
                    if (col1.Length <= 0) return;
                    txt_emptype.Value = col1.Trim();
                    txt_exp.Focus();
                    break;

                case "PMODEBUT":
                    if (col1.Length <= 0) return;
                    txt_mop.Value = col1.Trim();
                    //btnlbl7.Focus();
                    break;

                case "CTRYBUT":
                    if (col1.Length <= 0) return;
                    txt_country.Value = col2.Trim();
                    txt_tele.Focus();
                    break;

                case "CTRYBUT1":
                    if (col1.Length <= 0) return;
                    txt_country1.Value = col2.Trim();
                    txt_tele1.Focus();
                    break;

                case "CONTBUT":
                    if (col1.Length <= 0) return;
                    txt_esidisp.Value = col2.Trim();
                    ImageButton23.Focus();
                    break;

                case "ACODEBUT":
                    if (col1.Length <= 0) return;
                    txt_accounterp.Value = col2.Trim();
                    txt_accountscode.Value = col1.Trim();
                    txt_ifsccode.Focus();
                    break;

                case "SKILLBUT":
                    if (col1.Length <= 0) return;
                    txt_skills.Value = col2.Trim();
                    txt_child.Focus();
                    break;

                case "ACCOUNTBUT":
                    if (col1.Length <= 0) return;
                    txt_accounts.Value = col1.Substring(0, 1).Trim();
                    ImageButton_2.Focus();
                    break;

                case "BRANCH":
                    if (col1.Length <= 0) return;
                    txt_Branch.Value = col1.Trim();
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
                case "BTN_20":
                    break;
                case "BTN_21":
                    break;
                case "BTN_22":
                    break;
                case "BTN_23":
                    break;
                case "TICODE":
                    if (col1.Length <= 0) return;
                    break;

                case "MBUT":
                    txt_m_c.Value = col2;
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

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        if (col1.Length > 8) SQuery = "select * from item where trim(icode) in (" + col1 + ")";
                        else SQuery = "select * from item where trim(icode)='" + col1 + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_h3"] = "-";
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
                            sg1_dr["sg1_f4"] = dt.Rows[d]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();
                            //fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                            sg1_dr["sg1_t1"] = "";
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = "0";
                            sg1_dr["sg1_t4"] = "0";
                            sg1_dr["sg1_t5"] = "0";
                            sg1_dr["sg1_t6"] = "0";
                            sg1_dr["sg1_t7"] = "0";
                            sg1_dr["sg1_t8"] = "0";
                            sg1_dr["sg1_t9"] = "0";
                            sg1_dr["sg1_t10"] = "";
                            sg1_dr["sg1_t11"] = "";
                            sg1_dr["sg1_t12"] = "";
                            sg1_dr["sg1_t13"] = "";
                            sg1_dr["sg1_t14"] = "";
                            sg1_dr["sg1_t15"] = "";
                            sg1_dr["sg1_t16"] = "";

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
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
                    //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
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
                            //sg3_dr["sg3_srno"] = Convert.ToInt32(dt.Rows[i]["sg3_srno"].ToString());
                            sg3_dr["sg3_f1"] = dt.Rows[i]["sg3_f1"].ToString();
                            sg3_dr["sg3_f2"] = dt.Rows[i]["sg3_f2"].ToString();
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

                    #endregion
                    break;

                case "SG1_ROW_DT":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
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
                            //sg4_dr["sg4_srno"] = (i + 1);

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
                            //sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            //sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["sg1_t9"].ToString();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["sg1_t10"].ToString();
                            //sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
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
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List_E")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "select * from " + frm_tabname + " where branchcd='" + frm_mbr + "' and grade='" + frm_vty + "' and to_date(to_char(ent_dt,'dd/mm/yyyy'),'dd/mm/yyyy') " + PrdRange + " order by empcode";
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
                        if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
                        {
                            oDS5 = new DataSet();
                            oporow5 = null;
                            oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "wb_empmas_dtl");

                            save_fun5();

                            oDS5.Dispose();
                            oporow5 = null;
                            oDS5 = new DataSet();
                            oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "wb_empmas_dtl");
                        }
                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txt_empcode.Value.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            save_it = "Y";
                            i = 0;
                            do
                            {
                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(substr(empcode,3,6)) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND grade='" + txt_Category.Value.Trim() + "' ", 4, "VCH");
                                frm_vnum = txt_Category.Value + frm_vnum;
                                pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + System.DateTime.Now.ToString("dd/MM/yyyy"), frm_mbr, frm_vty, frm_vnum, System.DateTime.Now.ToString("dd/MM/yyyy"), "", frm_uname);
                            }
                            while (pk_error == "Y");
                            if (pk_error == "Y") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
                        {
                            save_fun5();
                        }

                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where trim(branchcd)||TRIM(GRADE)||trim(empcode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
                            {
                                fgen.execute_cmd(frm_qstr, frm_cocd, "update wb_empmas_dtl set branchcd='DD' where trim(branchcd)||TRIM(GRADE)||trim(empcode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            }
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
                        {
                            fgen.save_data(frm_qstr, frm_cocd, oDS5, "wb_empmas_dtl");
                        }
                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txt_empcode.Value + " Updated Successfully");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd='DD' and TRIM(GRADE)||trim(empcode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr").Substring(2, 8) + "'");
                            if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
                            {
                                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wb_empmas_dtl where branchcd='DD' and TRIM(GRADE)||trim(empcode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr").Substring(2, 8) + "'");
                            }
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txt_empcode.Value + " Saved Successfully ");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }

                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txt_empcode, frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); chk.Checked = false; rdbGender.SelectedValue = "M";
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
    }
    //------------------------------------------------------------------------------------
    public void create_tab2()
    {

    }
    //------------------------------------------------------------------------------------
    public void create_tab3()
    {
        sg3_dt = new DataTable();
        sg3_dr = null;
        // Hidden Field

        sg3_dt.Columns.Add(new DataColumn("sg3_SrNo", typeof(Int32)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t3", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t4", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t5", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t6", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t7", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t8", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t9", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t10", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t11", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t12", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t13", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t14", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t15", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t16", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t17", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t18", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t19", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t20", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void create_tab4()
    {
        sg4_dt = new DataTable();
        sg4_dr = null;
        // Hidden Field
        // sg4_dt.Columns.Add(new DataColumn("sg4_SrNo", typeof(Int32)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t1", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t2", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t3", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t4", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t5", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t6", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t7", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t8", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t9", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t10", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t11", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t12", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t13", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t14", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t15", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t16", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t17", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t18", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t19", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t20", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t21", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t22", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t23", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t24", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t25", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t26", typeof(string)));
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
            sg1_dt.Rows.Add(sg1_dr);
        }
    }
    //------------------------------------------------------------------------------------
    public void sg2_add_blankrows()
    {

    }
    //------------------------------------------------------------------------------------
    public void sg3_add_blankrows()
    {
        sg3_dr = sg3_dt.NewRow();
        sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
        // sg3_dr["sg3_f1"] = "-";
        //sg3_dr["sg3_f2"] = "-";
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
        //sg4_dr["sg4_SrNo"] = sg4_dt.Rows.Count + 1;
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
            sg1.Columns[10].HeaderStyle.Width = 30;
            sg1.Columns[11].HeaderStyle.Width = 30;
            sg1.Columns[12].HeaderStyle.Width = 50;
            //sg1.Columns[18].HeaderStyle.Width = 220;
            //sg1.Columns[19].HeaderStyle.Width = 220;
            //sg1.Columns[20].HeaderStyle.Width = 120;
            //sg1.Columns[21].HeaderStyle.Width = 120;
            //sg1.Columns[22].HeaderStyle.Width = 220;
            sg1.Columns[23].HeaderStyle.Width = 180;
            sg1.Columns[24].HeaderStyle.Width = 80;
            sg1.Columns[25].HeaderStyle.Width = 50;
            sg1.Columns[28].HeaderStyle.Width = 200;
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
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Document From The List");
                }
                break;

            case "SG1_ROW_ADD":
                if (index < sg1.Rows.Count - 1)
                {
                    //hf1.Value = index.ToString();
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    ////----------------------------
                    //hffield.Value = "SG1_ROW_ADD_E";
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    //make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                else
                {
                    #region for gridview 1
                    if (ViewState["sg1"] != null)
                    {
                        dt = new DataTable();
                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = Convert.ToInt32(sg1.Rows[i].Cells[12].Text.ToString());
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
                            //sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            //sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["sg1_t9"].ToString();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["sg1_t10"].ToString();
                            //sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                }
                break;

            case "SG1_DWN":
                filePath = sg1.Rows[index].Cells[27].Text.ToUpper();
                if (filePath.Length > 1)
                {
                    Response.ContentType = ContentType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                    Response.WriteFile(filePath);
                    Response.End();
                }
                break;

            case "SG1_VIEW":
                if (sg1.Rows[index].Cells[27].Text.Trim().Length > 1)
                {
                    filePath = sg1.Rows[index].Cells[27].Text.Substring(sg1.Rows[index].Cells[27].Text.ToUpper().IndexOf("UPLOAD"), sg1.Rows[index].Cells[27].Text.ToUpper().Length - sg1.Rows[index].Cells[27].Text.ToUpper().IndexOf("UPLOAD"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
                }
                break;

            case "SG1_UPLD":
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Fill Type Of Document First (Tab 7)");
                    return;
                }
                string UploadedFile = ((FileUpload)sg1.Rows[index].FindControl("FileUpload1")).FileName;
                string filepath = @"c:\TEJ_ERP\UPLOAD\";
                string fileName = frm_mbr.Trim() + txt_Category.Value.Trim() + txt_empcode.Value.Trim() + "~" + UploadedFile.Replace("&", "").Replace("%", "_");
                filepath = filepath + fileName;
                ((FileUpload)sg1.Rows[index].FindControl("FileUpload1")).PostedFile.SaveAs(filepath);
                ((FileUpload)sg1.Rows[index].FindControl("FileUpload1")).PostedFile.SaveAs(Server.MapPath("~/tej-base/Upload/") + fileName);
                sg1.Rows[index].Cells[26].Text = UploadedFile;
                sg1.Rows[index].Cells[27].Text = filepath;
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void sg3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg3.Rows[rowIndex].RowIndex);

        //if (txtvchnum.Value == "-")
        //{
        //    fgen.msg("-", "AMSG", "Doc No. not correct");
        //    return;
        //}
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
                sg3_dt = new DataTable();
                dt = (DataTable)ViewState["sg3"];
                z = dt.Rows.Count - 1;
                sg3_dt = dt.Clone();
                sg3_dr = null;
                i = 0;
                for (i = 0; i < sg4.Rows.Count; i++)
                {
                    sg4_dr = sg4_dt.NewRow();
                    sg4_dr["sg3_srno"] = (i + 1);
                    sg4_dr["sg3_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg3_t1")).Text.Trim();
                    sg4_dr["sg3_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg3_t2")).Text.Trim();
                    sg4_dt.Rows.Add(sg4_dr);
                }
                sg3_add_blankrows();
                ViewState["sg3"] = sg3_dt;
                sg3.DataSource = sg3_dt;
                sg3.DataBind();
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg4.Rows[rowIndex].RowIndex);

        //if (txtvchnum.Value == "-")
        //{
        //    fgen.msg("-", "AMSG", "Doc No. not correct");
        //    return;
        //}
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
    void save_fun()
    {
        //string curr_dt;
        mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_enable from fin_rsys_opt where opt_id='W0058'", "opt_enable");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        int i = 0;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = frm_mbr;
        oporow["grade"] = txt_Category.Value.Trim().Trim().ToUpper();
        oporow["EmpCode"] = frm_vnum.ToUpper().Trim();
        oporow["Name"] = txt_empname.Value.ToUpper().Trim();
        oporow["sex"] = txt_gender.Value.ToUpper().Trim();
        oporow["fhname"] = txt_fhname.Value.ToUpper().Trim();
        oporow["deptt"] = txtdepcode.Value.ToUpper().Trim();
        oporow["deptt1"] = fgen.make_double(txt_exp.Value.ToUpper().Trim());
        oporow["deptt2"] = fgen.make_double(txt_probationmths.Value.ToUpper().Trim());
        oporow["desg"] = Txt_descode.Value.ToUpper().Trim();
        oporow["section_"] = "Y";
        oporow["trade"] = txt_panno.Value.ToUpper().Trim();
        oporow["ADDR1"] = txt_add1.Value.ToUpper().Trim();
        oporow["addr2"] = txt_add2.Value.ToUpper().Trim();
        oporow["city"] = txt_city.Value.ToUpper().Trim();
        oporow["state"] = txt_state.Value.ToUpper().Trim();
        oporow["country"] = txt_country.Value.ToUpper().Trim();
        oporow["pin"] = txt_pincode.Value.ToUpper().Trim();
        oporow["phone"] = txt_offnumber.Value.ToUpper().Trim();
        oporow["mobile"] = txt_mobile.Value.ToUpper().Trim();
        oporow["email"] = txt_emailid.Value.ToUpper().Trim();
        oporow["paddr1"] = txt_add3.Value.ToUpper().Trim();
        oporow["bank"] = txt_bankname.Value.ToUpper().Trim();
        oporow["paddr2"] = txt_add4.Value.ToUpper().Trim();
        oporow["pcity"] = txt_city1.Value.ToUpper().Trim();
        oporow["pstate"] = txt_state1.Value.ToUpper().Trim();
        oporow["pcountry"] = txt_country1.Value.ToUpper().Trim();
        oporow["ppin"] = txt_pincode1.Value.ToUpper().Trim();
        oporow["pphone"] = txt_tele1.Value.ToUpper().Trim();
        oporow["scale"] = txt_scale.Value.ToUpper().Trim();
        oporow["el"] = fgen.make_double(txt_earned.Value.ToUpper().Trim());
        oporow["cl"] = fgen.make_double(txt_casual.Value.ToUpper().Trim());
        oporow["sl"] = fgen.make_double(txt_sick.Value.ToUpper().Trim());
        oporow["inc_dt1"] = fgen.make_double(txt_lastincrementdt.Value.ToUpper().Trim());
        oporow["pfcut"] = txt_cutpf.Value.ToUpper().Trim();
        oporow["cutvpf"] = txt_cutvpf.Value.ToUpper().Trim();
        oporow["esicut"] = txt_cutesi.Value.ToUpper().Trim();
        oporow["cut_wf"] = txt_cutwf.Value.ToUpper().Trim();
        oporow["pfno"] = txt_pfno.Value.ToUpper().Trim();
        oporow["pfnominee"] = txt_reportsto.Value.ToUpper().Trim();
        oporow["fpfnominee"] = txt_section.Value.ToUpper().Trim();
        oporow["esino"] = txt_esi.Value.ToUpper().Trim();
        oporow["esinominee"] = txtrepcode.Value.ToUpper().Trim();
        oporow["bnkacno"] = txt_bankers.Value.ToUpper().Trim();
        oporow["vpf_rate"] = 0;
        oporow["maternity"] = fgen.make_double(txt_maternal.Value.ToUpper().Trim());
        if (txt_marriagedt.Text.Length > 2)
        {
            oporow["married"] = "Y";
        }
        else
        {
            oporow["married"] = "-";
        }
        if ((txt_marriagedt.Text.ToUpper().Trim().Length > 2))
        {
            oporow["d_o_m"] = txt_marriagedt.Text.ToUpper().Trim();
        }
        else
        {
            oporow["d_o_m"] = DBNull.Value;
        }
        //if ((txt_leavingdate.Value.ToUpper().Trim().Length > 2))
        //{
        //    oporow["LEAVDT"] = txt_leavingdate.Value.ToUpper().Trim();
        //}
        //else
        //{
        //    oporow["LEAVDT"] = DBNull.Value;
        //}
        oporow["LEAVDT"] = DBNull.Value;
        oporow["leaving_dt"] = txt_leavingdate.Value.ToUpper().Trim();
        oporow["d_o_b"] = fgen.make_def_Date(txt_dob.Text.ToUpper().Trim(), vardate);
        oporow["dtjoin"] = fgen.make_def_Date(txt_joining.Text.ToUpper().Trim(), vardate);
        oporow["wrkhour"] = fgen.make_double(txt_wrkinghor.Value.ToUpper().Trim());
        if (fgen.make_double(txt_wrkinghor.Value.ToUpper().Trim()) == 0 || frm_cocd == "SKYP")
        {
            oporow["wrkhour"] = "8";
        }
        oporow["deptt_text"] = txt_departname.Value.ToUpper().Trim();
        oporow["inc_dt1"] = txt_lastincrementdt.Value.ToUpper().Trim();
        oporow["desg_text"] = txt_designation.Value.ToUpper().Trim();
        oporow["leaving_why"] = txt_reason.Value.ToUpper().Trim();
        oporow["esi_disp"] = txt_esidisp.Value.ToUpper().Trim();
        oporow["ent_by"] = frm_uname;
        oporow["ent_dt"] = vardate;
        oporow["cardno"] = txt_cardno.Value.ToUpper().Trim();
        if (frm_cocd == "SRIS")
        {
            oporow["cardno"] = oporow["empcode"];
        }
        oporow["qualific"] = txt_quali.Value.ToUpper().Trim();
        oporow["bloodgrp"] = txt_bloodgrp.Value.ToUpper().Trim();
        oporow["conf_dt"] = txt_confir.Value.ToUpper().Trim();
        oporow["op_coff"] = fgen.make_double(txt_coff.Value.ToUpper().Trim());
        oporow["bon_rate"] = 0;
        oporow["skillset"] = txt_skills.Value.ToUpper().Trim();
        oporow["wpw"] = "-";
        oporow["join_sal"] = fgen.make_double(txt_joiningsal.Value.ToUpper().Trim());
        oporow["inc_app_dt"] = "-";
        oporow["LEAVDT"] = DBNull.Value;
        oporow["curr_ctc"] = fgen.make_double(txt_currctc.Value.ToUpper().Trim());
        oporow["child_cnt"] = fgen.make_double(txt_child.Value.ToUpper().Trim());
        oporow["emp_type"] = txt_emptype.Value.ToUpper().Trim();
        oporow["ifsc_code"] = txt_ifsccode.Value.ToUpper().Trim();
        oporow["adharno"] = txt_adharno.Value.ToUpper().Trim();

        oporow["er1"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t1"))).Text.Trim());
        oporow["er2"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t2"))).Text.Trim());
        oporow["er3"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t3"))).Text.Trim());
        oporow["er4"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t4"))).Text.Trim());
        oporow["er5"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t5"))).Text.Trim());
        oporow["er6"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t6"))).Text.Trim());
        oporow["er7"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t7"))).Text.Trim());
        oporow["er8"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t8"))).Text.Trim());
        oporow["er9"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t9"))).Text.Trim());
        oporow["er10"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t10"))).Text.Trim());
        oporow["er11"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t11"))).Text.Trim());
        oporow["er12"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t12"))).Text.Trim());
        oporow["er13"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t13"))).Text.Trim());
        oporow["er14"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t14"))).Text.Trim());
        oporow["er15"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t15"))).Text.Trim());
        oporow["er16"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t16"))).Text.Trim());
        oporow["er17"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t17"))).Text.Trim());
        oporow["er18"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t18"))).Text.Trim());
        oporow["er19"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t19"))).Text.Trim());
        oporow["er20"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t20"))).Text.Trim());
        oporow["lta"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t21"))).Text.Trim());
        oporow["med"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t22"))).Text.Trim());
        oporow["bnp"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t23"))).Text.Trim());
        oporow["vehi"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t24"))).Text.Trim());
        oporow["reimgen"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t25"))).Text.Trim());
        oporow["reimtel"] = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t26"))).Text.Trim());

        oporow["ded1"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t1"))).Text.Trim());
        oporow["ded2"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t2"))).Text.Trim());
        oporow["ded3"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t3"))).Text.Trim());
        oporow["ded4"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t4"))).Text.Trim());
        oporow["ded5"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t5"))).Text.Trim());
        oporow["ded6"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t6"))).Text.Trim());
        oporow["ded7"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t7"))).Text.Trim());
        oporow["ded8"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t8"))).Text.Trim());
        oporow["ded9"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t9"))).Text.Trim());
        oporow["ded10"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t10"))).Text.Trim());
        oporow["ded11"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t11"))).Text.Trim());
        oporow["ded12"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t12"))).Text.Trim());
        oporow["ded13"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t13"))).Text.Trim());
        oporow["ded14"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t14"))).Text.Trim());
        oporow["ded15"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t15"))).Text.Trim());
        oporow["ded16"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t16"))).Text.Trim());
        oporow["ded17"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t17"))).Text.Trim());
        oporow["ded18"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t18"))).Text.Trim());
        oporow["ded19"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t19"))).Text.Trim());
        oporow["ded20"] = fgen.make_double(((TextBox)(sg3.Rows[i].FindControl("sg3_t20"))).Text.Trim());

        if (txtAttch.Text.Length > 1)
        {
            oporow["empimg"] = lblUpload.Text.Trim();
            //oporow["filename"] = txtAttch.Text.Trim();
        }

        oporow["old_empc"] = txt_oldcode.Value.Trim().ToUpper();
        oporow["uinno"] = txt_uanno.Value.Trim().ToUpper();
        oporow["erpecode"] = txt_erp.Value.Trim().ToUpper();
        oporow["op_mach"] = txt_m_c.Value.Trim().ToUpper();
        oporow["status"] = txt_accounts.Value.Trim().ToUpper();
        oporow["tfr_stat"] = txt_status.Value.Trim().ToUpper();
        oporow["fpfno"] = txt_accountscode.Value.Trim().ToUpper();
        oporow["sp_relashn"] = txt_Relation.Value.Trim().ToUpper();
        oporow["pymt_by"] = txt_mop.Value.Trim().ToUpper();
        if (txt_PFLimit.Value.Trim().ToUpper() == "Y")
        {
            oporow["mnthinc"] = 1;
        }
        else
        {
            oporow["mnthinc"] = 0;
        }
        if (frm_ulvl == "0")
        {
            oporow["appr_by"] = txt_App.Value.Trim().ToUpper();
            oporow["app_dt"] = txt_Appdt.Value.Trim().ToUpper();
        }
        else
        {
            oporow["appr_by"] = "-"; // IF ANY CHANGE DONE IN EMP MASTER BY ANY PERSON WHOSE ULEVEL < 0 THEN NEED TO APPROVE EMPLOYEE AGAIN.
            oporow["app_dt"] = "-";
        }
        if (frm_cocd == "HGLO")
        {
            oporow["mname"] = txt_Mname.Value.Trim().ToUpper();
            oporow["emergency"] = txt_Emergency.Value.Trim().ToUpper();
            oporow["list"] = txt_List.Value.Trim().ToUpper();
            oporow["plant"] = txt_Plant.Value.Trim().ToUpper();
            oporow["branch"] = txt_Branch.Value.Trim().ToUpper();
            oporow["fixed_amt"] = fgen.make_double(txt_FixedAmt.Value.Trim().ToUpper());
            oporow["hbonus"] = txt_Bonus.Value.Trim().ToUpper();
            oporow["hdays"] = txt_Days.Value.Trim().ToUpper();
            oporow["otafter"] = fgen.make_double(txt_OTAfter.Value.Trim().ToUpper());
        }
        if (mq0 == "Y")
        {
            oporow["branch_act"] = txt_Branch.Value.Trim().ToUpper(); // REQUIRED IN SPPI,HPPI
        }
        if (edmode.Value == "Y")
        {
            oporow["ent_by"] = ViewState["entby"].ToString();
            oporow["ent_dt"] = ViewState["entdt"].ToString();
        }
        else
        {
            oporow["ent_by"] = frm_uname;
            oporow["ent_dt"] = vardate;
        }
        oDS.Tables[0].Rows.Add(oporow);
    }
    //------------------------------------------------------------------------------------
    void save_fun5()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (i = 0; i < sg1.Rows.Count; i++)
        {
            if (((TextBox)(sg1.Rows[i].FindControl("sg1_t1"))).Text.Trim().ToUpper().Length > 1)
            {
                oporow5 = oDS5.Tables[0].NewRow();
                oporow5["branchcd"] = frm_mbr;
                oporow5["grade"] = txt_Category.Value.Trim().Trim().ToUpper();
                oporow5["empcode"] = frm_vnum.ToUpper().Trim();
                oporow5["document_type"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t1"))).Text.Trim().ToUpper();
                oporow5["doc_no"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t2"))).Text.Trim().ToUpper();
                oporow5["issue_dt"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t3"))).Text.Trim().ToUpper();
                oporow5["expiry_dt"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t4"))).Text.Trim().ToUpper();
                oporow5["iss_from"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t5"))).Text.Trim().ToUpper();
                oporow5["remarks"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t6"))).Text.Trim().ToUpper();
                oporow5["filename"] = sg1.Rows[i].Cells[26].Text.Trim();
                oporow5["filepath"] = sg1.Rows[i].Cells[27].Text.Trim();
                if (edmode.Value == "Y")
                {
                    oporow5["ent_by"] = ViewState["entby"].ToString();
                    oporow5["ent_dt"] = ViewState["entdt"].ToString();
                }
                else
                {
                    oporow5["ent_by"] = frm_uname;
                    oporow5["ent_dt"] = vardate;
                }
                oDS5.Tables[0].Rows.Add(oporow5);
            }
        }
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        SQuery = "select type1 as fstr,name as grade_name,Type1 as Grade_Code from type where id='I' and substr(type1,1,1)<'2' order by grade_code";
    }
    //------------------------------------------------------------------------------------   
    protected void sg4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg1r = 0; sg1r < sg4.Rows.Count; sg1r++)
            {
                for (int j = 0; j < sg4.Columns.Count; j++)
                {
                    sg4.Rows[sg1r].Cells[j].ToolTip = sg4.Rows[sg1r].Cells[j].Text;
                    if (sg4.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
                    {
                        sg4.Rows[sg1r].Cells[j].Text = sg4.Rows[sg1r].Cells[j].Text.Substring(0, 35);
                    }
                }
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg1r = 0; sg1r < sg3.Rows.Count; sg1r++)
            {
                for (int j = 0; j < sg3.Columns.Count; j++)
                {
                    sg4.Rows[sg1r].Cells[j].ToolTip = sg3.Rows[sg1r].Cells[j].Text;
                    if (sg3.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
                    {
                        sg3.Rows[sg1r].Cells[j].Text = sg3.Rows[sg1r].Cells[j].Text.Substring(0, 35);
                    }
                }
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnAtt_Click(object sender, EventArgs e)
    {
        string filepath = @"c:\TEJ_ERP\UPLOAD\";      //Server.MapPath("~/tej-base/UPLOAD/");
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            txtAttch.Text = Attch.FileName;
            filepath = filepath + frm_mbr.Trim() + txt_Category.Value.Trim() + txt_empcode.Value.Trim() + "~" + Attch.FileName;
            Attch.PostedFile.SaveAs(filepath);
            Attch.PostedFile.SaveAs(Server.MapPath("~/tej-base/UPLOAD/" + frm_mbr.Trim() + txt_Category.Value.Trim() + txt_empcode.Value.Trim() + "~" + Attch.FileName));
            lblUpload.Text = filepath;
            btnView1.Visible = true;
            btnDwnld1.Visible = true;
        }
        else
        {
            lblUpload.Text = "";
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnView1_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
        // ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload" + filePath.Replace("\\", "/").Replace("UPLOAD", "").Replace("upload", "") + "','90%','90%','Finsys Viewer');", true);
    }
    //------------------------------------------------------------------------------------
    protected void btnDwnld1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
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
    protected void btn_acn_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ACNBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Department", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btn_mgr_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MGRBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Section", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btn_sch_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SCHBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Designation", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btn_dist_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DISTBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Reports To", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btn_stat_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "STATBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select State", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btn_zone_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ZONEBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Scale", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btn_ctry_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CTRYBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Country", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btn_conti_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CONTBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select ESI Disp", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btn_skill_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SKILLBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Skills", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btn_m_c_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select M/C", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btn_accounts_code_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ACODEBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Accounts Code (ERP)", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btn_mode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PMODEBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Mode Of Payment", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btn_accounts_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ACCOUNTBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Account Category", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btn_emptype_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "EMPBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Employee Type", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnState1_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "STATBUT1";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select State", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnCountry1_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CTRYBUT1";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Country", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnPermanentsame_Click(object sender, ImageClickEventArgs e)
    {
        txt_add3.Value = txt_add1.Value.Trim();
        txt_add4.Value = txt_add2.Value.Trim();
        txt_city1.Value = txt_city.Value.Trim();
        txt_state1.Value = txt_state.Value.Trim();
        txt_country1.Value = txt_country.Value.Trim();
        txt_tele1.Value = txt_tele.Value.Trim();
        txt_pincode1.Value = txt_pincode.Value.Trim();
    }
    //------------------------------------------------------------------------------------
    protected void btnPresentsame_Click(object sender, ImageClickEventArgs e)
    {
        txt_add1.Value = txt_add3.Value.Trim();
        txt_add2.Value = txt_add4.Value.Trim();
        txt_city.Value = txt_city1.Value.Trim();
        txt_state.Value = txt_state1.Value.Trim();
        txt_country.Value = txt_country1.Value.Trim();
        txt_tele.Value = txt_tele1.Value.Trim();
        txt_pincode.Value = txt_pincode1.Value.Trim();
    }
    //------------------------------------------------------------------------------------
    public void Fetch_Col_Earn()
    {
        dt2 = new DataTable();
        if (edmode.Value == "")
        {
            grade = col1;
        }
        else
        {
            grade = fgenMV.Fn_Get_Mvar(frm_qstr, "U_GRADE");
        }
        SQuery = "select distinct ed_fld as col,ed_name as columns,morder from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + grade + "' and nvl(icat,'-')!='Y' and ed_fld like 'ER%' order by morder";
        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg4_dr = sg4_dt.NewRow();
        for (int d = 0; d < dt2.Rows.Count; d++)
        {
            sg4.HeaderRow.Cells[d].Text = dt2.Rows[d]["columns"].ToString().Trim().Replace(" ", "_");
        }
        if (sg4.Rows.Count > 0)
        {
            sg4.HeaderRow.Cells[20].Text = "LTA";
            sg4.HeaderRow.Cells[21].Text = "MEDICAL";
            sg4.HeaderRow.Cells[22].Text = "BNP";
            sg4.HeaderRow.Cells[23].Text = "VEHI.";
            sg4.HeaderRow.Cells[24].Text = "REIMB_GEN";
            sg4.HeaderRow.Cells[25].Text = "REIMB_TEL";
        }
    }
    //------------------------------------------------------------------------------------
    public void Fetch_Col_Ded()
    {
        dt1 = new DataTable();
        if (edmode.Value == "")
        {
            grade = col1;
        }
        else
        {
            grade = fgenMV.Fn_Get_Mvar(frm_qstr, "U_GRADE");
        }
        SQuery = "select distinct ed_fld as col,ed_name as columns,morder from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + grade + "' and nvl(icat,'-')!='Y' and ed_fld like 'DED%' order by morder";
        dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg3_dr = sg3_dt.NewRow();
        for (int d = 0; d < dt1.Rows.Count; d++)
        {
            sg3.HeaderRow.Cells[d].Text = dt1.Rows[d]["columns"].ToString().Trim().Replace(" ", "_");
        }
    }
    //------------------------------------------------------------------------------------
    protected void chk_CheckedChanged(object sender, EventArgs e)
    {
        if (chk.Checked == true)
        {
            txt_marriagedt.ReadOnly = false;
        }
        else
        {
            txt_marriagedt.ReadOnly = true;
            txt_marriagedt.Text = "";
        }
    }
    //------------------------------------------------------------------------------------
    protected void Cal_CTC()
    {
        double er1 = 0, er2 = 0, er3 = 0, er4 = 0, er5 = 0, er6 = 0, er7 = 0, er8 = 0, er9 = 0, er10 = 0, er11 = 0, er12 = 0, er13 = 0, er14 = 0, er15 = 0, er16 = 0, er17 = 0, er18 = 0, er19 = 0, er20 = 0, totern = 0, ctc = 0;
        for (int i = 0; i < sg4.Rows.Count; i++)
        {
            er1 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t1"))).Text.Trim());
            er2 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t2"))).Text.Trim());
            er3 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t3"))).Text.Trim());
            er4 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t4"))).Text.Trim());
            er5 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t5"))).Text.Trim());
            er6 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t6"))).Text.Trim());
            er7 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t7"))).Text.Trim());
            er8 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t8"))).Text.Trim());
            er9 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t9"))).Text.Trim());
            er10 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t10"))).Text.Trim());
            er11 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t11"))).Text.Trim());
            er12 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t12"))).Text.Trim());
            er13 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t13"))).Text.Trim());
            er14 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t14"))).Text.Trim());
            er15 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t15"))).Text.Trim());
            er16 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t16"))).Text.Trim());
            er17 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t17"))).Text.Trim());
            er18 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t18"))).Text.Trim());
            er19 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t19"))).Text.Trim());
            er20 = fgen.make_double(((TextBox)(sg4.Rows[i].FindControl("sg4_t20"))).Text.Trim());
            totern = er1 + er2 + er3 + er4 + er5 + er6 + er7 + er8 + er9 + er10 + er11 + er12 + er13 + er14 + er15 + er16 + er17 + er18 + er19 + er20;
        }
        #region
        DataTable dtPF_ER = new DataTable();
        dtPF_ER = fgen.getdata(frm_qstr, frm_cocd, "select rtrim(xmlagg(xmlelement(e,replace(TRIM(ED_FLD) ,'-',null)||'+')).extract('//text()').extract('//text()'),'+') as earnings from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txt_Category.Value.Trim() + "' and pf_yn='Y' and nvl(icat,'-')!='Y'");
        string PF_ER = "", ESI_ER = "", WF_ER = "";
        DataTable dtSelmas = new DataTable();
        dtSelmas = fgen.getdata(frm_qstr, frm_cocd, "SELECT RATE,PF_DIV,ED_FLD,ED_NAME,ESI_DIV,WF_DIV,EMPR_RATE,MAX_LMT FROM WB_SELMAST WHERE branchcd='" + frm_mbr + "' and grade='" + txt_Category.Value.Trim() + "' and ed_fld like 'DED%' and nvl(icat,'-')!='Y' order by morder");
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
        double PFTOT = 0, ESITOT = 0, WFTOT = 0;
        string[] PF_Earnings = PF_ER.Split('+');
        for (int i = 0; i < PF_Earnings.Length; i++)
        {
            switch (PF_Earnings[i].ToString())
            {
                case "ER1":
                    PFTOT += er1;
                    break;
                case "ER2":
                    PFTOT += er2;
                    break;
                case "ER3":
                    PFTOT += er3;
                    break;
                case "ER4":
                    PFTOT += er4;
                    break;
                case "ER5":
                    PFTOT += er5;
                    break;
                case "ER6":
                    PFTOT += er6;
                    break;
                case "ER7":
                    PFTOT += er7;
                    break;
                case "ER8":
                    PFTOT += er8;
                    break;
                case "ER9":
                    PFTOT += er9;
                    break;
                case "ER10":
                    PFTOT += er10;
                    break;
                case "ER11":
                    PFTOT += er11;
                    break;
                case "ER12":
                    PFTOT = er12;
                    break;
                case "ER13":
                    PFTOT = er13;
                    break;
                case "ER14":
                    PFTOT += er14;
                    break;
                case "ER15":
                    PFTOT += er15;
                    break;
                case "ER16":
                    PFTOT += er16;
                    break;
                case "ER17":
                    PFTOT += er17;
                    break;
                case "ER18":
                    PFTOT += er18;
                    break;
                case "ER19":
                    PFTOT += er19;
                    break;
                case "ER20":
                    PFTOT += er20;
                    break;
            }
        }

        double PF_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "RATE")) / 100;
        double PF_Empr_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "EMPR_RATE")) / 100;
        double PF_Limit = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "MAX_LMT"));
        string PF_Formula = "round(" + PFTOT * PF_Rate + ",2) as DED1";

        DataTable dtESI_ER = new DataTable();
        dtESI_ER = fgen.getdata(frm_qstr, frm_cocd, "select rtrim(xmlagg(xmlelement(e,replace(TRIM(ED_FLD) ,'-',null)||'+')).extract('//text()').extract('//text()'),'+') as earnings from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txt_Category.Value.Trim() + "' and esi_yn='Y' and nvl(icat,'-')!='Y'");

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

        string[] ESI_Earnings = PF_ER.Split('+');
        for (int i = 0; i < ESI_Earnings.Length; i++)
        {
            switch (ESI_Earnings[i].ToString())
            {
                case "ER1":
                    ESITOT += er1;
                    break;
                case "ER2":
                    ESITOT += er2;
                    break;
                case "ER3":
                    ESITOT += er3;
                    break;
                case "ER4":
                    ESITOT += er4;
                    break;
                case "ER5":
                    ESITOT += er5;
                    break;
                case "ER6":
                    ESITOT += er6;
                    break;
                case "ER7":
                    ESITOT += er7;
                    break;
                case "ER8":
                    ESITOT += er8;
                    break;
                case "ER9":
                    ESITOT += er9;
                    break;
                case "ER10":
                    ESITOT += er10;
                    break;
                case "ER11":
                    ESITOT += er11;
                    break;
                case "ER12":
                    ESITOT = er12;
                    break;
                case "ER13":
                    ESITOT = er13;
                    break;
                case "ER14":
                    ESITOT += er14;
                    break;
                case "ER15":
                    ESITOT += er15;
                    break;
                case "ER16":
                    ESITOT += er16;
                    break;
                case "ER17":
                    ESITOT += er17;
                    break;
                case "ER18":
                    ESITOT += er18;
                    break;
                case "ER19":
                    ESITOT += er19;
                    break;
                case "ER20":
                    ESITOT += er20;
                    break;
            }
        }

        double ESI_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED3'", "RATE")) / 100;
        double ESI_Empr_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED3'", "EMPR_RATE")) / 100;
        string ESI_Formula = "round(" + ESITOT * ESI_Rate + ",2) as DED3";
        DataTable dtWF_ER = new DataTable();
        dtWF_ER = fgen.getdata(frm_qstr, frm_cocd, "select rtrim(xmlagg(xmlelement(e,replace(TRIM(ED_FLD) ,'-',null)||'+')).extract('//text()').extract('//text()'),'+') as earnings from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txt_Category.Value.Trim() + "' and wf_yn='Y' and nvl(icat,'-')!='Y'");
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
        double WFWage = 0;
        string[] WF_Earnings = PF_ER.Split('+');
        for (int i = 0; i < WF_Earnings.Length; i++)
        {
            switch (WF_Earnings[i].ToString())
            {
                case "ER1":
                    WFTOT += er1;
                    break;
                case "ER2":
                    WFTOT += er2;
                    break;
                case "ER3":
                    WFTOT += er3;
                    break;
                case "ER4":
                    WFTOT += er4;
                    break;
                case "ER5":
                    WFTOT += er5;
                    break;
                case "ER6":
                    WFTOT += er6;
                    break;
                case "ER7":
                    WFTOT += er7;
                    break;
                case "ER8":
                    WFTOT += er8;
                    break;
                case "ER9":
                    WFTOT += er9;
                    break;
                case "ER10":
                    WFTOT += er10;
                    break;
                case "ER11":
                    WFTOT += er11;
                    break;
                case "ER12":
                    WFTOT = er12;
                    break;
                case "ER13":
                    WFTOT = er13;
                    break;
                case "ER14":
                    WFTOT += er14;
                    break;
                case "ER15":
                    WFTOT += er15;
                    break;
                case "ER16":
                    WFTOT += er16;
                    break;
                case "ER17":
                    WFTOT += er17;
                    break;
                case "ER18":
                    WFTOT += er18;
                    break;
                case "ER19":
                    WFTOT += er19;
                    break;
                case "ER20":
                    WFTOT += er20;
                    break;
            }
        }

        double WF_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED6'", "RATE")) / 100;
        double WF_Empr_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED6'", "EMPR_RATE"));
        double WF_Limit = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED6'", "MAX_LMT"));
        string WF_Formula = "round(" + WFTOT * WF_Rate + ",2) as DED6";
        double PFWage = 0, DED1 = 0, DED3 = 0, DED6 = 0;

        #endregion
        if (txt_cutpf.Value.ToUpper() == "Y")
        {
            if (PFTOT > PF_Limit)
            {
                PFWage = PF_Limit;
            }
            else
            {
                PFWage = PFTOT;
            }
            DED1 = Math.Round(PFWage * PF_Empr_Rate, 2);
        }

        if (txt_cutesi.Value.ToUpper() == "Y")
        {
            DED3 = Math.Round(ESITOT * ESI_Empr_Rate, 2);
        }

        if (WF_Rate > 0 && WF_Empr_Rate > 0)
        {
            WFWage = WFTOT * WF_Rate;
            if (WFWage > WF_Limit)
            {
                DED6 = WF_Limit * WF_Empr_Rate;
            }
            else
            {
                DED6 = Math.Round(WFTOT * WF_Empr_Rate, 2);
            }
        }
        ctc = totern + DED1 + DED3 + DED6;
        txt_currctc.Value = ctc.ToString();
    }
    //------------------------------------------------------------------------------------
    protected void btnCTC_Click(object sender, EventArgs e)
    {
        if (sg4.Rows.Count > 0)
        {
            if (((TextBox)sg4.Rows[0].FindControl("sg4_t1")).Text == "" || ((TextBox)sg4.Rows[0].FindControl("sg4_t1")).Text == "-")
            {
                fgen.msg("-", "AMSG", "Please Fill Atleast One Earning");
                return;
            }
            Cal_CTC();
        }
    }
    //------------------------------------------------------------------------------------
    protected void imgBranch_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BRANCH";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Branch", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnUpload_Click(object sender, EventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    //THESE FIELDS ARE OPENED FOR HGLO
    //ALTER TABLE EMPMAS ADD MNAME VARCHAR2(50) default '-';
    //ALTER TABLE EMPMAS ADD EMERGENCY VARCHAR2(10) default '-';
    //ALTER TABLE EMPMAS ADD LIST VARCHAR2(20) default '-';
    //ALTER TABLE EMPMAS ADD PLANT VARCHAR2(50) default '-'; 
    //ALTER TABLE EMPMAS ADD BRANCH VARCHAR2(50) default '-';
    //ALTER TABLE EMPMAS ADD MATERNITY VARCHAR2(10) DEFAULT '-';
    //ALTER TABLE EMPMAS ADD FIXED_AMT NUMBER(15,5) DEFAULT 0;
    //ALTER TABLE EMPMAS ADD HBONUS VARCHAR2(10) DEFAULT '-';
    //ALTER TABLE EMPMAS ADD HDAYS VARCHAR2(10) DEFAULT '-';
    //ALTER TABLE EMPMAS ADD OTAFTER NUMBER(10,2) DEFAULT 0;

    //NOT REQUIRED
    //SQuery = "CREATE TABLE WB_EMPMAS (GRADE CHAR(2) default '-',EMPCODE CHAR(10) default '-',NAME VARCHAR2(50) default '-',SEX CHAR(1) default '-',FHNAME VARCHAR2(50) default '-',DEPTT VARCHAR2(50) default '-',DEPTT2 NUMBER(5) default 0,DEPTT1 NUMBER(5) default 0,DESG VARCHAR2(3) default '-',DESCGRD CHAR(15) default '-',SECTION_ CHAR(15) default '-',TRADE CHAR(15) default '-',INDST CHAR(3) default '-',DTJOIN DATE default sysdate,LEAVDT DATE default sysdate,SCALE CHAR(30) default '-',EL NUMBER(4,1) default 0,CL NUMBER(4,1) default 0,SL NUMBER(4,1) default 0,INCREMNT NUMBER(7,2) default 0,MNTHINC NUMBER(2) default 0,PF VARCHAR2(10) default '-',PFCUT CHAR(1) default '-',CUTVPF CHAR(1) default '-',FPF NUMBER(10,2),ADVANCE NUMBER(10,2),ADVBAL NUMBER(10,2),ESI VARCHAR2(10) default '-',ESICUT CHAR(1) default '-',INSURABL CHAR(1) default '-',ITAX NUMBER(10,2) default 0,PFLOAN NUMBER(10,2) default 0,PFLOANBL NUMBER(10,2) default 0,PFNO CHAR(20) default '-',PFNOMINEE VARCHAR2(50) default '-',FPFNO CHAR(12) default '-',FPFNOMINEE VARCHAR2(50) default '-',ESINO CHAR(20) default '-',ESINOMINEE VARCHAR2(50) default '-',BANK VARCHAR2(50) default '-',BNKACNO VARCHAR2(20) default '-',ER1 NUMBER(8,2) default 0,ER2 NUMBER(8,2) default 0,ER3 NUMBER(8,2) default 0,ER4 NUMBER(8,2) default 0,ER5 NUMBER(8,2) default 0,ER6 NUMBER(8,2) default 0,ER7 NUMBER(8,2) default 0,ER8 NUMBER(8,2) default 0,ER9 NUMBER(8,2) default 0,ER10 NUMBER(8,2) default 0,ER11 NUMBER(8,2) default 0,ER12 NUMBER(8,2) default 0,ER13 NUMBER(8,2) default 0,ER14 NUMBER(8,2) default 0,ER15 NUMBER(8,2) default 0,ER16 NUMBER(8,2) default 0,ER17 NUMBER(8,2) default 0,ER18 NUMBER(8,2) default 0,ER19 NUMBER(8,2) default 0,ER20 NUMBER(8,2) default 0,DED1 NUMBER(8,2) default 0,DED2 NUMBER(8,2) default 0,DED3 NUMBER(8,2) default 0,DED4 NUMBER(8,2) default 0,DED5 NUMBER(8,2) default 0,DED6 NUMBER(8,2) default 0,DED7 NUMBER(8,2) default 0,DED8 NUMBER(8,2) default 0,DED9 NUMBER(8,2) default 0,DED10 NUMBER(8,2) default 0,DED11 NUMBER(8,2) default 0,DED12 NUMBER(8,2) default 0,DED13 NUMBER(8,2) default 0,DED14 NUMBER(8,2) default 0,DED15 NUMBER(8,2) default 0,DED16 NUMBER(8,2) default 0,DED17 NUMBER(8,2) default 0,DED18 NUMBER(8,2) default 0,DED19 NUMBER(8,2) default 0,DED20 NUMBER(8,2) default 0,LMCOINS NUMBER(5,2) default 0,OTHER NUMBER(8,2) default 0,SAV1 NUMBER(8,2) default 0,SAV2 NUMBER(8,2) default 0,SAV3 NUMBER(8,2) default 0,SAV4 NUMBER(8,2) default 0,SAV5 NUMBER(8,2) default 0,SAVINGS NUMBER(10,2) default 0,LTA NUMBER(8,2) default 0,BONUS NUMBER(8,2) default 0,COINS NUMBER(5,2) default 0,GENERATED NUMBER(1) default 0,STATUS CHAR(15) default '-',MCAT VARCHAR2(10) default '-',ADDR1 VARCHAR2(100) default '-', ADDR2 VARCHAR2(100) default '-',CITY VARCHAR2(50) default '-',STATE VARCHAR2(50) default '-',COUNTRY VARCHAR2(50) default '-',PIN VARCHAR2(20) default '-',PHONE VARCHAR2(50) default '-',MOBILE VARCHAR2(50) default '-',EMAIL VARCHAR2(50) default '-',PADDR1 VARCHAR2(50) default '-',PADDR2 VARCHAR2(50) default '-',PCITY VARCHAR2(50) default '-',PSTATE VARCHAR2(50) default '-',PCOUNTRY VARCHAR2(50) default '-',PPIN VARCHAR2(20) default '-',PPHONE VARCHAR2(50) default '-',BRANCHCD CHAR(2) default '-',VPF_RATE NUMBER(5,2) default 0,MARRIED CHAR(1) default '-',D_O_M DATE default sysdate,D_O_B DATE,WRKHOUR NUMBER(5,2),DEPTT_TEXT VARCHAR2(50) default '-',DESG_TEXT VARCHAR2(50) default '-',LEAVING_DT VARCHAR2(11) default '-',VCHNUM CHAR(6) default '-',VCHDATE CHAR(11) default '-',LEAVING_WHY VARCHAR2(15) default '-',ESI_DISP VARCHAR2(20) default '-',ENT_BY VARCHAR2(20),ENT_DT DATE default sysdate,TFR_STAT VARCHAR2(30) default '-',CARDNO VARCHAR2(10),MED NUMBER(10,2) default 0,BNP NUMBER(10,2),VEHI NUMBER(10,2) default 0,REIMGEN NUMBER(10,2) default 0,REIMTEL NUMBER(10,2) default 0,INC_DT1 VARCHAR2(10) default '-',OTH2 NUMBER(10,2) default 0,OTH3 NUMBER(10,2) default 0,QUALIFIC VARCHAR2(50) default '-',ERPECODE VARCHAR2(10) default '-',BLOODGRP VARCHAR2(3) default '-', SHIFT_TYPE VARCHAR2(1) default '-',SHIFT_CODE VARCHAR2(2) default '-',EMPIMG VARCHAR2(100) default '-',CONF_DT VARCHAR2(11) default '-',APP_BY VARCHAR2(20) default '-',APP_DT VARCHAR2(10) default '-',APPR_BY VARCHAR2(20) default '-',PYMT_BY VARCHAR2(10) default '-',OP_COFF NUMBER(10,2),BON_RATE NUMBER(10,2),OLD_EMPC VARCHAR2(10) default '-',CUTWF VARCHAR2(1) DEFAULT '-',CUT_WF VARCHAR2(1) default '-',SKILLSET  VARCHAR2(20) default '-',WPW VARCHAR2(15),OP_MACH VARCHAR2(30) default '-',INC_APP_DT VARCHAR2(10) default '-',JOIN_SAL NUMBER(10,2) default 0,CURR_CTC NUMBER(10,2) default 0,CHILD_CNT NUMBER(10,2) default 0,EMP_TYPE VARCHAR2(15) default '-',IFSC_CODE VARCHAR2(20) default '-',EDT_DTL VARCHAR2(35) default '-',UINNO VARCHAR2(25) default '-',INCR_M VARCHAR2(1) default '-', MLEAVE NUMBER(3) default 0, NEW_PFRULE VARCHAR2(1) default '-', DEDGRAT VARCHAR2(20) default '-',DEDCANT VARCHAR2(1) default '-',QTR_QPI NUMBER(12,2) default 0,CURR_BR VARCHAR2(2) default '-',ADHARNO VARCHAR2(25) default '-',SP_RELASHN VARCHAR2(20) default '-',ATN_ALLOW VARCHAR2(1) default '-',BNKCID VARCHAR2(20) default '-',MNAME VARCHAR2(50) default '-',EMERGENCY VARCHAR2(10) default '-',LIST VARCHAR2(20) default '-',PLANT VARCHAR2(50) default '-',BRANCH VARCHAR2(50) default '-',MATERNITY VARCHAR2(10) DEFAULT '-',FIXED_AMT NUMBER(15,5) DEFAULT 0)";
    //fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

    //THIS FIELD IS OPENED FOR ALL CLIENTS
    //ALTER TABLE EMMPMAS ADD BRANCH_ACT VARCHAR2(2);

    //THIS TABLE IS CREATED FOR SPPI,HPPI
    //SQuery = "create table wb_empmas_dtl (branchcd char(2) default '-',grade char(2) default '-',empcode varchar2(6) default '-',document_type varchar2(30) default '-',doc_no varchar2(30) default '-',issue_dt varchar2(10) default '-',expiry_dt varchar2(10) default '-',iss_from varchar2(50) default '-',remarks varchar2(100) default '-',filename varchar2(100) default '-',filepath varchar2(250) default '-',ent_by varchar2(20) default '-',ent_dt date default sysdate)";
    //fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
}