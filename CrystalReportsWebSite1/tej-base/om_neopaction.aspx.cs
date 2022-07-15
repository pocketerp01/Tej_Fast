using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

//ACT_TKEN

public partial class om_neopaction : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", xStartDt = "", Enable = "", mq0, mq1;
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
            btnlist.Visible = false;
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
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnprint.Disabled = false;
        create_tab();
        sg1_add_blankrows();
        //    btnlbl4.Enabled = false;
        // btnlbl7.Enabled = false;
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; btnprint.Disabled = true;
        //  btnlbl4.Enabled = true;
        //  btnlbl7.Enabled = true;
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
        frm_tabname = "scratch2";
        frm_vty = "AC";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        lbl1a.Text = frm_vty;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        try
        {
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE scratch2 MODIFY COL15 VARCHAR2(1000) DEFAULT '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE scratch2 MODIFY COL16 VARCHAR2(1000) DEFAULT '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE scratch2 MODIFY COL17 VARCHAR2(1000) DEFAULT '-'");
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE scratch2 MODIFY COL18 VARCHAR2(1000) DEFAULT '-'");
        }
        catch { }
        if (frm_cocd == "CCEL")
        {
            spnjobno.Visible = true; txtjobno.Visible = true;
            tddivision.Text = "Department";
            lblheader.Text = "Action Taken on Customer Request";
            SEL1.Visible = false; SEL2.Visible = false; tdbatch1.Visible = false;
        }
        else if (frm_cocd == "SEL")
        {
            spnjobno.Visible = false; txtjobno.Visible = false;
            tddivision.Text = "DivisionofComplaint";
            lblheader.Text = "Action Taken on Customer Complaint";
            lblBatch.InnerText = "Machine_Sr.No";
            tdinvoice.Text = "Req. No";
            txtinvbtch.ReadOnly = true;
            tdinvoiceDT.Text = "Date";            
            //DivParty.Visible = false;
            txtinvno.Visible = false;
            MadeBy.Visible = false;
            //if (frm_ulvl != "0")
            //    btninvno.Visible = false;
        }
        else
        {
            spnjobno.Visible = false; txtjobno.Visible = false;
            tddivision.Text = "DivisionofComplaint";
            lblheader.Text = "Action Taken on Customer Complaint";
            SEL1.Visible = false; SEL2.Visible = false; tdbatch1.Visible = false; DivAddress.Visible = false;
        }
        if (frm_cocd == "SRIS")
        {
            tdrply.Text = "Line Detail";
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
            case "New":
                if (frm_ulvl != "0")
                    SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.vchnum as complnt_no,to_char(a.vchdate,'dd/mm/yyyy') as compnt_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='CC' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " and trim(a.ent_by)='" + frm_uname + "' and substr(a.app_by,1,3)='[A]' and nvl(trim(a.chk_by),'-')='-' order by to_char(a.vchdate,'dd/mm/yyyy') desc";
                else
                {
                    SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.vchnum as complnt_no,to_char(a.vchdate,'dd/mm/yyyy') as compnt_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,a.col2 as type_of_complaint,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='CC' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " and substr(a.app_by,1,3)='[A]' and nvl(trim(a.chk_by),'-')='-' order by to_char(a.vchdate,'dd/mm/yyyy') desc";//old and real qry
                    if (frm_cocd == "SRIS") SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.vchnum as complnt_no,to_char(a.vchdate,'dd/mm/yyyy') as compnt_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='CC' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " /*and trim(a.ent_by)='" + frm_uname + "'*/ and substr(a.app_by,1,3)='[A]' and nvl(trim(a.chk_by),'-')='-' order by to_char(a.vchdate,'dd/mm/yyyy') desc";
                }
                if (frm_cocd == "SEL")
                {
                    SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.vchnum as complnt_no,to_char(a.vchdate,'dd/mm/yyyy') as compnt_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,a.col8 as machine_srno,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='CC' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " and substr(a.app_by,1,3)='[A]' and nvl(trim(a.chk_by),'-')='-' order by to_char(a.vchdate,'dd/mm/yyyy') desc";
                    if (frm_ulvl != "0") SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.vchnum as complnt_no,to_char(a.vchdate,'dd/mm/yyyy') as compnt_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='CC' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " and trim(a.col15)='" + frm_uname + "' and substr(a.app_by,1,3)='[A]' and nvl(trim(a.chk_by),'-')='-' order by to_char(a.vchdate,'dd/mm/yyyy') desc";
                }
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + ((TextBox)gr.FindControl("sg1_t2")).Text.Trim() + "'";
                    else col1 = "'" + ((TextBox)gr.FindControl("sg1_t2")).Text.Trim() + "'";
                }
                if (col1.Length <= 0) col1 = "'-'";
                SQuery = "";
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
                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(Trim(icode))>4 and icode like '9%' and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname";
                break;

            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "Print_E":
                if (frm_ulvl != "0") SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.col5 as inv_no,a.col6 as inv_Dt,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,to_char(vchdate,'yyyymmdd') as vdd from scratch2 a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE)  and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " and trim(a.ent_by)='" + frm_uname + "' order by vdd desc";
                else
                {
                    SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.col5 as inv_no,a.col6 as inv_Dt,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,a.col2 as type_of_complaint,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,to_char(vchdate,'yyyymmdd') as vdd from scratch2 a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE)  and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,entry_no desc";
                }
                if (frm_cocd == "SRIS") SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.col5 as inv_no,a.col6 as inv_Dt,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,a.col2 as type_of_complaint,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,to_char(vchdate,'yyyymmdd') as vdd from scratch2 a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc";
                if (frm_cocd == "SEL") SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.col22 as reqno,a.col23 as reqdt,d.col8 as machine_srno,a.col5 as inv_no,a.col6 as inv_Dt,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,to_char(A.vchdate,'yyyymmdd') as vdd from scratch2 a,famst b,item c,scratch d where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||trim(a.col22)||trim(a.col23)||Trim(A.acode)||trim(A.icode)=d.branchcd||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')||Trim(d.acode)||trim(d.icode) and a.type='" + frm_vty + "' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                {
                    if (frm_ulvl != "0") SQuery = "Select distinct trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.col5 as inv_no,a.col6 as inv_Dt,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,to_char(vchdate,'yyyymmdd') as vdd from scratch2 a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE)  and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " and trim(a.ent_by)='" + frm_uname + "' order by vdd desc";
                    else
                    {
                        SQuery = "Select distinct trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.col5 as inv_no,a.col6 as inv_Dt,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,a.col2 as type_of_complaint,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,to_char(vchdate,'yyyymmdd') as vdd from scratch2 a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE)  and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,entry_no desc";
                    }
                    if (frm_cocd == "SRIS") SQuery = "Select distinct trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.col5 as inv_no,a.col6 as inv_Dt,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,a.col2 as type_of_complaint,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,to_char(vchdate,'yyyymmdd') as vdd from scratch2 a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc";
                    if (frm_cocd == "SEL") SQuery = "Select distinct trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr, a.col22 as reqno,a.col23 as reqdt,d.col8 as machine_srno,a.col5 as inv_no,a.col6 as inv_Dt,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,b.aname as party_name,a.acode as code,c.iname as item_name,a.icode as erpcode,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,to_char(A.vchdate,'yyyymmdd') as vdd from scratch2 a,famst b,item c,scratch d where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||trim(a.col22)||trim(a.col23)||Trim(A.acode)||trim(A.icode)=d.branchcd||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')||Trim(d.acode)||trim(d.icode) and a.type='" + frm_vty + "' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
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
            typePopup = "Y";// IT IS SET HERE BECAUSE AT THE TIME OF NEW , A POPUP NEED TO BE ASKED.
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            set_Val();
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Customer Complaint", frm_qstr);
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
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' and vchdate " + DateRange + " AND TYPE='" + frm_vty + "' ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        //btnlbl4.Focus();
        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();
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
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus();
            return;
        }

        if (txtinvno.Text.Length <= 1)
        {
            fgen.msg("-", "AMSG", "Please Select Invoice No."); txtinvno.Focus();
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

        ViewState["sg1"] = null;
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')||TRIm(acode)||trim(icode)='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 16) + "'");
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
                    if (col1 == "") return;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "Select distinct a.vchnum as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.INVNO AS pono ,to_char(a.INVDATE,'dd/mm/yyyy') AS podate ,b.aname ,a.acode ,c.iname ,a.icode ,a.srno,a.COL1 as app,a.COL2,a.COL3,a.COL4,a.REMARKS as rmk,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.naration,a.col8,b.addr1||','||b.addr2||','||b.addr3 as address from scratch a ,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' order by a.srno");
                    if (dt.Rows.Count > 0)
                    {
                        txtcvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim(); txtcvchdate.Text = dt.Rows[0]["vchdate"].ToString().Trim();
                        txtinvno.Text = dt.Rows[0]["pono"].ToString().Trim(); txtinvdate.Text = dt.Rows[0]["podate"].ToString().Trim();
                        txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                        txtsacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txtsaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                        txticode.Text = dt.Rows[0]["icode"].ToString().Trim(); txtiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtsicode.Text = dt.Rows[0]["icode"].ToString().Trim(); txtsiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["naration"].ToString().Trim(); txtntrcmpln.Text = dt.Rows[0]["col3"].ToString().Trim();
                        txtent_by.Text = dt.Rows[0]["ent_by"].ToString().Trim(); txtent_dt.Text = dt.Rows[0]["ent_dt"].ToString().Trim();
                        ddntrofcmlnt.Text = dt.Rows[0]["col2"].ToString().Trim(); dddivisioncmltn.Text = dt.Rows[0]["col4"].ToString().Trim();
                        txtinvbtch.Text = dt.Rows[0]["col8"].ToString().Trim();
                        txtPaddr.Text = dt.Rows[0]["address"].ToString().Trim();
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
                            sg1_dr["sg1_f1"] = dt.Rows[i]["app"].ToString().Trim();
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_f6"] = "-";
                            sg1_dr["sg1_t1"] = dt.Rows[i]["rmk"].ToString().Trim();
                            sg1_dr["sg1_t2"] = "-";
                            sg1_dr["sg1_t3"] = "-";
                            sg1_dr["sg1_t4"] = "-";
                            sg1_dr["sg1_t5"] = "-";
                            sg1_dr["sg1_t6"] = "-";
                            sg1_dr["sg1_t7"] = "-";
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose(); setColHeadings();
                        fgen.EnableForm(this.Controls); disablectrl();
                        txtvchnum.Text = fgen.next_no(frm_qstr, frm_cocd, "select MAX(" + doc_nf.Value + ") as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    }
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
                        //txtlbl4.Text = dt.Rows[i]["frm_name"].ToString().Trim();
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
                    dt = new DataTable();                    
                    SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, a.vchnum as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.col5 AS pono ,a.col6 AS podate ,b.aname ,a.acode ,c.iname ,a.icode ,a.srno,a.COL1 as app,a.COL2,a.COL3,a.COL4,a.REMARKS as rmk,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.col13,a.col24,a.col25,a.col22,a.col23,a.col15,a.col16,a.col17,a.col18,a.col19,a.col27,a.col28,a.col20,a.col8,b.addr1||','||b.addr2||','||b.addr3 as address from " + frm_tabname + " a ,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + frm_mbr + frm_vty + col1 + "' order by a.srno";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                        col1 = txtvchnum.Text + txtvchdate.Text;
                        ViewState["fstr"] = col1;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);

                        txtcvchnum.Text = dt.Rows[0]["col22"].ToString().Trim();
                        txtcvchdate.Text = dt.Rows[0]["col23"].ToString().Trim();
                        txtinvno.Text = dt.Rows[0]["pono"].ToString().Trim();
                        txtinvdate.Text = dt.Rows[0]["podate"].ToString().Trim();
                        txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                        txtsacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtsaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                        txticode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtsicode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtsiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["col13"].ToString().Trim();
                        txtntrcmpln.Text = dt.Rows[0]["col3"].ToString().Trim();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString().Trim();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString().Trim();
                        txtent_by.Text = dt.Rows[0]["col24"].ToString().Trim();
                        txtent_dt.Text = dt.Rows[0]["col25"].ToString().Trim();
                        ddntrofcmlnt.Text = dt.Rows[0]["col2"].ToString().Trim();
                        dddivisioncmltn.Text = dt.Rows[0]["col4"].ToString().Trim();
                        txtrply.Text = dt.Rows[0]["col15"].ToString().Trim();
                        txtcorrective.Text = dt.Rows[0]["col16"].ToString().Trim();
                        txtpreventive.Text = dt.Rows[0]["col17"].ToString().Trim();
                        txtfact.Text = dt.Rows[0]["col18"].ToString().Trim();
                        dd1.SelectedItem.Text = dt.Rows[0]["col19"].ToString().Trim();
                        txtinvbtch.Text = dt.Rows[0]["col8"].ToString().Trim();
                        txtInform.Text = dt.Rows[0]["col20"].ToString().Trim();
                        txtCost.Text = dt.Rows[0]["col27"].ToString().Trim();
                        txtPerson.Text = dt.Rows[0]["col28"].ToString().Trim();
                        txtPaddr.Text = dt.Rows[0]["address"].ToString().Trim();
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
                            sg1_dr["sg1_f1"] = dt.Rows[i]["app"].ToString().Trim();
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_f6"] = "-";
                            sg1_dr["sg1_t1"] = dt.Rows[i]["rmk"].ToString().Trim();
                            sg1_dr["sg1_t2"] = "-";
                            sg1_dr["sg1_t3"] = "-";
                            sg1_dr["sg1_t4"] = "-";
                            sg1_dr["sg1_t5"] = "-";
                            sg1_dr["sg1_t6"] = "-";
                            sg1_dr["sg1_t7"] = "-";
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
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    string frm_rptName = "";
                   // SQuery = "select d.*,a.col15 as linedetails,A.col16 as actiontaken,a.col17 AS correctiveaction,a.col18 as factfinding,a.col19 as tktstatus,a.ent_by as screntby,a.ent_Dt as screntdt,b.aname,b.addr1 as paddr1,b.addr2 as paddr2,c.iname,c.cpartno from scratch d,famst b,item c,scratch2 a where A.branchcd||trim(A.col22)||trim(A.col23)||trim(A.acode)||trim(a.icode)||a.srno=trim(D.branchcd)||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')||trim(D.acode)||trim(d.icode)||d.srno and trim(a.acode)=trim(b.acodE) and trim(d.acode)=trim(b.acode) and trim(a.icode)=trim(c.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' and d.type='CC' order by a.srno";
                    SQuery = "select d.*,a.vchnum as act_vchnum,to_char(a.vchdate,'dd/mm/yyyy') as act_vchdt,a.col27 as cost,a.col28 as ourperson,a.col15 as linedetails,A.col16 as actiontaken,a.col17 AS correctiveaction,a.col18 as factfinding,a.col19 as tktstatus,a.ent_by as screntby,a.ent_Dt as screntdt,b.aname,b.addr1 as paddr1,b.addr2 as paddr2,c.iname,c.cpartno from scratch d,famst b,item c,scratch2 a where A.branchcd||trim(A.col22)||trim(A.col23)||trim(A.acode)||trim(a.icode)||a.srno=trim(D.branchcd)||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')||trim(D.acode)||trim(d.icode)||d.srno and trim(a.acode)=trim(b.acodE) and trim(d.acode)=trim(b.acode) and trim(a.icode)=trim(c.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' and d.type='CC' order by a.srno";
                    if (frm_cocd == "SRIS")
                    {
                        frm_rptName = "action_tkn_sris";
                    }
                    else
                    {
                        frm_rptName = "action_tkn";
                    }
                    fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "action_tkn", frm_rptName);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
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

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    // SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,b.weight from ivoucher a,item i,somas b where trim(a.icode)=trim(i.icode) and upper(trim(a.finvno))=trim(b.type)||'/'||trim(b.ordno)||' DT.'||to_char(b.orddt,'dd/mm/yyyy') and trim(a.icode)=trim(b.icode) and trim(a.invno)=trim(b.org_invno) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in Hidden Field 
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[d]["ICODE"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = dt.Rows[d]["WEIGHT"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = dt.Rows[d]["TAG_NO"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = dt.Rows[d]["INAME"].ToString().Trim();
                    }
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        //  SQuery = "Select a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname from ivoucherp a,item i where trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='15' and trim(a.icode)||trim(a.invno)||trim(a.desc_) ='" + col1.Trim() + "'  order by Tag_no";
                        //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,b.weight from ivoucher a,item i,somas b where trim(a.icode)=trim(i.icode) and upper(trim(a.finvno))=trim(b.type)||'/'||trim(b.ordno)||' DT.'||to_char(b.orddt,'dd/mm/yyyy') and trim(a.icode)=trim(b.icode) and trim(a.invno)=trim(b.org_invno) and a.branchcd='" + frm_mbr + "' and a.type='15' and upper(a.finvno)='" + txtOrder.Text.Trim().Substring(2, 2) + "/" + txtOrder.Text.Trim().Substring(4, 6) + " DT." + txtOrder.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||REPLACE(REPLACE(A.DESC_, CHR(13),''), CHR(10),'') ='" + col1.Trim() + "' order by Tag_no";
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
                            sg1_dr["sg1_f1"] = "-";
                            sg1_dr["sg1_f2"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_f6"] = "-";
                            sg1_dr["sg1_t1"] = dt.Rows[d]["weight"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[d]["tag_no"].ToString().Trim();
                            sg1_dr["sg1_t3"] = "-";
                            sg1_dr["sg1_t4"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_t5"] = "";
                            sg1_dr["sg1_t6"] = "";
                            sg1_dr["sg1_t7"] = "";
                            sg1_dr["sg1_t8"] = "";
                            sg1_dt.Rows.Add(sg1_dr);
                        }
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
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.Trim();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim();
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
            SQuery = "sELECT distinct trim(a.Vchnum) as Sheet_No,to_char(a.Vchdate,'dd/mm/yyyy') as Sheet_Dt,a.Job_no,a.Job_Dt,a.icode,i.iname,i.unit,a.a1 as qty_issue,a.a2 as no_of_cuts,a.a3 as cut_sheet,a.a4 as rej_sheet,a.mchcode as machine_code,a.mcstart as start_time,mcstop as stop_time,a.num1 as ok_sheet,a.num2 as total_cutsheet,a.Ent_by,a.Ent_Dt,to_char(a.vchdate,'yyyymmdd') as vdd FROM " + frm_tabname + " a,item i WHERE trim(a.icode)=trim(i.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' AND a.VCHDATE  " + PrdRange + " ORDER BY vdd DESC,Sheet_No DESC";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
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
                                save_it = "Y";
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
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
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
            sg1_dr["sg1_t1"] = "-";
            sg1_dr["sg1_t2"] = "-";
            sg1_dr["sg1_t3"] = "-";
            sg1_dr["sg1_t4"] = "-";
            sg1_dr["sg1_t5"] = "-";
            sg1_dr["sg1_t6"] = "-";
            sg1_dr["sg1_t7"] = "-";
            sg1_dr["sg1_t8"] = "-";
            sg1_dt.Rows.Add(sg1_dr);
        }
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
            sg1.Columns[10].Visible = false;
            sg1.Columns[11].HeaderStyle.Width = 30;
            sg1.Columns[11].Visible = false;
            sg1.Columns[12].HeaderStyle.Width = 50;
            sg1.Columns[13].HeaderStyle.Width = 400;
            sg1.Columns[15].Visible = false;
            sg1.Columns[16].Visible = false;
            sg1.Columns[17].Visible = false;
            sg1.Columns[18].Visible = false;
            sg1.Columns[20].Visible = false;
            sg1.Columns[21].Visible = false;
            sg1.Columns[22].Visible = false;
            sg1.Columns[23].Visible = false;
            sg1.Columns[24].Visible = false;
            sg1.Columns[25].Visible = false;
            sg1.Columns[26].Visible = false;
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
                    fgen.Fn_open_sseek("Select Tag", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Tag", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-"); hffield.Value = "WO";
        make_qry_4_popup();
        // fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);
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
        hffield.Value = "";
        make_qry_4_popup();
        // fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
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
            if (txtrply.Text.Trim().Length > 500)
            {
                oporow["col15"] = txtrply.Text.Trim().Substring(0, 499).ToUpper();
            }
            else
            {
                oporow["col15"] = txtrply.Text.Trim().ToUpper();
            }
            if (txtcorrective.Text.Trim().Length > 500)
            {
                oporow["col16"] = txtcorrective.Text.Trim().Substring(0, 499).ToUpper();
            }
            else
            {
                oporow["col16"] = txtcorrective.Text.Trim().ToUpper();
            }
            if (txtpreventive.Text.Trim().Length > 500)
            {
                oporow["col17"] = txtpreventive.Text.Trim().Substring(0, 499).ToUpper();
            }
            else
            {
                oporow["col17"] = txtpreventive.Text.Trim().ToUpper();
            }
            oporow["col18"] = txtfact.Text.Trim().ToUpper();
            if (txtfact.Text.Trim().Length > 500)
            {
                oporow["col18"] = txtfact.Text.Trim().Substring(0, 499).ToUpper();
            }
            else
            {
                oporow["col18"] = txtfact.Text.Trim().ToUpper();
            }
            oporow["col19"] = dd1.SelectedItem.Text.Trim().ToString();
            oporow["col8"] = txtinvbtch.Text.Trim().ToString().Trim();
            //******************************************
            oporow["col5"] = txtinvno.Text.Trim();
            oporow["col6"] = txtinvdate.Text.Trim();
            oporow["col22"] = txtcvchnum.Text.Trim();
            oporow["col23"] = txtcvchdate.Text.Trim();
            oporow["col24"] = txtent_by.Text.Trim();
            oporow["col25"] = txtent_dt.Text.Trim();
            oporow["acode"] = txtacode.Text.Trim();
            oporow["icode"] = txticode.Text.Trim();
            oporow["srno"] = sg1.Rows[i].RowIndex + 1;
            oporow["COL1"] = sg1.Rows[i].Cells[13].Text.Trim();
            oporow["COL2"] = ddntrofcmlnt.Text.ToUpper();
            oporow["COL3"] = txtntrcmpln.Text.Trim().ToUpper();
            oporow["COL4"] = dddivisioncmltn.Text.ToUpper();
            oporow["remarks"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
            oporow["col13"] = txtrmk.Text.Trim().ToUpper();

            if (txtInform.Text.Trim().Length > 170)
            {
                oporow["col20"] = txtInform.Text.Trim().Substring(0, 170).ToUpper();
            }
            else
            {
                oporow["col20"] = txtInform.Text.Trim().ToUpper();
            }

            oporow["col27"] = fgen.make_double(txtCost.Text.Trim());
            oporow["col28"] = txtPerson.Text.Trim();

            if (frm_vnum != "000000") // IF ANY ERROR OCCURS THEN IT SHOULD NOT UPDATE CHK_BY
            {
                if (frm_cocd == "SEL")
                {
                    if (dd1.SelectedItem.Text.Trim().ToString() == "Closed")
                        fgen.execute_cmd(frm_qstr, frm_cocd, "Update scratch set chk_by='" + frm_uname + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + "CC" + txtcvchnum.Text.Trim() + txtcvchdate.Text.Trim() + "'");
                }
                else
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "Update scratch set chk_by='" + frm_uname + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + "CC" + txtcvchnum.Text.Trim() + txtcvchdate.Text.Trim() + "'");
                }
            }

            if (edmode.Value == "Y")
            {
                oporow["eNt_by"] = ViewState["entby"].ToString();
                oporow["eNt_dt"] = ViewState["entdt"];
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
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "AC");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------
}