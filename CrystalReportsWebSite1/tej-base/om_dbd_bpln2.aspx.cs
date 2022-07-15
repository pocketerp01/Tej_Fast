using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_dbd_bpln2 : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, SQuery2, SQuery3, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;
    DataTable distmrsDt;
    DataTable bomChildDt, bomDt = new DataTable();
    DataTable mrs = new DataTable();
    DataRow mrdr = null;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string html_body = "";
    string Prg_Id, lbl1a_Text, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    string mq0, mq1, mq2, mq3, mq4, mq5, mq6;
    string xprd1, xprd2;
    int colCount = 81;
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
                fgen.DisableForm(this.Controls);
                enablectrl();
            }
            set_Val();

            btnedit.Visible = false;
            btndel.Visible = false;
            btnlist.Visible = false;
            btncancel.Visible = false;
        }
        gridWidth();
    }
    void gridWidth()
    {
        #region
        int col_count = 0;
        double wid = 0;
        double ad = 50;
        double totWidth = 0;
        int widthMake = 0;
        int colFound = 1;
        for (int i = 1; i < sg1.Columns.Count; i++)
        {
            //if (colFound > i)
            {
                if (sg1.Rows.Count > 0)
                {
                    widthMake = (sg1.Rows[0].Cells[i].Text.Trim().Length) * 10;
                    if (widthMake < 50) widthMake = 50;
                    if (widthMake > 150) widthMake = 150;
                    totWidth += Convert.ToDouble(widthMake);
                    sg1.Columns[i].HeaderStyle.Width = widthMake;
                }
            }
        }
        if (sg2.Rows.Count > 0)
        {
            col_count = sg2.HeaderRow.Cells.Count;
            for (int i = 0; i < col_count; i++)
            {
                ad = 10;
                if (sg2.Rows[0].Cells[i].Text.Length < 2) ad = 8;
                wid += fgen.make_double(sg2.Rows[0].Cells[i].Text.Length, 0) * ad;
            }
            try { sg2.Width = Convert.ToUInt16(wid + 100); }
            catch { sg2.Width = 1500; }

            if (sg2.Width.Value <= 800 || sg2.Width.Value > 2000) sg2.Width = Unit.Percentage(100);
        }
        if (sg3.Rows.Count > 0)
        {
            col_count = sg3.HeaderRow.Cells.Count;
            for (int i = 0; i < col_count; i++)
            {
                ad = 10;
                if (sg3.Rows[0].Cells[i].Text.Length < 2) ad = 8;
                wid += fgen.make_double(sg3.Rows[0].Cells[i].Text.Length, 0) * ad;
            }
            try { sg3.Width = Convert.ToUInt16(wid + 100); }
            catch { sg3.Width = 1500; }

            if (sg3.Width.Value <= 800 || sg3.Width.Value > 2000) sg3.Width = Unit.Percentage(100);
        }
        if (sg4.Rows.Count > 0)
        {
            col_count = sg4.HeaderRow.Cells.Count;
            for (int i = 0; i < col_count; i++)
            {
                ad = 10;
                if (sg4.Rows[0].Cells[i].Text.Length < 2) ad = 8;
                wid += fgen.make_double(sg4.Rows[0].Cells[i].Text.Length, 0) * ad;
            }
            try { sg4.Width = Convert.ToUInt16(wid + 100); }
            catch { sg4.Width = 1500; }

            if (sg4.Width.Value <= 800 || sg4.Width.Value > 2000) sg4.Width = Unit.Percentage(100);
        }
        #endregion
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



        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
        sg4.DataSource = sg4_dt; sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();
        showHeadings();

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
        doc_nf.Value = "CSSNO";
        doc_df.Value = "CSSDT";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "WB_CSS_LOG";
        switch (Prg_Id)
        {
            case "F60101":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CS");
                typePopup = "N";
                break;
            case "F40999":
                lblheader.Text = "MRP / MIT";
                colCount = 80;
                grid1.Attributes.Add("class", "col-md-12");
                grid2.Visible = false;
                grid3.Visible = false;
                grid4.Visible = false;
                break;
            default:
                lblheader.Text = "Machine Resource Management";
                colCount = 80;
                grid1.Attributes.Add("class", "col-md-12");
                grid2.Visible = false;
                grid3.Visible = false;
                grid4.Visible = false;
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {

        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        if (CSR.Length > 1) cond = " and trim(a.ccode)='" + cond + "'";
        switch (btnval)
        {
            case "BTN_23":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1,rate  FROM TYPE WHERE ID='A' order by name ";
                break;
            case "TACODE":
                //pop1
                Acode_Sel_query();
                break;
            case "TICODE":
                //pop1
                Icode_Sel_query();
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
            case "New":
                Type_Sel_query();
                break;
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            case "USR":
                SQuery = "SELECT DISTINCT USERNAME ,USERNAME AS COCD,FULL_NAME AS company_name FROM EVAS WHERE LENGTH(TRIM(USERNAME))<5 and NVL(USERNAME,'-')!='-' ORDER BY USERNAME";
                break;
            case "PERSON":
                //SQuery = "select mobile as fstr, name as Client_Person_name,remarks as email,mobile,acode as client_code,type1 as code, ent_by,ent_dt from typemst  where ID='CP' AND UPPER(TRIM(acode))='" + txtlbl4.Value.ToUpper().Trim() + "'   order by name ";                
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as CSS_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as CSS_Dt,a.ccode as company,a.Eicon as Subjects,substr(a.remarks,1,60) as Remarks,a.dir_comp,a.Last_Action,a.cont_name as person,a.cont_no as contact_no, a.Ent_by,a.ent_Dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where  a.branchcd='" + frm_mbr + "' and a.type='" + lbl1a_Text + "' " + cond + " order by vdd desc,a." + doc_nf.Value + " desc";
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
            string fn = "";
            if (frm_formID == "F40999") fn = "MRP_MIT";
            else fn = "do_MC_MGMT";
            switch (fn)
            {
                case "MRP_MIT":
                    hffield.Value = fn;
                    SQuery = "Select Vchnum as fstr,Vchnum,wk_Ref,sum(num01) as Qty,vchdate,to_char(Vchdate,'yyyymmdd') As dt_Str from sl_plan where branchcd='" + frm_mbr + "' and type='SL' and vchdate " + DateRange + " and num01>0 and wk_Ref>0 and upper(trim(isarch))<>'Y' group by Vchnum,wk_Ref,vchdate,to_char(Vchdate,'yyyymmdd')  having sum(num01)>0 order by vchdate desc,wk_Ref desc,vchnum desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_mseek("Select Sale Plan For MRP", frm_qstr);
                    btnsave.Visible = true;
                    break;
                default:
                    fgen.Fn_open_prddmp1("", frm_qstr);
                    break;
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    //------------------------------------------------------------------------------------
    void fillGrid()
    {
        SQuery = "";
        string fn = "";
        if (frm_formID == "F40999") fn = "MRP_MIT";
        else fn = "do_MC_MGMT";
        create_tab();
        switch (fn)
        {
            case "MRP_MIT":
                #region MRP MIT
                //***********
                #region checking Cyclical BOM

                dt = new DataTable();
                SQuery = "select branchcd||'-'||trim(icode)||'-'||trim(ibcode) as bom_link,branchcd,type,vchnum,vchdate,ent_by,ent_dt,edt_by,edt_dt from itemosp where branchcd!='DD' and branchcd||'-'||trim(icode)||'-'||trim(ibcode) in (Select branchcd||'-'||trim(ibcode)||'-'||trim(icode) from itemosp where branchcd!='DD')";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0) { cyclicBomRptLevel(); return; }

                SQuery = "select branchcd||'-'||trim(ibcode)||'-'||trim(icode) as bom_link,branchcd,type,vchnum,vchdate,ent_by,ent_dt,edt_by,edt_dt from itemosp where branchcd!='DD' and branchcd||'-'||trim(ibcode)||'-'||trim(icode) in (Select branchcd||'-'||trim(icode)||'-'||trim(ibcode) from itemosp where branchcd!='DD')";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0) { cyclicBomRptLevel(); return; }

                SQuery = "select B.INAME ,B.cdrgno,A.vchnum,A.vchdate,a.icode,count(vchnum) as lines from itemosp A,ITEM B  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and trim(A.icode)=trim(a.ibcode) AND A.type='BM' and A.branchcd='" + frm_mbr + "' and A.vchnum<>'000000' group by B.INAME ,B.cdrgno,A.vchnum,A.vchdate,a.icode order by A.vchdate desc ,A.vchnum desc";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0) { cyclicBomRptLevel(); return; }

                #endregion
                //***********

                MRP_W_MIT();
                btnsave.Disabled = false;                
                #endregion
                break;
            case "do_MC_MGMT":
                #region MIT

                fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");

                do_MC_MGMT();
                #endregion
                break;
            default:
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_cHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR,A.ORDNO AS PO_NO,TO_cHAR(A.ORDDT,'DD/MM/YYYY') AS PO_dT,B.ANAME AS CUSTOMER,A.ACODE AS CODE,a.type,TO_CHAR(A.ORDDT,'YYYYMMDD') AS VDD FROM POMAS A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '5%' AND A.ORDDT " + DateRange + " ORDER BY VDD DESC,a.ordno desc,a.type";

                lblSg1.Text = "Details of Purchase Orders";
                lblSg2.Text = "Details of Purchase Orders";
                lblSg3.Text = "Details of Purchase Orders";
                lblSg4.Text = "Details of Purchase Orders";
                break;
        }
        if (sg1_dt.Rows.Count > 0)
        {
            sg1.DataSource = sg1_dt;
            sg1.DataBind();

            //setGridWidth(sg1);
            gridWidth();
            showHeadings();
            ViewState["sg1_dt"] = sg1_dt;
        }
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
        hffield.Value = "SAVE";

        if (frm_formID == "F40999") fgen.msg("-", "SMSG", "Are You Sure, You Want To Make PR!!");
        //fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
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
        //Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
        Server.Transfer("~/tej-base/desktop.aspx?STR=" + frm_qstr);
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
        showHeadings();
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
        if (ViewState["sg1_dt"] != null)
        {
            sg1_dt = (DataTable)ViewState["sg1_dt"];
            if (sg1_dt.Rows.Count > 0) fgen.exp_to_excel(sg1_dt, "ms-excel", "xls", frm_cocd + "_" + lblheader.Text + "_" + DateTime.Now.ToString().Trim());
            else fgen.msg("-", "AMSG", "No Data to Export");
        }
        //SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + DateRange + " and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
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
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data a where par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

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

                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    //txtlbl4.Text = col1;
                    //txtlbl4a.Text = col2;

                    //txtlbl5.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    //txtlbl6.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");

                    //btnlbl7.Focus();
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
                    //txtlbl7.Text = col1;
                    //txtlbl7a.Text = col2;
                    //txtlbl2.Focus();
                    break;
                case "SG1_ROW_ADD":

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
                    break;
                case "SG3_ROW_ADD":

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


                            sg2_dt.Rows.Add(sg2_dr);
                        }

                        sg2_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg2_add_blankrows();

                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                    }
                    #endregion
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
                        showHeadings();
                    }
                    #endregion
                    break;
                case "MRP_MIT":
                    if (col1.Length > 1)
                        fillGrid();
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
            SQuery = "SELECT a.CSSNO as CSS_NO,to_char(A.CSsDT,'dd/mm/yyyy') as CSS_Dt,a.CCODE as Client_Code,a.dir_comp,a.Emodule as Module_Name,a.Eicon as Option_Name,a.Remarks,a.Req_type,a.Iss_type as Issue_Type,a.Cont_name,a.Ent_Dt,last_Action,last_Actdt,a.wrkrmk,a.app_by,a.app_dt,a.Cont_No,a.Cont_Email,to_chaR(a.CSSDT,'YYYYMMDD') as CSS_DTd FROM " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.cssdt " + PrdRange + " order by a.cssno ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            if (hffield.Value == "SAVE")
            {
                if (Request.Cookies["REPLY"].Value == "N") return;

                makePR();
                save_purchPlan();
                fgen.msg("-,", "AMSG", "Saved Successfully");
            }
            else if (hffield.Value == "SF")
            {
                if (Request.Cookies["REPLY"].Value == "N") return;
                genSFReq();
                fgen.msg("-,", "AMSG", "Saved Successfully");
            }
            else
            {
                PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                fillGrid();
            }
        }
    }
    //
    void makePR()
    {
        double pr_qty = 0;
        string vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(ORDNO) AS VCH FROM POMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='60' AND ORDDT " + DateRange + " ", 6, "VCH");
        oDS = new DataSet();
        oporow = null;
        oDS = fgen.fill_schema(frm_qstr, frm_cocd, "POMAS");

        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            if (fgen.make_double(sg1.Rows[i].Cells[1].Text.ToString()) < 0 && sg1.Rows[i].Cells[14].Text.ToString().Length > 3)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = "60";
                oporow["ORDNO"] = vnum;
                oporow["ORDDT"] = vardate;
                oporow["tr_insur"] = "AUTOMRP";
                oporow["PORDNO"] = "THRUMRP";
                oporow["BANK"] = "Production";
                oporow["ACODE"] = "60";
                oporow["prefsource"] = "-";

                oporow["Srno"] = i;
                oporow["icode"] = sg1.Rows[i].Cells[14].Text.Trim();
                oporow["psize"] = "-";
                oporow["app_by"] = "-";
                oporow["app_dt"] = vardate;
                oporow["o_qty"] = 0;
                oporow["qtyord"] = pr_qty;
                oporow["qtysupp"] = 0;

                oporow["qtybal"] = 0;
                oporow["unit"] = "-";
                oporow["desc_"] = "-";
                oporow["pflag"] = 1;
                oporow["delv_term"] = "-";

                oporow["delv_item"] = "-";
                oporow["remark"] = "-";
                oporow["pdisc"] = 0;
                oporow["ptax"] = 0;
                oporow["prate"] = 0;
                oporow["inst"] = 0;
                oporow["pexc"] = 0;
                oporow["PAMT"] = 0;

                oporow["PORDDT"] = vardate;
                oporow["invno"] = "-";
                oporow["invdate"] = vardate;
                oporow["Delivery"] = 0;
                oporow["DEL_MTH"] = 0;
                oporow["DEL_WK"] = 0;

                oporow["del_date"] = vardate;
                oporow["refdate"] = vardate;
                oporow["store_no"] = "SA";
                oporow["Amdtno"] = 0;
                oporow["desp_to"] = "-";

                oporow["packing"] = 0;
                oporow["payment"] = "-";
                oporow["stax"] = "-";
                oporow["EXC"] = "-";
                oporow["iopr"] = "-";
                oporow["pr_no"] = "-";
                oporow["pr_dt"] = vardate;
                oporow["amd_no"] = "-";
                oporow["del_Sch"] = "-";
                oporow["st31no"] = "-";
                oporow["tax"] = "-";
                oporow["wk1"] = 0;
                oporow["wk2"] = 0;
                oporow["wk3"] = 0;
                oporow["wk4"] = 0;
                oporow["ent_by"] = frm_uname;
                oporow["ent_dt"] = vardate;
                oporow["edt_by"] = "-";
                oporow["edt_dt"] = vardate;
                oporow["issue_no"] = 0;

                oporow["amd_no"] = "-";
                oporow["term"] = "-";
                oporow["vend_wt"] = 1;

                oporow["TEST"] = "AUTO";
                oporow["mode_tpt"] = "MRP";
                oporow["cscode1"] = "-";

                oporow["freight"] = "-";
                oporow["DOC_THR"] = "-";

                oDS.Tables[0].Rows.Add(oporow);
            }
        }
        fgen.save_data(frm_qstr, frm_cocd, oDS, "POMAS");
    }
    void save_purchPlan()
    {
        string vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(VCHNUM) AS VCH FROM PURCH_PLAN WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='10' AND VCHDATE " + DateRange + " ", 6, "VCH");
        oDS = new DataSet();
        oporow = null;
        oDS = fgen.fill_schema(frm_qstr, frm_cocd, "PURCH_PLAN");

        for (int i = 0; i < sg1.Rows.Count; i++)
        {

        }
        fgen.save_data(frm_qstr, frm_cocd, oDS, "PURCH_PLAN");
    }
    void genSFReq()
    {
        double pr_qty = 0;
        string vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(ORDNO) AS VCH FROM POMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='60' AND ORDDT " + DateRange + " ", 6, "VCH");
        oDS = new DataSet();
        oporow = null;
        oDS = fgen.fill_schema(frm_qstr, frm_cocd, "POMAS");

        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            if (fgen.make_double(sg1.Rows[i].Cells[1].Text.ToString()) < 0 && sg1.Rows[i].Cells[14].Text.ToString().Length > 3)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = "60";
                oporow["ORDNO"] = vnum;
                oporow["ORDDT"] = vardate;
            }
        }
    }
    //------------------------------------------------------------------------------------
    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        // Hidden Field        
        for (int i = 1; i < colCount; i++)
        {
            sg1_dt.Columns.Add(new DataColumn("sg1_t" + i.ToString(), typeof(string)));
        }
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
        for (int i = 1; i < colCount; i++)
        {
            sg1_dr["sg1_t" + i.ToString()] = "0";
        }

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
            z = 0;
            for (int i = z; i < e.Row.Cells.Count - 1; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.ToolTip = "You can click this cell";
                cell.Attributes["ondblclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}", SelectedGridCellIndex.ClientID, i, Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }

            //e.Row.Attributes["ondblclick"] = ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex);
            e.Row.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex) + ";}";
            //e.Row.ToolTip = "Click to select this row.";

            //sg1.HeaderRow.Cells[0].Style["display"] = "none";
            //e.Row.Cells[0].Style["display"] = "none";

            string fn = "";
            if (frm_formID == "F40999") fn = "MRP_MIT";
            else fn = "do_MC_MGMT";
            switch (fn)
            {
                case "do_MC_MGMT":
                    sg1.HeaderRow.Cells[1].Text = "Srno";
                    sg1.HeaderRow.Cells[2].Text = "Dated";
                    sg1.HeaderRow.Cells[3].Text = "Installed<br/>Capacity(Hrs)";
                    sg1.HeaderRow.Cells[4].Text = "Holiday/<br/>Festival(Hrs)";
                    sg1.HeaderRow.Cells[5].Text = "Available<br/>Time(Hrs)";
                    sg1.HeaderRow.Cells[6].Text = "Planned<br/>Shutdown(Hrs)";
                    sg1.HeaderRow.Cells[7].Text = "Net Available<br/>Time(Hrs)";
                    sg1.HeaderRow.Cells[8].Text = "PPC Planned<br/>Time(Hrs)";
                    sg1.HeaderRow.Cells[9].Text = "No Business<br/>(Hrs)";
                    sg1.HeaderRow.Cells[10].Text = "Un-Planned<br/>Shutdown(Hrs)";
                    sg1.HeaderRow.Cells[11].Text = "Possbile Operating<br/>Time(Hrs)";
                    sg1.HeaderRow.Cells[12].Text = "Actual<br/>Operating(Hrs)";
                    sg1.HeaderRow.Cells[13].Text = "Operating Hrly<br/>Ratio";
                    sg1.HeaderRow.Cells[14].Text = "UnPlanned D/Time<br/>Ratio";
                    sg1.HeaderRow.Cells[15].Text = "Prodn<br/>Effciency";
                    sg1.HeaderRow.Cells[16].Text = "Quality<br/>Effciency";
                    sg1.HeaderRow.Cells[17].Text = "Prodn KG<br/>(Achieved)";
                    sg1.HeaderRow.Cells[18].Text = "Sales KG<br/>(Achieved)";
                    sg1.HeaderRow.Cells[19].Text = "Sales Plan<br/>Reqd(Hrs)";

                    for (int i = 19; i < colCount; i++)
                    {
                        sg1.HeaderRow.Cells[i].Style["display"] = "none";
                        e.Row.Cells[i].Style["display"] = "none";
                    }

                    for (int x = 0; x < e.Row.Cells.Count; x++)
                    {
                        double d1 = 0;
                        bool chkNum = double.TryParse(e.Row.Cells[x].Text.Trim(), out d1);
                        if (chkNum)
                        {
                            e.Row.Cells[x].HorizontalAlign = HorizontalAlign.Right;
                            if (e.Row.Cells[x].Text == "0") e.Row.Cells[x].Text = "";
                        }
                        if (e.Row.Cells[x].Text.Trim().Contains("/")) e.Row.Cells[x].HorizontalAlign = HorizontalAlign.Center;
                    }
                    break;
                case "MRP_MIT*":
                    
                    break;
            }
        }
    }

    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg2.Rows[rowIndex].RowIndex);

        switch (var)
        {
            case "SG2_RMV":

                break;
            case "SG2_ROW_ADD":

                break;
        }
    }

    //------------------------------------------------------------------------------------
    protected void sg3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg3.Rows[rowIndex].RowIndex);


        switch (var)
        {
            case "SG3_RMV":

                break;
            case "SG3_ROW_ADD":

                break;
        }
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
    { }
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
    { }
    void Acode_Sel_query()
    {

    }
    void Icode_Sel_query()
    {

    }

    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F60101":
                SQuery = "SELECT 'CS' AS FSTR,'Support Request Logging' as NAME,'CS' AS CODE FROM dual";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CS");
                break;

        }
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
        }
    }
    protected void btnAtt_Click(object sender, EventArgs e)
    { }

    protected void btnView1_Click(object sender, ImageClickEventArgs e)
    { }
    protected void btnDwnld1_Click(object sender, ImageClickEventArgs e)
    { }
    protected void btnCocd_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "USR";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Company Code", frm_qstr);
    }
    protected void btnPersonName_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PERSON";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Person Name", frm_qstr);
    }
    void setGridWidth(GridView gName)
    {
        if (gName.Rows.Count > 0)
        {
            int col_count = gName.HeaderRow.Cells.Count;
            double wid = 500;
            for (int i = 0; i < col_count; i++)
            {
                wid += fgen.make_double(sg1.Columns[0].ItemStyle.Width.Value + 5, 0);
            }

            try { gName.Width = Convert.ToUInt16(wid + 100); }
            catch { gName.Width = 2000; }
        }
    }
    protected void sg1_SelectedIndexChanged(object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow row = sg1.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        mq0 = sg1.HeaderRow.Cells[selectedCellIndex].Text.Replace("<br/>", " "); // dynamic heading        
        if (selectedCellIndex > 0) selectedCellIndex -= 1;
        SQuery1 = "";
        string fn = "";
        if (frm_formID == "F40999") fn = "MRP_MIT";
        else frm_formID = "do_MC_MGMT";
        switch (fn)
        {
            case "do_MC_MGMT":
                if (selectedCellIndex == 3)
                {
                    SQuery = "Select a.icode as Mch_code,a.col1 as Mch_name,a.col3 as Offline_On,a.qty1 as Offline_hrs,a.col5 as Reason_Stated,a.vchdate as doc_no,a.vchnum as doc_dt from multivch a where a.branchcd='" + frm_mbr + "' and a.type='HF' and a.vchdate " + PrdRange + " and trim(col3)='" + row.Cells[2].Text.ToString().Trim() + "' order by a.vchdate,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + mq0, frm_qstr);
                }
                if (selectedCellIndex == 5)
                {
                    SQuery = "Select a.icode as Mch_code,a.col1 as Mch_name,a.col3 as Offline_On,a.qty1 as Offline_hrs,a.col5 as Reason_Stated,a.vchdate as doc_no,a.vchnum as doc_dt from multivch a where a.branchcd='" + frm_mbr + "' and a.type='OF' and a.vchdate " + PrdRange + " and trim(col3)='" + row.Cells[2].Text.ToString().Trim() + "' order by a.vchdate,a.vchnum,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + mq0, frm_qstr);
                }
                if (selectedCellIndex == 7)
                {
                    SQuery = "Select b.Iname,b.cpartno,a.ename,to_char(a.vchdate,'dd/mm/yyyy') as doc_date,sum(a.iqtyout) as Total_qty,round(sum(a.bcd)/60,0) as Total_hrs from prod_sheet a,item b where trim(A.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='20' and to_chaR(a.vchdate,'dd/mm/yyyy')='" + row.Cells[2].Text.ToString().Trim() + "' group by a.ename,b.Iname,b.cpartno,to_char(a.vchdate,'dd/mm/yyyy') order by b.iname";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + mq0, frm_qstr);
                }
                if (selectedCellIndex == 8)
                {
                    SQuery = "Select a.ename as Machine,round((sum(nvl(a.num1,0)+nvl(a.num2,0)+nvl(a.num3,0)+nvl(a.num4,0)+nvl(a.num5,0)+nvl(a.num6,0)+nvl(a.num7,0)+nvl(a.num8,0)+nvl(a.num9,0)+nvl(a.num10,0)+nvl(a.num11,0)+nvl(a.num12,0)))/60,2) as Down_Time_Hrs,round((sum(nvl(a.num1,0)))/60,2) AS RS1,round((sum(nvl(a.num2,0)))/60,2) AS RS2,round((sum(nvl(a.num3,0)))/60,2) AS RS3,round((sum(nvl(a.num4,0)))/60,2) AS RS4,round((sum(nvl(a.num5,0)))/60,2) AS RS5,round((sum(nvl(a.num6,0)))/60,2) AS RS6,round((sum(nvl(a.num7,0)))/60,2) AS RS7,round((sum(nvl(a.num8,0)))/60,2) AS RS8, round((sum(nvl(a.num9,0)))/60,2) AS RS9,round((sum(nvl(a.num10,0)))/60,2) AS RS10,round((sum(nvl(a.num11,0)))/60,2) AS RS11,round((sum(nvl(a.num12,0)))/60,2) AS RS12 from prod_sheet a where a.branchcd='" + frm_mbr + "' and a.type='90' and to_char(a.vchdate,'dd/mm/yyyy')='" + row.Cells[2].Text.ToString().Trim() + "' group by a.ename having round((sum(nvl(a.num1,0)+nvl(a.num2,0)+nvl(a.num3,0)+nvl(a.num4,0)+nvl(a.num5,0)+nvl(a.num6,0)+nvl(a.num7,0)+nvl(a.num8,0)+nvl(a.num9,0)+nvl(a.num10,0)+nvl(a.num11,0)+nvl(a.num12,0)))/60,2)>0 order by Down_Time_Hrs desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("Down Time Analysis Machine Wise for " + row.Cells[1].Text.ToString().Trim(), frm_qstr);
                }
                break;
        }
    }
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["ondblclick"] = ClientScript.GetPostBackClientHyperlink(sg2, "Select$" + e.Row.RowIndex);
            e.Row.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + ClientScript.GetPostBackClientHyperlink(sg2, "Select$" + e.Row.RowIndex) + ";}";
            e.Row.ToolTip = "Click to select this row.";

            sg2.HeaderRow.Cells[0].Style["display"] = "none";
            sg2.HeaderRow.Cells[1].Style["display"] = "none";

            e.Row.Cells[0].Style["display"] = "none";
            e.Row.Cells[1].Style["display"] = "none";
        }
    }
    protected void sg3_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void sg1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowIndex == 0) e.Row.TabIndex = 1;
            else e.Row.TabIndex = 2;
            if (Convert.ToDouble(e.Row.RowIndex.ToString()) == 0) e.Row.Attributes["onfocus"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "if (event.keyCode != 13) { javascript:return SelectSibling(event); }";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }
    protected void sg2_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = sg2.SelectedRow;
        SQuery2 = "";
        SQuery3 = "";
        switch (frm_formID)
        {
            case "F50101":
                SQuery2 = "SELECT DISTINCT A.VCHNUM AS ge_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS GE_dT,B.ANAME AS CUSTOMER,A.ACODE,TO_CHAR(a.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHERP A ,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='00' AND A.VCHDATE " + DateRange + "  ORDER BY VDD DESC, A.VCHNUM DESC";
                break;
        }

        if (SQuery2.Length > 0)
        {
            sg3_dt = new DataTable();
            sg3_dt = fgen.getdata(frm_qstr, frm_cocd, SQuery2);

            sg3.DataSource = sg3_dt;
            sg3.DataBind();
        }
        if (SQuery3.Length > 0)
        {
            sg4_dt = new DataTable();
            sg4_dt = fgen.getdata(frm_qstr, frm_cocd, SQuery3);

            sg4.DataSource = sg4_dt;
            sg4.DataBind();
        }
    }
    void cyclicBomRptLevel()
    {
        Session["send_dt"] = dt;
        fgen.Fn_open_rptlevel("Cyclical Bom With   (parent -> child -> parent", frm_qstr);
    }

    void MRP_W_MIT()
    {
        SQuery = "select icode,count(*) as cnt from (select wk_ref||trim(icode) as icode from  sl_plan WHERE BRANCHCD='" + frm_mbr + "' AND type='SL'  and trim(to_Char(wk_Ref,'999999')) in (" + col3 + ") and upper(trim(isarch))<>'Y'  GROUP BY wk_ref||trim(icode)) group by icode having count(*)>1 ";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            Session["send_dt"] = dt;
            fgen.Fn_open_rptlevel("Item With Repeat Planning , Cannot Proceed", frm_qstr);
            return;
        }

        SQuery = "select 'Total Weeks being planned' as Mtitle,count(*) as cnt,'MAX 12 Weeks at a Time' as Result from (select distinct wk_ref as icode from  sl_plan WHERE BRANCHCD='" + frm_mbr + "' AND type='SL'  and trim(to_Char(wk_Ref,'999999')) in(" + col3 + ") and upper(trim(isarch))<>'Y' ) having count(*)>12  ";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            Session["send_dt"] = dt;
            fgen.Fn_open_rptlevel("Please Plan upto 12 Weeks Sales Plan", frm_qstr);
            return;
        }
        string br_Str = "branchcd='" + frm_mbr + "'";

        xprd1 = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1";
        xprd2 = " between to_date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')";

        mq0 = "select b.iname,trim(a.icode) as icode,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,sum(a.wip) as wipqty,sum(a.imin) As imin from (Select icode, yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos,0 as wip,nvl(imin,0) As imin from itembal where " + br_Str + " union all Select erp_Code,0 as opening,0 as cdr,0 as ccr,0 as clos,closing,0 as imin from wipTOTlstk_" + frm_mbr + "  union all ";
        mq1 = "select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as wip,0 as imin from ivoucher where " + br_Str + " and type like '%' and vchdate " + xprd1 + " and store='Y' GROUP BY ICODE union all ";
        mq2 = "select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as wip,0 as imin from ivoucher where " + br_Str + " and type like '%' and vchdate " + xprd2 + " and store='Y' GROUP BY ICODE )a,item b where trim(A.icode)=trim(B.icodE) group by b.iname,trim(a.icode) having sum(a.opening)+sum(a.cdr)-sum(a.ccr)+sum(a.wip)+sum(a.imin)<>0 order by trim(A.icode)";
        SQuery = mq0 + mq1 + mq2;
        DataTable dtStk = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        SQuery = "select trim(erp_Code) As ICODE,sum(closing) as totalw from (Select erp_Code,closing from wipTOTlstk_" + frm_mbr + " ) group by trim(erp_Code) order by trim(erp_Code) ";
        DataTable dtStkW = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        mrs = new DataTable();
        mrs.Columns.Add("dated", typeof(string));
        mrs.Columns.Add("icode", typeof(string));
        mrs.Columns.Add("Parent", typeof(string));
        mrs.Columns.Add("icodeday", typeof(string));
        mrs.Columns.Add("REQD", typeof(string));

        DataTable mrs1 = new DataTable();
        mrs1.Columns.Add("icode", typeof(string));
        mrs1.Columns.Add("closing", typeof(double));
        mrs1.Columns.Add("used", typeof(double));
        mrs1.Columns.Add("balance", typeof(double));
        DataRow mrsdr = null;
        foreach (DataRow drStk in dtStk.Rows)
        {
            mrsdr = mrs1.NewRow();
            mrsdr["icode"] = drStk["icode"];
            mrsdr["closing"] = fgen.make_double(drStk["closing"].ToString()) + fgen.make_double(drStk["wipqty"].ToString());
            mrsdr["used"] = 0;
            mrsdr["balance"] = fgen.make_double(drStk["closing"].ToString()) + fgen.make_double(drStk["wipqty"].ToString());
            mrs1.Rows.Add(mrsdr);
        }

        SQuery = "select wk_ref as dated, nvl(num01,0) as num01, trim(icode) as icode,cur_Stk,sale_qty,trim(icode)||trim(sordno)||to_Char(sorddt,'dd/mm/yyyy') as fstr from  sl_plan WHERE BRANCHCD='" + frm_mbr + "' AND type='SL'  and trim(to_Char(wk_Ref,'999999')) in (" + col3 + ") and upper(trim(isarch))<>'Y' order by trim(icode),wk_REf";
        DataTable planDt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (planDt.Rows.Count <= 0) return;
        foreach (DataRow planDr in planDt.Rows)
        {
            planDr["cur_Stk"] = "0";
            planDr["sale_Qty"] = "0";
        }
        double mplan_qty = 0;
        foreach (DataRow planDr in planDt.Rows)
        {
            if (mrs1.Rows.Count > 0)
            {
                DataView dv = new DataView(mrs1, "ICODE='" + planDr["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                foreach (DataRow mrsdr1 in mrs1.Rows)
                {
                    if (mrsdr1["icode"].ToString().Trim() == planDr["icode"].ToString().Trim())
                    {
                        mplan_qty = fgen.make_double(planDr["num01"].ToString());
                        if (fgen.make_double(mrsdr1["balance"].ToString()) < mplan_qty)
                        {
                            if (mplan_qty > 0 && fgen.make_double(planDr["num01"].ToString()) > 0)
                            {
                                mrsdr1["used"] = mrsdr1["balance"];
                                mrsdr1["balance"] = 0;
                                mplan_qty -= fgen.make_double(mrsdr1["balance"].ToString());
                                planDr["cur_stk"] = mrsdr1["used"];
                            }
                        }
                        else
                        {
                            mrsdr1["used"] = fgen.make_double(mrsdr1["used"].ToString()) + mplan_qty;
                            mrsdr1["balance"] = fgen.make_double(mrsdr1["closing"].ToString()) - fgen.make_double(mrsdr1["used"].ToString());
                            planDr["cur_stk"] = mplan_qty;
                        }
                    }
                }
            }
        }

        //
        dt2 = new DataTable();
        SQuery = "select a.wk_ref as dated, nvl(a.num01,0) as plan_qty,a.cur_Stk as curr_stk,nvl(a.num01,0) as Net_reqd,b.iname,trim(a.icode) as icode,a.sale_Qty from  sl_plan a,item b WHERE trim(A.icode)=trim(B.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.type='SL'  and trim(to_Char(a.wk_Ref,'999999')) in (" + col3 + ") and upper(trim(a.isarch))<>'Y'  order by trim(a.icode),a.wk_REf";
        SQuery = "select a.wk_ref as dated, nvl(a.num01,0) as plan_qty,a.cur_Stk as curr_stk,nvl(a.num01,0)-a.cur_Stk as Net_reqd,b.iname,trim(a.icode) as icode,a.sale_Qty from  sl_plan a,item b WHERE trim(A.icode)=trim(B.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.type='SL'  and trim(to_Char(a.wk_Ref,'999999')) in (" + col3 + ") and upper(trim(a.isarch))<>'Y' and nvl(a.num01,0)-a.cur_Stk>0  order by trim(a.icode),a.wk_REf";

        SQuery = "SELECT * FROM (" + SQuery + ") ";

        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        SQuery = "SELECT distinct a.icode,a.IBCODE,a.IBQTY,b.iname FROM ITEMOSP a, item b WHERE a.BRANCHCD!='DD' AND a.TYPE='BM' and trim(a.ibcode)=trim(B.icode) ORDER BY a.icode,a.ibcode ";
        bomDt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        SQuery = "SELECT distinct trim(icode) as icode FROM ITEMOSP WHERE BRANCHCD!='DD' AND TYPE='BM' order by icode";
        bomChildDt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        /*
        SQuery = "select trim(ordno) as ordno,orddt,trim(icode) as icode,sum(qtyord)-sum(pomade) as pobal from (select ordno,orddt,icode,qtyord,0 as pomade from pomas where branchcd='" + frm_mbr + "' and type='60' and orddt " + PrdRange + " and trim(nvl(tr_insur,'-')) = 'AUTOMRP' union all select pr_no,pr_Dt,icode,0 as qtyord,qtyord as pomade from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt " + PrdRange + ") group by trim(ordno),orddt,trim(icode) having sum(qtyord)-sum(pomade)>0 and sum(pomade)=0";
        DataTable ins_pend = fgen.getdata(frm_qstr, frm_cocd, SQuery);
         */

        SQuery = "select trim(icode) as icode,sum(iqty_chl) as tot from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + DateRange + " and inspected='N' and store!='R' group by trim(icode) order by trim(icode)";
        DataTable ins_pend = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        SQuery = "select icode,sum(balpr) as Balpr from (select max(pflag)as pflag,ordno,orddt,trim(icode) as icode,sum(prq)-sum(poq) as Balpr from (Select pflag,ordno,orddt,icode,qtyord as prq,0 as poq from pomas where branchcd='" + frm_mbr + "' and type='60' and orddt>=to_DatE('" + frm_CDT1 + "','dd/mm/yyyy') union all Select null as pflag,pr_no,pr_dt,icode,0 as prq,qtyord as poq from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt>=to_DatE('" + frm_CDT1 + "','dd/mm/yyyy') and substr(term,1,2) not like '%CANCELLED%') group by ordno,orddt,trim(icode) having sum(prq)-sum(poq)>0 and max(pflag)<>0) group by icode order by icode";
        DataTable pr_pend = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        SQuery = "select icode,sum(Balpo) as Balpo from (select max(pflag)as pflag,ordno,orddt,trim(icode) as icode,sum(poq)-sum(rcvq) as Balpo from (Select pflag,ordno,orddt,acode,icode,qtyord as poq,0 as rcvq from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt>=to_DatE('01/04/2017','dd/mm/yyyy') union all Select null as pflag,ponum,podate,acode,icode,0 as prq,iqtyin as poq from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and potype like '5%' and vchdate>=to_DatE('01/04/2017','dd/mm/yyyy') and store in ('Y','N') ) group by ordno,orddt,trim(AcodE),trim(icode) having sum(poq)-sum(rcvq)>0 and max(pflag)<>1) group by icode order by icode";
        DataTable po_pend = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        SQuery = "select '-' as icode,0 as Balpo from dual";
        SQuery = "select icode,sum(Balpo) as Balpo from (select trim(icode) as icode,sum(poq)-sum(rcvq) as Balpo from (Select acode,icode,total as poq,0 as rcvq from schedule where branchcd='" + frm_mbr + "' and type like '66%' and vchdate " + DateRange + " union all Select acode,icode,0 as prq,iqtyin as poq from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and potype like '5%' and vchdate " + DateRange + " and store='Y' ) group by trim(AcodE),trim(icode) having sum(poq)-sum(rcvq)>0 ) group by icode order by icode";
        DataTable po_pend_Sch = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        SQuery = "Select substr(b.col4,7,4)||to_char(to_date(trim(b.col4),'DD/MM/YYYY'),'WW') as yr_wk, substr(b.col4,7,4)||substr(b.col4,4,2) as yrmth, trim(b.col4) as etadt,trim(a.icode) AS ERP_Code,sum(a.qty1) as Qtot from multivch a, multivch b where a.branchcd='" + frm_mbr + "' and a.type='MC' and trim(a.mrsrno)<>'Y' and b.branchcd='" + frm_mbr + "' and b.type='XT' and b.result='001' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.icode)||trim(b.col1) and length(trim(b.col4))=10 group by substr(b.col4,7,4)||to_char(to_date(trim(b.col4),'DD/MM/YYYY'),'WW'),trim(b.col4),trim(a.icode),substr(b.col4,7,4)||substr(b.col4,4,2) order by trim(a.icode),yr_wk";
        DataTable rsmit = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        foreach (DataRow dr2 in dt2.Rows)
        {
            getBOMChild(dr2["icode"].ToString().Trim(), fgen.make_double(dr2["net_reqd"].ToString().Trim()), dr2["dated"].ToString());
        }

        DataTable dtItList = fgen.getdata(frm_qstr, frm_cocd, "Select trim(icode) as icode,nvl(iname,'-') as iname,nvl(unit,'-') as unit,nvl(cpartno,'-') as cpartno,cdrgno,nvl(icat,'-') as icat,nvl(lead_time,0) as lead_times,nvl(PUR_LOT_SZ,0) as PUR_LOT_SZ from item where length(Trim(icode))>4 order by trim(icode)");

        create_tab();

        DataView mrsViewDT = new DataView(mrs, "", "DATED", DataViewRowState.CurrentRows);
        distmrsDt = mrsViewDT.ToTable(true, "DATED");
        ViewState["distmrsDt"] = distmrsDt;
        int sgc = 24;
        int sgc2 = 36;
        int sgc3 = 48;
        foreach (DataRow disrmrDr in distmrsDt.Rows)
        {
            sg1.HeaderRow.Cells[sgc].Text = "R" + disrmrDr["DATED"].ToString().Trim();
            sg1.HeaderRow.Cells[sgc2].Text = "M" + disrmrDr["DATED"].ToString().Trim();
            sg1.HeaderRow.Cells[sgc3].Text = "S" + disrmrDr["DATED"].ToString().Trim();
            sgc++;
            sgc2++;
            sgc3++;
        }

        double totrq = 0;
        DataView mrsView = new DataView(mrs, "", "ICODE", DataViewRowState.CurrentRows);
        DataTable distmrs = mrsView.ToTable(true, "ICODE");
        for (int i = 0; i < distmrs.Rows.Count; i++)
        {
            string mCode = distmrs.Rows[i]["icode"].ToString().Trim();
            sg1_dr = sg1_dt.NewRow();
            sg1_dr[12] = (i + 1);
            sg1_dr[13] = mCode;
            if (dtItList.Rows.Count > 0)
            {
                DataView dv = new DataView(dtItList, "icode='" + mCode + "'", "", DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                {
                    sg1_dr[3] = dv[0]["cpartno"].ToString();
                    sg1_dr[10] = dv[0]["lead_times"].ToString();
                    sg1_dr[9] = dv[0]["PUR_LOT_SZ"].ToString();
                    sg1_dr[14] = dv[0]["iname"].ToString();
                    sg1_dr[15] = dv[0]["icat"].ToString();
                    sg1_dr[16] = dv[0]["unit"].ToString();
                }
            }
            if (dtStk.Rows.Count > 0)
            {
                DataView dv = new DataView(dtStk, "ICODE='" + mCode + "'", "", DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                {
                    sg1_dr[11] = dv[0]["imin"].ToString();
                    sg1_dr[4] = dv[0]["closing"].ToString();
                    sg1_dr[18] = dv[0]["closing"].ToString();
                }
            }
            if (ins_pend.Rows.Count > 0)
            {
                // pending inspection
                sg1_dr[21] = fgen.seek_iname_dt(ins_pend, "ICODE='" + mCode + "'", "tot");
            }
            if (dtStkW.Rows.Count > 0)
            {
                DataView dv = new DataView(dtStkW, "ICODE='" + mCode + "'", "", DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                {
                    sg1_dr[4] = dv[0]["totalw"].ToString();
                    sg1_dr[19] = dv[0]["totalw"].ToString();
                }
            }

            sg1_dr[20] = fgen.make_double(sg1_dr[18].ToString()) + fgen.make_double(sg1_dr[19].ToString());
            sg1_dr[7] = sg1_dr[14];
            sg1_dr[8] = sg1_dr[16];

            if (mrs.Rows.Count > 0)
            {
                DataView dv = new DataView(mrs, "icode='" + mCode + "'", "", DataViewRowState.CurrentRows);
                for (int l = 0; l < dv.Count; l++)
                {
                    for (int w = 20; w < 36; w++)
                    {
                        if (sg1.HeaderRow.Cells[w].Text.Trim().ToUpper() == "R" + dv[l].Row["dated"].ToString().Trim())
                            sg1_dr[w - 1] = sg1_dr[w - 1].ToString().Trim().toDouble() + dv[l].Row["reqd"].ToString().toDouble();
                    }
                    totrq += fgen.make_double(dv[l].Row["reqd"].ToString());
                }
            }
            sg1_dr[20] = totrq;
            sg1_dr[1] = totrq;
            sg1_dr[0] = sg1_dr[4].ToString().toDouble() - sg1_dr[1].ToString().toDouble();
            if (rsmit.Rows.Count > 0)
            {
                int newIndex = 0;
                DataView dv = new DataView(rsmit, "erp_code='" + mCode + "'", "", DataViewRowState.CurrentRows);
                for (int l = 0; l < dv.Count; l++)
                {
                    for (int p = 36; p < 48; p++)
                    {
                        if (sg1.HeaderRow.Cells[p].Text.Trim().ToUpper() == "M" + dv[l].Row["yr_wk"].ToString().Trim())
                            newIndex = p;
                    }
                    if (newIndex > 0)
                    {
                        sg1_dr[newIndex - 1] = sg1_dr[newIndex - 1].ToString().Trim().toDouble() + dv[l].Row["qtot"].ToString().Trim().toDouble();
                    }
                }
            }

            if (pr_pend.Rows.Count > 0)
            {
                DataView dv = new DataView(pr_pend, "ICODE='" + mCode + "'", "", DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                {
                    sg1_dr[22] = dv[0].Row["balpr"].ToString().Trim();
                }
            }
            if (po_pend.Rows.Count > 0)
            {
                DataView dv = new DataView(po_pend, "ICODE='" + mCode + "'", "", DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                {
                    sg1_dr[60] = dv[0].Row["balpo"].ToString().Trim();
                }
            }
            if (po_pend_Sch.Rows.Count > 0)
            {
                DataView dv = new DataView(po_pend_Sch, "ICODE='" + mCode + "'", "", DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                {
                    sg1_dr[61] = fgen.make_double(sg1_dr[52].ToString()) + fgen.make_double(dv[0].Row["balpo"].ToString().Trim());
                }
            }

            int reqcol = 25;
            int mit_co = 36;
            sg1_dr[47] = sg1_dr[18].ToString().Trim().toDouble() + sg1_dr[19].ToString().Trim().toDouble() - sg1_dr[23].ToString().Trim().toDouble();
            for (int o = 49; o <= 60; o++)
            {
                sg1_dr[o - 1] = sg1_dr[o - 2].ToString().Trim().toDouble() + sg1_dr[mit_co - 1].ToString().Trim().toDouble() - sg1_dr[reqcol - 1].ToString().Trim().toDouble();
                reqcol++;
                mit_co++;
            }

            sg1_dr[62] = sg1_dr[60].ToString().Trim().toDouble() + sg1_dr[58].ToString().Trim().toDouble();
            sg1_dr[63] = "";

            sg1_dt.Rows.Add(sg1_dr);
        }
        DataView sort_view = new DataView();
        sort_view = sg1_dt.DefaultView;
        sort_view.Sort = "sg1_t13";
        sg1_dt = new DataTable();
        sg1_dt = sort_view.ToTable(true);
        sort_view.Dispose();

        #region po pending (but no plan)

        sg1_dr = sg1_dt.NewRow();
        sg1_dr[13] = "";
        sg1_dt.Rows.Add(sg1_dr);

        sg1_dr = sg1_dt.NewRow();
        sg1_dr[13] = "Po Pending (But No Plan)";
        sg1_dt.Rows.Add(sg1_dr);

        int srno = 0;
        int totcount = po_pend.Rows.Count;
        if (totcount > 100) totcount = 100;
        for (int pp = 0; pp < totcount; pp++)
        {
            string mCode = po_pend.Rows[pp]["ICODE"].ToString().Trim();
            string showRow = "Y";
            if (distmrs.Rows.Count > 0)
            {
                //DataView dv = new DataView(distmrs, "ICODE='" + mCode + "'", "", DataViewRowState.CurrentRows);
                //if (dv.Count > 0) showRow = "Y";
            }
            if (showRow == "Y")
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr[12] = (srno + 1);
                sg1_dr[13] = mCode;
                if (dtItList.Rows.Count > 0)
                {
                    DataView dv = new DataView(dtItList, "ICODE='" + mCode + "'", "ICODE", DataViewRowState.CurrentRows);
                    if (dv.Count > 0)
                    {
                        for (int i = 0; i < dv.Count; i++)
                        {
                            sg1_dr[7] = dv[i]["INAME"].ToString().Trim();
                            sg1_dr[10] = dv[i]["lead_times"].ToString().Trim();
                            sg1_dr[9] = dv[i]["PUR_LOT_SZ"].ToString().Trim();
                            sg1_dr[14] = dv[i]["INAME"].ToString().Trim();
                            sg1_dr[15] = dv[i]["ICAT"].ToString().Trim();
                            sg1_dr[16] = dv[i]["UNIT"].ToString().Trim();
                        }
                    }
                }
                if (dtStk.Rows.Count > 0)
                {
                    DataView dv = new DataView(dtStk, "ICODE='" + mCode + "'", "ICODE", DataViewRowState.CurrentRows);
                    if (dv.Count > 0)
                    {
                        for (int i = 0; i < dv.Count; i++)
                        {
                            sg1_dr[11] = dv[0]["imin"].ToString();
                            sg1_dr[4] = dv[0]["closing"].ToString();
                            sg1_dr[18] = dv[0]["closing"].ToString();
                        }
                    }
                }
                if (ins_pend.Rows.Count > 0)
                {
                    DataView dv = new DataView(ins_pend, "ICODE='" + mCode + "'", "ICODE", DataViewRowState.CurrentRows);
                    if (dv.Count > 0)
                    {
                        for (int i = 0; i < dv.Count; i++)
                        {
                            sg1_dr[21] = dv[0]["tot"].ToString();
                        }
                    }
                }
                if (dtStkW.Rows.Count > 0)
                {
                    DataView dv = new DataView(dtStkW, "ICODE='" + mCode + "'", "", DataViewRowState.CurrentRows);
                    if (dv.Count > 0)
                    {
                        sg1_dr[4] = dv[0]["totalw"].ToString();
                        sg1_dr[19] = dv[0]["totalw"].ToString();
                    }
                }

                int reqcol = 25;
                int mit_co = 36;
                sg1_dr[47] = sg1_dr[18].ToString().Trim().toDouble() + sg1_dr[19].ToString().Trim().toDouble() - sg1_dr[23].ToString().Trim().toDouble();
                for (int o = 49; o < 60; o++)
                {
                    sg1_dr[o - 1] = sg1_dr[o - 2].ToString().Trim().toDouble() + sg1_dr[mit_co - 1].ToString().Trim().toDouble() - sg1_dr[reqcol - 1].ToString().Trim().toDouble();
                    reqcol++;
                    mit_co++;
                }

                sg1_dr[0] = sg1_dr[4].ToString().toDouble() - sg1_dr[1].ToString().toDouble();
                sg1_dr[63] = sg1_dr[61].ToString().Trim().toDouble() + sg1_dr[59].ToString().Trim().toDouble();

                sg1_dt.Rows.Add(sg1_dr);
                srno++;
            }
        }
        #endregion
        ViewState["pindex"] = 1;
        ViewState["sg1"] = sg1_dt;

        sg1.DataSource = sg1_dt;
        sg1.DataBind();

        gridWidth();
        showHeadings();
    }
    void showHeadings()
    {
        switch (frm_formID)
        {
            case "F40999":
                #region gridview headings
                sg1.HeaderRow.Cells[1].Text = "Diff";
                sg1.HeaderRow.Cells[2].Text = "Reqd";
                sg1.HeaderRow.Cells[3].Text = "Partno";
                sg1.HeaderRow.Cells[4].Text = "WIP";
                sg1.HeaderRow.Cells[5].Text = "Store";
                sg1.HeaderRow.Cells[6].Text = "PR:Qty";
                sg1.HeaderRow.Cells[7].Text = "Short Qty";
                sg1.HeaderRow.Cells[8].Text = "Item";
                sg1.HeaderRow.Cells[9].Text = "Unit";
                sg1.HeaderRow.Cells[10].Text = "Lead-Time";
                sg1.HeaderRow.Cells[11].Text = "MOQ";
                sg1.HeaderRow.Cells[12].Text = "Min.Lvl";

                for (int i = 1; i < 13; i++)
                {
                    //sg1.HeaderRow.Cells[i].Style["display"] = "none";
                    //e.Row.Cells[i].Style["display"] = "none";
                }

                sg1.HeaderRow.Cells[13].Text = "srno";
                sg1.HeaderRow.Cells[14].Text = "code";
                sg1.HeaderRow.Cells[15].Text = "item";
                sg1.HeaderRow.Cells[16].Text = "Category";
                sg1.HeaderRow.Cells[17].Text = "Unit";
                sg1.HeaderRow.Cells[18].Text = "Total";

                sg1.HeaderRow.Cells[19].Text = "Store STK";
                sg1.HeaderRow.Cells[20].Text = "WIP STK";
                sg1.HeaderRow.Cells[21].Text = "Tot STK";

                sg1.HeaderRow.Cells[22].Text = "Pend_Insp";
                sg1.HeaderRow.Cells[23].Text = "Pend_P.R.";

                for (int i = 24; i < 36; i++)
                {
                    sg1.HeaderRow.Cells[i].Text = "Reqd";
                }
                for (int i = 36; i < 48; i++)
                {
                    sg1.HeaderRow.Cells[i].Text = "MIT";
                }
                for (int i = 48; i <= 60; i++)
                {
                    sg1.HeaderRow.Cells[i].Text = "Shortg";
                }

                sg1.HeaderRow.Cells[61].Text = "PO_Pend";
                sg1.HeaderRow.Cells[62].Text = "PO_Pend_Sup";
                sg1.HeaderRow.Cells[63].Text = "Ext.Matl Stat";
                sg1.HeaderRow.Cells[64].Text = "Covg 1";
                sg1.HeaderRow.Cells[65].Text = "Covg 2";
                sg1.HeaderRow.Cells[66].Text = "Covg 3";
                sg1.HeaderRow.Cells[67].Text = "PRQTY";

                for (int K = 0; K < sg1.Rows.Count; K++)
                {
                    for (int i = 68; i < sg1.Columns.Count; i++)
                    {
                        sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                        sg1.Rows[K].Cells[i].CssClass = "hidden";
                    }
                }

                int sgc = 24;
                int sgc2 = 36;
                int sgc3 = 48;
                if (ViewState["distmrsDt"] != null)
                {
                    distmrsDt = (DataTable)ViewState["distmrsDt"];
                    if (distmrsDt != null)
                    {
                        foreach (DataRow disrmrDr in distmrsDt.Rows)
                        {
                            sg1.HeaderRow.Cells[sgc].Text = "R" + disrmrDr["DATED"].ToString().Trim() + "  Reqd";
                            sg1.HeaderRow.Cells[sgc2].Text = "M" + disrmrDr["DATED"].ToString().Trim() + "  MIT";
                            sg1.HeaderRow.Cells[sgc3].Text = "S" + disrmrDr["DATED"].ToString().Trim() + "  Shortg";
                            sgc++;
                            sgc2++;
                            sgc3++;
                        }
                    }
                }
                for (int i = 0; i < sg1.Rows.Count; i++)
                {
                    if (sg1.Rows[i].Cells[1].Text.ToString().toDouble() > 0) sg1.Rows[i].Cells[1].BackColor = Color.LightGreen;
                    else sg1.Rows[i].Cells[1].BackColor = Color.LightPink;

                    for (int o = 48; o < 60; o++)
                    {
                        if (sg1.Rows[i].Cells[o].Text.ToString().toDouble() < 0)
                            sg1.Rows[i].Cells[o].BackColor = Color.LightPink;
                    }

                    for (int x = 2; x < 10; x++)
                    {
                        sg1.Columns[x].HeaderStyle.CssClass = "hidden";
                        sg1.Rows[i].Cells[x].CssClass = "hidden";
                    }
                }
                #endregion
                break;
        }
    }
    void nextPageIndex()
    {
        switch (frm_formID)
        {
            case "F40999":
                sg1_dt = (DataTable)ViewState["sg1"];

                sg1.DataSource = sg1_dt;
                sg1.DataBind();

                showHeadings();
                gridWidth();
                break;
        }

    }
    void getBOMChild(string _icode, double _bmQty, string _day)
    {
        string hasChild = "Y";
        col1 = fgen.seek_iname_dt(bomChildDt, "ICODE='" + _icode + "'", "ICODE");
        if (col1 == "0") hasChild = "N";
        //if (hasChild == "Y")
        //{

        //}
        //else
        {
            DataView dv = new DataView(bomDt, "ICODE='" + _icode + "'", "icode,ibcode", DataViewRowState.CurrentRows);
            if (dv.Count > 0)
            {
                for (int i = 0; i < dv.Count; i++)
                {
                    _icode = dv[i]["ibcode"].ToString().Trim();
                    mrdr = mrs.NewRow();
                    mrdr["dated"] = _day;
                    mrdr["Parent"] = dv[i]["icode"].ToString().Trim();
                    mrdr["icode"] = _icode;
                    mrdr["icodeday"] = _icode + "-" + _day.PadLeft(2, '0');
                    mrdr["reqd"] = fgen.make_double(fgen.make_double(dv[i]["ibqty"].ToString()) * _bmQty, 4);
                    mrs.Rows.Add(mrdr);

                    getBOMChild(_icode, fgen.make_double(fgen.make_double(dv[i]["ibqty"].ToString()) * _bmQty, 4), _day);
                }
            }
        }

    }
    void do_MC_MGMT()
    {
        SQuery = "Select icode,shots_day,cavity from machmst where branchcd='" + frm_mbr + "' order by icode,shots_day";
        DataTable dtMchMst = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        SQuery = "Select to_char(vchdate,'dd/mm/yyyy') as doc_date,round(sum(bcd)/60,0) as total from prod_sheet where branchcd='" + frm_mbr + "' and type='20' and to_chaR(vchdate,'yyyymm')='" + Convert.ToDateTime(fromdt).ToString("yyyyMM") + "' group by to_char(vchdate,'dd/mm/yyyy') order by to_char(vchdate,'dd/mm/yyyy')";
        DataTable dtProdSheet = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        SQuery = "Select trim(col3) as doc_date,sum(Qty1) as total from multivch where branchcd='" + frm_mbr + "' and type='OF' and to_chaR(vchdate,'yyyymm')='" + Convert.ToDateTime(fromdt).ToString("yyyyMM") + "' group by trim(col3) order by trim(col3)";
        DataTable dtMultiVch = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        string dtmfld = "nvl(a.num1, 0)+nvl(a.num2, 0)+nvl(a.num3, 0)+nvl(a.num4, 0)+nvl(a.num5, 0)+nvl(a.num6, 0)+nvl(a.num7, 0)+nvl(a.num8, 0)+nvl(a.num9, 0)+nvl(a.num10, 0)+nvl(a.num11, 0)+nvl(a.num12, 0)";
        SQuery = "Select to_char(a.vchdate,'dd/mm/yyyy') as doc_date,round(sum(" + dtmfld + ")/60,0) as total,round(sum(a.total),0) as totalwrk,sum((a.iqtyin+nvl(a.mlt_loss,0))*b.iweight) as Tot_Prodn_kg,sum(a.iqtyin) as Tot_ok,sum(nvl(a.mlt_loss,0)) as Tot_rej,sum(un_melt) as tgt_shot ,sum(noups)  as act_shot  from prod_sheet a,item b where trim(A.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='90' and to_chaR(a.vchdate,'yyyymm')='" + Convert.ToDateTime(fromdt).ToString("yyyyMM") + "' group by to_char(a.vchdate,'dd/mm/yyyy') order by to_char(a.vchdate,'dd/mm/yyyy')";
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        SQuery = "Select * from schedule where branchcd='" + frm_mbr + "' and type='46' and to_chaR(vchdate,'yyyymm')='" + Convert.ToDateTime(fromdt).ToString("yyyyMM") + "' order by icode";
        DataTable dtSchedule = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        SQuery = "Select trim(col3) as doc_date,sum(Qty1) as total from multivch where branchcd='" + frm_mbr + "' and type='HF' and to_chaR(vchdate,'yyyymm')='" + Convert.ToDateTime(fromdt).ToString("yyyyMM") + "' group by trim(col3) order by trim(col3)";
        DataTable dtMultiVchHF = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        SQuery = "Select to_char(a.vchdate,'dd/mm/yyyy') as doc_date,sum(a.iqtyout*b.iweight) as Tot_sales_kg from ivoucher a,item b where trim(A.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and to_chaR(a.vchdate,'yyyymm')='" + Convert.ToDateTime(fromdt).ToString("yyyyMM") + "' and substr(a.icode,1,1) in ('7','9') group by to_char(a.vchdate,'dd/mm/yyyy') order by to_char(a.vchdate,'dd/mm/yyyy')";
        DataTable dtIvoucher = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select balop from type where id='B' and type1='" + frm_mbr + "'", "balop");
        if (col1 == "0")
        {
            fgen.msg("-", "AMSG", "Please Register No. of M/c for MIS Monitoring purpose in Branch");
            return;
        }
        string zdt1 = "01/" + fromdt.Substring(3, 2) + "/" + fromdt.Substring(6, 4);
        string zdt2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT (LAST_DAY(ADD_MONTHS(to_datE('" + zdt1 + "','dd/mm/yyyy'),0))) AS xdt2 FROM DUAL ", "xdt2");

        TimeSpan difference = Convert.ToDateTime(zdt2).Date - Convert.ToDateTime(zdt1).Date;
        int zdays = (int)difference.TotalDays + 1;

        create_tab();
        //sg.text(-2, 1) = "M/c:" & mc_cnt
        for (int i = 0; i < zdays; i++)
        {
            sg1_dr = sg1_dt.NewRow();

            sg1_dr[0] = fgen.padlc((i + 1), 2) + ")";
            sg1_dr[1] = Convert.ToDateTime(fromdt).AddDays(i).ToString("dd/MM/yyyy");
            //installed capacity
            sg1_dr[2] = fgen.make_double(col1) * 24;
            //holiday /fest
            if (dtMultiVchHF.Rows.Count > 0) sg1_dr[3] = fgen.seek_iname_dt(dtMultiVchHF, "doc_date='" + sg1_dr[1].ToString() + "'", "total");
            //available capacity
            sg1_dr[4] = fgen.make_double(sg1_dr[2].ToString()) - fgen.make_double(sg1_dr[3].ToString());
            //planned down time
            if (dtMultiVch.Rows.Count > 0) sg1_dr[5] = fgen.seek_iname_dt(dtMultiVch, "doc_date='" + sg1_dr[1].ToString() + "'", "total");
            //possible oper time
            sg1_dr[6] = fgen.make_double(sg1_dr[4].ToString()) - fgen.make_double(sg1_dr[5].ToString());
            //available load hr
            if (dtProdSheet.Rows.Count > 0) sg1_dr[7] = fgen.seek_iname_dt(dtProdSheet, "doc_date='" + sg1_dr[1].ToString() + "'", "total");
            //no Business
            sg1_dr[8] = fgen.make_double(sg1_dr[6].ToString()) - fgen.make_double(sg1_dr[7].ToString());
            //unplanned shut down
            if (dt.Rows.Count > 0)
            {
                DataView dv = new DataView(dt, "doc_date='" + sg1_dr[1].ToString() + "'", "", DataViewRowState.CurrentRows);
                if (dv.Count > 0)
                {
                    sg1_dr[9] = dv[0]["total"].ToString();
                    sg1_dr[10] = dv[0]["totalwrk"].ToString();
                    if (fgen.make_double(dv[0]["tgt_shot"].ToString()) > 0)
                    {
                        sg1_dr[14] = fgen.make_double((fgen.make_double(dv[0]["act_shot"].ToString()) / fgen.make_double(dv[0]["tgt_shot"].ToString())) * 100, 2);
                    }
                    if (fgen.make_double(dv[0]["Tot_ok"].ToString()) + fgen.make_double(dv[0]["tot_rej"].ToString()) > 0)
                    {
                        sg1_dr[15] = fgen.make_double((fgen.make_double(dv[0]["Tot_ok"].ToString()) / (fgen.make_double(dv[0]["Tot_ok"].ToString()) + fgen.make_double(dv[0]["tot_rej"].ToString()))) * 100, 2);
                    }
                    sg1_dr[16] = fgen.make_double(dv[0]["Tot_Prodn_kg"].ToString(), 2);
                }
            }
            //actual oper time
            sg1_dr[11] = fgen.make_double(sg1_dr[10].ToString().Trim()) - fgen.make_double(sg1_dr[9].ToString());
            if (fgen.make_double(sg1_dr[10].ToString().Trim()) > 0)
            {
                sg1_dr[12] = fgen.make_double((fgen.make_double(sg1_dr[11].ToString().Trim()) / fgen.make_double(sg1_dr[10].ToString())) * 100, 2);
            }
            if (fgen.make_double(sg1_dr[9].ToString().Trim()) > 0)
            {
                sg1_dr[13] = fgen.make_double((fgen.make_double(sg1_dr[9].ToString().Trim()) / fgen.make_double(sg1_dr[10].ToString())) * 100, 2);
            }
            //sales in kg
            if (dtIvoucher.Rows.Count > 0) sg1_dr[17] = fgen.make_double(fgen.seek_iname_dt(dtIvoucher, "doc_date='" + sg1_dr[1].ToString() + "'", "Tot_sales_kg").ToString(), 2);
            //hrs as per schceduled
            if (dtSchedule.Rows.Count > 0)
            {
                string mcode = "";
                string mfld = "";
                double tot_mch_time = 0;
                foreach (DataRow drSchedule in dtSchedule.Rows)
                {
                    mcode = drSchedule["icode"].ToString().Trim();
                    mfld = drSchedule[9].ToString() + sg1_dr[1].ToString().Substring(0, 2);
                    if (dtMchMst.Rows.Count > 0)
                    {
                        DataView dv = new DataView(dtMchMst, "icode='" + mcode + "'", "", DataViewRowState.CurrentRows);
                        if (dv.Count > 0)
                        {
                            if (fgen.make_double(dv[0]["shots_day"].ToString()) * fgen.make_double(dv[0]["cavity"].ToString()) > 0)
                            {
                                tot_mch_time += fgen.make_double(fgen.make_double(mfld) / fgen.make_double(dv[0]["shots_day"].ToString()) * fgen.make_double(dv[0]["cavity"].ToString()), 0);
                            }
                        }
                    }
                }

                sg1_dr[18] = fgen.make_double(tot_mch_time, 2);
            }

            sg1_dt.Rows.Add(sg1_dr);
        }
    }
    protected void sg1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        sg1.PageIndex = e.NewPageIndex;
        nextPageIndex();
    }
    protected void btn1_ServerClick(object sender, EventArgs e)
    {
        switch (frm_formID)
        {
            case "F40999":
                SQuery = "select a.ordno,a.orddt,b.iname,sum(a.prq) as Prqty,sum(a.poq) as POQty,sum(a.prq)-sum(a.poq) as Bal_prqty,b.unit,max(bank) as deptt,max(tr_insur) as Ind_ref,trim(a.icode) as icode,max(a.pflag)as pflag from (Select tr_insur,bank,pflag,ordno,orddt,icode,qtyord as prq,0 as poq from pomas where branchcd='" + frm_mbr + "' and type='60' and orddt>=to_DatE('01/04/2017','dd/mm/yyyy') union all Select null as tr_insur,null as bank,null as pflag,pr_no,pr_dt,icode,0 as prq,qtyord as poq from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt>=to_DatE('01/04/2017','dd/mm/yyyy') and substr(term,1,2) not like '%CANCELLED%')a,item b where trim(A.icode)=trim(B.icode) group by b.iname,b.unit,a.ordno,a.orddt,trim(a.icode) having sum(a.prq)-sum(a.poq)>0 and max(a.pflag)<>0 order by a.orddt,a.ordno";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_rptlevel("PR Pending PO", frm_qstr);
                break;
        }
    }
    protected void bt2_ServerClick(object sender, EventArgs e)
    {
        switch (frm_formID)
        {
            case "F40999":
                SQuery = "select a.ordno,a.orddt,b.aname,c.iname,sum(a.poq) as PO_Qty,sum(a.rcvq) as MRR_Qty,sum(a.poq)-sum(a.rcvq) as Bal_Qty,c.unit,trim(a.icode) as icode,max(a.pflag)as pflag from (Select pflag,ordno,orddt,acode,icode,qtyord as poq,0 as rcvq from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt>=to_DatE('01/04/2017','dd/mm/yyyy') union all Select null as pflag,ponum,podate,acode,icode,0 as prq,iqtyin as poq from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and potype like '5%' and vchdate>=to_DatE('01/04/2017','dd/mm/yyyy') and store in ('Y','N') )a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(a.icode)=trim(C.icode) group by c.iname,c.unit,b.aname,a.ordno,a.orddt,trim(a.AcodE),trim(a.icode) having sum(a.poq)-sum(a.rcvq)>0 and max(a.pflag)<>1 order by a.orddt,a.ordno";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_rptlevel("PO Pending MRR", frm_qstr);
                break;
        }
    }
    protected void btn3_ServerClick(object sender, EventArgs e)
    {
        switch (frm_formID)
        {
            case "F40999":
                SQuery = "select a.vchnum,a.vchdate,b.aname,c.iname,sum(a.iqty_chl) as tot,c.unit,trim(a.icode) as icode,a.type from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(A.icode)=trim(C.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + DateRange + " and a.inspected='N' and a.store!='R' group by b.aname,c.iname,c.unit,trim(a.icode),a.type,a.vchdate,a.vchnum order by a.vchdate,a.vchnum,trim(a.icode)";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_rptlevel("MRR Pending for QA", frm_qstr);
                break;
        }
    }
    protected void btn4_ServerClick(object sender, EventArgs e)
    {
        switch (frm_formID)
        {
            case "F40999":
                SQuery = "select b.aname as Customer,c.Iname as Item_Name,a.Icode,a.Wk_ref,a.num01 as Plan_Qty,a.Arch01 as Arch_Qty,a.IsArch,a.ent_by,a.ent_Dt,a.edt_by,a.edt_dt,a.Acode,a.vchnum,a.vchdate from sl_plan a, famst b,item c where trim(A.acodE)=trim(b.acode) and trim(A.icodE)=trim(c.icode) and  a.type='SL' and a.branchcd='" + frm_mbr + "' AND a.VCHDATE " + DateRange + " order by a.vchdate,a.vchnum,a.srno";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_rptlevel("Sales Plan During", frm_qstr);
                break;
        }
    }
    protected void btn5_ServerClick(object sender, EventArgs e)
    {
        switch (frm_formID)
        {
            case "F40999":
                SQuery = "Select 'From '||A.o_deptt as Section_Name,B.iname as Item_Name ,B.cpartno as Part_no, a.iqty_wt as Qty_Tfr,b.Unit,a.Ent_Dt ,round(sysdate-a.ent_dt,3) as Pend_Days,a.Ent_by,a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr,a.Naration,A.BTCHNO,A.invno,a.t_deptt from ivoucher a, item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,2)='3A' and a.vchdate " + DateRange + " AND a.store='W' and trim(nvl(stage,'-')) in ('61','62','63','64','65','66','67','68','69','6A','6B','6C','6D','6E','6R') and nvl(a.iqtyin,0)=0 and nvl(a.iqty_wt,0)!=0 order by a.vchdate,a.vchnum";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_rptlevel("Material Pending Acceptance", frm_qstr);
                break;
        }
    }
    protected void btnSF_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SF";
        if (frm_formID == "F40999") fgen.msg("-", "SMSG", "Are You Sure, You Want Generate SF Requirment!!");
    }
}