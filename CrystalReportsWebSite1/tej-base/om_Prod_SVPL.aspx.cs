using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Drawing;
using System.Collections;
// F39551 dd_St

public partial class om_Prod_SVPL : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, fromdt, todt, vardate, party_cd, part_cd, typePopup = "Y", xStartDt = "", Enable = "", mq0 = "", mq1 = "";
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
                //if (dd_Stgtest.SelectedItem.Text == "A" || dd_Stgtest.SelectedItem.Text == "B") txtdisptime.Text = "510";
                //else txtdisptime.Text = "410";
            }

            setColHeadings();
            set_Val();
            typePopup = "N";
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
                if (orig_name.ToLower().Contains("sg1_t2")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

                //((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");             
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
        //  Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnprint.Disabled = false;
        btnline.Enabled = false; btnzone.Enabled = false; btnsupr.Enabled = false; btnshift.Enabled = false; btnpart.Enabled = false;
        create_tab();
        sg1_add_blankrows();
        create_tab1();
        sg2_add_blankrows();
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; btnprint.Disabled = true;
        btnline.Enabled = true; btnzone.Enabled = true; btnsupr.Enabled = true; btnshift.Enabled = true; btnpart.Enabled = true;
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
        frm_tabname = "WB_PROD_SVP";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "DE");
        lblheader.Text = "Data Entry";
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

            case "WO":
                SQuery = "Select Distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.org_invno)||trim(a.work_ordno)||trim(a.icode)||trim(a.cdrgno) as fstr,b.aname as Customer,a.Pordno,a.org_invno as WO_NO,a.acode,a.work_ordno as project,a.icode,i.iname,a.cdrgno as so_line_no,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.ordno,to_char(a.orddt,'yyyymmdd') as vdd from Somas a,famst b,item i where trim(a.acodE)=trim(b.acodE) and trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='4' and length(trim(nvl(a.app_by,'-')))> 1 and length(trim(nvl(a.org_invno,'-')))> 1 order by vdd desc,a.ordno desc";
                SQuery = "Select Distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.org_invno)||trim(a.work_ordno)||trim(a.icode)||trim(a.cdrgno) as fstr,b.aname as Customer,a.Pordno,a.org_invno as WO_NO,a.acode,a.work_ordno as project,a.icode,i.iname,a.cdrgno as so_line_no,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.ordno,to_char(a.orddt,'yyyymmdd') as vdd from Somas a,famst b,item i where trim(a.acodE)=trim(b.acodE) and trim(a.icode)=trim(i.icode) and a.branchcd!='DD' and substr(a.type,1,1)='4' and a.type!='44' and length(trim(nvl(a.app_by,'-')))> 1 and length(trim(nvl(a.org_invno,'-')))> 1 order by vdd desc,a.ordno desc";
                break;

            case "ZONE":
                SQuery = "SELECT TYPE1 AS FSTR,TYPE1 AS ZONECODE,TRIM(NAME) AS NAME  FROM TYPEGRP  where BRANCHCD='" + frm_mbr + "'  AND id='^P' order by TRIM(NAME)";
                break;

            case "SUPR":
                SQuery = "SELECT TYPE1 AS FSTR,TYPE1 AS CODE,NAME  FROM TYPEGRP  where BRANCHCD='" + frm_mbr + "'  AND id='^S' order by NAME";
                break;

            case "LINE":
                //SQuery = "SELECT distinct trim(a.lineno)||'~'||trim(b.name)||'~'||a.icode||'~'||trim(c.cpartno)||'~'||Trim(a.machcd)||'~'||trim(a.mch_names) as aa,trim(b.name) as line_name,trim(c.cpartno) as Cpartno,trim(a.lineno) as Line_No,a.icode  FROM itwstage a, typegrp b, item c where a.BRANCHCD='" + frm_mbr + "'  AND a. area='" + txtzcode.Text.Trim() + "' and trim(a.icode)=trim(c.icode) and trim(a.lineno)= trim(b.type1) and trim(b.id)='^Q' order by trim(b.name),trim(c.cpartno)";
                SQuery = "SELECT distinct trim(a.lineno)||'~'||trim(a.icode),trim(c.cpartno) as Cpartno,trim(a.lineno) as Line_No,a.icode  FROM itwstage a, typegrp b, item c where a.BRANCHCD='" + frm_mbr + "'  AND a. area='" + txtzcode.Text.Trim() + "' and trim(a.icode)=trim(c.icode) and trim(a.lineno)= trim(b.type1) and trim(b.id)='^Q' order by trim(a.lineno),trim(c.cpartno)";
                break;

            case "SHIFT":
                SQuery = "SELECT trim(TYPE1)||'~'||trim(name) AS FSTR,TYPE1 AS CODE,NAME  FROM TYPEGRP  where BRANCHCD='" + frm_mbr + "'  AND id='^R' order by NAME";
                break;

            case "PART":
                //SQuery = "SELECT DISTINCT TRIM(ICODE) AS FSTR,TRIM(ICODE) AS PART_cODE,TRIM(INAME) AS PART_NAME FROM ITEM WHERE LENGTH(TRIM(ICODE))>=8 AND SUBSTR(TRIM(ICODE),1,1) IN ('7','9')  ORDER BY PART_NAME";
                SQuery = "select DISTINCT trim(a.opcode) AS FSTR,trim(a.opcode) as OPcode,trim(a.icode) as icode,TRIM(B.INAME) AS PART_NAME,a.mtimE1 AS MACH_cODE,a.area,a.lineno from itwstage a,item b,typegrp c where trim(a.icode)=trim(b.icode) and trim(a.lineno)=trim(c.type1) and c.id='^Q'  and a.branchcd='" + frm_mbr + "' and a.type='10' and  A.LINENO='" + txtlinecode.Text + "'";
                SQuery = "select DISTINCT trim(a.opcode)||trim(a.icode) AS FSTR,trim(a.icode) as icode,a.machcd AS MACH_cODE from itwstage a,item b,typegrp c where trim(a.icode)=trim(b.icode) and trim(a.lineno)=trim(c.type1) and c.id='^Q'  and a.branchcd='" + frm_mbr + "' and a.type='10' and  A.LINENO='" + txtlinecode.Text + "'";//changes as per mam on 3.7.20
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + sg1.Rows[i].Cells[13].Text.Trim().Replace("-", "") + "'";
                    else col1 = "'" + sg1.Rows[i].Cells[13].Text.Trim().Replace("-", "") + "'";
                }
                if (col1.Length <= 0) col1 = "'-'";
                // {
                SQuery = "select distinct trim(opcode) as fstr,trim(opcode) as opcode,'-' as name from itwstage where branchcd='" + frm_mbr + "' and type='10' and vchdate " + DateRange + " ";

                break;

            case "SG1_ROW_OP":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + sg1.Rows[i].Cells[16].Text.Trim().Replace("-", "") + "'";
                    else col1 = "'" + sg1.Rows[i].Cells[16].Text.Trim().Replace("-", "") + "'";
                }
                if (col1.Length <= 0)
                {
                    SQuery = "select type1 as fstr,type1,name,id from typegrp where BRANCHCD='" + frm_mbr + "' AND id='FA' ";
                }
                else
                {
                    SQuery = "select type1 as fstr,type1,name,id from typegrp where BRANCHCD='" + frm_mbr + "' AND id='FA'  ";//and TYPE1 NOT IN (" + col1 + ")
                }
                break;

            case "SG2_ROW_ADD":
            case "SG2_ROW_ADD_E":
                col1 = "";
                for (i = 0; i < sg2.Rows.Count - 1; i++)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + sg2.Rows[i].Cells[3].Text.Trim().Replace("-", "") + "'";
                    else col1 = "'" + sg2.Rows[i].Cells[3].Text.Trim().Replace("-", "") + "'";
                }
                string cond = "'-'";
                if (col1 != "''" && col1 != "") cond = "" + col1 + "";
                //SQuery = "select trim(a.type1) as fstr,trim(a.name) as name,trim(a.type1) as type1,a.acref,trim(b.name) as Loss_Type from typegrp a, typegrp b where a.id='^T' and b.id='^U' and trim(a.acref)=trim(b.type1) and trim(a.type1) not in (" + cond + ") order by trim(b.name),trim(a.name)";//and TYPE1 NOT IN (" + col1 + ")                
                SQuery = "select trim(a.type1) as fstr,trim(a.name) as name,trim(a.type1) as type1,a.acref from typegrp a where a.id='^T' and trim(a.type1) not in (" + cond + ") order by trim(a.name)";//and TYPE1 NOT IN (" + col1 + ")                
                SQuery = "select distinct a.acref as fstr,a.acref as code,name from typegrp a where a.id='^U' and trim(a.type1) not in (" + cond + ") order by fstr";
                break;

            case "SG2_ADD":
                string code = "";
                //  int m = Convert.ToInt32(hf1.Value);
                code = sg2.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text.Trim();
                SQuery = "select type1 as fstr,acref as catg,name as detail,acref2 as loss_code from typegrp where id='^T' and acref='" + code + "' ORDER BY TYPE1";//old
                SQuery = "select type1 as fstr,trim(acref)||trim(acref2) as catg,name as detail,trim(acref2) as loss_code from typegrp where id='^T' and acref='" + code + "' ORDER BY TYPE1";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "Print_E":
                SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.cpartno as wo_no,a.acode as code,f.aname as customer,a.type,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst f WHERE trim(a.acode)=trim(f.acode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' AND a.VCHDATE  " + DateRange + " ORDER BY vdd desc,entry_no DESC";
                break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,A.SHIFT,a.type,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' AND a.VCHDATE " + DateRange + " ORDER BY vdd desc,entry_no DESC";
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
            set_Val();
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
            DDBind();
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
            create_tab1();
            sg2_add_blankrows();
            ViewState["sg2"] = sg2_dt;
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
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' and vchdate " + DateRange + " AND TYPE='" + frm_vty + "' ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        create_tab1();
        sg2_add_blankrows();
        sg2.DataSource = sg2_dt;
        sg2.DataBind();

        //ye rahi
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
        fgen.fill_zero(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus();
            return;
        }
        if (txtshifcode.Text == "-" || txtshifcode.Text == "" || txtshifcode.Text == "0")
        {
            fgen.msg("-", "AMSG", "Please Fill  Shift Incharge");
            txtshifcode.Focus();
            return;
        }
        if (txtsupcode.Text == "-" || txtsupcode.Text == "" || txtsupcode.Text == "0")
        {
            fgen.msg("-", "AMSG", "Please Fill Supervisor details");
            txtsupcode.Focus();
            return;
        }
        if (txtloss.Text == "")
        {
            fgen.msg("-", "AMSG", "Please Fill downtime details in 3rd Tab ");
            txtloss.Focus();
            return;
        }
        if (txttot_rej.Text == "")
        {
            fgen.msg("-", "AMSG", "Please Fill Rejection details in 2nd tab.");
            txttot_rej.Focus();
            return;
        }
        if (txtprodqty.Text == "" || txtprodqty.Text == "0")
        {
            fgen.msg("-", "AMSG", "Please Fill Total OK quantity produced.");
            txttot_rej.Focus();
            return;
        }
        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() == "-" || ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() == "0" || ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() == " ")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Fill Operator Name in Rows " + i + 1);
                return;
            }
        }
        if (dd_Shift_Status.SelectedItem.Text.Trim() == "PLEASE SELECT")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Select Shift Close Status");
            return;
        }

        Cal();
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
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();

        sg2_dt = new DataTable();
        create_tab1();
        sg2_add_blankrows();
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();

        ViewState["sg1"] = null;
        ViewState["sg2"] = null;
        setColHeadings();
        DDClear();
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
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from inspvch Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from inspvch a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "55" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(6, 10), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6) + "");
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
                        // txtlbl4.Text = dt.Rows[i]["frm_name"].ToString().Trim();
                        //txtlbl4a.Text = dt.Rows[i]["text"].ToString().Trim();
                        //txtlbl2.Text = dt.Rows[i]["frm_header"].ToString().Trim();
                        //txtlbl7.Text = dt.Rows[0]["ent_id"].ToString().Trim();
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
                            sg1_dr["sg1_t9"] = "";
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
                    DDBind();
                    SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt  from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtpartcode.Text = dt.Rows[0]["PCODE"].ToString().Trim();
                        txtpart.Text = dt.Rows[0]["PART"].ToString().Trim();
                        txtcapacity.Text = dt.Rows[0]["CAPCITY"].ToString().Trim();
                        txtline_Eff.Text = dt.Rows[0]["LINE_eFF"].ToString().Trim();
                        txtdisp.Text = dt.Rows[0]["DISP_LOSS"].ToString().Trim();
                        txtzcode.Text = dt.Rows[0]["ZCODE"].ToString().Trim();
                        txtzname.Text = dt.Rows[0]["ZONE"].ToString().Trim();
                        txtsupcode.Text = dt.Rows[0]["SUPCD"].ToString().Trim();
                        txtsupname.Text = dt.Rows[0]["SUPV"].ToString().Trim();
                        txtlinecode.Text = dt.Rows[0]["LINECD"].ToString().Trim();
                        txtine.Text = dt.Rows[0]["LINE"].ToString().Trim();
                        txtshifcode.Text = dt.Rows[0]["SHIFT_IC"].ToString().Trim();
                        txtshiftname.Text = dt.Rows[0]["IC_NAME"].ToString().Trim();
                        txtprodqty.Text = dt.Rows[0]["PROD_QTY"].ToString().Trim();
                        txtrej.Text = dt.Rows[0]["MCH_REJ"].ToString().Trim();
                        txtcastrej.Text = dt.Rows[0]["CAST_REJ"].ToString().Trim();
                        txtunproc_rej.Text = dt.Rows[0]["UNPROCREJ"].ToString().Trim();
                        txtdisptime.Text = dt.Rows[0]["DISP_SHIFT"].ToString().Trim();
                        dd_Stgtest.SelectedItem.Text = dt.Rows[0]["SHIFT"].ToString().Trim();
                        dd_Shift_Status.SelectedItem.Text = dt.Rows[0]["col16"].ToString().Trim();
                        txtloss.Text = dt.Rows[0]["num1"].ToString().Trim();
                        txttot_rej.Text = dt.Rows[0]["num2"].ToString().Trim();
                        txtgct.Text = dt.Rows[0]["num3"].ToString().Trim();

                        create_tab();
                        create_tab1();
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
                            sg1_dr["sg1_f1"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["col5"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["col2"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["col6"].ToString().Trim().Replace("-", "");
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        // =============EDIT
                        SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt  from inspvch a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + "55" + col1 + "' ORDER BY A.SRNO";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                        ViewState["fstr"] = col1;
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_f1"] = dt.Rows[i]["col1"].ToString().Trim();
                            sg2_dr["sg2_f3"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg2_dr["sg2_f4"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg2_dr["sg2_f5"] = dt.Rows[i]["col5"].ToString().Trim();
                            sg2_dr["sg2_t4"] = dt.Rows[i]["qty8"].ToString().Trim();
                            sg2_dt.Rows.Add(sg2_dr);
                        }

                        //sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        sg2_add_blankrows();
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        dt.Dispose(); sg1_dt.Dispose(); sg2_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                        Cal();
                    }
                    #endregion
                    Cal();
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_qa_reps(frm_qstr);
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

                //case "CELL"://NEED TO ASK
                //    if (col1.Length <= 0) return;
                //    dt = new DataTable();
                //    SQuery = "SELECT TYPE1,NAME  FROM TYPEGRP  where BRANCHCD='" + frm_mbr + "' AND id='HD' and type1='" + col1 + "'";
                //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //    if (dt.Rows.Count > 0)
                //    {
                //        txtmc.Text = dt.Rows[0]["TYPE1"].ToString().Trim();
                //        txtmcname.Text = dt.Rows[0]["NAME"].ToString().Trim();
                //    }
                //    Cal();
                //    break;

                case "ZONE":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    SQuery = "SELECT TYPE1,NAME  FROM TYPEGRP  where BRANCHCD='" + frm_mbr + "' AND id='^P' and type1='" + col1 + "'";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtzcode.Text = dt.Rows[0]["TYPE1"].ToString().Trim();
                        txtzname.Text = dt.Rows[0]["NAME"].ToString().Trim();
                    }
                    Cal();
                    break;

                case "SUPR"://MASTER CREATED
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    SQuery = "SELECT TYPE1,NAME  FROM TYPEGRP  where BRANCHCD='" + frm_mbr + "' AND id='^S' and type1='" + col1 + "'";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtsupcode.Text = dt.Rows[0]["TYPE1"].ToString().Trim();
                        txtsupname.Text = dt.Rows[0]["NAME"].ToString().Trim();
                    }
                    Cal();
                    break;

                case "LINE"://Master created                    
                    if (col1.Length < 1 || col1 == "" || col1 == "0") return;
                    else
                    {
                        SQuery = "SELECT distinct trim(a.lineno) as lineno,trim(b.name) as lname,a.icode as icode,trim(c.cpartno) as cpartno  FROM itwstage a, typegrp b, item c where a.BRANCHCD='" + frm_mbr + "'  AND a. area='" + txtzcode.Text.Trim() + "' and trim(a.icode)=trim(c.icode) and trim(a.lineno)= trim(b.type1) and trim(b.id)='^Q' and trim(a.lineno)||trim(a.icode)='" + col1.Split('~')[0].ToString() + col1.Split('~')[1].ToString() + "' order by trim(b.name),trim(c.cpartno)";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtlinecode.Text = dt.Rows[0]["lineno"].ToString().Trim();
                            txtine.Text = dt.Rows[0]["lname"].ToString().Trim();
                            txtpartcode.Text = dt.Rows[0]["icode"].ToString().Trim();
                            txtpart.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                            txtgct.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(mtime,0) as cycletime FROM ITWSTAGE a WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='10' AND trim(a.lineno)||trim(a.icode)='" + dt.Rows[0]["lineno"].ToString().Trim() + dt.Rows[0]["icode"].ToString().Trim() + "' AND UPPER(TRIM(nvl(a.indb,'-')))='Y'", "cycletime");

                            //txtlinecode.Text = (col1.Split('~')[0].ToString() == "0") ? "-" : col1.Split('~')[0].ToString();
                            //txtine.Text = (col1.Split('~')[1].ToString() == "0") ? "-" : col1.Split('~')[1].ToString();
                            //txtpartcode.Text = (col1.Split('~')[2].ToString() == "0") ? "-" : col1.Split('~')[2].ToString();
                            //txtpart.Text = (col1.Split('~')[3].ToString() == "0") ? "-" : col1.Split('~')[3].ToString();
                            //txtmc.Text = (col1.Split('~')[4].ToString() == "0") ? "-" : col1.Split('~')[4].ToString();
                            //txtmcname.Text = (col1.Split('~')[5].ToString() == "0") ? "-" : col1.Split('~')[5].ToString();
                            //txtgct.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(mtime,0) as cycletime FROM ITWSTAGE a WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='10' AND trim(a.lineno)||trim(a.machcd)||trim(a.icode)='" + col1.Split('~')[0].ToString() + col1.Split('~')[4].ToString() + col1.Split('~')[2].ToString() + "' AND UPPER(TRIM(nvl(a.indb,'-')))='Y' and UPPER(TRIM(nvl(a.indg,'-')))<>'Y'", "mtime");

                            //SQuery = "SELECT DISTINCT trim(a.opcode) as Op_code,trim(a.stagec) as stagec,trim(b.name) as stage_name,nvl(a.mtime,0) as cycletime FROM ITWSTAGE a, type b WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='10' AND trim(a.lineno)||trim(a.machcd)||trim(a.icode)='" + dt.Rows[0]["lineno"].ToString().Trim() + dt.Rows[0]["machcd"].ToString().Trim() + dt.Rows[0]["icode"].ToString().Trim() + "' AND UPPER(TRIM(nvl(a.indg,'-')))='Y' and b.id='K' and trim(b.type1)=trim(a.machcd) order by trim(a.OPCODE)";
                            SQuery = "SELECT DISTINCT trim(a.opcode) as Op_code,trim(a.stagec) as stagec,trim(b.name) as stage_name,nvl(a.mtime,0) as cycletime FROM ITWSTAGE a, type b WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='10' AND trim(a.lineno)||trim(a.icode)='" + dt.Rows[0]["lineno"].ToString().Trim() + dt.Rows[0]["icode"].ToString().Trim() + "' AND UPPER(TRIM(nvl(a.indg,'-')))='Y' and b.id='K' and trim(b.type1)=trim(a.stagec) order by trim(a.OPCODE)";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                create_tab();
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
                                    sg1_dr["sg1_f1"] = dt.Rows[d]["Op_code"].ToString().Trim();
                                    //sg1_dr["sg1_f2"] = dt.Rows[d]["Op_Name"].ToString().Trim();
                                    sg1_dr["sg1_f2"] = dt.Rows[d]["stagec"].ToString().Trim();
                                    sg1_dr["sg1_f4"] = dt.Rows[d]["stage_name"].ToString().Trim();
                                    sg1_dr["sg1_f5"] = dt.Rows[d]["cycletime"].ToString().Trim();
                                    sg1_dr["sg1_t1"] = "";
                                    sg1_dt.Rows.Add(sg1_dr);
                                }
                            }
                            ViewState["sg1"] = sg1_dt;
                            sg1.DataSource = sg1_dt;
                            sg1.DataBind();
                            if (dt.Rows != null) dt.Dispose();
                            if (sg1_dt != null) sg1_dt.Dispose();
                        }
                    }
                    Cal();
                    setColHeadings();
                    break;

                case "SHIFT"://master created
                    if (col1.Length <= 0) return;
                    else
                    {
                        txtshifcode.Text = (col1.Split('~')[0].ToString() == "0") ? "-" : col1.Split('~')[0].ToString();
                        txtshiftname.Text = (col1.Split('~')[1].ToString() == "0") ? "-" : col1.Split('~')[1].ToString();
                    }
                    Cal();
                    break;

                case "PART":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    string stage = "";
                    //    SQuery = "SELECT DISTINCT TRIM(ICODE) AS PART_cODE,TRIM(INAME) AS PART_NAME FROM ITEM WHERE LENGTH(TRIM(ICODE))>=8 AND trim(icode)='" + col1 + "'";
                    //select DISTINCT trim(a.opcode) as OPcode,trim(a.icode) as icode,TRIM(B.INAME) AS PART_NAME,a.mtimE1 AS MACH_cODE,mtime2,d.name AS MCH_AME,a.area,a.lineno from itwstage a,item b,typegrp c ,type d where trim(a.icode)=trim(b.icode) and trim(a.lineno)=trim(c.type1) and c.id='^Q'  and trim(a.mtime1)=substr(trim(d.name),0,4) and d.id='^' and a.branchcd='00' and a.type='10' and A.OPCODE='OP#40'
                    SQuery = "select DISTINCT trim(a.opcode) as OPcode,trim(a.icode) as icode,TRIM(B.INAME) AS PART_NAME,a.mtimE1 AS MACH_cODE,d.name AS MCH_AME,a.area,a.lineno from itwstage a,item b,typegrp c,type d where trim(a.icode)=trim(b.icode) and trim(a.lineno)=trim(c.type1) and c.id='^Q'  and trim(a.mtime1)=substr(trim(d.name),0,4) and d.id='^' and a.branchcd='" + frm_mbr + "' and a.type='10' and A.OPCODE='" + col1 + "'";
                    SQuery = "select DISTINCT trim(a.opcode) as OPcode,trim(a.icode) as icode,trim(a.stagec) as stage,TRIM(B.cpartno) AS PART_NAME,a.area,a.lineno from itwstage a,item b,typegrp c,type d where trim(a.icode)=trim(b.icode) and trim(a.lineno)=trim(c.type1) and c.id='^Q'  and trim(a.machcd)=trim(d.type1) and d.id='^' and a.branchcd='" + frm_mbr + "' and a.type='10' and trim(a.opcode)||trim(a.icode)='" + col1 + "'";
                    // SQuery = "select DISTINCT trim(a.opcode)||trim(a.icode)||trim(a.stagec) AS FSTR,trim(a.icode) as icode,a.machcd AS MACH_cODE from itwstage a,item b,typegrp c where trim(a.icode)=trim(b.icode) and trim(a.lineno)=trim(c.type1) and c.id='^Q'  and a.branchcd='" + frm_mbr + "' and a.type='10' and  A.LINENO='" + txtlinecode.Text + "'";//changes as per mam on 3.7.20

                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtpartcode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtpart.Text = dt.Rows[0]["PART_NAME"].ToString().Trim();
                        stage = dt.Rows[0]["stage"].ToString().Trim();
                    }

                    //Select Name,Type1 from type where id='^' order by type1  //FOR MACHINE NAME JOIN WITH THIS TABLE
                    SQuery = "SELECT DISTINCT OPCODE,'-' AS OPNAME FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND VCHDATE " + DateRange + " AND OPCODE='" + col1 + "' AND UPPER(TRIM(AREA))='G'";//old
                    SQuery = "SELECT DISTINCT OPCODE,'-' AS OPNAME,mtime as cycletime FROM ITWSTAGE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND VCHDATE " + DateRange + " AND trim(opcode)||trim(icode)||trim(stagec)='" + col1 + stage + "' AND UPPER(TRIM(AREA))='G'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        create_tab();
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
                            sg1_dr["sg1_f1"] = dt.Rows[d]["opcode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["OPNAME"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[d]["cycletime"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                    }
                    Cal();
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,b.weight from ivoucher a,item i,somas b where trim(a.icode)=trim(i.icode) and upper(trim(a.finvno))=trim(b.type)||'/'||trim(b.ordno)||' DT.'||to_char(b.orddt,'dd/mm/yyyy') and trim(a.icode)=trim(b.icode) and trim(a.invno)=trim(b.org_invno) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'')  ='" + col1.Trim() + "' order by Tag_no";
                    //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                    SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and /*a.branchcd='" + frm_mbr + "' and*/ a.type='15' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    //mq0 = "select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,COL11 AS FACING,COL12 AS FLANGE_STD,COL22 AS DESIGN_STD,COL4 AS VALVE_MODEL,COL3 AS RATING,COL2 AS SIZE_MM,COL25 AS CLIENT_TAG FROM SCRATCH WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='WO' AND COL26='" + txtWoLine.Text.Trim() + "' AND COL27='" + txtlbl4.Text.Trim() + "' AND UPPER(COL28)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND ICODE='" + txtIcode.Text.Trim() + "'";
                    //dt2 = new DataTable();
                    //dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in Hidden Field
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[d]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["finvno"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = dt.Rows[d]["TAG_NO"].ToString().Trim();
                    }
                    Cal();
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
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
                            sg1_dr["sg1_srno"] = Convert.ToInt32(sg1.Rows[i].Cells[12].Text);
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
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.ToString();
                            //sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[15].Text.ToString();
                            //sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[16].Text.ToString();                          
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        // SQuery = "select type1 as fstr,type1,name,id from typegrp where BRANCHCD='" + frm_mbr + "' AND id='FA' and type1 in ('" + col1.Trim() + "')";
                        SQuery = "select distinct trim(opcode) as opcode,'-' as name from itwstage where branchcd='" + frm_mbr + "' and type='10' and trim(opcode)='" + col1.Trim() + "'";
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
                            sg1_dr["sg1_f1"] = dt.Rows[d]["opcode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["name"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    else
                    {
                        create_tab();
                    }

                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    setColHeadings();
                    #endregion
                    break;

                case "SG1_ROW_OP":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = col3;
                    #endregion
                    break;

                case "SG2_ADD":
                    if (col1.Length <= 0) return;
                    //dt = new DataTable();
                    //if (ViewState["sg2"] != null)
                    //{
                    //    dt = new DataTable();
                    //    sg2_dt = new DataTable();
                    //    dt = (DataTable)ViewState["sg2"];
                    //    z = dt.Rows.Count - 1;
                    //    sg2_dt = dt.Clone();
                    //    sg2_dr = null;
                    //    // old existing of grid -- just to keep the data in curs
                    //    for (i = 0; i < dt.Rows.Count - 1; i++)
                    //    {
                    //        sg2_dr = sg2_dt.NewRow();                            
                    //        sg2_dr["sg2_f3"] = dt.Rows[i]["sg2_f3"];
                    //        sg2_dr["sg2_f4"] = dt.Rows[i]["sg2_f4"];
                    //        sg2_dr["sg2_t4"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text;
                    //        sg2_dt.Rows.Add(sg2_dr);
                    //    }
                    // SQuery = "select name as detail,acref as catg,acref2 as loss_code from typegrp where id='^T' and acref='" + col1.Trim() + "' ORDER BY TYPE1";
                    // sg2.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = col2;
                    sg2.Rows[Convert.ToInt32(hf1.Value)].Cells[5].Text = col3;
                    sg2.Rows[Convert.ToInt32(hf1.Value)].Cells[6].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    sg2.Rows[Convert.ToInt32(hf1.Value)].Cells[7].Text = col2;
                    ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t4")).Text = "";
                    //  }
                    // ViewState["sg2"] = sg2_dt;
                    //sg2.DataSource = sg2_dt;
                    //sg2.DataBind();
                    //dt.Dispose(); sg2_dt.Dispose();
                    //  ((TextBox)sg2.Rows[z].FindControl("sg2_t4")).Focus();
                    // setColHeadings();
                    break;



                case "SG2_ROW_ADD":
                    #region for gridview 2
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    if (ViewState["sg2"] != null)
                    {
                        dt = new DataTable();
                        sg2_dt = new DataTable();
                        dt = (DataTable)ViewState["sg2"];
                        z = dt.Rows.Count - 1;
                        sg2_dt = dt.Clone();
                        sg2_dr = null;
                        // old existing of grid -- just to keep the data in curs
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = dt.Rows[i]["sg2_srno"];
                            sg2_dr["sg2_f1"] = dt.Rows[i]["sg2_f1"];
                            sg2_dr["sg2_f3"] = sg2.Rows[i].Cells[5].Text.Replace("&nbsp;", "-");
                            sg2_dr["sg2_f4"] = sg2.Rows[i].Cells[6].Text.Replace("&nbsp;", "-");
                            sg2_dr["sg2_f5"] = sg2.Rows[i].Cells[7].Text.Replace("&nbsp;", "-");
                            sg2_dr["sg2_t4"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text;
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        //    SQuery = "select type1 as fstr,type1 as code ,name from type where  id='4' and type1='" + col1.Trim() + "'";
                        //  SQuery = "select a.type1,a.name,a.acref,b.name as Loss_Type from typegrp a, typegrp b where a.id='^T' and b.id='^U' and trim(a.acref)=trim(b.type1) and type1='" + col1.Trim() + "'";
                        //SQuery = "select trim(a.name) as name,trim(a.type1) as type1,a.acref,trim(b.name) as Loss_Type from typegrp a, typegrp b where a.id='^T' and b.id='^U' and trim(a.acref)=trim(b.type1)  and a.type1='" + col1.Trim() + "' ";

                        //    SQuery = "select trim(a.name) as name,trim(a.type1) as type1,a.acref from typegrp a where a.id='^T' and type1='"+col1+"' order by trim(a.name)";//and TYPE1 NOT IN (" + col1 + ")                
                        //  dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        //  for (int d = 0; d < dt.Rows.Count; d++)
                        // {
                        sg2_dr = sg2_dt.NewRow();
                        sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                        sg2_dr["sg2_f1"] = col1;
                        //sg2_dr["sg2_f3"] = col2;
                        //sg2_dr["sg2_f4"] = col3;
                        //sg2_dr["sg2_t4"] = "";
                        sg2_dt.Rows.Add(sg2_dr);
                        //}
                    }
                    else
                    {
                        create_tab1();
                        // sg2_add_blankrows();
                        SQuery = "select type1 as fstr,type1 as code ,name from type where  id='4' and type1='" + col1.Trim() + "'";//old
                        SQuery = "select a.acref as code from typegrp a where a.id='^T' and trim(a.acref)='" + col1.Trim() + "' order by fstr";//new
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_f1"] = dt.Rows[d]["code"].ToString().Trim();
                            //   sg2_dr["sg2_f2"] = dt.Rows[d]["name"].ToString().Trim();
                            // sg2_dr["sg2_f3"] = dt.Rows[d]["code"].ToString().Trim();
                            //  sg2_dr["sg2_t4"] = "";
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                    }
                    Cal();
                    sg2_add_blankrows();
                    ViewState["sg2"] = sg2_dt;
                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    dt.Dispose(); sg2_dt.Dispose();
                    ((TextBox)sg2.Rows[z].FindControl("sg2_t4")).Focus();
                    setColHeadings();
                    #endregion
                    break;

                case "SG2_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    //SQuery = "select type1 as fstr,type1 as code ,name from type where   id='4' and type1 in ('" + col1.Trim() + "')";
                    //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //for (int d = 0; d < dt.Rows.Count; d++)
                    //{
                    sg2.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = col2;
                    //sg2.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = col1;
                    //sg2.Rows[Convert.ToInt32(hf1.Value)].Cells[5].Text = col1;
                    ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t4")).Text = "";
                    //}
                    Cal();
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
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = (i + 1);
                            sg2_dr["sg2_f1"] = sg2.Rows[i].Cells[3].Text.Trim();
                            sg2_dr["sg2_f2"] = sg2.Rows[i].Cells[4].Text.Trim();
                            sg2_dr["sg2_f3"] = sg2.Rows[i].Cells[5].Text.Trim();
                            sg2_dr["sg2_t4"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim();
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        sg2_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
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
                            sg1_dr["sg1_f13"] = sg1.Rows[i].Cells[13].Text.Trim();
                            sg1_dr["sg1_f14"] = sg1.Rows[i].Cells[14].Text.Trim();
                            sg1_dr["sg1_f16"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f17"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
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
        frm_vty = "DE";
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "sELECT a.vchnum as entry_no,to_Char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.shift,a.pcode as partcode,a.part as partname,a.zcode as zone,a.zone as zone_name,a.supcd as supr_code,a.supv as supr_name,a.mc_cd as machcode,a.mc as machine,a.linecd as line_code,a.line as line_name,a.shift_ic ,a.ic_name as shift_inchg_name FROM " + frm_tabname + " a WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' AND a.VCHDATE " + PrdRange + " order by entry_no desc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }

            //for line
        else if (hffield.Value == "LINE")
        {
            party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
            part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
            col1 = party_cd + part_cd;
            if (col1.Length < 1 || col1 == "" || col1 == "0") return;

            SQuery = "SELECT distinct trim(a.lineno) as lineno,trim(b.name) as lname,a.icode as icode,trim(c.cpartno) as cpartno,trim(c.iname) as iname  FROM itwstage a, typegrp b, item c where a.BRANCHCD='" + frm_mbr + "'  AND a. area='" + txtzcode.Text.Trim() + "' and trim(a.icode)=trim(c.icode) and trim(a.lineno)= trim(b.type1) and trim(b.id)='^Q' and trim(a.lineno)||trim(a.icode)='" + col1 + "' order by trim(b.name),trim(c.cpartno)";
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count > 0)
            {
                txtlinecode.Text = dt.Rows[0]["lineno"].ToString().Trim();
                txtine.Text = dt.Rows[0]["lname"].ToString().Trim();
                txtpartcode.Text = dt.Rows[0]["icode"].ToString().Trim();
               // txtpart.Text = dt.Rows[0]["cpartno"].ToString().Trim();//old
                txtpart.Text = dt.Rows[0]["iname"].ToString().Trim();//new as 1 oct 20
                txtgct.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(mtime,0) as cycletime FROM ITWSTAGE a WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='10' AND trim(a.lineno)||trim(a.icode)='" + dt.Rows[0]["lineno"].ToString().Trim() + dt.Rows[0]["icode"].ToString().Trim() + "' AND UPPER(TRIM(nvl(a.indb,'-')))='Y'", "cycletime");

                //SQuery = "SELECT DISTINCT trim(a.opcode) as Op_code,trim(a.stagec) as stagec,trim(b.name) as stage_name,nvl(a.mtime,0) as cycletime FROM ITWSTAGE a, type b WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='10' AND trim(a.lineno)||trim(a.machcd)||trim(a.icode)='" + dt.Rows[0]["lineno"].ToString().Trim() + dt.Rows[0]["machcd"].ToString().Trim() + dt.Rows[0]["icode"].ToString().Trim() + "' AND UPPER(TRIM(nvl(a.indg,'-')))='Y' and b.id='K' and trim(b.type1)=trim(a.machcd) order by trim(a.OPCODE)";
                SQuery = "SELECT DISTINCT trim(a.opcode) as Op_code,a.mch_names,trim(a.stagec) as stagec,trim(b.name) as stage_name,nvl(a.mtime,0) as cycletime FROM ITWSTAGE a, type b WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='10' AND trim(a.lineno)||trim(a.icode)='" + dt.Rows[0]["lineno"].ToString().Trim() + dt.Rows[0]["icode"].ToString().Trim() + "' AND UPPER(TRIM(nvl(a.indg,'-')))='Y' and b.id='K' and trim(b.type1)=trim(a.stagec) order by trim(a.OPCODE)";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    create_tab();
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
                        // sg1_dr["sg1_f1"] = dt.Rows[d]["Op_code"].ToString().Trim(); //old
                        sg1_dr["sg1_f1"] = dt.Rows[d]["mch_names"].ToString().Trim();//new
                        //sg1_dr["sg1_f2"] = dt.Rows[d]["Op_Name"].ToString().Trim();
                     //   sg1_dr["sg1_f2"] = dt.Rows[d]["stagec"].ToString().Trim();//old..ioct
                        sg1_dr["sg1_f2"] = dt.Rows[d]["Op_code"].ToString().Trim();//new 1 oct
                        sg1_dr["sg1_f4"] = dt.Rows[d]["stage_name"].ToString().Trim();
                        sg1_dr["sg1_f5"] = dt.Rows[d]["cycletime"].ToString().Trim();
                        sg1_dr["sg1_t1"] = "";
                        sg1_dt.Rows.Add(sg1_dr);
                    }
                }
                ViewState["sg1"] = sg1_dt;
                sg1.DataSource = sg1_dt;
                sg1.DataBind();
                if (dt.Rows != null) dt.Dispose();
                if (sg1_dt != null) sg1_dt.Dispose();
            }
            Cal();
            setColHeadings();
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
                if (col1 == "Y" && Checked_ok == "Y")
                {
                    try
                    {
                        oDS = new DataSet();
                        oporow = null;
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                        // This is for checking that, is it ready to save the data

                        oDS2 = new DataSet();
                        oporow2 = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "INSPVCH");

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
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "INSPVCH");

                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnum.Text.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            //save_it = "N";

                            //for (i = 0; i < sg1.Rows.Count - 1; i++)
                            //{
                            save_it = "Y";
                            //}
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
                            cmd_query = "update inspvch set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "55" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, "inspvch");

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "delete from inspvch where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + "55" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
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
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); DDClear();
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
        sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(Int32)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f2", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_f3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        // sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));       
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
        //sg1_dr["sg1_f3"] = "-";
        sg1_dr["sg1_f4"] = "-";
        sg1_dr["sg1_f5"] = "-";
        sg1_dr["sg1_t1"] = "";
        // sg1_dr["sg1_t2"] = "-";
        // sg1_dr["sg1_t3"] = "-";         
        sg1_dt.Rows.Add(sg1_dr);
    }
    //------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------
    public void create_tab1()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f5", typeof(string)));

        //sg2_dt.Columns.Add(new DataColumn("sg2_t1", typeof(string)));
        //sg2_dt.Columns.Add(new DataColumn("sg2_t2", typeof(string)));
        //sg2_dt.Columns.Add(new DataColumn("sg2_t3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t4", typeof(string)));
    }
    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();
        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
        sg2_dr["sg2_f1"] = "-";
        sg2_dr["sg2_f2"] = "-";
        sg2_dr["sg2_f3"] = "-";
        sg2_dr["sg2_f4"] = "-";
        sg2_dr["sg2_f5"] = "-";
        //sg2_dr["sg2_t1"] = "-";
        //sg2_dr["sg2_t2"] = "-";
        //sg2_dr["sg2_t3"] = "-";
        sg2_dr["sg2_t4"] = "";
        sg2_dt.Rows.Add(sg2_dr);
    }

    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //sg1.HeaderRow.Cells[18].Style["display"] = "none";
            //e.Row.Cells[18].Style["display"] = "none";
            sg1.Columns[10].HeaderStyle.Width = 30;
            sg1.Columns[11].HeaderStyle.Width = 250;
            sg1.Columns[12].HeaderStyle.Width = 150;
            sg1.Columns[13].HeaderStyle.Width = 250;
            sg1.Columns[14].HeaderStyle.Width = 250;
            sg1.Columns[15].HeaderStyle.Width = 350;
            //sg1.Columns[15].HeaderStyle.Width = 100;
            //sg1.Columns[16].HeaderStyle.Width = 250;
            //sg1.Columns[17].HeaderStyle.Width =150;       
            //  sg1.Columns[18].HeaderStyle.Width = 250;
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
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Tag From The List");
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
                    fgen.Fn_open_sseek("Select Operation", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Operation", frm_qstr);
                }
                break;

            case "SG1_ROW_OP":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_OP";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Oprator", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------

    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TYPE";
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
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg1.Rows.Count; i++)
        {
            //if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().Length > 1)
            //{
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = txtvchnum.Text.Trim(); //frm_vnum.Trim().ToUpper();
            oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow["col19"] = frm_mbr + frm_vty + txtvchnum.Text.Trim().ToUpper() + txtvchdate.Text.Trim();//FSTRb for saving 2nd grid value in anothr table as per MG Mam
            oporow["SRNO"] = i + 1;
            oporow["SHIFT"] = dd_Stgtest.SelectedItem.Text.Trim().ToUpper();
            oporow["col16"] = dd_Shift_Status.SelectedItem.Text.Trim().ToUpper();
            oporow["COL4"] = sg1.Rows[i].Cells[11].Text.Trim(); //operation no
            oporow["COL5"] = sg1.Rows[i].Cells[12].Text.Trim(); //PROCESS CODE
            oporow["COL2"] = sg1.Rows[i].Cells[13].Text.Trim();//PROCESS NAME
            oporow["COL3"] = sg1.Rows[i].Cells[14].Text.Trim();//CYCLE TIME
            oporow["COL6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();// sg1.Rows[i].Cells[16].Text.Trim(); //operator name

            oporow["icode"] = txtpartcode.Text.Trim().ToUpper();///for bound field                              
            oporow["PCODE"] = txtpartcode.Text.Trim().ToUpper();
            oporow["ACODE"] = "-";
            oporow["PART"] = txtpart.Text.Trim().ToUpper();
            oporow["num1"] = fgen.make_double(txtloss.Text.Trim());
            oporow["num2"] = fgen.make_double(txttot_rej.Text.Trim());
            oporow["num3"] = fgen.make_double(txtgct.Text.Trim().ToUpper());

            oporow["CAPCITY"] = fgen.make_double(txtcapacity.Text.Trim().ToUpper());
            oporow["LINE_eFF"] = fgen.make_double(txtline_Eff.Text.Trim().ToUpper());
            oporow["DISP_LOSS"] = fgen.make_double(txtdisp.Text.Trim().ToUpper());

            oporow["ZCODE"] = txtzcode.Text.Trim().ToUpper();
            oporow["ZONE"] = txtzname.Text.Trim().ToUpper();
            oporow["SUPCD"] = txtsupcode.Text.Trim().ToUpper();
            oporow["SUPV"] = txtsupname.Text.Trim().ToUpper();
            oporow["LINECD"] = txtlinecode.Text.Trim().ToUpper();
            oporow["LINE"] = txtine.Text.Trim().ToUpper();
            oporow["SHIFT_IC"] = txtshifcode.Text.Trim().ToUpper();
            oporow["IC_NAME"] = txtshiftname.Text.Trim().ToUpper();

            oporow["PROD_QTY"] = fgen.make_double(txtprodqty.Text.Trim().ToUpper());
            oporow["MCH_REJ"] = fgen.make_double(txtrej.Text.Trim().ToUpper());
            oporow["CAST_REJ"] = fgen.make_double(txtcastrej.Text.Trim().ToUpper());
            oporow["UNPROCREJ"] = fgen.make_double(txtunproc_rej.Text.Trim().ToUpper());
            oporow["DISP_SHIFT"] = fgen.make_double(txtdisptime.Text.Trim().ToUpper());

            //grid 2 pending for saving


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
        // }
    }
    //------------------------------------------------------------------------------------

    void save_fun2()
    {
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        double shifttime = 0;
        for (i = 0; i < sg2.Rows.Count - 1; i++)
        {
            oporow2 = oDS2.Tables[0].NewRow();
            oporow2["BRANCHCD"] = frm_mbr;
            oporow2["TYPE"] = "55";
            oporow2["vchnum"] = txtvchnum.Text.Trim();// frm_vnum.Trim().ToUpper();
            oporow2["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow2["MRRNUM"] = "-";
            oporow2["mrrdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow2["btchno"] = frm_vnum.Trim().ToUpper();
            oporow2["btchdt"] = txtvchdate.Text.Trim().ToUpper();
            oporow2["grade"] = "-";

            oporow2["acode"] = "-";
            oporow2["icode"] = txtpartcode.Text.Trim().ToUpper();

            oporow2["contplan"] = "-";
            oporow2["sampqty"] = fgen.make_double(txtprodqty.Text.Trim());
            oporow2["Srno"] = i + 1;

            shifttime = (i == 0) ? fgen.make_double(txtdisptime.Text.Trim()) : 0;
            oporow2["qty7"] = shifttime;
            oporow2["QTY8"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim());

            oporow2["rejqty"] = fgen.make_double(txttot_rej.Text.Trim());
            oporow2["wono"] = "-";
            oporow2["matl"] = "-";
            oporow2["finish"] = "-";

            oporow2["Result"] = "0";

            oporow2["srno"] = sg2.Rows[i].Cells[2].Text.Trim(); ;//srno
            oporow2["Col1"] = sg2.Rows[i].Cells[3].Text.Trim(); ;//catgory
            oporow2["col3"] = sg2.Rows[i].Cells[5].Text.Trim(); ;//detail
            oporow2["col4"] = sg2.Rows[i].Cells[6].Text.Trim(); ;//losscode
            oporow2["col5"] = sg2.Rows[i].Cells[7].Text.Trim(); ;//catgcode
            oporow2["col6"] = frm_mbr + "DE" + txtvchnum.Text.Trim().ToUpper() + txtvchdate.Text.Trim(); //1ST TABLE FSTR
            oporow2["matl"] = "-";
            oporow2["finish"] = "-";
            oporow2["obsv1"] = "-";
            oporow2["obsv2"] = "-";
            oporow2["obsv3"] = "-";
            oporow2["obsv4"] = "-";
            oporow2["obsv5"] = "-";
            oporow2["obsv6"] = "-";
            oporow2["obsv7"] = "-";
            oporow2["obsv8"] = "-";
            oporow2["obsv9"] = "-";
            oporow2["obsv10"] = "-";
            oporow2["obsv11"] = "-";
            oporow2["obsv12"] = "-";
            oporow2["obsv13"] = "-";
            oporow2["obsv14"] = "-";
            oporow2["obsv15"] = "-";
            oporow2["omax"] = "-";
            oporow2["omin"] = "-";
            oporow2["obsv15"] = "-";
            oporow2["obsv16"] = "-";
            oporow2["cpartno"] = txtpart.Text.Trim().ToUpper();


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
    //-----------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "DE");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------
    public void DDClear()
    {
        dd_Stgtest.Items.Clear();
        dd_Shift_Status.Items.Clear();
    }
    //------------------------------------------------------------------------------------
    public void DDBind()
    {
        DDClear();
        // txtPMI.Text = "QAD-W-09";
        // txtClient.Text = "CLIENT TAG NO./ITEM NO.";

        dd_Stgtest.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        dd_Stgtest.Items.Add(new System.Web.UI.WebControls.ListItem("A", "A"));
        dd_Stgtest.Items.Add(new System.Web.UI.WebControls.ListItem("B", "B"));
        dd_Stgtest.Items.Add(new System.Web.UI.WebControls.ListItem("C", "C"));

        dd_Shift_Status.Items.Add(new System.Web.UI.WebControls.ListItem("PLEASE SELECT", "PLEASE SELECT"));
        dd_Shift_Status.Items.Add(new System.Web.UI.WebControls.ListItem("Y", "Y"));
        dd_Shift_Status.Items.Add(new System.Web.UI.WebControls.ListItem("N", "N"));
        //SQuery = "select 'PLEASE SELECT' as name from dual union all select name from typegrp where id ='^E'";
        //dt = new DataTable();
        //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        //if (dt.Rows.Count > 0)
        //{
        //    txtMachSrno.DataSource = dt;
        //    txtMachSrno.DataTextField = "name";
        //    txtMachSrno.DataValueField = "name";
        //    txtMachSrno.DataBind();
        //}        
    }
    //------------------------------------------------------------------------------------
    private void FillGrid()
    {
        create_tab();
        //SQuery = "SELECT 'COMPONENT' AS HEADING FROM DUAL UNION ALL SELECT 'VALVE TAG NO.' AS HEADING FROM DUAL UNION ALL SELECT 'CLIENT TAG NO.' AS HEADING FROM DUAL UNION ALL SELECT 'PMI TEST SEQUENCE_NO' AS HEADING FROM DUAL UNION ALL SELECT 'MATERIAL GRADE' AS HEADING FROM DUAL UNION ALL SELECT 'CR %' AS HEADING FROM DUAL UNION ALL SELECT 'NI %' AS HEADING FROM DUAL UNION ALL SELECT 'MO' AS HEADING FROM DUAL UNION ALL SELECT '-' AS HEADING FROM DUAL";
        //dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        //z = 1;
        //sg1_dr = sg1_dt.NewRow();
        //for (int i = 0; i < dt2.Rows.Count; i++)
        //{
        //    sg1_dr["sg1_srno"] = 1;
        //    sg1_dr["sg1_t" + z] = dt2.Rows[i]["heading"].ToString().Trim();
        //    z++;
        //}

        ArrayList GridHeading = new ArrayList();
        GridHeading.Add("COMPONENT");
        GridHeading.Add("VALVE TAG NO.");
        GridHeading.Add("CLIENT TAG NO. / ITEM NO");
        GridHeading.Add("PMI TEST SEQUENCE_NO");
        GridHeading.Add("MATERIAL GRADE");
        GridHeading.Add("CR %(REQ)");
        GridHeading.Add("NI %(REQ)");
        GridHeading.Add("MO(REQ)");
        GridHeading.Add("CR %(OBS)");
        GridHeading.Add("NI %(OBS)");
        GridHeading.Add("MO(OBS)");
        z = 1;
        sg1_dr = sg1_dt.NewRow();
        for (int i = 0; i < GridHeading.Count; i++)
        {
            sg1_dr["sg1_srno"] = 1;
            sg1_dr["sg1_f2"] = "-";
            sg1_dr["sg1_f6"] = "-";
            sg1_dr["sg1_t" + z] = GridHeading[i].ToString().Trim();
            if (GridHeading[i].ToString().Trim() == "MO(REQ)")
            {
                z = 10;
            }
            z++;
        }
        sg1_dt.Rows.Add(sg1_dr);
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        //if (sg1.Rows.Count > 0)
        //{
        //    sg1.Rows[0].Cells[10].Enabled = false;
        //    sg1.Rows[0].Cells[11].Enabled = false;
        //    ((DropDownList)sg1.Rows[0].FindControl("SG1_t10")).Enabled = false;
        //    sg1.Rows[0].BackColor = Color.Khaki;
        //}
    }
    //------------------------------------------------------------------------------------
    //protected void txtMachSrno_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    SQuery = "select type1,name,acref,acref2 from typegrp where id ='^E' and name='" + txtMachSrno.SelectedItem.Text.Trim() + "'";
    //    dt = new DataTable();
    //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
    //    if (dt.Rows.Count > 0)
    //    {
    //        txtMachMake.Text = dt.Rows[0]["acref"].ToString().Trim();
    //        txtMachModel.Text = dt.Rows[0]["acref2"].ToString().Trim();
    //    }
    //}
    //------------------------------------------------------------------------------------
    protected void sg1_t5_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddl.NamingContainer;
        int rowIndex = row.RowIndex;
        if (((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t2")).Text.Trim() == "-" || ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t2")).Text.Trim() == "")
        {
            fgen.msg("-", "AMSG", "Please Select Tag First At Line No. " + sg1.Rows[rowIndex].Cells[12].Text.Trim());
        }
        else
        {
            SQuery = "select type1,name,acref,acref2,acref3,p_acode from typegrp where id ='^F' and name='" + ((DropDownList)sg1.Rows[rowIndex].FindControl("sg1_t5")).Text.Trim() + "'";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count > 0)
            {
                ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t6")).Text = dt.Rows[0]["acref3"].ToString().Trim();
                ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t7")).Text = dt.Rows[0]["acref2"].ToString().Trim();
                ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t8")).Text = dt.Rows[0]["p_acode"].ToString().Trim();
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnzone_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ZONE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select ZONE", frm_qstr);
    }
    protected void btnsupr_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SUPR";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Supervisor", frm_qstr);
    }
    protected void btnline_Click(object sender, ImageClickEventArgs e)
    {
        if (txtzcode.Text.Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Zone First");
            return;
        }
        else if (txtdisptime.Text.Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Shift");
            return;
        }
        else
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_RCOL10", txtzcode.Text);//ZCODE VALUE
            fgen.Fn_open_Act_itm_prd("Select Line No. & Part No.", frm_qstr);
            hffield.Value = "LINE";
            //make_qry_4_popup();
            //fgen.Fn_open_sseek("Select Line", frm_qstr);
        }
    }
    protected void btnshift_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SHIFT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Shift Incharge", frm_qstr);
    }
    protected void btnpart_Click(object sender, ImageClickEventArgs e)
    {
        if (txtlinecode.Text.Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Line First");
            return;
        }
        else
        {
            hffield.Value = "PART";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Part", frm_qstr);
        }
    }
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
                if (index < sg2.Rows.Count)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG2_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Tag From The List");
                }
                break;

            case "SG2_ROW_ADD":
                if (index < sg2.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG2_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Category", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG2_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Category", frm_qstr);
                }
                break;

            case "SG2_ADD"://2nd btn in grid
                if (index < sg2.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG2_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Category", frm_qstr);
                }
                else
                {
                    fgen.msg("-", "AMSG", "Please Select Category First!! ");
                }
                break;
        }
    }
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // sg2.Columns[7].Visible = false;
    }

    protected void dd_Stgtest_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (dd_Stgtest.SelectedItem.Text == "A" || dd_Stgtest.SelectedItem.Text == "B") txtdisptime.Text = "510";
        //else txtdisptime.Text = "410";
        if (dd_Stgtest.SelectedItem.Value == "A" || dd_Stgtest.SelectedItem.Value == "B") txtdisptime.Text = "510";
        else txtdisptime.Text = "450";
    }

    void Cal()
    {
        double t1 = 0; double t2 = 0; double t3 = 0; double t4 = 0; double tot_Rej = 0; double cast_Rej = 0; double un_Rej = 0; double rej = 0;
        double prod = 0; double cap = 0; double line = 0; double LOSS = 0; double shift = 0; double prod1 = 0; double shift1 = 0; double ct = 0;
        //total rejection formula   
        rej = fgen.make_double(txtrej.Text.Trim());
        un_Rej = fgen.make_double(txtunproc_rej.Text.Trim());
        cast_Rej = fgen.make_double(txtcastrej.Text.Trim());
        tot_Rej = rej + un_Rej + cast_Rej;
        txttot_rej.Text = Convert.ToString(Math.Round(tot_Rej, 2)).Replace("Infinity", "0").Replace("NaN", "0");

        //=======total loss formula
        for (int i = 0; i < sg2.Rows.Count - 1; i++)
        {
            t1 += fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text);
        }
        txtloss.Text = Convert.ToString(Math.Round(t1, 2)).Replace("Infinity", "0").Replace("NaN", "0");


        ////display UNA/C LOSS MIN FORMULA========''***=(Shift time-Total loss time entered above-(Prod Qty * CT based on Part No, Line No/60))
        LOSS = fgen.make_double(txtloss.Text.Trim());
        shift = fgen.make_double(txtdisptime.Text.Trim());
        prod = fgen.make_double(txtprodqty.Text.Trim());
        //for (int i = 0; i < sg1.Rows.Count - 1; i++)
        //{
        //    t2 += fgen.make_double(sg1.Rows[i].Cells[14].Text.Trim());//cycletime
        //}
        //prod1 = (shift - LOSS - (prod * t2 / 60));

        //========capacity formulla=====  ###=(Shift time-50)*60/(CT based on "Part No", "Line No" from "Ip table 1" with "bottleneck Indicator" as "B")
        ct = fgen.make_double(txtgct.Text.Trim());
        shift1 = (shift - 50) / ct;
        txtcapacity.Text = Convert.ToString(Math.Round(shift1, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        prod1 = (shift - LOSS - (ct * prod));
        txtdisp.Text = Convert.ToString(Math.Round(prod1, 2)).Replace("Infinity", "0").Replace("NaN", "0");

        //======line efficieny formula===$$$=Production Qty/(Capacity as calculated above*85%)        
        cap = fgen.make_double(txtcapacity.Text.Trim());
        line = (prod / (cap * 85 / 100)) * 100;
        txtline_Eff.Text = Convert.ToString(Math.Round(line, 2)).Replace("Infinity", "0").Replace("NaN", "0");
        return;
    }

}