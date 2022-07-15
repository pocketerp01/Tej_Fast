using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class bsrv_action : System.Web.UI.Page
{
    string btnval, SQuery, co_cd, uname, col1, col2, col3, frm_mbr, vchnum, DateRange, year, HCID, ulvl, merr = "0", SQuery1, SQuery2, vardate, fromdt, todt, typePopup = "N";
    fgenDB fgen = new fgenDB();
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0; string mq0, mq1, mq2;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string ord_qty_valid;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", PrdRange, cmd_query;
    string frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, custom_filing_no;

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
                //doc_addl.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select (case when nvl(st_Sc,1)=0 then 1 else nvl(st_Sc,1) end )  as add_tx from type where id='B' and trim(upper(type1))=upper(Trim('" + frm_mbr + "'))", "add_tx");
                doc_addl.Value = "-";
                fgen.DisableForm(this.Controls);
                enablectrl();
            }
            set_Val();
            //  btnprint.Visible = false;
        }
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btncontactmode.Enabled = false; btnsrvtype.Enabled = false; btnfirstprsn.Enabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnparty.Enabled = false; btndealername.Enabled = false; btnengdeputed.Enabled = false;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnparty.Enabled = true; btndealername.Enabled = false; btnfirstprsn.Enabled = true;//DEALER BTN ABI DIABLE HAI QKI NO IDEA FROM WHERE DELAER SHOULD BE COME...SO MAKE IT USER ENTRY FIELD NOW
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; btnengdeputed.Enabled = true; btncontactmode.Enabled = true; btnsrvtype.Enabled = true;
    }
    //------------------------------------------------------------------------------------
    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
    }
    //------------------------------------------------------------------------------------
    public void set_Val()
    {
        //  HCID = Request.Cookies["rid"].Value.ToString();
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        switch (frm_formID)
        {
            case "F10351":
                lblheader.Text = "Srv. Req Entry";
                lbl1a.Text = "CC";
                frm_tabname = "WB_SERVICE"; frm_vty = "CC";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CC");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
                tab_bsrvreq.Visible = true;
                tab_actionbyho.Visible = false;
                img_div.Visible = false;
                tab_actionbyjaycee.Visible = false;
                break;
            case "F10352":
                lblheader.Text = "Action by HO";
                lbl1a.Text = "CH";
                frm_tabname = "WB_SERVICE"; frm_vty = "CH";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CH");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
                tab_bsrvreq.Visible = true;
                tab_actionbyho.Visible = true;
                tab_actionbyjaycee.Visible = false;
                img_div.Visible = false;
                #region Make textboxs readonly
                txtDGsrno.ReadOnly = true;
                txtsiteid.ReadOnly = true;
                txtsitename.ReadOnly = true;
                txtaddr1.ReadOnly = true;
                txtaddr2.ReadOnly = true;
                txtaddr3.ReadOnly = true;
                txtdealername.ReadOnly = false;//WHEN BUTTON IS ENABLE THEM DO IT READONLY TRUE
                txtcallatt.ReadOnly = true;
                txtcustpo.ReadOnly = true;
                txtcustpodt.ReadOnly = true;
                txtequipment.ReadOnly = true;
                txtcontactper.ReadOnly = true;
                txttel.ReadOnly = true;
                txtdesignation.ReadOnly = true;
                txttaddr1.ReadOnly = true;
                txttaddr2.ReadOnly = true;
                txttaddr3.ReadOnly = true;
                txtemailid.ReadOnly = true;
                txtprob.ReadOnly = true;
                txtrmk.ReadOnly = true;
                dd_list1.Enabled = false;
                ddresonforfail.Enabled = false;
                txtinstruction.ReadOnly = false;
                txtvchdate.ReadOnly = true;
                btnparty.Enabled = false;
                #endregion
                break;
            case "F10353":
                lblheader.Text = "Action by Service Engineer";
                lbl1a.Text = "CE";
                frm_tabname = "WB_SERVICE"; frm_vty = "CE";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CE");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
                tab_bsrvreq.Visible = true;
                tab_actionbyho.Visible = true;
                tab_actionbyjaycee.Visible = true;
                img_div.Visible = true;
                #region Make textboxs readonly
                txtDGsrno.ReadOnly = true;
                txtsiteid.ReadOnly = true;
                txtsitename.ReadOnly = true;
                txtaddr1.ReadOnly = true;
                txtaddr2.ReadOnly = true;
                txtaddr3.ReadOnly = true;
                txtcallatt.ReadOnly = true;
                txtcustpo.ReadOnly = true;
                txtcustpodt.ReadOnly = true;
                txtequipment.ReadOnly = true;
                txtcontactper.ReadOnly = true;
                txttel.ReadOnly = true;
                txtdesignation.ReadOnly = true;
                txttaddr1.ReadOnly = true;
                txttaddr2.ReadOnly = true;
                txttaddr3.ReadOnly = true;
                txtemailid.ReadOnly = true;
                txtprob.ReadOnly = true;
                txtrmk.ReadOnly = true;
                txtcatg.ReadOnly = true;
                //**************************
                txtengdupted.ReadOnly = true;
                txtcontactmode.ReadOnly = true;
                txtdeputdt.ReadOnly = true;
                txtsrvtype.ReadOnly = true;
                txtdealername.ReadOnly = true;
                txtperson.ReadOnly = true;
                txtinstruction.ReadOnly = true;
                dd_list1.Enabled = false;
                ddresonforfail.Enabled = false;
                btncatg.Enabled = false;
                btncontactmode.Enabled = false;
                btnengdeputed.Enabled = false;
                btnsrvtype.Enabled = false;
                btnparty.Enabled = false;
                btndealername.Enabled = false;
                btnfirstprsn.Enabled = false;
                //rdcategory.Enabled = false;
                txtdocdt.ReadOnly = true;
                #endregion
                break;
        }
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

            case "NEW_E_":
                switch (frm_formID)
                {
                    case "F10352":
                        SQuery = "SELECT FSTR,PARTY,CODE,DOC_NO,DOC_dT,VDD,SUM(QTY) AS PEND_FOR_HO FROM (SELECT A.BRANCHCD||TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS FSTR,B.ANAME AS PARTY,A.ACODE AS CODE,A.VCHNUM AS DOC_NO,TO_cHAR(a.VCHDATE,'DD/MM/YYYY') AS DOC_dT,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD,1 AS QTY FROM " + frm_tabname + " A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODe) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='CC' AND A.VCHDATE  " + DateRange + " union all SELECT A.BRANCHCD||TRIM(a.REFNUM)||TO_CHAR(a.REFDATE,'DD/MM/YYYY') AS FSTR,B.ANAME AS PARTY,A.ACODE AS CODE,A.REFNUM AS DOC_NO,TO_cHAR(a.REFDATE,'DD/MM/YYYY') AS DOC_dT,TO_CHAR(A.REFDATE,'YYYYMMDD') AS VDD,-1 AS QTY FROM " + frm_tabname + " A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODe) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='CH' AND A.REFDATE " + DateRange + ") HAVING SUM(QTY)>0 GROUP BY FSTR,PARTY,CODE,DOC_NO,DOC_dT,VDD ORDER BY VDD DESC";
                        SQuery = "SELECT FSTR,PARTY,MC_NO,CODE,DOC_NO,DOC_dT,VDD,SUM(QTY) AS PEND_FOR_HO FROM (SELECT A.BRANCHCD||TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS FSTR,B.ANAME AS PARTY,A.ACODE AS CODE,A.VCHNUM AS DOC_NO,TO_cHAR(a.VCHDATE,'DD/MM/YYYY') AS DOC_dT,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD,DGSRNO AS MC_NO,1 AS QTY FROM " + frm_tabname + " A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODe) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='CC' AND A.VCHDATE  " + DateRange + " union all SELECT A.BRANCHCD||TRIM(a.REFNUM)||TO_CHAR(a.REFDATE,'DD/MM/YYYY') AS FSTR,B.ANAME AS PARTY,A.ACODE AS CODE,A.REFNUM AS DOC_NO,TO_cHAR(a.REFDATE,'DD/MM/YYYY') AS DOC_dT,TO_CHAR(A.REFDATE,'YYYYMMDD') AS VDD,DGSRNO AS MC_NO,-1 AS QTY FROM " + frm_tabname + " A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODe) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='CH' AND A.REFDATE " + DateRange + ") HAVING SUM(QTY)>0 GROUP BY FSTR,PARTY,CODE,DOC_NO,MC_NO,DOC_dT,VDD ORDER BY VDD DESC";
                        break;
                    case "F10353":
                        SQuery = "SELECT A.BRANCHCD||a.TYPE||TRIM(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS FSTR,B.ANAME AS PARTY,a.dgsrno as MC_NO,A.VCHNUM AS DOC_NO,TO_cHAR(a.VCHDATE,'DD/MM/YYYY') AS DOC_dT,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD,A.ACODE AS CODE FROM " + frm_tabname + " A left outer join FAMST B on TRIM(A.ACODE)=TRIM(B.ACODe) where A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='CH' AND A.VCHDATE " + DateRange + "  and nvl(trim(a.chk_by),'-')='-' ORDER BY VDD DESC ";//old and running                      
                        break;
                }
                break;
            case "TS":
                SQuery = "select type1 as fstr,replace(name,'&','') as Category,type1 as code from typegrp where id='CT'  order by type1";
                break;
            case "SRVT":
                SQuery = "Select 'P1' as col1,'Payable' as Text,'-' as Action from dual union all Select 'F1' as col1,'Free' as Text,'-' as Action from dual union all Select 'W1' as col1,'Warranty' as Text,'-' as Action from dual";
                break;
            case "CMOD":
                SQuery = "Select 'T1' as col1,'Telecom' as Text,'-' as Action from dual union all Select 'E1' as col1,'Email' as Text,'-' as Action from dual union all Select 'F1' as col1,'Fax' as Text,'-' as Action from dual";
                break;
            case "ENG":
            case "PERSON":
                //SQuery = "select replace(name,'&','') as FSTR,replace(name,'&','') as NAME,EMPCODE from EMPMAS order by fstr";//old
                SQuery = "select username as fstr,username,userid as id,emailid,contactno from evas order by id";
                break;
            case "DL":
                SQuery = "SELECT ANAME AS FSTR,ANAME AS DEALER_NAME,ACODE AS CODE,ADDR1,ADDR2,ADDR3,TELNUM FROM CSMST WHERE TYPE='DL' ORDER BY ANAME";
                break;
            case "CUST":
                SQuery = "select acode,aname as customer_name,acode as customer_Code from famst where substr(Acode,1,2)='16'";
                break;
            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (frm_formID == "F10353")
                {
                    if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                        //SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,a.DGSRNO as MC_No,a.site_name,b.aname as cust_name,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.type,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                        SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.DGSRNO as MC_No,a.site_name,b.aname as cust_name,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.type,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a LEFT OUTER JOIN famst b ON trim(a.acode)=trim(b.acode) WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                }
                else
                {
                    if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                        //    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.type,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                        SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,a.DGSRNO as MC_No,a.site_name,b.aname as cust_name,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.type,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a LEFT OUTER JOIN famst b ON trim(a.acode)=trim(b.acode) WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
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
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F50111":
                SQuery = "SELECT '46' AS FSTR,'Sales Schedule' as NAME,'46' AS CODE FROM dual";
                break;
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
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
        }
    }
    //=================================================================
    void newCase(string vty)
    {
        #region
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
        switch (frm_formID)
        {
            case "F10351":
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
                txtvchnum.Text = frm_vnum;
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                disablectrl();
                fgen.EnableForm(this.Controls);
                hffield.Value = "TACODE";
                break;
            case "F10352":
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
                txtdocno.Text = frm_vnum;
                txtdocdt.Text = vardate;
                disablectrl();
                fgen.EnableForm(this.Controls);
                hffield.Value = "NEW_E_";
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select " + lblheader.Text + "", frm_qstr);
                break;
            case "F10353":
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
                txtdocno.Text = frm_vnum;
                txtdocdt.Text = vardate;
                disablectrl();
                fgen.EnableForm(this.Controls);
                hffield.Value = "NEW_E_";
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select " + lblheader.Text + "", frm_qstr);
                break;
        }
        #endregion
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
        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (frm_formID == "F10351")
        {
            if (txtDGsrno.Text.Trim().Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Enter a valid M/C no!!");
                return;
            }
            if (txtsiteid.Text.Trim().Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Fill Site ID!!");
                return;
            }
            if (txtsitename.Text.Trim().Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Fill Site Name!!");
                return;
            }
            if (txttaddr1.Text.Trim().Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Fill Site Address1!!");
                return;
            }
            if (txtprob.Text.Trim().Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Enter Problem Observed!!");
                return;
            }
            if (txtcallatt.Text.Trim().Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Fill Call Detail/Attended By !!");
                return;
            }
        }
        if (frm_formID == "F10352")
        {
            dhd = fgen.ChkDate(txtdeputdt.Text.ToString());
            if (dhd == 0)
            {
                fgen.msg("-", "AMSG", "Please Deputed Date !!");
                return;
            }
            if (txtengdupted.Text.Trim().Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Select Eng.Deputed !!");
                return;
            }
            //if (txtperson.Text.Trim().Length < 2)
            //{
            //    fgen.msg("-", "AMSG", "Please Select First Person !!");
            //    return;
            //}
            if (txtinstruction.Text.Trim().Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Fill Instruction to Engineer !!");
                return;
            }
        }
        if (frm_formID == "F10353")
        {
            if (rdworkdone.SelectedValue == "1")
            {
                dhd = fgen.ChkDate(txtnexttgt.Text.ToString());
                if (dhd == 0)
                {
                    fgen.msg("-", "AMSG", "Please Fill Next Target Date !!");
                    return;
                }
            }
            if (txtengrmk.Text.Trim().Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Enter Engineer Remarks !!");
                return;
            }
            if (txtmetwhom.Text.Trim().Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Fill Whom want to meet!!");
                return;
            }
            if (rdworkdone.SelectedValue == "0")
            {
                dhd = fgen.ChkDate(txtclosdt.Text.ToString());
                if (dhd == 0)
                {
                    fgen.msg("-", "AMSG", "Please Fill Close Date !!");
                    return;
                }
            }
        }
        Cal();
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
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        set_Val();
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                clearctrl(); fgen.ResetForm(this.Controls);
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
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;

                case "Del_E":
                    if (col1 == "") return;
                    clearctrl();
                    edmode.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;

                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Edit", frm_qstr);
                    break;

                case "TS":
                    if (col1 == "") return;
                    txtcatg.Text = col2;
                    break;

                case "Edit_E":
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,b.aname,c.iname from " + frm_tabname + " a left outer join famst b on trim(a.acode)=trim(B.acode) left outer join item c on trim(a.icode)=trim(c.icode) where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + col1 + "'";
                    ViewState["fstr"] = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        #region
                        switch (frm_formID)
                        {
                            case "F10351":
                                #region
                                txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                                txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdatE"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                                txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                                txticode.Text = dt.Rows[0]["icode"].ToString().Trim();
                                txtiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                                txtinvno.Text = dt.Rows[0]["invno"].ToString().Trim();
                                txtinvdate.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString().Trim();
                                txtemailid.Text = dt.Rows[0]["EMAIL_ID"].ToString().Trim();
                                txtDGsrno.Text = dt.Rows[0]["DGSRNO"].ToString().Trim();
                                txtengno.Text = dt.Rows[0]["ENGNO"].ToString().Trim();
                                txtguarnty_status.Text = dt.Rows[0]["G_STATUS"].ToString().Trim();
                                txtcontactper.Text = dt.Rows[0]["CONT_PER"].ToString().Trim();
                                txttel.Text = dt.Rows[0]["TELNO"].ToString().Trim();
                                txtdesignation.Text = dt.Rows[0]["DESG"].ToString().Trim();
                                txtaddr1.Text = dt.Rows[0]["ADDR1"].ToString().Trim();
                                txtaddr2.Text = dt.Rows[0]["ADDR2"].ToString().Trim();
                                txtaddr3.Text = dt.Rows[0]["ADDR3"].ToString().Trim();
                                txttaddr1.Text = dt.Rows[0]["ADDR4"].ToString().Trim();
                                txttaddr2.Text = dt.Rows[0]["ADDR5"].ToString().Trim();
                                txttaddr3.Text = dt.Rows[0]["ADDR6"].ToString().Trim();
                                txtcallatt.Text = dt.Rows[0]["CALL_DTL"].ToString().Trim();
                                txtcustpo.Text = dt.Rows[0]["CUST_PO_NO"].ToString().Trim();
                                txtcustpodt.Text = dt.Rows[0]["CUST_PO_DT"].ToString().Trim();
                                txtequipment.Text = dt.Rows[0]["EQUIP"].ToString().Trim();
                                txtprob.Text = dt.Rows[0]["PROB_OBSV"].ToString().Trim();
                                txtrmk.Text = dt.Rows[0]["RMK1"].ToString().Trim();
                                try { dd_list1.SelectedValue = dt.Rows[0]["PREV_MNT"].ToString().Trim(); }
                                catch { }
                                txtsiteid.Text = dt.Rows[0]["SITE_ID"].ToString().Trim();
                                txtsitename.Text = dt.Rows[0]["SITE_NAME"].ToString().Trim();
                                txtocrhr.Text = dt.Rows[0]["OCCR_TIME"].ToString().Trim();
                                try { ddresonforfail.SelectedItem.Text = dt.Rows[0]["REASON_FAIL"].ToString().Trim(); }
                                catch { }
                                #endregion
                                break;

                            case "F10352":
                                #region
                                txtdocno.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                                txtdocdt.Text = Convert.ToDateTime(dt.Rows[0]["vchdatE"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtvchnum.Text = dt.Rows[0]["REFNUM"].ToString().Trim();
                                txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["refdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                                txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                                txticode.Text = dt.Rows[0]["icode"].ToString().Trim();
                                txtiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                                txtemailid.Text = dt.Rows[0]["EMAIL_ID"].ToString().Trim();
                                txtDGsrno.Text = dt.Rows[0]["DGSRNO"].ToString().Trim();
                                txtengno.Text = dt.Rows[0]["ENGNO"].ToString().Trim();
                                txtguarnty_status.Text = dt.Rows[0]["G_STATUS"].ToString().Trim();
                                txtcontactper.Text = dt.Rows[0]["CONT_PER"].ToString().Trim();
                                txttel.Text = dt.Rows[0]["TELNO"].ToString().Trim();
                                txtdesignation.Text = dt.Rows[0]["DESG"].ToString().Trim();
                                txtaddr1.Text = dt.Rows[0]["ADDR1"].ToString().Trim();
                                txtaddr2.Text = dt.Rows[0]["ADDR2"].ToString().Trim();
                                txtaddr3.Text = dt.Rows[0]["ADDR3"].ToString().Trim();
                                txttaddr1.Text = dt.Rows[0]["ADDR4"].ToString().Trim();
                                txttaddr2.Text = dt.Rows[0]["ADDR5"].ToString().Trim();
                                txttaddr3.Text = dt.Rows[0]["ADDR6"].ToString().Trim();
                                txtcallatt.Text = dt.Rows[0]["CALL_DTL"].ToString().Trim();
                                txtcustpo.Text = dt.Rows[0]["CUST_PO_NO"].ToString().Trim();
                                txtcustpodt.Text = dt.Rows[0]["CUST_PO_DT"].ToString().Trim();
                                txtequipment.Text = dt.Rows[0]["EQUIP"].ToString().Trim();
                                txtprob.Text = dt.Rows[0]["PROB_OBSV"].ToString().Trim();
                                txtrmk.Text = dt.Rows[0]["RMK1"].ToString().Trim();
                                txtengdupted.Text = dt.Rows[0]["ENG_DEPUTED"].ToString().Trim();
                                txtcontactmode.Text = dt.Rows[0]["CONT_MODE"].ToString().Trim();
                                txtdeputdt.Text = dt.Rows[0]["DEPUTE_DT"].ToString().Trim();
                                txtcontactmode.Text = dt.Rows[0]["CONT_MODE"].ToString().Trim();
                                txtsrvtype.Text = dt.Rows[0]["SRV_TYPE"].ToString().Trim();
                                txtdealername.Text = dt.Rows[0]["DEALER_NAME"].ToString().Trim();
                                txtperson.Text = dt.Rows[0]["FIRST_PER"].ToString().Trim();
                                txtinstruction.Text = dt.Rows[0]["ENG_INSTRUCT"].ToString().Trim();
                                txtinvno.Text = dt.Rows[0]["invno"].ToString().Trim();
                                txtinvdate.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                //try { dd_list1.SelectedValue = dt.Rows[0]["PREV_MNT"].ToString().Trim(); }
                                //catch { }
                                txtcatg.Text = dt.Rows[0]["category"].ToString().Trim();
                                try { dd_list1.SelectedItem.Text = dt.Rows[0]["PREV_MNT"].ToString().Trim(); }
                                catch { }
                                try { ddresonforfail.SelectedItem.Text = dt.Rows[0]["REASON_FAIL"].ToString().Trim(); }
                                catch { }
                                txtcontact.Text = dt.Rows[0]["ENG_DEP_MOB"].ToString().Trim();//ENG DEPUTED MOB NO                              
                                txtpersoncontct.Text = dt.Rows[0]["F_PER_MOB"].ToString().Trim(); //FIRST PERSON MOBILE NO
                                txtsiteid.Text = dt.Rows[0]["SITE_ID"].ToString().Trim();
                                txtsitename.Text = dt.Rows[0]["SITE_NAME"].ToString().Trim();
                                txtocrhr.Text = dt.Rows[0]["OCCR_TIME"].ToString().Trim();
                                //******************************                                                                                                                                                                                                                                                                                                                        
                                // oporow["CATEGORY"] = rdcategory.SelectedValue.ToString().Trim();
                                #endregion
                                break;
                            case "F10353":
                                #region
                                txtactionvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                                txtactionvchdt.Text = Convert.ToDateTime(dt.Rows[0]["vchdatE"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtvchnum.Text = dt.Rows[0]["refnum"].ToString().Trim();
                                txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["refdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtdocno.Text = dt.Rows[0]["DOCNO"].ToString().Trim();
                                txtdocdt.Text = Convert.ToDateTime(dt.Rows[0]["docdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                                txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                                txticode.Text = dt.Rows[0]["icode"].ToString().Trim();
                                txtiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                                txtemailid.Text = dt.Rows[0]["EMAIL_ID"].ToString().Trim();
                                txtDGsrno.Text = dt.Rows[0]["DGSRNO"].ToString().Trim();
                                txtengno.Text = dt.Rows[0]["engno"].ToString().Trim();
                                txtguarnty_status.Text = dt.Rows[0]["G_STATUS"].ToString().Trim();
                                txtcontactper.Text = dt.Rows[0]["CONT_PER"].ToString().Trim();
                                txttel.Text = dt.Rows[0]["TELNO"].ToString().Trim();
                                txtdesignation.Text = dt.Rows[0]["DESG"].ToString().Trim();
                                txtaddr1.Text = dt.Rows[0]["ADDR1"].ToString().Trim();
                                txtaddr2.Text = dt.Rows[0]["ADDR2"].ToString().Trim();
                                txtaddr3.Text = dt.Rows[0]["ADDR3"].ToString().Trim();
                                txttaddr1.Text = dt.Rows[0]["ADDR4"].ToString().Trim();
                                txttaddr2.Text = dt.Rows[0]["ADDR5"].ToString().Trim();
                                txttaddr3.Text = dt.Rows[0]["ADDR6"].ToString().Trim();
                                txtcallatt.Text = dt.Rows[0]["CALL_DTL"].ToString().Trim();
                                txtcustpo.Text = dt.Rows[0]["CUST_PO_NO"].ToString().Trim();
                                txtcustpodt.Text = dt.Rows[0]["CUST_PO_dt"].ToString().Trim();
                                txtequipment.Text = dt.Rows[0]["EQUIP"].ToString().Trim();
                                txtprob.Text = dt.Rows[0]["PROB_OBSV"].ToString().Trim();
                                txtrmk.Text = dt.Rows[0]["RMK1"].ToString().Trim();
                                txtengdupted.Text = dt.Rows[0]["ENG_DEPUTED"].ToString().Trim();
                                txtcontactmode.Text = dt.Rows[0]["CONT_MODE"].ToString().Trim();
                                txtdeputdt.Text = dt.Rows[0]["DEPUTE_DT"].ToString().Trim();
                                txtsrvtype.Text = dt.Rows[0]["SRV_TYPE"].ToString().Trim();
                                txtdealername.Text = dt.Rows[0]["DEALER_NAME"].ToString().Trim();
                                txtperson.Text = dt.Rows[0]["FIRST_PER"].ToString().Trim();
                                txtinstruction.Text = dt.Rows[0]["ENG_INSTRUCT"].ToString().Trim();
                                txtinvno.Text = dt.Rows[0]["invno"].ToString().Trim();
                                txtinvdate.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txttimein.Text = dt.Rows[0]["TIME_IN"].ToString().Trim();
                                txttimeout.Text = dt.Rows[0]["TIME_OUT"].ToString().Trim();
                                txtnexttgt.Text = dt.Rows[0]["NXT_TRGT"].ToString().Trim();
                                txtengrmk.Text = dt.Rows[0]["ENG_RMK"].ToString().Trim();
                                txtcorraction.Text = dt.Rows[0]["CORR_ACT"].ToString().Trim();
                                txtprevntiveaction.Text = dt.Rows[0]["PREVEN_ACT"].ToString().Trim();
                                txtreasonforpend.Text = dt.Rows[0]["REASON_PEND"].ToString().Trim();
                                txtspares.Text = dt.Rows[0]["SPARES_RQD"].ToString().Trim();
                                txtrmkactionbyeng.Text = dt.Rows[0]["RMK2"].ToString().Trim();
                                txtsrvcost.Text = dt.Rows[0]["SERV_COST"].ToString().Trim();
                                txtsparecost.Text = dt.Rows[0]["SPARE_COST"].ToString().Trim();
                                txtmisccost.Text = dt.Rows[0]["MISC_COST"].ToString().Trim();
                                txttravconv.Text = dt.Rows[0]["TRAVEL_CONV"].ToString().Trim();
                                txttotcost.Text = dt.Rows[0]["tot_cost"].ToString().Trim();
                                // txthmrcost.Text = dt.Rows[0]["HMR"].ToString().Trim();
                                txtsiteid.Text = dt.Rows[0]["SITE_ID"].ToString().Trim();
                                txtsitename.Text = dt.Rows[0]["SITE_NAME"].ToString().Trim();
                                txtocrhr.Text = dt.Rows[0]["OCCR_TIME"].ToString().Trim();
                                try { dd_list1.SelectedValue = dt.Rows[0]["PREV_MNT"].ToString().Trim(); }
                                catch { }
                                try { ddresonforfail.SelectedItem.Text = dt.Rows[0]["REASON_FAIL"].ToString().Trim(); }
                                catch { }
                                txtcatg.Text = dt.Rows[0]["CATEGORY"].ToString().Trim();
                                txtmetwhom.Text = dt.Rows[0]["met_whom"].ToString().Trim();
                                rdworkdone.SelectedValue = dt.Rows[0]["WORK_DONE"].ToString().Trim();
                                if (dt.Rows[0]["filename"].ToString().Trim().Length > 1)
                                {
                                    lblUpload.Text = dt.Rows[i]["filepath"].ToString().Trim();
                                    txtAttch.Text = dt.Rows[i]["filename"].ToString().Trim();
                                }
                                #endregion
                                break;
                        }
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        Cal();
                        edmode.Value = "Y";
                        #endregion
                    }
                    break;

                case "NEW_E_":
                    if (col1 == "") return;
                    switch (frm_formID)
                    {
                        case "F10352":
                            SQuery = "select a.*,b.aname,c.iname from " + frm_tabname + " a left outer join famst b on trim(A.acode)=trim(B.acodE) left outer join item c on trim(a.icode)=trim(c.icode) where a.branchcd||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' AND A.TYPE='CC' ";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                #region
                                txtdocno.Text = fgen.next_no(frm_qstr, co_cd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                                txtdocdt.Text = vardate;
                                ///=========filling F10351 detail
                                txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                                txtvchdate.Text = dt.Rows[0]["vchdate"].ToString().Trim();
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                                txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                                txticode.Text = dt.Rows[0]["icode"].ToString().Trim();
                                txtiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                                txtinvno.Text = dt.Rows[0]["invno"].ToString().Trim();
                                txtinvdate.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString().Trim();
                                txtemailid.Text = dt.Rows[0]["EMAIL_ID"].ToString().Trim();
                                txtDGsrno.Text = dt.Rows[0]["DGSRNO"].ToString().Trim();
                                txtengno.Text = dt.Rows[0]["ENGNO"].ToString().Trim();
                                txtguarnty_status.Text = dt.Rows[0]["G_STATUS"].ToString().Trim();
                                txtcontactper.Text = dt.Rows[0]["CONT_PER"].ToString().Trim();
                                txttel.Text = dt.Rows[0]["TELNO"].ToString().Trim();
                                txtdesignation.Text = dt.Rows[0]["DESG"].ToString().Trim();
                                txtaddr1.Text = dt.Rows[0]["ADDR1"].ToString().Trim();
                                txtaddr2.Text = dt.Rows[0]["ADDR2"].ToString().Trim();
                                txtaddr3.Text = dt.Rows[0]["ADDR3"].ToString().Trim();
                                txttaddr1.Text = dt.Rows[0]["ADDR4"].ToString().Trim();
                                txttaddr2.Text = dt.Rows[0]["ADDR5"].ToString().Trim();
                                txttaddr3.Text = dt.Rows[0]["ADDR6"].ToString().Trim();
                                txtcallatt.Text = dt.Rows[0]["CALL_DTL"].ToString().Trim();
                                txtcustpo.Text = dt.Rows[0]["CUST_PO_NO"].ToString().Trim();
                                txtcustpodt.Text = dt.Rows[0]["CUST_PO_DT"].ToString().Trim();
                                txtequipment.Text = dt.Rows[0]["EQUIP"].ToString().Trim();
                                txtprob.Text = dt.Rows[0]["PROB_OBSV"].ToString().Trim();
                                txtrmk.Text = dt.Rows[0]["RMK1"].ToString().Trim();
                                try { dd_list1.SelectedItem.Text = dt.Rows[0]["PREV_MNT"].ToString().Trim(); }
                                catch { }
                                txtsiteid.Text = dt.Rows[0]["SITE_ID"].ToString().Trim();
                                txtsitename.Text = dt.Rows[0]["SITE_NAME"].ToString().Trim();
                                txtocrhr.Text = dt.Rows[0]["OCCR_TIME"].ToString().Trim();
                                try { ddresonforfail.SelectedItem.Text = dt.Rows[0]["REASON_FAIL"].ToString().Trim(); }
                                catch { }
                                #endregion
                            }
                            break;

                        case "F10353":
                            SQuery = "select a.*,b.aname,c.iname from " + frm_tabname + " a left outer join famst b on trim(A.acode)=trim(B.acodE) left outer join item c on trim(a.icode)=trim(c.icode) where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "'";
                            //SQuery = "select a.*,b.aname from " + frm_tabname + " a,famst b where trim(A.acode)=trim(B.acodE) and a.branchcd||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' AND TYPE='CH'";
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count > 0)
                            {
                                #region
                                txtactionvchnum.Text = fgen.next_no(frm_qstr, co_cd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                                txtactionvchdt.Text = vardate;
                                ///=========filling F10351 detail
                                txtdocno.Text = dt.Rows[0]["vchnum"].ToString().Trim(); //2nd form vchnum
                                txtdocdt.Text = dt.Rows[0]["vchdate"].ToString().Trim(); //2nd form vchdate
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                                txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                                txticode.Text = dt.Rows[0]["icode"].ToString().Trim();
                                txtiname.Text = dt.Rows[0]["iname"].ToString().Trim();
                                txtinvno.Text = dt.Rows[0]["invno"].ToString().Trim();
                                txtinvdate.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString().Trim();
                                txtvchnum.Text = dt.Rows[0]["refnum"].ToString().Trim();//1st from vchnum
                                txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["refdate"].ToString().Trim()).ToString().Trim();//1st form vchdate
                                txtemailid.Text = dt.Rows[0]["EMAIL_ID"].ToString().Trim();
                                txtDGsrno.Text = dt.Rows[0]["DGSRNO"].ToString().Trim();
                                txtengno.Text = dt.Rows[0]["ENGNO"].ToString().Trim();
                                txtguarnty_status.Text = dt.Rows[0]["G_STATUS"].ToString().Trim();
                                txtcontactper.Text = dt.Rows[0]["CONT_PER"].ToString().Trim();
                                txttel.Text = dt.Rows[0]["TELNO"].ToString().Trim();
                                txtdesignation.Text = dt.Rows[0]["DESG"].ToString().Trim();
                                txtaddr1.Text = dt.Rows[0]["ADDR1"].ToString().Trim();
                                txtaddr2.Text = dt.Rows[0]["ADDR2"].ToString().Trim();
                                txtaddr3.Text = dt.Rows[0]["ADDR3"].ToString().Trim();
                                txttaddr1.Text = dt.Rows[0]["ADDR4"].ToString().Trim();
                                txttaddr2.Text = dt.Rows[0]["ADDR5"].ToString().Trim();
                                txttaddr3.Text = dt.Rows[0]["ADDR6"].ToString().Trim();
                                txtcallatt.Text = dt.Rows[0]["CALL_DTL"].ToString().Trim();
                                txtcustpo.Text = dt.Rows[0]["CUST_PO_NO"].ToString().Trim();
                                txtcustpodt.Text = dt.Rows[0]["CUST_PO_DT"].ToString().Trim();
                                txtequipment.Text = dt.Rows[0]["EQUIP"].ToString().Trim();
                                txtprob.Text = dt.Rows[0]["PROB_OBSV"].ToString().Trim();
                                txtengdupted.Text = dt.Rows[0]["eng_deputed"].ToString().Trim();
                                txtcontactmode.Text = dt.Rows[0]["cont_mode"].ToString().Trim();
                                txtdeputdt.Text = dt.Rows[0]["depute_dt"].ToString().Trim();
                                txtsrvtype.Text = dt.Rows[0]["srv_Type"].ToString().Trim();
                                txtdealername.Text = dt.Rows[0]["dealer_name"].ToString().Trim();
                                txtperson.Text = dt.Rows[0]["first_per"].ToString().Trim();
                                txtcatg.Text = dt.Rows[0]["category"].ToString().Trim();
                                txtinstruction.Text = dt.Rows[0]["eng_instruct"].ToString().Trim();
                                txtrmk.Text = dt.Rows[0]["RMK1"].ToString().Trim();
                                try { dd_list1.SelectedValue = dt.Rows[0]["PREV_MNT"].ToString().Trim(); }
                                catch { }
                                txtsiteid.Text = dt.Rows[0]["SITE_ID"].ToString().Trim();
                                txtsitename.Text = dt.Rows[0]["SITE_NAME"].ToString().Trim();
                                txtocrhr.Text = dt.Rows[0]["OCCR_TIME"].ToString().Trim();
                                try { ddresonforfail.SelectedItem.Text = dt.Rows[0]["REASON_FAIL"].ToString().Trim(); }
                                catch { }
                                #endregion
                            }
                            break;
                            fgen.EnableForm(this.Controls);
                            disablectrl();
                            set_Val();
                            Cal();
                    }
                    break;
                case "New"://old code
                    #region
                    if (col1 == "") return;
                    //HCID = Request.Cookies["rid"].Value.ToString();
                    switch (frm_formID)
                    {
                        case "F10351":
                            SQuery = "select a.*,b.aname,b.addr1,b.addr2,b.addr3,b.email as pemail from tracksys a,famst b where trim(A.acode)=trim(B.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' ";
                            break;
                        default:
                            SQuery = "select a.*,b.aname,b.addr1,b.addr2,b.addr3 from scratch a,famst b where trim(A.acode)=trim(B.acodE) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' ";
                            break;
                    }
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, co_cd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtinvdate.Text = vardate;
                        txtcustpodt.Text = vardate;
                        txtnexttgt.Text = vardate;

                        switch (frm_formID)
                        {
                            case "F10351":
                                txtvchnum.Text = fgen.next_no(frm_qstr, co_cd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                                txtvchdate.Text = vardate;
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                                txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                                txtengno.Text = dt.Rows[0]["engno"].ToString().Trim();
                                txtguarnty_status.Text = dt.Rows[0]["G_STATUS"].ToString().Trim();
                                txtDGsrno.Text = dt.Rows[0]["dgsrno"].ToString().Trim();
                                txtinvno.Text = dt.Rows[0]["invno"].ToString().Trim();
                                txtinvdate.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtemailid.Text = dt.Rows[0]["pemail"].ToString().Trim();
                                txtaddr1.Text = dt.Rows[0]["addr1"].ToString().Trim();
                                txtaddr2.Text = dt.Rows[0]["addr2"].ToString().Trim();
                                txtaddr3.Text = dt.Rows[0]["addr3"].ToString().Trim();
                                txttaddr1.Text = dt.Rows[0]["taddr1"].ToString().Trim();
                                txttaddr2.Text = dt.Rows[0]["taddr2"].ToString().Trim();
                                txttaddr3.Text = dt.Rows[0]["taddr3"].ToString().Trim();
                                txtsiteid.Text = dt.Rows[0]["siteid"].ToString();
                                txtsitename.Text = dt.Rows[0]["sitename"].ToString();
                                ViewState["app_by"] = dt.Rows[0]["app_by"].ToString().Trim();
                                if (dt.Rows[0]["app_DT"].ToString().Trim().Length > 0) ViewState["app_dt"] = dt.Rows[0]["app_DT"].ToString().Trim();
                                else ViewState["app_dt"] = DateTime.Now.ToString("dd/MM/yyyy");
                                try { ddresonforfail.SelectedItem.Text = dt.Rows[0]["col34"].ToString().Trim(); }
                                catch { }
                                break;
                            default:
                                #region Default value for action by ho and eng
                                txtacode.Text = dt.Rows[0]["acode"].ToString().Trim();
                                txtaname.Text = dt.Rows[0]["aname"].ToString().Trim();
                                txtaddr1.Text = dt.Rows[0]["addr1"].ToString().Trim();
                                txtaddr2.Text = dt.Rows[0]["addr2"].ToString().Trim();
                                txtaddr3.Text = dt.Rows[0]["addr3"].ToString().Trim();

                                txtemailid.Text = dt.Rows[0]["email"].ToString().Trim();
                                txtDGsrno.Text = dt.Rows[0]["col1"].ToString().Trim();
                                txtengno.Text = dt.Rows[0]["col2"].ToString().Trim();
                                txtcontactper.Text = dt.Rows[0]["col3"].ToString().Trim();
                                txttel.Text = dt.Rows[0]["col4"].ToString().Trim();
                                txtdesignation.Text = dt.Rows[0]["col5"].ToString().Trim();

                                txttaddr1.Text = dt.Rows[0]["col6"].ToString().Trim();
                                txttaddr2.Text = dt.Rows[0]["col7"].ToString().Trim();
                                txttaddr3.Text = dt.Rows[0]["col8"].ToString().Trim();

                                txtcallatt.Text = dt.Rows[0]["col9"].ToString().Trim();
                                txtcustpo.Text = dt.Rows[0]["col10"].ToString().Trim();
                                txtcustpodt.Text = dt.Rows[0]["col11"].ToString().Trim();

                                txtequipment.Text = dt.Rows[0]["col12"].ToString().Trim();
                                txtprob.Text = dt.Rows[0]["col13"].ToString().Trim();
                                txtrmk.Text = dt.Rows[0]["remarks"].ToString().Trim();

                                txtengdupted.Text = dt.Rows[0]["col14"].ToString().Trim();
                                txtcontactmode.Text = dt.Rows[0]["col15"].ToString().Trim();
                                txtdeputdt.Text = Convert.ToDateTime(dt.Rows[0]["docdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                txtcontactmode.Text = dt.Rows[0]["col16"].ToString().Trim();
                                txtsrvtype.Text = dt.Rows[0]["col17"].ToString().Trim();
                                txtdealername.Text = dt.Rows[0]["col18"].ToString().Trim();
                                txtperson.Text = dt.Rows[0]["col19"].ToString().Trim();
                                txtinstruction.Text = dt.Rows[0]["col20"].ToString().Trim();

                                txtinvno.Text = dt.Rows[0]["REFNUM"].ToString().Trim();
                                txtinvdate.Text = Convert.ToDateTime(dt.Rows[0]["REFdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                                txttimein.Text = dt.Rows[0]["col21"].ToString().Trim();
                                txttimeout.Text = dt.Rows[0]["col22"].ToString().Trim();
                                if (dt.Rows[0]["col23"].ToString().Trim().Length > 0) txtnexttgt.Text = dt.Rows[0]["col23"].ToString().Trim();
                                else txtnexttgt.Text = vardate;
                                txttimein.Text = dt.Rows[0]["col24"].ToString().Trim();
                                txtengrmk.Text = dt.Rows[0]["col25"].ToString().Trim();
                                txtcorraction.Text = dt.Rows[0]["col26"].ToString().Trim();
                                txtprevntiveaction.Text = dt.Rows[0]["col27"].ToString().Trim();
                                txtreasonforpend.Text = dt.Rows[0]["col28"].ToString().Trim();
                                txtspares.Text = dt.Rows[0]["col29"].ToString().Trim();
                                txtdocno.Text = dt.Rows[0]["col30"].ToString().Trim();
                                txtdocdt.Text = dt.Rows[0]["col31"].ToString().Trim();
                                txtrmkactionbyeng.Text = dt.Rows[0]["naration"].ToString().Trim();

                                txtsrvcost.Text = dt.Rows[0]["num1"].ToString().Trim();
                                txtsparecost.Text = dt.Rows[0]["num2"].ToString().Trim();
                                txtmisccost.Text = dt.Rows[0]["num3"].ToString().Trim();
                                txttravconv.Text = dt.Rows[0]["num4"].ToString().Trim();
                                //   txthmrcost.Text = dt.Rows[0]["num5"].ToString().Trim();
                                txtmetwhom.Text = dt.Rows[0]["met_whom"].ToString().Trim();
                                txtsiteid.Text = dt.Rows[0]["col32"].ToString().Trim();
                                txtsitename.Text = dt.Rows[0]["col33"].ToString().Trim();
                                txtocrhr.Text = dt.Rows[0]["col34"].ToString().Trim();

                                ViewState["app_by"] = dt.Rows[0]["app_by"].ToString().Trim();
                                if (dt.Rows[0]["app_DT"].ToString().Trim().Length > 0) ViewState["app_dt"] = dt.Rows[0]["app_DT"].ToString().Trim();
                                else ViewState["app_dt"] = DateTime.Now.ToString("dd/MM/yyyy");
                                try { ddresonforfail.SelectedItem.Text = dt.Rows[0]["col34"].ToString().Trim(); }
                                catch { }

                                try { dd_list1.SelectedValue = dt.Rows[0]["col45"].ToString().Trim(); }
                                catch { }
                                //if (dt.Rows[0]["col46"].ToString().Trim().Length > 0) 
                                //    rdcategory.SelectedValue = dt.Rows[0]["col46"].ToString().Trim();
                                //else rdcategory.SelectedValue = "1";
                                if (dt.Rows[0]["col47"].ToString().Trim().Length > 0) rdworkdone.SelectedValue = dt.Rows[0]["col47"].ToString().Trim();
                                else rdworkdone.SelectedValue = "1";
                                #endregion
                                if (frm_formID == "F10352")
                                {
                                    txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                                    txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                    txtinvno.Text = dt.Rows[0]["invno"].ToString().Trim();
                                    txtinvdate.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                    txtdocno.Text = fgen.next_no(frm_qstr, co_cd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                                    txtdocdt.Text = vardate;
                                    txtengdupted.Focus();
                                }
                                if (frm_formID == "F10353")
                                {
                                    txtvchnum.Text = dt.Rows[0]["invno"].ToString().Trim();
                                    txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                    txtinvno.Text = dt.Rows[0]["REFNUM"].ToString().Trim();
                                    txtinvdate.Text = Convert.ToDateTime(dt.Rows[0]["REFdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                    txtdocno.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                                    txtdocdt.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                                    txtactionvchnum.Text = fgen.next_no(frm_qstr, co_cd, "select max(vchnum) as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                                    txtactionvchdt.Text = vardate;
                                    txttimein.Focus();
                                }
                                break;
                        }
                        Cal();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                    }
                    #endregion
                    break;
                case "SRVT":
                    txtsrvtype.Text = col2;
                    break;
                case "CMOD":
                    txtcontactmode.Text = col2;
                    break;
                case "ENG":
                    txtengdupted.Text = col2;
                    txtcontact.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    break;
                case "PERSON":
                    txtperson.Text = col2;
                    txtpersoncontct.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    break;
                case "DL":
                    txtdealername.Text = col2;
                    break;
                case "CUST":
                    if (col1 == "") return;
                    SQuery = "SELECT DISTINCT TRIM(ACODE) AS ACODE,TRIM(ANAME) AS CUSTOMER,TRIM(ADDR1) AS ADDR1,TRIM(ADDR2) AS ADDR2,TRIM(ADDR3) AS ADDR3 FROM FAMST WHERE TRIM(aCODE)='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtacode.Text = dt.Rows[0]["ACODE"].ToString().Trim();
                        txtaname.Text = dt.Rows[0]["CUSTOMER"].ToString().Trim();
                        txtaddr1.Text = dt.Rows[0]["ADDR1"].ToString().Trim();
                        txtaddr2.Text = dt.Rows[0]["ADDR2"].ToString().Trim();
                        txtaddr3.Text = dt.Rows[0]["ADDR3"].ToString().Trim();
                    }
                    break;

                case "Print":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", col1);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", frm_vty);
                    fgen.fin_engg_reps(frm_qstr);
                    break;
            }
        }
    }
    //--------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            switch (frm_formID)
            {
                case "F10351":
                    col3 = "List of Service Req Entry";
                    SQuery = "select a.vchnum as ser_req_no,to_char(a.vchdate,'dd/mm/yyyy') as sev_req_dt,trim(a.acode) as acode,b.aname as customer,a.addr1,a.addr2,a.addr3,a.addr4,a.addr5,a.addr6,a.prev_mnt,a.occr_time,a.reason_fail,a.dgsrno as M_C_NO,a.engno as Guarantee_Warranty_term,a.cont_per as contact_person,telno ,desg as designation  from " + frm_tabname + " a,famst b where a.branchcd='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' and a.vchdate " + PrdRange + " and trim(a.acode)=trim(b.acode)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " For the Period of " + fromdt + " To " + todt, frm_qstr);
                    break;
                case "F10352":
                    col3 = "List of Action by HO";
                    SQuery = "select a.vchnum as doc_no,to_char(a.vchdate,'dd/mm/yyyy') as doc_dt,a.refnum as ser_req_no,to_char(a.refdate,'dd/mm/yyyy') as ser_req_dt,trim(a.acode) as acode,b.aname as customer,a.addr1,a.addr2,a.addr3,a.prev_mnt,a.occr_time,a.reason_fail,a.dgsrno as M_C_NO,a.engno as Guarantee_Warranty_term,a.cont_per as contact_person,telno ,desg as designation,a.call_dtl as call_Detail,a.prob_obsv as prob_observed,a.rmk1,a.eng_deputed as eng_deputed,a.cont_mode as contact_mode,to_char(a.depute_dt,'dd/mm/yyyy') as deputation_Dt,a.srv_Type,a.dealer_name ,a.first_per as first_person,a.category,a.eng_instruct as instruction_to_eng from WB_SERVICE a,famst b where a.branchcd='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' and a.vchdate " + PrdRange + " and trim(a.acode)=trim(b.acode)";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " For the Period of " + fromdt + " To " + todt, frm_qstr);
                    break;

                case "F10353":
                    col3 = "List of Action by Service Engineer";
                    fgen.drillQuery(0, "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,'-' AS GSTR,a.vchnum as CSR_NO,to_char(a.vchdate,'dd/mm/yyyy') as CSR_Dt,a.DGSRNO as m_C_no,(case when nvl(a.chk_by,'-')='-' then 'OPEN' ELSE 'CLOSE' END) AS status  from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='CC' and a.vchdate " + DateRange + " order by a.vchnum desc", frm_qstr);
                    fgen.drillQuery(1, "select a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.refnum||to_char(a.refdate,'dd/mm/yyyy') as gstr, a.refnum as csr_no,to_char(a.refdate,'dd/mm/yyyy') as csr_date,a.dgsrno as m_c_no,(case when nvl(a.chk_by,'-')='-' then 'OPEN' ELSE 'CLOSE' END) AS status,a.acode,a.cust_po_no,to_char(a.cust_po_dt,'dd/mm/yyyy') as cust_po_dt,a.docno as ho_entry_no,to_char(a.docdate,'dd/mm/yyyy') as ho_entry_dt ,A.ENG_DEPUTED, A.ENG_INSTRUCT,A.DEALER_NAME,A.FIRST_PER,A.vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,A.TIME_IN,A.TIME_OUT,a.nxt_trgt,(case when a.work_done='0' then 'YES' else 'NO' end) as work_done,a.eng_rmk,a.corr_Act as corrective_Action,a.preven_act as preventive_Action,a.reason_pend as reason_for_pendency,a.spares_rqd,nvl(a.serv_cost,0) as service_cost,nvl(a.spare_COST,0) as spares_cost,nvl(a.misc_cost,0) as misc_cost,nvl(a.travel_conv,0) as travel_cost,a.rmk2 as remark from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='CE' and a.vchdate " + DateRange + " order by a.vchnum desc", frm_qstr);
                    fgen.Fn_DrillReport("List of " + lblheader.Text.Trim() + " For the Period of " + fromdt + " To " + todt, frm_qstr);
                    break;
            }
            hffield.Value = "-";
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
                    if (frm_formID != "F10353")
                    {
                        if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                        {
                            Checked_ok = "N";
                            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                        }
                    }
                }
            }
            //last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            //if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            //{
            //    Checked_ok = "N";
            //    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            //}
            //-----------------------------
            i = 0;
            hffield.Value = "";

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
                            switch (frm_formID)
                            {
                                case "F10351":
                                    frm_vnum = txtvchnum.Text.Trim();
                                    break;
                                case "F10352":
                                    frm_vnum = txtdocno.Text.Trim();
                                    break;
                                case "F10353":
                                    frm_vnum = txtactionvchnum.Text.Trim();
                                    break;
                            }
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
                        if (frm_formID == "F10352")
                        {
                            if (frm_cocd == "SEL")
                            {
                                fgen.send_sms(frm_qstr, frm_cocd, txtcontact.Text, "Dear " + txtengdupted.Text + ",Req No " + txtvchnum.Text + " M/c No. " + txtDGsrno.Text + " assigned to you", frm_uname);
                                fgen.send_sms(frm_qstr, frm_cocd, txtpersoncontct.Text, "Dear " + txtperson.Text + ",Req No " + txtvchnum.Text + " M/c No. " + txtDGsrno.Text + " assigned to you", frm_uname);
                            }
                        }

                        if (edmode.Value == "Y")
                        {
                            // fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            fgen.msg("-", "AMSG", lblheader.Text + " " + frm_vnum + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //  fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + frm_vnum + " Saved Successfully");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }

                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtvchdate.Text.Trim() + "'");
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
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

    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = frm_mbr;
        oporow["TYPE"] = frm_vty;
        switch (frm_formID)
        {
            case "F10351":
                #region
                oporow["vchnum"] = txtvchnum.Text.Trim();
                oporow["vchdate"] = txtvchdate.Text.Trim();
                oporow["acode"] = txtacode.Text.Trim();
                oporow["icode"] = txticode.Text.Trim();
                oporow["EMAIL_ID"] = txtemailid.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["DGSRNO"] = txtDGsrno.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ENGNO"] = txtengno.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["G_STATUS"] = txtguarnty_status.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["CONT_PER"] = txtcontactper.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["TELNO"] = txttel.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["DESG"] = txtdesignation.Text.Trim().ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ADDR1"] = txtaddr1.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ADDR2"] = txtaddr2.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ADDR3"] = txtaddr3.Text.Trim();
                //txtaddr1
                oporow["ADDR4"] = txttaddr1.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ADDR5"] = txttaddr2.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ADDR6"] = txttaddr3.Text.Trim();
                oporow["CALL_DTL"] = txtcallatt.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");

                oporow["CUST_PO_NO"] = txtcustpo.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                if (txtcustpodt.Text.Length < 2)
                {
                    oporow["CUST_PO_DT"] = vardate;
                }
                else
                {
                    oporow["CUST_PO_DT"] = txtcustpodt.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                }
                oporow["EQUIP"] = txtequipment.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                if (txtprob.Text.Trim().Length > 300)
                {
                    oporow["PROB_OBSV"] = txtprob.Text.ToUpper().ToString().Substring(0, 299);
                }
                else
                {
                    oporow["PROB_OBSV"] = txtprob.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                }
                oporow["invno"] = txtinvno.Text.Trim();
                if (txtinvdate.Text.Length < 2)
                {
                    oporow["invdate"] = vardate;
                }
                else
                {
                    oporow["invdate"] = txtinvdate.Text.Trim();
                }
                if (txtrmk.Text.Trim().Length > 300)
                {
                    oporow["RMK1"] = txtrmk.Text.Trim().ToUpper().Substring(0, 299);
                }
                else
                {
                    oporow["RMK1"] = txtrmk.Text.Trim().ToUpper();
                }

                //oporow["PREV_MNT"] = dd_list1.SelectedValue.ToString().Trim();
                oporow["PREV_MNT"] = dd_list1.SelectedItem.Text.ToString().Trim();
                oporow["SITE_ID"] = txtsiteid.Text.Trim().ToUpper();
                oporow["SITE_NAME"] = txtsitename.Text.Trim().ToUpper();
                oporow["OCCR_TIME"] = txtocrhr.Text.Trim().ToUpper();
                oporow["REASON_FAIL"] = ddresonforfail.SelectedItem.Text.ToString().Trim();
                #endregion
                break;
            case "F10352":
                #region
                oporow["vchnum"] = txtdocno.Text.Trim();
                oporow["vchdate"] = txtdocdt.Text.Trim();
                oporow["acode"] = txtacode.Text.Trim();
                oporow["icode"] = txticode.Text.Trim();
                oporow["EMAIL_ID"] = txtemailid.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["DGSRNO"] = txtDGsrno.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ENGNO"] = txtengno.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["G_STATUS"] = txtguarnty_status.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["CONT_PER"] = txtcontactper.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["TELNO"] = txttel.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["DESG"] = txtdesignation.Text.Trim().ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ADDR1"] = txtaddr1.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ADDR2"] = txtaddr2.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ADDR3"] = txtaddr3.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["addr4"] = txttaddr1.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["addr5"] = txttaddr2.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["addr6"] = txttaddr3.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["CALL_DTL"] = txtcallatt.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["CUST_PO_NO"] = txtcustpo.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["CUST_PO_DT"] = txtcustpodt.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["EQUIP"] = txtequipment.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["PROB_OBSV"] = txtprob.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["invno"] = txtinvno.Text.Trim();//FIRST FORM'S VCHNUM
                oporow["invdate"] = txtinvdate.Text.Trim();//FIRST FORM'S VCHDATE
                oporow["REFNUM"] = txtvchnum.Text.Trim();//1st from vchnum
                oporow["REFdate"] = txtvchdate.Text.Trim();//1st from vchdate
                oporow["RMK1"] = txtrmk.Text.Trim().ToUpper();
                //******************************
                oporow["HODATE"] = vardate;
                oporow["ENG_DEPUTED"] = txtengdupted.Text.Trim().ToUpper();
                oporow["CONT_MODE"] = txtcontactmode.Text.Trim().ToUpper();
                oporow["DEPUTE_DT"] = txtdeputdt.Text.Trim();
                oporow["CONT_MODE"] = txtcontactmode.Text.ToUpper();
                oporow["SRV_TYPE"] = txtsrvtype.Text.ToUpper();
                oporow["DEALER_NAME"] = txtdealername.Text.ToUpper();
                oporow["FIRST_PER"] = txtperson.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                if (txtinstruction.Text.Trim().Length > 300)
                {
                    oporow["ENG_INSTRUCT"] = txtinstruction.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").Substring(0, 299);
                }
                else
                {
                    oporow["ENG_INSTRUCT"] = txtinstruction.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                }
                oporow["PREV_MNT"] = dd_list1.SelectedItem.Text.ToString().Trim();
                // oporow["CATEGORY"] = rdcategory.SelectedValue.ToString().Trim();
                oporow["CATEGORY"] = txtcatg.Text.Trim().ToUpper();
                oporow["SITE_ID"] = txtsiteid.Text.Trim().ToUpper();
                oporow["SITE_NAME"] = txtsitename.Text.Trim().ToUpper();
                oporow["OCCR_TIME"] = txtocrhr.Text.Trim().ToUpper();
                oporow["REASON_FAIL"] = ddresonforfail.SelectedItem.Text.ToString().Trim();
                oporow["ENG_DEP_MOB"] = txtcontact.Text.ToString().Trim();
                oporow["F_PER_MOB"] = txtpersoncontct.Text.ToString().Trim();
                //oporow["app_by"] = ViewState["app_by"].ToString().Trim();
                //oporow["app_dt"] = ViewState["app_dt"].ToString().Trim();
                //if (frm_cocd == "SEL")
                //    fgen.send_sms(frm_cocd, txtcontact.Text, "Dear " + txtengdupted.Text + ", Welcome to " + frm_cocd + ", Task assigned", frm_uname);
                #endregion
                break;
            case "F10353":
                #region
                oporow["vchnum"] = txtactionvchnum.Text.Trim();
                oporow["vchdate"] = txtactionvchdt.Text.Trim();
                oporow["acode"] = txtacode.Text.Trim();
                oporow["icode"] = txticode.Text.Trim();
                oporow["EMAIL_ID"] = txtemailid.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["DGSRNO"] = txtDGsrno.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["engno"] = txtengno.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["G_STATUS"] = txtguarnty_status.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["CONT_PER"] = txtcontactper.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["TELNO"] = txttel.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["DESG"] = txtdesignation.Text.Trim().ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ADDR1"] = txtaddr1.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ADDR2"] = txtaddr2.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ADDR3"] = txtaddr3.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["addr4"] = txttaddr1.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["addr5"] = txttaddr2.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["addr6"] = txttaddr3.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["CALL_DTL"] = txtcallatt.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["CUST_PO_NO"] = txtcustpo.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["CUST_PO_dt"] = txtcustpodt.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["EQUIP"] = txtequipment.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["PROB_OBSV"] = txtprob.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["invno"] = txtinvno.Text.Trim();
                oporow["invdate"] = txtinvdate.Text.Trim();
                oporow["RMK1"] = txtrmk.Text.Trim().ToUpper();
                //******************************
                oporow["HODATE"] = vardate;
                oporow["ENG_DEPUTED"] = txtengdupted.Text.Trim().ToUpper();
                oporow["CONT_MODE"] = txtcontactmode.Text.Trim().ToUpper();
                oporow["DEPUTE_DT"] = txtdeputdt.Text.Trim();
                oporow["CONT_MODE"] = txtcontactmode.Text.ToUpper();
                oporow["SRV_TYPE"] = txtsrvtype.Text.ToUpper();
                oporow["DEALER_NAME"] = txtdealername.Text.ToUpper();
                oporow["FIRST_PER"] = txtperson.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["ENG_INSTRUCT"] = txtinstruction.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["DOCNO"] = txtdocno.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");//2nd from vchnum
                oporow["DOCDATE"] = txtdocdt.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");//2ndfrom vchdate
                oporow["REFNUM"] = txtvchnum.Text.Trim(); //1st from vchnum
                oporow["REFdate"] = txtvchdate.Text.Trim(); //1st from vchdate
                //******************************
                oporow["TIME_IN"] = txttimein.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["TIME_OUT"] = txttimeout.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["NXT_TRGT"] = txtnexttgt.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["TIME_IN"] = txttimein.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                // oporow["ENG_RMK"] = txtengrmk.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                if (txtengrmk.Text.Trim().Length > 300)
                {
                    oporow["ENG_RMK"] = txtengrmk.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").Substring(0, 299);
                }
                else
                {
                    oporow["ENG_RMK"] = txtengrmk.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                }
                //oporow["CORR_ACT"] = txtcorraction.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                if (txtcorraction.Text.Trim().Length > 300)
                {
                    oporow["CORR_ACT"] = txtcorraction.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").Substring(0, 299);
                }
                else
                {
                    oporow["CORR_ACT"] = txtcorraction.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                }
                //oporow["PREVEN_ACT"] = txtprevntiveaction.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                if (txtprevntiveaction.Text.Trim().Length > 300)
                {
                    oporow["PREVEN_ACT"] = txtprevntiveaction.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").Substring(0, 299);
                }
                else
                {
                    oporow["PREVEN_ACT"] = txtprevntiveaction.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                }
                // oporow["REASON_PEND"] = txtreasonforpend.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                if (txtreasonforpend.Text.Trim().Length > 300)
                {
                    oporow["REASON_PEND"] = txtreasonforpend.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").Substring(0, 299);
                }
                else
                {
                    oporow["REASON_PEND"] = txtreasonforpend.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                }
                oporow["SPARES_RQD"] = txtspares.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");

                if (txtrmkactionbyeng.Text.Trim().Length > 300)
                {
                    oporow["RMK2"] = txtrmkactionbyeng.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ").Substring(0, 299);
                }
                else
                {
                    oporow["RMK2"] = txtrmkactionbyeng.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                }
                oporow["SERV_COST"] = fgen.make_double(txtsrvcost.Text.Trim());
                oporow["SPARE_COST"] = fgen.make_double(txtsparecost.Text.Trim());
                oporow["MISC_COST"] = fgen.make_double(txtmisccost.Text.Trim());
                oporow["TRAVEL_CONV"] = fgen.make_double(txttravconv.Text.Trim());
                oporow["tot_cost"] = fgen.make_double(txttotcost.Text.Trim());
                oporow["met_whom"] = txtmetwhom.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                // oporow["HMR"] = fgen.make_double(txthmrcost.Text.Trim());
                oporow["PREV_MNT"] = dd_list1.SelectedItem.Text.ToString().Trim();
                oporow["CATEGORY"] = txtcatg.Text.Trim().ToUpper();
                oporow["WORK_DONE"] = rdworkdone.SelectedValue.ToString().Trim();
                oporow["SITE_ID"] = txtsiteid.Text.Trim().ToUpper();
                oporow["SITE_NAME"] = txtsitename.Text.Trim().ToUpper();
                oporow["OCCR_TIME"] = txtocrhr.Text.Trim().ToUpper();
                oporow["REASON_FAIL"] = ddresonforfail.SelectedItem.Text.ToString().Trim();
                if (txtAttch.Text.Length > 1)
                {
                    oporow["filepath"] = lblUpload.Text.Trim();
                    oporow["filename"] = txtAttch.Text.Trim();
                }
                // mq2 = "";   //for drill down report need update in table on cc type
                //mq2 = "update wb_Service set refnum='"+txtvchnum.Text+"',refdate=to_date('" + Convert.ToDateTime(txtvchdate.Text).ToString("dd/MM/yyyy").Trim() + "','dd/MM/yyyy') where branchcd='" + frm_mbr + "' and type='CC' ANd vchnum='" + txtvchnum.Text + "' and to_char(vchdate,'dd/mm/yyyy')='" + txtvchdate.Text + "'";
                //fgen.execute_cmd(frm_qstr, co_cd, mq2);

                if (chkclose.Checked == true)
                {
                    oporow["chk_by"] = uname;
                    oporow["chk_dt"] = vardate;
                    mq2 = "";
                    mq2 = "update wb_Service set chk_by='" + frm_uname + "',chk_dt=to_date('" + Convert.ToDateTime(vardate).ToString("dd/MM/yyyy").Trim() + "','dd/MM/yyyy') where branchcd='" + frm_mbr + "' and type='CC' ANd vchnum='" + txtvchnum.Text + "' and to_char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(txtvchdate.Text).ToString("dd/MM/yyyy") + "'";
                    fgen.execute_cmd(frm_qstr, co_cd, mq2);
                    mq2 = "";
                    mq2 = "update wb_Service set chk_by='" + frm_uname + "',chk_dt=to_date('" + Convert.ToDateTime(vardate).ToString("dd/MM/yyyy").Trim() + "','dd/MM/yyyy') where branchcd='" + frm_mbr + "' and type='CH' ANd vchnum='" + txtvchnum.Text + "' and to_char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(txtvchdate.Text).ToString("dd/MM/yyyy") + "'";
                    fgen.execute_cmd(frm_qstr, co_cd, mq2);
                }
                else
                {
                    oporow["chk_by"] = "-";
                    oporow["chk_dt"] = vardate;
                    mq2 = "";
                    mq2 = "update wb_Service set chk_by='-',chk_dt=to_date('" + Convert.ToDateTime(vardate).ToString("dd/MM/yyyy").Trim() + "','dd/MM/yyyy') where branchcd='" + frm_mbr + "' and type='CC' ANd vchnum='" + txtvchnum.Text + "' and to_char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(txtvchdate.Text).ToString("dd/MM/yyyy") + "'";
                    fgen.execute_cmd(frm_qstr, co_cd, mq2);

                    mq2 = "";
                    mq2 = "update wb_Service set chk_by='-',chk_dt=to_date('" + Convert.ToDateTime(vardate).ToString("dd/MM/yyyy").Trim() + "','dd/MM/yyyy') where branchcd='" + frm_mbr + "' and type='CH' ANd vchnum='" + txtvchnum.Text + "' and to_char(vchdate,'dd/mm/yyyy')='" + Convert.ToDateTime(txtvchdate.Text).ToString("dd/MM/yyyy") + "'";
                    fgen.execute_cmd(frm_qstr, co_cd, mq2);
                }
                //oporow["app_by"] = ViewState["app_by"].ToString().Trim();
                //oporow["app_dt"] = ViewState["app_dt"].ToString().Trim();
                #endregion
                break;
        }
        if (edmode.Value == "Y")
        {
            oporow["ent_by"] = ViewState["entby"].ToString();
            oporow["ent_dt"] = ViewState["entdt"].ToString();
            oporow["edt_by"] = frm_uname;
            oporow["edt_dt"] = vardate;
        }
        else
        {
            oporow["ent_by"] = frm_uname;
            oporow["ent_dt"] = vardate;
            oporow["edt_by"] = "-";
            oporow["eDt_dt"] = vardate;
        }
        oDS.Tables[0].Rows.Add(oporow);
    }
    //------------------------------------------------------------------------------------
    protected void btnengdeputed_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "ENG";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Engineer To Depute", frm_qstr);
    }
    protected void btncontactmode_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "CMOD";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Contact Mode", frm_qstr);
    }
    protected void btnsrvtype_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "SRVT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Service Type", frm_qstr);
    }
    protected void btndealername_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "DL";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Dealer", frm_qstr);
    }
    protected void btnparty_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "CUST";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer", frm_qstr);
    }
    protected void btncatg_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "TS";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer", frm_qstr);
    }
    protected void txtDGsrno_TextChanged(object sender, EventArgs e)
    {
        dt = new DataTable(); dt2 = new DataTable();
        //0535/04/2019 mc no for testing
        SQuery1 = "SELECT DISTINCT trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr, trim(a.acode) as acode,a.pordno as cust_pono,to_char(a.porddt,'dd/mm/yyyy') as cust_podt,a.weight as guaranty FROM SOMAS a WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%'  "; //AND ORDDT>TO_DATE('31/03/2017','DD/MM/YYYY')
        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);//SALE ORDER DT
        SQuery = "SELECT TRIM(a.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.PONUM)||TO_CHAR(A.PODATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ACODE) AS ACODE,TRIM(B.ANAME) AS ANAME,B.ADDR1,B.ADDR2,B.ADDR3,A.ICODE,A.CCENT AS MC_NO,A.PONUM,A.PODATE  FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '4%'  AND CCENT='" + txtDGsrno.Text.Trim() + "'";        //AND A.VCHDAte " + DateRange + "
        SQuery = "SELECT TRIM(a.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.PONUM)||TO_CHAR(A.PODATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ACODE) AS ACODE,TRIM(B.ANAME) AS ANAME,B.ADDR1,B.ADDR2,B.ADDR3,A.ICODE,trim(c.iname) as product,A.CCENT AS MC_NO,A.PONUM,A.PODATE FROM IVOUCHER A,FAMST B,item c WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and trim(a.icode)=trim(c.icode) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '4%'  AND CCENT='" + txtDGsrno.Text.Trim() + "'";
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //inv dt
        if (dt.Rows.Count > 0)
        {
            mq0 = ""; mq1 = ""; int j = 0;
            txtinvno.Text = dt.Rows[0]["vchnum"].ToString().Trim();
            txtinvdate.Text = dt.Rows[0]["VCHDATE"].ToString().Trim();
            txtacode.Text = dt.Rows[0]["ACODE"].ToString().Trim();
            txtaname.Text = dt.Rows[0]["ANAME"].ToString().Trim();
            txticode.Text = dt.Rows[0]["icode"].ToString().Trim();
            txtiname.Text = dt.Rows[0]["product"].ToString().Trim();
            txtaddr1.Text = dt.Rows[0]["ADDR1"].ToString().Trim();
            txtaddr2.Text = dt.Rows[0]["ADDR2"].ToString().Trim();
            txtaddr3.Text = dt.Rows[0]["ADDR3"].ToString().Trim();
            txtengno.Text = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[0]["fstr"].ToString().Trim() + "'", "guaranty");
            txtcustpo.Text = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[0]["fstr"].ToString().Trim() + "'", "cust_pono");
            txtcustpodt.Text = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[0]["fstr"].ToString().Trim() + "'", "cust_podt");
            mq0 = txtinvdate.Text.Substring(6, 4);
            mq1 = txtvchdate.Text.Substring(6, 4);
            j = Convert.ToInt32(mq1) - Convert.ToInt32(mq0);

            if (j > Convert.ToInt32(txtengno.Text))
            {
                txtguarnty_status.Text = "Beyond Guarantee/Warranty";
            }
            else
            {
                txtguarnty_status.Text = "Under Guarantee/Warranty";
            }
        }
    }
    protected void btnfirstprsn_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "PERSON";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Engineer To Depute", frm_qstr);
    }
    protected void rdworkdone_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdworkdone.SelectedValue == "0")
        {
            txtclosdt.ReadOnly = false;
            txtnexttgt.ReadOnly = true;
            txtnexttgt.Text = "";
        }
        else
        {
            txtclosdt.ReadOnly = true;
            txtnexttgt.ReadOnly = false;
            txtclosdt.Text = "";
        }
    }
    protected void btnAtt_Click(object sender, EventArgs e)
    {
        string filepath = @"c:\TEJ_erp\UPLOAD\";      //Server.MapPath("~/tej-base/UPLOAD/");
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            txtAttch.Text = Attch.FileName;
            filepath = filepath + "_" + txtvchnum.Text.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
            Attch.PostedFile.SaveAs(filepath);
            filepath = Server.MapPath("~/tej-base/UPLOAD/") + "_" + txtvchnum.Text.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
            Attch.PostedFile.SaveAs(filepath);
            lblUpload.Text = filepath;
            btnView1.Visible = true;
            btnDwnld1.Visible = true;
        }
        else
        {
            lblUpload.Text = "";
        }
    }

    protected void btnView1_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
        // ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "c:/TEJ_erp/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
    }

    protected void btnDwnld1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));

            Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
            Session["FileName"] = txtAttch.Text;
            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");
        }
        catch { }
    }
    void Cal()
    {
        double t1 = 0; double t2 = 0; double t3 = 0; double t4 = 0; double t5 = 0;
        t1 = fgen.make_double(txtsrvcost.Text.Trim());
        t2 = fgen.make_double(txtsparecost.Text.Trim());
        t3 = fgen.make_double(txtmisccost.Text.Trim());
        t4 = fgen.make_double(txttravconv.Text.Trim());
        t5 = (t1 + t2 + t3 + t4);
        txttotcost.Text = Convert.ToString(Math.Round(t5, 3)).Replace("Infinity", "0").Replace("NaN", "0");
    }

}
//============NEED TO ADD SOME FIELD IN Table

//alter table FINSEL.WB_SERVICE MODIFY ENG_INSTRUCT VARCHAR2(300);
//alter table FINSEL.WB_SERVICE MODIFY RMK2 VARCHAR2(300);
//alter table FINSEL.WB_SERVICE MODIFY RMK1 VARCHAR2(300);
//alter table FINSEL.WB_SERVICE MODIFY PROB_OBSV VARCHAR2(300);
//alter table FINSEL.WB_SERVICE MODIFY ENG_RMK VARCHAR2(300);
//alter table FINSEL.WB_SERVICE MODIFY CORR_ACT VARCHAR2(300);
//alter table FINSEL.WB_SERVICE MODIFY PREVEN_ACT VARCHAR2(300);
//alter table FINSEL.WB_SERVICE MODIFY REASON_PEND VARCHAR2(300);

//============NEW FIELDS NEED TO OPEN IN TABLE

//alter table FINSEL.WB_SERVICE add G_STATUS VARCHAR2(40);   
//alter table FINSEL.WB_SERVICE add ENG_DEP_MOB VARCHAR2(20);
//alter table FINSEL.WB_SERVICE add F_PER_MOB VARCHAR2(20);
//alter table FINSEL.WB_SERVICE ADD MET_WHOM VARCHAR2(30);
//alter table FINSEL.WB_SERVICE ADD clos_dt VARCHAR2(10);
//alter table FINSEL.WB_SERVICE ADD FILEPATH VARCHAR2(100);
//alter table FINSEL.WB_SERVICE ADD FILENAME VARCHAR2(60);
//alter table FINSEL.WB_SERVICE add tot_cost number(15,3);
