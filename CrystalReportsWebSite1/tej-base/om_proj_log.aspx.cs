using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_proj_log : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange;
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
                string chk_opt;
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0001'", "fstr");
                if (chk_opt != "Y")
                {
                    tab3.Visible = false;
                    btnPost.Visible = false;
                }
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0002'", "fstr");
                if (chk_opt != "Y")
                {
                    txtBarCode.Visible = false;
                    btnRead.Visible = false;
                }

                doc_addl.Value = "N";
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0003'", "fstr");
                if (chk_opt == "Y")
                {
                    doc_addl.Value = "Y";
                }

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
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

                ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");

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
        create_tab4();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();
        sg4_add_blankrows();

        btnlbl4.Enabled = false;
        btnlbl7.Enabled = false;

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
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
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

        frm_tabname = "WB_PROJ_LOG";

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
                SQuery = "select * from (select Acode,ANAME as Transporter,Acode as Code,Addr1 as Address,Addr2 as City from famst  where upper(ccode)='T' union all select 'Own' as Acode,'OWN' as Transporter,'-' as Code,'-' as Address,'-' as City from dual union all select 'PARTY VEHICLE' as Transporter,'-' as Code,'-' as Address,'-' as City from dual) order by  Transporter";
                break;
            case "BTN_17":
                SQuery = "Select Type1 as fstr,Name,type1 from type where id='>' order by name";
                break;
            case "BTN_18":
                SQuery = "Select Type1 as fstr,Name,type1 from type where id='<' order by name";
                break;

            case "BTN_19":
                SQuery = "SELECT '10' as fstr,'As Applicable' as NAME,'10' as Code FROM dual ";
                break;

            case "BTN_20":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM FAMST where substr(acode,1,2) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_21":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM FAMST where substr(acode,1,2) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_22":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM FAMST where substr(acode,1,2) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_23":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1,rate  FROM TYPE WHERE ID='A' order by name ";
                break;
            case "TACODE":
                SQuery = "SELECT DISTINCT a.USERNAME ,a.USERNAME AS COCD,a.FULL_NAME AS company_name FROM EVAS a,wb_proj_setup b WHERE trim(a.username)=trim(b.ccode) ORDER BY USERNAME";

                break;
            case "TICODE":
                //pop2

                SQuery = "select type1,name as State ,type1 as code from type where id='1' order by Name";

                //SQuery = "SELECT ICODE AS FSTR,INAME AS PRODUCT,ICODE AS CODE,UNIT,CPARTNO AS PARTNO FROM ITEM WHERE LENGTH(tRIM(ICODE))>4 ";
                break;
            case "TICODEX":
                SQuery = "select type1,name as State ,type1 as code from type where id='{' order by Name";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
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

                SQuery = "SELECT distinct a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(to_Char(a.srno,'9999')) as FStr,a.Mod_Detail as Module_detail,a.vchnum as Proj_no,to_char(a.vchdate,'dd/mm/yyyy') as Proj_Dt,A.mod_dlv_by as Mod_dlv_by,a.mod_tgtdt,a.srno,to_char(a.vchdate,'yyyymmdd') as VDD from wb_proj_setup a where trim(A.ccode) ='" + txtlbl4.Text.Trim() + "' and nvl(a.icat,'-')!='Y' order by VDD,a.vchnum,a.srno ";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);
                break;
            case "SG1_ROW_JOB":
                SQuery = "select * from (Select a.Vchnum||to_char(a.vchdate,'dd/mm/yyyy') as Fstr,B.Iname,b.Cpartno,b.cdrgno,A.Vchnum as Job_no,to_char(A.vchdate,'dd/mm/yyyy')as Job_Dt from costestimate a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.status!='Y' and a.vchdate " + DateRange + " and a.srno=1 order by a.vchdate desc,a.vchnum desc) where rownum<100";
                break;
            case "SG1_ROW_BTCH":
                SQuery = "select * from (Select a.Vchnum||to_char(a.vchdate,'dd/mm/yyyy') as Fstr,B.Iname,b.Cpartno,b.cdrgno,A.Vchnum as Job_no,to_char(A.vchdate,'dd/mm/yyyy')as Job_Dt from costestimate a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.status!='Y' and a.vchdate " + DateRange + " and a.srno=1 order by a.vchdate desc,a.vchnum desc) where rownum<100";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as entry_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as entry_Dt,a.Cust_Name ,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " order by vdd desc,a." + doc_nf.Value + " desc";
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
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
            make_qry_4_popup();
            fgen.Fn_open_sseek("select type", frm_qstr);

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
        if (txtlbl4.Text.Trim().Length < 2)
        {
            Checked_ok = "N";
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Department Not Filled Correctly !!");
        }

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }
        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only"); txtvchdate.Focus(); return; }



        int i;
        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[15].Text.Trim().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Length  <= 1)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Work Done Not Filled correctly at Line " + (i + 1) + "  !!");

                i = sg1.Rows.Count;
                return;
            }

            if (sg1.Rows[i].Cells[15].Text.Trim().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Length  <= 1)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Trainee not Filled correctly at Line " + (i + 1) + "  !!");

                i = sg1.Rows.Count;
                return;
            }

            if (sg1.Rows[i].Cells[15].Text.Trim().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Length  <= 1)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Trainee Deptt not Filled correctly at Line " + (i + 1) + "  !!");

                i = sg1.Rows.Count;
                return;
            }


            if (sg1.Rows[i].Cells[15].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text) <= 0)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Trainee Hrs not Filled correctly at Line " + (i + 1) + "  !!");

                i = sg1.Rows.Count;
                return;
            }

            if (sg1.Rows[i].Cells[15].Text.Trim().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Length < 10)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Date Not Filled Correctly at Line " + (i + 1) + "  !!");

                i = sg1.Rows.Count;
                return;
            }
            else
            {
                string curr_dt;
                string reqd_bydt;
                if (sg1.Rows[i].Cells[15].Text.Trim().Length > 2)
                {
                    curr_dt = Convert.ToDateTime(txtvchdate.Text).ToString("dd/MM/yyyy");
                    reqd_bydt = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text).ToString("dd/MM/yyyy");

                    if (Convert.ToDateTime(curr_dt) < Convert.ToDateTime(reqd_bydt))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Training Date Cannot be More than Current Date, See line No. " + (i + 1) + "  !!");
                        i = sg1.Rows.Count;
                        return;
                    }
                }
            }
        }



        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "Y");
        string ok_for_save = "Y"; string err_item, err_msg;










        //checkGridQty();

        ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
        err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");


        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
    }
    //------------------------------------------------------------------------------------
    string checkGridQty()
    {
        DataTable dtQty = new DataTable();
        dtQty.Columns.Add(new DataColumn("fstr", typeof(string)));
        dtQty.Columns.Add(new DataColumn("qty", typeof(double)));
        dtQty.Columns.Add(new DataColumn("iname", typeof(string)));
        DataRow drQty = null;
        foreach (GridViewRow gr in sg1.Rows)
        {
            if (gr.Cells[13].Text.ToString().Trim().Length > 4)
            {
                drQty = dtQty.NewRow();
                drQty["fstr"] = gr.Cells[13].Text.ToString().Trim() + "-" + txtlbl2.Text + "-" + txtlbl3.Text;
                drQty["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t2")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
            }
        }
        object sm;

        DataView distQty = new DataView(dtQty, "", "fstr", DataViewRowState.CurrentRows);
        DataTable dtQty1 = new DataTable();
        dtQty1 = distQty.ToTable(true, "fstr");
        foreach (DataRow drQty1 in dtQty1.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "fstr='" + drQty1["fstr"].ToString().Trim() + "'");


            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select (a.Qtyord)-(a.Soldqty) as Bal_Qty from (select fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||vchnum||'-'||to_ChaR(vchdate,'dd/mm/yyyy') as fstr,trim(Icode) as ERP_code,iqty_chl as Qtyord,0 as Soldqty,1 as prate from ivoucherp where branchcd='" + frm_mbr + "' and type like '00%'  and trim(Acode)||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')='" + txtlbl7.Text.Trim() + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "' union all SELECT trim(icode)||'-'||genum||'-'||to_ChaR(gedate,'dd/mm/yyyy') as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate from ivoucher where branchcd='" + frm_mbr + "' and type='0%' and trim(Acode)||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')='" + txtlbl7.Text.Trim() + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "' and trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and a.fstr='" + drQty1["fstr"].ToString().Trim() + "' order by B.Iname,trim(a.fstr)", "Bal_Qty");

            if (fgen.make_double(sm.ToString()) > fgen.make_double(col1) && fgen.make_double(col1) > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", drQty1["fstr"].ToString().Trim());
                break;
            }
        }
        return null;
    }

    string reelGridQty()
    {
        DataTable dtQty = new DataTable();
        dtQty.Columns.Add(new DataColumn("icode", typeof(string)));
        dtQty.Columns.Add(new DataColumn("qty", typeof(double)));
        dtQty.Columns.Add(new DataColumn("rcount", typeof(double)));
        dtQty.Columns.Add(new DataColumn("iname", typeof(string)));
        DataRow drQty = null;
        foreach (GridViewRow gr in sg1.Rows)
        {
            if (gr.Cells[13].Text.ToString().Trim().Length > 4)
            {
                drQty = dtQty.NewRow();
                drQty["icode"] = gr.Cells[13].Text.ToString().Trim();
                drQty["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t2")).Text.ToString().Trim());
                drQty["rcount"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t4")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
            }
        }

        DataTable dtQty1 = new DataTable();
        dtQty1.Columns.Add(new DataColumn("icode", typeof(string)));
        dtQty1.Columns.Add(new DataColumn("qty", typeof(double)));
        dtQty1.Columns.Add(new DataColumn("iname", typeof(string)));
        dtQty1.Columns.Add(new DataColumn("rcount", typeof(decimal)));
        DataRow drQty1 = null;
        col1 = "";
        i = 1;
        foreach (GridViewRow gr in sg2.Rows)
        {
            if (gr.Cells[3].Text.ToString().Trim().Length > 4 && fgen.make_double(((TextBox)gr.FindControl("sg2_t4")).Text.ToString().Trim()) > 0)
            {
                if (col1 != gr.Cells[3].Text.ToString().Trim()) i = 1;
                drQty1 = dtQty1.NewRow();
                drQty1["icode"] = gr.Cells[3].Text.ToString().Trim();
                col1 = gr.Cells[3].Text.ToString().Trim();
                drQty1["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty1["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg2_t4")).Text.ToString().Trim());
                drQty1["rcount"] = i;
                dtQty1.Rows.Add(drQty1);
                i++;
            }
        }

        object sm, sm1;

        DataView distQty = new DataView(dtQty, "", "icode", DataViewRowState.CurrentRows);
        DataTable dtQty2 = new DataTable();
        dtQty2 = distQty.ToTable(true, "icode");

        foreach (DataRow drQty2 in dtQty2.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "icode='" + drQty2["icode"].ToString().Trim() + "'");
            sm1 = dtQty1.Compute("sum(qty)", "icode='" + drQty2["icode"].ToString().Trim() + "'");

            if (fgen.make_double(sm.ToString()) != fgen.make_double(sm1.ToString()) && fgen.make_double(sm1.ToString()) > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_MSG", "Qty Mismatch for Item " + fgen.seek_iname_dt(dtQty, "icode='" + drQty2["icode"].ToString().Trim() + "'", "iname") + "'13' Grid 1 Qty : " + sm.ToString() + "'13'Grid 2 Qty : " + sm1.ToString());
                break;
            }

            sm = dtQty1.Compute("max(rcount)", "icode='" + drQty2["icode"].ToString().Trim() + "'");
            sm1 = dtQty.Compute("sum(rcount)", "icode='" + drQty2["icode"].ToString().Trim() + "'");

            if (fgen.make_double(sm.ToString()) != fgen.make_double(sm1.ToString()) && fgen.make_double(sm1.ToString()) > 0 && fgen.make_double(sm.ToString()) > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_MSG", "Qty Mismatch for Item " + fgen.seek_iname_dt(dtQty, "icode='" + drQty2["icode"].ToString().Trim() + "'", "iname") + "'13' Grid 1 Count : " + sm.ToString() + "'13'Grid 2 Count : " + sm1.ToString());
                break;
            }
        }
        return null;
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
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
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
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from poterm a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from budgmst a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
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
                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

                    txtlbl2.Text = frm_uname;


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

                    sg2_dt = new DataTable();
                    create_tab2();
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
                    //-------------------------------------------


                    break;
                    #endregion
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
                    fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();

                    string mv_col;
                    mv_col = frm_mbr + frm_vty + col1;

                    SQuery = "Select a.*,to_char(A.projdt,'dd/mm/yyyy') as proj_Dt,to_char(A.ent_Dt,'dd/mm/yyyy') as entdtd from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txtvchnum.Text = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl2.Text = dt.Rows[i]["ent_by"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["entdtd"].ToString().Trim();

                        txtlbl4.Text = dt.Rows[i]["ccode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["cust_name"].ToString().Trim();

                        txtrmk.Text = dt.Rows[i]["remarks"].ToString().Trim();

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


                            sg1_dr["sg1_f1"] = dt.Rows[i]["proj_Srn"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["mod_Detail"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["projno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["proj_Dt"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["MOD_DLV_BY"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["work_done"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["trainee"].ToString().Trim();

                            sg1_dr["sg1_t3"] = dt.Rows[i]["trainee_dpt"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["train_Dt"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["train_hrs"].ToString().Trim();


                            sg1_dt.Rows.Add(sg1_dr);
                        }


                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        //------------------------

                        // REEL TABLE
                        SQuery = "SELECT A.*,b.iname FROM REELVCH A,item b WHERE trim(a.icodE)=trim(B.icode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' order by a.srno";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        create_tab2();
                        sg2_dr = null;
                        i = 1;
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                sg2_dr = sg2_dt.NewRow();

                                sg2_dr["sg2_srno"] = i;
                                sg2_dr["sg2_h1"] = dr["icode"].ToString().Trim();
                                sg2_dr["sg2_h2"] = dr["icode"].ToString().Trim();
                                sg2_dr["sg2_h3"] = "";
                                sg2_dr["sg2_h4"] = "";
                                sg2_dr["sg2_h5"] = "";

                                sg2_dr["sg2_f1"] = dr["icode"].ToString().Trim();
                                sg2_dr["sg2_f2"] = dr["iname"].ToString().Trim();
                                sg2_dr["sg2_f3"] = "";
                                sg2_dr["sg2_f4"] = "";
                                sg2_dr["sg2_f5"] = "";

                                sg2_dr["sg2_t1"] = dr["kclreelno"].ToString().Trim();
                                sg2_dr["sg2_t2"] = dr["psize"].ToString().Trim();
                                sg2_dr["sg2_t3"] = dr["gsm"].ToString().Trim();
                                sg2_dr["sg2_t4"] = dr["reelwin"].ToString().Trim();
                                sg2_dr["sg2_t5"] = dr["irate"].ToString().Trim();
                                sg2_dr["sg2_t6"] = dr["coreelno"].ToString().Trim();
                                sg2_dr["sg2_t7"] = dr["reelspec1"].ToString().Trim();
                                sg2_dr["sg2_t8"] = dr["reelspec2"].ToString().Trim();
                                sg2_dr["sg2_t9"] = i.ToString(); ;
                                sg2_dr["sg2_t10"] = "";

                                sg2_dt.Rows.Add(sg2_dr);
                                i++;
                            }
                        }
                        sg2_add_blankrows();
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        dt.Dispose();
                        sg2_dt.Dispose();

                        //-----------------------
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

                        ////------------------------
                        //SQuery = "Select a.icode,to_chaR(a.dlv_Date,'dd/mm/yyyy') As dlv_Date,nvl(a.budgetcost,0) as budgetcost,nvl(a.actualcost,0) as actualcost,a.jobcardrqd,b.iname,nvl(b.cpartno,'-') As cpartno,nvl(b.cdrgno,'-') as cdrgno,nvl(b.unit,'-') as Unit from budgmst a,item b where trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' and 1=2 ORDER BY A.SRNO ";
                        ////union all Select '-' as icode,to_DatE(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy') as dlv_Date,0 as budgetcost,'-' as iname,'-' As cpartno,'-' as cdrgno,nvl(b.unit,'-') as Unit from dual              

                        //dt = new DataTable();
                        //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

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
                        //sg3_dt.Dispose();

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
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_prod_reps(frm_qstr);
                    break;
                case "TACODE":
                    //-----------------------------
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col3;

                    break;


                case "BTN_10":
                    if (col1.Length <= 0) return;
                    txtlbl10.Text = col2;
                    btnlbl11.Focus();
                    break;
                case "BTN_11":
                    if (col1.Length <= 0) return;
                    txtlbl11.Text = col2;
                    btnlbl12.Focus();
                    break;
                case "BTN_12":
                    if (col1.Length <= 0) return;
                    txtlbl12.Text = col2;
                    btnlbl13.Focus();
                    break;
                case "BTN_13":
                    if (col1.Length <= 0) return;
                    txtlbl13.Text = col2;
                    btnlbl14.Focus();
                    break;
                case "BTN_14":
                    if (col1.Length <= 0) return;
                    txtlbl14.Text = col2;
                    break;
                case "BTN_15":

                    break;
                case "BTN_16":

                    break;
                case "BTN_17":

                    break;
                case "BTN_18":

                    break;


                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    txtlbl2.Focus();
                    break;
                case "TICODEX":
                    if (col1.Length <= 0) return;
                    //txtlbl70.Text = col1;
                    //txtlbl71.Text = col2;
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
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            //sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                            //sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                            sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();


                        String pop_qry;

                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");

                        SQuery = "select to_Char(A.vchdate,'dd/mm/yyyy') As vchdt,a.* from wb_proj_setup a where a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(to_Char(a.srno,'9999')) in (" + col1 + ")";

                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
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

                            sg1_dr["sg1_f1"] = dt.Rows[d]["srno"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["mod_Detail"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["vchnum"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["vchdt"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["mod_dlv_by"].ToString().Trim();

                            sg1_dr["sg1_t1"] = "";
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = "";
                            sg1_dr["sg1_t5"] = "";
                            sg1_dr["sg1_t6"] = "";

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    //dt.Dispose(); 
                    sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    setGST();
                    break;
                case "SG2_ROW_ADD":
                    if (col1.Length < 2) return;
                    #region for gridview 2
                    if (col1.Length <= 0) return;
                    if (ViewState["sg2"] != null)
                    {
                        dt = new DataTable();
                        sg2_dt = new DataTable();
                        dt = (DataTable)ViewState["sg2"];
                        z = dt.Rows.Count - 1;
                        sg2_dt = dt.Clone();
                        sg2_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = Convert.ToInt32(dt.Rows[i]["sg2_srno"].ToString());
                            sg2_dr["sg2_h1"] = dt.Rows[i]["sg2_h1"].ToString();
                            sg2_dr["sg2_h2"] = dt.Rows[i]["sg2_h2"].ToString();
                            sg2_dr["sg2_h3"] = dt.Rows[i]["sg2_h3"].ToString();
                            sg2_dr["sg2_h4"] = dt.Rows[i]["sg2_h4"].ToString();
                            sg2_dr["sg2_h5"] = dt.Rows[i]["sg2_h5"].ToString();

                            sg2_dr["sg2_f1"] = dt.Rows[i]["sg2_f1"].ToString();
                            sg2_dr["sg2_f2"] = dt.Rows[i]["sg2_f2"].ToString();
                            sg2_dr["sg2_f3"] = dt.Rows[i]["sg2_f3"].ToString();
                            sg2_dr["sg2_f4"] = dt.Rows[i]["sg2_f4"].ToString();
                            sg2_dr["sg2_f5"] = dt.Rows[i]["sg2_f5"].ToString();

                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                            sg2_dr["sg2_t3"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim();
                            sg2_dr["sg2_t4"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim();
                            sg2_dr["sg2_t5"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim();
                            sg2_dr["sg2_t6"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim();
                            sg2_dr["sg2_t7"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim();
                            sg2_dr["sg2_t8"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim();
                            sg2_dr["sg2_t9"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim();
                            sg2_dr["sg2_t10"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim();

                            sg2_dt.Rows.Add(sg2_dr);
                        }

                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_h1"] = col1;
                            sg2_dr["sg2_h2"] = col2;
                            sg2_dr["sg2_h3"] = "-";
                            sg2_dr["sg2_h4"] = "-";
                            sg2_dr["sg2_h5"] = "-";

                            sg2_dr["sg2_f1"] = col1;
                            sg2_dr["sg2_f2"] = col2;
                            sg2_dr["sg2_f3"] = "-";
                            sg2_dr["sg2_f4"] = "-";
                            sg2_dr["sg2_f5"] = "-";

                            sg2_dr["sg2_t1"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                            sg2_dr["sg2_t2"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                            sg2_dr["sg2_t3"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");
                            sg2_dr["sg2_t4"] = "0";
                            sg2_dr["sg2_t5"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9").ToString().Trim().Replace("&amp", "");

                            sg2_dt.Rows.Add(sg2_dr);
                        }
                    }
                    sg2_add_blankrows();

                    ViewState["sg2"] = sg2_dt;
                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    dt.Dispose(); sg2_dt.Dispose();
                    ((TextBox)sg2.Rows[z].FindControl("sg2_t1")).Focus();
                    #endregion
                    setColHeadings();
                    setGST();
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
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, col1, txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");

                    setColHeadings();
                    break;
                case "SG2_ROW_JOB":
                    if (col1.Length <= 0) return;
                    ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t6")).Text = col2;
                    ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t7")).Text = col3;
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
                case "SG1_ROW_JOB":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t8")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t9")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    break;
                case "SG1_ROW_BTCH":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t12")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
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
                            //sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                            //sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                            sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                            //sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            //sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();

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
        string party_cd = "";
        string part_cd = "";

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
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


            SQuery = "SELECT Vchnum as Log_no,to_char(Vchdate,'dd/mm/yyyy') as Log_dt,CCode,Cust_name,projno,to_char(projdt,'dd/mm/yyyy') as proj_dt,proj_srn as line_no,Mod_Detail,Mod_dlv_by,ent_by,Trainee,Trainee_dpt,Train_Dt,Train_hrs,to_char(vchdate,'yyyymmdd') as VDD FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and vchdate " + PrdRange + " and ccode like '" + party_cd + "%' and ent_by like '" + part_cd + "%' order by vdd,vchnum,srno";


            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------

            for (i = 0; i < sg1.Rows.Count - 0; i++)
            {
                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text) <= 0)
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");
                    i = sg1.Rows.Count;
                }
            }

            string last_entdt;
            //checks
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and " + doc_df.Value + " " + DateRange + " ", "ldt");
            if (last_entdt == "0") { }
            else if (edmode.Value != "Y")
            {
                if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
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
                    //oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "ivchctrl");

                    oDS3 = new DataSet();
                    oporow3 = null;
                    //oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "reelvch");

                    oDS4 = new DataSet();
                    oporow4 = null;
                    //oDS4 = fgen.fill_schema(frm_qstr, frm_cocd, "budgmst");

                    oDS5 = new DataSet();
                    oporow5 = null;
                    oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");

                    // This is for checking that, is it ready to save the data
                    frm_vnum = "000000";
                    save_fun();
                    save_fun2();
                    save_fun3();
                    //save_fun4();
                    save_fun5();

                    oDS.Dispose();
                    oporow = null;
                    oDS = new DataSet();
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                    oDS2.Dispose();
                    oporow2 = null;
                    oDS2 = new DataSet();
                    //oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "ivchctrl");

                    oDS3.Dispose();
                    oporow3 = null;
                    oDS3 = new DataSet();
                    //oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "reelvch");

                    oDS4.Dispose();
                    oporow4 = null;
                    oDS4 = new DataSet();
                    //oDS4 = fgen.fill_schema(frm_qstr, frm_cocd, "budgmst");

                    oDS5.Dispose();
                    oporow5 = null;
                    oDS5 = new DataSet();
                    oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");


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
                            if (sg1.Rows[i].Cells[15].Text.Trim().Length > 2)
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
                    save_fun2();
                    save_fun3();
                    //save_fun4();
                    save_fun5();
                    string ddl_fld1;
                    string ddl_fld2;
                    ddl_fld1 = frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    ddl_fld2 = frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");

                    if (edmode.Value == "Y")
                    {
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "update ivchctrl set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "update reelvch set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "update budgmst set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update udf_Data set branchcd='DD' where par_tbl='" + frm_tabname + "' and par_fld='" + ddl_fld1 + "'");

                    }

                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                    //fgen.save_data(frm_qstr, frm_cocd, oDS2, "ivchctrl");
                    //fgen.save_data(frm_qstr, frm_cocd, oDS3, "REELvch");
                    //fgen.save_data(frm_qstr, frm_cocd, oDS4, "budgmst");
                    fgen.save_data(frm_qstr, frm_cocd, oDS5, "udf_Data");

                    if (edmode.Value == "Y")
                    {
                        fgen.msg("-", "AMSG", "Data Updated Successfully");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivchctrl where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from REELvch where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from budgmst where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data where branchcd='DD' and par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + ddl_fld2 + "'");
                    }
                    else
                    {
                        if (save_it == "Y")
                        {
                            fgen.msg("-", "AMSG", "Data Saved Successfully");
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
                    fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                    fgen.msg("-", "AMSG", ex.Message.ToString());
                    col1 = "N";
                }
            }
            #endregion
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
        //sg1_dt.Columns.Add(new DataColumn("sg1_t17", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t18", typeof(string)));

    }
    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        // Hidden Field
        sg2_dt.Columns.Add(new DataColumn("sg2_h1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h5", typeof(string)));

        sg2_dt.Columns.Add(new DataColumn("sg2_f1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f5", typeof(string)));

        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t6", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t7", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t8", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t9", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t10", typeof(string)));
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
        //sg1_dr["sg1_t17"] = "-";
        //sg1_dr["sg1_t18"] = "-";

        sg1_dt.Rows.Add(sg1_dr);
    }
    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();

        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;

        sg2_dr["sg2_h1"] = "-";
        sg2_dr["sg2_h2"] = "-";
        sg2_dr["sg2_h3"] = "-";
        sg2_dr["sg2_h4"] = "-";
        sg2_dr["sg2_h5"] = "-";

        sg2_dr["sg2_f1"] = "-";
        sg2_dr["sg2_f2"] = "-";
        sg2_dr["sg2_f3"] = "-";
        sg2_dr["sg2_f4"] = "-";
        sg2_dr["sg2_f5"] = "-";

        sg2_dr["sg2_t1"] = "-";
        sg2_dr["sg2_t2"] = "-";
        sg2_dr["sg2_t3"] = "-";
        sg2_dr["sg2_t4"] = "-";
        sg2_dr["sg2_t5"] = "-";
        sg2_dr["sg2_t6"] = "-";
        sg2_dr["sg2_t7"] = "-";
        sg2_dr["sg2_t8"] = "-";
        sg2_dr["sg2_t9"] = "-";
        sg2_dr["sg2_t10"] = "-";

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

            setGST();
            //if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
            //{
            //    sg1.HeaderRow.Cells[24].Text = "CGST";
            //    sg1.HeaderRow.Cells[25].Text = "SGST/UTGST";
            //}
            //else
            //{
            //    sg1.HeaderRow.Cells[24].Text = "IGST";
            //    sg1.HeaderRow.Cells[25].Text = "-";
            //}
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
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "SG1_ROW_JOB":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_JOB";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Job No.", frm_qstr);
                }
                break;
            case "SG1_ROW_BTCH":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_BTCH";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Batch No.", frm_qstr);
                }
                break;

            case "SG1_ROW_ADD":

                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Item", frm_qstr);
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
                hffield.Value = "SG2_ROW_ADD";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

                col1 = "";
                foreach (GridViewRow gr2 in sg2.Rows)
                {
                    if (col1.Length > 0) col1 += ",'" + gr2.Cells[3].Text.Trim().ToString() + ((TextBox)gr2.FindControl("sg2_t1")).Text.Trim().ToString() + "'";
                    else col1 = "'" + gr2.Cells[3].Text.Trim().ToString() + ((TextBox)gr2.FindControl("sg2_t1")).Text.Trim().ToString() + "'";
                }

                SQuery = "SELECT TRIM(ICODe) AS FSTR,INAME AS PRODUCT,ICODE AS ERPCODE,OPRATE1 AS SIZE_,OPRATE3 AS GSM,UNIT FROM ITEM WHERE TRIM(ICODE) LIKE '7%' ORDER BY ICODE ";
                SQuery = "SELECT TRIM(A.ICODE) AS FSTR,B.INAME AS PRODUCT,A.ICODE AS ERPCODE,A.KCLREELNO,A.COREELNO,B.OPRATE1,B.OPRATE3,B.UNIT,a.irate FROM REELVCH A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) and trim(a.icode)||trim(a.kclreelno) not in (" + col1 + ") ";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek("Select Item", frm_qstr);
                break;
            case "SG2_ROW_JOB":
                hffield.Value = "SG2_ROW_JOB";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

                SQuery = "Select distinct a.vchnum||a.vchdate as fstr,trim(a.Vchnum) as Job_no,to_Char(a.vchdate,'dd/mm/yyyy') as job_Dt,a.type,b.iname as item_name from costestimate a,item b where trim(a.icode)=trim(b.icodE) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + DateRange + " order by trim(a.vchnum)  ";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek("Select Job No", frm_qstr);
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
    protected void sg4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg4.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
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

    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer ", frm_qstr);
    }
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
        hffield.Value = "BTN_14";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_15";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_16";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_17";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_18";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl19_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_19";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }



    //------------------------------------------------------------------------------------
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl70_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODEX";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type ", frm_qstr);
    }

    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[15].Text.Length > 2)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = lbl1a.Text.Substring(0, 2);
                oporow["vchnum"] = frm_vnum.Trim();
                oporow["vchdate"] = txtvchdate.Text.Trim();


                oporow["ccode"] = txtlbl4.Text.Trim();
                oporow["cust_name"] = txtlbl4a.Text.Trim();

                oporow["srno"] = i + 1;
                oporow["proj_srn"] = fgen.make_double(sg1.Rows[i].Cells[13].Text.Trim());
                oporow["Mod_detail"] = sg1.Rows[i].Cells[14].Text.Trim();
                oporow["projno"] = sg1.Rows[i].Cells[15].Text.Trim();
                oporow["projdt"] = Convert.ToDateTime(fgen.make_def_Date(sg1.Rows[i].Cells[16].Text.Trim(), vardate).ToString()).ToString("dd/MM/yyyy");
                oporow["MOD_DLV_BY"] = sg1.Rows[i].Cells[17].Text.Trim();

                oporow["work_done"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                oporow["trainee"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                oporow["trainee_Dpt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                oporow["train_dt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                oporow["train_hrs"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim());

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
                oporow["remarks"] = txtrmk.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
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
    void save_fun5()
    {
        for (i = 0; i < sg4.Rows.Count - 0; i++)
        {
            if (((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim().Length > 1)
            {
                oporow5 = oDS5.Tables[0].NewRow();
                oporow5["branchcd"] = frm_mbr;
                oporow5["par_tbl"] = frm_tabname.ToUpper().Trim();
                oporow5["par_fld"] = frm_mbr + lbl1a.Text + frm_vnum + txtvchdate.Text.Trim();
                oporow5["udf_name"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                oporow5["udf_value"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
                oporow5["SRNO"] = i;

                oDS5.Tables[0].Rows.Add(oporow5);
            }
        }
    }
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
    
        SQuery = "SELECT '10' AS FSTR,'Project Update' as NAME,'10' as CODE FROM dual";


    }
    //------------------------------------------------------------------------------------
    void setGST()
    {

    }
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
 
    }
    protected void btnPost_ServerClick(object sender, EventArgs e)
    {

    }
    protected void btnRead_ServerClick(object sender, EventArgs e)
    {
        dt = new DataTable();
        sg1_dt = new DataTable();
        if (frm_cocd == "BONY" || frm_cocd == "SRPF")
        {
            if (txtBarCode.Value.Trim().Length < 21) return;
            string str = txtBarCode.Value.Trim();
            if (str.Contains("\r")) str = str.Replace("\r", "$");
            if (str.Contains("\n")) str = str.Replace("\n", "$");
            if (str.Contains("$$")) str = str.Replace("$$", "$");
            string[] sp = str.Split('$');
            col1 = "";
            foreach (string s in sp)
            {
                if (s.Length > 1)
                {
                    if (col1.Length > 0) col1 = col1 + "," + "'" + s.ToString() + "'";
                    else col1 = "'" + s.ToString() + "'";
                }
            }
            if (col1.Length < 2) return;
            dt2 = new DataTable();
            dt2 = fgen.getdata(frm_qstr, frm_cocd, "Select a.*,b.iname,b.cpartno,b.unit,b.cdrgno from finprim.scratch a,finprim.item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||a.vchnum||to_Char(a.Vchdate,'dd/mm/yyyy')='" + col1 + "'");
            if (dt2.Rows.Count > 0)
            {
                #region for gridview 1
                if (ViewState["sg1"] != null)
                {
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
                        //sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                        sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                        sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                        //sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                        sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                        sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                        sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                        sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                        sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                        sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                        //sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                        //sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();

                        //sg1_dt.Rows.Add(sg1_dr);
                    }

                    for (int d = 0; d < dt2.Rows.Count; d++)
                    {
                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, "select a.icode,a.iname,a.cpartno,a.cdrgno,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from item a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.icode) ='" + dt2.Rows[d]["col3"].ToString().Trim() + "' ");
                        sg1_dr = sg1_dt.NewRow();
                        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        sg1_dr["sg1_h1"] = dt2.Rows[d]["col3"].ToString().Trim();
                        sg1_dr["sg1_h2"] = col1;
                        sg1_dr["sg1_h3"] = dt2.Rows[d]["icode"].ToString().Trim();
                        sg1_dr["sg1_h4"] = dt2.Rows[d]["col2"].ToString().Trim();
                        sg1_dr["sg1_h5"] = dt2.Rows[d]["vchdate"].ToString().Trim();
                        sg1_dr["sg1_h6"] = "-";
                        sg1_dr["sg1_h7"] = "-";
                        sg1_dr["sg1_h8"] = "-";
                        sg1_dr["sg1_h9"] = "-";
                        sg1_dr["sg1_h10"] = "-";

                        sg1_dr["sg1_f1"] = dt2.Rows[d]["col3"].ToString().Trim();

                        if (dt3.Rows.Count > 0)
                        {
                            sg1_dr["sg1_f2"] = dt3.Rows[0]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt3.Rows[0]["cpartno"].ToString().Trim();
                            //sg1_dr["sg1_f4"] = dt3.Rows[0]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[i]["Icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                            sg1_dr["sg1_f5"] = dt3.Rows[0]["unit"].ToString().Trim();

                            //if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                            //{
                            //    sg1_dr["sg1_t7"] = dt.Rows[d]["num4"].ToString().Trim();
                            //    sg1_dr["sg1_t8"] = dt.Rows[d]["num5"].ToString().Trim();
                            //}
                            //else
                            //{
                            //    sg1_dr["sg1_t7"] = dt.Rows[d]["num6"].ToString().Trim();
                            //    sg1_dr["sg1_t8"] = "0";
                            //}
                        }

                        sg1_dr["sg1_t1"] = dt2.Rows[d]["col4"].ToString().Trim();
                        sg1_dr["sg1_t2"] = dt2.Rows[d]["col4"].ToString().Trim();
                        sg1_dr["sg1_t3"] = 0;
                        sg1_dr["sg1_t4"] = 0;

                        sg1_dr["sg1_t9"] = "";
                        sg1_dr["sg1_t10"] = dt2.Rows[d]["col2"].ToString().Trim(); ;
                        //sg1_dr["sg1_t11"] = dt.Rows[d]["iexc_Addl"].ToString().Trim();
                        //sg1_dr["sg1_t12"] = dt.Rows[d]["frt_pu"].ToString().Trim();
                        //sg1_dr["sg1_t13"] = dt.Rows[d]["pkchg_pu"].ToString().Trim();

                        string mpo_Dt = fgen.seek_iname(frm_qstr, frm_cocd, "select * From (Select ordno||'~'||orddt||'~'||prate from pomas where branchcd='" + frm_mbr + "' and type like '5%' and trim(Acode)='" + txtlbl4.Text.Trim() + "' and trim(icode)='" + dt2.Rows[d]["icode"].ToString().Trim() + "' and pflag<>1 order by orddt desc) where rownum<2", "");
                        if (mpo_Dt.Length > 2)
                        {
                            foreach (var v in mpo_Dt.Split('~'))
                            {
                                sg1_dr["sg1_t14"] = v[0].ToString().Trim();
                                sg1_dr["sg1_t15"] = "";
                                sg1_dr["sg1_t16"] = fgen.make_def_Date(v[1].ToString().Trim(), vardate);
                                sg1_dr["sg1_t5"] = v[2].ToString().Trim();
                            }
                        }
                        sg1_dt.Rows.Add(sg1_dr);

                        txtlbl5.Text = dt2.Rows[d]["col9"].ToString().Trim();
                        txtlbl6.Text = dt2.Rows[d]["col10"].ToString().Trim();
                    }
                    dt.Dispose();
                }
                sg1_add_blankrows();

                ViewState["sg1"] = sg1_dt;
                sg1.DataSource = sg1_dt;
                sg1.DataBind();
                sg1_dt.Dispose();
                ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                #endregion
                setColHeadings();
                setGST();
            }
        }
    }
}