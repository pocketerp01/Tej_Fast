using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.OleDb;

public partial class autoDrCrPip : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, nVty = "";
    DataTable dt, dt2, dt3, dt4;
    DataRow oporow, oporow1, oporow2; DataSet oDS, oDS1, oDS2;
    int i = 0, z = 0;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, addGrNo = "";
    //double double_val2, double_val1;
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
                    if (frm_qstr.Contains("^"))
                    {
                        if (frm_cocd != frm_qstr.Split('^')[0].ToString())
                        {
                            frm_cocd = frm_qstr.Split('^')[0].ToString();
                        }
                    }
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
                doc_addl.Value = "0";

                string lvch_5859 = fgen.getOption(frm_qstr, frm_cocd, "W0131", "OPT_ENABLE");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_lvch_5859", lvch_5859);
                string lvch_5859_date = fgen.getOption(frm_qstr, frm_cocd, "W0131", "OPT_PARAM");
                if (lvch_5859 == "Y")
                {
                    if (fgen.IsDate(lvch_5859_date))
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_lvch_5859_date", lvch_5859_date);
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Please check date in control W0131.System won't allow further processing.");
                        return;
                    }
                }
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
                btnedit.Visible = false;
                DataTable dtW = (DataTable)ViewState["dtn"];
                if (dtW != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtW, "", "ContentPlaceHolder1_datadiv").ToString(), false);
                }
                chktcs.Checked = true;
            }
            setColHeadings();
            set_Val();
            btnprint.Visible = false;
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

        // to hide and show to tab panel
        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
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
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "SCRATCH2";

        lblheader.Text = "ALL INV UPLOADING";

        addGrNo = "N";
        if (frm_cocd == "BONY")
        {
            addGrNo = "Y";
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "ZZ");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
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
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                SQuery = "SELECT Type1,Name,Type1 AS CODE,id2 as Ref FROM Type WHERE id='#' and id2='CL' ORDER BY Name ";
                break;
            case "TACODE":
                SQuery = "select acode,aname as customer,acode as code from famst where substr(trim(Acode),1,2) in ('16','02') order by acode";
                break;
            case "TRCODE":
                SQuery = "select acode,aname as customer,acode as code from famst where trim(Acode) like '2%' order by acode";
                break;
            case "DNCN":
                SQuery = "SELECT TYPE1,NAME AS REASON,TYPE1 AS CODE FROM TYPE WHERE ID='$' ORDER BY TYPE1";
                break;
            case "GSTCLASS":
                SQuery = "SELECT TYPE1,NAME AS REASON,TYPE1 AS CODE FROM TYPE WHERE ID='}' ORDER BY TYPE1";
                break;
            case "New":
            case "List":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    //SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.col33 as pono,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc"; //old
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,trim(a.acode) as acode,trim(b.aname) as customer,a.col33 as pono,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            //hffield.Value = "New";
            //make_qry_4_popup();
            //fgen.Fn_open_sseek("-", frm_qstr);

            // else comment upper code

            //frm_vnum = fgen.next_no(frm_qstr, frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
            frm_vty = "ZZ";
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            disablectrl();
            fgen.EnableForm(this.Controls);
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
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
        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only"); txtvchdate.Focus(); return; }

        if (txtacode.Value.Trim().Length < 2)
        { fgen.msg("-", "AMSG", "Please Select Customer Code!!"); txtvchdate.Focus(); return; }

        DataTable dtn = new DataTable();
        dtn = (DataTable)ViewState["dtn"];
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);

        DataView dv = new DataView(dtn);
        dtn = new DataTable();
        dtn = dv.ToTable(true, "PONO");
        dt = new DataTable();
        dt.Columns.Add("ENTRY_NO", typeof(string));
        dt.Columns.Add("ENTRY_DT", typeof(string));
        dt.Columns.Add("BRANCH", typeof(string));
        dt.Columns.Add("PONO", typeof(string));
        DataRow dr = null;
        foreach (DataRow drn in dtn.Rows)
        {
            dt2 = new DataTable();
            dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT distinct vchnum,vchdate,branchcd,COL33,min(num10) as num10 FROM SCRATCH2 WHERE COL33='" + drn["PONO"].ToString().Trim() + "' group by vchnum,vchdate,branchcd,COL33 order by vchnum desc");
            if (dt2.Rows.Count > 0)
            {
                dr = dt.NewRow();
                dr["entry_no"] = dt2.Rows[0]["vchnum"].ToString().Trim();
                dr["entry_dt"] = dt2.Rows[0]["vchdate"].ToString().Trim();
                dr["branch"] = dt2.Rows[0]["branchcd"].ToString().Trim();
                dr["PONO"] = dt2.Rows[0]["col33"].ToString().Trim();
                dt.Rows.Add(dr);
            }
        }
        string crFound = "N";
        //if (txtAname.Value.ToString().ToUpper().Contains("MARUTI"))
        //{
        //    if (dt2.Rows.Count > 0)
        //    {
        //        if (dt2.Rows[0]["num10"].ToString() == "0" && dt.Rows.Count > 0)
        //        {
        //            dtn = new DataTable();
        //            dtn = (DataTable)ViewState["dtn"];
        //            foreach (DataRow drn in dtn.Rows)
        //            {
        //                if (fgen.make_double(drn["col9"].ToString().Trim()) > 0) crFound = "Y";
        //            }
        //            if (crFound == "Y")
        //            {
        //                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", These Batch is already exist!!'13'Please Upload only Credit Entries");
        //                return;
        //            }
        //        }
        //    }
        //}
        //else if (dt.Rows.Count > 0)
        //{
        //    Session["send_dt"] = dt;
        //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
        //    fgen.Fn_open_rptlevel("These Batch No Already Exist!!'13'Please delete first befor uploading.", frm_qstr);
        //    return;
        //}

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A77'", "PARAMS");
        if ((col1 == "0" || col1 == "" || col1 == "-") && txtRcode.Value.Trim().Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Select Ledger to Save the Entry!!");
            return;
        }

        string readytoSave = "Y";
        dtn = (DataTable)ViewState["dtn"];
        DataTable dtIvoucher = new DataTable();
        dtIvoucher = fgen.getdata(frm_qstr, frm_cocd, "SELECT DISTINCT TRIM(A.VCHNUM) AS VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ICODE) AS ICODE,TRIM(A.ACODE) AS ACODE,TRIM(B.FULL_INVNO) AS FULL_INVNO FROM IVOUCHER A,SALE B WHERE A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=B.BRANCHCD||B.TYPE||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '4%' AND TRIM(A.ACODE)='" + txtacode.Value.Trim() + "' ");
        string mhd = "";
        foreach (DataRow drn in dtn.Rows)
        {
            mhd = fgen.seek_iname_dt(dtIvoucher, "VCHNUM='" + drn["INVNO"].ToString().Trim() + "' AND VCHDATE='" + drn["INVDT"].ToString().Trim() + "' AND ICODE='" + drn["icode"].ToString().Trim() + "' ", "VCHNUM");
            if (mhd == "0")
            {
                mhd = fgen.seek_iname_dt(dtIvoucher, "FULL_INVNO='" + drn["INVNO"].ToString().Trim() + "' AND VCHDATE='" + drn["INVDT"].ToString().Trim() + "' AND ICODE='" + drn["icode"].ToString().Trim() + "' ", "VCHNUM");
                if (mhd == "0")
                {
                    mhd = "Invoice No : " + drn["INVNO"].ToString().Trim() + " Date " + drn["INVDT"].ToString().Trim() + " ERPCODE : " + drn["icode"].ToString().Trim();
                    readytoSave = "N";
                    break;
                }
            }
        }
        if (readytoSave == "N")
        {
            fgen.msg("-", "AMSG", "Invoice Number not matching '13'" + mhd);
            return;
        }
        if (readytoSave == "Y")
        {
            hfCNote.Value = "Y";
            if (txtAname.Value.ToString().ToUpper().Contains("MARUTI"))
            {
                hffield.Value = "SAVE";
                fgen.msg("-", "CMSG", "Do You want to Make Credit Note too!!'13'(Select No for Debit Note Only)");
            }
            else fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
            btnsave.Disabled = true;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "Y")
        {
            clearctrl();
            set_Val();
            hffield.Value = "Del_E";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " ", frm_qstr);
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
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    protected void btnanex_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "ANEXX";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {

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
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')||trim(a.COL33)='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "'");//OLD
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table               
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||TRIM(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'  ");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||TRIM(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (select DISTINCT a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr from IVOUCHER A WHERE A.BTCHNO='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "')");
                // Deleing data from voucher Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from voucher a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (select DISTINCT a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr from IVOUCHER A WHERE A.BTCHNO='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "')");
                // Deleing data from Ivoucher Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivoucher a where A.BTCHNO='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' AND A.TYPE IN ('58','59') AND A.BRANCHCD='" + frm_mbr + "'");
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
        else if (hffield.Value == "SAVE")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y") hfCNote.Value = "Y";
            else hfCNote.Value = "N";
            DataTable dtn = new DataTable();
            dtn = (DataTable)ViewState["dtn"];
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);
            fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
            btnval = hffield.Value;
            switch (btnval)
            {
                case "List":
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
                    break;
                case "New":
                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    disablectrl();
                    fgen.EnableForm(this.Controls);
                    // Popup asking for Copy from Older Data
                    fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
                    hffield.Value = "NEW_E";
                    #endregion
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
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,b.Name as TM_Name,c.Name as CL_Name,d.name as Ef_Name from " + frm_tabname + " a,type b,type c,type d where b.id2='TM' and c.id2='CL' and d.id2='TS' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.type1) and trim(a.wcode)=trim(d.type1) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                        dt.Dispose();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;
                case "TACODE":
                    txtacode.Value = col1;
                    txtAname.Value = col2;
                    break;
                case "TRCODE":
                    txtRcode.Value = col1;
                    Text2.Value = col2;
                    break;
                case "DNCN":
                    txtDnCnCode.Value = col1;
                    txtDnCnName.Value = col2;
                    btnGstClass.Focus();
                    break;
                case "GSTCLASS":
                    txtGstClassCode.Value = col1;
                    txtGstClassName.Value = col2;
                    txtGstClassName.Focus();
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            SQuery = "SELECT a.vchnum as entryno,to_char(a.vchdate,'dd/mm/yyyy') as entrydt,a.col1 as invno,a.col2 as invdt,b.aname as customer,c.cpartno as partno,c.iname as part_name,a.col6 as qty_sold,a.col7 as old_rate,a.col8 as rev_rate,a.col9 as diff,a.col10 as diffval,a.col11 as pono,a.col12 as podt,a.col13 check_sheet_no FROM SCRATCH2 A,famst b,item c WHERE trim(a.acode)=trim(B.acode) and trim(A.icode)=trim(c.icode) and A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + DateRange + " ORDER BY A.COL33";

            SQuery = "select DISTINCT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL11 AS INVNO,A.COL12 AS INV_DATE,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,a.DIFF AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,a.TOTAL AS TOTAL,A.COL29 AS HSCODE,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code from (SELECT distinct a.acode,a.vchdate,a.icode,A.COL33,A.COL34 ,A.COL35 ,A.COL2 ,A.COL1 ,A.COL3 ,A.COL11 ,A.COL12 ,A.COL13 ,A.COL14 ,A.COL16 ,A.COL26 ,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 ,A.COL18 ,A.COL19 ,A.COL20 ,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 FROM SCRATCH2 A  WHERE a.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and a.num10>0 ) a, ivoucher b where TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL11)||TO_CHAR(TO_DATE(A.COL12,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(b.location)||to_char(b.vchdate,'dd/mm/yyyy') and b.type in ('58','59') order by a.col33";
            SQuery = "SELECT distinct A.COL33 AS BATCH_NO,A.COL4 AS PART_NO,A.COL5 AS PART_NAME,A.COL11 AS PO_NO,A.COL1 AS INVNO,A.COL2 AS INV_DATE ,A.COL6 AS QTY,A.COL7 AS OLD_RATE,A.COL8 AS NEW_RATE,(TO_NUMBER(A.COL7)-TO_NUMBER(A.COL8)) AS DIFF,a.col13 check_sheet_no,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code FROM SCRATCH2 A,ivoucher B WHERE a.BRANCHCD||TRIM(a.ACODE)||TRIM(A.ICODE)||TRIM(A.COL1)||TRIM(A.COL33)=B.BRANCHCD||TRIM(B.ACODE)||TRIM(B.ICODe)||trim(b.invno)||TRIM(B.LOCATION) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND a.vchdate " + PrdRange + " and b.type in ('58','59') and a.num10>0 order by a.col33 ";
            SQuery = "SELECT distinct a.acode,c.aname as customer,A.COL4 AS PART_NO,A.COL5 AS PART_NAME,A.COL1 AS INVNO,A.COL2 AS INV_DATE ,A.COL6 AS QTY,a.col7 as old_rate,A.COL8 AS NEW_RATE,(TO_NUMBER(A.COL7)-TO_NUMBER(A.COL8)) AS DIFF,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code FROM SCRATCH2 A,ivoucher B,famst c WHERE a.BRANCHCD||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)||trim(a.col1)||trim(A.col2)||a.col6=trim(b.btchno)||trim(B.acode)||trim(b.icode)||trim(b.invno)||to_char(b.invdate,'dd/mm/yyyy')||trim(b.iqty_chl) and trim(a.acode)=trim(C.acodE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND a.vchdate " + PrdRange + " and b.type in ('58','59') order by a.col1 ";
            //is qry me dr note no or cr note no me lagana hoga agar gstvchno>8 ho to gstvchno else mbr+type+vchnum
            // SQuery = "SELECT distinct a.acode,c.aname as customer,A.COL4 AS PART_NO,A.COL5 AS PART_NAME,A.COL1 AS INVNO,A.COL2 AS INV_DATE ,A.COL6 AS QTY,d.irate as old_rate,(TO_NUMBER(d.irate)+TO_NUMBER(A.COL8)) AS NEW_RATE,(TO_NUMBER(A.COL8)) AS DIFF,(CASE WHEN B.TYPE='59' THEN b.branchcd||b.type||'-'||trim(B.VCHNUM) END) as dr_note,(CASE WHEN B.TYPE='58' THEN b.branchcd||b.type||'-'||trim(B.VCHNUM) END) AS CR_NOTE, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code FROM SCRATCH2 A,ivoucher B,famst c,ivoucher d WHERE a.BRANCHCD||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)||trim(a.col1)||trim(A.col2)||a.col6=trim(b.btchno)||trim(B.acode)||trim(b.icode)||trim(b.invno)||to_char(b.invdate,'dd/mm/yyyy')||trim(b.iqty_chl) and trim(b.branchcd)||trim(b.invno)||to_char(b.invdate,'dd/mm/yyyy')||trim(b.icode)=trim(d.branchcd)||(case when length(trim(d.invno)) > 8 then trim(d.invno) else  trim(D.vchnum) end )||to_char(d.vchdate,'dd/mm/yyyy')||trim(d.icode) and d.type like '4%' and trim(a.acode)=trim(C.acodE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND a.vchdate " + PrdRange + " and b.type in ('58','59') order by a.col1 "; //old...22may2021
            SQuery = "SELECT distinct a.acode,c.aname as customer,A.COL4 AS PART_NO,A.COL5 AS PART_NAME,A.COL1 AS INVNO,A.COL2 AS INV_DATE ,A.COL6 AS QTY,d.irate as old_rate,(TO_NUMBER(d.irate)+TO_NUMBER(A.COL8)) AS NEW_RATE,(TO_NUMBER(A.COL8)) AS DIFF,(CASE WHEN B.TYPE='59' THEN trim(b.gstvch_no) END) as dr_note,(CASE WHEN B.TYPE='58' THEN trim(b.gstvch_no) END) AS CR_NOTE, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code FROM SCRATCH2 A,ivoucher B,famst c,ivoucher d WHERE a.BRANCHCD||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)||trim(a.icode)||trim(a.col1)||trim(A.col2)||a.col6=trim(b.btchno)||trim(B.acode)||trim(b.icode)||trim(b.invno)||to_char(b.invdate,'dd/mm/yyyy')||trim(b.iqty_chl) and trim(b.branchcd)||trim(b.invno)||to_char(b.invdate,'dd/mm/yyyy')||trim(b.icode)=trim(d.branchcd)||(case when length(trim(d.invno)) > 8 then trim(d.invno) else  trim(D.vchnum) end )||to_char(d.vchdate,'dd/mm/yyyy')||trim(d.icode) and d.type like '4%' and trim(a.acode)=trim(C.acodE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND a.vchdate " + PrdRange + " and b.type in ('58','59') order by a.col1 ";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevelJS("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "ANEXX") //this is summary debit notecredit note list
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select branchcd,type,vchnum,to_Char(vchdate,'dd/mm/yyyy') as vchdate,acode,aname as customer,FDDR1,FADDR2,faddr3,fstate,fstatecode,fgirno as pan_no,vencode,fgst_no as gst_no,iname as Material,cpartno as Material_Description,iunit,hscode,icode,iopr,sum(iqty_chl) as iqty_chl, purpose,desc_,naration,'-' AS ponum,NULL AS podate,'-' as invno,sysdate as invdate,sum(iamount) as iamount,0 as irate,'-' AS finvno,to_Char(exc_57f4dt,'dd/mm/yyyy') as exc_57f4dt,exc_rate ,sum(exc_amt) as exc_amt,refnum,iexc_addl,sum(cess_pu)  as cess_pu,cess_percent,sum(spexc_rate) as spexc_rate,sum(spexc_amt) as spexc_amt,sum(psize) as tcsamt,btchdt,form31,mfgdt,expdt from(SELECT (case when upper(A.naration) like 'SUPPL%' or substr(f.aname,1,4)='HERO' OR substr(f.aname,1,4)='TATA' then 'SUPPLEMENTARY INVOICE / DEBIT NOTE' ELSE 'DEBIT NOTE' end) as header,(CASE WHEN TRIM(NVL(F.PNAME,'-'))='-' THEN F.ANAME ELSE F.PNAME END) AS ANAME,F.ADDR1 AS FDDR1,F.ADDR2 AS FADDR2,F.ADDR3 AS FADDR3,F.STATEN AS FSTATE,SUBSTR(F.GST_NO,0,2) AS FSTATECODE,F.GIRNO AS FGIRNO,F.VENCODE,F.GST_NO AS FGST_NO,(CASE WHEN LENGTH(A.PURPOSE)>2 THEN A.PURPOSE ELSE I.INAME END) AS INAME,I.cpartno,I.UNIT AS IUNIT,I.HSCODE,A.* FROM IVOUCHER A,FAMST F ,ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='59' and a.vchdate  " + PrdRange + "  order by a.vchnum ) group by header,aname,FDDR1,faddr2,faddr3,fstate,fstatecode,fgirno,vencode,fgst_no,iname,cpartno,iunit,hscode,branchcd,type,vchnum,vchdate,acode,icode,iopr, purpose,desc_,naration,exc_57f4dt,exc_rate,refnum,iexc_addl,cess_percent,btchdt,form31,mfgdt,expdt";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevelJS("Annexure of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
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
                last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and " + doc_df.Value + " " + DateRange + " ", "ldt");
                if (last_entdt == "0")
                { }
                else
                {
                    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                    }
                }
            }

            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N";
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
                            save_it = "N";
                            save_it = "Y";

                            if (save_it == "Y")
                            {
                                i = 0;
                                do
                                {
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                    if (i > 20)
                                    {
                                        fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                                        pk_error = "N";
                                        i = 0;
                                    }
                                    i++;
                                }
                                while (pk_error == "Y");
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        ViewState["refNo"] = frm_vnum;
                        save_fun();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        save_fun2();

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", "Data Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "vipin@Tejaxo.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", "Data Saved Successfully");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
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
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
    }
    protected void btnlbl10_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl11_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl12_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl19_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl20_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl21_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl22_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnlbl23_Click(object sender, ImageClickEventArgs e)
    {

    }


    //------------------------------------------------------------------------------------
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        DataTable dtW = (DataTable)ViewState["dtn"];
        if (dtW != null)
        {
            DataView dvW = new DataView(dtW);
            dvW.Sort = "icode";
            dtW = new DataTable();
            dtW = dvW.ToTable();

            foreach (DataRow gr1 in dtW.Rows)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();
                oporow["ICODE"] = gr1["icode"].ToString().Trim();
                oporow["ACODE"] = txtacode.Value.Trim();

                oporow["col1"] = gr1["invno"].ToString().Trim();
                oporow["col2"] = gr1["invdt"].ToString().Trim();
                oporow["col3"] = gr1["icode"].ToString().Trim();
                oporow["col4"] = gr1["cpartno"].ToString().Trim();
                oporow["col5"] = gr1["iname"].ToString().Trim();
                oporow["col6"] = gr1["iqtyout"].ToString().Trim();
                oporow["col7"] = gr1["oldrate"].ToString().Trim();
                oporow["col8"] = gr1["rrate"].ToString().Trim();
                oporow["col9"] = gr1["diff"].ToString().Trim();
                oporow["col10"] = gr1["diffval"].ToString().Trim();
                oporow["col11"] = gr1["pono"].ToString().Trim();
                oporow["col12"] = gr1["podt"].ToString().Trim();
                //oporow["col13"] = gr1["sheetno"].ToString().Trim();

                oporow["col14"] = gr1["cgst"].ToString().Trim();
                oporow["col15"] = gr1["sgst"].ToString().Trim();
                oporow["col16"] = gr1["igst"].ToString().Trim();
                oporow["col17"] = gr1["hscode"].ToString().Trim();

                oporow["col33"] = gr1["pono"].ToString().Trim();

                oporow["col46"] = txtDnCnCode.Value.Trim();
                oporow["col47"] = txtGstClassCode.Value.Trim();

                oporow["num1"] = (fgen.make_double(txtCgst.Value.Trim()) > 0 ? fgen.make_double(txtCgst.Value.Trim()) : gr1["cgst"].ToString().Trim().toDouble());
                oporow["num2"] = (fgen.make_double(txtSgst.Value.Trim()) > 0 ? fgen.make_double(txtSgst.Value.Trim()) : gr1["sgst"].ToString().Trim().toDouble());

                if (hfCNote.Value == "Y") oporow["NUM10"] = 1;
                else
                {
                    if (fgen.make_double(gr1["rrate"].ToString().Trim()) > 0) oporow["NUM10"] = 1;
                    else oporow["NUM10"] = 0;
                }

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
            }
        }
    }

    void save_fun2()
    {
        string sal_code = "", par_code = "", tax_code = "", tax_code2 = "", schg_code = "", iopr = ""; string status = ""; string tcscode = "";
        double dVal = 0; double dVal1 = 0; double dVal2 = 0; double qty = 0; double basic = 0; double gstval = 0; double tcsrate = 0; double tcsamt = 0;
        string invoiceWise = "N"; string vinvno = ""; string vinvdt = ""; string newVnum = ""; string batchNo = ""; string multiinv_vnum = ""; string branchcd = ""; string invRmrk = "";
        string mhd = "";
        string Vgstno_cntrl = "";//CONTROL FOR Long Voucher Number for 58/59 series vouchers.
        string Vgstno_paramdt = "";
        string Vgstvch_no = "";//var for saving gstvch_no in both table
        string Saving_vch_ivch = ""; string saving_inv = "";
        double dValTot = 0;
        double dVal1Tot = 0;
        double dVal2Tot = 0;
        int srnoCounter = 1;
        string saveTo = "Y";
        int l = 1;
        DataTable dtparty = new DataTable();
        DataTable dtSale = new DataTable();
        DataView dv = new DataView();
        dtSale = fgen.getdata(frm_qstr, frm_cocd, "SELECT distinct branchcd,(CASE WHEN TRIM(NVL(FULL_INVNO,'-'))='-' THEN TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY') ELSE TRIM(FULL_INVNO)||TO_cHAR(VCHDATE,'DD/MM/YYYY') END) AS FSTR,BRANCHCD||TO_CHAR(VCHDATE,'YYYYMMDD')||VCHNUM AS FSTR2 FROM SALE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '4%' AND VCHDATE >=TO_DATE('01/04/2016','DD/MM/YYYY') AND TRIM(ACODE)='" + txtacode.Value + "' order by FSTR2 ");
        DataTable dtW = (DataTable)ViewState["dtn"];
        if (dtW != null)
        {
            DataView dvW = new DataView(dtW);
            //dvW.Sort = "icode";
            dtW = new DataTable();
            dtW = dvW.ToTable();
            dtparty = fgen.getdata(frm_qstr, frm_cocd, "select trim(Acode) as acode,nvl(status,'-') as status,nvl(CESSRATE,0) as tcsrate from famst where substr(trim(Acode),1,2)='16' order by acode asc");
            multiinv_vnum = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ENABLE_YN FROM CONTROLS WHERE ID='O43'", "ENABLE_YN");
            Vgstno_cntrl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_lvch_5859");
            Vgstno_paramdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_lvch_5859_date");
            Saving_vch_ivch = fgen.seek_iname(frm_qstr, frm_cocd, "select ENABLE_YN from stock where id='M338'", "ENABLE_YN");//////////control for saving invno,gstvchno,originv_no,originv_dt in ivch and vch table
            // Saving_vch_ivch = "Y";//CMNT THIS...THIS IS FOR TESTING
            //========================
            #region Complete Save Function
            dv = new DataView(dtW, "", "", DataViewRowState.CurrentRows);
            dt = new DataTable();
            dt = dv.ToTable(true, "invno", "invdt");
            int multicont = 1;
            if (multiinv_vnum == "Y")
            {//============this saving for multiple inv on single party
                oDS1 = new DataSet();
                oporow1 = null;
                oDS1 = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");
                foreach (DataRow dr in dt.Rows)
                {
                    #region
                    dt2 = new DataTable();
                    dv = new DataView(dtW, "invno='" + dr["invno"].ToString().Trim() + "' and invdt='" + dr["invdt"].ToString().Trim() + "'", "icode", DataViewRowState.CurrentRows);
                    dt3 = new DataTable();
                    dt3 = dv.ToTable();
                    if (dr["invno"].ToString().Trim().Length > 6) vinvno = dr["invno"].ToString().Trim();
                    else vinvno = fgen.padlc(Convert.ToInt32(dr["invno"].ToString().Trim()), 6);
                    vinvdt = Convert.ToDateTime(dr["invdt"].ToString().Trim()).ToString("dd/MM/yyyy");
                    newVnum = "Y";
                    invoiceWise = "N";
                    branchcd = mhd;
                    if (frm_cocd == "BONY" || frm_cocd == "SFAB" || frm_cocd == "ARVI" || frm_cocd == "NIRM" || frm_cocd == "PRAG" || frm_cocd == "RRP" || frm_cocd == "SDM" || frm_cocd == "PPPF" || frm_cocd == "PPPL" || frm_cocd == "PIPL" || frm_cocd == "SFLG" || frm_cocd == "SFL2" || frm_cocd == "SFL1" || frm_cocd == "SAIL") invoiceWise = "Y";
                    else invoiceWise = "N";
                    foreach (DataRow drw in dt3.Rows)
                    {
                        #region
                        saveTo = "Y";
                        if (saveTo == "Y")
                        {
                            string myinv = drw["invno"].ToString().Trim();
                            if (myinv.Length < 6) myinv = fgen.padlc(Convert.ToInt32(drw["invno"].ToString().Trim()), 6).ToString();
                            mhd = fgen.seek_iname_dt(dtSale, "fstr='" + myinv + Convert.ToDateTime(drw["invdt"].ToString().Trim()).ToString("dd/MM/yyyy") + "'", "branchcd");
                            if (mhd != "0")
                            {
                                branchcd = frm_mbr; ;
                                invRmrk = "";
                                dVal = 0;
                                dVal1 = 0;
                                dVal2 = 0;
                                //*******************                            
                                oporow1 = oDS1.Tables[0].NewRow();
                                oporow1["BRANCHCD"] = branchcd;

                                if (fgen.make_double(drw["rrate"].ToString().Trim()) > 0) nVty = "59";
                                else nVty = "58";
                                // nVty = "58";//for testing

                                oporow1["TYPE"] = nVty;
                                if (multicont == 1) frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, "IVOUCHER", "VCHNUM", "VCHDATE", branchcd, nVty, txtvchdate.Text.Trim(), frm_uname, frm_formID);
                                else newVnum = "N";

                                batchNo = drw["pono"].ToString().Trim();
                                oporow1["LOCATION"] = batchNo;
                                oporow1["vchnum"] = frm_vnum;
                                oporow1["vchdate"] = txtvchdate.Text.Trim();

                                oporow1["ACODE"] = txtacode.Value.Trim();

                                status = fgen.seek_iname_dt(dtparty, "acode='" + txtacode.Value.Trim() + "'", "status");
                                tcsrate = fgen.make_double(fgen.seek_iname_dt(dtparty, "acode='" + txtacode.Value.Trim() + "'", "tcsrate"));


                                oporow1["VCODE"] = txtacode.Value.ToString().Trim();
                                oporow1["ICODE"] = drw["icode"].ToString().Trim();
                                oporow1["srno"] = multicont;
                                oporow1["REC_ISS"] = "C";

                                oporow1["IQTYIN"] = 0;
                                oporow1["IQTYOUT"] = 0;

                                oporow1["IQTY_CHL"] = drw["iqtyout"].ToString().Trim();
                                qty = fgen.make_double(drw["iqtyout"].ToString().Trim());
                                oporow1["PURPOSE"] = drw["iname"].ToString().Trim();

                                invRmrk = "PO No. :" + batchNo;
                                invRmrk = drw["remarks"].ToString().Trim() + " " + txtrmk.Text.Trim();
                                oporow1["NARATION"] = invRmrk;

                                oporow1["finvno"] = drw["PONO"].ToString().Trim();
                                oporow1["PODATE"] = fgen.make_def_Date(drw["PODT"].ToString().Trim(), vardate);
                                //==========
                                if (Vgstno_cntrl == "Y" && Vgstno_paramdt.Trim().Length >= 10)
                                {
                                    if (Convert.ToDateTime(txtvchdate.Text.Trim()) >= Convert.ToDateTime(Vgstno_paramdt.ToString().Trim()))
                                    {
                                        //if vchdate>=Vgstno_paramdt then it will save else save '-' ......
                                        Vgstvch_no = frm_CDT1.Substring(8, 2) + frm_mbr + nVty + "-" + frm_vnum;
                                    }
                                }
                                else
                                {
                                    Vgstvch_no = frm_mbr + nVty + frm_vnum;
                                }
                                #region this saving as per MG Mam on control base M338..IF IT Y then saving diff...if it is N then saving diff in ivch and vch table
                                if (Saving_vch_ivch == "Y")
                                {
                                    if (nVty == "59" || nVty == "58")
                                    {
                                        if (drw["invno"].ToString().Trim().Length > 6)
                                            oporow1["originv_no"] = drw["invno"].ToString().Trim();
                                        else oporow1["INVNO"] = fgen.padlc(Convert.ToInt32(drw["invno"].ToString().Trim()), 6);
                                        oporow1["INVDATE"] = Convert.ToDateTime(drw["invdt"].ToString().Trim()).ToString("dd/MM/yyyy");
                                        oporow1["GSTVCH_NO"] = Vgstvch_no.Trim();

                                    }
                                }
                                else///when control in N....m338 control no need for this
                                {
                                    if (nVty == "59" || nVty == "58")
                                    {
                                        if (drw["invno"].ToString().Trim().Length > 6)
                                            oporow1["originv_no"] = drw["invno"].ToString().Trim();
                                        else oporow1["INVNO"] = fgen.padlc(Convert.ToInt32(drw["invno"].ToString().Trim()), 6);
                                        oporow1["INVDATE"] = Convert.ToDateTime(drw["invdt"].ToString().Trim()).ToString("dd/MM/yyyy");
                                        oporow1["GSTVCH_NO"] = Vgstvch_no.Trim();
                                    }
                                }
                                saving_inv = oporow1["INVNO"].ToString().Trim();

                                #endregion

                                #region//============OLD inv SAVING
                                //if (drw["invno"].ToString().Trim().Length > 6)
                                //    oporow1["INVNO"] = drw["invno"].ToString().Trim();
                                //else oporow1["INVNO"] = fgen.padlc(Convert.ToInt32(drw["invno"].ToString().Trim()), 6);
                                //oporow1["INVDATE"] = Convert.ToDateTime(drw["invdt"].ToString().Trim()).ToString("dd/MM/yyyy");
                                #endregion

                                oporow1["UNIT"] = "NOS";

                                double Rate = fgen.make_double(drw["rrate"].ToString().Trim(), 2);
                                if (Rate < 0) Rate = -1 * Rate;
                                oporow1["IRATE"] = Rate;

                                //OLD RATE + " ~ " + NEW RATE
                                oporow1["PNAME"] = fgen.make_double(drw["oldrate"].ToString().Trim(), 2) + "~" + fgen.make_double(drw["diff"].ToString().Trim(), 2);

                                dVal = Math.Round(fgen.make_double(drw["iqtyout"].ToString().Trim()) * Rate, 2);
                                if (dVal < 0) dVal = -1 * dVal;
                                oporow1["IAMOUNT"] = dVal;
                                dValTot += dVal;
                                //------for inv wise baisc value
                                basic += dVal;

                                oporow1["NO_CASES"] = drw["hscode"].ToString().Trim();
                                oporow1["EXC_57F4"] = drw["cpartno"].ToString().Trim();

                                if (addGrNo == "Y")
                                {
                                    oporow1["REFNUM"] = drw["GRNO"].ToString().Trim();
                                    oporow1["EXC_57F4DT"] = fgen.make_def_Date(drw["GRDT"].ToString().Trim(), vardate);
                                }
                                else
                                {
                                    oporow1["REFNUM"] = "-";
                                    oporow1["EXC_57F4DT"] = vardate;
                                }

                                if (fgen.make_double(drw["IGST"].ToString().Trim()) > 0)
                                {
                                    oporow1["IOPR"] = "IG";
                                    iopr = "IG";
                                    double igst = txtCgst.Value.ToString().toDouble();
                                    if (igst <= 0) igst = drw["igst"].ToString().Trim().toDouble();
                                    oporow1["EXC_RATE"] = igst;
                                    dVal1 = Math.Round(dVal * (igst / 100), 2);

                                    if (invoiceWise == "Y") dVal1Tot += Math.Round(dVal1, 2);
                                    else dVal1Tot = dVal1;
                                    dVal1Tot = Math.Round(dVal1Tot, 2);
                                    oporow1["EXC_AMT"] = Math.Round(dVal1, 2);
                                }
                                else
                                {
                                    iopr = "CG";
                                    oporow1["IOPR"] = "CG";
                                    double cgst = txtCgst.Value.ToString().toDouble();
                                    if (cgst <= 0) cgst = drw["cgst"].ToString().Trim().toDouble();
                                    oporow1["EXC_RATE"] = cgst;
                                    dVal1 = Math.Round(dVal * (cgst / 100), 2);

                                    if (invoiceWise == "Y") dVal1Tot += Math.Round(dVal1, 2);
                                    else dVal1Tot = dVal1;
                                    dVal1Tot = Math.Round(dVal1Tot, 2);
                                    oporow1["EXC_AMT"] = Math.Round(dVal1, 2);

                                    double sgst = txtSgst.Value.ToString().toDouble();
                                    if (sgst <= 0) sgst = drw["sgst"].ToString().Trim().toDouble();
                                    oporow1["CESS_PERCENT"] = sgst;
                                    dVal2 = Math.Round(dVal * (sgst / 100), 2);

                                    if (invoiceWise == "Y") dVal2Tot += Math.Round(dVal2, 2);
                                    else dVal2Tot = dVal2;
                                    dVal2Tot = Math.Round(dVal2Tot, 2);
                                    oporow1["CESS_PU"] = Math.Round(dVal2, 2);
                                }
                                //---------gst total
                                gstval += Math.Round(dVal1, 2) + Math.Round(dVal2, 2);//exc_amt+cess_pu                              
                                //--------------
                                oporow1["STORE"] = "N";
                                oporow1["MORDER"] = multicont;//srnoCounter;
                                //oporow1["SPEXC_RATE"] = dVal;
                                oporow1["SPEXC_RATE"] = 0;
                                //oporow1["SPEXC_AMT"] = dVal + dVal1 + dVal2;
                                oporow1["SPEXC_AMT"] = 0;
                                oporow1["psize"] = 0;
                                oporow1["gsm"] = 0;

                                if (iopr == "CG")
                                {
                                    if (tax_code.Length <= 0)
                                    {
                                        tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A77'", "PARAMS");
                                        sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A77'", "PARAMS2");
                                        tax_code2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A78'", "PARAMS");
                                    }
                                }
                                else
                                {
                                    if (tax_code.Length <= 0)
                                    {
                                        tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A79'", "PARAMS");
                                        sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A79'", "PARAMS2");
                                    }
                                }
                                if (schg_code.Length <= 0)
                                    schg_code = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(params) as param from controls where id='A41'", "param");

                                if (txtRcode.Value.Trim().Length > 2) sal_code = txtRcode.Value.Trim();

                                oporow1["RCODE"] = sal_code;

                                oporow1["MATTYPE"] = txtGstClassCode.Value;
                                oporow1["POTYPE"] = txtDnCnCode.Value;

                                oporow1["btchno"] = frm_mbr + ViewState["refNo"].ToString() + txtvchdate.Text.Trim();

                                if (edmode.Value == "Y")
                                {
                                    oporow1["eNt_by"] = ViewState["entby"].ToString();
                                    oporow1["eNt_dt"] = ViewState["entdt"].ToString();
                                    oporow1["edt_by"] = frm_uname;
                                    oporow1["edt_dt"] = vardate;
                                }
                                else
                                {
                                    oporow1["eNt_by"] = frm_uname;
                                    oporow1["eNt_dt"] = vardate;
                                    oporow1["edt_by"] = "-";
                                    oporow1["eDt_dt"] = vardate;
                                }

                                oDS1.Tables[0].Rows.Add(oporow1);
                                l++;
                                multicont++;
                            }
                        }
                        #endregion
                    }

                    //*******************
                    par_code = txtacode.Value.Trim();
                    //***********************
                    batchNo = "W" + batchNo;
                }//--------------end foreach loop
                // if (chktcs.Checked == true)  //THIS condition for when tcs is selected only then tcs should cal else no
                if (chktcs.Checked == true)
                {
                    tcsamt += (basic + gstval) * tcsrate / 100;
                    oDS1.Tables[0].Rows[0]["gsm"] = tcsrate;
                    oDS1.Tables[0].Rows[0]["psize"] = tcsamt;
                    oDS1.Tables[0].Rows[0]["spexc_amt"] = basic + gstval + tcsamt;
                }
                else
                {
                    tcsamt += 0;
                    oDS1.Tables[0].Rows[0]["gsm"] = 0;
                    oDS1.Tables[0].Rows[0]["psize"] = 0;
                    oDS1.Tables[0].Rows[0]["spexc_amt"] = basic + gstval;
                }

                oDS1.Tables[0].Rows[0]["spexc_rate"] = basic;
                //=============
                if (status == "Y")
                {
                    tcscode = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A95'", "PARAMS");
                }

                if (branchcd != null)
                {
                    //if (branchcd.Length > 1)
                    //{
                    //if (Saving_vch_ivch == "Y")
                    //{
                    branchcd = frm_mbr;
                    #region Voucher Saving ..
                    int crsrno = 50, drsrno = 1;
                    if (nVty == "58")
                    {
                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), drsrno, sal_code, par_code, fgen.make_double(dValTot, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                        drsrno++;
                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), drsrno, tax_code, par_code, fgen.make_double(dVal1Tot, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));

                        if (tax_code2.Length > 0)
                        {
                            drsrno++;
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), drsrno, tax_code2, par_code, fgen.make_double(dVal2Tot, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                        }
                        //fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, par_code, sal_code, 0, fgen.make_double(dValTot + dVal1Tot + dVal2Tot + tcsamt, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), crsrno, par_code, sal_code, 0, fgen.make_double(basic + dVal1Tot + dVal2Tot + tcsamt, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                        //----for tcs saving in voucher
                        if (chktcs.Checked == true)
                        {
                            drsrno++;
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), drsrno, tcscode, par_code, tcsamt, 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                        }
                    }
                    else
                    {///59
                        //fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, sal_code, fgen.make_double(dValTot + dVal1Tot + dVal2Tot + tcsamt, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), drsrno, par_code, sal_code, fgen.make_double(basic + dVal1Tot + dVal2Tot + tcsamt, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), crsrno, sal_code, par_code, 0, fgen.make_double(dValTot, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                        crsrno++;
                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), crsrno, tax_code, par_code, 0, fgen.make_double(dVal1Tot, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));

                        if (tax_code2.Length > 0)
                        {
                            crsrno++;
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), crsrno, tax_code2, par_code, 0, fgen.make_double(dVal2Tot, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                        }
                        //---for tcs saving in voucher
                        if (chktcs.Checked == true)
                        {
                            crsrno++;
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), crsrno, tcscode, par_code, 0, tcsamt, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                        }
                    }
                    #endregion
                    //  }                   
                    // }
                }

                if (oDS1 != null && oDS1.Tables[0].Rows.Count > 0)
                {
                    // oDS1.Tables[0].Rows[0]["SPEXC_AMT"] = fgen.make_double(dValTot + dVal1Tot + dVal2Tot, 2);
                    fgen.save_data(frm_qstr, frm_cocd, oDS1, "IVOUCHER");
                }
                    #endregion
            }
            //  }
            // newVnum = "Y";

            //////////===================else me old wala saving function hai
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    #region
                    dt2 = new DataTable();
                    dv = new DataView(dtW, "invno='" + dr["invno"].ToString().Trim() + "' and invdt='" + dr["invdt"].ToString().Trim() + "'", "icode", DataViewRowState.CurrentRows);
                    dt3 = new DataTable();
                    dt3 = dv.ToTable();
                    if (dr["invno"].ToString().Trim().Length > 6) vinvno = dr["invno"].ToString().Trim();
                    else vinvno = fgen.padlc(Convert.ToInt32(dr["invno"].ToString().Trim()), 6);
                    vinvdt = Convert.ToDateTime(dr["invdt"].ToString().Trim()).ToString("dd/MM/yyyy");
                    newVnum = "Y";
                    invoiceWise = "N";
                    branchcd = mhd;

                    if (frm_cocd == "BONY" || frm_cocd == "SFAB" || frm_cocd == "ARVI" || frm_cocd == "NIRM" || frm_cocd == "PRAG" || frm_cocd == "RRP" || frm_cocd == "SDM" || frm_cocd == "PPPF" || frm_cocd == "PPPL" || frm_cocd == "PIPL" || frm_cocd == "SFLG" || frm_cocd == "SFL2" || frm_cocd == "SFL1" || frm_cocd == "SAIL" || frm_cocd == "IPP" || frm_cocd == "ATOP") invoiceWise = "Y";//atop add as entry not saving..yogita..24.06.2021
                    else invoiceWise = "N";

                    if (invoiceWise == "Y")
                    {
                        oDS1 = new DataSet();
                        oporow1 = null;
                        oDS1 = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");
                    }
                    srnoCounter = 1;
                    foreach (DataRow drw in dt3.Rows)
                    {
                        //---------------
                        if (invoiceWise == "N")
                        {
                            oDS1 = new DataSet();
                            oporow1 = null;
                            oDS1 = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");
                        }
                        saveTo = "Y";
                        if (saveTo == "Y")
                        {
                            string myinv = drw["invno"].ToString().Trim();
                            if (myinv.Length < 6) myinv = fgen.padlc(Convert.ToInt32(drw["invno"].ToString().Trim()), 6).ToString();
                            mhd = fgen.seek_iname_dt(dtSale, "fstr='" + myinv + Convert.ToDateTime(drw["invdt"].ToString().Trim()).ToString("dd/MM/yyyy") + "'", "branchcd");
                            if (mhd != "0")
                            {
                                branchcd = mhd;
                                invRmrk = "";
                                gstval = 0;
                                basic = 0;
                                dVal = 0;
                                dVal1 = 0;
                                dVal2 = 0;
                                dValTot = 0;
                                dVal1Tot = 0;
                                dVal2Tot = 0;
                                tcsamt = 0;

                                //*******************                            

                                oporow1 = oDS1.Tables[0].NewRow();
                                oporow1["BRANCHCD"] = branchcd;

                                if (fgen.make_double(drw["rrate"].ToString().Trim()) > 0) nVty = "59";
                                else nVty = "58";
                                //nVty = "59";
                                oporow1["TYPE"] = nVty;
                                if (newVnum == "Y")
                                {
                                    i = 0;
                                    frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, "IVOUCHER", "VCHNUM", "VCHDATE", branchcd, nVty, txtvchdate.Text.Trim(), frm_uname, frm_formID);
                                    newVnum = "N";
                                }

                                batchNo = drw["pono"].ToString().Trim();
                                oporow1["LOCATION"] = batchNo;
                                oporow1["vchnum"] = frm_vnum;
                                oporow1["vchdate"] = txtvchdate.Text.Trim();

                                oporow1["ACODE"] = txtacode.Value.Trim();

                                status = fgen.seek_iname_dt(dtparty, "acode='" + txtacode.Value.Trim() + "'", "status");
                                tcsrate = fgen.make_double(fgen.seek_iname_dt(dtparty, "acode='" + txtacode.Value.Trim() + "'", "tcsrate"));

                                if (chktcs.Checked == true)
                                {
                                    if (status == "Y")
                                    {
                                        oporow1["gsm"] = tcsrate;
                                    }
                                    else
                                    {
                                        oporow1["gsm"] = 0;
                                    }
                                }
                                else
                                {
                                    oporow1["gsm"] = 0;
                                }
                                oporow1["VCODE"] = txtacode.Value.ToString().Trim();
                                oporow1["ICODE"] = drw["icode"].ToString().Trim();

                                oporow1["REC_ISS"] = "C";

                                oporow1["IQTYIN"] = 0;
                                oporow1["IQTYOUT"] = 0;

                                oporow1["IQTY_CHL"] = drw["iqtyout"].ToString().Trim();
                                qty = fgen.make_double(drw["iqtyout"].ToString().Trim());
                                oporow1["PURPOSE"] = drw["iname"].ToString().Trim();

                                invRmrk = "PO No. :" + batchNo;
                                invRmrk = drw["remarks"].ToString().Trim() + " " + txtrmk.Text.Trim();
                                oporow1["NARATION"] = invRmrk;

                                oporow1["finvno"] = drw["PONO"].ToString().Trim();
                                oporow1["PODATE"] = fgen.make_def_Date(drw["PODT"].ToString().Trim(), vardate);

                                if (drw["invno"].ToString().Trim().Length > 6)
                                    oporow1["INVNO"] = drw["invno"].ToString().Trim();
                                else oporow1["INVNO"] = fgen.padlc(Convert.ToInt32(drw["invno"].ToString().Trim()), 6);
                                oporow1["INVDATE"] = Convert.ToDateTime(drw["invdt"].ToString().Trim()).ToString("dd/MM/yyyy");

                                oporow1["UNIT"] = "NOS";

                                double Rate = fgen.make_double(drw["rrate"].ToString().Trim(), 2);
                                if (Rate < 0) Rate = -1 * Rate;
                                oporow1["IRATE"] = Rate;

                                //OLD RATE + " ~ " + NEW RATE
                                oporow1["PNAME"] = fgen.make_double(drw["oldrate"].ToString().Trim(), 2) + "~" + fgen.make_double(drw["diff"].ToString().Trim(), 2);

                                dVal = Math.Round(fgen.make_double(drw["iqtyout"].ToString().Trim()) * Rate, 2);
                                if (dVal < 0) dVal = -1 * dVal;
                                oporow1["IAMOUNT"] = dVal;
                                if (invoiceWise == "Y") dValTot += dVal;
                                else dValTot = dVal;
                                //------for inv wise baisc value
                                if (invoiceWise == "Y") basic += dVal;
                                else basic = dVal;
                                //-------
                                oporow1["NO_CASES"] = drw["hscode"].ToString().Trim();
                                oporow1["EXC_57F4"] = drw["cpartno"].ToString().Trim();

                                if (addGrNo == "Y")
                                {
                                    oporow1["REFNUM"] = drw["GRNO"].ToString().Trim();
                                    oporow1["EXC_57F4DT"] = fgen.make_def_Date(drw["GRDT"].ToString().Trim(), vardate);
                                }
                                else
                                {
                                    oporow1["REFNUM"] = "-";
                                    oporow1["EXC_57F4DT"] = vardate;
                                }

                                if (fgen.make_double(drw["IGST"].ToString().Trim()) > 0)
                                {
                                    oporow1["IOPR"] = "IG";
                                    iopr = "IG";
                                    double igst = txtCgst.Value.ToString().toDouble();
                                    if (igst <= 0) igst = drw["igst"].ToString().Trim().toDouble();
                                    oporow1["EXC_RATE"] = igst;
                                    dVal1 = Math.Round(dVal * (igst / 100), 2);
                                    if (invoiceWise == "Y") dVal1Tot += Math.Round(dVal1, 2);
                                    else dVal1Tot = dVal1;
                                    dVal1Tot = Math.Round(dVal1Tot, 2);
                                    oporow1["EXC_AMT"] = Math.Round(dVal1, 2);
                                }
                                else
                                {
                                    iopr = "CG";
                                    oporow1["IOPR"] = "CG";
                                    double cgst = txtCgst.Value.ToString().toDouble();
                                    if (cgst <= 0) cgst = drw["cgst"].ToString().Trim().toDouble();
                                    oporow1["EXC_RATE"] = cgst;
                                    dVal1 = Math.Round(dVal * (cgst / 100), 2);

                                    if (invoiceWise == "Y") dVal1Tot += Math.Round(dVal1, 2);
                                    else dVal1Tot = dVal1;
                                    dVal1Tot = Math.Round(dVal1Tot, 2);
                                    oporow1["EXC_AMT"] = Math.Round(dVal1, 2);

                                    double sgst = txtSgst.Value.ToString().toDouble();
                                    if (sgst <= 0) sgst = drw["sgst"].ToString().Trim().toDouble();
                                    oporow1["CESS_PERCENT"] = sgst;
                                    dVal2 = Math.Round(dVal * (sgst / 100), 2);

                                    if (invoiceWise == "Y") dVal2Tot += Math.Round(dVal2, 2);
                                    else dVal2Tot = dVal2;
                                    dVal2Tot = Math.Round(dVal2Tot, 2);
                                    oporow1["CESS_PU"] = Math.Round(dVal2, 2);
                                }
                                //---------gst total
                                if (invoiceWise == "Y") gstval += Math.Round(dVal1, 2) + Math.Round(dVal2, 2);//exc_amt+cess_pu
                                else gstval = Math.Round((dVal1 + dVal2), 2);
                                if (chktcs.Checked == true)
                                {
                                    if (status == "Y")
                                    {
                                        tcsamt = (basic + gstval) * tcsrate / 100;//gt tot
                                        oporow1["PSIZE"] = tcsamt;
                                    }
                                    else
                                    {
                                        oporow1["PSIZE"] = 0;
                                    }
                                }
                                else
                                {
                                    oporow1["PSIZE"] = 0;
                                }
                                tcscode = "";
                                tcscode = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A95'", "PARAMS");

                                //--------------
                                oporow1["STORE"] = "N";
                                oporow1["MORDER"] = srnoCounter;
                                oporow1["SPEXC_RATE"] = dVal;
                                oporow1["SPEXC_AMT"] = 0;

                                if (iopr == "CG")
                                {
                                    if (tax_code.Length <= 0)
                                    {
                                        tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A77'", "PARAMS");
                                        sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A77'", "PARAMS2");
                                        tax_code2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A78'", "PARAMS");
                                    }
                                }
                                else
                                {
                                    if (tax_code.Length <= 0)
                                    {
                                        tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A79'", "PARAMS");
                                        sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A79'", "PARAMS2");
                                    }
                                }
                                if (schg_code.Length <= 0)
                                    schg_code = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(params) as param from controls where id='A41'", "param");

                                if (txtRcode.Value.Trim().Length > 2) sal_code = txtRcode.Value.Trim();

                                oporow1["RCODE"] = sal_code;

                                oporow1["MATTYPE"] = txtGstClassCode.Value;
                                oporow1["POTYPE"] = txtDnCnCode.Value;

                                oporow1["btchno"] = frm_mbr + ViewState["refNo"].ToString() + txtvchdate.Text.Trim();
                                //
                                if (Vgstno_cntrl == "Y" && Vgstno_paramdt.Trim().Length >= 10)
                                {
                                    if (Convert.ToDateTime(txtvchdate.Text.Trim()) >= Convert.ToDateTime(Vgstno_paramdt.ToString().Trim()))
                                    {
                                        //if vchdate>=Vgstno_paramdt then it will save else save '-' ......
                                        Vgstvch_no = frm_CDT1.Substring(8, 2) + frm_mbr + nVty + "-" + frm_vnum;
                                    }
                                }
                                else
                                {
                                    Vgstvch_no = frm_mbr + nVty + frm_vnum; ;
                                }
                                oporow1["GSTVCH_NO"] = Vgstvch_no.Trim();

                                //=======
                                if (edmode.Value == "Y")
                                {
                                    oporow1["eNt_by"] = ViewState["entby"].ToString();
                                    oporow1["eNt_dt"] = ViewState["entdt"].ToString();
                                    oporow1["edt_by"] = frm_uname;
                                    oporow1["edt_dt"] = vardate;
                                }
                                else
                                {
                                    oporow1["eNt_by"] = frm_uname;
                                    oporow1["eNt_dt"] = vardate;
                                    oporow1["edt_by"] = "-";
                                    oporow1["eDt_dt"] = vardate;
                                }
                                oDS1.Tables[0].Rows.Add(oporow1);

                                l++;
                            }
                        }
                        if (status == "Y")
                        {
                            tcscode = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A95'", "PARAMS");
                        }
                        //*******************
                        par_code = txtacode.Value.Trim();
                        //***********************
                        //  batchNo = "W" + batchNo;
                        batchNo = Vgstvch_no;
                    }
                    //------------               
                    if (invoiceWise == "Y")
                    {
                        if (branchcd != null)
                        {
                            if (branchcd.Length > 1)
                            {
                                int crsrno = 50, drsrno = 1;
                                #region Voucher Saving
                                if (nVty == "58")
                                {
                                    // fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, sal_code, par_code, fgen.make_double(dValTot, 2), 0, vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);//OLD
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), drsrno, sal_code, par_code, fgen.make_double(dValTot, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, vinvno,Convert.ToDateTime(vinvdt));
                                    drsrno++;
                                    //fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 2, tax_code, par_code, fgen.make_double(dVal1Tot, 2), 0, vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);//OLD
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), drsrno, tax_code, par_code, fgen.make_double(dVal1Tot, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdt));

                                    if (tax_code2.Length > 0)
                                    {
                                        //fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 3, tax_code2, par_code, fgen.make_double(dVal2Tot, 2), 0, vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);
                                        drsrno++;
                                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), drsrno, tax_code2, par_code, fgen.make_double(dVal2Tot, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdt));
                                    }
                                    //fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, par_code, sal_code, 0, fgen.make_double(dValTot + dVal1Tot + dVal2Tot, 2), vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);//old wthout tcs
                                    // fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, par_code, sal_code, 0, fgen.make_double(dValTot + dVal1Tot + dVal2Tot + tcsamt, 2), vinvno, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);//OLD
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), crsrno, par_code, sal_code, 0, fgen.make_double(dValTot + dVal1Tot + dVal2Tot + tcsamt, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdt));

                                    if (chktcs.Checked == true)
                                    {
                                        //fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 4, tcscode, par_code, tcsamt, 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));//OLD
                                        drsrno++;
                                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), drsrno, tcscode, par_code, tcsamt, 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdt));
                                    }
                                }
                                else
                                {
                                    //fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, sal_code, fgen.make_double(dValTot + dVal1Tot + dVal2Tot + tcsamt, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);//OLD
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), drsrno, par_code, sal_code, fgen.make_double(dValTot + dVal1Tot + dVal2Tot + tcsamt, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdt));

                                    //fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, sal_code, par_code, 0, fgen.make_double(dValTot, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);//OLD
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), crsrno, sal_code, par_code, 0, fgen.make_double(dValTot, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdt));
                                    crsrno++;
                                    // fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 51, tax_code, par_code, 0, fgen.make_double(dVal1Tot, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), crsrno, tax_code, par_code, 0, fgen.make_double(dVal1Tot, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdt));

                                    if (tax_code2.Length > 0)
                                    {
                                        crsrno++;
                                        // fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 52, tax_code2, par_code, 0, fgen.make_double(dVal2Tot, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdt), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value);//OLD
                                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), crsrno, tax_code2, par_code, 0, fgen.make_double(dVal2Tot, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdt));
                                    }
                                    if (chktcs.Checked == true)
                                    {
                                        crsrno++;
                                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), crsrno, tcscode, par_code, 0, tcsamt, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdt));
                                    }
                                }
                                #endregion
                            }
                        }

                        if (oDS1 != null && oDS1.Tables[0].Rows.Count > 0)
                        {
                            if (chktcs.Checked == true)
                            {
                                oDS1.Tables[0].Rows[0]["SPEXC_AMT"] = fgen.make_double(dValTot + dVal1Tot + dVal2Tot + tcsamt, 2);
                                fgen.save_data(frm_qstr, frm_cocd, oDS1, "IVOUCHER");
                            }
                            else
                            {
                                oDS1.Tables[0].Rows[0]["SPEXC_AMT"] = fgen.make_double(dValTot + dVal1Tot + dVal2Tot, 2);
                                fgen.save_data(frm_qstr, frm_cocd, oDS1, "IVOUCHER");
                            }
                        }
                }
                    newVnum = "Y";
                    #endregion
                }
            }
        }
            #endregion
    }

    void save_fun3()
    {

    }

    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        SQuery = "SELECT 'ED' AS FSTR,'Record Efforts Done' as NAME,'ED' AS CODE FROM dual";
    }

    //'------------------------------------
    public static DataTable ConvertCSVtoDataTable(string strFilePath)
    {
        DataTable dt = new DataTable();
        using (StreamReader sr = new StreamReader(strFilePath))
        {
            string[] headers = sr.ReadLine().Split(',');
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                try
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
                catch (Exception ex)
                { }
            }
        }
        return dt;
    }
    //------------------------------------------------------------------------------------   
    protected void btnupload_Click(object sender, EventArgs e)
    {
        string ext = "", filesavepath = "";
        string excelConString = "";
        ViewState["mhd"] = "";
        if (txtacode.Value.Trim().Length > 2)
        {
            string filename = "";
            if (FileUpload1.HasFile)
            {
                ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                if (ext == ".xls")
                {
                    filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xls";
                    FileUpload1.SaveAs(filesavepath);
                    excelConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
                }
                else if (ext == ".csv")
                {
                    filename = "" + DateTime.Now.ToString("ddMMyyhhmmfff");
                    filesavepath = AppDomain.CurrentDomain.BaseDirectory + "tej-base\\Upload\\file" + filename + ".csv";
                    FileUpload1.SaveAs(filesavepath);
                    excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "tej-base\\Upload\\" + ";Extended Properties=\"Text;HDR=Yes;FMT=Delimited\"";
                }
                else if (ext == ".xlsx")
                {
                    filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xlsx";
                    FileUpload1.SaveAs(filesavepath);
                    excelConString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
                }
                else
                {
                    fgen.msg("-", "AMSG", "Please Select Excel File only in xls format!!");
                    return;
                }
                try
                {
                    OleDbConnection OleDbConn = new OleDbConnection();
                    OleDbConn.ConnectionString = excelConString;
                    OleDbConn.Open();
                    DataTable dt = OleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    OleDbConn.Close();
                    String[] excelSheets = new String[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[i] = row["TABLE_NAME"].ToString();
                        i++;
                    }
                    if (ext == ".csv")
                        excelSheets[0] = "file" + filename + ".csv";
                    OleDbCommand OleDbCmd = new OleDbCommand();
                    String Query = "";
                    Query = "SELECT  * FROM [" + excelSheets[0] + "]";
                    //==================
                    OleDbCmd.CommandText = Query;
                    OleDbCmd.Connection = OleDbConn;
                    OleDbCmd.CommandTimeout = 0;
                    OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                    objAdapter.SelectCommand = OleDbCmd;
                    objAdapter.SelectCommand.CommandTimeout = 0;
                    dt = null;
                    DataTable dt1 = new DataTable();
                    dt = ConvertCSVtoDataTable(filesavepath);
                    //  objAdapter.Fill(dt);
                    //OleDbCmd.CommandText = Query;
                    //OleDbCmd.Connection = OleDbConn;
                    //OleDbCmd.CommandTimeout = 0;
                    //OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                    //objAdapter.SelectCommand = OleDbCmd;
                    //objAdapter.SelectCommand.CommandTimeout = 0;
                    //dt = null;
                    //dt = new DataTable();
                    //objAdapter.Fill(dt);



                    DataTable dtn = new DataTable();
                    dtn.Columns.Add("INVNO", typeof(string));
                    dtn.Columns.Add("INVDT", typeof(string));
                    dtn.Columns.Add("ICODE", typeof(string));
                    dtn.Columns.Add("CPARTNO", typeof(string));
                    dtn.Columns.Add("Iname", typeof(string));
                    dtn.Columns.Add("IQTYOUT", typeof(double));
                    dtn.Columns.Add("OLDRATE", typeof(double));
                    dtn.Columns.Add("RRATE", typeof(double));
                    dtn.Columns.Add("DIFF", typeof(double));
                    dtn.Columns.Add("DIFFVAL", typeof(double));
                    dtn.Columns.Add("PONO", typeof(string));
                    dtn.Columns.Add("PODT", typeof(string));
                    dtn.Columns.Add("remarks", typeof(string));

                    dtn.Columns.Add("CGST", typeof(double));
                    dtn.Columns.Add("SGST", typeof(double));
                    dtn.Columns.Add("IGST", typeof(double));

                    dtn.Columns.Add("hscode", typeof(string));

                    if (addGrNo == "Y")
                    {
                        dtn.Columns.Add("GRNO", typeof(string));
                        dtn.Columns.Add("GRDT", typeof(string));
                    }

                    DataRow drn = null;
                    string mhd = "0";
                    string cgst = "", sgst = "", igst = "", hscode = "", tiname = "";
                    string cg_ig = "";

                    if (fgen.seek_iname(frm_qstr, frm_cocd, "select staten from famst where trim(acode)='" + txtacode.Value.Trim() + "'", "staten") == fgen.seek_iname(frm_qstr, frm_cocd, "select statenm from type where trim(type1)='" + frm_mbr + "' and id='B'", "statenm")) cg_ig = "CG";
                    else cg_ig = "IG";

                    DataTable dtSale = new DataTable();
                    dtSale = fgen.getdata(frm_qstr, frm_cocd, "SELECT distinct A.branchcd,TRIM(A.VCHNUM) as vchnum,TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS vchdate,A.BRANCHCD||TO_CHAR(A.VCHDATE,'YYYYMMDD')||A.VCHNUM AS FSTR2,A.IRATE,TRIM(A.ICODE) AS ICODE,TRIM(NVL(B.FULL_INVNO,'-')) AS FULL_INVNO FROM IVOUCHER A,SALE B WHERE A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY')=B.BRANCHCD||B.TYPE||TRIM(B.VCHNUM)||TO_cHAR(B.VCHDATE,'DD/MM/YYYY') AND  A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '4%' AND A.VCHDATE >=TO_DATE('01/04/2016','DD/MM/YYYY') AND TRIM(A.ACODE)='" + txtacode.Value.Trim() + "' order by FSTR2 ");
                    foreach (DataRow dr in dt.Rows)
                    {
                        i = dt.Rows.IndexOf(dr);
                        ViewState["mhd"] = "Row : " + (i + 1) + "'13' Part No: " + dr[2].ToString().Trim().ToUpper() + "'13' Invoice No : " + dr[0] + "'13' Dated : " + dr[1];
                        if (dr[2].ToString().Length > 2)
                        {
                            mhd = fgen.seek_iname_dt(dtn, "cpartno='" + dr[2].ToString().Trim().ToUpper().Replace("'", "") + "'", "icode");
                            cgst = fgen.seek_iname_dt(dtn, "cpartno='" + dr[2].ToString().Trim().ToUpper().Replace("'", "") + "'", "cgst");
                            sgst = fgen.seek_iname_dt(dtn, "cpartno='" + dr[2].ToString().Trim().ToUpper().Replace("'", "") + "'", "sgst");
                            igst = fgen.seek_iname_dt(dtn, "cpartno='" + dr[2].ToString().Trim().ToUpper().Replace("'", "") + "'", "igst");
                            tiname = fgen.seek_iname_dt(dtn, "cpartno='" + dr[2].ToString().Trim().ToUpper().Replace("'", "") + "'", "iname");
                            hscode = fgen.seek_iname_dt(dtn, "cpartno='" + dr[2].ToString().Trim().ToUpper().Replace("'", "") + "'", "hscode");

                            if (mhd == "0")
                            {
                                mhd = "0";
                                dt4 = new DataTable();
                                col3 = "";
                                if (dr[0].ToString().Trim().Length < 6)
                                    col3 = fgen.padlc(Convert.ToInt32(dr[0].ToString().Trim()), 6).ToString();
                                else col3 = dr[0].ToString().Trim();
                                SQuery = "select distinct a.icode,c.purpose as iname,c.exc_57f4 as cpartno,a.hscode,b.num4,b.num5,b.num6,b.num7 from item a,typegrp b,ivoucher c where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(c.vchnum)||to_char(c.vchdate,'dd/mm/yyyy')='" + col3 + Convert.ToDateTime(dr[1].ToString().Trim()).ToString("dd/MM/yyyy") + "' and c.type like '4%' and trim(c.exc_57f4)='" + dr[2].ToString().Trim().ToUpper().Replace("'", "") + "' and a.icode like '%' and trim(a.icode)=trim(c.icode) and c.branchcd='" + frm_mbr + "' ";
                                dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                                if (dt4.Rows.Count > 0)
                                {
                                    mhd = "1";
                                    mhd = dt4.Rows[0]["icode"].ToString().Trim();
                                    cgst = dt4.Rows[0]["num4"].ToString().Trim();
                                    sgst = dt4.Rows[0]["num5"].ToString().Trim();
                                    igst = dt4.Rows[0]["num6"].ToString().Trim();
                                    hscode = dt4.Rows[0]["hscode"].ToString().Trim();
                                    tiname = dt4.Rows[0]["iname"].ToString().Trim();
                                }
                                else
                                {
                                    dt4 = new DataTable();
                                    SQuery = "select a.icode,a.iname,a.cpartno,a.hscode,b.num4,b.num5,b.num6,b.num7 from item a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and a.icode like '%' and TRIM(a.CPARTNO)='" + dr[2].ToString().Trim().ToUpper().Replace("'", "") + "' order by icode desc ";
                                    dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                                    if (dt4.Rows.Count > 0)
                                    {
                                        mhd = "1";
                                        mhd = dt4.Rows[0]["icode"].ToString().Trim();
                                        cgst = dt4.Rows[0]["num4"].ToString().Trim();
                                        sgst = dt4.Rows[0]["num5"].ToString().Trim();
                                        igst = dt4.Rows[0]["num6"].ToString().Trim();
                                        hscode = dt4.Rows[0]["hscode"].ToString().Trim();
                                        tiname = dt4.Rows[0]["iname"].ToString().Trim();
                                    }
                                }
                            }
                            if (mhd != "0")
                            {
                                drn = dtn.NewRow();
                                drn["invno"] = dr[0].ToString().PadLeft(6, '0');
                                drn["invdt"] = Convert.ToDateTime(dr[1].ToString().Trim()).ToString("dd/MM/yyyy");

                                drn["icode"] = mhd;
                                drn["cpartno"] = dr[2].ToString().Trim().Replace("'", "");
                                drn["iname"] = tiname;
                                string oldrate = "";
                                oldrate = fgen.seek_iname_dt(dtSale, "FULL_INVNO='" + dr[0].ToString() + "' AND VCHDATE='" + Convert.ToDateTime(dr[1].ToString().Trim()).ToString("dd/MM/yyyy") + "' AND ICODE='" + mhd + "'", "IRATE");
                                if (oldrate == "0" || oldrate == "")
                                    oldrate = fgen.seek_iname_dt(dtSale, "VCHNUM='" + dr[0].ToString().PadLeft(6, '0') + "' AND VCHDATE='" + Convert.ToDateTime(dr[1].ToString().Trim()).ToString("dd/MM/yyyy") + "' AND ICODE='" + mhd + "'", "IRATE");
                             
                                drn["oldrate"] = fgen.make_double(oldrate, 3);
                                //if(frm_cocd=="ATOP")
                                //{
                                //    drn["iqtyout"] = fgen.make_double(dr[4].ToString().Trim(), 3);
                                //    drn["rrate"] = fgen.make_double(dr[6].ToString().Trim(), 3);
                                //    drn["diff"] = fgen.make_double(drn["oldrate"].ToString().Trim(), 3) + fgen.make_double(drn["rrate"].ToString().Trim(), 3);
                                //    drn["diffval"] = fgen.make_double(dr[8].ToString().Trim(), 3);
                                //    try
                                //    {
                                //        drn["remarks"] = dr[9].ToString().Trim();
                                //    }
                                //    catch { drn["remarks"] = "-"; }
                                //}
                                //else
                                //{
                                    drn["iqtyout"] = fgen.make_double(dr[5].ToString().Trim(), 3);
                                    drn["rrate"] = fgen.make_double(dr[4].ToString().Trim(), 3);
                                    drn["diff"] = fgen.make_double(drn["oldrate"].ToString().Trim(), 3) + fgen.make_double(drn["rrate"].ToString().Trim(), 3);
                                    drn["diffval"] = fgen.make_double(dr[6].ToString().Trim(), 3);
                                    try
                                    {
                                        drn["remarks"] = dr[7].ToString().Trim();
                                    }
                                    catch { drn["remarks"] = "-"; }
                               // }                                                                                                             

                                if (cg_ig == "CG") igst = "0";
                                else
                                {
                                    cgst = "0";
                                    sgst = "0";
                                }
                                drn["cgst"] = cgst;
                                drn["sgst"] = sgst;

                                drn["igst"] = igst;
                                drn["hscode"] = hscode;

                                if (addGrNo == "Y")
                                {
                                    try { drn["GRNO"] = dr[8].ToString().Trim(); }
                                    catch { drn["GRNO"] = "-"; }
                                    try { drn["GRDT"] = dr[9].ToString().Trim(); }
                                    catch { drn["GRdt"] = DateTime.Now.ToString("dd/MM/yyyy"); }
                                    try { drn["oldrate"] = dr[10].ToString().Trim(); }
                                    catch { }
                                    try { drn["diff"] = dr[11].ToString().Trim(); }
                                    catch { }
                                }
                                else
                                {
                                    try
                                    {
                                        drn["pono"] = dr[8].ToString().Trim();
                                    }
                                    catch { drn["pono"] = "-"; }
                                    try
                                    {
                                        drn["podt"] = dr[9].ToString().Trim();
                                    }
                                    catch { drn["podt"] = DateTime.Now.ToString("dd/MM/yyyy"); }
                                }

                                dtn.Rows.Add(drn);
                            }
                        }
                    }

                    ViewState["dtn"] = dtn;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);

                    fgen.msg("-", "AMSG", "Total Rows Imported : " + dtn.Rows.Count.ToString());
                }
                catch (Exception ex)
                {
                    fgen.FILL_ERR(ex.Message.ToString());
                    fgen.msg("-", "AMSG", "Please Check " + ViewState["mhd"].ToString() + "");
                }
            }
        }
        else
        {
            fgen.msg("-", "AMSG", "Please Select Customer First!!");
        }
    }

    protected void btnAcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Supplier ", frm_qstr);
    }
    protected void btnRcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TRCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Leadger ", frm_qstr);
    }
    protected void btnDNCN_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DNCN";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select D/N C/N Reaosn", frm_qstr);
    }
    protected void btnGstClass_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "GSTCLASS";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select GST Class", frm_qstr);
    }
    protected void btnFormat_ServerClick(object sender, EventArgs e)
    {
        set_Val();
        if (addGrNo == "Y")
        {
            SQuery = "select 'Invoice No.' as Invoice_No ,'Invoice Date' as Invoice_Date, 'Material' as Material ,'Material Description' as Material_Description,'Diff. Rate' as Diff_Rate,'Sum Of Invoice Qty' as Sum_Of_Invoice_Qty,'Remarks' as Remarks,'GR No.' as GR_No,'GR. Dt.' as GR_Dt,'Old Rate' as Old_Rate,'Diff' as Diff from dual";
        }
        else
        {
            //SQuery = "select 'Invoice No.' as Invoice_No ,'Invoice Date' as Invoice_Date, 'Material' as Material ,'Material Description' as Material_Description,'Diff. Rate' as Diff_Rate,'Sum Of Invoice Qty' as Sum_Of_Invoice_Qty,'Remarks' as Remarks,'PO. No.' as PO_No,'PO. Dt.' as PO_Dt from dual";//old
            SQuery = "select 'Invoice No.' as Invoice_No ,'Invoice Date' as Invoice_Date, 'Material' as Material ,'Material Description' as Material_Description,'Old Rate' as Old_Rate,'New Rate' as new_rate,'Diff. Rate' as Diff_Rate,'Sum Of Diff Value' as Sum_Of_diff_value,'Remarks' as Remarks from dual";//new
        }
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        fgen.exp_to_excel(dt, "ms-excel", ".xls", frm_cocd + "_Auto Dr Cr Note");
    }
    protected void btnfrmt_Click(object sender, EventArgs e)
    {
        SQuery = "select '-' as Invoice_no,null as invoice_date,'-' as partno,'-' as item_name,0 as rate,0 as qty,0 as value,'-' as remark from dual";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        fgen.exp_to_excel(dt, "xls", ".xls", "Format");
    }

}
