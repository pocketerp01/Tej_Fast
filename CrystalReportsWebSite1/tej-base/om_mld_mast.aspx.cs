using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_mld_mast : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, col7, vardate, fromdt, todt, next_year, typePopup = "N", mq0;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string html_body = "";
    string Prg_Id, lbl1a_Text, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
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
            if (lblUpload.Text.Length > 1)
            {
                btnView1.Visible = true;
                btnDwnld1.Visible = true;
            }
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
        btnlist.Disabled = false;
        btnprint.Disabled = false;
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
        frm_tabname = "WB_MASTER";
        lblheader.Text = "Mould Detailed Specification";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MM01");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        if (frm_ulvl == "3") cond = " and trim(a.ENT_BY)='" + frm_uname + "'";
        if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR.Trim() + "'";
        switch (btnval)
        {
            case "BTN_23":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1,rate  FROM TYPE WHERE ID='A' order by name ";
                break;

            case "New":
                Type_Sel_query();
                break;
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "acode":
                SQuery = "Select acode as fstr,aname as party_name ,acode as acode from famst where branchcd!='DD' and substr(acode,1,2) in ('16','02') ORDER BY ACODE,ANAME";
                break;

            case "Mould":
                SQuery = "select trim(nvl(a.type1,'-'))||'~'||trim(nvl(a.acref,'-'))||'~'||trim(a.acref2)||'~'||trim(nvl(a.lineno,0))||'~'||trim(nvl(a.provision,0))||'~'||trim(nvl(a.pageno,0))||'~'||trim(nvl(a.p_acode,'-'))||'~'||trim(nvl(a.p_icode,'-')) as fstr, trim(a.name) as Mould_name, trim(a.Acref) as Mld_code,trim(a.type1) as Mld_Srn,a.ent_by,to_char(a.Vchdate,'dd/mm/yyyy') as Ent_Dtd,a.p_Acode,a.p_icode from typegrp a where a.branchcd='" + frm_mbr + "' and a.id='MM' and trim(type1) not in (select trim(Col1) from wb_master where branchcd='" + frm_mbr + "' and id='MM01')  order by a.Name";
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    SQuery = "SELECT distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, trim(a.vchnum) as Entry_no,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') as entry_dt,a.ACODE AS code,b.aname as party,a.col1 as mould_code,c.name as mould_name,a.CPARTNO as mould_id,a.col6 as mould_size,to_char(a.vchdate,'yyyymmdd') as vdd FROM WB_MASTER a,famst b,typegrp c WHERE trim(A.acode)=trim(B.acode) and trim(a.col1)=trim(c.type1) and trim(a.branchcd)=trim(c.branchcd) and a.BRANCHCD='" + frm_mbr + "' AND a.id='" + frm_vty + "' AND c.id='MM' /*AND a.VCHDATE " + DateRange + "*/ ORDER BY vdd desc,TRIM(a.vchnum) DESC";
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
        //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "CSS : Query has been logged " + frm_vnum, html_body);
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
    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        frm_vty = vty;
        lbl1a.Text = vty;
        string mq0 = "";
        //mq0 = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + "";
        //for mould master
        mq0 = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND id='" + frm_vty + "' /*and vchdate " + DateRange + "*/";
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, mq0, 6, "VCH");

        txtvchnum.Value = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);


        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        disablectrl(); btnmldcode.Enabled = true; btnmldcode.Focus();
        fgen.EnableForm(this.Controls);
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
            fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return;
        }

        if (txtacode.Value.Length == 0 || txtacode.Value == "-")
        {
            fgen.msg("-", "AMSG", "Please Select Party/Customer Code And Name!!"); txtacode.Focus(); return;
        }
        if (txtcmsn_dt.Value.Length == 0 || txtcmsn_dt.Value == "-")
        {
            fgen.msg("-", "AMSG", "Please enter Commission Date"); txtcmsn_dt.Focus(); return;
        }

        //if (fgen.make_double(txt_tool_lyf.Value) == 0)
        //{
        //    fgen.msg("-", "AMSG", " Tool Life Cannot be 0 '13' Please enter tool_life in main mould master"); return;
        //}
        //if (fgen.make_double(txtn_cavit.Value) == 0)
        //{
        //    fgen.msg("-", "AMSG", "No Of Cavities Cannot be 0 '13' Please enter no_of_cavities in main mould master"); return;
        //}
        //if (fgen.make_double(txt_pm_count.Value) == 0)
        //{
        //    fgen.msg("-", "AMSG", "PM_Freq_Shot Cannot be 0 '13' Please enter PM_Freq_Shot in main mould master "); return;
        //}
        //if (fgen.make_double(txt_freq_hm.Value) == 0)
        //{
        //    fgen.msg("-", "AMSG", "HM_Freq_Shot Cannot be 0 '13' Please enter HM_Freq_Shot in main mould master"); return;
        //}

        if (fgen.make_double(txt_shot.Value) > fgen.make_double(txt_tool_lyf.Value))
        {
            fgen.msg("-", "AMSG", "Tool Life Already Exhausted,'13' Please Correct Tool Life in Main Mould Master Form");
            txt_shot.Focus();
            return;
        }

        if (txt_last_pm_dt.Value.Length == 0 || txt_last_pm_dt.Value == "-")
        {
            fgen.msg("-", "AMSG", "Please enter Last PM Date.It is required to calculate next plan date.If tool is new, then pls put Commission Date"); 
            txtcmsn_dt.Focus();
            return;
        }
        if (txt_last_hm_dt.Value.Length == 0 || txt_last_hm_dt.Value == "-")
        {
            fgen.msg("-", "AMSG", "Please enter Last HM Date.It is required to calculate next plan date.If tool is new, then pls put Commission Date"); 
            txtcmsn_dt.Focus(); 
            return;
        }
        if (txt_shots_acq.Value.Length == 0 || txt_shots_acq.Value == "-")
        {
            fgen.msg("-", "AMSG", "Please enter Shots at the time of acquisition(purchased) else put 0!!"); 
            txt_shots_acq.Focus(); 
            return;
        }
        if (txt_shot.Value.Length == 0 || txt_shot.Value == "-")
        {
            fgen.msg("-", "AMSG", "Please enter Total Shots on mould maintenance start date!!"); txt_shot.Focus(); return;
        }
        if (txt_tot_shot.Value.Length == 0 || txt_tot_shot.Value == "-")
        {
            fgen.msg("-", "AMSG", "Please enter Total Shots utilized till mould maintenance start date!!!!"); 
            txt_tot_shot.Focus(); 
            return;
        }
        if (txt_pm_freq_mth.Value.Length == 0 || txt_pm_freq_mth.Value == "-")
        {
            fgen.msg("-", "AMSG", "Please enter Frequency for PM in months!!");
            txt_pm_freq_mth.Focus();
            return;
        }
        if (txt_hm_freq_mth.Value.Length == 0 || txt_hm_freq_mth.Value == "-")
        {
            fgen.msg("-", "AMSG", "Please enter Frequency for HM in months!!");
            txt_hm_freq_mth.Focus(); 
            return;
        }
        if (fgen.make_double(txt_op_pm_count.Value) > fgen.make_double(txt_pm_freq_shots.Value))
        {
            fgen.msg("-", "AMSG", "Opening PM Count Should be Less than PM Freq Shots");
            txt_pm_freq_mth.Focus();
            return;
        }
        if (fgen.make_double(txt_op_hm_count.Value) > fgen.make_double(txt_hm_freq_shots.Value))
        {
            fgen.msg("-", "AMSG", "Opening HM Count Should be Less than HM Freq Shots");
            txt_pm_freq_mth.Focus();
            return;
        }

        //if (txtDispose.Value.Trim().ToUpper() == "Y")
        //{
        //    if (txtDisposeDt.Value.Length <= 1)
        //    {
        //        fgen.msg("-", "AMSG", "Please Fill Dispose Date"); 
        //        txtDisposeDt.Focus();
        //        return;
        //    }
        //    if (txtDisposeBy.Value.Length <= 1)
        //    {
        //        fgen.msg("-", "AMSG", "Please Fill Dispose By"); 
        //        txtDisposeBy.Focus();
        //        return;
        //    }
        //}
        //else
        //{
        //    if (txtDisposeDt.Value.Length > 1)
        //    {
        //        fgen.msg("-", "AMSG", "Please enter Y for Mould dispose as you have entered Dispose Date"); txtDispose.Focus();
        //        return;
        //    }
        //    if (txtDisposeBy.Value.Length > 1)
        //    {
        //        fgen.msg("-", "AMSG", "Please enter Y for Mould dispose as you have entered Dispose by"); txtDispose.Focus();
        //        return;
        //    }
        //}
        //if (fgen.make_double(txtfreq_max.Value) > fgen.make_double(txt_pm_count.Value))
        //{
        //    fgen.msg("-", "AMSG", "PM alert Shots Mail Should be Less than PM Freq Shots"); txt_pm_freq_mth.Focus();
        //    return;
        //}

        //if (fgen.make_double(txt_hlth_count.Value) > fgen.make_double(txt_freq_hm.Value))
        //{
        //    fgen.msg("-", "AMSG", "HM alert Shots Mail Should be Less than  HM Freq Shots"); txt_pm_freq_mth.Focus();
        //    return;
        //}

        //------------------------------------------------

        txt_shot.Value = (fgen.make_double(txt_shots_acq.Value) + fgen.make_double(txt_tot_shot.Value)).ToString();

        if (txt_tot_shot.Value.Length >= 1)
        {
            string a, b;
            a = txt_shot.Value;
            b = txt_tool_lyf.Value;
            txtblnc.Value = (fgen.make_double(b) - fgen.make_double(a)).ToString().Trim();
        }
        mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + frm_mbr + "'", "OPT_START");// check date for mm start date
        if (mq0.Length <= 1)
        {
            fgen.msg("-", "AMSG", "Mould Maintenenace Start Date Required For Each Branch, Please Contact your IT deptt.");
            return;
        }
        else
        {
            if (Convert.ToDateTime(Convert.ToDateTime(txtcmsn_dt.Value).ToString("dd/MM/yyyy")) > Convert.ToDateTime(mq0))
            {

            }
            else
            {
                if (Convert.ToDateTime(Convert.ToDateTime(txt_last_pm_dt.Value).ToString("dd/MM/yyyy")) > Convert.ToDateTime(mq0))
                {
                    fgen.msg("-", "AMSG", "Mould last PM date must be less than the Mould Maintenance Start Date else put maintenenace records through system.");
                    return;
                }
                if (Convert.ToDateTime(Convert.ToDateTime(txt_last_hm_dt.Value).ToString("dd/MM/yyyy")) > Convert.ToDateTime(mq0))
                {
                    //fgen.msg("-", "AMSG", "Mould last Maintenenace date must be less than the Mould Maintenance Start Date else put maintenenace records through system.");
                    fgen.msg("-", "AMSG", "Mould last HM date must be less than the Mould Maintenance Start Date else put maintenenace records through system.");
                    return;
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
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        SQuery = "SELECT TRIM(A.vchnum) AS ENTRY_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRY_DATE ,A.COL1 AS MOULD_CODE,c.name as mould_name,A.COL5 AS MOULD_ID,a.acode as code,b.aname as customer_name,A.icode AS PRODUCT_CODE,I.INAME AS PRODUCT,i.cpartno AS PART_NO,A.COL4 AS PART_NAME, a.name as model_name ,A.COL6 AS MOULD_SIZE,to_char(a.date1,'dd/mm/yyyy') as commisioning_date,A.COL14 AS MATERIAL ,A.NUM1 AS TONNAGE,A.NUM2 AS CAVITY,A.NUM3 AS CYCLE_TIME,A.NUM4 AS TOOL_LIFE,A.NUM6 AS PM_Freq_Shots,A.COL15 AS FREQUENCY_DECIDED_PER_SHOT,A.NUM7 AS Shot_till_Acquire,A.NUM8 AS Total_Shot_Till_Date ,A.NUM10 AS CO_Total_Shots,A.NUM11 AS BALANCE_SHOT,a.col9 as Mould_name_id,A.NUM5 AS HM_Max_Shots_Mail,A.NUM15 AS HM_Freq_Mths,col8 as last_hm_date,A.NUM12 AS PM_Freq_Mths,A.NUM14 AS PM_Max_Shots_Mail,col7 as last_pm_date FROM WB_MASTER A LEFT JOIN ITEM I ON TRIM(A.icode)=TRIM(I.ICODE), FAMST B,typegrp c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.col1)=trim(c.type1) and trim(a.branchcd)=trim(c.branchcd) AND A.BRANCHCD='" + frm_mbr + "' AND A.id='" + frm_vty + "' and c.id='MM' AND a.VCHDATE " + PrdRange + " ORDER BY a.vchnum DESC";
        SQuery = "SELECT TRIM(A.vchnum) AS ENTRY_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRY_DATE ,A.COL1 AS MOULD_CODE,C.NAME AS MOULD_NAME,A.CPARTNO AS MOULD_ID,A.ACODE AS CUST_CODE,B.ANAME AS CUSTOMER_NAME,A.ICODE AS PRODUCT_CODE,I.INAME AS PRODUCT,A.COL5 AS PARTNO,A.COL4 AS PART_NAME, A.NAME AS MODEL_NAME ,A.COL6 AS MOULD_SIZE,TO_CHAR(A.DATE1,'DD/MM/YYYY') AS COMMISIONING_DATE,A.COL14 AS MATERIAL ,A.NUM2 AS CAVITY,A.NUM4 AS TOOL_LIFE,A.NUM3 AS CYCLE_TIME,A.NUM1 AS CLAMPING_TONNAGE,A.NUM7 AS SHOTS_AT_ACQUISITION,A.NUM10 AS CO_TOTAL_SHOTS,A.NUM11 AS BALANCE_SHOT,A.NUM8 AS TOTAL_SHOT_TILL_DATE,A.COL9 AS MOULD_NAME_ID,a.col12 as OPENING_HM_COUNT,A.COL15 AS HM_FREQ_SHOTS,A.NUM13 AS FIRST_HM_COUNT,A.NUM15 AS HM_FREQ_MTHS,A.NUM5 AS HM_MAX_SHOTS_MAIL,A.COL8 AS LAST_HM_DATE,a.col11 as OPENING_PM_COUNT,A.NUM6 AS PM_FREQ_SHOTS,A.NUM12 AS PM_FREQ_MTHS,A.NUM14 AS PM_MAX_SHOTS_MAIL,A.COL7 AS LAST_PM_DATE,A.REMARKS,A.COL2 AS DISPOSE,A.COL10 AS DISPOSE_DATE,A.COL13 AS DISPOSE_BY, trim(A.Ent_by) as ent_by,TO_CHAR(A.ent_dt,'DD/MM/YYYY') AS Ent_Date,to_char(a.vchdate,'yyyymmdd') as vdd FROM WB_MASTER A LEFT JOIN ITEM I ON TRIM(A.icode)=TRIM(I.ICODE), FAMST B,typegrp c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.col1)=trim(c.type1) and trim(a.branchcd)=trim(c.branchcd) AND A.BRANCHCD='" + frm_mbr + "' AND A.id='" + frm_vty + "' and c.id='MM' ORDER BY vdd desc,ENTRY_NO DESC";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim(), frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.id||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                // FOR SAVING HISTORY AND IN FRM_VTY THERE IS 4 CHAR TYPE SO THAT WE DO SUBTRING OF IT AND SAVE THE INFO IN TWO CHAR TYPE OF FININFO TABLE
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty.Substring(2, 2), lblheader.Text.Trim() + " Type =" + frm_vty + " Deleted");
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
                    newCase(col1);
                    break;

                case "acode":
                    if (col1 == "") return;
                    txtacode.Value = col1;
                    txtcustname.Value = col2;
                    break;
                case "Mould":
                    if (col1 == "") return;
                    if (col1.Length < 1 || col1 == "" || col1 == "0")
                    {
                        return;
                    }
                    else
                    {
                        if (col1.Contains("~"))
                        {
                            txtacode.Value = col1.Split('~')[6].ToString();
                            if (txtacode.Value == "-")
                            {
                                fgen.msg("-", "AMSG", "Please Update The Customer in Mould Master");
                                return;
                            }
                            txtmldname.Value = col2;
                            txtmldcode.Value = col1.Split('~')[0].ToString();
                            txtmld_id.Value = col1.Split('~')[1].ToString();
                            txtn_cavit.Value = col1.Split('~')[2].ToString();

                            txt_tool_lyf.Value = col1.Split('~')[3].ToString();
                            txt_pm_freq_shots.Value = col1.Split('~')[4].ToString();
                            txt_hm_freq_shots.Value = col1.Split('~')[5].ToString();
                            txtcustname.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select aname  from famst where trim(AcodE)='" + txtacode.Value + "'  ", "aname");
                            txtprcode.Value = col1.Split('~')[7].ToString();
                            txtprname.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select iname  from item where trim(icodE)='" + txtprcode.Value + "'  ", "iname");
                        }
                        else
                        {
                            txtmldcode.Value = col1;
                        }

                    }

                    break;
                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a_Text = "CS";
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;
                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a_Text = "CS";
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

                    SQuery = "select a.*,b.aname as aname,c.name as Mould_name,i.iname from wb_master a left join item i on trim(a.icode)=trim(i.icode),famst b,typegrp c where trim(a.acode)=trim(b.acode) and trim(a.col1)=trim(c.type1) and trim(a.branchcd)=trim(c.branchcd) and a.branchcd='" + frm_mbr + "' and a.id='" + frm_vty + "' and c.id='MM' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') ='" + col1 + "' ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txtvchnum.Value = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtacode.Value = dt.Rows[0]["acode"].ToString().Trim();
                        txtcustname.Value = dt.Rows[0]["aname"].ToString().Trim();
                        txtmodel.Value = dt.Rows[0]["name"].ToString().Trim();
                        txtmldcode.Value = dt.Rows[0]["col1"].ToString().Trim();
                        txtmldname.Value = dt.Rows[0]["Mould_name"].ToString().Trim();
                        txtpart_no.Value = dt.Rows[0]["COL5"].ToString().Trim();
                        txtpart_name.Value = dt.Rows[0]["col4"].ToString().Trim();
                        txtmld_id.Value = dt.Rows[0]["cpartno"].ToString().Trim();
                        txtcmsn_dt.Value = Convert.ToDateTime(dt.Rows[0]["date1"].ToString().Trim()).ToString("yyyy-MM-dd");
                        txtmld_size.Value = dt.Rows[0]["col6"].ToString().Trim();
                        txt_last_pm_dt.Value = Convert.ToDateTime(dt.Rows[0]["col7"].ToString().Trim()).ToString("yyyy-MM-dd");
                        txt_last_hm_dt.Value = Convert.ToDateTime(dt.Rows[0]["col8"].ToString().Trim()).ToString("yyyy-MM-dd");
                        txt_op_pm_count.Value = dt.Rows[0]["col11"].ToString().Trim();//Op_PM_Count
                        txt_op_hm_count.Value = dt.Rows[0]["col12"].ToString().Trim();//Op_HM_Count

                        txt_tonnage.Value = dt.Rows[0]["num1"].ToString().Trim();
                        txtn_cavit.Value = dt.Rows[0]["num2"].ToString().Trim();
                        txt_cycle_tm.Value = dt.Rows[0]["num3"].ToString().Trim();
                        txt_tool_lyf.Value = dt.Rows[0]["num4"].ToString().Trim();
                        txt_hm_alert.Value = dt.Rows[0]["num5"].ToString().Trim();
                        txt_pm_freq_shots.Value = dt.Rows[0]["num6"].ToString().Trim();
                        txt_shots_acq.Value = dt.Rows[0]["num7"].ToString().Trim();
                        txt_shot.Value = dt.Rows[0]["num8"].ToString().Trim();
                        txt_hm_chk_freq.Value = dt.Rows[0]["num9"].ToString().Trim();
                        txt_tot_shot.Value = dt.Rows[0]["num10"].ToString().Trim();
                        txtblnc.Value = dt.Rows[0]["num11"].ToString().Trim();
                        txt_pm_freq_mth.Value = dt.Rows[0]["num12"].ToString().Trim();
                        txtfirst_hm_count.Value = dt.Rows[0]["num13"].ToString().Trim();
                        txt_pm_alert.Value = dt.Rows[0]["num14"].ToString().Trim();
                        txt_hm_freq_mth.Value = dt.Rows[0]["num15"].ToString().Trim();
                        txtprcode.Value = dt.Rows[0]["icode"].ToString().Trim();
                        txtprname.Value = dt.Rows[0]["iname"].ToString().Trim();
                        txt_hm_freq_shots.Value = dt.Rows[0]["col15"].ToString().Trim();
                        txtmat.Value = dt.Rows[0]["col14"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["remarks"].ToString().Trim();
                        txtAttch.Text = dt.Rows[0]["imagef"].ToString().Trim();
                        txtAttchPath.Text = dt.Rows[0]["imagepath"].ToString().Trim();
                        txt_mould_name_id.Value = dt.Rows[0]["col9"].ToString().Trim();

                        txtDispose.Value = dt.Rows[0]["col2"].ToString().Trim();
                        if (dt.Rows[0]["col10"].ToString().Trim().Length > 1)
                        {
                            txtDisposeDt.Value = Convert.ToDateTime(dt.Rows[0]["col10"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            txtDisposeDt.Value = "-";
                        }
                        txtDisposeBy.Value = dt.Rows[0]["col13"].ToString().Trim();

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                        txtvchnum.Disabled = true;
                        txtvchdate.Enabled = false;
                        btnmldcode.Enabled = false;
                        if (txtAttchPath.Text.Length > 1)
                        {
                            btnDwnld1.Visible = true;
                            btnView1.Visible = true;
                        }
                    }
                    #endregion
                    break;
                case "Print_E":

                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    break;
                case "TICODE":
                    if (col1.Length <= 0) return;
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
            // QUERY WRITTEN ON BTNLIST_CLICK
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
            string last_entdt;
            //checks
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and id='" + lbl1a_Text + "'  ", "ldt");
            if (last_entdt == "0" || edmode.Value == "Y")
            {
            }
            else
            {
                if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                {
                    Checked_ok = "N";
                    Checked_ok = "Y";
                    //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + Convert.ToDateTime(txtvchdate.Value.Trim()).ToString("dd/MM/yyyy") + ",Please Check !!");
                }
            }
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                //Checked_ok = "N";
                //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + Convert.ToDateTime(txtvchdate.Value.Trim()).ToString("dd/MM/yyyy") + " ,Please Check !!");
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
                        save_fun();

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
                                i = 0;
                                do
                                {
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and id='" + frm_vty + "' ", 6, "vch");
                                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("yyyy-MM-dd"), "", frm_uname);
                                    if (i > 20)
                                    {
                                        fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and id='" + frm_vty + "' ", 6, "vch");
                                        pk_error = "N";
                                        i = 0;
                                    }
                                    i++;
                                }
                                while (pk_error == "Y");
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();

                        if (edmode.Value == "Y")
                        {
                            //ddl_fld1 = fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                            //string type_depr = "40";
                            //ddl_fld2 = fgenMV.Fn_Get_Mvar(frm_qstr,"" ).Substring(0, 2) + type_depr + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr").Substring(3, 17);
                            string mycmd = "";
                            mycmd = "update " + frm_tabname + " set branchcd='DD' where branchcd||id||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/MM/yyyy')='" + frm_mbr + frm_vty + txtvchnum.Value.Trim() + txtvchdate.Text.Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mycmd);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            string mycmd2 = "";
                            mycmd2 = "delete from " + frm_tabname + " where branchcd='DD' and id||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/MM/yyyy')='" + frm_vty + txtvchnum.Value.Trim() + txtvchdate.Text.Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mycmd2);
                        }

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
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
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
    protected void btnlbl11_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl12_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl19_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl20_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl21_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl22_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl23_Click(object sender, ImageClickEventArgs e)
    {

    }

    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();

        oporow["BRANCHCD"] = frm_mbr;
        oporow["id"] = frm_vty;
        oporow["vchnum"] = txtvchnum.Value; // ENTRY NO
        oporow["VCHDATE"] = fgen.make_def_Date(txtvchdate.Text.Trim(), vardate); // ENTRY DATE
        oporow["acode"] = txtacode.Value.ToUpper().Trim(); // PARTY CODE
        oporow["icode"] = txtprcode.Value.ToUpper().Trim(); // ITEM CODE
        oporow["cpartno"] = txtmld_id.Value.ToUpper().Trim(); // ACREF OF TYPEGRP
        oporow["name"] = txtmodel.Value.ToUpper().Trim(); // MODEL NAME
        oporow["col1"] = txtmldcode.Value.ToUpper().Trim(); // TYPE1 OF TYPEGRP
        if (txtDispose.Value.ToUpper().Trim() != "Y")
        {
            oporow["col2"] = "N";
        }
        else
        {
            oporow["col2"] = txtDispose.Value.ToUpper().Trim(); // DISPOSE Y/N
        }
        // oporow["col3"] = txtpart_no.Value.ToUpper().Trim(); // PARTNO
        oporow["col4"] = txtpart_name.Value.ToUpper().Trim(); // PART NAME
        oporow["col5"] = txtpart_no.Value.ToUpper().Trim(); // PART NO
        oporow["col6"] = txtmld_size.Value.ToUpper().Trim(); // SIZE
        oporow["col7"] = Convert.ToDateTime(txt_last_pm_dt.Value.ToUpper().Trim()).ToString("dd/MM/yyyy"); // LAST PM DATE
        oporow["col8"] = Convert.ToDateTime(txt_last_hm_dt.Value.ToUpper().Trim()).ToString("dd/MM/yyyy"); // LAST HM DATE
        oporow["col9"] = txt_mould_name_id.Value.ToUpper().Trim(); // MOULD NAME ID
        if (txtDisposeDt.Value.Trim().Length > 1)
        {
            oporow["col10"] = Convert.ToDateTime(txtDisposeDt.Value.ToUpper().Trim()).ToString("dd/MM/yyyy"); // DISPOSE DT
        }
        else
        {
            oporow["col10"] = "-";
        }
        oporow["col11"] = fgen.make_double(txt_op_pm_count.Value.ToUpper().Trim()); // OP_PM_COUNT
        oporow["col12"] = fgen.make_double(txt_op_hm_count.Value.ToUpper().Trim()); // OP_HM_COUNT
        oporow["col13"] = txtDisposeBy.Value.ToUpper().Trim(); // DISPOSE BY
        oporow["col14"] = txtmat.Value.ToUpper().Trim().ToUpper(); // MATERIAL
        oporow["col15"] = fgen.make_double(txt_hm_freq_shots.Value.ToUpper().Trim()); // HM_FREQ_SHOTS
        oporow["date1"] = Convert.ToDateTime(txtcmsn_dt.Value.ToUpper().Trim()).ToString("dd/MM/yyyy"); // COMMISSION DT
        oporow["num1"] = fgen.make_double(txt_tonnage.Value.ToUpper().Trim()); // TONNAGE
        oporow["num2"] = fgen.make_double(txtn_cavit.Value.ToUpper().Trim()); // CAVITY
        oporow["num3"] = fgen.make_double(txt_cycle_tm.Value.ToUpper().Trim()); // CYCLE TIME
        oporow["num4"] = fgen.make_double(txt_tool_lyf.Value.ToUpper().Trim()); // TOOL LIFE
        oporow["num5"] = fgen.make_double(txt_hm_alert.Value.ToUpper().Trim()); // HM ALERT SHOTS FOR MAIL
        oporow["num6"] = fgen.make_double(txt_pm_freq_shots.Value.ToUpper().Trim()); // PM FREQ SHOTS
        oporow["num7"] = fgen.make_double(txt_shots_acq.Value.ToUpper().Trim()); // SHOTS ACQUISITION
        oporow["num8"] = fgen.make_double(txt_shot.Value.ToUpper().Trim()); // TOT SHOT TILL DT
        oporow["num9"] = fgen.make_double(txt_hm_chk_freq.Value.ToUpper().Trim()); // HM CHECK FREQ
        oporow["num10"] = fgen.make_double(txt_tot_shot.Value.ToUpper().Trim()); // CO TOTAL SHOTS
        oporow["num11"] = fgen.make_double(txtblnc.Value.ToUpper().Trim()); // BALANCE SHOTS
        oporow["num12"] = fgen.make_double(txt_pm_freq_mth.Value.ToUpper().Trim()); //PM_FREQ_MTHS
        oporow["num13"] = fgen.make_double(txtfirst_hm_count.Value.ToUpper().Trim()); // FIRST HM COUNT
        oporow["num14"] = fgen.make_double(txt_pm_alert.Value.ToUpper().Trim()); // PM ALERT SHOTS FOR MAIL
        oporow["num15"] = fgen.make_double(txt_hm_freq_mth.Value.ToUpper().Trim()); // HM_FREQ_MTHS
        if (txtrmk.Text.Trim().Length > 300)
        {
            oporow["remarks"] = txtrmk.Text.Trim().ToUpper().Substring(0, 299);
        }
        else
        {
            oporow["remarks"] = txtrmk.Text.Trim().ToUpper();
        }

        if (txtAttch.Text.Length > 1)
        {
            oporow["IMAGEF"] = txtAttch.Text.Trim();
            oporow["imagepath"] = txtAttchPath.Text.Trim();
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
            oporow["eDt_dt"] = vardate;
        }
        oDS.Tables[0].Rows.Add(oporow);
    }

    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
    }

    //------------------------------------------------------------------------------------

    protected void btnAtt_Click(object sender, EventArgs e)
    {
        string filepath = @"c:\TEJ_ERP\UPLOAD\";   //Server.MapPath("~/tej-base/UPLOAD/");
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            txtAttch.Text = Attch.FileName;
            string fileName = txtmld_id.Value.Trim().Replace("/", "_").Replace(@"\", "_") + "_" + txtvchnum.Value.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
            filepath = filepath + fileName;
            txtAttchPath.Text = filepath;
            txtAttch.Text = Attch.FileName;
            Attch.PostedFile.SaveAs(filepath);
            Attch.PostedFile.SaveAs(Server.MapPath("~/tej-base/Upload/") + fileName);
            lblUpload.Text = filepath;

            btnView1.Visible = true;
            btnDwnld1.Visible = true;
        }
        else
        {
            lblUpload.Text = "";
        }
    }

    protected void btnView1_Click(object sender, ImageClickEventArgs e)
    {
        lblUpload.Text = txtAttchPath.Text;

        string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
    }
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

    protected void btnacode_click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "acode";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Party", frm_qstr);
    }

    protected void btnmldcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Mould";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Mould", frm_qstr);
    }

    protected void btnprod_click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Prod";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Product", frm_qstr);
    }
}