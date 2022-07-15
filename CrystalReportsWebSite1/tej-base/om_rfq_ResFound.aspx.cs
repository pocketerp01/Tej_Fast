using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.IO;

public partial class om_rfq_ResFound6 : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, col7, vardate, fromdt, todt, next_year, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;
    string mq0 = "";
    DataTable dtCol = new DataTable();
    DataTable sg1_dt; DataRow sg1_dr;
    string Checked_ok;
    string save_it;
    string Prg_Id, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
    string mq1, mq2, mq3, mq4, mq5, mq6;
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
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    next_year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            set_Val();
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
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false; btnprint.Disabled = false; btnlbl4.Enabled = false; btnChild.Enabled = false;
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
        btnlbl4.Enabled = true;
        btnChild.Enabled = true;
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
        doc_nf.Value = "ORDNO";
        doc_df.Value = "ORDDT";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "WB_SORFQ";
        lblheader.Text = "RFQ Respond Foundry";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "RF");
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
                // original SQuery = "select distinct a.branchcd||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr ,trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,a.acode,trim(b.aname) as customer,a.icode,i.iname,a.type,to_char(a.orddt,'yyyymmdd') as vdd from wb_porfq a,famst b,item i where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='ER' and a.orddt between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_date('" + frm_CDT2 + "','dd/mm/yyyy') order by vdd,rfq_no";
                SQuery = "SELECT trim(a.fstr) as fstr,trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,A.TYPE,TRIM(a.icode) AS CODE,TRIM(i.iname) AS ITEM_NAME,to_char(a.orddt,'yyyymmdd') as vdd from (select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,'ENQUIRY REGISTER' AS TYPE,1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type ='ER' and nvl(trim(app_by),'-')!='C' union all select distinct branchcd||'ER'||trim(INVNO)||to_char(INVDATE,'dd/mm/yyyy') as fstr,INVNO,INVDATE,icode,'ENQUIRY REGISTER' AS TYPE,-1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='RF' union all select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,'ENG. CHANGE NOTIFICATION' AS TYPE,1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type ='EC' and nvl(trim(app_by),'-')!='C' union all select distinct branchcd||'EC'||trim(INVNO)||to_char(INVDATE,'dd/mm/yyyy') as fstr,INVNO,INVDATE,icode,'ENG. CHANGE NOTIFICATION' AS  TYPE,-1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='RF' )a,item i where trim(a.icode)=trim(i.icode) GROUP BY trim(a.fstr),trim(a.ordno),to_char(a.orddt,'dd/mm/yyyy'),TRIM(a.icode),TRIM(i.iname),to_char(a.orddt,'yyyymmdd'),A.TYPE HAVING SUM(QTY)>0 ORDER BY VDD,RFQ_NO";
                SQuery = "SELECT trim(a.fstr) as fstr,trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,A.TYPE,TRIM(a.icode) AS CODE,TRIM(i.iname) AS ITEM_NAME,to_char(a.orddt,'yyyymmdd') as vdd from (select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,'ENQUIRY REGISTER' AS TYPE,1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type ='ER' and nvl(trim(app_by),'-')!='C' union all select trim(pordno) as fstr,INVNO,INVDATE,icode,'ENQUIRY REGISTER' AS TYPE,-1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='RF' union all select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,'ENG. CHANGE NOTIFICATION' AS TYPE,1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type ='EC' and nvl(trim(app_by),'-')!='C' union all select distinct trim(pordno) as fstr,INVNO,INVDATE,icode,'ENG. CHANGE NOTIFICATION' AS  TYPE,-1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='RF' )a,item i where trim(a.icode)=trim(i.icode) GROUP BY trim(a.fstr),trim(a.ordno),to_char(a.orddt,'dd/mm/yyyy'),TRIM(a.icode),TRIM(i.iname),to_char(a.orddt,'yyyymmdd'),A.TYPE HAVING SUM(QTY)>0 ORDER BY VDD,RFQ_NO";
                SQuery = "SELECT trim(a.fstr) as fstr,trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,A.TYPE,TRIM(a.icode) AS CODE,TRIM(i.iname) AS ITEM_NAME,to_char(a.orddt,'yyyymmdd') as vdd from (select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,'ENQUIRY REGISTER' AS TYPE,1 AS QTY,PDISC,0 AS PDISC1 from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type ='ER' and nvl(trim(app_by),'-')!='C' union all select trim(pordno) as fstr,INVNO,INVDATE,icode,'ENQUIRY REGISTER' AS TYPE,-1 AS QTY,0 AS PDISC,PDISC AS PDISC1 from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='RF' AND SUBSTR(TRIM(PORDNO),3,2)='ER' union all select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,'ENG. CHANGE NOTIFICATION' AS TYPE,1 AS QTY,PDISC,0 AS PDISC1 from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type ='EC' and nvl(trim(app_by),'-')!='C' union all select trim(pordno) as fstr,INVNO,INVDATE,icode,'ENG. CHANGE NOTIFICATION' AS TYPE,-1 AS QTY,0 AS PDISC,PDISC AS PDISC1 from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='RF' AND SUBSTR(TRIM(PORDNO),3,2)='EC')a,item i where trim(a.icode)=trim(i.icode) GROUP BY trim(a.fstr),trim(a.ordno),to_char(a.orddt,'dd/mm/yyyy'),TRIM(a.icode),TRIM(i.iname),to_char(a.orddt,'yyyymmdd'),A.TYPE HAVING ((SUM(PDISC)-SUM(PDISC1))>0 OR SUM(QTY)>0) ORDER BY VDD,RFQ_NO";
                break;

            case "TICODE":
                SQuery = "select trim(a.icode)||trim(a.ibcode) as fstr, trim(a.icode) as parent_code,trim(a.ibcode) as child_code,i.iname as child_name from itemosp a,item i where trim(a.ibcode)=trim(i.icode) and a. icode like '9%' and a.icode='" + txtIcode.Text.Trim() + "' order by a.srno";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "SELECT distinct trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr,trim(a.ORDNO) as Entry_no,TO_CHAR(a.ORDDT,'DD/MM/YYYY') as entry_dt,a.INVNO as RFQ_no,to_char(a.INVdate,'dd/mm/yyyy') as RFQ_date,a.amd_no as child_code,trim(a.icode) as item_code,C.INAME,to_char(a.orddt,'yyyymmdd') as vdd FROM " + frm_tabname + " a,item c WHERE trim(a.icode)=trim(c.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.type='" + frm_vty + "' ORDER BY VDD DESC,Entry_no DESC";
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
            // if want to ask popup at the time of new            
            hffield.Value = "New";

            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
            // else comment upper code 
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        frm_vty = vty;
        lbl1a.Text = vty;
        mq0 = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND type='" + frm_vty + "'";
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, mq0, 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        btnlbl4.Focus();
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        disablectrl();
        fgen.EnableForm(this.Controls);
        // Popup asking for Copy from Older Data
        fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        hffield.Value = "NEW_E";
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
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return;
        }
        if (txtRFQ.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Select RFQ Details");
            btnlbl4.Focus(); return;
        }
        if (txtChildCode.Text.Trim().Length <= 1)
        {
            mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select count(trim(ibcode)) as totchild from itemosp where trim(icode)='" + txtIcode.Text + "'", "totchild");
            if (mq0.Trim() != "0")
            {
                fgen.msg("-", "AMSG", "This Item Code Has Child Parts.'13' Please Select Its Child Parts"); txtChildCode.Focus(); return;
            }
        }
        if (sg1.Rows.Count < 1)
        {
            fgen.msg("-", "AMSG", "Please Select Atleast One Attachment");
            return;
        }
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            if (((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).SelectedItem.Text.Trim() == "PLEASE SELECT")
            {
                fgen.msg("-", "AMSG", "Please Select Either Yes / No / Conditionally_Approve For " + ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim());
                return;
            }
            if (((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).SelectedItem.Text.Trim() == "YES")
            {
                if (sg1.Rows[i].Cells[5].Text.Trim().Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Add Attachment For " + ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim());
                    return;
                }
            }
            if (((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).SelectedItem.Text.Trim() == "NO")
            {
                if (sg1.Rows[i].Cells[5].Text.Trim().Length > 1)
                {
                    fgen.msg("-", "AMSG", "For " + ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() + " ,Attchment Is Added.'13' But 'No' Is Selected");
                    return;
                }
            }
            if (((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).SelectedItem.Text.Trim() == "CONDITIONALLY_APPROVE")
            {
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Fill Remarks For " + ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim());
                    return;
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
        sg1.DataSource = null;
        sg1.DataBind();
        ViewState["sg1"] = null;
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
        fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
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
            if (CP_BTN.Trim().Substring(0, 3) == "BTN" || CP_BTN.Trim().Substring(0, 3) == "SG1" || CP_BTN.Trim().Substring(0, 3) == "SG2" || CP_BTN.Trim().Substring(0, 3) == "SG3" || CP_BTN.Trim().Substring(0, 3) == "SG4")
            {
                btnval = CP_BTN;
            }
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "0");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "0");

        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                //mq1 = "select trim(pordno) as pordno from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='MC' and trim(pordno)='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                mq1 = "select nvl(trim(test),'-') as test from " + frm_tabname + " where  branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                mq2 = fgen.seek_iname(frm_qstr, frm_cocd, mq1, "test");
                if (mq2 == "0")
                {
                    // FOR DELETING TEST FLAG FIELD FROM LAST TABLE I.E TYPE EC OR ER
                    mq4 = "select trim(a.pordno) as pordno from " + frm_tabname + " a where a.branchcd||trim(a.type)||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                    mq5 = fgen.seek_iname(frm_qstr, frm_cocd, mq4, "pordno"); // ER OR EC NO.
                    mq6 = "update wb_sorfq set test='-' where branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + mq5 + "'"; ;
                    fgen.execute_cmd(frm_qstr, frm_cocd, mq6);

                    // Deleing data from Main Table
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                    // Deleing data from WSr Ctrl Table
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                    // Saving Deleting History
                    fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Type =" + frm_vty + " Deleted");
                    fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                    clearctrl(); fgen.ResetForm(this.Controls);
                }
                else
                {
                    fgen.msg("-", "AMSG", "Costing Entry Is Done.'13' Entry Cannot Be Deleted.");
                    clearctrl(); fgen.ResetForm(this.Controls);
                }
            }
        }
        else if (hffield.Value == "NEW_E")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                hffield.Value = "COPY_OLD";
                make_qry_4_popup();
                fgen.Fn_open_sseek(lblheader.Text + " Entry For Copy", frm_qstr);
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
                    #region
                    SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt,I.INAME,I.CPARTNO from " + frm_tabname + " a,ITEM I where TRIM(A.ICODE)=TRIM(I.ICODE) and a.branchcd||a.type||trim(a.ORDNO)||to_Char(a.ORDDT,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        //txtIcode.Text = dt.Rows[0]["ICODE"].ToString().Trim();
                        //txtIname.Text = dt.Rows[0]["INAME"].ToString().Trim();
                        //txtCpartNo.Text = dt.Rows[0]["CPARTNO"].ToString().Trim();
                        //txtAcode.Text = dt.Rows[0]["ACODE"].ToString().Trim();
                        //txtFstr.Text = dt.Rows[0]["PORDNO"].ToString().Trim();
                        //txtRFQ.Text = dt.Rows[0]["INVNO"].ToString().Trim();
                        //txtRFQDt.Text = Convert.ToDateTime(dt.Rows[0]["INVDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                        //txtTarget.Text = dt.Rows[0]["QTYBAL"].ToString().Trim();
                        txtFinish.Text = dt.Rows[0]["OTCOST2"].ToString().Trim();
                        txtCast.Text = dt.Rows[0]["WK1"].ToString().Trim();
                        txtBunch.Text = dt.Rows[0]["WK2"].ToString().Trim();
                        txtCavity.Text = dt.Rows[0]["WK3"].ToString().Trim();
                        txtCore.Text = dt.Rows[0]["OTCOST3"].ToString().Trim();
                        txtCore_Wt.Text = dt.Rows[0]["PSIZE"].ToString().Trim();
                        txtSleeve.Text = dt.Rows[0]["QTYORD"].ToString().Trim();
                        txtTooling.Text = dt.Rows[0]["QTYSUPP"].ToString().Trim();
                        txtRaw.Text = dt.Rows[0]["MODE_TPT"].ToString().Trim();
                        txtMaterial.Text = dt.Rows[0]["TR_INSUR"].ToString().Trim();
                        txtYield.Text = dt.Rows[0]["DESP_TO"].ToString().Trim();
                        txtCore_Type.Text = dt.Rows[0]["FREIGHT"].ToString().Trim();
                        txtSurface.Text = dt.Rows[0]["DOC_THR"].ToString().Trim();
                        txtHeat.Text = dt.Rows[0]["PACKING"].ToString().Trim();
                        txtLab.Text = dt.Rows[0]["PAYMENT"].ToString().Trim();
                        txtChemistry.Text = dt.Rows[0]["BANK"].ToString().Trim();
                        txtExtra.Text = dt.Rows[0]["STAX"].ToString().Trim();
                        txtFeasiblity.Text = dt.Rows[0]["EXC"].ToString().Trim();
                        txtRejection.Text = dt.Rows[0]["IOPR"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["remark"].ToString().Trim();
                        txtTest.Text = dt.Rows[0]["TEST"].ToString().Trim();

                        //create_tab();
                        //sg1_dr = null;
                        //for (i = 0; i < dt.Rows.Count; i++)
                        //{
                        //    sg1_dr = sg1_dt.NewRow();
                        //    sg1_dr["sg1_t1"] = dt.Rows[i]["kindattn"].ToString().Trim();
                        //    sg1_dr["sg1_t2"] = dt.Rows[i]["st31no"].ToString().Trim();
                        //    sg1_dr["sg1_t3"] = dt.Rows[i]["atch2"].ToString().Trim();
                        //    sg1_dr["sg1_t4"] = dt.Rows[i]["atch3"].ToString().Trim();
                        //    sg1_dr["sg1_t5"] = dt.Rows[i]["desc_"].ToString().Trim();
                        //    sg1_dt.Rows.Add(sg1_dr);
                        //}
                        //sg1.DataSource = sg1_dt;
                        //sg1.DataBind();
                        //ViewState["sg1"] = sg1_dt;
                        //fgen.EnableForm(this.Controls);
                        //for (int i = 0; i < sg1.Rows.Count; i++)
                        //{
                        //    string hf = ((HiddenField)sg1.Rows[i].FindControl("cmd1")).Value;
                        //    if (hf != "" && hf != "-")
                        //    {
                        //        ((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).Items.FindByText(hf).Selected = true;
                        //    }
                        //    //if (i != sg1.Rows.Count - 1) // FOR STOPPING IT FROM DISABLING LAST ROW
                        //    //{
                        //    //    sg1.Rows[i].Cells[0].Enabled = false;
                        //    //    ((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).Enabled = false;
                        //    //    ((FileUpload)sg1.Rows[i].FindControl("FileUpload1")).Enabled = false;
                        //    //    ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Enabled = false;
                        //    //}
                        //}
                        fgen.EnableForm(this.Controls);
                        disablectrl(); btnlbl4.Focus();
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
                    SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt,I.INAME,I.CPARTNO from " + frm_tabname + " a,ITEM I where TRIM(A.ICODE)=TRIM(I.ICODE) and a.branchcd||a.type||trim(a.ORDNO)||to_Char(a.ORDDT,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        txtvchnum.Text = dt.Rows[0]["ordno"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["orddt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtIcode.Text = dt.Rows[0]["ICODE"].ToString().Trim();
                        txtIname.Text = dt.Rows[0]["INAME"].ToString().Trim();
                        txtCpartNo.Text = dt.Rows[0]["CPARTNO"].ToString().Trim();
                        txtAcode.Text = dt.Rows[0]["ACODE"].ToString().Trim();
                        txtFstr.Text = dt.Rows[0]["PORDNO"].ToString().Trim();
                        txtRFQ.Text = dt.Rows[0]["INVNO"].ToString().Trim();
                        txtRFQDt.Text = Convert.ToDateTime(dt.Rows[0]["INVDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtTarget.Text = dt.Rows[0]["QTYBAL"].ToString().Trim();
                        txtFinish.Text = dt.Rows[0]["OTCOST2"].ToString().Trim();
                        txtCast.Text = dt.Rows[0]["WK1"].ToString().Trim();
                        txtBunch.Text = dt.Rows[0]["WK2"].ToString().Trim();
                        txtCavity.Text = dt.Rows[0]["WK3"].ToString().Trim();
                        txtCore.Text = dt.Rows[0]["OTCOST3"].ToString().Trim();
                        txtCore_Wt.Text = dt.Rows[0]["PSIZE"].ToString().Trim();
                        txtSleeve.Text = dt.Rows[0]["QTYORD"].ToString().Trim();
                        txtTooling.Text = dt.Rows[0]["QTYSUPP"].ToString().Trim();
                        txtRaw.Text = dt.Rows[0]["MODE_TPT"].ToString().Trim();
                        txtMaterial.Text = dt.Rows[0]["TR_INSUR"].ToString().Trim();
                        txtYield.Text = dt.Rows[0]["DESP_TO"].ToString().Trim();
                        txtCore_Type.Text = dt.Rows[0]["FREIGHT"].ToString().Trim();
                        txtSurface.Text = dt.Rows[0]["DOC_THR"].ToString().Trim();
                        txtHeat.Text = dt.Rows[0]["PACKING"].ToString().Trim();
                        txtLab.Text = dt.Rows[0]["PAYMENT"].ToString().Trim();
                        txtChemistry.Text = dt.Rows[0]["BANK"].ToString().Trim();
                        txtExtra.Text = dt.Rows[0]["STAX"].ToString().Trim();
                        txtFeasiblity.Text = dt.Rows[0]["EXC"].ToString().Trim();
                        txtRejection.Text = dt.Rows[0]["IOPR"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["remark"].ToString().Trim();
                        txtTest.Text = dt.Rows[0]["TEST"].ToString().Trim();
                        txtParentChild.Text = dt.Rows[0]["DELV_ITEM"].ToString().Trim();
                        txtChildCode.Text = dt.Rows[0]["AMD_NO"].ToString().Trim();
                        txtChildName.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(iname) as iname from item where icode='" + txtChildCode.Text.Trim() + "'", "iname");

                        mq0 = "select a.kindattn,a.st31no,a.atch2,a.atch3,'-' as desc_ from  wb_sorfq a where trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')='" + txtFstr.Text.Trim() + "' union all SELECT a.kindattn,a.st31no,a.atch2,a.atch3,a.desc_ from wb_sorfq a where trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "'";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt2.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_t1"] = dt2.Rows[i]["kindattn"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt2.Rows[i]["st31no"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt2.Rows[i]["atch2"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt2.Rows[i]["atch3"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt2.Rows[i]["desc_"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        ViewState["sg1"] = sg1_dt;
                        fgen.EnableForm(this.Controls);
                        for (int i = 0; i < sg1.Rows.Count; i++)
                        {
                            string hf = ((HiddenField)sg1.Rows[i].FindControl("cmd1")).Value;
                            if (hf != "" && hf != "-")
                            {
                                ((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).Items.FindByText(hf).Selected = true;
                            }
                            if (i != sg1.Rows.Count - 1) // FOR STOPPING IT FROM DISABLING LAST ROW
                            {
                                sg1.Rows[i].Cells[0].Enabled = false;
                                ((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).Enabled = false;
                                ((FileUpload)sg1.Rows[i].FindControl("FileUpload1")).Enabled = false;
                                ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Enabled = false;
                            }
                        }
                        dt.Dispose();
                        sg1_dt.Dispose();
                        disablectrl();
                        setColHeadings();
                        btnlbl4.Enabled = false;
                        btnChild.Enabled = false;
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "AK12");
                    fgen.fin_maint_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    SQuery = "select a.branchcd||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr,trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,a.icode,i.iname,i.cpartno,a.qtybal,a.acode,a.kindattn,a.st31no,a.atch2,a.atch3 from " + frm_tabname + " a,item i where trim(a.icode)=trim(i.icode) and a.branchcd||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')='" + col1 + "' order by a.srno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtRFQ.Text = dt.Rows[0]["rfq_no"].ToString().Trim();
                        txtRFQDt.Text = dt.Rows[0]["rfq_date"].ToString().Trim();
                        txtAcode.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtFstr.Text = dt.Rows[0]["fstr"].ToString().Trim();
                        txtIcode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtIname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtCpartNo.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                        txtTarget.Text = dt.Rows[0]["qtybal"].ToString().Trim();

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["kindattn"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["st31no"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["atch2"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["atch3"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dr = sg1_dt.NewRow();
                        sg1_dr["sg1_t1"] = "FEASIBILITY";
                        sg1_dr["sg1_t3"] = "-";
                        sg1_dr["sg1_t4"] = "-";
                        sg1_dr["sg1_t5"] = "-";
                        sg1_dt.Rows.Add(sg1_dr);
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        ViewState["sg1"] = sg1_dt;
                        for (int i = 0; i < sg1.Rows.Count - 1; i++)
                        {
                            string hf = ((HiddenField)sg1.Rows[i].FindControl("cmd1")).Value;
                            if (hf != "" && hf != "-")
                            {
                                ((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).Items.FindByText(hf).Selected = true;
                            }
                            sg1.Rows[i].Cells[0].Enabled = false;
                            ((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).Enabled = false;
                            ((FileUpload)sg1.Rows[i].FindControl("FileUpload1")).Enabled = false;
                            ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Enabled = false;
                        }
                    }
                    txtRaw.Focus();
                    break;

                case "TICODE":
                    if (col1.Length <= 0) return;
                    mq0 = "select trim(delv_item) as fstr,trim(amd_no) as child from wb_sorfq where branchcd='" + frm_mbr + "' and type='RF' and trim(pordno)='" + txtFstr.Text.Trim() + "' and trim(delv_item)='" + col1 + "'";
                    mq1 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "child");
                    if (mq1.Trim().Length > 1)
                    {
                        fgen.msg("-", "AMSG", lblheader.Text + " Of This Item (" + mq1 + ") Is Already Made.'13'Please Select Another Code");
                        return;
                    }
                    SQuery = "select a.ibcode,i.iname from itemosp a,item i where trim(a.ibcode)=trim(i.icode) and trim(a.icode)||trim(a.ibcode)='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtChildCode.Text = dt.Rows[0]["ibcode"].ToString().Trim();
                        txtChildName.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtParentChild.Text = col1;
                    }
                    txtMaterial.Focus();
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
            SQuery = "SELECT trim(a.ORDNO) as Entry_no,TO_CHAR(a.ORDDT,'DD/MM/YYYY') as entry_dt,a.INVNO as RFQ_no,to_char(a.INVdate,'dd/mm/yyyy') as RFQ_date,trim(a.icode) as item_code,C.INAME,c.cpartno as drg,a.mode_tpt as raw_partno,a.amd_no as child_code,b.iname as child_name,a.tr_insur as material_grade,a.qtybal as target_bal,a.OTCOST2 as finish_wt,a.wk1 as cast_wt,a.wk2 as bunch_wt,a.desp_to as yield_per,a.wk3 as no_of_cavity,a.OTCOST3 as no_of_core,a.psize as core_wt,a.freight as core_type,a.doc_thr as surface_treatment,a.packing as heat_treatment,a.qtyord as sleeve_cost,a.payment as lab_consent,a.bank as chemical_details,a.stax as extra_process,a.exc as feasibility,a.iopr as rejection_per,a.qtysupp as tooling_cost,a.remark,a.atch2 as file_name,a.atch3 as filepath,a.kindattn as drawing_type,a.st31no as yes_no,a.desc_ as cond_remarks,to_char(a.orddt,'yyyymmdd') as vdd FROM item c," + frm_tabname + " a left join item b on trim(a.amd_no)=trim(b.icode) WHERE trim(a.icode)=trim(c.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.type='" + frm_vty + "' AND a.ORDDT " + DateRange + " ORDER BY VDD DESC,TRIM(a.ORDNO) DESC,a.srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            string last_entdt;
            //checks
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'", "ldt");
            if (last_entdt == "0" || edmode.Value == "Y")
            {
            }
            else
            {
                if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + ",Please Check !!");
                }
            }
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + " ,Please Check !!");
            }
            // -----------------------------
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
                            save_it = "Y";

                            if (save_it == "Y")
                            {
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                string ffff = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, "between to_date('01/04/2019','dd/mm/yyyy') and to_date('31/03/2020','dd/mm/yyyy')", frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();

                        if (edmode.Value == "Y")
                        {
                            string mycmd = "";
                            mycmd = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/MM/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mycmd);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        string mycmd3 = "";  // SAVING FLAG IN ER OR EC ENTRY
                        mycmd3 = "update " + frm_tabname + " set TEST='R' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/MM/yyyy')='" + txtFstr.Text.Trim() + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, mycmd3);

                        if (edmode.Value == "Y")
                        {
                            mq5 = "update " + frm_tabname + " set test='" + txtTest.Text.Trim() + "' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mq5);
                        }

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            string mycmd2 = "";
                            mycmd2 = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mycmd2);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully!!");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); sg1.DataSource = null; sg1.DataBind(); ViewState["sg1"] = null;
                    }
                    catch (Exception ex)
                    {
                        btnsave.Disabled = false;
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
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        txtChildCode.Text = "";
        txtChildName.Text = "";
        txtParentChild.Text = "";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select RFQ Entry", frm_qstr);
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
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
       // mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select sum(pdisc) as pdisc from wb_sorfq where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(pordno)='" + txtFstr.Text.Trim() + "' and trim(icode)='" + txtIcode.Text.Trim() + "'", "pdisc");
        z = 1;
        for (int i = 3; i < sg1.Rows.Count; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["ordno"] = frm_vnum.Trim().ToUpper();
            oporow["orddt"] = txtvchdate.Text.Trim().ToUpper();
            oporow["ICODE"] = txtIcode.Text.Trim().ToUpper();
            oporow["ACODE"] = txtAcode.Text.Trim().ToUpper();
            oporow["INVNO"] = txtRFQ.Text.Trim().ToUpper();
            oporow["INVDATE"] = txtRFQDt.Text.Trim().ToUpper();
            oporow["QTYBAL"] = fgen.make_double(txtTarget.Text.Trim().ToUpper());
            oporow["OTCOST2"] = fgen.make_double(txtFinish.Text.Trim().ToUpper());
            if (txtParentChild.Text.Length > 1)
            {
                oporow["PDISC"] = 1;
            }
            else
            {
                oporow["PDISC"] = 0;
            }
            oporow["PEXC"] = 0;
            oporow["PTAX"] = 0;
            oporow["WK1"] = fgen.make_double(txtCast.Text.Trim().ToUpper());
            oporow["WK2"] = fgen.make_double(txtBunch.Text.Trim().ToUpper());
            oporow["WK3"] = fgen.make_double(txtCavity.Text.Trim().ToUpper());
            oporow["OTCOST3"] = fgen.make_double(txtCore.Text.Trim().ToUpper());
            oporow["PSIZE"] = fgen.make_double(txtCore_Wt.Text.Trim().ToUpper());
            oporow["QTYORD"] = fgen.make_double(txtSleeve.Text.Trim().ToUpper());
            oporow["QTYSUPP"] = fgen.make_double(txtTooling.Text.Trim().ToUpper());
            oporow["MODE_TPT"] = txtRaw.Text.Trim().ToUpper();
            oporow["TR_INSUR"] = txtMaterial.Text.Trim().ToUpper();
            oporow["DESP_TO"] = txtYield.Text.Trim().ToUpper();
            oporow["FREIGHT"] = txtCore_Type.Text.Trim().ToUpper();
            oporow["DOC_THR"] = txtSurface.Text.Trim().ToUpper();
            oporow["PACKING"] = txtHeat.Text.Trim().ToUpper();
            oporow["PAYMENT"] = txtLab.Text.Trim().ToUpper();
            oporow["BANK"] = txtChemistry.Text.Trim().ToUpper();
            oporow["STAX"] = txtExtra.Text.Trim().ToUpper();
            //oporow["EXC"] = txtFeasiblity.Text.Trim().ToUpper();
            oporow["EXC"] = "-";
            oporow["IOPR"] = txtRejection.Text.Trim().ToUpper();
            oporow["remark"] = txtrmk.Text.Trim().ToUpper();
            oporow["UNIT"] = "-";
            oporow["PORDNO"] = txtFstr.Text.Trim().ToUpper();
            oporow["PORDDT"] = vardate;
            oporow["DELIVERY"] = 0;
            oporow["DEL_MTH"] = 0;
            oporow["DEL_WK"] = 0;
            oporow["DEL_DATE"] = vardate;
            oporow["TERM"] = "-";
            oporow["DELV_TERM"] = "-";
            oporow["TERM"] = "-";
            oporow["INST"] = "-";
            oporow["REFDATE"] = vardate;
            oporow["DESC_"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper();
            oporow["PR_NO"] = "-";
            oporow["AMD_NO"] = txtChildCode.Text.Trim().ToUpper();
            oporow["DEL_SCH"] = "-";
            oporow["TAX"] = "-";
            oporow["WK4"] = 0;
            oporow["VEND_WT"] = 0;
            oporow["STORE_NO"] = "-";
            oporow["APP_BY"] = "-";
            oporow["APP_DT"] = vardate;
            oporow["ISSUE_NO"] = 0;
            oporow["PFLAG"] = 0;
            oporow["PR_DT"] = vardate;
            oporow["TEST"] = "-";
            oporow["PBASIS"] = "-";
            oporow["RATE_OK"] = 0;
            oporow["RATE_CD"] = 0;
            oporow["RATE_REJ"] = 0;
            oporow["PCESS"] = 0;
            oporow["DELV_ITEM"] = txtParentChild.Text.Trim().ToUpper();
            oporow["NXTMTH"] = 0;
            oporow["TRANSPORTER"] = "-";
            oporow["CSCODE"] = "-";
            oporow["EFFDATE"] = vardate;
            oporow["ST38NO"] = "-";
            oporow["NXTMTH2"] = 0;
            oporow["CURRENCY"] = "-";
            oporow["PEXCAMT"] = 0;
            oporow["PDISCAMT"] = 0;
            oporow["AMDTNO"] = 0;
            oporow["ORIGNALBR"] = "-";
            oporow["GSM"] = 0;
            oporow["CINAME"] = "-";
            oporow["IRATE"] = 0;
            oporow["OTCOST1"] = 0;
            oporow["O_QTY"] = 0;
            oporow["CHL_REF"] = "-";
            oporow["OTHAC1"] = "-";
            oporow["OTHAC2"] = "-";
            oporow["OTHAC3"] = "-";
            oporow["OTHAMT1"] = 0;
            oporow["OTHAMT2"] = 0;
            oporow["OTHAMT3"] = 0;
            oporow["D18NO"] = "-";
            oporow["TDISC_AMT"] = 0;
            oporow["CSCODE1"] = "-";
            oporow["PREFSOURCE"] = "-";
            oporow["POPREFIX"] = "-";
            oporow["RATE_DIFF"] = "-";
            oporow["RATE_COMM"] = 0;
            oporow["SPLRMK"] = "-";
            oporow["PDAYS"] = 0;
            oporow["EMAIL_STATUS"] = "-";
            oporow["CHK_BY"] = "-";
            oporow["CHK_DT"] = vardate;
            oporow["VALIDUPTO"] = vardate;
            oporow["ED_SERV"] = "-";
            oporow["ATCH1"] = "-";
            oporow["PDISCAMT2"] = 0;
            oporow["TXB_FRT"] = 0;
            oporow["PO_TOLR"] = 0;
            oporow["SRNO"] = z;
            oporow["ATCH2"] = sg1.Rows[i].Cells[5].Text.Trim();
            oporow["ATCH3"] = sg1.Rows[i].Cells[6].Text.Trim();
            oporow["KINDATTN"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
            oporow["ST31NO"] = ((DropDownList)sg1.Rows[i].FindControl("sg1_t2")).SelectedItem.Text.Trim().ToUpper();
            oporow["BILLCODE"] = "-";
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
            z++;
            oDS.Tables[0].Rows.Add(oporow);
        }
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "RF");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------
    protected void btnmrr_click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "mrr";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select MRR", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void Cal()
    {
        double cast_cavity = 0;
        cast_cavity = fgen.make_double(txtCast.Text) * fgen.make_double(txtCavity.Text);
        txtBunch.Text = (Math.Round((cast_cavity / fgen.make_double(txtYield.Text)) * 100, 2)).ToString();
    }
    //------------------------------------------------------------------------------------
    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        // Hidden Field
        sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t6", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            sg1.Columns[0].HeaderStyle.Width = 50;
            sg1.Columns[1].HeaderStyle.Width = 80;
            sg1.Columns[2].HeaderStyle.Width = 50;
            sg1.Columns[3].HeaderStyle.Width = 180;
            sg1.Columns[4].HeaderStyle.Width = 180;
            sg1.Columns[5].HeaderStyle.Width = 200;
            sg1.Columns[6].HeaderStyle.Width = 200;
            sg1.Columns[7].HeaderStyle.Width = 200;
            sg1.Columns[8].HeaderStyle.Width = 170;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = 0;
        if (var == "SG1_UPLD")
        {
            if (txtRFQ.Text.Length <= 1)
            {
                fgen.msg("-", "AMSG", "Please Select RFQ No.");
                return;
            }
            rowIndex = ((GridViewRow)((Button)e.CommandSource).NamingContainer).RowIndex;
        }
        else
        {
            rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        }
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);
        string filePath = "";
        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG1_RMV":
                filePath = sg1.Rows[index].Cells[6].Text.ToUpper();
                if (filePath.Length > 1)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    string secFilePath = Server.MapPath("~/tej-base/") + sg1.Rows[index].Cells[6].Text.Substring(sg1.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"), sg1.Rows[index].Cells[6].Text.ToUpper().Length - sg1.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"));
                    if (File.Exists(secFilePath))
                    {
                        File.Delete(secFilePath);
                    }
                }
                sg1.Rows[index].Cells[5].Text = "-";
                sg1.Rows[index].Cells[6].Text = "-";
                break;

            case "SG1_DWN":
                filePath = sg1.Rows[index].Cells[6].Text.ToUpper();
                if (filePath.Length > 1)
                {
                    Response.ContentType = ContentType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                    Response.WriteFile(filePath);
                    Response.End();
                }
                break;

            case "SG1_VIEW":
                if (sg1.Rows[index].Cells[6].Text.Trim().Length > 1)
                {
                    filePath = sg1.Rows[index].Cells[6].Text.Substring(sg1.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"), sg1.Rows[index].Cells[6].Text.ToUpper().Length - sg1.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
                }
                break;

            case "SG1_UPLD":
                string UploadedFile = ((FileUpload)sg1.Rows[index].FindControl("FileUpload1")).FileName;
                string filepath = @"c:\TEJ_ERP\UPLOAD\";
                string fileName = txtvchnum.Text.Trim() + fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY") + frm_CDT1.Replace(@"/", "_") + "~" + UploadedFile.Replace("&", "").Replace("%", "_");
                filepath = filepath + fileName;
                ((FileUpload)sg1.Rows[index].FindControl("FileUpload1")).PostedFile.SaveAs(filepath);
                ((FileUpload)sg1.Rows[index].FindControl("FileUpload1")).PostedFile.SaveAs(Server.MapPath("~/tej-base/Upload/") + fileName);
                sg1.Rows[index].Cells[5].Text = UploadedFile;
                sg1.Rows[index].Cells[6].Text = filepath;
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnUpload_Click(object sender, EventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnChild_Click(object sender, ImageClickEventArgs e)
    {
        if (txtIcode.Text.Trim().Length <= 1)
        {
            fgen.msg("-", "AMSG", "Please Select RFQ First");
            return;
        }
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select SF Code", frm_qstr);
    }
    //------------------------------------------------------------------------------------
}