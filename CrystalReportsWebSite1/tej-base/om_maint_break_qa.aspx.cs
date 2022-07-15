using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_maint_break_qa : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4;
    DataRow oporow; DataSet oDS;
    int i = 0, z = 0;

    DataTable dtCol = new DataTable();
    string Checked_ok, mq0 = "", mq1 = "", mq2 = "", mq3 = "", mq4 = "", mq5, mq6, mq7, mq8, mq9;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName, typePopup = "Y";
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
                doc_addl.Value = "-";
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false;
        btnlbl4.Enabled = false; btnlbl7.Enabled = false;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlbl4.Enabled = true; btnlbl7.Enabled = true;
        btnprint.Disabled = true; btnlist.Disabled = true;
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
        frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        Prg_Id = frm_formID;
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        frm_tabname = "WB_MAINT";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        btnprint.Visible = false;

        switch (frm_formID)
        {
            case "F75152":// MOULD BREAKDOWN
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MM06");
                frm_vty = "MM06";
                lbl2.Text = "Form to Record Mould Breakdown.";
                break;

            case "F75181":// QUALITY APPROVAL OK1
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MM08");
                frm_vty = "MM08";
                // lbl2.Text = "Form to Record Post Break down- Quality Approval OK1";
                break;

            case "F75182":// QUALITY APPROVAL OK2
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MM09");
                frm_vty = "MM09";
                // lbl2.Text = "Form to Record Post Break down Quality Approval OK2 ";
                break;
        }
        switch (frm_formID)
        {
            case "F75152":
                SQuery = "select trim(a.vchnum) as Entry_no, to_char(a.vchdate,'dd/mm/yyyy') as Entry_date,b.name as Mould_name,a.col1 as code,a.cpartno as Mould_code,a.btchno as Mc_code,a.title as mc_name,to_char(a.date1,'dd/mm/yyyy') as breakdown_date,a.col12 as beakdown_time,a.remarks,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,typegrp b where trim(a.col1)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and b.id='MM' and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and rownum<50 ORDER BY vdd desc,a.VCHNUM desc";
                break;

            case "F75181":
            case "F75182":
                SQuery = "select trim(a.vchnum) as Entry_no, to_char(a.vchdate,'dd/mm/yyyy') as Entry_date,b.name as mould_name,a.col1 as code,a.cpartno as mould_code,to_char(a.date1,'dd/mm/yyyy') as dated,a.col12 as time,a.col2 as username,a.num1 as no_of_samples,(case when nvl(a.result,'-')='Y' then 'ACCEPT' else 'REJECT' end) as result,a.remarks,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,typegrp b where trim(a.col1)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and b.id='MM' and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and rownum<50 ORDER BY vdd desc,a.VCHNUM desc";
                break;
        }
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg1.DataSource = dt;
        sg1.DataBind();
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
                if (frm_formID == "F75152")
                {
                    //LAST WORKING QUERY SQuery = "SELECT TRIM(TYPE1) as fstr , acref as mould_code, name as mould,type1 as code FROM TYPEGRP WHERE branchcd='" + frm_mbr + "' and ID='MM' ORDER BY Name";
                    // LAST WORKING QUERY COMMENTED ON 06/02/2019  SQuery = "SELECT TRIM(A.COL1) as fstr,A.CPARTNO as mould_code,B.name as mould,A.COL1 as code FROM  WB_MASTER A,TYPEGRP B WHERE TRIM(A.BRANCHCD)||TRIM(A.COL1)=TRIM(B.BRANCHCD)||TRIM(B.TYPE1) AND A.branchcd='" + frm_mbr + "' AND A.ID='MM01' and B.ID='MM' AND NVL(COL2,'-')!='Y' ORDER BY mould";
                    SQuery = "SELECT TRIM(A.COL1) as fstr,B.ACREF as mould_code,B.name as mould,TRIM(A.COL1) as code FROM (SELECT BRANCHCD,TRIM(COL1) AS COL1,1 AS QTY FROM WB_MASTER WHERE BRANCHCD='" + frm_mbr + "' AND ID='MM01' AND NVL(TRIM(COL2),'-')!='Y' UNION ALL SELECT BRANCHCD,TRIM(COL1) AS COL1,-1 AS QTY FROM (SELECT DISTINCT BRANCHCD,TRIM(COL1) AS COL1,1 AS QTY FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='MM06' UNION ALL SELECT DISTINCT BRANCHCD,TRIM(COL1) AS COL1,-1 AS QTY FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='MM09' AND NVL(TRIM(RESULT),'-')='Y') GROUP BY BRANCHCD,COL1 HAVING SUM(QTY)>0)A,TYPEGRP B WHERE TRIM(A.BRANCHCD)||TRIM(A.COL1)=TRIM(B.BRANCHCD)||TRIM(B.TYPE1) AND A.branchcd='" + frm_mbr + "' and B.ID='MM' GROUP BY  TRIM(A.COL1),B.ACREF,B.name HAVING SUM(QTY)>0 ORDER BY MOULD";
                    //SQuery = "SELECT TRIM(TYPE1) as fstr , acref as mould_id, name as mould,type1 as code,'-' as entry FROM TYPEGRP WHERE branchcd='" + frm_mbr + "' and ID='MM' ORDER BY Name";
                }
                else if (frm_formID == "F75181")
                {
                    // original SQuery = "select trim(a.col1) as fstr,b.acref as mould_id,b.name as mould ,trim(a.col1) as code from (SELECT distinct trim(col1) as col1,1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM07' and vchdate " + DateRange + " union all select distinct trim(col1) as col1,-1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM08' and vchdate " + DateRange + ")a,typegrp b where trim(a.col1)=trim(b.type1) and b.branchcd='" + frm_mbr + "' and b.id='MM' group by trim(a.col1),b.acref,b.name having sum(qty)>0";
                    SQuery = "select trim(a.entry)||trim(a.col1) as fstr,b.acref as mould_id,b.name as mould ,trim(a.col1) as code,  trim(a.entry) as entry_details from (SELECT distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as entry ,trim(col1) as col1,1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM07' union all select distinct trim(col11) as entry,trim(col1) as col1,-1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM08' and upper(trim(nvl(result,'-')))='Y')a,typegrp b where trim(a.col1)=trim(b.type1) and b.branchcd='" + frm_mbr + "' and b.id='MM' group by trim(a.entry),trim(a.col1),b.acref,b.name, trim(a.entry) having sum(qty)>0";
                }
                else if (frm_formID == "F75182")
                {
                    SQuery = "select trim(a.entry)||trim(a.col1) as fstr,b.acref as mould_id,b.name as mould ,trim(a.col1) as code , trim(a.entry) as entry_details from (SELECT distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as entry,trim(col1) as col1,1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM08' and trim(upper(nvl(result,'-')))='Y' union all select distinct trim(col11) as entry,trim(col1) as col1,-1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM09' and upper(trim(nvl(result,'-')))='Y')a,typegrp b where trim(a.col1)=trim(b.type1) and b.branchcd='" + frm_mbr + "' and b.id='MM' group by trim(a.entry),trim(a.col1),b.acref,b.name, trim(a.entry) having sum(qty)>0";
                }
                break;

            case "TICODE":
                SQuery = "select trim(mchcode) as fstr, Mchname ,Spec1 as Specification,Mchcode from Pmaint where branchcd='" + frm_mbr + "' and type='10' order by Mchname";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                {
                    if (frm_formID == "F75152")
                    {
                        SQuery = "select distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,trim(a.vchnum) as Entry_no,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,b.acref as Mould_code,b.name as Mould_name,a.col1 as mould_srn,a.type,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,typegrp b where trim(a.col1)=trim(b.type1) AND TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND B.ID='MM' and a.branchcd='" + frm_mbr + "' and a.type ='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,trim(a.vchnum) desc";
                    }
                    else if (frm_formID == "F75181" || frm_formID == "F75182")
                    {
                        SQuery = "select distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,trim(a.vchnum) as Entry_no,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Date,b.acref as Mould_code,b.name as Mould_name,a.col1 as mould_srn,(case when nvl(a.result,'-')='Y' then 'QA:OK' else 'QA:NG' end) as result,a.type,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,typegrp b where trim(a.col1)=trim(b.type1) AND TRIM(A.BRANCHCD)=TRIM(B.BRANCHCD) AND B.ID='MM' and a.branchcd='" + frm_mbr + "' and a.type ='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,trim(a.vchnum) desc";
                    }
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
            if (typePopup == "N") newCase(frm_vty);
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Edit", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        string mandField = "";
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

        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }
        if (txtrmk.Text.Length <= 1)
        {
            fgen.msg("-", "AMSG", "Please Fill Remarks related to Break Down");
            return;
        }

        string last_entdt = "";
        last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
        if (Convert.ToDateTime(txtlbl2.Text.ToString()) > Convert.ToDateTime(last_entdt))
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Breakdown Date " + Convert.ToDateTime(txtlbl2.Text).ToString("dd/MM/yyyy") + " ,Please Check !!");
            txtvchdate.Text = last_entdt;
            txtlbl2.Text = last_entdt;
            txtvchdate.Focus();
            return;
        }

        if (Convert.ToDateTime(txtlbl2.Text.Trim()) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtlbl2.Text.Trim()) > Convert.ToDateTime(todt))
        {
            fgen.msg("-", "AMSG", "Date outside " + fromdt + " to " + todt + " is Not Allowed!!'13'Fill date for This Year Only");
            txtlbl7.Focus();
            return;
        }


        if (frm_formID == "F75181")
        {
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select max(date1) as last_date from wb_maint  where branchcd='" + frm_mbr + "' and type='MM07' and col1='" + txtlbl4.Text.Trim() + "'", "last_date");
            if (Convert.ToDateTime(Convert.ToDateTime(txtlbl2.Text).ToString("dd/MM/yyyy")) < Convert.ToDateTime(col1))
            {
                fgen.msg("-", "AMSG", "Date Should Be More Than Or '13' Equal to Last OK Record - '" + Convert.ToDateTime(col1).ToString("dd/MM/yyyy") + "'"); txtlbl2.Focus(); return;
            }
        }
        if (frm_formID == "F75182")
        {
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select max(date1) as last_date from wb_maint  where branchcd='" + frm_mbr + "' and type='MM08' and col1='" + txtlbl4.Text.Trim() + "'", "last_date");
            if (Convert.ToDateTime(Convert.ToDateTime(txtlbl2.Text).ToString("dd/MM/yyyy")) < Convert.ToDateTime(col1))
            {
                fgen.msg("-", "AMSG", "Date Should Be More Than Or '13' Equal to Last Quality Approval OK1 - '" + Convert.ToDateTime(col1).ToString("dd/MM/yyyy") + "'"); txtlbl2.Focus(); return;
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
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("Select DateRange", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Month", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F75152":
                vty = "MM06";
                frm_vty = vty;
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
                lbl1a.Text = vty;
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'", 6, "VCH");
                txtvchnum.Text = frm_vnum;
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                break;

            case "F75181":
                vty = "MM08";
                frm_vty = vty;
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
                lbl1a.Text = vty;
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'", 6, "VCH");
                txtvchnum.Text = frm_vnum;
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                break;

            case "F75182":
                vty = "MM09";
                frm_vty = vty;
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
                lbl1a.Text = vty;
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'", 6, "VCH");
                txtvchnum.Text = frm_vnum;
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                break;
        }
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();
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
            if (CP_BTN.Trim().Substring(0, 3) == "BTN" || CP_BTN.Trim().Substring(0, 3) == "SG1" || CP_BTN.Trim().Substring(0, 3) == "SG2" || CP_BTN.Trim().Substring(0, 3) == "SG3")
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
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty.Substring(2, 2), lblheader.Text.Trim() + " Type =" + frm_vty + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                clearctrl(); fgen.ResetForm(this.Controls); set_Val(); // SET_VAL IS CALLED HERE SO THAT GRID SHOWS ENTERIES AFTER DELETION
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
                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");

                    disablectrl();
                    fgen.EnableForm(this.Controls);

                    // Popup asking for Copy from Older Data
                    fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
                    hffield.Value = "NEW_E";
                    break;
                    #endregion

                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    edmode.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    //hffield.Value = "Del_E";
                    //make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);                    
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

                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Edit", frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                    if (col1 == "") return;
                    clearctrl();
                    dt = new DataTable();
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    switch (Prg_Id)
                    {
                        case "F75152":
                            SQuery = "select a.vchnum as vchnum,a.vchdate as vchdate,a.cpartno,a.col1 as col1,a.btchno,a.title,a.date1 as date1,a.col12 as col12,b.name as mould_name,a.ent_by as ent_by ,a.ent_dt as ent_dt,a.acode,a.remarks from " + frm_tabname + " a,typegrp b where trim(a.col1)=trim(b.TYPE1) and trim(a.branchcd)=trim(b.branchcd) and a.branchcd='" + frm_mbr + "' and b.id='MM' and a.type='" + frm_vty + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "'";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                            ViewState["fstr"] = col1;
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                                txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtlbl4.Text = dt.Rows[0]["col1"].ToString().Trim();
                                txtlbl4a.Text = dt.Rows[0]["mould_name"].ToString().Trim();
                                txtlbl9.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                                txtlbl7.Text = dt.Rows[0]["btchno"].ToString().Trim();
                                txtlbl7a.Text = dt.Rows[0]["title"].ToString().Trim();
                                txtlbl2.Text = Convert.ToDateTime(dt.Rows[0]["date1"].ToString().Trim()).ToString("yyyy-MM-dd");
                                txtlbl3.Text = dt.Rows[0]["col12"].ToString().Trim();
                                txtrmk.Text = dt.Rows[0]["remarks"].ToString().Trim();
                            }
                            break;

                        case "F75181":
                        case "F75182":
                            SQuery = "select a.*,b.name as mould_name from " + frm_tabname + " a,typegrp b where trim(a.col1)=trim(b.TYPE1) and trim(a.branchcd)=trim(b.branchcd) and a.branchcd='" + frm_mbr + "' and b.id='MM' and a.type='" + frm_vty + "' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "'";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                            ViewState["fstr"] = col1;
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                                txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtlbl8.Text = dt.Rows[0]["COL2"].ToString().Trim();
                                txtlbl4.Text = dt.Rows[0]["COL1"].ToString().Trim();
                                txtlbl4a.Text = dt.Rows[0]["mould_name"].ToString().Trim();
                                txtlbl2.Text = Convert.ToDateTime(dt.Rows[0]["date1"].ToString().Trim()).ToString("yyyy-MM-dd");
                                txtlbl3.Text = dt.Rows[0]["col12"].ToString().Trim();
                                txtrmk.Text = dt.Rows[0]["remarks"].ToString().Trim();
                                txtlbl5.Text = dt.Rows[0]["NUM1"].ToString().Trim();
                                txtlbl9.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                                doc_addl.Value = dt.Rows[0]["col11"].ToString().Trim();
                                txtResult.Text = dt.Rows[0]["result"].ToString().Trim();
                            }
                            break;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dt.Dispose();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    break;

                case "Print_E":

                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    if (frm_formID == "F75181")
                    {
                        dt = new DataTable();
                        SQuery = "select trim(a.col1) as code,b.name as mould ,b.acref as mould_id,trim(a.entry) as entry from (SELECT distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as entry ,trim(col1) as col1,1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM07' union all select distinct trim(col11) as entry,trim(col1) as col1,-1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM08' and nvl(result,'-')!='Y')a,typegrp b where trim(a.col1)=trim(b.type1) and b.branchcd='" + frm_mbr + "' and b.id='MM' and trim(a.entry)||trim(a.col1)='" + col1 + "' group by trim(a.entry),trim(a.col1),b.acref,b.name having sum(qty)>0";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtlbl4.Text = dt.Rows[0]["code"].ToString().Trim();
                            txtlbl4a.Text = dt.Rows[0]["mould"].ToString().Trim();
                            txtlbl9.Text = dt.Rows[0]["mould_id"].ToString().Trim();
                            doc_addl.Value = dt.Rows[0]["entry"].ToString().Trim();
                        }
                    }
                    else if (frm_formID == "F75182")
                    {
                        dt = new DataTable();
                        SQuery = "select trim(a.entry) as entry,b.acref as mould_id,b.name as mould ,trim(a.col1) as code from (SELECT distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as entry,trim(col1) as col1,1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM08' and trim(nvl(result,'-'))='Y' union all select distinct trim(col11) as entry,trim(col1) as col1,-1 as qty from wb_maint where branchcd='" + frm_mbr + "' and type='MM09' and trim(nvl(result,'-'))='Y')a,typegrp b where trim(a.col1)=trim(b.type1) and b.branchcd='" + frm_mbr + "' and b.id='MM' and trim(a.entry)||trim(a.col1)='" + col1 + "' group by trim(a.entry),trim(a.col1),b.acref,b.name having sum(qty)>0";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtlbl4.Text = dt.Rows[0]["code"].ToString().Trim();
                            txtlbl4a.Text = dt.Rows[0]["mould"].ToString().Trim();
                            txtlbl9.Text = dt.Rows[0]["mould_id"].ToString().Trim();
                            doc_addl.Value = dt.Rows[0]["entry"].ToString().Trim();
                        }
                    }
                    else if (frm_formID == "F75152")
                    {
                        //dt = new DataTable();
                        //SQuery = "SELECT TRIM(TYPE1) as fstr , acref as mould_id, name as mould,type1 as code,'-' as entry FROM TYPEGRP WHERE branchcd='" + frm_mbr + "' and ID='MM' ORDER BY Name";
                        //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        //if (dt.Rows.Count > 0)
                        //{
                        txtlbl4.Text = col1; //dt.Rows[0]["code"].ToString().Trim();
                        txtlbl4a.Text = col3;//dt.Rows[0]["mould"].ToString().Trim();
                        txtlbl9.Text = col2;// dt.Rows[0]["mould_id"].ToString().Trim();
                        // doc_addl.Value = dt.Rows[0]["entry"].ToString().Trim();
                        btnlbl7.Focus();
                        //}
                    }
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

                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    txtlbl2.Focus();
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
        todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        if (hffield.Value == "List")
        {
            switch (frm_formID)
            {
                case "F75152":
                    SQuery = "select trim(a.vchnum) as voucher_no, to_char(a.vchdate,'dd/mm/yyyy') as voucher_date,b.name as mould_name,a.col1 as code,a.cpartno as mould_code,to_char(a.date1,'dd/mm/yyyy') as breakdown_date,a.col12 as beakdown_time,a.btchno as machine_code,a.title as machine_name,a.remarks,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,typegrp b where trim(a.col1)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and b.id='MM' and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + " ORDER BY vdd desc,a.VCHNUM desc";
                    break;

                case "F75181":
                case "F75182":
                    SQuery = "select trim(a.vchnum) as voucher_no, to_char(a.vchdate,'dd/mm/yyyy') as voucher_date,b.name as mould_name,a.col1 as code,a.cpartno as mould_code,to_char(a.date1,'dd/mm/yyyy') as dated,a.col12 as time,a.col2 as username,a.num1 as no_of_samples,(case when nvl(a.result,'-')='Y' then 'ACCEPT' else 'REJECT' end) as result,a.remarks,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,typegrp b where trim(a.col1)=trim(b.type1) and trim(a.branchcd)=trim(b.branchcd) and b.id='MM' and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + " ORDER BY vdd desc,a.VCHNUM desc";
                    break;
            }
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " For The Period " + fromdt + " To " + todt, frm_qstr);
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
                        Checked_ok = "N"; btnsave.Disabled = false;
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                    }
                }
            }

            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N"; btnsave.Disabled = false;
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
                            fgen.msg("-", "AMSG", lblheader.Text + " " + frm_vnum + " Updated Successfully");
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
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); setColHeadings(); set_Val();
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
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {

        string last_entdt = "";
        last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
        if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
        {

            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            txtvchdate.Text = last_entdt;
            txtlbl2.Text = last_entdt;
            txtvchdate.Focus();
            return;
        }

        if (Convert.ToDateTime(txtvchdate.Text.Trim()) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text.Trim()) > Convert.ToDateTime(todt))
        {
            fgen.msg("-", "AMSG", "Date outside " + fromdt + " to " + todt + " is Not Allowed!!'13'Fill date for This Year Only");
            txtlbl7.Focus();
            return;
        }


        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);
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
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        switch (Prg_Id)
        {
            case "F75152":
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum.Trim().ToUpper();
                oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
                oporow["icode"] = "-";
                oporow["acode"] = "-";
                oporow["COL1"] = txtlbl4.Text.Trim().ToUpper();
                oporow["col2"] = "-";
                oporow["COL3"] = "-";
                oporow["COL4"] = "-";
                oporow["COL5"] = "-";
                oporow["COL6"] = "-";
                oporow["COL7"] = "-";
                oporow["COL8"] = "-";
                oporow["COL9"] = "-";
                oporow["COL10"] = "-";
                oporow["DATE1"] = Convert.ToDateTime(txtlbl2.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
                oporow["col12"] = txtlbl3.Text.Trim().ToUpper();
                oporow["COL13"] = "-";
                oporow["col14"] = "-";
                oporow["col15"] = "-";
                oporow["btchno"] = txtlbl7.Text.Trim().ToUpper();
                oporow["title"] = txtlbl7a.Text.Trim().ToUpper();
                if (txtrmk.Text.Trim().Length > 300)
                {
                    oporow["REMARKS"] = txtrmk.Text.Trim().ToUpper().Substring(0, 299);
                }
                else
                {
                    oporow["REMARKS"] = txtrmk.Text.Trim().ToUpper();
                }
                oporow["col11"] = vardate;
                oporow["DATE2"] = vardate;
                oporow["RESULT"] = "-";
                oporow["CPARTNO"] = txtlbl9.Text.Trim().ToUpper();
                oporow["GRADE"] = "-";
                oporow["SRNO"] = i + 1;
                oporow["OBSV1"] = "-";
                oporow["OBSV2"] = "-";
                oporow["OBSV3"] = "-";
                oporow["OBSV4"] = "-";
                oporow["OBSV5"] = "-";
                oporow["OBSV6"] = "-";
                oporow["OBSV7"] = "-";
                oporow["OBSV8"] = "-";
                oporow["OBSV9"] = "-";
                oporow["OBSV10"] = "-";
                oporow["OBSV11"] = "-";
                oporow["OBSV12"] = "-";
                oporow["OBSV13"] = "-";
                oporow["obsv14"] = "-";
                oporow["obsv15"] = "-";
                oporow["NUM1"] = 0;
                oporow["NUM2"] = 0;
                oporow["NUM3"] = 0;
                oporow["NUM4"] = 0;
                oporow["NUM5"] = 0;
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
                oDS.Tables[0].Rows.Add(oporow);
                break;

            case "F75181":
            case "F75182":
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum.Trim().ToUpper();
                oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
                oporow["icode"] = "-";
                oporow["acode"] = "-";
                oporow["COL1"] = txtlbl4.Text.Trim().ToUpper();
                oporow["col2"] = txtlbl8.Text.Trim().ToUpper();
                oporow["COL3"] = "-";
                oporow["COL4"] = "-";
                oporow["COL5"] = "-";
                oporow["COL6"] = "-";
                oporow["COL7"] = "-";
                oporow["COL8"] = "-";
                oporow["COL9"] = "-";
                oporow["COL10"] = "-";
                oporow["date1"] = Convert.ToDateTime(txtlbl2.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
                // oporow["col11"] = doc_addl.Value;
                oporow["col12"] = txtlbl3.Text.Trim().ToUpper();
                //oporow["COL13"] = lstvch1.Value.Trim();
                oporow["col14"] = "-";
                oporow["col15"] = "-";
                oporow["btchno"] = "-";
                oporow["title"] = "-";
                oporow["cpartno"] = txtlbl9.Text.Trim();

                if (frm_formID == "F75181")//ok1 prod
                {
                    oporow["col11"] = doc_addl.Value;
                    if (edmode.Value == "Y")
                    {
                        oporow["COL13"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(COL13) AS COL13 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='MM08' AND COL1='" + txtlbl4.Text.Trim() + "' AND trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_vnum.Trim() + txtvchdate.Text.Trim() + "'", "COL13");
                    }
                    else
                    {
                        mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(COL11) AS COL11 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='MM07' AND COL1='" + txtlbl4.Text.Trim() + "' AND trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + doc_addl.Value + "'", "COL11");
                        oporow["COL13"] = mq0;
                    }
                }
                else
                {
                    oporow["col11"] = doc_addl.Value;
                    if (edmode.Value == "Y")
                    {
                        mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(COL13) AS COL13 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='MM09' AND COL1='" + txtlbl4.Text.Trim() + "' AND trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_vnum.Trim() + txtvchdate.Text.Trim() + "'", "COL13");// ok vchnum/vchdate
                        oporow["COL13"] = mq1;
                        mq2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(COL14) AS COL14 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='MM09' AND COL1='" + txtlbl4.Text.Trim() + "' AND trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_vnum.Trim() + txtvchdate.Text.Trim() + "'", "COL14");//breakdown  vchnum vchdate
                        oporow["COL14"] = mq2;
                    }
                    else
                    {
                        mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(COL11) AS COL11 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='MM08' AND COL1='" + txtlbl4.Text.Trim() + "' AND trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + doc_addl.Value + "'", "COL11");// ok vchnum/vchdate
                        oporow["COL13"] = mq1;
                        mq2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(COL13) AS COL13 FROM WB_MAINT WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='MM08' AND COL1='" + txtlbl4.Text.Trim() + "' AND trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + doc_addl.Value + "'", "COL13");//breakdown  vchnum vchdate
                        oporow["COL14"] = mq2;
                    }
                }

                if (txtrmk.Text.Trim().Length > 300)
                {
                    oporow["REMARKS"] = txtrmk.Text.Trim().ToUpper().Substring(0, 299);
                }
                else
                {
                    oporow["REMARKS"] = txtrmk.Text.Trim().ToUpper();
                }

                oporow["DATE2"] = vardate;
                if (txtResult.Text.Trim().ToUpper() != "Y")
                {
                    oporow["RESULT"] = "N";
                }
                else
                {
                    oporow["RESULT"] = txtResult.Text.Trim().ToUpper();
                }
                oporow["GRADE"] = "-";
                oporow["SRNO"] = i + 1;
                oporow["OBSV1"] = "-";
                oporow["OBSV2"] = "-";
                oporow["OBSV3"] = "-";
                oporow["OBSV4"] = "-";
                oporow["OBSV5"] = "-";
                oporow["OBSV6"] = "-";
                oporow["OBSV7"] = "-";
                oporow["OBSV8"] = "-";
                oporow["OBSV9"] = "-";
                oporow["OBSV10"] = "-";
                oporow["OBSV11"] = "-";
                oporow["OBSV12"] = "-";
                oporow["OBSV13"] = "-";
                oporow["obsv14"] = "-";
                oporow["obsv15"] = "-";
                oporow["NUM1"] = fgen.make_double(txtlbl5.Text.Trim());
                oporow["NUM2"] = 0;
                oporow["NUM3"] = 0;
                oporow["NUM4"] = 0;
                oporow["NUM5"] = 0;
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
                oDS.Tables[0].Rows.Add(oporow);
                break;
        }
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F75152":
                frm_vty = "MM06";
                break;

            case "F75181":
                frm_vty = "MM08";
                break;

            case "F75182":
                frm_vty = "MM09";
                break;
        }
    }
    //------------------------------------------------------------------------------------
}