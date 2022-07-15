using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class item_gen : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;
    string chk_used;


    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string html_body = "";
    string Prg_Id, lbl1a_Text, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_IndType;

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
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    frm_IndType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");
                    hfIndType.Value = frm_IndType;
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {


                string chk_curren = "";
                chk_curren = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT upper(trim(br_Curren)) as br_Curren FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "br_Curren");
                doc_addl.Value = chk_curren;

                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                if (Prg_Id == "F10116")
                {
                    lblheader.Text = "Item Master(FGS)";
                }
                else
                {
                    lblheader.Text = "Item Master(General)";
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
            if (CSR.Length > 1 || frm_ulvl == "3")
            {
            }
            if (lblUpload.Text.Length > 1)
            {
                btnView1.Visible = true;
                btnDwnld1.Visible = true;
            }
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

        if (dtCol == null) return;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        //tab5.Visible = false;
        //tab6.Visible = false;

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

        sg3_add_blankrows();
        sg4_add_blankrows();
        sg4.DataSource = sg4_dt; sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();

        ImageButton1.Enabled = false; ImageButton3.Enabled = false; ImageButton9.Enabled = false; ImageButton8.Enabled = false; ImageButton2.Enabled = false;
        ImageButton7.Enabled = false; ImageButton10.Enabled = false; ImageButton5.Enabled = false; ImageButton4.Enabled = false; ImageButton6.Enabled = false;
        ImageButton11.Enabled = false;
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
        ImageButton1.Enabled = true; ImageButton3.Enabled = true; ImageButton9.Enabled = true; ImageButton8.Enabled = true; ImageButton2.Enabled = true;
        ImageButton7.Enabled = true; ImageButton10.Enabled = true; ImageButton5.Enabled = true; ImageButton4.Enabled = true; ImageButton6.Enabled = true;
        ImageButton11.Enabled = true;

    }
    //------------------------------------------------------------------------------------
    public void clearctrl()
    {
        hffield.Value = "";
        edmode.Value = "";
        lblUpload.Text = "";
    }
    //------------------------------------------------------------------------------------
    public void set_Val()
    {
        doc_nf.Value = "icode";
        doc_df.Value = "icode";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "item";
        switch (Prg_Id)
        {
            case "F10111":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CS");
                typePopup = "N";
                break;
        }
        if (frm_IndType == "05" || frm_IndType == "06")
        {
            if (txt_mangrp.Value.Length > 2 && txt_subgrp.Value.Length > 2)
            {
                if (txt_mangrp.Value.Substring(0, 2) == "02" || txt_mangrp.Value.Substring(0, 2) == "07")
                {
                    txt_iname.Attributes.Add("readonly", "readonly");
                }
                else txt_iname.Attributes.Remove("readonly");
            }
            else txt_iname.Attributes.Remove("readonly");
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

            case "MGBUT":
                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                if (Prg_Id == "F10116")
                {
                    SQuery = "Select type1 as fstr,Name as Main_Grp_Name,Type1 as Code from type where id='Y' and substr(type1,1,1)>='7' order by type1";
                }
                else
                {
                    SQuery = "Select type1 as fstr,Name as Main_Grp_Name,Type1 as Code from type where id='Y' and substr(type1,1,1)<'7' order by type1";
                }
                break;
            case "SGBUT":
                SQuery = "Select icode as fstr,Iname as Sub_Grp_Name,Icode as Code,HSCODE from item where length(Trim(icode))=4 and trim(nvl(deac_by,'-'))='-' and substr(icode,1,2) like '" + txt_mangrp.Value.Substring(0, 2) + "%' order by icode";
                break;
            case "HSCBUT":
                if (doc_addl.Value == "INR")
                {
                    SQuery = "Select acref as fstr,Name as Main_HS_Name,acref as Code,num4 as CGST,num5 as SGST,num6 as IGST from typegrp where id='T1' order by acref";
                }
                else
                {
                    SQuery = "Select acref as fstr,Name as Main_HS_Name,acref as Code,num6 as VAT_RATE from typegrp where id='T1' order by acref";
                }

                break;
            case "BRWISEBUT":
                SQuery = "select * from (Select type1 as fstr,Name as Branch_Name,Type1 as Code from type where id='B' and upper(nvl(br_close,'-'))!='Y' union all Select '-' as fstr,'All' as Branch_Name,'99' as Code from Dual) order by code";
                break;
            case "UM1BUT":
                SQuery = "Select Name as fstr,Name as UOM,Type1 as Code from type where id='U' order by Name";
                break;
            case "UM2BUT":
                SQuery = "Select Name as fstr,Name as UOM,Type1 as Code from type where id='U' order by Name";
                break;
            case "CRITBUT":
                SQuery = "Select 'Y' as fstr,'Critical Item' Name ,'Y' as Code from dual union all Select 'N' as fstr,'Non-Critical Item' Name ,'N' as Code from dual ";
                break;
            case "ABCBUT":
                SQuery = "select Item_class as fstr , Item_class, 'Class' as Classification from (select 'A1' as Item_class from dual union all select 'A2' as Item_class from dual union all select 'B' as ITem_clas from dual union all select 'C' as ITem_clas from dual union all select 'D' as ITem_clas from dual union all select 'F' as ITem_clas from dual) ";
                break;
            case "CATGBUT":
                SQuery = "select Item_class as fstr , Item_class, 'Category' as Classification from (select 'DOM' as Item_class from dual union all select 'IMP' as Item_class from dual union all select 'N/A' as ITem_clas from dual ) ";
                break;
            case "LOCBUT":
                SQuery = "Select Name as fstr,Name as Location,type1 as Code from typegrp where branchcd='" + frm_mbr + "' and id='BN' order by acref";
                break;
            case "dim1_BUT":
                SQuery = "Select Name as fstr,Name as Location,type1 as Code from typegrp where branchcd!='DD' and id='#4' order by Type1";
                break;
            case "dim2_BUT":
                SQuery = "Select Name as fstr,Name as Location,type1 as Code from typegrp where branchcd!='DD' and id='#1' order by Type1";
                break;
            case "dim3_BUT":
                SQuery = "Select Name as fstr,Name as Location,type1 as Code from typegrp where branchcd!='DD' and id='#2' order by Type1";
                break;
            case "dim4_BUT":
                SQuery = "Select Name as fstr,Name as Location,type1 as Code from typegrp where branchcd!='DD' and id='#3' order by Type1";
                break;

            case "MILL":
                SQuery = "select type1 as fstr,Name as Mill,type1 as Code from typegrp where branchcd!='DD' and id='MI' order by name";
                break;
            case "MADEIN":
                SQuery = "select * from (Select type1 as fstr,Name as Manufacture_In,Type1 as Code from type where id='B' and upper(nvl(br_close,'-'))!='Y' union all Select '-' as fstr,'N/a' as Branch_Name,'99' as Code from Dual) order by code";
                //SQuery = "Select Type1 as fstr,Name as Manufacture_In,Type1 as Code from type where id='B' order by type1";
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
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
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD" || btnval == "Atch_E")
                {
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    if (Prg_Id == "F10116")
                    {
                        SQuery = "select A.icode as fstr,a.Iname as Item_Name,A.Cpartno as Part_no,a.Cdrgno as Drgw_no,a.icode as ERP_code,a.Maker" +
                            ",a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,A.mat4 as density,A.mat5 as micron,A.oprate3 as gsm,a.Edt_by" +
                            ",a.edt_Dt,a.showinbr,a.madeinbr from " + frm_tabname + " a where  a.branchcd='00' " +
                            "and length(Trim(A.icode))>4 and substr(a.icode,1,1) in ('7','8','9') order by A.Iname ";
                    }
                    else
                    {
                        SQuery = "select A.icode as fstr,a.Iname as Item_Name,A.Cpartno as Part_no,a.Cdrgno as Drgw_no,a.icode as ERP_code,a.Maker,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as " +
                            "entry_Dt,A.mat4 as density,A.mat5 as micron,A.oprate3 as gsm,a.Edt_by,a.edt_Dt,a.showinbr,a.madeinbr from " + frm_tabname + " a where  a.branchcd='00' " +
                            "and length(Trim(A.icode))>4 and substr(a.icode,1,1) not in ('7','8','9') order by A.Icode ";
                    }
                }
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
            newCase(frm_vty);
            fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
            hffield.Value = "NEW_E";

            //fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_DEAC", "-");
            //typePopup = "Y";
            //if (typePopup == "N")
            //    newCase(frm_vty);
            //else
            //{
            //    make_qry_4_popup();
            //    fgen.Fn_open_sseek("-", frm_qstr);
            //}

            //if (frm_ulvl == "3")
            //{
            //    //txtlbl4.Value = frm_uname;
            //    //txtlbl4.Disabled = true;
            //}
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
        string chk_indust = "";
        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;

        chk_indust = fgen.seek_iname(frm_qstr, frm_cocd, "select upper(Trim(opt_param)) as opt from fin_Rsys_opt where trim(opt_id)='W0000' ", "opt");

        double op1 = 0;
        double op2 = 0;
        double op3 = 0;
        string roff_val = "";
        int fldlen = 0;
        fldlen = txt_subgrp.Value.Trim().Length;
        if (txt_mangrp.Value.Trim().Length > 1)
        {
            if ((chk_indust == "05" || chk_indust == "06") && txt_mangrp.Value.ToUpper().Trim().Substring(0, 2) == "02")
            {

                op1 = fgen.make_double(toprate1.Value.ToUpper().Trim());
                op2 = fgen.make_double(toprate2.Value.ToUpper().Trim());
                op3 = fgen.make_double(toprate3.Value.ToUpper().Trim());

                if (op1 == 0 || op3 == 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "Please Fill Width/Length/GSM" + " Fields Require Input '13' Please Check in Fourth Tab (Specific Date) " + reqd_flds);
                    return;
                }

                if (frm_cocd != "KRSM") txt_iname.Value = txt_subgrp.Value.Trim().Substring(6, fldlen - 6) + " " + op1 + " X " + op2 + " X " + op3 + " Gsm ";
                txt_wt_grs.Value = (((op1 * op2 * op3) / 10000) / 1000).ToString();
                roff_val = fgen.seek_iname(frm_qstr, frm_cocd, "select round(" + txt_wt_grs.Value + ",6) as opt from dual", "opt");
                txt_wt_grs.Value = roff_val;
            }

            if ((chk_indust == "05" || chk_indust == "06") && txt_mangrp.Value.ToUpper().Trim().Substring(0, 2) == "07")
            {
                op1 = fgen.make_double(toprate1.Value.ToUpper().Trim());
                op2 = fgen.make_double(toprate2.Value.ToUpper().Trim());
                op3 = fgen.make_double(toprate3.Value.ToUpper().Trim());

                if (op1 == 0 || op3 == 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "Please Fill Width/GSM" + " Fields Require Input '13' Please fill " + reqd_flds);
                    return;
                }

                if (frm_cocd != "KRSM") txt_iname.Value = txt_subgrp.Value.Trim().Substring(6, fldlen - 6) + " " + op1 + " X " + op3 + " Gsm ";
            }
        }


        if (txt_mangrp.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Main Group";
        }

        if (txt_subgrp.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Sub Group";

        }
        if (txt_hscode.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "HS CODE";

        }

        if (txt_iname.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Item Name";

        }
        if (txt_pri_unit.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Primary Unit";

        }
        if (txt_brand.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Brand Name";

        }



        if (txt_madein.Value.Trim().Length < 2 && Prg_Id == "F10116")
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Made At Which Unit";

        }


        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }

        if (txt_mangrp.Value.Trim().Trim().ToString().Substring(0, 1) == "9")
        {
            if (fgen.make_double(txt_pack.Value.ToUpper().Trim()) == 0)
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , For Finished Goods, Standard Packing Fields Requires Input '13' Please fill 1, if not Available");
                return;

            }
        }
        string chk_exist = "";
        chk_exist = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(icode)||'-'||Iname as fstr from Item where trim(icode)!='" + txt_erp_code.Value + "' and upper(trim(iname))='" + txt_iname.Value.Trim().ToUpper() + "'", "fstr");
        if (chk_exist.ToString().Length > 5 && (frm_cocd != "MASS" && frm_cocd != "MAST"))
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " Such Item Name already Open , See '13' " + chk_exist + " '13' Please Re Check " + reqd_flds);
            return;
        }
        chk_exist = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(cpartno) as fstr from Item where trim(icode)!='" + txt_erp_code.Value + "' and upper(trim(cpartno))='" + txt_partno.Value.Trim().ToUpper() + "'", "fstr");
        if (chk_exist.ToString() != "0" && (frm_cocd == "MASS" || frm_cocd == "MAST"))
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " Such Part No. already Exist , See '13' " + chk_exist + " '13' Please Re Check " + reqd_flds);
            return;
        }
        if (edmode.Value == "Y")
        {
        }
        else
        {
            string chk_code;
            chk_code = fgen.seek_iname(frm_qstr, frm_cocd, "select max(icode) as existcd from item where branchcd!='DD' and substr(icode,1,4)='" + txt_subgrp.Value.Substring(0, 4) + "' and length(Trim(icode))>4  ", "existcd");
            if (chk_code == "0")
            {
                txt_erp_code.Value = txt_subgrp.Value.Substring(0, 4) + "0001";
            }
            else
            {
                chk_code = fgen.seek_iname(frm_qstr, frm_cocd, "select lpad(trim(max(icode)+1),8,'0') as existcd from item where branchcd!='DD' and substr(icode,1,4)='" + txt_subgrp.Value.Substring(0, 4) + "' and length(Trim(icode))>4  ", "existcd");
                txt_erp_code.Value = chk_code;
            }
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



        sg3_dt = new DataTable();
        sg4_dt = new DataTable();

        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();



        sg3_add_blankrows();


        sg4_add_blankrows();
        sg4.DataSource = sg4_dt;
        sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();


        ViewState["sg3"] = null;
        ViewState["sg4"] = null;
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "L1";
        fgen.msg("-", "CMSG", "Do You want to check List with Images'13'(No for without Image)");
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {

        hffield.Value = "PrtList";
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);


    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_Click(object sender, EventArgs e)
    {


        btnval = hffield.Value;
        //--
        string CP_deac;
        CP_deac = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_DEAC");
        if (CP_deac != "-" && CP_deac != "0")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                txt_deacby.Value = frm_uname;
                txt_deacDt.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            }
            else
            {
                txt_deacby.Value = "-";
                txt_deacDt.Value = "-";
            }

        }
        string CP_BTN;
        CP_BTN = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_FROM");
        string CP_HF1;
        CP_HF1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_HF1");
        hf1.Value = CP_HF1;
        if (CP_BTN.Trim().Length > 1)
        {
            if (CP_BTN.Trim().Substring(0, 3) == "BTN" || CP_BTN.Trim().Substring(0, 3) == "SG2" || CP_BTN.Trim().Substring(0, 3) == "SG3" || CP_BTN.Trim().Substring(0, 3) == "SG4")
            {
                btnval = CP_BTN;
            }
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_DEAC", "-");
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

                string mqry = "";
                mqry = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as fstr from ivoucher where substr(icode,1,8)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' ", "fstr");
                if (fgen.make_double(mqry.ToString()) > 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", " + mqry + " Transactions of This Item in Inventory , Deletion not Permitted !!");
                    return;
                }
                mqry = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as fstr from somas where substr(icode,1,8)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' ", "fstr");
                if (fgen.make_double(mqry.ToString()) > 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", " + mqry + " Transactions of This Item in Sales Orders , Deletion not Permitted !!");
                    return;
                }
                mqry = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as fstr from Pomas where substr(icode,1,8)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' ", "fstr");
                if (fgen.make_double(mqry.ToString()) > 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", " + mqry + " Transactions of This Item in Purchase Orders , Deletion not Permitted !!");
                    return;
                }

                mqry = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as fstr from Itemosp where substr(icode,1,8)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' ", "fstr");
                if (fgen.make_double(mqry.ToString()) > 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", " + mqry + " Transactions of This Item in BOM Table , Deletion not Permitted !!");
                    return;
                }
                mqry = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as fstr from Itemosp where substr(ibcode,1,8)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' ", "fstr");
                if (fgen.make_double(mqry.ToString()) > 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", " + mqry + " Transactions of This Item in BOM Table , Deletion not Permitted !!");
                    return;
                }

                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from itembal a where a.branchcd||trim(a.icode)='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"), vardate, frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
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
            ImageButton1.Focus();
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
                case "Atch_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                    string mcol7 = "";
                    string mcol1 = "";

                    mcol7 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");
                    mcol1 = col1 + mcol7 + "ITM";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", mcol1);
                    fgen.open_fileUploadPopup("Upload File for " + lblheader.Text, frm_qstr);
                    break;

                case "Edit_E":
                case "COPY_OLD":
                    //edit_Click
                    #region Edit Start
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_DEAC", "-");
                    if (col1 == "" || col1 == "-") return;
                    clearctrl();
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    string mv_col;
                    mv_col = col1;
                    SQuery = "Select a.* from " + frm_tabname + " a where a.icode='" + mv_col + "' ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txt_mangrp.Value = dt.Rows[i]["icode"].ToString().Trim().Substring(0, 2) + " :" + fgen.seek_iname(frm_qstr, frm_cocd, "select name as fstr from type where id='Y' and trim(type1)='" + dt.Rows[i]["icode"].ToString().Trim().Substring(0, 2) + "' ", "fstr");

                        txt_subgrp.Value = dt.Rows[i]["icode"].ToString().Trim().Substring(0, 4) + " :" + fgen.seek_iname(frm_qstr, frm_cocd, "select iname as fstr from item where length(Trim(icode))=4 and trim(icode)='" + dt.Rows[i]["icode"].ToString().Trim().Substring(0, 4) + "' ", "fstr");
                        if (btnval != "COPY_OLD")
                        {
                            txt_erp_code.Value = dt.Rows[i]["icode"].ToString().Trim();
                            txt_req_by.Value = dt.Rows[i]["req_by"].ToString().Trim();
                        }
                        txt_hscode.Value = dt.Rows[i]["hscode"].ToString().Trim();
                        txt_partno.Value = dt.Rows[i]["cpartno"].ToString().Trim();
                        txt_drgno.Value = dt.Rows[i]["cdrgno"].ToString().Trim();

                        txt_iname.Value = dt.Rows[i]["iname"].ToString().Trim();
                        txt_showin.Value = dt.Rows[i]["showinbr"].ToString().Trim();
                        txt_madein.Value = dt.Rows[i]["madeinbr"].ToString().Trim();

                        txt_req_by.Value = "-";

                        titem_dim1.Value = dt.Rows[i]["rep_dim1"].ToString().Trim();
                        titem_dim2.Value = dt.Rows[i]["rep_dim2"].ToString().Trim();
                        titem_dim3.Value = dt.Rows[i]["rep_dim3"].ToString().Trim();
                        titem_dim4.Value = dt.Rows[i]["rep_dim4"].ToString().Trim();

                        txt_ciname.Value = dt.Rows[i]["ciname"].ToString().Trim();
                        txt_pri_unit.Value = dt.Rows[i]["unit"].ToString().Trim();
                        txt_sec_unit.Value = dt.Rows[i]["no_proc"].ToString().Trim();

                        toprate1.Value = dt.Rows[i]["oprate1"].ToString().Trim();
                        toprate2.Value = dt.Rows[i]["oprate2"].ToString().Trim();
                        toprate3.Value = dt.Rows[i]["oprate3"].ToString().Trim();

                        t_BF.Value = dt.Rows[i]["bfactor"].ToString().Trim();
                        t_Mill.Value = dt.Rows[i]["pur_uom"].ToString().Trim();

                        t_oth1.Value = dt.Rows[i]["mat4"].ToString().Trim();
                        t_oth2.Value = dt.Rows[i]["mat5"].ToString().Trim();
                        t_oth3.Value = dt.Rows[i]["mat10"].ToString().Trim();

                        txt_deacby.Value = dt.Rows[i]["deac_by"].ToString().Trim();
                        txt_deacDt.Value = dt.Rows[i]["deac_dt"].ToString().Trim();

                        txt_appby.Value = dt.Rows[i]["app_by"].ToString().Trim();
                        txt_appdt.Value = dt.Rows[i]["app_dt"].ToString().Trim();

                        txt_crit_itm.Value = dt.Rows[i]["servicable"].ToString().Trim();
                        txt_irate.Value = dt.Rows[i]["irate"].ToString().Trim();
                        txt_pack.Value = dt.Rows[i]["packsize"].ToString().Trim();
                        txt_shelf.Value = dt.Rows[i]["default_us"].ToString().Trim();


                        txt_slow.Value = dt.Rows[i]["slow_mov"].ToString().Trim();
                        txt_leadt.Value = dt.Rows[i]["lead_time"].ToString().Trim();

                        txt_abc.Value = dt.Rows[i]["abc_class"].ToString().Trim();
                        txt_locn.Value = dt.Rows[i]["binno"].ToString().Trim();
                        txt_icat.Value = dt.Rows[i]["icat"].ToString().Trim();
                        txt_brand.Value = dt.Rows[i]["maker"].ToString().Trim();

                        txt_jw_ctrl.Value = dt.Rows[i]["jwq_ctrl"].ToString().Trim();
                        txt_stk_ctrl.Value = dt.Rows[i]["NON_STK"].ToString().Trim();
                        txt_wt_grs.Value = dt.Rows[i]["iweight"].ToString().Trim();
                        txt_wt_net.Value = dt.Rows[i]["wt_net"].ToString().Trim();
                        txt_iqd.Value = dt.Rows[i]["iqd"].ToString().Trim();
                        string op_bal_fld;
                        op_bal_fld = "YR_" + frm_CDT1.Substring(6, 4);
                        string ibal_data = "";
                        if (btnval != "COPY_OLD")
                        {
                            ibal_data = fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(" + op_bal_fld + ",0)||'#'||nvl(imin,0)||'#'||nvl(imax,0)||'#'||nvl(iord,0) as fstr from itembal where branchcd='" + frm_mbr + "' and trim(icode)='" + txt_erp_code.Value.Trim() + "' ", "fstr");
                            if (ibal_data.Contains("#"))
                            {
                                txt_balop.Value = ibal_data.Split('#')[0].ToString();
                                txt_min.Value = ibal_data.Split('#')[1].ToString();
                                txt_max.Value = ibal_data.Split('#')[2].ToString();
                                txt_rol.Value = ibal_data.Split('#')[3].ToString();
                            }
                        }

                        if (dt.Rows[i]["imagef"].ToString().Trim().Length > 1)
                        {
                            lblUpload.Text = dt.Rows[i]["imagef"].ToString().Trim();
                            btnView1.Visible = true;
                            //txtAttch.Text = dt.Rows[i]["filename"].ToString().Trim();
                        }

                        txt_iname.Focus();

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                        if (lblUpload.Text.Length > 1) btnDwnld1.Visible = true;
                    }
                    if (btnval == "COPY_OLD")
                    {
                        edmode.Value = "";
                    }
                    #endregion
                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;
                case "MGBUT":
                    if (col1.Length <= 0) return;
                    txt_mangrp.Value = col1 + " : " + col2;
                    txt_subgrp.Value = " ";
                    break;
                case "SGBUT":
                    if (col1.Length <= 0) return;
                    txt_subgrp.Value = col1.Trim() + " : " + col2;
                    txt_hscode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    break;
                case "HSCBUT":
                    if (col1.Length <= 0) return;
                    txt_hscode.Value = col1.Trim();
                    break;
                case "BRWISEBUT":
                    if (col1.Length <= 0) return;
                    txt_showin.Value = col1.Trim().Replace("'", "`");
                    if (col1.Trim() == "'-'")
                    {
                        txt_showin.Value = "-";
                    }
                    //txt_madein.Value = col1.Trim().Replace("'", "").Substring(0, 2);
                    break;

                case "UM1BUT":
                    if (col1.Length <= 0) return;
                    if (Prg_Id == "F10116")
                    {
                        chk_used = fgen.seek_iname(frm_qstr, frm_cocd, "select icode from ivoucher where trim(icode)='" + (txt_erp_code.Value).Trim() + "'", "icode");
                        if (chk_used.Length > 3)
                        {
                            fgen.msg("Alert", "AMSG", "Transaction for This Item Already Done, Unit Change Not Allowed.");
                        }
                        else txt_pri_unit.Value = col1.Trim();
                    }
                    else
                    {
                        txt_pri_unit.Value = col1.Trim();
                    }
                    break;
                case "UM2BUT":
                    if (col1.Length <= 0) return;
                    txt_sec_unit.Value = col1.Trim();
                    break;
                case "CRITBUT":
                    if (col1.Length <= 0) return;
                    txt_crit_itm.Value = col1.Trim();
                    break;
                case "ABCBUT":
                    txt_abc.Value = col1.Trim();
                    break;
                case "CATGBUT":
                    if (col1.Length <= 0) return;
                    txt_icat.Value = col1.Trim();
                    break;
                case "LOCBUT":
                    if (col1.Length <= 0) return;
                    txt_locn.Value = col1.Trim();
                    ImageButton4.Focus();
                    break;
                case "dim1_BUT":
                    if (col1.Length <= 0) return;
                    titem_dim1.Value = col1.Trim();
                    ImageButton13.Focus();
                    break;
                case "dim2_BUT":
                    if (col1.Length <= 0) return;
                    titem_dim2.Value = col1.Trim();
                    ImageButton14.Focus();
                    break;
                case "dim3_BUT":
                    if (col1.Length <= 0) return;
                    titem_dim3.Value = col1.Trim();
                    ImageButton15.Focus();
                    break;
                case "dim4_BUT":
                    if (col1.Length <= 0) return;
                    titem_dim4.Value = col1.Trim();
                    break;



                case "MILL":
                    if (col2.Length <= 0) return;
                    t_Mill.Value = col2.Trim();
                    t_oth1.Focus();
                    break;
                case "MADEIN":
                    if (col2.Length <= 0) return;
                    if (col1.Trim() == "-")
                    { txt_madein.Value = "-"; }
                    else
                    { txt_madein.Value = col1.Trim(); }
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

            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List" || hffield.Value == "List1" || hffield.Value == "PrtList")
        {
            string party_cd = "";
            string part_cd = "";
            party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
            part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
            if (party_cd.Trim().Length <= 1)
            {
                party_cd = "%";
            }
            if (part_cd.Trim().Length <= 1)
            {
                part_cd = "%";
            }

            SQuery = "insert into item(branchcd,icode,iname,ent_by,ent_Dt)(select distinct '00',substr(icode,1,4),'Sub Grp '||substr(icode,1,4),'-',to_DatE(sysdate,'dd/mm/yyyy') as sysd from item where substr(icode,1,4) not in (Select trim(icode) from item where length(Trim(icode))=4))";
            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

            SQuery = "insert into type(id,type1,name)(select distinct 'Y',substr(icode,1,2),'Main Grp '||substr(icode,1,2) from item where substr(icode,1,2) not in (Select trim(type1) from type where id='Y'))";
            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            if (Prg_Id == "F10116")
            {
                SQuery = "select a.Iname as Item_Name,A.Cpartno as Part_no,a.Cdrgno as Drgw_no,a.Ciname,a.HSCODE,a.unit,a.icode as ERP_code,substr(a.icode,1,4) as subgrp,a.Ent_by,a.ent_Dt,a.Edt_by,a.edt_Dt,replace(A.IMAGEF,'c:/tej_erp/','') AS IMG_SRC,a.showinbr,a.madeinbr,a.Maker as Make_or_Brand from " + frm_tabname + " a where  a.branchcd='00' and length(Trim(A.icode))>4 and a.icode like '9%' and a.icode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by substr(a.icode,1,4),A.Iname ";
            }
            else
            {
                SQuery = "select a.Iname as Item_Name,A.Cpartno as Part_no,a.Cdrgno as Drgw_no,a.Ciname,a.HSCODE,a.unit,a.icode as ERP_code,substr(a.icode,1,4) as subgrp,a.Ent_by,a.ent_Dt,a.Edt_by,a.edt_Dt,replace(A.IMAGEF,'c:/tej_erp/','') AS IMG_SRC,a.showinbr,a.madeinbr,a.Maker as Make_or_Brand from " + frm_tabname + " a where  a.branchcd='00' and length(Trim(A.icode))>4 and a.icode not like '9%' and a.icode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by substr(a.icode,1,4),A.Iname ";
            }


            switch (hffield.Value)
            {
                case "List1":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevelIMG("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);

                    break;
                case "List":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr,"");

                    break;

                case "PrtList":

                    if (frm_formID == "F10111")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F10111");
                        string header_n = "General Item Master List";
                        SQuery = "SELECT '" + header_n + "' as header, A.ICODE,A.INAME,A.UNIT AS ITEM_UNIT,A.HSCODE as TARRIFNO,A.MRP,A.CPARTNO,a.IRATE,A.IWEIGHT,TRIM(A.CINAME) AS NAME_PART,B.ICODE AS SUBG,B.INAME AS SNAME,d.IMAX,d.IMIN,C.TYPE1 AS MGRP,C.NAME AS MGNAME FROM ITEM A,ITEM B,TYPE C,itembal d WHERE trim(a.icode)=trim(d.icode) and SUBSTR(TRIM(A.ICODE),1,2)=TRIM(C.TYPE1)  AND SUBSTR(TRIM(A.ICODE),1,4)=TRIM(B.ICODE) AND LENGTH(TRIM(A.ICODE))=8 AND TRIM(C.ID)='Y' and substr(trim(a.icode),1,1) !='9' and a.icode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ORDER BY C.TYPE1,b.icode,A.ICODE";
                    }
                    else if (frm_formID == "F10116")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F10116");
                        string header_n = "FG Item Master List";
                        SQuery = "SELECT '" + header_n + "' as header, A.ICODE,A.INAME,A.UNIT AS ITEM_UNIT,A.HSCODE AS TARRIFNO,A.MRP,A.CPARTNO,a.IRATE,A.IWEIGHT,TRIM(A.CINAME) AS NAME_PART,B.ICODE AS SUBG,B.INAME AS SNAME,d.IMAX,d.IMIN,C.TYPE1 AS MGRP,C.NAME AS MGNAME FROM ITEM A,ITEM B,TYPE C,itembal d WHERE trim(a.icode)=trim(d.icode) and SUBSTR(TRIM(A.ICODE),1,2)=TRIM(C.TYPE1)  AND SUBSTR(TRIM(A.ICODE),1,4)=TRIM(B.ICODE) AND LENGTH(TRIM(A.ICODE))=8 AND TRIM(C.ID)='Y' and substr(trim(a.icode),1,1) ='9' and a.icode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'  ORDER BY C.TYPE1,b.icode,A.ICODE";
                    }
                    fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "ITEM_MASTER_main", "ITEM_MASTER_main");
                    break;
            }



            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------


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
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set icode='ZZ'||trim(replace(icode,'ZZ','')) where trim(" + doc_nf.Value + ")='" + ddl_fld1 + "'");
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
                                fgen.msg("-", "AMSG", "Item No " + txt_erp_code.Value + "'13' Saved , You can Approve the Same Using Approval options.");
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
                        "<td><b>Item Code</b></td><td><b>Item Name</b></td><td><b>User Name</b></td><td><b>Activity Date</b></td><td><b>ID</b></td>");


                        sb.Append("<tr>");
                        sb.Append("<td>");
                        sb.Append(txt_iname.Value.Trim());
                        sb.Append("</td>");
                        sb.Append("<td>");
                        sb.Append(txt_erp_code.Value.Trim());
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
                        fgen.send_Activity_mail(frm_qstr, frm_cocd, "Tejaxo ERP", frm_formID, subj + lblheader.Text + " #" + txt_erp_code.Value.Trim(), sb.ToString(), frm_uname);

                        //fgen.send_Activity_msg(frm_qstr, frm_cocd, frm_formID, subj + lblheader.Text + " #" + frm_vnum + " by " + frm_uname, frm_uname);

                        sb.Clear();
                        #endregion
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txt_erp_code.Value.Trim(), frm_uname, edmode.Value);

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

        oporow["ICODE"] = txt_erp_code.Value.ToUpper().Trim();
        oporow["hscode"] = txt_hscode.Value.ToUpper().Trim();
        oporow["cpartno"] = txt_partno.Value.ToUpper().Trim();
        oporow["cdrgno"] = txt_drgno.Value.ToUpper().Trim();

        oporow["iname"] = txt_iname.Value.ToUpper().Trim();
        oporow["showinbr"] = txt_showin.Value.ToUpper().Trim();
        oporow["madeinbr"] = txt_madein.Value.ToUpper().Trim();


        oporow["req_by"] = txt_req_by.Value.ToUpper().Trim();

        oporow["rep_dim1"] = titem_dim1.Value.ToUpper().Trim();
        oporow["rep_dim2"] = titem_dim2.Value.ToUpper().Trim();
        oporow["rep_dim3"] = titem_dim3.Value.ToUpper().Trim();
        oporow["rep_dim4"] = titem_dim4.Value.ToUpper().Trim();


        oporow["ciname"] = txt_ciname.Value.ToUpper().Trim();
        if (txt_ciname.Value.Length <= 2)
        {
            oporow["ciname"] = txt_iname.Value.ToUpper().Trim();
            //oporow["filename"] = txtAttch.Text.Trim();
        }


        oporow["unit"] = txt_pri_unit.Value.ToUpper().Trim();
        oporow["no_proc"] = txt_sec_unit.Value.ToUpper().Trim();

        oporow["servicable"] = txt_crit_itm.Value.ToUpper().Trim();
        oporow["irate"] = fgen.make_double(txt_irate.Value.ToUpper().Trim());
        oporow["packsize"] = fgen.make_double(txt_pack.Value.ToUpper().Trim());
        oporow["default_us"] = fgen.make_double(txt_shelf.Value.ToUpper().Trim());


        oporow["slow_mov"] = fgen.make_double(txt_slow.Value.ToUpper().Trim());
        oporow["lead_time"] = fgen.make_double(txt_leadt.Value.ToUpper().Trim());

        oporow["oprate1"] = fgen.make_double(toprate1.Value.ToUpper().Trim());
        oporow["oprate2"] = fgen.make_double(toprate2.Value.ToUpper().Trim());
        oporow["oprate3"] = fgen.make_double(toprate3.Value.ToUpper().Trim());


        oporow["bfactor"] = t_BF.Value.ToUpper().Trim();
        oporow["pur_uom"] = t_Mill.Value.ToUpper().Trim();

        oporow["mat4"] = t_oth1.Value.ToUpper().Trim();
        oporow["mat5"] = t_oth2.Value.ToUpper().Trim();
        oporow["mat10"] = t_oth3.Value.ToUpper().Trim();


        oporow["abc_class"] = txt_abc.Value.ToUpper().Trim();
        oporow["binno"] = txt_locn.Value.ToUpper().Trim();
        oporow["icat"] = txt_icat.Value.ToUpper().Trim();
        oporow["maker"] = txt_brand.Value.ToUpper().Trim();

        oporow["deac_by"] = txt_deacby.Value.ToUpper().Trim();
        oporow["deac_dt"] = txt_deacDt.Value.ToUpper().Trim();

        oporow["app_by"] = txt_appby.Value.ToUpper().Trim();

        oporow["jwq_ctrl"] = txt_jw_ctrl.Value.ToUpper().Trim();
        oporow["non_Stk"] = txt_stk_ctrl.Value.ToUpper().Trim();
        oporow["iweight"] = fgen.make_double(txt_wt_grs.Value.ToUpper().Trim());
        oporow["wt_net"] = fgen.make_double(txt_wt_net.Value.ToUpper().Trim());
        oporow["iqd"] = fgen.make_double(txt_iqd.Value.ToUpper().Trim());

        string op_bal_fld;
        op_bal_fld = "YR_" + frm_CDT1.Substring(6, 4);
        string chk_code;
        chk_code = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(icode) as existcd from itembal where branchcd='" + frm_mbr + "' and substr(icode,1,8)='" + txt_erp_code.Value.ToUpper().Trim() + "'", "existcd");
        if (chk_code.Length >= 8)
        {
            chk_code = "update itembal set binno='" + txt_locn.Value.Trim() + "'," + op_bal_fld + " = " + fgen.make_double(txt_balop.Value) + ",imin=" + fgen.make_double(txt_min.Value) + ",imax=" + fgen.make_double(txt_max.Value) + ",iord=" + fgen.make_double(txt_rol.Value) + ",birate=" + fgen.make_double(txt_irate.Value) + " where branchcd='" + frm_mbr + "' and trim(icode)='" + txt_erp_code.Value.Trim() + "'";
            fgen.execute_cmd(frm_qstr, frm_cocd, chk_code);
        }
        else
        {
            chk_code = "insert into itembal(br_icode,branchcd,icode,binno," + op_bal_fld + ",imin,imax,iord,birate)values('" + frm_mbr + txt_erp_code.Value.Trim() + "','" + frm_mbr + "','" + txt_erp_code.Value.Trim() + "','" + txt_locn.Value.Trim() + "'," + fgen.make_double(txt_balop.Value) + "," + fgen.make_double(txt_min.Value) + "," + fgen.make_double(txt_max.Value) + "," + fgen.make_double(txt_rol.Value) + "," + fgen.make_double(txt_irate.Value) + ")";
            fgen.execute_cmd(frm_qstr, frm_cocd, chk_code);
        }


        if (txtAttch.Text.Length > 1)
        {
            oporow["imagef"] = lblUpload.Text.Trim();
            //oporow["filename"] = txtAttch.Text.Trim();
        }


        if (edmode.Value == "Y")
        {
            oporow["eNt_by"] = ViewState["entby"].ToString();
            oporow["eNt_dt"] = fgen.make_def_Date(ViewState["entdt"].ToString(), vardate);
            oporow["edt_by"] = frm_uname;
            oporow["edt_dt"] = vardate;
            oporow["app_by"] = "-";
            oporow["app_dt"] = Convert.ToDateTime(vardate.ToString().Trim()).ToString("dd/MM/yyyy");
        }
        else
        {
            oporow["eNt_by"] = frm_uname;
            oporow["eNt_dt"] = vardate;
            oporow["edt_by"] = "-";
            oporow["eDt_dt"] = vardate;

            oporow["app_dt"] = txt_appdt.Value;
        }
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
        string filepath = @"c:\TEJ_ERP\UPLOAD\";      //Server.MapPath("~/tej-base/UPLOAD/");
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            txtAttch.Text = Attch.FileName;
            filepath += Attch.FileName;
            //filepath = filepath + txtlbl4.Value.Trim() + "_" + txtvchnum.Value.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
            Attch.PostedFile.SaveAs(Server.MapPath("~/tej-base/UPLOAD/") + Attch.FileName);
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
    protected void btn_mg_Click(object sender, ImageClickEventArgs e)
    {
        if (edmode.Value == "Y")
        {
            fgen.msg("-", "AMSG", "Code Change not Allowed in Edit mode !!");
            return;
        }
        else
        {
            hffield.Value = "MGBUT";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Main Group", frm_qstr);
        }
    }
    protected void btn_sg_Click(object sender, ImageClickEventArgs e)
    {
        if (edmode.Value == "Y")
        {
            fgen.msg("-", "AMSG", "Code Change not Allowed in Edit mode !!");
            return;
        }
        else
        {
            hffield.Value = "SGBUT";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Sub Group", frm_qstr);
        }
    }
    protected void btn_hsc_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "HSCBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select HS Code", frm_qstr);
    }
    protected void btn_br_wise_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BRWISEBUT";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select Branch Code(Only If Specific to Branch)", frm_qstr); 
    } 
    protected void btn_puom_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "UM1BUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Primary Unit", frm_qstr);
    }
    protected void btn_suom_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "UM2BUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Seondary Unit", frm_qstr);
    }
    protected void btn_crit_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CRITBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void btn_deac_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DEAC_BUT";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_DEAC", hffield.Value);
        fgen.msg("-", "CMSG", "Are You Sure!! You Want to Deactivate This Item");
    }

    protected void btn_abc_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ABCBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select ABC Class Group", frm_qstr);
    }

    protected void btn_catg_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CATGBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Category", frm_qstr);
    }

    protected void btn_locn_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "LOCBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Location", frm_qstr);
    }

    protected void btn_dim1_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "dim1_BUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Dimension 1", frm_qstr);
    }
    protected void btn_dim2_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "dim2_BUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Dimension 2", frm_qstr);
    }
    protected void btn_dim3_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "dim3_BUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Dimension 3", frm_qstr);
    }
    protected void btn_dim4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "dim4_BUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Dimension 4", frm_qstr);
    }
    protected void btn_mill_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MILL";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Mill", frm_qstr);
    }

    protected void btn_madein_click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MADEIN";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Our Unit in Which This item is Made", frm_qstr);
    }
    protected void btnAtch_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Atch_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr);
    }

}