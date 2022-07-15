using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_dbd_mgrph : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, SQuery2, SQuery3, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
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
    string chartScript = "";
    string stitle1, stitle2, stitle3, stitle4;
    string val_legnd1, val_legnd2, val_legnd3, val_legnd4;
    string graphType1, graphType2, graphType3, graphType4;
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
            btnsave.Visible = false;
            btnprint.Visible = false;
            btnlist.Visible = false;

            #region
            int col_count = 0;
            double wid = 0;
            double ad = 50;
            if (sg1.Rows.Count > 0)
            {
                col_count = sg1.HeaderRow.Cells.Count;
                wid = 0;
                for (int i = 0; i < col_count; i++)
                {
                    ad = 10;
                    if (sg1.Rows[0].Cells[i].Text.Length < 2) ad = 30;
                    else if (sg1.Rows[0].Cells[i].Text.Length < 5) ad = 25;
                    else if (sg1.Rows[0].Cells[i].Text.Length > 50) ad = 2;
                    else if (sg1.Rows[0].Cells[i].Text.Length > 25) ad = 5;
                    else if (sg1.Rows[0].Cells[i].Text.Length > 20) ad = 8;
                    wid += fgen.make_double(sg1.Rows[0].Cells[i].Text.Length, 0) * ad;
                }
                try { sg1.Width = Convert.ToUInt16(wid + 100); }
                catch { sg1.Width = 1500; }

                if (sg1.Width.Value <= 800) sg1.Width = Unit.Percentage(100);
            }

            #endregion
        }
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
        frm_tabname = "WB_CSS_LOG";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        
        switch (Prg_Id)
        {
            case "F60101":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CS");
                typePopup = "N";
                break;
            case "F45143":
                lblheader.Text = "CRM Leads Followup Review";
                break;
            case "F45153":
                lblheader.Text = "CRM Leads Registration Review";
                break;

            case "F90142":
                lblheader.Text = "Task Management Status";
                break;
            case "F50159G":
                lblheader.Text = "Dom Sales Status Review";
                break;
            case "F35108G":
                lblheader.Text = "Job Order Status Report";
                //gridDiv1.Style.Add("height", "205px");
                //gridDiv3.Style.Add("height", "205px");
                //grid4.Visible = false;
                break;
            default:
                lblheader.Text = "Data Review System (DRS)";
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

        chk_rights = "Y";
        if (chk_rights == "Y")
        {
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            switch (Prg_Id)
            {
                case "F45143":
                case "F45153":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "F90142":
                    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
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
        string party_cd = "";
        string part_cd = "";
        SQuery = "";
        string add_filt = "1=1";
        switch (frm_formID)
        {
            case "F45143":
                
                lblSg2.Text = "Leads Followup (All. Vertical)";
                
                if (frm_ulvl != "0")

                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                if (party_cd.Trim().Length <= 1)
                {
                    party_cd = "%";
                }
                if (part_cd.Trim().Length <= 1)
                {
                    part_cd = "%";
                }
                lblSg2.Text = "Leads Followup (All.Verticals)";

                if (frm_ulvl != "0" && frm_ulvl != "1") 
                {
                    lblSg2.Text = "Leads Followup (Curr Team Member)";    
                    add_filt = "Upper(trim(team_member))='" + frm_uname + "'";
                }

                add_filt = "1=1";
                //and ccode like '" + party_cd + "%' and Team_member like '" + part_cd + "%'
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.LACNO)||TO_cHAR(A.LACDT,'DD/MM/YYYY') AS FSTR,TO_cHAR(A.LACDT,'DD/MM/YYYY') AS Followup_DT,a.ent_by as Action_by,a.Lvertical,a.Ldescr,a.Lgrade,a.Curr_stat,a.Ent_Dt,A.Cont_Name,a.Cont_No,A.LACNO AS Task_No,TO_CHAR(A.LAcdt,'YYYYMMDD') AS VDD FROM wb_lead_Act A  WHERE A.BRANCHCD='" + frm_mbr + "' and a.type like 'LA%' AND A.LACDT " + PrdRange + " and " + add_filt + "  ORDER BY VDD DESC,a.LACNO desc";

                lblSg1.Text = "List of CRM Leads Followup";
                
                lblSg3.Text = "Leads Followup (Curr Team Member)";
                lblSg4.Text = " - ";

                break;
            case "F45153":

                lblSg2.Text = "Leads Registration (All. Vertical)";

                if (frm_ulvl != "0")

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                if (party_cd.Trim().Length <= 1)
                {
                    party_cd = "%";
                }
                if (part_cd.Trim().Length <= 1)
                {
                    part_cd = "%";
                }
                lblSg2.Text = "Leads Registration (All.Verticals)";

                if (frm_ulvl != "0" && frm_ulvl != "1")
                {
                    lblSg2.Text = "Leads Registration (Curr Team Member)";
                    add_filt = "Upper(trim(team_member))='" + frm_uname + "'";
                }

                add_filt = "1=1";
                //and ccode like '" + party_cd + "%' and Team_member like '" + part_cd + "%'
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.LRCNO)||TO_cHAR(A.LRCDT,'DD/MM/YYYY') AS FSTR,TO_cHAR(A.LRCDT,'DD/MM/YYYY') AS Registration_Dt,a.ent_by as Action_by,a.Lvertical,a.Ldescr,a.Lgrade,a.Curr_stat,a.Ent_Dt,A.Cont_Name,a.Cont_No,A.LRCNO AS Task_No,TO_CHAR(A.LRCDT,'YYYYMMDD') AS VDD FROM wb_lead_log A  WHERE A.BRANCHCD='" + frm_mbr + "' and a.type like 'LR%' AND A.LRCDT " + PrdRange + " and " + add_filt + "  ORDER BY VDD DESC,a.LRCNO desc";

                lblSg1.Text = "List of CRM Leads Registration";

                lblSg3.Text = "Leads Registration (Curr Team Member)";
                lblSg4.Text = " - ";

                break;

            case "F90142":
                
                lblSg2.Text = "Tasks Pending (All. Team Member)";

                if (frm_ulvl != "0")

                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                if (party_cd.Trim().Length <= 1)
                {
                    party_cd = "%";
                }
                if (part_cd.Trim().Length <= 1)
                {
                    part_cd = "%";
                }
                lblSg2.Text = "Tasks Pending (All. Team Member)";

                if (frm_ulvl != "0" && frm_ulvl != "1")
                {
                    lblSg2.Text = "Tasks Done (Curr Team Member)";
                    add_filt = "Upper(trim(team_member))='" + frm_uname + "'";
                }


                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.TRCNO)||TO_cHAR(A.TRCDT,'DD/MM/YYYY') AS FSTR,TO_cHAR(A.TRCDT,'DD/MM/YYYY') AS Task_DT,a.ent_by as Assign_by,a.Team_member,a.Task_type,a.CCode as Client,a.TGT_Days,a.Curr_stat,a.Last_actdt,A.Client_Name,a.Oremarks,A.TRCNO AS Task_No,TO_CHAR(A.trcdt,'YYYYMMDD') AS VDD FROM wb_task_log A  WHERE A.BRANCHCD='" + frm_mbr + "' and a.type like 'TR%' AND A.TRCDT " + PrdRange + " and " + add_filt + " and ccode like '" + party_cd + "%' and Team_member like '" + part_cd + "%' ORDER BY VDD DESC,a.TRCNO desc";

                lblSg1.Text = "List of Assigned Tasks";

                lblSg3.Text = "Tasks Pending (Curr Team Member)";
                lblSg4.Text = " - ";

                break;

            case "F50159G":
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY') AS FSTR,A.Vchnum AS Inv_No,TO_cHAR(A.vchdate,'DD/MM/YYYY') AS Inv_Dt,B.ANAME AS Customer,a.Bill_tot as Total_Amt,A.Mo_Vehi as Vehicle_no,A.Invtime as Inv_Time,A.ACODE AS CODE,a.type,TO_CHAR(A.vchdate,'YYYYMMDD') AS VDD FROM sale A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '4%' AND A.vchdate " + PrdRange + " ORDER BY VDD DESC,a.vchnum desc,a.type";

                lblSg1.Text = "List of Invoices";
                lblSg2.Text = "Item Wise Details for Invoice # ";
                lblSg3.Text = "Last Dispatch of Item for Invoice # ";
                lblSg4.Text = "Last Production of Item for Invoice #";

                break;

            case "F25191":
                SQuery = "SELECT DISTINCT TRIM(A.icode) AS FSTR,A.Icode AS ERP_code,a.Iname AS Item_Name,a.Cpartno,a.Cdrgno,a.Unit,TO_CHAR(A.ent_Dt,'YYYYMMDD') AS VDD FROM item a WHERE length(Trim(A.icode))>=8 ORDER BY a.Icode";

                lblSg1.Text = "Details of Items";
                lblSg2.Text = "Details of Selected Gate Inward";
                lblSg3.Text = "Details of Purchase Orders";
                lblSg4.Text = "Details of Purchase Orders";
                break;

            case "F10174":
                SQuery = "SELECT DISTINCT TRIM(A.icode) AS FSTR,A.Icode AS ERP_code,a.Iname AS Item_Name,a.Cpartno,a.Cdrgno,a.Unit,TO_CHAR(A.ent_Dt,'YYYYMMDD') AS VDD FROM item a WHERE length(Trim(A.icode))>=8 ORDER BY a.Icode";

                lblSg1.Text = "Details of Items";
                lblSg2.Text = "Details of Selected Gate Inward";
                lblSg3.Text = "Details of Purchase Orders";
                lblSg4.Text = "Details of Purchase Orders";
                break;

            case "F20159":
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY') AS FSTR,A.Vchnum AS MRR_NO,TO_cHAR(A.vchdate,'DD/MM/YYYY') AS MRR_dT,B.ANAME AS Suppliler,A.ACODE AS CODE,a.type,TO_CHAR(A.vchdate,'YYYYMMDD') AS VDD FROM ivoucherp A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '0%' AND A.vchdate " + PrdRange + " ORDER BY VDD DESC,a.vchnum desc,a.type";

                lblSg1.Text = "Details of Gate Inwards";
                lblSg2.Text = "Details of Selected Gate Inward";
                lblSg3.Text = "Details of Purchase Orders";
                lblSg4.Text = "Details of Purchase Orders";
                break;

            case "F25193":
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY') AS FSTR,A.Vchnum AS MRR_NO,TO_cHAR(A.vchdate,'DD/MM/YYYY') AS MRR_dT,B.ANAME AS Suppliler,A.ACODE AS CODE,a.type,TO_CHAR(A.vchdate,'YYYYMMDD') AS VDD FROM ivoucher A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '0%' AND A.vchdate " + PrdRange + " ORDER BY VDD DESC,a.vchnum desc,a.type";

                lblSg1.Text = "Details of MRR/GRN";
                lblSg2.Text = "Details of Selected MRR/GRN";
                lblSg3.Text = "Details of Purchase Orders";
                lblSg4.Text = "Details of Purchase Orders";
                break;
            case "F30159":
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY') AS FSTR,A.Vchnum AS MRR_NO,TO_cHAR(A.vchdate,'DD/MM/YYYY') AS MRR_dT,B.ANAME AS Suppliler,A.ACODE AS CODE,a.type,TO_CHAR(A.vchdate,'YYYYMMDD') AS VDD FROM ivoucher A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '0%' AND A.vchdate " + PrdRange + " ORDER BY VDD DESC,a.vchnum desc,a.type";

                lblSg1.Text = "Details of MRR/GRN";
                lblSg2.Text = "Details of Selected MRR/GRN";
                lblSg3.Text = "Details of Purchase Orders";
                lblSg4.Text = "Details of Purchase Orders";
                break;
            case "F47159":
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_cHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR,A.ORDNO AS SO_NO,TO_cHAR(A.ORDDT,'DD/MM/YYYY') AS SO_Date,B.ANAME AS CUSTOMER,A.ACODE AS CODE,a.type,TO_CHAR(A.ORDDT,'YYYYMMDD') AS VDD FROM SOMAS A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '4%' AND A.ORDDT " + PrdRange + " ORDER BY VDD DESC,a.ordno desc,a.type";

                lblSg1.Text = "List of Sales Ordedrs";
                lblSg2.Text = "Item Wise Details for Order # ";
                lblSg3.Text = "Last Dispatch of Item for Order # ";
                lblSg4.Text = "Last Production of Item for Order #";
                break;

            case "F50101":
                break;
            case "F35108G":
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY') AS FSTR,A.Vchnum AS Job_No,TO_cHAR(A.vchdate,'DD/MM/YYYY') AS JoB_Dt,a.icode as erpcode,b.iname as product,b.cpartno,b.unit,A.qty ,a.type,TO_CHAR(A.vchdate,'YYYYMMDD') AS VDD FROM costestimate A, item B WHERE TRIM(A.icode)=TRIM(b.icode) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '30%' AND A.vchdate " + PrdRange + " and a.srno=0 ORDER BY VDD DESC,a.vchnum desc,a.type";
                SQuery = "Select '" + frm_mbr + "'||'30'||trim(M.Job_no)||to_char(M.dated,'dd/mm/yyyy') as fstr, M.Job_no,to_char(M.dated,'dd/mm/yyyy') as job_dt,m.icode as erpcode,trim(N.iname)||' '||trim(m.btchno) as Item_name,N.Cpartno as Part_No,M.Job_qty,M.Prodn,M.Done,M.status,M.TOT_SHEET,M.issu,M.acode,m.REJALL,decode(trim(m.Iscancel),'Y','Cancel','N') as Iscancel,m.closeby,m.cancelby,m.JStatus,m.Supcl_BY,m.col18 as PWidth,m.col19 as PLength,m.nups from (Select X.Job_no,X.dated,'-' as Part_No,'-' as Item_name,X.qty as Job_qty,Nvl(y.prodn,0) as Prodn,round((Nvl(y.prodn,0)/(cASE WHEN X.qty=0 THEN 1 ELSE X.QTY END))*100,0)||'%' as Done, x.JStatus,x.status,x.Iscancel,x.az_by,x.az_Dt,X.COL14 AS TOT_SHEET,x.issu,X.icode,x.acode,x.convdate as fstr,x.picode,x.REJALL,x.closeby,x.cancelby,x.supcl_BY,x.btchno,x.col18,x.col19,x.nups,x.col24,x.col12,x.enqdt from (select A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Qty,a.icode,a.enqno as Iscancel,decode(a.JStatus,'Y','SupClose','U/JProcess') as JStatus,decode(a.Status,'Y','Complete','U/Process') as Status,a.acode,a.convdate,a.az_by,to_char(a.az_Dt,'dd/mm/yyyy') as az_dt,TO_NUMBER(A.COL14)*TO_NUMBER(A.COL13) AS COL14,ROUND((NVL(C.ISS,0)-TO_NUMBER(A.COL15))*TO_NUMBER(A.COL13),2) AS ISSU,TO_NUMBER(A.COL15) AS REJALL,a.picode,a.attach as Closeby,a.attach2 as cancelby,a.Supcl_BY,trim(a.col20) as btchno,a.col18,a.col19,a.col13 as nups,a.col24,a.col12,a.enqdt  from costestimate A LEFT OUTER JOIN (sELECT job_no,job_dt,SUM(A5) AS ISS FROM PROD_SHEET  WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='85' AND VCHDATE >=TO_dATe('" + frm_CDT1 + "','DD/MM/YYYY') and substr(icode,1,2)!='07**' GROUP BY job_no,job_dt) C ON A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(C.job_no)||TRIM(C.job_dt) WHERE a.branchcd='00' and a.type='30' and a.vchdate  between to_Date('01/04/2018','dd/mm/yyyy') and to_date('07/04/2018','dd/mm/yyyy')  and A.VCHNUM LIKE '%' AND A.SRNO=1 AND a.acode like '%%' and a.icode like '%%' and A.STATUS!='Y' and a.enqno!='Y') x left outer join (select trim(icode) as icode,trim(invno) as invno,sum(iqtyin) as prodn from ivoucher where branchcd='" + frm_mbr + "' and (type='15' OR type='16') and vchdate>=to_DatE('" + frm_CDT1 + "','dd/mm/yyyy') group by trim(icode),trim(invno)) y on trim(x.icode)=trim(y.icode) and trim(x.job_no)=trim(y.invno)  ) M left outer join item N on trim(M.icode)=trim(N.icode) where substr(m.icode,1,2) like '%%' order by M.Dated desc ,M.job_no desc";

                lblSg1.Text = "List of Job Card";
                lblSg2.Text = "Item Wise Details for Job Card # ";
                lblSg3.Text = "Process Wise Prodn Status # ";
                lblSg4.Text = "- #";
                break;

            default:
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_cHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR,A.ORDNO AS PO_NO,TO_cHAR(A.ORDDT,'DD/MM/YYYY') AS PO_dT,B.ANAME AS CUSTOMER,A.ACODE AS CODE,a.type,TO_CHAR(A.ORDDT,'YYYYMMDD') AS VDD FROM POMAS A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '5%' AND A.ORDDT " + DateRange + " ORDER BY VDD DESC,a.ordno desc,a.type";

                lblSg1.Text = "Details of Purchase Orders";
                lblSg2.Text = "Details of Purchase Orders";
                lblSg3.Text = "Details of Purchase Orders";
                lblSg4.Text = "Details of Purchase Orders";
                break;
        }
        if (SQuery.Length > 1)
        {
            sg1_dt = new DataTable();
            sg1_dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            sg1.DataSource = sg1_dt;
            sg1.DataBind();

            setGridWidth(sg1);
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
        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
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
        ViewState["sg1"] = null;
        ViewState["sg2"] = null;
        ViewState["sg3"] = null;
        ViewState["sg4"] = null;
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

                    break;
                case "SG4_RMV":

                    break;
                case "SG3_RMV":

                    break;
                case "SG1_RMV":

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
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fillGrid();
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

            e.Row.Attributes["ondblclick"] = ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex);
            e.Row.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex) + ";}";
            e.Row.ToolTip = "Click to select this row.";

            sg1.HeaderRow.Cells[0].Style["display"] = "none";
            sg1.HeaderRow.Cells[1].Style["display"] = "none";

            e.Row.Cells[0].Style["display"] = "none";
            e.Row.Cells[1].Style["display"] = "none";
        }
    }

    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------    

    //------------------------------------------------------------------------------------    
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
                wid += fgen.make_double(sg1.Columns[0].ItemStyle.Width.Value, 0);
            }

            try { gName.Width = Convert.ToUInt16(wid + 100); }
            catch { gName.Width = 1500; }
        }
    }
    protected void sg1_SelectedIndexChanged(object sender, EventArgs e)
    {
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        GridViewRow row = sg1.SelectedRow;
        SQuery1 = "";
        SQuery2 = "";
        SQuery3 = "";
        string add_filt = "";
        switch (frm_formID)
        {
            case "F45143":
                
                add_filt = "1=1";
                SQuery1 = "SELECT trim(upper(lvertical)) AS YR,count(*) AS Task_Count FROM wb_lead_Act WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE 'LA%' AND LACDT " + PrdRange + "  and " + add_filt + " GROUP BY trim(upper(lvertical)) order by trim(upper(lvertical)) ";
                SQuery2 = "select YR,Lead_Count  from (SELECT TO_CHAR(LACDT,'MON-YY') AS YR,count(*) AS Lead_Count,TO_CHAR(LACDT,'YYYYMM') as VDD FROM wb_lead_Act WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE 'LA%' AND LACDT " + PrdRange + " and trim(upper(lvertical))='" + row.Cells[4].Text.Trim().ToUpper() + "'  and " + add_filt + " GROUP BY TO_CHAR(LACDT,'MON-YY'),TO_CHAR(LACDT,'YYYYMM')) order by VDD ";

                
                lblSg1.Text = "CRM Leads Followup Review";
                lblSg2.Text = "Lead Followup (All. Vertical)";
                lblSg3.Text = "Lead Working (Curr Vertical) " + row.Cells[4].Text.Trim();
                lblSg4.Text = " - ";

                graphType1 = "pie";
                graphType2 = "column";

                break;
            case "F45153":

                add_filt = "1=1";
                SQuery1 = "SELECT trim(upper(lvertical)) AS YR,count(*) AS Task_Count FROM wb_lead_log WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE 'LR%' AND LRCDT " + PrdRange + "  and " + add_filt + " GROUP BY trim(upper(lvertical)) order by trim(upper(lvertical)) ";
                SQuery2 = "select YR,Lead_Count  from (SELECT TO_CHAR(LRCDT,'MON-YY') AS YR,count(*) AS Lead_Count,TO_CHAR(LRCDT,'YYYYMM') as VDD FROM wb_lead_log WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE 'LR%' AND LRCDT " + PrdRange + " and trim(upper(lvertical))='" + row.Cells[4].Text.Trim().ToUpper() + "'  and " + add_filt + " GROUP BY TO_CHAR(LRCDT,'MON-YY'),TO_CHAR(LRCDT,'YYYYMM')) order by VDD ";


                lblSg1.Text = "CRM Leads Registration Review";
                lblSg2.Text = "Lead Registration (All. Vertical)";
                lblSg3.Text = "Lead Working (Curr Vertical) " + row.Cells[4].Text.Trim();
                lblSg4.Text = " - ";

                graphType1 = "pie";
                graphType2 = "column";

                break;

            case "F90142":
                
                add_filt = "1=1";
                SQuery1 = "SELECT trim(upper(Team_member)) AS YR,count(*) AS Task_Count FROM wb_task_log WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE 'TR%' AND TRCDT " + PrdRange + " and trim(nvl(curr_Stat,'-'))='-' and " + add_filt +" GROUP BY trim(upper(Team_member)) order by trim(upper(Team_member)) ";
                SQuery2 = "select YR,Task_Count  from (SELECT TO_CHAR(TRCDT,'MON-YY') AS YR,count(*) AS Task_Count,TO_CHAR(TRCDT,'YYYYMM') as VDD FROM wb_task_log WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE 'TR%' AND TRCDT " + PrdRange + " and trim(upper(Team_member))='" + row.Cells[4].Text.Trim().ToUpper() + "' and trim(nvl(curr_Stat,'-'))='-' and " + add_filt + " GROUP BY TO_CHAR(TRCDT,'MON-YY'),TO_CHAR(TRCDT,'YYYYMM')) order by VDD ";

                lblSg1.Text = "List of Assigned Tasks";
                lblSg2.Text = "Tasks Pending (All. Team Member)";
                lblSg3.Text = "Tasks Pending (Curr Team Member) " + row.Cells[4].Text.Trim();
                lblSg4.Text = " - ";

                graphType1 = "pie";
                graphType2 = "column";

                if (frm_ulvl != "0")
                {
                    graphType1 = "bar";
                    lblSg2.Text = "Tasks Done (Curr Team Member)";
                    add_filt = "Upper(trim(team_member))='" + frm_uname + "'";
                    SQuery1 = "select YR,Task_Count  from (SELECT TO_CHAR(TRCDT,'MON-YY') AS YR,count(*) AS Task_Count,TO_CHAR(TRCDT,'YYYYMM') as VDD FROM wb_task_log WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE 'TR%' AND TRCDT " + PrdRange + " and trim(upper(Team_member))='" + row.Cells[4].Text.Trim().ToUpper() + "' and trim(nvl(curr_Stat,'-'))!='-' and " + add_filt + " GROUP BY TO_CHAR(TRCDT,'MON-YY'),TO_CHAR(TRCDT,'YYYYMM')) order by VDD ";
                    SQuery2 = "select YR,Task_Count  from (SELECT TO_CHAR(TRCDT,'MON-YY') AS YR,count(*) AS Task_Count,TO_CHAR(TRCDT,'YYYYMM') as VDD FROM wb_task_log WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE 'TR%' AND TRCDT " + PrdRange + " and trim(upper(Team_member))='" + row.Cells[4].Text.Trim().ToUpper() + "' and trim(nvl(curr_Stat,'-'))='-' and " + add_filt + " GROUP BY TO_CHAR(TRCDT,'MON-YY'),TO_CHAR(TRCDT,'YYYYMM')) order by VDD ";
                }


                break;

            case "F47159":
                SQuery1 = "SELECT a.ordno as Fstr,a.qtyord as Order_Qty,a.Ciname AS Item_Name,a.cpartno as Item_Code,A.ICODE AS ERPCODE,a.srno FROM somas A WHERE A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "' order by a.srno";
                SQuery2 = "Select to_char(Max(a.Vchdate),'dd/mm/yyyy') as Last_Disp,b.Iname,b.Cpartno from ivoucher a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + DateRange + " and trim(A.icode) in (SELECT trim(A.ICODE) AS ERPCODE FROM somas A WHERE A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "') and A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')!='" + row.Cells[1].Text.Trim() + "' group by b.Iname,b.Cpartno order by b.iname ";
                SQuery3 = "Select to_char(Max(a.Vchdate),'dd/mm/yyyy') as Last_Prodn,b.Iname,b.Cpartno from ivoucher a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '1%' and a.type>'14' and a.vchdate " + DateRange + " and trim(A.icode) in (SELECT trim(A.ICODE) AS ERPCODE FROM somas A WHERE A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "') group by b.Iname,b.Cpartno order by b.iname ";

                lblSg1.Text = "List of Sales Orders";
                lblSg2.Text = "Item Wise Details for Order # " + row.Cells[2].Text.Trim();
                lblSg3.Text = "Last Dispatch of Item for Order # " + row.Cells[2].Text.Trim();
                lblSg4.Text = "Last Production of Item for Order #" + row.Cells[2].Text.Trim();

                break;

            case "F50101":
                break;

            case "F35108G":
                SQuery1 = "SELECT a.icode as Fstr,a.col1 as srno,a.col2 as desc_,a.col3 as specs,a.col4 as multiply,a.col5 as qty_req,a.col6 as extra,A.col9 AS ERPCODE FROM costestimate A WHERE A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "' order by a.srno";
                SQuery2 = "select y.name as Stage_Name/*,sum(x.prod) as Production,sum(x.rej) as Rejection*/,sum(x.prod-x.rej) as Net_Prodn from (select a.srno,a.fm_Fact,a.stagec,a.icode,nvl(b.tot,0) as tot,nvl(b.prod,0) as prod ,nvl(b.rej,0) as rej,b.vchdate from itwstage a left outer join (select decode(trim(stage),'-','01',stage) as stage,icode,sum(nvl(decode(type,'85',iqtyout+nvl(a4,0),a2+nvl(a4,0)),0)) as tot,sum(nvl(decode(type,'85',iqtyout+nvl(a4,0),a2+nvl(a4,0)),0)) as prod,sum(nvl(a4,0)) as rej,Vchdate from PROD_SHEET  where branchcd='" + frm_mbr + "' and type in('85','88','86') and job_no||job_dt='" + row.Cells[2].Text.Trim() + row.Cells[3].Text.Trim() + "' and stage<>'08' group by stage,icode,Vchdate  union all Select '08' as stage,x.icode,sum(x.iqtyin) as Prodn,sum(x.iqtyin+nvl(x.rej_rw,0)) as Prodn,sum(x.rej_rw) as rejn ,Vchdate from ivoucher x where x.branchcd='" + frm_mbr + "' and (x.type='15' or x.type='16') and trim(x.invno)||trim(to_Char(x.invdate,'dd/mm/yyyy'))='" + row.Cells[2].Text.Trim() + row.Cells[3].Text.Trim() + "' and trim(x.icode)='" + row.Cells[4].Text.Trim() + "' group by x.icode,x.vchdate) b on trim(a.stagec)=trim(b.stage) where trim(a.icode)='" + row.Cells[4].Text.Trim() + "' order by a.icode,a.srno) x ,(Select type1,name,NVL(EXC_TARRIF,'-') AS IGNORESTG from type where id='K') y,item z where x.stagec=y.type1 and trim(x.icode)=trim(z.icode) group by y.name,x.icode,x.stagec,x.fm_fact,x.srno,Y.IGNORESTG order by x.icode,x.srno";
                SQuery3 = "";
                graphType1 = "column";
                graphType2 = "column";

                lblSg1.Text = "List of Job Card";
                lblSg2.Text = "Item Wise Details for Job Card # " + row.Cells[2].Text.Trim();
                lblSg3.Text = "Process Wise Prodn Status # " + row.Cells[2].Text.Trim();
                lblSg4.Text = "- #" + row.Cells[2].Text.Trim();
                break;
            default:
                SQuery1 = "SELECT A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO FROM POMAS A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) AND A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_cHAR(A.ORdDT,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "' order by a.icode";
                SQuery2 = SQuery1;
                SQuery3 = SQuery1;

                lblSg2.Text = "Details of Purchase Orders # " + row.Cells[2].Text.Trim();
                lblSg3.Text = "Details of Purchase Orders # " + row.Cells[2].Text.Trim();
                lblSg4.Text = "Details of Purchase Orders # " + row.Cells[2].Text.Trim();
                break;
        }

        if (SQuery1.Length > 0)
        {
            sg2_dt = new DataTable();
            sg2_dt = fgen.getdata(frm_qstr, frm_cocd, SQuery1);

            chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle1, graphType1, stitle1, val_legnd1, SQuery1, val_legnd1, "chart2", "", "");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart1", chartScript, false);
            //sg2.DataSource = sg2_dt;
            //sg2.DataBind();
        }
        if (SQuery2.Length > 0)
        {
            sg3_dt = new DataTable();
            sg3_dt = fgen.getdata(frm_qstr, frm_cocd, SQuery2);

            chartScript = fgen.Fn_FillChart(frm_cocd, frm_qstr, stitle2, graphType2, stitle2, val_legnd2, SQuery2, val_legnd2, "chart3", "", "");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Chart3", chartScript, false);
            //sg3.DataSource = sg3_dt;
            //sg3.DataBind();
        }
        if (SQuery3.Length > 0)
        {
            sg4_dt = new DataTable();
            sg4_dt = fgen.getdata(frm_qstr, frm_cocd, SQuery3);

            //sg4.DataSource = sg4_dt;
            //sg4.DataBind();
        }
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
}