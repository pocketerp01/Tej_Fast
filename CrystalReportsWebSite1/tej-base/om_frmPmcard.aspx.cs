using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_frmPmcard : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt2, dt3, dt4;
    DataRow oporow; DataSet oDS, oDs1;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_tabname1, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
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
                    frm_uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    frm_myear = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    frm_ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    frm_mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    //DateRange = "between to_date('01/04/2019','dd/mm/yyyy') and to_date('30/04/2020','dd/mm/yyyy')";
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                //doc_addl.Value = "0";

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();

            //txtPwd.Attributes.Add("type", "password");
            //txtCpwd.Attributes.Add("type", "password");
        }
    }
    //------------------------------------------------------------------------------------
    void getColHeading()
    {
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        if (dtCol == null || dtCol.Rows.Count <= 0)
        {
            dtCol = fgen.getdata(frm_qstr, frm_cocd, "SELECT UPPER(OBJ_NAME) AS OBJ_NAME,OBJ_CAPTION,OBJ_WIDTH,UPPER(OBJ_VISIBLE) AS OBJ_VISIBLE,nvl(col_no,0) as COL_NO,nvl(OBJ_MAXLEN,0) as OBJ_MAXLEN,nvl(OBJ_READONLY,'N') as OBJ_READONLY,NVL(OBJ_FMAND,'N') AS OBJ_FMAND FROM SYS_CONFIG WHERE UPPER(TRIM(FRM_NAME))='" + frm_formID + "'");
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
        lblheader.Text = "Maintenance Request/Complaint Card";
        btnprint.Visible = true;
        //if (frm_cocd == "MSES") divCan.Visible = false;
        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false;
        btnCamera.Disabled = true;
        create_tab();

        sg1_add_blankrows();
        //sg3_add_blankrows();

        //btnlbl4.Enabled = false;
        //btnlbl7.Enabled = false;

        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();

    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnCamera.Disabled = false;
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
        frm_tabname = "SCRATCH";
        frm_tabname1 = "MULTIVCH";
        frm_vty = "MN";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        typePopup = "N";
        switch (Prg_Id)
        {
            case "F75113":
                btnedit.Visible = false;
                break;
            case "F75111":
                btnnew.Visible = false;
                btnedit.InnerText = "New";                
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
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='G' and substr(type1,1,1)='1'";
                break;
            case "BTN_11":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='G' and substr(type1,1,1)='2'";
                break;
            case "BTN_12":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='G' and substr(type1,1,1)='3'";
                break;
            case "BTN_13":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='G' and substr(type1,1,1)='4'";
                break;
            case "BTN_14":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='H' and substr(type1,1,1)='1'";
                break;
            case "BTN_15":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='G' and substr(type1,1,1)='7'";
                break;
            case "BTN_16":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='G' and substr(type1,1,1)='6'";
                break;
            case "BTN_17":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='H' and substr(type1,1,1)='0'";
                break;
            case "BTN_18":
                SQuery = "SELECT '10' as fstr,'Required' as NAME,'10' as Code FROM dual union all SELECT '11' as fstr,'Not Required' as NAME,'11' as Code FROM dual";
                break;
            case "BTN_19":
                SQuery = "SELECT '10' as fstr,'As Applicable' as NAME,'10' as Code FROM dual ";
                break;

            case "BTN_20":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM FAMST where substr(acode,1,2) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_21":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM FAMST where substr(acode,1,2) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_22":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM FAMST where substr(acode,1,2) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_23":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1,rate  FROM TYPE WHERE ID='A' order by name ";
                break;
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
                SQuery = "SELECT distinct icode as fstr,iname as iname,icode as code FROM item ORDER BY iname asc ";
                break;
            case "New":
            case "List":
            case "Edit*":
            case "Del*":
            case "Print":
                SQuery = "select distinct VCHNUM||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR, col18 as Deptt,to_char(vchdate,'dd/mm/yyyy') as vchdate,vchnum||'  '||decode(trim(nvl(col22,'-')),'-','(Pending)','(Closed)') as Sheet_No ,col12 as Comp_type,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt,vchnum,num5 as Amt,type from scratch where  type='MN' and branchcd='" + frm_mbr + "' and vchnum<>'000000' and vchdate " + DateRange + " order by vchnum desc";
                break;
            case "MPLANT":
                SQuery = "Select type1 as fstr,name, type1 as code from type where ID='B' order by type1";
                break;
            case "MERP":
                SQuery = "Select id as fstr,id as code,text from FIN_MSYS where mlevel=1 order by id";
                if (frm_cocd == "MSES") SQuery = "select acode as fstr,name,acode as code from proj_mast where type='D1' order by name";
                break;
            case "DEPTT":
                if (frm_cocd == "BLIS" || frm_cocd == "BASV")
                {
                    SQuery = "SELECT type1 as fstr,name, type1 as code FROM type where id='D' and substr(type1,1,1)='0' order by type1";
                }
                else
                {
                    SQuery = "SELECT type1 as fstr,name, type1 as code FROM type where id='M' and substr(type1,1,1) in('6','7','8') order by name";
                }

                break;
            case "Shift":
                SQuery = "select type1 as fstr,name, type1 as code from type where id='D' and substr(type1,1,1)='1' ";
                break;
            case "MC":
                if (frm_cocd == "BLIS" || frm_cocd == "BASV")
                {
                    SQuery = "Select distinct mchcode as fstr, mchname  as name,acode as code from pmaint where branchcd='" + frm_mbr + "' and type='10' order by mchname ";
                }
                else if (frm_cocd == "SVPL")
                {
                    SQuery = "Select type1 as fstr,name, type1 as code from type where id in('^') order by name";
                }

                else if (frm_cocd == "JSGI" || frm_cocd == "SR")
                {
                    SQuery = "Select Icode as fstr,  Iname as name, from Item where length(Trim(icode))>4 and icode like '69%' order by Iname";
                }

                else
                {
                    SQuery = "Select distinct mchcode as fstr, mchname  as name,acode as code from pmaint where branchcd='" + frm_mbr + "' and type='10' order by mchname ";
                }

                break;
            case "Incharge":
                SQuery = "Select e.EMPCODE as fstr, e.NAME AS name,e.DESG as Code FROM EMPMAS e where e.branchcd='" + frm_mbr + "'  order by e.empcode";
                break;
            case "Nature":
                SQuery = "select 'AMC' as fstr,'AMC' as name,'AMC' as Code from dual union all select 'P/Maint' as fstr,'P/Maint' as name,'P/Maint' as Code from dual union all select 'Repair' as fstr,'Repair' as name,'Repair' as Code from dual union all select 'Consumables' as fstr,'Consumables' as name,'Consumables' as Code from dual";
                break;
            case "Comp":
                SQuery = "select type1 as fstr, name ,branchcd as code from typegrp where  id='MN'  order by srno";
                break;
            default:
                if (btnval == "Edit" || btnval == "Del" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct VCHNUM||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR, col18 as Deptt,to_char(vchdate,'dd/mm/yyyy') as vchdate,vchnum||'  '||decode(trim(nvl(col22,'-')),'-','(Pending)','(Closed)') as Sheet_No ,col12 as Comp_type,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt,vchnum,num5 as Amt,type from scratch where  type='MN' and branchcd='" + frm_mbr + "' and vchnum<>'000000' and vchdate " + DateRange + " order by vchnum desc";
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

            if (typePopup == "N") newCase(frm_vty);

            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }

            chkclr.Checked = false;
            TxtClr.Value = "-";


            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vchnum) AS vchnum FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and substr(type,1,2)='MN' and vchdate " + DateRange + " ORDER BY VCHDATE DESC  ", 6, "vchnum");
            TxtCardNo.Value = frm_vnum;
            TxtDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            TxtdateComp.Value = DateTime.Now.ToString("yyyy-MM-dd");
            //TxtDate.Value = todt;

            disablectrl();
            fgen.EnableForm(this.Controls);
            TxtCardNo.Focus();
            ///
            btnshift.Enabled = true;
            btnDeptt.Enabled = true;
            btnMc.Enabled = true;
            BtnIncharge.Enabled = true;


            //ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "disableFrame('div2');", true);
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
            fgen.Fn_open_sseek("Select Complaint No Edit", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");

        chkclr.Checked = false;
        TxtClr.Value = "-";

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

        if (TxtCardNo.Value.Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Enter Complaint No!!");
            TxtCardNo.Focus();
            return;
        }

        if (TxtShift.Value.Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Enter Shift!!");
            TxtShift.Focus();
            return;
        }

        if (Txtdept.Value.Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Enter Department!!");
            Txtdept.Focus();
            return;
        }
        if (edmode.Value == "Y")
        {
            if (txtDeptSup.Value.Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Enter Department Superior!!");
                txtDeptSup.Focus();
                return;
            }

            if (TxtProbObs.Value.Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Enter Problem Observed!!");
                TxtProbObs.Focus();
                return;
            }

            if (TxtCorrAct.Value.Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Enter Corrective Actons Taken!!");
                TxtCorrAct.Focus();
                return;
            }

            if (TxtClosureDate.Value.Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Enter Date of Closure!!");
                TxtClosureDate.Focus();
                return;
            }

            //int dateclos = fgen.ChkDate(TxtClosureDate.Value.ToString());
            //if (dateclos == 0)
            //{ fgen.msg("-", "AMSG", "Please Select a Valid Closure Date"); TxtClosureDate.Focus(); return; }

        }
        //int datecomp = fgen.ChkDate(TxtdateComp.Value.ToString());
        //if (datecomp == 0)
        //{ fgen.msg("-", "AMSG", "Please Select a Valid Expected Date of Completion"); TxtdateComp.Focus(); return; }

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }
        fgen.fill_dash(this.Controls);
        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
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
            fgen.Fn_open_sseek("Select Complaint No to Delete", frm_qstr);
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

        sg1_dt = new DataTable();
        sg2_dt = new DataTable();
        sg3_dt = new DataTable();
        create_tab();

        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();

        ViewState["sg1"] = null;

        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select " + lblheader.Text, frm_qstr);
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where branchcd='" + frm_mbr + "' and type='MN' and a.vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname1 + " a where branchcd='" + frm_mbr + "' and type='MN' and a.vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname1 + " a where branchcd='" + frm_mbr + "' and type='SS' and a.vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                //fgen.execute_cmd(frm_qstr,frm_cocd, "delete from wsr_ctrl a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
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
                    //txtMultiPlant.Value = col1;
                    //txtMultiPlant.Focus();
                    break;
                case "MERP":
                    //txtMultiERP.Value = col2;
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
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", col1);//for grade                           
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F75113");
                    fgen.fin_pmaint_reps(frm_qstr);
                    break;
                case "Edit":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select * from " + frm_tabname + " where BRANCHCD='" + frm_mbr + "' AND TYPE='MN' AND  VCHNUM||TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + col1 + "' and vchdate " + DateRange + " ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        string aname = "";
                        aname = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='D' and type1='" + dt.Rows[0]["acode"] + "'", "name");
                        TxtCardNo.Value = dt.Rows[0]["vchnum"].ToString().Trim();
                        TxtDate.Value = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("yyyy-MM-dd");
                        if (dt.Rows[0]["docdate"].ToString().Trim().Length > 3)
                            TxtdateComp.Value = Convert.ToDateTime(dt.Rows[0]["docdate"].ToString().Trim()).ToString("yyyy-MM-dd");
                        TxtShift.Value = aname;
                        TxtShiftCode.Value = dt.Rows[0]["acode"].ToString().Trim();
                        TxtMCCode.Value = dt.Rows[0]["col2"].ToString().Trim();
                        TxtMc.Value = dt.Rows[0]["col3"].ToString().Trim();
                        TxtDeptCode.Value = dt.Rows[0]["col4"].ToString().Trim();
                        TxtCost.Value = dt.Rows[0]["num5"].ToString().Trim();
                        txtDeptSup.Value = dt.Rows[0]["col6"].ToString().Trim();
                        TxtDescComp.Value = dt.Rows[0]["col1"].ToString().Trim();
                        TxtDescComp.Value = dt.Rows[0]["col20"].ToString().Trim();
                        TxtProbObs.Value = dt.Rows[0]["col5"].ToString().Trim();
                        TxtCorrAct.Value = dt.Rows[0]["remarks"].ToString().Trim();
                        TxtSpCons.Value = dt.Rows[0]["col7"].ToString().Trim();
                        Txtdept.Value = dt.Rows[0]["col18"].ToString().Trim();
                        TxtRemarks.Value = dt.Rows[0]["col13"].ToString().Trim();
                        TxtClosureDate.Value = dt.Rows[0]["col10"].ToString().Trim();
                        TxtClrHrs.Value = dt.Rows[0]["num1"].ToString().Trim();
                        TxtClrMins.Value = dt.Rows[0]["num2"].ToString().Trim();
                        Txthrs.Value = dt.Rows[0]["num3"].ToString().Trim();
                        TxtMins.Value = dt.Rows[0]["num4"].ToString().Trim();
                        TxtDownTime.Value = dt.Rows[0]["col8"].ToString().Trim();
                        TxtDowmMins.Value = dt.Rows[0]["col9"].ToString().Trim();
                        TxtComp.Value = dt.Rows[0]["col19"].ToString().Trim();
                        TxtRdProd.Value = dt.Rows[0]["col11"].ToString().Trim();
                        TxtCompType.Value = dt.Rows[0]["col12"].ToString().Trim();
                        TxtContAct.Value = dt.Rows[0]["col30"].ToString().Trim();
                        TxtPrevAct.Value = dt.Rows[0]["col31"].ToString().Trim();
                        TxtNature.Value = dt.Rows[0]["col25"].ToString().Trim();

                        string xclruserid = "";
                        xclruserid = dt.Rows[0]["chk_by"].ToString().Trim();
                        if (xclruserid.Length > 1)
                        {
                            TxtClr.Value = xclruserid;
                            chkclr.Checked = true;
                        }
                        else
                        {
                            TxtClr.Value = "-";
                            chkclr.Checked = false;
                        }

                        dt.Dispose();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString()).ToString("dd/MM/yyyy");
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                        //txtUserID.Disabled = true;

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CAM1", dt.Rows[0]["col28"].ToString().Trim());

                        SQuery = "Select a.Srno,a.icode,b.iname,b.irate,b.unit,a.col1,a.col2,a.sampqty,col3,col4 from " + frm_tabname1 + " a , item b where trim(a.icode)=trim(b.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='SS' AND  a.VCHNUM||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + col1 + "' and a.vchdate " + DateRange + " order by a.srno ";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                        ViewState["fstr"] = col1;
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_f1"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["unit"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["sampqty"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);

                            ViewState["sg1"] = sg1_dt;
                            fgen.EnableForm(this.Controls);
                            disablectrl();
                            setColHeadings();
                        }
                        sg1_add_blankrows();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        btnshift.Enabled = false;
                        btnDeptt.Enabled = false;
                        btnMc.Enabled = false;
                        BtnIncharge.Enabled = false;
                        ///ab kro check e h mane yaha ab kro grid me entry

                        TxtClosureDate.Value = DateTime.Now.ToString("yyyy-MM-dd");

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "disableFrame('div1');", true);
                    }
                    #endregion
                    break;
                case "DEPTT":
                    if (col1 == "") return;
                    TxtDeptCode.Value = col1;
                    Txtdept.Value = col2;
                    Txtdept.Focus();
                    break;
                case "Shift":
                    if (col1 == "") return;
                    TxtShiftCode.Value = col1;
                    TxtShift.Value = col2;
                    TxtShift.Focus();
                    break;
                case "MC":
                    if (col1 == "") return;
                    TxtMCCode.Value = col1;
                    TxtMc.Value = col2;
                    TxtMc.Focus();
                    break;
                case "Incharge":
                    if (col1 == "") return;
                    TxtInchCode.Value = col1;
                    TxtInch.Value = col2;
                    TxtInch.Focus();
                    break;
                case "Nature":
                    if (col1 == "") return;
                    TxtNatCode.Value = col1;
                    TxtNature.Value = col2;
                    TxtNature.Focus();
                    break;
                case "Comp":
                    if (col1 == "") return;
                    TxtCompType.Value = col2;
                    TxtCompType.Focus();
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
                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    SQuery = "select * from item where trim(icode)=" + col1 + " and length(Trim(icode))>4 order by icode asc";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = dt.Rows[0]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = dt.Rows[0]["iname"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = dt.Rows[0]["unit"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = "0";
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = dt.Rows[0]["irate"].ToString().Trim();
                    }
                    break;
                case "SG1_ROW_ADD":
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
                            sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                            sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        if (col1.Length > 8) SQuery = "select * from item where trim(icode) in (" + col1 + ") and length(Trim(icode))>4 order by icode asc";
                        else SQuery = "select * from item where trim(icode)=" + col1 + " and length(Trim(icode))>4 order by icode asc";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;

                            sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[d]["Unit"].ToString().Trim();
                            sg1_dr["sg1_t2"] = "0";
                            sg1_dr["sg1_t3"] = dt.Rows[d]["irate"].ToString().Trim();
                            sg1_dr["sg1_t4"] = "";
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    break;
                case "SG1_RMV":
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        i = 0;
                        for (i = 0; i < sg1.Rows.Count - 1; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = (i + 1);
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[3].Text.Trim();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[4].Text.Trim();

                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg1_add_blankrows();

                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                    }

                    setColHeadings();
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
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            // added 22/04/2020 :: VV
            SQuery = "select distinct col18 as Deptt,to_char(vchdate,'dd/mm/yyyy') as vchdate,vchnum||'  '||decode(trim(nvl(col22,'-')),'-','(Pending)','(Closed)') as Sheet_No ,col12 as Comp_type,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt,vchnum,num5 as Amt,type,col28 as img_src,to_char(vchdate,'yyyymmdd') as vdd from scratch where  branchcd='" + frm_mbr + "' and type='MN' and  vchdate " + PrdRange + " and vchnum<>'000000' order by vdd desc ,vchnum desc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevelIMG("List of " + lblheader.Text + "", frm_qstr);
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

            #region Number Gen and Save to Table
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                try
                {
                    oDS = new DataSet();
                    oporow = null;
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                    //oDs1 = new DataSet();
                    //oDs1 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);
                    // This is for checking that, is it ready to save the data
                    frm_vnum = "000000";
                    save_fun();


                    oDS.Dispose();
                    //oDs1.Dispose();
                    oporow = null;
                    oDS = new DataSet();
                    oDs1 = new DataSet();
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
                    oDs1 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);

                    if (edmode.Value == "Y")
                    {
                        save_it = "Y";
                        frm_vnum = TxtCardNo.Value;
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
                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vchnum) AS VCH FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='MN' and vchdate " + DateRange + " order by vchdate desc ", 6, "VCH");
                                //pk_error = fgen.chk_pk(frm_qstr,frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                if (i > 20)
                                {
                                    fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vchnum) AS VCH FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='MN' and vchdate " + DateRange + " order by vchdate desc ", 6, "VCH");
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
                    int xcountrows = 0;
                    xcountrows = sg1.Rows.Count;
                    if (sg1.Rows.Count > 1)
                    {
                        save_fun2();
                    }


                    if (edmode.Value == "Y")
                    {
                        cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd='" + frm_mbr + "' and type='MN' and vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "' and vchdate " + DateRange + "";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        if (sg1.Rows.Count > 1)
                        {
                            cmd_query = "update " + frm_tabname1 + " set branchcd='DD' where branchcd='" + frm_mbr + "' and type in ('MN','SS') and vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "' and vchdate " + DateRange + "";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }

                    }
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                    if (sg1.Rows.Count > 1)
                    {
                        fgen.save_data(frm_qstr, frm_cocd, oDs1, frm_tabname1);
                    }

                    if (edmode.Value == "Y")
                    {
                        cmd_query = "delete from " + frm_tabname + " where branchcd='DD' and type='MN' and  vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "' and vchdate " + DateRange + " ";
                        if (sg1.Rows.Count > 1)
                        {
                            cmd_query = "delete from " + frm_tabname1 + " where branchcd='DD' and type in ('MN','SS') and  vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "' and vchdate " + DateRange + " ";
                        }
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");



                    }
                    else
                    {
                        if (save_it == "Y")
                        {
                            //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                            fgen.msg("-", "AMSG", lblheader.Text + " " + " Saved Successfully ");
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
                    fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                    fgen.msg("-", "AMSG", ex.Message.ToString());
                    col1 = "N";
                }
            #endregion
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
        string acode = "";
        acode = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");

        string srno = "";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        srno = fgen.seek_iname(frm_qstr, frm_cocd, "select max(srno) as srno  from scratch", "srno");

        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = frm_mbr;
        oporow["vchnum"] = frm_vnum;
        oporow["vchdate"] = TxtDate.Value.ToString();
        oporow["Type"] = "MN";
        oporow["Srno"] = Convert.ToInt32(srno) + 1;
        oporow["acode"] = TxtShiftCode.Value.ToString();
        oporow["icode"] = "-";
        oporow["docdate"] = TxtdateComp.Value.ToString();
        oporow["col2"] = TxtMCCode.Value.ToString();
        oporow["col3"] = TxtMc.Value.ToString();
        oporow["col4"] = TxtDeptCode.Value.ToString();
        oporow["num5"] = TxtCost.Value.ToString().toDouble();
        oporow["col6"] = txtDeptSup.Value.ToString();
        oporow["col1"] = TxtDescComp.Value.ToString();
        oporow["col20"] = TxtDescComp.Value.ToString();
        oporow["col5"] = TxtProbObs.Value.ToString();
        oporow["remarks"] = TxtCorrAct.Value.ToString();
        oporow["col7"] = TxtSpCons.Value.ToString();
        oporow["col18"] = Txtdept.Value.ToString();
        oporow["col19"] = TxtComp.Value.ToString();
        oporow["col13"] = TxtRemarks.Value.ToString();
        oporow["col10"] = TxtClosureDate.Value.ToString();
        oporow["num1"] = TxtClrHrs.Value.ToString().toDouble();
        oporow["num2"] = TxtClrMins.Value.ToString().toDouble();
        oporow["num3"] = Txthrs.Value.ToString().toDouble();
        oporow["num4"] = TxtMins.Value.ToString().toDouble();
        oporow["col8"] = TxtDownTime.Value.ToString();
        oporow["col9"] = TxtDowmMins.Value.ToString();
        oporow["col11"] = TxtRdProd.Value.ToString();
        oporow["col12"] = TxtCompType.Value.ToString();
        oporow["col30"] = TxtContAct.Value.ToString();
        oporow["col31"] = TxtPrevAct.Value.ToString();
        oporow["col25"] = TxtNature.Value.Trim().ToUpper();

        if (TxtClr.Value.Length > 1)
        {
            oporow["chk_by"] = TxtClr.Value.Trim().ToUpper();
            oporow["chk_dt"] = DateTime.Today.ToShortDateString();
        }


        if (frm_cocd == "BLIS" || frm_cocd == "BASV")
        {
            if ((frm_cocd == "BLIS" || frm_cocd == "BASV") && (edmode.Value == "Y"))
            {
                oporow["col22"] = "Closed";
                oporow["col23"] = "-";
                oporow["col24"] = DateTime.Today.ToShortDateString();
            }
        }

        else
        {
            oporow["col22"] = "-";
            oporow["col23"] = "-";
            oporow["col24"] = DateTime.Today.ToShortDateString();
        }


        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_CAM1").Trim().Length > 1)
        {
            string cam_img = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CAM1").Trim().Substring(fgenMV.Fn_Get_Mvar(frm_qstr, "U_CAM1").Trim().ToUpper().IndexOf("UPLOAD"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_CAM1").Trim().Length - fgenMV.Fn_Get_Mvar(frm_qstr, "U_CAM1").Trim().ToUpper().IndexOf("UPLOAD"));
            oporow["col28"] = cam_img;
        }
        else
        {
            oporow["col28"] = "-";
        }


        if (edmode.Value == "Y")
        {
            oporow["eNt_by"] = ViewState["entby"].ToString();
            oporow["eNt_dt"] = ViewState["entdt"].ToString();
            oporow["edt_by"] = frm_uname;
            oporow["edt_dt"] = DateTime.Today.ToShortDateString();
        }
        else
        {
            oporow["eNt_by"] = frm_uname;
            oporow["eNt_dt"] = DateTime.Today.ToShortDateString();
            oporow["edt_by"] = "-";
            oporow["eDt_dt"] = DateTime.Today.ToShortDateString();
        }


        oDS.Tables[0].Rows.Add(oporow);


    }

    void save_fun2()
    {
        int xcount = 0;
        xcount = sg1.Rows.Count;
        if (xcount > 0)
        {
            oporow = oDs1.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = TxtDate.Value.ToString();
            oporow["Type"] = "MN";
            oporow["icode"] = "-";
            oporow["Srno"] = 1;
            oporow["col1"] = 1;
            oporow["col2"] = "-";
            oporow["sampqty"] = 0;
            oporow["col3"] = "-";
            oporow["col4"] = "-";
            //error coming here
            //((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text;
            oporow["ent_dt"] = DateTime.Today.ToShortDateString();
            oporow["ent_by"] = frm_uname;
            oDs1.Tables[0].Rows.Add(oporow);

            for (i = 0; i < sg1.Rows.Count - 1; i++)
            {
                //if (sg1.Rows[i].Cells[1].Text.Length > 2)
                //{
                oporow = oDs1.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = TxtDate.Value.ToString();
                oporow["Type"] = "SS";
                oporow["icode"] = sg1.Rows[i].Cells[3].Text.Trim();
                oporow["Srno"] = sg1.Rows[i].Cells[2].Text.Trim();
                oporow["col1"] = sg1.Rows[i].Cells[3].Text.Trim();
                oporow["col2"] = sg1.Rows[i].Cells[4].Text.Trim();
                oporow["sampqty"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text);
                oporow["col3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text;
                oporow["col4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text;
                oporow["ent_dt"] = DateTime.Today.ToShortDateString();
                oporow["ent_by"] = frm_uname;
                oDs1.Tables[0].Rows.Add(oporow);



                //}
            }
        }
    }
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F99245X":
                SQuery = "SELECT 'MN' AS FSTR,'Complaint Form' as NAME,'MN' AS CODE FROM dual";
                break;

        }
    }


    //------------------------------------------------------------------------------------   
    protected void btnDept_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DEPTT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Department", frm_qstr);
    }
    //btnComp_Click
    protected void btnShift_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Shift";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Shift", frm_qstr);
    }
    protected void btnComp_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Comp";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Comp. Type", frm_qstr);
    }
    protected void btnNature_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Nature";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type of Expense", frm_qstr);
    }
    protected void btnIncharge_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Incharge";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Incharge", frm_qstr);
    }
    protected void btnMc_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MC";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Machine", frm_qstr);
    }
    protected void btnMulERP_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MERP";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Multipbtnsavele Module", frm_qstr);
    }
    protected void btnMplant_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MPLANT";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select Plant", frm_qstr);
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
                    if (sg1.Rows[sg1r].Cells[j].Text.Trim().Length > 50)
                    {
                        sg1.Rows[sg1r].Cells[j].Text = sg1.Rows[sg1r].Cells[j].Text.Substring(0, 50);
                    }
                }
            }
        }
    }

    protected void sg1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes["onkeydown"] = "if (event.keyCode != 13) { javascript:return SelectSibling(event); }";
            //e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }

    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        if (TxtCardNo.Value == "-")
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
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;


            case "SG1_ROW_ADD":

                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Items", frm_qstr); // CHANGE ITEM TO ITEMS BY MADHVI ON 23 JULY 2018
                    //fgen.Fn_open_mseek("Select Item", frm_qstr);
                }
                break;
        }
    }


    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        // Hidden Field

        sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(Int32)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));

    }
    //------------------------------------------------------------------------------------


    public void sg1_add_blankrows()
    {
        sg1_dr = sg1_dt.NewRow();

        sg1_dr["sg1_SrNo"] = sg1_dt.Rows.Count + 1;
        sg1_dr["sg1_f1"] = "-";
        sg1_dr["sg1_f2"] = "-";
        sg1_dr["sg1_t1"] = "-";
        sg1_dr["sg1_t2"] = "-";
        sg1_dr["sg1_t3"] = "-";
        sg1_dr["sg1_t4"] = "-";

        sg1_dt.Rows.Add(sg1_dr);
    }
    //------------------------------------------------------------------------------------

    void newCase(string vty)
    {
        #region
        vty = "MN";
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        disablectrl();
        fgen.EnableForm(this.Controls);


        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();


        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        // Popup asking for Copy from Older Data
        //fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        //hffield.Value = "NEW_E";

        set_Val();
        #endregion
    }

    // added 22/04/2020 :: VV
    protected void btnCamera_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        hffield.Value = "";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL10", frm_mbr + frm_vty + TxtCardNo.Value + Convert.ToDateTime(TxtDate.Value).ToString("dd_MM_yyyy"));
        fgen.open_sseek_camera("", frm_qstr);
    }
    protected void chkclr_CheckedChanged(object sender, EventArgs e)
    {
        if (chkclr.Checked == true)
        {
            TxtClr.Value = frm_uname;
        }
        else
        {
            TxtClr.Value = "-";
        }
    }
}