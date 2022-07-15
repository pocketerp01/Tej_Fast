using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class acct_gen : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;

    DataTable sg1_dt; DataRow sg1_dr;

    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string html_body = "", MV_CLIENT_GRP = "";
    string Prg_Id, lbl1a_Text, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;

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
                    MV_CLIENT_GRP = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CLIENT_GRP");
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
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                doc_addl.Value = "1";
                string chk_opt = "";
                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                doc_GST.Value = "Y";
                //GSt india
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2017'", "fstr");
                if (chk_opt == "N")
                {
                    doc_GST.Value = "N";
                }

                lblheader.Text = "Accounts Master (Level 4)";

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
            if (CSR.Length > 1 || frm_ulvl == "3")
            {


            }
            if (lblUpload.Text.Length > 1)
            {
                btnView1.Visible = true;
                btnDwnld1.Visible = true;
            }
            //btnprint.Visible = false;

            txtWebLogin.Attributes.Add("type", "password");
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

        //tab5.Visible = false;
        tab7.Visible = false;




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

        sg3_add_blankrows();
        sg4_add_blankrows();



        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();


        sg4.DataSource = sg4_dt; sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();

        ImageButton1.Enabled = false; ImageButton3.Enabled = false; ImageButton9.Enabled = false;
        ImageButton8.Enabled = false; ImageButton2.Enabled = false; ImageButton7.Enabled = false;
        ImageButton10.Enabled = false;
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

        ImageButton1.Enabled = true; ImageButton3.Enabled = true; ImageButton9.Enabled = true;
        ImageButton8.Enabled = true; ImageButton2.Enabled = true; ImageButton7.Enabled = true;
        ImageButton10.Enabled = true;

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
        doc_nf.Value = "acode";
        doc_df.Value = "acode";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "famst";

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "AC");
        typePopup = "N";

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);

        tab6.Visible = false;

        divAero.Visible = false;
        if (frm_cocd == "AERO")
            divAero.Visible = true;
        if (frm_cocd == "AERO")
        {
            Label51.InnerText = "Hide_Cust_Name(Exp)";
            Label59.InnerText = "OtherInfo";
            txt_drg_lic.Attributes.Add("placeholder", "");
        }
        if (MV_CLIENT_GRP == "SG_TYPE")
        {
            Label31.InnerText = "CR._No.";
            Label32.InnerText = "VAT No.";
            //instead of CIN No. for SGRP
            Label64.InnerText = "Arabic_Language_Name";
            Label65.InnerText = "Arabic_Language_Address";
        }
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
        string home_cntry = "";
        switch (btnval)
        {
            case "ACNBUT":
                SQuery = "Select type1 as fstr,Name as Nature_Of_Account,Type1 as Code from type where id='#' order by type1";
                break;
            case "MGRBUT":
                string acnat;
                acnat = txt_led_nat.Value.Trim().Substring(0, 1);
                switch (acnat)
                {
                    case "0":
                        SQuery = "Select type1 as fstr,Name as Main_Grp_Name,Type1 as Code from type where id='Z' and type1 like '0%' order by type1";
                        break;
                    case "1":
                        SQuery = "Select type1 as fstr,Name as Main_Grp_Name,Type1 as Code from type where id='Z' and type1 like '1%' order by type1";
                        break;
                    case "2":
                        SQuery = "Select type1 as fstr,Name as Main_Grp_Name,Type1 as Code from type where id='Z' and type1 like '2%' order by type1";
                        break;
                    case "3":
                        SQuery = "Select type1 as fstr,Name as Main_Grp_Name,Type1 as Code from type where id='Z' and type1 like '3%' order by type1";
                        break;
                    case "4":
                        SQuery = "Select type1 as fstr,Name as Main_Grp_Name,Type1 as Code from type where id='Z' and substr(type1,1,1) >= '4' order by type1";
                        break;
                    case "5":
                        SQuery = "Select type1 as fstr,Name as Main_Grp_Name,Type1 as Code from type where id='Z' and substr(type1,1,1) >= '5' order by type1";
                        break;
                    case "9":
                        SQuery = "Select type1 as fstr,Name as Main_Grp_Name,Type1 as Code from type where id='Z' and substr(type1,1,1) >= '9' order by type1";
                        break;
                    default:
                        SQuery = "Select type1 as fstr,Name as Main_Grp_Name,Type1 as Code from type where id='Z' and type1 like '" + acnat + "%' order by type1";
                        break;
                }

                break;
            case "SCHBUT":
                if (txt_led_grp.Value.Length < 2)
                {
                    return;
                }
                SQuery = "Select type1 as fstr,Name as Schedule_Name,Type1 as Code from typegrp where branchcd!='DD' and id='A' and substr(type1,1,2) = '" + txt_led_grp.Value.Trim().Substring(0, 2) + "' order by type1";
                break;

            case "CONTBUT":
                SQuery = "select name as fstr,name as Continent ,type1 as code from typegrp where branchcd!='DD' and id='NM' order by name ";
                break;
            case "CTRYBUT":
                SQuery = "select trim(name) as fstr,trim(name)  as Country ,type1 as code from typegrp where branchcd!='DD' and id='CN' and trim(acref)='" + txt_cont_name.Value.ToString().Trim() + "' order by trim(name)  ";
                break;
            case "STATBUT":
                home_cntry = (txt_ctry_name.Value.ToString().ToUpper().Trim() == "INDIA") ? "from type where id= '{'" : " from typegrp where id='ES' and branchcd!='DD' and trim(acref)='" + txt_ctry_name.Value.ToString().Trim() + "'  ";
                SQuery = "select name as fstr,name as State_Name ,type1 as code " + home_cntry + " order by name ";
                break;
            case "DISTBUT":
                SQuery = "select name as fstr,name as District ,acref2 as State,Acref as country,type1 as code from typegrp where branchcd!='DD' and id='DT' and trim(acref2)='" + txt_stat_name.Value.ToString().Trim() + "' order by name ";
                break;
            case "BRWISEBUT":
                SQuery = "Select type1 as fstr,Name as Branch_Name,Type1 as Code from type where id='B' and upper(nvl(br_close,'-'))!='Y' order by type1";
                break;

            case "ZONEBUT":
                SQuery = "select trim(type1)||':'||trim(name) as fstr,name as Industry_Name ,type1 as code from typegrp where branchcd!='DD' and id='SI' order by name ";
                break;
            case "SEGMBUT":
                SQuery = "select trim(type1)||':'||trim(name) as fstr,name as Segment_Name ,type1 as code from typegrp where branchcd!='DD' and id='SM' order by name ";
                break;
            case "RSM":
                SQuery = "SELECT trim(Type1)||':'||Name as Fstr,Name,Type1 as Code,Acref as ASM_Name,Acref2 as RSM_Name from typegrp where branchcd!='DD' and id='EM' order by type1";
                break;
            case "CONTR_TERM":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='<' and substr(type1,1,1)='0'  order by type1";
                break;
            case "PAY_TERM":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='G' and substr(type1,1,1)='5' order by type1";
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
            case "PCUR":
                SQuery = "SELECT name as fstr,NAME as currency,Stform as RATE FROM TYPE WHERE ID='A' ORDER BY TYPE1";
                break;
            case "PTERMS":
                SQuery = "SELECT TYPE1 as fstr,TYPE1 as code,NAME as Description,is_number(Stform) as Interest FROM TYPE WHERE ID='G' and substr(type1,1,1)='5' ORDER BY TYPE1";
                break;
            case "PTAX":
                SQuery = "SELECT a.acref AS fstr,a.acref AS CODE,a.NAME as description,a.num6 as vat_rate FROM TYPEGRP a WHERE a.ID='T1' ORDER BY a.acref,a.num6";
                break;
            case "REG":
                SQuery = "select TRIM(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum as regis_entry_no,to_char(vchdate,'dd/mm/yyyy') as registeration_date,trim(acode) as acode,trim(aname) as aname,trim(bill_Addr) as bill_Address,trim(sh_name) as short_name,trim(city) as city from wb_famstdtl where branchcd='" + frm_mbr + "' and type in ('CR','VR') AND NVL(TRIM(ACODE),'-')='-' order by aname";
                break;
            case "TDSAC":
                SQuery = "select A.acode as fstr,a.Aname as Account_Name,A.Addr1 as Address_l1,a.addr2 as Address_l2,a.acode as ERP_Acode,a.Grp,b.Name,a.BUYCODE as oldcode,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from " + frm_tabname + " a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' AND SUBSTR(A.ACODE,1,2)='07' order by A.aname ";
                break;
            default:
                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD" || btnval == "Atch_E")
                    SQuery = "select A.acode as fstr,a.Aname as Account_Name,A.Addr1 as Address_l1,a.addr2 as Address_l2,a.acode as ERP_Acode,a.Grp,b.Name,a.BUYCODE as oldcode,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from " + frm_tabname + " a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' order by A.aname ";
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

            if (typePopup == "N")
            {
                newCase(frm_vty);
                fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Account'13'(No for make it new)");
                hffield.Value = "NEW_E";
            }
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }

            if (frm_ulvl == "3")
            {
                //txtlbl4.Value = frm_uname;
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

        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "'  ", 6, "VCH");


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



        sg3_dt = new DataTable();
        create_tab3();
        sg3_add_blankrows();

        setColHeadings();


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

        ImageButton1.Focus();
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " to edit", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        string chk_ind_gst = "";
        chk_ind_gst = fgen.seek_iname(frm_qstr, frm_cocd, "select upper(Trim(opt_enable)) as opt from fin_Rsys_opt_pw where branchcd='" + frm_mbr + "' and trim(opt_id)='W2017' ", "opt");

        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }

        fgen.fill_dash(this.Controls);


        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;


        if (txt_led_nat.Value.Trim().Length < 1)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Nature of A/c";
        }
        if (txt_led_grp.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Ledger Group";
        }

        if (txt_led_Sch.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Ledger Sub Group";

        }

        if (txt_aname.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Account Name";

        }

        if (txt_led_grp.Value.Trim().Length > 1)
        {
            // comment the code for some time. - AS PER MR. SUMIT ANAND #19/03/2021

            if ((txt_led_grp.Value.Trim().Substring(0, 2) == "05" 
                || txt_led_grp.Value.Trim().Substring(0, 2) == "06" 
                || txt_led_grp.Value.Trim().Substring(0, 2) == "16") 
                && !txt_over_sea.Value.Trim().Equals("Y"))
            {
                if (txt_stat_name.Value.Trim().Length < 2)
                {
                    reqd_nc = reqd_nc + 1;
                    reqd_flds = reqd_flds + " / " + "State Name";

                }
                if (txt_addr_1.Value.Trim().Length < 2)
                {
                    reqd_nc = reqd_nc + 1;
                    reqd_flds = reqd_flds + " / " + "Address";

                }

                if (txt_gst_no.Value.Trim().Length < 2 && chk_ind_gst != "N")
                {
                    reqd_nc = reqd_nc + 1;
                    reqd_flds = reqd_flds + " / " + "VAT/GST No.";

                }
                if (txt_mail_1.Value.Trim().Length < 2)
                {
                    reqd_nc = reqd_nc + 1;
                    reqd_flds = reqd_flds + " / " + "Email-Id";

                }

                if (MV_CLIENT_GRP == "SG_TYPE" && txt_led_grp.Value.Trim().Substring(0, 2) == "16" && fgen.make_double(txtMarkup.Value.Trim()) == 0 && (frm_mbr == "00" || frm_mbr == "10" || frm_mbr == "20" || frm_mbr == "30"))
                {
                    reqd_nc = reqd_nc + 1;
                    reqd_flds = reqd_flds + " / " + "Basic Markup";

                }
                if (MV_CLIENT_GRP == "SG_TYPE" && txt_led_grp.Value.Trim().Substring(0, 2) == "16" && fgen.make_double(txtMinMarkup.Value.Trim()) == 0 && (frm_mbr == "00" || frm_mbr == "10" || frm_mbr == "20" || frm_mbr == "30"))
                {
                    reqd_nc = reqd_nc + 1;
                    reqd_flds = reqd_flds + " / " + "Minimum Markup";

                }
                if (MV_CLIENT_GRP == "SG_TYPE" && txt_led_grp.Value.Trim().Substring(0, 2) == "16" && fgen.make_double(txtMaxMarkup.Value.Trim()) == 0 && (frm_mbr == "00" || frm_mbr == "10" || frm_mbr == "20" || frm_mbr == "30"))
                {
                    reqd_nc = reqd_nc + 1;
                    reqd_flds = reqd_flds + " / " + "Maximum Markup";

                }


                if (MV_CLIENT_GRP == "SG_TYPE" && txt_led_grp.Value.Trim().Substring(0, 2) == "16" && txtCurrency.Value.Trim().Length < 2 && (frm_mbr == "00" || frm_mbr == "10" || frm_mbr == "20" || frm_mbr == "30"))
                {
                    reqd_nc = reqd_nc + 1;
                    reqd_flds = reqd_flds + " / " + "Currency ";

                }

                if (txt_ctry_name.Value.Trim().Length < 2)
                {
                    reqd_nc = reqd_nc + 1;
                    reqd_flds = reqd_flds + " / " + "Country Name";

                }
                if (txt_cont_pers.Value.Trim().Length < 2)
                {
                    reqd_nc = reqd_nc + 1;
                    reqd_flds = reqd_flds + " / " + "Contact Person";

                }

                if (txt_zone_name.Value.Trim().Length < 2 && txt_led_grp.Value.Trim().Substring(0, 2) == "16")
                {
                    reqd_nc = reqd_nc + 1;
                    reqd_flds = reqd_flds + " / " + "Industry Type";
                }
            }
        }

        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }

        if (fgen.make_double(txt_TDS_perc.Value.Trim()) > 100)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,TDS % should be less than 100");
            return;
        }
        if (fgen.make_double(txt_TCS_perc.Value.Trim()) > 100)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,TCS % should be less than 100");
            return;
        }
        if (fgen.make_double(txt_cash_disc.Value.Trim()) > 100)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,Cash disc % should be less than 100");
            return;
        }
        if (fgen.make_double(txt_sale_disc.Value.Trim()) > 100)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,Sale disc  % should be less than 100");
            return;
        }
        txt_aname.Value = txt_aname.Value.ToUpper().Trim();

        if (chk_ind_gst == "Y" && txt_over_sea.Value.ToUpper() != "Y")
        {
            #region validation of pan card
            if ((txt_pan_no.Value.Trim().Length > 3))
            {
                if ((txt_pan_no.Value.Trim().Length < 10))
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. is a 10 Digit Number");
                    txt_pan_no.Focus();
                    return;
                }


                char[] str = txt_pan_no.Value.Trim().Substring(0, 5).ToCharArray();

                for (int i = 0; i < 5; i++)
                {
                    if (str[i] >= 65 && str[i] <= 90)
                    {


                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. is Not Correct (Digit 1-5 has to be An Alphabet)");
                        txt_pan_no.Focus();
                        return;
                    }
                }
                char[] str1 = txt_pan_no.Value.Trim().Substring(5, 4).ToCharArray();
                for (int i = 0; i < 4; i++)
                {
                    if (str1[i] >= 48 && str1[i] <= 57)
                    {


                    }
                    else
                    {

                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. is Not Correct (Digit 6-9 has to be A Number)");
                        txt_pan_no.Focus();
                        return;
                    }
                }

                char[] str2 = txt_pan_no.Value.Trim().Substring(9, 1).ToCharArray();
                for (int i = 0; i < 1; i++)
                {
                    if (str2[i] >= 65 && str2[i] <= 90)
                    {


                    }
                    else
                    {

                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PAN No. is Not Correct (Digit 10 has to be An Alphabet)");
                        txt_pan_no.Focus();
                        return;
                    }
                }



            }
            #endregion

            #region validation of gst
            if ((txt_gst_no.Value.Trim().Length > 3) && txt_ctry_name.Value.ToString().ToUpper().Trim() == "INDIA")
            {
                if ((txt_gst_no.Value.Trim().Length < 15))
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "GST No. is a 15 Digit Number");
                    txt_pan_no.Focus();
                    return;
                }

                char[] str = txt_gst_no.Value.Trim().Substring(0, 2).ToCharArray();

                for (int i = 0; i < 2; i++)
                {
                    if (str[i] >= 48 && str[i] <= 57)
                    {


                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "GST No. is Not Correct (Digit 1-2 has to be Numeric)");
                        txt_pan_no.Focus();
                        return;
                    }
                }

                char[] str1 = txt_gst_no.Value.Trim().Substring(12, 3).ToCharArray();

                for (int i = 0; i > 2 && i < 13; i++)
                {
                    if (str1[i] >= 48 || str1[i] <= 57 || str1[i] >= 65 || str1[i] <= 90)
                    {


                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "GSTNO. has to Contain Alphabets / Numeric Values only");
                        txt_pan_no.Focus();
                        return;
                    }
                }

            }
            #endregion

            #region validation of gst +pan card
            if ((txt_gst_no.Value.Trim().Length == 15) && (txt_pan_no.Value.Trim().Length == 10))
            {
                if ((txt_gst_no.Value.Trim().Substring(2, 10) != txt_pan_no.Value))
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "GST/PANNo. is Not Matching");
                    txt_gst_no.Focus();
                    return;

                }

            }

            #endregion
        }


        if (edmode.Value == "Y")
        {

        }
        else
        {
            string chk_code;
            string acnat;
            acnat = txt_led_grp.Value.Substring(0, 2);

            string pop_cmd = "";

            int padder = 0;
            string digit7code = "N";
            digit7code = fgen.getOption(frm_qstr, frm_cocd, "W0090", "OPT_ENABLE");
            if (digit7code == "Y")
            {
                padder = 1;
            }

            //////string code_pm1 = "3";
            //////string code_pm2 = "4";
            //////string digit7code = "N";
            //////digit7code = fgen.getOption(frm_qstr, frm_cocd, "W0090", "OPT_ENABLE");
            //////if (digit7code == "Y")
            //////{
            //////    code_pm1 = "4";
            //////    code_pm2 = "5";
            //////}


            string uv_numac = "";
            uv_numac = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_ACODE");
            if (uv_numac == "Y")
            {
                pop_cmd = "select LPAD(max(trim(acode)),'7','0') as existcd from famst where branchcd!='DD'";
                chk_code = fgen.seek_iname(frm_qstr, frm_cocd, pop_cmd, "existcd");

                //chk_code = (fgen.make_double(chk_code) + 1).ToString().Trim().PadLeft(7 + padder, '0');
                chk_code = (fgen.make_double(chk_code) + 1).ToString().Trim();

                if (chk_code == "0000001")
                {
                    txt_acode.Value = "1000001";
                }
                else
                {
                    txt_acode.Value = chk_code;
                }

            }
            else
            {
                switch (acnat)
                {
                    case "05":
                    case "06":
                    case "16":
                    case "17":
                        pop_cmd = "select max(trim(substr(acode,4,10)))  as existcd from famst where branchcd!='DD' and substr(acode,1,3)='" + txt_led_grp.Value.Substring(0, 2) + txt_aname.Value.Substring(0, 1) + "'";
                        chk_code = fgen.seek_iname(frm_qstr, frm_cocd, pop_cmd, "existcd");
                        chk_code = (fgen.make_double(chk_code) + 1).ToString().Trim().PadLeft(3 + padder, '0');
                        txt_acode.Value = acnat + txt_aname.Value.Substring(0, 1) + chk_code;
                        break;
                    default:
                        pop_cmd = "select max(trim(substr(acode,3,10))) as existcd from famst where branchcd!='DD' and trim(nvl(GRP,'-'))='" + txt_led_grp.Value.Substring(0, 2) + "'";
                        chk_code = fgen.seek_iname(frm_qstr, frm_cocd, pop_cmd, "existcd");
                        chk_code = (fgen.make_double(chk_code) + 1).ToString().Trim().PadLeft(4 + padder, '0');
                        txt_acode.Value = acnat + chk_code;
                        break;
                }
            }
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE AS COL1 FROM FAMST WHERE TRIM(UPPER(ANAME))='" + txt_aname.Value.Trim().ToUpper() + "' ", "COL1");
            if (col1 != "0")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,Same account is already exist on Code : " + col1);
                return;
            }
        }

        if (fgen.check_special_char(this.Controls) == "Y")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,Please Remove Special Charactor in red marked fields!!");
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



        sg3_add_blankrows();


        sg4_add_blankrows();
        sg4.DataSource = sg4_dt;
        sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();

        ViewState["sg1"] = null;

        ViewState["sg3"] = null;
        ViewState["sg4"] = null;
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        if (frm_cocd == "SGRP")
        {
            hffield.Value = "List";
            fgen.Fn_open_Act_itm_prd("-", frm_qstr);
        }
        else
        {
            hffield.Value = "L1";
            fgen.msg("-", "CMSG", "Want to View list of Accounts with Attached Image'13'(Press No to See Without Image)");
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "PRINT";
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);



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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from famst where trim(Acode) ='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from famstbal where trim(Acode) not in (Select trim(Acode) from famst)");

                // Saving Deleting History

                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"), vardate, frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " '13' " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                clearctrl(); fgen.ResetForm(this.Controls);
            }
        }
        else if (hffield.Value == "L1")
        {
            hffield.Value = "List";
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                hffield.Value = "List1";
            }
            fgen.Fn_open_Act_itm_prd("-", frm_qstr);
        }
        else if (hffield.Value == "NEW_E")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                hffield.Value = "COPY_OLD";
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
            txt_aname.Focus();
        }
        else if (hffield.Value == "DEC")
        {
            txt_deacby.Value = "";
            txt_deacDt.Value = "";
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                txt_deacby.Value = frm_uname;
                txt_deacDt.Value = vardate;
            }
        }
        else if (hffield.Value == "APP")
        {
            txt_appby.Value = "";
            txt_appdt.Value = "";
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                txt_appby.Value = frm_uname;
                txt_appdt.Value = vardate;
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
                    dt = new DataTable();
                    SQuery = "SELECT DISTINCT ACODE FROM VOUCHER WHERE TRIM(aCODE)='" + col1 + "' UNION ALL SELECT DISTINCT ACODE FROM SALE WHERE TRIM(aCODE)='" + col1 + "' UNION ALL SELECT DISTINCT ACODE FROM RECEBAL WHERE TRIM(aCODE)='" + col1 + "' UNION ALL SELECT DISTINCT ACODE FROM FAMSTBAL WHERE TRIM(aCODE)='" + col1 + "' AND YR_" + frm_myear + ">0 ";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        fgen.msg("-", "AMSG", "A/C Code : " + col1 + " (" + col2 + ") has the transactions'13'Can not be deleted!!");
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
                case "Atch_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                    string mcol7 = "";
                    string mcol1 = "";

                    mcol7 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");
                    mcol1 = col1 + mcol7 + "FAM";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", mcol1);
                    fgen.open_fileUploadPopup("Upload File for " + lblheader.Text, frm_qstr);
                    break;

                case "Edit_E":
                case "COPY_OLD":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "" || col1 == "-") return;
                    clearctrl();
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    string mv_col;
                    mv_col = col1;
                    SQuery = "Select nvl(a.ent_Dt,sysdate) as entry_Dt,a.* from " + frm_tabname + " a where a.acode='" + mv_col + "' ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["entry_Dt"].ToString();
                        //vipin
                        txt_led_nat.Value = dt.Rows[i]["grp"].ToString().Trim().Substring(0, 1) + " : " + fgen.seek_iname(frm_qstr, frm_cocd, "select name as fstr from type where id='#' and trim(type1)='" + dt.Rows[i]["grp"].ToString().Trim().Substring(0, 1) + "' ", "fstr");

                        txt_led_grp.Value = dt.Rows[i]["grp"].ToString().Trim().Substring(0, 2) + " :" + fgen.seek_iname(frm_qstr, frm_cocd, "select name as fstr from type where id='Z' and trim(type1)='" + dt.Rows[i]["grp"].ToString().Trim().Substring(0, 2) + "' ", "fstr");

                        txt_led_Sch.Value = dt.Rows[i]["bssch"].ToString().Trim().Substring(0, 4) + " :" + fgen.seek_iname(frm_qstr, frm_cocd, "select name as fstr from typegrp where branchcd!='DD' and id='A' and Trim(type1)='" + dt.Rows[i]["bssch"].ToString().Trim() + "' ", "fstr");


                        txt_alias_name.Value = dt.Rows[i]["pname"].ToString().Trim();

                        txt_aname.Value = dt.Rows[i]["aname"].ToString().Trim();
                        txt_showin.Value = dt.Rows[i]["showinbr"].ToString().Trim();

                        txt_dist_name.Value = dt.Rows[i]["district"].ToString().Trim();
                        txt_stat_name.Value = dt.Rows[i]["staten"].ToString().Trim();
                        txt_stat_code.Value = dt.Rows[i]["staffcd"].ToString().Trim();
                        txtPinCode.Value = dt.Rows[i]["PINCODE"].ToString().Trim();
                        txt_zone_name.Value = dt.Rows[i]["zoname"].ToString().Trim();
                        //chkactype.Checked = dt.Rows[i]["ccode"].ToString().Trim() == "T" ? true : false;//07042021

                        txt_segm_name.Value = dt.Rows[i]["segname"].ToString().Trim();
                        txt_ctry_name.Value = dt.Rows[i]["country"].ToString().Trim();
                        txt_cont_name.Value = dt.Rows[i]["continent"].ToString().Trim();

                        txt_addr_1.Value = dt.Rows[i]["addr1"].ToString().Trim();
                        txt_addr_2.Value = dt.Rows[i]["addr2"].ToString().Trim();
                        txt_addr_3.Value = dt.Rows[i]["addr3"].ToString().Trim();
                        txt_addr_4.Value = dt.Rows[i]["addr4"].ToString().Trim();

                        txt_tel_no.Value = dt.Rows[i]["telnum"].ToString().Trim();
                        txt_mail_1.Value = dt.Rows[i]["email"].ToString().Trim();
                        txt_mail_2.Value = dt.Rows[i]["email2"].ToString().Trim();
                        txt_cont_pers.Value = dt.Rows[i]["person"].ToString().Trim();
                        txt_cont_no.Value = dt.Rows[i]["mobile"].ToString().Trim();

                        if (btnval != "COPY_OLD")
                        {
                            txt_acode.Value = dt.Rows[i]["Acode"].ToString().Trim();
                            txt_pan_no.Value = dt.Rows[i]["girno"].ToString().Trim();
                            txt_cin_no.Value = dt.Rows[i]["cin_no"].ToString().Trim();
                            txt_gst_no.Value = dt.Rows[i]["gst_no"].ToString().Trim();
                            txt_over_sea.Value = dt.Rows[i]["Gstoversea"].ToString().Trim();
                            txt_comp_act.Value = dt.Rows[i]["Gstperson"].ToString().Trim();
                            txt_rev_chg.Value = dt.Rows[i]["GstRevChg"].ToString().Trim();


                            txt_bank_name.Value = dt.Rows[i]["rtg_bank"].ToString().Trim();
                            txt_ac_nat.Value = dt.Rows[i]["rtg_acty"].ToString().Trim();
                            txt_bank_addr.Value = dt.Rows[i]["rtg_addr"].ToString().Trim();
                            txt_bank_acno.Value = dt.Rows[i]["rtg_acno"].ToString().Trim();
                            txt_bank_ifsc.Value = dt.Rows[i]["rtg_ifsc"].ToString().Trim();

                            txt_bank_swift.Value = dt.Rows[i]["rtg_swift"].ToString().Trim();
                            txt_bank_tel.Value = dt.Rows[i]["rtg_tel"].ToString().Trim();

                            txt_pymt_days.Value = dt.Rows[i]["payment"].ToString().Trim();
                            txt_pymt_days.Value = dt.Rows[i]["pay_num"].ToString().Trim();

                            txt_grc_days.Value = dt.Rows[i]["balop"].ToString().Trim();
                            txt_cred_lmt.Value = dt.Rows[i]["climit"].ToString().Trim();


                            txt_dlv_term.Value = dt.Rows[i]["del_Term"].ToString().Trim();
                            txt_cod_term.Value = dt.Rows[i]["del_cod"].ToString().Trim();
                            txt_imp_note.Value = dt.Rows[i]["del_note"].ToString().Trim();
                            txt_way_bill.Value = dt.Rows[i]["del_wayb"].ToString().Trim();
                            txt_oth_note.Value = dt.Rows[i]["oth_notes"].ToString().Trim();

                            txt_drg_lic.Value = dt.Rows[i]["med_lic"].ToString().Trim();
                            txt_vend_code.Value = dt.Rows[i]["vencode"].ToString().Trim();
                            txt_old_code.Value = dt.Rows[i]["buycode"].ToString().Trim();

                            txt_affiliate.Value = dt.Rows[i]["lbt_no"].ToString().Trim();

                            txt_sal_Grp.Value = dt.Rows[i]["mktggrp"].ToString().Trim();
                            txt_cust_grp.Value = dt.Rows[i]["custgrp"].ToString().Trim();

                            txt_TDS_perc.Value = dt.Rows[i]["tdsrate"].ToString().Trim();
                            txt_TCS_perc.Value = dt.Rows[i]["cessrate"].ToString().Trim();
                            txt_cash_disc.Value = dt.Rows[i]["schgrate"].ToString().Trim();
                            txt_sale_disc.Value = dt.Rows[i]["disc"].ToString().Trim();

                            txt_tds_Ac.Value = dt.Rows[i]["drtot"].ToString().Trim().PadLeft(6, '0');
                            txt_gst_rating.Value = dt.Rows[i]["gstrating"].ToString().Trim();
                            txt_non_gst.Value = dt.Rows[i]["gstna"].ToString().Trim();
                            txt_gst_Exp.Value = dt.Rows[i]["gstpvexp"].ToString().Trim();


                            txt_cost_cent.Value = dt.Rows[i]["costcontrol"].ToString().Trim();
                            txt_dlv_days.Value = dt.Rows[i]["dlvtime"].ToString().Trim();
                            txt_intt_bill.Value = dt.Rows[i]["rateint"].ToString().Trim();
                            txt_so_tolr.Value = dt.Rows[i]["so_tolr"].ToString().Trim();

                            txt_sale_mail.Value = dt.Rows[i]["hr_ml"].ToString().Trim();
                            txt_hub_stk.Value = dt.Rows[i]["hubstk"].ToString().Trim();
                            txt_mult_ord.Value = dt.Rows[i]["asa"].ToString().Trim();
                            txt_ins_conv.Value = dt.Rows[i]["DLNO"].ToString().Trim();
                            txtowner.Value = dt.Rows[i]["OWNER"].ToString().Trim();
                            txtownerid.Value = dt.Rows[i]["OWNERID"].ToString().Trim();
                            txtnlaname.Value = dt.Rows[i]["NL_ANAME"].ToString().Trim();
                            txtnladdr.Value = dt.Rows[i]["NL_ADDR"].ToString().Trim();

                            txtWebLogin.Value = dt.Rows[i]["WEBLOGIN"].ToString().Trim();

                            txtMarkup.Value = dt.Rows[i]["BCODE1"].ToString().Trim();
                            txtMinMarkup.Value = dt.Rows[i]["BCODE2"].ToString().Trim();
                            txtMaxMarkup.Value = dt.Rows[i]["BCODE3"].ToString().Trim();

                            txtPayTerms.Value = dt.Rows[i]["PAYTERM"].ToString().Trim();
                            txtCurrency.Value = dt.Rows[i]["CURRCODE"].ToString().Trim();
                            txtTaxCode.Value = dt.Rows[i]["SERVTAXNO"].ToString().Trim();

                            tcsApplicable.Value = dt.Rows[i]["STATUS"].ToString().Trim();

                            txt_tds_codes.Value = dt.Rows[i]["BANK_NAME"].ToString().Trim();

                            if (dt.Rows[i]["CCODE"].ToString().Trim() == "T")
                                txtTpt.Value = "Y";
                            else txtTpt.Value = "N";

                            txt_deacby.Value = dt.Rows[i]["DEAC_BY"].ToString().Trim();
                            txt_deacDt.Value = dt.Rows[i]["DEAC_DT"].ToString().Trim();

                            txtPaymentTerms.Value = dt.Rows[i]["DLVBANK"].ToString().Trim();
                            txtContrTerms.Value = dt.Rows[i]["SEC_CHQBNK"].ToString().Trim();

                            txtRevisePO.Value = dt.Rows[i]["SKIP_PRT"].ToString().Trim();

                            txtCOCNumber.Value = dt.Rows[i]["FBT"].ToString().Trim();

                            string op_bal_fld;
                            op_bal_fld = "YR_" + frm_CDT1.Substring(6, 4);
                            string ibal_data = "";

                            ibal_data = fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(" + op_bal_fld + ",0)||'#'||nvl(ven_code,'-')||'#'||nvl(yr_2003,0) as fstr from famstbal where branchcd='" + frm_mbr + "' and trim(acode)='" + txt_acode.Value.Trim() + "' ", "fstr");
                            if (ibal_data.Contains("#"))
                            {
                                txt_balop.Value = ibal_data.Split('#')[0].ToString();
                                txt_vend_code.Value = ibal_data.Split('#')[1].ToString();
                                txt_balop_fx.Value = ibal_data.Split('#')[2].ToString();
                            }

                            if (dt.Rows[i]["fimglink"].ToString().Trim().Length > 1)
                            {
                                lblUpload.Text = dt.Rows[i]["fimglink"].ToString().Trim();
                                //txtAttch.Text = dt.Rows[i]["filename"].ToString().Trim();
                            }
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


                        }

                        //-----------------------
                        txt_aname.Focus();
                        //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                        if (lblUpload.Text.Length > 1)
                        {
                            btnDwnld1.Visible = true;
                            btnView1.Visible = true;
                        }
                    }
                    #endregion

                    if (btnval == "COPY_OLD")
                    {
                        edmode.Value = "";
                    }
                    set_Val();
                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;
                case "ACNBUT":
                    if (col1.Length <= 0) return;
                    txt_led_nat.Value = col1 + " : " + col2;


                    txt_led_grp.Value = " ";
                    txt_led_Sch.Value = " ";

                    ImageButton3.Focus();
                    break;

                case "MGRBUT":
                    if (col1.Length <= 0) return;
                    txt_led_grp.Value = col1 + " : " + col2;
                    txt_led_Sch.Value = " ";

                    ImageButton9.Focus();
                    //btnlbl7.Focus();
                    break;
                case "SCHBUT":
                    if (col1.Length <= 0) return;
                    txt_led_Sch.Value = col1.Trim() + " : " + col2;

                    txt_aname.Focus();
                    //btnlbl7.Focus();
                    break;

                case "CONTBUT":
                    if (col1.Length <= 0) return;
                    txt_cont_name.Value = col1.Trim();
                    ImageButton10.Focus();
                    break;

                case "CTRYBUT":
                    if (col1.Length <= 0) return;
                    txt_ctry_name.Value = col1.Trim();
                    ImageButton2.Focus();

                    //txt_addr_1.Focus();
                    break;

                case "STATBUT":
                    if (col1.Length <= 0) return;
                    txt_stat_name.Value = col1.Trim();
                    txt_stat_code.Value = col3.Trim();
                    //ImageButton10.Focus();
                    ImageButton8.Focus();
                    break;

                case "DISTBUT":
                    if (col1.Length <= 0) return;
                    txt_dist_name.Value = col1.Trim();

                    ImageButton7.Focus();
                    //btnlbl7.Focus();
                    break;
                case "BRWISEBUT":
                    if (col1.Length <= 0) return;
                    txt_showin.Value = col1.Trim().Replace("'", "`");
                    break;

                case "ZONEBUT":
                    if (col1.Length <= 0) return;
                    txt_zone_name.Value = col1.Trim();
                    //btnlbl7.Focus();
                    break;
                case "SEGMBUT":
                    if (col1.Length <= 0) return;
                    txt_segm_name.Value = col1.Trim();
                    //btnlbl7.Focus();
                    break;
                case "RSM":
                    txt_sal_Grp.Value = col1;
                    break;
                case "PCUR":
                    txtCurrency.Value = col1;
                    break;
                case "PTERMS":
                    txtPayTerms.Value = col1;
                    break;
                case "PTAX":
                    txtTaxCode.Value = col1;
                    break;
                case "REG":
                    if (col1 == "") return;
                    SQuery = "SELECT * FROM WB_FAMSTDTL WHERE TRIM(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        if (txt_aname.Value.Length < 2)
                            txt_aname.Value = dt.Rows[0]["aname"].ToString().Trim();
                        if (txt_ctry_name.Value.Length < 2)
                            txt_ctry_name.Value = dt.Rows[0]["country"].ToString().Trim();
                        if (txt_stat_name.Value.Length < 2)
                            txt_stat_name.Value = dt.Rows[0]["staten"].ToString().Trim();
                        if (txt_dist_name.Value.Length < 2)
                            txt_dist_name.Value = dt.Rows[0]["city"].ToString().Trim();
                        if (txt_gst_no.Value.Length < 2)
                            txt_gst_no.Value = dt.Rows[0]["vend_gst"].ToString().Trim();
                        hf_regis.Value = col1;//fstr value..this is used when update cmd is working on save_fun()--------for aero only
                    }
                    break;
                case "TDSAC":
                    txt_tds_Ac.Value = col1;
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


                    #endregion
                    break;
                case "SG1_ROW_TAX":

                    break;
                case "SG1_ROW_DT":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
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

                case "CONTR_TERM":
                    txtContrTerms.Value = col2;
                    break;
                case "PAY_TERM":
                    txtPaymentTerms.Value = col2;
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        string party_cd = "";
        string part_cd = "";

        if (hffield.Value == "PRINT")
        {
            party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
            part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
            if (party_cd.Trim().Length <= 1)
            {
                party_cd = "";
            }
            else party_cd = " and substr(a.grp,1,2) in (" + party_cd + ") ";
            if (part_cd.Trim().Length <= 1)
            {
                part_cd = "";
            }
            else part_cd = " and a.bssch in (" + part_cd + ")";

            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70172");
            string header_n = "Group Wise A/C List";

            //SQuery = "insert into typegrp(branchcd,id,type1,name)(select distinct '00','A',lpad(trim(bssch),4,'0'),'Sub Grp '||lpad(trim(bssch),4,'0') from famst where trim(bssch) not in (Select trim(type1) from typegrp where id='A'))";
            //fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
            SQuery = "SELECT '" + header_n + "' as header, SUBSTR(TRIM(A.grp),1,2) AS MGCODE,trim(B.NAME) AS MG,A.BSSCH AS SUBCODE,trim(C.NAME) AS SUBNAME,A.ACODE,trim(A.ANAME) as aname,(case when trim(A.EDT_BY)='-' then trim(A.ENT_BY) else trim(A.EDT_BY) end) as ent_by ,(case when trim(A.EDT_BY)='-' then to_char(A.ENT_dt,'dd/mm/yyyy') else to_char(A.EDT_dt,'dd/mm/yyyy') end) as ENT_DT,trim(nvl(a.nl_aname,'-')) as nl_aname,trim(nvl(a.nl_addr,'-')) as nl_addr FROM FAMST A,TYPE B,TYPEGRP C WHERE B.ID='Z' " + party_cd + " " + part_cd + " AND SUBSTR(TRIM(A.grp),1,2)=TRIM(B.TYPE1) AND C.ID='A' AND TRIM(A.BSSCH)=TRIM(C.TYPE1) ORDER BY SUBSTR(TRIM(A.grp),1,2), A.BSSCH, A.ANAME";
            fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, (MV_CLIENT_GRP != "SG_TYPE") ? "std_PartyMaster" : "std_PartyMasternl", (MV_CLIENT_GRP != "SG_TYPE") ? "std_PartyMaster" : "std_PartyMasternl");
            return;
        }

        if (hffield.Value == "List" || hffield.Value == "List1")
        {
            party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
            part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
            if (party_cd.Trim().Length <= 1)
            {
                party_cd = "";
            }
            else party_cd = " and substr(a.grp,1,2) in (" + party_cd + ") ";
            if (part_cd.Trim().Length <= 1)
            {
                part_cd = "";
            }
            else part_cd = " and a.bssch in (" + part_cd + ")";

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");


            string data_fld = "b.Name as Acc_Level_3,a.grp as Grp_Code,a.bssch as Sch_Code,a.Acode as Acc_Code,a.Aname as Ledger_Name,a.pname,a.Addr1,a.Addr2,a.Addr3,a.Addr4,a.district,a.staten,a.country,a.Gst_No as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.girno as PANno,a.cin_no as " + Label31.InnerText.Replace(".", "") + ",a.Telnum,a.Person,a.Designation,a.email,a.email2,a.mobile,a.Payment,a.balop as Grace_Days,a.Climit,a.actype,a.zcode,a.deals_in,a.custgrp,a.mktggrp,a.buycode,a.nl_aname, a.nl_addr,a.deac_by as deactivated_by,(case when nvl(a.deac_by,'-')!='-' then a.deac_dt else null end) as deactivated_dt, a.owner,a.ownerid,a.RTG_BANK,a.RTG_ACTY,a.RTG_IFSC,a.RTG_ACNO,a.RTG_ADDR,replace(a.fimglink,'c:/TEJ_erp/','') AS IMG_SRC,lbt_no from famst a left outer join (select type1,name from typegrp where branchcd='00' and id='A') b on trim(a.bssch)=trim(b.type1) ";


            SQuery = "Select " + data_fld + " where a.branchcd='00' " + party_cd + " " + part_cd + " ";

            SQuery = "Select b.Acct_Nature as Acc_Level_1,b.Acct_Grp as Acc_Level_2,a.* from (" + SQuery + ")a left outer join (Select distinct y.type1,x.name as Acct_Nature,y.name as Acct_Grp from type x,type y where x.id='#'and y.id='Z' and substr(y.type1,1,1)=trim(x.type1) )b on substr(a.grp_code,1,2)=trim(B.type1) ";

            SQuery = "Select ACC_LEVEL_1,GRP_CODE as L2_Code,ACC_LEVEL_2 as Level_2_Name,SCH_CODE as L3_Code,ACC_LEVEL_3  as Level_3_Name,ACC_CODE ,LEDGER_NAME as Account_Name,PNAME,ADDR1,ADDR2,ADDR3,ADDR4," + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",PANNO," + Label31.InnerText.Replace(".", "") + ",TELNUM,PERSON,DESIGNATION,EMAIL,EMAIL2,MOBILE,PAYMENT,GRACE_DAYS,CLIMIT,DISTRICT,STATEN,COUNTRY,ACTYPE,ZCODE,DEALS_IN,CUSTGRP,MKTGGRP,BUYCODE as integra_code,lbt_no as affiliate_code,deactivated_by,deactivated_dt,NL_ANAME,NL_ADDR,OWNER,OWNERID,RTG_BANK,RTG_ACTY,RTG_IFSC,RTG_ACNO,RTG_ADDR,IMG_SRC from (" + SQuery + ") order by grp_Code,sch_Code,acc_code";


            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            if (hffield.Value == "List1") fgen.Fn_open_rptlevelIMG("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            else fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim(), frm_qstr);
            //else fgen.drillQuery(0, SQuery, frm_qstr);
            fgen.Fn_DrillReport("", frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";

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
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from famstbal where trim(Acode) not in (Select trim(Acode) from famst)");

                        oDS = new DataSet();
                        oporow = null;
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);



                        oDS5 = new DataSet();
                        oporow5 = null;
                        oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");


                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();

                        save_fun5();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);



                        oDS5.Dispose();
                        oporow5 = null;
                        oDS5 = new DataSet();
                        oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");


                        if (edmode.Value == "Y")
                        {
                            //frm_vnum = txtvchnum.Value.Trim();
                            save_it = "Y";
                        }

                        else
                        {
                            save_it = "Y";


                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();

                        save_fun5();
                        string ddl_fld1;
                        string ddl_fld2;
                        ddl_fld1 = fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                        ddl_fld2 = fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set acode='ZZ'||trim(replace(acode,'ZZ','')) where trim(" + doc_nf.Value + ")='" + ddl_fld1 + "'");
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);


                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where trim(" + doc_nf.Value + ")='ZZ" + ddl_fld2 + "'");
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
                                fgen.msg("-", "AMSG", lblheader.Text + " " + " Saved Successfully");
                                //fgen.msg("-", "AMSG", "Account Code " + frm_vnum + "'13' has been Saved.");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }

                        if (hf_regis.Value.Length > 10)
                        {
                            {
                                SQuery = "update wb_famstdtl set acode='" + txt_acode.Value.ToUpper().Trim() + "',aname='" + txt_aname.Value.ToUpper().Trim() + "' where BRANCHCD||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + hf_regis.Value + "'";
                                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
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
                        "<td><b>A/c Master Code</b></td><td><b>A/c Master Name</b></td><td><b>User Name</b></td><td><b>Activity Date</b></td><td><b>ID</b></td>");
                        //vipin
                        //foreach (GridViewRow gr in sg1.Rows)
                        //{
                        //    if (gr.Cells[13].Text.Trim().Length > 4)
                        //    {


                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append(txt_acode.Value.Trim());
                        sb.Append("</td>");
                        sb.Append("<td>");
                        sb.Append(txt_aname.Value.Trim());
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
                        fgen.send_Activity_mail(frm_qstr, frm_cocd, "Tejaxo ERP", frm_formID, subj + lblheader.Text + " #" + frm_vnum, sb.ToString(), frm_uname);

                        //fgen.send_Activity_msg(frm_qstr, frm_cocd, frm_formID, subj + lblheader.Text + " #" + frm_vnum + " by " + frm_uname, frm_uname);

                        sb.Clear();
                        #endregion

                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txt_acode.Value.Trim(), frm_uname, edmode.Value);

                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                        hf_regis.Value = "";
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

        //if (txtvchnum.Value == "-")
        //{
        //    fgen.msg("-", "AMSG", "Doc No. not correct");
        //    return;
        //}
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

    }

    //------------------------------------------------------------------------------------
    protected void sg3_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void sg4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg4.Rows[rowIndex].RowIndex);

        //if (txtvchnum.Value == "-")
        //{
        //    fgen.msg("-", "AMSG", "Doc No. not correct");
        //    return;
        //}
        switch (var)
        {
            case "sg4_RMV":
                if (index < sg4.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "sg4_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "sg4_ROW_ADD":
                dt = new DataTable();
                sg4_dt = new DataTable();
                dt = (DataTable)ViewState["sg4"];
                z = dt.Rows.Count - 1;
                sg4_dt = dt.Clone();
                sg4_dr = null;
                i = 0;
                for (i = 0; i < sg4.Rows.Count; i++)
                {
                    sg4_dr = sg4_dt.NewRow();
                    sg4_dr["sg4_srno"] = (i + 1);
                    sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                    sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
                    sg4_dt.Rows.Add(sg4_dr);
                }
                sg4_add_blankrows();
                ViewState["sg4"] = sg4_dt;
                sg4.DataSource = sg4_dt;
                sg4.DataBind();
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

        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = "00";


        oporow["grp"] = txt_led_grp.Value.ToUpper().Trim().Substring(0, 2);
        oporow["bssch"] = txt_led_Sch.Value.ToUpper().Trim().Substring(0, 4);

        oporow["ACODE"] = txt_acode.Value.ToUpper().Trim();
        oporow["pname"] = txt_alias_name.Value.ToUpper().Trim();
        oporow["girno"] = txt_pan_no.Value.ToUpper().Trim();

        oporow["aname"] = txt_aname.Value.ToUpper().Trim();
        oporow["showinbr"] = txt_showin.Value.ToUpper().Trim();

        oporow["district"] = txt_dist_name.Value.ToUpper().Trim();
        oporow["staten"] = txt_stat_name.Value.ToUpper().Trim();
        oporow["staffcd"] = txt_stat_code.Value.ToUpper().Trim();
        oporow["PINCODE"] = txtPinCode.Value.ToUpper().Trim();

        oporow["zoname"] = txt_zone_name.Value.ToUpper().Trim();
        oporow["segname"] = txt_segm_name.Value.ToUpper().Trim();
        //try
        //{
        //    oporow["ccode"] = chkactype.Checked ? "T" : "-";//07042021
        //}
        //catch { }
        oporow["country"] = txt_ctry_name.Value.ToUpper().Trim();
        oporow["continent"] = txt_cont_name.Value.ToUpper().Trim();

        oporow["addr1"] = txt_addr_1.Value.ToUpper().Trim();
        oporow["addr2"] = txt_addr_2.Value.ToUpper().Trim();
        oporow["addr3"] = txt_addr_3.Value.ToUpper().Trim();
        oporow["addr4"] = txt_addr_4.Value.ToUpper().Trim();

        oporow["telnum"] = txt_tel_no.Value.ToUpper().Trim();
        oporow["email"] = txt_mail_1.Value.ToUpper().Trim();
        oporow["email2"] = txt_mail_2.Value.ToUpper().Trim();
        oporow["person"] = txt_cont_pers.Value.ToUpper().Trim();
        oporow["mobile"] = txt_cont_no.Value.ToUpper().Trim();


        oporow["cin_no"] = txt_cin_no.Value.ToUpper().Trim();
        oporow["gst_no"] = txt_gst_no.Value.ToUpper().Trim();
        oporow["gstoversea"] = txt_over_sea.Value.ToUpper().Trim();
        oporow["gstperson"] = txt_comp_act.Value.ToUpper().Trim();
        oporow["GstRevChg"] = txt_rev_chg.Value.ToUpper().Trim();


        oporow["RTG_BANK"] = txt_bank_name.Value.ToUpper().Trim();
        oporow["RTG_ACTY"] = txt_ac_nat.Value.ToUpper().Trim();
        oporow["RTG_ADDR"] = txt_bank_addr.Value.ToUpper().Trim();
        oporow["RTG_ACNO"] = txt_bank_acno.Value.ToUpper().Trim();
        oporow["RTG_IFSC"] = txt_bank_ifsc.Value.ToUpper().Trim();

        oporow["RTG_swift"] = txt_bank_swift.Value.ToUpper().Trim();
        oporow["rtg_tel"] = txt_bank_tel.Value.ToUpper().Trim();
        oporow["payment"] = txt_pymt_days.Value.ToUpper().Trim();
        oporow["pay_num"] = fgen.make_double(txt_pymt_days.Value.ToUpper().Trim());
        oporow["balop"] = fgen.make_double(txt_grc_days.Value.ToUpper().Trim());
        oporow["climit"] = fgen.make_double(txt_cred_lmt.Value.ToUpper().Trim());


        oporow["del_Term"] = txt_dlv_term.Value.ToUpper().Trim();
        oporow["del_COD"] = txt_cod_term.Value.ToUpper().Trim();
        oporow["del_note"] = txt_imp_note.Value.ToUpper().Trim();
        oporow["del_wayb"] = txt_way_bill.Value.ToUpper().Trim();
        oporow["oth_notes"] = txt_oth_note.Value.ToUpper().Trim();

        oporow["med_lic"] = txt_drg_lic.Value.ToUpper().Trim();
        oporow["vencode"] = txt_vend_code.Value.ToUpper().Trim();
        oporow["BUYCODE"] = txt_old_code.Value.ToUpper().Trim();
        oporow["lbt_no"] = txt_affiliate.Value.ToUpper().Trim();

        oporow["mktggrp"] = txt_sal_Grp.Value.ToUpper().Trim().Left(20);
        oporow["custgrp"] = txt_cust_grp.Value.ToUpper().Trim();

        oporow["tdsrate"] = fgen.make_double(txt_TDS_perc.Value.ToUpper().Trim());
        oporow["cessrate"] = fgen.make_double(txt_TCS_perc.Value.ToUpper().Trim());
        oporow["schgrate"] = fgen.make_double(txt_cash_disc.Value.ToUpper().Trim());
        oporow["disc"] = fgen.make_double(txt_sale_disc.Value.ToUpper().Trim());

        oporow["drtot"] = fgen.make_double(txt_tds_Ac.Value.ToUpper().Trim());
        oporow["GstRating"] = fgen.make_double(txt_gst_rating.Value.ToUpper().Trim());
        oporow["gstna"] = txt_non_gst.Value.ToUpper().Trim();
        oporow["gstPVexp"] = txt_gst_Exp.Value.ToUpper().Trim();



        oporow["costcontrol"] = txt_cost_cent.Value.ToUpper().Trim();
        oporow["STDRATE"] = 0;
        oporow["dlvtime"] = fgen.make_double(txt_dlv_days.Value.ToUpper().Trim());
        oporow["rateint"] = fgen.make_double(txt_intt_bill.Value.ToUpper().Trim());
        oporow["so_tolr"] = fgen.make_double(txt_so_tolr.Value.ToUpper().Trim());

        oporow["hr_ml"] = txt_sale_mail.Value.ToUpper().Trim();
        oporow["hubstk"] = txt_hub_stk.Value.ToUpper().Trim();
        oporow["ASA"] = txt_mult_ord.Value.ToUpper().Trim();
        oporow["DLNO"] = txt_ins_conv.Value.ToUpper().Trim();

        oporow["NL_ANAME"] = txtnlaname.Value.ToUpper().Trim();
        oporow["NL_ADDR"] = txtnladdr.Value.ToUpper().Trim();
        oporow["OWNER"] = txtowner.Value.ToUpper().Trim();
        oporow["OWNERID"] = txtownerid.Value.ToUpper().Trim();

        //
        oporow["DLVBANK"] = txtPaymentTerms.Value;
        oporow["SEC_CHQBNK"] = txtContrTerms.Value;

        oporow["SKIP_PRT"] = txtRevisePO.Value;

        oporow["FBT"] = txtCOCNumber.Value;

        string op_bal_fld;
        op_bal_fld = "YR_" + frm_CDT1.Substring(6, 4);
        string chk_code;
        chk_code = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(acode) as existcd from famstbal where branchcd='" + frm_mbr + "' and trim(acode)='" + txt_acode.Value.ToUpper().Trim() + "'", "existcd");



        if (chk_code.Length >= 6)
        {
            //chk_code = "update famstbal set ven_code='" + txt_vend_code.Value.Trim() + "'," + op_bal_fld + " = " + fgen.make_double() + " where branchcd='" + frm_mbr + "' and trim(acode)='" + txt_acode.Value.Trim() + "'";
            chk_code = "update famstbal set yr_2003='" + fgen.make_double(txt_balop_fx.Value) + "',ven_code='" + txt_vend_code.Value.Trim() + "'," + op_bal_fld + " = " + fgen.make_double(txt_balop.Value) + " where branchcd='" + frm_mbr + "' and trim(acode)='" + txt_acode.Value.Trim() + "'";
            // very important op. bal field missing
            fgen.execute_cmd(frm_qstr, frm_cocd, chk_code);
        }
        else
        {
            chk_code = "insert into famstbal(branchcd,acode,br_acode,ven_code,yr_2003," + op_bal_fld + ")values('" + frm_mbr + "','" + txt_acode.Value.Trim() + "','" + frm_mbr + txt_acode.Value.Trim() + "','" + txt_vend_code.Value.Trim() + "'," + fgen.make_double(txt_balop_fx.Value) + "," + fgen.make_double(txt_balop.Value) + ")";
            fgen.execute_cmd(frm_qstr, frm_cocd, chk_code);
        }


        if (txtAttch.Text.Length > 1)
        {
            oporow["fimglink"] = lblUpload.Text.Trim();
            //oporow["filename"] = txtAttch.Text.Trim();
        }


        if (edmode.Value == "Y")
        {
            oporow["eNt_by"] = ViewState["entby"].ToString();
            oporow["eNt_dt"] = fgen.make_def_Date(ViewState["entdt"].ToString(), vardate);
            oporow["edt_by"] = frm_uname;
            oporow["edt_dt"] = vardate;
            oporow["apprv_by"] = txt_appby.Value;
            oporow["apprv_dt"] = fgen.make_def_Date(txt_appdt.Value, vardate);
        }
        else
        {
            oporow["eNt_by"] = frm_uname;
            oporow["eNt_dt"] = vardate;
            oporow["edt_by"] = "-";
            oporow["eDt_dt"] = vardate;
            oporow["apprv_by"] = txt_appby.Value;
            oporow["apprv_dt"] = fgen.make_def_Date(txt_appdt.Value, vardate);
        }


        oporow["DEAC_BY"] = txt_deacby.Value;
        oporow["DEAC_DT"] = fgen.make_def_Date(txt_deacDt.Value, vardate);

        oporow["WEBLOGIN"] = txtWebLogin.Value;

        oporow["BCODE1"] = txtMarkup.Value;
        oporow["BCODE2"] = txtMinMarkup.Value;
        oporow["BCODE3"] = txtMaxMarkup.Value;
        oporow["PAYTERM"] = txtPayTerms.Value;
        oporow["CURRCODE"] = txtCurrency.Value;
        oporow["SERVTAXNO"] = txtTaxCode.Value;

        oporow["STATUS"] = tcsApplicable.Value;
        oporow["BANK_NAME"] = txt_tds_codes.Value;

        oporow["CCODE"] = (txtTpt.Value == "Y" ? "T" : "N");

        oDS.Tables[0].Rows.Add(oporow);
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
                //oporow5["par_fld"] = frm_mbr + lbl1a_Text + frm_vnum + txtvchdate.Value.Trim();
                oporow5["udf_name"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                oporow5["udf_value"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
                oporow5["SRNO"] = i;

                oDS5.Tables[0].Rows.Add(oporow5);
            }
        }
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
    protected void btnAtt_Click(object sender, EventArgs e)
    {
        string filepath = @"c:\TEJ_erp\UPLOAD\";      //Server.MapPath("~/tej-base/UPLOAD/");

        Attch.Visible = true;
        if (Attch.HasFile)
        {
            txtAttch.Text = Attch.FileName;
            filepath = filepath + frm_cocd + "_" + txt_acode.Value.Trim() + "~" + Attch.FileName;
            Attch.PostedFile.SaveAs(Server.MapPath("~/tej-base/UPLOAD/") + frm_cocd + "_" + txt_acode.Value.Trim() + "~" + Attch.FileName);
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

    protected void btnView1_Click(object sender, EventArgs e)
    {
        string filePath = lblUpload.Text;
        try
        {
            string newPath = Server.MapPath(@"~\tej-base\upload\");
            string filename = Path.GetFileName(filePath);
            newPath += filename;
            File.Copy(filePath, newPath, true);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filename + "','90%','90%','');", true);
        }
        catch { }
    }

    //protected void btnView1_Click(object sender, ImageClickEventArgs e)
    //{
    //    string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "c:/TEJ_erp/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Tejaxo Viewer');", true);
    //}
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


    protected void btn_acn_Click(object sender, ImageClickEventArgs e)
    {
        string uv_numac = "";
        uv_numac = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_ACODE");
        if (uv_numac == "Y")
        {
        }
        else
        {
            if (edmode.Value == "Y")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This Action Not Permitted in Edit Mode !!");
                return;
            }
        }
        hffield.Value = "ACNBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Account Nature", frm_qstr);
    }
    protected void btn_mgr_Click(object sender, ImageClickEventArgs e)
    {
        string uv_numac = "";
        uv_numac = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_ACODE");
        if (uv_numac == "Y")
        {
        }
        else
        {
            if (edmode.Value == "Y")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This Action Not Permitted in Edit Mode !!");
                return;
            }
        }

        hffield.Value = "MGRBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Ledger Group", frm_qstr);
    }
    protected void btn_sch_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SCHBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Schedule Code", frm_qstr);
    }

    protected void btn_br_wise_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BRWISEBUT";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select Branch Code(Only If Specific to Branch)", frm_qstr);

    }

    protected void btn_dist_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DISTBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select District", frm_qstr);
    }
    protected void btn_stat_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "STATBUT";
        txt_dist_name.Value = "-";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select State", frm_qstr);
    }
    protected void btn_zone_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ZONEBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Industry", frm_qstr);
    }
    protected void btn_segm_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SEGMBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Segment", frm_qstr);
    }

    protected void btnAtch_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Atch_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr);
    }

    protected void btn_ctry_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CTRYBUT";
        txt_dist_name.Value = "-";
        txt_stat_name.Value = "-";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Country", frm_qstr);
    }

    protected void btn_conti_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CONTBUT";
        txt_ctry_name.Value = "-";
        txt_dist_name.Value = "-";
        txt_stat_name.Value = "-";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Continent", frm_qstr);
    }

    protected void ImageButton16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "APP";
        fgen.msg("-", "CMSG", "Do You want to approve this account'13'");
    }
    protected void ImageButton18_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DEC";
        fgen.msg("-", "CMSG", "Do You want to deactivate this account'13'");
    }
    protected void ImageButton11_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PTERMS";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Pay Terms", frm_qstr);
    }
    protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PCUR";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Currency", frm_qstr);
    }
    protected void ImageButton12_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PTAX";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Tax Code", frm_qstr);
    }
    protected void btnShowCustReg_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "REG";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Account from Registration", frm_qstr);
    }
    protected void btnTDSAccount_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TDSAC";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select TDS Account Code", frm_qstr);
    }
    protected void btnSalesgrp_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "RSM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Sales Group", frm_qstr);
    }
    protected void btnPayTerms_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PAY_TERM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Payment Terms", frm_qstr);
    }
    protected void btnContTerms_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CONTR_TERM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Contract Terms", frm_qstr);
    }
}