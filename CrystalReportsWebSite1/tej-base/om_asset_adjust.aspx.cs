using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

//F70423 FA Adjustment Record
//70557 fixed asset insurance record
public partial class om_asset_adjust : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, col7, vardate, fromdt, todt, next_year, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0, flag = 0;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string html_body = "";
    string Prg_Id, lbl1a_Text, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond, lifeend;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
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
                    lbl1a_Text = "CS";
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    next_year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    lifeend = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7");
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                doc_addl.Value = "1";
                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                switch (Prg_Id)
                {
                    case "F70423":
                        lblheader.Text = "Fixed Asset Adjustment Record";
                        break;
                    case "F70557": //by yogita
                        lblheader.Text = "Fixed Asset Insurance Record";
                        break;
                }

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();

            if (frm_ulvl != "0")
            {
                btndel.Visible = false;
            }
            if (lblUpload.Text.Length > 1)
            {
                btnView1.Visible = true;
                btnDwnld1.Visible = true;
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

        //txtlbl2.Attributes.Add("readonly", "readonly");
        //txtlbl3.Attributes.Add("readonly", "readonly");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");


        //txtlbl5.Attributes.Add("readonly", "readonly");
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
        //switch (Prg_Id)
        //{

        //    //case "M12008":
        //    //    tab3.Visible = false;
        //    //    tab4.Visible = false;
        //    //    break;
        //    //case "F60101":
        //    //    AllTabs.Visible = false;
        //    //    break;
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

        // ddModule.Disabled = false;
        //ddIssueType.Disabled = false;
        //txtSup_by.Disabled = false;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true;
        btnedit.Disabled = true;
        btnsave.Disabled = true;
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
        switch (Prg_Id)
        {
            case "F70423":
                frm_tabname = "wb_fa_vch"; // flute master table.....REAL TABLE   
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "50");
                txt_lifeend.Attributes.Add("READONLY", "READONLY");
                Label9.Visible = false; txt_wdv_value_chng.Visible = false;
                Label11.Visible = false; txt_residual.Visible = false;
               break;
            case "F70557":
                frm_tabname = "wb_fa_vch";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "11");
                Label18.InnerText = "Insurance Document Date.";
                Label19.InnerText = "Insurance Value";
                txt_wdv_new.Attributes.Add("maxlength", "18");
                txt_wdv_new.Attributes.Add("placeholder", "Insurance Value");
                lbltxtrmk.Text = "Remarks";
                txt_bal_new.Attributes.Remove("readonly");
                Label15.InnerText = "Insurance Premium";
                txt_resi_new.Attributes.Add("maxlength", "8");
                txt_resi_new.Attributes.Add("placeholder", "Insurance Premium");
                Label17.Visible = false; txtWdv_val.Visible = false;
                Label9.Visible = false; txt_wdv_value_chng.Visible = false;
                ////another div
                Label16.InnerText = "Policy No.";
                txt_bal_new.Attributes.Add("maxlength", "18");
                txt_bal_new.Attributes.Add("placeholder", "Policy No");
                Label12.InnerText = "Renewal Date";
                txt_lifeend.Attributes.Remove("readonly");
                txt_lifeend.Attributes.Add("placeholder", "Renewal Date");
                Label11.Visible = false; txt_residual.Visible = false;
                Label20.Visible = false; txt_newdeprdays.Visible = false;
                Label13.Visible = false; txt_deprday.Visible = false;
                Label14.Visible = false; txt_daysnew.Visible = false;
                break;
        }
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
            case "INV":
                //SQuery = "SELECT DISTINCT trim(A.vchnum)||to_char(A.vchdate,'dd/mm/yyyy')||trim(A.branchcd)||trim(A.TYPE)||TRIM(A.ACODE) as col1,  B.ANAME, A.VCHNUM, to_char(a.vchdate,'dd/mm/yyyy') as vchdate, ,A.BILL_TOT ,A.BILL_QTY FROM SALE A ,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND a.TYPE='4D'  AND a.BRANCHCD='" + frm_mbr + "' ORDER BY a.VCHNUM ,a.vchdate";
                SQuery = "Select trim(b.aname),trim(b.aname) as aname,trim(a.acode)||'~'||trim(a.invno)||'~'|| invdate||'~'||sale_amt as acode_invno_dt_amt from (select a.fstr,sum(a.sqty) as sqty1,sum(a.fqty) as fqty1,trim(a.invno) as invno,invdate,max( amt_sale) as sale_amt, max(a.acode) as acode from (select trim(A.branchcd)||trim(A.vchnum)||to_char(A.vchdate,'dd/mm/yyyy') as fstr,A.BILL_QTY as sqty, 0 as fqty , trim(a.vchnum) as invno, to_char(a.vchdate,'dd/mm/yyyy') as invdate, a.amt_sale, a.acode  from sale a where a.TYPE='4D'  AND a.BRANCHCD='00' union all select trim(A.branchcd)||trim(A.invno)||to_char(A.invdate,'dd/mm/yyyy') as fstr,0 as sqty,a.iqtyout as fqty, trim(a.invno),to_char(a.invdate,'dd/mm/yyyy'),0 as amt_sale, null as acode  from wb_fa_vch a where type='20' AND a.BRANCHCD='00')a  group by a.fstr, trim(a.invno), invdate)a, famst b where TRIM(A.ACODE)=TRIM(B.ACODE) having sqty1 > fqty1";
                break;
            case "ASSET":
                SQuery = "select a.fstr,max(a.aname) as aname,trim(A.Acode) as acode,sum(a.inw) as tinw, sum(a.outw) as toutw, sum(a.inw) - sum(a.outw) as tbal from ( select a.branchcd|| trim(A.Acode)||to_ChaR(a.instdt,'dd/mm/yyyy') as fstr,a.assetname as aname, A.Acode, a.quantity as inw, 0 as outw  from wb_fa_pur a where A.type='10' and A.branchcd='" + frm_mbr + "' union all select a.branchcd|| trim(A.Acode)||to_char(a.instdt,'dd/mm/yyyy') as fstr,null as aname, A.Acode , 0 as inw, a.iqtyout as outw  from wb_fa_vch a where type='20' and A.branchcd='" + frm_mbr + "' ) a group by trim(a.acode),a.fstr having sum(a.inw) - sum(a.outw) > 0 order by acode";
                break;
            //case "PERSON":
            //    SQuery = "Select Rownum LIFE, Rownum SELECT_LIFE from dual Connect By Rownum <= 100";
            //    break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    // SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as CSS_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as CSS_Dt,a.ccode as company,a.Eicon as Subjects,substr(a.remarks,1,60) as Remarks,a.dir_comp,a.Last_Action,a.cont_name as person,a.cont_no as contact_no, a.Ent_by,a.ent_Dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where  a.branchcd='" + frm_mbr + "' and a.type='" + lbl1a_Text + "' " + cond + " order by vdd desc,a." + doc_nf.Value + " desc";
                    // SQuery = "SELECT distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')as fstr,to_char(Vchdate,'dd/mm/yyyy')  as Sale_Dt,Vchnum as Sale_No,Acode,,Ent_by,Ent_Dt FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE  " + DateRange + "   ORDER BY VCHNUM DESC";
                    SQuery = "select DISTINCT trim(a.branchcd)||trim(a.type)||trim(A.vchnum)||to_char(A.vchdate,'dd/mm/yyyy') as fstr, B.assetname ,A.Vchnum,to_char(A.vchdate,'dd/mm/yyyy') as Vch_date,A.Acode from wb_fa_vch A,wb_fa_pur B  WHERE a.branchcd||TRIM(A.ACODE)=b.branchcd||TRIM(B.ACODE)  AND A.VCHDATE " + DateRange + " AND A.type='" + frm_vty + "' and A.branchcd='" + frm_mbr + "' order by A.vchnum desc";

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
            // else comment upper code

            //frm_vnum = fgen.next_no(frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH" );
            //txtvchnum.Value = frm_vnum;
            //txtvchdate.Value = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            //disablectrl();
            //fgen.EnableForm(this.Controls);
            if (frm_ulvl == "3")
            {
                //txtlbl4.Value = frm_uname;
                // txtlbl4.Disabled = true;
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
        #region
        if (col1 == "") return;
        frm_vty = vty;

        string mq0 = "";
        if (Prg_Id == "F70423")
        {
            mq0 = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + "";
        }
        if (Prg_Id == "F70557")
        {
            mq0 = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + "";
        }
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, mq0, 6, "VCH");

        txtvchnum.Value = frm_vnum;
        txtvchdate.Value = Convert.ToDateTime(fgen.Fn_curr_dt(frm_cocd, frm_qstr)).ToString("yyyy-MM-dd");
        txtlbl5.Value = Convert.ToDateTime(fgen.Fn_curr_dt(frm_cocd, frm_qstr)).ToString("yyyy-MM-dd");
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
        int dhd = fgen.ChkDate(txtvchdate.Value.ToString());
        if (dhd == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return;
        }
        switch (Prg_Id)
        {
            case "F70423":
                cal();
                break;
            case "F70557":
                cal1();
                break;
        }
        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
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
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete  from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(10, 10), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
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
                case "ASSET":
                    txtlbl8.Value = col3;
                    txtlbl8a.Value = col2;
                    string col11 = "";
                    col11 = fgen.seek_iname(frm_qstr, frm_cocd, "select grpcode||'~'||trim(assetsupp)||'~'||original_cost||'~'||trim(assetsuppadd)||'~'||dom_imp||'~'||trim(locn)||'~'||trim(invno)||'~'||to_char(instdt,'dd/mm/yyyy')||'~'||to_char(LIFE_END,'dd/mm/yyyy')||'~'||BALLIFE||'~'||RESIDVAL||'~'||DEPRPDAY as PP  from wb_fa_pur where  type='10' and branchcd='" + frm_mbr + "' and trim(acode)='" + txtlbl8.Value + "'", "PP");

                    if (col11.Length > 1)
                    {
                        txtgrpcode.Value = col11.Split('~')[0].ToString();
                        if (col11.Split('~')[1].ToString() == "0")
                        {
                            txtSup_by.Value = "-";
                        }
                        else
                        {
                            txtSup_by.Value = col11.Split('~')[1].ToString();
                        }
                        if (col11.Split('~')[3].ToString() == "0")
                        {
                            txtSup_Address.Value = "-";
                        }
                        txtSup_Address.Value = col11.Split('~')[3].ToString();
                        ddCategory.Value = col11.Split('~')[4].ToString();
                        if (col11.Split('~')[5].ToString() == "0")
                        {
                            txtlocation.Value = "-";
                        }
                        else
                        {
                            txtlocation.Value = col11.Split('~')[5].ToString();
                        }
                        if (col11.Split('~')[6].ToString() == "0")
                        {
                            txt_invno.Value = "-";
                        }
                        else
                        {
                            txt_invno.Value = col11.Split('~')[6].ToString().Trim();
                        }

                        txtlbl5.Value = Convert.ToDateTime(col11.Split('~')[7].ToString().Trim()).ToString("yyyy-MM-dd");
                        txtgrpname.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select name from typegrp where id='FA' and type1='" + txtgrpcode.Value + "'", "name");
                        if (Prg_Id == "F70423")
                        {
                            txt_lifeend.Value = Convert.ToDateTime(col11.Split('~')[8].ToString().Trim()).ToString("yyyy-MM-dd");
                        }
                        txt_residual.Value = col11.Split('~')[10].ToString().Trim();
                        txt_deprday.Value = col11.Split('~')[11].ToString().Trim();
                        ////cow
                        //                        SQuery = "select sum(a.cramt)- sum(b.dramt),branchcd||acode||to_charinstdt,'dd/mm/yyyy') from wb_fa_vch where type='30' and  acode='" + txtlbl8.Value.Trim() + "' and branchcd='" + frm_mbr + "' ";
                        //                         = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "deprdays");


                        //                       fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL7", col11.Split('~')[8].ToString().Trim());

                        //string vcurrwdv = "SELECT trim(a.FSTR),sum(A.PURCH+A.ADJUST) AS CURRWDV   FROM (SELECT BRANCHCD||TRIM(ACODE)||TO_CHAR(INSTDT,'DD/MM/YYY') AS FSTR, ORIGINAL_COST AS PURCH, 0 AS ADJUST FROM WB_FA_PUR WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND ACODE='" + txtlbl8.Value.Trim() + "' UNION  all SELECT BRANCHCD||TRIM(ACODE)||TO_CHAR(INSTDT,'DD/MM/YYY') as fstr,  0 AS PURCH,(SUM(NVL(DRAMT,0))- SUM(NVL(CRAMT,0)))  AS ADJUST FROM WB_FA_VCH WHERE BRANCHCD='" + frm_mbr + "' AND (TYPE='30' OR TYPE='50') AND ACODE='" + txtlbl8.Value.Trim() + "' GROUP BY BRANCHCD||TRIM(ACODE)||TO_CHAR(INSTDT,'DD/MM/YYY')) A group by trim(a.FSTR)";
                        string vcurrwdv = "SELECT trim(a.FSTR),sum(A.PURCH+A.ADJUST) AS CURRWDV   FROM (SELECT BRANCHCD||TRIM(ACODE)||TO_CHAR(INSTDT,'DD/MM/YYY') AS FSTR, (ORIGINAL_COST- residval) AS PURCH, 0 AS ADJUST FROM WB_FA_PUR WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND ACODE='" + txtlbl8.Value.Trim() + "' UNION  all SELECT BRANCHCD||TRIM(ACODE)||TO_CHAR(INSTDT,'DD/MM/YYY') as fstr,  0 AS PURCH,(SUM(NVL(DRAMT,0))- SUM(NVL(CRAMT,0)))  AS ADJUST FROM WB_FA_VCH WHERE BRANCHCD='" + frm_mbr + "' AND (TYPE='30' OR TYPE='50') AND ACODE='" + txtlbl8.Value.Trim() + "' GROUP BY BRANCHCD||TRIM(ACODE)||TO_CHAR(INSTDT,'DD/MM/YYY')) A group by trim(a.FSTR)";
                        txtWdv_val.Value = fgen.seek_iname(frm_qstr, frm_cocd, vcurrwdv, "CURRWDV");

                    }

                    break;
                case "INV":
                    txtWdv_val.Value = col1;
                    if (col3.Length < 1 || col3 == "" || col3 == "0")
                    {
                    }
                    else
                    {
                        if (col3.Split('~')[1].ToString() == "0")
                        {
                            txtdate_rev.Value = "-";
                        }
                        if (col3.Split('~')[2].ToString() == "0")
                        {
                            //txt_saledt.Value = "-";
                        }
                        txtdate_rev.Value = col3.Split('~')[1].ToString();
                        //txt_saledt.Value =  Convert.ToDateTime(col3.Split('~')[2].ToString()).ToString("yyyy-MM-dd");
                        // txt_wdv_new.Value="1";
                    }
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
                    frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
                    string mv_col;
                    string col12 = "";
                    //mv_col = frm_mbr + frm_vty + col1;
                    mv_col = col1;
                    SQuery = "Select a.*,to_Char(a.ent_Dt,'dd/mm/yyyy') As ment_date,to_Char(a.invdate,'dd/mm/yyyy') As minvdate,upper(to_Char(a.vchdate,'yyyy-mm-dd')) As mvchdate from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + mv_col + "' ORDER BY A.vchnum";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        i = 0;
                        txtvchnum.Value = dt.Rows[i]["vchnum"].ToString().Trim();
                        txtvchdate.Value = Convert.ToDateTime(dt.Rows[i]["mvchdate"].ToString().Trim()).ToString("yyyy-MM-dd");
                        txtgrpcode.Value = dt.Rows[i]["grpcode"].ToString().Trim();
                        txtgrpname.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select name from typegrp where id='FA' and type1='" + txtgrpcode.Value + "'", "name");
                        txtlbl8.Value = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl8a.Value = col2;
                        SQuery = "select deprdays from wb_fa_vch where type='30' and  acode='" + txtlbl8.Value.Trim() + "' and branchcd='" + frm_mbr + "' and vchdate " + DateRange + "";
                        col12 = "";
                        col12 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(assetsupp)||'~'||trim(assetsuppadd)||'~'||locn||'~'||to_char(instdt,'dd/mm/yyyy')||'~'|| trim(invno)||'~'||to_char(invdate,'dd/mm/yyyy')||'~'||to_char(life_end,'dd/mm/yyyy')||'~'||trim(ballife)||'~'||trim(residval) as PP  from wb_fa_pur where  type='10' and branchcd='" + frm_mbr + "' and trim(acode)='" + txtlbl8.Value + "'", "PP");
                        if (col12.Length > 1)
                        {
                            txtSup_by.Value = col12.Split('~')[0].ToString();
                            txtSup_Address.Value = col12.Split('~')[1].ToString();
                            txtlocation.Value = col12.Split('~')[2].ToString();
                            txt_invno.Value = col12.Split('~')[4].ToString();
                            txtlbl5.Value = Convert.ToDateTime(col12.Split('~')[5].ToString()).ToString("yyyy-MM-dd");
                            txt_lifeend.Value = Convert.ToDateTime(col12.Split('~')[6].ToString()).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            txtSup_by.Value = "-";
                            txtSup_Address.Value = "-";
                            txtlocation.Value = "-";
                            txt_invno.Value = "-";
                            txtlbl5.Value = "-";
                            txt_lifeend.Value = "-";
                        }

                        switch (Prg_Id)
                        {
                            case "F70423":
                                txt_wdv_new.Value = (fgen.make_double(dt.Rows[i]["dramt"].ToString().Trim()) - fgen.make_double(dt.Rows[i]["cramt"].ToString().Trim())).ToString().Trim();
                                txtrmk.Text = dt.Rows[i]["naration"].ToString().Trim();
                                txtdate_rev.Value = Convert.ToDateTime(dt.Rows[i]["instdt"].ToString().Trim()).ToString("yyyy-MM-dd");
                                txt_resi_new.Value = dt.Rows[i]["salevalue"].ToString().Trim();
                                txt_bal_new.Value = dt.Rows[i]["depr_wbk"].ToString().Trim();
                                txtWdv_val.Value = dt.Rows[i]["assetval"].ToString().Trim();
                                txt_deprday.Value = dt.Rows[i]["depr_old"].ToString().Trim();
                                txt_wdv_value_chng.Value = Math.Round((fgen.make_double(dt.Rows[i]["dramt"].ToString().Trim()) - fgen.make_double(dt.Rows[i]["assetval"].ToString().Trim())), 2).ToString().Trim();
                                txt_newdeprdays.Value = dt.Rows[i]["deprdays"].ToString().Trim();
                                txt_daysnew.Value = dt.Rows[i]["depr"].ToString().Trim();
                               break;
                            case "F70557":
                                // txt_wdv_new.Value = dt.Rows[i]["dramt"].ToString().Trim();
                                txtrmk.Text = dt.Rows[i]["naration"].ToString().Trim();
                                txtdate_rev.Value = Convert.ToDateTime(dt.Rows[i]["instdt"].ToString().Trim()).ToString("yyyy-MM-dd");
                                //txt_resi_new.Value = dt.Rows[i]["salevalue"].ToString().Trim();
                                txt_bal_new.Value = dt.Rows[i]["mrr_ref"].ToString().Trim();
                                txt_wdv_new.Value = dt.Rows[i]["assetval"].ToString().Trim();
                                txt_resi_new.Value = dt.Rows[i]["assetval1"].ToString().Trim();
                                txt_lifeend.Value = Convert.ToDateTime(dt.Rows[i]["LIFE_END"].ToString().Trim()).ToString("yyyy-MM-dd");
                                txt_resi_new.Attributes.Remove("readonly");
                                txt_wdv_new.Attributes.Remove("readonly");
                                txt_bal_new.Attributes.Remove("readonly");
                                break;
                        }
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                        txtvchnum.Disabled = true;
                        if (frm_uname != "FINTEAM")
                        {
                            txtlbl8.Disabled = true;
                            txtlbl5.Disabled = true;
                            txtrmk.Enabled = true;
                        }
                        if (fgen.make_double(frm_ulvl) < 3)
                            if (lblUpload.Text.Length > 1) btnDwnld1.Visible = true;

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
                            sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                            sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
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

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
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
                    //((TextBox)sg3.Rows[z].FindControl("sg3_t1")).Focus();
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
            if (frm_ulvl == "3") cond = " and trim(a.ccode)='" + frm_uname + "'";
            if (CSR.Length > 1) cond = " and trim(a.ccode)='" + CSR + "'";
            switch (Prg_Id)
            {
                case "F70423":
                    // SQuery = "SELECT a.CSSNO as CSS_NO,to_char(A.CSsDT,'dd/mm/yyyy') as CSS_Dt,a.CCODE as Client_Code,a.dir_comp,a.Emodule as Module_Name,a.Eicon as Option_Name,a.Remarks,a.Req_type,a.Iss_type as Issue_Type,a.Cont_name,a.Cont_No,a.Cont_Email,a.ent_by,a.Ent_Dt,last_Action,last_Actdt,a.wrkrmk,a.app_by,a.app_dt,to_chaR(a.CSSDT,'YYYYMMDD') as CSS_DTd FROM " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.cssdt " + PrdRange + " " + cond + " and a.cssdt>=to_Date('01/10/2017','dd/mm/yyyy') order by a.cssno ";
                    SQuery = "SELECT  vchnum as Entry_No, to_char(vchdate,'dd/MM/yyyy') as Entry_Date, grpcode as GroupCode, acode As Code, dramt As debit_Amt, cramt As Credit_Amt, iqtyout as Quantity ,invno as Invoice_No,to_char(invdate,'dd/MM/yyyy') as Invoice_Date, ent_by,ent_dt FROM wb_fa_vch where branchcd='" + frm_mbr + "' and type='50' and vchdate " + PrdRange + " order by vchnum";
                    break;
                case "F70557":
                    SQuery = "SELECT  vchnum as Insurance_doc_No, to_char(vchdate,'dd/MM/yyyy') as Insurance_doc_Date, grpcode as GroupCode, acode As Code, ASSETVAL AS INSURANCE_VALUE,ASSETVAL1 AS INSURANCE_PREMIUM,to_char(instdt,'dd/mm/yyyy') as insuance_doc_date,mrr_ref as policy_no ,to_char(life_end,'dd/mm/yyyy') as renewal_date,invno as Invoice_No,to_char(invdate,'dd/MM/yyyy') as Invoice_Date, ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_Dt,naration as remarks FROM wb_fa_vch where branchcd='" + frm_mbr + "' and type='11' and vchdate " + PrdRange + " order by vchnum";
                    break;
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
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a_Text + "'  ", "ldt");
            if (last_entdt == "0" || edmode.Value == "Y")
            {
            }
            else
            {
                if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Value.ToString()))
                {
                    Checked_ok = "N";
                    Checked_ok = "Y";
                    //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + Convert.ToDateTime(txtvchdate.Value.Trim()).ToString("dd/MM/yyyy") + ",Please Check !!");
                }
            }
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Value.ToString()) > Convert.ToDateTime(last_entdt))
            {
                //Checked_ok = "N";
                //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + Convert.ToDateTime(txtvchdate.Value.Trim()).ToString("dd/MM/yyyy") + " ,Please Check !!");
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

                        oDS2 = new DataSet();
                        oporow2 = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "wb_fa_vch");

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
                        // save_fun2();
                        //save_fun3();
                        //save_fun4();
                        // save_fun5();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "wb_fa_vch");

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
                                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Value.Trim()).ToString("yyyy-mm-dd"), "", frm_uname);
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
                        // save_fun2();
                        //   save_fun4();
                        //  save_fun5();
                        if (edmode.Value == "Y")
                        {
                            //ddl_fld1 = fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                            //string type_depr = "40";
                            //ddl_fld2 = fgenMV.Fn_Get_Mvar(frm_qstr,"" ).Substring(0, 2) + type_depr + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr").Substring(3, 17);
                            string mycmd = "";
                            mycmd = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'yyyy-mm-dd')='" + frm_mbr + frm_vty + txtvchnum.Value.Trim() + txtvchdate.Value.Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mycmd);
                            // mycmd = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'yyyy-mm-dd')='" + frm_mbr + "40" + txtvchnum.Value.Trim() + txtvchdate.Value.Trim() + "'";
                            // fgen.execute_cmd(frm_qstr, frm_cocd, mycmd );
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        // fgen.save_data(frm_qstr, frm_cocd, oDS2, "wb_fa_vch");
                        //fgen.save_data(frm_cocd, oDS4, "budgmst");
                        //fgen.save_data(frm_cocd, oDS5, "udf_Data");
                        //fgen.save_data(frm_cocd, oDS2, "ivchctrl");
                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");
                            string mycmd2 = "";
                            mycmd2 = "delete from " + frm_tabname + " where branchcd='DD' and type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'yyyy-mm-dd')='" + frm_vty + txtvchnum.Value.Trim() + txtvchdate.Value.Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mycmd2);
                            // mycmd2 = "delete from " + frm_tabname + " where branchcd='DD' and type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'yyyy-mm-dd')='" + "40" + txtvchnum.Value.Trim() + txtvchdate.Value.Trim() + "'";
                            // fgen.execute_cmd(frm_qstr, frm_cocd, mycmd2 );
                            //fgen.execute_cmd(frm_qstr,frm_cocd, "delete from poterm where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + ddl_fld2 + "'");
                            //fgen.execute_cmd(frm_qstr,frm_cocd, "delete from budgmst where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + ddl_fld2 + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data where branchcd='DD' and par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + ddl_fld2 + "'");
                            //fgen.execute_cmd(frm_qstr,frm_cocd, "delete from ivchctrl where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
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

                                fgen.msg("-", "AMSG", "Entry no. " + txtvchnum.Value + " is Saved");
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

    void save_fun()
    {
        cal();
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();
        switch (Prg_Id)
        {
            case "F70423":
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["VCHNUM"] = txtvchnum.Value;
                oporow["VCHDATE"] = fgen.make_def_Date(txtvchdate.Value.Trim(), vardate);
                oporow["GRPCODE"] = txtgrpcode.Value.ToUpper().Trim();
                oporow["ACODE"] = txtlbl8.Value.ToUpper().Trim();
                if (fgen.make_double(txt_wdv_new.Value.Trim()) > 0)
                {
                    oporow["DRAMT"] = fgen.make_double(txt_wdv_new.Value.Trim());
                    oporow["CRAMT"] = 0;
                }
                else
                {
                    oporow["DRAMT"] = 0;
                    oporow["CRAMT"] = fgen.make_double(txt_wdv_new.Value.Trim()) * -1;
                }
                oporow["INVNO"] = txt_invno.Value.Trim();
                oporow["INVDATE"] = txtlbl5.Value.Trim();
                oporow["NARATION"] = txtrmk.Text.Trim();
                oporow["ASSETVAL"] = fgen.make_double(txtWdv_val.Value.Trim());
                oporow["INSTDT"] = fgen.make_def_Date(txtdate_rev.Value.Trim(), vardate);
                oporow["ASSETVAL1"] = fgen.make_double(txt_wdv_new.Value.Trim());
                oporow["SRNO"] = 0;
                oporow["iqtyin"] = 0;
                oporow["iqtyout"] = 0;
                //oporow["instdt"] = txtlbl5.Value.ToString().Trim();
                oporow["depr_wbk"] = fgen.make_double(txt_bal_new.Value);
                oporow["depr_old"] = fgen.make_double(txt_deprday.Value);
                oporow["salevalue"] = txt_resi_new.Value.ToString().Trim();
                oporow["depr"] = fgen.make_double(txt_daysnew.Value);
                oporow["fvchdate"] = fgen.make_def_Date(txtlbl5.Value.Trim(), vardate);
                //DateTime d1 = Convert.ToDateTime(txtlbl5.Value);
                //DateTime d2 = Convert.ToDateTime(txtdate_rev.Value);
                //String diff_dates = (d1 - d2).TotalDays.ToString();
                oporow["deprdays"] = fgen.make_double(txt_newdeprdays.Value);
                oporow["sale_ent"] = 0;
                oporow["iunit"] = 0;
                break;

            case "F70557":
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["VCHNUM"] = txtvchnum.Value;
                oporow["VCHDATE"] = fgen.make_def_Date(txtvchdate.Value.Trim(), vardate);
                oporow["GRPCODE"] = txtgrpcode.Value.ToUpper().Trim();
                oporow["ACODE"] = txtlbl8.Value.ToUpper().Trim();
                oporow["INVNO"] = txt_invno.Value.Trim();
                oporow["INVDATE"] = fgen.make_def_Date(txtlbl5.Value.Trim(), vardate);
                oporow["NARATION"] = txtrmk.Text.Trim();
                oporow["SRNO"] = 0;
                oporow["DRAMT"] = fgen.make_double(txt_wdv_value_chng.Value.Trim());
                oporow["INSTDT"] = fgen.make_def_Date(txtdate_rev.Value.Trim(), vardate);
                oporow["ASSETVAL"] = fgen.make_double(txt_wdv_new.Value.Trim());
                oporow["ASSETVAL1"] = fgen.make_double(txt_resi_new.Value.ToString().Trim());
                oporow["MRR_REF"] = txt_bal_new.Value.Trim();
                oporow["LIFE_END"] = fgen.make_def_Date(txt_lifeend.Value.Trim(), vardate);
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
        for (i = 0; i < sg4.Rows.Count - 0; i++)
        {
            if (((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim().Length > 1)
            {
                oporow5 = oDS5.Tables[0].NewRow();
                oporow5["branchcd"] = frm_mbr;
                oporow5["par_tbl"] = frm_tabname.ToUpper().Trim();
                oporow5["par_fld"] = frm_mbr + lbl1a_Text + frm_vnum + Convert.ToDateTime(txtvchdate.Value.Trim()).ToString("dd/MM/yyyy");
                oporow5["udf_name"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                oporow5["udf_value"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
                oporow5["SRNO"] = i;

                oDS5.Tables[0].Rows.Add(oporow5);
            }
        }
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

    protected void btnAtt_Click(object sender, EventArgs e)
    {
        string filepath = @"c:\TEJ_ERP\UPLOAD\";      //Server.MapPath("~/tej-base/UPLOAD/");
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            txtAttch.Text = Attch.FileName;
            filepath = filepath + "_" + txtvchnum.Value.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
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
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "c:/tej_erp/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Tejaxo Viewer');", true);
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
    protected void btnCocd_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ASSET";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Code", frm_qstr);
    }

    protected void btnSup_Click(object sender, ImageClickEventArgs e)
    {

        hffield.Value = "SUP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Supplied By", frm_qstr);
    }

    //protected void btnInvo_Click(object sender, ImageClickEventArgs e)
    //{

    //    hffield.Value = "INV";
    //    make_qry_4_popup();
    //    fgen.Fn_open_sseek("Select Sale Invoice", frm_qstr);
    //}

    protected void btnvalidate_ServerClick(object sender, EventArgs e)
    {
        switch (Prg_Id)
        {
            case "F70423":
                cal();
                break;
            case "F70557":
                cal1();
                break;
        }
        if (flag == 0)
        {
            fgen.msg("-", "AMSG", "  This forms Validates Successfully.Please press Save button to save the entry.");
            btnsave.Disabled = false;
            return;
        }
    }
    public void cal()
    {
        btnsave.Disabled = true;
        int dhd1 = fgen.ChkDate(txtdate_rev.Value.ToString());
        if (dhd1 == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Adjustment Date"); txtdate_rev.Focus();
            flag = 1; return;
        }

        if ((txtlbl8.Value.Length < 1) || (txtlbl8.Value == "-"))
        {
            fgen.msg("-", "AMSG", "Please Select Asset Code !!"); txtlbl8.Focus(); flag = 1;
            return;
        }

        if ((txt_wdv_new.Value == "-") || (txt_wdv_new.Value.Length < 1))
        {
            fgen.msg("-", "AMSG", "Please Enter the adjustment value !!"); txt_wdv_new.Focus(); flag = 1;
            return;
        }

        if ((txt_resi_new.Value == "-") || (txt_resi_new.Value.Length < 1))
        {
            fgen.msg("-", "AMSG", "Please Enter the revised Residual value !! else put old residual value."); txt_resi_new.Focus(); flag = 1;
            return;
        }

        if (fgen.make_double(txt_wdv_new.Value) > 0)
        {
            if ((fgen.make_double(txt_resi_new.Value) > fgen.make_double(txt_wdv_new.Value)))
            {
                fgen.msg("-", "AMSG", "Please Enter the Correct Residual value. It cannot be more than adjustment value. !!"); txt_resi_new.Focus(); flag = 1;
                return;
            }
        }
        if (txtrmk.Text.Trim().Length <= 2)
        {
            fgen.msg("-", "AMSG", "Please enter valid adjustment reasons  !!"); txtrmk.Focus(); flag = 1;
            return;
        }

        if ((fgen.make_double(txtWdv_val.Value.Trim()) + fgen.make_double(txt_wdv_new.Value.Trim())) <=0)
        {
            fgen.msg("-", "AMSG", "Please enter correct adjustment Value.The adjustment value cannot be less than current WDV !!"); txt_wdv_new.Focus(); flag = 1;
            return;
        }

        if (txt_wdv_new.Value.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please enter correct value  !!"); txt_wdv_new.Focus(); flag = 1;
            return;
        }

        if (txtdate_rev.Value.Trim().Length <= 2)
        {
            fgen.msg("-", "AMSG", "Please enter adjustment date  !!"); txtdate_rev.Focus(); flag = 1;
            return;
        }

        if (txtlbl5.Value.Trim().Length > 3)
        {
            if (Convert.ToDateTime(txtdate_rev.Value.Trim()) < Convert.ToDateTime(txtlbl5.Value.Trim()))
            {
                fgen.msg("-", "AMSG", "Adjustment date must be greater than Invoice Date.  !!"); txtdate_rev.Focus(); flag = 1;
                return;
            }
        }

        if (Convert.ToDateTime(txtdate_rev.Value) < Convert.ToDateTime(txtlbl5.Value))
        {
            fgen.msg("-", "AMSG", "Please Enter the Correct adjustment Date!! Cannot be less than the Installation Date"); txtdate_rev.Focus(); flag = 1;
            return;
        }

        if (Convert.ToDateTime(txtdate_rev.Value) > Convert.ToDateTime(txt_lifeend.Value))
        {
            fgen.msg("-", "AMSG", "Please Enter the Correct adjustment Date!! Cannot be more than the Life End Date"); txtdate_rev.Focus(); flag = 1;
            return;
        }

        DateTime d1 = Convert.ToDateTime(txt_lifeend.Value);
        DateTime d2 = Convert.ToDateTime(txtdate_rev.Value);
        String diff_dates = (d1 - d2).TotalDays.ToString();

        txt_bal_new.Value = (Convert.ToDouble(diff_dates) + 1).ToString();
        if (fgen.make_double(txt_wdv_new.Value) > 0)
        {
            txt_newdeprdays.Value = Math.Round((fgen.make_double(txt_wdv_new.Value) - (fgen.make_double(txt_resi_new.Value))) / fgen.make_double(txt_bal_new.Value), 2).ToString().Trim();
            txt_daysnew.Value = (fgen.make_double(txt_newdeprdays.Value) + fgen.make_double(txt_deprday.Value)).ToString();
        }//txt_newdeprdays.Value = Math.Round((fgen.make_double(txt_wdv_value_chng.Value) / fgen.make_double(txt_bal_new.Value)), 2).ToString();
        else
        {
             double vdep = Math.Round((fgen.make_double(txt_wdv_new.Value) + (fgen.make_double(txt_resi_new.Value))) / fgen.make_double(txt_bal_new.Value),2);
             txt_newdeprdays.Value = vdep.ToString().Trim();
            txt_daysnew.Value = (fgen.make_double(txt_newdeprdays.Value) - fgen.make_double(txt_deprday.Value)).ToString();
        }
    }
    public void cal1()
    {
        btnsave.Disabled = true;
        int dhd1 = fgen.ChkDate(txtdate_rev.Value.ToString());
        if (dhd1 == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Adjustment Date"); txtdate_rev.Focus();
            flag = 1; return;
        }

        dhd1 = fgen.ChkDate(txt_lifeend.Value.ToString());
        if (dhd1 == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Renewal Date"); txtdate_rev.Focus();
            flag = 1; return;
        }

        if ((txtlbl8.Value.Length < 1) || (txtlbl8.Value == "-"))
        {
            fgen.msg("-", "AMSG", "Please Select Asset Code !!"); txtlbl8.Focus(); flag = 1;
            return;
        }

        if ((txt_wdv_new.Value == "-") || (txt_wdv_new.Value.Length < 1))
        {
            fgen.msg("-", "AMSG", "Please Enter the Insurance value !!"); txt_wdv_new.Focus(); flag = 1;
            return;
        }

        if ((txt_resi_new.Value == "-") || (txt_resi_new.Value.Length < 1))
        {
            fgen.msg("-", "AMSG", "Please Enter the Insurance Premium value !! else put old Insurance Premium."); txt_resi_new.Focus(); flag = 1;
            return;
        }

        if (txtrmk.Text.Trim().Length <= 2)
        {
            fgen.msg("-", "AMSG", "Please enter valid Insurance Reasons  !!"); txtrmk.Focus(); flag = 1;
            return;
        }
        if ((txt_bal_new.Value == "-") || (txt_bal_new.Value.Length < 1))
        {
            fgen.msg("-", "AMSG", "Please Enter the Policy No."); txt_resi_new.Focus(); flag = 1;
            return;
        }

        if (txtdate_rev.Value.Trim().Length <= 2)
        {
            fgen.msg("-", "AMSG", "Please enter Insurance Document Date !!"); txtdate_rev.Focus(); flag = 1;
            return;
        }

        if (txtlbl5.Value.Trim().Length > 3)
        {
            if (Convert.ToDateTime(txtdate_rev.Value.Trim()) < Convert.ToDateTime(txtlbl5.Value.Trim()))
            {
                fgen.msg("-", "AMSG", "Adjustment date must be greater than Invoice Date.  !!"); txtdate_rev.Focus(); flag = 1;
                return;
            }
        }

        if (Convert.ToDateTime(txtdate_rev.Value) < Convert.ToDateTime(txtlbl5.Value))
        {
            fgen.msg("-", "AMSG", "Please Enter the Correct adjustment Date!! Cannot be less than the Installation Date"); txtdate_rev.Focus(); flag = 1;
            return;
        }

        if (Convert.ToDateTime(txtdate_rev.Value) > Convert.ToDateTime(txt_lifeend.Value))
        {
            fgen.msg("-", "AMSG", "Please Enter the Correct adjustment Date!! Cannot be more than the Life End Date"); txtdate_rev.Focus(); flag = 1;
            return;
        }
        btnvalidate.Disabled = true;
        //DateTime d1 = Convert.ToDateTime(txt_lifeend.Value);
        //DateTime d2 = Convert.ToDateTime(txtdate_rev.Value);
        //String diff_dates = (d1 - d2).TotalDays.ToString();

        // txt_bal_new.Value = (Convert.ToDouble(diff_dates) + 1).ToString();
        //   txt_newdeprdays.Value = Math.Round((fgen.make_double(txt_wdv_new.Value) - (fgen.make_double(txt_resi_new.Value))) / fgen.make_double(txt_bal_new.Value), 2).ToString().Trim();
        //txt_newdeprdays.Value = Math.Round((fgen.make_double(txt_wdv_value_chng.Value) / fgen.make_double(txt_bal_new.Value)), 2).ToString();
        //txt_daysnew.Value = (fgen.make_double(txt_newdeprdays.Value) - fgen.make_double(txt_deprday.Value)).ToString();
    }

}


