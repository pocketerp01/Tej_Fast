using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class om_vch_upload : System.Web.UI.Page
{
    string btnval, SQuery, uname, col1, col2, col3, cstr, vchnum, vardate, fromdt, todt, year, cond = "", vip = "", mq0 = "";
    string pk_error = "Y", chk_rights = "N", DateRange;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_tabname, frm_myear, frm_sql, frm_ulvl, frm_formID, frm_UserID;
    string co_cd, ulvl, merr = "0", HCID, VCH_STYLE = "N", frm_cDt1 = "", frm_cDt2 = "";
    DataTable dt, dt1; DataRow dr1;
    string fileNme = "";
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
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

                    if (frm_formID == "F70371") VCH_STYLE = "Y";
                    else VCH_STYLE = "N";
                    chk_f();
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl(); btnnew.Focus();
            }
            set_Val();
        }
    }

    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnvchnum.Enabled = false;
        btnext.Visible = true; btncan.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; FileUpload1.Enabled = false;
    }

    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnvchnum.Enabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnext.Visible = false; btncan.Visible = true; FileUpload1.Enabled = true;
    }

    public void chk_f()
    {
        merr = fgen.check_filed_name(frm_qstr, frm_cocd, "IVCHCTRL", "IMAGEF");

        if (merr == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD IMAGEF VARCHAR(100) DEFAULT '-'");
    }

    void set_Val()
    {
        if (VCH_STYLE == "Y")
        {
            hf_form_mode.Value = "VCH";
            lblheader.Text = "Upload Invoice Against Voucher No.";
            mrheading.InnerText = "Vch No./ Date";
            btnvchnum.ToolTip = "View Voucher";
        }
        else
        {
            hf_form_mode.Value = "MRR";
            lblheader.Text = "Upload Invoice Against MRR No.";
            mrheading.InnerText = "MRR No./ Date";
            btnvchnum.ToolTip = "View MRR";
        }
        if (frm_formID == "F15471")
        {
            hf_form_mode.Value = "PO";
            lblheader.Text = "Upload PO";
            mrheading.InnerText = "PO No./ Date";
            btnvchnum.ToolTip = "View PO";
            Amount.InnerText = "Qty Order";
        }
        if (frm_formID == "F50655")
        {
            hf_form_mode.Value = "INV";
            lblheader.Text = "Upload Invoice";
            mrheading.InnerText = "Vch No./ Date";
            btnvchnum.ToolTip = "View Invoice";
            btnvchnum.Visible = false;

        }
    }

    public void clearctrl()
    {
        hffield.Value = "";
        Form_vty.Value = "";
    }

    public void disp_data()
    {
        btnval = hffield.Value;
        switch (btnval)
        {
            case "New":
                SQuery = "SELECT type1 as fstr,name as mrr_type,type1 as code FROM TYPE WHERE ID='M' and type1 like '0%' order by type1";
                if (hf_form_mode.Value == "VCH")
                {
                    SQuery = "SELECT distinct trim(a.Type) as FStr,b.Name as Voucher_Type_Name,a.Type as Voucher_Type from Voucher A,type b WHERE a.branchcd='" + frm_mbr + "' and a.type like '%' and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') and trim(a.type)=trim(b.type1) and b.id='V' order by A.type";
                }
                else
                {
                    SQuery = "SELECT type1 as fstr,name as VCH_type,type1 as code FROM TYPE WHERE ID='M' AND TYPE1 LIKE '5%' order by type1";
                }
                if (frm_formID == "F25371")
                {
                    SQuery = "SELECT type1 as fstr,name as mrr_type,type1 as code FROM TYPE WHERE ID='M' and type1 like '0%' order by type1";
                }
                if (frm_formID == "F50655")
                {
                    SQuery = "SELECT type1 as fstr,name as INV_type,type1 as code FROM TYPE WHERE ID='V' and type1 like '4%' order by type1";
                }

                break;

            case "MRR":
                SQuery = "Select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as mrr_no,to_char(a.vchdate,'dd/mm/yyyy') as mrr_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,to_char(a.vchdate,'yyyymmdd') as vdd from ivchctrl a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type ='" + Form_vty.Value + "' and a.vchdate " + DateRange + " and trim(nvl(a.imagef,'-'))='-' order by vdd desc,a.vchnum desc";
                if (hf_form_mode.Value == "VCH")
                {
                    SQuery = "Select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as vch_no,to_char(a.vchdate,'dd/mm/yyyy') as vch_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,to_char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type ='" + Form_vty.Value + "' and a.vchdate " + DateRange + "  AND A.SRNO=50 order by vdd desc";
                    SQuery = "SELECT A.FSTR,A.VCHNUM AS Voucher_No,A.VCHDATE AS Voucher_Dt,A.ACODE AS Account_Code,B.ANAME as Account_Name,max(a.invno) as billno,max(a.invdate) as billdate,max(a.Ent_by) as Ent_by,to_char(to_date(a.vchdate,'dd/mm/yyyy'),'yyyymmdd') as VDD from (SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.BRANCHCD,A.TYPE,A.VCHNUM ,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,(case when a.type='50' then trim(a.rcode) else TRIM(A.ACODE) end) AS ACODE,1 AS QTY,a.ent_by,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate FROM VOUCHER A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + Form_vty.Value + "' and a.vchdate " + DateRange + " and a.srno=1 UNION ALL SELECT A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ACODE) AS ACODE,-1 AS QTY,null as Ent_by,null as invno,null as invdate FROM ATCHVCH A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + Form_vty.Value + "' and a.vchdate " + DateRange + ") a,famst b where trim(a.acodE)=trim(B.acodE) group by a.fstr,a.vchnum,a.vchdate,a.acode,b.aname having sum(qty)>0 order by vdd desc ,a.vchnum desc";
                }
                break;
            case "PO":
                SQuery = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY') AS FSTR,a.ordno ,TO_CHAR(A.orddt,'DD/MM/YYYY') AS order_dt,TRIM(A.ACODE) AS ACODE,b.aname as party,sum(a.qtyord) as qty_ord , sum(a.qtybal) as qtybal,to_char(a.orddt,'yyyymmdd') as vdd FROM pomas A,famst b WHERE trim(a.acode)=trim(B.acode) and A.BRANCHCD='" + frm_mbr + "' AND A.TYPE ='" + Form_vty.Value + "' and a.orddt " + DateRange + " group by a.ordno,a.orddt,a.acode, a.branchcd,a.type,b.aname order by vdd desc, a.ORDNO desc";
                break;
            case "INV":
                SQuery = "select trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,b.aname,a.type,c.name,a.bill_tot from sale a , famst b , type c where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(c.type1) and c.id='V'  and a.branchcd='" + frm_mbr + "' and a.type='" + Form_vty.Value + "' and a.vchdate " + DateRange + " ORDER BY A.VCHNUM DESC";
                break;
            default:
                if (btnval == "Edit" || btnval == "Del" || btnval == "Print" || btnval == "List")
                {
                    SQuery = "SELECT type1 as fstr,name as mrr_type,type1 as code FROM TYPE WHERE ID='M' and type1 like '0%' order by type1";

                    if (hf_form_mode.Value == "VCH") SQuery = "SELECT distinct trim(a.Type) as FStr,b.Name as Voucher_Type_Name,a.Type as Voucher_Type from Voucher A,type b WHERE a.branchcd='" + frm_mbr + "' and a.type like '%' and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') and trim(a.type)=trim(b.type1) and b.id='V' order by A.type";

                    if (hf_form_mode.Value == "PO") { SQuery = "SELECT type1 as fstr,name as mrr_type,type1 as code FROM TYPE WHERE ID='M' and type1 like '5%' order by type1"; }

                    if (hf_form_mode.Value == "INV") { SQuery = "SELECT type1 as fstr,name as INV_type,type1 as code FROM TYPE WHERE ID='V' and type1 like '4%' order by type1"; }
                }
                else if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                {
                    SQuery = "Select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as mrr_no,to_char(a.vchdate,'dd/mm/yyyy') as mrr_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,to_char(a.vchdate,'yyyymmdd') as vdd from ivchctrl a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type ='" + Form_vty.Value + "' and a.vchdate " + DateRange + "  order by vdd desc";
                    if (hf_form_mode.Value == "VCH")
                    {
                        SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as VCH_no,to_char(a.vchdate,'dd/mm/yyyy') as VCH_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,to_char(a.vchdate,'yyyymmdd') as vdd,1 as qty from VOUCHER a,famst b,ATCHVCH C where trim(a.acode)=trim(b.acode) AND A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=C.BRANCHCD||C.TYPE||TRIM(c.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') and a.branchcd='" + frm_mbr + "' and a.type ='" + Form_vty.Value + "' and a.vchdate " + DateRange + " and trim(nvl(C.MSGTXT,'-'))!='-' and a.srno=1 ";
                        if (Form_vty.Value == "50" || Form_vty.Value == "51")
                            SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as VCH_no,to_char(a.vchdate,'dd/mm/yyyy') as VCH_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,to_char(a.vchdate,'yyyymmdd') as vdd,1 as qty from VOUCHER a,famst b,ATCHVCH C where trim(a.rcode)=trim(b.acode) AND A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=C.BRANCHCD||C.TYPE||TRIM(c.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') and a.branchcd='" + frm_mbr + "' and a.type ='" + Form_vty.Value + "' and a.vchdate " + DateRange + " and trim(nvl(C.MSGTXT,'-'))!='-' and a.srno=1 ";
                    }
                    if (hf_form_mode.Value == "PO")
                    {
                        SQuery = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY') AS FSTR,a.ordno ,TO_CHAR(A.orddt,'DD/MM/YYYY') AS orddT,TRIM(A.ACODE) AS ACODE,b.aname as party,sum(a.qtyord) as qty_ord , sum(a.qtybal) as qtybal,to_char(a.orddt,'yyyymmdd') as vdd FROM pomas A,famst b WHERE trim(a.acode)=trim(b.acode) and A.BRANCHCD='" + frm_mbr + "' AND A.TYPE ='" + Form_vty.Value + "' and a.orddt " + DateRange + " and trim(nvl(a.atch1,'-'))!='-' group by a.ordno,a.orddt,a.acode, a.branchcd,a.type,b.aname order by vdd desc, a.ORDNO desc";
                    }
                    if (hf_form_mode.Value == "INV")
                    {
                        SQuery = "SELECT distinct A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY') AS FSTR,a.vchnum as Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Inv_Date,a.acode as Party_code,b.aname as Party_Name,a.Bill_tot as Inv_Tot_Amt FROM sale A,famst b, ATCHVCH c WHERE trim(a.acode)=trim(b.acode) and A.BRANCHCD='" + frm_mbr + "' AND A.TYPE ='" + Form_vty.Value + "' and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(c.branchcd)||trim(c.type)||trim(c.vchnum)||to_char(c.vchdate,'dd/mm/yyyy') and a.vchdate  " + DateRange + "  order by a.vchnum desc";
                    }

                }
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
        clearctrl();
        hffield.Value = "New";
        disp_data();
        fgen.Fn_open_sseek("-", frm_qstr);
    }

    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Edit";
        disp_data();
        fgen.Fn_open_sseek("-", frm_qstr);
    }

    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        if (sg1.Rows.Count > 0)
        {
            fgen.msg("-", "SMSG", "Are you Sure!! You Want to Save");
            btnsave.Disabled = true;
        }
        else
        {
            fgen.msg("-", "AMSG", "Please attach Voucher first!!");
        }
    }

    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Del";
        disp_data();
        fgen.Fn_open_sseek("-", frm_qstr);
    }

    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "List";
        disp_data();
        fgen.Fn_open_sseek("-", frm_qstr);
    }

    protected void btnext_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }

    protected void btncan_ServerClick(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();
        sg1.DataSource = null; sg1.DataBind();
    }

    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;

        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();

            if (col1 == "Y")
            {
                if (hf_form_mode.Value == "MRR")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "Update ivchctrl set imagef='-' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + Form_vty.Value.Trim() + "'");

                    fgen.msg("-", "AMSG", "Attached Voucher has been deleted for Mrr No. " + Form_vty.Value.Substring(4, 6) + "");
                }
                if (hf_form_mode.Value == "VCH")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM ATCHVCH where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + Form_vty.Value.Trim() + "'");
                    fgen.msg("-", "AMSG", "Attached Voucher has been deleted for Vch No. " + Form_vty.Value.Substring(4, 6) + "");
                }
                if (hf_form_mode.Value == "PO")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE POMAS SET ATCH1='-' WHERE branchcd||type||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + Form_vty.Value.Trim() + "'");
                    fgen.msg("-", "AMSG", "Attached PO File Path has Been Deleted For PO No." + Form_vty.Value.Substring(4, 6) + " ");
                }

                if (hf_form_mode.Value == "INV")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM ATCHVCH where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + Form_vty.Value.Trim() + "'");
                    fgen.msg("-", "AMSG", "Attached Invoice File Path has Been Deleted For Invoice No." + Form_vty.Value.Substring(4, 6) + " ");
                }

                clearctrl(); fgen.ResetForm(this.Controls);
            }
        }
        else
        {
            {
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
                col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

                switch (btnval)
                {
                    case "Del_E":
                        clearctrl();
                        Form_vty.Value = col1;
                        fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                        hffield.Value = "D";
                        break;
                    case "New":
                        Form_vty.Value = col1;
                        hffield.Value = "MRR";
                        if (frm_formID == "F15471")
                        {
                            hffield.Value = "PO";
                        }
                        if (frm_formID == "F50655")
                        {
                            hffield.Value = "INV";
                        }
                        disp_data();
                        fgen.Fn_open_sseek("-", frm_qstr);
                        break;
                    case "Edit":
                        Form_vty.Value = col1;
                        hffield.Value = "Edit_E";
                        disp_data();
                        fgen.Fn_open_sseek("-", frm_qstr);
                        break;
                    case "Del":
                        Form_vty.Value = col1;
                        hffield.Value = "Del_E";
                        disp_data();
                        fgen.Fn_open_sseek("-", frm_qstr);
                        break;
                    case "Edit_E":
                        SQuery = "Select a.*,b.aname,to_char(a.vchdate,'dd/mm/yyyy') as vcd,to_char(a.invdate,'dd/mm/yyyy') as ind from ivchctrl a,famst b where trim(A.acode)=trim(B.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + col1 + "')";
                        if (hf_form_mode.Value == "VCH") SQuery = "Select a.*,(a.CRAMT+a.dramt) as bill_tot,C.MSGTXT AS IMAGEF,b.aname,to_char(a.vchdate,'dd/mm/yyyy') as vcd,to_char(a.invdate,'dd/mm/yyyy') as ind from voucher a,famst b,ATCHVCH C where trim(A.rcode)=trim(B.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=C.branchcd||C.type||trim(C.vchnum)||to_char(C.vchdate,'dd/mm/yyyy') and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + col1 + "') AND A.SRNO=1 ORDER BY C.MSGDT";

                        dt = new DataTable();
                        if (hf_form_mode.Value != "PO")
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim(); txtvchdt.Text = dt.Rows[0]["vcd"].ToString().Trim();
                            txtbillno.Text = dt.Rows[0]["invno"].ToString().Trim(); txtbilldt.Text = dt.Rows[0]["ind"].ToString().Trim();
                            txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                            txttype.Text = Form_vty.Value.Trim();

                            if (hf_form_mode.Value == "MRR")
                            { txttypename.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='M' and type1='" + Form_vty.Value.Trim() + "'", "name").Trim(); }
                            if (hf_form_mode.Value == "VCH")
                            { txttypename.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='V' and type1='" + Form_vty.Value.Trim() + "'", "name").Trim(); }

                            txtamt.Text = dt.Rows[0]["bill_tot"].ToString().Trim();
                            create_tab();
                            if (VCH_STYLE == "Y")
                            {
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dr1 = dt1.NewRow();
                                    dr1["srno"] = (i + 1);
                                    dr1["filno"] = dt.Rows[i]["IMAGEF"].ToString().Trim();
                                    dt1.Rows.Add(dr1);
                                }
                            }
                            else
                            {
                                dr1 = dt1.NewRow();
                                dr1["srno"] = 1;
                                dr1["filno"] = dt.Rows[0]["IMAGEF"].ToString().Trim();
                                dt1.Rows.Add(dr1);
                            }
                            ViewState["sg1"] = dt1;
                            sg1.DataSource = dt1; sg1.DataBind();
                            disablectrl(); fgen.EnableForm(this.Controls); FileUpload1.Focus();
                        }
                        if (hf_form_mode.Value == "PO")
                        {
                            SQuery = "SELECT A.*, B.ANAME FROM POMAS A, FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY')='" + col1 + "' ";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                txtvchnum.Text = dt.Rows[0]["ordno"].ToString().Trim(); txtvchdt.Text = Convert.ToDateTime(dt.Rows[0]["orddt"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtbillno.Text = dt.Rows[0]["pr_no"].ToString().Trim(); txtbilldt.Text = Convert.ToDateTime(dt.Rows[0]["pr_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                                txttype.Text = Form_vty.Value.Trim();
                                txtamt.Text = "-";
                                txttypename.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='M' and type1='" + Form_vty.Value.Trim() + "'", "name").Trim();
                                txtamt.Text = dt.Rows[0]["qtyord"].ToString().Trim();
                                create_tab(); Form_vty.Value = col1;
                                dr1 = dt1.NewRow(); dr1["srno"] = 1;
                                dr1["filno"] = dt.Rows[0]["atch1"].ToString().Trim();
                                dt1.Rows.Add(dr1); ViewState["sg1"] = dt1;
                                sg1.DataSource = dt1; sg1.DataBind();
                                disablectrl(); fgen.EnableForm(this.Controls); FileUpload1.Focus();
                            }
                        }
                        else clearctrl();

                        if (hf_form_mode.Value == "INV")
                        {
                            //SQuery = "select a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.atch1,a.acode,b.aname,a.type,c.name,a.bill_tot,d.vchnum as PONO,to_char(d.vchdate,'dd/mm/yyyy') as PoDate  from sale a , famst b , type c , voucher d , atchvch e where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(c.type1)  and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)=trim(d.branchcd)||trim(d.type)||trim(d.INVNO)||to_char(d.INVDate,'dd/mm/yyyy')||trim(d.acode) and c.id='M' and  A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + col1 + "' ";
                            SQuery = "SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,b.aname,a.type,c.name,a.bill_tot,d.vchnum as PONO,to_char(d.vchdate,'dd/mm/yyyy') as PoDate,e.msgtxt FROM sale a , famst b , type c , voucher d, ATCHVCH e WHERE trim(a.acode)=trim(b.acode) and trim(a.type)=trim(c.type1)  and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)=trim(d.branchcd)||trim(d.type)||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')||trim(d.acode) and c.id='V' and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(e.branchcd)||trim(e.type)||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy') and  A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + col1 + "' ";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                txtvchnum.Text = dt.Rows[0]["PONO"].ToString().Trim(); txtvchdt.Text = Convert.ToDateTime(dt.Rows[0]["PoDate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtbillno.Text = dt.Rows[0]["vchnum"].ToString().Trim(); txtbilldt.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                                txttype.Text = dt.Rows[0]["type"].ToString().Trim(); txttypename.Text = dt.Rows[0]["name"].ToString().Trim();
                                txtamt.Text = dt.Rows[0]["bill_tot"].ToString().Trim();
                                create_tab(); Form_vty.Value = col1;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dr1 = dt1.NewRow();
                                    dr1["srno"] = dt1.Rows.Count + 1;
                                    dr1["filno"] = dt.Rows[i]["msgtxt"].ToString().Trim();
                                    dt1.Rows.Add(dr1); ViewState["sg1"] = dt1;
                                }


                                sg1.DataSource = dt1; sg1.DataBind();
                                disablectrl(); fgen.EnableForm(this.Controls); FileUpload1.Focus();
                            }


                        }

                        Form_vty.Value = col1;
                        break;
                    case "MRR":
                        SQuery = "Select a.*,b.aname,to_char(a.vchdate,'dd/mm/yyyy') as vcd,to_char(a.invdate,'dd/mm/yyyy') as ind from ivchctrl a,famst b where trim(A.acode)=trim(B.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + col1 + "')";
                        if (hf_form_mode.Value == "VCH") SQuery = "Select a.*,(a.CRAMT+a.dramt) as bill_tot,b.aname,to_char(a.vchdate,'dd/mm/yyyy') as vcd,to_char(a.invdate,'dd/mm/yyyy') as ind from voucher a,famst b where trim(A.acode)=trim(B.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + col1 + "') AND A.SRNO=1";

                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0) { }
                        else
                        {
                            if (hf_form_mode.Value == "VCH") SQuery = "Select a.*,a.CRAMT as bill_tot,b.aname,to_char(a.vchdate,'dd/mm/yyyy') as vcd,to_char(a.invdate,'dd/mm/yyyy') as ind from voucher a,famst b where trim(A.acode)=trim(B.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + col1 + "')  and a.CRAMT>0";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        }
                        if (dt.Rows.Count > 0)
                        {
                            // checked two days lock for file uploading 
                            if (frm_formID == "F25371" && fgen.getOption(frm_qstr, frm_cocd, "W0101", "OPT_ENABLE") == "Y")
                            {
                                string allowedDays = "";
                                allowedDays = fgen.getOption(frm_qstr, frm_cocd, "W0101", "OPT_PARAM");
                                if (Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString()) < DateTime.Now.AddDays(allowedDays.toDouble() * -1))
                                {
                                    if (frm_ulvl.toDouble() > 1)
                                    {
                                        fgen.msg("-", "AMSG", "You Can not Upload file Against this MRR as this is older then " + allowedDays + " days'13'Please contact to Admin");
                                        clearctrl();
                                        return;
                                    }
                                }
                            }

                            txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim(); txtvchdt.Text = dt.Rows[0]["vcd"].ToString().Trim();
                            txtbillno.Text = dt.Rows[0]["invno"].ToString().Trim(); txtbilldt.Text = dt.Rows[0]["ind"].ToString().Trim();
                            txttype.Text = Form_vty.Value.Trim();
                            if (txttype.Text == "50")
                            {
                                txtacode.Text = dt.Rows[0]["rcode"].ToString().Trim();
                                txtaname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE='" + txtacode.Text.Trim() + "'", "ANAME");
                            }
                            else
                            {
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                                txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                            }

                            if (hf_form_mode.Value == "MRR")
                            { txttypename.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='M' and type1='" + Form_vty.Value.Trim() + "'", "name").Trim(); }
                            if (hf_form_mode.Value == "VCH")
                            { txttypename.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='V' and type1='" + Form_vty.Value.Trim() + "'", "name").Trim(); }

                            txtamt.Text = dt.Rows[0]["bill_tot"].ToString().Trim();
                            dt1 = new DataTable(); dt1 = null;
                            create_tab(); Form_vty.Value = col1;
                            disablectrl(); fgen.EnableForm(this.Controls); FileUpload1.Focus();
                        }

                        else clearctrl();
                        break;
                    case "INV":
                        SQuery = "select a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,b.aname,a.type,c.name,a.bill_tot,d.vchnum as PONO,to_char(d.vchdate,'dd/mm/yyyy') as PoDate  from sale a , famst b , type c , voucher d where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(c.type1)  and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)=trim(d.branchcd)||trim(d.type)||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')||trim(d.acode) and c.id='V' and  A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + col1 + "' ";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtvchnum.Text = dt.Rows[0]["PONO"].ToString().Trim(); txtvchdt.Text = dt.Rows[0]["PoDate"].ToString().Trim();
                            txtbillno.Text = dt.Rows[0]["vchnum"].ToString().Trim(); txtbilldt.Text = dt.Rows[0]["vchdate"].ToString().Trim();
                            txttype.Text = dt.Rows[0]["type"].ToString().Trim(); txttypename.Text = dt.Rows[0]["name"].ToString().Trim();
                            txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                            txtamt.Text = dt.Rows[0]["bill_tot"].ToString().Trim();
                            create_tab(); Form_vty.Value = col1;
                            disablectrl(); fgen.EnableForm(this.Controls); FileUpload1.Focus();

                            //mq0 = "select  b.icode,b.iname,a.iqtyout,a.purpose,b.cpartno,b.cdrgno,b.unit from ivoucher a , item b  where  trim(a.icode)=trim(b.icode) and A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY')='" + col1 + "' and a.acode='" + dt.Rows[0]["acode"].ToString().Trim() + "' ";
                            //dt1 = new DataTable();
                            //dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                            //if (dt1.Rows.Count > 0)
                            //{

                            //}

                        }


                        break;
                    case "PO":
                        SQuery = "SELECT trim(a.ordno) as ordno ,TO_CHAR(A.orddt,'DD/MM/YYYY') AS orddT,a.pr_no,TO_CHAR(A.pr_dt,'DD/MM/YYYY') AS pr_dt,TRIM(A.ACODE) AS ACODE,sum(a.qtyord) as qty_ord,sum(a.qtybal) as qtybal, b.aname FROM pomas A , famst b  WHERE trim(a.acode)=trim(b.acode) and A.BRANCHCD||A.TYPE||TRIM(A.ordno)||TO_cHAR(A.orddt,'DD/MM/YYYY')='" + col1 + "' group by a.ordno,A.orddt,a.pr_no,A.pr_dt,A.ACODE,b.aname";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtvchnum.Text = dt.Rows[0]["ordno"].ToString().Trim(); txtvchdt.Text = dt.Rows[0]["orddT"].ToString().Trim();
                            txtbillno.Text = dt.Rows[0]["pr_no"].ToString().Trim(); txtbilldt.Text = dt.Rows[0]["pr_dt"].ToString().Trim();
                            txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                            txttype.Text = Form_vty.Value.Trim();
                            txttypename.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='M' and type1='" + Form_vty.Value.Trim() + "'", "name").Trim();

                            txtamt.Text = dt.Rows[0]["qty_ord"].ToString().Trim();
                            dt1 = new DataTable(); dt1 = null;
                            create_tab(); Form_vty.Value = col1;
                            disablectrl(); fgen.EnableForm(this.Controls); FileUpload1.Focus();
                        }

                        break;
                    case "List":
                        if (hf_form_mode.Value == "MRR")
                        {
                            SQuery = "Select a.vchnum as Mrr_no,to_char(a.vchdate,'dd/mm/yyyy') as Mrr_Date,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_date,b.aname as party_name,a.bill_tot as amt,a.type as mrr_type,to_char(a.vchdate,'yyymmdd') as vdd from ivchctrl a,famst b where trim(a.acode)=trim(B.acodE) and a.branchcd='" + frm_mbr + "' and a.type like '" + col1 + "%' and a.vchdate " + DateRange + "  and nvl(a.IMAGEF,'-')!='-' order by a.type,vdd desc ";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("List of Uploded Invoices Against MRR", frm_qstr);
                        }
                        if (hf_form_mode.Value == "VCH")
                        {
                            SQuery = "Select b.aname as party_name,a.vchnum as Vch_no,to_char(a.vchdate,'dd/mm/yyyy') as vch_Date,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_date,a.CRAMT+a.dramt as amt,a.mrnnum,to_char(a.mrndate,'dd/mm/yyyy') as mrndate ,a.type as vch_type,to_char(a.vchdate,'yyymmdd') as vdd from voucher a,famst b,ATCHVCH C where trim(a.acode)=trim(B.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=C.branchcd||C.type||trim(C.vchnum)||to_char(C.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type like '" + col1 + "%' and a.vchdate " + DateRange + "  and nvl(c.msgtxt,'-')!='-' and substr(a.acode,1,2) in ('06','16') order by a.type,vdd desc ";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("List of Uploded Bills Against Voucher Entry", frm_qstr);
                        }
                        if (hf_form_mode.Value == "PO")
                        {
                            //SQuery = "select a.*,b.aname from pomas a , famst b where trim(a.acode)=trim(b.acode) and branchcd='" + frm_mbr + "' and a.type='" + col1 + "' and orddt " + DateRange + " and nvl(a.atch1,'-')!='-'";
                            SQuery = "select trim(a.ordno) as order_no,to_char(a.orddt,'dd/mm/yyyy') as order_dt,a.pr_no,to_char(a.pr_dt,'dd/mm/yyyy') as pr_date,trim(a.acode) as party_code ,trim(b.aname) as Party_name,sum(a.qtyord) as Qty_ord,to_char(a.orddt,'yyymmdd') as vdd from pomas a , famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + col1 + "' and a.orddt " + DateRange + " and nvl(a.atch1,'-')!='-' group by a.ordno,a.orddt,a.pr_no,a.pr_dt,a.acode,b.aname";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("List Of Uploaded PO", frm_qstr);
                        }
                        if (hf_form_mode.Value == "INV")
                        {
                            //SQuery = "select a.vchnum as Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Inv_Date,a.acode as Party_code,b.aname as Party_Name,a.Bill_tot as Inv_Tot_Amt,a.atch1 as Inv_Attached from sale a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + col1 + "' and a.vchdate " + DateRange + " and nvl(a.ATCH1,'-')!='-'  ";
                            SQuery = "SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,b.aname,a.type,c.name,a.bill_tot,e.msgtxt FROM sale a , famst b , type c , voucher d, ATCHVCH e WHERE trim(a.acode)=trim(b.acode) and trim(a.type)=trim(c.type1)  and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)=trim(d.branchcd)||trim(d.type)||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')||trim(d.acode) and c.id='V' and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(e.branchcd)||trim(e.type)||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type='" + col1 + "' and a.vchdate " + DateRange + " ";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("List Of Uploaded Invoices", frm_qstr);
                        }
                        break;
                    case "SG1_RMV":
                        #region Remove Row from GridView
                        if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        {
                            dt = new DataTable();
                            DataTable sg1_dt = new DataTable();
                            dt = (DataTable)ViewState["sg1"];
                            int z = dt.Rows.Count;
                            sg1_dt = dt.Clone();
                            DataRow sg1_dr = null;
                            int i = 0;
                            for (i = 0; i < sg1.Rows.Count; i++)
                            {
                                sg1_dr = sg1_dt.NewRow();
                                sg1_dr[0] = sg1.Rows[i].Cells[1].Text;
                                sg1_dr[1] = sg1.Rows[i].Cells[4].Text;
                                sg1_dt.Rows.Add(sg1_dr);
                            }
                            sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();

                            ViewState["sg1"] = sg1_dt;
                            sg1.DataSource = sg1_dt;
                            sg1.DataBind();
                        }
                        #endregion
                        break;
                }
            }
        }
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        col1 = "";

        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
        if (col1 == "N")
        {
            btnsave.Disabled = false;
        }
        else
        {
            if (hf_form_mode.Value == "MRR") fgen.execute_cmd(frm_qstr, frm_cocd, "Update ivchctrl set imagef='" + sg1.Rows[0].Cells[4].Text.Trim() + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + Form_vty.Value + "'");
            if (hf_form_mode.Value == "VCH")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM ATCHVCH where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + Form_vty.Value + "'");

                DataSet oDS = new DataSet();
                DataRow oporow = null;
                oDS = fgen.fill_schema(frm_qstr, frm_cocd, "ATCHVCH");
                //fgen.execute_cmd(co_cd, "Update voucher set imagef='" + sg1.Rows[0].Cells[4].Text.Trim() + "' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + Form_vty.Value + "'");
                for (int i = 0; i < sg1.Rows.Count; i++)
                {
                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = Form_vty.Value.Substring(0, 2).Trim();
                    oporow["TYPE"] = Form_vty.Value.Substring(2, 2).Trim();
                    oporow["vchnum"] = Form_vty.Value.Substring(4, 6).Trim();
                    oporow["vchdate"] = Form_vty.Value.Substring(10, 10).Trim();
                    oporow["acode"] = txtacode.Text.Trim();
                    oporow["MSGDT"] = (i + 1);
                    oporow["msgtxt"] = sg1.Rows[i].Cells[4].Text.Trim();
                    oDS.Tables[0].Rows.Add(oporow);
                }
                // fgen.save_data(oDS, "ATCHVCH");
                fgen.save_data(frm_qstr, frm_cocd, oDS, "ATCHVCH");

                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE VOUCHER SET APP_BY='-' WHERE BRANCHCD||TYPE||TRIM(vCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + Form_vty.Value + "'");
            }
            if (hf_form_mode.Value == "PO")
            {
                SQuery = "UPDATE POMAS SET ATCH1='" + sg1.Rows[0].Cells[4].Text.Trim() + "' WHERE branchcd||type||trim(ORDNO)||to_char(ORDDT,'dd/mm/yyyy')='" + Form_vty.Value + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE POMAS SET ATCH1='" + sg1.Rows[0].Cells[4].Text.Trim() + "' WHERE branchcd||type||trim(ORDNO)||to_char(ORDDT,'dd/mm/yyyy')='" + Form_vty.Value + "'");
            }

            if (hf_form_mode.Value == "INV")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM ATCHVCH where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + Form_vty.Value + "'");
                DataSet oDS = new DataSet();
                DataRow oporow = null;
                oDS = fgen.fill_schema(frm_qstr, frm_cocd, "ATCHVCH");

                for (int i = 0; i < sg1.Rows.Count; i++)
                {
                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = Form_vty.Value.Substring(0, 2).Trim();
                    oporow["TYPE"] = Form_vty.Value.Substring(2, 2).Trim();
                    oporow["vchnum"] = Form_vty.Value.Substring(4, 6).Trim();
                    oporow["vchdate"] = Form_vty.Value.Substring(10, 10).Trim();
                    oporow["acode"] = txtacode.Text.Trim();
                    oporow["MSGDT"] = (i + 1);
                    oporow["msgtxt"] = sg1.Rows[i].Cells[4].Text.Trim();
                    oDS.Tables[0].Rows.Add(oporow);
                }

                //fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE Sale SET ATCH1='" + sg1.Rows[0].Cells[4].Text.Trim() + "' WHERE BRANCHCD||TYPE||TRIM(vCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + Form_vty.Value + "'");
                fgen.save_data(frm_qstr, frm_cocd, oDS, "ATCHVCH");

            }
        }
        if (hf_form_mode.Value == "PO") { fgen.msg("-", "AMSG", "PO Uploded Successfully"); }
        else if (hf_form_mode.Value == "INV") { fgen.msg("-", "AMSG", "Invoice Uploded Successfully"); }
        else
            fgen.msg("-", "AMSG", "Voucher Uploded Successfully");
        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); sg1.DataSource = null; sg1.DataBind();
    }

    protected void btnupload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            string filepath = @"c:\TEJ_ERP\UPLOAD\";
            //FileUpload1.SaveAs((Server.MapPath(filepath) + FileUpload1.FileName.Trim().ToString()));
            string ext = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower();
            cond = "";
            if (FileUpload1.FileName.Replace(ext, "").Length > 5) cond = FileUpload1.FileName.Replace(ext, "").Substring(0, 5);
            else cond = FileUpload1.FileName.Replace(ext, "");
            fileNme = "F-" + frm_mbr + "_" + txttype.Text.Trim() + "_" + txtvchnum.Text.Trim() + "_" + txtvchdt.Text.Replace("/", "") + "_" + (sg1.Rows.Count + 1) + "_" + cond + ext;
            if (System.IO.File.Exists(filepath + fileNme))
            {
                try
                {
                    System.IO.File.Delete(filepath + fileNme);
                }
                catch { }
                try
                {
                    System.IO.File.Delete(Server.MapPath("~/tej-base/Upload/") + fileNme);
                }
                catch { }
            }
            FileUpload1.PostedFile.SaveAs(filepath + fileNme);
            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/tej-base/Upload/") + fileNme);
            fill_grid();
        }
    }

    public void create_tab()
    {
        dt1 = new DataTable();
        dr1 = null;
        dt1.Columns.Add(new DataColumn("SrNo", typeof(Int32)));
        dt1.Columns.Add(new DataColumn("filno", typeof(string)));
        ViewState["sg1"] = dt1;
    }

    public void fill_grid()
    {
        HCID = frm_formID;
        if (ViewState["sg1"] != null)
        {
            dt = new DataTable();
            dt1 = new DataTable();
            dt = (DataTable)ViewState["sg1"];
            dt1 = dt.Clone();
            dr1 = null;
            if (HCID == "27111") { dt = new DataTable(); }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dr1 = dt1.NewRow();
                    dr1["srno"] = Convert.ToInt32(dt.Rows[i]["srno"].ToString());
                    dr1["filno"] = sg1.Rows[i].Cells[4].Text.Trim();
                    dt1.Rows.Add(dr1);
                }
            }
            dr1 = dt1.NewRow();
            dr1["srno"] = dt.Rows.Count + 1;
            dr1["filno"] = fileNme;
            dt1.Rows.Add(dr1);
        }
        ViewState["sg1"] = dt1;
        sg1.DataSource = dt1;
        sg1.DataBind();
    }

    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        HCID = frm_formID;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            switch (HCID)
            {
                case "27111":
                    e.Row.Cells[0].Style["display"] = "none";
                    sg1.HeaderRow.Cells[0].Style["display"] = "none";
                    ImageButton im = (ImageButton)e.Row.FindControl("btnrmv");
                    im.Visible = false;
                    break;
            }
        }
    }

    protected void btnvchnum_Click(object sender, ImageClickEventArgs e)
    {
        if (hf_form_mode.Value == "MRR")
        {
            hffield.Value = "MRR_P";
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NAME FROM TYPE WHERE ID='M' AND trim(TYPE1)='" + txttype.Text.Trim() + "'", "NAME");
            SQuery = "select '" + col1 + "' as typename,a.icode,b.iname,a.unit,a.PNAME, a.iqty_chl,a.iqty_chlwt,a.ponum,TO_CHAr(a.podate,'DD/MM/YYYY') as podate,  a.iqtyin as act,a.acpt_ud,a.rej_rw, a.iqty_wt as actwt,a.irate as rate,a.iqtyin*a.irate as amt, a.vchnum,TO_CHAr(a.vchdate,'dd/MM/yyyy') as vchdate,a.branchcd,a.type,a.invno,TO_CHAr(a.invdate,'dd/MM/yyyy') as invdate,a.refnum,TO_CHAr(a.refdate,'dd/MM/yyyy') as refdate,a.acode,c.aname,c.addr1 AS CADDR1,c.addr2 AS CADDR2,c.addr3 AS CADDR3,C.EMAIL AS C_EMAIL,C.TELNUM,a.mode_tpt, a.genum,TO_CHAr(a.gedate,'dd/MM/yyyy') as gedate,a.cess_pu,a.location,a.naration,a.ent_by,TO_CHAr(a.ent_dt,'DD/MM/YYYY') as ent_dt from ivoucher a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') in ('" + (frm_mbr + txttype.Text.Trim() + txtvchnum.Text.Trim() + txtvchdt.Text.Trim()) + "') AND A.STORE<>'R' order by vchdate,a.vchnum, a.srno";
            col1 = "select rvalue,amt_sale, amt_exc,shvalue,cust_amt,lessamt,cst_amt,ed_rate,lst_amt,lst_rate,frt_amt,pack_amt,insu_amt,other,excb_chg,VATSCHG from ivchctrl where BRANCHCD||TYPE||TRIM(vchnum)||TO_CHAr(vchdate,'DD/MM/YYYY') in ('" + (frm_mbr + txttype.Text.Trim() + txtvchnum.Text.Trim() + txtvchdt.Text.Trim()) + "')";
            //fgen.Print_Report_2ds(frm_qstr,frm_cocd, SQuery, "MRR", "MRR", col1, "rgpmst");
            fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "MRR", "MRR");
        }
        if (hf_form_mode.Value == "VCH")
        {
            col1 = "Select a.type,trim(substr(NVL(a.naration,'-'),1,220)) AS NARATION,nvl(a.mrnnum,'-') as mrnnum,NVL(a.mrndate,A.VCHDATE) AS MRNDATE,NVL(a.tax,'-') AS TAX,A.COSTCD,NVL(a.refnum,'-') AS REFNUM,NVL(a.invno,'-') AS INVNO,";
            col2 = " NVL(a.invdate,A.VCHDATE) AS INVDATE,nvl(a.CCENT,'-') as ccent,a.acode,a.rcode,a.vchnum,a.vchdate,nvl(a.app_by,'-') as app_by,nvl(a.app_date,a.vchdate) as app_Date,a.dramt,a.cramt,nvl(a.quantity,0)as quantity,NVL(a.refdate,A.VCHDATE) AS REFDATE,";
            col3 = " NVL(b.PERSON,'-') AS PERSON,NVL(b.aname,'-') AS ANAME,NVL(B.ANAME,'-') AS PARTY,a.ent_by,nvl(b.payment,'-') as pnm,a.tfcdr,a.tfccr,nvl(a.FCTYPE,'-') ";
            SQuery = col1 + col2 + col3 + " FCTYPE,c.name as VchTypeName from VOUCHER a left outer join famst b on TRIM(A.ACODE)=TRIM(B.ACODE) ,type c where a.type=c.type1 and c.id='V' and a.branchcd||a.type||a.VCHNUM||to_char(a.vchdate,'dd/mm/yyyy') ='" + (frm_mbr + txttype.Text.Trim() + txtvchnum.Text.Trim() + txtvchdt.Text.Trim()) + "' order by a.srno ";
            fgen.Print_Report(frm_qstr, frm_cocd, frm_mbr, SQuery, "vch_rpt", "vch_rpt");
        }
    }

    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);
        string filePath = "";

        switch (var)
        {
            case "Rmv":
                if (index < sg1.Rows.Count)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "Dwl":
                if (e.CommandArgument.ToString().Trim() != "")
                {
                    try
                    {
                        filePath = sg1.Rows[index].Cells[4].Text;

                        Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
                        Session["FileName"] = sg1.Rows[index].Cells[4].Text;
                        Response.Write("<script>");
                        Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                        Response.Write("</script>");
                    }
                    catch { }
                }
                break;
            case "View":
                filePath = sg1.Rows[index].Cells[4].Text;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Tejaxo Viewer');", true);
                break;
        }
    }
    protected void btnexit_ServerClick(Object sender, EventArgs s)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
}