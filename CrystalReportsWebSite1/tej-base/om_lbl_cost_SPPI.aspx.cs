using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;


//tej-base/web_mach_mast.aspx  ============ WB_MACH  ===========web machine master path and dummy icon id
// COST_SPPI  DUMMY ICON ON STATIC FOR SPPI COSTING

public partial class om_lbl_cost_SPPI : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, SQuery2, col1, col2, col3, vardate, fromdt, todt, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0; string mq0, mq1, mq2;
    double db0, db1, db2, db3, db4, db5, db6, db7, db8, db9, db10, db11, db12, db13, db14, db15, db16, db17, db18, db19, db20;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable dtCol = new DataTable();
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
                        hfFormID.Value = frm_formID;
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
                txt_consum_varnish.Text = "12";
                txtunit_conum_white_var.Text = "12";
                txt_varnish_usage.Text = "2.5";
                txtusage_ink.Text = "1";
                txt_Tot_ink_usage.Text = "2.5";
                txt_del_desp.Text = "50";
            }
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
        //tab5.Visible = false;
        //tab4.Visible = false;
        //tab3.Visible = false;
        //tab2.Visible = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false;
        btnrefresh.Disabled = true; btnwidth.Enabled = false;//btncylinder.Enabled = false;
        btnvarnish1.Enabled = false; btndie.Enabled = false; btnemb_varnish.Enabled = false; btnembossing_Var.Enabled = false;
        btnmatl1.Enabled = false; btnmatl3.Enabled = false; btnmatl2.Enabled = false; btnmatl4.Enabled = false; btnink.Enabled = false; btnplate.Enabled = false;
        btnFoil.Enabled = false; btnvarnish.Enabled = false; btnicode.Enabled = false; btnparty.Enabled = false; btnmch1.Enabled = false; btnmch2.Enabled = false;
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnwidth.Enabled = true; btnicode.Enabled = true; // btncylinder.Enabled = true;
        btnprint.Disabled = true; btnlist.Disabled = true; btnrefresh.Disabled = false; btnmch1.Enabled = true; btnmch2.Enabled = true;
        btnplate.Enabled = true; btnvarnish1.Enabled = true; btndie.Enabled = true;
        btnemb_varnish.Enabled = true; btnembossing_Var.Enabled = true;
        btnmatl1.Enabled = true; btnmatl3.Enabled = true; btnmatl2.Enabled = true; btnmatl4.Enabled = true; btnink.Enabled = true;
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
        frm_tabname = "somas_anx";
        switch (Prg_Id)
        {
            case "F10197":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "LB");
                lblheader.Text = "Label Costing";
                lblcostingdiv.Visible = false;
                lblcostingdiv3.Visible = true;
                lblcostingdiv2.Visible = true;
                break;
            default:
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "LC");//LABEL COSTING FOR SPPI
                lblheader.Text = "Offset Label Costing"; //F10199 FRM_ID
                lblcostingdiv.Visible = false;
                lblcostingdiv2.Visible = false;
                lblcostingdiv3.Visible = false;
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        typePopup = "N";
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
            case "MATL1":
            case "MATL4":
                //SQuery = "select trim(branchcd)||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||col2 as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col2 as code,col1 as name,num1 as price,to_char(vchdate,'yyyyMMdd') as vdd  from wb_master where branchcd='" + frm_mbr + "' and id='MM01' order by vdd desc";
                SQuery = "select trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr ,type1 as code,name,acref3 as price from TYPEGRP  where branchcd!='DD' and id='MM' order by type1";
                break;
            case "MATL2":
                SQuery = "select trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr ,type1 as code,name,acref3 as price from TYPEGRP  where branchcd!='DD' and id='^N' order by type1";
                break;

            case "MATL3":
                SQuery = "select trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr ,type1 as code,name,acref3 as price from TYPEGRP  where branchcd!='DD' and id='^O' order by type1";
                break;

            case "INK":
                SQuery = "select trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr ,type1 as code,name,acref3 as price from TYPEGRP  where branchcd!='DD' and id='^H' order by type1";
                break;

            case "PLATE":
                SQuery = "select trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr ,type1 as code,name,acref3 as price from TYPEGRP  where branchcd!='DD' and id='^G' order by type1";
                break;

            case "VARNISH":
                SQuery = "select trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr ,type1 as code,name,acref3 as price from TYPEGRP  where branchcd!='DD' and id='V1' order by type1";
                break;

            case "DIE":
                SQuery = "select trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr ,type1 as code,name,acref3 as price from TYPEGRP  where branchcd!='DD' and id='^I' order by type1";
                break;

            case "EMBOSSING_WHITE":
                SQuery = "select trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr ,type1 as code,name,acref3 as price from TYPEGRP  where branchcd!='DD' and id='^K' order by type1";
                break;

            case "EMBOSSING_VARNISH":
                SQuery = "select trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr ,type1 as code,name,acref3 as price from TYPEGRP  where branchcd!='DD' and id='^J' order by type1";
                break;

            case "MACHINE1":
            case "MACHINE2":
                SQuery = "select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,mchname,mchcode,tot_mcost as machine_cost from WB_MACH_COST  where branchcd='" + frm_mbr + "' and type='^G' and vchdate " + DateRange + "";
                SQuery = "select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,mchname,mchcode,tot_mcost as machine_cost from WB_MACH_COST  where branchcd='" + frm_mbr + "' and type='^G'";
                break;

            case "FOIL":
                SQuery = "select 'GOLD' AS FSTR,'GOLD_SILVER' AS CHOICE,'-' AS SELECTION FROM DUAL UNION ALL select 'DULL' AS FSTR,'DULL_GOLD' AS CHOICE,'-' AS SELECTION FROM DUAL UNION ALL select 'N' AS FSTR,'NONE' AS CHOICE,'-' AS SELECTION FROM DUAL";
                SQuery = "SELECT TYPE1 AS FSTR,NAME,ACREF3  FROM TYPEGRP WHERE ID='QM' ORDER BY FSTR";
                break;

            case "WIDTH":
                SQuery = "select  TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR,NVL(NUM2,0) AS WIDTH,NVL(NUM3,0) AS HEIGHT from wb_cylinder where branchcd='" + frm_mbr + "' and type='TM' and vchdate " + DateRange + "";
                break;

            case "CUST":
                SQuery = "SELECT TRIM(ACODE) AS FSTR,TRIM(ACODE) AS CUSTOMER_CODE,TRIM(ANAME) AS CUSTOMER FROM FAMST WHERE SUBSTR(TRIM(aCODE),1,2)='16' ORDER BY CUSTOMER";
                break;

            case "ITEM":
                SQuery = "SELECT TRIM(ICODE) AS FSTR,TRIM(ICODE) AS JOB_CODE,TRIM(INAME) AS JOB_NAME FROM ITEM WHERE SUBSTR(TRIM(ICODE),1,1) IN ('7','9') AND LENGTH(TRIM(ICODE))>=8 ORDER BY JOB_NAME";
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
            case "CYLINDER":
                cyl_cal();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.type,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
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
        if (Prg_Id != "F10197")
        {
            if (txtmatl1.Text.Trim() == "-" || txtmatl1.Text.Trim() == "")
            {
                fgen.msg("-", "AMSG", "Please Select Material 1");
                return;
            }
            //if (txtmatl2.Text.Trim() == "-" || txtmatl2.Text.Trim() == "")
            //{
            //    fgen.msg("-", "AMSG", "Please Select Material2");
            //    return;
            //}
            if (txtinkname.Text.Trim() == "-" || txtinkname.Text.Trim() == "")
            {
                fgen.msg("-", "AMSG", "Please Select Ink Value");
                return;
            }
            if (txtplate_name.Text.Trim() == "-" || txtplate_name.Text.Trim() == "")
            {
                fgen.msg("-", "AMSG", "Please Select Plates Value");
                return;
            }
            if (txtvarnish_name.Text.Trim() == "-" || txtvarnish_name.Text.Trim() == "")
            {
                fgen.msg("-", "AMSG", "Please Select Varnish Value");
                return;
            }
            //if (txtdiename.Text.Trim() == "-" || txtdiename.Text.Trim() == "")
            //{
            //    fgen.msg("-", "AMSG", "Please Select Die Value");
            //    return;
            //}
            //if (txtembvarnish_name.Text.Trim() == "-" || txtembvarnish_name.Text.Trim() == "")
            //{
            //    fgen.msg("-", "AMSG", "Please Select Embossing Varnish Value");
            //    return;
            //}
            //if (txtembossing_var_name.Text.Trim() == "-" || txtembossing_var_name.Text.Trim() == "")
            //{
            //    fgen.msg("-", "AMSG", "Please Select Embossing White Screen/Printing Value");
            //    return;
            //}

            if (txtmchname1.Text.Trim() == "-" || txtmchname1.Text.Trim() == "")
            {
                fgen.msg("-", "AMSG", "Please Select Machine A ");
                return;
            }
            //if (txtmchname2.Text.Trim() == "-" || txtmchname2.Text.Trim() == "")
            //{
            //    fgen.msg("-", "AMSG", "Please Select Machine B");
            //    return;
            //}
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
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        ViewState["sg1"] = null;
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
        vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtentby.Text = frm_uname;
        txtendtdt.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        //txtlbl5.Text = "-";
        // txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
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
        SQuery = "select trim(branchcd)||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr, num1,num2,num3 from wb_master where branchcd='" + frm_mbr + "' and id='AR01' and vchdate " + DateRange + "";
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
            if (CP_BTN.Trim().Substring(0, 3) == "BTN" || CP_BTN.Trim().Substring(0, 3) == "SG1")//|| CP_BTN.Trim().Substring(0, 3) == "SG2" || CP_BTN.Trim().Substring(0, 3) == "SG3"
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
                        //txtlbl2.Text = dt.Rows[i]["frm_header"].ToString().Trim();
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
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtfoil.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtfoil1.Text = dt.Rows[0]["acref3"].ToString().Trim();
                    }
                    Cal();
                    break;
                case "MATL1":
                    if (col1.Length <= 0) return;
                    //SQuery = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col2 as code,col1 as name,num1 as price,to_char(vchdate,'yyyyMMdd') as vdd  from wb_master where branchcd='" + frm_mbr + "' and id='MM01' and  trim(branchcd)||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||col2='" + col1 + "' order by vdd desc";
                    SQuery = "select type1 as code,name,acref3 as price from TYPEGRP  where trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' order by type1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtmatl1_code.Text = dt.Rows[0]["code"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
                        txtmatl1.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtmatl1_val.Text = dt.Rows[0]["price"].ToString().Trim();
                    }
                    Cal();
                    break;

                case "MATL2":
                    if (col1.Length <= 0) return;
                    //SQuery = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col2 as code,col1 as name,num1 as price,to_char(vchdate,'yyyyMMdd') as vdd  from wb_master where branchcd='" + frm_mbr + "' and id='MM01' and  trim(branchcd)||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||col2='" + col1 + "' order by vdd desc";
                    SQuery = "select type1 as code,name,acref3 as price from TYPEGRP  where  trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' order by type1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtmatl2_code.Text = dt.Rows[0]["code"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
                        txtmatl2.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtmatl2_val.Text = dt.Rows[0]["price"].ToString().Trim();
                    }
                    Cal();
                    break;

                case "MATL3":
                    if (col1.Length <= 0) return;
                    SQuery = "select type1 as code,name,acref3 as price from TYPEGRP  where  trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' order by type1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtmatl3_code.Text = dt.Rows[0]["code"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
                        txtmatl3.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtmatl3_val.Text = dt.Rows[0]["price"].ToString().Trim();
                    }
                    Cal();
                    break;

                case "MATL4":
                    if (col1.Length <= 0) return;
                    SQuery = "select type1 as code,name,acref3 as price from TYPEGRP  where  trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' order by type1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtmatl4_code.Text = dt.Rows[0]["code"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
                        txtmatl4.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtmatl4_val.Text = dt.Rows[0]["price"].ToString().Trim();
                    }
                    Cal();
                    break;

                case "INK":
                    if (col1.Length <= 0) return;
                    SQuery = "select type1 as code,name,acref3 as price from TYPEGRP  where  trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' order by type1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtmatl4_code.Text = dt.Rows[0]["code"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
                        txtinkname.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtinkval.Text = dt.Rows[0]["price"].ToString().Trim();
                    }
                    Cal();
                    break;

                case "PLATE":
                    if (col1.Length <= 0) return;
                    SQuery = "select type1 as code,name,acref3 as price from TYPEGRP  where trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' order by type1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtplatecode.Text = dt.Rows[0]["code"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
                        txtplate_name.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtplate_unit_cost.Text = dt.Rows[0]["price"].ToString().Trim();
                    }
                    Cal();
                    break;

                case "VARNISH":
                    if (col1.Length <= 0) return;
                    SQuery = "select type1 as code,name,acref3 as price from TYPEGRP  where trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' order by type1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvarnish_code.Text = dt.Rows[0]["code"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
                        txtvarnish_name.Text = dt.Rows[0]["name"].ToString().Trim();
                        txt_varnish_cost.Text = dt.Rows[0]["price"].ToString().Trim();
                    }
                    Cal();
                    break;

                case "DIE":
                    if (col1.Length <= 0) return;
                    SQuery = "select type1 as code,name,acref3 as price from TYPEGRP  where trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' order by type1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtdie_Code.Text = dt.Rows[0]["code"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
                        txtdiename.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtdierate.Text = dt.Rows[0]["price"].ToString().Trim();
                    }
                    Cal();
                    break;


                case "EMBOSSING_WHITE":
                    if (col1.Length <= 0) return;
                    SQuery = "select type1 as code,name,acref3 as price from TYPEGRP  where trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' order by type1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtembossing_var_code.Text = dt.Rows[0]["code"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
                        txtembossing_var_name.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtembossing_var_rate.Text = dt.Rows[0]["price"].ToString().Trim();
                    }
                    Cal();
                    break;

                case "EMBOSSING_VARNISH":
                    if (col1.Length <= 0) return;
                    SQuery = "select type1 as code,name,acref3 as price from TYPEGRP  where trim(branchcd)||trim(id)||trim(type1)||vchnum||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' order by type1";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtemb_varnish_code.Text = dt.Rows[0]["code"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
                        txtembvarnish_name.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtembvarnish_val.Text = dt.Rows[0]["price"].ToString().Trim();
                    }
                    Cal();
                    break;

                case "MACHINE1":
                    if (col1.Length <= 0) return;
                    /// FOR AUTO CAL FOR MACHINE COST AGAIN
                    hf3.Value = col1;
                    cal1();
                    #region this calcuation done in cal1();
                    //SQuery = "select * from WB_MACH_COST where trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'";
                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //db0 = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0; db13 = 0; db14 = 0; db15 = 0; db16 = 0; db17 = 0; db18 = 0; db19 = 0; db20 = 0;
                    //if (dt.Rows.Count > 0)
                    //{
                    //    db0 = fgen.make_double(dt.Rows[0]["mch_cost"].ToString().Trim());//machine cost=======b87
                    //    db1 = fgen.make_double(dt.Rows[0]["mch_rt_phr"].ToString().Trim()); //machine rate per hour======b88                       
                    //    db3 = fgen.make_double(dt.Rows[0]["oper_sal"].ToString().Trim());//operator salary=========b90
                    //    db4 = fgen.make_double(dt.Rows[0]["oper_sal_ph"].ToString().Trim());//operator salry per hour=====b91
                    //    db5 = fgen.make_double(dt.Rows[0]["no_imp_pmnt"].ToString().Trim());//no of impression p/mnt========b92
                    //    db6 = fgen.make_double(dt.Rows[0]["mx_rmtr_phr"].ToString().Trim()); //max running mtr in 1 hr=========b93
                    //    ////time for the job formula========txttot_rung_mtr/db6
                    //    db7 = fgen.make_double(txttot_rung_mtr.Text) / db6;///////time for the job=========b94                       
                    //    db8 = fgen.make_double(dt.Rows[0]["set_time"].ToString().Trim());///setting time==========b95
                    //    //  db9 = fgen.make_double(dt.Rows[0]["tot_time_job"].ToString().Trim());//total time for the job==========b96
                    //    db9 = db7 + db8;//total time for the job.......running time===========b96
                    //    db10 = fgen.make_double(dt.Rows[0]["tot_ele_use"].ToString().Trim());//Total Electricity usage ========b97
                    //    db11 = fgen.make_double(dt.Rows[0]["elce_chg_phr"].ToString().Trim());//Electricity charge for 1 hr =========b98
                    //    db12 = db11 * (db9 / 60);//Total Electricity charge for the job=======b99
                    //    //db2 = fgen.make_double(dt.Rows[0]["mch_cost1"].ToString().Trim());//machine cost for the job=====b89
                    //    db2 = ((db9 / 60) * db1);//machine cost for the job===========b89
                    //    db13 = Math.Round((db12 + db4 + db2) * fgen.make_double(txtpass.Text.Trim()), 2);
                    //    ////////
                    //    txtmch1_code.Text = dt.Rows[0]["mchcode"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
                    //    txtmchname1.Text = dt.Rows[0]["mchname"].ToString().Trim();
                    //    txtmch1_cost.Text = Convert.ToString(db13);
                    //    hf3.Value=txtmch1_cost.Text;
                    //}
                    #endregion
                    #region old logic for machine cost
                    //SQuery = "select mchname,mchcode,tot_mcost as machine_cost from WB_MACH_COST where trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'";
                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    txtmch1_code.Text = dt.Rows[0]["mchcode"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
                    //    txtmchname1.Text = dt.Rows[0]["mchname"].ToString().Trim();
                    //    txtmch1_cost.Text = dt.Rows[0]["machine_cost"].ToString().Trim();
                    //}
                    #endregion
                    Cal();
                    break;

                case "MACHINE2":
                    if (col1.Length <= 0) return;
                    /// FOR AUTO CAL FOR MACHINE COST AGAIN
                    hf4.Value = col1;
                    cal2();
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
                    SQuery = "Select a.* from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "'";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtacode.Text = dt.Rows[0]["ACODE"].ToString().Trim();
                        txticode.Text = dt.Rows[0]["ICODE"].ToString().Trim();
                        txtaname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(aname) as aname from famst where trim(acode)='" + dt.Rows[0]["ACODE"].ToString().Trim() + "'", "aname");//COL2
                        txtiname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(iname) as iname from item where trim(icode)='" + dt.Rows[0]["ICODE"].ToString().Trim() + "'", "iname");//COL3
                        if (txtaname.Text.Trim().Length <= 1)
                        {
                            txtaname.Text = dt.Rows[0]["ANAME"].ToString().Trim();
                        }
                        if (txtiname.Text.Trim().Length <= 1)
                        {
                            txtiname.Text = dt.Rows[0]["INAME"].ToString().Trim();
                        }
                        txtlbl_hyt.Text = dt.Rows[0]["T1"].ToString().Trim();
                        txtlbl_width.Text = dt.Rows[0]["T2"].ToString().Trim();
                        txtqty.Text = dt.Rows[0]["T3"].ToString().Trim();
                        txt_acros.Text = dt.Rows[0]["T4"].ToString().Trim();
                        txtaround.Text = dt.Rows[0]["T5"].ToString().Trim();
                        txtups.Text = dt.Rows[0]["T6"].ToString().Trim();
                        txttrmwastg.Text = dt.Rows[0]["T7"].ToString().Trim();
                        txtsetting_wstg_pclor.Text = dt.Rows[0]["T8"].ToString().Trim();
                        txtrungmtr_mtr.Text = dt.Rows[0]["T9"].ToString().Trim();
                        txt_tot_sqm.Text = dt.Rows[0]["T10"].ToString().Trim();
                        txtpass.Text = dt.Rows[0]["T11"].ToString().Trim();
                        txtreqwidth.Text = dt.Rows[0]["T12"].ToString().Trim();
                        txtcolor.Text = dt.Rows[0]["T13"].ToString().Trim();
                        txt_tot_wstg.Text = dt.Rows[0]["T14"].ToString().Trim();
                        txt_gap_acros.Text = dt.Rows[0]["T15"].ToString().Trim();
                        txtgaparound.Text = dt.Rows[0]["T16"].ToString().Trim();
                        txtdiff.Text = dt.Rows[0]["T17"].ToString().Trim();
                        txtrungmtr_mm.Text = dt.Rows[0]["T18"].ToString().Trim();
                        txttot_rung_mtr.Text = dt.Rows[0]["T19"].ToString().Trim();
                        txtprod_cost.Text = dt.Rows[0]["T20"].ToString().Trim();
                        txt_margin_considered.Text = dt.Rows[0]["T21"].ToString().Trim();
                        txtmargin_cost_AED.Text = dt.Rows[0]["T22"].ToString().Trim();
                        txttotal.Text = dt.Rows[0]["T23"].ToString().Trim();
                        txtvat_percent.Text = dt.Rows[0]["T24"].ToString().Trim();
                        txtval_Value.Text = dt.Rows[0]["T25"].ToString().Trim();
                        txt_grand_tot.Text = dt.Rows[0]["T26"].ToString().Trim();
                        txtmatl1.Text = dt.Rows[0]["T27"].ToString().Trim();
                        txtmatl1_val.Text = dt.Rows[0]["T28"].ToString().Trim();
                        txtmatl2.Text = dt.Rows[0]["T29"].ToString().Trim();
                        txtmatl2_val.Text = dt.Rows[0]["T30"].ToString().Trim();
                        txtmatl3.Text = dt.Rows[0]["T31"].ToString().Trim();
                        txtmatl3_val.Text = dt.Rows[0]["T32"].ToString().Trim();
                        txtmatl4.Text = dt.Rows[0]["T33"].ToString().Trim();
                        txtmatl4_val.Text = dt.Rows[0]["T34"].ToString().Trim();
                        txtinkname.Text = dt.Rows[0]["T35"].ToString().Trim();
                        txtinkval.Text = dt.Rows[0]["T36"].ToString().Trim();
                        txtusage_ink.Text = dt.Rows[0]["T37"].ToString().Trim();
                        txt_Tot_ink_usage.Text = dt.Rows[0]["T38"].ToString().Trim();
                        txt_tot_ink_cost.Text = dt.Rows[0]["T39"].ToString().Trim();
                        txtplate_name.Text = dt.Rows[0]["T40"].ToString().Trim();
                        txtplate_unit_cost.Text = dt.Rows[0]["T41"].ToString().Trim();
                        txt_tot_plate_cost.Text = dt.Rows[0]["T42"].ToString().Trim();
                        txtvarnish_name.Text = dt.Rows[0]["T43"].ToString().Trim();
                        txt_varnish_cost.Text = dt.Rows[0]["T44"].ToString().Trim();
                        txt_varnish_usage.Text = dt.Rows[0]["T45"].ToString().Trim();
                        txt_tot_varnish_cost.Text = dt.Rows[0]["T46"].ToString().Trim();
                        txtdiename.Text = dt.Rows[0]["T47"].ToString().Trim();
                        txtdierate.Text = dt.Rows[0]["T48"].ToString().Trim();
                        txtdie_area.Text = dt.Rows[0]["T49"].ToString().Trim();
                        txtdie_reqd.Text = dt.Rows[0]["T50"].ToString().Trim();
                        txtdie_width.Text = dt.Rows[0]["T51"].ToString().Trim();
                        txt_die_hight.Text = dt.Rows[0]["T52"].ToString().Trim();
                        txtdiecost.Text = dt.Rows[0]["T53"].ToString().Trim();
                        txtembvarnish_name.Text = dt.Rows[0]["T54"].ToString().Trim();
                        txtembvarnish_val.Text = dt.Rows[0]["T55"].ToString().Trim();
                        txtemb_area_varnish.Text = dt.Rows[0]["T56"].ToString().Trim();
                        txt_consum_varnish.Text = dt.Rows[0]["T57"].ToString().Trim();
                        txt_tot_embas_varnish_Val.Text = dt.Rows[0]["T58"].ToString().Trim();
                        txt_screen_exposing_chg.Text = dt.Rows[0]["T59"].ToString().Trim();
                        txt_totrate_emb_varnish.Text = dt.Rows[0]["T60"].ToString().Trim();
                        txtembossing_var_name.Text = dt.Rows[0]["T61"].ToString().Trim();
                        txtembossing_var_rate.Text = dt.Rows[0]["T62"].ToString().Trim();
                        txtarea_embosing_white.Text = dt.Rows[0]["T63"].ToString().Trim();
                        txtunit_conum_white_var.Text = dt.Rows[0]["T64"].ToString().Trim();
                        txtemb_Var_conum_white.Text = dt.Rows[0]["T65"].ToString().Trim();
                        txtscreen_exposing.Text = dt.Rows[0]["T66"].ToString().Trim();
                        txt_totrate_emb_white.Text = dt.Rows[0]["T67"].ToString().Trim();
                        txtmchname1.Text = dt.Rows[0]["T68"].ToString().Trim();
                        txtmch1_cost.Text = dt.Rows[0]["T69"].ToString().Trim();
                        txtmchname2.Text = dt.Rows[0]["T70"].ToString().Trim();
                        txtmch2_cost.Text = dt.Rows[0]["T71"].ToString().Trim();
                        txttot_rt_For_emb_varnish.Text = dt.Rows[0]["T72"].ToString().Trim();
                        txt_screen_print.Text = dt.Rows[0]["T73"].ToString().Trim();
                        txt_del_desp.Text = dt.Rows[0]["T74"].ToString().Trim();
                        txtmch1_code.Text = dt.Rows[0]["T75"].ToString().Trim();
                        txtmch2_code.Text = dt.Rows[0]["T76"].ToString().Trim();
                        txt_unit_price_matl.Text = dt.Rows[0]["T77"].ToString().Trim();
                        txtcost_matl.Text = dt.Rows[0]["T78"].ToString().Trim();
                        ///================================
                        ///

                        txtCylInch.Text = dt.Rows[0]["T79"].ToString().Trim();
                        txtTeeth.Text = dt.Rows[0]["T80"].ToString().Trim();
                        txtTotInkCons.Text = dt.Rows[0]["T81"].ToString().Trim();
                        txtPlateAreaCM.Text = dt.Rows[0]["T82"].ToString().Trim();

                        dt.Dispose();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        txtentby.Text = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        txtendtdt.Text = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                        Cal();
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
                    #region
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
                    #endregion
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
                case "CYLINDER2":
                    txtCylInch.Text = col3;
                    txtgaparound.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                    txtTeeth.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");
                    break;
                case "CYLINDER":
                    if (col3 == "")
                    {
                        hffield.Value = "CYLINDER2";
                        SQuery = "Select (num2*" + txtlbl_width.Text.ToString().toDouble() + ") as fstr,num1 as Cylinder_Inch,num2 as Cyliner_mm,0 as around_gap,num3 as Cylinder_Teeth, Name,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,to_char(EDT_DT,'dd/MM/yyyy') as edt_dt from wb_master where BRANCHCD!='DD' AND id='AR01'  ";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);

                        fgen.Fn_open_sseek("Showing All from Inventory", frm_qstr);
                    }
                    else
                    {
                        txtCylInch.Text = col3;
                        txtgaparound.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                        txtTeeth.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");
                    }
                    break;
                case "CYLINDER_M":
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "CALC", "Y");
                        cyl_cal();
                        //cyl_calWithAI();
                    }
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
            SQuery = "SELECT a.VCHNUM AS ENTRY_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS ENTRY_dT,to_char(a.vchdate,'yyyymmdd') as vdd,trim(a.acode) as cust_code,b.aname as cust_name,trim(a.icode) as item_code,c.iname as item_name,t1 as lbl_height,t2 as lbl_width,t3 as qty,t4 as acros,t5 as arnd,t6 as ups,t7 as actual_width_of_matl,t27 as matl1,t28 as matl1_rate,t29 as matl2,t30 as matl2_rate,t31 as matl3,t32 as matl3_rate,t33 as matl4,t34 as matl_rate,t35 as ink_name,t36 as ink_unit_rate,t40  as plate_name,t41 as plate_unit_cost,t43 as varnish_name,t44 as varnish_cost,t47 as die_name,t48 as die_unit_rate,t54 as embossing_varnish_name,t55 as embossing_unit_rate,t61 as emb_white_Screen_print,t62 as embwhite_unit_rt ,t68 as Printing_machine,t69 as mach_a_cost,t70 as slitting_mach,t71 as mach_b_cost from " + frm_tabname + " a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + " order by vdd desc";
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
        oporow["ACODE"] = txtacode.Text.Trim().ToUpper();//acode
        oporow["ICODE"] = txticode.Text.Trim();//icode
        oporow["ANAME"] = txtaname.Text.Trim().ToUpper();
        oporow["INAME"] = txtiname.Text.Trim().ToUpper();
        oporow["T1"] = fgen.make_double(txtlbl_hyt.Text.Trim());
        oporow["T2"] = fgen.make_double(txtlbl_width.Text.Trim());
        oporow["T3"] = fgen.make_double(txtqty.Text.Trim());
        oporow["T4"] = fgen.make_double(txt_acros.Text.Trim());
        oporow["T5"] = fgen.make_double(txtaround.Text.Trim());
        oporow["T6"] = fgen.make_double(txtups.Text.Trim());
        oporow["T7"] = fgen.make_double(txttrmwastg.Text.Trim());
        oporow["T8"] = fgen.make_double(txtsetting_wstg_pclor.Text.Trim());
        oporow["T9"] = fgen.make_double(txtrungmtr_mtr.Text.Trim());
        oporow["T10"] = fgen.make_double(txt_tot_sqm.Text.Trim());
        oporow["T11"] = fgen.make_double(txtpass.Text.Trim());
        oporow["T12"] = fgen.make_double(txtreqwidth.Text.Trim());
        oporow["T13"] = fgen.make_double(txtcolor.Text.Trim());
        oporow["T14"] = fgen.make_double(txt_tot_wstg.Text.Trim());
        oporow["T15"] = fgen.make_double(txt_gap_acros.Text.Trim());
        oporow["T16"] = fgen.make_double(txtgaparound.Text.Trim());
        oporow["T17"] = fgen.make_double(txtdiff.Text.Trim());
        oporow["T18"] = fgen.make_double(txtrungmtr_mm.Text.Trim());
        oporow["T19"] = fgen.make_double(txttot_rung_mtr.Text.Trim());

        oporow["T20"] = fgen.make_double(txtprod_cost.Text.Trim());
        oporow["T21"] = fgen.make_double(txt_margin_considered.Text.Trim());
        oporow["T22"] = fgen.make_double(txtmargin_cost_AED.Text.Trim());
        oporow["T23"] = fgen.make_double(txttotal.Text.Trim());
        oporow["T24"] = fgen.make_double(txtvat_percent.Text.Trim());
        oporow["T25"] = fgen.make_double(txtval_Value.Text.Trim());
        oporow["T26"] = fgen.make_double(txt_grand_tot.Text.Trim());
        //matl 1
        if (txtmatl1.Text.Length > 29)
        {
            oporow["T27"] = txtmatl1.Text.Trim().ToUpper().Substring(0, 29);
        }
        else
        {
            oporow["T27"] = txtmatl1.Text.Trim().ToUpper();
        }
        oporow["T28"] = fgen.make_double(txtmatl1_val.Text.Trim());
        //////////matl 2
        if (txtmatl2.Text.Length > 29)
        {
            oporow["T29"] = txtmatl2.Text.Trim().ToUpper().Substring(0, 29);
        }
        else
        {
            oporow["T29"] = txtmatl2.Text.Trim().ToUpper();
        }
        oporow["T30"] = fgen.make_double(txtmatl2_val.Text.Trim());
        ///matl3       
        if (txtmatl3.Text.Length > 29)
        {
            oporow["T31"] = txtmatl3.Text.Trim().ToUpper().Substring(0, 29);
        }
        else
        {
            oporow["T31"] = txtmatl3.Text.Trim().ToUpper();
        }
        oporow["T32"] = fgen.make_double(txtmatl3_val.Text.Trim());
        //matl 4
        if (txtmatl4.Text.Length > 29)
        {
            oporow["T33"] = txtmatl4.Text.Trim().ToUpper().Substring(0, 29);
        }
        else
        {
            oporow["T33"] = txtmatl4.Text.Trim().ToUpper();
        }
        oporow["T34"] = fgen.make_double(txtmatl4_val.Text.Trim());
        ////=============ink details
        if (txtinkname.Text.Length > 29)
        {
            oporow["T35"] = txtinkname.Text.Trim().ToUpper().Substring(0, 29);
        }
        else
        {
            oporow["T35"] = txtinkname.Text.Trim().ToUpper();
        }
        oporow["T36"] = fgen.make_double(txtinkval.Text.Trim());
        oporow["T37"] = fgen.make_double(txtusage_ink.Text.Trim());
        oporow["T38"] = fgen.make_double(txt_Tot_ink_usage.Text.Trim());
        oporow["T39"] = fgen.make_double(txt_tot_ink_cost.Text.Trim());
        //plates details
        if (txtplate_name.Text.Length > 29)
        {
            oporow["T40"] = txtplate_name.Text.Trim().ToUpper().Substring(0, 29);
        }
        else
        {
            oporow["T40"] = txtplate_name.Text.Trim().ToUpper();
        }
        oporow["T41"] = fgen.make_double(txtplate_unit_cost.Text.Trim());
        oporow["T42"] = fgen.make_double(txt_tot_plate_cost.Text.Trim());
        //varnish details
        if (txtvarnish_name.Text.Length > 29)
        {
            oporow["T43"] = txtvarnish_name.Text.Trim().ToUpper().Substring(0, 29);
        }
        else
        {
            oporow["T43"] = txtvarnish_name.Text.Trim().ToUpper();
        }
        oporow["T44"] = fgen.make_double(txt_varnish_cost.Text.Trim());
        oporow["T45"] = fgen.make_double(txt_varnish_usage.Text.Trim());
        oporow["T46"] = fgen.make_double(txt_tot_varnish_cost.Text.Trim());
        //============die details
        if (txtdiename.Text.Length > 29)
        {
            oporow["T47"] = txtdiename.Text.Trim().ToUpper().Substring(0, 29);
        }
        else
        {
            oporow["T47"] = txtdiename.Text.Trim().ToUpper();
        }
        oporow["T48"] = fgen.make_double(txtdierate.Text.Trim());
        oporow["T49"] = fgen.make_double(txtdie_area.Text.Trim());
        oporow["T50"] = fgen.make_double(txtdie_reqd.Text.Trim());
        oporow["T51"] = fgen.make_double(txtdie_width.Text.Trim());
        oporow["T52"] = fgen.make_double(txt_die_hight.Text.Trim());
        oporow["T53"] = fgen.make_double(txtdiecost.Text.Trim());
        if (txtembvarnish_name.Text.Length > 29)
        {
            oporow["T54"] = txtembvarnish_name.Text.Trim().ToUpper().Substring(0, 29);
        }
        else
        {
            oporow["T54"] = txtembvarnish_name.Text.Trim().ToUpper();
        }
        oporow["T55"] = fgen.make_double(txtembvarnish_val.Text.Trim());

        oporow["T56"] = fgen.make_double(txtemb_area_varnish.Text.Trim());
        oporow["T57"] = fgen.make_double(txt_consum_varnish.Text.Trim());
        oporow["T58"] = fgen.make_double(txt_tot_embas_varnish_Val.Text.Trim());

        oporow["T59"] = fgen.make_double(txt_screen_exposing_chg.Text.Trim());
        oporow["T60"] = fgen.make_double(txt_totrate_emb_varnish.Text.Trim());

        if (txtembossing_var_name.Text.Length > 29)
        {
            oporow["T61"] = txtembossing_var_name.Text.Trim().ToUpper().Substring(0, 29);
        }
        else
        {
            oporow["T61"] = txtembossing_var_name.Text.Trim().ToUpper();
        }
        oporow["T62"] = fgen.make_double(txtembossing_var_rate.Text.Trim());

        oporow["T63"] = fgen.make_double(txtarea_embosing_white.Text.Trim());
        oporow["T64"] = fgen.make_double(txtunit_conum_white_var.Text.Trim());

        oporow["T65"] = fgen.make_double(txtemb_Var_conum_white.Text.Trim());
        oporow["T66"] = fgen.make_double(txtscreen_exposing.Text.Trim());

        oporow["T67"] = fgen.make_double(txt_totrate_emb_white.Text.Trim());

        if (txtmchname1.Text.Length > 29)
        {
            oporow["T68"] = txtmchname1.Text.Trim().ToUpper().Substring(0, 29);
        }
        else
        {
            oporow["T68"] = txtmchname1.Text.Trim().ToUpper();
        }
        oporow["T69"] = fgen.make_double(txtmch1_cost.Text.Trim());

        if (txtmchname2.Text.Length > 29)
        {
            oporow["T70"] = txtmchname2.Text.Trim().ToUpper().Substring(0, 29);
        }
        else
        {
            oporow["T70"] = txtmchname2.Text.Trim().ToUpper();
        }

        oporow["T71"] = fgen.make_double(txtmch2_cost.Text.Trim());

        oporow["T72"] = fgen.make_double(txttot_rt_For_emb_varnish.Text.Trim());
        oporow["T73"] = fgen.make_double(txt_screen_print.Text.Trim());
        oporow["T74"] = fgen.make_double(txt_del_desp.Text.Trim());

        oporow["t75"] = txtmch1_code.Text.Trim();//machine a code
        oporow["t76"] = txtmch2_code.Text.Trim();//machine b code
        oporow["t77"] = fgen.make_double(txt_unit_price_matl.Text.Trim());
        oporow["t78"] = fgen.make_double(txtcost_matl.Text.Trim());

        oporow["t79"] = fgen.make_double(txtCylInch.Text.Trim());
        oporow["t80"] = fgen.make_double(txtTeeth.Text.Trim());

        oporow["t81"] = fgen.make_double(txtTotInkCons.Text.Trim());
        oporow["t82"] = fgen.make_double(txtPlateAreaCM.Text.Trim());

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
    void cal2()
    {
        #region
        SQuery = "select * from WB_MACH_COST where trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + hf4.Value + "'";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        db0 = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0; db13 = 0; db14 = 0; db15 = 0; db16 = 0; db17 = 0; db18 = 0; db19 = 0; db20 = 0;
        if (dt.Rows.Count > 0)
        {
            db0 = fgen.make_double(dt.Rows[0]["mch_cost"].ToString().Trim());//machine cost=======b87
            db1 = fgen.make_double(dt.Rows[0]["mch_rt_phr"].ToString().Trim()); //machine rate per hour======b88                       
            db3 = fgen.make_double(dt.Rows[0]["oper_sal"].ToString().Trim());//operator salary=========b90
            db4 = fgen.make_double(dt.Rows[0]["oper_sal_ph"].ToString().Trim());//operator salry per hour=====b91
            db14 = fgen.make_double(dt.Rows[0]["WRK_HR_PDAY"].ToString());
            db15 = fgen.make_double(dt.Rows[0]["DAY_WRK_PM"].ToString());
            db16 = db3 / (db14 * db15);
            db5 = fgen.make_double(dt.Rows[0]["no_imp_pmnt"].ToString().Trim());//no of impression p/mnt========b92
            db6 = fgen.make_double(dt.Rows[0]["mx_rmtr_phr"].ToString().Trim()); //max running mtr in 1 hr=========b93
            ////time for the job formula========txttot_rung_mtr/db6
            db7 = fgen.make_double(txttot_rung_mtr.Text) / db6;///////time for the job=========b94                       
            db8 = fgen.make_double(dt.Rows[0]["set_time"].ToString().Trim());///setting time==========b95
            //  db9 = fgen.make_double(dt.Rows[0]["tot_time_job"].ToString().Trim());//total time for the job==========b96
            db9 = db7 + db8;//total time for the job.......running time===========b96
            db10 = fgen.make_double(dt.Rows[0]["tot_ele_use"].ToString().Trim());//Total Electricity usage ========b97
            db11 = fgen.make_double(dt.Rows[0]["elce_chg_phr"].ToString().Trim());//Electricity charge for 1 hr =========b98
            db17 = db10 / (db15 * db14);
            // db12 = db11 * (db9 / 60);//Total Electricity charge for the job=======b99
            db12 = db17 * (db9 / 60);//Total Electricity charge for the job=======b99
            //db2 = fgen.make_double(dt.Rows[0]["mch_cost1"].ToString().Trim());//machine cost for the job=====b89
            db2 = ((db9 / 60) * db1);//machine cost for the job===========b89
            //db13 = Math.Round((db12 + db4 + db2) * fgen.make_double(txtpass.Text.Trim()), 2);
            db13 = Math.Round((db12 + db16 + db2) * fgen.make_double(txtpass.Text.Trim()), 2);
            ////////
            txtmch2_code.Text = dt.Rows[0]["mchcode"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
            txtmchname2.Text = dt.Rows[0]["mchname"].ToString().Trim();
            txtmch2_cost.Text = Convert.ToString(db13);
            txtJobTime2.Text = db7.ToString();
            txtElectricityChg2.Text = (Math.Round(db12, 2)).ToString();
        }
        #endregion
    }
    //------------------------------------------------------------------------------------
    void cal1()
    {
        #region
        SQuery = "select * from WB_MACH_COST where trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + hf3.Value + "'";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        db0 = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; db7 = 0; db8 = 0; db9 = 0; db10 = 0; db11 = 0; db12 = 0; db13 = 0; db14 = 0; db15 = 0; db16 = 0; db17 = 0; db18 = 0; db19 = 0; db20 = 0;
        if (dt.Rows.Count > 0)
        {
            db0 = fgen.make_double(dt.Rows[0]["mch_cost"].ToString().Trim());//machine cost=======b87
            db1 = fgen.make_double(dt.Rows[0]["mch_rt_phr"].ToString().Trim()); //machine rate per hour======b88                       
            db3 = fgen.make_double(dt.Rows[0]["oper_sal"].ToString().Trim());//operator salary=========b90
            db4 = fgen.make_double(dt.Rows[0]["oper_sal_ph"].ToString().Trim());//operator salry per hour=====b91
            db14 = fgen.make_double(dt.Rows[0]["WRK_HR_PDAY"].ToString());
            db15 = fgen.make_double(dt.Rows[0]["DAY_WRK_PM"].ToString());
            db16 = db3 / (db14 * db15);
            db5 = fgen.make_double(dt.Rows[0]["no_imp_pmnt"].ToString().Trim());//no of impression p/mnt========b92
            db6 = fgen.make_double(dt.Rows[0]["mx_rmtr_phr"].ToString().Trim()); //max running mtr in 1 hr=========b93
            ////time for the job formula========txttot_rung_mtr/db6
            db7 = fgen.make_double(txttot_rung_mtr.Text) / db6;///////time for the job=========b94                       
            db8 = fgen.make_double(dt.Rows[0]["set_time"].ToString().Trim());///setting time==========b95
            //  db9 = fgen.make_double(dt.Rows[0]["tot_time_job"].ToString().Trim());//total time for the job==========b96
            db9 = db7 + db8;//total time for the job.......running time===========b96
            db10 = fgen.make_double(dt.Rows[0]["tot_ele_use"].ToString().Trim());//Total Electricity usage ========b97
            db11 = fgen.make_double(dt.Rows[0]["elce_chg_phr"].ToString().Trim());//Electricity charge for 1 hr =========b98
            db17 = db10 / (db15 * db14);
            db12 = db17 * (db9 / 60);//Total Electricity charge for the job=======b99
            //db2 = fgen.make_double(dt.Rows[0]["mch_cost1"].ToString().Trim());//machine cost for the job=====b89
            db2 = ((db9 / 60) * db1);//machine cost for the job===========b89
            db13 = Math.Round((db12 + db16 + db2) * fgen.make_double(txtpass.Text.Trim()), 2);
            ////////
            txtmch1_code.Text = dt.Rows[0]["mchcode"].ToString().Trim();//HIDDEN ON FORM BUT CODE IS SAVED IN THIS
            txtmchname1.Text = dt.Rows[0]["mchname"].ToString().Trim();
            txtmch1_cost.Text = Convert.ToString(db13);
            txtJobTime1.Text = db7.ToString();
            txtElectricityChg1.Text = (Math.Round(db12, 2)).ToString();
        }
        #endregion
    }
    //------------------------------------------------------------------------------------
    void Cal()
    {
        return;
        double lbl_hyt = 0; double lbl_width = 0; double acros = 0; double around = 0; double gap_acros = 0; double gap_Around = 0; double req_width = 0; double tot_wstg = 0;
        double t1 = 0; double t2 = 0; double t3 = 0; double t4 = 0; double t5 = 0; double t6 = 0; double t7 = 0; double papaersize = 0; double qty = 0; double rung_mtr_mm = 0; double setting_wstg_colr = 0;
        double paper = 0; double width = 0; double height = 0; double wstg = 0; double gap = 0; double ups = 0; double diff = 0; double rung_mtr_mtr = 0; double color = 0; double tot_rmtr_used = 0;
        double tot_sqm = 0; double unit_prc_of_matl = 0; double tot_unit_cost = 0; double usage_ink_sqm_color = 0; double tot_ink_usage = 0; double gap_arnd = 0;
        double plate_unit_cost = 0; double tot_plate_cost = 0; double varnish = 0; double tot_varnish_cost = 0; double tot_varnish_cost1 = 0; double die_width = 0; double die_hyt = 0;
        double die_area = 0; double die_unit_rate = 0; double no_of_die_reqd = 0; double die_cost = 0; double emb_var = 0; double unit_consum_var = 0; double tot_emb_var = 0;
        double screen_emb_chg = 0; double emb_var1 = 0; double tot_rt_emb_var = 0; double area_embosing_white = 0; double unit_conum_white_var = 0; double emb_Var_conum_white = 0;
        double mch1_cost = 0; double tot_rt_For_emb_varnish = 0; double screen_print = 0; double prod_cost = 0; double margin_considered = 0; double margin_cost_AED = 0; double total = 0;
        double vat_percent = 0; double val_Value = 0; double gd_tot = 0; double v1 = 0; double v2 = 0; double v3 = 0; double del_desp = 0;
        #region
        //FOR REQ WIDTH formula================ //txtreqwidth = (txtlbl_hyt * txt_acros) + 20 + (txt_gap_acros * (txt_acros - 1))           
        lbl_hyt = fgen.make_double(txtlbl_hyt.Text.Trim());
        acros = fgen.make_double(txt_acros.Text.Trim());
        gap_acros = fgen.make_double(txt_gap_acros.Text.Trim());
        req_width = (lbl_hyt * acros) + 20 + (gap_acros * (acros - 1));
        txtreqwidth.Text = Convert.ToString(Math.Round(req_width, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ////=======for ups====sppi=====formula is======txtups = txt_acros X txtaround
        around = fgen.make_double(txtaround.Text.Trim());
        ups = (acros * 1) * (around * 1);
        txtups.Text = Convert.ToString(Math.Round(ups, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ////=====  for Diff for SPPI=======txtdiff = txttrmwastg - txtreqwidth
        wstg = fgen.make_double(txttrmwastg.Text.Trim());
        diff = (wstg - req_width);
        txtdiff.Text = Convert.ToString(Math.Round(diff, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        //===========formula for Running Mtr (MM)=========== txtrungmtr_mm  = (txtlbl_width +3) x (txtqty / txt_acros)
        lbl_width = fgen.make_double(txtlbl_width.Text.Trim());
        qty = fgen.make_double(txtqty.Text.Trim());
        rung_mtr_mm = (lbl_width + 3) * (qty / acros);
        txtrungmtr_mm.Text = Convert.ToString(Math.Round(rung_mtr_mm, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ///==========Formula for Running Mtr mtr=======txtrungmtr_mtr = txtrungmtr_mm/1000
        rung_mtr_mtr = rung_mtr_mm / 1000;
        txtrungmtr_mtr.Text = Convert.ToString(Math.Round(rung_mtr_mtr, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        //============fromula for total wastage===txt_tot_wstg =  txtcolor x txtsetting_wstg_pclor
        color = fgen.make_double(txtcolor.Text.Trim());
        setting_wstg_colr = fgen.make_double(txtsetting_wstg_pclor.Text.Trim());
        tot_wstg = (color * setting_wstg_colr);
        txt_tot_wstg.Text = Convert.ToString(Math.Round(tot_wstg, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        //===========  TOTAL RUNNING METER USED FORMULA===========  txttot_rung_mtr = txtrungmtr_mtr + txt_tot_wstg
        tot_rmtr_used = (rung_mtr_mtr + tot_wstg);
        txttot_rung_mtr.Text = Convert.ToString(Math.Round(tot_rmtr_used, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        //=============tot sq meter=======txt_tot_sqm = txttot_rung_mtr x (txttrmwastg/1000)
        tot_sqm = (tot_rmtr_used * 1) * ((wstg * 1) / 1000);
        txt_tot_sqm.Text = Convert.ToString(Math.Round(tot_sqm, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ///   //=============UNIT PRICE OF MATERIAL========  txt_unit_price_matl  =   txtmatl1_val  + txtmatl2_val  +  txtmatl3_val + txtmatl4_val
        t1 = fgen.make_double(txtmatl1_val.Text.Trim());
        t2 = fgen.make_double(txtmatl2_val.Text.Trim());
        t3 = fgen.make_double(txtmatl3_val.Text.Trim());
        t4 = fgen.make_double(txtmatl4_val.Text.Trim());
        unit_prc_of_matl = (t1 + t2 + t3 + t4);
        txt_unit_price_matl.Text = Convert.ToString(Math.Round(unit_prc_of_matl, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ///////=================total cost for the material=======txtcost_matl =  txt_tot_sqm x txt_unit_price_matl  
        tot_unit_cost = (tot_sqm * 1) * (unit_prc_of_matl * 1);
        txtcost_matl.Text = Convert.ToString(Math.Round(tot_unit_cost, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ///=================for Ink details ====total ink usage sq meter=========txt_Tot_ink_usage= txtusage_ink   x   txtcolor
        usage_ink_sqm_color = fgen.make_double(txtusage_ink.Text.Trim());
        tot_ink_usage = (usage_ink_sqm_color * 1) * (color * 1);
        txt_Tot_ink_usage.Text = Convert.ToString(Math.Round(tot_ink_usage, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        //=============  total ink cost formula=====txt_tot_ink_cost =  txtinkval  x  txt_Tot_ink_usage x txt_tot_sqm x txtpass
        t5 = fgen.make_double(txtinkval.Text.Trim());
        t6 = fgen.make_double(txtpass.Text.Trim());
        t7 = (t5 * 1) * (tot_ink_usage * 1) * (tot_sqm * 1) * (t6 * 1);
        txt_tot_ink_cost.Text = Convert.ToString(Math.Round(t7, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ////total plate cost formula======txt_tot_plate_cost = txtplate_unit_cost x txtcolor
        plate_unit_cost = fgen.make_double(txtplate_unit_cost.Text.Trim());
        tot_plate_cost = (plate_unit_cost * 1) * (color * 1);
        txt_tot_plate_cost.Text = Convert.ToString(Math.Round(tot_plate_cost, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        //varnish formula=====txt_tot_varnish_cost  = txt_varnish_usage x (txt_varnish_cost / 1000 ) x txt_tot_sqm           
        varnish = fgen.make_double(txt_varnish_usage.Text.Trim());
        tot_varnish_cost = fgen.make_double(txt_varnish_cost.Text.Trim());
        tot_varnish_cost1 = varnish * (tot_varnish_cost / 1000) * tot_sqm;
        txt_tot_varnish_cost.Text = Convert.ToString(Math.Round(tot_varnish_cost1, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ///die details -------------DIE WIDTH FORMULA---------txtdie_width  =  ((txtlbl_width + txtgaparound ) x txtaround) + txtgaparound 
        gap_arnd = fgen.make_double(txtgaparound.Text.Trim());
        die_width = (((lbl_width * 1) + (gap_arnd * 1)) * (around * 1)) + (gap_arnd * 1);
        txtdie_width.Text = Convert.ToString(Math.Round(die_width, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ///die details -------------DIE hight FORMULA---------txt_die_hight = (( txtlbl_hyt  + txt_gap_acros) * txt_acros) + txt_gap_acros
        die_hyt = (((lbl_hyt * 1) + (gap_acros * 1)) * (acros * 1)) + (gap_acros * 1);
        txt_die_hight.Text = Convert.ToString(Math.Round(die_hyt, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        //// area of die  =============(die width * diw hyt )/100===========  
        die_area = (die_width * die_hyt) / 100;
        txtdie_area.Text = Convert.ToString(Math.Round(die_area, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        #endregion
        ///total die cost formula=====txtdiecost  =  area of die x unit rate of die x no of die reqd 
        die_unit_rate = fgen.make_double(txtdierate.Text.Trim());
        no_of_die_reqd = fgen.make_double(txtdie_reqd.Text.Trim());
        die_cost = die_area * die_unit_rate * no_of_die_reqd;
        txtdiecost.Text = Convert.ToString(Math.Round(die_cost, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ///======================EMBOSSING VARNISH====txt_tot_embas_varnish_Val = txt_tot_sqm X (txtemb_area_varnish/100) X txt_consum_varnish         
        emb_var = fgen.make_double(txtemb_area_varnish.Text.Trim());
        unit_consum_var = fgen.make_double(txt_consum_varnish.Text.Trim());
        tot_emb_var = tot_sqm * (emb_var / 100) * unit_consum_var;
        txt_tot_embas_varnish_Val.Text = Convert.ToString(Math.Round(tot_emb_var, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        //total rate for embossing varnish==== txt_totrate_emb_varnish = (txt_tot_embas_varnish_Val /1000) x txtembvarnish_val + txt_screen_exposing_chg        
        screen_emb_chg = fgen.make_double(txt_screen_exposing_chg.Text.Trim());
        emb_var1 = fgen.make_double(txtembvarnish_val.Text.Trim());
        tot_rt_emb_var = (tot_emb_var / 1000) * emb_var1 + screen_emb_chg;
        txt_totrate_emb_varnish.Text = Convert.ToString(Math.Round(tot_rt_emb_var, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        txttot_rt_For_emb_varnish.Text = Convert.ToString(Math.Round(tot_rt_emb_var, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        //================embossing varnish white/screen printing
        ///==========total consumtion of embossing ====== txtemb_Var_conum_white = txtinkval x (txtarea_embosing_white/100) x txtunit_conum_white_var
        area_embosing_white = fgen.make_double(txtarea_embosing_white.Text.Trim());
        unit_conum_white_var = fgen.make_double(txtunit_conum_white_var.Text.Trim());
        emb_Var_conum_white = (t5 * 1) * ((area_embosing_white * 1) / 100) * (unit_conum_white_var * 1);
        txtemb_Var_conum_white.Text = Convert.ToString(Math.Round(emb_Var_conum_white, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ///=-----tot rate embosing white-------------txt_totrate_emb_white===txtemb_Var_conum_white x txtembossing_var_rate x txtscreen_exposing
        v1 = fgen.make_double(txtembossing_var_rate.Text.Trim());
        v2 = fgen.make_double(txtscreen_exposing.Text.Trim());
        v3 = emb_Var_conum_white * v1 + v2;
        txt_totrate_emb_white.Text = Convert.ToString(Math.Round(v3, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        txt_screen_print.Text = Convert.ToString(Math.Round(v3, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ///totAL PRODUCTION COST ==== txtprod_cost = txtcost_matl + txt_tot_ink_cost + txt_tot_plate_cost + txt_tot_varnish_cost + txtdiecost + txtmch1_cost + txttot_rt_For_emb_varnish + txt_screen_print
        mch1_cost = fgen.make_double(txtmch1_cost.Text.Trim());
        tot_rt_For_emb_varnish = fgen.make_double(txttot_rt_For_emb_varnish.Text.Trim());
        screen_print = fgen.make_double(txt_screen_print.Text.Trim());
        //  prod_cost = Math.Round(tot_unit_cost, 4) + Math.Round(t7, 3) + Math.Round(tot_plate_cost, 3) + Math.Round(tot_varnish_cost1, 3) + Math.Round(die_cost, 3) + Math.Round(mch1_cost, 3) + Math.Round(tot_rt_For_emb_varnish, 3) + Math.Round(screen_print, 3);
        prod_cost = tot_unit_cost + t7 + tot_plate_cost + tot_varnish_cost1 + die_cost + mch1_cost + tot_rt_For_emb_varnish + screen_print;
        txtprod_cost.Text = Math.Round(prod_cost, 3).ToString().Replace("Infinity", "0").Replace("NaN", "0");
        //margin considered (B) =====formula ===== prod_Cost x (margin cost /100)
        margin_considered = fgen.make_double(txt_margin_considered.Text.Trim());
        margin_cost_AED = (prod_cost * 1) * ((margin_considered * 1) / 100);
        txtmargin_cost_AED.Text = Convert.ToString(Math.Round(margin_cost_AED, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ////formula for TOTal=== txttotal  = prod_cost + margin_cost_AED
        total = prod_cost + margin_cost_AED;
        txttotal.Text = Convert.ToString(Math.Round(total, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ///vat value=====  vat vale === total x (vat %/100) 
        vat_percent = fgen.make_double(txtvat_percent.Text.Trim());
        val_Value = total * (vat_percent / 100);
        txtval_Value.Text = Convert.ToString(Math.Round(val_Value, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        ///formula for grand total  -==== txt_grand_tot = total + val value
        del_desp = fgen.make_double(txt_del_desp.Text.Trim());
        //gd_tot = total + val_Value;//oldformula
        gd_tot = total + val_Value + del_desp; //add delivery and desp amt...20.04.2020
        txt_grand_tot.Text = Convert.ToString(Math.Round(gd_tot, 3)).Replace("Infinity", "0").Replace("NaN", "0");
        //=========
        cal1();
        cal2();
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
        fgen.Fn_open_sseek("Select Item", frm_qstr);
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
    protected void btnmatl1_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "MATL1";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Material Type", frm_qstr);
    }
    protected void btnmatl2_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "MATL2";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Lamination Type", frm_qstr);
    }
    protected void btnmatl3_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "MATL3";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Foil Type", frm_qstr);
    }
    protected void btnmatl4_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "MATL4";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Material Type", frm_qstr);
    }
    protected void btnink_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "INK";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Ink Type", frm_qstr);
    }
    protected void btnplate_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "PLATE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Plate Unit Type", frm_qstr);
    }
    protected void btnvarnish1_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "VARNISH";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Varnish Type", frm_qstr);
    }
    protected void btndie_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "DIE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Die Unit Type", frm_qstr);
    }
    protected void btnemb_varnish_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "EMBOSSING_VARNISH";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Embossing Varnish Unit Type", frm_qstr);
    }

    protected void btnembossing_Var_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "EMBOSSING_WHITE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Embossing White Unit Type", frm_qstr);
    }
    protected void btnmch1_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "MACHINE1";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Printing Machine", frm_qstr);
    }
    protected void btnmch2_Click(object sender, ImageClickEventArgs e)
    {
        Cal();
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "MACHINE2";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Slitting Machine", frm_qstr);
    }
    protected void btnCyl_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CYLINDER";
        if (txtlbl_width.Text == "" || txtlbl_width.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please enter width before selection");
            return;
        }
        else
        {
            cyl_cal();
            //cyl_calWithAI();
        }
    }

    void cyl_calWithAI()
    {
        SQuery = "select distinct trim(branchcd)||trim(id)||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr, name,num1 as height,num2 as width,num3 as teeth,srno from wb_master where branchcd='" + frm_mbr + "' and id='AR01' order by num1";
        dt = new DataTable(); dt2 = new DataTable();
        dt2.Columns.Add("around", typeof(double));
        dt2.Columns.Add("height", typeof(double));
        dt2.Columns.Add("width", typeof(double));
        dt2.Columns.Add("gap", typeof(double));
        dt2.Columns.Add("teeth", typeof(double));
        dt2.Columns.Add("diff", typeof(double));
        dt2.Columns.Add("cal", typeof(double));
        dt2.Columns.Add("lbl_widht", typeof(double));
        oporow = dt2.NewRow();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//around master dt        
        double gft1 = 0; double gft2 = 0, gft3 = 0;
        double wieght = 0; double minrng = 0; double maxrng = 0;
        double t1 = 0;
        wieght = fgen.make_double(txtwidth.Text.Trim());
        minrng = fgen.make_double(txtMIN.Text.Trim());
        maxrng = fgen.make_double(txtMAX.Text.Trim());
        int[] myarr = new int[1] { 0 };
        if (hffield.Value == "CYLINDER_M")
            myarr = new int[8] { -8, -6, -4, -2, 2, 4, 6, 8 };

        double width = txtlbl_width.Text.toDouble();
        for (int k = 0; k < myarr.Length; k++)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                #region
                gft1 = Math.Round(fgen.make_double(dt.Rows[i]["height"].ToString().Trim()), 4);
                gft2 = Math.Round(fgen.make_double(dt.Rows[i]["width"].ToString().Trim()), 4);
                gft3 = Math.Round(fgen.make_double(dt.Rows[i]["teeth"].ToString().Trim()), 4);
                width = txtlbl_width.Text.toDouble();

                width = width - myarr[k].ToString().toDouble();

                t1 = width * txtaround.Text.toDouble();

                oporow = dt2.NewRow();
                oporow["height"] = gft1;
                oporow["width"] = gft2;
                oporow["teeth"] = gft3;
                if (t1 > 0 && width > 0)
                    oporow["gap"] = Math.Round(gft2 / width, 2);

                oporow["diff"] = Math.Round(gft2 - t1, 2);
                oporow["cal"] = t1;
                oporow["lbl_widht"] = width;
                dt2.Rows.Add(oporow);
                #endregion
            }
        }
        mq1 = ""; SQuery = "";
        col1 = "";
        mq0 = "";
        if (hffield.Value != "CYLINDER_M")
            mq0 = ",'" + col1 + "' as suggested";
        int x = 0;
        for (int i = 0; i < dt2.Rows.Count; i++)
        {
            if (dt2.Rows[i]["diff"].ToString().toDouble() >= 0 && dt2.Rows[i]["gap"].ToString().toDouble() >= 3 && dt2.Rows[i]["gap"].ToString().toDouble() <= 7)
            {
                col1 = "";
                if (x == 0) col1 = "Best Suited Size";
                if (mq1 == "")
                {
                    //mq1 += "select '-' as fstr, '" + dt2.Rows[i]["around"].ToString().Trim() + "' as around,'" + minrng + "' as minrange,'" + maxrng + "' as maxrange from dual";
                    mq1 += "select '" + dt2.Rows[i]["around"].ToString().Trim() + "' as fstr, '" + dt2.Rows[i]["height"].ToString().Trim() + "' as Cylinder_inch,'" + dt2.Rows[i]["width"].ToString() + "' as cylinder_mm,'" + dt2.Rows[i]["gap"].ToString().Trim() + "' as Around_gap,'" + dt2.Rows[i]["teeth"].ToString().Trim() + "' as teeth,'" + dt2.Rows[i]["lbl_widht"].ToString() + "' as label_width" + mq0 + " from dual";
                }
                else
                {
                    //  mq1 += "select '-' as fstr,  '" + dt2.Rows[i]["around"].ToString().Trim() + "' as around,'" + minrng + "' as minrange,'" + maxrng + "' as maxrange from dual  union all  ";
                    mq1 += " union all select '" + dt2.Rows[i]["around"].ToString().Trim() + "'  as fstr,'" + dt2.Rows[i]["height"].ToString().Trim() + "' as Cylinder_inch,'" + dt2.Rows[i]["width"].ToString() + "' as cylinder_mm,'" + dt2.Rows[i]["gap"].ToString().Trim() + "' as Around_gap,'" + dt2.Rows[i]["teeth"].ToString().Trim() + "' as teeth,'" + dt2.Rows[i]["lbl_widht"].ToString() + "' as label_width" + mq0 + " from dual";
                }
                x++;
            }
        }
        if (mq1 != "")
        {
            hffield.Value = "CYLINDER";
            SQuery = "select * from (" + mq1 + ") order by to_number(around_gap),to_number(cylinder_inch) ";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);

            fgen.Fn_open_sseek("Selecting Matching Cylinder from Inv. (Esc Show All)", frm_qstr);
        }
        else
        {
            hffield.Value = "CYLINDER_M";
            if (fgenMV.Fn_Get_Mvar(frm_qstr, "CALC") == "Y")
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "CALC", "N");
                fgen.msg("-", "AMSG", "No best suited cylinder matched!!");
            }
            else
            {
                hffield.Value = "CYLINDER_M";
                fgen.msg("-", "CMSG", "No best suited cylinder matched!!'13'If you want, system to calculate best match click on yes.");
            }
        }
    }
    void cyl_cal()
    {
        SQuery = "select distinct trim(branchcd)||trim(id)||vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr, name,num1 as height,num2 as width,num3 as teeth,srno from wb_master where branchcd='" + frm_mbr + "' and id='AR01' order by num1";
        dt = new DataTable(); dt2 = new DataTable();
        dt2.Columns.Add("around", typeof(double));
        dt2.Columns.Add("height", typeof(double));
        dt2.Columns.Add("width", typeof(double));
        dt2.Columns.Add("gap", typeof(double));
        dt2.Columns.Add("teeth", typeof(double));
        dt2.Columns.Add("diff", typeof(double));
        dt2.Columns.Add("cal", typeof(double));
        dt2.Columns.Add("lbl_widht", typeof(double));
        oporow = dt2.NewRow();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//around master dt        
        double gft1 = 0; double gft2 = 0, gft3 = 0;
        double wieght = 0; double minrng = 0; double maxrng = 0;
        double t1 = 0;
        wieght = fgen.make_double(txtwidth.Text.Trim());
        minrng = fgen.make_double(txtMIN.Text.Trim());
        maxrng = fgen.make_double(txtMAX.Text.Trim());
        int[] myarr = new int[1] { 0 };
        if (hffield.Value == "CYLINDER_M")
            myarr = new int[9] { 0, -8, -6, -4, -2, 2, 4, 6, 8 };
        double width = txtlbl_width.Text.toDouble();
        for (int k = 0; k < myarr.Length; k++)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                #region
                gft1 = Math.Round(fgen.make_double(dt.Rows[i]["height"].ToString().Trim()), 4);
                gft2 = Math.Round(fgen.make_double(dt.Rows[i]["width"].ToString().Trim()), 4);
                gft3 = Math.Round(fgen.make_double(dt.Rows[i]["teeth"].ToString().Trim()), 4);
                width = txtlbl_width.Text.toDouble();

                width = width - myarr[k].ToString().toDouble();

                t1 = width * txtaround.Text.toDouble();

                oporow = dt2.NewRow();
                oporow["height"] = gft1;
                oporow["width"] = gft2;
                oporow["teeth"] = gft3;
                if (t1 > 0 && width > 0)
                    oporow["gap"] = Math.Round(gft2 / width, 2);

                oporow["diff"] = Math.Round(gft2 - t1, 2);
                oporow["cal"] = t1;
                oporow["lbl_widht"] = width;
                dt2.Rows.Add(oporow);
                #endregion
            }
        }

        mq1 = ""; SQuery = "";
        col1 = "";
        mq0 = "";
        int x = 0;
        for (int i = 0; i < dt2.Rows.Count; i++)
        {
            if (dt2.Rows[i]["diff"].ToString().toDouble() >= 0)
            {
                col1 = "";
                if (x == 0) col1 = "Best Suited Size";
                if (mq1 == "")
                {
                    //mq1 += "select '-' as fstr, '" + dt2.Rows[i]["around"].ToString().Trim() + "' as around,'" + minrng + "' as minrange,'" + maxrng + "' as maxrange from dual";
                    mq1 += "select '" + dt2.Rows[i]["around"].ToString().Trim() + "' as fstr, '" + dt2.Rows[i]["height"].ToString().Trim() + "' as Cylinder_inch,'" + dt2.Rows[i]["width"].ToString() + "' as cylinder_mm,'" + dt2.Rows[i]["gap"].ToString().Trim() + "' as Around_gap,'" + dt2.Rows[i]["teeth"].ToString().Trim() + "' as teeth,'" + dt2.Rows[i]["lbl_widht"].ToString() + "' as label_width" + mq0 + ",'" + col1 + "' as rmk from dual";
                }
                else
                {
                    //  mq1 += "select '-' as fstr,  '" + dt2.Rows[i]["around"].ToString().Trim() + "' as around,'" + minrng + "' as minrange,'" + maxrng + "' as maxrange from dual  union all  ";
                    mq1 += " union all select '" + dt2.Rows[i]["around"].ToString().Trim() + "'  as fstr,'" + dt2.Rows[i]["height"].ToString().Trim() + "' as Cylinder_inch,'" + dt2.Rows[i]["width"].ToString() + "' as cylinder_mm,'" + dt2.Rows[i]["gap"].ToString().Trim() + "' as Around_gap,'" + dt2.Rows[i]["teeth"].ToString().Trim() + "' as teeth,'" + dt2.Rows[i]["lbl_widht"].ToString() + "' as label_width" + mq0 + ",'" + col1 + "' as rmk from dual";
                }
                x++;
            }
            else
            {
                col1 = "";
                if (mq1 == "")
                {
                    //mq1 += "select '-' as fstr, '" + dt2.Rows[i]["around"].ToString().Trim() + "' as around,'" + minrng + "' as minrange,'" + maxrng + "' as maxrange from dual";
                    mq1 += "select '" + dt2.Rows[i]["around"].ToString().Trim() + "' as fstr, '" + dt2.Rows[i]["height"].ToString().Trim() + "' as Cylinder_inch,'" + dt2.Rows[i]["width"].ToString() + "' as cylinder_mm,'" + dt2.Rows[i]["gap"].ToString().Trim() + "' as Around_gap,'" + dt2.Rows[i]["teeth"].ToString().Trim() + "' as teeth,'" + dt2.Rows[i]["lbl_widht"].ToString() + "' as label_width" + mq0 + ",'" + col1 + "' as rmk from dual";
                }
                else
                {
                    //  mq1 += "select '-' as fstr,  '" + dt2.Rows[i]["around"].ToString().Trim() + "' as around,'" + minrng + "' as minrange,'" + maxrng + "' as maxrange from dual  union all  ";
                    mq1 += " union all select '" + dt2.Rows[i]["around"].ToString().Trim() + "'  as fstr,'" + dt2.Rows[i]["height"].ToString().Trim() + "' as Cylinder_inch,'" + dt2.Rows[i]["width"].ToString() + "' as cylinder_mm,'" + dt2.Rows[i]["gap"].ToString().Trim() + "' as Around_gap,'" + dt2.Rows[i]["teeth"].ToString().Trim() + "' as teeth,'" + dt2.Rows[i]["lbl_widht"].ToString() + "' as label_width" + mq0 + ",'" + col1 + "' as rmk from dual";
                }
                x++;
            }
        }
        if (1 == 2)
        {
            if (mq1 != "")
            {
                hffield.Value = "CYLINDER";
                SQuery = "select * from (" + mq1 + ") order by to_number(around_gap),to_number(cylinder_inch) ";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);

                fgen.Fn_open_sseek("Selecting Matching Cylinder from Inventory (Esc Show All)", frm_qstr);
            }
        }
        SQuery = "select * from (Select (num2*" + txtlbl_width.Text.ToString().toDouble() + ") as fstr,num1 as Cylinder_Inch,num2 as Cyliner_mm,round((num2-(" + (txtlbl_width.Text.ToString().toDouble() * txtaround.Text.ToString().toDouble()) + ")) /" + txtaround.Text.ToString().toDouble() + ",2) as around_gap,num3 as Cylinder_Teeth, Name,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,to_char(EDT_DT,'dd/MM/yyyy') as edt_dt from wb_master where BRANCHCD!='DD' AND id='AR01' AND NUM2>" + (txtlbl_width.Text.ToString().toDouble() * txtaround.Text.ToString().toDouble()) + " and round((num2-(" + (txtlbl_width.Text.ToString().toDouble() * txtaround.Text.ToString().toDouble()) + ")) /" + txtaround.Text.ToString().toDouble() + ",2)>=3 ) order by around_gap,Cyliner_mm ";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);

        fgen.Fn_open_sseek("Selecting Matching Cylinder from Inv. (Esc Show All)", frm_qstr);
    }
    protected void btnCylView_ServerClick(object sender, EventArgs e)
    {
        SQuery = "Select Name,num1 as Cylinder_Inch,num2 as Cyliner_mm,num3 as Cylinder_Teeth,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,to_char(EDT_DT,'dd/MM/yyyy') as edt_dt from wb_master where BRANCHCD!='DD' AND id='AR01' order by type1 ";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("List of cylinder invenotry", frm_qstr);
    }
}