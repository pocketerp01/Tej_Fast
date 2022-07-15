using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

//CUST_CMPLT
public partial class om_cmplnt : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, cond, vardate, vchnum, fromdt, todt, typePopup = "Y", xStartDt = "", Enable = "", mq0, mq1;
    DataTable dt, dt1, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
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
                // doc_addl.Value = "1";
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            if (frm_ulvl == "0") txtjobno.ReadOnly = false;
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
        tab2.Visible = false;
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnprint.Disabled = false;
        create_tab();
        sg1_add_blankrows();
        btninvno.Enabled = false;
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; btnprint.Disabled = true;
        btninvno.Enabled = true;
    }
    //------------------------------------------------------------------------------------
    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
    }
    //------------------------------------------------------------------------------------
    public void set_Val()
    {
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        frm_tabname = "SCRATCH";
        frm_vty = "CC";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        lbl1a.Text = frm_vty;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        if (frm_cocd == "CCEL")
        {
            spnjobno.Visible = true; txtjobno.Visible = true;
            tddivision.Text = "Department"; txttechper.Visible = false;
            lblheader.Text = "Customer Request"; tdtechnicalper.Text = "";
            tdinvoice.Text = "Entry No"; tdcomplaint.Text = "Req. No";
            trextraval.Visible = false;
            tdtypcomplaint.Text = "Type of Request"; tdnaturcomplaint.Text = "Nature of Request";
            tdbatch1.Visible = false;
            lblheader.Text = "Customer Complaint";
        }
        else if (frm_cocd == "SRIS")
        {
            spnjobno.Visible = true; txtjobno.Visible = true; tdtechnicalper.Text = "Tech. Person"; txtjobno.Attributes.Add("Placeholder", "Ticket No.");
            spnjobno.InnerText = "Ticket No."; txtjobno.Width = 130; txttechper.Visible = true; trextraval.Visible = true;
            tdbatch1.Visible = true; txtinvbtch.ReadOnly = false;
            lblheader.Text = "Customer Complaint";
        }
        else if (frm_cocd == "SEL")
        {
            tdbatch1.Text = "Machine Sr.No";
            tdinvoice.Text = "Req. No";
            txtinvbtch.ReadOnly = false;
            DivAddress.Visible = false;
            DivParty.Visible = false;
            txtinvno.Visible = false;
            Label5.Visible = false; txtinvqty.Visible = false;
            Label6.Visible = false; txtpmrg.Visible = false;
            tdtechnicalper.Text = ""; txttechper.Visible = false;
            spnjobno.Visible = false; txtjobno.Visible = false; trextraval.Visible = false;
            lblheader.Text = "Customer Service Request";
            tdcomplaint.Text = "Ticket No";
            tdtypcomplaint.Text = "Type of Service";
            tdnaturcomplaint.Text = "Nature of Service";
            tddivision.Text = "Division of Service";
            tdbatch1.Visible = true;
            txtinvno.ReadOnly = true;
            txtinvdate.ReadOnly = true;
            if (frm_ulvl != "0")
            {
                btninvno.Visible = false;
            }
        }
        else
        {
            tdtechnicalper.Text = ""; txttechper.Visible = false;
            spnjobno.Visible = false; txtjobno.Visible = false; trextraval.Visible = false;
            tddivision.Text = "Division of Complaint";
            lblheader.Text = "Customer Complaint";
            tdbatch1.Visible = true; txtinvbtch.ReadOnly = true;
            txtinvno.ReadOnly = true;
            txtinvdate.ReadOnly = true;
            SEL1.Visible = false; SEL2.Visible = false; DivAddress.Visible = false;
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
            /////////////////
            sg1.Columns[12].HeaderStyle.Width = 50;
            sg1.Columns[13].HeaderStyle.Width = 400;
            //  sg1.Columns[14].HeaderStyle.Width = 1000;
            ///////////////////
            sg1.Columns[15].Visible = false;
            sg1.Columns[16].Visible = false;
            sg1.Columns[17].Visible = false;
            sg1.Columns[18].Visible = false;
            //    sg1.Columns[19].Visible = false;
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
                //if (txtlbl4.Text.Trim().Length <= 1)
                //{
                //    fgen.msg("-", "AMSG", "Please Select Work Order");
                //    return;
                //}
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
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CC");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
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
            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "Inv":
                if (frm_cocd == "NEOP")
                {
                    if (frm_ulvl != "0")
                    {
                        col1 = ""; col2 = "";
                        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(smsopts) as icons from evas where trim(upper(username))='" + frm_uname + "'", "icons");
                        if (col1.Length > 1)
                        {
                            string[] word = col1.Split(',');
                            foreach (string vp in word)
                            {
                                if (col2.Length > 0) col2 = col2 + "," + "'" + vp.ToString().Trim() + "'";
                                else col2 = "'" + vp.ToString().Trim() + "'";
                            }
                            if (col1 != "0") SQuery = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) AS FSTR,A.VCHNUM AS INV_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS INV_DT,B.ANAME AS PARTY_NAME,A.ACODE AS PARTY_CODE,C.INAME AS PRODUCT,C.ICODE AS ERP_CODE,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) AND A.TYPE LIKE '4%' AND A.TYPE!='47' and a.vchdate " + DateRange + " and trim(b.bssch) in (" + col2 + ") ORDER BY VDD";
                        }
                    }
                    else SQuery = "SELECT distinct A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) AS FSTR,A.VCHNUM AS INV_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS INV_DT,B.ANAME AS PARTY_NAME,A.ACODE AS PARTY_CODE,C.INAME AS PRODUCT,C.ICODE AS ERP_CODE,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) AND A.TYPE LIKE '4%' AND A.TYPE!='47' and a.vchdate " + DateRange + " ORDER BY VDD";
                }
                else
                {
                    SQuery = "SELECT distinct A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) AS FSTR,A.VCHNUM AS INV_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS INV_DT,B.ANAME AS PARTY_NAME,A.ACODE AS PARTY_CODE,C.INAME AS PRODUCT,C.ICODE AS ERP_CODE,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) AND A.TYPE LIKE '4%' AND A.TYPE!='47' and a.vchdate " + DateRange + " ORDER BY VDD desc ,vchnum desc";
                    if (frm_cocd == "CCEL")
                    {
                        //SQuery = "SELECT distinct A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) AS FSTR,d.cdrgno as job_no,A.VCHNUM AS INV_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS INV_DT,B.ANAME AS PARTY_NAME,A.ACODE AS PARTY_CODE,C.INAME AS PRODUCT,C.ICODE AS ERP_CODE,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHER A,FAMST B,ITEM C,somas d WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) and a.branchcd||a.type||a.ponum||to_Char(a.podate,'dd/mm/yyyy')=d.branchcd||d.type||d.ordno||to_Char(d.orddt,'dd/mm/yyyy') AND A.TYPE LIKE '4%' AND A.TYPE!='47' and a.vchdate " + DateRange + " ORDER BY VDD desc ,vchnum";
                        //SQuery = "SELECT distinct A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) AS FSTR,d.cdrgno as job_no,A.VCHNUM AS INV_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS INV_DT,B.ANAME AS PARTY_NAME,A.ACODE AS PARTY_CODE,C.INAME AS PRODUCT,C.ICODE AS ERP_CODE,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM SCRATCH A,FAMST B,ITEM C,somas d WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) and a.branchcd||a.type||a.ponum||to_Char(a.podate,'dd/mm/yyyy')=d.branchcd||d.type||d.ordno||to_Char(d.orddt,'dd/mm/yyyy') AND A.TYPE LIKE '4%' AND A.TYPE!='47' and a.vchdate " + DateRange + " ORDER BY VDD desc ,vchnum";
                        SQuery = "Select * from (SELECT distinct A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) AS FSTR,A.COL1 as job_no,B.ANAME AS PARTY_NAME,C.INAME AS PRODUCT,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM SCRATCH A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) AND A.TYPE ='CL' and a.vchdate " + DateRange + " union all " +
                            "SELECT distinct A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) AS FSTR,d.cdrgno as job_no,B.ANAME AS PARTY_NAME,C.INAME AS PRODUCT,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHER A,FAMST B,ITEM C,somas d WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) and a.branchcd||a.type||a.ponum||to_Char(a.podate,'dd/mm/yyyy')=d.branchcd||d.type||d.ordno||to_Char(d.orddt,'dd/mm/yyyy') AND A.TYPE LIKE '4%' AND A.TYPE!='47' and a.vchdate " + DateRange + " )";
                    }
                    if (frm_cocd == "SEL")
                    {
                        SQuery = "SELECT distinct A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) AS FSTR,A.VCHNUM AS INV_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS INV_DT,a.ccent as machine_srno,B.ANAME AS PARTY_NAME,A.ACODE AS PARTY_CODE,C.INAME AS PRODUCT,C.ICODE AS ERP_CODE,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) AND A.TYPE LIKE '40%' AND A.TYPE!='47' and a.vchdate " + DateRange + " and a.icode like '9%' ORDER BY VDD desc ,vchnum desc";
                        SQuery = "SELECT distinct A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) AS FSTR,a.ccent as machine_srno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS INV_DT,C.INAME AS PRODUCT,C.ICODE AS ERP_CODE,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) AND A.TYPE LIKE '40%' AND A.TYPE!='47' and a.vchdate " + DateRange + " and a.icode like '9%' ORDER BY VDD desc ";
                        SQuery = "SELECT distinct A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) AS FSTR,a.ccent as machine_srno,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS INV_DT,C.INAME AS PRODUCT,C.ICODE AS ERP_CODE,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD FROM (SELECT BRANCHCD,TYPE,ACODE,ICODE,VCHNUM,VCHDATE,CCENT FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '40%' AND TYPE!='47' and vchdate " + DateRange + " and icode like '9%' UNION ALL SELECT BRANCHCD,TYPE,ACODE,ICODE,INVNO,INVDATE,COL8 FROM SCRATCH WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='OD' ) A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe)  ";
                    }
                }
                break;

            case "Print_E":
            case "List":
                // if (frm_ulvl != "0") cond = " and trim(a.ent_by)='" + frm_uname + "'";
                cond = frm_ulvl != "0" ? " and trim(a.ent_by)='" + frm_uname + "'" : "";
                if (frm_cocd == "SEL")
                {
                    cond = frm_ulvl == "M" ? "AND TRIM(A.ACODE)='" + frm_uname + "'" : "";
                    mq0 = "Req";
                }
                else
                {
                    mq0 = "cmplnt";
                }
                SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr,a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as " + mq0 + "_no,to_char(a.vchdate,'dd/mm/yyyy') as " + mq0 + "_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,c.iname as item_name,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.vchnum,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='CC' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " " + cond + " order by vdd desc,a.vchnum desc";
                if (frm_cocd == "CCEL") SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr,a.col6 as job_no, a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as Req_no,to_char(a.vchdate,'dd/mm/yyyy') as Req_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,c.iname as item_name,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.vchnum as vdd from " + frm_tabname + " a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='CC' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " " + cond + " order by a.vchnum desc";
                if (frm_cocd == "SRIS") SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr,a.col6 as Ticket_no, a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as Req_no,to_char(a.vchdate,'dd/mm/yyyy') as Req_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,c.iname as item_name,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.vchnum as vdd from " + frm_tabname + " a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='CC' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " order by a.vchnum desc";
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                {
                    //if (frm_ulvl != "0") cond = " and trim(a.ent_by)='" + frm_uname + "'";
                    cond = frm_ulvl != "0" ? " and trim(a.ent_by)='" + frm_uname + "'" : "";
                    if (frm_cocd == "SEL")
                    {
                        cond = frm_ulvl == "M" ? "AND TRIM(A.ACODE)='" + frm_uname + "'" : "";
                        mq0 = "Req";
                    }
                    else
                    {
                        mq0 = "cmplnt";
                    }
                    // COMMENTED ON 11 JAN 2019 BY MADHVI BECAUSE IT GIVES MISSING EXPRESSION ERROR WHEN ULEVEL!=0
                    //ORIGINAL QUERY  SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr," + cond + " a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as cmplnt_no,to_char(a.vchdate,'dd/mm/yyyy') as cmpnt_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,c.iname as item_name,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.vchnum as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='CC' and a.branchcd='" + mbr + "' and a.vchdate " + DateRange + " " + cond + " order by a.vchnum desc";
                    SQuery = "Select distinct trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr,a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as " + mq0 + "_no,to_char(a.vchdate,'dd/mm/yyyy') as " + mq0 + "_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,c.iname as item_name,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.vchnum,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='CC' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " " + cond + " order by vdd desc,a.vchnum desc";
                    if (frm_cocd == "CCEL") SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr,a.col6 as job_no, a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as Req_no,to_char(a.vchdate,'dd/mm/yyyy') as Req_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,c.iname as item_name,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.vchnum as vdd from " + frm_tabname + " a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='CC' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " " + cond + " order by a.vchnum desc";
                    if (frm_cocd == "SRIS") SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr,a.col6 as Ticket_no, a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as Req_no,to_char(a.vchdate,'dd/mm/yyyy') as Req_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,c.iname as item_name,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.vchnum as vdd from " + frm_tabname + " a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='CC' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " order by a.vchnum desc";
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
    void fill_drop()
    {
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, "Select srno,name from typegrp where id='TC' and type1='000000' order by srno");
        ddntrofcmlnt.DataSource = dt;
        ddntrofcmlnt.DataTextField = "name";
        ddntrofcmlnt.DataValueField = "srno";
        ddntrofcmlnt.DataBind();

        dt1 = new DataTable();
        dt1 = fgen.getdata(frm_qstr, frm_cocd, "Select srno,name from typegrp where id='DC' and type1='000000' order by srno");
        dddivisioncmltn.DataSource = dt1;
        dddivisioncmltn.DataTextField = "name";
        dddivisioncmltn.DataValueField = "srno";
        dddivisioncmltn.DataBind();
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            hffield.Value = "New";
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            set_Val();
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
            if (frm_cocd == "SRIS")
            {
                if (frm_mbr == "00") cond = "U2"; else cond = "U6";
                txtjobno.Text = vchnum + "/" + DateTime.Now.ToString("ddMMyyyy") + "/" + cond;
            }
            fill_drop();
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
        btninvno.Focus();
        //create_tab();
        //sg1_add_blankrows();
        //sg1.DataSource = sg1_dt;
        //sg1.DataBind();
        //setColHeadings();
        //ViewState["sg1"] = sg1_dt;
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
            if (frm_cocd == "CCEL")
            {
                fgen.Fn_open_sseek("Select Your Request", frm_qstr);
            }
            else
            {
                fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Edit", frm_qstr);
            }
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
        if (frm_cocd == "SRIS")
        {
            if (sg1.Rows.Count > 0) btnhideF_s_Click(sender, e);
            else fgen.msg("-", "AMSG", "No data in Grid");
        }
        else
        {
            if (txtinvno.Text == "0" || txtinvno.Text == "-")
            {
                fgen.msg("-", "AMSG", "Please Select Invoice No.");
                btninvno.Focus();
                return;
            }
            if (sg1.Rows.Count > 0) fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
            btnsave.Disabled = true;
        }
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
            if (frm_cocd == "CCEL")
            {
                fgen.Fn_open_sseek("Select Your Request", frm_qstr);
            }
            else
            {
                fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Delete", frm_qstr);
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Print";
        make_qry_4_popup();
        if (frm_cocd == "CCEL")
        {
            fgen.Fn_open_sseek("Select Your Request", frm_qstr);
        }
        else
        {
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        make_qry_4_popup();
        if (frm_cocd == "SEL") fgen.Fn_open_prddmp1("-", frm_qstr);
        else
        {
            if (frm_cocd == "CCEL")
            {
                fgen.Fn_open_sseek("Select Your Request", frm_qstr);
            }
            else
            {
                fgen.Fn_open_sseek("Select Your Complaint", frm_qstr);
            }
        }
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
        ddntrofcmlnt.Items.Clear(); dddivisioncmltn.Items.Clear();

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
                fgen.msg("-", "AMSG", "Details are deleted for Complaint No. " + edmode.Value.Substring(0, 6) + "");
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
                case "Inv":
                    #region
                    clearctrl();
                    dt = new DataTable();
                    SQuery = "SELECT B.ANAME,b.addr1||','||b.addr2||','||b.addr3 as address,C.INAME ,a.*  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) and A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) in ('" + col1 + "')";
                    if (frm_cocd == "CCEL") SQuery = "SELECT A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS vchdate,B.ANAME ,A.ACODE ,C.INAME ,C.ICODE ,a.col1 FROM SCRATCH A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) and A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) in ('" + col1 + "')";
                    if (col1.Substring(2, 2) == "OD") SQuery = "SELECT A.invno as vchnum,1 as iqtyout,TO_CHAR(A.invdate,'DD/MM/YYYY') AS vchdate,B.ANAME ,b.addr1||','||b.addr2||','||b.addr3 as address,A.ACODE ,C.INAME ,C.ICODE,'-' as o_deptt,a.col8 as ccent,a.col13,a.col12 FROM SCRATCH A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) and A.BRANCHCD||A.TYPE||TRIM(A.invno)||TO_CHAR(A.invDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) in ('" + col1 + "')";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtinvno.Text = dt.Rows[0]["vchnum"].ToString().Trim(); txtinvdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                        txticode.Text = dt.Rows[0]["icode"].ToString().Trim(); txtiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtinvqty.Text = dt.Rows[0]["iqtyout"].ToString().Trim(); txtinvbtch.Text = dt.Rows[0]["o_deptt"].ToString().Trim();
                        if (frm_cocd == "CCEL") txtjobno.Text = dt.Rows[0]["col1"].ToString().Trim();
                        if (frm_cocd == "SEL")
                        {
                            txtinvbtch.Text = dt.Rows[0]["ccent"].ToString().Trim();
                            if (col1.Substring(2, 2) == "OD")
                            {
                                txtGur.Text = dt.Rows[0]["col12"].ToString().Trim();
                                txtGurDate.Text = dt.Rows[0]["col13"].ToString().Trim();
                                if (txtGurDate.Text.Trim().Length > 2)
                                {
                                    col3 = txtGurDate.Text.Substring(6, 4);
                                    col2 = txtvchdate.Text.Substring(6, 4);
                                    col3 = (fgen.make_double(col2) - fgen.make_double(col3)).ToString().Trim();
                                    if (fgen.make_double(col3) > fgen.make_double(txtGur.Text.Substring(0, 1))) lblGRStatus.Text = "Beyond Guaranty/Warranty";
                                    else lblGRStatus.Text = "Under Guaranty/Warranty";
                                }
                            }
                            else
                            {
                                txtGur.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT WEIGHT FROM SOMAS WHERE BRANCHCD||TYPE||TRIM(ORDNO)||TO_cHAR(ORDDT,'DD/MM/YYYY')||trim(icode)='" + dt.Rows[0]["branchcd"].ToString().Trim() + dt.Rows[0]["type"].ToString().Trim() + dt.Rows[0]["ponum"].ToString().Trim() + Convert.ToDateTime(dt.Rows[0]["podate"].ToString().Trim()).ToString("dd/MM/yyyy") + dt.Rows[0]["icode"].ToString().Trim() + "' ", "weight");
                                txtGurDate.Text = txtvchdate.Text.Trim();
                            }
                        }
                        txtPaddr.Text = dt.Rows[0]["address"].ToString().Trim();
                    }
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "Select srno,name as app,'-' as rmk from typegrp where id='CM' and type1='000000' order by srno");
                    create_tab();
                    sg1_dr = null;
                    for (i = 0; i < dt.Rows.Count; i++)
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
                        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        sg1_dr["sg1_f1"] = dt.Rows[i]["app"].ToString().Trim();
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
                    //   sg1_add_blankrows();
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    ViewState["sg1"] = sg1_dt;
                    dt.Dispose(); sg1_dt.Dispose();
                    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    txtpmrg.Focus();
                    fgen.EnableForm(this.Controls);
                    disablectrl();
                    setColHeadings();
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
                    fill_drop();
                    SQuery = "Select distinct a.vchnum as vchnum,a.vchdate,a.INVNO AS pono ,to_char(a.INVDATE,'dd/mm/yyyy') AS podate ,b.aname ,a.acode ,c.iname ,a.icode ,a.srno,a.COL1 as app,a.COL2,a.COL3,a.COL4,a.col6,a.col7,a.col8,a.col9,a.col10,a.COL12,a.COL13,A.COL14,a.REMARKS as rmk,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.naration,a.num1,a.num2,a.num3,a.num4,a.num5,a.num6 from " + frm_tabname + " a ,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + frm_mbr + frm_vty + col1 + "' order by a.srno";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    ViewState["fstr"] = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1.Substring(0, 16));
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtinvno.Text = dt.Rows[0]["pono"].ToString().Trim();
                        txtinvdate.Text = dt.Rows[0]["podate"].ToString().Trim();
                        txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                        txticode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["naration"].ToString().Trim();
                        txtntrcmpln.Text = dt.Rows[0]["col3"].ToString().Trim();
                        txtinvbtch.Text = dt.Rows[0]["COL8"].ToString().Trim();
                        txtinvqty.Text = dt.Rows[0]["COL9"].ToString().Trim();
                        txtpmrg.Text = dt.Rows[0]["COL10"].ToString().Trim();
                        txtGur.Text = dt.Rows[0]["COL12"].ToString().Trim();
                        txtGurDate.Text = dt.Rows[0]["COL13"].ToString().Trim();
                        lblGRStatus.Text = dt.Rows[0]["COL14"].ToString().Trim();
                        if (frm_cocd == "CCEL" || frm_cocd == "SRIS") txtjobno.Text = dt.Rows[0]["col6"].ToString().Trim();
                        if (frm_cocd == "SRIS")
                        {
                            txttechper.Text = dt.Rows[0]["COL7"].ToString().Trim();
                            txtinvbtch.Text = dt.Rows[0]["COL8"].ToString().Trim();
                            txtinvqty.Text = dt.Rows[0]["COL9"].ToString().Trim();
                            txtpmrg.Text = dt.Rows[0]["COL10"].ToString().Trim();
                            txttpt.Text = dt.Rows[0]["num1"].ToString().Trim();
                            txtlodging.Text = dt.Rows[0]["num2"].ToString().Trim();
                            txtfooding.Text = dt.Rows[0]["num3"].ToString().Trim();
                            txtmisc.Text = dt.Rows[0]["num4"].ToString().Trim();
                            txttot.Text = dt.Rows[0]["num5"].ToString().Trim(); cal();
                        }
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
                        col1 = dt.Rows[0]["col2"].ToString().Trim(); col2 = dt.Rows[0]["col4"].ToString().Trim();
                        ddntrofcmlnt.SelectedItem.Text = col1;
                        dddivisioncmltn.SelectedItem.Text = col2;
                        //sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString().Trim();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString().Trim();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    string frm_rptName = "";
                    if (frm_cocd == "NEOP")
                    {
                        // COMMENTED ON MADHVI ON 07/02/2019, FOR SHOWING BATCH NO. IN THE PRINT
                        //SQuery = "Select distinct a.vchnum as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.INVNO AS pono ,TO_CHAR(a.invdate,'dd/mm/yyyy') as podate ,b.aname ,a.acode ,c.iname ,a.icode ,a.srno,a.col1 as app,a.col6,a.remarks as rmk,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt from scratch a ,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' order by a.srno";
                        SQuery = "Select distinct a.vchnum as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.INVNO AS pono ,TO_CHAR(a.invdate,'dd/mm/yyyy') as podate ,b.aname ,a.acode ,c.iname ,a.icode ,a.srno,a.col1 as app,a.col6,a.remarks as rmk,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,A.COL8 AS BATCH from scratch a ,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' order by a.srno";
                        frm_rptName = "neopcmplnt";
                    }
                    else if (frm_cocd == "SEL")
                    {
                        SQuery = "Select distinct a.*,b.aname,b.addr1 as paddr1,b.addr2 as paddr2,b.email as pemail,c.iname,c.cpartno,0  iqtyout from scratch a ,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' order by a.srno";
                        frm_rptName = "cmplnt";
                    }
                    else
                    {
                        SQuery = "Select distinct a.*,b.aname,b.addr1 as paddr1,b.addr2 as paddr2,b.email as pemail,c.iname,c.cpartno,d.iqtyout  from scratch a ,famst b,item c,ivoucher D where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||trim(A.invno)||to_char(a.invdate,'dd/mm/yyyy')||trim(a.acode)||TRIM(a.icode)=D.branchcd||trim(D.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')||trim(D.acode)||TRIM(D.icode) and d.type like '4%' and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' order by a.srno";
                        if (frm_cocd == "SRIS")
                        {
                            frm_rptName = "cmplntsris";
                        }
                        else
                        {
                            frm_rptName = "cmplnt";
                        }
                    }
                    fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, frm_rptName, frm_rptName);
                    break;

                case "List":
                    // if (frm_ulvl == "0") cond = " and trim(a.ent_by)='" + frm_uname.Trim() + "'";
                    if (frm_ulvl != "0") cond = " and trim(a.ent_by)='" + frm_uname.Trim() + "'";
                    SQuery = "Select distinct a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt, a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' " + cond + " order by vdd desc,a.srno";
                    if (frm_cocd == "CCEL") SQuery = "Select distinct a.col2 as type_of_req,a.col3 as ntr_of_req,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as req_no,to_Char(a.vchdate,'dd/mm/yyyy') as req_dt, a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' " + cond + " order by vdd desc,a.srno";
                    if (frm_cocd == "SRIS") SQuery = "Select distinct a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt,a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,a.num1 as tpt_amt,a.num2 as lodging_amt,a.num3 as fooding_amt,a.num4 as misc_amt,a.num5 as Tot_amt,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' /*" + cond + "*/ order by vdd desc,a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    if (frm_cocd == "CCEL") fgen.Fn_open_rptlevel("Request List", frm_qstr);
                    else fgen.Fn_open_rptlevel("Complaint List", frm_qstr);
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------  
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        set_Val();
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            if (frm_ulvl == "0") SQuery = "Select distinct a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt,a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.vchnum,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE)  and a.vchdate " + DateRange + " and a.type='CC' order by vdd desc,a.vchnum desc,a.srno";
            else SQuery = "Select distinct a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt,a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.vchnum,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.vchdate " + DateRange + " and a.type='CC' and trim(a.ent_by)='" + frm_uname.Trim() + "' order by vdd desc,a.vchnum desc,a.srno";
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
                                //string doc_is_ok = "";
                                //frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                //doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                //if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }

                                //  CONTINUOUS NUMBER IS REQUIRED THAT'S WHY THIS FUNCTION IS USED
                                i = 0;
                                do
                                {
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum)+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "'", 6, "vch");
                                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + System.DateTime.Now.ToString("dd/MM/yyyy"), frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                }
                                while (pk_error == "Y");
                                if (pk_error == "Y") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }

                                if (frm_cocd == "SRIS")
                                {
                                    if (frm_mbr == "00") cond = "U2"; else cond = "U6";
                                    txtjobno.Text = vchnum + "/" + DateTime.Now.ToString("ddMMyyyy") + "/" + cond;
                                }
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

                        col3 = "";
                        if (frm_cocd == "NEOP")
                        {
                            col3 = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum from inspmst where branchcd!='DD' and type='70' and trim(icode)='" + txticode.Text.Trim() + "'", "vchnum");
                            if (col3.Trim().Length == 6) fgen.execute_cmd(frm_qstr, frm_cocd, "update inspmst set app_by='-' where branchcd!='DD' and type='70' and trim(icode)='" + txticode.Text.Trim() + "'");
                            col3 = fgen.seek_iname(frm_qstr, frm_cocd, "select ibcode from itemosp where branchcd!='DD' and type='BM' and trim(icode)='" + txticode.Text.Trim() + "' and substr(ibcode,1,1)>='7' ", "ibcode");
                            if (col3.Trim().Length == 8) fgen.execute_cmd(frm_qstr, frm_cocd, "update inspmst set app_by='-' where branchcd!='DD' and type='70' and trim(icode)='" + col3.Trim() + "'");
                        }

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
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); sg1.DataSource = null; sg1.DataBind();
                        ddntrofcmlnt.Items.Clear(); dddivisioncmltn.Items.Clear(); lblGRStatus.Text = "";
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
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg1.Rows.Count; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow["INVNO"] = txtinvno.Text.Trim().ToUpper();
            oporow["INVDATE"] = txtinvdate.Text.Trim().ToUpper();
            oporow["acode"] = txtacode.Text.Trim().ToUpper();
            oporow["icode"] = txticode.Text.Trim().ToUpper();
            oporow["srno"] = i + 1;
            oporow["COL1"] = sg1.Rows[i].Cells[13].Text.Trim();
            oporow["COL2"] = ddntrofcmlnt.SelectedItem.Text.ToUpper();
            oporow["COL3"] = txtntrcmpln.Text.Trim().ToUpper();
            oporow["COL4"] = dddivisioncmltn.SelectedItem.Text.ToUpper();
            oporow["COL9"] = txtinvqty.Text.Trim(); oporow["COL10"] = txtpmrg.Text.Trim().ToUpper();
            if (frm_cocd == "CCEL" || frm_cocd == "SRIS") oporow["COL6"] = txtjobno.Text.Trim().ToUpper();
            oporow["remarks"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
            if (txtrmk.Text.Trim().Length > 125)
            {
                oporow["naration"] = txtrmk.Text.Trim().ToUpper().Substring(0, 124);
            }
            else
            {
                oporow["naration"] = txtrmk.Text.Trim().ToUpper();
            }
            if (frm_cocd == "SRIS")
            {
                oporow["COL7"] = txttechper.Text.Trim().ToUpper();
                oporow["COL8"] = txtinvbtch.Text.Trim().ToUpper();
                oporow["COL9"] = txtinvqty.Text.Trim().ToUpper();
                oporow["COL10"] = txtpmrg.Text.Trim().ToUpper();
                oporow["num1"] = txttpt.Text.Trim().ToUpper().Replace("&nbsp;", "0").Replace("-", "0");
                oporow["num2"] = txtlodging.Text.Trim().ToUpper().Replace("&nbsp;", "0").Replace("-", "0");
                oporow["num3"] = txtfooding.Text.Trim().ToUpper().Replace("&nbsp;", "0").Replace("-", "0");
                oporow["num4"] = txtmisc.Text.Trim().ToUpper().Replace("&nbsp;", "0").Replace("-", "0");
                oporow["num5"] = txttot.Text.Trim().ToUpper().Replace("&nbsp;", "0").Replace("-", "0");
            }
            oporow["COL8"] = txtinvbtch.Text.Trim().ToUpper();
            oporow["COL12"] = txtGur.Text.Trim();
            oporow["COL13"] = txtGurDate.Text.Trim();
            oporow["COL14"] = lblGRStatus.Text.Trim();
            if (edmode.Value == "Y")
            {
                oporow["app_by"] = "-";
                oporow["app_dt"] = vardate;
                oporow["chk_by"] = "-";
                oporow["chk_dt"] = vardate;
                oporow["eNt_by"] = ViewState["entby"].ToString();
                oporow["eNt_dt"] = ViewState["entdt"].ToString();
                oporow["edt_by"] = frm_uname;
                oporow["edt_dt"] = vardate;
            }
            else
            {
                oporow["app_by"] = "-";
                oporow["app_dt"] = vardate;
                oporow["chk_by"] = "-";
                oporow["chk_dt"] = vardate;
                oporow["eNt_by"] = frm_uname;
                oporow["eNt_dt"] = vardate;
                oporow["edt_by"] = "-";
                oporow["eDt_dt"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);
        }
    }
    //------------------------------------------------------------------------------------
    protected void btninvno_Click(object sender, ImageClickEventArgs e)
    {
        clearctrl();
        hffield.Value = "Inv";
        make_qry_4_popup();
        if (frm_cocd == "CCEL") fgen.Fn_open_sseek("Select Job No.", frm_qstr);
        else fgen.Fn_open_sseek("Select Invoice No.", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    public void myfun()
    {
        string vip = "";
        vip = vip + "<script type='text/javascript'>function calculateSum() {";
        vip = vip + "var a=fill_zero(document.getElementById('ctl00_ContentPlaceHolder1_txttpt').value);";
        vip = vip + "var b=fill_zero(document.getElementById('ctl00_ContentPlaceHolder1_txtlodging').value);";
        vip = vip + "var c=fill_zero(document.getElementById('ctl00_ContentPlaceHolder1_txtfooding').value);";
        vip = vip + "var d=fill_zero(document.getElementById('ctl00_ContentPlaceHolder1_txtmisc').value);";
        vip = vip + "document.getElementById('ctl00_ContentPlaceHolder1_txttot').value = (a*1) + (b*1) + (c*1) + (d*1); ";

        vip = vip + "}";
        vip = vip + "function fill_zero(val){ if(isNaN(val)) return 0; if(isFinite(val)) return val; }</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", vip.ToString(), false);
    }
    //------------------------------------------------------------------------------------
    public void cal()
    {
        txttot.Text = Convert.ToString(Math.Round(fgen.make_double(txttpt.Text.Trim()) + fgen.make_double(txtlodging.Text.Trim()) + fgen.make_double(txtfooding.Text.Trim()) + fgen.make_double(txtmisc.Text.Trim())));
    }
    //------------------------------------------------------------------------------------
}