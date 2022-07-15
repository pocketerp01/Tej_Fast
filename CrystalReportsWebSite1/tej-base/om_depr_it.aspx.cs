using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_depr_it : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query, vview60, vview70;
    string frm_mbr, frm_vty, frm_vnum, frm_vnum1, frm_vnum2, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
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
                    //frm_mbr = "01";
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    frm_vnum1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7");
                    frm_vnum2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                doc_addl.Value = "1";
                lblheader.Text = "Depreciation Calculator-Income Tax Act";
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
            lbl1a.Visible = false;
            typePopup = "N";
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
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false; btnprint.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btncal.Enabled = false;
        create_tab();
        create_tab2();
        create_tab3();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();

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
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = true; btnprint.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; btncal.Enabled = true;
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
        frm_tabname = "WB_FA_VCH";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vview60 = "wb_view_60" + frm_mbr + "";
        vview70 = "wb_view_70" + frm_mbr + "";
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
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "80");
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
            case "TACODE":
                //SQuery = "SELECT distinct a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum||trim(a.icode)||trim(a.srno) AS FSTR,trim(a.Vchnum) as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,c.Iname,b.Aname as Supplier,a.Invno,A.Refnum as chl_no from ivoucher a ,famst b,item c where trim(A.icode)=trim(c.icode) and trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + DateRange + " and NVL(a.inspected,'N')='N' order by a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum||trim(a.icode)||trim(a.srno)";
                SQuery = "SELECT type1 as fstr,name as grade_name,Type1 as Grade_Code  from type where id='I' and type1 like '0%'";
                break;
            case "MRESULT":
                SQuery = "SELECT '01' as fstr,'ACCEPTED' as Results,'01' as Qa_Code from dual union all SELECT '02' as fstr,'REJECTED' as Results,'02' as Qa_Code from dual union all SELECT '03' as fstr,'ACCEPT U/Dev.' as Results,'03' as Qa_Code from dual union all SELECT '04' as fstr,'ACCEPT U/Seg.' as Results,'04' as Qa_Code from dual";
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
                //        pquery = "select trim(icode) as icode,sum(cnt) as tot from (select icode,1 as cnt from item where length(trim(nvl(deac_by,'-')))<=1 and length(trim(icode))>4 union all select distinct icode,-1 as cnt from inspmst where branchcd!='DD' and type='" + lbl1a.Text + "' and trim(Acode)='" + txtlbl4.Value.Trim() + "') group by trim(icode) having sum(cnt)>0 ";
                //        SQuery = "SELECT a.Icode AS FSTR,trim(b.Iname) as Item_name,a.Icode,b.Cpartno,b.Cdrgno,b.unit from ("+ pquery +")a ,Item b where trim(A.icode)=trim(B.icode) and length(trim(nvl(b.deac_by,'-')))<=1 and length(trim(b.icode))>4 and substr(b.icode,1,1) in ('7','8','9') order by b.iname";
                //        break;
                //}

                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":

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
                    SQuery = "select distinct trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')||type as fstr,vchnum,to_Char(vchdate,'dd/mm/yyyy') as vchdate,type from WB_FA_VCH where branchcd='" + frm_mbr + "' and type='80' and VCHDATE " + DateRange + " AND  vchnum<>'000000' order by vchnum desc";
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
            //txtvchnum.Value = frm_vnum;
            //txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            //disablectrl();
            //fgen.EnableForm(this.Controls);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
        btnsave.Disabled = true;
    }
    void newCase(string vty)
    {
        #region
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");
        txtvchnum.Value = frm_vnum;
        //txtvchdate.Text = Convert.ToDateTime(frm_CDT2).ToString().Trim();
        txtvchdate.Text = vardate;
        txtlastdt.Text = Convert.ToDateTime(frm_CDT1).AddDays(-1).ToString().Trim();



        //calculate vchnum for type 60
        frm_vnum1 = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='60' ", 6, "VCH");
        //txtvchnum.Value = frm_vnum;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL7", frm_vnum1);


        //calculate vchnum for type 70
        frm_vnum2 = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' ", 6, "VCH");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL8", frm_vnum2);


        disablectrl();
        fgen.EnableForm(this.Controls);

        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();
        int j;
        //for (j = i; j < 10; j++)
        //{
        //    sg1_add_blankrows();
        //}
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
            btnsave.Disabled = true; btncal.Enabled = true;
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
        //string chk_freeze = "";
        ////chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1043", txtvchdate.Text.Trim());
        //if (chk_freeze == "1")
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Rolling Freeze Date !!");
        //    return;
        //}
        //if (chk_freeze == "2")
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Fixed Freeze Date !!");
        //    return;
        //}

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
        //checkcode rkvsv
        hffield.Value = "Print";
        fgen.Fn_open_prddmp1("Choose Time Period", frm_qstr);


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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 16) + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");


                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + "Delete");
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
                    btnsave.Disabled = true;

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

                        //txtlbl4.Value = dt.Rows[i]["frm_name"].ToString().Trim();
                        //txtlbl4a.Text = dt.Rows[i]["text"].ToString().Trim();
                        //txtlbl2.Text = dt.Rows[i]["frm_header"].ToString().Trim();
                        txtlbl7.Value = dt.Rows[0]["ent_id"].ToString().Trim();
                        //txtlbl7a.Text = dt.Rows[0]["ent_by"].ToString().Trim();
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
                case "Print":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "select * from WB_FA_VCH where trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')||type ='" + col1 + "' and branchcd='" + frm_mbr + "' order by vchnum,vchdate,srno";

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
 
                    double d1 = 0, d2 = 0, d3 = 0, d4 = 0, d5 =0, d6 = 0;
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Value = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
  
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

                            sg1_dr["sg1_f1"] = dt.Rows[i]["block"].ToString().Trim();
                            string vname = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(name)||'~'||to_char(num4+num5) as vname from typegrp where id='DI' and type1= '" + dt.Rows[i]["block"].ToString().Trim() + "' ", "vname");
                            sg1_dr["sg1_f2"] = vname.Split('~')[0].ToString();
                            sg1_dr["sg1_f3"] = vname.Split('~')[1].ToString();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["assetval1"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["less180"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["more180"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["sale_it"].ToString().Trim();//swap iqtyin with asseval1
                            sg1_dr["sg1_t3"] = dt.Rows[i]["dramt"].ToString().Trim();//
                            sg1_dr["sg1_t4"] = dt.Rows[i]["depr"].ToString().Trim();

                            d1 += fgen.make_double(dt.Rows[i]["assetval1"].ToString().Trim());
                            d2 += fgen.make_double(dt.Rows[i]["less180"].ToString().Trim());
                            d3 += fgen.make_double(dt.Rows[i]["more180"].ToString().Trim());
                            d4 += fgen.make_double(dt.Rows[i]["sale_it"].ToString().Trim());
                            d5 += fgen.make_double(dt.Rows[i]["dramt"].ToString().Trim());
                            d6 += fgen.make_double(dt.Rows[i]["depr"].ToString().Trim());

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        txtlbl4.Value = (Math.Round(d1, 2)).ToString();
                        txtlbl7.Value = (Math.Round(d2, 2)).ToString();
                        Text3.Value = (Math.Round(d3, 2)).ToString();
                        Text1.Value = (Math.Round(d4, 2)).ToString();
                        Text2.Value = (Math.Round(d5, 2)).ToString();
                        txtlbl101.Value = (Math.Round(d6, 2)).ToString();

                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);

                        disablectrl();
                        txtvchdate.Enabled = false;
                        btncal.Enabled = false;
                        btnsave.Disabled = true;

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
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    SQuery = "select  EMPCODE,NAME, DEPTT_TEXT,DESG_TEXT,DTJOIN from empmas  where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + col1 + "' order by grade";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        //txtvchnum.Value = dt.Rows[0]["vchnum"].ToString().Trim();
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

                        txtlbl4.Value = col1;
                        //txtlbl4a.Text = col2;
                        //txtlbl4.Value = dt.Rows[i]["acode"].ToString().Trim();
                        //txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where trim(upper(acode))=upper(Trim('" + txtlbl4.Value + "'))", "aname");

                        //txtlbl7.Value = dt.Rows[i]["icode"].ToString().Trim();
                        //txtlbl7a.Text = dt.Rows[i]["iname"].ToString().Trim();

                        //txtlbl8.Text = dt.Rows[i]["iqtyin"].ToString().Trim();
                        //txtlbl9.Text = dt.Rows[i]["btchno"].ToString().Trim();
                    }
                    dt.Dispose();
                    // SQuery = "Select * from inspmst a where a.branchcd='" + frm_mbr + "' and a.icode='" + txtlbl7.Value + "' ORDER BY A.srno";
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
                        edmode.Value = "Y";
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
                    //txtlbl7.Value = col1;
                    //txtlbl7a.Text = col2;
                    //txtlbl2.Focus();
                    break;
                case "MRESULT":

                    if (col1.Length <= 0) return;
                    txtlbl101.Value = col1;
                    //txtlbl101a.Text = col2;
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
                    //if (col1.Length <= 0) return;
                    //if (ViewState["sg1"] != null)
                    //{
                    //    dt = new DataTable();
                    //    sg1_dt = new DataTable();
                    //    dt = (DataTable)ViewState["sg1"];
                    //    z = dt.Rows.Count - 1;
                    //    sg1_dt = dt.Clone();
                    //    sg1_dr = null;
                    //    for (i = 0; i < dt.Rows.Count - 1; i++)
                    //    {
                    //        sg1_dr = sg1_dt.NewRow();
                    //        sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                    //        sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
                    //        sg1_dr["sg1_h2"] = dt.Rows[i]["sg1_h2"].ToString();
                    //        sg1_dr["sg1_h3"] = dt.Rows[i]["sg1_h3"].ToString();
                    //        sg1_dr["sg1_h4"] = dt.Rows[i]["sg1_h4"].ToString();
                    //        sg1_dr["sg1_h5"] = dt.Rows[i]["sg1_h5"].ToString();
                    //        sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                    //        sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                    //        sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                    //        sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                    //        sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();

                    //        sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                    //        sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                    //        sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                    //        sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                    //        sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                    //        sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                    //        sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                    //        sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                    //        sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                    //        sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                    //        sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                    //        sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                    //        sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                    //        sg1_dt.Rows.Add(sg1_dr);
                    //    }

                    //    dt = new DataTable();
                    //    if (col1.Length > 6) SQuery = "select * from evas where trim(userid) in (" + col1 + ")";
                    //    else SQuery = "select * from evas where trim(userid)='" + col1 + "'";
                    //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    //    for (int d = 0; d < dt.Rows.Count; d++)
                    //    {
                    //        sg1_dr = sg1_dt.NewRow();
                    //        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                    //        sg1_dr["sg1_h1"] = dt.Rows[d]["userid"].ToString().Trim();
                    //        sg1_dr["sg1_h2"] = dt.Rows[d]["username"].ToString().Trim();
                    //        sg1_dr["sg1_h3"] = "-";
                    //        sg1_dr["sg1_h4"] = "-";
                    //        sg1_dr["sg1_h5"] = "-";
                    //        sg1_dr["sg1_h6"] = "-";
                    //        sg1_dr["sg1_h7"] = "-";
                    //        sg1_dr["sg1_h8"] = "-";
                    //        sg1_dr["sg1_h9"] = "-";
                    //        sg1_dr["sg1_h10"] = "-";

                    //        sg1_dr["sg1_f1"] = dt.Rows[d]["USERID"].ToString().Trim();
                    //        sg1_dr["sg1_f2"] = dt.Rows[d]["full_Name"].ToString().Trim();
                    //        sg1_dr["sg1_f3"] = dt.Rows[d]["username"].ToString().Trim();
                    //        sg1_dr["sg1_f4"] = dt.Rows[d]["contactno"].ToString().Trim();
                    //        sg1_dr["sg1_f5"] = dt.Rows[d]["emailid"].ToString().Trim();

                    //        sg1_dr["sg1_t1"] = "";
                    //        sg1_dr["sg1_t2"] = "";
                    //        sg1_dr["sg1_t3"] = "";
                    //        sg1_dr["sg1_t4"] = "";
                    //        sg1_dr["sg1_t5"] = "";
                    //        sg1_dr["sg1_t6"] = "";
                    //        sg1_dr["sg1_t7"] = "";
                    //        sg1_dr["sg1_t8"] = "";
                    //        sg1_dr["sg1_t9"] = "";
                    //        sg1_dr["sg1_t10"] = "";
                    //        sg1_dr["sg1_t11"] = "";
                    //        sg1_dr["sg1_t12"] = "";
                    //        sg1_dr["sg1_t13"] = "";
                    //        sg1_dr["sg1_t14"] = "";
                    //        sg1_dr["sg1_t15"] = "";
                    //        sg1_dr["sg1_t16"] = "";

                    //        sg1_dt.Rows.Add(sg1_dr);
                    //    }
                    //}
                    //sg1_add_blankrows();

                    //ViewState["sg1"] = sg1_dt;
                    //sg1.DataSource = sg1_dt;
                    //sg1.DataBind();
                    //dt.Dispose(); sg1_dt.Dispose();
                    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    break;
                case "SG1_ROW_ADD_E":
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

        }


        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {


            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            SQuery = "select a.block,a.assetcode,b.assetname,b.assetid,a.more180 as ast_more_180,a.less_180 a ast_less_180,a.sale_it as sold,a.vchnum, to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.ent_by as Entryby ,  To_char(a.ent_dt,'dd/mm/yyyy') as EnterDate  from WB_FA_VCH a, wb_fa_pur b WHERE a.TYPE='80' AND a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and a.branch||a.acode||a.block=b.branch||b.acode||b.block order by a.vchnum,a.acode";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "Print" || hffield.Value == "Print_E")
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70406");
            fgen.fin_acct_reps(frm_qstr);
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
                    string convdt = (Convert.ToDateTime(txtvchdate.Text)).ToString("dd/MM/yyyy");
                    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(convdt))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!"); txtvchdate.Focus();
                    }
                }
            }

            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!"); txtvchdate.Focus();
            }
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

                        oDS2 = new DataSet();
                        oporow2 = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS3 = new DataSet();
                        oporow3 = null;
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun2();
                        save_fun3();
                        save_fun();



                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS3.Dispose();
                        oporow3 = null;
                        oDS3 = new DataSet();
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);



                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnum.Value.Trim();
                            save_it = "Y";
                        }

                        else
                        {
                            save_it = "N";
                            for (i = 0; i < sg1.Rows.Count - 0; i++)
                            {


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

                        save_fun2();
                        save_fun3();
                        save_fun();


                        if (edmode.Value == "Y")
                        {


                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);


                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS3, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Value + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);


                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "vipin@Tejaxo.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Value + " Saved Successfully ");
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
        //sg1_dt.Columns.Add(new DataColumn("sg1_t11", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t12", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t13", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t14", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t15", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t16", typeof(string)));

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
        //sg1_dr["sg1_t11"] = "-";
        //sg1_dr["sg1_t12"] = "-";
        //sg1_dr["sg1_t13"] = "-";
        //sg1_dr["sg1_t14"] = "-";
        //sg1_dr["sg1_t15"] = "-";
        //sg1_dr["sg1_t16"] = "-";

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
                //sg1.Rows[sg1r].Cells[8].Attributes.Add("readonly", "false");
                sg1.HeaderRow.Cells[20].Width = 140;
                // sg1.HeaderRow.Cells[10].Visible=  false;
                // sg1.HeaderRow.Cells[11].Visible = false;
                //  sg1.HeaderRow.Cells[24].Visible = false;
                //   sg1.HeaderRow.Cells[25].Visible = false;
                //  sg1.HeaderRow.Cells[26].Visible = false;
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

                //if (index < sg1.Rows.Count - 1)
                //{
                //    hf1.Value = index.ToString();
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                //    //----------------------------
                //    hffield.Value = "SG1_ROW_ADD_E";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                //    make_qry_4_popup();
                //    fgen.Fn_open_sseek("Select Item", frm_qstr);
                //}
                //else
                //{
                //    hffield.Value = "SG1_ROW_ADD";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                //    make_qry_4_popup();
                //    fgen.Fn_open_mseek("Select Item", frm_qstr);
                //    //fgen.Fn_open_mseek("Select Item", frm_qstr);
                //}
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

        if (txtvchnum.Value == "-")
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


        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {

            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();
            oporow["SRNO"] = i;
            oporow["block"] = sg1.Rows[i].Cells[13].Text.Trim();
            //oporow["deprdays"] = sg1.Rows[i].Cells[14].Text.Trim();//deprdays for opening
            oporow["less180"] = sg1.Rows[i].Cells[17].Text.Trim();
            oporow["more180"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);// assets b/f
            oporow["sale_it"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text); // current addition
            oporow["deprdays"] = sg1.Rows[i].Cells[15].Text.Trim();
            // oporow["wdv_it"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
            oporow["cramt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text);
            //if (fgen.make_double(sg1.Rows[i].Cells[17].Text.Trim()) + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text) + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text) - fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text) > 0)
            //{
                oporow["dramt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) ;
            //}
            //else
            //{
            //    oporow["dramt"] = "0";
            //}
            oporow["assetval1"] = sg1.Rows[i].Cells[16].Text.Trim();

            oporow["depr"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text);

            if (edmode.Value == "Y")
            {
                oporow["eNt_by"] = ViewState["entby"].ToString();
                oporow["eNt_dt"] = ViewState["entdt"].ToString();
            }
            else
            {
                oporow["eNt_by"] = frm_uname;
                oporow["eNt_dt"] = vardate;
            }

            oDS.Tables[0].Rows.Add(oporow);

        }
    }
    void save_fun2()
    {

        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");


        string passed_date = "", passed_date2 = "";
        frm_vnum1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7");

        // logic for passed_date

        if (fgen.make_double(frm_CDT2.Trim().Substring(0, 6)) % 4 == 0)
        {
            passed_date = "04/10/2018";
            passed_date2 = "05/10/" + frm_CDT1.Trim().Substring(6, 4);
        }

        else
        {
            passed_date = "03/10/" + frm_CDT1.Trim().Substring(6, 4);
            passed_date2 = "04/10/" + frm_CDT1.Trim().Substring(6, 4);
        }

        //SQuery = "SELECT TRIM(A.ACODE) AS ACODE,TRIM(A.block) AS block, SUM(A.LESS180) aS LESS180,SUM(A.MORE180) AS MORE180, SUM(A.SALE) AS SALE FROM (SELECT TRIM(A.ACODE) AS ACODE, TRIM(A.block) AS block, A.ORIGINAL_COST  AS LESS180,0 AS MORE180,0 AS sale FROM WB_FA_PUR A left outer join wb_fa_vch b on A.branchcd||trim(A.acode)= b.branchcd||trim(b.acode) AND B.TYPE='20' AND to_date(B.sale_dt,'dd/mm/yyyy') between TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY') WHERE A.TYPE='10' AND A.INSTDT between TO_dATE('" + passed_date + "','DD/MM/YYYY') AND TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY') UNION ALL SELECT TRIM(A.ACODE) AS ACODE,TRIM(A.block) AS block, 0  AS LESS180,A.ORIGINAL_COST AS MORE180, 0 AS sale FROM WB_FA_PUR A left outer join wb_fa_vch b on A.branchcd||trim(A.acode)= b.branchcd||trim(b.acode) AND B.TYPE='20' AND to_date(B.sale_dt,'dd/mm/yyyy') between TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY') WHERE A.TYPE='10' AND A.INSTDT between TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_dATE('" + passed_date + "','DD/MM/YYYY')UNION ALL SELECT TRIM(ACODE) AS ACODE,TRIM(block) AS block, 0  AS LESS180,0 AS MORE180,CRAMT AS sale FROM WB_FA_VCH WHERE TYPE='20' AND to_date(sale_dt,'dd/mm/yyyy') between TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY') and instdt<TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY'))A GROUP BY TRIM(A.ACODE), TRIM(A.block) ";

        SQuery = "SELECT '" + frm_mbr + "' as branchcd,TRIM(A.ACODE) AS ACODE,a.instdt,TRIM(A.block) AS block, SUM(A.LESS180) aS LESS180,SUM(A.MORE180) AS MORE180, SUM(A.SALE) AS SALE FROM (SELECT TRIM(A.ACODE) AS ACODE, a.instdt,TRIM(A.block) AS block, 0  AS LESS180,A.ORIGINAL_COST AS MORE180, 0 AS sale FROM WB_FA_PUR A where A.branchcd||trim(A.acode) NOT IN (select b.branchcd||trim(b.acode) from wb_fa_vch b where B.TYPE='20' AND to_date(b.sale_dt,'dd/mm/yyyy')>TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY')) and A.TYPE='10' AND A.INSTDT between TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_dATE('" + passed_date + "','DD/MM/YYYY') union all SELECT TRIM(A.ACODE) AS ACODE, a.instdt,TRIM(A.block) AS block, a.original_cost  AS LESS180,0 AS MORE180, 0 AS sale FROM WB_FA_PUR A where A.branchcd||trim(A.acode) NOT IN (select b.branchcd||trim(b.acode) from wb_fa_vch b where B.TYPE='20' AND to_date(b.sale_dt,'dd/mm/yyyy')>TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY')) and A.TYPE='10' AND A.INSTDT between TO_dATE('" + passed_date2 + "','DD/MM/YYYY') AND TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY') UNION ALL SELECT TRIM(ACODE) AS ACODE,instdt,TRIM(block) AS block, 0  AS LESS180,0 AS MORE180,salevalue AS sale FROM WB_FA_VCH WHERE TYPE='20' AND to_date(sale_dt,'dd/mm/yyyy') between TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY') and instdt<TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY'))A GROUP BY TRIM(A.ACODE), TRIM(A.block),a.instdt";
        SQuery = "SELECT '" + frm_mbr + "' as branchcd,TRIM(A.ACODE) AS ACODE,a.instdt,TRIM(A.BLOCK) AS BLOCK,SUM(A.LESS180) aS LESS180,SUM(A.MORE180) AS MORE180, SUM(A.SALE) AS SALE FROM " + vview60 + " A /* WHERE TO_date(a.VCHDATE,'dd/MM/yyyy')=to_date('" + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')*/ GROUP BY TRIM(A.BLOCK),a.branchcd,TRIM(A.ACODE),a.instdt";
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        if (dt.Rows.Count > 0)
        {

            for (i = 0; i < dt.Rows.Count; i++)
            {

                oporow2 = oDS2.Tables[0].NewRow();
                oporow2["BRANCHCD"] = frm_mbr;
                oporow2["TYPE"] = "60";
                oporow2["vchnum"] = frm_vnum1;
                oporow2["vchdate"] = txtvchdate.Text.Trim();
                oporow2["SRNO"] = i;
                oporow2["block"] = dt.Rows[i]["block"].ToString().Trim();
                oporow2["less180"] = dt.Rows[i]["less180"].ToString().Trim();
                oporow2["more180"] = dt.Rows[i]["more180"].ToString().Trim();
                oporow2["sale_it"] = dt.Rows[i]["sale"].ToString().Trim();
                oporow2["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                oporow2["dramt"] = "0";
                oporow2["cramt"] = "0";
                oporow2["iqtyin"] = "0";
                oporow2["iqtyout"] = "0";
                oporow2["instdt"] = dt.Rows[i]["instdt"].ToString().Trim();
                oporow2["sale_dt"] = "-";
                if (edmode.Value == "Y")
                {
                    oporow2["eNt_by"] = ViewState["entby"].ToString();
                    oporow2["eNt_dt"] = ViewState["entdt"].ToString();
                }
                else
                {
                    oporow2["eNt_by"] = frm_uname;
                    oporow2["eNt_dt"] = vardate;
                }
                oDS2.Tables[0].Rows.Add(oporow2);
            }

        }
    }

    void save_fun3()
    {
        frm_vnum2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        string nrows = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as tot from wb_fa_vch where type='80' and  vchdate =to_date('" + frm_CDT1 + "','dd/mm/yyyy')-1", "tot");
        if (fgen.make_int(nrows) > 0)
        //SQuery = "SELECT TRIM(A.BLOCK) AS BLOCK,SUM(A.LESS180) aS LESS180,SUM(A.MORE180) AS MORE180, SUM(A.SALE_IT) AS SALE FROM WB_FA_VCH A WHERE A.TYPE='60' AND TO_CHAR(VCHDATE,'dd/MM/yyyy')='" + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + "' GROUP BY TRIM(A.BLOCK)";
        SQuery = "SELECT '" + frm_mbr + "' as branchcd,TRIM(A.BLOCK) AS BLOCK,SUM(A.LESS180) aS LESS180,SUM(A.MORE180) AS MORE180, SUM(A.SALE) AS SALE FROM " + vview60 + " A /* WHERE TO_date(a.VCHDATE,'dd/MM/yyyy')=to_date('" + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')*/ GROUP BY TRIM(A.BLOCK),a.branchcd";
        if (fgen.make_int(nrows) > 0)
        {
            SQuery = "SELECT TRIM(C.BLOCK) AS BLOCK, TRIM(B.NAME) as BLOCKNAME,(NVL(B.NUM4,0))+ (nvl(b.num5,0)) AS PERCENTAGE, SUM(C.OPENING) as DRAMT,SUM(C.LESS180) AS LESS180 ,SUM(C.MORE180) AS MORE180,sum(C.sale) AS SALE FROM(SELECT TRIM(a.BLOCK) AS BLOCK, SUM(a.OPENING) aS OPENING,SUM(a.LESS180) aS LESS180,SUM(a.MORE180) AS MORE180,sum(a.sale) as sale  FROM (SELECT TRIM(block) AS BLOCK, nvl(dramt,0) AS opening,0  AS LESS180,0 AS MORE180, 0 as sale FROM WB_FA_vch  WHERE branchcd='00' and type='80' and vchdate=TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY')-1 UNION ALL SELECT  TRIM(a.block) AS BLOCK, 0 AS OPENING,nvl(a.less180,0) AS LESS180,nvl(a.more180,0) AS MORE180, nvl(a.sale,0) as sale FROM " + vview70 + " a WHERE a.branchcd='" + frm_mbr + "')a  group by TRIM(A.BLOCK)) C, TYPEGRP B WHERE TRIM(C.BLOCK)= TRIM(B.TYPE1) AND B.ID='DI' group by TRIM(C.BLOCK), TRIM(B.NAME),(NVL(B.NUM4,0))+ (nvl(b.num5,0)) order by TRIM(B.NAME)";
        }
        else
        {
            SQuery = "SELECT TRIM(C.BLOCK) AS BLOCK, TRIM(B.NAME) as BLOCKNAME,(NVL(B.NUM4,0))+ (nvl(b.num5,0)) AS PERCENTAGE, SUM(C.OPENING) as DRAMT,SUM(C.LESS180) AS LESS180 ,SUM(C.MORE180) AS MORE180,sum(C.sale) AS SALE FROM( SELECT TRIM(a.BLOCK) AS BLOCK, SUM(a.OPENING) aS opening,SUM(a.LESS180) aS LESS180,SUM(a.MORE180) AS MORE180,sum(a.sale) as sale  FROM ( SELECT TRIM(type1) AS BLOCK,num6 AS OPENING,0  AS LESS180,0 AS MORE180,0 as sale  FROM typegrp WHERE id='ZX' UNION ALL SELECT  TRIM(a.block) AS BLOCK, 0 AS OPENING,nvl(a.less180,0) AS LESS180,nvl(a.more180,0) AS MORE180, nvl(a.sale,0) as sale FROM " + vview70 + " a where branchcd='" + frm_mbr + "')a  group by TRIM(A.BLOCK)) C, TYPEGRP B WHERE TRIM(C.BLOCK)= TRIM(B.TYPE1) AND B.ID='DI' group by TRIM(C.BLOCK), TRIM(B.NAME),(NVL(B.NUM4,0))+ (nvl(b.num5,0)) order by TRIM(B.NAME) ";
        }

        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        if (dt.Rows.Count > 0)
        {

            for (i = 0; i < dt.Rows.Count; i++)
            {

                oporow3 = oDS3.Tables[0].NewRow();
                oporow3["BRANCHCD"] = frm_mbr;
                oporow3["TYPE"] = "70";
                oporow3["vchnum"] = frm_vnum1;
                oporow3["vchdate"] = txtvchdate.Text.Trim();
                oporow3["SRNO"] = i;
                oporow3["block"] = dt.Rows[i]["block"].ToString().Trim();
                oporow3["less180"] = dt.Rows[i]["less180"].ToString().Trim();
                oporow3["more180"] = dt.Rows[i]["more180"].ToString().Trim();
                oporow3["sale_it"] = dt.Rows[i]["sale"].ToString().Trim();
                oporow2["dramt"] = "0";
                oporow2["cramt"] = "0";
                oporow2["iqtyin"] = "0";
                oporow2["iqtyout"] = "0";
                oporow2["sale_dt"] = "-";

                if (edmode.Value == "Y")
                {
                    oporow3["eNt_by"] = ViewState["entby"].ToString();
                    oporow3["eNt_dt"] = ViewState["entdt"].ToString();
                    oporow2["instdt"] = ViewState["entdt"].ToString();
                }
                else
                {
                    oporow3["eNt_by"] = frm_uname;
                    oporow3["eNt_dt"] = vardate;
                    oporow2["instdt"] = vardate;
                }
                oDS3.Tables[0].Rows.Add(oporow3);
            }
        }
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
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
                break;

        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }



    protected void txt_TextChanged(object sender, EventArgs e)
    {
        //fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
        // made logic to get working hours and working minutes
        //string dttoh = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text;
        //string dttom = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text;
        //string dtfromh = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text;
        //string dtfromm = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text;


        //DateTime dtFrom = DateTime.Parse(dtfromh + ":" + dtfromm);
        //DateTime dtTo = DateTime.Parse(dttoh + ":" + dttom);

        //int timeDiff = dtFrom.Subtract(dtTo).Hours;
        //int timediff2 = dtFrom.Subtract(dtTo).Minutes;


        //TextBox txtName = ((TextBox)sg1.Rows[i].FindControl("sg1_t5"));
        //txtName.Text = timeDiff.ToString();

        //TextBox txtName1 = ((TextBox)sg1.Rows[i].FindControl("sg1_t6"));
        //txtName1.Text = timediff2.ToString();



    }
    //------------------------------------------------------------------------------------   
    protected void btncal_Click(object sender, EventArgs e)
    {

        DataTable dt_vch = new DataTable();
        DataTable dt_op = new DataTable();
        DataTable dt_op70 = new DataTable();

        if (txtvchnum.Value.Length < 2)
        {
            fgen.msg("-", "AMSG", "Please press the new button");
            txtvchdate.Focus(); return;
        }


        string lastdepdt = "";


        int month = Convert.ToInt32(txtvchdate.Text.Substring(3, 2));


        if (month >= 04)
        {
            if (Convert.ToInt32(txtvchdate.Text.Substring(6, 4)) > Convert.ToInt32(frm_myear))
            {


                fgen.msg("-", "AMSG", "Please Select a Valid Date within the financial year logged -In");
                txtvchdate.Focus(); return;


            }
        }
        else
        {
            if (Convert.ToInt32(txtvchdate.Text.Substring(6, 4)) < Convert.ToInt32(frm_myear))
            {
                fgen.msg("-", "AMSG", "Please Select a Valid Date within the financial year logged -In"); txtvchdate.Focus(); return;

            }

        }
        if (txtlastdt.Text == "")
        {

            fgen.msg("-", "AMSG", "Please enter a Valid last  Depreciation Date or 31st March of Last financial year"); txtlastdt.Focus();

            return;


        }


        //checking the previous year depreciation calculation is valid or not

        SQuery = "select distinct  SUBSTR(to_char(vchdate,'dd/MM/yyyy'),7,10) as vchdate from wb_fa_vch where type='80' and branchcd='" + frm_mbr + "' order by vchdate desc";
        SQuery = "select distinct to_char(vchdate,'dd/MM/yyyy') as vchdate from wb_fa_vch where type='80' and branchcd='" + frm_mbr + "' order by vchdate desc";

        dt_vch = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        string curr = frm_CDT2.Trim().Substring(6, 4);
        int pr_date = Convert.ToInt32(curr) - 1;


        if (dt_vch.Rows.Count > 0)
        {

            for (int vch = 0; vch < dt_vch.Rows.Count; vch++)
            {
                if (Convert.ToString(Convert.ToDateTime(txtvchdate.Text).ToString("yyyy")) == Convert.ToString(pr_date))
                {
                    break;
                }
                else
                {
                    fgen.msg("", "AMSG", "You have Logged-In in wrong financial year.Depreciation already calculated for this year.");
                    return;
                }

            }

            int dhd1 = fgen.ChkDate(txtlastdt.Text);
            int dhd2 = fgen.ChkDate(txtvchdate.Text);
            if ((dhd1 == 0) || (dhd2 == 0))
            {
                fgen.msg("-", "AMSG", "Invalid date format");
                txtlastdt.Focus();

                return;
            }
        }

        string chklastdate = "";
        chklastdate = fgen.seek_iname(frm_qstr, frm_cocd, "select to_date('" + frm_CDT1 + "','dd/MM/yyyy')-1  as dd from dual", "dd");

        if (Convert.ToDateTime(txtlastdt.Text.Trim()) < Convert.ToDateTime(chklastdate))
        {
            fgen.msg("-", "AMSG", "Please Select a Valid last  Depreciation Date. Depreciation can't be calculated for more than a year");
            txtlastdt.Focus();
            return;
        }


        // Change code 
        string passed_date = ""; string passed_date2 = "";
        frm_vnum1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7");

        // logic for passed_date

        if (fgen.make_double(frm_CDT2.Trim().Substring(0, 6)) % 4 == 0)
        {
            passed_date = "04/10/" + frm_CDT1.Trim().Substring(6, 4);
            passed_date2 = "05/10/" + frm_CDT1.Trim().Substring(6, 4);
        }

        else
        {
            passed_date = "03/10/" + frm_CDT1.Trim().Substring(6, 4);
            passed_date2 = "04/10/" + frm_CDT1.Trim().Substring(6, 4);
        }

        //save in type 60
        string vchdate = "";
        if (txtvchdate.Text.Trim().Length > 1)
        {
            vchdate = Convert.ToDateTime(txtvchdate.Text).ToString("dd/MM/yyyy");
        }
        //else
        //{
        //    vchdate = "-";          
        //}
        //SQuery = "SELECT '" + frm_mbr + "' as branchcd,TRIM(A.ACODE) AS ACODE,a.instdt,TRIM(A.block) AS block, SUM(A.LESS180) aS LESS180,SUM(A.MORE180) AS MORE180, SUM(A.SALE) AS SALE FROM (SELECT TRIM(A.ACODE) AS ACODE, a.instdt,TRIM(A.block) AS block, 0  AS LESS180,A.ORIGINAL_COST AS MORE180, 0 AS sale FROM WB_FA_PUR A, wb_fa_vch b where A.branchcd||trim(A.acode)= b.branchcd||trim(b.acode) AND B.TYPE='20' AND to_date(b.sale_dt,'dd/mm/yyyy')>TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY') and A.TYPE='10' AND A.INSTDT between TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_dATE('" + passed_date + "','DD/MM/YYYY') union all SELECT TRIM(A.ACODE) AS ACODE, a.instdt,TRIM(A.block) AS block, 0  AS LESS180,A.ORIGINAL_COST AS MORE180, 0 AS sale FROM WB_FA_PUR A, wb_fa_vch b where A.branchcd||trim(A.acode)= b.branchcd||trim(b.acode) AND B.TYPE='20' AND to_date(b.sale_dt,'dd/mm/yyyy')>TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY') and A.TYPE='10' AND A.INSTDT between TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_dATE('" + passed_date + "','DD/MM/YYYY') UNION ALL SELECT TRIM(ACODE) AS ACODE,instdt,TRIM(block) AS block, 0  AS LESS180,0 AS MORE180,salevalue AS sale FROM WB_FA_VCH WHERE TYPE='20' AND to_date(sale_dt,'dd/mm/yyyy') between TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY') and instdt<TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY'))A GROUP BY TRIM(A.ACODE), TRIM(A.block),a.instdt";
        SQuery = "SELECT '" + frm_mbr + "' as branchcd,TRIM(A.ACODE) AS ACODE,a.instdt,TRIM(A.block) AS block, SUM(A.LESS180) aS LESS180,SUM(A.MORE180) AS MORE180, SUM(A.SALE) AS SALE FROM (SELECT TRIM(A.ACODE) AS ACODE, a.instdt,TRIM(A.block) AS block, 0  AS LESS180,A.ORIGINAL_COST AS MORE180, 0 AS sale FROM WB_FA_PUR A where A.branchcd||trim(A.acode) NOT IN (select b.branchcd||trim(b.acode) from wb_fa_vch b where B.TYPE='20' AND to_date(b.sale_dt,'dd/mm/yyyy')>TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY')) and A.TYPE='10' AND A.INSTDT between TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_dATE('" + passed_date + "','DD/MM/YYYY') union all SELECT TRIM(A.ACODE) AS ACODE, a.instdt,TRIM(A.block) AS block, a.original_cost  AS LESS180,0 AS MORE180, 0 AS sale FROM WB_FA_PUR A where A.branchcd||trim(A.acode) NOT IN (select b.branchcd||trim(b.acode) from wb_fa_vch b where B.TYPE='20' AND to_date(b.sale_dt,'dd/mm/yyyy')>TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY')) and A.TYPE='10' AND A.INSTDT between TO_dATE('" + passed_date2 + "','DD/MM/YYYY') AND TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY') UNION ALL SELECT TRIM(ACODE) AS ACODE,instdt,TRIM(block) AS block, 0  AS LESS180,0 AS MORE180,salevalue AS sale FROM WB_FA_VCH WHERE TYPE='20' AND to_date(sale_dt,'dd/mm/yyyy') between TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY') and instdt<TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY'))A GROUP BY TRIM(A.ACODE), TRIM(A.block),a.instdt";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view " + vview60 + " as(SELECT * FROM (" + SQuery + "))");

        //  SQuery = "SELECT TRIM(A.BLOCK) AS BLOCK,a.instdt,SUM(A.LESS180) aS LESS180,SUM(A.MORE180) AS MORE180, SUM(A.SALE_IT) AS SALE FROM WB_FA_VCH A WHERE A.TYPE='60' AND TO_CHAR(VCHDATE,'dd/MM/yyyy')='" + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + "' GROUP BY TRIM(A.BLOCK),a.instdt";
        SQuery = "SELECT '" + frm_mbr + "' as branchcd,TRIM(A.BLOCK) AS BLOCK,SUM(A.LESS180) aS LESS180,SUM(A.MORE180) AS MORE180, SUM(A.SALE) AS SALE FROM " + vview60 + " A /* WHERE TO_date(a.VCHDATE,'dd/MM/yyyy')=to_date('" + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + "','dd/mm/yyyy')*/ GROUP BY TRIM(A.BLOCK),a.branchcd";

        dt3 = new DataTable();
        dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view " + vview70 + " as(SELECT * FROM (" + SQuery + "))");

        if (Convert.ToDateTime(txtlastdt.Text) > Convert.ToDateTime(txtvchdate.Text))
        {

            fgen.msg("-", "AMSG", "Your Last Depreciation date cannot be greater than  current one.Last Dep. Date entered/database contains '" + Convert.ToDateTime(lastdepdt).ToString("dd/MM/yyyy") + "'"); txtlastdt.Focus();

            return;
        }

        //MASTER QUERY
        //SQuery = "SELECT TRIM(C.BLOCK) AS BLOCK, TRIM(B.NAME) as BLOCKNAME,NVL(B.NUM4,0) AS PERCENTAGE, SUM(C.OPENING+C.DRAMT) as DRAMT,SUM(C.LESS180) AS LESS180 ,SUM(C.MORE180) AS MORE180,sum(C.sale) AS SALE FROM(SELECT TRIM(a.BLOCK) AS BLOCK, SUM(a.OPENING) aS OPENING,SUM(a.DRAMT) as DRAMT,SUM(a.LESS180) aS LESS180,SUM(a.MORE180) AS MORE180,sum(a.sale) as sale  FROM (SELECT TRIM(type1) AS BLOCK,num6 AS OPENING,0 AS DRAMT,0  AS LESS180,0 AS MORE180,0 as sale  FROM typegrp WHERE id='DI' UNION ALL SELECT  TRIM(block) AS BLOCK, 0 AS OPENING,(sum(nvl(less180,0))+sum(nvl(more180,0))- sum(nvl(sale,0))) AS DRAMT,0  AS LESS180,0 AS MORE180, 0 as sale FROM vview6000 WHERE  instdt<TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY') group by TRIM(block) UNION ALL SELECT  TRIM(block) AS BLOCK, 0 AS OPENING,0 AS DRAMT,sum(nvl(less180,0)) AS LESS180,sum(nvl(more180,0)) AS MORE180, sum(nvl(sale,0)) as sale FROM vview7000 WHERE  instdt between TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_dATE('" + frm_CDT2 + "','DD/MM/YYYY') group by TRIM(block) )a  group by TRIM(A.BLOCK)) C, TYPEGRP B WHERE TRIM(C.BLOCK)= TRIM(B.TYPE1) AND B.ID='DI' group by TRIM(C.BLOCK), TRIM(B.NAME),B.NUM4 group by TRIM(C.BLOCK)  ";
        string nrows = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as tot from wb_fa_vch where type='80' and  vchdate =to_date('" + frm_CDT1 + "','dd/mm/yyyy')-1", "tot");
        if (fgen.make_int(nrows) > 0)
        {
            SQuery = "SELECT TRIM(C.BLOCK) AS BLOCK, TRIM(B.NAME) as BLOCKNAME,(NVL(B.NUM4,0))+ (nvl(b.num5,0)) AS PERCENTAGE, SUM(C.OPENING) as DRAMT,SUM(C.LESS180) AS LESS180 ,SUM(C.MORE180) AS MORE180,sum(C.sale) AS SALE FROM(SELECT TRIM(a.BLOCK) AS BLOCK, SUM(a.OPENING) aS OPENING,SUM(a.LESS180) aS LESS180,SUM(a.MORE180) AS MORE180,sum(a.sale) as sale  FROM (SELECT TRIM(block) AS BLOCK, (case when (nvl(dramt,0) - nvl(cramt,0)) <0 then 0 else (nvl(dramt,0) - nvl(cramt,0)) end )   AS opening,0  AS LESS180,0 AS MORE180, 0 as sale FROM WB_FA_vch  WHERE branchcd='00' and type='80' and vchdate=TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY')-1 UNION ALL SELECT  TRIM(a.block) AS BLOCK, 0 AS OPENING,nvl(a.less180,0) AS LESS180,nvl(a.more180,0) AS MORE180, nvl(a.sale,0) as sale FROM " + vview70 + " a WHERE a.branchcd='" + frm_mbr + "')a  group by TRIM(A.BLOCK)) C, TYPEGRP B WHERE TRIM(C.BLOCK)= TRIM(B.TYPE1) AND B.ID='DI' group by TRIM(C.BLOCK), TRIM(B.NAME),(NVL(B.NUM4,0))+ (nvl(b.num5,0)) order by TRIM(B.NAME)";
        }
        else
        {
            SQuery = "SELECT TRIM(C.BLOCK) AS BLOCK, TRIM(B.NAME) as BLOCKNAME,(NVL(B.NUM4,0))+ (nvl(b.num5,0)) AS PERCENTAGE, SUM(C.OPENING) as DRAMT,SUM(C.LESS180) AS LESS180 ,SUM(C.MORE180) AS MORE180,sum(C.sale) AS SALE FROM( SELECT TRIM(a.BLOCK) AS BLOCK, SUM(a.OPENING) aS opening,SUM(a.LESS180) aS LESS180,SUM(a.MORE180) AS MORE180,sum(a.sale) as sale  FROM ( SELECT TRIM(type1) AS BLOCK,num6 AS OPENING,0  AS LESS180,0 AS MORE180,0 as sale  FROM typegrp WHERE id='ZX' UNION ALL SELECT  TRIM(a.block) AS BLOCK, 0 AS OPENING,nvl(a.less180,0) AS LESS180,nvl(a.more180,0) AS MORE180, nvl(a.sale,0) as sale FROM " + vview70 + " a where branchcd='" + frm_mbr + "')a  group by TRIM(A.BLOCK)) C, TYPEGRP B WHERE TRIM(C.BLOCK)= TRIM(B.TYPE1) AND B.ID='DI' group by TRIM(C.BLOCK), TRIM(B.NAME),(NVL(B.NUM4,0))+ (nvl(b.num5,0)) order by TRIM(B.NAME) ";
        }

        lastdepdt = Convert.ToDateTime(txtlastdt.Text.Trim()).ToString("dd/MM/yyyy");
        dt4 = new DataTable();
        dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //fetching data from wb_fa_pur and wb_fa_vch

        if (dt4.Rows.Count < 1)
            {
                fgen.msg("-", "AMSG", " No Data found.");
                return;
            }
        if (dt4.Rows.Count > 0)
        {
            create_tab();
            sg1_dr = null;
            for (i = 0; i < dt4.Rows.Count; i++)
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

                sg1_dr["sg1_f1"] = dt4.Rows[i]["block"].ToString().Trim();
                sg1_dr["sg1_f2"] = dt4.Rows[i]["blockname"].ToString().Trim();
                sg1_dr["sg1_f3"] = dt4.Rows[i]["PERCENTAGE"].ToString().Trim();

                string v1 = "", v2 = "", v3 = "", v4 = "";
                if (dt4.Rows.Count > 0)
                    {
                        v1 = dt4.Rows[i]["dramt"].ToString().Trim();
                        v2 = dt4.Rows[i]["less180"].ToString().Trim();
                        v3 = dt4.Rows[i]["more180"].ToString().Trim();
                        v4 = dt4.Rows[i]["sale"].ToString().Trim();
                    }
                // sg1_dr["sg1_f4"] = Math.Round(fgen.make_double(v1) + fgen.make_double(v2) + fgen.make_double(v3) - fgen.make_double(v4), 2);
                sg1_dr["sg1_f4"] = dt4.Rows[i]["dramt"].ToString().Trim();
                sg1_dr["sg1_f5"] = dt4.Rows[i]["LESS180"].ToString().Trim();
                sg1_dr["sg1_t1"] = dt4.Rows[i]["MORE180"].ToString().Trim();
                sg1_dr["sg1_t2"] = dt4.Rows[i]["SALE"].ToString().Trim();

                double bal = fgen.make_double(dt4.Rows[i]["DRAMT"].ToString().Trim()) + fgen.make_double(dt4.Rows[i]["LESS180"].ToString().Trim()) + fgen.make_double(dt4.Rows[i]["MORE180"].ToString().Trim()) - fgen.make_double(dt4.Rows[i]["SALE"].ToString().Trim());
                if (bal < 0)
                {
                    sg1_dr["sg1_t3"] = 0;
                }
                else
                {
                    sg1_dr["sg1_t3"] = bal;
                }
                double depr = 0;
                //calculate depreciation value
                depr = fgen.make_double(dt4.Rows[i]["LESS180"].ToString().Trim()) * fgen.make_double(dt4.Rows[i]["PERCENTAGE"].ToString().Trim()) * 0.5 / 100;
                depr = depr + fgen.make_double(dt4.Rows[i]["MORE180"].ToString().Trim()) * (fgen.make_double(dt4.Rows[i]["PERCENTAGE"].ToString().Trim())) / 100;
                depr = depr + fgen.make_double(dt4.Rows[i]["DRAMT"].ToString().Trim()) * (fgen.make_double(dt4.Rows[i]["PERCENTAGE"].ToString().Trim()) / 100);
                depr = fgen.make_double(depr - fgen.make_double(dt4.Rows[i]["sale"].ToString().Trim()),2);

                if (depr <= 0)
                {
                    sg1_dr["sg1_t4"] = 0;
                }
                else
                {
                    sg1_dr["sg1_t4"] = depr;
                }

                sg1_dt.Rows.Add(sg1_dr);

            }
        }

        ViewState["sg1"] = sg1_dt;
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        dt.Dispose(); sg1_dt.Dispose();

        double d = 0, d1 = 0, d2 = 0, d3 = 0, d4 =0, d5 = 0;
        if (sg1.Rows.Count > 1)
        {
            for (int i1 = 0; i1 < sg1.Rows.Count; i1++)
            {
                d += fgen.make_double(sg1.Rows[i1].Cells[16].Text.Trim());
                d1 += fgen.make_double(sg1.Rows[i1].Cells[17].Text.Trim());
                d2 += fgen.make_double(((TextBox)sg1.Rows[i1].FindControl("sg1_t1")).Text.Trim());
                d3 += fgen.make_double(((TextBox)sg1.Rows[i1].FindControl("sg1_t2")).Text.Trim());
                d4 += fgen.make_double(((TextBox)sg1.Rows[i1].FindControl("sg1_t3")).Text.Trim());
                d5 += fgen.make_double(((TextBox)sg1.Rows[i1].FindControl("sg1_t4")).Text.Trim());
            }
            txtlbl4.Value = (Math.Round(d, 2)).ToString();
            txtlbl7.Value = (Math.Round(d1, 2)).ToString();
            Text3.Value = (Math.Round(d2, 2)).ToString();
            Text1.Value = (Math.Round(d3, 2)).ToString();
            Text2.Value = (Math.Round(d4, 2)).ToString();
            txtlbl101.Value = (Math.Round(d5, 2)).ToString();
        }
        btnsave.Disabled = false;
    }
}





