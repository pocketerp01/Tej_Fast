using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_task_asgn : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, cond;
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
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_DeptType, frm_DeptCode;
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
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
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

        txtlbl2.Attributes.Add("readonly", "readonly");
        txtlbl3.Attributes.Add("readonly", "readonly");

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        //my_Tabs
        //txtlbl2.Attributes["required"] = "true";
        //txtlbl2.BackColor = System.Drawing.ColorTranslator.FromHtml("#E0FF00");
        // to hide and show to tab panel

        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "M09024":
            case "M10003":
            case "M11003":
            case "M10012":
            case "M11012":
            case "M12008":
                tab3.Visible = false;
                tab4.Visible = false;
                break;
        }

        tab4.Visible = true;
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
        doc_nf.Value = "VCHNUM";
        doc_df.Value = "VCHDATE";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "PROJ_ASGN";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "AP");
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

            case "BTN_20":

                break;
            case "BTN_21":

                break;
            case "BTN_22":

                break;
            case "BTN_23":

                break;
            case "TACODE":
                //pop1
                SQuery = "SELECT acode as fstr,NAME,acode FROM proj_mast WHERE branchcd!='DD' and type='P8' order by acode desc ";
                break;
            case "TICODE":
                //pop1
                SQuery = "SELECT acode as fstr,NAME,acode FROM proj_mast WHERE branchcd!='DD' and type='P1' order by name ";
                break;
            case "TICODE10":
                //pop1                
                frm_DeptType = fgen.seek_iname(frm_qstr, frm_cocd, "select TRIM(UPPER(B.req_name)) AS req_name FROM EVAS A,PROJ_MAST B WHERE TRIM(A.DEPTT)=TRIM(b.ACODE) AND B.TYPE='P8' AND A.USERID='" + frm_UserID + "' AND A.USERNAME='" + frm_uname + "'", "req_name");
                frm_DeptCode = fgen.seek_iname(frm_qstr, frm_cocd, "select a.deptt FROM EVAS A WHERE A.USERID='" + frm_UserID + "' AND A.USERNAME='" + frm_uname + "'", "deptt");
                SQuery = "SELECT BRANCHCD||TYPE||TRIM(vCHNUM) as fstr,NAME,Vchnum as Proj_num,Start_Dt,End_Dt,Proj_hrs as Est_Hrs,TO_cHAR(VCHDATE,'YYYYMMDD') AS VDD FROM proj_dtl WHERE branchcd!='DD' and type='P0' /*and trim(DPCODE)='" + frm_DeptCode + "'*/ and trim(icode)='" + frm_UserID + "' and upper(trim(nvl(proj_refno,'NO')))!='YES' order by VDD DESC,vchnum desc ";
                if (frm_DeptType == "SUPPORT")
                {
                    //rdintOff.Visible = false;
                    SQuery = "SELECT BRANCHCD||'P0'||TRIM(PJCODE) as fstr,PROJ_NAME AS NAME,PJCODE as Proj_num,vchnum as task_Assign_no,ment_by assign_by,ment_dt as assign_dt,TO_cHAR(VCHDATE,'YYYYMMDD') AS VDD FROM proj_ASGN WHERE branchcd!='DD' and type='" + frm_vty + "' and I_O='1' order by VDD DESC,vchnum desc";
                    SQuery = "SELECT distinct BRANCHCD||'P0'||TRIM(PJCODE) as fstr,PROJ_NAME AS NAME,PJCODE as Proj_num,pjcode,remarks1 as activity FROM proj_ASGN WHERE branchcd!='DD' and type='" + frm_vty + "' and I_O='1' order by pjcode";
                }

                // final query
                SQuery = "SELECT BRANCHCD||TYPE||TRIM(vCHNUM) as fstr,NAME,Vchnum as Proj_num,TO_cHAR(VCHDATE,'YYYYMMDD') AS VDD,'-' as ts FROM proj_dtl WHERE branchcd!='DD' and type='P0' /*and trim(DPCODE)='" + frm_DeptCode + "'*/ and trim(icode)='" + frm_UserID + "' and upper(trim(nvl(proj_refno,'NO')))!='YES' ";
                SQuery = SQuery + " union all SELECT distinct BRANCHCD||'P0'||TRIM(PJCODE) as fstr,PROJ_NAME AS NAME,PJCODE as Proj_num,TO_cHAR(VCHDATE,'YYYYMMDD') AS VDD,'Offloaded Task' as ts FROM proj_ASGN WHERE branchcd!='DD' and type='" + frm_vty + "' and I_O='1' and ASGeCODE in (" + frm_DeptCode.Replace("`", "'") + ") ";

                break;
            case "TICODE13":
                //pop1
                SQuery = "SELECT acode as fstr,NAME,acode FROM proj_mast WHERE branchcd!='DD' and type='P2' order by name ";
                break;
            case "TICODE16":
                //pop1
                SQuery = "SELECT acode as fstr,NAME,acode,log_ref FROM proj_mast WHERE branchcd!='DD' and type='P7' order by name ";
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[13].Text.Trim() + "'";
                    else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                }
                if (col1.Length <= 0) col1 = "'-'";
                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(Trim(icode))>4 and icode like '9%' and trim(icode) in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname  ";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
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
            case "SG1_ROW_TAX":

                SQuery = "Select Type1 as fstr,Name,Type1 as Code,nvl(Rate,0) as Rate,nvl(Excrate,0) as Schg,exc_Addr as Ref_Code from type where id='S' and length(Trim(nvl(cstno,'-')))<=1 order by name";
                break;
            case "SG2_ROW_ADD":
                SQuery = "SELECT distinct branchcd||type||TRIM(vCHNUM)||to_char(vchdate,'dd/mm/yyyy') AS FSTR,NAME AS Milestone,REQ_NAME AS REQ_BY,MENT_BY AS ENTBY,vchnum as code FROM PROJ_MAST WHERE BRANCHCD='" + frm_mbr + "' and TYPE='M1' order by vchnum";
                SQuery = "SELECT BRANCHCD||TYPE||TRIM(VCHNUM)||TO_cHAR(vCHDATE,'DD/MM/YYY')||desc_ AS FSTR,desc_ AS MILESTONE,soremarks AS HRS FROM BUDGMST WHERE BRANCHCD='" + frm_mbr + "' and type='PM' and vchnum='" + txtlbl4.Text.Trim() + "' ";
                break;
            case "MILESTONE":
                SQuery = "SELECT vchnum||srno as fstr,desc_ AS MILESTONE,(case when jobcardqty>0 then jobcardqty else round(is_number(soremarks)-jobcardqty,2) end) AS HRS,REQ_CL_RSN as milestone_status FROM BUDGMST WHERE BRANCHCD='" + frm_mbr + "' and type='PM' and vchnum='" + txtlbl4.Text.Trim() + "' and vchdate " + DateRange + " and UPPER(REQ_CL_RSN) not like 'COMPLETE%' ";
                break;
            case "New":
                Type_Sel_query();
                break;
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            case "ASSGN":
                SQuery = "SELECT a.USERID,a.USERNAME AS NAME,a.USERID AS CODE,'" + txtlbl7a.Text.Trim() + "' as dept,a.ERPDEPTT as designation FROM EVAS a WHERE replace(a.deptt,'`','') like ('%" + txtlbl7.Text.Trim() + "%') order by a.USERNAME";
                //frm_DeptType = fgen.seek_iname(frm_qstr, frm_cocd, "select TRIM(UPPER(B.req_name)) AS req_name FROM EVAS A,PROJ_MAST B WHERE TRIM(A.DEPTT)=TRIM(b.ACODE) AND B.TYPE='P8' AND A.USERID='" + frm_UserID + "' AND A.USERNAME='" + frm_uname + "'", "req_name");
                frm_DeptCode = fgen.seek_iname(frm_qstr, frm_cocd, "select TRIM(deptt) AS deptt FROM EVAS A WHERE A.USERID='" + frm_UserID + "' AND A.USERNAME='" + frm_uname + "'", "deptt");
                //if (frm_DeptType == "SUPPORT") SQuery = "SELECT a.USERID,a.USERNAME AS NAME,a.USERID AS CODE,b.name as dept,a.ERPDEPTT as designation FROM EVAS a,PROJ_MAST B WHERE TRIM(A.DEPTT)=TRIM(b.ACODE) AND UPPER(B.req_name)='SUPPORT' AND B.TYPE='P8' and a.DEPTT='" + frm_DeptCode.ToString().Trim() + "' order by a.USERNAME";
                if (rdintOff.SelectedItem.Text == "  Offload")
                {
                    SQuery = "SELECT a.USERID,a.USERNAME AS NAME,a.USERID AS CODE FROM EVAS a,PROJ_MAST B WHERE TRIM(A.DEPTT)=TRIM(b.ACODE) AND UPPER(B.req_name)='SUPPORT' AND B.TYPE='P8' order by a.USERNAME";
                    SQuery = "SELECT acode as fstr,NAME,acode,req_name as dept_t FROM proj_mast WHERE branchcd!='DD' and type='P8' /*and trim(upper(req_name))='SUPPORT'*/ order by name ";
                }
                else if (hf1.Value == "OFFLOADED TASK")
                {
                    SQuery = "SELECT a.USERID,a.USERNAME AS NAME,a.USERID AS CODE,a.ERPDEPTT as designation FROM EVAS a WHERE replace(a.deptt,'`','') like ('%" + frm_DeptCode.Replace("`", "") + "%') order by a.USERNAME";
                }
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                {
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as entry_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as entry_Dt,a.Proj_Name as Name,b.name as dept,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a ,proj_mast b where trim(a.DPCODE)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + lbl1a.Text + "' and b.type='P8' order by vdd desc,a." + doc_nf.Value + " desc";
                    if (fgen.make_double(frm_ulvl) > 0)
                        SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as entry_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as entry_Dt,a.Proj_Name as Name,b.name as dept,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a,proj_mast b where trim(a.DPCODE)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + lbl1a.Text + "' and b.type='P8' and a.ment_by='" + frm_uname + "' order by vdd desc,a." + doc_nf.Value + " desc";
                }
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        ddActivity.ClearSelection();
        DDlbl14.ClearSelection();
        ddlbl5.ClearSelection();
        ddTType.ClearSelection();
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";
            make_qry_4_popup();
            fgen.Fn_open_sseek("-", frm_qstr);

            // else comment upper code

            //frm_vnum = fgen.next_no(frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
            //txtvchnum.Text = frm_vnum;
            //txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            //disablectrl();
            //fgen.EnableForm(this.Controls);
            DDlbl14.Items.FindByText("-Select-").Selected = true;
            ddTType.Items.FindByText("New").Selected = true;
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

        fgen.fill_dash(this.Controls);


        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }
        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only"); txtvchdate.Focus(); return; }

        string err_msg;
        err_msg = "";
        if (txtlbl4.Text.Trim().Length < 2)
        {
            err_msg = err_msg + "Department | ";
        }
        if (txtlbl7.Text.Trim().Length < 2)
        {
            err_msg = err_msg + "B.U. | ";
        }
        if (txtlbl10.Text.Trim().Length < 2)
        {
            err_msg = err_msg + "Proj Name | ";
        }
        if (txtlbl13.Text.Trim().Length < 2)
        {
            err_msg = err_msg + "Task Type | ";
        }
        if (txtassineecode.Text.Trim().Length < 2)
        {
            err_msg = err_msg + "Assignee | ";
        }
        if (txtlbl8.Text.Trim().Length < 2)
        {
            err_msg = err_msg + "Assigned Date | ";
        }
        if (err_msg.Trim().Length > 2)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + err_msg + " Not Filled Correctly, Please update The Required Fields !!");
            return;
        }
        if (DDlbl14.SelectedValue.ToString().ToUpper() == "-SELECT-")
        {
            fgen.msg("-", "AMSG", "Please Select I.A.!!");
            return;
        }
        //if (DDlbl14.SelectedValue.ToString().ToUpper() == "YES" && txtlbl14.Text.Trim() == "-")
        //{
        //    fgen.msg("-", "AMSG", "Please add I.A Checklist No.!!");
        //    return;
        //}
        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT REQ_NAME FROM PROJ_MAST WHERE TYPE='P8' and acode='" + txtlbl7.Text.Trim() + "'", "REQ_NAME");
        if (col1.ToString().ToUpper().Trim() == "OFFSHORE")
        {
            if (txtlbl5.Text.Trim().Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Fill DW/PCN No.!!");
                return;
            }
            if (txtlbl15.Text.Trim() == "-" || txtlbl15.Text.Trim() == "")
            {
                fgen.msg("-", "AMSG", "Please Select Priority for the Project!!");
                return;
            }
            if (TextName2.Text.Trim().Length < 2)
            {
                fgen.msg("-", "AMSG", "Please add Perticulars of Activity!!");
                return;
            }
            //TextName1.Text = TextName2.Text;
        }
        if (ddTType.SelectedItem.Text.Trim().ToUpper() == "REWORK" && txtrmk.Text.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Fill Remarks for Reworking.!!");
            return;
        }
        if (rdintOff.SelectedItem.Text != "  Offload" && fgen.make_double(txtlbl6a.Text.Trim()) <= 0)
        {
            fgen.msg("-", "AMSG", "Please Add Estimate Hrs!!");
            return;
        }
        if (ddActivity.SelectedItem.Text.Trim().ToUpper() == "--SELECT--")
        {
            fgen.msg("-", "AMSG", "Activity not Selected!!");
            return;
        }
        if (sg2.Rows.Count < 2)
        {
            //col1 = fgen.seek_iname(frm_qstr,frm_cocd, "select sum(jobcardqty) as jobcardqty from budgmst where branchcd='" + frm_mbr + "' and type='BB' and vchnum='" + txtlbl4.Text.Trim() + "'", "jobcardqty");
            //txtlbl6.Text = col1;
        }

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
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
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + DateRange + " and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
        //fgen.Fn_FillChart(frm_cocd, frm_qstr, "Testing Chart", "line", "Main Heading", "Sub Heading", SQuery);
        //hffield.Value = "Print";
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
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
                //fgen.execute_cmd(frm_qstr,frm_cocd, "delete from poterm a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                //fgen.execute_cmd(frm_qstr,frm_cocd, "delete from budgmst a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
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
                    //new_click
                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;

                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

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

                    //--------------------------------
                    ////sg4_dt = new DataTable();
                    ////create_tab4();
                    ////sg4_add_blankrows();
                    ////sg4_add_blankrows();
                    ////sg4_add_blankrows();
                    ////sg4_add_blankrows();
                    ////sg4_add_blankrows();
                    ////sg4.DataSource = sg4_dt;
                    ////sg4.DataBind();
                    ////setColHeadings();
                    ////ViewState["sg4"] = sg4_dt;


                    break;
                    #endregion
                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
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
                case "MILESTONE":
                    if (col1 == "") return;
                    txtMileStoneCode.Text = col1;
                    txtMileStone.Text = col2;
                    txtMilestoneStatus.Text = col3;
                    //************************
                    //Total Hrs
                    txtlbl6.Text = col3;
                    //Total Assign Hrs
                    txtlbl9.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select sum(EST_HRS) as EST_HRS from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and TRIM(PJCODE)='" + txtlbl4.Text + "' AND TRIM(MILESTONECODE)='" + txtMileStoneCode.Text.Trim() + "' ", "EST_HRS");
                    // Diff
                    txtlbl12.Text = (fgen.make_double(col3) - fgen.make_double(txtlbl9.Text)).ToString();
                    //Actual Hrs
                    col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT round(SUM(a.DT_HRS),2) as ss FROM PROJ_DTIME a,proj_asgn b WHERE a.branchcd||trim(a.projcode)=b.branchcd||trim(b.pjcode) and a.BRANCHCD='" + frm_mbr + "' and a.type='UP' and a.PROJCODE='" + txtlbl4.Text + "' AND TRIM(b.milestonecode)='" + txtMileStoneCode.Text.Trim() + "'", "ss");
                    txtlbl9a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select round(sum(round(24*(to_date(uend_Dt||' '||uend_time,'dd/mm/yyyy hh24:mi')-to_date(ustart_Dt||' '||ustart_time,'dd/mm/yyyy hh24:mi')),2))-" + col3 + ",2) as ss from proj_updt where Projcode='" + txtlbl4.Text + "' AND TRIM(milestonecode)='" + txtMileStoneCode.Text.Trim() + "'", "ss");
                    txtlbl6.Focus();
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    string mv_col;
                    mv_col = frm_mbr + frm_vty + col1;
                    SQuery = "Select a.*,to_Char(a.ment_Dt,'dd/mm/yyyy') As ment_date,to_Char(a.mapp_Dt,'dd/mm/yyyy') As mapp_date from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + mv_col + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["entby"] = dt.Rows[0]["ment_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ment_dt"].ToString();

                        txtvchnum.Text = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl2.Text = dt.Rows[i]["ment_by"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["mapp_by"].ToString().Trim();

                        txtMileStoneCode.Text = dt.Rows[i]["milestonecode"].ToString().Trim();
                        txtMileStone.Text = dt.Rows[i]["milestone"].ToString().Trim();
                        txtMilestoneStatus.Text = dt.Rows[i]["milestonestatus"].ToString().Trim();
                        txtPsp.Text = dt.Rows[i]["psp"].ToString().Trim();

                        txtlbl4.Text = dt.Rows[i]["PJCODE"].ToString().Trim();
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from proj_DTL where branchcd='" + frm_mbr + "' and type='P0' and trim(vchnum)='" + txtlbl4.Text.Trim() + "'", "name");
                        txtlbl7.Text = dt.Rows[i]["DPCODE"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from proj_mast where branchcd='" + frm_mbr + "' and type='P8' and trim(Acode)='" + txtlbl7.Text.Trim() + "'", "name");
                        txtlbl10.Text = dt.Rows[i]["ACODE"].ToString().Trim();
                        txtlbl10a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from proj_mast where branchcd='" + frm_mbr + "' and type='CS' and trim(Acode)='" + txtlbl10.Text.Trim() + "'", "name");
                        txtlbl52.Text = dt.Rows[i]["TKCODE"].ToString().Trim();
                        txtlbl52a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select username as name from evas where trim(userid)='" + txtlbl52.Text.Trim() + "'", "name");
                        txtlbl13.Text = dt.Rows[i]["BUCODE"].ToString().Trim();
                        txtlbl13a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from proj_mast where branchcd='" + frm_mbr + "' and type='P1' and trim(acode)='" + txtlbl13.Text.Trim() + "'", "name");
                        txtlbl16.Text = dt.Rows[i]["catg"].ToString().Trim();
                        txtlbl16a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from proj_mast where branchcd='" + frm_mbr + "' and type='P9' and trim(acode)='" + txtlbl16.Text.Trim() + "'", "name");

                        txtassineecode.Text = dt.Rows[i]["ASGeCODE"].ToString().Trim();

                        if (dt.Rows[i]["I_O"].ToString().Trim() == "0") txtassineeName.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select username as name from evas where branchcd='" + frm_mbr + "' and trim(userid)='" + txtassineecode.Text.Trim() + "'", "name");
                        else txtassineeName.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT acode as fstr,NAME,acode,req_name as dept_t FROM proj_mast WHERE branchcd!='DD' and type='P8' and acode='" + txtassineecode.Text + "'", "name");

                        txtlbl5.Text = dt.Rows[i]["DPC_NO"].ToString().Trim();

                        txtlbl6.Text = dt.Rows[i]["GIVEN_HR"].ToString().Trim();
                        txtlbl9.Text = dt.Rows[i]["USED_HRS"].ToString().Trim();
                        txtlbl12.Text = dt.Rows[i]["DIFF_HRS"].ToString().Trim();

                        txtlbl6a.Text = dt.Rows[i]["EST_HRS"].ToString().Trim();
                        txtlbl9a.Text = dt.Rows[i]["Alloted_HRS"].ToString().Trim();
                        txtlbl12a.Text = dt.Rows[i]["Left_HRS"].ToString().Trim();

                        txtlbl8.Text = dt.Rows[i]["ASSGN_DT"].ToString().Trim();
                        txtlbl11.Text = dt.Rows[i]["ASSGN_TIME"].ToString().Trim();

                        try
                        {
                            ddTType.ClearSelection();
                            ddTType.Items.FindByText(dt.Rows[i]["TTYPE"].ToString().Trim());

                            txtrmk.Text = dt.Rows[i]["rework"].ToString().Trim();
                        }
                        catch { }

                        TextName2.Text = dt.Rows[i]["remarks2"].ToString().Trim();

                        txtlbl12.Text = dt.Rows[i]["ALERT_DT"].ToString().Trim();

                        txtlbl14.Text = dt.Rows[i]["IAC_FILLED"].ToString().Trim();
                        try
                        {
                            DDlbl14.ClearSelection();
                            DDlbl14.Items.FindByValue(dt.Rows[i]["IA_FILLED"].ToString().Trim()).Selected = true;
                        }
                        catch { }
                        txtlbl15.Text = dt.Rows[i]["OTHERS"].ToString().Trim();
                        try
                        {
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, "select name,ACODE from proj_mast where branchcd!='DD' and type='A1' and trim(req_by)='" + txtlbl7.Text.Trim() + "' order by name");
                            if (dt.Rows.Count > 0)
                            {
                                ddActivity.DataSource = dt;
                                ddActivity.DataTextField = "name";
                                ddActivity.DataValueField = "ACODE";
                                ddActivity.DataBind();
                            }

                            ddActivity.ClearSelection();
                            ddActivity.Items.FindByText(dt.Rows[i]["remarks1"].ToString().Trim()).Selected = true;
                            rdintOff.ClearSelection();
                            rdintOff.Items.FindByValue(dt.Rows[i]["I_O"].ToString().Trim()).Selected = true;
                            rdintOff.Enabled = false;

                            ddlbl5.ClearSelection();
                            ddlbl5.Items.FindByText(dt.Rows[i]["ICAT"].ToString().Trim()).Selected = true;

                            ddTType.ClearSelection();
                            ddTType.Items.FindByText(dt.Rows[i]["TTYPE"].ToString().Trim()).Selected = true;

                        }
                        catch { }

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            //sg1_dr["sg1_h1"] = "-";
                            //sg1_dr["sg1_h2"] = "-";
                            //sg1_dr["sg1_h3"] = "-";
                            //sg1_dr["sg1_h4"] = "-";
                            //sg1_dr["sg1_h5"] = "-";
                            //sg1_dr["sg1_h6"] = "-";


                            //sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                            //sg1_dr["sg1_f2"] = dt.Rows[i]["Iname"].ToString().Trim();
                            //sg1_dr["sg1_f3"] = dt.Rows[i]["ICpartno"].ToString().Trim();
                            //sg1_dr["sg1_f4"] = dt.Rows[i]["ICdrgno"].ToString().Trim();
                            //sg1_dr["sg1_f5"] = dt.Rows[i]["Unit"].ToString().Trim();

                            //sg1_dr["sg1_t1"] = dt.Rows[i]["desc_"].ToString().Trim();
                            //sg1_dr["sg1_t2"] = dt.Rows[i]["cu_chldt1"].ToString().Trim();
                            //sg1_dr["sg1_t3"] = dt.Rows[i]["qtyord"].ToString().Trim();
                            //sg1_dr["sg1_t4"] = dt.Rows[i]["irate"].ToString().Trim();
                            //sg1_dr["sg1_t5"] = dt.Rows[i]["cdisc"].ToString().Trim();

                            //sg1_dr["sg1_t6"] = dt.Rows[i]["class"].ToString().Trim();
                            //sg1_dr["sg1_t7"] = dt.Rows[i]["ipack"].ToString().Trim();
                            //sg1_dr["sg1_t8"] = dt.Rows[i]["SD"].ToString().Trim();

                            //sg1_dr["sg1_t9"] = dt.Rows[i]["pexc"].ToString().Trim();
                            //sg1_dr["sg1_t10"] = dt.Rows[i]["st_type"].ToString().Trim();
                            //sg1_dr["sg1_t11"] = dt.Rows[i]["ptax"].ToString().Trim();
                            //sg1_dr["sg1_t12"] = dt.Rows[i]["desc9"].ToString().Trim();
                            //sg1_dr["sg1_t13"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            //sg1_dr["sg1_t14"] = dt.Rows[i]["iexc_Addl"].ToString().Trim();
                            //sg1_dr["sg1_t15"] = dt.Rows[i]["qtysupp"].ToString().Trim();
                            //sg1_dr["sg1_t16"] = dt.Rows[i]["sta_Rate"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        //------------------------
                        //SQuery = "Select nvl(a.terms,'-') as terms,nvl(a.condi,'-') as condi from poterm a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + mv_col + "' ORDER BY a.sno";
                        //dt = new DataTable();
                        //dt = fgen.getdata(frm_qstr,frm_cocd, SQuery);

                        //create_tab2();
                        //sg2_dr = null;
                        //if (dt.Rows.Count > 0)
                        //{
                        //    for (i = 0; i < dt.Rows.Count; i++)
                        //    {

                        //        sg2_dr = sg2_dt.NewRow();
                        //        sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;

                        //        sg2_dr["sg2_t1"] = dt.Rows[i]["terms"].ToString().Trim();
                        //        sg2_dr["sg2_t2"] = dt.Rows[i]["condi"].ToString().Trim();

                        //        sg2_dt.Rows.Add(sg2_dr);
                        //    }
                        //}
                        //sg2_add_blankrows();
                        //ViewState["sg2"] = sg2_dt;
                        //sg2.DataSource = sg2_dt;
                        //sg2.DataBind();
                        //dt.Dispose();
                        //sg2_dt.Dispose();
                        //------------------------
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

                        //SQuery = "Select a.icode,to_chaR(a.dlv_Date,'dd/mm/yyyy') As dlv_Date,nvl(a.budgetcost,0) as budgetcost,nvl(a.actualcost,0) as actualcost,a.jobcardrqd,b.iname,nvl(b.cpartno,'-') As cpartno,nvl(b.cdrgno,'-') as cdrgno,nvl(b.unit,'-') as Unit from budgmst a,item b where trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + mv_col + "' ORDER BY A.SRNO ";
                        ////union all Select '-' as icode,to_DatE(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy') as dlv_Date,0 as budgetcost,'-' as iname,'-' As cpartno,'-' as cdrgno,nvl(b.unit,'-') as Unit from dual                         

                        //dt = new DataTable();
                        //dt = fgen.getdata(frm_qstr,frm_cocd, SQuery);

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
                        btnlbl4.Enabled = false;
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                        rdintOff.Enabled = false;
                    }
                    #endregion
                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;

                    btnlbl7.Focus();
                    break;
                case "SG2_ROW_ADD":
                    double d1 = 0;
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

                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                            sg2_dr["sg2_t3"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim();
                            d1 += fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim());
                            sg2_dt.Rows.Add(sg2_dr);
                        }

                        dt = new DataTable();
                        SQuery = "select * from budgmst where BRANCHCD||TYPE||TRIM(VCHNUM)||TO_cHAR(vCHDATE,'DD/MM/YYY')||desc_='" + col1 + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = d + 1;
                            sg2_dr["sg2_t1"] = dt.Rows[d]["desc_"].ToString().Trim();
                            sg2_dr["sg2_t2"] = dt.Rows[d]["soremarks"].ToString().Trim();
                            sg2_dr["sg2_t3"] = dt.Rows[d]["REQ_CL_RSN"].ToString().Trim();
                            d1 += fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim());
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                    }
                    sg2_add_blankrows();

                    ViewState["sg2"] = sg2_dt;
                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    dt.Dispose(); sg2_dt.Dispose();
                    ((TextBox)sg2.Rows[z].FindControl("sg2_t2")).Focus();
                    //txtlbl6.Text = d1.ToString();
                    #endregion
                    setColHeadings();
                    break;
                case "ASSGN":
                    if (col1.Length <= 0) return;
                    txtassineecode.Text = col1;
                    txtassineeName.Text = col2;
                    rdintOff.Enabled = false;

                    frm_DeptType = fgen.seek_iname(frm_qstr, frm_cocd, "select TRIM(UPPER(B.req_name)) AS req_name FROM EVAS A,PROJ_MAST B WHERE TRIM(A.DEPTT)=TRIM(b.ACODE) AND B.TYPE='P8' AND A.USERID='" + frm_UserID + "' AND A.USERNAME='" + frm_uname + "'", "req_name");
                    if (rdintOff.SelectedItem.Text == "  Offload")
                    {
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "select '--Select--' as name,'000000' as acode from dual union all select name,ACODE from proj_mast where branchcd!='DD' and type='A1' and trim(req_by)='" + txtassineecode.Text.Trim() + "' ");
                        if (dt.Rows.Count > 0)
                        {
                            ddActivity.DataSource = dt;
                            ddActivity.DataTextField = "name";
                            ddActivity.DataValueField = "ACODE";
                            ddActivity.DataBind();
                        }
                    }
                    btnMileStone.Focus();
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
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;

                    txtlbl7.Focus();
                    btnlbl10.Focus();
                    break;
                case "TICODE10":
                    if (col1.Length <= 0) return;
                    txtlbl10.Text = col1;
                    txtlbl10a.Text = col2;
                    //txtlbl5.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    //txtlbl9.Text = fgen.seek_iname(frm_qstr,frm_cocd, "select substr(new_time,13,10) as new_time from (select to_char(sysdate,'dd/mm/yyyy :hh24:mi')  as new_time from dual)", "new_time");                    

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT * FROM PROJ_DTL WHERE BRANCHCD||TYPE||TRIM(VCHNUM)='" + col1 + "' ");
                    i = 0;
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl4.Text = dt.Rows[i]["vchnum"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[i]["dpcode"].ToString().Trim();
                        txtlbl10.Text = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl13.Text = dt.Rows[i]["bucode"].ToString().Trim();
                        txtlbl16.Text = dt.Rows[i]["catg"].ToString().Trim();
                        txtlbl52.Text = dt.Rows[i]["icode"].ToString().Trim();

                        txtlbl4a.Text = dt.Rows[i]["name"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from proj_mast where branchcd='" + frm_mbr + "' and type='P8' and trim(Acode)='" + txtlbl7.Text.Trim() + "'", "name");
                        txtlbl10a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from proj_mast where branchcd='" + frm_mbr + "' and type='CS' and trim(acode)='" + txtlbl10.Text.Trim() + "'", "name");
                        txtlbl13a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from proj_mast where branchcd='" + frm_mbr + "' and type='P1' and trim(acode)='" + txtlbl13.Text.Trim() + "'", "name");
                        txtlbl16a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from proj_mast where branchcd='" + frm_mbr + "' and type='P9' and trim(acode)='" + txtlbl16.Text.Trim() + "'", "name");
                        txtlbl52a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select username as name from evas where trim(userid)='" + txtlbl52.Text.Trim() + "'", "name");

                        txtlbl8.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtlbl11.Text = DateTime.Now.ToString("HH:mm");

                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "select '--Select--' as name,'000000' as acode from dual union all (select name,ACODE from proj_mast where branchcd!='DD' and type='A1' and trim(req_by)='" + txtlbl7.Text.Trim() + "' )");
                        if (dt.Rows.Count > 0)
                        {
                            ddActivity.DataSource = dt;
                            ddActivity.DataTextField = "name";
                            ddActivity.DataValueField = "ACODE";
                            ddActivity.DataBind();
                        }
                    }
                    //frm_DeptType = fgen.seek_iname(frm_qstr, frm_cocd, "select TRIM(UPPER(B.req_name)) AS req_name FROM EVAS A,PROJ_MAST B WHERE TRIM(A.DEPTT)=TRIM(b.ACODE) AND B.TYPE='P8' AND A.USERID='" + frm_UserID + "' AND A.USERNAME='" + frm_uname + "'", "req_name");
                    frm_DeptType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    if (frm_DeptType.ToUpper().Trim() == "OFFLOADED TASK")
                    {
                        hf1.Value = "OFFLOADED TASK";
                        dt = new DataTable();
                        SQuery = "SELECT * FROM PROJ_ASGN WHERE branchcd!='DD' and type='" + frm_vty + "' and I_O='1' and PJCODE='" + txtlbl4.Text.Trim() + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        i = 0;
                        if (dt.Rows.Count > 0)
                        {
                            txtMileStoneCode.Text = dt.Rows[i]["milestonecode"].ToString().Trim();
                            txtMileStone.Text = dt.Rows[i]["milestone"].ToString().Trim();
                            txtMilestoneStatus.Text = dt.Rows[i]["milestonestatus"].ToString().Trim();
                            txtPsp.Text = dt.Rows[i]["psp"].ToString().Trim();

                            txtlbl5.Text = dt.Rows[i]["DPC_NO"].ToString().Trim();

                            txtlbl6.Text = dt.Rows[i]["EST_HRS"].ToString().Trim();
                            txtlbl9.Text = dt.Rows[i]["Alloted_HRS"].ToString().Trim();
                            txtlbl12.Text = dt.Rows[i]["Left_HRS"].ToString().Trim();

                            txtlbl8.Text = dt.Rows[i]["ASSGN_DT"].ToString().Trim();
                            txtlbl11.Text = dt.Rows[i]["ASSGN_TIME"].ToString().Trim();

                            try
                            {
                                ddTType.ClearSelection();
                                ddTType.Items.FindByText(dt.Rows[i]["TTYPE"].ToString().Trim());
                            }
                            catch { }

                            //txtlbl11.Text = dt.Rows[i]["ASSGN_TIME"].ToString().Trim();
                            //txtlbl12.Text = dt.Rows[i]["ALERT_DT"].ToString().Trim();

                            txtlbl14.Text = dt.Rows[i]["IAC_FILLED"].ToString().Trim();

                            try
                            {
                                DDlbl14.ClearSelection();
                                DDlbl14.Items.FindByValue(dt.Rows[i]["IA_FILLED"].ToString().Trim()).Selected = true;
                            }
                            catch { }
                            txtlbl15.Text = dt.Rows[i]["OTHERS"].ToString().Trim();
                            try
                            {
                                dt4 = new DataTable();
                                dt4 = fgen.getdata(frm_qstr, frm_cocd, "select '--Select--' as name,'000000' as acode from dual union all (select name,ACODE from proj_mast where branchcd!='DD' and type='A1' )");
                                if (dt4.Rows.Count > 0)
                                {
                                    ddActivity.DataSource = dt4;
                                    ddActivity.DataTextField = "name";
                                    ddActivity.DataValueField = "ACODE";
                                    ddActivity.DataBind();
                                }

                                ddActivity.ClearSelection();
                                ddActivity.Items.FindByText(dt.Rows[i]["remarks1"].ToString().Trim()).Selected = true;

                                ddActivity.Enabled = false;

                                ddlbl5.ClearSelection();
                                ddlbl5.Items.FindByText(dt.Rows[i]["ICAT"].ToString().Trim()).Selected = true;

                                ddTType.ClearSelection();
                                ddTType.Items.FindByText(dt.Rows[i]["TTYPE"].ToString().Trim()).Selected = true;

                                TextName2.Text = dt.Rows[i]["remarks2"].ToString().Trim();
                                txtrmk.Text = dt.Rows[i]["rework"].ToString().Trim();
                            }
                            catch { }
                        }
                    }

                    btnassineecode.Focus();
                    break;
                case "TICODE13":
                    if (col1.Length <= 0) return;
                    txtlbl13.Text = col1;
                    txtlbl13a.Text = col2;
                    txtlbl13.Focus();
                    btnlbl16.Focus();
                    break;
                case "TICODE16":
                    if (col1.Length <= 0) return;
                    txtassineecode.Text = col1;
                    txtassineeName.Text = col2;
                    txtlbl16.Focus();
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
                case "SG1_ROW_TAX":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t16")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t12")).Focus();
                    break;
                case "SG1_ROW_DT":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    break;

                //case "sg1_Row_Tax_E":
                //    if (col1.Length <= 0) return;
                //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[27].Text = col1;
                //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[28].Text = col2;
                //    setColHeadings();
                //    break;
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
                            sg2_dr["sg2_t3"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim();

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
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            cond = " and trim(asgecode)='" + frm_UserID + "' ";
            SQuery = "Select a." + doc_nf.Value + " as Assign_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Assign_Dt,a.Proj_name,a.AsgeName,a.Est_hrs,a.Assgn_Dt,A.Assgn_Time,a.Target_dt,a.Alert_Dt,a.IAC_Filled,a.Ment_by,a.Ment_Dt,a.Mapp_by,(Case when length(Trim(A.Mapp_by))<2 then null else a.Mapp_dt end) as App_Dt from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "'  and a.type like '" + frm_vty + "%' " + cond + " order by a." + doc_df.Value + ",a." + doc_nf.Value + ",a.srno ";
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
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and " + doc_df.Value + " " + DateRange + " ", "ldt");
            if (last_entdt == "0" || edmode.Value == "Y")
            {
            }
            else
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

                        oDS2 = new DataSet();
                        oporow2 = null;
                        //oDS2 = fgen.fill_schema(frm_qstr,frm_cocd, "ivchctrl");

                        oDS3 = new DataSet();
                        oporow3 = null;
                        //oDS3 = fgen.fill_schema(frm_qstr,frm_cocd, "poterm");

                        oDS4 = new DataSet();
                        oporow4 = null;
                        //oDS4 = fgen.fill_schema(frm_qstr,frm_cocd, "budgmst");

                        oDS5 = new DataSet();
                        oporow5 = null;
                        oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");


                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();
                        //save_fun2();
                        save_fun3();
                        save_fun4();
                        save_fun5();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        //oDS2 = fgen.fill_schema(frm_qstr,frm_cocd, "ivchctrl");

                        oDS3.Dispose();
                        oporow3 = null;
                        oDS3 = new DataSet();
                        //oDS3 = fgen.fill_schema(frm_qstr,frm_cocd, "poterm");

                        oDS4.Dispose();
                        oporow4 = null;
                        oDS4 = new DataSet();
                        //oDS4 = fgen.fill_schema(frm_qstr,frm_cocd, "budgmst");

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
                            save_it = "Y";
                            //for (i = 0; i < sg1.Rows.Count - 0; i++)
                            //{
                            //    if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2)
                            //    {
                            //        save_it = "Y";
                            //    }
                            //}

                            if (save_it == "Y")
                            {

                                i = 0;


                                do
                                {
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                    if (i > 20)
                                    {
                                        fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                                        pk_error = "N";
                                        i = 0;
                                    }
                                    i++;
                                }
                                while (pk_error == "Y");
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        //save_fun2();
                        save_fun3();
                        save_fun4();
                        save_fun5();
                        string ddl_fld1;
                        string ddl_fld2;
                        ddl_fld1 = frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                        ddl_fld2 = frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                        if (edmode.Value == "Y")
                        {

                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + ddl_fld1 + "'");
                            //fgen.execute_cmd(frm_qstr,frm_cocd, "update poterm set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + ddl_fld1 + "'");
                            //fgen.execute_cmd(frm_qstr,frm_cocd, "update budgmst set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + ddl_fld1 + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update udf_Data set branchcd='DD' where par_tbl='" + frm_tabname + "' and par_fld='" + ddl_fld1 + "'");
                            //fgen.execute_cmd(frm_qstr,frm_cocd, "update ivchctrl set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");

                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        //fgen.save_data(frm_qstr, frm_cocd, oDS3, "poterm");
                        //fgen.save_data(frm_qstr, frm_cocd, oDS4, "budgmst");
                        fgen.save_data(frm_qstr, frm_cocd, oDS5, "udf_Data");
                        //fgen.save_data(frm_qstr, frm_cocd, oDS2, "ivchctrl");

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + ddl_fld2 + "'");
                            //fgen.execute_cmd(frm_qstr,frm_cocd, "delete from poterm where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + ddl_fld2 + "'");
                            //fgen.execute_cmd(frm_qstr,frm_cocd, "delete from budgmst where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + ddl_fld2 + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data where branchcd='DD' and par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + ddl_fld2 + "'");
                            //fgen.execute_cmd(frm_qstr,frm_cocd, "delete from ivchctrl where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");

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
        sg2_dt.Columns.Add(new DataColumn("sg2_DD2", typeof(string)));
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
        sg2_dr = sg2_dt.NewRow();


        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
        sg2_dr["sg2_t1"] = "-";
        sg2_dr["sg2_t2"] = "-";
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
            case "SG1_ROW_TAX":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_TAX";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                break;
            case "SG1_ROW_DT":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_DT";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

                    fgen.Fn_open_dtbox("Select Date", frm_qstr);

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
                if (index < sg2.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG2_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Milestone", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG2_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Milestone", frm_qstr);
                }
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
        hffield.Value = "TICODE10";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Project ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select BU_Name ", frm_qstr);
    }
    protected void btnlbl10_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE10";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Project Name ", frm_qstr);
    }
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE13";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Task Type ", frm_qstr);
    }
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE16";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Assignee ", frm_qstr);
    }

    //------------------------------------------------------------------------------------


    protected void btnlbl11_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl12_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
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

        oporow["BRANCHCD"] = frm_mbr;
        oporow["orignalbr"] = frm_mbr;
        oporow["TYPE"] = lbl1a.Text;
        oporow["" + doc_nf.Value + ""] = frm_vnum;
        oporow["" + doc_df.Value + ""] = txtvchdate.Text.Trim();
        oporow["SRNO"] = i;

        oporow["Remarks1"] = TextName1.Text;
        oporow["Remarks1"] = ddActivity.SelectedItem.Text.Trim();
        oporow["Remarks2"] = TextName2.Text;
        oporow["rework"] = txtrmk.Text;

        oporow["DPCODE"] = txtlbl7.Text;
        oporow["ACODE"] = txtlbl10.Text;
        oporow["TKCODE"] = txtlbl52.Text;
        oporow["BUCODE"] = txtlbl13.Text;
        oporow["CATG"] = txtlbl16.Text;

        oporow["PJCODE"] = txtlbl4.Text;
        oporow["Proj_Name"] = txtlbl4a.Text;

        oporow["ASGeCODE"] = txtassineecode.Text;
        oporow["ASGeName"] = txtassineeName.Text;

        oporow["milestonecode"] = txtMileStoneCode.Text;
        oporow["milestone"] = txtMileStone.Text;

        oporow["DPC_NO"] = txtlbl5.Text;

        oporow["GIVEN_HR"] = fgen.make_double(txtlbl6.Text);
        oporow["USED_HRS"] = fgen.make_double(txtlbl9.Text);
        oporow["DIFF_HRS"] = fgen.make_double(txtlbl12.Text);

        oporow["EST_HRS"] = fgen.make_double(txtlbl6a.Text);
        oporow["Alloted_HRS"] = fgen.make_double(txtlbl9a.Text);
        oporow["Left_HRS"] = fgen.make_double(txtlbl12a.Text);

        oporow["ASSGN_DT"] = txtlbl8.Text;
        oporow["ASSGN_TIME"] = txtlbl11.Text;
        //oporow["TARGET_DT"] = txtlbl11.Text;
        //oporow["ALERT_DT"] = txtlbl12.Text;
        oporow["IA_FILLED"] = DDlbl14.SelectedValue.ToString().Trim();
        oporow["IAC_FILLED"] = txtlbl14.Text.Trim().ToUpper();

        oporow["OTHERS"] = txtlbl15.Text;
        oporow["ASGrCODE"] = txtassineecode.Text;

        oporow["I_O"] = rdintOff.SelectedValue.ToString();
        oporow["milestonestatus"] = txtMilestoneStatus.Text;
        oporow["PSP"] = txtPsp.Text;

        oporow["ICAT"] = ddlbl5.SelectedItem.Text;
        oporow["TTYPE"] = ddTType.SelectedItem.Text;

        oporow["email_status"] = "N";

        if (edmode.Value == "Y")
        {
            oporow["meNt_by"] = ViewState["entby"].ToString();
            oporow["meNt_dt"] = ViewState["entdt"].ToString();
            oporow["medt_by"] = frm_uname;
            oporow["medt_dt"] = vardate;
            oporow["mapp_by"] = "-";
            oporow["mapp_dt"] = vardate;
        }
        else
        {
            oporow["meNt_by"] = frm_uname;
            oporow["meNt_dt"] = vardate;
            oporow["medt_by"] = "-";
            oporow["meDt_dt"] = vardate;
            oporow["mapp_by"] = "-";
            oporow["mapp_dt"] = vardate;
        }
        oDS.Tables[0].Rows.Add(oporow);

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
        SQuery = "SELECT 'AP' AS FSTR,'Assign Task/Project' as NAME,'AP' AS CODE FROM dual";

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
    protected void btnassineecode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ASSGN";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Assign", frm_qstr);
    }
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }
    protected void btnMileStone_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MILESTONE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Milestone", frm_qstr);
    }
    protected void ddActivity_SelectedIndexChanged(object sender, EventArgs e)
    {
        dt = fgen.getdata(frm_qstr, frm_cocd, "select name,ACODE from proj_mast where branchcd!='DD' and type='A1' and trim(req_by)='" + txtlbl7.Text.Trim() + "' order by name");
        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT desig FROM PROJ_MAST WHERE branchcd!='DD' and type='A1' and trim(req_by)='" + txtlbl7.Text.Trim() + "' AND TRIM(ACODE)='" + ddActivity.SelectedItem.Value.ToString().Trim() + "' ", "desig");
        txtPsp.Text = col1;
    }
}