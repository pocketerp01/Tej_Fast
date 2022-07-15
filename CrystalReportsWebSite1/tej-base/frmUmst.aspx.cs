using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.IO;

public partial class frmUmst8 : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4;
    DataRow oporow; DataSet oDS;
    int i = 0, z = 0;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            btnnew.Focus();
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            frm_PageName = Path.GetFileName(Request.Url.AbsoluteUri);
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
                doc_addl.Value = "0";

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();

            txtPwd.Attributes.Add("type", "password");
            txtCpwd.Attributes.Add("type", "password");            
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
        tab5.Visible = false;
        tab4.Visible = false;
        tab3.Visible = false;
        tab2.Visible = false;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "*******":
                tab3.Visible = false;
                tab4.Visible = false;
                break;
        }
        if (Prg_Id == "*******")
        {
            tab5.Visible = true;
        }
        lblheader.Text = "User Management";
        btnprint.Visible = false;
        if (frm_cocd == "MSES") divCan.Visible = false;
        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        ImageButton1.Attributes.Add("onclick", "this.disabled=true;"); btnDept.Attributes.Add("onclick", "this.disabled=true;"); btnMplant.Attributes.Add("onclick", "this.disabled=true;");
        ImageButton2.Attributes.Add("onclick", "this.disabled=true;"); ImageButton3.Attributes.Add("onclick", "this.disabled=true;");
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        ImageButton1.Attributes.Add("onclick", "this.disabled=false;"); btnDept.Attributes.Add("onclick", "this.disabled=false;"); btnMplant.Attributes.Add("onclick", "this.disabled=false;");
        ImageButton2.Attributes.Add("onclick", "this.disabled=false;"); ImageButton3.Attributes.Add("onclick", "this.disabled=false;");
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
        doc_nf.Value = "USERID";
        doc_df.Value = "";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "EVAS";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);

        if (txtPwd.Value == "") txtPwd.Style.Add("border", "1px solid red;");
        else txtPwd.Style.Remove("border");
        if (txtCpwd.Value == "") txtCpwd.Style.Add("border", "1px solid red;");
        else txtCpwd.Style.Remove("border");
        if (txtUserID.Value == "") txtUserID.Style.Add("border", "1px solid red;");
        else txtUserID.Style.Remove("border");
        if (txtUserName.Value == "") txtUserName.Style.Add("border", "1px solid red;");
        else txtUserName.Style.Remove("border");
        if (txtDept.Value == "") txtDept.Style.Add("border", "1px solid red;");
        else txtDept.Style.Remove("border");
        if (txtMultiPlant.Value == "") txtMultiPlant.Style.Add("border", "1px solid red;");
        else txtMultiPlant.Style.Remove("border");
        if (txtEmail.Value == "") txtEmail.Style.Add("border", "1px solid red;");
        else txtEmail.Style.Remove("border");
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
            case "TACODE":
                //pop1
                SQuery = "SELECT Type1,Name,Type1 AS CODE,id2 as Ref FROM Type WHERE id='#' and id2='TM' ORDER BY Name ";

                break;
            case "TICODE":
                //pop2
                SQuery = "SELECT ACODE AS FSTR,ANAME AS PARTY,ACODE AS CODE,ADDR1,ADDR2 FROM CSMST where branchcd!='DD' ORDER BY aname ";
                //SQuery = "SELECT ICODE AS FSTR,INAME AS PRODUCT,ICODE AS CODE,UNIT,CPARTNO AS PARTNO FROM ITEM WHERE LENGTH(tRIM(ICODE))>4 ";
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                SQuery = "SELECT Type1,Name,Type1 AS CODE,id2 as Ref FROM Type WHERE id='#' and id2='CL' ORDER BY Name ";
                break;
            case "New":
            case "List":
            case "Edit*":
            case "Del*":
            case "Print":
                Type_Sel_query();
                break;
            case "MPLANT":
                SQuery = "Select type1 as fstr,name, type1 as code from type where ID='B' order by type1";
                break;
            case "MERP":
                SQuery = "Select id as fstr,id as code,text from FIN_MSYS where mlevel=1 order by id";
                if (frm_cocd == "MSES") SQuery = "select acode as fstr,name,acode as code from proj_mast where type='D1' order by name";
                break;
            case "DEPTT":
                SQuery = "Select type1 as fstr,name, type1 as code from type where ID='M' and type1 like '6%' order by type1";
                if (frm_cocd == "MSES")
                    SQuery = "SELECT distinct TRIM(vCHNUM) AS FSTR,NAME AS DEPARTMENT_NAME,REQ_NAME AS REQ_BY,MENT_BY AS ENTBY,vchnum as code FROM PROJ_MAST WHERE TYPE='P8' order by vchnum";
                break;
            case "WIP":
                SQuery = "SELECT TYPE1 AS FSTR,Name as Unit_Name,type1 as Code FROM type where id='1' and substr(type1,1,1) in ('6') order by type1";
                break;
            case "FSEC":
                SQuery = "SELECT TYPE1 AS FSTR,NAME,TYPE1  FROM TYPE WHERE ID=':' and type1>='10' order by type1";
                break;
            default:
                if (btnval == "Edit" || btnval == "Del" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "Select userid as fstr,Username,Userid,decode(trim(ulevel),'0','0:TOP LEVEL','1','1:Administrator','2','2:Department Head','2.5','2:View Rights','3:Operator','4:Secured') Rights,Emailid,Contactno,Deptt  from EVAS where branchcd='" + frm_mbr + "' order by Userid";
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
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_DEAC", "-");

        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            //hffield.Value = "New";
            //make_qry_4_popup();
            //fgen.Fn_open_sseek("-", frm_qstr);

            // else comment upper code

            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(userid) AS VCH FROM " + frm_tabname + " ", 6, "VCH");
            txtUserIDNo.Value = frm_vnum;
            disablectrl();
            fgen.EnableForm(this.Controls);
            txtUserID.Focus();
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
            fgen.Fn_open_sseek("Select User ID to Edit", frm_qstr);
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

        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select userid from evas where trim(upper(username))='" + txtUserID.Value.ToUpper().Trim() + "'", "userid");
        if (col1 != "0" && edmode.Value != "Y")
        {
            fgen.msg("-", "AMSG", "User ID already Exist!!"); txtUserID.Focus();
            return;
        }

        if (txtPwd.Value.ToUpper().Trim() != txtCpwd.Value.ToUpper().Trim())
        {
            fgen.msg("-", "AMSG", "Password and Confirm Password is not matching!!"); txtPwd.Focus();
            return;
        }

        if (pwd1.Value == "WRONG")
        {
            fgen.msg("-", "AMSG", "Password is not correct!!"); txtPwd.Focus();
            return;
        }

        if (frm_ulvl == "2.5")
        {
            fgen.msg("-", "CMSG", "Dear " + frm_uname + " ,You Have Rights to View Only.So ERP Will Not Allow You to Modify Data !");
            return;
        }
        if (txtDept.Value.Trim().ToUpper() == "GATE" && Convert.ToDouble(dd2.Value) < 2)
        {
            fgen.msg("-", "CMSG", "Dear " + frm_uname + " ,Gate ID Cannot be Above Operator Level !"); dd2.Focus();
            return;
        }


        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;


        if (txtUserID.Value.Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / User-Id";
        }

        if (txtEmail.Value.Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / Email-id";
        }

        if (txtUserName.Value.Trim() == "" || txtUserName.Value.Trim() == "-")
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / User Name";
        }
        if (txtUserID.Value.Trim().ToUpper() == txtPwd.Value.Trim().ToUpper())
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / Password and Username Cannot be Similar";
        }
        if (txtPwd.Value.Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / Please Enter Password for this User!!";
        }

        if (txtDept.Value.Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / Please Enter Department for this User!!";
        }

        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }





        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }
        fgen.fill_dash(this.Controls);
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
            fgen.Fn_open_sseek("Select User ID to Delete", frm_qstr);
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
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

        SQuery = "SELECT a.Username,a.Deptt,decode(trim(ulevel),'0','0:TOP LEVEL','1','1:Administrator','2','2:Department Head','2.5','2:View Rights','3:Operator','4:Secured') Rights,a.Can_ADD,a.Can_edit,a.Can_del,a.allowbr as Br_allowed,a.mdeptt as Multi_deptt,a.branchcd,a.userid,a.ent_by,a.ent_Dt,a.edt_by,a.edt_Dt,a.close_by,a.close_dt  FROM evas a, type b where a.branchcd=b.type1 and b.id='B' and a.branchcd!='DD' and a.branchcd='" + frm_mbr + "' order by a.Userid";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("List of Users", frm_qstr);
        hffield.Value = "-";
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + DateRange + " and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
        fgen.Fn_FillChart(frm_cocd, frm_qstr, "Testing Chart", "line", "Main Heading", "Sub Heading", SQuery, "");
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
        //--
        string CP_deac;
        CP_deac = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_DEAC");
        if (CP_deac != "-" && CP_deac != "0")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                txt_deac_by.Value = frm_uname;
                txt_deac_dt.Value = vardate;
            }
            else
            {
                txt_deac_by.Value = "-";
                txt_deac_dt.Value = "-";
            }

        }

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
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_DEAC", "-");
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.userid='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                //fgen.execute_cmd(frm_qstr,frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), DateTime.Now.ToString("dd/MM/yyyy"), frm_uname, "US", lblheader.Text.Trim() + " Deleted");
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
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");

                    disablectrl();
                    fgen.EnableForm(this.Controls);

                    // Popup asking for Copy from Older Data
                    fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
                    hffield.Value = "NEW_E";
                    break;
                    #endregion
                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    edmode.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    //hffield.Value = "Del_E";
                    //make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);                    
                    break;
                case "MPLANT":
                    txtMultiPlant.Value = col1;
                    txtPwd.Focus();
                    break;
                case "WIP":
                    txt_wip.Value = col1;
                    txtMultiPlant.Focus();
                    break;
                case "DEAC_BUT":
                    col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
                    if (col1 == "Y")
                    {
                        txt_deac_by.Value = frm_uname;
                        txt_deac_dt.Value = vardate;
                    }
                    else
                    {
                        txt_deac_by.Value = "-";
                        txt_deac_dt.Value = "";
                    }
                    ImageButton2.Focus();
                    break;

                case "FSEC":
                    txt_fsec.Value = col1;
                    break;
                case "MERP":
                    // txtMultiERP.Value = col2;
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
                case "Edit":
                    //edit_Click
                    #region Edit Start
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_DEAC", "-");
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select * from " + frm_tabname + " where userid='" + col1 + "' ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtUserIDNo.Value = dt.Rows[0]["userid"].ToString().Trim();
                        txtUserID.Value = dt.Rows[0]["username"].ToString().Trim();

                        string sdeptt = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where ID='M' and type1='" + dt.Rows[0]["ERPdeptt"].ToString().Trim() + "' ", "name");

                        txtDept.Value = sdeptt;
                        txtDeptCode.Value = dt.Rows[0]["ERPdeptt"].ToString().Trim();
                        txtDeptCode.Value = dt.Rows[0]["deptt"].ToString().Trim();

                        txtPwd.Value = dt.Rows[0]["level3pw"].ToString().Trim();
                        txtCpwd.Value = dt.Rows[0]["level3pw"].ToString().Trim();
                        //txtMultiERP.Value = dt.Rows[0]["ERPDEPTT"].ToString().Trim();
                        dd2.Value = dt.Rows[0]["ulevel"].ToString();
                        txtEmail.Value = dt.Rows[0]["emailid"].ToString().Trim();
                        txtMobile.Value = dt.Rows[0]["contactno"].ToString().Trim();
                        txtCanAdd.Value = dt.Rows[0]["CAN_ADD"].ToString().Trim();
                        txtCanEdit.Value = dt.Rows[0]["CAN_EDIT"].ToString().Trim();
                        txtCanDel.Value = dt.Rows[0]["CAN_DEL"].ToString().Trim();
                        txtMultiPlant.Value = dt.Rows[0]["ALLOWBR"].ToString().Trim();
                        lblUpload.Text = dt.Rows[0]["icons"].ToString().Trim();
                        txtMstFile.Value = dt.Rows[0]["CAN_MST"].ToString().Trim();
                        txtCanAppr.Value = dt.Rows[0]["CAN_APPRV"].ToString().Trim();
                        txtCanViewCons.Value = dt.Rows[0]["CAN_CON"].ToString().Trim();
                        txtUserName.Value = dt.Rows[0]["FULL_NAME"].ToString().Trim();
                        txtCanVW.Value = dt.Rows[0]["CAN_PY"].ToString().Trim();
                        txtCanVW.Value = dt.Rows[0]["CAN_CHPYV"].ToString().Trim();
                        txt_deac_by.Value = dt.Rows[i]["close_by"].ToString().Trim();
                        Txt_desk.Value = dt.Rows[i]["CAN_TNS"].ToString().Trim();
                        txt_grid.Value = dt.Rows[i]["DT_GRID"].ToString().Trim();
                        txt_sess.Value = dt.Rows[i]["ERP_SESS"].ToString().Trim();

                        if (dt.Rows[i]["PWCHGDT"].ToString().Trim() == "" || dt.Rows[i]["PWCHGDT"].ToString().Trim() == "-")
                        { }
                        else
                        {
                            txt_pwdchg.Value = Convert.ToDateTime(dt.Rows[i]["PWCHGDT"]).ToString("dd/MM/yyyy");
                        }

                        if (dt.Rows[i]["close_dt"].ToString().Trim() == "" || dt.Rows[i]["close_dt"].ToString().Trim() == "-")
                        { }
                        else
                        {
                            txt_deac_dt.Value = Convert.ToDateTime(dt.Rows[i]["close_dt"]).ToString("dd/MM/yyyy");
                        }
                        txt_fsec.Value = dt.Rows[i]["ALLOWFSEC"].ToString().Trim();
                        txt_wip.Value = dt.Rows[i]["ALLOWPROD"].ToString().Trim();

                        dt.Dispose();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                        set_Val();
                        txtUserID.Disabled = true;                        
                    }
                    #endregion
                    break;
                case "DEPTT":
                    if (col1 == "") return;
                    txtDeptCode.Value = col1;
                    txtDept.Value = col2;
                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
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

            hffield.Value = "-";
        }
        else
        {
            if (edmode.Value == "Y")
            {

            }
            else
            {

            }

            i = 0;
            setColHeadings();

            Checked_ok = "Y";
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
                            save_it = "Y";
                            frm_vnum = txtUserIDNo.Value;
                        }
                        else
                        {


                            save_it = "Y";
                            if (save_it == "Y")
                            {
                                i = 0;
                                do
                                {
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(userid) AS VCH FROM " + frm_tabname + " ", 6, "VCH");
                                    //pk_error = fgen.chk_pk(frm_qstr,frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                    if (i > 10)
                                    {
                                        fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(userid) AS VCH FROM " + frm_tabname + " ", 6, "VCH");
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

                        save_fun();


                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where USERID='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);


                        if (edmode.Value == "Y")
                        {
                            cmd_query = "delete from " + frm_tabname + " where USERID='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "' and branchcd='DD'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");
                        }
                        else
                        {
                            {
                                if (save_it == "Y")
                                {
                                    //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");save
                                    fgen.msg("-", "AMSG", lblheader.Text + " " + " Saved Successfully ");
                                }
                                else
                                {
                                    fgen.msg("-", "AMSG", "Data Not Saved");
                                }
                            }
                        }

                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                        hffield.Value = "SAVED";
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
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Supplier ", frm_qstr);
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


        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = frm_mbr;
        oporow["USERID"] = frm_vnum;
        oporow["USERNAME"] = txtUserID.Value.Trim().ToUpper();

        oporow["Deptt"] = txtDeptCode.Value;

        //Select Case Left(ur.text, 1)
        //    Case "0", "1"
        //        rsevas!DEPTT = "0 : Global"
        //    Case Else
        //        rsevas!DEPTT = Trim(ud.text)
        //End Select

        oporow["LEVEL1PW"] = txtPwd.Value.Trim().ToUpper();
        oporow["LEVEL2PW"] = txtPwd.Value.Trim().ToUpper();
        oporow["LEVEL3PW"] = txtPwd.Value.Trim().ToUpper();
        oporow["TYPE"] = "01";
        oporow["ULEVEL"] = dd2.Value;
        oporow["ERPDEPTT"] = txtDeptCode.Value.Trim().ToUpper();

        oporow["EMAILID"] = txtEmail.Value.Trim().ToUpper();
        oporow["CONTACTNO"] = txtMobile.Value.Trim().ToUpper();
        oporow["CAN_aDD"] = txtCanAdd.Value.Trim().ToUpper();
        oporow["CAN_EDIT"] = txtCanEdit.Value.Trim().ToUpper();
        oporow["CAN_DEL"] = txtCanDel.Value.Trim().ToUpper();
        oporow["ALLOWBR"] = txtMultiPlant.Value.Trim().ToUpper();
        oporow["ICONS"] = lblUpload.Text.Trim();
        oporow["SMSOPTS"] = "-";
        oporow["CAN_MST"] = txtMstFile.Value.Trim().ToUpper();
        oporow["CAN_aPPRV"] = txtCanAppr.Value.Trim().ToUpper();
        if (txt_sess.Value.Trim().toDouble() == 0)
        { oporow["ERP_SESS"] = 1; }
        else
        {
            oporow["ERP_SESS"] = txt_sess.Value.Trim().toDouble();
        }
        oporow["CAN_APPRV"] = txtCanAppr.Value.Trim().ToUpper();
        oporow["ALLOWPROD"] = txt_wip.Value.Trim().ToUpper();
        oporow["CAN_CON"] = txtCanViewCons.Value.Trim().ToUpper();
        oporow["ALLOWFSEC"] = txt_fsec.Value.Trim().ToUpper();
        oporow["ALLOWIGRP"] = "-";
        oporow["FULL_NAME"] = txtUserName.Value.Trim().ToUpper();
        oporow["HDD_ID"] = "-";
        oporow["CTERMINAL"] = "-";
        oporow["HDD_SID"] = "-";
        oporow["CAN_TNS"] = Txt_desk.Value.Trim().ToUpper();
        oporow["DT_GRID"] = txt_grid.Value.Trim().ToUpper();
        oporow["ALLOWPROD2"] = "-";
        oporow["CAN_PY"] = txtCanVW.Value.Trim().ToUpper();
        oporow["CAN_CHPYV"] = txtCanVW.Value.Trim().ToUpper();
        oporow["CAN_ITM"] = "-";
        oporow["CAN_ACM"] = "-";
        oporow["CAN_ADM"] = "-";
        oporow["close_by"] = txt_deac_by.Value.ToUpper().Trim();
        if (txt_deac_dt.Value.Length == 10 && txt_deac_by.Value.Length > 1)
        {
            oporow["close_dt"] = fgen.make_def_Date(txt_deac_dt.Value.ToUpper().Trim(), vardate);
        }
        else
        {
            oporow["close_dt"] = DBNull.Value;
        }
        if (txt_pwdchg.Value.Length == 10)
        {
            oporow["PWCHGDT"] = fgen.make_def_Date(txt_pwdchg.Value.ToUpper().Trim(), vardate);
        }
        else
        {
            oporow["PWCHGDT"] = DBNull.Value;
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

        //oporow["app_by"] = "-";
        //oporow["app_dt"] = vardate;
        oDS.Tables[0].Rows.Add(oporow);
    }
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        SQuery = "SELECT 'ED' AS FSTR,'Record Efforts Done' as NAME,'ED' AS CODE FROM dual";
    }
    //------------------------------------------------------------------------------------   
    protected void btnDept_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DEPTT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Department", frm_qstr);
    }
    protected void btnMulERP_Click(object sender, ImageClickEventArgs e)
    {
        //hffield.Value = "MERP";
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("Select Multiple Module", frm_qstr);
    }
    protected void btnMplant_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MPLANT";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select Plant", frm_qstr);
    }
    protected void btnDeact_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DEAC_BUT";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_DEAC", hffield.Value);
        fgen.msg("-", "CMSG", "Are You Sure!! You Want to Deactivate This User");
    }
    protected void btnWIP_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "WIP";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select", frm_qstr);
    }
    protected void btnfsec_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "FSEC";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select", frm_qstr);
    }
    protected void btnAtt_Click(object sender, EventArgs e)
    {
        string filepath = "";
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            string ext = Path.GetExtension(Attch.FileName).ToLower();
            filepath = frm_cocd + "_" + "DP_" + txtUserIDNo.Value + ext;
            Attch.PostedFile.SaveAs(Server.MapPath("~/tej-base/UPLOAD/") + filepath);
            Attch.PostedFile.SaveAs(@"c:\TEJ_ERP\upload\" + filepath);
            lblUpload.Text = filepath;
            btnView1.Visible = true;
            btnDwnld1.Visible = true;
        }
        else
        {
            lblUpload.Text = "";
        }
    }
    protected void btnView1_Click(object sender, EventArgs e)
    {
        string filePath = lblUpload.Text;
        try
        {
            string newPath = Server.MapPath(@"~\tej-base\upload\");
            string filename = Path.GetFileName(filePath);
            newPath += filename;
            File.Copy(filePath, newPath, true);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filename + "','90%','90%','');", true);
        }
        catch { }
    }
    protected void btnDwnld1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));

            Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
            //Session["FileName"] = txtAttch.Text;
            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");
        }
        catch { }
    }
}