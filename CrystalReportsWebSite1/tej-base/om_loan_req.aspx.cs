using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Oracle.ManagedDataAccess.Client;
using System.Net.Mail;
using System.IO;


public partial class om_loan_req : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, SQuery2, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", mq2, PDateRange, Arr_Month_Name = "";
    DataTable dt, dt2, dt3, dt4, dt5; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0; string mq0 = "";
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok; string FileName = "";
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_tabname1, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
    string empid = "";
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
            // frm_uname = "E00020001";//hardcode for testing
            if (frm_uname.ToUpper().Trim().Substring(0, 1) == "E")
            {
                btnCocd.Visible = false;
            }
            if (!Page.IsPostBack)
            {
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
        //tab1.Visible = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void set_Val()
    {
        frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        Prg_Id = frm_formID;
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        frm_vty = "01";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "01");   

        switch (frm_formID)
        {
            case "F82705":// Loan Request
            case "F85127":
                lblheader.Text = "Loan Request";
                frm_tabname = "payloan";
                lblno.InnerText = "Loan_Vch_No";
                lbldt.InnerText = "Date";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
                DivAdvance.Visible = false;
                break;

            case "F85126":
                lblheader.Text = "Advance Payment";
                frm_tabname = "payadv";
                  lblno.InnerText = "Entry No";
                lbldt.InnerText = "Entry Date";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
                DivLoan.Visible = false; DivLoan1.Visible = false; Label10.Visible = false; txtstartdt.Visible = false;
                break;
        }
        switch (frm_formID)
        {
            case "F82705":
            case "F85127":
                //SQuery = "select a.vcnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " A WHERE  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and rownum<50 ORDER BY vdd desc,a.VCHNUM desc";
                SQuery = "select a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.empcode,b.name as employee,a.grade,nvl(a.dramt,0) as amount,nvl(a.cramt,0) as No_of_Installment,nvl(instamt,0) as Monthly_installment,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,to_char(a.vchdate,'yyyymmdd') as vdd,A.APP_BY,TO_CHAR(a.app_dt,'dd/mm/yyyy') as app_dt from payloan A,EMPMAS B  WHERE trim(a.branchcd)||trim(a.empcode)||trim(a.grade)=trim(b.branchcd)||trim(b.empcode)||trim(b.grade) and  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " and rownum<50 ORDER BY vdd desc,a.VCHNUM desc";
                break;

            case "F85126":
                SQuery = "select a.vchnum as Entry_No,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Dt,a.grade,a.empcode,b.name as employee,nvl(a.dramt,0) as amount,nvl(a.cramt,0) as No_of_Installment,nvl(instamt,0) as Monthly_installment,a.surebycd,a.surebynm,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " A,EMPMAS B WHERE trim(a.branchcd)||trim(a.empcode)||trim(a.grade)=trim(b.branchcd)||trim(b.empcode)||trim(b.grade) and  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " ORDER BY vdd desc,a.VCHNUM desc";
                break;
        }
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg1.DataSource = dt;
        sg1.DataBind();
    }

    protected void btncancel_ServerClick(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();
        sg1_dt = new DataTable();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        ViewState["sg1"] = null;
        setColHeadings();
    }

    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false; btnprint.Disabled = false;
        //btnacode.Enabled = false; btnitem.Enabled = false;
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
        // btnacode.Enabled = true; btnitem.Enabled = true;
    }

    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }

    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }

    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
    }

    public void cal()
    {
        int a, b, c;
        a = Convert.ToInt32(txtamt.Text); //amount
        b = Convert.ToInt32(txtinst.Text); //installment p/m
        if (b > 0)
        {
            c = a / b;
            txtamt1.Text = Convert.ToString(c);
        }
    }

    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_formID, frm_formID);
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
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }

    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + "", 6, "VCH");
        txtvchnew.Text = frm_vnum;
        txtdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        if (frm_uname.ToUpper().Trim().Substring(0, 1) == "E")
        {
            empid = frm_uname.Split('E')[1];
            SQuery = "select empcode,name,deptt_text,deptt,grade,to_char(dtjoin,'dd/mm/yyyy') as joindt,sum(nvl(er1,0)+nvl(er2,0)+nvl(er3,0)+nvl(er4,0)+nvl(er5,0)+nvl(er6,0)+nvl(er7,0)+nvl(er8,0)+nvl(er9,0)+nvl(er10,0)) as sal from empmas where trim(branchcd)||trim(empcode)='" + empid + "' group by empcode,name,to_char(dtjoin,'dd/mm/yyyy'),deptt_text,deptt,grade order by empcode";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count > 0)
            {
                txtcode.Text = dt.Rows[0]["empcode"].ToString().Trim();
                txtname.Text = dt.Rows[0]["name"].ToString().ToUpper().Trim();
                txtreason.Text = dt.Rows[0]["deptt_text"].ToString().Trim();
                txtjoindt.Text = dt.Rows[0]["joindt"].ToString().Trim();
                txtgrade.Text = dt.Rows[0]["grade"].ToString().Trim();
                txtsalry.Text = dt.Rows[0]["sal"].ToString().Trim();
                deptt.Value = dt.Rows[0]["deptt"].ToString().Trim(); //hidden field for save deptt code
            }
        }
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        disablectrl(); btnCocd.Enabled = true; btnCocd.Focus();
        fgen.EnableForm(this.Controls);
        #endregion
    }

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

    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        if (txtcode.Text == "" || txtcode.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Enter Employee Name First!!"); return;
        }
        if (txtamt.Text == "" || txtamt.Text == "-" || txtamt.Text == "0")
        {
            fgen.msg("-", "AMSG", "Please Enter Amount"); return;
        }
        if (txtinst.Text == "" || txtinst.Text == "-" || txtinst.Text == "0")
        {
            fgen.msg("-", "AMSG", "Please Enter No of Installment/Monthly"); return;
        }
        if (frm_formID == "F85127")
        {
            if (txtrunningloan.Text == "" || txtrunningloan.Text == "-")
            {
                fgen.msg("-", "AMSG", "Please Enter YES/NO in any Running Loan"); return;
            }
            if (txtrunningloan.Text == "YES" || txtrunningloan.Text == "yes" || txtrunningloan.Text == "y" || txtrunningloan.Text == "Y")
            {
                if (txtosamt.Text == "" || txtosamt.Text == "0" || txtosamt.Text == "-")
                {
                    fgen.msg("-", "AMSG", "Please Enter Value in OutStanding Amount "); return;
                }
            }
            if (txtstartdt.Text == "" || txtstartdt.Text == "-")
            {
                fgen.msg("-", "AMSG", "Please fill Installment Start Date "); return;
            }
        }
        fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
        return;
    }

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

    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Month", frm_qstr);
    }

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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(4, 16) + "'");
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(4, 16) + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(4, 6), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(10, 10), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(4, 6) + "");
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

                case "EMP_CD":
                    if (col1.Length <= 1) return;
                    SQuery = "select empcode,name,deptt_text,deptt,grade,to_char(dtjoin,'dd/mm/yyyy') as joindt,sum(nvl(er1,0)+nvl(er2,0)+nvl(er3,0)+nvl(er4,0)+nvl(er5,0)+nvl(er6,0)+nvl(er7,0)+nvl(er8,0)+nvl(er9,0)+nvl(er10,0)) as sal from empmas where branchcd='" + frm_mbr + "' and trim(branchcd)||trim(empcode)||trim(grade)='" + col1 + "' group by empcode,name,to_char(dtjoin,'dd/mm/yyyy'),deptt_text,deptt,grade order by empcode";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtcode.Text = dt.Rows[0]["empcode"].ToString().Trim();
                        txtname.Text = dt.Rows[0]["name"].ToString().ToUpper().Trim();
                        txtreason.Text = dt.Rows[0]["deptt_text"].ToString().Trim();
                        txtjoindt.Text = dt.Rows[0]["joindt"].ToString().Trim();
                        txtgrade.Text = dt.Rows[0]["grade"].ToString().Trim();
                        txtsalry.Text = dt.Rows[0]["sal"].ToString().Trim();
                        deptt.Value = dt.Rows[0]["deptt"].ToString().Trim(); //hidden field for save deptt code
                    }
                    if (frm_formID == "F85127")
                    {
                        txtamt.Focus();
                    }
                    else
                    {
                        btnSurety.Focus();
                    }
                    break;

                case "Edit_E":
                    #region
                    if (col1.Length <= 1) return;
                    clearctrl();
                    dt = new DataTable();
                    if (frm_formID == "F85127")
                    {
                        SQuery = "select a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.empcode,b.name,a.grade,to_char(b.dtjoin,'dd/mm/yyyy') as joindt,to_char(a.inst_st_dt,'dd/mm/yyyy') as inst_Dt,a.deptt,nvl(dramt,0) as amt,nvl(a.cramt,0) as inst_mth,nvl(os_amt,0) as os_Amt,nvl(a.INSTAMT,0) as instamt,nvl(a.CURRSAL,0) as salry,a.remark,a.cur_loan,a.ent_by,a.ent_Dt,a.edt_by,to_char(a.edt_dt,'dd/mm/yyyy') as edt_Dt  from " + frm_tabname + " a,empmas b where trim(a.branchcd)||trim(a.empcode)||trim(a.grade)=trim(b.branchcd)||trim(b.empcode)||trim(b.grade) and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' and a.ent_by='" + frm_uname.Trim() + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtvchnew.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                            txtdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString()).ToString("dd/MM/yyyy").Trim();
                            txtcode.Text = dt.Rows[0]["empcode"].ToString().Trim();
                            txtname.Text = dt.Rows[0]["name"].ToString().Trim();
                            txtreason.Text = dt.Rows[0]["deptt"].ToString().Trim();
                            txtjoindt.Text = dt.Rows[0]["joindt"].ToString().Trim();
                            txtamt.Text = dt.Rows[0]["AMT"].ToString().Trim();
                            txtinst.Text = dt.Rows[0]["inst_mth"].ToString().Trim();
                            txtamt1.Text = dt.Rows[0]["instamt"].ToString().Trim();
                            txtstartdt.Text = dt.Rows[0]["inst_Dt"].ToString().Trim();
                            txtrunningloan.Text = dt.Rows[0]["cur_loan"].ToString().Trim();
                            txtosamt.Text = dt.Rows[0]["os_amt"].ToString().Trim();
                            txtrmk.Text = dt.Rows[0]["remark"].ToString().Trim();
                            txtgrade.Text = dt.Rows[0]["grade"].ToString().Trim();
                            txtsalry.Text = dt.Rows[0]["salry"].ToString().Trim();
                            edmode.Value = "Y"; ViewState["entby"] = dt.Rows[0]["ent_by"].ToString(); ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                            fgen.EnableForm(this.Controls); disablectrl();
                        }
                    }
                    else
                    {
                        SQuery = "select a.*,b.name,to_char(b.dtjoin,'dd/mm/yyyy') as joindt from " + frm_tabname + " a,empmas b where trim(a.branchcd)||trim(a.empcode)||trim(a.grade)=trim(b.branchcd)||trim(b.empcode)||trim(b.grade) and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtvchnew.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                            txtdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString()).ToString("dd/MM/yyyy").Trim();
                            txtcode.Text = dt.Rows[0]["empcode"].ToString().Trim();
                            txtname.Text = dt.Rows[0]["name"].ToString().Trim();
                            txtreason.Text = dt.Rows[0]["deptt"].ToString().Trim();
                            txtjoindt.Text = dt.Rows[0]["joindt"].ToString().Trim();
                            txtamt.Text = dt.Rows[0]["dramt"].ToString().Trim();
                            txtinst.Text = dt.Rows[0]["cramt"].ToString().Trim();
                            txtamt1.Text = dt.Rows[0]["instamt"].ToString().Trim();
                            txtSurety_Code.Text = dt.Rows[0]["surebycd"].ToString().Trim();
                            txtSurety.Text = dt.Rows[0]["surebynm"].ToString().Trim();
                            txtrmk.Text = dt.Rows[0]["remark"].ToString().Trim();
                            txtgrade.Text = dt.Rows[0]["grade"].ToString().Trim();
                            txtsalry.Text = dt.Rows[0]["currsal"].ToString().Trim();
                            edmode.Value = "Y"; ViewState["entby"] = dt.Rows[0]["ent_by"].ToString(); ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                            fgen.EnableForm(this.Controls); disablectrl();
                        }
                    }
                    #endregion
                    break;

                //case "Print_E":
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", col1); //fstr
                //        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F82705");
                //        fgen.fin_hrm_reps(frm_qstr);                   
                //    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    break;

                case "SURETY":
                    txtSurety_Code.Text = col2;
                    txtSurety.Text = col3;
                    txtamt.Focus();
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
            }            
        }
    }
   
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            if (frm_formID == "F85127")
            {
                SQuery = "select a.vchnum as Loan_Req_Entry_No,to_char(a.vchdate,'dd/mm/yyyy') as Loan_Req_Entry_Dt,a.empcode,b.name as employee,a.grade,nvl(a.dramt,0) as amount,nvl(a.cramt,0) as No_of_Installment,nvl(instamt,0) as Monthly_installment,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " A,EMPMAS B WHERE trim(a.branchcd)||trim(a.empcode)||trim(a.grade)=trim(b.branchcd)||trim(b.empcode)||trim(b.grade) and  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + " and a.ent_by='" + frm_uname.Trim() + "' ORDER BY vdd desc,a.VCHNUM desc";
            }
            else
            {
                SQuery = "select a.vchnum as Entry_No,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Dt,a.grade,a.empcode,b.name as employee,nvl(a.dramt,0) as amount,nvl(a.cramt,0) as No_of_Installment,nvl(instamt,0) as Monthly_installment,a.surebycd,a.surebynm,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " A,EMPMAS B WHERE trim(a.branchcd)||trim(a.empcode)||trim(a.grade)=trim(b.branchcd)||trim(b.empcode)||trim(b.grade) and  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + " ORDER BY vdd desc,a.VCHNUM desc";
            }
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            string last_entdt;
            //checks
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  ", "ldt");
            if (last_entdt == "0" || edmode.Value == "Y")
            {
            }
            else
            {
                if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtdate.Text.ToString()))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtdate.Text.ToString() + ",Please Check !!");
                }
            }
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtdate.Text.ToString() + " ,Please Check !!");
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
                        if (frm_formID == "F85127")
                        {
                            save_fun();
                        }
                        else
                        {
                            save_fun2();
                        }

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnew.Text.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            save_it = "Y";
                            if (save_it == "Y")
                            {
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        if (frm_formID == "F85127")
                        {
                            save_fun();
                        }
                        else
                        {
                            save_fun2();
                        }

                        if (edmode.Value == "Y")
                        {
                            string mycmd = "";
                            // if (edmode.Value == "Y") fgen.execute_cmd(frm_qstr, frm_cocd, "update wb_payloan set branchcd='DD' where branchcd='" + mbr + "' and type='01' and trim(vchnum)='" + txtvchnew.Text.Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + txtdate.Text.Trim() + "'");
                            mycmd = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/MM/yyyy')='" + frm_mbr + frm_vty + txtvchnew.Text.Trim() + txtdate.Text.Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mycmd);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnew.Text + " Updated Successfully");
                            string mycmd2 = "";
                            mycmd2 = "delete from " + frm_tabname + " where branchcd='DD' and trim(type)||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/MM/yyyy')='" + frm_vty + txtvchnew.Text.Trim() + txtdate.Text.Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mycmd2);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnew.Text + " Saved Successfully!!");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnew.Text + " " + txtdate.Text.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); set_Val();
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

    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();
        oporow["branchcd"] = frm_mbr;
        oporow["TYPE"] = frm_vty;
        oporow["vchnum"] = txtvchnew.Text.Trim();
        oporow["vchdate"] = txtdate.Text.Trim();
        oporow["EMPCODE"] = txtcode.Text.Trim();//empcode        
        oporow["DEPTT"] = deptt.Value.ToUpper(); //DEPARTMENT FIELD        
        oporow["GRADE"] = txtgrade.Text.Trim().ToUpper();
        oporow["DRAMT"] = fgen.make_double(txtamt.Text.Trim()); //number value....amount
        oporow["CRAMT"] = fgen.make_double(txtinst.Text.Trim()); //number....no of installemnt 
        oporow["INSTAMT"] = fgen.make_double(txtamt1.Text.Trim());//inst.amt p/mth
        oporow["inst_st_dt"] = Convert.ToDateTime(txtstartdt.Text).ToString("dd/MM/yyyy").Trim();
        oporow["cur_loan"] = txtrunningloan.Text.Trim().ToUpper();
        oporow["os_amt"] = fgen.make_double(txtosamt.Text.Trim());
        oporow["REMARK"] = txtrmk.Text.Trim().ToUpper();
        oporow["CURRSAL"] = fgen.make_double(txtsalry.Text.Trim());

        if (edmode.Value == "Y")
        {
            oporow["eNt_by"] = ViewState["entby"].ToString();
            oporow["eNt_dt"] = ViewState["entdt"];
            oporow["edt_by"] = frm_uname;
            oporow["edt_dt"] = vardate;
            oporow["app_by"] = "-";
            oporow["app_Dt"] = vardate;
        }
        else
        {
            oporow["eNt_by"] = frm_uname;
            oporow["eNt_dt"] = vardate;
            oporow["edt_by"] = "-";
            oporow["eDt_dt"] = vardate;
            oporow["app_by"] = "-";
            oporow["app_Dt"] = vardate;
        }
        oDS.Tables[0].Rows.Add(oporow);
    }

    void save_fun2()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();
        oporow["branchcd"] = frm_mbr;
        oporow["TYPE"] = frm_vty;
        oporow["vchnum"] = txtvchnew.Text.Trim();
        oporow["vchdate"] = txtdate.Text.Trim();
        oporow["EMPCODE"] = txtcode.Text.Trim();
        oporow["DEPTT"] = txtreason.Text.Trim().ToUpper();
        oporow["GRADE"] = txtgrade.Text.Trim().ToUpper();
        oporow["DRAMT"] = fgen.make_double(txtamt.Text.Trim());
        oporow["CRAMT"] = fgen.make_double(txtinst.Text.Trim());
        oporow["INSTAMT"] = fgen.make_double(txtamt1.Text.Trim());
        oporow["SUREBYCD"] = txtSurety_Code.Text.Trim().ToUpper();
        oporow["SUREBYNM"] = txtSurety.Text.Trim().ToUpper();
        oporow["REMARK"] = txtrmk.Text.Trim().ToUpper();
        oporow["CURRSAL"] = fgen.make_double(txtsalry.Text.Trim());

        if (edmode.Value == "Y")
        {
            oporow["eNt_by"] = ViewState["entby"].ToString();
            oporow["eNt_dt"] = ViewState["entdt"];
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
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "01");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }

    public void make_qry_4_popup()
    {
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        switch (btnval)
        {
            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "List":
                SQuery = "SELECT 'action taken' AS FSTR,'action taken' AS Task FROM DUAL UNION ALL SELECT 'no action taken' AS FSTR,'no action taken'  AS Task FROM DUAL";
                break;

            case "EMP_CD":
                SQuery = "select trim(branchcd)||trim(empcode)||trim(grade) as fstr,empcode,name,to_char(dtjoin,'dd/mm/yyyy') as joindt,deptt_text,grade from empmas where branchcd='" + frm_mbr + "' order by empcode";
                //select trim(branchcd)||trim(empcode)||trim(grade) as fstr,empcode,name,to_char(dtjoin,'dd/mm/yyyy') as joindt,deptt_text,grade,sum(nvl(er1,0)+nvl(er2,0)+nvl(er3,0)+nvl(er4,0)+nvl(er5,0)+nvl(er6,0)+nvl(er7,0)+nvl(er8,0)+nvl(er9,0)+nvl(er10,0)) as sal  from empmas where branchcd='00' group by empcode,name,to_char(dtjoin,'dd/mm/yyyy'),deptt_text,grade,branchcd order by empcode 
                break;

            case "SURETY":
                SQuery = "select trim(branchcd)||trim(empcode)||trim(grade) as fstr,empcode,name,to_char(dtjoin,'dd/mm/yyyy') as joindt,deptt_text,grade from empmas where branchcd='" + frm_mbr + "' and empcode!='" + txtcode.Text.Trim() + "' order by empcode";
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    if (frm_formID == "F85127")
                    {
                        col1 = "";
                        if (frm_ulvl != "0") col1 = " and upper(trim(ent_by))='" + frm_uname.Trim() + "'";
                        SQuery = "select trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.empcode,b.name as employee,a.grade,a.deptt,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " and nvl(trim(a.app_by),'-')='-' order by vdd desc,entry_no desc";                      
                    }
                    else
                    {
                        SQuery = "select trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.empcode,b.name as employee,a.grade,a.deptt,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,entry_no desc";
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

    protected void btnCocd_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "EMP_CD";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Employee", frm_qstr);
    }

    protected void txtrunningloan_TextChanged(object sender, EventArgs e)
    {
        if (txtrunningloan.Text.ToUpper() == "Y" || txtrunningloan.Text.ToUpper() == "YES")
        {
            txtosamt.ReadOnly = false;
        }
        if (txtrunningloan.Text.ToUpper() == "N" || txtrunningloan.Text.ToUpper() == "NO")
        {
            txtosamt.Text = "";
            txtosamt.ReadOnly = true;
        }
    }

    protected void btnSurety_Click(object sender, ImageClickEventArgs e)
    {
        if (txtcode.Text.Length > 1)
        {
            hffield.Value = "SURETY";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Surety By", frm_qstr);
        }
        else
        {
            fgen.msg("-", "AMSG", "Please Select Employee First");
        }
    }
}

//ALTER TABLE FINECPL.PAYLOAN ADD CUR_LOAN NUMBER(10,2);
// ALTER TABLE FINECPL.PAYLOAN ADD OS_AMT NUMBER(10,2);
// ALTER TABLE FINECPL.PAYLOAN ADD INST_ST_DT DATE;
//ALTER TABLE FINECPL.PAYLOAN ADD APP_BY  VARCHAR2(50);

