using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.IO;
using System.Data.OleDb;

public partial class om_rcpt_vch : System.Web.UI.Page
{
    DataTable dt, dt1;

    DataTable sg2_dt; DataRow sg2_dr;

    DataRow dr1, oporow;
    DataRow oporow3;
    DataSet oDS;
    DataSet oDS3;
    //----------------------------
    string btnval, col1, col2, col3, fill_Date, tmp_var, vip = "", mq1, mq0;
    string pk_error = "Y", chk_rights = "N", DateRange;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_tabname, frm_myear, frm_sql, frm_ulvl, frm_formID, frm_UserID;
    //----------------------------
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string SQuery, HCID, merr = "0", eff_Dt, m1, frm_cDt1, frm_cDt2;
    double cum_bal;
    int i, z = 0;
    string PrdRange = "";
    string fromdt = "";
    string todt = "";
    string currdt;
    fgenDB fgen = new fgenDB();
    //----------------------------------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        // for loading page 
        frm_tabname = "VOUCHER";
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
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
                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    tmp_var = "A";
                }
                else Response.Redirect("~/login.aspx");
            }

            btnnew.Focus();
            txtbillamount.ReadOnly = true;
            txtbalamt.ReadOnly = true;
            fill_Date = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            if (!Page.IsPostBack)
            {
                hf3.Value = fgen.getOptionPW(frm_qstr, frm_cocd, "W2037", "OPT_ENABLE", frm_mbr);
                hfHOPayRcvConcept.Value = fgen.getOption(frm_qstr, frm_cocd, "W0153", "OPT_ENABLE");
                hfHObr.Value = fgen.getOption(frm_qstr, frm_cocd, "W0153", "OPT_PARAM");
                fgen.DisableForm(this.Controls);
                enablectrl();
                set_Val();
                getColHeading();
            }
            cal();
            //if (sg1.Rows.Count > 1) myfun();
            txtchqdt.Attributes.Add("onkeypress", "return clickEnter('" + txttamt.ClientID + "', event)");
            txtvchdate.Attributes.Add("onkeypress", "return clickEnter('" + btnlbl4.ClientID + "', event)");
            setColHeadings();
        }
    }
    //----------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------
    void getColHeading()
    {
        return;
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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F70101":
            case "F70106":
                tab2.Visible = false;
                //tab3.Visible = false;
                tab4.Visible = false;
                tab5.Visible = false;
                tab6.Visible = false;
                break;
        }
        return;
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

            //for (int K = 0; K < sg1.Rows.Count; K++)
            //{
            //    if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

            //    ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");
            //    ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Add("autocomplete", "off");

            //    ((TextBox)sg1.Rows[K].FindControl("sg1_t7")).Attributes.Add("readonly", "readonly");
            //    ((TextBox)sg1.Rows[K].FindControl("sg1_t8")).Attributes.Add("readonly", "readonly");
            //}
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

        //switch (frm_formID)
        //{
        //    case "F70101":
        //        sg1.HeaderRow.Cells[7].Text = "Dr.Amt";
        //        sg1.HeaderRow.Cells[8].Text = "Cr.Amt";
        //        break;
        //    case "F70106":
        //        sg1.HeaderRow.Cells[7].Text = "Cr.Amt";
        //        sg1.HeaderRow.Cells[8].Text = "Dr.Amt";
        //        break;
        //}

        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        // for enable/disable some variables

        btnnew.Disabled = false;
        btnedit.Disabled = false;
        btncancel.Visible = false;
        btndel.Disabled = false;

        btnexit.Visible = true;
        btnsave.Disabled = true;
        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;

        btnprint.Disabled = false;
        btnlist.Disabled = false;

        create_tab();
        add_blankrows();
        sg1.DataSource = dt1;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false;

        // 2nd tab grid
        create_tab2();
        sg2_add_blankrows();

        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();


    }
    //----------------------------------------------------------------------------------------

    public void disablectrl()
    {
        // for disable/enable some variables
        btnnew.Disabled = true;
        btnedit.Disabled = true;
        btnsave.Disabled = false;
        btndel.Disabled = true;
        btnprint.Disabled = true;
        btnlist.Disabled = true;


        btncancel.Visible = true;
        btnexit.Visible = false;


        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;

    }
    //----------------------------------------------------------------------------------------

    public void clearctrl()
    {
        // for clearing some variables
        hffield.Value = "";
        edmode.Value = "";
    }
    //----------------------------------------------------------------------------------------

    public void set_Val()
    {
        // for setting radio button , table , head label on various options
        lblheader.Text = "Receipt/Payment Voucher";
        switch (frm_formID)
        {
            case "F70101":
                lblheader.Text = "Receipt Voucher";
                sg1.HeaderRow.Cells[7].Text = "Dr.Amt";
                sg1.HeaderRow.Cells[8].Text = "Cr.Amt";
                break;
            case "F70106":
                lblheader.Text = "Payment Voucher";

                sg1.HeaderRow.Cells[7].Text = "Cr.Amt";
                sg1.HeaderRow.Cells[8].Text = "Dr.Amt";
                break;
        }
        frm_tabname = "voucher";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY"); ;
    }
    //----------------------------------------------------------------------------------------

    public void make_qry_4_popup()
    {
        // for making query based on button value selected
        btnval = hffield.Value; set_Val();
        frm_vty = lbl1a.InnerText.Trim();
        //popselected.Value.Trim();

        switch (btnval)
        {
            case "TACODE":
                string addl_Flts = "";
                SQuery = "select Acode as fstr,ANAME as Party,Acode as Code,Addr1 as Address,Addr2 as City,Payment,GRP,nvl(schgrate,0) as CDR  from famst where branchcd='00' and length(Trim(nvl(deac_by,'-')))<=1 order by aname ";
                if (lbl1a.InnerText.Substring(0, 1) == "2")
                {
                    // commented on 24/05/2021 - to show all the ac
                    //addl_Flts = " and trim(nvl(GRP,'-')) in ('02','03','06','07','12','14','17')";
                    //if (lbl1a.InnerText.Substring(0, 2) == "20")
                    //{
                    //    addl_Flts = " and (trim(nvl(GRP,'-')) in ('02','03','06','07','12','14','17') or substr(nvl(GRP,'-'),1,1)>'2' ) ";
                    //}
                }
                else
                {
                    // commented on 24/05/2021 - to show all the ac
                    //addl_Flts = " and trim(nvl(GRP,'-')) in ('02','03','12','16','14','17')";
                }

                SQuery = "select A.acode as fstr,replacE(ANAME,'''','`') as Account_Name,A.Addr1 as Address_l1,a.addr2 as Address_l2,a.acode as ERP_Acode,a.Grp,b.Name,a.deac_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1 and trim(acode)!='" + tbank_code.Text.Trim() + "' " + addl_Flts + "  order by Account_Name";
                SQuery = "select A.acode as fstr,replacE(ANAME,'''','`') as Account_Name,A.Addr1 as Address_l1,a.addr2 as Address_l2,a.acode as ERP_Acode,a.Grp,b.Name,a.deac_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1 and trim(acode)!='" + tbank_code.Text.Trim() + "' " + addl_Flts + "  union all select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,A.Addr1 as Address_l1,a.addr2 as Address_l2,a.acode as ERP_Acode,a.Grp,b.Name,a.deac_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a ,(Select type1,name from type where id='Z') b,(Select trim(acode) as acode,sum(Dramt)-sum(Cramt) as tot from recdata where branchcd='" + frm_mbr + "' group by trim(AcodE) having  sum(Dramt)-sum(Cramt)<>0)  c where trim(a.grp)=trim(B.type1)and trim(a.acode)=trim(c.acode) and length(Trim(nvl(a.deac_by,'-')))>1 and trim(a.acode)!='" + tbank_code.Text.Trim() + "' " + addl_Flts + "  ";
                //SQuery = "select A.acode as fstr,replacE(ANAME,'''','`') as Account_Name,A.Addr1 as Address_l1,a.addr2 as Address_l2,a.acode as ERP_Acode,a.Grp,b.Name,a.deac_by as Deactivated_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD'  and trim(acode)!='" + tbank_code.Text.Trim() + "' " + addl_Flts + "  order by Account_Name";
                break;

            case "RMK":
                SQuery = "SELECT ID,NAME AS NARRATION,TYPE1 AS CODE FROM TYPE WHERE ID='N' ORDER BY TYPE1 ";
                break;

            case "TICODE":
                SQuery = "select Acode as fstr,ANAME as Party,Acode as Code,Addr1 as Address,Addr2 as City,Payment,GRP,nvl(schgrate,0) as CDR  from famst where branchcd='00' and trim(acode)!='" + tbank_code.Text.Trim() + "' and length(Trim(nvl(deac_by,'-')))<=1 order by aname ";

                break;
            case "EMPCODE":
                SQuery = "select Acode as fstr,ANAME as Party,Acode as Code,Addr1 as Address,Addr2 as City,Payment,GRP,nvl(schgrate,0) as CDR  from famst where branchcd='00' and trim(acode)!='" + tbank_code.Text.Trim() + "' and length(Trim(nvl(deac_by,'-')))<=1 order by aname ";

                break;
            case "EXPCODE":

                //if (frm_vty.Substring(0, 1) == "1")
                //{
                //    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as Voucher_No,to_char(a.vchdate,'dd/mm/yyyy') as Voucher_Dt,b.Aname as Account_Name,A.Refnum,to_char(a.refdate,'dd/mm/yyyy') as Refdate,a.dramt as amt,A.TYPE,a.Ent_by,to_Char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(A.rcode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " and a.Dramt>0 order by vdd desc,a.vchnum desc";
                //}
                //else
                //{
                //    SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as Voucher_No,to_char(a.vchdate,'dd/mm/yyyy') as Voucher_Dt,b.Aname as Account_Name,A.Refnum,to_char(a.refdate,'dd/mm/yyyy') as Refdate,a.cramt as amt,A.TYPE,a.Ent_by,to_Char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(A.rcode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " and a.Cramt>0 order by vdd desc,a.vchnum desc";
                //}
                SQuery = "select Acode as fstr,ANAME as Party,Acode as Code,Addr1 as Address,Addr2 as City,Payment,GRP,nvl(schgrate,0) as CDR  from famst where branchcd='00' and length(Trim(nvl(deac_by,'-')))<=1  order by aname ";

                break;

            case "Row_Add":
            case "Row_Edit":
                if (sg1.Rows.Count > 1)
                {
                    col1 = ""; col2 = "";
                    foreach (GridViewRow r1 in sg1.Rows)
                    {
                        if (col2.Length > 0) col2 = col2 + "," + "'" + r1.Cells[3].Text.Trim() + "'";
                        else col2 = "'" + r1.Cells[3].Text.Trim() + "'";

                        if (col1.Length > 0) col1 = col1 + "," + "'" + ((TextBox)r1.FindControl("txtInvno")).Text.Trim() + "'";
                        else col1 = "'" + ((TextBox)r1.FindControl("txtInvno")).Text.Trim() + "'";
                    }
                    col2 = "(" + col2 + ")";
                    col1 = "(" + col1 + ")";
                }
                else
                {
                    col2 = " ('')";
                    col1 = " ('-')";
                }
                SQuery = "select acode as fstr,Aname as ANAME,Acode as Acode,'On/Ac' as Bill_no,to_char(sysdate,'dd/mm/yyyy') as Bill_Dt,'0' as BAL_AMT,0 as pay_num,'" + frm_mbr + "' as branchcd,'" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR_NAME") + "' as branch_name,'-' as branch_code from famst a where length(Trim(nvl(deac_by,'-')))<=1 order by acode,aname";
                if (hfBillMSG.Value == "BILL")
                {
                    string mq_qry1 = "";
                    if (lbl1a.InnerText.Substring(0, 1) == "1")
                    {
                        mq_qry1 = "to_char(SUM(a.dramt) -SUM(a.cramt),'999999999.99') as Bal_amt";
                    }
                    else
                    {
                        mq_qry1 = "to_char(SUM(a.cramt) -SUM(a.dramt),'999999999.99')as Bal_amt";
                    }

                    SQuery = "select trim(a.ACODE)||trim(upper(nvl(a.invno,'-')))||to_char(a.invdate,'dd/mm/yyyy') as fstr,b.Aname,trim(a.ACODE) as Acode,trim(upper(nvl(a.invno,'-'))) as Bill_No,to_char(a.invdate,'dd/mm/yyyy') as Bill_Dt," + mq_qry1 + ",to_char(a.invdate,'yyyymmdd') as Bill_sort,b.pay_num from recdata a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and trim(a.acode) in (" + (txtacode.Text.Trim().Contains("'") ? txtacode.Text.Trim() : "'" + txtacode.Text.Trim() + "'") + ") and trim(upper(nvl(a.invno,'-'))) not in " + col1 + " GROUP BY b.aname,trim(upper(nvl(a.invno,'-'))),to_char(a.invdate,'dd/mm/yyyy'),to_char(a.invdate,'yyyymmdd'),trim(a.ACODE),b.pay_num,trim(a.ACODE)||trim(upper(nvl(a.invno,'-')))||to_char(a.invdate,'dd/mm/yyyy') having SUM(a.dramt) -SUM(a.cramt)<>0 order by to_char(a.invdate,'yyyymmdd'),trim(upper(nvl(a.invno,'-')))";
                    if (hfHOPayRcvConcept.Value == "Y")
                        SQuery = "select a.branchcd||trim(a.ACODE)||trim(upper(nvl(a.invno,'-')))||to_char(a.invdate,'dd/mm/yyyy') as fstr,b.Aname,trim(a.ACODE) as Acode,trim(upper(nvl(a.invno,'-'))) as Bill_No,to_char(a.invdate,'dd/mm/yyyy') as Bill_Dt," + mq_qry1 + ",to_char(a.invdate,'yyyymmdd') as Bill_sort,b.pay_num,a.branchcd,c.name as branch_name,c.acode as branch_code from recdata a,famst b,type c where trim(a.acode)=trim(b.acode) and trim(a.branchcd)=trim(c.type1) and c.id='B' and a.branchcd not in ('DD','88') and trim(a.acode) in (" + (txtacode.Text.Trim().Contains("'") ? txtacode.Text.Trim() : "'" + txtacode.Text.Trim() + "'") + ") and trim(upper(nvl(a.invno,'-'))) not in " + col1 + " GROUP BY b.aname,trim(upper(nvl(a.invno,'-'))),to_char(a.invdate,'dd/mm/yyyy'),to_char(a.invdate,'yyyymmdd'),trim(a.ACODE),b.pay_num,a.branchcd||trim(a.ACODE)||trim(upper(nvl(a.invno,'-')))||to_char(a.invdate,'dd/mm/yyyy'),a.branchcd,c.name,c.acode having SUM(a.dramt) -SUM(a.cramt)<>0 order by to_char(a.invdate,'yyyymmdd'),trim(upper(nvl(a.invno,'-')))";
                }
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "Atch_E")
                {
                    if (frm_vty.Substring(0, 1) == "1")
                    {
                        SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as Voucher_No,to_char(a.vchdate,'dd/mm/yyyy') as Voucher_Dt,b.Aname as Account_Name,b.grp,A.Refnum,to_char(a.refdate,'dd/mm/yyyy') as Refdate,a.dramt as amt,A.TYPE,a.Ent_by,to_Char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(A.rcode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " and a.Dramt>0 order by vdd desc,a.vchnum desc";
                    }
                    else
                    {
                        SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as Voucher_No,to_char(a.vchdate,'dd/mm/yyyy') as Voucher_Dt,b.Aname as Account_Name,b.grp,A.Refnum,to_char(a.refdate,'dd/mm/yyyy') as Refdate,a.cramt as amt,A.TYPE,a.Ent_by,to_Char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(A.rcode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " and a.Cramt>0 order by vdd desc,a.vchnum desc";
                    }

                }
                if (btnval == "New" || btnval == "Edit" || btnval == "Del" || btnval == "Print" || btnval == "List")
                {
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    string my_vid = "";
                    switch (Prg_Id)
                    {
                        case "F70101":
                            my_vid = "1";
                            break;
                        case "F70106":
                            my_vid = "2";
                            break;
                        case "F70111":
                            my_vid = "30";
                            break;

                    }
                    SQuery = "Select a.type1 as fstr,a.Name,a.Type1 as Code,a.Acode as Account,a.Addr1 as Users,b.Aname as Ledger_Name,b.Grp,a.stform as br_cd From Type a left outer join famst b on trim(a.acode)=trim(B.acode) where a.id='V' and a.type1 like '" + my_vid + "%' order by a.type1";
                    SQuery = "select * from (" + SQuery + ") where (case when length(trim(nvl(br_cd,'-')))=2 then instr(nvl(br_cd,'-'),'" + frm_mbr + "') else 1 end)>0 order by Code";


                }
                break;
        }

        if (SQuery.Length > 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
    //----------------------------------------------------------------------------------------

    protected void btnnew_Click(object sender, EventArgs e)
    {
        if (hfHOPayRcvConcept.Value == "Y" && frm_mbr != hfHObr.Value.ToUpper().Trim())
        {
            fgen.msg("-", "AMSG", "You have enabled H.O payment method (W0153)'13'Entries allowed only in " + hfHObr.Value.ToUpper().Trim() + " Branch"); return;
        }
        // for new button popup
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        hf1.Value = "";
        clearctrl();
        set_Val();
        if (chk_rights == "Y")
        {

            hffield.Value = "New";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to add new entry for this form!!");
    }
    //----------------------------------------------------------------------------------------

    protected void btnedit_Click(object sender, EventArgs e)
    {
        if (hfHOPayRcvConcept.Value == "Y" && frm_mbr != hfHObr.Value.ToUpper().Trim())
        {
            fgen.msg("-", "AMSG", "You have enabled H.O payment method (W0153)'13'Entries allowed only in " + hfHObr.Value.ToUpper().Trim() + " Branch"); return;
        }


        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        set_Val();
        if (chk_rights == "Y")
        {
            hffield.Value = "Edit";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to add new entry for this form!!");
    }
    //----------------------------------------------------------------------------------------

    protected void btnsave_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY"); ;
        if (txtCurrnRate.Text.toDouble() == 0)
            txtCurrnRate.Text = "1";
        foreach (GridViewRow gr in sg1.Rows)
        {
            if (gr.Cells[17].Text.toDouble() <= 0)
            {
                gr.Cells[17].Text = "1";
            }
        }

        frm_vty = lbl1a.InnerText.Trim();
        calc();
        // for save button checking & working
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to save data in this form!!");
            return;
        }

        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N" && edmode.Value == "Y")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to save data in edit mode!!");
            return;
        }

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a valid Date"); txtvchdate.Focus(); return; }

        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1")) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2")))
        {
            fgen.msg("-", "AMSG", "Back Year Date is not allowed!!'13'Fill date for this year only");
            txtvchdate.Focus();
            return;
        }

        if (txtremarks.Text.Trim().Length <= 2 && lbl1a.InnerText.Substring(0, 2) == "20")
        {
            fgen.msg("-", "AMSG", "Please put Remarks/Naration '13' For the Voucher");
            txtremarks.Focus();
            return;


        }

        double oth_amt_val = fgen.make_double(txtbalamt.Text, 2);
        if (oth_amt_val < 0)
        {
            oth_amt_val = oth_amt_val * -1;

        }
        if (txtothac.Text.Trim().Length < 6 && oth_amt_val != 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",There is Unadjusted Amount (" + oth_amt_val + ") ,'13'Put the Other A/c Details to Save the voucher");
            return;

        }

        string chk_alent;
        //chk_alent = fgen.seek_iname(frm_qstr, frm_cocd, "select type||'-'||vchnum||'-'||to_char(vchdate,'dd/mm/yyyy') as ldt from voucher where branchcd='" + frm_mbr + "' and type like '" + frm_vty.Substring(0, 1) + "%' and vchdate " + DateRange + " and type||vchnum||to_char(vchdate,'dd/mm/yyyy')='" + frm_vty + txtvchnum.Text + txtvchdate.Text + "' ", "ldt");
        //if (chk_alent != "0")
        //{
        //    Checked_ok = "N";
        //    //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , This Gate Entry Already Entered in MRR No." + chk_alent + ",Please Check, Edit/Save not Allowed !!");
        //    //return;
        //}

        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;

        if (txttrefnum.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Chq No.";
        }

        if (txtchqdt.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Chq Dt.";

        }


        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }

        // cheque checking 
        reqd_flds = "";
        switch (lbl1a.InnerText.Left(1))
        {
            case "1":
                break;
            case "2":
                reqd_flds = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(VCHNUM)||'-'||TO_cHAR(VCHDATE,'DD/MM/YYYY') AS COL1 FROM VOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND TRIM(REFNUM)='" + txttrefnum.Text.Trim() + "' AND TRIM(VCHNUM)!='" + txtvchnum.Text.Trim() + "' ", "COL1");
                if (reqd_flds != "0")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This Chq/DD number is already entered for Entry No " + reqd_flds.Split('-')[0] + " Dated " + reqd_flds.Split('-')[1]);
                    return;
                }
                break;

        }

        // second grid check
        double totgrid2 = 0;
        foreach (GridViewRow gr2 in sg2.Rows)
        {
            totgrid2 += ((TextBox)gr2.FindControl("sg2_t2")).Text.Trim().toDouble();

        }


        string tdsExist = "N";
        double totTDSAmt = 0, totAmountNotinTDS = 0;
        if (frm_vty.Left(1) == "1")
        {
            foreach (GridViewRow gr in sg1.Rows)
            {
                if (gr.Cells[4].Text.Trim().ToUpper().Contains("TDS"))
                {
                    tdsExist = "Y";
                    totTDSAmt += ((TextBox)gr.FindControl("txtmanualfor")).Text.Trim().toDouble();
                }
                else
                    totAmountNotinTDS += ((TextBox)gr.FindControl("txtdedn")).Text.Trim().toDouble();
            }
            if (tdsExist == "Y" && (totTDSAmt != totAmountNotinTDS) && totTDSAmt > 0)
            {
                fgen.msg("-", "AMSG", "TDS Account is selected, Please Adjust TDS Amount against one of the Bill !!");
                return;
            }
        }
        if (totgrid2 > txttamt.Text.toDouble())
        {
            fgen.msg("-", "AMSG", "Installment Amount can not be greater then bank amount. Check 2nd Grid");
            return;
        }

        fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
        btnsave.Disabled = true;
    }
    //----------------------------------------------------------------------------------------
    protected void btndel_Click(object sender, EventArgs e)
    {
        if (hfHOPayRcvConcept.Value == "Y" && frm_mbr != hfHObr.Value.ToUpper().Trim())
        {
            fgen.msg("-", "AMSG", "You have enabled H.O payment method (W0153)'13'Entries allowed only in " + hfHObr.Value.ToUpper().Trim() + " Branch"); return;
        }

        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",You do not have rights to delete data in this form");
        }
        else
        {
            clearctrl();
            set_Val();
            hffield.Value = "Del";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Type to Delete", frm_qstr);
        }
    }
    //----------------------------------------------------------------------------------------

    protected void btnexit_Click(object sender, EventArgs e)
    {
        // for exit button working
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
    //----------------------------------------------------------------------------------------

    protected void btncancel_Click(object sender, EventArgs e)
    {
        // for cancel button working
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();
        dt1 = new DataTable();
        create_tab();
        add_blankrows();
        sg1.DataSource = dt1;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; dt1.Dispose();
        ViewState["sg1"] = null;
        setColHeadings();


        //2nd tab grid
        sg2_dt = new DataTable();
        create_tab2();
        sg2_add_blankrows();
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();





    }
    //----------------------------------------------------------------------------------------

    protected void btnlist_Click(object sender, EventArgs e)
    {
        // for list button 
        clearctrl();
        hffield.Value = "List";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type for List", frm_qstr);
    }
    //----------------------------------------------------------------------------------------

    protected void cmdrep1_Click(object sender, EventArgs e)
    {
        // for doing print
        hffield.Value = "CMD_REP1";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    //----------------------------------------------------------------------------------------
    protected void cmdrep2_Click(object sender, EventArgs e)
    {
        // for doing print
        hffield.Value = "CMD_REP2";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    //----------------------------------------------------------------------------------------

    protected void btnprint_Click(object sender, EventArgs e)
    {
        if (hfHOPayRcvConcept.Value == "Y" && frm_mbr != hfHObr.Value.ToUpper().Trim())
        {
            fgen.msg("-", "AMSG", "You have enabled H.O payment method (W0153)'13'Entries allowed only in " + hfHObr.Value.ToUpper().Trim() + " Branch"); return;
        }
        // for doing print
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
    }
    //----------------------------------------------------------------------------------------

    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
        // for doing multiple work on postback 
        set_Val();
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + popselected.Value + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from WB_PPVCH_DTL a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + popselected.Value + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + popselected.Value + "'");

                if (hfHOPayRcvConcept.Value == "Y")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where trim(a.ST_ENTFORM)='" + popselected.Value + "'");


                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, popselected.Value.Substring(4, 6), popselected.Value.Substring(10, 10), frm_uname, popselected.Value.Substring(2, 2), "Voucher DELETED");
                fgen.msg("-", "AMSG", "Details are deleted for Voucher Entry " + popselected.Value.Substring(4, 6) + "");
                clearctrl(); fgen.ResetForm(this.Controls);
            }
        }
        else if (hffield.Value == "BALEXCEED")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            double totalSelctedAmt = 0, totalChqAmt = 0, myAmt = 0;
            totalChqAmt = txttamt.Text.ToString().toDouble();

            foreach (GridViewRow gr in sg1.Rows)
            {
                if (((CheckBox)gr.FindControl("chk1")).Checked)
                {
                    if (((TextBox)gr.FindControl("txtmanualfor")).Text.ToString().toDouble() > 0)
                        myAmt = ((TextBox)gr.FindControl("txtmanualfor")).Text.ToString().toDouble();
                    else if (((TextBox)gr.FindControl("txtpassfor")).Text.ToString().toDouble() > 0)
                        myAmt = ((TextBox)gr.FindControl("txtpassfor")).Text.ToString().toDouble();
                    else myAmt = gr.Cells[9].Text.toDouble();
                    totalSelctedAmt += myAmt;
                }
            }
            if (totalSelctedAmt > totalChqAmt)
            {
                if (col1 == "Y")
                {
                    if (sg1.Rows[Convert.ToInt32(hf2.Value)].Cells[9].Text.toDouble() > 0)
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf2.Value)].FindControl("txtmanualfor")).Text = (sg1.Rows[Convert.ToInt32(hf2.Value)].Cells[9].Text.toDouble() - (totalSelctedAmt - totalChqAmt)).ToString();
                    else ((TextBox)sg1.Rows[Convert.ToInt32(hf2.Value)].FindControl("txtmanualfor")).Text = (((TextBox)sg1.Rows[Convert.ToInt32(hf2.Value)].FindControl("txtmanualfor")).Text.toDouble() - (totalSelctedAmt - totalChqAmt)).ToString();
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf2.Value)].FindControl("txtmanualfor")).Focus();
                }
                else txtothamt.Text = (Math.Round(totalSelctedAmt - totalChqAmt, 2)).ToString();
            }

            hf1.Value = "";
        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

            switch (btnval)
            {
                case "CMD_REP1":
                    popselected.Value = col1;
                    frm_vty = col1;
                    lbl1a.InnerText = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);

                    fgen.Fn_open_prddmp1("Select Date Range for List Of Bom Listing", frm_qstr);
                    break;

                case "New":
                    if (col1.Length < 2) return;
                    clearctrl();
                    set_Val();
                    popselected.Value = col1;
                    frm_vty = col1;

                    lbl1a.InnerText = col1;

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbltypename.Text = col1 + " : " + col2;


                    string chk_opt = "";
                    string cond = "";
                    cond = " and type='" + frm_vty + "'";
                    switch (lbl1a.InnerText.Left(1))
                    {
                        case "1":
                            chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0191'", "fstr");
                            if (chk_opt == "Y")
                            {
                                cond = " and type like '1%' ";
                            }
                            break;
                        case "2":
                            chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0192'", "fstr");
                            if (chk_opt == "Y")
                            {
                                cond = " and type like '2%' ";
                            }
                            break;

                    }

                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "'  " + cond + " and vchdate " + DateRange + " ", 6, "vch");


                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fill_Date;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.ACODE,B.ANAME FROM TYPE A,FAMST B WHERE TRIM(a.ACODE)=TRIM(b.ACODE) AND A.id='V' and trim(a.type1)='" + col1 + "'");
                    if (dt.Rows.Count > 0)
                    {
                        tbank_code.Text = dt.Rows[0]["acode"].ToString().Trim();
                        tbank_name.Text = dt.Rows[0]["aname"].ToString().Trim();

                        tbank_bal.Text = acBal(tbank_code.Text.Trim().Replace("'", ""));
                    }

                    if (col1.Trim().Length == 2)
                    {
                        if ((col1.Substring(0, 1) == "1" || col1.Substring(0, 1) == "2") && dt.Rows.Count <= 0)
                        {
                            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Account Code linked to Type " + col1 + " is Not Valid !!");
                            return;
                        }
                    }
                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT MAX(REFNUM) AS REFNUM FROM VOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND IS_NUMBER(REFNUM)>1  ", "REFNUM");
                    txttrefnum.Text = (col1.toDouble() + 1).ToString();
                    if (txttrefnum.Text.Length == 1) txttrefnum.Text = (col1.toDouble() + 1).ToString().PadLeft(3, '0');
                    txtchqdt.Text = DateTime.Now.ToString("yyyy-MM-dd");


                    //2nd tab grid
                    sg2_dt = new DataTable();
                    create_tab2();
                    for (i = 0; i < 12; i++)
                    {
                        sg2_add_blankrows();

                    }


                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    setColHeadings();
                    ViewState["sg2"] = sg2_dt;


                    disablectrl(); fgen.EnableForm(this.Controls);
                    btnlbl4.Focus();
                    break;
                case "Del":
                    if (col1.Length < 2) return;
                    clearctrl();
                    set_Val();
                    hffield.Value = "Del_E";
                    popselected.Value = col1;
                    lbl1a.InnerText = col1;
                    frm_vty = col1;
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Voucher to delete", frm_qstr);
                    break;
                case "Del_E":
                    if (col1.Length < 2) return;
                    clearctrl();
                    popselected.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;
                case "Edit":
                    // this is after type selection 
                    if (col1.Length < 2) return;
                    clearctrl();
                    set_Val();
                    hffield.Value = "Edit_E";
                    frm_vty = col1;
                    lbl1a.InnerText = col1;
                    tbank_code.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to edit", frm_qstr);
                    break;
                case "Edit_E":
                    if (col1.Length < 2) return;
                    // this is after entry selection
                    popselected.Value = col1;
                    tbank_code.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT Acode FROM Type WHERE id='V' and TRIM(type1)='" + lbl1a.InnerText.Trim() + "' ", "ACODE");
                    tbank_name.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(aCODE)='" + tbank_code.Text.Trim() + "' ", "ANAME");

                    string mcol1 = "";
                    mcol1 = col1;
                    if (frm_vty.Substring(0, 1) == "1")
                    {
                        SQuery = "select c.acode as typcode,a.*,b.aname from voucher a,famst b,type c where c.id='V' and a.type=c.type1 and trim(A.rcode)=trim(B.acode) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "'  order by a.srno";
                        if (hfHOPayRcvConcept.Value == "Y")
                            SQuery = "select c.acode as typcode,a.*,b.aname from voucher a,famst b,type c where c.id='V' and a.type=c.type1 and trim(A.rcode)=trim(B.acode) and (a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' or a.st_entform='" + col1 + "' ) and a.oscl=1 order by a.type,a.srno";
                    }
                    else
                    {
                        SQuery = "select c.acode as typcode,a.*,b.aname from voucher a,famst b,type c where c.id='V' and a.type=c.type1 and trim(A.rcode)=trim(B.acode) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' and trim(a.acode)!='" + tbank_code.Text.Trim() + "' order by a.srno";
                        if (hfHOPayRcvConcept.Value == "Y")
                            SQuery = "select c.acode as typcode,a.*,b.aname from voucher a,famst b,type c where c.id='V' and a.type=c.type1 and trim(A.rcode)=trim(B.acode) and (a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' or a.st_entform='" + col1 + "' ) and a.oscl=1 and trim(a.acode)!='" + tbank_code.Text.Trim() + "' order by a.type,a.srno";
                    }

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txttamt.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7");
                        // Filing textbox of the form
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim(); txtvchdate.Text = dt.Rows[0]["vchdate"].ToString().Trim();

                        txttrefnum.Text = dt.Rows[0]["refnum"].ToString().Trim();
                        txtchqdt.Text = Convert.ToDateTime(fgen.make_def_Date(dt.Rows[0]["refdate"].ToString().Trim(), DateTime.Now.ToString("dd/MM/yyyy"))).ToString("yyyy-MM-dd");

                        txtremarks.Text = dt.Rows[0]["naration"].ToString().Trim();

                        tslip_no.Text = dt.Rows[0]["ref1"].ToString().Trim();
                        tslip_Name.Text = dt.Rows[0]["stform"].ToString().Trim();


                        if (frm_vty.Substring(0, 1) == "1")
                        {
                            txtacode.Text = dt.Rows[0]["rcode"].ToString().Trim();
                            txtaname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(aCODE)='" + txtacode.Text.Trim().Replace("'", "") + "' ", "ANAME");
                        }
                        else
                        {
                            txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                            txtaname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(aCODE)='" + txtacode.Text.Trim().Replace("'", "") + "' ", "ANAME");
                        }

                        ViewState["ent_by"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["ent_dt"] = dt.Rows[0]["ent_dAtE"].ToString();

                        double fullamt = 0;
                        col1 = ""; col2 = ""; col3 = "";
                        create_tab();
                        int firrow = 0;
                        if (frm_vty.Substring(0, 1) == "2") firrow = 1;
                        if (hfHOPayRcvConcept.Value == "Y") firrow = 1;

                        foreach (DataRow dr in dt.Rows)
                        {
                            if (firrow > 0)
                            {
                                dr1 = dt1.NewRow();
                                dr1["acode"] = dr["acode"].ToString().Trim();
                                mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME||'^'||payment as aname FROM FAMST WHERE ACODE='" + dr["acode"].ToString().Trim() + "'", "aname");
                                dr1["aname"] = mq0.Split('^')[0];
                                dr1["invno"] = dr["invno"].ToString().Trim();
                                dr1["invdate"] = Convert.ToDateTime(fgen.make_def_Date(dr["invdate"].ToString().Trim(), DateTime.Now.ToString("dd/MM/yyyy"))).ToString("yyyy-MM-dd");
                                dr1["damt"] = dr["fcrate"].ToString().Trim();
                                dr1["camt"] = dr["fcrate1"].ToString().Trim();
                                dr1["net"] = fgen.make_double(dr1["damt"].ToString().toDouble() - dr1["camt"].ToString().toDouble(), 2);
                                //if (frm_vty.Substring(0, 1) == "1")
                                //{
                                //    fullamt += dr["dramt"].ToString().Trim().toDouble() + dr["cramt"].ToString().Trim().toDouble();
                                //    dr1["manualamt"] = dr["dramt"].ToString().Trim().toDouble() + dr["cramt"].ToString().Trim().toDouble();
                                //    dr1["passamt"] = dr["dramt"].ToString().Trim().toDouble() + dr["cramt"].ToString().Trim().toDouble();
                                //}
                                //else
                                {
                                    fullamt += dr["dramt"].ToString().Trim().toDouble() + dr["cramt"].ToString().Trim().toDouble();
                                    dr1["manualamt"] = dr["dramt"].ToString().Trim().toDouble() + dr["cramt"].ToString().Trim().toDouble();
                                    dr1["passamt"] = dr["dramt"].ToString().Trim().toDouble() + dr["cramt"].ToString().Trim().toDouble();
                                }
                                dr1["cumbal"] = fullamt;
                                dr1["rmk"] = dr["naration"].ToString().Trim();
                                //if (frm_vty.Substring(0, 1) == "1" && dr["DEPTT"].ToString().Trim() == "OTH")
                                //{
                                //    txtothac.Text = dr["acode"].ToString().Trim();
                                //    txtothname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(aCODE)='" + txtothac.Text.Trim() + "' ", "ANAME");
                                //    txtothamt.Text = dr["dramt"].ToString().Trim();
                                //}
                                //else
                                {
                                    dt1.Rows.Add(dr1);
                                    col1 += "," + "'" + dr1["acode"] + "'";
                                    col2 += "," + "'" + dr1["aname"] + "'";
                                    if (mq0.Contains("^"))
                                        col3 += "," + mq0.Split('^')[1];
                                }

                                dr1["hfdd"] = dr["DEPTT"].ToString().Trim();
                            }
                            firrow++;
                        }
                        add_blankrows();
                        ViewState["sg1"] = dt1;
                        sg1.DataSource = dt1;
                        sg1.DataBind();
                        foreach (GridViewRow gr in sg1.Rows)
                        {
                            ((CheckBox)gr.FindControl("chk1")).Checked = true;
                        }

                        txttamt.Text = (fullamt - txtothamt.Text.toDouble()).ToString();
                        if (frm_vty.Substring(0, 1) == "1")
                            txttamt.Text = dt.Compute("sum(CRAMT)", "").ToString();

                        //txtacode.Text = col1.TrimStart(',');
                        //txtaname.Text = col2.TrimStart(',');
                        //lblPayTerms.Text = col3.TrimStart(',');


                        lblPayTerms.Text = "Terms:" + fgen.seek_iname(frm_qstr, frm_cocd, "SELECT nvl(pay_num,0) as pay_num FROM famst WHERE TRIM(acode)='" + txtacode.Text.Trim() + "'", "pay_num");



                        //------------------------ 2nd grid for exp amortization option
                        SQuery = "Select nvl(AMTZ_xpcode,'-') as AMTZ_xpcode,to_char(nvl(a.AMTZ_DATE,a.vchdate),'dd/mm/yyyy') as AMTZ_dt,nvl(a.AMTZ_amt,0) as AMTZ_amt from WB_PPVCH_DTL a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + mcol1 + "' ORDER BY a.srno";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        create_tab2();
                        sg2_dr = null;
                        int fill_rows = 0;
                        if (dt.Rows.Count > 0)
                        {
                            txt_expcode.Text = dt.Rows[i]["AMTZ_xpcode"].ToString().Trim();

                            if (txt_expcode.Text.Trim().Length > 1)
                            {
                                txt_expname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(aCODE)='" + txt_expcode.Text.Trim() + "' ", "ANAME");
                            }
                            for (i = 0; i < dt.Rows.Count; i++)
                            {

                                sg2_dr = sg2_dt.NewRow();
                                sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;

                                sg2_dr["sg2_t1"] = dt.Rows[i]["AMTZ_dt"].ToString().Trim();
                                sg2_dr["sg2_t2"] = dt.Rows[i]["AMTZ_amt"].ToString().Trim();

                                sg2_dt.Rows.Add(sg2_dr);
                                fill_rows = fill_rows + 1;
                            }
                        }

                        for (i = fill_rows; i < 12; i++)
                        {

                            sg2_add_blankrows();
                        }


                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        dt.Dispose();
                        sg2_dt.Dispose();

                        ////-----------------------
                        //sg2_add_blankrows();
                        //ViewState["sg2"] = sg2_dt;
                        //sg2.DataSource = sg2_dt;
                        //sg2.DataBind();
                        //dt.Dispose();
                        //sg2_dt.Dispose();
                        //------------------------

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                        setDropDown();

                        tbank_bal.Text = acBal(tbank_code.Text.Trim().Replace("'", ""));
                        tparty_bal.Text = acBal(txtacode.Text.Trim().Replace("'", ""));
                    }
                    break;
                case "TACODE":
                    if (col1.Length < 2 && fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL1").Length < 2) return;
                    frm_vty = popselected.Value;
                    txtacode.Text = col1;
                    txtaname.Text = col2;
                    lblPayTerms.Text = "Terms:" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    txttrefnum.Focus();

                    // new working
                    if (hf3.Value == "Y")
                    {
                        txtacode.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL1").Split(':')[1];
                        txtaname.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL1").Split(':')[0];

                        txtCurrn.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2");
                        txtCurrnRate.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL11");
                    }

                    tparty_bal.Text = acBal(txtacode.Text.Trim().Replace("'", ""));

                    if (lbl1a.InnerText.Substring(0, 1) == "1")
                    {
                        SQuery = "select b.aname,nvl(b.pay_num,0) as pay_num,trim(upper(nvl(a.invno,'-'))) as invno,a.invdate,to_char(SUM(a.dramt),'999999999.99') as dramt,to_char(SUM(a.cramt),'999999999.99') as cramt,to_char(SUM(a.dramt) -SUM(a.cramt),'999999999.99') as NET ,trim(a.ACODE) as acode,'-' as rmk from recdata a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and trim(a.acode) " + (txtacode.Text.Contains("'") ? " in (" + txtacode.Text.Trim() + ")" : "='" + txtacode.Text.Trim().Replace("'", "") + "'") + " GROUP BY b.aname,nvl(b.pay_num,0),trim(upper(nvl(a.invno,'-'))),a.INVDATE,trim(a.ACODE) having SUM(a.dramt) -SUM(a.cramt)<>0 order by a.INVDATE,trim(upper(nvl(a.invno,'-')))";
                    }
                    else
                    {
                        SQuery = "select b.aname,nvl(b.pay_num,0) as pay_num,trim(upper(nvl(a.invno,'-'))) as invno,a.invdate,to_char(SUM(a.cramt),'999999999.99') as dramt,to_char(SUM(a.dramt),'999999999.99') as cramt ,to_Char(SUM(a.cramt) -SUM(a.dramt),'999999999.99') as NET ,trim(a.ACODE) as acode,'-' as rmk from recdata a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and trim(a.acode) " + (txtacode.Text.Contains("'") ? " in (" + txtacode.Text.Trim() + ")" : "='" + txtacode.Text.Trim().Replace("'", "") + "'") + " GROUP BY b.aname,nvl(b.pay_num,0),trim(upper(nvl(a.invno,'-'))),a.INVDATE,trim(a.ACODE) having SUM(a.dramt) -SUM(a.cramt)<>0 order by a.INVDATE,trim(upper(nvl(a.invno,'-')))";
                    }
                    string chkon_str = "";
                    string chkon_Ac = "";
                    chkon_str = "select count(*) as cnt from (" + SQuery + ")";
                    chkon_Ac = fgen.seek_iname(frm_qstr, frm_cocd, chkon_str, "cnt");

                    create_tab();
                    if (fgen.getOption(frm_qstr, frm_cocd, "W0147", "OPT_ENABLE") == "Y")
                    {
                        if (fgen.make_double(chkon_Ac) == 0)
                        {
                            SQuery = "select '-' as Aname,0 as pay_num,'ON A/C' as invno,to_char(sysdate,'dd/mm/yyyy') As invdate,0 as dramt,0 as cramt,0 as NET,'-' as rmk ,'" + txtacode.Text.Replace("'", "") + "' as ACODE from dual ";
                        }
                        if (hf3.Value == "Y")
                        {
                            if (fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL5") == "ADVANCE" || fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL5") == "ON A/C")
                                SQuery = "select '-' as Aname,0 as pay_num,'" + fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL5") + "' as invno,to_char(sysdate,'dd/mm/yyyy') As invdate,'" + (fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL4").toDouble() * fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL11").toDouble()) + "' as dramt,0 as cramt,'" + (fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL4").toDouble() * fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL11").toDouble()) + "' as NET ,'" + txtacode.Text.Replace("'", "") + "' as ACODE,'Bill No. " + fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL6") + " Dt : " + fgen.make_def_Date(fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL7"), txtvchdate.Text) + " for " + fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL4") + " " + fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2") + " @ " + fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL11") + "' as rmk from dual ";
                        }

                        dt = new DataTable();
                        dt1 = new DataTable();
                        create_tab();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        cum_bal = 0;

                        foreach (DataRow dr in dt.Rows)
                        {
                            dr1 = dt1.NewRow();
                            dr1["acode"] = dr["acode"];
                            dr1["aname"] = dr["aname"];
                            dr1["invno"] = dr["invno"];
                            dr1["invdate"] = Convert.ToDateTime(fgen.make_def_Date(dr["invdate"].ToString(), DateTime.Now.ToString("dd/MM/yyyy"))).ToString("yyyy-MM-dd");
                            dr1["camt"] = dr["cramt"];
                            dr1["damt"] = dr["dramt"];
                            dr1["net"] = dr["net"];
                            dr1["passamt"] = 0;
                            cum_bal = cum_bal + Math.Round(dr["NET"].ToString().toDouble(), 2);
                            dr1["cumbal"] = cum_bal;
                            dr1["manualamt"] = 0;

                            //Math.Round(Convert.ToDouble(dr["Dramt"]) - Convert.ToDouble(dr["cramt"]), 2).ToString();

                            dr1["rmk"] = dr["rmk"];

                            txtremarks.Text = dr["rmk"].ToString();
                            //dr1["duedt"] = Convert.ToDateTime(dr1["invdate"].ToString()).AddDays(fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().toDouble()).ToString("dd/MM/yyyy");
                            dr1["duedt"] = Convert.ToDateTime(dr1["invdate"].ToString()).AddDays(dr["pay_num"].ToString().toDouble()).ToString("dd/MM/yyyy");

                            if (lbl1a.InnerText.Substring(0, 1) == "1")
                                dr1["hfdd"] = "CR";

                            dr1["hfLock"] = "Y";

                            if (hf3.Value == "Y")
                            {
                                // filling old rate of Fx 
                                mq0 = "Select nvl(tfccr,0)-nvl(tfcdr,0)||'~'||tfcr as val from voucher where branchcd!='DD' and (type like '5%' or (type in ('31','32'))) and trim(Acode)='" + txtacode.Text + "' and upper(Trim(invno))='" + dr1["invno"].ToString().Trim() + "' and to_chaR(invdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr1["invdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "'";
                                col3 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "val");
                                if (col3 == "0" || (col3.Split('~')[0].toDouble() == 0 && col3.Split('~')[1].toDouble() == 0))
                                {
                                    mq0 = "Select tfccr||'~'||tfcr as val from voucher where branchcd!='DD' and type like '2%' and trim(Acode)='" + txtacode.Text + "' and upper(Trim(invno))='" + dr1["invno"].ToString().Trim() + "' and to_chaR(invdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr1["invdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "'";
                                    col3 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "val");
                                    if (col3 == "0" || (col3.Split('~')[0].toDouble() == 0 && col3.Split('~')[1].toDouble() == 0))
                                    {
                                        mq0 = "Select tfccr||'~'||tfcr as val from voucher where branchcd!='DD' and trim(Acode)='" + txtacode.Text + "' and upper(Trim(invno))='" + dr1["invno"].ToString().Trim() + "' and to_chaR(invdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr1["invdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "'";
                                        col3 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "val");
                                    }
                                }
                                if (col3.Contains("~"))
                                {
                                    dr1["orig_fx_bal"] = col3.Split('~')[1];
                                    dr1["curr_fx_bal"] = 0;
                                    dr1["orig_fx_amt"] = col3.Split('~')[0];

                                    if (frm_vty.Left(1) == "1")
                                    {
                                        dr1["orig_fx_amt"] = (dr["dramt"].ToString().toDouble() - dr["cramt"].ToString().toDouble()) / col3.Split('~')[1].toDouble();
                                    }
                                }
                            }

                            dt1.Rows.Add(dr1);
                        }
                    }
                    add_blankrows();
                    sg1.DataSource = dt1; sg1.DataBind(); ViewState["sg1"] = dt1;
                    setColHeadings();

                    setDropDown();

                    break;
                case "RMK":
                    txtremarks.Text = col2;
                    break;
                case "TICODE":
                    txtothac.Text = col1; txtothname.Text = col2;
                    break;
                case "EMPCODE":
                    tslip_Name.Text = col1 + ":" + col2;
                    //txtothname.Text = col2;
                    break;
                case "EXPCODE":
                    txt_expcode.Text = col1;
                    txt_expname.Text = col2;
                    break;

                case "Print":
                    if (col1.Length < 2) return;
                    popselected.Value = col1;
                    frm_vty = col1;
                    lbl1a.InnerText = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Voucher Type to Print", frm_qstr);
                    break;
                case "SAVED":
                    //hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                        fgen.fin_acct_reps(frm_qstr);
                    }
                    break;
                case "Atch_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1.Substring(4, 16));
                    fgen.open_fileUploadPopup("Upload File for " + lblheader.Text, frm_qstr);
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "" + col1 + "");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_acct_reps(frm_qstr);
                    break;
                case "List":
                    popselected.Value = col1;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    fgen.Fn_open_prddmp1("Select Date Range for List Of Vouchers", frm_qstr);
                    //fgen.Fn_open_Act_itm_prd("-", frm_qstr);
                    break;
                case "FORX":
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL1").toDouble() > 0)
                    {
                        ((CheckBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("chk1")).Checked = true;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("txtmanualfor")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2");
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL1");
                    }
                    break;
                case "Row_Add":
                    if (ViewState["sg1"] != null)
                    {
                        dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        dt1 = dt.Clone();
                        dr1 = null;
                        cum_bal = 0;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            dr1 = dt1.NewRow();
                            dr1["srno"] = dt1.Rows.Count + 1;
                            dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                            dr1["ANAME"] = dt.Rows[i]["ANAME"].ToString().Trim();
                            dr1["invno"] = ((TextBox)sg1.Rows[i].FindControl("txtInvno")).Text;
                            dr1["invdate"] = ((TextBox)sg1.Rows[i].FindControl("txtInvDt")).Text;
                            dr1["camt"] = dt.Rows[i]["camt"].ToString().Trim();
                            dr1["damt"] = dt.Rows[i]["damt"].ToString().Trim();
                            dr1["net"] = dt.Rows[i]["net"].ToString().Trim();
                            dr1["passamt"] = ((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text;
                            dr1["cumbal"] = "0";
                            dr1["manualamt"] = ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text;
                            dr1["rmk"] = ((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text;
                            dr1["duedt"] = ((TextBox)sg1.Rows[i].FindControl("txtDueDt")).Text;
                            dr1["hfM"] = dt.Rows[i]["hfM"].ToString().Trim();
                            if (((CheckBox)sg1.Rows[i].FindControl("chk1")).Checked)
                                dr1["hfChk"] = "Y";
                            else dr1["hfChk"] = "N";

                            dr1["hfdd"] = ((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value;
                            dr1["hfLock"] = ((HiddenField)sg1.Rows[i].FindControl("hfLock")).Value;
                            cum_bal = cum_bal + Math.Round(dr1["NET"].ToString().toDouble(), 2);
                            dr1["cumbal"] = cum_bal;
                            dr1["BR_ACODE"] = sg1.Rows[i].Cells[21].Text.Trim();
                            dt1.Rows.Add(dr1);
                        }
                        // here

                        DataTable dtBills = new DataTable();
                        SQuery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
                        dtBills = fgen.getdata(frm_qstr, frm_cocd, "SELECT * FROM ( " + SQuery + ") WHERE FSTR IN (" + col1 + ") ");
                        for (int x = 0; x < dtBills.Rows.Count; x++)
                        {
                            dr1 = dt1.NewRow();
                            dr1["srno"] = dt1.Rows.Count + 1;
                            dr1["acode"] = dtBills.Rows[x]["ACODE"].ToString().Trim();
                            dr1["ANAME"] = dtBills.Rows[x]["ANAME"].ToString().Trim();
                            dr1["invno"] = dtBills.Rows[x]["BILL_NO"].ToString().Trim();
                            dr1["invdate"] = Convert.ToDateTime(fgen.make_def_Date(dtBills.Rows[x]["BILL_DT"].ToString().Trim(), txtvchdate.Text)).ToString("yyyy-MM-dd");
                            dr1["damt"] = dtBills.Rows[x]["BAL_AMT"].ToString().Trim();
                            dr1["camt"] = 0;

                            dr1["net"] = dtBills.Rows[x]["BAL_AMT"].ToString().Trim();
                            dr1["passamt"] = 0;
                            dr1["cumbal"] = "0";
                            dr1["manualamt"] = 0;
                            dr1["rmk"] = "-";
                            dr1["hfM"] = "Y";
                            cum_bal = cum_bal + Math.Round(dr1["NET"].ToString().toDouble(), 2);
                            dr1["cumbal"] = cum_bal;
                            dr1["hfChk"] = "Y";
                            if (lbl1a.InnerText.Substring(0, 1) == "1")
                                dr1["hfdd"] = "CR";
                            else dr1["hfdd"] = "DR";

                            dr1["duedt"] = Convert.ToDateTime(dr1["invdate"].ToString()).AddDays(dtBills.Rows[x]["pay_num"].ToString().toDouble()).ToString("dd/MM/yyyy");

                            if (hf3.Value == "Y")
                            {
                                // filling old rate of Fx 
                                mq0 = "Select nvl(tfccr,0)-nvl(tfcdr,0)||'~'||tfcr as val from voucher where branchcd!='DD' and (type like '5%' or (type in ('31','32'))) and trim(Acode)='" + txtacode.Text + "' and upper(Trim(invno))='" + dr1["invno"].ToString().Trim() + "' and to_chaR(invdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr1["invdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "'";
                                col3 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "val");
                                if (col3 == "0" || (col3.Split('~')[0].toDouble() == 0 && col3.Split('~')[1].toDouble() == 0))
                                {
                                    mq0 = "Select tfccr||'~'||tfcr as val from voucher where branchcd!='DD' and type like '2%' and trim(Acode)='" + txtacode.Text + "' and upper(Trim(invno))='" + dr1["invno"].ToString().Trim() + "' and to_chaR(invdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr1["invdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "'";
                                    col3 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "val");
                                    if (col3 == "0" || (col3.Split('~')[0].toDouble() == 0 && col3.Split('~')[1].toDouble() == 0))
                                    {
                                        mq0 = "Select tfccr||'~'||tfcr as val from voucher where branchcd!='DD' and trim(Acode)='" + txtacode.Text + "' and upper(Trim(invno))='" + dr1["invno"].ToString().Trim() + "' and to_chaR(invdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr1["invdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "'";
                                        col3 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "val");
                                    }
                                }
                                if (col3.Contains("~"))
                                {
                                    dr1["orig_fx_bal"] = col3.Split('~')[1];
                                    dr1["curr_fx_bal"] = 0;
                                    dr1["orig_fx_amt"] = col3.Split('~')[0];
                                }
                            }
                            if (hfHOPayRcvConcept.Value == "Y")
                                dr1["br_acode"] = dtBills.Rows[x]["branchcd"].ToString().Trim() + "-" + dtBills.Rows[x]["branch_code"].ToString().Trim();

                            dt1.Rows.Add(dr1);
                        }
                    }
                    add_blankrows();

                    ViewState["sg1"] = dt1;
                    sg1.DataSource = dt1;
                    sg1.DataBind();

                    setDropDown();


                    //((TextBox)sg1.Rows[z].FindControl("txtchlqty")).Focus();
                    break;
                case "Row_Edit":
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = col2;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("txtInvno")).Text = col3;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("txtInvDt")).Text = Convert.ToDateTime(fgen.make_def_Date(fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5"), txtvchdate.Text)).ToString("yyyy-MM-dd");

                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[7].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6"); ;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[8].Text = "0";
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[9].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6"); ;
                    break;
                case "Rmv":
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        ViewState["sg1"] = dt;
                        sg1.DataSource = dt;
                        sg1.DataBind();
                        dt.Dispose();
                    }
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

                case "BILLMSG":

                    col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                    {
                        hfBillMSG.Value = "";
                        if (col1 == "Y") hfBillMSG.Value = "BILL";
                        int index = Convert.ToInt32(hf1.Value);
                        if (index < sg1.Rows.Count - 1)
                        {
                            hf1.Value = index.ToString();
                            hffield.Value = "Row_Edit";
                            make_qry_4_popup();
                            fgen.Fn_open_sseek("Select Account", frm_qstr);
                        }
                        else
                        {
                            hffield.Value = "Row_Add";
                            make_qry_4_popup();
                            fgen.Fn_open_mseek("Select Account", frm_qstr);
                        }
                    }
                    break;
            }
        }
    }
    //----------------------------------------------------------------------------------------
    void setDropDown()
    {
        if (sg1.Rows.Count > 0)
        {
            foreach (GridViewRow gr in sg1.Rows)
            {
                if (((HiddenField)gr.FindControl("hfdd")).Value == "DR" || ((HiddenField)gr.FindControl("hfdd")).Value == "CR")
                    ((HtmlSelect)gr.FindControl("dd2")).Value = ((HiddenField)gr.FindControl("hfdd")).Value;
            }
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        // for doing save action 
        if (hffield.Value == "List")
        {
            frm_vty = popselected.Value.Trim();
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");

            SQuery = "select a.vchnum as vOUCHER_NO,to_char(a.vchdate,'dd/mm/yyyy') as VCH_Dt,b.aname as accounts,a.dramt,a.cramt,a.srno,A.TYPE,to_Char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + " order by vdd desc,a.vchnum desc,a.srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("" + lblheader.Text + " List for the period of " + fromdt + " to " + todt, frm_qstr);
        }
        else if (hffield.Value == "CMD_REP1")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "select vchnum,vchdate,icode,ent_by,ent_Dt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + popselected.Value.Trim() + "' and vchdate " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE") + " order by vchdate,vchnum");
            fgen.Fn_open_rptlevel("Entry List for the period of " + fromdt + " to " + todt, frm_qstr);
        }
        else
        {
            col1 = "";
            set_Val();
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "N")
            {
                btnsave.Disabled = false;
            }
            else
            {
                try
                {
                    //myfun();
                    calc();
                    oDS = new DataSet();
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                    oporow3 = null;
                    oDS3 = new DataSet();
                    oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "WB_PPVCH_DTL");


                    // This is for checking that, is it ready to save the data
                    frm_vnum = "000000";
                    save_data();

                    save_fun3();


                    // check total dramt and cramt
                    double myCheckDr = 0, myCheckCr = 0;
                    if (oDS.Tables[0].Rows.Count > 0)
                    {

                        ///COMMENTED ON 12/07/2022 TO BYPASS CHECKING
                        foreach (DataRow drCheck in oDS.Tables[0].Rows)
                        {
                            myCheckDr += drCheck["DRAMT"].ToString().toDouble(2);
                            myCheckCr += drCheck["CRAMT"].ToString().toDouble(2);
                        }


                    }
                    if (myCheckCr.toDouble(2) != myCheckDr.toDouble(2))
                        //if (txtbillamount.Text.Trim() != txttamt.Text.Trim())
                    {
                        btnsave.Disabled = false;
                        fgen.msg("", "AMSG", "Total Debit and Credit amount is not matching, Please check again before saving'13' Dr Amount : " + myCheckDr + "'13' Cr Amount : " + myCheckCr + " !!");
                        return;
                    }


                    oDS.Dispose();
                    oporow = null;
                    oDS = new DataSet();
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                    oDS3.Dispose();
                    oporow3 = null;
                    oDS3 = new DataSet();
                    oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "WB_PPVCH_DTL");


                    if (edmode.Value == "Y")
                        frm_vnum = txtvchnum.Text.Trim();
                    else
                    {
                        i = 0;
                        //do
                        //{
                        //    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum)+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                        //    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname + frm_mbr + frm_vty + frm_vnum + System.DateTime.Now.ToString("dd/MM/yyyy"), frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                        //    i++;
                        //}
                        //while (pk_error == "Y");

                        //if (save_it == "Y")
                        //{


                        string chk_opt = "";
                        string continueNumberSer = "N";
                        switch (lbl1a.InnerText.Left(1))
                        {
                            case "1":
                                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0191'", "fstr");
                                if (chk_opt == "Y")
                                {
                                    continueNumberSer = "Y";
                                }
                                break;
                            case "2":
                                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0192'", "fstr");
                                if (chk_opt == "Y")
                                {
                                    continueNumberSer = "Y";
                                }
                                break;

                        }

                        string doc_is_ok = "";
                        if (continueNumberSer == "Y") frm_vnum = fgen.Fn_next_doc_no_inv(frm_qstr, frm_cocd, frm_tabname, "VCHNUM", "VCHDATE", frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                        else frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, "VCHNUM", "VCHDATE", frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                        doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                        if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }


                    }
                    save_data();
                    save_fun3();

                    if (edmode.Value == "Y")
                    {
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='88' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + popselected.Value.Trim() + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update WB_PPVCH_DTL set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + popselected.Value.Trim() + "'");
                    }

                    //btnsave.Disabled = false;
                    //if (1 == 2)
                    {
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        fgen.save_data(frm_qstr, frm_cocd, oDS3, "WB_PPVCH_DTL");

                        //fgen.send_mail("Tejaxo ERP","pkgupta@Tejaxo.in","","","ITEWSTAGE",""

                        if (edmode.Value == "Y")
                        {

                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd='88' and type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + popselected.Value.ToString().Substring(2, 18) + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from WB_PPVCH_DTL where branchcd='DD' and type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + popselected.Value.ToString().Substring(2, 18) + "'");
                            if (frm_vty.Substring(0, 1) == "1")
                            {
                                fgen.msg("-", "CMSG", "Voucher No." + frm_vnum + " Updated Successfully'13'Do You want to Print The Voucher ");
                            }
                            else
                            {
                                fgen.msg("-", "CMSG", "Voucher No." + frm_vnum + " Updated Successfully'13'Do You want to Print Voucher");
                            }
                        }
                        else
                        {
                            if (frm_vty.Substring(0, 1) == "1")
                            {
                                fgen.msg("-", "CMSG", "Voucher No." + frm_vnum + " Saved Successfully'13'Do You want to Print The Voucher ");
                            }
                            else
                            {
                                fgen.msg("-", "CMSG", "Voucher No." + frm_vnum + " Saved Successfully'13'Do You want to Print Voucher");
                            }
                        }

                        #region Email Sending Function
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        //html started                            
                        sb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
                        sb.Append("<h3>" + fgenCO.chk_co(frm_cocd) + "</h3>");
                        sb.Append("<h5>" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR_NAME") + "</h5>");
                        sb.Append("<br>Dear Sir/Mam,<br> This is to advise that, Payment voucher has been passed for Amount : <b>" + txttamt.Text.Trim() + " </b>");
                        sb.Append("<br>Party Name : " + txtaname.Text.Trim());
                        sb.Append("<br>Cheque / DD Number : " + txttrefnum.Text.Trim());
                        sb.Append("<br>Cheque / DD Date : " + txtchqdt.Text.Trim());

                        sb.Append("<br><br>Thanks & Regards");
                        sb.Append("<h5>Note: This Report is Auto generated from Tejaxo ERP. The above details are to the best of information <br> and data available to the ERP System. For any discrepancy/ clarification kindly get in touch with the concerned official. </h5>");
                        sb.Append("</body></html>");

                        //send mail
                        string subj = "";
                        if (edmode.Value == "Y") subj = "Edited : ";
                        else subj = "New Entry : ";
                        fgen.send_Activity_mail(frm_qstr, frm_cocd, "Tejaxo ERP", frm_formID, subj + lblheader.Text + " #" + frm_vnum, sb.ToString(), frm_uname);


                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr"), frm_uname, edmode.Value);

                        sb.Clear();
                        #endregion

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim() + "'");
                        fgen.ResetForm(this.Controls);
                        fgen.DisableForm(this.Controls);
                        enablectrl();
                        clearctrl();
                        col1 = "N";
                        hffield.Value = "SAVED";
                        setColHeadings();
                    }
                }
                catch (Exception ex)
                {
                    btnsave.Disabled = false;
                    fgen.msg("-", "AMSG", ex.Message.ToString());
                    col1 = "N";
                }
            }
        }
    }
    //----------------------------------------------------------------------------------------

    public void create_tab()
    {
        // for making a table structure
        dt1 = new DataTable();
        dr1 = null;
        dt1.Columns.Add(new DataColumn("srno", typeof(Int32)));
        dt1.Columns.Add(new DataColumn("acode", typeof(string)));
        dt1.Columns.Add(new DataColumn("aname", typeof(string)));
        dt1.Columns.Add(new DataColumn("Invno", typeof(string)));
        dt1.Columns.Add(new DataColumn("invdate", typeof(string)));
        dt1.Columns.Add(new DataColumn("camt", typeof(string)));
        dt1.Columns.Add(new DataColumn("damt", typeof(string)));
        dt1.Columns.Add(new DataColumn("net", typeof(string)));

        dt1.Columns.Add(new DataColumn("passamt", typeof(double)));
        dt1.Columns.Add(new DataColumn("manualamt", typeof(double)));

        dt1.Columns.Add(new DataColumn("cumbal", typeof(string)));
        dt1.Columns.Add(new DataColumn("rmk", typeof(string)));
        dt1.Columns.Add(new DataColumn("duedt", typeof(string)));
        dt1.Columns.Add(new DataColumn("hfM", typeof(string)));
        dt1.Columns.Add(new DataColumn("hfChk", typeof(string)));
        dt1.Columns.Add(new DataColumn("hfdd", typeof(string)));
        dt1.Columns.Add(new DataColumn("hfLock", typeof(string)));
        dt1.Columns.Add(new DataColumn("orig_fx_bal", typeof(string)));
        dt1.Columns.Add(new DataColumn("curr_fx_bal", typeof(string)));
        dt1.Columns.Add(new DataColumn("orig_fx_amt", typeof(string)));
        dt1.Columns.Add(new DataColumn("TaxDedn", typeof(string)));
        dt1.Columns.Add(new DataColumn("br_acode", typeof(string)));
    }
    //----------------------------------------------------------------------------------------

    public void add_blankrows()
    {
        // for making a blank table row 
        if (dt1 == null) return;
        dr1 = dt1.NewRow();
        dr1["acode"] = "-";
        dr1["invno"] = "-";
        dr1["invdate"] = "-";
        dr1["camt"] = "0";
        dr1["damt"] = "0";
        dr1["net"] = "0";
        dr1["passamt"] = 0;
        dr1["manualamt"] = "0";
        dr1["cumbal"] = 0;
        dr1["rmk"] = "-";
        dt1.Rows.Add(dr1);
    }
    //----------------------------------------------------------------------------------------
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
    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();
        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
        sg2_dr["sg2_t1"] = "-";

        sg2_dr["sg2_t2"] = "-";
        sg2_dt.Rows.Add(sg2_dr);
    }


    protected void hptacode_Click(object sender, ImageClickEventArgs e)
    {
        // for popup in header block for item /party 
        hffield.Value = "TACODE";
        make_qry_4_popup();


        if (frm_vty.Substring(0, 1) == "1") fgen.Fn_open_mseek("Select Party Name", frm_qstr);
        else fgen.Fn_open_sseek("Select Party Name", frm_qstr);
    }
    //----------------------------------------------------------------------------------------
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

    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            // for options in GRID add, rmv etc
            string var = e.CommandName.ToString();
            int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
            int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

            switch (var)
            {
                case "SG1_RMV":
                    if (index < sg1.Rows.Count - 1)
                    {
                        hf1.Value = index.ToString();
                        hffield.Value = "Rmv";
                        fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove this item from list");
                    }
                    break;
                case "Row_Add":
                    if (fgen.getOption(frm_qstr, frm_cocd, "W0147", "OPT_ENABLE") != "Y" && lbl1a.InnerText.Substring(0, 1) != "2")
                    {
                        if (txttamt.Text.toDouble() <= 0)
                        {
                            fgen.msg("-", "AMSG", "Please Fill Cheque Amount before Proceeding!!");
                            txttamt.Focus();
                            return;
                        }
                    }

                    hf1.Value = index.ToString();
                    hffield.Value = "BILLMSG";
                    fgen.msg("-", "CMSG", "Do you want to Select Invoice'13'or'13'Post on other account entry ?");
                    break;
                case "btnForx":
                    Session["Filled"] = "Y";
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "M_COL1", sg1.Rows[index].Cells[19].Text.Trim());
                    fgenMV.Fn_Set_Mvar(frm_qstr, "M_COL2", (sg1.Rows[index].Cells[19].Text.Trim().toDouble() * txtCurrnRate.Text.toDouble()).ToString());
                    fgenMV.Fn_Set_Mvar(frm_qstr, "M_COL3", txtCurrnRate.Text.toDouble().ToString());
                    hffield.Value = "FORX";
                    fgen.Fn_ValueBoxMultiple("Please Enter forex rate", frm_qstr, "300px", "180px");
                    break;
            }
        }
        catch { }
    }
    //----------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        // for word wrap in case of large text , makes grid if std size
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[4].Attributes.Add("style", "white-space: nowrap;");
            //ViewState["OrigData"] = e.Row.Cells[4].Text;
            //if (e.Row.Cells[4].Text.Length >= 25) //Just change the value of 30 based on your requirements
            //{
            //    e.Row.Cells[4].Text = e.Row.Cells[4].Text.Substring(0, 25) + "...";
            //    e.Row.Cells[4].ToolTip = ViewState["OrigData"].ToString();
            //}

            //e.Row.Cells[0].Style["display"] = "none";
            //sg1.HeaderRow.Cells[0].Style["display"] = "none";

            if (((HiddenField)e.Row.FindControl("hfM")).Value == "Y")
            {
                ((TextBox)e.Row.FindControl("txtInvno")).ReadOnly = false;
                ((TextBox)e.Row.FindControl("txtInvDt")).ReadOnly = false;
            }
            else
            {
                ((TextBox)e.Row.FindControl("txtInvno")).ReadOnly = true;
                ((TextBox)e.Row.FindControl("txtInvDt")).ReadOnly = true;
            }

            if (((HiddenField)e.Row.FindControl("hfChk")).Value == "Y")
                ((CheckBox)e.Row.FindControl("chk1")).Checked = true;
            else ((CheckBox)e.Row.FindControl("chk1")).Checked = false;

            if (((HiddenField)e.Row.FindControl("hfLock")).Value == "Y")
            {
                ((HtmlSelect)e.Row.FindControl("dd2")).Disabled = true;
            }
            if (((TextBox)e.Row.FindControl("txtInvno")).Text.ToUpper() == "ON A/C")
            {
                ((TextBox)e.Row.FindControl("txtInvDt")).ReadOnly = false;
            }

            currdt = DateTime.Now.ToString("dd/MM/yyyy");

            ((TextBox)e.Row.FindControl("txtpassfor")).Attributes.Add("readonly", "readonly");

            if (((TextBox)e.Row.FindControl("txtDueDt")).Text.Length > 2)
            {
                if (((TextBox)e.Row.FindControl("txtInvno")).Text.ToUpper() != "-")
                {
                    if (Convert.ToDateTime(currdt) > Convert.ToDateTime(((TextBox)e.Row.FindControl("txtDueDt")).Text))
                    {
                        e.Row.BackColor = System.Drawing.Color.LightGreen;
                    }
                    else
                    {
                        e.Row.BackColor = System.Drawing.Color.LightPink;
                    }
                }
            }
            if (((TextBox)e.Row.FindControl("txtInvno")).Text.ToUpper() == "N/F")
            {
                e.Row.BackColor = System.Drawing.Color.Yellow;
            }
        }
    }
    //----------------------------------------------------------------------------------------
    void cal()
    {
        // for calculation in grid
        double vp = 0;
        //for (int zk = 0; zk < sg1.Rows.Count - 1; zk++)
        {
            //vp1 = Convert.ToDouble(((TextBox)sg1.Rows[zk].FindControl("txtfld1")).Text.Trim());
            //vp += vp1;
        }
        lblqtysum.InnerHtml = vp.ToString();
    }
    //----------------------------------------------------------------------------------------
    public void myfun() { }
    //{
    //    vip = ""; mq1 = "ContentPlaceHolder1_";
    //    vip = vip + "<script type='text/javascript'>function calculateSum() {";
    //    vip = vip + "var vp=0;var vp1=0; var fill_amt=0;";
    //    mq0 = "";
    //    for (int zk = 0; zk < sg1.Rows.Count; zk++)
    //    {

    //        //vip = vip + " if(fill_zero(document.getElementById('ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "').value) > 0) {document.getElementById('ContentPlaceHolder1_sg1_chk1_" + zk + "').checked = true; }";

    //        //vip = vip + " else {document.getElementById('ContentPlaceHolder1_sg1_chk1_" + zk + "').checked = false; }";

    //        vip = vip + "var chk_result" + zk + " = document.getElementById('ContentPlaceHolder1_sg1_chk1_" + zk + "').checked;";
    //        vip = vip + "if(chk_result" + zk + "==true) { ";

    //        // added this to check for chq num and date before checkbox                                                
    //        vip = vip + "if (document.getElementById('ContentPlaceHolder1_lbl1a').innerText.substring(0, 1) == 1) { if(document.getElementById('ContentPlaceHolder1_txttrefnum').value=='' || document.getElementById('ContentPlaceHolder1_txttrefnum').value=='-') { document.getElementById('ContentPlaceHolder1_hf1').value='CHQMSG'; document.getElementById('ContentPlaceHolder1_sg1_chk1_" + zk + "').checked = false; openBox(); return false;}" +
    //            "if((document.getElementById('ContentPlaceHolder1_txttamt').value*1)<=0 ) { document.getElementById('ContentPlaceHolder1_hf1').value='CHQAMSG'; document.getElementById('ContentPlaceHolder1_sg1_chk1_" + zk + "').checked = false; openBox(); return false;}" +
    //        "}";

    //        vip = vip + "if (document.getElementById('ContentPlaceHolder1_lbl1a').innerText.substring(0, 1) == 2) { if(document.getElementById('ContentPlaceHolder1_txttrefnum').value=='' || document.getElementById('ContentPlaceHolder1_txttrefnum').value=='-') { document.getElementById('ContentPlaceHolder1_hf1').value='CHQMSG'; document.getElementById('ContentPlaceHolder1_sg1_chk1_" + zk + "').checked = false; openBox(); return false;}" +
    //        "}";

    //        vip = vip + "document.getElementById('ContentPlaceHolder1_sg1_txtpassfor_" + zk + "').value= fill_zero('" + sg1.Rows[zk].Cells[9].Text.Trim() + "'); ";
    //        vip = vip + " if(fill_zero(document.getElementById('ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "').value) > 0) { fill_amt = document.getElementById('ContentPlaceHolder1_sg1_txtmanualfor_" + zk + "').value; }";
    //        vip = vip + " else { fill_amt = document.getElementById('ContentPlaceHolder1_sg1_txtpassfor_" + zk + "').value; }";

    //        vip = vip + "}";
    //        vip = vip + "else { document.getElementById('ContentPlaceHolder1_sg1_txtpassfor_" + zk + "').value= 0; fill_amt = 0; }";

    //        vip = vip + "vp=(vp*1) + (document.getElementById('ContentPlaceHolder1_sg1_txtpassfor_" + zk + "').value * 1);";

    //        vip = vip + "vp1=(vp1*1) + (fill_amt * 1);";

    //        //if ((i + 1) < sg1.Rows.Count)
    //        //    ((TextBox)sg1.Rows[i].FindControl("txtrmk")).Attributes.Add("onkeypress", "return clickEnter('" + ((CheckBox)sg1.Rows[i + 1].FindControl("chk1")).ClientID + "', event)");            
    //    }

    //    vip = vip + "document.getElementById('ContentPlaceHolder1_lblqtysum').innerHTML = vp; ";
    //    vip = vip + "document.getElementById('ContentPlaceHolder1_txtbillamount').value = vp1; ";
    //    //vip = vip + "document.getElementById('ContentPlaceHolder1_txtbalamt').value = fill_zero(document.getElementById('ContentPlaceHolder1_txtbillamount').value) - fill_zero(document.getElementById('ContentPlaceHolder1_txttamt').value); ";

    //    vip = vip + "if (document.getElementById('ContentPlaceHolder1_lbl1a').innerText.substring(0, 1) == 2){document.getElementById('ContentPlaceHolder1_txttamt').value = fill_zero(document.getElementById('ContentPlaceHolder1_txtbillamount').value);}";
    //    vip = vip + "else {document.getElementById('ContentPlaceHolder1_txtbalamt').value = fill_zero(document.getElementById('ContentPlaceHolder1_txtbillamount').value) - fill_zero(document.getElementById('ContentPlaceHolder1_txttamt').value);}";

    //    vip = vip + "if (document.getElementById('ContentPlaceHolder1_lbl1a').innerText.substring(0, 1)==1) {" +
    //    "if(((document.getElementById('ContentPlaceHolder1_txtbillamount').value*1)>(document.getElementById('ContentPlaceHolder1_txttamt').value*1)) && document.getElementById('ContentPlaceHolder1_hf1').value!='BALEXCEED') { document.getElementById('ContentPlaceHolder1_hf1').value='BALEXCEED'; openBox(); return false;}}";

    //    ///vipin to correct      
    //    //vip = vip + "debugger;";
    //    //vip = vip + "debugger;";
    //    ///vipin to correct

    //    vip = vip + "}";
    //    vip = vip + "function fill_zero(val){ if(isNaN(val)) return 0; if(isFinite(val)) return val; }</script>";
    //    ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", vip.ToString(), false);

    //}
    //(fill_zero(t1) + fill_zero(t2) + fill_zero(t3)).toFixed(3);
    //vip = vip + "document.getElementById('ContentPlaceHolder1_txtbalamt').value = fill_zero(document.getElementById('ContentPlaceHolder1_txtbillamount').value) - fill_zero(document.getElementById('ContentPlaceHolder1_txttamt').value); ";
    //----------------------------------------------------------------------------------------
    public void calc()
    {
        double double_vp = 0, double_Vp1 = 0, fill_amt = 0, othAmt = 0;

        for (int zk = 0; zk < sg1.Rows.Count; zk++)
        {
            CheckBox chk1 = ((CheckBox)sg1.Rows[zk].FindControl("chk1"));
            if (chk1.Checked == true)
            {
                ((TextBox)sg1.Rows[zk].FindControl("txtpassfor")).Text = sg1.Rows[zk].Cells[9].Text.Trim();
                if (lbl1a.InnerText.Substring(0, 1) == "2")
                {
                    if (((HtmlSelect)sg1.Rows[zk].FindControl("dd2")).Value == "DR")
                    {
                        if (fgen.make_double(((TextBox)sg1.Rows[zk].FindControl("txtmanualfor")).Text) > 0) fill_amt = fgen.make_double(((TextBox)sg1.Rows[zk].FindControl("txtmanualfor")).Text);
                        else fill_amt = fgen.make_double(((TextBox)sg1.Rows[zk].FindControl("txtpassfor")).Text);

                        double_Vp1 += fill_amt;
                    }
                    else othAmt += fgen.make_double(((TextBox)sg1.Rows[zk].FindControl("txtmanualfor")).Text);
                }
                else
                {
                    if (((HtmlSelect)sg1.Rows[zk].FindControl("dd2")).Value == "CR")
                    {
                        if (fgen.make_double(((TextBox)sg1.Rows[zk].FindControl("txtmanualfor")).Text) > 0) fill_amt = fgen.make_double(((TextBox)sg1.Rows[zk].FindControl("txtmanualfor")).Text);
                        else fill_amt = fgen.make_double(((TextBox)sg1.Rows[zk].FindControl("txtpassfor")).Text);

                        double_Vp1 += fill_amt;
                    }
                    else othAmt += fgen.make_double(((TextBox)sg1.Rows[zk].FindControl("txtmanualfor")).Text);
                }
            }
            else
            {
                fill_amt = 0;
                ((TextBox)sg1.Rows[zk].FindControl("txtpassfor")).Text = "0";
            }
        }

        txtbillamount.Text = double_Vp1.ToString();
        txtbalamt.Text = ((fgen.make_double(txtbillamount.Text) * 1) - (fgen.make_double(txttamt.Text) + othAmt)).ToString();
    }
    //----------------------------------------------------------------------------------------
    protected void btnh_Click(object sender, EventArgs e)
    {
        // to add row on pressing enter in grid
        ((ImageButton)sg1.Rows[0].FindControl("btnadd")).Focus();
    }
    //----------------------------------------------------------------------------------------
    void save_data()
    {
        // to save data into virtual table and then final database    
        //string frm_ent_time = fgen.Fn_curr_dt_time(frm_cocd, frm_qstr);

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY"); ;
        frm_vty = lbl1a.InnerText.Trim();
        double tot_dramt = 0;
        double tot_cramt = 0;
        string vardate;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        string my_nar = "";
        my_nar = txtremarks.Text;
        int srno = 0; double largest_value_amt = 0, to_fill_amt = 0, passamt = 0, finalAmountToSave = 0; string largest_value_acode = "";
        if (frm_vty.Substring(0, 1) == "1") { srno = 50; } else srno = 1;

        //grid saving
        string auto_nar = "";
        for (i = 0; i <= sg1.Rows.Count - 1; i++)
        {
            if (((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text.ToString().toDouble() + ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.ToString().toDouble() != 0)
            {
                auto_nar = auto_nar + "|" + ((TextBox)sg1.Rows[i].FindControl("txtInvno")).Text;
            }
        }

        if (my_nar.Trim().Length <= 2)
        {
            if (auto_nar.Length > 150)
            {
                auto_nar = auto_nar.Substring(1, 150);
            }
            if (lbl1a.InnerText.Substring(0, 1) == "1")
            {
                my_nar = "Rcvd Agst " + auto_nar + ", via Chq/DD. " + txttrefnum.Text + " Dated " + txtchqdt.Text;
            }
            else
            {
                my_nar = "Paid Agst " + auto_nar + ", via Chq/DD. " + txttrefnum.Text + " Dated " + txtchqdt.Text;
            }

        }
        if (txtremarks.Text.Trim().Length <= 2)
        {
            txtremarks.Text = my_nar;
        }

        // largest code
        DataTable dtLr = ((DataTable)ViewState["sg1"]).Clone();
        string zamt = "";
        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            dr1 = dtLr.NewRow();
            dr1["srno"] = i + 1;
            dr1["acode"] = sg1.Rows[i].Cells[3].Text.ToString().Trim();
            dr1["ANAME"] = sg1.Rows[i].Cells[4].Text;
            dr1["invno"] = ((TextBox)sg1.Rows[i].FindControl("txtInvno")).Text;
            dr1["invdate"] = ((TextBox)sg1.Rows[i].FindControl("txtInvDt")).Text;

            dr1["camt"] = fgen.make_double(sg1.Rows[i].Cells[7].Text);
            dr1["damt"] = fgen.make_double(sg1.Rows[i].Cells[8].Text);
            dr1["net"] = fgen.make_double(sg1.Rows[i].Cells[9].Text);

            dr1["passamt"] = ((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text;
            dr1["cumbal"] = "0";
            dr1["manualamt"] = ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text;

            dr1["rmk"] = ((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text;
            dr1["duedt"] = ((TextBox)sg1.Rows[i].FindControl("txtDueDt")).Text;
            //dr1["hfM"] = dt.Rows[i]["hfM"].ToString().Trim();
            if (((CheckBox)sg1.Rows[i].FindControl("chk1")).Checked)
                dr1["hfChk"] = "Y";
            else dr1["hfChk"] = "N";

            dr1["hfdd"] = ((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value;
            dr1["hfLock"] = ((HiddenField)sg1.Rows[i].FindControl("hfLock")).Value;
            cum_bal = cum_bal + Math.Round(dr1["NET"].ToString().toDouble(), 2);
            dr1["cumbal"] = cum_bal;
            dr1["BR_ACODE"] = sg1.Rows[i].Cells[21].Text.Trim().ToUpper().Replace("&NBSP;", "");
            dtLr.Rows.Add(dr1);
        }


        int largamtPassAmt = (dtLr.Compute("max(passamt)", string.Empty)).ToString().toInt();

        int largamtManualAmt = (dtLr.Compute("max(manualamt)", string.Empty)).ToString().toInt();

        if (largamtPassAmt > largamtManualAmt)
        {
            //largest_value_acode = fgen.seek_iname_dt(dtLr, "passamt=" + largamtPassAmt + "", "ACODE");
            DataView dv = new DataView(dtLr, "", "passamt desc", DataViewRowState.CurrentRows);
            if (dv.Count > 0)
                largest_value_acode = dv[0].Row["ACODE"].ToString();
        }
        else
        {
            DataView dv = new DataView(dtLr, "", "manualamt desc", DataViewRowState.CurrentRows);
            if (dv.Count > 0)
                largest_value_acode = dv[0].Row["ACODE"].ToString();
        }

        string tdsExist = "N", tdsAccountCD = "";
        double totTDSAmt = 0, totAmountNotinTDS = 0;

        foreach (GridViewRow gr in sg1.Rows)
        {
            if (gr.Cells[4].Text.Trim().ToUpper().Contains("TDS"))
            {
                tdsExist = "Y";
                totTDSAmt += ((TextBox)gr.FindControl("txtmanualfor")).Text.Trim().toDouble();
                tdsAccountCD = gr.Cells[3].Text.Trim();
            }
            else
                totAmountNotinTDS += ((TextBox)gr.FindControl("txtdedn")).Text.Trim().toDouble();
        }


        // H.O Payment Receive Concept w0153 - 33 type entry



        if (hfHOPayRcvConcept.Value == "Y" && (lbl1a.InnerText.Substring(0, 1) == "1" || lbl1a.InnerText.Substring(0, 1) == "2"))
        {

            if (frm_cocd == "MPAC")
            {
                save_multi_frm_vch_mpac();
            }

            else
            {
                save_multi_frm_vch();
            }

        }

        else
        {
            for (i = 0; i <= sg1.Rows.Count - 1; i++)
            {
                if (((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text.ToString().toDouble() + ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.ToString().toDouble() != 0)
                {
                    #region FX Gain / Loss
                    if (sg1.Rows[i].Cells[17].Text.toDouble() != txtCurrnRate.Text.toDouble())
                    {
                        to_fill_amt = 0;
                        oporow = oDS.Tables[0].NewRow();
                        oporow["BRANCHCD"] = frm_mbr;
                        oporow["TYPE"] = frm_vty;
                        oporow["vchnum"] = frm_vnum;
                        oporow["vchdate"] = txtvchdate.Text.Trim();
                        oporow["srno"] = srno;

                        oporow["oscl"] = 0;
                        oporow["quantity"] = 0;

                        oporow["FCTYPE"] = txtCurrn.Text;
                        oporow["TFCR"] = txtCurrnRate.Text;

                        oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                        oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                        oporow["ACODE"] = sg1.Rows[i].Cells[3].Text.Trim();
                        oporow["RCODE"] = tbank_code.Text.Trim();
                        if (largest_value_acode == "0") largest_value_acode = tbank_code.Text.Trim();
                        if (sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "06" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "16" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "05") { }
                        else oporow["RCODE"] = largest_value_acode;

                        oporow["invno"] = ((TextBox)sg1.Rows[i].FindControl("txtInvno")).Text.ToUpper();
                        oporow["invdate"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("txtInvDt")).Text, vardate);

                        oporow["fcrate"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[7].Text.Trim()));
                        oporow["fcrate1"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[8].Text.Trim()));

                        if (largest_value_amt > 0)
                        {
                            passamt = ((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text.toDouble();
                            if (((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim().toDouble() > 0)
                                passamt = ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim().toDouble();
                            if (passamt > largest_value_amt)
                            {
                                largest_value_amt = passamt;
                            }
                        }
                        else
                        {
                            largest_value_amt = passamt;
                        }


                        if (((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim().toDouble() > 0)
                        {
                            to_fill_amt = ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim().toDouble();
                        }
                        else
                        {
                            to_fill_amt = ((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text.Trim().toDouble();
                        }

                        to_fill_amt = Math.Round(sg1.Rows[i].Cells[17].Text.toDouble() * sg1.Rows[i].Cells[18].Text.toDouble());
                        finalAmountToSave = to_fill_amt;
                        oporow["fcdramt"] = 0;
                        oporow["fccramt"] = 0;

                        //to_fill_amt = 2;
                        if (frm_vty.Substring(0, 1) == "1")
                        {
                            if (to_fill_amt > 0)
                            {
                                oporow["dramt"] = 0;
                                oporow["Cramt"] = Math.Abs(to_fill_amt);

                                oporow["tfcdr"] = 0;
                                oporow["tfccr"] = Math.Abs(to_fill_amt);

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = Math.Round(sg1.Rows[i].Cells[18].Text.toDouble(), 2);

                                if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "DR")
                                {
                                    oporow["dramt"] = Math.Abs(to_fill_amt);
                                    oporow["Cramt"] = 0;

                                    oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                    oporow["tfccr"] = 0;

                                    oporow["FCDRAMT"] = Math.Round(sg1.Rows[i].Cells[18].Text.toDouble(), 2);
                                    oporow["FCCRAMT"] = 0;
                                }
                            }
                            else
                            {
                                oporow["dramt"] = Math.Abs(to_fill_amt);
                                oporow["Cramt"] = 0;

                                oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                oporow["tfccr"] = 0;

                                oporow["FCDRAMT"] = Math.Round(sg1.Rows[i].Cells[18].Text.toDouble(), 2);
                                oporow["FCCRAMT"] = 0;

                                if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "CR")
                                {
                                    oporow["dramt"] = 0;
                                    oporow["Cramt"] = Math.Abs(to_fill_amt);

                                    oporow["tfcdr"] = 0;
                                    oporow["tfccr"] = Math.Abs(to_fill_amt);

                                    oporow["FCDRAMT"] = 0;
                                    oporow["FCCRAMT"] = Math.Round(sg1.Rows[i].Cells[18].Text.toDouble(), 2);
                                }
                            }
                            tot_cramt = tot_cramt + to_fill_amt;
                        }
                        else
                        {

                            if (to_fill_amt < 0)
                            {
                                oporow["dramt"] = 0;
                                oporow["Cramt"] = Math.Abs(to_fill_amt);

                                oporow["tfcdr"] = 0;
                                oporow["tfccr"] = Math.Abs(to_fill_amt);

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = Math.Round(sg1.Rows[i].Cells[18].Text.toDouble(), 2);

                                if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "DR")
                                {
                                    oporow["dramt"] = Math.Abs(to_fill_amt);
                                    oporow["Cramt"] = 0;

                                    oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                    oporow["tfccr"] = 0;

                                    oporow["FCDRAMT"] = Math.Round(sg1.Rows[i].Cells[18].Text.toDouble(), 2);
                                    oporow["FCCRAMT"] = 0;
                                }
                            }
                            else
                            {
                                oporow["dramt"] = Math.Abs(to_fill_amt);
                                oporow["Cramt"] = 0;

                                oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                oporow["tfccr"] = 0;

                                oporow["FCDRAMT"] = Math.Round(sg1.Rows[i].Cells[18].Text.toDouble(), 2);
                                oporow["FCCRAMT"] = 0;

                                if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "CR")
                                {
                                    oporow["dramt"] = 0;
                                    oporow["Cramt"] = Math.Abs(to_fill_amt);

                                    oporow["tfcdr"] = 0;
                                    oporow["tfccr"] = Math.Abs(to_fill_amt);

                                    oporow["FCDRAMT"] = 0;
                                    oporow["FCCRAMT"] = Math.Round(sg1.Rows[i].Cells[18].Text.toDouble(), 2);
                                }
                            }


                            tot_dramt = tot_dramt + to_fill_amt;
                        }


                        oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                        oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

                        if (((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text.Trim().Length <= 1)
                        {
                            oporow["naration"] = my_nar.Trim();
                        }
                        else
                        {
                            oporow["naration"] = ((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text.Trim();
                        }


                        oporow["tax"] = "-";
                        oporow["stax"] = 0;
                        oporow["post"] = 0;

                        oporow["grno"] = "-";
                        oporow["grdate"] = vardate;
                        oporow["mrndate"] = vardate;

                        oporow["DEPTT"] = ((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value;
                        //oporow["app_Date"] = System.DateTime.Now;

                        if (edmode.Value == "Y")
                        {
                            oporow["ent_by"] = ViewState["ent_by"].ToString();
                            oporow["ent_date"] = ViewState["ent_dt"].ToString();
                            oporow["edt_by"] = frm_uname;
                            oporow["edt_date"] = vardate;
                        }
                        else
                        {
                            oporow["ent_by"] = frm_uname;
                            oporow["ent_date"] = vardate;
                            oporow["edt_by"] = "-";
                            oporow["edt_date"] = vardate;
                        }
                        oDS.Tables[0].Rows.Add(oporow);
                        srno++;

                        //***************************************************************


                        to_fill_amt = 0;
                        oporow = oDS.Tables[0].NewRow();
                        oporow["BRANCHCD"] = frm_mbr;
                        oporow["TYPE"] = frm_vty;
                        oporow["vchnum"] = frm_vnum;
                        oporow["vchdate"] = txtvchdate.Text.Trim();
                        oporow["srno"] = srno;

                        oporow["oscl"] = 0;
                        oporow["quantity"] = 0;

                        oporow["FCTYPE"] = txtCurrn.Text;
                        oporow["TFCR"] = txtCurrnRate.Text;

                        oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                        oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                        mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='" + (sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "06" ? "A94" : "A76") + "'", "params");
                        oporow["ACODE"] = mq1;
                        oporow["RCODE"] = sg1.Rows[i].Cells[3].Text.Trim();

                        if (sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "06" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "16" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "05") { }
                        else oporow["RCODE"] = largest_value_acode;

                        oporow["invno"] = ((TextBox)sg1.Rows[i].FindControl("txtInvno")).Text.ToUpper();
                        oporow["invdate"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("txtInvDt")).Text, vardate);

                        oporow["fcrate"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[7].Text.Trim()));
                        oporow["fcrate1"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[8].Text.Trim()));

                        to_fill_amt = Math.Round(finalAmountToSave - ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim().toDouble());

                        oporow["fcdramt"] = 0;
                        oporow["fccramt"] = 0;

                        //to_fill_amt = 2;
                        if (frm_vty.Substring(0, 1) == "1")
                        {
                            if (to_fill_amt > 0)
                            {
                                oporow["dramt"] = 0;
                                oporow["Cramt"] = Math.Abs(to_fill_amt);

                                oporow["tfcdr"] = 0;
                                oporow["tfccr"] = Math.Abs(to_fill_amt);

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = 0;

                                if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "DR")
                                {
                                    oporow["dramt"] = Math.Abs(to_fill_amt);
                                    oporow["Cramt"] = 0;

                                    oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                    oporow["tfccr"] = 0;

                                    oporow["FCDRAMT"] = 0;
                                    oporow["FCCRAMT"] = 0;
                                }
                            }
                            else
                            {
                                oporow["dramt"] = Math.Abs(to_fill_amt);
                                oporow["Cramt"] = 0;

                                oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                oporow["tfccr"] = 0;

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = 0;

                                if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "CR")
                                {
                                    oporow["dramt"] = 0;
                                    oporow["Cramt"] = Math.Abs(to_fill_amt);

                                    oporow["tfcdr"] = 0;
                                    oporow["tfccr"] = Math.Abs(to_fill_amt);

                                    oporow["FCDRAMT"] = 0;
                                    oporow["FCCRAMT"] = 0;
                                }
                            }
                            tot_cramt = tot_cramt + to_fill_amt;
                        }
                        else
                        {

                            if (to_fill_amt < 0)
                            {
                                oporow["dramt"] = 0;
                                oporow["Cramt"] = Math.Abs(to_fill_amt);

                                oporow["tfcdr"] = 0;
                                oporow["tfccr"] = Math.Abs(to_fill_amt);

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = 0;

                                if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "DR")
                                {
                                    oporow["dramt"] = Math.Abs(to_fill_amt);
                                    oporow["Cramt"] = 0;

                                    oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                    oporow["tfccr"] = 0;

                                    oporow["FCDRAMT"] = 0;
                                    oporow["FCCRAMT"] = 0;
                                }
                            }
                            else
                            {
                                oporow["dramt"] = Math.Abs(to_fill_amt);
                                oporow["Cramt"] = 0;

                                oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                oporow["tfccr"] = 0;

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = 0;

                                if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "CR")
                                {
                                    oporow["dramt"] = 0;
                                    oporow["Cramt"] = Math.Abs(to_fill_amt);

                                    oporow["tfcdr"] = 0;
                                    oporow["tfccr"] = Math.Abs(to_fill_amt);

                                    oporow["FCDRAMT"] = 0;
                                    oporow["FCCRAMT"] = 0;
                                }
                            }


                            tot_dramt = tot_dramt + to_fill_amt;
                        }


                        oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                        oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

                        if (((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text.Trim().Length <= 1)
                        {
                            oporow["naration"] = my_nar.Trim();
                        }
                        else
                        {
                            oporow["naration"] = ((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text.Trim();
                        }


                        oporow["tax"] = "-";
                        oporow["stax"] = 0;
                        oporow["post"] = 0;

                        oporow["grno"] = "-";
                        oporow["grdate"] = vardate;
                        oporow["mrndate"] = vardate;

                        oporow["DEPTT"] = ((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value;
                        //oporow["app_Date"] = System.DateTime.Now;

                        if (edmode.Value == "Y")
                        {
                            oporow["ent_by"] = ViewState["ent_by"].ToString();
                            oporow["ent_date"] = ViewState["ent_dt"].ToString();
                            oporow["edt_by"] = frm_uname;
                            oporow["edt_date"] = vardate;
                        }
                        else
                        {
                            oporow["ent_by"] = frm_uname;
                            oporow["ent_date"] = vardate;
                            oporow["edt_by"] = "-";
                            oporow["edt_date"] = vardate;
                        }
                        oDS.Tables[0].Rows.Add(oporow);
                        srno++;
                    }
                    #endregion
                    #region Other
                    else
                    {
                        to_fill_amt = 0;

                        oporow = oDS.Tables[0].NewRow();
                        oporow["BRANCHCD"] = frm_mbr;
                        oporow["TYPE"] = frm_vty;
                        oporow["vchnum"] = frm_vnum;
                        oporow["vchdate"] = txtvchdate.Text.Trim();
                        oporow["srno"] = srno;

                        oporow["oscl"] = 0;
                        oporow["quantity"] = 0;

                        oporow["FCTYPE"] = txtCurrn.Text;
                        oporow["TFCR"] = txtCurrnRate.Text;

                        oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                        oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                        oporow["ACODE"] = sg1.Rows[i].Cells[3].Text.Trim();
                        oporow["RCODE"] = tbank_code.Text.Trim();

                        ///removed 22/6/2021
                        //if (sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "06" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "16" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "05") { }
                        //else oporow["RCODE"] = largest_value_acode;

                        oporow["invno"] = ((TextBox)sg1.Rows[i].FindControl("txtInvno")).Text.ToUpper();
                        oporow["invdate"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("txtInvDt")).Text, vardate);

                        oporow["fcrate"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[7].Text.Trim()));
                        oporow["fcrate1"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[8].Text.Trim()));

                        if (largest_value_amt > 0)
                        {
                            passamt = ((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text.toDouble();
                            if (((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim().toDouble() > 0)
                                passamt = ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim().toDouble();
                            if (passamt > largest_value_amt)
                            {
                                largest_value_amt = passamt;
                            }
                        }
                        else
                        {
                            largest_value_amt = passamt;
                        }


                        if (((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim().toDouble() > 0)
                        {
                            to_fill_amt = ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim().toDouble();
                        }
                        else
                        {
                            to_fill_amt = ((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text.Trim().toDouble();
                        }

                        totTDSAmt = 0;
                        if (frm_vty.Left(1) == "1")
                        {
                            if (((TextBox)sg1.Rows[i].FindControl("txtdedn")).Text.Trim().toDouble() > 0)
                            {
                                totTDSAmt = ((TextBox)sg1.Rows[i].FindControl("txtdedn")).Text.Trim().toDouble();
                                to_fill_amt = (to_fill_amt - ((TextBox)sg1.Rows[i].FindControl("txtdedn")).Text.Trim().toDouble());
                            }
                        }
                        oporow["fcdramt"] = 0;
                        oporow["fccramt"] = 0;

                        //to_fill_amt = 2;
                        if (frm_vty.Substring(0, 1) == "1")
                        {
                            if (to_fill_amt > 0)
                            {
                                oporow["dramt"] = 0;
                                oporow["Cramt"] = Math.Abs(to_fill_amt);

                                oporow["tfcdr"] = 0;
                                oporow["tfccr"] = Math.Abs(to_fill_amt);

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);

                                if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "DR")
                                {
                                    oporow["dramt"] = Math.Abs(to_fill_amt);
                                    oporow["Cramt"] = 0;

                                    oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                    oporow["tfccr"] = 0;

                                    oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                    oporow["FCCRAMT"] = 0;
                                }
                            }
                            else
                            {
                                oporow["dramt"] = Math.Abs(to_fill_amt);
                                oporow["Cramt"] = 0;

                                oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                oporow["tfccr"] = 0;

                                oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                oporow["FCCRAMT"] = 0;

                                if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "CR")
                                {
                                    oporow["dramt"] = 0;
                                    oporow["Cramt"] = Math.Abs(to_fill_amt);

                                    oporow["tfcdr"] = 0;
                                    oporow["tfccr"] = Math.Abs(to_fill_amt);

                                    oporow["FCDRAMT"] = 0;
                                    oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                }
                            }
                            tot_cramt = tot_cramt + to_fill_amt;
                        }
                        else
                        {

                            if (to_fill_amt < 0)
                            {
                                oporow["dramt"] = 0;
                                oporow["Cramt"] = Math.Abs(to_fill_amt);

                                oporow["tfcdr"] = 0;
                                oporow["tfccr"] = Math.Abs(to_fill_amt);

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);

                                //if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "DR")
                                //{
                                //    oporow["dramt"] = Math.Abs(to_fill_amt);
                                //    oporow["Cramt"] = 0;

                                //    oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                //    oporow["tfccr"] = 0;

                                //    oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                //    oporow["FCCRAMT"] = 0;
                                //}
                            }
                            else
                            {
                                oporow["dramt"] = Math.Abs(to_fill_amt);
                                oporow["Cramt"] = 0;

                                oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                oporow["tfccr"] = 0;

                                oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                oporow["FCCRAMT"] = 0;

                                if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "CR")
                                {
                                    oporow["dramt"] = 0;
                                    oporow["Cramt"] = Math.Abs(to_fill_amt);

                                    oporow["tfcdr"] = 0;
                                    oporow["tfccr"] = Math.Abs(to_fill_amt);

                                    oporow["FCDRAMT"] = 0;
                                    oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                }
                            }


                            tot_dramt = tot_dramt + to_fill_amt;
                        }


                        oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                        oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

                        if (((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text.Trim().Length <= 1)
                        {
                            oporow["naration"] = my_nar.Trim();
                        }
                        else
                        {
                            oporow["naration"] = ((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text.Trim();
                        }


                        oporow["tax"] = "-";
                        oporow["stax"] = 0;
                        oporow["post"] = 0;

                        oporow["grno"] = "-";
                        oporow["grdate"] = vardate;
                        oporow["mrndate"] = vardate;

                        oporow["DEPTT"] = ((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value;
                        //oporow["app_Date"] = System.DateTime.Now;

                        if (edmode.Value == "Y")
                        {
                            oporow["ent_by"] = ViewState["ent_by"].ToString();
                            oporow["ent_date"] = ViewState["ent_dt"].ToString();
                            oporow["edt_by"] = frm_uname;
                            oporow["edt_date"] = vardate;
                        }
                        else
                        {
                            oporow["ent_by"] = frm_uname;
                            oporow["ent_date"] = vardate;
                            oporow["edt_by"] = "-";
                            oporow["edt_date"] = vardate;
                        }
                        oDS.Tables[0].Rows.Add(oporow);
                        srno++;

                        if (totTDSAmt > 0)
                        {
                            to_fill_amt = 0;

                            oporow = oDS.Tables[0].NewRow();
                            oporow["BRANCHCD"] = frm_mbr;
                            oporow["TYPE"] = frm_vty;
                            oporow["vchnum"] = frm_vnum;
                            oporow["vchdate"] = txtvchdate.Text.Trim();
                            oporow["srno"] = srno;

                            oporow["oscl"] = 0;
                            oporow["quantity"] = 0;

                            oporow["FCTYPE"] = txtCurrn.Text;
                            oporow["TFCR"] = txtCurrnRate.Text;

                            oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                            oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                            oporow["ACODE"] = sg1.Rows[i].Cells[3].Text.Trim();
                            oporow["RCODE"] = tdsAccountCD;
                            if (sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "06" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "16" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "05") { }
                            else oporow["RCODE"] = largest_value_acode;

                            oporow["invno"] = ((TextBox)sg1.Rows[i].FindControl("txtInvno")).Text.ToUpper();
                            oporow["invdate"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("txtInvDt")).Text, vardate);

                            oporow["fcrate"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[7].Text.Trim()));
                            oporow["fcrate1"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[8].Text.Trim()));

                            if (largest_value_amt > 0)
                            {
                                passamt = ((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text.toDouble();
                                if (((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim().toDouble() > 0)
                                    passamt = ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim().toDouble();
                                if (passamt > largest_value_amt)
                                {
                                    largest_value_amt = passamt;
                                }
                            }
                            else
                            {
                                largest_value_amt = passamt;
                            }


                            if (((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim().toDouble() > 0)
                            {
                                to_fill_amt = ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text.Trim().toDouble();
                            }
                            else
                            {
                                to_fill_amt = ((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text.Trim().toDouble();
                            }

                            oporow["fcdramt"] = 0;
                            oporow["fccramt"] = 0;

                            to_fill_amt = totTDSAmt;

                            //to_fill_amt = 2;
                            if (frm_vty.Substring(0, 1) == "1")
                            {
                                if (to_fill_amt > 0)
                                {
                                    oporow["dramt"] = 0;
                                    oporow["Cramt"] = Math.Abs(to_fill_amt);

                                    oporow["tfcdr"] = 0;
                                    oporow["tfccr"] = Math.Abs(to_fill_amt);

                                    oporow["FCDRAMT"] = 0;
                                    oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);

                                    if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "DR")
                                    {
                                        oporow["dramt"] = Math.Abs(to_fill_amt);
                                        oporow["Cramt"] = 0;

                                        oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                        oporow["tfccr"] = 0;

                                        oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                        oporow["FCCRAMT"] = 0;
                                    }
                                }
                                else
                                {
                                    oporow["dramt"] = Math.Abs(to_fill_amt);
                                    oporow["Cramt"] = 0;

                                    oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                    oporow["tfccr"] = 0;

                                    oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                    oporow["FCCRAMT"] = 0;

                                    if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "CR")
                                    {
                                        oporow["dramt"] = 0;
                                        oporow["Cramt"] = Math.Abs(to_fill_amt);

                                        oporow["tfcdr"] = 0;
                                        oporow["tfccr"] = Math.Abs(to_fill_amt);

                                        oporow["FCDRAMT"] = 0;
                                        oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                    }
                                }
                                tot_cramt = tot_cramt + to_fill_amt;
                            }
                            else
                            {

                                if (to_fill_amt < 0)
                                {
                                    oporow["dramt"] = 0;
                                    oporow["Cramt"] = Math.Abs(to_fill_amt);

                                    oporow["tfcdr"] = 0;
                                    oporow["tfccr"] = Math.Abs(to_fill_amt);

                                    oporow["FCDRAMT"] = 0;
                                    oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);

                                    //if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "DR")
                                    //{
                                    //    oporow["dramt"] = Math.Abs(to_fill_amt);
                                    //    oporow["Cramt"] = 0;

                                    //    oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                    //    oporow["tfccr"] = 0;

                                    //    oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                    //    oporow["FCCRAMT"] = 0;
                                    //}
                                }
                                else
                                {
                                    oporow["dramt"] = Math.Abs(to_fill_amt);
                                    oporow["Cramt"] = 0;

                                    oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                    oporow["tfccr"] = 0;

                                    oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                    oporow["FCCRAMT"] = 0;

                                    if (((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value == "CR")
                                    {
                                        oporow["dramt"] = 0;
                                        oporow["Cramt"] = Math.Abs(to_fill_amt);

                                        oporow["tfcdr"] = 0;
                                        oporow["tfccr"] = Math.Abs(to_fill_amt);

                                        oporow["FCDRAMT"] = 0;
                                        oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                    }
                                }


                                tot_dramt = tot_dramt + to_fill_amt;
                            }


                            oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                            oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

                            if (((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text.Trim().Length <= 1)
                            {
                                oporow["naration"] = my_nar.Trim();
                            }
                            else
                            {
                                oporow["naration"] = ((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text.Trim();
                            }


                            oporow["tax"] = "-";
                            oporow["stax"] = 0;
                            oporow["post"] = 0;

                            oporow["grno"] = "-";
                            oporow["grdate"] = vardate;
                            oporow["mrndate"] = vardate;

                            oporow["DEPTT"] = ((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value;
                            //oporow["app_Date"] = System.DateTime.Now;

                            if (edmode.Value == "Y")
                            {
                                oporow["ent_by"] = ViewState["ent_by"].ToString();
                                oporow["ent_date"] = ViewState["ent_dt"].ToString();
                                oporow["edt_by"] = frm_uname;
                                oporow["edt_date"] = vardate;
                            }
                            else
                            {
                                oporow["ent_by"] = frm_uname;
                                oporow["ent_date"] = vardate;
                                oporow["edt_by"] = "-";
                                oporow["edt_date"] = vardate;
                            }
                            oDS.Tables[0].Rows.Add(oporow);
                            srno++;
                        }
                    }
                    #endregion
                }
            }
            //**
            i = 0;
            //bank saving
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();

            oporow["ACODE"] = tbank_code.Text.Trim();
            oporow["RCODE"] = largest_value_acode;


            if (srno > 50) srno = 1; else srno = 50;

            oporow["srno"] = srno;

            oporow["FCTYPE"] = txtCurrn.Text;
            oporow["TFCR"] = txtCurrnRate.Text;

            oporow["oscl"] = 0;

            oporow["fcrate"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[7].Text.Trim()));
            oporow["fcrate1"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[8].Text.Trim()));

            oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").ToUpper();
            oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
            oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

            oporow["quantity"] = 0;

            if (frm_vty.Substring(0, 1) == "1")
            {
                oporow["dramt"] = txttamt.Text.Trim();
                oporow["tfccr"] = 0;
                oporow["tfcdr"] = txttamt.Text.Trim();
                oporow["Cramt"] = 0;
                oporow["fcdramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
                oporow["fccramt"] = 0;
            }
            else
            {
                oporow["dramt"] = 0;
                oporow["tfccr"] = txttamt.Text.Trim();
                oporow["tfcdr"] = 0;
                oporow["Cramt"] = txttamt.Text.Trim();
                oporow["fcdramt"] = 0;
                oporow["fccramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
            }
            oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
            oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

            oporow["naration"] = (lbl1a.InnerText.Substring(0, 1) == "1" ? "Rcvd" : "Paid") + " via Chq/DD. " + txttrefnum.Text + " Dated " + txtchqdt.Text;

            oporow["tax"] = "-";
            oporow["stax"] = 0;
            oporow["post"] = 0;


            oporow["grno"] = "-";
            oporow["grdate"] = vardate;
            oporow["mrndate"] = vardate;

            //oporow["bank_Date"] = null;
            //oporow["app_Date"] = System.DateTime.Now;

            oporow["mrndate"] = System.DateTime.Now;

            if (edmode.Value == "Y")
            {
                oporow["ent_by"] = ViewState["ent_by"].ToString();
                oporow["ent_dAtE"] = ViewState["ent_dt"].ToString();
                oporow["edt_by"] = frm_uname;
                oporow["edt_dAtE"] = vardate;
            }
            else
            {
                oporow["ent_by"] = frm_uname;
                oporow["ent_date"] = vardate;
                oporow["edt_by"] = "-";
                oporow["edt_date"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);
            srno++;

            //*********************************************************************************
            //oth amt saving

            double oth_amt_val = fgen.make_double(txtothamt.Text);
            if (oth_amt_val < 0)
            {
                oth_amt_val = oth_amt_val * -1;

            }
            if (txtothac.Text.Trim().Length > 2 && oth_amt_val != 0)
            {
                //oporow = oDS.Tables[0].NewRow();
                //oporow["BRANCHCD"] = frm_mbr;
                //oporow["TYPE"] = frm_vty;
                //oporow["vchnum"] = frm_vnum;
                //oporow["vchdate"] = txtvchdate.Text.Trim();

                //oporow["ACODE"] = largest_value_acode;
                //oporow["RCODE"] = txtothac.Text.Trim();

                //oporow["srno"] = 100;

                //oporow["FCTYPE"] = 0;
                //oporow["TFCR"] = 1;
                //oporow["oscl"] = 0;

                //oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                //oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
                //oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                //oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                //oporow["quantity"] = 0;

                //if (frm_vty.Substring(0, 1) == "1")
                //{
                //    oporow["dramt"] = oth_amt_val;
                //    oporow["tfccr"] = 0;
                //    oporow["tfcdr"] = oth_amt_val;
                //    oporow["Cramt"] = 0;
                //    oporow["fcdramt"] = 0;
                //    oporow["fccramt"] = 0;
                //}
                //else
                //{
                //    oporow["dramt"] = 0;
                //    oporow["tfccr"] = oth_amt_val;
                //    oporow["tfcdr"] = 0;
                //    oporow["Cramt"] = oth_amt_val;
                //    oporow["fcdramt"] = 0;
                //    oporow["fccramt"] = 0;
                //}

                //oporow["naration"] = "-";

                //oporow["tax"] = "-";
                //oporow["stax"] = 0;
                //oporow["post"] = 0;
                //oporow["fcrate"] = 0;
                //oporow["fcrate1"] = 0;

                //oporow["grno"] = "-";
                //oporow["grdate"] = vardate;
                //oporow["mrndate"] = vardate;

                ////oporow["bank_Date"] = null;
                ////oporow["app_Date"] = System.DateTime.Now;


                //if (edmode.Value == "Y")
                //{
                //    oporow["ent_by"] = ViewState["ent_by"].ToString();
                //    oporow["ent_dAtE"] = ViewState["ent_dt"].ToString();
                //    oporow["edt_by"] = frm_uname;
                //    oporow["edt_dAtE"] = vardate;
                //}
                //else
                //{
                //    oporow["ent_by"] = frm_uname;
                //    oporow["ent_date"] = vardate;
                //    oporow["edt_by"] = "-";
                //    oporow["edt_date"] = vardate;
                //}
                //oDS.Tables[0].Rows.Add(oporow);

                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["ACODE"] = txtothac.Text.Trim();
                oporow["RCODE"] = largest_value_acode;

                oporow["srno"] = srno;

                oporow["FCTYPE"] = txtCurrn.Text;
                oporow["TFCR"] = txtCurrnRate.Text;
                oporow["oscl"] = 0;

                oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").ToUpper();
                oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
                oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                oporow["quantity"] = 0;

                if (frm_vty.Substring(0, 1) == "1")
                {
                    oporow["dramt"] = oth_amt_val;
                    oporow["tfccr"] = 0;
                    oporow["tfcdr"] = oth_amt_val;
                    oporow["Cramt"] = 0;
                    oporow["fcdramt"] = oth_amt_val;
                    oporow["fccramt"] = 0;
                }
                else
                {
                    oporow["dramt"] = 0;
                    oporow["tfccr"] = oth_amt_val;
                    oporow["tfcdr"] = 0;
                    oporow["Cramt"] = oth_amt_val;
                    oporow["fcdramt"] = 0;
                    oporow["fccramt"] = oth_amt_val;
                }
                oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

                oporow["naration"] = "-";

                oporow["tax"] = "-";
                oporow["stax"] = 0;
                oporow["post"] = 0;
                oporow["fcrate"] = 0;
                oporow["fcrate1"] = 0;

                oporow["grno"] = "-";
                oporow["grdate"] = vardate;
                oporow["mrndate"] = vardate;

                oporow["deptt"] = "OTH";

                //oporow["bank_Date"] = null;
                //oporow["app_Date"] = System.DateTime.Now;

                if (edmode.Value == "Y")
                {
                    oporow["ent_by"] = ViewState["ent_by"].ToString();
                    oporow["ent_dAtE"] = ViewState["ent_dt"].ToString();
                    oporow["edt_by"] = frm_uname;
                    oporow["edt_dAtE"] = vardate;
                }
                else
                {
                    oporow["ent_by"] = frm_uname;
                    oporow["ent_date"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["edt_date"] = vardate;
                }
                oDS.Tables[0].Rows.Add(oporow);
                srno++;
            }
        }
    }

    void save_multi_frm_vch()
    {

        string vardate;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        DataTable dtLr = ((DataTable)ViewState["sg1"]).Clone();
        double totAmtToPost = 0;
        double srno = 0;
        double passamt = 0;
        double tot_dramt = 0;
        double tot_cramt = 0;
        double largest_value_amt = 0;

        double totTDSAmt = 0;
        double to_fill_amt = 0;
        string zamt = "";
        string largest_value_acode = "";
        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            dr1 = dtLr.NewRow();
            dr1["srno"] = i + 1;
            dr1["acode"] = sg1.Rows[i].Cells[3].Text.ToString().Trim();
            dr1["ANAME"] = sg1.Rows[i].Cells[4].Text;
            dr1["invno"] = ((TextBox)sg1.Rows[i].FindControl("txtInvno")).Text;
            dr1["invdate"] = ((TextBox)sg1.Rows[i].FindControl("txtInvDt")).Text;

            dr1["camt"] = fgen.make_double(sg1.Rows[i].Cells[7].Text);
            dr1["damt"] = fgen.make_double(sg1.Rows[i].Cells[8].Text);
            dr1["net"] = fgen.make_double(sg1.Rows[i].Cells[9].Text);

            dr1["passamt"] = ((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text;
            dr1["cumbal"] = "0";
            dr1["manualamt"] = ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text;

            dr1["rmk"] = ((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text;
            dr1["duedt"] = ((TextBox)sg1.Rows[i].FindControl("txtDueDt")).Text;
            //dr1["hfM"] = dt.Rows[i]["hfM"].ToString().Trim();
            if (((CheckBox)sg1.Rows[i].FindControl("chk1")).Checked)
                dr1["hfChk"] = "Y";
            else dr1["hfChk"] = "N";

            dr1["hfdd"] = ((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value;
            dr1["hfLock"] = ((HiddenField)sg1.Rows[i].FindControl("hfLock")).Value;
            cum_bal = cum_bal + Math.Round(dr1["NET"].ToString().toDouble(), 2);
            dr1["cumbal"] = cum_bal;
            dr1["BR_ACODE"] = sg1.Rows[i].Cells[21].Text.Trim().ToUpper().Replace("&NBSP;", "");
            dtLr.Rows.Add(dr1);
        }

        int largamtPassAmt = (dtLr.Compute("max(passamt)", string.Empty)).ToString().toInt();

        int largamtManualAmt = (dtLr.Compute("max(manualamt)", string.Empty)).ToString().toInt();

        if (largamtPassAmt > largamtManualAmt)
        {
            //largest_value_acode = fgen.seek_iname_dt(dtLr, "passamt=" + largamtPassAmt + "", "ACODE");
            DataView dv = new DataView(dtLr, "", "passamt desc", DataViewRowState.CurrentRows);
            if (dv.Count > 0)
                largest_value_acode = dv[0].Row["ACODE"].ToString();
        }
        else
        {
            DataView dv = new DataView(dtLr, "", "manualamt desc", DataViewRowState.CurrentRows);
            if (dv.Count > 0)
                largest_value_acode = dv[0].Row["ACODE"].ToString();
        }

        DataView dvx = new DataView(dtLr, "", "br_acode", DataViewRowState.CurrentRows);
        DataTable distBRAcode = dvx.ToTable(true, "br_acode");

        string multi_frm_nar = "";
        multi_frm_nar = (lbl1a.InnerText.Substring(0, 1) == "1" ? "Rcpt from " : "Pymt to ") + txtaname.Text.Trim() + " Thru " + tbank_name.Text.Trim() + " Vide " + txttrefnum.Text + " Dt. " + txtchqdt.Text;
        string new_frm_vnum = "";
        new_frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' AND TYPE='33' and vchdate " + DateRange + " ", 6, "vch");
        DataTable finalDtToSave = new DataTable();
        #region   --  BRANCH SAVING
        for (int x = 0; x < distBRAcode.Rows.Count; x++)
        {
            srno = 0;
            totAmtToPost = 0;

            dvx = new DataView(dtLr, "br_acode='" + distBRAcode.Rows[x]["br_acode"].ToString().Trim() + "'", "br_acode", DataViewRowState.CurrentRows);

            new_frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + distBRAcode.Rows[x]["br_acode"].ToString().Trim() + "' AND TYPE='33' and vchdate " + DateRange + " ", 6, "vch");

            finalDtToSave = dvx.ToTable(true);
            for (i = 0; i < finalDtToSave.Rows.Count; i++)
            {
                //&& finalDtToSave.Rows[i]["BR_ACODE"].ToString().Left(2) !=frm_mbr 
                if (finalDtToSave.Rows[i]["ACODE"].ToString().Trim().Length > 3)
                {
                    to_fill_amt = 0;

                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[0];
                    oporow["TYPE"] = "33";
                    oporow["vchnum"] = new_frm_vnum;
                    oporow["vchdate"] = txtvchdate.Text.Trim();
                    oporow["srno"] = srno;

                    oporow["oscl"] = 0;
                    oporow["quantity"] = 0;

                    oporow["FCTYPE"] = txtCurrn.Text;
                    oporow["TFCR"] = txtCurrnRate.Text;

                    oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                    oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                    oporow["ACODE"] = finalDtToSave.Rows[i]["ACODE"].ToString();

                    oporow["RCODE"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "ACODE");
                    //finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[1];

                    if (sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "06" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "16" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "05")
                    {
                    }
                    else
                    {
                        oporow["RCODE"] = largest_value_acode;
                    }

                    oporow["invno"] = finalDtToSave.Rows[i]["invno"].ToString();
                    oporow["invdate"] = fgen.make_def_Date(finalDtToSave.Rows[i]["invdate"].ToString(), vardate);

                    oporow["fcrate"] = Math.Abs(fgen.make_double(finalDtToSave.Rows[i]["camt"].ToString()));
                    oporow["fcrate1"] = Math.Abs(fgen.make_double(finalDtToSave.Rows[i]["damt"].ToString()));

                    if (largest_value_amt > 0)
                    {
                        passamt = finalDtToSave.Rows[i]["passamt"].ToString().toDouble();
                        if (finalDtToSave.Rows[i]["manualamt"].ToString().toDouble() > 0)
                            passamt = finalDtToSave.Rows[i]["manualamt"].ToString().toDouble();
                        if (passamt > largest_value_amt)
                        {
                            largest_value_amt = passamt;
                        }
                    }
                    else
                    {
                        largest_value_amt = passamt;
                    }


                    if (finalDtToSave.Rows[i]["manualamt"].ToString().toDouble() > 0)
                    {
                        to_fill_amt = finalDtToSave.Rows[i]["manualamt"].ToString().toDouble();
                    }
                    else
                    {
                        to_fill_amt = finalDtToSave.Rows[i]["passamt"].ToString().toDouble();
                    }

                    totTDSAmt = 0;
                    if (frm_vty.Left(1) == "1")
                    {
                        if (finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble() > 0)
                        {
                            totTDSAmt = finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble();
                            to_fill_amt = (to_fill_amt - finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble());
                        }
                    }
                    oporow["fcdramt"] = 0;
                    oporow["fccramt"] = 0;

                    //to_fill_amt = 2;
                    if (frm_vty.Substring(0, 1) == "1")
                    {
                        if (to_fill_amt > 0)
                        {
                            oporow["dramt"] = 0;
                            oporow["Cramt"] = Math.Abs(to_fill_amt);

                            oporow["tfcdr"] = 0;
                            oporow["tfccr"] = Math.Abs(to_fill_amt);

                            oporow["FCDRAMT"] = 0;
                            oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);

                            if (finalDtToSave.Rows[i]["hfdd"].ToString() == "DR")
                            {
                                oporow["dramt"] = Math.Abs(to_fill_amt);
                                oporow["Cramt"] = 0;

                                oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                oporow["tfccr"] = 0;

                                oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                oporow["FCCRAMT"] = 0;
                            }
                        }
                        else
                        {
                            oporow["dramt"] = Math.Abs(to_fill_amt);
                            oporow["Cramt"] = 0;

                            oporow["tfcdr"] = Math.Abs(to_fill_amt);
                            oporow["tfccr"] = 0;

                            oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            oporow["FCCRAMT"] = 0;

                            if (finalDtToSave.Rows[i]["hfdd"].ToString() == "CR")
                            {
                                oporow["dramt"] = 0;
                                oporow["Cramt"] = Math.Abs(to_fill_amt);

                                oporow["tfcdr"] = 0;
                                oporow["tfccr"] = Math.Abs(to_fill_amt);

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            }
                        }
                        tot_cramt = tot_cramt + to_fill_amt;
                    }
                    else
                    {
                        if (to_fill_amt < 0)
                        {
                            oporow["dramt"] = 0;
                            oporow["Cramt"] = Math.Abs(to_fill_amt);

                            oporow["tfcdr"] = 0;
                            oporow["tfccr"] = Math.Abs(to_fill_amt);

                            oporow["FCDRAMT"] = 0;
                            oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                        }
                        else
                        {
                            oporow["dramt"] = Math.Abs(to_fill_amt);
                            oporow["Cramt"] = 0;

                            oporow["tfcdr"] = Math.Abs(to_fill_amt);
                            oporow["tfccr"] = 0;

                            oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            oporow["FCCRAMT"] = 0;

                            if (finalDtToSave.Rows[i]["hfdd"].ToString() == "CR")
                            {
                                oporow["dramt"] = 0;
                                oporow["Cramt"] = Math.Abs(to_fill_amt);

                                oporow["tfcdr"] = 0;
                                oporow["tfccr"] = Math.Abs(to_fill_amt);

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            }
                        }


                        tot_dramt = tot_dramt + to_fill_amt;
                    }

                    totAmtToPost += to_fill_amt;
                    oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                    oporow["stform"] = tslip_Name.Text.Trim().ToUpper();


                    oporow["naration"] = multi_frm_nar;


                    oporow["tax"] = "-";
                    oporow["stax"] = 0;
                    oporow["post"] = 0;

                    oporow["grno"] = "-";
                    oporow["grdate"] = vardate;
                    oporow["mrndate"] = vardate;

                    oporow["DEPTT"] = finalDtToSave.Rows[i]["hfdd"].ToString();
                    //oporow["app_Date"] = System.DateTime.Now;

                    if (edmode.Value == "Y")
                    {
                        oporow["ent_by"] = ViewState["ent_by"].ToString();
                        oporow["ent_date"] = ViewState["ent_dt"].ToString();
                        oporow["edt_by"] = frm_uname;
                        oporow["edt_date"] = vardate;
                    }
                    else
                    {
                        oporow["ent_by"] = frm_uname;
                        oporow["ent_date"] = vardate;
                        oporow["edt_by"] = "-";
                        oporow["edt_date"] = vardate;
                    }
                    oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
                    oDS.Tables[0].Rows.Add(oporow);
                    srno++;
                }

            }
            #region Bank Saving
            i = 0;
            //bank saving
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[0];
            oporow["TYPE"] = "33";
            oporow["vchnum"] = new_frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();

            oporow["ACODE"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "ACODE");
            oporow["RCODE"] = finalDtToSave.Rows[i]["ACODE"].ToString();

            if (srno > 50) srno = 1; else srno = 50;

            oporow["srno"] = srno;

            oporow["FCTYPE"] = txtCurrn.Text;
            oporow["TFCR"] = txtCurrnRate.Text;

            oporow["oscl"] = 0;

            oporow["fcrate"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[7].Text.Trim()));
            oporow["fcrate1"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[8].Text.Trim()));

            oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").ToUpper();
            oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
            oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

            oporow["quantity"] = 0;

            if (frm_vty.Substring(0, 1) == "1")
            {
                oporow["dramt"] = totAmtToPost;
                oporow["tfccr"] = 0;
                oporow["tfcdr"] = totAmtToPost;
                oporow["Cramt"] = 0;
                oporow["fcdramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
                oporow["fccramt"] = 0;
            }
            else
            {
                oporow["dramt"] = 0;
                oporow["tfccr"] = totAmtToPost;
                oporow["tfcdr"] = 0;
                oporow["Cramt"] = totAmtToPost;
                oporow["fcdramt"] = 0;
                oporow["fccramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
            }
            oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
            oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

            //if (finalDtToSave.Rows[i]["rmk"].ToString().Trim().Length <= 1)
            //{
            //    oporow["naration"] = my_nar.Trim() + " " + multi_frm_nar;
            //}
            //else
            //{
            //    oporow["naration"] = finalDtToSave.Rows[i]["rmk"].ToString() + " " + multi_frm_nar;
            //}
            oporow["naration"] = multi_frm_nar;

            oporow["tax"] = "-";
            oporow["stax"] = 0;
            oporow["post"] = 0;


            oporow["grno"] = "-";
            oporow["grdate"] = vardate;
            oporow["mrndate"] = vardate;

            //oporow["bank_Date"] = null;
            //oporow["app_Date"] = System.DateTime.Now;

            oporow["mrndate"] = System.DateTime.Now;

            if (edmode.Value == "Y")
            {
                oporow["ent_by"] = ViewState["ent_by"].ToString();
                oporow["ent_dAtE"] = ViewState["ent_dt"].ToString();
                oporow["edt_by"] = frm_uname;
                oporow["edt_dAtE"] = vardate;
            }
            else
            {
                oporow["ent_by"] = frm_uname;
                oporow["ent_date"] = vardate;
                oporow["edt_by"] = "-";
                oporow["edt_date"] = vardate;
            }
            oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
            if (totAmtToPost > 0)
            {
                oDS.Tables[0].Rows.Add(oporow);
                srno++;
            }
            //*********************************************************************************
            //oth amt saving

            double oth_amt_val = fgen.make_double(txtothamt.Text);
            if (oth_amt_val < 0)
            {
                oth_amt_val = oth_amt_val * -1;

            }
            if (txtothac.Text.Trim().Length > 2 && oth_amt_val != 0)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[0];
                oporow["TYPE"] = "33";
                oporow["vchnum"] = new_frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["ACODE"] = txtothac.Text.Trim();
                oporow["RCODE"] = largest_value_acode;

                oporow["srno"] = srno;

                oporow["FCTYPE"] = txtCurrn.Text;
                oporow["TFCR"] = txtCurrnRate.Text;
                oporow["oscl"] = 0;

                oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").ToUpper();
                oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
                oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                oporow["quantity"] = 0;

                if (frm_vty.Substring(0, 1) == "1")
                {
                    oporow["dramt"] = oth_amt_val;
                    oporow["tfccr"] = 0;
                    oporow["tfcdr"] = oth_amt_val;
                    oporow["Cramt"] = 0;
                    oporow["fcdramt"] = oth_amt_val;
                    oporow["fccramt"] = 0;
                }
                else
                {
                    oporow["dramt"] = 0;
                    oporow["tfccr"] = oth_amt_val;
                    oporow["tfcdr"] = 0;
                    oporow["Cramt"] = oth_amt_val;
                    oporow["fcdramt"] = 0;
                    oporow["fccramt"] = oth_amt_val;
                }
                oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

                //if (finalDtToSave.Rows[i]["rmk"].ToString().Trim().Length <= 1)
                //{
                //    oporow["naration"] = my_nar.Trim() + " " + multi_frm_nar;
                //}
                //else
                //{
                //    oporow["naration"] = finalDtToSave.Rows[i]["rmk"].ToString() + " " + multi_frm_nar;
                //}
                oporow["naration"] = multi_frm_nar;
                oporow["tax"] = "-";
                oporow["stax"] = 0;
                oporow["post"] = 0;
                oporow["fcrate"] = 0;
                oporow["fcrate1"] = 0;

                oporow["grno"] = "-";
                oporow["grdate"] = vardate;
                oporow["mrndate"] = vardate;

                oporow["deptt"] = "OTH";

                //oporow["bank_Date"] = null;
                //oporow["app_Date"] = System.DateTime.Now;

                if (edmode.Value == "Y")
                {
                    oporow["ent_by"] = ViewState["ent_by"].ToString();
                    oporow["ent_dAtE"] = ViewState["ent_dt"].ToString();
                    oporow["edt_by"] = frm_uname;
                    oporow["edt_dAtE"] = vardate;
                }
                else
                {
                    oporow["ent_by"] = frm_uname;
                    oporow["ent_date"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["edt_date"] = vardate;
                }
                oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
                oDS.Tables[0].Rows.Add(oporow);
                srno++;
            }
            #endregion

        }

        #endregion
        {
            totAmtToPost = 0;
            srno = 0;
            dvx = new DataView(dtLr, "", "br_acode", DataViewRowState.CurrentRows);
            distBRAcode = dvx.ToTable(true, "br_acode");
            finalDtToSave = new DataTable();
            finalDtToSave.Columns.Add("BR_ACODE");
            finalDtToSave.Columns.Add("MBR");
            finalDtToSave.Columns.Add("ACODE");
            finalDtToSave.Columns.Add("RCODE");
            finalDtToSave.Columns.Add("CAMT");
            finalDtToSave.Columns.Add("DAMT");
            finalDtToSave.Columns.Add("MANUALAMT");
            finalDtToSave.Columns.Add("PASSAMT");
            finalDtToSave.Columns.Add("AMT");
            finalDtToSave.Columns.Add("TaxDedn");
            finalDtToSave.Columns.Add("HFDD");
            DataRow finalDr = null;
            for (int x = 0; x < distBRAcode.Rows.Count; x++)
            {
                dvx = new DataView(dtLr, "br_acode='" + distBRAcode.Rows[x]["br_acode"].ToString().Trim() + "'", "br_acode", DataViewRowState.CurrentRows);
                DataTable dtTot = dvx.ToTable(true);
                double fcamt = 0, fdamt = 0, fmanualamt = 0, fpassamt = 0, famt = 0;
                string fhfdd = "";
                for (i = 0; i < dtTot.Rows.Count; i++)
                {

                    fcamt += dtTot.Rows[i]["CAMT"].ToString().toDouble();
                    fdamt += dtTot.Rows[i]["DAMT"].ToString().toDouble();
                    fmanualamt += dtTot.Rows[i]["manualamt"].ToString().toDouble();
                    fpassamt += dtTot.Rows[i]["passamt"].ToString().toDouble();
                    famt += dtTot.Rows[i]["NET"].ToString().toDouble();

                    fhfdd = dtTot.Rows[i]["HFDD"].ToString();
                }

                finalDr = finalDtToSave.NewRow();
                finalDr["MBR"] = distBRAcode.Rows[x]["br_acode"].ToString().Trim().Split('-')[0];
                finalDr["br_ACODE"] = distBRAcode.Rows[x]["br_acode"].ToString().Trim();
                finalDr["ACODE"] = distBRAcode.Rows[x]["br_acode"].ToString().Trim().Split('-')[1];
                finalDr["RCODE"] = "";
                finalDr["CAMT"] = fcamt;
                finalDr["DAMT"] = fdamt;
                finalDr["PASSAMT"] = fpassamt;
                finalDr["MANUALAMT"] = fmanualamt;
                finalDr["AMT"] = famt;
                finalDr["HFDD"] = fhfdd;
                finalDtToSave.Rows.Add(finalDr);
            }
            tot_dramt = 0;

            for (i = 0; i < finalDtToSave.Rows.Count; i++)
            {
                to_fill_amt = 0;
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = new_frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();
                oporow["srno"] = srno;

                oporow["oscl"] = 0;
                oporow["quantity"] = 0;

                oporow["FCTYPE"] = txtCurrn.Text;
                oporow["TFCR"] = txtCurrnRate.Text;

                oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");


                //if (finalDtToSave.Rows[i]["BR_ACODE"].ToString().Left(2)==frm_mbr)
                //{

                //    oporow["ACODE"] = largest_value_acode;
                //}
                //else
                //{
                oporow["ACODE"] = finalDtToSave.Rows[i]["ACODE"].ToString();
                //}

                oporow["RCODE"] = tbank_code.Text.Trim();

                oporow["invno"] = "-";
                oporow["invdate"] = vardate;

                oporow["fcrate"] = Math.Abs(fgen.make_double(finalDtToSave.Rows[i]["camt"].ToString()));
                oporow["fcrate1"] = Math.Abs(fgen.make_double(finalDtToSave.Rows[i]["damt"].ToString()));

                if (largest_value_amt > 0)
                {
                    passamt = finalDtToSave.Rows[i]["passamt"].ToString().toDouble();
                    if (finalDtToSave.Rows[i]["manualamt"].ToString().toDouble() > 0)
                        passamt = finalDtToSave.Rows[i]["manualamt"].ToString().toDouble();
                    if (passamt > largest_value_amt)
                    {
                        largest_value_amt = passamt;
                    }
                }
                else
                {
                    largest_value_amt = passamt;
                }


                if (finalDtToSave.Rows[i]["manualamt"].ToString().toDouble() > 0)
                {
                    to_fill_amt = finalDtToSave.Rows[i]["manualamt"].ToString().toDouble();
                }
                else
                {
                    to_fill_amt = finalDtToSave.Rows[i]["passamt"].ToString().toDouble();
                }

                totTDSAmt = 0;
                if (frm_vty.Left(1) == "1")
                {
                    if (finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble() > 0)
                    {
                        totTDSAmt = finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble();
                        to_fill_amt = (to_fill_amt - finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble());
                    }
                }
                oporow["fcdramt"] = 0;
                oporow["fccramt"] = 0;

                //to_fill_amt = 2;
                if (frm_vty.Substring(0, 1) == "1")
                {
                    if (to_fill_amt > 0)
                    {
                        oporow["dramt"] = 0;
                        oporow["Cramt"] = Math.Abs(to_fill_amt);

                        oporow["tfcdr"] = 0;
                        oporow["tfccr"] = Math.Abs(to_fill_amt);

                        oporow["FCDRAMT"] = 0;
                        oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);

                        if (finalDtToSave.Rows[i]["hfdd"].ToString() == "DR")
                        {
                            oporow["dramt"] = Math.Abs(to_fill_amt);
                            oporow["Cramt"] = 0;

                            oporow["tfcdr"] = Math.Abs(to_fill_amt);
                            oporow["tfccr"] = 0;

                            oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            oporow["FCCRAMT"] = 0;
                        }
                    }
                    else
                    {
                        oporow["dramt"] = Math.Abs(to_fill_amt);
                        oporow["Cramt"] = 0;

                        oporow["tfcdr"] = Math.Abs(to_fill_amt);
                        oporow["tfccr"] = 0;

                        oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                        oporow["FCCRAMT"] = 0;

                        if (finalDtToSave.Rows[i]["hfdd"].ToString() == "CR")
                        {
                            oporow["dramt"] = 0;
                            oporow["Cramt"] = Math.Abs(to_fill_amt);

                            oporow["tfcdr"] = 0;
                            oporow["tfccr"] = Math.Abs(to_fill_amt);

                            oporow["FCDRAMT"] = 0;
                            oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                        }
                    }
                    tot_cramt = tot_cramt + to_fill_amt;
                }
                else
                {
                    if (to_fill_amt < 0)
                    {
                        oporow["dramt"] = 0;
                        oporow["Cramt"] = Math.Abs(to_fill_amt);

                        oporow["tfcdr"] = 0;
                        oporow["tfccr"] = Math.Abs(to_fill_amt);

                        oporow["FCDRAMT"] = 0;
                        oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                    }
                    else
                    {
                        oporow["dramt"] = Math.Abs(to_fill_amt);
                        oporow["Cramt"] = 0;

                        oporow["tfcdr"] = Math.Abs(to_fill_amt);
                        oporow["tfccr"] = 0;

                        oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                        oporow["FCCRAMT"] = 0;

                        if (finalDtToSave.Rows[i]["hfdd"].ToString() == "CR")
                        {
                            oporow["dramt"] = 0;
                            oporow["Cramt"] = Math.Abs(to_fill_amt);

                            oporow["tfcdr"] = 0;
                            oporow["tfccr"] = Math.Abs(to_fill_amt);

                            oporow["FCDRAMT"] = 0;
                            oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                        }
                    }


                    tot_dramt = tot_dramt + to_fill_amt;
                }


                totAmtToPost += to_fill_amt;
                oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

                //if (finalDtToSave.Rows[i]["rmk"].ToString().Trim().Length <= 1)
                //{
                //    oporow["naration"] = my_nar.Trim() + " " + multi_frm_nar;
                //}
                //else
                //{
                //    oporow["naration"] = finalDtToSave.Rows[i]["rmk"].ToString() + " " + multi_frm_nar;
                //}
                oporow["naration"] = multi_frm_nar;
                oporow["tax"] = "-";
                oporow["stax"] = 0;
                oporow["post"] = 0;

                oporow["grno"] = "-";
                oporow["grdate"] = vardate;
                oporow["mrndate"] = vardate;

                oporow["DEPTT"] = finalDtToSave.Rows[i]["hfdd"].ToString();
                //oporow["app_Date"] = System.DateTime.Now;

                if (edmode.Value == "Y")
                {
                    oporow["ent_by"] = ViewState["ent_by"].ToString();
                    oporow["ent_date"] = ViewState["ent_dt"].ToString();
                    oporow["edt_by"] = frm_uname;
                    oporow["edt_date"] = vardate;
                }
                else
                {
                    oporow["ent_by"] = frm_uname;
                    oporow["ent_date"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["edt_date"] = vardate;
                }
                oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
                oDS.Tables[0].Rows.Add(oporow);
                srno++;
            }

            #region Bank Saving
            i = 0;
            //bank saving
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = new_frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();

            oporow["ACODE"] = tbank_code.Text.Trim();

            if (frm_vty.Substring(0, 1) == "1")
            {
                oporow["RCODE"] = largest_value_acode;
            }
            else
            {
                oporow["RCODE"] = txtacode.Text;
            }



            //fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "ACODE");

            if (srno > 50) srno = 1; else srno = 50;

            oporow["srno"] = srno;

            oporow["FCTYPE"] = txtCurrn.Text;
            oporow["TFCR"] = txtCurrnRate.Text;

            oporow["oscl"] = 0;

            oporow["fcrate"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[7].Text.Trim()));
            oporow["fcrate1"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[8].Text.Trim()));

            oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").ToUpper();
            oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
            oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

            oporow["quantity"] = 0;

            if (frm_vty.Substring(0, 1) == "1")
            {
                oporow["dramt"] = totAmtToPost;
                oporow["tfccr"] = 0;
                oporow["tfcdr"] = totAmtToPost;
                oporow["Cramt"] = 0;
                oporow["fcdramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
                oporow["fccramt"] = 0;
            }
            else
            {
                oporow["dramt"] = 0;
                oporow["tfccr"] = totAmtToPost;
                oporow["tfcdr"] = 0;
                oporow["Cramt"] = totAmtToPost;
                oporow["fcdramt"] = 0;
                oporow["fccramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
            }
            oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
            oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

            //if (finalDtToSave.Rows[i]["rmk"].ToString().Trim().Length <= 1)
            //{
            //    oporow["naration"] = my_nar.Trim() + " " + multi_frm_nar;
            //}
            //else
            //{
            //    oporow["naration"] = finalDtToSave.Rows[i]["rmk"].ToString() + " " + multi_frm_nar;
            //}
            oporow["naration"] = multi_frm_nar;

            oporow["tax"] = "-";
            oporow["stax"] = 0;
            oporow["post"] = 0;


            oporow["grno"] = "-";
            oporow["grdate"] = vardate;
            oporow["mrndate"] = vardate;

            //oporow["bank_Date"] = null;
            //oporow["app_Date"] = System.DateTime.Now;

            oporow["mrndate"] = System.DateTime.Now;

            if (edmode.Value == "Y")
            {
                oporow["ent_by"] = ViewState["ent_by"].ToString();
                oporow["ent_dAtE"] = ViewState["ent_dt"].ToString();
                oporow["edt_by"] = frm_uname;
                oporow["edt_dAtE"] = vardate;
            }
            else
            {
                oporow["ent_by"] = frm_uname;
                oporow["ent_date"] = vardate;
                oporow["edt_by"] = "-";
                oporow["edt_date"] = vardate;
            }
            oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
            oDS.Tables[0].Rows.Add(oporow);
            srno++;
            #endregion
        }

    }

    void save_multi_frm_vch_mpac()
    {
        //cow
        string vardate;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        DataTable dtLr = ((DataTable)ViewState["sg1"]).Clone();
        dtLr.Columns.Add("OSCL");
        double totAmtToPost = 0;
        double srno = 0;
        double passamt = 0;
        double tot_dramt = 0;
        double tot_cramt = 0;
        double largest_value_amt = 0;

        double totTDSAmt = 0;
        double to_fill_amt = 0;
        string zamt = "";
        string largest_value_acode = "";
        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            dr1 = dtLr.NewRow();
            dr1["srno"] = i + 1;
            dr1["acode"] = sg1.Rows[i].Cells[3].Text.ToString().Trim();
            dr1["ANAME"] = sg1.Rows[i].Cells[4].Text;
            dr1["invno"] = ((TextBox)sg1.Rows[i].FindControl("txtInvno")).Text;
            dr1["invdate"] = ((TextBox)sg1.Rows[i].FindControl("txtInvDt")).Text;

            dr1["camt"] = fgen.make_double(sg1.Rows[i].Cells[7].Text);
            dr1["damt"] = fgen.make_double(sg1.Rows[i].Cells[8].Text);
            dr1["net"] = fgen.make_double(sg1.Rows[i].Cells[9].Text);

            dr1["passamt"] = ((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text;
            dr1["cumbal"] = "0";
            dr1["manualamt"] = ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text;

            dr1["rmk"] = ((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text;
            dr1["duedt"] = ((TextBox)sg1.Rows[i].FindControl("txtDueDt")).Text;
            //dr1["hfM"] = dt.Rows[i]["hfM"].ToString().Trim();
            if (((CheckBox)sg1.Rows[i].FindControl("chk1")).Checked)
                dr1["hfChk"] = "Y";
            else dr1["hfChk"] = "N";

            dr1["hfdd"] = ((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value;
            dr1["hfLock"] = ((HiddenField)sg1.Rows[i].FindControl("hfLock")).Value;
            cum_bal = cum_bal + Math.Round(dr1["NET"].ToString().toDouble(), 2);
            dr1["cumbal"] = cum_bal;
            dr1["BR_ACODE"] = sg1.Rows[i].Cells[21].Text.Trim().ToUpper().Replace("&NBSP;", "");
            dr1["OSCL"] = "1";
            dtLr.Rows.Add(dr1);
        }

        int largamtPassAmt = (dtLr.Compute("max(passamt)", string.Empty)).ToString().toInt();

        int largamtManualAmt = (dtLr.Compute("max(manualamt)", string.Empty)).ToString().toInt();

        if (largamtPassAmt > largamtManualAmt)
        {
            //largest_value_acode = fgen.seek_iname_dt(dtLr, "passamt=" + largamtPassAmt + "", "ACODE");
            DataView dv = new DataView(dtLr, "", "passamt desc", DataViewRowState.CurrentRows);
            if (dv.Count > 0)
                largest_value_acode = dv[0].Row["ACODE"].ToString();
        }
        else
        {
            DataView dv = new DataView(dtLr, "", "manualamt desc", DataViewRowState.CurrentRows);
            if (dv.Count > 0)
                largest_value_acode = dv[0].Row["ACODE"].ToString();
        }

        DataView dvx = new DataView(dtLr, "", "br_acode", DataViewRowState.CurrentRows);
        DataTable distBRAcode = dvx.ToTable(true, "br_acode");

        string multi_frm_nar = "";
        multi_frm_nar = (lbl1a.InnerText.Substring(0, 1) == "1" ? "Rcpt from " : "Pymt to ") + txtaname.Text.Trim() + " Thru " + tbank_name.Text.Trim() + " Vide " + txttrefnum.Text + " Dt. " + txtchqdt.Text;
        string new_frm_vnum = "";
        new_frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' AND TYPE='33' and vchdate " + DateRange + " ", 6, "vch");
        DataTable finalDtToSave = new DataTable();
        #region   --  BRANCH SAVING
        for (int x = 0; x < distBRAcode.Rows.Count; x++)
        {
            srno = 0;
            totAmtToPost = 0;
            new_frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + distBRAcode.Rows[x]["br_acode"].ToString().Trim() + "' AND TYPE='33' and vchdate " + DateRange + " ", 6, "vch");

            dvx = new DataView(dtLr, "br_acode='" + distBRAcode.Rows[x]["br_acode"].ToString().Trim() + "'", "br_acode", DataViewRowState.CurrentRows);
            finalDtToSave = dvx.ToTable(true);
            for (i = 0; i < finalDtToSave.Rows.Count; i++)
            {
                // 
                if (finalDtToSave.Rows[i]["ACODE"].ToString().Trim().Length > 3 && finalDtToSave.Rows[i]["BR_ACODE"].ToString().Left(2) != frm_mbr)
                {
                    to_fill_amt = 0;

                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[0];
                    oporow["TYPE"] = "33";
                    oporow["vchnum"] = new_frm_vnum;
                    oporow["vchdate"] = txtvchdate.Text.Trim();
                    oporow["srno"] = srno;

                    oporow["oscl"] = finalDtToSave.Rows[i]["OSCL"].ToString().toDouble();
                    oporow["quantity"] = 0;

                    oporow["FCTYPE"] = txtCurrn.Text;
                    oporow["TFCR"] = txtCurrnRate.Text;

                    oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                    oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                    oporow["ACODE"] = finalDtToSave.Rows[i]["ACODE"].ToString();

                    oporow["RCODE"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "ACODE");
                    //finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[1];

                    if (sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "06" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "16" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "05")
                    {
                    }
                    else
                    {
                        oporow["RCODE"] = largest_value_acode;
                    }

                    oporow["invno"] = finalDtToSave.Rows[i]["invno"].ToString();
                    oporow["invdate"] = fgen.make_def_Date(finalDtToSave.Rows[i]["invdate"].ToString(), vardate);

                    oporow["fcrate"] = Math.Abs(fgen.make_double(finalDtToSave.Rows[i]["camt"].ToString()));
                    oporow["fcrate1"] = Math.Abs(fgen.make_double(finalDtToSave.Rows[i]["damt"].ToString()));

                    if (largest_value_amt > 0)
                    {
                        passamt = finalDtToSave.Rows[i]["passamt"].ToString().toDouble();
                        if (finalDtToSave.Rows[i]["manualamt"].ToString().toDouble() > 0)
                            passamt = finalDtToSave.Rows[i]["manualamt"].ToString().toDouble();
                        if (passamt > largest_value_amt)
                        {
                            largest_value_amt = passamt;
                        }
                    }
                    else
                    {
                        largest_value_amt = passamt;
                    }


                    if (finalDtToSave.Rows[i]["manualamt"].ToString().toDouble() > 0)
                    {
                        to_fill_amt = finalDtToSave.Rows[i]["manualamt"].ToString().toDouble();
                    }
                    else
                    {
                        to_fill_amt = finalDtToSave.Rows[i]["passamt"].ToString().toDouble();
                    }

                    totTDSAmt = 0;
                    if (frm_vty.Left(1) == "1")
                    {
                        if (finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble() > 0)
                        {
                            totTDSAmt = finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble();
                            to_fill_amt = (to_fill_amt - finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble());
                        }
                    }
                    oporow["fcdramt"] = 0;
                    oporow["fccramt"] = 0;

                    //to_fill_amt = 2;
                    if (frm_vty.Substring(0, 1) == "1")
                    {
                        if (to_fill_amt > 0)
                        {
                            oporow["dramt"] = 0;
                            oporow["Cramt"] = Math.Abs(to_fill_amt);

                            oporow["tfcdr"] = 0;
                            oporow["tfccr"] = Math.Abs(to_fill_amt);

                            oporow["FCDRAMT"] = 0;
                            oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);

                            if (finalDtToSave.Rows[i]["hfdd"].ToString() == "DR")
                            {
                                oporow["dramt"] = Math.Abs(to_fill_amt);
                                oporow["Cramt"] = 0;

                                oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                oporow["tfccr"] = 0;

                                oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                oporow["FCCRAMT"] = 0;
                            }
                        }
                        else
                        {
                            oporow["dramt"] = Math.Abs(to_fill_amt);
                            oporow["Cramt"] = 0;

                            oporow["tfcdr"] = Math.Abs(to_fill_amt);
                            oporow["tfccr"] = 0;

                            oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            oporow["FCCRAMT"] = 0;

                            if (finalDtToSave.Rows[i]["hfdd"].ToString() == "CR")
                            {
                                oporow["dramt"] = 0;
                                oporow["Cramt"] = Math.Abs(to_fill_amt);

                                oporow["tfcdr"] = 0;
                                oporow["tfccr"] = Math.Abs(to_fill_amt);

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            }
                        }
                        tot_cramt = tot_cramt + to_fill_amt;
                    }
                    else
                    {
                        if (to_fill_amt < 0)
                        {
                            oporow["dramt"] = 0;
                            oporow["Cramt"] = Math.Abs(to_fill_amt);

                            oporow["tfcdr"] = 0;
                            oporow["tfccr"] = Math.Abs(to_fill_amt);

                            oporow["FCDRAMT"] = 0;
                            oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                        }
                        else
                        {
                            oporow["dramt"] = Math.Abs(to_fill_amt);
                            oporow["Cramt"] = 0;

                            oporow["tfcdr"] = Math.Abs(to_fill_amt);
                            oporow["tfccr"] = 0;

                            oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            oporow["FCCRAMT"] = 0;

                            if (finalDtToSave.Rows[i]["hfdd"].ToString() == "CR")
                            {
                                oporow["dramt"] = 0;
                                oporow["Cramt"] = Math.Abs(to_fill_amt);

                                oporow["tfcdr"] = 0;
                                oporow["tfccr"] = Math.Abs(to_fill_amt);

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            }
                        }


                        tot_dramt = tot_dramt + to_fill_amt;
                    }

                    totAmtToPost += to_fill_amt;
                    oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                    oporow["stform"] = tslip_Name.Text.Trim().ToUpper();


                    oporow["naration"] = multi_frm_nar;


                    oporow["tax"] = "-";
                    oporow["stax"] = 0;
                    oporow["post"] = 0;

                    oporow["grno"] = "-";
                    oporow["grdate"] = vardate;
                    oporow["mrndate"] = vardate;

                    oporow["DEPTT"] = finalDtToSave.Rows[i]["hfdd"].ToString();
                    //oporow["app_Date"] = System.DateTime.Now;

                    if (edmode.Value == "Y")
                    {
                        oporow["ent_by"] = ViewState["ent_by"].ToString();
                        oporow["ent_date"] = ViewState["ent_dt"].ToString();
                        oporow["edt_by"] = frm_uname;
                        oporow["edt_date"] = vardate;
                    }
                    else
                    {
                        oporow["ent_by"] = frm_uname;
                        oporow["ent_date"] = vardate;
                        oporow["edt_by"] = "-";
                        oporow["edt_date"] = vardate;
                    }
                    oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
                    oDS.Tables[0].Rows.Add(oporow);
                    srno++;
                }
            }
            #region Bank Saving
            i = 0;
            //bank saving
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[0];
            oporow["TYPE"] = "33";
            oporow["vchnum"] = new_frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();

            oporow["ACODE"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "ACODE");
            oporow["RCODE"] = finalDtToSave.Rows[i]["ACODE"].ToString();

            if (srno > 50) srno = 1; else srno = 50;

            oporow["srno"] = srno;

            oporow["FCTYPE"] = txtCurrn.Text;
            oporow["TFCR"] = txtCurrnRate.Text;

            oporow["oscl"] = 0;

            oporow["fcrate"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[7].Text.Trim()));
            oporow["fcrate1"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[8].Text.Trim()));

            oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").ToUpper();
            oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
            oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

            oporow["quantity"] = 0;

            if (frm_vty.Substring(0, 1) == "1")
            {
                oporow["dramt"] = totAmtToPost;
                oporow["tfccr"] = 0;
                oporow["tfcdr"] = totAmtToPost;
                oporow["Cramt"] = 0;
                oporow["fcdramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
                oporow["fccramt"] = 0;
            }
            else
            {
                oporow["dramt"] = 0;
                oporow["tfccr"] = totAmtToPost;
                oporow["tfcdr"] = 0;
                oporow["Cramt"] = totAmtToPost;
                oporow["fcdramt"] = 0;
                oporow["fccramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
            }
            oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
            oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

            //if (finalDtToSave.Rows[i]["rmk"].ToString().Trim().Length <= 1)
            //{
            //    oporow["naration"] = my_nar.Trim() + " " + multi_frm_nar;
            //}
            //else
            //{
            //    oporow["naration"] = finalDtToSave.Rows[i]["rmk"].ToString() + " " + multi_frm_nar;
            //}
            oporow["naration"] = multi_frm_nar;

            oporow["tax"] = "-";
            oporow["stax"] = 0;
            oporow["post"] = 0;


            oporow["grno"] = "-";
            oporow["grdate"] = vardate;
            oporow["mrndate"] = vardate;

            //oporow["bank_Date"] = null;
            //oporow["app_Date"] = System.DateTime.Now;

            oporow["mrndate"] = System.DateTime.Now;

            if (edmode.Value == "Y")
            {
                oporow["ent_by"] = ViewState["ent_by"].ToString();
                oporow["ent_dAtE"] = ViewState["ent_dt"].ToString();
                oporow["edt_by"] = frm_uname;
                oporow["edt_dAtE"] = vardate;
            }
            else
            {
                oporow["ent_by"] = frm_uname;
                oporow["ent_date"] = vardate;
                oporow["edt_by"] = "-";
                oporow["edt_date"] = vardate;
            }
            oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
            if (totAmtToPost > 0)
            {
                oDS.Tables[0].Rows.Add(oporow);
                srno++;
            }
            //*********************************************************************************
            //oth amt saving

            double oth_amt_val = fgen.make_double(txtothamt.Text);
            if (oth_amt_val < 0)
            {
                oth_amt_val = oth_amt_val * -1;

            }
            if (txtothac.Text.Trim().Length > 2 && oth_amt_val != 0)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[0];
                oporow["TYPE"] = "33";
                oporow["vchnum"] = new_frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["ACODE"] = txtothac.Text.Trim();
                oporow["RCODE"] = largest_value_acode;

                oporow["srno"] = srno;

                oporow["FCTYPE"] = txtCurrn.Text;
                oporow["TFCR"] = txtCurrnRate.Text;
                oporow["oscl"] = 0;

                oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").ToUpper();
                oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
                oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                oporow["quantity"] = 0;

                if (frm_vty.Substring(0, 1) == "1")
                {
                    oporow["dramt"] = oth_amt_val;
                    oporow["tfccr"] = 0;
                    oporow["tfcdr"] = oth_amt_val;
                    oporow["Cramt"] = 0;
                    oporow["fcdramt"] = oth_amt_val;
                    oporow["fccramt"] = 0;
                }
                else
                {
                    oporow["dramt"] = 0;
                    oporow["tfccr"] = oth_amt_val;
                    oporow["tfcdr"] = 0;
                    oporow["Cramt"] = oth_amt_val;
                    oporow["fcdramt"] = 0;
                    oporow["fccramt"] = oth_amt_val;
                }
                oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

                //if (finalDtToSave.Rows[i]["rmk"].ToString().Trim().Length <= 1)
                //{
                //    oporow["naration"] = my_nar.Trim() + " " + multi_frm_nar;
                //}
                //else
                //{
                //    oporow["naration"] = finalDtToSave.Rows[i]["rmk"].ToString() + " " + multi_frm_nar;
                //}
                oporow["naration"] = multi_frm_nar;
                oporow["tax"] = "-";
                oporow["stax"] = 0;
                oporow["post"] = 0;
                oporow["fcrate"] = 0;
                oporow["fcrate1"] = 0;

                oporow["grno"] = "-";
                oporow["grdate"] = vardate;
                oporow["mrndate"] = vardate;

                oporow["deptt"] = "OTH";

                //oporow["bank_Date"] = null;
                //oporow["app_Date"] = System.DateTime.Now;

                if (edmode.Value == "Y")
                {
                    oporow["ent_by"] = ViewState["ent_by"].ToString();
                    oporow["ent_dAtE"] = ViewState["ent_dt"].ToString();
                    oporow["edt_by"] = frm_uname;
                    oporow["edt_dAtE"] = vardate;
                }
                else
                {
                    oporow["ent_by"] = frm_uname;
                    oporow["ent_date"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["edt_date"] = vardate;
                }
                oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
                oDS.Tables[0].Rows.Add(oporow);
                srno++;
            }
            #endregion
        }

        #endregion

        #region   --  MPAC extra saving
        for (int x = 0; x < distBRAcode.Rows.Count; x++)
        {
            totAmtToPost = 0;

            dvx = new DataView(dtLr, "br_acode='" + distBRAcode.Rows[x]["br_acode"].ToString().Trim() + "'", "br_acode", DataViewRowState.CurrentRows);
            finalDtToSave = dvx.ToTable(true);
            for (i = 0; i < finalDtToSave.Rows.Count; i++)
            {
                // 
                if (finalDtToSave.Rows[i]["ACODE"].ToString().Trim().Length > 3 && finalDtToSave.Rows[i]["BR_ACODE"].ToString().Left(2) == frm_mbr)
                {
                    to_fill_amt = 0;

                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[0];
                    oporow["TYPE"] = frm_vty;
                    oporow["vchnum"] = frm_vnum;
                    oporow["vchdate"] = txtvchdate.Text.Trim();
                    oporow["srno"] = srno;

                    oporow["oscl"] = finalDtToSave.Rows[i]["OSCL"].ToString().toDouble();
                    oporow["quantity"] = 0;

                    oporow["FCTYPE"] = txtCurrn.Text;
                    oporow["TFCR"] = txtCurrnRate.Text;

                    oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                    oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                    oporow["ACODE"] = finalDtToSave.Rows[i]["ACODE"].ToString();

                    oporow["RCODE"] = tbank_code.Text.Trim();
                    //fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "ACODE");
                    //finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[1];

                    if (sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "06" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "16" || sg1.Rows[i].Cells[3].Text.Trim().Left(2) == "05")
                    {
                    }
                    else
                    {
                        oporow["RCODE"] = largest_value_acode;
                    }

                    oporow["invno"] = finalDtToSave.Rows[i]["invno"].ToString();
                    oporow["invdate"] = fgen.make_def_Date(finalDtToSave.Rows[i]["invdate"].ToString(), vardate);

                    oporow["fcrate"] = Math.Abs(fgen.make_double(finalDtToSave.Rows[i]["camt"].ToString()));
                    oporow["fcrate1"] = Math.Abs(fgen.make_double(finalDtToSave.Rows[i]["damt"].ToString()));

                    if (largest_value_amt > 0)
                    {
                        passamt = finalDtToSave.Rows[i]["passamt"].ToString().toDouble();
                        if (finalDtToSave.Rows[i]["manualamt"].ToString().toDouble() > 0)
                            passamt = finalDtToSave.Rows[i]["manualamt"].ToString().toDouble();
                        if (passamt > largest_value_amt)
                        {
                            largest_value_amt = passamt;
                        }
                    }
                    else
                    {
                        largest_value_amt = passamt;
                    }


                    if (finalDtToSave.Rows[i]["manualamt"].ToString().toDouble() > 0)
                    {
                        to_fill_amt = finalDtToSave.Rows[i]["manualamt"].ToString().toDouble();
                    }
                    else
                    {
                        to_fill_amt = finalDtToSave.Rows[i]["passamt"].ToString().toDouble();
                    }

                    totTDSAmt = 0;
                    if (frm_vty.Left(1) == "1")
                    {
                        if (finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble() > 0)
                        {
                            totTDSAmt = finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble();
                            to_fill_amt = (to_fill_amt - finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble());
                        }
                    }
                    oporow["fcdramt"] = 0;
                    oporow["fccramt"] = 0;

                    //to_fill_amt = 2;
                    if (frm_vty.Substring(0, 1) == "1")
                    {
                        if (to_fill_amt > 0)
                        {
                            oporow["dramt"] = 0;
                            oporow["Cramt"] = Math.Abs(to_fill_amt);

                            oporow["tfcdr"] = 0;
                            oporow["tfccr"] = Math.Abs(to_fill_amt);

                            oporow["FCDRAMT"] = 0;
                            oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);

                            if (finalDtToSave.Rows[i]["hfdd"].ToString() == "DR")
                            {
                                oporow["dramt"] = Math.Abs(to_fill_amt);
                                oporow["Cramt"] = 0;

                                oporow["tfcdr"] = Math.Abs(to_fill_amt);
                                oporow["tfccr"] = 0;

                                oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                                oporow["FCCRAMT"] = 0;
                            }
                        }
                        else
                        {
                            oporow["dramt"] = Math.Abs(to_fill_amt);
                            oporow["Cramt"] = 0;

                            oporow["tfcdr"] = Math.Abs(to_fill_amt);
                            oporow["tfccr"] = 0;

                            oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            oporow["FCCRAMT"] = 0;

                            if (finalDtToSave.Rows[i]["hfdd"].ToString() == "CR")
                            {
                                oporow["dramt"] = 0;
                                oporow["Cramt"] = Math.Abs(to_fill_amt);

                                oporow["tfcdr"] = 0;
                                oporow["tfccr"] = Math.Abs(to_fill_amt);

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            }
                        }
                        tot_cramt = tot_cramt + to_fill_amt;
                    }
                    else
                    {
                        if (to_fill_amt < 0)
                        {
                            oporow["dramt"] = 0;
                            oporow["Cramt"] = Math.Abs(to_fill_amt);

                            oporow["tfcdr"] = 0;
                            oporow["tfccr"] = Math.Abs(to_fill_amt);

                            oporow["FCDRAMT"] = 0;
                            oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                        }
                        else
                        {
                            oporow["dramt"] = Math.Abs(to_fill_amt);
                            oporow["Cramt"] = 0;

                            oporow["tfcdr"] = Math.Abs(to_fill_amt);
                            oporow["tfccr"] = 0;

                            oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            oporow["FCCRAMT"] = 0;

                            if (finalDtToSave.Rows[i]["hfdd"].ToString() == "CR")
                            {
                                oporow["dramt"] = 0;
                                oporow["Cramt"] = Math.Abs(to_fill_amt);

                                oporow["tfcdr"] = 0;
                                oporow["tfccr"] = Math.Abs(to_fill_amt);

                                oporow["FCDRAMT"] = 0;
                                oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            }
                        }


                        tot_dramt = tot_dramt + to_fill_amt;
                    }

                    totAmtToPost += to_fill_amt;
                    oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                    oporow["stform"] = tslip_Name.Text.Trim().ToUpper();


                    oporow["naration"] = multi_frm_nar;


                    oporow["tax"] = "-";
                    oporow["stax"] = 0;
                    oporow["post"] = 0;

                    oporow["grno"] = "-";
                    oporow["grdate"] = vardate;
                    oporow["mrndate"] = vardate;

                    oporow["DEPTT"] = finalDtToSave.Rows[i]["hfdd"].ToString();
                    //oporow["app_Date"] = System.DateTime.Now;

                    if (edmode.Value == "Y")
                    {
                        oporow["ent_by"] = ViewState["ent_by"].ToString();
                        oporow["ent_date"] = ViewState["ent_dt"].ToString();
                        oporow["edt_by"] = frm_uname;
                        oporow["edt_date"] = vardate;
                    }
                    else
                    {
                        oporow["ent_by"] = frm_uname;
                        oporow["ent_date"] = vardate;
                        oporow["edt_by"] = "-";
                        oporow["edt_date"] = vardate;
                    }
                    oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
                    oDS.Tables[0].Rows.Add(oporow);
                    srno++;
                }
            }
            #region Bank Saving
            i = 0;
            //bank saving
            if (finalDtToSave.Rows[i]["BR_ACODE"].ToString().Left(2) == frm_mbr)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[0];
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["ACODE"] = tbank_code.Text.Trim();
                //fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "ACODE");
                oporow["RCODE"] = finalDtToSave.Rows[i]["ACODE"].ToString();

                //if (srno > 50) srno = 1; else srno = 50;
                srno++;

                oporow["srno"] = srno;

                oporow["FCTYPE"] = txtCurrn.Text;
                oporow["TFCR"] = txtCurrnRate.Text;

                oporow["oscl"] = 0;

                oporow["fcrate"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[7].Text.Trim()));
                oporow["fcrate1"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[8].Text.Trim()));

                oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").ToUpper();
                oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
                oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                oporow["quantity"] = 0;

                totAmtToPost = txttamt.Text.toDouble();

                if (frm_vty.Substring(0, 1) == "1")
                {
                    oporow["dramt"] = totAmtToPost;
                    oporow["tfccr"] = 0;
                    oporow["tfcdr"] = totAmtToPost;
                    oporow["Cramt"] = 0;
                    oporow["fcdramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
                    oporow["fccramt"] = 0;
                }
                else
                {
                    oporow["dramt"] = 0;
                    oporow["tfccr"] = totAmtToPost;
                    oporow["tfcdr"] = 0;
                    oporow["Cramt"] = totAmtToPost;
                    oporow["fcdramt"] = 0;
                    oporow["fccramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
                }
                oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

                //if (finalDtToSave.Rows[i]["rmk"].ToString().Trim().Length <= 1)
                //{
                //    oporow["naration"] = my_nar.Trim() + " " + multi_frm_nar;
                //}
                //else
                //{
                //    oporow["naration"] = finalDtToSave.Rows[i]["rmk"].ToString() + " " + multi_frm_nar;
                //}
                oporow["naration"] = multi_frm_nar;

                oporow["tax"] = "-";
                oporow["stax"] = 0;
                oporow["post"] = 0;


                oporow["grno"] = "-";
                oporow["grdate"] = vardate;
                oporow["mrndate"] = vardate;

                //oporow["bank_Date"] = null;
                //oporow["app_Date"] = System.DateTime.Now;

                oporow["mrndate"] = System.DateTime.Now;

                if (edmode.Value == "Y")
                {
                    oporow["ent_by"] = ViewState["ent_by"].ToString();
                    oporow["ent_dAtE"] = ViewState["ent_dt"].ToString();
                    oporow["edt_by"] = frm_uname;
                    oporow["edt_dAtE"] = vardate;
                }
                else
                {
                    oporow["ent_by"] = frm_uname;
                    oporow["ent_date"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["edt_date"] = vardate;
                }
                oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
                if (totAmtToPost > 0)
                {
                    oDS.Tables[0].Rows.Add(oporow);
                    srno++;
                }
            }

            // making extra bank row in mpac style
            if (finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[0] != frm_mbr)
            {
                DataView dvEx = new DataView(oDS.Tables[0], "TYPE='33' AND BRANCHCD<>'" + frm_mbr + "' AND DRAMT>0", "", DataViewRowState.CurrentRows);
                if (dvEx.Count > 0)
                {
                    for (int g = 0; g < 1; g++)
                    {
                        oporow = oDS.Tables[0].NewRow();
                        oporow["BRANCHCD"] = frm_mbr;
                        oporow["TYPE"] = frm_vty;
                        oporow["vchnum"] = frm_vnum;
                        oporow["vchdate"] = txtvchdate.Text.Trim();

                        oporow["ACODE"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[0] + "'", "ACODE");
                        oporow["RCODE"] = finalDtToSave.Rows[i]["ACODE"].ToString();

                        //if (srno > 50) srno = 1; else srno = 50;
                        srno++;

                        oporow["srno"] = srno;

                        oporow["FCTYPE"] = txtCurrn.Text;
                        oporow["TFCR"] = txtCurrnRate.Text;

                        oporow["oscl"] = 0;

                        oporow["fcrate"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[7].Text.Trim()));
                        oporow["fcrate1"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[8].Text.Trim()));

                        oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").ToUpper();
                        oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
                        oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                        oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                        oporow["quantity"] = 0;


                        DataTable dtxxx = new DataTable();
                        dtxxx = dvEx.ToTable();

                        totAmtToPost = dtxxx.Compute("sum(dramt)", "").ToString().toDouble();

                        if (frm_vty.Substring(0, 1) == "2")
                        {
                            oporow["dramt"] = totAmtToPost;
                            oporow["tfccr"] = 0;
                            oporow["tfcdr"] = totAmtToPost;
                            oporow["Cramt"] = 0;
                            oporow["fcdramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
                            oporow["fccramt"] = 0;
                        }
                        else
                        {
                            oporow["dramt"] = 0;
                            oporow["tfccr"] = totAmtToPost;
                            oporow["tfcdr"] = 0;
                            oporow["Cramt"] = totAmtToPost;
                            oporow["fcdramt"] = 0;
                            oporow["fccramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
                        }
                        oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                        oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

                        //if (finalDtToSave.Rows[i]["rmk"].ToString().Trim().Length <= 1)
                        //{
                        //    oporow["naration"] = my_nar.Trim() + " " + multi_frm_nar;
                        //}
                        //else
                        //{
                        //    oporow["naration"] = finalDtToSave.Rows[i]["rmk"].ToString() + " " + multi_frm_nar;
                        //}
                        oporow["naration"] = multi_frm_nar;

                        oporow["tax"] = "-";
                        oporow["stax"] = 0;
                        oporow["post"] = 0;


                        oporow["grno"] = "-";
                        oporow["grdate"] = vardate;
                        oporow["mrndate"] = vardate;

                        //oporow["bank_Date"] = null;
                        //oporow["app_Date"] = System.DateTime.Now;

                        oporow["mrndate"] = System.DateTime.Now;

                        if (edmode.Value == "Y")
                        {
                            oporow["ent_by"] = ViewState["ent_by"].ToString();
                            oporow["ent_dAtE"] = ViewState["ent_dt"].ToString();
                            oporow["edt_by"] = frm_uname;
                            oporow["edt_dAtE"] = vardate;
                        }
                        else
                        {
                            oporow["ent_by"] = frm_uname;
                            oporow["ent_date"] = vardate;
                            oporow["edt_by"] = "-";
                            oporow["edt_date"] = vardate;
                        }
                        oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
                        if (totAmtToPost > 0)
                        {
                            oDS.Tables[0].Rows.Add(oporow);
                            srno++;
                        }
                    }
                }
            }
            //*********************************************************************************
            //oth amt saving

            double oth_amt_val = fgen.make_double(txtothamt.Text);
            if (oth_amt_val < 0)
            {
                oth_amt_val = oth_amt_val * -1;

            }
            if (txtothac.Text.Trim().Length > 2 && oth_amt_val != 0)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = finalDtToSave.Rows[i]["BR_ACODE"].ToString().Split('-')[0];
                oporow["TYPE"] = "33";
                oporow["vchnum"] = new_frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["ACODE"] = txtothac.Text.Trim();
                oporow["RCODE"] = largest_value_acode;

                oporow["srno"] = srno;

                oporow["FCTYPE"] = txtCurrn.Text;
                oporow["TFCR"] = txtCurrnRate.Text;
                oporow["oscl"] = 0;

                oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").ToUpper();
                oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
                oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

                oporow["quantity"] = 0;

                if (frm_vty.Substring(0, 1) == "1")
                {
                    oporow["dramt"] = oth_amt_val;
                    oporow["tfccr"] = 0;
                    oporow["tfcdr"] = oth_amt_val;
                    oporow["Cramt"] = 0;
                    oporow["fcdramt"] = oth_amt_val;
                    oporow["fccramt"] = 0;
                }
                else
                {
                    oporow["dramt"] = 0;
                    oporow["tfccr"] = oth_amt_val;
                    oporow["tfcdr"] = 0;
                    oporow["Cramt"] = oth_amt_val;
                    oporow["fcdramt"] = 0;
                    oporow["fccramt"] = oth_amt_val;
                }
                oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

                //if (finalDtToSave.Rows[i]["rmk"].ToString().Trim().Length <= 1)
                //{
                //    oporow["naration"] = my_nar.Trim() + " " + multi_frm_nar;
                //}
                //else
                //{
                //    oporow["naration"] = finalDtToSave.Rows[i]["rmk"].ToString() + " " + multi_frm_nar;
                //}
                oporow["naration"] = multi_frm_nar;
                oporow["tax"] = "-";
                oporow["stax"] = 0;
                oporow["post"] = 0;
                oporow["fcrate"] = 0;
                oporow["fcrate1"] = 0;

                oporow["grno"] = "-";
                oporow["grdate"] = vardate;
                oporow["mrndate"] = vardate;

                oporow["deptt"] = "OTH";

                //oporow["bank_Date"] = null;
                //oporow["app_Date"] = System.DateTime.Now;

                if (edmode.Value == "Y")
                {
                    oporow["ent_by"] = ViewState["ent_by"].ToString();
                    oporow["ent_dAtE"] = ViewState["ent_dt"].ToString();
                    oporow["edt_by"] = frm_uname;
                    oporow["edt_dAtE"] = vardate;
                }
                else
                {
                    oporow["ent_by"] = frm_uname;
                    oporow["ent_date"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["edt_date"] = vardate;
                }
                oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
                oDS.Tables[0].Rows.Add(oporow);
                srno++;
            }
            #endregion
        }

        #endregion
        if (1 == 2)
        {
            #region extra
            totAmtToPost = 0;
            srno = 0;
            dvx = new DataView(dtLr, "", "br_acode", DataViewRowState.CurrentRows);
            distBRAcode = dvx.ToTable(true, "br_acode");
            finalDtToSave = new DataTable();
            finalDtToSave.Columns.Add("BR_ACODE");
            finalDtToSave.Columns.Add("MBR");
            finalDtToSave.Columns.Add("ACODE");
            finalDtToSave.Columns.Add("RCODE");
            finalDtToSave.Columns.Add("CAMT");
            finalDtToSave.Columns.Add("DAMT");
            finalDtToSave.Columns.Add("MANUALAMT");
            finalDtToSave.Columns.Add("PASSAMT");
            finalDtToSave.Columns.Add("AMT");
            finalDtToSave.Columns.Add("TaxDedn");
            finalDtToSave.Columns.Add("HFDD");
            DataRow finalDr = null;
            for (int x = 0; x < distBRAcode.Rows.Count; x++)
            {
                dvx = new DataView(dtLr, "br_acode='" + distBRAcode.Rows[x]["br_acode"].ToString().Trim() + "'", "br_acode", DataViewRowState.CurrentRows);
                DataTable dtTot = dvx.ToTable(true);
                double fcamt = 0, fdamt = 0, fmanualamt = 0, fpassamt = 0, famt = 0;
                string fhfdd = "";
                for (i = 0; i < dtTot.Rows.Count; i++)
                {

                    fcamt += dtTot.Rows[i]["CAMT"].ToString().toDouble();
                    fdamt += dtTot.Rows[i]["DAMT"].ToString().toDouble();
                    fmanualamt += dtTot.Rows[i]["manualamt"].ToString().toDouble();
                    fpassamt += dtTot.Rows[i]["passamt"].ToString().toDouble();
                    famt += dtTot.Rows[i]["NET"].ToString().toDouble();

                    fhfdd = dtTot.Rows[i]["HFDD"].ToString();
                }

                finalDr = finalDtToSave.NewRow();
                finalDr["MBR"] = distBRAcode.Rows[x]["br_acode"].ToString().Trim().Split('-')[0];
                finalDr["br_ACODE"] = distBRAcode.Rows[x]["br_acode"].ToString().Trim();
                finalDr["ACODE"] = distBRAcode.Rows[x]["br_acode"].ToString().Trim().Split('-')[1];
                finalDr["RCODE"] = "";
                finalDr["CAMT"] = fcamt;
                finalDr["DAMT"] = fdamt;
                finalDr["PASSAMT"] = fpassamt;
                finalDr["MANUALAMT"] = fmanualamt;
                finalDr["AMT"] = famt;
                finalDr["HFDD"] = fhfdd;
                finalDtToSave.Rows.Add(finalDr);
            }
            tot_dramt = 0;

            for (i = 0; i < finalDtToSave.Rows.Count; i++)
            {
                to_fill_amt = 0;
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = new_frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();
                oporow["srno"] = srno;

                oporow["oscl"] = finalDtToSave.Rows[i]["OSCL"].ToString().toDouble();
                oporow["quantity"] = 0;

                oporow["FCTYPE"] = txtCurrn.Text;
                oporow["TFCR"] = txtCurrnRate.Text;

                oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");


                //if (finalDtToSave.Rows[i]["BR_ACODE"].ToString().Left(2)==frm_mbr)
                //{

                //    oporow["ACODE"] = largest_value_acode;
                //}
                //else
                //{
                oporow["ACODE"] = finalDtToSave.Rows[i]["ACODE"].ToString();
                //}

                oporow["RCODE"] = tbank_code.Text.Trim();

                oporow["invno"] = "-";
                oporow["invdate"] = vardate;

                oporow["fcrate"] = Math.Abs(fgen.make_double(finalDtToSave.Rows[i]["camt"].ToString()));
                oporow["fcrate1"] = Math.Abs(fgen.make_double(finalDtToSave.Rows[i]["damt"].ToString()));

                if (largest_value_amt > 0)
                {
                    passamt = finalDtToSave.Rows[i]["passamt"].ToString().toDouble();
                    if (finalDtToSave.Rows[i]["manualamt"].ToString().toDouble() > 0)
                        passamt = finalDtToSave.Rows[i]["manualamt"].ToString().toDouble();
                    if (passamt > largest_value_amt)
                    {
                        largest_value_amt = passamt;
                    }
                }
                else
                {
                    largest_value_amt = passamt;
                }


                if (finalDtToSave.Rows[i]["manualamt"].ToString().toDouble() > 0)
                {
                    to_fill_amt = finalDtToSave.Rows[i]["manualamt"].ToString().toDouble();
                }
                else
                {
                    to_fill_amt = finalDtToSave.Rows[i]["passamt"].ToString().toDouble();
                }

                totTDSAmt = 0;
                if (frm_vty.Left(1) == "1")
                {
                    if (finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble() > 0)
                    {
                        totTDSAmt = finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble();
                        to_fill_amt = (to_fill_amt - finalDtToSave.Rows[i]["TaxDedn"].ToString().toDouble());
                    }
                }
                oporow["fcdramt"] = 0;
                oporow["fccramt"] = 0;

                //to_fill_amt = 2;
                if (frm_vty.Substring(0, 1) == "1")
                {
                    if (to_fill_amt > 0)
                    {
                        oporow["dramt"] = 0;
                        oporow["Cramt"] = Math.Abs(to_fill_amt);

                        oporow["tfcdr"] = 0;
                        oporow["tfccr"] = Math.Abs(to_fill_amt);

                        oporow["FCDRAMT"] = 0;
                        oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);

                        if (finalDtToSave.Rows[i]["hfdd"].ToString() == "DR")
                        {
                            oporow["dramt"] = Math.Abs(to_fill_amt);
                            oporow["Cramt"] = 0;

                            oporow["tfcdr"] = Math.Abs(to_fill_amt);
                            oporow["tfccr"] = 0;

                            oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                            oporow["FCCRAMT"] = 0;
                        }
                    }
                    else
                    {
                        oporow["dramt"] = Math.Abs(to_fill_amt);
                        oporow["Cramt"] = 0;

                        oporow["tfcdr"] = Math.Abs(to_fill_amt);
                        oporow["tfccr"] = 0;

                        oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                        oporow["FCCRAMT"] = 0;

                        if (finalDtToSave.Rows[i]["hfdd"].ToString() == "CR")
                        {
                            oporow["dramt"] = 0;
                            oporow["Cramt"] = Math.Abs(to_fill_amt);

                            oporow["tfcdr"] = 0;
                            oporow["tfccr"] = Math.Abs(to_fill_amt);

                            oporow["FCDRAMT"] = 0;
                            oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                        }
                    }
                    tot_cramt = tot_cramt + to_fill_amt;
                }
                else
                {
                    if (to_fill_amt < 0)
                    {
                        oporow["dramt"] = 0;
                        oporow["Cramt"] = Math.Abs(to_fill_amt);

                        oporow["tfcdr"] = 0;
                        oporow["tfccr"] = Math.Abs(to_fill_amt);

                        oporow["FCDRAMT"] = 0;
                        oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                    }
                    else
                    {
                        oporow["dramt"] = Math.Abs(to_fill_amt);
                        oporow["Cramt"] = 0;

                        oporow["tfcdr"] = Math.Abs(to_fill_amt);
                        oporow["tfccr"] = 0;

                        oporow["FCDRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                        oporow["FCCRAMT"] = 0;

                        if (finalDtToSave.Rows[i]["hfdd"].ToString() == "CR")
                        {
                            oporow["dramt"] = 0;
                            oporow["Cramt"] = Math.Abs(to_fill_amt);

                            oporow["tfcdr"] = 0;
                            oporow["tfccr"] = Math.Abs(to_fill_amt);

                            oporow["FCDRAMT"] = 0;
                            oporow["FCCRAMT"] = Math.Round(Math.Abs(to_fill_amt) / txtCurrnRate.Text.toDouble(), 2);
                        }
                    }


                    tot_dramt = tot_dramt + to_fill_amt;
                }


                totAmtToPost += to_fill_amt;
                oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
                oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

                //if (finalDtToSave.Rows[i]["rmk"].ToString().Trim().Length <= 1)
                //{
                //    oporow["naration"] = my_nar.Trim() + " " + multi_frm_nar;
                //}
                //else
                //{
                //    oporow["naration"] = finalDtToSave.Rows[i]["rmk"].ToString() + " " + multi_frm_nar;
                //}
                oporow["naration"] = multi_frm_nar;
                oporow["tax"] = "-";
                oporow["stax"] = 0;
                oporow["post"] = 0;

                oporow["grno"] = "-";
                oporow["grdate"] = vardate;
                oporow["mrndate"] = vardate;

                oporow["DEPTT"] = finalDtToSave.Rows[i]["hfdd"].ToString();
                //oporow["app_Date"] = System.DateTime.Now;

                if (edmode.Value == "Y")
                {
                    oporow["ent_by"] = ViewState["ent_by"].ToString();
                    oporow["ent_date"] = ViewState["ent_dt"].ToString();
                    oporow["edt_by"] = frm_uname;
                    oporow["edt_date"] = vardate;
                }
                else
                {
                    oporow["ent_by"] = frm_uname;
                    oporow["ent_date"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["edt_date"] = vardate;
                }
                oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
                oDS.Tables[0].Rows.Add(oporow);
                srno++;
            }

            #region Bank Saving
            i = 0;
            //bank saving
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = new_frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();

            oporow["ACODE"] = tbank_code.Text.Trim();

            if (frm_vty.Substring(0, 1) == "1")
            {
                oporow["RCODE"] = largest_value_acode;
            }
            else
            {
                oporow["RCODE"] = txtacode.Text;
            }



            //fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "ACODE");

            if (srno > 50) srno = 1; else srno = 50;

            oporow["srno"] = srno;

            oporow["FCTYPE"] = txtCurrn.Text;
            oporow["TFCR"] = txtCurrnRate.Text;

            oporow["oscl"] = 0;

            oporow["fcrate"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[7].Text.Trim()));
            oporow["fcrate1"] = Math.Abs(fgen.make_double(sg1.Rows[i].Cells[8].Text.Trim()));

            oporow["invno"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").ToUpper();
            oporow["invdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");
            oporow["refnum"] = txttrefnum.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
            oporow["refdate"] = Convert.ToDateTime(txtchqdt.Text.Trim()).ToString("dd/MM/yyyy");

            oporow["quantity"] = 0;

            if (frm_vty.Substring(0, 1) == "1")
            {
                oporow["dramt"] = totAmtToPost;
                oporow["tfccr"] = 0;
                oporow["tfcdr"] = totAmtToPost;
                oporow["Cramt"] = 0;
                oporow["fcdramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
                oporow["fccramt"] = 0;
            }
            else
            {
                oporow["dramt"] = 0;
                oporow["tfccr"] = totAmtToPost;
                oporow["tfcdr"] = 0;
                oporow["Cramt"] = totAmtToPost;
                oporow["fcdramt"] = 0;
                oporow["fccramt"] = Math.Round(txttamt.Text.Trim().toDouble() / txtCurrnRate.Text.toDouble(), 2);
            }
            oporow["ref1"] = tslip_no.Text.Trim().ToUpper();
            oporow["stform"] = tslip_Name.Text.Trim().ToUpper();

            //if (finalDtToSave.Rows[i]["rmk"].ToString().Trim().Length <= 1)
            //{
            //    oporow["naration"] = my_nar.Trim() + " " + multi_frm_nar;
            //}
            //else
            //{
            //    oporow["naration"] = finalDtToSave.Rows[i]["rmk"].ToString() + " " + multi_frm_nar;
            //}
            oporow["naration"] = multi_frm_nar;

            oporow["tax"] = "-";
            oporow["stax"] = 0;
            oporow["post"] = 0;


            oporow["grno"] = "-";
            oporow["grdate"] = vardate;
            oporow["mrndate"] = vardate;

            //oporow["bank_Date"] = null;
            //oporow["app_Date"] = System.DateTime.Now;

            oporow["mrndate"] = System.DateTime.Now;

            if (edmode.Value == "Y")
            {
                oporow["ent_by"] = ViewState["ent_by"].ToString();
                oporow["ent_dAtE"] = ViewState["ent_dt"].ToString();
                oporow["edt_by"] = frm_uname;
                oporow["edt_dAtE"] = vardate;
            }
            else
            {
                oporow["ent_by"] = frm_uname;
                oporow["ent_date"] = vardate;
                oporow["edt_by"] = "-";
                oporow["edt_date"] = vardate;
            }
            oporow["ST_ENTFORM"] = frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim();
            oDS.Tables[0].Rows.Add(oporow);
            srno++;
            #endregion

            #endregion
        }

    }

    void save_fun3()
    {
        string vardate;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg2.Rows.Count - 0; i++)
        {
            if (((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim().Length > 1)
            {
                oporow3 = oDS3.Tables[0].NewRow();
                oporow3["BRANCHCD"] = frm_mbr;

                oporow3["TYPE"] = frm_vty;
                oporow3["vchnum"] = frm_vnum;
                oporow3["vchdate"] = txtvchdate.Text.Trim();
                oporow3["SRNO"] = i;

                if (edmode.Value == "Y")
                {
                    oporow3["ent_by"] = ViewState["ent_by"].ToString();
                    oporow3["ent_dt"] = ViewState["ent_dt"].ToString();
                    oporow3["edt_by"] = frm_uname;
                    oporow3["edt_dt"] = vardate;
                }
                else
                {
                    oporow3["ent_by"] = frm_uname;
                    oporow3["ent_dt"] = vardate;
                    oporow3["edt_by"] = "-";
                    oporow3["edt_dt"] = vardate;
                }


                oporow3["AMTZ_ppcode"] = txtacode.Text.Trim().Replace("'", "");
                oporow3["AMTZ_xpcode"] = txt_expcode.Text.Trim();
                oporow3["AMTZ_DATE"] = fgen.make_def_Date(((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim(), txtvchdate.Text.Trim());
                oporow3["AMTZ_amt"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim());
                oDS3.Tables[0].Rows.Add(oporow3);
            }
        }
    }

    //-------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        //m1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT opt_Start FROM FIN_RSYS_OPT_PW WHERE branchcd='" + frm_mbr + "' and UPPER(TRIM(OPT_ID))='W2001' ", "OPT_START");
        if (fgen.getOption(frm_qstr, frm_cocd, "W0100", "OPT_ENABLE") == "Y")
            //m1 = fgen.getOption(frm_qstr, frm_cocd, "W0161", "OPT_PARAM");
            m1 = fgen.getOption(frm_qstr, frm_cocd, "W2001", "OPT_PARAM");
        else
            m1 = fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='R01' and enable_yn='Y' ", "params");
        if (m1 != "0")
        {
            eff_Dt = " a.vchdate>= to_date('" + m1.Trim() + "','dd/mm/yyyy') ";
            fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view recdata as(select branchcd,TRIM(ACODE) AS ACODE,TRIM(INVNO) AS INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT a.branchcd,a.ACODE,a.INVNO,a.INVDATE ,nvl(a.DRAMT,0) AS DRAMT,nvl(a.CRAMT,0) AS CRAMT ,(nvl(a.dramt,0))-(nvl(a.cramt,0)) as net FROM VOUCHER a,famst b WHERE trim(a.acode)=trim(b.acode) and a.BRANCHCD!='88' AND a.BRANCHCD!='DD' AND " + eff_Dt + "  and  SUBSTR(b.grp,1,2)IN('02','05','06','16')  UNION ALL SELECT a.branchcd,a.ACODE,a.INVNO,a.INVDATE ,nvl(a.DRAMT,0) AS DRAMT,nvl(a.CRAMT,0) AS CRAMT ,nvl(a.dramt,0)-nvl(a.cramt,0) as net FROM RECEBAL a,famst b WHERE a.branchcd!='DD' and trim(a.acode)=trim(B.acode) and SUBSTR(b.grp,1,2)IN('02','05','06','16') ) c  GROUP BY branchcd,TRIM(ACODE),TRIM(INVNO),INVDATE HAVING SUM(dRAMT)-SUM(CRAMT)<>0)  ORDER BY branchcd,ACODE,INVDATE,INVNO ");
        }
        else
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Please Set Starting Date W2001 !!");
            return;
        }
        hffield.Value = "TACODE";
        make_qry_4_popup();

        if (hf3.Value == "Y")
        {
            fgen.Fn_ValueBoxFinance("Voucher Information", frm_qstr, "800px", "500px");
        }
        else
        {
            if (frm_vty.Substring(0, 1) == "1") fgen.Fn_open_mseek("Select Party", frm_qstr);
            else fgen.Fn_open_sseek("Select Party", frm_qstr);
        }
    }

    //-------------------------------------------------------
    protected void btnexp_Click(object sender, ImageClickEventArgs e)
    {
        // for edit button popup                
        hffield.Value = "EXPCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Expense Code", frm_qstr);
    }
    protected void btnemp_Click(object sender, ImageClickEventArgs e)
    {
        // for edit button popup                
        hffield.Value = "EMPCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Other Link Code", frm_qstr);
    }

    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        // for edit button popup                
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Other Link Code", frm_qstr);
    }
    protected void btnAtch_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Atch_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr);
    }

    //-------------------------------------------------------
    protected void sg1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.TabIndex = -1;
            //e.Row.Attributes["onclick"] = string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            //e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event); ";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }
    protected void btnGridPop_Click(object sender, EventArgs e)
    {
        if (hf1.Value == "CHQMSG")
        {
            fgen.msg("-", "AMSG", "Please Fill Cheque Number and Date before Proceeding!!");
            txttrefnum.Focus();
            return;
        }

        if (hf1.Value == "CHQAMSG" && lbl1a.InnerText.Substring(0, 1) != "2")
        {
            fgen.msg("-", "AMSG", "Please Fill Cheque Amount before Proceeding!!");
            txttamt.Focus();
            return;
        }

        if (hf1.Value == "BALEXCEED")
        {
            hffield.Value = "BALEXCEED";
            fgen.msg("-", "CMSG", "The Value of Selected Bills Exceeds The Chq Amount'13'Do You want to Auto Adjust against This Bill!!");
        }
        if (hf1.Value.Contains("txtmanualfor"))
        {
            hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_txtmanualfor_", "");
            insertRow();
        }
        if (hf1.Value.Contains("txtrmk"))
        {
            hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_txtrmk_", "");
            insertRow();
        }
    }
    void insertRow()
    {
        #region Remove Row from GridView
        {
            dt = new DataTable();
            DataTable sg1_dt = new DataTable();
            dt = (DataTable)ViewState["sg1"];
            DataRow sg1_dr = null;
            create_tab();
            sg1_dt = dt1;
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                if (i == hf1.Value.ToString().toDouble() + 1)
                {
                    add_blankrows();
                }

                sg1_dr = sg1_dt.NewRow();
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    sg1_dr[c] = dt.Rows[i][c];
                }
                sg1_dt.Rows.Add(sg1_dr);
            }
            ViewState["sg1"] = sg1_dt;
            add_blankrows();
            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            for (i = 0; i < sg1.Rows.Count; i++)
            {
                sg1.Rows[i].Cells[13].Text = (i + 1).ToString();
            }
        }
        #endregion
        setColHeadings();
    }

    string acBal(string selecAcode)
    {
        string xprd1 = "between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt1 + "','dd/mm/yyyy') -1";
        string SQueryx = "select sum(opb)+sum(inbal)-sum(outbal) as bal from (select sum(yr_" + frm_myear + ") as opb,0 as inbal,0 as outbal from famstbal where branchcd IN ('" + frm_mbr + "') and acode  in ('" + selecAcode + "') group by acode union all select sum(nvl(DRAMT,0))-sum(nvl(CRAMT,0)) as obal,0 as inbal,0 as outbal from voucher where branchcd IN ('" + frm_mbr + "') and VCHDATE " + xprd1 + " and acode  in ('" + selecAcode + "') union all select 0 as opbal,(case when sum(A.DRAMT)-sum(A.CRAMT)>0 then ABS(sum(A.DRAMT)-sum(A.CRAMT)) else 0 end) AS IQTYIN,(case when sum(A.DRAMT)-sum(A.CRAMT)>0 then 0 else abs(sum(A.DRAMT)-sum(A.CRAMT)) end) AS IQTYOUT from voucher A where a.branchcd IN ('" + frm_mbr + "') and A.VCHDATE " + DateRange + " AND A.ACODE  IN ('" + selecAcode + "') )";
        return fgen.seek_iname(frm_qstr, frm_cocd, SQueryx, "BAL");
    }
    protected void btnForxRate_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnRmk_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "RMK";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Narration", frm_qstr);
    }
    protected void sg1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        z = dt.Rows.Count - 1;
        dr1 = null;
        cum_bal = 0;
        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            //dr1["srno"] = dt1.Rows.Count + 1;
            dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
            dr1["ANAME"] = dt.Rows[i]["ANAME"].ToString().Trim();
            dr1["invno"] = ((TextBox)sg1.Rows[i].FindControl("txtInvno")).Text;
            dr1["invdate"] = ((TextBox)sg1.Rows[i].FindControl("txtInvDt")).Text;
            dr1["camt"] = dt.Rows[i]["camt"].ToString().Trim();
            dr1["damt"] = dt.Rows[i]["damt"].ToString().Trim();
            dr1["net"] = dt.Rows[i]["net"].ToString().Trim();
            dr1["passamt"] = ((TextBox)sg1.Rows[i].FindControl("txtpassfor")).Text;
            dr1["cumbal"] = "0";
            dr1["manualamt"] = ((TextBox)sg1.Rows[i].FindControl("txtmanualfor")).Text;
            dr1["rmk"] = ((TextBox)sg1.Rows[i].FindControl("txtrmk")).Text;
            dr1["duedt"] = ((TextBox)sg1.Rows[i].FindControl("txtDueDt")).Text;
            dr1["hfM"] = dt.Rows[i]["hfM"].ToString().Trim();
            if (((CheckBox)sg1.Rows[i].FindControl("chk1")).Checked)
                dr1["hfChk"] = "Y";
            else dr1["hfChk"] = "N";

            dr1["hfdd"] = ((HtmlSelect)sg1.Rows[i].FindControl("dd2")).Value;
            dr1["hfLock"] = ((HiddenField)sg1.Rows[i].FindControl("hfLock")).Value;
            cum_bal = cum_bal + Math.Round(dr1["NET"].ToString().toDouble(), 2);
            dr1["cumbal"] = cum_bal;
        }

        sg1.PageIndex = e.NewPageIndex;
        DataTable dt1 = new DataTable();
        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        dt1 = dt;
        ViewState["sg1"] = dt1;
        if (dt1 != null)
        {
            sg1.DataSource = dt1;
            sg1.DataBind(); dt1.Dispose();
        }
        else
        {
            sg1.DataSource = null;
            sg1.DataBind();
        }

        TabName.Value = "DescTab";
        setColHeadings();

        setDropDown();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        //if (lbl1a.InnerText.Substring(0, 1) == "1")
        //{
        //    SQuery = "select b.aname,nvl(b.pay_num,0) as pay_num,trim(upper(nvl(a.invno,'-'))) as invno,a.invdate,to_char(SUM(a.dramt),'999999999.99') as dramt,to_char(SUM(a.cramt),'999999999.99') as cramt,to_char(SUM(a.dramt) -SUM(a.cramt),'999999999.99') as NET ,trim(a.ACODE) as acode,'-' as rmk from recdata a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and trim(a.acode) " + (txtacode.Text.Contains("'") ? " in (" + txtacode.Text.Trim() + ")" : "='" + txtacode.Text.Trim().Replace("'", "") + "'") + " GROUP BY b.aname,nvl(b.pay_num,0),trim(upper(nvl(a.invno,'-'))),a.INVDATE,trim(a.ACODE) having SUM(a.dramt) -SUM(a.cramt)<>0 order by a.INVDATE,trim(upper(nvl(a.invno,'-')))";
        //}
        //else
        //{
        //    SQuery = "select b.aname,nvl(b.pay_num,0) as pay_num,trim(upper(nvl(a.invno,'-'))) as invno,a.invdate,to_char(SUM(a.cramt),'999999999.99') as dramt,to_char(SUM(a.dramt),'999999999.99') as cramt ,to_Char(SUM(a.cramt) -SUM(a.dramt),'999999999.99') as NET ,trim(a.ACODE) as acode,'-' as rmk from recdata a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and trim(a.acode) " + (txtacode.Text.Contains("'") ? " in (" + txtacode.Text.Trim() + ")" : "='" + txtacode.Text.Trim().Replace("'", "") + "'") + " GROUP BY b.aname,nvl(b.pay_num,0),trim(upper(nvl(a.invno,'-'))),a.INVDATE,trim(a.ACODE) having SUM(a.dramt) -SUM(a.cramt)<>0 order by a.INVDATE,trim(upper(nvl(a.invno,'-')))";
        //}

        //dt = new DataTable();
        //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        //if (dt.Rows.Count > 0)
        //{
        //    fgen.exp_to_excel(dt, "excel", "xls", "FileToImport");
        //}

        dt = new DataTable();
        dt = (DataTable)ViewState["sg1"];
        fgen.exp_to_excel(dt, "excel", "xls", "FileToImport");
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        string ext = "", filesavepath = "", excelConString = "";
        DataTable dtn = new DataTable();
        string filename = "";
        //if (txtacode.Value.Trim().Length > 2)
        {
            if (xmlUpload.HasFile)
            {
                ext = Path.GetExtension(xmlUpload.FileName).ToLower();
                if (ext == ".xls")
                {
                    filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xls";
                    xmlUpload.SaveAs(filesavepath);
                    excelConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
                }
                else if (ext == ".csv")
                {
                    filename = "" + DateTime.Now.ToString("ddMMyyhhmmfff");
                    filesavepath = AppDomain.CurrentDomain.BaseDirectory + "tej-base\\Upload\\file" + filename + ".csv";
                    xmlUpload.SaveAs(filesavepath);
                    excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "tej-base\\Upload\\" + ";Extended Properties=\"Text;HDR=Yes;FMT=Delimited\"";
                }
                else if (ext == ".xlsx")
                {
                    filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xlsx";
                    xmlUpload.SaveAs(filesavepath);
                    excelConString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
                }
                else
                {
                    fgen.msg("-", "AMSG", "Please Select Excel File only in xls format!!");
                    return;
                }


                dtn = new DataTable();
                if (ext == ".csv")
                {
                    var allValues = File.ReadAllText(filesavepath).Split('\n');
                    int x = 0, colN = 0;
                    dt = new DataTable();
                    DataRow myRow = null;
                    foreach (string singleRow in allValues)
                    {
                        if (singleRow != "")
                        {
                            var allCols = singleRow.Split(',');
                            colN = 0;
                            if (x != 0) myRow = dt.NewRow();
                            foreach (string cols in allCols)
                            {
                                if (x == 0)
                                {
                                    dt.Columns.Add(cols);
                                }
                                else
                                {
                                    try
                                    {
                                        myRow[colN] = cols;
                                    }
                                    catch { }
                                    colN++;
                                }
                            }
                            if (x != 0) dt.Rows.Add(myRow);
                            x++;
                        }
                    }
                    dtn = dt;
                }
                else
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
                    if (ext == ".csv")
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

                    dtn = dt;
                }

                if (dtn.Rows.Count > 0)
                {
                    if (lbl1a.InnerText.Substring(0, 1) == "1")
                    {
                        SQuery = "select b.aname,nvl(b.pay_num,0) as pay_num,trim(upper(nvl(a.invno,'-'))) as invno,a.invdate,to_char(SUM(a.dramt),'999999999.99') as dramt,to_char(SUM(a.cramt),'999999999.99') as cramt,to_char(SUM(a.dramt) -SUM(a.cramt),'999999999.99') as NET ,trim(a.ACODE) as acode,'-' as rmk from recdata a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and trim(a.acode) " + (txtacode.Text.Contains("'") ? " in (" + txtacode.Text.Trim() + ")" : "='" + txtacode.Text.Trim().Replace("'", "") + "'") + " GROUP BY b.aname,nvl(b.pay_num,0),trim(upper(nvl(a.invno,'-'))),a.INVDATE,trim(a.ACODE) having SUM(a.dramt) -SUM(a.cramt)<>0 order by a.INVDATE,trim(upper(nvl(a.invno,'-')))";
                    }
                    else
                    {
                        SQuery = "select b.aname,nvl(b.pay_num,0) as pay_num,trim(upper(nvl(a.invno,'-'))) as invno,a.invdate,to_char(SUM(a.cramt),'999999999.99') as dramt,to_char(SUM(a.dramt),'999999999.99') as cramt ,to_Char(SUM(a.cramt) -SUM(a.dramt),'999999999.99') as NET ,trim(a.ACODE) as acode,'-' as rmk from recdata a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and trim(a.acode) " + (txtacode.Text.Contains("'") ? " in (" + txtacode.Text.Trim() + ")" : "='" + txtacode.Text.Trim().Replace("'", "") + "'") + " GROUP BY b.aname,nvl(b.pay_num,0),trim(upper(nvl(a.invno,'-'))),a.INVDATE,trim(a.ACODE) having SUM(a.dramt) -SUM(a.cramt)<>0 order by a.INVDATE,trim(upper(nvl(a.invno,'-')))";
                    }
                    DataTable dtBills = new DataTable();
                    dtBills = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    create_tab();
                    string mhd = "";
                    for (int x = 0; x < dtn.Rows.Count; x++)
                    {
                        mhd = fgen.seek_iname_dt(dtBills, "ACODE='" + dtn.Rows[x][1].ToString().Trim() + "' AND INVNO='" + dtn.Rows[x][3].ToString().Trim() + "' AND INVDATE='" + dtn.Rows[x][4].ToString().Trim() + "' ", "ACODE");

                        {
                            dr1 = dt1.NewRow();
                            dr1["srno"] = x + 1;
                            dr1["acode"] = dtn.Rows[x][1].ToString().Trim();
                            dr1["ANAME"] = fgen.seek_iname_dt(dtBills, "ACODE='" + dtn.Rows[x][1].ToString().Trim() + "'", "aname");
                            if (mhd != "0")
                            {
                                dr1["invno"] = dtn.Rows[x][3].ToString().Trim();
                                dr1["invdate"] = Convert.ToDateTime(fgen.make_def_Date(dtn.Rows[x][4].ToString().Trim(), txtvchdate.Text)).ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                dr1["invno"] = "N/F";
                                dr1["invdate"] = Convert.ToDateTime(fgen.make_def_Date(txtvchdate.Text, txtvchdate.Text)).ToString("yyyy-MM-dd");
                            }
                            if (mhd != "0")
                                dr1["damt"] = dtn.Rows[x][7].ToString().Trim();
                            dr1["camt"] = 0;
                            if (mhd != "0")
                                dr1["net"] = dtn.Rows[x][7].ToString().Trim();
                            dr1["passamt"] = "0";
                            dr1["cumbal"] = "0";
                            dr1["manualamt"] = "0";
                            if (mhd == "0")
                            {
                                dr1["rmk"] = "Bill : " + dtn.Rows[x][3].ToString().Trim() + " (Amt : " + dtn.Rows[x][7].ToString().Trim() + ")" + " : " + dtn.Rows[x][4].ToString().Trim();
                            }
                            dr1["hfM"] = "Y";
                            cum_bal = cum_bal + Math.Round(dr1["NET"].ToString().toDouble(), 2);
                            dr1["cumbal"] = cum_bal;
                            dr1["hfChk"] = "Y";
                            if (lbl1a.InnerText.Substring(0, 1) == "1")
                                dr1["hfdd"] = "CR";
                            else dr1["hfdd"] = "DR";

                            //dr1["duedt"] = Convert.ToDateTime(dtn.Rows[x][7].ToString().Trim()).AddDays(dtBills.Rows[x]["pay_num"].ToString().toDouble()).ToString("dd/MM/yyyy");

                            if (hf3.Value == "Y")
                            {
                                // filling old rate of Fx 
                                mq0 = "Select nvl(tfccr,0)-nvl(tfcdr,0)||'~'||tfcr as val from voucher where branchcd!='DD' and (type like '5%' or (type in ('31','32'))) and trim(Acode)='" + txtacode.Text + "' and upper(Trim(invno))='" + dr1["invno"].ToString().Trim() + "' and to_chaR(invdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr1["invdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "'";
                                col3 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "val");
                                if (col3 == "0" || (col3.Split('~')[0].toDouble() == 0 && col3.Split('~')[1].toDouble() == 0))
                                {
                                    mq0 = "Select tfccr||'~'||tfcr as val from voucher where branchcd!='DD' and type like '2%' and trim(Acode)='" + txtacode.Text + "' and upper(Trim(invno))='" + dr1["invno"].ToString().Trim() + "' and to_chaR(invdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr1["invdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "'";
                                    col3 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "val");
                                    if (col3 == "0" || (col3.Split('~')[0].toDouble() == 0 && col3.Split('~')[1].toDouble() == 0))
                                    {
                                        mq0 = "Select tfccr||'~'||tfcr as val from voucher where branchcd!='DD' and trim(Acode)='" + txtacode.Text + "' and upper(Trim(invno))='" + dr1["invno"].ToString().Trim() + "' and to_chaR(invdate,'dd/mm/yyyy')='" + Convert.ToDateTime(dr1["invdate"].ToString().Trim()).ToString("dd/MM/yyyy") + "'";
                                        col3 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "val");
                                    }
                                }
                                if (col3.Contains("~"))
                                {
                                    dr1["orig_fx_bal"] = col3.Split('~')[1];
                                    dr1["curr_fx_bal"] = 0;
                                    dr1["orig_fx_amt"] = col3.Split('~')[0];
                                }
                            }

                            dt1.Rows.Add(dr1);
                        }
                    }

                    add_blankrows();

                    ViewState["sg1"] = dt1;
                    sg1.DataSource = dt1;
                    sg1.DataBind();

                    setDropDown();
                }
            }
        }
    }
}