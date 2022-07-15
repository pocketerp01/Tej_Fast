﻿using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class Web_mach_mast : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string html_body = "";
    string Prg_Id, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
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
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                doc_addl.Value = "1";
                fgen.execute_cmd(frm_qstr, frm_cocd, "update typegrp set vchnum=lpad(Trim(type1),6,'0') where trim(nvl(vchnum,'-'))='-'");

                string chk_opt = "";
                doc_GST.Value = "Y";
                //GSt india
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2017'", "fstr");
                if (chk_opt == "N")
                {
                    doc_GST.Value = "N";
                }
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
            if (frm_ulvl != "0") btndel.Visible = false;
            //btnprint.Visible = false;
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
                /*
                ((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("readonly", "readonly");
                 * */
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
        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        tab1.Visible = true; //for list
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
        btnlist.Disabled = false; btnmachine.Enabled = false;
        btnprint.Disabled = false;
        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();

        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();
        sg4_add_blankrows();

        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
        sg4.DataSource = sg4_dt; sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true;
        btnedit.Disabled = true;
        btnsave.Disabled = false;
        btnlist.Disabled = true;
        btnmachine.Enabled = true;
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
        string tbl_id;
        tbl_id = "";
        doc_nf.Value = "VCHNUM";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "WB_MACH_COST";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        typePopup = "N";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        switch (Prg_Id)
        {
            case "F10205":
                tbl_id = "^G";//MASTER FOR SPPI FOR COSITNG SHEET
                lblheader.Text = "Web Machine Master";
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", tbl_id);
        dt = new DataTable();
        switch (Prg_Id)
        {
            case "F10205":
                dt = fgen.getdata(frm_qstr, frm_cocd, "select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,MCHCODE,MCHNAME,ENT_BY,TO_CHAR(ENT_dT,'DD/MM/YYYY') AS ENT_DT from " + frm_tabname + " where TYPE='" + tbl_id + "' AND VCHDATE " + DateRange + " order by vchnum");
                break;
            default:
                cond = "";
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1,Name,acref as param1,acref2 as param2,acref3 as param3,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,to_char(EDT_DT,'dd/mm/yyyy') as edt_dt from " + frm_tabname + " where BRANCHCD!='DD' AND id='" + tbl_id + "' " + cond + " order by type1 ");
                break;
        }
        sg5.DataSource = dt;
        sg5.DataBind();
        sg4.DataSource = null;
        sg4.DataBind();
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        SQuery = "";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        if (CSR.Length > 1) cond = " and trim(a.ccode)='" + cond + "'";
        switch (btnval)
        {
            case "MACHINE":
                SQuery = "select distinct trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(mchcode) as fstr,mchname,mchcode from pmaint where branchcd='" + frm_mbr + "' and type='10' order by fstr";
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                break;

            case "SG1_ROW_TAX":
                break;

            case "New":
                Type_Sel_query();
                break;

            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt,A.MCHCODE,A.MCHNAME,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt from " + frm_tabname + " a where  a.branchcd='" + frm_mbr + "' and trim(a.TYPE)='" + frm_vty + "' " + cond + " order by a." + doc_nf.Value + " desc";
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

            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
        
            if (CSR.Length > 1)
            {            
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND type='" + frm_vty + "'", 6, "VCH");

        txtvchnum.Value = frm_vnum;
        txtvchdate.Value = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
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

        sg2_dt = new DataTable();
        create_tab2();
        sg2_add_blankrows();
        sg2_add_blankrows();
        sg2_add_blankrows();
        sg2_add_blankrows();
        sg2_add_blankrows();
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        setColHeadings();
        ViewState["sg2"] = sg2_dt;

        sg3_dt = new DataTable();
        create_tab3();
        sg3_add_blankrows();
        sg3.DataSource = sg3_dt;
        sg3.DataBind();
        setColHeadings();
        ViewState["sg3"] = sg3_dt;

        //-------------------------------------------
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        SQuery = "Select nvl(a.obj_name,'-') as udf_name from udf_config a where trim(a.frm_name)='" + Prg_Id + "' ORDER BY a.srno";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        create_tab4();
        sg4_dr = null;
        if (dt.Rows.Count > 0)
        {
            for (i = 0; i < dt.Rows.Count; i++)
            {
                sg4_dr = sg4_dt.NewRow();
                sg4_dr["sg4_srno"] = sg4_dt.Rows.Count + 1;

                sg4_dr["sg4_t1"] = dt.Rows[i]["udf_name"].ToString().Trim();
                sg4_dt.Rows.Add(sg4_dr);
            }
        }
        sg4_add_blankrows();
        ViewState["sg4"] = sg4_dt;
        sg4.DataSource = sg4_dt;
        sg4.DataBind();
        dt.Dispose();
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Edit", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        Cal();
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }

        //if (frm_mbr != "00")
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please operate this Option from Plant Code 00 !!");
        //    return;
        //}       

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Value.ToString());
        if (dhd == 0) { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        if (txtcode.Value.Trim().Length < 1 || txtcode.Value.Trim() == "-")
        {
            fgen.msg("-", "AMSG", " Please Select Machine!!");
            return;
        }

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
    }
    //------------------------------------------------------------------------------------
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        if (Prg_Id == "F70421" || Prg_Id == "F70437")
        {
        }
        else
        {
            if (frm_mbr != "00")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please operate this Option from Plant Code 00 !!");
                return;
            }
        }

        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "Y")
        {
            clearctrl();
            set_Val();
            hffield.Value = "Del";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
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
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F10205":
                SQuery = "select vchnum as opt_no,to_char(vchdate,'dd/mm/yyyy') as vchdate,mchcode,mchname,nvl(MCH_COST,0) as machine_cost,nvl(YR_CONSDER,0) as year_consider,nvl(WRK_HR_PDAY,0) as working_hr_pday,nvl(DAY_WRK_PM,0) as days_worked_pmonth,nvl(TOT_MTH_YR,0) as total_months_pyear,nvl(TOT_HR,0) as total_hr,nvl(MCH_RT_PHR,0) as machine_Rate_Phr,nvl(OPER_SAL,0) as operator_salary,nvl(oper_sal_ph,0) as operator_sal_p_hr, ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt from " + frm_tabname + " where type='" + frm_vty + "' order by vchnum desc";
                break;
            default:
                SQuery = "select distinct a.type1 as Type_Code,a.Name as Type_Name,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt from " + frm_tabname + " a where a.branchcd='00' and trim(a.ID)='" + frm_vty + "'  order by a.type1";
                break;
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + "", frm_qstr);
        hffield.Value = "-";
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        string header_n = lblheader.Text;
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        //SQuery = "SELECT '" + lblheader.Text + "' as header,'" + lbl3.InnerText + "' as h1,'" + Label5.InnerText + "' as h2,'" + Label2.InnerText + "' as h3, '" + Label3.InnerText + "' as h4, '" + Label4.InnerText + "'as h5, '" + Label6.InnerText + "'as h6, A.TYPE1 AS CODE,A.NAME as name,nvl(a.num4,0) as df1 ,nvl(a.num5,0) as df2,nvl(a.num6,0) as df3, nvl(a.num7,0) as df4 FROM TYPEGRP A  WHERE trim(a.ID)='" + frm_vty + "'  ORDER BY A.TYPE1";
        SQuery = "SELECT '" + lblheader.Text + "' as header,'" + lbl3.InnerText + "' as h1,'" + Label5.InnerText + "' as h2,'" + Label2.Text + "' as h3, '" + Label3.Text + "' as h4, '" + Label4.Text + "'as h5, '" + Label6.InnerText + "'as h6,A.TYPE1 AS CODE,A.NAME as name,a.acref as extra,a.lineno,a.acref3 as other_ref,'-' as col4  FROM TYPEGRP A  WHERE trim(a.ID)='" + frm_vty + "'   ORDER BY A.TYPE1";
        if (Prg_Id == "F70177")
            SQuery = "SELECT '" + lblheader.Text + "' as header,'Entry No' as h1,'" + Label5.InnerText + "' as h2,'HSN/SAC' as h3, 'CGST %' as h4, 'SGST %' as h5, 'IGST %' as h6,A.TYPE1 AS CODE,A.NAME as name,a.acref as extra,to_char(a.num4,'999.99') lineno,to_char(a.num5,'999.99') as other_ref,to_char(a.num6,'999.99') as col4  FROM TYPEGRP A  WHERE trim(a.ID)='" + frm_vty + "'   ORDER BY A.TYPE1";

        fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "tgpopmst", "tgpopmst");

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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||trim(a.type)||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                clearctrl(); fgen.ResetForm(this.Controls); set_Val();
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
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);

                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;
                case "Edit":
                    if (col1 == "") return;
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);

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
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "" || col1 == "-") return;
                    clearctrl();
                    frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                    string mv_col;
                    mv_col = frm_mbr + frm_vty + col1;
                    SQuery = "Select a.*  from " + frm_tabname + " a where a.branchcd||trim(a.TYPE)||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + mv_col + "' ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txtvchnum.Value = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Value = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");
                        //oporow[doc_nf.Value] = frm_vnum;
                        //oporow[doc_df.Value] = txtvchdate.Value.Trim();
                        //oporow["vchnum"] = frm_vnum.Trim();
                        //oporow["vchdate"] = txtvchdate.Value.Trim();
                        txtcode.Value = dt.Rows[0]["MCHCODE"].ToString();
                        txtname.Value = dt.Rows[0]["MCHNAME"].ToString();
                        txtmch_cost.Value = dt.Rows[0]["MCH_COST"].ToString();
                        mch_Cost.Value = dt.Rows[0]["MCH_COST1"].ToString();
                        txt_yr_considered.Value = dt.Rows[0]["YR_CONSDER"].ToString();
                        txt_wrk_pday.Value = dt.Rows[0]["WRK_HR_PDAY"].ToString();
                        txt_day_wrked_pm.Value = dt.Rows[0]["DAY_WRK_PM"].ToString();
                        txt_tot_mth_pyear.Value = dt.Rows[0]["TOT_MTH_YR"].ToString();
                        txttot_hrs.Value = dt.Rows[0]["TOT_HR"].ToString();
                        txtmch_rate_ph.Value = dt.Rows[0]["MCH_RT_PHR"].ToString();
                        txtoper_sal.Value = dt.Rows[0]["OPER_SAL"].ToString();
                        txtno_of_impresion_mnt.Value = dt.Rows[0]["NO_IMP_PMNT"].ToString();
                        txtmax_runn_mtr_A.Value = dt.Rows[0]["MX_RMTR_PHR1"].ToString();
                        txtmax_runn_mtr_hr.Value = dt.Rows[0]["MX_RMTR_PHR"].ToString();
                        txtjob_time.Value = dt.Rows[0]["JOB_TIME"].ToString();
                        txtsetting_tym.Value = dt.Rows[0]["SET_TIME"].ToString();
                        txttot_job_time.Value = dt.Rows[0]["TOT_TIME_JOB"].ToString();
                        txtop_Sal_phr.Value = dt.Rows[0]["oper_sal_ph"].ToString();
                        txt_elect_use.Value = dt.Rows[0]["tot_ele_use"].ToString();
                        txt_elec_charge.Value = dt.Rows[0]["elce_chg_phr"].ToString();
                        txttot_mch_cost.Value = dt.Rows[0]["tot_mcost"].ToString();
                        
                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        //------------------------
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                        txtvchnum.Disabled = true;
                        txtvchdate.Disabled = true;
                        Cal();
                    }
                    #endregion
                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||trim(A.ID)||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;

                case "MACHINE":
                    if (col1.Length < 1 || col1 == "" || col1 == "0")
                    {
                        return;
                    }
                    else
                    {
                        string mhd = "";
                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select  distinct trim(mchcode) as fstr from WB_MACH_COST where branchcd='" + frm_mbr + "' and mchcode='" + col3 + "'", "fstr");
                        if (mhd == "0" || mhd == "")
                        {
                            SQuery = "select mchname,mchcode from pmaint where type='10' and trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(mchcode)='" + col1 + "'";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                txtcode.Value = dt.Rows[0]["mchcode"].ToString().Trim();
                                txtname.Value = dt.Rows[0]["mchname"].ToString().Trim();
                            }
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Master already created for this machine.Please select another machine!!"); return;
                        }
                    }
                    break;

                case "TYPECODE":
                    if (fgen.make_double(col1) <= 0)
                    {
                        fgen.msg("-", "AMSG", "Please Choose Code Which is Available !!");
                        return;
                    }
                    else
                    {
                        txtlbl4.Value = col2;
                    }

                    break;
                case "ACTGCODE":
                    if (col1.Length < 1 || col1 == "" || col1 == "0")
                    {
                        return;
                    }
                    else
                    {
                        if (col1.Contains("~"))
                        {
                            txtlbl4.Value = col1.Split('~')[0].ToString();
                            txtlbl2.Value = col1.Split('~')[1].ToString();
                            txtlbl5.Value = col1.Split('~')[2].ToString();
                            txtlbl6.Value = col1.Split('~')[3].ToString();
                        }
                        else
                        {
                            //txtlbl2.Value = col1;
                            //  txtlbl3.Value = col1;
                            //txtlbl4.Value = col3;
                        }
                    }
                    txtlbl7.Focus();
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    //txtlbl4.Text = col1;
                    //txtlbl4a.Text = col2;

                    //txtlbl5.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    //txtlbl6.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");

                    //btnlbl7.Focus();
                    break;


                case "TICODE":
                    if (col1.Length <= 0) return;
                    //txtlbl7.Text = col1;
                    //txtlbl7a.Text = col2;
                    //txtlbl2.Focus();
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
                case "SG1_ROW_TAX":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t16")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t12")).Focus();
                    break;
                case "SG1_ROW_DT":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    break;

                //case "sg1_Row_Tax_E":
                //    if (col1.Length <= 0) return;
                //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[27].Text = col1;
                //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[28].Text = col2;
                //    setColHeadings();
                //    break;
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
            SQuery = "SELECT a.Id,a.Type1,a.Name,a.Typedpt,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_Dtd,a.Edt_by,to_char(a.edt_Dt,'dd/mm/yyyy') as Edt_Dtd FROM " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "'  order by a.type1 ";
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
                        frm_vnum = txtvchnum.Value.Trim();
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
                            i = 0;
                            do
                            {
                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' ", 6, "vch");
                                pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Value.Trim()).ToString("yyyy-MM-dd"), "", frm_uname);
                                if (i > 20)
                                {
                                    fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' ", 6, "vch");
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
                    string ddl_fld1;
                    string ddl_fld2;
                    ddl_fld1 = frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    ddl_fld2 = frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    if (edmode.Value == "Y")
                    {
                        SQuery = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                    }
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                    if (edmode.Value == "Y")
                    {
                        fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Value.Trim() + " Updated Successfully");
                        SQuery = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                    }
                    else
                    {
                        if (save_it == "Y")
                        {
                            //html_body = html_body + "Please note your SRF No : " + frm_vnum + "<br>";
                            //html_body = html_body + "Finsys team will contact you in case of any further clarification required within next 3 working days. You can track your service request through SRF status also.<br>";
                            //html_body = html_body + "Always at your service, <br>";
                            //html_body = html_body + "Finsys support <br>";

                            //fgen.send_mail(frm_cocd, "Tejaxo ERP", txtlbl5.Value, "", "", "SRF : Query has been logged " + frm_vnum, html_body);

                            //fgen.msg("-", "AMSG", "SRF No " + frm_vnum + "'13'Finsys team will contact you in case of any further clarification required within next 3 working days. You can track your service request through SRF status also.");
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Value + " Saved Successfully");
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Data Not Saved");
                        }
                    }
                    #region Email Sending Function
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //html started                            
                    sb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
                    sb.Append("<h3>" + fgenCO.chk_co(frm_cocd) + "</h3>");
                    sb.Append("<br>Dear Sir/Mam,<br> This is to advise that the following " + lblheader.Text + " has been saved by " + frm_uname + ".<br><br>");

                    //table structure
                    sb.Append("<table border=1 cellspacing=1 cellpadding=1 style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; color: #474646'>");

                    sb.Append("<tr style='color: #FFFFFF; background-color: #0099FF; font-weight: 700; font-family: Arial, Helvetica, sans-serif'>" +
                    "<td><b>SubGrp Code</b></td><td><b>SubGrp Name</b></td><td><b>User Name</b></td><td><b>Activity Date</b></td><td><b>ID</b></td>");
                    //vipin
                    //foreach (GridViewRow gr in sg1.Rows)
                    //{
                    //    if (gr.Cells[13].Text.Trim().Length > 4)
                    //    {

                    sb.Append("<tr>");
                    sb.Append("<td>");
                    sb.Append(txtlbl4.Value.Trim());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(txtlbl2.Value.Trim());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(frm_uname);
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(vardate);
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(Prg_Id);
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    //    }
                    //}
                    sb.Append("</table></br>");

                    sb.Append("Thanks & Regards");
                    sb.Append("<h5>Note: This is an Auto generated Mail from Tejaxo ERP. The above details are to the best of information <br> and data available to the ERP System. For any discrepancy/ clarification kindly get in touch with the concerned official. </h5>");
                    sb.Append("</body></html>");
                    //send mail
                    string subj = "";
                    if (edmode.Value == "Y") subj = "Edited : ";
                    else subj = "New Entry : ";
                    //uncomment below line for shoot mail
                    //  fgen.send_Activity_mail(frm_qstr, frm_cocd, "Tejaxo ERP", frm_formID, subj + lblheader.Text + " #" + frm_vnum, sb.ToString(), frm_uname);                   
                    sb.Clear();
                    #endregion
                    set_Val();
                    fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                }
                catch (Exception ex)
                {
                    fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                    fgen.msg("-", "AMSG", ex.Message.ToString());
                    col1 = "N";
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
        sg1_dr["sg1_t3"] = "0";
        sg1_dr["sg1_t4"] = "0";
        sg1_dr["sg1_t5"] = "0";
        sg1_dr["sg1_t6"] = "0";
        sg1_dr["sg1_t7"] = "0";
        sg1_dr["sg1_t8"] = "0";
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
    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();


        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
        sg2_dr["sg2_t1"] = "-";
        sg2_dr["sg2_t2"] = "-";
        sg2_dt.Rows.Add(sg2_dr);
    }
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

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG1_RMV":

                break;
            case "SG1_ROW_TAX":

                break;
            case "SG1_ROW_DT":

                break;

            case "SG1_ROW_ADD":


                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg2.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
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

        if (txtvchnum.Value == "-")
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
    protected void sg4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg4.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "sg4_RMV":

                break;
            case "sg4_ROW_ADD":

                break;
        }
    }

    //------------------------------------------------------------------------------------


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

    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();
        ///==========
        oporow["BRANCHCD"] = frm_mbr;
        oporow["TYPE"] = frm_vty;
        oporow[doc_nf.Value] = frm_vnum;
        oporow[doc_df.Value] = txtvchdate.Value.Trim();
        oporow["vchnum"] = frm_vnum.Trim();
        oporow["vchdate"] = txtvchdate.Value.Trim();
        oporow["MCHCODE"] = txtcode.Value.Trim();
        oporow["MCHNAME"] = txtname.Value.Trim();
        oporow["MCH_COST"] = fgen.make_double(txtmch_cost.Value.Trim());
        oporow["MCH_COST1"] = fgen.make_double(mch_Cost.Value.Trim());
        oporow["YR_CONSDER"] = fgen.make_double(txt_yr_considered.Value.Trim());
        oporow["WRK_HR_PDAY"] = fgen.make_double(txt_wrk_pday.Value.Trim());
        oporow["DAY_WRK_PM"] = fgen.make_double(txt_day_wrked_pm.Value.Trim());
        oporow["TOT_MTH_YR"] = fgen.make_double(txt_tot_mth_pyear.Value.Trim());
        oporow["TOT_HR"] = fgen.make_double(txttot_hrs.Value.Trim());
        oporow["MCH_RT_PHR"] = fgen.make_double(txtmch_rate_ph.Value.Trim());
        //oporow["MX_RMTR_PHR1"] = fgen.make_double(txtmax_runn_mtr_A.Value.Trim());
        oporow["OPER_SAL"] = fgen.make_double(txtoper_sal.Value.Trim());
        oporow["NO_IMP_PMNT"] = fgen.make_double(txtno_of_impresion_mnt.Value.Trim());
        oporow["MX_RMTR_PHR"] = fgen.make_double(txtmax_runn_mtr_hr.Value.Trim());
        oporow["MX_RMTR_PHR1"] = fgen.make_double(txtmax_runn_mtr_A.Value.Trim());
        oporow["JOB_TIME"] = fgen.make_double(txtjob_time.Value.Trim());
        oporow["SET_TIME"] = fgen.make_double(txtsetting_tym.Value.Trim());
        oporow["TOT_TIME_JOB"] = fgen.make_double(txttot_job_time.Value.Trim());
        oporow["oper_sal_ph"] = fgen.make_double(txtop_Sal_phr.Value.Trim());
        oporow["tot_ele_use"] = fgen.make_double(txt_elect_use.Value.Trim());
        oporow["elce_chg_phr"] = fgen.make_double(txt_elec_charge.Value.Trim());
        oporow["tot_mcost"] = fgen.make_double(txttot_mch_cost.Value.Trim());

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

    void Acode_Sel_query()
    {

    }
    void Icode_Sel_query()
    {

    }

    void Type_Sel_query()
    {


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
            e.Row.Cells[0].Style["display"] = "none";
            sg4.HeaderRow.Cells[0].Style["display"] = "none";
            e.Row.Cells[1].Style["display"] = "none";
            sg4.HeaderRow.Cells[1].Style["display"] = "none";
        }
    }  
    protected void btnactg_Click(object sender, ImageClickEventArgs e)
    {
    }

    void Cal()
    {
        double ff12 = 0; double ff13 = 0; double ff14 = 0; double ff15 = 0; double ff16 = 0; double ff17 = 0; double ff18 = 0; double ff19 = 0;
        double ff20 = 0; double ff21 = 0; double ff22 = 0; double ff23 = 0; double ff24 = 0; double ff25 = 0; double ff26 = 0; double ff27 = 0; double tot_elc_chg = 0;
        double ff28 = 0; double ff29 = 0; double ff30 = 0; double elc_use = 0; double elc_chg = 0; double mch_cost = 0; double tot_mch_cost = 0; double elec = 0;
        ff12 = fgen.make_double(txt_yr_considered.Value.Trim());
        ff13 = fgen.make_double(txt_wrk_pday.Value.Trim());
        ff14 = fgen.make_double(txt_day_wrked_pm.Value.Trim());
        ff15 = fgen.make_double(txt_tot_mth_pyear.Value.Trim());
        ff16 = ff12 * ff13 * ff14 * ff15;
        txttot_hrs.Value = Convert.ToString(Math.Round(ff16, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        /////================machine rate formula  
        ff17 = fgen.make_double(txtmch_cost.Value.Trim());
        ff18 = ff17 / ff16;
        txtmch_rate_ph.Value = Convert.ToString(Math.Round(ff18, 6)).Replace("Infinity", "0").Replace("NaN", "0");
        ///////==================operator salary in hr       
        ff19 = fgen.make_double(txtoper_sal.Value.Trim());
        ff20 = ff19 / (ff13 * ff14);
        txtop_Sal_phr.Value = Convert.ToString(Math.Round(ff20, 6)).Replace("Infinity", "0").Replace("NaN", "0");
        ///============time for the job
        //isme formula hai/..........b30/b104..but b30 costing part ka field hai .so abi is field ko user entry kiya hai and jab costing form per 
        //work hoga tab Total Running meter used ko b104 se divide kr denge
        //==============total time for the job   
        ff21 = fgen.make_double(txtjob_time.Value.Trim());
        ff22 = fgen.make_double(txtsetting_tym.Value.Trim());
        ff23 = ff21 + ff22;///  (txtjob_time + txtsetting_tym)/60 *b15 ........b15 ko 1 mana hia qki ye b costing from ka filed hai
        txttot_job_time.Value = Convert.ToString(Math.Round(ff23, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        /////max running mtr formula for B      
        ff24 = fgen.make_double(txtmax_runn_mtr_A.Value.Trim());
        ff25 = fgen.make_double(txtno_of_impresion_mnt.Value.Trim());
        ff26 = ff24 * ff25;
        txtmax_runn_mtr_hr.Value = Convert.ToString(Math.Round(ff26, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        ////=============for final machine cost      mch_Cost                           
        ff27 = fgen.make_double(txttot_job_time.Value.Trim());
        ff28 = fgen.make_double(txtmch_rate_ph.Value.Trim());
        ff29 = fgen.make_double(txtop_Sal_phr.Value.Trim());
        ff30 = ((ff27 / 60) * ff28);
        mch_Cost.Value = Convert.ToString(Math.Round(ff30, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        //formula for elcetricity charge for 1 hr             
        elec = fgen.make_double(txt_elec_charge.Value.Trim());
        tot_elc_chg = elec * (ff27 / 60);
        txt_tot_elec_chg.Value = Convert.ToString(Math.Round(tot_elc_chg, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        //      //==================================================================     
        elc_use = fgen.make_double(txt_elect_use.Value.Trim());
        elc_chg = elc_use / (ff14 * ff13);
        txt_elec_charge.Value = Convert.ToString(Math.Round(elc_chg, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        //      //==================================================================        
        tot_mch_cost = tot_elc_chg + ff29 + ff30;
        txttot_mch_cost.Value = Convert.ToString(Math.Round(tot_mch_cost, 2)).Replace("Infinity", "0").Replace("NaN", "0");
    }
    protected void btnmachine_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MACHINE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Machine", frm_qstr);
    }
}

// ALTER TABLE FINSPPI.WB_MACH_COST ADD MCH_cOST1 NUMBER(12,2);