using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class cost_corr : System.Web.UI.Page
{
    int dhd;
    string SQuery, cstr, cond = "", vip = "";
    string pk_error = "Y", chk_rights = "N";
    string frm_mbr, btnval, constr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_tabname, frm_myear, frm_sql, frm_ulvl, frm_formID, frm_UserID;
    string fromdt, todt, xprdRange, DateRange, vchnum, vardate, col1, col2, col3, year;
    string ulvl, merr = "0", HCID, VCH_STYLE = "N";
    DataTable dt1; DataRow dr1;
    DataTable dt; DataSet oDS = new DataSet();
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
                    btnnew.Focus();

                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                    frm_cocd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    fromdt = "01/04/" + frm_myear;
                    todt = "31/03/" + Convert.ToString(Convert.ToInt32(frm_myear) + 1);
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    DateRange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";
                    // vardate = fgen.vardate(co_cd, year);                                  
                    chk_tab();
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl(); allow_txt();
            }
        }
    }
    public void allow_txt()
    {
        if (frm_cocd == "PION" || frm_cocd == "DEMO")
        {
            //tr1.Visible = false;          // tr2.Visible = false;
            txtpt1.Visible = true; tl1.Visible = true; tl2.Visible = true; tl3.Visible = true;
            lbl1.Text = "Length"; lbl2.Text = "Breadth"; lbl3.Text = "Height"; lbl4.Text = "PLY"; lbl5.Text = "UPS";
            lbl6.Text = "Printing"; lbl7.Text = "Joints"; lbl8.Text = "Pins"; lbl9.Text = "Wastage%"; lbl10.Text = "Gross Sheet Wt.";
            lbl11.Text = "Area/PC"; lbl15.Text = "T.Rct"; lbl16.Text = "Cost Paper"; lbl17.Text = "Deckle"; lbl18.Text = "Length"; lbl19.Text = "B.S"; lbl20.Text = "ECT";
            lbl23.Text = "Req. B.S."; lbl24.Text = "Req. ECT"; lbl24.Text = "Caliper"; lbl26.Text = "C.S.";
            txtpt41.Visible = false; txtpt55.Visible = false; txtpt59.Visible = false; txtpt76.Visible = false; txtpt77.Visible = false;
            lbl27.Text = ""; lbl28.Text = ""; lbl29.Text = ""; lbl30.Text = ""; lbl31.Text = ""; lbl32.Text = ""; txtpt101.Visible = false; txtpt102.Visible = false; txtpt103.Visible = false; txtpt104.Visible = false; txtpt105.Visible = false; txtpt107.Visible = false; txtpt106.Visible = false;
        }
        else
        {
            txtpt1.Visible = false; lbl12.Text = "Size_ID"; lbl13.Text = "Allow_ID"; lbl14.Text = "Size_OD";
            lbl2.Text = "Length(mm)"; lbl3.Text = "Breadth(mm)"; lbl4.Text = "Height(mm)"; lbl5.Text = "PLY"; lbl6.Text = "Allow_Deckle";
            lbl7.Text = "Allow_Length"; lbl8.Text = "Joints"; lbl9.Text = "Pins"; lbl10.Text = "Wastage%"; lbl11.Visible = false;
            tl1.Visible = false; tl2.Visible = false; tl3.Visible = false;
            lbl1.Text = ""; txtpt10.ReadOnly = false;
            lbl15.Text = "T.Gsm/T.Rct"; lbl16.Text = "Medium_Wt(g)"; lbl17.Text = "Paper_Rate(Kg)"; lbl18.Text = "Cost/Medium"; lbl19.Visible = false; lbl20.Visible = false;
            lbl23.Visible = false; lbl24.Visible = false; lbl25.Visible = false; lbl26.Visible = false;
            trct1.Visible = false; trct2.Visible = false; trct3.Visible = false; trct4.Visible = false; trct5.Visible = false; trct6.Visible = false; trct7.Visible = false; trct8.Visible = false;
            Td1.Visible = true; Td2.Visible = true; Td3.Visible = true; Td4.Visible = true; Td5.Visible = true; Td6.Visible = true; Td7.Visible = true; Td8.Visible = true;
            lbl32.Text = ""; lbl27.Text = ""; lbl28.Text = ""; lbl29.Text = ""; lbl30.Text = ""; lbl31.Text = ""; txtpt101.Visible = false; txtpt102.Visible = false; txtpt103.Visible = false; txtpt104.Visible = false; txtpt105.Visible = false; txtpt107.Visible = false; txtpt106.Visible = false;

            if (frm_cocd == "BEST" || frm_cocd == "PACT" || frm_cocd == "ECPL")
            {
                lbl28.Text = "";
                txtpt102.Visible = true;
                txtpt102.BackColor = System.Drawing.Color.White;
                txtpt107.Visible = true;
                txtpt107.BackColor = System.Drawing.Color.White;
                txtpt103.Visible = true;
                txtpt103.BackColor = System.Drawing.Color.White;

                lbl28.Text = "Conv_Cost/Box";
                lbl32.Text = "Total_Cost/Box";
                lbl29.Text = "Total_Cost/Kg";
            }

            txtaname.ReadOnly = false;
        }
    }
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btnprnt.Disabled = false; btndel.Disabled = false;
        btnext.Text = "Exit"; btnext.Enabled = true; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btninv.Enabled = false; btnso.Enabled = false;
        btnicode.Enabled = false; btnacode.Enabled = false;
    }
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btnprnt.Disabled = true; btndel.Disabled = true;
        btnext.Text = "Cancel"; btnext.Enabled = true; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnicode.Enabled = true; btnacode.Enabled = true; btninv.Enabled = true; btnso.Enabled = true;
    }
    public void chk_tab()
    {
        btnval = fgen.seek_iname(frm_qstr, frm_cocd, "Select tname from tab where tname like 'SOMAS_ANX%'", "tname");
        if (btnval == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "CREATE TABLE SOMAS_ANX ( BRANCHCD CHAR(2) DEFAULT '-',TYPE CHAR(2) DEFAULT '-',VCHNUM  CHAR(6) DEFAULT '-',VCHDATE DATE DEFAULT SYSDATE,CDRGNO CHAR(10) DEFAULT '-',ACODE CHAR(10) DEFAULT '-',ICODE CHAR(10) DEFAULT '-',T1 VARCHAR2(30) DEFAULT '-',T2 VARCHAR2(30) DEFAULT '-',T3 VARCHAR2(30) DEFAULT '-',T4 VARCHAR2(30) DEFAULT '-',T5 VARCHAR2(30) DEFAULT '-',T6 VARCHAR2(30) DEFAULT '-',T7 VARCHAR2(30) DEFAULT '-',T8 VARCHAR2(30) DEFAULT '-'," +
        "T9 VARCHAR2(30) DEFAULT '-', T10 VARCHAR2(30) DEFAULT '-', T11 VARCHAR2(30) DEFAULT '-', T12 VARCHAR2(30) DEFAULT '-', T13 VARCHAR2(30) DEFAULT '-', T14 VARCHAR2(30) DEFAULT '-', T15 VARCHAR2(30) DEFAULT '-', T16 VARCHAR2(30) DEFAULT '-', T17 VARCHAR2(30) DEFAULT '-', T18 VARCHAR2(30) DEFAULT '-', T19 VARCHAR2(30) DEFAULT '-', T20 VARCHAR2(30) DEFAULT '-', T21 VARCHAR2(30) DEFAULT '-', T22 VARCHAR2(30) DEFAULT '-', T23 VARCHAR2(30) DEFAULT '-', T24 VARCHAR2(30) DEFAULT '-', T25 VARCHAR2(30) DEFAULT '-'," +
        "T26 VARCHAR2(30) DEFAULT '-', T27 VARCHAR2(30) DEFAULT '-', T28 VARCHAR2(30) DEFAULT '-', T29 VARCHAR2(30) DEFAULT '-', T30 VARCHAR2(30) DEFAULT '-', T31 VARCHAR2(30) DEFAULT '-', T32 VARCHAR2(30) DEFAULT '-', T33 VARCHAR2(30) DEFAULT '-', T34 VARCHAR2(30) DEFAULT '-', T35 VARCHAR2(30) DEFAULT '-', T36 VARCHAR2(30) DEFAULT '-', T37 VARCHAR2(30) DEFAULT '-', T38 VARCHAR2(30) DEFAULT '-', T39 VARCHAR2(30) DEFAULT '-', T40 VARCHAR2(30) DEFAULT '-',T41 VARCHAR2(30) DEFAULT '-', T42 VARCHAR2(30) DEFAULT '-', T43 VARCHAR2(30) DEFAULT '-'," +
        "T44 VARCHAR2(30) DEFAULT '-', T45 VARCHAR2(30) DEFAULT '-', T46 VARCHAR2(30) DEFAULT '-', T47 VARCHAR2(30) DEFAULT '-', T48 VARCHAR2(30) DEFAULT '-', T49 VARCHAR2(30) DEFAULT '-', T50 VARCHAR2(30) DEFAULT '-', T51 VARCHAR2(30) DEFAULT '-', T52 VARCHAR2(30) DEFAULT '-', T53 VARCHAR2(30) DEFAULT '-', T54 VARCHAR2(30) DEFAULT '-', T55 VARCHAR2(30) DEFAULT '-', T56 VARCHAR2(30) DEFAULT '-', T57 VARCHAR2(30) DEFAULT '-', T58 VARCHAR2(30) DEFAULT '-', T59 VARCHAR2(30) DEFAULT '-', T60 VARCHAR2(30) DEFAULT '-', T61 VARCHAR2(30) DEFAULT '-', T62 VARCHAR2(30) DEFAULT '-'," +
        "T63 VARCHAR2(30) DEFAULT '-', T64 VARCHAR2(30) DEFAULT '-', T65 VARCHAR2(30) DEFAULT '-', T66 VARCHAR2(30) DEFAULT '-', T67 VARCHAR2(30) DEFAULT '-', T68 VARCHAR2(30) DEFAULT '-', T69 VARCHAR2(30) DEFAULT '-', T70 VARCHAR2(30) DEFAULT '-', T71 VARCHAR2(30) DEFAULT '-', T72 VARCHAR2(30) DEFAULT '-', T73 VARCHAR2(30) DEFAULT '-', T74 VARCHAR2(30) DEFAULT '-', T75 VARCHAR2(30) DEFAULT '-', T76 VARCHAR2(30) DEFAULT '-', T77 VARCHAR2(30) DEFAULT '-', T78 VARCHAR2(30) DEFAULT '-', T79 VARCHAR2(30) DEFAULT '-', T80 VARCHAR2(30) DEFAULT '-', T81 VARCHAR2(30) DEFAULT '-'," +
        "T82 VARCHAR2(30) DEFAULT '-', T83 VARCHAR2(30) DEFAULT '-', T84 VARCHAR2(30) DEFAULT '-', T85 VARCHAR2(30) DEFAULT '-', T86 VARCHAR2(30) DEFAULT '-', T87 VARCHAR2(30) DEFAULT '-', T88 VARCHAR2(30) DEFAULT '-', T89 VARCHAR2(30) DEFAULT '-', T90 VARCHAR2(30) DEFAULT '-', T91 VARCHAR2(30) DEFAULT '-', T92 VARCHAR2(30) DEFAULT '-', T93 VARCHAR2(30) DEFAULT '-', T94 VARCHAR2(30) DEFAULT '-', T95 VARCHAR2(30) DEFAULT '-', T96 VARCHAR2(30) DEFAULT '-', T97 VARCHAR2(30) DEFAULT '-', T98 VARCHAR2(30) DEFAULT '-', T99 VARCHAR2(30) DEFAULT '-', T100 VARCHAR2(30) DEFAULT '-'," +
        "T101 VARCHAR2(30) DEFAULT '-', T102 VARCHAR2(30) DEFAULT '-', T103 VARCHAR2(30) DEFAULT '-', T104 VARCHAR2(30) DEFAULT '-', T105 VARCHAR2(30) DEFAULT '-', T106 VARCHAR2(30) DEFAULT '-', T107 VARCHAR2(30) DEFAULT '-', T108 VARCHAR2(30) DEFAULT '-', T109 VARCHAR2(30) DEFAULT '-', T110 VARCHAR2(30) DEFAULT '-', T111 VARCHAR2(30) DEFAULT '-', T112 VARCHAR2(30) DEFAULT '-', T113 VARCHAR2(30) DEFAULT '-', T114 VARCHAR2(30) DEFAULT '-', T115 VARCHAR2(30) DEFAULT '-', T116 VARCHAR2(30) DEFAULT '-', T117 VARCHAR2(30) DEFAULT '-', T118 VARCHAR2(30) DEFAULT '-', T119 VARCHAR2(30) DEFAULT '-'," +
        "T120 VARCHAR2(30) DEFAULT '-', T121 VARCHAR2(30) DEFAULT '-', T122 VARCHAR2(30) DEFAULT '-', T123 VARCHAR2(30) DEFAULT '-', T124 VARCHAR2(30) DEFAULT '-', T125 VARCHAR2(30) DEFAULT '-', T126 VARCHAR2(30) DEFAULT '-', T127 VARCHAR2(30) DEFAULT '-', T128 VARCHAR2(30) DEFAULT '-', T129 VARCHAR2(30) DEFAULT '-', T130 VARCHAR2(30) DEFAULT '-', T131 VARCHAR2(30) DEFAULT '-', T132 VARCHAR2(30) DEFAULT '-', T133 VARCHAR2(30) DEFAULT '-', T134 VARCHAR2(30) DEFAULT '-', T135 VARCHAR2(30) DEFAULT '-', T136 VARCHAR2(30) DEFAULT '-', T137 VARCHAR2(30) DEFAULT '-', T138 VARCHAR2(30) DEFAULT '-'," +
        "T139 VARCHAR2(30) DEFAULT '-', T140 VARCHAR2(30) DEFAULT '-', ENT_BY VARCHAR2(20) DEFAULT '-', ENT_DT DATE DEFAULT SYSDATE, EDT_BY VARCHAR2(20) DEFAULT '-', EDT_DT DATE DEFAULT SYSDATE)");
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
            case "ticode":
                SQuery = "select Icode as FSTR,REPLACE(INAME,'&quot','') AS Item,Icode as Item_code,Cpartno,Cdrgno from Item where substr(icode,1,1) in ('9') and length(Trim(icode))>4 order by Iname";
                break;
            case "tacode":
                SQuery = "select acode as FSTR,REPLACE(aNAME,'&quot','') AS Party_Name,acode as Party_code from famst WHERE SUBSTR(ACODE,1,2)='16' order by Aname";
                break;
            default:
                if (btnval == "Edit" || btnval == "Print" || btnval == "Del" || btnval == "New_E")
                    SQuery = "select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as CostSheet_no,to_char(a.vchdate,'dd/mm/yyyy') as CostSheet_dt,(case when trim(nvl(b.aname,'-'))='-' then a.t120 else b.aname end) as party_name from somas_anx a left outer join famst b on trim(a.acode)=trim(b.acode) where a.VCHDATE " + DateRange + " AND a.type='PN' and a.branchcd='" + frm_mbr + "' order by a.vchnum desc";
                break;
        }
        if (SQuery.Length > 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("-", frm_qstr);
        }
    }
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Edit"; edmode.Value = "Y";
        disp_data(); hfname.Value = "";
    }
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "New";
        SQuery = "select max(vchnum) as vch from somas_anx where type='PN' and branchcd='" + frm_mbr + "' and vchdate " + DateRange + "";
        vchnum = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch");
        txtvchnum.Text = vchnum.Trim();
        txtvchdt.Text = vardate;
        btnacode.Visible = true; txtacode.Visible = true;
        btnicode.Visible = true; txticode.Visible = true; txtiname.ReadOnly = true;
        fgen.EnableForm(this.Controls); disablectrl(); btnacode.Focus();
        hfname.Value = "";

        fgen.msg("-", "CMSG", "Do You Want to Copy from Old Costing");
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "";
        fgen.fill_zero(this.Controls);
        if (txtaname.Text == "" || txtaname.Text == null || txtaname.Text == "0" || txtaname.Text == "-")
        { fgen.msg("-", "AMSG", "Please select Party Name!!"); btnacode.Focus(); return; }
        if (txtiname.Text == "" || txtiname.Text == null || txtiname.Text == "0" || txtiname.Text == "-")
        { fgen.msg("-", "AMSG", "Please select Item Name!!"); btnicode.Focus(); return; }
        dhd = fgen.ChkDate(txtvchdt.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a valid Date"); txtvchdt.Focus(); return; }
        if (Convert.ToDateTime(txtvchdt.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdt.Text) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Date is not allowed!! '13'Fill date for this year only"); txtvchdt.Focus(); return; }
        else fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
        btnsave.Disabled = true;
    }
    protected void btnprnt_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Print";
        disp_data();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Del";
        disp_data();
    }
    protected void btnext_Click(object sender, EventArgs e)
    {
        if (btnext.Text == "Exit")
        {
            Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
        }
        else
        {
            fgen.ResetForm(this.Controls);
            fgen.DisableForm(this.Controls);
            enablectrl();
            btnacode.Visible = true; txtacode.Visible = true;
            btnicode.Visible = true; txticode.Visible = true; txtiname.ReadOnly = true;
            hfname.Value = "";
        }
    }
    protected void btnicode_Click(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "ticode";
        disp_data();
    }
    protected void btnacode_Click(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "tacode";
        disp_data();
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;

        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();

            if (col1 == "Y")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from somas_anx a where A.BRANCHCD||A.TYPE||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + edmode.Value + "'");
                fgen.msg("-", "AMSG", "Details are deleted for " + edmode.Value.Substring(4, 6) + "");
                clearctrl(); fgen.ResetForm(this.Controls);
            }
            else
            { }
        }
        else if (hffield.Value == "New")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                hffield.Value = "New_E";
                disp_data();
                fgen.Fn_open_sseek("-", frm_qstr);
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
                    case "ticode":
                        txticode.Text = col1.Trim().ToString();
                        txtiname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select iname from item where trim(icode)='" + col1.Trim() + "'", "iname");
                        txtpt2.Focus();
                        break;
                    case "tacode":
                        txtacode.Text = col1.Trim(); txtaname.Text = col2.Trim(); btnicode.Focus();
                        break;
                    case "Edit":
                    case "New_E":
                        if (col1 == "") return;
                        dt = new DataTable(); DataTable dt1 = new DataTable(); DataTable dt2 = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "select * from somas_anx A where A.BRANCHCD||A.TYPE||A.vchnum||TO_CHAR(A.vchdate,'DD/MM/YYYY') IN ('" + col1.Trim() + "')");
                        dt1 = fgen.getdata(frm_qstr, frm_cocd, "select aname from famst where acode='" + dt.Rows[0]["acode"].ToString().Trim() + "'");
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, "select iname from item where icode='" + dt.Rows[0]["icode"].ToString().Trim() + "'");
                        txtvchnum.Text = col2.Trim(); txtvchdt.Text = col3.Trim(); txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txticode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        if (dt1.Rows.Count > 0)
                        {
                            txtaname.Text = dt1.Rows[0]["aname"].ToString().Trim();
                            txtiname.Text = dt2.Rows[0]["iname"].ToString().Trim();
                        }
                        {

                            int x = 99;
                            if (frm_cocd == "SYDB" || frm_cocd == "ALIN" || frm_cocd == "RELI" || frm_cocd == "BEST" || frm_cocd == "PACT" || frm_cocd == "ECPL") x = 106;
                            for (int i = 1; i <= x; i++)
                            {
                                int z = 1;
                                z = z + i;
                                string txtbid = "ctl00$ContentPlaceHolder1$txtpt" + z.ToString();
                                string colid = "t" + i.ToString();
                                TextBox txtv = Page.FindControl(txtbid.Trim()) as TextBox;
                                txtv.Text = dt.Rows[0][colid].ToString().Trim();
                            }
                        }
                        if (dt.Rows[0]["t119"].ToString().Trim() == "MANUAL")
                        {
                            txtaname.Text = dt.Rows[0]["t120"].ToString().Trim();
                            txtiname.Text = dt.Rows[0]["t121"].ToString().Trim();

                            btnacode.Visible = false; txtacode.Visible = false;
                            btnicode.Visible = false; txticode.Visible = false; hfname.Value = "MANUAL";
                        }
                        if (frm_cocd == "SYDB" || frm_cocd == "ALIN" || frm_cocd == "RELI") txthead.Text = dt.Rows[0]["t122"].ToString().Trim();
                        else txthead.Text = txtpt81.Text;
                        dd1.SelectedIndex = Convert.ToInt32(dt.Rows[0]["CDRGNO"].ToString());
                        fgen.EnableForm(this.Controls); disablectrl();
                        break;
                    case "Print":
                        if (col1 == "") return;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                        fgen.fin_engg_reps(frm_qstr);
                        break;
                    case "Del":
                        edmode.Value = col1;
                        hffield.Value = "D";
                        fgen.msg("-", "CMSG", "Are You Sure, You want to Delete !!");
                        break;
                }
            }
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        if (hffield.Value == "LINV" || hffield.Value == "LSO")
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
            xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            fgenMV.Fn_Set_Mvar(frm_qstr, "SHEADER", "Item : " + txtiname.Text.Trim());
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");

            if (hffield.Value == "LINV")
                SQuery = "select distinct a.vchnum as fstr, a.vchnum as Bill_No,to_char(a.vchdate,'dd/mm/yyyy') as bil_date,a.icode as ERP_code,b.aname as Party_Name,a.irate as Basic_rate from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.type like '4%' and a.type!='47' and trim(a.icode)='" + txticode.Text.Trim() + "' and a.vchdate " + xprdRange + " order by a.vchnum desc";
            else if (hffield.Value == "LSO")
                SQuery = "select distinct a.ordno as fstr,a.ordno as SO_no,to_char(a.orddt,'dd/mm/yyyy') as SO_date,a.icode as erp_code,b.aname as Party_Name,a.irate as Basic_rate from somas a,famst b where trim(a.acode)=trim(b.acode) and a.type like '4%' and a.type!='47' and a.orddt " + xprdRange + " and trim(a.icode)='" + txticode.Text.Trim() + "' order by a.ordno";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("-", frm_qstr);
        }
        else
        {
            col1 = "";
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            // col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            if (col1 != "Y")
            { }
            else
            {
                if (edmode.Value == "Y")
                    fgen.execute_cmd(frm_qstr, frm_cocd, "update somas_anx set branchcd='DD' where branchcd='" + frm_mbr + "' and type='PN' and vchnum='" + txtvchnum.Text.Trim() + "' and to_DatE(to_char(vchdate,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + txtvchdt.Text.Trim() + "','dd/MM/yyyy')");

                oDS = new DataSet();
                oDS = fgen.fill_schema(frm_qstr, frm_cocd, "SOMAS_ANX");

                DataRow oporow = oDS.Tables[0].NewRow();

                if (edmode.Value == "Y")
                    vchnum = txtvchnum.Text.Trim();
                else
                {
                    SQuery = "select max(vchnum) as vch from somas_anx where type='PN' and branchcd='" + frm_mbr + "' and vchdate " + DateRange + "";
                    vchnum = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch");
                }

                oporow["vchnum"] = vchnum.Trim();
                oporow["vchdate"] = txtvchdt.Text.Trim();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = "PN";
                oporow["acode"] = txtacode.Text.Trim();
                oporow["icode"] = txticode.Text.Trim();
                oporow["CDRGNO"] = dd1.SelectedIndex.ToString();
                //if (frm_cocd == "NITC" || frm_cocd == "ECPL" || frm_cocd == "NITP" || frm_cocd == "SYDB" || frm_cocd == "TGIP" || frm_cocd == "STOR" || frm_cocd == "PRIN" || frm_cocd == "MCPL" || frm_cocd == "MAYU" || frm_cocd == "KPFL" || frm_cocd == "PANO" || frm_cocd == "ALIN" || frm_cocd == "CCOR" || frm_cocd == "RELI")
                {
                    int x = 99;
                    if (frm_cocd == "SYDB" || frm_cocd == "ALIN" || frm_cocd == "RELI" || frm_cocd == "BEST" || frm_cocd == "PACT" || frm_cocd == "ECPL") x = 106;
                    for (int i = 1; i <= x; i++)
                    {
                        int z = 1;
                        z = z + i;
                        string txtbid = "ctl00$ContentPlaceHolder1$txtpt" + z.ToString();
                        TextBox txtv = Page.FindControl(txtbid.Trim()) as TextBox;
                        oporow["T" + i.ToString() + ""] = txtv.Text.ToString().Trim();
                    }
                    if (hfname.Value == "MANUAL")
                    {
                        oporow["t119"] = "MANUAL";
                        if (txtaname.Text.Trim().Length > 30)
                        {
                            oporow["t120"] = txtaname.Text.Trim().Substring(0, 29).ToUpper();
                            oporow["t121"] = txtiname.Text.Trim().Substring(0, 29).ToUpper();
                        }
                        else
                        {
                            oporow["t120"] = txtaname.Text.Trim().ToUpper();
                            oporow["t121"] = txtiname.Text.Trim().ToUpper();
                        }
                    }
                    oporow["t122"] = txthead.Text.Trim();
                }
                if (edmode.Value == "Y")
                {
                    oporow["eNt_by"] = frm_uname;
                    oporow["eNt_dt"] = vardate;
                    oporow["edt_by"] = frm_uname;
                    oporow["edt_dt"] = todt;
                }
                else
                {
                    oporow["eNt_by"] = frm_uname;
                    oporow["eNt_dt"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["eDt_dt"] = vardate;
                }

                oDS.Tables[0].Rows.Add(oporow);
                fgen.save_data(frm_qstr, frm_cocd, oDS, "SOMAS_ANX");

                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from somas_anx where branchcd='DD' and type='PN' and vchnum='" + txtvchnum.Text.Trim() + "' and to_DatE(to_char(vchdate,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + txtvchdt.Text.Trim() + "','dd/mm/yyyy')");
                if (edmode.Value == "Y") { fgen.msg("-", "AMSG", "Data Updated Successfully"); fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); }
                else { fgen.msg("-", "AMSG", "Data Saved Successfully"); fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); }
                hfname.Value = "";
            }
        }
    }
    public void cal()
    {
        {
            try
            {
                fgen.fill_zero(this.Controls);
                txtpt18.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt2.Text.Trim()) + Convert.ToDouble(txtpt12.Text.Trim()), 2));
                txtpt19.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt3.Text.Trim()) + Convert.ToDouble(txtpt13.Text.Trim()), 2));
                txtpt20.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt4.Text.Trim()) + Convert.ToDouble(txtpt14.Text.Trim()), 2));

                if (dd1.SelectedIndex == 0)
                {
                    txtpt15.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt19.Text.Trim()) + Convert.ToDouble(txtpt20.Text.Trim())) + Convert.ToDouble(txtpt6.Text.Trim()), 2));
                    txtpt16.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt18.Text.Trim()) + Convert.ToDouble(txtpt19.Text.Trim())) * 2 + (Convert.ToDouble(txtpt7.Text.Trim()) + Convert.ToDouble(txtpt8.Text.Trim())), 2));
                }
                else if (dd1.SelectedIndex == 1)
                {
                    txtpt15.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt19.Text.Trim()) + Convert.ToDouble(txtpt20.Text.Trim()) + Convert.ToDouble(txtpt19.Text.Trim())) + Convert.ToDouble(txtpt6.Text.Trim()), 2));
                    txtpt16.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt18.Text.Trim()) + Convert.ToDouble(txtpt19.Text.Trim())) * 2 + (Convert.ToDouble(txtpt7.Text.Trim()) + Convert.ToDouble(txtpt8.Text.Trim())), 2));
                }
                else if (dd1.SelectedIndex == 2)
                {
                    txtpt15.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt20.Text.Trim()) + Convert.ToDouble(txtpt18.Text.Trim()) + Convert.ToDouble(txtpt20.Text.Trim())) + Convert.ToDouble(txtpt6.Text.Trim()), 2));
                    txtpt16.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt20.Text.Trim()) + Convert.ToDouble(txtpt19.Text.Trim()) + Convert.ToDouble(txtpt20.Text.Trim())) + (Convert.ToDouble(txtpt7.Text.Trim()) + Convert.ToDouble(txtpt8.Text.Trim())), 2));
                }
                else if (dd1.SelectedIndex == 3)
                {
                    txtpt15.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt19.Text.Trim())) + Convert.ToDouble(txtpt6.Text.Trim()), 2));
                    txtpt16.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt18.Text.Trim())) + (Convert.ToDouble(txtpt7.Text.Trim()) + Convert.ToDouble(txtpt8.Text.Trim())), 2));
                }
                else if (dd1.SelectedIndex == 4)
                {
                    txtpt15.Text = Convert.ToString(Math.Round(((Convert.ToDouble(txtpt19.Text.Trim()) / 2) + Convert.ToDouble(txtpt20.Text.Trim())) + Convert.ToDouble(txtpt6.Text.Trim()), 2));
                    txtpt16.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt18.Text.Trim()) + Convert.ToDouble(txtpt19.Text.Trim())) * 2 + (Convert.ToDouble(txtpt7.Text.Trim()) + Convert.ToDouble(txtpt8.Text.Trim())), 2));
                }
                else if (dd1.SelectedIndex == 5)
                {
                    txtpt15.Text = txtpt20.Text;
                    txtpt16.Text = Convert.ToString(Math.Round(((Convert.ToDouble(txtpt18.Text.Trim())) + Convert.ToDouble(txtpt19.Text.Trim()) + Convert.ToDouble(txtpt18.Text.Trim())) + Convert.ToDouble(txtpt7.Text.Trim()), 2));
                }
                else if (dd1.SelectedIndex == 6)
                {
                    txtpt15.Text = Convert.ToString(Math.Round((((30 + (Convert.ToDouble(txtpt19.Text.Trim()) / 2)) + Convert.ToDouble(txtpt20.Text.Trim()) + (Convert.ToDouble(txtpt19.Text.Trim()) + 30)) + Convert.ToDouble(txtpt6.Text.Trim())), 2));
                    txtpt16.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt18.Text.Trim()) + Convert.ToDouble(txtpt19.Text.Trim())) * 2 + (Convert.ToDouble(txtpt7.Text.Trim()) + Convert.ToDouble(txtpt8.Text.Trim())), 2));
                }

                txtpt21.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt15.Text.Trim()) / 10, 2));
                txtpt22.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt16.Text.Trim()) / 10, 2));

                //  T.Gsm / T.Rct
                txtpt29.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt25.Text.Trim()) * Convert.ToDouble(txtpt28.Text.Trim()), 2));
                txtpt38.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt34.Text.Trim()) * Convert.ToDouble(txtpt37.Text.Trim()), 2));
                txtpt47.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt43.Text.Trim()) * Convert.ToDouble(txtpt46.Text.Trim()), 2));
                txtpt56.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt52.Text.Trim()) * Convert.ToDouble(txtpt55.Text.Trim()), 2));
                txtpt65.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt61.Text.Trim()) * Convert.ToDouble(txtpt64.Text.Trim()), 2));
                txtpt74.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt70.Text.Trim()) * Convert.ToDouble(txtpt73.Text.Trim()), 2));
                txtpt97.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt93.Text.Trim()) * Convert.ToDouble(txtpt96.Text.Trim()), 2));

                txtpt79.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt29.Text.Trim()) + Convert.ToDouble(txtpt38.Text.Trim()) + Convert.ToDouble(txtpt47.Text.Trim()) + Convert.ToDouble(txtpt56.Text.Trim()) + Convert.ToDouble(txtpt65.Text.Trim()) + Convert.ToDouble(txtpt74.Text.Trim()) + Convert.ToDouble(txtpt97.Text.Trim()), 2));

                //Meduim wt
                txtpt30.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt21.Text.Trim()) * Convert.ToDouble(txtpt22.Text.Trim()) * Convert.ToDouble(txtpt29.Text.Trim())) / 10000 * (100 + (Convert.ToDouble(txtpt10.Text.Trim()))) / 100, 2));
                txtpt39.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt21.Text.Trim()) * Convert.ToDouble(txtpt22.Text.Trim()) * Convert.ToDouble(txtpt38.Text.Trim())) / 10000 * (100 + (Convert.ToDouble(txtpt10.Text.Trim()))) / 100, 2));
                txtpt48.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt21.Text.Trim()) * Convert.ToDouble(txtpt22.Text.Trim()) * Convert.ToDouble(txtpt47.Text.Trim())) / 10000 * (100 + (Convert.ToDouble(txtpt10.Text.Trim()))) / 100, 2));
                txtpt57.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt21.Text.Trim()) * Convert.ToDouble(txtpt22.Text.Trim()) * Convert.ToDouble(txtpt56.Text.Trim())) / 10000 * (100 + (Convert.ToDouble(txtpt10.Text.Trim()))) / 100, 2));
                txtpt66.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt21.Text.Trim()) * Convert.ToDouble(txtpt22.Text.Trim()) * Convert.ToDouble(txtpt65.Text.Trim())) / 10000 * (100 + (Convert.ToDouble(txtpt10.Text.Trim()))) / 100, 2));
                txtpt75.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt21.Text.Trim()) * Convert.ToDouble(txtpt22.Text.Trim()) * Convert.ToDouble(txtpt74.Text.Trim())) / 10000 * (100 + (Convert.ToDouble(txtpt10.Text.Trim()))) / 100, 2));
                txtpt98.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt21.Text.Trim()) * Convert.ToDouble(txtpt22.Text.Trim()) * Convert.ToDouble(txtpt97.Text.Trim())) / 10000 * (100 + (Convert.ToDouble(txtpt10.Text.Trim()))) / 100, 2));

                txtpt80.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt30.Text.Trim()) + Convert.ToDouble(txtpt39.Text.Trim()) + Convert.ToDouble(txtpt48.Text.Trim()) + Convert.ToDouble(txtpt57.Text.Trim()) + Convert.ToDouble(txtpt66.Text.Trim()) + Convert.ToDouble(txtpt75.Text.Trim()) + Convert.ToDouble(txtpt98.Text.Trim()), 2));

                if (frm_cocd == "SYDB" || frm_cocd == "ALIN" || frm_cocd == "RELI")
                {
                    txtpt90.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt42.Text.Trim()) + Convert.ToDouble(txtpt51.Text.Trim()) + Convert.ToDouble(txtpt60.Text.Trim()) + Convert.ToDouble(txtpt69.Text.Trim()) + Convert.ToDouble(txtpt78.Text.Trim())
                    + Convert.ToDouble(txtpt82.Text.Trim()) + Convert.ToDouble(txtpt83.Text.Trim()) + Convert.ToDouble(txtpt87.Text.Trim()) + Convert.ToDouble(txtpt88.Text.Trim()) + Convert.ToDouble(txtpt89.Text.Trim()), 2));
                    txtpt92.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt90.Text.Trim()) + Convert.ToDouble(txtpt91.Text.Trim()), 2));

                    txtpt17.Text = txtpt80.Text.Trim(); txtpt23.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt17.Text.Trim()) / 1000, 2));
                    txtpt84.Text = txtpt21.Text.Trim(); txtpt85.Text = txtpt22.Text.Trim(); txtpt86.Text = txtpt17.Text.Trim();

                    //
                    txtpt32.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt30.Text.Trim()) * (Convert.ToDouble(txtpt31.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / 1000), 2));
                    txtpt41.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt39.Text.Trim()) * (Convert.ToDouble(txtpt40.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / 1000), 2));
                    txtpt50.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt48.Text.Trim()) * (Convert.ToDouble(txtpt49.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / 1000), 2));
                    txtpt59.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt57.Text.Trim()) * (Convert.ToDouble(txtpt58.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / 1000), 2));
                    txtpt68.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt66.Text.Trim()) * (Convert.ToDouble(txtpt67.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / 1000), 2));
                    txtpt77.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt75.Text.Trim()) * (Convert.ToDouble(txtpt76.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / 1000), 2));
                    txtpt100.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt98.Text.Trim()) * (Convert.ToDouble(txtpt99.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / 1000), 2));

                    txtpt81.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt32.Text.Trim()) + Convert.ToDouble(txtpt41.Text.Trim()) + Convert.ToDouble(txtpt50.Text.Trim()) + Convert.ToDouble(txtpt59.Text.Trim()) + Convert.ToDouble(txtpt68.Text.Trim()) + Convert.ToDouble(txtpt77.Text.Trim()) + Convert.ToDouble(txtpt100.Text.Trim()), 2));

                    txthead.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt80.Text.Trim()) * Convert.ToDouble(txtpt33.Text.Trim()) / 1000) + Convert.ToDouble(txtpt81.Text), 2));
                    txtpt103.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt101.Text.Trim()) + Convert.ToDouble(txtpt102.Text.Trim()) + Convert.ToDouble(txtpt107.Text.Trim()) + Convert.ToDouble(txthead.Text.Trim()), 2));
                    txtpt104.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt103.Text.Trim()) * Convert.ToDouble(txtpt106.Text.Trim()) / 100, 2));
                    txtpt105.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt103.Text.Trim()) + Convert.ToDouble(txtpt104.Text.Trim()), 2));
                }
                else
                {
                    //TOTAL
                    txtpt90.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt33.Text.Trim()) + Convert.ToDouble(txtpt42.Text.Trim()) + Convert.ToDouble(txtpt51.Text.Trim()) + Convert.ToDouble(txtpt60.Text.Trim()) + Convert.ToDouble(txtpt69.Text.Trim()) + Convert.ToDouble(txtpt78.Text.Trim())
                        + Convert.ToDouble(txtpt82.Text.Trim()) + Convert.ToDouble(txtpt83.Text.Trim()) + Convert.ToDouble(txtpt87.Text.Trim()) + Convert.ToDouble(txtpt88.Text.Trim()) + Convert.ToDouble(txtpt89.Text.Trim()), 2));
                    txtpt92.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt90.Text.Trim()) + Convert.ToDouble(txtpt91.Text.Trim()), 2));

                    txtpt17.Text = txtpt80.Text.Trim(); txtpt23.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt17.Text.Trim()) / 1000, 2));
                    txtpt84.Text = txtpt21.Text.Trim(); txtpt85.Text = txtpt22.Text.Trim(); txtpt86.Text = txtpt17.Text.Trim();
                    //

                    double kg = 1000;
                    if (frm_cocd == "PERF")
                    {
                        txtpt32.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt30.Text.Trim()) * (Convert.ToDouble(txtpt31.Text.Trim())) / kg), 2));
                        txtpt41.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt39.Text.Trim()) * (Convert.ToDouble(txtpt40.Text.Trim())) / kg), 2));
                        txtpt50.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt48.Text.Trim()) * (Convert.ToDouble(txtpt49.Text.Trim())) / kg), 2));
                        txtpt59.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt57.Text.Trim()) * (Convert.ToDouble(txtpt58.Text.Trim())) / kg), 2));
                        txtpt68.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt66.Text.Trim()) * (Convert.ToDouble(txtpt67.Text.Trim())) / kg), 2));
                        txtpt77.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt75.Text.Trim()) * (Convert.ToDouble(txtpt76.Text.Trim())) / kg), 2));
                        txtpt100.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt98.Text.Trim()) * (Convert.ToDouble(txtpt99.Text.Trim())) / kg), 2));

                        txtpt81.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt32.Text.Trim()) + Convert.ToDouble(txtpt41.Text.Trim()) + Convert.ToDouble(txtpt50.Text.Trim()) + Convert.ToDouble(txtpt59.Text.Trim()) + Convert.ToDouble(txtpt68.Text.Trim()) + Convert.ToDouble(txtpt77.Text.Trim()) + Convert.ToDouble(txtpt100.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim()), 2));
                    }
                    else
                    {
                        txtpt32.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt30.Text.Trim()) * (Convert.ToDouble(txtpt31.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / kg), 2));
                        txtpt41.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt39.Text.Trim()) * (Convert.ToDouble(txtpt40.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / kg), 2));
                        txtpt50.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt48.Text.Trim()) * (Convert.ToDouble(txtpt49.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / kg), 2));
                        txtpt59.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt57.Text.Trim()) * (Convert.ToDouble(txtpt58.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / kg), 2));
                        txtpt68.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt66.Text.Trim()) * (Convert.ToDouble(txtpt67.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / kg), 2));
                        txtpt77.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt75.Text.Trim()) * (Convert.ToDouble(txtpt76.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / kg), 2));
                        txtpt100.Text = Convert.ToString(Math.Round((Convert.ToDouble(txtpt98.Text.Trim()) * (Convert.ToDouble(txtpt99.Text.Trim()) + Convert.ToDouble(txtpt92.Text.Trim())) / kg), 2));

                        txtpt81.Text = Convert.ToString(Math.Round(Convert.ToDouble(txtpt32.Text.Trim()) + Convert.ToDouble(txtpt41.Text.Trim()) + Convert.ToDouble(txtpt50.Text.Trim()) + Convert.ToDouble(txtpt59.Text.Trim()) + Convert.ToDouble(txtpt68.Text.Trim()) + Convert.ToDouble(txtpt77.Text.Trim()) + Convert.ToDouble(txtpt100.Text.Trim()), 2));
                    }
                    txthead.Text = txtpt81.Text;
                }

                if (frm_cocd == "BEST" || frm_cocd == "PACT" || frm_cocd == "ECPL")
                {
                    txtpt102.Text = Convert.ToString(Math.Round((txtpt86.Text.toDouble() * 0.001) * txtpt92.Text.toDouble(), 2));
                    txtpt107.Text = Convert.ToString(Math.Round((txtpt102.Text.toDouble() * 1) + txthead.Text.toDouble(), 2));
                    if ((txtpt86.Text.toDouble() * 0.001) > 0 && (txtpt107.Text.toDouble() * 1) > 0)
                        txtpt103.Text = Convert.ToString(Math.Round((txtpt107.Text.toDouble() * 1) / (txtpt86.Text.toDouble() * 0.001), 2));
                }

                fgen.fill_zero(this.Controls);
            }
            catch
            {
                fgen.fill_zero(this.Controls);
            }
        }
    }
    protected void dd1_SelectedIndexChanged(object sender, EventArgs e)
    {
        cal();
    }
    protected void txtpt2_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt3.Focus();
    }
    protected void txtpt3_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt4.Focus();
    }
    protected void txtpt4_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt12.Focus();
    }
    protected void txtpt12_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt13.Focus();
    }
    protected void txtpt13_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt14.Focus();
    }
    protected void txtpt14_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt5.Focus();
    }
    protected void txtpt5_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt6.Focus();
    }
    protected void txtpt6_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt7.Focus();
    }
    protected void txtpt7_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt8.Focus();
    }
    protected void txtpt8_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt9.Focus();
    }
    protected void txtpt9_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt10.Focus();
    }
    protected void txtpt10_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt25.Focus();
    }
    protected void txtpt11_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt25.Focus();
    }
    protected void txtpt25_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt26.Focus();
    }
    protected void txtpt26_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt34.Focus();
    }
    protected void txtpt34_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt35.Focus();
    }
    protected void txtpt35_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt43.Focus();
    }
    protected void txtpt43_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt44.Focus();
    }
    protected void txtpt44_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt52.Focus();
    }
    protected void txtpt52_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt53.Focus();
    }
    protected void txtpt53_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt61.Focus();
    }
    protected void txtpt61_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt62.Focus();
    }
    protected void txtpt62_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt70.Focus();
    }
    protected void txtpt70_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt71.Focus();
    }
    protected void txtpt71_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt93.Focus();
    }
    protected void txtpt93_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt94.Focus();
    }
    protected void txtpt94_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt28.Focus();
    }
    protected void txtpt37_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt46.Focus();
    }
    protected void txtpt28_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt37.Focus();
    }
    protected void txtpt46_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt55.Focus();
    }
    protected void txtpt55_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt64.Focus();
    }
    protected void txtpt64_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt73.Focus();
    }
    protected void txtpt73_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt96.Focus();
    }
    protected void txtpt96_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt31.Focus();
    }
    protected void txtpt31_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt40.Focus();
    }
    protected void txtpt40_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt49.Focus();
    }
    protected void txtpt49_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt58.Focus();
    }
    protected void txtpt58_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt67.Focus();
    }
    protected void txtpt67_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt76.Focus();
    }
    protected void txtpt76_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt99.Focus();
    }
    protected void txtpt99_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt33.Focus();
    }
    protected void txtpt33_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt42.Focus();
    }
    protected void txtpt42_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt51.Focus();
    }
    protected void txtpt51_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt60.Focus();
    }
    protected void txtpt60_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt69.Focus();
    }
    protected void txtpt69_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt78.Focus();
    }
    protected void txtpt83_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt87.Focus();
    }
    protected void txtpt78_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt83.Focus();
    }
    protected void txtpt87_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt88.Focus();
    }
    protected void txtpt88_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt89.Focus();
    }
    protected void txtpt89_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt90.Focus();
    }
    protected void txtpt90_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt91.Focus();
    }
    protected void txtpt91_TextChanged(object sender, EventArgs e)
    {
        cal();
        if (frm_cocd == "SYDB" || frm_cocd == "ALIN" || frm_cocd == "RELI") txtpt101.Focus();
        else
            btnsave.Focus();
    }
    protected void btninv_Click(object sender, EventArgs e)
    {
        if (txticode.Text == "" || txticode.Text == "0")
        { fgen.msg("-", "AMSG", "Please select Item first"); btnicode.Focus(); }
        else
        {
            hffield.Value = "";
            hffield.Value = "LINV";
            fgen.Fn_open_prddmp1("-", frm_qstr);
        }
    }
    protected void btnso_Click(object sender, EventArgs e)
    {
        if (txticode.Text == "" || txticode.Text == "0")
        { fgen.msg("-", "AMSG", "Please select Item first"); btnicode.Focus(); }
        else
        {
            hffield.Value = "";
            hffield.Value = "LSO";
            fgen.Fn_open_prddmp1("-", frm_qstr);
        }
    }
    protected void txtpt101_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt102.Focus();
    }
    protected void txtpt102_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt107.Focus();
    }
    protected void txtpt106_TextChanged(object sender, EventArgs e)
    {
        cal();
        btnsave.Focus();
    }
    protected void txtpt107_TextChanged(object sender, EventArgs e)
    {
        cal();
        txtpt106.Focus();
    }
    protected void txtaname_TextChanged1(object sender, EventArgs e)
    {
        btnacode.Visible = false; txtacode.Visible = false;
        btnicode.Visible = false; txticode.Visible = false; txtiname.ReadOnly = false;
        txticode.Text = ""; txtacode.Text = "";
        hfname.Value = "MANUAL"; txtiname.Focus();
    }
}