using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Drawing;

public partial class om_maint_plan : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, SQuery2, SQuery3, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, mq11, mq12, mq13, mq14, mq15, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", header_n;
    DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dt7, dt8, dt9, dt10, dt11; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok; string rate;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1; DateTime date1, date2;
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
                doc_addl.Value = "-";
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
            typePopup = "N";
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
                ((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("readonly", "readonly");
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
        tab5.Visible = false;
        tab4.Visible = false;
        tab3.Visible = false;
        tab2.Visible = false;
        //tab1.Visible = false;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
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
        //btnlbl7.Enabled = false;
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
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlbl4.Enabled = true;
        //btnlbl7.Enabled = true;
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
        frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        Prg_Id = frm_formID;
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        frm_tabname = "WB_MAINT";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);

        switch (frm_formID)
        {
            case "F75150": // FOR MOULD MAINT PLAN PM
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MM02");
                lbl2.Text = "On selection of month(1 entry pm),ERP will automatically plan (PM) for moulds which should undergo PM as per their last PM done date added to PM months (frequency).By clicking add btn in the grid, manual selection of other moulds for plan is also possible. ";
                break;

            case "F75155":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MM03");
                lbl2.Text = "On selection of month(1 entry pm),ERP will automatically plan (HM) for moulds which should undergo HM as per their last HM done date added to HM months (frequency).By clicking add btn in the grid, manual selection of other moulds for plan is also possible. ";
                break;

            case "F75153":  //FOR OK TO PRODN
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MM07");
                lbl2.Text = "Form to Record OK For Production entry ";
                txtlbl2.ReadOnly = true;
                txtlbl3.ReadOnly = true;
                break;
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
                if (frm_formID == "F75150" || frm_formID == "F75155")  //FOR MOULD MAINT PLAN AND MOULD BREAKDOWN AND OK TO PRODN
                {
                    SQuery = "SELECT MTHNUM AS FSTR,MTHNUM AS MONTH_NUM,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                }
                else
                {
                    SQuery = "select trim(a.entry)||trim(a.mould_code ) as fstr,a.mould_code as code,b.name as mould_name,a.breakdown_date,max(a.btchno) As mc_code,max(a.title) As mc_name from (select distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as entry , col1 as mould_code,to_char(date1,'dd/mm/yyyy') as breakdown_date,1 as qty,btchno,title from wb_maint where branchcd='" + frm_mbr + "' and type='MM06' and vchdate " + DateRange + " union all select distinct col11 as entry ,col1 as mould_code,to_char(date1,'dd/mm/yyyy')  as breakdown_date,-1 as qty,btchno,title from wb_maint where branchcd='" + frm_mbr + "' and type='MM07' and vchdate " + DateRange + ") a,typegrp b where trim(a.mould_code)=trim(b.type1) and b.branchcd='" + frm_mbr + "' and b.id='MM' group by trim(a.entry),a.mould_code,b.name,a.breakdown_date having sum(qty)>0 order by b.name";
                }
                break;

            case "TICODE":
                //pop2btnhideg
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[13].Text.Trim().Length > 1)
                    {
                        if (col1.Length > 0) col1 = col1 + "," + "'" + gr.Cells[13].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                    }
                }

                if (col1.Length > 0)
                {
                    if (frm_formID == "F75150" || frm_formID == "F75155")  //FOR MOULD PLAN
                    {
                        col1 = " and trim(B.type1) not in (" + col1 + ")";
                    }
                    else if (frm_formID == "F75153")
                    {
                        col1 = " and trim(icode) not in (" + col1 + ")";
                    }
                }
                else
                {
                    col1 = "";
                }
                if (frm_formID == "F75150" || frm_formID == "F75155")  //FOR MOULD PLAN
                {
                    SQuery = "select TRIM(a.col1) AS FSTR,b.name AS MOULD,b.acref AS mould_code,a.col1 AS CODE from wb_master a, typegrp b where trim(a.col1)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and a.branchcd='" + frm_mbr + "' and a.id='MM01' and b.id='MM' " + col1 + " AND NVL(A.COL2,'-')!='Y'";
                }
                else  // for Ok TO PRODUCTION
                {
                    SQuery = "select trim(icode) as fstr,icode as item_code,iname as item_name,(case when nvl(iqd,0)=0 then irate else iqd end) as rate,unit from item where substr(trim(icode),1,2)>='30' and substr(trim(icode),1,2)<='60' and length(trim(icode))>4 " + col1 + "  order by Iname";
                }
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                break;

            case "SG1_ADD_MAC":
                //               SQuery = "select '01' as fstr,'01' as code,'Moulding' as machine_name from dual";
                break;

            case "SG1_ADD_SPR":
                SQuery = "SELECT '01' AS FSTR, '01' AS CODE,'DRILL' AS SPARE FROM DUAL";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "Print_E":
                if (frm_formID == "F75150" || frm_formID == "F75155")  //FOR MOULD MAINT PLAN AND MOULD BREAKDOWN AND OK TO PRODN
                {
                    SQuery = "SELECT MTHNUM AS FSTR,MTHNUM AS MONTH_NUM,MTHNAME,MTHNUM FROM MTHS ORDER BY MTHSNO";
                }
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    if (frm_formID == "F75150" || frm_formID == "F75155") // FOR MOULD MAINT PLAN
                    {
                        SQuery = "select distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,trim(a.vchnum) as Entry_no,trim(obsv2) as plan_month,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Dt,a.Ent_by ,a.Ent_dt, to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,trim(a.vchnum) desc";
                    }
                    else // for Ok TO PRODUCTION
                    {
                        SQuery = "Select distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr , trim(a.vchnum) as Entry_no, to_char(a.vchdate,'dd/mm/yyyy') as Entry_Dt,b.name as mould_name,a.Ent_by,a.Ent_Dt,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,typegrp b where trim(a.col1)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) AND B.ID='MM' and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,trim(a.vchnum) desc";
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
            if (typePopup == "N") newCase(frm_vty);
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
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }


        if (frm_formID == "F75153")
        {
            string last_entdt = "";
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtlbl5.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + Convert.ToDateTime(txtlbl5.Text).ToString("dd/MM/yyyy") + " ,Please Check !!");
                txtlbl5.Focus();
                return;
            }

            if (Convert.ToDateTime(txtlbl5.Text.Trim()) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtlbl5.Text.Trim()) > Convert.ToDateTime(todt))
            {
                fgen.msg("-", "AMSG", "Date outside " + fromdt + " to " + todt + " is Not Allowed!!'13'Fill date for This Year Only");
                txtlbl7.Focus();
                return;
            }
        }

        if (frm_formID == "F75150" || frm_formID == "F75155")
        {
            if (txtlbl4.Text.Length <= 1)
            {
                fgen.msg("-", "AMSG", "Please Select Month");
                txtlbl4.Focus(); return;
            }
            if (sg1.Rows.Count <= 1)
            {
                fgen.msg("-", "AMSG", "No Mould to Save!!'13'Please Select Some Moulds First"); return;
            }

            frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
            if (Convert.ToInt32(txtlbl4.Text) > 3 && Convert.ToInt32(txtlbl4.Text) <= 12)
            {

            }
            else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
            for (int i = 0; i < sg1.Rows.Count - 1; i++)
            {
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Length > 10)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Kindly Correct Planning Date At Line No. " + sg1.Rows[i].Cells[12].Text + " '13' Year is not correct.");
                    return;
                }
                mq0 = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text).ToString("dd/MM/yyyy");
                if (mq0.Substring(3, 7) != txtlbl4.Text + "/" + frm_myear)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Kindly Correct The Planning Date At Line No. " + sg1.Rows[i].Cells[12].Text + " '13' The Plan Date Should Fall In The Selected Month.");
                    return;
                }
                string start_dt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + frm_mbr + "'", "OPT_START");// check date for mm start date
                if (Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text) < Convert.ToDateTime(start_dt))
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , The Planning Date At Line No. " + sg1.Rows[i].Cells[12].Text + " '13' cannot be less than Maitenance Module Start Date, else correct PM date in master specifications.");
                    return;
                }
                mq1 = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text).ToString("dd/MM/yyyy");
                if (Convert.ToDateTime(mq1) > Convert.ToDateTime(mq0))
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Kindly remove the Mould At Line No. " + sg1.Rows[i].Cells[12].Text + " '13' The mould is already maintained at a later date.");
                    return;
                }

            }
        }
        else if (frm_formID == "F75153")
        {
            if (txtlbl4.Text.Length <= 1)
            {
                fgen.msg("-", "AMSG", "Please Select Mould");
                txtlbl4.Focus(); return;
            }
            if (txtlbl5.Text.Length <= 1)
            {
                fgen.msg("-", "AMSG", "Please Fill OK Date");
                txtlbl5.Focus(); return;
            }
            if (txtlbl6.Text.Length <= 1)
            {
                fgen.msg("-", "AMSG", "Please Fill OK Time");
                txtlbl6.Focus(); return;
            }

            if (Convert.ToDateTime(txtlbl5.Text) < Convert.ToDateTime(txtlbl2.Text))
            {
                fgen.msg("-", "AMSG", "OK Date Cannot Be Less Than Date of Occurance");
                txtlbl5.Focus(); return;
            }
            if (Convert.ToDateTime(txtlbl5.Text) == Convert.ToDateTime(txtlbl2.Text))
            {
                if (Convert.ToDateTime(txtlbl6.Text) <= Convert.ToDateTime(txtlbl3.Text))
                {
                    fgen.msg("-", "AMSG", "OK Time Cannot Be Less Than Or Equals To Time of Occurance");
                    txtlbl6.Focus(); return;
                }
            }
            if (txtlbl8.Text.Length <= 1)
            {
                fgen.msg("-", "AMSG", "Please Fill DownTime Reason");
                txtlbl8.Focus(); return;
            }
            for (int i = 0; i < sg1.Rows.Count - 1; i++)
            {
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text == "" || ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text == "-")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill Qty At Line No. " + sg1.Rows[i].Cells[12].Text + "");
                    return;
                }
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
        fgen.Fn_open_prddmp1("Select DateRange", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Month", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region

        if (frm_formID == "F75150")
        {
            vty = "MM02";
            frm_vty = vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
            lbl1a.Text = vty;
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'", 6, "VCH");
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        }
        else if (frm_formID == "F75155")
        {
            vty = "MM03";
            frm_vty = vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
            lbl1a.Text = vty;
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'", 6, "VCH");
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        }
        else if (frm_formID == "F75153")
        {
            vty = "MM07";
            frm_vty = vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
            lbl1a.Text = vty;
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'", 6, "VCH");
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        }
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
        #endregion
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty.Substring(2, 2), lblheader.Text.Trim() + " Type =" + frm_vty + " Deleted");
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
            else
            {
                btnlbl4.Focus();
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
                    z = 0;
                    date1 = Convert.ToDateTime(col3).AddMonths(+1);
                    mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select mthname from mths where mthnum='" + date1.ToString().Substring(3, 2) + "'", "mthname");
                    SQuery = "select distinct vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate from wb_maint where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and OBSV2='" + date1.ToString().Substring(3, 7) + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        fgen.msg("", "AMSG", "Planning of Next Month (" + mq0 + ") is Already Done.'13' Deletion Is Not Allowed");
                        fgen.ResetForm(this.Controls);
                        fgen.DisableForm(this.Controls);
                        clearctrl();
                        enablectrl();
                        return;
                    }
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
                    clearctrl(); mq0 = "";

                    #region  for OK TO PRODUCTION
                    if (frm_formID == "F75153")
                    {
                        SQuery = "SELECT a.*,b.name as mould_name,i.iname,i.unit FROM " + frm_tabname + " a left join item i on trim(a.icode)=trim(i.icode),typegrp b WHERE trim(a.col1)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE ='" + frm_vty + "' and b.id='MM' AND TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + col1 + "' order by a.srno";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                        ViewState["fstr"] = col1;
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                            txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                            txtlbl4a.Text = dt.Rows[0]["mould_name"].ToString().Trim();
                            txtlbl4.Text = dt.Rows[0]["col1"].ToString().Trim();
                            txtlbl7.Text = dt.Rows[0]["btchno"].ToString().Trim();
                            txtlbl7a.Text = dt.Rows[0]["title"].ToString().Trim();
                            txtlbl2.Text = Convert.ToDateTime(dt.Rows[0]["date1"].ToString().Trim()).ToString("yyyy-MM-dd");
                            txtlbl3.Text = dt.Rows[0]["col12"].ToString().Trim();
                            txtlbl8.Text = dt.Rows[0]["col13"].ToString().Trim();
                            txtlbl5.Text = Convert.ToDateTime(dt.Rows[0]["date2"].ToString().Trim()).ToString("yyyy-MM-dd");
                            txtlbl6.Text = dt.Rows[0]["col15"].ToString().Trim();
                            txtrmk.Text = dt.Rows[0]["remarks"].ToString().Trim();
                            txtCpartno.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                            doc_addl.Value = dt.Rows[0]["col11"].ToString().Trim();
                            create_tab();
                            sg1_dr = null;
                            if (dt.Rows[0]["icode"].ToString().Trim().Length > 1)
                            {
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
                                    sg1_dr["sg1_f1"] = dt.Rows[i]["icode"].ToString().Trim();
                                    sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                                    sg1_dr["sg1_f3"] = dt.Rows[i]["unit"].ToString().Trim();
                                    sg1_dr["sg1_t1"] = dt.Rows[i]["num1"].ToString().Trim();
                                    sg1_dr["sg1_t2"] = dt.Rows[i]["num2"].ToString().Trim();
                                    sg1_dr["sg1_t5"] = dt.Rows[i]["num3"].ToString().Trim();
                                    sg1_dt.Rows.Add(sg1_dr);
                                }
                            }
                        }
                    }

                    #endregion

                    #region Mould Plan
                    else
                    {
                        SQuery = "SELECT a.vchnum as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,A.CPARTNO,a.obsv1 as obsv1,a.DATE1 as DATE1,a.obsv14 as obsv14,a.obsv15 as obsv15,A.COL1 as col1,a.col3 as col3,a.col4 as col4,a.date2 as date2,b.name as mould_name,a.result as result,a.ent_by as ent_by,a.ent_dt as ent_dt,a.obsv2 FROM " + frm_tabname + " a , typegrp b WHERE trim(a.COL1)=trim(b.TYPE1) and trim(a.branchcd)=trim(b.branchcd) AND B.ID='MM' and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' and b.id='MM' AND TRIM(a.VCHNUM)||To_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + col1 + "' ORDER BY A.SRNO";
                        //SQuery = "select a.branchcd,a.type,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.icode,a.col1,a.col2,a.col3,to_char(a.doc_dt,'dd/mm/yyyy') as doc_dt,b.type1,a.ent_by, a.remarks,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt from "+frm_tabname+" a , typegrp b where trim(a.icode)=trim(b.acref) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "'";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                        ViewState["fstr"] = col1;
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (frm_formID == "F75150")
                        {
                            SQuery1 = "select trim(col1) as col1,to_char(date2,'dd/mm/yyyy') as plan_date,obsv2 as plan_month from wb_maint where branchcd='" + frm_mbr + "' and type='MM04' and obsv2='" + dt.Rows[0]["obsv2"].ToString().Trim() + "'";
                        }
                        else
                        {
                            SQuery1 = "select trim(col1) as col1,to_char(date2,'dd/mm/yyyy') as plan_date,obsv2 as plan_month from wb_maint where branchcd='" + frm_mbr + "' and type='MM05' and obsv2='" + dt.Rows[0]["obsv2"].ToString().Trim() + "'";
                        }
                        dt1 = new DataTable();
                        dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                        if (dt.Rows.Count > 0)
                        {
                            txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                            txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                            string sysdate = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'mm/yyyy') as ldt from dual", "ldt");
                            if (Convert.ToDateTime(dt.Rows[0]["obsv2"].ToString().Trim()) < Convert.ToDateTime(sysdate))
                            {
                                fgen.msg("-", "AMSG", "Editing Allowed For Current Month Only");
                                return;
                            }
                            txtlbl4.Text = dt.Rows[0]["obsv14"].ToString().Trim();
                            txtlbl4a.Text = dt.Rows[0]["obsv15"].ToString().Trim();
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
                                sg1_dr["sg1_f1"] = dt.Rows[i]["COL1"].ToString().Trim();
                                sg1_dr["sg1_f2"] = dt.Rows[i]["mould_name"].ToString().Trim();
                                sg1_dr["sg1_f3"] = dt.Rows[i]["CPARTNO"].ToString().Trim();
                                sg1_dr["sg1_f4"] = dt.Rows[i]["COL3"].ToString().Trim();
                                sg1_dr["sg1_t1"] = dt.Rows[i]["COL4"].ToString().Trim();
                                sg1_dr["sg1_t2"] = Convert.ToDateTime(dt.Rows[i]["DATE2"].ToString().Trim()).ToString("dd/MM/yyyy");
                                sg1_dr["sg1_t5"] = dt.Rows[i]["RESULT"].ToString().Trim();
                                sg1_dr["sg1_t4"] = dt.Rows[i]["obsv1"].ToString().Trim();
                                sg1_dr["sg1_t6"] = Convert.ToDateTime(dt.Rows[i]["DATE1"].ToString().Trim()).ToString("yyyy-MM-dd");
                                if (dt1.Rows.Count > 0)
                                {
                                    mq0 = fgen.seek_iname_dt(dt1, "col1='" + dt.Rows[i]["col1"].ToString().Trim() + "'", "col1");
                                }
                                if (mq0.Trim().Length > 1)
                                {
                                    sg1_dr["sg1_t7"] = "DONE";
                                }
                                else
                                {
                                    sg1_dr["sg1_t7"] = "-";
                                }
                                sg1_dt.Rows.Add(sg1_dr);
                            }
                        }
                    }
                    #endregion // mould plan

                    if (dt.Rows.Count > 0)
                    {
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        if (frm_formID == "F75153")
                        {
                            Cal();
                        }
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        if (frm_formID == "F75150" || frm_formID == "F75155")
                        {
                            for (int i = 0; i < sg1.Rows.Count - 1; i++)
                            {
                                mq1 = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text;
                                if (mq1 == "DONE")
                                {
                                    TextBox txt1 = (((TextBox)(sg1.Rows[i].FindControl("sg1_t6")))); ;
                                    txt1.Enabled = false;

                                }
                            }
                        }
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
                    fgen.fin_maint_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    SQuery = "";
                    if (frm_formID == "F75150" || frm_formID == "F75155")
                    {
                        if (Convert.ToInt32(col2) > 3 && Convert.ToInt32(col2) <= 12)
                        {

                        }
                        else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                        SQuery = "select distinct vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate from wb_maint where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and OBSV2='" + col2 + '/' + frm_myear + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            fgen.msg("", "AMSG", "Planning of this Month is Already Done on Entry No. " + dt.Rows[0]["vchnum"].ToString() + " Dt. " + dt.Rows[0]["vchdate"].ToString() + " '13' Please Edit that Entry!!");
                            fgen.ResetForm(this.Controls);
                            fgen.DisableForm(this.Controls);
                            clearctrl();
                            enablectrl();
                            return;
                        }
                        z = 0;
                        mq1 = "";
                        if (col2 == "01")
                        {
                            // IF SELECTED MONTH IS JANUARY THEN YEAR SHOULD BE -1 AND MONTH IS HARD CODED TO 12 AS 01-1 GIVES 0 AS A RESULT
                            mq1 = (Convert.ToInt32(frm_myear) - 1).ToString();
                            z = 12;
                        }
                        else
                        {
                            mq1 = frm_myear;
                            z = Convert.ToInt32(col2) - 1;
                        }
                        mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select mthname from mths where mthnum='" + fgen.padlc(z, 2) + "'", "mthname");
                        SQuery = "select distinct vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate from wb_maint where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and OBSV2='" + fgen.padlc(z, 2) + '/' + mq1 + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count <= 0)
                        {
                            mq3 = "";
                            mq3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + frm_mbr + "'", "OPT_START");// check date for mm start date
                            if (fgen.make_double(mq1 + fgen.padlc(z, 2)) >= fgen.make_double(mq3.Substring(6, 4) + mq3.Substring(3, 2)))
                            {
                                fgen.msg("", "AMSG", "Planning of Previous Month (" + mq0 + ") is Not Done.");
                                fgen.ResetForm(this.Controls);
                                fgen.DisableForm(this.Controls);
                                clearctrl();
                                enablectrl();
                                return;
                            }
                        }
                        txtlbl4.Text = col2;
                        txtlbl4a.Text = col3;

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
                                sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString().Replace("&amp;", "&");
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
                                sg1_dt.Rows.Add(sg1_dr);
                            }
                            dt1 = new DataTable();
                            if (frm_formID == "F75150" || frm_formID == "F75155")
                            {
                                if (frm_formID == "F75150")
                                {
                                    SQuery = "select b.name AS MOULD,b.acref AS MOULD_no,a.branchcd,b.provision as PM_life,sum(a.PM_FREQ_AS_PER_MONTH) as PM_FREQ_AS_PER_MONTH, a.CODE,max(to_char(a.col7,'dd/mm/yyyy')) as col7 from (select a.branchcd as branchcd, trim(a.col1) AS CODE,a.num12 AS PM_FREQ_AS_PER_MONTH,to_date(a.col7,'dd/mm/yyyy') as col7 from wb_master a where a.branchcd='" + frm_mbr + "' and a.id='MM01' AND NVL(A.COL2,'-')!='Y' union all select distinct a.branchcd as branchcd,trim(a.col1) AS CODE,0 AS PM_FREQ_AS_PER_MONTH,to_date(to_char(a.date1,'dd/mm/yyyy'),'dd/mm/yyyy') as col7 from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM04'  ) a ,typegrp b where trim(a.code)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and b.id='MM' group by b.name,b.acref,a.branchcd,b.provision,a.CODE";
                                    SQuery = "select b.name AS MOULD,b.acref AS MOULD_no,a.branchcd,b.provision as PM_life,sum(a.PM_FREQ_AS_PER_MONTH) as PM_FREQ_AS_PER_MONTH, a.CODE,max(a.col7) as col7 from (select a.branchcd as branchcd, trim(a.col1) AS CODE,a.num12 AS PM_FREQ_AS_PER_MONTH,to_char(to_date(a.col7,'dd/mm/yyyy'),'yyyymmdd') as col7 from wb_master a where a.branchcd='" + frm_mbr + "' and a.id='MM01' AND NVL(A.COL2,'-')!='Y' union all select distinct a.branchcd as branchcd,trim(a.col1) AS CODE,0 AS PM_FREQ_AS_PER_MONTH,to_char(a.date1,'yyyymmdd') as col7 from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM04') a ,typegrp b where trim(a.code)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and b.id='MM'  group by b.name,b.acref,a.branchcd,b.provision,a.CODE";
                                    mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + frm_mbr + "'", "OPT_START");// check date for mm start date
                                    DateRange = "Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1";
                                    SQuery1 = "Select max(Name) as Mould_Name ,Acref as Mould_No,max(provision) as Mnt_life,sum(prodn) as shots_utilised,max(provision)-(sum(prodn)-sum(maint)) as Bal_mainT_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as maint,0 as prodn,to_number(provision) as provision from typegrp where branchcd='" + frm_mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num1,0 as prodn,0 as provision from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM04' and a.date1 Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0) as totp,0 as provision from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and vchdate " + DateRange + ") group by acref ";

                                    mq4 = "select distinct mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'dd/mm/yyyy') as vchdate from wb_maint where branchcd='" + frm_mbr + "' and type='MM04' ) group by mould";
                                    mq4 = "select distinct mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'yyyymmdd') as vchdate from wb_maint where branchcd='" + frm_mbr + "' and type='MM04') group by mould";
                                }
                                else
                                {
                                    SQuery = "select b.name AS MOULD,b.acref AS MOULD_no,a.branchcd,sum(a.HM_FREQ_AS_PER_MONTH) as HM_FREQ_AS_PER_MONTH,b.pageno as HM_life, a.CODE,max(to_char(a.col8,'dd/mm/yyyy')) as col8 from (select a.branchcd as branchcd, trim(a.col1) AS CODE,a.num15 AS HM_FREQ_AS_PER_MONTH,to_date(a.col8,'dd/mm/yyyy') as col8 from wb_master a where a.branchcd='" + frm_mbr + "' and a.id='MM01' AND NVL(A.COL2,'-')!='Y' union all select a.branchcd as branchcd,trim(a.col1) AS CODE,0 AS HM_FREQ_AS_PER_MONTH,to_date(to_char(a.date1,'dd/mm/yyyy'),'dd/mm/yyyy') as col8 from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM05'  ) a ,typegrp b where trim(a.code)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and b.id='MM' group by b.name,b.acref,a.branchcd,b.pageno,a.CODE";
                                    mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + frm_mbr + "'", "OPT_START");// check date for mm start date
                                    DateRange = "Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1";
                                    //SQuery1 = "Select max(Name) as Mould_Name ,Acref as Mould_No,max(pageno) as Mnt_life,sum(prodn) as shots_utilised,max(pageno)-(sum(prodn)-sum(maint)) as Bal_mainT_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as maint,0 as prodn,to_number(pageno) as pageno from typegrp where branchcd='" + frm_mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num1,0 as prodn,0 as pageno from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM05' and a.date1 " + DateRange + " union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0) as totp,0 as pageno from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and vchdate " + DateRange + ") group by acref"; //old
                                    SQuery1 = "Select max(Name) as Mould_Name ,Acref as Mould_No,max(pageno) as Mnt_life,sum(prodn) as shots_utilised,max(pageno)-(sum(prodn)-sum(maint)) as Bal_mainT_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as maint,0 as prodn,to_number(pageno) as pageno from typegrp where branchcd='" + frm_mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num1,0 as prodn,0 as pageno from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM05' and a.date1 " + DateRange + " union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,nvl(noups,0)*nvl(fm_fact,0) as totp,0 as pageno from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and vchdate " + DateRange + ") group by acref"; //change in formula onyl

                                    mq4 = "select distinct mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'dd/mm/yyyy') as vchdate from wb_maint where branchcd='" + frm_mbr + "' and type='MM05' ) group by mould";

                                    mq1 = "SELECT b.cpartno as acref,to_char(b.date1,'dd/mm/yyyy') as comm_dt, trim(b.num13) as first_hm, to_number(trim(b.col12)) as op, b.col1 AS MOULD_CODE,b.col4 AS MOULD_NAME,to_number(b.col15) AS HM_life,B.NUM5 AS ALERT,TRIM(B.ACODE) AS ACODE,TRIM(C.ANAME) AS PARTY,TRIM(B.ICODE) AS ICODE,b.col8 as last_hm_dt  FROM  WB_MASTER B ,FAMST C WHERE  TRIM(B.ACODE)=TRIM(C.ACODE) AND B.BRANCHCD='" + frm_mbr + "' AND B.ID='MM01' AND NVL(B.COL2,'-')!='Y' order by mould_code"; // b.col1='0134'
                                    dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1); //master picking  dt
                                }
                                dt = new DataTable();
                                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//master 

                                dt2 = new DataTable();
                                dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);//life

                                dt4 = new DataTable();
                                dt4 = fgen.getdata(frm_qstr, frm_cocd, mq4);//maintenenace checking data
                                if (mq0.Length <= 1)
                                {
                                    fgen.msg("-", "AMSG", "Mould Maintenenace Start Date Required For Each Branch");
                                    return;
                                }
                                for (int i = 0; i < dt.Rows.Count; i++)
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
                                    sg1_dr["sg1_f1"] = dt.Rows[i]["code"].ToString().Trim();
                                    sg1_dr["sg1_f2"] = dt.Rows[i]["mould"].ToString().Trim();
                                    sg1_dr["sg1_f3"] = dt.Rows[i]["mould_no"].ToString().Trim();
                                    if (frm_formID == "F75150")
                                    {
                                        mq5 = ""; mq6 = ""; mq7 = ""; mq8 = "";
                                        mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dt.Rows[i]["code"].ToString().Trim() + "'", "vchdate");
                                        if (mq5.Length == 8)
                                        {
                                            date1 = DateTime.ParseExact(mq5, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                            mq5 = date1.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                        }
                                        mq6 = mq5;
                                        if (mq5.Trim().Length <= 1)
                                        {
                                            mq5 = dt.Rows[i]["col7"].ToString().Trim();
                                            if (mq5.Length == 8)
                                            {
                                                date1 = DateTime.ParseExact(mq5, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                                mq5 = date1.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                            }
                                            mq6 = mq5;
                                            if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                                            {
                                            }
                                            else
                                            {
                                                mq5 = mq0;
                                            }
                                        }
                                        //mq7 = fgen.seek_iname(frm_qstr, frm_cocd, " Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); 
                                        mq7 = fgen.seek_iname(frm_qstr, frm_cocd, " Select trim(pvchnum) as Acref ,sum(nvl(noups,0)*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); 
                                        sg1_dr["sg1_t1"] = fgen.make_double(dt.Rows[i]["PM_life"].ToString().Trim()) - fgen.make_double(mq7);
                                        sg1_dr["sg1_t2"] = mq6;
                                        sg1_dr["sg1_f4"] = dt.Rows[i]["pm_life"].ToString().Trim();
                                    }
                                    else
                                    {
                                        mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dt.Rows[i]["code"].ToString().Trim() + "'", "vchdate");
                                        mq6 = mq5;
                                        mq8 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "hm_life");
                                        if (mq5.Trim().Length <= 1)
                                        {
                                            mq5 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "last_hm_dt");
                                            mq6 = mq5;
                                            if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                                            {
                                            }
                                            else
                                            {
                                                mq5 = mq0;
                                            }
                                            if (Convert.ToDateTime(mq5) == Convert.ToDateTime(fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "comm_dt")))
                                            {
                                                mq8 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "first_hm");
                                            }
                                            else
                                            {
                                            }
                                        }
                                        //mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                        mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum(nvl(noups,0)*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                        sg1_dr["sg1_t1"] = fgen.make_double(mq8) - fgen.make_double(mq7);
                                        sg1_dr["sg1_t2"] = mq6;
                                        sg1_dr["sg1_f4"] = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "hm_life");
                                    }
                                    if (frm_formID == "F75150")
                                    {
                                        sg1_dr["sg1_t4"] = fgen.make_double(dt.Rows[i]["PM_FREQ_AS_PER_MONTH"].ToString().Trim());
                                    }
                                    else
                                    {
                                        sg1_dr["sg1_t4"] = fgen.make_double(dt.Rows[i]["HM_FREQ_AS_PER_MONTH"].ToString().Trim());
                                    }
                                    if (frm_formID == "F75150")
                                    {
                                        date1 = Convert.ToDateTime(sg1_dr["sg1_t2"]).AddMonths(fgen.make_int(dt.Rows[i]["PM_FREQ_AS_PER_MONTH"].ToString().Trim()));
                                    }
                                    else
                                    {
                                        date1 = Convert.ToDateTime(sg1_dr["sg1_t2"]).AddMonths(fgen.make_int(dt.Rows[i]["HM_FREQ_AS_PER_MONTH"].ToString().Trim()));
                                    }
                                    sg1_dr["sg1_t6"] = Convert.ToDateTime(date1).AddDays(-1).ToString("yyyy-MM-dd");
                                    sg1_dr["sg1_t7"] = "DUE";
                                    if (Convert.ToDateTime(sg1_dr["sg1_t6"]).ToString("dd/MM/yyyy").Substring(3, 7) == txtlbl4.Text + "/" + frm_myear)
                                    {
                                        mq9 += ",'" + dt.Rows[i]["code"].ToString().Trim() + "'";
                                        sg1_dt.Rows.Add(sg1_dr);
                                    }
                                }

                                if (frm_formID == "F75150")
                                {
                                    SQuery = "select c.name AS MOULD,c.acref AS MOULD_no,b.num12 as PM_FREQ_AS_PER_MONTH, trim(a.col1) as CODE,b.col7,a.plan_date,a.plan_month,c.provision as pm_life from (select distinct col1,to_char(date1,'dd/mm/yyyy') as plan_date,obsv2 as plan_month,1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM02' and nvl(trim(grade),'-')!='Y' union all select distinct col1,to_char(date2,'dd/mm/yyyy') as plan_date,obsv2 as plan_month,-1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM04') a ,wb_master b,typegrp c where trim(a.col1)=trim(b.col1) and b.id='MM01' and trim(a.col1)=trim(c.type1) and b.branchcd='" + frm_mbr + "' and c.branchcd='" + frm_mbr + "' and c.id='MM' AND NVL(B.COL2,'-')!='Y' group by c.name,c.acref,b.num12,trim(a.col1),b.col7,a.plan_date,a.plan_month,c.provision having sum(qty)>0 order by mould";
                                    // PRESENT IN MASTER BUT NOT PLANNED
                                    SQuery1 = "SELECT TRIM(a.COL1) AS CODE,c.name AS MOULD,c.acref AS MOULD_no,c.provision as pm_life,sum(a.num12) as PM_FREQ_AS_PER_MONTH,max(a.col7) as col7 FROM (SELECT COL1,1 AS QTY,num12,to_char(to_date(col7,'dd/mm/yyyy'),'dd/mm/yyyy') as col7 FROM WB_MASTER WHERE BRANCHCD='" + frm_mbr + "' AND ID='MM01' AND NVL(COL2,'-')!='Y' UNION ALL SELECT COL1,-1 AS QTY,0 as num12,null as col7 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='MM02')a,typegrp c where trim(a.col1)=trim(c.type1) and c.branchcd='" + frm_mbr + "' and c.id='MM' GROUP BY COL1,c.name,c.acref,c.provision having sum(qty)>0 ORDER BY CODE";
                                    // NOT COMING IN NEXT PLANNED DATE
                                    SQuery2 = "SELECT TRIM(A.COL1) AS Code,MAX(A.DATE1) AS DATE1,c.name AS MOULD,c.acref AS MOULD_no,c.provision as pm_life,MAX(A.OBSV2) AS OBSV2,A.NUM12 AS PM_FREQ_AS_PER_MONTH FROM (SELECT A.BRANCHCD,A.COL1,TO_CHAR(A.DATE1,'YYYYMMDD') AS DATE1,TO_CHAR(TO_DATE(A.OBSV2,'MM/YYYY'),'YYYYMM') AS OBSV2,B.NUM12 FROM WB_MAINT A,WB_MASTER B WHERE TRIM(A.BRANCHCD)||TRIM(A.COL1)=TRIM(B.BRANCHCD)||TRIM(B.COL1) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='MM04' AND B.ID='MM01')A,TYPEGRP C WHERE TRIM(A.COL1)=TRIM(C.TYPE1) AND A.BRANCHCD='" + frm_mbr + "' AND C.BRANCHCD='" + frm_mbr + "' AND C.ID='MM' GROUP BY TRIM(A.COL1),c.namE,c.acref,c.provision,A.NUM12 ORDER BY CODE";
                                    SQuery3 = "SELECT TRIM(COL1) AS COL1,TO_CHAR(TO_DATE(OBSV2,'MM/YYYY'),'YYYYMM') AS OBSV2 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='MM02' order by obsv2 desc";
                                }
                                else
                                {
                                    SQuery = "select c.name AS MOULD,c.acref AS MOULD_no,b.num15 as HM_FREQ_AS_PER_MONTH, trim(a.col1) as CODE,b.col8,a.plan_date,a.plan_month,c.pageno as HM_life  from (select distinct col1,to_char(date1,'dd/mm/yyyy') as plan_date,obsv2 as plan_month,1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM03' and nvl(trim(grade),'-')!='Y' union all select distinct col1,to_char(date2,'dd/mm/yyyy') as plan_date,obsv2 as plan_month,-1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM05') a ,wb_master b,typegrp c where trim(a.col1)=trim(b.col1) and b.id='MM01' and trim(a.col1)=trim(c.type1) and b.branchcd='" + frm_mbr + "' and c.branchcd='" + frm_mbr + "' and c.id='MM' AND NVL(B.COL2,'-')!='Y'  group by c.name,c.acref,b.num15,trim(a.col1),b.col8,a.plan_date,a.plan_month,c.pageno having sum(qty)>0 order by mould";
                                    // PRESENT IN MASTER BUT NOT PLANNED
                                    SQuery1 = "-";
                                    // NOT COMING IN NEXT PLANNED DATE
                                    SQuery2 = "-";
                                    SQuery3 = "-";
                                }
                                #region PLANNED BUT NOT MAINTAINED
                                dt = new DataTable();
                                dt6 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                                dt5 = new DataTable();

                                if (dt6.Rows.Count > 0)
                                {
                                    DataView dv = new DataView(dt6);
                                    dt5 = dv.ToTable(true, "code");
                                }

                                foreach (DataRow dr in dt5.Rows)
                                {
                                    DataView dv2 = new DataView(dt6, "code='" + dr["code"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                    dt = dv2.ToTable();

                                    z = dt.Rows.Count;
                                    for (int i = 0; i < dt.Rows.Count; i++)
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
                                        sg1_dr["sg1_f1"] = dt.Rows[i]["code"].ToString().Trim();
                                        sg1_dr["sg1_f2"] = dt.Rows[i]["mould"].ToString().Trim();
                                        sg1_dr["sg1_f3"] = dt.Rows[i]["mould_no"].ToString().Trim();
                                        sg1_dr["sg1_f4"] = fgen.make_double(fgen.seek_iname_dt(dt2, "MOULD_NO='" + dt.Rows[i]["MOULD_NO"].ToString().Trim() + "'", "MNT_LIFE"));
                                        sg1_dr["sg1_t1"] = fgen.make_double(fgen.seek_iname_dt(dt2, "MOULD_NO='" + dt.Rows[i]["MOULD_NO"].ToString().Trim() + "'", "Bal_mainT_life"));
                                        if (frm_formID == "F75150")
                                        {
                                            mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dt.Rows[i]["code"].ToString().Trim() + "'", "vchdate");
                                            if (mq5.Length == 8)
                                            {
                                                date1 = DateTime.ParseExact(mq5, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                                mq5 = date1.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                            }
                                            mq6 = mq5;
                                            if (mq5.Trim().Length <= 1)
                                            {
                                                mq5 = dt.Rows[i]["col7"].ToString().Trim();
                                                if (mq5.Length == 8)
                                                {
                                                    date1 = DateTime.ParseExact(mq5, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                                    mq5 = date1.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                mq6 = mq5;
                                                if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                                                {
                                                }
                                                else
                                                {
                                                    mq5 = mq0;
                                                }
                                            }
                                            //mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                            mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum(nvl(noups,0)*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                            //sg1_dr["sg1_t1"] = mq6;
                                            //sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["PM_life"].ToString().Trim()) - fgen.make_double(mq7);
                                            sg1_dr["sg1_t1"] = fgen.make_double(dt.Rows[i]["PM_life"].ToString().Trim()) - fgen.make_double(mq7);
                                            sg1_dr["sg1_t2"] = mq6;
                                            sg1_dr["sg1_f4"] = dt.Rows[i]["PM_life"].ToString().Trim();
                                        }
                                        else
                                        {
                                            mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dt.Rows[i]["code"].ToString().Trim() + "'", "vchdate");
                                            mq6 = mq5;
                                            mq8 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "hm_life");
                                            if (mq5.Trim().Length <= 1)
                                            {
                                                mq5 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "last_hm_dt");
                                                mq6 = mq5;
                                                if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                                                {
                                                }
                                                else
                                                {
                                                    mq5 = mq0;
                                                }
                                                if (Convert.ToDateTime(mq5) == Convert.ToDateTime(fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "comm_dt")))
                                                {
                                                    mq8 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "first_hm");
                                                }
                                                else
                                                {
                                                }
                                            }
                                            //mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                            mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum(nvl(noups,0)*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                            sg1_dr["sg1_t1"] = fgen.make_double(mq8) - fgen.make_double(mq7);
                                            sg1_dr["sg1_t2"] = mq6;
                                            sg1_dr["sg1_f4"] = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "hm_life");
                                        }
                                        if (frm_formID == "F75150")
                                        {
                                            sg1_dr["sg1_t4"] = fgen.make_double(dt.Rows[i]["PM_FREQ_AS_PER_MONTH"].ToString().Trim());
                                        }
                                        else
                                        {
                                            sg1_dr["sg1_t4"] = fgen.make_double(dt.Rows[i]["HM_FREQ_AS_PER_MONTH"].ToString().Trim());
                                        }
                                        sg1_dr["sg1_t6"] = Convert.ToDateTime("01/" + txtlbl4.Text + "/" + frm_myear).ToString("yyyy-MM-dd");
                                        sg1_dr["sg1_t7"] = "OVERDUE";
                                        if (z > 1)
                                        {
                                            sg1_dr["sg1_t8"] = "PLANNED2TIMES";
                                        }
                                        sg1_dt.Rows.Add(sg1_dr);
                                    }
                                }
                                #endregion

                                #region PRESENT IN MASTER BUT NOT PLANNED EVEN ONCE
                                if (SQuery1.Length > 1)
                                {
                                    dt7 = new DataTable();
                                    dt7 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                                    dt8 = new DataTable();

                                    if (dt7.Rows.Count > 0)
                                    {
                                        DataView dv = new DataView(dt7);
                                        dt8 = dv.ToTable(true, "code");
                                    }

                                    dt = new DataTable();
                                    foreach (DataRow dr in dt8.Rows)
                                    {
                                        DataView dv2 = new DataView(dt7, "code='" + dr["code"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                        dt = dv2.ToTable();
                                        mq13 = ""; mq14 = ""; mq15 = "";
                                        for (int i = 0; i < dt.Rows.Count; i++)
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
                                            sg1_dr["sg1_f1"] = dt.Rows[i]["code"].ToString().Trim();
                                            sg1_dr["sg1_f2"] = dt.Rows[i]["mould"].ToString().Trim();
                                            sg1_dr["sg1_f3"] = dt.Rows[i]["mould_no"].ToString().Trim();
                                            sg1_dr["sg1_f4"] = fgen.make_double(fgen.seek_iname_dt(dt2, "MOULD_NO='" + dt.Rows[i]["MOULD_NO"].ToString().Trim() + "'", "MNT_LIFE"));
                                            sg1_dr["sg1_t1"] = fgen.make_double(fgen.seek_iname_dt(dt2, "MOULD_NO='" + dt.Rows[i]["MOULD_NO"].ToString().Trim() + "'", "Bal_mainT_life"));
                                            if (frm_formID == "F75150")
                                            {
                                                mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dt.Rows[i]["code"].ToString().Trim() + "'", "vchdate");
                                                if (mq5.Length == 8)
                                                {
                                                    date1 = DateTime.ParseExact(mq5, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                                    mq5 = date1.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                }
                                                mq6 = mq5;
                                                if (mq5.Trim().Length <= 1)
                                                {
                                                    mq5 = dt.Rows[i]["col7"].ToString().Trim();
                                                    if (mq5.Length == 8)
                                                    {
                                                        date1 = DateTime.ParseExact(mq5, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                                        mq5 = date1.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                    }
                                                    mq6 = mq5;
                                                    if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                                                    {
                                                    }
                                                    else
                                                    {
                                                        mq5 = mq0;
                                                    }
                                                }
                                                //mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                                mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum(nvl(noups,0)*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                                //sg1_dr["sg1_t1"] = mq6;
                                                //sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["PM_life"].ToString().Trim()) - fgen.make_double(mq7);
                                                sg1_dr["sg1_t1"] = fgen.make_double(dt.Rows[i]["PM_life"].ToString().Trim()) - fgen.make_double(mq7);
                                                sg1_dr["sg1_t2"] = mq6;
                                                sg1_dr["sg1_f4"] = dt.Rows[i]["PM_life"].ToString().Trim();
                                            }
                                            else
                                            {
                                                mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dt.Rows[i]["code"].ToString().Trim() + "'", "vchdate");
                                                mq6 = mq5;
                                                mq8 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "hm_life");
                                                if (mq5.Trim().Length <= 1)
                                                {
                                                    mq5 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "last_hm_dt");
                                                    mq6 = mq5;
                                                    if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                                                    {
                                                    }
                                                    else
                                                    {
                                                        mq5 = mq0;
                                                    }
                                                    if (Convert.ToDateTime(mq5) == Convert.ToDateTime(fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "comm_dt")))
                                                    {
                                                        mq8 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "first_hm");
                                                    }
                                                    else
                                                    {
                                                    }
                                                }
                                                //mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                                mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum(nvl(noups,0)*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                                sg1_dr["sg1_t1"] = fgen.make_double(mq8) - fgen.make_double(mq7);
                                                sg1_dr["sg1_t2"] = mq6;
                                                sg1_dr["sg1_f4"] = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "hm_life");
                                            }
                                            if (frm_formID == "F75150")
                                            {
                                                sg1_dr["sg1_t4"] = fgen.make_double(dt.Rows[i]["PM_FREQ_AS_PER_MONTH"].ToString().Trim());
                                            }
                                            else
                                            {
                                                sg1_dr["sg1_t4"] = fgen.make_double(dt.Rows[i]["HM_FREQ_AS_PER_MONTH"].ToString().Trim());
                                            }
                                            sg1_dr["sg1_t6"] = Convert.ToDateTime("01/" + txtlbl4.Text + "/" + frm_myear).ToString("yyyy-MM-dd");
                                            sg1_dr["sg1_t7"] = "NOT PLANNED / OVERDUE";
                                            sg1_dr["sg1_t8"] = "NOTPLANNED";
                                            if (frm_formID == "F75150")
                                            {
                                                date1 = Convert.ToDateTime(sg1_dr["sg1_t2"]).AddMonths(fgen.make_int(dt.Rows[i]["PM_FREQ_AS_PER_MONTH"].ToString().Trim()));
                                            }
                                            else
                                            {
                                                date1 = Convert.ToDateTime(sg1_dr["sg1_t2"]).AddMonths(fgen.make_int(dt.Rows[i]["HM_FREQ_AS_PER_MONTH"].ToString().Trim()));
                                            }
                                            // sg1_dr["sg1_t6"] = Convert.ToDateTime(date1).ToString("yyyy-MM-dd");
                                            date2 = date1;
                                            mq14 = date2.ToString().Substring(6, 4) + date2.ToString().Substring(3, 2);
                                            mq15 = frm_myear + txtlbl4.Text;
                                            sg1_dr["sg1_t6"] = Convert.ToDateTime("01/" + txtlbl4.Text + "/" + frm_myear).ToString("yyyy-MM-dd");
                                            if (fgen.make_int(mq14) <= fgen.make_int(mq15))
                                            {
                                                sg1_dt.Rows.Add(sg1_dr);
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region DUE TO WRONG PICKING OF MAX MAINT.DATE
                                if (SQuery2.Length > 1)
                                {
                                    dt9 = new DataTable();
                                    dt9 = fgen.getdata(frm_qstr, frm_cocd, SQuery2);
                                    dt10 = new DataTable();

                                    dt11 = new DataTable();
                                    dt11 = fgen.getdata(frm_qstr, frm_cocd, SQuery3);

                                    if (dt9.Rows.Count > 0)
                                    {
                                        DataView dv = new DataView(dt9);
                                        dt10 = dv.ToTable(true, "code");
                                    }

                                    dt = new DataTable();
                                    foreach (DataRow dr in dt10.Rows)
                                    {
                                        DataView dv2 = new DataView(dt9, "code='" + dr["code"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                                        dt = dv2.ToTable();
                                        mq11 = ""; mq12 = ""; mq13 = ""; mq14 = ""; mq15 = "";
                                        mq10 = "";
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            mq11 = dt.Rows[i]["date1"].ToString().Trim();
                                            if (mq11.Length == 8)
                                            {
                                                date1 = DateTime.ParseExact(mq11, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                                mq12 = date1.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                date2 = Convert.ToDateTime(mq12).AddMonths(fgen.make_int(dt.Rows[i]["PM_FREQ_AS_PER_MONTH"].ToString().Trim()));
                                                mq13 = date2.ToString().Substring(3, 8);
                                                mq14 = date2.ToString().Substring(6, 4) + date2.ToString().Substring(3, 2);
                                                mq15 = frm_myear + txtlbl4.Text;
                                            }

                                            // mq10 = fgen.seek_iname_dt(dt11, "col1='" + dr["code"].ToString().Trim() + "' and obsv2='" + dt.Rows[i]["obsv2"].ToString().Trim() + "'", "OBSV2");
                                            mq10 = fgen.seek_iname_dt(dt11, "col1='" + dr["code"].ToString().Trim() + "'", "OBSV2");
                                            if (Convert.ToInt32(mq10.Trim()) < Convert.ToInt32(mq14.Trim()))
                                            {
                                                if (fgen.make_int(mq14) < fgen.make_int(mq15))
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
                                                    sg1_dr["sg1_f1"] = dt.Rows[i]["code"].ToString().Trim();
                                                    sg1_dr["sg1_f2"] = dt.Rows[i]["mould"].ToString().Trim();
                                                    sg1_dr["sg1_f3"] = dt.Rows[i]["mould_no"].ToString().Trim();
                                                    sg1_dr["sg1_f4"] = fgen.make_double(fgen.seek_iname_dt(dt2, "MOULD_NO='" + dt.Rows[i]["MOULD_NO"].ToString().Trim() + "'", "MNT_LIFE"));
                                                    sg1_dr["sg1_t1"] = fgen.make_double(fgen.seek_iname_dt(dt2, "MOULD_NO='" + dt.Rows[i]["MOULD_NO"].ToString().Trim() + "'", "Bal_mainT_life"));
                                                    if (frm_formID == "F75150")
                                                    {
                                                        mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dt.Rows[i]["code"].ToString().Trim() + "'", "vchdate");
                                                        if (mq5.Length == 8)
                                                        {
                                                            date1 = DateTime.ParseExact(mq5, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                                            mq5 = date1.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                        }
                                                        mq6 = mq5;
                                                        if (mq5.Trim().Length <= 1)
                                                        {
                                                            mq5 = dt.Rows[i]["col7"].ToString().Trim();
                                                            if (mq5.Length == 8)
                                                            {
                                                                date1 = DateTime.ParseExact(mq5, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                                                                mq5 = date1.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                                            }
                                                            mq6 = mq5;
                                                            if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                                                            {
                                                            }
                                                            else
                                                            {
                                                                mq5 = mq0;
                                                            }
                                                        }
                                                        //mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                                        mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum(nvl(noups,0)*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                                        //sg1_dr["sg1_t1"] = mq6;
                                                        //sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["PM_life"].ToString().Trim()) - fgen.make_double(mq7);
                                                        sg1_dr["sg1_t1"] = fgen.make_double(dt.Rows[i]["PM_life"].ToString().Trim()) - fgen.make_double(mq7);
                                                        sg1_dr["sg1_t2"] = mq6;
                                                        sg1_dr["sg1_f4"] = dt.Rows[i]["PM_life"].ToString().Trim();
                                                    }
                                                    else
                                                    {
                                                        mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dt.Rows[i]["code"].ToString().Trim() + "'", "vchdate");
                                                        mq6 = mq5;
                                                        mq8 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "hm_life");
                                                        if (mq5.Trim().Length <= 1)
                                                        {
                                                            mq5 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "last_hm_dt");
                                                            mq6 = mq5;
                                                            if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                                                            {
                                                            }
                                                            else
                                                            {
                                                                mq5 = mq0;
                                                            }
                                                            if (Convert.ToDateTime(mq5) == Convert.ToDateTime(fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "comm_dt")))
                                                            {
                                                                mq8 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "first_hm");
                                                            }
                                                            else
                                                            {
                                                            }
                                                        }
                                                        //mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                                        mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum(nvl(noups,0)*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                                        sg1_dr["sg1_t1"] = fgen.make_double(mq8) - fgen.make_double(mq7);
                                                        sg1_dr["sg1_t2"] = mq6;
                                                        sg1_dr["sg1_f4"] = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "hm_life");
                                                    }
                                                    if (frm_formID == "F75150")
                                                    {
                                                        sg1_dr["sg1_t4"] = fgen.make_double(dt.Rows[i]["PM_FREQ_AS_PER_MONTH"].ToString().Trim());
                                                    }
                                                    else
                                                    {
                                                        sg1_dr["sg1_t4"] = fgen.make_double(dt.Rows[i]["HM_FREQ_AS_PER_MONTH"].ToString().Trim());
                                                    }
                                                    sg1_dr["sg1_t6"] = Convert.ToDateTime("01/" + txtlbl4.Text + "/" + frm_myear).ToString("yyyy-MM-dd");
                                                    sg1_dr["sg1_t7"] = "NOT MAINTAINED / UNPLANNED";
                                                    sg1_dr["sg1_t8"] = "NOTMAINTAINED_UNPLANNED";
                                                    sg1_dt.Rows.Add(sg1_dr);
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        sg1.Rows[z].Cells[10].Focus();
                        setColHeadings();
                        for (int i = 0; i < sg1.Rows.Count - 1; i++)
                        {
                            mq1 = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text;
                            if (mq1 == "PLANNED2TIMES" || mq1 == "NOTPLANNED" || mq1 == "NOTMAINTAINED_UNPLANNED")
                            {
                                TextBox txt1 = (((TextBox)(sg1.Rows[i].FindControl("sg1_t6")))); ;
                                txt1.BackColor = Color.LightSkyBlue;
                            }
                        }
                    }
                    else if (frm_formID == "F75153")
                    {
                        txtlbl4.Text = col2;
                        txtlbl4a.Text = col3;
                        SQuery = "select trim(a.entry) as entry,a.mould_code as code,b.name as mould_name,a.breakdown_date,max(a.btchno) As mach_Cd,max(a.title) As mach_name,trim(b.acref) as cpartno from (select distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as entry , col1 as mould_code,to_char(date1,'dd/mm/yyyy') as breakdown_date,1 as qty,btchno,title from wb_maint where branchcd='" + frm_mbr + "' and type='MM06' and vchdate " + DateRange + " union all select distinct col11 as entry ,col1 as mould_code,to_char(date1,'dd/mm/yyyy')  as breakdown_date,-1 as qty,btchno,title from wb_maint where branchcd='" + frm_mbr + "' and type='MM07' and vchdate " + DateRange + ") a,typegrp b where trim(a.mould_code)=trim(b.type1) and b.branchcd='" + frm_mbr + "' and b.id='MM' and trim(a.entry)||trim(a.mould_code )='" + col1 + "' group by trim(a.entry),a.mould_code,b.name,a.breakdown_date,trim(b.acref) having sum(qty)>0 order by b.name";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtlbl7.Text = dt.Rows[0]["mach_cd"].ToString().Trim();
                            txtlbl7a.Text = dt.Rows[0]["mach_name"].ToString().Trim();
                            txtlbl2.Text = Convert.ToDateTime(dt.Rows[0]["breakdown_date"].ToString().Trim()).ToString("yyyy-MM-dd");
                            txtlbl3.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select col12 from wb_maint where branchcd='" + frm_mbr + "' and type='MM06' and trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(col1)='" + col1 + "'", "col12");
                            txtCpartno.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                            doc_addl.Value = dt.Rows[0]["entry"].ToString().Trim();
                        }
                    }
                    btnlbl4.Enabled = false;
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
                    if (col1.Length <= 0) return;
                    txtUserID.Text = col1;
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
                            sg1_dr["sg1_srno"] = Convert.ToInt32(sg1.Rows[i].Cells[12].Text);
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
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString().Replace("&amp;", "&");
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        if (frm_formID == "F75150" || frm_formID == "F75155")
                        {
                            frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                            if (Convert.ToInt32(txtlbl4.Text) > 3 && Convert.ToInt32(txtlbl4.Text) <= 12)
                            {

                            }
                            else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                            dt1 = new DataTable();
                            if (frm_formID == "F75150")
                            {
                                SQuery = "select b.name AS MOULD,b.acref AS MOULD_no,a.branchcd,sum(a.PM_FREQ_AS_PER_MONTH) as PM_FREQ_AS_PER_MONTH,b.provision as PM_life,a.CODE,max(to_char(a.col7,'dd/mm/yyyy')) as col7 from (select a.branchcd as branchcd, trim(a.col1) AS CODE,a.num12 AS PM_FREQ_AS_PER_MONTH,to_date(a.col7,'dd/mm/yyyy') as col7 from wb_master a where a.branchcd='" + frm_mbr + "' and a.id='MM01' AND NVL(A.COL2,'-')!='Y' union all select a.branchcd as branchcd,trim(a.col1) AS CODE,0 AS PM_FREQ_AS_PER_MONTH,to_date(to_char(a.date1,'dd/mm/yyyy'),'dd/mm/yyyy') as col7 from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM04'  ) a ,typegrp b where trim(a.code)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and b.id='MM' and code in (" + col1 + ") group by b.name,b.acref,a.branchcd,b.provision,a.CODE";

                                mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + frm_mbr + "'", "OPT_START");// check date for mm start date
                                DateRange = "Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1";
                                SQuery1 = "Select max(Name) as Mould_Name ,Acref as Mould_No,max(provision) as Mnt_life,sum(prodn) as shots_utilised,max(provision)-(sum(prodn)-sum(maint)) as Bal_mainT_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as maint,0 as prodn,to_number(provision) as provision from typegrp where branchcd='" + frm_mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num1,0 as prodn,0 as provision from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM04' and a.date1 Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0) as totp,0 as provision from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and vchdate " + DateRange + ") group by acref ";

                                mq4 = "select distinct mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'dd/mm/yyyy') as vchdate from wb_maint where branchcd='" + frm_mbr + "' and type='MM04' ) group by mould";
                            }
                            else
                            {
                                SQuery = "select b.name AS MOULD,b.acref AS MOULD_no,a.branchcd, sum(a.HM_FREQ_AS_PER_MONTH) as HM_FREQ_AS_PER_MONTH,b.pageno as HM_life,a.CODE,max(to_char(a.col8,'dd/mm/yyyy')) as col8 from (select a.branchcd as branchcd, trim(a.col1) AS CODE,a.num15 AS HM_FREQ_AS_PER_MONTH,to_date(a.col8,'dd/mm/yyyy') as col8 from wb_master a where a.branchcd='" + frm_mbr + "' and a.id='MM01' AND NVL(A.COL2,'-')!='Y' union all select a.branchcd as branchcd,trim(a.col1) AS CODE,0 AS HM_FREQ_AS_PER_MONTH,to_date(to_char(a.date1,'dd/mm/yyyy'),'dd/mm/yyyy') as col8 from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM05'  ) a ,typegrp b where trim(a.code)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and b.id='MM' and code in (" + col1 + ") group by b.name,b.acref,a.branchcd,b.pageno,a.CODE";

                                mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + frm_mbr + "'", "OPT_START");// check date for mm start date
                                DateRange = "Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1";
                                //SQuery1 = "Select max(Name) as Mould_Name ,Acref as Mould_No,max(pageno) as Mnt_life,sum(prodn) as shots_utilised,max(pageno)-(sum(prodn)-sum(maint)) as Bal_mainT_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as maint,0 as prodn,to_number(pageno) as pageno from typegrp where branchcd='" + frm_mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num1,0 as prodn,0 as pageno from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM05' and a.date1 " + DateRange + " union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0) as totp,0 as pageno from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and vchdate " + DateRange + ") group by acref";
                                SQuery1 = "Select max(Name) as Mould_Name ,Acref as Mould_No,max(pageno) as Mnt_life,sum(prodn) as shots_utilised,max(pageno)-(sum(prodn)-sum(maint)) as Bal_mainT_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as maint,0 as prodn,to_number(pageno) as pageno from typegrp where branchcd='" + frm_mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num1,0 as prodn,0 as pageno from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM05' and a.date1 " + DateRange + " union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,nvl(noups,0)*nvl(fm_fact,0) as totp,0 as pageno from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and vchdate " + DateRange + ") group by acref";

                                mq4 = "select distinct mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'dd/mm/yyyy') as vchdate from wb_maint where branchcd='" + frm_mbr + "' and type='MM05' ) group by mould";

                                mq1 = "SELECT b.cpartno as acref,to_char(b.date1,'dd/mm/yyyy') as comm_dt, trim(b.num13) as first_hm, to_number(trim(b.col12)) as op, b.col1 AS MOULD_CODE,b.col4 AS MOULD_NAME,to_number(b.col15) AS HM_life,B.NUM5 AS ALERT,TRIM(B.ACODE) AS ACODE,TRIM(C.ANAME) AS PARTY,TRIM(B.ICODE) AS ICODE,b.col8 as last_hm_dt  FROM  WB_MASTER B ,FAMST C WHERE  TRIM(B.ACODE)=TRIM(C.ACODE) AND B.BRANCHCD='" + frm_mbr + "' AND B.ID='MM01' AND NVL(B.COL2,'-')!='Y' order by mould_code"; // b.col1='0134'
                                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1); //master picking  dt
                            }
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                            dt2 = new DataTable();
                            dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);

                            dt4 = new DataTable();
                            dt4 = fgen.getdata(frm_qstr, frm_cocd, mq4);//maintenenace checking data
                            for (int i = 0; i < dt.Rows.Count; i++)
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
                                sg1_dr["sg1_f1"] = dt.Rows[i]["code"].ToString().Trim();
                                sg1_dr["sg1_f2"] = dt.Rows[i]["mould"].ToString().Trim();
                                sg1_dr["sg1_f3"] = dt.Rows[i]["mould_no"].ToString().Trim();
                                if (frm_formID == "F75150")
                                {
                                    mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dt.Rows[i]["code"].ToString().Trim() + "'", "vchdate");
                                    mq6 = mq5;
                                    if (mq5.Trim().Length <= 1)
                                    {
                                        mq5 = dt.Rows[i]["col7"].ToString().Trim();
                                        mq6 = mq5;
                                        if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                                        {
                                        }
                                        else
                                        {
                                            mq5 = mq0;
                                        }
                                    }
                                    //mq7 = fgen.seek_iname(frm_qstr, frm_cocd, " Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                    mq7 = fgen.seek_iname(frm_qstr, frm_cocd, " Select trim(pvchnum) as Acref ,sum(nvl(noups,0)*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                    //sg1_dr["sg1_t1"] = mq6;
                                    //sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["PM_life"].ToString().Trim()) - fgen.make_double(mq7);
                                    sg1_dr["sg1_t1"] = fgen.make_double(dt.Rows[i]["PM_life"].ToString().Trim()) - fgen.make_double(mq7);
                                    sg1_dr["sg1_t2"] = mq6;
                                    sg1_dr["sg1_f4"] = dt.Rows[i]["PM_life"].ToString().Trim();
                                }
                                else
                                {
                                    mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dt.Rows[i]["code"].ToString().Trim() + "'", "vchdate");
                                    mq6 = mq5;
                                    mq8 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "hm_life");
                                    if (mq5.Trim().Length <= 1)
                                    {
                                        mq5 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "last_hm_dt");
                                        mq6 = mq5;
                                        if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                                        {
                                        }
                                        else
                                        {
                                            mq5 = mq0;
                                        }
                                        if (Convert.ToDateTime(mq5) == Convert.ToDateTime(fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "comm_dt")))
                                        {
                                            mq8 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "first_hm");
                                        }
                                        else
                                        {
                                        }
                                    }
                                    //mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                    mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum(nvl(noups,0)*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                    sg1_dr["sg1_t1"] = fgen.make_double(mq8) - fgen.make_double(mq7);
                                    sg1_dr["sg1_t2"] = mq6;
                                    sg1_dr["sg1_f4"] = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "hm_life");
                                }
                                //sg1_dr["sg1_f4"] = fgen.make_double(fgen.seek_iname_dt(dt2, "MOULD_NO='" + dt.Rows[i]["MOULD_NO"].ToString().Trim() + "'", "MNT_LIFE"));
                                //sg1_dr["sg1_t1"] = fgen.make_double(fgen.seek_iname_dt(dt2, "MOULD_NO='" + dt.Rows[i]["MOULD_NO"].ToString().Trim() + "'", "Bal_mainT_life"));
                                if (frm_formID == "F75150")
                                {
                                    sg1_dr["sg1_t4"] = fgen.make_double(dt.Rows[i]["PM_FREQ_AS_PER_MONTH"].ToString().Trim());
                                }
                                else
                                {
                                    sg1_dr["sg1_t4"] = fgen.make_double(dt.Rows[i]["HM_FREQ_AS_PER_MONTH"].ToString().Trim());
                                }
                                if (frm_formID == "F75150")
                                {
                                    date1 = Convert.ToDateTime(sg1_dr["sg1_t2"]).AddMonths(fgen.make_int(dt.Rows[i]["PM_FREQ_AS_PER_MONTH"].ToString().Trim()));
                                }
                                else
                                {
                                    date1 = Convert.ToDateTime(sg1_dr["sg1_t2"]).AddMonths(fgen.make_int(dt.Rows[i]["HM_FREQ_AS_PER_MONTH"].ToString().Trim()));
                                }
                                sg1_dr["sg1_t6"] = Convert.ToDateTime(date1).AddDays(-1).ToString("yyyy-MM-dd");
                                sg1_dr["sg1_t7"] = "DUE";
                                sg1_dt.Rows.Add(sg1_dr);
                            }
                        }
                        else if (frm_formID == "F75153")
                        {
                            dt2 = new DataTable();
                            dt2 = fgen.getdata(frm_qstr, frm_cocd, "select irate as rate,icode,vchdate,vchnum,to_char(vchdate,'yyyymmdd')||trim(vchnum) as vdd from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and type not in ('04','08') and icode in (" + col1 + ")  order by vdd desc");
                            rate = "0";
                            SQuery = "select icode as item_code,iname as item_name,unit,(case when nvl(iqd,0)=0 then irate else iqd end) as rate from item where trim(icode) in (" + col1 + ") order by item_name";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            for (int i = 0; i < dt.Rows.Count; i++)
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
                                sg1_dr["sg1_f1"] = dt.Rows[i]["item_code"].ToString().Trim();
                                sg1_dr["sg1_f2"] = dt.Rows[i]["item_name"].ToString().Trim();
                                sg1_dr["sg1_f3"] = dt.Rows[i]["unit"].ToString().Trim();
                                if (dt2.Rows.Count > 0)
                                {
                                    rate = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["item_code"].ToString().Trim() + "'", "rate");
                                }
                                if (rate == "0")
                                {
                                    rate = dt.Rows[i]["rate"].ToString().Trim();
                                }
                                sg1_dr["sg1_t2"] = rate;
                                sg1_dt.Rows.Add(sg1_dr);
                            }
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();

                    //if (dt.Rows.Count > 0) dt.Dispose();

                    if (sg1_dt.Rows.Count > 0) sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt2 = new DataTable(); dt1 = new DataTable();
                    if (frm_formID == "F75150" || frm_formID == "F75155")
                    {
                        frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                        if (Convert.ToInt32(txtlbl4.Text) > 3 && Convert.ToInt32(txtlbl4.Text) <= 12)
                        {

                        }
                        else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                        if (frm_formID == "F75150")
                        {
                            SQuery = "select b.name AS MOULD,b.acref AS MOULD_no,a.branchcd, sum(a.PM_FREQ_AS_PER_MONTH) as PM_FREQ_AS_PER_MONTH,b.provision as pm_life, a.CODE,max(to_char(a.col7,'dd/mm/yyyy')) as col7 from (select a.branchcd as branchcd, trim(a.col1) AS CODE,a.num12 AS PM_FREQ_AS_PER_MONTH,to_date(a.col7,'dd/mm/yyyy') as col7 from wb_master a where a.branchcd='" + frm_mbr + "' and a.id='MM01' AND NVL(A.COL2,'-')!='Y' union all select a.branchcd as branchcd,trim(a.col1) AS CODE,0 AS PM_FREQ_AS_PER_MONTH,to_date(to_char(a.date1,'dd/mm/yyyy'),'dd/mm/yyyy') as col7 from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM04'  ) a ,typegrp b where trim(a.code)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and b.id='MM' and code='" + col1 + "' group by b.name,b.acref,a.branchcd,b.provision,a.CODE";

                            mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + frm_mbr + "'", "OPT_START");// check date for mm start date
                            DateRange = "Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1";
                            SQuery1 = "Select max(Name) as Mould_Name ,Acref as Mould_No,max(provision) as Mnt_life,sum(prodn) as shots_utilised,max(provision)-(sum(prodn)-sum(maint)) as Bal_mainT_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as maint,0 as prodn,to_number(provision) as provision from typegrp where branchcd='" + frm_mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num1,0 as prodn,0 as provision from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM04' and a.date1 Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0) as totp,0 as provision from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and vchdate " + DateRange + ") group by acref ";

                            mq4 = "select distinct mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'dd/mm/yyyy') as vchdate from wb_maint where branchcd='" + frm_mbr + "' and type='MM04' ) group by mould";
                        }
                        else
                        {
                            SQuery = "select b.name AS MOULD,b.acref AS MOULD_no,a.branchcd, sum(a.HM_FREQ_AS_PER_MONTH) as HM_FREQ_AS_PER_MONTH,b.pageno as HM_life,a.CODE,max(to_char(a.col8,'dd/mm/yyyy')) as col8 from (select a.branchcd as branchcd, trim(a.col1) AS CODE,a.num15 AS HM_FREQ_AS_PER_MONTH,to_date(a.col8,'dd/mm/yyyy') as col8 from wb_master a where a.branchcd='" + frm_mbr + "' and a.id='MM01' AND NVL(A.COL2,'-')!='Y' union all select a.branchcd as branchcd,trim(a.col1) AS CODE,0 AS HM_FREQ_AS_PER_MONTH,to_date(to_char(a.date1,'dd/mm/yyyy'),'dd/mm/yyyy') as col8 from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM05'  ) a ,typegrp b where trim(a.code)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and b.id='MM' and code='" + col1 + "' group by b.name,b.acref,a.branchcd,b.pageno,a.CODE";

                            mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_START FROM FIN_RSYS_OPT_PW WHERE OPT_ID='W1078' and branchcd='" + frm_mbr + "'", "OPT_START");// check date for mm start date
                            DateRange = "Between to_date('" + mq0 + "','dd/mm/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1";
                            //SQuery1 = "Select max(Name) as Mould_Name ,Acref as Mould_No,max(pageno) as Mnt_life,sum(prodn) as shots_utilised,max(pageno)-(sum(prodn)-sum(maint)) as Bal_mainT_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as maint,0 as prodn,to_number(pageno) as pageno from typegrp where branchcd='" + frm_mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num1,0 as prodn,0 as pageno from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM05' and a.date1 " + DateRange + " union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,(nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0) as totp,0 as pageno from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and vchdate " + DateRange + ") group by acref";
                            SQuery1 = "Select max(Name) as Mould_Name ,Acref as Mould_No,max(pageno) as Mnt_life,sum(prodn) as shots_utilised,max(pageno)-(sum(prodn)-sum(maint)) as Bal_mainT_life from (Select distinct Name ,trim(Acref) as acref,to_number(lineno) as lineno ,0 as maint,0 as prodn,to_number(pageno) as pageno from typegrp where branchcd='" + frm_mbr + "' and id='MM'  union all Select null as name ,trim(a.cpartno) as acref,0 as mlife,a.num1,0 as prodn,0 as pageno from wb_maint a where a.branchcd='" + frm_mbr + "' and a.type='MM05' and a.date1 " + DateRange + " union all Select null as Name,trim(pvchnum) as Acref , 0 as Capa,0 as mlife,nvl(noups,0)*nvl(fm_fact,0) as totp,0 as pageno from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and vchdate " + DateRange + ") group by acref";

                            mq4 = "select distinct mould,max(vchdate) as vchdate from (select distinct col1 as mould ,to_char(date1,'dd/mm/yyyy') as vchdate from wb_maint where branchcd='" + frm_mbr + "' and type='MM05' ) group by mould";

                            mq1 = "SELECT b.cpartno as acref,to_char(b.date1,'dd/mm/yyyy') as comm_dt, trim(b.num13) as first_hm, to_number(trim(b.col12)) as op, b.col1 AS MOULD_CODE,b.col4 AS MOULD_NAME,to_number(b.col15) AS HM_life,B.NUM5 AS ALERT,TRIM(B.ACODE) AS ACODE,TRIM(C.ANAME) AS PARTY,TRIM(B.ICODE) AS ICODE,b.col8 as last_hm_dt  FROM  WB_MASTER B ,FAMST C WHERE  TRIM(B.ACODE)=TRIM(C.ACODE) AND B.BRANCHCD='" + frm_mbr + "' AND B.ID='MM01' AND NVL(B.COL2,'-')!='Y' order by mould_code"; // b.col1='0134'
                            dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1); //master picking  dt
                        }

                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);

                        dt4 = new DataTable();
                        dt4 = fgen.getdata(frm_qstr, frm_cocd, mq4);//maintenenace checking data
                        if (dt.Rows.Count > 0)
                        {
                            //********* Saving in Hidden Field 
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[0]["code"].ToString().Trim();
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[0]["mould"].ToString().Trim();
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[0]["mould_no"].ToString().Trim();


                            if (frm_formID == "F75150")
                            {
                                mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dt.Rows[i]["code"].ToString().Trim() + "'", "vchdate");
                                mq6 = mq5;
                                if (mq5.Trim().Length <= 1)
                                {
                                    mq5 = dt.Rows[i]["col7"].ToString().Trim();
                                    mq6 = mq5;
                                    if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                                    {
                                    }
                                    else
                                    {
                                        mq5 = mq0;
                                    }
                                }
                                //mq7 = fgen.seek_iname(frm_qstr, frm_cocd, " Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                mq7 = fgen.seek_iname(frm_qstr, frm_cocd, " Select trim(pvchnum) as Acref ,sum(nvl(noups,0)*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                //sg1_dr["sg1_t1"] = mq6;
                                //sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["PM_life"].ToString().Trim()) - fgen.make_double(mq7);
                                ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = (fgen.make_double(dt.Rows[i]["PM_life"].ToString().Trim()) - fgen.make_double(mq7)).ToString();
                                ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = mq6;
                                sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[i]["PM_life"].ToString().Trim();
                            }
                            else
                            {
                                mq5 = fgen.seek_iname_dt(dt4, "mould ='" + dt.Rows[i]["code"].ToString().Trim() + "'", "vchdate");
                                mq6 = mq5;
                                mq8 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "hm_life");
                                if (mq5.Trim().Length <= 1)
                                {
                                    mq5 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "last_hm_dt");
                                    mq6 = mq5;
                                    if (Convert.ToDateTime(mq5) > Convert.ToDateTime(mq0))
                                    {
                                    }
                                    else
                                    {
                                        mq5 = mq0;
                                    }
                                    if (Convert.ToDateTime(mq5) == Convert.ToDateTime(fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "comm_dt")))
                                    {
                                        mq8 = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "first_hm");
                                    }
                                    else
                                    {
                                    }
                                }
                                //mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                mq7 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(pvchnum) as Acref ,sum(nvl(noups,0)*nvl(fm_fact,0)) as totp from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and pvchnum='" + dt.Rows[i]["mould_no"].ToString().Trim() + "' and vchdate between to_date('" + mq5 + "','dd/MM/yyyy') and to_date('01/" + txtlbl4.Text + "/" + frm_myear + "','dd/mm/yyyy')-1 group by trim(pvchnum)", "totp"); ;
                                ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = (fgen.make_double(mq8) - fgen.make_double(mq7)).ToString();
                                ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = mq6;
                                sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgen.seek_iname_dt(dt1, "mould_code='" + dt.Rows[i]["code"].ToString().Trim() + "'", "hm_life");
                            }
                            if (frm_formID == "F75150")
                            {
                                ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = (fgen.make_double(dt.Rows[0]["PM_FREQ_AS_PER_MONTH"].ToString().Trim())).ToString();
                            }
                            else if (frm_formID == "F75155")
                            {
                                ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = (fgen.make_double(dt.Rows[0]["HM_FREQ_AS_PER_MONTH"].ToString().Trim())).ToString();
                            }
                            if (frm_formID == "F75150")
                            {
                                date1 = Convert.ToDateTime(dt.Rows[0]["col7"].ToString().Trim()).AddMonths(fgen.make_int(dt.Rows[0]["PM_FREQ_AS_PER_MONTH"].ToString().Trim()));
                            }
                            else if (frm_formID == "F75155")
                            {
                                date1 = Convert.ToDateTime(dt.Rows[0]["col8"].ToString().Trim()).AddMonths(fgen.make_int(dt.Rows[0]["HM_FREQ_AS_PER_MONTH"].ToString().Trim()));
                            }
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t6")).Text = Convert.ToDateTime(date1).AddDays(-1).ToString("yyyy-MM-dd");
                        }
                    }
                    else if (frm_formID == "F75153")
                    {
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, "select irate as rate,icode,vchdate,vchnum,to_char(vchdate,'yyyymmdd')||trim(vchnum) as vdd from ivoucher where branchcd='" + frm_mbr + "' and  type like '0%' and type not in ('04','08') and icode  ='" + col1 + "'  order by vdd desc");
                        rate = "0";
                        SQuery = "select icode as item_code,iname as item_name,unit,(case when nvl(iqd,0)=0 then irate else iqd end) as rate from item where trim(icode)  ='" + col1 + "' order by item_code";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        if (dt.Rows.Count > 0)
                        {
                            //********* Saving in Hidden Field 
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[0]["item_code"].ToString().Trim();
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[0]["item_name"].ToString().Trim();
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[0]["unit"].ToString().Trim();
                            //********* Saving in GridView Value
                            if (dt2.Rows.Count > 0)
                            {
                                rate = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["item_code"].ToString().Trim() + "'", "rate");
                            }
                            if (rate == "0")
                            {
                                rate = dt.Rows[i]["rate"].ToString().Trim();
                            }
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = rate;
                        }
                    }
                    setColHeadings();
                    break;

                case "SG1_ADD_MAC":
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
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString().Replace("&amp;", "&");
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        if (frm_formID == "F75152")
                        {
                            SQuery = "select  '01' as fstr,'01' as code,'Moulding' as machine_name from dual";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = dt.Rows[0]["machine_name"].ToString().Trim();
                            }
                        }
                        else
                        {
                            dt = new DataTable();
                            SQuery = "select trim(acode)||'/'||trim(srno) as fstr,mchname as Machine_Name,trim(acode)||'/'||trim(srno) as Machine_Code,mch_seq from pmaint where branchcd='" + frm_mbr + "' and type='10' and trim(acode)||'/'||trim(srno)='" + col1 + "' order by acode,srno";
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = dt.Rows[0]["machine_code"].ToString().Trim();
                                ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = dt.Rows[0]["machine_name"].ToString().Trim();
                            }
                        }
                    }
                    #endregion
                    setColHeadings();
                    break;

                case "SG1_ADD_SPR":
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
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString().Replace("&amp;", "&");
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        if (frm_formID == "F75153")
                        {
                            SQuery = "select  '01' as fstr,'01' as code,'drill' as spare from dual";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t8")).Text = dt.Rows[0]["spare"].ToString().Trim();
                            }
                        }
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
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.Trim();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim().Replace("&amp;", "&");
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        //if (edmode.Value == "Y")
                        //{
                        //    //sg1_dr["sg1_f1"] = "*" + sg1.Rows[-1 + Convert.ToInt32(hf1.Value.Trim())].Cells[13].Text.Trim();
                        //    sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        //}
                        //else
                        //{
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        // }
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
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
        todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        if (hffield.Value == "List")
        {
            if (frm_formID == "F75150" || frm_formID == "F75155")  // FOR MOULDING MAINT PLAN
            {
                if (frm_formID == "F75150")
                {
                    SQuery = "SELECT TRIM(A.VCHNUM) AS ENTRY_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRY_DATE,A.COL1 AS CODE,A.CPARTNO AS MOULD_CODE,B.NAME AS MOULD_NAME,A.COL3 AS PM_LIFE,A.COL4 AS PM_BAL_LIFE,TO_CHAR(A.DATE2,'DD/MM/YYYY') AS PM_LAST_DATE,A.RESULT AS REMARKS,A.OBSV1 AS FREQ_MONTH,TO_CHAR(A.DATE1,'dd/mm/yyyy') AS PLAN_DATE,a.ent_by,a.ent_Dt,to_char(a.vchdate,'yyyymmdd') as vdd FROM WB_MAINT A ,TYPEGRP B WHERE trim(a.COL1)=trim(b.TYPE1) and trim(a.branchcd)=trim(b.branchcd) AND B.ID='MM' AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE = '" + frm_vty + "' AND A.VCHDATE " + PrdRange + " order by vdd,ENTRY_NO,a.srno";
                }
                else
                {
                    SQuery = "SELECT TRIM(A.VCHNUM) AS ENTRY_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRY_DATE,A.COL1 AS CODE,A.CPARTNO AS MOULD_CODE,B.NAME AS MOULD_NAME,A.COL3 AS HM_LIFE,A.COL4 AS HM_BAL_LIFE,TO_CHAR(A.DATE2,'DD/MM/YYYY') AS HM_LAST_DATE,A.RESULT AS REMARKS,A.OBSV1 AS FREQ_MONTH,TO_CHAR(A.DATE1,'dd/mm/yyyy') AS PLAN_DATE,a.ent_by,a.ent_Dt,to_char(a.vchdate,'yyyymmdd') as vdd FROM WB_MAINT A ,TYPEGRP B WHERE trim(a.COL1)=trim(b.TYPE1) and trim(a.branchcd)=trim(b.branchcd) AND B.ID='MM' AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE = '" + frm_vty + "' AND A.VCHDATE " + PrdRange + " order by vdd,ENTRY_NO,a.srno";
                }
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            }
            else
            {
                SQuery = "SELECT a.vchnum as Entry_no, to_char(a.vchdate,'dd/mm/yyyy') as Entry_date,b.name as mould_name,a.col1 as code,a.cpartno as mould_Code,to_char(a.date1,'dd/mm/yyyy') as breakdown_date,a.col12 as breakdown_time,a.btchno as machine_code,a.title as machine_name,TO_CHAR(a.date2,'DD/MM/YYYY') as ok_date,a.col15 as ok_time,a.col13 as downtime_reason,a.icode,i.iname,i.unit,a.num1 as qty,a.num2 as rate,a.num3 as amt,a.remarks,a.ent_by,a.ent_Dt FROM WB_MAINT a left join item i on trim(a.icode)=trim(i.icode),typegrp b WHERE trim(a.col1)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and b.id='MM' and a.branchcd='" + frm_mbr + "' and a.TYPE='" + frm_vty + "' and a.vchdate " + PrdRange + " ORDER BY Entry_no";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            }
        }
        else if (hffield.Value == "Print_E")
        {
            if (frm_formID == "F75150")
            {
                PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                string header_n = "Mould Plan";
                SQuery = "";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_rptlevel(header_n + "For the Period" + fromdt + "to" + todt, frm_qstr);

            }
            else
            {
                PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                string header_n = "Mould Breakdown";
                SQuery = "";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_rptlevel(header_n + "For the Period " + fromdt + "to" + todt, frm_qstr);
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
                        Checked_ok = "N"; btnsave.Disabled = false;
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                    }
                }
            }

            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N"; btnsave.Disabled = false;
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
                if (Checked_ok == "Y")
                {
                    try
                    {
                        oDS = new DataSet();
                        oporow = null;
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";

                        if (frm_formID == "F75153")
                        {
                            save_fun2();
                        }
                        else if (frm_formID == "F75150" || frm_formID == "F75155" || frm_formID == "F75101")
                        {
                            save_fun();
                        }

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
                            if (frm_formID == "F75153")
                            {
                                if (sg1.Rows.Count > 1)
                                {
                                    for (i = 0; i < sg1.Rows.Count - 0; i++)
                                    {
                                        if (sg1.Rows[i].Cells[14].Text.Trim().Length > 1)
                                        {
                                            save_it = "Y";
                                        }
                                    }
                                }
                                else
                                {
                                    save_it = "Y"; // ITEM SELECTION NOT COMPULSORY IN OK TO PRODUCTION
                                }
                            }
                            else if (frm_formID == "F75150" || frm_formID == "F75155" || frm_formID == "F75101")
                            {
                                for (i = 0; i < sg1.Rows.Count - 0; i++)
                                {
                                    if (sg1.Rows[i].Cells[14].Text.Trim().Length > 1)
                                    {
                                        save_it = "Y";
                                    }
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

                        if (frm_formID == "F75153")
                        {
                            save_fun2();
                        }
                        else if (frm_formID == "F75150" || frm_formID == "F75155" || frm_formID == "F75101")
                        {
                            save_fun();
                        }

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + frm_vnum + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
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
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); setColHeadings();
                    }
                    catch (Exception ex)
                    {
                        btnsave.Disabled = false;
                        fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N";
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
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        string alr_done = "";

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
                    alr_done = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t7")).Text;
                    if (alr_done.Length > 1)
                    {
                        fgen.msg("-", "AMSG", "Maintenance already done, cannot remove This Mould From The List");
                        return;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Mould From The List");
                }
                break;

            case "SG1_ROW_ADD":
                if (txtlbl4.Text.Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Select Month First");
                    return;
                }

                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    alr_done = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t7")).Text;
                    if (alr_done.Length > 1)
                    {
                        fgen.msg("-", "AMSG", "Maintenance already done, cannot change This Mould.");
                        return;
                    }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Mould", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Moulds", frm_qstr);
                }
                break;

            case "SG1_ADD_MAC":
                hf1.Value = index.ToString();
                if (sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text.Trim().Length > 1)
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ADD_MAC";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Machine", frm_qstr);
                }
                else
                {
                    fgen.msg("-", "AMSG", "Please Select Stage First!!!!");
                    return;
                }
                break;

            case "SG1_ADD_SPR":
                hf1.Value = index.ToString();
                if (sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text.Trim().Length > 1)
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //---------------------
                    hffield.Value = "SG1_ADD_SPR";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Machine", frm_qstr);
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
                break;
            case "SG2_ROW_ADD":
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
                break;
            case "SG3_ROW_ADD":
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {

        string last_entdt = "";
        last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
        if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
        {

            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            txtvchdate.Text = last_entdt;
            txtlbl2.Text = last_entdt;
            txtvchdate.Focus();
            return;
        }

        if (Convert.ToDateTime(txtvchdate.Text.Trim()) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text.Trim()) > Convert.ToDateTime(todt))
        {
            fgen.msg("-", "AMSG", "Date outside " + fromdt + " to " + todt + " is Not Allowed!!'13'Fill date for This Year Only");
            txtlbl7.Focus();
            return;
        }

        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);
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
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        if (frm_formID == "F75150" || frm_formID == "F75155")
        {
            frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
            if (Convert.ToInt32(txtlbl4.Text) > 3 && Convert.ToInt32(txtlbl4.Text) <= 12)
            {

            }
            else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
        }
        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            if (sg1.Rows[i].Cells[14].Text.Length > 1)
            {
                if (frm_formID == "F75150" || frm_formID == "F75155")  // for moulD maint Plan
                {
                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = frm_mbr;
                    oporow["TYPE"] = frm_vty;
                    oporow["vchnum"] = frm_vnum.Trim().ToUpper();
                    oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
                    oporow["icode"] = "-";
                    oporow["col1"] = sg1.Rows[i].Cells[13].Text.Trim().ToUpper();
                    oporow["COL2"] = "-";
                    oporow["col3"] = fgen.make_double(sg1.Rows[i].Cells[16].Text.Trim().ToUpper());
                    oporow["col4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper());
                    oporow["DATE2"] = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
                    oporow["RESULT"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper();
                    oporow["OBSV1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper();
                    oporow["OBSV3"] = "-";
                    oporow["obsv14"] = txtlbl4.Text.Trim().ToUpper();
                    oporow["obsv15"] = txtlbl4a.Text.Trim().ToUpper();
                    oporow["TITLE"] = "-";
                    oporow["BTCHNO"] = "-";
                    oporow["ACODE"] = "-";
                    oporow["CPARTNO"] = sg1.Rows[i].Cells[15].Text.Trim().ToUpper();
                    oporow["GRADE"] = "-";
                    oporow["SRNO"] = i + 1;
                    oporow["COL5"] = "-";
                    oporow["COL6"] = "-";
                    oporow["COL7"] = "-";
                    oporow["COL8"] = "-";
                    oporow["COL9"] = "-";
                    oporow["COL10"] = "-";
                    oporow["COL11"] = "-";
                    oporow["COL12"] = "-";
                    oporow["COL13"] = "-";
                    oporow["COL14"] = "-";
                    oporow["COL15"] = "-";
                    oporow["DATE1"] = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
                    oporow["OBSV2"] = txtlbl4.Text + "/" + frm_myear;
                    oporow["OBSV4"] = "-";
                    oporow["OBSV5"] = "-";
                    oporow["OBSV6"] = "-";
                    oporow["OBSV7"] = "-";
                    oporow["OBSV8"] = "-";
                    oporow["OBSV9"] = "-";
                    oporow["OBSV10"] = "-";
                    oporow["OBSV11"] = "-";
                    oporow["OBSV12"] = "-";
                    oporow["OBSV13"] = "-";
                    oporow["NUM1"] = 0;
                    oporow["NUM2"] = 0;
                    oporow["NUM3"] = 0;
                    oporow["NUM4"] = 0;
                    oporow["NUM5"] = 0;

                    oporow["REMARKS"] = "-";

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
                else if (frm_formID == "F75101") // FOR MAINT PLAN
                {
                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = frm_mbr;
                    oporow["TYPE"] = frm_vty;
                    oporow["vchnum"] = frm_vnum.Trim().ToUpper();
                    oporow["vchdate"] = txtvchdate.Text.Trim();
                    oporow["MCHNAME"] = sg1.Rows[i].Cells[13].Text.Trim().ToUpper();
                    oporow["MCHCODE"] = sg1.Rows[i].Cells[14].Text.Trim().ToUpper();
                    oporow["SPEC1"] = "-";
                    oporow["SPEC2"] = "-";
                    oporow["SPEC3"] = "-";
                    oporow["SPEC4"] = "-";
                    oporow["SPEC5"] = "-";
                    oporow["LOCN"] = "-";
                    oporow["MAINTBY"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
                    oporow["MAINTDT"] = Convert.ToDateTime(sg1.Rows[i].Cells[15].Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
                    oporow["MAINTMTH"] = 0;
                    oporow["REMARKS"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper();
                    oporow["SRNO"] = i + 1;
                    oporow["ACODE"] = txtlbl4.Text.Trim().ToUpper();
                    oporow["ICODE"] = "-";
                    oporow["PR_NO"] = "-";
                    oporow["PR_DT"] = vardate;
                    oporow["PO_NO"] = "-";
                    oporow["PO_DT"] = vardate;
                    oporow["FASSTNO"] = "-";
                    oporow["ITMREMARKS"] = "-";
                    oporow["MAINTAMT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper());
                    oporow["APPVEN"] = ((DropDownList)sg1.Rows[i].FindControl("dd1")).SelectedItem.Text.Trim().ToUpper().Replace("&NBSP;", "");
                    //((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().ToUpper();
                    oporow["EXP_DATE"] = "-";
                    oporow["MCH_SEQ"] = 0;
                    oporow["ESPL_TAG"] = "-";
                    oporow["TOOLUSED"] = 0;
                    oporow["NCAPA"] = 0;
                    oporow["WAR_INFO"] = "-";
                    oporow["WAR_DATE"] = "-";
                    oporow["AMC_INFO"] = "-";
                    oporow["AMC_DATE"] = "-";
                    oporow["AMC_DATE"] = "-";
                    oporow["OTH_INFO"] = "-";
                    oporow["CONV_MACH"] = "N";

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
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        i = 0;
        if (sg1.Rows.Count > 1)
        {
            for (i = 0; i < sg1.Rows.Count - 1; i++)
            {
                if (sg1.Rows[i].Cells[14].Text.Length > 1)
                {
                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = frm_mbr;
                    oporow["TYPE"] = frm_vty;
                    oporow["vchnum"] = frm_vnum.Trim().ToUpper();
                    oporow["vchdate"] = txtvchdate.Text.Trim();
                    oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim().ToUpper();
                    oporow["acode"] = "-";
                    oporow["col1"] = txtlbl4.Text.Trim().ToUpper();
                    oporow["col2"] = "-";
                    oporow["col3"] = "-";
                    oporow["col4"] = "-";
                    oporow["col5"] = "-";
                    oporow["col6"] = "-";
                    oporow["col7"] = "-";
                    oporow["col8"] = "-";
                    oporow["col9"] = "-";
                    oporow["col10"] = "-";
                    // mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr from wb_maint where branchcd='" + frm_mbr + "' and type='MM06' and trim(col1)='" + txtlbl4.Text.Trim() + "'","fstr");
                    //mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(a.mould_code ) as fstr,a.mould_code as code,b.name as mould_name,a.breakdown_date,max(a.btchno) As mc_code,max(a.title) As mc_name,trim(a.vchnum)||a.vchdate as entry from (select distinct vchnum as vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate , col1 as mould_code,to_char(date1,'dd/mm/yyyy') as breakdown_date,1 as qty,btchno,title from wb_maint where branchcd='" + frm_mbr + "' and type='MM06' and vchdate " + DateRange + " union all select distinct vchnum as vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate ,col1 as mould_code,to_char(date1,'dd/mm/yyyy')  as breakdown_date,-1 as qty,btchno,title from wb_maint where branchcd='" + frm_mbr + "' and type='MM07' and vchdate " + DateRange + ") a,typegrp b where trim(a.mould_code)=trim(b.type1) and b.branchcd='" + frm_mbr + "' and b.id='MM' and a.mould_code='" + txtlbl4.Text.Trim() + "' group by a.mould_code,b.name,a.breakdown_date,trim(a.vchnum),a.vchdate having sum(qty)>0 order by b.name", "entry");
                    oporow["col11"] = doc_addl.Value.Trim();
                    oporow["col12"] = txtlbl3.Text.Trim().ToUpper();
                    oporow["col13"] = txtlbl8.Text.Trim().ToUpper();
                    oporow["col14"] = vardate;
                    oporow["col15"] = txtlbl6.Text.Trim().ToUpper();
                    oporow["btchno"] = txtlbl7.Text.Trim().ToUpper();
                    oporow["title"] = txtlbl7a.Text.Trim().ToUpper();
                    oporow["DATE1"] = Convert.ToDateTime(txtlbl2.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
                    oporow["DATE2"] = Convert.ToDateTime(txtlbl5.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
                    oporow["RESULT"] = "-";
                    oporow["CPARTNO"] = txtCpartno.Text.Trim().ToUpper();
                    oporow["GRADE"] = "-";
                    oporow["SRNO"] = i + 1;
                    oporow["OBSV1"] = "-";
                    oporow["OBSV2"] = "-";
                    oporow["OBSV3"] = "-";
                    oporow["OBSV4"] = "-";
                    oporow["OBSV5"] = "-";
                    oporow["OBSV6"] = "-";
                    oporow["OBSV7"] = "-";
                    oporow["OBSV8"] = "-";
                    oporow["OBSV9"] = "-";
                    oporow["OBSV10"] = "-";
                    oporow["OBSV11"] = "-";
                    oporow["OBSV12"] = "-";
                    oporow["OBSV13"] = "-";
                    oporow["OBSV14"] = "-";
                    oporow["OBSV15"] = "-";
                    oporow["NUM1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper());
                    oporow["NUM2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper());
                    oporow["NUM3"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper());
                    oporow["NUM4"] = 0;
                    oporow["NUM5"] = 0;
                    if (txtrmk.Text.Trim().Length > 300)
                    {
                        oporow["REMARKS"] = txtrmk.Text.Trim().ToUpper().Substring(0, 299);
                    }
                    else
                    {
                        oporow["REMARKS"] = txtrmk.Text.Trim().ToUpper();
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
            }
        }
        else
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum.Trim().ToUpper();
            oporow["vchdate"] = txtvchdate.Text.Trim();
            oporow["icode"] = "-";
            oporow["acode"] = "-";
            oporow["col1"] = txtlbl4.Text.Trim().ToUpper();
            oporow["col2"] = "-";
            oporow["col3"] = "-";
            oporow["col4"] = "-";
            oporow["col5"] = "-";
            oporow["col6"] = "-";
            oporow["col7"] = "-";
            oporow["col8"] = "-";
            oporow["col9"] = "-";
            oporow["col10"] = "-";
            //mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(a.mould_code ) as fstr,a.mould_code as code,b.name as mould_name,a.breakdown_date,max(a.btchno) As mc_code,max(a.title) As mc_name,trim(a.vchnum)||a.vchdate as entry from (select distinct vchnum as vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate , col1 as mould_code,to_char(date1,'dd/mm/yyyy') as breakdown_date,1 as qty,btchno,title from wb_maint where branchcd='" + frm_mbr + "' and type='MM06' and vchdate " + DateRange + " union all select distinct vchnum as vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate ,col1 as mould_code,to_char(date1,'dd/mm/yyyy')  as breakdown_date,-1 as qty,btchno,title from wb_maint where branchcd='" + frm_mbr + "' and type='MM07' and vchdate " + DateRange + ") a,typegrp b where trim(a.mould_code)=trim(b.type1) and b.branchcd='" + frm_mbr + "' and b.id='MM' and a.mould_code='" + txtlbl4.Text.Trim() + "' group by a.mould_code,b.name,a.breakdown_date,trim(a.vchnum),a.vchdate having sum(qty)>0 order by b.name", "entry");
            oporow["col11"] = doc_addl.Value.Trim().ToUpper();
            oporow["col12"] = txtlbl3.Text.Trim().ToUpper();
            oporow["col13"] = txtlbl8.Text.Trim().ToUpper();
            oporow["col14"] = vardate;
            oporow["col15"] = txtlbl6.Text.Trim().ToUpper();
            oporow["btchno"] = txtlbl7.Text.Trim().ToUpper();
            oporow["title"] = txtlbl7a.Text.Trim().ToUpper();
            oporow["DATE1"] = Convert.ToDateTime(txtlbl2.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
            oporow["DATE2"] = Convert.ToDateTime(txtlbl5.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
            oporow["RESULT"] = "-";
            oporow["CPARTNO"] = txtCpartno.Text.Trim().ToUpper();
            oporow["GRADE"] = "-";
            oporow["SRNO"] = i + 1;
            oporow["OBSV1"] = "-";
            oporow["OBSV2"] = "-";
            oporow["OBSV3"] = "-";
            oporow["OBSV4"] = "-";
            oporow["OBSV5"] = "-";
            oporow["OBSV6"] = "-";
            oporow["OBSV7"] = "-";
            oporow["OBSV8"] = "-";
            oporow["OBSV9"] = "-";
            oporow["OBSV10"] = "-";
            oporow["OBSV11"] = "-";
            oporow["OBSV12"] = "-";
            oporow["OBSV13"] = "-";
            oporow["OBSV14"] = "-";
            oporow["OBSV15"] = "-";
            oporow["NUM1"] = 0;
            oporow["NUM2"] = 0;
            oporow["NUM3"] = 0;
            oporow["NUM4"] = 0;
            oporow["NUM5"] = 0;
            if (txtrmk.Text.Trim().Length > 300)
            {
                oporow["REMARKS"] = txtrmk.Text.Trim().ToUpper().Substring(0, 299);
            }
            else
            {
                oporow["REMARKS"] = txtrmk.Text.Trim().ToUpper();
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
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F75150":
                frm_vty = "MM02";
                break;
            case "F75155":
                frm_vty = "MM03";
                break;
            case "F75153":
                frm_vty = "MM07";
                break;
        }
    }
    //------------------------------------------------------------------------------------
    private void Cal()
    {
        double qty = 0, rate = 0, amt = 0;
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            qty = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
            rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text);
            amt += qty * rate;
        }
        txtlbl9.Text = amt.ToString();
    }
    //------------------------------------------------------------------------------------
    protected void sg1_SelectedIndexChanged(object sender, EventArgs e)
    {
        //var grid = (GridView)sender;
        //GridViewRow row = sg1.SelectedRow;
        //int rowIndex = grid.SelectedIndex;
        //int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        //if (selectedCellIndex < 0) selectedCellIndex = 0;
        //mq0 = sg1.HeaderRow.Cells[selectedCellIndex].Text.Replace("<br/>", " "); // dynamic heading
        //if (selectedCellIndex > 0) selectedCellIndex -= 1;
        //mq1 = row.Cells[1].Text.Trim();
        //mq2 = row.Cells[3].Text.Trim();
        //mq3 = "";
    }
    protected void sg1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        sg1.PageIndex = e.NewPageIndex;
        //fillGrid();
    }
}