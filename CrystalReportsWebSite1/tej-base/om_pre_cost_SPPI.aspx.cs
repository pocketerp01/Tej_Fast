using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;


public partial class om_pre_cost_SPPI : System.Web.UI.Page
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
            //setColHeadings();
            set_Val();
            string home_curr = fgen.seek_iname(frm_qstr, frm_cocd, "select br_curren from type where id='B' and type1='" + frm_mbr + "'", "");
            Label21.InnerText = "Extrusion (" + home_curr + "/HR)";
            Label13.InnerText = "Printing-Roto (" + home_curr + " /HR)";
            Label17.InnerText = "Printing-BOBST (" + home_curr + "/HR)";
            Label24.InnerText = "Printing-CI (" + home_curr + "/HR)";
            Label27.InnerText = "RM Unit Price (" + home_curr + ")";
            Label32.InnerText = "Cost/Kg(" + home_curr + ")";
            Label29.InnerText = "Lamination (" + home_curr + "/HR)";
            Label31.InnerText = "Slitting (" + home_curr + "/HR)";
            Label48.InnerText = "Pouching (" + home_curr + "/HR)";
            Label50.InnerText = "Bag-Chicken (" + home_curr + "/HR)";
            Label55.InnerText = "Total_Cost(In " + home_curr + ")";
            Label51.InnerText = "Fuel (" + home_curr + "/HRS)";
            Label74.InnerText = "Extrusion (" + home_curr + "/HR)";
            Label75.InnerText = "Per_Pc_Price(" + home_curr + ")";
            Label91.InnerText = "Printing-Roto (" + home_curr + "/HR)";
            Label93.InnerText = "Printing-BOBST (" + home_curr + "/HR)";
            Label95.InnerText = "Printing-CI (" + home_curr + "/HR)";
            Label100.InnerText = "Lamination (" + home_curr + "/HR)";
            Label105.InnerText = "Slitting (" + home_curr + "/HR)";
            Label106.InnerText = "Pouching (" + home_curr + "/HR)";
            Label108.InnerText = "Bag-Chicken (" + home_curr + "/HR)";
            Label69.InnerText = "Total Cost (In " + home_curr + ")";
            Label107.InnerText = "Labour Cost (" + home_curr + "/Kg)";
            Label115.InnerText = "Selling_Price(Per Kg) " + home_curr + "";
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
                //for (int i = 0; i < 10; i++)
                //{
                //    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                //    sg1.Rows[K].Cells[i].CssClass = "hidden";
                //}
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
        btnrefresh.Disabled = true;
        btnicode.Enabled = false; btnparty.Enabled = false;
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
        btnicode.Enabled = true; // btncylinder.Enabled = true;
        btnprint.Disabled = true; btnlist.Disabled = true; btnrefresh.Disabled = false;
        btnparty.Enabled = true;
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
        frm_tabname = "WB_PRECOST";
        lblheader.Text = "Detailed Flexible Costing";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "PC");
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

            case "CUST":
                SQuery = "SELECT TRIM(ACODE) AS FSTR,TRIM(ACODE) AS CUSTOMER_CODE,TRIM(ANAME) AS CUSTOMER FROM FAMST WHERE SUBSTR(TRIM(ACODE),1,2)='16' ORDER BY CUSTOMER";
                break;

            case "ITEM":
                SQuery = "SELECT TRIM(ICODE) AS FSTR,TRIM(ICODE) AS JOB_CODE,TRIM(INAME) AS JOB_NAME FROM ITEM WHERE SUBSTR(TRIM(ICODE),1,1) IN ('7','9') AND LENGTH(TRIM(ICODE))>=8 ORDER BY JOB_NAME";
                break;
            case "I":
                SQuery = "SELECT TRIM(ICODE) AS FSTR,TRIM(INAME) AS iname,TRIM(ICODE) AS icode,unit FROM ITEM WHERE SUBSTR(TRIM(ICODE),1,1) <'7' AND LENGTH(TRIM(ICODE))>=8 ORDER BY iname  ";
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
            case "EX1":
            case "P1":
            case "P2":
            case "P3":
            case "LA":
            case "PO":
            case "SL":
            case "BA":
            case "CK":
                SQuery = "SELECT tot_mcost AS FSTR,MCHNAME AS MACHINE_NAME,MCHCODE,tot_mcost AS MCHN_COST,VCHNUM FROM WB_MACH_COST WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='^G' ORDER BY VCHNUM ";
                break;
            case "Print_E":
                SQuery = "select distinct trim(a.branchcd)||trim(a.type)||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.type,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.type,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
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

            makeMyStyleGrid();
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    void makeMyStyleGrid()
    {
        create_tab();
        //sg1_dr = sg1_dt.NewRow();
        //sg1_dr["sg1_h1"] = "H";
        //sg1_dr["sg1_t1"] = "ICODE";
        //sg1_dr["sg1_t2"] = "Raw Materials";
        //sg1_dr["sg1_t3"] = "Thickness";
        //sg1_dr["sg1_t4"] = "Density";
        //sg1_dr["sg1_t5"] = "GSM";
        //sg1_dr["sg1_t6"] = "RM Mixed";
        //sg1_dr["sg1_t7"] = "RM Price(USD)";
        //sg1_dr["sg1_t8"] = "RM Price(AED)";
        //sg1_dr["sg1_t9"] = "Cost/Kg(USD)";
        //sg1_dr["sg1_t10"] = "Cost/Kg(AED)";
        //sg1_dt.Rows.Add(sg1_dr);

        for (int i = 0; i < 6; i++)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_h1"] = "T1";
            if (i > 2)
                sg1_dr["sg1_h1"] = "T2";
            col1 = "";
            if (i == 0) col1 = "PET";
            if (i == 1) col1 = "MET PET";
            if (i == 2) col1 = "LDPE TRANSPARENT";
            if (i == 3) col1 = "Ink";
            if (i == 4) col1 = "Adhesive 1 (S.L)";
            if (i == 5) col1 = "Adhesive 1 (S.L)";
            sg1_dr["sg1_h2"] = col1;
            sg1_dt.Rows.Add(sg1_dr);
        }

        sg1_dr = sg1_dt.NewRow();
        sg1_dr["sg1_h1"] = "C";
        sg1_dr["sg1_h2"] = "Total";
        sg1_dt.Rows.Add(sg1_dr);

        for (int i = 0; i < 7; i++)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_h1"] = "T4";
            if (i == 0)
                sg1_dr["sg1_h1"] = "T3";
            if (i > 4)
                sg1_dr["sg1_h1"] = "T5";
            col1 = "";
            if (i == 0) col1 = "Wastage";
            if (i == 1) col1 = "Solvent";
            if (i == 2) col1 = "Zipper";
            if (i == 3) col1 = "Packing-glue";
            if (i == 4) col1 = "Packing-pet strip";
            if (i == 5) col1 = "Packing-ctn";
            if (i == 6) col1 = "Packing-bobbin & others";
            sg1_dr["sg1_h2"] = col1;
            sg1_dt.Rows.Add(sg1_dr);
        }
        sg1_dr = sg1_dt.NewRow();
        sg1_dr["sg1_h1"] = "C2";
        sg1_dr["sg1_h2"] = "Total RM Cost / kg";
        sg1_dt.Rows.Add(sg1_dr);

        sg1.DataSource = sg1_dt;
        sg1.DataBind();

        ViewState["sg1"] = sg1_dt;
        setHead();
    }
    void setHead()
    {
        sg1.HeaderRow.Cells[1].Text = "Heading";
        sg1.HeaderRow.Cells[10].Text = "ICODE";
        sg1.HeaderRow.Cells[11].Text = "Raw Materials";
        sg1.HeaderRow.Cells[12].Text = "Thickness";
        sg1.HeaderRow.Cells[13].Text = "Density";
        sg1.HeaderRow.Cells[14].Text = "GSM";
        sg1.HeaderRow.Cells[15].Text = "RM Mixed";
        sg1.HeaderRow.Cells[16].Text = "RM Price(USD)";
        sg1.HeaderRow.Cells[17].Text = "RM Price(AED)";
        sg1.HeaderRow.Cells[18].Text = "Cost/Kg(USD)";
        sg1.HeaderRow.Cells[19].Text = "Cost/Kg(AED)";
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
        fgen.fill_zero(this.Controls);
        //Cal();
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        if (txtaname.Text.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Customer");
            return;
        }
        if (txtiname.Text.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Item");
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
        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        ViewState["sg1"] = null;
        //setColHeadings();
        setHead();
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
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        vty = "PC";
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        sg1_dt = new DataTable();
        create_tab();
        sg1_dr = null;
        sg1_add_blankrows();
        ViewState["sg1"] = sg1_dt;
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        //setColHeadings();
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

                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from WB_PRECOST_RAW a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
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
                        //setColHeadings();
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
                case "EX1":
                    txtConvExtCost.Text = col1;
                    break;
                case "P1":
                    txtConvPrinRotoCost.Text = col1;
                    break;
                case "P2":
                    txtConvPrinBobstCost.Text = col1;
                    break;
                case "P3":
                    txtConvPrinCICost.Text = col1;
                    break;
                case "LA":
                    txtConvLamCost.Text = col1;
                    break;
                case "PO":
                    txtConvPouchingCost.Text = col1;
                    break;
                case "SL":
                    txtConvSlittingCost.Text = col1;
                    break;
                case "BA":
                    txtConvBagGeneralCost.Text = col1;
                    break;
                case "CK":
                    txtConvBagChickenCost.Text = col1;
                    break;
                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", col1);
                    fgen.fin_engg_reps(frm_qstr);
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
                    //Cal();
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
                    //Cal();
                    break;
                case "I":
                    if (col1.Length > 1)
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = col1;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = col2;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Focus();
                    }
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
                        txtaname.Text = dt.Rows[0]["ANAME"].ToString().Trim();
                        txtiname.Text = dt.Rows[0]["INAME"].ToString().Trim();
                        txtStructure.Text = dt.Rows[0]["structure"].ToString().Trim();
                        txtPrintType.Text = dt.Rows[0]["print_type"].ToString().Trim();
                        txtOrder.Text = dt.Rows[0]["order_qty"].ToString().Trim();
                        txtCYL.Text = dt.Rows[0]["cyl_amor"].ToString().Trim();
                        txtColour.Text = dt.Rows[0]["color"].ToString().Trim();
                        txtLPO.Text = dt.Rows[0]["lpo_no"].ToString().Trim();
                        txtPetThick.Text = dt.Rows[0]["pet_thick"].ToString().Trim();
                        txtPetDensity.Text = dt.Rows[0]["pet_dens"].ToString().Trim();
                        txtPetGSM.Text = dt.Rows[0]["pet_gsm"].ToString().Trim();
                        txtPetRM.Text = dt.Rows[0]["pet_rm"].ToString().Trim();
                        txtPetUSD.Text = dt.Rows[0]["pet_price1"].ToString().Trim();
                        txtPetAED.Text = dt.Rows[0]["pet_price2"].ToString().Trim();
                        txtPetKgUSD.Text = dt.Rows[0]["pet_cost1"].ToString().Trim();
                        txtPetKgAED.Text = dt.Rows[0]["pet_cost2"].ToString().Trim();
                        txtMetThick.Text = dt.Rows[0]["met_thick"].ToString().Trim();
                        txtMetDensity.Text = dt.Rows[0]["met_dens"].ToString().Trim();
                        txtMetGSM.Text = dt.Rows[0]["met_gsm"].ToString().Trim();
                        txtMetRM.Text = dt.Rows[0]["met_rm"].ToString().Trim();
                        txtMetUSD.Text = dt.Rows[0]["met_price1"].ToString().Trim();
                        txtMetAED.Text = dt.Rows[0]["met_price2"].ToString().Trim();
                        txtMetKgUSD.Text = dt.Rows[0]["met_cost1"].ToString().Trim();
                        txtMetKgAED.Text = dt.Rows[0]["met_cost2"].ToString().Trim();
                        txtLPDEThick.Text = dt.Rows[0]["lpde_thick"].ToString().Trim();
                        txtLPDEDensity.Text = dt.Rows[0]["lpde_dens"].ToString().Trim();
                        txtLPDEGSM.Text = dt.Rows[0]["lpde_gsm"].ToString().Trim();
                        txtLPDERM.Text = dt.Rows[0]["lpde_rm"].ToString().Trim();
                        txtLPDEUSD.Text = dt.Rows[0]["lpde_price1"].ToString().Trim();
                        txtLPDEAED.Text = dt.Rows[0]["lpde_price2"].ToString().Trim();
                        txtLPDEKgUSD.Text = dt.Rows[0]["lpde_cost1"].ToString().Trim();
                        txtLPDEKgAED.Text = dt.Rows[0]["lpde_cost2"].ToString().Trim();
                        txtInkThick.Text = dt.Rows[0]["ink_thick"].ToString().Trim();
                        txtInkDensity.Text = dt.Rows[0]["ink_dens"].ToString().Trim();
                        txtInkGSM.Text = dt.Rows[0]["ink_gsm"].ToString().Trim();
                        txtInkRM.Text = dt.Rows[0]["ink_rm"].ToString().Trim();
                        txtInkUSD.Text = dt.Rows[0]["ink_price1"].ToString().Trim();
                        txtInkAED.Text = dt.Rows[0]["ink_price2"].ToString().Trim();
                        txtInkKgUSD.Text = dt.Rows[0]["ink_cost1"].ToString().Trim();
                        txtInkKgAED.Text = dt.Rows[0]["ink_cost2"].ToString().Trim();
                        txtAdh1Thick.Text = dt.Rows[0]["adh1_thick"].ToString().Trim();
                        txtAdh1Density.Text = dt.Rows[0]["adh1_dens"].ToString().Trim();
                        txtAdh1GSM.Text = dt.Rows[0]["adh1_gsm"].ToString().Trim();
                        txtAdh1RM.Text = dt.Rows[0]["adh1_rm"].ToString().Trim();
                        txtAdh1USD.Text = dt.Rows[0]["adh1_price1"].ToString().Trim();
                        txtAdh1AED.Text = dt.Rows[0]["adh1_price2"].ToString().Trim();
                        txtAdh1KgUSD.Text = dt.Rows[0]["adh1_cost1"].ToString().Trim();
                        txtAdh1KgAED.Text = dt.Rows[0]["adh1_cost2"].ToString().Trim();
                        txtAdh2Thick.Text = dt.Rows[0]["adh2_thick"].ToString().Trim();
                        txtAdh2GSM.Text = dt.Rows[0]["adh2_dens"].ToString().Trim();
                        txtAdh2Density.Text = dt.Rows[0]["adh2_gsm"].ToString().Trim();
                        txtAdh2RM.Text = dt.Rows[0]["adh2_rm"].ToString().Trim();
                        txtAdh2USD.Text = dt.Rows[0]["adh2_price1"].ToString().Trim();
                        txtAdh2AED.Text = dt.Rows[0]["adh2_price2"].ToString().Trim();
                        txtAdh2KgUSD.Text = dt.Rows[0]["adh2_cost1"].ToString().Trim();
                        txtAdh2KgAED.Text = dt.Rows[0]["adh2_cost2"].ToString().Trim();
                        txtTotGSM.Text = dt.Rows[0]["tot_gsm"].ToString().Trim();
                        txtTotRM.Text = dt.Rows[0]["tot_rm"].ToString().Trim();
                        txtTotKgUSD.Text = dt.Rows[0]["tot_price1"].ToString().Trim();
                        txtTotKgAED.Text = dt.Rows[0]["tot_price2"].ToString().Trim();
                        txtWastageRM.Text = dt.Rows[0]["wastage"].ToString().Trim();
                        txtWastageKGUSD.Text = dt.Rows[0]["wastage_price1"].ToString().Trim();
                        txtWastageKgAED.Text = dt.Rows[0]["wastage_price2"].ToString().Trim();
                        txtSolventKgUSD.Text = dt.Rows[0]["solvent_price1"].ToString().Trim();
                        txtSolventKgAED.Text = dt.Rows[0]["solvent_price2"].ToString().Trim();
                        txtZipperUSD.Text = dt.Rows[0]["zipper1"].ToString().Trim();
                        txtZipperAED.Text = dt.Rows[0]["zipper2"].ToString().Trim();
                        txtZipperKgUSD.Text = dt.Rows[0]["zipper3"].ToString().Trim();
                        txtZipperKgAED.Text = dt.Rows[0]["zipper4"].ToString().Trim();
                        txtPackingUSD.Text = dt.Rows[0]["packglue1"].ToString().Trim();
                        txtPackingAED.Text = dt.Rows[0]["packglue2"].ToString().Trim();
                        txtPackingKgUSD.Text = dt.Rows[0]["packglue3"].ToString().Trim();
                        txtPackingKgAED.Text = dt.Rows[0]["packglue4"].ToString().Trim();
                        txtPackUSD.Text = dt.Rows[0]["packpet1"].ToString().Trim();
                        txtPackAED.Text = dt.Rows[0]["packpet2"].ToString().Trim();
                        txtPackKgUSD.Text = dt.Rows[0]["packpet3"].ToString().Trim();
                        txtPackKgAED.Text = dt.Rows[0]["packpet4"].ToString().Trim();
                        txtPackCTN.Text = dt.Rows[0]["ctn"].ToString().Trim();
                        txtPackBobbin1.Text = dt.Rows[0]["bobbin1"].ToString().Trim();
                        txtPackBobbin2.Text = dt.Rows[0]["bobbin2"].ToString().Trim();
                        txtTotRMKgUSD.Text = dt.Rows[0]["tot_rmcostkg1"].ToString().Trim();
                        txtTotRMKgAED.Text = dt.Rows[0]["tot_rmcostkg2"].ToString().Trim();
                        txtConvExtCost.Text = dt.Rows[0]["convextcost"].ToString().Trim();
                        txtConvExtHr.Text = dt.Rows[0]["convexthr"].ToString().Trim();
                        txtConvExtTot.Text = dt.Rows[0]["convexttot"].ToString().Trim();
                        txtConvPrinRotoCost.Text = dt.Rows[0]["convrotocost"].ToString().Trim();
                        txtConvPrinRotoHr.Text = dt.Rows[0]["convrotohr"].ToString().Trim();
                        txtConvPrinRotoTot.Text = dt.Rows[0]["convrototot"].ToString().Trim();
                        txtConvPrinBobstCost.Text = dt.Rows[0]["convbobstcost"].ToString().Trim();
                        txtConvPrinBobstHr.Text = dt.Rows[0]["convbobsthr"].ToString().Trim();
                        txtConvPrinBobstTot.Text = dt.Rows[0]["convbobsttot"].ToString().Trim();
                        txtConvPrinCICost.Text = dt.Rows[0]["convcicost"].ToString().Trim();
                        txtConvPrinCIHr.Text = dt.Rows[0]["convcihr"].ToString().Trim();
                        txtConvPrinCITot.Text = dt.Rows[0]["convcitot"].ToString().Trim();
                        txtConvLamCost.Text = dt.Rows[0]["convlamcost"].ToString().Trim();
                        txtConvLamHr.Text = dt.Rows[0]["convlamhr"].ToString().Trim();
                        txtConvLamTot.Text = dt.Rows[0]["convlamtot"].ToString().Trim();
                        txtConvSlittingCost.Text = dt.Rows[0]["convslitcost"].ToString().Trim();
                        txtConvSlittingHr.Text = dt.Rows[0]["convslithr"].ToString().Trim();
                        txtConvSlittingTot.Text = dt.Rows[0]["convslittot"].ToString().Trim();
                        txtConvPouchingCost.Text = dt.Rows[0]["convpouchcost"].ToString().Trim();
                        txtConvPouchingHr.Text = dt.Rows[0]["convpouchhr"].ToString().Trim();
                        txtConvPouchingTot.Text = dt.Rows[0]["convpouchtot"].ToString().Trim();
                        txtConvBagChickenCost.Text = dt.Rows[0]["convbagchickencost"].ToString().Trim();
                        txtConvBagChickenHr.Text = dt.Rows[0]["convbagchickenhr"].ToString().Trim();
                        txtConvBagChickenTot.Text = dt.Rows[0]["convbagchickentot"].ToString().Trim();
                        txtConvBagGeneralCost.Text = dt.Rows[0]["convbaggencost"].ToString().Trim();
                        txtConvBagGeneralHr.Text = dt.Rows[0]["convbaggenhr"].ToString().Trim();
                        txtConvBagGeneralTot.Text = dt.Rows[0]["convbaggentot"].ToString().Trim();
                        txtConvTot.Text = dt.Rows[0]["convtot"].ToString().Trim();
                        txtMach1.Text = dt.Rows[0]["convmachcost"].ToString().Trim();
                        txtConvFuelCost.Text = dt.Rows[0]["convfuel1"].ToString().Trim();
                        txtConvFuelHr.Text = dt.Rows[0]["convfuel2"].ToString().Trim();
                        txtConvFuelTot.Text = dt.Rows[0]["convfuel3"].ToString().Trim();
                        txtMachine1.Text = dt.Rows[0]["convmackg1"].ToString().Trim();
                        txtMachine2.Text = dt.Rows[0]["convmackg2"].ToString().Trim();
                        txtPower1.Text = dt.Rows[0]["convpower1"].ToString().Trim();
                        txtPower2.Text = dt.Rows[0]["convpower2"].ToString().Trim();
                        txtFuel1.Text = dt.Rows[0]["convcharger1"].ToString().Trim();
                        txtFuel2.Text = dt.Rows[0]["convcharger2"].ToString().Trim();
                        txtLabourCost1.Text = dt.Rows[0]["convlabour1"].ToString().Trim();
                        txtLabourCost2.Text = dt.Rows[0]["convlabour2"].ToString().Trim();
                        txtFreight1.Text = dt.Rows[0]["convfrght1"].ToString().Trim();
                        txtFreight2.Text = dt.Rows[0]["convfrght2"].ToString().Trim();
                        txtConvTotCostKg.Text = dt.Rows[0]["convtotkg"].ToString().Trim();
                        txtMgmtFin1.Text = dt.Rows[0]["convprod1"].ToString().Trim();
                        txtMgmtFin2.Text = dt.Rows[0]["convprod2"].ToString().Trim();
                        txtMgmtCost1.Text = dt.Rows[0]["convmgmt1"].ToString().Trim();
                        txtMgmtCost2.Text = dt.Rows[0]["convmgmt2"].ToString().Trim();
                        txtFin1.Text = dt.Rows[0]["convfin1"].ToString().Trim();
                        txtFin2.Text = dt.Rows[0]["convfin2"].ToString().Trim();
                        txtConvTotKg1.Text = dt.Rows[0]["convfinaltotkg1"].ToString().Trim();
                        txtConvTotKg2.Text = dt.Rows[0]["convfinaltotkg2"].ToString().Trim();
                        txtExtCost.Text = dt.Rows[0]["extcost"].ToString().Trim();
                        txtExtHr.Text = dt.Rows[0]["exthr"].ToString().Trim();
                        txtExtTot.Text = dt.Rows[0]["exttot"].ToString().Trim();
                        txtPrinRotoCost.Text = dt.Rows[0]["rotocost"].ToString().Trim();
                        txtPrinRotoHr.Text = dt.Rows[0]["rotohr"].ToString().Trim();
                        txtPrinRotoTot.Text = dt.Rows[0]["rototot"].ToString().Trim();
                        txtPrinBobstCost.Text = dt.Rows[0]["bobstcost"].ToString().Trim();
                        txtPrinBobstHr.Text = dt.Rows[0]["bobsthr"].ToString().Trim();
                        txtPrinBobstTot.Text = dt.Rows[0]["bobsttot"].ToString().Trim();
                        txtPrinCICost.Text = dt.Rows[0]["cicost"].ToString().Trim();
                        txtPrinCIHr.Text = dt.Rows[0]["cihr"].ToString().Trim();
                        txtPrinCITot.Text = dt.Rows[0]["citot"].ToString().Trim();
                        txtLamCost.Text = dt.Rows[0]["lamcost"].ToString().Trim();
                        txtLamHr.Text = dt.Rows[0]["lamhr"].ToString().Trim();
                        txtLamTot.Text = dt.Rows[0]["lamtot"].ToString().Trim();
                        txtSlittingCost.Text = dt.Rows[0]["slitcost"].ToString().Trim();
                        txtSlittingHr.Text = dt.Rows[0]["slithr"].ToString().Trim();
                        txtSlittingTot.Text = dt.Rows[0]["slittot"].ToString().Trim();
                        txtPouchingCost.Text = dt.Rows[0]["pouchcost"].ToString().Trim();
                        txtPouchingHr.Text = dt.Rows[0]["pouchhr"].ToString().Trim();
                        txtPouchingTot.Text = dt.Rows[0]["pouchtot"].ToString().Trim();
                        txtBagChickenCost.Text = dt.Rows[0]["bagchickencost"].ToString().Trim();
                        txtBagChickenHr.Text = dt.Rows[0]["bagchickenhr"].ToString().Trim();
                        txtBagChickenTot.Text = dt.Rows[0]["baggencost"].ToString().Trim();
                        txtBagGeneralCost.Text = dt.Rows[0]["baggencost"].ToString().Trim();
                        txtBagGeneralHr.Text = dt.Rows[0]["baggenhr"].ToString().Trim();
                        txtBagGeneralTot.Text = dt.Rows[0]["baggentot"].ToString().Trim();
                        txtTotalCost.Text = dt.Rows[0]["totcost"].ToString().Trim();
                        txtLabourCost.Text = dt.Rows[0]["labourcostkg"].ToString().Trim();
                        txtPerPcPrice.Text = dt.Rows[0]["perpcprice"].ToString().Trim();
                        txtPerPcPriceFils.Text = dt.Rows[0]["perpcfills"].ToString().Trim();
                        txtOrderPcs.Text = dt.Rows[0]["orderpcs"].ToString().Trim();
                        txtOrderKg.Text = dt.Rows[0]["orderkgs"].ToString().Trim();
                        txtAmortized1.Text = dt.Rows[0]["amortize1"].ToString().Trim();
                        txtAmortized2.Text = dt.Rows[0]["amortize2"].ToString().Trim();
                        txtAmortized3.Text = dt.Rows[0]["amortize3"].ToString().Trim();
                        txtAmortized4.Text = dt.Rows[0]["amortize4"].ToString().Trim();
                        txtAmortized5.Text = dt.Rows[0]["amortize5"].ToString().Trim();
                        txtAmortized6.Text = dt.Rows[0]["amortize6"].ToString().Trim();
                        txtCurrent1.Text = dt.Rows[0]["current1"].ToString().Trim();
                        txtCurrent2.Text = dt.Rows[0]["current2"].ToString().Trim();
                        txtCurrent3.Text = dt.Rows[0]["current3"].ToString().Trim();
                        txtCurrent4.Text = dt.Rows[0]["current3"].ToString().Trim();
                        txtCurrent5.Text = dt.Rows[0]["current5"].ToString().Trim();
                        txtCurrent6.Text = dt.Rows[0]["current6"].ToString().Trim();
                        txtRemarks.Text = dt.Rows[0]["remarks"].ToString().Trim();
                        txtCyAct.Text = dt.Rows[0]["cyact"].ToString().Trim();
                        txtCyPaid.Text = dt.Rows[0]["cypaid"].ToString().Trim();
                        txtCyFills.Text = dt.Rows[0]["cyfills"].ToString().Trim();
                        txtCyWidth.Text = dt.Rows[0]["cyplate"].ToString().Trim();
                        txtCyCircum.Text = dt.Rows[0]["cycircum"].ToString().Trim();
                        txtCyAmor.Text = dt.Rows[0]["cyamortize"].ToString().Trim();
                        txtCySupp.Text = dt.Rows[0]["cysupp"].ToString().Trim();
                        txtCyOrder.Text = dt.Rows[0]["cyorder"].ToString().Trim();
                        txtFlapW.Text = dt.Rows[0]["flapw"].ToString().Trim();
                        txtFlapL.Text = dt.Rows[0]["flapl"].ToString().Trim();
                        txtFlapThickness.Text = dt.Rows[0]["flapthick"].ToString().Trim();
                        txtFlapDown.Text = dt.Rows[0]["flapdown"].ToString().Trim();
                        txtFlapL2.Text = dt.Rows[0]["flapl2"].ToString().Trim();
                        txtFlapThickness2.Text = dt.Rows[0]["flapthick2"].ToString().Trim();
                        txtFlapWt.Text = dt.Rows[0]["flapwt"].ToString().Trim();
                        txtFlapDownWt.Text = dt.Rows[0]["flapdownwt"].ToString().Trim();
                        txtGlue.Text = dt.Rows[0]["gluezipper"].ToString().Trim();
                        txtBagPieceKg.Text = dt.Rows[0]["bagpiece"].ToString().Trim();
                        txtBagPieceMtr.Text = dt.Rows[0]["piecemtr"].ToString().Trim();
                        txtZipper.Text = dt.Rows[0]["zippermtr"].ToString().Trim();
                        txtBagWidth.Text = dt.Rows[0]["bagw"].ToString().Trim();
                        txtBagLength.Text = dt.Rows[0]["bagl"].ToString().Trim();
                        txtBagWeight.Text = dt.Rows[0]["bagwt"].ToString().Trim();
                        txtPackingWt.Text = dt.Rows[0]["packingbagwt"].ToString().Trim();
                        txtPackingMode.Text = dt.Rows[0]["packingmode"].ToString().Trim();
                        txtPackingPkt.Text = dt.Rows[0]["pkt"].ToString().Trim();
                        txtSticker1.Text = dt.Rows[0]["sticker1"].ToString().Trim();
                        txtSticker2.Text = dt.Rows[0]["sticker2"].ToString().Trim();
                        txtSticker3.Text = dt.Rows[0]["sticker3"].ToString().Trim();
                        txtRod1.Text = dt.Rows[0]["rod1"].ToString().Trim();
                        txtRod2.Text = dt.Rows[0]["rod2"].ToString().Trim();
                        txtRod3.Text = dt.Rows[0]["rod3"].ToString().Trim();
                        txtWasher1.Text = dt.Rows[0]["washer1"].ToString().Trim();
                        txtWasher2.Text = dt.Rows[0]["washer2"].ToString().Trim();
                        txtWasher3.Text = dt.Rows[0]["washer3"].ToString().Trim();
                        txtOther1.Text = dt.Rows[0]["others1"].ToString().Trim();
                        txtOther2.Text = dt.Rows[0]["others2"].ToString().Trim();
                        txtOther3.Text = dt.Rows[0]["others3"].ToString().Trim();
                        txtPackTotal.Text = dt.Rows[0]["packingtot"].ToString().Trim();
                        txt1Kg.Text = dt.Rows[0]["for1kg"].ToString().Trim();
                        dt.Dispose();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        //setColHeadings();
                        edmode.Value = "Y";
                        //Cal();

                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.* FROM WB_PRECOST_RAW A WHERE a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO");

                        create_tab();
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                sg1_dr = sg1_dt.NewRow();

                                sg1_dr["sg1_h1"] = dt.Rows[i]["COLHEAD"].ToString().Trim();
                                sg1_dr["sg1_h2"] = dt.Rows[i]["RMATHEAD"].ToString().Trim();

                                sg1_dr["sg1_t1"] = dt.Rows[i]["ICODE"].ToString().Trim();
                                sg1_dr["sg1_t2"] = fgen.seek_iname(frm_qstr, frm_cocd, "sELECT INAME FROM ITEM WHERE TRIM(ICODE)='" + dt.Rows[i]["ICODE"].ToString().Trim() + "'", "iname");
                                sg1_dr["sg1_t3"] = dt.Rows[i]["NUM1"].ToString().Trim();
                                sg1_dr["sg1_t4"] = dt.Rows[i]["NUM2"].ToString().Trim();
                                sg1_dr["sg1_t5"] = dt.Rows[i]["NUM3"].ToString().Trim();
                                sg1_dr["sg1_t6"] = dt.Rows[i]["NUM4"].ToString().Trim();
                                sg1_dr["sg1_t7"] = dt.Rows[i]["NUM5"].ToString().Trim();
                                sg1_dr["sg1_t8"] = dt.Rows[i]["NUM6"].ToString().Trim();
                                sg1_dr["sg1_t9"] = dt.Rows[i]["NUM7"].ToString().Trim();
                                sg1_dr["sg1_t10"] = dt.Rows[i]["NUM8"].ToString().Trim();

                                sg1_dt.Rows.Add(sg1_dr);

                            }

                            sg1.DataSource = sg1_dt;
                            sg1.DataBind();

                            ViewState["sg1"] = sg1_dt;
                            setHead();
                        }
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
                    //setColHeadings();
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
                    //setColHeadings();
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
                    //setColHeadings();
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
            SQuery = "SELECT A.VCHNUM AS ENTRY_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRY_DT,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD,TRIM(A.ACODE) AS CUST_CODE,A.ANAME AS CUST_NAME,TRIM(A.ICODE) AS ITEM_CODE,A.INAME AS ITEM_NAME,A.STRUCTURE,A.PRINT_TYPE,A.ORDER_QTY,A.CYL_AMOR,A.COLOR,A.LPO_NO AS LPO_NUMBER_PRO_INV FROM " + frm_tabname + " A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + PrdRange + " ORDER BY VDD DESC";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " For The Period of " + fromdt + " To " + todt, frm_qstr);
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
            //setColHeadings();

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
                        oporow = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "WB_PRECOST_RAW");

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();

                        save_fun2();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "WB_PRECOST_RAW");

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

                        save_fun2();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "update WB_PRECOST_RAW set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, "WB_PRECOST_RAW");

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                            cmd_query = "delete from WB_PRECOST_RAW where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
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
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                        //setColHeadings();
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
            //sg1.HeaderRow.Cells[1].Width = 250;
            //sg1.HeaderRow.Cells[11].Width = 250;

            for (int i = 0; i < 10; i++)
            {
                if (i != 1)
                {
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    e.Row.Cells[i].CssClass = "hidden";
                }
            }

            //sg1_dr["sg1_h1"] = "H";
            //sg1_dr["sg1_t1"] = "ICODE";
            //sg1_dr["sg1_t2"] = "Raw Materials";
            //sg1_dr["sg1_t3"] = "Thickness";
            //sg1_dr["sg1_t4"] = "Density";
            //sg1_dr["sg1_t5"] = "GSM";
            //sg1_dr["sg1_t6"] = "RM Mixed";
            //sg1_dr["sg1_t7"] = "RM Price(USD)";
            //sg1_dr["sg1_t8"] = "RM Price(AED)";
            //sg1_dr["sg1_t9"] = "Cost/Kg(USD)";
            //sg1_dr["sg1_t10"] = "Cost/Kg(AED)";

            if (e.Row.Cells[1].Text.Trim().ToUpper() == "SOLVENT")
            {
                ((TextBox)e.Row.FindControl("sg1_t7")).Text = "Mtr in KGS";
                ((TextBox)e.Row.FindControl("sg1_t7")).Attributes.Add("readonly", "readonly");
                ((TextBox)e.Row.FindControl("sg1_t8")).Text = "Price";
                ((TextBox)e.Row.FindControl("sg1_t8")).Attributes.Add("readonly", "readonly");
            }

            for (int i = 11; i < 21; i++)
            {
                // if header / calc field then make them readonly
                if (e.Row.Cells[0].Text == "C" || e.Row.Cells[0].Text == "C2")
                {
                    ((TextBox)e.Row.FindControl("sg1_t" + (i - 10) + "")).Attributes.Add("readonly", "readonly");
                }
                if ((e.Row.Cells[0].Text == "T2" || e.Row.Cells[0].Text == "C" || e.Row.Cells[0].Text == "C2" || e.Row.Cells[1].Text.ToUpper() == "WASTAGE") && i < 15)
                    ((TextBox)e.Row.FindControl("sg1_t" + (i - 10) + "")).Style.Add("display", "none");
                if (e.Row.Cells[1].Text.ToUpper() == "WASTAGE" && (i == 15 || i == 17 || i == 18))
                    ((TextBox)e.Row.FindControl("sg1_t" + (i - 10) + "")).Style.Add("display", "none");
                if ((e.Row.Cells[0].Text == "T4") && i < 17)
                    ((TextBox)e.Row.FindControl("sg1_t" + (i - 10) + "")).Style.Add("display", "none");
                if ((e.Row.Cells[0].Text == "T5") && i < 20)
                    ((TextBox)e.Row.FindControl("sg1_t" + (i - 10) + "")).Style.Add("display", "none");
                if (e.Row.Cells[1].Text.ToUpper() == "TOTAL" && (i == 17 || i == 18))
                    ((TextBox)e.Row.FindControl("sg1_t" + (i - 10) + "")).Style.Add("display", "none");

            }
            if (e.Row.Cells[0].Text == "T1")
            {
                ((TextBox)e.Row.FindControl("sg1_t1")).BackColor = Color.LightBlue;
                ((TextBox)e.Row.FindControl("sg1_t3")).BackColor = Color.LightBlue;
                ((TextBox)e.Row.FindControl("sg1_t4")).BackColor = Color.LightBlue;
                ((TextBox)e.Row.FindControl("sg1_t7")).BackColor = Color.LightBlue;
            }
            if (e.Row.Cells[0].Text == "T2")
            {
                ((TextBox)e.Row.FindControl("sg1_t5")).BackColor = Color.LightBlue;
                ((TextBox)e.Row.FindControl("sg1_t7")).BackColor = Color.LightBlue;
            }
            if (e.Row.Cells[0].Text == "T3")
            {
                ((TextBox)e.Row.FindControl("sg1_t6")).BackColor = Color.LightBlue;
            }
            if (e.Row.Cells[0].Text == "T4")
            {
                if (e.Row.Cells[1].Text.Trim().ToUpper() == "SOLVENT") ((TextBox)e.Row.FindControl("sg1_t10")).BackColor = Color.LightBlue;
                else ((TextBox)e.Row.FindControl("sg1_t8")).BackColor = Color.LightBlue;
            }
            if (e.Row.Cells[0].Text == "T5")
            {
                ((TextBox)e.Row.FindControl("sg1_t10")).BackColor = Color.LightBlue;
            }
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
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    // hffield.Value = "SG1_ROW_ADD_E";
                    hffield.Value = "TACODE";
                    hf2.Value = "SG1_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                }
                else
                {
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
        oporow["vchdate"] = txtvchdate.Text.Trim();
        oporow["ACODE"] = txtacode.Text.Trim().ToUpper();
        oporow["ICODE"] = txticode.Text.Trim().ToUpper();
        oporow["ANAME"] = txtaname.Text.Trim().ToUpper();
        oporow["INAME"] = txtiname.Text.Trim().ToUpper();
        oporow["structure"] = txtStructure.Text.Trim().ToUpper();
        oporow["print_type"] = txtPrintType.Text.Trim().ToUpper();
        oporow["order_qty"] = fgen.make_double(txtOrder.Text);
        oporow["cyl_amor"] = fgen.make_double(txtCYL.Text);
        oporow["color"] = fgen.make_double(txtColour.Text);
        oporow["lpo_no"] = txtLPO.Text.Trim().ToUpper();


        //-------------
        //oporow["pet_thick"] = fgen.make_double(txtPetThick.Text);
        //oporow["pet_dens"] = fgen.make_double(txtPetDensity.Text);
        //oporow["pet_gsm"] = fgen.make_double(txtPetGSM.Text);
        //oporow["pet_rm"] = fgen.make_double(txtPetRM.Text);
        //oporow["pet_price1"] = fgen.make_double(txtPetUSD.Text);
        //oporow["pet_price2"] = fgen.make_double(txtPetAED.Text);
        //oporow["pet_cost1"] = fgen.make_double(txtPetKgUSD.Text);
        //oporow["pet_cost2"] = fgen.make_double(txtPetKgAED.Text);
        //oporow["met_thick"] = fgen.make_double(txtMetThick.Text);
        //oporow["met_dens"] = fgen.make_double(txtMetDensity.Text);
        //oporow["met_gsm"] = fgen.make_double(txtMetGSM.Text);
        //oporow["met_rm"] = fgen.make_double(txtMetRM.Text);
        //oporow["met_price1"] = fgen.make_double(txtMetUSD.Text);
        //oporow["met_price2"] = fgen.make_double(txtMetAED.Text);
        //oporow["met_cost1"] = fgen.make_double(txtMetKgUSD.Text);
        //oporow["met_cost2"] = fgen.make_double(txtMetKgAED.Text);
        //oporow["lpde_thick"] = fgen.make_double(txtLPDEThick.Text);
        //oporow["lpde_dens"] = fgen.make_double(txtLPDEDensity.Text);
        //oporow["lpde_gsm"] = fgen.make_double(txtLPDEGSM.Text);
        //oporow["lpde_rm"] = fgen.make_double(txtLPDERM.Text);
        //oporow["lpde_price1"] = fgen.make_double(txtLPDEUSD.Text);
        //oporow["lpde_price2"] = fgen.make_double(txtLPDEAED.Text);
        //oporow["lpde_cost1"] = fgen.make_double(txtLPDEKgUSD.Text);
        //oporow["lpde_cost2"] = fgen.make_double(txtLPDEKgAED.Text);
        //oporow["ink_thick"] = fgen.make_double(txtInkThick.Text);
        //oporow["ink_dens"] = fgen.make_double(txtInkDensity.Text);
        //oporow["ink_gsm"] = fgen.make_double(txtInkGSM.Text);
        //oporow["ink_rm"] = fgen.make_double(txtInkRM.Text);
        //oporow["ink_price1"] = fgen.make_double(txtInkUSD.Text);
        //oporow["ink_price2"] = fgen.make_double(txtInkAED.Text);
        //oporow["ink_cost1"] = fgen.make_double(txtInkKgUSD.Text);
        //oporow["ink_cost2"] = fgen.make_double(txtInkKgAED.Text);
        //oporow["adh1_thick"] = fgen.make_double(txtAdh1Thick.Text);
        //oporow["adh1_dens"] = fgen.make_double(txtAdh1Density.Text);
        //oporow["adh1_gsm"] = fgen.make_double(txtAdh1GSM.Text);
        //oporow["adh1_rm"] = fgen.make_double(txtAdh1RM.Text);
        //oporow["adh1_price1"] = fgen.make_double(txtAdh1USD.Text);
        //oporow["adh1_price2"] = fgen.make_double(txtAdh1AED.Text);
        //oporow["adh1_cost1"] = fgen.make_double(txtAdh1KgUSD.Text);
        //oporow["adh1_cost2"] = fgen.make_double(txtAdh1KgAED.Text);
        //oporow["adh2_thick"] = fgen.make_double(txtAdh2Thick.Text);
        //oporow["adh2_dens"] = fgen.make_double(txtAdh2GSM.Text);
        //oporow["adh2_gsm"] = fgen.make_double(txtAdh2Density.Text);
        //oporow["adh2_rm"] = fgen.make_double(txtAdh2RM.Text);
        //oporow["adh2_price1"] = fgen.make_double(txtAdh2USD.Text);
        //oporow["adh2_price2"] = fgen.make_double(txtAdh2AED.Text);
        //oporow["adh2_cost1"] = fgen.make_double(txtAdh2KgUSD.Text);
        //oporow["adh2_cost2"] = fgen.make_double(txtAdh2KgAED.Text);
        oporow["tot_gsm"] = fgen.make_double(txtTotGSM.Text);
        oporow["tot_rm"] = fgen.make_double(txtTotRM.Text);
        //oporow["tot_price1"] = fgen.make_double(txtTotKgUSD.Text);
        //oporow["tot_price2"] = fgen.make_double(txtTotKgAED.Text);
        //oporow["wastage"] = fgen.make_double(txtWastageRM.Text);
        //oporow["wastage_price1"] = fgen.make_double(txtWastageKGUSD.Text);
        //oporow["wastage_price2"] = fgen.make_double(txtWastageKgAED.Text);
        //oporow["solvent_price1"] = fgen.make_double(txtSolventKgUSD.Text);
        //oporow["solvent_price2"] = fgen.make_double(txtSolventKgAED.Text);
        //oporow["zipper1"] = fgen.make_double(txtZipperUSD.Text);
        //oporow["zipper2"] = fgen.make_double(txtZipperAED.Text);
        //oporow["zipper3"] = fgen.make_double(txtZipperKgUSD.Text);
        //oporow["zipper4"] = fgen.make_double(txtZipperKgAED.Text);
        //oporow["packglue1"] = fgen.make_double(txtPackingUSD.Text);
        //oporow["packglue2"] = fgen.make_double(txtPackingAED.Text);
        //oporow["packglue3"] = fgen.make_double(txtPackingKgUSD.Text);
        //oporow["packglue4"] = fgen.make_double(txtPackingKgAED.Text);
        //oporow["packpet1"] = fgen.make_double(txtPackUSD.Text);
        //oporow["packpet2"] = fgen.make_double(txtPackAED.Text);
        //oporow["packpet3"] = fgen.make_double(txtPackKgUSD.Text);
        //oporow["packpet4"] = fgen.make_double(txtPackKgAED.Text);
        //oporow["ctn"] = fgen.make_double(txtPackCTN.Text);
        //oporow["bobbin1"] = fgen.make_double(txtPackBobbin1.Text);
        //oporow["bobbin2"] = fgen.make_double(txtPackBobbin2.Text);
        oporow["tot_rmcostkg1"] = fgen.make_double(txtTotRMKgUSD.Text);
        oporow["tot_rmcostkg2"] = fgen.make_double(txtTotRMKgAED.Text);
        //-------------



        oporow["convextcost"] = fgen.make_double(txtConvExtCost.Text);
        oporow["convexthr"] = fgen.make_double(txtConvExtHr.Text);
        oporow["convexttot"] = fgen.make_double(txtConvExtTot.Text);
        oporow["convrotocost"] = fgen.make_double(txtConvPrinRotoCost.Text);
        oporow["convrotohr"] = fgen.make_double(txtConvPrinRotoHr.Text);
        oporow["convrototot"] = fgen.make_double(txtConvPrinRotoTot.Text);
        oporow["convbobstcost"] = fgen.make_double(txtConvPrinBobstCost.Text);
        oporow["convbobsthr"] = fgen.make_double(txtConvPrinBobstHr.Text);
        oporow["convbobsttot"] = fgen.make_double(txtConvPrinBobstTot.Text);
        oporow["convcicost"] = fgen.make_double(txtConvPrinCICost.Text);
        oporow["convcihr"] = fgen.make_double(txtConvPrinCIHr.Text);
        oporow["convcitot"] = fgen.make_double(txtConvPrinCITot.Text);
        oporow["convlamcost"] = fgen.make_double(txtConvLamCost.Text);
        oporow["convlamhr"] = fgen.make_double(txtConvLamHr.Text);
        oporow["convlamtot"] = fgen.make_double(txtConvLamTot.Text);
        oporow["convslitcost"] = fgen.make_double(txtConvSlittingCost.Text);
        oporow["convslithr"] = fgen.make_double(txtConvSlittingHr.Text);
        oporow["convslittot"] = fgen.make_double(txtConvSlittingTot.Text);
        oporow["convpouchcost"] = fgen.make_double(txtConvPouchingCost.Text);
        oporow["convpouchhr"] = fgen.make_double(txtConvPouchingHr.Text);
        oporow["convpouchtot"] = fgen.make_double(txtConvPouchingTot.Text);
        oporow["convbagchickencost"] = fgen.make_double(txtConvBagChickenCost.Text);
        oporow["convbagchickenhr"] = fgen.make_double(txtConvBagChickenHr.Text);
        oporow["convbagchickentot"] = fgen.make_double(txtConvBagChickenTot.Text);
        oporow["convbaggencost"] = fgen.make_double(txtConvBagGeneralCost.Text);
        oporow["convbaggenhr"] = fgen.make_double(txtConvBagGeneralHr.Text);
        oporow["convbaggentot"] = fgen.make_double(txtConvBagGeneralTot.Text);
        oporow["convtot"] = fgen.make_double(txtConvTot.Text);
        oporow["convmachcost"] = fgen.make_double(txtMach1.Text);
        oporow["convfuel1"] = fgen.make_double(txtConvFuelCost.Text);
        oporow["convfuel2"] = fgen.make_double(txtConvFuelHr.Text);
        oporow["convfuel3"] = fgen.make_double(txtConvFuelTot.Text);
        oporow["convmackg1"] = fgen.make_double(txtMachine1.Text);
        oporow["convmackg2"] = fgen.make_double(txtMachine2.Text);
        oporow["convpower1"] = fgen.make_double(txtPower1.Text);
        oporow["convpower2"] = fgen.make_double(txtPower2.Text);
        oporow["convcharger1"] = fgen.make_double(txtFuel1.Text);
        oporow["convcharger2"] = fgen.make_double(txtFuel2.Text);
        oporow["convlabour1"] = fgen.make_double(txtLabourCost1.Text);
        oporow["convlabour2"] = fgen.make_double(txtLabourCost2.Text);
        oporow["convfrght1"] = fgen.make_double(txtFreight1.Text);
        oporow["convfrght2"] = fgen.make_double(txtFreight2.Text);
        oporow["convtotkg"] = fgen.make_double(txtConvTotCostKg.Text);
        oporow["convprod1"] = fgen.make_double(txtMgmtFin1.Text);
        oporow["convprod2"] = fgen.make_double(txtMgmtFin2.Text);
        oporow["convmgmt1"] = fgen.make_double(txtMgmtCost1.Text);
        oporow["convmgmt2"] = fgen.make_double(txtMgmtCost2.Text);
        oporow["convfin1"] = fgen.make_double(txtFin1.Text);
        oporow["convfin2"] = fgen.make_double(txtFin2.Text);
        oporow["convfinaltotkg1"] = fgen.make_double(txtConvTotKg1.Text);
        oporow["convfinaltotkg2"] = fgen.make_double(txtConvTotKg2.Text);
        oporow["extcost"] = fgen.make_double(txtExtCost.Text);
        oporow["exthr"] = fgen.make_double(txtExtHr.Text);
        oporow["exttot"] = fgen.make_double(txtExtTot.Text);
        oporow["rotocost"] = fgen.make_double(txtPrinRotoCost.Text);
        oporow["rotohr"] = fgen.make_double(txtPrinRotoHr.Text);
        oporow["rototot"] = fgen.make_double(txtPrinRotoTot.Text);
        oporow["bobstcost"] = fgen.make_double(txtPrinBobstCost.Text);
        oporow["bobsthr"] = fgen.make_double(txtPrinBobstHr.Text);
        oporow["bobsttot"] = fgen.make_double(txtPrinBobstTot.Text);
        oporow["cicost"] = fgen.make_double(txtPrinCICost.Text);
        oporow["cihr"] = fgen.make_double(txtPrinCIHr.Text);
        oporow["citot"] = fgen.make_double(txtPrinCITot.Text);
        oporow["lamcost"] = fgen.make_double(txtLamCost.Text);
        oporow["lamhr"] = fgen.make_double(txtLamHr.Text);
        oporow["lamtot"] = fgen.make_double(txtLamTot.Text);
        oporow["slitcost"] = fgen.make_double(txtSlittingCost.Text);
        oporow["slithr"] = fgen.make_double(txtSlittingHr.Text);
        oporow["slittot"] = fgen.make_double(txtSlittingTot.Text);
        oporow["pouchcost"] = fgen.make_double(txtPouchingCost.Text);
        oporow["pouchhr"] = fgen.make_double(txtPouchingHr.Text);
        oporow["pouchtot"] = fgen.make_double(txtPouchingTot.Text);
        oporow["bagchickencost"] = fgen.make_double(txtBagChickenCost.Text);
        oporow["bagchickenhr"] = fgen.make_double(txtBagChickenHr.Text);
        oporow["bagchickentot"] = fgen.make_double(txtBagChickenTot.Text);
        oporow["baggencost"] = fgen.make_double(txtBagGeneralCost.Text);
        oporow["baggenhr"] = fgen.make_double(txtBagGeneralHr.Text);
        oporow["baggentot"] = fgen.make_double(txtBagGeneralTot.Text);
        oporow["totcost"] = fgen.make_double(txtTotalCost.Text);
        oporow["labourcostkg"] = fgen.make_double(txtLabourCost.Text);
        oporow["perpcprice"] = fgen.make_double(txtPerPcPrice.Text);
        oporow["perpcfills"] = fgen.make_double(txtPerPcPriceFils.Text);
        oporow["orderpcs"] = fgen.make_double(txtOrderPcs.Text);
        oporow["orderkgs"] = fgen.make_double(txtOrderKg.Text);
        oporow["amortize1"] = fgen.make_double(txtAmortized1.Text);
        oporow["amortize2"] = fgen.make_double(txtAmortized2.Text);
        oporow["amortize3"] = fgen.make_double(txtAmortized3.Text);
        oporow["amortize4"] = fgen.make_double(txtAmortized4.Text);
        oporow["amortize5"] = fgen.make_double(txtAmortized5.Text);
        oporow["amortize6"] = fgen.make_double(txtAmortized6.Text);
        oporow["current1"] = fgen.make_double(txtCurrent1.Text);
        oporow["current2"] = fgen.make_double(txtCurrent2.Text);
        oporow["current3"] = fgen.make_double(txtCurrent3.Text);
        oporow["current4"] = fgen.make_double(txtCurrent4.Text);
        oporow["current5"] = fgen.make_double(txtCurrent5.Text);
        oporow["current6"] = fgen.make_double(txtCurrent6.Text);
        oporow["remarks"] = txtRemarks.Text.Trim().ToUpper();
        oporow["cyact"] = fgen.make_double(txtCyAct.Text);
        oporow["cypaid"] = fgen.make_double(txtCyPaid.Text);
        oporow["cyfills"] = fgen.make_double(txtCyFills.Text);
        oporow["cyplate"] = fgen.make_double(txtCyWidth.Text);
        oporow["cycircum"] = fgen.make_double(txtCyCircum.Text);
        oporow["cyamortize"] = fgen.make_double(txtCyAmor.Text);
        oporow["cysupp"] = fgen.make_double(txtCySupp.Text);
        oporow["cyorder"] = fgen.make_double(txtCyOrder.Text);
        oporow["flapw"] = fgen.make_double(txtFlapW.Text);
        oporow["flapl"] = fgen.make_double(txtFlapL.Text);
        oporow["flapthick"] = fgen.make_double(txtFlapThickness.Text);
        oporow["flapdown"] = fgen.make_double(txtFlapDown.Text);
        oporow["flapl2"] = fgen.make_double(txtFlapL2.Text);
        oporow["flapthick2"] = fgen.make_double(txtFlapThickness2.Text);
        oporow["flapwt"] = fgen.make_double(txtFlapWt.Text);
        oporow["flapdownwt"] = fgen.make_double(txtFlapDownWt.Text);
        oporow["gluezipper"] = fgen.make_double(txtGlue.Text);
        oporow["bagpiece"] = fgen.make_double(txtBagPieceKg.Text);
        oporow["piecemtr"] = fgen.make_double(txtBagPieceMtr.Text);
        oporow["zippermtr"] = fgen.make_double(txtZipper.Text);
        oporow["bagw"] = fgen.make_double(txtBagWidth.Text);
        oporow["bagl"] = fgen.make_double(txtBagLength.Text);
        oporow["bagwt"] = fgen.make_double(txtBagWeight.Text);
        oporow["packingbagwt"] = fgen.make_double(txtPackingWt.Text);
        oporow["packingmode"] = fgen.make_double(txtPackingMode.Text);
        oporow["pkt"] = fgen.make_double(txtPackingPkt.Text);
        oporow["sticker1"] = fgen.make_double(txtSticker1.Text);
        oporow["sticker2"] = fgen.make_double(txtSticker2.Text);
        oporow["sticker3"] = fgen.make_double(txtSticker3.Text);
        oporow["rod1"] = fgen.make_double(txtRod1.Text);
        oporow["rod2"] = fgen.make_double(txtRod2.Text);
        oporow["rod3"] = fgen.make_double(txtRod3.Text);
        oporow["washer1"] = fgen.make_double(txtWasher1.Text);
        oporow["washer2"] = fgen.make_double(txtWasher2.Text);
        oporow["washer3"] = fgen.make_double(txtWasher3.Text);
        oporow["others1"] = fgen.make_double(txtOther1.Text);
        oporow["others2"] = fgen.make_double(txtOther2.Text);
        oporow["others3"] = fgen.make_double(txtOther3.Text);
        oporow["packingtot"] = fgen.make_double(txtPackTotal.Text);
        oporow["for1kg"] = fgen.make_double(txt1Kg.Text);
        if (edmode.Value == "Y")
        {
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
    void save_fun2()
    {
        int mys = 0;
        foreach (GridViewRow gr in sg1.Rows)
        {
            oporow2 = oDS2.Tables[0].NewRow();
            oporow2["BRANCHCD"] = frm_mbr;
            oporow2["TYPE"] = frm_vty;
            oporow2["vchnum"] = frm_vnum;
            oporow2["vchdate"] = txtvchdate.Text.Trim();

            oporow2["srno"] = mys;
            mys++;

            oporow2["ICODE"] = ((TextBox)gr.FindControl("sg1_t1")).Text;

            oporow2["COLHEAD"] = gr.Cells[0].Text;
            oporow2["RMATHEAD"] = gr.Cells[1].Text;

            oporow2["NUM1"] = ((TextBox)gr.FindControl("sg1_t3")).Text.toDouble();
            oporow2["NUM2"] = ((TextBox)gr.FindControl("sg1_t4")).Text.toDouble();
            oporow2["NUM3"] = ((TextBox)gr.FindControl("sg1_t5")).Text.toDouble();
            oporow2["NUM4"] = ((TextBox)gr.FindControl("sg1_t6")).Text.toDouble();
            oporow2["NUM5"] = ((TextBox)gr.FindControl("sg1_t7")).Text.toDouble();
            oporow2["NUM6"] = ((TextBox)gr.FindControl("sg1_t8")).Text.toDouble();
            oporow2["NUM7"] = ((TextBox)gr.FindControl("sg1_t9")).Text.toDouble();
            oporow2["NUM8"] = ((TextBox)gr.FindControl("sg1_t10")).Text.toDouble();

            if (edmode.Value == "Y")
            {
                oporow2["ent_by"] = ViewState["entby"].ToString();
                oporow2["ent_dt"] = ViewState["entdt"].ToString();
                oporow2["edt_by"] = frm_uname;
                oporow2["edt_dt"] = vardate;
            }
            else
            {
                oporow2["ent_by"] = frm_uname;
                oporow2["ent_dt"] = vardate;
                oporow2["edt_by"] = "-";
                oporow2["eDt_dt"] = vardate;
            }
            oDS2.Tables[0].Rows.Add(oporow2);
        }
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
    }
    //------------------------------------------------------------------------------------
    void Cal()
    {
        double CyAct = 0, CyAmortize = 0, CyOrder = 0, FlapWeight = 0, FlapDownWeight = 0, BagPieceKg = 0, BagPieceMtr = 0, Zipper = 0, BagWeight = 0, ConvExtTot = 0, ConvPrinRotoTot = 0, ConvPrinBobstTot = 0, ConvPrinCITot = 0, ConvLamTot = 0, ConvSlittingTot = 0, ConvPouchingTot = 0, ConvBagChickenTot = 0, ConvBagGeneralTot = 0, ConvTot = 0, Mach1 = 0, ConvFuelHr = 0, Machine1 = 0, Power1 = 0, Fuel1 = 0, LabourCost1 = 0, Freight1 = 0, ConvTotCostKg = 0, ConvTotKg1 = 0, ConvTotKg2 = 0, ExtTot = 0, PrinRotoTot = 0, PrinBobstTot = 0, PrinCITot = 0, LamTot = 0, SlittingTot = 0, PouchingTot = 0, BagChickenTot = 0, BagGeneralTot = 0, TotalCost = 0, LabourCost = 0, PerPcPrice = 0, PerPcPriceFils = 0, OrderKg = 0, Amortized6 = 0, Current6 = 0, Amortized1 = 0, Amortized2 = 0, Amortized3 = 0, Amortized5 = 0, Current1 = 0, Current2 = 0, Current3 = 0, Current5 = 0, MgmtFin2 = 0;
        double PetGSM = 0, MetGSM = 0, LdpeGSM = 0, InkGSM = 0, Adh1GSM = 0, Adh2GSM = 0, TotGSM = 0, PetRM = 0, MetRM = 0, InkRM = 0, LdpeRM = 0, Adh1RM = 0, Adh2RM = 0, TotRM = 0;
        double PetAED = 0, PetCostUSD = 0, PetCostAED = 0, MetAED = 0, MetCostUSD = 0, MetCostAED = 0, LdpeAED = 0, LdpeCostUSD = 0, LdpeCostAED = 0, InkAED = 0, InkCostUSD = 0, InkCostAED = 0, Adh1AED = 0, Adh1CostUSD = 0, Adh1CostAED = 0, Adh2AED = 0, Adh2CostUSD = 0, Adh2CostAED = 0, TotCostUSD = 0, TotCostAED = 0;
        double SolventKgUSD = 0, WastageKGUSD = 0, WastageKGAED = 0, ZipperKgUSD = 0, ZipperKgAED = 0, PackingKgUSD = 0, PackingKgAED = 0, PackKgUSD = 0, PackKgAED = 0, TotRmKgUSD = 0, TotRmKgAED = 0;
        double PackingBagWt = 0, PackingPkt = 0, Sticker = 0, Rod = 0, Washer = 0, Others = 0, TotPacking = 0, For1Kg = 0;

        CyAct = fgen.make_double(txtCyWidth.Text) * fgen.make_double(txtCyCircum.Text) * fgen.make_double(txtColour.Text) * fgen.make_double(txtCyFills.Text);
        txtCyAct.Text = (Math.Round(CyAct, 1)).ToString();
        CyAmortize = (CyAct - fgen.make_double(txtCyPaid.Text)) / fgen.make_double(txtCYL.Text);
        txtCyAmor.Text = (Math.Round(CyAmortize, 5)).ToString();
        CyOrder = (CyAct - fgen.make_double(txtCyPaid.Text)) / fgen.make_double(txtOrder.Text);
        txtCyOrder.Text = (Math.Round(CyOrder, 2)).ToString();

        FlapWeight = fgen.make_double(txtFlapW.Text) * fgen.make_double(txtFlapL.Text) * fgen.make_double(txtFlapThickness.Text) * 0.925;
        txtFlapWt.Text = (Math.Round(FlapWeight, 2)).ToString();
        FlapDownWeight = fgen.make_double(txtFlapDown.Text) * fgen.make_double(txtFlapL2.Text) * fgen.make_double(txtFlapThickness2.Text) * 0.925;
        txtFlapDownWt.Text = (Math.Round(FlapDownWeight, 2)).ToString();

        PetGSM = fgen.make_double(txtPetThick.Text) * fgen.make_double(txtPetDensity.Text);
        MetGSM = fgen.make_double(txtMetThick.Text) * fgen.make_double(txtMetDensity.Text);
        LdpeGSM = fgen.make_double(txtLPDEThick.Text) * fgen.make_double(txtLPDEDensity.Text);
        InkGSM = fgen.make_double(txtInkGSM.Text);
        Adh1GSM = fgen.make_double(txtAdh1GSM.Text);
        Adh2GSM = fgen.make_double(txtAdh2GSM.Text);
        TotGSM = PetGSM + MetGSM + LdpeGSM + InkGSM + Adh1GSM + Adh2GSM;

        txtPetGSM.Text = (Math.Round(PetGSM, 2)).ToString();
        txtMetGSM.Text = (Math.Round(MetGSM, 2)).ToString();
        txtLPDEGSM.Text = (Math.Round(LdpeGSM, 2)).ToString();
        txtTotGSM.Text = (Math.Round(TotGSM, 2)).ToString();

        PetRM = (PetGSM / TotGSM) * 100;
        MetRM = (MetGSM / TotGSM) * 100;
        LdpeRM = (LdpeGSM / TotGSM) * 100;
        InkRM = (InkGSM / TotGSM) * 100;
        Adh1RM = (Adh1GSM / TotGSM) * 100;
        Adh2RM = (Adh2GSM / TotGSM) * 100;
        TotRM = PetRM + MetRM + LdpeRM + InkRM + Adh1RM + Adh2RM;

        txtPetRM.Text = (Math.Round(PetRM, 2)).ToString();
        txtMetRM.Text = (Math.Round(MetRM, 2)).ToString();
        txtLPDERM.Text = (Math.Round(LdpeRM, 2)).ToString();
        txtInkRM.Text = (Math.Round(InkRM, 2)).ToString();
        txtAdh1RM.Text = (Math.Round(Adh1RM, 2)).ToString();
        txtAdh2RM.Text = (Math.Round(Adh2RM, 2)).ToString();
        //txtTotRM.Text = (Math.Round(TotRM, 2)).ToString();

        PetAED = fgen.make_double(txtPetUSD.Text) * 3.675;
        MetAED = fgen.make_double(txtMetUSD.Text) * 3.675;
        LdpeAED = fgen.make_double(txtLPDEUSD.Text) * 3.675;
        InkAED = fgen.make_double(txtInkUSD.Text) * 3.675;
        Adh1AED = fgen.make_double(txtAdh1USD.Text) * 3.675;
        Adh2AED = fgen.make_double(txtAdh2USD.Text) * 3.675;

        txtPetAED.Text = (Math.Round(PetAED, 2)).ToString();
        txtMetAED.Text = (Math.Round(MetAED, 2)).ToString();
        txtLPDEAED.Text = (Math.Round(LdpeAED, 2)).ToString();
        txtInkAED.Text = (Math.Round(InkAED, 2)).ToString();
        txtAdh1AED.Text = (Math.Round(Adh1AED, 2)).ToString();
        txtAdh2AED.Text = (Math.Round(Adh2AED, 2)).ToString();

        PetCostUSD = (PetRM * fgen.make_double(txtPetUSD.Text)) / 100;
        MetCostUSD = (MetRM * fgen.make_double(txtMetUSD.Text)) / 100;
        LdpeCostUSD = (LdpeRM * fgen.make_double(txtLPDEUSD.Text)) / 100;
        InkCostUSD = (InkRM * fgen.make_double(txtInkUSD.Text)) / 100;
        Adh1CostUSD = (Adh1RM * fgen.make_double(txtAdh1USD.Text)) / 100;
        Adh2CostUSD = (Adh2RM * fgen.make_double(txtAdh2USD.Text)) / 100;
        TotCostUSD = PetCostUSD + MetCostUSD + LdpeCostUSD + InkCostUSD + Adh1CostUSD + Adh2CostUSD;

        txtPetKgUSD.Text = (Math.Round(PetCostUSD, 2)).ToString();
        txtMetKgUSD.Text = (Math.Round(MetCostUSD, 2)).ToString();
        txtLPDEKgUSD.Text = (Math.Round(LdpeCostUSD, 2)).ToString();
        txtInkKgUSD.Text = (Math.Round(InkCostUSD, 2)).ToString();
        txtAdh1KgUSD.Text = (Math.Round(Adh1CostUSD, 2)).ToString();
        txtAdh2KgUSD.Text = (Math.Round(Adh2CostUSD, 2)).ToString();
        //txtTotKgUSD.Text = (Math.Round(TotCostUSD, 2)).ToString();

        PetCostAED = PetCostUSD * 3.675;
        MetCostAED = MetCostUSD * 3.675;
        LdpeCostAED = LdpeCostUSD * 3.675;
        InkCostAED = InkCostUSD * 3.675;
        Adh1CostAED = Adh1CostUSD * 3.675;
        Adh2CostAED = Adh2CostUSD * 3.675;
        TotCostAED = PetCostAED + MetCostAED + LdpeCostAED + InkCostAED + Adh1CostAED + Adh2CostAED;

        txtPetKgAED.Text = (Math.Round(PetCostAED, 2)).ToString();
        txtMetKgAED.Text = (Math.Round(MetCostAED, 2)).ToString();
        txtLPDEKgAED.Text = (Math.Round(LdpeCostAED, 2)).ToString();
        txtInkKgAED.Text = (Math.Round(InkCostAED, 2)).ToString();
        txtAdh1KgAED.Text = (Math.Round(Adh1CostAED, 2)).ToString();
        txtAdh2KgAED.Text = (Math.Round(Adh2CostAED, 2)).ToString();
        //txtTotKgAED.Text = (Math.Round(TotCostAED, 2)).ToString();

        WastageKGUSD = (TotCostUSD * fgen.make_double(txtWastageRM.Text)) / 100;
        txtWastageKGUSD.Text = (Math.Round(WastageKGUSD, 2)).ToString();
        WastageKGAED = WastageKGUSD * 3.675;
        txtWastageKgAED.Text = (Math.Round(WastageKGAED, 2)).ToString();
        SolventKgUSD = fgen.make_double(txtSolventKgAED.Text) / 3.675;
        txtSolventKgUSD.Text = (Math.Round(SolventKgUSD, 2)).ToString();

        BagWeight = (fgen.make_double(txtBagWidth.Text) * (fgen.make_double(txtBagLength.Text) * 2) * TotGSM) + FlapWeight + FlapDownWeight + fgen.make_double(txtGlue.Text);
        txtBagWeight.Text = (Math.Round(BagWeight, 2)).ToString();
        BagPieceKg = 1000 / BagWeight;
        txtBagPieceKg.Text = (Math.Round(BagPieceKg, 2)).ToString();
        BagPieceMtr = 1 / fgen.make_double(txtBagWidth.Text);
        txtBagPieceMtr.Text = (Math.Round(BagPieceMtr, 2)).ToString();
        Zipper = BagPieceKg / BagPieceMtr;
        txtZipper.Text = (Math.Round(Zipper, 2)).ToString();

        txtZipperUSD.Text = (Math.Round(Zipper, 2)).ToString();
        txtPackingUSD.Text = (Math.Round(Zipper, 2)).ToString();
        txtPackUSD.Text = (Math.Round(Zipper, 2)).ToString();

        ZipperKgAED = fgen.make_double(txtZipperAED.Text) * Zipper;
        PackingKgAED = fgen.make_double(txtPackingAED.Text) * Zipper;
        PackKgAED = fgen.make_double(txtPackAED.Text) * Zipper;
        ZipperKgUSD = ZipperKgAED / 3.675;
        PackingKgUSD = PackingKgAED / 3.675;
        // PackKgUSD = PackKgAED / 3.675;

        txtZipperKgAED.Text = (Math.Round(ZipperKgAED, 2)).ToString();
        txtPackingKgAED.Text = (Math.Round(PackingKgAED, 2)).ToString();
        txtPackKgAED.Text = (Math.Round(PackKgAED, 2)).ToString();

        txtZipperKgUSD.Text = (Math.Round(ZipperKgUSD, 2)).ToString();
        txtPackingKgUSD.Text = (Math.Round(PackingKgUSD, 2)).ToString();
        //txtPackKgUSD.Text = (Math.Round(PackKgUSD, 2)).ToString();

        TotRmKgUSD = TotCostUSD + WastageKGUSD + SolventKgUSD + ZipperKgUSD + fgen.make_double(txtPackBobbin1.Text);
        txtTotRMKgUSD.Text = (Math.Round(TotRmKgUSD, 2)).ToString();

        PackingBagWt = BagWeight / 1000;
        txtPackingWt.Text = (Math.Round(PackingBagWt, 6)).ToString();
        PackingPkt = PackingBagWt * fgen.make_double(txtPackingMode.Text);
        txtPackingPkt.Text = (Math.Round(PackingPkt, 4)).ToString();
        Sticker = fgen.make_double(txtSticker2.Text) * fgen.make_double(txtSticker3.Text);
        Rod = fgen.make_double(txtRod2.Text) * fgen.make_double(txtRod3.Text);
        Washer = fgen.make_double(txtWasher2.Text) * fgen.make_double(txtWasher3.Text);
        Others = fgen.make_double(txtOther2.Text) * fgen.make_double(txtOther3.Text);
        TotPacking = Sticker + Rod + Washer + Others;
        txtSticker1.Text = (Math.Round(Sticker, 2)).ToString();
        txtRod1.Text = (Math.Round(Rod, 2)).ToString();
        txtWasher1.Text = (Math.Round(Washer, 2)).ToString();
        txtOther1.Text = (Math.Round(Others, 2)).ToString();
        txtPackTotal.Text = (Math.Round(TotPacking, 2)).ToString();
        For1Kg = 1 / PackingPkt * TotPacking;
        txt1Kg.Text = (Math.Round(For1Kg, 8)).ToString();
        txtPackBobbin2.Text = (Math.Round(For1Kg, 2)).ToString();

        TotRmKgAED = TotCostAED + WastageKGAED + fgen.make_double(txtSolventKgAED.Text) + ZipperKgAED + PackingKgAED + PackKgAED + fgen.make_double(txtPackCTN.Text) + For1Kg;
        txtTotRMKgAED.Text = (Math.Round(TotRmKgAED, 2)).ToString();

        ConvExtTot = fgen.make_double(txtConvExtCost.Text) * fgen.make_double(txtConvExtHr.Text);
        ConvPrinRotoTot = fgen.make_double(txtConvPrinRotoCost.Text) * fgen.make_double(txtConvPrinRotoHr.Text);
        ConvPrinBobstTot = fgen.make_double(txtConvPrinBobstCost.Text) * fgen.make_double(txtConvPrinBobstHr.Text);
        ConvPrinCITot = fgen.make_double(txtConvPrinCICost.Text) * fgen.make_double(txtConvPrinCIHr.Text);
        ConvLamTot = fgen.make_double(txtConvLamCost.Text) * fgen.make_double(txtConvLamHr.Text);
        ConvSlittingTot = fgen.make_double(txtConvSlittingCost.Text) * fgen.make_double(txtConvSlittingHr.Text);
        ConvPouchingTot = fgen.make_double(txtConvPouchingCost.Text) * fgen.make_double(txtConvPouchingHr.Text);
        ConvBagChickenTot = fgen.make_double(txtConvBagChickenCost.Text) * fgen.make_double(txtConvBagChickenHr.Text);
        ConvBagGeneralTot = fgen.make_double(txtConvBagGeneralCost.Text) * fgen.make_double(txtConvBagGeneralHr.Text);
        ConvTot = ConvExtTot + ConvPrinRotoTot + ConvPrinBobstTot + ConvPrinCITot + ConvLamTot + ConvSlittingTot + ConvPouchingTot + ConvBagChickenTot + ConvBagGeneralTot;
        txtConvExtTot.Text = (Math.Round(ConvExtTot, 2)).ToString();
        txtConvPrinRotoTot.Text = (Math.Round(ConvPrinRotoTot, 2)).ToString();
        txtConvPrinBobstTot.Text = (Math.Round(ConvPrinBobstTot, 2)).ToString();
        txtConvPrinCITot.Text = (Math.Round(ConvPrinCITot, 2)).ToString();
        txtConvLamTot.Text = (Math.Round(ConvLamTot, 2)).ToString();
        txtConvSlittingTot.Text = (Math.Round(ConvSlittingTot, 2)).ToString();
        txtConvPouchingTot.Text = (Math.Round(ConvPouchingTot, 2)).ToString();
        txtConvBagChickenTot.Text = (Math.Round(ConvBagChickenTot, 2)).ToString();
        txtConvBagGeneralTot.Text = (Math.Round(ConvBagGeneralTot, 2)).ToString();
        txtConvTot.Text = (Math.Round(ConvTot, 2)).ToString();
        Mach1 = ConvTot / fgen.make_double(txtOrder.Text);
        txtMach1.Text = (Math.Round(Mach1, 2)).ToString();
        txtMachine2.Text = (Math.Round(Mach1, 2)).ToString();
        ConvFuelHr = fgen.make_double(txtConvFuelCost.Text) * 14;
        txtConvFuelHr.Text = (Math.Round(ConvFuelHr, 2)).ToString();
        Machine1 = Mach1 / 3.675;
        txtMachine1.Text = (Math.Round(Machine1, 2)).ToString();
        Power1 = fgen.make_double(txtPower2.Text) / 3.675;
        txtPower1.Text = (Math.Round(Power1, 2)).ToString();
        Fuel1 = fgen.make_double(txtFuel2.Text) / 3.675;
        txtFuel1.Text = (Math.Round(Fuel1, 2)).ToString();
        MgmtFin2 = TotRmKgAED + fgen.make_double(txtConvTotCostKg.Text);
        txtMgmtFin2.Text = (Math.Round(MgmtFin2, 2)).ToString();
        ConvTotKg1 = fgen.make_double(txtMgmtFin1.Text) + fgen.make_double(txtMgmtCost1.Text) + fgen.make_double(txtFin1.Text);
        txtConvTotKg1.Text = (Math.Round(ConvTotKg1, 2)).ToString();
        ConvTotKg2 = fgen.make_double(txtMgmtFin2.Text) + fgen.make_double(txtMgmtCost2.Text) + fgen.make_double(txtFin2.Text);
        txtConvTotKg2.Text = (Math.Round(ConvTotKg2, 2)).ToString();

        ExtTot = fgen.make_double(txtExtCost.Text) * fgen.make_double(txtExtHr.Text);
        PrinRotoTot = fgen.make_double(txtPrinRotoCost.Text) * fgen.make_double(txtPrinRotoHr.Text);
        PrinBobstTot = fgen.make_double(txtPrinBobstCost.Text) * fgen.make_double(txtPrinBobstHr.Text);
        PrinCITot = fgen.make_double(txtPrinCICost.Text) * fgen.make_double(txtPrinCIHr.Text);
        LamTot = fgen.make_double(txtLamCost.Text) * fgen.make_double(txtLamHr.Text);
        SlittingTot = fgen.make_double(txtSlittingCost.Text) * fgen.make_double(txtSlittingHr.Text);
        PouchingTot = fgen.make_double(txtPouchingCost.Text) * fgen.make_double(txtPouchingHr.Text);
        BagChickenTot = fgen.make_double(txtBagChickenCost.Text) * fgen.make_double(txtBagChickenHr.Text);
        BagGeneralTot = fgen.make_double(txtBagGeneralCost.Text) * fgen.make_double(txtBagGeneralHr.Text);
        txtExtTot.Text = (Math.Round(ExtTot, 2)).ToString();
        txtPrinRotoTot.Text = (Math.Round(PrinRotoTot, 2)).ToString();
        txtPrinBobstTot.Text = (Math.Round(PrinBobstTot, 2)).ToString();
        txtPrinCITot.Text = (Math.Round(PrinCITot, 2)).ToString();
        txtLamTot.Text = (Math.Round(LamTot, 2)).ToString();
        txtSlittingTot.Text = (Math.Round(SlittingTot, 2)).ToString();
        txtPouchingTot.Text = (Math.Round(PouchingTot, 2)).ToString();
        txtBagChickenTot.Text = (Math.Round(BagChickenTot, 2)).ToString();
        txtBagGeneralTot.Text = (Math.Round(BagGeneralTot, 2)).ToString();
        TotalCost = ExtTot + PrinRotoTot + PrinBobstTot + PrinCITot + LamTot + SlittingTot + PouchingTot + BagChickenTot + BagGeneralTot;
        txtTotalCost.Text = (Math.Round(TotalCost, 2)).ToString();
        LabourCost = TotalCost / fgen.make_double(txtOrder.Text);
        txtLabourCost.Text = (Math.Round(LabourCost, 2)).ToString();
        txtCurrent4.Text = txtAmortized4.Text;
        Amortized6 = ConvTotKg2 + CyAmortize;
        Current6 = ConvTotKg2 + CyOrder;
        Amortized5 = fgen.make_double(txtAmortized4.Text) / 3.675;
        Current5 = fgen.make_double(txtCurrent4.Text) / 3.675;
        Amortized3 = ((fgen.make_double(txtAmortized4.Text) - Amortized6) / fgen.make_double(txtAmortized4.Text)) * 100;
        Amortized2 = ((fgen.make_double(txtAmortized4.Text) - (CyAmortize + TotRmKgAED)) / fgen.make_double(txtAmortized4.Text)) * 100;
        Current2 = (1 - ((CyOrder + TotRmKgAED) / fgen.make_double(txtCurrent4.Text))) * 100;
        Amortized1 = (Amortized6 / fgen.make_double(txtAmortized4.Text)) * 100;
        Current1 = (Current6 / fgen.make_double(txtCurrent4.Text)) * 100;
        Current3 = 100 - Current1;

        txtAmortized6.Text = (Math.Round(Amortized6, 2)).ToString();
        txtCurrent6.Text = (Math.Round(Current6, 2)).ToString();
        txtAmortized5.Text = (Math.Round(Amortized5, 2)).ToString();
        txtCurrent5.Text = (Math.Round(Current5, 2)).ToString();
        txtAmortized3.Text = (Math.Round(Amortized3, 2)).ToString();
        txtCurrent3.Text = (Math.Round(Current3, 2)).ToString();
        txtAmortized2.Text = (Math.Round(Amortized2, 2)).ToString();
        txtCurrent2.Text = (Math.Round(Current2, 2)).ToString();
        txtAmortized1.Text = (Math.Round(Amortized1, 2)).ToString();
        txtCurrent1.Text = (Math.Round(Current1, 2)).ToString();
        PerPcPrice = fgen.make_double(txtAmortized4.Text) / BagPieceKg;
        txtPerPcPrice.Text = (Math.Round(PerPcPrice, 3)).ToString();
        PerPcPriceFils = fgen.make_double(txtAmortized4.Text) / BagPieceKg * 100;
        txtPerPcPriceFils.Text = (Math.Round(PerPcPriceFils, 3)).ToString();
        OrderKg = fgen.make_double(txtOrderPcs.Text) * BagWeight / 1000;
        txtOrderKg.Text = (Math.Round(OrderKg, 2)).ToString();

        txtLabourCost2.Text = txtLabourCost.Text;
        LabourCost1 = fgen.make_double(txtLabourCost2.Text) / 3.675;
        txtLabourCost1.Text = (Math.Round(LabourCost1, 2)).ToString();
        Freight1 = fgen.make_double(txtFreight2.Text) / 3.675;
        txtFreight1.Text = (Math.Round(Freight1, 2)).ToString();
        ConvTotCostKg = Mach1 + fgen.make_double(txtPower2.Text) + fgen.make_double(txtFuel2.Text) + fgen.make_double(txtLabourCost2.Text) + fgen.make_double(txtFreight2.Text);
        txtConvTotCostKg.Text = (Math.Round(ConvTotCostKg, 2)).ToString();
    }
    //------------------------------------------------------------------------------------    
    protected void btnrefresh_ServerClick(object sender, EventArgs e)
    {
        Cal();
    }
    //------------------------------------------------------------------------------------
    protected void btnparty_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "CUST";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnicode_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "ITEM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Item", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    //SQuery = "create table wb_precost ( branchcd varchar2(2) default '-',type varchar2(2) default '-',vchnum varchar2(6) default '-',vchdate date default sysdate,acode varchar2(6) default '-',icode varchar2(8) default '-',aname varchar2(150) default '-',iname varchar2(150) default '-',structure varchar2(30) default '-',print_type varchar2(30) default '-',lpo_no varchar2(30) default '-',order_qty number(20,6) default 0,cyl_amor number(20,6) default 0,color number(20,6) default 0,pet_thick number(20,6) default 0,pet_dens number(20,6) default 0,pet_gsm number(20,6) default 0,pet_rm number(20,6) default 0,pet_price1 number(20,6) default 0,pet_price2 number(20,6) default 0,pet_cost1 number(20,6) default 0,pet_cost2 number(20,6) default 0,met_thick number(20,6) default 0,met_dens number(20,6) default 0,met_gsm number(20,6) default 0,met_rm number(20,6) default 0,met_price1 number(20,6) default 0,met_price2 number(20,6) default 0,met_cost1 number(20,6) default 0,met_cost2 number(20,6) default 0,lpde_thick number(20,6) default 0,lpde_dens number(20,6) default 0,lpde_gsm number(20,6) default 0,lpde_rm number(20,6) default 0,lpde_price1 number(20,6) default 0,lpde_price2 number(20,6) default 0,lpde_cost1 number(20,6) default 0,lpde_cost2 number(20,6) default 0,ink_thick number(20,6) default 0,ink_dens number(20,6) default 0,ink_gsm number(20,6) default 0,ink_rm number(20,6) default 0,ink_price1 number(20,6) default 0,ink_price2 number(20,6) default 0,ink_cost1 number(20,6) default 0,ink_cost2 number(20,6) default 0,adh1_thick number(20,6) default 0,adh1_dens number(20,6) default 0,adh1_gsm number(20,6) default 0,adh1_rm number(20,6) default 0,adh1_price1 number(20,6) default 0,adh1_price2 number(20,6) default 0,adh1_cost1 number(20,6) default 0,adh1_cost2 number(20,6) default 0,adh2_thick number(20,6) default 0,adh2_dens number(20,6) default 0,adh2_gsm number(20,6) default 0,adh2_rm number(20,6) default 0,adh2_price1 number(20,6) default 0,adh2_price2 number(20,6) default 0,adh2_cost1 number(20,6) default 0,adh2_cost2 number(20,6) default 0,tot_gsm number(20,6) default 0,tot_rm number(20,6) default 0,tot_price1 number(20,6) default 0,tot_price2 number(20,6) default 0,wastage number(20,6) default 0,wastage_price1 number(20,6) default 0,wastage_price2 number(20,6) default 0,solvent_price1 number(20,6) default 0,solvent_price2 number(20,6) default 0,zipper1 number(20,6) default 0,zipper2 number(20,6) default 0,zipper3 number(20,6) default 0,zipper4 number(20,6) default 0,packglue1 number(20,6) default 0,packglue2 number(20,6) default 0,packglue3 number(20,6) default 0,packglue4 number(20,6) default 0,packpet1 number(20,6) default 0,packpet2 number(20,6) default 0,packpet3 number(20,6) default 0,packpet4 number(20,6) default 0,ctn number(20,6) default 0,bobbin1 number(20,6) default 0,bobbin2 number(20,6) default 0,tot_rmcostkg1 number(20,6) default 0,tot_rmcostkg2 number(20,6) default 0,convextcost number(20,6) default 0,convexthr number(20,6) default 0,convexttot number(20,6) default 0,convrotocost number(20,6) default 0,convrotohr number(20,6) default 0,convrototot number(20,6) default 0,convbobstcost number(20,6) default 0,convbobsthr number(20,6) default 0,convbobsttot number(20,6) default 0,convcicost number(20,6) default 0,convcihr number(20,6) default 0,convcitot number(20,6) default 0,convlamcost number(20,6) default 0,convlamhr number(20,6) default 0,convlamtot number(20,6) default 0,convslitcost number(20,6) default 0,convslithr number(20,6) default 0,convslittot number(20,6) default 0,convpouchcost number(20,6) default 0,convpouchhr number(20,6) default 0,convpouchtot number(20,6) default 0,convbagchickencost number(20,6) default 0,convbagchickenhr number(20,6) default 0,convbagchickentot number(20,6) default 0,convbaggencost number(20,6) default 0,convbaggenhr number(20,6) default 0,convbaggentot number(20,6) default 0,convtot number(20,6) default 0,convmachcost number(20,6) default 0,convfuel1 number(20,6) default 0,convfuel2 number(20,6) default 0,convfuel3 number(20,6) default 0,convmackg1 number(20,6) default 0,convmackg2 number(20,6) default 0,convpower1 number(20,6) default 0,convpower2 number(20,6) default 0,convcharger1 number(20,6) default 0,convcharger2 number(20,6) default 0,convlabour1 number(20,6) default 0,convlabour2 number(20,6) default 0,convfrght1 number(20,6) default 0,convfrght2 number(20,6) default 0,convtotkg number(20,6) default 0,convprod1 number(20,6) default 0,convprod2 number(20,6) default 0,convmgmt1 number(20,6) default 0,convmgmt2 number(20,6) default 0,convfin1 number(20,6) default 0,convfin2 number(20,6) default 0,convfinaltotkg1 number(20,6) default 0,convfinaltotkg2 number(20,6) default 0,extcost number(20,6) default 0,exthr number(20,6) default 0,exttot number(20,6) default 0,rotocost number(20,6) default 0,rotohr number(20,6) default 0,rototot number(20,6) default 0,bobstcost number(20,6) default 0,bobsthr number(20,6) default 0,bobsttot number(20,6) default 0,cicost number(20,6) default 0,cihr number(20,6) default 0,citot number(20,6) default 0,lamcost number(20,6) default 0,lamhr number(20,6) default 0,lamtot number(20,6) default 0,slitcost number(20,6) default 0,slithr number(20,6) default 0,slittot number(20,6) default 0,pouchcost number(20,6) default 0,pouchhr number(20,6) default 0,pouchtot number(20,6) default 0,bagchickencost number(20,6) default 0,bagchickenhr number(20,6) default 0,bagchickentot number(20,6) default 0,baggencost number(20,6) default 0,baggenhr number(20,6) default 0,baggentot number(20,6) default 0,totcost number(20,6) default 0,labourcostkg number(20,6) default 0,perpcprice number(20,6) default 0,perpcfills number(20,6) default 0,orderpcs number(20,6) default 0,orderkgs number(20,6) default 0,amortize1 number(20,6) default 0,amortize2 number(20,6) default 0,amortize3 number(20,6) default 0,amortize4 number(20,6) default 0,amortize5 number(20,6) default 0,amortize6 number(20,6) default 0,current1 number(20,6) default 0,current2 number(20,6) default 0,current3 number(20,6) default 0,current4 number(20,6) default 0,current5 number(20,6) default 0,current6 number(20,6) default 0,remarks varchar2(100) default '-',cyact number(20,6) default 0,cypaid number(20,6) default 0,cyfills number(20,6) default 0,cyplate number(20,6) default 0,cycircum number(20,6) default 0,cyamortize number(20,6) default 0,cysupp number(20,6) default 0,cyorder number(20,6) default 0,flapw number(20,6) default 0,flapl number(20,6) default 0,flapthick number(20,6) default 0,flapdown number(20,6) default 0,flapl2 number(20,6) default 0,flapthick2 number(20,6) default 0,flapwt number(20,6) default 0,flapdownwt number(20,6) default 0,gluezipper number(20,6) default 0,bagpiece number(20,6) default 0,piecemtr number(20,6) default 0,zippermtr number(20,6) default 0,bagw number(20,6) default 0,bagl number(20,6) default 0,bagwt number(20,6) default 0,packingbagwt number(20,6) default 0,packingmode number(20,6) default 0,pkt number(20,6) default 0,sticker1 number(20,6) default 0,sticker2 number(20,6) default 0,sticker3 number(20,6) default 0,rod1 number(20,6) default 0,rod2 number(20,6) default 0,rod3 number(20,6) default 0,washer1 number(20,6) default 0,washer2 number(20,6) default 0,washer3 number(20,6) default 0,others1 number(20,6) default 0,others2 number(20,6) default 0,others3 number(20,6) default 0,packingtot number(20,6) default 0,for1kg number(20,6) default 0,ent_by varchar2(20) default '-',ent_dt date default sysdate,edt_by varchar2(20) default '-',edt_dt date default sysdate)";
    //fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
    protected void btnGridPop_Click(object sender, EventArgs e)
    {
        hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_sg1_t1_", "");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
        if (hf2.Value == "I")
        {
            hffield.Value = "I";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Item", frm_qstr);
        }
        if (hf2.Value == "A")
        {
            hffield.Value = "A";
            insertRow();
        }
    }

    void insertRow()
    {
        dt = new DataTable();
        sg1_dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        sg1_dt = dt.Clone();
        sg1_dr = null;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.Trim();
            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.Trim();

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
            sg1_dt.Rows.Add(sg1_dr);
        }
        ViewState["sg1"] = sg1_dt;

        dt = new DataTable();
        sg1_dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        sg1_dr = null;
        create_tab();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (i == hf1.Value.ToString().toDouble() + 1)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_h1"] = dt.Rows[Convert.ToInt16(hf1.Value)]["sg1_h1"];
                sg1_dr["sg1_h2"] = dt.Rows[Convert.ToInt16(hf1.Value)]["sg1_h2"];
                sg1_dt.Rows.Add(sg1_dr);
            }

            sg1_dr = sg1_dt.NewRow();
            for (int c = 0; c < dt.Columns.Count; c++)
            {
                sg1_dr[c] = dt.Rows[i][c];
            }
            sg1_dt.Rows.Add(sg1_dr);
        }
        ViewState["sg1"] = sg1_dt;
        sg1.DataSource = sg1_dt;
        sg1.DataBind();

        setHead();
    }
    protected void btnExtrusionAED_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "EX1";
        make_qry_4_popup();
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btnPrintingROTOAed_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "P1";
        make_qry_4_popup();
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btnPrintBobstAED_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "P2";
        make_qry_4_popup();
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btnPrintCIAed_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "P3";
        make_qry_4_popup();
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btnLaminationAED_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "LA";
        make_qry_4_popup();
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btnConvSlittingCost_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SL";
        make_qry_4_popup();
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btnConvPouchingCost_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PO";
        make_qry_4_popup();
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btnConvBagGen_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BA";
        make_qry_4_popup();
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btnBagChicken_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CK";
        make_qry_4_popup();
        fgen.Fn_open_sseek("", frm_qstr);
    }
}