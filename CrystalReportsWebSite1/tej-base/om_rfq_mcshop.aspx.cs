using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.IO;

public partial class om_rfq_mcshop : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", xStartDt = "", Enable = "";
    DataTable dt, dt1, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8;
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
            btnprint.Visible = false;
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
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnprint.Disabled = false;
        create_tab(); btnrfq.Enabled = false; btnChild.Enabled = false;
        sg1_add_blankrows();
        btnlbl4.Enabled = false;
        btnlbl7.Enabled = false;
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        create_tab2();
        sg2_add_blankrows();
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; btnprint.Disabled = true;
        btnlbl4.Enabled = true; btnrfq.Enabled = true;
        btnlbl7.Enabled = true; btnChild.Enabled = true;
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
        doc_nf.Value = "ordno";
        doc_df.Value = "orddt";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "WB_SORFQ";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MC");
        lblheader.Text = "RFQ Respond M/c Shop";
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

            case "item":
                SQuery = "select distinct trim(a.icode) as fstr,trim(a.icode) as item_code,trim(b.iname) as item_name ,b.unit,trim(b.cpartno) as part_no from itwstage a, item b where trim(a.icode)=trim(b.icode) and length(trim(a.icode))>4 and substr((trim(a.icode),1,2)>='7' order by trim(a.icode)";
                break;

            case "TACODE":
                //SQuery = "SELECT trim(a.fstr) as fstr,trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,A.TYPE,TRIM(a.icode) AS CODE,TRIM(i.iname) AS ITEM_NAME,to_char(a.orddt,'yyyymmdd') as vdd  from  (select branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,'ENQUIRY REGISTER' AS TYPE,1 AS QTY from wb_porfq where branchcd='" + frm_mbr + "' and type ='ER' union all select distinct branchcd||'ER'||trim(INVNO)||to_char(INVDATE,'dd/mm/yyyy') as fstr,INVNO,INVDATE,icode,'ENQUIRY REGISTER' AS TYPE,-1 AS QTY from wb_porfq where branchcd='" + frm_mbr + "' and type='MC' union all select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,'ENG. CHANGE NOTIFICATION' AS TYPE,1 AS QTY from wb_porfq where branchcd='" + frm_mbr + "' and type ='EC' union all select distinct branchcd||'EC'||trim(INVNO)||to_char(INVDATE,'dd/mm/yyyy') as fstr,INVNO,INVDATE,icode,'ENG. CHANGE NOTIFICATION' AS  TYPE,-1 AS QTY from wb_porfq where branchcd='" + frm_mbr + "' and type='MC')a,item i where trim(a.icode)=trim(i.icode) GROUP BY trim(a.fstr),trim(a.ordno),to_char(a.orddt,'dd/mm/yyyy'),TRIM(a.icode),TRIM(i.iname),to_char(a.orddt,'yyyymmdd'),A.TYPE HAVING SUM(QTY)>0 ORDER BY FSTR";
                SQuery = "SELECT trim(a.fstr) as fstr,trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,TRIM(a.icode) AS CODE,TRIM(i.iname) AS ITEM_NAME,to_char(a.orddt,'yyyymmdd') as vdd  from (select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type ='RF' and nvl(trim(app_by),'-')!='C' union all select distinct branchcd||'RF'||trim(INVNO)||to_char(INVDATE,'dd/mm/yyyy') as fstr,INVNO,INVDATE,icode,-1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='MC' )a,item i where trim(a.icode)=trim(i.icode) GROUP BY trim(a.fstr),trim(a.ordno),to_char(a.orddt,'dd/mm/yyyy'),TRIM(a.icode),TRIM(i.iname),to_char(a.orddt,'yyyymmdd') HAVING SUM(QTY)>0 ORDER BY FSTR";
                SQuery = "SELECT trim(a.fstr) as fstr,trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,A.TYPE,TRIM(a.icode) AS CODE,TRIM(i.iname) AS ITEM_NAME,to_char(a.orddt,'yyyymmdd') as vdd  from  (select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,'ENQUIRY REGISTER' AS TYPE,1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type ='ER' and nvl(trim(app_by),'-')!='C' union all select trim(pordno) as fstr,INVNO,INVDATE,icode,'ENQUIRY REGISTER' AS TYPE,-1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='MC' union all select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,'ENG. CHANGE NOTIFICATION' AS TYPE,1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type ='EC' and nvl(trim(app_by),'-')!='C' union all select distinct trim(pordno) as fstr,INVNO,INVDATE,icode,'ENG. CHANGE NOTIFICATION' AS  TYPE,-1 AS QTY from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='MC' )a,item i where trim(a.icode)=trim(i.icode) GROUP BY trim(a.fstr),trim(a.ordno),to_char(a.orddt,'dd/mm/yyyy'),TRIM(a.icode),TRIM(i.iname),to_char(a.orddt,'yyyymmdd'),A.TYPE HAVING SUM(QTY)>0 ORDER BY VDD,RFQ_NO";
                // ONLY ENTERIES HAVING SRNO IS 1 ARE PICKED BECAUSE IN TYPE MC, DATA IS SAVED ON THE BASIS OF GRID 1 AND IF SRNO CONDITION NOT APPLIED IT GIVES WRONG DATA.
                SQuery = "SELECT trim(a.fstr) as fstr,trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,A.TYPE,TRIM(a.icode) AS CODE,TRIM(i.iname) AS ITEM_NAME,to_char(a.orddt,'yyyymmdd') as vdd from (select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,'ENQUIRY REGISTER' AS TYPE,1 AS QTY,PEXC,0 AS PDISC1 from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type ='ER' and nvl(trim(app_by),'-')!='C' union all select trim(pbasis) as fstr,INVNO,INVDATE,icode,'ENQUIRY REGISTER' AS TYPE,-1 AS QTY,0 AS PEXC,PDISC AS PDISC1 from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='MC' AND SUBSTR(TRIM(PBASIS),3,2)='ER' AND SRNO='1' union all select DISTINCT branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,'ENG. CHANGE NOTIFICATION' AS TYPE,1 AS QTY,PEXC,0 AS PDISC1 from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type ='EC' and nvl(trim(app_by),'-')!='C' union all select trim(pbasis) as fstr,INVNO,INVDATE,icode,'ENG. CHANGE NOTIFICATION' AS TYPE,-1 AS QTY,0 AS PEXC,PDISC AS PDISC1 from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='MC' AND SUBSTR(TRIM(PBASIS),3,2)='EC' AND SRNO='1')a,item i where trim(a.icode)=trim(i.icode) GROUP BY trim(a.fstr),trim(a.ordno),to_char(a.orddt,'dd/mm/yyyy'),TRIM(a.icode),TRIM(i.iname),to_char(a.orddt,'yyyymmdd'),A.TYPE  HAVING ((SUM(PEXC)-SUM(PDISC1))>0 OR SUM(QTY)>0)ORDER BY VDD,RFQ_NO";
                break;

            case "TICODE":
                SQuery = "select trim(a.icode)||trim(a.ibcode) as fstr, trim(a.icode) as parent_code,trim(a.ibcode) as child_code,i.iname as child_name from itemosp a,item i where trim(a.ibcode)=trim(i.icode) and a. icode like '9%' and a.icode='" + txtIcode.Text.Trim() + "' order by a.srno";
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
                //SQuery = "Select icode as fstr, Ciname,Cpartno,icode,cdrgno from somas where branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + txtgrade.Text.Trim() + "' order by Srno";
                SQuery = "";

                break;

            case "SG1_ROW_ADD1":
            case "SG1_ROW_ADD_E1":
                string stage = "0";
                stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text;
                SQuery = "";
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
                    SQuery = "SELECT distinct trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr,a.ordno as entry_no,to_char(a.orddt,'dd/mm/yyyy') as entry_dt,a.INVNO as RFQ_no,to_char(a.INVdate,'dd/mm/yyyy') as RFQ_date,a.amd_no as child_code,trim(a.icode) as item_code,trim(b.iname) as component_name,b.cpartno as component_part,to_char(a.orddt,'yyyymmdd') as vdd FROM " + frm_tabname + " A, item B WHERE TRIM(A.iCODE)=TRIM(B.iCODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' ORDER BY VDD DESC,TRIM(a.ORDNO) DESC";
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
            frm_vty = "MC";
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
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnrfq.Focus();
        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        create_tab2();
        sg2_add_blankrows();
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        setColHeadings();
        ViewState["sg2"] = sg2_dt;
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
        cal();
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus();
            return;
        }
        if (txtRfqNo.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Select RFQ Details");
            btnrfq.Focus(); return;
        }
        if (txtChildCode.Text.Trim().Length <= 1)
        {
            mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select count(trim(ibcode)) as totchild from itemosp where trim(icode)='" + txtIcode.Text + "'", "totchild");
            if (mq0.Trim() != "0")
            {
                fgen.msg("-", "AMSG", "This Item Code Has Child Parts.'13' Please Select Its Child Parts"); txtChildCode.Focus(); return;
            }
        }
        if (sg1.Rows.Count <= 1)
        {
            fgen.msg("-", "AMSG", "Please Select Item Stages");
            return;
        }
        if (sg3.Rows.Count < 1)
        {
            fgen.msg("-", "AMSG", "Please Select Atleast One Attachment");
            return;
        }
        for (int i = 0; i < sg3.Rows.Count; i++)
        {
            if (((DropDownList)sg3.Rows[i].FindControl("sg3_t2")).SelectedItem.Text.Trim() == "PLEASE SELECT")
            {
                fgen.msg("-", "AMSG", "Please Select Either Yes / No / Conditionally_Approve For " + ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim() + "'13'(Tab 3)");
                return;
            }
            if (((DropDownList)sg3.Rows[i].FindControl("sg3_t2")).SelectedItem.Text.Trim() == "YES")
            {
                if (sg3.Rows[i].Cells[5].Text.Trim().Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Add Attachment For " + ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim() + "'13'(Tab 3)");
                    return;
                }
            }
            if (((DropDownList)sg3.Rows[i].FindControl("sg3_t2")).SelectedItem.Text.Trim() == "NO")
            {
                if (sg3.Rows[i].Cells[5].Text.Trim().Length > 1)
                {
                    fgen.msg("-", "AMSG", "For " + ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim() + " ,Attchment Is Added.'13' But 'No' Is Selected (Tab 3)");
                    return;
                }
            }
            if (((DropDownList)sg3.Rows[i].FindControl("sg3_t2")).SelectedItem.Text.Trim() == "CONDITIONALLY_APPROVE")
            {
                if (((TextBox)sg3.Rows[i].FindControl("sg3_t5")).Text.Trim().Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Fill Remarks For " + ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim() + "'13'(Tab 3)");
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
        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        ViewState["sg1"] = null;
        create_tab2();
        sg2_add_blankrows();
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        ViewState["sg2"] = null;
        sg3.DataSource = null;
        sg3.DataBind();
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
                //mq1 = "select trim(pordno) as pordno from WB_CACOST where branchcd='" + frm_mbr + "' and type='CA01' and trim(pordno)='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                mq1 = "select trim(pbasis) as pbasis from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                mq2 = fgen.seek_iname(frm_qstr, frm_cocd, mq1, "pbasis");
                mq3 = "select nvl(trim(test),'-') as test from " + frm_tabname + " where branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + mq2 + "'";
                mq4 = fgen.seek_iname(frm_qstr, frm_cocd, mq3, "test");
                if (mq4 != "Q")
                {
                    // FOR DELETING TEST FLAG FIELD FROM LAST TABLE I.E TYPE RF
                    //mq4 = "select trim(a.pordno) as pordno,trim(a.pbasis) as pbasis from " + frm_tabname + " a where a.branchcd||trim(a.type)||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                    //mq5 = fgen.seek_iname(frm_qstr, frm_cocd, mq4, "pordno");
                    //mq6 = "update wb_sorfq set test='-' where branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + mq5 + "'"; ;
                    //fgen.execute_cmd(frm_qstr, frm_cocd, mq6);

                    // FOR UPDATING TEST FLAG =R IN FIRST TABLE I.E TYPE EC OR ER AS MACHINE SHOP ENTRY IS DELETING
                    //mq7 = fgen.seek_iname(frm_qstr, frm_cocd, mq4, "pbasis");
                    mq7 = mq2;
                    mq8 = "update wb_sorfq set PR_NO='-' where branchcd||trim(type)||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + mq7 + "'";
                    fgen.execute_cmd(frm_qstr, frm_cocd, mq8);

                    // Deleing data from Main Table
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "M1" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "M2" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                    // Deleing data from WSr Ctrl Table
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                    // Saving Deleting History
                    fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(6, 10), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                    fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6) + "");
                    clearctrl(); fgen.ResetForm(this.Controls);
                }
                else
                {
                    fgen.msg("-", "AMSG", "Quotation Entry Is Done.'13' Entry Cannot Be Deleted.");
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
                btnrfq.Focus();
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
                    SQuery = "Select a.* ,trim(c.iname) as iname,c.cpartno ,c.cdrgno,t.name from " + frm_tabname + " a ,item c,type t where trim(a.icode)=trim(c.icode) and trim(a.cscode1)=trim(t.type1) and t.id='[' and a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    mq0 = "Select a.* from " + frm_tabname + " a where a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')='" + frm_mbr + "M1" + col1 + "' ORDER BY A.SRNO";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0); // FOR 2 GRID
                    //mq1 = "Select a.* from " + frm_tabname + " a where a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')='" + frm_mbr + "M2" + col1 + "' ORDER BY A.SRNO";
                    //dt3 = new DataTable();
                    //dt3 = fgen.getdata(frm_qstr, frm_cocd, mq1); // FOR ATTACHMENTS
                    if (dt.Rows.Count > 0)
                    {
                        //txtRfqNo.Text = dt.Rows[0]["INVNO"].ToString().Trim();
                        //txtRfqDate.Text = Convert.ToDateTime(dt.Rows[0]["INVDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                        //txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                        //txtIcode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        //txtIname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        //txtCpart.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                        //txtDrg.Text = dt.Rows[0]["cdrgno"].ToString().Trim();
                        txttotcost1.Text = dt.Rows[0]["OTCOST2"].ToString().Trim();
                        txteffperc.Text = dt.Rows[0]["RATE_OK"].ToString().Trim();
                        txteff.Text = dt.Rows[0]["RATE_CD"].ToString().Trim();
                        txtrejperc.Text = dt.Rows[0]["RATE_REJ"].ToString().Trim();
                        txtrej.Text = dt.Rows[0]["IRATE"].ToString().Trim();
                        txtoverperc.Text = dt.Rows[0]["OTCOST1"].ToString().Trim();
                        txtover.Text = dt.Rows[0]["O_QTY"].ToString().Trim();
                        txtprofperc.Text = dt.Rows[0]["WK1"].ToString().Trim();
                        txtprf.Text = dt.Rows[0]["WK2"].ToString().Trim();
                        txticcperc.Text = dt.Rows[0]["WK3"].ToString().Trim();
                        txticc.Text = dt.Rows[0]["WK4"].ToString().Trim();
                        txttotcost2.Text = dt.Rows[0]["PDISCAMT2"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["remark"].ToString().Trim();
                        txttoolcost.Text = dt.Rows[0]["QTYORD"].ToString().Trim();
                        //txtFstr.Text = dt.Rows[0]["PORDNO"].ToString().Trim();
                        //txtFstr2.Text = dt.Rows[0]["PBASIS"].ToString().Trim();
                        txtTest.Text = dt.Rows[0]["TEST"].ToString().Trim();
                        txtM_C_WT.Text = dt.Rows[0]["VEND_WT"].ToString().Trim();
                        //create_tab();
                        //sg1_dr = null;
                        //for (i = 0; i < dt.Rows.Count; i++)
                        //{
                        //    sg1_dr = sg1_dt.NewRow();
                        //    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        //    sg1_dr["sg1_h1"] = "-";
                        //    sg1_dr["sg1_h2"] = "-";
                        //    sg1_dr["sg1_h3"] = "-";
                        //    sg1_dr["sg1_h4"] = "-";
                        //    sg1_dr["sg1_h5"] = "-";
                        //    sg1_dr["sg1_h6"] = "-";
                        //    sg1_dr["sg1_h7"] = "-";
                        //    sg1_dr["sg1_h8"] = "-";
                        //    sg1_dr["sg1_h9"] = "-";
                        //    sg1_dr["sg1_h10"] = "-";
                        //    sg1_dr["sg1_f1"] = dt.Rows[i]["cscode1"].ToString().Trim();
                        //    sg1_dr["sg1_f2"] = dt.Rows[i]["CINAME"].ToString().Trim();
                        //    sg1_dr["sg1_f3"] = dt.Rows[i]["name"].ToString().Trim();
                        //    sg1_dr["sg1_t1"] = dt.Rows[i]["OTHAMT1"].ToString().Trim();
                        //    sg1_dr["sg1_t2"] = dt.Rows[i]["OTHAMT2"].ToString().Trim();
                        //    sg1_dr["sg1_t3"] = dt.Rows[i]["OTHAMT3"].ToString().Trim();
                        //    sg1_dr["sg1_t4"] = "-";
                        //    sg1_dr["sg1_t5"] = "-";
                        //    sg1_dr["sg1_t6"] = "-";
                        //    sg1_dr["sg1_t7"] = "-";
                        //    sg1_dt.Rows.Add(sg1_dr);
                        //}

                        //sg1_add_blankrows();
                        //ViewState["sg1"] = sg1_dt;
                        //sg1.DataSource = sg1_dt;
                        //sg1.DataBind();

                        create_tab2();
                        sg2_dr = null;
                        for (i = 0; i < dt2.Rows.Count; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_t1"] = dt2.Rows[i]["PREFSOURCE"].ToString().Trim();
                            sg2_dr["sg2_t2"] = dt2.Rows[i]["RATE_DIFF"].ToString().Trim();
                            sg2_dr["sg2_t3"] = dt2.Rows[i]["SPLRMK"].ToString().Trim();
                            sg2_dr["sg2_t4"] = dt2.Rows[i]["PDISCAMT2"].ToString().Trim();
                            sg2_dr["sg2_t5"] = dt2.Rows[i]["TXB_FRT"].ToString().Trim();
                            sg2_dr["sg2_t6"] = dt2.Rows[i]["CINAME"].ToString().Trim();
                            sg2_dr["sg2_t7"] = dt2.Rows[i]["DOC_THR"].ToString().Trim();
                            sg2_dr["sg2_t8"] = dt2.Rows[i]["OTHAMT1"].ToString().Trim();
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        if (dt2.Rows.Count <= 1)
                        {
                            sg2_add_blankrows();
                        }
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();

                        //create_tab3();
                        //sg3_dr = null;
                        //for (i = 0; i < dt3.Rows.Count; i++)
                        //{
                        //    sg3_dr = sg3_dt.NewRow();
                        //    sg3_dr["sg3_t1"] = dt3.Rows[i]["kindattn"].ToString().Trim();
                        //    sg3_dr["sg3_t2"] = dt3.Rows[i]["st31no"].ToString().Trim();
                        //    sg3_dr["sg3_t3"] = dt3.Rows[i]["atch2"].ToString().Trim();
                        //    sg3_dr["sg3_t4"] = dt3.Rows[i]["atch3"].ToString().Trim();
                        //    sg3_dr["sg3_t5"] = dt3.Rows[i]["desc_"].ToString().Trim();
                        //    sg3_dt.Rows.Add(sg3_dr);
                        //}
                        //sg3.DataSource = sg3_dt;
                        //sg3.DataBind();
                        //ViewState["sg3"] = sg3_dt;
                        //fgen.EnableForm(this.Controls);
                        //for (int i = 0; i < sg3.Rows.Count; i++)
                        //{
                        //    string hf = ((HiddenField)sg3.Rows[i].FindControl("cmd1")).Value;
                        //    if (hf != "" && hf != "-")
                        //    {
                        //        ((DropDownList)sg3.Rows[i].FindControl("sg3_t2")).Items.FindByText(hf).Selected = true;
                        //    }
                        ////    if (i <= 2) // FOR STOPPING IT FROM DISABLING LAST ROW
                        ////    {
                        ////        sg3.Rows[i].Cells[0].Enabled = false;
                        ////        ((DropDownList)sg3.Rows[i].FindControl("sg3_t2")).Enabled = false;
                        ////        ((FileUpload)sg3.Rows[i].FindControl("FileUpload1")).Enabled = false;
                        ////        ((TextBox)sg3.Rows[i].FindControl("sg3_t5")).Enabled = false;
                        ////    }
                        //}

                        dt.Dispose(); //sg1_dt.Dispose();
                        dt2.Dispose(); sg2_dt.Dispose();
                        //dt3.Dispose(); sg3_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        btnrfq.Focus();
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
                    SQuery = "Select a.* ,trim(c.iname) as iname,c.cpartno ,c.cdrgno,t.name from " + frm_tabname + " a ,item c,type t where trim(a.icode)=trim(c.icode) and trim(a.cscode1)=trim(t.type1) and t.id='[' and a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    mq0 = "Select a.* from " + frm_tabname + " a where a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')='" + frm_mbr + "M1" + col1 + "' ORDER BY A.SRNO";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0); // FOR 2 GRID
                    //mq1 = "Select a.* from " + frm_tabname + " a where a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')='" + frm_mbr + "M2" + col1 + "' ORDER BY A.SRNO";
                    //dt3 = new DataTable();
                    //dt3 = fgen.getdata(frm_qstr, frm_cocd, mq1); // FOR ATTACHMENTS
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["ordno"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["orddt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtRfqNo.Text = dt.Rows[0]["INVNO"].ToString().Trim();
                        txtRfqDate.Text = Convert.ToDateTime(dt.Rows[0]["INVDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtIcode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtIname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtCpart.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                        txtDrg.Text = dt.Rows[0]["cdrgno"].ToString().Trim();
                        txttotcost1.Text = dt.Rows[0]["OTCOST2"].ToString().Trim();
                        txteffperc.Text = dt.Rows[0]["RATE_OK"].ToString().Trim();
                        txteff.Text = dt.Rows[0]["RATE_CD"].ToString().Trim();
                        txtrejperc.Text = dt.Rows[0]["RATE_REJ"].ToString().Trim();
                        txtrej.Text = dt.Rows[0]["IRATE"].ToString().Trim();
                        txtoverperc.Text = dt.Rows[0]["OTCOST1"].ToString().Trim();
                        txtover.Text = dt.Rows[0]["O_QTY"].ToString().Trim();
                        txtprofperc.Text = dt.Rows[0]["WK1"].ToString().Trim();
                        txtprf.Text = dt.Rows[0]["WK2"].ToString().Trim();
                        txticcperc.Text = dt.Rows[0]["WK3"].ToString().Trim();
                        txticc.Text = dt.Rows[0]["WK4"].ToString().Trim();
                        txttotcost2.Text = dt.Rows[0]["PDISCAMT2"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["remark"].ToString().Trim();
                        txttoolcost.Text = dt.Rows[0]["QTYORD"].ToString().Trim();
                        txtFstr.Text = dt.Rows[0]["PORDNO"].ToString().Trim();
                        txtFstr2.Text = dt.Rows[0]["PBASIS"].ToString().Trim();
                        txtTest.Text = dt.Rows[0]["TEST"].ToString().Trim();
                        txtM_C_WT.Text = dt.Rows[0]["VEND_WT"].ToString().Trim();
                        txtParentChild.Text = dt.Rows[0]["DELV_ITEM"].ToString().Trim();
                        txtChildCode.Text = dt.Rows[0]["AMD_NO"].ToString().Trim();
                        txtChildName.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(iname) as iname from item where icode='" + txtChildCode.Text.Trim() + "'", "iname");

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
                            sg1_dr["sg1_f1"] = dt.Rows[i]["cscode1"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["CINAME"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["name"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["OTHAMT1"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["OTHAMT2"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["OTHAMT3"].ToString().Trim();
                            sg1_dr["sg1_t4"] = "-";
                            sg1_dr["sg1_t5"] = "-";
                            sg1_dr["sg1_t6"] = "-";
                            sg1_dr["sg1_t7"] = "-";
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();

                        create_tab2();
                        sg2_dr = null;
                        for (i = 0; i < dt2.Rows.Count; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_t1"] = dt2.Rows[i]["PREFSOURCE"].ToString().Trim();
                            sg2_dr["sg2_t2"] = dt2.Rows[i]["RATE_DIFF"].ToString().Trim();
                            sg2_dr["sg2_t3"] = dt2.Rows[i]["SPLRMK"].ToString().Trim();
                            sg2_dr["sg2_t4"] = dt2.Rows[i]["PDISCAMT2"].ToString().Trim();
                            sg2_dr["sg2_t5"] = dt2.Rows[i]["TXB_FRT"].ToString().Trim();
                            sg2_dr["sg2_t6"] = dt2.Rows[i]["CINAME"].ToString().Trim();
                            sg2_dr["sg2_t7"] = dt2.Rows[i]["DOC_THR"].ToString().Trim();
                            sg2_dr["sg2_t8"] = dt2.Rows[i]["OTHAMT1"].ToString().Trim();
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        if (dt2.Rows.Count <= 1)
                        {
                            sg2_add_blankrows();
                        }
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();

                        mq1 = "select a.kindattn,a.st31no,a.atch2,a.atch3,'-' as desc_ from wb_sorfq a where trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')='" + txtFstr.Text.Trim() + "' union all SELECT a.kindattn,a.st31no,a.atch2,a.atch3,a.desc_ from wb_sorfq a where trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')='" + frm_mbr + "M2" + col1 + "'";
                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, mq1);

                        create_tab3();
                        sg3_dr = null;
                        for (i = 0; i < dt3.Rows.Count; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_t1"] = dt3.Rows[i]["kindattn"].ToString().Trim();
                            sg3_dr["sg3_t2"] = dt3.Rows[i]["st31no"].ToString().Trim();
                            sg3_dr["sg3_t3"] = dt3.Rows[i]["atch2"].ToString().Trim();
                            sg3_dr["sg3_t4"] = dt3.Rows[i]["atch3"].ToString().Trim();
                            sg3_dr["sg3_t5"] = dt3.Rows[i]["desc_"].ToString().Trim();
                            sg3_dt.Rows.Add(sg3_dr);
                        }
                        sg3.DataSource = sg3_dt;
                        sg3.DataBind();
                        ViewState["sg3"] = sg3_dt;
                        fgen.EnableForm(this.Controls);
                        for (int i = 0; i < sg3.Rows.Count; i++)
                        {
                            string hf = ((HiddenField)sg3.Rows[i].FindControl("cmd1")).Value;
                            if (hf != "" && hf != "-")
                            {
                                ((DropDownList)sg3.Rows[i].FindControl("sg3_t2")).Items.FindByText(hf).Selected = true;
                            }
                            if (i <= 2) // FOR STOPPING IT FROM DISABLING LAST ROW
                            {
                                sg3.Rows[i].Cells[0].Enabled = false;
                                ((DropDownList)sg3.Rows[i].FindControl("sg3_t2")).Enabled = false;
                                ((FileUpload)sg3.Rows[i].FindControl("FileUpload1")).Enabled = false;
                                ((TextBox)sg3.Rows[i].FindControl("sg3_t5")).Enabled = false;
                            }
                        }

                        dt.Dispose(); sg1_dt.Dispose();
                        dt2.Dispose(); sg2_dt.Dispose();
                        dt3.Dispose(); sg3_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        disablectrl();
                        setColHeadings();
                        btnrfq.Enabled = false;
                        btnChild.Enabled = false;
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_qa_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    SQuery = "select trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,a.icode,i.iname,i.cpartno,i.cdrgno,a.acode,trim(a.pordno) as pordno from " + frm_tabname + " a,item i where trim(a.icode)=trim(i.icode) and a.branchcd||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')='" + col1 + "'";
                    SQuery = "select trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,a.icode,i.iname,i.cpartno,i.cdrgno,a.acode,a.kindattn,a.st31no,a.atch2,a.atch3 from " + frm_tabname + " a,item i where trim(a.icode)=trim(i.icode) and a.branchcd||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')='" + col1 + "' order by a.srno";

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtRfqNo.Text = dt.Rows[0]["rfq_no"].ToString().Trim();
                        txtRfqDate.Text = dt.Rows[0]["rfq_date"].ToString().Trim();
                        txtIcode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtIname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtCpart.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                        txtDrg.Text = dt.Rows[0]["cdrgno"].ToString().Trim();
                        txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtFstr.Text = col1.Trim();
                        //txtFstr2.Text = dt.Rows[0]["pordno"].ToString().Trim(); // BRANCHCD||TYPE||ORDNO||ORDDT OF RF
                        txtFstr2.Text = col1.Trim();

                        create_tab3();
                        sg3_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_t1"] = dt.Rows[i]["kindattn"].ToString().Trim();
                            sg3_dr["sg3_t2"] = dt.Rows[i]["st31no"].ToString().Trim();
                            sg3_dr["sg3_t3"] = dt.Rows[i]["atch2"].ToString().Trim();
                            sg3_dr["sg3_t4"] = dt.Rows[i]["atch3"].ToString().Trim();
                            sg3_dt.Rows.Add(sg3_dr);
                        }
                        for (i = 0; i < 4; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            if (i == 0)
                            {
                                sg3_dr["sg3_t1"] = "FEASIBILITY";
                            }
                            if (i == 1)
                            {
                                sg3_dr["sg3_t1"] = "CYCLE TIME BREAKUP ";
                            }
                            if (i == 2)
                            {
                                sg3_dr["sg3_t1"] = "BALLONING DRAWING ";
                            }
                            if (i == 3)
                            {
                                sg3_dr["sg3_t1"] = "CLARIFICATION SHEET ";
                            }
                            sg3_dr["sg3_t3"] = "-";
                            sg3_dr["sg3_t4"] = "-";
                            sg3_dr["sg3_t5"] = "-";
                            sg3_dt.Rows.Add(sg3_dr);
                        }
                        sg3.DataSource = sg3_dt;
                        sg3.DataBind();
                        ViewState["sg3"] = sg3_dt;
                        for (int i = 0; i < sg3.Rows.Count - 4; i++)
                        {
                            string hf = ((HiddenField)sg3.Rows[i].FindControl("cmd1")).Value;
                            if (hf != "" && hf != "-")
                            {
                                ((DropDownList)sg3.Rows[i].FindControl("sg3_t2")).Items.FindByText(hf).Selected = true;
                            }
                            sg3.Rows[i].Cells[0].Enabled = false;
                            ((DropDownList)sg3.Rows[i].FindControl("sg3_t2")).Enabled = false;
                            ((FileUpload)sg3.Rows[i].FindControl("FileUpload1")).Enabled = false;
                            ((TextBox)sg3.Rows[i].FindControl("sg3_t5")).Enabled = false;
                        }

                        #region Item Stage Mapping
                        //create_tab();
                        //dt1 = new DataTable();
                        //SQuery = "select a.stagec,a.mtime1,a.mtime,b.name,a.srno from itwstage a,type b where trim(a.stagec)=trim(b.type1) and a.branchcd='" + frm_mbr + "' and a.type='10' and b.id='[' and a.icode='" + txtIcode.Text.Trim() + "' order by a.srno";
                        //dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        //if (dt1.Rows.Count < 1)
                        //{
                        //    fgen.msg("-", "AMSG", "Stages For This Item Is Not Defined !!");
                        //    return;
                        //}
                        //for (int d = 0; d < dt1.Rows.Count; d++)
                        //{
                        //    sg1_dr = sg1_dt.NewRow();
                        //    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        //    sg1_dr["sg1_h1"] = "-";
                        //    sg1_dr["sg1_h2"] = "-";
                        //    sg1_dr["sg1_h3"] = "-";
                        //    sg1_dr["sg1_h4"] = "-";
                        //    sg1_dr["sg1_h5"] = "-";
                        //    sg1_dr["sg1_h6"] = "-";
                        //    sg1_dr["sg1_h7"] = "-";
                        //    sg1_dr["sg1_h8"] = "-";
                        //    sg1_dr["sg1_h9"] = "-";
                        //    sg1_dr["sg1_h10"] = "-";
                        //    sg1_dr["sg1_f1"] = dt1.Rows[d]["stagec"].ToString().Trim();
                        //    sg1_dr["sg1_f2"] = dt1.Rows[d]["mtime1"].ToString().Trim();
                        //    sg1_dr["sg1_f3"] = dt1.Rows[d]["name"].ToString().Trim();
                        //    sg1_dr["sg1_f4"] = "-";
                        //    sg1_dr["sg1_f5"] = "-";
                        //    sg1_dr["sg1_f6"] = "-";
                        //    sg1_dr["sg1_t1"] = dt1.Rows[d]["mtime"].ToString().Trim();
                        //    sg1_dr["sg1_t2"] = "";
                        //    sg1_dr["sg1_t3"] = "";
                        //    sg1_dr["sg1_t4"] = "-";
                        //    sg1_dr["sg1_t5"] = "-";
                        //    sg1_dr["sg1_t6"] = "-";
                        //    sg1_dr["sg1_t7"] = "-";
                        //    sg1_dr["sg1_t8"] = "-";
                        //    sg1_dt.Rows.Add(sg1_dr);
                        //}
                        //sg1_add_blankrows();
                        //ViewState["sg1"] = sg1_dt;
                        //sg1.DataSource = sg1_dt;
                        //sg1.DataBind();
                        //dt.Dispose(); sg1_dt.Dispose();
                        //((TextBox)sg1.Rows[z].FindControl("sg1_t2")).Focus();
                        //setColHeadings();
                        //btnChild.Enabled = true;
                        #endregion
                    }
                    btnChild.Focus();
                    break;

                case "TICODE":
                    if (col1.Length <= 0) return;
                    mq0 = "select trim(delv_item) as fstr,trim(amd_no) as child from wb_sorfq where branchcd='" + frm_mbr + "' and type='MC' and trim(pbasis)='" + txtFstr.Text.Trim() + "' and trim(delv_item)='" + col1 + "'";
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

                        #region Item Stage Mapping
                        create_tab();
                        dt1 = new DataTable();
                        SQuery = "select a.stagec,a.mtime1,a.mtime,b.name,a.srno from itwstage a,type b where trim(a.stagec)=trim(b.type1) and a.branchcd='" + frm_mbr + "' and a.type='10' and b.id='[' and a.icode='" + txtChildCode.Text.Trim() + "' order by a.srno";
                        dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt1.Rows.Count < 1)
                        {
                            fgen.msg("-", "AMSG", "Stages For This Item Is Not Defined !!");
                            return;
                        }
                        for (int d = 0; d < dt1.Rows.Count; d++)
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
                            sg1_dr["sg1_f1"] = dt1.Rows[d]["stagec"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt1.Rows[d]["mtime1"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt1.Rows[d]["name"].ToString().Trim();
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_f6"] = "-";
                            sg1_dr["sg1_t1"] = dt1.Rows[d]["mtime"].ToString().Trim();
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = "-";
                            sg1_dr["sg1_t5"] = "-";
                            sg1_dr["sg1_t6"] = "-";
                            sg1_dr["sg1_t7"] = "-";
                            sg1_dr["sg1_t8"] = "-";
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t2")).Focus();
                        setColHeadings();
                        #endregion
                    }
                    txtM_C_WT.Focus();
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
                    //SQuery = "Select distinct a.invno as Wo_No,a.desc_ as Tag_no,a.IQTYIN AS Qty,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,a.Ent_by,a.icode,i.iname,a.finvno,b.weight,b.cdrgno from ivoucher a,item i,somas b where trim(a.icode)=trim(i.icode) and upper(trim(a.finvno))=trim(b.type)||'/'||trim(b.ordno)||' DT.'||to_char(b.orddt,'dd/mm/yyyy') and trim(a.icode)=trim(b.icode) and trim(a.invno)=trim(b.org_invno) and a.branchcd='" + frm_mbr + "' and a.type='15' and a.invno='" + txtIcode.Text.Trim() + "' and upper(a.finvno)='" + txtgrade.Text.Trim().Substring(2, 2) + "/" + txtgrade.Text.Trim().Substring(4, 6) + " DT." + txtgrade.Text.Trim().Substring(10, 10) + "' AND trim(a.icode)||trim(a.invno)||trim(a.desc_) ='" + col1.Trim() + "' order by Tag_no";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (col1.Length <= 0) return;
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in Hidden Field 
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[8].Text = dt.Rows[d]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[9].Text = dt.Rows[d]["cdrgno"].ToString().Trim();

                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = (fgen.make_double(dt.Rows[d]["weight"].ToString().Trim())).ToString();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = dt.Rows[d]["Tag_no"].ToString().Trim();
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

                        dt = new DataTable();
                        SQuery = "select a.type,a.vchnum,a.icode,a.srno,a.stagec,a.mtime,a.opcode,b.name from itwstage a ,type b  where trim(a.stagec)=trim(b.type1) and b.id='[' and  trim(a.icode) '" + col1 + "'";
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
                            sg1_dr["sg1_h9"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_h10"] = dt.Rows[d]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f1"] = "-";
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_f6"] = "-";
                            sg1_dr["sg1_t1"] = fgen.make_double(dt.Rows[d]["weight"].ToString().Trim());
                            sg1_dr["sg1_t2"] = dt.Rows[d]["Tag_no"].ToString().Trim();
                            sg1_dr["sg1_t3"] = "";
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
                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                            sg2_dr["sg2_t3"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim();
                            sg2_dr["sg2_t4"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim();
                            sg2_dr["sg2_t5"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim();
                            sg2_dr["sg2_t6"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim();
                            sg2_dr["sg2_t7"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim();
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        sg2_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        for (i = 0; i < sg2.Rows.Count; i++)
                        {
                            sg2.Rows[i].Cells[2].Text = (i + 1).ToString();
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
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "sELECT distinct trim(a.ordno) as Entry_No,to_char(a.orddt,'dd/mm/yyyy') as entry_Dt,a.icode as item_code,i.iname as component_name,i.cpartno as Component_part_no ,i.cdrgno as component_drg_no,a.amd_no as child,b.iname as child_name,a.srno,a.cscode1 as operation_no,a.CINAME as machine,t.name as operation_desc,a.OTHAMT1 as setup_time,a.OTHAMT2 as mhr,a.OTHAMT3 as cost,a.OTCOST2 as total_cost,a.RATE_OK as efficiency_perc,a.RATE_CD as efficiency_value,a.RATE_REJ as rej_perc,a.IRATE as rej_value,a.OTCOST1 as overhead_perc,a.O_QTY as overhead_value,a.WK1 as profit_perc,a.WK2 as profit_value,a.wk3 as icc_perc ,a.wk4 as icc_value,a.PDISCAMT2 as total_cost_part ,a.remark ,a.Ent_by,a.Ent_Dt,to_char(a.orddt,'yyyymmdd') as vdd FROM item i,type t," + frm_tabname + " a left join item b on trim(a.amd_no)=trim(b.icode) where trim(a.icode)=trim(i.icode) and trim(a.cscode1)=trim(t.type1) and t.id='[' and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' AND a.orddt  " + PrdRange + " ORDER BY VDD DESC,TRIM(a.ORDNO) DESC,srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period " + fromdt + " to " + todt, frm_qstr);
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

                        oDS2 = new DataSet();
                        oporow = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                        frm_vnum = "000000";
                        save_fun2();

                        oDS3 = new DataSet();
                        oporow = null;
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                        frm_vnum = "000000";
                        save_fun3();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2.Dispose();
                        oporow = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS3.Dispose();
                        oporow = null;
                        oDS3 = new DataSet();
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

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

                        save_fun2();

                        save_fun3();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "M1" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "M2" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS3, frm_tabname);

                        string mycmd4 = ""; // SAVING FLAG IN ER ENTRY
                        mycmd4 = "update " + frm_tabname + " set PR_NO='M' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/MM/yyyy')='" + txtFstr2.Text.Trim() + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, mycmd4);

                        //string mycmd3 = "";  // SAVING FLAG IN RF ENTRY
                        //mycmd3 = "update " + frm_tabname + " set TEST='M' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/MM/yyyy')='" + txtFstr.Text.Trim() + "'";
                        //fgen.execute_cmd(frm_qstr, frm_cocd, mycmd3);

                        if (edmode.Value == "Y")
                        {
                            mq3 = "update " + frm_tabname + " set test='" + txtTest.Text.Trim() + "' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mq3);
                        }

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + "M1" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + "M2" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
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
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); sg3.DataSource = null; sg3.DataBind(); ViewState["sg3"] = null;
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

            sg1.HeaderRow.Cells[10].Style["display"] = "none";
            e.Row.Cells[10].Style["display"] = "none";

            sg1.Columns[10].HeaderStyle.Width = 30;
            sg1.Columns[11].HeaderStyle.Width = 50;
            sg1.Columns[12].HeaderStyle.Width = 200;
            sg1.Columns[13].HeaderStyle.Width = 200;
            sg1.Columns[14].HeaderStyle.Width = 200;
            sg1.Columns[15].HeaderStyle.Width = 200;
            sg1.Columns[19].HeaderStyle.Width = 200;
            sg1.Columns[20].HeaderStyle.Width = 200;
            sg1.Columns[21].HeaderStyle.Width = 200;
            sg1.Columns[22].HeaderStyle.Width = 200;
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
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Stage From The List");
                }
                break;

            case "SG1_ROW_ADD":
                if (txtIcode.Text.Trim().Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Select Item First"); btnlbl4.Focus();
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
                    fgen.Fn_open_mseek("Select Stage", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Stage", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "item";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);
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
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
    }
    //------------------------------------------------------------------------------------   
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
            {
                //save data into the wb_porfq table of type=MC
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["ordno"] = frm_vnum.Trim().ToUpper();
                oporow["orddt"] = txtvchdate.Text.Trim().ToUpper();
                oporow["SRNO"] = i + 1;
                oporow["icode"] = txtIcode.Text.Trim().ToUpper();
                oporow["acode"] = txtacode.Text.Trim().ToUpper();
                oporow["INVNO"] = txtRfqNo.Text.Trim().ToUpper();
                oporow["PORDNO"] = txtFstr.Text.Trim().ToUpper();// RF ENTRY NO. .... NOW ER OR EC NO
                oporow["PBASIS"] = txtFstr2.Text.Trim().ToUpper();// ER OR EC ENTRY NO.
                oporow["INVDATE"] = txtRfqDate.Text.Trim().ToUpper();
                oporow["CSCODE1"] = sg1.Rows[i].Cells[13].Text.Trim().ToUpper();
                oporow["CINAME"] = sg1.Rows[i].Cells[14].Text.Trim().ToUpper();
                oporow["PREFSOURCE"] = "-";
                oporow["OTHAMT1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper());
                oporow["OTHAMT2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper());
                oporow["OTHAMT3"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper());
                oporow["OTCOST2"] = fgen.make_double(txttotcost1.Text.Trim().ToUpper());
                oporow["RATE_OK"] = fgen.make_double(txteffperc.Text.Trim().ToUpper());
                oporow["RATE_CD"] = fgen.make_double(txteff.Text.Trim().ToUpper());
                oporow["RATE_REJ"] = fgen.make_double(txtrejperc.Text.Trim().ToUpper());
                oporow["IRATE"] = fgen.make_double(txtrej.Text.Trim().ToUpper());
                oporow["OTCOST1"] = fgen.make_double(txtoverperc.Text.Trim().ToUpper());
                oporow["O_QTY"] = fgen.make_double(txtover.Text.Trim().ToUpper());
                oporow["WK1"] = fgen.make_double(txtprofperc.Text.Trim().ToUpper());
                oporow["WK2"] = fgen.make_double(txtprf.Text.Trim().ToUpper());
                oporow["WK3"] = fgen.make_double(txticcperc.Text.Trim().ToUpper());
                oporow["WK4"] = fgen.make_double(txticc.Text.Trim().ToUpper());
                oporow["PDISCAMT2"] = fgen.make_double(txttotcost2.Text.Trim().ToUpper());
                oporow["remark"] = txtrmk.Text.Trim().ToUpper();
                oporow["VEND_WT"] = fgen.make_double(txtM_C_WT.Text.Trim().ToUpper());
                oporow["APP_BY"] = "-";
                oporow["APP_DT"] = vardate;
                oporow["ISSUE_NO"] = "0";
                oporow["STAX"] = "-";
                oporow["EXC"] = "-";
                oporow["IOPR"] = "-";
                oporow["PR_NO"] = "-";
                oporow["AMD_NO"] = txtChildCode.Text.Trim().ToUpper();
                oporow["DELV_ITEM"] = txtParentChild.Text.Trim().ToUpper();
                oporow["DEL_SCH"] = "-";
                oporow["TAX"] = "-";
                oporow["TERM"] = "-";
                oporow["DELV_TERM"] = "-";
                oporow["DEL_DATE"] = vardate;
                oporow["DEL_WK"] = "0";
                oporow["DEL_MTH"] = "0";
                oporow["DELIVERY"] = "0";
                oporow["PORDDT"] = vardate;
                oporow["QTYBAL"] = "0";
                oporow["QTYSUPP"] = "0";
                oporow["QTYORD"] = fgen.make_double(txttoolcost.Text.Trim().ToUpper());
                oporow["PSIZE"] = "-";
                oporow["OTCOST3"] = "0";
                oporow["PTAX"] = "0";
                oporow["PEXC"] = "0";
                if (txtParentChild.Text.Length > 1)
                {
                    oporow["PDISC"] = 1;
                }
                else
                {
                    oporow["PDISC"] = 0;
                }
                oporow["INST"] = "-";
                oporow["REFDATE"] = vardate;
                oporow["MODE_TPT"] = "-";
                oporow["TR_INSUR"] = "-";
                oporow["DESP_TO"] = "-";
                oporow["FREIGHT"] = "-";
                oporow["DOC_THR"] = "-";
                oporow["PACKING"] = "-";
                oporow["PAYMENT"] = "-";
                oporow["BANK"] = "-";
                oporow["UNIT"] = "-";
                oporow["ATCH2"] = "-";
                oporow["ATCH3"] = "-";
                oporow["KINDATTN"] = "-";
                oporow["ST31NO"] = "-";
                oporow["BILLCODE"] = "-";
                oporow["PREFSOURCE"] = "-";
                oporow["DESC_"] = "-";
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
                    oporow["edt_dt"] = vardate;
                }
                oDS.Tables[0].Rows.Add(oporow);
            }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg2.Rows.Count; i++)
        {
            if (((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim().Length > 1)
            {
                //save data into the wb_porfq table of type=MC
                oporow = oDS2.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = "M1";
                oporow["ordno"] = frm_vnum.Trim().ToUpper();
                oporow["orddt"] = txtvchdate.Text.Trim().ToUpper();
                oporow["SRNO"] = i + 1;
                oporow["icode"] = txtIcode.Text.Trim().ToUpper();
                oporow["acode"] = txtacode.Text.Trim().ToUpper();
                oporow["INVNO"] = txtRfqNo.Text.Trim().ToUpper();
                oporow["PORDNO"] = txtFstr.Text.Trim().ToUpper();// ER ENTRY NO.
                oporow["PBASIS"] = txtFstr2.Text.Trim().ToUpper();// ER ENTRY NO.
                oporow["INVDATE"] = txtRfqDate.Text.Trim().ToUpper();
                oporow["CSCODE1"] = "-";
                oporow["PREFSOURCE"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim().ToUpper();
                oporow["RATE_DIFF"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim().ToUpper();
                oporow["SPLRMK"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim().ToUpper();
                oporow["CINAME"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim().ToUpper();
                oporow["DOC_THR"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim().ToUpper();
                oporow["PDISCAMT2"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim().ToUpper());
                oporow["TXB_FRT"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim().ToUpper());
                oporow["OTHAMT1"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim().ToUpper());
                oporow["OTHAMT2"] = 0;
                oporow["OTHAMT3"] = 0;
                oporow["OTCOST2"] = 0;
                oporow["RATE_OK"] = 0;
                oporow["RATE_CD"] = 0;
                oporow["RATE_REJ"] = 0;
                oporow["IRATE"] = 0;
                oporow["OTCOST1"] = 0;
                oporow["O_QTY"] = 0;
                oporow["WK1"] = 0;
                oporow["WK2"] = 0;
                oporow["WK3"] = 0;
                oporow["WK4"] = 0;
                oporow["remark"] = "-";
                oporow["VEND_WT"] = "0";
                oporow["APP_BY"] = "-";
                oporow["APP_DT"] = vardate;
                oporow["ISSUE_NO"] = "0";
                oporow["STAX"] = "-";
                oporow["EXC"] = "-";
                oporow["IOPR"] = "-";
                oporow["PR_NO"] = "-";
                oporow["AMD_NO"] = txtChildCode.Text.Trim().ToUpper();
                oporow["DELV_ITEM"] = txtParentChild.Text.Trim().ToUpper();
                oporow["DEL_SCH"] = "-";
                oporow["TAX"] = "-";
                oporow["TERM"] = "-";
                oporow["DELV_TERM"] = "-";
                oporow["DEL_DATE"] = vardate;
                oporow["DEL_WK"] = "0";
                oporow["DEL_MTH"] = "0";
                oporow["DELIVERY"] = "0";
                oporow["PORDDT"] = vardate;
                oporow["QTYBAL"] = "0";
                oporow["QTYSUPP"] = "0";
                oporow["QTYORD"] = 0;
                oporow["PSIZE"] = "-";
                oporow["OTCOST3"] = "0";
                oporow["PTAX"] = "0";
                oporow["PEXC"] = "0";
                if (txtParentChild.Text.Length > 1)
                {
                    oporow["PDISC"] = 1;
                }
                else
                {
                    oporow["PDISC"] = 0;
                }
                oporow["INST"] = "-";
                oporow["REFDATE"] = vardate;
                oporow["MODE_TPT"] = "-";
                oporow["TR_INSUR"] = "-";
                oporow["DESP_TO"] = "-";
                oporow["FREIGHT"] = frm_mbr + frm_vty + frm_vnum.Trim().ToUpper() + txtvchdate.Text.Trim();
                oporow["PACKING"] = "-";
                oporow["PAYMENT"] = "-";
                oporow["BANK"] = "-";
                oporow["UNIT"] = "-";
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
                    oporow["edt_dt"] = vardate;
                }
                oDS2.Tables[0].Rows.Add(oporow);
            }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun3()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        z = 1;
        for (i = 3; i < sg3.Rows.Count; i++)
        {
            oporow = oDS3.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = "M2";
            oporow["ordno"] = frm_vnum.Trim().ToUpper();
            oporow["orddt"] = txtvchdate.Text.Trim().ToUpper();
            oporow["icode"] = txtIcode.Text.Trim().ToUpper();
            oporow["acode"] = txtacode.Text.Trim().ToUpper();
            oporow["INVNO"] = txtRfqNo.Text.Trim().ToUpper();
            oporow["PORDNO"] = txtFstr.Text.Trim().ToUpper();// ER ENTRY NO.
            oporow["PBASIS"] = txtFstr2.Text.Trim().ToUpper();// ER ENTRY NO.
            oporow["INVDATE"] = txtRfqDate.Text.Trim().ToUpper();
            oporow["CSCODE1"] = "-";
            oporow["CINAME"] = "-";
            oporow["PREFSOURCE"] = "-";
            oporow["RATE_DIFF"] = "-";
            oporow["SPLRMK"] = "-";
            oporow["DOC_THR"] = "-";
            oporow["PDISCAMT2"] = 0;
            oporow["TXB_FRT"] = 0;
            oporow["OTHAMT1"] = 0;
            oporow["OTHAMT2"] = 0;
            oporow["OTHAMT3"] = 0;
            oporow["OTCOST2"] = 0;
            oporow["RATE_OK"] = 0;
            oporow["RATE_CD"] = 0;
            oporow["RATE_REJ"] = 0;
            oporow["IRATE"] = 0;
            oporow["OTCOST1"] = 0;
            oporow["O_QTY"] = 0;
            oporow["WK1"] = 0;
            oporow["WK2"] = 0;
            oporow["WK3"] = 0;
            oporow["WK4"] = 0;
            oporow["remark"] = "-";
            oporow["VEND_WT"] = "0";
            oporow["APP_BY"] = "-";
            oporow["APP_DT"] = vardate;
            oporow["ISSUE_NO"] = "0";
            oporow["STAX"] = "-";
            oporow["EXC"] = "-";
            oporow["IOPR"] = "-";
            oporow["PR_NO"] = "-";
            oporow["AMD_NO"] = txtChildCode.Text.Trim().ToUpper();
            oporow["DELV_ITEM"] = txtParentChild.Text.Trim().ToUpper();
            oporow["DEL_SCH"] = "-";
            oporow["TAX"] = "-";
            oporow["TERM"] = "-";
            oporow["DELV_TERM"] = "-";
            oporow["DEL_DATE"] = vardate;
            oporow["DEL_WK"] = "0";
            oporow["DEL_MTH"] = "0";
            oporow["DELIVERY"] = "0";
            oporow["PORDDT"] = vardate;
            oporow["QTYBAL"] = "0";
            oporow["QTYSUPP"] = "0";
            oporow["QTYORD"] = 0;
            oporow["PSIZE"] = "-";
            oporow["OTCOST3"] = "0";
            oporow["PTAX"] = "0";
            oporow["PEXC"] = "0";
            if (txtParentChild.Text.Length > 1)
            {
                oporow["PDISC"] = 1;
            }
            else
            {
                oporow["PDISC"] = 0;
            }
            oporow["INST"] = "-";
            oporow["REFDATE"] = vardate;
            oporow["MODE_TPT"] = "-";
            oporow["TR_INSUR"] = "-";
            oporow["DESP_TO"] = "-";
            oporow["FREIGHT"] = frm_mbr + frm_vty + frm_vnum.Trim().ToUpper() + txtvchdate.Text.Trim();
            oporow["DOC_THR"] = "-";
            oporow["PACKING"] = "-";
            oporow["PAYMENT"] = "-";
            oporow["BANK"] = "-";
            oporow["UNIT"] = "-";
            oporow["BILLCODE"] = "-";
            oporow["DESC_"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t5")).Text.Trim().ToUpper();
            oporow["SRNO"] = z;
            oporow["ATCH2"] = sg3.Rows[i].Cells[5].Text.Trim();
            oporow["ATCH3"] = sg3.Rows[i].Cells[6].Text.Trim();
            oporow["KINDATTN"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim().ToUpper();
            oporow["ST31NO"] = ((DropDownList)sg3.Rows[i].FindControl("sg3_t2")).SelectedItem.Text.Trim().ToUpper();
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
                oporow["edt_dt"] = vardate;
            }
            z++;
            oDS3.Tables[0].Rows.Add(oporow);
        }
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MC");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------
    protected void btnicode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Item";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Item", frm_qstr);
    }
    //------------------------------------------------------------------------------------ 
    protected void btnrfq_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        txtChildCode.Text = "";
        txtChildName.Text = "";
        txtParentChild.Text = "";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select RFQ Entry", frm_qstr);
    }
    //------------------------------------------------------------------------------------ 
    public void cal()
    {
        double qty = 0, rate = 0, amt = 0, amt2 = 0, gsm5 = 0, gsm6 = 0, gsm7 = 0, gsm8 = 0, gsm9 = 0, effperc = 0, eff = 0;
        double bopqty = 0, boprate = 0, bopamt = 0, boptotamt = 0;
        for (int i = 0; i < sg1.Rows.Count - 1; i++)
        {
            qty = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
            rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text);
            amt = qty * rate;
            amt2 += qty * rate;
            ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text = Math.Round(amt, 3).ToString();
        }
        txttotcost1.Text = amt2.ToString();
        if (txteffperc.Text.Length >= 1)
        {
            //txteff.Text=txteffperc.Text
            //gsm5 = Convert.ToInt32(txteffperc.Text) / 100;
            gsm5 = Convert.ToDouble(txteffperc.Text) / 100;
            effperc = (1 + (1 - gsm5));
            eff = fgen.make_double(txttotcost1.Text) * effperc;
            txteff.Text = Math.Round(eff, 2).ToString();
        }
        if (txtrejperc.Text.Length >= 1)
        {
            gsm6 = fgen.make_double(txttotcost1.Text) * fgen.make_double(txtrejperc.Text) / 100;
            txtrej.Text = Math.Round(gsm6, 2).ToString();
        }
        if (txtoverperc.Text.Length >= 1)
        {
            gsm7 = fgen.make_double(txttotcost1.Text) * fgen.make_double(txtoverperc.Text) / 100;
            txtover.Text = Math.Round(gsm7, 2).ToString();
        }
        if (txtprofperc.Text.Length >= 1)
        {
            gsm8 = fgen.make_double(txttotcost1.Text) * fgen.make_double(txtprofperc.Text) / 100;
            txtprf.Text = Math.Round(gsm8, 2).ToString();
        }
        if (txticcperc.Text.Length >= 1)
        {
            gsm9 = fgen.make_double(txttotcost1.Text) * fgen.make_double(txticcperc.Text) / 100;
            txticc.Text = Math.Round(gsm9, 2).ToString();
        }
        for (int i = 0; i < sg2.Rows.Count; i++)
        {
            bopamt = 0;
            bopqty = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text);
            boprate = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text);
            bopamt = bopqty * boprate;
            boptotamt += bopamt;
            ((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text = Math.Round(bopamt, 2).ToString();
        }
        txttotcost2.Text = Math.Round(fgen.make_double(txteff.Text.Trim()) + fgen.make_double(txtrej.Text.Trim()) + fgen.make_double(txtover.Text.Trim()) + fgen.make_double(txtprf.Text.Trim()) + fgen.make_double(txticc.Text.Trim()) + boptotamt, 2).ToString();
    }
    //------------------------------------------------------------------------------------
    public void create_tab3()
    {
        sg3_dt = new DataTable();
        sg3_dr = null;
        // Hidden Field
        sg3_dt.Columns.Add(new DataColumn("sg3_t1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t3", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t4", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t5", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t6", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    protected void sg3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            sg3.Columns[0].HeaderStyle.Width = 50;
            sg3.Columns[1].HeaderStyle.Width = 80;
            sg3.Columns[2].HeaderStyle.Width = 50;
            sg3.Columns[3].HeaderStyle.Width = 180;
            sg3.Columns[4].HeaderStyle.Width = 180;
            sg3.Columns[5].HeaderStyle.Width = 200;
            sg3.Columns[6].HeaderStyle.Width = 200;
            sg3.Columns[7].HeaderStyle.Width = 200;
            sg3.Columns[8].HeaderStyle.Width = 170;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = 0;
        if (var == "SG3_UPLD")
        {
            rowIndex = ((GridViewRow)((Button)e.CommandSource).NamingContainer).RowIndex;
        }
        else
        {
            rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        }
        int index = Convert.ToInt32(sg3.Rows[rowIndex].RowIndex);
        string filePath = "";
        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG3_RMV":
                filePath = sg3.Rows[index].Cells[6].Text.ToUpper();
                if (filePath.Length > 1)
                {
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    string secFilePath = Server.MapPath("~/tej-base/") + sg3.Rows[index].Cells[6].Text.Substring(sg3.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"), sg3.Rows[index].Cells[6].Text.ToUpper().Length - sg3.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"));
                    if (File.Exists(secFilePath))
                    {
                        File.Delete(secFilePath);
                    }
                }
                sg3.Rows[index].Cells[5].Text = "-";
                sg3.Rows[index].Cells[6].Text = "-";
                break;

            case "SG3_DWN":
                filePath = sg3.Rows[index].Cells[6].Text.ToUpper();
                if (filePath.Length > 1)
                {
                    Response.ContentType = ContentType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
                    Response.WriteFile(filePath);
                    Response.End();
                }
                break;

            case "SG3_VIEW":
                if (sg3.Rows[index].Cells[6].Text.Trim().Length > 1)
                {
                    filePath = sg3.Rows[index].Cells[6].Text.Substring(sg3.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"), sg3.Rows[index].Cells[6].Text.ToUpper().Length - sg3.Rows[index].Cells[6].Text.ToUpper().IndexOf("UPLOAD"));
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
                }
                break;

            case "SG3_UPLD":
                string UploadedFile = ((FileUpload)sg3.Rows[index].FindControl("FileUpload1")).FileName;
                string filepath = @"c:\TEJ_ERP\UPLOAD\";
                string fileName = txtvchnum.Text.Trim() + fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY") + frm_CDT1.Replace(@"/", "_") + "~" + UploadedFile.Replace("&", "").Replace("%", "_");
                filepath = filepath + fileName;
                ((FileUpload)sg3.Rows[index].FindControl("FileUpload1")).PostedFile.SaveAs(filepath);
                ((FileUpload)sg3.Rows[index].FindControl("FileUpload1")).PostedFile.SaveAs(Server.MapPath("~/tej-base/Upload/") + fileName);
                sg3.Rows[index].Cells[5].Text = UploadedFile;
                sg3.Rows[index].Cells[6].Text = filepath;
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnUpload_Click(object sender, EventArgs e)
    {

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
        sg2_dt.Columns.Add(new DataColumn("sg2_t5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t6", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t7", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t8", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void sg2_add_blankrows()
    {
        if (sg2_dt != null)
        {
            sg2_dr = sg2_dt.NewRow();
            sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
            sg2_dr["sg2_t1"] = "-";
            sg2_dr["sg2_t2"] = "-";
            sg2_dr["sg2_t3"] = "-";
            sg2_dr["sg2_t4"] = "-";
            sg2_dr["sg2_t5"] = "-";
            sg2_dr["sg2_t6"] = "-";
            sg2_dr["sg2_t7"] = "-";
            sg2_dr["sg2_t8"] = "-";
            sg2_dt.Rows.Add(sg2_dr);
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            sg2.Columns[0].HeaderStyle.Width = 30;
            sg2.Columns[1].HeaderStyle.Width = 30;
            sg2.Columns[2].HeaderStyle.Width = 50;
            sg2.Columns[3].HeaderStyle.Width = 170;
            sg2.Columns[4].HeaderStyle.Width = 170;
            sg2.Columns[5].HeaderStyle.Width = 170;
            sg2.Columns[6].HeaderStyle.Width = 140;
            sg2.Columns[7].HeaderStyle.Width = 140;
            sg2.Columns[8].HeaderStyle.Width = 140;
            sg2.Columns[9].HeaderStyle.Width = 160;
            sg2.Columns[10].HeaderStyle.Width = 160;
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
                if (index < sg2.Rows.Count)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG2_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Line From The List");
                }
                break;

            case "SG2_ROW_ADD":
                if (index < sg2.Rows.Count - 1)
                {

                }
                else
                {
                    if (ViewState["sg2"] != null)
                    {
                        dt = (DataTable)ViewState["sg2"];
                        z = dt.Rows.Count - 1;
                        sg2_dt = dt.Clone();
                        sg1_dr = null;
                        i = 0;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = (i + 1);
                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                            sg2_dr["sg2_t3"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim();
                            sg2_dr["sg2_t4"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim();
                            sg2_dr["sg2_t5"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim();
                            sg2_dr["sg2_t6"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim();
                            sg2_dr["sg2_t7"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim();
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                    }
                    sg2_add_blankrows();
                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    ViewState["sg2"] = sg2_dt;
                }
                break;
        }
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