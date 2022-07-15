using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_fixed_asset_pur : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, col7, vardate, fromdt, todt, next_year, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;
    int ast_chk_flg = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string html_body = "";
    string Prg_Id, lbl1a_Text, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
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
                    lbl1a_Text = "10";
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    next_year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }


            if (!Page.IsPostBack)
            {
                doc_addl.Value = "1";
                lblheader.Text = "Fixed Asset Purchase Record";
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
            btnprint.Visible = false;

            if (ddwarrantydays.Value == "Y")
            {
                txt_warranty_date.Disabled = false;
            }
            else
            {
                txt_warranty_date.Disabled = true;
            }

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
                if (orig_name.ToLower().Contains("sg1_t11")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));
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

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");


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
        btnnew.Disabled = false; btnedit.Disabled = false; btndel.Disabled = false; btnsave.Disabled = true;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false;
        btnprint.Disabled = false;

        //create_tab();
        //create_tab2();
        //create_tab3();
        //create_tab4();

        //sg1_add_blankrows();
        //sg2_add_blankrows();
        //sg3_add_blankrows();
        //sg4_add_blankrows();



        //sg1.DataSource = sg1_dt; sg1.DataBind();
        //if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        //sg2.DataSource = sg2_dt; sg2.DataBind();
        //if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        //sg3.DataSource = sg3_dt; sg3.DataBind();
        //if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
        //sg4.DataSource = sg4_dt; sg4.DataBind();
        //if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();

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
        frm_tabname = "WB_FA_PUR";

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
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
            case "TACODE":
                //pop1
                Acode_Sel_query();
                break;
            case "STICKER":
                SQuery = "select trim(branchcd)||trim(acode)||trim(vchnum)||to_char(vchdate,'dd/MM/yyyy') as fstr,acode,assetname,grpcode,assetsupp,assetsuppadd from wb_fa_pur where branchcd='" + frm_mbr + "' and type='10' ORDER BY ACODE DESC";
                break;
            case "TICODE":
                //pop1
                Icode_Sel_query();
                break;
            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            case "SUP":
                SQuery = "select trim(b.aname)||'~'||trim(b.addr1)||'~'||trim(a.invno)||'~'||to_char(a.invdate,'dd/mm/yyyy') as col1 , trim(b.aname) as aname,trim(a.voucherlink) as voucherlink,a.f_book as assets_accounts,a.a_book as assets_FA_Module,a.rcode,a.invno,to_char(a.invdate,'dd/mm/yyyy') as Inv_date from (select a.voucherlink, sum(a.fbook) as f_book, sum(a.abook) as a_book,max(a.rcode) as rcode,invno,invdate from (select a.Branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.Vchdate,'dd/mm/yyyy') as voucherlink, (dramt) as fbook, 0 as abook,rcode,invno,invdate from voucher a where a.branchcd='" + frm_mbr + "' and substr(type,1,1)= '5' and a.vchdate>= To_date('01/04/2017','dd/mm/yyyy') and substr(acode,1,2)='10' union all select nvl(trim(a.voucherlink),'-'),0 as fbook, (basiccost) as abook,null as rcode,invno,invdate from wb_fa_pur a where branchcd='" + frm_mbr + "' and type='10' and a.vchdate>= To_date('01/04/2017','dd/mm/yyyy') )a group by a.voucherlink,a.invno,a.invdate having sum(a.fbook)-sum(a.abook)>0 ) a, famst b where trim(a.rcode)=trim(b.acode) order by col1";

                break;
            case "ASSETCD":
                SQuery = "Select  Type1 as fstr,Type1 as Code,Name as Particulars from TYPEGRP where branchcd !='DD' and id='FA' order by type1";
                break;
            case "DEPART":
                SQuery = "Select  Type1 as fstr,Type1 as Code,Name as Department_name from TYPE where id='M' and type1 like '6%' order by type1";
                break;
            case "BLOCK":
                SQuery = "Select type1 as fstr, type1 as Code, name as Block_name from typegrp where id='DI' order by type1 ";
                break;
            case "LOCATE":
                SQuery = "Select type1 as fstr, type1 as Code, name as Location_name from typegrp where id='LF' order by type1 ";
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    //    SQuery = "SELECT distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')as fstr,to_char(Vchdate,'dd/mm/yyyy')  as Pur_Dt,Vchnum as Pur_No,AssetSupp,Assetname,assetid, Ent_by,Ent_Dt, to_char(vchdate,'yyyymmdd') as vdd FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'  ORDER BY vdd DESC";
                    SQuery = "SELECT distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')as fstr,to_char(Vchdate,'dd/mm/yyyy')  as Pur_Dt,Vchnum as Pur_No,AssetSupp,Assetname,assetid,grpcode, Ent_by,to_char(Ent_Dt,'dd/mm/yyyy') as ent_Dt, to_char(vchdate,'yyyymmdd') as vdd FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'  ORDER BY vdd DESC";
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
        //fgen.send_mail(frm_cocd, "Tejaxo ERP", "vipin@Tejaxo.in", "", "", "CSS : Query has been logged " + frm_vnum, html_body);
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
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        frm_vty = vty;

        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");

        txtvchnum.Value = frm_vnum;
        txtvchdate.Value = Convert.ToDateTime(fgen.Fn_curr_dt(frm_cocd, frm_qstr)).ToString("yyyy-MM-dd");
        txtlbl5.Value = Convert.ToDateTime(fgen.Fn_curr_dt(frm_cocd, frm_qstr)).ToString("yyyy-MM-dd");
        txtQuantity.Value = "1";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        txtOpDep.Value = "0";
        txt_usedlife.Value = "0";

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

        sg4_dt = new DataTable();
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

        //--------------------------------
        ////sg4_dt = new DataTable();
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
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
        string orig_vchdt;
        orig_vchdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OLD_DATE");
        if (edmode.Value != "Y")
        {
            orig_vchdt = txtvchdate.Value;
        }

        if (Convert.ToDateTime(txtvchdate.Value) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Value) > Convert.ToDateTime(todt))
        {
            fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only");
            txtvchdate.Focus();
            return;
        }

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Value.ToString());
        if (dhd == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return;
        }

        ast_chk_flg = 1;
        cal();

        if (ast_chk_flg == 0)
        {
            string mandField = "";
            mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
            if (mandField.Length > 1)
            {
                fgen.msg("-", "AMSG", mandField);
                return;
            }

            string saveflag = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_");

            fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
            btnsave.Disabled = true;
        }
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
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data a where par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
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
                    newCase(col1);
                    break;
                case "ASSETCD":
                    if (col1 == "") return;
                    txtlbl8.Value = col2;
                    txtlbl8a.Value = col3;
                    btnlocation.Focus();
                    break;
                case "BLOCK":
                    if (col1 == "") return;
                    Textblockg.Value = col1;
                    txtblock.Value = col3;
                    btnCocd.Focus();
                    break;
                case "LOCATE":
                    if (col1 == "") return;
                    txtlocation.Value = col3;
                    txtlocationc.Value = col2;
                    ImageButton1.Focus();
                    break;
                case "DEPART":
                    if (col1 == "") return;
                    txt_department.Value = col3;
                    txtdepartc.Value = col2;
                    txtlbl13.Focus();
                    break;
                case "SUP":

                    if (col1.Length < 1 || col1 == "" || col1 == "0")
                    {
                        return;
                    }
                    else
                    {

                        if (col1.Split('~')[0].ToString() == "0")
                        {
                            txtSup_by.Value = "-";
                        }
                        if (col1.Split('~')[1].ToString() == "0")
                        {
                            txtSup_Address.Value = "-";
                        }
                        if (col1.Split('~')[2].ToString() == "0")
                        {
                            txtlbl12.Text = "-";
                        }
                        Voucherlink.Value = col3;
                        txtSup_by.Value = col1.Split('~')[0].ToString();
                        txtSup_Address.Value = col1.Split('~')[1].ToString();
                        txtlbl2.Value = col1.Split('~')[2].ToString();
                        txtlbl5.Value = Convert.ToDateTime((col1.Split('~')[3])).ToString("yyyy-MM-dd");
                    }
                    txtblock.Focus();
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
                    string chkname = "";
                    chkname = fgen.seek_iname(frm_qstr, frm_cocd, "select assetid from WB_FA_vch where assetid='" + col1.Substring(0, 6) + "' and branchcd='" + frm_mbr + "' and type='30'", "assetid");
                    if (chkname != "0")
                    {
                        fgen.msg("-", "AMSG", "Depreciation already calculated on this Asset Id.Delection not allowed !!");
                        return;
                    }
                    else
                    {
                        fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                        hffield.Value = "D";
                    }
                    break;
                case "Print":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;

                case "STICKER":

                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70410");
                    fgen.fin_acct_reps(frm_qstr);// NAME OF REPORT FOLDER

                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "" || col1 == "-") return;
                    clearctrl();
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    string mv_col;
                    mv_col = frm_mbr + frm_vty + col1;
                    SQuery = "Select a.*,to_Char(a.ent_Dt,'dd/mm/yyyy') As ment_date from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + mv_col + "' ORDER BY A.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txtvchnum.Value = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Value = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("yyyy-MM-dd");
                        //txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

                        //edit command and fetch data from  table wb_fa_pur
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OLD_DATE", txtvchdate.Value);

                        txtlbl8a.Value = dt.Rows[i]["grp"].ToString().Trim();
                        txtlbl3.Value = dt.Rows[i]["basiccost"].ToString().Trim();
                        //ddIssueType.Value = dt.Rows[i]["ISS_tYPE"].ToString().Trim();
                        ddDomImp.Value = dt.Rows[i]["dom_imp"].ToString().Trim();
                        ddtangible.Value = dt.Rows[i]["tangible"].ToString().Trim();
                        ddpurchentry.Value = dt.Rows[i]["purentry"].ToString().Trim();
                        txtdepr_perday.Value = dt.Rows[i]["deprpday"].ToString().Trim();
                        Voucherlink.Value = dt.Rows[i]["voucherlink"].ToString().Trim();
                        txttotal_life.Value = dt.Rows[i]["totlife"].ToString().Trim();
                        txtbal_life.Value = dt.Rows[i]["ballife"].ToString().Trim();
                        //txtdep_rate.Value = dt.Rows[i]["deprate"].ToString().Trim();
                        txtOpDep.Value = dt.Rows[i]["op_dep"].ToString().Trim();
                        txtQuantity.Value = dt.Rows[i]["Quantity"].ToString().Trim();

                        txtlocationc.Value = dt.Rows[i]["locn"].ToString().Trim();
                        string slocation = fgen.seek_iname(frm_qstr, frm_cocd, "select name as slocation from typegrp where branchcd='" + frm_mbr + "' and id='LF' and type1='" + dt.Rows[i]["locn"].ToString().Trim() + "' ", "slocation");
                        txtlocation.Value = slocation;

                        //txtinstalldate.Value = dt.Rows[i]["instdt"].ToString().Trim();
                        txtlbl4.Value = dt.Rows[i]["assetid"].ToString().Trim();
                        txtaname.Value = dt.Rows[i]["assetname"].ToString().Trim();
                        txtSup_by.Value = dt.Rows[i]["assetsupp"].ToString().Trim();
                        txtSup_Address.Value = dt.Rows[i]["assetsuppadd"].ToString().Trim();

                        txtinstalldate.Value = Convert.ToDateTime(dt.Rows[i]["instdt"].ToString().Trim()).ToString("yyyy-MM-dd");
                        txtlbl5.Value = Convert.ToDateTime(dt.Rows[i]["invdate"].ToString().Trim()).ToString("yyyy-MM-dd");
                        txtlbl2.Value = dt.Rows[i]["invno"].ToString().Trim();

                        txtlbl8.Value = dt.Rows[i]["grpcode"].ToString().Trim();
                        txt_installCost.Value = dt.Rows[i]["install_cost"].ToString().Trim();
                        txt_CustomDuty.Value = dt.Rows[i]["custom_duty"].ToString().Trim();
                        txt_originalcost.Value = dt.Rows[i]["original_cost"].ToString().Trim();
                        Textblockg.Value = dt.Rows[i]["block"].ToString().Trim();
                        txtblock.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select name from typegrp where  id='DI' and type1='" + dt.Rows[i]["block"].ToString() + "'", "name");

                        txt_life.Value = dt.Rows[i]["life"].ToString().Trim();

                        txtlbl2.Value = dt.Rows[i]["invno"].ToString().Trim();
                        txtdeprab_val.Value = dt.Rows[i]["depableval"].ToString().Trim();
                        txtresidual_value.Value = dt.Rows[i]["residval"].ToString().Trim();
                        Voucherlink.Value = dt.Rows[i]["Voucherlink"].ToString();
                        txt_usedlife.Value = dt.Rows[i]["USED_LIFE"].ToString();
                        txtlife_end.Value = Convert.ToDateTime(dt.Rows[i]["life_end"].ToString()).ToString("dd/MM/yyyy");
                        txt_otherchrges.Value = dt.Rows[i]["other_chrgs"].ToString();
                        txtdepr_perday.Value = dt.Rows[i]["deprpday"].ToString();
                        txtdepartc.Value = dt.Rows[i]["dcode"].ToString();
                        txt_department.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where  id='M' and type1='" + dt.Rows[i]["dcode"].ToString() + "' and substr(type1,1,1)='6'", "name");
                        ddwarrantydays.Value = dt.Rows[i]["warranty"].ToString();
                        if (dt.Rows[i]["warranty"].ToString() == "Y")
                        {
                            txt_warranty_date.Value = Convert.ToDateTime(dt.Rows[0]["warranty_dt"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            txt_warranty_date.Value = "";
                        }
                        ddOwner.Value = dt.Rows[i]["owner"].ToString();
                        TextBox1.Text = dt.Rows[i]["other_ref"].ToString();
                        txtAttch.Text = dt.Rows[i]["imagef"].ToString().Trim();
                        txtAttchPath.Text = dt.Rows[i]["imagePATH"].ToString().Trim();
                        Txtoldtag.Value = dt.Rows[i]["col1"].ToString().Trim();
                        txtadddepp.Value = dt.Rows[i]["adddepp"].ToString();


                        dt.Dispose();
                        if (1 == 2)
                        {
                            SQuery = "Select nvl(a.udf_name,'-') as udf_name,nvl(a.udf_value,'-') as udf_value from udf_Data a where trim(a.par_tbl)='" + frm_tabname + "' and trim(a.par_fld)='" + mv_col + "' ORDER BY a.srno";
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
                                    sg4_dr["sg4_t2"] = dt.Rows[i]["udf_value"].ToString().Trim();

                                    sg4_dt.Rows.Add(sg4_dr);
                                }
                            }
                            sg4_add_blankrows();
                            ViewState["sg4"] = sg4_dt;
                            sg4.DataSource = sg4_dt;
                            sg4.DataBind();
                            dt.Dispose();
                            sg4_dt.Dispose();
                            //------------------------

                            //
                            sg3_dt.Dispose();
                        }

                        //-----------------------

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                        txtlbl4.Disabled = true;
                        txtvchnum.Disabled = true;
                        if (txtAttchPath.Text.Length > 1)
                        {
                            btnDwnld1.Visible = true;
                            btnView1.Visible = true;
                        }
                    }
                    #endregion
                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    break;
                //case "BTN_10":
                //    break;
                //case "BTN_11":
                //    break;
                //case "BTN_12":
                //    break;
                //case "BTN_13":
                //    break;
                //case "BTN_14":
                //    break;
                //case "BTN_15":
                //    break;
                //case "BTN_16":
                //    break;
                //case "BTN_17":
                //    break;
                //case "BTN_18":
                //    break;
                //case "BTN_19":
                //    break;
                //case "BTN_20":
                //    break;
                //case "BTN_21":
                //    break;
                //case "BTN_22":
                //    break;
                //case "BTN_23":
                //    break;
                case "TICODE":
                    if (col1.Length <= 0) return;
                    break;
                //case "SG1_ROW_ADD":
                //    #region for gridview 1
                //    if (col1.Length <= 0) return;
                //    if (ViewState["sg1"] != null)
                //    {
                //        dt = new DataTable();
                //        sg1_dt = new DataTable();
                //        dt = (DataTable)ViewState["sg1"];
                //        z = dt.Rows.Count - 1;
                //        sg1_dt = dt.Clone();
                //        sg1_dr = null;
                //        for (i = 0; i < dt.Rows.Count - 1; i++)
                //        {
                //            sg1_dr = sg1_dt.NewRow();
                //            sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                //            sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
                //            sg1_dr["sg1_h2"] = dt.Rows[i]["sg1_h2"].ToString();
                //            sg1_dr["sg1_h3"] = dt.Rows[i]["sg1_h3"].ToString();
                //            sg1_dr["sg1_h4"] = dt.Rows[i]["sg1_h4"].ToString();
                //            sg1_dr["sg1_h5"] = dt.Rows[i]["sg1_h5"].ToString();
                //            sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                //            sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                //            sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                //            sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                //            sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();

                //            sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                //            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                //            sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                //            sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                //            sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                //            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                //            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                //            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                //            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                //            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                //            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                //            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                //            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                //            sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                //            sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                //            sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                //            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                //            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                //            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                //            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                //            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();

                //            sg1_dt.Rows.Add(sg1_dr);
                //        }

                //        dt = new DataTable();
                //        if (col1.Length > 8) SQuery = "select * from item where trim(icode) in (" + col1 + ")";
                //        else SQuery = "select * from item where trim(icode)='" + col1 + "'";
                //        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                //        for (int d = 0; d < dt.Rows.Count; d++)
                //        {
                //            sg1_dr = sg1_dt.NewRow();
                //            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                //            sg1_dr["sg1_h1"] = dt.Rows[d]["icode"].ToString().Trim();
                //            sg1_dr["sg1_h2"] = dt.Rows[d]["iname"].ToString().Trim();
                //            sg1_dr["sg1_h3"] = "-";
                //            sg1_dr["sg1_h4"] = "-";
                //            sg1_dr["sg1_h5"] = "-";
                //            sg1_dr["sg1_h6"] = "-";
                //            sg1_dr["sg1_h7"] = "-";
                //            sg1_dr["sg1_h8"] = "-";
                //            sg1_dr["sg1_h9"] = "-";
                //            sg1_dr["sg1_h10"] = "-";

                //            sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                //            sg1_dr["sg1_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                //            sg1_dr["sg1_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();
                //            sg1_dr["sg1_f4"] = dt.Rows[d]["cdrgno"].ToString().Trim();
                //            sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();
                //            //fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                //            sg1_dr["sg1_t1"] = "";
                //            sg1_dr["sg1_t2"] = "";
                //            sg1_dr["sg1_t3"] = "0";
                //            sg1_dr["sg1_t4"] = "0";
                //            sg1_dr["sg1_t5"] = "0";
                //            sg1_dr["sg1_t6"] = "0";
                //            sg1_dr["sg1_t7"] = "0";
                //            sg1_dr["sg1_t8"] = "0";
                //            sg1_dr["sg1_t9"] = "0";
                //            sg1_dr["sg1_t10"] = "";
                //            sg1_dr["sg1_t11"] = "";
                //            sg1_dr["sg1_t12"] = "";
                //            sg1_dr["sg1_t13"] = "";
                //            sg1_dr["sg1_t14"] = "";
                //            sg1_dr["sg1_t15"] = "";
                //            sg1_dr["sg1_t16"] = "";

                //            sg1_dt.Rows.Add(sg1_dr);
                //        }
                //    }
                //    sg1_add_blankrows();

                //    ViewState["sg1"] = sg1_dt;
                //    sg1.DataSource = sg1_dt;
                //    sg1.DataBind();
                //    dt.Dispose(); sg1_dt.Dispose();
                //    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                //    #endregion
                //    setColHeadings();
                //    break;
                //case "SG1_ROW_ADD_E":
                //    if (col1.Length <= 0) return;
                //    if (edmode.Value == "Y")
                //    {
                //        //return;
                //    }

                //    //********* Saving in Hidden Field 
                //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                //    //********* Saving in GridView Value
                //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                //    //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                //    setColHeadings();
                //    break;
                //case "SG3_ROW_ADD":
                //    #region for gridview 1
                //    if (col1.Length <= 0) return;
                //    if (ViewState["sg3"] != null)
                //    {
                //        dt = new DataTable();
                //        sg3_dt = new DataTable();
                //        dt = (DataTable)ViewState["sg3"];
                //        z = dt.Rows.Count - 1;
                //        sg3_dt = dt.Clone();
                //        sg3_dr = null;
                //        for (i = 0; i < dt.Rows.Count - 1; i++)
                //        {
                //            sg3_dr = sg3_dt.NewRow();
                //            sg3_dr["sg3_srno"] = Convert.ToInt32(dt.Rows[i]["sg3_srno"].ToString());
                //            sg3_dr["sg3_f1"] = dt.Rows[i]["sg3_f1"].ToString();
                //            sg3_dr["sg3_f2"] = dt.Rows[i]["sg3_f2"].ToString();
                //            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                //            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                //            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                //            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();
                //            sg3_dt.Rows.Add(sg3_dr);
                //        }

                //        dt = new DataTable();
                //        if (col1.Length > 8) SQuery = "select * from item where trim(icode) in (" + col1 + ")";
                //        else SQuery = "select * from item where trim(icode)='" + col1 + "'";
                //        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                //        for (int d = 0; d < dt.Rows.Count; d++)
                //        {
                //            sg3_dr = sg3_dt.NewRow();
                //            sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;

                //            sg3_dr["sg3_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                //            sg3_dr["sg3_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                //            sg3_dr["sg3_t1"] = "";
                //            sg3_dr["sg3_t2"] = "";
                //            sg3_dr["sg3_t3"] = "";
                //            sg3_dr["sg3_t4"] = "";
                //            sg3_dt.Rows.Add(sg3_dr);
                //        }
                //    }
                //    sg3_add_blankrows();

                //    ViewState["sg3"] = sg3_dt;
                //    sg3.DataSource = sg3_dt;
                //    sg3.DataBind();
                //    dt.Dispose(); sg3_dt.Dispose();
                //    //((TextBox)sg3.Rows[z].FindControl("sg3_t1")).Focus();
                //    #endregion
                //    break;
                //case "SG1_ROW_TAX":
                //    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                //    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                //    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t16")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

                //    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t12")).Focus();
                //    break;
                //case "SG1_ROW_DT":
                //    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                //    break;

                //case "SG2_RMV":
                //    #region Remove Row from GridView
                //    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                //    {
                //        dt = new DataTable();
                //        sg2_dt = new DataTable();
                //        dt = (DataTable)ViewState["sg2"];
                //        z = dt.Rows.Count - 1;
                //        sg2_dt = dt.Clone();
                //        sg2_dr = null;
                //        i = 0;
                //        for (i = 0; i < sg2.Rows.Count - 1; i++)
                //        {
                //            sg2_dr = sg2_dt.NewRow();
                //            sg2_dr["sg2_srno"] = (i + 1);
                //            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                //            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                //            sg2_dt.Rows.Add(sg2_dr);
                //        }

                //        sg2_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                //        sg2_add_blankrows();

                //        ViewState["sg2"] = sg2_dt;
                //        sg2.DataSource = sg2_dt;
                //        sg2.DataBind();
                //    }
                //    #endregion
                //    setColHeadings();
                //    break;
                //case "SG4_RMV":
                //    #region Remove Row from GridView
                //    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                //    {
                //        dt = new DataTable();
                //        sg4_dt = new DataTable();
                //        dt = (DataTable)ViewState["sg4"];
                //        z = dt.Rows.Count - 1;
                //        sg4_dt = dt.Clone();
                //        sg4_dr = null;
                //        i = 0;
                //        for (i = 0; i < sg4.Rows.Count - 1; i++)
                //        {
                //            sg4_dr = sg4_dt.NewRow();
                //            sg4_dr["sg4_srno"] = (i + 1);

                //            sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                //            sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();


                //            sg4_dt.Rows.Add(sg4_dr);
                //        }

                //        sg4_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                //        sg4_add_blankrows();

                //        ViewState["sg4"] = sg4_dt;
                //        sg4.DataSource = sg4_dt;
                //        sg4.DataBind();
                //    }
                //    #endregion
                //    setColHeadings();
                //    break;
                //case "SG3_RMV":
                //    #region Remove Row from GridView
                //    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                //    {
                //        dt = new DataTable();
                //        sg3_dt = new DataTable();
                //        dt = (DataTable)ViewState["sg3"];
                //        z = dt.Rows.Count - 1;
                //        sg3_dt = dt.Clone();
                //        sg3_dr = null;
                //        i = 0;
                //        for (i = 0; i < sg3.Rows.Count - 1; i++)
                //        {
                //            sg3_dr = sg3_dt.NewRow();
                //            sg3_dr["sg3_srno"] = (i + 1);
                //            sg3_dr["sg3_f1"] = sg3.Rows[i].Cells[3].Text.Trim();
                //            sg3_dr["sg3_f2"] = sg3.Rows[i].Cells[4].Text.Trim();

                //            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                //            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                //            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                //            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();

                //            sg3_dt.Rows.Add(sg3_dr);
                //        }

                //        sg3_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                //        sg3_add_blankrows();

                //        ViewState["sg3"] = sg3_dt;
                //        sg3.DataSource = sg3_dt;
                //        sg3.DataBind();
                //    }
                //    #endregion
                //    setColHeadings();
                //    break;
                //case "SG1_RMV":
                //    #region Remove Row from GridView
                //    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                //    {
                //        dt = new DataTable();
                //        sg1_dt = new DataTable();
                //        dt = (DataTable)ViewState["sg1"];
                //        z = dt.Rows.Count - 1;
                //        sg1_dt = dt.Clone();
                //        sg1_dr = null;
                //        i = 0;
                //        for (i = 0; i < sg1.Rows.Count - 1; i++)
                //        {
                //            sg1_dr = sg1_dt.NewRow();
                //            sg1_dr["sg1_srno"] = (i + 1);
                //            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.Trim();
                //            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.Trim();
                //            sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.Trim();
                //            sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.Trim();
                //            sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.Trim();
                //            sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.Trim();
                //            sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.Trim();
                //            sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.Trim();
                //            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.Trim();
                //            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.Trim();

                //            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.Trim();
                //            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim();
                //            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                //            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                //            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();

                //            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                //            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                //            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                //            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                //            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                //            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                //            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                //            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                //            sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                //            sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                //            sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                //            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                //            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                //            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                //            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                //            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();

                //            sg1_dt.Rows.Add(sg1_dr);
                //        }

                //        if (edmode.Value == "Y")
                //        {
                //            //sg1_dr["sg1_f1"] = "*" + sg1.Rows[-1 + Convert.ToInt32(hf1.Value.Trim())].Cells[13].Text.Trim();

                //            sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                //        }
                //        else
                //        {
                //            sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                //        }

                //        sg1_add_blankrows();

                //        ViewState["sg1"] = sg1_dt;
                //        sg1.DataSource = sg1_dt;
                //        sg1.DataBind();
                //    }
                //    #endregion
                //    setColHeadings();
                //    break;
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
            SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/MM/yyyy') as Entry_date, acode as Asset_code, assetid, AssetName,Basiccost,install_cost,custom_duty,other_chrgs,original_cost,op_dep,deprpday, to_char(Instdt,'dd/MM/yyyy') as Installdt, ent_by,to_char(ent_dt,'dd/MM/yyyy')as dt  from wb_fa_pur  where branchcd='" + frm_mbr + "' and Type='10' and Vchdate " + PrdRange + " order by vchnum,vchdate";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------

            string last_entdt;
            //checks
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a_Text + "'  ", "ldt");
            if (last_entdt == "0" || edmode.Value == "Y")
            {
            }
            else
            {
                if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Value.ToString()))
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + "already entered");
                }
            }
            ////last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            ////if (Convert.ToDateTime(txtvchdate.Value.ToString()) > Convert.ToDateTime(last_entdt))
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

                        oDS2 = new DataSet();
                        oporow2 = null;
                        //oDS2 = fgen.fill_schema(frm_qstr,frm_cocd, "ivchctrl");

                        oDS3 = new DataSet();
                        oporow3 = null;
                        //oDS3 = fgen.fill_schema(frm_qstr,frm_cocd, "poterm");

                        oDS4 = new DataSet();
                        oporow4 = null;
                        //oDS4 = fgen.fill_schema(frm_qstr,frm_cocd, "budgmst");

                        oDS5 = new DataSet();
                        oporow5 = null;
                        //oDS5 = fgen.fill_schema(frm_qstr,frm_cocd, "udf_data");


                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();


                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        //oDS2 = fgen.fill_schema(frm_qstr,frm_cocd, "ivchctrl");

                        oDS3.Dispose();
                        oporow3 = null;
                        oDS3 = new DataSet();
                        //oDS3 = fgen.fill_schema(frm_qstr,frm_cocd, "poterm");

                        oDS4.Dispose();
                        oporow4 = null;
                        oDS4 = new DataSet();
                        //oDS4 = fgen.fill_schema(frm_qstr,frm_cocd, "budgmst");

                        oDS5.Dispose();
                        oporow5 = null;
                        oDS5 = new DataSet();
                        //oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");


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
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Value.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }

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
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + ddl_fld1 + "'");
                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);



                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + ddl_fld2 + "'");
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //html_body = html_body + "Please note your CSS No : " + frm_vnum + "<br>";
                                //html_body = html_body + "Tejaxo ERP Customer Support Team Will analyse the same within next 2-3 working days.<br>";
                                //html_body = html_body + "You can track Progress on your service request through CSS status also.<br>";
                                //html_body = html_body + "Always at your service, <br>";
                                //html_body = html_body + "Tejaxo support <br>";

                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", txtlbl5.Value, "", "", "CSS : Query has been logged " + frm_vnum, html_body);

                                fgen.msg("-", "AMSG", lblheader.Text + " Entry No. " + txtvchnum.Value + " Data saved Successfully.");
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
    public void create_tab()
    {
        //sg1_dt = new DataTable();
        //sg1_dr = null;
        //// Hidden Field
        //sg1_dt.Columns.Add(new DataColumn("sg1_h1", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_h2", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_h3", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_h4", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_h5", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_h6", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_h7", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_h8", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_h9", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_h10", typeof(string)));

        //sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(Int32)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_f1", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_f2", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_f3", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_f4", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_f5", typeof(string)));

        //sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t5", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t6", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t7", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t8", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t9", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t10", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t11", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t12", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t13", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t14", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t15", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t16", typeof(string)));

    }
    public void create_tab2()
    {
        //sg2_dt = new DataTable();
        //sg2_dr = null;
        //// Hidden Field

        //sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
        //sg2_dt.Columns.Add(new DataColumn("sg2_t1", typeof(string)));
        //sg2_dt.Columns.Add(new DataColumn("sg2_t2", typeof(string)));
        //sg2_dt.Columns.Add(new DataColumn("sg2_t3", typeof(string)));
        //sg2_dt.Columns.Add(new DataColumn("sg2_t4", typeof(string)));

    }

    public void create_tab3()
    {


        //sg3_dt = new DataTable();
        //sg3_dr = null;
        //// Hidden Field

        //sg3_dt.Columns.Add(new DataColumn("sg3_SrNo", typeof(Int32)));
        //sg3_dt.Columns.Add(new DataColumn("sg3_f1", typeof(string)));
        //sg3_dt.Columns.Add(new DataColumn("sg3_f2", typeof(string)));
        //sg3_dt.Columns.Add(new DataColumn("sg3_t1", typeof(string)));
        //sg3_dt.Columns.Add(new DataColumn("sg3_t2", typeof(string)));
        //sg3_dt.Columns.Add(new DataColumn("sg3_t3", typeof(string)));
        //sg3_dt.Columns.Add(new DataColumn("sg3_t4", typeof(string)));

    }

    public void create_tab4()
    {
        //sg4_dt = new DataTable();
        //sg4_dr = null;
        //// Hidden Field

        //sg4_dt.Columns.Add(new DataColumn("sg4_SrNo", typeof(Int32)));
        //sg4_dt.Columns.Add(new DataColumn("sg4_t1", typeof(string)));
        //sg4_dt.Columns.Add(new DataColumn("sg4_t2", typeof(string)));
        //sg4_dt.Columns.Add(new DataColumn("sg4_t3", typeof(string)));
        //sg4_dt.Columns.Add(new DataColumn("sg4_t4", typeof(string)));

    }

    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        //sg1_dr = sg1_dt.NewRow();
        //sg1_dr["sg1_h1"] = "-";
        //sg1_dr["sg1_h2"] = "-";
        //sg1_dr["sg1_h3"] = "-";
        //sg1_dr["sg1_h4"] = "-";
        //sg1_dr["sg1_h5"] = "-";
        //sg1_dr["sg1_h6"] = "-";
        //sg1_dr["sg1_h7"] = "-";
        //sg1_dr["sg1_h8"] = "-";
        //sg1_dr["sg1_h9"] = "-";
        //sg1_dr["sg1_h10"] = "-";

        //sg1_dr["sg1_SrNo"] = sg1_dt.Rows.Count + 1;


        //sg1_dr["sg1_f1"] = "-";
        //sg1_dr["sg1_f2"] = "-";
        //sg1_dr["sg1_f3"] = "-";
        //sg1_dr["sg1_f4"] = "-";
        //sg1_dr["sg1_f5"] = "-";

        //sg1_dr["sg1_t1"] = "-";
        //sg1_dr["sg1_t2"] = "-";
        //sg1_dr["sg1_t3"] = "0";
        //sg1_dr["sg1_t4"] = "0";
        //sg1_dr["sg1_t5"] = "0";
        //sg1_dr["sg1_t6"] = "0";
        //sg1_dr["sg1_t7"] = "0";
        //sg1_dr["sg1_t8"] = "0";
        //sg1_dr["sg1_t9"] = "-";
        //sg1_dr["sg1_t10"] = "-";
        //sg1_dr["sg1_t11"] = "-";
        //sg1_dr["sg1_t12"] = "-";
        //sg1_dr["sg1_t13"] = "-";
        //sg1_dr["sg1_t14"] = "-";
        //sg1_dr["sg1_t15"] = "-";
        //sg1_dr["sg1_t16"] = "-";

        //sg1_dt.Rows.Add(sg1_dr);
    }
    public void sg2_add_blankrows()
    {
        //sg2_dr = sg2_dt.NewRow();


        //sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
        //sg2_dr["sg2_t1"] = "-";
        //sg2_dr["sg2_t2"] = "-";
        //sg2_dt.Rows.Add(sg2_dr);
    }
    public void sg3_add_blankrows()
    {
        //sg3_dr = sg3_dt.NewRow();

        //sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
        //sg3_dr["sg3_f1"] = "-";
        //sg3_dr["sg3_f2"] = "-";
        //sg3_dr["sg3_t1"] = "-";
        //sg3_dr["sg3_t2"] = "-";
        //sg3_dr["sg3_t3"] = "-";
        //sg3_dr["sg3_t4"] = "-";

        //sg3_dt.Rows.Add(sg3_dr);
    }

    public void sg4_add_blankrows()
    {
        //sg4_dr = sg4_dt.NewRow();


        //sg4_dr["sg4_SrNo"] = sg4_dt.Rows.Count + 1;
        //sg4_dr["sg4_t1"] = "-";
        //sg4_dr["sg4_t2"] = "-";
        //sg4_dt.Rows.Add(sg4_dr);
    }

    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    for (int sg1r = 0; sg1r < sg1.Rows.Count; sg1r++)
        //    {
        //        for (int j = 0; j < sg1.Columns.Count; j++)
        //        {
        //            sg1.Rows[sg1r].Cells[j].ToolTip = sg1.Rows[sg1r].Cells[j].Text;
        //            if (sg1.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
        //            {
        //                sg1.Rows[sg1r].Cells[j].Text = sg1.Rows[sg1r].Cells[j].Text.Substring(0, 35);
        //            }
        //        }
        //    }
        //}
    }

    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }

    //------------------------------------------------------------------------------------
    protected void sg3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //string var = e.CommandName.ToString();
        //int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        //int index = Convert.ToInt32(sg3.Rows[rowIndex].RowIndex);

        //if (txtvchnum.Value == "-")
        //{
        //    fgen.msg("-", "AMSG", "Doc No. not correct");
        //    return;
        //}
        //switch (var)
        //{
        //    case "SG3_RMV":

        //        break;
        //    case "SG3_ROW_ADD":

        //        break;
        //}
    }
    protected void sg4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //string var = e.CommandName.ToString();
        //int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        //int index = Convert.ToInt32(sg4.Rows[rowIndex].RowIndex);

        //if (txtvchnum.Value == "-")
        //{
        //    fgen.msg("-", "AMSG", "Doc No. not correct");
        //    return;
        //}
        //switch (var)
        //{
        //    case "sg4_RMV":
        //        if (index < sg4.Rows.Count - 1)
        //        {
        //            hf1.Value = index.ToString();
        //            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
        //            //----------------------------
        //            hffield.Value = "sg4_RMV";
        //            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        //            fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
        //        }
        //        break;
        //    case "sg4_ROW_ADD":
        //        dt = new DataTable();
        //        sg4_dt = new DataTable();
        //        dt = (DataTable)ViewState["sg4"];
        //        z = dt.Rows.Count - 1;
        //        sg4_dt = dt.Clone();
        //        sg4_dr = null;
        //        i = 0;
        //        for (i = 0; i < sg4.Rows.Count; i++)
        //        {
        //            sg4_dr = sg4_dt.NewRow();
        //            sg4_dr["sg4_srno"] = (i + 1);
        //            sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
        //            sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
        //            sg4_dt.Rows.Add(sg4_dr);
        //        }
        //        sg4_add_blankrows();
        //        ViewState["sg4"] = sg4_dt;
        //        sg4.DataSource = sg4_dt;
        //        sg4.DataBind();
        //        break;
        //}
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

    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();

        oporow["BRANCHCD"] = frm_mbr;
        oporow["VCHNUM"] = txtvchnum.Value.Trim();
        oporow["VCHDATE"] = Convert.ToDateTime(txtvchdate.Value.Trim());
        oporow["TYPE"] = frm_vty;
        oporow["GRP"] = txtlbl8a.Value.ToUpper().Trim();
        oporow["GRPCODE"] = txtlbl8.Value.ToUpper().Trim();
        oporow["ASSETNAME"] = txtaname.Value.ToUpper().Trim();
        oporow["ASSETSUPP"] = txtSup_by.Value.ToUpper().Trim();
        oporow["ASSETSUPPADD"] = txtSup_Address.Value.ToUpper().Trim();
        oporow["ASSETID"] = txtlbl4.Value;
        oporow["INVNO"] = txtlbl2.Value.Trim();
        oporow["INVDATE"] = txtlbl5.Value;
        oporow["ACODE"] = txtvchnum.Value.Trim();
        oporow["QUANTITY"] = txtQuantity.Value;
        oporow["VOUCHERLINK"] = Voucherlink.Value.Trim();
        oporow["OP_DEP"] = Math.Round(fgen.make_double(txtOpDep.Value.Trim()), 2);
        oporow["PURENTRY"] = ddpurchentry.Value.Trim();
        oporow["TANGIBLE"] = ddtangible.Value.Trim();
        oporow["DOM_IMP"] = ddDomImp.Value.Trim();
        oporow["BASICCOST"] = Math.Round(fgen.make_double(txtlbl3.Value.Trim()), 2);
        oporow["INSTALL_COST"] = Math.Round(fgen.make_double(txt_installCost.Value.Trim()), 2);
        oporow["CUSTOM_DUTY"] = Math.Round(fgen.make_double(txt_CustomDuty.Value.Trim()), 2);
        oporow["OTHER_CHRGS"] = Math.Round(fgen.make_double(txt_otherchrges.Value.Trim()), 2);
        oporow["ORIGINAL_COST"] = Math.Round(fgen.make_double(txt_originalcost.Value.Trim()), 2);
        oporow["life"] = txt_life.Value.Trim();
        oporow["LOCN"] = txtlocationc.Value.Trim();
        oporow["INSTDT"] = fgen.make_def_Date(Convert.ToDateTime(txtinstalldate.Value.Trim()).ToString("dd/MM/yyyy"), vardate);
        oporow["WARRANTY"] = ddwarrantydays.Value.Trim();
        if (txt_warranty_date.Value.Length == 10)
        {
            oporow["WARRANTY_DT"] = fgen.make_def_Date(txt_warranty_date.Value.Trim(), vardate);
        }
        else
        {
            oporow["WARRANTY_DT"] = System.DateTime.Today.ToString("dd/MM/yyyy");
        }


        oporow["DCODE"] = fgen.seek_iname(frm_qstr, frm_cocd, "select type1 from type where id='M' and name='" + txt_department.Value.Trim() + "' and substr(type1,1,1)='6'", "type1");
        oporow["owner"] = ddOwner.Value.Trim();
        if (txtAttch.Text.Length > 1)
        {
            oporow["IMAGEF"] = txtAttch.Text.ToUpper().Trim();
            oporow["IMAGEPATH"] = txtAttchPath.Text.ToUpper().Trim();
        }

        oporow["DEPRPDAY"] = Math.Round(fgen.make_double(txtdepr_perday.Value), 2);
        oporow["DEPABLEVAL"] = Math.Round(fgen.make_double(txtdeprab_val.Value), 2);
        oporow["RESIDVAL"] = Math.Round(fgen.make_double(txtresidual_value.Value), 2);
        oporow["USED_LIFE"] = fgen.make_double(txt_usedlife.Value);
        oporow["col1"] = Txtoldtag.Value.Trim();
        oporow["BLOCK"] = Textblockg.Value.Trim();

        DateTime instdtcon = Convert.ToDateTime(txtinstalldate.Value.ToString());
        DateTime enddate = (instdtcon.AddYears(Convert.ToInt32(txt_life.Value.Trim()))).AddDays(-1);
        string strenddate = enddate.ToString("dd/MM/yyyy");
        oporow["life_end"] = Convert.ToDateTime(strenddate).ToString("dd/MM/yyyy").Trim();

        DateTime lifenddt = Convert.ToDateTime(txtlife_end.Value.ToString().Trim());
        DateTime instaldt = Convert.ToDateTime(txtinstalldate.Value.ToString().Trim());
        Double vtotlife = ((lifenddt - instaldt).TotalDays) + 1;
        oporow["totlife"] = Convert.ToString(vtotlife);
        oporow["BALLIFE"] = txtbal_life.Value;
        //oporow["AMC"] = ddamc.Value;
        oporow["other_ref"] = TextBox1.Text.Trim();
        oporow["adddepp"] = fgen.make_double(txtadddepp.Value.Trim());
        if (edmode.Value == "Y")
        {
            oporow["eNt_by"] = ViewState["entby"].ToString();
            oporow["eNt_dt"] = ViewState["entdt"].ToString();
            oporow["edt_by"] = frm_uname;
            oporow["edt_dt"] = vardate;
            // oporow["app_by"] = "-";
            //oporow["app_dt"] = vardate;
        }
        else
        {
            oporow["eNt_by"] = frm_uname;
            oporow["eNt_dt"] = vardate;
            oporow["edt_by"] = "-";
            oporow["eDt_dt"] = vardate;
            // oporow["app_by"] = "-";
            //  oporow["app_dt"] = vardate;
        }
        oDS.Tables[0].Rows.Add(oporow);

    }
    void save_fun2()
    {

    }
    void save_fun3()
    {

    }
    void save_fun4()
    {

    }
    void save_fun5()
    {

    }
    void Acode_Sel_query()
    {

    }
    void Icode_Sel_query()
    {

    }

    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        //switch (Prg_Id)
        //{
        //    case "F60101":
        //        SQuery = "SELECT 'CS' AS FSTR,'Support Request Logging' as NAME,'CS' AS CODE FROM dual";
        //        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CS");
        //        break;

        //}
    }

    //------------------------------------------------------------------------------------   
    protected void sg4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    for (int sg1r = 0; sg1r < sg4.Rows.Count; sg1r++)
        //    {
        //        for (int j = 0; j < sg4.Columns.Count; j++)
        //        {
        //            sg4.Rows[sg1r].Cells[j].ToolTip = sg4.Rows[sg1r].Cells[j].Text;
        //            if (sg4.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
        //            {
        //                sg4.Rows[sg1r].Cells[j].Text = sg4.Rows[sg1r].Cells[j].Text.Substring(0, 35);
        //            }
        //        }
        //    }
        //    e.Row.Cells[0].Style["display"] = "none";
        //    sg4.HeaderRow.Cells[0].Style["display"] = "none";
        //    sg4.HeaderRow.Cells[1].Style["display"] = "none";
        //    e.Row.Cells[1].Style["display"] = "none";
        //}
    }
    protected void btnAtt_Click(object sender, EventArgs e)
    {
        string filepath = @"c:\TEJ_ERP\UPLOAD\";      //Server.MapPath("~/tej-base/UPLOAD/");
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            txtAttch.Text = Attch.FileName;
            string fileName = txtlbl4.Value.Trim().Replace("/", "_").Replace(@"\", "_") + "_" + txtvchnum.Value.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
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
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Tejaxo Viewer');", true);
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
    protected void btnCocd_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ASSETCD";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Code", frm_qstr);
    }

    protected void btnSup_Click(object sender, ImageClickEventArgs e)
    {

        hffield.Value = "SUP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Supplied By", frm_qstr);
    }
    protected void btnlocation_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "LOCATE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Location", frm_qstr);
    }
    protected void btnblock_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BLOCK";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Block", frm_qstr);
    }
    protected void btn_tag_ServerClick(object sender, EventArgs e)
    {

        hffield.Value = "STICKER";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);

    }
    protected void btndepartment_Click(object sender, ImageClickEventArgs e)
    {

        hffield.Value = "DEPART";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select DEPARTMENT", frm_qstr);
    }


    public void cal()
    {
        int flag = 0;
        double adddepcal = 0;
        if (edmode.Value == "Y")
        { }
        else
        {
            string VCH_DATE = "";
            VCH_DATE = fgen.seek_iname(frm_qstr, frm_cocd, "select (Case when to_DatE('" + txtvchdate.Value + "','yyyy-mm-dd')< MAX(VCHDATE) then 'NO' else 'OK' end) as fstr from  WB_FA_PUR WHERE branchcd='" + frm_mbr + "' and TYPE='10'", "fstr");

            if (VCH_DATE == "NO")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Asset Entries Already Made in Dates Later then Current Doc Date, Entry not Allowed ,Please Re Check");
                return;
            }
        }

        if ((txt_life.Value == "") || (txt_life.Value == "-"))
        {
            fgen.msg("", "ASMG", "Please enter life."); txt_life.Focus(); flag = 1;
            return;
        }

        if ((txtinstalldate.Value == "") || (txtinstalldate.Value == "-"))
        {
            fgen.msg("", "ASMG", "Please enter install date."); txtinstalldate.Focus(); flag = 1;
            return;
        }

        if (Convert.ToInt32(txt_life.Value) <= 0)
        {
            fgen.msg("", "ASMG", "Life cannot be 0 or less than 0."); txt_life.Focus(); flag = 1;
            return;
        }

        if (Convert.ToInt32(txtQuantity.Value) != 1)
        {
            fgen.msg("", "ASMG", "Quantity can be 1 only."); txtQuantity.Focus(); flag = 1;
            return;
        }

        DateTime instdtcon = Convert.ToDateTime(txtinstalldate.Value.ToString());
        DateTime enddate = (instdtcon.AddYears(Convert.ToInt32(txt_life.Value.Trim()))).AddDays(-1);
        string strenddate = enddate.ToString("dd/MM/yyyy");
        txtlife_end.Value = Convert.ToDateTime(strenddate).ToString("dd/MM/yyyy").Trim();

        DateTime lifenddt = Convert.ToDateTime(txtlife_end.Value.ToString().Trim());
        DateTime instaldt = Convert.ToDateTime(txtinstalldate.Value.ToString().Trim());
        Double vtotlife = ((lifenddt - instaldt).TotalDays) + 1;
        txttotal_life.Value = Convert.ToString(vtotlife);

        double balance = (vtotlife - Convert.ToDouble(txt_usedlife.Value));
        if (balance < 0)
        {
            txtbal_life.Value = "0";
        }
        else
        {
            txtbal_life.Value = balance.ToString();
        }

        if (txtlbl3.Value.Length < 2)
        {
            fgen.msg("-", "AMSG", " Please Enter Basic cost. !!"); txtlbl3.Focus(); flag = 1;
            return;
        }


        if (txtOpDep.Value.Trim() == "")
        {
            fgen.msg("", "ASMG", "Please Enter Opening depreciation, else put 0"); txtOpDep.Focus(); flag = 1;
            return;

        }
        txt_originalcost.Value = Convert.ToString(fgen.make_double(txt_CustomDuty.Value.Trim()) + fgen.make_double(txt_installCost.Value.Trim()) + fgen.make_double(txtlbl3.Value.Trim()) + fgen.make_double(txt_CustomDuty.Value.Trim()) + fgen.make_double(txt_otherchrges.Value.Trim()));
        double val3 = fgen.make_double(txt_originalcost.Value.Trim());
        double val4 = fgen.make_double(txtOpDep.Value.Trim());
        double val5 = (val3 - val4);
        txtdeprab_val.Value = val5.ToString();

        string residper = "";
        residper = fgen.getOption(frm_qstr, frm_cocd, "W1075", "OPT_PARAM");
        txtresidual_value.Value = Convert.ToString(val3 * (Convert.ToDouble(residper)) / 100);

        if ((txtresidual_value.Value == "") || (txt_originalcost.Value == "") || (txttotal_life.Value == ""))
        {

            fgen.msg("", "ASMG", "Please Enter  Residual  Value/Original cost/ Life."); txtresidual_value.Focus(); flag = 1;
            return;

        }

        if (txttotal_life.Value.Trim() == "0")
        {
            fgen.msg("", "ASMG", "Total Value cannot be zero"); txttotal_life.Focus(); flag = 1;
            return;
        }

        if (Convert.ToDouble(txtresidual_value.Value) > Convert.ToDouble(txt_originalcost.Value))
        {
            fgen.msg("-", "AMSG", " Please Enter correct Residual value !! Residual Value should be less than Original cost"); txtresidual_value.Focus(); flag = 1;
            return;
        }
        else
        {
            if (residper.ToString().Trim() == "" || residper.ToString().Trim() == "0")
            { }
            else
            {
                double val6 = val3 * (Convert.ToDouble(residper)) / 100;
                if (Convert.ToDouble(txtresidual_value.Value) == val6)
                { }
                else
                {
                    fgen.msg("-", "AMSG", " Please Enter correct Residual value !! Residual Value should be (" + residper + " %) in accordance with the percentage specified in master control."); txtresidual_value.Focus(); flag = 1;
                    return;
                }
            }
        }

        string vch_balq = "";
        string vch_amt = "";
        if (Voucherlink.Value.Length < 20)
        { }
        else
        {
            if (Voucherlink.Value.Substring(1, 1) == "5")
            {
                vch_balq = "select sum(a.fbook)-sum(a.abook) as bal_val from (select a.Branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.Vchdate,'dd/mm/yyyy') as voucherlink, (dramt) as fbook, 0 as abook,rcode,invno,invdate from voucher a where a.branchcd='" + frm_mbr + "' and substr(type,1,1)= '5' and a.vchdate>= To_date('01/04/2017','dd/mm/yyyy') and substr(acode,1,2)='10' and branchcd||type||vchnum||to_char(vchdate,'dd/mm/yyyy')='" + Voucherlink.Value + "' union all select trim(a.voucherlink),0 as fbook, (basiccost) as abook,null as rcode,invno,invdate from wb_fa_pur a where branchcd='" + frm_mbr + "' and type='10' and a.vchdate>= To_date('01/04/2017','dd/mm/yyyy') and vchnum||to_char(vchdate,'yyyy-mm-dd')!='" + txtvchnum.Value + txtvchdate.Value + "')a group by a.voucherlink,a.invno,a.invdate having sum(a.fbook)-sum(a.abook)>0";
                vch_amt = fgen.seek_iname(frm_qstr, frm_cocd, vch_balq, "bal_Val");
                if (fgen.make_double(txtlbl3.Value) > fgen.make_double(vch_amt))
                {
                    fgen.msg("-", "AMSG", "Accounts Vch Balance Amt " + vch_amt + " is less that Amount  Booked here " + txtlbl3.Value + ", Please Re check ");
                    txtlbl3.Focus();
                    return;
                }
            }
            else
            {

            }
        }

        string chkname = "";
        chkname = fgen.seek_iname(frm_qstr, frm_cocd, "select assetid from WB_FA_PUR where assetid='" + txtlbl4.Value.Trim() + "' and branchcd='" + frm_mbr + "' and type='10'", "assetid");
        if (chkname != "0" && edmode.Value != "Y")
        {
            fgen.msg("-", "AMSG", "Asset Id already exists in database !!"); txtlbl4.Focus(); flag = 1;
            return;
        }

        if (txtlbl4.Value.Length < 2)
        {
            fgen.msg("-", "AMSG", "Asset ID not entered !!"); txtlbl4.Focus(); flag = 1;
            return;
        }

        if (txtlbl8.Value.Length < 2)
        {

            fgen.msg("-", "AMSG", "Group code not entered !!"); txtlbl8.Focus(); flag = 1;
            return;
        }

        if (txtSup_by.Value.Length < 2)
        {

            fgen.msg("-", "AMSG", "Supplier Name not selected !!"); txtSup_by.Focus(); flag = 1;
            return;
        }

        if (txtlbl5.Value.Length < 2)
        {

            fgen.msg("-", "AMSG", "Invoice Date not entered !!"); txtlbl5.Focus(); flag = 1;
            return;
        }

        if (txtaname.Value.Length < 2)
        {
            fgen.msg("-", "AMSG", "Name of Asset not entered !!"); txtaname.Focus(); flag = 1;
            return;
        }
        if (txtinstalldate.Value.Length < 2)
        {
            fgen.msg("-", "AMSG", " Please Enter Install Date !!"); txtinstalldate.Focus(); flag = 1;
            return;
        }

        //if (Convert.ToDateTime(txtinstalldate.Value) > Convert.ToDateTime(frm_CDT2))
        //{
        //    fgen.msg("-", "AMSG", " Install Date cannot be greater than Financial Year end date !!"); txtinstalldate.Focus(); flag = 1;
        //    return;
        //}

        //if (Convert.ToDateTime(txtinstalldate.Value) < Convert.ToDateTime(frm_CDT1))
        //{
        //    fgen.msg("-", "AMSG", " Install Date cannot be less than Financial Year start date !!"); txtinstalldate.Focus(); flag = 1;
        //    return;
        //}

        if ((txt_life.Value.Length < 1) || (txt_life.Value == "-"))
        {
            fgen.msg("-", "AMSG", " Please enter the  life in years!!"); txt_life.Focus(); flag = 1;
            return;
        }
        if (txttotal_life.Value.Length < 2)
        {
            fgen.msg("-", "AMSG", " Please click on the icon to calculate total life !!"); txttotal_life.Focus(); flag = 1;
            return;
        }

        if ((txtdeprab_val.Value.Length < 1) || (txtdeprab_val.Value == "-"))
        {
            fgen.msg("-", "AMSG", " Please Enter Depreciation value !!"); txtdeprab_val.Focus(); flag = 1;
            return;
        }

        if (Convert.ToDouble(txtdeprab_val.Value) > Convert.ToDouble(txt_originalcost.Value))
        {
            fgen.msg("-", "AMSG", " Depreciation value cannot be more than Original cost !!"); txtdeprab_val.Focus(); flag = 1;
            return;

        }

        if (ddwarrantydays.Value.ToString() == "Y")
        {
            if (txt_warranty_date.Value.Length < 2)
            {
                fgen.msg("-", "AMSG", " Please Enter Warranty Date as warranty applicable is Yes. !!"); flag = 1; txt_warranty_date.Focus();
                return;
            }
            else
            {
                int dhd = fgen.ChkDate(txt_warranty_date.Value.ToString());
                if (dhd == 1)
                {
                    if (Convert.ToDateTime(txt_warranty_date.Value) < Convert.ToDateTime(txtinstalldate.Value))
                    {
                        fgen.msg("-", "AMSG", "Warranty Date cannot be less than Installation Date. !!"); flag = 1; txt_warranty_date.Focus();
                        return;
                    }
                }
                else
                {
                    fgen.msg("-", "AMSG", "Please Enter a Valid Warranty Date"); txt_warranty_date.Focus(); flag = 1; return;
                }
            }
        }

        //if (Convert.ToDateTime(txtinstalldate.Value) > System.DateTime.Now.Date)
        //{

        //    fgen.msg("-", "AMSG", " Install date cannot  be greater than current date !!"); txtinstalldate.Focus(); flag = 1;
        //    return;

        //}

        if (txtOpDep.Value.Length < 1 || txtOpDep.Value == "-")
        {
            fgen.msg("-", "AMSG", " Please Enter Opening Depreciation value or put 0 !!"); txtOpDep.Focus(); flag = 1;
            return;
        }


        if (txtadddepp.Value == "-" || txtadddepp.Value == "")
        {
            adddepcal = 1;
        }
        else
        {
            adddepcal = 1 + (fgen.make_double(txtadddepp.Value) / 100);
        }

        double deprpday = Math.Round((((fgen.make_double(txt_originalcost.Value) - fgen.make_double(txtresidual_value.Value)) / (fgen.make_int(txttotal_life.Value)) * fgen.make_int(txtQuantity.Value)) * adddepcal), 2);
        if (deprpday < 0)
        {
            txtdepr_perday.Value = "0";
        }
        else
        {
            txtdepr_perday.Value = deprpday.ToString();
        }

        ast_chk_flg = flag;
    }
}