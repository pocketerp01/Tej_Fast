using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_Contain_detail : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0; string mq0, mq1, mq2;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string ord_qty_valid;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
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
            setColHeadings();
            set_Val();
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
            dtCol = fgen.getdata(frm_qstr, frm_cocd, "SELECT UPPER(OBJ_NAME) AS OBJ_NAME,OBJ_CAPTION,OBJ_WIDTH,UPPER(OBJ_VISIBLE) AS OBJ_VISIBLE,nvl(col_no,0) as COL_NO,nvl(OBJ_MAXLEN,0) as OBJ_MAXLEN,nvl(OBJ_READONLY,'N') as OBJ_READONLY,NVL(OBJ_FMAND,'N') AS OBJ_FMAND FROM SYS_CONFIG WHERE UPPER(TRIM(FRM_NAME))='" + frm_formID + "' order by srno");
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
                    //sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                    //sg1.Rows[0].Cells[sR].Style.Add("width", mcol_width + "px");
                }
            }
        }

        //txtlbl8.Attributes.Add("readonly", "readonly");
        //txtlbl9.Attributes.Add("readonly", "readonly");
        // to hide and show to tab panel
        tab5.Visible = false;
        tab4.Visible = false;
        tab3.Visible = false;
        tab2.Visible = false;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        //switch (Prg_Id)
        //{
        //    case "M09024":
        //    case "M10003":
        //    case "M11003":
        //    case "M10012":
        //    case "M11012":
        //    case "M12008":
        //        tab3.Visible = false;
        //        tab4.Visible = false;
        //        break;
        //}
        //if (Prg_Id == "M12008")
        //{
        //    tab5.Visible = true;
        //    txtlbl8.Attributes.Remove("readonly");
        //    txtlbl9.Attributes.Remove("readonly");
        //}
        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        create_tab();
        create_tab2();
        create_tab3();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();
        btnlbl4.Enabled = false; btnlbl5.Enabled = false; btnshipline.Enabled = false; btnconsign.Enabled = false; btnnotify.Enabled = false; btnNotify2.Enabled = false;
        btnlbl7.Enabled = false; btnmbr.Enabled = false;
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
        btnprint.Disabled = false; btnlist.Disabled = false;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlbl4.Enabled = true; btnlbl7.Enabled = true; btnprint.Disabled = true; btnlist.Disabled = true;
        btnmbr.Enabled = true;
        btnlbl5.Enabled = true; btnshipline.Enabled = true; btnconsign.Enabled = true; btnnotify.Enabled = true; btnNotify2.Enabled = true;
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
        lblheader.Text = "Container Details";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
        frm_tabname = "wb_exp_frt";
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
            case "TACODE":
                //pop1
                SQuery = "SELECT DISTINCT trim(type1) as fstr , name , trim(type1) as code from typegrp where id='^9'";
                break;

            case "SHIPLINE":
                SQuery = "SELECT DISTINCT trim(type1) as fstr , name , trim(type1) as code from typegrp where id='^A'";
                break;

            case "NATURE":
                SQuery = "SELECT DISTINCT trim(type1) as fstr , name , trim(type1) as code from typegrp where id='^B'";
                break;

            case "NOTIFY":
            case "NOTIFY2":
            case "CONSIGN":
                SQuery = "select trim(acode) as fstr, trim(aname) as name , trim(acode) as code from csmst order by trim(acode)";
                break;

            case "MBR":
                SQuery = "select 'H' AS FSTR,'HEAD OFFICE' AS SELECTION,'YES' AS CHOICE FROM DUAL UNION ALL SELECT 'M' AS FSTR,'CURRENT BRANCH' AS SELECTION,'NO' AS CHOICE FROM DUAL";
                break;

            case "TICODE":
                //pop2
                SQuery = "SELECT Type1 AS FSTR,NAME AS Deptt,Type1 AS CODE FROM type where id='V' and type1 like '4%' order by Name";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[13].Text.Trim().Length > 2)
                    {
                        if (col1.Length > 0) col1 = col1 + "," + "'" + gr.Cells[13].Text.Trim() + gr.Cells[9].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[13].Text.Trim() + gr.Cells[9].Text.Trim() + "'";
                    }
                }
                if (col1.Length > 0)
                {
                    col1 = " and trim(a.invno)||(a.invdate) not in (" + col1 + ")";
                }
                else
                {
                    col1 = "";
                }
                PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                //SQuery = "select a.branchcd||a.type||trim(a.invno)||a.invdate as fstr,a.invno,a.invdate ,trim(a.acode) as party_code,trim(b.aname) as party_name from (select distinct branchcd,type,trim(vchnum) as invno,to_char(vchdate,'dd/mm/yyyy') as invdate,trim(acode) as acode ,1 as qty from IVOUCHERP where branchcd='" + frm_mbr + "' and type='4F' and vchdate " + PrdRange + " union all select distinct BRANCHCD,'4F' AS TYPE,trim(INVNO) as invno,to_char(INVDATE,'dd/mm/yyyy') as invdate,trim(acode) as acode,-1 AS QTY from wb_exp_frt WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + PrdRange + ") a,famst b where trim(a.acode)=trim(b.acode)  " + col1 + " group by a.branchcd||a.type||trim(a.invno)||a.invdate ,invno,a.invdate ,trim(a.acode),trim(b.aname) having sum(qty)>0 order by fstr"; //old
                SQuery = "select a.branchcd||a.type||trim(a.invno)||a.invdate as fstr,a.invno,a.invdate ,trim(a.acode) as party_code,trim(b.aname) as party_name from (select distinct branchcd,type,trim(vchnum) as invno,to_char(vchdate,'dd/mm/yyyy') as invdate,trim(acode) as acode ,1 as qty from IVOUCHERP where branchcd='" + frm_mbr + "' and type='4F' and vchdate " + PrdRange + " union all select distinct BRANCHCD,'4F' AS TYPE,trim(INVNO) as invno,to_char(INVDATE,'dd/mm/yyyy') as invdate,trim(acode) as acode,-1 AS QTY from wb_exp_frt WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + PrdRange + ") a,famst b where trim(a.acode)=trim(b.acode)  " + col1 + " group by a.branchcd||a.type||trim(a.invno)||a.invdate ,invno,a.invdate ,trim(a.acode),trim(b.aname) having sum(qty)>0 order by fstr";
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
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,A.CONTAINER_NO,A.CONT_SIZE,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
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
    void newCase(string vty)
    {
        #region
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();
        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        // hffield.Value = "TACODE";
        //fgen.Fn_open_prddmp1("-", frm_qstr);
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
        Cal();
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
        if(txtmbrcode.Text.Length<2)
        {
            fgen.msg("-", "AMSG", "Please Select Branch Address!!");
            return;
        }
        //if (txtlbl2.Text.Trim().Length <= 1)
        //{
        //    fgen.msg("-", "AMSG", "Please Fill Container No!!");
        //    return;
        //}
        //if (txtlbl3.Text.Trim().Length <= 1)
        //{
        //    fgen.msg("-", "AMSG", "Please Fill Container Size!!");
        //    return;
        //}
        //if (txtlbl4.Text.Trim().Length <= 1)
        //{
        //    fgen.msg("-", "AMSG", "Please Select Forwarding Agents!!");
        //    return;
        //}

        //if (txtlbl5.Text.Trim().Length <= 1)
        //{
        //    fgen.msg("-", "AMSG", "Please Select Nature of Shipment!!");
        //    return;
        //}
      
        //if (txtconsigncode.Text.Trim().Length <= 1)
        //{
        //    fgen.msg("-", "AMSG", "Please Select Consignee!!");
        //    return;
        //}
        //if (txtnotcode.Text.Trim().Length <= 1)
        //{
        //    fgen.msg("-", "AMSG", "Please Select Notify!!");
        //    return;
        //}
        //if (txtlbl8.Text.Trim().Length <= 1)
        //{
        //    fgen.msg("-", "AMSG", "Please Select Notify2!!");
        //    return;
        //}
        if (sg1.Rows.Count <= 1)
        {
            fgen.msg("-", "AMSG", "No Export Invoice to Save!!'13'Please Select Some Export Invoice First"); return;
        }

        for (i = 0; i < sg1.Rows.Count-1; i++)
        {
            dhd = fgen.ChkDate(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToString());
            if (dhd == 0)
            { fgen.msg("-", "AMSG", "Please Fill Valid Shipping Date"); txtvchdate.Focus(); return; }

            //if (((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim() == "-")
            //{
            //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Duty Saved(INR) At Line No. " + sg1.Rows[i].Cells[12].Text.Trim());
            //    return;
            //}
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
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||TRIM(a.type)||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
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
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
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
                        txtmbr.Text = dt.Rows[0]["OBSV7"].ToString().Trim();
                        txtmbrcode.Text = dt.Rows[0]["obsv10"].ToString().Trim();
                        txtlbl4.Text= dt.Rows[0]["OBSV4"].ToString().Trim();                       
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(name) as name from typegrp where id='^9' and trim(type1)='" + txtlbl4.Text.Trim() + "'", "name");
                        txtnatcode.Text = dt.Rows[0]["OBSV1"].ToString().Trim();
                        txtlbl5.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(name) as name from typegrp where id='^B' and type1='" + txtnatcode.Text.Trim() + "'", "name");
                        txtshipcode.Text = dt.Rows[0]["OBSV2"].ToString().Trim();
                        txtshipline.Text = dt.Rows[0]["OBSV11"].ToString().Trim();// fgen.seek_iname(frm_qstr, frm_cocd, "select trim(name) as name from typegrp where id='^A' and type1='" + txtshipcode.Text.Trim() + "'", "name");
                        txtocnvess.Text = dt.Rows[0]["ciname"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[0]["CONTAINER_NO"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[0]["CONT_SIZE"].ToString().Trim();
                        txtconsigncode.Text = dt.Rows[0]["CSCODE"].ToString().Trim();
                        txtconsign.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(aname) as name from csmst where trim(acode)='" + txtconsigncode.Text.Trim() + "'", "name");
                        txtnotcode.Text = dt.Rows[0]["VCODE"].ToString().Trim();
                        txtnotifyname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(aname) as name from csmst where trim(acode)='" + txtnotcode.Text.Trim() + "'", "name");
                        txtlbl8.Text = dt.Rows[0]["vcode2"].ToString().Trim();
                        txtlbl9.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(aname) as name from csmst where trim(acode)='" + txtlbl8.Text.Trim() + "'", "name");
                        txtportdisc.Text = dt.Rows[0]["PORT_DESCH"].ToString().Trim();
                        txtpdelv.Text = dt.Rows[0]["DELV"].ToString().Trim();
                        txtrcpt.Text = dt.Rows[0]["PRECIEPT"].ToString().Trim();
                        txtload.Text = dt.Rows[0]["PLOAD"].ToString().Trim();                   
                        txtlbl20.Text = dt.Rows[0]["rmk1"].ToString().Trim();
                        txtlbl21.Text = dt.Rows[0]["rmk2"].ToString().Trim();
                        txtspclinstn.Text = dt.Rows[0]["rmk3"].ToString().Trim();
                        txtlbl22.Text = dt.Rows[0]["obsv8"].ToString().Trim();//freight rmk3
                        txtlbl23.Text = dt.Rows[0]["obsv9"].ToString().Trim();//freight rmk4                         
                        txtbl.Text = dt.Rows[0]["OBSV3"].ToString().Trim();
                        txtdispdate.Text = dt.Rows[0]["date1"].ToString().Trim();
                        txtsuffdate.Text = dt.Rows[0]["date2"].ToString().Trim();
                        txtrailout.Text = dt.Rows[0]["date3"].ToString().Trim();
                        txtsobdate.Text = dt.Rows[0]["date4"].ToString().Trim();
                        txteta.Text = dt.Rows[0]["date5"].ToString().Trim();
                        txtdocsent.Text = dt.Rows[0]["date6"].ToString().Trim();
                        txtdodate.Text = dt.Rows[0]["date7"].ToString().Trim();
                        txtwaredate.Text = dt.Rows[0]["date8"].ToString().Trim();                       
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
                            sg1_dr["sg1_h8"] = dt.Rows[i]["acode"].ToString().Trim();
                            sg1_dr["sg1_h9"] = dt.Rows[i]["hscode"].ToString().Trim();
                            sg1_dr["sg1_h10"] = Convert.ToDateTime(dt.Rows[i]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_f1"] = dt.Rows[i]["invno"].ToString().Trim();
                            sg1_dr["sg1_f2"] = fgen.make_double(dt.Rows[i]["iamount"].ToString().Trim());
                            sg1_dr["sg1_f3"] = Convert.ToDateTime(dt.Rows[i]["refdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["CIF_VAL"].ToString().Trim());
                            sg1_dr["sg1_f5"] = fgen.make_double(dt.Rows[i]["FORIGN_VAL"].ToString().Trim());
                            sg1_dr["sg1_f6"] = fgen.make_double(dt.Rows[i]["iqtyin"].ToString().Trim());
                            sg1_dr["sg1_f7"] = "-";
                            sg1_dr["sg1_t1"] = dt.Rows[i]["NUM1"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["NUM2"].ToString().Trim();                        
                            sg1_dr["sg1_t3"] = dt.Rows[i]["comm_desc"].ToString().Trim();//full_commidity_Desc                      
                            sg1_dr["sg1_t4"] = dt.Rows[i]["OBSV5"].ToString().Trim();//PACKAGE                         
                            sg1_dr["sg1_t5"] = dt.Rows[i]["OBSV6"].ToString().Trim();//SHIPPING BILL NO                        
                            sg1_dr["sg1_t6"] = Convert.ToDateTime(dt.Rows[i]["ship_dt"]).ToString("dd/MM/yyyy");//shipping bill date
                            sg1_dr["sg1_t7"] = dt.Rows[i]["NUM7"].ToString().Trim();
                            sg1_dr["sg1_t27"] = dt.Rows[i]["NUM8"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["NUM9"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["NUM10"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["NUM11"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["NUM12"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["NUM13"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["NUM14"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["NUM15"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["NUM16"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["NUM17"].ToString().Trim();
                            sg1_dr["sg1_t17"] = dt.Rows[i]["NUM18"].ToString().Trim();
                            sg1_dr["sg1_t18"] = dt.Rows[i]["NUM19"].ToString().Trim();
                         
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_add_blankrows();
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

                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_esales_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    btnlbl5.Focus();
                    break;
                case "SHIPLINE":
                    if (col1.Length <= 0) return;
                    txtshipcode.Text = col1;
                    txtshipline.Text = col2;
                    txtocnvess.Focus();
                    break;
                case "NATURE":
                    if (col1.Length <= 0) return;
                    txtnatcode.Text = col1;
                    txtlbl5.Text = col2;
                    btnshipline.Focus();
                    break;
                case "CONSIGN":
                    if (col1.Length <= 0) return;
                    txtconsigncode.Text = col1;
                    txtconsign.Text = col2;
                    btnnotify.Focus();
                    break;
                case "NOTIFY":
                    if (col1.Length <= 0) return;
                    txtnotcode.Text = col1;
                    txtnotifyname.Text = col2;
                    break;
                case "NOTIFY2":
                    if (col1.Length <= 0) return;
                    txtlbl8.Text = col1;
                    txtlbl9.Text = col2;
                    break;
                case "MBR":
                    if (col1.Length <= 0) return;
                    if (col1 == "H")
                    {
                        txtmbrcode.Text = "00";
                        txtmbr.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select distinct name from type where id='B' AND TYPE1='" + txtmbrcode.Text + "'", "name");
                    }
                    else
                    {
                        txtmbrcode.Text = frm_mbr;
                        txtmbr.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select distinct name from type where id='B' AND TYPE1='" + frm_mbr + "'", "name");
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
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
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
                            sg1_dr["sg1_f6"] = dt.Rows[i]["sg1_f6"].ToString();
                            sg1_dr["sg1_f7"] = dt.Rows[i]["sg1_f7"].ToString();
                            sg1_dr["sg1_f8"] = dt.Rows[i]["sg1_f8"].ToString();
                            sg1_dr["sg1_f9"] = dt.Rows[i]["sg1_f9"].ToString();
                            sg1_dr["sg1_f10"] = dt.Rows[i]["sg1_f10"].ToString();
                            sg1_dr["sg1_f11"] = dt.Rows[i]["sg1_f11"].ToString();
                            sg1_dr["sg1_f12"] = dt.Rows[i]["sg1_f12"].ToString();
                            sg1_dr["sg1_f13"] = dt.Rows[i]["sg1_f13"].ToString();
                            sg1_dr["sg1_f14"] = dt.Rows[i]["sg1_f14"].ToString();
                            sg1_dr["sg1_f15"] = dt.Rows[i]["sg1_f15"].ToString();
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
                        dt2 = new DataTable();
                        SQuery1 = "select A.ACODE,A.VCHNUM,to_char(A.vchdate,'dd/mm/yyyy') as vchdate,sum(a.iweight) as iweight,sum(a.btchno) as btchno,B.fob_tot,B.tptbill_dt ,B.no_bdls,b.exc_tarrif from IVOUCHERP A ,salep B  wherE trim(A.vchnum)||to_char(A.vchdate,'dd/mm/yyyy')=trim(B.vchnum)||to_char(B.vchdate,'dd/mm/yyyy') AND trim(A.branchcd)||trim(A.type)||trim(A.vchnum)||to_char(A.vchdate,'dd/mm/yyyy') in (" + col1 + ") GROUP BY A.VCHNUM,to_char(A.vchdate,'dd/mm/yyyy'),B.fob_tot,B.tptbill_dt ,B.no_bdls,b.exc_tarrif,a.acode ORDER BY A.VCHNUM";
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                        mq0 = ""; mq1 = ""; mq2 = "";
                        for (int d = 0; d < dt2.Rows.Count; d++)
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
                            sg1_dr["sg1_h8"] = dt2.Rows[d]["ACODE"].ToString().Trim();
                            sg1_dr["sg1_h9"] = dt2.Rows[d]["exc_tarrif"].ToString().Trim();
                            sg1_dr["sg1_h10"] = Convert.ToDateTime(dt2.Rows[d]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_f1"] = dt2.Rows[d]["vchnum"].ToString().Trim();
                            sg1_dr["sg1_f2"] = fgen.make_double(dt2.Rows[d]["fob_tot"].ToString().Trim());
                            sg1_dr["sg1_f3"] = Convert.ToDateTime(dt2.Rows[d]["tptbill_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_f4"] = fgen.make_double(dt2.Rows[d]["iweight"].ToString().Trim());
                            sg1_dr["sg1_f5"] = fgen.make_double(dt2.Rows[d]["btchno"].ToString().Trim()); ;
                            sg1_dr["sg1_f6"] = fgen.make_double(dt2.Rows[d]["no_bdls"].ToString().Trim());
                            sg1_dr["sg1_f7"] = "";
                            sg1_dr["sg1_f8"] = "";
                            sg1_dr["sg1_f9"] = "";
                            sg1_dr["sg1_f10"] = "";

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
                            sg1_dr["sg1_t30"] = "";
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
                    dt2.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    SQuery1 = "select TRIM(A.ACODE) AS ACODE,TRIM(A.VCHNUM) AS VCHNUM,to_char(A.vchdate,'dd/mm/yyyy') as vchdate,sum(a.iweight) as iweight,sum(a.btchno) as btchno,B.fob_tot,B.tptbill_dt ,B.no_bdls,b.exc_tarrif from IVOUCHERP A ,salep B  wherE trim(A.vchnum)||to_char(A.vchdate,'dd/mm/yyyy')=trim(B.vchnum)||to_char(B.vchdate,'dd/mm/yyyy') AND trim(A.branchcd)||trim(A.type)||trim(A.vchnum)||to_char(A.vchdate,'dd/mm/yyyy') ='" + col1 + "' GROUP BY TRIM(A.ACODE) ,TRIM(A.VCHNUM),to_char(A.vchdate,'dd/mm/yyyy'),B.fob_tot,B.tptbill_dt ,B.no_bdls,b.exc_tarrif  ORDER BY VCHNUM";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in GridView Value
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[8].Text = dt.Rows[d]["ACODE"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[9].Text = dt.Rows[d]["exc_tarrif"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[10].Text = Convert.ToDateTime(dt2.Rows[d]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[d]["vchnum"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt2.Rows[d]["fob_tot"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = Convert.ToDateTime(dt2.Rows[d]["tptbill_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[d]["iweight"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[d]["btchno"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["no_bdls"].ToString().Trim();
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
                        dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
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
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.Trim();
                            sg1_dr["sg1_f7"] = sg1.Rows[i].Cells[19].Text.Trim();
                            sg1_dr["sg1_f8"] = sg1.Rows[i].Cells[20].Text.Trim();
                            sg1_dr["sg1_f9"] = sg1.Rows[i].Cells[21].Text.Trim();
                            sg1_dr["sg1_f10"] = sg1.Rows[i].Cells[22].Text.Trim();
                            sg1_dr["sg1_f11"] = sg1.Rows[i].Cells[23].Text.Trim();
                            sg1_dr["sg1_f12"] = sg1.Rows[i].Cells[24].Text.Trim();
                            sg1_dr["sg1_f13"] = sg1.Rows[i].Cells[25].Text.Trim();
                            sg1_dr["sg1_f14"] = sg1.Rows[i].Cells[26].Text.Trim();
                            sg1_dr["sg1_f15"] = sg1.Rows[i].Cells[27].Text.Trim();
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
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            //SQuery = "select A.container_no,A.CONT_SIZE ,B.NAME AS FORWARD_AGENT,trim(A.invno) as invoice_no,TO_CHAR(A.invdate,'DD/MM/YYYY') AS INVDATE,A.iamount as value,TO_CHAR(A.refdate,'DD/MM/YYYY') as shipment_date,A.cif_val as net_weight,A.forign_val as total_weight,A.iqtyin as no_of_pkgs,A.num1 as pkg_no_from,A.num2 as pkg_no_to,A.num3 as local_charge_inr,A.num4 as local_charge_fc,A.num5 as custom_clr_charges_inr,A.num6 as custom_clr_charges_fc,A.num7 as concor_rail_charges_inr,A.num8 as concor_rail_charges_fc,A.num9 as ocean_freight_inr,A.num10 as ocean_freight_fc,A.Num11 as thc_charges_inr ,A.num12 as thc_charges_fc,A.num13 as doc_charges_inr,A.num14 as doc_charges_fc,A.num15  as  other_charges_inr,A.num16 as other_charges_fc,A.num17 as ex_rate,A.num18 as total_freight_fc,A.num19 as total_freight_inr,to_char(a.vchdate,'yyyymmdd') as vdd from wb_exp_frt A, TYPEGRP B WHERE TRIM(A.obsv4) = TRIM(B.TYPE1) AND B.ID='^9' AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' and a.vchdate " + PrdRange + " order by vdd desc ";
            //SQuery = "select a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,A.container_no,A.CONT_SIZE ,B.NAME AS FORWARD_AGENT,trim(A.invno) as invoice_no,TO_CHAR(A.invdate,'DD/MM/YYYY') AS INVDATE,A.iamount as value,TO_CHAR(A.refdate,'DD/MM/YYYY') as shipment_date,A.cif_val as net_weight,A.forign_val as total_weight,A.iqtyin as no_of_pkgs,A.num1 as pkg_no_from,A.num2 as pkg_no_to,A.num3 as local_charge_inr,A.num4 as local_charge_fc,A.num5 as custom_clr_charges_inr,A.num6 as custom_clr_charges_fc,A.num7 as concor_rail_charges_inr,A.num8 as concor_rail_charges_fc,A.num9 as ocean_freight_inr,A.num10 as ocean_freight_fc,A.Num11 as thc_charges_inr ,A.num12 as thc_charges_fc,A.num13 as doc_charges_inr,A.num14 as doc_charges_fc,A.num15  as  other_charges_inr,A.num16 as other_charges_fc,A.num17 as ex_rate,A.num18 as total_freight_fc,A.num19 as total_freight_inr,to_char(a.vchdate,'yyyymmdd') as vdd from wb_exp_frt A, TYPEGRP B WHERE TRIM(A.obsv4) = TRIM(B.TYPE1) AND B.ID='^9' AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' and a.vchdate " + PrdRange + " order by vdd desc,entry_no desc ";
            SQuery = "select a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,A.container_no,A.CONT_SIZE ,B.NAME AS FORWARD_AGENT,trim(A.invno) as invoice_no,TO_CHAR(A.invdate,'DD/MM/YYYY') AS INVDATE,A.iamount as value,TO_CHAR(A.refdate,'DD/MM/YYYY') as shipment_date,A.cif_val as net_weight,A.forign_val as total_weight,A.iqtyin as no_of_pkgs,A.num1 as pkg_no_from,A.num2 as pkg_no_to,A.num17 as ex_rate,A.num18 as total_freight_fc,A.num19 as total_freight_inr,to_char(a.vchdate,'yyyymmdd') as vdd from wb_exp_frt A, TYPEGRP B WHERE TRIM(A.obsv4) = TRIM(B.TYPE1) AND B.ID='^9' AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' and a.vchdate " + PrdRange + " order by vdd desc,entry_no desc ";
            //
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " For The Period Of " + fromdt + " To " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "TACODE")
        {
            mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_HF1");
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            if (hf2.Value == "SG1_ROW_ADD_E")
            {
                hffield.Value = "SG1_ROW_ADD_E";
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Invoice", frm_qstr);
            }
            else
            {
                hffield.Value = "SG1_ROW_ADD";
                make_qry_4_popup();
                fgen.Fn_open_mseek("Select Invoice", frm_qstr);
            }
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------
            //if (txtlbl4.Text.Trim().Length < 2)
            //{
            //    Checked_ok = "N";
            //    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Customer Not Filled Correctly !!");
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
                            save_it = "N";
                            for (i = 0; i < sg1.Rows.Count - 0; i++)
                            {
                                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2)
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
            //e.Row.Cells[1].Width = 30;
            //e.Row.Cells[1].Width = 30;
            //e.Row.Cells[3].Width = 30;
            sg1.Columns[10].HeaderStyle.Width = 40;
            sg1.Columns[11].HeaderStyle.Width = 40; sg1.Columns[12].HeaderStyle.Width = 60; sg1.Columns[13].HeaderStyle.Width = 100; sg1.Columns[14].HeaderStyle.Width = 100;
            sg1.Columns[15].HeaderStyle.Width = 100; sg1.Columns[16].HeaderStyle.Width = 100; sg1.Columns[17].HeaderStyle.Width = 100; sg1.Columns[15].HeaderStyle.Width = 100;
            sg1.Columns[19].HeaderStyle.Width = 100; sg1.Columns[20].HeaderStyle.Width = 100; sg1.Columns[21].HeaderStyle.Width = 100; sg1.Columns[22].HeaderStyle.Width = 100;
            sg1.Columns[23].HeaderStyle.Width = 100; sg1.Columns[24].HeaderStyle.Width = 100; sg1.Columns[25].HeaderStyle.Width = 100; sg1.Columns[26].HeaderStyle.Width = 100; sg1.Columns[27].HeaderStyle.Width = 100;
            sg1.Columns[28].HeaderStyle.Width = 130; sg1.Columns[29].HeaderStyle.Width = 130; sg1.Columns[30].HeaderStyle.Width = 130; sg1.Columns[31].HeaderStyle.Width = 130;
            sg1.Columns[32].HeaderStyle.Width = 160; sg1.Columns[33].HeaderStyle.Width = 160; sg1.Columns[34].HeaderStyle.Width = 160; sg1.Columns[35].HeaderStyle.Width = 160;
            sg1.Columns[36].HeaderStyle.Width = 130; sg1.Columns[37].HeaderStyle.Width = 130; sg1.Columns[38].HeaderStyle.Width = 130; sg1.Columns[39].HeaderStyle.Width = 130;
            sg1.Columns[40].HeaderStyle.Width = 130; sg1.Columns[41].HeaderStyle.Width = 130; sg1.Columns[42].HeaderStyle.Width = 130; sg1.Columns[43].HeaderStyle.Width = 130;
            sg1.Columns[44].HeaderStyle.Width = 130; sg1.Columns[45].HeaderStyle.Width = 160; sg1.Columns[46].HeaderStyle.Width = 160;

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
                if (txtlbl2.Text.Trim().Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Fill Container Number First !!");
                    return;
                }

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
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Forwarding Agent", frm_qstr);
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
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            mq0 = "";
            if (sg1.Rows[i].Cells[13].Text.Length > 2)
            {
                #region
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
                oporow["SRNO"] = i + 1;
                oporow["obsv4"] = txtlbl4.Text.Trim();
                oporow["OBSV2"] = txtshipcode.Text.Trim().ToUpper();
                oporow["obsv11"] = txtshipline.Text.Trim().ToUpper();
                //mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select TYPE1 from typegrp where id='^A' and trim(NAME)='" + txtshipline.Text.Trim() + "'", "name"); //shipping line master name
                oporow["OBSV1"] = txtnatcode.Text.Trim().ToUpper(); //NATURE SHIP code            
                oporow["CINAME"] = txtocnvess.Text.Trim().ToUpper();
                oporow["SHIP_LINE"] = txtshipline.Text.Trim().ToUpper(); //SHIP LINE
                oporow["icode"] = "-";
                oporow["CONTAINER_NO"] = txtlbl2.Text.Trim().ToUpper(); //CONTAINER NO
                oporow["CONT_SIZE"] = txtlbl3.Text.Trim().ToUpper(); // CONTAINER SIZE              
                oporow["rmk1"] = txtlbl20.Text.Trim().ToUpper();
                oporow["rmk2"] = txtlbl21.Text.Trim().ToUpper();
                oporow["rmk3"] = txtspclinstn.Text.Trim().ToUpper();
                oporow["obsv8"] = txtlbl22.Text.Trim().ToUpper();//freight rmk3
                oporow["obsv9"] = txtlbl23.Text.Trim().ToUpper();  //freight rmk4                  
                oporow["OBSV3"] = txtbl.Text.Trim().ToUpper(); //bl

                oporow["date1"] = txtdispdate.Text.Trim().ToUpper();
                oporow["date2"] = txtsuffdate.Text.Trim().ToUpper();
                oporow["date3"] = txtrailout.Text.Trim().ToUpper();
                oporow["date4"] = txtsobdate.Text.Trim().ToUpper();
                oporow["date5"] = txteta.Text.Trim().ToUpper();
                oporow["date6"] = txtdocsent.Text.Trim().ToUpper();
                oporow["date7"] = txtdodate.Text.Trim().ToUpper();
                oporow["date8"] = txtwaredate.Text.Trim().ToUpper();

                oporow["cscode"] = txtconsigncode.Text.Trim().ToUpper();
                oporow["vcode"] = txtnotcode.Text.Trim().ToUpper();
                oporow["vcode2"] = txtlbl8.Text.Trim().ToUpper();
                oporow["PORT_DESCH"] = txtportdisc.Text.Trim().ToUpper();
                oporow["DELV"] = txtpdelv.Text.Trim().ToUpper();
                oporow["PRECIEPT"] = txtrcpt.Text.Trim().ToUpper();
                oporow["PLOAD"] = txtload.Text.Trim().ToUpper();
                oporow["hscode"] = sg1.Rows[i].Cells[8].Text.Trim().ToUpper();
                oporow["invno"] = sg1.Rows[i].Cells[13].Text.Trim().ToUpper();
                oporow["invdate"] = sg1.Rows[i].Cells[9].Text.Trim().ToUpper();
                oporow["iamount"] = sg1.Rows[i].Cells[14].Text.Trim().ToUpper();
                oporow["refdate"] = sg1.Rows[i].Cells[15].Text.Trim().ToUpper();
                oporow["CIF_VAL"] = sg1.Rows[i].Cells[16].Text.Trim().ToUpper();
                oporow["FORIGN_VAL"] = sg1.Rows[i].Cells[17].Text.Trim().ToUpper();
                oporow["iqtyin"] = sg1.Rows[i].Cells[18].Text.Trim().ToUpper();
                oporow["acode"] = sg1.Rows[i].Cells[7].Text.Trim().ToUpper();
                oporow["num17"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().ToUpper());
                oporow["num18"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim().ToUpper());
                oporow["num19"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim().ToUpper());
                oporow["NUM1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper());
                oporow["NUM2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper());                            
                oporow["comm_desc"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper(); //FULl COMODITY DESC                
                oporow["OBSV5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper();//PACKAGE                         
                oporow["OBSV6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper();//shipping bill no             
                oporow["ship_dt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper();//shipping date             
                oporow["obsv10"] = txtmbrcode.Text.Trim();
                oporow["OBSV7"] = txtmbr.Text.Trim().ToUpper();
                oporow["NUM3"] = 0;
                oporow["NUM4"] = 0;
                oporow["NUM5"] = 0;
                oporow["NUM6"] = 0;
                oporow["NUM7"] = 0;
                oporow["NUM8"] = 0;
                oporow["NUM9"] = 0;
                oporow["NUM10"] = 0;
                oporow["NUM11"] = 0;
                oporow["NUM12"] = 0;
                oporow["NUM13"] = 0;
                oporow["NUM14"] = 0;
                oporow["NUM15"] = 0;
               
                #endregion
             
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
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
    }
    //------------------------------------------------------------------------------------       
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
        }
    }
    //------------------------------------------------------------------------------------
    void Cal()
    {
        double fob_sb = 0;
        double fob_foreign = 0;
        for (int i = 0; i < sg1.Rows.Count - 1; i++)
        {
            fob_foreign = 0; fob_sb = 0;
            fob_sb = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text);
            fob_foreign = fob_sb * fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text);
            ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text = Math.Round(fob_foreign, 2).ToString();
        }
    }
    protected void btnlbl5_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "NATURE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Nature of Shipping ", frm_qstr);
    }
    protected void btnshipline_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SHIPLINE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Shipping Line ", frm_qstr);
    }
    protected void btnnotify_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "NOTIFY";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Notify Name ", frm_qstr);
    }
    protected void btnconsign_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CONSIGN";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Consignee Name ", frm_qstr);
    }
    protected void btnNotify2_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "NOTIFY2";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Notify To Name ", frm_qstr);
    }
    protected void btnmbr_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MBR";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Branch Address ", frm_qstr);
    }
}
/// ADD shit_dt in table .cmd is given below
/// alter table finsagm.wb_exp_frt add SHIP_DT DATE;
