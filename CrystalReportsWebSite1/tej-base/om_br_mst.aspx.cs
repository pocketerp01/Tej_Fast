using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Diagnostics;

public partial class om_br_mst : System.Web.UI.Page
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
    string html_body = "";
    string Prg_Id, lbl1a_Text, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    string a, b, c;
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
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                doc_addl.Value = "1";

                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                lblheader.Text = "Branch Master";

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
        doc_nf.Value = "type1";        
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "type";

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "B");
        typePopup = "N";

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
            case "MGRBUT":
                SQuery = "select acode as fstr,ANAME as Name,Acode as ERP_Code,Addr1 as Address,Addr2 as City from famst where substr(acode,1,2) in ('02')  order by aname";

                break;
            case "BRCDBUT":
                SQuery = SQuery = "select coded as fstr,coded as Code_No_Available,max(name) as Code_Name,(case when sum(Valu)>0 then 'Code Available' else 'Code Already Used' end) as Code_Status from (select lpad(trim(to_char(rownum,'99')),2,'0') as coded,1 as Valu,null as name from (select rowid,rownum from FIN_MSYS order by id) where rownum<100 union all select type1,-1 as coded,name from type where id='B') group by coded  order by Coded";

                break;

            case "STATBUT":
                SQuery = "select name as fstr ,name as State_Name ,type1 as code from type where id='{' order by Name";
                break;
            case "COSTBUT":
                SQuery = "Select Name,Type1 from typegrp where id='C' and length(Trim(type1))=3 order by type1 ";
                break;
            case "CTRYBUT":
                SQuery = "select name as fstr,name as Country ,type1 as code from typegrp where branchcd!='DD' and id='CN' order by name ";
                break;
            case "BNKACTBUT":
                SQuery = "Select Aname,ACode from Famst order by Acode";
                break;
            case "IVLBUT":
                SQuery = "Select Name,Type1 from typegrp where id='C' and type1 like '-%' and length(Trim(type1))=5 order by type1";
                break;
            case "CURRBUT":
                SQuery = "Select trim(Name) as fstr,trim(Name) as Currency_Name,Type1 from type where id='A' order by trim(Name)";
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":


            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":

            case "New":
                Type_Sel_query();
                break;
            case "Edit":


            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                SQuery = "select trim(type1) as fstr, Name as Branch_Name,addr as Address_Line1,addr1 as Address_line2,Gst_no,type1 as Branch_code from type where id='B' and trim(type1)='"+frm_mbr+"' ";
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
            hffield.Value = "New";
            fgen.open_pwdbox("-", frm_qstr);
            return;

        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    void newCase(string vty)
    {
        #region
        //if (col1 == "") return;
        frm_vty = vty;

        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE  id='B' ", 2, "VCH");
        txt_brnchcd.Value = frm_vnum;

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


        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;

        // still implement overseas functionality w0050

        if (txt_plnt_name.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Plant Name";
        }
        if (txt_stat_name.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "StateName";
        }
        if (txt_ctry_name.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Country";
        }

        if (txt_curren.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Currency";
        }


        if (txt_pan_no.Value.Trim().Length < 2)
        {
            
            if (txt_ctry_name.Value == "INDIA")
            {
                reqd_nc = reqd_nc + 1;
                reqd_flds = reqd_flds + " / " + "PAN no.";
            }

        }

        if (txt_gst_no.Value.Trim().Length < 2)
        {
            
            if (txt_ctry_name.Value=="INDIA")
            {
                reqd_nc = reqd_nc + 1;
                reqd_flds = reqd_flds + " / " + "GST no."; 
            }

        }
        if ( txt_acode.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Acode";

        }
        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }

        if (txt_gst_no.Value.Trim().Length < 15 && txt_ctry_name.Value == "INDIA")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "GSTNO.should be in 15 length");
            txt_gst_no.Focus();
            return;

        }
        if (txt_pan_no.Value.Trim().Length < 10 && txt_ctry_name.Value == "INDIA")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PANNO.should be in 10 length");
            txt_pan_no.Focus();
            return;

        }

        if (txt_ctry_name.Value == "INDIA")
        {
            #region validation of pan card
            if ((txt_pan_no.Value.Trim().Length == 10))
            {

                char[] str = txt_pan_no.Value.Trim().Substring(0, 5).ToCharArray();

                for (int i = 0; i < 5; i++)
                {
                    if (str[i] >= 65 && str[i] <= 90)
                    {


                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PANNo. is not appropriate");
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

                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PANNo. is not appropriate");
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

                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "PANNo. is not appropriate");
                        txt_pan_no.Focus();
                        return;
                    }
                }



            }
            #endregion

            #region validation of gst
            if ((txt_gst_no.Value.Trim().Length == 15))
            {

                char[] str = txt_gst_no.Value.Trim().Substring(0, 2).ToCharArray();

                for (int i = 0; i < 2; i++)
                {
                    if (str[i] >= 48 && str[i] <= 57)
                    {


                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "GSTNO. is not appropriate");
                        txt_pan_no.Focus();
                        return;
                    }
                }

                char[] str1 = txt_gst_no.Value.Trim().Substring(12, 3).ToCharArray();

                for (int i = 0; i < 3; i++)
                {
                    if (str1[i] >= 48 || str1[i] <= 57 || str1[i] >= 65 || str1[i] <= 90)
                    {


                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "GSTNO. is not appropriate");
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
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + "GST/PANNo. is not appropriate");
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
        //PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        string headerN = "";
        SQuery = "SELECT '-' as fstr,'-' as gstr,a.Name as Branch_Name,a.addr as Address_Line1,a.addr1 as Address_line2,a.Gst_no,a.Gir_num as Pan_no,a.Tele,a.email,a.website,a.statenm,a.raddr,a.raddr1,a.Acode,a.type1 as Branch_code FROM " + frm_tabname + " a where a.id='B' order by a.type1 ";
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        //fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " ", frm_qstr);

        //SQuery = "select '-' as fstr,'-' as gstr,b.Iname,b.Cpartno,sum(a.opening) as Opening_Stock,sum(a.cdr) as Receipts,sum(a.ccr) as Issues,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stock,b.Unit,TRIM(A.ICODE) AS ERP_Code,max(a.imin) as Min_lvl,max(a.imax) as Max_lvl,max(a.iord) as ReOrder_lvl  from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where BRANCHCD='" + frm_mbr + "' and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,(nvl(iqtyin,0))-(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr,0 as imin,0 as imax,0 as iord FROM IVOUCHER where BRANCHCD='" + frm_mbr + "' and TYPE LIKE '%' AND VCHDATE " + xprd1 + " and store='Y' union all select branchcd,trim(icode) as icode,0 as op,(nvl(iqtyin,0)) as cdr,(nvl(iqtyout,0)) as ccr,0 as imin,0 as imax,0 as iord from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%'  AND VCHDATE " + xprd2 + " and store='Y') a,item b where trim(A.icode)=trim(B.icode) and a.icode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' and substr(A.icode,1,1)<'8' GROUP BY b.Iname,b.Cpartno,b.Unit,TRIM(A.ICODE) ORDER BY trim(a.ICODE)";
        
        headerN = "List of " + lblheader.Text.Trim() + " ";
        fgen.drillQuery(0, SQuery, frm_qstr, "1#", "3#4#5#6#7#8#", "350#150#150#150#150#150#");
        fgen.Fn_DrillReport(headerN, frm_qstr);

        hffield.Value = "-";
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

                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where id='B' and  type1='"+ fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                // Saving Deleting History
                //public void save_info(string Qstr, string pco_Cd, string mbr, string zvnum, string zvdate, string zuser, string ztype, string zremark)
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, frm_mbr  , vardate  , frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
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
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_CONFIRM") == "2")
                    {
                        newCase(col1);
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Create New Branch !!");
                        return;
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
                    fgen.Fn_open_sseek("Select Branch to Edit", frm_qstr);
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
                    string mv_col;
                    mv_col = col1;
                    SQuery = "Select a.* from " + frm_tabname + " a where id='B' and TRIM(A.TYPE1)='" + mv_col + "' ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txt_plnt_name.Value = dt.Rows[i]["Name"].ToString();
                        txt_acode.Value = dt.Rows[i]["ACODE"].ToString();
                        Txt_ivl.Value = dt.Rows[i]["Rate"].ToString();
                        txt_bank_iec.Value = dt.Rows[i]["STForm"].ToString();
                        
                        txt_addr_1.Value = dt.Rows[i]["Addr"].ToString();
                        txt_addr_2.Value = dt.Rows[i]["addr1"].ToString();
                        txt_addr_3.Value = dt.Rows[i]["addr2"].ToString();

                        txt_cit_name.Value = dt.Rows[i]["place"].ToString();
                        txt_stat_name.Value = dt.Rows[i]["statenm"].ToString();
                        txt_ctry_name.Value = dt.Rows[i]["countrynm"].ToString();
                        
                        txt_tel_no.Value = dt.Rows[i]["tele"].ToString();
                        txt_fax_no.Value = dt.Rows[i]["fax"].ToString().Trim();

                        txt_ecc_num.Value = dt.Rows[i]["ec_code"].ToString().Trim();
                        
                        txt_pla_no.Value = dt.Rows[i]["exc_pla"].ToString().Trim();
                        txt_exc_regn_no.Value = dt.Rows[i]["exc_regn"].ToString().Trim();
                        
                        txt_gst_no.Value = dt.Rows[i]["gst_no"].ToString().Trim();
                        txt_pan_no.Value = dt.Rows[i]["gir_num"].ToString().Trim();
                        txt_tin_no.Value = dt.Rows[i]["rcnum"].ToString().Trim();
                        txt_pf_no.Value = dt.Rows[i]["audit_"].ToString().Trim();
                        txt_msme_no.Value = dt.Rows[i]["msme_no"].ToString().Trim();
                        
                        
                        txt_brnchcd.Value = dt.Rows[i]["type1"].ToString().Trim();
                        txt_tax_add1.Value = dt.Rows[i]["st_sc"].ToString().Trim();
                        txt_wtm_cons.Value = dt.Rows[i]["st_tt"].ToString().Trim();

                        txt_wtm_purch.Value = dt.Rows[i]["balop"].ToString().Trim();
                        txt_wtm_cons.Value = dt.Rows[i]["balcb"].ToString().Trim();
                        txt_zipcode.Value = dt.Rows[i]["zipcode"].ToString().Trim();
                        txt_mail_1.Value = dt.Rows[i]["email"].ToString().Trim();

                        txt_web_site.Value = dt.Rows[i]["website"].ToString().Trim();
                        txt_cst_no.Value = dt.Rows[i]["cstno"].ToString().Trim();
                        txt_cst_dt.Value = dt.Rows[i]["cstdt"].ToString().Trim();
                        txt_regd_office.Value = dt.Rows[i]["raddr"].ToString().Trim();
                        
                        txt_regd_office1.Value = dt.Rows[i]["raddr1"].ToString().Trim();
                        txt_gst_no.Value = dt.Rows[i]["gst_no"].ToString().Trim();
                        txt_prefix_po.Value = dt.Rows[i]["poprefix"].ToString().Trim();
                        txt_bank_name.Value = dt.Rows[i]["BANKNAME"].ToString().Trim();
                        
                        txt_head_off.Value = dt.Rows[i]["haddr"].ToString().Trim();
                        txt_bank_addr.Value = dt.Rows[i]["BANKADDR"].ToString().Trim();
                        txt_bank_addr1.Value = dt.Rows[i]["BANKADDR1"].ToString().Trim();
                        txt_bank_acno.Value = dt.Rows[i]["BANKAC"].ToString().Trim();
                        
                        txt_email1.Value = dt.Rows[i]["EMAIL1"].ToString().Trim();
                        txt_email2.Value = dt.Rows[i]["EMAIL2"].ToString().Trim();
                        txt_email3.Value = dt.Rows[i]["EMAIL3"].ToString().Trim();
                        txt_email4.Value = dt.Rows[i]["EMAIL4"].ToString().Trim();

                        txt_bankpf.Value = dt.Rows[i]["bank_pf"].ToString().Trim();
                        
                        txt_cin.Value = dt.Rows[i]["co_cin"].ToString().Trim();

                        txt_act_code.Value = dt.Rows[i]["exc_item"].ToString().Trim();
                        txt_serv_no.Value = dt.Rows[i]["exc_tarrif"].ToString().Trim();
                        txt_address.Value = dt.Rows[i]["exc_addr"].ToString().Trim(); 
                        
                        //txt_range.Value= dt.Rows[i]["exc_rang"].ToString().Trim();
                        //txt_rate.Value = dt.Rows[i]["cexc_comm"].ToString().Trim();
                        //txt_divi.Value = dt.Rows[i]["exc_div"].ToString().Trim();


                        txt_curren.Value = dt.Rows[i]["br_Curren"].ToString().Trim();
                        txt_less_1.Value = dt.Rows[i]["exc_rang"].ToString().Trim();

                        txt_fmt_1.Value = dt.Rows[i]["num_fmt1"].ToString().Trim();
                        txt_fmt_2.Value = dt.Rows[i]["num_fmt2"].ToString().Trim();


                        txt_bank_rtgs.Value= dt.Rows[i]["ifsc_code"].ToString().Trim(); 
                        txt_lutno.Value=dt.Rows[i]["bond_ut"].ToString().Trim(); 
                        txt_bank_swift.Value= dt.Rows[i]["vat_form"].ToString().Trim(); 
                        txt_mfg_no.Value= dt.Rows[i]["mfg_licno"].ToString().Trim(); 
                        Txt_invoice.Value= dt.Rows[i]["iprefix"].ToString().Trim(); 
                        txt_brnch.Value= dt.Rows[i]["vchnum"].ToString().Trim(); 
                        Txt_ro_phone.Value= dt.Rows[i]["rphone"].ToString().Trim(); 
                        Txt_head_off1.Value= dt.Rows[i]["haddr1"].ToString().Trim(); 
                        txt_phone_no.Value= dt.Rows[i]["hphone"].ToString().Trim(); 
                        txt_esi.Value= dt.Rows[i]["esirate"].ToString().Trim(); 
                        txt_plant_capacity.Value= dt.Rows[i]["balop"].ToString().Trim(); 
                        txt_gst_aspip.Value= dt.Rows[i]["gstasp_ip"].ToString().Trim(); 
                        txt_gst_ewbuid.Value= dt.Rows[i]["gstewb_id"].ToString().Trim(); 
                        txt_ewbpwd.Value= dt.Rows[i]["gstewb_pw"].ToString().Trim(); 
                        txt_ef_uname.Value= dt.Rows[i]["gstefu_id"].ToString().Trim(); 
                        txt_ef_pwd.Value= dt.Rows[i]["gstefu_pw"].ToString().Trim(); 
                        txt_cd_key.Value= dt.Rows[i]["gstefu_cdkey"].ToString().Trim();
                        txt_gst_api.Value = dt.Rows[i]["gst_apiadd"].ToString().Trim();
                        Txt_send_mail.Value = dt.Rows[i]["mail_fld1"].ToString().Trim(); 
                        Txt_send_pwd.Value= dt.Rows[i]["mail_fld2"].ToString().Trim(); 
                        Txt_send_port.Value= dt.Rows[i]["mail_fld4"] .ToString().Trim(); 
                        Txt_smtp.Value= dt.Rows[i]["mail_fld3"].ToString().Trim(); 
                        Txt_ssl.Value= dt.Rows[i]["mail_fld5"].ToString().Trim(); 
                        Txt_cc_to.Value= dt.Rows[i]["mail_fld6"].ToString().Trim(); 
                        txt_tcs_no.Value= dt.Rows[i]["tcs_num"].ToString().Trim(); 
                        txt_esi_regn.Value= dt.Rows[i]["status"].ToString().Trim();
                        if (dt.Rows[0]["rcdate"].ToString().Trim() == "" || dt.Rows[0]["rcdate"].ToString().Trim() == "-") 
                        {
                            Txt_tin_dt.Value = "-";
                        }
                        else
                        {
                            Txt_tin_dt.Value = Convert.ToDateTime(dt.Rows[0]["rcdate"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }
                        if (dt.Rows[0]["cstdt"].ToString().Trim() == "" || dt.Rows[0]["cstdt"].ToString().Trim() == "-")
                        {
                            txt_cst_dt.Value ="-";
                        }
                        else
                        {
                            txt_cst_dt.Value = Convert.ToDateTime(dt.Rows[0]["cstdt"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }
                        if (dt.Rows[0]["wipstdt"].ToString().Trim() == "" || dt.Rows[0]["wipstdt"].ToString().Trim() == "-")
                        {
                            txt_wip_srtdate.Value = "-";
                        }
                        else
                        {
                            txt_wip_srtdate.Value = Convert.ToDateTime(dt.Rows[0]["wipstdt"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }
                        if (dt.Rows[0]["lotstkdt"].ToString().Trim() == "" || dt.Rows[0]["lotstkdt"].ToString().Trim() == "-")
                        {
                            txt_lotwise_stkdt.Value = "-";
                        }
                        else
                        {
                            txt_lotwise_stkdt.Value = Convert.ToDateTime(dt.Rows[0]["lotstkdt"].ToString().Trim()).ToString("yyyy-MM-dd");
                        }

                        txt_estab_code.Value = dt.Rows[i]["est_code"].ToString().Trim(); 
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                        if (lblUpload.Text.Length > 1) btnDwnld1.Visible = true;
                    }
                    #endregion
                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;
                case "ACNBUT":
                    if (col1.Length <= 0) return;
                    break;

                case "MGRBUT":
                    if (col1.Length <= 0) return;
                    txt_acode.Value = col1;
                    break;
                case "BRCDBUT":
                    if (col1.Length <= 0) return;

                    txt_brnchcd.Value = col1;
                    break;

                case "BNKACTBUT":
                    if (col1.Length <= 0) return;
                    txt_bank_acc.Value = col1.Trim();
                    //btnlbl7.Focus();
                    break;

                case "STATBUT":
                    if (col1.Length <= 0) return;
                    txt_stat_name.Value = col1.Trim();
                    break;

                case "COSTBUT":
                    if (col1.Length <= 0) return;
                    txt_cost_center.Value = col1.Trim();
                    break;

                case "CTRYBUT":
                    if (col1.Length <= 0) return;
                    txt_ctry_name.Value = col1.Trim();
                    break;

                case "IVLBUT":
                    if (col1.Length <= 0) return;
                    Txt_ivl.Value = col1.Trim();
                    break;
                case "CURRBUT":
                    if (col1.Length <= 0) return;
                    txt_curren.Value = col1.Trim();
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

                        if (edmode.Value == "Y")
                        {
                            //  fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set id='B'||trim(replace(id,'B','')) where trim(" + doc_nf.Value + ")='" + ddl_fld2 + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set tbranchcd='DD' where trim(TYPE1)='" + ddl_fld1 + "' AND Id='B'");
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);


                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");

                            fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txt_brnchcd.Value.ToUpper().Trim(), frm_uname, edmode.Value);

                            fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM  " + frm_tabname + " where tbranchcd='DD' and id='B' and trim(TYPE1)='" + ddl_fld1 + "'");
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //html_body = html_body + "Please note your CSS No : " + frm_vnum + "<br>";
                                //html_body = html_body + "Tejaxo ERP Customer Support Team Will analyse the same within next 2-3 working days.<br>";
                                //html_body = html_body + "You can track Progress on your service request through CSS status also.<br>";
                                //html_body = html_body + "Always at your service, <br>";
                                //html_body = html_body + "Finsys support <br>";

                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", txtlbl5.Value, "", "", "CSS : Query has been logged " + frm_vnum, html_body);

                                fgen.msg("-", "AMSG", "New Branch has been Opened , Please Fill All Details Responsibly");


                                fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txt_brnchcd.Value.ToUpper().Trim(), frm_uname, edmode.Value);
                                
                                //updateMessage();

                                // string str = @"C:\Users\admin\Documents\Visual Studio 2013\Projects\ConsoleApplication1\bin\Debug\Console Application.exe";
                                //Process process = new Process();
                                // process.StartInfo.FileName = str;
                                // process.Start();
                                // Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Call();", true);
                                // Response.Redirect("https://api.whatsapp.com/send?phone=919958177242&text=you have successfully open a new branch");




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
        oporow["Id"] = "B";
        oporow["Type1"] = txt_brnchcd.Value.ToUpper().Trim();
        oporow["Name"] = txt_plnt_name.Value.ToUpper().Trim();
        oporow["ACODE"] = txt_acode.Value.ToUpper().Trim();
        oporow["Rate"] = fgen.make_double(Txt_ivl.Value.ToUpper().Trim());
        oporow["STForm"] = txt_bank_iec.Value.ToUpper().Trim();

        oporow["Addr"] = txt_addr_1.Value.ToUpper().Trim();
        oporow["addr1"] = txt_addr_2.Value.ToUpper().Trim();
        oporow["addr2"] = txt_addr_3.Value.ToUpper().Trim();

        oporow["place"] = txt_cit_name.Value.ToUpper().Trim();
        oporow["statenm"] = txt_stat_name.Value.ToUpper().Trim();
        oporow["countrynm"] = txt_ctry_name.Value.ToUpper().Trim();
        oporow["tele"] = txt_tel_no.Value.ToUpper().Trim();
        oporow["fax"] = txt_fax_no.Value.ToUpper().Trim();
        oporow["exc_regn"] = txt_exc_regn_no.Value.ToUpper().Trim();

        

        oporow["exc_rang"] = "-";
        oporow["exc_div"] = "-";
        oporow["cexc_comm"] = "-";

        oporow["br_Curren"] = txt_curren.Value.ToUpper().Trim(); 
        oporow["exc_rang"] = txt_less_1.Value.ToUpper().Trim(); 
        oporow["num_fmt1"] = txt_fmt_1.Value.ToUpper().Trim();
        oporow["num_fmt2"] = txt_fmt_2.Value.ToUpper().Trim();

        oporow["exc_pla"] = txt_pla_no.Value.ToUpper().Trim();
        oporow["ec_code"] = txt_ecc_num.Value.ToUpper().Trim();
        oporow["gir_num"] = txt_pan_no.Value.ToUpper().Trim();
        oporow["rcnum"] = txt_tin_no.Value.ToUpper().Trim();
        oporow["audit_"] = txt_pf_no.Value.ToUpper().Trim();
        oporow["msme_no"] = txt_msme_no.Value.ToUpper().Trim();
        oporow["exc_item"] = txt_act_code.Value.ToUpper().Trim();
        oporow["exc_tarrif"] = txt_serv_no.Value.ToUpper().Trim();
        oporow["exc_addr"] = txt_address.Value.ToUpper().Trim();
        
        
        oporow["notification"] = "-";

        oporow["st_sc"] = fgen.make_double(txt_tax_add1.Value.ToUpper().Trim());
        oporow["st_tt"] = fgen.make_double(txt_wtm_cons.Value.ToUpper().Trim());
        oporow["balop"] = fgen.make_double(txt_wtm_purch.Value.ToUpper().Trim());
        oporow["balcb"] = fgen.make_double(txt_wtm_cons.Value.ToUpper().Trim());
        oporow["zipcode"] = txt_zipcode.Value.ToUpper().Trim();
        oporow["email"] = txt_mail_1.Value.Trim();
        oporow["website"] = txt_web_site.Value.Trim();
        oporow["cstno"] = txt_cst_no.Value.ToUpper().Trim();

        if (txt_cst_dt.Value.Length > 1)
        {
            oporow["cstdt"] = Convert.ToDateTime(txt_cst_dt.Value.Trim()).ToString("dd/MM/yyyy");
        }
        else
        {
            oporow["cstdt"] = DBNull.Value;
        }

        if(Txt_tin_dt.Value.Length> 1)
        {
            oporow["rcdate"] = Convert.ToDateTime(Txt_tin_dt.Value.Trim()).ToString("dd/MM/yyyy");
        }
        else
        {
            oporow["rcdate"] = DBNull.Value;
        }

        if (txt_wip_srtdate.Value.Length > 1)
        {
            oporow["wipstdt"] = Convert.ToDateTime(txt_wip_srtdate.Value.Trim()).ToString("dd/MM/yyyy");
        }
        else
        {
            oporow["wipstdt"] ="-";
        }

        if (txt_lotwise_stkdt.Value.Length > 1)
        {
            oporow["lotstkdt"] = Convert.ToDateTime(txt_lotwise_stkdt.Value.Trim()).ToString("dd/MM/yyyy");
        }
        else
        {
            oporow["lotstkdt"] = "-";
        }

        oporow["raddr"] = txt_regd_office.Value.ToUpper().Trim();
        oporow["raddr1"] = txt_regd_office1.Value.ToUpper().Trim();
        oporow["gst_no"] = txt_gst_no.Value.ToUpper().Trim();
        oporow["poprefix"] = txt_prefix_po.Value.ToUpper().Trim();
        oporow["BANKNAME"] = txt_bank_name.Value.ToUpper().Trim();
        oporow["haddr"] = txt_head_off.Value.ToUpper().Trim();
        oporow["BANKADDR"] = txt_bank_addr.Value.ToUpper().Trim();
        oporow["BANKADDR1"] = txt_bank_addr1.Value.ToUpper().Trim();
        oporow["ifsc_code"] = txt_bank_rtgs.Value.ToUpper().Trim();
        oporow["bond_ut"] = txt_lutno.Value.ToUpper().Trim();
        oporow["vat_form"] = txt_bank_swift.Value.ToUpper().Trim();
        oporow["mfg_licno"] = txt_mfg_no.Value.ToUpper().Trim();
        oporow["iprefix"] = Txt_invoice.Value.ToUpper().Trim();
        oporow["vchnum"] = txt_brnch.Value.ToUpper().Trim();
        oporow["rphone"] = Txt_ro_phone.Value.ToUpper().Trim();
        oporow["haddr1"] = Txt_head_off1.Value.ToUpper().Trim();
        oporow["hphone"] = txt_phone_no.Value.ToUpper().Trim();
        oporow["esirate"] = fgen.make_double(txt_esi.Value.Trim());
        oporow["balop"] = fgen.make_double(txt_plant_capacity.Value.Trim());
        oporow["gstasp_ip"] = txt_gst_aspip.Value.ToUpper().Trim();
        oporow["gstewb_id"] = txt_gst_ewbuid.Value.ToUpper().Trim();
        oporow["gstewb_pw"] = txt_ewbpwd.Value.Trim();
        oporow["gstefu_id"] = txt_ef_uname.Value.ToUpper().Trim();
        oporow["gstefu_pw"] = txt_ef_pwd.Value.Trim();
        oporow["gstefu_cdkey"] = txt_cd_key.Value.ToUpper().Trim();
        oporow["gst_apiadd"] = txt_gst_api.Value.ToUpper().Trim();
        oporow["mail_fld1"] = Txt_send_mail.Value.ToUpper().Trim();
        oporow["mail_fld2"] = Txt_send_pwd.Value.ToUpper().Trim();
        oporow["mail_fld4"] = Txt_send_port.Value.ToUpper().Trim();
        oporow["mail_fld3"] = Txt_smtp.Value.ToUpper().Trim();
        oporow["mail_fld5"] = Txt_ssl.Value.ToUpper().Trim();
        oporow["mail_fld6"] = Txt_cc_to.Value.ToUpper().Trim();
        oporow["tcs_num"] = txt_tcs_no.Value.ToUpper().Trim();
        oporow["status"] = txt_esi_regn.Value.ToUpper().Trim();
        oporow["est_code"] = txt_estab_code.Value.ToUpper().Trim();

        oporow["BANKAC"] = txt_bank_acno.Value.ToUpper().Trim();
        oporow["EMAIL1"] = txt_email1.Value.Trim();
        oporow["email2"] = txt_email2.Value.Trim();
        oporow["email3"] = txt_email3.Value.Trim();
        oporow["email4"] = txt_email4.Value.Trim();
        oporow["bank_pf"] = txt_bankpf.Value.ToUpper().Trim();
        oporow["ent_by"] = frm_uname;
        oporow["ent_dt"] = vardate;
        oporow["co_cin"] = txt_cin.Value.ToUpper().Trim();
        oporow["tbranchcd"] = txt_brnchcd.Value.ToUpper().Trim();
        oporow["tvchdate"] = vardate;

        string op_bal_fld;
        op_bal_fld = "YR_" + frm_CDT1.Substring(6, 4);
        string chk_code;

        if (txtAttch.Text.Length > 1)
        {
            oporow["fimglink"] = lblUpload.Text.Trim();
        }

        if (edmode.Value == "Y")
        {
            oporow["eNt_by"] = ViewState["entby"].ToString();
            oporow["eNt_dt"] = ViewState["entdt"].ToString();
            oporow["ent_by"] = frm_uname;
            oporow["ent_dt"] = vardate;
        }
        else
        {
            oporow["eNt_by"] = frm_uname;
            oporow["eNt_dt"] = vardate;
            oporow["ent_by"] = frm_uname;
            oporow["ent_dt"] = vardate;
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
            //filepath = filepath + txtlbl4.Value.Trim() + "_" + txtvchnum.Value.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
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
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "c:/tej_erp/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
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

    protected void btn_mgr_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MGRBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Account Code", frm_qstr);
    }
    protected void btn_brcd_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BRCDBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Branch Code", frm_qstr);
    }

    protected void btn_costcent_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "COSTBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Cost center code", frm_qstr);
    }
    protected void btn_stat_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "STATBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select State", frm_qstr);
    }
    protected void btn_bnkacct_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BNKACTBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select  Bank account", frm_qstr);
    }
    protected void btn_ctry_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CTRYBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Country", frm_qstr);
    }

    protected void btn_ivl_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "IVLBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select level cost center cost", frm_qstr);
    }

    protected void btn_curr_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CURRBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Currency", frm_qstr);
    }

    public void updateMessage()
    {
        string str;
        str = "insert into message(sno,message,flag,PHNO)values('1','you have open a new branch','N','" + txt_phone_no.Value + "')";
        fgen.execute_cmd(frm_qstr, frm_cocd, str);


    }
}