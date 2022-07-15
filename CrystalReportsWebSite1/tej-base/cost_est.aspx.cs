using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Oracle.ManagedDataAccess.Client;

//FLEX_COST ON OMNI LOCAL BKUP ID==ENGG WEB
public partial class cost_est : System.Web.UI.Page
{
    string btnval, SQuery, co_Cd, uname, col1, col2, col3, mbr, cstr, vchnum, vardate, fromdt, todt, DateRange, year, ulvl, dfstring, typePopup = "Y";
    DataTable dt; DataRow oporow; DataSet oDS, oDS1;
    OracleConnection con; OracleDataAdapter da;
    int usg, i = 0;
    string save_it;
    string Checked_ok;
    string Prg_Id;
    DataTable dtCol = new DataTable();
    string pk_error = "Y", chk_rights = "N", PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vty1, frm_vnum, frm_vnum1, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_tabname1, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, custom_filing_no;
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
                //doc_addl.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select (case when nvl(st_Sc,1)=0 then 1 else nvl(st_Sc,1) end )  as add_tx from type where id='B' and trim(upper(type1))=upper(Trim('" + frm_mbr + "'))", "add_tx");
                doc_addl.Value = "-";
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
                tab_active();
                // rdform.SelectedIndex = 0;
            }
            if (frm_cocd == "MLAB")
            {
                trexise.Visible = false;
                trexise_1.Visible = false;
                saletaxrow.Visible = false;
                saletaxrow_1.Visible = false;
            }
            fill_value();
            setColHeadings();
            set_Val();
            btnprint.Visible = false;
        }
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
        frm_tabname = "somas_anx";
        frm_tabname1 = "scratch";
        lblheader.Text = "Costing Sheet";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MM");//COSTING FORM
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME1", frm_tabname1);
        typePopup = "N";
        // tab_active();
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
        #region
        //if (sg1.Rows.Count <= 0) return;
        //for (int sR = 0; sR < sg1.Columns.Count; sR++)
        //{
        //    string orig_name;
        //    double tb_Colm;
        //    tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
        //    orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();

        //    for (int K = 0; K < sg1.Rows.Count; K++)
        //    {
        //        #region hide hidden columns
        //        for (int i = 0; i < 10; i++)
        //        {
        //            sg1.Columns[i].HeaderStyle.CssClass = "hidden";
        //            sg1.Rows[K].Cells[i].CssClass = "hidden";
        //        }
        //        #endregion
        //        if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t5")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t6")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t7")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t8")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t9")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t12")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t13")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t14")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t15")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t17")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t18")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t19")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t20")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t21")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t22")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t23")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t24")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t25")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t26")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t27")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t28")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t29")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t30")).Attributes.Add("autocomplete", "off");
        //        ((TextBox)sg1.Rows[K].FindControl("sg1_t31")).Attributes.Add("autocomplete", "off");
        //  }
        //    orig_name = orig_name.ToUpper();
        //    //if (sg1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_NAME"))
        //    if (sR == tb_Colm)
        //    {
        //        // hidding column
        //        if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
        //        {
        //            sg1.Columns[sR].Visible = false;
        //        }
        //        // Setting Heading Name
        //        sg1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
        //        // Setting Col Width
        //        string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
        //        if (fgen.make_double(mcol_width) > 0)
        //        {
        //            sg1.HeaderRow.Cells[sR].Width = Convert.ToInt32(mcol_width);
        //            //sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
        //            //sg1.Rows[0].Cells[sR].Style.Add("width", mcol_width + "px");
        //        }
        //    }
        //}
        // to hide and show to tab panel
        TabPanel1.Visible = false;
        TabPanel2.Visible = false;
        TabPanel5.Visible = false;
        #endregion
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void tab_active()
    {
        // TabContainer1.ActiveTabIndex = 2;
        TabPanel1.Visible = false;
        TabPanel2.Visible = false;
        TabPanel3.Visible = true;
        TabPanel4.Visible = false;
        TabPanel5.Visible = false;
        lbltd1.Text = "Materials";
        lbltd1.Attributes.Add("font", "bold");
        lbltd2.Text = "Thickness"; lbltd3.Text = "SP GR"; lbltd4.Text = "Gsm"; lbltd5.Text = "Rs/Kg"; lbltd6.Text = "Rs/Sqmtr";
        rdform.SelectedValue = "0";
        if (frm_cocd == "MFLX")
        {
            TabPanel2.InnerText = "Flexible Packaging";
            trexise.Visible = false;
            trexise_1.Visible = false;
        }

        else
        {
            //TabContainer1.ActiveTabIndex = 0;
            TabPanel1.Visible = true;
            TabPanel2.Visible = true;
            TabPanel3.Visible = true;
            TabPanel4.Visible = false;
            TabPanel5.Visible = true;
        }
    }
    public void fill_value()
    {
        txtrt4.Text = "1.4"; txtrt4a.Text = "1.4"; txtrt10.Text = "0.92"; txtrt16.Text = "0.95"; txtrt22.Text = "0.905"; txtrt28.Text = "0.905"; txtrt49.Text = "0.905";
        txtrt54.Text = "0.94"; txtrt59.Text = "2.67"; txtrt64.Text = "1.35"; txtrt69.Text = "1.4"; txtrt74.Text = "0.96"; txtrt79.Text = "0.98";
        txtrt84.Text = "1.14"; txtrt89.Text = "1"; txtrt94.Text = "0.67"; txtrt99.Text = "1"; txtrt104.Text = "1"; txtrt109.Text = "1"; txtrt134.Text = "2";
        if (ulvl == "0")
        {
            txtrt4.ReadOnly = false; txtrt10.ReadOnly = false; txtrt16.ReadOnly = false; txtrt22.ReadOnly = false; txtrt28.ReadOnly = false; txtrt49.ReadOnly = false;
            txtrt54.ReadOnly = false; txtrt59.ReadOnly = false; txtrt64.ReadOnly = false; txtrt69.ReadOnly = false; txtrt74.ReadOnly = false; txtrt79.ReadOnly = false;
            txtrt84.ReadOnly = false; txtrt89.ReadOnly = false; txtrt94.ReadOnly = false; txtrt99.ReadOnly = false; txtrt104.ReadOnly = false; txtrt109.ReadOnly = false;
        }
        if (frm_cocd != "MLAB")
        {
            txtrt6a.ReadOnly = true;
            txtrt3a.ReadOnly = true;
        }
        //else//this else portion is not merged in main code.......
        //{
        //    txtrt6a.Visible = false;
        //    txtrt3a.Visible = false;
        //}
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btncal.Enabled = false; btnacode.Enabled = false; btnicode.Enabled = false;
        btnprint.Disabled = false; btnlist.Disabled = false; btnrefresh.Disabled = true;
    }
    //------------------------------------------------------------------------------------
    protected void btncancel_ServerClick(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();
    }
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btncal.Enabled = true; btnacode.Enabled = true; btnicode.Enabled = true;
        btnprint.Disabled = true; btnlist.Disabled = true; btnrefresh.Disabled = false;
    }
    public void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F50111":
                SQuery = "SELECT '46' AS FSTR,'Sales Schedule' as NAME,'46' AS CODE FROM dual";
                break;
        }
    }
    public void make_qry_4_popup()
    {
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME1");
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
            case "tacode":
                if (frm_cocd == "INFI")
                {
                    if (ulvl == "0") SQuery = "select ANAME as PArty,Acode as Party_Code,Addr1 as Address,Addr2 as City,buycode as Old,payment from famst  where substr(acode,1,2) in ('16','02')  order by aname ";
                    else
                    {
                        col1 = ""; col2 = "";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "select acode,EVASID from famst where trim(evasid) <> '-'");

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["EVASID"].ToString().Trim().Contains(col1))
                            {
                                if (col2.Length > 0) col2 = col2 + "," + "'" + dr["acode"].ToString().Trim() + "'";
                                else col2 = "'" + dr["acode"].ToString().Trim() + "'";
                            }
                        }
                        SQuery = "select acode as fstr,ANAME as PArty,Acode as Party_Code,Addr1 as Address,Addr2 as City,buycode as Old,payment from famst where trim(acode) in (" + col2 + ") and substr(acode,0,2) in ('16','02')";
                    }
                }
                else SQuery = "select ANAME as PArty,Acode as Party_Code,Addr1 as Address,Addr2 as City,buycode as Old,payment from famst  where substr(acode,1,2) in ('16','02')  order by aname ";
                break;
            case "ticode":
                SQuery = "select icode as fstr,Iname as Item,Icode as Item_code,Cpartno,Cdrgno from Item where substr(icode,1,1) in ('9') and length(Trim(icode))>4 order by Iname";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    //    SQuery = "select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as CostSheet_no,to_char(a.vchdate,'dd/mm/yyyy') as CostSheet_dt,(case when trim(nvl(b.aname,'-'))='-' then a.t120 else b.aname end) as party_name ,a.t91 as Line_no from somas_anx a left outer join famst b on trim(a.acode)=trim(b.acode) where a.branchcd='" + frm_mbr + "' AND a.type='MM' and a.VCHDATE " + DateRange + " order by a.vchdate desc ,a.vchnum desc";
                    SQuery = "select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as CostSheet_no,to_char(a.vchdate,'dd/mm/yyyy') as CostSheet_dt,A.ANAME as party_name ,a.t91 as Line_no from somas_anx a left outer join famst b on trim(a.acode)=trim(b.acode) where a.branchcd='" + frm_mbr + "' AND a.type='MM' and a.VCHDATE " + DateRange + " order by a.vchdate desc ,a.vchnum desc";
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

    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        if (frm_cocd == "PRIN" && frm_mbr != "00") //FOR PRIN ENABLE ONLY FOR UNIT-III(00 MBR)... AS PER BANSAL SIR...27.12.19
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form.Please select " + frm_mbr + " branch!!");
            return;
        }
        else
        {
            chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
            clearctrl();
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
    }
    void newCase(string vty)
    {
        #region
        vty = "MM";
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        string mq1 = "";
        mq1 = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ";
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, mq1, 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdt.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        //btnlbl4.Focus();
        #endregion
    }
    //-----------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        if (frm_cocd == "PRIN" && frm_mbr != "00") //FOR PRIN ENABLE ONLY FOR UNIT-III(00 MBR)... AS PER BANSAL SIR...27.12.19
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form.Please select " + frm_mbr + " branch!!");
            return;
        }
        else
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
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
        //-------------------------
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
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
                case "tacode":
                    txtacode.Text = col2.Trim().ToString(); txtaname.Text = col1.Trim().ToString();
                    btnicode.Focus();
                    break;
                case "ticode":
                    txticode.Text = col3.Trim().ToString(); txtiname.Text = col2.Trim().ToString();
                    break;
                //case "Del":
                //    edmode.Value = col1;
                //    hffield.Value = "D";
                //    fgen.msg("-", "CMSG", "Are You Sure, You want to Delete !!");
                //    break;
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
                case "Print_E":
                    //fgen.Print_Report(frm_cocd,frm_qstr mbr, "select p.*,k.col from (SELECT X.*,(case when trim(nvl(Y.INAME,'-'))='-' then x.t121 else Y.INAME end) as INAME FROM (select a.*, (case when trim(nvl(b.aname,'-'))='-' then a.t120 else b.aname end) as aname from (select * from somas_anx where BRANCHCD||TYPE||TRIM(vchnum)||TO_CHAr(vchdate,'DD/MM/YYYY') in ('" + col1 + "')) a left outer join famst B on trim(a.acode)=trim(b.acode) ) X LEFT OUTER JOIN ITEM Y ON TRIM(X.ICODE)=TRIM(Y.ICODE)) p left outer join (select distinct col2,docdate,count(col1) as col from scratch where branchcd='AM' and type='TC' group by col2,docdate) k on trim(p.vchnum)=trim(k.col2)", "costing_sheet_MMPL", "costing_sheet_MMPL");
                    fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, "select p.*,k.col from (SELECT X.*,(case when trim(nvl(Y.INAME,'-'))='-' then x.t121 else Y.INAME end) as INAME FROM (select a.*, (case when trim(nvl(b.aname,'-'))='-' then a.t120 else b.aname end) as aname from (select * from somas_anx where BRANCHCD||TYPE||TRIM(vchnum)||TO_CHAr(vchdate,'DD/MM/YYYY') in ('" + col1 + "')) a left outer join famst B on trim(a.acode)=trim(b.acode) ) X LEFT OUTER JOIN ITEM Y ON TRIM(X.ICODE)=TRIM(Y.ICODE)) p left outer join (select distinct col2,docdate,count(col1) as col from scratch where branchcd='AM' and type='TC' group by col2,docdate) k on trim(p.vchnum)=trim(k.col2)", "costing_sheet_MMPL", "costing_sheet_MMPL");
                    break;
                case "Edit_E":
                    #region
                    if (col1 == "") return;
                    dt = new DataTable(); DataTable dt1 = new DataTable(); DataTable dt2 = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select * from somas_anx where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'");
                    //  dt1 = fgen.getdata(frm_qstr, frm_cocd, "select aname from famst where acode='" + dt.Rows[0]["acode"].ToString().Trim() + "'");
                    //  dt2 = fgen.getdata(frm_qstr, frm_cocd, "select iname from item where icode='" + dt.Rows[0]["icode"].ToString().Trim() + "'");
                    txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim(); txtvchdt.Text = col3.Trim(); txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txticode.Text = dt.Rows[0]["icode"].ToString().Trim();
                    txtaname.Text = dt.Rows[0]["aname"].ToString().Trim(); txtiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                    txtamd.Visible = false;
                    vchnum = fgen.next_no(frm_qstr, frm_cocd, "select max(col1) as vch from scratch where type='TC' and branchcd='AM' and trim(col2)='" + dt.Rows[0]["vchnum"].ToString().Trim() + "'", 2, "vch");
                    txtamd.Text = vchnum.Trim();

                    // if (dt1.Rows.Count > 0)
                    // {
                    // txtaname.Text = dt1.Rows[0]["aname"].ToString().Trim();
                    // txtiname.Text = dt2.Rows[0]["iname"].ToString().Trim();
                    // }
                    if (frm_cocd == "MMPL" || frm_cocd == "SYDB" || frm_cocd == "MFLX" || frm_cocd == "PAIL" || frm_cocd == "STLC" || frm_cocd == "HPPI" || frm_cocd == "SPPI" || frm_cocd == "OMNI" || frm_cocd == "MLAB" || frm_cocd == "PRIN")
                    {
                        #region
                        if (dt.Rows[0]["cdrgno"].ToString().Trim() == "ROTO")
                        {
                            txtrt3.Text = dt.Rows[0]["t1"].ToString().Trim();
                            txtrt5.Text = dt.Rows[0]["t2"].ToString().Trim();
                            txtrt6.Text = dt.Rows[0]["t3"].ToString().Trim();
                            txtrt7.Text = dt.Rows[0]["t4"].ToString().Trim();

                            txtrt9.Text = dt.Rows[0]["t5"].ToString().Trim();
                            txtrt11.Text = dt.Rows[0]["t6"].ToString().Trim();
                            txtrt12.Text = dt.Rows[0]["t7"].ToString().Trim();
                            txtrt13.Text = dt.Rows[0]["t8"].ToString().Trim();

                            txtrt15.Text = dt.Rows[0]["t9"].ToString().Trim();
                            txtrt17.Text = dt.Rows[0]["t10"].ToString().Trim();
                            txtrt18.Text = dt.Rows[0]["t11"].ToString().Trim();
                            txtrt19.Text = dt.Rows[0]["t12"].ToString().Trim();

                            txtrt21.Text = dt.Rows[0]["t13"].ToString().Trim();
                            txtrt23.Text = dt.Rows[0]["t14"].ToString().Trim();
                            txtrt24.Text = dt.Rows[0]["t15"].ToString().Trim();
                            txtrt25.Text = dt.Rows[0]["t16"].ToString().Trim();

                            txtrt27.Text = dt.Rows[0]["t17"].ToString().Trim();
                            txtrt29.Text = dt.Rows[0]["t18"].ToString().Trim();
                            txtrt30.Text = dt.Rows[0]["t19"].ToString().Trim();
                            txtrt31.Text = dt.Rows[0]["t20"].ToString().Trim();

                            txtrt48.Text = dt.Rows[0]["t21"].ToString().Trim();
                            txtrt50.Text = dt.Rows[0]["t22"].ToString().Trim();
                            txtrt51.Text = dt.Rows[0]["t23"].ToString().Trim();
                            txtrt52.Text = dt.Rows[0]["t24"].ToString().Trim();

                            txtrt53.Text = dt.Rows[0]["t25"].ToString().Trim();
                            txtrt55.Text = dt.Rows[0]["t26"].ToString().Trim();
                            txtrt56.Text = dt.Rows[0]["t27"].ToString().Trim();
                            txtrt57.Text = dt.Rows[0]["t28"].ToString().Trim();

                            txtrt58.Text = dt.Rows[0]["t29"].ToString().Trim();
                            txtrt60.Text = dt.Rows[0]["t30"].ToString().Trim();
                            txtrt61.Text = dt.Rows[0]["t31"].ToString().Trim();
                            txtrt62.Text = dt.Rows[0]["t32"].ToString().Trim();

                            txtrt63.Text = dt.Rows[0]["t33"].ToString().Trim();
                            txtrt65.Text = dt.Rows[0]["t34"].ToString().Trim();
                            txtrt66.Text = dt.Rows[0]["t35"].ToString().Trim();
                            txtrt67.Text = dt.Rows[0]["t36"].ToString().Trim();

                            txtrt68.Text = dt.Rows[0]["t37"].ToString().Trim();
                            txtrt70.Text = dt.Rows[0]["t38"].ToString().Trim();
                            txtrt71.Text = dt.Rows[0]["t39"].ToString().Trim();
                            txtrt72.Text = dt.Rows[0]["t40"].ToString().Trim();

                            txtrt73.Text = dt.Rows[0]["t41"].ToString().Trim();
                            txtrt75.Text = dt.Rows[0]["t42"].ToString().Trim();
                            txtrt76.Text = dt.Rows[0]["t43"].ToString().Trim();
                            txtrt77.Text = dt.Rows[0]["t44"].ToString().Trim();

                            txtrt78.Text = dt.Rows[0]["t45"].ToString().Trim();
                            txtrt80.Text = dt.Rows[0]["t46"].ToString().Trim();
                            txtrt81.Text = dt.Rows[0]["t47"].ToString().Trim();
                            txtrt82.Text = dt.Rows[0]["t48"].ToString().Trim();

                            txtrt83.Text = dt.Rows[0]["t49"].ToString().Trim();
                            txtrt85.Text = dt.Rows[0]["t50"].ToString().Trim();
                            txtrt86.Text = dt.Rows[0]["t51"].ToString().Trim();
                            txtrt87.Text = dt.Rows[0]["t52"].ToString().Trim();

                            txtrt88.Text = dt.Rows[0]["t53"].ToString().Trim();
                            txtrt90.Text = dt.Rows[0]["t54"].ToString().Trim();
                            txtrt91.Text = dt.Rows[0]["t55"].ToString().Trim();
                            txtrt92.Text = dt.Rows[0]["t56"].ToString().Trim();

                            txtrt93.Text = dt.Rows[0]["t57"].ToString().Trim();
                            txtrt95.Text = dt.Rows[0]["t58"].ToString().Trim();
                            txtrt96.Text = dt.Rows[0]["t59"].ToString().Trim();
                            txtrt97.Text = dt.Rows[0]["t60"].ToString().Trim();

                            txtrt98.Text = dt.Rows[0]["t61"].ToString().Trim();
                            txtrt100.Text = dt.Rows[0]["t62"].ToString().Trim();
                            txtrt101.Text = dt.Rows[0]["t63"].ToString().Trim();
                            txtrt102.Text = dt.Rows[0]["t64"].ToString().Trim();

                            txtrt103.Text = dt.Rows[0]["t65"].ToString().Trim();
                            txtrt105.Text = dt.Rows[0]["t66"].ToString().Trim();
                            txtrt106.Text = dt.Rows[0]["t67"].ToString().Trim();
                            txtrt107.Text = dt.Rows[0]["t68"].ToString().Trim();

                            txtrt108.Text = dt.Rows[0]["t69"].ToString().Trim();
                            txtrt110.Text = dt.Rows[0]["t70"].ToString().Trim();
                            txtrt111.Text = dt.Rows[0]["t71"].ToString().Trim();
                            txtrt112.Text = dt.Rows[0]["t72"].ToString().Trim();

                            //tot
                            txtrt33.Text = dt.Rows[0]["t73"].ToString().Trim();
                            txtrt113.Text = dt.Rows[0]["t74"].ToString().Trim();
                            txtrt114.Text = dt.Rows[0]["t75"].ToString().Trim();

                            //wastage row
                            txtrt115.Text = dt.Rows[0]["t76"].ToString().Trim();
                            txtrt116.Text = dt.Rows[0]["t77"].ToString().Trim();
                            txtrt117.Text = dt.Rows[0]["t79"].ToString().Trim();

                            //tot rmc
                            txtrt118.Text = dt.Rows[0]["t80"].ToString().Trim();
                            txtrt119.Text = dt.Rows[0]["t81"].ToString().Trim();

                            //selling price
                            txtrt35.Text = dt.Rows[0]["t82"].ToString().Trim();
                            txtrt36.Text = dt.Rows[0]["t83"].ToString().Trim();

                            //VA
                            txtrt37.Text = dt.Rows[0]["t84"].ToString().Trim();
                            txtrt38.Text = dt.Rows[0]["t85"].ToString().Trim();

                            //pouch dimenson 
                            txtrt120.Text = dt.Rows[0]["t86"].ToString().Trim();
                            txtrt121.Text = dt.Rows[0]["t87"].ToString().Trim();
                            txtrt122.Text = dt.Rows[0]["t88"].ToString().Trim();
                            txtrt123.Text = dt.Rows[0]["t89"].ToString().Trim();
                            txtrt124.Text = dt.Rows[0]["t90"].ToString().Trim();

                            //2nd last
                            txtrt41.Text = dt.Rows[0]["t91"].ToString().Trim();
                            txtrt42.Text = dt.Rows[0]["t92"].ToString().Trim();
                            txtrt43.Text = dt.Rows[0]["t93"].ToString().Trim();

                            //LAST
                            txtrt44.Text = dt.Rows[0]["t94"].ToString().Trim();
                            txtrt47.Text = dt.Rows[0]["t95"].ToString().Trim();

                            txtpayterms.Text = dt.Rows[0]["t96"].ToString().Trim();
                            txtdelvlocn.Text = dt.Rows[0]["t97"].ToString().Trim();
                            txtprt.Text = dt.Rows[0]["t98"].ToString().Trim();
                            txtannualqty.Text = dt.Rows[0]["t99"].ToString().Trim();

                            txtrt125.Text = dt.Rows[0]["t78"].ToString().Trim();
                            txtrt126.Text = dt.Rows[0]["t100"].ToString().Trim();

                            txtrt126.Text = dt.Rows[0]["t100"].ToString().Trim();
                            txtrt4.Text = dt.Rows[0]["t101"].ToString().Trim();
                            txtrt10.Text = dt.Rows[0]["t102"].ToString().Trim();
                            txtrt16.Text = dt.Rows[0]["t103"].ToString().Trim();
                            txtrt22.Text = dt.Rows[0]["t104"].ToString().Trim();
                            txtrt28.Text = dt.Rows[0]["t105"].ToString().Trim();
                            txtrt49.Text = dt.Rows[0]["t106"].ToString().Trim();
                            txtrt54.Text = dt.Rows[0]["t107"].ToString().Trim();
                            txtrt59.Text = dt.Rows[0]["t108"].ToString().Trim();
                            txtrt64.Text = dt.Rows[0]["t109"].ToString().Trim();
                            txtrt69.Text = dt.Rows[0]["t110"].ToString().Trim();
                            txtrt74.Text = dt.Rows[0]["t111"].ToString().Trim();
                            txtrt79.Text = dt.Rows[0]["t112"].ToString().Trim();
                            txtrt84.Text = dt.Rows[0]["t113"].ToString().Trim();
                            txtrt89.Text = dt.Rows[0]["t114"].ToString().Trim();
                            txtrt94.Text = dt.Rows[0]["t115"].ToString().Trim();
                            txtrt99.Text = dt.Rows[0]["t116"].ToString().Trim();
                            txtrt104.Text = dt.Rows[0]["t117"].ToString().Trim();
                            txtrt109.Text = dt.Rows[0]["t118"].ToString().Trim();

                            if (dt.Rows[0]["t119"].ToString().Trim() == "MANUAL")
                            {
                                txtaname.Text = dt.Rows[0]["t120"].ToString().Trim();
                                txtiname.Text = dt.Rows[0]["t121"].ToString().Trim();
                                btnacode.Visible = false; txtacode.Visible = false;
                                btnicode.Visible = false; txticode.Visible = false; hfname.Value = "MANUAL";
                            }
                            txtrt127.Text = dt.Rows[0]["t122"].ToString().Trim();
                            txtrt128.Text = dt.Rows[0]["t123"].ToString().Trim();
                            txtrt129.Text = dt.Rows[0]["t124"].ToString().Trim();
                            txtrt130.Text = dt.Rows[0]["t125"].ToString().Trim();
                            txtrt131.Text = dt.Rows[0]["t126"].ToString().Trim();
                            txtrt132.Text = dt.Rows[0]["t127"].ToString().Trim();
                            txtrt133.Text = dt.Rows[0]["t128"].ToString().Trim();
                            txtrt134.Text = dt.Rows[0]["t129"].ToString().Trim();
                        }
                        #endregion
                    }
                    else
                    {
                        #region
                        if (dt.Rows[0]["cdrgno"].ToString().Trim() == "FOLD")
                        {
                            txtff1.Text = dt.Rows[0]["t1"].ToString().Trim();
                            txtff2.Text = dt.Rows[0]["t2"].ToString().Trim();
                            txtff3.Text = dt.Rows[0]["t3"].ToString().Trim();
                            txtff4.Text = dt.Rows[0]["t4"].ToString().Trim();
                            txtff5.Text = dt.Rows[0]["t5"].ToString().Trim();
                            txtff6.Text = dt.Rows[0]["t6"].ToString().Trim();
                            txtff7.Text = dt.Rows[0]["t7"].ToString().Trim();
                            txtff8.Text = dt.Rows[0]["t8"].ToString().Trim();
                            txtff9.Text = dt.Rows[0]["t9"].ToString().Trim();
                            txtff10.Text = dt.Rows[0]["t10"].ToString().Trim();
                            txtff11.Text = dt.Rows[0]["t11"].ToString().Trim();
                            txtff12.Text = dt.Rows[0]["t12"].ToString().Trim();
                            txtff13.Text = dt.Rows[0]["t13"].ToString().Trim();
                            txtff14.Text = dt.Rows[0]["t14"].ToString().Trim();
                            txtff15.Text = dt.Rows[0]["t15"].ToString().Trim();
                            txtff16.Text = dt.Rows[0]["t16"].ToString().Trim();

                            txtff17.Text = dt.Rows[0]["t17"].ToString().Trim();
                            txtff18.Text = dt.Rows[0]["t18"].ToString().Trim();
                            txtff19.Text = dt.Rows[0]["t19"].ToString().Trim();
                            txtff20.Text = dt.Rows[0]["t20"].ToString().Trim();
                            txtff21.Text = dt.Rows[0]["t21"].ToString().Trim();
                            txtff22.Text = dt.Rows[0]["t22"].ToString().Trim();
                            txtff23.Text = dt.Rows[0]["t23"].ToString().Trim();
                            txtff24.Text = dt.Rows[0]["t24"].ToString().Trim();
                            txtff25.Text = dt.Rows[0]["t25"].ToString().Trim();
                            txtff26.Text = dt.Rows[0]["t26"].ToString().Trim();
                            txtff27.Text = dt.Rows[0]["t27"].ToString().Trim();
                            txtff28.Text = dt.Rows[0]["t28"].ToString().Trim();
                            txtff29.Text = dt.Rows[0]["t29"].ToString().Trim();
                            txtff30.Text = dt.Rows[0]["t30"].ToString().Trim();
                            txtff31.Text = dt.Rows[0]["t31"].ToString().Trim();
                            txtff32.Text = dt.Rows[0]["t32"].ToString().Trim();

                            txtff33.Text = dt.Rows[0]["t33"].ToString().Trim();
                            txtff34.Text = dt.Rows[0]["t34"].ToString().Trim();
                            txtff35.Text = dt.Rows[0]["t35"].ToString().Trim();
                            txtff36.Text = dt.Rows[0]["t36"].ToString().Trim();
                            txtff37.Text = dt.Rows[0]["t37"].ToString().Trim();
                            txtff38.Text = dt.Rows[0]["t38"].ToString().Trim();
                            txtff39.Text = dt.Rows[0]["t39"].ToString().Trim();
                            txtff40.Text = dt.Rows[0]["t40"].ToString().Trim();
                            txtff41.Text = dt.Rows[0]["t41"].ToString().Trim();
                            txtff42.Text = dt.Rows[0]["t42"].ToString().Trim();

                            txtff43.Text = dt.Rows[0]["t43"].ToString().Trim();
                            txtff44.Text = dt.Rows[0]["t44"].ToString().Trim();
                            txtff45.Text = dt.Rows[0]["t45"].ToString().Trim();
                            txtff46.Text = dt.Rows[0]["t46"].ToString().Trim();
                            txtff47.Text = dt.Rows[0]["t47"].ToString().Trim();
                            txtff48.Text = dt.Rows[0]["t48"].ToString().Trim();
                            txtff49.Text = dt.Rows[0]["t49"].ToString().Trim();
                            txtff50.Text = dt.Rows[0]["t50"].ToString().Trim();
                            txtff51.Text = dt.Rows[0]["t51"].ToString().Trim();

                            txtff52.Text = dt.Rows[0]["t52"].ToString().Trim();
                            txtff53.Text = dt.Rows[0]["t53"].ToString().Trim();
                            txtff54.Text = dt.Rows[0]["t54"].ToString().Trim();
                            txtff55.Text = dt.Rows[0]["t55"].ToString().Trim();
                            txtff56.Text = dt.Rows[0]["t56"].ToString().Trim();
                            txtff57.Text = dt.Rows[0]["t57"].ToString().Trim();
                            txtff58.Text = dt.Rows[0]["t58"].ToString().Trim();
                            txtff59.Text = dt.Rows[0]["t59"].ToString().Trim();
                            txtff60.Text = dt.Rows[0]["t60"].ToString().Trim();

                            txtpayterms.Text = dt.Rows[0]["t90"].ToString().Trim(); txtdelvlocn.Text = dt.Rows[0]["t91"].ToString().Trim();
                            txtprt.Text = dt.Rows[0]["t92"].ToString().Trim(); txtannualqty.Text = dt.Rows[0]["t93"].ToString().Trim();
                        }
                        #endregion
                        else if (dt.Rows[0]["cdrgno"].ToString().Trim() == "CORR")
                        {

                        }
                        else if (dt.Rows[0]["cdrgno"].ToString().Trim() == "ROTO")
                        {
                            #region
                            txtrt1.Text = dt.Rows[0]["t1"].ToString().Trim();
                            txtrt2.Text = dt.Rows[0]["t2"].ToString().Trim();
                            txtrt3.Text = dt.Rows[0]["t3"].ToString().Trim();
                            txtrt4.Text = dt.Rows[0]["t4"].ToString().Trim();
                            txtrt5.Text = dt.Rows[0]["t5"].ToString().Trim();
                            txtrt6.Text = dt.Rows[0]["t6"].ToString().Trim();
                            txtrt7.Text = dt.Rows[0]["t7"].ToString().Trim();
                            txtrt8.Text = dt.Rows[0]["t8"].ToString().Trim();
                            txtrt9.Text = dt.Rows[0]["t9"].ToString().Trim();
                            txtrt10.Text = dt.Rows[0]["t10"].ToString().Trim();
                            txtrt11.Text = dt.Rows[0]["t11"].ToString().Trim();
                            txtrt12.Text = dt.Rows[0]["t12"].ToString().Trim();
                            txtrt13.Text = dt.Rows[0]["t13"].ToString().Trim();
                            txtrt14.Text = dt.Rows[0]["t14"].ToString().Trim();
                            txtrt15.Text = dt.Rows[0]["t15"].ToString().Trim();
                            txtrt16.Text = dt.Rows[0]["t16"].ToString().Trim();

                            txtrt17.Text = dt.Rows[0]["t17"].ToString().Trim();
                            txtrt18.Text = dt.Rows[0]["t18"].ToString().Trim();
                            txtrt19.Text = dt.Rows[0]["t19"].ToString().Trim();
                            txtrt20.Text = dt.Rows[0]["t20"].ToString().Trim();
                            txtrt21.Text = dt.Rows[0]["t21"].ToString().Trim();
                            txtrt22.Text = dt.Rows[0]["t22"].ToString().Trim();
                            txtrt23.Text = dt.Rows[0]["t23"].ToString().Trim();
                            txtrt24.Text = dt.Rows[0]["t24"].ToString().Trim();
                            txtrt25.Text = dt.Rows[0]["t25"].ToString().Trim();
                            txtrt26.Text = dt.Rows[0]["t26"].ToString().Trim();
                            txtrt27.Text = dt.Rows[0]["t27"].ToString().Trim();
                            txtrt28.Text = dt.Rows[0]["t28"].ToString().Trim();
                            txtrt29.Text = dt.Rows[0]["t29"].ToString().Trim();
                            txtrt30.Text = dt.Rows[0]["t30"].ToString().Trim();
                            txtrt31.Text = dt.Rows[0]["t31"].ToString().Trim();
                            txtrt32.Text = dt.Rows[0]["t32"].ToString().Trim();

                            txtrt33.Text = dt.Rows[0]["t33"].ToString().Trim();
                            txtrt34.Text = dt.Rows[0]["t34"].ToString().Trim();
                            txtrt35.Text = dt.Rows[0]["t35"].ToString().Trim();
                            txtrt36.Text = dt.Rows[0]["t36"].ToString().Trim();
                            txtrt37.Text = dt.Rows[0]["t37"].ToString().Trim();
                            txtrt38.Text = dt.Rows[0]["t38"].ToString().Trim();
                            txtrt39.Text = dt.Rows[0]["t39"].ToString().Trim();
                            txtrt40.Text = dt.Rows[0]["t40"].ToString().Trim();
                            txtrt41.Text = dt.Rows[0]["t41"].ToString().Trim();
                            txtrt42.Text = dt.Rows[0]["t42"].ToString().Trim();

                            txtrt43.Text = dt.Rows[0]["t43"].ToString().Trim();
                            txtrt44.Text = dt.Rows[0]["t44"].ToString().Trim();
                            txtrt45.Text = dt.Rows[0]["t45"].ToString().Trim();
                            txtrt46.Text = dt.Rows[0]["t46"].ToString().Trim();
                            txtrt47.Text = dt.Rows[0]["t47"].ToString().Trim();
                            #endregion
                        }
                        else if (dt.Rows[0]["cdrgno"].ToString().Trim() == "LABL")
                        {
                            #region
                            txtlt1.Text = dt.Rows[0]["t1"].ToString().Trim();
                            txtlt2.Text = dt.Rows[0]["t2"].ToString().Trim();
                            txtlt3.Text = dt.Rows[0]["t3"].ToString().Trim();
                            txtlt4.Text = dt.Rows[0]["t4"].ToString().Trim();
                            txtlt5.Text = dt.Rows[0]["t5"].ToString().Trim();
                            txtlt6.Text = dt.Rows[0]["t6"].ToString().Trim();
                            txtlt7.Text = dt.Rows[0]["t7"].ToString().Trim();
                            txtlt8.Text = dt.Rows[0]["t8"].ToString().Trim();
                            txtlt9.Text = dt.Rows[0]["t9"].ToString().Trim();
                            txtlt10.Text = dt.Rows[0]["t10"].ToString().Trim();
                            txtlt11.Text = dt.Rows[0]["t11"].ToString().Trim();
                            txtlt12.Text = dt.Rows[0]["t12"].ToString().Trim();
                            txtlt13.Text = dt.Rows[0]["t13"].ToString().Trim();
                            txtlt14.Text = dt.Rows[0]["t14"].ToString().Trim();
                            txtlt15.Text = dt.Rows[0]["t15"].ToString().Trim();
                            txtlt16.Text = dt.Rows[0]["t16"].ToString().Trim();

                            txtlt17.Text = dt.Rows[0]["t17"].ToString().Trim();
                            txtlt18.Text = dt.Rows[0]["t18"].ToString().Trim();
                            txtlt19.Text = dt.Rows[0]["t19"].ToString().Trim();
                            txtlt20.Text = dt.Rows[0]["t20"].ToString().Trim();
                            txtlt21.Text = dt.Rows[0]["t21"].ToString().Trim();
                            txtlt22.Text = dt.Rows[0]["t22"].ToString().Trim();
                            txtlt23.Text = dt.Rows[0]["t23"].ToString().Trim();
                            txtlt24.Text = dt.Rows[0]["t24"].ToString().Trim();
                            txtlt25.Text = dt.Rows[0]["t25"].ToString().Trim();
                            txtlt26.Text = dt.Rows[0]["t26"].ToString().Trim();
                            txtlt27.Text = dt.Rows[0]["t27"].ToString().Trim();
                            txtlt28.Text = dt.Rows[0]["t28"].ToString().Trim();
                            txtlt29.Text = dt.Rows[0]["t29"].ToString().Trim();
                            txtlt30.Text = dt.Rows[0]["t30"].ToString().Trim();
                            #endregion
                        }
                    }
                    dt.Dispose();
                    ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                    ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                    fgen.EnableForm(this.Controls);
                    disablectrl();
                    setColHeadings();
                    edmode.Value = "Y";
                    #endregion
                    break;
            }
        }
    }

    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        if (frm_cocd == "PRIN" && frm_mbr != "00") //FOR PRIN ENABLE ONLY FOR UNIT-III(00 MBR)... AS PER BANSAL SIR...27.12.19
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form.Please select " + frm_mbr + " branch!!");
            return;
        }
        else
        {
            hffield.Value = "List";
            fgen.Fn_open_prddmp1("-", frm_qstr);
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME1");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            //  SQuery = "SELECT a.VCHNUM AS ENTRY_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS ENTRY_dT,to_char(a.vchdate,'yyyymmdd') as vdd,nvl(a.COL1,'-') AS MATERIAL,nvl(a.COL2,'-') AS FOIL,nvl(a.num1,0) as trim_wastage,nvl(a.num2,0) as process_wastage,nvl(a.num3,0) as paper_film,nvl(a.num4,0) as varnish_gsm,nvl(a.num5,0) as varnish_used,nvl(A.num6,0) as overheads,nvl(a.num7,0) as profit,nvl(a.num8,0) as foil_value,nvl(a.num9,0) as ink_gsm,nvl(num10,0) as ink,nvl(num11,0) as varnish,nvl(num12,0) as varnish_papr_ink_foil_totval,nvl(num13,0) as O_H,nvl(num14,0) as total_Cost,nvl(num15,0) as profit1,nvl(num16,0) as sp,nvl(num17,0) as width,nvl(num18,0) as height,nvl(num19,0) as sq_inch,nvl(num20,0) as sq_inch_of_lbl,nvl(num21,0) as price_per_thousand,nvl(num22,0) as wastage,nvl(a.num23,0) as price_per_pc,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_Dt  from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + "  order by vdd desc,a.vchnum desc,a.srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " For The Period Of " + fromdt + " To " + todt, frm_qstr);
            hffield.Value = "-";
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
                    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdt.Text.ToString()))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdt.Text.ToString() + ",Please Check !!");
                    }
                }
            }
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
            }
            else
            {
                if (Checked_ok == "Y")
                {
                    try
                    {
                        oDS = new DataSet();
                        oporow = null;
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);//somas_anx

                        oDS1 = new DataSet();
                        oporow = null;
                        oDS1 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);//scratch

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();

                        oDS.Dispose();
                        oDS1.Dispose();

                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oporow = null;
                        oDS1 = new DataSet();
                        oDS1 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);

                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnum.Text.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            save_it = "Y";
                            //for (i = 0; i < sg1.Rows.Count - 0; i++)
                            //{
                            //    save_it = "Y";
                            //}
                            if (save_it == "Y")
                            {
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdt.Text.Trim(), frm_uname, Prg_Id);//somas_Anx
                                //  frm_vnum1 = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname1, doc_nf.Value, doc_df.Value, frm_mbr, "AM", txtvchdt.Text.Trim(), frm_uname, Prg_Id); //SCRATCH
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                        }
                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);
                        save_fun();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(4, 16) + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);//for scratch
                            //cmd_query = "update " + frm_tabname1 + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + "AM" + "TC" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(4, 16) + "'";
                            cmd_query = "update " + frm_tabname1 + " set branchcd='DD' where branchcd||type||trim(COL2)||to_char(docdate,'dd/mm/yyyy')='" + "AM" + "TC" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(4, 16) + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);//for somas_anx
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS1, frm_tabname1);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(4, 16) + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            //cmd_query = "delete from " + frm_tabname1 + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + "AM" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(4, 16) + "'";
                            cmd_query = "delete from " + frm_tabname1 + " where branchcd||type||trim(COL2)||to_char(docdate,'dd/mm/yyyy')='DD" + "TC" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(4, 16) + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        //fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + txtvchdt.Text.Trim(), frm_uname, edmode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtvchdt.Text.Trim() + "'");
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

    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME1");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();
        #region
        if (edmode.Value.Trim() == "Y") vchnum = txtvchnum.Text.Trim();
        else vchnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from somas_anx where type='MM' and branchcd='" + frm_mbr + "' and vchdate " + DateRange + "", 6, "vch");
        oporow["vchnum"] = vchnum.Trim();
        oporow["vchdate"] = txtvchdt.Text.Trim();
        oporow["BRANCHCD"] = frm_mbr;
        oporow["TYPE"] = "MM";
        oporow["acode"] = txtacode.Text.Trim();//NOT SHOWING ON FORM
        oporow["ANAME"] = txtaname.Text.Trim();
        oporow["INAME"] = txtiname.Text.Trim();
        oporow["icode"] = txticode.Text.Trim();//NOT SHOWING ON FORM

        if (frm_cocd == "HPPI" || frm_cocd == "SPPI" || frm_cocd == "MMPL" || frm_cocd == "SYDB" || frm_cocd == "MFLX" || frm_cocd == "PAIL" || frm_cocd == "STLC" || frm_cocd == "OMNI" || frm_cocd == "MLAB" || frm_cocd == "PRIN")
        {
            #region
            oporow["CDRGNO"] = "ROTO";
            oporow["t1"] = txtrt3.Text.Trim();
            oporow["t2"] = txtrt5.Text.Trim();
            oporow["t3"] = txtrt6.Text.Trim();
            oporow["t4"] = txtrt7.Text.Trim();

            oporow["t5"] = txtrt9.Text.Trim();
            oporow["t6"] = txtrt11.Text.Trim();
            oporow["t7"] = txtrt12.Text.Trim();
            oporow["t8"] = txtrt13.Text.Trim();

            oporow["t9"] = txtrt15.Text.Trim();
            oporow["t10"] = txtrt17.Text.Trim();
            oporow["t11"] = txtrt18.Text.Trim();
            oporow["t12"] = txtrt19.Text.Trim();

            oporow["t13"] = txtrt21.Text.Trim();
            oporow["t14"] = txtrt23.Text.Trim();
            oporow["t15"] = txtrt24.Text.Trim();
            oporow["t16"] = txtrt25.Text.Trim();

            oporow["t17"] = txtrt27.Text.Trim();
            oporow["t18"] = txtrt29.Text.Trim();
            oporow["t19"] = txtrt30.Text.Trim();
            oporow["t20"] = txtrt31.Text.Trim();

            oporow["t21"] = txtrt48.Text.Trim();
            oporow["t22"] = txtrt50.Text.Trim();
            oporow["t23"] = txtrt51.Text.Trim();
            oporow["t24"] = txtrt52.Text.Trim();

            oporow["t25"] = txtrt53.Text.Trim();
            oporow["t26"] = txtrt55.Text.Trim();
            oporow["t27"] = txtrt56.Text.Trim();
            oporow["t28"] = txtrt57.Text.Trim();

            oporow["t29"] = txtrt58.Text.Trim();
            oporow["t30"] = txtrt60.Text.Trim();
            oporow["t31"] = txtrt61.Text.Trim();
            oporow["t32"] = txtrt62.Text.Trim();

            oporow["t33"] = txtrt63.Text.Trim();
            oporow["t34"] = txtrt65.Text.Trim();
            oporow["t35"] = txtrt66.Text.Trim();
            oporow["t36"] = txtrt67.Text.Trim();

            oporow["t37"] = txtrt68.Text.Trim();
            oporow["t38"] = txtrt70.Text.Trim();
            oporow["t39"] = txtrt71.Text.Trim();
            oporow["t40"] = txtrt72.Text.Trim();

            oporow["t41"] = txtrt73.Text.Trim();
            oporow["t42"] = txtrt75.Text.Trim();
            oporow["t43"] = txtrt76.Text.Trim();
            oporow["t44"] = txtrt77.Text.Trim();

            oporow["t45"] = txtrt78.Text.Trim();
            oporow["t46"] = txtrt80.Text.Trim();
            oporow["t47"] = txtrt81.Text.Trim();
            oporow["t48"] = txtrt82.Text.Trim();

            oporow["t49"] = txtrt83.Text.Trim();
            oporow["t50"] = txtrt85.Text.Trim();
            oporow["t51"] = txtrt86.Text.Trim();
            oporow["t52"] = txtrt87.Text.Trim();

            oporow["t53"] = txtrt88.Text.Trim();
            oporow["t54"] = txtrt90.Text.Trim();
            oporow["t55"] = txtrt91.Text.Trim();
            oporow["t56"] = txtrt92.Text.Trim();

            oporow["t57"] = txtrt93.Text.Trim();
            oporow["t58"] = txtrt95.Text.Trim();
            oporow["t59"] = txtrt96.Text.Trim();
            oporow["t60"] = txtrt97.Text.Trim();

            oporow["t61"] = txtrt98.Text.Trim();
            oporow["t62"] = txtrt100.Text.Trim();
            oporow["t63"] = txtrt101.Text.Trim();
            oporow["t64"] = txtrt102.Text.Trim();

            oporow["t65"] = txtrt103.Text.Trim();
            oporow["t66"] = txtrt105.Text.Trim();
            oporow["t67"] = txtrt106.Text.Trim();
            oporow["t68"] = txtrt107.Text.Trim();

            oporow["t69"] = txtrt108.Text.Trim();
            oporow["t70"] = txtrt110.Text.Trim();
            oporow["t71"] = txtrt111.Text.Trim();
            oporow["t72"] = txtrt112.Text.Trim();

            //tot
            oporow["t73"] = txtrt33.Text.Trim();
            oporow["t74"] = txtrt113.Text.Trim();
            oporow["t75"] = txtrt114.Text.Trim();

            //wastage row
            oporow["t76"] = txtrt115.Text.Trim();
            oporow["t77"] = txtrt116.Text.Trim();
            oporow["t78"] = txtrt125.Text.Trim();
            oporow["t79"] = txtrt117.Text.Trim();

            //tot rmc
            oporow["t80"] = txtrt118.Text.Trim();
            oporow["t81"] = txtrt119.Text.Trim();

            //selling price
            oporow["t82"] = txtrt35.Text.Trim();
            oporow["t83"] = txtrt36.Text.Trim();

            //VA
            oporow["t84"] = txtrt37.Text.Trim();
            oporow["t85"] = txtrt38.Text.Trim();

            //pouch dimenson 
            oporow["t86"] = txtrt120.Text.Trim();
            oporow["t87"] = txtrt121.Text.Trim();
            oporow["t88"] = txtrt122.Text.Trim();
            oporow["t89"] = txtrt123.Text.Trim();
            oporow["t90"] = txtrt124.Text.Trim();

            //2nd last
            oporow["t91"] = txtrt41.Text.Trim();
            oporow["t92"] = txtrt42.Text.Trim();
            oporow["t93"] = txtrt43.Text.Trim();

            //LAST
            oporow["t94"] = txtrt44.Text.Trim();
            oporow["t95"] = txtrt47.Text.Trim();

            oporow["t96"] = txtpayterms.Text.Trim();
            oporow["t97"] = txtdelvlocn.Text.Trim();
            oporow["t98"] = txtprt.Text.Trim();
            oporow["t99"] = txtannualqty.Text.Trim();

            oporow["t100"] = txtrt126.Text.Trim();
            oporow["t101"] = txtrt4.Text.Trim();
            oporow["t102"] = txtrt10.Text.Trim();
            oporow["t103"] = txtrt16.Text.Trim();
            oporow["t104"] = txtrt22.Text.Trim();
            oporow["t105"] = txtrt28.Text.Trim();
            oporow["t106"] = txtrt49.Text.Trim();
            oporow["t107"] = txtrt54.Text.Trim();
            oporow["t108"] = txtrt59.Text.Trim();
            oporow["t109"] = txtrt64.Text.Trim();
            oporow["t110"] = txtrt69.Text.Trim();
            oporow["t111"] = txtrt74.Text.Trim();
            oporow["t112"] = txtrt79.Text.Trim();
            oporow["t113"] = txtrt84.Text.Trim();
            oporow["t114"] = txtrt89.Text.Trim();
            oporow["t115"] = txtrt94.Text.Trim();
            oporow["t116"] = txtrt99.Text.Trim();
            oporow["t117"] = txtrt104.Text.Trim();
            oporow["t118"] = txtrt109.Text.Trim();

            if (hfname.Value == "MANUAL")
            {
                oporow["t119"] = "MANUAL";
                if (txtaname.Text.Trim().Length > 30)
                {
                    oporow["t120"] = txtaname.Text.Trim().Substring(0, 29).ToUpper();
                    oporow["t121"] = txtiname.Text.Trim().Substring(0, 29).ToUpper();
                }
                else
                {
                    oporow["t120"] = txtaname.Text.Trim().ToUpper();
                    oporow["t121"] = txtiname.Text.Trim().ToUpper();
                }
            }
            oporow["t122"] = txtrt127.Text.Trim();
            oporow["t123"] = txtrt128.Text.Trim();
            oporow["t124"] = txtrt129.Text.Trim();
            oporow["t125"] = txtrt130.Text.Trim();
            oporow["t126"] = txtrt131.Text.Trim();
            oporow["t127"] = txtrt132.Text.Trim();
            oporow["t128"] = txtrt133.Text.Trim();
            oporow["t129"] = txtrt134.Text.Trim();
            #endregion
        }
        else
        {
            #region
            oporow["cdrgno"] = "FOLD";
            oporow["t1"] = txtff1.Text;
            oporow["t2"] = txtff2.Text;
            oporow["t3"] = txtff3.Text;
            oporow["t4"] = txtff4.Text;
            oporow["t5"] = txtff5.Text;
            oporow["t6"] = txtff6.Text;
            oporow["t7"] = txtff7.Text;
            oporow["t8"] = txtff8.Text;
            oporow["t9"] = txtff9.Text;
            oporow["t10"] = txtff10.Text;
            oporow["t11"] = txtff11.Text;
            oporow["t12"] = txtff12.Text;
            oporow["t13"] = txtff13.Text;
            oporow["t14"] = txtff14.Text;
            oporow["t15"] = txtff15.Text;
            oporow["t16"] = txtff16.Text;
            oporow["t17"] = txtff17.Text;
            oporow["t18"] = txtff18.Text;
            oporow["t19"] = txtff19.Text;
            oporow["t20"] = txtff20.Text;
            oporow["t21"] = txtff21.Text;
            oporow["t22"] = txtff22.Text;
            oporow["t23"] = txtff23.Text;
            oporow["t24"] = txtff24.Text;
            oporow["t25"] = txtff25.Text;
            oporow["t26"] = txtff26.Text;
            oporow["t27"] = txtff27.Text;
            oporow["t28"] = txtff28.Text;
            oporow["t29"] = txtff29.Text;
            oporow["t30"] = txtff30.Text;
            oporow["t31"] = txtff31.Text;
            oporow["t32"] = txtff32.Text;
            oporow["t33"] = txtff33.Text;
            oporow["t34"] = txtff34.Text;
            oporow["t35"] = txtff35.Text;
            oporow["t36"] = txtff36.Text;
            oporow["t37"] = txtff37.Text;
            oporow["t38"] = txtff38.Text;
            oporow["t39"] = txtff39.Text;
            oporow["t40"] = txtff40.Text;
            oporow["t41"] = txtff41.Text;
            oporow["t42"] = txtff42.Text;
            oporow["t43"] = txtff43.Text;
            oporow["t44"] = txtff44.Text;
            oporow["t45"] = txtff45.Text;
            oporow["t46"] = txtff46.Text;
            oporow["t47"] = txtff47.Text;
            oporow["t48"] = txtff48.Text;
            oporow["t49"] = txtff49.Text;
            oporow["t50"] = txtff50.Text;
            oporow["t51"] = txtff51.Text;
            oporow["t52"] = txtff52.Text;
            oporow["t53"] = txtff53.Text;
            oporow["t54"] = txtff54.Text;
            oporow["t55"] = txtff55.Text;
            oporow["t56"] = txtff56.Text;
            oporow["t57"] = txtff57.Text;
            oporow["t58"] = txtff58.Text;
            oporow["t59"] = txtff59.Text;
            oporow["t60"] = txtff60.Text;
            oporow["t90"] = txtpayterms.Text;
            oporow["t91"] = txtdelvlocn.Text;
            oporow["t92"] = txtprt.Text;
            oporow["t93"] = txtannualqty.Text;

            if (fgen.make_double(txtrt1.Text) > 0)
            {
                oporow["cdrgno"] = "ROTO";
                oporow["t1"] = txtrt1.Text.Trim();
                oporow["t2"] = txtrt2.Text.Trim();
                oporow["t3"] = txtrt3.Text.Trim();
                oporow["t4"] = txtrt4.Text.Trim();
                oporow["t5"] = txtrt5.Text.Trim();
                oporow["t6"] = txtrt6.Text.Trim();
                oporow["t7"] = txtrt7.Text.Trim();
                oporow["t8"] = txtrt8.Text.Trim();
                oporow["t9"] = txtrt9.Text.Trim();
                oporow["t10"] = txtrt10.Text.Trim();
                oporow["t11"] = txtrt11.Text.Trim();
                oporow["t12"] = txtrt12.Text.Trim();
                oporow["t13"] = txtrt13.Text.Trim();
                oporow["t14"] = txtrt14.Text.Trim();
                oporow["t15"] = txtrt15.Text.Trim();
                oporow["t16"] = txtrt16.Text.Trim();
                oporow["t17"] = txtrt17.Text.Trim();
                oporow["t18"] = txtrt18.Text.Trim();
                oporow["t19"] = txtrt19.Text.Trim();
                oporow["t20"] = txtrt20.Text.Trim();
                oporow["t21"] = txtrt21.Text.Trim();
                oporow["t22"] = txtrt22.Text.Trim();
                oporow["t23"] = txtrt23.Text.Trim();
                oporow["t24"] = txtrt24.Text.Trim();
                oporow["t25"] = txtrt25.Text.Trim();
                oporow["t26"] = txtrt26.Text.Trim();
                oporow["t27"] = txtrt27.Text.Trim();
                oporow["t28"] = txtrt28.Text.Trim();
                oporow["t29"] = txtrt29.Text.Trim();
                oporow["t30"] = txtrt30.Text.Trim();
                oporow["t31"] = txtrt31.Text.Trim();
                oporow["t32"] = txtrt32.Text.Trim();
                oporow["t33"] = txtrt33.Text.Trim();
                oporow["t34"] = txtrt34.Text.Trim();
                oporow["t35"] = txtrt35.Text.Trim();
                oporow["t36"] = txtrt36.Text.Trim();
                oporow["t37"] = txtrt37.Text.Trim();
                oporow["t38"] = txtrt38.Text.Trim();
                oporow["t39"] = txtrt39.Text.Trim();
                oporow["t40"] = txtrt40.Text.Trim();
                oporow["t41"] = txtrt41.Text.Trim();
                oporow["t42"] = txtrt42.Text.Trim();
                oporow["t43"] = txtrt43.Text.Trim();
                oporow["t44"] = txtrt44.Text.Trim();
                oporow["t45"] = txtrt45.Text.Trim();
                oporow["t46"] = txtrt46.Text.Trim();
                oporow["t47"] = txtrt47.Text.Trim();
            }
            if (fgen.make_double(txtlt1.Text) > 0)
            {
                oporow["cdrgno"] = "LABL";
                oporow["t1"] = txtlt1.Text.Trim();
                oporow["t2"] = txtlt2.Text.Trim();
                oporow["t3"] = txtlt3.Text.Trim();
                oporow["t4"] = txtlt4.Text.Trim();
                oporow["t5"] = txtlt5.Text.Trim();
                oporow["t6"] = txtlt6.Text.Trim();
                oporow["t7"] = txtlt7.Text.Trim();
                oporow["t8"] = txtlt8.Text.Trim();
                oporow["t9"] = txtlt9.Text.Trim();
                oporow["t10"] = txtlt10.Text.Trim();
                oporow["t11"] = txtlt11.Text.Trim();
                oporow["t12"] = txtlt12.Text.Trim();
                oporow["t13"] = txtlt13.Text.Trim();
                oporow["t14"] = txtlt14.Text.Trim();
                oporow["t15"] = txtlt15.Text.Trim();
                oporow["t16"] = txtlt16.Text.Trim();
                oporow["t17"] = txtlt17.Text.Trim();
                oporow["t18"] = txtlt18.Text.Trim();
                oporow["t19"] = txtlt19.Text.Trim();
                oporow["t20"] = txtlt20.Text.Trim();
                oporow["t21"] = txtlt21.Text.Trim();
                oporow["t22"] = txtlt22.Text.Trim();
                oporow["t23"] = txtlt23.Text.Trim();
                oporow["t24"] = txtlt24.Text.Trim();
                oporow["t25"] = txtlt25.Text.Trim();
                oporow["t26"] = txtlt26.Text.Trim();
                oporow["t27"] = txtlt27.Text.Trim();
                oporow["t28"] = txtlt28.Text.Trim();
                oporow["t29"] = txtlt29.Text.Trim();
                oporow["t30"] = txtlt30.Text.Trim();
            }

            #endregion
        }
        if (edmode.Value == "Y")
        {
            oporow["eNt_by"] = uname;
            oporow["eNt_dt"] = todt;
            oporow["edt_by"] = uname;
            oporow["edt_dt"] = todt;
        }
        else
        {
            oporow["eNt_by"] = uname;
            oporow["eNt_dt"] = todt;
            oporow["edt_by"] = "-";
            oporow["edt_dt"] = todt;
        }
        oDS.Tables[0].Rows.Add(oporow);

        if ((frm_cocd == "HPPI" || frm_cocd == "SPPI" || frm_cocd == "MMPL" || frm_cocd == "SYDB" || frm_cocd == "MFLX" || frm_cocd == "PAIL" || frm_cocd == "STLC" || frm_cocd == "OMNI" || frm_cocd == "MLAB" || frm_cocd == "PRIN") && edmode.Value == "Y")
        {
            oporow = oDS1.Tables[0].NewRow();
            txtamd.Text = fgen.next_no(frm_qstr, frm_cocd, "select max(col1) as vch from scratch where  branchcd='AM' and type='TC' and trim(col2)='" + txtvchnum.Text.Trim() + "'", 2, "vch");
            oporow["icode"] = txticode.Text.Trim();
            oporow["acode"] = txtacode.Text.Trim();
            oporow["ANAME"] = txtaname.Text.Trim();
            oporow["INAME"] = txtiname.Text.Trim();
            oporow["type"] = "TC";
            oporow["branchcd"] = "AM";
            oporow["col1"] = txtamd.Text.Trim();
            oporow["col2"] = txtvchnum.Text.Trim();
            oporow["docdate"] = txtvchdt.Text.Trim();

            if (edmode.Value == "Y")
            {
                oporow["eNt_by"] = uname;
                oporow["eNt_dt"] = todt;
                oporow["edt_by"] = "-";
                oporow["edt_dt"] = todt;
            }
            oDS1.Tables[0].Rows.Add(oporow);
        }
        #endregion
    }
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        if (frm_cocd == "PRIN" && frm_mbr != "00") //FOR PRIN ENABLE ONLY FOR UNIT-III(00 MBR)... AS PER BANSAL SIR...27.12.19
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form.Please select " + frm_mbr + " branch!!");
            return;
        }
        else
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
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        fgen.fill_zero(this.Controls);
        Cal();
        if (txtaname.Text == "" || txtaname.Text == "0")
        { fgen.msg("-", "AMSG", "Please select Customer Name"); return; }
        if (txtiname.Text == "" || txtiname.Text == "0")
        { fgen.msg("-", "AMSG", "Please select Item Name"); return; }
        //if (txtpayterms.Text == "" || txtpayterms.Text == "0")
        //{ fgen.msg("-", "AMSG", "Please fill Payment term"); return; }
        //if (txtdelvlocn.Text == "-" || txtdelvlocn.Text == "0")
        //{ fgen.msg("-", "AMSG", "Please fill Delv. Location"); return; }
        if (txtprt.Text == "-" || txtprt.Text == "0")
        { fgen.msg("-", "AMSG", "Please fill Min Order Qty"); return; }
        if (txtannualqty.Text == "-" || txtannualqty.Text == "0")
        { fgen.msg("-", "AMSG", "Please fill Annual Order Qty"); return; }
        //if (txtrt47.Text == "-" || txtrt47.Text == "0")
        //{ fgen.msg("-", "AMSG", "Selling Price should be more than 0"); return; }

        if (frm_cocd == "HPPI" || frm_cocd == "SPPI" || frm_cocd == "MMPL" || frm_cocd == "STLC" || frm_cocd == "MLAB" || frm_cocd == "SCPL" || frm_cocd == "SYDB" || frm_cocd == "MFLX" || frm_cocd == "PAIL" || frm_cocd == "OMNI" || frm_cocd == "PRIN") fgen.msg("-", "SMSG", "Are You Sure!! You want to save");
        else
        {
            if (txtff6.Text == "" || txtff13.Text == "" || txtff7.Text == "" || txtff8.Text == "" || txtff9.Text == "" || txtff10.Text == "" || txtprt.Text == "" || txtannualqty.Text == "")
                fgen.msg("-", "AMSG", "Please Fill Mandatory Field (yellow Color)");
            else
            {
                dfstring = ""; usg = 0;

                if (fgen.make_double(txtrt1.Text) > 0)
                {
                    dfstring = dfstring + ",Roto";
                    usg = usg + 1;
                }
                if (fgen.make_double(txtlt1.Text) > 0)
                {
                    dfstring = dfstring + ",Label";
                    usg = usg + 1;
                }
                if (usg > 1) fgen.msg("-", "AMSG", "Please use one of the Module for 1 Sheet Data Filled in " + dfstring + "");
                else
                {
                    hffield.Value = "SURE_S";
                    fgen.Fn_open_sseek("-", frm_qstr);
                }
            }
        }
    }
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
    }
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
    protected void btnacode_Click(object sender, EventArgs e)
    {
        hffield.Value = "tacode";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnicode_Click(object sender, EventArgs e)
    {
        hffield.Value = "ticode";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btncal_Click(object sender, EventArgs e)
    {
        Cal();
    }
    protected void rdform_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdform.SelectedValue == "0")
        {
            // pch1.Visible = true; pch2.Visible = true; pch3.Visible = true; pch4.Visible = true;
            pch1.Attributes.Add("style", "display:normal"); //
            pch2_.Attributes.Add("style", "display:normal");
            pch2_1.Attributes.Add("style", "display:normal");
            pch2.Attributes.Add("style", "display:normal");
            pch3.Attributes.Add("style", "display:normal");
            pch3_.Attributes.Add("style", "display:normal");
            pch4.Attributes.Add("style", "display:normal");
            Selling_price.Attributes.Add("style", "display:normal");
            //style="display:normal"
        }
        else
        {
            // pch1.Visible = false; pch2.Visible = false; pch3.Visible = false; pch4.Visible = false;
            pch1.Attributes.Add("style", "display:none");
            pch2_.Attributes.Add("style", "display:none");
            pch2_1.Attributes.Add("style", "display:none");
            pch2.Attributes.Add("style", "display:none");
            pch3.Attributes.Add("style", "display:none");
            pch3_.Attributes.Add("style", "display:none");
            pch4.Attributes.Add("style", "display:none");
            Selling_price.Attributes.Add("style", "display:none");
            this.ClientScript.RegisterStartupScript(this.GetType(), "show", "<script>document.getElementById('ContentPlaceHolder1_txtrt134').style.display = 'none'</script>");
        }
    }

    protected void txtaname_TextChanged(object sender, EventArgs e)
    {
        btnacode.Visible = false; txtacode.Visible = false;
        btnicode.Visible = false; txticode.Visible = false; txtiname.ReadOnly = false;
        txticode.Text = ""; txtacode.Text = "";
        hfname.Value = "MANUAL"; txtiname.Focus();
    }

    protected void btnrefresh_ServerClick(object sender, EventArgs e)
    {
        Cal();
    }
    void Cal()
    {
        double ff12 = 0, ff14 = 0, ff17 = 0, ff19 = 0, ff21 = 0, ff23 = 0, ff25 = 0, ff27 = 0, ff29 = 0;
        double ff31 = 0, ff34 = 0, ff36 = 0, ff38 = 0, ff40 = 0, ff42 = 0, ff44 = 0, ff49 = 0;
        double ff52 = 0, ff54 = 0, ff56 = 0, ff57 = 0, ff59 = 0, ff60 = 0;
        fgen.fill_zero(this.Controls);
        double pet_thick = 0; double pet_spgr = 0; double pet_gsm = 0; double pet_kgs = 0; double pet_sqm = 0; double nat_thick = 0; double nat_spgr = 0; double nat_gsm = 0; double nat_kgs = 0; double nat_sqm = 0;
        double pet_thick1 = 0; double pet_spgr1 = 0; double pet_gsm1 = 0; double pet_kgs1 = 0; double pet_sqm1 = 0;
        double wop_thick = 0; double wop_spgr = 0; double Wop_gsm = 0; double Wop_kgs = 0; double Wop_sqm = 0; double bop_thick = 0; double bop_spgr = 0; double bop_gsm = 0; double bop_kgs = 0; double bop_sqm = 0;
        double bop_thick1 = 0; double bop_spgr1 = 0; double bop_gsm1 = 0; double bop_kgs1 = 0; double bop_sqm1 = 0; double cpp_thick = 0; double cpp_spgr = 0; double cpp_gsm = 0; double cpp_kgs = 0; double cpp_sqm = 0;
        double cpp_thick1 = 0; double cpp_spgr1 = 0; double cpp_gsm1 = 0; double cpp_kgs1 = 0; double cpp_sqm1 = 0; double foil_thick = 0; double foil_spgr = 0; double foil_gsm = 0; double foil_kgs = 0; double foil_sqm = 0;
        double shrnk_thick = 0; double shrnk_spgr = 0; double shrnk_gsm = 0; double shrnk_kgs = 0; double shrnk_sqm = 0;
        double shrnk_thick1 = 0; double shrnk_spgr1 = 0; double shrnk_gsm1 = 0; double shrnk_kgs1 = 0; double shrnk_sqm1 = 0;
        double pe_nat_thick = 0; double pet_nat_spgr = 0; double pe_nat_gsm = 0; double pe_nat_kgs = 0; double pe_nat_sqm = 0;
        double pe_wop_thick = 0; double pe_wop_spgr = 0; double pe_wop_gsm = 0; double pe_wop_kgs = 0; double pe_wop_sqm = 0;
        double nyl_Thick = 0; double nyl_spgr = 0; double nyl_gsm = 0; double nyl_kgs = 0; double nyl_sqm = 0;
        double pap_thick = 0; double pap_spgr = 0; double pap_gsm = 0; double pap_kgs = 0; double pap_sqm = 0;
        double pearl_thick = 0; double pearl_spgr = 0; double pearl_gsm = 0; double pearl_kgs = 0; double pearl_sqm = 0;
        double adh_thick = 0; double adh_spgr = 0; double adh_gsm = 0; double adh_kgs = 0; double adh_sqm = 0;
        double adh1_thick = 0; double adh1_spgr = 0; double adh1_gsm = 0; double adh1_kgs = 0; double adh1_sqm = 0;
        double ink_thick = 0; double ink_spgr = 0; double ink_gsm = 0; double ink_kgs = 0; double ink_sqm = 0;
        double tot = 0; double tot1 = 0; double tot2 = 0; double tot3 = 0; double tot4 = 0; double tot5 = 0; double tot6 = 0; double tot7 = 0; double tot8 = 0; double tot9 = 0; double tot10 = 0;
        double tot11 = 0; double tot12 = 0; double tot13 = 0; double tot14 = 0; double tot15 = 0; double tot16 = 0; double tot17 = 0; double tot18 = 0; double tot19 = 0; double tot20 = 0;

        if (frm_cocd == "HPPI" || frm_cocd == "SPPI" || frm_cocd == "MMPL" || frm_cocd == "STLC" || frm_cocd == "MLAB" || frm_cocd == "SCPL" || frm_cocd == "SYDB" || frm_cocd == "MFLX" || frm_cocd == "PAIL" || frm_cocd == "OMNI" || frm_cocd == "PRIN")
        {
            #region
            pet_thick = fgen.make_double(txtrt3.Text.Trim());
            pet_spgr = fgen.make_double(txtrt4.Text.Trim());
            pet_gsm = pet_thick * pet_spgr;
            txtrt5.Text = Convert.ToString(Math.Round(pet_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            pet_kgs = fgen.make_double(txtrt6.Text.Trim());
            pet_sqm = (pet_gsm * pet_kgs) / 1000;
            txtrt7.Text = Convert.ToString(Math.Round(pet_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            ///for pet 2 nd row
            pet_thick1 = fgen.make_double(txtrt3a.Text.Trim());
            pet_spgr1 = fgen.make_double(txtrt4a.Text.Trim());
            pet_gsm1 = pet_thick1 * pet_spgr1;
            txtrt5a.Text = Convert.ToString(Math.Round(pet_gsm1, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            pet_kgs1 = fgen.make_double(txtrt6a.Text.Trim());
            pet_sqm1 = (pet_gsm1 * pet_kgs1) / 1000;
            txtrt7a.Text = Convert.ToString(Math.Round(pet_sqm1, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            ///=================for NAT (2ND ROW)               
            //   txtrt13.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtrt11.Text.Trim()) * Convert.ToDouble(txtrt12.Text.Trim())) / 1000, 2));
            nat_thick = fgen.make_double(txtrt9.Text.Trim());
            nat_spgr = fgen.make_double(txtrt10.Text.Trim());
            nat_gsm = nat_thick * nat_spgr;
            txtrt11.Text = Convert.ToString(Math.Round(nat_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            nat_kgs = fgen.make_double(txtrt12.Text.Trim());
            nat_sqm = (nat_gsm * nat_kgs) / 1000;
            txtrt13.Text = Convert.ToString(Math.Round(nat_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            ///PE WOP (MICRON)--------3RD ROW
            wop_thick = fgen.make_double(txtrt15.Text.Trim());
            wop_spgr = fgen.make_double(txtrt16.Text.Trim());
            Wop_gsm = wop_thick * wop_spgr;
            txtrt17.Text = Convert.ToString(Math.Round(Wop_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            Wop_kgs = fgen.make_double(txtrt18.Text.Trim());
            Wop_sqm = (Wop_gsm * Wop_kgs) / 1000;
            txtrt19.Text = Convert.ToString(Math.Round(Wop_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            ///BOPP (PLN, MET, HS) (micron)-----4th ROW               
            bop_thick = fgen.make_double(txtrt21.Text.Trim());
            bop_spgr = fgen.make_double(txtrt22.Text.Trim());
            bop_gsm = bop_thick * bop_spgr;
            txtrt23.Text = Convert.ToString(Math.Round(bop_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            bop_kgs = fgen.make_double(txtrt24.Text.Trim());
            bop_sqm = (bop_gsm * bop_kgs) / 1000;
            txtrt25.Text = Convert.ToString(Math.Round(bop_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //BOPP WOP (micron) for 5TH ROW          
            bop_thick1 = fgen.make_double(txtrt27.Text.Trim());
            bop_spgr1 = fgen.make_double(txtrt28.Text.Trim());
            bop_gsm1 = bop_thick1 * bop_spgr1;
            txtrt29.Text = Convert.ToString(Math.Round(bop_gsm1, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            bop_kgs1 = fgen.make_double(txtrt30.Text.Trim());
            bop_sqm1 = (bop_gsm1 * bop_kgs1) / 1000;
            txtrt31.Text = Convert.ToString(Math.Round(bop_sqm1, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //====CPP (NAT, MET) (micron) for 6 TH ROW            
            cpp_thick = fgen.make_double(txtrt48.Text.Trim());
            cpp_spgr = fgen.make_double(txtrt49.Text.Trim());
            cpp_gsm = cpp_thick * cpp_spgr;
            txtrt50.Text = Convert.ToString(Math.Round(cpp_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            cpp_kgs = fgen.make_double(txtrt51.Text.Trim());
            cpp_sqm = (cpp_gsm * cpp_kgs) / 1000;
            txtrt52.Text = Convert.ToString(Math.Round(cpp_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //CPP W OPQ (micron) for 7TH ROW           
            cpp_thick1 = fgen.make_double(txtrt53.Text.Trim());
            cpp_spgr1 = fgen.make_double(txtrt54.Text.Trim());
            cpp_gsm1 = cpp_thick1 * cpp_spgr1;
            txtrt55.Text = Convert.ToString(Math.Round(cpp_gsm1, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            cpp_kgs1 = fgen.make_double(txtrt56.Text.Trim());
            cpp_sqm1 = (cpp_gsm1 * cpp_kgs1) / 1000;
            txtrt57.Text = Convert.ToString(Math.Round(cpp_sqm1, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //FOIL (micron) FOR 8TH ROW            
            foil_thick = fgen.make_double(txtrt58.Text.Trim());
            foil_spgr = fgen.make_double(txtrt59.Text.Trim());
            foil_gsm = foil_thick * foil_spgr;
            txtrt60.Text = Convert.ToString(Math.Round(foil_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            foil_kgs = fgen.make_double(txtrt61.Text.Trim());
            foil_sqm = (foil_gsm * foil_kgs) / 1000;
            txtrt62.Text = Convert.ToString(Math.Round(foil_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //Shrink PVC (micron) FOR 9TH ROW              
            shrnk_thick = fgen.make_double(txtrt63.Text.Trim());
            shrnk_spgr = fgen.make_double(txtrt64.Text.Trim());
            shrnk_gsm = shrnk_thick * shrnk_spgr;
            txtrt65.Text = Convert.ToString(Math.Round(shrnk_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            shrnk_kgs = fgen.make_double(txtrt66.Text.Trim());
            shrnk_sqm = (shrnk_gsm * shrnk_kgs) / 1000;
            txtrt67.Text = Convert.ToString(Math.Round(shrnk_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //Shrink PET (micron) for 10th ROW    
            shrnk_thick1 = fgen.make_double(txtrt68.Text.Trim());
            shrnk_spgr1 = fgen.make_double(txtrt69.Text.Trim());
            shrnk_gsm1 = shrnk_thick1 * shrnk_spgr1;
            txtrt70.Text = Convert.ToString(Math.Round(shrnk_gsm1, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            shrnk_kgs1 = fgen.make_double(txtrt71.Text.Trim());
            shrnk_sqm1 = (shrnk_gsm1 * shrnk_kgs1) / 1000;
            txtrt72.Text = Convert.ToString(Math.Round(shrnk_sqm1, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            ///   PE NAT Nylon (micron) for 11 th row  
            pe_nat_thick = fgen.make_double(txtrt73.Text.Trim());
            pet_nat_spgr = fgen.make_double(txtrt74.Text.Trim());
            pe_nat_gsm = pe_nat_thick * pet_nat_spgr;
            txtrt75.Text = Convert.ToString(Math.Round(pe_nat_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            pe_nat_kgs = fgen.make_double(txtrt76.Text.Trim());
            pe_nat_sqm = (pe_nat_gsm * pe_nat_kgs) / 1000;
            txtrt77.Text = Convert.ToString(Math.Round(pe_nat_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //PE WOP Nylon (micron) for 12 th row
            pe_wop_thick = fgen.make_double(txtrt78.Text.Trim());
            pe_wop_spgr = fgen.make_double(txtrt79.Text.Trim());
            pe_wop_gsm = pe_wop_thick * pe_wop_spgr;
            txtrt80.Text = Convert.ToString(Math.Round(pe_wop_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            pe_wop_kgs = fgen.make_double(txtrt81.Text.Trim());
            pe_wop_sqm = (pe_wop_gsm * pe_wop_kgs) / 1000;
            txtrt82.Text = Convert.ToString(Math.Round(pe_wop_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //NYLON (micron) for 13 th row
            nyl_Thick = fgen.make_double(txtrt83.Text.Trim());
            nyl_spgr = fgen.make_double(txtrt84.Text.Trim());
            nyl_gsm = nyl_Thick * nyl_spgr;
            txtrt85.Text = Convert.ToString(Math.Round(nyl_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            nyl_kgs = fgen.make_double(txtrt86.Text.Trim());
            nyl_sqm = (nyl_gsm * nyl_kgs) / 1000;
            txtrt87.Text = Convert.ToString(Math.Round(nyl_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //====PAPER (micron) for 14 th ROW
            pap_thick = fgen.make_double(txtrt88.Text.Trim());
            pap_spgr = fgen.make_double(txtrt89.Text.Trim());
            pap_gsm = pap_thick * pap_spgr;
            txtrt90.Text = Convert.ToString(Math.Round(pap_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            pap_kgs = fgen.make_double(txtrt91.Text.Trim());
            pap_sqm = (pap_gsm * pap_kgs) / 1000;
            txtrt92.Text = Convert.ToString(Math.Round(pap_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //=======PEARL BOPP (micron) FOR 15 TH ROW               
            pearl_thick = fgen.make_double(txtrt93.Text.Trim());
            pearl_spgr = fgen.make_double(txtrt94.Text.Trim());
            pearl_gsm = pearl_thick * pearl_spgr;
            txtrt95.Text = Convert.ToString(Math.Round(pearl_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            pearl_kgs = fgen.make_double(txtrt96.Text.Trim());
            pearl_sqm = (pearl_gsm * pearl_kgs) / 1000;
            txtrt97.Text = Convert.ToString(Math.Round(pearl_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //=======ADH SOLVENTLESS (gsm) FOR 16 TH ROW
            adh_thick = fgen.make_double(txtrt98.Text.Trim());
            adh_spgr = fgen.make_double(txtrt99.Text.Trim());
            adh_gsm = adh_thick * adh_spgr;
            txtrt100.Text = Convert.ToString(Math.Round(adh_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            adh_kgs = fgen.make_double(txtrt101.Text.Trim());
            adh_sqm = (adh_gsm * adh_kgs) / 1000;
            txtrt102.Text = Convert.ToString(Math.Round(adh_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //========ADH SOLVENT BASE (gsm) for 17th ROW
            adh1_thick = fgen.make_double(txtrt103.Text.Trim());
            adh1_spgr = fgen.make_double(txtrt104.Text.Trim());
            adh1_gsm = adh1_thick * adh1_spgr;
            txtrt105.Text = Convert.ToString(Math.Round(adh1_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            adh1_kgs = fgen.make_double(txtrt106.Text.Trim());
            adh1_sqm = (adh1_gsm * adh1_kgs) / 1000;
            txtrt107.Text = Convert.ToString(Math.Round(adh1_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //====================Ink (gsm) for 18th ROW
            ink_thick = fgen.make_double(txtrt108.Text.Trim());
            ink_spgr = fgen.make_double(txtrt109.Text.Trim());
            ink_gsm = ink_thick * ink_spgr;
            txtrt110.Text = Convert.ToString(Math.Round(ink_gsm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            ink_kgs = fgen.make_double(txtrt111.Text.Trim());
            ink_sqm = (ink_gsm * ink_kgs) / 1000;
            txtrt112.Text = Convert.ToString(Math.Round(ink_sqm, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //=======
            //main tot
            tot = Math.Round((pet_gsm + nat_gsm + Wop_gsm + bop_gsm + bop_gsm1 + cpp_gsm + cpp_gsm1 + foil_gsm + shrnk_gsm + shrnk_gsm1 + pe_nat_gsm + pe_wop_gsm + nyl_gsm + pap_gsm + pearl_gsm + adh_gsm + adh1_gsm + ink_gsm), 6);
            tot1 = Math.Round((pet_sqm + nat_sqm + Wop_sqm + bop_sqm + bop_sqm1 + cpp_sqm + cpp_sqm1 + foil_sqm + shrnk_sqm + shrnk_sqm1 + pe_nat_sqm + pe_wop_sqm + nyl_sqm + pap_sqm + pearl_sqm + adh_sqm + adh1_sqm + ink_sqm), 6);
            //main tot
            //          txtrt113.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt5.Text.Trim()) + Convert.ToDouble(txtrt11.Text.Trim()) + Convert.ToDouble(txtrt17.Text.Trim()) + Convert.ToDouble(txtrt23.Text.Trim()) + Convert.ToDouble(txtrt29.Text.Trim()) + Convert.ToDouble(txtrt50.Text.Trim()) + Convert.ToDouble(txtrt55.Text.Trim()) + Convert.ToDouble(txtrt60.Text.Trim()) + Convert.ToDouble(txtrt65.Text.Trim()) + Convert.ToDouble(txtrt70.Text.Trim()) + Convert.ToDouble(txtrt75.Text.Trim()) + Convert.ToDouble(txtrt80.Text.Trim()) + Convert.ToDouble(txtrt85.Text.Trim()) + Convert.ToDouble(txtrt90.Text.Trim()) + Convert.ToDouble(txtrt95.Text.Trim()) + Convert.ToDouble(txtrt100.Text.Trim()) + Convert.ToDouble(txtrt105.Text.Trim()) + Convert.ToDouble(txtrt110.Text.Trim()), 2));
            //main tot
            //            txtrt114.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt7.Text.Trim()) + Convert.ToDouble(txtrt13.Text.Trim()) + Convert.ToDouble(txtrt19.Text.Trim()) + Convert.ToDouble(txtrt25.Text.Trim()) + Convert.ToDouble(txtrt31.Text.Trim()) + Convert.ToDouble(txtrt52.Text.Trim()) + Convert.ToDouble(txtrt57.Text.Trim()) + Convert.ToDouble(txtrt62.Text.Trim()) + Convert.ToDouble(txtrt67.Text.Trim()) + Convert.ToDouble(txtrt72.Text.Trim()) + Convert.ToDouble(txtrt77.Text.Trim()) + Convert.ToDouble(txtrt82.Text.Trim()) + Convert.ToDouble(txtrt87.Text.Trim()) + Convert.ToDouble(txtrt92.Text.Trim()) + Convert.ToDouble(txtrt97.Text.Trim()) + Convert.ToDouble(txtrt102.Text.Trim()) + Convert.ToDouble(txtrt107.Text.Trim()) + Convert.ToDouble(txtrt112.Text.Trim()), 2));

            txtrt113.Text = Convert.ToString(Math.Round(tot, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            txtrt114.Text = Convert.ToString(Math.Round(tot1, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            tot2 = Math.Round((tot1 / tot * 1000), 6);
            txtrt33.Text = Convert.ToString(Math.Round(tot2, 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //txtrt33.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtrt114.Text.Trim()) / Convert.ToDouble(txtrt113.Text.Trim())) * 1000, 2));
            //=======================
            //txtrt113.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt5.Text.Trim()) + Convert.ToDouble(txtrt11.Text.Trim()) + Convert.ToDouble(txtrt17.Text.Trim()) + Convert.ToDouble(txtrt23.Text.Trim()) + Convert.ToDouble(txtrt29.Text.Trim()) + Convert.ToDouble(txtrt50.Text.Trim()) + Convert.ToDouble(txtrt55.Text.Trim()) + Convert.ToDouble(txtrt60.Text.Trim()) + Convert.ToDouble(txtrt65.Text.Trim()) + Convert.ToDouble(txtrt70.Text.Trim()) + Convert.ToDouble(txtrt75.Text.Trim()) + Convert.ToDouble(txtrt80.Text.Trim()) + Convert.ToDouble(txtrt85.Text.Trim()) + Convert.ToDouble(txtrt90.Text.Trim()) + Convert.ToDouble(txtrt95.Text.Trim()) + Convert.ToDouble(txtrt100.Text.Trim()) + Convert.ToDouble(txtrt105.Text.Trim()) + Convert.ToDouble(txtrt110.Text.Trim()), 6)).Replace("Infinity", "0").Replace("NaN", "0");

            //txtrt114.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt7.Text.Trim()) + Convert.ToDouble(txtrt13.Text.Trim()) + Convert.ToDouble(txtrt19.Text.Trim()) + Convert.ToDouble(txtrt25.Text.Trim()) + Convert.ToDouble(txtrt31.Text.Trim()) + Convert.ToDouble(txtrt52.Text.Trim()) + Convert.ToDouble(txtrt57.Text.Trim()) + Convert.ToDouble(txtrt62.Text.Trim()) + Convert.ToDouble(txtrt67.Text.Trim()) + Convert.ToDouble(txtrt72.Text.Trim()) + Convert.ToDouble(txtrt77.Text.Trim()) + Convert.ToDouble(txtrt82.Text.Trim()) + Convert.ToDouble(txtrt87.Text.Trim()) + Convert.ToDouble(txtrt92.Text.Trim()) + Convert.ToDouble(txtrt97.Text.Trim()) + Convert.ToDouble(txtrt102.Text.Trim()) + Convert.ToDouble(txtrt107.Text.Trim()) + Convert.ToDouble(txtrt112.Text.Trim()), 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //txtrt33.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtrt114.Text.Trim()) / Convert.ToDouble(txtrt113.Text.Trim())) * 1000, 6)).Replace("Infinity", "0").Replace("NaN", "0");          

            txtrt118.Text = Convert.ToString(Math.Round(tot2 * (1 + ((Convert.ToDouble(txtrt115.Text.Trim())) / 100)) + Convert.ToDouble(txtrt116.Text.Trim()), 6)).Replace("Infinity", "0").Replace("NaN", "0");
            tot3 = Convert.ToDouble(txtrt118.Text);
            //txtrt118.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt33.Text) * (1 + ((Convert.ToDouble(txtrt115.Text.Trim())) / 100)) + Convert.ToDouble(txtrt116.Text.Trim()), 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //txtrt118.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt33.Text) * (1 + ((Convert.ToDouble(txtrt115.Text.Trim())) / 100)) + Convert.ToDouble(txtrt116.Text.Trim()), 2));
            //=======================
            tot4 = Math.Round(((tot3 * tot) / 1000), 6);
            txtrt119.Text = Convert.ToString(tot4).Replace("Infinity", "0").Replace("NaN", "0");
            //txtrt119.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtrt118.Text.Trim()) * Convert.ToDouble(txtrt113.Text.Trim())) / 1000, 6)).Replace("Infinity", "0").Replace("NaN", "0"); ;
            //=======================
            //txtrt35.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt118.Text.Trim()) * 1.30764, 2));
            //==================================================
            tot5 = Convert.ToDouble(txtrt117.Text);
            //document.getElementById('ContentPlaceHolder1_txtrt35').value = (tot3 + (tot3 * (tot5 / 100)));
            txtrt35.Text = Convert.ToString(Math.Round(tot3 + (tot3 * (tot5 / 100)), 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //txtrt35.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt118.Text.Trim()) + (Convert.ToDouble(txtrt118.Text.Trim()) * (Convert.ToDouble(txtrt117.Text.Trim()) / 100)), 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //==================================================     
            tot6 = Math.Round((tot4) / (1 - (tot5 / 100)), 6);
            txtrt36.Text = Convert.ToString(tot6).Replace("Infinity", "0").Replace("NaN", "0"); //reel length
            //txtrt36.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt119.Text.Trim()) / (1 - ((Convert.ToDouble(txtrt117.Text.Trim())) / 100)), 6)).Replace("Infinity", "0").Replace("NaN", "0");
            //==================================================            
            tot7 = Math.Round((tot6 - tot4), 6);
            txtrt38.Text = Convert.ToString(tot7).Replace("Infinity", "0").Replace("NaN", "0");
            //  txtrt38.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtrt36.Text.Trim()) - Convert.ToDouble(txtrt119.Text.Trim())), 6)).Replace("Infinity", "0").Replace("NaN", "0"); ;  
            //==================================================        
            //  tot8 = Math.Round((tot3 + (tot3 * (tot5 / 100))), 6);
            //   txtrt37.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtrt35.Text.Trim() - Convert.ToDouble(txtrt118.Text.Trim())),6).Replace("Infinity", "0").Replace("NaN", "0");
            txtrt37.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt35.Text.Trim()) - Convert.ToDouble(txtrt118.Text.Trim()), 6)); //FORMULA ON WFINSERP CODE
            //  txtrt37.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt35.Text.Trim()) - Convert.ToDouble(txtrt118.Text.Trim()), 2)); //FORMULA ON PMCL CODE
            // txtrt37.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt35.Text.Trim()) - Convert.ToDouble(txtrt118.Text.Trim()), 6)).Replace("Infinity", "0").Replace("NaN", "0"); 
            //==================================================     
            //txtrt123.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtrt120.Text.Trim()) * Convert.ToDouble(txtrt121.Text.Trim())) / 1000000, 7)).Replace("Infinity", "0").Replace("NaN", "0"); ;                  
            tot9 = Convert.ToDouble(txtrt120.Text);
            tot10 = Convert.ToDouble(txtrt121.Text);
            txtrt123.Text = Convert.ToString((tot9 * tot10) / 1000000).Replace("Infinity", "0").Replace("NaN", "0");
            //==================================
            tot11 = Math.Round(((tot9 * tot10) / 1000000) * Convert.ToDouble(txtrt113.Text.Trim()), 6);
            txtrt124.Text = Convert.ToString(tot11).Replace("Infinity", "0").Replace("NaN", "0");
            //txtrt124.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt123.Text.Trim()) * Convert.ToDouble(txtrt113.Text.Trim()), 6)).Replace("Infinity", "0").Replace("NaN", "0"); ;
            //==================================
            tot12 = Math.Round((tot11 * Convert.ToDouble(txtrt35.Text.Trim())), 6);
            txtrt41.Text = Convert.ToString(tot12).Replace("Infinity", "0").Replace("NaN", "0");
            //txtrt41.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt124.Text.Trim()) * Convert.ToDouble(txtrt35.Text.Trim()), 6)).Replace("Infinity", "0").Replace("NaN", "0"); ;
            //==================================  
            tot13 = Math.Round((Convert.ToDouble(txtrt41.Text.Trim()) + Convert.ToDouble(txtrt42.Text.Trim())) * (Convert.ToDouble(txtrt134.Text.Trim()) / 100), 6);
            txtrt43.Text = Convert.ToString(tot13).Replace("Infinity", "0").Replace("NaN", "0");
            //txtrt43.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtrt41.Text.Trim()) + Convert.ToDouble(txtrt42.Text.Trim())) * (Convert.ToDouble(txtrt134.Text.Trim()) / 100), 6)).Replace("Infinity", "0").Replace("NaN", "0"); 
            //==================================  
            //txtrt47.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtrt41.Text.Trim()) + Convert.ToDouble(txtrt42.Text.Trim()) + Convert.ToDouble(txtrt43.Text.Trim()) + Convert.ToDouble(txtrt44.Text.Trim()), 6)).Replace("Infinity", "0").Replace("NaN", "0"); ;
            tot14 = Math.Round((tot12 + Convert.ToDouble(txtrt42.Text.Trim()) + Convert.ToDouble(txtrt43.Text.Trim()) + Convert.ToDouble(txtrt44.Text.Trim())), 6);
            txtrt47.Text = Convert.ToString(tot14).Replace("Infinity", "0").Replace("NaN", "0");
            //==================================  
            txtrt126.Text = Convert.ToString(Math.Round((1 / tot11), 6)).Replace("Infinity", "0").Replace("NaN", "0");
            // txtrt126.Text = Convert.ToString(Math.Round(1 / Convert.ToDouble(txtrt124.Text.Trim()), 6)).Replace("Infinity", "0").Replace("NaN", "0"); 
            //==================================  
            txtrt125.Text = Convert.ToString(Math.Round((1000 / tot11), 6)).Replace("Infinity", "0").Replace("NaN", "0");
            // txtrt125.Text = Convert.ToString(Math.Round(1000 / Convert.ToDouble(txtrt124.Text.Trim()), 6)).Replace("Infinity", "0").Replace("NaN", "0"); 
            //==================================  
            double rmc_exc = 0, exc = 0, netexc = 0, allexc = 0, saletx = 0;
            rmc_exc = tot3 + Convert.ToDouble(txtrt127.Text.Trim());
            exc = rmc_exc * (Convert.ToDouble(txtrt128.Text.Trim()) / 100);
            netexc = exc * (Convert.ToDouble(txtrt129.Text.Trim()) / 100);
            allexc = exc * (Convert.ToDouble(txtrt130.Text.Trim()) / 100);
            saletx = (allexc + rmc_exc + exc + netexc + Convert.ToDouble(txtrt131.Text.Trim())) * (Convert.ToDouble(txtrt132.Text.Trim()) / 100);
            txtrt133.Text = Convert.ToString(Math.Round(saletx + rmc_exc + exc + netexc + allexc, 6));
            fgen.fill_zero(this.Controls);
            #endregion
        }
        else
        {
            if (fgen.make_double(txtff7.Text) == 0 || fgen.make_double(txtprt.Text) == 0 || fgen.make_double(txtannualqty.Text) == 0)
            { }
            else
            {
                #region
                fgen.fill_zero(this.Controls);
                ff12 = fgen.make_double(txtff9.Text.Trim());
                tot15 = fgen.make_double(txtff10.Text.Trim());
                tot16 = fgen.make_double(txtff8.Text.Trim());
                tot17 = Math.Round(((ff12 * tot15 * tot16) / 10000000) * (fgen.make_double(txtprt.Text.Trim()) * fgen.make_double(txtff7.Text.Trim())), 6) / 1;
                txtff12.Text = Convert.ToString(tot17).Replace("Infinity", "0").Replace("NaN", "0");
                //ff12 = Math.Round(((fgen.make_double(txtff9.Text.Trim()) * fgen.make_double(txtff10.Text.Trim()) * fgen.make_double(txtff8.Text.Trim())) / 10000000) * (fgen.make_double(txtprt.Text.Trim()) * fgen.make_double(txtff7.Text.Trim())), 6) / 1;
                // txtff12.Text = Convert.ToString(ff12).Replace("Infinity", "0").Replace("NaN", "0");
                //=================================== 
                //ff14 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff13").value * 1) * tot17);
                //  document.getElementById('ContentPlaceHolder1_txtff14').value = ff14;
                ff14 = Math.Round((fgen.make_double(txtff13.Text.Trim()) * tot17), 6);
                txtff14.Text = Convert.ToString(ff14).Replace("Infinity", "0").Replace("NaN", "0");
                //===================================
                // ff17 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff6").value * 1) * fill_zero(document.getElementById("ContentPlaceHolder1_txtff16").value * 1));
                //  document.getElementById('ContentPlaceHolder1_txtff17').value = ff17;
                ff17 = Math.Round(fgen.make_double(txtff6.Text.Trim()) * fgen.make_double(txtff16.Text.Trim()), 6);
                txtff17.Text = Convert.ToString(ff17).Replace("Infinity", "0").Replace("NaN", "0");
                //=================================== 
                //ff19 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff6").value * 1) * fill_zero(document.getElementById("ContentPlaceHolder1_txtff18").value * 1)) * (fill_zero(document.getElementById("ContentPlaceHolder1_txtprt").value * 1) / fill_zero(document.getElementById("ContentPlaceHolder1_txtff7").value * 1) / 1000);
                // document.getElementById('ContentPlaceHolder1_txtff19').value = ff19;
                ff19 = Math.Round((fgen.make_double(txtff6.Text.Trim()) * fgen.make_double(txtff18.Text.Trim())) * (fgen.make_double(txtprt.Text.Trim()) / fgen.make_double(txtff7.Text.Trim()) / 1000), 6);
                txtff19.Text = Convert.ToString(ff19).Replace("Infinity", "0").Replace("NaN", "0");
                //===================================
                //  ff21 = (fill_zero(document.getElementById("ContentPlaceHolder1_txtff20").value * 1) / fill_zero(document.getElementById("ContentPlaceHolder1_txtannualqty").value * 1)) / 1000;

                ff21 = Math.Round((fgen.make_double(txtff20.Text.Trim()) / fgen.make_double(txtannualqty.Text.Trim())) / 1000, 6);
                txtff21.Text = Convert.ToString(ff21).Replace("Infinity", "0").Replace("NaN", "0");
                //===================================
                ff23 = Math.Round(((fgen.make_double(txtff22.Text) / 100) * (fgen.make_double(txtff9.Text) / 2.54 * fgen.make_double(txtff10.Text) / 2.54)) / fgen.make_double(txtff7.Text) * 1000, 6);
                txtff23.Text = Convert.ToString(ff23).Replace("Infinity", "0").Replace("NaN", "0");
                //===================================
                ff25 = Math.Round((fgen.make_double(txtff24.Text.Trim()) / fgen.make_double(txtannualqty.Text.Trim())) / 1000, 6);
                txtff25.Text = Convert.ToString(ff25).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff27 = Math.Round(Convert.ToDouble((fgen.make_double(txtff26.Text.Trim()) / 100) * fgen.make_double(txtff9.Text.Trim()) / 2.54 * fgen.make_double(txtff10.Text.Trim()) / 2.54) / fgen.make_double(txtff7.Text.Trim()) * 1000, 6);
                txtff27.Text = Convert.ToString(ff27).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff29 = Math.Round((fgen.make_double(txtff28.Text.Trim()) / fgen.make_double(txtannualqty.Text.Trim())) / 1000, 6);
                txtff29.Text = Convert.ToString(ff29).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff31 = Math.Round(Convert.ToDouble((fgen.make_double(txtff30.Text.Trim()) / 100) * fgen.make_double(txtff9.Text.Trim()) / 2.54 * fgen.make_double(txtff10.Text.Trim()) / 2.54) / fgen.make_double(txtff7.Text.Trim()) * 1000, 6);
                txtff31.Text = Convert.ToString(ff31).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff34 = Math.Round(fgen.make_double(txtff32.Text.Trim()) * fgen.make_double(txtff33.Text.Trim()) * 1000, 6);
                txtff34.Text = Convert.ToString(ff34).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff36 = Math.Round((fgen.make_double(txtff35.Text.Trim()) / fgen.make_double(txtannualqty.Text.Trim())) / 1000, 6);
                txtff36.Text = Convert.ToString(ff36).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff38 = Math.Round((fgen.make_double(txtff37.Text.Trim()) / fgen.make_double(txtannualqty.Text.Trim())) / 1000, 6);
                txtff38.Text = Convert.ToString(ff38).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff40 = Math.Round(fgen.make_double(txtff39.Text.Trim()) / fgen.make_double(txtff7.Text.Trim()), 6);
                txtff40.Text = Convert.ToString(ff40).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff42 = Math.Round((fgen.make_double(txtff41.Text.Trim()) / fgen.make_double(txtannualqty.Text.Trim())) / 1000, 6);
                txtff42.Text = Convert.ToString(ff42).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff44 = Math.Round(fgen.make_double(txtff43.Text.Trim()) / fgen.make_double(txtff7.Text.Trim()), 6);
                txtff44.Text = Convert.ToString(ff44).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff49 = Math.Round(Convert.ToDouble(((Convert.ToDouble(txtff45.Text.Trim()) / 10000) * (Convert.ToDouble(txtff46.Text.Trim()) * 1.4)) * Convert.ToDouble(txtff47.Text.Trim())) + Convert.ToDouble(txtff48.Text.Trim()), 6);
                txtff49.Text = Convert.ToString(ff49).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff52 = Math.Round(Convert.ToDouble(((Convert.ToDouble(txtff9.Text.Trim()) * Convert.ToDouble(txtff10.Text.Trim()) / 10000) * (Convert.ToDouble(txtff51.Text.Trim()) * 1.3 + Convert.ToDouble(txtff50.Text.Trim()))) / Convert.ToDouble(txtff7.Text.Trim()) * 1.03), 6);
                txtff52.Text = Convert.ToString(ff52).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff54 = Math.Round(fgen.make_double(txtff52.Text.Trim()) * fgen.make_double(txtff53.Text.Trim()), 6);
                txtff54.Text = Convert.ToString(ff54).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff56 = Math.Round((fgen.make_double(txtff52.Text.Trim()) + fgen.make_double(txtff12.Text.Trim()) + fgen.make_double(txtff55.Text.Trim())) * 2, 6);
                txtff56.Text = Convert.ToString(ff56).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff57 = Math.Round(ff14 + ff17 + ff19 + ff21 + ff23 + ff25 + ff27 + ff29 + ff31 + ff34 + ff36 + ff38 + ff40 + ff42 + ff44 + ff49 + ff54 + ff56, 6);
                txtff57.Text = Convert.ToString(ff57).Replace("Infinity", "0").Replace("NaN", "0");
                //================================== 
                ff59 = Math.Round(ff57 * ((100 + fgen.make_double(txtff58.Text.Trim())) / 100), 6);
                txtff59.Text = Convert.ToString(ff59).Replace("Infinity", "0").Replace("NaN", "0");
                ////=====================
                ff60 = Math.Round(ff59 / 1000, 8);
                txtff60.Text = Convert.ToString(ff60).Replace("Infinity", "0").Replace("NaN", "0");
                fgen.fill_zero(this.Controls);
                #endregion
            }
        }
    }
}
// ALTER TABLE FINMLAB.SOMAS_ANX ADD ANAME VARCHAR2(150);
// ALTER TABLE FINMLAB.SOMAS_ANX ADD INAME VARCHAR2(150);
// ALTER TABLE FINMLAB.SCRATCH ADD ANAME VARCHAR2(150);
// ALTER TABLE FINMLAB.SCRATCH ADD INAME VARCHAR2(150);

