using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_gst_Dtl : System.Web.UI.Page
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
    string frm_mbr, frm_vty, frm_fchar, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    string mhd, mhd1, mq0, mq1, mq2;
    string home_curr = "";
    string home_divider = "";
    string home_div_iden = "";
    string numbr_fmt = "";
    string numbr_fmt2 = "";
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
                    mhd = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                    mhd1 = System.DateTime.Now.Date.ToString("yyyy/MM");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                    //DateRange = "between to_date('01/01/2021','dd/mm/yyyy') and to_date('" + mhd + "','dd/mm/yyyy')";//rmv  this ...for testing only
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "update type set tvchnum=lpad(Trim(type1),6,'0') where trim(nvl(tvchnum,'-'))='-'");
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            //  setColHeadings();
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
            dtCol = fgen.getdata(frm_qstr, frm_cocd, "SELECT UPPER(OBJ_NAME) AS OBJ_NAME,OBJ_CAPTION,OBJ_WIDTH,UPPER(OBJ_VISIBLE) AS OBJ_VISIBLE,nvl(col_no,0) as COL_NO,nvl(OBJ_MAXLEN,0) as OBJ_MAXLEN,nvl(OBJ_READONLY,'N') as OBJ_READONLY,NVL(OBJ_FMAND,'N') AS OBJ_FMAND FROM SYS_CONFIG WHERE UPPER(TRIM(FRM_NAME))='" + frm_formID + "'");
        }
        ViewState["d" + frm_qstr + frm_formID] = dtCol;
    }
    //------------------------------------------------------------------------------------
    //void setColHeadings()
    //{
    //    dtCol = new DataTable();
    //    dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
    //    if (dtCol == null || dtCol.Rows.Count <= 0)
    //    {
    //        getColHeading();
    //    }
    //    dtCol = new DataTable();
    //    dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
    //    #region hide hidden columns
    //    sg1.Columns[0].Visible = false;
    //    sg1.Columns[1].Visible = false;
    //    sg1.Columns[2].Visible = false;
    //    sg1.Columns[3].Visible = false;
    //    sg1.Columns[4].Visible = false;
    //    sg1.Columns[5].Visible = false;
    //    sg1.Columns[6].Visible = false;
    //    sg1.Columns[7].Visible = false;
    //    sg1.Columns[8].Visible = false;
    //    sg1.Columns[9].Visible = false;
    //    #endregion
    //    if (dtCol == null) return;
    //    if (sg1.Rows.Count <= 0) return;
    //    for (int sR = 0; sR < sg1.Columns.Count; sR++)
    //    {
    //        string orig_name;
    //        double tb_Colm;
    //        tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
    //        orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();

    //        for (int K = 0; K < sg1.Rows.Count; K++)
    //        {
    //            if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));
    //            ((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("readonly", "readonly");
    //            ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("readonly", "readonly");
    //            ((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("readonly", "readonly");
    //        }

    //        orig_name = orig_name.ToUpper();
    //        //if (sg1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_NAME"))
    //        if (sR == tb_Colm)
    //        {
    //            // hidding column
    //            if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
    //            {
    //                sg1.Columns[sR].Visible = false;
    //            }
    //            // Setting Heading Name
    //            sg1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
    //            // Setting Col Width
    //            string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
    //            if (fgen.make_double(mcol_width) > 0)
    //            {
    //                sg1.HeaderRow.Cells[sR].Width = Convert.ToInt32(mcol_width);
    //                sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
    //            }
    //        }
    //    }

    //    //txtlbl2.Attributes.Add("readonly", "readonly");
    //    //txtlbl3.Attributes.Add("readonly", "readonly");
    //    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");


    //    //txtlbl5.Attributes.Add("readonly", "readonly");
    //    //txtlbl6.Attributes.Add("readonly", "readonly");

    //    //my_Tabs
    //    //txtlbl2.Attributes["required"] = "true";
    //    //txtlbl2.BackColor = System.Drawing.ColorTranslator.FromHtml("#E0FF00");
    //    // to hide and show to tab panel

    //    tab2.Visible = false;
    //    tab3.Visible = false;
    //    tab4.Visible = false;
    //    tab5.Visible = false;
    //    tab6.Visible = false;

    //    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
    //    switch (Prg_Id)
    //    {

    //        case "M12008":
    //            tab3.Visible = false;
    //            tab4.Visible = false;
    //            break;
    //        case "F60161":
    //            //AllTabs.Visible = false;
    //            break;
    //    }
    //    tab1.Visible = true;
    //    tab2.Visible = false;
    //    tab3.Visible = false;
    //    tab4.Visible = false;
    //    tab5.Visible = false;

    //    fgen.SetHeadingCtrl(this.Controls, dtCol);

    //}
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false;
        btnprint.Disabled = false;
        create_tab();
        //  create_tab2();
        //create_tab3();
        //create_tab4();

        sg1_add_blankrows();
        //  sg2_add_blankrows();
        //sg3_add_blankrows();
        //sg4_add_blankrows();

        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
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
        if (numbr_fmt2 == "" || numbr_fmt2 == null)
        {
            home_curr = "Rs";
            home_divider = "100000";
            home_div_iden = "Lakh";
            numbr_fmt = "" + numbr_fmt2 + "";
            numbr_fmt2 = "999,999,999";

            {
                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT BR_CURREN||'~'||'1000'||'~'||'000'||'~'||NUM_FMT1||'~'||NUM_FMT2 AS FSTR FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "' ", "FSTR");
                if (mhd != "0")
                {
                    home_curr = (mhd.Split('~')[0] == "" || mhd.Split('~')[0] == "-" || mhd.Split('~')[0] == "0") ? home_curr : mhd.Split('~')[0];
                    home_divider = (mhd.Split('~')[1] == "" || mhd.Split('~')[1] == "-" || mhd.Split('~')[1] == "0") ? home_divider : mhd.Split('~')[1];
                    home_div_iden = (mhd.Split('~')[2] == "" || mhd.Split('~')[2] == "-" || mhd.Split('~')[2] == "0") ? home_div_iden : mhd.Split('~')[2];
                    numbr_fmt = (mhd.Split('~')[3] == "" || mhd.Split('~')[3] == "-" || mhd.Split('~')[3] == "0") ? numbr_fmt : mhd.Split('~')[3];
                    numbr_fmt2 = (mhd.Split('~')[4] == "" || mhd.Split('~')[4] == "-" || mhd.Split('~')[4] == "0") ? numbr_fmt2 : mhd.Split('~')[4];
                }
            }
        }
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        string comb_char;
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");
        comb_char = frm_vty;

        if (frm_fchar != "%")
        {
            comb_char = frm_vty + frm_fchar;
        }
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        if (CSR.Length > 1) cond = " and trim(a.ccode)='" + cond + "'";
        switch (btnval)
        {
            case "TYPECODE":
                if (comb_char.Length > 1)
                {
                    SQuery = "select * from (select sum(Valu) as fstr,coded as Code_No_Available,max(name) as Code_Name,(case when sum(Valu)>0 then 'Code Available' else 'Code Already Used' end) as Code_Status from (select lpad(trim(to_char(rownum,'99')),2,'0') as coded,1 as Valu,null as name from (select rowid,rownum from FIN_MSYS order by id) where rownum<100 union all select type1,-1 as coded,name from type where id='" + frm_vty + "' and type1 like '" + frm_fchar + "%') group by coded ) where Code_No_Available like '" + frm_fchar + "%' order by Code_No_Available ";
                }
                break;
            case "SALE_INV_BTN1":
            case "SALE_HSN_BTN2":
            case "SALE_HSN_BTN3":
            case "SALE_INV_BTN4":
            case "SALE_INV_BTN5":
                SQuery = "select TRIM(type1) as fstr, trim(type1) as code,trim(gst_no) as gstno ,trim(name) as name from type where id='B'";
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
                frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");
                SQuery = "select distinct a.type1 as fstr,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt,a.Name,a.type1 as Code,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where  a.tbranchcd='00' and trim(a.ID)='" + frm_vty + "' and trim(a.type1) like '" + frm_fchar + "%'  order by a.type1";
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
        frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");
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
                //txtlbl4.Value = CSR;
                //txtlbl4.Disabled = true;
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    void newCase(string vty)
    {
    }
    //------------------------------------------------------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        if (frm_mbr != "00")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please operate this Option from Plant Code 00 !!");
            return;
        }

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
        if (frm_mbr != "00")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please operate this Option from Plant Code 00 !!");
            return;
        }

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Value.ToString());
        if (dhd == 0) { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
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
        //create_tab3();
        //create_tab4();

        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();

        //sg2_add_blankrows();
        //sg2.DataSource = sg2_dt;
        //sg2.DataBind();
        //if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();

        //sg3_add_blankrows();
        //sg3.DataSource = sg3_dt;
        //sg3.DataBind();
        //if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();

        //sg4_add_blankrows();
        //sg4.DataSource = sg4_dt;
        //sg4.DataBind();
        //if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();


        ViewState["sg1"] = null;
        // ViewState["sg2"] = null;
        //ViewState["sg3"] = null;
        //ViewState["sg4"] = null;
        // setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");

        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        SQuery = "select distinct a.type1 as Type_Code,a.Name,a.Acode,a.type1 as Code,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt from " + frm_tabname + " a where  a.tbranchcd='00' and trim(a.ID)='" + frm_vty + "' and trim(a.type1) like '" + frm_fchar + "%'  order by a.type1";
        switch (Prg_Id)
        {
            case "F10101":
                SQuery = "select distinct a.type1 as Item_Grp_Code,a.Name as Item_Grp_Name,a.Acode as Acctg_Code,a.type1 as Type_Code,b.aname as Acctg_Name,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,a.MEdt_by,to_char(a.Medt_Dt,'dd/mm/yyyy') as Edt_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt from " + frm_tabname + " a left outer join famst b on trim(A.acode)=trim(B.acode) where a.tbranchcd='00' and trim(a.ID)='" + frm_vty + "' and trim(a.type1) like '" + frm_fchar + "%'  order by a.type1";
                break;
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
        hffield.Value = "-";


    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F10101");
        string header_n = "Item Group List";
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

        frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {
            string mqry = "";
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (frm_vty == "Y")
            {
                mqry = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as fstr from item where substr(icode,1,2)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'", "fstr");
                if (fgen.make_double(mqry.ToString()) > 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", " + mqry + " Items Opened under this Group , Deletion not Permitted !!");
                    return;
                }

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
                case "SALE_INV_BTN1":
                case "SALE_HSN_BTN2":
                case "SALE_HSN_BTN3":
                case "SALE_INV_BTN4":
                case "SALE_INV_BTN5":
                    hf2.Value = col1;
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;

            }
        }
    }
    //---------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");

        string xprd1 = "", xprd2 = "", xprdrange = "", xbstring = "";

        xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");

        xbstring = "branchcd='" + frm_mbr + "'";

        string sumdtl = "S";
        string repvty = "";
        string repamtfld = "";
        string repacode = "substr(b.grp,1,1)='2'";

        ////switch (val)
        ////{

        ////    case "F70710":
        ////        my_rep_head = "VAT/GST Payable Summary for the period " + value1 + " To " + value2 + "";
        ////        sumdtl = "S";
        ////        break;
        ////    case "F70712":
        ////        break;
        ////    case "F70712A":
        ////        my_rep_head = "VAT/GST Payable (Item Wise Details) for the period " + value1 + " To " + value2 + "";
        ////        sumdtl = "D";
        ////        break;
        ////    case "F70713":
        ////        my_rep_head = "VAT/GST Payable Details for the period " + value1 + " To " + value2 + "";
        ////        sumdtl = "D";
        ////        break;

        ////    case "F70714":
        ////        my_rep_head = "VAT/GST Receivable Summary for the period " + value1 + " To " + value2 + "";
        ////        repvty = "5";
        ////        repamtfld = "Dramt";

        ////        repacode = "substr(b.grp,1,1) in ('3','4','5')";

        ////        sumdtl = "S";
        ////        break;
        ////    case "F70716":
        ////        my_rep_head = "VAT/GST Receivable Details for the period " + value1 + " To " + value2 + "";
        ////        sumdtl = "D";
        ////        repamtfld = "Dramt";
        ////        repacode = "substr(b.grp,1,1) in ('3','4','5')";
        ////        repvty = "5";
        ////        break;

        ////    case "F70716A":
        ////        my_rep_head = "VAT/GST Receivable (Item Wise Details) for the period " + value1 + " To " + value2 + "";
        ////        repvty = "5";
        ////        repacode = "substr(b.grp,1,1) in ('3','4','5')";
        ////        repamtfld = "Dramt";
        ////        sumdtl = "D";
        ////        break;
        ////    case "F70717":
        ////        my_rep_head = "VAT/GST Receivable Summary for the period " + value1 + " To " + value2 + "";
        ////        repvty = "5";
        ////        repamtfld = "Dramt";
        ////        repacode = "substr(b.grp,1,1) in ('3','4','5')";
        ////        break;

        ////}
        string my_rep_head = "";

        string txcode1 = "", txcode2 = "", txcode3 = "", txcode4 = "", txcode5 = "", txcode6 = "";

        ////xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
        ////txcode1 = fgen.getOption(frm_qstr, frm_cocd, (repvty == "4" ? "W0083" : "W0084"), "OPT_PARAM"); //fin_Rsys_opt TABLE FOR THIS CONTROL....

        ////if (hom_ctry == "INDIA")
        ////{
        ////    if (repvty == "5")
        ////    {
        ////        txcode1 = fgen.seek_iname(frm_qstr, co_cd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0080'", "OPT_PARAM");
        ////        txcode2 = fgen.seek_iname(frm_qstr, co_cd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0081'", "OPT_PARAM");
        ////        txcode3 = fgen.seek_iname(frm_qstr, co_cd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0082'", "OPT_PARAM");
        ////    }
        ////    else
        ////    {
        //}
        //else
        //{
        //    if (repvty == "5")
        //    {
        //        txcode1 = fgen.seek_iname(frm_qstr, co_cd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0084'", "OPT_PARAM");
        //        txcode2 = fgen.seek_iname(frm_qstr, co_cd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W008X'", "OPT_PARAM");
        //        txcode3 = fgen.seek_iname(frm_qstr, co_cd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W008X'", "OPT_PARAM");
        //        mq0 = "select * from (Select a.type,a.rcode as acode," + repamtfld + " as Sale_amt,0 as CGST,0 as SGST,0 as IGST,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.invno,a.invdate,a.vchnum,a.vchdate from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprd2 + " and trim(a.acode) not in ('" + txcode1 + "','" + txcode2 + "','" + txcode3 + "') union all Select a.type,null as acode,0 as Sale_amt," + repamtfld + " as CGST,0 as SGST,0 as IGST,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.invno,a.invdate,a.vchnum,a.vchdate from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprd2 + " and a.acode = '" + txcode1.Trim() + "' union all Select a.type,null as acode,0 as Sale_amt,0 as CGST," + repamtfld + " as SGST,0 as IGST,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.invno,a.invdate,a.vchnum,a.vchdate from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprd2 + " and a.acode = '" + txcode2.Trim() + "' union all Select a.type,null as acode,0 as Sale_amt,0 as CGST,0 as SGST," + repamtfld + " as IGST,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.invno,a.invdate,a.vchnum,a.vchdate from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprd2 + " and a.acode = '" + txcode3.Trim() + "') where nvl(Sale_amt,0)+nvl(CGST,0)+nvl(SGST,0)+nvl(IGST,0)<>0 ";

        //        mq1 = "select '-' as fstr,'-' as gstr,'-' as type,'Totals' as Party_Code,sum(a.Sale_amt) as Sale_Amount,sum(a.CGST) as CGST_Amount,sum(a.sgst) as SGST_Amount,sum(a.IGST) as IGST_Amount,'-' as invno,null as invdate,'-' as vchnum,null as vchdate,'DS1' as DataSet from (" + mq0 + ") a  union all select '-' as fstr,'-' as gstr,a.type,max(a.acode) as Party_Code,sum(a.Sale_amt) as Sale_Amount,sum(a.CGST) as CGST_Amount,sum(a.sgst) as SGST_Amount,sum(a.IGST) as IGST_Amount,a.invno,a.invdate,a.vchnum,a.vchdate,'DS2' as DataSet from (" + mq0 + ") a group by a.fstr,a.type,a.invno,a.invdate,a.vchnum,a.vchdate";

        //    }
        //    else
        //    {
        //        txcode1 = fgen.seek_iname(frm_qstr, co_cd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0083'", "OPT_PARAM");
        //        txcode2 = fgen.seek_iname(frm_qstr, co_cd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W007X'", "OPT_PARAM");
        //        txcode3 = fgen.seek_iname(frm_qstr, co_cd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W007X'", "OPT_PARAM");
        //        mq0 = "select * from (Select a.type,a.rcode as acode,(case when substr(type,1,1)='4' then " + repamtfld + " else -1*dramt end) as Sale_amt,0 as CGST,0 as SGST,0 as IGST,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.invno as vchnum,a.vchdate from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and (a.type like '" + repvty + "%' or a.type in ('53','58')) and a.vchdate " + xprd2 + " and trim(a.acode) not in ('" + txcode1 + "','" + txcode2 + "','" + txcode3 + "') union all Select a.type,null as acode,0 as Sale_amt,(case when substr(type,1,1)='4' then " + repamtfld + " else -1*dramt end) as CGST,0 as SGST,0 as IGST,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.invno as vchnum,vchdate from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and (a.type like '" + repvty + "%' or a.type in ('53','58')) and a.vchdate " + xprd2 + " and a.acode = '" + txcode1.Trim() + "' union all Select a.type,null as acode,0 as Sale_amt,0 as CGST,(case when substr(type,1,1)='4' then " + repamtfld + " else -1*dramt end) as SGST,0 as IGST,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.invno as vchnum,vchdate from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and (a.type like '" + repvty + "%' or a.type in ('53','58')) and a.vchdate " + xprd2 + " and a.acode = '" + txcode2.Trim() + "' union all Select a.type,null as acode,0 as Sale_amt,0 as CGST,0 as SGST,(case when substr(type,1,1)='4' then " + repamtfld + " else -1*dramt end) as IGST,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.invno as vchnum,vchdate from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + mbr + "' and (a.type like '" + repvty + "%' or a.type in ('53','58')) and a.vchdate " + xprd2 + " and a.acode = '" + txcode3.Trim() + "') where nvl(Sale_amt,0)+nvl(CGST,0)+nvl(SGST,0)+nvl(IGST,0)<>0 ";

        //        mq1 = "select '-' as fstr,'-' as gstr,'-' as type,'Totals' as Party_Code,sum(a.Sale_amt) as Sale_Amount,sum(a.CGST) as CGST_Amount,sum(a.sgst) as SGST_Amount,sum(a.IGST) as IGST_Amount,'-' as vchnum,null as vchdate,'DS1' as DataSet from (" + mq0 + ") a  union all select '-' as fstr,'-' as gstr,a.type,max(a.acode) as Party_Code,sum(a.Sale_amt) as Sale_Amount,sum(a.CGST) as CGST_Amount,sum(a.sgst) as SGST_Amount,sum(a.IGST) as IGST_Amount,a.vchnum,a.vchdate,'DS2' as DataSet from (" + mq0 + ") a group by a.fstr,a.type,a.vchnum,a.vchdate";

        //    }


        //}
        string tds_ac1 = "", tds_ac2 = "", tds_ac3 = "", tds_ac4 = "", tds_ac5 = "", tds_ac6 = "", tds_ac7 = "";

        tds_ac1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_PARAM FROM FIN_RSYS_OPT WHERE UPPER(TRIM(OPT_ID))='W0194'", "OPT_PARAM");
        tds_ac2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_PARAM FROM FIN_RSYS_OPT WHERE UPPER(TRIM(OPT_ID))='W0195'", "OPT_PARAM");
        tds_ac3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_PARAM FROM FIN_RSYS_OPT WHERE UPPER(TRIM(OPT_ID))='W0196'", "OPT_PARAM");
        tds_ac4 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_PARAM FROM FIN_RSYS_OPT WHERE UPPER(TRIM(OPT_ID))='W0197'", "OPT_PARAM");
        tds_ac5 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_PARAM FROM FIN_RSYS_OPT WHERE UPPER(TRIM(OPT_ID))='W0198'", "OPT_PARAM");
        tds_ac6 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_PARAM FROM FIN_RSYS_OPT WHERE UPPER(TRIM(OPT_ID))='W0199'", "OPT_PARAM");
        tds_ac7 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_PARAM FROM FIN_RSYS_OPT WHERE UPPER(TRIM(OPT_ID))='W0115'", "OPT_PARAM");

        switch (hffield.Value)
        {

            case "PUR_INV_BTN1":

                repvty = "5";
                sumdtl = "D";
                repamtfld = "Dramt";
                txcode1 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0080'", "OPT_PARAM");
                txcode2 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0081'", "OPT_PARAM");
                txcode3 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0082'", "OPT_PARAM");

                txcode4 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0123'", "OPT_PARAM");
                txcode5 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0124'", "OPT_PARAM");

                mq0 = "select branchcd,type,max(acode) as acode1,sum(Sale_amt)as Sale_amt,sum(CGST) As CGST,sum(SGST)AS SGST,sum(IGST) As IGST,sum(tcsamt) as tcsamt,fstr,vchnum,vchdate,invno,invdate from (Select a.branchcd,a.type,a.rcode as acode," + repamtfld + " as Sale_amt,0 as CGST,0 as SGST,0 as IGST,0 as tcsamt,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and  trim(A.acode) not in ('" + txcode1.Trim() + "','" + txcode2.Trim() + "','" + txcode3.Trim() + "','" + txcode4.Trim() + "','" + txcode5.Trim() + "','" + tds_ac1.Trim() + "','" + tds_ac2.Trim() + "','" + tds_ac3.Trim() + "','" + tds_ac4.Trim() + "','" + tds_ac5.Trim() + "','" + tds_ac6.Trim() + "','" + tds_ac7.Trim() + "') and trim(A.Rcode) not in ('" + tds_ac1.Trim() + "','" + tds_ac2.Trim() + "','" + tds_ac3.Trim() + "','" + tds_ac4.Trim() + "','" + tds_ac5.Trim() + "','" + tds_ac6.Trim() + "','" + tds_ac7.Trim() + "') and a.Dramt>0 union all Select a.branchcd,a.type,null as acode,0 as Sale_amt," + repamtfld + " as CGST,0 as SGST,0 as IGST,0 as tcsamt,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode1.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST," + repamtfld + " as SGST,0 as IGST,0 as tcsamt,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode2.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST,0 as SGST," + repamtfld + " as IGST,0 as tcsamt,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode3.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST,0 as SGST,0 as IGST," + repamtfld + " as TCSAMT,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode in ('" + txcode4.Trim() + "','" + txcode5.Trim() + "')) group by branchcd,type,fstr,vchnum,vchdate,invno,invdate having sum(Sale_amt)+sum(CGST)+sum(SGST)+sum(IGST)+sum(tcsamt)>0 ";

                mq1 = "select '-' as fstr,'-' as gstr,c.Name as Type_Name,b.Aname,b.staffcd as StateCode,b.GST_No,a.Branchcd,a.type,a.Invno,to_char(a.Invdate,'dd/mm/yyyy') as Inv_Date,(a.Sale_amt)+(a.CGST)+(a.sgst)+(a.IGST) as Inv_value_wout_tcs,trim(b.staffcd)||'-'||b.staten as State_Code,trim(b.staffcd)||'-'||b.staten as Place_Supp,b.GSTREVCHG,'Regular' as Inv_type,'-' as Ecom_no,(case when (a.Sale_amt)>0 then round(((a.CGST+a.sgst+a.IGST)/(a.Sale_amt))*100,2) else 0 end) as TAX_Rate,(a.Sale_amt) as Taxable_Amount,(a.CGST) as CGST_Amount,(a.sgst) as SGST_Amount,(a.IGST) as IGST_Amount,'-' as Sb_no,'-' as Sb_dt,'-' as port_cd,(a.TCSAMT) as TCS_Amount,(a.Sale_amt)+(a.CGST)+(a.sgst)+(a.IGST)+(a.TCSAMT) as Grand_Total,a.acode1 as Party_Code,a.Vchnum,to_char(a.vchdate,'dd/mm/yyyy') as Vch_date from (" + mq0 + ") a,famst b,type c where a.type!='58' and a.type!='59' and a.type=c.type1 and c.id='V' and trim(a.type)=trim(c.type1)  and trim(a.acode1)=trim(b.acode)  order by a.vchdate,a.vchnum ";

                fgen.drillQuery(0, mq1, frm_qstr);
                fgen.Fn_DrillReport("Purchase Invoice Wise Data During  " + fromdt + " To " + todt + "  ", frm_qstr);

                hffield.Value = "-";

                break;

            case "PUR_INV_BTN2":

                mq1 = "Select a.vchnum as Inv_No,to_Char(a.vchdate,'dd/mm/yyyy') as Inv_Dt,a.aname as Supplier,a.gst_no as GST_No,a.staten as State_Name,a.St_code as State_Code,sum(a.iqtyin)as Qty_tot,a.unit,sum(a.iamount) as Basic_Val,sum(a.Tool_Cost) as Tool_Cost,sum(a.pack_Cost) as pack_Cost,sum(a.frt_Cost) as frt_Cost,a.HSCODE,a.CGST_RT,sum(a.CGST_amt) as CGST_amt,a.SGST_Rate,sum(a.SGST_amt) as SGST_amt,a.IGST_Rt,sum(a.IGST_amt) as IGST_amt,a.Invno as Long_Inv_No,a.Invdate as Ref_Inv_Dt,a.type,b.bill_Tot,(case when length(Trim(a.gst_no))=15 then 'B2B' when length(Trim(a.gst_no))<15 and b.bill_tot>=250000 then 'B2CL'  when length(Trim(a.gst_no))<15 and b.bill_tot<250000 then 'B2CS' else '-' end) as GST_Catg,a.branchcd,a.pl_gst_no as plant_gst_no,a.plnm as Plant_name,to_Char(a.vchdate,'yyyymmdd') as VDD from (Select a.branchcd,A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyin,a.irate,a.ichgs as Disc,a.iamount,round(a.iqtyin*a.iexc_Addl,2) as Tool_Cost,round(a.iqtyin*a.ipack,2) as pack_Cost,round(a.iqtyin*a.idiamtr,2) as frt_Cost,(Case when trim(A.UNIT)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.UNIT)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.UNIT)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.Unit)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.Unit)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.unit)='IG' then a.exc_amt else 0 end) as IGST_amt,a.Invno,A.Invdate,a.type,c.unit,d.gst_no as pl_gst_no,d.name as plnm from ivoucher a, famst b , item c,type d where a.branchcd=d.type1 and d.id='B' and trim(a.acode)=trim(b.acode) and  trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate  " + xprdrange + " and a.store in ('Y','N') order by a.vchdate,a.vchnum,a.morder) a, ivchctrl b where trim(a.branchcd)||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||b.type||b.vchnum||to_Char(b.vchdate,'dd/mm/yyyy')  group by a.plnm,a.pl_gst_no,a.branchcd,a.vchnum,a.vchdate,a.aname,a.gst_no,a.staten,a.hscode,a.St_code,a.CGST_RT,a.SGST_Rate,a.IGST_Rt,a.type,a.invno,a.invdate,a.unit,b.bill_Tot,to_Char(a.vchdate,'yyyymmdd') order by VDD,a.vchnum";

                fgen.drillQuery(0, mq1, frm_qstr);
                fgen.Fn_DrillReport("Purchase Invoice Wise HSN Wise Data During  " + fromdt + " To " + todt + "  ", frm_qstr);

                hffield.Value = "-";
                break;
            case "PUR_INV_BTN3":

                mq1 = "Select a.HSCODE,sum(a.iqtyin)as Qty_tot,a.unit,sum(a.iamount) as Basic_Val,sum(a.Tool_Cost) as Tool_Cost,sum(a.pack_Cost) as pack_Cost,sum(a.frt_Cost) as frt_Cost,a.CGST_RT,sum(a.CGST_amt) as CGST_amt,a.SGST_Rate,sum(a.SGST_amt) as SGST_amt,a.IGST_Rt,sum(a.IGST_amt) as IGST_amt,a.type,a.branchcd,a.pl_gst_no as plant_gst_no,a.plnm as Plant_name from (Select a.branchcd,A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyin,a.irate,a.ichgs as Disc,a.iamount,round(a.iqtyin*a.iexc_Addl,2) as Tool_Cost,round(a.iqtyin*a.ipack,2) as pack_Cost,round(a.iqtyin*a.idiamtr,2) as frt_Cost,(Case when trim(A.Unit)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.Unit)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.Unit)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.Unit)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.Unit)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.Unit)='IG' then a.exc_amt else 0 end) as IGST_amt,a.Invno,A.Invdate,a.type,c.unit,d.gst_no as pl_gst_no,d.name as plnm from ivoucher a, famst b , item c,type d where a.branchcd=d.type1 and d.id='B' and trim(a.acode)=trim(b.acode) and  trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate  " + xprdrange + " and a.store in ('Y','N') order by a.vchdate,a.vchnum,a.morder) a, ivchctrl b where trim(a.branchcd)||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||b.type||b.vchnum||to_Char(b.vchdate,'dd/mm/yyyy')  group by a.plnm,a.pl_gst_no,a.branchcd,a.hscode,a.CGST_RT,a.SGST_Rate,a.IGST_Rt,a.type,a.unit order by a.hscode";

                fgen.drillQuery(0, mq1, frm_qstr);
                fgen.Fn_DrillReport("HSN Wise Purchase Data During  " + fromdt + " To " + todt + "  ", frm_qstr);

                hffield.Value = "-";
                break;

            case "PUR_INV_BTN4":

                repvty = "5";
                sumdtl = "D";
                repamtfld = "Dramt";
                txcode1 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0137'", "OPT_PARAM");
                txcode2 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0138'", "OPT_PARAM");
                txcode3 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0139'", "OPT_PARAM");

                txcode4 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0123'", "OPT_PARAM");
                txcode5 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0124'", "OPT_PARAM");

                mq0 = "select branchcd,type,max(acode) as acode1,sum(Sale_amt)as Sale_amt,sum(CGST) As CGST,sum(SGST)AS SGST,sum(IGST) As IGST,sum(tcsamt) as tcsamt,fstr,vchnum,vchdate,invno,invdate from (Select a.branchcd,a.type,a.rcode as acode," + repamtfld + " as Sale_amt,0 as CGST,0 as SGST,0 as IGST,0 as tcsamt,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and  trim(A.acode) not in ('" + txcode1.Trim() + "','" + txcode2.Trim() + "','" + txcode3.Trim() + "','" + txcode4.Trim() + "','" + txcode5.Trim() + "','" + tds_ac1.Trim() + "','" + tds_ac2.Trim() + "','" + tds_ac3.Trim() + "','" + tds_ac4.Trim() + "','" + tds_ac5.Trim() + "','" + tds_ac6.Trim() + "','" + tds_ac7.Trim() + "') and trim(A.Rcode) not in ('" + tds_ac1.Trim() + "','" + tds_ac2.Trim() + "','" + tds_ac3.Trim() + "','" + tds_ac4.Trim() + "','" + tds_ac5.Trim() + "','" + tds_ac6.Trim() + "','" + tds_ac7.Trim() + "') and a.Dramt>0 union all Select a.branchcd,a.type,null as acode,0 as Sale_amt," + repamtfld + " as CGST,0 as SGST,0 as IGST,0 as tcsamt,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode1.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST," + repamtfld + " as SGST,0 as IGST,0 as tcsamt,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode2.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST,0 as SGST," + repamtfld + " as IGST,0 as tcsamt,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode3.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST,0 as SGST,0 as IGST," + repamtfld + " as TCSAMT,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode in ('" + txcode4.Trim() + "','" + txcode5.Trim() + "')) group by branchcd,type,fstr,vchnum,vchdate,invno,invdate having sum(Sale_amt)+sum(CGST)+sum(SGST)+sum(IGST)+sum(tcsamt)>0 ";

                mq1 = "select '-' as fstr,'-' as gstr,c.Name as Type_Name,b.Aname,b.staffcd as StateCode,b.GST_No,a.Branchcd,a.type,a.Invno,to_char(a.Invdate,'dd/mm/yyyy') as Inv_Date,(a.Sale_amt)+(a.CGST)+(a.sgst)+(a.IGST) as Inv_value_wout_tcs,trim(b.staffcd)||'-'||b.staten as State_Code,trim(b.staffcd)||'-'||b.staten as Place_Supp,b.GSTREVCHG,'Regular' as Inv_type,'-' as Ecom_no,(case when (a.Sale_amt)>0 then round(((a.CGST+a.sgst+a.IGST)/(a.Sale_amt))*100,2) else 0 end) as TAX_Rate,(a.Sale_amt) as Taxable_Amount,(a.CGST) as CGST_Amount,(a.sgst) as SGST_Amount,(a.IGST) as IGST_Amount,'-' as Sb_no,'-' as Sb_dt,'-' as port_cd,(a.TCSAMT) as TCS_Amount,(a.Sale_amt)+(a.CGST)+(a.sgst)+(a.IGST)+(a.TCSAMT) as Grand_Total,a.acode1 as Party_Code,a.Vchnum,to_char(a.vchdate,'dd/mm/yyyy') as Vch_date from (" + mq0 + ") a,famst b,type c where upper(nvl(b.gstoversea,'-'))<>'Y' and a.type=c.type1 and c.id='V' and a.type in ('57','5B') and trim(a.type)=trim(c.type1)  and trim(a.acode1)=trim(b.acode)  order by a.vchdate,a.vchnum ";

                fgen.drillQuery(0, mq1, frm_qstr);
                fgen.Fn_DrillReport("Purchase Invoice Wise Data During  " + fromdt + " To " + todt + "  ", frm_qstr);

                hffield.Value = "-";

                break;
            case "PUR_INV_BTN5":

                mq1 = "Select nvl(a.finvno,'-') as Ac_Vch,A.vchnum,a.vchdate,b.aname,nvl(b.gst_no,'-') as gst_no,nvl(b.staten,'-') as staten,b.staffcd as St_code,a.invno,a.invdate,c.iname,c.hscode,(case when nvl(a.iqtyin,0)=0 then a.iqty_chl else a.iqtyin end) as iqtyin,a.iqty_Wt,a.irate,a.iamount,round((case when nvl(a.iqtyin,0)=0 then a.iqty_chl else a.iqtyin end)*a.irate,2) as QtyXRate,nvl(a.ichgs,0) as L_cost,round((case when nvl(a.iqtyin,0)=0 then a.iqty_chl else a.iqtyin end)*nvl(a.ichgs,0),2) as QtyXLC,round(a.exp_punit,2) as Txb_Chg,a.iexc_Addl as tamtz,a.unit as TX_type,(Case when trim(A.Unit)='CG' or trim(A.IOPR)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.Unit)='CG' or trim(A.IOPR)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.Unit)='CG' or trim(A.IOPR)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.Unit)='CG' or trim(A.IOPR)='CG' then a.cess_pu else 0 end) as SGST_amt, (Case when trim(A.Unit)='IG' or trim(A.IOPR)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.Unit)='IG' or trim(A.IOPR)='IG' then a.exc_amt else 0 end) as IGST_amt,a.icode,a.type,nvl(a.Location,'-') as portcode,a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acodE)||trim(a.icode) as fstr,nvl(c.unit,'-') as unit,nvl(Txb_punit,0) as ImpTxbVal,nvl(a.store,'-') as store,a.branchcd from ivoucher a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and (a.type like '0%' or a.type in ('5A','5B','57') )  and a.vchdate " + xprdrange + " and nvl(a.store,'-')!='R' and a.type!='08' order by a.vchdate,a.vchnum,a.srno ";

                //mq1 = "Select a.HSCODE,sum(a.iqtyin)as Qty_tot,a.unit,sum(a.iamount) as Basic_Val,sum(a.Tool_Cost) as Tool_Cost,sum(a.pack_Cost) as pack_Cost,sum(a.frt_Cost) as frt_Cost,a.CGST_RT,sum(a.CGST_amt) as CGST_amt,a.SGST_Rate,sum(a.SGST_amt) as SGST_amt,a.IGST_Rt,sum(a.IGST_amt) as IGST_amt,a.type,a.branchcd,a.pl_gst_no as plant_gst_no,a.plnm as Plant_name from (Select a.branchcd,A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyin,a.irate,a.ichgs as Disc,a.iamount,round(a.iqtyin*a.iexc_Addl,2) as Tool_Cost,round(a.iqtyin*a.ipack,2) as pack_Cost,round(a.iqtyin*a.idiamtr,2) as frt_Cost,(Case when trim(A.Unit)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.Unit)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.Unit)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.Unit)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.Unit)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.Unit)='IG' then a.exc_amt else 0 end) as IGST_amt,a.Invno,A.Invdate,a.type,c.unit,d.gst_no as pl_gst_no,d.name as plnm from ivoucher a, famst b , item c,type d where a.branchcd=d.type1 and d.id='B' and trim(a.acode)=trim(b.acode) and  trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate  " + xprdrange + " and a.store in ('Y','N') order by a.vchdate,a.vchnum,a.morder) a, ivchctrl b where trim(a.branchcd)||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||b.type||b.vchnum||to_Char(b.vchdate,'dd/mm/yyyy')  group by a.plnm,a.pl_gst_no,a.branchcd,a.hscode,a.CGST_RT,a.SGST_Rate,a.IGST_Rt,a.type,a.unit order by a.hscode";

                fgen.drillQuery(0, mq1, frm_qstr);
                fgen.Fn_DrillReport("HSN Wise Purchase Data During  " + fromdt + " To " + todt + "  ", frm_qstr);

                hffield.Value = "-";
                break;

            case "SALE_INV_BTN1":
                repvty = "4";
                sumdtl = "D";
                repamtfld = "cramt";
                txcode1 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0077'", "OPT_PARAM");
                txcode2 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0078'", "OPT_PARAM");
                txcode3 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0079'", "OPT_PARAM");

                txcode4 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0123'", "OPT_PARAM");
                txcode5 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0124'", "OPT_PARAM");
                txcode6 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0114'", "OPT_PARAM");
                string prefix = "";
                if (frm_cocd == "MASS" || frm_cocd == "MAST")
                {
                    prefix = "MAS/" + fgen.make_int(frm_CDT1.Split('/')[2].Substring(2, 2)) + "-" + (fgen.make_int(frm_CDT1.Split('/')[2].Substring(2, 2)) + 1) + "/";
                }

                mq0 = "select branchcd,type,max(acode) as acode1,sum(Sale_amt)as Sale_amt,sum(CGST) As CGST,sum(SGST)AS SGST,sum(IGST) As IGST,sum(tcsamt) as tcsamt,sum(tool_amtz) as tool_amtz,fstr,vchnum,vchdate,'" + prefix + "'||invno as invno,invdate from (Select a.branchcd,a.type,a.acode,0 as Sale_amt,0 as CGST,0 as SGST,0 as IGST,0 as tcsamt,round(iqtyout*nvl(iexc_addl,0),2) as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from ivoucher a where a.branchcd in (" + hf2.Value + ") and a.type like '4%' and a.vchdate " + xprdrange + " union all Select a.branchcd,a.type,a.rcode as acode," + repamtfld + " as Sale_amt,0 as CGST,0 as SGST,0 as IGST,0 as tcsamt,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd in (" + hf2.Value + ") and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and  trim(A.acode) not in ('" + txcode1.Trim() + "','" + txcode2.Trim() + "','" + txcode3.Trim() + "','" + txcode4.Trim() + "','" + txcode5.Trim() + "','" + txcode6.Trim() + "') and a.cramt>0 union all Select a.branchcd,a.type,null as acode,0 as Sale_amt," + repamtfld + " as CGST,0 as SGST,0 as IGST,0 as tcsamt,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd in (" + hf2.Value + ") and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode1.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST," + repamtfld + " as SGST,0 as IGST,0 as tcsamt,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd in (" + hf2.Value + ")  and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode2.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST,0 as SGST," + repamtfld + " as IGST,0 as tcsamt,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd in (" + hf2.Value + ") and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode3.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST,0 as SGST,0 as IGST," + repamtfld + " as TCSAMT,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate from voucher a where a.branchcd in (" + hf2.Value + ") and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode in ('" + txcode4.Trim() + "','" + txcode5.Trim() + "')) group by branchcd,type,fstr,vchnum,vchdate,invno,invdate having sum(Sale_amt)+sum(CGST)+sum(SGST)+sum(IGST)+sum(tcsamt)>0 ";
                //
                mq1 = "select '-' as fstr,'-' as gstr,c.Name as Type_Name,b.Aname,b.staffcd as StateCode,b.GST_No,a.Branchcd,a.type,a.Invno,to_char(a.Invdate,'dd/mm/yyyy') as Inv_Date,(a.Sale_amt)+(a.CGST)+(a.sgst)+(a.IGST) as Inv_value_wout_tcs,trim(b.staffcd)||'-'||b.staten as State_Code,trim(b.staffcd)||'-'||b.staten as Place_Supp,b.GSTREVCHG,'Regular' as Inv_type,'-' as Ecom_no,TRUNC((case when (a.Sale_amt)>0 then round(((A.CGST+A.SGST+A.IGST)/(a.Sale_amt))*100,2) else 0 end)) as TAX_Rate,a.Sale_amt as Sales_Amount,nvl(a.tool_amtz,0) as Tool_Amtz_Amount,(a.Sale_amt+nvl(a.tool_amtz,0)) as Taxable_Amount,(a.CGST) as CGST_Amount,(a.sgst) as SGST_Amount,(a.IGST) as IGST_Amount,'-' as Sb_no,'-' as Sb_dt,'-' as port_cd,(a.TCSAMT) as TCS_Amount,(a.Sale_amt)+(a.CGST)+(a.sgst)+(a.IGST)+(a.TCSAMT) as Grand_Total,a.acode1 as Party_Code  from (" + mq0 + ") a,famst b,type c where a.type=c.type1 and c.id='V' and trim(a.type)=trim(c.type1)  and trim(a.acode1)=trim(b.acode)  order by a.vchdate,a.vchnum ";
                fgen.drillQuery(0, mq1, frm_qstr);
                fgen.Fn_DrillReport("Sales Invoice Wise Data During  " + fromdt + " To " + todt + "  ", frm_qstr, "Sales_Amount,Tool_Amtz_Amount,Taxable_Amount,CGST_Amount,SGST_Amount,IGST_Amount,TCS_Amount,Grand_Total");

                hffield.Value = "-";

                break;
          

            case "SALE_HSN_BTN2":
                     mq1 = "Select a.vchnum as Inv_No,to_Char(a.vchdate,'dd/mm/yyyy') as Inv_Dt,a.aname as Customer,a.gst_no as GST_No,a.staten as State_Name,a.St_code as State_Code,sum(a.iqtyout)as Qty_tot,a.unit,sum(a.iamount) as Basic_Val,sum(a.Tool_Cost) as Tool_Cost,sum(a.pack_Cost) as pack_Cost,sum(a.frt_Cost) as frt_Cost,a.HSCODE,a.CGST_RT,sum(a.CGST_amt) as CGST_amt,a.SGST_Rate,sum(a.SGST_amt) as SGST_amt,a.IGST_Rt,sum(a.IGST_amt) as IGST_amt,nvl(tcsamt,0) as tcsamt_Invwise,a.Invno as Long_Inv_No,a.Invdate as Ref_Inv_Dt,a.type,b.bill_Tot,(case when length(Trim(a.gst_no))=15 then 'B2B' when length(Trim(a.gst_no))<15 and b.bill_tot>=250000 then 'B2CL'  when length(Trim(a.gst_no))<15 and b.bill_tot<250000 then 'B2CS' else '-' end) as GST_Catg,a.branchcd,a.pl_gst_no as plant_gst_no,a.plnm as Plant_name,to_Char(a.vchdate,'yyyymmdd') as VDD from (Select a.branchcd,A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyout,a.irate,a.ichgs as Disc,a.iamount,round(a.iqtyout*a.iexc_Addl,2) as Tool_Cost,round(a.iqtyout*a.ipack,2) as pack_Cost,round(a.iqtyout*a.idiamtr,2) as frt_Cost,(Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.iopr)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.iopr)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.iopr)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.iopr)='IG' then a.exc_amt else 0 end) as IGST_amt,a.Invno,A.Invdate,a.type,c.unit,d.gst_no as pl_gst_no,d.name as plnm from ivoucher a, famst b , item c,type d where a.branchcd=d.type1 and d.id='B' and trim(a.acode)=trim(b.acode) and  trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate  " + xprdrange + "  order by a.vchdate,a.vchnum,a.morder) a, sale b where trim(a.branchcd)||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||b.type||b.vchnum||to_Char(b.vchdate,'dd/mm/yyyy')  group by a.plnm,a.pl_gst_no,a.branchcd,a.vchnum,a.vchdate,a.aname,a.gst_no,a.staten,a.hscode,a.St_code,a.CGST_RT,a.SGST_Rate,a.IGST_Rt,a.type,a.invno,a.invdate,a.unit,b.bill_Tot,nvl(tcsamt,0),to_Char(a.vchdate,'yyyymmdd') order by VDD,a.vchnum";
                     mq1 = "Select a.vchnum as Inv_No,to_Char(a.vchdate,'dd/mm/yyyy') as Inv_Dt,a.aname as Customer,a.gst_no as GST_No,a.staten as State_Name,a.St_code as State_Code,sum(a.iqtyout)as Qty_tot,a.unit,sum(a.iamount) as Basic_Val,sum(a.Tool_Cost) as Tool_Cost,sum(a.pack_Cost) as pack_Cost,sum(a.frt_Cost) as frt_Cost,a.HSCODE,x.name as hsn_name,a.CGST_RT,sum(a.CGST_amt) as CGST_amt,a.SGST_Rate,sum(a.SGST_amt) as SGST_amt,a.IGST_Rt,sum(a.IGST_amt) as IGST_amt,nvl(tcsamt,0) as tcsamt_Invwise,a.Invno as Long_Inv_No,a.Invdate as Ref_Inv_Dt,a.type,b.bill_Tot,(case when length(Trim(a.gst_no))=15 then 'B2B' when length(Trim(a.gst_no))<15 and b.bill_tot>=250000 then 'B2CL'  when length(Trim(a.gst_no))<15 and b.bill_tot<250000 then 'B2CS' else '-' end) as GST_Catg,a.branchcd,a.pl_gst_no as plant_gst_no,a.plnm as Plant_name,to_Char(a.vchdate,'yyyymmdd') as VDD from (Select a.branchcd,A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyout,a.irate,a.ichgs as Disc,a.iamount,round(a.iqtyout*a.iexc_Addl,2) as Tool_Cost,round(a.iqtyout*a.ipack,2) as pack_Cost,round(a.iqtyout*a.idiamtr,2) as frt_Cost,(Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.iopr)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.iopr)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.iopr)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.iopr)='IG' then a.exc_amt else 0 end) as IGST_amt,a.Invno,A.Invdate,a.type,c.unit,d.gst_no as pl_gst_no,d.name as plnm from ivoucher a, famst b , item c,type d where a.branchcd=d.type1 and d.id='B' and trim(a.acode)=trim(b.acode) and  trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate  " + xprdrange + "  order by a.vchdate,a.vchnum,a.morder) a left outer join typegrp x on trim(a.hscode)=trim(x.acref) and x.id='T1' , sale b where trim(a.branchcd)||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||b.type||b.vchnum||to_Char(b.vchdate,'dd/mm/yyyy')  group by a.plnm,a.pl_gst_no,a.branchcd,a.vchnum,a.vchdate,a.aname,a.gst_no,a.staten,a.hscode,a.St_code,a.CGST_RT,a.SGST_Rate,a.IGST_Rt,a.type,a.invno,a.invdate,a.unit,b.bill_Tot,nvl(tcsamt,0),to_Char(a.vchdate,'yyyymmdd'),x.name order by VDD,a.vchnum";
                //
                     mq1 = "Select a.vchnum as Inv_No,to_Char(a.vchdate,'dd/mm/yyyy') as Inv_Dt,a.aname as Customer,a.gst_no as GST_No,a.staten as State_Name,a.St_code as State_Code,sum(a.iqtyout)as Qty_tot,a.unit,sum(a.iamount) as Basic_Val,sum(a.Tool_Cost) as Tool_Cost,sum(a.pack_Cost) as pack_Cost,sum(a.frt_Cost) as frt_Cost,a.HSCODE,x.name as hsn_name,a.CGST_RT,sum(a.CGST_amt) as CGST_amt,a.SGST_Rate,sum(a.SGST_amt) as SGST_amt,a.IGST_Rt,sum(a.IGST_amt) as IGST_amt,nvl(tcsamt,0) as tcsamt_Invwise,a.Invno as Long_Inv_No,a.Invdate as Ref_Inv_Dt,a.type,b.bill_Tot,(case when length(Trim(a.gst_no))=15 then 'B2B' when length(Trim(a.gst_no))<15 and b.bill_tot>=250000 then 'B2CL'  when length(Trim(a.gst_no))<15 and b.bill_tot<250000 then 'B2CS' else '-' end) as GST_Catg,a.branchcd,a.pl_gst_no as plant_gst_no,a.plnm as Plant_name,to_Char(a.vchdate,'yyyymmdd') as VDD from (Select a.branchcd,A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyout,a.irate,a.ichgs as Disc,a.iamount,round(a.iqtyout*a.iexc_Addl,2) as Tool_Cost,round(a.iqtyout*a.ipack,2) as pack_Cost,round(a.iqtyout*a.idiamtr,2) as frt_Cost,(Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.iopr)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.iopr)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.iopr)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.iopr)='IG' then a.exc_amt else 0 end) as IGST_amt,a.Invno,A.Invdate,a.type,c.unit,d.gst_no as pl_gst_no,d.name as plnm from ivoucher a, famst b , item c,type d where a.branchcd=d.type1 and d.id='B' and trim(a.acode)=trim(b.acode) and  trim(a.icode)=trim(c.icode) and a.branchcd in (" + hf2.Value + ") and a.type like '4%' and a.vchdate  " + xprdrange + "  order by a.vchdate,a.vchnum,a.morder) a left outer join typegrp x on trim(a.hscode)=trim(x.acref) and x.id='T1' , sale b where trim(a.branchcd)||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||b.type||b.vchnum||to_Char(b.vchdate,'dd/mm/yyyy')  group by a.plnm,a.pl_gst_no,a.branchcd,a.vchnum,a.vchdate,a.aname,a.gst_no,a.staten,a.hscode,a.St_code,a.CGST_RT,a.SGST_Rate,a.IGST_Rt,a.type,a.invno,a.invdate,a.unit,b.bill_Tot,nvl(tcsamt,0),to_Char(a.vchdate,'yyyymmdd'),x.name order by VDD,a.vchnum";
                     fgen.drillQuery(0, mq1, frm_qstr);
                     fgen.Fn_DrillReport("Sales Invoice Wise HSN Wise Data During  " + fromdt + " To " + todt + "  ", frm_qstr);

                     hffield.Value = "-";
                     break;
                   
            case "SALE_HSN_BTN3":

                     mq1 = "Select a.HSCODE,sum(a.iqtyout)as Qty_tot,a.unit,sum(a.iamount) as Basic_Val,sum(a.Tool_Cost) as Tool_Cost,sum(a.pack_Cost) as pack_Cost,sum(a.frt_Cost) as frt_Cost,a.CGST_RT,sum(a.CGST_amt) as CGST_amt,a.SGST_Rate,sum(a.SGST_amt) as SGST_amt,a.IGST_Rt,sum(a.IGST_amt) as IGST_amt,a.type,a.branchcd,a.pl_gst_no as plant_gst_no,a.plnm as Plant_name from (Select a.branchcd,A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyout,a.irate,a.ichgs as Disc,a.iamount,round(a.iqtyout*a.iexc_Addl,2) as Tool_Cost,round(a.iqtyout*a.ipack,2) as pack_Cost,round(a.iqtyout*a.idiamtr,2) as frt_Cost,(Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.iopr)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.iopr)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.iopr)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.iopr)='IG' then a.exc_amt else 0 end) as IGST_amt,a.Invno,A.Invdate,a.type,c.unit,d.gst_no as pl_gst_no,d.name as plnm from ivoucher a, famst b , item c,type d where a.branchcd=d.type1 and d.id='B' and trim(a.acode)=trim(b.acode) and  trim(a.icode)=trim(c.icode) and a.branchcd in (" + hf2.Value + ")  and a.type like '4%' and a.vchdate  " + xprdrange + "  order by a.vchdate,a.vchnum,a.morder) a, sale b where trim(a.branchcd)||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||b.type||b.vchnum||to_Char(b.vchdate,'dd/mm/yyyy')  group by a.plnm,a.pl_gst_no,a.branchcd,a.hscode,a.CGST_RT,a.SGST_Rate,a.IGST_Rt,a.type,a.unit order by a.hscode";
                     mq1 = "Select a.HSCODE,c.name as hsn_name,sum(a.iqtyout)as Qty_tot,a.unit,sum(a.iamount) as Basic_Val,sum(a.Tool_Cost) as Tool_Cost,sum(a.pack_Cost) as pack_Cost,sum(a.frt_Cost) as frt_Cost,a.CGST_RT,sum(a.CGST_amt) as CGST_amt,a.SGST_Rate,sum(a.SGST_amt) as SGST_amt,a.IGST_Rt,sum(a.IGST_amt) as IGST_amt,a.type,a.branchcd,a.pl_gst_no as plant_gst_no,a.plnm as Plant_name from (Select a.branchcd,A.vchnum,a.vchdate,b.aname,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyout,a.irate,a.ichgs as Disc,a.iamount,round(a.iqtyout*a.iexc_Addl,2) as Tool_Cost,round(a.iqtyout*a.ipack,2) as pack_Cost,round(a.iqtyout*a.idiamtr,2) as frt_Cost,(Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.iopr)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.iopr)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.iopr)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.iopr)='IG' then a.exc_amt else 0 end) as IGST_amt,a.Invno,A.Invdate,a.type,c.unit,d.gst_no as pl_gst_no,d.name as plnm from ivoucher a, famst b , item c,type d where a.branchcd=d.type1 and d.id='B' and trim(a.acode)=trim(b.acode) and  trim(a.icode)=trim(c.icode) and a.branchcd in (" + hf2.Value + ")  and a.type like '4%' and a.vchdate  " + xprdrange + "  order by a.vchdate,a.vchnum,a.morder) a left outer join typegrp c on trim(a.hscode)=trim(C.acref) and c.id='T1' , sale b where trim(a.branchcd)||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||b.type||b.vchnum||to_Char(b.vchdate,'dd/mm/yyyy')  group by a.plnm,a.pl_gst_no,a.branchcd,a.hscode,a.CGST_RT,a.SGST_Rate,a.IGST_Rt,a.type,a.unit,c.name order by a.hscode";
                fgen.drillQuery(0, mq1, frm_qstr);
                fgen.Fn_DrillReport("HSN Wise Sales Data During  " + fromdt + " To " + todt + "  ", frm_qstr);

                hffield.Value = "-";
                break;

            case "SALE_INV_BTN4":
                repvty = "59";
                sumdtl = "D";
                repamtfld = "cramt";
                txcode1 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0077'", "OPT_PARAM");
                txcode2 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0078'", "OPT_PARAM");
                txcode3 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0079'", "OPT_PARAM");

                txcode4 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0123'", "OPT_PARAM");
                txcode5 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0124'", "OPT_PARAM");
                txcode6 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0114'", "OPT_PARAM");

                mq0 = "select branchcd,type,max(acode) as acode1,sum(Sale_amt)as Sale_amt,sum(CGST) As CGST,sum(SGST)AS SGST,sum(IGST) As IGST,sum(tcsamt) as tcsamt,sum(tool_amtz) as tool_amtz,fstr,vchnum,vchdate,invno,invdate,max(originv_no) As originv_no,max(originv_dt) as originv_dt   from (Select a.branchcd,a.type,a.acode,0 as Sale_amt,0 as CGST,0 as SGST,0 as IGST,0 as tcsamt,round(iqty_chl*nvl(iexc_addl,0),2) as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate,null as originv_no,null as originv_dt  from ivoucherp a where a.branchcd in (" + hf2.Value + ")  and a.type like '59%' and a.vchdate " + xprdrange + " union all Select a.branchcd,a.type,a.rcode as acode," + repamtfld + " as Sale_amt,0 as CGST,0 as SGST,0 as IGST,0 as tcsamt,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate,a.originv_no,a.originv_dt  from voucher a where a.branchcd in (" + hf2.Value + ")  and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and  trim(A.acode) not in ('" + txcode1.Trim() + "','" + txcode2.Trim() + "','" + txcode3.Trim() + "','" + txcode4.Trim() + "','" + txcode5.Trim() + "','" + txcode6.Trim() + "') and a.cramt>0 union all Select a.branchcd,a.type,null as acode,0 as Sale_amt," + repamtfld + " as CGST,0 as SGST,0 as IGST,0 as tcsamt,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate,a.originv_no,a.originv_dt  from voucher a where a.branchcd in (" + hf2.Value + ")  and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode1.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST," + repamtfld + " as SGST,0 as IGST,0 as tcsamt,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate,a.originv_no,a.originv_dt  from voucher a where a.branchcd in (" + hf2.Value + ")  and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode2.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST,0 as SGST," + repamtfld + " as IGST,0 as tcsamt,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate,a.originv_no,a.originv_dt  from voucher a where a.branchcd in (" + hf2.Value + ")  and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode3.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST,0 as SGST,0 as IGST," + repamtfld + " as TCSAMT,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate,a.originv_no,a.originv_dt  from voucher a where a.branchcd in (" + hf2.Value + ")  and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode in ('" + txcode4.Trim() + "','" + txcode5.Trim() + "')) group by branchcd,type,fstr,vchnum,vchdate,invno,invdate having sum(Sale_amt)+sum(CGST)+sum(SGST)+sum(IGST)+sum(tcsamt)>0 ";

                mq1 = "select '-' as fstr,'-' as gstr,c.Name as Type_Name,b.Aname,b.staffcd as StateCode,b.GST_No,a.Branchcd,a.type,a.Invno,to_char(a.Invdate,'dd/mm/yyyy') as Inv_Date,(a.Sale_amt)+(a.CGST)+(a.sgst)+(a.IGST) as Inv_value_wout_tcs,trim(b.staffcd)||'-'||b.staten as State_Code,trim(b.staffcd)||'-'||b.staten as Place_Supp,b.GSTREVCHG,'Regular' as Inv_type,'-' as Ecom_no,(case when (a.Sale_amt)>0 then round(((a.CGST+a.sgst+a.IGST)/(a.Sale_amt))*100,2) else 0 end) as TAX_Rate,a.Sale_amt as Sales_Amount,nvl(a.tool_amtz,0) as Tool_Amtz_Amount,(a.Sale_amt+nvl(a.tool_amtz,0)) as Taxable_Amount,(a.CGST) as CGST_Amount,(a.sgst) as SGST_Amount,(a.IGST) as IGST_Amount,'-' as Sb_no,'-' as Sb_dt,'-' as port_cd,(a.TCSAMT) as TCS_Amount,(a.Sale_amt)+(a.CGST)+(a.sgst)+(a.IGST)+(a.TCSAMT) as Grand_Total,originv_no,to_char(originv_dt,'dd/mm/yyyy') As originv_dt,a.acode1 as Party_Code  from (" + mq0 + ") a,famst b,type c where a.type=c.type1 and c.id='V' and trim(a.type)=trim(c.type1)  and trim(a.acode1)=trim(b.acode)  order by a.vchdate,a.vchnum ";

                fgen.drillQuery(0, mq1, frm_qstr);
                fgen.Fn_DrillReport("Debit Notes Wise Data During  " + fromdt + " To " + todt + "  ", frm_qstr, "Sales_Amount,Tool_Amtz_Amount,Taxable_Amount,CGST_Amount,SGST_Amount,IGST_Amount,TCS_Amount,Grand_Total");

                hffield.Value = "-";

                break;


            case "SALE_INV_BTN5":
                repvty = "58";
                sumdtl = "D";
                repamtfld = "Dramt";
                txcode1 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0077'", "OPT_PARAM");
                txcode2 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0078'", "OPT_PARAM");
                txcode3 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0079'", "OPT_PARAM");

                txcode4 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0123'", "OPT_PARAM");
                txcode5 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0124'", "OPT_PARAM");
                txcode6 = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_PARAM from FIN_RSYS_OPT where opt_id='W0114'", "OPT_PARAM");



                mq0 = "select branchcd,type,max(acode) as acode1,sum(Sale_amt)as Sale_amt,sum(CGST) As CGST,sum(SGST)AS SGST,sum(IGST) As IGST,sum(tcsamt) as tcsamt,sum(tool_amtz) as tool_amtz,fstr,vchnum,vchdate,invno,invdate,originv_no,originv_dt from (Select a.branchcd,a.type,a.rcode as acode," + repamtfld + " as Sale_amt,0 as CGST,0 as SGST,0 as IGST,0 as tcsamt,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate,a.originv_no,a.originv_dt from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and  trim(A.acode) not in ('" + txcode1.Trim() + "','" + txcode2.Trim() + "','" + txcode3.Trim() + "','" + txcode4.Trim() + "','" + txcode5.Trim() + "','" + txcode6.Trim() + "') and a." + repamtfld + ">0 union all Select a.branchcd,a.type,null as acode,0 as Sale_amt," + repamtfld + " as CGST,0 as SGST,0 as IGST,0 as tcsamt,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate,a.originv_no,a.originv_dt  from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode1.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST," + repamtfld + " as SGST,0 as IGST,0 as tcsamt,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate,a.originv_no,a.originv_dt  from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode2.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST,0 as SGST," + repamtfld + " as IGST,0 as tcsamt,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate,a.originv_no,a.originv_dt  from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode = '" + txcode3.Trim() + "' union all Select a.branchcd,a.type,null as acode,0 as Sale_amt,0 as CGST,0 as SGST,0 as IGST," + repamtfld + " as TCSAMT,0 as tool_amtz,a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy') as fstr,a.vchnum,a.vchdate,a.invno,a.invdate,a.originv_no,a.originv_dt  from voucher a where a.branchcd='" + frm_mbr + "' and a.type like '" + repvty + "%' and a.vchdate " + xprdrange + " and a.acode in ('" + txcode4.Trim() + "','" + txcode5.Trim() + "')) group by originv_no,originv_dt,branchcd,type,fstr,vchnum,vchdate,invno,invdate having sum(Sale_amt)+sum(CGST)+sum(SGST)+sum(IGST)+sum(tcsamt)>0 ";

                mq1 = "select '-' as fstr,'-' as gstr,c.Name as Type_Name,b.Aname,b.staffcd as StateCode,b.GST_No,a.Branchcd,a.type,a.Invno,to_char(a.Invdate,'dd/mm/yyyy') as Inv_Date,(a.Sale_amt)+(a.CGST)+(a.sgst)+(a.IGST) as Inv_value_wout_tcs,trim(b.staffcd)||'-'||b.staten as State_Code,trim(b.staffcd)||'-'||b.staten as Place_Supp,b.GSTREVCHG,'Regular' as Inv_type,'-' as Ecom_no,(case when (a.Sale_amt)>0 then round(((a.CGST+a.sgst+a.IGST)/(a.Sale_amt))*100,2) else 0 end) as TAX_Rate,a.Sale_amt as Sales_Amount,nvl(a.tool_amtz,0) as Tool_Amtz_Amount,(a.Sale_amt+nvl(a.tool_amtz,0)) as Taxable_Amount,(a.CGST) as CGST_Amount,(a.sgst) as SGST_Amount,(a.IGST) as IGST_Amount,'-' as Sb_no,'-' as Sb_dt,'-' as port_cd,(a.TCSAMT) as TCS_Amount,(a.Sale_amt)+(a.CGST)+(a.sgst)+(a.IGST)+(a.TCSAMT) as Grand_Total,a.originv_no,to_chaR(a.originv_dt,'dd/mm/yyyy') AS originv_dt,a.acode1 as Party_Code  from (" + mq0 + ") a,famst b,type c where a.type=c.type1 and c.id='V' and trim(a.type)=trim(c.type1)  and trim(a.acode1)=trim(b.acode)  order by a.vchdate,a.vchnum ";

                fgen.drillQuery(0, mq1, frm_qstr);
                fgen.Fn_DrillReport(btn5.Text + " During  " + fromdt + " To " + todt + "  ", frm_qstr, "Sales_Amount,Tool_Amtz_Amount,Taxable_Amount,CGST_Amount,SGST_Amount,IGST_Amount,TCS_Amount,Grand_Total");

                hffield.Value = "-";

                break;




        }

    }

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

        // sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(Int32)));
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

    //public void create_tab3()
    //{


    //    sg3_dt = new DataTable();
    //    sg3_dr = null;
    //    // Hidden Field

    //    sg3_dt.Columns.Add(new DataColumn("sg3_SrNo", typeof(Int32)));
    //    sg3_dt.Columns.Add(new DataColumn("sg3_f1", typeof(string)));
    //    sg3_dt.Columns.Add(new DataColumn("sg3_f2", typeof(string)));
    //    sg3_dt.Columns.Add(new DataColumn("sg3_t1", typeof(string)));
    //    sg3_dt.Columns.Add(new DataColumn("sg3_t2", typeof(string)));
    //    sg3_dt.Columns.Add(new DataColumn("sg3_t3", typeof(string)));
    //    sg3_dt.Columns.Add(new DataColumn("sg3_t4", typeof(string)));

    //}

    //public void create_tab4()
    //{
    //    sg4_dt = new DataTable();
    //    sg4_dr = null;
    //    // Hidden Field

    //    sg4_dt.Columns.Add(new DataColumn("sg4_SrNo", typeof(Int32)));
    //    sg4_dt.Columns.Add(new DataColumn("sg4_t1", typeof(string)));
    //    sg4_dt.Columns.Add(new DataColumn("sg4_t2", typeof(string)));
    //    sg4_dt.Columns.Add(new DataColumn("sg4_t3", typeof(string)));
    //    sg4_dt.Columns.Add(new DataColumn("sg4_t4", typeof(string)));

    //}

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

        // sg1_dt.Rows.Add(sg1_dr);
    }
    //public void sg2_add_blankrows()
    //{
    //    sg2_dr = sg2_dt.NewRow();


    //    sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
    //    sg2_dr["sg2_t1"] = "-";
    //    sg2_dr["sg2_t2"] = "-";
    //    sg2_dt.Rows.Add(sg2_dr);
    //}
    //public void sg3_add_blankrows()
    //{
    //    sg3_dr = sg3_dt.NewRow();

    //    sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
    //    sg3_dr["sg3_f1"] = "-";
    //    sg3_dr["sg3_f2"] = "-";
    //    sg3_dr["sg3_t1"] = "-";
    //    sg3_dr["sg3_t2"] = "-";
    //    sg3_dr["sg3_t3"] = "-";
    //    sg3_dr["sg3_t4"] = "-";

    //    sg3_dt.Rows.Add(sg3_dr);
    //}

    //public void sg4_add_blankrows()
    //{
    //    sg4_dr = sg4_dt.NewRow();


    //    sg4_dr["sg4_SrNo"] = sg4_dt.Rows.Count + 1;
    //    sg4_dr["sg4_t1"] = "-";
    //    sg4_dr["sg4_t2"] = "-";
    //    sg4_dt.Rows.Add(sg4_dr);
    //}

    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 

            //  e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Left;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[1].Width = 150;
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
    //protected void sg2_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    string var = e.CommandName.ToString();
    //    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
    //    int index = Convert.ToInt32(sg2.Rows[rowIndex].RowIndex);

    //    if (txtvchnum.Value == "-")
    //    {
    //        fgen.msg("-", "AMSG", "Doc No. not correct");
    //        return;
    //    }
    //    switch (var)
    //    {
    //        case "SG2_RMV":

    //            break;
    //        case "SG2_ROW_ADD":

    //            break;
    //    }
    //}

    ////------------------------------------------------------------------------------------
    //protected void sg3_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    string var = e.CommandName.ToString();
    //    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
    //    int index = Convert.ToInt32(sg3.Rows[rowIndex].RowIndex);

    //    if (txtvchnum.Value == "-")
    //    {
    //        fgen.msg("-", "AMSG", "Doc No. not correct");
    //        return;
    //    }
    //    switch (var)
    //    {
    //        case "SG3_RMV":

    //            break;
    //        case "SG3_ROW_ADD":

    //            break;
    //    }
    //}
    //protected void sg4_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    string var = e.CommandName.ToString();
    //    int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
    //    int index = Convert.ToInt32(sg4.Rows[rowIndex].RowIndex);

    //    if (txtvchnum.Value == "-")
    //    {
    //        fgen.msg("-", "AMSG", "Doc No. not correct");
    //        return;
    //    }
    //    switch (var)
    //    {
    //        case "sg4_RMV":

    //            break;
    //        case "sg4_ROW_ADD":

    //            break;
    //    }
    //}

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



    void Type_Sel_query()
    {


    }

    //------------------------------------------------------------------------------------   

    protected void btntype_Click(object sender, ImageClickEventArgs e)
    {
        if (edmode.Value == "Y")
        {
            fgen.msg("-", "AMSG", "Code Change not Allowed in Edit mode !!");

            return;
        }
        else
        {
            hffield.Value = "TYPECODE";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Type Code", frm_qstr);
        }
    }
    protected void btnactg_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ACTGCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Account Code", frm_qstr);
    }

    protected void btn1_Click(object sender, EventArgs e)
    {
        hffield.Value = "SALE_INV_BTN1";
        make_qry_4_popup();
        fgen.Fn_open_mseek("", frm_qstr);
    }
    protected void btn2_Click(object sender, EventArgs e)
    {
        hffield.Value = "SALE_HSN_BTN2";       
        make_qry_4_popup();
        fgen.Fn_open_mseek("", frm_qstr);
    }

    protected void btn3_Click(object sender, EventArgs e)
    {
        hffield.Value = "SALE_HSN_BTN3";
        make_qry_4_popup();
        fgen.Fn_open_mseek("", frm_qstr);
    }
    protected void btn4_Click(object sender, EventArgs e)
    {
        hffield.Value = "SALE_INV_BTN4";
        make_qry_4_popup();
        fgen.Fn_open_mseek("", frm_qstr);
        //lbl1.Text = btn4.Text;
        //mhd = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
        //mhd1 = System.DateTime.Now.Date.ToString("yyyyMM");
        //if (frm_cocd == "ARIO" || frm_cocd == "NAVK")
        //{
        //    //seekSql = "Select substr(icode,1,2) as branchcd,sum(iamount) as today,0 as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type not in ('4G','47') and vchdate=to_DatE('" & Format(ServerDate, "dd/mm/yyyy") & "','dd/mm/yyyy') group by substr(icode,1,2) union all Select substr(icode,1,2) as branchcd,0 as today,sum(iamount) as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type not in ('4G','47') and  to_char(vchdate,'yyyymm')='" & Format(ServerDate, "yyyymm") & "' group by substr(icode,1,2)" _& " union all Select substr(icode,1,2) as branchcd,0 as today,0 as mtd,sum(iamount) as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type not in ('4G','47') and  vchdate " + DateRange + " group by substr(icode,1,2)"//finsys qry
        //    mq0 = "Select substr(icode,1,2) as branchcd,sum(iamount) as today,0 as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type not in ('4G','47') and vchdate=to_DatE('" + mhd + "','dd/mm/yyyy') group by substr(icode,1,2) union all Select substr(icode,1,2) as branchcd,0 as today,sum(iamount) as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type not in ('4G','47') and  to_char(vchdate,'yyyymm')='" + mhd1 + "' group by substr(icode,1,2) union all Select substr(icode,1,2) as branchcd,0 as today,0 as mtd,sum(iamount) as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type not in ('4G','47') and  vchdate " + DateRange + " group by substr(icode,1,2)";
        //}
        //else
        //{
        //    //   seekSql = "Select substr(icode,1,2) as branchcd,sum(iamount) as today,0 as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and vchdate=to_DatE('" & Format(ServerDate, "dd/mm/yyyy") & "','dd/mm/yyyy') group by substr(icode,1,2) union all Select substr(icode,1,2) as branchcd,0 as today,sum(iamount) as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and  to_char(vchdate,'yyyymm')='" & Format(ServerDate, "yyyymm") & "' group by substr(icode,1,2)" _& " union all Select substr(icode,1,2) as branchcd,0 as today,0 as mtd,sum(iamount) as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and  vchdate " + DateRange + " group by substr(icode,1,2)" //fnsys qry
        //    mq0 = "Select substr(icode,1,2) as branchcd,sum(iamount) as today,0 as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and vchdate=to_DatE('" + mhd + "','dd/mm/yyyy') group by substr(icode,1,2) union all Select substr(icode,1,2) as branchcd,0 as today,sum(iamount) as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and  to_char(vchdate,'yyyymm')='" + mhd1 + "' group by substr(icode,1,2) union all Select substr(icode,1,2) as branchcd,0 as today,0 as mtd,sum(iamount) as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and  vchdate  " + DateRange + " group by substr(icode,1,2)";
        //}
        ////   If (cd = "ARIO" Or cd = "NAVK") Or cd = "ARCF" Or CLIENTGRP = "GRP_HENA" Then
        //if (frm_cocd == "ARIO" || frm_cocd == "NAVK")
        //{
        //    SQuery = "select b.name as Name_of_Unit,to_char(sum(today),'" + numbr_fmt2 + "')  as Today,to_char(sum(mtd),'" + numbr_fmt2 + "') as this_month,to_char(sum(ytd),'" + numbr_fmt2 + "') as this_year,a.branchcd,sum(today) as Todayd,sum(mtd) as MTDD,sum(ytd) as ytdd from (" + mq0 + ") a,type b where substr(a.branchcd,1,1)='9' and b.id='Y' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";
        //}
        //else
        //{
        //    //SQuery = "select b.name as Name_of_Unit,to_char(sum(today),'" + numbr_fmt2 + "')  as Today,to_char(sum(mtd),'" + numbr_fmt2 + "') as this_month,to_char(sum(ytd),'" + numbr_fmt2 + "') as this_year,a.branchcd,sum(today) as Todayd,sum(mtd) as MTDD,sum(ytd) as ytdd from (" + mq0 + ") a,type b where b.id='Y' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";//old
        //    SQuery = "select b.name as Name_of_Unit,to_char(sum(today),'" + numbr_fmt2 + "')  as Today,to_char(sum(mtd),'" + numbr_fmt2 + "') as this_month,to_char(sum(ytd),'" + numbr_fmt2 + "') as this_year from (" + mq0 + ") a,type b where b.id='Y' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";//after rmv some colms 
        //}
        //dt = new DataTable();
        //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//Showing Basic Sales
        //if (dt.Rows.Count > 0)
        //{
        //    create_tab();
        //    sg1_add_blankrows();

        //    sg1.DataSource = dt;
        //    sg1.DataBind();
        //    dt.Dispose();
        //}
        //else
        //{
        //    fgen.msg("-", "AMSG", "NO DATA FOUND");
        //}
    }

    protected void btn5_Click(object sender, EventArgs e)
    {
        hffield.Value = "SALE_INV_BTN5";
        make_qry_4_popup();
        fgen.Fn_open_mseek("", frm_qstr);

        //lbl1.Text = btn5.Text;
        //mhd = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
        //mhd1 = System.DateTime.Now.Date.ToString("yyyyMM");
        ////  seekSql = "Select substr(icode,1,2) as branchcd,sum((qtyord)*((irate*(decode(curr_Rate,0,1,curr_rate)))*((100-cdisc)/100))) as today,0 as mtd,0 as ytd from somas where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and orddt=to_DatE('" & Format(ServerDate, "dd/mm/yyyy") & "','dd/mm/yyyy') group by substr(icode,1,2) union all Select substr(icode,1,2) as branchcd,0 as today,sum((qtyord)*((irate*(decode(curr_Rate,0,1,curr_rate)))*((100-cdisc)/100))) as mtd,0 as ytd from somas where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and  to_char(orddt,'yyyymm')='" & Format(ServerDate, "yyyymm") & "' group by substr(icode,1,2)" _ & " union all Select substr(icode,1,2) as branchcd,0 as today,0 as mtd,sum((qtyord)*((irate*(decode(curr_Rate,0,1,curr_rate)))*((100-cdisc)/100))) as ytd from somas where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and  orddt " + DateRange + " group by substr(icode,1,2)"//main fins qry
        //mq0 = "Select substr(icode,1,2) as branchcd,sum((qtyord)*((irate*(decode(curr_Rate,0,1,curr_rate)))*((100-cdisc)/100))) as today,0 as mtd,0 as ytd from somas where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and orddt=to_DatE('" + mhd + "','dd/mm/yyyy') group by substr(icode,1,2) union all Select substr(icode,1,2) as branchcd,0 as today,sum((qtyord)*((irate*(decode(curr_Rate,0,1,curr_rate)))*((100-cdisc)/100))) as mtd,0 as ytd from somas where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and  to_char(orddt,'yyyymm')='" + mhd1 + "' group by substr(icode,1,2)  union all Select substr(icode,1,2) as branchcd,0 as today,0 as mtd,sum((qtyord)*((irate*(decode(curr_Rate,0,1,curr_rate)))*((100-cdisc)/100))) as ytd from somas where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and  orddt " + DateRange + " group by substr(icode,1,2)";
        ////SQuery = "select b.name as Name_of_Unit,to_char(sum(today),'" + numbr_fmt2 + "')  as Today,to_char(sum(mtd),'" + numbr_fmt2 + "') as this_month,to_char(sum(ytd),'" + numbr_fmt2 + "') as this_year,a.branchcd,sum(today) as Todayd,sum(mtd) as MTDD,sum(ytd) as ytdd from (" + mq0 + ") a,type b where b.id='Y' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";////old
        //SQuery = "select b.name as Name_of_Unit,to_char(sum(today),'" + numbr_fmt2 + "')  as Today,to_char(sum(mtd),'" + numbr_fmt2 + "') as this_month,to_char(sum(ytd),'" + numbr_fmt2 + "') as this_year from (" + mq0 + ") a,type b where b.id='Y' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";
        //dt = new DataTable();
        //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//Showing Basic Sales
        //if (dt.Rows.Count > 0)
        //{
        //    create_tab();
        //    sg1_add_blankrows();

        //    sg1.DataSource = dt;
        //    sg1.DataBind();
        //    dt.Dispose();
        //}
        //else
        //{
        //    fgen.msg("-", "AMSG", "NO DATA FOUND");
        //}
    }
    protected void btn6_Click(object sender, EventArgs e)
    {
        hffield.Value = "PUR_INV_BTN1";
        fgen.Fn_open_prddmp1("-", frm_qstr);


    }
    protected void btn7_Click(object sender, EventArgs e)
    {

        hffield.Value = "PUR_INV_BTN2";
        fgen.Fn_open_prddmp1("-", frm_qstr);

    }
    protected void btn8_Click(object sender, EventArgs e)
    {
        hffield.Value = "PUR_INV_BTN3";
        fgen.Fn_open_prddmp1("-", frm_qstr);


    }
    protected void btn9_Click(object sender, EventArgs e)
    {
        hffield.Value = "PUR_INV_BTN4";
        fgen.Fn_open_prddmp1("-", frm_qstr);

        ////lbl1.Text = btn9.Text;
        ////mhd = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
        ////mhd1 = System.DateTime.Now.Date.ToString("yyyyMM");
        //////seekSql = "Select branchcd,sum(dramt-cramt) as today,0 as mtd,0 as ytd from voucher where branchcd!='88' and substr(type,1,1)='2' and substr(acode,1,2) in('05','06') and vchdate=to_DatE('" & Format(ServerDate, "dd/mm/yyyy") & "','dd/mm/yyyy') group by branchcd union all Select branchcd,0 as today,sum(dramt-cramt) as mtd,0 as ytd from voucher where branchcd!='88' and substr(type,1,1)='2' and substr(acode,1,2) in('05','06') and to_char(vchdate,'yyyymm')='" & Format(ServerDate, "yyyymm") & "' group by branchcd union all Select branchcd,0 as today,0 as mtd,sum(dramt-cramt) as ytd from voucher where branchcd!='88' and substr(type,1,1)='2' and substr(acode,1,2) in('05','06') and vchdate " + DateRange + " group by branchcd"//finsts qry
        //////mq0 = "select a.branchcd,a.today,a.mtd,a.ytd,a.acode from (select branchcd,sum(dramt-cramt) as today,0 as mtd,0 as ytd,acode from voucher where branchcd!='88' and substr(type,1,1)='2' and vchdate=to_DatE('" + mhd + "','dd/mm/yyyy') group by branchcd,acode union all Select branchcd,0 as today,sum(dramt-cramt) as mtd,0 as ytd,acode from voucher where branchcd!='88' and substr(type,1,1)='2' and to_char(vchdate,'yyyymm')='" + mhd1 + "' group by branchcd,acode union all Select branchcd,0 as today,0 as mtd,sum(dramt-cramt) as ytd,acode from voucher where branchcd!='88' and substr(type,1,1)='2' and vchdate " + DateRange + " group by branchcd,acode) a ,famst b where trim(a.acode)=trim(b.acode) and b.grp in ('05','06')";
        ////mq0 = "select a.branchcd,sum(a.dramt-a.cramt) as today,0 as mtd,0 as ytd from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd!='88' and substr(a.type,1,1)='2' and a.vchdate=to_DatE('" + mhd + "','dd/mm/yyyy') and b.grp in ('05','06') group by a.branchcd union all Select a.branchcd,0 as today,sum(a.dramt-a.cramt) as mtd,0 as ytd from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd!='88' and substr(a.type,1,1)='2' and to_char(a.vchdate,'yyyymm')='" + mhd1 + "' and b.grp in ('05','06') group by a.branchcd union all Select a.branchcd,0 as today,0 as mtd,sum(a.dramt-a.cramt) as ytd from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd!='88' and substr(a.type,1,1)='2' and a.vchdate " + DateRange + " and b.grp in ('05','06')  group by a.branchcd";
        //////SQuery = "select b.name as Name_of_Unit,to_char(sum(today),'" + numbr_fmt2 + "')  as Today,to_char(sum(mtd),'" + numbr_fmt2 + "') as this_month,to_char(sum(ytd),'" + numbr_fmt2 + "') as this_year,a.branchcd,sum(today) as Todayd,sum(mtd) as MTDD,sum(ytd) as ytdd from (" + mq0 + ") a,type b where id='B' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";//old
        ////SQuery = "select b.name as Name_of_Unit,to_char(sum(today),'" + numbr_fmt2 + "')  as Today,to_char(sum(mtd),'" + numbr_fmt2 + "') as this_month,to_char(sum(ytd),'" + numbr_fmt2 + "') as this_year from (" + mq0 + ") a,type b where id='B' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";
        ////dt = new DataTable();
        ////dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);// "Showing Basic Order Value"
        ////if (dt.Rows.Count > 0)
        ////{
        ////    create_tab();
        ////    sg1_add_blankrows();
        ////    sg1.DataSource = dt;
        ////    sg1.DataBind();
        ////    dt.Dispose();
        ////}
        ////else
        ////{
        ////    fgen.msg("-", "AMSG", "NO DATA FOUND");
        ////    sg1.DataSourceID = null;
        ////    sg1.DataBind();
        ////}

    }
    protected void btn10_Click(object sender, EventArgs e)
    {
        hffield.Value = "PUR_INV_BTN5";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
}