using System;
using System.Collections.Generic;
using System.Collections;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Text;
using System.IO;
using System.Net;

public partial class om_einv : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, col4, col5, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt2, dt3, dt4, dt5; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string doc_is_ok = "";
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query, make_ewayb = "N", catcode = "", firm = "";
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, demo_einv = "N", web_einv_ok = "N";
    string flag = "";
    string used_opt = "";
    string coaddr3 = "", tfrom_pin = "", cotel = "", coemail = "";
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
                    //frm_mbr = "01";
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
                doc_addl.Value = "1";

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
                hfmake_EwayBill.Value = fgen.getOptionPW(frm_qstr, frm_cocd, "W1080", "OPT_enable", frm_mbr);
                if (frm_cocd == "GGRP" || frm_cocd == "MPAC" || frm_cocd == "PGTL" || frm_cocd == "ROYL")
                { hf_einv_ok.Value = "Y"; }
                else
                { hf_einv_ok.Value = "N"; }
                hfdemo.Value = fgen.getOptionPW(frm_qstr, frm_cocd, "W1082", "OPT_enable", frm_mbr);
            }
            setColHeadings();
            set_Val();
            typePopup = "N";
            make_ewayb = hfmake_EwayBill.Value;
            web_einv_ok = hf_einv_ok.Value;
            demo_einv = hfdemo.Value;
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

        //txtlbl8.Attributes.Add("readonly", "readonly");
        //txtlbl9.Attributes.Add("readonly", "readonly");



        // to hide and show to tab panel
        tab5.Visible = false;
        tab4.Visible = false;
        tab3.Visible = false;
        tab2.Visible = true;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnshow.Disabled = true; btnjson.Disabled = true; command6.Disabled = true;
        create_tab();
        create_tab2();
        create_tab3();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();

        //btnlbl4.Enabled = false;
        //btnlbl7.Enabled = false;




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
        btnshow.Disabled = false; btnjson.Disabled = false; command6.Disabled = false;
        //btnlbl4.Enabled = true;
        // btnlbl7.Enabled = true;
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
        frm_tabname = "einv_rec";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "IR");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        if (hfdemo.Value == "Y")
        { command6.Visible = true; }
        else
        { command6.Visible = false; }
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

            case "TACODE":
                SQuery = "SELECT type1 as fstr,name as grade_name,Type1 as Grade_Code  from type where id='I' and type1 like '0%'";
                break;
            case "MRESULT":
                SQuery = "SELECT '01' as fstr,'ACCEPTED' as Results,'01' as Qa_Code from dual union all SELECT '02' as fstr,'REJECTED' as Results,'02' as Qa_Code from dual union all SELECT '03' as fstr,'ACCEPT U/Dev.' as Results,'03' as Qa_Code from dual union all SELECT '04' as fstr,'ACCEPT U/Seg.' as Results,'04' as Qa_Code from dual";
                break;

            case "DOCTYPE":

                SQuery = "SELECT 'invoice' AS FSTR,'invoice' as type,'invoice' as Name FROM dual union all  SELECT 'DrCr' AS FSTR,'Dr/Cr Note' as type,'Dr/Cr Note' as Name FROM dual";
                break;

            case "sg1_t11":
                SQuery = "select * from (select Acode,ANAME as Transporter,Acode as Code,gst_no as transp_id,Addr1 as Address,Addr2 as City from famst  where upper(ccode)='T' union all select 'Own' as Acode,'OWN' as Transporter,'-' as Code,'-' as transp_id,'-' as Address,'-' as City from dual union all select 'party' as acode,'PARTY VEHICLE' as Transporter,'-' as Code,'-' as trans_id,'-' as Address,'-' as City from dual) order by  Transporter";
                break;
            case "TICODE":
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":

                SQuery = "select name as fstr, name as District ,type1 as code,acref from typegrp where id='DT' order by name";
                break;


            case "SG1_ROW_ADD1":
            case "SG1_ROW_ADD1_E":

                SQuery = "select * from (select ANAME AS FSTR,ANAME as Transporter,Acode as Code,exc_regn  as TPT_ID,gst_no from famst  where (trim(nvl(GRP,'-')) in ('05','06') or  upper(ccode)='T' or acode='" + fgen.seek_iname(frm_qstr, frm_cocd, "select acode from type where id='B' and type1='" + frm_mbr + "'", "acode") + "' or acode='" + sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text + "')  ) order by  Transporter";
                //SQuery = "SELECT userid AS FSTR,Full_Name AS Client_Name,username as CCode FROM evas where branchcd!='DD' and username!='-' and userid>'000052' and trim(userid) not in (select trim(Ccode) from wb_oms_log where branchcd!='DD' and to_char(opldt,'yyyymm')=to_char(to_DaTE('" + txtvchdate.Text  + "','dd/mm/yyyy'),'yyyymm')) order by Username";
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
                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(Trim(icode))>4 and icode like '9%' and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname  ";
                break;


            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "select fstr, doc_dtl,vch_date,entry_dt, sum(cnt) as No_records,dtsort from (select trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') as fstr,vchnum||'  '||trim(ent_by) as  Doc_dtl,to_char(vchdate,'dd/mm/yyyy') as Vch_Date,to_chaR(ent_dt,'dd/mm/yyyy') as entry_Dt,1 as cnt,to_char(vchdate,'yyyymmdd') As dtsort from einv_rec where VCHDATE " + DateRange + " AND type='" + frm_vty + "' and branchcd='" + frm_mbr + "'  order by vchdate desc ,vchnum desc) group by fstr, doc_dtl,vch_date,entry_dt,dtsort order by dtsort  desc,doc_dtl desc ";
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
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "FINSYS");
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

            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

            frm_vty = "IR";
            lbl1a.Text = frm_vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);

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
    void newCase(string vty)
    {
        #region
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl4.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no,place,zipcode from type where id='B' and type1='" + frm_mbr + "'", "place");
        txtlbl7.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no,place,zipcode from type where id='B' and type1='" + frm_mbr + "'", "zipcode");

        disablectrl();
        fgen.EnableForm(this.Controls);
        //btnlbl4.Focus();

        sg1_dt = new DataTable();
        create_tab();
        //int j;
        //for (j = i; j < 10; j++)
        //{
        //    sg1_add_blankrows();
        //}

        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        // Popup asking for Copy from Older Data
        //fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        //hffield.Value = "NEW_E";        

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

        for (i = 0; i < sg1.Rows.Count; i++)
        {
            string chk_h_n = fgen.seek_iname(frm_qstr, frm_cocd, "Select doc_type||'-'||trim(Doc_no)||' Upd No. '||vchnum||' '||to_Char(vchdate,'dd/mm/yyyy')||' '||ent_by as fstr from einv_rec where branchcd='" + frm_mbr + "' and type like 'IR%' and vchnum||to_Char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + Convert.ToDateTime(txtvchdate.Text.Trim()).ToShortDateString() + "' and trim(upper(irn_no))='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text + "' and trim(nvl(irn_no,'-')) <> '-'", "fstr");
            if (chk_h_n.Length > 2)
            {
                fgen.msg("-", "AMSG", "Such IRN already Entered See doc No. " + chk_h_n + " See line " + (i + 1) + "");
                return;
            }
        }
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        if (sg1.Rows[0].Cells[15].Text.Length < 2)
        {
            fgen.msg("", "ASMG", "There is no entry to be found to save!!!!!!");
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
        Response.Redirect("~/fin-base/desktop.aspx?STR=" + frm_qstr);
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
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        fgen.Fn_open_prddmp1("Select Date for Print", frm_qstr);
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").Substring(0, 6), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + "Delete");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").Substring(0, 6) + "");
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
        else if (hffield.Value == "demo")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                used_opt = "-";
                gen_eway_bill("WEBT");
            }
        }
        else if (hffield.Value == "LIST_IRN")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                SQuery = "select a.vchnum as Invoice_No,to_char(a.vchdate,'dd/mm/yyyy') as Invoice_Dt,b.aname as Customer,trim(a.einv_no) as IRN,trim(nvl(a.st_entform,'-')) as eway_bill,a.bill_tot as Bill_Total from sale a , famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " AND a.type like '4%'  order by a.vchdate desc,a.vchnum desc";
            }
            else
            {
                SQuery = "select a.* ,b.aname as Customer from (select a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as acode,trim(nvl(a.gstvchnum,'-')) as IRN,sum(a.spexc_rate) as Total from ivoucher a where a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " AND a.type in ('58','59') group by a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.acode) ,trim(nvl(a.gstvchnum,'-')))a, famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE) order by a.vchdate desc,a.vchnum desc";
            }
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Document list with IRN ", frm_qstr, "");
            hffield.Value = "-";
        }

        else if (hffield.Value == "Print")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                SQuery = "select distinct trim(a.einv_no) as fstr,trim(a.vchnum)as Inv_no,to_char(a.vchdate,'dd/mm/yyyy') as Inv_Date,a.type,trim(a.acode) as party_code,trim(a.einv_no) as IRN,trim(b.aname) as Party_name,to_char(a.vchdate,'yyyymmdd') as vdd from sale a, famst b where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + PrdRange + " and trim(nvl(a.einv_no,'-')) <> '-'  and trim(a.acode)=trim(B.acode) order by to_char(a.vchdate,'yyyymmdd'),trim(a.vchnum) desc";
            }
            else
            {
                SQuery = "select distinct trim(a.gstvchnum) as fstr, trim(a.vchnum) as Note_no,to_char(a.vchdate,'dd/mm/yyyy') as Note_dt,a.type,trim(a.acode) as party_code,trim(a.gstvchnum) as IRN,trim(b.aname) as Party_name,to_char(a.vchdate,'yyyymmdd') as vdd from ivoucher a, famst b where a.branchcd='" + frm_mbr + "' and a.type in ('58','59') and a.vchdate " + PrdRange + " and trim(a.acode)=trim(b.acode) and trim(nvl(a.gstvchnum,'-')) != '-' order by to_char(a.vchdate,'yyyymmdd') DESC,trim(a.vchnum) desc";
            }
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "FINSYS");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("Select Document", frm_qstr);
            hffield.Value = "Print_F";

        }
        else if (hffield.Value == "Print_F")
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            string g_uid, g_pwd, g_zip, g_efuuid, g_efupwd, g_efukey, g_api_link, res, v_gstin, cc_string, MY_INS_CERT;
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, "select gstewb_id,gstewb_pw,zipcode,gstefu_id,gstefu_pw,gstefu_cdkey,irn_apiadd,upper(trim(gst_no)) as aa from type where id='B' and type1='" + frm_mbr + "'");

            g_uid = dt.Rows[0]["gstewb_id"].ToString();
            g_pwd = dt.Rows[0]["gstewb_pw"].ToString();
            g_zip = dt.Rows[0]["zipcode"].ToString();

            g_efuuid = dt.Rows[0]["gstefu_id"].ToString();
            g_efupwd = dt.Rows[0]["gstefu_pw"].ToString();
            g_efukey = dt.Rows[0]["gstefu_cdkey"].ToString();
            g_api_link = dt.Rows[0]["irn_apiadd"].ToString().Trim();
            v_gstin = dt.Rows[0]["aa"].ToString();

            if (g_api_link.Length < 10)
            {
                fgen.msg("-", "AMSG", "Portal API not linked in Plant Master , Please contact Administrator");
                return;
            }
            g_api_link = g_api_link.ToUpper().Replace("/GENIRN", "/PrintEInvByIRN");

            cc_string = "{'IRN': '" + col1 + "','GSTIN': '" + v_gstin + "','CDKey': '" + g_efukey + "','EInvUserName': '" + g_uid + "','EInvPassword': '" + g_pwd + "','EFUserName': '" + g_efuuid + "','EFPassword': '" + g_efupwd + "'}";

            cc_string = cc_string.Replace("^", " ");
            cc_string = cc_string.Replace("'", "\"");

            res = MakeWebRequest("POST", g_api_link, cc_string);
            string AA, BB;
            AA = res.IndexOf("File").ToString();
            BB = res.IndexOf("pdf").ToString();
            if (AA == "0" || BB == "0")
            {
                fgen.msg("-", "AMSG", "" + res + "");
                return;
            }
            else
            {
                MY_INS_CERT = "";
                var dicddd = new Dictionary<string, object>();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                ArrayList itemss = jss.Deserialize<ArrayList>(res);
                foreach (var value in itemss)
                {
                    dicddd = ((Dictionary<string, object>)itemss[0]);
                    foreach (var d in dicddd)
                    {
                        if (d.Key.ToString().ToUpper() == "ERRORMESSAGE")
                        {
                            if (d.Value.ToString().Length > 4)
                            {
                                fgen.msg("-", "AMSG", "" + res + "");
                                return;
                            }
                        }
                        if (d.Key.ToString().ToUpper() == "FILE")
                        {
                            MY_INS_CERT = d.Value.ToString();
                            break;
                        }
                    }

                }
                //sMY_INS_CERT = (res.Substring(Convert.ToInt16(AA + 7) ,150)).Substring(1,-3);
                // Response.Redirect("http://115.124.126.137/einvasp_prod/PrintHTMLTemplates/" + col1 + ".pdf");
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string fil_loc = (g_api_link + "/" + col1 + ".pdf");
                // string fil_loc = ("http://115.124.126.137/einvasp_prod/PrintHTMLTemplates/" + col1 + ".pdf");
                Session["mymst"] = "Y";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "','98%','98%','" + "" + "');", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "OpenWindow", "window.open('" + MY_INS_CERT + "');", true);
            }

            hffield.Value = "-";
        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
            col5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

            switch (btnval)
            {
                case "New":
                    newCase(col1);
                    break;
                case "COPY_OLD":
                    #region Copy from Old Temp
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,b.text from " + frm_tabname + " a left outer join fin_msys b on trim(a.frm_name)=trim(b.id) where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {

                        txtlbl4.Text = dt.Rows[i]["frm_name"].ToString().Trim();
                        // txtlbl4a.Text = dt.Rows[i]["text"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[i]["frm_header"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["ent_id"].ToString().Trim();
                        // txtlbl7a.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");



                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["text"].ToString().Trim();
                            sg1_dr["sg1_h3"] = dt.Rows[i]["frm_name"].ToString().Trim();
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
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();

                    SQuery = "SELECT  a.*,b.aname,b.gst_no from " + frm_tabname + " a , famst b  where  trim(a.acode)=trim(b.acode) and  a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY a.SRNO";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl4.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no,place,zipcode from type where id='B' and type1='" + frm_mbr + "'", "place");
                        txtlbl7.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no,place,zipcode from type where id='B' and type1='" + frm_mbr + "'", "zipcode");

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

                            sg1_dr["sg1_f1"] = dt.Rows[i]["d_dfrom"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["d_Cscode"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["acode"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["doc_type"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["Doc_No"].ToString().Trim();
                            sg1_dr["sg1_t1"] = Convert.ToDateTime(dt.Rows[i]["doc_Dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_t2"] = dt.Rows[i]["aname"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["gst_no"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["to_state"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["doc_value"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["VEHI_NO"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["APPX_DIST"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["irn_no"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["irnqr_1"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["irnqr_2"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["ack_no"].ToString().Trim();
                            sg1_dr["sg1_t17"] = dt.Rows[i]["ack_dt"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["gto_place"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["gto_pin"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["gtpt_name"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["gtpt_id"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["gtpt_Code"].ToString().Trim();
                            sg1_dr["sg1_t18"] = dt.Rows[i]["irn_stat"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }


                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
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
                case "DOCTYPE":
                    PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                    if (col1.Length <= 0) return;
                    if (col1 == "invoice")
                    {

                        SQuery = "select trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy')as fstr,b.Aname as Party_Name,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy') as link_doc,b.Staten,a.vchdate as Doc_Dt,a.vchnum as Doc_No,max(a.bill_tot) as Inv_amt from (Select type,vchnum,vchdate,acode,1 as docx,bill_tot from sale where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + PrdRange + "  union all Select doc_type,trim(doc_no) as doc_no,doc_Dt,acode,-1 as docx,0 as amt from einv_rec where branchcd='" + frm_mbr + "' and type like 'IR%' and doc_dt " + PrdRange + " and doc_type like '4%' )a,famst b where trim(A.acode)=trim(B.acode) group by b.Aname,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy'),b.Staten,a.vchdate,a.vchnum having sum(docx)>0  order by a.vchdate DESC,a.vchnum desc";
                        // ------------for testing purpose in KLAS----------------------
                        //SQuery = "select trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy')as fstr,b.Aname as Party_Name,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy') as link_doc,b.Staten,a.vchdate as Doc_Dt,a.vchnum as Doc_No,max(a.bill_tot) as Inv_amt from (Select type,vchnum,vchdate,acode,1 as docx,bill_tot from sale where branchcd='00' and type like '4%' and vchdate  between to_date('01/04/2020','dd/mm/yyyy') and to_date('17/02/2021','dd/mm/yyyy') and vchnum='004573' and branchcd='00' and type='40' and vchdate=to_date('17/02/2021','dd/mm/yyyy')   )a,famst b where trim(A.acode)=trim(B.acode) group by b.Aname,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy'),b.Staten,a.vchdate,a.vchnum having sum(docx)>0  order by a.vchdate DESC,a.vchnum desc";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek("Select Entry ", frm_qstr);
                        hffield.Value = "INV";
                    }
                    else
                    {
                        SQuery = "select trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy')as fstr,b.Aname as Party_Name,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy') as link_doc,b.Staten,a.vchdate as Doc_Dt,a.vchnum as Doc_No,sum(docx) as aa from (Select type,vchnum,vchdate,acode,1 as docx,sum(spexc_amt) as bill_tot from ivoucher where branchcd='" + frm_mbr + "' and type in ('58','59') and vchdate " + PrdRange + " group by type,vchnum,vchdate,acode union all Select doc_type,trim(doc_no) as doc_no,doc_Dt,acode,-1 as docx,0 as amt from einv_rec where branchcd='" + frm_mbr + "' and type like 'IR%' and doc_dt " + PrdRange + " and doc_type not like '4%')a,famst b where trim(A.acode)=trim(B.acode) group by b.Aname,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy'),b.Staten,a.vchdate,a.vchnum  having sum(docx)>0  order by a.vchdate DESC,a.vchnum desc";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek("Select Entry ", frm_qstr);
                        hffield.Value = "DRCR";
                    }
                    break;
                case "DRCR":
                case "INV":
                    if (col1.Length <= 0) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    flag = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL12");
                    if (btnval == "DRCR")
                    {

                        SQuery = "select distinct a.tpt_names as tpt_name,'-' as tptcode,nvl(b.district,'-') as district,nvl(b.pincode,'-') as pincode,b.aname,replace(replace(replace(a.binno,'/',''),'-',''),' ','') as vehi_no2,replace(replace(replace(a.mode_tpt,'/',''),'-',''),' ','') as mo_vehi,nvl(b.staten,'-') as staten,a.approxval as bill_tot,a.type,a.vchnum,a.vchdate,a.acode,nvl(b.gst_no,'-') As gst_no,nvl(c.brdist_kms,0) As dist_kms,nvl(a.st_entform,'-') as st_entform,b.staffcd from ivoucher a, famst b,famstbal c where c.branchcd='" + frm_mbr + "' and trim(a.acode)=trim(C.acode) and trim(a.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '2%' and a.vchnum||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy') in (" + col1 + ") order by a.vchdate,a.vchnum";
                        SQuery = "select a.vchnum,a.vchdate,a.acode,'-' as desp_From,a.tpt_names as tpt_name,'-' as tptcode,nvl(b.district,'-') as district,nvl(b.addr3,'-') as addr3,nvl(b.pincode,'-') as pincode,b.aname,replace(replace(replace(a.binno,'/',''),'-',''),' ','') as vehi_no2,replace(replace(replace(a.mode_tpt,'/',''),'-',''),' ','') as mo_vehi,nvl(b.staten,'-') as staten,a.bill_tot,a.type,nvl(b.gst_no,'-') As gst_no,0 As dist_kms,a.st_entform,b.staffcd from (select a.vchnum,a.vchdate,a.acode,a.tpt_names,a.binno,a.mode_tpt,sum(nvl(a.spexc_amt,0)) as bill_tot,a.type,nvl(a.gstvchnum,'-') as st_entform from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type in ('58','59') and a.vchnum||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy') in (" + col1 + ") group by a.vchnum,a.vchdate,a.acode,a.tpt_names,a.binno,a.mode_tpt,a.type,nvl(a.gstvchnum,'-')) a, famst b where trim(a.acode)=trim(B.acode)";
                    }
                    else
                    {
                        SQuery = "select nvl(a.ins_no,'-') as tpt_name,nvl(a.tptcode,'-') as tptcode,nvl(b.district,'-') as district,nvl(b.pincode,'-') as pincode,b.aname,replace(replace(replace(a.mo_vehi,'/',''),'-',''),' ','') as mo_vehi,nvl(b.staten,'-') as staten,a.bill_tot,a.type,a.vchnum,a.vchdate,a.acode,nvl(b.gst_no,'-') As gst_no,nvl(c.brdist_kms,0) As dist_kms,nvl(a.st_entform,'-') as st_entform,b.staffcd from sale a, famst b,famstbal c where c.branchcd='" + frm_mbr + "' and  trim(a.acode)=trim(c.acode) and trim(a.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%'  and a.vchnum||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy') in (" + col1 + ") order by a.vchdate,a.vchnum";
                        SQuery = "select nvl(a.desp_from,'-') as desp_from,nvl(a.ins_no,'-') as tpt_name,nvl(a.tptcode,'-') as tptcode,nvl(b.district,'-') as district,nvl(b.addr3,'-') as addr3,nvl(b.pincode,'-') as pincode,b.aname,replace(replace(replace(a.mo_vehi,'/',''),'-',''),' ','') as mo_vehi,nvl(b.staten,'-') as staten,a.bill_tot,a.type,a.vchnum,a.vchdate,a.acode,nvl(b.gst_no,'-') As gst_no,0 As dist_kms,nvl(a.einv_no,'-') as st_entform,b.staffcd,a.cscode from sale a, famst b where trim(a.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%'  and a.vchnum||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy') in (" + col1 + ") order by a.vchdate,a.vchnum";
                    }
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);


                    if (dt.Rows.Count > 0)
                    {
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
                            sg1_dr["sg1_srno"] = i + 1;
                            sg1_dr["sg1_f1"] = dt.Rows[i]["desp_From"].ToString().Trim();
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = dt.Rows[i]["acode"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["Type"].ToString().Trim();//Convert.ToDateTime(dt.Rows[i]["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_f5"] = dt.Rows[i]["vchnum"].ToString().Trim();

                            sg1_dr["sg1_t1"] = Convert.ToDateTime(dt.Rows[i]["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_t2"] = dt.Rows[i]["aname"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["gst_no"].ToString().Trim();
                            if (dt.Rows[i]["staffcd"].ToString().Trim() == "-")
                            {
                                // fgen.execute_cmd(frm_qstr, frm_cocd, "update famst set staffcd='" + dt.Rows[i]["gst_no"].ToString().Trim().Substring(0, 2) + "' where trim(acode)='" + dt.Rows[i]["acode"].ToString().Trim() + "'");
                            }
                            sg1_dr["sg1_t4"] = dt.Rows[i]["staten"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["bill_tot"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["MO_VEHI"].ToString().Trim();
                            double distance = Convert.ToDouble(fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(brdist_kms,0) as brdist_kms from famstbal where branchcd='" + frm_mbr + "' and trim(Acode)='" + dt.Rows[i]["acode"].ToString().Trim() + "'", "brdist_kms"));
                            if (distance <= 0)
                            {
                                fgen.msg("-", "AMSG", "Please update distance from Sale Locn to Customer for Line no. " + i + "");
                            }
                            sg1_dr["sg1_t7"] = distance;
                            sg1_dr["sg1_t8"] = dt.Rows[i]["st_entform"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["district"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["pincode"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["tpt_name"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["tptcode"].ToString().Trim();
                            if (dt.Rows[i]["tptcode"].ToString().Trim().Length == 6)
                            {
                                sg1_dr["sg1_t12"] = fgen.seek_iname(frm_qstr, frm_cocd, "select (Case when length(Trim(nvl(exc_regn,'-')))>5 then exc_regn else gst_no end) as tpt_id from famst where trim(Acode)='" + dt.Rows[i]["tptcode"].ToString().Trim() + "'", "tpt_id");
                            }

                            if (dt.Rows[i]["Type"].ToString().Trim().Substring(0, 1) == "4")
                                sg1_dr["sg1_t20"] = "1";
                            else
                                sg1_dr["sg1_t20"] = "-";
                            sg1_dt.Rows.Add(sg1_dr);
                        }


                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        // ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        // ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        hffield.Value = "";
                        //edmode.Value = "Y";
                    }
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;


                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;


                    //SQuery = "Select b.iname,b.cpartno,b.cdrgno,b.unit,trim(a.srno) as morder1,a.*,to_chaR(a.invdate,'dd/mm/yyyy') as pinvdate,to_chaR(a.vchdate,'dd/mm/yyyy') as pvchdate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum||trim(a.icode)||trim(a.srno)='" + col1 + "' ORDER BY A.srno";
                    SQuery = "select  EMPCODE,NAME, DEPTT_TEXT,DESG_TEXT,DTJOIN from empmas  where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + col1 + "' order by grade";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        //txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        //txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");



                        //txtlbl10.Text = dt.Rows[i]["iqty_chl"].ToString().Trim();
                        //txtlbl11.Text = dt.Rows[i]["iqtyin"].ToString().Trim();
                        //txtlbl12.Text = dt.Rows[i]["acpt_ud"].ToString().Trim();
                        //txtlbl13.Text = dt.Rows[i]["rej_rw"].ToString().Trim();
                        //txtlbl14.Text = dt.Rows[i]["iexc_addl"].ToString().Trim();

                        //doc_addl.Value = dt.Rows[i]["morder1"].ToString().Trim();

                        //txtlbl2.Text = dt.Rows[i]["vchnum"].ToString().Trim();
                        //txtlbl3.Text = dt.Rows[i]["pvchdate"].ToString().Trim();

                        //txtlbl5.Text = dt.Rows[i]["invno"].ToString().Trim();
                        //txtlbl6.Text = dt.Rows[i]["pinvdate"].ToString().Trim();

                        txtlbl4.Text = col1;
                        //txtlbl4a.Text = col2;
                        //txtlbl4.Text = dt.Rows[i]["acode"].ToString().Trim();
                        //txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where trim(upper(acode))=upper(Trim('" + txtlbl4.Text + "'))", "aname");

                        //txtlbl7.Text = dt.Rows[i]["icode"].ToString().Trim();
                        //txtlbl7a.Text = dt.Rows[i]["iname"].ToString().Trim();

                        //txtlbl8.Text = dt.Rows[i]["iqtyin"].ToString().Trim();
                        //txtlbl9.Text = dt.Rows[i]["btchno"].ToString().Trim();
                    }
                    dt.Dispose();
                    // SQuery = "Select * from inspmst a where a.branchcd='" + frm_mbr + "' and a.icode='" + txtlbl7.Text + "' ORDER BY A.srno";
                    SQuery = "select  EMPCODE AS COL1,NAME AS COL2, DEPTT_TEXT AS COL3,DESG_TEXT AS COL4,TO_CHAR(DTJOIN,'dd/MM/yyyy') AS COL6,ENT_DT,ENT_BY from empmas  where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + col1 + "' order by grade";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
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
                            sg1_dr["sg1_srno"] = i + 1;
                            sg1_dr["sg1_f1"] = dt.Rows[i]["col1"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["col2"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["col6"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }


                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        //edmode.Value = "Y";
                    }
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

                    //if (col1.Length <= 0) return;
                    //txtlbl7.Text = col1;
                    //txtlbl7a.Text = col2;
                    //txtlbl2.Focus();
                    break;
                case "MRESULT":

                    if (col1.Length <= 0) return;
                    //txtlbl101.Text = col1;
                    //txtlbl101a.Text = col2;
                    break;
                case "sg1_t11":
                    if (col1.Length > 1)
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = col2;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t12")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t13")).Text = col1;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t12")).Focus();
                    }
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        if (col1.Length > 0)
                            SQuery = "select  type1 as fstr, name as District ,type1 as code,acref from typegrp where id='DT' order by name where trim(code) ='" + col1 + "'";

                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();

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

                    //********* Saving in Hidden Field 
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                    // sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    //********* Saving in GridView Value
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t7")).Text = col1;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    setColHeadings();
                    break;
                case "SG1_ROW_ADD1_E":
                    if (col1.Length <= 0) return;

                    //********* Saving in Hidden Field 
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                    // sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    //********* Saving in GridView Value
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t9")).Text = col1;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Text = col3;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = col5;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
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
                    //#region Remove Row from GridView
                    //if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    //{
                    //    dt = new DataTable();
                    //    sg1_dt = new DataTable();
                    //    dt = (DataTable)ViewState["sg1"];
                    //    z = dt.Rows.Count - 1;
                    //    sg1_dt = dt.Clone();
                    //    sg1_dr = null;
                    //    i = 0;
                    //    for (i = 0; i < sg1.Rows.Count - 1; i++)
                    //    {
                    //        sg1_dr = sg1_dt.NewRow();
                    //        sg1_dr["sg1_srno"] = (i + 1);
                    //        sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.Trim();
                    //        sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.Trim();
                    //        sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.Trim();
                    //        sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.Trim();
                    //        sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.Trim();
                    //        sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.Trim();
                    //        sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.Trim();
                    //        sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.Trim();
                    //        sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.Trim();
                    //        sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.Trim();

                    //        sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.Trim();
                    //        sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim();
                    //        sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                    //        sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                    //        sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();

                    //        sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                    //        sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                    //        sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                    //        sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                    //        sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                    //        sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                    //        sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                    //        sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                    //        sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                    //        sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                    //        sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                    //        sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                    //        sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                    //        sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                    //        sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                    //        sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();

                    //        sg1_dt.Rows.Add(sg1_dr);
                    //    }

                    //    if (edmode.Value == "Y")
                    //    {
                    //        //sg1_dr["sg1_f1"] = "*" + sg1.Rows[-1 + Convert.ToInt32(hf1.Value.Trim())].Cells[13].Text.Trim();

                    //        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                    //    }
                    //    else
                    //    {
                    //        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                    //    }

                    //    sg1_add_blankrows();

                    //    ViewState["sg1"] = sg1_dt;
                    //    sg1.DataSource = sg1_dt;
                    //    sg1.DataBind();
                    //}
                    //#endregion
                    setColHeadings();
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        frm_vty = "IR";
        DateTime cdate = Convert.ToDateTime("01/01/2018");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "Show")
        {

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            if (Convert.ToDateTime(PrdRange.Substring(17, 10)) < cdate)
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please choose Dates in GST Regime (01/01/2018 onward)!!");
                return;
            }
            else if (Convert.ToDateTime(PrdRange.Substring(56, 10)) < cdate)
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " ,  Please cshoose Dates in GST Regime (01/01/2018 onward)!!");
                return;
            }
            else
            {
                hffield.Value = "DOCTYPE";
                make_qry_4_popup();
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "FINSYS");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);

                fgen.Fn_open_sseek("Select Entry ", frm_qstr);
                return;

            }
        }

        if (hffield.Value == "LIST_IRN" || hffield.Value == "Print")
        {
            fgen.msg("-", "CMSG", "Choose Yes for Invoice '13' Choose No for Dr/Cr Note");
            return;
        }
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            //SQuery = "select a.Vchnum as Templ_no,to_char(a.vchdate,'dd/mm/yyyy') as Templ_Dt,c.Aname as Supplier,b.Iname,b.Cpartno,a.Col1 as Parameter,a.col2 as Standard,a.col3 as Lower_lmt,a.col4 as Upper_limit,a.acode,a.icode,a.Ent_by,a.ent_Dt ,to_Char(a.vchdate,'yyyymmdd') as vdd,a.srno from " + frm_tabname + " a,item b,famst c where trim(A.acode)=trim(c.acode) and trim(A.icode)=trim(b.icode) and  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + " order by vdd ,a.vchnum ,a.srno";
            SQuery = "select a.doc_type,a.doc_no,to_char(a.doc_Dt,'dd/mm/yyyy') as doc_dt,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,b.ANAME,b.gst_no,b.stATEN,a.doc_value,a.vehi_no,a.appx_dist,a.irn_no,a.gto_pin,a.ent_by,a.d_Cscode,a.acode,nvl(a.irn_stat,'-') AS IRN_STAT,trim(nvl(a.ack_no,'-')) as Ack_no,trim(nvl(a.ack_dt,'-')) as Ack_dt,trim(nvl(a.eway_bill,'-')) as eway_bill,a.exe_ver from einv_rec a,famst b where trim(a.acode)=trim(B.acode) and a.VCHDATE " + PrdRange + " AND a.type='" + frm_vty + "' and a.branchcd='" + frm_mbr + "'  order by to_char(a.vchdate,'dd/mm/yyyy') ,a.vchnum,a.doc_no ";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of Docs  Created Through IRN facility between " + fromdt + "to" + todt, frm_qstr, "");
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------
            //if (txtlbl4.Text.Trim().Length < 2)
            //{
            //    Checked_ok = "N";
            //    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Department Not Filled Correctly !!");
            //}
            //for (i = 0; i < sg1.Rows.Count - 0; i++)
            //{
            //    if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) <= 0)
            //    {
            //        Checked_ok = "N";
            //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");
            //        i = sg1.Rows.Count;
            //    }
            //}

            if (frm_vty == "20")
            {
                if (fgen.make_double(txtlbl11.Text) < (fgen.make_double(txtlbl12.Text) + fgen.make_double(txtlbl13.Text)))
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Total of Accpted and Rejected Quantity Not Filled Correctly !!");
                    return;
                }

                if (fgen.make_double(txtlbl12.Text) < 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Accepted Quantity Not Filled Correctly   !!");
                    return;
                }
                if (fgen.make_double(txtlbl13.Text) < 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Rejected Quantity Not Filled Correctly   !!");
                    return;
                }
            }
            string last_entdt;
            //checks
            if (edmode.Value == "Y")
            {
            }
            else
            {
                //last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  ", "ldt");
                //if (last_entdt == "0")
                //{ }
                //else
                //{
                //    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                //    {
                //        Checked_ok = "N";
                //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                //    }
                //}
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
                        ///Datatable and dataset
                        ///Dattable  =rows and colums
                        ///Dataset = tables ka records



                        oDS = new DataSet();
                        oporow = null;
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);



                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();
                        //save_fun2();


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
                            save_it = "N";
                            for (i = 0; i < sg1.Rows.Count - 0; i++)
                            {

                                if (sg1.Rows[i].Cells[15].Text.Trim().Length > 1)
                                {
                                    save_it = "Y";
                                }
                            }

                            if (save_it == "Y" && oDS.Tables[0].Rows.Count > 0)
                            {
                                doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }

                                //i = 0;


                                //do
                                //{
                                //    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' ", 6, "vch");
                                //    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                //    if (i > 20)
                                //    {
                                //        fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                //        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' ", 6, "vch");
                                //        pk_error = "N";
                                //        i = 0;
                                //    }
                                //    i++;
                                //}
                                //while (pk_error == "Y");
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        //save_fun2();



                        if (edmode.Value == "Y" && oDS.Tables[0].Rows.Count > 0)
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }

                        //final saving in oracle
                        if (oDS.Tables[0].Rows.Count > 0)
                            fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);


                        // for every row - updating number in ivoucher and sale 
                        for (i = 0; i < sg1.Rows.Count; i++)
                        {
                            if ((sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1)) == "4")
                            {
                                do_upd_tran_file("SALE", i);// save
                            }
                            else
                            {
                                do_upd_tran_file("IVOUCHER", i);// save
                            }
                        }



                        if (edmode.Value == "Y" && oDS.Tables[0].Rows.Count > 0)
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);


                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Finsys ERP", "vipin@tejaxo.com", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
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

                //sg1.Columns[10].Visible = false;
                //sg1.Columns[11].Visible = false;
                ////sg1.Columns[31].Visible = false;
                ////sg1.Columns[32].Visible = false;
                ////sg1.Columns[33].Visible = false;
                ////sg1.Columns[34].Visible = false;
                ////sg1.Columns[35].Visible = false;


                // // set column width

                ////sg1.HeaderRow.Cells[13].Text = "ACODE";
                //sg1.HeaderRow.Cells[13].Width = 30;
                ////sg1.HeaderRow.Cells[14].Text = "TYPE";
                //sg1.HeaderRow.Cells[14].Width = 30;
                ////sg1.HeaderRow.Cells[15].Text = "DOC_NO";
                //sg1.HeaderRow.Cells[15].Width = 50;
                ////sg1.HeaderRow.Cells[16].Text = "A/CNAME";
                //sg1.HeaderRow.Cells[16].Width = 30;
                ////sg1.HeaderRow.Cells[17].Text = "GSTNO";
                //sg1.HeaderRow.Cells[17].Width = 100;
                ////sg1.HeaderRow.Cells[18].Text = "DESTSTATE";
                //sg1.HeaderRow.Cells[18].Width = 70;
                ////sg1.HeaderRow.Cells[19].Text = "VALUE";
                //sg1.HeaderRow.Cells[19].Width = 60;
                ////sg1.HeaderRow.Cells[20].Text = "VEHI_NUMBER";
                //sg1.HeaderRow.Cells[20].Width = 80;
                ////sg1.HeaderRow.Cells[21].Text = "DISTANCE";
                //sg1.HeaderRow.Cells[21].Width = 70;
                ////sg1.HeaderRow.Cells[22].Text = "EWAY_BILL_NO";
                //sg1.HeaderRow.Cells[22].Width = 110;

                ////sg1.HeaderRow.Cells[23].Text = "TOPLACE";
                //sg1.HeaderRow.Cells[23].Width = 110;
                ////sg1.HeaderRow.Cells[24].Text = "TOPINCODE";
                //sg1.HeaderRow.Cells[24].Width = 110;
                ////sg1.HeaderRow.Cells[25].Text = "TRANSPNAME";
                //sg1.HeaderRow.Cells[25].Width = 110;

                ////sg1.HeaderRow.Cells[26].Text = "TRANSID";
                //sg1.HeaderRow.Cells[26].Width = 110;
                ////sg1.HeaderRow.Cells[27].Text = "TRANSPCODE";
                //sg1.HeaderRow.Cells[27].Width = 110;


                // //sg1.Rows[sg1r].Cells[8].Attributes.Add("readonly", "false");


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
            //if (index < sg1.Rows.Count - 1)
            //{
            //    hf1.Value = index.ToString();
            //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            //    //----------------------------
            //    hffield.Value = "SG1_RMV";
            //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
            //    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
            //}
            //break;


            case "SG1_ROW_ADD":

                if (index < sg1.Rows.Count)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select place", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Item", frm_qstr);
                    //fgen.Fn_open_mseek("Select Item", frm_qstr);
                }
                break;

            case "SG1_ROW_ADD1":

                if (index < sg1.Rows.Count)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_ADD1_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Transporter", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD1";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Item", frm_qstr);
                    //fgen.Fn_open_mseek("Select Item", frm_qstr);
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
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        string foundInv = "";

        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[15].Text.Trim().Length > 1)
            {
                foundInv = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT BRANCHCD||TRIM(DOC_TYPE)||TRIM(DOC_NO)||TO_CHAR(DOC_DT,'DD/MM/YYYY')||tRIM(ACODE) AS FSTR FROM EINV_REC WHERE BRANCHCD||TRIM(DOC_TYPE)||TRIM(DOC_NO)||TO_CHAR(DOC_DT,'DD/MM/YYYY')||tRIM(ACODE)='" + frm_mbr + sg1.Rows[i].Cells[16].Text.Trim() + sg1.Rows[i].Cells[17].Text.Trim() + ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text + sg1.Rows[i].Cells[15].Text.Trim() + "' ", "fstr");
                if (foundInv.Length < 2)
                {
                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = frm_mbr;
                    oporow["TYPE"] = frm_vty;
                    oporow["vchnum"] = txtvchnum.Text.Trim();
                    oporow["vchdate"] = txtvchdate.Text.Trim();

                    oporow["SRNO"] = i + 1;
                    oporow["d_dfrom"] = sg1.Rows[i].Cells[13].Text.Trim();
                    oporow["d_Cscode"] = sg1.Rows[i].Cells[14].Text.Trim();

                    oporow["acode"] = sg1.Rows[i].Cells[15].Text.Trim();
                    oporow["doc_type"] = sg1.Rows[i].Cells[16].Text.Trim();
                    oporow["Doc_No"] = sg1.Rows[i].Cells[17].Text.Trim();
                    oporow["doc_Dt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text;
                    oporow["to_state"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text;
                    oporow["doc_value"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text;
                    oporow["vehi_no"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text;
                    oporow["appx_dist"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text;
                    oporow["irn_no"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text;
                    oporow["irnqr_1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text;
                    oporow["irnqr_2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text;
                    oporow["ack_no"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text;
                    oporow["ack_dt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text;
                    oporow["gto_place"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text;
                    oporow["gto_pin"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text;
                    oporow["gtpt_name"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text;
                    oporow["gtpt_id"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text;
                    oporow["gtpt_Code"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text;
                    oporow["irn_stat"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text;

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

                    }
                    oDS.Tables[0].Rows.Add(oporow);
                }
            }

        }
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
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F30111":
                ////SQuery = "SELECT '10' AS FSTR,'Quality Outward Certificate' as NAME,'10' AS CODE FROM dual";
                ////fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "EW");
                break;

        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "IR");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }

    //------------------------------------------------------------------------------------   
    protected void btnshow_ServerClick(object sender, EventArgs e)
    {
        if (txtvchnum.Text.Trim().Length < 6)
        {
            fgen.msg("-", "AMSG", "Please Press New to Start");
            return;
        }
        if (sg1.Rows[0].Cells[13].Text.Trim().Length > 2)
        {
            fgen.msg("", "ASMG", "Invoice Already Selected , Please Make New Sheet");
            return;
        }
        hffield.Value = "Show";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    protected void btnjson_ServerClick(object sender, EventArgs e)
    {
        if (web_einv_ok == "N")
        {
            fgen.msg("", "ASMG", "Please Get Webtel Utility Activated");
            return;
        }
        gen_eway_bill("WEBT");
        used_opt = "-";
    }

    private void gen_eway_bill(string FOPT)
    {
        used_opt = FOPT;
        string cc_final = "", vupd_tax, send_59_row, send_unitmaster, send_gstvch_no, chgd_pos, send_ciname;
        dt2 = new DataTable();

        send_ciname = fgen.getOptionPW(frm_qstr, frm_cocd, "W1084", "OPT_enable", frm_mbr);
        send_unitmaster = fgen.getOption(frm_qstr, frm_cocd, "W0216", "OPT_enable");
        send_gstvch_no = fgen.seek_iname(frm_qstr, frm_cocd, "select upper(trim(enable_yn))||trim(params) as enable_yn from controls where id='O37'", "enable_yn");

        string err_str;
        string[] Edesc = new string[50];
        int err_Cnt;
        DataTable dt = new DataTable();

        if (txtvchnum.Text.Trim().Length < 6)
        {
            fgen.msg("-", "AMSG", "Please Press New to Start");
            return;
        }
        cc_final = "";
        string g_uid, g_pwd, g_zip;
        string g_efuuid, g_efupwd, g_efukey, gst_name, VNAME, coaddr1 = "", coaddr2 = "";
        string g_api_link;

        int TOT_INV;
        TOT_INV = 0;
        string mygstno = "";

        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, "select gst_no,gstewb_id,gstewb_pw,zipcode,gstefu_id,gstefu_pw,gstefu_cdkey,irn_apiadd,trim(tele) as tele,trim(name) as name,addr,addr1,place,email from type where id='B' and type1='" + frm_mbr + "'");

        mygstno = dt.Rows[0]["gst_no"].ToString();
        g_uid = dt.Rows[0]["gstewb_id"].ToString();
        g_pwd = dt.Rows[0]["gstewb_pw"].ToString();
        g_zip = dt.Rows[0]["zipcode"].ToString();
        g_efuuid = dt.Rows[0]["gstefu_id"].ToString();
        g_efupwd = dt.Rows[0]["gstefu_pw"].ToString();
        g_efukey = dt.Rows[0]["gstefu_cdkey"].ToString();
        g_api_link = dt.Rows[0]["irn_apiadd"].ToString();
        cotel = dt.Rows[0]["tele"].ToString();
        gst_name = dt.Rows[0]["name"].ToString();
        coaddr1 = dt.Rows[0]["addr"].ToString();
        coaddr2 = dt.Rows[0]["addr1"].ToString();
        coaddr3 = dt.Rows[0]["Place"].ToString();
        coemail = dt.Rows[0]["email"].ToString();

        VNAME = fgen.getOption(frm_qstr, frm_cocd, "W0215", "OPT_enable");
        if (VNAME != "Y")
        {
            gst_name = fgenCO.chk_co(frm_cocd);
        }


        if ((g_api_link.ToString().Trim().Length < 10) && (FOPT == "WEBT"))
        {
            // g_api_link = "http://ip.webtel.in/ewaygsp2/sandbox/EWayBill/GenEWB";
            //HTTP://IP.WEBTEL.IN/EWAYGSP/EWAYBILL/GENEWB.
            fgen.msg("", "ASMG", "Portal API not linked in Plant Master , JSON File Will be Generated");
            //return;
        }

        if (cotel.Trim().Length > 12 || cotel.Trim().Length < 10)
        {
            //fgen.msg("", "ASMG", "Please correct company telephone no in branch master.IRN Generation not possible.");
            //return;
        }

        string res, AA = "", BB = "", cc = "", dd = "";
        string gf01, gf02, gf03, gf04, gf05, gf06, gf07, gf08, gf09, gf10;
        string gf11, gf12, gf13, gf14, gf15, gf16, gf17, gf18, gf19, gf20;
        string gf21, gf22, gf23, gf24, gf25, gf26, gf27, gf28, gf29, gf30;
        string gf31, gf32, gf33, gf34, gf344, gf345, gf35, gf36, gf37, gf38, gf39, gf40;
        string gf41, gf42, gf43, gf44, gf45, gf46, gf47, gf48, gf49, tran_type = "", sagm_4f_hs;
        string gf51, gf52, gf53, gf54, gf55, gf56, gf18a, gf56a, gf18b, gf54a, gf54b, gf56b, incl_ipack_irate = "", gf346, gf347;
        int i; double chk_iamt;
        //string p As Object

        err_str = "";
        err_Cnt = 0;
        tran_type = "1";
        for (i = 0; i < sg1.Rows.Count; i++)
        {
            if ((sg1.Rows[i].Cells[17].Text.Trim().Length > 1) && (((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper().Length < 2))
            {
                err_str = err_str + " Place Not Filled ";
                err_Cnt = err_Cnt + 1;
                ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).BackColor = System.Drawing.Color.Cyan;
            }


            if ((sg1.Rows[i].Cells[17].Text.Trim().Length > 1) && (((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().Length < 6))
            {
                err_str = err_str + " Pin Code of Buyer Not Correct  ";
                err_Cnt = err_Cnt + 1;
                ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).BackColor = System.Drawing.Color.Cyan;
            }
            if (make_ewayb == "Y")
            {
                if ((sg1.Rows[i].Cells[17].Text.Trim().Length > 1) && (((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim() == "ROAD") && (((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim().ToUpper().Length <= 10))
                {
                    err_str = err_str + " Transporter ID Not Filled.Its required for making Eway Bill.";
                    err_Cnt = err_Cnt + 1;
                    ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).BackColor = System.Drawing.Color.Cyan;
                }

                if ((sg1.Rows[i].Cells[17].Text.Trim().Length > 1) && (((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper().Length <= 3 && sg1.Rows[i].Cells[16].Text.Substring(0, 1) != "5"))
                {
                    err_str = err_str + " Vehicle No. Not Filled.";
                    err_Cnt = err_Cnt + 1;
                    ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).BackColor = System.Drawing.Color.Cyan;

                }
            }


        }

        if (err_Cnt > 0)
        {
            fgen.msg("", "ASMG", "Total errors " + err_Cnt + "Please Correct Indicated Cells to Proceed");
            return;
        }


        for (i = 0; i < sg1.Rows.Count; i++)
        {

            if (sg1.Rows[i].Cells[17].Text.Trim().Length > 1)
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update famst set addr3='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper() + "' where trim(nvl(addr3,'-'))='-' and trim(acode)='" + sg1.Rows[i].Cells[15].Text + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "commit");
            }


            if (sg1.Rows[i].Cells[17].Text.Trim().Length > 1)
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update famst set pincode='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().ToUpper() + "' where trim(nvl(pincode,'-'))='-' and trim(acode)='" + sg1.Rows[i].Cells[15].Text + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "commit");
            }


        }

        send_59_row = "";
        send_59_row = fgen.getOptionPW(frm_qstr, frm_cocd, "W1081", "OPT_enable", frm_mbr);
        if (send_59_row == "N")
        {
            for (i = 0; i < sg1.Rows.Count; i++)
            {
                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4")
                {
                    if (((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().Length > 1)
                    {
                        upd_addl_sal_exp(frm_mbr + sg1.Rows[i].Cells[16].Text.Trim() + sg1.Rows[i].Cells[17].Text.Trim() + sg1.Rows[i].Cells[18].Text.Trim());
                    }
                }
            }
        }

        int rowcnt = 0;
        for (i = 0; i < sg1.Rows.Count; i++)
        {
            if (sg1.Rows[i].Cells[17].Text.Trim().Length > 1)
            {
                rowcnt = rowcnt + 1;
            }
        }
        if (rowcnt > 50)
        {
            fgen.msg("", "ASMG", "Please choose Maximum 50 Invoices at a time");
            return;
        }
        //
        if (edmode.Value == "Y")
        { }
        else
        {
            // for saving
            oDS = new DataSet();
            oporow = null;
            oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
            //  new number
            doc_is_ok = "";
            frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
            doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
            if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
            save_fun();
            fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
        }
        ///

        double chl_taxes;
        chl_taxes = 0;

        for (i = 0; i < sg1.Rows.Count; i++)
        {
            if ((((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Length > 1) && (((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper().Length < 60))
            {
                chgd_pos = "";
                gf01 = mygstno;
                gf02 = "O";
                gf03 = "1";
                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "4F")
                {
                    gf03 = "3";
                }
                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4")
                {
                    gf04 = "INV";
                }
                else
                {
                    if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "58")
                    {
                        gf04 = "CRN";
                    }
                    else
                    {
                        gf04 = "DBN";
                    }
                }

                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4")
                {
                    gf05 = "(Case when trim(nvl(b.full_invno,'-'))='-' then a.vchnum else b.full_invno end )";
                }
                else
                {
                    gf05 = "(Case when trim(nvl(a.gstvch_no,'-'))='-' then a.branchcd||a.type||'-'||a.vchnum else trim(nvl(a.gstvch_no,'-')) end )";
                }
                if (FOPT == "JSON")
                {
                    gf06 = "to_char(a.vchdate,'yyyy-mm-dd')";
                }
                else
                {
                    gf06 = "to_char(a.vchdate,'yyyy-mm-dd')";
                }

                firm = fgenCO.chk_co(frm_cocd);

                gf07 = "'" + mygstno + "'";
                gf08 = "'" + firm.Trim().Left(100) + "'";

                gf09 = "'" + coaddr1.Trim().Left(100) + "'";
                gf10 = "'" + coaddr2.Trim().Left(100) + "'";
                gf11 = "'" + txtlbl4.Text.Trim().Left(50) + "'";
                gf12 = "'" + mygstno.Substring(0, 2) + "'";
                gf13 = "'" + txtlbl7.Text.Trim() + "'";
                //   'customer
                gf14 = "c.gst_no";
                gf15 = "trim(replace(replace(c.aname,chr(34),''),chr(39),''))";
                gf16 = "substr(trim(replace(replace(replace(c.addr1,'''','`'),chr(34),''),chr(39),'')),1,100)";
                gf17 = "substr(trim(replace(replace(replace(c.addr2,'''','`'),chr(34),''),chr(39),'')),1,100)";
                gf18 = "substr(trim(replace(replace(replace(c.district,'''','`'),chr(34),''),chr(39),'')),1,100)";
                gf18a = "substr(trim(replace(replace(replace(nvl(c.country,'-'),'''','`'),chr(34),''),chr(39),'')),1,100)";
                gf18b = "substr(trim(replace(replace(replace(nvl(c.addr3,'-'),'''','`'),chr(34),''),chr(39),'')),1,100)";
                gf19 = "trim(c.staffcd)";
                gf20 = "trim(c.pincode)";

                gf21 = "1";
                gf22 = "'" + ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim() + "'";

                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) != "4")
                {
                    gf23 = "trim(a.thru)";
                }
                else gf23 = "trim(b.ins_no)";
                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4" && sg1.Rows[i].Cells[14].Text.Trim().Length == 6)
                {
                    gf24 = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                }
                else gf24 = "nvl(f.brdist_kms,0)";

                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4")
                {
                    gf25 = "(case when trim(b.grno)='-' then null else trim(b.grno) end)";
                    gf26 = "to_char(b.grdate,'yyyymmdd')";
                    gf27 = "trim(replace(replace(replace(b.mo_Vehi,'/',''),'-',''),' ',''))";
                }

                else
                {
                    gf25 = "'-'";
                    gf26 = "'-'";
                    gf27 = "'-'";
                }

                gf28 = "nvl(a.morder,0)";
                //gf29 = "substr(trim(e.name),1,100)";
                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) != "4")
                {
                    gf30 = "substr(trim(replace(replace(replace(d.iname,'" + "" + "',' Inch'),'''','`'),chr(39),'')),1,120)";
                    gf32 = "a.iqty_chl";
                    gf347 = "(1";
                }
                else
                {
                    if (send_ciname == "Y")
                    {
                        gf30 = "substr(trim(replace(replace(d.ciname,'" + "" + "',' Inch'),'''','`')),1,120)";
                    }
                    else
                    {
                        gf30 = "substr(trim(replace(replace(a.purpose,'" + "" + "',' Inch'),'''','`')),1,120)";
                    }
                    gf32 = "(case when (a.iqtyout=0 and a.irate > 0) then 1 else a.iqtyout end)";
                    if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "48") gf32 = "a.iqty_chl";
                    gf347 = "(1-nvl(a.ichgs,0)/100)";
                    //gf347 = "(1-(nvl(a.ichgs,0)+nvl(a.st_nmodv,0)+nvl(a.st_modv,0))/100)";
                    //gf347 = "((1-(nvl(a.ichgs,0)+nvl(a.st_modv,0))/100) - ((1-(nvl(a.ichgs,0)+nvl(a.st_modv,0))/100)* nvl(a.st_nmodv,0)/100))";
                    gf347 = "(case when nvl(a.st_modv,0) > 0 then ((1-(nvl(a.ichgs,0)+nvl(a.st_modv,0))/100) - ((1-(nvl(a.ichgs,0)+nvl(a.st_modv,0))/100)* nvl(a.st_nmodv,0)/100)) else (1-(nvl(a.ichgs,0)+nvl(a.st_nmodv,0)+nvl(a.st_modv,0))/100) end";
                }

                gf31 = "trim(replace(replace(replace(replace(d.hscode,'.',''),'/',''),'-',''),' ',''))";
                gf33 = "trim(d.unit)";

                if (frm_cocd == "MEGA") incl_ipack_irate = "N";
                if (incl_ipack_irate == "Y")
                    gf346 = "a.irate + a.ipack";
                else
                    gf346 = "a.irate";

                if (send_59_row == "Y")
                {

                    gf34 = "round(" + gf32 + "*(a.irate * " + gf347 + ")),2)+round(" + gf32 + " * nvl(a.ipack,0),2)+round(" + gf32 + " * nvl(a.iexc_Addl,0),2)+round(" + gf32 + " * nvl(a.idiamtr,0),2)";
                    gf344 = "round(" + gf32 + " * nvl(a.idiamtr,0),2)"; //item_otherchg
                    gf35 = "(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end)";
                    gf36 = "(Case when trim(A.iopr)='CG' then a.cess_pu else 0 end)";
                    gf37 = "(Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end)";
                    gf38 = "(Case when trim(A.iopr)='CG' then a.exc_Amt else 0 end)";
                    gf39 = "(Case when trim(A.iopr)='IG' then a.exc_Rate else 0 end)";
                    gf40 = "(Case when trim(A.iopr)='IG' then a.exc_Amt else 0 end)";
                    vupd_tax = " 1=1 ";
                }
                else
                {
                    gf34 = "round(" + gf32 + " * (a.irate * " + gf347 + ")),2)+round(" + gf32 + " * nvl(a.ipack,0),2)+nvl(a.exp_punit,0)+round(" + gf32 + " * nvl(a.iexc_Addl,0),2)+round( " + gf32 + " * nvl(a.idiamtr,0),2)";
                    gf344 = "round(" + gf32 + " * nvl(a.idiamtr,0),2)";//item_otherchg
                    gf35 = "(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end)";
                    gf36 = "(Case when trim(A.iopr)='CG' then a.cess_pu+nvl(a.rej_sdv,0) else 0 end)";
                    gf37 = "(Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end)";
                    gf38 = "(Case when trim(A.iopr)='CG' then a.exc_Amt+nvl(a.rej_rw,0) else 0 end)";
                    gf39 = "(Case when trim(A.iopr)='IG' then a.exc_Rate else 0 end)";
                    gf40 = "(Case when trim(A.iopr)='IG' then a.exc_Amt+nvl(a.rej_rw,0) else 0 end)";
                    vupd_tax = " nvl(d.tax_item,'-')!='Y' ";
                }

                if (frm_cocd == "AMAR" && sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4")
                { // for item adding other charges withour seperate row bot no tax on additional cost
                    gf34 = "(a.iqtyout*(a.irate * (1-nvl(a.ichgs,0)/100)))+ round(a.iqtyout*nvl(a.iexc_Addl,0),2)";
                    gf344 = "round(a.iqtyout*nvl(a.ipack,0),2)+round(a.iqtyout*nvl(a.idiamtr,0),2)"; //item_otherchg
                }

                if (frm_cocd == "SAGM" && sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "4F") gf32 = "a.iqtyout";

                gf41 = "0";
                gf42 = "0";
                gf43 = g_uid;
                gf44 = g_pwd;

                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4")
                {
                    gf45 = "nvl(b.amt_sale,0)";
                    gf46 = "nvl(b.bill_tot,0)";
                    gf47 = "nvl(b.cscode,'-')";
                    gf48 = "nvl(b.amt_Extexc,0)+ nvl(b.tcsamt,0) ";
                    gf49 = "nvl(b.desp_from,'-')";
                    gf53 = "nvl(a.ichgs,0)";

                }
                else
                {
                    gf45 = "-";
                    gf46 = "-";
                    gf47 = "'-' ";
                    gf48 = "0 ";
                    gf49 = " '-' ";
                    gf53 = "0 ";

                }

                gf51 = "trim(c.telnum)";
                gf52 = "trim(c.email)";
                gf54 = "trim(c.ins_no)";
                gf54a = "trim(nvl(b.pvt_mark,'-'))";
                gf54b = "to_char(b.tptbill_dt,'dd/mm/yyyy')";
                gf56 = "nvl(b.amt_extexc,0)";
                gf55 = "(b.bill_tot - b.amt_Sale - b.st_Amt - b.amt_Exc - b.rvalue - nvl(b.AMT_REA, 0) - nvl(b.AMT_JOB, 0) - nvl(b.tcsamt, 0)  + nvl(b.totdisc_amt,0)) ";
                gf56 = "nvl(b.amt_extexc,0)";
                gf56a = "nvl(b.tcsamt,0)";
                gf56b = "0";
                //gf01 = "05AAACD5767E1ZT";
                //gf43 = "05AAACD5767E1ZT";
                //gf44 = "abc123@@";
                //g_efuuid = "05AAACD8069KIZF";
                // g_efupwd = "abc123@@";
                // g_efukey = "1000687";

                if (frm_cocd == "MEGA") gf56a = "nvl(b.tcsamt,0)+ nvl(b.amt_stsc, 0)";
                gf55 = "(b.bill_tot -b.amt_Sale - b.st_Amt - b.amt_Exc - b.rvalue - nvl(b.AMT_REA, 0) - nvl(b.AMT_JOB, 0) - nvl(b.amt_Sttt, 0) - nvl(b.tcsamt, 0)  + nvl(b.totdisc_amt,0)- nvl(b.amt_stsc, 0)) ";
                if (frm_cocd == "LRFP")
                {
                    gf56b = "nvl(b.tsubs_amt,0)";
                    gf55 = "(b.bill_tot -b.amt_Sale - b.st_Amt - b.amt_Exc - b.rvalue - nvl(b.AMT_REA, 0) - nvl(b.AMT_JOB, 0) - nvl(b.amt_Sttt, 0) - nvl(b.tcsamt, 0)  + nvl(b.totdisc_amt,0)- nvl(b.amt_stsc, 0)+ nvl(b.tsubs_amt,0)) ";
                }
                if (frm_cocd == "BONY")
                {
                    gf56a = "nvl(b.tcsamt,0)+ nvl(b.retention, 0)";
                    gf55 = "(b.bill_tot -b.amt_Sale - b.st_Amt - b.amt_Exc - b.rvalue - nvl(b.AMT_REA, 0) - nvl(b.AMT_JOB, 0) - nvl(b.tcsamt, 0) - nvl(b.retention, 0)  + nvl(b.totdisc_amt,0)) ";
                }

                //    AA = "'" + gf01 + "' as GSTIN,'" + gf02 + "' as sup_type,'" + gf03 + "' as sub_type,'" + gf04 + "' as doc_type," + gf05 + " as doc_no," + gf06 + " as doc_Dt," + gf07 + " as sup_gst," + gf08 + " as sup_nam," + gf09 + " as sup_add1," + gf10 + " as sup_add2," + gf11 + " as sup_add3," + gf12 + " as sup_state," + gf13 + " as sup_pin," + gf14 + " as rec_gst," + gf15 + " as rec_nam," + gf16 + " as rec_add1," + gf17 + " as rec_add2," + gf18 + " as rec_add3," + gf19 + " as rec_state," + gf20 + " as rec_pin,";
                //     BB = "" + gf21 + " as tran_mode," + gf22 + " as tran_ID," + gf23 + " as tran_name," + gf24 + " as tran_dist," + gf25 + " as tran_doc," + gf26 + " as tran_dt," + gf27 + " as vehi_no," + gf28 + " as item_no," + gf29 + " as prod_name," + gf30 + " as prod_desc," + gf31 + " as hs_code," + gf32 + " as Quantity," + gf33 + " as quan_unit," + gf34 + " as taxb_val," + gf35 + " as sgst_rt," + gf36 + " as sgst_val," + gf37 + " as cgst_rt," + gf38 + " as cgst_val," + gf39 + " as igst_rt," + gf40 + " as igst_val," + gf41 + " as cess_rt,";
                AA = "'" + gf01 + "' as GSTIN,'" + gf02 + "' as sup_type,'" + gf03 + "' as sub_type,'" + gf04 + "' as doc_type," + gf05 + " as doc_no," + gf06 + " as doc_Dt," + gf07 + " as sup_gst," + gf08 + " as sup_nam," + gf09 + " as sup_add1," + gf10 + " as sup_add2," + gf11 + " as sup_add3," + gf12 + " as sup_state," + gf13 + " as sup_pin," + gf14 + " as rec_gst," + gf15 + " as rec_nam," + gf16 + " as rec_add1," + gf17 + " as rec_add2," + gf18 + " as rec_add3," + gf18a + " as rec_add4," + gf18b + " as rec_add5," + gf19 + " as rec_state," + gf20 + " as rec_pin,";
                BB = "" + gf21 + " as tran_mode," + gf22 + " as tran_ID," + gf23 + " as tran_name," + gf24 + " as tran_dist," + gf25 + " as tran_doc," + gf26 + " as tran_dt," + gf27 + " as vehi_no," + gf28 + " as item_no," + gf30 + " as prod_desc," + gf31 + " as hs_code," + gf346 + " as irate,a.iamount," + gf53 + " as ichgs," + gf32 + " as Quantity," + gf33 + " as quan_unit," + gf34 + " as taxb_val," + gf344 + " as ioth_chg," + gf35 + " as sgst_rt," + gf36 + " as sgst_val," + gf37 + " as cgst_rt," + gf38 + " as cgst_val," + gf39 + " as igst_rt," + gf40 + " as igst_val," + gf41 + " as cess_rt," + gf42 + " as cess_val,";

                string spl_cond;
                spl_cond = " 1=1 ";
                if (frm_cocd == "WPPL") spl_cond = " a.irate>0 ";
                if (frm_cocd == "MCPL") spl_cond = " a.irate>0 and upper(trim(nvl(a.BINNO,'-')))!='Y' ";

                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4")
                {
                    //cc = "" + gf42 + " as cess_val,'" + gf43 + "' as ewb_user,'" + gf44 + "' as ewb_pwd," + gf45 + " as billstot from ivoucher a,(select branchcd,type,vchnum,vchdate,post,approxval,sum(exc_amt) as amt_Exc,sum(cess_pu) as rvalue from ivoucher where branchcd='" + frm_mbr + "' and type='" + sg1.Rows[i].Cells[14].Text.Trim() + "' and vchnum='" + sg1.Rows[i].Cells[15].Text.Trim() + "' and to_Char(vchdate,'dd/mm/yyyy')='" + sg1.Rows[i].Cells[16].Text.Trim() + "' group by approxval,branchcd,type,vchnum,vchdate,post)b, famst c, item d,typegrp e,famstbal f where a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')=b.branchcd||b.type||b.vchnum||to_char(b.vchdate,'dd/mm/yyyy') and nvl(d.tax_item,'-')!='Y' and f.branchcd='" + frm_mbr + "' and trim(a.acode)=trim(f.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + sg1.Rows[i].Cells[14].Text.Trim() + "' and a.vchnum='" + sg1.Rows[i].Cells[15].Text.Trim() + "' and to_Char(a.vchdate,'dd/mm/yyyy')='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and  e.id='T1' and trim(d.hscode)=trim(e.acref) and trim(a.acode)=trim(C.acode) and trim(a.icode)=trim(d.icode) order by a.morder";
                    cc = "'" + gf43 + "' as ewb_user,'" + gf44 + "' as ewb_pwd," + gf45 + " as billstot," + gf46 + " as billgtot," + gf55 + " as ro_off," + gf56 + " as tot_othchg," + gf47 + " as my_Cscode," + gf48 + " as oth_chgs," + gf49 + " as desp_from1," + gf51 + " as rec_phone," + gf52 + " as rec_email ,a.iopr,a.morder,to_char(b.amt_Exc,99999999.99) as amt_exc,to_char(b.rvalue,99999999.99) as rvalue,b.fdue,b.ins_co,c.pay_num,b.amt_Sale,b.naration,c.district,nvl(b.insp_amt,0) as insp_amt," + gf56a + " as tcsamt," + gf56b + " as return,nvl(b.amt_Extexc,0) as amt_extexc,nvl(b.amt_sttt,0) as amt_sttt,upper(trim(nvl(a.buyer,'-'))) as buyer," + gf54a + " as shp_bill," + gf54b + " as shp_billdt,nvl(b.totdisc_amt,0) as totdisc_amt,nvl(b.chgd_pos,'-') as chgd_pos,0 as iexc_Addl from ivoucher a, sale b , famst c, item d,famstbal f where " + spl_cond + " and " + vupd_tax + "  and f.branchcd='" + frm_mbr + "' and trim(a.acode)=trim(f.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and a.vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and to_Char(a.vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "'  and a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')=b.branchcd||b.type||b.vchnum||to_char(b.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(C.acode) and trim(a.icode)=trim(d.icode) order by " + gf28 + "";
                }
                else
                {
                    // cc = "" + gf42 + " as cess_val,'" + gf43 + "' as ewb_user,'" + gf44 + "' as ewb_pwd," + gf45 + " as billstot from ivoucher a, sale b , famst c, item d,typegrp e,famstbal f where nvl(d.tax_item,'-')!='Y' and f.branchcd='" + frm_mbr + "' and trim(a.acode)=trim(f.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + sg1.Rows[i].Cells[14].Text.Trim() + "' and a.vchnum='" + sg1.Rows[i].Cells[15].Text.Trim() + "' and to_Char(a.vchdate,'dd/mm/yyyy')='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and  e.id='T1' and trim(d.hscode)=trim(e.acref) and a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')=b.branchcd||b.type||b.vchnum||to_char(b.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(C.acode) and trim(a.icode)=trim(d.icode) order by a.morder";
                    string seekn1, seekn2, seekn3, seekn4, seekn5;
                    dt4 = new DataTable();
                    dt4 = fgen.getdata(frm_qstr, frm_cocd, "select (sum(nvl(iamount,0)) + sum(nvl(exc_amt,0)) + sum(nvl(cess_pu,0))+ sum(nvl(psize,0))) as seekn1,sum(nvl(exc_amt,0)) as seekn2,sum(nvl(cess_pu,0)) as seekn3,(sum(nvl(iamount,0)) + sum(nvl(exc_amt,0)) + sum(nvl(cess_pu,0))) as seekn4,(sum(nvl(iexc_Addl,0)*nvl(iqty_chl,0))) as seekn5 from ivoucher where type ='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and branchcd= '" + frm_mbr + "' and vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and to_Char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "' group by branchcd,type,vchnum, to_char(vchdate,'dd/mm/yyyy')");
                    seekn1 = dt4.Rows[0]["seekn1"].ToString();
                    seekn2 = dt4.Rows[0]["seekn2"].ToString();
                    seekn3 = dt4.Rows[0]["seekn3"].ToString();
                    seekn4 = dt4.Rows[0]["seekn4"].ToString();
                    seekn5 = dt4.Rows[0]["seekn5"].ToString();
                    cc = "'" + gf43 + "' as ewb_user,'" + gf44 + "' as ewb_pwd," + Convert.ToDouble(seekn4) + " as billstot,0 as ro_off,0 as totdisc_amt," + Convert.ToDouble(seekn1) + " as billgtot," + gf47 + " as my_Cscode," + gf48 + " as oth_chgs," + gf49 + " as desp_from1," + gf51 + " as rec_phone," + gf52 + " as rec_email ,a.iopr," + Convert.ToDouble(seekn2) + " as amt_exc," + Convert.ToDouble(seekn3) + " as rvalue,'-' as fdue,'-' as ins_co,c.pay_num," + Convert.ToDouble(seekn1) + " as amt_sale," + Convert.ToDouble(seekn4) + " as amt_extexc,'-' as naration,c.district,trim(nvl(a.invno,'-')) as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,nvl(a.psize,0) as tcsamt,0 as insp_amt,'-' as chgd_pos," + Convert.ToDouble(seekn5) + " as iexc_Addl from ivoucher a, famst c, item d,famstbal f where " + spl_cond + " and a.branchcd='" + frm_mbr + "' and trim(a.acode)=trim(f.acode)  and f.branchcd='" + frm_mbr + "' and a.type='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and a.vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and to_Char(a.vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "'  and trim(a.acode)=trim(C.acode) and trim(a.icode)=trim(d.icode) order by " + gf28 + "";

                }



                catcode = "select " + AA + BB + cc;
                // ''' taxb_val is sum of iamount
                if (frm_cocd == "SAGM" && sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "4F")
                {
                    string v_hscode; string v_hsdesc; string v_icoded; double v_hsrate;
                    v_hscode = "";
                    v_icoded = "";
                    sagm_4f_hs = "";

                    dt4 = new DataTable();
                    dt4 = fgen.getdata(frm_qstr, frm_cocd, "select trim(b.hscode) as hscode,a.icode as item,tot from (select trim(a.icode) as icode,sum(a.iamount) as tot from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and a.vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and to_Char(a.vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "' and substr(trim(a.icode),1,2)!='59' group by trim(a.icode) ) a,item b where trim(a.icode)=trim(b.icode)  order by tot desc");

                    v_hscode = dt4.Rows[0]["hscode"].ToString();
                    v_icoded = dt4.Rows[0]["item"].ToString();

                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, "select trim(name) as aa,trim(num6) as ab from typegrp where id='T1' and trim(acref)='" + v_hscode + "'");

                    v_hsdesc = dt3.Rows[0]["aa"].ToString();
                    v_hsrate = fgen.make_double(dt3.Rows[0]["ab"].ToString());

                    catcode = "select iexc_Addl,chgd_pos,'" + v_hscode + "' as hs_code,1 as item_no,'" + v_hsdesc + "' as prod_desc,max(quan_unit) as quan_unit,1 as morder," + v_hsrate + " as igst_rt,0 as cgst_rt, 0 as sgst_rt,GSTIN,sup_type,sub_type,doc_type,doc_no,doc_Dt,sup_gst,sup_nam,sup_add1,sup_add2,sup_add3,sup_state,sup_pin,rec_gst,rec_nam, rec_add1, rec_add2,rec_add3,rec_add4,rec_add5,rec_state,rec_pin,tran_mode,tran_ID,tran_name,tran_dist,tran_doc,tran_dt,vehi_no,round(sum(iamount)/sum(Quantity),4) as irate,sum(iamount) as iamount,sum(totdisc_amt) as totdisc_amt,sum(ichgs) as ichgs,sum(Quantity) as Quantity,sum(amt_sttt) as amt_sttt,sum(iamount) as taxb_val,sum(ioth_chg) as ioth_chg,sum(sgst_val) as sgst_val,sum(cgst_val) as cgst_val, sum(igst_val) as igst_val,sum(oth_chgs) as oth_chgs,MAX(cess_rt) as cess_rt,sum(cess_val) as cess_val,ewb_user,ewb_pwd,sum(billstot) as billstot,sum(billgtot) as billgtot,sum(ro_off) as ro_off,my_Cscode,desp_from1,rec_phone,rec_email ,iopr,sum(amt_exc) as amt_exc,sum(rvalue) as rvalue,fdue,ins_co,pay_num,sum(amt_Sale) as amt_Sale,naration,district,max(insp_amt) as insp_amt,sum(tcsamt) as tcsamt,sum(amt_extexc) as amt_extexc,buyer,shp_bill,shp_billdt,sum(tot_othchg) as tot_othchg from  (" + catcode + ") GROUP BY GSTIN,sup_type,sub_type,doc_type,doc_no,doc_Dt,sup_gst,sup_nam,sup_add1, sup_add2,sup_add3,sup_state,sup_pin,rec_gst,rec_nam, rec_add1, rec_add2,rec_add3,rec_add4,rec_add5,rec_state,rec_pin,tran_mode,tran_ID,tran_name,tran_dist,tran_doc,tran_dt,vehi_no,ewb_user,ewb_pwd,my_Cscode,desp_from1,rec_phone,rec_email ,iopr,fdue,ins_co,pay_num,naration,district,buyer,shp_bill,shp_billdt";

                }


                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, catcode);

                catcode = "";
                if (dt.Rows.Count <= 0)
                {
                    fgen.msg("", "ASMG", "Please Check Data Linkage , Data is not OK(might be some single or double quote in some data field)");
                    return;
                }

                else
                {
                    string cs_st_cd, ds_st_cd;
                    string cs_add1, cs_add2, cs_add3, cs_pinc;
                    string ds_add1, ds_add2, ds_add3, ds_add4, ds_pinc, ds_gst, ds_aname;
                    TOT_INV = TOT_INV + 1;
                    int d = 0;

                    if (FOPT == "JSON")
                    {

                        dt5 = new DataTable();
                        dt5 = fgen.getdata(frm_qstr, frm_cocd, "select gst_no,cstaffcd,addr1,addr2,addr3,pincode from csmst where trim(acode)='" + dt.Rows[0]["my_Cscode"].ToString().Trim() + "'");

                        if (mygstno.Length == 15)
                            cs_st_cd = dt5.Rows[0]["gst_no"].ToString().Substring(0, 2);
                        else
                            cs_st_cd = dt5.Rows[0]["cstaffcd"].ToString().Trim();
                        if (cs_st_cd.Length <= 1)
                        {
                            cs_st_cd = dt.Rows[0]["rec_State"].ToString().Trim();
                        }
                        else
                        {
                            tran_type = "SHP";
                        }

                        cs_add1 = dt.Rows[0]["rec_add1"].ToString().Trim();
                        cs_add2 = dt.Rows[0]["rec_add2"].ToString().Trim();
                        cs_add3 = dt.Rows[0]["rec_add3"].ToString().Trim();
                        cs_pinc = dt.Rows[0]["pincode"].ToString().Trim();

                        if (dt.Rows[0]["my_Cscode"].ToString().Length >= 6)
                        {
                            cs_add1 = dt5.Rows[0]["addr1"].ToString().Trim();
                            cs_add2 = dt5.Rows[0]["addr2"].ToString().Trim();
                            cs_add3 = dt5.Rows[0]["addr3"].ToString().Trim();
                            cs_pinc = dt5.Rows[0]["addr1"].ToString().Trim();
                        }

                        if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "4F" || sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "42" || sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "4E")
                        {
                            AA = "{'genMode':'','userGstin': '" + mygstno + "','SupplyType':'" + dt.Rows[0]["sup_type"].ToString() + "','subSupplyType': '" + dt.Rows[0]["sub_type"].ToString() + "','DocType':'" + dt.Rows[0]["doc_type"].ToString() + "','DocNo': '" + dt.Rows[0]["doc_no"].ToString() + "','DocDate': '" + dt.Rows[0]["doc_dt"].ToString() + "','fromGstin':'" + dt.Rows[0]["sup_gst"].ToString() + "','fromTrdName':'" + dt.Rows[0]["sup_nam"].ToString() + "','fromAddr1': '" + dt.Rows[0]["sup_add1"].ToString() + "','fromAddr2':'" + dt.Rows[0]["sup_add2"].ToString() + "','fromPlace': '" + dt.Rows[0]["sup_add3"].ToString() + "','fromPincode':'" + dt.Rows[0]["sup_pin"].ToString() + "','fromStateCode':  '" + dt.Rows[0]["sup_state"].ToString() + "','actualFromStateCode':'" + dt.Rows[0]["sup_state"].ToString() + "','toGstin': '" + dt.Rows[0]["rec_gst"].ToString() + "','toTrdname': '" + dt.Rows[0]["rec_nam"].ToString() + "','toAddr1': '" + cs_add1 + "','toAddr2': '" + cs_add2 + "','toPlace': '" + cs_add3 + "','toPincode': '" + cs_pinc + "','toStateCode': '" + "96" + "','actualToStateCode': '" + dt.Rows[0]["rec_state"].ToString() + "','totalValue': " + dt.Rows[0]["billstot"].ToString() + ",'cgstValue': " + dt.Rows[0]["cgst_val"].ToString() + ",'sgstValue': " + dt.Rows[0]["sgst_val"].ToString() + ",'igstValue': " + dt.Rows[0]["igst_val"].ToString() + ",'cessValue': " + dt.Rows[0]["cess_val"].ToString() + ",";
                        }
                        else
                        {
                            AA = "{'genMode':'','userGstin': '" + mygstno + "','SupplyType':'" + dt.Rows[0]["sup_type"].ToString() + "','subSupplyType': '" + dt.Rows[0]["sub_type"].ToString() + "','DocType':'" + dt.Rows[0]["doc_type"].ToString() + "','DocNo': '" + dt.Rows[0]["doc_no"].ToString() + "','DocDate': '" + dt.Rows[0]["doc_dt"].ToString() + "','fromGstin':'" + dt.Rows[0]["sup_gst"].ToString() + "','fromTrdName':'" + dt.Rows[0]["sup_nam"].ToString() + "','fromAddr1': '" + dt.Rows[0]["sup_add1"].ToString() + "','fromAddr2':'" + dt.Rows[0]["sup_add2"].ToString() + "','fromPlace': '" + dt.Rows[0]["sup_add3"].ToString() + "','fromPincode':'" + dt.Rows[0]["sup_pin"].ToString() + "','fromStateCode':  '" + dt.Rows[0]["sup_state"].ToString() + "','actualFromStateCode':'" + dt.Rows[0]["sup_state"].ToString() + "','toGstin': '" + dt.Rows[0]["rec_gst"].ToString() + "','toTrdname': '" + dt.Rows[0]["rec_nam"].ToString() + "','toAddr1': '" + cs_add1 + "','toAddr2': '" + cs_add2 + "','toPlace': '" + cs_add3 + "','toPincode': '" + cs_pinc + "','toStateCode': '" + dt.Rows[0]["rec_state"].ToString() + "','actualToStateCode': '" + cs_st_cd + "','totalValue': " + dt.Rows[0]["billstot"].ToString() + ",'cgstValue': " + dt.Rows[0]["cgst_val"].ToString() + ",'sgstValue': " + dt.Rows[0]["sgst_val"].ToString() + ",'igstValue': " + dt.Rows[0]["igst_val"].ToString() + ",'cessValue': " + dt.Rows[0]["cess_val"].ToString() + ",";
                        }

                        if (frm_cocd == "MCPL" || frm_cocd == "MIRP")
                        {
                            BB = "'TotNonAdvolVal':0,'OthValue':" + (1 * fgen.make_double(dt.Rows[0]["oth_chgs"].ToString())) + ",'totInvValue': " + dt.Rows[0]["billgtot"].ToString() + ",'transMode': '" + dt.Rows[0]["tran_mode"].ToString() + "','transDistance': " + dt.Rows[0]["tran_dist"].ToString() + ",'transporterName': '" + dt.Rows[0]["tran_name"].ToString() + "','transporterId': '" + dt.Rows[0]["tran_id"].ToString() + "','transDocNo': '" + dt.Rows[0]["tran_doc"].ToString() + "','transDocDate': '" + dt.Rows[0]["tran_dt"].ToString() + "','vehicleNo': '" + dt.Rows[0]["vehi_no"].ToString() + "','vehicleType': 'R','mainHsnCode': " + dt.Rows[0]["hs_code"].ToString() + ",'itemList':[";
                        }
                        else
                        {
                            BB = "'TotNonAdvolVal':0,'OthValue':" + (-1 * fgen.make_double(dt.Rows[0]["oth_chgs"].ToString())) + ",'totInvValue': " + dt.Rows[0]["billgtot"].ToString() + ",'transMode': '" + dt.Rows[0]["tran_mode"].ToString() + "','transDistance': " + dt.Rows[0]["tran_dist"].ToString() + ",'transporterName': '" + dt.Rows[0]["tran_name"].ToString() + "','transporterId': '" + dt.Rows[0]["tran_id"].ToString() + "','transDocNo': '" + dt.Rows[0]["tran_doc"].ToString() + "','transDocDate': '" + dt.Rows[0]["tran_dt"].ToString() + "','vehicleNo': '" + dt.Rows[0]["vehi_no"].ToString() + "','vehicleType': 'R','mainHsnCode': " + dt.Rows[0]["hs_code"].ToString() + ",'itemList':[";
                        }


                        cc = "";
                        int itm_cnt; string vprod_name;
                        itm_cnt = 1;
                        // AA = "{'userGstin': '" + mygstno + "','SupplyType': '" + dt.Rows[0]["sup_type"].ToString() + "','subSupplyType': '" + dt.Rows[0]["sub_type"].ToString() + "','DocType': '" + dt.Rows[0]["doc_type"].ToString() + "','DocNo': '" + dt.Rows[0]["doc_no"].ToString() + "','DocDate': '" + dt.Rows[0]["doc_dt"].ToString() + "','transType': '" + tran_type + "','fromGstin': '" + dt.Rows[0]["sup_gst"].ToString() + "','fromTrdName': '" + dt.Rows[0]["sup_nam"].ToString() + "','fromAddr1': '" + dt.Rows[0]["sup_add1"].ToString() + "','fromAddr2': '" + dt.Rows[0]["sup_add2"].ToString() + "','fromPlace': '" + dt.Rows[0]["sup_add3"].ToString() + "','fromPincode': '" + dt.Rows[0]["sup_pin"].ToString() + "','fromStateCode': '" + dt.Rows[0]["sup_state"].ToString() + "','actualFromStateCode': '" + dt.Rows[0]["sup_state"].ToString() + "', 'toGstin': '" + dt.Rows[0]["rec_gst"].ToString() + "','toTrdname': '" + dt.Rows[0]["rec_nam"].ToString() + "','toAddr1': '" + dt.Rows[0]["rec_add1"].ToString() + "','toAddr2': '" + dt.Rows[0]["rec_add2"].ToString() + "','toPlace': '" + dt.Rows[0]["rec_add3"].ToString() + "','toPincode': '" + dt.Rows[0]["rec_pin"].ToString() + "','toStateCode': '" + dt.Rows[0]["rec_state"].ToString() + "','actualToStateCode': '" + dt.Rows[0]["rec_state"].ToString() + "','totalValue': " + dt.Rows[0]["billstot"].ToString() + ",'cgstValue': " + dt.Rows[0]["cgst_val"].ToString() + ",'sgstValue': " + dt.Rows[0]["sgst_val"].ToString() + ",'igstValue': " + dt.Rows[0]["igst_val"].ToString() + ",'cessValue': " + dt.Rows[0]["cess_val"].ToString() + ",";                    
                        // BB = "'TotNonAdvolVal':0,'OthValue':" + ( (-1) * Convert.ToInt32(( dt.Rows[0]["oth_chgs"].ToString()))) + ",'totInvValue': " + dt.Rows[0]["billgtot"].ToString().Trim() + ",'transMode': '" + dt.Rows[0]["tran_mode"].ToString() + "','transDistance': " + dt.Rows[0]["tran_dist"].ToString() + ",'transporterName': '" + dt.Rows[0]["tran_name"].ToString() + "','transporterId': '" + dt.Rows[0]["tran_id"].ToString() + "','transDocNo': '" + dt.Rows[0]["tran_doc"].ToString() + "','transDocDate': '" + dt.Rows[0]["tran_dt"].ToString() + "','vehicleNo': '" + dt.Rows[0]["vehi_no"].ToString() + "','vehicleType': 'R','totInvValue': " + dt.Rows[0]["billstot"].ToString() + ",'mainHsnCode': " + dt.Rows[0]["hs_code"].ToString() + ",'itemList':[";
                        do
                        {
                            vprod_name = fgen.seek_iname(frm_qstr, frm_cocd, "select substr(trim(name),1,100) as aa from typegrp where id='T1' and trim(acref)='" + dt.Rows[d]["hscode"].ToString() + "'", "aa");
                            dd = "{'ItemNo': " + dt.Rows[d]["item_no"].ToString() + ",'productName': '" + vprod_name + "','productDesc': '" + dt.Rows[d]["prod_desc"].ToString() + "','hsnCode':" + dt.Rows[d]["hs_code"].ToString() + ",'quantity': " + dt.Rows[d]["quantity"].ToString() + ",'qtyUnit': '" + dt.Rows[d]["quan_unit"].ToString() + "','taxableAmount': " + dt.Rows[d]["taxb_val"].ToString() + ",'sgstRate': " + dt.Rows[d]["sgst_rt"].ToString() + ",'cgstRate': " + dt.Rows[d]["cgst_rt"].ToString() + ",'igstRate': " + dt.Rows[d]["igst_rt"].ToString() + ",'cessRate': " + dt.Rows[d]["cess_rt"].ToString() + "  }";

                            if (dt.Rows.Count == 1)
                            {
                                cc = cc + dd;
                            }

                            else
                                cc = cc + dd + ",";

                            d++;
                        } while (d < dt.Rows.Count);


                        cc = AA + BB + cc + "]},";
                        cc = cc.Replace("},]", "}]");
                        // cc = cc.Replace("'", "\"");
                        Edesc[i] = cc;
                    }
                    else
                    {

                        string einfst1, einfst2, einfst3, einfst4, einfst5, einfst6, einfst7, einfst8, einfst9;
                        string einf1, einf2, einf3, einf4, einf5, einf6, einf7, einf8, einf9, einf10;
                        string einf11, einf12, einf13, einf14, einf15, einf16, einf17, einf18, einf19, einf20;
                        string einf21, einf22, einf23, einf24, einf25, einf26, einf27, einf28, einf29, einf30;
                        string einf31, einf32, einf33, einf34, einf35, einf36, einf37, einf38 = "", einf39, einf40;
                        string einf41, einf42, einf46, einf43, einf44, einf45, einf47, einf48, einf49, einf50;
                        string einf51, einf52, einf53 = "", einf54 = "", einf55 = "", einf56, einf57, einf58, einf59, einf60;
                        string einf61, einf62, einf63, einf64, einf65, einf66, einf67, einf68, einf69, einf70;
                        string einf71, einf72, einf73, einf74, einf75, einf76, einf77, einf78, einf79, einf80;
                        string einf81, einf82, einf83, einf84, einf85, einf86, einf87, einf88, einf89;
                        string einf90, einf91, einf92, einf93, einf94, einf95, einf96, einf97, einf98, einf99;
                        string einf100, einf101, einf102, einf103, einf104, einf105, einf106, einf107, einf108, einf109;
                        string einf110, einf111, einf112, einf113, einf114, einf115, einf116, einf117 = "", einf118, einf119, einf120, einf121, einf122, exp_consg;
                        string v_stype, v_unit;

                        v_stype = "";
                        v_unit = "";

                        SQuery = "Select Name,type1,trim(exc_apply) as Supply_type from type where id='V' and type1 like '4%' and nvl(exc_apply,'-')!='-' order by type1";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        //webtel wala shuru hota hai
                        //cc = "{ 'Push_Data_List': ["
                        cc = "";
                        int RUN_CNTR = 1;
                        einfst1 = "{'Push_Data_List': {'Data': [";
                        int d1 = 0;
                        //  rs1.MoveFirst
                        do
                        {
                            v_stype = "XXX";
                            if (dt2.Rows.Count <= 0)
                            {
                                fgen.msg("-", "AMSG", "Please map the Supply types as per GST in Accounts Voucher Type Master.'13' IRN Generation not possible.");
                                return;
                            }
                            else
                            {
                                v_stype = fgen.seek_iname_dt(dt2, "type1='" + sg1.Rows[i].Cells[16].Text.Trim() + "'", "Supply_type");
                            }
                            switch (v_stype)
                            {
                                case "4":
                                    if (Convert.ToDouble(dt.Rows[d1]["igst_val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["sgst_val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["cgst_val"].ToString()) > 0)
                                    {
                                        einf1 = "EXPWP";
                                    }
                                    else
                                    {
                                        einf1 = "EXPWOP";
                                    }
                                    break;
                                case "6":
                                    if (Convert.ToDouble(dt.Rows[d1]["igst_val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["sgst_val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["cgst_val"].ToString()) > 0)
                                    {
                                        einf1 = "SEZWP";
                                    }
                                    else
                                    {
                                        einf1 = "SEZWOP";
                                    }
                                    break;
                                default:
                                    einf1 = "B2B";
                                    break;

                            }
                            //   for sez parties.
                            if (einf1 == "B2B" && ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() != "N/A")
                            {
                                einf1 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(bank_acno)) as aa from famst where trim(acode)='" + dt.Rows[0]["my_Cscode"].ToString().Trim() + "'", "aa");
                                if (einf1 == "SEZ")
                                {
                                    if (Convert.ToDouble(dt.Rows[d1]["igst_val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["sgst_val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["cgst_val"].ToString()) > 0)
                                    {
                                        einf1 = "SEZWP";
                                    }
                                    else
                                    {
                                        einf1 = "SEZWOP";
                                    }
                                    v_stype = "9"; //for identifying dr/cr for sez party
                                }
                                else einf1 = "B2B";
                            }
                            //for checking export parties in 58,59
                            if ((sg1.Rows[i].Cells[16].Text.Trim() == "58" || sg1.Rows[i].Cells[16].Text.Trim() == "59") && (sg1.Rows[i].Cells[20].Text.Trim() == "N/A" || sg1.Rows[i].Cells[20].Text.Trim() == "URP"))
                            {
                                einf1 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(gstoversea)) as aa from famst where trim(acode)='" + dt.Rows[0]["my_Cscode"].ToString().Trim() + "'", "aa");
                                if (einf1 == "Y")
                                {
                                    if (Convert.ToDouble(dt.Rows[d1]["igst_val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["sgst_val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["cgst_val"].ToString()) > 0)
                                    {
                                        einf1 = "EXPWP";
                                    }
                                    else
                                    {
                                        einf1 = "EXPWOP";
                                    }
                                    v_stype = "10"; //for identifying dr/cr for sez party
                                }
                                else einf1 = "B2B";
                            }

                            if (frm_cocd == "DREM" && sg1.Rows[i].Cells[16].Text.Trim() == "4S" && ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() == "N/A") einf1 = "EXPWP";
                            einf2 = "N";
                            einf3 = "REG";
                            einf4 = "N";
                            einf5 = dt.Rows[d1]["doc_type"].ToString().Trim();
                            einf6 = dt.Rows[d1]["Doc_No"].ToString().Trim();
                            einf7 = Convert.ToDateTime(dt.Rows[d1]["doc_Dt"].ToString()).ToShortDateString();
                            einf8 = dt.Rows[d1]["Gstin"].ToString().Trim();
                            einf9 = gst_name;
                            einf10 = coaddr1;

                            einf11 = coaddr2;
                            einf12 = dt.Rows[d1]["Gstin"].ToString().Substring(0, 2).Trim();
                            einf13 = coaddr3;
                            einf15 = txtlbl7.Text.Trim();
                            einf16 = dt.Rows[d1]["sup_state"].ToString().Trim();
                            einf17 = cotel.ToString().Left(10);
                            einf18 = coemail;
                            if (dt.Rows[d1]["rec_gst"].ToString().Length == 15) einf19 = dt.Rows[d1]["rec_gst"].ToString().Trim();
                            else einf19 = "URP";
                            if (einf19 == "URP" && (v_stype.Trim() != "4" && v_stype != "10"))
                            {
                                fgen.msg("-", "AMSG", " B2C sale in " + dt.Rows[d1]["Doc_no"].ToString() + ". IRN Generation not possible.");
                                return;
                            }
                            einf20 = dt.Rows[d1]["rec_nam"].ToString().Trim();
                            einf21 = dt.Rows[d1]["rec_add1"].ToString().Trim().ToUpper();
                            einf22 = dt.Rows[d1]["rec_add2"].ToString().Trim().ToUpper();
                            if (dt.Rows[d1]["rec_add3"].ToString().Trim() == "-") einf23 = dt.Rows[d1]["rec_add5"].ToString().Left(100).Trim();
                            else einf23 = dt.Rows[d1]["rec_add3"].ToString().Left(100).Trim();//IIf(Trim(checknullc(rs1!)) = "-", Left(Trim(rs1!rec_add5), 100), Left(Trim(rs1!rec_add3), 100))
                            if (einf23.Length < 3)
                            {
                                fgen.msg("-", "AMSG", " Please correct location(can be between 3-100 characters) for " + dt.Rows[d1]["rec_nam"].ToString() + ".IRN Generation not possible.");
                                return;
                            }
                            if (v_stype == "4" || v_stype == "10") einf24 = "96";
                            else einf24 = dt.Rows[d1]["rec_State"].ToString().Trim();
                            if (v_stype == "4" || v_stype == "10") einf26 = "999999";
                            else einf26 = dt.Rows[d1]["rec_pin"].ToString().Trim();
                            if (v_stype == "4" || v_stype == "10") einf27 = "96";
                            else einf27 = dt.Rows[d1]["rec_State"].ToString().Trim();
                            if (dt.Rows[d1]["rec_phone"].ToString().Trim().Length >= 12) einf28 = dt.Rows[d1]["rec_phone"].ToString().Left(12).Trim();
                            else einf28 = dt.Rows[d1]["rec_phone"].ToString().Trim();
                            if (dt.Rows[d1]["rec_phone"].ToString() == "-") einf28 = "^";
                            if (einf28.Length < 10 && einf28.Length > 12)
                            {
                                fgen.msg("-", "AMSG", " Please correct buyer telephone no for " + dt.Rows[d1]["rec_nam"].ToString() + "IRN Generation not possible.");
                                return;
                            }
                            if (dt.Rows[d1]["rec_email"].ToString().Trim().Length <= 1 || dt.Rows[d1]["rec_email"].ToString().Trim() == "-")
                            {
                                einf29 = "^";
                            }
                            else
                            {
                                einf29 = dt.Rows[d1]["rec_email"].ToString().Trim();
                            }

                            einf30 = RUN_CNTR.ToString();
                            RUN_CNTR = RUN_CNTR + 1;
                            einf31 = dt.Rows[d1]["prod_desc"].ToString().Trim();
                            einf32 = dt.Rows[d1]["hs_code"].ToString().Trim();
                            einf33 = "^";
                            if (dt.Rows[d1]["hs_code"].ToString().Trim().Left(2) == "99") einf25 = "Y"; //service hsn
                            else einf25 = "N";
                            einf34 = (Math.Round(fgen.make_double(dt.Rows[d1]["quantity"].ToString().Trim()), 2)).ToString();
                            einf35 = "0";
                            if (send_unitmaster == "Y")
                            {
                                v_unit = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(nvl(exc_tarrif,'-')) as aa from type where id='U' and trim(name)='" + dt.Rows[d1]["quan_unit"].ToString().Trim() + "'", "aa");
                                if (v_unit == "-") einf36 = dt.Rows[d1]["quan_unit"].ToString().Trim();
                                else einf36 = v_unit;
                            }
                            else
                            { einf36 = dt.Rows[d1]["quan_unit"].ToString(); }
                            einf37 = (Math.Round(fgen.make_double(dt.Rows[d1]["irate"].ToString()), 3)).ToString();
                            einf41 = (Math.Round(fgen.make_double(dt.Rows[d1]["taxb_val"].ToString()), 2)).ToString(); //Item_AssAmt
                            einf40 = (Math.Round(fgen.make_double(dt.Rows[d1]["ioth_chg"].ToString()), 2)).ToString(); //Item_OthChrg
                            einf48 = Math.Round(Convert.ToDouble(dt.Rows[d1]["taxb_val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["cgst_Val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["sgst_val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["igst_val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["ioth_chg"].ToString()), 2).ToString();
                            einf39 = Math.Round((Convert.ToDouble(dt.Rows[d1]["quantity"].ToString()) * Convert.ToDouble(dt.Rows[d1]["irate"].ToString())) * (Convert.ToDouble(dt.Rows[d1]["ichgs"].ToString()) / 100), 2).ToString(); //Item_Discount
                            //einf38 = (Convert.ToDouble(dt.Rows[d1]["taxb_val"].ToString() + (Convert.ToDouble(dt.Rows[d1]["quantity"].ToString()) * Convert.ToDouble(dt.Rows[d1]["irate"].ToString())) * (Convert.ToDouble(dt.Rows[d1]["ichgs"].ToString()) / 100))).ToString(); //Item_TotAmt
                            if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4")
                            {
                                if (Math.Round(fgen.make_double(dt.Rows[d1]["iamount"].ToString()) * ((fgen.make_double(dt.Rows[d1]["cgst_Rt"].ToString()) + fgen.make_double(dt.Rows[d1]["sgst_rt"].ToString()) + fgen.make_double(dt.Rows[d1]["igst_Rt"].ToString())) / 100), 2) != Math.Round((fgen.make_double(dt.Rows[d1]["igst_val"].ToString()) + fgen.make_double(dt.Rows[d1]["cgst_Val"].ToString()) + fgen.make_double(dt.Rows[d1]["sgst_val"].ToString())), 2))
                                //If Abs(Round(RS1!iamount * (RS1!cgst_Rt / 100), 2) + Round(RS1!iamount * (RS1!igst_Rt / 100), 2) + Round(RS1!iamount * (RS1!sgst_rt / 100), 2) - (Round((RS1!igst_val + RS1!cgst_Val + RS1!sgst_val), 2))) > 0.01 Then
                                {
                                    einf41 = (Math.Round(fgen.make_double(dt.Rows[d1]["taxb_val"].ToString()), 2)).ToString();
                                }
                                else
                                {
                                    einf41 = (Math.Round(fgen.make_double(dt.Rows[d1]["iamount"].ToString()), 2)).ToString();
                                }

                                einf48 = (Math.Round(fgen.make_double(dt.Rows[d1]["cgst_Val"].ToString()) + fgen.make_double(dt.Rows[d1]["sgst_val"].ToString()) + fgen.make_double(dt.Rows[d1]["igst_val"].ToString()) + fgen.make_double(dt.Rows[d1]["ioth_chg"].ToString()), 2) + Math.Round(fgen.make_double(einf41), 2)).ToString();//Item_TotItemVal
                            }

                            if (frm_cocd == "AMAR" && sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4")
                            { //for item adding other charges withour seperate row bot no tax on additional cost
                                einf40 = "^";
                                einf48 = (Math.Round(fgen.make_double(dt.Rows[d1]["iamount"].ToString()) + Convert.ToDouble(dt.Rows[d1]["cgst_Val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["sgst_val"].ToString()) + Convert.ToDouble(dt.Rows[d1]["igst_val"].ToString()), 2)).ToString();//Item_TotItemValRound(rs1!iamount + rs1!cgst_Val + rs1!sgst_val + rs1!igst_val, 2) 'Item_TotItemVal including taxes
                            }
                            einf14 = "^";
                            einf38 = (Math.Round(fgen.make_double((((fgen.make_double(einf41)) + (fgen.make_double(einf39))) - fgen.make_double(einf40)).ToString()), 2)).ToString();

                            einf42 = Math.Round((fgen.make_double(dt.Rows[d1]["cgst_Rt"].ToString()) + fgen.make_double(dt.Rows[d1]["sgst_rt"].ToString()) + fgen.make_double(dt.Rows[d1]["igst_Rt"].ToString())), 2).ToString();
                            if (einf1.Trim() == "SEZWOP" || einf1.Trim() == "EXPWOP") einf42 = "0";

                            einf43 = Math.Round(fgen.make_double(dt.Rows[d1]["igst_val"].ToString().Trim()), 2).ToString();
                            einf44 = Math.Round(fgen.make_double(dt.Rows[d1]["cgst_Val"].ToString().Trim()), 2).ToString();
                            einf45 = Math.Round(fgen.make_double(dt.Rows[d1]["sgst_val"].ToString().Trim()), 2).ToString();
                            einf46 = "0";
                            einf47 = "0";
                            einf49 = "^";
                            einf50 = "^";
                            einf51 = "^";
                            if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "5")
                            {
                                einf52 = Math.Round(fgen.make_double(dt.Rows[d1]["AMT_EXTEXC"].ToString()) - (fgen.make_double(dt.Rows[d1]["amt_Exc"].ToString()) + fgen.make_double(dt.Rows[d1]["rvalue"].ToString())), 2).ToString();
                            }
                            else
                            {
                                einf52 = Math.Round((fgen.make_double(dt.Rows[d1]["AMT_SALE"].ToString()) + fgen.make_double(dt.Rows[d1]["AMT_STTT"].ToString()) + fgen.make_double(dt.Rows[d1]["AMT_EXTEXC"].ToString())), 2).ToString();
                                if (frm_cocd == "ROYL") { einf52 = Math.Round((fgen.make_double(dt.Rows[d1]["AMT_SALE"].ToString()) + fgen.make_double(dt.Rows[d1]["AMT_STTT"].ToString())), 2).ToString(); }

                            }
                            if (dt.Rows[d1]["iopr"].ToString().Trim() == "CG") einf53 = (Math.Round(fgen.make_double(dt.Rows[d1]["amt_Exc"].ToString()), 2)).ToString(); else { einf53 = "0"; }
                            if (dt.Rows[d1]["iopr"].ToString().Trim() == "CG") einf54 = (Math.Round(fgen.make_double(dt.Rows[d1]["rvalue"].ToString()), 2)).ToString(); else { einf54 = "0"; }
                            if (dt.Rows[d1]["iopr"].ToString().Trim() == "IG") einf55 = (Math.Round(fgen.make_double(dt.Rows[d1]["amt_Exc"].ToString()), 2)).ToString(); else { einf55 = "0"; }
                            einf56 = "0";
                            einf57 = "0";
                            einf58 = "0";
                            if (dt.Rows[d1]["ro_off"].ToString() == "")
                                einf59 = "0";
                            else
                                einf59 = dt.Rows[d1]["ro_off"].ToString();
                            einf60 = dt.Rows[d1]["INSP_AMT"].ToString();
                            //if(sg1.Rows[i].Cells[16].Text.Trim().Substring(0,2) == "4S"){
                            //    einf61 = (Convert.ToDouble(dt.Rows[d1]["AMT_EXTEXC"].ToString()) +  Convert.ToDouble(dt.Rows[d1]["rvalue"].ToString())).ToString();
                            //}
                            //else{
                            einf61 = Math.Round(fgen.make_double(dt.Rows[d1]["billgtot"].ToString()), 2).ToString();
                            //}

                            einf62 = "^";
                            einf63 = "^";
                            einf64 = "^";
                            einf65 = "^";
                            einf66 = "^";
                            einf67 = "^";
                            einf68 = "^";
                            einf69 = "^";
                            einf70 = "^";
                            einf71 = "^";

                            string cs_gstin, cs_name, drcrno;
                            if (sg1.Rows[i].Cells[16].Text.Trim() == "58" || sg1.Rows[i].Cells[16].Text.Trim() == "59")
                            {
                                if (dt.Rows[d1]["invno"].ToString() == "-")
                                {
                                    einf72 = "^";
                                    einf74 = "^";
                                }
                                else
                                {
                                    einf72 = dt.Rows[d1]["invno"].ToString().Trim();
                                    einf74 = Convert.ToDateTime(dt.Rows[d1]["invdate"].ToString()).ToShortDateString();
                                }

                            }
                            else
                            {
                                einf72 = "^";
                                einf74 = "^";
                            }


                            einf73 = "^";
                            einf75 = "^";
                            einf76 = "^";
                            einf77 = "^";
                            einf78 = "^";
                            einf79 = "^";
                            einf80 = "^";
                            einf81 = "^";
                            einf82 = "^";
                            einf83 = "^";

                            cs_add1 = "^";
                            cs_add2 = "^";
                            cs_add3 = "^";
                            cs_pinc = "^";
                            cs_gstin = "^";
                            cs_name = "^";
                            cs_st_cd = "^";
                            if (dt.Rows[d1]["my_Cscode"].ToString().Trim().Length >= 6)
                            {
                                dt4 = new DataTable();
                                dt4 = fgen.getdata(frm_qstr, frm_cocd, "select nvl(cstaffcd,'-') as cstaffcd,nvl(addr1,'-') as addr1,nvl(addr2,'-') as addr2,nvl(addr3,'-') as addr3,nvl(pincode,'-') as pincode,nvl(gst_no,'-') as gst_no,trim(aname) as aname from csmst where trim(acode)='" + dt.Rows[0]["my_Cscode"].ToString().Trim() + "'");
                                if (dt4.Rows.Count > 0)
                                {
                                    if (dt4.Rows[0]["gst_no"].ToString().Length == 15)
                                        cs_st_cd = dt4.Rows[0]["gst_no"].ToString().Substring(0, 2);
                                    else
                                        cs_st_cd = dt4.Rows[0]["cstaffcd"].ToString().Substring(0, 2);
                                }

                                if (cs_st_cd == "" || cs_st_cd.Length != 2)
                                {
                                    fgen.msg("-", "AMSG", "Consignee state not linked, IRN will not be generated.");
                                    return;
                                }
                                if (dt4.Rows.Count > 0)
                                {
                                    cs_add1 = dt4.Rows[0]["addr1"].ToString().Left(100);
                                    cs_add2 = dt4.Rows[0]["addr2"].ToString().Left(100);
                                    cs_add3 = dt4.Rows[0]["addr3"].ToString().Left(50);
                                    cs_pinc = dt4.Rows[0]["pincode"].ToString();
                                    if (dt4.Rows[0]["gst_no"].ToString().Length != 15) cs_gstin = "URP"; else cs_gstin = dt4.Rows[0]["gst_no"].ToString();
                                    cs_name = dt4.Rows[0]["aname"].ToString();
                                }

                                einf3 = "SHP";
                                if ((v_stype == "4" || v_stype == "10") && cs_st_cd == "99")
                                {
                                    cs_st_cd = "96";
                                    cs_pinc = "999999";
                                }
                            }
                            if (v_stype == "4" && make_ewayb == "Y")
                            {
                                dt4 = new DataTable();
                                dt4 = fgen.getdata(frm_qstr, frm_cocd, "select trim(acref4) as stcd,trim(acref) as acref,trim(acref2) as acref2,trim(acref3) as acref3,trim(num6) as num6,trim(name) as name from typegrp where id='^M' and upper(trim(name))='" + dt.Rows[0]["my_Cscode"].ToString().Trim() + "'");
                                if (dt4.Rows.Count > 0)
                                {
                                    exp_consg = dt4.Rows[0]["stcd"].ToString().Trim();
                                    cs_add1 = dt4.Rows[0]["acref"].ToString().Trim();
                                    cs_add2 = dt4.Rows[0]["acref2"].ToString().Trim();
                                    cs_add3 = dt4.Rows[0]["acref3"].ToString().Trim();
                                    cs_pinc = dt4.Rows[0]["num6"].ToString().Trim();
                                    cs_name = dt4.Rows[0]["name"].ToString().Trim();
                                    cs_st_cd = dt4.Rows[0]["stcd"].ToString().Trim();
                                    if (dt.Rows[0]["rec_add5"].ToString() == "-") einf23 = dt.Rows[0]["rec_add5"].ToString();
                                }
                                cs_gstin = "URP";
                            }
                            einf84 = cs_gstin;
                            einf85 = cs_name;
                            einf86 = cs_add3;
                            einf87 = cs_st_cd;
                            einf88 = cs_pinc;
                            einf89 = cs_add1;
                            einf90 = cs_add2;
                            einf93 = "^";
                            einf94 = "^";

                            einf95 = "^";
                            einf96 = "^";
                            einf97 = "^";
                            einf98 = "^";

                            einf99 = "^";
                            einf100 = "^";
                            einf101 = "^";
                            einf102 = "^";
                            einf103 = "^";
                            einf104 = "^";
                            einf105 = "^";
                            einf106 = "^";

                            einf107 = "^";
                            einf108 = "^";
                            einf109 = "^";
                            einf110 = "^";
                            einf111 = "^";
                            if (v_stype == "4")
                            {
                                if (dt.Rows[0]["shp_bill"].ToString().Trim().Length < 4)
                                {
                                    einf112 = "^";
                                    einf113 = "^";
                                }
                                else
                                {
                                    einf112 = dt.Rows[0]["shp_bill"].ToString().Trim();
                                    einf113 = dt.Rows[0]["shp_billdt"].ToString().Trim();
                                }
                                einf114 = dt.Rows[0]["BUYER"].ToString().Trim();
                            }
                            else
                            {
                                einf112 = "^";
                                einf113 = "^";
                                einf114 = "^";
                            }


                            einf115 = "^";
                            if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "5")
                            {
                                einf116 = Math.Round(Convert.ToDouble(dt.Rows[0]["iexc_Addl"]), 2).ToString();
                            }
                            else
                            {
                                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "4S") einf116 = Math.Round((Convert.ToDouble(dt.Rows[0]["AMT_SALE"]) + Convert.ToDouble(dt.Rows[0]["oth_chgs"])), 2).ToString();
                                else einf116 = Math.Round((fgen.make_double(dt.Rows[0]["AMT_EXTEXC"].ToString()) + fgen.make_double(dt.Rows[0]["totdisc_amt"].ToString()) + fgen.make_double(dt.Rows[0]["return"].ToString())), 2).ToString();
                            }
                            einf117 = Math.Round(fgen.make_double(dt.Rows[0]["tcsamt"].ToString()), 2).ToString();
                            if (fgen.make_double(einf116) < 0)
                            {
                                einf117 = Math.Round(fgen.make_double(dt.Rows[0]["tcsamt"].ToString()), 2).ToString();
                                einf116 = "0";
                            }
                            if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "4S")
                            {
                                if (fgen.make_double(einf61) == Math.Round(fgen.make_double(einf117) + fgen.make_double(einf52) + fgen.make_double(einf59) + fgen.make_double(einf53) + fgen.make_double(einf54) + fgen.make_double(einf55) - fgen.make_double(einf116), 2))
                                {
                                }
                                else if (fgen.make_double(einf61) > fgen.make_double(einf116) + fgen.make_double(einf117) + fgen.make_double(einf52) + fgen.make_double(einf59) + fgen.make_double(einf53) + fgen.make_double(einf54) + fgen.make_double(einf55))
                                {
                                    einf117 = (Math.Round(fgen.make_double(einf61) - (fgen.make_double(einf117) + fgen.make_double(einf52) + fgen.make_double(einf59) + fgen.make_double(einf53) + fgen.make_double(einf54) + fgen.make_double(einf55)), 2)).ToString();
                                }
                                else
                                {
                                    einf116 = (Math.Round((fgen.make_double(einf117) + fgen.make_double(einf52) + fgen.make_double(einf59) + fgen.make_double(einf53) + fgen.make_double(einf54) + fgen.make_double(einf55)) - fgen.make_double(einf61), 2)).ToString();
                                }

                            }

                            einf118 = "^";

                            ds_gst = "^";
                            ds_aname = "^";
                            ds_add1 = dt.Rows[0]["sup_add1"].ToString().Trim();
                            ds_add2 = dt.Rows[0]["sup_add2"].ToString().Trim();
                            ds_add3 = dt.Rows[0]["sup_add3"].ToString().Trim();
                            ds_add4 = dt.Rows[0]["sup_state"].ToString().Trim();
                            ds_pinc = dt.Rows[0]["sup_pin"].ToString().Trim();

                            if (dt.Rows[0]["desp_From1"].ToString().Length == 6)
                            {
                                dt4 = new DataTable();
                                dt4 = fgen.getdata(frm_qstr, frm_cocd, "select '-' as aa,gst_no,addr1,addr2,addr3,staffcd,pincode,trim(tenum) as tele,trim(email) as email,aname from famst where trim(acode)='" + dt.Rows[0]["desp_From1"].ToString() + "'");
                                if (dt4.Rows.Count > 0)
                                {
                                    ds_st_cd = dt4.Rows[0]["aa"].ToString().Trim();
                                    einf73 = dt4.Rows[0]["gst_no"].ToString().Trim();
                                    einf78 = dt4.Rows[0]["addr1"].ToString().Trim();
                                    einf79 = dt4.Rows[0]["addr2"].ToString().Trim();
                                    einf75 = dt4.Rows[0]["addr3"].ToString().Trim();
                                    einf77 = dt4.Rows[0]["staffcd"].ToString().Trim();
                                    einf76 = dt4.Rows[0]["pincode"].ToString().Trim();
                                    einf82 = dt4.Rows[0]["tele"].ToString().Trim();
                                    einf83 = dt4.Rows[0]["email"].ToString().Trim();
                                    einf80 = dt4.Rows[0]["aname"].ToString().Trim();
                                }
                                if (einf3 == "SHP") einf3 = "CMB";
                                else einf3 = "DIS";
                            }
                            if (frm_cocd == "SAGM" && sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "4F")
                            {
                                if (fgen.make_double(einf38) - (fgen.make_double(einf34) * fgen.make_double(einf37)) >= 0)
                                {
                                    einf117 = "0";
                                    einf40 = "0";
                                    einf39 = "0";
                                }
                                else
                                {
                                    einf39 = (Math.Round((fgen.make_double(einf34) * fgen.make_double(einf37)) - fgen.make_double(einf38), 2)).ToString();
                                    einf40 = "0";
                                    einf117 = "0";
                                }
                                einf39 = "0";
                                einf40 = "0";
                                einf52 = fgen.make_double(einf41).ToString();
                                einf55 = fgen.make_double(einf43).ToString();
                                einf61 = fgen.make_double(einf48).ToString();
                                einf116 = fgen.make_double(einf39).ToString();
                            }

                            if (make_ewayb == "Y")
                            {
                                einf122 = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();  //wb_Mode
                                einf119 = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim(); //Ewb_TransId
                                einf120 = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim(); //Ewb_TransName
                                einf121 = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim(); //Ewb_VehNo
                            }

                            else
                            {
                                einf122 = "^";
                                einf119 = "^";
                                einf120 = "^";
                                einf121 = "^";
                            }
                            string demogstin = "";
                            //dummy gstin for demo purpose
                            if (demo_einv == "Y" && hffield.Value == "demo")
                            {
                                demogstin = dt.Rows[0]["GSTIN"].ToString().Substring(0, 2);
                                g_pwd = "Admin!23";//EFPassword and EinvPassword
                                g_efupwd = "Admin!23..";//EFPassword and EinvPassword
                                g_efukey = "1000687";
                                g_efuuid = "29AAACW3775F000"; // EFUserName
                                switch (demogstin)
                                {
                                    case "01":
                                        einf8 = "01AAACW3775F008"; //BillFrom_Gstin
                                        einf73 = "01AAACW3775F008";  //ShipFrom_Gstin
                                        g_uid = "01AAACW3775F008"; //EInvUserName
                                        break;
                                    case "02":
                                        einf8 = "02AAACW3775F009";
                                        einf73 = "02AAACW3775F009";
                                        g_uid = "02AAACW3775F009";
                                        break;
                                    case "03":
                                        einf8 = "03AAACW3775F010";
                                        einf73 = "03AAACW3775F010";
                                        g_uid = "03AAACW3775F010";
                                        break;
                                    case "04":
                                        einf8 = "04AAACW3775F011";
                                        einf73 = "04AAACW3775F011";
                                        g_uid = "04AAACW3775F011";
                                        break;
                                    case "05":
                                        einf8 = "05AAACW3775F012";
                                        einf73 = "05AAACW3775F012";
                                        g_uid = "05AAACW3775F012";
                                        break;
                                    case "06":
                                        einf8 = "06AAACW3775F013";
                                        einf73 = "06AAACW3775F013";
                                        g_uid = "06AAACW3775F013";
                                        break;
                                    case "07":
                                        einf8 = "07AAACW3775F006";
                                        einf73 = "07AAACW3775F006";
                                        g_uid = "07AAACW3775F006";
                                        break;
                                    case "08":
                                        einf8 = "08AAACW3775F014";
                                        einf73 = "08AAACW3775F014";
                                        g_uid = "08AAACW3775F014";
                                        break;
                                    case "09":
                                        einf8 = "09AAACW3775F015";
                                        einf73 = "09AAACW3775F015";
                                        g_uid = "09AAACW3775F015";
                                        break;
                                    case "10":
                                        einf8 = "10AAACW3775F016";
                                        einf73 = "10AAACW3775F016";
                                        g_uid = "10AAACW3775F016";
                                        break;
                                    case "11":
                                        einf8 = "11AAACW3775F017";
                                        einf73 = "11AAACW3775F017";
                                        g_uid = "11AAACW3775F017";
                                        break;
                                    case "12":
                                        einf8 = "12AAACW3775F018";
                                        einf73 = "12AAACW3775F018";
                                        g_uid = "12AAACW3775F018";
                                        break;
                                    case "13":
                                        einf8 = "13AAACW3775F019";
                                        einf73 = "13AAACW3775F019";
                                        g_uid = "13AAACW3775F019";
                                        break;
                                    case "14":
                                        einf8 = "14AAACW3775F020";
                                        einf73 = "14AAACW3775F020";
                                        g_uid = "14AAACW3775F020";
                                        break;
                                    case "15":
                                        einf8 = "15AAACW3775F021";
                                        einf73 = "15AAACW3775F021";
                                        g_uid = "15AAACW3775F021";
                                        break;
                                    case "16":
                                        einf8 = "16AAACW3775F022";
                                        einf73 = "16AAACW3775F022";
                                        g_uid = "16AAACW3775F022";
                                        break;
                                    case "17":
                                        einf8 = "17AAACW3775F023";
                                        einf73 = "17AAACW3775F023";
                                        g_uid = "17AAACW3775F023";
                                        break;
                                    case "18":
                                        einf8 = "18AAACW3775F024";
                                        einf73 = "18AAACW3775F024";
                                        g_uid = "18AAACW3775F024";
                                        break;
                                    case "19":
                                        einf8 = "19AAACW3775F025";
                                        einf73 = "19AAACW3775F025";
                                        g_uid = "19AAACW3775F025";
                                        break;
                                    case "20":
                                        einf8 = "20AAACW3775F026";
                                        einf73 = "20AAACW3775F026";
                                        g_uid = "20AAACW3775F026";
                                        break;
                                    case "21":
                                        einf8 = "21AAACW3775F027";
                                        einf73 = "21AAACW3775F027";
                                        g_uid = "21AAACW3775F027";
                                        break;
                                    case "22":
                                        einf8 = "22AAACW3775F028";
                                        einf73 = "22AAACW3775F028";
                                        g_uid = "22AAACW3775F028";
                                        break;
                                    case "23":
                                        einf8 = "23AAACW3775F029";
                                        einf73 = "23AAACW3775F029";
                                        g_uid = "23AAACW3775F029";
                                        break;
                                    case "24":
                                        einf8 = "24AAACW3775F030";
                                        einf73 = "24AAACW3775F030";
                                        g_uid = "24AAACW3775F030";
                                        break;
                                    case "25":
                                        einf8 = "25AAACW3775F031";
                                        einf73 = "25AAACW3775F031";
                                        g_uid = "25AAACW3775F031";
                                        break;
                                    case "26":
                                        einf8 = "26AAACW3775F032";
                                        einf73 = "26AAACW3775F032";
                                        g_uid = "26AAACW3775F032";
                                        break;
                                    case "27":
                                        einf8 = "27AAACW3775F007";
                                        einf73 = "27AAACW3775F007";
                                        g_uid = "27AAACW3775F007";
                                        break;
                                    case "28":
                                        einf8 = "28AAACW3775F007"; // no credentials in list
                                        einf73 = "28AAACW3775F007";
                                        g_uid = "28AAACW3775F007";
                                        break;
                                    case "29":
                                        einf8 = "29AAACW3775F000";
                                        einf73 = "29AAACW3775F000";
                                        g_uid = "29AAACW3775F000";
                                        g_pwd = "Admin!23..";
                                        break;
                                    case "30":
                                        einf8 = "30AAACW3775F033";
                                        einf73 = "30AAACW3775F033";
                                        g_uid = "30AAACW3775F033";
                                        break;
                                    case "31":
                                        einf8 = "31AAACW3775F034";
                                        einf73 = "31AAACW3775F034";
                                        g_uid = "31AAACW3775F034";
                                        break;
                                    case "32":
                                        einf8 = "32AAACW3775F035";
                                        einf73 = "32AAACW3775F035";
                                        g_uid = "32AAACW3775F035";
                                        break;
                                    case "33":
                                        einf8 = "33AAACW3775F036";
                                        einf73 = "33AAACW3775F036";
                                        g_uid = "33AAACW3775F036";
                                        break;
                                    case "34":
                                        einf8 = "34AAACW3775F037";
                                        einf73 = "34AAACW3775F037";
                                        g_uid = "34AAACW3775F037";
                                        break;
                                    case "35":
                                        einf8 = "35AAACW3775F038";
                                        einf73 = "35AAACW3775F038";
                                        g_uid = "35AAACW3775F038";
                                        break;
                                    case "36":
                                        einf8 = "36AAACW3775F039";
                                        einf73 = "36AAACW3775F039";
                                        g_uid = "36AAACW3775F039";
                                        break;
                                    case "37":
                                        einf8 = "37AAACW3775F040";
                                        einf73 = "37AAACW3775F040";
                                        g_uid = "37AAACW3775F040";
                                        break;
                                }
                            }

                            einfst2 = "{'Gstin': '" + einf8 + "','Version': '1.01','Irn': '','Tran_TaxSch': 'GST','Tran_SupTyp': '" + einf1 + "','Tran_RegRev': '" + einf2 + "','Tran_Typ': '" + einf3 + "','Tran_EcmGstin': '" + einf71 + "','Tran_IgstOnIntra': 'N','Doc_Typ': '" + einf5 + "','Doc_No': '" + einf6 + "','Doc_Dt': '" + einf7 + "','BillFrom_Gstin': '" + einf8 + "','BillFrom_TrdNm': '" + einf9 + "','BillFrom_LglNm': '" + einf9 + "','BillFrom_Addr1': '" + einf10 + "','BillFrom_Addr2': '" + einf11 + "','BillFrom_Loc': '" + einf13 + "','BillFrom_Pin': '" + einf15 + "',";
                            einfst3 = "'BillFrom_Stcd': '" + einf16 + "','BillFrom_Ph': '" + einf17 + "','BillFrom_Em': '" + einf18 + "','BillTo_Gstin': '" + einf19 + "','BillTo_TrdNm': '" + einf20 + "','BillTo_LglNm': '" + einf20 + "','BillTo_Pos': '" + einf24 + "','BillTo_Addr1': '" + einf21 + "','BillTo_Addr2': '" + einf22 + "','BillTo_Loc': '" + einf23 + "','BillTo_Pin': '" + einf26 + "','BillTo_Stcd': '" + einf27 + "','BillTo_Ph': '" + einf28 + "','BillTo_Em': '" + einf29 + "','ShipFrom_Nm': '" + einf73 + "','ShipFrom_Addr1': '" + einf78 + "','ShipFrom_Addr2': '" + einf79 + "','ShipFrom_Loc': '" + einf75 + "','ShipFrom_Pin': '" + einf76 + "','ShipFrom_Stcd': '" + einf77 + "',";
                            einfst4 = "'Item_SlNo': '" + einf30 + "','Item_PrdDesc': '" + einf31 + "','Item_HsnCd': '" + einf32 + "','Item_IsServc': '" + einf25 + "','Item_Barcde': '" + einf33 + "','Item_Qty': '" + einf34 + "','Item_FreeQty': '" + einf35 + "','Item_Unit': '" + einf36 + "','Item_UnitPrice': '" + einf37 + "','Item_TotAmt': '" + einf38 + "','Item_Discount': '" + einf39 + "','Item_OthChrg': '" + einf40 + "', 'Item_PreTaxVal': '" + einf14 + "','Item_AssAmt': '" + einf41 + "','Item_GstRt': '" + einf42 + "','Item_IgstAmt': '" + einf43 + "','Item_CgstAmt': '" + einf44 + "','Item_SgstAmt': '" + einf45 + "','Item_CesRt': '" + einf46 + "','Item_CesAmt': '" + einf46 + "',";
                            einfst5 = "'Item_CesNonAdvlAmt': '" + einf46 + "','Item_StateCesRt': '" + einf47 + "','Item_StateCesAmt': '" + einf47 + "','Item_StateCesNonAdvlAmt': '" + einf47 + "','Item_TotItemVal': '" + einf48 + "','Item_OrdLineRef': '" + einf49 + "','Item_OrgCntry': '" + einf49 + "','Item_PrdSlNo': '" + einf49 + "','Item_Attrib_Nm': '" + einf49 + "','Item_Attrib_Val': '" + einf49 + "','Item_Bch_Nm': '" + einf49 + "','Item_Bch_ExpDt': '" + einf50 + "','Item_Bch_WrDt': '" + einf51 + "','Val_AssVal': '" + einf52 + "','Val_CgstVal': '" + einf53 + "','Val_SgstVal': '" + einf54 + "','Val_IgstVal': '" + einf55 + "','Val_CesVal': '" + einf56 + "','Val_StCesVal': '" + einf57 + "','Val_Discount': '" + einf116 + "','Val_OthChrg': '" + einf117 + "',";
                            einfst6 = "'Val_RndOffAmt': '" + einf59 + "','Val_TotInvVal': '" + einf61 + "','Val_TotInvValFc': '" + einf60 + "','Pay_Nm': '" + einf62 + "','Pay_AcctDet': '" + einf98 + "','Pay_Mode': '" + einf63 + "','Pay_FinInsBr': '" + einf95 + "','Pay_PayTerm': '" + einf64 + "','Pay_PayInstr': '" + einf65 + "','Pay_CrTrn': '" + einf96 + "','Pay_DirDr': '" + einf97 + "','Pay_CrDay': '" + einf66 + "','Pay_PaidAmt': '" + einf67 + "','Pay_PaymtDue': '" + einf68 + "', 'Ref_InvRm': '" + einf69 + "','Ref_InvStDt': '" + einf70 + "','Ref_InvEndDt ':  '" + einf71 + "','Ref_PrecDoc_InvNo': '" + einf72 + "','Ref_PrecDoc_InvDt': '" + einf74 + "','Ref_PrecDoc_OthRefNo': '" + einf75 + "','Ref_Contr_RecAdvRefr': '" + einf75 + "','Ref_Contr_RecAdvDt': '" + einf75 + "',";
                            einfst7 = "'Ref_Contr_TendRefr': '" + einf75 + "','Ref_Contr_ContrRefr': '" + einf75 + "','Ref_Contr_ExtRefr': '" + einf75 + "','Ref_Contr_ProjRefr': '" + einf75 + "','Ref_Contr_PORefr': '" + einf75 + "','Ref_Contr_PORefDt': '" + einf75 + "',";
                            einfst8 = "'ShipTo_Gstin': '" + einf84 + "','ShipTo_LglNm': '" + einf85 + "','ShipTo_TrdNm': '" + einf85 + "','ShipTo_Addr1': '" + einf89 + "','ShipTo_Addr2': '" + einf90 + "','ShipTo_Loc': '" + einf86 + "','ShipTo_Pin': '" + einf88 + "','ShipTo_Stcd': '" + einf87 + "','Exp_ShipBNo': '" + einf112 + "','Exp_ShipBDt': '" + einf113 + "','Exp_Port': '" + einf114 + "',";

                            einfst9 = "'AddlDoc_Url': '" + einf75 + "','AddlDoc_Docs': '" + einf75 + "','AddlDoc_Info': '" + einf75 + "','Ewb_TransMode': '" + einf122 + "','Ewb_TransName': '" + einf120 + "','Ewb_TransId': '" + einf119 + "','Ewb_VehNo': '" + einf121 + "','CDKey': '" + g_efukey + "','EInvUserName': '" + g_uid + "','EInvPassword': '" + g_pwd + "','EFUserName': '" + g_efuuid + "','EFPassword': '" + g_efupwd + "'},";

                            cc = (cc.Trim()) + (einfst2.Trim()) + (einfst3.Trim()) + (einfst4.Trim()) + (einfst5.Trim()) + (einfst6.Trim()) + (einfst7.Trim()) + (einfst8.Trim()) + (einfst9.Trim());


                            //AA = "{'GSTIN': '" + mygstno + "','SupplyType': '" + dt.Rows[d1]["sup_type"].ToString() +"','SubType': '" + dt.Rows[d1]["sub_type"].ToString() +"','DocType': '" + dt.Rows[d1]["doc_type"].ToString() + "','DocNo': '" + dt.Rows[d1]["doc_no"].ToString() + "','DocDate': '" + dt.Rows[d1]["doc_dt"].ToString() + "','SupGSTIN': '"  + dt.Rows[d1]["sup_gst"].ToString() +  "','SupName': '" + dt.Rows[d1]["sup_nam"].ToString() + "','SupAdd1': '"+ dt.Rows[d1]["sup_add1"].ToString() + "','SupAdd2': '" +dt.Rows[d]["sup_add2"].ToString()+ "','SupCity': '" +dt.Rows[d1]["sup_add3"].ToString()+ "','SupState': '" + dt.Rows[d1]["sup_state"].ToString() +"','SupPincode': '"+ dt.Rows[d1]["sup_pin"].ToString()+ "','RecGSTIN': '"+ dt.Rows[d1]["rec_gst"].ToString() +"','RecName': '" + dt.Rows[d1]["rec_nam"].ToString() + "','RecAdd1': '" + dt.Rows[d1]["rec_add1"].ToString() + "','RecAdd2': '" + dt.Rows[d]["rec_add2"].ToString() + "','Reccity': '" + dt.Rows[d1]["rec_add3"].ToString() +"','RecState': '"+ dt.Rows[d1]["rec_state"].ToString() +"','Recpincode': '" + dt.Rows[d1]["rec_pin"].ToString() + "','TransMode': '" + dt.Rows[d1]["tran_mode"].ToString() +  "',";
                            //BB = "'TransporterId': '" + dt.Rows[d1]["tran_id"].ToString() + "','TransporterName': '" + dt.Rows[d1]["tran_name"].ToString() + "','TransDistance': " + dt.Rows[d1]["tran_dist"].ToString() + ",'TransDocNo': '" + dt.Rows[d1]["tran_doc"].ToString() + "','TransDocDate': '" + dt.Rows[d1]["tran_dt"].ToString() + "','CessAdvol': 'd','VehicleNo': '" + dt.Rows[d1]["vehi_no"].ToString() + "','VehicleType': 'R','ItemNo': " + dt.Rows[d1]["item_no"].ToString() + ",'ProductName': '" + dt.Rows[d]["prod_name"].ToString() + "','ProductDesc': '"+ dt.Rows[d1]["prod_desc"].ToString() +"','HSNCode': " + dt.Rows[d1]["hs_code"].ToString() +",'Quantity': " + dt.Rows[d1]["quantity"].ToString() + ",'QtyUnit': '" + dt.Rows[d1]["quan_unit"].ToString() + "','TaxableValue': " +dt.Rows[d1]["taxb_val"].ToString()+ ",'SGSTRate': " +dt.Rows[d1]["sgst_rt"].ToString()+ ",'SGSTValue': " +dt.Rows[d1]["sgst_val"].ToString()+ ",'CGSTRate': "+dt.Rows[d1]["cgst_Rt"].ToString()+",'CGSTValue': "+dt.Rows[d1]["cgst_Val"].ToString()+ ",'IGSTRate': "+dt.Rows[d1]["igst_Rt"].ToString()+ ",'IGSTValue': " +dt.Rows[d1]["igst_val"].ToString()+",'CessRate': " +dt.Rows[d1]["cess_rt"].ToString()+ ",'CessValue': " +dt.Rows[d1]["cess_val"].ToString()+ ",'EWBUserName': '" +g_uid + "','EWBPassword': '" + g_pwd + "'  }";
                            //if (dt.Rows.Count == 1)
                            //    cc = cc + AA + BB;
                            //else
                            //    cc = cc + AA + BB + ",";

                            d1++;
                        } while (d1 < dt.Rows.Count);

                        cc = cc.TrimEnd(',');
                        cc = cc + "]}}";
                        if (cc.Trim().Substring(cc.Length - 1, 1) == ",")
                        {
                            cc = cc.Substring(0, cc.Length - 1);

                        }
                        cc = einfst1 + cc;
                        //cc = cc.Replace("'",""+""+"");
                        cc = cc.Replace("^", " ");
                        // cc = cc.Replace("-"," ");

                        cc_final = cc_final + cc;
                        string makewebrequest = "";


                        //cc = cc + "],'Year':2018,'Month':1,'EFUserName':'" + g_efuuid + "','EFPassword':'" + g_efupwd + "','CDKey':'" + g_efukey + "'}";
                        //cc="{ 'Push_Data_List': [{'GSTIN': '06AACCR0859H1ZE','SupplyType': 'O','SubType': '1','DocType': 'INV','DocNo': '203701','DocDate': '20191214','SupGSTIN': '06AACCR0859H1ZE','SupName': 'XLERATE DRIVELINE PVT LTD.','SupAdd1': 'REGD.OFFICE CUM WORKS:SHED NO.1+3,GURUKUL INDUSTRIAL  ESTATE','SupAdd2': 'FARIDABAD(HARYANA-121003)','SupCity': 'FARIDABAD','SupState': '06','SupPincode': '121003','RecGSTIN': '32ADTFS1746H1ZT','RecName': 'STANDARD AUTO DISTRIBUTORS (KOZHIKODE)','RecAdd1': '5/1100S,T,U,V,SREE HARI BUILDING','RecAdd2': 'KOTTARAM CROSS ROAD,','Reccity': 'CALICUT','RecState': '32','Recpincode': '673006','TransMode': '1','TransporterId': '07AAECS4363H1ZA','TransporterName': 'SAFEXPRESS PRIVATE LIMITED','TransDistance': 2750,'TransDocNo': '-','TransDocDate': '20191214','CessAdvol': 'd','VehicleNo': '','VehicleType': 'R','ItemNo': 1,'ProductName': 'CHAPTER HEAD 8708','ProductDesc': '310 DIA CLUTCH PLATE FOR TATA BLACK FACING','HSNCode': 8708,'Quantity': 6,'QtyUnit': 'NOS','TaxableValue': 8064,'SGSTRate': 0,'SGSTValue': 0,'CGSTRate': 0,'CGSTValue': 0,'IGSTRate': 28,'IGSTValue': 2257.92,'CessRate': 0,'CessValue': 0,'EWBUserName': 'xdilfbd_API_XDI','EWBPassword': '06AACCR0859H1ZE'  },{'GSTIN': '06AACCR0859H1ZE','SupplyType': 'O','SubType': '1','DocType': 'INV','DocNo': '203701','DocDate': '20191214','SupGSTIN': '06AACCR0859H1ZE','SupName': 'XLERATE DRIVELINE PVT LTD.','SupAdd1': 'REGD.OFFICE CUM WORKS:SHED NO.1+3,GURUKUL INDUSTRIAL  ESTATE','SupAdd2': 'FARIDABAD(HARYANA-121003)','SupCity': 'FARIDABAD','SupState': '06','SupPincode': '121003','RecGSTIN': '32ADTFS1746H1ZT','RecName': 'STANDARD AUTO DISTRIBUTORS (KOZHIKODE)','RecAdd1': '5/1100S,T,U,V,SREE HARI BUILDING','RecAdd2': 'KOTTARAM CROSS ROAD,','Reccity': 'CALICUT','RecState': '32','Recpincode': '673006','TransMode': '1','TransporterId': '07AAECS4363H1ZA','TransporterName': 'SAFEXPRESS PRIVATE LIMITED','TransDistance': 2750,'TransDocNo': '-','TransDocDate': '20191214','CessAdvol': 'd','VehicleNo': '','VehicleType': 'R','ItemNo': 2,'ProductName': 'CHAPTER HEAD 8708','ProductDesc': 'CLUTCH PLATE TATA 407 TURBO','HSNCode': 8708,'Quantity': 5,'QtyUnit': 'NOS','TaxableValue': 4370,'SGSTRate': 0,'SGSTValue': 0,'CGSTRate': 0,'CGSTValue': 0,'IGSTRate': 28,'IGSTValue': 1223.60,'CessRate': 0,'CessValue': 0,'EWBUserName': 'xdilfbd_API_XDI','EWBPassword': '06AACCR0859H1ZE'  },{'GSTIN': '06AACCR0859H1ZE','SupplyType': 'O','SubType': '1','DocType': 'INV','DocNo': '203701','DocDate': '20191214','SupGSTIN': '06AACCR0859H1ZE','SupName': 'XLERATE DRIVELINE PVT LTD.','SupAdd1': 'REGD.OFFICE CUM WORKS:SHED NO.1+3,GURUKUL INDUSTRIAL  ESTATE','SupAdd2': 'FARIDABAD(HARYANA-121003)','SupCity': 'FARIDABAD','SupState': '06','SupPincode': '121003','RecGSTIN': '32ADTFS1746H1ZT','RecName': 'STANDARD AUTO DISTRIBUTORS (KOZHIKODE)','RecAdd1': '5/1100S,T,U,V,SREE HARI BUILDING','RecAdd2': 'KOTTARAM CROSS ROAD,','Reccity': 'CALICUT','RecState': '32','Recpincode': '673006','TransMode': '1','TransporterId': '07AAECS4363H1ZA','TransporterName': 'SAFEXPRESS PRIVATE LIMITED','TransDistance': 2750,'TransDocNo': '-','TransDocDate': '20191214','CessAdvol': 'd','VehicleNo': '','VehicleType': 'R','ItemNo': 3,'ProductName': 'CHAPTER HEAD 8708','ProductDesc': 'FG-CLUTCH PLATE 280 DIA','HSNCode': 8708,'Quantity': 5,'QtyUnit': 'NOS','TaxableValue': 6305,'SGSTRate': 0,'SGSTValue': 0,'CGSTRate': 0,'CGSTValue': 0,'IGSTRate': 28,'IGSTValue': 1765.40,'CessRate': 0,'CessValue': 0,'EWBUserName': 'xdilfbd_API_XDI','EWBPassword': '06AACCR0859H1ZE'  }],'Year':2018,'Month':1,'EFUserName':'05AAACD8069KIZF','EFPassword':'abc123@@','CDKey':'1000687'}";
                        cc = cc.Replace("'", "\"");

                        if (demo_einv == "Y" && hffield.Value == "demo")
                        {
                            g_api_link = "http://einvsandbox.webtel.in/v1.03/GenIRN";

                            //fgen.msg("-","AMSG","Dear " + frm_UserID + " ,Demo_IRN being generated, Do you want to save it in the database.");                    
                        }

                        makewebrequest = MakeWebRequest("POST", g_api_link, cc);
                        // makewebrequest = MakeWebRequest("POST", "http://einvsandbox.webtel.in/v1.03/GenIRN", cc);
                        // makewebrequest = MakeWebRequest("POST", "http://ip.webtel.in/eWayGSP2/sandbox/EWayBill/GenEWB", cc);
                        //[{"ErrorMessage":"","ErrorCode":"","Status":"1","GSTIN":"06AAACZ6373J1ZY","DocNo":"4S/004321","DocType":"INV","DocDate":"01/10/2021","Irn":"edb0f7e4ec53ce4e3c4e9bcd1a582f3be2c68d0c24dd82208600e59fd4e5c1d5","AckDate":"2021-10-01 12:49:00","AckNo":132111332599110,"EwbNo":null,"EwbDt":null,"EwbValidTill":null,"SignedQRCode":"eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ0NDQwNUM3ODFFNDgyNTA3MkIzNENBNEY4QkRDNjA2Qzg2QjU3MjAiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJSRVFGeDRIa2dsQnlzMHlrLUwzR0JzaHJWeUEifQ.eyJkYXRhIjoie1wiU2VsbGVyR3N0aW5cIjpcIjA2QUFBQ1o2MzczSjFaWVwiLFwiQnV5ZXJHc3RpblwiOlwiMDdBQUdGRzUxODZLMVowXCIsXCJEb2NOb1wiOlwiNFMvMDA0MzIxXCIsXCJEb2NUeXBcIjpcIklOVlwiLFwiRG9jRHRcIjpcIjAxLzEwLzIwMjFcIixcIlRvdEludlZhbFwiOjc2ODQxLjAsXCJJdGVtQ250XCI6MyxcIk1haW5Ic25Db2RlXCI6XCI4NzA4OTkwMFwiLFwiSXJuXCI6XCJlZGIwZjdlNGVjNTNjZTRlM2M0ZTliY2QxYTU4MmYzYmUyYzY4ZDBjMjRkZDgyMjA4NjAwZTU5ZmQ0ZTVjMWQ1XCIsXCJJcm5EdFwiOlwiMjAyMS0xMC0wMSAxMjo0OTowMFwifSIsImlzcyI6Ik5JQyJ9.W0OdDSzIBiek7qWAvJ0TKsE1hH8ck8sD-S1ZpdhKf4l4ei0N5v5FpWBfcKjQqvfQ0YTmNysR-akSSbYWC95urItJkgq87NDoMTRFGDItZMvsmzCu1BTwcLkciRmtpfcAsQn2F7gHUKh2E7vWYlXrFgFJEMdAkz5AEA0-cFmP7HYKVJj2pabqhH4L348P916HpcyQNTJlRPAPkS9nosJmcy8BvNPHpXUtopSVH3usg2fDbQ2qK48LZo1RQwbfFTCRMTpUAh985IBNQb0-G95K798LXfwQ3jk2AbAyYei_xlNx9DNQM9Xh9o2Wn9PCoutTnLag0ogsXeDMIsIyHOU5kg","SignedInvoice":"eyJhbGciOiJSUzI1NiIsImtpZCI6IjQ0NDQwNUM3ODFFNDgyNTA3MkIzNENBNEY4QkRDNjA2Qzg2QjU3MjAiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJSRVFGeDRIa2dsQnlzMHlrLUwzR0JzaHJWeUEifQ.eyJkYXRhIjoie1wiQWNrTm9cIjoxMzIxMTEzMzI1OTkxMTAsXCJBY2tEdFwiOlwiMjAyMS0xMC0wMSAxMjo0OTowMFwiLFwiSXJuXCI6XCJlZGIwZjdlNGVjNTNjZTRlM2M0ZTliY2QxYTU4MmYzYmUyYzY4ZDBjMjRkZDgyMjA4NjAwZTU5ZmQ0ZTVjMWQ1XCIsXCJWZXJzaW9uXCI6XCIxLjFcIixcIlRyYW5EdGxzXCI6e1wiVGF4U2NoXCI6XCJHU1RcIixcIlN1cFR5cFwiOlwiQjJCXCIsXCJSZWdSZXZcIjpcIk5cIixcIklnc3RPbkludHJhXCI6XCJOXCJ9LFwiRG9jRHRsc1wiOntcIlR5cFwiOlwiSU5WXCIsXCJOb1wiOlwiNFMvMDA0MzIxXCIsXCJEdFwiOlwiMDEvMTAvMjAyMVwifSxcIlNlbGxlckR0bHNcIjp7XCJHc3RpblwiOlwiMDZBQUFDWjYzNzNKMVpZXCIsXCJMZ2xObVwiOlwiR0FMSU8gR1JPVVBcIixcIlRyZE5tXCI6XCJHQUxJTyBHUk9VUFwiLFwiQWRkcjFcIjpcIlBMT1QgTk8uLSA1MixTRUNUT1IgLSA1MyxQSEFTRS1WLCBIU0lJREMgSU5EVVNUUklBTCBFU1RBVEVcIixcIkFkZHIyXCI6XCJLVU5ETEksU09OSVBBVCwgSEFSWUFOQSAoSU5ESUEpIC0xMzEwMjhcIixcIkxvY1wiOlwiS1VORExJXCIsXCJQaW5cIjoxMzEwMjgsXCJTdGNkXCI6XCIwNlwiLFwiUGhcIjpcIjAxMzA0MDkyNlwiLFwiRW1cIjpcIklORk9ASlNHLkFTSUFcIn0sXCJCdXllckR0bHNcIjp7XCJHc3RpblwiOlwiMDdBQUdGRzUxODZLMVowXCIsXCJMZ2xObVwiOlwiR0FMSU8gR1JBUEhJQ1MgICBNR1AgKFNBTEUpXCIsXCJUcmRObVwiOlwiR0FMSU8gR1JBUEhJQ1MgICBNR1AgKFNBTEUpXCIsXCJQb3NcIjpcIjA3XCIsXCJBZGRyMVwiOlwiMTUvMjUvNUEgLCBGSVJTVCBGTE9PUiAsIE1BTkdPTFBVUiBLQUxBTlwiLFwiQWRkcjJcIjpcIlBPQ0tFVCAgIDQgLCBTRUNUT1IgICAyICxcIixcIkxvY1wiOlwiTkVXIERFTEhJXCIsXCJQaW5cIjoxMTAwODUsXCJFbVwiOlwiQklMTElOR0BHQUxJT0lORElBLkNPTVwiLFwiU3RjZFwiOlwiMDdcIn0sXCJJdGVtTGlzdFwiOlt7XCJJdGVtTm9cIjowLFwiU2xOb1wiOlwiMVwiLFwiSXNTZXJ2Y1wiOlwiTlwiLFwiUHJkRGVzY1wiOlwiU1dJRlQgMjAxOCBET09SIEhBTkRMRSBDT1ZFUiAoNyBQQ1MpLUdGWE0tMDEwXCIsXCJIc25DZFwiOlwiODcwODk5MDBcIixcIlF0eVwiOjIxMi4wLFwiRnJlZVF0eVwiOjAuMCxcIlVuaXRcIjpcIlNFVFwiLFwiVW5pdFByaWNlXCI6MzEyLjUsXCJUb3RBbXRcIjo2NjI1MC4wLFwiRGlzY291bnRcIjo5Mjc1LjAsXCJBc3NBbXRcIjo1Njk3NS4wLFwiR3N0UnRcIjoyOC4wLFwiSWdzdEFtdFwiOjE1OTUzLjAsXCJUb3RJdGVtVmFsXCI6NzI5MjguMH0se1wiSXRlbU5vXCI6MCxcIlNsTm9cIjpcIjJcIixcIklzU2VydmNcIjpcIk5cIixcIlByZERlc2NcIjpcIkJSRUVaQSAyMDE3IERPT1IgSEFORExFIENPVkVSICg5IFBDUyBXL08gU0VOU09SKS1HRlhNLTAxN1wiLFwiSHNuQ2RcIjpcIjg3MDg5OTAwXCIsXCJRdHlcIjo4LjAsXCJGcmVlUXR5XCI6MC4wLFwiVW5pdFwiOlwiU0VUXCIsXCJVbml0UHJpY2VcIjozMTIuNSxcIlRvdEFtdFwiOjI1MDAuMCxcIkRpc2NvdW50XCI6MzUwLjAsXCJBc3NBbXRcIjoyMTUwLjAsXCJHc3RSdFwiOjI4LjAsXCJJZ3N0QW10XCI6NjAyLjAsXCJUb3RJdGVtVmFsXCI6Mjc1Mi4wfSx7XCJJdGVtTm9cIjowLFwiU2xOb1wiOlwiM1wiLFwiSXNTZXJ2Y1wiOlwiTlwiLFwiUHJkRGVzY1wiOlwiQUxUTyA4MDAgMjAxNiBET09SIEhBTkRMRSBDT1ZFUi1HRlhNLTAyOVwiLFwiSHNuQ2RcIjpcIjg3MDg5OTAwXCIsXCJRdHlcIjo5LjAsXCJGcmVlUXR5XCI6MC4wLFwiVW5pdFwiOlwiU0VUXCIsXCJVbml0UHJpY2VcIjoxMTcuMTksXCJUb3RBbXRcIjoxMDU0LjcxLFwiRGlzY291bnRcIjoxNDcuNjYsXCJBc3NBbXRcIjo5MDcuMDUsXCJHc3RSdFwiOjI4LjAsXCJJZ3N0QW10XCI6MjUzLjk3LFwiVG90SXRlbVZhbFwiOjExNjEuMDJ9XSxcIlZhbER0bHNcIjp7XCJBc3NWYWxcIjo2MDAzMi4wNSxcIkNnc3RWYWxcIjowLjAsXCJTZ3N0VmFsXCI6MC4wLFwiSWdzdFZhbFwiOjE2ODA4Ljk3LFwiQ2VzVmFsXCI6MC4wLFwiU3RDZXNWYWxcIjowLjAsXCJUb3RJbnZWYWxcIjo3Njg0MS4wfX0iLCJpc3MiOiJOSUMifQ.i-xhwlpUIqmcw-oBLqECxJaAZYfx4jmICHjp8m1rgzGWxJCWSKCPrX67wU_QzLAoCX2W-GvGueyA-kbI0JJCUvI_T2DbLG2w3m8S7-oOxv3TOuG-HxW0PX3wFMTf7I5auoZNNEsCq97iFDFDMppWfG6GSS-1K1b_vYI-GLF1zgsj427aZQ3iJNylCMVy-LgcxAmw1skMuxxV3O2EYQtZKNomMlWOAEhBilII6kgqTRzbQlfM0XsqG1ukgIL7kW69UHLbSHaMmqPF81Xz0q4FZkxNcEazEDwGX8WLmRVYH2nK27LAr1IMvLSnMDtBdghVJrgU2diqOEhzEwkQzBEgVw","IrnStatus":"ACT","InfoDtls":null,"Remarks":null,"UniqueKey":""}]
                        if (FOPT == "JSON")
                        {
                        }
                        else
                        {
                            string ee = "", ff = "", gg = "", irnd = "", my_qrcd = "", MY_INS_NO, vackno = "", vackdt = "";
                            //AA = makewebrequest.IndexOf("DocDate").ToString();
                            //BB = makewebrequest.IndexOf("AckDate").ToString();
                            //ee = makewebrequest.IndexOf("SignedQRCode").ToString();
                            //ff = makewebrequest.IndexOf("SignedInvoice").ToString();
                            var dicddd = new Dictionary<string, object>();
                            JavaScriptSerializer jss = new JavaScriptSerializer();
                            ArrayList itemss = jss.Deserialize<ArrayList>(makewebrequest);
                            ArrayList itemsss = jss.Deserialize<ArrayList>(makewebrequest);
                            foreach (var value in itemss)
                            {
                                dicddd = ((Dictionary<string, object>)itemss[0]);
                                foreach (var dc in dicddd)
                                {
                                    if (dc.Key.ToString().ToUpper() == "ERRORMESSAGE")
                                    {
                                        if (dc.Value.ToString().Length > 3)
                                            if (dc.Value.ToString().Substring(0, 13).ToUpper() == "DUPLICATE IRN" && ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Length != 64)
                                            //"Duplicate IRN. AckNo: 132111332599110; Irn: edb0f7e4ec53ce4e3c4e9bcd1a582f3be2c68d0c24dd82208600e59fd4e5c1d5; AckDt: 2021-10-01 12:49:00. AckNo : 132111332599110; Irn : edb0f7e4ec53ce4e3c4e9bcd1a582f3be2c68d0c24dd82208600e59fd4e5c1d5; AckDt : 2021-10-01 12:49:00"
                                            {
                                                vackno = dc.Value.ToString().Substring(22, 15);
                                                irnd = dc.Value.ToString().Substring(43, 64);
                                                vackdt = dc.Value.ToString().Substring(117, 15);
                                                ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).BackColor = System.Drawing.Color.Yellow;
                                                ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text = vackno;
                                                ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text = vackdt;
                                                ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text = irnd;
                                                if ((sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1)) == "4")
                                                {
                                                    do_upd_tran_file("SALE", i);// for duplicate irn creation
                                                }
                                                else
                                                {
                                                    do_upd_tran_file("IVOUCHER", i);
                                                }

                                                fgen.msgBig(frm_qstr, "-", "AMSG", "Error Message : " + dc.Value.ToString() + "CAUTION !!!! Original IRN retrieved. QR CODE details can be retrieved upto 2 days only from IRN portal.Pls use get QR code button or contact FINSYS SUPPORT.");
                                                return;
                                            }
                                            else
                                            {
                                                //fgen.msgBig(frm_qstr, "-", "AMSG", "JSON File Generated AT c:\\TEJ_erp\\WTEINVWeb.JSON  ::::Error Message from Government Einvoice portal : " + dc.Value.ToString());
                                                fgen.msgBig(frm_qstr, "-", "AMSG", "JSON File Generated AT c:\\TEJ_erp\\WTEINVWeb.JSON  ::::Error Message from Government Einvoice portal : " + dc.Value.ToString() + " for Document  :" + frm_mbr + "-" + einf5 + "-" + einf6 + "-" + einf7 + ".");
                                                //  string filePath1 = "c:\\TEJ_erp\\WTEINVWeb.JSON";//old
                                                string filePath1 = "c:\\TEJ_erp\\UPLOAD\\WTEINVWeb_" + frm_uname + ".JSON";
                                                StreamWriter w1;
                                                w1 = File.CreateText(filePath1);
                                                cc_final = cc_final.Replace("'", "\"");
                                                w1.WriteLine(cc_final);
                                                w1.Flush();
                                                w1.Close();
                                                ///

                                                #region yogita THIS IS FOR DOWNLOAD JSON FILE ON LOCAL SYSTEM
                                                string mq7 = @"c:\TEJ_ERP\upload\";
                                                if (!Directory.Exists(mq7)) Directory.CreateDirectory(mq7);
                                                string fileName = "WTEINVWeb_" + frm_uname + "";
                                                string filepath = @"c:\TEJ_ERP\Upload\WTEINVWeb_" + frm_uname + ".JSON";
                                                Session["FilePath"] = fileName + ".json";
                                                Session["FileName"] = fileName + ".json";
                                                Response.Write("<script>");
                                                Response.Write("window.open('../fin-base/dwnlodFile.aspx','_blank')");
                                                Response.Write("</script>");
                                                fgen.msg("-", "AMSG", "File has been downloaded at " + filepath + ".txt" + "");
                                                #endregion

                                                return;
                                            }
                                    }
                                    if (dc.Key.ToString() == "DocDate")
                                    {
                                        AA = dc.Value.ToString();
                                    }
                                    if (dc.Key.ToString() == "AckDate")
                                    {
                                        BB = dc.Value.ToString();
                                    }
                                    if (dc.Key.ToString() == "SignedQRCode")
                                    {
                                        ee = dc.Value.ToString();
                                    }
                                    if (dc.Key.ToString() == "SignedInvoice")
                                    {
                                        ff = dc.Value.ToString();
                                    }
                                    if (dc.Key.ToString() == "Irn")
                                    {
                                        irnd = dc.Value.ToString();
                                    }
                                    if (dc.Key.ToString() == "AckNo")
                                    {
                                        vackno = dc.Value.ToString();
                                    }
                                    if (dc.Key.ToString() == "UniqueKey")
                                    {
                                        gg = dc.Value.ToString();
                                    }
                                }
                            }
                            if (ee == "0" || ff == "0") MY_INS_NO = makewebrequest;
                            else
                            {
                                my_qrcd = ee;
                                vackdt = BB;
                            }
                            if (ee == "0" || ff == "0") return;
                            if (irnd.ToString().Length >= 64 && my_qrcd.ToString().Length >= 100)
                            {
                                ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text = irnd;
                                if (my_qrcd.ToString().Length >= 3000)
                                {
                                    ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text = my_qrcd.Substring(1, 3000);
                                }
                                else
                                {
                                    ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text = my_qrcd;
                                }
                                if (my_qrcd.ToString().Length > 3000)
                                {
                                    ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text = "-";
                                }
                                ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text = vackno;
                                ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text = vackdt;
                                ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).BackColor = System.Drawing.Color.GreenYellow;


                                if ((sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1)) == "4")
                                {
                                    do_upd_tran_file("SALE", i);// for actual einv creation
                                }
                                else
                                {
                                    do_upd_tran_file("IVOUCHER", i);
                                }
                                if ((FOPT == "JSON") || (FOPT == "WEBT"))
                                {
                                    if (FOPT == "JSON")
                                    {
                                        //TextWriter tw = File.CreateText(@"c:\TEJ_ERP\EwayBillTest.JSON");
                                        string filePath = "c:\\TEJ_erp\\EwayBillTest123.JSON";
                                        StreamWriter w = File.CreateText(filePath);
                                        int ii = 0;
                                        AA = "{'version': '1.0.0501','billLists':[";
                                        //AA = AA.Replace("'", "”");
                                        AA = AA.Replace("'", "\"");
                                        w.WriteLine(AA);
                                        //tw.WriteLine(AA);

                                        for (ii = 0; ii < TOT_INV; ii++)
                                        {
                                            cc = "";
                                            if (Edesc[ii].Length > 1)
                                            {
                                                if (ii == TOT_INV - 1)
                                                {
                                                    //TextWriter tw = File.CreateText(@"c:\TEJ_ERP\EwayBillTest.JSON");
                                                    filePath = "c:\\TEJ_erp\\WTEINVWEB.JSON";
                                                    w = File.CreateText(filePath);
                                                    AA = "{'version': '1.0.0501','billLists':[";
                                                    //AA = AA.Replace("'", "”");
                                                    AA = AA.Replace("'", "\"");
                                                    w.WriteLine(AA);
                                                    //tw.WriteLine(AA);

                                                    for (ii = 0; ii < TOT_INV; ii++)
                                                    {
                                                        cc = Edesc[ii].Substring(0, Edesc[ii].Length - 2) + "}]}";
                                                    }
                                                }
                                                else
                                                {
                                                    cc = Edesc[ii];
                                                }

                                                cc = cc.Replace("'", "\"");

                                            }
                                            w.WriteLine(cc);
                                        }
                                        w.Flush();
                                        w.Close();
                                        fgen.msg("", "ASMG", "JSON File Generated AT c:\\TEJ_erp\\WTEINV.JSON Upload this File to GST PORTAL to Generate E-Invoice Then Update on This Screen and Save");
                                        return;
                                    }
                                    else
                                    {
                                        // string filePath1 = "c:\\TEJ_erp\\WTEINVWeb.JSON";//old
                                        string filePath1 = "c:\\TEJ_erp\\UPLOAD\\WTEINVWeb_" + frm_uname + ".JSON";

                                        StreamWriter w1;
                                        w1 = File.CreateText(filePath1);
                                        cc_final = cc_final.Replace("'", "\"");
                                        w1.WriteLine(cc_final);
                                        //TextWriter tw1 = File.CreateText(@"c:\TEJ_ERP\WTEWAYBILL.JSON");
                                        w1.Flush();
                                        w1.Close();
                                        fgen.msg("", "ASMG", "WEBTEL JSON File Generated AT c:\\TEJ_erp\\WTEINVWeb.JSON");

                                        #region yogita THIS IS FOR DOWNLOAD JSON FILE ON LOCAL SYSTEM
                                        string mq7 = @"c:\TEJ_ERP\upload\";
                                        if (!Directory.Exists(mq7)) Directory.CreateDirectory(mq7);
                                        string fileName = "WTEINVWeb_" + frm_uname + "";
                                        string filepath = @"c:\TEJ_ERP\Upload\WTEINVWeb_" + frm_uname + ".JSON";
                                        Session["FilePath"] = fileName + ".json";
                                        Session["FileName"] = fileName + ".json";
                                        Response.Write("<script>");
                                        Response.Write("window.open('../fin-base/dwnlodFile.aspx','_blank')");
                                        Response.Write("</script>");
                                        fgen.msg("-", "AMSG", "File has been downloaded at " + filepath + ".txt" + "");
                                        #endregion
                                    }
                                }

                            }
                        }

                    }
                }

            }
        }

        if ((FOPT == "JSON") || (FOPT == "WEBT"))
        {
            if (FOPT == "JSON")
            {
                //TextWriter tw = File.CreateText(@"c:\TEJ_ERP\EwayBillTest.JSON");
                string filePath = "c:\\TEJ_erp\\EwayBillTest123.JSON";
                StreamWriter w = File.CreateText(filePath);
                int ii = 0;
                AA = "{'version': '1.0.0501','billLists':[";
                //AA = AA.Replace("'", "”");
                AA = AA.Replace("'", "\"");
                w.WriteLine(AA);
                //tw.WriteLine(AA);

                for (ii = 0; ii < TOT_INV; ii++)
                {
                    cc = "";
                    if (Edesc[ii].Length > 1)
                    {
                        if (ii == TOT_INV - 1)
                        {
                            //TextWriter tw = File.CreateText(@"c:\TEJ_ERP\EwayBillTest.JSON");
                            filePath = "c:\\TEJ_erp\\WTEINVWEB.JSON";
                            w = File.CreateText(filePath);
                            AA = "{'version': '1.0.0501','billLists':[";
                            //AA = AA.Replace("'", "”");
                            AA = AA.Replace("'", "\"");
                            w.WriteLine(AA);
                            //tw.WriteLine(AA);

                            for (ii = 0; ii < TOT_INV; ii++)
                            {
                                cc = Edesc[ii].Substring(0, Edesc[ii].Length - 2) + "}]}";

                            }
                        }
                        else
                        {
                            cc = Edesc[ii];
                        }

                        cc = cc.Replace("'", "\"");

                    }
                    w.WriteLine(cc);
                }
                w.Flush();
                w.Close();

                fgen.msg("", "ASMG", "JSON File Generated AT c:\\TEJ_erp\\WTEINV.JSON Upload this File to GST PORTAL to Generate E-Invoice Then Update on This Screen and Save");
                return;
            }
            else
            {

                string filePath1 = "c:\\TEJ_erp\\WTEINVWeb.JSON";
                StreamWriter w1;
                w1 = File.CreateText(filePath1);
                cc_final = cc_final.Replace("'", "\"");
                w1.WriteLine(cc_final);
                //TextWriter tw1 = File.CreateText(@"c:\TEJ_ERP\WTEWAYBILL.JSON");
                if (FOPT == "WEBT")
                {
                    w1.Flush();
                    w1.Close();
                    fgen.msg("", "ASMG", "WEBTEL JSON File Generated AT c:\\TEJ_erp\\WTEINVWeb.JSON");
                }
            }
        }
    }

    public string MakeWebRequest(string method, string url, string post_data)
    {

        string a = "", responseString = "";
        // Response.Write("http://www.808.dk/", "GET", ""));

        //var request = (HttpWebRequest)WebRequest.Create(url);


        //var data = Encoding.ASCII.GetBytes(post_data);

        //request.Method = "POST";
        //request.ContentType = "application/json";
        //request.ContentLength = data.Length;


        //using (var stream = request.GetRequestStream())
        //{
        //    stream.Write(data, 0, data.Length);
        //}

        //var response = (HttpWebResponse)request.GetResponse();

        //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        return responseString;
    }

    protected void command6_ServerClick(object sender, EventArgs e)
    {
        if (txtvchnum.Text.Trim().Length < 6)
        {
            fgen.msg("", "ASMG", "Please Press New to Start");
            return;
        }
        if (hfdemo.Value == "Y")
        { demo_einv = "Y"; }
        else
        {
            fgen.msg("", "ASMG", "Demo Einvoice disabled.Please Contact Admin.");
            demo_einv = "N";
            return;
        }
        if (web_einv_ok == "N")
        {
            fgen.msg("", "ASMG", "Please Get Webtel Demo Utility Activated");
            return;
        }
        else
        {
            hffield.Value = "demo";
            fgen.msg("-", "CMSG", "Are You Sure!! Demo IRN will be generated!!");
        }
    }

    public void upd_addl_sal_exp(string inv_Refnum)
    {
        DataTable rsitms = new DataTable();
        DataTable rsitms1 = new DataTable();
        DataTable rs = new DataTable();
        fgen.execute_cmd(frm_qstr, frm_cocd, "update item set tax_item='Y' where icode like '59%' and trim(icode) in (Select trim(icode) from ivoucher where trim(branchcd)||type||vchnum||to_char(Vchdate,'dd/mm/yyyy')='" + inv_Refnum + "' and iqtyout=0 and icode like '59%')");
        fgen.execute_cmd(frm_qstr, frm_cocd, "commit");
        SQuery = "Select a.branchcd||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,sum(a.iamount) as TotExp,sum(nvl(a.exc_amt,0)) as Tottx1,sum(nvl(a.cess_pu,0)) as Tottx2 from ivoucher a, famst b,item c where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(B.acode) and trim(a.branchcd)||a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy')='" + inv_Refnum + "' and nvl(c.tax_item,'-')='Y' group by a.branchcd||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy') order by a.branchcd||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')";
        rsitms = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        col1 = "Select a.branchcd||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.exp_punit,a.rej_rw,a.rej_sdv from ivoucher a, famst b where trim(a.acode)=trim(B.acode) and  trim(a.branchcd)||a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy')='" + inv_Refnum + "' and a.morder=1 and (nvl(a.exp_punit,0)=0 or nvl(a.rej_Rw,0)=0)  and a.branchcd||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy') in (Select a.branchcd||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy') as fstr from ivoucher a, famst b,item c where trim(a.acode)=trim(B.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)||a.type||a.vchnum||to_char(a.Vchdate,'dd/mm/yyyy')='" + inv_Refnum + "' and nvl(c.tax_item,'-')='Y' ) order by a.branchcd||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')";
        rsitms1 = fgen.getdata(frm_qstr, frm_cocd, col1);
        for (int x = 0; x < rsitms1.Rows.Count; x++)
        {
            DataView dv1 = new DataView(rsitms, "FSTR='" + rsitms1.Rows[x]["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
            rs = dv1.ToTable();
            for (int o = 0; o < rs.Rows.Count; o++)
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE ivoucher SET exp_punit='" + fgen.make_double(rs.Rows[o]["totexp"].ToString().Trim()) + "',rej_rw='" + fgen.make_double(rs.Rows[o]["Tottx1"].ToString().Trim()) + "',rej_sdv='" + fgen.make_double(rs.Rows[o]["Tottx2"].ToString().Trim()) + "' where branchcd||type||vchnum||to_char(Vchdate,'dd/mm/yyyy')='" + rs.Rows[o]["fstr"].ToString().Trim() + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "commit");
            }
        }
    }
    protected void command3_ServerClick(object sender, EventArgs e)
    {
        fgen.Fn_open_prddmp1("", frm_qstr);
        //fgen.msg("-", "CMSG", "1.Invoice '13' 2.Dr/Cr Note");
        hffield.Value = "LIST_IRN";
    }
    protected void command7_ServerClick(object sender, EventArgs e)
    {
        string g_uid, g_pwd, g_zip, g_efuuid, g_efupwd, g_efukey, g_api_link, res, v_gstin, cc_string, vg_irn;
        if (Convert.ToInt32(frm_ulvl) < 2)
        {
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, "select gstewb_id,gstewb_pw,zipcode,gstefu_id,gstefu_pw,gstefu_cdkey,irn_apiadd,upper(trim(gst_no)) as aa from type where id='B' and type1='" + frm_mbr + "'");

            g_uid = dt.Rows[0]["gstewb_id"].ToString();
            g_pwd = dt.Rows[0]["gstewb_pw"].ToString();
            g_zip = dt.Rows[0]["zipcode"].ToString();

            g_efuuid = dt.Rows[0]["gstefu_id"].ToString();
            g_efupwd = dt.Rows[0]["gstefu_pw"].ToString();
            g_efukey = dt.Rows[0]["gstefu_cdkey"].ToString();
            g_api_link = dt.Rows[0]["irn_apiadd"].ToString().Trim();
            v_gstin = dt.Rows[0]["aa"].ToString();

            if (g_api_link.Length < 10)
            {
                fgen.msg("-", "AMSG", "Portal API not linked in Plant Master , Please contact Administrator");
                return;
            }

            for (int i = 0; i < sg1.Rows.Count; i++)
            {
                vg_irn = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text;
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Length == 64 && ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Length < 10)
                {
                    g_api_link = g_api_link.ToUpper().Replace("/GENIRN", "/GetEInvoiceByIRN");
                    cc_string = "{'Push_Data_List': { 'Data': [{'IRN': '" + vg_irn + "','GSTIN': '" + v_gstin + "','CDKey': '" + g_efukey + "','EInvUserName': '" + g_uid + "','EInvPassword': '" + g_pwd + "','EFUserName': '" + g_efuuid + "','EFPassword': '" + g_efupwd + "'}";
                    cc_string = cc_string.Replace("^", " ");
                    res = MakeWebRequest("POST", g_api_link, cc_string);
                    string AA, BB;
                    AA = res.IndexOf("ErrorMessage").ToString();
                    BB = "}]";
                    string my_qrcd = ""; string ee = ""; string ff = ""; string irnd = ""; string vackno = ""; string vackdt = "", MY_INS_NO;
                    var dicddd = new Dictionary<string, object>();
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    ArrayList itemss = jss.Deserialize<ArrayList>(res);
                    foreach (var value in itemss)
                    {
                        dicddd = ((Dictionary<string, object>)itemss[0]);
                        foreach (var dc in dicddd)
                        {
                            if (dc.Key.ToString() == "DocDate")
                            {
                                AA = dc.Value.ToString();
                            }
                            if (dc.Key.ToString() == "AckDate")
                            {
                                BB = dc.Value.ToString();
                            }
                            if (dc.Key.ToString() == "SignedQRCode")
                            {
                                ee = dc.Value.ToString();
                            }
                            if (dc.Key.ToString() == "SignedInvoice")
                            {
                                ff = dc.Value.ToString();
                            }
                            if (dc.Key.ToString() == "Irn")
                            {
                                irnd = dc.Value.ToString();
                            }
                            if (dc.Key.ToString() == "AckNo")
                            {
                                vackno = dc.Value.ToString();
                            }

                        }

                    }
                    //AA = res.IndexOf("DocDate").ToString();
                    //BB = res.IndexOf("AckDate").ToString();
                    //ee = res.IndexOf("SignedQRCode").ToString();
                    //ff = res.IndexOf("SignedInvoice").ToString();
                    if (ee == "0" || ff == "0") MY_INS_NO = res;
                    else
                    {
                        my_qrcd = ee;
                        vackdt = BB;
                        //my_qrcd = res.Substring(Convert.ToInt16(ee) + 15, (Convert.ToInt16(ff) - 21) - (Convert.ToInt16(ee) - 3));
                        //vackdt = res.Substring(Convert.ToInt16(BB) + 10, 19);
                        //vackno = res.Substring(Convert.ToInt16(BB) + 39, 15);
                        //MY_INS_NO = res.Substring(Convert.ToInt16(AA) + 22, 104);
                    }
                    if (ee == "0" || ff == "0")
                    {
                        fgen.msgBig(frm_qstr, "-", "AMSG", "" + res + "");
                        return;
                    }
                    if (vg_irn.Trim().Length == 64 && my_qrcd.Trim().Length >= 100)
                    {

                        ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text = my_qrcd;
                        //((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text = my_qrcd;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text = vackno;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text = vackdt;
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text = "A";
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).BackColor = System.Drawing.Color.LimeGreen;
                    }
                    else
                    {
                        fgen.msgBig(frm_qstr, "-", "AMSG", "For Line No. " + i + 1 + "'13' Message from Einvoice Portal '13' " + res + "");
                        AA = "Chk Data";
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).BackColor = System.Drawing.Color.Cyan;
                    }

                }
            }
        }
    }

    public void fillLog(string msg)
    {
        string ppath = @"c:\TEJ_ERP\jsonLog1.txt";
        try
        {
            if (File.Exists(ppath))
            {
                StreamWriter w = File.AppendText(ppath);
                w.WriteLine(msg.ToString() + "-->" + DateTime.Now.ToString("ddMMyyyy hh:mm:ss tt"));
                w.WriteLine("=====================================================================");
                w.Flush();
                w.Close();
            }
            else
            {
                StreamWriter w = new StreamWriter(ppath, true);
                w.WriteLine(msg.ToString() + "-->" + DateTime.Now.ToString("ddMMyyyy hh:mm:ss tt"));
                w.WriteLine("=====================================================================");
                w.Flush();
                w.Close();
            }
        }
        catch { }
    }

    void do_upd_tran_file(string upd_tb, int row_nm)
    {
        DataSet odsS = new DataSet();
        DataRow oporows = null;
        DataTable rssample1 = new DataTable();


        string key_Str = "";
        key_Str = frm_mbr + sg1.Rows[row_nm].Cells[16].Text + sg1.Rows[row_nm].Cells[17].Text + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t1")).Text + sg1.Rows[row_nm].Cells[15].Text;


        SQuery = "select branchcd||TRIM(doc_type)||trim(doc_no)||to_char(doc_Dt,'dd/mm/yyyy')||trim(acode) as fstr,irn_no,irnqr_1,irnqr_2,ack_no,ack_dt,irn_stat,eway_bill from EINV_REC where branchcd='" + frm_mbr + "' and vchnum='" + txtvchnum.Text + "' and vchdate =to_DatE('" + txtvchdate.Text + "','dd/mm/yyyy') and branchcd||trim(doc_type)||trim(doc_no)||to_char(doc_dt,'dd/mm/yyyy')||trim(acode)='" + key_Str + "' order by branchcd||doc_type||trim(doc_no)||to_char(doc_Dt,'dd/mm/yyyy')||acode";
        rssample1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (rssample1.Rows.Count > 0)
        {
            for (int x = 0; x < rssample1.Rows.Count; x++)
            {
                //, eway_bill='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t19")).Text + "'
                SQuery = "UPDATE EINV_REC SET IRN_NO='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t8")).Text + "' , irnqr_1='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t14")).Text + "', irnqr_2='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t15")).Text + "', ack_no='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t16")).Text + "', ack_dt='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t17")).Text + "', irn_stat='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t18")).Text + "' WHERE branchcd||trim(doc_type)||trim(doc_no)||to_char(doc_dt,'dd/mm/yyyy')||trim(acode)='" + rssample1.Rows[x]["fstr"].ToString().Trim() + "' ";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
            }
            DataTable rssample = new DataTable();
            if (upd_tb.ToUpper() == "IVOUCHER")
            {
                //& Trim(sg.text(row_nm, -2)) &
                SQuery = "select trim(branchcd)||trim(type)||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy')||trim(acode) as fstr,gstvchnum from " + upd_tb + " where  trim(branchcd)||trim(type)||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy')||trim(acode)='" + key_Str + "'";
            }
            else
            {
                //& Trim(sg.text(row_nm, -2)) &
                SQuery = "select trim(branchcd)||trim(type)||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy')||trim(acode) as fstr,einv_no,st_entform from " + upd_tb + " where  trim(branchcd)||trim(type)||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy')||trim(acode)='" + key_Str + "' ";
            }
            rssample = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (rssample.Rows.Count > 0)
                for (int x = 0; x < rssample.Rows.Count; x++)
                {
                    DataView dv = new DataView(rssample1, "FSTR='" + rssample.Rows[x]["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                    if (rssample.Rows.Count > 0)
                    {
                        for (int o = 0; o < dv.Count; o++)
                        {
                            if (upd_tb.ToUpper() == "IVOUCHER")
                            {
                                SQuery = "UPDATE IVOUCHER SET GSTVCHNUM='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t8")).Text + "' WHERE branchcd||TRIM(type)||TRIM(vchnum)||to_char(Vchdate,'dd/mm/yyyy')||trim(acode)='" + key_Str + "' ";
                                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                            }
                            else
                            {
                                // fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE EINV_REC SET einv_no='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t8")).Text + "' WHERE branchcd||type||vchnum||to_char(Vchdate,'dd/mm/yyyy')||trim(acode)='" + rssample.Rows[o]["fstr"].ToString().Trim() + "' ");
                                SQuery = "UPDATE SALE SET einv_no='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t8")).Text + "' WHERE branchcd||TRIM(type)||TRIM(vchnum)||to_char(Vchdate,'dd/mm/yyyy')||trim(acode)='" + key_Str + "' ";
                                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                if (rssample.Rows[x]["st_entform"].ToString().Trim().Length < 3 && make_ewayb == "Y")
                                {
                                    SQuery = "UPDATE SALE SET st_entform='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t19")).Text + "' WHERE branchcd||TRIM(type)||TRIM(vchnum)||to_char(Vchdate,'dd/mm/yyyy')||trim(acode)='" + key_Str + "' ";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                }
                            }
                        }
                    }
                }
        }

    }


    protected void btnGridPop_Click(object sender, EventArgs e)
    {
        if (hf1.Value.Contains("sg1_t11_"))
        {
            hffield.Value = "sg1_t11";
            hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_sg1_t11_", "");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Transport", frm_qstr);
        }
    }
}

//command4 - btnjson_ServerClick
//command1 - btnshow_ServerClick
//command6 - demo
//web_einv_ok = "N"  - have to change later
//comp_vehi_no - have to change later

//[{"ErrorMessage":"","ErrorCode":"","Status":"1","GSTIN":"09AAACW3775F015","DocNo":"2122-02-200003","DocType":"INV","DocDate":"31/03/2021","Irn":"b7734ce17c219fd4f0b0c3806a0e65fe8cd1a0d4feabd92736fe8ef3bff5b51c","AckDate":"2021-09-21 14:26:27","AckNo":142110006857980,"EwbNo":null,"EwbDt":null,"EwbValidTill":null,"SignedQRCode":"eyJhbGciOiJSUzI1NiIsImtpZCI6IkVEQzU3REUxMzU4QjMwMEJBOUY3OTM0MEE2Njk2ODMxRjNDODUwNDciLCJ0eXAiOiJKV1QiLCJ4NXQiOiI3Y1Y5NFRXTE1BdXA5NU5BcG1sb01mUElVRWMifQ.eyJkYXRhIjoie1wiU2VsbGVyR3N0aW5cIjpcIjA5QUFBQ1czNzc1RjAxNVwiLFwiQnV5ZXJHc3RpblwiOlwiMDJBQUZDQTMxNTRNMlpVXCIsXCJEb2NOb1wiOlwiMjEyMi0wMi0yMDAwMDNcIixcIkRvY1R5cFwiOlwiSU5WXCIsXCJEb2NEdFwiOlwiMzEvMDMvMjAyMVwiLFwiVG90SW52VmFsXCI6ODQwNjMwLjAsXCJJdGVtQ250XCI6NSxcIk1haW5Ic25Db2RlXCI6XCI0ODE5MTAxMFwiLFwiSXJuXCI6XCJiNzczNGNlMTdjMjE5ZmQ0ZjBiMGMzODA2YTBlNjVmZThjZDFhMGQ0ZmVhYmQ5MjczNmZlOGVmM2JmZjViNTFjXCIsXCJJcm5EdFwiOlwiMjAyMS0wOS0yMSAxNDoyNjoyN1wifSIsImlzcyI6Ik5JQyJ9.r1Z0ViDU4o3tvGIBmdjzq5nfJlM1UC-gsodSU2T3HTIaquIUx5FZld_b0Uj3D9w0a6LBRWLLorqHERJGBuRxqocPIoiF4WVksd7OmCIJpS38uG1GfDIeXp0GbwEkwCWUgkMW2-TRS6OqliCHUer-hPFviyNHxei-xD_rb95deAijb0XJdmzT3M88688gVgara8QqAxZse28c8U9IrgP2w5u8KDKdq_T3FabK1Tcrwjueq720ZNeKOyyhHux2C34LWah3Jseq6jYef8tf_AC18rtD-zL56utnm4LaXjgyAG26bRFjqnGXRxe2W2yQj814vLQVzU1gZK8stLCqjoXXag","SignedInvoice":"eyJhbGciOiJSUzI1NiIsImtpZCI6IkVEQzU3REUxMzU4QjMwMEJBOUY3OTM0MEE2Njk2ODMxRjNDODUwNDciLCJ0eXAiOiJKV1QiLCJ4NXQiOiI3Y1Y5NFRXTE1BdXA5NU5BcG1sb01mUElVRWMifQ.eyJkYXRhIjoie1wiQWNrTm9cIjoxNDIxMTAwMDY4NTc5ODAsXCJBY2tEdFwiOlwiMjAyMS0wOS0yMSAxNDoyNjoyN1wiLFwiSXJuXCI6XCJiNzczNGNlMTdjMjE5ZmQ0ZjBiMGMzODA2YTBlNjVmZThjZDFhMGQ0ZmVhYmQ5MjczNmZlOGVmM2JmZjViNTFjXCIsXCJWZXJzaW9uXCI6XCIxLjFcIixcIlRyYW5EdGxzXCI6e1wiVGF4U2NoXCI6XCJHU1RcIixcIlN1cFR5cFwiOlwiQjJCXCIsXCJSZWdSZXZcIjpcIk5cIixcIklnc3RPbkludHJhXCI6XCJOXCJ9LFwiRG9jRHRsc1wiOntcIlR5cFwiOlwiSU5WXCIsXCJOb1wiOlwiMjEyMi0wMi0yMDAwMDNcIixcIkR0XCI6XCIzMS8wMy8yMDIxXCJ9LFwiU2VsbGVyRHRsc1wiOntcIkdzdGluXCI6XCIwOUFBQUNXMzc3NUYwMTVcIixcIkxnbE5tXCI6XCJNRUVSVVQgUEFDS0FHSU5HIElORFVTVFJJRVNcIixcIlRyZE5tXCI6XCJNRUVSVVQgUEFDS0FHSU5HIElORFVTVFJJRVNcIixcIkFkZHIxXCI6XCJQTE9UIE5PLTUgU0VDVE9SLTMsU0hBVEFCREkgTkFHQVIsSU5ETC5BUkVBIFBBUlRBUFVSIDI1MDEwMyAoVS5QLilcIixcIkFkZHIyXCI6XCJBVCBLSEFTUkEgTk8tNzQwLTc0MSw3NDUgXFx1MDAyNiBPVEhFUlMsVklMTC1NQVNPT1JJIE1BV0FOQVwiLFwiTG9jXCI6XCJNRUVSVVRcIixcIlBpblwiOjI1MDEwMyxcIlN0Y2RcIjpcIjA5XCIsXCJQaFwiOlwiNzIxNzAxMDA1NFwiLFwiRW1cIjpcIklORk9ATUVFUlVUUEFDS0FHSU5HLkNPTVwifSxcIkJ1eWVyRHRsc1wiOntcIkdzdGluXCI6XCIwMkFBRkNBMzE1NE0yWlVcIixcIkxnbE5tXCI6XCJBTFBMQSBJTkRJQSBQVlQgTFREIFVOSVQgLSAwMlwiLFwiVHJkTm1cIjpcIkFMUExBIElORElBIFBWVCBMVEQgVU5JVCAtIDAyXCIsXCJQb3NcIjpcIjAyXCIsXCJBZGRyMVwiOlwiVklMTEFHRS0gS0FVTkRJICwgUE9TVCBPRkZJQ0UtIEJBRERJLFwiLFwiQWRkcjJcIjpcIlRFSFNJTC0gTkFMQUdBUkggLCBESVNUUklDVC0gU09MQU4sXCIsXCJMb2NcIjpcIk1FRVJVVFwiLFwiUGluXCI6MTczMjA1LFwiUGhcIjpcIjkzMTI0NTY3ODhcIixcIkVtXCI6XCJwcmFtb2Quam9zaGlAYWxwbGEuY29tXCIsXCJTdGNkXCI6XCIwMlwifSxcIkl0ZW1MaXN0XCI6W3tcIkl0ZW1Ob1wiOjAsXCJTbE5vXCI6XCIxXCIsXCJJc1NlcnZjXCI6XCJOXCIsXCJQcmREZXNjXCI6XCJDT1JSIEJPWCBIQVJQQyxJTixCT1RPVFItVEM2MDBYMy0yNCBQQUNLIDMxNjYwNzJcIixcIkhzbkNkXCI6XCI0ODE5MTAxMFwiLFwiUXR5XCI6NTAwMC4wLFwiRnJlZVF0eVwiOjAuMCxcIlVuaXRcIjpcIk5PU1wiLFwiVW5pdFByaWNlXCI6MzAuMCxcIlRvdEFtdFwiOjE1MDAwMC4wLFwiQXNzQW10XCI6MTUwMDAwLjAsXCJHc3RSdFwiOjEyLjAsXCJJZ3N0QW10XCI6MTgwMDAuMCxcIlRvdEl0ZW1WYWxcIjoxNjgwMDAuMH0se1wiSXRlbU5vXCI6MCxcIlNsTm9cIjpcIjJcIixcIklzU2VydmNcIjpcIk5cIixcIlByZERlc2NcIjpcIkNPTElOIDI1MCBNTCAoTkVXKVwiLFwiSHNuQ2RcIjpcIjQ4MTkxMDEwXCIsXCJRdHlcIjo1MDAwLjAsXCJGcmVlUXR5XCI6MC4wLFwiVW5pdFwiOlwiTk9TXCIsXCJVbml0UHJpY2VcIjozMC4wLFwiVG90QW10XCI6MTUwMDAwLjAsXCJBc3NBbXRcIjoxNTAwMDAuMCxcIkdzdFJ0XCI6MTIuMCxcIklnc3RBbXRcIjoxODAwMC4wLFwiVG90SXRlbVZhbFwiOjE2ODAwMC4wfSx7XCJJdGVtTm9cIjowLFwiU2xOb1wiOlwiM1wiLFwiSXNTZXJ2Y1wiOlwiTlwiLFwiUHJkRGVzY1wiOlwiQk9UVE9NIFBMQVRFIERFVFRPTCBFQVJUSCBTT0FQIDc1RyBCM0cxIChXSVRIT1VUIFNLSUxMRVQpIC0gMzEyNzg3OFwiLFwiSHNuQ2RcIjpcIjQ4MTkxMDEwXCIsXCJRdHlcIjo1MDAwLjAsXCJGcmVlUXR5XCI6MC4wLFwiVW5pdFwiOlwiTk9TXCIsXCJVbml0UHJpY2VcIjozMC4wLFwiVG90QW10XCI6MTUwMDAwLjAsXCJBc3NBbXRcIjoxNTAwMDAuMCxcIkdzdFJ0XCI6MTIuMCxcIklnc3RBbXRcIjoxODAwMC4wLFwiVG90SXRlbVZhbFwiOjE2ODAwMC4wfSx7XCJJdGVtTm9cIjowLFwiU2xOb1wiOlwiNFwiLFwiSXNTZXJ2Y1wiOlwiTlwiLFwiUHJkRGVzY1wiOlwiQkxFTkRFUiBQUklERSAtIDc1MCBNTCAgUVRZIC0yMFwiLFwiSHNuQ2RcIjpcIjQ4MTkxMDEwXCIsXCJRdHlcIjo1MDAwLjAsXCJGcmVlUXR5XCI6MC4wLFwiVW5pdFwiOlwiTk9TXCIsXCJVbml0UHJpY2VcIjozMC4wLFwiVG90QW10XCI6MTUwMDAwLjAsXCJBc3NBbXRcIjoxNTAwMDAuMCxcIkdzdFJ0XCI6MTIuMCxcIklnc3RBbXRcIjoxODAwMC4wLFwiVG90SXRlbVZhbFwiOjE2ODAwMC4wfSx7XCJJdGVtTm9cIjowLFwiU2xOb1wiOlwiNVwiLFwiSXNTZXJ2Y1wiOlwiTlwiLFwiUHJkRGVzY1wiOlwiQkxBREUgRklMTEVSIFlPUktFUiBLUENHSEZDQjAxMzhcIixcIkhzbkNkXCI6XCI0ODE5MTAxMFwiLFwiUXR5XCI6NTAwMC4wLFwiRnJlZVF0eVwiOjAuMCxcIlVuaXRcIjpcIk5PU1wiLFwiVW5pdFByaWNlXCI6MzAuMCxcIlRvdEFtdFwiOjE1MDAwMC4wLFwiQXNzQW10XCI6MTUwMDAwLjAsXCJHc3RSdFwiOjEyLjAsXCJJZ3N0QW10XCI6MTgwMDAuMCxcIlRvdEl0ZW1WYWxcIjoxNjgwMDAuMH1dLFwiVmFsRHRsc1wiOntcIkFzc1ZhbFwiOjc1MDAwMC4wLFwiQ2dzdFZhbFwiOjAuMCxcIlNnc3RWYWxcIjowLjAsXCJJZ3N0VmFsXCI6OTAwMDAuMCxcIkNlc1ZhbFwiOjAuMCxcIlN0Q2VzVmFsXCI6MC4wLFwiT3RoQ2hyZ1wiOjYzMC4wLFwiVG90SW52VmFsXCI6ODQwNjMwLjB9fSIsImlzcyI6Ik5JQyJ9.RkR-OdrTi72rtuUxj6bANPdd9ey-MFXeQVEOrTDXw_TWVgywvF1fYfclwTV9cJvaPR-Q3Ei9BMxZADC8lu1ZXdKRqkCb067g0Qyx_9Ahk93QZT0Gcu20QxUBdAOqJonf7PavDwQ6girv6CX12S_G5hesQrl6r3yd4jzm9BE93ea96syx5wNFk_3BQs4LaOLJAEB3n9o7igpIkGGW6RGYAZ398TF5CFfrnnx0uV-m9Bgw01xxPGGJL0e94rieMLDFP5NtR4yiv_cRgbNq5hxfzwEcmoP7RseJvtsoUzr_UINvJ3eo235wpj4QX3kou3xQqoJlrne2QRfbCk1TG82Wcg","IrnStatus":"ACT","InfoDtls":null,"Remarks":null,"UniqueKey":""}]