using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class oldmcdata : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, cstr, vchnum, vardate, fromdt, todt, year, cond = "", vip = "";
    string pk_error = "Y", chk_rights = "N", DateRange;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_tabname, frm_myear, frm_sql, frm_ulvl, frm_formID, frm_UserID;
    DataTable dt; DataRow oporow;
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
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl(); btnnew.Focus(); set_val();
            }
            myfun();
            if (frm_ulvl == "0") txtjobno.ReadOnly = false;
            btnprint.Visible = false;
        }
    }
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnprint.Disabled = false;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btninvno.Enabled = false;

        btncancel.Visible = false;
        btnexit.Visible = true;
    }
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnprint.Disabled = false;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btninvno.Enabled = true;

        btncancel.Visible = true;
        btnexit.Visible = false;
    }
    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
    }
    public void set_val()
    {
        frm_vty = "OD";
        if (frm_cocd == "CCEL")
        {
            spnjobno.Visible = true; txtjobno.Visible = true;
            tddivision.InnerText = "Department"; txttechper.Visible = false;
            lblheader.Text = "Customer Request"; tdtechnicalper.InnerText = "";
            tdinvoice.InnerText = "Entry No"; tdcomplaint.InnerText = "Req. No"; trextraval.Visible = false;
            tdtypcomplaint.InnerText = "Type of Request"; tdnaturcomplaint.InnerText = "Nature of Request";
            tdbatch1.Visible = false;
        }
        if (frm_cocd == "SRIS")
        {
            spnjobno.Visible = true; txtjobno.Visible = true; tdtechnicalper.InnerText = "Tech. Person"; txtjobno.Attributes.Add("Placeholder", "Ticket No.");
            spnjobno.InnerText = "Ticket No."; txtjobno.Width = 130; txttechper.Visible = true; trextraval.Visible = true;
            tdbatch1.Visible = true; txtinvbtch.ReadOnly = false;
            lblheader.Text = "Customer Complaint";
        }
        else
        {
            tdtechnicalper.InnerText = ""; txttechper.Visible = false;
            spnjobno.Visible = false; txtjobno.Visible = false; trextraval.Visible = false;
            lblheader.Text = "Old Machine Data Entry";
            tdcomplaint.InnerText = "Entry No";
            tdnaturcomplaint.InnerText = "Nature of Service";
            tddivision.InnerText = "Division of Service";
            tdbatch1.Visible = true;
        }

        if (frm_cocd == "SEL")
        {
            lblBatch.InnerText = "Machine Sr.No";

            txtinvbtch.ReadOnly = false;
            /*
            DivAddress.Visible = false;
            DivParty.Visible = false;
            txtinvno.Visible = false;*/
        }
    }
    public void disp_data()
    {
        set_val();
        btnval = hffield.Value;
        switch (btnval)
        {
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
                    }
                }
                break;
            case "TACODE":
                SQuery = "SELECT ACODE AS FSTR,ANAME AS CUSTOMER,ACODE AS CODE,ADDR1,ADDR2 FROM FAMST WHERE ACODE LIKE '16%' ORDER BY ACODE";
                break;
            case "TICODE":
                SQuery = "SELECT ICODE AS FSTR,INAME AS PRODUCT,CPARTNO,ICODE AS CODE,UNIT FROM ITEM WHERE LENGTH(TRIM(ICODE))>4 and substr(icode,1,1) in ('7','9') ORDER BY ICODE";
                break;
            default:
                if (btnval == "Edit" || btnval == "Del" || btnval == "Print" || btnval == "List")
                {
                    if (frm_ulvl != "0") cond = " and trim(a.ent_by)='" + frm_uname + "'";
                    SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr,a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,c.iname as item_name,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.vchnum as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='" + frm_vty + "' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " " + cond + " order by a.vchnum desc";
                    if (frm_cocd == "CCEL") SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr,a.col6 as job_no, a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as Req_no,to_char(a.vchdate,'dd/mm/yyyy') as Req_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,c.iname as item_name,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.vchnum as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='OD' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " " + cond + " order by a.vchnum desc";
                    if (frm_cocd == "SRIS") SQuery = "Select distinct a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode) as fstr,a.col6 as Ticket_no, a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as Req_no,to_char(a.vchdate,'dd/mm/yyyy') as Req_dt,a.invno as inv_no,to_char(a.invdate,'dd/mm/yyyy') as inv_Dt,b.aname as party_name,c.iname as item_name,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.vchnum as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.type='OD' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " order by a.vchnum desc";
                }
                break;
        }
        if (SQuery.Length > 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
    void fill_drop()
    {
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, "Select srno,name from typegrp where id='TC' and type1='000000' order by srno");
        ddntrofcmlnt.DataSource = dt;
        ddntrofcmlnt.DataTextField = "name";
        ddntrofcmlnt.DataValueField = "srno";
        ddntrofcmlnt.DataBind();

        DataTable dt1 = new DataTable();
        dt1 = fgen.getdata(frm_qstr, frm_cocd, "Select srno,name from typegrp where id='DC' and type1='000000' order by srno");
        dddivisioncmltn.DataSource = dt1;
        dddivisioncmltn.DataTextField = "name";
        dddivisioncmltn.DataValueField = "srno";
        dddivisioncmltn.DataBind();
    }
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "New";
        vchnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from scratch where branchcd='" + frm_mbr + "' AND TYPE='OD' ", 6, "vch");
        fgen.EnableForm(this.Controls); disablectrl();
        btninvno.Focus();
        txtvchnum.Text = vchnum;
        txtvchdate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
        if (frm_cocd == "SRIS")
        {
            if (frm_mbr == "00") cond = "U2"; else cond = "U6";
            txtjobno.Text = vchnum + "/" + DateTime.Now.ToString("ddMMyyyy") + "/" + cond;
        }
        //fill_drop();
        btnAcode.Focus();
    }
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Edit";
        disp_data();
        if (frm_cocd == "CCEL") fgen.Fn_open_sseek("Select Your Request", frm_qstr);
        else fgen.Fn_open_sseek("Select Your " + lblheader.Text + "", frm_qstr);
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        //cal();
        fgen.fill_dash(this.Controls);
        if (txtacode.Text.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Customer!!");
            return;
        }
        if (txticode.Text.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Product!!");
            return;
        }
        if (txtinvno.Text.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Enter Invoice No!!");
            return;
        }
        if (txtinvbtch.Text.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Enter Machine Sr No!!");
            return;
        }
        if (txtGur.Text.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Enter Guaranty/Warranty Terms!!");
            return;
        }

        fgen.msg("-", "SMSG", "Are you sure!! you want to save");

    }
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Del";
        disp_data();
        if (frm_cocd == "CCEL") fgen.Fn_open_sseek("Select Your Request", frm_qstr);
        else fgen.Fn_open_sseek("Select Your " + lblheader.Text + "", frm_qstr);
    }
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Print";
        disp_data();
        if (frm_cocd == "CCEL") fgen.Fn_open_sseek("Select Your Request", frm_qstr);
        else fgen.Fn_open_sseek("Select Your " + lblheader.Text + "", frm_qstr);
    }
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        if (frm_cocd == "SEL") fgen.Fn_open_prddmp1("-", frm_qstr);
        else
        {
            disp_data();
            if (frm_cocd == "CCEL") fgen.Fn_open_sseek("Select Your Request", frm_qstr);
            else fgen.Fn_open_sseek("Select Your " + lblheader.Text + "", frm_qstr);
        }
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;

        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();

            if (col1 == "Y")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from scratch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||TRIM(a.acode)||TRIM(a.icode)='" + edmode.Value + "'");
                fgen.msg("-", "AMSG", "Details are deleted for " + lblheader.Text + " No. " + edmode.Value.Substring(4, 6) + "");
                clearctrl(); fgen.ResetForm(this.Controls);
            }
        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
            {
                switch (btnval)
                {
                    case "Inv":
                        dt = new DataTable();
                        SQuery = "SELECT B.ANAME,b.addr1||','||b.addr2||','||b.addr3 as address ,C.INAME ,a.*  FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) and A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) in ('" + col1 + "')";
                        if (frm_cocd == "CCEL") SQuery = "SELECT A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS vchdate,B.ANAME ,A.ACODE ,C.INAME ,C.ICODE ,a.col1 FROM SCRATCH A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODe) and A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ACODE)||TRIM(A.ICODE) in ('" + col1 + "')";
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
                                txtGur.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT WEIGHT FROM SOMAS WHERE BRANCHCD||TYPE||TRIM(ORDNO)||TO_cHAR(ORDDT,'DD/MM/YYYY')||trim(icode)='" + dt.Rows[0]["branchcd"].ToString().Trim() + dt.Rows[0]["type"].ToString().Trim() + dt.Rows[0]["ponum"].ToString().Trim() + Convert.ToDateTime(dt.Rows[0]["podate"].ToString().Trim()).ToString("dd/MM/yyyy") + dt.Rows[0]["icode"].ToString().Trim() + "' ", "weight");
                                txtGurDate.Text = txtvchdate.Text.Trim();
                            }
                            txtPaddr.Text = dt.Rows[0]["address"].ToString().Trim();
                        }
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "Select srno,name as app,'-' as rmk from typegrp where id='CM' and type1='000000' order by srno");
                        sg1.DataSource = dt;
                        sg1.DataBind();

                        if (frm_cocd == "SEL")
                        {
                        }
                        break;
                    case "TACODE":
                        txtacode.Text = col1;
                        txtaname.Text = col2;
                        txtPaddr.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                        btnIcode.Focus();
                        break;
                    case "TICODE":
                        txticode.Text = col1;
                        txtiname.Text = col2;
                        txtinvno.Focus();
                        break;
                    case "Del":
                        if (col1 == "") return;
                        clearctrl();
                        edmode.Value = col1;
                        fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                        hffield.Value = "D";
                        break;
                    case "Edit":
                        if (col1 == "") return;
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "Select distinct a.vchnum as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.INVNO AS pono ,to_char(a.INVDATE,'dd/mm/yyyy') AS podate ,b.aname ,a.acode ,c.iname ,a.icode ,a.srno,a.COL1 as app,a.COL2,a.COL3,a.COL4,a.col6,a.col7,a.col8,a.col9,a.col10,a.COL12,a.COL13,a.REMARKS as rmk,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt,a.naration,a.num1,a.num2,a.num3,a.num4,a.num5,a.num6 from scratch a ,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' order by a.srno");
                        ViewState["fstr"] = col1;
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString(); txtvchdate.Text = dt.Rows[0]["vchdate"].ToString();
                        txtinvno.Text = dt.Rows[0]["pono"].ToString(); txtinvdate.Text = dt.Rows[0]["podate"].ToString();
                        txtacode.Text = dt.Rows[0]["acode"].ToString(); txtaname.Text = dt.Rows[0]["aname"].ToString();
                        txticode.Text = dt.Rows[0]["icode"].ToString(); txtiname.Text = dt.Rows[0]["iname"].ToString();
                        txtinvbtch.Text = dt.Rows[0]["COL8"].ToString();
                        txtGur.Text = dt.Rows[0]["COL12"].ToString();
                        txtGurDate.Text = dt.Rows[0]["COL13"].ToString();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString(); ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        fgen.EnableForm(this.Controls); disablectrl();
                        edmode.Value = "Y";
                        break;
                    case "List":
                        if (frm_ulvl == "0") cond = " and trim(a.ent_by)='" + frm_uname.Trim() + "'";
                        SQuery = "Select distinct a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt, a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' " + cond + " order by vdd desc,a.srno";
                        if (frm_cocd == "CCEL") SQuery = "Select distinct a.col2 as type_of_req,a.col3 as ntr_of_req,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as req_no,to_Char(a.vchdate,'dd/mm/yyyy') as req_dt, a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' " + cond + " order by vdd desc,a.srno";
                        if (frm_cocd == "SRIS") SQuery = "Select distinct a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt,a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,a.num1 as tpt_amt,a.num2 as lodging_amt,a.num3 as fooding_amt,a.num4 as misc_amt,a.num5 as Tot_amt,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' /*" + cond + "*/ order by vdd desc,a.srno";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                        if (frm_cocd == "CCEL") fgen.Fn_open_rptlevel("Request List", frm_qstr);
                        else fgen.Fn_open_rptlevel("" + lblheader.Text + " List", frm_qstr);
                        break;
                    case "Print":
                        if (frm_cocd == "NEOP")
                        {
                            SQuery = "Select distinct a.vchnum as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.INVNO AS pono ,TO_CHAR(a.invdate,'dd/mm/yyyy') as podate ,b.aname ,a.acode ,c.iname ,a.icode ,a.srno,a.col1 as app,a.col6,a.remarks as rmk,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_Dt from scratch a ,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' order by a.srno";
                            fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "neopcmplnt", "neopcmplnt");
                        }
                        else
                        {
                            SQuery = "Select distinct a.*,b.aname,b.addr1 as paddr1,b.addr2 as paddr2,b.email as pemail,c.iname,c.cpartno,d.iqtyout  from scratch a ,famst b,item c,ivoucher D where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.branchcd||trim(A.invno)||to_char(a.invdate,'dd/mm/yyyy')||trim(a.acode)||TRIM(a.icode)=D.branchcd||trim(D.vchnum)||to_char(d.vchdate,'dd/mm/yyyy')||trim(D.acode)||TRIM(D.icode) and d.type like '4%' and a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)='" + col1 + "' order by a.srno";
                            if (frm_cocd == "SRIS") fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "cmplntsris", "cmplntsris");
                            else fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "cmplnt", "cmplnt");
                        }
                        break;
                }
            }
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        col1 = "";
        if (hffield.Value == "List")
        {
            DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            if (frm_ulvl == "0") SQuery = "Select distinct a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt,a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.vchdate " + DateRange + " and a.type='OD' order by vdd desc,a.srno";
            else SQuery = "Select distinct a.vchnum||(case when nvl(a.app_by,'-')='-' then ' UnApproved' when substr(a.app_by,1,3)='[A]' then ' Approved' else ' Refused' end) as complnt_no,to_Char(a.vchdate,'dd/mm/yyyy') as complnt_dt,a.invno as pono,to_char(a.invdate,'dd/mm/yyyy') as podate,b.aname as party,a.acode as code,c.iname as product,c.icode as erpcode,a.col2 as type_of_complnt,a.col3 as ntr_of_complnt,a.srno,a.col1 as app,a.remarks as rmk,a.naration,a.ent_by,to_Char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from scratch a,famst b,item c where trim(a.acodE)=trim(b.acodE) and trim(a.icodE)=trim(C.icodE) and a.vchdate " + DateRange + " and a.type='OD' and trim(a.ent_by)='" + frm_uname.Trim() + "' order by vdd desc,a.srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("" + lblheader.Text + " List", frm_qstr);
        }
        else
        {
            if (frm_cocd == "SRIS") { col1 = "Y"; fgen.send_cookie("REPLY", "Y"); }
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                if (edmode.Value == "Y") fgen.execute_cmd(frm_qstr, frm_cocd, "update scratch set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(acode)||trim(icode)='" + ViewState["fstr"].ToString().Trim() + "'");


                DataSet oDS = new DataSet();
                oDS = fgen.fill_schema(frm_qstr, frm_cocd, "SCRATCH");

                if (edmode.Value == "Y") vchnum = txtvchnum.Text.Trim();
                else
                {
                    vchnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum) as vch from scratch where branchcd='" + frm_mbr + "' AND TYPE='OD' ", 6, "vch");
                    if (frm_cocd == "SRIS")
                    {
                        if (frm_mbr == "00") cond = "U2"; else cond = "U6";
                        txtjobno.Text = vchnum + "/" + DateTime.Now.ToString("ddMMyyyy") + "/" + cond;
                    }
                }

                //foreach (GridViewRow r1 in sg1.Rows)
                {
                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = frm_mbr;
                    oporow["TYPE"] = "OD";
                    oporow["vchnum"] = vchnum;
                    oporow["vchdate"] = txtvchdate.Text.Trim();
                    oporow["INVNO"] = txtinvno.Text.Trim();
                    oporow["INVDATE"] = txtinvdate.Text.Trim();
                    oporow["acode"] = txtacode.Text.Trim();
                    oporow["icode"] = txticode.Text.Trim();
                    oporow["srno"] = 1;

                    oporow["COL8"] = txtinvbtch.Text.Trim();
                    oporow["COL12"] = txtGur.Text.Trim();
                    oporow["COL13"] = txtGurDate.Text.Trim();

                    if (edmode.Value == "Y")
                    {
                        oporow["app_by"] = "-";
                        oporow["app_dt"] = DateTime.Now;

                        oporow["chk_by"] = "-";
                        oporow["chk_dt"] = DateTime.Now;
                        oporow["eNt_by"] = ViewState["entby"].ToString();
                        oporow["eNt_dt"] = ViewState["entdt"];
                        oporow["edt_by"] = frm_uname;
                        oporow["edt_dt"] = DateTime.Now;
                    }
                    else
                    {
                        oporow["app_by"] = "-";
                        oporow["app_dt"] = DateTime.Now;

                        oporow["chk_by"] = "-";
                        oporow["chk_dt"] = DateTime.Now;
                        oporow["eNt_by"] = frm_uname;
                        oporow["eNt_dt"] = DateTime.Now;
                        oporow["edt_by"] = "-";
                        oporow["eDt_dt"] = DateTime.Now;
                    }
                    oDS.Tables[0].Rows.Add(oporow);
                }
                fgen.save_data(frm_qstr, frm_cocd, oDS, "SCRATCH");

                col3 = "";
                if (edmode.Value == "Y") { fgen.msg("-", "AMSG", "Data Updated Successfully"); fgen.execute_cmd(frm_qstr, frm_cocd, "delete from scratch where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + ViewState["fstr"].ToString().Substring(2, 18) + "' "); }
                else { fgen.msg("-", "AMSG", "Data Saved Successfully"); }
                fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                ViewState["sg1"] = null;
                sg1.DataSource = null;
                sg1.DataBind();
            }
        }
    }
    protected void btninvno_Click(object sender, ImageClickEventArgs e)
    {
        clearctrl();
        hffield.Value = "Inv";
        disp_data();
        if (frm_cocd == "CCEL") fgen.Fn_open_sseek("Select Job No.", frm_qstr);
        else fgen.Fn_open_sseek("Select Inovice No.", frm_qstr);
    }
    public void myfun()
    {
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
    public void cal()
    {
        fgen.fill_zero(this.Controls);
        txttot.Text = Convert.ToString(Math.Round(Convert.ToDouble(txttpt.Text.Trim()) + Convert.ToDouble(txtlodging.Text.Trim()) + Convert.ToDouble(txtfooding.Text.Trim()) + Convert.ToDouble(txtmisc.Text.Trim())));
    }
    protected void btncancel_ServerClick(object sender, EventArgs e)
    {
        // for cancel button working
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();
        sg1.DataSource = null;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false;
        ViewState["sg1"] = null;
    }
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        // for exit button working
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }
    protected void btnAcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        disp_data();
        fgen.Fn_open_sseek("Select Customer", frm_qstr);
    }
    protected void btnIcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        disp_data();
        fgen.Fn_open_sseek("Select Product", frm_qstr);
    }
}