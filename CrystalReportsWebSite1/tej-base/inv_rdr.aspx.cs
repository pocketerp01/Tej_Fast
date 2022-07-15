using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.HtmlControls;

using System.IO;
using System.Drawing;

public partial class inv_rdr : System.Web.UI.Page
{
    IFormatProvider AustralianDateFormat;
    string btnval, SQuery, col1, col2, col3, col4, vchnum, vardate, fromdt, todt, DateRange, year, mhd, merr = "0", cond, value1;
    DataRow oporow; DataTable dt, dt1, dt2, dt3, dt4, dt5;
    fgenDB fgen = new fgenDB();
    string frm_mbr, frm_vty, frm_qstr, frm_vnum, frm_url, frm_cocd, frm_uname, frm_tabname, frm_myear, frm_sql, frm_ulvl, frm_formID, frm_UserID;
    string mq0 = "", mq1 = "", mq2 = "", mq3, frm_tab_ivch, frm_tab_reel, Prg_Id, PrdRange, datefrmt, chk_rights;
    DataSet oDS;
    int j = 0;
    string Checked_ok;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            txtinv.Focus();
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
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    AustralianDateFormat = System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat;
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl(); //btnnew.Focus(); set_Val(); btnnew_ServerClick(sender, e);
                txtinv.ReadOnly = false;
                txtinv.Focus();
            }
            set_Val();
        }
    }
    public void enablectrl()
    {
        btnnew.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false;
        btncan.Visible = false; btnext.Visible = true; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
    }
    public void disablectrl()
    {
        btnnew.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = true;
        btncan.Visible = true; btnext.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
    }
    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
    }
    public void check_fields()
    {
        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "BUDGMST", "ENT_BY");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE BUDGMST ADD ENT_BY VARCHAR2(30) DEFAULT '-'");

        mhd = fgen.check_filed_name(frm_qstr, frm_cocd, "BUDGMST", "ENT_DT");
        if (mhd == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE BUDGMST ADD ENT_DT DATE DEFAULT SYSDATE");

    }
    public void set_Val()
    {
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        frm_vty = "ZG";
        //HCID = Request.Cookies["rid"].Value.ToString();
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "32011":
            case "F20125":
                frm_vty = "ZG";
                frm_tab_ivch = "IVOUCHERP";
                lblheader.Text = "Gate Out Entry"; trlbl.Visible = false; reelloc.Visible = false;
                tdbarcode.InnerText = "Invoice Bar Code"; tdreelname.Visible = false; tdreeltxt.Visible = false;
                tr1.Visible = true;
                if (frm_cocd == "BUPL" || frm_cocd == "WPPL" || frm_cocd == "GTCF" || frm_cocd == "GIRP" || frm_cocd == "KPPL" || frm_cocd == "KESH" || frm_cocd == "MEGH" || frm_cocd == "UKB" || frm_cocd == "MINV" || frm_cocd == "MIRP")
                {
                    mq2 = "N";
                }
                else mq2 = "Y";

                mq1 = "to_char(invdate,'yyyymmdd')";
                datefrmt = "to_char(vchdate,'yyyymmdd')";
                break;
            case "22055":
            case "AK17":
                frm_tab_ivch = "IVOUCHER";
                frm_tab_reel = "reelvch";
                frm_vty = "31";
                lblheader.Text = "Reel Issue";
                tr1.Visible = true; btndel.Visible = false; btnlist.Visible = false; reelloc.Visible = false;
                if (frm_cocd == "TGIP") { tdbarcode.InnerText = "Job Card"; tdreelname.Visible = true; tdreeltxt.Visible = true; trlbl.Visible = true; }
                else { tdbarcode.InnerText = "Bar Code Value"; tdreelname.Visible = false; tdreeltxt.Visible = false; trlbl.Visible = false; }
                break;
            case "22095":
            case "F25125":
                frm_tab_ivch = "SCRATCH";
                frm_vty = "RL";
                lblheader.Text = "Physical Verification of Product";
                tr1.Visible = true; btndel.Visible = false; btnlist.Visible = true; reelloc.Visible = false;
                tdbarcode.InnerText = "Barcode"; tdreelname.Visible = false; tdreeltxt.Visible = false; trlbl.Visible = false;
                break;

            case "F15125":
                frm_vty = "76";
                frm_tab_ivch = "budgmst";
                lblheader.Text = "Kanban Entry"; trlbl.Visible = false; reelloc.Visible = false;
                tdbarcode.InnerText = "Item Bar Code"; tdreelname.Visible = false; tdreeltxt.Visible = false;
                tr1.Visible = true;
                break;

            case "F25124":
                frm_tab_ivch = "SCRATCH"; txtvchnum.Visible = false; txtvchdate.Visible = false; tdentryno.Visible = false; tdentrydt.Visible = false;
                lblheader.Text = "Stacking Entry"; trlbl.Visible = false; reelloc.Visible = true;
                tdbarcode.InnerText = "Reel BarCode"; tdreelname.Visible = false; tdreeltxt.Visible = false; btndel.Visible = false;
                //tr1.Visible = true; 
                break;
            case "F50125":
                frm_tab_ivch = "IVOUCHERW";
                frm_vty = "PL";
                lblheader.Text = "Pick List";
                tr1.Visible = true; btndel.Visible = false; btnlist.Visible = true; reelloc.Visible = false;
                tdbarcode.InnerText = "Barcode"; tdreelname.Visible = false; tdreeltxt.Visible = false; trlbl.Visible = false;
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TAB_IVCH", frm_tab_ivch);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TAB_REEL", frm_tab_reel);
    }
    public void disp_data()
    {
        btnval = hffield.Value;
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tab_ivch = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_IVCH");
        frm_tab_reel = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_REEL");
        switch (btnval)
        {
            case "":
                SQuery = "";
                break;
            default:
                if (btnval == "Edit" || btnval == "Del" || btnval == "Print")
                    if (frm_formID == "F15125")
                    {
                        SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,b.aname as party_name from " + frm_tab_ivch + " a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type ='" + frm_vty + "' and a.vchdate " + DateRange + " order by a.vchnum desc";
                    }
                    else
                        SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as ge_no,to_char(a.vchdate,'dd/mm/yyyy') as ge_date,b.aname as party_name,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt from ivoucherp a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type= 'ZG' and a.vchdate " + DateRange + " order by a.vchnum desc";
                break;
        }
        if (SQuery.Length > 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl(); txtinv.ReadOnly = false;
        set_Val(); check_fields();
        hffield.Value = "New";
        if (chk_rights == "Y")
        {
            //HCID = Request.Cookies["rid"].Value.ToString();
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            switch (Prg_Id)
            {
                case "22055":
                case "AK17":
                    txtvchnum.Text = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from ivoucher where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                    break;
                case "22095":
                case "F25125":
                case "F15125":
                case "F50125":
                case "32011":
                case "F20125":
                    txtvchnum.Text = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from " + frm_tab_ivch + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                    break;
                case "F25124":
                    dt = new DataTable();
                    SQuery = "SELECT NAME AS FSTR FROM TYPEGRP WHERE ID='BN'";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ddreelloc.DataSource = dt;
                        ddreelloc.DataTextField = "fstr";
                        ddreelloc.DataValueField = "fstr";
                        ddreelloc.DataBind();
                    }
                    break;
            }
            txtvchdate.Text = vardate;
            fgen.EnableForm(this.Controls); disablectrl(); txtinv.Focus();
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
        string chk_freeze = "";
        chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1064", txtvchdate.Text.Trim());
        if (chk_freeze == "1")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Rolling Freeze Date !!");
            return;
        }
        if (chk_freeze == "2")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Fixed Freeze Date !!");
            return;
        }

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "32011":
            case "F20125":
                if (frm_cocd == "MINV*" || frm_cocd == "GTCF" || frm_cocd == "GIRP" || frm_cocd == "KTPL" || frm_cocd == "KPPL" || frm_cocd == "KESH" || frm_cocd == "MEGH" || frm_cocd == "UKB" || frm_cocd == "MINV" || frm_cocd == "MIRP")
                {
                    if (sg1.Rows.Count > 0) fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Dear " + frm_uname + ", Selected invoice is already dispatched!!')", true);
                        //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Selected invoice is already dispatched!!");
                        txtinv.Text = ""; txtinv.Focus();
                    }
                }
                else
                {
                    for (int i = 0; i < sg1.Rows.Count; i++)
                    {
                        value1 = sg1.Rows[i].Cells[1].Text.Trim();
                        if (frm_cocd == "DLJM" || frm_cocd == "MEGA" || frm_cocd == "KUNS" || frm_cocd == "WPPL" || frm_cocd == "GTCF" || frm_cocd == "GIRP" || frm_cocd == "KPPL" || frm_cocd == "KESH" || frm_cocd == "MEGH" || frm_cocd == "UKB" || frm_cocd == "MINV" || frm_cocd == "MIRP") cond = "trim(stage)||TRIM(IOPR)||trim(invno)||to_char(invdate,'dd/mm/yyyy')='" + value1 + "'";
                        else cond = "trim(stage)||TRIM(IOPR)||trim(invno)||to_char(invdate,'yyyymmdd')='" + value1 + "'";

                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select distinct trim(stage)||TRIM(IOPR)||trim(invno)||to_char(invdate,'yyyymmdd') as fstr from ivoucherp where type='ZG' and vchdate " + DateRange + " and " + cond + "", "fstr");
                        if (mhd == "0")
                        {
                            if (frm_cocd == "DLJM" || frm_cocd == "MEGA" || frm_cocd == "KUNS" || frm_cocd == "WPPL" || frm_cocd == "GTCF" || frm_cocd == "KPPL" || frm_cocd == "KESH" || frm_cocd == "MEGH" || frm_cocd == "UKB") cond = "trim(branchcd)||TRIM(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + value1 + "'";
                            else cond = "trim(branchcd)||TRIM(type)||trim(vchnum)||to_char(vchdate,'yyyymmdd')='" + value1 + "'";
                            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "Select distinct trim(branchcd)||TRIM(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr from ivoucher where " + cond + "", "fstr");
                            if (mhd != "0") fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Dear " + frm_uname + ", Selected Invoice Information is not Correct!!')", true);
                                //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Selected Invoice Information is not Correct!!");
                                txtinv.Text = ""; txtinv.Focus();
                                break;
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Dear " + frm_uname + ", Selected invoice is already dispatched!!')", true);
                            //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Selected invoice is already dispatched!!");
                            txtinv.Text = ""; txtinv.Focus();
                            break;
                        }
                    }
                }
                break;
            case "22055":
            case "AK17":
                if (frm_cocd == "TGIP")
                {
                    if (txtinv.Text.Trim().Length <= 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Please Select Job Card First!!')", true);
                        //fgen.msg("-", "AMSG", "Please Select Job Card First!!");
                        txtinv.Focus(); return;
                    }
                    if (sg1.Rows.Count > 0)
                    {
                        int chk = 0;
                        foreach (GridViewRow gr in sg1.Rows)
                        {
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, "select kclreelno,sum(reelwin)-sum(reelwout) as tot from (select distinct kclreelno,sum(reelwin) as reelwin,0 as reelwout from reelvch where type in ('02','11') and branchcd='" + frm_mbr + "' group by kclreelno union all select distinct kclreelno,0 as reelwin,sum(reelwout) as reelwout from reelvch where type in ('31','32') and branchcd='" + frm_mbr + "' group by kclreelno) where trim(kclreelno)='" + gr.Cells[1].Text.Trim() + "'  group by kclreelno");
                            if (dt.Rows.Count > 0) chk = 0;
                            else { chk = 1; break; }
                            dt.Dispose();
                        }
                        if (chk == 0) fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
                        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Selected Reel Not Found!!");
                    }
                    else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", No Reel Not Found!!");
                }
                else
                {
                    if (sg1.Rows.Count > 1)
                    {
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "select kclreelno,sum(reelwin)-sum(reelwout) as tot from (select distinct kclreelno,sum(reelwin) as reelwin,0 as reelwout from reelvch where type in ('02','11') and branchcd='" + frm_mbr + "' group by kclreelno union all select distinct kclreelno,0 as reelwin,sum(reelwout) as reelwout from reelvch where type in ('31','32') and branchcd='" + frm_mbr + "' group by kclreelno) where trim(kclreelno)='" + txtreel.Text.Trim() + "'  group by kclreelno");
                        if (dt.Rows.Count > 0)
                        {
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, "select A.ciname as iname,A.icode,b.REELWIN,b.psize,b.gsm from item A,REELVCH B where TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' and trim(b.kclreelno)='" + txtreel.Text.Trim() + "' and b.type='02'");
                            if (dt.Rows.Count > 0) fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
                            else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Selected Reel Not Found!!");
                        }
                        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Selected Reel Not Found!!");
                    }
                    else fgen.msg("-", "AMSG", "Please fill Correct Reel No.");
                }
                break;
            case "22095":
            case "F25125":
                if (sg1.Rows.Count > 0)
                {
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        TextBox tk1 = (gr.FindControl("sg1_tk1") as TextBox);
                        if (tk1.Text.Length <= 0) tk1.Text = gr.Cells[4].Text.Trim();
                    }
                    fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Please Scan atleast one reel!!')", true);
                    //fgen.msg("-", "AMSG", "Please Scan atleast one reel!!");
                    txtinv.Focus();
                }
                break;
            case "F15125":
                if (sg1.Rows.Count > 0) fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Please Scan atleast one Item!!')", true);
                    txtinv.Focus();
                }
                break;
            case "F25124":
                if (sg1.Rows.Count > 0) fgen.msg("-", "SMSG", "Are You Sure, You Want to Update Reel Loction!!");
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Please Scan Reel First!!')", true);
                    txtinv.Focus();
                }
                break;
            default:
                if (sg1.Rows.Count > 0)
                {
                    fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
                }
                break;
        }
    }
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Del";
        disp_data();
        //fgen.open_sseek("-");
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btncan_ServerClick(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl(); sg1.DataSource = null; ViewState["sg1"] = null;
        enablectrl(); sg1.DataBind(); btnnew.Focus(); ddreelloc.Items.Clear();
        lblwtis.Text = ""; lblwtrq.Text = ""; lbljobname.Text = "";
    }
    protected void btnext_ServerClick(object sender, EventArgs e) { Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr); }
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
        frm_tab_ivch = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_IVCH");
        frm_tab_reel = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_REEL");

        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();

            if (col1 == "Y")
            {
                if (frm_formID == "F15125")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tab_ivch + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + edmode.Value + "'");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from WSR_CTRL a where a.branchcd||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + edmode.Value + "'");
                    fgen.msg("-", "AMSG", "Details are deleted for order " + edmode.Value.Substring(4, 6) + "");
                    clearctrl(); fgen.ResetForm(this.Controls);
                }
                else
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivoucherp a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + edmode.Value + "'");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from WSR_CTRL a where a.branchcd||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + edmode.Value + "'");
                    fgen.msg("-", "AMSG", "Details are deleted for order " + edmode.Value.Substring(4, 6) + "");
                    clearctrl(); fgen.ResetForm(this.Controls);
                }

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
                    if (col1.Length > 5) { }
                    else return;
                    clearctrl();
                    edmode.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;
            }

        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tab_ivch = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_IVCH");
        frm_tab_reel = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_REEL");
        DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

        if (hffield.Value == "List")
        {
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            switch (Prg_Id)
            {
                case "32011":
                case "F20125":
                    SQuery = "Select a.vchnum as ge_no,to_char(a.vchdate,'dd/mm/yyyy') as ge_date,b.aname as party_name,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,trim(a.icode) as item_code,trim(c.iname) as item_name,a.ent_by,TO_CHAR(A.ent_dt,'DD/MM/YYYY HH24:MI') AS ENTDT,A.IQTYOUT AS QTY,d.MO_VEHI as vehicle from " + frm_tab_ivch + " a,famst b,item c,sale d where trim(a.stage)||trim(a.iopr)||trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')=d.branchcd||d.type||trim(d.vchnum)||to_Char(d.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type ='" + frm_vty + "' and a.vchdate " + DateRange + " order by a.vchnum desc";
                    SQuery = "Select a.vchnum as go_no,to_char(a.vchdate,'dd/mm/yyyy') as go_date,b.aname as party_name,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,a.ent_by as GO_by,trim(to_char(d.remvdate,'dd/mm/yyyy')||' '||d.invtime) as inv_creation_date_time,TO_CHAR(A.ent_dt,'DD/MM/YYYY HH24:MI') AS Inv_Gate_Out_Date,round(round(a.ent_dt- to_Date(trim(to_char(d.remvdate,'dd/mm/yyyy')||' '||d.invtime),'dd/mm/yyyy hh24:mi:ss'),2) * 60 * 24) as time_taken_in_min,trim(a.icode) as item_code,trim(c.iname) as item_name,A.IQTYOUT AS QTY,d.MO_VEHI as vehicle from " + frm_tab_ivch + " a,famst b,item c,sale d where trim(a.stage)||trim(a.iopr)||trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')=d.branchcd||d.type||trim(d.vchnum)||to_Char(d.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type ='" + frm_vty + "' and a.vchdate " + DateRange + " order by a.vchnum desc";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "22095":
                case "F25125":
                    SQuery = "select trim(a.vchnum) as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,Trim(a.acode) as reel_no,trim(a.icode) as item_code, trim(b.iname) as item_name,num1 as Qty, num2 as phy_qty  from " + frm_tab_ivch + " a, item b  where trim(a.icode)=trim(b.icode) and a.type='" + frm_vty + "' and a.branchcd='" + frm_mbr + "' and vchdate " + DateRange + " order by entry_no";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
                    break;

                case "F15125":
                    SQuery = "SELECT TRIM(A.VCHNUM) AS ENTRY_NO, TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRY_DATE ,TRIM(A.ACODE) AS SUPPLIER_CODE,TRIM(B.ANAME) AS SUPPLIER_NAME, TRIM(A.ICODE) AS ITEM_CODE,TRIM(C.INAME) AS ITEM_NAME ,A.JOBCARDQTY AS QTY FROM " + frm_tab_ivch + " A, FAMST B , ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + DateRange + "";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " For the Period of " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "F25124":
                    SQuery = "SELECT * FROM REELVCH WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '0%' AND VCHDATE " + DateRange + " AND TRIM(RLOCN)!='-'";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " For the Period of " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "F50125":
                    SQuery = "SELECT A.VCHNUM AS ENTRY_NO,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS ENTRY_DT,A.ICODE AS ERPCODE,B.INAME AS PRODUCT,B.CPARTNO,A.IQTYOUT AS QTY FROM IVOUCHERW A,ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODe) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '" + frm_vty + "%' AND A.VCHDATE " + DateRange + "";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " For the Period of " + fromdt + " to " + todt, frm_qstr);
                    break;
            }
            hffield.Value = "-";
        }
        else
        {
            set_Val();
            col1 = ""; System.Text.StringBuilder msb;
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                // HCID = Request.Cookies["rid"].Value.ToString();
                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                switch (Prg_Id)
                {
                    case "F15125":
                        #region
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tab_ivch);
                        if (edmode.Value == "Y") vchnum = txtvchnum.Text.Trim();
                        else vchnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from " + frm_tab_ivch + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                        foreach (GridViewRow gdr in sg1.Rows)
                        {
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, "Select trim(icode) AS ICODE,trim(iname),imax,imin,iord from item where trim(icode)='" + gdr.Cells[2].Text.Trim() + "'");

                            oporow = oDS.Tables[0].NewRow();
                            oporow["BRANCHCD"] = frm_mbr;
                            oporow["TYPE"] = frm_vty;
                            oporow["vchnum"] = vchnum;
                            oporow["vchdate"] = vardate;
                            //oporow["icode"] = dt.Rows[0]["kclreelno"].ToString().Trim();
                            oporow["icode"] = dt.Rows[0]["icode"].ToString().Trim();
                            oporow["acode"] = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(acode) as acode from famst where trim(acode)='" + gdr.Cells[1].Text.Trim().Substring(2, 6) + "'", "acode");

                            //oporow["iqtyin"] = dt.Rows[0]["imax"];
                            oporow["jobcardqty"] = dt.Rows[0]["iord"];
                            // oporow["iqty_chl"] = dt.Rows[0]["iord"];
                            oporow["ent_by"] = frm_uname;
                            oporow["ent_dt"] = vardate;
                            oporow["edt_by"] = "-";
                            oporow["edt_dt"] = vardate;
                            oDS.Tables[0].Rows.Add(oporow);
                        }
                        if (col1 == "Y")
                        {
                            string doc_is_ok = "";
                            frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tab_ivch, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                            doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                            if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tab_ivch);

                        #region send mail

                        mq0 = ""; //int j = 0;
                        dt3 = new DataTable();
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, "Select distinct trim(a.acode) as Acode,TRIM(A.ICODE) AS ICODE,trim(c.iname) as iname,c.cpartno,trim(b.aname) as aname,trim(b.email) as email,trim(b.addr1) as addr1,trim(b.addr2) as addr2,trim(b.addr3) as addr3,trim(b.person) as person,D.IORD as kan_qty ,c.unit from " + frm_tab_ivch + " a,famst b,item c,ITEMBAL D where trim(a.acode)=trim(b.acode) AND TRIM(A.ICODE)=TRIM(D.ICODe) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' AND D.BRANCHCD='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchnum='" + txtvchnum.Text.Trim() + "' and a.vchdate=to_date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy') order by acode");
                        foreach (DataRow dtnew_Row in dt3.Rows)
                        {
                            j = 0;
                            msb = new System.Text.StringBuilder();
                            msb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");

                            //dt2 = new DataTable();
                            // dt2 = fgen.getdata(frm_qstr, frm_cocd, "Select distinct a.acode,b.email,trim(A.stage)||TRIM(A.IOPR)||trim(A.invno)||to_char(A.invdate,'dd/mm/yyyy') AS FSTR from "+frm_tab_ivch+" a,famst b,sale c where trim(a.acode)=trim(b.acode) and trim(A.stage)||TRIM(A.IOPR)||trim(A.invno)||to_char(A.invdate,'dd/mm/yyyy')=c.branchcd||c.type||trim(c.vchnum)||to_Char(c.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type='"+frm_vty+"' and a.vchnum='" + txtvchnum.Text.Trim() + "' and a.vchdate=to_date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy') and trim(a.acode)='" + dtnew_Row["acode"].ToString().Trim() + "' and c.cscode='" + dtnew_Row["cscode"].ToString().Trim() + "' order by a.acode");
                            #region Mail Body of WPPL
                            //foreach (DataRow dr_2 in dt2.Rows)
                            //{
                            //dt1 = new DataTable();
                            //SQuery = "Select a.*,b.aname,b.email,to_char(a.invdate,'dd/mm/yyyy') as vhd,TRIM(C.PRT_NM1) AS PRT_NM1,TRIM(c.PRT_NM2) AS PRT_NM2,TRIM(C.PRT_NM3) AS PRT_NM3, TRIM(C.PRT_NM4) AS PRT_NM4,c.cpartno,c.cdrgno,d.drv_name,d.drv_mobile,d.mo_vehi,d.mode_tpt,e.finvno as pono from ivoucherp a,famst b,item c,sale d,ivoucher e where trim(a.acode)=trim(b.acodE) and trim(a.icode)=trim(c.icodE) and trim(a.branchcd)||TRIM(a.iopr)||trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')=trim(d.branchcd)||TRIM(d.type)||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy') and trim(a.branchcd)||TRIM(a.iopr)||trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')||trim(a.icode)=trim(e.branchcd)||TRIM(e.type)||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy')||trim(e.icode) and a.branchcd='" + frm_mbr + "' and a.type='ZG' and a.vchnum='" + txtvchnum.Text.Trim() + "' and a.vchdate=to_date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy') and trim(a.acode)='" + dr_2["acode"].ToString().Trim() + "'";

                            //SQuery = "Select B.ACODE AS PCODE,b.PNAME AS aname,b.addr1 as addr1,e.iqtyout,b.email,e.vchnum as invno,to_char(e.vchdate,'dd/mm/yyyy') as vhd,TRIM(C.PRT_NM1) AS PRT_NM1,TRIM(c.PRT_NM2) AS PRT_NM2,TRIM(C.PRT_NM3) AS PRT_NM3, TRIM(C.PRT_NM4) AS PRT_NM4,c.cpartno,c.cdrgno,d.drv_name,d.drv_mobile,d.mo_vehi,d.mode_tpt,e.finvno as pono from cSmst b,item c,sale d,ivoucher e where trim(d.CSCODE)=trim(b.ACODE) and trim(e.icode)=trim(c.icodE) and trim(d.branchcd)||TRIM(D.type)||trim(D.vchnum)||to_char(D.vchdate,'dd/mm/yyyy')=trim(e.branchcd)||TRIM(e.type)||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy') and trim(e.branchcd)||TRIM(e.type)||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy')='" + dr_2["FSTR"].ToString().Trim() + "' ";
                            //dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            // if (dt1.Rows.Count <= 0)
                            //{
                            // Same Query Part 2
                            //   SQuery = "Select B.ACODE AS PCODE,b.aname,e.iqtyout,b.email,b.addr1 as addr1,e.vchnum as invno,to_char(e.vchdate,'dd/mm/yyyy') as vhd,TRIM(C.PRT_NM1) AS PRT_NM1,TRIM(c.PRT_NM2) AS PRT_NM2,TRIM(C.PRT_NM3) AS PRT_NM3, TRIM(C.PRT_NM4) AS PRT_NM4,c.cpartno,c.cdrgno,d.drv_name,d.drv_mobile,d.mo_vehi,d.mode_tpt,e.finvno as pono from famst b,item c,sale d,ivoucher e where trim(d.acode)=trim(b.acodE) and trim(e.icode)=trim(c.icodE) and trim(d.branchcd)||TRIM(D.type)||trim(D.vchnum)||to_char(D.vchdate,'dd/mm/yyyy')=trim(e.branchcd)||TRIM(e.type)||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy') and trim(e.branchcd)||TRIM(e.type)||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy')='" + dr_2["FSTR"].ToString().Trim() + "' and trim(d.acode)='" + dr_2["acode"].ToString().Trim() + "'";
                            //   dt1 = new DataTable();
                            //  dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            //}
                            if (dt3.Rows.Count > 0)
                            {
                                if (j == 0)
                                {
                                    msb.Append("M/s  " + dt3.Rows[0]["aname"].ToString().Trim() + ",<br/>");
                                    msb.Append("" + dt3.Rows[0]["addr1"].ToString().Trim() + ",<br/>");
                                    msb.Append("" + dt3.Rows[0]["addr2"].ToString().Trim() + ",<br/>");
                                    msb.Append("" + dt3.Rows[0]["addr3"].ToString().Trim() + ",<br/><br/>");
                                    msb.Append("<br>===========================================================<br>");

                                    if (dt3.Rows[0]["CPARTNO"].ToString().Trim().Length > 3)
                                        msb.Append("<br>Dear <b>" + dt3.Rows[0]["person"].ToString().Trim() + ",</b><br>");
                                    else
                                        msb.Append("<br>Dear Sir/Madam,<br>");

                                    msb.Append("<br>This is to inform you that our part code : <b>" + dt3.Rows[0]["cpartno"].ToString().Trim() + "</b>");
                                    msb.Append("<br>Item Name : <b>" + dt3.Rows[0]["iname"].ToString().Trim() + "</b>");
                                    msb.Append("<br>ERP code : <b>" + dt3.Rows[0]["icode"].ToString().Trim() + "</b> is required at our works.<br>");
                                    msb.Append("<br>Please supply : <b>" + dt3.Rows[0]["kan_qty"].ToString().Trim() + " , " + dt3.Rows[0]["unit"].ToString().Trim() + "</b>");
                                    msb.Append("<br>as per our KANBAN agreement within 3 Days of getting this mail.<br>");
                                    msb.Append("<br>Your prompt supply shall make our association stronger.<br>");
                                    msb.Append("<br><b>Thanks & Regards,</b>");

                                    //msb.Append("<html><body style=' font-weight: 500; font-size: 13px; >");
                                    msb.Append("<br>For " + fgenCO.chk_co(frm_cocd) + "");
                                    dt4 = new DataTable();
                                    if (frm_mbr.Length > 2) SQuery = "select name,addr,addr1 from type where id='B' and type1 in ('" + frm_mbr.Substring(0, 2) + "')";
                                    else SQuery = "select name,addr,addr1 from type where id='B' and type1='" + frm_mbr + "'";
                                    dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                                    msb.Append("<br>(" + dt4.Rows[0][0].ToString().Trim() + ")");
                                    msb.Append("<br>" + dt4.Rows[0][1].ToString().Trim() + "");
                                    msb.Append("<br>" + dt4.Rows[0][2].ToString().Trim() + "<br>");

                                    msb.Append("<br>--------------------------------------------------------");

                                    col3 = fgen.Fn_curr_dt_time(frm_qstr, frm_cocd);
                                    col4 = col3.Substring(10, 9);

                                    msb.Append("<br>Date :  " + col3.Substring(0, 10) + "");
                                    msb.Append("<br>Time :  " + col4 + "");

                                    msb.Append("<br><br><br>");
                                    //msb.Append("</body></html>");
                                }
                                // mq0 = "";                                       
                            }
                            //}
                            //msb.Append("</table>");
                            msb.Append("<br>===========================================================<br>");
                            msb.Append("<br>This Report is Auto generated from the Tejaxo ERP. Please do not reply to this email / sender.");
                            msb.Append("<br>The above details are to be best of information and data available to the ERP system.");
                            msb.Append("<br>Errors or Omissions if any are regretted.<br/>");
                            //msb.Append("Thanks and Regards,<br/>");
                            //msb.Append("" + fgenCO.chk_co(frm_cocd) + "");
                            msb.Append("</body></html>");

                            merr = fgen.send_mail(frm_cocd, "Tejaxo ERP", dt3.Rows[0]["EMAIL"].ToString().Trim(), "", "", "KANBAN System", msb.ToString().Trim());
                            #endregion
                        }
                        #endregion

                        #endregion
                        break;

                    case "32011":
                    case "F20125":
                        #region
                        if (edmode.Value == "Y") fgen.execute_cmd(frm_qstr, frm_cocd, "update ivoucherp set branchcd='DD' where branchcd='" + frm_mbr + "' and type='ZG' and vchnum='" + txtvchnum.Text.Trim() + "' and vchdate=to_date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy') ");

                        //con.Open();
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, "ivoucherp");

                        if (edmode.Value == "Y") vchnum = txtvchnum.Text.Trim();
                        else
                        {
                            string doc_is_ok = "";
                            vchnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, "IVOUCHERP", doc_nf.Value, doc_df.Value, frm_mbr, "ZG", txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                            doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                            if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                        }

                        string fstrar = "";
                        foreach (GridViewRow gr in sg1.Rows)
                        {
                            if (!fstrar.Contains(gr.Cells[1].Text.Trim()))
                            {
                                fstrar += "," + gr.Cells[1].Text.Trim();

                                if (frm_cocd == "DLJM" || frm_cocd == "MEGA" || frm_cocd == "KUNS" || frm_cocd == "WPPL" || frm_cocd == "MINV*" || frm_cocd == "KTPL" || frm_cocd == "GTCF" || frm_cocd == "GIRP" || frm_cocd == "KPPL" || frm_cocd == "KESH" || frm_cocd == "MEGH" || frm_cocd == "UKB") cond = "trim(branchcd)||TRIM(type)||trim(vchnum)||" + datefrmt + "='" + gr.Cells[1].Text.ToString().Trim() + "'";
                                else cond = "trim(branchcd)||TRIM(type)||trim(vchnum)||" + datefrmt + "='" + gr.Cells[1].Text.ToString().Trim() + "'";
                                SQuery = "Select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,type,branchcd,iqtyout,t_deptt,acode,icode from ivoucher where " + cond + "";
                                dt = new DataTable();
                                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        oporow = oDS.Tables[0].NewRow();
                                        oporow["BRANCHCD"] = frm_mbr;
                                        oporow["TYPE"] = "ZG";
                                        oporow["vchnum"] = vchnum;
                                        oporow["vchdate"] = txtvchdate.Text.Trim();
                                        oporow["stage"] = dr["branchcd"].ToString();
                                        oporow["IOPR"] = dr["type"].ToString();
                                        oporow["invno"] = dr["vchnum"].ToString();
                                        oporow["invdate"] = dr["vchdate"].ToString();
                                        oporow["icode"] = dr["icode"].ToString();
                                        oporow["acode"] = dr["acode"].ToString();
                                        oporow["iqtyout"] = dr["iqtyout"];
                                        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TO_CHAR(SYSDATE,'HH24:MI:SS DD/MM/YYYY') AS LDT FROM DUAL", "LDT");
                                        oporow["ent_by"] = frm_uname;
                                        oporow["ent_dt"] = vardate;
                                        oporow["edt_by"] = "-";
                                        oporow["edt_dt"] = vardate;
                                        oDS.Tables[0].Rows.Add(oporow);
                                        //if (frm_cocd == "BUPL" || frm_cocd == "WPPL" || frm_cocd == "GTCF" || frm_cocd == "GIRP" || frm_cocd == "KPPL" || frm_cocd == "KESH" || frm_cocd == "MEGH" || frm_cocd == "UKB") { }
                                        //else
                                        if (mq2 == "Y")
                                        {
                                            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "Select dlv_date from budgmst where trim(desc_)='" + dr["t_deptt"].ToString().Trim() + "' and trim(icode)='" + dr["icode"].ToString().Trim() + "' and trim(acode)='" + dr["acode"].ToString().Trim() + "' and branchcd='" + frm_mbr + "'", "dlv_date");
                                            if (col1 != "0")
                                            {
                                                col1 = Convert.ToDateTime(col1).ToString("dd/MM/yyyy HH:mm:ss");
                                                SQuery = "update budgmst set invno='" + dr["vchnum"].ToString().Trim() + "' ,invdate=to_date('" + dr["vchdate"].ToString() + "','dd/mm/yyyy hh24:mi:ss') ,ibranchcd='" + dr["branchcd"].ToString().Trim() + "' ,itype='" + dr["type"].ToString().Trim() + "' ,iqtyout=" + dr["iqtyout"].ToString() + ", goutno='" + vchnum + "', goutdate=TO_DATE('" + DateTime.Now.ToString("dd/MM/yyyy HH:mi:ss") + "','DD/MM/YYYY hh24:mi:ss') where trim(desc_)='" + dr["t_deptt"].ToString().Trim() + "' and trim(icode)='" + dr["icode"].ToString().Trim() + "' and trim(acode)='" + dr["acode"].ToString().Trim() + "' AND TO_CHAR(DLV_DATE,'DD/MM/YYYY Hh24:mi:ss')=TO_CHAR(TO_DATE('" + col1 + "','DD/MM/YYYY hh24:mI:ss'),'DD/MM/YYYY HH24:MI:SS')";
                                                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                                            }
                                        }
                                        else { }
                                    }
                                    dt.Dispose();
                                }
                                //da.Update(oDS, "ivoucherp");
                                if (oDS.Tables[0].Rows.Count > 0)
                                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tab_ivch); //ivoucherp
                            }
                        }

                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivoucherp where branchcd='DD' and type='ZG' and vchnum='" + txtvchnum.Text.Trim() + "' and vchdate=to_date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy') ");
                            fgen.msg("-", "AMSG", "Data Updated Successfully");
                        }
                        string drivername = "", mobileno = "", vehino = "";
                        if (frm_cocd == "DLJM") { }
                        else
                        {
                            #region send mail
                            if (frm_cocd == "WPPL" || frm_cocd == "GTCF")
                            {
                                mq0 = "";
                                DataTable dtnew_ = new DataTable();

                                dtnew_ = fgen.getdata(frm_qstr, frm_cocd, "Select distinct a.acode,b.cscode from ivoucherp a,sale b where trim(A.stage)||TRIM(A.IOPR)||trim(A.invno)||to_char(A.invdate,'dd/mm/yyyy')=b.branchcd||b.type||trim(b.vchnum)||to_Char(b.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type='ZG' and a.vchnum='" + txtvchnum.Text.Trim() + "' and a.vchdate=to_date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy') order by a.acode");
                                foreach (DataRow dtnew_Row in dtnew_.Rows)
                                {
                                    j = 0;
                                    msb = new System.Text.StringBuilder();
                                    msb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");

                                    dt2 = new DataTable();
                                    dt2 = fgen.getdata(frm_qstr, frm_cocd, "Select distinct a.acode,b.email,trim(A.stage)||TRIM(A.IOPR)||trim(A.invno)||to_char(A.invdate,'dd/mm/yyyy') AS FSTR from ivoucherp a,famst b,sale c where trim(a.acode)=trim(b.acode) and trim(A.stage)||TRIM(A.IOPR)||trim(A.invno)||to_char(A.invdate,'dd/mm/yyyy')=c.branchcd||c.type||trim(c.vchnum)||to_Char(c.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type='ZG' and a.vchnum='" + txtvchnum.Text.Trim() + "' and a.vchdate=to_date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy') and trim(a.acode)='" + dtnew_Row["acode"].ToString().Trim() + "' and c.cscode='" + dtnew_Row["cscode"].ToString().Trim() + "' order by a.acode");
                                    #region Mail Body of WPPL
                                    foreach (DataRow dr_2 in dt2.Rows)
                                    {
                                        dt1 = new DataTable();
                                        SQuery = "Select a.*,b.aname,b.email,to_char(a.invdate,'dd/mm/yyyy') as vhd,TRIM(C.PRT_NM1) AS PRT_NM1,TRIM(c.PRT_NM2) AS PRT_NM2,TRIM(C.PRT_NM3) AS PRT_NM3, TRIM(C.PRT_NM4) AS PRT_NM4,c.cpartno,c.cdrgno,d.drv_name,d.drv_mobile,d.mo_vehi,d.mode_tpt,e.finvno as pono from ivoucherp a,famst b,item c,sale d,ivoucher e where trim(a.acode)=trim(b.acodE) and trim(a.icode)=trim(c.icodE) and trim(a.branchcd)||TRIM(a.iopr)||trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')=trim(d.branchcd)||TRIM(d.type)||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy') and trim(a.branchcd)||TRIM(a.iopr)||trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')||trim(a.icode)=trim(e.branchcd)||TRIM(e.type)||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy')||trim(e.icode) and a.branchcd='" + frm_mbr + "' and a.type='ZG' and a.vchnum='" + txtvchnum.Text.Trim() + "' and a.vchdate=to_date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy') and trim(a.acode)='" + dr_2["acode"].ToString().Trim() + "'";

                                        SQuery = "Select B.ACODE AS PCODE,b.PNAME AS aname,b.addr1 as addr1,e.iqtyout,b.email,e.vchnum as invno,to_char(e.vchdate,'dd/mm/yyyy') as vhd,TRIM(C.PRT_NM1) AS PRT_NM1,TRIM(c.PRT_NM2) AS PRT_NM2,TRIM(C.PRT_NM3) AS PRT_NM3, TRIM(C.PRT_NM4) AS PRT_NM4,c.cpartno,c.cdrgno,d.drv_name,d.drv_mobile,d.mo_vehi,d.mode_tpt,e.finvno as pono from cSmst b,item c,sale d,ivoucher e where trim(d.CSCODE)=trim(b.ACODE) and trim(e.icode)=trim(c.icodE) and trim(d.branchcd)||TRIM(D.type)||trim(D.vchnum)||to_char(D.vchdate,'dd/mm/yyyy')=trim(e.branchcd)||TRIM(e.type)||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy') and trim(e.branchcd)||TRIM(e.type)||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy')='" + dr_2["FSTR"].ToString().Trim() + "' ";
                                        dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                                        if (dt1.Rows.Count <= 0)
                                        {
                                            // Same Query Part 2
                                            SQuery = "Select B.ACODE AS PCODE,b.aname,e.iqtyout,b.email,b.addr1 as addr1,e.vchnum as invno,to_char(e.vchdate,'dd/mm/yyyy') as vhd,TRIM(C.PRT_NM1) AS PRT_NM1,TRIM(c.PRT_NM2) AS PRT_NM2,TRIM(C.PRT_NM3) AS PRT_NM3, TRIM(C.PRT_NM4) AS PRT_NM4,c.cpartno,c.cdrgno,d.drv_name,d.drv_mobile,d.mo_vehi,d.mode_tpt,e.finvno as pono from famst b,item c,sale d,ivoucher e where trim(d.acode)=trim(b.acodE) and trim(e.icode)=trim(c.icodE) and trim(d.branchcd)||TRIM(D.type)||trim(D.vchnum)||to_char(D.vchdate,'dd/mm/yyyy')=trim(e.branchcd)||TRIM(e.type)||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy') and trim(e.branchcd)||TRIM(e.type)||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy')='" + dr_2["FSTR"].ToString().Trim() + "' and trim(d.acode)='" + dr_2["acode"].ToString().Trim() + "'";
                                            dt1 = new DataTable();
                                            dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                                        }
                                        if (dt1.Rows.Count > 0)
                                        {
                                            if (j == 0)
                                            {
                                                msb.Append("M/s  " + dt1.Rows[0]["aname"].ToString().Trim() + ",<br/>");
                                                msb.Append("" + dt1.Rows[0]["addr1"].ToString().Trim() + ",<br/><br/>");
                                                msb.Append("for your kind information below items has been despatched to you<br/><br/>");
                                                msb.Append("<table cellspacing=2 cellpadding=2 style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; color: #474646'>");
                                                msb.Append("<tr style='font-weight: 700; font-family: Arial, Helvetica, sans-serif'><td><b>Invoice No</b></td><td><b>Invoice Date</b></td><td><b>Po.No.</b></td><td><b>Description</b></td><td><b>Qty.</b></td></tr>");
                                            }
                                            mq0 = "";
                                            foreach (DataRow dr in dt1.Rows)
                                            {
                                                if (mq0.Length <= 1)
                                                {
                                                    mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT EMAIL FROM CSMST WHERE TRIM(ACODE)='" + dr["pcode"].ToString().Trim() + "'", "EMAIL");
                                                    if (mq0.Length <= 1) mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT EMAIL FROM FAMST WHERE TRIM(ACODE)='" + dr["pcode"].ToString().Trim() + "'", "EMAIL");
                                                }

                                                if (fgen.make_double(dr["iqtyout"].ToString()) > 0)
                                                {
                                                    msb.Append("<tr>");
                                                    msb.Append("<td>");
                                                    msb.Append(dr["invno"].ToString());
                                                    msb.Append("</td>");
                                                    msb.Append("<td>");
                                                    msb.Append(dr["vhd"].ToString());
                                                    msb.Append("</td>");
                                                    msb.Append("<td>");
                                                    if (frm_cocd == "DLJM") msb.Append(dr["cdrgno"].ToString());
                                                    else if (frm_cocd == "WPPL" || frm_cocd == "GTCF") msb.Append(dr["pono"].ToString());
                                                    //else msb.Append(dr["PRT_NM1"].ToString() + "<br>" + dr["PRT_NM2"].ToString() + "<br>" + dr["PRT_NM3"].ToString() + "<br>" + dr["PRT_NM4"].ToString());
                                                    msb.Append("</td>");
                                                    msb.Append("<td style='width:250px;'>");
                                                    //msb.Append(dr["iname"].ToString());
                                                    msb.Append(dr["PRT_NM1"].ToString() + "<br>" + dr["PRT_NM2"].ToString() + "<br>" + dr["PRT_NM3"].ToString() + "<br>" + dr["PRT_NM4"].ToString());
                                                    msb.Append("</td>");
                                                    msb.Append("<td>");
                                                    msb.Append(dr["iqtyout"].ToString());
                                                    msb.Append("</td>");
                                                    msb.Append("</tr>");

                                                    drivername = dr["drv_name"].ToString();
                                                    mobileno = dr["drv_mobile"].ToString();
                                                    if (frm_cocd == "DLJM")
                                                        vehino = dr["mode_tpt"].ToString();
                                                    else vehino = dr["mo_vehi"].ToString();
                                                    j++;
                                                }
                                            }
                                        }
                                    }
                                    msb.Append("</table>");

                                    msb.Append("<br><br>");
                                    vardate = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TO_CHAR(SYSDATE,'HH24:MI:SS DD/MM/YYYY') AS LDT FROM DUAL", "LDT");

                                    msb.Append("Gate Out Time : " + vardate + "<br>");
                                    msb.Append("Driver Name   : " + drivername + "<br>");
                                    msb.Append("Driver Phone  : " + mobileno + "<br>");
                                    msb.Append("Vehicle No.   : " + vehino + "<br>");

                                    msb.Append("<br><br>");

                                    msb.Append("<br>===========================================================<br>");
                                    msb.Append("<br>This Report is Auto generated from the Tejaxo ERP. Please do not reply to this email / sender.");
                                    msb.Append("<br>The above details are to be best of information and data available to the ERP system.");
                                    msb.Append("<br>Errors or Omissions if any are regretted.<br/>");
                                    msb.Append("Thanks and Regards,<br/>");
                                    msb.Append("" + fgenCO.chk_co(frm_cocd) + "");
                                    msb.Append("</body></html>");

                                    //merr = fgen.send_mail("Tejaxo ERP", mq0, "", "", "Material Gate Outward", msb.ToString().Trim(), "smtp.gmail.com", 587, 1, "rrrbaghel@gmail.com", "finsyserp123");
                                    merr = fgen.send_mail(frm_cocd, "Tejaxo ERP", mq0, "", "", "Material Gate Outward", msb.ToString().Trim());
                                    #endregion
                                }
                            }
                            else
                            {
                                fstrar = "";
                                foreach (GridViewRow gr in sg1.Rows)
                                {
                                    if (!fstrar.Contains(gr.Cells[1].Text.Trim()))
                                    {
                                        fstrar += "," + gr.Cells[1].Text.Trim();

                                        if (1 == 1) cond = "and trim(a.branchcd)||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd')='" + gr.Cells[1].Text.Trim() + "'";
                                        else cond = "and trim(a.branchcd)||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + gr.Cells[1].Text.Trim() + "'";
                                        SQuery = "Select a.*,b.aname,b.email,to_char(a.vchdate,'dd/mm/yyyy') as vhd,c.iname,c.cpartno,c.cdrgno,d.drv_name,d.drv_mobile,d.mo_vehi,d.mode_tpt from ivoucher a,famst b,item c,sale d where trim(a.acode)=trim(b.acodE) and trim(a.icode)=trim(c.icodE) and trim(a.branchcd)||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(d.branchcd)||TRIM(d.type)||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy') " + cond + "";
                                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                                        msb = new System.Text.StringBuilder();
                                        msb.Append("<html><body>");
                                        msb.Append("M/s  " + dt.Rows[0]["aname"].ToString().Trim() + ",<br/><br/>");
                                        msb.Append("for your kind information below items has been despatched to you<br/><br/>");
                                        msb.Append("<table style='border-collapse: collapse; border: 1px solid black;'>");
                                        msb.Append("<tr style='border-collapse: collapse; border: 1px solid black;'>" +
                                            "<td style='border-collapse: collapse; border: 1px solid black;'><b>Invoice No</b></td><td style='border-collapse: collapse; border: 1px solid black;'><b>Invoice Date</b></td><td style='border-collapse: collapse; border: 1px solid black;'><b>Po.No.</b></td><td style='border-collapse: collapse; border: 1px solid black;'><b>Description</b></td><td style='border-collapse: collapse; border: 1px solid black;'><b>Qty.</b></td></tr>");
                                        foreach (DataRow dr in dt.Rows)
                                        {
                                            msb.Append("<tr style='border-collapse: collapse; border: 1px solid black;'><td style='border-collapse: collapse; border: 1px solid black;'>");
                                            msb.Append(dr["invno"].ToString());
                                            msb.Append("</td>");
                                            msb.Append("<td style='border-collapse: collapse; border: 1px solid black;'>");
                                            msb.Append(dr["vhd"].ToString());
                                            msb.Append("</td>");
                                            msb.Append("<td style='border-collapse: collapse; border: 1px solid black;'>");
                                            if (frm_cocd == "DLJM") msb.Append(dr["cdrgno"].ToString());
                                            else msb.Append(dr["cpartno"].ToString());
                                            msb.Append("</td>");
                                            msb.Append("<td style='border-collapse: collapse; border: 1px solid black; width:250px'>");
                                            msb.Append(dr["iname"].ToString());
                                            msb.Append("</td>");
                                            msb.Append("<td style='border-collapse: collapse; border: 1px solid black;'>");
                                            msb.Append(dr["iqtyout"].ToString());
                                            msb.Append("</td>");

                                            msb.Append("</tr>");

                                            drivername = dr["drv_name"].ToString().Trim();
                                            mobileno = dr["drv_mobile"].ToString().Trim();
                                            if (frm_cocd == "DLJM")
                                                vehino = dr["mode_tpt"].ToString().Trim();
                                            else vehino = dr["mo_vehi"].ToString().Trim();
                                        }
                                        msb.Append("</table><br/><br/>");

                                        msb.Append("<br><br>");
                                        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TO_CHAR(SYSDATE,'HH24:MI:SS DD/MM/YYYY') AS LDT FROM DUAL", "LDT");

                                        msb.Append("Gate Out Time : " + vardate + "<br>");
                                        msb.Append("Driver Name   : " + drivername + "<br>");
                                        msb.Append("Driver Phone  : " + mobileno + "<br>");
                                        msb.Append("Vehicle No.   : " + vehino + "<br>");

                                        msb.Append("<br><br>");

                                        msb.Append("<br>===========================================================<br>");
                                        msb.Append("<br>This Report is Auto generated from the Tejaxo ERP.");
                                        msb.Append("<br>The above details are to be best of information and data available to the ERP system.");
                                        msb.Append("<br>Errors or Omissions if any are regretted.");
                                        msb.Append("Thanks and Regards,<br/>");
                                        //msb.Append("" + fgen.chk_co(co_cd) + "");
                                        msb.Append("" + fgenCO.chk_co(frm_cocd) + " Mail No 5001");
                                        msb.Append("</body></html>");
                                        string subj = "Invoice Gate Out by Scan at " + vardate + "";
                                        if (frm_cocd == "KTPL") //merr = fgen.send_mail(frm_cocd, "Tejaxo ERP", dt.Rows[0]["email"].ToString().Trim(), "gsharma@eshinegroup.co.in", "sonali@eshinegroup.co.in;", "Material Gate Outward", msb.ToString(), "192.168.1.10", 25, 0, "reports@kthree-india.com", "Test@4321");
                                            merr = fgen.send_mail(frm_cocd, "Tejaxo ERP", dt.Rows[0]["email"].ToString().Trim(), "gsharma@eshinegroup.co.in", "sonali@eshinegroup.co.in;", subj, msb.ToString());
                                        else //fgen.send_mail_new(frm_cocd,"Tejaxo ERP", dt.Rows[0]["email"].ToString().Trim(), "", "", "Material Gate Outward", msb.ToString());
                                            fgen.send_mail(frm_cocd, "Tejaxo ERP", dt.Rows[0]["email"].ToString().Trim(), "", "", subj, msb.ToString());
                                    }
                                }
                            }
                            #endregion
                        }
                        #endregion
                        break;
                    case "22055":
                    case "AK17":
                        #region
                        if (sg1.Rows.Count > 0)
                        {
                            //con.Open();

                            //da = new OracleDataAdapter(new OracleCommand("SELECT * FROM ivoucher where 1=2 ", con));
                            // cb = new OracleCommandBuilder(da);
                            oDS = new DataSet();
                            oDS = fgen.fill_schema(frm_qstr, frm_cocd, "ivoucher");
                            // da.FillSchema(oDS, SchemaType.Source);
                            //con.Close();
                            //pTable = oDS.Tables["Table"];
                            //pTable.TableName = "ivoucher";
                            vchnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from ivoucher where type='31' AND BRANCHCD='" + frm_mbr + "' and vchdate " + DateRange + "", 6, "vch");
                            vardate = fgen.seek_iname(frm_qstr, frm_cocd, "Select TO_DATE(to_Char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy') as syd from dual", "syd").Trim();
                            for (int i = 0; i < sg1.Rows.Count; i++)
                            {
                                DataRow oporow = oDS.Tables[0].NewRow();
                                dt = new DataTable();
                                dt = fgen.getdata(frm_qstr, frm_cocd, "select a.grade,a.psize,a.gsm,a.acode,a.irate,b.unit,a.reelspec2 from reelvch a,ivoucher b where trim(a.icode)=trim(b.icode) and b.type='02' and trim(kclreelno)='" + sg1.Rows[i].Cells[1].Text.Trim() + "' and a.type='02'");

                                oporow["vchnum"] = vchnum.Trim();
                                oporow["vchdate"] = vardate;
                                oporow["BRANCHCD"] = frm_mbr;
                                oporow["TYPE"] = "31";
                                oporow["srno"] = sg1.Rows[i].Cells[0].Text.Trim();
                                oporow["icode"] = sg1.Rows[i].Cells[2].Text.Trim();
                                oporow["iqtyout"] = sg1.Rows[i].Cells[3].Text.Trim();
                                oporow["iqty_chl"] = sg1.Rows[i].Cells[3].Text.Trim();
                                oporow["REC_ISS"] = "C";
                                oporow["ACODe"] = "64";
                                oporow["o_deptt"] = "PRODUCTION";
                                oporow["t_deptt"] = "Stores";
                                oporow["STORE"] = "Y";
                                oporow["iqtyin"] = 0;
                                oporow["ichgs"] = 0;
                                oporow["freight"] = "-";
                                oporow["tc_no"] = "-";
                                oporow["iqty_wt"] = 0;
                                oporow["styleno"] = "-";
                                oporow["location"] = "-";
                                oporow["store_no"] = "00";
                                oporow["buyer"] = "-";
                                oporow["isize"] = "-";
                                oporow["cavity"] = "1";
                                oporow["iweight"] = 0;
                                oporow["inspected"] = "Y";
                                oporow["DESC_"] = dt.Rows[0]["psize"] + " X " + dt.Rows[0]["gsm"];
                                oporow["genum"] = dt.Rows[0]["psize"];
                                oporow["gedate"] = vardate;
                                oporow["irate"] = dt.Rows[0]["irate"];
                                oporow["iamount"] = Convert.ToDouble(dt.Rows[0]["irate"]) * Convert.ToDouble(sg1.Rows[i].Cells[3].Text.Trim());
                                oporow["invno"] = sg1.Rows[i].Cells[5].Text.Trim();
                                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "Select replace(nvl(trim(enable_yN),'N'),'-','N') as cond from controls where id='M192'", "cond");
                                if (mhd == "Y") oporow["invdate"] = sg1.Rows[i].Cells[5].Text.Trim();
                                else oporow["invdate"] = fgen.seek_iname(frm_qstr, frm_cocd, "Select to_char(to_date('" + sg1.Rows[i].Cells[6].Text.Trim() + "','yyyymmdd'),'dd/mm/yyyy') as syd from dual", "syd");
                                oporow["unit"] = dt.Rows[0]["unit"].ToString();

                                oporow["ENT_BY"] = frm_uname;
                                oporow["ENT_Dt"] = vardate;
                                oporow["EdT_BY"] = "-";
                                oporow["EdT_Dt"] = vardate;

                                oDS.Tables[0].Rows.Add(oporow);
                            }
                            if (col1 == "Y")
                            {
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tab_ivch, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                            fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tab_ivch); //ivoucher

                            //da.Update(oDS, "ivoucher");
                            //con.Open();
                            //da = new OracleDataAdapter(new OracleCommand("SELECT * FROM reelvch where 1=2 ", con));
                            //cb = new OracleCommandBuilder(da);
                            oDS = new DataSet();
                            oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tab_reel);
                            //da.FillSchema(oDS, SchemaType.Source);
                            //con.Close();
                            //pTable = oDS.Tables["Table"];
                            //pTable.TableName = "reelvch";
                            for (int i = 0; i < sg1.Rows.Count; i++)
                            {
                                DataRow oporow = oDS.Tables[0].NewRow();

                                dt = new DataTable();
                                dt = fgen.getdata(frm_qstr, frm_cocd, "select a.grade,a.psize,a.gsm,a.acode,a.irate,a.reelspec2,a.COREELNO from reelvch a where trim(kclreelno)='" + sg1.Rows[i].Cells[1].Text.Trim() + "' and a.type='02'");

                                dt2 = new DataTable();
                                dt2 = fgen.getdata(frm_qstr, frm_cocd, "select distinct A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Qty,B.INAME as Item_Name,a.Icode,b.Cpartno from costestimate A,ITEM B  WHERE upper(trim(a.status))<>'Y' and A.SRNO=0 AND trim(A.ICODE)=trim(B.ICODE) and a.type='30' and a.vchnum='" + sg1.Rows[i].Cells[5].Text.Trim() + "' order by A.vchdate desc ,A.vchnum desc");

                                oporow["vchnum"] = vchnum.Trim();
                                oporow["vchdate"] = vardate;
                                oporow["BRANCHCD"] = frm_mbr;
                                oporow["TYPE"] = "31";
                                oporow["srno"] = sg1.Rows[i].Cells[0].Text.Trim();
                                oporow["kclreelno"] = sg1.Rows[i].Cells[1].Text.Trim();
                                oporow["icode"] = sg1.Rows[i].Cells[2].Text.Trim();
                                oporow["reelwout"] = sg1.Rows[i].Cells[3].Text.Trim();
                                oporow["reelwin"] = 0;
                                oporow["irate"] = dt.Rows[0]["irate"];
                                oporow["psize"] = dt.Rows[0]["psize"];
                                oporow["gsm"] = dt.Rows[0]["gsm"];
                                oporow["COREELNO"] = dt.Rows[0]["COREELNO"];
                                oporow["job_no"] = sg1.Rows[i].Cells[5].Text.Trim();
                                mhd = fgen.seek_iname(frm_qstr, frm_cocd, "Select replace(nvl(trim(enable_yN),'N'),'-','N') as cond from controls where id='M192'", "cond");
                                if (mhd == "Y") oporow["job_dt"] = sg1.Rows[i].Cells[5].Text.Trim();
                                oporow["job_dt"] = fgen.seek_iname(frm_qstr, frm_cocd, "Select to_char(to_date('" + sg1.Rows[i].Cells[6].Text.Trim() + "','yyyymmdd'),'dd/mm/yyyy') as syd from dual", "syd");
                                if (dt2.Rows[0]["Item_Name"].ToString().Length > 30) oporow["REELSPEC2"] = dt2.Rows[0]["item_name"].ToString().Substring(0, 30);
                                else oporow["REELSPEC2"] = dt2.Rows[0]["Item_Name"];
                                //oporow["REELSPEC2"] = dt2.Rows[0]["Item_Name"];

                                oporow["REC_ISS"] = "C";
                                oporow["GRADE"] = "A";
                                oporow["ACODe"] = dt.Rows[0]["acode"];
                                oporow["STORE_no"] = "00";
                                oporow["posted"] = "Y";
                                oporow["UNLINK"] = "N";

                                oDS.Tables[0].Rows.Add(oporow);
                            }

                            //da.Update(oDS, "reelvch");
                            fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tab_reel);
                        }
                        #endregion
                        break;
                    case "22095":
                    case "F25125":
                        #region
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tab_ivch);
                        if (edmode.Value == "Y") vchnum = txtvchnum.Text.Trim();
                        else vchnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tab_ivch, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);

                        mhd = "0";
                        foreach (GridViewRow gdr in sg1.Rows)
                        {
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, "Select a.*,b.iname from (select trim(icode) as icode,kclreelno,sum(reelwin)-sum(reelwout) as tot from (select distinct trim(icode) as icode,kclreelno,sum(reelwin) as reelwin,0 as reelwout from reelvch where substr(type,1,1) in ('0','1') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode) union all select distinct trim(icode) as icode,kclreelno,0 as reelwin,sum(reelwout) as reelwout from reelvch where type in ('31','32') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode)) where trim(kclreelno)='" + gdr.Cells[1].Text.Trim() + "' group by kclreelno,trim(icode)) a,item b where trim(a.icodE)=trim(b.icode) and a.tot>0");
                            if (dt.Rows.Count > 0)
                            {
                                mhd = "1";
                                oporow = oDS.Tables[0].NewRow();
                                oporow["BRANCHCD"] = frm_mbr;
                                oporow["TYPE"] = "RL";
                                oporow["vchnum"] = vchnum;
                                oporow["vchdate"] = vardate;
                                oporow["acode"] = dt.Rows[0]["kclreelno"].ToString().Trim();
                                oporow["icode"] = dt.Rows[0]["icode"].ToString().Trim();
                                oporow["num1"] = dt.Rows[0]["tot"];
                                oporow["num2"] = ((TextBox)gdr.FindControl("sg1_tk1")).Text.Trim().toDouble();
                                oporow["ent_by"] = frm_uname;
                                oporow["ent_dt"] = vardate;
                                oporow["edt_by"] = "-";
                                oporow["edt_dt"] = vardate;
                                oDS.Tables[0].Rows.Add(oporow);
                            }
                        }
                        if (mhd == "1") //da.Update(oDS, "scratch");                        
                            fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tab_ivch);
                        #endregion
                        break;
                    case "F50125":
                        #region
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tab_ivch);
                        if (edmode.Value == "Y") vchnum = txtvchnum.Text.Trim();
                        else vchnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tab_ivch, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);

                        mhd = "0";
                        foreach (GridViewRow gdr in sg1.Rows)
                        {
                            {
                                mhd = "1";
                                oporow = oDS.Tables[0].NewRow();
                                oporow["BRANCHCD"] = frm_mbr;
                                oporow["TYPE"] = frm_vty;
                                oporow["vchnum"] = vchnum;
                                oporow["vchdate"] = vardate;

                                oporow["morder"] = (gdr.RowIndex + 1);
                                oporow["icode"] = gdr.Cells[2].Text.Trim();
                                oporow["btchno"] = gdr.Cells[5].Text.Trim();
                                oporow["purpose"] = gdr.Cells[1].Text.Trim();
                                oporow["iqtyout"] = gdr.Cells[6].Text.Trim().toDouble();


                                oporow["ent_by"] = frm_uname;
                                oporow["ent_dt"] = vardate;
                                oporow["edt_by"] = "-";
                                oporow["edt_dt"] = vardate;
                                oDS.Tables[0].Rows.Add(oporow);
                            }
                        }
                        if (mhd == "1") //da.Update(oDS, "scratch");                        
                            fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tab_ivch);
                        #endregion
                        break;
                    case "F25124":
                        if (sg1.Rows.Count > 0)
                        {
                            for (int i = 0; i < sg1.Rows.Count; i++)
                            {
                                hffield.Value = sg1.Rows[i].Cells[1].Text.Trim();
                                SQuery = "UPDATE REELVCH SET RLOCN='" + ddreelloc.SelectedItem.Text.Trim().ToUpper() + "' WHERE BRANCHCD||TYPE||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')||TRIM(ICODE)||TRIM(KCLREELNO)='" + hffield.Value + "'";
                                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                            }
                        }
                        break;
                }
                fgen.msg("-", "AMSG", "Data Saved Successfully.");
                fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); clearctrl(); sg1.DataSource = null; sg1.DataBind(); enablectrl(); txtinv.ReadOnly = false; lblwtis.Text = ""; lblwtrq.Text = ""; lbljobname.Text = ""; ViewState["sg1"] = null; ddreelloc.Items.Clear();
            }
        }
    }
    public void add_R()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        switch (Prg_Id)
        {
            case "32011":
            case "F20125":
                #region  Gate Out Entry
                if (ViewState["sg1"] != null)
                {
                    dt1 = new DataTable();
                    dt1 = (DataTable)ViewState["sg1"];
                    DataRow dr1 = null;

                    if (txtinv.Text.Substring(2, 1) == "5")
                    {
                        if (txtinv.Text.Substring(2, 2) == "50") frm_vty = "4A";
                        if (txtinv.Text.Substring(2, 2) == "51") frm_vty = "4B";
                        if (txtinv.Text.Substring(2, 2) == "52") frm_vty = "4C";
                        if (txtinv.Text.Substring(2, 2) == "53") frm_vty = "4D";
                        if (txtinv.Text.Substring(2, 2) == "54") frm_vty = "4E";
                        if (txtinv.Text.Substring(2, 2) == "55") frm_vty = "4F";
                        if (txtinv.Text.Substring(2, 2) == "56") frm_vty = "4G";

                        if (txtinv.Text.Trim().Length > 18) value1 = txtinv.Text.Substring(0, 2) + frm_vty + txtinv.Text.Substring(4, 16);
                        else value1 = txtinv.Text.Substring(0, 2) + frm_vty + txtinv.Text.Substring(4, 14);
                    }
                    else value1 = txtinv.Text.Trim();

                    if (frm_cocd == "DLJM" || frm_cocd == "WPPL" || frm_cocd == "GTCF" || frm_cocd == "MEGA" || frm_cocd == "KUNS" || frm_cocd == "KPPL" || frm_cocd == "KESH" || frm_cocd == "MEGH" || frm_cocd == "UKB" || frm_cocd == "MIRP") cond = "trim(a.branchcd)||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd')='" + value1.Trim() + "'";
                    else cond = "trim(a.branchcd)||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + value1.Trim() + "'";

                    dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, frm_cocd, "Select distinct trim(a.branchcd)||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd') as fstr,b.aname,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,d.drv_name,d.drv_mobile,d.mo_vehi,d.mode_tpt from ivoucher a,famst b,SALE D where trim(a.acodE)=trim(b.acodE) AND A.BRANCHCD||a.TYPE||TRIM(a.VCHNUM)||TO_cHAR(a.VCHDATE,'DD/MM/YYYY')=D.BRANCHCD||D.TYPE||TRIM(D.VCHNUM)||TO_cHAR(D.VCHDATE,'DD/MM/YYYY') and " + cond + "");
                    dt = fgen.getdata(frm_qstr, frm_cocd, "Select trim(a.branchcd)||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd') as fstr,b.aname,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,d.drv_name,d.drv_mobile,d.mo_vehi,d.mode_tpt,a.iqtyout as qty,c.iname,a.morder from ivoucher a,famst b,SALE D,item c where trim(a.icode)=trim(c.icode) and trim(a.acodE)=trim(b.acodE) AND A.BRANCHCD||a.TYPE||TRIM(a.VCHNUM)||TO_cHAR(a.VCHDATE,'DD/MM/YYYY')=D.BRANCHCD||D.TYPE||TRIM(D.VCHNUM)||TO_cHAR(D.VCHDATE,'DD/MM/YYYY') and " + cond + " order by a.morder");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {


                            dr1 = dt1.NewRow();
                            dr1["SrNo"] = dt1.Rows.Count + 1;
                            dr1["fstr"] = dt.Rows[i]["fstr"].ToString().Trim();
                            dr1["invno"] = dt.Rows[i]["vchnum"].ToString().Trim();
                            dr1["invdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                            dr1["party"] = dt.Rows[i]["aname"].ToString().Trim();

                            dr1["Product"] = dt.Rows[i]["iname"].ToString().Trim();
                            dr1["Qty"] = dt.Rows[i]["qty"].ToString().Trim();

                            dr1["DRIVER_NAME"] = dt.Rows[0]["drv_name"].ToString().Trim();
                            dr1["dRIVER_MOB"] = dt.Rows[0]["drv_mobile"].ToString().Trim();
                            dr1["vehicle"] = dt.Rows[0]["MO_VEHI"].ToString().Trim();

                            dt1.Rows.Add(dr1);
                        }
                        ViewState["sg1"] = dt1;
                        fillGrid(dt1);
                    }
                }
                else
                {
                    dt1 = new DataTable();
                    DataRow dr1 = null;
                    dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                    dt1.Columns.Add(new DataColumn("fstr", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Invno", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Invdate", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Party", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Product", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Qty", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Driver_Name", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Driver_Mob", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Vehicle", typeof(string)));

                    if (txtinv.Text.Substring(2, 1) == "5")
                    {
                        if (txtinv.Text.Substring(2, 2) == "50") frm_vty = "4A";
                        if (txtinv.Text.Substring(2, 2) == "51") frm_vty = "4B";
                        if (txtinv.Text.Substring(2, 2) == "52") frm_vty = "4C";
                        if (txtinv.Text.Substring(2, 2) == "53") frm_vty = "4D";
                        if (txtinv.Text.Substring(2, 2) == "54") frm_vty = "4E";
                        if (txtinv.Text.Substring(2, 2) == "55") frm_vty = "4F";
                        if (txtinv.Text.Substring(2, 2) == "56") frm_vty = "4G";

                        if (txtinv.Text.Trim().Length > 18) value1 = txtinv.Text.Substring(0, 2) + frm_vty + txtinv.Text.Substring(4, 16);
                        else value1 = txtinv.Text.Substring(0, 2) + frm_vty + txtinv.Text.Substring(4, 14);
                    }
                    else value1 = txtinv.Text.Trim();

                    if (frm_cocd == "DLJM" || frm_cocd == "MEGA" || frm_cocd == "KUNS" || frm_cocd == "WPPL" || frm_cocd == "GTCF" || frm_cocd == "KPPL" || frm_cocd == "KESH" || frm_cocd == "MEGH" || frm_cocd == "UKB") cond = "trim(a.branchcd)||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + value1 + "'";
                    else cond = "trim(a.branchcd)||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd')='" + value1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "Select trim(a.branchcd)||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd') as fstr,b.aname,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,d.drv_name,d.drv_mobile,d.mo_vehi,d.mode_tpt,a.iqtyout as qty,c.iname,a.morder from ivoucher a,famst b,SALE D,item c where trim(a.icode)=trim(c.icode) and trim(a.acodE)=trim(b.acodE) AND A.BRANCHCD||a.TYPE||TRIM(a.VCHNUM)||TO_cHAR(a.VCHDATE,'DD/MM/YYYY')=D.BRANCHCD||D.TYPE||TRIM(D.VCHNUM)||TO_cHAR(D.VCHDATE,'DD/MM/YYYY') and " + cond + " order by a.morder");
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            dr1 = dt1.NewRow();
                            dr1["SrNo"] = dt1.Rows.Count + 1;
                            dr1["fstr"] = dt.Rows[i]["fstr"].ToString().Trim();
                            dr1["Invno"] = dt.Rows[i]["vchnum"].ToString().Trim();
                            dr1["Invdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                            dr1["Party"] = dt.Rows[i]["aname"].ToString().Trim();

                            dr1["Product"] = dt.Rows[i]["iname"].ToString().Trim();
                            dr1["Qty"] = dt.Rows[i]["qty"].ToString().Trim();

                            dr1["DRIVER_NAME"] = dt.Rows[0]["drv_name"].ToString().Trim();
                            dr1["dRIVER_MOB"] = dt.Rows[0]["drv_mobile"].ToString().Trim();
                            dr1["vehicle"] = dt.Rows[0]["MO_VEHI"].ToString().Trim();

                            dt1.Rows.Add(dr1);
                        }
                        ViewState["sg1"] = dt1;
                        fillGrid(dt1);
                    }
                }
                txtinv.Text = ""; value1 = ""; txtinv.Focus();
                #endregion
                break;
            case "22095":
            case "F25125":
                #region Physical Verification Reel
                if (ViewState["sg1"] != null)
                {
                    dt1 = new DataTable();
                    dt1 = (DataTable)ViewState["sg1"];
                    DataRow dr1 = null;
                    if (dt1.Rows.Count > 0)
                    {
                        dr1 = dt1.NewRow();
                        dr1["SrNo"] = dt1.Rows.Count + 1;
                        dr1["ReelNo"] = txtinv.Text.Trim();

                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "Select a.*,b.iname from (select trim(icode) as icode,kclreelno,sum(reelwin)-sum(reelwout) as tot from (select distinct trim(icode) as icode,kclreelno,sum(reelwin) as reelwin,0 as reelwout from reelvch where substr(type,1,1) in ('0','1') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode) union all select distinct trim(icode) as icode,kclreelno,0 as reelwin,sum(reelwout) as reelwout from reelvch where type in ('31','32') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode)) where trim(kclreelno)='" + txtinv.Text.Trim() + "' group by kclreelno,trim(icode)) a,item b where trim(a.icodE)=trim(b.icode) and a.tot>0");
                        if (dt.Rows.Count > 0)
                        {
                            dr1["icode"] = dt.Rows[0]["icode"].ToString().Trim();
                            dr1["qty"] = dt.Rows[0]["tot"].ToString().Trim();
                            dr1["iname"] = dt.Rows[0]["iname"].ToString();
                        }
                        dt1.Rows.Add(dr1);

                        ViewState["sg1"] = dt1;
                        fillGrid(dt1);
                    }
                }
                else
                {
                    dt1 = new DataTable();
                    DataRow dr1 = null;
                    dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                    dt1.Columns.Add(new DataColumn("ReelNo", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Icode", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Iname", typeof(string)));
                    dt1.Columns.Add(new DataColumn("qty", typeof(string)));
                    dr1 = dt1.NewRow();

                    dr1["SrNo"] = dt1.Rows.Count + 1;
                    dr1["ReelNo"] = txtinv.Text.Trim();

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "Select a.*,b.iname from (select trim(icode) as icode,kclreelno,sum(reelwin)-sum(reelwout) as tot from (select distinct trim(icode) as icode,kclreelno,sum(reelwin) as reelwin,0 as reelwout from reelvch where substr(type,1,1) in ('0','1') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode) union all select distinct trim(icode) as icode,kclreelno,0 as reelwin,sum(reelwout) as reelwout from reelvch where type in ('31','32') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode)) where trim(kclreelno)='" + txtinv.Text.Trim() + "' group by kclreelno,trim(icode)) a,item b where trim(a.icodE)=trim(b.icode) and a.tot>0");
                    if (dt.Rows.Count > 0)
                    {
                        dr1["icode"] = dt.Rows[0]["icode"].ToString().Trim();
                        dr1["qty"] = dt.Rows[0]["tot"].ToString().Trim();
                        dr1["iname"] = dt.Rows[0]["iname"].ToString();
                    }
                    dt1.Rows.Add(dr1);

                    ViewState["sg1"] = dt1;
                    fillGrid(dt1);
                }
                #endregion
                break;

            case "22055":
            case "AK17":
                #region Reel Entry
                if (ViewState["sg1"] != null)
                {
                    dt1 = new DataTable();
                    dt1 = (DataTable)ViewState["sg1"];
                    DataRow dr1 = null;
                    if (dt1.Rows.Count > 0)
                    {

                        dr1 = dt1.NewRow();
                        dr1["JobNo"] = hf1.Value;
                        dr1["JobDt"] = hf2.Value;
                        dr1["SrNo"] = dt1.Rows.Count + 1;
                        dr1["ReelNo"] = txtreel.Text.Trim();
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "Select a.*,b.iname from (select trim(icode) as icode,kclreelno,sum(reelwin)-sum(reelwout) as tot from (select distinct trim(icode) as icode,kclreelno,sum(reelwin) as reelwin,0 as reelwout from reelvch where substr(type,1,1) in ('0','1') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode) union all select distinct trim(icode) as icode,kclreelno,0 as reelwin,sum(reelwout) as reelwout from reelvch where type in ('31','32') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode)) where trim(kclreelno)='" + txtreel.Text.Trim() + "' group by kclreelno,trim(icode)) a,item b where trim(a.icodE)=trim(b.icode) and a.tot>0");
                        if (dt.Rows.Count > 0)
                        {
                            dr1["icode"] = dt.Rows[0]["icode"].ToString().Trim();
                            dr1["qty"] = dt.Rows[0]["tot"].ToString().Trim();
                            dr1["iname"] = dt.Rows[0]["iname"].ToString();
                            if (Convert.ToInt32(lblwtis.Text.Trim()) > 0) lblwtis.Text = (Convert.ToInt32(lblwtis.Text.Trim()) + Convert.ToInt32(dt.Rows[0]["tot"].ToString().Trim())).ToString();
                            else lblwtis.Text = dt.Rows[0]["tot"].ToString().Trim();
                        }
                        dt1.Rows.Add(dr1);
                        ViewState["sg1"] = dt1;
                        fillGrid(dt1);
                    }
                }
                else
                {
                    dt1 = new DataTable();
                    DataRow dr1 = null;
                    dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                    dt1.Columns.Add(new DataColumn("ReelNo", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Icode", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Qty", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Iname", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Jobno", typeof(string)));
                    dt1.Columns.Add(new DataColumn("JobDt", typeof(string)));
                    dr1 = dt1.NewRow();

                    dr1["JobNo"] = hf1.Value;
                    dr1["JobDt"] = hf2.Value;

                    dr1["SrNo"] = dt1.Rows.Count + 1;
                    dr1["ReelNo"] = txtreel.Text.Trim();

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "Select a.*,b.iname from (select trim(icode) as icode,kclreelno,sum(reelwin)-sum(reelwout) as tot from (select distinct trim(icode) as icode,kclreelno,sum(reelwin) as reelwin,0 as reelwout from reelvch where substr(type,1,1) in ('0','1') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode) union all select distinct trim(icode) as icode,kclreelno,0 as reelwin,sum(reelwout) as reelwout from reelvch where type in ('31','32') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode)) where trim(kclreelno)='" + txtreel.Text.Trim() + "' group by kclreelno,trim(icode)) a,item b where trim(a.icodE)=trim(b.icode) and a.tot>0");
                    if (dt.Rows.Count > 0)
                    {
                        dr1["icode"] = dt.Rows[0]["icode"].ToString().Trim();
                        dr1["qty"] = dt.Rows[0]["tot"].ToString().Trim();
                        dr1["iname"] = dt.Rows[0]["iname"].ToString();
                        lblwtis.Text = dt.Rows[0]["tot"].ToString().Trim();
                    }
                    dt1.Rows.Add(dr1);

                    ViewState["sg1"] = dt1;
                    fillGrid(dt1);
                }
                #endregion
                break;
            case "F15125":

                #region
                if (ViewState["sg1"] != null)
                {
                    dt1 = new DataTable();
                    dt1 = (DataTable)ViewState["sg1"];
                    DataRow dr1 = null;
                    if (dt1.Rows.Count > 0)
                    {
                        dr1 = dt1.NewRow();
                        dr1["SrNo"] = dt1.Rows.Count + 1;
                        dr1["FSTR"] = txtinv.Text.Trim();
                        dr1["ICODE"] = hf1.Value;

                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT B.INAME,A.IMIN,A.IMAX,A.IORD FROM ITEMBAL A, ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.ICODE='" + hf1.Value + "'");
                        if (dt.Rows.Count > 0)
                        {
                            dr1["INAME"] = dt.Rows[0]["INAME"].ToString().Trim();
                            dr1["IMIN"] = dt.Rows[0]["IMIN"].ToString();
                            dr1["IMAX"] = dt.Rows[0]["IMAX"].ToString().Trim();
                            dr1["IORD"] = dt.Rows[0]["IORD"].ToString();
                        }
                        dt1.Rows.Add(dr1);
                        ViewState["sg1"] = dt1;
                        fillGrid(dt1);
                    }
                }
                else
                {
                    dt1 = new DataTable();
                    DataRow dr1 = null;
                    dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                    dt1.Columns.Add(new DataColumn("FSTR", typeof(string)));
                    dt1.Columns.Add(new DataColumn("ICODE", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Iname", typeof(string)));
                    dt1.Columns.Add(new DataColumn("IMAX", typeof(string)));
                    dt1.Columns.Add(new DataColumn("IMIN", typeof(string)));
                    dt1.Columns.Add(new DataColumn("IORD", typeof(string)));
                    //dt1.Columns.Add(new DataColumn("JobDt", typeof(string)));
                    dr1 = dt1.NewRow();

                    dr1["SrNo"] = dt1.Rows.Count + 1;
                    dr1["FSTR"] = txtinv.Text.Trim();
                    dr1["ICODE"] = txtinv.Text.Trim().Substring(8, 8);

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT B.INAME,A.IMIN,A.IMAX,A.IORD FROM ITEMBAL A, ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.ICODE='" + hf1.Value.Substring(0, 8) + "'");
                    //dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT INAME,IMIN,IMAX,IORD FROM ITEM WHERE ICODE='" + hf1.Value.Substring(0, 8) + "'");
                    if (dt.Rows.Count > 0)
                    {
                        dr1["INAME"] = dt.Rows[0]["INAME"].ToString().Trim();
                        dr1["IMAX"] = dt.Rows[0]["IMAX"].ToString().Trim();
                        dr1["IMIN"] = dt.Rows[0]["IMIN"].ToString();
                        dr1["IORD"] = dt.Rows[0]["IORD"].ToString();
                    }
                    dt1.Rows.Add(dr1);

                    ViewState["sg1"] = dt1;
                    fillGrid(dt1);
                }
                #endregion

                break;

            case "F25124":
            case "F50125":
                if (ViewState["sg1"] != null)
                {
                    dt1 = new DataTable();
                    dt1 = (DataTable)ViewState["sg1"];
                    DataRow dr1 = null;
                    if (dt1.Rows.Count > 0)
                    {
                        dr1 = dt1.NewRow();
                        dr1["SrNo"] = dt1.Rows.Count + 1;
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "select a.branchcd||a.type||a.vchnum||TO_CHAR(a.vchdate,'DD/MM/YYYY')||TRIM(A.ICODE)||TRIM(a.kclreelno) AS FSTR,TRIM(a.icode) AS ICODE,TRIM(a.kclreelno) AS kclreelno,b.iname,a.reelwin as reelwin,TRIM(B.CPARTNO) AS CPARTNO from REELVCH a,item b WHERE trim(a.icode)=trim(b.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' AND a.VCHDATE " + DateRange + " AND a.KCLREELNO='" + hf1.Value.Trim() + "'");
                        if (dt.Rows.Count > 0)
                        {
                            dr1["FSTR"] = dt.Rows[0]["FSTR"].ToString().Trim();
                            dr1["ICODE"] = dt.Rows[0]["ICODE"].ToString().Trim();
                            dr1["INAME"] = dt.Rows[0]["INAME"].ToString().Trim();
                            dr1["CPARTNO"] = dt.Rows[0]["CPARTNO"].ToString();
                            dr1["KCLREELNO"] = dt.Rows[0]["KCLREELNO"].ToString().Trim();
                            dr1["QTY"] = dt.Rows[0]["reelwin"].ToString();
                        }
                        dt1.Rows.Add(dr1);
                        ViewState["sg1"] = dt1;
                        fillGrid(dt1);
                    }
                }
                else
                {
                    dt1 = new DataTable();
                    DataRow dr1 = null;
                    dt1.Columns.Add(new DataColumn("SrNo", typeof(string)));
                    dt1.Columns.Add(new DataColumn("FSTR", typeof(string)));
                    dt1.Columns.Add(new DataColumn("ICODE", typeof(string)));
                    dt1.Columns.Add(new DataColumn("Iname", typeof(string)));
                    dt1.Columns.Add(new DataColumn("CPARTNO", typeof(string)));
                    dt1.Columns.Add(new DataColumn("KCLREELNO", typeof(string)));
                    dt1.Columns.Add(new DataColumn("QTY", typeof(string)));
                    //dt1.Columns.Add(new DataColumn("JobDt", typeof(string)));
                    dr1 = dt1.NewRow();

                    dr1["SrNo"] = dt1.Rows.Count + 1;

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select a.branchcd||a.type||a.vchnum||TO_CHAR(a.vchdate,'DD/MM/YYYY')||TRIM(A.ICODE)||TRIM(a.kclreelno) AS FSTR,TRIM(a.icode) AS ICODE,TRIM(a.kclreelno) AS kclreelno,b.iname,a.reelwin as reelwin,TRIM(B.CPARTNO) AS CPARTNO from REELVCH a,item b WHERE trim(a.icode)=trim(b.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' AND a.VCHDATE " + DateRange + " AND a.KCLREELNO='" + hf1.Value.Trim() + "'");
                    if (dt.Rows.Count > 0)
                    {
                        dr1["FSTR"] = dt.Rows[0]["FSTR"].ToString().Trim();
                        dr1["ICODE"] = dt.Rows[0]["ICODE"].ToString().Trim();
                        dr1["INAME"] = dt.Rows[0]["INAME"].ToString().Trim();
                        dr1["CPARTNO"] = dt.Rows[0]["CPARTNO"].ToString();
                        dr1["KCLREELNO"] = dt.Rows[0]["KCLREELNO"].ToString().Trim();
                        dr1["QTY"] = dt.Rows[0]["reelwin"].ToString();
                    }
                    dt1.Rows.Add(dr1);

                    ViewState["sg1"] = dt1;
                    fillGrid(dt1);
                }
                break;
        }
    }
    protected void txtinv_TextChanged(object sender, EventArgs e)
    {
        dt = new DataTable();
        // HCID = Request.Cookies["rid"].Value.ToString();
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        try
        {
            switch (Prg_Id)
            {
                case "32011":
                case "F20125":
                    #region Gate Out Entry
                    if (txtinv.Text.Substring(2, 1) == "5")
                    {
                        if (txtinv.Text.Substring(2, 2) == "50") frm_vty = "4A";
                        if (txtinv.Text.Substring(2, 2) == "51") frm_vty = "4B";
                        if (txtinv.Text.Substring(2, 2) == "52") frm_vty = "4C";
                        if (txtinv.Text.Substring(2, 2) == "53") frm_vty = "4D";
                        if (txtinv.Text.Substring(2, 2) == "54") frm_vty = "4E";
                        if (txtinv.Text.Substring(2, 2) == "55") frm_vty = "4F";
                        if (txtinv.Text.Substring(2, 2) == "56") frm_vty = "4G";

                        if (txtinv.Text.Trim().Length > 18) value1 = txtinv.Text.Substring(0, 2) + frm_vty + txtinv.Text.Substring(4, 16);
                        else value1 = txtinv.Text.Substring(0, 2) + frm_vty + txtinv.Text.Substring(4, 14);
                    }
                    else value1 = txtinv.Text.Trim();

                    if (sg1.Rows.Count > 0)
                    {
                        for (int t = 0; t < sg1.Rows.Count; t++)
                        {
                            if (sg1.Rows[t].Cells[1].Text.Trim() == value1.Trim())
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Invoice Already Scanned and Exist in grid')", true);
                                //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Invoice Already Scanned and Exist in grid!!");
                                txtinv.Text = ""; txtinv.Focus(); break;
                            }
                        }
                    }

                    if (frm_cocd == "DLJM" || frm_cocd == "MEGA" || frm_cocd == "KUNS" || frm_cocd == "WPPL" || frm_cocd == "GTCF" || frm_cocd == "KPPL" || frm_cocd == "KESH" || frm_cocd == "MEGH" || frm_cocd == "UKB") cond = "trim(stage)||TRIM(IOPR)||trim(invno)||" + mq1 + "='" + value1 + "'";
                    else cond = "trim(stage)||TRIM(IOPR)||trim(invno)||" + mq1 + "='" + value1 + "'";
                    mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select distinct trim(stage)||TRIM(IOPR)||trim(invno)||to_char(invdate,'yyyymmdd') as fstr from ivoucherp where type='ZG' and vchdate " + DateRange + " and " + cond + "", "fstr");
                    if (mhd == "0")
                    {
                        if (frm_cocd == "DLJM" || frm_cocd == "MEGA" || frm_cocd == "KUNS" || frm_cocd == "WPPL" || frm_cocd == "GTCF" || frm_cocd == "KPPL" || frm_cocd == "KESH" || frm_cocd == "MEGH" || frm_cocd == "UKB") cond = "trim(branchcd)||TRIM(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + value1 + "'";
                        else cond = "trim(branchcd)||TRIM(type)||trim(vchnum)||to_char(vchdate,'yyyymmdd')='" + value1 + "'";
                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "Select distinct trim(branchcd)||TRIM(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr from ivoucher where " + cond + "", "fstr");
                        if (mhd != "0") add_R();
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Dear " + frm_uname + ", Selected Invoice Information is not Correct!!')", true);
                            //fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Selected Invoice Information is not Correct!!");
                            txtinv.Text = ""; txtinv.Focus();
                        }
                    }
                    else
                    {

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Dear " + frm_uname + ", Selected invoice is already dispatched!!')", true);
                        //fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Selected invoice is already dispatched!!");
                        txtinv.Text = ""; txtinv.Focus();
                    }
                    #endregion
                    break;
                case "22055":
                case "AK17":
                    #region Reel Entry
                    if (txtinv.Text.Trim().Length > 10)
                    {
                        hf1.Value = txtinv.Text.Substring(0, 6).Trim();
                        dt = new DataTable();
                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "Select replace(nvl(trim(enable_yN),'N'),'-','N') as cond from controls where id='M192'", "cond");
                        if (mhd == "Y")
                        {
                            hf2.Value = txtinv.Text.Substring(6, 10).Trim();
                            cond = "a.vchdate=to_date('" + hf2.Value.Trim() + "','dd/mm/yyyy')";
                        }
                        else
                        {
                            hf2.Value = txtinv.Text.Substring(6, 8).Trim();
                            cond = "a.vchdate=to_date('" + hf2.Value.Trim() + "','yyyymmdd')";
                        }
                        dt = fgen.getdata(frm_qstr, frm_cocd, "select a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,round(sum(A.col7),2) as qty,b.iname,a.col18,a.col19 from costestimate a,item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and upper(trim(a.status))<>'Y' and a.type='30' and nvl(A.app_by,'-')!='-' and a.vchnum='" + hf1.Value.Trim() + "' and " + cond + " group by a.vchnum,to_char(a.vchdate,'dd/mm/yyyy'),B.INAME,a.col18,a.col19");
                        if (dt.Rows.Count > 0)
                        {
                            dt2 = new DataTable();
                            dt2 = fgen.getdata(frm_qstr, frm_cocd, "select distinct a.vchnum,max(b.oprate1) as mxoprate1,max(b.oprate3) as mxoprate3,min(b.oprate1) as mnoprate1,min(b.oprate3) as mnoprate3 from costestimate a,item b where trim(a.col9)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and upper(trim(a.status))<>'Y' and a.type='30' and nvl(A.app_by,'-')!='-' and a.vchnum='" + hf1.Value.Trim() + "' and " + cond + " group by a.vchnum");
                            lblwtrq.Text = dt.Rows[0]["qty"].ToString(); lblwtis.Text = "0";
                            lbljobname.Text = dt.Rows[0]["iname"].ToString().Trim() + ", Length: " + dt.Rows[0]["col18"].ToString() + " Width: " + dt.Rows[0]["col19"].ToString() + "<br>" +
                                "MAX Size: " + dt2.Rows[0]["mxoprate1"].ToString() + ", MIN: " + dt2.Rows[0]["mnoprate1"].ToString() + "<br>" + "MAX GSM: " + dt2.Rows[0]["mxoprate3"].ToString() + ", MIN: " + dt2.Rows[0]["mnoprate3"].ToString();
                            txtinv.Text = "No." + hf1.Value + " Dt." + hf2.Value;
                            txtinv.ReadOnly = true; txtreel.Focus();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Not a valid Job Card!!')", true);
                            //fgen.msg("-", "AMSG", "Not a valid Job Card!!");
                            txtinv.Text = ""; txtinv.Focus();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Not a valid Job Card!!')", true);
                        //fgen.msg("-", "AMSG", "Not a valid Job Card!!");
                        txtinv.Text = ""; txtinv.Focus();
                    }
                    #endregion
                    break;
                case "22095":
                case "F25125":
                    #region Physical Verification Reel
                    if (txtinv.Text.Trim().Length > 1)
                    {
                        if (sg1.Rows.Count > 0)
                        {
                            for (int t = 0; t < sg1.Rows.Count; t++)
                            {
                                if (sg1.Rows[t].Cells[1].Text.Trim() == txtinv.Text.Trim())
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Reel Already Exist in grid')", true);
                                    //fgen.msg("-", "AMSG", "Reel Already Exist in grid!!");
                                    txtinv.Text = ""; txtinv.Focus();
                                    break;
                                }
                            }
                        }
                        SQuery = "Select * from (select trim(icode) as icode,kclreelno,sum(reelwin)-sum(reelwout) as tot from (select distinct trim(icode) as icode,kclreelno,sum(reelwin) as reelwin,0 as reelwout from reelvch where substr(type,1,1) in ('0','1') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode) union all select distinct trim(icode) as icode,kclreelno,0 as reelwin,sum(reelwout) as reelwout from reelvch where type in ('31','32') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode)) where trim(kclreelno)='" + txtinv.Text.Trim() + "' group by kclreelno,trim(icode)) where tot>0";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0) { add_R(); }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('No Reel Found!!')", true);
                        //fgen.msg("-", "AMSG", "No Reel Found!!");
                        txtinv.Text = "";
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Please Select Job Card First!!')", true);
                        // fgen.msg("-", "AMSG", "Please Select Job Card First!!");
                        txtreel.Text = ""; txtinv.Focus();
                    }
                    #endregion
                    break;
                case "F15125": // KANBAN FORm
                    #region
                    if (txtinv.Text.Trim().Length >= 14)
                    {
                        hf1.Value = txtinv.Text.Substring(8, 8).Trim();
                        if (sg1.Rows.Count > 0)
                        {
                            for (int t = 0; t < sg1.Rows.Count; t++)
                            {
                                if (sg1.Rows[t].Cells[1].Text.Trim() == txtinv.Text.Trim())
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Item Already Exist in grid')", true);
                                    //fgen.msg("-", "AMSG", "Reel Already Exist in grid!!");
                                    txtinv.Text = ""; txtinv.Focus();
                                    break;
                                }
                            }
                        }
                        //mhd = fgen.seek_iname(frm_qstr, frm_cocd, "select distinct trim(icode) as fstr from " + frm_tab_ivch + " where type='" + frm_vty + "' and vchdate " + DateRange + " and icode='" + hf1.Value.ToString() + "'", "fstr");
                        //if (mhd == "0")
                        //{
                        SQuery = "SELECT B.ICODE ,TRIM(B.INAME) AS INAME , A.IMAX,A.IMIN,A.IORD FROM ITEMBAL A,ITEM B WHERE TRIM(A.ICODe)=TRIM(B.ICODe) AND A.BRANCHCD='" + frm_mbr + "' AND TRIM(B.ICODE)='" + hf1.Value.Trim().ToUpper() + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        if (dt.Rows.Count > 0)
                        {
                            if (fgen.make_double(dt.Rows[0]["imin"].ToString()) == 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Min Value Is Zero Plz increase!!')", true);
                                txtinv.Text = ""; txtinv.Focus();
                                return;
                            }
                            //mq3 = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, hf1.Value.Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                            //if (fgen.make_double(dt.Rows[0]["IMIN"].ToString()) > fgen.make_double(mq3))
                            //{
                            //    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", "Item : " + hf1.Value.ToString().Trim() + "-" + dt.Rows[0]["iname"].ToString().Trim() + "'13' Stock Qty : " + mq3 + "'13' Please Check Your Closing Stock");
                            //    //string err_msg = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");
                            //    fgen.msg("-", "AMSG", "Item : " + hf1.Value.ToString().Trim() + "-" + dt.Rows[0]["iname"].ToString().Trim() + "'13' Stock Qty : " + mq3 + "'13' Please Check Your Closing Stock");
                            //    return;
                            //}

                            add_R();

                        }
                        else ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('No Item Found!!')", true);
                        txtinv.Text = ""; txtinv.Focus();
                        //}
                        //else
                        //{
                        //  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Dear " + frm_uname + ", Selected Item is already dispatched!!')", true);
                        //txtinv.Text = ""; txtinv.Focus();
                        //}
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Please Select Correct Item First!!')", true);
                        txtinv.Text = ""; txtinv.Focus();
                    }
                    #endregion
                    break;
                case "F25124":
                    #region
                    if (txtinv.Text.Trim().Length >= 8)
                    {
                        hf1.Value = txtinv.Text.Trim();
                        if (sg1.Rows.Count > 0)
                        {
                            for (int t = 0; t < sg1.Rows.Count; t++)
                            {
                                if (sg1.Rows[t].Cells[5].Text.Trim() == txtinv.Text.Trim())
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('reel already exist in grid')", true);
                                    //fgen.msg("-", "amsg", "reel already exist in grid!!");
                                    txtinv.Text = ""; txtinv.Focus();
                                    break;
                                }
                            }

                        }
                        SQuery = "select a.vchnum,a.vchdate,a.icode,a.kclreelno,b.iname,a.reelwin as reelwin from REELVCH a,item b WHERE trim(a.icode)=trim(b.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' AND a.VCHDATE " + DateRange + "  AND A.KCLREELNO='" + txtinv.Text.Trim() + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            add_R();
                            txtinv.Text = ""; reelloc.Focus();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('No Reel found!!')", true);
                            txtinv.Text = ""; txtinv.Focus();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Please Select Correct Reel First!!')", true);
                        txtinv.Text = ""; txtinv.Focus();
                    }

                    #endregion
                    break;
                case "F50125":
                    #region
                    if (txtinv.Text.Trim().Length >= 8)
                    {
                        hf1.Value = txtinv.Text.Trim();
                        if (sg1.Rows.Count > 0)
                        {
                            for (int t = 0; t < sg1.Rows.Count; t++)
                            {
                                if (sg1.Rows[t].Cells[5].Text.Trim() == txtinv.Text.Trim())
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Batch already exist in grid')", true);
                                    //fgen.msg("-", "amsg", "reel already exist in grid!!");
                                    txtinv.Text = ""; txtinv.Focus();
                                    break;
                                }
                            }

                        }
                        SQuery = "select a.vchnum,a.vchdate,a.icode,a.kclreelno,b.iname,a.reelwin as reelwin from REELVCH a,item b WHERE trim(a.icode)=trim(b.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '0%' AND a.VCHDATE " + DateRange + "  AND A.KCLREELNO='" + txtinv.Text.Trim() + "'";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            add_R();
                            txtinv.Text = ""; reelloc.Focus();
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('No Reel found!!')", true);
                            txtinv.Text = ""; txtinv.Focus();
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Please Select Correct Reel First!!')", true);
                        txtinv.Text = ""; txtinv.Focus();
                    }

                    #endregion
                    break;
            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Please Scan Again!!')", true);
            // fgen.msg("-", "AMSG", "Please Scan Again!!");
            txtinv.Text = ""; txtinv.Focus();
        }
    }
    protected void txtreel_TextChanged(object sender, EventArgs e)
    {
        dt = new DataTable();
        // HCID = Request.Cookies["rid"].Value.ToString();
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "32011":
            case "F20125":
                btnsave_ServerClick(sender, e);
                break;

            case "22055":
            case "AK17":
                if (txtinv.Text.Trim().Length > 1)
                {
                    if (sg1.Rows.Count > 0)
                    {
                        for (int t = 0; t < sg1.Rows.Count; t++)
                        {
                            if (sg1.Rows[t].Cells[1].Text.Trim() == txtreel.Text.Trim())
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Reel Already Exist in grid')", true);
                                //fgen.msg("-", "AMSG", "Reel Already Exist in grid");
                                txtreel.Text = ""; txtreel.Focus();
                                break;
                            }
                        }
                    }
                    SQuery = "Select * from (select trim(icode) as icode,kclreelno,sum(reelwin)-sum(reelwout) as tot from (select distinct trim(icode) as icode,kclreelno,sum(reelwin) as reelwin,0 as reelwout from reelvch where substr(type,1,1) in ('0','1') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode) union all select distinct trim(icode) as icode,kclreelno,0 as reelwin,sum(reelwout) as reelwout from reelvch where type in ('31','32') and branchcd='" + frm_mbr + "' group by kclreelno,trim(icode)) where trim(kclreelno)='" + txtreel.Text.Trim() + "' group by kclreelno,trim(icode)) where tot>0";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        if (frm_cocd == "TGIP")
                        //if (frm_cocd == "WPPL")
                        {
                            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "Select replace(nvl(trim(enable_yn),'N'),'-','N') as cond from controls where id='M192'", "cond");
                            if (mhd == "Y") cond = "and c.vchdate=to_date('" + hf2.Value.Trim() + "','dd/mm/yyyy')";
                            else cond = "and c.vchdate=to_date('" + hf2.Value.Trim() + "','yyyymmdd')";
                        }
                        dt = new DataTable();
                        SQuery = "select A.ciname as iname,A.icode,b.REELWIN,b.psize,b.gsm from item A,REELVCH B where TRIM(A.ICODE)=TRIM(B.ICODE) AND A.BRANCHCD='" + frm_mbr + "' and trim(b.kclreelno)='" + txtreel.Text.Trim() + "' and substr(b.type,1,1) in ('0','1')";
                        if (frm_cocd == "TGIP") SQuery = "select A.ciname as iname,A.icode,b.REELWIN,b.psize,b.gsm from item A,REELVCH B,costestimate c where TRIM(A.ICODE)=TRIM(B.ICODE) and trim(A.icode)=trim(c.col9) AND A.BRANCHCD='" + frm_mbr + "' and trim(b.kclreelno)='" + txtreel.Text.Trim() + "' and substr(b.type,1,1) in ('0','1') and c.vchnum='" + hf1.Value.Trim() + "' " + cond + "";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0) { add_R(); }
                        else ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('No Reel Found!!')", true);
                        //fgen.msg("-", "AMSG", "No Reel Found!!");
                    }
                    else ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('No Reel Found!!')", true);
                    //fgen.msg("-", "AMSG", "No Reel Found!!");
                    txtreel.Text = "";
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Please Select Job Card First!!')", true);
                    //fgen.msg("-", "AMSG", "Please Select Job Card First");
                    txtreel.Text = ""; txtinv.Focus();
                }
                dt.Dispose();
                break;
            case "F25124":

                break;
        }
        txtreel.Focus();
    }
    protected void btncan_Click(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl(); ddreelloc.Items.Clear();
    }
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    void fillGrid(DataTable dataFromFill)
    {
        DataTable neWDt = dataFromFill.Copy();
        ViewState["sg1"] = neWDt;
        makeColNameAsMine(dataFromFill);
        sg1.DataSource = dataFromFill;
        sg1.DataBind();
        hideAndRenameCol();
    }
    void makeColNameAsMine(DataTable dtColNameTable)
    {
        int colFound = dtColNameTable.Columns.Count;
        int colSrno = 1;
        for (int i = 0; i < 7; i++)
        {
            if (colFound > i) dtColNameTable.Columns[i].ColumnName = "sg1_f" + colSrno;
            else dtColNameTable.Columns.Add("sg1_f" + colSrno, typeof(string));
            colSrno++;
        }
        if (colFound > 7) dtColNameTable.Columns[7].ColumnName = "sg1_tk1";
        else dtColNameTable.Columns.Add("sg1_tk1", typeof(string));
    }
    void hideAndRenameCol()
    {
        DataTable dtColNameTab = (DataTable)ViewState["sg1"];
        int colFound = dtColNameTab.Columns.Count;
        int totResrvCol = 0;
        int x = 0;
        x = fgen.make_int(Session["hfWindowSize"].ToString());
        if (x == 0) x = 1000;
        double totWidth = 0;
        int widthMake = 0;
        for (int i = 0; i <= 7; i++)
        {
            if (colFound > i)
            {
                sg1.HeaderRow.Cells[i].Text = dtColNameTab.Columns[i - totResrvCol].ColumnName;
                if (sg1.Rows.Count > 0)
                {
                    widthMake = (sg1.Rows[0].Cells[i].Text.Length) * 10;
                    if (widthMake < 50) widthMake = 50;
                    if (widthMake > 200) widthMake = 200;
                    totWidth += Convert.ToDouble(widthMake);
                    sg1.Columns[i].HeaderStyle.Width = widthMake;
                    if (sg1.Columns[i].HeaderStyle.CssClass == "hidden")
                    {
                        if (i > 2)
                        {
                            sg1.Columns[i].HeaderStyle.CssClass = "";
                            for (int k = 0; k < sg1.Rows.Count; k++)
                            {
                                sg1.Rows[k].Cells[i].CssClass = "";
                            }
                        }
                    }
                }
            }
            else
            {
                sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                for (int k = 0; k < sg1.Rows.Count; k++)
                {
                    sg1.Rows[k].Cells[i].CssClass = "hidden";
                }
            }
        }
        if (totWidth < 1200 && x > 600)
        {
            totWidth = 0;
            double gridFoundWidth = Math.Round(x * .95);
            string sepWidth = Math.Round(gridFoundWidth / (colFound - 0)).ToString();
            for (int i = 0; i < colFound; i++)
            {
                if (Convert.ToInt16(sepWidth) < 50) sepWidth = "100";
                //sg1.Columns[i].HeaderStyle.Width = Convert.ToInt16(sepWidth);
                //totWidth += Convert.ToDouble(sepWidth);
            }
        }
        if (frm_formID == "F25125")
        {
            for (int i = 0; i < sg1.Rows.Count; i++)
            {
                sg1.Columns[7].HeaderStyle.CssClass = "GridviewScrollHeader2";
                sg1.Rows[i].Cells[7].CssClass = "GridviewScrollItem2";
            }
        }
        if (frm_formID == "F20125")
        {
            for (int i = 0; i < sg1.Rows.Count; i++)
            {
                ((TextBox)sg1.Rows[i].FindControl("sg1_tk1")).ReadOnly = true;
            }
        }
    }
}