using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_mis_txt : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
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
    string Prg_Id, lbl1a_Text, CSR;
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
                    lbl1a_Text = "TR";
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
                lblheader.Text = "Performance Monitoring (Yearly)";

                Box1_01.InnerText = "Production_Tonnage";
                Box1_02.InnerText = "SBD";
                Box1_03.InnerText = "FBD";
                Box1_04.InnerText = "BWL";

                Box2_01.InnerText = "Packed_Tonnage";
                Box2_02.InnerText = "SBD";
                Box2_03.InnerText = "FBD";
                Box2_04.InnerText = "BWL";


                Box3_01.InnerText = "Shipments_Completed (L)";
                Box3_02.InnerText = "SBD";
                Box3_03.InnerText = "FBD";
                Box3_04.InnerText = "BWL";

                Box4_01.InnerText = "Value_Of_Open_PO (L)";
                Box4_02.InnerText = "SBD";
                Box4_03.InnerText = "FBD";
                Box4_04.InnerText = "BWL";

                Box5_01.InnerText = "Number_of_Containers";
                Box5_02.InnerText = "SBD";
                Box5_03.InnerText = "FBD";
                Box5_04.InnerText = "BWL";

                Box6_01.InnerText = "Preventive_Maint_Completion";
                Box6_02.InnerText = "SBD";
                Box6_03.InnerText = "FBD";
                Box6_04.InnerText = "BWL";

                Box7_01.InnerText = "Finance(Total AR) (L)";
                Box7_02.InnerText = "SBD";
                Box7_03.InnerText = "FBD";
                Box7_04.InnerText = "BWL";

                Box8_01.InnerText = "Finance(Total AP) (L)";
                Box8_02.InnerText = "SBD";
                Box8_03.InnerText = "FBD";
                Box8_04.InnerText = "BWL";

                Box9_01.InnerText = "Tot Inven. (L)";
                Box9_02.InnerText = "SBD";
                Box9_03.InnerText = "FBD";
                Box9_04.InnerText = "BWL";

                Box10_01.InnerText = "Tot_Revenue(Baisc_val)";
                Box10_02.InnerText = "SBD";
                Box10_03.InnerText = "FBD";
                Box10_04.InnerText = "BWL";

                Box11_01.InnerText = "Cost (Total Freight)";
                Box11_02.InnerText = "SBD";
                Box11_03.InnerText = "FBD";
                Box11_04.InnerText = "BWL";

                Box12_01.InnerText = "Cost (Prv Mnth)";
                Box12_02.InnerText = "SBD";
                Box12_03.InnerText = "FBD";
                Box12_04.InnerText = "BWL";

                if (frm_cocd == "OMNI")
                {
                    Box1_01.InnerText = "Production_Tonnage";
                    Box1_02.InnerText = "Main_Plant";
                    Box1_T03.Visible = false;
                    Box1_T04.Visible = false;
                    Box1_03.Visible = false;
                    Box1_04.Visible = false;

                    Box2_01.InnerText = "Dispatch_Qty (K)";
                    Box2_02.InnerText = "Main_Plant";
                    Box2_T03.Visible = false;
                    Box2_T04.Visible = false;
                    Box2_03.Visible = false;
                    Box2_04.Visible = false;

                    Box3_01.InnerText = "Expense (L)";
                    Box3_02.InnerText = "Main_Plant";
                    Box3_T03.Visible = false;
                    Box3_T04.Visible = false;
                    Box3_03.Visible = false;
                    Box3_04.Visible = false;                    

                    Box4_01.InnerText = "Value_Of_Open_PO (L)";
                    Box4_02.InnerText = "Main_Plant";
                    Box4_T03.Visible = false;
                    Box4_T04.Visible = false;
                    Box4_03.Visible = false;
                    Box4_04.Visible = false;

                    Box5_01.InnerText = "Number_of_Containers";
                    Box5_02.InnerText = "Main_Plant";
                    Box5_T03.Visible = false;
                    Box5_T04.Visible = false;
                    Box5_03.Visible = false;
                    Box5_04.Visible = false;

                    div_box5.Visible = false;

                    Box6_01.InnerText = "Preventive_Maint_Completion";
                    Box6_02.InnerText = "Main_Plant";
                    Box6_T03.Visible = false;
                    Box6_T04.Visible = false;
                    Box6_03.Visible = false;
                    Box6_04.Visible = false;

                    div_box6.Visible = false;

                    Box7_01.InnerText = "Finance(Total AR) (L)";
                    Box7_02.InnerText = "Main_Plant";
                    Box7_T03.Visible = false;
                    Box7_T04.Visible = false;
                    Box7_03.Visible = false;
                    Box7_04.Visible = false;

                    Box8_01.InnerText = "Finance(Total AP) (L)";
                    Box8_02.InnerText = "Main_Plant";
                    Box8_T03.Visible = false;
                    Box8_T04.Visible = false;
                    Box8_03.Visible = false;
                    Box8_04.Visible = false;

                    Box9_01.InnerText = "Tot Inven. (L)";
                    Box9_02.InnerText = "Main_Plant";
                    Box9_T03.Visible = false;
                    Box9_T04.Visible = false;
                    Box9_03.Visible = false;
                    Box9_04.Visible = false;

                    Box10_01.InnerText = "Tot_Revenue(Baisc_val)";
                    Box10_02.InnerText = "Main_Plant";
                    Box10_T03.Visible = false;
                    Box10_T04.Visible = false;
                    Box10_03.Visible = false;
                    Box10_04.Visible = false;

                    Box11_01.InnerText = "Cost (Total Freight)";
                    Box11_02.InnerText = "Main_Plant";
                    Box11_T03.Visible = false;
                    Box11_T04.Visible = false;
                    Box11_03.Visible = false;
                    Box11_04.Visible = false;

                    div_box11.Visible = false;

                    Box12_01.InnerText = "Cost (Prv Mnth)";
                    Box12_02.InnerText = "Main_Plant";
                    Box12_T03.Visible = false;
                    Box12_T04.Visible = false;
                    Box12_03.Visible = false;
                    Box12_04.Visible = false;

                    div_box12.Visible = false;
                }

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();

                getValues();
            }
            setColHeadings();
            set_Val();

            if (frm_ulvl != "0")
            {
                btndel.Visible = false;
            }
            if (CSR != "-")
            {


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

        //txtlbl2.Attributes.Add("readonly", "readonly");
        //txtlbl3.Attributes.Add("readonly", "readonly");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");



        //txtlbl6.Attributes.Add("readonly", "readonly");

        //my_Tabs
        //txtlbl2.Attributes["required"] = "true";
        //txtlbl2.BackColor = System.Drawing.ColorTranslator.FromHtml("#E0FF00");
        // to hide and show to tab panel

        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        AllTabs.Visible = false;

        //switch (Prg_Id)
        //{

        //    case "M12008":
        //        tab3.Visible = false;
        //        tab4.Visible = false;
        //        break;
        //    case "F45101":
        //        AllTabs.Visible = false;
        //        break;
        //}



        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false;
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


        btnedit.Visible = false; btnsave.Visible = false; btndel.Visible = false; btnlist.Visible = false;



    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {

        btnedit.Disabled = true;
        btnsave.Disabled = false;
        btnlist.Disabled = true;
        btnprint.Disabled = true;
        btndel.Disabled = true;
        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;
        btnexit.Visible = false;
        btncancel.Visible = true;
        btnedit.Visible = false; btnsave.Visible = false; btndel.Visible = false; btnlist.Visible = false;
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
        doc_nf.Value = "TRCNO";
        doc_df.Value = "TRCDT";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "WB_TASK_LOG";

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "TR");
        typePopup = "N";

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {

        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        if (CSR.Length > 1) cond = " and trim(a.ccode)='" + cond + "'";
        switch (btnval)
        {


            case "BTN_23":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1,rate  FROM TYPE WHERE ID='A' order by name ";
                break;
            case "TACODE":
                //pop1
                Acode_Sel_query();
                break;
            case "TICODE":
                //pop1
                Icode_Sel_query();
                break;
            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[13].Text.Trim() + "'";
                    else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                }
                if (col1.Length <= 0) col1 = "'-'";
                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(Trim(icode))>4 and icode like '9%' and trim(icode) in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname  ";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
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
                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(Trim(icode))>4 and icode like '9%' and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname  ";
                break;
            case "SG1_ROW_TAX":
                SQuery = "Select Type1 as fstr,Name,Type1 as Code,nvl(Rate,0) as Rate,nvl(Excrate,0) as Schg,exc_Addr as Ref_Code from type where id='S' and length(Trim(nvl(cstno,'-')))<=1 order by name";
                break;
            case "New":
                Type_Sel_query();
                break;
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            case "CCODE":
                SQuery = "SELECT DISTINCT USERNAME ,USERNAME AS COCD,FULL_NAME AS company_name FROM EVAS WHERE userid>'000060' and NVL(USERNAME,'-')!='-' ORDER BY USERNAME";
                break;
            case "PERSON":
                SQuery = "SELECT DISTINCT USERNAME ,USERNAME AS COCD,userid AS Team_ID,emailid FROM EVAS WHERE userid<='000060' and NVL(USERNAME,'-')!='-' ORDER BY USERNAME";
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Rec_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Rec_Dt,a.Ccode as Client_Code,a.Client_Name,a.Team_Member ,a.Client_Person,a.Client_Phone, a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where  a.branchcd='" + frm_mbr + "' and a.type='" + lbl1a_Text + "' " + cond + " order by vdd desc,a." + doc_nf.Value + " desc";
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
            // else comment upper code


            if (CSR.Length > 1)
            {

            }
            getValues();
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        frm_vty = vty;

        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");



        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        //txtlbl2.Text = frm_uname;
        //txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

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

        //--------------------------------
        ////sg4_dt = new DataTable();
        ////create_tab4();
        ////sg4_add_blankrows();
        ////sg4_add_blankrows();
        ////sg4_add_blankrows();
        ////sg4_add_blankrows();
        ////sg4_add_blankrows();
        ////sg4.DataSource = sg4_dt;
        ////sg4.DataBind();
        ////setColHeadings();
        ////ViewState["sg4"] = sg4_dt;        
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

        fgen.fill_dash(this.Controls);



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
        //SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + DateRange + " and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
        //fgen.Fn_FillChart(frm_cocd, frm_qstr, "Testing Chart", "line", "Main Heading", "Sub Heading", SQuery);
        //hffield.Value = "Print";
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
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
                //fgen.execute_cmd(frm_qstr,frm_cocd, "delete from poterm a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                //fgen.execute_cmd(frm_qstr,frm_cocd, "delete from budgmst a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data a where par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
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

                    break;
                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a_Text = "TR";
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;
                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a_Text = "TR";
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
                    string mv_col;
                    mv_col = frm_mbr + frm_vty + col1;
                    SQuery = "Select a.*,to_Char(a.ent_Dt,'dd/mm/yyyy') As ment_date,to_Char(a.app_Dt,'dd/mm/yyyy') As mapp_date from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + mv_col + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();



                        if (dt.Rows[i]["filename"].ToString().Trim().Length > 1)
                        {
                            lblUpload.Text = dt.Rows[i]["filepath"].ToString().Trim();
                            txtAttch.Text = dt.Rows[i]["filename"].ToString().Trim();
                        }


                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            //sg1_dr["sg1_h1"] = "-";
                            //sg1_dr["sg1_h2"] = "-";
                            //sg1_dr["sg1_h3"] = "-";
                            //sg1_dr["sg1_h4"] = "-";
                            //sg1_dr["sg1_h5"] = "-";
                            //sg1_dr["sg1_h6"] = "-";


                            //sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                            //sg1_dr["sg1_f2"] = dt.Rows[i]["Iname"].ToString().Trim();
                            //sg1_dr["sg1_f3"] = dt.Rows[i]["ICpartno"].ToString().Trim();
                            //sg1_dr["sg1_f4"] = dt.Rows[i]["ICdrgno"].ToString().Trim();
                            //sg1_dr["sg1_f5"] = dt.Rows[i]["Unit"].ToString().Trim();

                            //sg1_dr["sg1_t1"] = dt.Rows[i]["desc_"].ToString().Trim();
                            //sg1_dr["sg1_t2"] = dt.Rows[i]["cu_chldt1"].ToString().Trim();
                            //sg1_dr["sg1_t3"] = dt.Rows[i]["qtyord"].ToString().Trim();
                            //sg1_dr["sg1_t4"] = dt.Rows[i]["irate"].ToString().Trim();
                            //sg1_dr["sg1_t5"] = dt.Rows[i]["cdisc"].ToString().Trim();

                            //sg1_dr["sg1_t6"] = dt.Rows[i]["class"].ToString().Trim();
                            //sg1_dr["sg1_t7"] = dt.Rows[i]["ipack"].ToString().Trim();
                            //sg1_dr["sg1_t8"] = dt.Rows[i]["SD"].ToString().Trim();

                            //sg1_dr["sg1_t9"] = dt.Rows[i]["pexc"].ToString().Trim();
                            //sg1_dr["sg1_t10"] = dt.Rows[i]["st_type"].ToString().Trim();
                            //sg1_dr["sg1_t11"] = dt.Rows[i]["ptax"].ToString().Trim();
                            //sg1_dr["sg1_t12"] = dt.Rows[i]["desc9"].ToString().Trim();
                            //sg1_dr["sg1_t13"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            //sg1_dr["sg1_t14"] = dt.Rows[i]["iexc_Addl"].ToString().Trim();
                            //sg1_dr["sg1_t15"] = dt.Rows[i]["qtysupp"].ToString().Trim();
                            //sg1_dr["sg1_t16"] = dt.Rows[i]["sta_Rate"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        //------------------------
                        //SQuery = "Select nvl(a.terms,'-') as terms,nvl(a.condi,'-') as condi from poterm a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + mv_col + "' ORDER BY a.sno";
                        //dt = new DataTable();
                        //dt = fgen.getdata(frm_qstr,frm_cocd, SQuery);

                        //create_tab2();
                        //sg2_dr = null;
                        //if (dt.Rows.Count > 0)
                        //{
                        //    for (i = 0; i < dt.Rows.Count; i++)
                        //    {

                        //        sg2_dr = sg2_dt.NewRow();
                        //        sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;

                        //        sg2_dr["sg2_t1"] = dt.Rows[i]["terms"].ToString().Trim();
                        //        sg2_dr["sg2_t2"] = dt.Rows[i]["condi"].ToString().Trim();

                        //        sg2_dt.Rows.Add(sg2_dr);
                        //    }
                        //}
                        //sg2_add_blankrows();
                        //ViewState["sg2"] = sg2_dt;
                        //sg2.DataSource = sg2_dt;
                        //sg2.DataBind();
                        //dt.Dispose();
                        //sg2_dt.Dispose();
                        //------------------------
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

                            //SQuery = "Select a.icode,to_chaR(a.dlv_Date,'dd/mm/yyyy') As dlv_Date,nvl(a.budgetcost,0) as budgetcost,nvl(a.actualcost,0) as actualcost,a.jobcardrqd,b.iname,nvl(b.cpartno,'-') As cpartno,nvl(b.cdrgno,'-') as cdrgno,nvl(b.unit,'-') as Unit from budgmst a,item b where trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + mv_col + "' ORDER BY A.SRNO ";
                            ////union all Select '-' as icode,to_DatE(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy') as dlv_Date,0 as budgetcost,'-' as iname,'-' As cpartno,'-' as cdrgno,nvl(b.unit,'-') as Unit from dual                         

                            //dt = new DataTable();
                            //dt = fgen.getdata(frm_qstr,frm_cocd, SQuery);

                            //create_tab3();
                            //sg3_dr = null;
                            //if (dt.Rows.Count > 0)
                            //{
                            //    for (i = 0; i < dt.Rows.Count; i++)
                            //    {
                            //        sg3_dr = sg3_dt.NewRow();
                            //        sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;
                            //        sg3_dr["sg3_f1"] = dt.Rows[i]["icode"].ToString().Trim();
                            //        sg3_dr["sg3_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            //        sg3_dr["sg3_t1"] = dt.Rows[i]["dlv_Date"].ToString().Trim();
                            //        sg3_dr["sg3_t2"] = dt.Rows[i]["budgetcost"].ToString().Trim();
                            //        sg3_dr["sg3_t3"] = dt.Rows[i]["actualcost"].ToString().Trim();
                            //        sg3_dr["sg3_t4"] = dt.Rows[i]["jobcardrqd"].ToString().Trim();
                            //        sg3_dt.Rows.Add(sg3_dr);
                            //    }
                            //}
                            //sg3_add_blankrows();
                            //ViewState["sg3"] = sg3_dt;
                            //sg3.DataSource = sg3_dt;
                            //sg3.DataBind();
                            //dt.Dispose();
                            //
                            sg3_dt.Dispose();
                        }

                        //-----------------------
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";



                    }
                    #endregion
                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    //txtlbl4.Text = col1;
                    //txtlbl4a.Text = col2;

                    //txtlbl5.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    //txtlbl6.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");

                    //btnlbl7.Focus();
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
                case "BTN_20":
                    break;
                case "BTN_21":
                    break;
                case "BTN_22":
                    break;
                case "BTN_23":
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
            SQuery = "SELECT a.TRCNO as TRC_NO,to_char(A.TRCDT,'dd/mm/yyyy') as tRC_Dt,a.CCODE,a.Client_Name,a.Task_type,a.Team_member,Tgt_days as Time_Limit,a.Client_Person,a.Client_Phone,a.Oremarks,a.Ent_Dt,last_Action,last_Actdt,a.app_by,a.app_dt,to_chaR(a.TRCDT,'YYYYMMDD') as TRC_DTd FROM " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.TRCdt " + PrdRange + " order by a.TRCno ";
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

            }
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");

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
                    frm_tabname = "WB_Task_LOG";
                    save_fun();
                    //save_fun2();
                    save_fun3();
                    save_fun4();
                    save_fun5();

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
                    oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");


                    if (edmode.Value == "Y")
                    {

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


                    }

                    // If Vchnum becomes 000000 then Re-Save
                    if (frm_vnum == "000000") btnhideF_Click(sender, e);

                    save_fun();
                    //save_fun2();
                    save_fun3();
                    save_fun4();
                    save_fun5();
                    string ddl_fld1;
                    string ddl_fld2;
                    ddl_fld1 = frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    ddl_fld2 = frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    if (edmode.Value == "Y")
                    {



                    }
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                    //fgen.save_data(frm_qstr, frm_cocd, oDS3, "poterm");
                    //fgen.save_data(frm_qstr, frm_cocd, oDS4, "budgmst");
                    fgen.save_data(frm_qstr, frm_cocd, oDS5, "udf_Data");
                    //fgen.save_data(frm_qstr, frm_cocd, oDS2, "ivchctrl");

                    if (edmode.Value == "Y")
                    {
                        fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");

                    }
                    else
                    {
                        if (save_it == "Y")
                        {
                            html_body = html_body + "Please Note that Task No : " + frm_vnum + "<br>";
                            html_body = html_body + "For :<br>";

                            html_body = html_body + "<br>";


                            html_body = html_body + "<br>";

                            html_body = html_body + "<br>";
                            html_body = html_body + "<br>";
                            html_body = html_body + "We request that the Above Task should be done as per schedule given .<br>";
                            html_body = html_body + "Please update your Task Action in Task Action Routine.<br>";
                            html_body = html_body + "<br>";
                            html_body = html_body + "Thanks,<br>";
                            html_body = html_body + "<br>";
                            html_body = html_body + "Assigned By : " + frm_uname + "<br>";
                            string mhd = "";

                            fgen.msg("-", "AMSG", "Task No " + frm_vnum + "'13'We appreciate your Entry to Finsys Task Mgr.Aiming for Systematic Working.");
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

    }

    //------------------------------------------------------------------------------------   
    protected void sg4_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void btnAtt_Click(object sender, EventArgs e)
    {
        string filepath = @"c:/tej_erp/UPLOAD/";
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            txtAttch.Text = Attch.FileName;

            Attch.PostedFile.SaveAs(filepath);
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
        string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "c:/tej_erp/" + filePath.Replace("\\", "/") + "','90%','90%','Finsys Viewer');", true);
    }
    protected void btnDwnld1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));

            Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
            Session["FileName"] = txtAttch.Text;
            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");
        }
        catch { }
    }

    void getValues()
    {
        string branchcd_cd = "'01','04','08'";
        string branchcode = "01";
        if (frm_cocd == "OMNI")
        {
            branchcd_cd = "'00'";
            branchcode = "00";
        }

        //Production_Tonnage
        SQuery = "SELECT ROUND(SUM(A.IQTYIN * (CASE WHEN IS_NUMBER(B.IWEIGHT)>0 THEN IS_NUMBER(B.IWEIGHT) ELSE 1 END)) / 1000) AS QTY,A.BRANCHCD FROM PROD_SHEETK A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) AND A.BRANCHCD IN (" + branchcd_cd + ") AND A.TYPE='86' AND A.ACODE='61' AND A.VCHDATE " + DateRange + " GROUP BY A.BRANCHCD ORDER BY A.BRANCHCD ";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            Box1_T02.Value = fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY");
            Box1_T03.Value = fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY");
            Box1_T04.Value = fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY");
        }
        Box1_T01.Value = (Box1_T02.Value.ToString().toDouble() + Box1_T03.Value.ToString().toDouble() + Box1_T04.Value.ToString().toDouble()).ToString();

        //Packed_Tonnage 
        SQuery = "SELECT round(SUM(A.IQTYIN * (CASE WHEN IS_NUMBER(B.IWEIGHT)>0 THEN IS_NUMBER(B.IWEIGHT) ELSE 1 END)) / 1000) AS QTY,A.BRANCHCD FROM IVOUCHER A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) AND A.BRANCHCD IN (" + branchcd_cd + ") AND A.TYPE='17' AND A.VCHDATE " + DateRange + " GROUP BY A.BRANCHCD ORDER BY A.BRANCHCD ";
        if (frm_cocd == "OMNI")
            SQuery = "SELECT round(SUM(A.IQTYOUT / 1000)) AS QTY,A.BRANCHCD FROM IVOUCHER A WHERE A.BRANCHCD IN (" + branchcd_cd + ") AND A.TYPE like '4%' AND A.VCHDATE " + DateRange + " GROUP BY A.BRANCHCD ORDER BY A.BRANCHCD ";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            Box2_T02.Value = fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY");
            Box2_T03.Value = fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY");
            Box2_T04.Value = fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY");
        }
        Box2_T01.Value = (Box2_T02.Value.ToString().toDouble() + Box2_T03.Value.ToString().toDouble() + Box2_T04.Value.ToString().toDouble()).ToString();

        //Shipments_Completed 
        SQuery = "SELECT ROUND(SUM(IAMOUNT/100000),2) AS QTY,BRANCHCD FROM IVOUCHER WHERE BRANCHCD IN (" + branchcd_cd + ") AND TYPE LIKE '4%' AND VCHDATE " + DateRange + " GROUP BY BRANCHCD ORDER BY BRANCHCD ";
        if (frm_cocd == "OMNI")
            SQuery = "SELECT ROUND(SUM(DRAMT/100000),2) AS QTY,BRANCHCD FROM VOUCHER WHERE BRANCHCD IN (" + branchcd_cd + ") AND TO_CHAR(VCHDATE,'MM/YYYY')=TO_CHAR(ADD_MONTHS(SYSDATE,-1),'MM/YYYY')  GROUP BY BRANCHCD ORDER BY BRANCHCD ";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            Box3_T02.Value = fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY");
            Box3_T03.Value = fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY");
            Box3_T04.Value = fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY");
        }
        Box3_T01.Value = (Box3_T02.Value.ToString().toDouble() + Box3_T03.Value.ToString().toDouble() + Box3_T04.Value.ToString().toDouble()).ToString();

        //Value_Of_Open_PO
        SQuery = "SELECT round(SUM(QTYORD),2) AS QTY,BRANCHCD FROM POMAS WHERE BRANCHCD IN (" + branchcd_cd + ") AND TYPE LIKE '5%' AND ORDDT " + DateRange + " GROUP BY BRANCHCD ORDER BY BRANCHCD ";
        SQuery = "SELECT SUM(Pend_Value) AS QTY,BRANCHCD FROM (Select a.Ordno as SO_No,to_char(A.orddt,'dd/mm/yyyy') as SO_DT,b.Aname as Customer_Name,c.Iname as Part_Name,c.cpartno as Part_Number,a.qtyord as Order_Qty,a.Soldqty as Despatch_Qty,a.bal_qty as Pend_Qty,c.Unit,round(a.bal_qty*a.srate,2) as Pend_Value,a.Pordno as Cust_po_no,trim(a.acode) as ERP_Acode,trim(a.icode) as ERP_Icode,to_chaR(a.orddt,'yyyymmdd') as VDD,A.BRANCHCD from wbvu_pending_so a, famst b,item c where a.branchcd in (" + branchcd_cd + ") and a.orddt " + DateRange + " and trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(c.icode) and a.bal_qty>0) GROUP BY BRANCHCD ORDER BY BRANCHCD";
        SQuery = "SELECT ROUND(SUM(prate * bal_qty)/100000,2) AS QTY,BRANCHCD FROM (select trim(a.Fstr) as fstr,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Cpartno as Part_no,a.Prate,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Cdrgno,b.Unit,b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty,null as btchno,null as btchdt,a.BRANCHCD from (select fstr,ERP_code,max(prate) as prate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,BRANCHCD  from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate,BRANCHCD from pomas where branchcd in (" + branchcd_cd + ") and type like '5%' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy') union all SELECT trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate,BRANCHCD from ivoucherp where branchcd in (" + branchcd_cd + ") and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') )  group by fstr,ERP_code,BRANCHCD having (case when sum(Qtyord)>0 then sum(Qtyord)-sum(Soldqty) else max(prate) end)>0  )a,item b where trim(a.erp_code)=trim(B.icode) ) group by branchcd";
        SQuery = "select ROUND(sum(nvl(a.prate,0)* nvl(a.bal,0)) / 100000,2) AS QTY, A.BRANCHCD from PENDING_PO_ALL A where A.BRANCHCD in (" + branchcd_cd + ") and A.TYPE like '5%' AND A.ORDDT >= to_date('01/04/2018','dd/mm/yyyy') GROUP BY A.BRANCHCD ";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            Box4_T02.Value = fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY");
            Box4_T03.Value = fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY");
            Box4_T04.Value = fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY");
        }
        Box4_T01.Value = (Box4_T02.Value.ToString().toDouble() + Box4_T03.Value.ToString().toDouble() + Box4_T04.Value.ToString().toDouble()).ToString();

        //Number_of_Containers 
        SQuery = "SELECT COUNT(distinct VCHNUM) AS QTY,BRANCHCD FROM IVOUCHERP WHERE BRANCHCD IN (" + branchcd_cd + ") AND TYPE='2Z' AND VCHDATE " + DateRange + " GROUP BY BRANCHCD ORDER BY BRANCHCD ";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            Box5_T02.Value = fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY");
            Box5_T03.Value = fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY");
            Box5_T04.Value = fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY");
        }
        Box5_T01.Value = (Box5_T02.Value.ToString().toDouble() + Box5_T03.Value.ToString().toDouble() + Box5_T04.Value.ToString().toDouble()).ToString();

        //Preventive_Maint_Completion 
        SQuery = "SELECT COUNT(VCHNUM) AS QTY,BRANCHCD FROM PMAINT WHERE BRANCHCD IN (" + branchcd_cd + ") AND TYPE='66' AND VCHDATE " + DateRange + " GROUP BY BRANCHCD ORDER BY BRANCHCD ";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            Box6_T02.Value = fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY");
            Box6_T03.Value = fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY");
            Box6_T04.Value = fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY");
        }

        //Finance total AR
        SQuery = "SELECT ROUND(SUM((NET)/100000),2) AS QTY,BRANCHCD FROM RECDATA WHERE BRANCHCD IN (" + branchcd_cd + ") AND SUBSTR(ACODE,1,2)='16' GROUP BY BRANCHCD ORDER BY BRANCHCD ";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        Box7_T01.Value = "0";
        if (dt.Rows.Count > 0)
        {
            Box7_T02.Value = (fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY").ToString().toDouble() > 0 ? fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY").ToString() + " Dr" : (fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY").ToString().toDouble() * -1) + " Cr");
            Box7_T03.Value = (fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY").ToString().toDouble() > 0 ? fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY").ToString() + " Dr" : (fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY").ToString().toDouble() * -1) + " Cr");
            Box7_T04.Value = (fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY").ToString().toDouble() > 0 ? fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY").ToString() + " Dr" : (fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY").ToString().toDouble() * -1) + " Cr");
            Box7_T01.Value = (fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY").ToString().toDouble() + fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY").ToString().toDouble() + fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY").ToString().toDouble()).ToString();
            Box7_T01.Value += (Box7_T01.Value.toDouble() > 0) ? " Dr" : " Cr";
            Box7_T01.Value = Box7_T01.Value.Replace("-", "");
        }

        //Finance total AP
        SQuery = "SELECT ROUND(SUM((NET)/100000),2) AS QTY,BRANCHCD FROM RECDATA WHERE BRANCHCD IN (" + branchcd_cd + ") AND SUBSTR(ACODE,1,2) IN ('06','05') GROUP BY BRANCHCD ORDER BY BRANCHCD ";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        Box8_T01.Value = "0";
        if (dt.Rows.Count > 0)
        {
            Box8_T02.Value = (fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY").ToString().toDouble() > 0 ? fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY").ToString() + " Dr" : (fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY").ToString().toDouble() * -1) + " Cr");
            Box8_T03.Value = (fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY").ToString().toDouble() > 0 ? fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY").ToString() + " Dr" : (fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY").ToString().toDouble() * -1) + " Cr");
            Box8_T04.Value = (fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY").ToString().toDouble() > 0 ? fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY").ToString() + " Dr" : (fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY").ToString().toDouble() * -1) + " Cr");
            Box8_T01.Value = (fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY").ToString().toDouble() + fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY").ToString().toDouble() + fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY").ToString().toDouble()).ToString();
            Box8_T01.Value += (Box8_T01.Value.toDouble() > 0) ? " Dr" : " Cr";
            Box8_T01.Value = Box8_T01.Value.Replace("-", "");
        }

        //Finance total invn        
        SQuery = "SELECT ROUND(SUM(A.STKVALUE) / 100000,2) AS QTY,A.BRANCHCD FROM (Select sum(a.Closing_Stk*b.irate) as stkvalue,A.ICODE,A.BRANCHCD from (select sum(a.opening)||'~'||sum(a.cdr)||'~'||sum(a.ccr)||'~'||(Sum(a.opening)+sum(a.cdr)-sum(a.ccr))||'~'||sum(a.imin)||'~'||sum(a.imax)||'~'||sum(a.iord) AS ALLFLD,a.icode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stk,sum(a.imin) as imin,sum(a.imax) as imax,sum(a.iord) as iord,A.BRANCHCD from (Select branchcd,trim(icode) as icode,yr_" + frm_CDT1.Substring(6, 4) + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where BRANCHCD IN (" + branchcd_cd + ") union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr, 0 as aaa , 0 as aaa1,0 as aaa2 from IVOUCHER where BRANCHCD IN (" + branchcd_cd + ") and TYPE LIKE '%' AND VCHDATE " + DateRange + "  and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE,A.BRANCHCD) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) GROUP BY A.ICODE,A.BRANCHCD) A GROUP BY A.BRANCHCD";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            Box9_T02.Value = fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY");
            Box9_T03.Value = fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY");
            Box9_T04.Value = fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY");
        }
        Box9_T01.Value = (Box9_T02.Value.toDouble() + Box9_T03.Value.toDouble() + Box9_T04.Value.toDouble()).ToString();

        //Tot Revenue
        SQuery = "SELECT SUM(NET) AS QTY,BRANCHCD FROM RECDATA WHERE BRANCHCD IN (" + branchcd_cd + ") AND SUBSTR(ACODE,1,2) IN ('06','05') GROUP BY BRANCHCD ORDER BY BRANCHCD ";
        SQuery = "SELECT ROUND(SUM(IAMOUNT/100000),2) AS QTY,BRANCHCD FROM IVOUCHER WHERE BRANCHCD IN (" + branchcd_cd + ") AND TYPE LIKE '4%' AND TYPE NOT IN ('47','4A') AND VCHDATE " + DateRange + " GROUP BY BRANCHCD ORDER BY BRANCHCD ";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            Box10_T02.Value = fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY");
            Box10_T03.Value = fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY");
            Box10_T04.Value = fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY");
        }
        Box10_T01.Value = (Box10_T02.Value.toDouble() + Box10_T03.Value.toDouble() + Box10_T04.Value.toDouble()).ToString();

        //Cost (Total Freight)
        SQuery = "SELECT ROUND(SUM(DRAMT/100000),2) AS QTY,BRANCHCD FROM VOUCHER WHERE BRANCHCD IN (" + branchcd_cd + ") AND VCHDATE " + DateRange + " AND ACODE IN ('320011','500003') GROUP BY BRANCHCD ORDER BY BRANCHCD ";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            Box11_T02.Value = fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY");
            Box11_T03.Value = fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY");
            Box11_T04.Value = fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY");
        }
        Box11_T01.Value = (Box11_T02.Value.toDouble() + Box11_T03.Value.toDouble() + Box11_T04.Value.toDouble()).ToString();

        //Cost (Prv Mnth PPL)
        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT OPT_PARAM2 FROM FIN_RSYS_OPT WHERE opt_ID='W2021'", "OPT_PARAM2");
        if (col1 != "0")
        {
            col2 = "";
            if (col1.Contains(","))
            {
                foreach (string st in col1.Split(','))
                {
                    col2 += "," + "'" + st + "'";
                }
                col2 = col2.TrimStart(',');
            }
            else col2 = "'" + col1 + "'";
            SQuery = "SELECT ROUND(SUM(DRAMT/100000),2) AS QTY,BRANCHCD FROM VOUCHER WHERE BRANCHCD IN (" + branchcd_cd + ") AND TO_CHAR(VCHDATE,'MM/YYYY')=TO_CHAR(ADD_MONTHS(SYSDATE,-1),'MM/YYYY') AND ACODE IN (" + col2 + ") GROUP BY BRANCHCD ORDER BY BRANCHCD ";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count > 0)
            {
                Box12_T02.Value = fgen.seek_iname_dt(dt, "BRANCHCD='" + branchcode + "'", "QTY");
                Box12_T03.Value = fgen.seek_iname_dt(dt, "BRANCHCD='04'", "QTY");
                Box12_T04.Value = fgen.seek_iname_dt(dt, "BRANCHCD='08'", "QTY");
            }
            Box12_T01.Value = (Box12_T02.Value.toDouble() + Box12_T03.Value.toDouble() + Box12_T04.Value.toDouble()).ToString();
        }
    }
}