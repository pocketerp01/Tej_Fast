using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class duplx_cost : System.Web.UI.Page
{
    string btnval, SQuery, co_cd, uname, col1, col2, col3, mbr, vchnum, vardate, fromdt, todt, DateRange, year, vty, HCID, ulvl, tabname, vip = "", mq0 = "";
    DataTable dt; DataRow oporow;
    fgenDB fgen = new fgenDB();
    string frm_url, frm_PageName, frm_qstr, frm_formID, frm_CDT1, frm_cocd, frm_UserID;
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
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    frm_cocd = co_cd;
                    uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
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
                fgen.DisableForm(this.Controls);
                enablectrl(); btnnew.Focus(); chk_tab(); btnlist.Visible = false;
            }
            myfun(); set_Val();
        }
    }
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnext.Visible = true; btncan.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnicode.Enabled = false; btnacode.Enabled = false;
    }
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnext.Visible = false; btncan.Visible = true;
        btnicode.Enabled = true; btnacode.Enabled = true;
    }
    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
    }
    public void chk_tab()
    {
        btnval = fgen.seek_iname(frm_qstr, co_cd, "Select tname from tab where tname like 'SOMAS_ANX%'", "tname");
        if (btnval == "0") fgen.execute_cmd(frm_qstr, co_cd, "CREATE TABLE SOMAS_ANX ( BRANCHCD CHAR(2) DEFAULT '-',TYPE CHAR(2) DEFAULT '-',VCHNUM  CHAR(6) DEFAULT '-',VCHDATE DATE DEFAULT SYSDATE,CDRGNO CHAR(10) DEFAULT '-',ACODE CHAR(10) DEFAULT '-',ICODE CHAR(10) DEFAULT '-',T1 VARCHAR2(30) DEFAULT '-',T2 VARCHAR2(30) DEFAULT '-',T3 VARCHAR2(30) DEFAULT '-',T4 VARCHAR2(30) DEFAULT '-',T5 VARCHAR2(30) DEFAULT '-',T6 VARCHAR2(30) DEFAULT '-',T7 VARCHAR2(30) DEFAULT '-',T8 VARCHAR2(30) DEFAULT '-'," +
 "T9 VARCHAR2(30) DEFAULT '-', T10 VARCHAR2(30) DEFAULT '-', T11 VARCHAR2(30) DEFAULT '-', T12 VARCHAR2(30) DEFAULT '-', T13 VARCHAR2(30) DEFAULT '-', T14 VARCHAR2(30) DEFAULT '-', T15 VARCHAR2(30) DEFAULT '-', T16 VARCHAR2(30) DEFAULT '-', T17 VARCHAR2(30) DEFAULT '-', T18 VARCHAR2(30) DEFAULT '-', T19 VARCHAR2(30) DEFAULT '-', T20 VARCHAR2(30) DEFAULT '-', T21 VARCHAR2(30) DEFAULT '-', T22 VARCHAR2(30) DEFAULT '-', T23 VARCHAR2(30) DEFAULT '-', T24 VARCHAR2(30) DEFAULT '-', T25 VARCHAR2(30) DEFAULT '-'," +
 "T26 VARCHAR2(30) DEFAULT '-', T27 VARCHAR2(30) DEFAULT '-', T28 VARCHAR2(30) DEFAULT '-', T29 VARCHAR2(30) DEFAULT '-', T30 VARCHAR2(30) DEFAULT '-', T31 VARCHAR2(30) DEFAULT '-', T32 VARCHAR2(30) DEFAULT '-', T33 VARCHAR2(30) DEFAULT '-', T34 VARCHAR2(30) DEFAULT '-', T35 VARCHAR2(30) DEFAULT '-', T36 VARCHAR2(30) DEFAULT '-', T37 VARCHAR2(30) DEFAULT '-', T38 VARCHAR2(30) DEFAULT '-', T39 VARCHAR2(30) DEFAULT '-', T40 VARCHAR2(30) DEFAULT '-',T41 VARCHAR2(30) DEFAULT '-', T42 VARCHAR2(30) DEFAULT '-', T43 VARCHAR2(30) DEFAULT '-'," +
 "T44 VARCHAR2(30) DEFAULT '-', T45 VARCHAR2(30) DEFAULT '-', T46 VARCHAR2(30) DEFAULT '-', T47 VARCHAR2(30) DEFAULT '-', T48 VARCHAR2(30) DEFAULT '-', T49 VARCHAR2(30) DEFAULT '-', T50 VARCHAR2(30) DEFAULT '-', T51 VARCHAR2(30) DEFAULT '-', T52 VARCHAR2(30) DEFAULT '-', T53 VARCHAR2(30) DEFAULT '-', T54 VARCHAR2(30) DEFAULT '-', T55 VARCHAR2(30) DEFAULT '-', T56 VARCHAR2(30) DEFAULT '-', T57 VARCHAR2(30) DEFAULT '-', T58 VARCHAR2(30) DEFAULT '-', T59 VARCHAR2(30) DEFAULT '-', T60 VARCHAR2(30) DEFAULT '-', T61 VARCHAR2(30) DEFAULT '-', T62 VARCHAR2(30) DEFAULT '-'," +
 "T63 VARCHAR2(30) DEFAULT '-', T64 VARCHAR2(30) DEFAULT '-', T65 VARCHAR2(30) DEFAULT '-', T66 VARCHAR2(30) DEFAULT '-', T67 VARCHAR2(30) DEFAULT '-', T68 VARCHAR2(30) DEFAULT '-', T69 VARCHAR2(30) DEFAULT '-', T70 VARCHAR2(30) DEFAULT '-', T71 VARCHAR2(30) DEFAULT '-', T72 VARCHAR2(30) DEFAULT '-', T73 VARCHAR2(30) DEFAULT '-', T74 VARCHAR2(30) DEFAULT '-', T75 VARCHAR2(30) DEFAULT '-', T76 VARCHAR2(30) DEFAULT '-', T77 VARCHAR2(30) DEFAULT '-', T78 VARCHAR2(30) DEFAULT '-', T79 VARCHAR2(30) DEFAULT '-', T80 VARCHAR2(30) DEFAULT '-', T81 VARCHAR2(30) DEFAULT '-'," +
 "T82 VARCHAR2(30) DEFAULT '-', T83 VARCHAR2(30) DEFAULT '-', T84 VARCHAR2(30) DEFAULT '-', T85 VARCHAR2(30) DEFAULT '-', T86 VARCHAR2(30) DEFAULT '-', T87 VARCHAR2(30) DEFAULT '-', T88 VARCHAR2(30) DEFAULT '-', T89 VARCHAR2(30) DEFAULT '-', T90 VARCHAR2(30) DEFAULT '-', T91 VARCHAR2(30) DEFAULT '-', T92 VARCHAR2(30) DEFAULT '-', T93 VARCHAR2(30) DEFAULT '-', T94 VARCHAR2(30) DEFAULT '-', T95 VARCHAR2(30) DEFAULT '-', T96 VARCHAR2(30) DEFAULT '-', T97 VARCHAR2(30) DEFAULT '-', T98 VARCHAR2(30) DEFAULT '-', T99 VARCHAR2(30) DEFAULT '-', T100 VARCHAR2(30) DEFAULT '-'," +
 "T101 VARCHAR2(30) DEFAULT '-', T102 VARCHAR2(30) DEFAULT '-', T103 VARCHAR2(30) DEFAULT '-', T104 VARCHAR2(30) DEFAULT '-', T105 VARCHAR2(30) DEFAULT '-', T106 VARCHAR2(30) DEFAULT '-', T107 VARCHAR2(30) DEFAULT '-', T108 VARCHAR2(30) DEFAULT '-', T109 VARCHAR2(30) DEFAULT '-', T110 VARCHAR2(30) DEFAULT '-', T111 VARCHAR2(30) DEFAULT '-', T112 VARCHAR2(30) DEFAULT '-', T113 VARCHAR2(30) DEFAULT '-', T114 VARCHAR2(30) DEFAULT '-', T115 VARCHAR2(30) DEFAULT '-', T116 VARCHAR2(30) DEFAULT '-', T117 VARCHAR2(30) DEFAULT '-', T118 VARCHAR2(30) DEFAULT '-', T119 VARCHAR2(30) DEFAULT '-'," +
 "T120 VARCHAR2(30) DEFAULT '-', T121 VARCHAR2(30) DEFAULT '-', T122 VARCHAR2(30) DEFAULT '-', T123 VARCHAR2(30) DEFAULT '-', T124 VARCHAR2(30) DEFAULT '-', T125 VARCHAR2(30) DEFAULT '-', T126 VARCHAR2(30) DEFAULT '-', T127 VARCHAR2(30) DEFAULT '-', T128 VARCHAR2(30) DEFAULT '-', T129 VARCHAR2(30) DEFAULT '-', T130 VARCHAR2(30) DEFAULT '-', T131 VARCHAR2(30) DEFAULT '-', T132 VARCHAR2(30) DEFAULT '-', T133 VARCHAR2(30) DEFAULT '-', T134 VARCHAR2(30) DEFAULT '-', T135 VARCHAR2(30) DEFAULT '-', T136 VARCHAR2(30) DEFAULT '-', T137 VARCHAR2(30) DEFAULT '-', T138 VARCHAR2(30) DEFAULT '-'," +
 "T139 VARCHAR2(30) DEFAULT '-', T140 VARCHAR2(200) DEFAULT '-', ENT_BY VARCHAR2(20) DEFAULT '-', ENT_DT DATE DEFAULT SYSDATE, EDT_BY VARCHAR2(20) DEFAULT '-', EDT_DT DATE DEFAULT SYSDATE)");
    }
    public void set_Val()
    {
        lblheader.Text = "Duplex Costing Sheet";
        tabname = "somas_anx"; vty = "DC";
    }
    public void disp_data()
    {
        btnval = hffield.Value;
        switch (btnval)
        {
            case "ACODE":
                SQuery = "SELECT ACODE AS FSTR,ANAME AS CUSTOMER,ACODE AS CODE,ADDR2 FROM FAMST WHERE TRIM(ACODE) LIKE '16%' ORDER BY ACODE";
                break;
            case "ICODE":
                SQuery = "SELECT ICODE AS FSTR,INAME AS PRODUCT,ICODE AS ERPCODE,CPARTNO AS PARTNO,UNIT FROM ITEM WHERE TRIM(ICODe) LIKE '9%' AND LENGTH(TRIM(ICODe))>6 ORDER BY ICODE";
                break;
            default:
                if (btnval == "Edit" || btnval == "Del" || btnval == "Print")
                    SQuery = "select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as CostSheet_no,to_char(a.vchdate,'dd/mm/yyyy') as CostSheet_dt,(case when trim(nvl(b.aname,'-'))='-' then a.t120 else b.aname end) as party_name,a.t121 as product from " + tabname + " a left outer join famst b on trim(a.acode)=trim(b.acode) where a.VCHDATE " + DateRange + " AND a.type='" + vty + "' and a.branchcd='" + mbr + "' order by a.vchnum desc";
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
        clearctrl(); set_Val();
        hffield.Value = "New";
        vchnum = fgen.next_no(frm_qstr, co_cd, "select max(vchnum) as vch from " + tabname + " where branchcd='" + mbr + "' and type='" + vty + "' and vchdate " + DateRange + "", 6, "vch");
        txtvchnum.Text = vchnum; txtvchdate.Text = vardate;
        fgen.EnableForm(this.Controls); disablectrl(); txtacode.Focus(); hfname.Value = ""; btnacode.Focus();
    }
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        clearctrl(); set_Val();
        hffield.Value = "Edit";
        disp_data(); hfname.Value = "";
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        fgen.fill_zero(this.Controls);
        if (txtaname.Text == "" || txtaname.Text == null || txtaname.Text == "0" || txtaname.Text == "-")
        { fgen.msg("-", "AMSG", "Please select Party Name!!"); btnacode.Focus(); return; }
        if (txtiname.Text == "" || txtiname.Text == null || txtiname.Text == "0" || txtiname.Text == "-")
        { fgen.msg("-", "AMSG", "Please select Item Name!!"); btnicode.Focus(); return; }
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a valid Date"); txtvchdate.Focus(); return; }
        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Back Year Date is not allowed!!'13'Fill date for this year only"); txtvchdate.Focus(); return; }
        cal();
        if (Convert.ToDouble(tk62.Text.Trim()) > 0) fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
        else fgen.msg("-", "AMSG", "Cost Per Carton is Zero!!");
    }
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        clearctrl(); set_Val();
        hffield.Value = "Del";
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
        enablectrl(); hfname.Value = "";
    }
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "List";
        disp_data();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        disp_data();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value; set_Val();
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                fgen.execute_cmd(frm_qstr, co_cd, "delete from " + tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + edmode.Value + "'");
                fgen.msg("-", "AMSG", "Details are deleted for Cost Sheet No. " + edmode.Value.Substring(4, 6) + "");
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
                    case "Del":
                        clearctrl();
                        edmode.Value = col1;
                        fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                        hffield.Value = "D";
                        break;
                    case "Edit":
                        clearctrl();
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, co_cd, "select * from somas_anx A where A.BRANCHCD||A.TYPE||A.vchnum||TO_CHAR(A.vchdate,'DD/MM/YYYY') IN ('" + col1.Trim() + "')");
                        if (dt.Rows.Count > 0)
                        {
                            ViewState["fstr"] = col1; ViewState["ent_by"] = dt.Rows[0]["ent_by"].ToString().Trim(); ViewState["ent_dt"] = dt.Rows[0]["ent_dt"].ToString().Trim();
                            txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim(); txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                            txtacode.Text = dt.Rows[0]["acode"].ToString().Trim(); txticode.Text = dt.Rows[0]["icode"].ToString().Trim();
                            txtaname.Text = fgen.seek_iname(frm_qstr, co_cd, "select aname from famst where trim(acode)='" + dt.Rows[0]["acode"].ToString().Trim() + "'", "aname");
                            txtiname.Text = fgen.seek_iname(frm_qstr, co_cd, "select iname from item where trim(icode)='" + dt.Rows[0]["icode"].ToString().Trim() + "'", "iname");
                            int x = 71;
                            for (int i = 1; i <= x; i++)
                            {
                                string txtbid = "ctl00$ContentPlaceHolder1$tk" + i.ToString();
                                string colid = "t" + i.ToString();
                                TextBox txtv = Page.FindControl(txtbid.Trim()) as TextBox;
                                txtv.Text = dt.Rows[0][colid].ToString().Trim();
                            }
                            if (dt.Rows[0]["t119"].ToString().Trim() == "MANUAL")
                            {
                                txtaname.Text = dt.Rows[0]["t120"].ToString().Trim();
                                txtiname.Text = dt.Rows[0]["t121"].ToString().Trim();

                                btnacode.Visible = false; txtacode.Visible = false;
                                btnicode.Visible = false; txticode.Visible = false; hfname.Value = "MANUAL";
                            }
                            txtrmk.Text = dt.Rows[0]["t140"].ToString().Trim();
                            cal(); edmode.Value = "Y";
                            fgen.EnableForm(this.Controls); disablectrl();
                        }
                        break;
                    case "ACODE":
                        txtacode.Text = col1;
                        txtaname.Text = fgen.seek_iname(frm_qstr, co_cd, "SELECT ANAME FROM FAMST WHERE TRIM(aCODE)='" + col1.Trim() + "'", "ANAME");
                        btnicode.Focus();
                        break;
                    case "ICODE":
                        txticode.Text = col1;
                        txtiname.Text = fgen.seek_iname(frm_qstr, co_cd, "SELECT INAME FROM ITEM WHERE TRIM(ICODE)='" + col1.Trim() + "'", "INAME");
                        tk1.Focus();
                        break;
                    case "Print":
                        SQuery = "Select a.*,(case when trim(nvl(b.INAME,'-'))='-' then a.t121 else b.INAME end) as INAME from (select a.*,(case when trim(nvl(b.aname,'-'))='-' then a.t120 else b.aname end) as aname from (Select * from somas_anx a where A.BRANCHCD||A.TYPE||A.VCHNUM||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + col1.Trim() + "') a left outer join famst b on trim(a.acode)=trim(b.acode)) a left outer join item b on trim(a.icode)=trim(b.icode) ";
                        fgen.Print_Report(co_cd, frm_qstr, mbr, SQuery, "dsct", "dsct");
                        break;
                    case "List":

                        break;
                }
            }
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        col1 = ""; set_Val();
        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
        if (col1 == "Y")
        {
            if (edmode.Value == "Y") fgen.execute_cmd(frm_qstr, co_cd, "update " + tabname + " set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + ViewState["fstr"].ToString() + "'");

            DataSet oDS = new DataSet();
            oDS = fgen.fill_schema(frm_qstr, co_cd, tabname);
            if (edmode.Value == "Y") vchnum = txtvchnum.Text.Trim();
            else vchnum = fgen.next_no(frm_qstr, co_cd, "select max(vchnum) as vch from " + tabname + " where branchcd='" + mbr + "' and type='" + vty + "' and vchdate " + DateRange + "", 6, "vch");
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = mbr;
            oporow["TYPE"] = vty;
            oporow["vchnum"] = vchnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();
            oporow["acode"] = txtacode.Text.Trim();
            oporow["icode"] = txticode.Text.Trim();

            int x = 71;
            for (int i = 1; i <= x; i++)
            {
                string txtbid = "ctl00$ContentPlaceHolder1$tk" + i.ToString();
                TextBox txtv = Page.FindControl(txtbid.Trim()) as TextBox;
                oporow["T" + i.ToString() + ""] = txtv.Text.ToString().Trim();
            }
            if (hfname.Value == "MANUAL")
            {
                oporow["t119"] = "MANUAL";
                if (txtaname.Text.Trim().Length > 30)
                {
                    oporow["t120"] = txtaname.Text.Trim().Substring(0, 29).ToUpper();                    
                }
                else
                {
                    oporow["t120"] = txtaname.Text.Trim().ToUpper();                    
                }
            }
            if (txtiname.Text.Trim().Length > 30)
                oporow["t121"] = txtiname.Text.Trim().Substring(0, 29).ToUpper();
            else oporow["t121"] = txtiname.Text.Trim();

            oporow["t140"] = txtrmk.Text.Trim().ToUpper();
            if (edmode.Value == "Y")
            {
                oporow["eNt_by"] = uname;
                oporow["eNt_dt"] = System.DateTime.Now.ToShortDateString();
                oporow["edt_by"] = uname;
                oporow["edt_dt"] = System.DateTime.Now.ToShortDateString();
            }
            else
            {
                oporow["eNt_by"] = uname;
                oporow["eNt_dt"] = System.DateTime.Now.ToShortDateString();
                oporow["edt_by"] = "-";
                oporow["eDt_dt"] = System.DateTime.Now.ToShortDateString();
            }
            oDS.Tables[0].Rows.Add(oporow);
            fgen.save_data(frm_qstr, co_cd, oDS, tabname);

            if (edmode.Value == "Y") { fgen.msg("-", "AMSG", "Data Updated Successfully"); fgen.execute_cmd(frm_qstr, co_cd, "delete from " + tabname + " where branchcd='DD' and type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + ViewState["fstr"].ToString().Substring(2, 18) + "'"); }
            else { fgen.msg("-", "AMSG", "Data Saved Successfully"); }
            fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); hfname.Value = "";
        }
    }
    protected void cal()
    {
        fgen.fill_zero(this.Controls);
        try
        {
            tk11.Text = Math.Round(Convert.ToDouble(tk6.Text.Trim()) / Convert.ToDouble(tk4.Text.Trim()), 3).ToString();
            tk19.Text = Math.Round(Convert.ToDouble(tk1.Text.Trim()) * Convert.ToDouble(tk2.Text.Trim()) * Convert.ToDouble(tk5.Text.Trim()), 3).ToString();
            tk20.Text = Math.Round(Convert.ToDouble(tk19.Text.Trim()) / 1550, 3).ToString();
            tk21.Text = tk11.Text; tk22.Text = Math.Round(Convert.ToDouble(tk21.Text.Trim()) / 1000, 6).ToString(); tk23.Text = tk17.Text;
            tk24.Text = Math.Round(Convert.ToDouble(tk22.Text.Trim()) * Convert.ToDouble(tk17.Text.Trim()) / 100, 3).ToString();
            tk25.Text = tk20.Text;
            tk26.Text = Math.Round(Convert.ToDouble(tk22.Text.Trim()) + Convert.ToDouble(tk24.Text.Trim()), 3).ToString();
            tk27.Text = Math.Round(Convert.ToDouble(tk25.Text.Trim()) * Convert.ToDouble(tk26.Text.Trim()), 3).ToString();
            tk28.Text = Math.Round(Convert.ToDouble(tk27.Text.Trim()) * Convert.ToDouble(tk8.Text.Trim()), 3).ToString();
            tk29.Text = tk3.Text; tk30.Text = tk9.Text;
            tk31.Text = Math.Round(Convert.ToDouble(tk29.Text.Trim()) * Convert.ToDouble(tk30.Text.Trim()), 3).ToString();
            tk32.Text = tk3.Text; tk33.Text = tk10.Text;
            tk34.Text = Math.Round(Convert.ToDouble(tk32.Text.Trim()) * Convert.ToDouble(tk33.Text.Trim()) * Convert.ToDouble(tk11.Text.Trim()) / 1000, 3).ToString();
            tk35.Text = Math.Round(Convert.ToDouble(tk1.Text.Trim()) * Convert.ToDouble(tk2.Text.Trim()), 3).ToString();
            tk36.Text = tk12.Text; tk37.Text = tk11.Text;
            tk38.Text = Math.Round(Convert.ToDouble(tk36.Text.Trim()) * Convert.ToDouble(tk37.Text.Trim()) * Convert.ToDouble(tk35.Text.Trim()) / 100, 3).ToString();
            tk39.Text = Math.Round(Convert.ToDouble(tk1.Text.Trim()) * Convert.ToDouble(tk2.Text.Trim()), 3).ToString();
            tk40.Text = tk13.Text; tk41.Text = tk11.Text;
            tk42.Text = Math.Round(Convert.ToDouble(tk39.Text.Trim()) * Convert.ToDouble(tk40.Text.Trim()) * Convert.ToDouble(tk41.Text.Trim()) / 100, 3).ToString();
            tk43.Text = tk15.Text; tk44.Text = tk11.Text;
            tk45.Text = Math.Round(Convert.ToDouble(tk43.Text.Trim()) * Convert.ToDouble(tk44.Text.Trim()) / 1000, 3).ToString();
            tk46.Text = tk14.Text; tk47.Text = tk11.Text;
            tk48.Text = Math.Round(Convert.ToDouble(tk46.Text.Trim()) * Convert.ToDouble(tk47.Text.Trim()) / 1000, 3).ToString();
            tk49.Text = tk18.Text; tk50.Text = tk11.Text;
            tk51.Text = Math.Round(Convert.ToDouble(tk49.Text.Trim()) * Convert.ToDouble(tk50.Text.Trim()) / 1000, 3).ToString();
            tk52.Text = tk7.Text; tk53.Text = tk6.Text;
            tk54.Text = Math.Round(Convert.ToDouble(tk52.Text.Trim()) * Convert.ToDouble(tk53.Text.Trim()) / 1000, 3).ToString();
            tk55.Text = tk16.Text; tk56.Text = tk6.Text;
            tk57.Text = Math.Round(Convert.ToDouble(tk55.Text.Trim()) * Convert.ToDouble(tk56.Text.Trim()) / 1000, 3).ToString();
            // ********************** Total **********************
            tk58.Text = Math.Round(Convert.ToDouble(tk24.Text.Trim()) + Convert.ToDouble(tk28.Text.Trim()) + Convert.ToDouble(tk31.Text.Trim()) + Convert.ToDouble(tk34.Text.Trim()) + Convert.ToDouble(tk38.Text.Trim())
                + Convert.ToDouble(tk42.Text.Trim()) + Convert.ToDouble(tk45.Text.Trim()) + Convert.ToDouble(tk48.Text.Trim()) + Convert.ToDouble(tk51.Text.Trim()) + Convert.ToDouble(tk54.Text.Trim()) + Convert.ToDouble(tk57.Text.Trim()), 3).ToString();
            tk59.Text = Math.Round((Convert.ToDouble(tk58.Text.Trim()) * Convert.ToDouble(tk60.Text.Trim())) / 100, 3).ToString();
            tk61.Text = Math.Round((Convert.ToDouble(tk58.Text.Trim()) + Convert.ToDouble(tk59.Text.Trim())), 3).ToString();
            tk62.Text = Math.Round(Convert.ToDouble(tk61.Text.Trim()) / Convert.ToDouble(tk6.Text.Trim()), 3).ToString();
            //tk63.Text = Math.Round((Convert.ToDouble(tk27.Text.Trim()) * (Convert.ToDouble(tk71.Text.Trim()) / 100)) / Convert.ToDouble(tk6.Text.Trim()), 6).ToString();
            tk63.Text = Math.Round((Convert.ToDouble(tk27.Text.Trim()) * (Convert.ToDouble(tk71.Text.Trim()))) / Convert.ToDouble(tk6.Text.Trim()), 6).ToString();
            tk64.Text = Math.Round(Convert.ToDouble(tk63.Text.Trim()) + Convert.ToDouble(tk62.Text.Trim()), 6).ToString();
            tk65.Text = Math.Round(Convert.ToDouble(tk64.Text.Trim()) + (Convert.ToDouble(tk64.Text.Trim()) * (Convert.ToDouble(tk71.Text.Trim()))), 6).ToString();
        }
        catch { }
    }
    public void myfun()
    {
        //vip = "";
        mq0 = "ContentPlaceHolder1_";
        vip = vip + "<script type='text/javascript'>function calculateSum() {";
        vip = vip + "document.getElementById('" + mq0 + "tk11').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk6').value) / fill_zero(document.getElementById('" + mq0 + "tk4').value)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk19').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk1').value) * fill_zero(document.getElementById('" + mq0 + "tk2').value) * fill_zero(document.getElementById('" + mq0 + "tk5').value)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk20').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk19').value) / 1550).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk21').value = document.getElementById('" + mq0 + "tk11').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk22').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk21').value) / 1000).toFixed(6) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk23').value = document.getElementById('" + mq0 + "tk17').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk24').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk22').value) * fill_zero(document.getElementById('" + mq0 + "tk17').value) / 100).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk25').value = document.getElementById('" + mq0 + "tk20').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk26').value = fill_zero( (fill_zero(document.getElementById('" + mq0 + "tk22').value)*1) + (fill_zero(document.getElementById('" + mq0 + "tk24').value)*1)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk27').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk25').value) * fill_zero(document.getElementById('" + mq0 + "tk26').value)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk28').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk27').value) * fill_zero(document.getElementById('" + mq0 + "tk8').value)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk29').value = document.getElementById('" + mq0 + "tk3').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk30').value = document.getElementById('" + mq0 + "tk9').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk31').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk29').value) * fill_zero(document.getElementById('" + mq0 + "tk30').value)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk32').value = document.getElementById('" + mq0 + "tk3').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk33').value = document.getElementById('" + mq0 + "tk10').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk34').value = fill_zero( (fill_zero(document.getElementById('" + mq0 + "tk32').value) * fill_zero(document.getElementById('" + mq0 + "tk33').value) * fill_zero(document.getElementById('" + mq0 + "tk11').value)) / 1000).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk35').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk1').value) * fill_zero(document.getElementById('" + mq0 + "tk2').value)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk36').value = document.getElementById('" + mq0 + "tk12').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk37').value = document.getElementById('" + mq0 + "tk11').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk38').value = fill_zero( (fill_zero(document.getElementById('" + mq0 + "tk36').value) * fill_zero(document.getElementById('" + mq0 + "tk37').value) * fill_zero(document.getElementById('" + mq0 + "tk35').value)) / 100).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk39').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk1').value) * fill_zero(document.getElementById('" + mq0 + "tk2').value)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk40').value = document.getElementById('" + mq0 + "tk13').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk41').value = document.getElementById('" + mq0 + "tk11').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk42').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk39').value) * fill_zero(document.getElementById('" + mq0 + "tk40').value) * fill_zero(document.getElementById('" + mq0 + "tk41').value) / 100).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk43').value = document.getElementById('" + mq0 + "tk15').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk44').value = document.getElementById('" + mq0 + "tk11').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk45').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk43').value) * fill_zero(document.getElementById('" + mq0 + "tk44').value) / 1000).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk46').value = document.getElementById('" + mq0 + "tk14').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk47').value = document.getElementById('" + mq0 + "tk11').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk48').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk46').value) * fill_zero(document.getElementById('" + mq0 + "tk47').value) / 1000).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk49').value = document.getElementById('" + mq0 + "tk18').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk50').value = document.getElementById('" + mq0 + "tk11').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk51').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk49').value) * fill_zero(document.getElementById('" + mq0 + "tk50').value) / 1000).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk52').value = document.getElementById('" + mq0 + "tk7').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk53').value = document.getElementById('" + mq0 + "tk6').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk54').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk52').value) * fill_zero(document.getElementById('" + mq0 + "tk53').value) / 1000).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk55').value = document.getElementById('" + mq0 + "tk16').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk56').value = document.getElementById('" + mq0 + "tk6').value;";
        vip = vip + "document.getElementById('" + mq0 + "tk57').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk55').value) * fill_zero(document.getElementById('" + mq0 + "tk56').value) / 1000).toFixed(3) ;";
        // ********************** Total **********************
        vip = vip + "document.getElementById('" + mq0 + "tk58').value = fill_zero( (fill_zero(document.getElementById('" + mq0 + "tk24').value)*1) + (fill_zero(document.getElementById('" + mq0 + "tk28').value)*1) + (fill_zero(document.getElementById('" + mq0 + "tk31').value)*1) + (fill_zero(document.getElementById('" + mq0 + "tk34').value)*1) " +
            "+ (fill_zero(document.getElementById('" + mq0 + "tk38').value)*1) + (fill_zero(document.getElementById('" + mq0 + "tk42').value)*1) + (fill_zero(document.getElementById('" + mq0 + "tk45').value)*1) + (fill_zero(document.getElementById('" + mq0 + "tk48').value)*1) + (fill_zero(document.getElementById('" + mq0 + "tk51').value)*1) " +
            "+ (fill_zero(document.getElementById('" + mq0 + "tk54').value)*1) + (fill_zero(document.getElementById('" + mq0 + "tk57').value)*1)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk59').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk58').value) * fill_zero(document.getElementById('" + mq0 + "tk60').value) / 100).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk61').value = fill_zero( ((fill_zero(document.getElementById('" + mq0 + "tk58').value)*1) + (fill_zero(document.getElementById('" + mq0 + "tk59').value)*1))).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk62').value = fill_zero( fill_zero(document.getElementById('" + mq0 + "tk61').value) / fill_zero(document.getElementById('" + mq0 + "tk6').value)).toFixed(3) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk63').value = fill_zero( (fill_zero(document.getElementById('" + mq0 + "tk27').value) * (fill_zero(document.getElementById('" + mq0 + "tk71').value) ) ) / fill_zero(document.getElementById('" + mq0 + "tk6').value)).toFixed(6) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk64').value = fill_zero( (fill_zero(document.getElementById('" + mq0 + "tk63').value)*1) + (fill_zero(document.getElementById('" + mq0 + "tk62').value)*1)).toFixed(6) ;";
        vip = vip + "document.getElementById('" + mq0 + "tk65').value = fill_zero( (fill_zero(document.getElementById('" + mq0 + "tk64').value)*1) + ((fill_zero(document.getElementById('" + mq0 + "tk64').value) * ( fill_zero(document.getElementById('" + mq0 + "tk71').value) ))*1)).toFixed(3) ;";
        vip = vip + "}";
        vip = vip + "function fill_zero(val){ if(isNaN(val)) return 0; if(isFinite(val)) return val; }</script>";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", vip.ToString(), false);
    }
    protected void txtaname_TextChanged(object sender, EventArgs e)
    {
        btnacode.Visible = false; txtacode.Visible = false;
        btnicode.Visible = false; txticode.Visible = false; txtiname.ReadOnly = false;
        txticode.Text = ""; txtacode.Text = "";
        hfname.Value = "MANUAL"; txtiname.Focus();
    }
    protected void btnacode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ACODE";
        disp_data();
        fgen.Fn_open_sseek("Select Customer", frm_qstr);
    }
    protected void btnicode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ICODE";
        disp_data();
        fgen.Fn_open_sseek("Select Product", frm_qstr);
    }
}