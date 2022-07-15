using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_tgpop_mst : System.Web.UI.Page
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
    string frm_mbr, frm_tid, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
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
                //chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2017'", "fstr");
                //if (chk_opt == "N")
                //{
                //    doc_GST.Value = "N";
                //}
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2027'", "fstr");
                if (chk_opt == "Y")
                //Member GCC Country
                {
                    doc_GST.Value = "GCC";
                }
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
            if (frm_ulvl != "0") btndel.Visible = false;
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
        switch (Prg_Id)
        {

            case "M12008":
                tab3.Visible = false;
                tab4.Visible = false;
                break;

            case "F60161":
                //AllTabs.Visible = false;
                break;
        }
        tab1.Visible = true;
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
        string tbl_id;
        tbl_id = "";
        doc_nf.Value = "VCHNUM";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "TYPEGRP";
        typePopup = "N";
        if (Prg_Id == "F40252" || Prg_Id == "F40254")
        {
            frm_tabname = "TYPEWIP";
            typePopup = "Y";
        }


        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        hcid.Value = Prg_Id;

        switch (Prg_Id)
        {
            case "F80116":
                tbl_id = "TP";
                lblheader.Text = "Training Topics Master";
                break;
            case "F70182":
                tbl_id = "DT";
                lblheader.Text = "Districts Master";
                lbl3.Visible = true; txtlbl3.Visible = true;
                txtlbl3.Attributes.Add("readonly", "readonly"); lbl3.InnerText = "Country";
                txtlbl4.Attributes.Add("readonly", "readonly");
                txtlbl5.Attributes.Add("readonly", "readonly");
                txtlbl6.Attributes.Add("readonly", "readonly");
                txtlbl7.Attributes.Add("readonly", "readonly");
                txtlbl8.Attributes.Add("readonly", "readonly");
                txtlbl9.Attributes.Add("readonly", "readonly");
                lbl4.Visible = true; txtlbl4.Visible = true; lbl4.InnerText = "State";
                Label6.Visible = true; txtlbl8.Visible = true;
                Label7.Visible = true; txtlbl9.Visible = true;
                btnPersonName.Visible = true;
                ImageButton1.Visible = true;
                break;
            case "F70183":
            case "F10139A":
            case "F10123":
                tbl_id = "CN";
                lblheader.Text = "Country Master";
                lbl3.InnerText = "Continent";
                if (Prg_Id == "F10139A")
                {
                    tbl_id = "WI";
                    lblheader.Text = "Plant WIP Stages Master";
                    lbl3.InnerText = "WIP Section Code*";
                }
                if (Prg_Id == "F10123")
                {
                    tbl_id = "KK";
                    lblheader.Text = "Process/Operation Master(Plant Wise)";
                    lbl3.InnerText = "WIP Section Code*";

                    Label2.Text = "Sheet/Crtn*";
                    Label3.Text = "Make Ready Time*";
                    Label4.Text = "Process Time/1000*";

                }

                lbl3.Visible = true; txtlbl3.Visible = true;
                txtlbl3.Attributes.Add("readonly", "readonly");


                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = true; txtlbl8.Visible = true;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = true;
                ImageButton1.Visible = false;
                break;



            case "F70193":
                tbl_id = "ES";
                lblheader.Text = "Native State Master";
                lbl3.Visible = true; txtlbl3.Visible = true;
                txtlbl3.Attributes.Add("readonly", "readonly");
                lbl3.InnerText = "Country";
                txtlbl5.Attributes.Add("readonly", "readonly");
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = true; txtlbl8.Visible = true;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = true;
                ImageButton1.Visible = false;
                break;
            case "F70194":
            case "F70184":
            case "F40252":
            case "F40254":
            case "F77101":
            case "F10100A":

                if (Prg_Id == "F77101")
                {
                    lblheader.Text = "Sales Segments(Govt/Pvt/NGO/Edu/FMCG)";
                    tbl_id = "SM";
                }

                if (Prg_Id == "F70184")
                {
                    lblheader.Text = "Continent Master";
                    tbl_id = "NM";
                }
                if (Prg_Id == "F70194")
                {
                    lblheader.Text = "Zone Master";
                    tbl_id = "ZO";
                }
                if (Prg_Id == "F40252")
                {
                    lblheader.Text = "Rejection Reason Master";
                    lbl3.InnerText = "WIP Stage";
                    tbl_id = "RJC";
                }
                if (Prg_Id == "F40254")
                {
                    lblheader.Text = "Downtime Reason Master";
                    lbl3.InnerText = "Wip Stage";
                    tbl_id = "DTC";
                }
                if (Prg_Id == "F10100A")
                {
                    lblheader.Text = "Item Classification for MIS/Valuation";
                    tbl_id = "YY";
                }

                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                break;
            case "F70701":
            case "F70702":
            case "F70703":
            case "F70704":
                switch (Prg_Id)
                {
                    case "F70701":
                        tbl_id = "L1";
                        lblheader.Text = "Cost Centers Level 1";
                        break;
                    case "F70702":
                        tbl_id = "L2";
                        lblheader.Text = "Cost Centers Level 2";
                        break;
                    case "F70703":
                        tbl_id = "L3";
                        lblheader.Text = "Cost Centers Level 3";
                        break;
                    case "F70704":
                        tbl_id = "L4";
                        lblheader.Text = "Business Segment/Groups";
                        break;
                }
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                break;
            case "F10127A":
            case "F10127B":
            case "F10127C":
            case "F10127D":
                switch (Prg_Id)
                {
                    case "F10127A":
                        tbl_id = "#4";
                        lblheader.Text = "Item Type Master (Dimension 1)";
                        break;
                    case "F10127B":
                        tbl_id = "#1";
                        lblheader.Text = "Item Application Master (Dimension 2)";
                        break;
                    case "F10127C":
                        tbl_id = "#2";
                        lblheader.Text = "Item Class Master (Dimension 3)";
                        break;
                    case "F10127D":
                        tbl_id = "#3";
                        lblheader.Text = "Item SubClass Master (Dimension 4)";
                        break;
                }
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                break;
            case "F45172":
                tbl_id = "$1";
                lblheader.Text = "Regional Sales Managers Master";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                break;

            case "F45174":
                tbl_id = "$2";
                lblheader.Text = "Area Sales Managers Master";
                lbl3.Visible = true; txtlbl3.Visible = true;
                txtlbl3.Attributes.Add("readonly", "readonly");
                lbl3.InnerText = "RSM_Name";
                txtlbl5.Attributes.Add("readonly", "readonly");
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = true; txtlbl8.Visible = true;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = true;
                ImageButton1.Visible = false;
                break;

            case "F45176":
                tbl_id = "EM";
                lblheader.Text = "Territory Sales Managers Master";
                lbl3.Visible = true; txtlbl3.Visible = true;
                txtlbl3.Attributes.Add("readonly", "readonly"); lbl3.InnerText = "ASM_Name";
                txtlbl4.Attributes.Add("readonly", "readonly");
                lbl4.Visible = true; txtlbl4.Visible = true; lbl4.InnerText = "RSM_Name";
                Label6.Visible = true; txtlbl8.Visible = true;
                Label7.Visible = true; txtlbl9.Visible = true;
                btnPersonName.Visible = true;
                ImageButton1.Visible = false;
                break;

            case "F10187"://MATL RATE MASTER
                tbl_id = "MM";
                lblheader.Text = "Material Master (Label Costing)";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                Label2.Text = "Price";
                break;

            case "F10193V": //VARNISH RATE MASTER////////////need to showing icon for SPPI
                tbl_id = "V1";
                lblheader.Text = "Varnish Master (Label Costing)";
                txtlbl5.MaxLength = 4;
                tab1.InnerText = "These are already created Varnish Master being showing below";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                Label2.Text = "Price";
                break;

            case "F10193Q"://QUALITY MASTER
                tbl_id = "QM";
                lblheader.Text = "Quality Master (Label Costing)";
                txtlbl5.MaxLength = 4;
                tab1.InnerText = "These are already created Quality Master being showing below";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                Label2.Text = "Price";
                break;

            case "F10125":
                #region
                tbl_id = "BN";
                lblheader.Text = "Rack / Bin / Location Master";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;


            case "F61110":// Complaint Types
                tbl_id = "E1";
                lblheader.Text = "Customer Complaint Types";
                tab1.InnerText = "These are already created Customer Complaint types being showing below";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                Label2.Text = "Remark";
                break;

            case "F61111":// Complaint Types
                tbl_id = "E2";
                lblheader.Text = "Complaint Analysis Types";
                tab1.InnerText = "These are already created Analysis types being showing below";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                Label2.Text = "Remark";
                break;

            case "F61112":// Categories / priority
                tbl_id = "E3";
                lblheader.Text = "Complaint Catagories/priority Types";
                tab1.InnerText = "These are already created Catagories types being showing below";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                Label2.Text = "Remark";
                break;




            case "F10206":// FOIL MASTER
                tbl_id = "^O";//"^M" pehle tha ab change kiya hai
                lblheader.Text = "Foil Master (Label Costing)";
                txtlbl5.MaxLength = 4;
                tab1.InnerText = "These are already created Foil Master being showing below";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                Label2.Text = "Price";
                break;

            case "F10207":// LAMINATION MASTER
                tbl_id = "^N";
                lblheader.Text = "Lamination Master (Label Costing)";
                txtlbl5.MaxLength = 4;
                tab1.InnerText = "These are already created Lamination Master being showing below";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                Label2.Text = "Price";
                break;

            case "F70174":
                tbl_id = "A";
                lblheader.Text = "Accounts Schedules Master";
                txtlbl3.Attributes.Add("readonly", "readonly");
                txtlbl4.Attributes.Add("readonly", "readonly");
                break;

            case "F70177":
                #region
                tbl_id = "T1";
                lblheader.Text = "GST/VAT Rates Master";
                lbl3.InnerText = "HS Code";
                lbl4.InnerText = "Goods/Service(G/S)";
                Label2.Text = "CGST %";
                Label3.Text = "SGST %";
                Label4.Text = "IGST %";
                Label6.InnerText = "Cess %";
                if (doc_GST.Value == "GCC")
                {
                    lblheader.Text = "VAT Rates Master";
                    lbl3.InnerText = "VAT Code";
                    lbl4.InnerText = "Goods/Service(G/S)";
                    Label2.Text = "..";
                    Label3.Text = "..";
                    Label4.Text = "VAT %";
                    Label6.InnerText = "Cess %";
                }
                Label7.InnerText = "Taxable(Y/N)";
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;

                txtlbl4.MaxLength = 1;
                txtlbl9.MaxLength = 1;                
                #endregion
                break;
            case "F10138":
                #region
                tbl_id = "MI";
                lblheader.Text = "Mill Master";
                lbl3.InnerText = "Mill Short Name";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;
            case "F10139":
                #region
                tbl_id = "J1";
                lblheader.Text = "Color Master";
                lbl3.InnerText = "No. of Colors";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;

            case "F45165":
            case "F45167":
            case "F45168":
            case "F45169":
            case "F45179":
                #region
                tbl_id = "SL";
                lblheader.Text = "Lead Action Master";
                if (Prg_Id == "F45168")
                { tbl_id = "ST"; lblheader.Text = "Lead Source Master"; }
                if (Prg_Id == "F45167")
                { tbl_id = "SU"; lblheader.Text = "Contact Status Master"; }
                if (Prg_Id == "F45169")
                { tbl_id = "SI"; lblheader.Text = "Industry Type Master"; }
                if (Prg_Id == "F45179")
                { tbl_id = "SN"; lblheader.Text = "Contact Level Master"; }

                lbl3.InnerText = "Status_Tag";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;

            case "F10137":
                #region
                tbl_id = "ZF";
                lblheader.Text = "Ply Flute Master";
                lbl3.InnerText = "Name";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;

            case "F70420":
                #region
                tbl_id = "FA";
                lblheader.Text = "Fixed Asset Master(Companies Act)";
                lbl3.InnerText = "Sch_Code"; txtlbl5.Visible = false;
                Label2.Visible = false; txtlbl15.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                txtlbl4.Visible = false; lbl4.Visible = false;
                lbl3.Visible = false; txtlbl3.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;

            case "F70421":
                #region
                tbl_id = "LF";
                lblheader.Text = "Fixed Asset Location Master";
                lbl3.InnerText = "Locn_Code"; txtlbl5.Visible = false;
                Label2.Visible = false; txtlbl15.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;

            case "F70419":
                #region
                tbl_id = "DI";
                lblheader.Text = "Income Tax Block Master";
                lbl3.InnerText = "Block Code";
                Label2.Text = " Dep %";
                Label3.Text = "Add. Dep. %";
                Label3.Text = "Add. Dep. %"; txtlbl4.Visible = true;
                Label4.Text = "Op_Block_WDV"; txtlbl7.Visible = true;
                lbl3.Visible = false; txtlbl3.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;

            case "F70437":// can be deleted mg 16.8.20
                #region
                //tbl_id = "ZX";
                //lblheader.Text = "Income Tax Block Master Opening WDV";
                //txtlbl2.Attributes.Add("readonly", "readonly");
                //txtlbl4.Attributes.Add("readonly", "readonly");
                //txtlbl5.Attributes.Add("readonly", "readonly");
                //txtlbl6.Attributes.Add("readonly", "readonly");
                //lbl3.InnerText = "Block Code";
                //Label2.Text = " Dep %";
                //Label3.Text = "Add. Dep. %"; txtlbl4.Visible = true;
                //Label4.Text = "Op_Block_WDV"; txtlbl7.Visible = true;
                //txtlbl3.Visible = false; lbl4.Visible = false;
                //Label6.Visible = false; txtlbl8.Visible = false;
                //Label7.Visible = false; txtlbl9.Visible = false;
                //ImageButton1.Visible = false;
                #endregion
                break;

            case "F10136":
                #region
                tbl_id = "FU";
                lblheader.Text = "Flute Master";
                Label5.InnerText = "Flute_Name";
                lbl3.Visible = false;
                Label2.Text = "% Extra";
                Label3.Text = "Height";
                Label4.Text = "Other Ref"; txtlbl7.Visible = true;
                Label4.Visible = true;
                txtlbl4.Visible = false; lbl4.Visible = false;
                lbl3.Visible = false; txtlbl3.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;

            case "F85133":
                #region
                tbl_id = "HD";
                lblheader.Text = "Designation Master";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label2.Visible = false; Label3.Visible = false; Label4.Visible = false;
                Label6.Visible = false; Label7.Visible = false;
                txtlbl5.Visible = false; txtlbl6.Visible = false; txtlbl7.Visible = false;
                txtlbl8.Visible = false; txtlbl9.Visible = false;
                #endregion
                break;
            case "F85155":
                #region
                tbl_id = "HT";
                lblheader.Text = "Department Master";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label2.Visible = false; Label3.Visible = false; Label4.Visible = false;
                Label6.Visible = false; Label7.Visible = false;
                txtlbl5.Visible = false; txtlbl6.Visible = false; txtlbl7.Visible = false;
                txtlbl8.Visible = false; txtlbl9.Visible = false;
                #endregion
                break;

            case "F10554":
                #region
                lblheader.Text = "Visit Type Master";
                tbl_id = "VC";
                lbl3.Visible = false; divPersonName.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; ImageButton1.Visible = false; txtlbl4.Visible = false;
                Label2.Visible = false; txtlbl5.Visible = false; Label3.Visible = false; txtlbl6.Visible = false;
                Label2.Visible = false; txtlbl5.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                #endregion
                break;
            case "F10555":
                #region
                lblheader.Text = "Information Master";
                tbl_id = "IM";
                lbl3.Visible = false; divPersonName.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; ImageButton1.Visible = false; txtlbl4.Visible = false;
                Label2.Visible = false; txtlbl5.Visible = false; Label3.Visible = false; txtlbl6.Visible = false;
                Label2.Visible = false; txtlbl5.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = true; txtlbl8.Visible = true;
                Label7.Visible = false; txtlbl9.Visible = false;
                #endregion
                break;
            case "F10556":
                #region
                lblheader.Text = "Expense Master";
                tbl_id = "EB";
                lbl3.Visible = false; divPersonName.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; ImageButton1.Visible = false; txtlbl4.Visible = false;
                Label2.Visible = false; txtlbl5.Visible = false; Label3.Visible = false; txtlbl6.Visible = false;
                Label2.Visible = false; txtlbl5.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                #endregion
                break;
            case "F40061":
                #region
                lblheader.Text = "Shift Master";
                tbl_id = "SF";
                #endregion
                break;

            case "F40062":
                lblheader.Text = "Fixture Details";
                tbl_id = "FX";
                break;

            case "F10308":
                #region
                tbl_id = "^7";
                lblheader.Text = "Area Master";
                lbl3.InnerText = "Name";
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label2.Visible = false; txtlbl5.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;

            case "F10129":
                #region
                tbl_id = "^8";
                lblheader.Text = "Family Master";
                txtlbl5.Visible = false;
                Label2.Visible = false; txtlbl15.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;

            case "F55524":
                #region
                tbl_id = "^9";
                lblheader.Text = "Forwarding Agent Master";
                lbl3.InnerText = "Account Code";
                txtlbl5.Visible = false;
                Label2.Visible = false; txtlbl15.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;

            case "F55525":
                #region
                tbl_id = "^A";
                lblheader.Text = "Shipping Line Master";
                lbl3.InnerText = "Account Code";
                txtlbl5.Visible = false;
                Label2.Visible = false; txtlbl15.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;

            case "F55526":
                #region
                tbl_id = "^B";
                lblheader.Text = "Nature Of Shipment";
                txtlbl5.Visible = false;
                Label2.Visible = false; txtlbl15.Visible = false;
                Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                lbl3.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; txtlbl4.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;
                #endregion
                break;

            case "F10061":
                #region
                tbl_id = "CM";
                lblheader.Text = "Complaint Master";
                lbl3.Visible = false; divPersonName.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; ImageButton1.Visible = false; txtlbl4.Visible = false;
                Label2.Visible = false; txtlbl5.Visible = false; Label3.Visible = false; txtlbl6.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                #endregion
                break;

            case "F10062":
                #region
                tbl_id = "TC";
                lblheader.Text = "Complaint Type Master";
                lbl3.Visible = false; divPersonName.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; ImageButton1.Visible = false; txtlbl4.Visible = false;
                Label2.Visible = false; txtlbl5.Visible = false; Label3.Visible = false; txtlbl6.Visible = false;
                Label2.Visible = false; txtlbl5.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                #endregion
                break;

            case "F10063":
                #region
                tbl_id = "DC";
                lblheader.Text = "Complaint Division Master";
                lbl3.Visible = false; divPersonName.Visible = false; txtlbl3.Visible = false;
                lbl4.Visible = false; ImageButton1.Visible = false; txtlbl4.Visible = false;
                Label2.Visible = false; txtlbl5.Visible = false; Label3.Visible = false; txtlbl6.Visible = false;
                Label2.Visible = false; txtlbl5.Visible = false;
                Label4.Visible = false; txtlbl7.Visible = false;
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;
                #endregion
                break;

            case "F45166":
                #region
                tbl_id = "^C";
                lblheader.Text = "Lead Category Master";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label2.Visible = false; Label3.Visible = false; Label4.Visible = false;
                Label6.Visible = false; Label7.Visible = false;
                txtlbl5.Visible = false; txtlbl6.Visible = false; txtlbl7.Visible = false;
                txtlbl8.Visible = false; txtlbl9.Visible = false;
                #endregion
                break;

            case "F45151":
                #region
                tbl_id = "^V";
                lblheader.Text = "Lead Action Stage Master";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label2.Visible = false; Label3.Visible = false; Label4.Visible = false;
                Label6.Visible = false; Label7.Visible = false;
                txtlbl5.Visible = false; txtlbl6.Visible = false; txtlbl7.Visible = false;
                txtlbl8.Visible = false; txtlbl9.Visible = false;
                #endregion
                break;
            //^D IS USED FOR INDUSTRY MASTER ...WRITTEN IN MAIN
            case "F30365":
                #region
                tbl_id = "^E";
                lblheader.Text = "Machine Master";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                Label2.Text = "Machine Make";
                Label3.Text = "Machine Model";
                Label5.InnerText = "Machine Sr. No.";
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label4.Visible = false;
                Label6.Visible = false; Label7.Visible = false;
                txtlbl7.Visible = false;
                txtlbl8.Visible = false; txtlbl9.Visible = false;
                #endregion
                break;

            case "F30366":
                #region
                tbl_id = "^F";
                lblheader.Text = "Chemical Grade Master";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                Label2.Text = "Cu";
                Label3.Text = "Ni";
                Label5.InnerText = "Grade";
                Label4.Text = " Cr";
                Label6.InnerText = "Mo";
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label7.Visible = false;
                txtlbl9.Visible = false;
                txtlbl5.MaxLength = 10;
                txtlbl6.MaxLength = 10;
                txtlbl7.MaxLength = 10;
                txtlbl8.MaxLength = 10;
                #endregion
                break;
            //for SPPI....CREATE BY YOGITA........17JAN20
            case "F10200":
            case "F10201":
            case "F10202":
            case "F10203":
            case "F10204":
                #region
                if (Prg_Id == "F10200")
                {
                    tbl_id = "^G";
                    lblheader.Text = "Plate Unit Master";
                }
                else if (Prg_Id == "F10201")
                {
                    tbl_id = "^H";
                    lblheader.Text = "Ink Master";
                }
                else if (Prg_Id == "F10202")
                {
                    tbl_id = "^I";
                    lblheader.Text = "Die Master";
                }
                else if (Prg_Id == "F10203")
                {
                    tbl_id = "^J";
                    lblheader.Text = "Embossing Varnish Master";
                }
                else if (Prg_Id == "F10204")
                {
                    tbl_id = "^K";
                    lblheader.Text = "Embossing White/Screen Printing Master";
                }
                lbl3.Visible = false; lbl4.Visible = false;
                Label5.InnerText = "Name";
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                Label2.Text = "Value";
                txtlbl6.MaxLength = 10;
                Label3.Visible = false;
                txtlbl3.Visible = false;
                txtlbl6.Visible = false;//
                Label4.Visible = false;
                txtlbl4.Visible = false;
                txtlbl7.Visible = false;
                Label6.Visible = false;
                txtlbl8.Visible = false;
                Label7.Visible = false;
                txtlbl9.Visible = false;
                #endregion
                break;

            case "F30368":
                tbl_id = "^L";
                lblheader.Text = "Defect Master";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
                Label5.InnerText = "Defect Name";
                Label4.Visible = false;
                Label6.Visible = false;
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label7.Visible = false;
                txtlbl9.Visible = false;
                txtlbl5.Visible = false; txtlbl6.Visible = false;
                txtlbl7.Visible = false; txtlbl8.Visible = false;
                break;

            case "F55252":
                tbl_id = "WD";
                lblheader.Text = "Drawing Design Type Master";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
                Label5.InnerText = "Design Type ";
                Label4.Visible = false;
                Label6.Visible = false;
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label7.Visible = false;
                txtlbl9.Visible = false;
                txtlbl5.Visible = false; txtlbl6.Visible = false;
                txtlbl7.Visible = false; txtlbl8.Visible = false;
                break;
            case "F55254":
                tbl_id = "WT";
                lblheader.Text = "Drawing Type Master";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                //Label2.Visible = false;
                //Label3.Visible = false;
                Label5.InnerText = "Drawing Type";
                //Label4.Visible = false;
                Label6.Visible = false;
                txtlbl3.Visible = false;
                txtlbl4.Visible = false;

                Label7.Visible = false;
                txtlbl9.Visible = false;
                //txtlbl5.Visible = false; txtlbl6.Visible = false;
                //txtlbl7.Visible = false; 
                txtlbl8.Visible = false;
                break;
            case "F39501":
                tbl_id = "^P";
                lblheader.Text = "Zone Master- Production";// for line production svpl
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
                Label5.InnerText = "Zone Type";
                Label4.Visible = false;
                Label6.Visible = false;
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label7.Visible = false;
                txtlbl9.Visible = false;
                txtlbl5.Visible = false; txtlbl6.Visible = false;
                txtlbl7.Visible = false; txtlbl8.Visible = false;
                break;
            case "F39502":
                tbl_id = "^Q";
                lblheader.Text = "Line Master";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
                Label5.InnerText = "Line Type";
                Label4.Visible = false;
                Label6.Visible = false;
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label7.Visible = false;
                txtlbl9.Visible = false;
                txtlbl5.Visible = false; txtlbl6.Visible = false;
                txtlbl7.Visible = false; txtlbl8.Visible = false;
                break;
            case "F39503":
                tbl_id = "^R";
                lblheader.Text = "Shift Incharge Master";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
                Label5.InnerText = "Incharge Name";
                Label4.Visible = false;
                Label6.Visible = false;
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label7.Visible = false;
                txtlbl9.Visible = false;
                txtlbl5.Visible = false; txtlbl6.Visible = false;
                txtlbl7.Visible = false; txtlbl8.Visible = false;
                break;
            case "F39504":
                tbl_id = "^S";
                lblheader.Text = "Supervisor Master";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
                Label5.InnerText = "Supervisor Name";
                Label4.Visible = false;
                Label6.Visible = false;
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label7.Visible = false;
                txtlbl9.Visible = false;
                txtlbl5.Visible = false; txtlbl6.Visible = false;
                txtlbl7.Visible = false; txtlbl8.Visible = false;
                break;
            case "F39505":
                tbl_id = "^T";
                lblheader.Text = "Loss Code Master";
                lbl3.Visible = true; lbl4.Visible = false;
                btnPersonName.Visible = true; ImageButton1.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
                Label5.InnerText = "Code_Name";
                Label4.Visible = false;
                Label6.Visible = false;
                txtlbl3.Visible = true; txtlbl4.Visible = false; txtlbl3.Disabled = true;
                Label7.Visible = false;
                txtlbl9.Visible = false;
                txtlbl5.Visible = false; txtlbl6.Visible = false;
                txtlbl7.Visible = false; txtlbl8.Visible = false;
                break;
            case "F39506"://not using in svpl created for their production midule loss code is handling now
                tbl_id = "^U";
                lblheader.Text = "Loss Type Master";
                lbl3.Visible = true; lbl4.Visible = false; lbl3.InnerText = "Type_Code";
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                Label2.Visible = false;
                Label3.Visible = false;
                Label5.InnerText = "Type_Name";
                Label4.Visible = false;
                Label6.Visible = false;
                txtlbl3.Visible = true; txtlbl4.Visible = false;
                Label7.Visible = false;
                txtlbl9.Visible = false;
                txtlbl5.Visible = false; txtlbl6.Visible = false;
                txtlbl7.Visible = false; txtlbl8.Visible = false;
                break;

            case "F55256":
                tbl_id = "C1";
                lblheader.Text = "Customer Master (For Drawing Module)";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label5.InnerText = "Customer Name";
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;

                Label2.Text = "Contact.No";
                Label3.Text = "Address";
                Label4.Text = "Finsys_Code";
                break;
            case "F55257":
                tbl_id = "P1";
                lblheader.Text = "Product Master (For Drawing Module)";
                lbl3.Visible = false; lbl4.Visible = false;
                btnPersonName.Visible = false; ImageButton1.Visible = false;
                txtlbl3.Visible = false; txtlbl4.Visible = false;
                Label5.InnerText = "Product Name";
                Label6.Visible = false; txtlbl8.Visible = false;
                Label7.Visible = false; txtlbl9.Visible = false;

                Label2.Text = "Modal.Number";
                Label3.Text = "Part.Number";
                Label4.Text = "Drg.Number";
                break;
        }

        if (frm_formID == "F55257" || frm_formID == "F55256" || frm_formID == "F55254") { }
        else txtlbl5.Attributes.Add("onkeypress", "return isDecimalKey(event)");

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TID", tbl_id);
        dt = new DataTable();
        switch (Prg_Id)
        {
            case "F70177":
                if (doc_GST.Value == "GCC") dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1,Name,acref as param1,acref2 as Good_Servc,dpt as Taxable,num6 as Vat_Rate,num7 as Cess,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,EDT_DT from " + frm_tabname + " where BRANCHCD!='DD' AND id='" + tbl_id + "' order by type1 ");
                else dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1,Name,acref as param1,acref2 as Good_Servc,dpt as Taxable,num4 as CGST,num5 as SGST,num6 as IGST,num7 as Cess,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,EDT_DT from " + frm_tabname + " where BRANCHCD!='DD' AND id='" + tbl_id + "' order by type1 ");
                break;
            case "F40252":
            case "F40254":
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1,Name,acref as param1,acref2 as param2,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,EDT_DT from " + frm_tabname + " where BRANCHCD='" + frm_mbr + "' AND id='" + ID_TWIP.Value + "' order by type1 ");
                break;

            case "F70419":
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1,Name,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,EDT_DT from " + frm_tabname + " where BRANCHCD!='DD' AND id='" + tbl_id + "' order by type1 ");
                break;
            case "F70420":
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1,Name,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,EDT_DT from " + frm_tabname + " where BRANCHCD!='DD' AND id='" + tbl_id + "' order by type1 ");
                break;
            case "F70421":
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1,Name,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,EDT_DT from " + frm_tabname + " where BRANCHCD='" + frm_mbr + "' AND id='" + tbl_id + "' order by type1 ");
                break;
            case "F10136":
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1,Name,acref as perc_extra,lineno as flute_height,acref3 as param3,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,EDT_DT from " + frm_tabname + " where BRANCHCD!='DD' AND id='" + tbl_id + "' order by type1 ");
                break;
            case "F10129":
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1 AS CODE,Name,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,(case when nvl(trim(edt_by),'-')='-' then '-' else to_char(EDT_DT,'dd/mm/yyyy') end) as edt_dt from " + frm_tabname + " where BRANCHCD='" + frm_mbr + "' AND id='" + tbl_id + "' order by type1 ");
                break;
            case "F55524":
            case "F55525":
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select a.ID,a.Type1 AS CODE,a.Name,a.acref as acc_code,f.aname as acc_name,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.EDT_BY,(case when nvl(trim(a.edt_by),'-')='-' then '-' else to_char(a.EDT_DT,'dd/mm/yyyy') end) as edt_dt from " + frm_tabname + " a,famst f where trim(a.acref)=trim(f.acode) and a.BRANCHCD='" + frm_mbr + "' AND a.id='" + tbl_id + "' order by code");
                break;
            case "F55526":
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1 AS CODE,Name,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,(case when nvl(trim(edt_by),'-')='-' then '-' else to_char(EDT_DT,'dd/mm/yyyy') end) as edt_dt from " + frm_tabname + " where BRANCHCD='" + frm_mbr + "' AND id='" + tbl_id + "' order by type1 ");
                break;
            case "F85133":
                dt = fgen.getdata(frm_qstr, frm_cocd, "select id,type1 as code,name as Designation from " + frm_tabname + " where id='" + tbl_id + "' order by code");
                break;
            case "F45151":
            case "F45166":
                dt = fgen.getdata(frm_qstr, frm_cocd, "select id,type1 as code,name,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt from " + frm_tabname + " where id='" + tbl_id + "' order by code");
                break;
            case "F10193V":
            case "F10193Q":
                //Select Name,acref3 as price,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,to_char(EDT_DT,'dd/mm/yyyy') as edt_dt from TYPEGRP where BRANCHCD!='DD' AND id='V1'  order by type1 
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select Name,acref3 as price,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,to_char(EDT_DT,'dd/mm/yyyy') as edt_dt  from " + frm_tabname + " where id='" + tbl_id + "' order by Name");
                break;
            case "F30365":
                dt = fgen.getdata(frm_qstr, frm_cocd, "select id,type1 as code,name as machine_srno,acref as machine_make,acref2 as machine_model,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt from " + frm_tabname + " where id='" + tbl_id + "' order by code");
                break;
            case "F30366":
                dt = fgen.getdata(frm_qstr, frm_cocd, "select id,type1 as code,name as grade,acref as cu,acref2 as ni,acref3 as cr,p_acode as mo,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt from " + frm_tabname + " where id='" + tbl_id + "' order by code");
                break;
            case "F30368":
                dt = fgen.getdata(frm_qstr, frm_cocd, "select id,type1 as code,name as defect,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt from " + frm_tabname + " where id='" + tbl_id + "' order by code");
                break;
            case "F10206":
            case "F10207":
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select Name,acref3 as price,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,to_char(EDT_DT,'dd/mm/yyyy') as edt_dt  from " + frm_tabname + " where id='" + tbl_id + "' order by Name");
                break;
            case "F10139A":
            case "F10123":
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1,Name,acref as param1,acref2 as param2,acref3 as param3,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,to_char(EDT_DT,'dd/mm/yyyy') as edt_dt from " + frm_tabname + " where BRANCHCD='" + frm_mbr + "' AND id='" + tbl_id + "' " + cond + " order by type1 ");
                break;
            case "F55256":
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1,trim(Name) as Name,trim(acref) as contact_no,trim(acref2) as address,trim(acref3) as finsys_code,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,to_char(EDT_DT,'dd/mm/yyyy') as edt_dt from " + frm_tabname + " where BRANCHCD!='DD' AND id='" + tbl_id + "' " + cond + " order by type1 ");
                break;
            case "F55257":
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1,trim(Name) as Name,trim(acref) as modal_no,trim(acref2) as part_number,trim(acref3) as drg_no,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,to_char(EDT_DT,'dd/mm/yyyy') as edt_dt from " + frm_tabname + " where BRANCHCD!='DD' AND id='" + tbl_id + "' " + cond + " order by type1 ");
                break;

            default:
                cond = "";
                if (frm_formID == "F10555")
                    cond = " AND TYPE1<'100'";
                dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1,trim(Name) as Name,trim(acref) as param1,trim(acref2) as param2,trim(acref3) as param3,trim(acref4) as param4,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,to_char(EDT_DT,'dd/mm/yyyy') as edt_dt from " + frm_tabname + " where BRANCHCD!='DD' AND id='" + tbl_id + "' " + cond + " order by type1 ");
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
        frm_tid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        if (CSR.Length > 1) cond = " and trim(a.ccode)='" + cond + "'";
        switch (btnval)
        {
            case "ACTGCODE":
                switch (Prg_Id)
                {
                    case "F10139A":
                        SQuery = "select coded as fstr,coded as Code_No_Available,max(name) as Code_Name,(case when sum(Valu)>0 then 'Code Available' else 'Code Already Used' end) as Code_Status from (select '6'||lpad(trim(to_char(rownum,'9')),1,'0') as coded,1 as Valu,null as name from (select rowid,rownum from FIN_MSYS order by id) where rownum<10 union all select TRIM(aCREF) AS ACREF,-1 as coded,name from typegrp where branchcd='" + frm_mbr + "' and id='WI') group by coded HAVING sum(Valu)>0 order by Coded";
                        break;
                    case "F10123":
                        SQuery = "select trim(acref) as fstr,name as WIP_STG_name,trim(acref) as WIP_STG_Code,TYPE1 as code from typegrp where branchcd='" + frm_mbr + "' and id ='WI' order by Acref";
                        break;

                    case "F45174":
                        SQuery = "select trim(type1)||':'||trim(upper(name)) as fstr,trim(type1)||':'||trim(upper(name)) as RSM_Name,TYPE1 as code from typegrp where id ='$1' order by trim(type1)||':'||trim(upper(name))";
                        break;
                    case "F45176":
                        SQuery = "select trim(type1)||':'||trim(upper(name)) as fstr,trim(type1)||':'||trim(upper(name)) as ASM_Name,trim(acref) as RSM_Name,TYPE1 as code from typegrp where id ='$2' order by trim(type1)||':'||trim(upper(name))";
                        break;

                    case "F70174":
                        SQuery = "select trim(type1) as fstr,Name as Grp_Name,Type1 as Code,substr(acode,1,2) as grps from Type where id='Z' order by type1";
                        break;
                    //case "F70437":
                    //    SQuery = "select trim(type1)||'~'||nvl(num4,0)||'~'||nvl(num5,0)||'~'||nvl(trim(Name),'-') as fstr,trim(Name) as Block_Name,Type1 as Block_Code from Typegrp where id='DI' and branchcd||trim(type1) not in (select branchcd||trim(acref2) from typegrp where id='ZX') order by type1";
                    //    break;
                    case "F55524":
                    case "F55525":
                        SQuery = "select trim(acode) as fstr,acode as code,aname as name from famst where substr(trim(acode),1,2)='06' order by name";
                        break;
                    case "F39505":
                        SQuery = "select trim(acref) as fstr,TYPE1 as code,name as name,trim(acref) as Acode from typegrp where id ='^U' order by name";
                        break;
                    case "F70183":
                        SQuery = "select trim(Name)||'~'||nvl(trim(type1),'-') as fstr,name as name,TYPE1 as code,trim(acref) as Acode from typegrp where id ='NM' order by name";
                        break;
                    case "F70182":
                        SQuery = "select trim(Name)||'~'||nvl(trim(TYPE1),'-') as fstr,name as name,TYPE1 as code,trim(acref) as Acode from typegrp where id ='CN' order by name";
                        //union all select '00H'||'~'||'Home Country' as fstr,'00H' as code,'1.Home Country' as name,'-' as acode from dual order by name";
                        break;
                    case "F70193":
                        SQuery = "select trim(Name)||'~'||nvl(trim(Type1),'-') as fstr,name as Name,TYPE1 as code,trim(acref) as Acode from typegrp where id ='CN' order by name";
                        break;
                    default:
                        SQuery = "select trim(Acode) as fstr,Aname as Account_Name,Acode as Code,substr(acode,1,2) as grps from famst where length(trim(nvl(deac_by,'-'))) <2 and substr(acode,1,2) in ('03','12') order by grps,Aname";
                        break;
                }
                break;
            case "TYPECODE":
                switch (Prg_Id)
                {
                    case "F70182":

                        SQuery = "select trim(Name)||'~'||nvl(trim(type1),'-') as fstr,trim(name) as name,TYPE1 as code,trim(acref) as Acode from typegrp where id ='ES' and trim(acref)='" + txtlbl3.Value.ToString().Trim() + "' order by name";
                        break;
                    default:
                        SQuery = "select * from (select sum(Valu) as fstr,coded as Code_No_Available,max(name) as Code_Name,(case when sum(Valu)>0 then 'Code Available' else 'Code Already Used' end) as Code_Status from (select '" + txtlbl3.Value.Trim() + "'||lpad(trim(to_char(rownum,'99')),2,'0') as coded,1 as Valu,null as name from (select rowid,rownum from FIN_MSYS order by id) where rownum<100 union all select trim(type1) as type1,-1 as coded,name from typegrp where branchcd='00' and  id='" + frm_tid + "' and  substr(type1,1,2)='" + txtlbl3.Value.Trim() + "' ) group by coded ) where 1=1 order by Code_No_Available ";
                        break;
                }
                break;
            case "BTN_23":

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
                cond = "";
                if (frm_formID == "F10555")
                    cond = " AND a.TYPE1<'100'";
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    frm_tid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TID");
                SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt,a.type1,a.Name,a.acref as param1,a.acref2 as param2,a.acref3 as param3,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where  a.branchcd='" + frm_mbr + "' and trim(a.ID)='" + frm_tid + "' " + cond + " order by vdd desc,a." + doc_nf.Value + " desc";

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

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        if (Prg_Id == "F70421" || Prg_Id == "F10123" || Prg_Id == "F10139A" || Prg_Id == "F10125" || Prg_Id == "F40252" || Prg_Id == "F40254")
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

        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        frm_tid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TID");
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";
            txtlbl2.Focus();



            if (typePopup == "N") newCase(frm_tid);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
            // else comment upper code

            //frm_vnum = fgen.next_no(frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_tid + "' AND VCHDATE " + DateRange + " ", 6, "VCH" );
            //txtvchnum.Value = frm_vnum;
            //txtvchdate.Value = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            disablectrl();
            //fgen.EnableForm(this.Controls);
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
        #region
        if (col1 == "") return;
        frm_tid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TID");

        if (frm_formID == "F10555")
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND ID='" + frm_tid + "' AND TYPE1<'100' ", 6, "VCH");
        else frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND ID='" + frm_tid + "' ", 6, "VCH");

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


        if (Prg_Id == "F40252" || Prg_Id == "F40254")
        {
            txtlbl5.Value = col1;
            txtlbl6.Value = col2;
            ID_TWIP.Value = frm_tid + col1;
            set_Val();
        }

        #endregion
    }
    //------------------------------------------------------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        if (Prg_Id == "F70421" || Prg_Id == "F10123" || Prg_Id == "F10139A" || Prg_Id == "F10125" || Prg_Id == "F40252" || Prg_Id == "F40254")
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
        //save_Click
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }


        if (Prg_Id == "F70421" || Prg_Id == "F10123" || Prg_Id == "F10139A" || Prg_Id == "F10125" || Prg_Id == "F40252" || Prg_Id == "F40254")
        {
        }
        else if (frm_mbr != "00")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please operate this Option from Plant Code 00 !!");
            return;
        }

        if (Prg_Id == "F70177")
        {
            if (doc_GST.Value == "GCC")
            {

            }
            else
            {
                if (fgen.make_double(txtlbl5.Value.ToUpper().Trim()) + fgen.make_double(txtlbl6.Value.ToUpper().Trim()) != fgen.make_double(txtlbl7.Value.ToUpper().Trim()))
                {
                    fgen.msg("-", "AMSG", "CGST% + SGST% should be equal to IGST% !!");
                    return;
                }
            }
        }

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Value.ToString());
        if (dhd == 0) { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        if (txtlbl2.Value.Trim().Length < 1 || txtlbl2.Value.Trim() == "-")
        {
            fgen.msg("-", "AMSG", " Please Enter Name of This Master Type !!");
            return;
        }
        if (txtlbl3.Value.Trim().Length < 2 && (Prg_Id == "F55524" || Prg_Id == "F55525"))
        {
            fgen.msg("-", "AMSG", "Please Select Account Code !!");
            return;
        }
        if (txtlbl3.Value.Trim().Length < 2 && Prg_Id == "F70174")
        {
            fgen.msg("-", "AMSG", "Please Select Account Group !!");
            return;
        }
        if (txtlbl3.Value.Trim().Length < 2 && Prg_Id == "F45174")
        {
            fgen.msg("-", "AMSG", "Please Select RSM Name !!");
            return;
        }
        if (txtlbl3.Value.Trim().Length < 2 && Prg_Id == "F45176")
        {
            fgen.msg("-", "AMSG", "Please Select ASM Name !!");
            return;
        }

        if (txtlbl3.Value.Trim().Length <= 1 && Prg_Id == "F10139A")
        {
            fgen.msg("-", "AMSG", "Please Select WIP Stage Code !!");
            return;
        }
        if (txtlbl3.Value.Trim().Length <= 1 && Prg_Id == "F10123")
        {
            fgen.msg("-", "AMSG", "Please Select WIP Stage Code !!");
            return;
        }


        if (txtlbl4.Value.Trim().Length < 2 && Prg_Id == "F70174")
        {
            fgen.msg("-", "AMSG", "Please Select Schedule Code !!");
            return;
        }
        if (txtlbl5.Value.Trim().toDouble() <= 0 && Prg_Id == "F70419")
        {
            fgen.msg("-", "AMSG", "Please Enter Rate of Depreciation !!"); txtlbl5.Focus();
            return;
        }
        if ((txtlbl7.Value == "" || txtlbl7.Value == "-") && Prg_Id == "F70419")
        {
            fgen.msg("-", "AMSG", "Please Enter block WDV Value, else put 0 !!");
            return;
        }

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }
        if (Prg_Id == "F55524")
        {
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT DISTINCT TRIM(ACREF) AS CODE FROM TYPEGRP WHERE BRANCHCD='" + frm_mbr + "' AND ID='^9' AND ACREF='" + txtlbl3.Value + "'");
            if (dt.Rows.Count > 0)
            {
                fgen.msg("-", "AMSG", lblheader.Text + " Exists Already For This Account.'13'Please Check");
                return;
            }
        }
        if (Prg_Id == "F10125")
        {
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT DISTINCT TRIM(name) AS CODE FROM TYPEGRP WHERE BRANCHCD='" + frm_mbr + "' AND ID='BN' AND upper(Trim(vchnum))!='" + txtvchnum.Value.Trim().ToUpper() + "' and upper(Trim(name))='" + txtlbl2.Value.Trim().ToUpper() + "'");
            if (dt.Rows.Count > 0)
            {
                fgen.msg("-", "AMSG", lblheader.Text + " Exists Already With This Name.'13'Please Check");
                return;
            }
        }

        if (Prg_Id == "F55525")
        {
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT DISTINCT TRIM(ACREF) AS CODE FROM TYPEGRP WHERE BRANCHCD='" + frm_mbr + "' AND ID='^A' AND ACREF='" + txtlbl3.Value + "'");
            if (dt.Rows.Count > 0)
            {
                fgen.msg("-", "AMSG", lblheader.Text + " Exists Already For This Account.'13'Please Check");
                return;
            }
        }
        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        btnsave.Disabled = true;
    }
    //------------------------------------------------------------------------------------
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        if (Prg_Id == "F70421" || Prg_Id == "F10123" || Prg_Id == "F10139A" || Prg_Id == "F10125" || Prg_Id == "F40252" || Prg_Id == "F40254")
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
        frm_tid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");

        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        switch (Prg_Id)
        {
            case "F70174":
                SQuery = "select distinct a.type1 as Actg_Sch_Code,a.Name as Actg_Sch_Name,b.Type1 as Ac_Grp_Cd,b.Name as Ac_Grp_name,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.Edt_by,to_char(a.edt_Dt,'dd/mm/yyyy') as Edt_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt from " + frm_tabname + " a left outer join type b on substr(A.type1,1,2)=trim(B.type1) and b.id='Z' where a.branchcd='00' and trim(a.ID)='" + frm_tid + "'  order by a.type1";
                break;
            case "F70419":
                SQuery = "select distinct a.type1 as Block_Code,a.Name as Block_Name,a.num4 as Dep_per,a.num5 as add_dep_per,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.Edt_by,to_char(a.edt_Dt,'dd/mm/yyyy') as Edt_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt from " + frm_tabname + " a where a.branchcd='00' and trim(a.ID)='" + frm_tid + "'  order by a.type1";
                break;
            //case "F70437":
            //    SQuery = "select distinct a.type1 as Block_Code,a.Name as Block_Name,a.num4 as Dep_per,a.num5 as add_dep_per,a.num6 as opening_WDV,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.Edt_by,to_char(a.edt_Dt,'dd/mm/yyyy') as Edt_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and trim(a.ID)='" + frm_tid + "'  order by a.type1";
            //    break;
            case "F70420":
                SQuery = "select distinct a.type1 as Sch_Code,a.Name as Asset_Group_Name,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.Edt_by,to_char(a.edt_Dt,'dd/mm/yyyy') as Edt_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt from " + frm_tabname + " a where a.branchcd='00' and trim(a.ID)='" + frm_tid + "'  order by a.type1";
                break;
            case "F70421":
                SQuery = "select distinct a.type1 as Loc_Code,a.Name as Location_Name,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.Edt_by,to_char(a.edt_Dt,'dd/mm/yyyy') as Edt_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and trim(a.ID)='" + frm_tid + "'  order by a.type1";
                break;
            case "F70177":
                SQuery = "select a.Name,a.acref as HS_Code,a.acref2 as Good_Servc,a.dpt as Taxable,a.num4 as CGS,a.num5 as SGST,a.num6 as IGST,a.num7 as Cess,a.type1 as Code,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.EDT_BY,to_char(a.edt_dt,'dd/mm/yyyy') as Edit_Dt from " + frm_tabname + " a where a.branchcd='00' and trim(a.ID)='" + frm_tid + "'  order by a.type1";
                break;
            case "F10129":
                SQuery = "Select VCHNUM AS OPT_NO,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS Opt_Date,ID,Type1 AS CODE,Name,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,EDT_DT from " + frm_tabname + " where BRANCHCD='" + frm_mbr + "' AND id='" + frm_tid + "' order by type1";
                break;
            case "F55524":
            case "F55525":
                SQuery = "Select a.ID,a.Type1 AS CODE,a.Name,a.acref as acc_code,f.aname as acc_name,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.EDT_BY,(case when nvl(trim(a.edt_by),'-')='-' then '-' else to_char(a.EDT_DT,'dd/mm/yyyy') end) as edt_dt from " + frm_tabname + " a,famst f where trim(a.acref)=trim(f.acode) and a.BRANCHCD='" + frm_mbr + "' AND a.id='" + frm_tid + "' order by code";
                break;
            case "F55526":
                SQuery = "Select ID,Type1 AS CODE,Name,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,EDT_BY,(case when nvl(trim(edt_by),'-')='-' then '-' else to_char(EDT_DT,'dd/mm/yyyy') end) as edt_dt from " + frm_tabname + " where BRANCHCD='" + frm_mbr + "' AND id='" + frm_tid + "' order by type1";
                break;
            case "F30365":
                SQuery = "select id,type1 as code,name as machine_srno,acref as machine_make,acref2 as machine_model,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt from " + frm_tabname + " where id='" + frm_tid + "' order by code";
                break;
            case "F30366":
                SQuery = "select id,type1 as code,name as grade,acref as cu,acref2 as ni,acref3 as cr,p_acode as mo,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt from " + frm_tabname + " where id='" + frm_tid + "' order by code";
                break;
            case "F30368":
                SQuery = "select id,type1 as code,name as defect,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt from " + frm_tabname + " where id='" + frm_tid + "' order by code";
                break;
            default:
                SQuery = "select distinct a.type1 as Type_Code,a.Name as Type_Name,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt from " + frm_tabname + " a where a.branchcd='00' and trim(a.ID)='" + frm_tid + "'  order by a.type1";
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
        frm_tid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TID");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        //SQuery = "SELECT '" + lblheader.Text + "' as header,'" + lbl3.InnerText + "' as h1,'" + Label5.InnerText + "' as h2,'" + Label2.InnerText + "' as h3, '" + Label3.InnerText + "' as h4, '" + Label4.InnerText + "'as h5, '" + Label6.InnerText + "'as h6, A.TYPE1 AS CODE,A.NAME as name,nvl(a.num4,0) as df1 ,nvl(a.num5,0) as df2,nvl(a.num6,0) as df3, nvl(a.num7,0) as df4 FROM TYPEGRP A  WHERE trim(a.ID)='" + frm_tid + "'  ORDER BY A.TYPE1";
        if (Prg_Id == "F10125")
        {
            hffield.Value = "Print_E";
            make_qry_4_popup();
            fgen.Fn_open_mseek("Select " + lblheader.Text + " to print", frm_qstr);
        }
        else
        {
            SQuery = "SELECT '" + lblheader.Text + "' as header,'" + lbl3.InnerText + "' as h1,'" + Label5.InnerText + "' as h2,'" + Label2.Text + "' as h3, '" + Label3.Text + "' as h4, '" + Label4.Text + "'as h5, '" + Label6.InnerText + "'as h6,A.TYPE1 AS CODE,A.NAME as name,a.acref as extra,a.lineno,a.acref3 as other_ref,'-' as col4  FROM TYPEGRP A  WHERE trim(a.ID)='" + frm_tid + "'   ORDER BY A.TYPE1";
            if (Prg_Id == "F70177")
                SQuery = "SELECT '" + lblheader.Text + "' as header,'Entry No' as h1,'" + Label5.InnerText + "' as h2,'HSN/SAC' as h3, 'CGST %' as h4, 'SGST %' as h5, 'IGST %' as h6,A.TYPE1 AS CODE,A.NAME as name,a.acref as extra,to_char(a.num4,'999.99') lineno,to_char(a.num5,'999.99') as other_ref,to_char(a.num6,'999.99') as col4  FROM TYPEGRP A  WHERE trim(a.ID)='" + frm_tid + "'   ORDER BY A.TYPE1";

            fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "tgpopmst", "tgpopmst");
        }
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
        if (Prg_Id == "F40252" || Prg_Id == "F40254")
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_TID", ID_TWIP.Value);
        }

        frm_tid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {

            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||trim(a.ID)||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_tid + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_tid + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_tid, lblheader.Text.Trim() + " Deleted");
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
                    if (Prg_Id == "F40252" || Prg_Id == "F40254")
                    {
                        ID_TWIP.Value = frm_tid + col1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TID", ID_TWIP.Value);
                    }
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;
                case "Edit":
                    if (col1 == "") return;

                    if (Prg_Id == "F40252" || Prg_Id == "F40254")
                    {
                        txtlbl5.Value = col1;
                        txtlbl6.Value = col2;
                        ID_TWIP.Value = frm_tid + col1;
                    }

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

                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "" || col1 == "-") return;
                    clearctrl();
                    frm_tid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TID");

                    string mv_col;
                    if (Prg_Id == "F40252" || Prg_Id == "F40254")
                    {
                        frm_tid = ID_TWIP.Value;
                    }

                    mv_col = frm_mbr + frm_tid + col1;
                    SQuery = "Select a.*,substr(a.type1,1,2) as acgrp,to_Char(a.ent_Dt,'dd/mm/yyyy') As ment_date from " + frm_tabname + " a where a.branchcd||trim(a.ID)||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + mv_col + "' ";
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

                        txtlbl2.Value = dt.Rows[i]["NAME"].ToString().Trim();
                        txtlbl3.Value = dt.Rows[i]["ACREF"].ToString().Trim();
                        txtlbl4.Value = dt.Rows[i]["ACREF2"].ToString().Trim();
                        txtlbl5.Value = dt.Rows[i]["ACREF3"].ToString().Trim();

                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                        switch (Prg_Id)
                        {
                            case "F10136":
                                txtlbl5.Value = dt.Rows[i]["acref"].ToString().Trim();
                                txtlbl6.Value = dt.Rows[i]["lineno"].ToString().Trim();
                                txtlbl7.Value = dt.Rows[i]["acref3"].ToString().Trim();
                                break;
                            case "F70419":
                            case "F70420":
                            case "F70421":
                            case "F70174":
                            case "F70437":
                            case "F10308":
                                if (Prg_Id == "F70437")
                                {
                                    string abcd = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(name)||'~'||num4||'~'||num5 as getdata from typegrp where id='DI' and branchcd||type1='" + frm_mbr + "" + dt.Rows[i]["acref2"].ToString().Trim() + "'", "getdata");
                                    txtlbl2.Value = abcd.Split('~')[0].ToString();
                                    txtlbl5.Value = abcd.Split('~')[1].ToString();
                                    txtlbl6.Value = abcd.Split('~')[2].ToString();
                                }
                                else
                                {
                                    txtlbl2.Value = dt.Rows[i]["NAME"].ToString().Trim();
                                    txtlbl3.Value = dt.Rows[i]["acgrp"].ToString().Trim();
                                }
                                txtlbl4.Value = dt.Rows[i]["type1"].ToString().Trim();
                                if (Prg_Id == "F70419")
                                {
                                    txtlbl5.Value = dt.Rows[i]["num4"].ToString().Trim();
                                    txtlbl6.Value = dt.Rows[i]["num5"].ToString().Trim();
                                }
                                if (Prg_Id == "F70419")
                                {
                                    txtlbl7.Value = dt.Rows[i]["num6"].ToString().Trim();
                                }
                                break;

                            case "F70177":
                            case "F10123":
                                txtlbl5.Value = dt.Rows[i]["num4"].ToString().Trim();
                                txtlbl6.Value = dt.Rows[i]["num5"].ToString().Trim();
                                txtlbl7.Value = dt.Rows[i]["num6"].ToString().Trim();
                                txtlbl8.Value = dt.Rows[i]["num7"].ToString().Trim();
                                txtlbl9.Value = dt.Rows[i]["dpt"].ToString().Trim();
                                break;
                            case "F10125":
                                txtlbl2.Value = dt.Rows[i]["Name"].ToString().Trim();
                                txtlbl5.Value = dt.Rows[i]["provision"].ToString().Trim();
                                break;

                            case "F30365":
                                txtlbl2.Value = dt.Rows[i]["Name"].ToString().Trim();
                                txtlbl5.Value = dt.Rows[i]["acref"].ToString().Trim();
                                txtlbl6.Value = dt.Rows[i]["acref2"].ToString().Trim();
                                break;

                            case "F30366":
                            case "F55256":
                            case "F55257":
                            case "F55254":
                                txtlbl2.Value = dt.Rows[i]["Name"].ToString().Trim();
                                txtlbl5.Value = dt.Rows[i]["acref"].ToString().Trim();
                                txtlbl6.Value = dt.Rows[i]["acref2"].ToString().Trim();
                                txtlbl7.Value = dt.Rows[i]["acref3"].ToString().Trim();
                                if (Prg_Id == "F30366")
                                    txtlbl8.Value = dt.Rows[i]["p_acode"].ToString().Trim();
                                break;
                            case "F70182":
                                txtlbl6.Value = dt.Rows[i]["acref4"].ToString().Trim();
                                break;
                        }

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

                        //txtlbl4.Disabled = true;

                        //txtlbl2.Disabled = true;
                        //txtlbl3.Disabled = true;
                        //txtlbl5.Disabled = true;
                        //txtrmk.Enabled = false;                        
                    }
                    #endregion
                    break;
                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
                    fgen.fin_engg_reps(frm_qstr);
                    break;
                case "TYPECODE":
                    if (col1.Length < 1 || col1 == "" || col1 == "0")
                    {
                        return;
                    }
                    else
                    {
                        if (col1.Contains("~"))
                        {
                            txtlbl4.Value = (col1.Split('~')[0].ToString() == "0" || col1.Split('~')[0].ToString() == "") ? "-" : col1.Split('~')[0].ToString();
                            txtlbl5.Value = (col1.Split('~')[1].ToString() == "0" || col1.Split('~')[1].ToString() == "") ? "-" : col1.Split('~')[1].ToString();
                        }
                        else if (fgen.make_double(col1) <= 0)
                        {
                            fgen.msg("-", "AMSG", "Please Choose Code Which is Available !!");
                            return;
                        }
                        else
                        {
                            txtlbl4.Value = col2;
                        }
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
                            txtlbl3.Value = (col1.Split('~')[0].ToString() == "0" || col1.Split('~')[0].ToString() == "") ? "-" : col1.Split('~')[0].ToString();
                            txtlbl5.Value = (col1.Split('~')[1].ToString() == "0" || col1.Split('~')[1].ToString() == "") ? "-" : col1.Split('~')[1].ToString();
                        }
                        else
                        {
                            txtlbl3.Value = col1;
                        }
                    }
                    if (Prg_Id == "F45176")
                    {
                        txtlbl4.Value = col3;
                    }
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

        frm_tid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            SQuery = "SELECT a.Id,a.Type1,a.Name,a.Typedpt,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_Dtd,a.Edt_by,to_char(a.edt_Dt,'dd/mm/yyyy') as Edt_Dtd FROM " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.id='" + frm_tid + "'  order by a.type1 ";
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

                    if (Prg_Id == "F40252" || Prg_Id == "F40254")
                    {
                        frm_tid = ID_TWIP.Value;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TID", frm_tid);
                    }

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
                                cond = "";
                                if (frm_formID == "F10555")
                                    cond = " AND TYPE1<'100'";

                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and ID='" + frm_tid + "' " + cond + " ", 6, "vch");
                                pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_tid + frm_vnum + frm_CDT1, frm_mbr, frm_tid, frm_vnum, txtvchdate.Value.Trim(), "", frm_uname);
                                if (i > 10)
                                {
                                    fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and ID='" + frm_tid + "' ", 6, "vch");

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

                    if (frm_formID == "F70704" && frm_vnum == "000001") { frm_vnum = "101001"; }
                    if (frm_formID == "F70701" && frm_vnum == "000001") { frm_vnum = "102001"; }
                    if (frm_formID == "F70702" && frm_vnum == "000001") { frm_vnum = "103001"; }
                    if (frm_formID == "F70703" && frm_vnum == "000001") { frm_vnum = "104001"; }

                    if (frm_formID == "F45172" && frm_vnum == "000001") { frm_vnum = "101001"; }
                    if (frm_formID == "F45174" && frm_vnum == "000001") { frm_vnum = "102001"; }
                    if (frm_formID == "F45176" && frm_vnum == "000001") { frm_vnum = "103001"; }

                    save_fun();

                    string ddl_fld1;
                    string ddl_fld2;
                    ddl_fld1 = frm_mbr + frm_tid + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    ddl_fld2 = frm_tid + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    if (edmode.Value == "Y")
                    {

                        fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||trim(ID)||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + ddl_fld1 + "'");


                    }
                    try
                    {
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");

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
                                fgen.msg("-", "AMSG", "Data Saved");

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
                        //fgen.send_Activity_mail(frm_qstr, frm_cocd, "Tejaxo ERP", frm_formID, subj + lblheader.Text + " #" + frm_vnum, sb.ToString(), frm_uname);

                        //fgen.send_Activity_msg(frm_qstr, frm_cocd, frm_formID, subj + lblheader.Text + " #" + frm_vnum + " by " + frm_uname, frm_uname);

                        sb.Clear();
                        #endregion
                    }
                    catch (Exception ERR) { }

                    
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||trim(ID)||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + ddl_fld2 + "'");

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
        frm_tid = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();

        oporow["BRANCHCD"] = frm_mbr;

        oporow["ID"] = frm_tid;
        oporow[doc_nf.Value] = frm_vnum;
        oporow[doc_df.Value] = txtvchdate.Value.Trim();

        if (txtlbl4.Value.ToUpper().Trim().Length <= 1)
        {
            oporow["type1"] = frm_vnum.Trim().Substring(3, 3);
        }
        else
        {
            oporow["type1"] = txtlbl4.Value.ToUpper().Trim();
        }
        if (frm_tid == "BZ" || frm_tid == "L1" || frm_tid == "L2" || frm_tid == "L3" || frm_tid == "$1" || frm_tid == "$2" || frm_tid == "EM")
        {
            oporow["type1"] = frm_vnum.Trim().Substring(2, 4);
        }
        oporow["vchnum"] = frm_vnum.Trim();

        oporow["Name"] = txtlbl2.Value.ToUpper().Trim();
        oporow["acref"] = txtlbl3.Value.ToUpper().Trim();
        oporow["acref2"] = txtlbl4.Value.ToUpper().Trim();
        oporow["acref3"] = txtlbl5.Value.ToUpper().Trim();

        switch (Prg_Id)
        {
            case "F70182":
                oporow["type1"] = frm_vnum.Trim().Substring(3, 3);
                oporow["acref4"] = txtlbl6.Value.ToUpper().Trim();
                break;

            case "F10136":
                oporow["acref"] = txtlbl5.Value.ToUpper().Trim();
                oporow["lineno"] = fgen.make_double(txtlbl6.Value.ToUpper().Trim());
                oporow["acref3"] = txtlbl7.Value.ToUpper().Trim();
                break;
            case "F70177":
            case "F70419":
            case "F70420":
            case "F70421":
            case "F10123":
                oporow["num4"] = fgen.make_double(txtlbl5.Value.ToUpper().Trim());
                oporow["num5"] = fgen.make_double(txtlbl6.Value.ToUpper().Trim());
                oporow["num6"] = fgen.make_double(txtlbl7.Value.ToUpper().Trim());
                oporow["num7"] = fgen.make_double(txtlbl8.Value.ToUpper().Trim());
                oporow["dpt"] = txtlbl9.Value.ToUpper().Trim();
                break;
            case "F10125":
                oporow["Name"] = txtlbl2.Value.ToUpper().Trim();
                oporow["provision"] = fgen.make_double(txtlbl5.Value.ToUpper().Trim());
                break;
            case "F30365":
                oporow["Name"] = txtlbl2.Value.ToUpper().Trim();
                oporow["acref"] = txtlbl5.Value.ToUpper().Trim();
                oporow["acref2"] = txtlbl6.Value.ToUpper().Trim();
                break;

            case "F30366":
                oporow["Name"] = txtlbl2.Value.ToUpper().Trim();
                oporow["acref"] = txtlbl5.Value.ToUpper().Trim();
                oporow["acref2"] = txtlbl6.Value.ToUpper().Trim();
                oporow["acref3"] = txtlbl7.Value.ToUpper().Trim();
                oporow["p_acode"] = txtlbl8.Value.ToUpper().Trim();
                break;
            case "F55256":
            case "F55257":
            case "F55254":
                oporow["type1"] = frm_vnum;
                oporow["acref"] = txtlbl5.Value.ToUpper().Trim();
                oporow["acref2"] = txtlbl6.Value.ToUpper().Trim();
                oporow["acref3"] = txtlbl7.Value.ToUpper().Trim();
                break;
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

    void Acode_Sel_query()
    {

    }
    void Icode_Sel_query()
    {

    }

    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F40252":
            case "F40254":
                SQuery = "select trim(Acref) as fstr, NAME,trim(Acref) as Code from typegrp where branchcd='" + frm_mbr + "' and id='WI' and substr(acref,1,1)='6' order by trim(Acref)";
                break;
        }
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
    protected void btntype_Click(object sender, ImageClickEventArgs e)
    {
        if (edmode.Value == "Y" && (frm_formID != "F39505" && frm_formID != "F70182" && frm_formID != "F70183" && frm_formID != "F70193" && frm_formID != "F39505" && frm_formID != "F39505"))
        {
            fgen.msg("-", "AMSG", "Code Change not Allowed in Edit mode !!");
            return;
        }

        hffield.Value = "ACTGCODE";
        make_qry_4_popup();
        if (frm_formID == "F55524" || frm_formID == "F55525")
        {
            fgen.Fn_open_sseek("Select Account Code", frm_qstr);
        }
        else
        {
            fgen.Fn_open_sseek("Select Code", frm_qstr);
        }
    }
    protected void btnactg_Click(object sender, ImageClickEventArgs e)
    {
        if (txtlbl3.Value.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select " + lbl3.InnerText + " first !!");

            return;
        }

        if (edmode.Value == "Y" && frm_formID != "F70182")
        {
            fgen.msg("-", "AMSG", "Code Change not Allowed in Edit mode !!");

            return;
        }
        else
        {
            hffield.Value = "TYPECODE";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Code", frm_qstr);
        }
    }
    ///***********************************************Instructions
    // if using '~' in "actg" hidden field make_query send 4 values with 3 '~' seperators for values to be put in txtlbl2,4,5,6
    // if using '~' in "typecode" hidden field make_query send 4 values with 1 '~' seperators for values to be put in txtlbl4,5

    ///******************************************************************

}