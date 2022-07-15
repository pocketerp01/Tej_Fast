using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;

public partial class om_vch_view : System.Web.UI.Page
{
    string co_cd, uname, col1, col2, col3, vardate, fromdt, todt, year, ulvl, merr = "0", HCID, VCH_STYLE = "N";
    DataTable dt, dt1; DataRow dr1;
    fgenDB fgen = new fgenDB();
    string btnval, SQuery, cstr, cond = "", vip = "", frm_cDt1 = "", frm_cDt2 = "";
    string pk_error = "Y", chk_rights = "N", DateRange;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_cocd, frm_qstr, frm_uname, frm_tabname, frm_myear, frm_sql, frm_ulvl, frm_formID, frm_UserID, filePath = "";

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
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    fromdt = "01/04/" + frm_myear;
                    todt = "31/03/" + Convert.ToString(Convert.ToInt32(frm_myear) + 1);
                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    if (frm_formID == "F70373") VCH_STYLE = "Y";
                    else VCH_STYLE = "N";
                    chk_f();
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl(); btnview.Focus();
                btnview.Focus();
            }
            set_Val();

        }
    }


    public void enablectrl()
    {
        btnview.Disabled = false; btnvchnum.Enabled = false; btnlist.Disabled = false;
        btnext.Visible = true; btncan.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
    }
    public void disablectrl()
    {
        btnview.Disabled = true; btnvchnum.Enabled = true; btnlist.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnext.Visible = false; btncan.Visible = true;
    }
    public void chk_f()
    {
        merr = fgen.check_filed_name(frm_qstr, frm_cocd, "IVCHCTRL", "IMAGEF");
        if (merr == "0")
            fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE IVCHCTRL ADD IMAGEF VARCHAR(100) DEFAULT '-'");
    }
    void set_Val()
    {
        if (VCH_STYLE == "Y")
        {
            hf_form_mode.Value = "VCH";
            lblheader.Text = "View Invoice Against Voucher";
            mrheading.InnerText = "Vch No./ Date";
            btnvchnum.ToolTip = "View Voucher";
        }
        else
        {
            hf_form_mode.Value = "MRR";
            lblheader.Text = "View Invoice in MRR";
            mrheading.InnerText = "MRR No./ Date";
            btnvchnum.ToolTip = "View MRR";
        }
        if (frm_formID == "F15473")
        {
            hf_form_mode.Value = "PO";
            lblheader.Text = "View PO";
            mrheading.InnerText = "PO No./ Date";
            btnvchnum.ToolTip = "View PO";
            billdate.InnerText = "PR No./Date";
            Amount.InnerText = "Qty Order";
        }
        if (frm_formID == "F50659")
        {
            hf_form_mode.Value = "INV";
            lblheader.Text = "View Invoice";
            mrheading.InnerText = "Vch No./ Date";
            btnvchnum.ToolTip = "View Invoice";
            btnvchnum.Visible = false;

        }
    }
    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
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
                    SQuery = "SELECT type1 as fstr,name as VCH_type,type1 as code FROM TYPE WHERE ID='V' order by type1";

                }
                else
                {
                    SQuery = "SELECT type1 as fstr,name as mrr_type,type1 as code FROM TYPE WHERE ID='M' and type1 like '5%' order by type1";
                }

                break;
            case "MRR":
                SQuery = "Select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as mrr_no,to_char(a.vchdate,'dd/mm/yyyy') as mrr_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,to_char(a.vchdate,'yyyymmdd') as vdd from ivchctrl a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type ='" + edmode.Value + "' and a.vchdate " + DateRange + " and trim(nvl(a.imagef,'-'))='-' order by vdd desc";
                if (hf_form_mode.Value == "VCH") SQuery = "Select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as vch_no,to_char(a.vchdate,'dd/mm/yyyy') as vch_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,to_char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type ='" + edmode.Value + "' and a.vchdate " + DateRange + " and trim(nvl(a.imagef,'-'))='-' AND A.SRNO=1 order by vdd desc";
                break;
            case "PO":
                SQuery = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY')||trim(a.icode) AS FSTR,A.ORDNO AS ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORDDT,B.ANAME AS PARTY_NAME,A.PR_NO AS PR_NO,TO_CHAR(A.PR_DT,'DD/MM/YYYY') AS PR_DT,trim(a.icode) as Item_code,c.iname as Item_name,TO_CHAR(A.ORDDT,'yyyymmdd') as vdd from pomas a , famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + edmode.Value + "' and a.orddt " + DateRange + " and trim(nvl(a.atch1,'-'))!='-' order by vdd desc";
                break;
            default:
                if (btnval == "Edit" || btnval == "Del" || btnval == "Print" || btnval == "List")
                {
                    if (hf_form_mode.Value == "VCH")
                    {
                        //SQuery = "SELECT type1 as fstr,name as VCH_type,type1 as code FROM TYPE WHERE ID='V' order by type1";
                        SQuery = "SELECT distinct trim(a.Type) as FStr,b.Name as Voucher_Type_Name,a.Type as Voucher_Type from Voucher A,type b WHERE a.branchcd='" + frm_mbr + "' and a.type like '%' and a.vchdate between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') and trim(a.type)=trim(b.type1) and b.id='V' order by A.type";
                    }
                    else if (hf_form_mode.Value == "INV")
                    {
                        SQuery = "SELECT type1 as fstr,name as mrr_type,type1 as code FROM TYPE WHERE ID='V' and type1 like '4%' order by type1";
                    }
                    else
                    {
                        SQuery = "SELECT type1 as fstr,name as mrr_type,type1 as code FROM TYPE WHERE ID='M' and type1 like '5%' order by type1";
                    }


                    if (frm_formID == "F25373") SQuery = "SELECT type1 as fstr,name as mrr_type,type1 as code FROM TYPE WHERE ID='M' and type1 like '0%' order by type1";

                }
                else if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                {
                    SQuery = "Select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as mrr_no,to_char(a.vchdate,'dd/mm/yyyy') as mrr_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,to_char(a.vchdate,'yyyymmdd') as vdd from ivchctrl a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type ='" + edmode.Value + "' and a.vchdate " + DateRange + " and trim(nvl(a.imagef,'-'))!='-' order by vdd desc";
                    if (frm_cocd == "SRIS") SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as mrr_no,to_char(a.vchdate,'dd/mm/yyyy') as mrr_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,to_char(a.vchdate,'yyyymmdd') as vdd from IVOUCHER a,famst b,VOUCHER C where trim(a.acode)=trim(b.acode) AND TRIM(A.FINVNO)=TRIM(C.VCHNUM)||' '||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') AND C.TYPE LIKE '5%' AND C.BRANCHCD='" + frm_mbr + "' and a.branchcd='" + frm_mbr + "' and a.type ='" + edmode.Value + "' and a.vchdate " + DateRange + "  and length(Trim(nvl(c.app_by,'-')))>1 order by vdd desc";
                    if (hf_form_mode.Value == "VCH")
                    {
                        SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as VCH_no,to_char(a.vchdate,'dd/mm/yyyy') as VCH_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,to_char(a.vchdate,'yyyymmdd') as vdd from VOUCHER a,famst b,ATCHVCH C where trim(a.acode)=trim(b.acode) AND A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)=C.BRANCHCD||C.TYPE||TRIM(c.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE) and a.branchcd='" + frm_mbr + "' and a.type ='" + edmode.Value + "' and a.vchdate " + DateRange + " and trim(nvl(C.MSGTXT,'-'))!='-' AND A.SRNO=1 order by vdd desc";
                        SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as VCH_no,to_char(a.vchdate,'dd/mm/yyyy') as VCH_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,a.mrnnum,to_char(a.mrndate,'dd/mm/yyyy') as mrndate ,a.type as vch_type,to_char(a.vchdate,'yyyymmdd') as vdd from VOUCHER a,famst b,ATCHVCH C where trim(a.acode)=trim(b.acode) AND A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)=C.BRANCHCD||C.TYPE||TRIM(c.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE) and a.branchcd='" + frm_mbr + "' and a.type ='" + edmode.Value + "' and a.vchdate " + DateRange + " and trim(nvl(C.MSGTXT,'-'))!='-' AND A.SRNO=1 order by vdd desc";
                        frm_vty = edmode.Value;
                        SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as VCH_no,to_char(a.vchdate,'dd/mm/yyyy') as VCH_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,a.mrnnum,to_char(a.mrndate,'dd/mm/yyyy') as mrndate ,a.type as vch_type,to_char(a.vchdate,'yyyymmdd') as vdd,1 as qty from VOUCHER a,famst b,ATCHVCH C where trim(a.acode)=trim(b.acode) AND A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=C.BRANCHCD||C.TYPE||TRIM(c.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') and a.branchcd='" + frm_mbr + "' and a.type ='" + frm_vty + "' and a.vchdate " + DateRange + " and trim(nvl(C.MSGTXT,'-'))!='-' and a.srno=1 ";
                        if (frm_vty == "50" || frm_vty == "51")
                            SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as VCH_no,to_char(a.vchdate,'dd/mm/yyyy') as VCH_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,a.mrnnum,to_char(a.mrndate,'dd/mm/yyyy') as mrndate ,a.type as vch_type,to_char(a.vchdate,'yyyymmdd') as vdd,1 as qty from VOUCHER a,famst b,ATCHVCH C where trim(a.rcode)=trim(b.acode) AND A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=C.BRANCHCD||C.TYPE||TRIM(c.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY') and a.branchcd='" + frm_mbr + "' and a.type ='" + frm_vty + "' and a.vchdate " + DateRange + " and trim(nvl(C.MSGTXT,'-'))!='-' and a.srno=1 ";
                    }
                    if (hf_form_mode.Value == "INV")
                    {
                        SQuery = "SELECT distinct A.BRANCHCD||A.TYPE||TRIM(A.vchnum)||TO_cHAR(A.vchdate,'DD/MM/YYYY') AS FSTR,a.vchnum as Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Inv_Date,a.acode as Party_code,b.aname as Party_Name,a.Bill_tot as Inv_Tot_Amt FROM sale A,famst b, ATCHVCH c WHERE trim(a.acode)=trim(b.acode) and A.BRANCHCD='" + frm_mbr + "' AND A.TYPE ='" + edmode.Value + "' and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(c.branchcd)||trim(c.type)||trim(c.vchnum)||to_char(c.vchdate,'dd/mm/yyyy') and a.vchdate  " + DateRange + " and trim(nvl(C.MSGTXT,'-'))!='-' order by a.vchnum desc";
                    }
                    if (frm_formID == "F15473")
                    {
                        SQuery = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR,A.ORDNO AS ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORDDT,B.ANAME AS PARTY_NAME,A.PR_NO AS PR_NO,TO_CHAR(A.PR_DT,'DD/MM/YYYY') AS PR_DT,sum(a.qtyord) as Qty_order ,TO_CHAR(A.ORDDT,'yyyymmdd') as vdd from pomas a ,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + edmode.Value + "' and a.orddt " + DateRange + " and trim(nvl(a.atch1,'-'))!='-' group by A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY'),A.ORDNO ,TO_CHAR(A.ORDDT,'DD/MM/YYYY') ,B.ANAME ,A.PR_NO ,TO_CHAR(A.PR_DT,'DD/MM/YYYY'),TO_CHAR(A.ORDDT,'yyyymmdd') order by vdd desc,a.ordno desc";
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
    protected void btnview_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Edit";
        disp_data();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List_E";
        fgen.Fn_open_prddmp1("-", frm_qstr);
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
        {
            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "") != null || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "") != null || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "") != null)
            {
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
                col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

                switch (btnval)
                {
                    case "Edit":
                        edmode.Value = col1;
                        hffield.Value = "Edit_E";
                        disp_data();
                        fgen.Fn_open_sseek("-", frm_qstr);
                        break;
                    case "Edit_E":
                        SQuery = "Select a.*,b.aname,to_char(a.vchdate,'dd/mm/yyyy') as vcd,to_char(a.invdate,'dd/mm/yyyy') as ind from ivchctrl a,famst b where trim(A.acode)=trim(B.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + col1 + "')";
                        if (hf_form_mode.Value == "VCH")
                        {
                            SQuery = "Select a.*,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,(a.CRAMT+a.dramt) as bill_tot,b.aname,to_char(a.vchdate,'dd/mm/yyyy') as vcd,to_char(a.invdate,'dd/mm/yyyy') as ind from voucher a,famst b where trim(A.rcode)=trim(B.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + col1 + "') AND A.SRNO=1";
                        }

                        if (frm_formID == "F15473")
                        {
                            SQuery = "SELECT distinct A.ORDNO AS ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORDDT,B.ANAME AS aname,a.acode ,A.PR_NO AS PR_NO,TO_CHAR(A.PR_DT,'DD/MM/YYYY') AS PR_DT,a.atch1,TO_CHAR(A.ORDDT,'yyyymmdd') as vdd from pomas a , famst b where trim(a.acode)=trim(b.acode) and A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY')='" + col1 + "' order by vdd desc";
                            SQuery = "SELECT A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS FSTR,A.ORDNO AS ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORDDT,B.ANAME,a.acode,A.PR_NO AS PR_NO,TO_CHAR(A.PR_DT,'DD/MM/YYYY') AS PR_DT,sum(a.qtyord) as Qty_order ,TO_CHAR(A.ORDDT,'yyyymmdd') as vdd,a.atch1 from pomas a ,famst b where trim(a.acode)=trim(b.acode) and A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY')='" + col1 + "' and trim(nvl(a.atch1,'-'))!='-' group by A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY'),A.ORDNO ,TO_CHAR(A.ORDDT,'DD/MM/YYYY') ,B.ANAME ,A.PR_NO ,TO_CHAR(A.PR_DT,'DD/MM/YYYY'),TO_CHAR(A.ORDDT,'yyyymmdd'),a.acode,a.atch1 order by vdd desc,a.ordno desc";
                        }
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            if (frm_formID == "F15473")
                            {
                                txtvchnum.Text = dt.Rows[0]["ordno"].ToString().Trim(); txtvchdt.Text = dt.Rows[0]["orddt"].ToString().Trim();
                                txtbillno.Text = dt.Rows[0]["pr_no"].ToString().Trim(); txtbilldt.Text = dt.Rows[0]["pr_dt"].ToString().Trim();
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                                txttype.Text = edmode.Value.Trim(); txtamt.Text = dt.Rows[0]["Qty_order"].ToString().Trim();
                                txttypename.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='M' and type1='" + edmode.Value.Trim() + "'", "name").Trim();
                                create_tab(); edmode.Value = col1;
                                dr1 = dt1.NewRow(); dr1["srno"] = 1;
                                dr1["filno"] = dt.Rows[0]["atch1"].ToString().Trim();
                                dt1.Rows.Add(dr1); ViewState["sg1"] = dt1;
                                sg1.DataSource = dt1; sg1.DataBind();
                                disablectrl(); fgen.EnableForm(this.Controls);
                            }
                            else
                            {
                                txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim(); txtvchdt.Text = dt.Rows[0]["vcd"].ToString().Trim();
                                txtbillno.Text = dt.Rows[0]["invno"].ToString().Trim(); txtbilldt.Text = dt.Rows[0]["ind"].ToString().Trim();
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                                txttype.Text = edmode.Value.Trim();

                                if (hf_form_mode.Value == "MRR")
                                {
                                    txttypename.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='M' and type1='" + edmode.Value.Trim() + "'", "name").Trim();
                                }
                                if (hf_form_mode.Value == "VCH")
                                {
                                    txttypename.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='V' and type1='" + edmode.Value.Trim() + "'", "name").Trim();
                                }

                                txtamt.Text = dt.Rows[0]["bill_tot"].ToString().Trim();
                                create_tab(); edmode.Value = col1;
                                if (VCH_STYLE == "Y")
                                {
                                    DataTable dtX = new DataTable();
                                    dtX = fgen.getdata(frm_qstr, frm_cocd, "SELECT * FROM ATCHVCH WHERE BRANCHCD||TYPE||tRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + dt.Rows[0]["fstr"].ToString().Trim() + "' ");

                                    for (int i = 0; i < dtX.Rows.Count; i++)
                                    {

                                        dr1 = dt1.NewRow();
                                        dr1["srno"] = (i + 1);
                                        dr1["filno"] = dtX.Rows[i]["msgtxt"].ToString().Trim();
                                        dt1.Rows.Add(dr1);
                                    }
                                }
                                else
                                {
                                    dr1 = dt1.NewRow(); dr1["srno"] = 1;
                                    dr1["filno"] = dt.Rows[0]["IMAGEF"].ToString().Trim();
                                    dt1.Rows.Add(dr1);
                                }
                                ViewState["sg1"] = dt1;
                                sg1.DataSource = dt1; sg1.DataBind();
                                disablectrl(); fgen.EnableForm(this.Controls);
                            }
                        }
                        else clearctrl();
                        if (hf_form_mode.Value == "INV")
                        {
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
                                create_tab(); edmode.Value = col1;
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    dr1 = dt1.NewRow();
                                    dr1["srno"] = dt1.Rows.Count + 1;
                                    dr1["filno"] = dt.Rows[i]["msgtxt"].ToString().Trim();
                                    dt1.Rows.Add(dr1); ViewState["sg1"] = dt1;
                                }


                                sg1.DataSource = dt1; sg1.DataBind();
                                disablectrl(); fgen.EnableForm(this.Controls);
                            }
                        }

                        break;
                }
            }
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        if (hf_form_mode.Value == "PO")
        {
            SQuery = "SELECT A.ORDNO AS PO_NUMBER, TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS PO_DATE,A.PR_NO AS PR_NUMBER,TO_CHAR(A.PR_DT,'DD/MM/YYYY') AS PR_DATE,B.ANAME AS pARTY_NAME ,A.ACODE AS PARTY_CODE, A.QTYORD AS qTY_ORDER , A.TYPE AS po_TYPE,trim(a.icode) as Item_code,trim(c.iname) as item_name ,TO_CHAR(A.ORDDT,'yyyymmdd') AS VDD FROM POMAS A ,FAMST B,c.item WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '5%' AND A.ORDDT between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and nvl(a.ATCH1,'-')!='-' ORDER BY A.TYPE,VDD DESC ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of Uploded Purchase Orders Against PR", frm_qstr);
        }
        else if (hf_form_mode.Value == "INV")
        {
            SQuery = "SELECT a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,b.aname,a.type,c.name,a.bill_tot,e.msgtxt FROM sale a , famst b , type c , voucher d, ATCHVCH e WHERE trim(a.acode)=trim(b.acode) and trim(a.type)=trim(c.type1)  and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)=trim(d.branchcd)||trim(d.type)||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')||trim(d.acode) and c.id='V' and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(e.branchcd)||trim(e.type)||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type LIKE '4%' and a.vchdate between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List Of Uploaded Invoices", frm_qstr);
        }
        else
        {
            SQuery = "Select b.aname as party_name,a.vchnum as Vch_no,to_char(a.vchdate,'dd/mm/yyyy') as vch_Date,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_date,a.CRAMT+a.dramt as amt,a.mrnnum,to_char(a.mrndate,'dd/mm/yyyy') as mrndate ,a.type as vch_type,to_char(a.vchdate,'yyymmdd') as vdd from voucher a,famst b,ATCHVCH C where trim(a.acode)=trim(B.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=C.branchcd||C.type||trim(C.vchnum)||to_char(C.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type like '" + col1 + "%' and a.vchdate " + DateRange + "  and nvl(c.msgtxt,'-')!='-' and substr(a.acode,1,2) in ('06','16') order by a.type,vdd desc ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of Uploded Bills Against Voucher Entry", frm_qstr);
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
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        HCID = frm_formID;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].Style["display"] = "none";
            sg1.HeaderRow.Cells[0].Style["display"] = "none";
            ImageButton im = (ImageButton)e.Row.FindControl("btnrmv");
            im.Visible = false;
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
            fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "MRR", "MRR");
        }
        if (hf_form_mode.Value == "VCH")
        {
            col1 = "Select a.type,trim(substr(NVL(a.naration,'-'),1,220)) AS NARATION,nvl(a.mrnnum,'-') as mrnnum,NVL(a.mrndate,A.VCHDATE) AS MRNDATE,NVL(a.tax,'-') AS TAX,A.COSTCD,NVL(a.refnum,'-') AS REFNUM,NVL(a.invno,'-') AS INVNO,";
            col2 = " NVL(a.invdate,A.VCHDATE) AS INVDATE,nvl(a.CCENT,'-') as ccent,a.acode,a.rcode,a.vchnum,a.vchdate,nvl(a.app_by,'-') as app_by,nvl(a.app_date,a.vchdate) as app_Date,a.dramt,a.cramt,nvl(a.quantity,0)as quantity,NVL(a.refdate,A.VCHDATE) AS REFDATE,";
            col3 = " NVL(b.PERSON,'-') AS PERSON,NVL(b.aname,'-') AS ANAME,NVL(B.ANAME,'-') AS PARTY,a.ent_by,nvl(b.payment,'-') as pnm,a.tfcdr,a.tfccr,nvl(a.FCTYPE,'-') ";
            SQuery = col1 + col2 + col3 + " FCTYPE,c.name as VchTypeName from VOUCHER a left outer join famst b on TRIM(A.ACODE)=TRIM(B.ACODE) ,type c where a.type=c.type1 and c.id='V' and a.branchcd||a.type||a.VCHNUM||to_char(a.vchdate,'dd/mm/yyyy') ='" + (frm_mbr + txttype.Text.Trim() + txtvchnum.Text.Trim() + txtvchdt.Text.Trim()) + "' order by a.srno ";
            fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "vch_rpt", "vch_rpt");
        }
    }
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        switch (var)
        {
            case "Dwl":
                {
                    filePath = sg1.Rows[index].Cells[4].Text;

                    Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
                    Session["FileName"] = sg1.Rows[index].Cells[4].Text;
                    Response.Write("<script>");
                    Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                    Response.Write("</script>");
                }
                break;
            case "View":
                filePath = sg1.Rows[index].Cells[4].Text;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "c:/tej_erp/UPLOAD/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Tejaxo Viewer');", true);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Tejaxo Viewer');", true);
                break;
        }
    }
}