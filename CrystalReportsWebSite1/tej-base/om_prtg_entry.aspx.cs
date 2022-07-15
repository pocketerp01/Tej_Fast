using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_prtg_entry : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y"; double checkOkQty = 0, checkSeq = 0, checkstartt = 0, checkendt = 0;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    string stage = "0"; string stagename = "";
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
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
            }
            setColHeadings();
            set_Val();
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
                    sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                }
            }
        }
        tab5.Visible = false;
        tab4.Visible = false;
        tab3.Visible = false;
        tab2.Visible = true;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        //btnprint.Visible = true;
        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false; btnprint.Disabled = false;
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
        fetch_col_rejection();
        fetch_col_downtime();
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
        btnlbl4.Enabled = true;
        btnlbl7.Enabled = true;
        btnlist.Disabled = true;
        btnprint.Disabled = true;
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
        frm_tabname = "prod_sheet";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "86");

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
                SQuery = "select  type1 as fstr,NAME,place,type1 from type where id='D' and substr(type1,1,1)='1' order by name";
                break;
            case "TICODE":
                SQuery = "select trim(acode)||'/'||srno as fstr,mchname as Machine_Name,trim(acode)||'/'||srno as Machine_Code,mch_seq from pmaint where branchcd='" + frm_mbr + "' and type='10' order by acode,srno";
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                SQuery = "select a.type1 as fstr,A.NAME,A.type1,B.CNT AS ITEMS from type A,(select DISTINCT stagec,count(icode) AS CNT from itwstage  GROUP BY STAGEC) B where A.id='K' AND A.TYPE1=B.STAGEC order by A.TYPE1";
                break;

            case "SG1_ROW_ADD1":
            case "SG1_ROW_ADD_E1":
                stage = "0";
                stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text;
                col1 = ""; col2 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[17].Text.Trim() + gr.Cells[16].Text.Trim() + ((TextBox)(gr.FindControl("sg1_t21"))).Text.Trim() + "'";
                    else col1 = "'" + gr.Cells[17].Text.Trim() + gr.Cells[16].Text.Trim() + ((TextBox)(gr.FindControl("sg1_t21"))).Text.Trim() + "'";
                }
                if (col1.Length > 0)
                {
                    col2 = " and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) not in (" + col1 + ")";
                }
                //ORIGINAL  SQuery = "select distinct  trim(a.Icode)||'.'||trim(a.vchnum) as fstr, '['||trim(a.COL16)||' Clr]'||trim(b.Iname) as Item_Name,trim(a.Icode)||'.'||trim(a.vchnum) as Item_Code,b.Cpartno as Part_No,d.aname as Customer,a.ENQDT as Delv_Dt,a.vchnum as Job_No,a.col18||'X'||a.col19 as Cut_Size from costestimate a, item b,itwstage c,famst d where trim(nvl(a.app_by,'-'))!='-' and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(d.acode) and a.type='30' and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.status='N' and c.stagec='" + stage + "' order by trim(a.Icode)||'.'||trim(a.vchnum)";
                SQuery = "select distinct  trim(a.Icode)||'.'||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, '['||trim(a.COL16)||' Clr]'||trim(b.Iname) as Item_Name,trim(a.Icode)||'.'||trim(a.vchnum) as Item_Code,b.Cpartno as Part_No,d.aname as Customer,to_char(a.ENQDT,'dd/mm/yyyy') as Delv_Dt,a.vchnum as Job_No,a.col18||'X'||a.col19 as Cut_Size from costestimate a, item b,itwstage c,famst d where trim(nvl(a.app_by,'-'))!='-' and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(d.acode) and a.type='30' and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.status='N' and c.stagec='" + stage + "' " + col2 + " order by trim(a.Icode)||'.'||trim(a.vchnum)";
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

            case "WorkOrder":
                SQuery = "SELECT TYPE1 AS FSTR,NAME,TYPE1 FROM TYPE  WHERE ID='K' ORDER BY TYPE1";
                break;

            case "Print_E":
                SQuery = "select distinct trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,b.name,a.ename as Machine,c.iname,a.job_no,A.JOB_dT,a.ent_by,a.prevcode from prod_sheet a ,(select NAME,type1 from type where id='K' order by TYPE1 ) b,item c where a.stage=b.type1 and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' AND a.type='" + frm_vty + "' and a.VCHDATE  " + DateRange + "  and a.vchnum<>'000000' order by a.vchnum desc";
                break;
            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:

                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(a.vchnum)||trim(to_char(a.vchdate,'dd/mm/yyyy')) as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,b.name,a.ename as Machine,c.iname,a.job_no,A.JOB_dT,a.ent_by,a.prevcode from prod_sheet a ,(select NAME,type1 from type where id='K' order by TYPE1 ) b,item c where a.stage=b.type1 and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' AND a.type='" + frm_vty + "' and a.VCHDATE  " + DateRange + "  and a.vchnum<>'000000' order by a.vchnum desc";
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



            frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
            lbl1a.Text = frm_vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);

            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "'  AND VCHDATE " + DateRange + " AND TYPE='" + frm_vty + "' ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl2.Text = frm_uname;
        txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl5.Text = "-";
        txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
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
        fetch_col_rejection();
        fetch_col_downtime();
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " To Edit", frm_qstr);
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

        if (txtlbl4.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill " + lbl4.Text);
            return;
        }

        if (txtlbl7.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill " + lbl7.Text);
            return;
        }
        if (sg1.Rows.Count <= 1)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Select Stage");
            return;
        }

        for (int i = 0; i < sg1.Rows.Count - 1; i++)
        {
            checkOkQty = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t7"))).Text.Trim());
            checkSeq = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t37"))).Text.Trim());

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() == "-" || ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() == "" || ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim() == "-" || ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim() == "")
            {
                fgen.msg("-", "AMSG", "Please Fill Start Time and End Time!!");
                return;
            }

            if (checkOkQty == 0)
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill Ok Qty At Line No. " + sg1.Rows[i].Cells[12].Text + "");
                return;
            }
            else if (checkSeq == 0)
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill Sequence(last column) At Line No. " + sg1.Rows[i].Cells[12].Text + "");
                return;
            }
        }
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " To Delete", frm_qstr);
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
        fetch_col_rejection();
        fetch_col_downtime();
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
        //frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
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
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt,i.iname,i.cpartno from " + frm_tabname + " a,item i where trim(a.icode)=trim(i.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
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
                        txtlbl4.Text = dt.Rows[i]["SHFTCODE"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["prevcode"].ToString().Trim();
                        txtlbl5.Text = dt.Rows[i]["edt_by"].ToString().Trim();
                        txtlbl6.Text = Convert.ToDateTime(dt.Rows[i]["edt_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl7.Text = dt.Rows[i]["mchcode"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[i]["ename"].ToString().Trim();
                        txtlbl101.Text = dt.Rows[i]["subcode"].ToString().Trim();
                        doc_addl.Value = dt.Rows[0]["srno"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["naration"].ToString().Trim();
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

                            sg1_dr["sg1_f1"] = dt.Rows[i]["stage"].ToString().Trim();
                            sg1_dr["sg1_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='K' and type1='" + dt.Rows[i]["stage"].ToString().Trim() + "'", "name");
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = dt.Rows[i]["job_dt"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["job_no"].ToString().Trim();
                            // sg1_dr["sg1_f6"] = fgen.seek_iname(frm_qstr, frm_cocd, "select CPARTNO from ITEM where  ICODE='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "CPARTNO");
                            sg1_dr["sg1_f6"] = dt.Rows[i]["cpartno"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["mcstart"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["mcstop"].ToString().Trim();
                            sg1_dr["sg1_t21"] = dt.Rows[i]["icode"].ToString().Trim();
                            // sg1_dr["sg1_t3"] = fgen.seek_iname(frm_qstr, frm_cocd, "select iname from item  where icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "iname");
                            sg1_dr["sg1_t3"] = dt.Rows[i]["iname"].ToString().Trim();

                            sg1_dr["sg1_t4"] = fgen.seek_iname(frm_qstr, frm_cocd, "select rate from type  where id='K' and type1='" + dt.Rows[i]["stage"].ToString().Trim() + "'", "rate");
                            sg1_dr["sg1_t5"] = fgen.seek_iname(frm_qstr, frm_cocd, "select excrate from type  where id='K' and type1='" + dt.Rows[i]["stage"].ToString().Trim() + "'", "excrate");
                            sg1_dr["sg1_t6"] = dt.Rows[i]["A1"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["a2"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["a4"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["a5"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["a6"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["num1"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["num2"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["num3"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["num4"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["num5"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["num6"].ToString().Trim();
                            sg1_dr["sg1_t17"] = dt.Rows[i]["num7"].ToString().Trim();
                            sg1_dr["sg1_t18"] = dt.Rows[i]["num8"].ToString().Trim();
                            sg1_dr["sg1_t19"] = dt.Rows[i]["num9"].ToString().Trim();
                            sg1_dr["sg1_t20"] = dt.Rows[i]["num10"].ToString().Trim();

                            sg1_dr["sg1_t26"] = dt.Rows[i]["a11"].ToString().Trim();
                            sg1_dr["sg1_t27"] = dt.Rows[i]["a12"].ToString().Trim();
                            sg1_dr["sg1_t28"] = dt.Rows[i]["a13"].ToString().Trim();
                            sg1_dr["sg1_t29"] = dt.Rows[i]["a14"].ToString().Trim();
                            sg1_dr["sg1_t30"] = dt.Rows[i]["a15"].ToString().Trim();
                            sg1_dr["sg1_t31"] = dt.Rows[i]["a16"].ToString().Trim();
                            sg1_dr["sg1_t32"] = dt.Rows[i]["a17"].ToString().Trim();
                            sg1_dr["sg1_t33"] = dt.Rows[i]["a18"].ToString().Trim();
                            sg1_dr["sg1_t34"] = dt.Rows[i]["a19"].ToString().Trim();
                            sg1_dr["sg1_t35"] = dt.Rows[i]["a20"].ToString().Trim();

                            sg1_dr["sg1_t22"] = dt.Rows[i]["a7"].ToString().Trim();
                            sg1_dr["sg1_t23"] = dt.Rows[i]["a8"].ToString().Trim();
                            sg1_dr["sg1_t24"] = dt.Rows[i]["remarks"].ToString().Trim();
                            sg1_dr["sg1_t25"] = dt.Rows[i]["remarks2"].ToString().Trim();
                            sg1_dr["sg1_t36"] = dt.Rows[i]["glue_code"].ToString().Trim();
                            sg1_dr["sg1_t37"] = dt.Rows[i]["noups"].ToString().Trim();
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
                        fetch_col_rejection();
                        fetch_col_downtime();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40111");
                    fgen.fin_prodpp_reps(frm_qstr);
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

                case "MRESULT":
                    if (col1.Length <= 0) return;
                    txtlbl101.Text = col1;
                    txtlbl101a.Text = col2;
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    txtlbl101.Text = col3;
                    btnlbl7.Focus();
                    break;

                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
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
                            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.ToString();
                            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.ToString();
                            sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.ToString();
                            sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.ToString();
                            sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.ToString();
                            sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.ToString();
                            sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.ToString();
                            sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.ToString();
                            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.ToString();
                            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.ToString();
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.ToString();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.ToString().Replace("&amp;", "&");
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.ToString();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.ToString();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.ToString();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.ToString();
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
                            sg1_dr["sg1_t36"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t36")).Text.Trim();
                            sg1_dr["sg1_t37"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t37")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        SQuery = "select  a.type1 as fstr,A.NAME,A.type1,B.CNT AS ITEMS from type A,(select DISTINCT stagec,count(icode) AS CNT from itwstage  GROUP BY STAGEC) B where A.id='K' AND A.TYPE1=B.STAGEC and a.type1 in('" + col1 + "') order by A.TYPE1";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";
                            sg1_dr["sg1_f1"] = dt.Rows[d]["Type1"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["Name"].ToString().Trim();
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_f6"] = "-";
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
                            sg1_dr["sg1_t36"] = "";
                            sg1_dr["sg1_t37"] = "";
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
                    fetch_col_rejection();
                    fetch_col_downtime();
                    break;

                case "SG1_ROW_ADD_E":
                    dt = new DataTable();
                    //if (col1.Length > 6) SQuery = "select * from evas where trim(userid) in (" + col1 + ")";
                    //else SQuery = "select * from evas where trim(userid)='" + col1 + "'";
                    SQuery = "select  a.type1 as fstr,A.NAME,A.type1,B.CNT AS ITEMS from type A,(select DISTINCT stagec,count(icode) AS CNT from itwstage  GROUP BY STAGEC) B where A.id='K' AND A.TYPE1=B.STAGEC and a.type1 in('" + col1 + "') order by A.TYPE1";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[0]["Name"].ToString().Trim();
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[0]["TYPE1"].ToString().Trim();

                    //if (col1.Length <= 0) return;
                    ////********* Saving in Hidden Field 
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    ////********* Saving in GridView Value
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD1":
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
                            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.ToString();
                            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.ToString();
                            sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.ToString();
                            sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.ToString();
                            sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.ToString();
                            sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.ToString();
                            sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.ToString();
                            sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.ToString();
                            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.ToString();
                            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.ToString();
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.ToString();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.ToString().Replace("&amp;", "&");
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.ToString();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.ToString();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.ToString();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.ToString();
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
                            sg1_dr["sg1_t36"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t36")).Text.Trim();
                            sg1_dr["sg1_t37"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t37")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    stage = "0"; stagename = "";
                    hf1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL13");
                    int RowInsertAt = Convert.ToInt32(hf1.Value);
                    stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text;
                    dt = new DataTable();
                    SQuery = "select distinct b.Iname as iname,a.Icode as iCode,b.Cpartno,a.vchnum,a.qty,to_char(a.vchdate,'dd/mm/yyyy')as vchdate,trim(a.Icode)||'.'||trim(a.vchnum) as fstr,a.col17 from costestimate a, item b,itwstage c where trim(a.icode)=trim(b.icode) and a.type='30' and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.status='N' and c.stagec='" + stage + "' and trim(a.Icode)||'.'||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + col1 + ") order by trim(a.Icode)||'.'||trim(a.vchnum)";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        if (d == 0)
                        {
                            //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = "-";
                            //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["cpartno"].ToString().Trim();
                            //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[d]["vchnum"].ToString().Trim();
                            //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[d]["vchdate"].ToString().Trim();
                            //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = dt.Rows[d]["iname"].ToString().Trim();
                            //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["Cpartno"].ToString().Trim();
                            //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[22].Width = 70;
                            //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t6")).Text = dt.Rows[d]["qty"].ToString().Trim();
                            //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t21")).Text = dt.Rows[d]["iCode"].ToString().Trim();
                            //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select rate from type where id='K' and type1='" + stage + "'", "rate");
                            //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t5")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select excrate from type where id='K' and type1='" + stage + "'", "excrate");
                            //stagename = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STAGEC FROM ITWSTAGE WHERE SRNO>(SELECT SRNO FROM ITWSTAGE WHERE ICODE='90020488' AND STAGEC='" + stage + "' AND ROWNUM<=1)AND ROWNUM<=1 ORDER BY SRNO", "stagec");
                            //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t36")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='K' and type1='" + stagename + " '", "name");
                        }
                        else
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";
                            sg1_dr["sg1_f1"] = stage;
                            sg1_dr["sg1_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "select a.type1 as fstr,A.NAME,A.type1,B.CNT AS ITEMS from type A,(select DISTINCT stagec,count(icode) AS CNT from itwstage  GROUP BY STAGEC) B where A.id='K' AND A.TYPE1=B.STAGEC and a.type1 in('" + stage + "') order by A.TYPE1", "name");
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = dt.Rows[d]["vchdate"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["vchnum"].ToString().Trim();
                            sg1_dr["sg1_f6"] = dt.Rows[d]["Cpartno"].ToString().Trim();
                            sg1_dr["sg1_t1"] = "";
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_t4"] = fgen.seek_iname(frm_qstr, frm_cocd, "select rate from type where id='K' and type1='" + stage + "'", "rate");
                            sg1_dr["sg1_t5"] = fgen.seek_iname(frm_qstr, frm_cocd, "select excrate from type where id='K' and type1='" + stage + "'", "excrate");
                            sg1_dr["sg1_t6"] = dt.Rows[d]["qty"].ToString().Trim();
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
                            sg1_dr["sg1_t21"] = dt.Rows[d]["icode"].ToString().Trim();
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
                            stagename = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STAGEC FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND ICODE='" + dt.Rows[d]["ICODE"].ToString().Trim() + "' AND SRNO>(SELECT SRNO FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND  TYPE='10' AND ICODE='" + dt.Rows[d]["ICODE"].ToString().Trim() + "' AND STAGEC='" + stage + "' AND ROWNUM<=1)AND ROWNUM<=1 ORDER BY SRNO", "stagec");
                            sg1_dr["sg1_t36"] = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='K' and type1='" + stagename + "'", "name");
                            sg1_dr["sg1_t37"] = "";
                            sg1_dt.Rows.InsertAt(sg1_dr, RowInsertAt);
                        }
                        RowInsertAt++;
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        int d = 0;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["cpartno"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[d]["vchnum"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[d]["vchdate"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = dt.Rows[d]["iname"].ToString().Trim();
                        //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[22].Width = 70;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t6")).Text = dt.Rows[d]["qty"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t21")).Text = dt.Rows[d]["iCode"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select rate from type where id='K' and type1='" + stage + "'", "rate");
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t5")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select excrate from type where id='K' and type1='" + stage + "'", "excrate");
                        // string f = "SELECT STAGEC FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND ICODE='" + dt.Rows[d]["ICODE"].ToString().Trim() + "' AND SRNO>(SELECT SRNO FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND  TYPE='10' AND ICODE='" + dt.Rows[d]["ICODE"].ToString().Trim() + "' AND STAGEC='" + stage + "' AND ROWNUM<=1)AND ROWNUM<=1 ORDER BY SRNO";
                        stagename = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STAGEC FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND ICODE='" + dt.Rows[d]["ICODE"].ToString().Trim() + "' AND SRNO>(SELECT SRNO FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND  TYPE='10' AND ICODE='" + dt.Rows[d]["ICODE"].ToString().Trim() + "' AND STAGEC='" + stage + "' AND ROWNUM<=1)AND ROWNUM<=1 ORDER BY SRNO", "stagec");
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t36")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='K' and type1='" + stagename + "'", "name");
                    }

                    for (int i = 0; i < sg1.Rows.Count; i++)
                    {
                        sg1.Rows[i].Cells[12].Text = (i + 1).ToString();
                    }
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    fetch_col_rejection();
                    fetch_col_downtime();
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
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim().Replace("&amp;", "&");
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.Trim();
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
                            sg1_dr["sg1_t36"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t36")).Text.Trim();
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
                    setColHeadings();
                    break;

                case "WorkOrder":
                    hffield.Value = "WorkOrder";
                    hfWorkOrder.Value = col1;
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "select a.Vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.stage,t.name as stage_name,a.icode,b.Iname,b.Cpartno,a.shftcode,a.prevcode as shift,a.mchcode as machine_code,a.ename as machine,a.Ent_by,a.ent_Dt ,to_Char(a.vchdate,'yyyymmdd') as vdd,a.srno from " + frm_tabname + " a,item b,type t where trim(A.icode)=trim(b.icode) and trim(a.stage)=trim(t.type1) and t.id='K' and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + " order by vdd ,a.vchnum ,a.srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "WorkOrder")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "select trim(b.iname)||'['||trim(b.cpartno)||']' as Model_Name,trim(c.name) as Process,trim(a.vchnum) as Wo_Num,to_char(a.vchdate,'dd/mm/yyyy') as Wo_Date,a.prevcode as Shift_Code ,a.a2 as Ok_Prod,a.a7 as OK_NG,nvl(a.a2,0)+nvl(a.a7,0) as Tot_Prod,a.Remarks as Down_Time_Reason,A.Remarks2 as Rejn_Reason,a.icode,a.stage From prod_sheet a,item b,(select NAME,type1 from type where id='K') c where trim(a.icode)=trim(b.icode) and trim(a.stage)=trim(c.type1) and a.branchcd='" + frm_mbr + "'  and  a.type='90'  and a.vchdate " + DateRange + " and ((a.a2>0) or (a.a7>0))  and a.stage ='" + hfWorkOrder.Value.Trim() + "' order by a.vchdate,a.vchnum,a.srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Work Order List For the Period " + fromdt + " To " + todt, frm_qstr);
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

            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
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
                                save_it = "Y";
                            }

                            if (save_it == "Y")
                            {
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N")
                                {
                                    fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return;
                                }
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
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + frm_vnum + " Saved Successfully ");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
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
    ////------------------------------------------------------------------------------------
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
        sg1_dt.Columns.Add(new DataColumn("sg1_t36", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t37", typeof(string)));
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
        if (sg1_dt == null) return;
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
        sg1_dr["sg1_t36"] = "-";
        sg1_dr["sg1_t37"] = "-";
        sg1_dt.Rows.Add(sg1_dr);
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
            //sg1.HeaderRow.Cells[20].Width = 100;
            //sg1.HeaderRow.Cells[21].Width = 100;
            //sg1.HeaderRow.Cells[30].Width = 100;
            //sg1.HeaderRow.Cells[31].Width = 100;
            //sg1.HeaderRow.Cells[32].Width = 100;
            //sg1.HeaderRow.Cells[33].Width = 100;
            //sg1.HeaderRow.Cells[34].Width = 100;
            //sg1.HeaderRow.Cells[35].Width = 100;
            //sg1.HeaderRow.Cells[36].Width = 100;
            //sg1.HeaderRow.Cells[37].Width = 100;
            //sg1.HeaderRow.Cells[38].Width = 100;
            //sg1.HeaderRow.Cells[39].Width = 100;
            //sg1.HeaderRow.Cells[45].Width = 100;
            //sg1.HeaderRow.Cells[46].Width = 100;
            //sg1.HeaderRow.Cells[47].Width = 100;
            //sg1.HeaderRow.Cells[48].Width = 100;
            //sg1.HeaderRow.Cells[49].Width = 100;
            //sg1.HeaderRow.Cells[50].Width = 100;

            //sg1.Rows[sg1r].Cells[8].Attributes.Add("readonly", "false");
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
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Stage And Its Details From The List");
                }
                break;


            case "SG1_ROW_ADD":
                if (txtlbl7.Text.Length > 1)
                {
                    if (index < sg1.Rows.Count - 1)
                    {
                        hf1.Value = index.ToString();
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                        //----------------------------
                        hffield.Value = "SG1_ROW_ADD_E";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("Select Stage", frm_qstr);
                    }
                    else
                    {
                        hffield.Value = "SG1_ROW_ADD";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("Select Stage", frm_qstr);
                        //fgen.Fn_open_mseek("Select Item", frm_qstr);
                    }
                }
                else
                {
                    fgen.msg("-", "AMSG", "Please Select Machine First!!");
                    btnlbl7.Focus(); return;
                }
                break;

            case "SG1_ROW_ADD1":

                if (sg1.Rows[Convert.ToInt32(index)].Cells[13].Text.Trim().Length > 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL13", hf1.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    hffield.Value = "SG1_ROW_ADD1";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select item", frm_qstr);
                }

                else
                {
                    fgen.msg("-", "AMSG", "Please Select Stage First!!");
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
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Result", frm_qstr);
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
        fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum.Trim().ToUpper();
                oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
                oporow["SRNO"] = i + 1;
                //save data into the prod_sheet table
                oporow["icode"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim().ToUpper();
                oporow["acode"] = "-";
                oporow["a1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper());
                oporow["a2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper());
                oporow["a3"] = 0;
                oporow["a4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper());
                double q1 = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper()) + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper());
                oporow["a5"] = q1;
                double q = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper()) - fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper());
                oporow["a6"] = q;
                oporow["total"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper());
                oporow["mlt_loss"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper());
                oporow["a7"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim().ToUpper());
                oporow["a8"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim().ToUpper());
                oporow["a9"] = 0;
                oporow["a10"] = 0;
                oporow["stage"] = sg1.Rows[i].Cells[13].Text.Trim().ToUpper();
                oporow["iqtyin"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper());
                oporow["iqtyout"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper());
                oporow["subcode"] = txtlbl101.Text;
                oporow["mchcode"] = txtlbl7.Text;
                oporow["ename"] = txtlbl7a.Text;
                oporow["prevcode"] = txtlbl4a.Text.Trim().ToUpper().ToUpper();
                // oporow["prevstage"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t36")).Text);
                oporow["SHFTCODE"] = txtlbl4.Text.Trim().ToUpper();
                oporow["noups"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t37")).Text.Trim().ToUpper());
                oporow["job_no"] = sg1.Rows[i].Cells[17].Text.Trim().ToUpper();
                oporow["job_dt"] = sg1.Rows[i].Cells[16].Text.Trim().ToUpper();
                oporow["mcstart"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
                oporow["mcstop"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper();

                oporow["un_melt"] = 0;
                oporow["flag"] = 1;
                oporow["lmd"] = 0;
                oporow["bcd"] = 0;
                oporow["num11"] = 0; // in web only 10 rej reasons are showing and this filed id is for 11 reason
                oporow["num12"] = 0; // in web only 10 rej reasons are showing and this filed id is for 12 reason
                oporow["mtime"] = "-";
                oporow["exc_time"] = "-";
                oporow["tempr"] = "-";
                oporow["irate"] = 0;
                oporow["mseq"] = 0;
                oporow["fm_fact"] = 1;
                oporow["pcpshot"] = 1;
                oporow["PBTCHNO"] = "-";
                oporow["OPR_DTL"] = "-";
                oporow["OEE_R"] = 0;
                oporow["HCUT"] = 0;
                oporow["ALSTTIM"] = 0;
                oporow["ALTCTIM"] = 0;
                oporow["CUST_REF"] = 0;
                oporow["CELL_REF"] = "-";
                oporow["CELL_REFN"] = "-";
                oporow["dcode"] = "-";
                oporow["a21"] = 0;
                oporow["a22"] = 0;
                oporow["a23"] = 0;
                oporow["a24"] = 0;
                oporow["a25"] = 0;
                oporow["a26"] = 0;
                oporow["a27"] = 0;
                oporow["a28"] = 0;
                oporow["a29"] = 0;
                oporow["a30"] = 0;
                oporow["prevstage"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STAGEC FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND ICODE='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim().ToUpper() + "' AND SRNO>(SELECT SRNO FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND  TYPE='10' AND ICODE='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim().ToUpper() + "' AND STAGEC='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "' AND ROWNUM<=1)AND ROWNUM<=1 ORDER BY SRNO", "stagec");
                oporow["var_code"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper();
                oporow["naration"] = txtrmk.Text.Trim().ToUpper().ToUpper();
                oporow["empcode"] = "-";
                oporow["film_code"] = "-";

                // time slot is not saving as it will subtract the two dates but error is occured.(no date time)
                // oporow["TSLOT"] = Convert.ToDateTime("01/01/2010" + " " + ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text) - Convert.ToDateTime("01/01/2010" + " " + ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper());
                DateTime date1 = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text);
                DateTime date2 = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
                TimeSpan Diff = date1 - date2;
                oporow["TSLOT"] = Diff.TotalMinutes.ToString();
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t36")).Text.Trim().ToUpper().Length > 10)
                {
                    oporow["glue_code"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t36")).Text.Trim().ToUpper().Substring(0, 9);
                }
                else
                {
                    oporow["glue_code"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t36")).Text.Trim().ToUpper();
                }

                oporow["wo_no"] = frm_vnum;
                oporow["wo_dt"] = txtvchdate.Text;
                oporow["Remarks"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text.Trim().ToUpper();
                oporow["Remarks2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text.Trim().ToUpper();

                // add rejection columns
                oporow["num1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim().ToUpper());
                oporow["num2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim().ToUpper());
                oporow["num3"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim().ToUpper());
                oporow["num4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim().ToUpper());
                oporow["num5"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim().ToUpper());
                oporow["num6"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().ToUpper());
                oporow["num7"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim().ToUpper());
                oporow["num8"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim().ToUpper());
                oporow["num9"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim().ToUpper());
                oporow["num10"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim().ToUpper());

                //add downtime columns
                oporow["a11"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text.Trim().ToUpper());
                oporow["a12"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t27")).Text.Trim().ToUpper());
                oporow["a13"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t28")).Text.Trim().ToUpper());
                oporow["a14"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t29")).Text.Trim().ToUpper());
                oporow["a15"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t30")).Text.Trim().ToUpper());
                oporow["a16"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t31")).Text.Trim().ToUpper());
                oporow["a17"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t32")).Text.Trim().ToUpper());
                oporow["a18"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t33")).Text.Trim().ToUpper());
                oporow["a19"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t34")).Text.Trim().ToUpper());
                oporow["a20"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t35")).Text.Trim().ToUpper());

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
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {

    }
    //------------------------------------------------------------------------------------
    void save_fun3()
    {

    }
    //------------------------------------------------------------------------------------
    void save_fun4()
    {

    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "86");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------
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
    public void fetch_col_rejection()
    {
        DataTable dt2 = new DataTable();
        SQuery = "select  initcap(substr(Name,1,10)) as Name from (Select  ID,Name,type1 from type where id='4' order by type1) where rownum<=10";

        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg1_dr = sg1_dt.NewRow();
        i = 0;

        if (dt2.Rows.Count > 0)
        {
            int d = 30;
            do
            {
                sg1.HeaderRow.Cells[d].Text = dt2.Rows[i]["Name"].ToString().Trim();
                d = d + 1;
                i = i + 1;

            } while (i < dt2.Rows.Count);
        }
    }
    //------------------------------------------------------------------------------------
    public void fetch_col_downtime()
    {
        DataTable dt2 = new DataTable();

        SQuery = "select initcap(substr(Name,1,10)) as Name  from (Select  ID,Name,type1 from type where id='8' order by type1) where rownum<=10";

        //SQuery = "select lower(substr(Name,1,10)) as Name from(Select  Name,type1,branchcd from typewip where branchcd!='DD' and id='DTC61' order by type1) where rownum<=10";

        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg1_dr = sg1_dt.NewRow();
        i = 0;

        if (dt2.Rows.Count > 0)
        {
            int d = 45;
            do
            {
                sg1.HeaderRow.Cells[d].Text = dt2.Rows[i]["Name"].ToString().Trim();
                d = d + 1;
                i = i + 1;

            } while (i < dt2.Rows.Count);
        }

    }
    //------------------------------------------------------------------------------------   
    protected void btnWorkOrder_Click(object sender, EventArgs e)
    {
        hffield.Value = "WorkOrder";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Stage", frm_qstr);
    }
    //------------------------------------------------------------------------------------   
    protected void btnUnappJob_Click(object sender, EventArgs e)
    {
        SQuery = "SELECT TRIM(B.INAME) as Item_Name,TRIM(a.icode) AS ITEM_CODE,a.Type,TRIM(A.vchnum) AS Job_No,TO_CHAR(A.vchdate,'DD/MM/YYYY') as Dated,substr(TRIM(a.convdate),5,6) as ordno,TO_CHAR(NVL(A.QTY,0),'999,999,999,999.99') as Qty,TRIM(B.CPARTNO) AS CPARTNO ,TRIM(a.col12) as Rmk from costestimate A,ITEM B  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + DateRange + "  and trim(nvl(a.app_by,'-'))='-' and A.SRNO=0 order by A.vchdate desc ,A.vchnum desc";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("Unapproved Job Cards For the Period " + fromdt + " To " + todt, frm_qstr);
    }
}
//------------------------------------------------------------------------------------
