using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;


public partial class om_lbl_cost_MLAB : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, SQuery2, col1, col2, col3, vardate, fromdt, todt, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0; string mq0, mq1, mq2;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable dtCol = new DataTable();
    // DataRow dr = new DataRow();
    string Checked_ok;
    string ord_qty_valid;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, custom_filing_no;
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
            }
            if (txtminrng.Text == "")
            {
                txtminrng.Text = "1.7";
                txtmaxrng.Text = "5";
            }
            if (txtinkgsm.Text == "") txtinkgsm.Text = "4";
            if (txtvarnish_gsm.Text == "") txtvarnish_gsm.Text = "4";
            setColHeadings();
            set_Val();
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
                ((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t5")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t6")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t7")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t8")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t9")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t12")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t13")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t14")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t15")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t17")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t18")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t19")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t20")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t21")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t22")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t23")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t24")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t25")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t26")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t27")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t28")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t29")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t30")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t31")).Attributes.Add("autocomplete", "off");
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
                    sg1.HeaderRow.Cells[sR].Width = Convert.ToInt32(mcol_width);
                    //sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                    //sg1.Rows[0].Cells[sR].Style.Add("width", mcol_width + "px");
                }
            }
        }
        // to hide and show to tab panel
        tab5.Visible = false;
        tab4.Visible = false;
        tab3.Visible = false;
        tab2.Visible = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false;
        btnrefresh.Disabled = true; btncylinder.Enabled = false; btnwidth.Enabled = false; btnC.Enabled = false;
        btnFoil.Enabled = false; btnvarnish.Enabled = false; btnicode.Enabled = false; btnparty.Enabled = false;
        create_tab();
        create_tab2();
        create_tab3();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();
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
        btncylinder.Enabled = true; btnwidth.Enabled = true; btnicode.Enabled = true;
        btnprint.Disabled = true; btnlist.Disabled = true; btnrefresh.Disabled = false; btnC.Enabled = true;
        btnFoil.Enabled = true; btnvarnish.Enabled = true; btnparty.Enabled = true;
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
        frm_tabname = "WB_CYLINDER";
        lblheader.Text = "Label Costing";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "TM");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        typePopup = "N";
        txtoh_trm.Style.Add("readonly", "readonly");
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

            case "VARNISH":
                SQuery = "select distinct  trim(icode) as fstr,trim(icode) as icode,trim(iname) as Material,irate from item where length(trim(icode))>=8 and substr(trim(icode),1,1)='9' and irate!=0 order by icode";//
                SQuery = "select 'UV' AS FSTR,'UV' AS CHOICE,500 AS VALUE FROM DUAL UNION ALL select 'VARNISH' AS FSTR,'MATT' AS CHOICE,1000 AS VALUE FROM DUAL UNION ALL select 'GLOSS' AS FSTR,'GLOSS_MATT' AS CHOICE,1500 AS VALUE FROM DUAL UNION ALL select 'TEXT' AS FSTR,'TEXTURE' AS CHOICE,2000 AS VALUE FROM DUAL";//FOR TESTING ONLY
                SQuery = "SELECT TYPE1 AS FSTR ,NAME,ACREF3 FROM TYPEGRP WHERE ID='V1' ORDER BY FSTR";
                break;

            case "CYLINDER":
                cyl_cal();
                // SQuery = "select fstr,arnd,ar||'/'||NUM16 AS ARND_HEIGHT,col2 AS MIN_RNGE,col3 AS MAX_RNGE  from (select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3, num18 as arnd,'1' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3, num19 as arnd,'2' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num20 as arnd,'3' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num21 as arnd ,'4' as ar,NUM16  from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num22 as arnd ,'5' as ar,NUM16  from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "' union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num23 as arnd,'6' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num24 as arnd,'7' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num25 as arnd,'8' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num26 as arnd,'9' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num27 as arnd,'10' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num28 as arnd,'11' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num29 as arnd,'12' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "'  union all  select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num30 as arnd,'13' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num31 as arnd,'14' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num32 as arnd,'15' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='" + txtwidthmm.Text + "' ) where arnd>col2 and arnd<col3 ";
                // SQuery = "select fstr,arnd,ar||'/'||NUM16 AS ARND_HEIGHT,col2 AS MIN_RNGE,col3 AS MAX_RNGE  from (select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3, num18 as arnd,'1' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate  " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND NUM18>is_number(col2) and NUM18<is_number(col3)  UNION ALL select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3, num19 as arnd,'2' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND num19>is_number(col2) and num19<is_number(col3)  UNION ALL select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num20 as arnd,'3' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate  " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND num20>is_number(col2) and num20<is_number(col3)  UNION ALL select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num21 as arnd ,'4' as ar,NUM16  from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND num21>is_number(col2) and num21<is_number(col3)  UNION ALL select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num22 as arnd ,'5' as ar,NUM16  from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND COL1='" + txtwidthmm.Text + "'AND num22>is_number(col2) and num22<is_number(col3)  UNION ALL select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num23 as arnd,'6' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND num23>is_number(col2) and num23<is_number(col3)  UNION ALL select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num24 as arnd,'7' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate  " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND num24>is_number(col2) and num24<is_number(col3)  UNION ALL select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num25 as arnd,'8' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND num25>is_number(col2) and num25<is_number(col3)  UNION ALL  select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num26 as arnd,'9' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate  " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND num26>is_number(col2) and num26<is_number(col3)  UNION ALL select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num27 as arnd,'10' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND num27>is_number(col2) and num27<is_number(col3)  UNION ALL select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num28 as arnd,'11' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND num28>is_number(col2) and num28<is_number(col3)  UNION ALL  select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num29 as arnd,'12' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND num29>is_number(col2) and num29<is_number(col3)  UNION ALL   select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num30 as arnd,'13' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND num30>is_number(col2) and num30<is_number(col3)  UNION ALL  select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num31 as arnd,'14' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND num31>col2 and num31<col3  UNION ALL  select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num32 as arnd,'15' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND COL1='" + txtwidthmm.Text + "' AND num32>is_number(col2) and num32<is_number(col3)) ";
                break;

            case "MATL":
                SQuery = "select COL2 AS fstr,col1 as name,num1 as price,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col2 as code,to_char(vchdate,'yyyyMMdd') as vdd  from wb_master where branchcd='" + frm_mbr + "' and id='MM01' order by vdd desc";
                if (frm_cocd == "SPPI")
                    SQuery = "select type1 as fstr,name,acref3 as price,type1 as code from TYPEGRP where id='MM' order by type1 ";
                break;

            case "FOIL":
                SQuery = "select 'GOLD' AS FSTR,'GOLD_SILVER' AS CHOICE,'-' AS SELECTION FROM DUAL UNION ALL select 'DULL' AS FSTR,'DULL_GOLD' AS CHOICE,'-' AS SELECTION FROM DUAL UNION ALL select 'N' AS FSTR,'NONE' AS CHOICE,'-' AS SELECTION FROM DUAL";
                SQuery = "SELECT TYPE1 AS FSTR,NAME,ACREF3  FROM TYPEGRP WHERE ID='QM' ORDER BY FSTR";
                if (frm_cocd == "SPPI")
                    SQuery = "SELECT TYPE1 AS FSTR,NAME,ACREF3  FROM TYPEGRP WHERE ID='^M' ORDER BY FSTR";
                break;

            case "WIDTH":
                SQuery = "select  TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR,NVL(NUM2,0) AS WIDTH,NVL(NUM3,0) AS HEIGHT from wb_cylinder where branchcd='" + frm_mbr + "' and type='TM' and vchdate " + DateRange + "";
                break;

            case "CUST":
                SQuery = "SELECT TRIM(ACODE) AS FSTR,TRIM(ACODE) AS CUSTOMER_CODE,TRIM(ANAME) AS CUSTOMER FROM FAMST WHERE SUBSTR(TRIM(aCODE),1,2)='16' ORDER BY CUSTOMER";
                break;

            case "ITEM":
                SQuery = "SELECT TRIM(ICODE) AS FSTR,TRIM(ICODE) AS JOB_CODE,TRIM(INAME) AS JOB_NAME FROM ITEM WHERE SUBSTR(TRIM(ICODE),1,1) IN ('7','9') ORDER BY JOB_NAME";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[13].Text.Trim().Length > 2)
                    {
                        if (col1.Length > 0) col1 = col1 + "," + "'" + gr.Cells[13].Text.Trim() + gr.Cells[14].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[13].Text.Trim() + gr.Cells[14].Text.Trim() + "'";
                    }
                }
                if (col1.Length > 0)
                {
                    col1 = " and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') not in (" + col1 + ")";
                }
                else
                {
                    col1 = "";
                }
                PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                SQuery = "";
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                //pop3
                // to avoid repeat of item
                col1 = "";
                if (btnval != "SG3_ROW_ADD" && btnval != "SG3_ROW_ADD_E")
                {
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[13].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                    }
                }
                if (col1.Length <= 0) col1 = "'-'";
                SQuery = "";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,NVL(A.COL2,'-') as cust_name,NVL(A.COL3,'-') as item_name,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.type,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
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
            if (SQuery != "")
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            }
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
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
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
        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }
        if (txtaname.Text.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Customer");
            return;
        }
        if (txtiname.Text.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Job_Name");
            return;
        }
        if (txtvarnish_trm.Text.Trim() == "-" || txtvarnish_trm.Text.Trim() == "")
        {
            fgen.msg("-", "AMSG", "Please Enter Value of Height(in MM) value");
            return;
        }
        if (txtwidthmm.Text.Trim() == "-" || txtwidthmm.Text.Trim() == "")
        {
            fgen.msg("-", "AMSG", "Please Enter Value of Width(in MM) value");
            return;
        }
        if (txtfoil_trm.Text.Trim() == "-" || txtfoil_trm.Text.Trim() == "")
        {
            fgen.msg("-", "AMSG", "Please Select Cylinder Value");
            return;
        }
        if (txtink_trm.Text.Trim() == "-" || txtink_trm.Text.Trim() == "")
        {
            fgen.msg("-", "AMSG", "Please Enter Across Value");
            return;
        }
        ///for label costing
        if (txtmatl.Text.Trim().Length < 1)
        {
            fgen.msg("-", "AMSG", "Please height Material");
            return;
        }
        Cal();
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
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Entry for Print", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        vty = "TM";
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtentby.Text = frm_uname;
        txtendtdt.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl5.Text = "-";
        txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtp1.Text = "1"; txtp2.Text = "2"; txtp3.Text = "3"; txtp4.Text = "4"; txtp5.Text = "5"; txtp6.Text = "6"; txtp7.Text = "7"; txtp8.Text = "8"; txtp9.Text = "9"; txtp10.Text = "10"; txtp11.Text = "11"; txtp12.Text = "12"; txtp13.Text = "13"; txtp14.Text = "14"; txtp15.Text = "15";
        disablectrl();
        fgen.EnableForm(this.Controls);
        //btnlbl4.Focus();
        sg1_dt = new DataTable();
        create_tab();
        sg1_dr = null;
        //setColHeadings();
        //sg1_add_blankrows();
        hffield.Value = "TACODE";
        dt = new DataTable();
        SQuery = "select trim(branchcd)||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr, num1,num2,num3 from wb_master where branchcd='" + frm_mbr + "' and id='AR01' ";
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_srno"] = i + 1;
            sg1_dr["sg1_f8"] = fgen.make_double(dt.Rows[i]["num1"].ToString().Trim());
            sg1_dr["sg1_f9"] = fgen.make_double(dt.Rows[i]["num2"].ToString().Trim());
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
            sg1_dt.Rows.Add(sg1_dr);
        }
        sg1_add_blankrows();
        ViewState["sg1"] = sg1_dt;
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        // fgen.Fn_open_prddmp1("-", frm_qstr);
        // Popup asking for Copy from Older Data
        //fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        //hffield.Value = "NEW_E";
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
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
                case "New":
                    newCase(col1);
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

                        //txtlbl4.Text = dt.Rows[i]["frm_name"].ToString().Trim();
                        //txtlbl4a.Text = dt.Rows[i]["text"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[i]["frm_header"].ToString().Trim();
                        //txtlbl7.Text = dt.Rows[0]["ent_id"].ToString().Trim();
                        //txtlbl7a.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
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
                    fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", col1);//FSTR
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", frm_vty);
                    fgen.fin_engg_reps(frm_qstr);
                    break;

                //case "MATL":
                //    if (col1.Length <= 0) return;
                //    //SQuery = "select distinct trim(icode) as icode,trim(iname) as Material,irate from item where trim(icode)='" + col1 + "' and length(trim(icode))>=8 and substr(trim(icode),1,1)='9' order by icode";
                //    SQuery = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col2 as code,col1 as name,num1 as price,to_char(vchdate,'yyyyMMdd') as vdd  from wb_master where branchcd='" + frm_mbr + "' and id='MM01' and  trim(branchcd)||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' order by vdd desc";
                //    dt = new DataTable();
                //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //    if (dt.Rows.Count > 0)
                //    {                     
                //        txtcode.Text = dt.Rows[0]["code"].ToString().Trim();
                //        txtmatl.Text = dt.Rows[0]["name"].ToString().Trim();
                //        txtpaper_Film.Text = dt.Rows[0]["price"].ToString().Trim();
                //    }
                //    Cal();
                //    break;

                case "VARNISH":
                    if (col1.Length <= 0) return;
                    #region old code
                    //SQuery = "select distinct trim(icode) as icode,trim(iname) as Material,irate from item where trim(icode)='" + col1 + "' and length(trim(icode))>=8 and substr(trim(icode),1,1)='9' order by icode";
                    //SQuery = "select 'UV' AS FSTR,'UV' AS CHOICE,500 AS VALUE FROM DUAL UNION ALL select 'VARNISH' AS FSTR,'MATT' AS CHOICE,1000 AS VALUE FROM DUAL UNION ALL select 'GLOSS' AS FSTR,'GLOSS_MATT' AS CHOICE,1500 AS VALUE FROM DUAL UNION ALL select 'TEXT' AS FSTR,'TEXTURE' AS CHOICE,2000 AS VALUE FROM DUAL";//FOR TESTING ONLY //OLD
                    // dt = new DataTable();
                    //   dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    ///FOR TESTING.............so hardcode as per excel........need to ask
                    //    if (col1 == "UV")
                    //    {
                    //        txtvarnish.Text = "500";
                    //    }
                    //    else if (col1 == "VARNISH")
                    //    {
                    //        txtvarnish.Text = "1000";
                    //    }
                    //    else if (col1 == "GLOSS")
                    //    {
                    //        txtvarnish.Text = "1500";
                    //    }
                    //    else if (col1 == "TEXT")
                    //    {
                    //        txtvarnish.Text = "2000";
                    //    }
                    //}
                    #endregion
                    //picking from varnish master
                    SQuery = "SELECT ACREF3 ,NAME FROM TYPEGRP WHERE ID='V1' AND TYPE1='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvarnish.Text = dt.Rows[0]["ACREF3"].ToString().Trim();
                        txtvarnishname.Text = dt.Rows[0]["NAME"].ToString().Trim();
                    }
                    Cal();
                    break;

                case "FOIL":
                    if (col1.Length <= 0) return;
                    #region //OLD
                    //SQuery = "select 'GOLD' AS FSTR,'GOLD_SILVER' AS CHOICE,'-' AS SELECTION FROM DUAL UNION ALL select 'DULL' AS FSTR,'DULL_GOLD' AS CHOICE,'-' AS SELECTION FROM DUAL UNION ALL select 'N' AS FSTR,'NONE' AS CHOICE,'-' AS SELECTION FROM DUAL";
                    //if (col1 == "GOLD")
                    //{
                    //    txtfoil.Text = "GOLD/SILVER";
                    //    txtfoil1.Text = "18";
                    //}
                    //else if (col1 == "DULL")
                    //{
                    //    txtfoil.Text = "DULL GOLD";
                    //    txtfoil1.Text = "34";
                    //}
                    //else
                    //{
                    //    txtfoil.Text = "NONE";
                    //    txtfoil1.Text = "0";
                    //}
                    #endregion
                    ///=======picking from quality master
                    SQuery = "SELECT TYPE1 AS FSTR,NAME,ACREF3  FROM TYPEGRP WHERE ID='QM' and type1='" + col1 + "' ORDER BY FSTR";
                    if (frm_cocd == "SPPI")
                        SQuery = "SELECT TYPE1 AS FSTR,NAME,ACREF3  FROM TYPEGRP WHERE ID='^M' ORDER BY FSTR";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtfoil.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtfoil1.Text = dt.Rows[0]["acref3"].ToString().Trim();
                    }
                    Cal();
                    break;
                case "CYLINDER":///need change in logic as per
                    if (col1.Length <= 0) return;
                    // SQuery = "select fstr,arnd,ar||'/'||NUM16 AS ARND_HEIGHT,col2 AS MIN_RNGE,col3 AS MAX_RNGE  from (select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3, num18 as arnd,'1' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3, num19 as arnd,'2' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num20 as arnd,'3' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num21 as arnd ,'4' as ar,NUM16  from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num22 as arnd ,'5' as ar,NUM16  from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8' union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num23 as arnd,'6' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num24 as arnd,'7' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num25 as arnd,'8' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num26 as arnd,'9' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num27 as arnd,'10' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num28 as arnd,'11' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num29 as arnd,'12' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8'  union all  select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num30 as arnd,'13' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num31 as arnd,'14' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8'  union all select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,col2,col3,num32 as arnd,'15' as ar,NUM16   from wb_cylinder where branchcd='" + frm_mbr + "' and type='CC' and vchdate " + DateRange + " AND NUM16='8' ) where arnd>col2 and arnd<col3 ";
                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //txtfoil1_trm.Text = col2;//GAP VALUE
                    //txtinkgsm_trm.Text = col3.Split('/')[0].ToString().Trim(); //AROUND VALUE
                    //txtfoil_trm.Text = col3.Split('/')[1].ToString().Trim(); //HEIGHT VAL
                    txtfoil1_trm.Text = col3;
                    txtinkgsm_trm.Text = col1;
                    txtfoil_trm.Text = col2;
                    break;

                case "WIDTH":
                    if (col1.Length <= 0) return;
                    SQuery = "select NVL(NUM2,0) AS WIDTH,NVL(NUM3,0) AS HEIGHT from wb_cylinder where branchcd='" + frm_mbr + "' and type='TM' and vchdate " + DateRange + " and  TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtwidth.Text = dt.Rows[0]["WIDTH"].ToString().Trim();
                        txtvarnish_trm.Text = dt.Rows[0]["HEIGHT"].ToString().Trim();
                    }
                    break;

                case "MATL":
                    if (col1.Length <= 0) return;
                    //SQuery = "select distinct trim(icode) as icode,trim(iname) as Material,irate from item where trim(icode)='" + col1 + "' and length(trim(icode))>=8 and substr(trim(icode),1,1)='9' order by icode";
                    //SQuery = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col2 as code,col1 as name,num1 as price,to_char(vchdate,'yyyyMMdd') as vdd  from wb_master where branchcd='" + frm_mbr + "' and id='MM01' and  trim(branchcd)||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||col2='" + col1 + "' order by vdd desc";
                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //if (dt.Rows.Count > 0)
                    {
                        txtcode.Text = col1;
                        txtmatl.Text = col2;
                        txtpaper_Film.Text = col3;
                    }
                    Cal();
                    break;

                case "CUST":
                    SQuery = "SELECT TRIM(ACODE) AS FSTR,TRIM(ACODE) AS CUSTOMER_CODE,TRIM(ANAME) AS CUSTOMER FROM FAMST WHERE trim(acode)='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtaname.Text = dt.Rows[0]["CUSTOMER"].ToString().Trim();
                        txtacode.Text = dt.Rows[0]["CUSTOMER_CODE"].ToString().Trim();
                    }
                    Cal();
                    break;
                case "ITEM":
                    SQuery = "SELECT TRIM(ICODE) AS FSTR,TRIM(ICODE) AS JOB_CODE,TRIM(INAME) AS JOB_NAME FROM ITEM WHERE trim(icode)='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtiname.Text = dt.Rows[0]["JOB_NAME"].ToString().Trim();
                        txticode.Text = dt.Rows[0]["JOB_CODE"].ToString().Trim();
                    }
                    Cal();
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.* from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        //txtaname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(aname) as aname from famst where trim(acode)='" + dt.Rows[0]["ACODE"].ToString().Trim() + "'", "aname");//OLD LOGIC
                        txtaname.Text = dt.Rows[0]["COL2"].ToString().Trim();//NEW
                        //  txtiname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(iname) as iname from item where trim(icode)='" + dt.Rows[0]["ICODE"].ToString().Trim() + "'", "iname");//COL3
                        txtiname.Text = dt.Rows[0]["COL3"].ToString().Trim();//NEW
                        txtwidthmm.Text = dt.Rows[0]["num1"].ToString().Trim();
                        txtvarnish_trm.Text = dt.Rows[0]["num2"].ToString().Trim();
                        txtfoil_trm.Text = dt.Rows[0]["num3"].ToString().Trim();
                        txtfoil1_trm.Text = dt.Rows[0]["num4"].ToString().Trim();
                        txtinkgsm_trm.Text = dt.Rows[0]["num5"].ToString().Trim();
                        txtink_trm.Text = dt.Rows[0]["num6"].ToString().Trim();
                        txtoh_trm.Text = dt.Rows[0]["num7"].ToString().Trim();
                        txttrmwastg.Text = dt.Rows[0]["num8"].ToString().Trim();
                        txtstep1.Text = dt.Rows[0]["num9"].ToString().Trim();
                        txtstep2.Text = dt.Rows[0]["num10"].ToString().Trim();
                        txtstep3.Text = dt.Rows[0]["num11"].ToString().Trim();
                        txtstep4.Text = dt.Rows[0]["num12"].ToString().Trim();
                        txtstep5.Text = dt.Rows[0]["num13"].ToString().Trim();
                        /////===========================label costing form edit part                    
                        TextBox14.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        TextBox15.Text = Convert.ToDateTime(dt.Rows[0]["ent_Dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtcode.Text = dt.Rows[0]["col6"].ToString().Trim();

                        if (frm_cocd == "SPPI") txtmatl.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select NAME from TYPE where id='MM' and TYPE1='" + txtcode.Text + "' ", "NAME");
                        else txtmatl.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select  DISTINCT COL1,COL2 from wb_master where branchcd='" + frm_mbr + "' and id='MM01' and col2='" + txtcode.Text + "' ", "col1");      //dt.Rows[0]["col1"].ToString().Trim();                      

                        txtfoil.Text = dt.Rows[0]["col5"].ToString().Trim();
                        txtvarnishname.Text = dt.Rows[0]["col7"].ToString().Trim();
                        //txttrimwstg.Text = dt.Rows[0]["num1"].ToString().Trim(); //already on top in trim wastage part
                        txtprocess_wstg.Text = dt.Rows[0]["num14"].ToString().Trim();
                        txtpaper_Film.Text = dt.Rows[0]["num15"].ToString().Trim();
                        txtvarnish_gsm.Text = dt.Rows[0]["num16"].ToString().Trim();
                        txtvarnish.Text = dt.Rows[0]["num17"].ToString().Trim();//VARNISH USED
                        txtoverheads.Text = dt.Rows[0]["num18"].ToString().Trim();
                        txtprofit.Text = dt.Rows[0]["num19"].ToString().Trim();
                        txtfoil1.Text = dt.Rows[0]["num20"].ToString().Trim();
                        txtinkgsm.Text = dt.Rows[0]["num21"].ToString().Trim();
                        txtink.Text = dt.Rows[0]["num22"].ToString().Trim();
                        txtvarnish1.Text = dt.Rows[0]["num23"].ToString().Trim();
                        txttot.Text = dt.Rows[0]["num24"].ToString().Trim();
                        txtoh.Text = dt.Rows[0]["num25"].ToString().Trim();
                        txttotcost.Text = dt.Rows[0]["num26"].ToString().Trim();
                        txtprofit1.Text = dt.Rows[0]["num27"].ToString().Trim();
                        txtsp.Text = dt.Rows[0]["num28"].ToString().Trim();
                        txtwidth.Text = dt.Rows[0]["num29"].ToString().Trim();
                        txtheight.Text = dt.Rows[0]["num30"].ToString().Trim();
                        txtsqinch.Text = dt.Rows[0]["num31"].ToString().Trim();
                        txtsq_inch_lbl.Text = dt.Rows[0]["num32"].ToString().Trim();
                        txtprice.Text = dt.Rows[0]["num33"].ToString().Trim();
                        txtwastge.Text = dt.Rows[0]["num34"].ToString().Trim();
                        txtperpc.Text = dt.Rows[0]["num35"].ToString().Trim();

                        txtoverhentry.Text = dt.Rows[0]["num36"].ToString().Trim();
                        txtprofitentry.Text = dt.Rows[0]["num37"].ToString().Trim();
                        txtLablePrice.Text = dt.Rows[0]["num38"].ToString().Trim();

                        try
                        {
                            ddSel.ClearSelection();
                            ddSel.Items.FindByText(dt.Rows[0]["col15"].ToString().Trim()).Selected = true;
                        }
                        catch { }

                        txtRmk.Text = dt.Rows[0]["naration"].ToString().Trim();

                        ///================================
                        dt.Dispose();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        txtentby.Text = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        txtendtdt.Text = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                        //Cal();
                    }
                    #endregion
                    break;

                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    //txtlbl4.Text = col1;
                    //txtlbl4a.Text = col2;
                    hffield.Value = "TACODE_E";
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    // btnlbl7.Focus();
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
                    //txtlbl7.Text = col1;
                    // txtlbl7a.Text = col2;
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
                            sg1_dr["sg1_f6"] = dt.Rows[i]["sg1_f6"].ToString();
                            sg1_dr["sg1_f7"] = dt.Rows[i]["sg1_f7"].ToString();
                            //sg1_dr["sg1_f8"] = dt.Rows[i]["sg1_f8"].ToString();
                            //sg1_dr["sg1_f9"] = dt.Rows[i]["sg1_f9"].ToString();
                            sg1_dr["sg1_f8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f8")).Text.Trim();
                            sg1_dr["sg1_f9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f9")).Text.Trim();
                            sg1_dr["sg1_f10"] = dt.Rows[i]["sg1_f10"].ToString();
                            sg1_dr["sg1_f11"] = dt.Rows[i]["sg1_f11"].ToString();
                            sg1_dr["sg1_f12"] = dt.Rows[i]["sg1_f12"].ToString();
                            sg1_dr["sg1_f13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f13")).Text.Trim();
                            sg1_dr["sg1_f14"] = dt.Rows[i]["sg1_f14"].ToString();
                            sg1_dr["sg1_f15"] = dt.Rows[i]["sg1_f15"].ToString();
                            sg1_dr["sg1_f16"] = dt.Rows[i]["sg1_f16"].ToString();
                            sg1_dr["sg1_f17"] = dt.Rows[i]["sg1_f17"].ToString();
                            sg1_dr["sg1_f18"] = dt.Rows[i]["sg1_f18"].ToString();
                            sg1_dr["sg1_f19"] = dt.Rows[i]["sg1_f19"].ToString();
                            sg1_dr["sg1_f20"] = dt.Rows[i]["sg1_f20"].ToString();
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
                            sg1_dr["sg1_t28"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t28")).Text.Trim();
                            sg1_dr["sg1_t29"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t29")).Text.Trim();
                            sg1_dr["sg1_t30"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t30")).Text.Trim();
                            sg1_dr["sg1_t31"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t31")).Text.Trim();
                            sg1_dr["sg1_t32"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t32")).Text.Trim();
                            sg1_dr["sg1_t33"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t33")).Text.Trim();
                            sg1_dr["sg1_t34"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t34")).Text.Trim();
                            sg1_dr["sg1_t35"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t35")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        dt2 = new DataTable();
                        custom_filing_no = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9").ToString().Trim().Replace("&amp", "");
                        SQuery = "select trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') as fstr,b.vchnum,to_char(b.vchdate,'dd/mm/yyyy') as vchdate,b.acode,f.aname,b.destcount as country,b.cscode,to_char(b.remvdate,'dd/mm/yyyy') as remvdate,b.bill_tot,b.insp_amt as foreign_amt,b.amt_exc as igst_claimed,b.curren,b.chlnum,to_char(b.chldate,'dd/MM/yyyy') as chldate,c.aname as cons from famst f,salep b left join csmst c on trim(b.cscode)=trim(c.acode) where trim(b.acode)=trim(f.acode) and trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') in (" + custom_filing_no + ")  order by vchnum";
                        SQuery1 = "select trim(a.vchnum)||trim(a.vchdate) as fstr,sum(a.iqtyout) as iqtyout,max(a.hscode) as hscode,a.export_under,max(name) as name,a.acpt_ud as curr_rate from(select iqtyout,null as hscode,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,(case when nvl(trim(store_no),'-')='19' then 'LUT' when nvl(trim(store_no),'-')='18' then 'ADV. LIC AND IGST' else 'DUTY FREE' end) as export_under,null as name,acpt_ud from ivoucherp where trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') in (" + custom_filing_no + ") union all select 0 as iqtyout,i.hscode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,(case when nvl(trim(a.store_no),'-')='19' then 'LUT' when nvl(trim(store_no),'-')='18' then 'ADV. LIC AND IGST' else 'DUTY FREE' end) as export_under,t.name as name,a.acpt_ud from ivoucherp a,item i,typegrp t where trim(a.icode)=trim(i.icode) and trim(i.hscode)=trim(t.acref) and t.id='T1' and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + custom_filing_no + ") and a.morder='1')a group by trim(a.vchnum),trim(a.vchdate),a.export_under,a.acpt_ud";
                        //SQuery2 = "select trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,exprmk as country from hundip where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') in (" + custom_filing_no + ")  order by vchnum";
                        SQuery2 = "select trim(a.chlnum)||to_char(a.chldate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.chlnum,to_char(a.chldate,'dd/MM/yyyy') as chldate from sale a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + col1 + ")  order by vchnum";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery2);
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            // filling value in sg1_h1
                            // saving icode in this field
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
                            if (dt3.Rows.Count > 0)
                            {
                                sg1_dr["sg1_f1"] = fgen.seek_iname_dt(dt3, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "vchnum");
                                sg1_dr["sg1_f2"] = fgen.seek_iname_dt(dt3, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "vchdate");
                            }
                            sg1_dr["sg1_f3"] = dt.Rows[d]["acode"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["aname"].ToString().Trim();
                            sg1_dr["sg1_f6"] = dt.Rows[d]["country"].ToString().Trim();
                            sg1_dr["sg1_f7"] = dt.Rows[d]["remvdate"].ToString().Trim();
                            // sg1_dr["sg1_f8"] = dt.Rows[d]["bill_tot"].ToString().Trim();
                            sg1_dr["sg1_f8"] = "0";
                            sg1_dr["sg1_f9"] = dt.Rows[d]["foreign_amt"].ToString().Trim();
                            sg1_dr["sg1_f10"] = dt.Rows[d]["igst_claimed"].ToString().Trim();
                            if (dt2.Rows.Count > 0)
                            {
                                sg1_dr["sg1_f5"] = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "iqtyout");
                                sg1_dr["sg1_f11"] = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "hscode");
                                sg1_dr["sg1_f12"] = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "export_under");
                                sg1_dr["sg1_f14"] = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "name");
                                sg1_dr["sg1_t30"] = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "curr_rate");
                            }
                            sg1_dr["sg1_f13"] = "";
                            sg1_dr["sg1_f16"] = dt.Rows[d]["vchnum"].ToString().Trim();
                            sg1_dr["sg1_f17"] = dt.Rows[d]["vchdate"].ToString().Trim();
                            sg1_dr["sg1_f18"] = dt.Rows[d]["curren"].ToString().Trim();
                            sg1_dr["sg1_f19"] = dt.Rows[d]["cscode"].ToString().Trim();
                            sg1_dr["sg1_f20"] = dt.Rows[d]["cons"].ToString().Trim();
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
                            sg1_dr["sg1_t17"] = "";
                            sg1_dr["sg1_t18"] = "";
                            sg1_dr["sg1_t19"] = "";
                            sg1_dr["sg1_t20"] = "";
                            sg1_dr["sg1_t21"] = "";
                            sg1_dr["sg1_t22"] = "";
                            sg1_dr["sg1_t23"] = "";
                            sg1_dr["sg1_t24"] = "";
                            sg1_dr["sg1_t25"] = "";
                            sg1_dr["sg1_t26"] = "";
                            sg1_dr["sg1_t27"] = "";
                            sg1_dr["sg1_t28"] = "";
                            sg1_dr["sg1_t29"] = "";
                            sg1_dr["sg1_t31"] = "";
                            sg1_dr["sg1_t32"] = "";
                            sg1_dr["sg1_t33"] = "";
                            sg1_dr["sg1_t34"] = "";
                            sg1_dr["sg1_t35"] = "";
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_f13")).Focus();
                    #endregion
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    dt2 = new DataTable();
                    custom_filing_no = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9").ToString().Trim().Replace("&amp", "");
                    SQuery = "select trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') as fstr,b.vchnum,to_char(b.vchdate,'dd/mm/yyyy') as vchdate,b.acode,f.aname,b.destcount as country,b.cscode,to_char(b.remvdate,'dd/mm/yyyy') as remvdate,b.bill_tot,b.insp_amt as foreign_amt,b.amt_exc as igst_claimed,b.curren,b.chlnum,to_char(b.chldate,'dd/MM/yyyy') as chldate,c.aname as cons from famst f,salep b left join csmst c on trim(b.cscode)=trim(c.acode) where trim(b.acode)=trim(f.acode) and trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') ='" + custom_filing_no + "'  order by vchnum";
                    SQuery1 = "select trim(a.vchnum)||trim(a.vchdate) as fstr,sum(a.iqtyout) as iqtyout,max(a.hscode) as hscode,a.export_under,max(name) as name,a.acpt_ud as curr_rate from(select iqtyout,null as hscode,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,(case when nvl(trim(store_no),'-')='19' then 'LUT' when nvl(trim(store_no),'-')='18' then 'ADV. LIC AND IGST' else 'DUTY FREE' end) as export_under,null as name,acpt_ud from ivoucherp where trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')= '" + custom_filing_no + "' union all select 0 as iqtyout,i.hscode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,(case when nvl(trim(a.store_no),'-')='19' then 'LUT' when nvl(trim(store_no),'-')='18' then 'ADV. LIC AND IGST' else 'DUTY FREE' end) as export_under,t.name as name,a.acpt_ud from ivoucherp a,item i,typegrp t where trim(a.icode)=trim(i.icode) and trim(i.hscode)=trim(t.acref) and t.id='T1' and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')= '" + custom_filing_no + "' and a.morder='1')a group by trim(a.vchnum),trim(a.vchdate),a.export_under,a.acpt_ud";
                    //SQuery2 = "select trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,exprmk as country from hundip where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') in (" + custom_filing_no + ")  order by vchnum";
                    SQuery2 = "select trim(a.chlnum)||to_char(a.chldate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.chlnum,to_char(a.chldate,'dd/MM/yyyy') as chldate from sale a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') ='" + col1 + "'  order by vchnum";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery2);
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in GridView Value
                        if (dt3.Rows.Count > 0)
                        {
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = fgen.seek_iname_dt(dt3, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "vchnum");
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = fgen.seek_iname_dt(dt3, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "vchdate");
                        }
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[d]["vchnum"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[d]["vchdate"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[d]["acode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["aname"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[22].Text = dt.Rows[d]["country"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[23].Text = dt.Rows[d]["remvdate"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[19].Text = dt.Rows[d]["cscode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[20].Text = dt.Rows[d]["cons"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_f8")).Text = "0";
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_f9")).Text = dt.Rows[d]["foreign_amt"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[26].Text = dt.Rows[d]["curren"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[28].Text = dt.Rows[d]["igst_claimed"].ToString().Trim();
                        if (dt2.Rows.Count > 0)
                        {
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[21].Text = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "iqtyout");
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[29].Text = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "hscode");
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[30].Text = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "export_under");
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[32].Text = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "name");
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t30")).Text = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "curr_rate"); ;
                        }
                    }
                    hf2.Value = "";
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
                        for (i = 0; i < dt.Rows.Count - 1; i++)
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
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[17].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[18].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[21].Text.Trim();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[22].Text.Trim();
                            sg1_dr["sg1_f7"] = sg1.Rows[i].Cells[23].Text.Trim();
                            //sg1_dr["sg1_f8"] = sg1.Rows[i].Cells[22].Text.Trim();
                            //sg1_dr["sg1_f9"] = sg1.Rows[i].Cells[23].Text.Trim();
                            sg1_dr["sg1_f8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f8")).Text.Trim();
                            sg1_dr["sg1_f9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f9")).Text.Trim();
                            sg1_dr["sg1_f10"] = sg1.Rows[i].Cells[28].Text.Trim();
                            sg1_dr["sg1_f11"] = sg1.Rows[i].Cells[29].Text.Trim();
                            sg1_dr["sg1_f12"] = sg1.Rows[i].Cells[30].Text.Trim();
                            //sg1_dr["sg1_f13"] = sg1.Rows[i].Cells[28].Text.Trim();
                            sg1_dr["sg1_f13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f13")).Text.Trim();
                            sg1_dr["sg1_f14"] = sg1.Rows[i].Cells[32].Text.Trim();
                            sg1_dr["sg1_f15"] = sg1.Rows[i].Cells[33].Text.Trim();
                            sg1_dr["sg1_f16"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f17"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f18"] = sg1.Rows[i].Cells[26].Text.Trim();
                            sg1_dr["sg1_f19"] = sg1.Rows[i].Cells[19].Text.Trim();
                            sg1_dr["sg1_f20"] = sg1.Rows[i].Cells[20].Text.Trim();

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
                            sg1_dr["sg1_t28"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t28")).Text.Trim();
                            sg1_dr["sg1_t29"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t29")).Text.Trim();
                            sg1_dr["sg1_t30"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t30")).Text.Trim();
                            sg1_dr["sg1_t31"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t31")).Text.Trim();
                            sg1_dr["sg1_t32"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t32")).Text.Trim();
                            sg1_dr["sg1_t33"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t33")).Text.Trim();
                            sg1_dr["sg1_t34"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t34")).Text.Trim();
                            sg1_dr["sg1_t35"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t35")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
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
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");

            //SQuery = "SELECT a.VCHNUM AS ENTRY_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS ENTRY_dT,to_char(a.vchdate,'yyyymmdd') as vdd,trim(a.acode) as cust_code,b.aname as cust_name,trim(a.icode) as item_code,c.iname as item_name,nvl(a.COL1,'-') AS MATERIAL,nvl(a.COL2,'-') AS FOIL,nvl(a.num1,0) as WIDTH_IN_MM,nvl(a.num2,0) as HEIGHT_IN_MM,nvl(a.num3,0) as CYLINDER,nvl(a.num4,0) as GAP,nvl(a.num5,0) as AROUND,nvl(A.num6,0) as ACROS,nvl(a.num7,0) as paper_size,nvl(a.num8,0) as trim_wastage,nvl(a.num9,0) as step1,nvl(num10,0) as step2,nvl(num11,0) as step3,nvl(num12,0) as step4,nvl(num13,0) as step5,nvl(num14,0) as total_Cost,nvl(num15,0) as paper_film,nvl(num16,0) as varnish_gsm,nvl(num17,0) as varnish_used,nvl(num18,0) as overheads,nvl(num19,0) as profit,nvl(num20,0) as foil_val,nvl(num21,0) as ink_gsm,nvl(num22,0) as ink,nvl(a.num23,0) as varnish,nvl(a.num24,0) as varnish_papr_ink_foil,nvl(a.num25,0) as o_h,nvl(a.num26,0) as totalcost,nvl(a.num27,0) as profit1,nvl(a.num28,0) as sp,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_Dt  from " + frm_tabname + " a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + "  order by vdd desc,a.vchnum desc,a.srno";
            SQuery = "SELECT a.VCHNUM AS ENTRY_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS ENTRY_dT,to_char(a.vchdate,'yyyymmdd') as vdd,NVL(A.COL2,'-') as cust_name,NVL(A.COL3,'-') as item_name,nvl(a.COL1,'-') AS MATERIAL,nvl(a.COL2,'-') AS FOIL,nvl(a.num1,0) as WIDTH_IN_MM,nvl(a.num2,0) as HEIGHT_IN_MM,nvl(a.num3,0) as CYLINDER,nvl(a.num4,0) as GAP,nvl(a.num5,0) as AROUND,nvl(A.num6,0) as ACROS,nvl(a.num7,0) as paper_size,nvl(a.num8,0) as trim_wastage,nvl(a.num9,0) as step1,nvl(num10,0) as step2,nvl(num11,0) as step3,nvl(num12,0) as step4,nvl(num13,0) as step5,nvl(num14,0) as total_Cost,nvl(num15,0) as paper_film,nvl(num16,0) as varnish_gsm,nvl(num17,0) as varnish_used,nvl(num18,0) as overheads,nvl(num19,0) as profit,nvl(num20,0) as foil_val,nvl(num21,0) as ink_gsm,nvl(num22,0) as ink,nvl(a.num23,0) as varnish,nvl(a.num24,0) as varnish_papr_ink_foil,nvl(a.num25,0) as o_h,nvl(a.num26,0) as totalcost,nvl(a.num27,0) as profit1,nvl(a.num28,0) as sp,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_Dt  from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + "  order by vdd desc,a.vchnum desc,a.srno";
            //changed query as per client / upper query is all ok
            SQuery = "SELECT a.VCHNUM AS ENTRY_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS ENTRY_dT,to_char(a.vchdate,'yyyymmdd') as vdd,NVL(A.COL2,'-') as cust_name,NVL(A.COL3,'-') as item_name,nvl(a.num33,0) as Price_per_thousand,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_Dt  from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + "  order by vdd desc,a.vchnum desc,a.srno";
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
                    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
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
                            frm_vnum = txtvchnum.Text.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            //save_it = "N";
                            //for (i = 0; i < sg1.Rows.Count - 0; i++)
                            //{
                            save_it = "Y";
                            // }
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

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

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
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }

                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtvchdate.Text.Trim() + "'");
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
        sg1_dt.Columns.Add(new DataColumn("sg1_f6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f10", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f11", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f13", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f14", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f16", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f20", typeof(string)));
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
        sg1_dt.Columns.Add(new DataColumn("sg1_t32", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t33", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t34", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t35", typeof(string)));

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
            sg1_dr["sg1_f6"] = "-";
            sg1_dr["sg1_f7"] = "-";
            sg1_dr["sg1_f8"] = "-";
            sg1_dr["sg1_f9"] = "-";
            sg1_dr["sg1_f10"] = "-";
            sg1_dr["sg1_f11"] = "-";
            sg1_dr["sg1_f12"] = "-";
            sg1_dr["sg1_f13"] = "-";
            sg1_dr["sg1_f14"] = "-";
            sg1_dr["sg1_f15"] = "-";
            sg1_dr["sg1_f16"] = "-";
            sg1_dr["sg1_f17"] = "-";
            sg1_dr["sg1_f18"] = "-";
            sg1_dr["sg1_f19"] = "-";
            sg1_dr["sg1_f20"] = "-";
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
            sg1_dr["sg1_t32"] = "-";
            sg1_dr["sg1_t33"] = "-";
            sg1_dr["sg1_t34"] = "-";
            sg1_dr["sg1_t35"] = "-";
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
            e.Row.Cells[1].Width = 30;
            e.Row.Cells[1].Width = 30;
            e.Row.Cells[3].Width = 30;
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
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Export Invoice From The List");
                }
                break;

            case "SG1_ROW_ADD":
                if (index < sg1.Rows.Count - 1)
                {
                    // ON + BUTTON DATE RANGE HAVE TO BE ASKED THAT'S WHY CASE IS CHANGED
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    // hffield.Value = "SG1_ROW_ADD_E";
                    hffield.Value = "TACODE";
                    hf2.Value = "SG1_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    // make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Export Invoice", frm_qstr);                  
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                }
                else
                {
                    // ON + BUTTON DATE RANGE HAVE TO BE ASKED THAT'S WHY CASE IS CHANGED
                    //hffield.Value = "SG1_ROW_ADD";
                    hffield.Value = "TACODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    //make_qry_4_popup();
                    //fgen.Fn_open_mseek("Select Export Invoice", frm_qstr);
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
    //protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    //{
    //    hffield.Value = "TACODE";
    //    make_qry_4_popup();
    //    fgen.Fn_open_sseek("Select Customer", frm_qstr);
    //}
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
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = frm_mbr;
        oporow["TYPE"] = frm_vty;
        oporow["vchnum"] = frm_vnum;
        oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
        oporow["SRNO"] = 1;
        oporow["ACODE"] = txtacode.Text.Trim().ToUpper();//acode
        oporow["ICODE"] = txticode.Text.Trim();//icode
        oporow["col2"] = txtaname.Text.Trim().ToUpper();//ANAME
        oporow["COL3"] = txtiname.Text.Trim().ToUpper();//INAME
        oporow["col4"] = hf4.Value.Trim();//vARNISH POPUP ICODE SAVED           
        oporow["num1"] = fgen.make_double(txtwidthmm.Text.Trim());
        oporow["num2"] = fgen.make_double(txtvarnish_trm.Text.Trim());//HEIGHT IN MM VALUE
        oporow["num3"] = fgen.make_double(txtfoil_trm.Text.Trim());//cylinder
        oporow["num4"] = fgen.make_double(txtfoil1_trm.Text.Trim());//gap
        oporow["num5"] = fgen.make_double(txtinkgsm_trm.Text.Trim());//around
        oporow["num6"] = fgen.make_double(txtink_trm.Text.Trim());//across
        oporow["num7"] = fgen.make_double(txtoh_trm.Text.Trim());//papaer size
        oporow["num8"] = fgen.make_double(txttrmwastg.Text.Trim());
        oporow["num9"] = fgen.make_double(txtstep1.Text.Trim());
        oporow["num10"] = fgen.make_double(txtstep2.Text.Trim());
        oporow["num11"] = fgen.make_double(txtstep3.Text.Trim());
        oporow["num12"] = fgen.make_double(txtstep4.Text.Trim());
        oporow["num13"] = fgen.make_double(txtstep5.Text.Trim());
        //=======================================================for label costing
        if (txtmatl.Text.Length > 20)
        {
            oporow["col1"] = txtmatl.Text.Trim().ToUpper().Substring(0, 19);
        }
        else
        {
            oporow["col1"] = txtmatl.Text.Trim().ToUpper();
        }
        oporow["col5"] = txtfoil.Text.Trim().ToUpper();//GOLD/SILVER
        oporow["COL6"] = txtcode.Text.Trim();//MATL POPUP ICODE SAVED 
        oporow["col7"] = txtvarnishname.Text.Trim();
        oporow["col15"] = ddSel.SelectedItem.Text;
        //oporow["col4"] = hf4.Value.Trim();//vARNISH POPUP ICODE SAVED ///////////PENDING TO CHE3CK IN COST FORM
        //   oporow["num1"] = fgen.make_double(txttrimwstg.Text.Trim());//already in trim wastage part
        oporow["num14"] = fgen.make_double(txtprocess_wstg.Text.Trim());
        oporow["num15"] = fgen.make_double(txtpaper_Film.Text.Trim());
        oporow["num16"] = fgen.make_double(txtvarnish_gsm.Text.Trim());
        oporow["num17"] = fgen.make_double(txtvarnish.Text.Trim());//VARNISH USED
        oporow["num18"] = fgen.make_double(txtoverheads.Text.Trim());
        oporow["num19"] = fgen.make_double(txtprofit.Text.Trim());
        oporow["num20"] = fgen.make_double(txtfoil1.Text.Trim());
        oporow["num21"] = fgen.make_double(txtinkgsm.Text.Trim());
        oporow["num22"] = fgen.make_double(txtink.Text.Trim());
        oporow["num23"] = fgen.make_double(txtvarnish1.Text.Trim());
        oporow["num24"] = fgen.make_double(txttot.Text.Trim());
        oporow["num25"] = fgen.make_double(txtoh.Text.Trim());
        oporow["num26"] = fgen.make_double(txttotcost.Text.Trim());
        oporow["num27"] = fgen.make_double(txtprofit1.Text.Trim());
        oporow["num28"] = fgen.make_double(txtsp.Text.Trim());
        oporow["num29"] = fgen.make_double(txtwidth.Text.Trim());
        oporow["num30"] = fgen.make_double(txtheight.Text.Trim());
        oporow["num31"] = fgen.make_double(txtsqinch.Text.Trim());
        oporow["num32"] = fgen.make_double(txtsq_inch_lbl.Text.Trim());
        oporow["num33"] = fgen.make_double(txtprice.Text.Trim());
        oporow["num34"] = fgen.make_double(txtwastge.Text.Trim());
        oporow["num35"] = fgen.make_double(txtperpc.Text.Trim());

        oporow["num36"] = fgen.make_double(txtoverhentry.Text.Trim());
        oporow["num37"] = fgen.make_double(txtprofitentry.Text.Trim());

        oporow["num38"] = fgen.make_double(txtLablePrice.Text.Trim());

        oporow["NARATION"] = txtRmk.Text.Trim();

        //=========================================================        
        if (edmode.Value == "Y")
        {
            //   txtacode.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(acode) as acode from famst where upper(trim(aname))='" + txtaname.Text.Trim().ToUpper() + "'", "acode");
            // txticode.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(icode) as icode from item where upper(trim(iname))='" + txtiname.Text.Trim().ToUpper() + "'", "icode");
            oporow["ent_by"] = ViewState["entby"].ToString();
            oporow["ent_dt"] = ViewState["entdt"].ToString();
            oporow["edt_by"] = frm_uname;
            oporow["edt_dt"] = vardate;
        }
        else
        {
            oporow["ent_by"] = frm_uname;
            oporow["ent_dt"] = vardate;
            oporow["edt_by"] = "-";
            oporow["eDt_dt"] = vardate;
        }

        oDS.Tables[0].Rows.Add(oporow);
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {

    }
    //------------------------------------------------------------------------------------
    void save_fun3()
    {

    }
    //------------------------------------------------------------------------------------
    void save_fun4()
    {


    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F50111":
                SQuery = "SELECT '46' AS FSTR,'Sales Schedule' as NAME,'46' AS CODE FROM dual";
                break;
        }
    }
    //------------------------------------------------------------------------------------
    void cyl_cal()
    {
        SQuery = "select distinct trim(branchcd)||trim(id)||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr, name,num1 as height,num2 as width,srno from wb_master where branchcd='" + frm_mbr + "' and id='AR01' order by num1";
        dt = new DataTable(); dt2 = new DataTable();
        dt2.Columns.Add("around", typeof(double));
        dt2.Columns.Add("height", typeof(double));
        dt2.Columns.Add("gap", typeof(double));
        oporow = dt2.NewRow();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//around master dt
        double s1 = 0; double s2 = 0; double s3 = 0; double s4 = 0; double s5 = 0; double s6 = 0; double s7 = 0; double s8 = 0; double s9 = 0;
        double s10 = 0; double s11 = 0; double s12 = 0; double s13 = 0; double s14 = 0; double s15 = 0; double s16 = 0; double s17 = 0;
        double t1 = 0; double t2 = 0; double t3 = 0; double t4 = 0; double t5 = 0; double t6 = 0; double t7 = 0; double t8 = 0; double t9 = 0; double t10 = 0; double t11 = 0; double t12 = 0; double t13 = 0; double t14 = 0; double t15 = 0;
        double gft1 = 0; double gft2 = 0; double gt1 = 0; double gt2 = 0; double gt3 = 0; double gt4 = 0; double gt5 = 0; double gt6 = 0; double gt7 = 0; double gt8 = 0; double gt9 = 0; double gt10 = 0; double gt11 = 0; double gt12 = 0; double gt13 = 0; double gt14 = 0; double gt15 = 0;
        double wieght = 0; double minrng = 0; double maxrng = 0;
        wieght = fgen.make_double(txtwidthmm.Text.Trim());
        minrng = fgen.make_double(txtminrng.Text.Trim());
        maxrng = fgen.make_double(txtmaxrng.Text.Trim());
        t1 = 1; t2 = 2; t3 = 3; t4 = 4; t5 = 5; t6 = 6; t7 = 7; t8 = 8; t9 = 9; t10 = 10; t11 = 11; t12 = 12; t13 = 13; t14 = 14; t15 = 15;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            #region
            gft1 = Math.Round(fgen.make_double(dt.Rows[i]["height"].ToString().Trim()), 4);
            gft2 = Math.Round(fgen.make_double(dt.Rows[i]["width"].ToString().Trim()), 4);
            //============
            gt1 = ((gft2 - (wieght * t1)) / t1).toDouble(5);
            gt2 = ((gft2 - (wieght * t2)) / t2).toDouble(5);
            gt3 = ((gft2 - (wieght * t3)) / t3).toDouble(5);
            gt4 = ((gft2 - (wieght * t4)) / t4).toDouble(5);
            gt5 = ((gft2 - (wieght * t5)) / t5).toDouble(5);
            gt6 = ((gft2 - (wieght * t6)) / t6).toDouble(5);
            gt7 = ((gft2 - (wieght * t7)) / t7).toDouble(5);
            gt8 = ((gft2 - (wieght * t8)) / t8).toDouble(5);
            gt9 = ((gft2 - (wieght * t9)) / t9).toDouble(5);
            gt10 = ((gft2 - (wieght * t10)) / t10).toDouble(5);
            gt11 = ((gft2 - (wieght * t11)) / t11).toDouble(5);
            gt12 = ((gft2 - (wieght * t12)) / t12).toDouble(5);
            gt13 = ((gft2 - (wieght * t13)) / t13).toDouble(5);
            gt14 = ((gft2 - (wieght * t14)) / t14).toDouble(5);
            gt15 = ((gft2 - (wieght * t15)) / t15).toDouble(5);
            //////comparing best around 
            if (gt1 >= minrng && gt1 <= maxrng)
            {
                oporow = dt2.NewRow();
                s1 = Math.Round(gt1, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s1;
                oporow["around"] = 1;
                dt2.Rows.Add(oporow);
            }
            if (gt2 >= minrng && gt2 <= maxrng)
            {
                oporow = dt2.NewRow();
                s2 = Math.Round(gt2, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s2;
                oporow["around"] = 2;
                dt2.Rows.Add(oporow);
            }
            if (gt3 >= minrng && gt3 <= maxrng)
            {
                oporow = dt2.NewRow();
                s3 = Math.Round(gt3, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s3;
                oporow["around"] = 3;
                dt2.Rows.Add(oporow);
            }
            if (gt4 >= minrng && gt4 <= maxrng)
            {
                oporow = dt2.NewRow();
                s4 = Math.Round(gt4, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s4;
                oporow["around"] = 4;
                dt2.Rows.Add(oporow);
            }
            if (gt5 >= minrng && gt5 <= maxrng)
            {
                oporow = dt2.NewRow();
                s5 = Math.Round(gt5, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s5;
                oporow["around"] = 5;
                dt2.Rows.Add(oporow);
            }
            if (gt6 >= minrng && gt6 <= maxrng)
            {
                oporow = dt2.NewRow();
                s6 = Math.Round(gt6, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s6;
                oporow["around"] = 6;
                dt2.Rows.Add(oporow);
            }
            if (gt7 >= minrng && gt7 <= maxrng)
            {
                oporow = dt2.NewRow();
                s7 = Math.Round(gt7, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s7;
                oporow["around"] = 7;
                dt2.Rows.Add(oporow);
            }
            if (gt8 >= minrng && gt8 <= maxrng)
            {
                oporow = dt2.NewRow();
                s8 = Math.Round(gt8, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s8;
                oporow["around"] = 8;
                dt2.Rows.Add(oporow);
            }
            if (gt9 >= minrng && gt9 <= maxrng)
            {
                oporow = dt2.NewRow();
                s9 = Math.Round(gt9, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s9;
                oporow["around"] = 9;
                dt2.Rows.Add(oporow);
            }
            if (gt10 >= minrng && gt10 <= maxrng)
            {
                oporow = dt2.NewRow();
                s10 = Math.Round(gt10, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s10;
                oporow["around"] = 10;
                dt2.Rows.Add(oporow);
            }
            if (gt11 >= minrng && gt11 <= maxrng)
            {
                oporow = dt2.NewRow();
                s11 = Math.Round(gt11, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s11;
                oporow["around"] = 11;
                dt2.Rows.Add(oporow);
            }
            if (gt12 >= minrng && gt12 <= maxrng)
            {
                oporow = dt2.NewRow();
                s12 = Math.Round(gt12, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s12;
                oporow["around"] = 12;
                dt2.Rows.Add(oporow);
            }
            if (gt13 >= minrng && gt13 <= maxrng)
            {
                oporow = dt2.NewRow();
                s13 = Math.Round(gt13, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s13;
                oporow["around"] = 13;
                dt2.Rows.Add(oporow);
            }
            if (gt14 >= minrng && gt14 <= maxrng)
            {
                oporow = dt2.NewRow();
                s14 = Math.Round(gt14, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s14;
                oporow["around"] = 14;
                dt2.Rows.Add(oporow);
            }
            if (gt15 >= minrng && gt15 <= maxrng)
            {
                oporow = dt2.NewRow();
                s15 = Math.Round(gt15, 4);
                oporow["height"] = gft1;
                oporow["gap"] = s15;
                oporow["around"] = 15;
                dt2.Rows.Add(oporow);
            }
            #endregion
        }
        mq1 = ""; SQuery = "";
        for (int i = 0; i < dt2.Rows.Count; i++)
        {
            if (i == dt2.Rows.Count - 1)
            {
                //mq1 += "select '-' as fstr, '" + dt2.Rows[i]["around"].ToString().Trim() + "' as around,'" + minrng + "' as minrange,'" + maxrng + "' as maxrange from dual";
                mq1 += "select '" + dt2.Rows[i]["around"].ToString().Trim() + "' as fstr, '" + dt2.Rows[i]["height"].ToString().Trim() + "' as Cylinder,'" + dt2.Rows[i]["gap"].ToString().Trim() + "' as Around_gap,'" + dt2.Rows[i]["around"].ToString().Trim() + "' as no_of_around from dual";
            }
            else
            {
                //  mq1 += "select '-' as fstr,  '" + dt2.Rows[i]["around"].ToString().Trim() + "' as around,'" + minrng + "' as minrange,'" + maxrng + "' as maxrange from dual  union all  ";
                mq1 += "select '" + dt2.Rows[i]["around"].ToString().Trim() + "'  as fstr,'" + dt2.Rows[i]["height"].ToString().Trim() + "' as Cylinder,'" + dt2.Rows[i]["gap"].ToString().Trim() + "' as Around_gap, '" + dt2.Rows[i]["around"].ToString().Trim() + "' as no_of_around from dual union all ";
            }
        }
        if (mq1 == "")
        {
            SQuery = "";
            fgen.msg("-", "AMSG", "No Matching cylinder found !!'13'Minimum Range : " + txtminrng.Text + "'13'Maximum Range : " + txtmaxrng.Text);
        }
        else
            SQuery = "select * from (" + mq1 + ") order by to_number(around_gap),to_number(cylinder) ";
    }
    //------------------------------------------------------------------------------------
    void Cal()
    {
        double t1 = 0; double t2 = 0; double t3 = 0; double t4 = 0; double t5 = 0; double t6 = 0; double papaersize = 0;
        double paper = 0; double width = 0; double height = 0; double acros = 0; double wstg = 0; double gap = 0;

        //procss_wstg = fgen.make_double(txtprocess_wstg.Text.Trim());
        //wstg = (Math.Round(tot, 2) * trim_Wstg / 100 + ((Math.Round(tot, 2) * trim_Wstg / 100) + Math.Round(tot, 2)) * procss_wstg / 100);
        //txtwastge.Text = Convert.ToString(Math.Round(wstg, 2)).Replace("Infinity", "0").Replace("NaN", "0");

        width = fgen.make_double(txtwidthmm.Text.Trim());
        height = fgen.make_double(txtvarnish_trm.Text.Trim());
        acros = fgen.make_double(txtink_trm.Text.Trim());
        gap = fgen.make_double(txtfoil1_trm.Text.Trim());

        papaersize = ((height * acros) + ((acros - 1) * 3) + 15) / 10;

        txtoh_trm.Text = Convert.ToString(Math.Round(papaersize, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        paper = fgen.make_double(txtoh_trm.Text.Trim());

        t1 = 1000000 / (paper * 10); //step 1
        t2 = t1 / (width + gap);
        t3 = 1550 / ((width * height) / 645);
        t4 = t3 / acros;
        t5 = t4 - t2;
        t6 = t5 / t4 * 100;
        txtstep1.Text = Convert.ToString(Math.Round(t1, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        txtstep2.Text = Convert.ToString(Math.Round(t2, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        txtstep3.Text = Convert.ToString(Math.Round(t3, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        txtstep4.Text = Convert.ToString(Math.Round(t4, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        txtstep5.Text = Convert.ToString(Math.Round(t5, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        txttrmwastg.Text = Convert.ToString(Math.Round(t6, 2)).Replace("Infinity", "0").Replace("NaN", "0");

        ///////////////////////////LABEL COSTING CALCUALTION

        double varnish_gsm = 0; double varnish_used = 0; double varnish = 0; double trim_Wstg = 0; double overhead = 0; double oh = 0; double totcost = 0;
        double profit = 0; double profit1 = 0; double sp = 0; double sqinch = 0; double sqinch_lbl = 0; double price = 0; double pc = 0;
        double procss_wstg = 0; wstg = 0; double inkgsm = 0; double ink = 0; double foil = 0; double film = 0; double tot = 0; height = 0; width = 0;
        ///for ink
        inkgsm = fgen.make_double(txtinkgsm.Text.Trim());
        ink = inkgsm * 1600 / 1000;
        txtink.Text = Convert.ToString(Math.Round(ink, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        //for varnish
        varnish_gsm = fgen.make_double(txtvarnish_gsm.Text.Trim());
        varnish_used = fgen.make_double(txtvarnish.Text.Trim());
        varnish = varnish_gsm * varnish_used / 1000;
        txtvarnish1.Text = Convert.ToString(Math.Round(varnish, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        //======================for total
        foil = fgen.make_double(txtfoil1.Text.Trim());
        film = fgen.make_double(txtpaper_Film.Text.Trim());
        tot = Math.Round(ink, 2) + Math.Round(varnish, 2) + foil + film;
        txttot.Text = Convert.ToString(Math.Round(tot, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        //for wastage
        trim_Wstg = fgen.make_double(txttrmwastg.Text.Trim());
        procss_wstg = fgen.make_double(txtprocess_wstg.Text.Trim());
        wstg = (Math.Round(tot, 2) * (trim_Wstg / 100) + ((Math.Round(tot, 2) * (trim_Wstg / 100)) + Math.Round(tot, 2)) * (procss_wstg / 100));
        txtwastge.Text = Convert.ToString(Math.Round(wstg, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        ////for O/H
        overhead = fgen.make_double(txtoverheads.Text.Trim());
        oh = (Math.Round(tot + wstg, 2) * overhead / 100);
        txtoh.Text = Convert.ToString(Math.Round(oh, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        //for total cost
        //totcost = Math.Round(tot, 2) + Math.Round(wstg, 2) + Math.Round(oh, 2);
        totcost = Math.Round(tot, 2) + Math.Round(wstg, 2);
        txttotcost.Text = Convert.ToString(Math.Round(totcost + oh, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        //for profit
        profit = fgen.make_double(txtprofit.Text.Trim());
        profit1 = (Math.Round(totcost, 2) * profit / 100);
        txtprofit1.Text = Convert.ToString(Math.Round(profit1, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        //for sp
        totcost = Math.Round(tot + wstg + oh, 2);
        sp = Math.Round(totcost, 2) + Math.Round(profit1, 2);
        txtsp.Text = Convert.ToString(Math.Round(sp, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        //for sq inch
        sqinch = Math.Round(Math.Round(sp, 2) / 1550 * 100, 2);
        txtsqinch.Text = Convert.ToString(Math.Round(sqinch, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        //for sq inch label
        width = fgen.make_double(txtwidthmm.Text.Trim());
        height = fgen.make_double(txtvarnish_trm.Text.Trim());
        sqinch_lbl = Math.Round((width * height) / 645, 2);
        txtsq_inch_lbl.Text = Convert.ToString(Math.Round(sqinch_lbl, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        ///for price
        price = Math.Round(sqinch_lbl, 2) * Math.Round(sqinch, 2) * 10;
        double mult = 2;
        if (ddSel.SelectedItem.Text.ToUpper() == "SINGLE") mult = 1;
        txtprice.Text = Convert.ToString(Math.Round(price, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        if (mult == 1) divLablePrice.Visible = false;
        else divLablePrice.Visible = true;
        if (mult < 2) mult = 0;
        txtLablePrice.Text = Convert.ToString(Math.Round(price * mult, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        //for price per pc
        pc = Math.Round(price, 5) / 1000;
        txtperpc.Text = Convert.ToString(Math.Round(pc, 5)).Replace("Infinity", "0").Replace("NaN", "0");
    }
    //------------------------------------------------------------------------------------    
    protected void btnwidth_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "WIDTH";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Width", frm_qstr);
    }
    protected void btnrefresh_ServerClick(object sender, EventArgs e)
    {
        Cal();
    }
    protected void btncylinder_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "CYLINDER";
        if (txtwidthmm.Text == "" || txtwidthmm.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please enter width before selection");
            return;
        }
        else
        {
            make_qry_4_popup();
            if (SQuery != "")
                fgen.Fn_open_sseek("Select Entry", frm_qstr);
        }
    }
    protected void btnparty_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "CUST";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer", frm_qstr);
    }
    protected void btnicode_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "ITEM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select JobName", frm_qstr);
    }
    protected void btnC_ServerClick(object sender, EventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "MATL";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Material ", frm_qstr);
    }
    protected void btnvarnish_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "VARNISH";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Varnish", frm_qstr);
    }
    protected void btnFoil_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "FOIL";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Foil Type", frm_qstr);
    }
}

// ALTER TABLE FINMLAB.WB_CYLINDER MODIFY COL2 VARCHAR2(50);
//ALTER TABLE FINMLAB.WB_CYLINDER MODIFY COL3 VARCHAR2(50);
