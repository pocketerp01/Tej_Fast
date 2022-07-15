using System;
using System.Data;
using System.Web;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_dbd_mgrid : System.Web.UI.Page
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
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond, party_cd, part_cd;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    double double_val2, double_val1;
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

                btnnew_ServerClick(null, null);

            }
            set_Val();

            btnedit.Visible = false;
            btndel.Visible = false;
            btnsave.Visible = false;
            btnprint.Visible = false;
            btnlist.Visible = false;

        }
        txtSearch.Enabled = true;
        setGridWidth();
        btnExpToExcel.Enabled = true;
    }
    void setGridWidth()
    {
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
            if (wid > 2000) wid = 2000;
            try { sg1.Width = Convert.ToUInt16(wid + 100); }
            catch { sg1.Width = 1500; }

            //if (sg1.Width.Value <= 800) sg1.Width = Unit.Percentage(100);
        }
        if (sg2.Rows.Count > 0)
        {
            wid = 0;
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
            wid = 0;
            col_count = sg3.HeaderRow.Cells.Count;
            for (int i = 0; i < col_count; i++)
            {
                ad = 10;
                if (sg3.Rows[0].Cells[i].Text.Length < 2) ad = 8;
                wid += fgen.make_double(sg3.Rows[0].Cells[i].Text.Length, 0) * ad;
            }
            if (wid > 800) wid = 500;
            try { sg3.Width = Convert.ToUInt16(wid + 100); }
            catch { sg3.Width = 500; }

            //if (sg3.Width.Value <= 800 || sg3.Width.Value > 2000) sg3.Width = Unit.Percentage(100);
        }
        if (sg4.Rows.Count > 0)
        {
            wid = 0;
            col_count = sg4.HeaderRow.Cells.Count;
            for (int i = 0; i < col_count; i++)
            {
                ad = 10;
                if (sg4.Rows[0].Cells[i].Text.Length < 2) ad = 8;
                wid += fgen.make_double(sg4.Rows[0].Cells[i].Text.Length, 0) * ad;
            }
            if (wid < 500) wid = 500;
            try { sg4.Width = Convert.ToUInt16(wid + 100); }
            catch { sg4.Width = 1500; }

            //if (sg4.Width.Value <= 800 || sg4.Width.Value > 2000) sg4.Width = Unit.Percentage(100);
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
            case "F40122":

            case "F35108":

                lblheader.Text = "Job Order Status Report";
                btnClose.Visible = true;
                //gridDiv1.Style.Add("height", "205px");
                //gridDiv3.Style.Add("height", "205px");
                //grid4.Visible = false;
                break;
            default:
                lblheader.Text = "Data Review System (DRS)";
                btnClose.Visible = false;
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
        //chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        //if (chk_rights == "Y")
        if (1 == 1)
        {
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            frm_tabname = "WB_CSS_LOG";
            switch (Prg_Id)
            {
                case "F15245":
                case "F25234":
                case "F50135":
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
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
        todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        SQuery = "";
        string addl_cond = "";
        switch (frm_formID)
        {

            case "F15245":
            case "F25234":
            case "F50135":
                string xprd1 = "";
                string xprd2 = "";
                string mq0 = "";
                string mq1 = "";
                string mq2 = "";
                string mq3 = "";
                string fg_Cd = "";
                if (frm_formID == "F50135")
                {
                    fg_Cd = "9";
                }
                string my_cyear = "";
                my_cyear = "yr_" + frm_CDT1.Substring(6, 4);

                xprd1 = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1";
                xprd2 = " between to_date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";

                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                mq0 = "select trim(A.icode)as Fstr,b.Iname as Item_Name,b.Cpartno as Part_No,sum(a.opening) as Opening,sum(a.cdr) as Rcpts,sum(a.ccr) as Issues,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,max(a.imin) As Min_lvl,max(qap) as Qa_pending,b.Unit,trim(a.icode) as Icode from (Select icode, " + my_cyear + " as opening,0 as cdr,0 as ccr,0 as clos,nvl(imin,0) as imin,0 as qap from ITEMBAL where branchcd='" + frm_mbr + "'  union all  ";
                mq1 = "select icode,0 as op,0 as cdr,0 as ccr,0 as clos,0 as xmin,sum(iqty_chl) as qap from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + xprd2 + " and store='N' GROUP BY ICODE union all ";
                mq2 = "select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as xmin,0 as qap from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd1 + " and store='Y' GROUP BY ICODE union all ";
                mq3 = "select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as xmin,0 as qap  from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd2 + " and store='Y'  GROUP BY ICODE )a ,item b where trim(A.icode)=trim(B.icodE) and substr(A.icode,1,1) like '" + fg_Cd + "%' and substr(A.icode,1,2) like '" + party_cd + "%' and substr(A.icode,1,4) like '" + part_cd + "%' group by b.iname,b.cpartno,b.unit,trim(a.icode),substr(a.icode,1,4) having sum(opening)+sum(cdr)+sum(ccr)<>0 order by substr(a.icode,1,4),b.iname";
                //SQuery = "create or replace view MS_STK_" + frm_mbr + " as(SELECT * FROM (" + mq0 + mq1 + mq2 + "))" ;
                SQuery = mq0 + mq1 + mq2 + mq3;
                lblSg1.Text = "Stock Summary + Analysis";
                lblSg2.Text = "Monthly Movement of Selected Item";
                lblSg3.Text = "Last P.O. of Selected Item";
                lblSg4.Text = "Last Issue of Selected Item";
                if (frm_formID == "F50135")
                {
                    lblSg3.Text = "Last S.O. of Selected Item ";
                    lblSg4.Text = "Last Sale of Selected Item ";
                }
                break;

            case "F10174":
                SQuery = "SELECT DISTINCT TRIM(A.icode) AS FSTR,A.Icode AS ERP_code,a.Iname AS Item_Name,a.Cpartno,a.Cdrgno,a.Unit,TO_CHAR(A.ent_Dt,'YYYYMMDD') AS VDD FROM item a WHERE length(Trim(A.icode))>=8 ORDER BY a.Icode";

                lblSg1.Text = "Master List of Items";
                lblSg2.Text = "BOM Details of Selected Item";
                lblSg3.Text = "P.O.Details of Selected Item";
                lblSg4.Text = "MRR.Details of Selected Item";
                break;

            case "F15159":
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY') AS FSTR,A.Ordno AS PO_NO,TO_cHAR(A.orddt,'DD/MM/YYYY') AS PO_Date,B.ANAME AS Supplier,a.RATE_CD as PO_Value,a.app_by,A.ACODE AS CODE,a.type,TO_CHAR(A.orddt,'YYYYMMDD') AS VDD FROM pomas A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '5%' AND A.orddt " + PrdRange + " ORDER BY VDD DESC,a.ordno desc,a.type";

                lblSg1.Text = "Details of Purchase Orders";
                lblSg2.Text = "Item Wise Details for Order # ";
                lblSg3.Text = "Last PR of Item for Order # ";
                lblSg4.Text = "Last MRR of Item for Order #";
                break;
            case "F20159":
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY') AS FSTR,A.Vchnum AS GE_NO,TO_cHAR(A.vchdate,'DD/MM/YYYY') AS GE_Date,B.ANAME AS Supplier,a.Mtime,a.Invno,to_Char(a.invdate,'dd/mm/yyyy') as Inv_dt,A.ACODE AS CODE,a.type,TO_CHAR(A.vchdate,'YYYYMMDD') AS VDD FROM ivoucherp A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '0%' AND A.vchdate " + PrdRange + " ORDER BY VDD DESC,a.vchnum desc,a.type";

                lblSg1.Text = "Details of Gate Inwards";
                lblSg2.Text = "Details of Selected Gate Inward";
                lblSg3.Text = "Last PO of Items for G.E. # ";
                lblSg4.Text = "Last MRR of Item for G.E. #";
                break;

            case "F25193":
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY') AS FSTR,A.Vchnum AS MRR_NO,TO_cHAR(A.vchdate,'DD/MM/YYYY') AS MRR_dT,B.ANAME AS Supplier,A.ACODE AS CODE,a.type,TO_CHAR(A.vchdate,'YYYYMMDD') AS VDD FROM ivoucher A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '0%' AND A.vchdate " + PrdRange + " and a.store in ('Y','N') ORDER BY VDD DESC,a.vchnum desc,a.type";

                lblSg1.Text = "Details of MRR/GRN Inwards";
                lblSg2.Text = "Details of Selected MRR/GRN ";
                lblSg3.Text = "Last P.O. of Items for MRR. # ";
                lblSg4.Text = "Last G.E. of Items for MRR. #";
                break;

            case "F25191":
                SQuery = "SELECT DISTINCT TRIM(A.icode) AS FSTR,A.Icode AS ERP_code,a.Iname AS Item_Name,a.Cpartno,a.Cdrgno,a.Unit,TO_CHAR(A.ent_Dt,'YYYYMMDD') AS VDD FROM item a WHERE length(Trim(A.icode))>=8 ORDER BY a.Icode";

                lblSg1.Text = "Details of Items";
                lblSg2.Text = "Details of Selected Gate Inward";
                lblSg3.Text = "Details of Purchase Orders";
                lblSg4.Text = "Details of Purchase Orders";
                break;



            case "F30159":
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY') AS FSTR,A.Vchnum AS MRR_NO,TO_cHAR(A.vchdate,'DD/MM/YYYY') AS MRR_dT,B.ANAME AS Supplier,(case when a.pname!='-' then 'Inspected' else 'Pending' end) as Insp_Stat,A.ACODE AS CODE,a.type,TO_CHAR(A.vchdate,'YYYYMMDD') AS VDD FROM ivoucher A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '0%' AND A.vchdate " + PrdRange + " and a.store in ('Y','N') ORDER BY VDD DESC,a.vchnum desc,a.type";

                lblSg1.Text = "Details of MRR/GRN";
                lblSg2.Text = "Details of Selected MRR/GRN";
                lblSg3.Text = "Details of QA Rejection of Such Items";
                lblSg4.Text = "Details of Line Rejection of Such Items";
                break;
            case "F47159":
            case "F49159":
            case "F50136":
                addl_cond = "1=1";
                if (frm_formID == "F49159")
                {
                    addl_cond = "a.type='4F'";
                }
                SQuery = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_cHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR,A.ORDNO AS SO_NO,TO_cHAR(A.ORDDT,'DD/MM/YYYY') AS SO_Date,B.ANAME AS CUSTOMER,sum(a.qtyord) as SO_qty,sum(a.soldqty) as Disp_Qty,sum(A.bal_Qty) as bal_Qty,a.type,TO_CHAR(A.ORDDT,'YYYYMMDD') AS VDD FROM wbvu_pending_so A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '4%' AND A.ORDDT " + PrdRange + " and " + addl_cond + " group by A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_cHAR(A.ORDDT,'DD/MM/YYYY'),A.ORDNO,TO_cHAR(A.ORDDT,'DD/MM/YYYY'),B.ANAME,a.type,TO_CHAR(A.ORDDT,'YYYYMMDD') ORDER BY VDD DESC,a.ordno desc,a.type";
                //SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_cHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR,A.ORDNO AS SO_NO,TO_cHAR(A.ORDDT,'DD/MM/YYYY') AS SO_Date,B.ANAME AS CUSTOMER,A.ACODE AS CODE,a.type,TO_CHAR(A.ORDDT,'YYYYMMDD') AS VDD FROM SOMAS A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '4%' AND A.ORDDT " + PrdRange + " ORDER BY VDD DESC,a.ordno desc,a.type";

                lblSg1.Text = "List of Sales Orders";
                lblSg2.Text = "Item Wise Details for Order # ";
                lblSg3.Text = "Last Dispatch of Item for Order # ";
                lblSg4.Text = "Last Production of Item for Order #";
                break;
            case "F50159":
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY') AS FSTR,A.Vchnum AS Inv_No,TO_cHAR(A.vchdate,'DD/MM/YYYY') AS Inv_Dt,B.ANAME AS Customer,a.Bill_tot as Total_Amt,A.Mo_Vehi as Vehicle_no,A.Invtime as Inv_Time,A.ACODE AS CODE,a.type,TO_CHAR(A.vchdate,'YYYYMMDD') AS VDD FROM sale A, FAMST B WHERE TRIM(A.ACODe)=TRIM(b.ACODe) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '4%' AND A.vchdate " + PrdRange + " ORDER BY VDD DESC,a.vchnum desc,a.type";

                lblSg1.Text = "List of Invoices";
                lblSg2.Text = "Item Wise Details for Invoice # ";
                lblSg3.Text = "Last Dispatch of Item for Invoice # ";
                lblSg4.Text = "Last Production of Item for Invoice #";

                break;

            case "F50101":
                break;
            case "F40122":

            case "F35108":
                SQuery = "SELECT DISTINCT A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY') AS FSTR,A.Vchnum AS Job_No,TO_cHAR(A.vchdate,'DD/MM/YYYY') AS JoB_Dt,a.icode as erpcode,b.iname as product,b.cpartno,b.unit,A.qty ,a.type,TO_CHAR(A.vchdate,'YYYYMMDD') AS VDD FROM costestimate A, item B WHERE TRIM(A.icode)=TRIM(b.icode) AND A.BRANCHCD='" + frm_mbr + "' and a.type like '30%' AND A.vchdate " + PrdRange + " and a.srno=0 ORDER BY VDD DESC,a.vchnum desc,a.type";

                SQuery = "Select '" + frm_mbr + "'||'30'||trim(M.Job_no)||to_char(M.dated,'dd/mm/yyyy') as fstr, M.Job_no,to_char(M.dated,'dd/mm/yyyy') as job_dt,trim(N.iname)||' '||trim(m.btchno) as Item_name,M.Job_qty,M.Prodn,M.Done,M.status,M.TOT_SHEET,M.issu,M.acode,m.REJALL,decode(trim(m.Iscancel),'Y','Cancel','N') as Iscancel,m.closeby,m.cancelby,m.JStatus,N.Cpartno as Part_No,m.Supcl_BY,m.col18 as PWidth,m.col19 as PLength,m.nups,m.icode as erpcode,substr(m.fstr,1,20) as solink from (Select X.Job_no,X.dated,'-' as Part_No,'-' as Item_name,X.qty as Job_qty,Nvl(y.prodn,0) as Prodn,round((Nvl(y.prodn,0)/(cASE WHEN X.qty=0 THEN 1 ELSE X.QTY END))*100,0)||'%' as Done, x.JStatus,x.status,x.Iscancel,x.az_by,x.az_Dt,X.COL14 AS TOT_SHEET,x.issu,X.icode,x.acode,x.convdate as fstr,x.picode,x.REJALL,x.closeby,x.cancelby,x.supcl_BY,x.btchno,x.col18,x.col19,x.nups,x.col24,x.col12,x.enqdt from (select A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Qty,a.icode,a.enqno as Iscancel,decode(a.JStatus,'Y','SupClose','U/JProcess') as JStatus,decode(a.Status,'Y','Complete','U/Process') as Status,a.acode,a.convdate,a.az_by,to_char(a.az_Dt,'dd/mm/yyyy') as az_dt,TO_NUMBER(A.COL14)*TO_NUMBER(A.COL13) AS COL14,ROUND((NVL(C.ISS,0)-TO_NUMBER(A.COL15))*TO_NUMBER(A.COL13),2) AS ISSU,TO_NUMBER(A.COL15) AS REJALL,a.picode,a.attach as Closeby,a.attach2 as cancelby,a.Supcl_BY,trim(a.col20) as btchno,a.col18,a.col19,a.col13 as nups,a.col24,a.col12,a.enqdt  from costestimate A LEFT OUTER JOIN (sELECT job_no,job_dt,SUM(A5) AS ISS FROM PROD_SHEET  WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='85' AND VCHDATE >=TO_dATe('" + fromdt + "','DD/MM/YYYY') and substr(icode,1,2)!='07**' GROUP BY job_no,job_dt) C ON A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(C.job_no)||TRIM(C.job_dt) WHERE a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + DateRange + " and A.VCHNUM LIKE '%' AND A.SRNO=1 AND a.acode like '%%' and a.icode like '%%' and 1=1) x left outer join (select trim(icode) as icode,trim(invno) as invno,sum(iqtyin) as prodn from ivoucher where branchcd='" + frm_mbr + "' and (type='15' OR type='16') and vchdate>=to_DatE('" + fromdt + "','dd/mm/yyyy') group by trim(icode),trim(invno)) y on trim(x.icode)=trim(y.icode) and trim(x.job_no)=trim(y.invno)  ) M left outer join item N on trim(M.icode)=trim(N.icode) where substr(m.icode,1,2) like '%%' order by M.Dated desc ,M.job_no desc";
                SQuery = "Select '" + frm_mbr + "'||'30'||trim(M.Job_no)||to_char(M.dated,'dd/mm/yyyy') as fstr, M.Job_no,to_char(M.dated,'dd/mm/yyyy') as job_dt,trim(N.iname)||' '||trim(m.btchno) as Item_name,M.Job_qty,M.Prodn,M.Done,M.status,M.TOT_SHEET,M.issu,M.acode,m.REJALL,decode(trim(m.Iscancel),'Y','Cancel','N') as Iscancel,m.closeby,m.cancelby,m.JStatus,N.Cpartno as Part_No,m.Supcl_BY,m.col18 as PWidth,m.col19 as PLength,m.nups,m.icode as erpcode,substr(m.fstr,1,20) as solink,substr(m.fstr,4,6) as so_no,f.aname as customer from (Select X.Job_no,X.dated,'-' as Part_No,'-' as Item_name,X.qty as Job_qty,Nvl(y.prodn,0) as Prodn,round((Nvl(y.prodn,0)/(cASE WHEN X.qty=0 THEN 1 ELSE X.QTY END))*100,0)||'%' as Done, x.JStatus,x.status,x.Iscancel,x.az_by,x.az_Dt,X.COL14 AS TOT_SHEET,x.issu,X.icode,x.acode,x.convdate as fstr,x.picode,x.REJALL,x.closeby,x.cancelby,x.supcl_BY,x.btchno,x.col18,x.col19,x.nups,x.col24,x.col12,x.enqdt from (select A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Qty,a.icode,a.enqno as Iscancel,decode(a.JStatus,'Y','SupClose','U/JProcess') as JStatus,decode(a.Status,'Y','Complete','U/Process') as Status,a.acode,a.convdate,a.az_by,to_char(a.az_Dt,'dd/mm/yyyy') as az_dt,TO_NUMBER(A.COL14)*TO_NUMBER(A.COL13) AS COL14,ROUND((NVL(C.ISS,0)-TO_NUMBER(A.COL15))*TO_NUMBER(A.COL13),2) AS ISSU,TO_NUMBER(A.COL15) AS REJALL,a.picode,a.attach as Closeby,a.attach2 as cancelby,a.Supcl_BY,trim(a.col20) as btchno,a.col18,a.col19,a.col13 as nups,a.col24,a.col12,a.enqdt  from costestimate A LEFT OUTER JOIN (sELECT job_no,job_dt,SUM(A5) AS ISS FROM PROD_SHEET  WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='85' AND VCHDATE >=TO_dATe('" + fromdt + "','DD/MM/YYYY') and substr(icode,1,2)!='07**' GROUP BY job_no,job_dt) C ON A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(C.job_no)||TRIM(C.job_dt) WHERE a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + PrdRange + "  and A.VCHNUM LIKE '%' AND A.SRNO=1 AND a.acode like '%%' and a.icode like '%%' and 1=1) x left outer join (select trim(icode) as icode,trim(invno) as invno,sum(iqtyin) as prodn from ivoucher where branchcd='" + frm_mbr + "' and (type='15' OR type='16') and vchdate>=to_DatE('" + fromdt + "','dd/mm/yyyy')  and store<>'W' group by trim(icode),trim(invno)) y on trim(x.icode)=trim(y.icode) and trim(x.job_no)=trim(y.invno)  ) M left outer join item N on trim(M.icode)=trim(N.icode), famst f where trim(m.acodE)=trim(f.acodE) and substr(m.icode,1,2) like '%%' order by M.Dated desc ,M.job_no desc";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_JOBQUERY", SQuery);

                lblSg1.Text = "List of Job Card";
                lblSg2.Text = "Item Wise Details for Job Card # ";
                lblSg3.Text = "Process Wise Prodn Status # ";
                lblSg4.Text = "- #";

                sg2.DataSource = null; sg2.DataBind();
                sg3.DataSource = null; sg3.DataBind();
                sg4.DataSource = null; sg4.DataBind();
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
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_JOBQUERY", SQuery);
            sg1_dt = new DataTable();
            sg1_dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            ViewState["sg1"] = sg1_dt;

            lblTotcount.Text = "Total Rows : " + sg1_dt.Rows.Count;

            switch (frm_formID)
            {
                case "F40122":


                case "F35108":
                    for (int i = 0; i < sg1.Rows.Count; i++)
                    {
                        if (sg1.Rows[i].Cells[8].Text.Trim().ToUpper() == "COMPLETE")
                        {
                            sg1.Rows[i].Cells[2].BackColor = Color.Red;
                        }
                    }
                    break;
            }
        }
        setGridWidth();
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
        else if (hffield.Value == "JOBCLOSE")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                hffield.Value = "CJOBCLOSE";
                fgen.Fn_ValueBox("Please Fill Closing Reason!!", frm_qstr);
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
                    }
                    #endregion
                    break;
                case "CJOBCLOSE":
                    string scode = fgenMV.Fn_Get_Mvar(frm_qstr, "U_JOBCARD");
                    int index = fgen.make_int(fgenMV.Fn_Get_Mvar(frm_qstr, "SG1_INDEX"));
                    if (scode.Length > 6)
                    {
                        SQuery = "update costestimate set clo_dt=SYSDATE ,attach='" + frm_UserID + " " + vardate + "',status='Y' where branchcd='" + frm_mbr + "' and type='30' and vchnum||to_char(vchdate,'dd/mm/yyyy')='" + scode + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        SQuery = "update costestimate set comments5='" + col1 + "',clo_dt=SYSDATE,attach='" + frm_UserID + " " + vardate + "',status='Y' where srno=0 and branchcd='" + frm_mbr + "' and type='30' and vchnum||to_char(vchdate,'dd/mm/yyyy')='" + scode + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                        sg1.Rows[index].Cells[2].BackColor = Color.Red;

                        fgen.msg("-", "AMSG", "Job Card No " + scode.Substring(0, 6) + " has been marked as Closed!!");
                    }
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
        for (int i = 0; i < 15; i++)
        {
            sg3_dt.Columns.Add(new DataColumn("sg3_t" + (i + 1), typeof(string)));
        }
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

            z = 0;
            for (int i = z; i < e.Row.Cells.Count - 1; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.ToolTip = "You can click this cell";
                cell.Attributes["ondblclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}", SelectedGridCellIndex.ClientID, i, Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }

            e.Row.Attributes["onkeypress"] = "if (event.keyCode == 13) {" + ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex) + ";}";

            sg1.HeaderRow.Cells[0].Style["display"] = "none";
            sg1.HeaderRow.Cells[1].Style["display"] = "none";

            e.Row.Cells[0].Style["display"] = "none";
            e.Row.Cells[1].Style["display"] = "none";
        }
    }
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            sg2.HeaderRow.Cells[0].Style["display"] = "none";

            e.Row.Cells[0].Style["display"] = "none";

            for (int i = z; i < e.Row.Cells.Count - 1; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.ToolTip = "You can click this cell";
                cell.Attributes["ondblclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}", SelectedGridCellIndex2.ClientID, i, Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }
    protected void sg3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            sg3.HeaderRow.Cells[0].Style["display"] = "none";
            e.Row.Cells[0].Style["display"] = "none";

            for (int i = z; i < e.Row.Cells.Count - 1; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.ToolTip = "You can click this cell";
                cell.Attributes["ondblclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}", SelectedGridCellIndex3.ClientID, i, Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }

            switch (frm_formID)
            {
                case "F40122":
                    sg3.HeaderRow.Cells[1].Style["display"] = "none";
                    e.Row.Cells[1].Style["display"] = "none";

                    sg3.HeaderRow.Cells[2].Text = "Item Name";
                    sg3.HeaderRow.Cells[3].Text = "Unit";
                    sg3.HeaderRow.Cells[4].Text = "Issue";
                    sg3.HeaderRow.Cells[5].Text = "Rcvd";
                    sg3.HeaderRow.Cells[6].Text = "Cut";
                    sg3.HeaderRow.Cells[7].Text = "C/Sheet";
                    sg3.HeaderRow.Cells[8].Text = "R/Sheet";
                    sg3.HeaderRow.Cells[9].Text = "Remarks";
                    sg3.HeaderRow.Cells[10].Text = "Ok Sheet";

                    for (int i = 11; i < 16; i++)
                    {
                        sg3.HeaderRow.Cells[i].Style["display"] = "none";
                        e.Row.Cells[i].Style["display"] = "none";
                    }

                    break;
            }
        }
    }
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


            for (int i = z; i < e.Row.Cells.Count - 1; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.ToolTip = "You can click this cell";
                cell.Attributes["ondblclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}", SelectedGridCellIndex4.ClientID, i, Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }

            sg4.HeaderRow.Cells[0].Style["display"] = "none";
            e.Row.Cells[0].Style["display"] = "none";
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
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
    protected void sg1_SelectedIndexChanged(object sender, EventArgs e)
    {
        string addl_cond = "";
        var grid = (GridView)sender;
        GridViewRow row = sg1.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        if (selectedCellIndex < 0) selectedCellIndex = 0;
        string mq0 = sg1.HeaderRow.Cells[selectedCellIndex].Text.Replace("<br/>", " "); // dynamic heading        
        if (selectedCellIndex > 0) selectedCellIndex -= 1;

        SQuery1 = "";
        SQuery2 = "";
        SQuery3 = "";
        switch (frm_formID)
        {
            case "F10174":
                SQuery1 = "SELECT distinct b.iname AS Item_Name,a.ibqty as BOM_Qty,b.unit,b.cpartno as Item_Code,A.ICODE AS ERPCODE,a.srno FROM itemosp A ,item b WHERE trim(a.icode)=trim(B.icode) and trim(A.ibcode)='" + row.Cells[1].Text.Trim() + "' order by b.iname";
                SQuery2 = "Select to_char(Max(a.orddt),'dd/mm/yyyy') as Last_PR,b.Iname,sum(a.qtyord) as PO_Qty,b.unit,b.Cpartno from POMAS a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + DateRange + " and trim(A.icode) in (SELECT trim(A.icode) AS ERPCODE FROM item A WHERE trim(a.icode)='" + row.Cells[1].Text.Trim() + "')  group by b.Iname,b.unit,b.Cpartno order by b.iname ";
                SQuery3 = "Select to_char(Max(a.vchdate),'dd/mm/yyyy') as Last_MRR,b.Iname,sum(a.iqtyin) as MRR_Qty,b.unit,b.Cpartno from ivoucher a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + DateRange + " and a.store!='R' and trim(A.icode) in (SELECT trim(A.icode) AS ERPCODE FROM ITEM A WHERE trim(A.icode)='" + row.Cells[1].Text.Trim() + "')  group by b.Iname,b.unit,b.Cpartno order by b.iname ";

                lblSg1.Text = "Master List of Items";
                lblSg2.Text = "BOM Details of Selected Item " + row.Cells[2].Text.Trim();
                lblSg3.Text = "P.O.Details of Selected Item " + row.Cells[2].Text.Trim();
                lblSg4.Text = "MRR.Details of Selected Item " + row.Cells[2].Text.Trim();
                break;

            case "F15159":
                SQuery1 = "SELECT a.Ciname AS Item_Name,a.qtyord as Order_Qty,b.unit,b.cpartno as Item_Code,A.ICODE AS ERPCODE,a.srno FROM Pomas A ,item b WHERE trim(a.icode)=trim(B.icode) and A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "' order by a.srno";
                SQuery2 = "Select to_char(Max(a.orddt),'dd/mm/yyyy') as Last_PR,b.Iname,sum(a.qtyord) as pr_Qty,b.unit,b.Cpartno from POMAS a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '60%' and a.orddt " + DateRange + " and trim(A.icode)||trim(a.ordno)||to_Char(A.orddt,'dd/mm/yyyy') in (SELECT trim(A.icode)||trim(a.pr_no)||to_Char(A.pr_dt,'dd/mm/yyyy') AS ERPCODE FROM Pomas A WHERE A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "')  group by b.Iname,b.unit,b.Cpartno order by b.iname ";
                SQuery3 = "Select to_char(Max(a.vchdate),'dd/mm/yyyy') as Last_MRR,b.Iname,sum(a.iqtyin) as MRR_Qty,b.unit,b.Cpartno from ivoucher a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + DateRange + "  and a.store!='R' and trim(A.icode)||trim(a.ponum)||to_Char(A.podate,'dd/mm/yyyy') in (SELECT trim(A.icode)||trim(a.ordno)||to_Char(A.orddt,'dd/mm/yyyy') AS ERPCODE FROM Pomas A WHERE A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "')  group by b.Iname,b.unit,b.Cpartno order by b.iname ";

                lblSg1.Text = "Details of Purchase Orders";
                lblSg2.Text = "Item Wise Details for Order # " + row.Cells[2].Text.Trim();
                lblSg3.Text = "Last PR of Item for Order # " + row.Cells[2].Text.Trim();
                lblSg4.Text = "Last MRR of Item for Order #" + row.Cells[2].Text.Trim();

                if (selectedCellIndex == 1)
                {
                    col1 = row.Cells[2].Text.Trim() + row.Cells[3].Text.Trim();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", row.Cells[8].Text.Trim());
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + col1 + "'");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1004");
                    fgen.fin_purc_reps(frm_qstr);
                    return;
                }
                break;
            case "F20159":
                SQuery1 = "SELECT b.Iname AS Item_Name,a.iqty_chl as GE_Qty,b.unit,b.cpartno as Item_Code,A.ICODE AS ERPCODE,a.srno FROM ivoucherp A ,item b WHERE trim(a.icode)=trim(B.icode) and A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "' order by a.srno";
                SQuery2 = "Select to_char(Max(a.orddt),'dd/mm/yyyy') as Last_PR,b.Iname,sum(a.qtyord) as PO_Qty,b.unit,b.Cpartno from POMAS a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + DateRange + " and trim(A.icode)||trim(a.ordno)||to_Char(A.orddt,'dd/mm/yyyy') in (SELECT trim(A.icode)||trim(a.ponum)||to_Char(A.podate,'dd/mm/yyyy') AS ERPCODE FROM ivoucherp A WHERE A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "')  group by b.Iname,b.unit,b.Cpartno order by b.iname ";
                SQuery3 = "Select to_char(Max(a.vchdate),'dd/mm/yyyy') as Last_MRR,b.Iname,sum(a.iqtyin) as MRR_Qty,b.unit,b.Cpartno from ivoucher a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + DateRange + "  and a.store!='R' and trim(A.icode)||trim(a.genum)||to_Char(A.gedate,'dd/mm/yyyy') in (SELECT trim(A.icode)||trim(a.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') AS ERPCODE FROM ivoucherp A WHERE A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "')  group by b.Iname,b.unit,b.Cpartno order by b.iname ";

                lblSg1.Text = "Details of Gate Inwards";
                lblSg2.Text = "Details of Selected Gate Inward " + row.Cells[2].Text.Trim();
                lblSg3.Text = "Last PO of Items for G.E. # " + row.Cells[2].Text.Trim();
                lblSg4.Text = "Last MRR of Item for G.E. #" + row.Cells[2].Text.Trim();

                break;

            case "F15245":
            case "F25234":
            case "F50135":
                SQuery1 = "Select to_char(a.vchdate,'MONTH') as Mth_name,sum(a.iqtyin) as Rcpt_Qty,sum(a.iqtyout) as Issue_Qty,to_char(a.vchdate,'YYYYMM') as mths from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '%' and a.vchdate " + DateRange + " and trim(A.icode)  in (SELECT distinct trim(A.icode) AS ERPCODE FROM item A WHERE trim(a.icode)='" + row.Cells[1].Text.Trim() + "')  group by to_char(a.vchdate,'YYYYMM'),to_char(a.vchdate,'MONTH') order by to_char(a.vchdate,'YYYYMM') ";
                SQuery2 = "Select to_char(Max(a.orddt),'dd/mm/yyyy') as Last_PO,b.Aname,sum(a.qtyord) as PO_Qty from POMAS a ,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + DateRange + " and trim(A.icode)  in (SELECT distinct trim(A.icode) AS ERPCODE FROM item A WHERE trim(a.icode)='" + row.Cells[1].Text.Trim() + "')  group by b.aname ";
                SQuery3 = "Select to_char(Max(a.vchdate),'dd/mm/yyyy') as Last_Issue,b.name,sum(a.iqtyout) as Issue_Qty from ivoucher a ,type b where trim(A.acode)=trim(B.type1) and b.id='M' and a.branchcd='" + frm_mbr + "' and a.type like '3%' and a.vchdate " + DateRange + " and trim(A.icode)  in (SELECT distinct trim(A.icode) AS ERPCODE FROM item A WHERE trim(a.icode)='" + row.Cells[1].Text.Trim() + "')  group by b.name ";

                lblSg1.Text = "Stock Summary + Analysis";
                lblSg2.Text = "Monthly Movement of Selected Item " + row.Cells[2].Text.Trim();
                lblSg3.Text = "Last P.O. of Selected Item " + row.Cells[2].Text.Trim();
                lblSg4.Text = "Last Issue of Selected Item " + row.Cells[2].Text.Trim();
                if (frm_formID == "F50135")
                {
                    lblSg3.Text = "Last S.O. of Selected Item " + row.Cells[2].Text.Trim();
                    lblSg4.Text = "Last Sale of Selected Item " + row.Cells[2].Text.Trim();
                    SQuery2 = "Select to_char(Max(a.orddt),'dd/mm/yyyy') as Last_SO,b.Aname,sum(a.qtyord) as SO_Qty from SOMAS a ,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.orddt " + DateRange + " and trim(A.icode)  in (SELECT distinct trim(A.icode) AS ERPCODE FROM item A WHERE trim(a.icode)='" + row.Cells[1].Text.Trim() + "')  group by b.aname ";
                    SQuery3 = "Select to_char(Max(a.vchdate),'dd/mm/yyyy') as Last_Inv,b.aname,sum(a.iqtyout) as Sales_Qty from ivoucher a ,famst b where trim(A.acode)=trim(B.acode)  and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + DateRange + " and trim(A.icode)  in (SELECT distinct trim(A.icode) AS ERPCODE FROM item A WHERE trim(a.icode)='" + row.Cells[1].Text.Trim() + "')  group by b.aname ";

                }


                break;


            case "F25193":
                SQuery1 = "SELECT b.Iname AS Item_Name,a.iqty_chl as MRR_Qty,b.unit,b.cpartno as Item_Code,A.ICODE AS ERPCODE,a.srno FROM ivoucher A ,item b WHERE trim(a.icode)=trim(B.icode) and A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "' and a.store!='R' order by a.srno";
                SQuery2 = "Select to_char(Max(a.orddt),'dd/mm/yyyy') as Last_PR,b.Iname,sum(a.qtyord) as PO_Qty,b.unit,b.Cpartno from POMAS a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '5%' and a.orddt " + DateRange + " and trim(A.icode)||trim(a.ordno)||to_Char(A.orddt,'dd/mm/yyyy') in (SELECT trim(A.icode)||trim(a.ponum)||to_Char(A.podate,'dd/mm/yyyy') AS ERPCODE FROM ivoucher A WHERE A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "')  group by b.Iname,b.unit,b.Cpartno order by b.iname ";
                SQuery3 = "Select to_char(Max(a.vchdate),'dd/mm/yyyy') as Last_MRR,b.Iname,sum(a.iqty_chl) as GE_Qty,b.unit,b.Cpartno from ivoucherp a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + DateRange + " and trim(A.icode)||trim(a.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') in (SELECT trim(A.icode)||trim(a.genum)||to_Char(A.gedate,'dd/mm/yyyy') AS ERPCODE FROM ivoucher A WHERE A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "')  group by b.Iname,b.unit,b.Cpartno order by b.iname ";


                lblSg1.Text = "Details of MRR/GRN Inwards";
                lblSg2.Text = "Details of Selected Gate Inward " + row.Cells[2].Text.Trim();
                lblSg3.Text = "Last P.O. of Items for MRR. # " + row.Cells[2].Text.Trim();
                lblSg4.Text = "Last G.E. of Items for MRR #" + row.Cells[2].Text.Trim();

                break;

            case "F30159":
                SQuery1 = "SELECT b.Iname AS Item_Name,a.iqty_chl as MRR_Qty,a.rej_rw as Rej_qty,b.unit,b.cpartno as Item_Code,A.ICODE AS ERPCODE,a.srno FROM ivoucher A ,item b WHERE trim(a.icode)=trim(B.icode) and A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "' and a.store!='R' order by a.srno";
                SQuery2 = "Select to_char(Max(a.vchdate),'dd/mm/yyyy') as Last_MRR,b.Iname,sum(a.iqty_chl) as inw_Qty,sum(a.iqtyin) as Rej_Qty,b.unit,b.Cpartno from ivoucher a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + DateRange + " and a.store='R' and trim(A.icode) in (SELECT trim(A.icode) AS ERPCODE FROM ivoucher A WHERE A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "')  group by b.Iname,b.unit,b.Cpartno order by b.iname ";
                SQuery3 = "Select to_char(Max(a.vchdate),'dd/mm/yyyy') as Last_LRN,b.Iname,sum(a.iqtyin) as Rej_Qty,b.unit,b.Cpartno from ivoucher a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '14%' and a.vchdate " + DateRange + " and a.store='R' and trim(A.icode) in (SELECT trim(A.icode) AS ERPCODE FROM ivoucher A WHERE A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "')  group by b.Iname,b.unit,b.Cpartno order by b.iname ";

                lblSg1.Text = "Details of MRR/GRN";
                lblSg2.Text = "Details of Selected MRR/GRN " + row.Cells[2].Text.Trim();
                lblSg3.Text = "Details of QA Rejection of Such Items " + row.Cells[2].Text.Trim();
                lblSg4.Text = "Details of Line Rejection of Such Items " + row.Cells[2].Text.Trim();

                break;


            case "F47159":
            case "F49159":
            case "F50136":
                addl_cond = "1=1";
                if (frm_formID == "F49159")
                {
                    addl_cond = "a.type='4F'";
                }

                SQuery1 = "SELECT a.ordno as Fstr,a.qtyord as Order_Qty,a.Ciname AS Item_Name,a.cpartno as Item_Code,A.ICODE AS ERPCODE,a.srno FROM somas A WHERE A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "' order by a.srno";
                SQuery2 = "Select to_char(Max(a.Vchdate),'dd/mm/yyyy') as Last_Disp,b.Iname,b.Cpartno from ivoucher a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and " + addl_cond + " and a.vchdate " + DateRange + " and trim(A.icode) in (SELECT trim(A.ICODE) AS ERPCODE FROM somas A WHERE A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "') and A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')!='" + row.Cells[1].Text.Trim() + "' group by b.Iname,b.Cpartno order by b.iname ";
                SQuery3 = "Select to_char(Max(a.Vchdate),'dd/mm/yyyy') as Last_Prodn,b.Iname,b.Cpartno from ivoucher a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '1%' and a.type>'14' and a.vchdate " + DateRange + " and trim(A.icode) in (SELECT trim(A.ICODE) AS ERPCODE FROM somas A WHERE A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "') group by b.Iname,b.Cpartno order by b.iname ";

                lblSg1.Text = "List of Sales Orders";
                lblSg2.Text = "Item Wise Details for Order # " + row.Cells[2].Text.Trim();
                lblSg3.Text = "Last Dispatch of Item for Order # " + row.Cells[2].Text.Trim();
                lblSg4.Text = "Last Production of Item for Order #" + row.Cells[2].Text.Trim();

                break;

            case "F50159":
                SQuery1 = "SELECT a.Vchnum as Fstr,a.iqtyout as Qty_Sold,a.Purpose AS Item_Name,a.Exc_57f4 as Item_Code,A.ICODE AS ERPCODE,a.morder FROM ivoucher A WHERE A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "' order by a.morder";
                SQuery2 = "Select to_char(Max(a.Vchdate),'dd/mm/yyyy') as Last_Disp,b.Iname,b.Cpartno  from ivoucher a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + DateRange + " and trim(A.icode) in (SELECT trim(A.ICODE) AS ERPCODE FROM ivoucher A WHERE A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "') and A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')!='" + row.Cells[1].Text.Trim() + "' group by b.Iname,b.Cpartno order by b.iname ";
                SQuery3 = "Select to_char(Max(a.Vchdate),'dd/mm/yyyy') as Last_Prodn,b.Iname,b.Cpartno from ivoucher a ,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '1%' and a.type>'14' and a.vchdate " + DateRange + " and trim(A.icode) in (SELECT trim(A.ICODE) AS ERPCODE FROM ivoucher A WHERE A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "') group by b.Iname,b.Cpartno  order by b.iname ";

                lblSg1.Text = "List of Invoices";
                lblSg2.Text = "Item Wise Details for Invoice # " + row.Cells[2].Text.Trim();
                lblSg3.Text = "Last Dispatch of Item for Invoice # " + row.Cells[2].Text.Trim();
                lblSg4.Text = "Last Production of Item for Invoice #" + row.Cells[2].Text.Trim();

                break;
            case "F50101":
                break;

            case "F40122":
                col1 = row.Cells[2].Text.Trim() + row.Cells[3].Text.Trim();
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_JOBCARD", col1);
                fgenMV.Fn_Set_Mvar(frm_qstr, "SG1_INDEX", rowIndex.ToString());
                string sicode = row.Cells[22].Text.Trim();
                if (selectedCellIndex == 2)
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "30");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + col1 + "'");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_prod_reps(frm_qstr);
                    return;
                }
                if (selectedCellIndex == 3)
                {
                    SQuery = "select '-' as fstr,y.name as Stage_Name,x.prod as Production,x.rej as Rejection,x.prod-x.rej as Net_Prodn,to_char(x.Vchdate,'dd/mm/yyyy') Dated,x.stagec as StageCode,x.icode as Erpcode,x.vchnum,x.ename as Machine,x.ent_by,x.ent_Dt from (select a.srno,a.stagec,a.icode,nvl(b.tot,0) as tot,nvl(b.prod,0) as prod ,nvl(b.rej,0) as rej,b.vchdate,b.vchnum,b.ename,b.ent_by,b.ent_Dt from itwstage a left outer join (select decode(trim(stage),'-','01',stage) as stage,icode,sum(nvl(decode(type,'85',iqtyout+nvl(a4,0),a2+nvl(a4,0)),0)) as tot,sum(nvl(decode(type,'85',iqtyout+nvl(a4,0),a2+nvl(a4,0)),0)) as prod,sum(nvl(a4,0)) as rej,Vchdate,vchnum,ename,ent_by,ent_Dt from PROD_SHEET  where branchcd='" + frm_mbr + "' and type in('85','88','86') and job_no||job_dt='" + col1 + "' and stage<>'08' group by stage,icode,Vchdate,vchnum,ename,ent_by,ent_Dt  union all Select '08' as stage,x.icode,sum(x.iqtyin+nvl(x.rej_rw,0)) as Prodn,sum(x.iqtyin+nvl(x.rej_rw,0)) as Prodn,sum(x.rej_rw) as rejn ,Vchdate,vchnum,'-' as ename,x.ent_by,x.ent_Dt from ivoucher x where x.branchcd='" + frm_mbr + "' and x.type in('15','16') and trim(x.invno)||trim(to_Char(x.invdate,'dd/mm/yyyy'))='" + col1 + "' and trim(x.icode)='" + sicode + "' group by x.icode,x.vchdate,x.vchnum,x.ent_by,x.ent_Dt) b on trim(a.stagec)=trim(b.stage) where a.branchcd='" + frm_mbr + "' and trim(a.icode)='" + sicode + "' order by a.icode,a.srno) x ,(Select type1,name from type where id='K') y,item z where x.stagec=y.type1 and trim(x.icode)=trim(z.icode) order by x.icode,x.srno,x.Vchdate";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Status of Job No: " + row.Cells[2].Text.Trim() + " Dt : " + row.Cells[3].Text.Trim() + " ", frm_qstr);
                    return;
                }
                if (selectedCellIndex == 4)
                {
                    SQuery = "select '-' as fstr,a.vchnum as WO_NO,to_char(a.vchdate,'dd/mm/yyyy') as Dated,b.name as Process,a.Ename as Machine,c.Iname,a.a1 as Qty,c.cpartno,a.job_no,a.job_dt,a.ent_by,a.ent_dt from PROD_SHEET  a ,item c,(select NAME,type1 from type where id='K' order by TYPE1 ) b where trim(a.stage)=b.type1  and trim(a.icode)=trim(c.icode) and a.VCHDATE " + DateRange + " AND a.type='90' and a.branchcd='" + frm_mbr + "' and TRIM(job_no)||TRIM(job_dt)='" + col1 + "' order by a.vchdate desc ,a.vchnum desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_sseek("Status of Job No: " + row.Cells[2].Text.Trim() + " Dt : " + row.Cells[3].Text.Trim() + " ", frm_qstr);
                    return;
                }
                if (selectedCellIndex == 5)
                {
                    // job vs actual report to be link here.
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "30");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + col1 + "'");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_prod_reps(frm_qstr);
                    return;
                }

                dt = new DataTable();
                SQuery = "select COL13,comments3,vchnum,comments5,attach from costestimate where branchcd='" + frm_mbr + "' and type='30' and vchnum||to_Char(vchdate,'dd/mm/yyyy')='" + col1 + "' and srno=0";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                SQuery1 = "SELECT a.icode as Fstr,a.col1 as srno,a.col2 as desc_,a.col3 as specs,a.col4 as multiply,a.col5 as qty_req,a.col6 as extra,A.col9 AS ERPCODE FROM costestimate A WHERE A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "' order by a.srno";
                SQuery1 = "";
                SQuery2 = "";
                SQuery3 = "select y.name as Stage,sum(x.prod) as Prod,sum(x.rej) as Rej,sum(x.prod-x.rej) as Net_Prodn,x.stagec as Code,x.srno,x.icode as Erpcode,x.fm_Fact,Y.IGNORESTG as istg from (select a.srno,a.fm_Fact,a.stagec,a.icode,nvl(b.tot,0) as tot,nvl(b.prod,0) as prod ,nvl(b.rej,0) as rej,b.vchdate from itwstage a left outer join (select decode(trim(stage),'-','01',stage) as stage,icode,sum(nvl(decode(type,'85',iqtyout+nvl(a4,0),a2+nvl(a4,0)),0)) as tot,sum(nvl(decode(type,'85',iqtyout+nvl(a4,0),a2+nvl(a4,0)),0)) as prod,sum(nvl(a4,0)) as rej,Vchdate from PROD_SHEET  where branchcd='" + frm_mbr + "' and type in('85','88','86') and job_no||job_dt='" + col1 + "' and stage<>'08' group by stage,icode,Vchdate  union all Select '08' as stage,x.icode,sum(x.iqtyin) as Prodn,sum(x.iqtyin+nvl(x.rej_rw,0)) as Prodn,sum(x.rej_rw) as rejn ,Vchdate from ivoucher x where x.branchcd='" + frm_mbr + "' and (x.type='15' or x.type='16') and trim(x.invno)||trim(to_Char(x.invdate,'dd/mm/yyyy'))='" + col1 + "' and trim(x.icode)='" + sicode + "' group by x.icode,x.vchdate) b on trim(a.stagec)=trim(b.stage) where trim(a.icode)='" + sicode + "' order by a.icode,a.srno) x ,(Select type1,name,NVL(EXC_TARRIF,'-') AS IGNORESTG from type where id='K') y,item z where x.stagec=y.type1 and trim(x.icode)=trim(z.icode) group by y.name,x.icode,x.stagec,x.fm_fact,x.srno,Y.IGNORESTG order by x.icode,x.srno";
                SQuery3 = "select y.name as Stage,sum(x.prod) as Prod,sum(x.rej) as Rej,sum(x.prod-x.rej) as Net_Prodn,x.stagec as Code,x.srno,x.icode as Erpcode,x.fm_Fact,Y.IGNORESTG as istg from (select a.srno,a.fm_Fact,a.stagec,a.icode,nvl(b.tot,0) as tot,nvl(b.prod,0) as prod ,nvl(b.rej,0) as rej,b.vchdate from (select * from itwstage where branchcd!='DD') a left outer join (select decode(trim(stage),'-','01',stage) as stage,icode,sum(nvl(decode(type,'85',iqtyout+nvl(a4,0),a2+nvl(a4,0)),0)) as tot,sum(nvl(decode(type,'85',iqtyout+nvl(a4,0),a2+nvl(a4,0)),0)) as prod,sum(nvl(a4,0)) as rej,Vchdate from PROD_SHEET  where branchcd='" + frm_mbr + "' and type in('85','88','86') and job_no||job_dt='" + col1 + "' and stage<>'08' group by stage,icode,Vchdate  union all Select '08' as stage,x.icode,sum(x.iqtyin) as Prodn,sum(x.iqtyin+nvl(x.rej_rw,0)) as Prodn,sum(x.rej_rw) as rejn ,Vchdate from ivoucher x where x.branchcd='" + frm_mbr + "' and (x.type='15' or x.type='16') and trim(x.invno)||trim(to_Char(x.invdate,'dd/mm/yyyy'))='" + col1 + "' and trim(x.icode)='" + sicode + "'  and x.store<>'W' group by x.icode,x.vchdate) b on trim(a.stagec)=trim(b.stage) where trim(a.icode)='" + sicode + "' order by a.icode,a.srno) x ,(Select type1,name,NVL(EXC_TARRIF,'-') AS IGNORESTG from type where id='K') y,item z where x.stagec=y.type1 and trim(x.icode)=trim(z.icode) group by y.name,x.icode,x.stagec,x.fm_fact,x.srno,Y.IGNORESTG order by x.icode,x.srno";

                lblSg1.Text = "List of Job Card";
                lblSg2.Text = "Print, Delv, job Qty, Sales Qty Status # " + row.Cells[2].Text.Trim();
                lblSg3.Text = "Paper Issue and Cutting Status # " + row.Cells[2].Text.Trim();
                if (dt.Rows.Count > 0) lblSg4.Text = "Process Wise Prodn Status (" + dt.Rows[0]["COL13"].ToString().Trim() + " Ups Job) # " + row.Cells[2].Text.Trim();
                else lblSg4.Text = "Process Wise Prodn Status # " + row.Cells[2].Text.Trim();

                gridDiv1.Style.Add("height", "70px");
                gridDiv3.Style.Add("height", "115px");
                gridDiv4.Style.Add("height", "170px");

                sg2.DataSource = null; sg2.DataBind();
                sg3.DataSource = null; sg3.DataBind();
                break;
            default:
                //SQuery1 = "SELECT A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO FROM POMAS A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) AND A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_cHAR(A.ORdDT,'DD/MM/YYYY')='" + row.Cells[1].Text.Trim() + "' order by a.icode";
                SQuery1 = "SELECT A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO FROM POMAS A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) AND TRIM(A.icode)='" + row.Cells[1].Text.Trim() + "' order by a.icode";
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

            sg2.DataSource = sg2_dt;
            sg2.DataBind();
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

            if (frm_formID == "F40122")
            {
                double_val1 = 0;
                for (int i = 0; i < sg4_dt.Rows.Count; i++)
                {
                    double_val1 += fgen.make_double(sg4_dt.Rows[i]["Rej"].ToString().Trim());
                }
                sg4_dr = sg4_dt.NewRow();
                sg4_dr["srno"] = "1";
                sg4_dr["Stage"] = "Tot Rejection";
                sg4_dr["Rej"] = double_val1;
                sg4_dt.Rows.Add(sg4_dr);
            }

            sg4.DataSource = sg4_dt;
            sg4.DataBind();

            if (frm_formID == "F40122")
            {
                for (int i = 0; i < sg4.Rows.Count; i++)
                {
                    if (sg4.Rows[i].Cells[1].Text.Trim() == "Tot Rejection")
                    {
                        sg4.Rows[i].Cells[3].BackColor = Color.Red;
                    }
                }
            }
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
    protected void sg3_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void sg4_SelectedIndexChanged(object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow row = sg4.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex4.Value);
        string mq0 = sg1.HeaderRow.Cells[selectedCellIndex].Text.Replace("<br/>", " "); // dynamic heading        
        if (selectedCellIndex > 0) selectedCellIndex -= 1;

        switch (frm_formID)
        {
            case "F40122":
                string snp_cd = "08";
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_JOBCARD");
                string sicode = row.Cells[7].Text.Trim();
                string stgCode = row.Cells[5].Text.Trim();
                SQuery = "Select * from (select '-' as fstr,y.name as Stage_Name,x.prod as Production,x.rej as Rejection,x.prod-x.rej as Net_Prodn,to_char(x.Vchdate,'dd/mm/yyyy') Dated,x.ename as Machine,X.PREVCODE AS SHIFTN,X.ENT_BY,x.stagec as StageCode,x.icode as Erpcode from (select a.srno,a.stagec,a.icode,nvl(b.tot,0) as tot,nvl(b.prod,0) as prod ,nvl(b.rej,0) as rej,b.vchdate,b.ename,B.ENT_BY,B.PREVCODE from itwstage a left outer join (select decode(trim(stage),'-','01',stage) as stage,icode,sum(nvl(decode(type,'85',iqtyout+nvl(a4,0),a2)+nvl(a4,0),0)) as tot,sum(nvl(decode(type,'85',iqtyout+nvl(a4,0),a2+nvl(a4,0)),0)) as prod,sum(nvl(a4,0)) as rej,Vchdate,ename,ENT_BY,PREVCODE from PROD_SHEET  where branchcd='" + frm_mbr + "' and type in('85','88','86') and job_no||job_dt='" + col1 + "' and stage<>'08' group by stage,icode,Vchdate,ename,ENT_BY,PREVCODE  union all Select '08' as stage,x.icode,sum(x.iqtyin+nvl(x.rej_rw,0)) as Prod,sum(x.iqtyin+nvl(x.rej_rw,0)) as Prodn,sum(x.rej_rw) as rejn ,Vchdate,'-' as ename,ENT_BY,'-' AS PREVCODE from ivoucher x where x.branchcd='" + frm_mbr + "' and (x.type='15' or x.type='16') and trim(x.invno)||trim(to_Char(x.invdate,'dd/mm/yyyy'))='" + col1 + "' and trim(x.icode)='" + sicode + "' group by x.icode,X.EnT_BY,x.vchdate) b on trim(a.stagec)=trim(b.stage) where trim(a.icode)='" + sicode + "' order by a.icode,a.srno) x ,(Select type1,name from type where id='K') y,item z where x.stagec=y.type1 and trim(x.icode)=trim(z.icode) order by x.icode,x.srno) where stagecode='" + stgCode + "' order by Dated";
                SQuery = "Select * from (select '-' as fstr,y.name as Stage_Name,x.prod as Production,x.rej as Rejection,x.prod-x.rej as Net_Prodn,to_char(x.Vchdate,'dd/mm/yyyy') Dated,x.ename as Machine,X.PREVCODE AS SHIFTN,X.ENT_BY,x.stagec as StageCode,x.icode as Erpcode from (select a.srno,a.stagec,a.icode,nvl(b.tot,0) as tot,nvl(b.prod,0) as prod ,nvl(b.rej,0) as rej,b.vchdate,b.ename,B.ENT_BY,B.PREVCODE from (select * from itwstage where branchcd!='DD') a left outer join (select decode(trim(stage),'-','01',stage) as stage,icode,sum(nvl(decode(type,'85',iqtyout+nvl(a4,0),a2)+nvl(a4,0),0)) as tot,sum(nvl(decode(type,'85',iqtyout+nvl(a4,0),a2+nvl(a4,0)),0)) as prod,sum(nvl(a4,0)) as rej,Vchdate,ename,ENT_BY,PREVCODE from PROD_SHEET  where branchcd='" + frm_mbr + "' and type in('85','88','86') and job_no||job_dt='" + col1 + "' and stage<>'" + snp_cd + "' group by stage,icode,Vchdate,ename,ENT_BY,PREVCODE  union all Select '" + snp_cd + "' as stage,x.icode,sum(x.iqtyin+nvl(x.rej_rw,0)) as Prod,sum(x.iqtyin+nvl(x.rej_rw,0)) as Prodn,sum(x.rej_rw) as rejn ,Vchdate,'-' as ename,ENT_BY,'-' AS PREVCODE from ivoucher x where x.branchcd='" + frm_mbr + "' and (x.type='15' or x.type='16') and trim(x.invno)||trim(to_Char(x.invdate,'dd/mm/yyyy'))='" + col1 + "' and trim(x.icode)='" + sicode + "'  and x.store<>'W'  group by x.icode,X.EnT_BY,x.vchdate) b on trim(a.stagec)=trim(b.stage) where trim(a.icode)='" + sicode + "' order by a.icode,a.srno) x ,(Select type1,name from type where id='K') y,item z where x.stagec=y.type1 and trim(x.icode)=trim(z.icode) order by x.icode,x.srno) where stagecode='" + stgCode + "' order by Dated";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek(lblSg4.Text + " Stage " + row.Cells[1].Text.Trim(), frm_qstr);
                break;
        }
    }
    //------------------------------------------------------------------------------------
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
    protected void sg2_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void sg3_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void sg4_RowCreated(object sender, GridViewRowEventArgs e)
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
    protected void lnkSg2_Click(object sender, EventArgs e)
    {
        if (sg1.Rows.Count > 0)
        {
            switch (frm_formID)
            {
                case "F40122":
                    int index = fgen.make_int(fgenMV.Fn_Get_Mvar(frm_qstr, "SG1_INDEX"));
                    string solnk = sg1.Rows[index].Cells[23].Text.Trim();
                    string sicode = sg1.Rows[index].Cells[22].Text.Trim();
                    string sacode = sg1.Rows[index].Cells[11].Text.Trim();

                    SQuery2 = "select sum(tent_qty) as Print_qty,sum(firm_qty) as Delv_qty,sum(job) as job,sum(sold) as sold,trim(acode) as acode,trim(icode) as icode from (select acode,icode,sum(budgetcost) as Firm_Qty,sum(actualcost) as Tent_Qty,0 as job,0 as sold  from budgmst where solink='" + solnk + "' and trim(acode)||trim(icode)='" + sacode + sicode + "' AND  vchdate>=to_date('01/04/2009','dd/mm/yyyy') and branchcd='" + frm_mbr + "' and type='46' group by acode,icode union all select acode,icode,0 as Firm_Qty,0 as Tent_Qty,sum(qty) as job,0 as sale  from costestimate where branchcd='" + frm_mbr + "' and type='30' and substr(convdate,1,20)='" + solnk + "' and trim(acode)||trim(icode)='" + sacode + sicode + "' AND vchdate>=to_date('01/04/2009','dd/mm/yyyy') and srno=1 and status<>'Y' group by acode,icode union all select acode,icode,0 as Firm_Qty,0 as Tent_Qty,0 as job,sum(iqtyout) as sold  from ivoucher where trim(acode)||trim(icode)='" + sacode + sicode + "' AND vchdate>=to_date('01/04/2009','dd/mm/yyyy') and branchcd='" + frm_mbr + "' and substr(type,1,1)='4' group by acode,icode) group by trim(acode),trim(icode)";
                    sg2_dt = new DataTable();
                    sg2_dt = fgen.getdata(frm_qstr, frm_cocd, SQuery2);

                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    break;
            }
        }
    }
    protected void lnkSg3_Click(object sender, EventArgs e)
    {
        if (sg1.Rows.Count > 0)
        {
            switch (frm_formID)
            {
                case "F40122":
                    int index = fgen.make_int(fgenMV.Fn_Get_Mvar(frm_qstr, "SG1_INDEX"));
                    col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_JOBCARD");
                    string solnk = sg1.Rows[index].Cells[23].Text.Trim();
                    string sicode = sg1.Rows[index].Cells[22].Text.Trim();
                    string sacode = sg1.Rows[index].Cells[11].Text.Trim();

                    create_tab3();

                    SQuery3 = "SELECT A.*,B.INAME,b.unit FROM (select icode,sum(iqtyout)-sum(iqtyin) as tot from ivoucher where upper(Trim(nvl(desc_,'-'))) not like '%SHEETER%' and branchcd='" + frm_mbr + "' and type in ('32','31','30','11','13','35') and trim(invno)||to_char(invdate,'dd/mm/yyyy')='" + col1 + "' and nvl(iqtyout,0)+nvl(iqtyin,0)>0 group by icode) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) ";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery3);
                    double_val1 = 0;
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_t1"] = dt.Rows[i]["ICODE"].ToString().Trim();
                            sg3_dr["sg3_t2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg3_dr["sg3_t3"] = dt.Rows[i]["unit"].ToString().Trim();
                            sg3_dr["sg3_t4"] = dt.Rows[i]["tot"].ToString().Trim();
                            double_val1 += fgen.make_double(dt.Rows[i]["tot"].ToString().Trim());
                            sg3_dt.Rows.Add(sg3_dr);
                        }

                        sg3_dr = sg3_dt.NewRow();
                        sg3_dr["sg3_t1"] = "";
                        sg3_dr["sg3_t2"] = "Total";
                        sg3_dr["sg3_t3"] = "";
                        sg3_dr["sg3_t4"] = double_val1;
                        sg3_dt.Rows.Add(sg3_dr);
                    }
                    else
                    {
                        sg3_dr = sg3_dt.NewRow();
                        sg3_dr["sg3_t2"] = "No Issue Slip Found";
                        sg3_dt.Rows.Add(sg3_dr);
                    }

                    //
                    SQuery3 = "select a.*,b.iname,b.cpartno,b.unit from PROD_SHEET a,item b where trim(a.icode)=trim(B.icode) and a.type='85' and a.branchcd='" + frm_mbr + "' and trim(a.job_no)||trim(a.job_dt)='" + col1 + "' order by a.srno ";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery3);
                    double_val1 = 0;
                    double cuts = 0, oks = 0;
                    if (dt.Rows.Count > 0)
                    {
                        sg3_dr = sg3_dt.NewRow();
                        sg3_dt.Rows.Add(sg3_dr);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_t1"] = dt.Rows[i]["ICODE"].ToString().Trim();
                            sg3_dr["sg3_t2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg3_dr["sg3_t3"] = dt.Rows[i]["unit"].ToString().Trim();
                            sg3_dr["sg3_t4"] = dt.Rows[i]["a8"].ToString().Trim();
                            sg3_dr["sg3_t5"] = dt.Rows[i]["a1"].ToString().Trim();
                            sg3_dr["sg3_t6"] = dt.Rows[i]["a2"].ToString().Trim();
                            sg3_dr["sg3_t7"] = dt.Rows[i]["a3"].ToString().Trim();
                            cuts += fgen.make_double(dt.Rows[i]["a3"].ToString().Trim());
                            sg3_dr["sg3_t8"] = dt.Rows[i]["a4"].ToString().Trim();
                            sg3_dr["sg3_t9"] = dt.Rows[i]["remakrs2"].ToString().Trim();
                            sg3_dr["sg3_t10"] = dt.Rows[i]["a5"].ToString().Trim();
                            oks += fgen.make_double(dt.Rows[i]["a5"].ToString().Trim());
                            sg3_dt.Rows.Add(sg3_dr);
                        }

                        sg3_dr = sg3_dt.NewRow();
                        sg3_dr["sg3_t1"] = "";
                        sg3_dr["sg3_t2"] = "Cut Sheet";
                        sg3_dr["sg3_t3"] = "";
                        sg3_dr["sg3_t4"] = cuts;
                        sg3_dt.Rows.Add(sg3_dr);

                        sg3_dr = sg3_dt.NewRow();
                        sg3_dr["sg3_t1"] = "";
                        sg3_dr["sg3_t2"] = "Ok Sheet";
                        sg3_dr["sg3_t3"] = "";
                        sg3_dr["sg3_t4"] = cuts;
                        sg3_dt.Rows.Add(sg3_dr);
                    }
                    else
                    {
                        sg3_dr = sg3_dt.NewRow();
                        sg3_dr["sg3_t1"] = "";
                        sg3_dr["sg3_t2"] = "";
                        sg3_dr["sg3_t3"] = "";
                        sg3_dr["sg3_t4"] = "";
                        sg3_dr["sg3_t5"] = "Rej.All";
                        sg3_dr["sg3_t6"] = sg1.Rows[index].Cells[12].Text.Trim(); ;
                        sg3_dt.Rows.Add(sg3_dr);
                    }

                    sg3.DataSource = sg3_dt;
                    sg3.DataBind();

                    for (int i = 0; i < sg3.Rows.Count; i++)
                    {
                        if (sg3.Rows[i].Cells[2].Text.Trim() == "Total")
                        {
                            sg3.Rows[i].Cells[4].BackColor = Color.Yellow;
                        }
                        if (sg3.Rows[i].Cells[5].Text.Trim() == "Rej.All")
                        {
                            sg3.Rows[i].Cells[5].BackColor = Color.LightGreen;
                            sg3.Rows[i].Cells[6].BackColor = Color.LightGreen;
                        }
                    }
                    break;
            }
        }
    }
    protected void btnClose_ServerClick(object sender, EventArgs e)
    {
        col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_JOBCARD");
        if (col1.Length > 6)
        {
            hffield.Value = "JOBCLOSE";
            fgen.msg("-", "CMSG", "Are You Sure, You Want to Close Job Card# " + col1.Substring(0, 6));
        }
        else
        {
            fgen.msg("-", "AMSG", "Pleas Select Job Card!!");
        }
    }
    protected void sg1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        sg1.PageIndex = e.NewPageIndex;
        fillGrid();
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        SQuery = "";
        SQuery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_JOBQUERY");
        if (SQuery.Length > 2)
        {
            dt = new DataTable();
            dt = (DataTable)ViewState["sg1"];

            DataTable dt1 = new DataTable();
            dt1 = fgen.search_vip(frm_qstr, frm_cocd, SQuery, txtSearch.Text.Trim().ToUpper());

            ViewState["sg1"] = dt1;
            if (dt1 != null)
            {
                sg1.DataSource = dt1;
                sg1.DataBind(); dt1.Dispose();
                lblTotcount.Text = "Total Rows : " + dt1.Rows.Count;
            }
            else
            {
                sg1.DataSource = null;
                sg1.DataBind();
            }
        }
    }
    protected void btnExpToExcel_Click(object sender, ImageClickEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        if (dt.Rows.Count > 0) fgen.exp_to_excel(dt, "ms-excel", "xls", frm_cocd + "_" + lblheader.Text + "_" + DateTime.Now.ToString().Trim());
        else fgen.msg("-", "AMSG", "No Data to Export"); dt.Dispose();
    }
}