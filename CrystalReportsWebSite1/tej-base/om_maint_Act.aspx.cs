using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_maint_Act : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";

    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it, mq0, mq1 = "", mq2 = "";
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1; string rate = "0";
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
                doc_addl.Value = "-";
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
                ((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("readonly", "readonly");
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
        tab2.Visible = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false;
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
        btnprint.Disabled = true; btnlist.Disabled = true;
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
        if (frm_formID == "F75151")
        {
            doc_nf.Value = "vchnum";
            doc_df.Value = "vchdate";
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            frm_tabname = "WB_MAINT";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MM04");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
            lbl20.Text = "This Is Preventive Maintenance Done Record";
            btnprint.Visible = false;
        }
        else if (frm_formID == "F75156")
        {
            doc_nf.Value = "vchnum";
            doc_df.Value = "vchdate";
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            frm_tabname = "WB_MAINT";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MM05");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
            lbl20.Text = "This Is Health Maintenance Done Record";
            btnprint.Visible = false;
        }
        else if (frm_formID == "F75106")
        {
            doc_nf.Value = "vchnum";
            doc_df.Value = "vchdate";
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            frm_tabname = "PMAINT";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "20");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        }
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
                //if (frm_formID == "F75151")
                //{
                //    SQuery = "select a.col1,b.acref as code,b.name,trim(a.col1) as mould_code,a.fstr from(select trim(col1) as col1, branchcd||trim(col1)||TO_CHAR(DATE1,'MM/YYYY') as fstr,1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM02' union all select trim(col1) as col1,branchcd||trim(col1)||TO_CHAR(DATE2,'MM/YYYY') as fstr, -1 as qty from wb_maint  where branchcd='" + frm_mbr + "' and type='MM04')a,typegrp b where trim(a.col1)=trim(b.type1) and b.branchcd='" + frm_mbr + "' and b.id='MM' group by a.col1,a.fstr,b.name,b.acref having sum(qty)>0";
                //}
                //else if (frm_formID == "F75156")
                //{
                //    SQuery = "select a.col1,b.acref as code,b.name,trim(a.col1) as mould_code,a.fstr from(select trim(col1) as col1, branchcd||trim(col1)||TO_CHAR(DATE1,'MM/YYYY') as fstr,1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM03' union all select trim(col1) as col1,branchcd||trim(col1)||TO_CHAR(DATE2,'MM/YYYY') as fstr, -1 as qty from wb_maint  where branchcd='" + frm_mbr + "' and type='MM05')a,typegrp b where trim(a.col1)=trim(b.type1) and b.branchcd='" + frm_mbr + "' and b.id='MM' group by a.col1,a.fstr,b.name,b.acref having sum(qty)>0";
                //}
                if (frm_formID == "F75151")
                {
                    SQuery = "select a.col1,b.acref as code,b.name,trim(a.col1) as mould_code,a.fstr from(select trim(col1) as col1, branchcd||trim(col1)||TO_CHAR(DATE1,'MM/YYYY') as fstr,1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM02' and TRIM(OBSV2)='" + Convert.ToDateTime(txtlbl7.Text).ToString("dd/MM/yyyy").Substring(3, 7) + "' union all select trim(col1) as col1,branchcd||trim(col1)||TO_CHAR(DATE2,'MM/YYYY') as fstr, -1 as qty from wb_maint  where branchcd='" + frm_mbr + "' and type='MM04' and TRIM(OBSV2)='" + Convert.ToDateTime(txtlbl7.Text).ToString("dd/MM/yyyy").Substring(3, 7) + "')a,typegrp b where trim(a.col1)=trim(b.type1) and b.branchcd='" + frm_mbr + "' and b.id='MM' group by a.col1,a.fstr,b.name,b.acref having sum(qty)>0";
                }
                else if (frm_formID == "F75156")
                {
                    SQuery = "select a.col1,b.acref as code,b.name,trim(a.col1) as mould_code,a.fstr from(select trim(col1) as col1, branchcd||trim(col1)||TO_CHAR(DATE1,'MM/YYYY') as fstr,1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM03' and TRIM(OBSV2)='" + Convert.ToDateTime(txtlbl7.Text).ToString("dd/MM/yyyy").Substring(3, 7) + "' union all select trim(col1) as col1,branchcd||trim(col1)||TO_CHAR(DATE2,'MM/YYYY') as fstr, -1 as qty from wb_maint  where branchcd='" + frm_mbr + "' and type='MM05' and TRIM(OBSV2)='" + Convert.ToDateTime(txtlbl7.Text).ToString("dd/MM/yyyy").Substring(3, 7) + "')a,typegrp b where trim(a.col1)=trim(b.type1) and b.branchcd='" + frm_mbr + "' and b.id='MM' group by a.col1,a.fstr,b.name,b.acref having sum(qty)>0";
                }
                else
                {
                    SQuery = "Select type1 as fstr, type1,name from type where id=':' and type1>'10' order by type1";
                }
                break;

            case "TICODE":
                //pop2
                SQuery = "SELECT Type1 AS FSTR,NAME AS Deptt,Type1 AS CODE FROM type where id='M' and type1 like '6%' order by Name";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[13].Text.Trim().Length > 1)
                    {
                        if (col1.Length > 0) col1 = col1 + "," + "'" + gr.Cells[13].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                    }
                }
                if (frm_formID == "F75151" || frm_formID == "F75156") // mould maint done
                {
                    if (col1.Length > 0)
                    {
                        col1 = " and trim(icode) not in (" + col1 + ")";
                    }
                    else
                    {
                        col1 = "";
                    }
                    SQuery = "select trim(icode) as fstr,icode as item_code,iname as item_name,(case when nvl(iqd,0)=0 then irate else iqd end) as rate,unit from item where substr(trim(icode),1,2)>='30' and substr(trim(icode),1,2)<='60' and length(trim(icode))>4 " + col1 + "  order by Iname";
                }
                else  // maint Done
                {
                    if (col1.Length > 0)
                    {
                        col1 = " and trim(type1) not in (" + col1 + ")";
                    }
                    else
                    {
                        col1 = "";
                    }
                    SQuery = "select DISTINCT trim(mch_code) as fstr,trim(Machine_Name) as Machine_Name,trim(Mch_Code) as Mch_Code,trim(Inst_no) as Inst_no,trim(Plan_date) as Plan_date,sum(Mcnt) as Mcnt  from (select acode,vchnum,vchdate,mchname as Machine_Name,mchcode as Mch_Code,'-' as Inst_no,maintdt as Plan_date,1 as Mcnt  from pmaint where branchcd='00' and type='66' and vchdate between to_date('01/04/2018','dd/mm/yyyy') and to_date('31/03/2019','dd/mm/yyyy') union all select acode,vchnum,vchdate,mchname as Machine_Name,mchcode as Mch_Code,'-' as Inst_no,spec4 as Plan_date,-1 as sss from pmaint where branchcd='00' and type='20' and vchdate between to_date('01/04/2018','dd/mm/yyyy') and to_date('31/03/2019','dd/mm/yyyy')) where acode='" + txtlbl4.Text + "' group by trim(Machine_Name),trim(Mch_Code),trim(Inst_no),trim(Plan_date) having sum(Mcnt)=1 order by trim(Machine_Name),trim(Mch_Code),trim(Inst_no),trim(Plan_date)";
                }
                break;

            case "SG1_ADD_MAC":
                SQuery = "select trim(acode)||'/'||trim(srno) as fstr,mchname as Machine_Name,trim(acode)||'/'||trim(srno) as Machine_Code,mch_seq from pmaint where branchcd='" + frm_mbr + "' and type='10' order by acode,srno";
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "Print_E":
                fgen.Fn_open_prddmp1("Select DateRange", frm_qstr);
                //SQuery = "select distinct a.branchcd||a.type||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as Mapping_no,to_char(a.vchdate,'dd/mm/yyyy') as Map_Dt,b.IName as Product_Name,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,Item b where trim(A.icode)=trim(B.Icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' order by vdd desc,a.vchnum desc";
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    if (frm_formID == "F75151" || frm_formID == "F75156") // MOULD MAINT DONE
                    {
                        SQuery = "SELECT distinct TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS FSTR,TRIM(a.VCHNUM) AS Entry_no,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS Entry_dt,b.Acref as Mould_code,b.name as mould_name,a.Ent_by,a.Ent_dt,TO_CHAR(a.VCHDATE,'YYYYMMDD') AS VDD,TRIM(a.col1) AS CODE,a.TYPE FROM " + frm_tabname + " a,typegrp b WHERE trim(a.col1)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and b.id='MM' and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE ='" + frm_vty + "' AND a.VCHDATE " + DateRange + " ORDER BY vdd desc,TRIM(a.VCHNUM) desc";
                    }
                    else
                    {   // maint Done
                        SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,TRIM(a.VCHNUM) as Entry_no,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') as Entry_Dt,b.name as Section_Name,a.ent_by,a.type, from pmaint a, (Select type1,name from type where id=':' and type1>'10') b where  trim(a.acode)=trim(b.type1) and a.type='" + frm_vty + "' and a.branchcd='" + frm_mbr + "' and a.vchnum<>'000000' and a.vchdate " + DateRange + " order by a.vchdate desc ,trim(a.vchnum) desc";
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
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        if (frm_formID == "F75151" || frm_formID == "F75156")
        {
            Cal();
            if (txtlbl4.Text.Length <= 1)
            {
                fgen.msg("-", "AMSG", "Please Select Mould");
                txtlbl4.Focus(); return;
            }
            if (txtlbl7.Text.Length <= 1)
            {
                fgen.msg("-", "AMSG", "Please Fill Date");
                txtlbl7.Focus(); return;
            }
            if (txtlbl7a.Text.Length <= 1)
            {
                fgen.msg("-", "AMSG", "Please Fill Time");
                txtlbl7a.Focus(); return;
            }
            if (Convert.ToDateTime(txtlbl7.Text) > Convert.ToDateTime(txtvchdate.Text))
            {
                fgen.msg("-", "AMSG", "Maintenance Date Can Not Be Greater Than Entry Date");
                return;
            }
            //if (sg1.Rows.Count <= 1)
            //{
            //    fgen.msg("-", "AMSG", "No Item to Save!!'13'Please Select Some Item First"); return;
            //}
            for (int i = 0; i < sg1.Rows.Count - 1; i++)
            {
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text == "" || ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text == "-")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill Qty At Line No. " + sg1.Rows[i].Cells[12].Text + "");
                    return;
                }
            }
        }

        //string mandField = "";
        //mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        //if (mandField.Length > 1)
        //{
        //    fgen.msg("-", "AMSG", mandField);
        //    return;
        //}

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
        fgen.Fn_open_prddmp1("Select DateRange", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        if (frm_formID == "F75151")
        {
            vty = "MM04";
            frm_vty = vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
            lbl1a.Text = vty;
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'", 6, "VCH");
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        }

        else if (frm_formID == "F75156")
        {
            vty = "MM05";
            frm_vty = vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
            lbl1a.Text = vty;
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'", 6, "VCH");
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        }
        else
        {
            vty = "20";
            frm_vty = vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
            lbl1a.Text = vty;
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            txtlbl2.Text = frm_uname;
            txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            txtlbl5.Text = "-";
            txtlbl6.Text = "-";
        }
        disablectrl();
        fgen.EnableForm(this.Controls);
        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        txtlbl7.Focus();
        //Popup asking for Copy from Older Data
        //fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        //hffield.Value = "NEW_E";
        #endregion
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
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty.Substring(2, 2), lblheader.Text.Trim() + " Type =" + frm_vty + " Deleted");
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
            else
            {
                btnlbl4.Focus();
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
                    if (frm_formID == "F75151" || frm_formID == "F75156")  // for Mould Maint Done
                    {
                        SQuery = "SELECT a.*,b.name as mould_name,i.iname,i.unit,to_char(a.date2,'dd/mm/yyyy') as plandate FROM " + frm_tabname + " a left join item i on trim(a.icode)=trim(i.icode),typegrp b WHERE trim(a.col1)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE ='" + frm_vty + "' and b.id='MM' AND TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + col1 + "' order by a.srno";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                        ViewState["fstr"] = col1;
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                            txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                            txtlbl4.Text = dt.Rows[0]["col1"].ToString().Trim();
                            txtlbl4a.Text = dt.Rows[0]["mould_name"].ToString().Trim();
                            txtlbl7.Text = Convert.ToDateTime(dt.Rows[0]["date1"].ToString().Trim()).ToString("yyyy-MM-dd");
                            txtlbl7a.Text = dt.Rows[0]["col12"].ToString().Trim();
                            txtlbl2.Text = dt.Rows[0]["col14"].ToString().Trim();
                            txtlbl3.Text = dt.Rows[0]["num1"].ToString().Trim();
                            txtrmk.Text = dt.Rows[0]["remarks"].ToString().Trim();
                            txtlbl6.Text = dt.Rows[0]["num2"].ToString().Trim();
                            txtPlanDate.Text = dt.Rows[0]["plandate"].ToString().Trim();
                            create_tab();
                            sg1_dr = null;
                            if (dt.Rows[0]["icode"].ToString().Trim().Length > 1)
                            {
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
                                    sg1_dr["sg1_f1"] = dt.Rows[i]["icode"].ToString().Trim();
                                    sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                                    sg1_dr["sg1_f3"] = dt.Rows[i]["unit"].ToString().Trim();
                                    sg1_dr["sg1_t1"] = dt.Rows[i]["num3"].ToString().Trim();
                                    sg1_dr["sg1_t2"] = dt.Rows[i]["num4"].ToString().Trim();
                                    sg1_dr["sg1_t3"] = dt.Rows[i]["num5"].ToString().Trim();
                                    sg1_dt.Rows.Add(sg1_dr);
                                }
                            }
                        }
                    }
                    else if (frm_formID == "F75106") // Maint Done
                    {
                        SQuery = "Select distinct a.*,b.name as Section_Name from pmaint a, (Select type1,name from type where id=':' and type1>'10') b where  a.acode=b.type1 and a.type='" + frm_vty + "' and a.branchcd='" + frm_mbr + "' and a.vchnum<>'000000' and trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' order by a.vchdate desc ,a.vchnum desc";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                        ViewState["fstr"] = col1;
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                            txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                            txtlbl4.Text = dt.Rows[i]["acode"].ToString().Trim();
                            txtlbl4a.Text = dt.Rows[i]["Section_Name"].ToString().Trim();
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
                                sg1_dr["sg1_f1"] = dt.Rows[i]["MCHNAME"].ToString().Trim();
                                sg1_dr["sg1_f2"] = dt.Rows[i]["MCHCODE"].ToString().Trim();
                                sg1_dr["sg1_f3"] = dt.Rows[i]["SPEC4"].ToString().Trim();
                                sg1_dr["sg1_f4"] = dt.Rows[i]["MAINTDT"].ToString().Trim();
                                sg1_dr["sg1_f5"] = "-";
                                sg1_dr["sg1_t1"] = dt.Rows[i]["MAINTBY"].ToString().Trim();
                                sg1_dr["sg1_t2"] = dt.Rows[i]["REMARKS"].ToString().Trim();
                                sg1_dr["sg1_t3"] = dt.Rows[i]["MAINTAMT"].ToString().Trim();
                                sg1_dr["sg1_t16"] = dt.Rows[i]["APPVEN"].ToString().Trim();
                                sg1_dt.Rows.Add(sg1_dr);
                            }
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        if (frm_formID == "F75151" || frm_formID == "F75156")
                        {
                            Cal();
                        }
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
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F10133");
                    fgen.fin_engg_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col3;
                    if (frm_formID == "F75151")
                    {
                        mq0 = "MM04";
                        mq1 = "col7";
                    }
                    else if (frm_formID == "F75156")
                    {
                        mq0 = "MM05";
                        mq1 = "col8";
                    }

                    dt = new DataTable();
                    SQuery = "select max(to_date(date1,'dd/mm/yyyy')) as done_date from wb_maint where branchcd='" + frm_mbr + "' and type='" + mq0 + "' and trim(col1)='" + txtlbl4.Text.Trim() + "'";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    mq2 = "";
                    if (frm_formID == "F75151")
                    {
                        mq2 = "MM02";
                    }
                    else if (frm_formID == "F75156")
                    {
                        mq2 = "MM03";
                    }
                    dt2 = new DataTable();
                    string popq = "";
                    
                    popq="select to_char(date1,'dd/mm/yyyy') as date1 from wb_maint where branchcd='" + frm_mbr + "' and type='" + mq2 + "' and col1='" + txtlbl4.Text.Trim() + "' and TRIM(OBSV2)='" + Convert.ToDateTime(txtlbl7.Text).ToString("dd/MM/yyyy").Substring(3, 7) + "' ";
                    if (frm_formID == "F75156")
                    {
                        popq = "select to_char(date1,'dd/mm/yyyy') as date1 from wb_maint where branchcd='" + frm_mbr + "' and type='" + mq2 + "' and branchcd||trim(col1)||TRIM(OBSV2)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").Trim() + "' ";
                    }
                    
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, popq);

                    if (dt2.Rows.Count > 0)
                    {
                        txtPlanDate.Text = Convert.ToDateTime(dt2.Rows[0]["date1"].ToString().Trim()).ToString("dd/MM/yyyy");
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["done_date"].ToString().Trim().Length > 1)
                        {
                            txtlbl2.Text = Convert.ToDateTime(dt.Rows[0]["done_date"].ToString().Trim()).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            SQuery = "select (case when to_char(to_date(" + mq1 + ",'dd/MM/yyyy'))= to_char(date1,'dd/MM/yyyy') then '-' else " + mq1 + " end) as done_date from wb_master where branchcd='" + frm_mbr + "' and id='MM01' and trim(col1)='" + txtlbl4.Text.Trim() + "'";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                txtlbl2.Text = dt.Rows[0]["done_date"].ToString().Trim();
                            }
                        }
                    }
                    else
                    {
                        SQuery = "select (case when to_char(to_date(" + mq1 + ",'dd/MM/yyyy'))= to_char(date1,'dd/MM/yyyy') then '-' else " + mq1 + " end) as done_date from wb_master where branchcd='" + frm_mbr + "' and id='MM01' and col1='" + txtlbl4.Text.Trim() + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtlbl2.Text = dt.Rows[0]["done_date"].ToString().Trim();
                        }
                    }
                    string last_maint_Dt = "", prd_from_last = ""; mq0 = ""; string count = "", maint_start_dt = "", first_count = "";
                    maint_start_dt = fgen.seek_iname(frm_qstr, frm_cocd, "select OPT_START from fin_rsys_opt_pw where opt_id='W1078' and branchcd='" + frm_mbr + "'", "OPT_START");
                    if (frm_formID == "F75151")
                    {
                        mq0 = "MM04";
                        last_maint_Dt = fgen.seek_iname(frm_qstr, frm_cocd, "select max(to_date(date1,'dd/mm/yyyy')) as vchdate from wb_maint where branchcd='" + frm_mbr + "' and type='" + mq0 + "' and col1='" + txtlbl4.Text.Trim() + "'", "vchdate");
                        if (last_maint_Dt.Trim().Length <= 1)
                        {
                            last_maint_Dt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(col7) as col7 from wb_master where id='MM01' and col1='" + txtlbl4.Text.Trim() + "'", "col7");
                        }
                        
                        first_count = fgen.seek_iname(frm_qstr, frm_cocd, "select to_number(col11) as col11 from wb_master where id='MM01' and trim(col1)='" + txtlbl4.Text.Trim() + "' and branchcd='" + frm_mbr + "'", "col11");
                        //prd_from_last = fgen.seek_iname(frm_qstr, frm_cocd, "select sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totals from prod_sheet where branchcd='" + frm_mbr + "' and type='90'  and vchdate>to_DaTE('" + maint_start_dt + "','dd/mm/yyyy') and vchdate<=to_date('" + Convert.ToDateTime(txtlbl7.Text).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and trim(pvchnum)='" + col2.Trim() + "'", "totals");//old
                        prd_from_last = fgen.seek_iname(frm_qstr, frm_cocd, "select sum(nvl(noups,0)*nvl(fm_fact,0)) as totals from prod_sheet where branchcd='" + frm_mbr + "' and type='90'  and vchdate>to_DaTE('" + maint_start_dt + "','dd/mm/yyyy') and vchdate<=to_date('" + Convert.ToDateTime(txtlbl7.Text).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and trim(pvchnum)='" + col2.Trim() + "'", "totals");//change in formula
                        count = fgen.seek_iname(frm_qstr, frm_cocd, "select provision as count from typegrp where branchcd='" + frm_mbr + "' and id='MM' and type1='" + txtlbl4.Text.Trim() + "'", "count");
                    }
                    else if (frm_formID == "F75156")
                    {
                        mq0 = "MM05";
                        last_maint_Dt = fgen.seek_iname(frm_qstr, frm_cocd, "select max(to_date(date1,'dd/mm/yyyy')) as vchdate from wb_maint where branchcd='" + frm_mbr + "' and type='" + mq0 + "' and col1='" + txtlbl4.Text.Trim() + "'", "vchdate");
                        if (last_maint_Dt.Trim().Length <= 1)
                        {
                            last_maint_Dt = fgen.seek_iname(frm_qstr, frm_cocd, "select col8 from wb_master where id='MM01' and col1='" + txtlbl4.Text.Trim() + "'", "col8");
                            count = fgen.seek_iname(frm_qstr, frm_cocd, "select pageno as count from typegrp where branchcd='" + frm_mbr + "' and id='MM' and type1='" + txtlbl4.Text.Trim() + "'", "count");
                        }
                        string comm_dt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(date1,'dd/mm/yyyy') as comm_dt from wb_master where id='MM01' and trim(col1)='" + txtlbl4.Text.Trim() + "'", "comm_dt");
                        if (Convert.ToDateTime(last_maint_Dt) == Convert.ToDateTime(comm_dt))
                        {
                            first_count = fgen.seek_iname(frm_qstr, frm_cocd, "select to_number(col12) as col12 from wb_master where id='MM01' and col1='" + txtlbl4.Text.Trim() + "'", "col12");
                        }
                        else
                        {
                            count = fgen.seek_iname(frm_qstr, frm_cocd, "select pageno as count from typegrp where branchcd='" + frm_mbr + "' and id='MM' and type1='" + txtlbl4.Text.Trim() + "'", "count");
                        }
                        //prd_from_last = fgen.seek_iname(frm_qstr, frm_cocd, "select sum((nvl(iqtyin,0)+nvl(mlt_loss,0))*nvl(fm_fact,0)) as totals from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and vchdate>to_DaTE('" + maint_start_dt + "','dd/mm/yyyy') and vchdate<=to_date('" + Convert.ToDateTime(txtlbl7.Text).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and pvchnum='" + col2.Trim() + "'", "totals");
                        prd_from_last = fgen.seek_iname(frm_qstr, frm_cocd, "select sum(nvl(noups,0)*nvl(fm_fact,0)) as totals from prod_sheet where branchcd='" + frm_mbr + "' and type='90' and vchdate>to_DaTE('" + maint_start_dt + "','dd/mm/yyyy') and vchdate<=to_date('" + Convert.ToDateTime(txtlbl7.Text).ToString("dd/MM/yyyy") + "','dd/mm/yyyy') and pvchnum='" + col2.Trim() + "'", "totals");
                    }
                    prd_from_last = (fgen.make_double(prd_from_last) + fgen.make_double(first_count)).ToString();
                    txtlbl3.Text = prd_from_last;
                    txtlbl6.Text = (fgen.make_double(count) - fgen.make_double(prd_from_last)).ToString();
                    txtlbl7.Enabled = false;
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
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString().Replace("&amp;", "&");
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
                        if (frm_formID == "F75151" || frm_formID == "F75156") //for Mould Maint Done
                        {
                            dt2 = new DataTable();
                            dt2 = fgen.getdata(frm_qstr, frm_cocd, "select irate as rate,icode,vchdate,vchnum,to_char(vchdate,'yyyymmdd')||trim(vchnum) as vdd from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and type not in ('04','08') and icode in (" + col1 + ")  order by vdd desc");
                            rate = "0";
                            SQuery = "select icode as item_code,iname as item_name,unit,(case when nvl(iqd,0)=0 then irate else iqd end) as rate from item where trim(icode) in (" + col1 + ") order by item_name";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            for (int i = 0; i < dt.Rows.Count; i++)
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
                                sg1_dr["sg1_f1"] = dt.Rows[i]["item_code"].ToString().Trim();
                                sg1_dr["sg1_f2"] = dt.Rows[i]["item_name"].ToString().Trim();
                                sg1_dr["sg1_f3"] = dt.Rows[i]["unit"].ToString().Trim();
                                if (dt2.Rows.Count > 0)
                                {
                                    rate = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["item_code"].ToString().Trim() + "'", "rate");
                                }
                                if (rate == "0")
                                {
                                    rate = dt.Rows[i]["rate"].ToString().Trim();
                                }
                                sg1_dr["sg1_t2"] = rate;
                                sg1_dt.Rows.Add(sg1_dr);
                            }
                        }
                        else   // For Maint Done
                        {
                            dt = new DataTable();
                            SQuery = "select * from (select * from pmaint where branchcd='" + frm_mbr + "' and type='66' and vchdate " + DateRange + " union all select * from pmaint where branchcd='" + frm_mbr + "' and type='20' and vchdate " + DateRange + ") where mchcode='" + col1 + "'";
                            //SQuery = "select Mchname,Mchcode,Inst_no,Maintdt from (select Mchname,Mchcode,Inst_no,Maintdt,(to_Date(MAINTDT,'dd/mm/yyyy')+maintmth*30)+round(((5/12)*maintmth),0) as due_Dt from (SELECT DISTINCT MCHNAME,MCHCODE,spec5 as Inst_No,MAINTDT,maintmth FROM PMAINT where branchcd='00' and type='10' and trim(acode)='12' order by MCHNAME) ORDER BY MCHNAME) WHERE MCHCODE="+ col1 +" ORDER BY MCHNAME";
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

                                sg1_dr["sg1_f1"] = dt.Rows[d]["MCHNAME"].ToString().Trim();
                                sg1_dr["sg1_f2"] = dt.Rows[d]["MCHCODE"].ToString().Trim();
                                sg1_dr["sg1_f3"] = dt.Rows[d]["MAINTDT"].ToString().Trim();
                                sg1_dr["sg1_f4"] = vardate;
                                sg1_dr["sg1_f5"] = "-";
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
                    dt = new DataTable();
                    if (frm_formID == "F75151" || frm_formID == "F75156") //for Mould Maint Done
                    {
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, "select irate as rate,icode,vchdate,vchnum,to_char(vchdate,'yyyymmdd')||trim(vchnum) as vdd from ivoucher where branchcd='" + frm_mbr + "' and  type like '0%' and type not in ('04','08') and icode  ='" + col1 + "'  order by vdd desc");
                        rate = "0";
                        SQuery = "select icode as item_code,iname as item_name,unit,(case when nvl(iqd,0)=0 then irate else iqd end) as rate from item where trim(icode)  ='" + col1 + "' order by item_code";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            //********* Saving in Hidden Field 
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[0]["item_code"].ToString().Trim();
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[0]["item_name"].ToString().Trim();
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[0]["unit"].ToString().Trim();
                            //********* Saving in GridView Value
                            if (dt2.Rows.Count > 0)
                            {
                                rate = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["item_code"].ToString().Trim() + "'", "rate");
                            }
                            if (rate == "0")
                            {
                                rate = dt.Rows[i]["rate"].ToString().Trim();
                            }
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = rate;
                        }
                        else // For Maint Done
                        {
                            SQuery = "select * from (select * from pmaint where branchcd='" + frm_mbr + "' and type='66' and vchdate " + DateRange + " union all select * from pmaint where branchcd='" + frm_mbr + "' and type='20' and vchdate " + DateRange + ") where mchcode='" + col1 + "'";
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                //********* Saving in Hidden Field 
                                sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[0]["MCHNAME"].ToString().Trim();
                                sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[0]["MCHCODE"].ToString().Trim();
                                //********* Saving in GridView Value
                                sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[0]["MAINTDT"].ToString().Trim();
                            }
                        }
                    }
                    setColHeadings();
                    break;

                case "SG1_ADD_MAC":
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
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString().Replace("&amp;", "&");
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
                        SQuery = "select trim(acode)||'/'||trim(srno) as fstr,mchname as Machine_Name,trim(acode)||'/'||trim(srno) as Machine_Code,mch_seq from pmaint where branchcd='" + frm_mbr + "' and type='10' and trim(acode)||'/'||trim(srno)='" + col1 + "' order by acode,srno";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = dt.Rows[0]["machine_code"].ToString().Trim();
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = dt.Rows[0]["machine_name"].ToString().Trim();
                        }
                    }
                    #endregion
                    setColHeadings();
                    break;

                case "SG3_ROW_ADD":
                    break;

                case "SG2_RMV":
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
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim().Replace("&amp;", "&");
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
                        //if (edmode.Value == "Y")
                        //{
                        //    //sg1_dr["sg1_f1"] = "*" + sg1.Rows[-1 + Convert.ToInt32(hf1.Value.Trim())].Cells[13].Text.Trim();
                        //    sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        //}
                        //else
                        //{
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        // }
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
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            if (frm_formID == "F75151" || frm_formID == "F75156")
            {
                SQuery = "select a.vchnum as Entry_no,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,b.name as Mould_name,a.col1 as Code,a.cpartno as part_no,a.icode as item_code,i.iname as item_name,i.unit,a.num3 as qty ,a.num4 as rate,a.num5 as amt,to_char(a.date1,'dd/mm/yyyy') as done_date,a.col12 as Dtime,a.col14 as Last_Done_dt ,a.ent_by as ent_by,a.ent_Dt,a.type  from " + frm_tabname + " a left join item i on trim(a.icode)=trim(i.icode),typegrp b where trim(a.col1)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and b.id='MM' and a.vchdate " + PrdRange + " order by a.vchnum desc";
            }
            else if (frm_formID == "F75106")
            {
                SQuery = "select a.vchnum as Entry_no,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.mchname as name,a.mchcode as code,a.spec4 as last_done_dt,a.maintby as done_at, a.maintdt as done_dt,a.maintamt as amount_exp,a.remarks,a.ent_by,a.ent_Dt from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + "  order by a.vchnum desc";
            }

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "Print_E")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

            fgen.drillQuery(0, "select distinct trim(mchcode)||trim(mchname) as fstr,'-' as gstr, mchname as equipment_name,mchcode as equipment_code, srno as instances from pmaint where branchcd='" + frm_mbr + "' and type='20' and vchdate " + PrdRange + "", frm_qstr);
            fgen.drillQuery(1, "select '-' as fstr,trim(mchcode)||trim(mchname) as gstr,mchname as equipment_name,mchcode as equipment_code,maintdt as done_on,maintby as done_by,maintamt as amount from pmaint where branchcd='" + frm_mbr + "' and type='20' and vchdate " + PrdRange + " ", frm_qstr);
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            string header_n = "Equipment/ Machines Whose Maintenance Done";
            fgen.Fn_DrillReport(header_n + " for the Period " + fromdt + " to " + todt, frm_qstr);
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------
            if (txtlbl4.Text.Trim().Length < 2)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please fill Record!!");
            }
            string last_entdt;
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum as Doc_no from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and trim(icode)='" + txtlbl4.Text.Trim() + "' and trim(vchnum)!='" + txtvchnum.Text.Trim() + "'", "Doc_no");
            if (last_entdt.Trim().Length < 6)
            { }
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
                        Checked_ok = "N"; btnsave.Disabled = false;
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                    }
                }
            }

            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N"; btnsave.Disabled = false;
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
                        if (frm_formID == "F75106")
                        {
                            save_fun2();
                        }
                        else if (frm_formID == "F75151" || frm_formID == "F75156")
                        {
                            save_fun();
                        }

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
                            if (frm_formID == "F75151" || frm_formID == "F75156")
                            {
                                if (sg1.Rows.Count > 1)
                                {
                                    for (i = 0; i < sg1.Rows.Count - 0; i++)
                                    {
                                        if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
                                        {
                                            save_it = "Y";
                                        }
                                    }
                                }
                                else
                                {
                                    save_it = "Y"; // ITEM SELECTION NOT COMPULSORY IN RECORDING FORM
                                }
                            }
                            else if (frm_formID == "F75106")
                            {
                                for (i = 0; i < sg1.Rows.Count - 0; i++)
                                {
                                    if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
                                    {
                                        save_it = "Y";
                                    }
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

                        if (frm_formID == "F75106")
                        {
                            save_fun2();
                        }
                        else if (frm_formID == "F75151" || frm_formID == "F75156")
                        {
                            save_fun();
                        }

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + frm_vnum + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); setColHeadings();
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
                if (txtlbl4.Text.Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Select Mould First");
                    return;
                }
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
                    fgen.Fn_open_mseek("Select Items", frm_qstr);
                }
                break;

            case "SG1_ADD_MAC":
                hf1.Value = index.ToString();
                if (sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text.Trim().Length > 1)
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ADD_MAC";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Machine", frm_qstr);
                }
                else
                {
                    fgen.msg("-", "AMSG", "Please Select Stage First!!!!");
                    return;
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
                //if (index < sg2.Rows.Count - 1)
                //{
                //    hf1.Value = index.ToString();
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                //    //----------------------------
                //    hffield.Value = "SG2_RMV";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                //    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                //}
                break;
            case "SG2_ROW_ADD":
                //dt = new DataTable();
                //sg2_dt = new DataTable();
                //dt = (DataTable)ViewState["sg2"];
                //z = dt.Rows.Count - 1;
                //sg2_dt = dt.Clone();
                //sg2_dr = null;
                //i = 0;
                //for (i = 0; i < sg2.Rows.Count; i++)
                //{
                //    sg2_dr = sg2_dt.NewRow();
                //    sg2_dr["sg2_srno"] = (i + 1);
                //    sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                //    sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                //    sg2_dt.Rows.Add(sg2_dr);
                //}
                //sg2_add_blankrows();
                //ViewState["sg2"] = sg2_dt;
                //sg2.DataSource = sg2_dt;
                //sg2.DataBind();
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
                //if (index < sg3.Rows.Count - 1)
                //{
                //    hf1.Value = index.ToString();
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                //    //----------------------------
                //    hffield.Value = "SG3_RMV";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                //    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                //}
                break;
            case "SG3_ROW_ADD":
                //if (index < sg3.Rows.Count - 1)
                //{
                //    hf1.Value = index.ToString();
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                //    //----------------------------
                //    hffield.Value = "SG3_ROW_ADD_E";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                //    make_qry_4_popup();
                //    fgen.Fn_open_sseek("Select Item", frm_qstr);
                //}
                //else
                //{
                //    hffield.Value = "SG3_ROW_ADD";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                //    make_qry_4_popup();
                //    fgen.Fn_open_mseek("Select Item", frm_qstr);
                //}
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        if (txtlbl7.Text.Trim().Length <= 1)
        {
            fgen.msg("-", frm_qstr, "Please Fill Maintenance Date Before Selecting Mould");
            return;
        }
        string last_entdt = "";
        last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
        if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
        {
            Checked_ok = "N"; btnsave.Disabled = false;
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            txtvchdate.Focus();
            return;
        }

        if (Convert.ToDateTime(txtlbl7.Text.Trim()) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtlbl7.Text.Trim()) > Convert.ToDateTime(todt))
        {
            fgen.msg("-", "AMSG", "Date outside " + fromdt + " to " + todt + " is Not Allowed!!'13'Fill date for This Year Only");
            txtlbl7.Focus();
            return;
        }

        if (Convert.ToDateTime(txtlbl7.Text) > Convert.ToDateTime(txtvchdate.Text))
        {
            fgen.msg("-", "AMSG", "Maintenance Date Can Not Be Greater Than Entry Date");
            return;
        }

        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);
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
        i = 0; dt2 = new DataTable(); mq0 = "";
        if (frm_formID == "F75151")
        {
            mq0 = "MM02";
        }
        else if (frm_formID == "F75156")
        {
            mq0 = "MM03";
        }
        dt2 = fgen.getdata(frm_qstr, frm_cocd, "select cpartno,to_char(date1,'dd/mm/yyyy') as date1,obsv2 from wb_maint where branchcd='" + frm_mbr + "' and type='" + mq0 + "' and col1='" + txtlbl4.Text.Trim() + "' and TRIM(OBSV2)='" + Convert.ToDateTime(txtlbl7.Text).ToString("dd/MM/yyyy").Substring(3, 7) + "' ");
        if (sg1.Rows.Count > 1)
        {
            for (i = 0; i < sg1.Rows.Count - 0; i++)
            {
                if (sg1.Rows[i].Cells[13].Text.Length > 1)
                {
                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = frm_mbr;
                    oporow["TYPE"] = frm_vty;
                    oporow["vchnum"] = frm_vnum.Trim().ToUpper();
                    oporow["vchdate"] = txtvchdate.Text.Trim();
                    oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim();
                    ////oporow["col1"] = txtlbl4a.Text.Trim();
                    oporow["COL1"] = txtlbl4.Text.Trim();
                    // oporow["col2"] = txtlbl4.Text.Trim();//cow
                    ////oporow["COL3"] = Txtacref.Text.Trim();
                    oporow["COL2"] = "-";
                    oporow["col3"] = "-";
                    oporow["COL4"] = "-";
                    oporow["col5"] = "-";
                    oporow["col6"] = "-";
                    oporow["col7"] = "-";
                    oporow["col8"] = "-";
                    oporow["COL9"] = "-";
                    oporow["COL10"] = "-";
                    // oporow["col11"] = Convert.ToDateTime(txtlbl7.Text.Trim()).ToString("dd/MM/yyyy");
                    oporow["DATE1"] = Convert.ToDateTime(txtlbl7.Text.Trim()).ToString("dd/MM/yyyy");
                    oporow["col11"] = "-";
                    oporow["col12"] = txtlbl7a.Text.Trim();
                    oporow["col13"] = txtlbl3.Text.Trim();
                    oporow["col14"] = txtlbl2.Text.Trim();
                    oporow["COL15"] = "-";
                    if (txtrmk.Text.Trim().Length > 300)
                    {
                        oporow["remarks"] = txtrmk.Text.Trim().ToUpper().Substring(0, 299);
                    }
                    else
                    {
                        oporow["remarks"] = txtrmk.Text.Trim().ToUpper();
                    }
                    if (i == 0)
                    {
                        oporow["NUM1"] = fgen.make_double(txtlbl3.Text.Trim()); // SHOTS AFTER LAST PRODUCTION DATE
                    }
                    else
                    {
                        oporow["NUM1"] = 0;
                    }
                    oporow["TITLE"] = "-";
                    oporow["BTCHNO"] = "-";
                    oporow["ACODE"] = "-";
                    oporow["GRADE"] = "-";
                    oporow["SRNO"] = i + 1;
                    if (dt2.Rows.Count > 0)
                    {
                        oporow["DATE2"] = dt2.Rows[0]["date1"].ToString().Trim(); // plan date
                        oporow["OBSV2"] = dt2.Rows[0]["obsv2"].ToString().Trim(); // plan month
                        oporow["CPARTNO"] = dt2.Rows[0]["cpartno"].ToString().Trim();
                    }
                    oporow["RESULT"] = "-";
                    oporow["OBSV1"] = "-";
                    oporow["OBSV3"] = "-";
                    oporow["OBSV4"] = "-";
                    oporow["OBSV5"] = "-";
                    oporow["OBSV6"] = "-";
                    oporow["OBSV7"] = "-";
                    oporow["OBSV8"] = "-";
                    oporow["OBSV9"] = "-";
                    oporow["OBSV10"] = "-";
                    oporow["OBSV11"] = "-";
                    oporow["OBSV12"] = "-";
                    oporow["OBSV13"] = "-";
                    oporow["OBSV14"] = "-";
                    oporow["OBSV15"] = "-";
                    oporow["NUM2"] = txtlbl6.Text.Trim(); // balance SHOTS
                    oporow["NUM3"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper());
                    oporow["NUM4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper());
                    oporow["NUM5"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper());
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
        else
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum.Trim().ToUpper();
            oporow["vchdate"] = txtvchdate.Text.Trim();
            oporow["icode"] = "-";
            oporow["COL1"] = txtlbl4.Text.Trim();
            oporow["COL2"] = "-";
            oporow["col3"] = "-";
            oporow["COL3"] = "-";
            oporow["COL4"] = "-";
            oporow["col5"] = "-";
            oporow["col6"] = "-";
            oporow["col7"] = "-";
            oporow["col8"] = "-";
            oporow["COL9"] = "-";
            oporow["COL10"] = "-";
            // oporow["col11"] = Convert.ToDateTime(txtlbl7.Text.Trim()).ToString("dd/MM/yyyy");
            oporow["DATE1"] = Convert.ToDateTime(txtlbl7.Text.Trim()).ToString("dd/MM/yyyy");
            oporow["col11"] = "-";
            oporow["col12"] = txtlbl7a.Text.Trim();
            oporow["col13"] = txtlbl3.Text.Trim();
            oporow["col14"] = txtlbl2.Text.Trim();
            oporow["COL15"] = "-";
            if (txtrmk.Text.Trim().Length > 300)
            {
                oporow["remarks"] = txtrmk.Text.Trim().ToUpper().Substring(0, 299);
            }
            else
            {
                oporow["remarks"] = txtrmk.Text.Trim().ToUpper();
            }
            oporow["NUM1"] = fgen.make_double(txtlbl3.Text.Trim()); // SHOTS AFTER LAST PRODUCTION DATE
            oporow["TITLE"] = "-";
            oporow["BTCHNO"] = "-";
            oporow["ACODE"] = "-";
            oporow["GRADE"] = "-";
            oporow["SRNO"] = i + 1;
            if (dt2.Rows.Count > 0)
            {
                oporow["DATE2"] = dt2.Rows[0]["date1"].ToString().Trim(); // plan date
                oporow["OBSV2"] = dt2.Rows[0]["obsv2"].ToString().Trim(); // plan month
                oporow["CPARTNO"] = dt2.Rows[0]["cpartno"].ToString().Trim();
            }
            oporow["RESULT"] = "-";
            oporow["OBSV1"] = "-";
            oporow["OBSV3"] = "-";
            oporow["OBSV4"] = "-";
            oporow["OBSV5"] = "-";
            oporow["OBSV6"] = "-";
            oporow["OBSV7"] = "-";
            oporow["OBSV8"] = "-";
            oporow["OBSV9"] = "-";
            oporow["OBSV10"] = "-";
            oporow["OBSV11"] = "-";
            oporow["OBSV12"] = "-";
            oporow["OBSV13"] = "-";
            oporow["OBSV14"] = "-";
            oporow["OBSV15"] = "-";
            oporow["NUM2"] = txtlbl6.Text.Trim(); // LAST SHOTS
            oporow["NUM3"] = 0;
            oporow["NUM4"] = 0;
            oporow["NUM5"] = 0;
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
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        i = 0;
        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Length > 1)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum.Trim().ToUpper();
                oporow["vchdate"] = txtvchdate.Text.Trim();
                oporow["MCHNAME"] = sg1.Rows[i].Cells[13].Text.Trim().ToUpper();
                oporow["MCHCODE"] = sg1.Rows[i].Cells[14].Text.Trim().ToUpper();
                oporow["SPEC4"] = sg1.Rows[i].Cells[15].Text.Trim().ToUpper();
                oporow["MAINTBY"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
                oporow["MAINTDT"] = sg1.Rows[i].Cells[16].Text.Trim().ToUpper();
                oporow["MAINTMTH"] = 0;
                oporow["REMARKS"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper();
                oporow["SRNO"] = i + 1;
                oporow["ACODE"] = txtlbl4.Text.Trim().ToUpper();
                oporow["ICODE"] = "-";
                oporow["PR_NO"] = "-";
                oporow["PR_DT"] = vardate;
                oporow["PO_NO"] = "-";
                oporow["PO_DT"] = vardate;
                oporow["FASSTNO"] = "-";
                oporow["ITMREMARKS"] = "-";
                oporow["MAINTAMT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper());
                oporow["APPVEN"] = ((DropDownList)sg1.Rows[i].FindControl("dd1")).SelectedItem.Text.Trim().ToUpper().Replace("&NBSP;", "");
                //fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().ToUpper());
                oporow["EXP_DATE"] = "-";
                oporow["MCH_SEQ"] = 0;
                oporow["ESPL_TAG"] = "-";
                oporow["TOOLUSED"] = 0;
                oporow["NCAPA"] = 0;
                oporow["WAR_INFO"] = "-";
                oporow["WAR_DATE"] = "-";
                oporow["AMC_INFO"] = "-";
                oporow["AMC_DATE"] = "-";
                oporow["AMC_DATE"] = "-";
                oporow["OTH_INFO"] = "-";
                oporow["CONV_MACH"] = "N";
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
        switch (Prg_Id)
        {
            case "F10133":
                SQuery = "SELECT '10' AS FSTR,'Process Mapping' as NAME,'10' AS CODE FROM dual";
                break;
        }
    }
    //------------------------------------------------------------------------------------
    private void Cal()
    {
        double qty = 0, rate = 0, amt = 0;
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            qty = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
            rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text);
            amt += qty * rate;
        }
        txtlbl5.Text = amt.ToString();
    }
    //------------------------------------------------------------------------------------
}