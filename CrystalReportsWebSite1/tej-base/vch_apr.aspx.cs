using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class vch_apr : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, fromdt, todt, year;
    string DateRange;
    string frm_mbr,  frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, frm_ulvl, frm_formID, frm_UserID;
    string merr = "0", HCID, VCH_STYLE = "N", filePath = "";
    DataTable dt, dt1; DataRow dr1;
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
                    fromdt = "01/04/" + year;
                    todt = "31/03/" + Convert.ToString(Convert.ToInt32(year) + 1);

                    if (frm_formID == "F70372") VCH_STYLE = "Y";
                    else VCH_STYLE = "N";
                    chk_f();
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl(); btnview.Focus(); btnapr.Disabled = true;
                if (frm_formID == "F15472")
                {
                    // txtvchnum.Visible=false;
                    //txtvchdt.Visible = false;
                    //txttype.Visible = false;
                    //txttypename.Visible = false;
                    //MrrNoDate.Visible = false;
                    //Label1.Visible = false;

                }
                set_val();
            }
            btnvchnum.Enabled = true;
            btnpodetails.Enabled = true;
            btnvchdetails.Enabled = true;
        }
    }
    public void enablectrl()
    {
        btnview.Disabled = false; btnvchnum.Enabled = false; btnpodetails.Enabled = false; btnvchdetails.Enabled = false;
        btnext.Visible = true; btncan.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
    }
    public void disablectrl()
    {
        btnview.Disabled = true; btnvchnum.Enabled = true; btnpodetails.Enabled = true; btnvchdetails.Enabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnext.Visible = false; btncan.Visible = true;
    }
    public void chk_f()
    {
        //merr = fgen.check_filed_name(co_cd, "IVCHCTRL", "IMAGEF");
        //if (merr == "0") fgen.execute_cmd(co_cd, "ALTER TABLE IVCHCTRL ADD IMAGEF VARCHAR(100) DEFAULT '-'");
    }
    void set_val()
    {
        if (frm_formID == "F15472")
        {
            hf_form_mode.Value = "PO";
            lbHeader.Text = "PO File Approve";
            voucher.InnerText = "PO No./ Date";
            MrrNoDate.InnerText = "PR No./ Date";
            vcher.InnerText = "PO Type/Name";
            Label1.InnerText = "PR Type/Name";
            txtbillno.Visible = false;
            txtbilldt.Visible = false;
            Label2.Visible = false;
            txtamt.Visible = false;
            Label3.Visible = false;
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
                break;
            case "MRR":
                SQuery = "Select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as mrr_no,to_char(a.vchdate,'dd/mm/yyyy') as mrr_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,to_char(a.vchdate,'yyyymmdd') as vdd from ivchctrl a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type ='" + edmode.Value + "' and a.vchdate " + DateRange + " and trim(nvl(a.imagef,'-'))='-' order by vdd desc";
                break;
            case "PO":
                SQuery = "select distinct a.branchcd||a.potype||trim(a.POnum)||to_char(a.podate,'dd/mm/yyyy') as fstr,a.PONUM as po_num,to_Char(a.podate,'dd/mm/yyyy') as PO_DT,b.aname as party,a.acode as code from ivoucher a, famst b where trim(a.acode)=trim(b.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + (frm_mbr + txttype.Text.Trim() + txtvchnum.Text.Trim() + txtvchdt.Text.Trim()) + "') order by a.PONUM desc,to_char(a.podate,'dd/mm/yyyy') desc";
                break;
            default:
                if (btnval == "Edit" || btnval == "Del" || btnval == "Print" || btnval == "List")
                {
                    SQuery = "SELECT type1 as fstr,name as mrr_type,type1 as code FROM TYPE WHERE ID='M' and type1 like '0%' order by type1";
                    if (VCH_STYLE == "Y") SQuery = "SELECT type1 as fstr,name as VCH_type,type1 as code FROM TYPE WHERE ID='V' order by type1";
                    else if (frm_formID == "F15472") { SQuery = "SELECT type1 as fstr,name as mrr_type,type1 as code FROM TYPE WHERE ID='M' and type1 like '5%' order by type1"; }
                }
                else if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                {
                    SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as mrr_no,to_char(a.vchdate,'dd/mm/yyyy') as mrr_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,to_char(a.vchdate,'yyyymmdd') as vdd from ivchctrl a,famst b,ivoucher c,voucher d where trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=c.branchcd||c.type||trim(c.vchnum)||to_char(c.vchdate,'dd/mm/yyyy') and d.branchcd||trim(d.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')=a.branchcd||substr(c.finvno,1,6)||substr(c.finvno,8,11) and d.type like '5%' and a.branchcd='" + frm_mbr + "' and a.type ='" + edmode.Value + "' and a.vchdate " + DateRange + " and trim(nvl(a.imagef,'-'))!='-' and trim(nvl(d.app_by,'-'))='-' order by vdd desc";
                    SQuery = "Select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as mrr_no,to_char(a.vchdate,'dd/mm/yyyy') as mrr_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,to_char(a.vchdate,'yyyymmdd') as vdd from ivchctrl a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type ='" + edmode.Value + "' and a.vchdate " + DateRange + " /*and trim(nvl(a.imagef,'-'))!='-'*/ order by vdd desc";
                    if (VCH_STYLE == "Y") SQuery = "Select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as VCH_no,to_char(a.vchdate,'dd/mm/yyyy') as VCH_dt,b.aname as party,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_dt,a.mrnnum,to_char(a.mrndate,'dd/mm/yyyy') as mrndate ,a.type as vch_type,to_char(a.vchdate,'yyyymmdd') as vdd from VOUCHER a,famst b,ATCHVCH C where trim(a.acode)=trim(b.acode) AND A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)=C.BRANCHCD||C.TYPE||TRIM(c.VCHNUM)||TO_CHAR(C.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE) and a.branchcd='" + frm_mbr + "' and a.type ='" + edmode.Value + "' and a.vchdate " + DateRange + " and trim(nvl(C.MSGTXT,'-'))!='-' and a.srno=1 and nvl(trim(a.app_by),'-')='-' order by vdd desc";
                    else if (frm_formID == "F15472")
                    {
                        SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr,a.ordno as PO_no,to_char(a.orddt,'dd/mm/yyyy') as PO_dt,b.aname as party_name,a.acode As party_code, a.pr_no , to_char(a.pr_dt,'dd/mm/yyyy') as pr_dt,to_char(a.orddt,'yyyymmdd') as vdd  from pomas a , famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + edmode.Value + "' and a.orddt " + DateRange + " and nvl(trim(a.atch1),'-')!='-' and nvl(trim(a.atch2),'-')='-' order by vdd desc";
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
    protected void btnapr_ServerClick(object sender, EventArgs e)
    {
        fgen.msg("-", "SMSG", "Are You Sure!! You want to Approve");
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
        // Response.Redirect("desktop.aspx");
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
            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "") != null || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "") != null || fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "") != null)
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
                        if (VCH_STYLE == "Y") SQuery = "Select a.*,(a.CRAMT+a.dramt) as bill_tot,C.MSGTXT AS IMAGEF,b.aname,to_char(a.vchdate,'dd/mm/yyyy') as vcd,to_char(a.invdate,'dd/mm/yyyy') as ind from voucher a,famst b,ATCHVCH C where trim(A.acode)=trim(B.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=C.branchcd||C.type||trim(C.vchnum)||to_char(C.vchdate,'dd/mm/yyyy') and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + col1 + "') and a.srno=1 ORDER BY C.MSGDT";
                        else if (frm_formID == "F15472")
                        {
                            SQuery = "select distinct a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr,a.ordno as ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,b.aname as aname,a.acode As acode, a.pr_no , to_char(a.pr_dt,'dd/mm/yyyy') as pr_dt,a.atch1 as atch1,to_char(a.orddt,'yyyymmdd') as vdd  from pomas a , famst b where trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')='" + col1 + "' order by vdd desc";
                        }
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            if (frm_formID == "F15472")
                            {
                                txtvchnum.Text = dt.Rows[0]["pr_no"].ToString().Trim(); txtvchdt.Text = dt.Rows[0]["pr_dt"].ToString().Trim();
                                txtvchnum0.Text = dt.Rows[0]["ordno"].ToString().Trim(); txtvchdt0.Text = dt.Rows[0]["orddt"].ToString().Trim();
                                txtbillno.Text = dt.Rows[0]["pr_no"].ToString().Trim(); txtbilldt.Text = dt.Rows[0]["pr_dt"].ToString().Trim();
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                                txttype.Text = edmode.Value.Trim(); txttypename.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='M' and type1='" + edmode.Value.Trim() + "'", "name").Trim();
                                txttype0.Text = edmode.Value.Trim(); txttypename0.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='M' and type1='" + edmode.Value.Trim() + "'", "name").Trim();
                                txtamt.Text = "-";
                                create_tab(); edmode.Value = col1;
                                dr1 = dt1.NewRow(); dr1["srno"] = 1;
                                dr1["filno"] = dt.Rows[0]["atch1"].ToString().Trim();
                                dt1.Rows.Add(dr1); ViewState["sg1"] = dt1;
                                sg1.DataSource = dt1; sg1.DataBind();
                                btnapr.Disabled = false;
                            }
                            else
                            {

                                txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim(); txtvchdt.Text = dt.Rows[0]["vcd"].ToString().Trim();
                                txtvchnum0.Text = dt.Rows[0]["vchnum"].ToString().Trim(); txtvchdt0.Text = dt.Rows[0]["vcd"].ToString().Trim();
                                txtbillno.Text = dt.Rows[0]["invno"].ToString().Trim(); txtbilldt.Text = dt.Rows[0]["ind"].ToString().Trim();
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                                txttype.Text = edmode.Value.Trim();
                                txttypename.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='M' and type1='" + edmode.Value.Trim() + "'", "name").Trim();
                                txtamt.Text = dt.Rows[0]["bill_tot"].ToString().Trim();
                                create_tab();
                                edmode.Value = col1;
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
                                    dr1 = dt1.NewRow(); dr1["srno"] = 1;
                                    dr1["filno"] = dt.Rows[0]["IMAGEF"].ToString().Trim();
                                    dt1.Rows.Add(dr1);
                                }
                                ViewState["sg1"] = dt1;
                                sg1.DataSource = dt1; sg1.DataBind();
                                if (VCH_STYLE != "Y")
                                {
                                    DataTable mdt = new DataTable();
                                    merr = fgen.seek_iname(frm_qstr, frm_cocd, "Select distinct nvl(finvno,'-') as finvno from ivoucher where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'", "finvno").Trim();
                                    mdt = fgen.getdata(frm_qstr, frm_cocd, "Select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, a.vchnum as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.type,b.name from voucher a,type b where trim(a.type)=trim(b.type1) and a.type like '5%' and trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + merr.Trim().Replace(" ", "") + "'");
                                    if (mdt.Rows.Count > 0)
                                    {
                                        txtvchnum0.Text = mdt.Rows[0]["vchnum"].ToString(); txtvchdt0.Text = mdt.Rows[0]["vchdate"].ToString();
                                        txttype0.Text = mdt.Rows[0]["type"].ToString(); txttypename0.Text = mdt.Rows[0]["name"].ToString();
                                        btnapr.Disabled = false;

                                        edmode.Value = mdt.Rows[0]["fstr"].ToString();
                                    }
                                    else fgen.msg("-", "AMSG", "No Voucher Made for Selected MRR");
                                }

                                else
                                {
                                    txttype0.Text = txttype.Text; txttypename0.Text = txttypename.Text;
                                    btnapr.Disabled = false;
                                }
                                disablectrl(); fgen.EnableForm(this.Controls);

                            }
                        }
                        else clearctrl();
                        break;
                    case "List":
                        if (frm_formID == "F70372")
                        {
                            SQuery = "Select b.aname as party_name,a.vchnum as Vch_no,to_char(a.vchdate,'dd/mm/yyyy') as vch_Date,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_date,a.CRAMT+a.dramt as amt,a.mrnnum,to_char(a.mrndate,'dd/mm/yyyy') as mrndate ,a.type as vch_type,to_char(a.vchdate,'yyymmdd') as vdd from voucher a,famst b,ATCHVCH C where trim(a.acode)=trim(B.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=C.branchcd||C.type||trim(C.vchnum)||to_char(C.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type like '" + col1 + "%' and a.vchdate " + DateRange + "  and nvl(c.msgtxt,'-')!='-' and substr(a.acode,1,2) in ('06','16') order by a.type,vdd desc ";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("List of Uploded Bills Against Voucher Entry", frm_qstr);

                        }
                        else if (frm_formID == "F15472")
                        {
                            SQuery = "select a.ordno,to_char(a.orddt,'dd/mm/yyyy') as PO_dt, a.pr_no as pr_No , to_char(a.pr_dt,'dd/mm/yyyy') as pr_date,a.acode as party_code,b.aname as party_name from pomas a , famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '" + col1 + "' and a.orddt" + DateRange + " and nvl(a.atch1,'-')!='-' and nvl(a.atch2,'-')!='-' ";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("List of Approved Uploded PO", frm_qstr);
                        }
                        else
                        {
                            SQuery = "Select a.vchnum as Mrr_no,to_char(a.vchdate,'dd/mm/yyyy') as Mrr_Date,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_date,b.aname as party_name,a.bill_tot as amt,a.mrnnum,to_char(a.mrndate,'dd/mm/yyyy') as mrndate ,a.type as vch_type,a.type as mrr_type,to_char(a.vchdate,'yyymmdd') as vdd from ivchctrl a,famst b where trim(a.acode)=trim(B.acodE) and a.branchcd='" + frm_mbr + "' and a.type like '" + col1 + "%' and a.vchdate " + DateRange + "  and nvl(a.IMAGEF,'-')!='-' order by a.type,vdd desc ";
                            //fgen.send_cookie("seekSql", SQuery);
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                            fgen.Fn_open_rptlevel("List of Approved Uploded Invoices Against MRR", frm_qstr);
                        }
                        break;
                    case "PO":
                        string mq0 = col1;
                        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from pomas a,famst d where trim(a.othac1)=trim(d.acode) and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in (" + mq0.Trim() + ") ", "aname");
                        col2 = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from pomas a,famst d where trim(a.othac2)=trim(d.acode) and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in (" + mq0.Trim() + ") ", "aname");
                        col3 = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from pomas a,famst d where trim(a.othac3)=trim(d.acode) and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in (" + mq0.Trim() + ") ", "aname");
                        SQuery = "select distinct a.branchcd,'" + col1 + "' as othacn1,'" + col2 + "'as othacn2,'" + col3 + "'as othacn3,a.pexc,a.type,a.ordno,a.DEL_MTH, to_char(a.orddt,'dd/mm/yyyy') as orddt,a.srno,a.icode,b.iname,b.cpartno,a.unit, a.ciname as cinm,a.desc_,a.qtyord,a.prate as rate,a.pdisc as disc,a.pexc as ed,a.pcess as cess,a.invno,a.ptax, to_char(a.del_date ,'dd/mm/yyyy') as delvdt , to_char(a.effdate ,'dd/mm/yyyy') as effdate,e.name AS PO_TYPE, (CASE WHEN trim(NVL(a.app_by,'-')) = '-' THeN 'DRAFT P.O.' else 'Purchase Order' end ) as App_status,   a.app_by,to_char(a.app_dt ,'dd/mm/yyyy') as app_dt,a.pamt,a.psize,a.acode,a.inst,a.term,a.qtysupp,a.qtybal,a.pordno, to_char(a.porddt ,'dd/mm/yyyy') as porddt,to_char(a.invdate ,'dd/mm/yyyy') as invdate,a.delivery,a.del_mth,a.del_wk, a.delv_term,to_char(a.refdate ,'dd/mm/yyyy') as refdate,a.mode_tpt,a.tr_insur,a.desp_to,a.freight,a.doc_thr,a.packing,a.payment,a.bank,a.stax,a.exc,a.iopr,a.pr_no as prnum,a.amd_no,a.del_sch as wono,a.wk1,a.wk2,a.wk3,a.wk4 as pnf,a.vend_wt,a.store_no,a.ent_by,to_char(a.ent_dt ,'dd/mm/yyyy') as ent_dt,a.splrmk as splr,d.aname,d.rc_num2 as pcstno,d.girno as ppanno,d.rc_num as ptinno,d.EXC_NUM as peccno,d.addr1 as caddr1,d.addr2 as caddr2,d.addr3 as caddr3,d.telnum as telephone,d.person as person,d.mobile as mobile,d.email as email, a.issue_no,a.pflag,to_char(a.pr_dt ,'dd/mm/yyyy') as prdate,a.test,a.pbasis,a.rate_ok,a.rate_cd,a.rate_rej,a.delv_item,a.transporter, a.st38no,a.nxtmth2,a.currency,a.remark,a.pexcamt as edr,a.pdiscamt as discr,a.amdtno,a.orignalbr,a.gsm,a.o_prate,a.o_qty,a.chl_ref,a.othac1,a.othac2,a.othac3,a.othamt1,a.othamt2,a.othamt3,a.st31no,a.d18no,a.tdisc_amt,a.cscode1,a.billcode,a.kindattn,a.prefsource,a.poprefix,a.rate_comm,a.splrmk,a.pdays,a.chk_by from pomas a, item b, famst d,type e where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(d.acode) and a.type=e.type1 and e.id='M' AND SUBSTR(e.TYPE1,1,1) IN ('5') AND e.TYPE1 <> '54' and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in (" + mq0.Trim() + ")  order by orddt,a.ordno,a.srno";
                        fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "POP", "POP");
                        break;
                }
            }
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        if (hffield.Value == "List_E")
        {
            SQuery = "Select a.vchnum as Mrr_no,to_char(a.vchdate,'dd/mm/yyyy') as Mrr_Date,a.invno as bill_no,to_Char(a.invdate,'dd/mm/yyyy') as bill_date,b.aname as party_name,a.bill_tot as amt,a.type as mrr_type,to_char(a.vchdate,'yyymmdd') as vdd from ivchctrl a,famst b where trim(a.acode)=trim(B.acodE) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate between to_date('" + Request.Cookies["Value1"].Value.ToString().Trim().Replace("&amp", "") + "','dd/mm/yyyy') and to_date('" + Request.Cookies["Value1"].Value.ToString().Trim().Replace("&amp", "") + "','dd/mm/yyyy') and nvl(a.IMAGEF,'-')!='-' order by a.type,vdd desc ";
            fgen.send_cookie("seekSql", SQuery);
            fgen.Fn_open_rptlevel("List of Uploded Invoices Against MRR", frm_qstr);
        }
        else
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                if (frm_formID == "F15472")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "update pomas set atch2='" + frm_uname + "',atch3=sysdate, APP_BY='" + frm_uname + "',APP_DT=SYSDATE where branchcd||type||trim(ordno)||to_Char(orddt,'dd/mm/yyyy')='" + edmode.Value.Trim() + "'");
                    enablectrl(); fgen.DisableForm(this.Controls); fgen.ResetForm(this.Controls); sg1.DataSource = null; sg1.DataBind(); btnapr.Disabled = true; clearctrl();
                    fgen.msg("-", "AMSG", "PO File Approval Successfully Done");
                }
                else
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "update voucher set app_by='" + frm_uname + "',app_date=sysdate where branchcd||type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')='" + edmode.Value.Trim() + "'");
                    enablectrl(); fgen.DisableForm(this.Controls); fgen.ResetForm(this.Controls); sg1.DataSource = null; sg1.DataBind(); btnapr.Disabled = true; clearctrl();
                    fgen.msg("-", "AMSG", "Voucher Approval Successfully Done");
                }
            }
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
        //col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NAME FROM TYPE WHERE ID='M' AND trim(TYPE1)='" + txttype.Text.Trim() + "'", "NAME");
        //SQuery = "select '" + col1 + "' as typename,a.icode,b.iname,a.unit,a.PNAME, a.iqty_chl,a.iqty_chlwt,a.ponum,TO_CHAr(a.podate,'DD/MM/YYYY') as podate,  a.iqtyin as act,a.acpt_ud,a.rej_rw, a.iqty_wt as actwt,a.irate as rate,a.iqtyin*a.irate as amt, a.vchnum,TO_CHAr(a.vchdate,'dd/MM/yyyy') as vchdate,a.branchcd,a.type,a.invno,TO_CHAr(a.invdate,'dd/MM/yyyy') as invdate,a.refnum,TO_CHAr(a.refdate,'dd/MM/yyyy') as refdate,a.acode,c.aname,c.addr1 AS CADDR1,c.addr2 AS CADDR2,c.addr3 AS CADDR3,C.EMAIL AS C_EMAIL,C.TELNUM,a.mode_tpt, a.genum,TO_CHAr(a.gedate,'dd/MM/yyyy') as gedate,a.cess_pu,a.location,a.naration,a.ent_by,TO_CHAr(a.ent_dt,'DD/MM/YYYY') as ent_dt from ivoucher a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') in ('" + (frm_mbr + txttype.Text.Trim() + txtvchnum.Text.Trim() + txtvchdt.Text.Trim()) + "') AND A.STORE<>'R' order by vchdate,a.vchnum, a.srno";
        //col1 = "select rvalue,amt_sale, amt_exc,shvalue,cust_amt,lessamt,cst_amt,ed_rate,lst_amt,lst_rate,frt_amt,pack_amt,insu_amt,other,excb_chg,VATSCHG from ivchctrl where BRANCHCD||TYPE||TRIM(vchnum)||TO_CHAr(vchdate,'DD/MM/YYYY') in ('" + (frm_mbr + txttype.Text.Trim() + txtvchnum.Text.Trim() + txtvchdt.Text.Trim()) + "')";
        //// fgen.Print_Report_2ds(co_cd, mbr, SQuery, "MRR", "MRR", col1, "rgpmst");
        //fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "MRR", "MRR");

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", txttype.Text.Trim());
        col1 = "'" + (txtvchnum.Text.Trim() + txtvchdt.Text.Trim()) + "'";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", col1);
        if (col1.Length < 2) return;
        col2 = "F1002";
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", col2);
        fgen.fin_invn_reps(frm_qstr);
    }
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        switch (var)
        {
            case "Dwl":
                if (e.CommandArgument.ToString().Trim() != "")
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
    protected void btnpodetails_Click(object sender, ImageClickEventArgs e)
    {
        if (frm_formID == "F15472")
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", txttype0.Text);
            col1 = "'" + txtvchnum0.Text + txtvchdt0.Text + "'";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", col1);
            if (col1.Length < 2) return;
            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1004");
            if (frm_cocd == "DREM") fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
            fgen.fin_purc_reps(frm_qstr);
        }
        dt = new DataTable();
        SQuery = "select distinct a.branchcd||a.potype||trim(a.POnum)||to_char(a.podate,'dd/mm/yyyy') as fstr,a.PONUM as po_num,to_Char(a.podate,'dd/mm/yyyy') as PO_DT,b.aname as party,a.acode as code from ivoucher a, famst b where trim(a.acode)=trim(b.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + (frm_mbr + txttype.Text.Trim() + txtvchnum.Text.Trim() + txtvchdt.Text.Trim()) + "') order by a.PONUM desc,to_char(a.podate,'dd/mm/yyyy') desc";
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows.Count > 1)
            {
                hffield.Value = "PO";
                disp_data();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
            else
            {
                //col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from pomas a,famst d where trim(a.othac1)=trim(d.acode) and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + dt.Rows[0]["fstr"].ToString() + "') ", "aname");
                //col2 = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from pomas a,famst d where trim(a.othac2)=trim(d.acode) and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + dt.Rows[0]["fstr"].ToString() + "') ", "aname");
                //col3 = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from pomas a,famst d where trim(a.othac3)=trim(d.acode) and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + dt.Rows[0]["fstr"].ToString() + "') ", "aname");
                //SQuery = "select distinct a.branchcd,'" + col1 + "' as othacn1,'" + col2 + "'as othacn2,'" + col3 + "'as othacn3,a.pexc,a.type,a.ordno,a.DEL_MTH, to_char(a.orddt,'dd/mm/yyyy') as orddt,a.srno,a.icode,b.iname,b.cpartno,a.unit, a.ciname as cinm,a.desc_,a.qtyord,a.prate as rate,a.pdisc as disc,a.pexc as ed,a.pcess as cess,a.invno,a.ptax, to_char(a.del_date ,'dd/mm/yyyy') as delvdt , to_char(a.effdate ,'dd/mm/yyyy') as effdate,e.name AS PO_TYPE, (CASE WHEN trim(NVL(a.app_by,'-')) = '-' THeN 'DRAFT P.O.' else 'Purchase Order' end ) as App_status,   a.app_by,to_char(a.app_dt ,'dd/mm/yyyy') as app_dt,a.pamt,a.psize,a.acode,a.inst,a.term,a.qtysupp,a.qtybal,a.pordno, to_char(a.porddt ,'dd/mm/yyyy') as porddt,to_char(a.invdate ,'dd/mm/yyyy') as invdate,a.delivery,a.del_mth,a.del_wk, a.delv_term,to_char(a.refdate ,'dd/mm/yyyy') as refdate,a.mode_tpt,a.tr_insur,a.desp_to,a.freight,a.doc_thr,a.packing,a.payment,a.bank,a.stax,a.exc,a.iopr,a.pr_no as prnum,a.amd_no,a.del_sch as wono,a.wk1,a.wk2,a.wk3,a.wk4 as pnf,a.vend_wt,a.store_no,a.ent_by,to_char(a.ent_dt ,'dd/mm/yyyy') as ent_dt,a.splrmk as splr,d.aname,d.rc_num2 as pcstno,d.girno as ppanno,d.rc_num as ptinno,d.EXC_NUM as peccno,d.addr1 as caddr1,d.addr2 as caddr2,d.addr3 as caddr3,d.telnum as telephone,d.person as person,d.mobile as mobile,d.email as email, a.issue_no,a.pflag,to_char(a.pr_dt ,'dd/mm/yyyy') as prdate,a.test,a.pbasis,a.rate_ok,a.rate_cd,a.rate_rej,a.delv_item,a.transporter, a.st38no,a.nxtmth2,a.currency,a.remark,a.pexcamt as edr,a.pdiscamt as discr,a.amdtno,a.orignalbr,a.gsm,a.o_prate,a.o_qty,a.chl_ref,a.othac1,a.othac2,a.othac3,a.othamt1,a.othamt2,a.othamt3,a.st31no,a.d18no,a.tdisc_amt,a.cscode1,a.billcode,a.kindattn,a.prefsource,a.poprefix,a.rate_comm,a.splrmk,a.pdays,a.chk_by from pomas a, item b, famst d,type e where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(d.acode) and a.type=e.type1 and e.id='M' AND SUBSTR(e.TYPE1,1,1) IN ('5') AND e.TYPE1 <> '54' and a.BRANCHCD||a.TYPE||TRIM(a.ordno)||TO_CHAr(a.orddt,'DD/MM/YYYY') in ('" + dt.Rows[0]["fstr"].ToString().Trim() + "') order by orddt,a.ordno,a.srno";
                //fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "POP", "POP");

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", dt.Rows[0]["fstr"].ToString().Trim().Substring(2, 2));
                col1 = "'" + dt.Rows[0]["fstr"].ToString().Trim().Substring(4, 16) + "'";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", col1);
                if (col1.Length < 2) return;
                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1004");
                if (frm_cocd == "DREM") fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
                fgen.fin_purc_reps(frm_qstr);
            }
        }
    }
    protected void btnvchdetails_Click(object sender, ImageClickEventArgs e)
    {
        if (frm_formID == "F70372")
        {
            col1 = "Select a.type,trim(substr(NVL(a.naration,'-'),1,220)) AS NARATION,nvl(a.mrnnum,'-') as mrnnum,NVL(a.mrndate,A.VCHDATE) AS MRNDATE,NVL(a.tax,'-') AS TAX,A.COSTCD,NVL(a.refnum,'-') AS REFNUM,NVL(a.invno,'-') AS INVNO,";
            col2 = " NVL(a.invdate,A.VCHDATE) AS INVDATE,nvl(a.CCENT,'-') as ccent,a.acode,a.rcode,a.vchnum,a.vchdate,nvl(a.app_by,'-') as app_by,nvl(a.app_date,a.vchdate) as app_Date,a.dramt,a.cramt,nvl(a.quantity,0)as quantity,NVL(a.refdate,A.VCHDATE) AS REFDATE,";
            col3 = " NVL(b.PERSON,'-') AS PERSON,NVL(b.aname,'-') AS ANAME,NVL(B.ANAME,'-') AS PARTY,a.ent_by,nvl(b.payment,'-') as pnm,a.tfcdr,a.tfccr,nvl(a.FCTYPE,'-') ";
            SQuery = col1 + col2 + col3 + " FCTYPE,c.name as VchTypeName from VOUCHER a left outer join famst b on TRIM(A.ACODE)=TRIM(B.ACODE) ,type c where a.type=c.type1 and c.id='V' and a.branchcd||a.type||a.VCHNUM||to_char(a.vchdate,'dd/mm/yyyy') ='" + (frm_mbr + txttype.Text.Trim() + txtvchnum.Text.Trim() + txtvchdt.Text.Trim()) + "' order by a.srno ";
            fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "vch_rpt", "vch_rpt");
        }
        else
        {
            merr = fgen.seek_iname(frm_qstr, frm_cocd, "Select distinct nvl(a.finvno,'-') as finvno from ivoucher a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in ('" + (frm_mbr + txttype.Text.Trim() + txtvchnum.Text.Trim() + txtvchdt.Text.Trim()) + "')", "finvno").Trim();
            if (merr == "-" || merr == "0") fgen.msg("-", "AMSG", "No Voucher Made for the Selected MRR");
            else
            {
                col1 = "Select a.type,trim(substr(NVL(a.naration,'-'),1,220)) AS NARATION,nvl(a.mrnnum,'-') as mrnnum,NVL(a.mrndate,A.VCHDATE) AS MRNDATE,NVL(a.tax,'-') AS TAX,A.COSTCD,NVL(a.refnum,'-') AS REFNUM,NVL(a.invno,'-') AS INVNO,";
                col2 = " NVL(a.invdate,A.VCHDATE) AS INVDATE,nvl(a.CCENT,'-') as ccent,a.acode,a.rcode,a.vchnum,a.vchdate,nvl(a.app_by,'-') as app_by,nvl(a.app_date,a.vchdate) as app_Date,a.dramt,a.cramt,nvl(a.quantity,0)as quantity,NVL(a.refdate,A.VCHDATE) AS REFDATE,";
                col3 = " NVL(b.PERSON,'-') AS PERSON,NVL(b.aname,'-') AS ANAME,NVL(B.ANAME,'-') AS PARTY,a.ent_by,nvl(b.payment,'-') as pnm,a.tfcdr,a.tfccr,nvl(a.FCTYPE,'-') ";
                SQuery = col1 + col2 + col3 + " FCTYPE,c.name as VchTypeName from VOUCHER a left outer join famst b on TRIM(A.ACODE)=TRIM(B.ACODE) ,type c where a.type=c.type1 and c.id='V' and a.branchcd||a.type||a.VCHNUM||to_char(a.vchdate,'dd/mm/yyyy') ='" + frm_mbr + "50" + merr.Replace(" ", "") + "' order by a.srno ";
                fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "vch_rpt", "vch_rpt");
            }
        }
    }
}