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


//DRAW....ecpl local formid

public partial class draw : System.Web.UI.Page
{
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
    int i, z = 0, srno, filesrno;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query, btnval;
    string frm_mbr, frm_vty, frm_vnum, frm_url, fromdt, todt, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, wSeriesControl = "";
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

                HFOPT.Value = fgen.getOptionPW(frm_qstr, frm_cocd, "W2030", "OPT_ENABLE", frm_mbr);
                hfLead.Value = fgen.getOptionPW(frm_qstr, frm_cocd, "W2031", "OPT_ENABLE", frm_mbr);
            }
            // btnAtt.Visible = false;
        }
        setColHeadings();
        set_Val();

    }

    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = true; btncancel.Visible = false;
        btnlist.Disabled = false; //  btnprint.Disabled = false;
        btndno.Enabled = false; btndtype.Enabled = false;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlist.Disabled = true;//  btnprint.Disabled = true;
        btndno.Enabled = true; btndtype.Enabled = true;
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
    { }

    //------------------------------------------------------------------------------------
    public void clearctrl()
    {
        hffield.Value = "";
        edmode.Value = "";
    }
    //-------------------------------------------------------------------------------------
    public void set_Val()
    {
        doc_nf.Value = "VCHNUM";
        doc_df.Value = "VCHDATE";
        lblheader.Text = "Drawing Entry";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "WB_DRAWREC";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "DE");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        wSeriesControl = HFOPT.Value;
        //typePopup = "N";     
        divLeadInfo.Visible = false;
        if (hfLead.Value == "Y")
        {
            divLeadInfo.Visible = true;

            if (txtRejRemarks.Text.Length <= 10)
                divCustRej.Style.Add("display", "none");

            txtrno.ReadOnly = true;
        }
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
            case "DT":
                col1 = "";
                col1 = fgen.seek_iname(frm_qstr, co_cd, "select trim(userid) as userid from evas where trim(username)='" + frm_uname + "'", "userid");
                SQuery = "select type1 as fstr,name as DRAWing_TYPE,type1 as Code from typemst where id='WT' AND TRIM(USER) LIKE '%" + col1 + "%'  order by type1"; //OLD
                SQuery = "select type1 as fstr,name as DRAWing_TYPE,type1 as Code from TYPEGRP where id='WT' order by type1";
                break;
            case "DN":
                SQuery = "select icode as fstr, iname as product,icode AS erpcode,unit,cpartno as partno from item where length(trim(icode))=8 order by iname";
                if (wSeriesControl == "Y")
                    SQuery = "select type1 as fstr, name as product,type1 AS code,acref as detail1,acref2 as detail2,acref3 as detail3 from TYPEGRP WHERE BRANCHCD NOT IN ('DD','88') AND ID='P1' order by type1,name";
                break;
            case "ACODE":
                SQuery = "select acode as fstr, aname as customer,acode AS code from famst where substr(trim(acode),1,2) in ('16') order by acode,aname";
                if (wSeriesControl == "Y")
                    SQuery = "select type1 as fstr, name as customer,type1 AS code,acref as detail1,acref2 as detail2,acref3 as detail3 from TYPEGRP WHERE BRANCHCD NOT IN ('DD','88') AND ID='C1' order by type1,name";
                break;
            case "SURE":
                SQuery = "Select 'YES' as col1,'Yes,Please' as Text,'Record Will be Deleted' as Action from dual union all Select 'NO' as col1,'No,Do Not' as Text,'Record Will Not be Deleted' as Action from dual";
                break;
            case "DSTYPE":
                SQuery = "SELECT TYPE1 AS FSTR,NAME AS DESIGN_TYPE,TYPE1 AS CODE,ENT_BY FROM TYPEGRP WHERE ID='WD' ORDER BY TYPE1";
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
                SQuery = "SELECT TRIM(A.LRCNO)||'-'||TO_cHAR(A.LRCDT,'DD/MM/YYYY') AS FSTR,TRIM(A.LRCNO) AS LEAD_NO,TO_cHAR(A.LRCDT,'DD/MM/YYYY') AS LEAD_DT,A.Lsubject AS PRODUCT_REQUIRMENT,A.LRemarks AS CLIENT_REMARKS,A.APP_BY,TO_cHAR(A.APP_DT,'DD/MM/YYYY') AS APP_dT,A.ENT_BY,TO_CHAR(A.ENT_DT,'DD/MM/YYYY') AS ENT_dT,TO_cHAR(A.LRCDT,'YYYYMMDD') AS VDD FROM WB_LEAD_LOG A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='LR' AND TRIM(A.LRCNO)||TO_cHAR(A.LRCDT,'DD/MM/YYYY') NOT IN (SELECT TRIM(INVNO)||TO_CHAR(INVDATE,'DD/MM/YYYY') AS FSTR FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND TRIM(NVL(APP_BY,'-'))!='-' AND SUBSTR(APP_BY,1,3)!='[C]') ORDER BY VDD DESC,TRIM(A.LRCNO) DESC ";
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD" || btnval == "Print_E")
                {
                    if (wSeriesControl == "Y")
                        SQuery = "SELECT distinct A.branchcd||A.type||A.vchnum||to_char(A.vchdate,'DD/MM/YYYY') as fstr,A.vchnum as entry_no,to_char(A.vchdate,'dd/mm/yyyy') as entry_date,C.NAME AS PRODUCT_NAME,C.ACREF2 AS PART_NAME,B.NAME AS CUSTOMER,A.dno as part_no,A.rno as revision_no,A.ent_by,A.ent_dt, to_char(A.vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + " A,TYPEGRP B,TYPEGRP c WHERE B.ID='C1' AND C.ID='P1' AND trim(B.TYPE1)=TRIM(A.ACODE) AND TRIM(C.TYPE1)=TRIM(A.ICODE) AND A.branchcd ='" + frm_mbr + "' and A.type='DE' /*and  vchdate " + DateRange + "*/ " + DRID + " order by VDD desc,A.vchnum desc ";
                    else SQuery = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,dno as part_no,rno as revision_no,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + "  where branchcd ='" + frm_mbr + "' and type='DE' /*and  vchdate " + DateRange + "*/ " + DRID + " order by VDD desc,vchnum desc ";
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


    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            if (hfLead.Value == "Y")
            {
                typePopup = "Y";
            }
            hffield.Value = "New";
            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Lead No", frm_qstr);
            }
            btnCust.Focus();
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }

    void newCase(string vty)
    {
        #region
        vty = "DE";
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
        setColHeadings();
        ViewState["filesrno"] = 0;

        if (hfLead.Value == "Y")
        {
            txtLeadNO.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
            txtLeadDT.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
            txtSubject.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
            txtClientRemarks.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");

            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT a.*,to_char(A.VCHDATE,'YYYYMMDD')||TRIM(A.VCHNUM) AS VDD,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr FROM " + frm_tabname + " A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND TRIM(A.INVNO)||TO_CHAR(A.INVDATE,'DD/MM/YYYY')='" + txtLeadNO.Text.Trim() + txtLeadDT.Text.Trim() + "' AND SUBSTR(A.APP_BY,1,3)='[C]' ORDER BY VDD DESC ");
            if (dt.Rows.Count > 0)
            {
                col1 = dt.Rows[0]["FSTR"].ToString().Trim();
                divCustRej.Style.Remove("display");
                txtRejRemarks.Text = "Drawing Entry No - Dt : " + dt.Rows[0]["vchnum"].ToString().Trim() + " - " + Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy") + ", Reason of rejection : " + dt.Rows[0]["FILENAME"].ToString().Trim();

                txtIcode.Text = dt.Rows[0]["dno"].ToString().Trim();
                txtAcode.Text = dt.Rows[0]["acode"].ToString().Trim();
                txtAName.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT aname FROM FAMST WHERE ACODE='" + txtAcode.Text.Trim() + "'", "ANAME");
                txtdno.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT CPARTNO FROM ITEM WHERE ICODE='" + txtIcode.Text.Trim() + "'", "CPARTNO");
                txtIname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT iname as CPARTNO FROM ITEM WHERE ICODE='" + txtIcode.Text.Trim() + "'", "CPARTNO");
                txtrno.Text = dt.Rows[0]["rno"].ToString().Trim();
                hf1.Value = dt.Rows[0]["Tno"].ToString().Trim();

                txtremarks.Text = dt.Rows[0]["REMARKS"].ToString().Trim();
                txtECNO.Text = dt.Rows[0]["col1"].ToString().Trim();

                txtrno.Text = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(is_number(rno)) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND TRIM(ACODE)||TRIM(ICODE)||tRIM(INVNO)||TO_cHAR(INVDATE,'DD/MM/YYYYY')='" + txtAcode.Text.Trim() + txtIcode.Text.Trim() + txtLeadNO.Text.Trim() + txtLeadDT.Text.Trim() + "'", 3, "VCH");
                txtRdt.Text = DateTime.Now.ToString("yyyy-MM-dd");

                txtLeadNO.Text = dt.Rows[0]["INVNO"].ToString().Trim();
                txtLeadDT.Text = Convert.ToDateTime(fgen.make_def_Date(dt.Rows[0]["INVDATE"].ToString().Trim(), vardate)).ToString("dd/MM/yyyy");

                txtClientRemarks.Text = dt.Rows[0]["FINVNO"].ToString().Trim();
                txtSubject.Text = dt.Rows[0]["COL5"].ToString().Trim();

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.* FROM ATCHVCH A WHERE a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' order by a.MSGDT");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sg1_dr = sg1_dt.NewRow();
                    sg1_dr["srno"] = Convert.ToInt32(dt.Rows[i]["MSGDT"].ToString());
                    sg1_dr["filno"] = dt.Rows[i]["MSGTXT"].ToString();

                    sg1_dr["design"] = dt.Rows[i]["TERMINAL"].ToString();
                    sg1_dr["dactive"] = dt.Rows[i]["MSGFROM"].ToString();
                    sg1_dr["candown"] = dt.Rows[i]["MSGTO"].ToString();
                    sg1_dr["stage"] = dt.Rows[i]["INVNO"].ToString();
                    sg1_dt.Rows.Add(sg1_dr);
                }
                ViewState["sg1"] = sg1_dt;
                sg1.DataSource = sg1_dt;
                sg1.DataBind();

                ViewState["filesrno"] = sg1_dt.Rows.Count;
                set_oldRow();
            }
            else
            {
                divCustRej.Style.Add("display", "none");
            }

        }
        #endregion
    }


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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ATCHVCH a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
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

                    break;

                case "DT":
                    txtdtype.Text = col2;
                    hf1.Value = col1;
                    //btnview_Click(sender, e);
                    //btnDesignType.Focus();
                    break;
                case "DSTYPE":
                    //txtdesigncode.Text = col1;
                    //txtDesignType.Text = col2;
                    txtremarks.Focus();
                    break;
                case "ACODE":
                    txtAcode.Text = col1;
                    txtAName.Text = col2;
                    btndno.Focus();
                    break;
                case "DN":
                    //if (mbr == "03" || mbr == "04") txtdno.Text = sname;
                    //else
                    //{
                    //    if (sname.Length > 4)
                    //        txtdno.Text = fgen.substr_numeric(sname, 0);
                    //}
                    if (col1.Length > 3)
                    {
                        col4 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(VCHNUM)||'-'||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND TRIM(ACODE)='" + txtAcode.Text.Trim() + "' AND TRIM(ICODE)='" + col1 + "' ", "FSTR");
                        if (col4.Length > 5)
                        {
                            fgen.msg("Duplicate Drawing entry not allowed!!", "AMSG", "Drawing is already there for'13'Product : " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + " '13'Customer : " + txtAName.Text + "'13'See Entry No-Date " + col4 + " ");
                            return;
                        }
                        else
                        {
                            txtIcode.Text = col1;
                            txtdno.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");
                            txtIname.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                            btndtype.Focus();
                        }
                    }
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
                        txtdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                        //txtdtype.Text = dt.Rows[0]["dtype"].ToString().Trim();
                        txtIcode.Text = dt.Rows[0]["dno"].ToString().Trim();
                        txtAcode.Text = dt.Rows[0]["acode"].ToString().Trim();
                        if (wSeriesControl == "Y")
                            txtAName.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NAME FROM TYPEGRP WHERE ID='C1' AND TYPE1='" + txtAcode.Text.Trim() + "'", "NAME");
                        else
                            txtAName.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT aname FROM FAMST WHERE ACODE='" + txtAcode.Text.Trim() + "'", "ANAME");
                        if (wSeriesControl == "Y")
                        {
                            txtdno.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF2 FROM TYPEGRP WHERE ID='P1' AND TYPE1='" + txtIcode.Text.Trim() + "'", "ACREF2");
                            txtIname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NAME FROM TYPEGRP WHERE ID='P1' AND TYPE1='" + txtIcode.Text.Trim() + "'", "NAME");
                        }
                        else
                        {
                            txtdno.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT CPARTNO FROM ITEM WHERE ICODE='" + txtIcode.Text.Trim() + "'", "CPARTNO");
                            txtIname.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT iname as CPARTNO FROM ITEM WHERE ICODE='" + txtIcode.Text.Trim() + "'", "CPARTNO");
                        }
                        txtrno.Text = dt.Rows[0]["rno"].ToString().Trim();
                        hf1.Value = dt.Rows[0]["Tno"].ToString().Trim();

                        //txtdesigncode.Text = dt.Rows[0]["t8"].ToString().Trim();
                        //txtDesignType.Text = dt.Rows[0]["t9"].ToString().Trim();

                        entby = dt.Rows[0]["ent_by"].ToString().Trim();
                        edt = dt.Rows[0]["ent_dt"].ToString().Trim();

                        txtpre.Text = entby;
                        txtedit.Text = dt.Rows[0]["edt_by"].ToString().Trim();

                        txtremarks.Text = dt.Rows[0]["REMARKS"].ToString().Trim();
                        txtECNO.Text = dt.Rows[0]["col1"].ToString().Trim();

                        if (dt.Rows[0]["dtype"].ToString().Trim().Length > 3)
                            txtRdt.Text = Convert.ToDateTime(dt.Rows[0]["dtype"].ToString().Trim()).ToString("yyyy-MM-dd");

                        if (hfLead.Value == "Y")
                        {
                            txtLeadNO.Text = dt.Rows[0]["INVNO"].ToString().Trim();
                            txtLeadDT.Text = Convert.ToDateTime(fgen.make_def_Date(dt.Rows[0]["INVDATE"].ToString().Trim(), vardate)).ToString("dd/MM/yyyy");

                            txtClientRemarks.Text = dt.Rows[0]["FINVNO"].ToString().Trim();
                            txtSubject.Text = dt.Rows[0]["COL5"].ToString().Trim();
                        }

                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        dt.Dispose();
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                        create_tab();
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.* FROM ATCHVCH A WHERE a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' order by a.MSGDT");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["srno"] = Convert.ToInt32(dt.Rows[i]["MSGDT"].ToString());
                            sg1_dr["filno"] = dt.Rows[i]["MSGTXT"].ToString();

                            sg1_dr["design"] = dt.Rows[i]["TERMINAL"].ToString();
                            sg1_dr["dactive"] = dt.Rows[i]["MSGFROM"].ToString();
                            sg1_dr["candown"] = dt.Rows[i]["MSGTO"].ToString();
                            sg1_dr["stage"] = dt.Rows[i]["INVNO"].ToString();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();

                        ViewState["filesrno"] = sg1_dt.Rows.Count;

                        set_oldRow();
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

                            sg1_dr["srno"] = Convert.ToInt32(dt.Rows[i]["srno"].ToString());
                            sg1_dr["filno"] = sg1.Rows[i].Cells[4].Text.Trim();
                            sg1_dr["design"] = ((DropDownList)sg1.Rows[i].FindControl("ddDesign")).SelectedItem.Text;
                            sg1_dr["dactive"] = ((DropDownList)sg1.Rows[i].FindControl("ddActive")).SelectedItem.Text;
                            sg1_dr["candown"] = ((DropDownList)sg1.Rows[i].FindControl("ddDwnl")).SelectedItem.Text;
                            sg1_dr["stage"] = ((DropDownList)sg1.Rows[i].FindControl("ddStage")).SelectedItem.Text;
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();

                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();

                        set_oldRow();
                    }
                    #endregion
                    break;
            }
        }
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

    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        sg1_dt.Columns.Add(new DataColumn("SrNo", typeof(Int32)));
        sg1_dt.Columns.Add(new DataColumn("filno", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("design", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("dactive", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("candown", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("stage", typeof(string)));
        ViewState["sg1"] = sg1_dt;
    }
    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        if (sg1_dt != null)
        {
            sg1_dr = sg1_dt.NewRow();
            sg1_dt.Rows.Add(sg1_dr);
        }
    }

    private DataTable FileTable
    {
        get
        {
            if (ViewState["fileTable"] != null)
                return (DataTable)ViewState["fileTable"];
            else
            {
                dt = new DataTable();

                dt.Columns.Add("srno", typeof(string));
                dt.Columns.Add("FILENAME", typeof(string));
                dt.Columns.Add("FILEPATH", typeof(string));
                dt.Columns.Add("FILETYPE", typeof(string));

                ViewState["fileTable"] = dt;
                return dt;
            }
        }
        set
        {
            ViewState["fileTable"] = value;

        }
    }
    private void BindRepeater()
    {
        //grddisp.DataSource = FileTable;
        //grddisp.DataBind();
        //grddisp.Visible = true;
    }

    public void viewpic(string imgpath)
    {
        Session["MYURL"] = imgpath;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('Attachment Preview Window','View.aspx','95%','95%');});", true);
    }



    private void cleargrd_ds()
    {
        FileTable = null;
        //grddisp.DataSource = FileTable;
        //grddisp.DataBind();
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
            string ext = System.IO.Path.GetExtension(Attch.FileName).ToLower();
            txtAttch.Text = Attch.FileName;
            if (ViewState["filesrno"] != null) filesrno = (int)ViewState["filesrno"];

            filepath = frm_mbr + "_" + "DI" + "_" + txtdocno.Text.Trim() + "_" + txtdate.Text.Replace(@"/", "_") + "_File_" + (filesrno + 1);
            filename = filepath + ext;
            Attch.PostedFile.SaveAs(@"c:\TEJ_ERP\UPLOAD\" + filename);

            filepath = Server.MapPath("~/tej-base/UPLOAD/") + filename;
            Attch.PostedFile.SaveAs(filepath);
            fill_grid();
            filesrno++;
            ViewState["filesrno"] = filesrno;
        }
        else
        {
            lblUpload.Text = "";
        }
    }
    public void fill_grid()
    {
        if (ViewState["sg1"] != null)
        {
            dt = new DataTable();
            dt1 = new DataTable();
            dt = (DataTable)ViewState["sg1"];
            dt1 = dt.Clone();
            dr1 = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][1].ToString().Length > 1)
                {
                    dr1 = dt1.NewRow();
                    dr1["srno"] = Convert.ToInt32(dt.Rows[i]["srno"].ToString());
                    dr1["filno"] = sg1.Rows[i].Cells[4].Text.Trim();
                    //dr1["design"] = ((HiddenField)sg1.Rows[i].FindControl("hfdesign")).Value;
                    //dr1["dactive"] = ((HiddenField)sg1.Rows[i].FindControl("hfactive")).Value;
                    //dr1["candown"] = ((HiddenField)sg1.Rows[i].FindControl("hfdown")).Value;
                    //dr1["stage"] = ((HiddenField)sg1.Rows[i].FindControl("hfStage")).Value;
                    dr1["design"] = ((DropDownList)sg1.Rows[i].FindControl("ddDesign")).SelectedItem.Text;
                    dr1["dactive"] = ((DropDownList)sg1.Rows[i].FindControl("ddActive")).SelectedItem.Text;
                    dr1["candown"] = ((DropDownList)sg1.Rows[i].FindControl("ddDwnl")).SelectedItem.Text;
                    dr1["stage"] = ((DropDownList)sg1.Rows[i].FindControl("ddStage")).SelectedItem.Text;
                    dt1.Rows.Add(dr1);
                }
            }
            dr1 = dt1.NewRow();
            dr1["srno"] = dt.Rows.Count + 1;
            dr1["filno"] = filename;
            dt1.Rows.Add(dr1);
        }
        ViewState["sg1"] = dt1;
        sg1.DataSource = dt1;
        sg1.DataBind();

        set_oldRow();
    }
    void set_oldRow()
    {
        z = 0;
        foreach (GridViewRow gr in sg1.Rows)
        {
            if (z < sg1.Rows.Count)
            {
                try
                {
                    ((DropDownList)gr.FindControl("ddDesign")).Items.FindByText(((HiddenField)gr.FindControl("hfdesign")).Value).Selected = true;
                }
                catch { }
                try
                {
                    ((DropDownList)gr.FindControl("ddActive")).Items.FindByText(((HiddenField)gr.FindControl("hfactive")).Value).Selected = true;
                }
                catch { }
                try
                {
                    ((DropDownList)gr.FindControl("ddDwnl")).Items.FindByText(((HiddenField)gr.FindControl("hfdown")).Value).Selected = true;
                }
                catch { }
                try
                {
                    //((DropDownList)gr.FindControl("ddStage")).SelectedItem.Value = ((HiddenField)gr.FindControl("hfStage")).Value;
                    ((DropDownList)gr.FindControl("ddStage")).Items.FindByText(((HiddenField)gr.FindControl("hfStage")).Value).Selected = true;
                }
                catch { }
                z++;
            }
        }
    }
    protected void btnDown_Click(object sender, EventArgs e)
    {
        try
        {
            string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
            //Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");//old
            Session["FilePath"] = lblUpload.Text;
            Session["FileName"] = txtAttch.Text;
            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");
        }
        catch { }
    }

    protected void btnpr1_Click(object sender, EventArgs e)
    {
        //if (PhotoUpload.HasFile)
        //{

        //    fName = PhotoUpload.FileName;
        //    compnay_code = co_cd;

        //    fName = compnay_code + "_" + txtdocno.Text + "_" + fName;
        //    fpath = Server.MapPath("Uploads") + "\\" + fName;
        //    PhotoUpload.SaveAs(fpath);

        //    string vpath = @"c:\TEJ_ERP\uploads\" + fName;
        //    PhotoUpload.SaveAs(vpath);

        //    extension = Path.GetExtension(fpath).ToLower();
        //    DataRow dr = FileTable.NewRow();
        //    dr["srno"] = grddisp.Rows.Count + 1;
        //    dr["FileName"] = fName;
        //    dr["FilePath"] = fpath;
        //    dr["FileType"] = extension;
        //    FileTable.Rows.Add(dr);
        //    BindRepeater();
        //}
    }
    protected void LnkBtn_Click(object sender, EventArgs e)
    {
        //i = 0;
        //hfbtnmode.Value = "PR1";
        //fName = ""; fpath = ""; extension = "";
        //LinkButton selectButton = (LinkButton)sender;
        //GridViewRow row = (GridViewRow)selectButton.Parent.Parent;
        //fpath = grddisp.Rows[row.RowIndex].Cells[5].Text.Trim().ToString();
        //extension = grddisp.Rows[row.RowIndex].Cells[6].Text.Trim().ToString();
        //OpenMyFile(fpath, extension);
    }
    protected void LnkBtn1_Click(object sender, EventArgs e)
    {
        // i = 0;
        //hfbtnmode.Value = "DN1";
        // fName = ""; fpath = ""; extension = "";
        // LinkButton selectButton = (LinkButton)sender;
        // GridViewRow row = (GridViewRow)selectButton.Parent.Parent;
        // fpath = grddisp.Rows[row.RowIndex].Cells[5].Text.Trim().ToString();
        // extension = grddisp.Rows[row.RowIndex].Cells[6].Text.Trim().ToString();
        // OpenMyFile(fpath, extension);
    }
    public void OpenMyFile(string fpath, string extension)
    {
        i = 0;
        i = fpath.IndexOf(@"\Uploads");
        fName = fpath.Substring(i, fpath.Length - i);
        if (hfbtnmode.Value == "PR1")
        {
            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp" || extension == ".pdf")
                viewpic(fName);
            else
                viewpic("XXXX");
        }
        if (hfbtnmode.Value == "DN1") DownloadFile(fName);
    }

    protected void grddisp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //if (grddisp.Rows[e.RowIndex].Cells[5].Text == "" || grddisp.Rows[e.RowIndex].Cells[5].Text == "&nbsp;") { }
        //else
        //{
        //    DataRow[] dr = FileTable.Select("SRNO='" + grddisp.DataKeys[e.RowIndex][0].ToString() + "'");
        //    FileTable.Rows.Remove(dr[0]);
        //    grddisp.Rows[e.RowIndex].Style.Add("display", "none");
        //    grddisp.Visible = false;
        //    foreach (GridViewRow row in grddisp.Rows)
        //    {
        //        if (row.Style["display"] != "none")
        //        {
        //            grddisp.Visible = true;
        //            continue;
        //        }
        //    }
        //}
    }
    protected void grddisp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate) && (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header))
        {
            e.Row.Cells[3].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[5].Visible = false;
            e.Row.Cells[6].Visible = false;
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
            SQuery = "SELECT vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,dtype as drawing_type,dno as part_no,rno as revision_no,T9 AS drawing_stage,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + "  where branchcd ='" + frm_mbr + "' and type='DE' and  vchdate " + PrdRange + " order by VDD desc,vchnum desc ";
            SQuery = "SELECT a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,c.aname as customer,a.acode as code,b.iname as part_name,b.cpartno as part_number,a.icode as erpcode,a.dtype as drawing_type,a.dno as part_no,a.rno as revision_no,a.T9 AS drawing_stage,a.ent_by,a.ent_dt, to_char(a.vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + " a,ITEM B,FAMST C where TRIM(A.ICODE)=TRIM(b.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND a.branchcd ='" + frm_mbr + "' and a.type='DE' and a.vchdate " + PrdRange + " order by VDD desc,a.vchnum desc ";
            if (wSeriesControl == "Y")
                SQuery = "SELECT a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,c.name as customer,a.acode as code,b.name as part_name,b.ACREF2 as part_number,a.icode as erpcode,a.dtype as drawing_type,a.dno as part_no,a.rno as revision_no,a.T9 AS drawing_stage,a.ent_by,a.ent_dt, to_char(a.vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + " a,TYPEGRP B,TYPEGRP C where TRIM(A.ICODE)=TRIM(b.TYPE1) AND B.ID='P1' AND TRIM(A.ACODE)=TRIM(C.TYPE1) AND C.ID='C1' AND a.branchcd ='" + frm_mbr + "' and a.type='DE' and a.vchdate " + PrdRange + " order by VDD desc,a.vchnum desc ";

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
                            cmd_query = "update ATCHVCH set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        oDS2 = new DataSet();
                        oporow2 = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "ATCHVCH");
                        for (i = 0; i < sg1.Rows.Count; i++)
                        {
                            oporow2 = oDS2.Tables[0].NewRow();
                            oporow2["BRANCHCD"] = frm_mbr;
                            oporow2["TYPE"] = frm_vty;
                            oporow2["vchnum"] = frm_vnum;
                            oporow2["vchdate"] = txtdate.Text;
                            oporow2["acode"] = txtAcode.Text.Trim();
                            oporow2["MSGDT"] = sg1.Rows[i].Cells[1].Text.Trim();
                            oporow2["msgtxt"] = sg1.Rows[i].Cells[4].Text.Trim();
                            oporow2["TERMINAL"] = ((DropDownList)sg1.Rows[i].FindControl("ddDesign")).SelectedItem.Text;
                            oporow2["MSGFROM"] = ((DropDownList)sg1.Rows[i].FindControl("ddActive")).SelectedItem.Text;
                            oporow2["MSGTO"] = ((DropDownList)sg1.Rows[i].FindControl("ddDwnl")).SelectedItem.Text;
                            oporow2["INVNO"] = ((DropDownList)sg1.Rows[i].FindControl("ddStage")).SelectedItem.Text;
                            oDS2.Tables[0].Rows.Add(oporow2);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, "ATCHVCH");

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtdocno.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR").Substring(2, 18) + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "delete from ATCHVCH where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR").Substring(2, 18) + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
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
                        sg1_dt = new DataTable();
                        create_tab();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        ViewState["sg1"] = null;
                        ViewState["filesrno"] = 0;
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
        oporow["vchdate"] = txtdate.Text.Trim();
        oporow["ACODE"] = txtAcode.Text;
        if (txtRdt.Text.Length > 5)
            oporow["dtype"] = Convert.ToDateTime(txtRdt.Text).ToString("dd/MM/yyyy");
        else oporow["dtype"] = "-";
        oporow["ICODE"] = txtIcode.Text;
        oporow["dno"] = txtIcode.Text;
        oporow["rno"] = txtrno.Text;
        oporow["tno"] = hf1.Value;

        //oporow["T8"] = txtdesigncode.Text.Trim();
        //oporow["T9"] = ddMain.SelectedItem.Value;

        oporow["INVNO"] = txtLeadNO.Text.Trim();
        oporow["INVDATE"] = fgen.make_def_Date(txtLeadDT.Text.Trim(), vardate);

        oporow["FINVNO"] = txtClientRemarks.Text.Trim();
        oporow["COL5"] = txtSubject.Text.Trim();
        oporow["COL6"] = txtSubject.Text.Trim();

        oporow["REMARKS"] = txtremarks.Text.Trim();

        oporow["COL1"] = txtECNO.Text.Trim();

        oporow["FILENAME"] = "";

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
    //----------------------

    protected void btnctye_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        if (txtAcode.Text.Length < 3)
        {
            fgen.msg("-", "AMSG", "Please Select Party Name First.");
            return;
        }
        else
        {
            hffield.Value = "DN";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Part No", frm_qstr);
        }
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtdate.Focus(); return; }
        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }
        if (frm_ulvl == "2.5")
        {
            fgen.msg("-", "AMSG", "Dear  " + frm_uname + ",You Have Rights to View Only, So ERP Will Not Allow You to Modify Data !");
            return;
        }

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        btnsave.Disabled = true;
    }
    //------------------------------------------------------------------------------------
    protected void btnView1_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','');", true);
    }

    //------------------------------------------------------------------------------------   
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

            dt = new DataTable();
            if (ViewState["DSGT"] == null)
            {
                SQuery = "select type1 as fstr,name as DESIGN_TYPE,type1 as Code from TYPEGRP where id='WD' order by type1";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                ViewState["DSGT"] = dt;
            }
            else
            {
                dt = new DataTable();
                dt = (DataTable)ViewState["DSGT"];
            }
            if (dt.Rows.Count > 0)
            {
                DropDownList dd = (DropDownList)e.Row.FindControl("ddDesign");
                dd.DataSource = dt;
                dd.DataTextField = "DESIGN_TYPE";
                dd.DataValueField = "DESIGN_TYPE";
                dd.DataBind();
            }

            if (ViewState["DSGT1"] == null)
            {
                SQuery = "select 'Activate' as col1 from dual union all select 'De-Activate' as col1 from dual ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                ViewState["DSGT1"] = dt;
            }
            else
            {
                dt = new DataTable();
                dt = (DataTable)ViewState["DSGT1"];
            }
            if (dt.Rows.Count > 0)
            {
                DropDownList dda = (DropDownList)e.Row.FindControl("ddActive");
                dda.DataSource = dt;
                dda.DataTextField = "COL1";
                dda.DataValueField = "COL1";
                dda.DataBind();
            }
            if (ViewState["DSGT2"] == null)
            {
                SQuery = "select 'YES' as col1 from dual union all select 'NO' as col1 from dual ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                ViewState["DSGT2"] = dt;
            }
            else
            {
                dt = new DataTable();
                dt = (DataTable)ViewState["DSGT2"];
            }
            if (dt.Rows.Count > 0)
            {
                DropDownList ddD = (DropDownList)e.Row.FindControl("ddDwnl");
                ddD.DataSource = dt;
                ddD.DataTextField = "COL1";
                ddD.DataValueField = "COL1";
                ddD.DataBind();
            }
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
            case "Rmv":
                if (index < sg1.Rows.Count)
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','');", true);
                break;
        }
    }
    //------------------------------------------------------------------------------------

    //=====================

    protected void btnsave_Click(object sender, EventArgs e)
    {

    }
    protected void btnOKTarget_Click(object sender, EventArgs e)
    {
        btnhideF_s_Click(sender, e);
    }
    protected void btnCancelTarget_Click(object sender, EventArgs e)
    {
        btnsave.Disabled = false;
    }
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }

    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Entry for Print", frm_qstr);
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

    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
    }

    protected void btncancel_ServerClick(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();
        sg1_dt = new DataTable();
        create_tab();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        ViewState["sg1"] = null;
        ViewState["filesrno"] = 0;
        setColHeadings();
        lblUpload.Text = "";
        set_Val();
    }

    //protected void btnDesignType_Click(object sender, ImageClickEventArgs e)
    //{
    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
    //    hffield.Value = "DSTYPE";
    //    make_qry_4_popup();
    //    fgen.Fn_open_sseek("Select Design Type", frm_qstr);
    //}
    protected void btndtype_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "DT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Drawing Type", frm_qstr);
    }
    protected void btnCust_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer", frm_qstr);
    }
}
