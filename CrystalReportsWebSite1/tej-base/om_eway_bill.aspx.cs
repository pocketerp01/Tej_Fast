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
using Newtonsoft.Json.Linq;
//EWAY  ICON ID

public partial class om_eway_bill : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, col4, col5, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string web_Tel_ok = "N", ERP_M337_long_Invno = "Y";// has to be changed later
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName, CLIENTGRP = "", web_eway_ok = "N";
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    string flag = "";
    string used_opt = "";


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
                if (frm_cocd == "GGRP" || frm_cocd == "MPAC" || frm_cocd == "PGTL" || frm_cocd == "ROYL" || frm_cocd == "MAST" || frm_cocd == "MASS")
                { hf_eway_ok.Value = "Y"; }
                else
                { hf_eway_ok.Value = "N"; }

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();

            typePopup = "N";
        }
        web_eway_ok = hf_eway_ok.Value;
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
        showbtn.Disabled = true; command2.Disabled = true; jsonbtn.Disabled = true;
        create_tab();
        create_tab2();
        create_tab3();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();

        btnlbl4.Enabled = false;
        btnlbl7.Enabled = false;




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
        showbtn.Disabled = false; command2.Disabled = false; jsonbtn.Disabled = false;
        btnlbl4.Enabled = true;
        btnlbl7.Enabled = true;
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
        frm_tabname = "ewayb_rec";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        //switch (Prg_Id)
        //{
        //    case "F30111":
        //        SQuery = "SELECT '20' AS FSTR,'Quality Inward Certificate' as NAME,'20' AS CODE FROM dual";
        //        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "20");
        //        break;
        //    case "F30112":
        //        SQuery = "SELECT '40' AS FSTR,'Quality In-proc Certificate' as NAME,'40' AS CODE FROM dual";
        //        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "40");
        //        break;
        //    case "F30113":
        //        SQuery = "SELECT '10' AS FSTR,'Quality Outward Certificate' as NAME,'10' AS CODE FROM dual";
        //        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        //        break;

        //}
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "EW");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
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
                //SQuery = "SELECT distinct a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum||trim(a.icode)||trim(a.srno) AS FSTR,trim(a.Vchnum) as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,c.Iname,b.Aname as Supplier,a.Invno,A.Refnum as chl_no from ivoucher a ,famst b,item c where trim(A.icode)=trim(c.icode) and trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + DateRange + " and NVL(a.inspected,'N')='N' order by a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum||trim(a.icode)||trim(a.srno)";
                SQuery = "SELECT type1 as fstr,name as grade_name,Type1 as Grade_Code  from type where id='I' and type1 like '0%'";
                break;
            case "MRESULT":
                SQuery = "SELECT '01' as fstr,'ACCEPTED' as Results,'01' as Qa_Code from dual union all SELECT '02' as fstr,'REJECTED' as Results,'02' as Qa_Code from dual union all SELECT '03' as fstr,'ACCEPT U/Dev.' as Results,'03' as Qa_Code from dual union all SELECT '04' as fstr,'ACCEPT U/Seg.' as Results,'04' as Qa_Code from dual";
                break;

            case "DOCTYPE":
                SQuery = "SELECT 'invoice' AS FSTR,'invoice' as type,'invoice' as Name FROM dual union all  SELECT 'CHL' AS FSTR,'Dlv Chl.' as type,'Dlv Chl' as Name FROM dual";
                // SQuery = "SELECT 'invoice' AS FSTR,'invoice' as type FROM dual union all  SELECT 'Dlvchl' AS FSTR,'Dlvchl' as type FROM dual";
                break;

            case "TICODE":

                //Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                //string pquery;
                //switch (Prg_Id)
                //{
                //    case "F30101":
                //        pquery = "select trim(icode) as icode,sum(cnt) as tot from (select icode,1 as cnt from item where length(trim(nvl(deac_by,'-')))<=1 and length(trim(icode))>4 union all select distinct icode,-1 as cnt from inspmst where branchcd!='DD' and type='" + lbl1a.Text + "') group by trim(icode) having sum(cnt)>0 ";
                //        SQuery = "SELECT a.Icode AS FSTR,trim(b.Iname) as Item_name,a.Icode,b.Cpartno,b.Cdrgno,b.unit from ("+ pquery +")a ,Item b where trim(A.icode)=trim(B.icode) and length(trim(nvl(b.deac_by,'-')))<=1 and length(trim(b.icode))>4 and substr(b.icode,1,1) < ('9') order by b.iname";
                //        break;
                //    case "F30106":
                //        pquery = "select trim(icode) as icode,sum(cnt) as tot from (select icode,1 as cnt from item where length(trim(nvl(deac_by,'-')))<=1 and length(trim(icode))>4 union all select distinct icode,-1 as cnt from inspmst where branchcd!='DD' and type='" + lbl1a.Text + "') group by trim(icode) having sum(cnt)>0 ";
                //        SQuery = "SELECT a.Icode AS FSTR,trim(b.Iname) as Item_name,a.Icode,b.Cpartno,b.Cdrgno,b.unit from ("+ pquery +")a ,Item b where trim(A.icode)=trim(B.icode) and length(trim(nvl(b.deac_by,'-')))<=1 and length(trim(b.icode))>4 and substr(b.icode,1,1) in ('7','9') order by b.iname";
                //        break;
                //    case "F30111":
                //        pquery = "select trim(icode) as icode,sum(cnt) as tot from (select icode,1 as cnt from item where length(trim(nvl(deac_by,'-')))<=1 and length(trim(icode))>4 union all select distinct icode,-1 as cnt from inspmst where branchcd!='DD' and type='" + lbl1a.Text + "' and trim(Acode)='" + txtlbl4.Text.Trim() + "') group by trim(icode) having sum(cnt)>0 ";
                //        SQuery = "SELECT a.Icode AS FSTR,trim(b.Iname) as Item_name,a.Icode,b.Cpartno,b.Cdrgno,b.unit from ("+ pquery +")a ,Item b where trim(A.icode)=trim(B.icode) and length(trim(nvl(b.deac_by,'-')))<=1 and length(trim(b.icode))>4 and substr(b.icode,1,1) in ('7','8','9') order by b.iname";
                //        break;
                //}

                break;
            case "sg1_t11":
                SQuery = "select * from (select Acode,ANAME as Transporter,Acode as Code,gst_no as transp_id,Addr1 as Address,Addr2 as City from famst  where upper(ccode)='T' union all select 'Own' as Acode,'OWN' as Transporter,'-' as Code,'-' as transp_id,'-' as Address,'-' as City from dual union all select 'party' as acode,'PARTY VEHICLE' as Transporter,'-' as Code,'-' as trans_id,'-' as Address,'-' as City from dual) order by  Transporter";
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":

                SQuery = "select name as fstr, name as District ,type1 as code,acref from typegrp where id='DT' order by name";
                //SQuery = "SELECT userid AS FSTR,Full_Name AS Client_Name,username as CCode FROM evas where branchcd!='DD' and username!='-' and userid>'000052' and trim(userid) not in (select trim(Ccode) from wb_oms_log where branchcd!='DD' and to_char(opldt,'yyyymm')=to_char(to_DaTE('" + txtvchdate.Text  + "','dd/mm/yyyy'),'yyyymm')) order by Username";
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


                    SQuery = "select trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') as fstr,vchnum||'  '||trim(ent_by) as  Doc_dtl,vchdate as Vch_Date,count(*) as Documents,to_chaR(ent_dt,'dd/mm/yyyy') as entry_Dt,vchnum from ewayb_rec where VCHDATE " + DateRange + " AND type='" + frm_vty + "' and branchcd='" + frm_mbr + "' group by vchnum||'  '||trim(ent_by),vchnum,vchdate,to_chaR(ent_dt,'dd/mm/yyyy'),trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') order by vchdate desc ,vchnum desc";
                //SQuery = "select  MTHNUM AS FSTR,MTHNAME AS MONTH_NAME ,MTHNUM AS MONTH FROM MTHS ORDER BY MTHNUM";
                //SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Report_no,to_char(a.vchdate,'dd/mm/yyyy') as Report_Dt,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' order by vdd desc,a.vchnum desc";
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

            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

            switch (Prg_Id)
            {
                case "F30111":
                    frm_vty = "20";
                    break;
                case "F30112":
                    frm_vty = "40";
                    break;
                case "F30113":
                    frm_vty = "10";
                    break;
                case "F50053":
                    frm_vty = "EW";
                    break;

            }
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
        txtlbl2.Text = frm_uname;
        txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl5.Text = "-";
        txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        //CODE TO FILL PLACE

        txtlbl4.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no,place,zipcode from type where id='B' and type1='" + frm_mbr + "'", "place");
        txtlbl7.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no,place,zipcode from type where id='B' and type1='" + frm_mbr + "'", "zipcode");

        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();

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
        string chk_freeze = "";
        //chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1043", txtvchdate.Text.Trim());
        if (chk_freeze == "1")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Rolling Freeze Date !!");
            return;
        }
        if (chk_freeze == "2")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Fixed Freeze Date !!");
            return;
        }

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }


        //if (txtlbl101.Text == "-" || txtlbl101.Text == "" )
        //{

        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + " Please Fill Result");
        //    return;
        //}

        //string mandField = "";
        //mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        //if (mandField.Length > 1)
        //{
        //    fgen.msg("-", "AMSG", mandField);
        //    return;
        //}

        if (sg1.Rows[0].Cells[17].Text.Length < 2)
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
        else if (hffield.Value == "PrintWay")
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
            col5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

            if (col5.Trim().Length > 10)
            {

                col5 = col5.Trim().Replace('-', ' ').Trim();
                string URLPRINT = "https://ewayapi.mygstcafe.com/managed/v1.03/DetailedPrint?ewbNumber=" + col5;
                var DATA = MakeWebRequestPrint(URLPRINT);

                Random rnd = new Random();
                var t = col5 + "_" + rnd.Next(100000, 999999).ToString();
                string fileName = "c:\\TEJ_erp\\UPLOAD\\WTEWAYBILL_Print_" + t + ".pdf";

                using (var stream = File.Create(fileName))
                {
                    DATA.GetResponseStream().CopyTo(stream);
                }

                Byte[] bytes = File.ReadAllBytes(fileName);
                String file = Convert.ToBase64String(bytes);
                hf_filename.Value = "WTEWAYBILL_" + t + ".pdf";
                hf_filebase.Value = file;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "downloadme();", true);

            }
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
                        txtlbl4a.Text = dt.Rows[i]["text"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[i]["frm_header"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["ent_id"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["ent_by"].ToString().Trim();
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
                case "sg1_t11":
                    if (col1.Length > 1)
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = col2;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t12")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t13")).Text = col1;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t12")).Focus();
                    }
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
                    if (col1 == "") return;
                    clearctrl();
                    //SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    //SQuery = "SELECT A.VCHNUM,A.VCHDATE,A.GRADE,A.EMPCODE,A.TIMEINHR,A.TIMEINMIN,A.TIMEOUTHR,A.TIMEOUTMIN,A.HRWRK,A.MINWRK,B.NAME,B.DEPTT_TEXT,B.DESG_TEXT,B.DTJOIN FROM ATTN A ,EMPMAS B WHERE TRIM(A.EMPCODE)=TRIM(B.EMPCODE) AND TRIM(A.GRADE)=TRIM(B.GRADE) AND TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + col1 + "'";
                    //   SQuery = "SELECT A.VCHNUM,A.VCHDATE,A.GRADE,A.EMPCODE,A.SRNO,A.TIMEINHR,A.TIMEINMIN,A.TIMEOUTHR,A.TIMEOUTMIN,A.HRWRK,A.MINWRK,A.ENT_BY,A.ENT_DT,B.NAME,B.DEPTT_TEXT,B.DESG_TEXT,B.DTJOIN FROM ATTN A ,EMPMAS B WHERE TRIM(A.EMPCODE)=TRIM(B.EMPCODE) AND TRIM(A.GRADE)=TRIM(B.GRADE) AND TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND  a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    SQuery = "SELECT  * from " + frm_tabname + "  where branchcd||type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY SRNO";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");


                        txtlbl2.Text = dt.Rows[i]["ent_by"].ToString().Trim();
                        txtlbl3.Text = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString().Trim()).ToString("dd/MM/yyyy");

                        // txtlbl5.Text = dt.Rows[i]["btchno"].ToString().Trim();
                        // txtlbl6.Text = dt.Rows[i]["btchdt"].ToString().Trim();

                        txtlbl4.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no,place,zipcode from type where id='B' and type1='" + frm_mbr + "'", "place");
                        txtlbl7.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no,place,zipcode from type where id='B' and type1='" + frm_mbr + "'", "zipcode");


                        doc_addl.Value = dt.Rows[0]["srno"].ToString().Trim();

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

                            //sg1_dr["sg1_f1"] = dt.Rows[i]["d_dfrom"].ToString().Trim();
                            //sg1_dr["sg1_f2"] = dt.Rows[i]["d_Cscode"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["acode"].ToString().Trim();
                            //sg1_dr["sg1_f4"] = dt.Rows[i]["type"].ToString().Trim();//Convert.ToDateTime(dt.Rows[i]["DOC_DT"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_f4"] = dt.Rows[i]["doc_type"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["Doc_No"].ToString().Trim();//fgen.seek_iname(frm_qstr, frm_cocd, "select ANAME FROM FAMST WHERE ACODE='" + dt.Rows[i]["ACODE"].ToString().Trim() + "'", "ANAME");
                            sg1_dr["sg1_t1"] = Convert.ToDateTime(dt.Rows[i]["DOC_DT"].ToString().Trim()).ToString("dd/MM/yyyy");//fgen.seek_iname(frm_qstr, frm_cocd, "sselect (Case when length(Trim(nvl(exc_regn,'-')))>5 then exc_regn else gst_no end) as tpt_id from famst where trim(Acode)='" + dt.Rows[i]["ACODE"].ToString().Trim() + "'", "tpt_id");

                            sg1_dr["sg1_t2"] = fgen.seek_iname(frm_qstr, frm_cocd, "select ANAME FROM FAMST WHERE ACODE='" + dt.Rows[i]["ACODE"].ToString().Trim() + "'", "ANAME");
                            sg1_dr["sg1_t3"] = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no FROM FAMST WHERE ACODE='" + dt.Rows[i]["ACODE"].ToString().Trim() + "'", "gst_no");
                            sg1_dr["sg1_t4"] = dt.Rows[i]["TO_STATE"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["DOC_VALUE"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["VEHI_NO"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["APPX_DIST"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["EWAY_BILL"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["GTO_PLACE"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["GTO_PIN"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["GTPT_NAME"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["GTPT_ID"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["GTPT_CODE"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }


                        //int j;
                        //for (j = i; j < 30; j++)
                        //{
                        //    sg1_add_blankrows();
                        //}


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
                        //SQuery = "select trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy')as fstr, b.Aname as Party_Name,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy') as link_doc,b.Staten,a.vchdate as Doc_Dt,a.vchnum as Doc_No,max(a.bill_tot) as Inv_amt from (Select type,vchnum,vchdate,acode,1 as docx,bill_tot from sale where branchcd='" + frm_mbr + "' and type like '4%' and vchdate>to_DaTE('01/01/2018','dd/mm/yyyy') and vchdate " + PrdRange + " and bill_tot>0 union all Select doc_type,trim(doc_no) as doc_no,doc_Dt,acode,-1 as docx,0 as amt from ewayb_Rec where branchcd='" + frm_mbr + "' and type like 'EW%' and vchdate>to_DaTE('01/01/2018','dd/mm/yyyy'))a,famst b where trim(A.acode)=trim(B.acode) group by b.Aname,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy'),b.Staten,a.vchdate,a.vchnum having sum(docx)>0 order by a.vchdate,a.vchnum";
                        SQuery = "select trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy')as fstr, b.Aname as Party_Name,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy') as link_doc,b.Staten,a.vchdate as Doc_Dt,a.vchnum as Doc_No,max(a.bill_tot) as Inv_amt,max(a.full_invno) As full_invno from (Select type,vchnum,vchdate,acode,1 as docx,bill_tot,full_invno from sale where branchcd='" + frm_mbr + "' and type like '4%' and vchdate>to_DaTE('01/01/2018','dd/mm/yyyy') and vchdate " + PrdRange + " and bill_tot>0 union all Select doc_type,trim(doc_no) as doc_no,doc_Dt,acode,-1 as docx,0 as amt,null as full_invno from ewayb_Rec where branchcd='" + frm_mbr + "' and type like 'EW%' and vchdate>to_DaTE('01/01/2018','dd/mm/yyyy') and doc_type like '4%' )a,famst b where trim(A.acode)=trim(B.acode) group by b.Aname,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy'),b.Staten,a.vchdate,a.vchnum having sum(docx)>0  order by a.vchdate DESC,a.vchnum desc";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek("Select Entry ", frm_qstr);
                        hffield.Value = "INV";

                    }
                    else
                    {
                        //SQuery = "select trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy')as fstr, b.Aname as Party_Name,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy') as link_doc,b.Staten,a.vchdate as Doc_Dt,a.vchnum as Doc_No from (Select type,vchnum,vchdate,acode,1 as docx from ivoucher where branchcd='" + frm_mbr + "' and type like '2%' and vchdate>to_DaTE('01/01/2018','dd/mm/yyyy') and vchdate " + PrdRange + " union all Select doc_type,trim(doc_no) as doc_no,doc_Dt,acode,-1 as docx from ewayb_Rec where branchcd='" + frm_mbr + "' and type like 'EW%' and vchdate>to_DaTE('01/01/2018','dd/mm/yyyy'))a,famst b where trim(A.acode)=trim(B.acode) group by b.Aname,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy'),b.Staten,a.vchdate,a.vchnum having sum(docx)>0 order by a.vchdate,a.vchnum";
                        SQuery = "select trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy')as fstr,b.Aname as Party_Name,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy') as link_doc,b.Staten,a.vchdate as Doc_Dt,a.vchnum as Doc_No,sum(docx) as aa from (Select distinct type,vchnum,vchdate,acode,1 as docx from ivoucher where branchcd='" + frm_mbr + "' and (type like '2%' or type like '65%') and vchdate>to_DaTE('01/01/2018','dd/mm/yyyy') and vchdate " + PrdRange + " and length(trim(vchnum))=6 union all Select doc_type,trim(doc_no) as doc_no,doc_Dt,acode,-1 as docx from ewayb_Rec where branchcd='" + frm_mbr + "' and type like 'EW%' and vchdate>to_DaTE('01/01/2018','dd/mm/yyyy') and doc_type not like '4%' )a,famst b where trim(A.acode)=trim(B.acode) group by b.Aname,trim(a.vchnum)||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy'),b.Staten,a.vchdate,a.vchnum  having sum(docx)>0  order by a.vchdate DESC,a.vchnum desc";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_mseek("Select Entry ", frm_qstr);
                        hffield.Value = "CHL";
                        ////flag = "1";
                        ////fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL12", flag);

                    }
                    break;
                case "CHL":
                case "INV":
                    if (col1.Length <= 0) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    //flag = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL12");


                    if (btnval == "INV")
                        SQuery = "SELECT A.*,nvl(c.brdist_kms,0) As dist_kms FROM (select nvl(a.desp_from,'-') as desp_from,nvl(a.ins_no,'-') as tpt_name,nvl(a.tptcode,'-') as tptcode,nvl(b.district,'-') " +
                            "as district,nvl(b.pincode,'-') as pincode,b.aname,replace(replace(replace(a.mo_vehi,'/',''),'-',''),' ','') as mo_vehi,nvl(b.staten,'-') as staten,ROUND(a.bill_tot,0) AS bill_tot,a.type,a.vchnum" +
                            ",a.vchdate,a.acode,nvl(b.gst_no,'-') As gst_no,nvl(a.st_entform,'-') as st_entform,b.staffcd,a.cscode from sale a, famst b where trim(a.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%'  and a.vchnum||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy') in (" + col1 + ") order by a.vchdate,a.vchnum ) A LEFT OUTER JOIN (SELECT * FROM famstbal WHERE BRANCHCD='" + frm_mbr + "') c ON   trim(a.acode)=trim(c.acode) ";
                    else
                        SQuery = "select distinct '-' as desp_From,a.tpt_names as tpt_name,'-' as tptcode,nvl(b.district,'-') as district,nvl(b.pincode,'-') as pincode,b.aname,replace(replace(replace(a.binno,'/',''),'-',''),' ','') as vehi_no2,replace(replace(replace(a.mode_tpt,'/',''),'-',''),' ','') as mo_vehi,nvl(b.staten,'-') as staten,nvl(a.approxval,0) as bill_tot,a.type,a.vchnum,a.vchdate,a.acode,nvl(b.gst_no,'-') As gst_no,nvl(c.brdist_kms,0) As dist_kms,nvl(a.st_entform,'-') as st_entform,b.staffcd from ivoucher a, famst b,famstbal c where c.branchcd='" + frm_mbr + "' and trim(a.acode)=trim(C.acode) and trim(a.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and (a.type like '2%' or a.type like '65%') and a.vchnum||'-'||a.type||'-'||to_char(a.vchdate,'dd/mm/yyyy') in (" + col1 + ") and a.iqtyout>0 order by a.vchdate,a.vchnum";
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
                            sg1_dr["sg1_f3"] = dt.Rows[i]["ACODE"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["TYPE"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["VCHNUM"].ToString().Trim();

                            sg1_dr["sg1_t1"] = Convert.ToDateTime(dt.Rows[i]["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_t2"] = dt.Rows[i]["ANAME"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["GST_NO"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["STATEN"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["BILL_TOT"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["MO_VEHI"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["DIST_KMS"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["st_entform"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["district"].ToString().Trim();//old
                            double distance = Convert.ToDouble(fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(brdist_kms,0) as brdist_kms from famstbal where branchcd='" + frm_mbr + "' and trim(Acode)='" + dt.Rows[i]["acode"].ToString().Trim() + "'", "brdist_kms"));
                            if (distance <= 0)
                            {
                                fgen.msg("-", "AMSG", "Please update distance from Sale Locn to Customer for Line no. " + i + "");
                            }
                            sg1_dr["sg1_t7"] = distance;
                            sg1_dr["sg1_t10"] = dt.Rows[i]["pincode"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["TPT_NAME"].ToString().Trim();
                            sg1_dr["sg1_t12"] = fgen.seek_iname(frm_qstr, frm_cocd, "select (Case when length(Trim(nvl(exc_regn,'-')))>5 then exc_regn else gst_no end) as tpt_id from " +
                                "famst where upper(trim(Aname))='" + dt.Rows[i]["TPT_NAME"].ToString().Trim().ToUpper() + "'", "tpt_id");
                            string tran_id_chk;
                            if (sg1_dr["sg1_t12"] == "0" || sg1_dr["sg1_t12"] == "-")
                            {
                                tran_id_chk = "";
                            }
                            else
                            {
                                tran_id_chk = sg1_dr["sg1_t12"].ToString().Trim();
                            }
                            sg1_dr["sg1_t12"] = tran_id_chk;
                            sg1_dr["sg1_t13"] = dt.Rows[i]["TPTCODE"].ToString().Trim();
                            // sg1_dr["sg1_t12"] = dt.Rows[i][""].ToString().Trim();
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
                        txtlbl4a.Text = col2;
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
                    txtlbl101.Text = col1;
                    txtlbl101a.Text = col2;
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

        switch (Prg_Id)
        {
            case "F30111":
                frm_vty = "20";
                break;
            case "F30112":
                frm_vty = "40";
                break;
            case "F30113":
                frm_vty = "10";
                break;
            case "SS01":
                frm_vty = "EW";
                break;

        }

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
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);

                fgen.Fn_open_sseek("Select Entry ", frm_qstr);
                return;

            }
        }

        if (hffield.Value == "List2")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select a.vchnum as Invoice_No,a.vchdate as Invoice_Dt,b.aname as Customer,a.St_Entform as EWay_bill,a.bill_tot as Bill_Total,a.mode_tpt as Vehi_no,b.dist_kms,b.staten from sale a , famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " AND a.type like '4%'  order by a.vchdate desc,a.vchnum desc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of Docs with E-Way Bill", frm_qstr);
            hffield.Value = "-";
            return;
        }

        if (hffield.Value == "PrintWay")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select a.vchnum as fstr,a.vchnum as Invoice_No,a.vchdate as Invoice_Dt,b.aname as Customer,a.St_Entform as EWay_bill,a.bill_tot as Bill_Total,a.mode_tpt as Vehi_no,b.dist_kms,b.staten from sale a , famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " AND a.type like '4%'  order by a.vchdate desc,a.vchnum desc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("Select Entry ", frm_qstr);
            hffield.Value = "PrintWay";
            return;
        }

        if (hffield.Value == "List")
        {


            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            //SQuery = "select a.Vchnum as Templ_no,to_char(a.vchdate,'dd/mm/yyyy') as Templ_Dt,c.Aname as Supplier,b.Iname,b.Cpartno,a.Col1 as Parameter,a.col2 as Standard,a.col3 as Lower_lmt,a.col4 as Upper_limit,a.acode,a.icode,a.Ent_by,a.ent_Dt ,to_Char(a.vchdate,'yyyymmdd') as vdd,a.srno from " + frm_tabname + " a,item b,famst c where trim(A.acode)=trim(c.acode) and trim(A.icode)=trim(b.icode) and  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + " order by vdd ,a.vchnum ,a.srno";

            SQuery = "select a.doc_type,a.doc_no,a.doc_Dt,b.ANAME,b.gst_no,b.stATEN,a.doc_value,a.vehi_no,a.appx_dist,a.eway_bill,a.gto_place,a.gto_pin,a.gtpt_name,a.gtpt_id,a.gtpt_Code,a.vchnum,a.vchdate,a.ent_by,a.d_Cscode,a.acode from ewayb_rec a,famst b where trim(a.acode)=trim(B.acode) and a.VCHDATE " + PrdRange + " AND a.type='" + frm_vty + "' and a.branchcd='" + frm_mbr + "'  order by a.vchdate ,a.vchnum ";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of Docs Through Eway Bill Created", frm_qstr);
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

                                if (sg1.Rows[i].Cells[17].Text.Trim().Length > 1)
                                {
                                    save_it = "Y";
                                }
                            }

                            if (save_it == "Y")
                            {

                                string doc_is_ok = "";
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

                        if (edmode.Value == "Y")
                        {


                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);


                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

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
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "vipin@tejaxo.com", "", "", "Hello", "test Mail");
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

                sg1.Columns[10].Visible = false;
                sg1.Columns[11].Visible = false;
                //sg1.Columns[31].Visible = false;
                //sg1.Columns[32].Visible = false;
                //sg1.Columns[33].Visible = false;
                //sg1.Columns[34].Visible = false;
                //sg1.Columns[35].Visible = false;


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

    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Grade ", frm_qstr);
    }
    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MRESULT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Result", frm_qstr);
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
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        return;
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        //if (frm_vnum != "000000" && frm_vty == "20")
        //{
        //    cmd_query = "update ivoucher set pname='" + frm_uname + "',tc_no='" + txtvchnum.Text + "',qc_date=sysdate,qcdate=to_datE(sysdate,'dd/mm/yyyy'),ACTUAL_INSP='Y',store='Y',inspected='Y',desc_=DECODE(Trim(desc_),'-','',Trim(desc_))||'QA.No.'||'" + txtvchnum.Text + "',IQTYIN=" + (fgen.make_double(txtlbl11.Text) - fgen.make_double(txtlbl13.Text)) + ",ACPT_UD =" + (fgen.make_double(txtlbl11.Text) - fgen.make_double(txtlbl13.Text)) + ",REJ_RW=" + fgen.make_double(txtlbl13.Text) + ",IEXC_aDDL =" + fgen.make_double(txtlbl14.Text) + " where branchcd='" + frm_mbr + "' and type like '0%' and vchnum ='" + txtlbl2.Text + "' and vchdate=to_Date('" + txtlbl3.Text + "','dd/mm/yyyy') and srno='" + doc_addl.Value.Trim() + "' and acode ='" + txtlbl4.Text + "' and icode ='" + txtlbl7.Text + "' and store<>'R'";
        //    fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
        //    if (fgen.make_double(txtlbl13.Text) > 0)
        //    {
        //        cmd_query = "delete from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchnum ='" + txtlbl2.Text + "' and vchdate=to_Date('" + txtlbl3.Text + "','dd/mm/yyyy') and srno='" + doc_addl.Value.Trim() + "' and acode ='" + txtlbl4.Text + "' and icode ='" + txtlbl7.Text + "' and store='R'";
        //        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

        //        cmd_query = "insert into ivoucher(vcode,iamount,btchno,btchdt,tc_no,pname,inspected,actual_insp,qcdate,qc_date,srno,branchcd,type,vchnum,vchdate,acode,icode,store,iqty_chl,iqtyin,iqtyout,acpt_ud,rej_rw,ponum,podate,rgpnum,rgpdate,invno,invdate,genum,gedate,rec_iss,ent_by,ent_dt,edt_by,edt_dt)(select acode,rej_rw*irate,btchno,btchdt,'" + txtvchnum.Text + "',pname,inspected,actual_insp,qcdate,qc_date,srno,branchcd,type,vchnum,vchdate,acode,icode,'R',iqty_chl," + fgen.make_double(txtlbl13.Text) + " as iqtyin,0 as iqtyout,0 as acpt_ud,0 as rej_rw,ponum,podate,rgpnum,rgpdate,invno,invdate,genum,gedate,rec_iss,ent_by,ent_dt,edt_by,edt_dt from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchnum ='" + txtlbl2.Text + "' and vchdate=to_Date('" + txtlbl3.Text + "','dd/mm/yyyy') and srno='" + doc_addl.Value.Trim() + "' and store<>'R' and acode ='" + txtlbl4.Text + "' and icode ='" + txtlbl7.Text + "')";
        //        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

        //    }

        //}

        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[17].Text.Trim().Length > 1)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = txtvchnum.Text.Trim();
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["SRNO"] = i + 1;
                oporow["DOC_TYPE"] = sg1.Rows[i].Cells[16].Text.Trim();
                oporow["DOC_NO"] = sg1.Rows[i].Cells[17].Text.Trim();

                oporow["DOC_DT"] = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString();
                oporow["ACODE"] = sg1.Rows[i].Cells[15].Text.Trim();
                oporow["TO_STATE"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text;
                oporow["VEHI_NO"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text;
                oporow["DOC_VALUE"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text;
                oporow["APPX_DIST"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text;
                oporow["EWAY_BILL"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text;
                oporow["GTO_PLACE"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text;
                oporow["GTO_PIN"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text;
                oporow["GTPT_NAME"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text;
                oporow["GTPT_ID"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text;
                oporow["GTPT_CODE"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text;

                //if (i == 0)
                //{
                //    oporow["obj1"] = fgen.make_double(txtlbl15.Text);
                //    oporow["obj2"] = fgen.make_double(txtlbl16.Text);
                //    oporow["obj3"] = fgen.make_double(txtlbl17.Text);
                //    oporow["obj4"] = fgen.make_double(txtlbl18.Text);
                //    oporow["obj5"] = fgen.make_double(txtlbl19.Text);
                //}
                //else
                //{
                //    oporow["obj1"] = 0;
                //    oporow["obj2"] = 0;
                //    oporow["obj3"] = 0;
                //    oporow["obj4"] = 0;
                //    oporow["obj5"] = 0;

                //}

                //    oporow["qty1"] = fgen.make_double(txtlbl10.Text);
                //    oporow["qty2"] = fgen.make_double(txtlbl11.Text);
                //    oporow["qty3"] = fgen.make_double(txtlbl12.Text);
                //    oporow["qty4"] = fgen.make_double(txtlbl13.Text);
                //    oporow["qty5"] = fgen.make_double(txtlbl14.Text);



                //oporow["title"] = txtrmk.Text.Trim();

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
                    //oporow["edt_by"] = "-";
                    //oporow["eDt_dt"] = vardate;
                }

                oDS.Tables[0].Rows.Add(oporow);

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
                SQuery = "SELECT '20' AS FSTR,'Quality Inward Certificate' as NAME,'20' AS CODE FROM dual";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "20");
                break;
            case "F30112":
                SQuery = "SELECT '40' AS FSTR,'Quality In-proc Certificate' as NAME,'40' AS CODE FROM dual";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "40");
                break;
            case "F30113":
                SQuery = "SELECT '10' AS FSTR,'Quality Outward Certificate' as NAME,'10' AS CODE FROM dual";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "EW");
                break;

        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "EW");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }

    protected void txt_TextChanged(object sender, EventArgs e)
    {
        //fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
        // made logic to get working hours and working minutes
        string dttoh = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text;
        string dttom = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text;
        string dtfromh = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text;
        string dtfromm = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text;


        DateTime dtFrom = DateTime.Parse(dtfromh + ":" + dtfromm);
        DateTime dtTo = DateTime.Parse(dttoh + ":" + dttom);

        int timeDiff = dtFrom.Subtract(dtTo).Hours;
        int timediff2 = dtFrom.Subtract(dtTo).Minutes;


        TextBox txtName = ((TextBox)sg1.Rows[i].FindControl("sg1_t5"));
        txtName.Text = timeDiff.ToString();

        TextBox txtName1 = ((TextBox)sg1.Rows[i].FindControl("sg1_t6"));
        txtName1.Text = timediff2.ToString();



    }
    //------------------------------------------------------------------------------------   
    protected void btnshow_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List2";
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("Select Entry ", frm_qstr);
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    protected void btnjson_ServerClick(object sender, EventArgs e)
    {


        if (txtvchnum.Text.Trim().Length < 6)
        {
            fgen.msg("", "ASMG", "Please Press New to Start");
            return;

        }
        //if (txtlbl7.Text == "-")
        //{
        //    fgen.msg("", "ASMG", "Pin Code reqd , put in branch master");
        //    return;
        //}


        gen_eway_bill("JSON");
        used_opt = "-";
    }
    private void gen_eway_bill(string FOPT)
    {
        used_opt = "FOPT";
        string chk_ewb = "";
        string catcode;


        chk_ewb = fgen.seek_iname(frm_qstr, frm_cocd, "select enable_yn from stock where id='M253'", "enable_yn");
        if (chk_ewb == "N")
        {
            fgen.msg("", "ASMG", "Please Get This Option Activated , Please Contact Tejaxo Support");
            return;

        }

        string chk_eirn, vewayirn;

        chk_eirn = fgen.getOptionPW(frm_qstr, frm_cocd, "W1085", "OPT_enable", frm_mbr);
        ////chk_O51 = fgen.seek_iname(frm_qstr, frm_cocd, " select trim(upper(enable_yn)) as chk, trim(params) as chk_dt from controls where id='O51'", "chk");
        ////chk_O51_dt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(enable_yn)) as chk, trim(params) as chk_dt from controls where id='O51'", "chk_dt");
        // ERP_M337_long_Invno = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(opt_enable)) as chk from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and opt_id='W2041'", "chk");
        ERP_M337_long_Invno = "Y";// hard code for testing for web
        ////if (chk_O51 == "Y")
        ////{
        ////    if (!fgen.CheckIsDate(chk_O51_dt))
        ////    {
        ////        fgen.msg("", "ASMG", "Date in control O51 not in proper format.Please correct");
        ////        return;
        ////    }
        ////}

        string err_str;
        string[] Edesc = new string[50];
        int err_Cnt;
        DataTable dt = new DataTable();
        string send_unitmaster;

        send_unitmaster = fgen.seek_iname(frm_qstr, frm_cocd, "select upper(trim(enable_yn))||trim(params) as enable_yn from controls where id='O36'", "enable_yn");

        string g_uid, g_pwd, g_zip, g_efuuid, g_efupwd, g_efukey, g_api_link, cotel, gst_name, mysez;
        int TOT_INV;
        string mygstno = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no,place,zipcode from type where id='B' and type1='" + frm_mbr + "'", "gst_no");

        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, "select gst_no,gstewb_id,gstewb_pw,zipcode,gstefu_id,gstefu_pw,gstefu_cdkey,gst_apiadd,trim(tele) as tele,trim(name) as name,upper(trim(sez_yn)) as sez_yn from type where id='B' and type1='" + frm_mbr + "'");

        mygstno = dt.Rows[0]["gst_no"].ToString();
        g_uid = dt.Rows[0]["gstewb_id"].ToString();
        g_pwd = dt.Rows[0]["gstewb_pw"].ToString();
        g_zip = dt.Rows[0]["zipcode"].ToString();
        g_efuuid = dt.Rows[0]["gstefu_id"].ToString();
        g_efupwd = dt.Rows[0]["gstefu_pw"].ToString();
        g_efukey = dt.Rows[0]["gstefu_cdkey"].ToString();
        g_api_link = dt.Rows[0]["gst_apiadd"].ToString();
        cotel = dt.Rows[0]["tele"].ToString();
        gst_name = dt.Rows[0]["name"].ToString();
        mysez = dt.Rows[0]["sez_yn"].ToString();

        string VNAME = fgen.getOption(frm_qstr, frm_cocd, "W0215", "OPT_enable");
        if (VNAME != "Y")
        {
            gst_name = fgenCO.chk_co(frm_cocd);
        }
        TOT_INV = 0;

        if ((g_api_link.ToString().Trim().Length < 10) && (FOPT == "WEBT"))
        {

            g_api_link = "https://ewayapi.mygstcafe.com/managed/v1.03/GenerateEwayBill";
            //fgen.msg("", "ASMG", "Portal API not linked in Plant Master , JSON File Will be Generated");

        }

        string res;
        string AA = "", BB = "", cc = "", dd = "";
        string gf01, gf02, gf03, gf04, gf05, gf06, gf07, gf08, gf09, gf10;
        string gf11, gf12, gf13, gf14, gf15, gf16, gf17, gf18, gf19, gf20;
        string gf21, gf22, gf23, gf24, gf25, gf26, gf27, gf28, gf29, gf30;
        string gf31, gf32, gf33, gf34, gf35, gf36, gf37, gf38, gf39, gf40;
        string gf41, gf42, gf43, gf44, gf45, gf46, gf47, gf48, gf49, tran_type;
        string gf50, gf51, gf481, gf52, gf53, gf49a;
        int i = 0;

        err_str = "";
        err_Cnt = 0;
        tran_type = "1";
        for (i = 0; i < sg1.Rows.Count; i++)
        {
            if ((sg1.Rows[i].Cells[17].Text.Trim().Length > 1) && (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim()) > 4))
            {
                err_str = err_str + " Eway Bill Already Filled , Pl Remove ";
                err_Cnt = err_Cnt + 1;
                ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).BackColor = System.Drawing.Color.Red;
            }

            //if ((sg1.Rows[i].Cells[17].Text.Trim().Length > 1) && (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim()) <= 0))
            //{
            //    err_str = err_str + " Distance Not Filled ";
            //    err_Cnt = err_Cnt + 1;
            //    ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).BackColor = System.Drawing.Color.Red;
            //}

            if ((sg1.Rows[i].Cells[17].Text.Trim().Length > 1) && (((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper().Length < 2))
            {
                err_str = err_str + " Place Not Filled ";
                err_Cnt = err_Cnt + 1;
                ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).BackColor = System.Drawing.Color.Red;
            }


            if ((sg1.Rows[i].Cells[17].Text.Trim().Length > 1) && (((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().Length < 6))
            {
                err_str = err_str + " Pin Code Not Filled  ";
                err_Cnt = err_Cnt + 1;
                ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).BackColor = System.Drawing.Color.Red;
            }
            if ((sg1.Rows[i].Cells[17].Text.Trim().Length > 1) && (((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper().Length <= 3) && frm_cocd != "BONYG" && frm_cocd != "SFAB" && frm_cocd != "ELEC")
            {
                err_str = err_str + " Vehicle No. Not Filled  ";
                err_Cnt = err_Cnt + 1;
                ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).BackColor = System.Drawing.Color.Red;
            }

        }
        if (err_Cnt > 0)
        {
            fgen.msg("", "ASMG", "Total errors " + err_Cnt + "Please Correct Indicated Cells to Proceed");
            return;
        }

        ////'        If Len(Trim(sg.text(i, 1))) > 1 And Len(Trim(sg.text(i, 11))) < 10 Then
        ////'            sg.CellBackColor(i, 11) = vbRed
        ////'            err_Cnt = err_Cnt + 1
        ////'        End If

        //----------commented for testing purpose
        for (i = 0; i < sg1.Rows.Count; i++)
        {
            if ((sg1.Rows[i].Cells[17].Text.Trim().Length > 1) && (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim()) > 0))
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update famstbal set brdist_kms=" + Convert.ToDouble(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim()) + " where brdist_kms=0 and trim(branchcd)='" + frm_mbr + "' and trim(acode)='" + sg1.Rows[i].Cells[15].Text + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "commit");
            }

            if (sg1.Rows[i].Cells[17].Text.Trim().Length > 1)
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update famst set district='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper() + "' where trim(nvl(district,'-'))='-' and trim(acode)='" + sg1.Rows[i].Cells[15].Text + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "commit");
            }


            if (sg1.Rows[i].Cells[17].Text.Trim().Length > 1)
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update famst set pincode='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().ToUpper() + "' where trim(nvl(pincode,'-'))='-' and trim(acode)='" + sg1.Rows[i].Cells[15].Text + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "commit");
            }


            if (((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper().Length > 10)
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update famst set ccode='T',exc_regn='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim().ToUpper() + "' where trim(nvl(exc_regn,'-'))='-' and trim(acode)='" + sg1.Rows[i].Cells[15].Text + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "commit");
            }

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper().Length > 1 && sg1.Rows[i].Cells[16].Text.Substring(0, 1) == "2" && ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim().ToUpper().Length > 2)
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update sale set ins_no='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim().ToUpper() + "',mo_vehi='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper() + "',tptcode='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim().ToUpper() + "' where trim(branchcd)||type||vchnum||to_char(Vchdate,'dd/mm/yyyy')='" + frm_mbr + sg1.Rows[i].Cells[16].Text.Trim() + sg1.Rows[i].Cells[17].Text.Trim() + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "' and trim(acode)='" + sg1.Rows[i].Cells[15].Text.Trim() + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "commit");
            }
            if (((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper().Length > 1 && sg1.Rows[i].Cells[16].Text.Substring(0, 1) == "4" && ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper().Length > 2)
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update sale set ins_no='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim().ToUpper() + "',mo_vehi='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper() + "' where trim(branchcd)||type||vchnum||to_char(Vchdate,'dd/mm/yyyy')='" + frm_mbr + sg1.Rows[i].Cells[16].Text.Trim() + sg1.Rows[i].Cells[17].Text.Trim() + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "' and trim(acode)='" + sg1.Rows[i].Cells[15].Text.Trim() + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "commit");
            }

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper().Length > 1 && sg1.Rows[i].Cells[16].Text.Substring(0, 1) == "2" && ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim().ToUpper().Length > 2)
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update ivoucher set tpt_names='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim().ToUpper() + "',mode_Tpt='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper() + "' where trim(branchcd)||type||vchnum||to_char(Vchdate,'dd/mm/yyyy')='" + frm_mbr + sg1.Rows[i].Cells[16].Text.Trim() + sg1.Rows[i].Cells[17].Text.Trim() + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "' and trim(acode)='" + sg1.Rows[i].Cells[15].Text.Trim() + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "commit");
            }

        }

        err_str = "";
        err_Cnt = 0;

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

        for (i = 0; i < sg1.Rows.Count; i++)
        {
            if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) != "4")
            {
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().Length > 1)
                {
                    upd_addl_sal_exp(frm_mbr + sg1.Rows[i].Cells[16].Text.Trim() + sg1.Rows[i].Cells[17].Text.Trim() + sg1.Rows[i].Cells[18].Text.Trim());
                }
            }
        }

        double chl_taxes;
        chl_taxes = 0;
        string vhscode, vprod_name, vsubsupdesc;
        vsubsupdesc = "";
        chl_taxes = 0;

        for (i = 0; i < sg1.Rows.Count; i++)
        {


            if ((sg1.Rows[i].Cells[17].Text.Trim().Length > 1) && (((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper().Length < 10))
            {
                gf01 = mygstno;
                gf02 = "O";


                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) != "4")
                {
                    gf03 = "4";
                    gf04 = "CHL";


                    if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "29" || sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "65")
                    {
                        gf03 = "5";
                    }
                    if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "29" && sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "02")
                    {
                        gf03 = "5";
                    }
                    if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "25" && (frm_cocd == "MMC"))
                    {
                        gf03 = "5";
                    }
                    if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "21" && CLIENTGRP == "GRP_LOGW" && sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "02")
                    {
                        gf03 = "5";
                    }
                    if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "22")
                    {
                        gf03 = "8";
                        vsubsupdesc = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT substr(trim(NAME),1,20) as aa FROM TYPE WHERE ID='M' and trim(type1)='22'", "aa");
                    }

                }
                else
                {

                    gf03 = "1";

                    if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "4F")
                    {
                        gf03 = "3";
                    }
                    gf04 = "INV";
                }

                string firm = "", coaddr1, coaddr2 = "";
                //firm = fgen.seek_iname(frm_qstr,frm_cocd,"select name from type where id='B' and type1= '"+frm_mbr+"'","name");
                coaddr1 = fgen.seek_iname(frm_qstr, frm_cocd, "select addr from type where id='B' and type1='" + frm_mbr + "'", "addr");
                coaddr2 = fgen.seek_iname(frm_qstr, frm_cocd, "select addr1 from type where id='B' and type1='" + frm_mbr + "'", "addr1");



                // branchwise companycode

                if ((frm_cocd == "MLGI") || (frm_cocd == "UISW") || (frm_cocd == "SNPX") || (frm_cocd == "HENA") || (frm_cocd == "SIGM") || (frm_cocd == "LRFP") || (frm_cocd == "AZUR") || (frm_cocd == "JACL") || (frm_cocd == "OTTO") || (frm_cocd == "MTPL") || (frm_cocd == "SSEN") || (frm_cocd == "SKYP") || (frm_cocd == "SKHA") || (frm_cocd == "SPMA") || (frm_cocd == "SHIV") || (frm_cocd == "HIMT") || (frm_cocd == "BUPS"))
                {
                    if (frm_cocd != "MICR")
                    {
                        firm = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='B' and type1= '" + frm_mbr + "'", "name");
                    }
                }
                else
                {

                    firm = fgenCO.chk_co(frm_cocd);
                }

                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "2")
                    gf05 = "(Case when length(trim(nvl(a.gstvch_no,'-'))) < 8 then a.vchnum else trim(a.gstvch_no) end )";
                else
                {
                    if (ERP_M337_long_Invno == "Y" && sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4")
                        gf05 = "(Case when substr(nvl(b.full_invno,'-'),1,1)='-' then a.vchnum else trim(b.full_invno) end )";
                    else
                        gf05 = "a.vchnum";
                }
                //if (FOPT == "JSON")
                    gf06 = "to_char(a.vchdate,'dd/mm/yyyy')";
                //else
                //    gf06 = "to_char(a.vchdate,'yyyymmdd')";
                // 'supplier
                gf07 = "'" + mygstno + "'";
                gf08 = "'" + firm + "'";

                gf09 = "'" + coaddr1 + "'";
                gf10 = "'" + coaddr2 + "'";
                gf11 = "'" + txtlbl4.Text + "'";
                if (FOPT == "WEBT")
                {
                    if (mysez == "Y")
                        gf12 = "99";
                    else
                        gf12 = "'" + mygstno.Substring(0, 2) + "'";
                }
                else
                {
                    gf12 = "'" + mygstno.Substring(0, 2) + "'";
                }

                gf13 = "'" + txtlbl7.Text + "'";
                //   'customer
                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "4F") gf14 = "'URP'";
                else gf14 = "c.gst_no";
                gf15 = "trim(c.aname)";
                gf50 = "(case when trim(nvl(c.bank_acno, '-'))='SEZ' then 1 else 0 end)";
                gf16 = "substr(trim(replace(c.addr1,'''','`')),1,120)";
                gf17 = "substr(trim(replace(c.addr2,'''','`')),1,120)";
                gf18 = "substr(trim(replace(c.district,'''','`')),1,120)";
                gf19 = "trim(c.staffcd)";
                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4" && sg1.Rows[i].Cells[13].Text.Trim().Length == 6)
                    gf20 = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                else
                    gf20 = "trim(c.pincode)";

                gf21 = "1";
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim() == "-") gf22 = "''";
                else gf22 = "'" + ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim() + "'";

                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) != "4")
                {
                    gf23 = "trim(a.thru)";
                }
                else
                    gf23 = "trim(b.ins_no)";

                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4" && sg1.Rows[i].Cells[13].Text.Trim().Length == 6)
                    gf24 = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                else
                    gf24 = "nvl(f.brdist_kms,0)";

                gf25 = "'-'";

                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) != "4")
                {

                    if (FOPT == "JSON")
                    {
                        gf26 = "to_char(a.vchdate,'dd/mm/yyyy')";
                    }
                    else
                    {
                        gf26 = "to_char(a.vchdate,'yyyymmdd')";
                    }

                    //gf27 = "trim(replace(replace(replace(a.binno,'/',''),'-',''),' ',''))";// in pTejaxo binno
                    gf27 = "trim(replace(replace(replace(a.mode_tpt,'/',''),'-',''),' ',''))";
                }

                else
                {
                    gf25 = "(case when trim(b.grno)='-' then null else trim(b.grno) end)";
                    //if (FOPT == "JSON")
                    //{
                        gf26 = "to_char(b.grdate,'dd/mm/yyyy')";
                    //}
                    //else
                    //{
                    //    gf26 = "to_char(b.grdate,'yyyymmdd')";
                    //}
                    gf27 = "trim(replace(replace(replace(b.mo_Vehi,'/',''),'-',''),' ',''))";
                }

                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "2")

                    gf28 = "nvl(a.morder+1,0)";
                else
                    gf28 = "nvl(a.morder,0)";
                gf29 = "substr(trim(e.name),1,100)";//for changing hs code
                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) != "4")
                    gf30 = "substr(trim(replace(replace(d.iname,'" + "" + "',' Inch'),'''','`')),1,100)";
                else
                {
                    gf30 = "substr(trim(replace(replace(a.purpose,'" + "" + "',' Inch'),'''','`')),1,100)";
                }
                gf31 = "trim(d.hscode)";
                gf32 = "a.iqtyout";
                gf33 = "trim(d.unit)";

                gf34 = "a.iamount+nvl(a.exp_punit,0)+round(a.iqtyout*nvl(a.iexc_Addl,0),2)+round(a.iqtyout*nvl(a.ipack,0),2)+round(a.iqtyout*nvl(a.idiamtr,0),2)";

                if (FOPT == "JSON")
                {
                    if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) != "4")
                    {
                        gf35 = "nvl(a.cess_percent,0)";
                        gf36 = "nvl(b.rvalue,0)";
                        gf37 = "nvl(a.exc_Rate,0)";
                        gf38 = "nvl(b.amt_Exc,0)";
                        gf39 = "(Case when 1=2 then a.exc_Rate else 0 end)";
                        gf40 = "(Case when 1=2 then b.amt_Exc else 0 end)";
                    }
                    else
                    {
                        gf35 = "(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end)";
                        gf36 = "(Case when trim(b.post)='C' then b.rvalue else 0 end)";
                        gf37 = "(Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end)";
                        gf38 = "(Case when trim(b.post)='C' then b.amt_Exc else 0 end)";
                        gf39 = "(Case when trim(A.iopr)='IG' then a.exc_Rate else 0 end)";
                        gf40 = "(Case when trim(b.post)='I' then b.amt_Exc else 0 end)";
                    }
                }
                else
                {
                    if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) != "4")
                    {
                        gf35 = "(Case when trim(A.post)='1' then nvl(a.cess_percent,0) else 0 end)";
                        gf36 = "(Case when trim(A.post)='1' then nvl(a.cess_pu,0) else 0 end)";
                        gf37 = "(Case when trim(A.post)='1' then nvl(a.exc_Rate,0) else 0 end)";
                        gf38 = "(Case when trim(A.post)='1' then nvl(a.exc_Amt,0) else 0 end)";
                        gf39 = "(Case when trim(A.post)='2' then nvl(a.exc_Rate,0) else 0 end)";
                        gf40 = "(Case when trim(A.post)='2' then nvl(a.exc_Amt,0) else 0 end)";
                    }
                    else
                    {
                        gf35 = "(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end)";
                        gf36 = "(Case when trim(A.iopr)='CG' then a.cess_pu+nvl(a.rej_sdv,0) else 0 end)";
                        gf37 = "(Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end)";
                        gf38 = "(Case when trim(A.iopr)='CG' then a.exc_Amt+nvl(a.rej_rw,0) else 0 end)";
                        gf39 = "(Case when trim(A.iopr)='IG' then a.exc_Rate else 0 end)";
                        gf40 = "(Case when trim(A.iopr)='IG' then a.exc_Amt+nvl(a.rej_rw,0) else 0 end)";

                    }
                }
                gf41 = "0";
                gf42 = "0";
                gf43 = g_uid;
                gf44 = g_pwd;

                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) != "4")
                {
                    if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "6")
                    {// to set for 65 type inter unit sale challan mg 19/10/2021

                        chl_taxes = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select MAX(SPEXC_RATE) as tota from ivoucher where branchcd='" + frm_mbr + "' and type='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and vchdate=to_DaTE('" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "','dd/mm/yyyy')", "tota"));
                        gf45 = "0+" + chl_taxes;
                        chl_taxes = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select MAX(spexc_amt) as tota from ivoucher where branchcd='" + frm_mbr + "' and type='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and vchdate=to_DaTE('" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "','dd/mm/yyyy')", "tota"));
                        gf46 = "0+" + chl_taxes;
                    }
                    else
                    {
                        chl_taxes = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select sum(nvl(Exc_amt,0)+nvl(Cess_pu,0)) as tota from ivoucher where branchcd='" + frm_mbr + "' and type='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and vchdate=to_DaTE('" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "','dd/mm/yyyy')", "tota"));
                        gf45 = "nvl(a.approxval,0)-" + chl_taxes;
                        //gf46 = "nvl(a.approxval,0)+" + chl_taxes;
                        gf46 = "nvl(a.approxval,0)";
                    }
                }
                else
                {
                    //if (FOPT == "JSON")
                    //{
                        gf45 = "nvl(b.amt_sale,0)";
                    //}
                    //else
                    //{
                    //    gf45 = "nvl(b.bill_tot,0)";
                    //}
                    gf46 = "round(nvl(b.bill_tot,0),0)";
                }
                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) != "4")
                {
                    gf47 = "nvl(a.branchcd,'-')";
                }
                else
                {
                    gf47 = "nvl(b.cscode,'-')";
                }
                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) != "4")
                {
                    gf48 = "0";
                    gf481 = "0";
                    gf49 = " '-' ";
                }
                else
                {
                    gf48 = "nvl(b.amt_Extexc,0)";
                    gf481 = "nvl(b.tcsamt,0) ";
                    gf49 = "nvl(b.desp_from,'-')";
                }

                if (chk_eirn == "Y" && sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "4")
                    gf49a = ",nvl(b.einv_no,'-') as einvno";
                else
                    gf49a = ", '-' as einvno ";

                gf51 = "trim(a.buyer)";

                //gf01 = "05AAACD5767E1ZT";
                //gf43 = "05AAACD5767E1ZT";
                //gf44 = "abc123@@";
                //g_efuuid = "05AAACD8069KIZF";
                // g_efupwd = "abc123@@";
                // g_efukey = "1000687";

                AA = "'" + gf01 + "' as GSTIN,'" + gf02 + "' as sup_type,'" + gf03 + "' as sub_type,'" + gf04 + "' as doc_type," + gf05 + " as doc_no," + gf06 + " as doc_Dt" +
                    "," + gf07 + " as sup_gst," + gf08 + " as sup_nam," + gf09 + " as sup_add1," + gf10 + " as sup_add2," + gf11 + " as sup_add3," + gf12 + " as sup_state," +
                    "" + gf13 + " as sup_pin," + gf14 + " as rec_gst," + gf15 + " as rec_nam," + gf50 + " as sez_flag ," + gf16 + " as rec_add1," + gf17 + " as rec_add2," + gf18 + " as rec_add3" +
                    "," + gf19 + " as rec_state," + gf20 + " as rec_pin,";
                //BB = "" + gf21 + " as tran_mode," + gf22 + " as tran_ID," + gf23 + " as tran_name," + gf24 + " as tran_dist," + gf25 + " as tran_doc," + gf26 + " as tran_dt," + gf27 + " as vehi_no," + gf28 + " as item_no," + gf29 + " as prod_name," + gf30 + " as prod_desc," + gf31 + " as hs_code," + gf32 + " as Quantity," + gf33 + " as quan_unit," + gf34 + " as taxb_val," + gf35 + " as sgst_rt," + gf36 + " as sgst_val," + gf37 + " as cgst_rt," + gf38 + " as cgst_val," + gf39 + " as igst_rt," + gf40 + " as igst_val," + gf41 + " as cess_rt,"+gf51+" as portcode,";
                BB = "" + gf21 + " as tran_mode," + gf22 + " as tran_ID," + gf23 + " as tran_name," + gf24 + " as tran_dist," + gf25 + " as tran_doc," + gf26 + " as tran_dt," +
                    "" + gf27 + " as vehi_no," + gf28 + " as item_no," + gf29 + " as prod_name," + gf30 + " as prod_desc," + gf31 + " as hs_code," + gf32 + " as Quantity," + gf33 + " as quan_unit" +
                    "," + gf34 + " as taxb_val," + gf35 + " as sgst_rt," + gf36 + " as sgst_val," + gf37 + " as cgst_rt," + gf38 + " as cgst_val," + gf39 + " as igst_rt," + gf40 + " as igst_val" +
                    "," + gf41 + " as cess_rt," + gf51 + " as portcode,";
                if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "2")
                {
                    if (frm_cocd == "RTEC" || frm_cocd == "ACOT" && sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "25")
                    {
                        cc = "" + gf42 + " as cess_val,'" + gf43 + "' as ewb_user,'" + gf44 + "' as ewb_pwd," + gf45 + " as billstot," + gf46 + " as billgtot," + gf47 + " as my_Cscode," + gf48 + " as oth_chgs," + gf481 + " as tcs_amt," + gf49 + " as desp_from1 " + gf49a + " from ivoucher a,(select branchcd,type,vchnum,vchdate,post,sum(IAMOUNT+approxval) as approxval,sum(exc_amt+IS_NUMBER(COL2)) as amt_Exc,sum(cess_pu+IS_NUMBER(COL3)) as rvalue from ivoucher where branchcd='" + frm_mbr + "' and type='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and to_Char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "' group by branchcd,type,vchnum,vchdate,post)b, famst c, item d,typegrp e,famstbal f where a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')=b.branchcd||b.type||b.vchnum||to_char(b.vchdate,'dd/mm/yyyy') and nvl(d.tax_item,'-')!='Y' and  e.id='T1' and trim(d.hscode)=trim(e.acref) and f.branchcd='" + frm_mbr + "' and trim(a.acode)=trim(f.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and a.vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and to_Char(a.vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "' and  e.id='T1' and trim(d.hscode)=trim(e.acref) and trim(a.acode)=trim(C.acode) and trim(a.icode)=trim(d.icode) order by a.morder";
                    }
                    else
                    {
                        cc = "" + gf42 + " as cess_val,'" + gf43 + "' as ewb_user,'" + gf44 + "' as ewb_pwd," + gf45 + " as billstot," + gf46 + " as billgtot," + gf47 + " as my_Cscode," + gf48 + " as oth_chgs," + gf481 + " as tcs_amt," + gf49 + " as desp_from1 " + gf49a + "  from ivoucher a,(select branchcd,type,vchnum,vchdate,post,approxval,sum(exc_amt) as amt_Exc,sum(cess_pu) as rvalue from ivoucher where branchcd='" + frm_mbr + "' and type='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and to_Char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "' group by approxval,branchcd,type,vchnum,vchdate,post)b, famst c, item d,typegrp e,famstbal f where a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')=b.branchcd||b.type||b.vchnum||to_char(b.vchdate,'dd/mm/yyyy') and nvl(d.tax_item,'-')!='Y' and  e.id='T1' and trim(d.hscode)=trim(e.acref) and f.branchcd='" + frm_mbr + "' and trim(a.acode)=trim(f.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and a.vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and to_Char(a.vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "' and  e.id='T1' and trim(d.hscode)=trim(e.acref) and trim(a.acode)=trim(C.acode) and trim(a.icode)=trim(d.icode) order by a.morder";
                    }
                }
                else if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 1) == "6")
                {
                    cc = "" + gf42 + " as cess_val,'" + gf43 + "' as ewb_user,'" + gf44 + "' as ewb_pwd," + gf45 + " as billstot," + gf46 + " as billgtot," + gf47 + " as my_Cscode," + gf48 + " as oth_chgs," + gf481 + " as tcs_amt," + gf49 + " as desp_from1 " + gf49a + "   from ivoucher a,(select branchcd,type,vchnum,vchdate,post,MAX(spexc_amt) AS approxval,sum(exc_amt) as amt_Exc,sum(cess_pu) as rvalue from ivoucher where branchcd='" + frm_mbr + "' and type='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and to_Char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "' group by branchcd,type,vchnum,vchdate,post)b, famst c,typegrp e, item d,famstbal f where a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')=b.branchcd||b.type||b.vchnum||to_char(b.vchdate,'dd/mm/yyyy') and nvl(d.tax_item,'-')!='Y' and f.branchcd='" + frm_mbr + "' and trim(a.acode)=trim(f.acode) and  a.branchcd='" + frm_mbr + "' and a.type='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and a.vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and to_Char(a.vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "' and  e.id='T1' and trim(d.hscode)=trim(e.acref) and trim(a.acode)=trim(C.acode) and trim(a.icode)=trim(d.icode) order by a.morder";
                }
                else
                {
                    string spl_cond;
                    spl_cond = " 1=1 and ";
                    if (frm_cocd == "WPPL" || 1 == 1)
                    {
                        spl_cond = " a.irate>0 and ";
                    }
                    cc = "" + gf42 + " as cess_val,'" + gf43 + "' as ewb_user,'" + gf44 + "' as ewb_pwd," + gf45 + " as billstot," + gf46 + " as billgtot," + gf47 + " as my_Cscode," + gf48 + " as oth_chgs," + gf481 + " as tcs_amt," + gf49 + " as desp_from1 " + gf49a + "  from ivoucher a, sale b , famst c, item d,typegrp e,famstbal f where " + spl_cond + " nvl(d.tax_item,'-')!='Y' and  e.id='T1' and trim(d.hscode)=trim(e.acref) and f.branchcd='" + frm_mbr + "' and trim(a.acode)=trim(f.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + sg1.Rows[i].Cells[16].Text.Trim() + "' and a.vchnum='" + sg1.Rows[i].Cells[17].Text.Trim() + "' and to_Char(a.vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToShortDateString() + "'  and  e.id='T1' and trim(d.hscode)=trim(e.acref) and a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')=b.branchcd||b.type||b.vchnum||to_char(b.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(C.acode) and trim(a.icode)=trim(d.icode) order by a.morder";//and  e.id='T1' and trim(d.hscode)=trim(e.acref)
                }


                if ((frm_cocd == "RTEC" || frm_cocd == "ACOT") && sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "25")
                {
                    catcode = "select " + AA + BB + cc;
                    catcode = "select a.*,0 as tcs_Amt from (Select GSTIN,sup_type,sub_type,doc_type,doc_no,doc_Dt,sup_gst,sup_nam,sup_add1,sup_add2,sup_add3,sup_state,sup_pin,rec_gst,rec_nam,rec_add1,rec_add2,rec_add3,rec_state,rec_pin,tran_mode,tran_ID,tran_name,tran_dist,tran_doc,tran_dt,vehi_no,1 as item_no,prod_name,'Job Work Value' as prod_desc,hs_code,sum(Quantity) as Quantity,quan_unit,sum(taxb_val) as taxb_val,sgst_rt,max(sgst_val) as sgst_val,cgst_rt,max(cgst_val) as cgst_val,igst_rt,max(igst_val)as igst_val,cess_rt,cess_val,ewb_user,ewb_pwd,MAX(billstot) as billstot,MAX(billgtot) as billgtot,my_Cscode,max(oth_chgs) as oth_chgs,'-' as desp_from1 from (" + catcode + ") group by GSTIN,sup_type,sub_type,doc_type,doc_no,doc_Dt,sup_gst,sup_nam,sup_add1,sup_add2,sup_add3,sup_state,sup_pin,rec_gst,rec_nam,rec_add1,rec_add2,rec_add3,rec_state,rec_pin,tran_mode,tran_ID,tran_name,tran_dist,tran_doc,tran_dt,vehi_no,prod_name,hs_code,quan_unit,sgst_rt,cgst_rt,igst_rt,cess_rt,cess_val,ewb_user,ewb_pwd,my_Cscode union all Select GSTIN,sup_type,sub_type,doc_type,doc_no,doc_Dt,sup_gst,sup_nam,sup_add1,sup_add2,sup_add3,sup_state,sup_pin,rec_gst,rec_nam,rec_add1,rec_add2,rec_add3,rec_state,rec_pin,tran_mode,tran_ID,tran_name,tran_dist,tran_doc,tran_dt,vehi_no,1 as item_no,prod_name,'Material Value' as prod_desc,hs_code,sum(Quantity) as Quantity,quan_unit,MAX(billstot)-sum(taxb_val) as taxb_val,sgst_rt,max(sgst_val) as sgst_val,cgst_rt,max(cgst_val) as cgst_val,igst_rt,max(igst_val)as igst_val,cess_rt,cess_val,ewb_user,ewb_pwd,MAX(billstot) as billstot,MAX(billgtot) as billgtot,my_Cscode,max(oth_chgs) as oth_chgs,'-' as desp_from1 from (" + catcode + ") group by GSTIN,sup_type,sub_type,doc_type,doc_no,doc_Dt,sup_gst,sup_nam,sup_add1,sup_add2,sup_add3,sup_state,sup_pin,rec_gst,rec_nam,rec_add1,rec_add2,rec_add3,rec_state,rec_pin,tran_mode,tran_ID,tran_name,tran_dist,tran_doc,tran_dt,vehi_no,prod_name,hs_code,quan_unit,sgst_rt,cgst_rt,igst_rt,cess_rt,cess_val,ewb_user,ewb_pwd,my_Cscode)a  order by a.sup_type,a.sub_type,a.doc_type,a.doc_no,a.doc_Dt";
                }
                else
                {
                    catcode = "select " + AA + BB + cc;
                }

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, catcode);

                catcode = "";
                if (dt.Rows.Count <= 0)
                {
                    fgen.msg("", "ASMG", "Please Check Data Linkage , Data is not OK");
                    return;
                }

                else
                {
                    //'cc = "["
                    TOT_INV = TOT_INV + 1;
                    int d = 0;

                    string cs_st_cd = "", ds_st_cd, rec_st_cd;
                    string cs_add1, cs_add2, cs_add3, cs_pinc;
                    string ds_add1, ds_add2, ds_add3, ds_add4, ds_pinc, ds_gst, ds_aname;

                    //if (FOPT == "JSON")
                    {
                        cc = "";
                        cs_st_cd = fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(trim(cstaffcd),'-') as cstaffcd from csmst where trim(acode)='" + dt.Rows[0]["my_Cscode"].ToString().Trim() + "'", "cstaffcd");
                        if (mygstno.Length == 15)
                            cs_st_cd = fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(trim(gst_no),'-') as gst_no from csmst where trim(acode)='" + dt.Rows[0]["my_Cscode"].ToString().Trim() + "'", "gst_no").Left(2);
                        else
                            cs_st_cd = cs_st_cd;
                        if (cs_st_cd.Length < 2)
                        {
                            cs_st_cd = dt.Rows[0]["rec_State"].ToString();
                        }
                        else
                        {
                            tran_type = "2";
                        }

                        cs_add1 = dt.Rows[0]["rec_add1"].ToString();
                        cs_add2 = dt.Rows[0]["rec_add2"].ToString();
                        cs_add3 = dt.Rows[0]["rec_add3"].ToString();
                        cs_pinc = dt.Rows[0]["rec_pin"].ToString();

                        if (dt.Rows[0]["my_Cscode"].ToString().Length >= 6)
                        {
                            dt4 = new DataTable();
                            dt4 = fgen.getdata(frm_qstr, frm_cocd, "select addr1,addr2,addr3,pincode from csmst where trim(acode)='" + dt.Rows[0]["my_Cscode"].ToString().Trim() + "'");
                            if (dt4.Rows.Count > 0)
                            {
                                cs_add1 = dt4.Rows[0]["addr1"].ToString();
                                cs_add2 = dt4.Rows[0]["addr2"].ToString();
                                cs_add3 = dt4.Rows[0]["addr3"].ToString();
                                cs_pinc = dt4.Rows[0]["pincode"].ToString();
                            }

                        }
                        string transid = "";
                        if (dt.Rows[0]["tran_id"].ToString() == "-")
                            transid = "-";
                        else
                            transid = dt.Rows[0]["tran_id"].ToString();

                        string pfix = "";
                        if (frm_cocd == "MASS" && hffield.Value == "INV")
                        {
                            pfix = "MAS/22-23/";
                        }
                        else if (frm_cocd == "MASS" && hffield.Value == "CHL")
                        {
                            pfix = "MAS/CH/22-23/";
                        }

                        if (frm_cocd == "MAST" && hffield.Value == "INV")
                        {
                            pfix = "MT/22-23/";
                        }
                        else if (frm_cocd == "MAST" && hffield.Value == "CHL")
                        {
                            pfix = "MT/CH/22-23/";
                        }                        
                        if (sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "4F" || sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "42" || sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 2) == "4E")
                        {
                            AA = "{'supplyType':'" + dt.Rows[0]["sup_type"].ToString() + "','subSupplyType': '" + dt.Rows[0]["sub_type"].ToString() + "'," +
                               "'subSupplyDesc':'" + vsubsupdesc + "','docType':'" + dt.Rows[0]["doc_type"].ToString() + "','docNo': '"+pfix + dt.Rows[0]["doc_no"].ToString() + "'," +
                               "'docDate': '" + dt.Rows[0]["doc_dt"].ToString() + "','fromGstin':'" + dt.Rows[0]["sup_gst"].ToString() + "','fromTrdName':'" + dt.Rows[0]["sup_nam"].ToString() + "'" +
                               ",'fromAddr1': '" + dt.Rows[0]["sup_add1"].ToString() + "','fromAddr2':'" + dt.Rows[0]["sup_add2"].ToString() + "','fromPlace': '" + dt.Rows[0]["sup_add3"].ToString() + "'" +
                               ",'fromPincode':'" + dt.Rows[0]["sup_pin"].ToString() + "','fromStateCode':  '" + dt.Rows[0]["sup_state"].ToString() + "'," +
                               "'actFromStateCode':'" + dt.Rows[0]["sup_state"].ToString() + "','toGstin': '" + dt.Rows[0]["rec_gst"].ToString() + "'," +
                               "'toTrdname': '" + dt.Rows[0]["rec_nam"].ToString() + "','toAddr1': '" + cs_add1 + "','toAddr2': '" + cs_add2 + "','toPlace': '" + cs_add3 + "'," +
                               "'toPincode': '" + cs_pinc + "','toStateCode': '" + dt.Rows[0]["rec_state"].ToString() + "','actToStateCode': '" + cs_st_cd + "'," +
                               "'totalValue': " + dt.Rows[0]["billstot"].ToString() + ",'cgstValue': " + dt.Rows[0]["cgst_val"].ToString() + ",'sgstValue': " + dt.Rows[0]["sgst_val"].ToString() + "," +
                               "'igstValue': " + dt.Rows[0]["igst_val"].ToString() + ",'cessValue': " + dt.Rows[0]["cess_val"].ToString() + ",";
                        }
                        else
                        {
                            AA = "{'supplyType':'" + dt.Rows[0]["sup_type"].ToString() + "','subSupplyType': '" + dt.Rows[0]["sub_type"].ToString() + "'," +
                                "'subSupplyDesc':'" + vsubsupdesc + "','docType':'" + dt.Rows[0]["doc_type"].ToString() + "','docNo': '"+ pfix + dt.Rows[0]["doc_no"].ToString() + "'," +
                                "'docDate': '" + dt.Rows[0]["doc_dt"].ToString() + "','fromGstin':'" + dt.Rows[0]["sup_gst"].ToString() + "','fromTrdName':'" + dt.Rows[0]["sup_nam"].ToString() + "'" +
                                ",'fromAddr1': '" + dt.Rows[0]["sup_add1"].ToString() + "','fromAddr2':'" + dt.Rows[0]["sup_add2"].ToString() + "','fromPlace': '" + dt.Rows[0]["sup_add3"].ToString() + "'" +
                                ",'fromPincode':'" + dt.Rows[0]["sup_pin"].ToString() + "','fromStateCode':  '" + dt.Rows[0]["sup_state"].ToString() + "'," +
                                "'actFromStateCode':'" + dt.Rows[0]["sup_state"].ToString() + "','toGstin': '" + dt.Rows[0]["rec_gst"].ToString() + "'," +
                                "'toTrdname': '" + dt.Rows[0]["rec_nam"].ToString() + "','toAddr1': '" + cs_add1 + "','toAddr2': '" + cs_add2 + "','toPlace': '" + cs_add3 + "'," +
                                "'toPincode': '" + cs_pinc + "','toStateCode': '" + dt.Rows[0]["rec_state"].ToString() + "','actToStateCode': '" + cs_st_cd + "'," +
                                "'totalValue': " + dt.Rows[0]["billstot"].ToString() + ",'cgstValue': " + dt.Rows[0]["cgst_val"].ToString() + ",'sgstValue': " + dt.Rows[0]["sgst_val"].ToString() + "," +
                                "'igstValue': " + dt.Rows[0]["igst_val"].ToString() + ",'cessValue': " + dt.Rows[0]["cess_val"].ToString() + ",";

                        }
                        BB = "'cessNonAdvolValue':0,'otherValue':" + (-1 * fgen.make_double(dt.Rows[0]["oth_chgs"].ToString()) + fgen.make_double(dt.Rows[0]["tcs_amt"].ToString())) + "," +
                            "'transMode': '" + dt.Rows[0]["tran_mode"].ToString() + "','transDistance': " + dt.Rows[0]["tran_dist"].ToString() + "," +
                            "'transporterName': '" + dt.Rows[0]["tran_name"].ToString() + "','transporterId': '" + transid + "','transDocNo': '" + dt.Rows[0]["tran_doc"].ToString() + "'," +
                            "'transDocDate': '" + dt.Rows[0]["tran_dt"].ToString() + "','vehicleNo': '" + dt.Rows[0]["vehi_no"].ToString() + "','vehicleType': 'R'," +
                            "'totInvValue': " + dt.Rows[0]["billgtot"].ToString() + ",'mainHsnCode': " + dt.Rows[0]["hs_code"].ToString() + ",'transactionType':'" + tran_type + "','itemList':[";
                        do
                        {
                            vhscode = dt.Rows[d]["hs_code"].ToString().Replace(".", "");
                            vprod_name = fgen.seek_iname(frm_qstr, frm_cocd, "select substr(trim(name),1,100) as aa from typegrp where id='T1' and trim(acref)='" + dt.Rows[d]["hs_code"].ToString().Trim() + "' ", "aa");
                            dd = "{'productName': '" + vprod_name + "','productDesc': '" + dt.Rows[d]["prod_desc"].ToString() + "','hsnCode':" + dt.Rows[d]["hs_code"].ToString() + "," +
                                "'quantity': " + dt.Rows[d]["quantity"].ToString() + ",'qtyUnit': '" + dt.Rows[d]["quan_unit"].ToString() + "','taxableAmount': " + dt.Rows[d]["taxb_val"].ToString() + "," +
                                "'sgstRate':" + dt.Rows[d]["sgst_rt"].ToString() + ",'cgstRate': " + dt.Rows[d]["cgst_rt"].ToString() + ",'igstRate': " + dt.Rows[d]["igst_rt"].ToString() + "," +
                                "'cessRate': " + dt.Rows[d]["cess_rt"].ToString() + ",'cessNonAdvol':" + 0 + "  }";

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
                  
                }
            }
        }
        if ((FOPT == "JSON") || (FOPT == "WEBT"))
        {
            if ((FOPT == "JSON") || (FOPT == "WEBT"))
            {
                //TextWriter tw = File.CreateText(@"c:\TEJ_ERP\EwayBillTest.JSON");
                // string filePath = "c:\\TEJ_erp\\EwayBillWeb.JSON";
                Random rnd = new Random();
                var t = rnd.Next(10000000, 99999999).ToString();
                string filePath = "c:\\TEJ_erp\\UPLOAD\\EwayBillWeb_" + t + ".JSON";

                StreamWriter w;
                w = File.CreateText(filePath);
                int ii = 0;
              

                List<string> list = new List<string>(Edesc);
                list.RemoveAll(str => String.IsNullOrEmpty(str));

                var DATA = string.Join("", list);
                DATA = DATA = DATA.Substring(0, DATA.Length - 1);
                w.WriteLine(DATA);
                w.Flush();
                w.Close();
                fgen.msg("", "ASMG", "JSON File Generated AT " + filePath + " Upload this File to GST PORTAL to Generate EWAY BILLThen Update on This Screen and Save");

                #region 
                string mq7 = @"c:\TEJ_ERP\upload\";
                if (!Directory.Exists(mq7)) Directory.CreateDirectory(mq7);
              
                #endregion

               
            }
            if (FOPT == "WEBT")
            {
                int ii = 0;
                //AA = "{'version': '1.0.0621','billLists':[";
                AA = AA.Replace("'", "\"");
                string myjs = "";
              

                List<string> list = new List<string>(Edesc);
                list.RemoveAll(str => String.IsNullOrEmpty(str));
                string DATA = "";
                int row_nm = 0;
                string errmsg = "";
                string sucsmsg = "";
                foreach (var way in list) {
                    DATA = way.Substring(0, way.Length - 1);

                    string makewebrequest = "";
                    DATA = DATA.Replace("'", "\"");
                    res = MakeWebRequest("POST", g_api_link, DATA);
                    makewebrequest = res.Trim().Replace("\\", "");

                    //makewebrequest = "{\"status\":1,\"data\":{\"ewayBillNo\":\"301433278498\",\"ewayBillDate\":\"08/04/2022 03:44:00 PM\",\"validUpto\":\"09/04/2022 11:59:00 PM\",\"alert\":\", Distance between these two pincodes is 6, \"}}";
                    var dicddd = new Dictionary<string, object>();
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    ArrayList itemss = jss.Deserialize<ArrayList>(makewebrequest);
                    ArrayList itemsss = jss.Deserialize<ArrayList>(makewebrequest);
                    row_nm++;
                    var eway = "";
                    try
                    {
                        JObject json = JObject.Parse(makewebrequest);
                        var tname = ((Newtonsoft.Json.Linq.JProperty)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)json.Last).First).First)).Name.ToString().Trim();
                        if (tname.Equals("ewayBillNo"))
                        {
                            eway = ((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)((Newtonsoft.Json.Linq.JContainer)json.Last).First).First).First.ToString();
                            ((TextBox)sg1.Rows[row_nm-1].FindControl("sg1_t8")).Text = eway;
                            sucsmsg += row_nm;
                        }
                    }
                    catch (Exception err) {
                        errmsg += "," +row_nm;
                    }

                    Random rnd = new Random();
                    var t = rnd.Next(100000, 999999).ToString();
                    string filePath1 = "c:\\TEJ_erp\\UPLOAD\\WTEWAYBILL_RES_" + t + ".JSON";
                    StreamWriter w1;
                    w1 = File.CreateText(filePath1);
                    w1.WriteLine(makewebrequest);
                    w1.Flush();
                    w1.Close();
                    //Console.WriteLine(Console.Read());
                    
                   

                }

                string msg = "";
                if (sucsmsg.Equals("")) msg = "Eway Bill Generate for Lines " + sucsmsg;
                if (errmsg.Equals("")) msg = "Error In Lines " + errmsg;
                fgen.msg("-", "AMSG", "File has been downloaded at c:\\tej_erp\\upload and " + msg);
                return;

            }
            //-------------------------------
        }
        // tw.WriteLine(cc);
    }



    protected void btnwebtel_ServerClick(object sender, EventArgs e)
    {


        if (txtvchnum.Text.Trim().Length < 6)
        {
            fgen.msg("", "ASMG", "Please Press New to Start");
            return;

        }
        if (txtlbl7.Text == "-")
        {
            fgen.msg("", "ASMG", "Pin Code reqd , put in branch master");
            return;
        }

        if (web_eway_ok == "N")
        {
            fgen.msg("", "ASMG", "Please Get GST Utility Activated");
            return;
        }
        gen_eway_bill("WEBT");
        used_opt = "-";
    }

    public string MakeWebRequest(string method, string url, string post_data)
    {
        string a = "", responseString="";
        //Response.Write("http://www.808.dk/", "GET", ""));

        var request = (HttpWebRequest)WebRequest.Create(url);
        var data = Encoding.ASCII.GetBytes(post_data);

        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = data.Length;

        request.Headers["GSTIN"] = "06AHBPR1750J1ZD";
        request.Headers["Username"] = "Mas_Techno_API_blb";
        request.Headers["Password"] = "Mastech@123";
        request.Headers["CustomerId"] = "ASP10121";
        request.Headers["APIId"] = "V3g3dAoS-gfkj-Mxbl-rdb3-NlFoYmuU";
        request.Headers["APISecret"] = "V3g3dAoSgfkjMxbl";
        request.Headers["Source"] = "API";
        request.Headers["Environment-type"] = "Production";


        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        var response = (HttpWebResponse)request.GetResponse();
        responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        return responseString;
    }

    public HttpWebResponse MakeWebRequestPrint(string url)
    {
        string a = "", responseString = "";
        //Response.Write("http://www.808.dk/", "GET", ""));

        var request = (HttpWebRequest)WebRequest.Create(url);

        request.Method = "GET";
        request.ContentType = "application/json";

        request.Headers["GSTIN"] = "06AHBPR1750J1ZD";
        request.Headers["Username"] = "Mas_Techno_API_blb";
        request.Headers["Password"] = "Mastech@123";
        request.Headers["CustomerId"] = "ASP10121";
        request.Headers["APIId"] = "V3g3dAoS-gfkj-Mxbl-rdb3-NlFoYmuU";
        request.Headers["APISecret"] = "V3g3dAoSgfkjMxbl";
        request.Headers["Source"] = "API";
        request.Headers["Environment-type"] = "Production";
        return (HttpWebResponse)request.GetResponse();

    }

    protected void showbtn_ServerClick(object sender, EventArgs e)
    {
        if (txtvchnum.Text.Trim().Length < 6)
        {
            fgen.msg("", "ASMG", "Please Press New to Start");
            return;
        }

        if (sg1.Rows[0].Cells[17].Text.Trim().Length > 2)
        {
            fgen.msg("", "ASMG", "Invoice Already Selected , Please Make New Sheet");
            return;
        }

        hffield.Value = "Show";
        fgen.Fn_open_prddmp1("Select Date", frm_qstr);
    }
    protected void jsonbtn_ServerClick(object sender, EventArgs e)
    {
        if (txtvchnum.Text.Trim().Length < 6)
        {
            fgen.msg("", "ASMG", "Please Press New to Start");
            return;

        }
        if (txtlbl7.Text == "-")
        {
            fgen.msg("", "ASMG", "Pin Code reqd , put in branch master");
            return;
        }

        if (web_eway_ok == "N")
        {
            fgen.msg("", "ASMG", "Please Get Webtel Utility Activated");
            return;
        }

        gen_eway_bill("WEBT");
        used_opt = "-";
    }

    protected void btneway_ServerClick(object sender, EventArgs e)
    {
        if (txtvchnum.Text.Trim().Length < 6)
        {
            fgen.msg("", "ASMG", "Please Press New to Start");
            return;

        }
        if (txtlbl7.Text == "-")
        {
            fgen.msg("", "ASMG", "Pin Code reqd , put in branch master");
            return;
        }

        if (web_eway_ok == "N")
        {
            fgen.msg("", "ASMG", "Please Get Webtel Utility Activated");
            return;
        }

        gen_eway_bill("WEBT");
        used_opt = "-";
    }
    protected void command2_ServerClick(object sender, EventArgs e)
    {
        if (txtvchnum.Text.Trim().Length < 6)
        {
            fgen.msg("", "ASMG", "Please Press New to Start");
            return;
        }
        if (txtlbl7.Text == "-")
        {
            fgen.msg("", "ASMG", "Pin Code reqd , put in branch master");
            return;
        }
        gen_eway_bill("JSON");
        used_opt = "-";
    }
    void do_upd_tran_file(string upd_tb, int row_nm)
    {
        DataSet odsS = new DataSet();
        DataRow oporows = null;
        DataTable rssample1 = new DataTable();
        DataTable rssample = new DataTable();
        //select branchcd||doc_type||trim(doc_no)||to_char(doc_Dt,'dd/mm/yyyy')||trim(acode) as fstr,eway_bill from ewayb_rec where branchcd='" + frm_mbr + "' and vchnum='" + txtvchnum.Text + "' and vchdate =to_DatE('" + txtvchdate.Text + "','dd/mm/yyyy') order by branchcd||doc_type||trim(doc_no)||to_char(doc_Dt,'dd/mm/yyyy')||acode
        rssample1 = fgen.getdata(frm_qstr, frm_cocd, "select branchcd||doc_type||trim(doc_no)||to_char(doc_Dt,'dd/mm/yyyy')||trim(acode) as fstr,eway_bill from ewayb_rec where branchcd='" + frm_mbr + "' and vchnum='" + txtvchnum.Text + "' and vchdate =to_DatE('" + txtvchdate.Text + "','dd/mm/yyyy') order by branchcd||doc_type||trim(doc_no)||to_char(doc_Dt,'dd/mm/yyyy')||acode");
        //
        if (rssample1.Rows.Count > 0)
        {
            for (int x = 0; x < rssample1.Rows.Count; x++)
            {
                //, eway_bill='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t19")).Text + "'
                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE ewayb_rec SET eway_bill='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t8")).Text + "' WHERE branchcd||type||vchnum||to_char(Vchdate,'dd/mm/yyyy')||trim(acode)='" + rssample1.Rows[x]["fstr"].ToString().Trim() + "' ");
            }

            //
            SQuery = "select distinct trim(branchcd)||trim(type)||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy')||trim(acode) as fstr,st_entform from " + upd_tb + " where vchnum='" + sg1.Rows[row_nm].Cells[17].Text.Trim() + "' and branchcd||type||vchnum||to_char(Vchdate,'dd/mm/yyyy') in (select branchcd||doc_type||trim(doc_no)||to_char(doc_Dt,'dd/mm/yyyy') from ewayb_rec where branchcd='" + frm_mbr + "' and vchnum='" + txtvchnum.Text + "' and  vchdate =to_DatE('" + txtvchdate.Text + "','dd/mm/yyyy')) order by trim(branchcd)||trim(type)||trim(vchnum)||to_char(Vchdate,'dd/mm/yyyy')||trim(acode)";
            rssample = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (rssample.Rows.Count > 0)
                for (int x = 0; x < rssample.Rows.Count; x++)
                {
                    DataView dv = new DataView(rssample1, "FSTR='" + rssample.Rows[x]["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                    if (rssample.Rows.Count > 0)
                    {
                        fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE " + upd_tb + "  SET st_entform='" + ((TextBox)sg1.Rows[row_nm].FindControl("sg1_t8")).Text + "' WHERE branchcd||type||vchnum||to_char(Vchdate,'dd/mm/yyyy')||trim(acode)='" + rssample.Rows[x]["fstr"].ToString().Trim() + "' ");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "commit");
                    }
                }
        }
    }
    void upd_addl_sal_exp(string inv_Refnum)
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
            DataView dv1 = new DataView(rsitms, "FSTR='" + rsitms1.Rows[i]["fstr"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
            rs = dv1.ToTable();
            for (int o = 0; o < rs.Rows.Count; o++)
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE ivoucher SET exp_punit='" + fgen.make_double(rs.Rows[o]["totexp"].ToString().Trim()) + "',rej_rw='" + fgen.make_double(rs.Rows[o]["Tottx1"].ToString().Trim()) + "',rej_sdv='" + fgen.make_double(rs.Rows[o]["Tottx2"].ToString().Trim()) + "' where branchcd||type||vchnum||to_char(Vchdate,'dd/mm/yyyy')='" + rs.Rows[o]["fstr"].ToString().Trim() + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "commit");
            }
        }
    }

    ////#region new

    ////Session["send_dt"] = dt;
    ////mq7 = @"c:\TEJ_ERP\upload\";
    ////if (!Directory.Exists(mq7)) Directory.CreateDirectory(mq7);

    ////fileName = "mul_upload_web" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");
    ////filepath = @"c:\TEJ_ERP\upload\" + fileName + ".txt";
    ////write_to_txt(ph_tbl, filepath);
    //////zipFilePath += "," + mq7 + fileName;
    //////zipFileName += "," + fileName;
    //////dt.Clear();

    //////zipFilePath = zipFilePath.TrimStart(',');
    //////zipFileName = zipFileName.TrimStart(',');

    ////Session["FilePath"] = fileName + ".txt";
    ////Session["FileName"] = fileName + ".txt";
    ////Response.Write("<script>");
    ////Response.Write("window.open('../fin-base/dwnlodFile.aspx','_blank')");
    //////Response.Write("window.open('../fin-base/makeZipDwnload.aspx','_blank')");
    ////Response.Write("</script>");
    ////fgen.msg("-", "AMSG", "File has been downloaded at " + fileName + ".txt" + "");
    ////#endregion

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

    protected void btnprint_ServerClick1(object sender, EventArgs e)
    {

        hffield.Value = "PrintWay";
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("Select Entry ", frm_qstr);
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);

        //res = MakeWebRequest("GET", g_api_link, DATA);

    }
}

//CLIENTGRP - have to see what it is to be set at login page