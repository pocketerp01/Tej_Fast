using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_attn_entryh : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, mq0, typePopup = "Y";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
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

        // to hide and show to tab panel      
        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        // tab6.Visible = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
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
        frm_tabname = "attn";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        lblheader.Text = "Daily Attendance";
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        SQuery = ""; set_Val();
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
                SQuery = "SELECT type1 as fstr,name as grade_name,Type1 as Grade_Code from type where id='I' and type1 like '0%' order by fstr";
                break;

            case "TICODE":
                SQuery = "select distinct trim(branch) as fstr,branch from empmas where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' order by branch";
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

            case "Edit_E_":
                SQuery = "select mthnum as fstr,mthnum,mthname from mths";
                break;

            case "Print":
            case "List":
            case "Edit":
            case "New":
            case "Del":
                Type_Sel_query();
                break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Entry_no,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Dt,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.grade='" + hf2.Value + "' and a.vchdate " + DateRange + " order by a.vchnum desc";

                else if (btnval == "Edit_E")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Entry_no,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Dt,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.grade='" + hf2.Value + "' and to_char(a.vchdate,'mm/yyyy')='" + col1 + "/" + frm_myear + "' order by a.vchnum desc";

                else if (btnval == "Print_E" || btnval == "List_E")
                    SQuery = "select distinct  trim(a.branchcd)||trim(a.grade)||trim(a.empcode) as fstr,b.name ,a.empcode,b.fhname from attn a,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode) =trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.grade='" + col1 + "' and a.vchdate " + DateRange + " order by name";
                break;
        }
        if (typePopup == "N" && (btnval == "Edit*" || btnval == "Del*" | btnval == "Print"))
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
            frm_vty = "10";
            lbl1a.Text = frm_vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);

            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select  Grade", frm_qstr);
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        frm_vty = "10";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        mq0 = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ";
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, mq0, 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl2.Text = frm_uname;
        txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl5.Text = "-";
        txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();
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
            fgen.Fn_open_sseek("Select  Grade", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
        hffield.Value = "Edit";
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Grade", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        Cal();
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        if (txtlbl4.Text.Length < 2)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Select Grade!!");
            return;
        }

        if (txtlbl7.Text.Length < 2)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Select Branch!!");
            return;
        }

        if (edmode.Value == "")
        {
            SQuery = "SELECT distinct vchnum  FROM attn WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10'  and to_char(vchdate,'dd/mm/yyyy')='" + txtvchdate.Text + "'";
            mq0 = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "vchnum");
            if (mq0.Length > 1)
            {
                fgen.msg("-", "AMSG", "Data Already Entered For This Date " + txtvchdate.Text + "");
                return;
            }
        }

        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text == "-" || ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text == "")
            {
                fgen.msg("-", "AMSG", "Please Enter Time In (Hrs) At Line No. " + sg1.Rows[i].Cells[12].Text.Trim() + "");
                return;
            }

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text == "-" || ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text == "")
            {
                fgen.msg("-", "AMSG", "Please Enter Time Out (Hrs) At Line No. " + sg1.Rows[i].Cells[12].Text.Trim() + "");
                return;
            }

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text == "-" || ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text == "" || ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text == "0")
            {
                fgen.msg("-", "AMSG", "Please Enter only 1 or 2 '13' 1 for Same Day and 2 for Next Day At Line No. " + sg1.Rows[i].Cells[12].Text.Trim() + "");
                return;
            }
            else
            {
                string dttoh = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text;
                string dttom = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text;
                string dtfromh = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text;
                string dtfromm = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text;
                string DT1 = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text;
                if (DT1 == "1")
                {
                    #region
                    if (Convert.ToInt32(dtfromh) < Convert.ToInt32(dttoh))
                    {
                        fgen.msg("-", "AMSG", "Out Time Can't be less than In Time!!");
                        return;
                    }
                    else
                    {
                        if (dtfromm != "-" && dtfromm != "" && dtfromm != " ")
                        {
                            if (Convert.ToInt32(dtfromh) > 24)
                            {
                                fgen.msg("-", "AMSG", "Hours can't be more than 24!!");
                                return;
                            }
                            if (Convert.ToInt32(dttoh) > 24)
                            {
                                fgen.msg("-", "AMSG", "Hours can't be more than 24!!");
                                return;
                            }
                            if (Convert.ToInt32(dtfromh) == 24 && Convert.ToInt32(dtfromm) > 1)
                            {
                                fgen.msg("-", "AMSG", "Please enter valid Time in Time Out(Hrs)!!");
                                return;
                            }
                            if (dttom.Length < 1)
                            {
                                fgen.msg("-", "AMSG", "Please enter Value Time In (Min)!!");
                                return;
                            }
                            if (Convert.ToInt32(dttom) == 60 || Convert.ToInt32(dttom) > 60)
                            {
                                fgen.msg("-", "AMSG", "Please enter Valid Min in Time_in(Min)!!");
                                return;
                            }
                            if (Convert.ToInt32(dtfromm) == 60 || Convert.ToInt32(dtfromm) > 60)
                            {
                                fgen.msg("-", "AMSG", "Please enter Valid Min in Time_Out(Min)!!");
                                return;
                            }
                            DateTime dtFrom = DateTime.Parse(dtfromh + ":" + dtfromm);
                            DateTime dtTo = DateTime.Parse(dttoh + ":" + dttom);

                            int timeDiff = dtFrom.Subtract(dtTo).Hours;
                            int timediff2 = dtFrom.Subtract(dtTo).Minutes;

                            TextBox txtName = ((TextBox)sg1.Rows[i].FindControl("sg1_t6"));
                            txtName.Text = timeDiff.ToString();

                            TextBox txtName1 = ((TextBox)sg1.Rows[i].FindControl("sg1_t7"));
                            txtName1.Text = timediff2.ToString();
                        }
                        if (dtfromm.Length < 1)
                        {
                            fgen.msg("-", "AMSG", "Please enter Value inTime Out (Min)!!");
                            return;
                        }
                    }
                    #endregion
                }
                else if (DT1 == "2")
                {
                    #region
                    if (dtfromm != "-" && dtfromm != "" && dtfromm != " ")
                    {
                        if (Convert.ToInt32(dtfromh) > 24)
                        {
                            fgen.msg("-", "AMSG", "Hours can't be more than 24!!");
                            return;
                        }
                        if (Convert.ToInt32(dttoh) > 24)
                        {
                            fgen.msg("-", "AMSG", "Hours can't be more than 24!!");
                            return;
                        }
                        if (Convert.ToInt32(dtfromh) == 24 && Convert.ToInt32(dtfromm) > 1)
                        {
                            fgen.msg("-", "AMSG", "Please enter valid Time in Time Out(Hrs)!!");
                            return;
                        }
                        if (dttom.Length < 1)
                        {
                            fgen.msg("-", "AMSG", "Please enter Value Time In (Min)!!");
                            return;
                        }
                        if (Convert.ToInt32(dttom) == 60 || Convert.ToInt32(dttom) > 60)
                        {
                            fgen.msg("-", "AMSG", "Please enter Valid Min in Time_in(Min)!!");
                            return;
                        }
                        if (Convert.ToInt32(dtfromm) == 60 || Convert.ToInt32(dtfromm) > 60)
                        {
                            fgen.msg("-", "AMSG", "Please enter Valid Min in Time_Out(Min)!!");
                            return;
                        }
                        DateTime dtFrom = DateTime.Parse(dtfromh + ":" + dtfromm);
                        DateTime dtTo = DateTime.Parse(dttoh + ":" + dttom);

                        int timeDiff = 24 - Convert.ToInt32(dttoh) + Convert.ToInt32(dtfromh);
                        int timediff2 = Convert.ToInt32(dtfromm) - Convert.ToInt32(dttom);
                        //=======================
                        if (timeDiff < 1)
                        {
                            timeDiff = timeDiff * -1;
                        }
                        if (timediff2 < 1)
                        {
                            timediff2 = timediff2 * -1;
                        }
                        TextBox txtName = ((TextBox)sg1.Rows[i].FindControl("sg1_t6"));
                        txtName.Text = timeDiff.ToString();

                        TextBox txtName1 = ((TextBox)sg1.Rows[i].FindControl("sg1_t7"));
                        txtName1.Text = timediff2.ToString();
                    }
                    if (dtfromm.Length < 1)
                    {
                        fgen.msg("-", "AMSG", "Please enter Value in Time Out (Min)!!");
                        return;
                    }
                    #endregion
                }
            }
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
            fgen.Fn_open_sseek("Select Grade", frm_qstr);
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
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
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
                    hf2.Value = col1; //grade value
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry No to Delete", frm_qstr);
                    break;

                case "Edit":
                    if (col1 == "") return;
                    hf2.Value = col1; //grade value
                    hffield.Value = "Edit_E_";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Month", frm_qstr);
                    break;

                case "Edit_E_":
                    if (col1 == "") return;
                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry No to Edit", frm_qstr);
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
                    fgen.Fn_open_sseek("Select Empcode No to Print", frm_qstr);
                    break;

                case "List":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "List_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Empcode for View", frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "SELECT a.*,B.NAME,B.DEPTT_TEXT,B.DESG_TEXT,B.DTJOIN,C.NAME AS GRADE_NAME FROM ATTN A ,EMPMAS B,TYPE C WHERE TRIM(A.EMPCODE)=TRIM(B.EMPCODE) AND TRIM(A.GRADE)=TRIM(B.GRADE) AND TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND TRIM(A.GRADE)=TRIM(C.TYPE1) AND C.ID='I' AND  a.branchcd ='" + frm_mbr + "' and a.type='" + frm_vty + "' and trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl2.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        txtlbl3.Text = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl5.Text = dt.Rows[0]["edt_by"].ToString().Trim();
                        txtlbl6.Text = Convert.ToDateTime(dt.Rows[0]["edt_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4.Text = dt.Rows[0]["grade"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["GRADE_NAME"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["ODLCDTL"].ToString().Trim();
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
                            sg1_dr["sg1_f1"] = dt.Rows[i]["EMPCODE"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["NAME"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["DEPTT_TEXT"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["DESG_TEXT"].ToString().Trim();
                            sg1_dr["sg1_f5"] = Convert.ToDateTime(dt.Rows[i]["DTJOIN"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_t1"] = dt.Rows[i]["TIMEINHR"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["TIMEINMIN"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["TIMEOUTHR"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["TIMEOUTMIN"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["HLD_TAG"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["HRWRK"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["MINWRK"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["dt1"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["dt2"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["dt3"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["dt4"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["dt5"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["Tot_ded"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["Sunday_pay"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["tot_ot"].ToString().Trim();
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
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    hffield.Value = "Print_E";
                    hf1.Value = col1;//fstr value                    
                    break;

                case "List_E":
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    hffield.Value = "List_E";
                    hf1.Value = col1;//fstr value            
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    SQuery = "SELECT distinct vchnum  FROM attn WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' and grade='" + col1 + "' and to_char(vchdate,'dd/mm/yyyy')='" + txtvchdate.Text + "'";
                    mq0 = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "vchnum");
                    if (mq0.Length > 1)
                    {
                        fgen.msg("-", "AMSG", "Data Already Entered For This Date " + txtvchdate.Text + "");
                        return;
                    }
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    btnlbl7.Focus();
                    break;

                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col2;
                    SQuery = "select  EMPCODE AS COL1,NAME AS COL2, DEPTT_TEXT AS COL3,DESG_TEXT AS COL4,TO_CHAR(DTJOIN,'dd/MM/yyyy') AS COL6,ENT_DT,ENT_BY from empmas where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + txtlbl4.Text.Trim() + "' and branch='" + txtlbl7.Text.Trim() + "' and substr(nvl(trim(appr_by),'-'),1,3)='[A]' order by empcode";
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
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
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
                        if (col1.Length > 6) SQuery = "select * from evas where trim(userid) in (" + col1 + ")";
                        else SQuery = "select * from evas where trim(userid)='" + col1 + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[d]["userid"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[d]["username"].ToString().Trim();
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";

                            sg1_dr["sg1_f1"] = dt.Rows[d]["USERID"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["full_Name"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["username"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["contactno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["emailid"].ToString().Trim();

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
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    //********* Saving in GridView Value
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
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
                    Cal();
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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List_E" || hffield.Value == "Print_E")
        {
            if (hffield.Value == "Print_E")
            {
                PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", hf1.Value); //FSTR VALUE
                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F85102");
                fgen.fin_pay_reps(frm_qstr);
            }
            if (hffield.Value == "List_E")
            {
                PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                SQuery = "select  b.name as empname,a.empcode,b.desg_text,nvl(a.timeinhr,0) as timeinhr,nvl(a.timeinmin,0) as timeinmin,nvl(a.timeouthr,0) as timeouthr,nvl(a.timeoutmin,0) as timeoutmin,nvl(a.hrwrk,0) as hrwrk,nvl(a.minwrk,0) as minwrk,nvl(a.dt1,0) as down_Time1,nvl(a.dt2,0) as down_Time2,nvl(a.dt3,0) as down_Time3,nvl(a.dt4,0) as down_time4,nvl(a.dt5,0) as Total_Downtime  from attn a ,empmas b where trim(a.branchcd)||trim(a.grade)||trim(a.empcode) =trim(b.branchcd)||trim(b.grade)||trim(b.empcode) and trim(a.branchcd)||trim(a.grade)||trim(a.empcode)='" + hf1.Value + "' and a.vchdate " + PrdRange + " and a.type='10'";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            }
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------    
            //string last_entdt;
            //checks
            //if (edmode.Value == "Y")
            //{
            //}
            //else
            //{
            //    last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  ", "ldt");
            //    if (last_entdt == "0")
            //    { }
            //    else
            //    {
            //        if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
            //        {
            //            Checked_ok = "N";
            //            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
            //        }
            //    }
            //}

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
                                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
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
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
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

            sg1.Columns[10].HeaderStyle.Width = 30;
            sg1.Columns[11].HeaderStyle.Width = 30;
            sg1.Columns[12].HeaderStyle.Width = 50;
            sg1.Columns[13].HeaderStyle.Width = 80;
            sg1.Columns[14].HeaderStyle.Width = 150;
            sg1.Columns[15].HeaderStyle.Width = 100;
            sg1.Columns[16].HeaderStyle.Width = 100;
            sg1.Columns[17].HeaderStyle.Width = 80;
            ///==================text boxes
            sg1.Columns[18].HeaderStyle.Width = 85;
            sg1.HeaderRow.Cells[18].Text = "Time_In(Hrs)";
            sg1.Columns[19].HeaderStyle.Width = 90;
            sg1.HeaderRow.Cells[19].Text = "Time_In(Min)";
            sg1.Columns[20].HeaderStyle.Width = 100;
            sg1.HeaderRow.Cells[20].Text = "Time_Out(Hrs)";
            sg1.Columns[21].HeaderStyle.Width = 100;
            sg1.HeaderRow.Cells[21].Text = "Time_Out(Min)";
            sg1.Columns[22].HeaderStyle.Width = 50;
            sg1.HeaderRow.Cells[22].Text = "Day";
            sg1.Columns[23].HeaderStyle.Width = 100;
            sg1.HeaderRow.Cells[23].Text = "Tot_Time(Hrs)";
            sg1.Columns[24].HeaderStyle.Width = 110;
            sg1.HeaderRow.Cells[24].Text = "Tot_Time(Min)";
            sg1.Columns[25].HeaderStyle.Width = 100;
            sg1.HeaderRow.Cells[25].Text = "Fooding(Amt)";
            sg1.Columns[26].HeaderStyle.Width = 120;
            sg1.HeaderRow.Cells[26].Text = "Tot_2D_Dedn(Hrs)";
            sg1.Columns[27].HeaderStyle.Width = 150;
            sg1.HeaderRow.Cells[27].Text = "Tot_Late_Coming(Min)";
            sg1.Columns[28].HeaderStyle.Width = 90;
            sg1.HeaderRow.Cells[28].Text = "Tot_Fine";
            sg1.Columns[29].HeaderStyle.Width = 140;
            sg1.HeaderRow.Cells[29].Text = "Tot_Sleeping(Min)";
            sg1.Columns[30].HeaderStyle.Width = 120;
            sg1.HeaderRow.Cells[30].Text = "Tot_Other_Dedn";
            sg1.Columns[31].HeaderStyle.Width = 120;
            sg1.HeaderRow.Cells[31].Text = "Sunday_Payable";
            sg1.Columns[32].HeaderStyle.Width = 100;
            sg1.HeaderRow.Cells[32].Text = "Tot_OT_Hrs";
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
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Grade ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
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
        if (txtlbl4.Text.Length <= 1)
        {
            fgen.msg("-", "AMSG", "Please Select Grade First");
        }
        else
        {
            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            hffield.Value = "TICODE";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Branch ", frm_qstr);
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
        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();
                oporow["SRNO"] = i;
                oporow["grade"] = txtlbl4.Text;
                oporow["empcode"] = sg1.Rows[i].Cells[13].Text.Trim();
                oporow["timeinhr"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
                oporow["timeinmin"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text);
                oporow["timeouthr"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                oporow["timeoutmin"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text);
                oporow["HLD_TAG"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                oporow["hrwrk"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text);
                oporow["minwrk"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                oporow["dt1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);
                oporow["dt2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text);
                oporow["dt3"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text);
                oporow["dt4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text);
                oporow["dt5"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text);
                oporow["Tot_ded"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text);
                oporow["Tot_ot"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text);
                oporow["Sunday_pay"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text);
                oporow["REFR"] = 0;
                oporow["SHFTINHR"] = 0;
                oporow["SHFTINMIN"] = 0;
                oporow["EXTINHR"] = 0;
                oporow["EXTINMIN"] = 0;
                oporow["SHFTTAG"] = "-";
                oporow["LNCH_HR"] = 0;
                oporow["ODLOCN"] = "-";
                oporow["ODLCDTL"] = txtlbl7.Text.Trim();
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
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
        SQuery = "select type1 as fstr,name as grade_name,Type1 as Grade_Code from type where id='I' and type1 like '0%' order by grade_code";
    }
    //------------------------------------------------------------------------------------
    protected void txt_TextChanged(object sender, EventArgs e)
    {
        //fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);      
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        #region old logic
        string dttoh = ((TextBox)sg1.Rows[index].FindControl("sg1_t1")).Text;
        string dttom = ((TextBox)sg1.Rows[index].FindControl("sg1_t2")).Text;
        string dtfromh = ((TextBox)sg1.Rows[index].FindControl("sg1_t3")).Text;
        string dtfromm = ((TextBox)sg1.Rows[index].FindControl("sg1_t4")).Text;
        if (dtfromm != "-" && dtfromm != "" && dtfromm != " ")
        {
            DateTime dtFrom = DateTime.Parse(dtfromh + ":" + dtfromm);//old              
            DateTime dtTo = DateTime.Parse(dttoh + ":" + dttom);

            int timeDiff = dtFrom.Subtract(dtTo).Hours;
            int timediff2 = dtFrom.Subtract(dtTo).Minutes;

            TextBox txtName = ((TextBox)sg1.Rows[index].FindControl("sg1_t6"));
            txtName.Text = timeDiff.ToString();

            TextBox txtName1 = ((TextBox)sg1.Rows[index].FindControl("sg1_t7"));
            txtName1.Text = timediff2.ToString();
        }
        #endregion
    }
    //------------------------------------------------------------------------------------
    protected void sg1_t10_TextChanged(object sender, EventArgs e)
    {
    }
    //------------------------------------------------------------------------------------
    protected void sg1_t5_TextChanged(object sender, EventArgs e)
    {
        GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
        int index = gvRow.RowIndex;
        string dttoh = ((TextBox)sg1.Rows[index].FindControl("sg1_t1")).Text;
        string dttom = ((TextBox)sg1.Rows[index].FindControl("sg1_t2")).Text;
        string dtfromh = ((TextBox)sg1.Rows[index].FindControl("sg1_t3")).Text;
        string dtfromm = ((TextBox)sg1.Rows[index].FindControl("sg1_t4")).Text;
        ////

        string DT1 = ((TextBox)sg1.Rows[index].FindControl("sg1_t5")).Text;
        if (DT1 == "1")
        {
            #region
            if (Convert.ToInt32(dtfromh) < Convert.ToInt32(dttoh))
            {
                fgen.msg("-", "AMSG", "Out Time Can't be less than In Time!!");
                return;
            }
            else
            {
                if (dtfromm != "-" && dtfromm != "" && dtfromm != " ")
                {
                    if (Convert.ToInt32(dtfromh) > 24)
                    {
                        fgen.msg("-", "AMSG", "Hours can't be more than 24!!");
                        return;
                    }
                    if (Convert.ToInt32(dttoh) > 24)
                    {
                        fgen.msg("-", "AMSG", "Hours can't be more than 24!!");
                        return;
                    }
                    if (Convert.ToInt32(dtfromh) == 24 && Convert.ToInt32(dtfromm) > 1)
                    {
                        fgen.msg("-", "AMSG", "Please enter valid Time in Time Out(Hrs)!!");
                        return;
                    }
                    if (dttom.Length < 1)
                    {
                        fgen.msg("-", "AMSG", "Please enter Value Time In (Min)!!");
                        return;
                    }
                    if (Convert.ToInt32(dttom) == 60 || Convert.ToInt32(dttom) > 60)
                    {
                        fgen.msg("-", "AMSG", "Please enter Valid Min in Time_in(Min)!!");
                        return;
                    }
                    if (Convert.ToInt32(dtfromm) == 60 || Convert.ToInt32(dtfromm) > 60)
                    {
                        fgen.msg("-", "AMSG", "Please enter Valid Min in Time_Out(Min)!!");
                        return;
                    }
                    DateTime dtFrom = DateTime.Parse(dtfromh + ":" + dtfromm);
                    DateTime dtTo = DateTime.Parse(dttoh + ":" + dttom);

                    int timeDiff = dtFrom.Subtract(dtTo).Hours;
                    int timediff2 = dtFrom.Subtract(dtTo).Minutes;

                    TextBox txtName = ((TextBox)sg1.Rows[index].FindControl("sg1_t6"));
                    txtName.Text = timeDiff.ToString();

                    TextBox txtName1 = ((TextBox)sg1.Rows[index].FindControl("sg1_t7"));
                    txtName1.Text = timediff2.ToString();
                }
                if (dtfromm.Length < 1)
                {
                    fgen.msg("-", "AMSG", "Please enter Value inTime Out (Min)!!");
                    return;
                }
            }
            #endregion
        }
        else if (DT1 == "2")
        {
            #region
            if (dtfromm != "-" && dtfromm != "" && dtfromm != " ")
            {
                if (Convert.ToInt32(dtfromh) > 24)
                {
                    fgen.msg("-", "AMSG", "Hours can't be more than 24!!");
                    return;
                }
                if (Convert.ToInt32(dttoh) > 24)
                {
                    fgen.msg("-", "AMSG", "Hours can't be more than 24!!");
                    return;
                }
                if (Convert.ToInt32(dtfromh) == 24 && Convert.ToInt32(dtfromm) > 1)
                {
                    fgen.msg("-", "AMSG", "Please enter valid Time in Time Out(Hrs)!!");
                    return;
                }
                if (dttom.Length < 1)
                {
                    fgen.msg("-", "AMSG", "Please enter Value Time In (Min)!!");
                    return;
                }
                if (Convert.ToInt32(dttom) == 60 || Convert.ToInt32(dttom) > 60)
                {
                    fgen.msg("-", "AMSG", "Please enter Valid Min in Time_in(Min)!!");
                    return;
                }
                if (Convert.ToInt32(dtfromm) == 60 || Convert.ToInt32(dtfromm) > 60)
                {
                    fgen.msg("-", "AMSG", "Please enter Valid Min in Time_Out(Min)!!");
                    return;
                }
                DateTime dtFrom = DateTime.Parse(dtfromh + ":" + dtfromm);
                DateTime dtTo = DateTime.Parse(dttoh + ":" + dttom);

                int timeDiff = 24 - Convert.ToInt32(dttoh) + Convert.ToInt32(dtfromh);
                int timediff2 = Convert.ToInt32(dtfromm) - Convert.ToInt32(dttom);
                //=======================
                if (timeDiff < 1)
                {
                    timeDiff = timeDiff * -1;
                }
                if (timediff2 < 1)
                {
                    timediff2 = timediff2 * -1;
                }
                TextBox txtName = ((TextBox)sg1.Rows[index].FindControl("sg1_t6"));
                txtName.Text = timeDiff.ToString();

                TextBox txtName1 = ((TextBox)sg1.Rows[index].FindControl("sg1_t7"));
                txtName1.Text = timediff2.ToString();
            }
            if (dtfromm.Length < 1)
            {
                fgen.msg("-", "AMSG", "Please enter Value in Time Out (Min)!!");
                return;
            }
            #endregion
        }
        if (Convert.ToInt32(DT1) > 2)
        {
            fgen.msg("-", "AMSG", "Please Enter only 1 or 2 '13' 1 for Same Day and 2 for Next Day!!");
            return;
        }
    }
    //------------------------------------------------------------------------------------
    public void Cal()
    {
        SQuery = "select trim(empcode) as empcode,otafter from empmas where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' order by empcode";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        double food = 0, dedn = 0, lt_comng = 0, fine = 0, sleep = 0, tot_dedn = 0, tot_ot = 0, wrk_hr = 0, mas_othrs = 0;
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            food = 0; dedn = 0; lt_comng = 0; fine = 0; sleep = 0; tot_dedn = 0; tot_ot = 0; wrk_hr = 0; mas_othrs = 0;
            food = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim());
            dedn = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim());
            lt_comng = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim());
            fine = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim());
            sleep = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim());
            tot_dedn = food + dedn + lt_comng + fine + sleep;
            ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text = tot_dedn.ToString();
            wrk_hr = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) - fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
            mas_othrs = fgen.make_double(fgen.seek_iname_dt(dt, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "otafter"));
            if (mas_othrs > 0)
            {
                tot_ot = wrk_hr - mas_othrs;
            }
            ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text = tot_ot.ToString();
        }
    }
    //------------------------------------------------------------------------------------
}

//ALTER TABLE ATTN ADD dt1 number(5,2) DEFAULT 0 ;

//ALTER TABLE ATTN ADD dt2 number(5,2) DEFAULT 0 ;

// ALTER TABLE ATTN ADD dt3 number(5,2) DEFAULT 0 ;

// ALTER TABLE ATTN ADD dt4 number(5,2) DEFAULT 0 ;

// ALTER TABLE ATTN ADD dt5 number(5,2) DEFAULT 0 ;

//ALTER TABLE ATTN ADD Tot_ded number(5,1) DEFAULT 0 ;

//ALTER TABLE ATTN ADD Tot_OT number(5,2) DEFAULT 0 ;

//ALTER TABLE ATTN ADD Sunday_pay number(5,2) DEFAULT 0 ;

// ALTER TABLE ATTN ADD RMK VARCHAR2(200)  DEFAULT '-'; NOT REQUIRED
