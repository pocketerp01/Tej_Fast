using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Data.OleDb;
using System.Drawing;

public partial class om_pay_data : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, SQuery2, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", mq2, PDateRange, Arr_Month_Name = "", mq3 = "", todaydate = "";
    DataTable dt, dt2, dt3, dt4, dt5; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0; string mq0 = "";
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok; string FileName = "";
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_tabname1, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
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
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
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
            typePopup = "Y";
            btnlist.InnerText = "Print Attn";
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
                for (int i = 0; i < 4; i++)
                {
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[K].Cells[i].CssClass = "hidden";
                }
                #endregion
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));
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
        // to hide and show to tab panel
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        if (Prg_Id == "F85106")
        {
            tab2.Visible = false;
        }
        else
        {
            tab2.Visible = true;
        }
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
        btnprint.Disabled = false; btnlist.Disabled = false; btnAttn.Enabled = false; btnImport.Enabled = false; btnFormat.Enabled = false;
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
        btnprint.Disabled = true; btnlist.Disabled = true; btnAttn.Enabled = true; btnImport.Enabled = true; btnFormat.Enabled = true;
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
        lblheader.Text = "Pay Data";

        if (frm_formID == "F85156")
        {
            lblheader.Text = "Full & Final Data";
        }

        doc_nf.Value = "vchnum";
        doc_df.Value = "date_";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "pay";
        frm_tabname1 = "scratch2";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL8", frm_tabname1);
        if (frm_formID == "F85156")
        {
            btnAttn.Visible = false;
            btnFormat.Visible = false;
            Sal2.Visible = false;
            lblBonus.Visible = false;
            txtBonus_Per.Visible = false;
            txtBonus_Amt.Visible = false;
            lblOthers.Visible = false;
            txtOthers.Visible = false;
        }
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

            case "SHFTCODE":
                break;

            case "DEPTTCODE":
                break;

            case "TICODE":
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":

                break;

            case "SG1_ROW_ADD1":
            case "SG1_ROW_ADD_E1":
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
            case "List":
                Type_Sel_query();
                break;

            case "New_E":
            case "List_E":
                SQuery = "select trim(mthnum) as fstr,mthname ,mthnum from mths order by mthsno";
                break;

            case "Print_E":
            case "Print_E2":
                SQuery = "select distinct to_char(date_,'mm/yyyy') as fstr,vchnum as entry_no, to_char(date_,'mm/yyyy') as month_year,ent_by,to_char(date_,'yyyymmdd') as vdd from pay where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and date_ " + DateRange + " order by vdd desc,entry_no desc";
                break;

            case "Print1":
                SQuery = "select trim(type1) AS FSTR,NAME AS BRANCH,TYPE1 AS CODE FROM TYPE WHERE ID='B' ORDER BY CODE";
                break;

            case "Print_E1":
                Type_Sel_query();
                break;

            //case "Print_E":
            //    SQuery = "select distinct trim(A.vchnum)||to_Char(a.date_,'dd/mm/yyyy') as fstr,A.vchnum,to_Char(a.date_,'dd/mm/yyyy') as dated,A.type,a.ent_by,a.branchcd,a.grade,to_Char(a.date_,'yyyymmdd') as vdd from " + frm_tabname + " A where grade='" + txtlbl4.Text.Trim() + "' and branchcd='" + frm_mbr + "' and date_ " + DateRange + " AND LENGTH(tRIM(nvl(A.LEAVING_dT,'-')))<5 and trim(nvl(a.deptt1,'-'))!='ONEBY' order by vdd desc,A.vchnum desc";
            //    break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "COPY_OLD" || btnval == "Del_E")
                {
                    if (frm_formID == "F85156")
                    {
                        SQuery = "select distinct trim(A.vchnum)||to_Char(a.date_,'dd/mm/yyyy') as fstr,A.vchnum,to_Char(a.date_,'dd/mm/yyyy') as dated,A.type,a.empcode,e.name,a.branchcd,a.grade as grade_code,t.name as grade,to_Char(a.date_,'yyyymmdd') as vdd from " + frm_tabname + " A,type t,empmas e where trim(a.grade)=trim(t.type1) and trim(a.branchcd)||trim(a.grade)||trim(a.empcode)=trim(e.branchcd)||trim(e.grade)||trim(e.empcode) and t.id='I' and a.grade='" + grade.Value.Trim() + "' and a.branchcd='" + frm_mbr + "' and a.date_ " + DateRange + " AND trim(nvl(a.deptt1,'-'))='ONEBY' order by vdd desc,A.vchnum desc";
                    }
                    else
                    {
                        SQuery = "select distinct trim(A.vchnum)||to_Char(a.date_,'dd/mm/yyyy') as fstr,A.vchnum,to_Char(a.date_,'dd/mm/yyyy') as dated,A.type,a.ent_by,a.branchcd,a.grade as grade_code,t.name as grade,to_Char(a.date_,'yyyymmdd') as vdd from " + frm_tabname + " A,type t where trim(a.grade)=trim(t.type1) and t.id='I' and a.grade='" + grade.Value.Trim() + "' and a.branchcd='" + frm_mbr + "' and a.date_ " + DateRange + " AND LENGTH(tRIM(nvl(A.LEAVING_dT,'-')))<5 and trim(nvl(a.deptt1,'-'))!='ONEBY' order by vdd desc,A.vchnum desc";
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
                fgen.Fn_open_sseek("Select Grade", frm_qstr);
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
        frm_vty = "10";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        lbl1a.Text = frm_vty;
        string mon_end_dt = "";
        int month = fgen.make_int(col1) + 1; int year;
        frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
        todaydate = "01/" + hf2.Value + "/" + frm_myear;
        switch (vty)
        {
            case "12":
                if (Convert.ToDateTime(todaydate) <= Convert.ToDateTime(frm_CDT1))
                {
                    mon_end_dt = "01/01/" + (fgen.make_int(frm_myear)).ToString();
                    year = fgen.make_int(frm_myear);
                }
                else
                {
                    mon_end_dt = "01/01/" + (fgen.make_int(frm_myear) + 1).ToString();
                    year = fgen.make_int(frm_myear);
                }
                //mon_end_dt = "01/01/" + (fgen.make_int(frm_myear) + 1).ToString(); // ORIGINAL
                //year = fgen.make_int(frm_myear); // ORIGINAL
                break;

            case "01":
            case "02":
            case "03":
                if (Convert.ToDateTime(todaydate) > Convert.ToDateTime(frm_CDT1))
                {
                    mon_end_dt = "01/" + fgen.padlc(month, 2) + "/" + frm_CDT1.Substring(6, 4);
                    year = fgen.make_int(frm_CDT1.Substring(6, 4));
                }
                else
                {
                    mon_end_dt = "01/" + fgen.padlc(month, 2) + "/" + frm_CDT2.Substring(6, 4);
                    year = fgen.make_int(frm_CDT2.Substring(6, 4));
                }
                //mon_end_dt = "01/" + fgen.padlc(month, 2) + "/" + (fgen.make_int(frm_myear) + 1).ToString(); // ORIGINAL
                //year = fgen.make_int(frm_myear) + 1;// ORIGINAL
                break;

            default:
                mon_end_dt = "01/" + fgen.padlc(month, 2) + "/" + frm_myear;
                year = fgen.make_int(frm_myear);
                break;
        }
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and date_ " + DateRange + "", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = Convert.ToDateTime(mon_end_dt).AddDays(-1).ToString("dd/MM/yyyy");
        txtlbl2.Text = frm_uname;
        txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl5.Text = "-";
        txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

        int weekly_off = CountSundays(year, Convert.ToInt32(vty));
        int days = Convert.ToInt32(txtvchdate.Text.Substring(0, 2)) - weekly_off;
        txtlbl8.Text = days.ToString();
        txtlbl8.Text = txtvchdate.Text.Substring(0, 2);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();
        dt5 = new DataTable();
        string control_allowed = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ENABLE_YN FROM CONTROLS WHERE ID='R16'", "ENABLE_YN");
        if (control_allowed == "Y")
        {
            mq2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='R16'", "PARAMS");
            mq3 = "create or replace view advstat as(select a.branchcd,a.grade,a.empcode,Sum(a.dramt) as Dramt,nvl(b.advretu,0) as cramt from payadv a left outer join (select branchcd,grade,empcode,sum(advance) as advretu from pay where branchcd='" + frm_mbr + "' and grade='" + grade.Value.Trim() + "' and date_>=to_DatE('" + mq2 + "','dd/mm/yyyy') group by branchcd,grade,empcode) b on a.branchcd=b.branchcd and trim(a.empcode)=trim(b.empcode) and trim(a.grade)=trim(b.grade) where a.branchcd='" + frm_mbr + "' and a.grade='" + grade.Value.Trim() + "' and vchdate>=to_DatE('" + mq2 + "','dd/mm/yyyy') group by a.branchcd,a.grade,a.empcode,nvl(b.advretu,0)) order by empcode";
        }
        else
        {
            mq3 = "create or replace view advstat as(select a.branchcd,a.grade,a.empcode,Sum(a.dramt) as Dramt,nvl(b.advretu,0) as cramt from payadv a left outer join (select branchcd,grade,empcode,sum(advance) as advretu from pay where branchcd='" + frm_mbr + "' and grade='" + grade.Value.Trim() + "' group by branchcd,grade,empcode) b on a.branchcd=b.branchcd and trim(a.empcode)=trim(b.empcode) and trim(a.grade)=trim(b.grade) where a.branchcd='" + frm_mbr + "' and a.grade='" + grade.Value.Trim() + "' group by a.branchcd,a.grade,a.empcode,nvl(b.advretu,0)) order by empcode";
        }
        fgen.execute_cmd(frm_qstr, frm_cocd, mq3);

        double minstamt = 0, seekn1 = 0;
        if (frm_formID == "F85156")
        {
            SQuery = "select nvl(a.deptt_text,'') as deptt_Text,to_char(a.dtjoin,'dd/mm/yyyy') as dtjoin,nvl(a.appr_by,'-') as appr_by,nvl(a.erpecode,'XXXXXX') as erpecode,nvl(a.empcode,'XXXXXX') as empcode,nvl(a.name,'-') as Name,nvl(a.cardno,'-') as cardno,a.old_empc,nvl(a.fhname,'-') as fhname,A.WRKHOUR,nvl(a.deptt,'-') as deptt,nvl(a.desg,'-') as desg,b.dramt,(b.dramt-b.cramt) as advbal,nvl(a.ded4,0) as tds,nvl(a.ded7,0) as ded7,nvl(a.ded8,0) as ded8,nvl(a.ded9,0) as ded9,a.leaving_Dt,a.ded12,a.tfr_stat,a.grade,t.name as gradename from type t,empmas a left outer join advstat b on trim(a.empcode)=trim(b.empcode) and trim(a.grade)=trim(b.grade) and a.branchcd=b.branchcd where trim(a.grade)=trim(t.type1) and t.id='I' and substr(nvl(trim(a.tfr_stat),'-'),1,8)<>'TRANSFER' and TRIM(nvl(a.leaving_dt,'-'))='-' and  a.branchcd='" + frm_mbr + "' and a.grade='" + grade.Value.Trim() + "' and a.dtjoin<=to_DatE('" + txtvchdate.Text + "','dd/mm/yyyy') and substr(nvl(trim(appr_by),'-'),1,3)='[A]' and a.empcode='" + col1 + "' order by a.empcode";
        }
        else
        {
            SQuery = "select nvl(a.deptt_text,'') as deptt_Text,to_char(a.dtjoin,'dd/mm/yyyy') as dtjoin,nvl(a.appr_by,'-') as appr_by,nvl(a.erpecode,'XXXXXX') as erpecode,nvl(a.empcode,'XXXXXX') as empcode,nvl(a.name,'-') as Name,nvl(a.cardno,'-') as cardno,a.old_empc,nvl(a.fhname,'-') as fhname,A.WRKHOUR,nvl(a.deptt,'-') as deptt,nvl(a.desg,'-') as desg,b.dramt,(b.dramt-b.cramt) as advbal,nvl(a.ded4,0) as tds,nvl(a.ded7,0) as ded7,nvl(a.ded8,0) as ded8,nvl(a.ded9,0) as ded9,a.leaving_Dt,a.ded12,a.tfr_stat,a.grade,t.name as gradename from type t,empmas a left outer join advstat b on trim(a.empcode)=trim(b.empcode) and trim(a.grade)=trim(b.grade) and a.branchcd=b.branchcd where trim(a.grade)=trim(t.type1) and t.id='I' and substr(nvl(trim(a.tfr_stat),'-'),1,8)<>'TRANSFER' and TRIM(nvl(a.leaving_dt,'-'))='-' and  a.branchcd='" + frm_mbr + "' and a.grade='" + grade.Value.Trim() + "' and a.dtjoin<=to_DatE('" + txtvchdate.Text + "','dd/mm/yyyy') and substr(nvl(trim(appr_by),'-'),1,3)='[A]' order by a.empcode";
        }
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        create_tab();
        sg1_dr = null;
        int holmth = fgen.make_int(fgen.seek_iname(frm_qstr, frm_cocd, "select count(vchnum) as holds from payhol where branchcd='" + frm_mbr + "' and type='10' and to_char(hol_date,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'", "holds"));
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            minstamt = 0; seekn1 = 0;
            sg1_dr = sg1_dt.NewRow();
            txtlbl4.Text = dt.Rows[i]["grade"].ToString().Trim();
            txtlbl4a.Text = dt.Rows[i]["gradename"].ToString().Trim();
            sg1_dr["sg1_h11"] = dt.Rows[i]["deptt_Text"].ToString().Trim();
            sg1_dr["sg1_h12"] = dt.Rows[i]["erpecode"].ToString().Trim();
            sg1_dr["sg1_h13"] = dt.Rows[i]["tfr_stat"].ToString().Trim();
            //sg1_dr["sg1_h14"] = dt.Rows[i][""].ToString().Trim();tmj
            sg1_dr["sg1_h15"] = dt.Rows[i]["desg"].ToString().Trim();
            sg1_dr["sg1_h16"] = dt.Rows[i]["leaving_Dt"].ToString().Trim();
            sg1_dr["sg1_h17"] = dt.Rows[i]["dtjoin"].ToString().Trim();
            //sg1_dr["sg1_h18"] = dt.Rows[i][""].ToString().Trim();//coff
            sg1_dr["sg1_h19"] = dt.Rows[i]["appr_by"].ToString().Trim();
            //sg1_dr["sg1_h20"] = dt.Rows[i][""].ToString().Trim();//comp
            sg1_dr["sg1_h21"] = dt.Rows[i]["deptt"].ToString().Trim();
            sg1_dr["sg1_h22"] = dt.Rows[i]["WRKHOUR"].ToString().Trim();
            //sg1_dr["sg1_h23"] = dt.Rows[i][""].ToString().Trim();// adv giv
            sg1_dr["sg1_h5"] = dt.Rows[i]["cardno"].ToString().Trim();
            sg1_dr["sg1_h6"] = dt.Rows[i]["advbal"].ToString().Trim();
            sg1_dr["sg1_h7"] = txtvchdate.Text.Substring(0, 2);
            sg1_dr["sg1_h8"] = dt.Rows[i]["empcode"].ToString().Trim();
            sg1_dr["sg1_h9"] = dt.Rows[i]["Name"].ToString().Trim();
            sg1_dr["sg1_h10"] = dt.Rows[i]["fhname"].ToString().Trim();
            sg1_dr["sg1_SrNo"] = i + 1;
            sg1_dr["sg1_f1"] = dt.Rows[i]["empcode"].ToString().Trim();
            sg1_dr["sg1_t5"] = weekly_off;
            sg1_dr["sg1_t1"] = days - holmth;
            sg1_dr["sg1_t3"] = holmth;
            sg1_dr["sg1_t11"] = dt.Rows[i]["ded7"].ToString().Trim();
            sg1_dr["sg1_t12"] = dt.Rows[i]["tds"].ToString().Trim();
            minstamt = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select INSTAMT  from payadv where branchcd='" + frm_mbr + "' and empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "' and grade='" + txtlbl4.Text + "' and INSTAMT >0 order by vchdate desc", "instamt"));
            if (fgen.make_double(dt.Rows[i]["advbal"].ToString().Trim()) > 0)
            {
                if (fgen.make_double(dt.Rows[i]["advbal"].ToString().Trim()) > minstamt)
                {
                    sg1_dr["sg1_t13"] = minstamt;
                }
                else
                {
                    sg1_dr["sg1_t13"] = fgen.make_double(dt.Rows[i]["advbal"].ToString().Trim());
                }
            }

            dt5 = fgen.getdata(frm_qstr, frm_cocd, "select sum(instamt) as inst,sum(dramt)-sum(Cramt) as bal from (select branchcd,grade,empcode,instamt,dramt,0 as cramt from payloan where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "' and vchdate<=to_datE('" + txtvchdate.Text + "','dd/mm/yyyy') union all select branchcd,grade,empcode,0 as instamt,0 as dramt,loan_ded as cramt from pay where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "')");
            seekn1 = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select INSTAMT from payLOAN where branchcd='" + frm_mbr + "' and empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "' and grade='" + txtlbl4.Text.Trim() + "' and INSTAMT >0 and vchdate<=to_datE('" + txtvchdate.Text + "','dd/mm/yyyy') order by vchdate desc", "instamt"));
            if (dt5.Rows.Count > 0)
            {
                if (fgen.make_double(dt5.Rows[0]["bal"].ToString()) > 0)
                {
                    if (seekn1 > fgen.make_double(dt5.Rows[0]["bal"].ToString()))
                    {
                        sg1_dr["sg1_t11"] = fgen.make_double(dt5.Rows[0]["bal"].ToString());
                    }
                    else
                    {
                        sg1_dr["sg1_t11"] = seekn1;
                    }
                }
            }
            sg1_dt.Rows.Add(sg1_dr);
        }
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
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
            fgen.Fn_open_sseek("Select Grade", frm_qstr);
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

        string reqd_flds; reqd_flds = "";
        int reqd_nc; reqd_nc = 0;

        double present = 0, holiday = 0, wk_off = 0, el = 0, cl = 0, sl = 0, tot = 0;
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            present = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim());
            holiday = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim());
            wk_off = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim());
            el = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim());
            cl = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim());
            sl = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim());
            tot = present + holiday + wk_off + el + cl + sl;
            if (tot > fgen.make_double(txtlbl8.Text))
            {
                reqd_nc = reqd_nc + 1;
                reqd_flds = reqd_flds + " / " + "Please Check Attendance at At Line No. " + sg1.Rows[i].Cells[12].Text.Trim();
                sg1.Rows[i].BackColor = Color.Yellow;
            }
            else
            {
                sg1.Rows[i].BackColor = Color.White;
            }
            ((TextBox)sg1.Rows[i].FindControl("sg1_h7")).Text = tot.ToString();
            ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text = (fgen.make_double(txtlbl8.Text) - tot).ToString();
            if (sg1.Rows[i].Cells[53].Text.Trim().Length > 1)
            {
                z = 0;
                z = (Convert.ToDateTime(txtvchdate.Text) - Convert.ToDateTime(sg1.Rows[i].Cells[53].Text.Trim())).Days + 1;
                if (z < fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_h7")).Text.Trim()))
                {
                    reqd_nc = reqd_nc + 1;
                    reqd_flds = reqd_flds + " / " + "Check Data At Line No. " + sg1.Rows[i].Cells[12].Text.Trim() + "'13'Total Days Entered , Exceed Days of Month after Joining '13' Saving Not Allowed, Days Payable Max " + z;
                    sg1.Rows[i].BackColor = Color.Yellow;
                }
                else
                {
                    sg1.Rows[i].BackColor = Color.White;
                }
            }
            //if (sg1.Rows[i].Cells[55].Text.Trim().Length <= 1)
            //{
            //    reqd_nc = reqd_nc + 1;
            //    reqd_flds = reqd_flds + " / " + "Employee Not Yet Approved For Salary Calculation.Please Check At Line No. " + sg1.Rows[i].Cells[12].Text.Trim();
            //    sg1.Rows[i].BackColor = Color.Yellow;
            //}
            else
            {
                sg1.Rows[i].BackColor = Color.White;
            }
        }
        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_flds.TrimStart('/'));
            return;
        }
        if (frm_formID == "F85156")
        {
            if (txtLeaving_Dt.Text == "-" || txtLeaving_Dt.Text == "")
            {
                fgen.msg("-", "AMSG", "Please Update For Leaving Date of This Employee");
                return;
            }
            dhd = fgen.ChkDate(txtLeaving_Dt.Text.ToString());
            if (dhd == 0)
            {
                fgen.msg("-", "AMSG", "Please Select a Valid Leaving Date"); txtLeaving_Dt.Focus(); return;
            }
            if (Convert.ToDateTime(txtLeaving_Dt.Text) > Convert.ToDateTime(System.DateTime.Now.Date))
            {
                fgen.msg("-", "AMSG", "Leaving Date Cannot Be More Than Current Date");
                return;
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
        FileUpload1.Visible = false;
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Grade", frm_qstr);
        //fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_enable from fin_rsys_opt where opt_id='W0058'", "opt_enable");
        if (mq0 == "Y")
        {
            hffield.Value = "Print1";
            mq0 = "Select Branch";
        }
        else
        {
            hffield.Value = "Print";
            mq0 = "Select Grade";
        }
        make_qry_4_popup();
        fgen.Fn_open_sseek(mq0, frm_qstr);
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
        frm_vty = "10";
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from pay_el a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
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
                    grade.Value = col1;
                    if (col1 == "") return;
                    hffield.Value = "New_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Month", frm_qstr);
                    break;

                case "New_E":
                    todaydate = "01/" + col1 + "/" + frm_myear;
                    hf2.Value = col1;
                    if (Convert.ToDateTime(todaydate) > Convert.ToDateTime(frm_CDT1))
                    {
                        frm_myear = frm_CDT1.Substring(6, 4);
                    }
                    else
                    {
                        frm_myear = frm_CDT2.Substring(6, 4);
                    }

                    //if (Convert.ToInt32(col1) > 3 && Convert.ToInt32(col1) <= 12)
                    //{

                    //}
                    //else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                    mq2 = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_start from fin_rsys_opt_pw where branchcd='" + frm_mbr + "' and opt_id='W2028'", "opt_start");
                    if (mq2.Trim().Length <= 1)
                    {
                        fgen.msg("-", "AMSG", "Please Enter Web Payroll Start Date In Master Controls (W2028)");
                        return;
                    }
                    else
                    {
                        if (Convert.ToDateTime(mq2.Substring(3, 7)) > Convert.ToDateTime(col1 + "/" + frm_myear))
                        {
                            fgen.msg("-", "AMSG", "Web Payroll Start Date is " + mq2 + " But The Selected Month is " + col1 + "/" + frm_myear + ".'13' Entry Not Allowed");
                            return;
                        }
                    }
                    if (frm_formID == "F85156")
                    {
                        doc_addl.Value = col1;
                        mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(last_day(to_date('" + col1 + "/" + frm_myear + "','mm/yyyy')),'dd/mm/yyyy') as lastdate from dual", "lastdate");
                        hffield.Value = "New_E1";
                        SQuery = "Select trim(a.Empcode) as fstr, a.Name,a.Empcode,a.Fhname,a.Deptt_Text as Deptt,to_char(a.dtjoin,'dd/mm/yyyy') as dtjoin from empmas a where length(Trim(a.leaving_Dt))<5 and a.dtjoin<=to_DatE('" + mq0 + "','dd/mm/yyyy') AND substr(nvl(trim(a.tfr_stat),'-'),1,8)<>'TRANSFER' and a.branchcd='" + frm_mbr + "' and a.grade='" + grade.Value.Trim() + "' and a.empcode not in (select empcode from pay where branchcd='" + frm_mbr + "' and grade='" + grade.Value + "' and to_char(date_,'mmyyyy')='" + col1 + frm_myear + "') order by a.EMPCODE";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        fgen.Fn_open_sseek("Employee Whose Salary Not Yet Made In Selected Month", frm_qstr);
                    }
                    else
                    {
                        mq0 = "select distinct vchnum,to_char(date_,'dd/mm/yyyy') as vchdate from pay where branchcd='" + frm_mbr + "' and grade='" + grade.Value + "' and to_char(date_,'mmyyyy')='" + col1 + frm_myear + "' AND length(trim(nvl(leaving_dt,'-')))<5 and trim(nvl(deptt1,'-'))!='ONEBY'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        if (dt.Rows.Count > 0)
                        {
                            fgen.msg("-", "AMSG", "Data Already Entered For Month " + col1 + "/" + frm_myear + "");
                            return;
                        }
                        newCase(col1);
                        Fetch_Arrear_Name();
                    }
                    break;

                case "New_E1":
                    newCase(doc_addl.Value);
                    Fetch_Arrear_Name();
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
                    grade.Value = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
                    lbl1a.Text = "10";
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry to Delete", frm_qstr);
                    break;

                case "Edit":
                    if (col1 == "") return;
                    grade.Value = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
                    lbl1a.Text = "10";
                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry to Edit", frm_qstr);
                    break;

                case "Del_E":
                    if (col1 == "") return;
                    mq2 = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_start from fin_rsys_opt_pw where branchcd='"+ frm_mbr +"' and opt_id='W2028'", "opt_start");
                    if (mq2.Trim().Length <= 1)
                    {
                        fgen.msg("-", "AMSG", "Please Enter Web Payroll Start Date In Master Controls");
                        return;
                    }
                    else
                    {
                        if (Convert.ToDateTime(mq2.Substring(3, 7)) > Convert.ToDateTime(col1.Substring(9, 7)))
                        {
                            fgen.msg("-", "AMSG", "Web Payroll Start Date is " + mq2 + " But The Selected Month is " + col1.Substring(9, 7) + ".'13' Entry Not Allowed");
                            return;
                        }
                    }
                    clearctrl();
                    edmode.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;

                case "Print":
                    if (col1 == "") return;
                    frm_vty = "10";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                    txtlbl4.Text = col1;
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry", frm_qstr);
                    break;

                case "Print1":
                    if (col1 == "") return;
                    frm_vty = "10";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                    doc_addl.Value = col1;
                    hffield.Value = "Print_E1";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Grade", frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    mq2 = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_start from fin_rsys_opt_pw where branchcd='"+ frm_mbr +"' and opt_id='W2028'", "opt_start");
                    if (mq2.Trim().Length <= 1)
                    {
                        fgen.msg("-", "AMSG", "Please Enter Web Payroll Start Date In Master Controls");
                        return;
                    }
                    else
                    {
                        if (Convert.ToDateTime(mq2.Substring(3, 7)) > Convert.ToDateTime(col1.Substring(9, 7)))
                        {
                            fgen.msg("-", "AMSG", "Web Payroll Start Date is " + mq2 + " But The Selected Month is " + col1.Substring(9, 7) + ".'13' Entry Not Allowed");
                            return;
                        }
                    }
                    clearctrl();
                    SQuery = "Select a.*,to_chaR(a.ent_date,'dd/mm/yyyy') as ent_dt,nvl(trim(a.status),'-') as status_,t.name as gradename from " + frm_tabname + " a,type t where trim(a.grade)=trim(t.type1) and t.id='I' and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.date_,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["date_"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4.Text = dt.Rows[0]["grade"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["gradename"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[0]["ent_dt"].ToString().Trim();
                        txtlbl8.Text = txtvchdate.Text.Substring(0, 2);

                        SQuery1 = "select nvl(a.deptt_text,'') as deptt_Text,to_char(a.dtjoin,'dd/mm/yyyy') as dtjoin,nvl(a.appr_by,'-') as appr_by,nvl(a.erpecode,'XXXXXX') as erpecode,nvl(a.empcode,'XXXXXX') as empcode,nvl(a.name,'-') as Name,nvl(a.cardno,'-') as cardno,a.old_empc,nvl(a.fhname,'-') as fhname,A.WRKHOUR,nvl(a.deptt,'-') as deptt,nvl(a.desg,'-') as desg,0as dramt,0 as advbal,nvl(a.ded4,0) as tds,nvl(a.ded7,0) as ded7,nvl(a.ded8,0) as ded8,nvl(a.ded9,0) as ded9,a.leaving_Dt,a.ded12 from empmas a where substr(nvl(trim(a.tfr_stat),'-'),1,8)<>'TRANSFER' and TRIM(nvl(a.leaving_dt,'-'))='-' and  a.branchcd='" + frm_mbr + "' and a.grade='" + txtlbl4.Text.Trim() + "' and a.dtjoin<=to_DatE('" + txtvchdate.Text + "','dd/mm/yyyy') order by a.empcode";
                        SQuery1 = "select nvl(a.deptt_text,'') as deptt_Text,to_char(a.dtjoin,'dd/mm/yyyy') as dtjoin,nvl(a.appr_by,'-') as appr_by,nvl(a.erpecode,'XXXXXX') as erpecode,nvl(a.empcode,'XXXXXX') as empcode,nvl(a.name,'-') as Name,nvl(a.cardno,'-') as cardno,a.old_empc,nvl(a.fhname,'-') as fhname,A.WRKHOUR,nvl(a.deptt,'-') as deptt,nvl(a.desg,'-') as desg,0as dramt,0 as advbal,nvl(a.ded4,0) as tds,nvl(a.ded7,0) as ded7,nvl(a.ded8,0) as ded8,nvl(a.ded9,0) as ded9,a.leaving_Dt,a.ded12 from empmas a where substr(nvl(trim(a.tfr_stat),'-'),1,8)<>'TRANSFER' and a.branchcd='" + frm_mbr + "' and a.grade='" + txtlbl4.Text.Trim() + "' order by a.empcode";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);

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
                            sg1_dr["sg1_h5"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "cardno");
                            sg1_dr["sg1_h6"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "advbal");
                            sg1_dr["sg1_h7"] = dt.Rows[i]["WORKDAYS"].ToString().Trim();
                            sg1_dr["sg1_h8"] = dt.Rows[i]["empcode"].ToString().Trim();
                            sg1_dr["sg1_h9"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "Name");
                            sg1_dr["sg1_h10"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "fhname");
                            sg1_dr["sg1_f1"] = dt.Rows[i]["EMPCODE"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["PRESENT"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["ABSENT"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["SHL"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["OT"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["OFFDAYS"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["EL"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["CL"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["SL"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["HOURS"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["MNTS"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["DED7"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["TDS"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["DED5"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["DED8"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["DED9"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["AR1"].ToString().Trim();
                            sg1_dr["sg1_t17"] = dt.Rows[i]["AR2"].ToString().Trim();
                            sg1_dr["sg1_t18"] = dt.Rows[i]["AR3"].ToString().Trim();
                            if (frm_cocd == "MLGI" || frm_cocd == "MLGA")
                            {
                                sg1_dr["sg1_t19"] = dt.Rows[i]["ER9"].ToString().Trim();
                                sg1_dr["sg1_t20"] = dt.Rows[i]["ER10"].ToString().Trim();
                            }
                            else
                            {
                                sg1_dr["sg1_t19"] = "0";
                                sg1_dr["sg1_t20"] = "0";
                            }
                            sg1_dr["sg1_t21"] = dt.Rows[i]["LWP"].ToString().Trim();
                            sg1_dr["sg1_t22"] = dt.Rows[i]["DESCGRD"].ToString().Trim();
                            sg1_dr["sg1_t23"] = dt.Rows[i]["AR4"].ToString().Trim();
                            sg1_dr["sg1_t25"] = dt.Rows[i]["AR5"].ToString().Trim();
                            sg1_dr["sg1_t26"] = dt.Rows[i]["AR6"].ToString().Trim();
                            sg1_dr["sg1_t27"] = dt.Rows[i]["AR7"].ToString().Trim();
                            sg1_dr["sg1_t28"] = dt.Rows[i]["AR8"].ToString().Trim();
                            sg1_dr["sg1_t29"] = dt.Rows[i]["AR9"].ToString().Trim();
                            sg1_dr["sg1_t30"] = dt.Rows[i]["AR10"].ToString().Trim();
                            sg1_dr["sg1_t31"] = dt.Rows[i]["HOURS2"].ToString().Trim();
                            sg1_dr["sg1_h11"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "deptt_Text");
                            sg1_dr["sg1_h12"] = dt.Rows[i]["ERPECODE"].ToString().Trim();
                            sg1_dr["sg1_h13"] = dt.Rows[i]["STATUS_"].ToString().Trim();
                            sg1_dr["sg1_h14"] = dt.Rows[i]["TMJ"].ToString().Trim();
                            sg1_dr["sg1_h15"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "desg");
                            sg1_dr["sg1_h16"] = dt.Rows[i]["LEAVING_DT"].ToString().Trim();
                            sg1_dr["sg1_h17"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "dtjoin");
                            sg1_dr["sg1_h19"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "appr_by");
                            sg1_dr["sg1_h21"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "deptt");
                            sg1_dr["sg1_h22"] = dt.Rows[i]["WRKHRS"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_date"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        Fetch_Arrear_Name();
                        edmode.Value = "Y";
                        if (frm_formID == "F85156")
                        {
                            txtLTA.Text = dt.Rows[0]["AR10"].ToString();
                            txtMedical.Text = dt.Rows[0]["AR11"].ToString();
                            txtGratuity_Yrs.Text = dt.Rows[0]["AR12"].ToString();
                            txtGratuity_Rs.Text = dt.Rows[0]["AR13"].ToString();
                            txtEL_Days.Text = dt.Rows[0]["AR14"].ToString();
                            txtEL_Rs.Text = dt.Rows[0]["AR15"].ToString();
                            txtNotice_Rs.Text = dt.Rows[0]["AR16"].ToString();
                            txtOthers.Text = dt.Rows[0]["AR17"].ToString();
                            txtBonus_Amt.Text = dt.Rows[0]["AR18"].ToString();
                            txtNotice_Days.Text = dt.Rows[0]["NPDAYS"].ToString();
                            txtBonus_Per.Text = dt.Rows[0]["BON_RATE"].ToString();
                            txtLeaving_Dt.Text = dt.Rows[0]["LEAVING_DT"].ToString();
                            txtReason.Text = dt.Rows[0]["LEAVING_WHY"].ToString();
                        }
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", txtlbl4.Text.Trim());
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", col1);
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", col2); // MONTH NAME
                    frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_pay_reps(frm_qstr);
                    txtlbl4.Text = "";
                    break;

                case "Print_E1":
                    if (col1.Length < 2) return;
                    txtlbl4.Text = col1;
                    hffield.Value = "Print_E2";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry", frm_qstr);
                    break;

                case "Print_E2":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", txtlbl4.Text.Trim());
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", col1);
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", col2); // MONTH NAME
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", doc_addl.Value);
                    frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_pay_reps(frm_qstr);
                    txtlbl4.Text = "";
                    break;

                case "List":
                    if (col1 == "") return;
                    frm_vty = "10";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                    txtlbl4.Text = col1;
                    hffield.Value = "List_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Month", frm_qstr);
                    break;

                case "List_E":
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", txtlbl4.Text.Trim());
                    if (Convert.ToInt32(col1) > 3 && Convert.ToInt32(col1) <= 12)
                    {

                    }
                    else { frm_myear = (Convert.ToInt32(frm_myear) + 1).ToString(); }
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL3", col1);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL4", col2); // month name
                    frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F85150"); // ATTENDANCE REGISTER IS ALREADY MERGE ON THIS ID THATS' WHY IT IS HARD CODED
                    fgen.fin_pay_reps(frm_qstr);
                    txtlbl4.Text = "";
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    break;

                case "ATTN":
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, "SELECT DISTINCT MAXHRS FROM WB_SELMAST where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and nvl(icat,'-')!='Y'");
                        if (dt3.Rows.Count > 0)
                        {
                            mq0 = dt3.Rows[0]["maxhrs"].ToString().Trim();
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Kindly Update Masters For Max. Working Hrs");
                            return;
                        }

                        SQuery = "SELECT * FROM (SELECT x.branchcd,x.grade,x.name,X.leaving_dt,X.DED7,X.DED4,x.appr_by,x.mcat,x.fhname,X.WRKHOUR,x.EMPCODE,ROUND((sum(nvl((CASE WHEN (x.HR_4D BETWEEN 1 AND x.wrkhour) THEN x.hr_4d END),0)) +sum(nvl((CASE WHEN (x.HR_4D > x.wrkhour) THEN x.wrkhour END),0)))/x.wrkhour,2) as totd,sum(nvl((CASE WHEN (x.HR_4D BETWEEN 1 AND x.wrkhour) THEN x.hr_4d END),0)) as stdd  ,sum(nvl((CASE WHEN (x.HR_4D > x.wrkhour) THEN x.wrkhour END),0)) as otd,sum(nvl((CASE WHEN (x.HR_4D > x.wrkhour) THEN x.hr_4d-x.wrkhour END),0)) as othr from  (SELECT a.branchcd,a.grade,A.EMPCODE,A.MDAY,A.HR_4D,b.name,B.leaving_dt,B.DED7,B.DED4,b.appr_by,b.mcat,b.fhname,decode(B.WRKHOUR,0,8,B.WRKHOUR) as wrkhour FROM (select BRANCHCD,grade,EMPCODE,to_number(to_char(vchdate,'dd')) as mday,round(sum(HRWRK+(MINWRK/60)),2) as HR_4D from ATTN where GRADE='" + txtlbl4.Text.Trim() + "' AND branchcd='" + frm_mbr + "' and to_char(vchdate,'mmyyyy') ='" + txtvchdate.Text.Substring(3, 2) + txtvchdate.Text.Substring(6, 4) + "' group by BRANCHCD,grade,EMPCODE,to_number(to_char(vchdate,'dd'))) A LEFT OUTER JOIN EMPMAS B ON A.EMPCODE=B.EMPCODE AND A.BRANCHCD=B.BRANCHCD and a.grade=b.grade ) x group by x.branchcd,x.grade,x.empcode,x.name,X.leaving_dt,x.appr_by,x.mcat,x.fhname,X.DED7,X.DED4,x.wrkhour) WHERE TRIM(nvl(leaving_dt,'-'))='-' ORDER BY EMPCODE";
                        SQuery = "SELECT * FROM (SELECT x.branchcd,x.grade,x.name,X.leaving_dt,X.DED7,X.DED4,x.appr_by,x.mcat,x.fhname,X.WRKHOUR,x.EMPCODE,ROUND((sum(nvl((CASE WHEN (x.HR_4D BETWEEN 1 AND x.wrkhour) THEN x.hr_4d END),0)) +sum(nvl((CASE WHEN (x.HR_4D > x.wrkhour) THEN x.wrkhour END),0)))/x.wrkhour,2) as totd,sum(nvl((CASE WHEN (x.HR_4D BETWEEN 1 AND x.wrkhour) THEN x.hr_4d END),0)) as stdd  ,sum(nvl((CASE WHEN (x.HR_4D > x.wrkhour) THEN x.wrkhour END),0)) as otd,sum(nvl((CASE WHEN (x.HR_4D > x.wrkhour) THEN x.hr_4d-x.wrkhour END),0)) as othr from  (SELECT a.branchcd,a.grade,A.EMPCODE,A.MDAY,A.HR_4D,b.name,B.leaving_dt,B.DED7,B.DED4,b.appr_by,b.mcat,b.fhname,decode(B.WRKHOUR,0," + mq0 + ",B.WRKHOUR) as wrkhour FROM (select BRANCHCD,grade,EMPCODE,to_number(to_char(vchdate,'dd')) as mday,round(sum(HRWRK+(MINWRK/60)),2) as HR_4D from ATTN where GRADE='" + txtlbl4.Text.Trim() + "' AND branchcd='" + frm_mbr + "' and to_char(vchdate,'mmyyyy') ='" + txtvchdate.Text.Substring(3, 2) + txtvchdate.Text.Substring(6, 4) + "' group by BRANCHCD,grade,EMPCODE,to_number(to_char(vchdate,'dd'))) A LEFT OUTER JOIN EMPMAS B ON A.EMPCODE=B.EMPCODE AND A.BRANCHCD=B.BRANCHCD and a.grade=b.grade ) x group by x.branchcd,x.grade,x.empcode,x.name,X.leaving_dt,x.appr_by,x.mcat,x.fhname,X.DED7,X.DED4,x.wrkhour) WHERE TRIM(nvl(leaving_dt,'-'))='-' ORDER BY EMPCODE";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        SQuery1 = "select nvl(a.deptt_text,'') as deptt_Text,to_char(a.dtjoin,'dd/mm/yyyy') as dtjoin,nvl(a.appr_by,'-') as appr_by,nvl(a.erpecode,'XXXXXX') as erpecode,nvl(a.empcode,'XXXXXX') as empcode,nvl(a.name,'-') as Name,nvl(a.cardno,'-') as cardno,a.old_empc,nvl(a.fhname,'-') as fhname,A.WRKHOUR,nvl(a.deptt,'-') as deptt,nvl(a.desg,'-') as desg,b.dramt,(b.dramt-b.cramt) as advbal,nvl(a.ded4,0) as tds,nvl(a.ded7,0) as ded7,nvl(a.ded8,0) as ded8,nvl(a.ded9,0) as ded9,a.leaving_Dt,a.ded12,nvl(trim(a.status),'-') as status from empmas a left outer join advstat b on trim(a.empcode)=trim(b.empcode) and trim(a.grade)=trim(b.grade) and a.branchcd=b.branchcd where substr(nvl(trim(a.tfr_stat),'-'),1,8)<>'TRANSFER'' and TRIM(nvl(a.leaving_dt,'-'))='-' and  a.branchcd='" + frm_mbr + "' and a.grade='" + txtlbl4.Text.Trim() + "' and a.dtjoin<=to_DatE('" + txtvchdate.Text + "','dd/mm/yyyy') order by a.empcode";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                        create_tab();
                        sg1_dr = null;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_h1"] = "-";
                            sg1_dr["sg1_h2"] = "-";
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "cardno");
                            sg1_dr["sg1_h6"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "advbal");
                            sg1_dr["sg1_h7"] = dt.Rows[i]["totd"].ToString().Trim();
                            sg1_dr["sg1_h8"] = dt.Rows[i]["empcode"].ToString().Trim();
                            sg1_dr["sg1_h9"] = dt.Rows[i]["Name"].ToString().Trim();
                            sg1_dr["sg1_h10"] = dt.Rows[i]["fhname"].ToString().Trim();
                            sg1_dr["sg1_SrNo"] = i + 1;
                            sg1_dr["sg1_f1"] = dt.Rows[i]["empcode"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["totd"].ToString().Trim();
                            sg1_dr["sg1_t2"] = fgen.make_double(txtlbl8.Text) - fgen.make_double(dt.Rows[i]["totd"].ToString().Trim());
                            //sg1_dr["sg1_t3"] = dt.Rows[i][""].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["othr"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["ded4"].ToString().Trim();
                            sg1_dr["sg1_h11"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "deptt_Text");
                            sg1_dr["sg1_h12"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "ERPECODE");
                            sg1_dr["sg1_h13"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "STATUS");
                            sg1_dr["sg1_h14"] = "-";
                            sg1_dr["sg1_h15"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "desg");
                            sg1_dr["sg1_h16"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "LEAVING_DT");
                            sg1_dr["sg1_h17"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "dtjoin");
                            sg1_dr["sg1_h18"] = "-";
                            sg1_dr["sg1_h19"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "appr_by");
                            sg1_dr["sg1_h20"] = "-";
                            sg1_dr["sg1_h21"] = fgen.seek_iname_dt(dt2, "empcode='" + dt.Rows[i]["empcode"].ToString().Trim() + "'", "deptt");
                            sg1_dr["sg1_h22"] = dt.Rows[i]["WRKHOUR"].ToString().Trim();
                            sg1_dr["sg1_h23"] = "0";
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        setColHeadings();
                        ViewState["sg1"] = sg1_dt;
                        Fetch_Arrear_Name();
                    }
                    break;

                case "FORMAT":
                    create_tab();
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    txtlbl4.Text = col1;
                    Fetch_Arrear_Name();
                    //mq2 = "select distinct ed_fld as col,ed_name as columns,morder from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + col1.Trim() + "' and nvl(icat,'-')!='Y' and ed_fld like 'ER%' and morder<11 order by morder";
                    //dt2 = new DataTable();
                    //dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                    //for (int i = 0; i < dt2.Rows.Count; i++)
                    //{
                    //    SQuery += ",0 as Ar_" + dt2.Rows[i]["columns"].ToString().Trim().Replace(" ", "_").Replace(".", "");
                    //}
                    dt = new DataTable();
                    if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
                    {
                        dt.Columns.Add("SRNO", typeof(string));
                        dt.Columns.Add("CARDNO", typeof(string));
                        dt.Columns.Add("PRESENT", typeof(string));
                        dt.Columns.Add("ABSENT", typeof(string));
                        dt.Columns.Add("WK_OFF", typeof(string));
                        dt.Columns.Add("EL", typeof(string));
                        dt.Columns.Add("CL", typeof(string));
                        dt.Columns.Add("SL", typeof(string));
                        dt.Columns.Add("OT1", typeof(string));
                        dt.Columns.Add("OT2", typeof(string));
                    }
                    else
                    {
                        dt.Columns.Add("Srno", typeof(string));
                        dt.Columns.Add("Empcode", typeof(string));
                        z = 1;
                        for (int i = 17; i < 47; i++)
                        {
                            try
                            {
                                dt.Columns.Add(sg1.HeaderRow.Cells[i].Text.Replace("(", "_").Replace(")", "").Replace(" ", "_").Replace(".", "_").Replace("/", "_"));
                            }
                            catch
                            {
                                dt.Columns.Add("Ar" + z);
                                z++;
                            }
                        }
                    }
                    txtlbl4.Text = "";
                    FileName = frm_cocd + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".csv";
                    //fgen.exp_to_excel(dt, "ms-excel", "xls", FileName);
                    string filepath = Server.MapPath("~/tej-base/Upload/") + FileName;
                    fgen.CreateCSVFile(dt, Server.MapPath("~/erp_docs/Upload/") + FileName);
                    Session["FilePath"] = FileName;
                    Session["FileName"] = FileName;
                    Response.Write("<script>");
                    Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                    Response.Write("</script>");
                    fgen.msg("-", "AMSG", "The file has been downloaded!!");
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
                    break;

                case "SHFTCODE":
                    if (col1.Length <= 0) return;
                    break;

                case "DEPTTCODE":
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
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[14].Text.ToString();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[15].Text.ToString();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[16].Text.ToString();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[17].Text.ToString();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[18].Text.ToString();
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        dt = new DataTable();
                        SQuery = "select trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy') as fstr, b.iname as Item_Name,b.cpartno as Part_No,a.erp_Code,a.job_no,to_char(a.Dated,'dd/mm/yyyy') as dated,sum(a.Job_Qty) as Job_qty,a.acode,sum(a.prodn) as Prodn_qty,MAX(A.PRODDT) AS PROD_DT from (select a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,0 as prodn,a.acode,null as proddt from costestimate A WHERE  a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + PDateRange + " and A.SRNO=0 AND trim(nvl(a.status,'N'))<>'Y' union all select a.icode as ERP_Code,A.enqno AS Job_No,A.enqdt as Dated,0 as Job_Qty,to_number(a.col4) as prodn,a.acode,A.VCHDATE from costestimate A WHERE a.branchcd='" + frm_mbr + "' and a.type='60' and a.vchdate  " + PDateRange + " )a, item b where  trim(A.erp_Code)=trim(B.icode) and trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy')='" + col1 + "' group by b.iname,b.cpartno,a.erp_Code,a.job_no,a.dated,a.acode having sum(a.Job_Qty)-sum(a.prodn)>0 order by b.iname";
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
                            sg1_dr["sg1_f1"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[d]["erp_code"].ToString().Trim() + "'", "CL");
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = dt.Rows[d]["Job_No"].ToString().Trim();
                            sg1_dr["sg1_f4"] = Convert.ToDateTime(dt.Rows[d]["dated"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_t1"] = dt.Rows[d]["ERP_Code"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[d]["Item_Name"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[d]["Part_No"].ToString().Trim();
                            sg1_dr["sg1_t6"] = fgen.seek_iname(frm_qstr, frm_cocd, "select * from (select acode,max(irate) as irt from somas where branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and icat<>'Y' and trim(icode)='" + dt.Rows[d]["ERP_Code"].ToString().Trim() + "' group by acode) order by irt desc", "irt");
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t4")).Focus();
                    #endregion
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD_E":
                    dt = new DataTable();
                    SQuery = "select trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy') as fstr,a.ERP_Code, b.iname as Item_Name,b.cpartno,a.erp_Code as Part_No,a.job_no,to_char(a.Dated,'dd/mm/yyyy') as dated,sum(a.Job_Qty) as Job_qty,a.acode,sum(a.prodn) as Prodn_qty,MAX(A.PRODDT) AS PROD_DT from (select a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,0 as prodn,a.acode,null as proddt from costestimate A WHERE  a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + PDateRange + " and A.SRNO=0 AND trim(nvl(a.status,'N'))<>'Y' union all select a.icode as ERP_Code,A.enqno AS Job_No,A.enqdt as Dated,0 as Job_Qty,to_number(a.col4) as prodn,a.acode,A.VCHDATE from costestimate A WHERE a.branchcd='" + frm_mbr + "' and a.type='60' and a.vchdate  " + PDateRange + " )a, item b where  trim(A.erp_Code)=trim(B.icode) and trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy')='" + col1 + "' group by b.iname,b.cpartno,a.erp_Code,a.job_no,a.dated,a.acode having sum(a.Job_Qty)-sum(a.prodn)>0 order by b.iname";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[0]["Job_No"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[0]["dated"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = dt.Rows[0]["ERP_Code"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = dt.Rows[0]["Item_Name"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = dt.Rows[0]["Part_No"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t6")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select * from (select acode,max(irate) as irt from somas where branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and icat<>'Y' and trim(icode)='" + dt.Rows[0]["ERP_Code"].ToString().Trim() + "' group by acode) order by irt desc", "irt");
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[0]["erp_code"].ToString().Trim() + "'", "CL");
                        setColHeadings();
                    }
                    break;

                case "SG1_ROW_ADD1":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
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
                    //}
                    string stage = "0";
                    stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text;
                    dt = new DataTable();
                    //if (col1.Length > 6) SQuery = "select * from evas where trim(userid) in (" + col1 + ")";
                    //else SQuery = "select * from evas where trim(userid)='" + col1 + "'";
                    SQuery = "select distinct b.Iname as iname,a.Icode as iCode,b.Cpartno,a.vchnum,a.qty,to_char(a.vchdate,'dd/mm/yyyy')as vchdate,trim(a.Icode)||'.'||trim(a.vchnum) as fstr,a.col17 from costestimate a, item b,itwstage c where trim(a.icode)=trim(b.icode) and a.type='30' and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.status='N' and c.stagec='" + stage + "' and trim(a.Icode)||'.'||trim(a.vchnum) in (" + col1 + ") order by trim(a.Icode)||'.'||trim(a.vchnum)";
                    //SQuery = "select  a.type1 as fstr,A.NAME,A.type1,B.CNT AS ITEMS from type A,(select DISTINCT stagec,count(icode) AS CNT from itwstage  GROUP BY STAGEC) B where A.id='K' AND A.TYPE1=B.STAGEC and a.type1 in("+col1 +") order by A.TYPE1";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        sg1.Rows[d].Cells[18].Text = dt.Rows[d]["cpartno"].ToString().Trim();

                        //sg1_dr["sg1_f6"] = dt.Rows[d]["cpartno"].ToString().Trim();
                        sg1.Rows[d].Cells[17].Text = dt.Rows[d]["vchnum"].ToString().Trim();
                        sg1.Rows[d].Cells[16].Text = dt.Rows[d]["vchdate"].ToString().Trim();

                        ((TextBox)sg1.Rows[d].FindControl("sg1_t3")).Text = dt.Rows[d]["iname"].ToString().Trim();
                        sg1.Rows[d].Cells[18].Text = dt.Rows[d]["Cpartno"].ToString().Trim();
                        sg1.Rows[d].Cells[22].Width = 70;
                        // sg1_dr["sg1_t2"] = "";
                        //sg1_dr["sg1_t3"] = "";
                        ((TextBox)sg1.Rows[d].FindControl("sg1_t6")).Text = dt.Rows[d]["qty"].ToString().Trim();
                        ((TextBox)sg1.Rows[d].FindControl("sg1_t21")).Text = dt.Rows[d]["iCode"].ToString().Trim();
                        ((TextBox)sg1.Rows[d].FindControl("sg1_t4")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select rate from type where id='K' and type1='" + stage + "'", "rate");
                        ((TextBox)sg1.Rows[d].FindControl("sg1_t5")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select excrate from type where id='K' and type1='" + stage + "'", "excrate");


                        string stagename = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STAGEC FROM ITWSTAGE WHERE SRNO>(SELECT SRNO FROM ITWSTAGE WHERE ICODE='90020488' AND STAGEC='" + stage + "' AND ROWNUM<=1)AND ROWNUM<=1 ORDER BY SRNO", "stagec");

                        ((TextBox)sg1.Rows[d].FindControl("sg1_t36")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='K' and type1='" + stagename + " '", "name");
                        //sg1_dr["sg1_t4"] = dt.Rows[d]["Type1"].ToString().Trim();
                        //sg1_dr["sg1_t5"] = "";
                        //sg1_dr["sg1_t7"] = "";
                        // ((TextBox)sg1.Rows[d].FindControl("sg1_t1")).Text = dt.Rows[d]["qty"].ToString().Trim();
                        //sg1_dr["sg1_t8"] = "";
                        //sg1_dr["sg1_t9"] = "";
                        //sg1_dr["sg1_t10"] = "";
                        //  sg1_dr["sg1_t11"] = dt.Rows[d]["icode"].ToString().Trim();
                        //sg1_dr["sg1_t12"] = "";
                        //sg1_dr["sg1_t13"] = "";
                        //sg1_dr["sg1_t14"] = "";
                        //sg1_dr["sg1_t15"] = "";
                        //sg1_dr["sg1_t16"] = "";

                        // sg1_dt.Rows.Add(sg1_dr);
                    }

                    //sg1_add_blankrows();

                    //ViewState["sg1"] = sg1_dt;
                    //sg1.DataSource = sg1_dt;
                    //sg1.DataBind();
                    //dt.Dispose(); sg1_dt.Dispose();
                    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
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
                            sg1_dr["sg1_srno"] = sg1.Rows[i].Cells[13].Text.Trim();
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[14].Text.Trim();
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[17].Text.Trim();
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        setColHeadings();
                    }
                    #endregion
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = "10";
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "SELECT a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode as cust_code,c.aname as cust_name,a.icode as item_code,b.iname as item_name,a.enqno as jobno,to_char(A.enqdt,'dd/mm/yyyy') as job_dt from costestimate a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + " order by entry_no";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
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
                if (col1 == "Y" && Checked_ok == "Y")
                {
                    try
                    {
                        oDS = new DataSet();
                        oporow = null;
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2 = new DataSet();
                        oporow2 = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();
                        save_fun2();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);

                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnum.Text.Trim();
                            save_it = "Y";
                            cmd_query = "update pay_el set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            //save_it = "N";
                            //for (i = 0; i < sg1.Rows.Count - 0; i++)
                            //{
                            //    if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
                            //    {
                            //        save_it = "Y";
                            //    }
                            //}
                            save_it = "Y";
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

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                            cmd_query = "update " + frm_tabname1 + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, frm_tabname1);

                        save_fun4(); // FOR CALCULATING PF,ESI,WF,PT

                        if (frm_formID == "F85156")
                        {
                            SQuery = "update empmas set leaving_dt='" + txtLeaving_Dt.Text + "',leaving_why='" + txtReason.Text.Trim().ToUpper() + "' where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and empcode='" + sg1.Rows[0].Cells[13].Text.Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        }

                        if (edmode.Value == "Y")
                        {
                            if (frm_formID == "F85156")
                            {
                                fgen.msg("-", "AMSG", "Master Updated,Employee Marked As Left Employee");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            }
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                            cmd_query = "delete from " + frm_tabname1 + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            // DELETING FROM SCRATCH2 AGAIN
                            cmd_query = "delete from " + frm_tabname1 + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                            cmd_query = "delete from pay_el where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                                if (frm_formID == "F85156")
                                {
                                    fgen.msg("-", "AMSG", "Master Updated,Employee Marked As Left Employee");
                                }
                                else
                                {
                                    fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
                                }
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); FileUpload1.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N"; btnsave.Disabled = false;
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
        sg1_dt.Columns.Add(new DataColumn("sg1_h11", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h13", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h14", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h16", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h20", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h21", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h22", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h23", typeof(string)));
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
            sg1.Columns[7].HeaderStyle.Width = 80;
            sg1.Columns[12].HeaderStyle.Width = 50;
            sg1.Columns[13].HeaderStyle.Width = 80;
            if (frm_cocd == "MLGI" || frm_cocd == "MLGA")
            {
                sg1.HeaderRow.Cells[32].Text = "OSA";
                sg1.HeaderRow.Cells[33].Text = "OT.Inc";
            }
            else if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
            {
                sg1.HeaderRow.Cells[32].Text = "OT";
                sg1.HeaderRow.Cells[33].Text = "Spl.OT";
            }
            //sg1.HeaderRow.Cells[37].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER1'", "ename");
            //sg1.HeaderRow.Cells[38].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER2'", "ename");
            //sg1.HeaderRow.Cells[39].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER3'", "ename");
            //sg1.HeaderRow.Cells[40].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER4'", "ename");
            //sg1.HeaderRow.Cells[41].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER5'", "ename");
            //sg1.HeaderRow.Cells[42].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER6'", "ename");
            //sg1.HeaderRow.Cells[43].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER7'", "ename");
            //sg1.HeaderRow.Cells[44].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER8'", "ename");
            //sg1.HeaderRow.Cells[45].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER9'", "ename");
            //sg1.HeaderRow.Cells[46].Text = "Ar:" + fgen.seek_iname(frm_qstr, frm_cocd, "select initcap(trim(ename)) as ename from selmas where grade='" + txtlbl4.Text.Trim() + "' and er='ER10'", "ename");

            //sg1.HeaderRow.Cells[37].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER1'", "ename");
            //sg1.HeaderRow.Cells[38].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER2'", "ename");
            //sg1.HeaderRow.Cells[39].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER3'", "ename");
            //sg1.HeaderRow.Cells[40].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER4'", "ename");
            //sg1.HeaderRow.Cells[41].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER5'", "ename");
            //sg1.HeaderRow.Cells[42].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER6'", "ename");
            //sg1.HeaderRow.Cells[43].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER7'", "ename");
            //sg1.HeaderRow.Cells[44].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER8'", "ename");
            //sg1.HeaderRow.Cells[45].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER9'", "ename");
            //sg1.HeaderRow.Cells[46].Text = "Ar:" + fgen.seek_iname_dt(dtEarning, "er='ER10'", "ename");
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
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                    //fgen.Fn_open_mseek("Select Item", frm_qstr);
                }
                break;

            case "SG1_ROW_ADD1":
                if (sg1.Rows[Convert.ToInt32(index)].Cells[13].Text.Trim().Length > 1)
                {
                    hf1.Value = index.ToString();
                    hffield.Value = "SG1_ROW_ADD1";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select item", frm_qstr);
                }
                else
                {
                    fgen.msg("-", "AMSG", "Please Select Stage First!!");
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
        hffield.Value = "SHFTCODE";
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
        hffield.Value = "DEPTTCODE";
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

        // EMPMAS
        dt2 = new DataTable();
        dt2 = fgen.getdata(frm_qstr, frm_cocd, "select branchcd,trim(grade) as grade,trim(empcode) as empcode,pfcut,esicut,cutvpf,er1,er2,er3,er4,er5,er6,er7,er8,er9,er10,er11,er12,er13,er14,er15,er16,er17,er18,er19,er20,ded1,ded2,ded3,ded4,ded5,ded6,ded7,ded8,ded9,ded10,ded11,ded12,ded13,ded14,ded15,ded16,ded17,ded18,ded19,ded20,wrkhour,leaving_dt,erpecode,deptt,d_o_b,branch_act,round(months_between(TRUNC(to_date('" + txtvchdate.Text + "','dd/mm/yyyy')),to_date(to_char(d_o_b,'dd/mm/yyyy'),'dd/mm/yyyy'))/12,2) as age from empmas where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' order by empcode");

        double Tot_Earnings = 0, ER_DIV1 = 0, OT_Div1 = 0, ER = 0, ERATEVAL = 0, Months = 0, Arr_Hrs = 0, Arr_WorkDays = 0, Arr_Totdays = 0, OverTime_Earnings = 0, OverTime_Earnings2 = 0; string ErateApp = "", ErateApp1 = ""; Arr_Month_Name = "";
        double ar1 = 0, ar2 = 0, ar3 = 0, ar4 = 0, ar5 = 0, ar6 = 0, ar7 = 0, ar8 = 0, ar9 = 0, ar10 = 0, ar11 = 0, ar12 = 0, ar13 = 0, ar14 = 0, ar15 = 0, ar16 = 0, ar17 = 0, ar18 = 0, ar19 = 0, ar20 = 0;

        // INCREMENT ENTRY
        mq2 = "select trim(empcode) as empcode,er1,er2,er3,er4,er5,er6,er7,er8,er9,er10,er11,er12,er13,er14,er15,er16,er17,er18,er19,er20,inc_app_dt,vchdate,trunc(months_between(to_date(vchdate,'dd/mm/yyyy'),to_date(inc_app_dt,'dd/mm/yyyy'))) as mon FROM PAYINCR WHERE branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and vchdate='" + txtvchdate.Text + "' and length(trim(empimg))>2";
        dt3 = new DataTable();
        dt3 = fgen.getdata(frm_qstr, frm_cocd, mq2);

        #region Earnings Formula
        string selvch = ""; mq3 = "";
        DataTable dtOT = new DataTable();
        dtOT = fgen.getdata(frm_qstr, frm_cocd, "select TRIM(ED_FLD) as ED_FLD,trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and ot_yn='Y' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/");
        string OT_Div = fgen.seek_iname(frm_qstr, frm_cocd, "select ot_div from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/", "ot_div");
        //string ER_Div = fgen.seek_iname(frm_qstr, frm_cocd, "select ern_div from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/", "ern_div");
        DataTable dt_ER_Div = new DataTable();
        dt_ER_Div = fgen.getdata(frm_qstr, frm_cocd, "select ern_div,trim(ed_fld) as ed_fld from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and nvl(icat,'-')!='Y' and ed_fld like 'ER%' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/ order by morder");
        string OverTime = fgen.seek_iname(frm_qstr, frm_cocd, "select ot_days from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/", "ot_days");

        DataTable dtOT2 = new DataTable();
        dtOT2 = fgen.getdata(frm_qstr, frm_cocd, "select TRIM(ED_FLD) as ED_FLD,trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and ot2_yn='Y' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/");
        string OT_Div2 = fgen.seek_iname(frm_qstr, frm_cocd, "select ot_div2 from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/", "ot_div2");
        string OverTime2 = fgen.seek_iname(frm_qstr, frm_cocd, "select ot_days2 from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/", "ot_days2");

        #endregion

        double PF_Limit = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select MAX_LMT from wb_selmast where ED_FLD='DED1'", "MAX_LMT"));
        Int32 Round_Off_Earnings = fgen.make_int(fgen.seek_iname(frm_qstr, frm_cocd, "select opt_param from fin_rsys_opt where opt_id='W0060'", "opt_param"));

        for (i = 0; i < sg1.Rows.Count; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum.Trim().ToUpper();
            oporow["DATE_"] = txtvchdate.Text.Trim().ToUpper();
            oporow["SRNO"] = i + 1;
            oporow["EMPCODE"] = sg1.Rows[i].Cells[13].Text.Trim().ToUpper();
            oporow["DEPTT"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim().ToUpper() + "'", "DEPTT");
            oporow["DEPTT2"] = "-";
            if (frm_formID == "F85156")
            {
                oporow["DEPTT1"] = "ONEBY";
            }
            else
            {
                oporow["DEPTT1"] = "-";
            }
            oporow["DESG"] = "-";
            oporow["GRADE"] = txtlbl4.Text.Trim().ToUpper();
            oporow["DESCGRD"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim().ToUpper();
            oporow["SECTION_"] = "-";
            oporow["TRADE"] = "N";
            oporow["ESINO"] = "-";
            oporow["TOTDAYS"] = txtlbl8.Text.Trim();
            oporow["PRESENT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper());
            oporow["SHL"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper());
            oporow["MNTS"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().ToUpper());
            oporow["CL"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper());
            oporow["EL"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper());
            oporow["SL"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper());
            oporow["LWP"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim().ToUpper());
            oporow["ESIL"] = 0;
            oporow["ABSENT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper());
            oporow["LATE"] = 0;
            oporow["HOURS"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper());
            oporow["OT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper());
            oporow["NIGHTS"] = 0;
            oporow["CPL"] = 0;
            oporow["GWA"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_h7")).Text.Trim().ToUpper());
            oporow["GWARR"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er1");
            oporow["SD"] = "-";
            oporow["PRDINC"] = 0;
            oporow["ESIGW"] = 0;
            oporow["PFCUT"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "pfcut");
            oporow["ESICUT"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "esicut");
            oporow["PFNO"] = "-";
            oporow["WORKDAYS"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_h7")).Text.Trim().ToUpper());
            oporow["ERATE1"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er1"));
            oporow["ERATE2"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er2"));
            oporow["ERATE3"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er3"));
            oporow["ERATE4"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er4"));
            oporow["ERATE5"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er5"));
            oporow["ERATE6"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er6"));
            oporow["ERATE7"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er7"));
            oporow["ERATE8"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er8"));

            if (frm_cocd == "MLGI" || frm_cocd == "MLGA")
            {
                if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim().ToUpper()) > 0)
                {
                    oporow["ERATE9"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim().ToUpper());
                }
                else
                {
                    oporow["ERATE9"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er9"));
                }
                if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim().ToUpper()) > 0)
                {
                    oporow["ERATE10"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim().ToUpper());
                }
                else
                {
                    oporow["ERATE10"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er10"));
                }
            }
            else
            {
                oporow["ERATE9"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er9"));
                oporow["ERATE10"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er10"));
            }
            oporow["ERATE11"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er11"));
            oporow["ERATE12"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er12"));
            oporow["ERATE13"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er13"));
            oporow["ERATE14"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er14"));
            oporow["ERATE15"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er15"));
            oporow["ERATE16"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er16"));
            oporow["ERATE17"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er17"));
            oporow["ERATE18"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er18"));
            oporow["ERATE19"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er19"));
            oporow["ERATE20"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er20"));
            oporow["DRATE1"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded1"));
            oporow["DRATE2"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded2"));
            oporow["DRATE3"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded3"));
            oporow["DRATE4"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded4"));
            oporow["DRATE5"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded5"));
            oporow["DRATE6"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded6"));
            oporow["DRATE7"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded7"));
            oporow["DRATE8"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded8"));
            oporow["DRATE9"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded9"));
            oporow["DRATE10"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded10"));
            oporow["DRATE11"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded11"));
            oporow["DRATE12"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded12"));
            oporow["DRATE13"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded13"));
            oporow["DRATE14"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded14"));
            oporow["DRATE15"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded15"));
            oporow["DRATE16"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded16"));
            oporow["DRATE17"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded17"));
            oporow["DRATE18"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded18"));
            oporow["DRATE19"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded19"));
            oporow["DRATE20"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded20"));

            // FOR CALCULATING EARNINGS
            OT_Div1 = 0; ER = 0; Tot_Earnings = 0; ar1 = 0; ar2 = 0; ar3 = 0; ar4 = 0; ar5 = 0; ar6 = 0; ar7 = 0; ar8 = 0; ar9 = 0; ar10 = 0; ar11 = 0; ar12 = 0; ar13 = 0; ar14 = 0; ar15 = 0; ar16 = 0; ar17 = 0; ar18 = 0; ar19 = 0; ar20 = 0; Arr_Hrs = 0; Arr_WorkDays = 0; Arr_Totdays = 0; Arr_Month_Name = "";
            //if (ER_Div == "TOTDAYS")
            //{
            //    ER_DIV1 = fgen.make_double(oporow["TOTDAYS"].ToString());
            //}
            //else if (ER_Div == "WORKDAYS")
            //{
            //    ER_DIV1 = fgen.make_double(oporow["WORKDAYS"].ToString());
            //}
            //else
            //{
            //    ER_DIV1 = fgen.make_double(ER_Div);
            //}
            if (OT_Div == "TOTDAYS")
            {
                OT_Div1 = fgen.make_double(oporow["TOTDAYS"].ToString());
            }
            else if (OT_Div == "WORKDAYS")
            {
                OT_Div1 = fgen.make_double(oporow["WORKDAYS"].ToString());
            }
            else
            {
                OT_Div1 = fgen.make_double(OT_Div);
            }
            for (int k = 1; k < 21; k++)
            {
                ERATEVAL = 0; ErateApp = ""; ER_DIV1 = 0;
                mq3 = fgen.seek_iname_dt(dt_ER_Div, "ED_FLD='ER" + k + "'", "ern_div");
                if (mq3 == "TOTDAYS")
                {
                    ER_DIV1 = fgen.make_double(oporow["TOTDAYS"].ToString());
                }
                else if (mq3 == "WORKDAYS")
                {
                    ER_DIV1 = fgen.make_double(oporow["WORKDAYS"].ToString());
                }
                else
                {
                    if (fgen.make_double(mq3) == 0)
                    {
                        ER_DIV1 = 1;
                    }
                    else
                    {
                        ER_DIV1 = fgen.make_double(mq3);
                    }
                }
                ErateApp = fgen.seek_iname_dt(dtOT, "ED_FLD='ER" + k + "'", "ED_FLD");
                if (ErateApp.Length > 1)
                {
                    ERATEVAL = fgen.make_double(oporow["ERATE" + k].ToString());
                }
                if (frm_cocd == "MLGI" || frm_cocd == "MLGA")
                {
                    if (k == 9)
                    {
                        if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim().ToUpper()) > 0)
                        {
                            ER = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim().ToUpper());
                        }
                        else
                        {
                            ER = Math.Round(fgen.make_double(oporow["ERATE" + k].ToString()) * (fgen.make_double(oporow["WORKDAYS"].ToString()) / ER_DIV1), Round_Off_Earnings) + Math.Round(fgen.make_double((ERATEVAL / OT_Div1).ToString().Replace("Infinity", "0").Replace("NaN", "0").Replace("∞", "0")) * fgen.make_double((fgen.make_double(oporow["OT"].ToString()) / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())).ToString().Replace("Infinity", "0").Replace("NaN", "0").Replace("∞", "0")), Round_Off_Earnings);
                        }
                    }
                    else if (k == 10)
                    {
                        if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim().ToUpper()) > 0)
                        {
                            ER = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim().ToUpper());
                        }
                        else
                        {
                            ER = Math.Round(fgen.make_double(oporow["ERATE" + k].ToString()) * (fgen.make_double(oporow["WORKDAYS"].ToString()) / ER_DIV1), Round_Off_Earnings) + Math.Round(fgen.make_double((ERATEVAL / OT_Div1).ToString().Replace("Infinity", "0").Replace("NaN", "0").Replace("∞", "0")) * fgen.make_double((fgen.make_double(oporow["OT"].ToString()) / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())).ToString().Replace("Infinity", "0").Replace("NaN", "0").Replace("∞", "0")), Round_Off_Earnings);
                        }
                    }
                    else
                    {
                        ER = Math.Round(fgen.make_double(oporow["ERATE" + k].ToString()) * (fgen.make_double(oporow["WORKDAYS"].ToString()) / ER_DIV1), Round_Off_Earnings) + Math.Round(fgen.make_double((ERATEVAL / OT_Div1).ToString().Replace("Infinity", "0").Replace("NaN", "0").Replace("∞", "0")) * fgen.make_double((fgen.make_double(oporow["OT"].ToString()) / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())).ToString().Replace("Infinity", "0").Replace("NaN", "0").Replace("∞", "0")), Round_Off_Earnings);
                    }
                }
                else
                {
                    ER = Math.Round(fgen.make_double(oporow["ERATE" + k].ToString()) * (fgen.make_double(oporow["WORKDAYS"].ToString()) / ER_DIV1), Round_Off_Earnings) + Math.Round(fgen.make_double((ERATEVAL / OT_Div1).ToString().Replace("Infinity", "0").Replace("NaN", "0").Replace("∞", "0")) * fgen.make_double((fgen.make_double(oporow["OT"].ToString()) / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())).ToString().Replace("Infinity", "0").Replace("NaN", "0").Replace("∞", "0")), Round_Off_Earnings);
                    if (ER > fgen.make_double(oporow["ERATE" + k].ToString()))
                    {
                        ER = fgen.make_double(oporow["ERATE" + k].ToString());
                    }
                }
                oporow["ER" + k] = ER;
                Tot_Earnings += ER;
            }
            //----------------------------
            oporow["GWAMNT"] = fgen.make_double(oporow["ER1"].ToString());

            // CALCULATING ARREAR
            if (dt3.Rows.Count > 0)
            {
                ER_DIV1 = 0; OT_Div1 = 0;
                Months = fgen.make_double(dt3.Rows[0]["mon"].ToString().Trim());
                for (int l = 0; l < Months; l++)
                {
                    if (l == 0)
                    {
                        ErateApp = dt3.Rows[0]["inc_app_dt"].ToString().Trim();
                    }
                    cmd_query = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(add_months(to_date('" + ErateApp + "','dd/mm/yyyy')," + l + "),'mm/yyyy') as arr_mth from payincr where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and vchdate='" + txtvchdate.Text + "' and length(trim(empimg))>2", "");
                    if (cmd_query.Length > 2)
                    {
                        Arr_Month_Name += "," + cmd_query.Substring(0, 2);
                    }
                    SQuery2 = "Select empcode,ERATE1,date_,workdays,totdays,hours from pay where trim(empcode)='" + sg1.Rows[i].Cells[13].Text.Trim() + "' and branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and (to_char(date_,'mm/yyyy')) ='" + cmd_query + "'";
                    dt4 = new DataTable();
                    dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery2);
                    foreach (DataRow dr in dt4.Rows)
                    {
                        Arr_Hrs = fgen.make_double(dr["hours"].ToString().Trim());
                        Arr_Totdays = fgen.make_double(dr["totdays"].ToString().Trim());
                        Arr_WorkDays = fgen.make_double(dr["workdays"].ToString().Trim());

                        //if (ER_Div == "TOTDAYS")
                        //{
                        //    ER_DIV1 = Arr_Totdays;
                        //}
                        //else if (ER_Div == "WORKDAYS")
                        //{
                        //    ER_DIV1 = Arr_WorkDays;
                        //}
                        //else
                        //{
                        //    ER_DIV1 = fgen.make_double(ER_Div);
                        //}                       
                        if (OT_Div == "TOTDAYS")
                        {
                            OT_Div1 = Arr_Totdays;
                        }
                        else if (OT_Div == "WORKDAYS")
                        {
                            OT_Div1 = Arr_WorkDays;
                        }
                        else
                        {
                            OT_Div1 = fgen.make_double(OT_Div);
                        }

                        for (int k = 1; k < 21; k++)
                        {
                            ERATEVAL = 0; ErateApp1 = ""; ER_DIV1 = 0;
                            mq3 = fgen.seek_iname_dt(dt_ER_Div, "ED_FLD='ER" + k + "'", "ern_div");
                            if (mq3 == "TOTDAYS")
                            {
                                ER_DIV1 = Arr_Totdays;
                            }
                            else if (mq3 == "WORKDAYS")
                            {
                                ER_DIV1 = Arr_WorkDays;
                            }
                            else
                            {
                                if (fgen.make_double(mq3) == 0)
                                {
                                    ER_DIV1 = 1;
                                }
                                else
                                {
                                    ER_DIV1 = fgen.make_double(mq3);
                                }
                            }
                            ErateApp1 = fgen.seek_iname_dt(dtOT, "ED_FLD='ER" + k + "'", "ED_FLD");
                            if (ErateApp1.Length > 1)
                            {
                                ERATEVAL = fgen.make_double(oporow["ERATE" + k].ToString());
                            }

                            switch (k)
                            {
                                case 1:
                                    ar1 = ar1 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er1")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 2:
                                    ar2 = ar2 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er2")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 3:
                                    ar3 = ar3 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er3")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 4:
                                    ar4 = ar4 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er4")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 5:
                                    ar5 = ar5 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er5")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 6:
                                    ar6 = ar6 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er6")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 7:
                                    ar7 = ar7 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er7")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 8:
                                    //ar8 = ar8 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er8")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er8")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    ar8 = ar8 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er8")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 9:
                                    ar9 = ar9 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er9")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 10:
                                    ar10 = ar10 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er10")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 11:
                                    ar11 = ar11 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er11")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 12:
                                    ar12 = ar12 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er12")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 13:
                                    ar13 = ar13 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er13")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 14:
                                    ar14 = ar14 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er14")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 15:
                                    ar15 = ar15 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er15")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 16:
                                    ar16 = ar16 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er16")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 17:
                                    ar17 = ar17 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er17")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 18:
                                    ar18 = ar18 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er18")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 19:
                                    ar19 = ar19 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er19")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                                case 20:
                                    ar20 = ar20 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er20")) * (Arr_WorkDays / ER_DIV1), Round_Off_Earnings) + Math.Round((ERATEVAL / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), Round_Off_Earnings);
                                    break;
                            }
                        }

                        //ar1 = ar1 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er1")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er1")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar2 = ar2 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er2")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er2")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar3 = ar3 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er3")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er3")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar4 = ar4 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er4")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er4")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar5 = ar5 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er5")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er5")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar6 = ar6 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er6")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er6")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar7 = ar7 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er7")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er7")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar8 = ar8 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er8")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er8")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar9 = ar9 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er9")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er9")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar10 = ar10 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er10")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er10")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar11 = ar11 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er11")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er11")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar12 = ar12 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er12")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er12")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar13 = ar13 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er13")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er13")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar14 = ar14 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er14")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er14")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar15 = ar15 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er15")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er15")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar16 = ar16 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er16")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er16")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar17 = ar17 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er17")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er17")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar18 = ar18 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er18")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er18")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar19 = ar19 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er19")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er19")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                        //ar20 = ar20 + Math.Round(fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er20")) * (Arr_WorkDays / ER_DIV1), 2) + Math.Round((fgen.make_double(fgen.seek_iname_dt(dt3, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "er20")) / OT_Div1) * (Arr_Hrs / fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim())), 2);
                    }
                }
            }
            oporow["AR1"] = ar1;
            oporow["AR2"] = ar2;
            oporow["AR3"] = ar3;
            oporow["AR4"] = ar4;
            oporow["AR5"] = ar5;
            oporow["AR6"] = ar6;
            oporow["AR7"] = ar7;
            oporow["AR8"] = ar8;
            oporow["AR9"] = ar9;
            if (frm_formID == "F85156")
            {
                ar10 = 0; ar11 = 0; ar12 = 0; ar13 = 0; ar14 = 0; ar15 = 0; ar16 = 0; ar17 = 0; ar18 = 0;
                oporow["AR10"] = fgen.make_double(txtLTA.Text.Trim());
                oporow["AR11"] = fgen.make_double(txtMedical.Text.Trim());
                oporow["AR12"] = fgen.make_double(txtGratuity_Yrs.Text.Trim());
                oporow["AR13"] = fgen.make_double(txtGratuity_Rs.Text.Trim());
                oporow["AR14"] = fgen.make_double(txtEL_Days.Text.Trim());
                oporow["AR15"] = fgen.make_double(txtEL_Rs.Text.Trim());
                oporow["AR16"] = fgen.make_double(txtNotice_Rs.Text.Trim());
                oporow["AR17"] = fgen.make_double(txtOthers.Text.Trim());
                oporow["AR18"] = fgen.make_double(txtBonus_Amt.Text.Trim());
                oporow["NPDAYS"] = fgen.make_double(txtNotice_Days.Text.Trim());
                oporow["BON_RATE"] = fgen.make_double(txtBonus_Per.Text.Trim());
                oporow["LEAVING_DT"] = txtLeaving_Dt.Text.Trim();
                oporow["LEAVING_WHY"] = txtReason.Text.Trim().ToUpper();
            }
            else
            {
                oporow["AR10"] = ar10;
                oporow["AR11"] = ar11;
                oporow["AR12"] = ar12;
                oporow["AR13"] = ar13;
                oporow["AR14"] = ar14;
                oporow["AR15"] = ar15;
                oporow["AR16"] = ar16;
                oporow["AR17"] = ar17;
                oporow["AR18"] = ar18;
                oporow["NPDAYS"] = 0;
                oporow["BON_RATE"] = 0;
                oporow["LEAVING_DT"] = sg1.Rows[i].Cells[52].Text.Trim().ToUpper();
                oporow["LEAVING_WHY"] = "-";
            }
            oporow["AR19"] = ar19;
            oporow["AR20"] = ar20;
            //-------------------------------

            oporow["DED1"] = 0;
            oporow["DED2"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "ded2"));
            oporow["DED3"] = 0;
            oporow["DED4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim().ToUpper());
            oporow["DED5"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim().ToUpper());
            oporow["DED6"] = 0;
            oporow["DED7"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim().ToUpper());
            oporow["DED8"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim().ToUpper());
            oporow["DED9"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim().ToUpper());
            oporow["DED10"] = 0;
            oporow["DED11"] = 0;
            oporow["DED12"] = 0;
            oporow["DED13"] = 0;
            oporow["DED14"] = 0;
            oporow["DED15"] = 0;
            oporow["DED16"] = 0;
            oporow["DED17"] = 0;
            oporow["DED18"] = 0;
            oporow["DED19"] = 0;
            oporow["DED20"] = 0;
            oporow["TOTERN"] = Tot_Earnings + ar1 + ar2 + ar3 + ar4 + ar5 + ar6 + ar7 + ar8 + ar9 + ar10 + ar11 + ar12 + ar13 + ar14 + ar15 + ar16 + ar17 + ar18 + ar19 + ar20;
            oporow["TOTDED"] = 0;
            oporow["NETSLRY"] = 0;
            oporow["COINS"] = 0;//to be asked
            oporow["ATINC"] = 0;
            oporow["ESI"] = 0;
            oporow["CEPF"] = 0;
            oporow["STATUS"] = sg1.Rows[i].Cells[49].Text.Trim().ToUpper().Replace("&nbsp;", "-").Replace("&AMP;", "-").Replace("-;AMP;AMP;NBSP;", "-");
            oporow["OFFDAYS"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper());
            oporow["CUTVPF"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "cutvpf");
            oporow["TOTSAL"] = fgen.make_double(oporow["ERATE1"].ToString()) + fgen.make_double(oporow["ERATE2"].ToString()) + fgen.make_double(oporow["ERATE3"].ToString()) + fgen.make_double(oporow["ERATE4"].ToString()) + fgen.make_double(oporow["ERATE5"].ToString()) + fgen.make_double(oporow["ERATE6"].ToString()) + fgen.make_double(oporow["ERATE7"].ToString()) + fgen.make_double(oporow["ERATE8"].ToString()) + fgen.make_double(oporow["ERATE9"].ToString()) + fgen.make_double(oporow["ERATE10"].ToString()) + fgen.make_double(oporow["ERATE11"].ToString()) + fgen.make_double(oporow["ERATE12"].ToString()) + fgen.make_double(oporow["ERATE13"].ToString()) + fgen.make_double(oporow["ERATE14"].ToString()) + fgen.make_double(oporow["ERATE15"].ToString()) + fgen.make_double(oporow["ERATE16"].ToString()) + fgen.make_double(oporow["ERATE17"].ToString()) + fgen.make_double(oporow["ERATE18"].ToString()) + fgen.make_double(oporow["ERATE19"].ToString()) + fgen.make_double(oporow["ERATE20"].ToString());
            oporow["ADVANCE"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim().ToUpper());
            oporow["WRKHRS"] = fgen.make_double(sg1.Rows[i].Cells[58].Text.Trim().ToUpper());
            oporow["TDS"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim().ToUpper());
            oporow["VONC"] = 0;//to be asked
            OverTime_Earnings = 0; OverTime_Earnings2 = 0;
            for (z = 0; z < dtOT.Rows.Count; z++)
            {
                OverTime_Earnings += fgen.make_double(oporow[dtOT.Rows[z]["ed_fld"].ToString().Trim().Replace("ER", "ERATE")].ToString());
            }

            for (int k = 0; k < dtOT2.Rows.Count; k++)
            {
                OverTime_Earnings2 += fgen.make_double(oporow[dtOT2.Rows[k]["ed_fld"].ToString().Trim().Replace("ER", "ERATE")].ToString());
            }

            if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper()) == 0 && OverTime_Earnings == 0)
            {
                OverTime_Earnings = 0;
            }
            if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t31")).Text.Trim().ToUpper()) == 0 && OverTime_Earnings2 == 0)
            {
                OverTime_Earnings2 = 0;
            }
            if (fgen.make_double(OT_Div) == 0)
            {
                OT_Div = "1";
            }
            if (fgen.make_double(OT_Div2) == 0)
            {
                OT_Div2 = "1";
            }
            oporow["VERO"] = Math.Round(OverTime_Earnings / fgen.make_double(OT_Div) * fgen.make_double(OverTime) * fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper()), 2);
            oporow["VERO2"] = Math.Round(OverTime_Earnings2 / fgen.make_double(OT_Div2) * fgen.make_double(OverTime2) * fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t31")).Text.Trim().ToUpper()), 2);
            oporow["VERO_ON"] = Math.Round(OverTime_Earnings / fgen.make_double(OT_Div) * fgen.make_double(OverTime), 2);
            oporow["VERO_ON2"] = Math.Round(OverTime_Earnings2 / fgen.make_double(OT_Div2) * fgen.make_double(OverTime2), 2);
            oporow["HOURS2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t31")).Text.Trim().ToUpper());
            oporow["REFR"] = 0;
            oporow["OTH2"] = fgen.make_double(oporow["er2"].ToString());
            oporow["OTH3"] = fgen.make_double(oporow["er3"].ToString());
            oporow["ROTH2"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er2"));
            oporow["ROTH3"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "er3"));
            oporow["SALHOLD"] = "-";
            oporow["SALPAID"] = "-";
            oporow["ERPECODE"] = sg1.Rows[i].Cells[48].Text.Trim().ToUpper().Replace("&nbsp;", "-");
            oporow["ADDLESI"] = 0;
            oporow["CALC_PF_WG"] = 0;
            oporow["PADVBAL"] = 0;
            oporow["COFF"] = 0;
            oporow["BON_ELIG"] = 0;
            oporow["AR21"] = 0;
            oporow["TNUM"] = 0;
            oporow["TNUM2"] = 0;
            oporow["LOAN_DED"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim().ToUpper());
            oporow["TMJ"] = sg1.Rows[i].Cells[50].Text.Trim().Replace("&nbsp;", "-").ToUpper();
            oporow["TML"] = "N";
            oporow["LTA"] = 0;
            oporow["AR22"] = 0;
            oporow["MLEV"] = 0;
            oporow["LMT_PF"] = PF_Limit;
            oporow["LEAVING_TXT"] = "-";
            oporow["BONMINWG"] = 0;
            oporow["ESI_WAGE"] = 0;
            oporow["AGE"] = fgen.make_double(fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "age"));
            oporow["SELVCH"] = selvch;
            oporow["ent_by"] = frm_uname;
            oporow["ent_date"] = vardate;
            oporow["branch_act"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'", "branch_act");
            //if (edmode.Value == "Y")
            //{
            //    oporow["ent_by"] = ViewState["entby"].ToString();
            //    oporow["ent_date"] = ViewState["entdt"].ToString();
            //    //oporow["edt_by"] = frm_uname;
            //    //oporow["edt_dt"] = vardate;
            //}
            //else
            //{
            //    oporow["ent_by"] = frm_uname;
            //    oporow["ent_date"] = vardate;
            //    //oporow["edt_by"] = "-";
            //    //oporow["edt_dt"] = vardate;
            //}
            oDS.Tables[0].Rows.Add(oporow);

            if (frm_vnum != "000000")
            {
                SQuery = "";
                SQuery = "insert into pay_el(day4calc,branchcd,type,vchnum,vchdate,grade,empcode,el_accru,cl_accru,sl_accru) values (" + fgen.make_double(oporow["WORKDAYS"].ToString().Trim()) + ",'" + frm_mbr + "','" + frm_vty + "','" + txtvchnum.Text.Trim() + "',to_Date('" + txtvchdate.Text + "','dd/mm/yyyy'),'" + txtlbl4.Text.Trim().ToUpper() + "','" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'," + Math.Round(fgen.make_double(oporow["WORKDAYS"].ToString().Trim()) / 20, 2) + "," + Math.Round(fgen.make_double(oporow["WORKDAYS"].ToString().Trim()) / 43, 2) + "," + Math.Round(fgen.make_double(oporow["WORKDAYS"].ToString().Trim()) / 43, 2) + ")";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
            }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (i = 0; i < sg1.Rows.Count; i++)
        {
            oporow2 = oDS2.Tables[0].NewRow();
            oporow2["branchcd"] = frm_mbr;
            oporow2["type"] = frm_vty;
            oporow2["vchnum"] = frm_vnum.Trim().ToUpper();
            oporow2["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow2["srno"] = i + 1;
            oporow2["acode"] = sg1.Rows[i].Cells[13].Text.Trim().ToUpper();
            oporow2["col1"] = txtlbl4.Text.Trim().ToUpper();
            oporow2["icode"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper());
            oporow2["num1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper());
            oporow2["num2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper());
            oporow2["num3"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper());
            oporow2["num4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper());
            oporow2["num5"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper());

            if (edmode.Value == "Y")
            {
                oporow2["ent_by"] = ViewState["entby"].ToString();
                oporow2["ent_dt"] = ViewState["entdt"].ToString();
            }
            else
            {
                oporow2["ent_by"] = frm_uname;
                oporow2["ent_dt"] = vardate;
            }
            oDS2.Tables[0].Rows.Add(oporow2);
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun3()
    {

    }
    //------------------------------------------------------------------------------------
    void save_fun4()
    {
        string PF_ER = "", ESI_ER = "", WF_ER = "", PF_YN = "", ESI_YN = "", Apply_15000_PFLimit = "", selvch = ""; double Tot_Earnings = 0, Tot_Ded = 0, Net_Sal = 0, Tot_Sal = 0;
        double Amt_Of_PF = 0, WFWage = 0, DED1 = 0, DED3 = 0, DED6 = 0, DED10 = 0, Amt_For_WF = 0, PT = 0, Amt_of_WF = 0; string PT_ER = ""; bool Pass = false;
        double PF_AMT_CS = 0, PF_RT_CS = 0, PF_RT_ES = 0, PF_SAL = 0, ESI_SAL = 0, ESI_RT_CS = 0, ESI_RT_ES = 0, ESI_AMT_CS = 0, WF_AMT_CS = 0, WF_RT_CS = 0, WF_RT_ES = 0;
        string Show_OT = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_enable from fin_rsys_opt where opt_id='W0059'", "opt_enable");
        Int32 NetSlry_RoundOff = fgen.make_int(fgen.seek_iname(frm_qstr, frm_cocd, "select opt_param from fin_rsys_opt where opt_id='W0061'", "opt_param"));

        #region
        dt2 = new DataTable();
        dt2 = fgen.getdata(frm_qstr, frm_cocd, "select trim(empcode) as empcode,mnthinc from empmas where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text + "'");

        DataTable dtPF_ER = new DataTable();
        dtPF_ER = fgen.getdata(frm_qstr, frm_cocd, "select rtrim(xmlagg(xmlelement(e,replace(TRIM(ED_FLD) ,'-',null)||'+')).extract('//text()').extract('//text()'),'+') as earnings,trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and pf_yn='Y' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/ group by trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')");

        DataTable dtSelmas = new DataTable();
        dtSelmas = fgen.getdata(frm_qstr, frm_cocd, "SELECT RATE,PF_DIV,ED_FLD,ED_NAME,ESI_DIV,WF_DIV,EMPR_RATE,MAX_LMT,trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr FROM WB_SELMAST WHERE branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and ed_fld like 'DED%' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/ order by morder");

        if (dtPF_ER.Rows.Count > 0)
        {
            //PF_ER = dtPF_ER.Rows[0]["earnings"].ToString().Trim().Replace("ER", "ERATE");
            PF_ER = dtPF_ER.Rows[0]["earnings"].ToString().Trim();
            if (PF_ER == "")
            {
                PF_ER = "0";
            }
        }
        else
        {
            PF_ER = "0";
        }
        string PF_Div = fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "PF_DIV");
        double PF_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "RATE"));
        double PF_Empr_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "EMPR_RATE"));
        double PF_Limit = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "MAX_LMT"));
        //string PF_Formula = "round(((" + PF_ER + ")/" + PF_Div + ")*WORKDAYS,2) as DED1"; // DONE ON THE INSTRUCTIONS OF PUNEET SIR
        string PF_Formula = "round((" + PF_ER + "),2) as DED1";
        selvch = fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED1'", "FSTR");

        DataTable dtESI_ER = new DataTable();
        dtESI_ER = fgen.getdata(frm_qstr, frm_cocd, "select rtrim(xmlagg(xmlelement(e,replace(TRIM(ED_FLD) ,'-',null)||'+')).extract('//text()').extract('//text()'),'+') as earnings from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and esi_yn='Y' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/");

        if (dtESI_ER.Rows.Count > 0)
        {
            ESI_ER = dtESI_ER.Rows[0]["earnings"].ToString().Trim();
            if (ESI_ER == "")
            {
                ESI_ER = "0";
            }
        }
        else
        {
            ESI_ER = "0";
        }
        string ESI_Div = fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED3'", "ESI_DIV");
        double ESI_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED3'", "RATE"));
        double ESI_Empr_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED3'", "EMPR_RATE"));
        //string ESI_Formula = "round(((" + ESI_ER + ")/" + ESI_Div + ")*WORKDAYS,2) as DED3"; // DONE ON THE INSTRUCTIONS OF MAYURI MAM
        string ESI_Formula = "round((" + ESI_ER + "),2) as DED3";

        DataTable dtWF_ER = new DataTable();
        dtWF_ER = fgen.getdata(frm_qstr, frm_cocd, "select rtrim(xmlagg(xmlelement(e,replace(TRIM(ED_FLD) ,'-',null)||'+')).extract('//text()').extract('//text()'),'+') as earnings from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and wf_yn='Y' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/");
        if (dtWF_ER.Rows.Count > 0)
        {
            WF_ER = dtWF_ER.Rows[0]["earnings"].ToString().Trim();
            if (WF_ER == "")
            {
                WF_ER = "0";
            }
        }
        else
        {
            WF_ER = "0";
        }
        string WF_Div = fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED6'", "WF_DIV");
        double WF_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED6'", "RATE"));
        double WF_Empr_Rate = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED6'", "EMPR_RATE"));
        double WF_Limit = fgen.make_double(fgen.seek_iname_dt(dtSelmas, "ED_FLD='DED6'", "MAX_LMT"));
        //string WF_Formula = "round(((" + WF_ER + ")/" + WF_Div + ")*WORKDAYS,2) as DED6"; // DONE ON THE INSTRUCTIONS OF PUNEET SIR
        string WF_Formula = "round((" + WF_ER + "),2) as DED6";

        string state = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(b.type1) as code,a.statenm from type a, type b where trim(a.statenm)=trim(b.name) and a.id='B' and a.type1='" + frm_mbr + "' and b.id='{'", "code");

        DataTable dtPT = new DataTable();
        dtPT = fgen.getdata(frm_qstr, frm_cocd, "select a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.ed_fld,a.sal_frm,a.sal_upto,a.mth01,a.mth02,a.mth03,a.mth04,a.mth05,a.mth06,a.mth07,a.mth08,a.mth09,a.mth10,a.mth11,a.mth12 from WB_PTAX a where a.branchcd='" + frm_mbr + "' and a.type='11' and a.grade='" + txtlbl4.Text.Trim() + "' and a.staten='" + state.ToString() + "' and nvl(icat,'-')!='Y' order by morder");

        DataTable dtPT_ER = new DataTable();
        dtPT_ER = fgen.getdata(frm_qstr, frm_cocd, "select rtrim(xmlagg(xmlelement(e,replace(TRIM(ED_FLD) ,'-',null)||'+')).extract('//text()').extract('//text()'),'+') as earnings,trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and pt_yn='Y' and nvl(icat,'-')!='Y' /*and to_char(eff_from,'mm/yyyy') ='" + txtvchdate.Text.Substring(3, 7) + "' and to_char(eff_to,'mm/yyyy')='" + txtvchdate.Text.Substring(3, 7) + "'*/ group by trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')");
        if (dtPT_ER.Rows.Count > 0)
        {
            PT_ER = dtPT_ER.Rows[0]["earnings"].ToString().Trim();
            if (PT_ER == "")
            {
                PT_ER = "0";
            }
        }
        else
        {
            PT_ER = "0";
        }
        #endregion

        double er1 = 0, er2 = 0, er3 = 0, er4 = 0, er5 = 0, er6 = 0, er7 = 0, er8 = 0, er9 = 0, er10 = 0, er11 = 0, er12 = 0, er13 = 0, er14 = 0, er15 = 0, er16 = 0, er17 = 0, er18 = 0, er19 = 0, er20 = 0;
        double erate1 = 0, erate2 = 0, erate3 = 0, erate4 = 0, erate5 = 0, erate6 = 0, erate7 = 0, erate8 = 0, erate9 = 0, erate10 = 0, erate11 = 0, erate12 = 0, erate13 = 0, erate14 = 0, erate15 = 0, erate16 = 0, erate17 = 0, erate18 = 0, erate19 = 0, erate20;
        double ar1 = 0, ar2 = 0, ar3 = 0, ar4 = 0, ar5 = 0, ar6 = 0, ar7 = 0, ar8 = 0, ar9 = 0, ar10 = 0, ar11 = 0, ar12 = 0, ar13 = 0, ar14 = 0, ar15 = 0, ar16 = 0, ar17 = 0, ar18 = 0, ar19 = 0, ar20 = 0;

        SQuery1 = ""; double AMT_FOR_PF = 0, DED2 = 0, DED4 = 0, DED5 = 0, DED7 = 0, DED8 = 0, DED9 = 0, DED11 = 0, DED12 = 0, DED13 = 0, DED14 = 0, DED15 = 0, DED16 = 0, DED17 = 0, DED18 = 0, DED19 = 0, DED20 = 0;
        double pr_vero = 0, pr_vero2 = 0, pr_vero_on = 0, pr_vero_on2 = 0;
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            SQuery = "select " + PF_Formula + "," + ESI_Formula + "," + WF_Formula + "," + PT_ER + " as PT,trim(empcode) as empcode,pfcut,esicut,cutvpf,totern,ded2,ded4,ded5,ded7,ded8,ded9,ded11,ded12,ded13,ded14,ded15,ded16,ded17,ded18,ded19,ded20,vero,vero2,vero_on,vero_on2,er1,er2,er3,er4,er5,er6,er7,er8,er9,er10,er11,er12,er13,er14,er15,er16,er17,er18,er19,er20,erate1,erate2,erate3,erate4,erate5,erate6,erate7,erate8,erate9,erate10,erate11,erate12,erate13,erate14,erate15,erate16,erate17,erate18,erate19,erate20,ar1,ar2,ar3,ar4,ar5,ar6,ar7,ar8,ar9,ar10,ar11,ar12,ar13,ar14,ar15,ar16,ar17,ar18,ar19,ar20 from pay where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text + "' and vchnum='" + txtvchnum.Text + "' and to_char(date_,'dd/mm/yyyy')='" + txtvchdate.Text + "' and empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            Tot_Ded = 0; Net_Sal = 0; DED1 = 0; DED3 = 0; DED6 = 0; PF_YN = ""; ESI_YN = ""; Tot_Earnings = 0; Amt_Of_PF = 0; WFWage = 0; DED10 = 0; AMT_FOR_PF = 0; DED2 = 0; DED4 = 0; DED5 = 0; DED7 = 0; DED8 = 0; DED9 = 0;
            DED11 = 0; DED12 = 0; DED13 = 0; DED14 = 0; DED15 = 0; DED16 = 0; DED17 = 0; DED18 = 0; DED19 = 0; DED20 = 0; Amt_For_WF = 0; Amt_of_WF = 0; pr_vero = 0; pr_vero2 = 0; pr_vero_on = 0; pr_vero_on2 = 0;
            PF_AMT_CS = 0; PF_RT_CS = 0; PF_RT_ES = 0; PF_SAL = 0; ESI_SAL = 0; ESI_RT_CS = 0; ESI_RT_ES = 0; ESI_AMT_CS = 0; WF_AMT_CS = 0; WF_RT_CS = 0; WF_RT_ES = 0;
            er1 = 0; er2 = 0; er3 = 0; er4 = 0; er5 = 0; er6 = 0; er7 = 0; er8 = 0; er9 = 0; er10 = 0; er11 = 0; er12 = 0; er13 = 0; er14 = 0; er15 = 0; er16 = 0; er17 = 0; er18 = 0; er19 = 0; er20 = 0;
            erate1 = 0; erate2 = 0; erate3 = 0; erate4 = 0; erate5 = 0; erate6 = 0; erate7 = 0; erate8 = 0; erate9 = 0; erate10 = 0; erate11 = 0; erate12 = 0; erate13 = 0; erate14 = 0; erate15 = 0; erate16 = 0; erate17 = 0; erate18 = 0; erate19 = 0; erate20 = 0;
            ar1 = 0; ar2 = 0; ar3 = 0; ar4 = 0; ar5 = 0; ar6 = 0; ar7 = 0; ar8 = 0; ar9 = 0; ar10 = 0; ar11 = 0; ar12 = 0; ar13 = 0; ar14 = 0; ar15 = 0; ar16 = 0; ar17 = 0; ar18 = 0; ar19 = 0; ar20 = 0;

            if (dt.Rows.Count > 0)
            {
                #region
                Amt_For_WF = fgen.make_double(dt.Rows[0]["ded6"].ToString().Trim());
                PF_YN = dt.Rows[0]["pfcut"].ToString().Trim();
                ESI_YN = dt.Rows[0]["esicut"].ToString().Trim();
                Tot_Earnings = fgen.make_double(dt.Rows[0]["totern"].ToString().Trim());
                PT = fgen.make_double(dt.Rows[0]["PT"].ToString().Trim());
                AMT_FOR_PF = fgen.make_double(dt.Rows[0]["ded1"].ToString().Trim());
                DED2 = fgen.make_double(dt.Rows[0]["ded2"].ToString().Trim());
                DED4 = fgen.make_double(dt.Rows[0]["ded4"].ToString().Trim());
                DED5 = fgen.make_double(dt.Rows[0]["ded5"].ToString().Trim());
                DED7 = fgen.make_double(dt.Rows[0]["ded7"].ToString().Trim());
                DED8 = fgen.make_double(dt.Rows[0]["ded8"].ToString().Trim());
                DED9 = fgen.make_double(dt.Rows[0]["ded9"].ToString().Trim());
                DED11 = fgen.make_double(dt.Rows[0]["ded11"].ToString().Trim());
                DED12 = fgen.make_double(dt.Rows[0]["ded12"].ToString().Trim());
                DED13 = fgen.make_double(dt.Rows[0]["ded13"].ToString().Trim());
                DED14 = fgen.make_double(dt.Rows[0]["ded14"].ToString().Trim());
                DED15 = fgen.make_double(dt.Rows[0]["ded15"].ToString().Trim());
                DED16 = fgen.make_double(dt.Rows[0]["ded16"].ToString().Trim());
                DED17 = fgen.make_double(dt.Rows[0]["ded17"].ToString().Trim());
                DED18 = fgen.make_double(dt.Rows[0]["ded18"].ToString().Trim());
                DED19 = fgen.make_double(dt.Rows[0]["ded19"].ToString().Trim());
                DED20 = fgen.make_double(dt.Rows[0]["ded20"].ToString().Trim());
                pr_vero = fgen.make_double(dt.Rows[0]["vero"].ToString().Trim());
                pr_vero_on = fgen.make_double(dt.Rows[0]["vero_on"].ToString().Trim());
                pr_vero2 = fgen.make_double(dt.Rows[0]["vero2"].ToString().Trim());
                pr_vero_on2 = fgen.make_double(dt.Rows[0]["vero_on2"].ToString().Trim());
                er1 = fgen.make_double(dt.Rows[0]["er1"].ToString().Trim());
                er2 = fgen.make_double(dt.Rows[0]["er2"].ToString().Trim());
                er3 = fgen.make_double(dt.Rows[0]["er3"].ToString().Trim());
                er4 = fgen.make_double(dt.Rows[0]["er4"].ToString().Trim());
                er5 = fgen.make_double(dt.Rows[0]["er5"].ToString().Trim());
                er6 = fgen.make_double(dt.Rows[0]["er6"].ToString().Trim());
                er7 = fgen.make_double(dt.Rows[0]["er7"].ToString().Trim());
                er8 = fgen.make_double(dt.Rows[0]["er8"].ToString().Trim());
                er9 = fgen.make_double(dt.Rows[0]["er9"].ToString().Trim());
                er10 = fgen.make_double(dt.Rows[0]["er10"].ToString().Trim());
                er11 = fgen.make_double(dt.Rows[0]["er11"].ToString().Trim());
                er12 = fgen.make_double(dt.Rows[0]["er12"].ToString().Trim());
                er13 = fgen.make_double(dt.Rows[0]["er13"].ToString().Trim());
                er14 = fgen.make_double(dt.Rows[0]["er14"].ToString().Trim());
                er15 = fgen.make_double(dt.Rows[0]["er15"].ToString().Trim());
                er16 = fgen.make_double(dt.Rows[0]["er16"].ToString().Trim());
                er17 = fgen.make_double(dt.Rows[0]["er17"].ToString().Trim());
                er18 = fgen.make_double(dt.Rows[0]["er18"].ToString().Trim());
                er19 = fgen.make_double(dt.Rows[0]["er19"].ToString().Trim());
                er20 = fgen.make_double(dt.Rows[0]["er20"].ToString().Trim());
                erate1 = fgen.make_double(dt.Rows[0]["erate1"].ToString().Trim());
                erate2 = fgen.make_double(dt.Rows[0]["erate2"].ToString().Trim());
                erate3 = fgen.make_double(dt.Rows[0]["erate3"].ToString().Trim());
                erate4 = fgen.make_double(dt.Rows[0]["erate4"].ToString().Trim());
                erate5 = fgen.make_double(dt.Rows[0]["erate5"].ToString().Trim());
                erate6 = fgen.make_double(dt.Rows[0]["erate6"].ToString().Trim());
                erate7 = fgen.make_double(dt.Rows[0]["erate7"].ToString().Trim());
                erate8 = fgen.make_double(dt.Rows[0]["erate8"].ToString().Trim());
                erate9 = fgen.make_double(dt.Rows[0]["erate9"].ToString().Trim());
                erate10 = fgen.make_double(dt.Rows[0]["erate10"].ToString().Trim());
                erate11 = fgen.make_double(dt.Rows[0]["erate11"].ToString().Trim());
                erate12 = fgen.make_double(dt.Rows[0]["erate12"].ToString().Trim());
                erate13 = fgen.make_double(dt.Rows[0]["erate13"].ToString().Trim());
                erate14 = fgen.make_double(dt.Rows[0]["erate14"].ToString().Trim());
                erate15 = fgen.make_double(dt.Rows[0]["erate15"].ToString().Trim());
                erate16 = fgen.make_double(dt.Rows[0]["erate16"].ToString().Trim());
                erate17 = fgen.make_double(dt.Rows[0]["erate17"].ToString().Trim());
                erate18 = fgen.make_double(dt.Rows[0]["erate18"].ToString().Trim());
                erate19 = fgen.make_double(dt.Rows[0]["erate19"].ToString().Trim());
                erate20 = fgen.make_double(dt.Rows[0]["erate20"].ToString().Trim());
                ar1 = fgen.make_double(dt.Rows[0]["ar1"].ToString().Trim());
                ar2 = fgen.make_double(dt.Rows[0]["ar2"].ToString().Trim());
                ar3 = fgen.make_double(dt.Rows[0]["ar3"].ToString().Trim());
                ar4 = fgen.make_double(dt.Rows[0]["ar4"].ToString().Trim());
                ar5 = fgen.make_double(dt.Rows[0]["ar5"].ToString().Trim());
                ar6 = fgen.make_double(dt.Rows[0]["ar6"].ToString().Trim());
                ar7 = fgen.make_double(dt.Rows[0]["ar7"].ToString().Trim());
                ar8 = fgen.make_double(dt.Rows[0]["ar8"].ToString().Trim());
                ar9 = fgen.make_double(dt.Rows[0]["ar9"].ToString().Trim());
                ar10 = fgen.make_double(dt.Rows[0]["ar10"].ToString().Trim());
                ar11 = fgen.make_double(dt.Rows[0]["ar11"].ToString().Trim());
                ar12 = fgen.make_double(dt.Rows[0]["ar12"].ToString().Trim());
                ar13 = fgen.make_double(dt.Rows[0]["ar13"].ToString().Trim());
                ar14 = fgen.make_double(dt.Rows[0]["ar14"].ToString().Trim());
                ar15 = fgen.make_double(dt.Rows[0]["ar15"].ToString().Trim());
                ar16 = fgen.make_double(dt.Rows[0]["ar16"].ToString().Trim());
                ar17 = fgen.make_double(dt.Rows[0]["ar17"].ToString().Trim());
                ar18 = fgen.make_double(dt.Rows[0]["ar18"].ToString().Trim());
                ar19 = fgen.make_double(dt.Rows[0]["ar19"].ToString().Trim());
                ar20 = fgen.make_double(dt.Rows[0]["ar20"].ToString().Trim());
                #endregion
            }

            // THIS IF CONDITION IS WRITTEN FOR TESTING PURPOSE
            //if (sg1.Rows[i].Cells[13].Text.Trim() == "010155")
            //{

            //}

            #region PF
            if (PF_YN == "Y")
            {
                Apply_15000_PFLimit = fgen.seek_iname_dt(dt2, "empcode='" + sg1.Rows[i].Cells[7].Text.Trim() + "'", "mnthinc");
                //if (Apply_15000_PFLimit == "1")
                //{
                //    if (Tot_Earnings > PF_Limit)
                //    {
                //        PFWage = PF_Limit;
                //    }
                //    else
                //    {
                //        PFWage = Tot_Earnings;
                //    }
                //}
                //else
                //{
                //    PFWage = Tot_Earnings;
                //}
                if (Apply_15000_PFLimit == "1")
                {
                    if (AMT_FOR_PF > PF_Limit)
                    {
                        Amt_Of_PF = PF_Limit;
                    }
                    else
                    {
                        Amt_Of_PF = AMT_FOR_PF;
                    }
                }
                else
                {
                    Amt_Of_PF = AMT_FOR_PF;
                }
                //PF_RT_ES = PF_Rate * 100;
                //PF_RT_CS = PF_Empr_Rate * 100;
                //PF_SAL = Amt_Of_PF;
                //DED1 = Math.Ceiling(Amt_Of_PF * PF_Rate);
                //PF_AMT_CS = Math.Ceiling(Amt_Of_PF * PF_Empr_Rate);
                //-----------------------------------
                PF_RT_ES = PF_Rate;
                PF_RT_CS = PF_Empr_Rate;
                PF_SAL = Amt_Of_PF;
                DED1 = Math.Ceiling(Amt_Of_PF * (PF_Rate / 100));
                PF_AMT_CS = Math.Ceiling(Amt_Of_PF * (PF_Empr_Rate / 100));
            }
            #endregion

            #region ESI
            if (ESI_YN == "Y")
            {
                //ESI_RT_ES = ESI_Rate * 100;
                //ESI_RT_CS = ESI_Empr_Rate * 100;
                //if (dt.Rows.Count > 0)
                //{
                //    ESI_SAL = fgen.make_double(dt.Rows[0]["ded3"].ToString().Trim());
                //}
                //ESI_AMT_CS = Math.Ceiling(ESI_SAL * ESI_Empr_Rate);
                //DED3 = Math.Ceiling(ESI_SAL * ESI_Rate);
                //---------------------------------
                ESI_RT_ES = ESI_Rate;
                ESI_RT_CS = ESI_Empr_Rate;
                if (dt.Rows.Count > 0)
                {
                    ESI_SAL = fgen.make_double(dt.Rows[0]["ded3"].ToString().Trim());
                }
                ESI_AMT_CS = Math.Ceiling(ESI_SAL * (ESI_Empr_Rate / 100));
                DED3 = Math.Ceiling(ESI_SAL * (ESI_Rate / 100));
            }
            #endregion

            #region WF
            //WF_RT_CS = WF_Empr_Rate;// HERE WF_Empr_Rate DENOTES THE MULTIPLYING FACTOR
            //WF_RT_ES = WF_Rate * 100;
            //Amt_of_WF = Amt_For_WF * WF_Rate;
            //if (Amt_of_WF > WF_Limit)
            //{
            //    DED6 = WF_Limit;
            //    WFWage = WF_Limit * WF_Rate;
            //}
            //else
            //{
            //    WFWage = (Amt_of_WF * 100) / WF_Rate;
            //}
            //WF_AMT_CS = Math.Ceiling(Amt_of_WF * WF_RT_CS);
            //-------------------------------------
            if (WF_Rate > 0 && WF_Empr_Rate > 0) // AS PER MAYURI MAM WF DEDUCTION IS NOT APPLICABLE TO ALL STATES THAT'S WHY THIS NEW IF CONDITION IS APPLIED
            {
                WF_RT_CS = WF_Empr_Rate;// HERE WF_Empr_Rate DENOTES THE MULTIPLYING FACTOR
                WF_RT_ES = WF_Rate;
                Amt_of_WF = Amt_For_WF * (WF_Rate / 100);
                if (Amt_of_WF > WF_Limit)
                {
                    DED6 = WF_Limit;
                    WFWage = WF_Limit * (WF_Rate / 100);
                }
                else
                {
                    DED6 = Math.Ceiling(Amt_of_WF);
                    WFWage = (Amt_of_WF * 100) / (WF_Rate / 100);
                }
                WF_AMT_CS = Math.Ceiling(DED6 * WF_RT_CS);
            }
            #endregion

            #region PT
            foreach (DataRow dr in dtPT.Rows)
            {
                Pass = IsWithin(PT, fgen.make_double(dr["sal_frm"].ToString().Trim()), fgen.make_double(dr["sal_upto"].ToString().Trim()));
                if (Pass == true)
                {
                    if (Arr_Month_Name.Length > 1)
                    {
                        string[] mth = Arr_Month_Name.TrimStart(',').Split(',');
                        for (int k = 0; k < mth.Length; k++)
                        {
                            DED10 += fgen.make_double(fgen.seek_iname_dt(dtPT, "sal_frm=" + fgen.make_double(dr["sal_frm"].ToString().Trim()) + " AND sal_upto=" + fgen.make_double(dr["sal_upto"].ToString().Trim()) + "", "mth" + mth[k]));
                        }
                    }
                    DED10 += fgen.make_double(fgen.seek_iname_dt(dtPT, "sal_frm=" + fgen.make_double(dr["sal_frm"].ToString().Trim()) + " AND sal_upto=" + fgen.make_double(dr["sal_upto"].ToString().Trim()) + "", "mth" + txtvchdate.Text.Substring(3, 2)));
                    break;
                }
            }
            #endregion

            Tot_Ded = DED1 + DED2 + DED3 + DED4 + DED5 + DED6 + DED7 + DED8 + DED9 + DED10 + DED11 + DED12 + DED13 + DED14 + DED15 + DED16 + DED17 + DED18 + DED19 + DED20;
            Tot_Earnings = er1 + er2 + er3 + er4 + er5 + er6 + er7 + er8 + er9 + er10 + er11 + er12 + er13 + er14 + er15 + er16 + er17 + er18 + er19 + er20 + ar1 + ar2 + ar3 + ar4 + ar5 + ar6 + ar7 + ar8 + ar9 + ar10 + ar11 + ar12 + ar13 + ar14 + ar15 + ar16 + ar17 + ar18 + ar19 + ar20;
            Tot_Sal = erate1 + erate2 + erate3 + erate4 + erate5 + erate6 + erate7 + erate8 + erate9 + erate10 + erate11 + erate12 + erate13 + erate14 + erate15 + erate16 + erate17 + erate18 + erate19 + erate20;
            Net_Sal = Math.Round(Tot_Earnings - Tot_Ded, NetSlry_RoundOff);
            SQuery1 = "update pay set ded1=" + DED1 + ",ded3=" + DED3 + ",ded6=" + Math.Ceiling(DED6) + ",ded10=" + Math.Ceiling(DED10) + ",totsal=" + Tot_Sal + ",totern=" + Tot_Earnings + ",totded=" + Tot_Ded + ",netslry=" + Net_Sal + ",ESI_RT_ES = " + ESI_RT_ES + ",ESI_RT_CS =" + ESI_RT_CS + ",ESI_SAL=" + ESI_SAL + ",ESI_AMT_CS=" + ESI_AMT_CS + ",PF_AMT_CS=" + PF_AMT_CS + ",PF_RT_CS=" + PF_RT_CS + ", PF_RT_ES=" + PF_RT_ES + ",PF_SAL=" + PF_SAL + ",WF_AMT_CS=" + WF_AMT_CS + ", WF_RT_CS=" + WF_RT_CS + ", WF_RT_ES=" + WF_RT_ES + ",WF_SAL=" + WFWage.ToString().Replace("Infinity", "0").Replace("NaN", "0").Replace("∞", "0") + ",selvch='" + selvch + "' where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text + "' and vchnum='" + txtvchnum.Text + "' and to_char(date_,'dd/mm/yyyy')='" + txtvchdate.Text + "' and empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'";
            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery1);
            //if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
            if (Show_OT == "Y")
            {
                Int32 Round_Off_Earnings = fgen.make_int(fgen.seek_iname(frm_qstr, frm_cocd, "select opt_param from fin_rsys_opt where opt_id='W0060'", "opt_param"));
                //SQuery2 = "update pay set er9=" + pr_vero + ",er10=" + pr_vero2 + ",erate9=" + pr_vero_on + ",erate10=" + pr_vero_on2 + " where empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'";
                Tot_Earnings = er1 + er2 + er3 + er4 + er5 + er6 + er7 + er8 + pr_vero + pr_vero2 + er11 + er12 + er13 + er14 + er15 + er16 + er17 + er18 + er19 + er20 + ar1 + ar2 + ar3 + ar4 + ar5 + ar6 + ar7 + ar8 + ar9 + ar10 + ar11 + ar12 + ar13 + ar14 + ar15 + ar16 + ar17 + ar18 + ar19 + ar20;
                Net_Sal = Math.Round(Tot_Earnings - Tot_Ded, NetSlry_RoundOff);
                Tot_Sal = erate1 + erate2 + erate3 + erate4 + erate5 + erate6 + erate7 + erate8 + pr_vero_on + pr_vero_on2 + erate11 + erate12 + erate13 + erate14 + erate15 + erate16 + erate17 + erate18 + erate19 + erate20;
                SQuery2 = "update pay set er10=" + Math.Round(pr_vero, Round_Off_Earnings) + ",er9=" + Math.Round(pr_vero2, Round_Off_Earnings) + ",erate10=" + pr_vero_on + ",erate9=" + pr_vero_on2 + ",totern=" + Tot_Earnings + ",totsal=" + Tot_Sal + ",netslry=" + Net_Sal + " where empcode='" + sg1.Rows[i].Cells[13].Text.Trim().ToUpper() + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery2);
            }
        }
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
        SQuery = "select type1 as fstr,name as grade_name,Type1 as Grade_Code from type where id='I' and substr(type1,1,1)<'2' order by grade_code";
    }
    //------------------------------------------------------------------------------------
    static int CountSundays(int year, int month)
    {
        var firstDay = new DateTime(year, month, 1);
        var day29 = firstDay.AddDays(28);
        var day30 = firstDay.AddDays(29);
        var day31 = firstDay.AddDays(30);
        if ((day29.Month == month && day29.DayOfWeek == DayOfWeek.Sunday) || (day30.Month == month && day30.DayOfWeek == DayOfWeek.Sunday) || (day31.Month == month && day31.DayOfWeek == DayOfWeek.Sunday))
        {
            return 5;
        }
        else
        {
            return 4;
        }
    }
    //------------------------------------------------------------------------------------
    public void Fetch_Arrear_Name()
    {
        dt4 = new DataTable();
        // SQuery = "select initcap(trim(ename)) as ename,trim(er) as er from selmas where grade='" + txtlbl4.Text.Trim() + "' and morder<10 order by morder";
        SQuery = "select distinct ed_fld as col,initcap(trim(ed_name)) as columns,morder from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and nvl(icat,'-')!='Y' and ed_fld like 'ER%' and morder<11 order by morder";
        dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        z = 37;
        for (int d = 0; d < dt4.Rows.Count; d++)
        {
            if (sg1.Rows.Count > 0)
            {
                sg1.HeaderRow.Cells[z].Text = "Ar:" + dt4.Rows[d]["columns"].ToString().Trim().Replace(" ", "_").Replace(".", "_");
            }
            z++;
        }
    }
    //------------------------------------------------------------------------------------
    public static bool IsWithin(double value, double minimum, double maximum)
    {
        return value >= minimum && value <= maximum;
    }
    //------------------------------------------------------------------------------------
    protected void btnAttn_Click(object sender, EventArgs e)
    {
        hffield.Value = "ATTN";
        fgen.msg("-", "CMSG", "Do You Want to Auto Fill Daily Attendance Data ?");
    }
    //------------------------------------------------------------------------------------
    protected void btnFormat_Click(object sender, EventArgs e)
    {
        //foreach (GridViewRow row in sg1.Rows)
        //{
        //    for (int i = 0; i < sg1.Columns.Count; i++)
        //    {
        //        String header = sg1.Columns[i].HeaderText;
        //        String cellText = row.Cells[i].Text;

        //    }
        //}
        if (txtlbl4.Text.Trim().Length <= 1)
        {
            hffield.Value = "FORMAT";
            SQuery = "select type1 as fstr,name as grade_name,Type1 as Grade_Code from type where id='I' and type1 like '0%' order by grade_code";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("Select Grade", frm_qstr);
        }
        else
        {
            //mq0 = "select 0 as srno,'-'empcode,0 as present,0 as absent,0 as holiday,0 as Wrk_Hr,0 as Wk_Off,0 as el,0 as cl,0 as sl,0 as Ext_Hr,0 as Ext_Min,0 as Loan,0 as tds,0 as Adv_Ded,0 as Canteen,0 as Medical,0 as Kpi_Inc,0 as Oth_Inc,0 as Ar_Days,0 as Ar_Mth,0 as kpi";
            //mq2 = "select distinct ed_fld as col,ed_name as columns,morder from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and nvl(icat,'-')!='Y' and ed_fld like 'ER%' and morder<11 order by morder";
            //dt2 = new DataTable();
            //dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);
            //for (int i = 0; i < dt2.Rows.Count; i++)
            //{
            //    SQuery += ",0 as Ar_" + dt2.Rows[i]["columns"].ToString().Trim().Replace(" ", "_").Replace(".", "");
            //}
            //SQuery1 = mq0 + SQuery + " from dual";
            //dt = new DataTable();
            //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery1);

            dt = new DataTable();
            if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
            {
                dt.Columns.Add("SRNO", typeof(string));
                dt.Columns.Add("CARDNO", typeof(string));
                dt.Columns.Add("PRESENT", typeof(string));
                dt.Columns.Add("ABSENT", typeof(string));
                dt.Columns.Add("WK_OFF", typeof(string));
                dt.Columns.Add("EL", typeof(string));
                dt.Columns.Add("CL", typeof(string));
                dt.Columns.Add("SL", typeof(string));
                dt.Columns.Add("OT1", typeof(string));
                dt.Columns.Add("OT2", typeof(string));
            }
            else
            {
                dt.Columns.Add("SRNO", typeof(string));
                dt.Columns.Add("EMPCODE", typeof(string));
                z = 1;
                for (int i = 17; i < 47; i++)
                {
                    try
                    {
                        dt.Columns.Add(sg1.HeaderRow.Cells[i].Text.Replace("(", "_").Replace(")", "").Replace(" ", "_").Replace(".", "_").Replace("/", "_"));
                    }
                    catch
                    {
                        dt.Columns.Add("Ar" + z);
                        z++;
                    }
                }
            }
            FileName = frm_cocd + "_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".csv";
            //fgen.exp_to_excel(dt, "ms-excel", "xls", FileName);
            string filepath = Server.MapPath("~/tej-base/Upload/") + FileName;
            fgen.CreateCSVFile(dt, Server.MapPath("~/erp_docs/Upload/") + FileName);
            Session["FilePath"] = FileName;
            Session["FileName"] = FileName;
            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");
            fgen.msg("-", "AMSG", "The file has been downloaded!!");
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnImport_Click(object sender, EventArgs e)
    {
        FileUpload1.Visible = true;
    }
    //------------------------------------------------------------------------------------
    protected void btnAtt_Click(object sender, EventArgs e)
    {
        FileUpload1.Visible = true;
        string filesavepath = "";
        string excelConString = "";
        if (FileUpload1.HasFile)
        {
           string filename = "" + DateTime.Now.ToString("ddMMyyhhmmfff");
            filesavepath = AppDomain.CurrentDomain.BaseDirectory + "tej-base\\Upload\\file" + filename + ".csv";
            //filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".csv";
            FileUpload1.SaveAs(filesavepath);
            // excelConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
            excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "tej-base\\Upload\\" + ";Extended Properties=\"Text;HDR=Yes;FMT=Delimited\"";
            try
            {
                OleDbConnection OleDbConn = new OleDbConnection(); OleDbConn.ConnectionString = excelConString;
                OleDbConn.Open();
                DataTable dt = OleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                OleDbConn.Close();
                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }
                excelSheets[0] = "file" + filename + ".csv";
                OleDbCommand OleDbCmd = new OleDbCommand();
                String Query = "";
                Query = "SELECT  * FROM [" + excelSheets[0] + "]";
                OleDbCmd.CommandText = Query;
                OleDbCmd.Connection = OleDbConn;
                OleDbCmd.CommandTimeout = 0;
                OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                objAdapter.SelectCommand = OleDbCmd;
                objAdapter.SelectCommand.CommandTimeout = 0;
                dt = null;
                dt = new DataTable();
                objAdapter.Fill(dt);
                //mq0 = "select 0 as srno,'-'empcode,0 as present,0 as absent,0 as holiday,0 as Wrk_Hr,0 as Wk_Off,0 as el,0 as cl,0 as sl,0 as Ext_Hr,0 as Ext_Min,0 as Loan,0 as tds,0 as Adv_Ded,0 as Canteen,0 as Medical,0 as Kpi_Inc,0 as Oth_Inc,0 as Ar_Days,0 as Ar_Mth,0 as kpi";
                //mq2 = "select distinct ed_fld as col,ed_name as columns,morder from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and nvl(icat,'-')!='Y' and ed_fld like 'ER%' and morder<11 order by morder";
                //dt2 = new DataTable();
                //dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                //string[] arrear_col = new string[dt2.Rows.Count];
                //for (int k = 0; k < dt2.Rows.Count; k++)
                //{
                //    SQuery += ",0 as Ar_" + dt2.Rows[k]["columns"].ToString().Trim().Replace(" ", "_").Replace(".", "");
                //    arrear_col[k] = "Ar_" + dt2.Rows[k]["columns"].ToString().Trim().Replace(" ", "_").Replace(".", "");
                //}
                //SQuery1 = mq0 + SQuery + " from dual";
                //dt3 = new DataTable();
                //dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                dt3 = new DataTable();
                if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
                {
                    dt3.Columns.Add("SRNO", typeof(string));
                    dt3.Columns.Add("CARDNO", typeof(string));
                    dt3.Columns.Add("PRESENT", typeof(string));
                    dt3.Columns.Add("ABSENT", typeof(string));
                    dt3.Columns.Add("WK_OFF", typeof(string));
                    dt3.Columns.Add("EL", typeof(string));
                    dt3.Columns.Add("CL", typeof(string));
                    dt3.Columns.Add("SL", typeof(string));
                    dt3.Columns.Add("OT1", typeof(string));
                    dt3.Columns.Add("OT2", typeof(string));
                }
                else
                {
                    mq2 = "select distinct ed_fld as col,ed_name as columns,morder from wb_selmast where branchcd='" + frm_mbr + "' and grade='" + txtlbl4.Text.Trim() + "' and nvl(icat,'-')!='Y' and ed_fld like 'ER%' and morder<11 order by morder";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                    string[] arrear_col = new string[dt2.Rows.Count];
                    for (int k = 0; i < dt2.Rows.Count; k++)
                    {
                        SQuery += ",0 as Ar:" + dt2.Rows[k]["columns"].ToString().Trim().Replace(" ", "_").Replace(".", "");
                        arrear_col[k] = "Ar:" + dt2.Rows[k]["columns"].ToString().Trim().Replace(" ", "_").Replace(".", "");
                    }
                    dt3.Columns.Add("SRNO", typeof(string));
                    dt3.Columns.Add("EMPCODE", typeof(string));
                    z = 1;
                    for (int l = 17; l < 47; l++)
                    {
                        try
                        {
                            dt3.Columns.Add(sg1.HeaderRow.Cells[l].Text.Replace("(", "_").Replace(")", "").Replace(" ", "_").Replace(".", "_").Replace("/", "_"));
                        }
                        catch
                        {
                            dt3.Columns.Add("Ar" + z);
                            z++;
                        }
                    }
                }
                if (dt.Columns.Count == dt3.Columns.Count)
                {
                    // dt3.Columns["empcode"].MaxLength = 6;
                    //dt3.Rows.RemoveAt(0);
                    for (int j = 0; j < dt3.Columns.Count; j++)
                    {
                        if (dt3.Columns[j].ColumnName.ToString().Trim().ToUpper() != dt.Columns[j].ColumnName.ToString().Trim().ToUpper())
                        {
                            fgen.msg("-", "AMSG", "Names Are Not As Per The Prescribed Format. Original Column Name Is " + dt3.Columns[j].ColumnName.ToString().Trim().ToUpper() + ".But You Have Changed The Column Name To " + dt.Columns[j].ColumnName.ToString().Trim().ToUpper() + "");
                            return;
                        }
                    }
                }
                else
                {
                    fgen.msg("-", "AMSG", " Please Put Exact Number Of Columns As Prescribed");
                    return;
                }
                int count = 1, colcount = 22; oporow4 = null;

                if (frm_cocd == "SPPI" || frm_cocd == "HPPI")
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[1].ToString().Trim().Length > 1)
                        {
                            oporow4 = dt3.NewRow();
                            oporow4["srno"] = count;
                            oporow4["cardno"] = dr[1].ToString().Trim();
                            oporow4["present"] = fgen.make_double(dr[2].ToString().Trim());
                            oporow4["absent"] = fgen.make_double(dr[3].ToString().Trim());
                            oporow4["Wk_Off"] = fgen.make_double(dr[4].ToString().Trim());
                            oporow4["el"] = fgen.make_double(dr[5].ToString().Trim());
                            oporow4["cl"] = fgen.make_double(dr[6].ToString().Trim());
                            oporow4["sl"] = fgen.make_double(dr[7].ToString().Trim());
                            oporow4["OT1"] = fgen.make_double(dr[8].ToString().Trim());
                            oporow4["OT2"] = fgen.make_double(dr[9].ToString().Trim());
                            dt3.Rows.Add(oporow4);
                        }
                    }
                    create_tab();
                    sg1_dr = null;
                    SQuery1 = "select nvl(a.deptt_text,'') as deptt_Text,to_char(a.dtjoin,'dd/mm/yyyy') as dtjoin,nvl(a.appr_by,'-') as appr_by,nvl(a.erpecode,'XXXXXX') as erpecode,nvl(a.empcode,'XXXXXX') as empcode,nvl(a.name,'-') as Name,nvl(a.cardno,'-') as cardno,a.old_empc,nvl(a.fhname,'-') as fhname,A.WRKHOUR,nvl(a.deptt,'-') as deptt,nvl(a.desg,'-') as desg,b.dramt,(b.dramt-b.cramt) as advbal,nvl(a.ded4,0) as tds,nvl(a.ded7,0) as ded7,nvl(a.ded8,0) as ded8,nvl(a.ded9,0) as ded9,a.leaving_Dt,a.ded12,nvl(trim(a.status),'-') as status from empmas a left outer join advstat b on trim(a.empcode)=trim(b.empcode) and trim(a.grade)=trim(b.grade) and a.branchcd=b.branchcd where substr(nvl(trim(a.tfr_stat),'-'),1,8)<>'TRANSFER' and TRIM(nvl(a.leaving_dt,'-'))='-' and  a.branchcd='" + frm_mbr + "' and a.grade='" + txtlbl4.Text.Trim() + "' and a.dtjoin<=to_DatE('" + txtvchdate.Text + "','dd/mm/yyyy') order by a.empcode";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                    for (int l = 0; l < dt3.Rows.Count; l++)
                    {
                        sg1_dr = sg1_dt.NewRow();
                        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        sg1_dr["sg1_h1"] = "-";
                        sg1_dr["sg1_h2"] = "-";
                        sg1_dr["sg1_h3"] = "-";
                        sg1_dr["sg1_h4"] = "-";
                        sg1_dr["sg1_h5"] = fgen.seek_iname_dt(dt2, "cardno='" + dt3.Rows[l]["cardno"].ToString().Trim() + "'", "empcode");
                        sg1_dr["sg1_h7"] = fgen.make_double(dt3.Rows[l]["present"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["Wk_Off"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["el"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["cl"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["sl"].ToString().Trim());
                        sg1_dr["sg1_h6"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1_dr["sg1_h5"].ToString().Trim() + "'", "advbal");
                        sg1_dr["sg1_h8"] = sg1_dr["sg1_h5"].ToString().Trim();
                        sg1_dr["sg1_h9"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1_dr["sg1_h5"].ToString().Trim() + "'", "Name");
                        sg1_dr["sg1_h10"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1_dr["sg1_h5"].ToString().Trim() + "'", "fhname");
                        sg1_dr["sg1_f1"] = sg1_dr["sg1_h5"].ToString().Trim();
                        sg1_dr["sg1_t1"] = dt3.Rows[l]["PRESENT"].ToString().Trim();
                        sg1_dr["sg1_t2"] = dt3.Rows[l]["ABSENT"].ToString().Trim();
                        sg1_dr["sg1_t5"] = dt3.Rows[l]["Wk_Off"].ToString().Trim();
                        sg1_dr["sg1_t6"] = dt3.Rows[l]["EL"].ToString().Trim();
                        sg1_dr["sg1_t7"] = dt3.Rows[l]["CL"].ToString().Trim();
                        sg1_dr["sg1_t8"] = dt3.Rows[l]["SL"].ToString().Trim();
                        sg1_dr["sg1_t9"] = dt3.Rows[l]["ot1"].ToString().Trim();
                        sg1_dr["sg1_t31"] = dt3.Rows[l]["ot2"].ToString().Trim();
                        sg1_dr["sg1_h11"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1_dr["sg1_h5"].ToString().Trim() + "'", "deptt_Text");
                        sg1_dr["sg1_h12"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1_dr["sg1_h5"].ToString().Trim() + "'", "ERPECODE");
                        sg1_dr["sg1_h13"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1_dr["sg1_h5"].ToString().Trim() + "'", "STATUS");
                        sg1_dr["sg1_h14"] = "-";
                        sg1_dr["sg1_h15"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1_dr["sg1_h5"].ToString().Trim() + "'", "desg");
                        sg1_dr["sg1_h16"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1_dr["sg1_h5"].ToString().Trim() + "'", "LEAVING_DT");
                        sg1_dr["sg1_h17"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1_dr["sg1_h5"].ToString().Trim() + "'", "dtjoin");
                        sg1_dr["sg1_h18"] = "-";
                        sg1_dr["sg1_h19"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1_dr["sg1_h5"].ToString().Trim() + "'", "appr_by");
                        sg1_dr["sg1_h20"] = "-";
                        sg1_dr["sg1_h21"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1_dr["sg1_h5"].ToString().Trim() + "'", "deptt");
                        sg1_dr["sg1_h22"] = fgen.seek_iname_dt(dt2, "empcode='" + sg1_dr["sg1_h5"].ToString().Trim() + "'", "wrkhour");
                        sg1_dr["sg1_h23"] = "0";
                        sg1_dt.Rows.Add(sg1_dr);
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr[1].ToString().Trim().Length > 1)
                        {
                            oporow4 = dt3.NewRow();
                            oporow4["srno"] = count;
                            oporow4["empcode"] = fgen.padlc(Convert.ToInt32(dr[1].ToString().Trim()), 6);
                            oporow4["present"] = fgen.make_double(dr[2].ToString().Trim());
                            oporow4["absent"] = fgen.make_double(dr[3].ToString().Trim());
                            oporow4["holiday"] = fgen.make_double(dr[4].ToString().Trim());
                            oporow4["Wrk_Hr"] = fgen.make_double(dr[5].ToString().Trim());
                            oporow4["Wk_Off"] = fgen.make_double(dr[6].ToString().Trim());
                            oporow4["el"] = fgen.make_double(dr[7].ToString().Trim());
                            oporow4["cl"] = fgen.make_double(dr[8].ToString().Trim());
                            oporow4["sl"] = fgen.make_double(dr[9].ToString().Trim());
                            oporow4["Ext_Hr"] = fgen.make_double(dr[10].ToString().Trim());
                            oporow4["Ext_Min"] = fgen.make_double(dr[11].ToString().Trim());
                            oporow4[dt.Columns[12].ColumnName.Trim()] = fgen.make_double(dr[12].ToString().Trim());
                            oporow4[dt.Columns[13].ColumnName.Trim()] = fgen.make_double(dr[13].ToString().Trim());
                            oporow4[dt.Columns[14].ColumnName.Trim()] = fgen.make_double(dr[14].ToString().Trim());
                            oporow4[dt.Columns[15].ColumnName.Trim()] = fgen.make_double(dr[15].ToString().Trim());
                            oporow4[dt.Columns[16].ColumnName.Trim()] = fgen.make_double(dr[16].ToString().Trim());
                            oporow4[dt.Columns[17].ColumnName.Trim()] = fgen.make_double(dr[17].ToString().Trim());
                            oporow4[dt.Columns[18].ColumnName.Trim()] = fgen.make_double(dr[18].ToString().Trim());
                            oporow4[dt.Columns[19].ColumnName.Trim()] = fgen.make_double(dr[19].ToString().Trim());
                            oporow4[dt.Columns[20].ColumnName.Trim()] = fgen.make_double(dr[20].ToString().Trim());
                            oporow4[dt.Columns[21].ColumnName.Trim()] = fgen.make_double(dr[21].ToString().Trim());
                            for (int k = 0; k < 10; k++)
                            {
                                oporow4[dt.Columns[colcount].ColumnName.Trim()] = fgen.make_double(dr[colcount].ToString().Trim());
                                colcount++;
                            }
                            //for (int k = 0; k < arrear_col.Length; k++)
                            //{
                            //    oporow4[arrear_col[k].ToString()] = fgen.make_double(dr[colcount].ToString().Trim());
                            //    colcount++;
                            //}
                            count++;
                            colcount = 22;
                            dt3.Rows.Add(oporow4);
                        }
                    }
                    create_tab();
                    sg1_dr = null;
                    SQuery1 = "select nvl(a.deptt_text,'') as deptt_Text,to_char(a.dtjoin,'dd/mm/yyyy') as dtjoin,nvl(a.appr_by,'-') as appr_by,nvl(a.erpecode,'XXXXXX') as erpecode,nvl(a.empcode,'XXXXXX') as empcode,nvl(a.name,'-') as Name,nvl(a.cardno,'-') as cardno,a.old_empc,nvl(a.fhname,'-') as fhname,A.WRKHOUR,nvl(a.deptt,'-') as deptt,nvl(a.desg,'-') as desg,b.dramt,(b.dramt-b.cramt) as advbal,nvl(a.ded4,0) as tds,nvl(a.ded7,0) as ded7,nvl(a.ded8,0) as ded8,nvl(a.ded9,0) as ded9,a.leaving_Dt,a.ded12,nvl(trim(a.status),'-') as status from empmas a left outer join advstat b on trim(a.empcode)=trim(b.empcode) and trim(a.grade)=trim(b.grade) and a.branchcd=b.branchcd where substr(nvl(trim(a.tfr_stat),'-'),1,8)<>'TRANSFER' and TRIM(nvl(a.leaving_dt,'-'))='-' and  a.branchcd='" + frm_mbr + "' and a.grade='" + txtlbl4.Text.Trim() + "' and a.dtjoin<=to_DatE('" + txtvchdate.Text + "','dd/mm/yyyy') order by a.empcode";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                    for (int l = 0; l < dt3.Rows.Count; l++)
                    {
                        sg1_dr = sg1_dt.NewRow();
                        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        sg1_dr["sg1_h1"] = "-";
                        sg1_dr["sg1_h2"] = "-";
                        sg1_dr["sg1_h3"] = "-";
                        sg1_dr["sg1_h4"] = "-";
                        sg1_dr["sg1_h5"] = fgen.seek_iname_dt(dt2, "empcode='" + dt3.Rows[l]["empcode"].ToString().Trim() + "'", "cardno");
                        //sg1_dr["sg1_h7"] = fgen.make_double(txtlbl8.Text) - fgen.make_double(dt3.Rows[l]["present"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["holiday"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["Wk_Off"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["el"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["cl"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["sl"].ToString().Trim());
                        sg1_dr["sg1_h7"] = fgen.make_double(dt3.Rows[l]["present"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["holiday"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["Wk_Off"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["el"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["cl"].ToString().Trim()) + fgen.make_double(dt3.Rows[l]["sl"].ToString().Trim());
                        sg1_dr["sg1_h6"] = fgen.seek_iname_dt(dt2, "empcode='" + dt3.Rows[l]["empcode"].ToString().Trim() + "'", "advbal");
                        sg1_dr["sg1_h8"] = dt3.Rows[l]["empcode"].ToString().Trim();
                        sg1_dr["sg1_h9"] = fgen.seek_iname_dt(dt2, "empcode='" + dt3.Rows[l]["empcode"].ToString().Trim() + "'", "Name");
                        sg1_dr["sg1_h10"] = fgen.seek_iname_dt(dt2, "empcode='" + dt3.Rows[l]["empcode"].ToString().Trim() + "'", "fhname");
                        sg1_dr["sg1_f1"] = dt3.Rows[l]["EMPCODE"].ToString().Trim();
                        sg1_dr["sg1_t1"] = dt3.Rows[l]["PRESENT"].ToString().Trim();
                        sg1_dr["sg1_t2"] = dt3.Rows[l]["ABSENT"].ToString().Trim();
                        sg1_dr["sg1_t3"] = dt3.Rows[l]["holiday"].ToString().Trim();
                        sg1_dr["sg1_t4"] = dt3.Rows[l]["Wrk_Hr"].ToString().Trim();
                        sg1_dr["sg1_t5"] = dt3.Rows[l]["Wk_Off"].ToString().Trim();
                        sg1_dr["sg1_t6"] = dt3.Rows[l]["EL"].ToString().Trim();
                        sg1_dr["sg1_t7"] = dt3.Rows[l]["CL"].ToString().Trim();
                        sg1_dr["sg1_t8"] = dt3.Rows[l]["SL"].ToString().Trim();
                        sg1_dr["sg1_t9"] = dt3.Rows[l]["Ext_Hr"].ToString().Trim();
                        sg1_dr["sg1_t10"] = dt3.Rows[l]["Ext_Min"].ToString().Trim();
                        sg1_dr["sg1_t11"] = dt3.Rows[l][12].ToString().Trim();
                        sg1_dr["sg1_t12"] = dt3.Rows[l][13].ToString().Trim();
                        sg1_dr["sg1_t13"] = dt3.Rows[l][14].ToString().Trim();
                        sg1_dr["sg1_t14"] = dt3.Rows[l][15].ToString().Trim();
                        sg1_dr["sg1_t15"] = dt3.Rows[l][16].ToString().Trim();
                        sg1_dr["sg1_t19"] = dt3.Rows[l][17].ToString().Trim();
                        sg1_dr["sg1_t20"] = dt3.Rows[l][18].ToString().Trim();
                        sg1_dr["sg1_t21"] = dt3.Rows[l][19].ToString().Trim();
                        sg1_dr["sg1_t22"] = dt3.Rows[l][20].ToString().Trim();
                        sg1_dr["sg1_t24"] = dt3.Rows[l][21].ToString().Trim();
                        sg1_dr["sg1_t16"] = dt3.Rows[l][22].ToString().Trim();
                        sg1_dr["sg1_t17"] = dt3.Rows[l][23].ToString().Trim();
                        sg1_dr["sg1_t18"] = dt3.Rows[l][24].ToString().Trim();
                        sg1_dr["sg1_t23"] = dt3.Rows[l][25].ToString().Trim();
                        sg1_dr["sg1_t25"] = dt3.Rows[l][26].ToString().Trim();
                        sg1_dr["sg1_t26"] = dt3.Rows[l][27].ToString().Trim();
                        sg1_dr["sg1_t27"] = dt3.Rows[l][28].ToString().Trim();
                        sg1_dr["sg1_t28"] = dt3.Rows[l][29].ToString().Trim();
                        sg1_dr["sg1_t29"] = dt3.Rows[l][30].ToString().Trim();
                        sg1_dr["sg1_t30"] = dt3.Rows[l][31].ToString().Trim();
                        sg1_dr["sg1_h11"] = fgen.seek_iname_dt(dt2, "empcode='" + dt3.Rows[l]["empcode"].ToString().Trim() + "'", "deptt_Text");
                        sg1_dr["sg1_h12"] = fgen.seek_iname_dt(dt2, "empcode='" + dt3.Rows[l]["empcode"].ToString().Trim() + "'", "ERPECODE");
                        sg1_dr["sg1_h13"] = fgen.seek_iname_dt(dt2, "empcode='" + dt3.Rows[l]["empcode"].ToString().Trim() + "'", "STATUS");
                        sg1_dr["sg1_h14"] = "-";
                        sg1_dr["sg1_h15"] = fgen.seek_iname_dt(dt2, "empcode='" + dt3.Rows[l]["empcode"].ToString().Trim() + "'", "desg");
                        sg1_dr["sg1_h16"] = fgen.seek_iname_dt(dt2, "empcode='" + dt3.Rows[l]["empcode"].ToString().Trim() + "'", "LEAVING_DT");
                        sg1_dr["sg1_h17"] = fgen.seek_iname_dt(dt2, "empcode='" + dt3.Rows[l]["empcode"].ToString().Trim() + "'", "dtjoin");
                        sg1_dr["sg1_h18"] = "-";
                        sg1_dr["sg1_h19"] = fgen.seek_iname_dt(dt2, "empcode='" + dt3.Rows[l]["empcode"].ToString().Trim() + "'", "appr_by");
                        sg1_dr["sg1_h20"] = "-";
                        sg1_dr["sg1_h21"] = fgen.seek_iname_dt(dt2, "empcode='" + dt3.Rows[l]["empcode"].ToString().Trim() + "'", "deptt");
                        sg1_dr["sg1_h22"] = fgen.seek_iname_dt(dt2, "empcode='" + dt3.Rows[l]["empcode"].ToString().Trim() + "'", "wrkhour");
                        sg1_dr["sg1_h23"] = "0";
                        sg1_dt.Rows.Add(sg1_dr);
                    }
                }
                sg1.DataSource = sg1_dt;
                sg1.DataBind();
                setColHeadings();
                ViewState["sg1"] = sg1_dt;
                Fetch_Arrear_Name();
            }
            catch { }
        }
        else
        {
            // lblUpload.Text = "";
        }
    }
    //------------------------------------------------------------------------------------
}