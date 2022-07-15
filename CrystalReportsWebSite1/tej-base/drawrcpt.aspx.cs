using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using System.Text;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;



public partial class drawrcpt : System.Web.UI.Page
{
    //OracleConnection con = new OracleConnection();
    //DataTable dt, pTable;
    //DataSet oDS = new DataSet();
    //OracleCommand cmd;
    //OracleDataAdapter da;
    //IFormatProvider AustralianDateFormat;
    //DataRow oporow;
    //TimeSpan objTimeSpan;
    //StringBuilder sb;
    //OracleCommandBuilder cb;
    //DateTime vchdate, presentdate, entdate, idate, rdate, rsdate, DAT1, DAT2, invdate;

    //string vchnum, query, btnmode, daterange, col1, ulevel, vardate, mlvl;
    //string tco_cd, mbr, co_cd, uname, cdt1, cdt2, scode, sname, seek, entby, edt, edmode, headername, xmlfile;
    //string uright, can_add, can_edit, can_del, acessuser;
    //string fName, fpath, filename, mypath, compnay_code, extension;
    //string mflag, mailmsg, sendtoemail, subject, xmltag, mailpath, mailport, branchname, col3, col4;
    //int i, srno;

    DataTable dt = new DataTable();
    DataTable dt1 = new DataTable();
    DataTable dt2 = new DataTable();
    DataTable dt3 = new DataTable();
    DataTable dtCol = new DataTable();
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataRow dr1;
    DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2;
    string Checked_ok;
    string save_it;
    string vchnum, query, btnmode, daterange, SQuery1, SQuery2, col1, col2, ulevel, vardate, mlvl, mq1, DRID, typePopup = "N";
    string tco_cd, mbr, custom_filing_no, co_cd, uname, cdt1, cdt2, scode, sname, seek, entby, edt, headername, xmlfile;
    string uright, can_add, can_edit, can_del, acessuser, filePath, SQuery;
    string fName, fpath, filename, mypath, compnay_code, extension;
    string sendtoemail, subject, xmltag, mailpath, mailport, branchname, col3, col4, mailmsg, mflag;
    int i, z = 0, srno;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query, btnval;
    string frm_mbr, frm_vty, frm_vnum, frm_url, fromdt, todt, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    int ssl, port;
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
                fgen.DisableForm(this.Controls);
                enablectrl();
            }
        }
        setColHeadings();
        set_Val();

    }

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    if (Request.UrlReferrer == null) Response.Redirect("login.aspx");
    //    else
    //    {
    //        tco_cd = Request.Cookies["CK_COFILEVARS"].Value.ToString();
    //        co_cd = tco_cd.Substring(0, 4).Trim();
    //        cdt1 = tco_cd.Substring(9, 10);
    //        cdt2 = tco_cd.Substring(19, 10);
    //        mbr = Request.Cookies["CK_mbr"].Value.ToString();
    //        uname = Request.Cookies["UNAME"].Value.ToString();
    //        mlvl = Request.Cookies["UL_ACODE"].Value.ToString();
    //        ulevel = mlvl.Substring(0, 1);
    //        AustralianDateFormat = System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat;
    //        daterange = "between to_Date('" + cdt1 + "','dd/MM/yyyy') and to_Date('" + cdt2 + "','dd/MM/yyyy')";
    //        vardate = fgen.CurrDate(co_cd);

    //        con = new OracleConnection(fgen.GetCon(co_cd));

    //        if (Convert.ToDouble(ulevel) > 1)
    //            acessuser = uname;
    //        else acessuser = "";



    //        if (!IsPostBack)
    //        {
    //            clearcontrol();

    //            fgen.DisableForm(this.Page);

    //            if (Request.Cookies["U_RIGHT"] != null)
    //            {

    //                uright = Request.Cookies["U_RIGHT"].Value.ToString();
    //                can_add = uright.Substring(0, 1);
    //                can_edit = uright.Substring(1, 1);
    //                can_del = uright.Substring(2, 1);
    //            }

    //            if (can_add == "N") btnnew.Visible = false;
    //            else btnnew.Visible = true;
    //            if (can_edit == "N") btnedit.Visible = false;
    //            else btnedit.Visible = true;
    //            if (can_del == "N") btndelete.Visible = false;
    //            else btndelete.Visible = true;


    //            btnenable();
    //            btnsave.Disabled = true;
    //        }
    //    }
    //}

    //public void btnenable()
    //{
    //    btnnew.Disabled = false;
    //    btnedit.Disabled = false;
    //    btndelete.Disabled = false;
    //    btnsave.Disabled = false;
    //    btnlist.Disabled = false;
    //    btnexit.Disabled = false;
    //    btnexit.InnerHtml = "Exit";
    //}
    //public void btndisable()
    //{
    //    btnnew.Disabled = true;
    //    btnedit.Disabled = true;
    //    btndelete.Disabled = true;
    //    btnsave.Disabled = true;
    //    btnlist.Disabled = true;
    //    btnexit.Disabled = false;
    //    btnexit.InnerHtml = "Cancel";
    //}
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = true; btncancel.Visible = false;
        btnlist.Disabled = false; //  btnprint.Disabled = false;
        btnedn.Enabled = false;
        //btndno.Enabled = false; btndtype.Enabled = false; btnview.Enabled = false; btnDesignType.Enabled = false;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlist.Disabled = true;//  btnprint.Disabled = true;
        btnedn.Enabled = true;
      //  btndno.Enabled = true; btndtype.Enabled = true; btnview.Enabled = true; btnDesignType.Enabled = true;
    }
    //--------------------------------------
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
        //    dtCol = new DataTable();
        //    dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        //    if (dtCol == null || dtCol.Rows.Count <= 0)
        //    {
        //        getColHeading();
        //    }
        //    dtCol = new DataTable();
        //    dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];

        //    if (dtCol == null) return;
        //    if (sg1.Rows.Count <= 0) return;
        //    for (int sR = 0; sR < sg1.Columns.Count; sR++)
        //    {
        //        string orig_name;
        //        double tb_Colm;
        //        tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
        //        orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();

        //        for (int K = 0; K < sg1.Rows.Count; K++)
        //        {
        //            #region hide hidden columns
        //            for (int i = 0; i < 10; i++)
        //            {
        //                sg1.Columns[i].HeaderStyle.CssClass = "hidden";
        //                sg1.Rows[K].Cells[i].CssClass = "hidden";
        //            }
        //            #endregion
        //            if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));
        //            ((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("readonly", "readonly");
        //            ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("readonly", "readonly");
        //            ((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("readonly", "readonly");
        //        }
        //        orig_name = orig_name.ToUpper();
        //        //if (sg1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_NAME"))
        //        if (sR == tb_Colm)
        //        {
        //            // hidding column
        //            if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
        //            {
        //                sg1.Columns[sR].Visible = false;
        //            }
        //            // Setting Heading Name
        //            sg1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
        //            // Setting Col Width
        //            string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
        //            if (fgen.make_double(mcol_width) > 0)
        //            {
        //                sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
        //                sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
        //            }
        //        }
        //    }

        //    //txtlbl8.Attributes.Add("readonly", "readonly");
        //    //txtlbl9.Attributes.Add("readonly", "readonly");



        //    //// to hide and show to tab panel
        //    //tab5.Visible = false;
        //    //tab4.Visible = false;
        //    //tab3.Visible = false;
        //    //tab2.Visible = false;

        //    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        //    //switch (Prg_Id)
        //    //{
        //    //    case "M09024":
        //    //    case "M10003":
        //    //    case "M11003":
        //    //    case "M10012":
        //    //    case "M11012":
        //    //    case "M12008":
        //    //        tab3.Visible = false;
        //    //        tab4.Visible = false;
        //    //        break;
        //    //}
        //    //if (Prg_Id == "M12008")
        //    //{
        //    //    tab5.Visible = true;
        //    //    txtlbl8.Attributes.Remove("readonly");
        //    //    txtlbl9.Attributes.Remove("readonly");
        //    //}
        //    fgen.SetHeadingCtrl(this.Controls, dtCol);

    }

    //------------------------------------------------------------------------------------
    public void clearctrl()
    {
        hffield.Value = "";
        edmode.Value = "";
    }
    //-------------------------------------------------------------------------------------
    public void set_Val()
    {
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        lblheader.Text = "Drawing Receipt Entry";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "WB_DRAWREC";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "DR");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        //typePopup = "N";     
    }
    //===============================================
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
            case "ED":
                SQuery = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY') as fstr,vchnum as edN,to_char(vchdate,'dd/mm/yyyy') as EDN_date,dtype as drawing_type,dno as drawing_no,issue_to,issue_by,issue_date,to_date(rdate,'dd/mm/yyyy') as return_target_Date,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + "  where branchcd ='" + frm_mbr + "' and type='DI' /*and  vchdate " + DateRange + "*/ and vchnum||to_char(vchdate,'DDMMYYYY') not in (select invno||to_char(invdate,'DDMMYYYY') from " + frm_tabname + " where type='DR' AND branchcd ='" + frm_mbr + "' /*AND vchdate " + DateRange + "*/ ) order by VDD desc,vchnum desc";
                break;
            case "SURE":
                SQuery = "Select 'YES' as col1,'Yes,Please' as Text,'Record Will be Deleted' as Action from dual union all Select 'NO' as col1,'No,Do Not' as Text,'Record Will Not be Deleted' as Action from dual";
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[13].Text.Trim().Length > 1)
                    {
                        if (col1.Length > 0) col1 = col1 + "," + "'" + gr.Cells[13].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                    }
                }
                if (col1.Length > 0)
                {
                    col1 = " and trim(type1) not in (" + col1 + ")";
                }
                else
                {
                    col1 = "";
                }
                SQuery = "select type1 as fstr,name as proc_name,type1 as code from type where id='K' " + col1 + " order by code";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case"DT":
                mq1 = "dtype as drawing_type,";
                SQuery = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY') as fstr," + mq1 + " vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,branchcd as br_code,rno as revision_no,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from DRAWREC  where  type='DE' /*and  vchdate " + daterange + "*/ order by VDD desc,vchnum desc ";
                break;

            case"DN":
                mq1 = "dno as drawing_no,";
                SQuery = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY') as fstr," + mq1 + " vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,branchcd as br_code,rno as revision_no,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from DRAWREC  where  type='DE' /*and  vchdate " + daterange + "*/ order by VDD desc,vchnum desc ";
                break;
                
            case "IO":
            case "IB":
                SQuery = "select type1 as fstr,name as USER_name,type1 as Code from typemst where id='SM'  order by type1";
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD" || btnval == "Print_E")                  
                SQuery = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,invno as issue_no,dtype as drawing_type,dno as drawing_no, to_char(rcpt_date,'dd/mm/yyyy') as issue_date,to_char(rdate,'dd/mm/yyyy') as return_target_date,to_char(issue_date,'dd/mm/yyyy') as actual_return_date,rno as revision_no,issue_by ,tno as delay_if_any,remarks as delay_reason,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from "+frm_tabname+"  where branchcd ='" + frm_mbr + "' and type='DR' /*and  vchdate " + DateRange + "*/ order by VDD desc,vchnum desc ";
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
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F10133":
                SQuery = "SELECT '10' AS FSTR,'Process Mapping' as NAME,'10' AS CODE FROM dual";
                break;
        }
    }

    //public void disp_data()
    //{
    //    query = "";
    //    btnmode = hfbtnmode.Value;
    //    col1 = "";
    //    switch (btnmode)
    //    {
    //        case "ED":
    //            query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as edN,to_char(vchdate,'dd/mm/yyyy') as EDN_date,dtype as drawing_type,dno as drawing_no,issue_to,issue_by,issue_date,to_date(rdate,'dd/mm/yyyy') as return_target_Date,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from DRAWREC  where branchcd ='" + mbr + "' and type='DI' /*and  vchdate " + daterange + "*/ and vchnum||to_char(vchdate,'DDMMYYYY') not in (select invno||to_char(invdate,'DDMMYYYY') from DRAWREC where type='DR' AND branchcd ='" + mbr + "' /*AND vchdate " + daterange + "*/ ) order by VDD desc,vchnum desc ";
    //            break;
    //        case "SURE":
    //            query = "Select 'YES' as col1,'Yes,Please' as Text,'Record Will be Deleted' as Action from dual union all Select 'NO' as col1,'No,Do Not' as Text,'Record Will Not be Deleted' as Action from dual";
    //            break;
    //        default:
    //            if (btnmode == "Edit" || btnmode == "Del")
    //                query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,invno as issue_no,dtype as drawing_type,dno as drawing_no, to_char(rcpt_date,'dd/mm/yyyy') as issue_date,to_char(rdate,'dd/mm/yyyy') as return_target_date,to_char(issue_date,'dd/mm/yyyy') as actual_return_date,rno as revision_no,issue_by ,tno as delay_if_any,remarks as delay_reason,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from DRAWREC  where branchcd ='" + mbr + "' and type='DR' /*and  vchdate " + daterange + "*/ order by VDD desc,vchnum desc ";
    //            else if (btnmode == "DT" || btnmode == "DN")
    //            {
    //                if (btnmode == "DT")
    //                    col1 = "dtype as drawing_type,";
    //                else if (btnmode == "DN")
    //                    col1 = "dno as drawing_no,";
    //                query = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr," + col1 + " vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,branchcd as br_code,rno as revision_no,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from DRAWREC  where  type='DE' /*and  vchdate " + daterange + "*/ order by VDD desc,vchnum desc ";
    //            }
    //            else if (btnmode == "IO" || btnmode == "IB")
    //                query = "select type1 as fstr,name as USER_name,type1 as Code from typemst where id='SM'  order by type1";
    //            break;
    //    }
    //    if (query == "") { }
    //    else
    //    {

    //        Response.Cookies["popupid"].Value = "Tejaxo";
    //        Response.Cookies["seeksql"].Value = query;
    //    }
    //}
    //public void cleardata()
    //{

    //    fgen.ResetForm(this.Page);
    //    fgen.RemoveTextBoxBorder(this.Page);
    //    cleargrd_ds();
    //    ViewState["DRAW"] = null;
    //}
    //public void clearcontrol()
    //{

    //    fgen.ResetForm(this.Page);
    //    fgen.RemoveTextBoxBorder(this.Page);
    //    alermsg.Style.Add("display", "none");
    //    cleargrd_ds();
    //    ViewState["DRAW"] = null;
    //}



    //protected void btnnew_Click(object sender, EventArgs e)
    //{
    //    clearcontrol();


    //    fgen.EnableForm(this.Page);

    //    hfedmode.Value = "N";
    //    query = "select max(vchnum) as vch from DRAWREC where branchcd = '" + mbr + "' and type='DR' /*and vchdate " + daterange + "*/ ";
    //    txtdocno.Text = fgen.Gen_No(co_cd, query, "vch", 6);
    //    hffielddt.Value = vardate;
    //    txtdate.Value = hffielddt.Value;
    //    btndisable();
    //    btnsave.Disabled = false;

    //    txtidate.Value = vardate;
    //    txtpre.Text = uname;
    //}

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
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }

    void newCase(string vty)
    {
        #region
        vty = "DR";
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
        txtdocno.Text = frm_vnum;
        txtdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        if (edmode.Value == "Y")
        {
            txtedit.Text = frm_uname;
        }
        txtpre.Text = frm_uname;
        disablectrl();
        fgen.EnableForm(this.Controls);
        sg1_dt = new DataTable();
        create_tab();
        sg1_dr = null;
        sg1_add_blankrows();
        ViewState["sg1"] = sg1_dt;
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        //// Popup asking for Copy from Older Data
        ////fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        ////hffield.Value = "NEW_E";
        #endregion
    }

    //public void sseekfunc()
    //{
    //    disp_data();
    //    OpenPopup("SSEEK");
    //}
    //public void AlertMsg(string msgtype, string msgname)
    //{
    //    switch (msgtype)
    //    {
    //        case "AMSG":
    //            alermsg.InnerHtml = msgname;
    //            alermsg.Style.Add("display", "block");
    //            break;

    //    }
    //}
    //public void OpenPopup(string popuptype)
    //{
    //    headername = "";
    //    btnmode = hfbtnmode.Value;
    //    switch (popuptype)
    //    {
    //        case "SSEEK":
    //            switch (btnmode)
    //            {
    //                case "ED":
    //                    headername = "EDN Selection";
    //                    break;
    //                case "SURE":
    //                    headername = "Confirmation for Deletion";
    //                    break;
    //                case "Edit":
    //                    headername = "Edit Drawing Receipt  Master";
    //                    break;
    //                case "Del":
    //                    headername = "Delete Drawing Receipt Master";
    //                    break;
    //                default:
    //                    break;
    //            }
    //            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('" + headername + "','SSeek.aspx','75%','82%',false);});", true);
    //            break;
    //    }
    //}

    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            hffield.Value = "Edit";
            typePopup = "N";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Edit", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }

    //protected void btnedit_Click(object sender, EventArgs e)
    //{
    //    clearcontrol();
    //    hfbtnmode.Value = "Edit";
    //    hfedmode.Value = "Y";
    //    sseekfunc();
    //}

    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        // Hidden Field
        sg1_dt.Columns.Add(new DataColumn("sg1_h1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h10", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(Int32)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f10", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f11", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f13", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f14", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f16", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f20", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t10", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t11", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t13", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t14", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t16", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t20", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t21", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t22", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t23", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t24", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t25", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t26", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t27", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t28", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t29", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t30", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t31", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t32", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t33", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t34", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t35", typeof(string)));

    }
    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        if (sg1_dt != null)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_h1"] = "-";
            sg1_dr["sg1_h2"] = "-";
            sg1_dr["sg1_h3"] = "-";
            sg1_dr["sg1_h4"] = "-";
            sg1_dr["sg1_h5"] = "-";
            sg1_dr["sg1_h6"] = "-";
            sg1_dr["sg1_h7"] = "-";
            sg1_dr["sg1_h8"] = "-";
            sg1_dr["sg1_h9"] = "-";
            sg1_dr["sg1_h10"] = "-";
            sg1_dr["sg1_SrNo"] = sg1_dt.Rows.Count + 1;
            sg1_dr["sg1_f1"] = "-";
            sg1_dr["sg1_f2"] = "-";
            sg1_dr["sg1_f3"] = "-";
            sg1_dr["sg1_f4"] = "-";
            sg1_dr["sg1_f5"] = "-";
            sg1_dr["sg1_f6"] = "-";
            sg1_dr["sg1_f7"] = "-";
            sg1_dr["sg1_f8"] = "-";
            sg1_dr["sg1_f9"] = "-";
            sg1_dr["sg1_f10"] = "-";
            sg1_dr["sg1_f11"] = "-";
            sg1_dr["sg1_f12"] = "-";
            sg1_dr["sg1_f13"] = "-";
            sg1_dr["sg1_f14"] = "-";
            sg1_dr["sg1_f15"] = "-";
            sg1_dr["sg1_f16"] = "-";
            sg1_dr["sg1_f17"] = "-";
            sg1_dr["sg1_f18"] = "-";
            sg1_dr["sg1_f19"] = "-";
            sg1_dr["sg1_f20"] = "-";
            sg1_dr["sg1_t1"] = "-";
            sg1_dr["sg1_t2"] = "-";
            sg1_dr["sg1_t3"] = "-";
            sg1_dr["sg1_t4"] = "-";
            sg1_dr["sg1_t5"] = "-";
            sg1_dr["sg1_t6"] = "-";
            sg1_dr["sg1_t7"] = "-";
            sg1_dr["sg1_t8"] = "-";
            sg1_dr["sg1_t9"] = "-";
            sg1_dr["sg1_t10"] = "-";
            sg1_dr["sg1_t11"] = "-";
            sg1_dr["sg1_t12"] = "-";
            sg1_dr["sg1_t13"] = "-";
            sg1_dr["sg1_t14"] = "-";
            sg1_dr["sg1_t15"] = "-";
            sg1_dr["sg1_t16"] = "-";
            sg1_dr["sg1_t17"] = "-";
            sg1_dr["sg1_t18"] = "-";
            sg1_dr["sg1_t19"] = "-";
            sg1_dr["sg1_t20"] = "-";
            sg1_dr["sg1_t21"] = "-";
            sg1_dr["sg1_t22"] = "-";
            sg1_dr["sg1_t23"] = "-";
            sg1_dr["sg1_t24"] = "-";
            sg1_dr["sg1_t25"] = "-";
            sg1_dr["sg1_t26"] = "-";
            sg1_dr["sg1_t27"] = "-";
            sg1_dr["sg1_t28"] = "-";
            sg1_dr["sg1_t29"] = "-";
            sg1_dr["sg1_t30"] = "-";
            sg1_dr["sg1_t31"] = "-";
            sg1_dr["sg1_t32"] = "-";
            sg1_dr["sg1_t33"] = "-";
            sg1_dr["sg1_t34"] = "-";
            sg1_dr["sg1_t35"] = "-";
            sg1_dt.Rows.Add(sg1_dr);
        }
    }

    //-------------------------------------------

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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
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
                case "New":
                    newCase(col1);
                    break;

                case "COPY_OLD":
                    #region Copy from Old Temp
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,b.text from " + frm_tabname + " a left outer join FIN_MSYS b on trim(a.frm_name)=trim(b.id) where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {

                        //txtlbl4.Text = dt.Rows[i]["frm_name"].ToString().Trim();
                        //txtlbl4a.Text = dt.Rows[i]["text"].ToString().Trim();
                        //txtlbl2.Text = dt.Rows[i]["frm_header"].ToString().Trim();
                        //txtlbl7.Text = dt.Rows[0]["ent_id"].ToString().Trim();
                        //txtlbl7a.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                        // create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = "-";
                            sg1_dr["sg1_h2"] = "-";
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";
                            sg1_dr["sg1_f1"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["text"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_t1"] = dt.Rows[i]["OBJ_NAME"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["OBJ_CAPTION"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["OBJ_WIDTH"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["OBJ_VISIBLE"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["col_no"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["obj_maxlen"].ToString().Trim();
                            sg1_dr["sg1_t7"] = "";
                            if (frm_tabname.ToUpper() == "SYS_CONFIG")
                            {
                                sg1_dr["sg1_t7"] = dt.Rows[i]["OBJ_READONLY"].ToString().Trim();
                            }
                            sg1_dr["sg1_t8"] = "";
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1_add_blankrows();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                    }
                    #endregion
                    break;

                case "DT":
                    txtdtype.Text = col2;
                    hf1.Value = col1;               
                    break;
                case "ED":
                    dt = new DataTable();
                    SQuery = "SELECT * from " + frm_tabname + "  where branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + col1 + "'";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtedn.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                       // txtedate.Value = dt.Rows[0]["vchdate"].ToString().Trim().Substring(0, 10);
                        txtdtype.Text = dt.Rows[0]["dtype"].ToString().Trim();
                        txtdno.Text = dt.Rows[0]["dno"].ToString().Trim();
                        txtrno.Text = dt.Rows[0]["rno"].ToString().Trim();
                        txtrdate.Text = dt.Rows[0]["rdate"].ToString().Trim().Substring(0, 10);
                        txtiby.Text = dt.Rows[0]["issue_by"].ToString().Trim();
                        txtisdate.Text = dt.Rows[0]["issue_date"].ToString().Trim().Substring(0, 10);

                        //DAT1 = DateTime.ParseExact(txtidate.Value, "dd/MM/yyyy", null);
                        //DAT2 = DateTime.ParseExact(txtrdate.Value, "dd/MM/yyyy", null);
                        //objTimeSpan = DAT1 - DAT2;
                        //txtdelay.Text = Convert.ToString(System.Math.Round(Convert.ToDouble(objTimeSpan.Days), 2));
                        hf1.Value = dt.Rows[0]["finvno"].ToString().Trim();
                    }

                    cleargrd_ds();
                    dt = new DataTable();
                    SQuery = "select * from filetable where branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + hf1.Value + "'";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        sg1.DataSource = dt;
                        sg1.DataBind();
                        sg1.Visible = true;
                        ViewState["DRAW"] = dt;
                    }

                    break;
                case "DN":
                    //if (mbr == "03" || mbr == "04") txtdno.Text = sname;
                    //else
                    //{
                    //    if (sname.Length > 4)
                    //        txtdno.Text = fgen.substr_numeric(sname, 0);
                    //}
                    txtdno.Text = col2;// fgen.substr_numeric(sname, 0);
                    break;


                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;

                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Edit", frm_qstr);
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
                    fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", col1);
                    fgen.fin_engg_reps(frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.* from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + col1 + "'";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtdocno.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                      // hffielddt.Value = dt.Rows[0]["vchdate"].ToString().Trim().Substring(0, 10);
                        txtdate.Text = dt.Rows[0]["vchdate"].ToString().Trim().Substring(0, 10);
                        txtedn.Text = dt.Rows[0]["invno"].ToString().Trim();
                        txtedate.Text = dt.Rows[0]["invdate"].ToString().Trim().Substring(0, 10);
                        txtdtype.Text = dt.Rows[0]["dtype"].ToString().Trim();
                        txtdno.Text = dt.Rows[0]["dno"].ToString().Trim();
                        txtidate.Text = dt.Rows[0]["issue_date"].ToString().Trim().Substring(0, 10);
                        txtrdate.Text = dt.Rows[0]["rdate"].ToString().Trim().Substring(0, 10);
                        txtrno.Text = dt.Rows[0]["rno"].ToString().Trim();
                        hf1.Value = dt.Rows[0]["finvno"].ToString().Trim();
                        txtisdate.Text = dt.Rows[0]["rcpt_date"].ToString().Trim().Substring(0, 10);
                        txtiby.Text = dt.Rows[0]["issue_by"].ToString().Trim();
                        txtdelay.Text = dt.Rows[0]["tno"].ToString().Trim();
                        txtreason.Text = dt.Rows[0]["remarks"].ToString().Trim();
                        entby = dt.Rows[0]["ent_by"].ToString().Trim();
                        edt = dt.Rows[0]["ent_dt"].ToString().Trim();
                        txtpre.Text = entby;
                        txtedit.Text = dt.Rows[0]["edt_by"].ToString().Trim();                       
                        DateTime DAT1, DAT2;
                        DAT1 = Convert.ToDateTime(txtidate.Text);
                        DAT2 = Convert.ToDateTime(txtrdate.Text);
                        TimeSpan ts = DAT1 - DAT2;                      
                        txtdelay.Text = Convert.ToString(ts.Days);

                        ViewState["ENTBY"] = entby;
                        ViewState["ENTDT"] = edt;
                        if (txtAttch.Text.Length > 1)
                        {
                            lblUpload.Text = dt.Rows[0]["filepath"].ToString().Trim();
                            mq1 = dt.Rows[0]["filename"].ToString().Trim().Split('~')[1];
                            txtAttch.Text = mq1;
                        }
                        else if (dt.Rows[0]["filepath"].ToString().Trim().Length > 1)
                        {
                            lblUpload.Text = dt.Rows[0]["filepath"].ToString().Trim();
                            txtAttch.Text = dt.Rows[0]["filename"].ToString().Trim();
                        }
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        txtedit.Text = frm_uname;
                        //=================
                        dt = new DataTable();
                        SQuery = "select * from filetable where branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + hf1.Value + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            sg1.DataSource = dt;
                            sg1.DataBind();
                            sg1.Visible = true;
                            ViewState["sg1_dt"] = dt;// ViewState["DRAW"]
                        }                                                                  
                        fgen.EnableForm(this.Controls);
                        dt.Dispose();
                        disablectrl();
                        btnView1.Enabled = true;
                        btnDown.Enabled = true;
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
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
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    if (ViewState["sg1"] != null)
                    {
                        dt = new DataTable();
                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = Convert.ToInt32(sg1.Rows[i].Cells[12].Text.ToString());
                            sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["sg1_h2"].ToString();
                            sg1_dr["sg1_h3"] = dt.Rows[i]["sg1_h3"].ToString();
                            sg1_dr["sg1_h4"] = dt.Rows[i]["sg1_h4"].ToString();
                            sg1_dr["sg1_h5"] = dt.Rows[i]["sg1_h5"].ToString();
                            sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                            sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                            sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                            sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                            sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();
                            sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                            sg1_dr["sg1_f6"] = dt.Rows[i]["sg1_f6"].ToString();
                            sg1_dr["sg1_f7"] = dt.Rows[i]["sg1_f7"].ToString();
                            //sg1_dr["sg1_f8"] = dt.Rows[i]["sg1_f8"].ToString();
                            //sg1_dr["sg1_f9"] = dt.Rows[i]["sg1_f9"].ToString();
                            sg1_dr["sg1_f8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f8")).Text.Trim();
                            sg1_dr["sg1_f9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f9")).Text.Trim();
                            sg1_dr["sg1_f10"] = dt.Rows[i]["sg1_f10"].ToString();
                            sg1_dr["sg1_f11"] = dt.Rows[i]["sg1_f11"].ToString();
                            sg1_dr["sg1_f12"] = dt.Rows[i]["sg1_f12"].ToString();
                            sg1_dr["sg1_f13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f13")).Text.Trim();
                            sg1_dr["sg1_f14"] = dt.Rows[i]["sg1_f14"].ToString();
                            sg1_dr["sg1_f15"] = dt.Rows[i]["sg1_f15"].ToString();
                            sg1_dr["sg1_f16"] = dt.Rows[i]["sg1_f16"].ToString();
                            sg1_dr["sg1_f17"] = dt.Rows[i]["sg1_f17"].ToString();
                            sg1_dr["sg1_f18"] = dt.Rows[i]["sg1_f18"].ToString();
                            sg1_dr["sg1_f19"] = dt.Rows[i]["sg1_f19"].ToString();
                            sg1_dr["sg1_f20"] = dt.Rows[i]["sg1_f20"].ToString();
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                            sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                            sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                            sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();
                            sg1_dr["sg1_t19"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                            sg1_dr["sg1_t20"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim();
                            sg1_dr["sg1_t21"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();
                            sg1_dr["sg1_t22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim();
                            sg1_dr["sg1_t23"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim();
                            sg1_dr["sg1_t24"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text.Trim();
                            sg1_dr["sg1_t25"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text.Trim();
                            sg1_dr["sg1_t26"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text.Trim();
                            sg1_dr["sg1_t27"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t27")).Text.Trim();
                            sg1_dr["sg1_t28"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t28")).Text.Trim();
                            sg1_dr["sg1_t29"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t29")).Text.Trim();
                            sg1_dr["sg1_t30"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t30")).Text.Trim();
                            sg1_dr["sg1_t31"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t31")).Text.Trim();
                            sg1_dr["sg1_t32"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t32")).Text.Trim();
                            sg1_dr["sg1_t33"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t33")).Text.Trim();
                            sg1_dr["sg1_t34"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t34")).Text.Trim();
                            sg1_dr["sg1_t35"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t35")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_f13")).Focus();
                    #endregion
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD_E":
                    #region
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    // dt2 = new DataTable();
                    custom_filing_no = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9").ToString().Trim().Replace("&amp", "");
                    SQuery = "select trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') as fstr,b.vchnum,to_char(b.vchdate,'dd/mm/yyyy') as vchdate,b.acode,f.aname,b.destcount as country,b.cscode,to_char(b.remvdate,'dd/mm/yyyy') as remvdate,b.bill_tot,b.insp_amt as foreign_amt,b.amt_exc as igst_claimed,b.curren,b.chlnum,to_char(b.chldate,'dd/MM/yyyy') as chldate,c.aname as cons from famst f,salep b left join csmst c on trim(b.cscode)=trim(c.acode) where trim(b.acode)=trim(f.acode) and trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') ='" + custom_filing_no + "'  order by vchnum";
                    SQuery1 = "select trim(a.vchnum)||trim(a.vchdate) as fstr,sum(a.iqtyout) as iqtyout,max(a.hscode) as hscode,a.export_under,max(name) as name,a.acpt_ud as curr_rate from(select iqtyout,null as hscode,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,(case when nvl(trim(store_no),'-')='19' then 'LUT' when nvl(trim(store_no),'-')='18' then 'ADV. LIC AND IGST' else 'DUTY FREE' end) as export_under,null as name,acpt_ud from ivoucherp where trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')= '" + custom_filing_no + "' union all select 0 as iqtyout,i.hscode,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,(case when nvl(trim(a.store_no),'-')='19' then 'LUT' when nvl(trim(store_no),'-')='18' then 'ADV. LIC AND IGST' else 'DUTY FREE' end) as export_under,t.name as name,a.acpt_ud from ivoucherp a,item i,typegrp t where trim(a.icode)=trim(i.icode) and trim(i.hscode)=trim(t.acref) and t.id='T1' and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')= '" + custom_filing_no + "' and a.morder='1')a group by trim(a.vchnum),trim(a.vchdate),a.export_under,a.acpt_ud";
                    SQuery2 = "select trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,exprmk as country from hundip where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') in (" + custom_filing_no + ")  order by vchnum";
                    SQuery2 = "select trim(a.chlnum)||to_char(a.chldate,'dd/mm/yyyy') as fstr,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.chlnum,to_char(a.chldate,'dd/MM/yyyy') as chldate from sale a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') ='" + col1 + "'  order by vchnum";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery2);
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in GridView Value
                        if (dt3.Rows.Count > 0)
                        {
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = fgen.seek_iname_dt(dt3, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "vchnum");
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = fgen.seek_iname_dt(dt3, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "vchdate");
                        }
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[d]["vchnum"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[d]["vchdate"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[d]["acode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["aname"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[22].Text = dt.Rows[d]["country"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[23].Text = dt.Rows[d]["remvdate"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[19].Text = dt.Rows[d]["cscode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[20].Text = dt.Rows[d]["cons"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_f8")).Text = "0";
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_f9")).Text = dt.Rows[d]["foreign_amt"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[26].Text = dt.Rows[d]["curren"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[28].Text = dt.Rows[d]["igst_claimed"].ToString().Trim();
                        if (dt2.Rows.Count > 0)
                        {
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[21].Text = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "iqtyout");
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[29].Text = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "hscode");
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[30].Text = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "export_under");
                            sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[32].Text = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "name");
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t30")).Text = fgen.seek_iname_dt(dt2, "fstr='" + dt.Rows[d]["vchnum"].ToString().Trim() + dt.Rows[d]["vchdate"].ToString().Trim() + "'", "curr_rate"); ;
                        }
                    }
                    hf2.Value = "";
                    setColHeadings();
                    #endregion
                    break;

                case "SG1_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        i = 0;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = (i + 1);
                            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.Trim();
                            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.Trim();
                            sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.Trim();
                            sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.Trim();
                            sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.Trim();
                            sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.Trim();
                            sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.Trim();
                            sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.Trim();
                            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.Trim();
                            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.Trim();

                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.Trim();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[17].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[18].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[21].Text.Trim();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[22].Text.Trim();
                            sg1_dr["sg1_f7"] = sg1.Rows[i].Cells[23].Text.Trim();
                            //sg1_dr["sg1_f8"] = sg1.Rows[i].Cells[22].Text.Trim();
                            //sg1_dr["sg1_f9"] = sg1.Rows[i].Cells[23].Text.Trim();
                            sg1_dr["sg1_f8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f8")).Text.Trim();
                            sg1_dr["sg1_f9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f9")).Text.Trim();
                            sg1_dr["sg1_f10"] = sg1.Rows[i].Cells[28].Text.Trim();
                            sg1_dr["sg1_f11"] = sg1.Rows[i].Cells[29].Text.Trim();
                            sg1_dr["sg1_f12"] = sg1.Rows[i].Cells[30].Text.Trim();
                            //sg1_dr["sg1_f13"] = sg1.Rows[i].Cells[28].Text.Trim();
                            sg1_dr["sg1_f13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f13")).Text.Trim();
                            sg1_dr["sg1_f14"] = sg1.Rows[i].Cells[32].Text.Trim();
                            sg1_dr["sg1_f15"] = sg1.Rows[i].Cells[33].Text.Trim();
                            sg1_dr["sg1_f16"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f17"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f18"] = sg1.Rows[i].Cells[26].Text.Trim();
                            sg1_dr["sg1_f19"] = sg1.Rows[i].Cells[19].Text.Trim();
                            sg1_dr["sg1_f20"] = sg1.Rows[i].Cells[20].Text.Trim();

                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                            sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                            sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                            sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();
                            sg1_dr["sg1_t19"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                            sg1_dr["sg1_t20"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim();
                            sg1_dr["sg1_t21"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();
                            sg1_dr["sg1_t22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim();
                            sg1_dr["sg1_t23"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim();
                            sg1_dr["sg1_t24"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text.Trim();
                            sg1_dr["sg1_t25"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text.Trim();
                            sg1_dr["sg1_t26"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text.Trim();
                            sg1_dr["sg1_t27"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t27")).Text.Trim();
                            sg1_dr["sg1_t28"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t28")).Text.Trim();
                            sg1_dr["sg1_t29"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t29")).Text.Trim();
                            sg1_dr["sg1_t30"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t30")).Text.Trim();
                            sg1_dr["sg1_t31"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t31")).Text.Trim();
                            sg1_dr["sg1_t32"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t32")).Text.Trim();
                            sg1_dr["sg1_t33"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t33")).Text.Trim();
                            sg1_dr["sg1_t34"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t34")).Text.Trim();
                            sg1_dr["sg1_t35"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t35")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        for (i = 0; i < sg1.Rows.Count; i++)
                        {
                            sg1.Rows[i].Cells[12].Text = (i + 1).ToString();
                        }
                    }
                    #endregion
                    setColHeadings();
                    break;
            }
        }
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg1r = 0; sg1r < sg1.Rows.Count; sg1r++)
            {
                for (int j = 0; j < sg1.Columns.Count; j++)
                {
                    sg1.Rows[sg1r].Cells[j].ToolTip = sg1.Rows[sg1r].Cells[j].Text;
                    if (sg1.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
                    {
                        sg1.Rows[sg1r].Cells[j].Text = sg1.Rows[sg1r].Cells[j].Text.Substring(0, 35);
                    }
                }
            }
            e.Row.Cells[1].Width = 30;
            e.Row.Cells[1].Width = 30;
            e.Row.Cells[3].Width = 30;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);
        if (txtdocno.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG1_RMV":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Export Invoice From The List");
                }
                break;

            case "SG1_ROW_ADD":
                if (index < sg1.Rows.Count - 1)
                {
                    // ON + BUTTON DATE RANGE HAVE TO BE ASKED THAT'S WHY CASE IS CHANGED
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    // hffield.Value = "SG1_ROW_ADD_E";
                    hffield.Value = "TACODE";
                    hf2.Value = "SG1_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    // make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Export Invoice", frm_qstr);                  
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                }
                else
                {
                    // ON + BUTTON DATE RANGE HAVE TO BE ASKED THAT'S WHY CASE IS CHANGED
                    //hffield.Value = "SG1_ROW_ADD";
                    hffield.Value = "TACODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    //make_qry_4_popup();
                    //fgen.Fn_open_mseek("Select Export Invoice", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btncancel_ServerClick(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();
        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        ViewState["sg1"] = null;
        setColHeadings();
        lblUpload.Text = "";
    }

    //protected void btnhideF_Click(object sender, EventArgs e)
    //{
    //    scode = ""; sname = ""; seek = "";

    //    if (Request.Cookies["Column1"].Value != null)
    //    {
    //        scode = Request.Cookies["Column1"].Value.ToString().Trim();
    //        scode = scode.Replace("&AMP;", "&").Trim();
    //    }
    //    if (Request.Cookies["Column2"].Value != null)
    //    {
    //        sname = Request.Cookies["Column2"].Value.ToString().Trim();
    //        sname = sname.Replace("&AMP;", "&").Trim();
    //    }
    //    if (Request.Cookies["Column3"].Value != null)
    //    {
    //        seek = Request.Cookies["Column3"].Value.ToString().Trim();
    //        seek = seek.Replace("&AMP;", "&").Trim();
    //    }

    //    con.Open();

    //    btnmode = hfbtnmode.Value;

    //    switch (btnmode)
    //    {

    //        case "ED":

    //            dt = new DataTable();
    //            da = new OracleDataAdapter("SELECT * from DRAWREC  where branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY')='"+scode+"' ", con);
    //            da.Fill(dt);
    //            if (dt.Rows.Count > 0)
    //            {
    //                txtedn.Text = dt.Rows[0]["vchnum"].ToString().Trim();
    //                txtedate.Value = dt.Rows[0]["vchdate"].ToString().Trim().Substring(0, 10);
    //                txtdtype.Text = dt.Rows[0]["dtype"].ToString().Trim();
    //                txtdno.Text = dt.Rows[0]["dno"].ToString().Trim();
    //                txtrno.Text = dt.Rows[0]["rno"].ToString().Trim();
    //                txtrdate.Value = dt.Rows[0]["rdate"].ToString().Trim().Substring(0, 10);
    //                txtiby.Text = dt.Rows[0]["issue_by"].ToString().Trim();
    //                txtisdate.Value = dt.Rows[0]["issue_date"].ToString().Trim().Substring(0, 10);

    //                DAT1 = DateTime.ParseExact(txtidate.Value, "dd/MM/yyyy", null);
    //                DAT2 = DateTime.ParseExact(txtrdate.Value, "dd/MM/yyyy", null);
    //                objTimeSpan = DAT1 - DAT2;
    //                txtdelay.Text = Convert.ToString(System.Math.Round(Convert.ToDouble(objTimeSpan.Days), 2));
    //                hf1.Value = dt.Rows[0]["finvno"].ToString().Trim();
    //            }

    //            cleargrd_ds();
    //            dt = new DataTable();
    //            da = new OracleDataAdapter("select * from filetable where branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY')='" + hf1.Value + "'", con);
    //            da.Fill(dt);
    //            if (dt.Rows.Count > 0)
    //            {
    //                grd.DataSource = dt;
    //                grd.DataBind();
    //                grd.Visible = true;
    //                ViewState["DRAW"] = dt;
    //            }

    //            break;


    //        case "Edit":
    //            dt = new DataTable();
    //            da = new OracleDataAdapter("select * from DRAWREC  where branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY')='" + scode + "'", con);
    //            da.Fill(dt);
    //            txtdocno.Text = dt.Rows[0]["vchnum"].ToString().Trim();
    //            hffielddt.Value = dt.Rows[0]["vchdate"].ToString().Trim().Substring(0, 10);
    //            txtdate.Value = hffielddt.Value;
    //            txtedn.Text = dt.Rows[0]["invno"].ToString().Trim();
    //            txtedate.Value = dt.Rows[0]["invdate"].ToString().Trim().Substring(0, 10);
    //            txtdtype.Text = dt.Rows[0]["dtype"].ToString().Trim();
    //            txtdno.Text = dt.Rows[0]["dno"].ToString().Trim();
    //            txtidate.Value = dt.Rows[0]["issue_date"].ToString().Trim().Substring(0, 10);
    //            txtrdate.Value = dt.Rows[0]["rdate"].ToString().Trim().Substring(0, 10);
    //            txtrno.Text = dt.Rows[0]["rno"].ToString().Trim();
    //            hf1.Value = dt.Rows[0]["finvno"].ToString().Trim();
    //            txtisdate.Value = dt.Rows[0]["rcpt_date"].ToString().Trim().Substring(0, 10);
    //            txtiby.Text = dt.Rows[0]["issue_by"].ToString().Trim();
    //            txtdelay.Text = dt.Rows[0]["tno"].ToString().Trim();
    //            txtreason.Text = dt.Rows[0]["remarks"].ToString().Trim();
    //            entby = dt.Rows[0]["ent_by"].ToString().Trim();
    //            edt = dt.Rows[0]["ent_dt"].ToString().Trim();
    //            txtpre.Text = entby;
    //            txtedit.Text = dt.Rows[0]["edt_by"].ToString().Trim();
    //            DAT1 = DateTime.ParseExact(txtidate.Value, "dd/MM/yyyy", null);
    //            DAT2 = DateTime.ParseExact(txtrdate.Value, "dd/MM/yyyy", null);
    //            objTimeSpan = DAT1 - DAT2;
    //            txtdelay.Text = Convert.ToString(System.Math.Round(Convert.ToDouble(objTimeSpan.Days), 2));
    //            ViewState["ENTBY"] = entby;
    //            ViewState["ENTDT"] = edt;
    //            dt = new DataTable();
    //            da = new OracleDataAdapter("select * from filetable where branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY')='" + hf1.Value + "'", con);
    //            da.Fill(dt);
    //            if (dt.Rows.Count > 0)
    //            {
    //                grd.DataSource = dt;
    //                grd.DataBind();
    //                grd.Visible = true;
    //                ViewState["DRAW"] = dt;
    //            }
    //            fgen.EnableForm(this.Page);
    //            btndisable();
    //            btnsave.Disabled = false;
    //            break;

    //        case "Del":
    //            ViewState["COL1"] = scode;
    //            ViewState["COL2"] = sname;
    //            hfbtnmode.Value = "SURE";
    //            sseekfunc();
    //            break;
    //        case "SURE":
    //            if (scode == "NO") { }
    //            else
    //            {
    //                scode = ""; sname = "";

    //                scode = (string)ViewState["COL1"];
    //                sname = (string)ViewState["COL2"];

    //                cmd = new OracleCommand("delete from DRAWREC  where branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY')='" + scode + "'", con);
    //                cmd.ExecuteNonQuery();




    //                fgen.Tracking_Detail(co_cd, scode.Substring(0, 2), scode.Substring(4, 6), scode, uname, scode.Substring(2, 2), "DRAWING RECEIPT DELETED", vardate, "");

    //                AlertMsg("AMSG", "Doc No. " + sname + " has been Deleted Successfully.");
    //                fgen.ResetForm(this.Page);

    //                ViewState["COL1"] = null;
    //                ViewState["COL2"] = null;
    //            }
    //            break;

    //        default:
    //            break;
    //    }
    //    con.Close();
    //}
    protected void btnView1_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
        // ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "c:/tej_erp/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
    }


    public void DownloadFile(string filepath)
    {
        filename = ""; mypath = "";
        filename = filepath.Remove(0, 9);
        mypath = Server.MapPath("~" + filepath);
        Response.Clear();
        Response.AppendHeader("content-disposition", "attachment; filename=" + filename);
        Response.ContentType = "application/octet-stream";
        Response.WriteFile(mypath);
        Response.Flush();
        Response.End();
    }
    protected void btnAtt_Click(object sender, EventArgs e)
    {
        string filepath = @"c:\TEJ_ERP\UPLOAD\";
        filepath = Server.MapPath("~/tej-base/UPLOAD/");
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            txtAttch.Text = Attch.FileName;
           // filepath = filepath + "_" + txtdocno.Text.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
            filepath = filepath + "_" + frm_mbr + "_" + "DR" + "_" + txtdocno.Text.Trim() + "_" + txtdate.Text.Replace(@"/", "_") + "~" + Attch.FileName;
            Attch.PostedFile.SaveAs(filepath);
            //filepath = Server.MapPath("~/tej-base/UPLOAD/") + "_" + txtdocno.Text.Trim() + frm_CDT1.Replace(@"/", "_") + "~" + Attch.FileName;
            filepath = Server.MapPath("~/tej-base/UPLOAD/") + "_" + frm_mbr + "_" + "DR" + "_" + txtdocno.Text.Trim() + "_" + txtdate.Text.Replace(@"/", "_") + "~" + Attch.FileName;
            Attch.PostedFile.SaveAs(filepath);
            lblUpload.Text = filepath;
            btnView1.Visible = true;
            btnDown.Visible = true;
        }
        else
        {
            lblUpload.Text = "";
        }
    }

    protected void btnDown_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
        //    filePath = @"c:\TEJ_ERP\" + filePath;
        //    Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
        //    Session["FileName"] = txtAttch.Text;
        //    Response.Write("<script>");
        //    Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
        //    Response.Write("</script>");
        //}
        //catch { }
        try
        {
            string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
           // Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");//old
            Session["FilePath"] = lblUpload.Text;
            Session["FileName"] = txtAttch.Text;
            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");
        }
        catch { }
    }

    

    private void cleargrd_ds()
    {
        sg1.DataSource = null;
        sg1.DataBind();
    }
    protected void LnkGtn_Click(object sender, EventArgs e)
    {
        i = 0;
        hfbtnmode.Value = "";
        hfbtnmode.Value = "PR1";
        fName = ""; fpath = ""; extension = "";
        LinkButton selectButton = (LinkButton)sender;
        GridViewRow row = (GridViewRow)selectButton.Parent.Parent;
        fpath = sg1.Rows[row.RowIndex].Cells[4].Text.Trim().ToString();
        extension = sg1.Rows[row.RowIndex].Cells[5].Text.Trim().ToString();
       // OpenMyFile(fpath, extension);
    }
    protected void LnkGtn1_Click(object sender, EventArgs e)
    {
        i = 0;
        hfbtnmode.Value = "";
        hfbtnmode.Value = "DN1";
        fName = ""; fpath = ""; extension = "";
        LinkButton selectButton = (LinkButton)sender;
        GridViewRow row = (GridViewRow)selectButton.Parent.Parent;
        fpath = sg1.Rows[row.RowIndex].Cells[4].Text.Trim().ToString();
        extension = sg1.Rows[row.RowIndex].Cells[5].Text.Trim().ToString();
     //   OpenMyFile(fpath, extension);
    }
    //public void OpenMyFile(string fpath, string extension)
    //{
    //    i = 0;
    //    i = fpath.IndexOf(@"\Uploads");
    //    fName = fpath.Substring(i, fpath.Length - i);
    //    if (hfbtnmode.Value == "PR1")
    //    {
    //        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp" || extension == ".pdf")
    //            viewpic(fName);
    //        else
    //            viewpic("XXXX");
    //    }
    //    if (hfbtnmode.Value == "DN1") DownloadFile(fName);
    //}
    public void viewpic(string imgpath)
    {
        Session["MYURL"] = imgpath;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('Attachment Preview Window','View.aspx','95%','95%');});", true);
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate) && (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header))
        {
            e.Row.Cells[2].Visible = false;
            e.Row.Cells[4].Visible = false;
            e.Row.Cells[5].Visible = false;


        }
    }


    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "SELECT distinct 'NODRILL' as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,dtype as drawing_type,dno as drawing_no,rno as revision_no,T9 AS DESIGN_TYPE,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + "  where branchcd ='" + frm_mbr + "' and type='"+frm_vty+"' and  vchdate " + PrdRange + " order by VDD desc,vchnum desc ";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " For The Period of " + fromdt + " To " + todt, frm_qstr);
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
                    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtdate.Text.ToString()))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtdate.Text.ToString() + ",Please Check !!");
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
                            frm_vnum = txtdocno.Text.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            //save_it = "N";
                            //for (i = 0; i < sg1.Rows.Count - 0; i++)
                            //{
                            save_it = "Y";
                            // }
                            if (save_it == "Y")
                            {
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtdocno.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR").Substring(2, 18) + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "delete from filetable where branchcd='" + frm_mbr + "' and type='DR' and vchnum='" + txtdocno.Text.Trim() + "' and to_DatE(to_char(vchdate,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + txtdate.Text + "','dd/mm/yyyy')";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");

                                vchnum = "";
                                SQuery = "select max(vchnum) as vch from mailbox2 where branchcd = '" + mbr + "' and type='10' /*and vchdate " + daterange + "*/";
                              //  vchnum = fgen.Gen_No(co_cd, query, "vch", 6);
                                vchnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, "mailbox2", doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtdate.Text.Trim(), frm_uname, Prg_Id);


                                dt = new DataTable();
                                SQuery = "Select trim(user) from typemst where trim(acode)='43103' and id='MI'";
                                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                                if (dt.Rows.Count > 0)
                                {
                                    string[] muser = dt.Rows[0][0].ToString().Trim().Split(',');

                                    for ( i = 0; i < muser.Length; i++)
                                    {
                                       // fgen.mailsave(mbr, "10", vchnum, hffielddt.Value, "Drawing Print No : " + txtdno.Text + " Drawing Type : " + txtdtype.Text + " Revision No : " + txtrno.Text + " Against Issue Serial No : " + txtdocno.Text + " and Issue on Date : " + txtisdate.Value + " has been received on date : " + txtrdate.Value, muser[i].ToString().Trim(), vardate, uname, txtrdate.Value, co_cd, uname);
                                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, "Drawing Print No : " + txtdno.Text + " Drawing Type : " + txtdtype.Text + " Revision No : " + txtrno.Text + " Against Issue Serial No : " + txtdocno.Text + " and Issue on Date : " + txtisdate.Text + " has been received on date : " + txtrdate.Text, frm_uname, edmode.Value);
                                    }
                                }


                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtdocno.Text + " Saved Successfully");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }

                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtdocno.Text + txtdate.Text.Trim(), frm_uname, edmode.Value);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtdate.Text.Trim() + "'");
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); setColHeadings();
                        lblUpload.Text = "";
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
        oporow["vchnum"] = frm_vnum;
        oporow["vchdate"] = Convert.ToDateTime(txtdate.Text.Trim()).ToString("dd/MM/yyyy");
        oporow["dtype"] = txtdtype.Text;
        oporow["dno"] = txtdno.Text;
        oporow["rno"] = txtrno.Text;
        oporow["issue_date"] = fgen.make_def_Date(txtidate.Text.Trim(), vardate);
        oporow["rdate"] =  fgen.make_def_Date(txtrdate.Text.Trim(), vardate);
        oporow["tno"] = txtdelay.Text;
        oporow["remarks"] = txtreason.Text;
        oporow["rcpt_date"] = fgen.make_def_Date(txtisdate.Text.Trim(), vardate);
        oporow["invno"] = txtedn.Text;
        oporow["invdate"] = fgen.make_def_Date(txtedate.Text.Trim(), vardate);
        oporow["issue_by"] = txtiby.Text;
        oporow["finvno"] = hf1.Value;


        //txtisdate    
        //    invdate = DateTime.Parse(txtedate.Value, AustralianDateFormat);
        //    idate = DateTime.Parse(txtidate.Value, AustralianDateFormat);
        //    rdate = DateTime.Parse(txtrdate.Value, AustralianDateFormat);
        //    rsdate = DateTime.Parse(txtisdate.Value, AustralianDateFormat);
        //        oporow["issue_date"] = idate;
        //        oporow["rdate"] = rdate;
        //        oporow["rcpt_date"] = rsdate;
        //        oporow["invdate"] =  invdate;
        if (txtAttch.Text.Length > 1)
        {
            oporow["filepath"] = lblUpload.Text.Trim();
            oporow["filename"] = txtAttch.Text.Trim();
        }
        else if (lblUpload.Text.Length > 1)
        {
            oporow["filepath"] = lblUpload.Text.Trim();
            oporow["filename"] = lblUpload.Text.Trim().Split('~')[1];
        }
        else
        {
            oporow["filepath"] = "-";
            oporow["filename"] = "-";
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

    //protected void btnhideF_S_Click(object sender, EventArgs e)
    //{
    //    vchdate = DateTime.Parse(hffielddt.Value, AustralianDateFormat);
    //    invdate = DateTime.Parse(txtedate.Value, AustralianDateFormat);
    //    idate = DateTime.Parse(txtidate.Value, AustralianDateFormat);
    //    rdate = DateTime.Parse(txtrdate.Value, AustralianDateFormat);
    //    rsdate = DateTime.Parse(txtisdate.Value, AustralianDateFormat);
    //    presentdate = DateTime.Parse(fgen.InserTime(vardate), AustralianDateFormat);

    //    entby = (string)ViewState["ENTBY"];
    //    edt = (string)ViewState["ENTDT"];

    //    edmode = hfedmode.Value;

    //    col1 = "";
    //    col1 = Request.Cookies["Column1"].Value.ToString().Trim();
    //    if (col1 == "NO") { }
    //    else
    //    {
    //        fgen.fillcontrol(this.Controls);

    //        con.Open();

    //        if (edmode == "Y")
    //        {
    //            entdate = DateTime.Parse(edt);

    //            cmd = new OracleCommand("update DRAWREC  set branchcd='DD' where branchcd ='" + mbr + "' and type='DR' and vchnum='" + txtdocno.Text.Trim() + "' and to_DatE(to_char(vchdate,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + hffielddt.Value + "','dd/mm/yyyy')", con);
    //            cmd.ExecuteNonQuery();

    //        }

    //        da = new OracleDataAdapter();
    //        cb = new OracleCommandBuilder();
    //        oDS = new DataSet();
    //        pTable = new DataTable();

    //        da = new OracleDataAdapter(new OracleCommand("SELECT * FROM DRAWREC  where 1=2 ", con));
    //        cb = new OracleCommandBuilder(da);
    //        da.FillSchema(oDS, SchemaType.Source);

    //        pTable = oDS.Tables["Table"];
    //        pTable.TableName = "DRAWREC";

    //        vchnum = string.Empty;

    //        if (edmode == "Y") { }
    //        else
    //        {
    //            query = "select max(vchnum) as vch from DRAWREC where branchcd = '" + mbr + "' and type='DR' /*and vchdate " + daterange + "*/ ";
    //            txtdocno.Text = fgen.Gen_No(co_cd, query, "vch", 6);
    //        }
    //        vchnum = txtdocno.Text.Trim();



    //        oporow = oDS.Tables["DRAWREC"].NewRow();


    //        oporow["vchnum"] = vchnum.Trim();
    //        oporow["vchdate"] = vchdate;
    //        oporow["BRANCHCD"] = mbr;
    //        oporow["TYPE"] = "DR";
    //        oporow["dtype"] = txtdtype.Text;
    //        oporow["dno"] = txtdno.Text;
    //        oporow["rno"] = txtrno.Text;
    //        oporow["issue_date"] = idate;
    //        oporow["rdate"] = rdate;
    //        oporow["tno"] = txtdelay.Text;
    //        oporow["remarks"] = txtreason.Text;
    //        oporow["rcpt_date"] = rsdate;
    //        oporow["invno"] = txtedn.Text;
    //        oporow["invdate"] =  invdate;
    //        oporow["issue_by"] = txtiby.Text;
    //        oporow["finvno"] = hf1.Value;


    //        if (edmode == "Y")
    //        {
    //            oporow["ent_by"] = entby;
    //            oporow["ent_dt"] = entdate;
    //            oporow["edt_by"] = uname;
    //            oporow["edt_dt"] = presentdate;
    //        }
    //        else
    //        {
    //            oporow["ent_by"] = uname;
    //            oporow["ent_dt"] = presentdate;
    //            oporow["edt_by"] = "-";
    //            oporow["edt_dt"] = presentdate;

    //        }


    //        oDS.Tables["DRAWREC"].Rows.Add(oporow);

    //        da.Update(oDS, "DRAWREC");

    //        if (edmode == "Y")
    //        {
    //            cmd = new OracleCommand("delete from DRAWREC  where branchcd='DD' and type='DR' and vchnum='" + txtdocno.Text.Trim() + "' and to_DatE(to_char(vchdate,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + hffielddt.Value + "','dd/mm/yyyy')", con);
    //            cmd.ExecuteNonQuery();

    //            cmd = new OracleCommand("delete from filetable where branchcd='" + mbr + "' and type='DR' and vchnum='" + txtdocno.Text.Trim() + "' and to_DatE(to_char(vchdate,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + hffielddt.Value + "','dd/mm/yyyy')", con);
    //            cmd.ExecuteNonQuery();
    //        }

    //        vchnum = "";
    //        query = "select max(vchnum) as vch from mailbox2 where branchcd = '" + mbr + "' and type='10' /*and vchdate " + daterange + "*/";
    //        vchnum = fgen.Gen_No(co_cd, query, "vch", 6);


    //        dt = new DataTable();
    //        da = new OracleDataAdapter("Select trim(auser) from typemst where trim(acode)='43103' and id='MI'", con);
    //        da.Fill(dt);

    //        if(dt.Rows.Count>0)
    //        {
    //            string [] muser=dt.Rows[0][0].ToString().Trim().Split(',');

    //            for (int i = 0; i < muser.Length; i++)
    //            {
    //                fgen.mailsave(mbr, "10", vchnum, hffielddt.Value, "Drawing Print No : " + txtdno.Text + " Drawing Type : " + txtdtype.Text + " Revision No : " + txtrno.Text + " Against Issue Serial No : " + txtdocno.Text + " and Issue on Date : " + txtisdate.Value + " has been received on date : " + txtrdate.Value, muser[i].ToString().Trim(), vardate, uname, txtrdate.Value, co_cd, uname);
    //            }
    //        }

            
    //        if (edmode == "Y")
    //            AlertMsg("AMSG", "Drawing Receipt No. " + txtdocno.Text + " Dated " + hffielddt.Value + " Updated Successfully." );
    //        else
    //            AlertMsg("AMSG", "Drawing Receipt No. " + txtdocno.Text + " Dated " + hffielddt.Value + " Saved Successfully." );

    //        cleardata();


    //        fgen.DisableForm(this.Page);
    //        btnenable();
    //        btnsave.Disabled = true;

    //        con.Close();
    //    }
    //}




    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
        //  fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtdate.Focus(); return; }

        //dhd = fgen.ChkDate(txtrdate.Text.ToString());
        //if (dhd == 0)
        //{ fgen.msg("-", "AMSG", "Please Select a Valid Return Traget Date"); txtrdate.Focus(); return; }

        //string mandField = "";
        //mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        //if (mandField.Length > 1)
        //{
        //    fgen.msg("-", "AMSG", mandField);
        //    return;
        //}
        if (frm_ulvl == "2.5")
        {
            fgen.msg("-", "AMSG", "Dear  " + frm_uname + ",You Have Rights to View Only, So ERP Will Not Allow You to Modify Data !");
            return;
        }
        //if (txtdtype.Text.Trim().Length < 2)
        //{
        //    fgen.msg("-", "AMSG", "Please Select Drawing Type");
        //    return;
        //}
        //if (txtdno.Text.Trim().Length < 2)
        //{            
        //    fgen.msg("-", "AMSG", "Please enter Drawing No.");           
        //    return;
        //}
        if (Convert.ToDateTime(txtrdate.Text) < Convert.ToDateTime(txtdate.Text))
        {
            fgen.msg("-", "AMSG", "Return date cannot be less than the entry date.");
            txtdate.Focus();
            return;
        }
        if (txtAttch.Text.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please select attachment");
            return;
        }

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        btnsave.Disabled = true;
    }

   // protected void btnsave_Click(object sender, EventArgs e)
  //  {

        //fgen.RemoveTextBoxBorder(this.Page);
        //alermsg.Style.Add("display", "none");
        //if (ulevel == "2.5")
        //{
        //    AlertMsg("AMSG", "Dear  " + uname + ",You Have Rights to View Only, So ERP Will Not Allow You to Modify Data !");
        //    return;
        //}
        //if (txtdtype.Text.Trim().Length == 0)
        //{
        //    AlertMsg("AMSG", "Please enter Drawing Type.");
        //    txtdtype.BorderColor = Color.Red;
        //    return;
        //}
        //if (txtdno.Text.Trim().Length == 0)
        //{
        //    AlertMsg("AMSG", "Please enter Drawing No.");
        //    txtdno.BorderColor = Color.Red;
        //    return;
        //}
 
        //if (txtrdate.Value.Length > 4)
        //{

        //    DateTime dat1 = DateTime.ParseExact(txtrdate.Value, "dd/MM/yyyy", null);
        //    DateTime dat2 = DateTime.ParseExact(txtdate.Value, "dd/MM/yyyy", null);

        //    if (dat1.Date < dat2.Date)
        //    {
        //        AlertMsg("AMSG", "Return date cannot be less than the entry date.");
        //        txtrdate.Style.Add("Border", "1px Red solid");
        //        return;
        //    }
        //}

    //    hfbtnmode.Value = "SURE_S";
    //    btnsave.Disabled = true;
    //    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "confirm", "<script>$(document).ready(function(){ MyConfirm2('Are you sure you want save Drawing Receipt Record?'); });</script>", false);

//}

    protected void btnOKTarget_Click(object sender, EventArgs e)
    {
        btnhideF_s_Click(sender, e);
    }
    protected void btnCancelTarget_Click(object sender, EventArgs e)
    {
        btnsave.Disabled = false;
    }

    //protected void btnlist_Click(object sender, EventArgs e)
    //{
    //    query = "";
    //    clearcontrol();
    //    hfedmode.Value = "LI";

    //    headername = "Drawing Receipt Report";
    //    query = "SELECT distinct 'NODRILL' as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,invno as issue_no,dtype as drawing_type,dno as drawing_no, to_char(rcpt_date,'dd/mm/yyyy') as issue_date,to_char(rdate,'dd/mm/yyyy') as return_target_date,to_char(issue_date,'dd/mm/yyyy') as actual_return_date,rno as revision_no,issue_by ,tno as delay_if_any,remarks as delay_reason,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from DRAWREC  where branchcd ='" + mbr + "' and type='DR' /*and  vchdate " + daterange + "*/ order by VDD desc,vchnum desc ";


    //    if (query == "") { }
    //    else
    //    {
    //        Response.Cookies["seeksql"].Value = query;
    //        Response.Cookies["headername"].Value = headername;
    //    }
    //    ScriptManager.RegisterStartupScript(btnlist, this.GetType(), "abcd", "$(document).ready(function(){OpenPopup('" + headername + "','rptlevel2.aspx','90%','97%',false);});", true);
    //}

    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }


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

    //protected void btndelete_Click(object sender, EventArgs e)
    //{
    //    hfedmode.Value = "D";
    //    hfbtnmode.Value = "Del";
    //    sseekfunc();
    //}

    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }

    //protected void btnexit_Click(object sender, EventArgs e)
    //{
    //    if (btnexit.InnerHtml == "Exit")
    //        Response.Redirect("Desktop.aspx");
    //    else
    //    {
    //        clearcontrol();

    //        fgen.DisableForm(this.Page);
    //        btnenable();
    //        btnsave.Disabled = true;

    //        ViewState["ENTBY"] = null;
    //        ViewState["ENTDT"] = null;
    //        ViewState["COL1"] = null;
    //        ViewState["COL2"] = null;
    //    }
    //}
    protected void btnedn_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "ED";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select EDN", frm_qstr);
    }
   

}
