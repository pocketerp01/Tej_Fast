using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;

using System.IO;

public partial class om_finsys_options : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", remv_02 = "";
    DataTable dt, dt2, dt3, dt4;
    DataRow oporow; DataSet oDS, oDs1;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr; DataRow dr; DataRow dr1; DataRow dr2; DataRow dr3;
    DataTable sg3_dt; DataRow sg3_dr; DataTable dt5; DataTable dt6; DataTable dt7; DataTable dt8; DataTable dt9; DataTable dt10; DataTable dt11; DataTable dt12; DataTable dt13; DataTable dt14;
    DataTable dtCol = new DataTable();
    DataSet ds;
    string Checked_ok;
    string save_it;
    string Prg_Id;

    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_tabname1, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
    string m4 = DateTime.Today.Year + "04";
    string m5 = DateTime.Today.Year + "05";
    string m6 = DateTime.Today.Year + "06";
    string m7 = DateTime.Today.Year + "07";
    string m8 = DateTime.Today.Year + "08";
    string m9 = DateTime.Today.Year + "09";
    string m10 = DateTime.Today.Year + "10";
    string m11 = DateTime.Today.Year + "11";
    string m12 = DateTime.Today.Year + "12";
    string m1 = DateTime.Today.Year + 0 + "01";
    string m2 = DateTime.Today.Year + 0 + "02";
    string m3 = DateTime.Today.Year + 0 + "03";
    //double double_val2, double_val1;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            //btnnew.Focus();
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
                    //DateRange = "between to_date('01/04/2019','dd/mm/yyyy') and to_date('30/07/2020','dd/mm/yyyy')";
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                //doc_addl.Value = "0";

                fgen.DisableForm(this.Controls);
                enablectrl();
                //getColHeading();
            }
            //setColHeadings();
            set_Val();
            Fill_sg1();

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
            //getColHeading();
        }
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        if (dtCol == null) return;

        // to hide and show to tab panel
        //tab5.Visible = false;
        //tab4.Visible = false;
        //tab3.Visible = false;
        //tab2.Visible = false;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "*******":
                //tab3.Visible = false;
                //tab4.Visible = false;
                break;
        }
        if (Prg_Id == "*******")
        {
            //tab5.Visible = true;
        }
        //lblheader.Text = "Delivery Status";
        //btnprint.Visible = true;
        //btnlist.Visible = true;
        //btnTraExc.Visible = true;
        //if (frm_cocd == "MSES") divCan.Visible = false;
        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {

        // btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        //btnlist.Disabled = false; btnTraExc.Disabled = false;

        create_tab();
        create_tab1();
        create_tab2();

        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();

        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();

    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {

        btnhideF.Enabled = true; btnhideF_s.Enabled = true; //btnexit.Visible = false; btncancel.Visible = true;
        //btnCamera.Disabled = false;
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
        frm_tabname = "enq_mast";
        frm_vty = "20";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        typePopup = "N";
    }
    //------------------------------------------------------------------------------------

    public void Fill_sg1()
    {

    }
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
                SQuery = "select NAme,type1 from typegrp where id='A' and type1 like '16%' order by Type1";
                break;
            case "SALEXY":
                SQuery = "select TYPE1 AS FSTR, NAme,type1 from typegrp where id='A' and type1 like '16%' order by Type1";
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
            case "Zone":
                //pop1
                SQuery = "select '1' as fstr,'North' as name,'1' as Code  from dual union all select '2' as fstr,'South' as name,'2' as Code from dual union all select '3' as fstr,'East'  as name,'3' as code from dual union all select '4' as fstr,'West' as name,'4' as code from dual union all select '5' as fstr,'Central' as name,'5' as code from dual union all select '6' as fstr,'n/a' as name,'6' as code from dual";
                break;
            case "Client":
                SQuery = "select acode as fstr , aname as name , acode as acode,addr1,addr2,addr3,email,Telnum from famst where branchcd='" + frm_mbr + "' and substr(acode,1,2) in ('16') order by aname";
                break;
            case "Enq":
                SQuery = "select type1 as fstr,name,type1 as code from type where id='@' and substr(type1,1,1) in('0') order by type1 asc ";
                break;
            case "Priority":
                SQuery = "select 'High' as fstr,'High' as name,'High' as Code from dual union all select 'Medium' as fstr,'Medium' as name,'Medium' as Code from dual union all select 'Low' as fstr,'Low' as name,'Low' as Code from dual ";
                break;
            case "Dom":
                SQuery = "select 'DOM' as fstr,'DOM' as name,'DOM' as Code from dual union all select 'EXP' as fstr,'EXP' as name,'EXP' as Code from dual ";
                break;
            case "TICODE":
                //pop2
                SQuery = "SELECT ACODE AS FSTR,ANAME AS PARTY,ACODE AS CODE,ADDR1,ADDR2 FROM CSMST where branchcd!='DD' ORDER BY aname ";
                //SQuery = "SELECT ICODE AS FSTR,INAME AS PRODUCT,ICODE AS CODE,UNIT,CPARTNO AS PARTNO FROM ITEM WHERE LENGTH(tRIM(ICODE))>4 ";
                break;
            case "Person":
                SQuery = "SELECT empcode as fstr,name,desg_text,deptt_text from empmas order by name";
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                SQuery = "select distinct icode as fstr,iname as name,cpartno,cdrgno from item where branchcd='" + frm_mbr + "' and substr(trim(icode),1,2) in ('90') order by name asc";
                break;
            case "New":
            case "List":
            case "Edit*":
            case "Del*":
            case "Print":
                SQuery = "select distinct vchnum||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy') as Dated,Client,ent_by,ent_dt from enq_mast where vchdate between to_date('01/01/2020','dd/mm/yyyy') and to_date('31/12/2020','dd/mm/yyyy') and type='20' and branchcd='" + frm_mbr + "' and vchnum<>'000000'  order by vchnum desc";
                break;
            case "MPLANT":
                SQuery = "Select type1 as fstr,name, type1 as code from type where ID='B' order by type1";
                break;
            case "CompTyp":
                SQuery = "Select type1 as fstr,name from type where ID='(' ";
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
                    SQuery = "select distinct a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') as fstr, a.vchnum||'  '||decode(trim(a.pflag),0,'(Closed)',' ') as enq_no,a.vchdate as Dated,a.Client,a.epriority as EStatus,a.Item,a.icode,A.ENT_BY,A.ENT_dT,a.vchnum,a.tcol24 as etype  from " + frm_tabname + " a where  type='20' and branchcd='" + frm_mbr + "' and vchnum<>'000000' order by vchdate desc ,vchnum desc";
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

            //chkclr.Checked = false;
            //TxtClr.Value = "-";


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
            fgen.Fn_open_sseek("Select Complaint No Edit", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");

        //chkclr.Checked = false;
        //TxtClr.Value = "-";

    }
    //------------------------------------------------------------------------------------
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
        create_tab1();
        create_tab2();

        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();


        ViewState["sg1"] = null;
        ViewState["sg2"] = null;
        ViewState["sg3"] = null;

        //setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";

        fgen.Fn_open_prddmp1("Select Period", frm_qstr);


    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        //fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr);
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
                SQuery = "delete from " + frm_tabname + " a where branchcd='" + frm_mbr + "' and type='20' and a.vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                //fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), DateTime.Now.ToString("dd/MM/yyyy"), frm_uname, "US", lblheader.Text.Trim() + " Deleted");
                //fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
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


        else if (hffield.Value == "Show")
        {

            col2 = Request.Cookies["reply"].Value.ToString();
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", col2);

        }

        else if (hffield.Value == "DEBTAGE")
        {
            SQuery = "Select type1 as fstr, Name,Type1 from typegrp where branchcd!='DD' and id='A' and substr(type1,1,2)='16' order by type1";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("-", frm_qstr);
            hffield.Value = "DEBTAGEE";
        }

        else if (hffield.Value == "CREDITAGE")
        {
            SQuery = "Select type1 as fstr, Name,Type1 from typegrp where branchcd!='DD' and id='A' and substr(type1,1,2) in ('05','06') order by type1";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("-", frm_qstr);
            hffield.Value = "CREDITAGEE";
        }

        else if (hffield.Value == "DEBTAGEE")
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = Request.Cookies["REPLY"].Value.ToString().Trim();
            SQuery = "select m.aname as Party,m.ADDR1 as Address,to_char(sum(n.total)) as Total_Outstanding,to_char(sum(n.slab1)) as Current_Os,to_char(sum(n.slab2)) as OVER_30_60,to_char(sum(n.slab3)) as OVER_61_90,to_char(sum(n.slab4)) as OVER_90_180,to_char(sum(n.slab5)) as OVER_181,n.acode,sum(n.total) as totos,sum(n.slab1) as s1,sum(n.slab2) as s2,sum(n.slab3) as s3,sum(n.slab4) as s4,sum(n.slab5) as s5,m.Payment as P_days,m.Climit  as Cr_limit,m.Zcode from (SELECT acode,dramt-cramt as total,(CASE WHEN (sysdate-invdate BETWEEN 0 AND 30) THEN dramt-cramt END) as slab1  ,(CASE WHEN (sysdate-invdate BETWEEN 30 AND 60) THEN dramt-cramt END) as slab2,(CASE WHEN (sysdate-invdate BETWEEN 60 AND 90) THEN dramt-cramt END) as slab3,(CASE WHEN (sysdate-invdate BETWEEN 90 AND 180) THEN dramt-cramt END) as slab4,(CASE WHEN (sysdate-invdate > 180) THEN dramt-cramt END) as slab5 from  recdata) n left outer join famst m on trim(n.acode)=trim(m.acode) where substr(n.acode,1,2) in ('16') and n.total<>0 and m.bssch like '" + col1 + "%' and m.zcode like '%%' group by m.aname,m.addr1,m.climit,m.payment,n.acode,m.zcode having sum(n.total)>0";
            if (col2 == "Y")
            {
                SQuery = "Select * from (" + SQuery + ") where totos>0 and (nvl(s4,0)+nvl(s5,0))>0 order by s4 desc";
            }
            else if (col2 == "N")
            {
                SQuery = "Select * from (" + SQuery + ") where totos>0 and (nvl(s1,0)+nvl(s2,0))>0 order by s2 desc";
            }
            else
            {
                SQuery = "Select * from (" + SQuery + ") where totos>0 order by Party";
            }
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Debtors Ageing As on " + DateTime.Today.ToShortDateString() + "", frm_qstr);
            hffield.Value = "-";
        }

        else if (hffield.Value == "SALEAGENTWISE")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            string cons = "";
            if (col1 == "Y")
            {
                cons = " A.BRANCHCD<>'DD'";
            }
            else
            {
                cons = " A.BRANCHCD='" + frm_mbr + "'";
            }
            SQuery = "select /*+ INDEX_DESC(sale ind_SAL_BRTYDT) */ to_Char(vchdate,'YYYY MONTH') as Month_Name,to_char(sum(amt_sale)) as Basic_Value,to_char(sum(Bill_tot)) as Gross_Value,to_Char(vchdate,'YYYYMM') as mth from sale where vchdate " + DateRange + " and branchcd not in ('88','DD') and type not in ('47') and upper(trim(nvl(thru,'-')))='" + col2.Trim().ToUpper() + "' group by to_Char(vchdate,'YYYYMM') ,to_Char(vchdate,'YYYY MONTH') order by to_Char(vchdate,'YYYYMM')";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Monthly Details of Sales for " + col2.Trim().ToUpper() + "", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "CREDITAGEE")
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = Request.Cookies["REPLY"].Value.ToString().Trim();
            SQuery = "select m.aname as Party,m.ADDR1 as Address,to_char(sum(n.total)) as Total_Outstanding,to_char(sum(n.slab1)) as Current_Os,to_char(sum(n.slab2)) as OVER_30_60,to_char(sum(n.slab3)) as OVER_61_90,to_char(sum(n.slab4)) as OVER_90_180,to_char(sum(n.slab5)) as OVER_181,n.acode,sum(n.total) as totos,sum(n.slab1) as s1,sum(n.slab2) as s2,sum(n.slab3) as s3,sum(n.slab4) as s4,sum(n.slab5) as s5 from (SELECT acode,dramt-cramt as total,(CASE WHEN (sysdate-invdate BETWEEN 0 AND 30) THEN dramt-cramt END) as slab1  ,(CASE WHEN (sysdate-invdate BETWEEN 30 AND 60) THEN dramt-cramt END) as slab2,(CASE WHEN (sysdate-invdate BETWEEN 60 AND 90) THEN dramt-cramt END) as slab3,(CASE WHEN (sysdate-invdate BETWEEN 90 AND 180) THEN dramt-cramt END) as slab4,(CASE WHEN (sysdate-invdate > 180) THEN dramt-cramt END) as slab5 from  recdata) n left outer join famst m on n.acode=m.acode where substr(n.acode,1,2) in ('05','06') and m.bssch like '" + col1 + "%' group by m.aname,m.addr1,n.acode HAVING sum(n.total)<>0 ORDER BY M.ANAME";
            if (col2 == "Y")
            {
                SQuery = "Select * from (" + SQuery + ") where totos<0 and (nvl(s4,0)+nvl(s5,0))<0 order by s4 ";
            }
            else if (col2 == "N")
            {
                SQuery = "Select * from (" + SQuery + ") where totos<0 and (nvl(s1,0)+nvl(s2,0))<0 order by s2 ";
            }
            else
            {
                SQuery = "Select * from (" + SQuery + ") where totos<0  order by Party";
            }
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Creditors Ageing As on " + DateTime.Today.ToShortDateString() + "", frm_qstr);
            hffield.Value = "-";
        }

        else if (hffield.Value == "MML")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            if (col1.Length < 3)
            {
                SQuery = "select b.iname,b.cpartno,a.issued,b.unit,b.irate,a.icode,b.deac_Dt,b.ent_by,b.ent_Dt from (select trim(icode)as icode,sum(bom) as gg,sum(iqtyout) as issued from (select icode,0 as bom,iqtyout from ivoucher where branchcd='" + frm_mbr + "' and type like '3%' and vchdate " + PrdRange + " and store='Y' union all select distinct icode,1 as bom,0 as iqtyout from itembal where branchcd='" + frm_mbr + "' and nvl(imin,0)+nvl(imax,0)>0) group by trim(icode) having sum(bom)=0 ) a, item b where trim(a.icode)=trim(B.icodE) and length(Trim(b.icode))>4 and a.issued>0 order by a.issued desc,B.iname ";
            }
            else
            {
                SQuery = "select b.iname,b.cpartno,a.issued,b.unit,b.irate,a.icode,b.deac_Dt,b.ent_by,b.ent_Dt from (select trim(icode)as icode,sum(bom) as gg,sum(iqtyout) as issued  from (select icode,0 as bom,iqtyout from ivoucher where branchcd='" + frm_mbr + "' and type like '3%' and vchdate " + PrdRange + " and store='Y' union all select distinct icode,1 as bom,0 as iqtyout from itembal where branchcd='" + frm_mbr + "' and nvl(imin,0)+nvl(imax,0)>0) group by trim(icode) having sum(bom)=0 ) a, item b where trim(a.icode)=trim(B.icodE) and length(Trim(b.icode))>4 and a.issued>0 and substr(a.icode,1,2) in (" + col1 + ") order by a.issued desc,B.iname ";
            }

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Issued Items without Min/Max ( For " + frm_mbr + ") " + PrdRange + "", frm_qstr);
            hffield.Value = "-";

        }

        else if (hffield.Value == "PORI")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            //col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            SQuery = "Select A.Orddt,B.aname as Supplier,C.iname as Item,A.Qtyord as Qty_ord,(case when round(1*(a.prate*((100-a.pdisc)/100)),4)>A.Nxtmth then 'Increase' else 'Decrease' END) as Status, round(nvl(a.wk3,1)*(a.prate*((100-a.pdisc)/100)),4) as Net_PO_Rate,round(nvl(a.wk3,1)*A.Nxtmth,3) as Old_Rate,a.type,a.rate_diff as Reason,A.Ordno,A.Ent_By,A.App_by from pomas a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.Icode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,2) in ('50','51') and a.orddt " + PrdRange + " and  round(1*(a.prate*((100-a.pdisc)/100)),4)<>nvl(a.nxtmth,0) and nvl(a.nxtmth,0)<>0 and a.type not in ('52','53') and substr(a.icode,1,2)!='59' order by a.orddt,a.ordno";
            SQuery = "Select Supplier,Item,Orddt,Reason,Status, Net_PO_Rate,Old_Rate,Net_PO_Rate-Old_Rate as Diff,to_char((Case when Old_Rate>0 then round(((Net_PO_Rate-Old_Rate)/Old_Rate)*100,2) else 0 end),'9999.99') as Percentg,Qty_ord,Ordno,type,Ent_By,App_by from (" + SQuery + ") order by Qty_ord desc,orddt,ordno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("PO Type 50,51 Items With Rate Increase / Decrease ( For " + frm_mbr + ") " + PrdRange + "", frm_qstr);
            hffield.Value = "-";

        }

        else if (hffield.Value == "CUSTWISESALE")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            if (frm_cocd == "SAPL")
            {
                SQuery = "select b.Iname as Item,to_char(sum(a.Iqtyout)) as Quantity,to_char(sum(a.iamount)) as Basic_Value,to_char(round(sum(a.iamount)/sum(decode(a.iqtyout,0,1,a.Iqtyout)),2),'9,99,999.99') as Avg_Rate,a.icode from ivoucher a left outer join item b on a.icode=b.icode where a.type<>'47' and trim(a.acode)='" + col1 + "' and a.branchcd='" + frm_mbr + "' group by b.iname,a.icode order by b.iname";
            }
            else
            {
                SQuery = "select b.Iname as Item,to_char(sum(a.Iqtyout)) as Quantity,to_char(sum(a.iamount)) as Basic_Value,to_char(round(sum(a.iamount)/sum(decode(a.iqtyout,0,1,a.Iqtyout)),2),'9,99,999.99') as Avg_Rate,a.icode from ivoucher a left outer join item b on a.icode=b.icode where a.type<>'47' and trim(a.acode)='" + col1 + "' and a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,1)='4' group by b.iname,a.icode order by b.iname";
            }
            if (remv_02 == "Y")
            {
                SQuery = "select b.Iname as Item,to_char(sum(a.Iqtyout)) as Quantity,to_char(sum(a.iamount)) as Basic_Value,to_char(round(sum(a.iamount)/sum(decode(a.iqtyout,0,1,a.Iqtyout)),2),'9,99,999.99') as Avg_Rate,a.icode from ivoucher a left outer join item b on a.icode=b.icode where a.type<>'47' and trim(a.acode)='" + col1 + "' and a.branchcd!='DD' and a.vchdate " + PrdRange + " and substr(a.type,1,1)='4' group by b.iname,a.icode order by b.iname";
            }

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Details of Sales", frm_qstr);
            hffield.Value = "-";
        }

        else if (hffield.Value == "VIPL")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            if (col1.Length < 3)
            {
                SQuery = "select b.aname,b.addr1,a.acode,b.ent_by,b.ent_Dt from (select trim(acode)as acode,sum(bom) as gg from (select distinct acode,1 as bom from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt " + PrdRange + " union all select distinct acode,-1 as bom from appvendvch where branchcd='" + frm_mbr + "' ) group by trim(acode) having sum(bom)=1 ) a, famst b where substr(a.acode,1,2) in ('05','06') and trim(a.acode)=trim(B.acodE) and length(Trim(b.acode))>4 order by B.aname ";
            }
            else
            {
                SQuery = "select b.aname,b.addr1,a.acode,b.ent_by,b.ent_Dt from (select trim(acode)as acode,sum(bom) as gg from (select distinct acode,1 as bom from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt " + PrdRange + " union all select distinct acode,-1 as bom from appvendvch where branchcd='" + frm_mbr + "' ) group by trim(acode) having sum(bom)=1 ) a, famst b where substr(a.acode,1,2) in (" + col1 + ") and trim(a.acode)=trim(B.acodE) and length(Trim(b.acode))>4 order by B.aname ";
            }

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("PO Vendors without Approved Price List ( For " + frm_mbr + ") " + PrdRange + "", frm_qstr);
            hffield.Value = "-";

        }
        else if (hffield.Value == "INSP")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            string addl_Con = "";
            if (col1.Length < 3)
            {
                addl_Con = "1=1";

            }
            else
            {
                addl_Con = "substr(a.icode,1,2) in (" + col1 + ")";

            }

            SQuery = "select b.iname,b.cpartno,a.sal_Qty as Purch_Qty,b.unit,b.cdrgno,a.icode,b.ent_by,b.ent_Dt from (select trim(icode)as icode,sum(bom) as gg,sum(sal_Qty) as sal_Qty from (select icode,0 as bom,sum(iqtyin) as sal_Qty from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and type!='04' and vchdate " + PrdRange + " and store='Y' group by icode union all select distinct icode,1 as bom,0 as salqty from inspmst where branchcd='" + frm_mbr + "' and type='20') group by trim(icode) having sum(bom)=0 ) a, item b where trim(a.icode)=trim(B.icodE) and " + addl_Con + " and length(Trim(b.icode))>4 order by a.sal_Qty desc,B.iname ";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Items Purchase without Inspection Templates ( For " + frm_mbr + ") " + PrdRange + "", frm_qstr);
            hffield.Value = "-";

        }

        else if (hffield.Value == "INSPITEM")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            string addl_Con = "";
            if (col1.Length < 3)
            {
                addl_Con = "1=1";

            }
            else
            {
                addl_Con = "substr(a.icode,1,2) in (" + col1 + ")";

            }

            SQuery = "select b.iname,b.cpartno,a.sal_Qty as Issued_Qty,b.unit,b.cdrgno,a.icode,b.ent_by,b.ent_Dt from (select trim(icode)as icode,sum(bom) as gg,sum(sal_Qty) as sal_Qty from (select icode,0 as bom,sum(iqtyout) as sal_Qty from ivoucher where branchcd='" + frm_mbr + "' and type like '3%' and type!='36' and vchdate " + PrdRange + " and store='Y' group by icode union all select distinct icode,1 as bom,0 as salqty from inspvch where branchcd='" + frm_mbr + "' and type='20') group by trim(icode) having sum(bom)=0 ) a, item b where trim(a.icode)=trim(B.icodE) and " + addl_Con + " and length(Trim(b.icode))>4 order by a.sal_Qty desc,B.iname ";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Items Issued without Inspection Templates ( For " + frm_mbr + ") " + PrdRange + "", frm_qstr);
            hffield.Value = "-";

        }

        else if (hffield.Value == "STOCKSTATUS")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            string xprd1 = "between to_date('" + Convert.ToDateTime(frm_CDT1).AddYears(-1).ToShortDateString() + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(todt).AddYears(-1).ToShortDateString() + "','dd/mm/yyyy')";
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            string addl_Con = "";
            if (col1.Length < 3)
            {
                addl_Con = "1=1";

            }
            else
            {
                addl_Con = "substr(a.icode,1,2) in (" + col1 + ")";

            }

            string mq0 = "select b.iname,a.icode,sum(a.opening) as Opening,sum(a.cdr) as Inwards,sum(a.ccr) as Outwards,sum(opening)+sum(cdr)-sum(ccr) as closing,max(avgcons) as Avg_Day_Cons,b.unit,b.cpartno,substr(a.icode,1,4) As sub_Grp from (Select icode, " + DateTime.Today.Year + " as opening,0 as cdr,0 as ccr,0 as clos,0 as avgcons from itembal where branchcd='" + frm_mbr + "' union all  ";
            string mq1 = "select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as avgcons from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd1 + " and store='Y' GROUP BY ICODE union all ";
            string mq2 = "select icode,0 as op,0 as cdr,0 as ccr,0 as clos,round((sum(iqtyout)-sum(iqtyin))) as avgcons from ivoucher where branchcd='" + frm_mbr + "' and substr(type,1,1) in ('1','2','3','4') and type!='36' and vchdate " + PrdRange + " and store='Y' GROUP BY ICODE union all ";
            string mq3 = "select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as avgcons from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + PrdRange + " and store='Y' GROUP BY ICODE ) a, item b where trim(A.icode)=trim(B.icode) and substr(a.icode,1,1)<'8' and " + addl_Con + " group by b.iname,b.unit,b.cpartno,a.icode,substr(a.icode,1,4) having sum(a.opening)+sum(a.cdr)-sum(a.ccr)>0 ";
            SQuery = mq0 + mq1 + mq2 + mq3;


            SQuery = "select Iname,Opening,Inwards,Outwards,Closing,Avg_Day_Cons,(Case when closing>0 and Avg_Day_Cons>0 then round(closing/Avg_Day_Cons,0) else 999 end) as Days_Stock,Unit,Icode,Cpartno,Sub_Grp from (" + SQuery + ") order by Days_Stock desc,Iname";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Stock Summary (Needing Attention ) " + PrdRange + "", frm_qstr);
            hffield.Value = "-";

        }

        else if (hffield.Value == "SALEXY")
        {
            hffield.Value = "SALEXY";
            fgen.Fn_open_prddmp1("-", frm_qstr);
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", col1);
        }
        else if (hffield.Value == "PURNOTISSUE")
        {
            col1 = Request.Cookies["reply"].Value.ToString();
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "Select C.aname as Vendor,b.Iname,a.iqtyin,a.irate,a.iqtyin*a.irate as Amount,a.vchdate,a.icode,round(sysdate-a.vchdate,0) as Days_old,a.vchnum,a.ent_by,a.ponum,a.podate,a.type,a.potype from ivoucher a,item b,famst c where trim(A.acode)=trim(c.acode) and trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type in('02','05','07','0U') and vchdate between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(todt).AddDays(Convert.ToInt32(col1)).ToShortDateString() + "','dd/mm/yyyy') order by a.vchdate,b.Iname";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Purchased but Not Issued between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(todt).AddDays(Convert.ToInt32(col1)).ToShortDateString() + "','dd/mm/yyyy')", frm_qstr);
            hffield.Value = "-";
        }

        else if (hffield.Value == "DOCVOL")
        {
            col1 = Request.Cookies["reply"].Value.ToString();
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            if (col1 == "Y")
            {
                SQuery = "Select distinct vchnum||vchdate as fstr, branchcd,vchnum||vchdate as vchnum,type,ent_by from voucher where branchcd not in ('88','DD') and ENT_DATE " + PrdRange + " and type not like '4%'";
            }
            else
            {
                SQuery = "Select distinct vchnum||vchdate as fstr, branchcd,vchnum||vchdate as vchnum,type,ent_by from voucher where branchcd ='" + frm_mbr + "' and ENT_DATE " + PrdRange + " and type not like '4%'";
            }

            SQuery = "Select a.branchcd||a.Type||a.Ent_By as fstr, B.Name,A.Type,count(a.Vchnum) as Entries,a.Ent_by,c.name as br_name from (" + SQuery + ") a ,type b,type c where b.id='V' and c.id='B' and a.type=b.type1 and a.branchcd=c.type1 group by c.name,a.branchcd,b.name,a.type,a.ent_by order by a.type";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("", frm_qstr);
            //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            //fgen.Fn_open_rptlevel("-", frm_qstr);
            //fgen.Fn_open_rptlevel("Purchased but Not Issued between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(todt).AddDays(Convert.ToInt32(col1)).ToShortDateString() + "','dd/mm/yyyy')", frm_qstr);
            hffield.Value = "DOCVOLSHOW";
        }
        else if (hffield.Value == "DOCVOLSHOW")
        {
            col1 = Request.Cookies["reply"].Value.ToString();
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            if (col1 == "Y")
            {
                SQuery = "Select distinct vchnum||vchdate as fstr, branchcd,vchnum||vchdate as vchnum,type,ent_by from voucher where branchcd not in ('88','DD') and ENT_DATE " + PrdRange + " and type not like '4%'";
            }
            else
            {
                SQuery = "Select distinct vchnum||vchdate as fstr, branchcd,vchnum||vchdate as vchnum,type,ent_by from voucher where branchcd ='" + frm_mbr + "' and ENT_DATE " + PrdRange + " and type not like '4%'";
            }

            SQuery = "Select B.Name,A.Type,d.aname,a.dramt,a.cramt,a.naration,a.Ent_by,a.vchdate,a.vchnum,c.name as br_name,a.branchcd from voucher a ,type b,type c,famst d where trim(a.acodE)=trim(d.acode) and b.id='V' and c.id='B' and a.type=b.type1 and a.branchcd=c.type1 and a.branchcd||a.type||a.ent_by='" + col2 + "' and a.vchdate " + PrdRange + " order by a.vchdate,a.type,a.vchnum,a.srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("-", frm_qstr);
            hffield.Value = "";
        }
        else if (hffield.Value == "LATEHR")
        {
            col1 = Request.Cookies["reply"].Value.ToString();
            if (col1 == "0")
            {
                col1 = "17";
            }
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            string party_cd = "";
            string part_cd = "";
            string txtitemgrp = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1");
            string txtitemsubgrp = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR2");
            string txtitemstart = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3");
            string txtitemend = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4");

            string txtpartygrp = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR5");
            string txtpartysubgrp = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR6");
            string txtpartystart = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR7");
            string txtpartyend = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR8");

            if (txtpartystart.Trim().Length <= 1)
            {
                party_cd = "LIKE '%%'";
            }
            else
            {
                party_cd = "between '" + txtpartystart + "' and '" + txtpartyend + "'";
            }
            if (txtitemstart.Trim().Length <= 1)
            {
                part_cd = "LIKE '%%'";
            }
            else
            {
                part_cd = "between '" + txtitemstart + "' and '" + txtitemend + "'";
            }

            SQuery = "Select to_char(a.vchdate,'dd/mm/yyyy') as Dated,a.Vchnum as GE_No,b.aname as Supplier,trim(a.invno)||','||trim(a.refnum) as Bill_Chl,c.iname as Item_Name,a.mtime as Time_In,a.iqty_chl as Qty,c.unit,c.cpartno as Code,a.ponum as P_O_No,a.Ent_by,a.ent_dt,a.Icode from ivoucherp a, famst b , item c where a.branchcd='" + frm_mbr + "' and substr(A.type,1,2) like '00' and a.vchdate " + PrdRange + "  and substr(a.mtime,1,2)>='" + col1.ToString().Trim().PadRight(2, '0') + "' and TRIM(a.ICODE)=trim(c.icode) and  TRIM(A.ACODE)=TRIM(b.acode) and TRIM(a.ACODE) " + party_cd + " and TRIM(A.icode)  " + part_cd + " order by vchdate,type,vchnum,srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("-", frm_qstr);
            hffield.Value = "";
        }

        else
        {
            string rely = "";
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

            col1 = Request.Cookies["reply"].Value.ToString();
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", col1);

            btnval = hffield.Value;
            switch (btnval)
            {
                case "List":

                    fgen.msg("-", "PMSG", "Select 1 for Hours and 2 One Shift and 3 for Two Shift!");
                    hffield.Value = "Show";

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
                    //fgen.Fn_open_sseek("Select Entry No to Delete", frm_qstr);                    
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
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "P70106H");
                    fgen.fin_maint_reps(frm_qstr);
                    break;
                case "Edit":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.* from " + frm_tabname + " a where a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='20' AND  a.VCHNUM||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + col1 + "'  ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {

                        dt.Dispose();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString()).ToString("dd/MM/yyyy");
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        //setColHeadings();
                        edmode.Value = "Y";



                        create_tab();

                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_f1"] = dt.Rows[i]["Item"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["ItemCat"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["sub_cat"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["qtyord"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["irate"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["vat"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["excise"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["Freight"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["Total"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["remarks"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);

                            fgen.EnableForm(this.Controls);
                            disablectrl();
                            //setColHeadings();

                            ViewState["sg1"] = sg1_dt;
                            fgen.EnableForm(this.Controls);
                            disablectrl();
                            //setColHeadings();
                        }



                        sg1_dt.Dispose();


                        // ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                    }
                    #endregion
                    break;
                case "Client":


                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "CustomerComp", "CustomerComp");
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
            fgen.msg("-", "CMSG", "Select Yes for Open Jobs and No for Pend. SO.!");

        }

        else if (hffield.Value == "SALEVSCOL")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select Month_Name,to_char(sum(Sales)) as Sales,to_char(sum(Collection)) as Collection,mth from (select to_Char(a.vchdate,'YYYY MONTH') as Month_Name,sum(a.Bill_tot) as Sales,0 as Collection,to_Char(a.vchdate,'YYYYMM') as mth from sale a where a.vchdate " + PrdRange + " and a.branchcd!='DD' and a.type<>'47' and substr(A.acode,1,2)!='02' group by to_Char(a.vchdate,'YYYYMM') ,to_Char(a.vchdate,'YYYY MONTH') union all select to_Char(a.vchdate,'YYYY MONTH') as Month_Name,0 as Gross_Value,sum(cramt)-sum(dramt) as collection,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.branchcd!='88' and a.type like '1%' and a.vchdate " + PrdRange + " and substr(A.acode,1,2)='16' group by to_Char(a.vchdate,'YYYYMM') ,to_Char(a.vchdate,'YYYY MONTH')) group by Month_Name,mth order by mth";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Monthly Sales Vs Collection" + PrdRange + "", frm_qstr);
            hffield.Value = "-";
        }

        else if (hffield.Value == "CUSTWISESALE")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            if (frm_cocd == "SAPL")
            {
                SQuery = "select a.acode as fstr, /*+ INDEX_DESC(sale ind_SAL_BRTYDT) */ B.aname as Customer,to_char(sum(bill_qty)) as Quantity,to_char(sum(amt_sale)) as Basic_Value,to_char(sum(Bill_tot)) as Gross_Value from sale a,famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.branchcd='" + frm_mbr + "' group by a.acode,B.aname order by B.aname";
            }
            else
            {
                SQuery = "select a.acode as fstr, /*+ INDEX_DESC(sale ind_SAL_BRTYDT) */ B.aname as Customer,to_char(sum(bill_qty)) as Quantity,to_char(sum(amt_sale)) as Basic_Value,to_char(sum(Bill_tot)) as Gross_Value from sale a,famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.vchdate " + PrdRange + " and a.branchcd='" + frm_mbr + "' and a.type!='47' group by a.acode,B.aname order by B.aname";
            }

            if (remv_02 == "Y")
            {
                SQuery = "select a.acode as fstr, /*+ INDEX_DESC(sale ind_SAL_BRTYDT) */ B.aname as Customer,to_char(sum(bill_qty)) as Quantity,to_char(sum(amt_sale)) as Basic_Value,to_char(sum(Bill_tot)) as Gross_Value from sale a,famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.vchdate " + PrdRange + " and a.branchcd!='DD' and a.type!='47' and substr(a.acode,1,2)!='02' group by a.acode,B.aname order by B.aname";
            }
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("Customer Wise Sales : Quantity , Basic , Gross", frm_qstr);
        }

        else if (hffield.Value == "PURCHTREND")
        {
            fgen.msg("-", "SMSG", "Select YES for ALL PLANT and NO for FLEXIBLE DIVISION");
            hffield.Value = "PURCHTRENDD";
        }
        else if (hffield.Value == "DIRPT")
        {
            fgen.msg("-", "SMSG", "Select YES for PENDING and NO for ALL");
            hffield.Value = "DIRPTT";
        }

        else if (hffield.Value == "DIRPTT")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                SQuery = "Select a.col3 as Time_Slot,a.col6 as Locn,a.col4 as Item,a.col5 as Enagare_Qty,nvl(b.sold,0) as Sale_qty,to_number(a.col5) - nvl(b.sold,0) as Bal_qty,(case when to_number(a.col5) - nvl(b.sold,0)>0 then 'Pending' else 'Complete' end) as DI_Status,a.col8 as Enagare_Number from scratch a left outer join(select trim(acode) as acode,trim(icode) as icode,trim(mtime) as mtime,trim(location) as location,sum(iqtyout) as sold from ivoucher where branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and vchdate=to_datE('" + fromdt + "','dd/mm/yyyy') group by trim(acode),tRIm(icode),trim(mtime),trim(location)) b on trim(a.icode)=trim(b.icode) and trim(A.acode)=trim(b.acode) and trim(a.col3)=trim(b.mtime) and trim(a.col6)=trim(b.location) where a.branchcd='" + frm_mbr + "' and a.type='EN' and a.vchdate=to_datE('" + fromdt + "','dd/mm/yyyy') ";
                SQuery = "Select * from (" + SQuery + ") z where to_number(z.Enagare_Qty) - z.Sale_qty>0 order by z.time_slot,z.Locn,z.Item";
            }
            else
            {
                SQuery = "Select a.col3 as Time_Slot,a.col6 as Location,a.col4 as Item,a.col5 as Enagare_Qty,nvl(b.sold,0) as Sale_qty,to_number(a.col5) - nvl(b.sold,0) as Bal_qty,(case when to_number(a.col5) - nvl(b.sold,0)>0 then 'Pending' else 'Complete' end) as DI_Status,a.col8 as Enagare_Number from scratch a left outer join(select trim(acode) as acode,trim(icode) as icode,trim(mtime) as mtime,trim(location) as location,sum(iqtyout) as sold from ivoucher where branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and vchdate=to_datE('" + fromdt + "','dd/mm/yyyy') group by trim(acode),tRIm(icode),trim(mtime),trim(location)) b on trim(a.icode)=trim(b.icode) and trim(A.acode)=trim(b.acode) and trim(a.col3)=trim(b.mtime) and trim(a.col6)=trim(b.location) where a.branchcd='" + frm_mbr + "' and a.type='EN' and a.vchdate=to_datE('" + fromdt + "','dd/mm/yyyy') order by a.col3,col6,a.col4";
            }

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("E-Nagare Status for " + fromdt + "", frm_qstr);
            hffield.Value = "-";
        }

        else if (hffield.Value == "SALEAGENTWISE")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            string cons = "";
            if (col1 == "Y")
            {
                cons = " A.BRANCHCD<>'DD'";
            }
            else
            {
                cons = " A.BRANCHCD='" + frm_mbr + "'";
            }
            SQuery = "select Nvl(a.thru,'-') as fstr, /*+ INDEX_DESC(sale ind_SAL_BRTYDT) */ Nvl(a.thru,'-') as Agent,to_char(sum(a.bill_tot)) as Total_sale,to_char(sum(a.Bill_qty)) as Total_Qty,to_char(sum(round(a.Bill_tot*(a.advrcvd/100),2))) as Commission from sale a where a.vchdate " + DateRange + " and " + cons + " and a.type<>'47' group by Nvl(a.thru,'-') order by Nvl(a.thru,'-')";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("Sales Agent Wise Performance", frm_qstr);
        }

        else if (hffield.Value == "PURCHTRENDD")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            string party_cd = "";
            string part_cd = "";
            string m4 = DateTime.Today.Year + "04";
            string m5 = DateTime.Today.Year + "05";
            string m6 = DateTime.Today.Year + "06";
            string m7 = DateTime.Today.Year + "07";
            string m8 = DateTime.Today.Year + "08";
            string m9 = DateTime.Today.Year + "09";
            string m10 = DateTime.Today.Year + "10";
            string m11 = DateTime.Today.Year + "11";
            string m12 = DateTime.Today.Year + "12";
            string m1 = DateTime.Today.Year + 1 + "01";
            string m2 = DateTime.Today.Year + 1 + "02";
            string m3 = DateTime.Today.Year + 1 + "03";
            string txtitemgrp = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1");
            string txtitemsubgrp = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR2");
            string txtitemstart = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3");
            string txtitemend = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4");

            string txtpartygrp = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR5");
            string txtpartysubgrp = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR6");
            string txtpartystart = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR7");
            string txtpartyend = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR8");

            if (txtpartystart.Trim().Length <= 1)
            {
                party_cd = "LIKE '%%'";
            }
            else
            {
                party_cd = "between '" + txtpartystart + "' and '" + txtpartyend + "'";
            }
            if (txtitemstart.Trim().Length <= 1)
            {
                part_cd = "LIKE '%%'";
            }
            else
            {
                part_cd = "between '" + txtitemstart + "' and '" + txtitemend + "'";
            }
            string cons = "";
            if (col1 == "Y")
            {
                cons = " A.BRANCHCD<>'DD'";
            }
            else
            {
                cons = " A.BRANCHCD='" + frm_mbr + "'";
            }
            string acfld = "", autoshiftrej = "", Prt_RAV = "";
            if (autoshiftrej == "Y")
            {
                acfld = "round(sum(a.iamount)/sum(a.iqtyin+nvl(a.rej_rw,0)),2)";
            }
            else
            {
                acfld = "round(sum(a.iamount)/sum(a.iqtyin),2)";
            }

            if (Prt_RAV == "Y")
            {
                cmd_query = "update ivoucher set ST_NMODV=(CASE WHEN substr(icode,1,2)<='02' THEN iqty_wt*ipack ELSE iqtyin*ipack END), ST_MODV=(CASE WHEN substr(icode,1,2)<='02' THEN iqty_wt ELSE iqtyin END) where branchcd='" + frm_mbr + "' and substr(type,1,1)='0' and vchdate " + PrdRange + " and ST_MODV is null";
                fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                SQuery = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Qty' as Rep,b.bssch as grp,c.Iname as Purpose,c.Cpartno as exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.ST_MODV),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.ST_MODV),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.ST_MODV),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.ST_MODV),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.ST_MODV),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.ST_MODV),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.ST_MODV),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.ST_MODV),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.ST_MODV),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.ST_MODV),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.ST_MODV),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.ST_MODV),0) as Mar,a.acode,a.icode from ivoucher a ,famst b,item c where TRIM(A.ACODE)=TRIM(b.acode) and TRIM(A.iCODE)=TRIM(c.icode) and " + cons + " and substr(a.type,1,1)='0' and a.vchdate " + PrdRange + " and trim(a.acode) " + party_cd + " and trim(a.icode) " + part_cd + " and a.store='Y' group by a.acode,b.bssch,c.Iname,c.cpartno,a.icode,b.aname,to_char(vchdate,'yyyymm')  union all Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Value' as Rep,b.bssch as grp,c.Iname as purpose,c.cpartno as exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.ST_NMODV),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.ST_NMODV),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ", sum(a.ST_NMODV),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.ST_NMODV),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.ST_NMODV),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.ST_NMODV),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.ST_NMODV),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.ST_NMODV),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.ST_NMODV),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.ST_NMODV),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.ST_NMODV),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.ST_NMODV),0) as Mar,a.acode,a.icode from ivoucher a ,famst b,item c where TRIM(A.ACODE)=TRIM(b.acode) and TRIM(A.iCODE)=TRIM(c.icode) and " + cons + " and substr(a.type,1,1)='0' and a.vchdate " + PrdRange + "  and trim(a.acode) " + party_cd + " and trim(a.icode) " + part_cd + " and a.store='Y' group by a.acode,b.bssch,a.icode,c.Iname,c.cpartno,b.aname,to_char(vchdate,'yyyymm')";
            }
            else
            {
                SQuery = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Qty' as Rep,b.bssch as grp,c.Iname as Purpose,c.Cpartno as exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iqtyin),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iqtyin),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iqtyin),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iqtyin),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iqtyin),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iqtyin),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iqtyin),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iqtyin),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iqtyin),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iqtyin),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iqtyin),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iqtyin),0) as Mar,a.acode,a.icode from ivoucher a ,famst b,item c where TRIM(A.ACODE)=TRIM(b.acode) and TRIM(A.iCODE)=TRIM(c.icode) and " + cons + " and substr(a.type,1,1)='0' and a.vchdate " + PrdRange + " and substr(a.icode,1,2) like '" + txtitemgrp + "%' and trim(a.acode) " + party_cd + " and trim(a.icode) " + part_cd + " and a.store='Y' and a.iqtyin>0 group by a.acode,b.bssch,c.Iname,c.cpartno,a.icode,b.aname,to_char(vchdate,'yyyymm')  union all Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Rate(Avg)' as Rep,b.bssch as grp,c.Iname as purpose,c.cpartno as exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + "," + acfld + ",0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + "," + acfld + ",0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ", " + acfld + ",0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + "," + acfld + ",0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + "," + acfld + ",0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + "," + acfld + ",0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + "," + acfld + ",0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + "," + acfld + ",0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + "," + acfld + ",0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + "," + acfld + ",0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + "," + acfld + ",0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + "," + acfld + ",0) as Mar,a.acode,a.icode from ivoucher a ,famst b,item c where TRIM(A.ACODE)=TRIM(b.acode) and TRIM(A.iCODE)=TRIM(c.icode) and  " + cons + " and substr(a.type,1,1)='0' and substr(a.icode,1,2) like '" + txtitemgrp + "%' and a.vchdate " + PrdRange + "  and trim(a.acode) " + party_cd + " and trim(a.icode) " + part_cd + " and a.store='Y' and a.iqtyin>0 group by a.acode,b.bssch,a.icode,c.Iname,c.cpartno,b.aname,to_char(vchdate,'yyyymm') union all Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Value' as Rep,b.bssch as grp,c.Iname as purpose,c.cpartno as exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iamount),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iamount),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ", sum(a.iamount),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iamount),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iamount),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iamount),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iamount),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iamount),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iamount),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iamount),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iamount),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iamount),0) as Mar,a.acode,a.icode from ivoucher a ,famst b,item c where TRIM(A.ACODE)=TRIM(b.acode) and TRIM(A.iCODE)=TRIM(c.icode) and  " + cons + " and substr(a.type,1,1)='0' and substr(a.icode,1,2) like '" + txtitemgrp + "%' and a.vchdate " + PrdRange + "  and trim(a.acode) " + party_cd + " and trim(a.icode) " + part_cd + " and a.store='Y' and a.iqtyin>0 group by a.acode,b.bssch,a.icode,c.Iname,c.cpartno,b.aname,to_char(vchdate,'yyyymm')";
            }

            SQuery = "Select x.Account,x.Rep,y.Name,upper(x.purpose) as Item,upper(x.exc_57f4) as Partno,to_char(decode(rep,'Rate(Avg)',0,sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar))) as Totals,to_char(sum(x.April)) as April,to_char(sum(x.May)) as May,to_char(sum(x.June)) as June,to_Char(sum(x.July)) as July,to_char(sum(x.August)) as August,to_Char(sum(x.Sept)) as Sept,to_char(sum(x.oct)) as Oct,to_Char(sum(x.Nov)) as Nov,to_char(sum(x.Dec)) as Dec,to_Char(sum(x.Jan)) as Jan,to_char(sum(x.Feb)) as Feb,to_Char(sum(x.Mar)) as Mar,x.Acode,x.icode from (" + SQuery + ") x left outer join (select type1,name from typegrp where id='A') y on trim(x.grp)=trim(y.type1) group by x.account,x.acode,upper(x.purpose),x.icode,upper(x.exc_57f4),x.rep,y.name order by x.Account";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Month Wise Purchase ( Qty / Value ) Analysis", frm_qstr);
            hffield.Value = "-";
        }

        else if (hffield.Value == "SALESDATA")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            string qstring = "";
            if (col1 == "Y")
            {
                qstring = " <>'88'";
            }
            else
            {
                qstring = " ='" + frm_mbr + "'";
            }
            if (frm_cocd == "KESL" || frm_cocd == "KCSL" || frm_cocd == "JOBCOAT")
            {
                SQuery = "select /*+ INDEX_DESC(sale ind_SAL_BRTYDT) */ vchdate as Dated,to_char(sum(bill_qty)) as Quantity,to_char(sum(amt_sale+nvl(amt_Rea,0))) as Basic_Value,to_char(sum(Bill_tot)) as Gross_Value,sum(amt_sale) as amt,count(*) as Invoices from sale where vchdate " + DateRange + " and branchcd " + qstring + "  and type<>'47' group by vchdate order by vchdate desc";
            }
            else
            {
                SQuery = "select /*+ INDEX_DESC(sale ind_SAL_BRTYDT) */ vchdate as Dated,to_char(sum(bill_qty)) as Quantity,to_char(sum(amt_sale)) as Basic_Value,to_char(sum(Bill_tot)) as Gross_Value,sum(amt_sale) as amt,count(*) as Invoices,round(sum(wt_num),0) as wt_sold from sale where vchdate " + DateRange + " and branchcd " + qstring + "  and type<>'47' group by vchdate order by vchdate desc ";
            }

            if (remv_02 == "Y")
            {
                SQuery = "select /*+ INDEX_DESC(sale ind_SAL_BRTYDT) */ vchdate as Dated,to_char(sum(bill_qty)) as Quantity,to_char(sum(amt_sale)) as Basic_Value,to_char(sum(Bill_tot)) as Gross_Value,sum(amt_sale) as amt,count(*) as Invoices from sale where vchdate " + DateRange + " and branchcd " + qstring + "  and type<>'47' and substr(acode,1,2)!='02' group by vchdate order by vchdate desc ";
            }

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Day Wise Sales : Quantity , Basic , Gross ", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "SALEORDREC")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            string qstring = "";
            if (col1 == "Y")
            {
                qstring = " a.BRANCHCD<>'DD'";
            }
            else
            {
                qstring = " a.BRANCHCD='" + frm_mbr + "'";
            }

            if (remv_02 == "Y")
            {
                SQuery = "select to_Char(a.orddt,'YYYY MONTH') as Month_Name,to_char(sum(a.qtyord)) as Quantity,to_char(sum(a.qtyord*a.irate)) as Amount,to_char(sum(a.qtyord*a.irate*a.CURR_RATE)) as AED,to_Char(a.orddt,'YYYYMM') as mth from somas a where a.orddt " + DateRange + " and " + qstring + " and a.type!='47' and substr(a.acode,1,2)!='02' group by to_Char(a.orddt,'YYYYMM') ,to_Char(a.orddt,'YYYY MONTH') order by to_Char(a.orddt,'YYYYMM')";
            }
            else
            {
                SQuery = "select to_Char(a.orddt,'YYYY MONTH') as Month_Name,to_char(sum(a.qtyord)) as Quantity,to_char(sum(a.qtyord*a.irate)) as Amount,to_char(sum(a.qtyord*a.irate*a.CURR_RATE)) as AED,to_Char(a.orddt,'YYYYMM') as mth from somas a where a.orddt " + DateRange + " and " + qstring + " and a.type!='47' group by to_Char(a.orddt,'YYYYMM') ,to_Char(a.orddt,'YYYY MONTH') order by to_Char(a.orddt,'YYYYMM')";
            }
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Month Wise Orders : Quantity , Basic_Value", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "SALESTREND")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            string party_cd = "";
            string part_cd = "";
            string txtitemgrp = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1");
            string txtitemsubgrp = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR2");
            string txtitemstart = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3");
            string txtitemend = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4");

            string txtpartygrp = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR5");
            string txtpartysubgrp = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR6");
            string txtpartystart = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR7");
            string txtpartyend = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR8");

            if (txtpartystart.Trim().Length <= 1)
            {
                party_cd = "LIKE '%%'";
            }
            else
            {
                party_cd = "between '" + txtpartystart + "' and '" + txtpartyend + "'";
            }
            if (txtitemstart.Trim().Length <= 1)
            {
                part_cd = "LIKE '%%'";
            }
            else
            {
                part_cd = "between '" + txtitemstart + "' and '" + txtitemend + "'";
            }
            if (frm_cocd == "PRPL")
            {

                SQuery = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Qty' as Rep,b.bssch as grp,a.purpose,a.exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iqtyout),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iqtyout),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iqtyout),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iqtyout),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iqtyout),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iqtyout),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iqtyout),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iqtyout),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iqtyout),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iqtyout),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iqtyout),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iqtyout),0) as Mar,a.acode,a.icode from ivoucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode) where a.branchcd = '" + frm_mbr + "' and substr(a.type,1,1)='4' and substr(a.icode,1,1)='9' and a.vchdate " + PrdRange + " and trim(a.acode) " + party_cd + " and trim(a.icode) " + part_cd + " group by a.acode,b.bssch,a.purpose,a.exc_57f4,a.icode,b.aname,to_char(vchdate,'yyyymm')  union all Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Value' as Rep,b.bssch as grp,a.purpose,a.exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iamount),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iamount),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ", sum(a.iamount),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iamount),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iamount),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iamount),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iamount),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iamount),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iamount),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iamount),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iamount),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iamount),0) as Mar,a.acode,a.icode from ivoucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode)  where a.branchcd = '" + frm_mbr + "' and substr(a.type,1,1)='4' and a.type!='47' and a.vchdate " + PrdRange + "  and substr(a.icode,1,1)='9' and trim(a.acode) " + party_cd + " and trim(a.icode) " + part_cd + " group by a.acode,b.bssch,a.icode,a.purpose,a.exc_57f4,b.aname,to_char(vchdate,'yyyymm')";
            }

            else
            {
                SQuery = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Qty' as Rep,b.bssch as grp,a.purpose,a.exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iqtyout),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iqtyout),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iqtyout),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iqtyout),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iqtyout),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iqtyout),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iqtyout),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iqtyout),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iqtyout),0) as Dec,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iqtyout),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iqtyout),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iqtyout),0) as Mar,a.acode,a.icode from ivoucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode) where a.branchcd = '" + frm_mbr + "' and substr(a.type,1,1)='4' and a.vchdate " + PrdRange + " and trim(a.acode) " + party_cd + " and trim(a.icode) " + part_cd + " group by a.acode,b.bssch,a.purpose,a.exc_57f4,a.icode,b.aname,to_char(vchdate,'yyyymm')  union all Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Value' as Rep,b.bssch as grp,a.purpose,a.exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iamount),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iamount),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iamount),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iamount),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iamount),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iamount),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iamount),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iamount),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iamount),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iamount),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iamount),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iamount),0) as Mar,a.acode,a.icode from ivoucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode) where a.branchcd = '" + frm_mbr + "' and substr(a.type,1,1)='4' and a.type!='47' and a.vchdate " + PrdRange + "  and trim(a.acode) " + party_cd + " and trim(a.icode) " + part_cd + " and substr(a.acode,1,2)!='02' group by a.acode,b.bssch,a.icode,a.purpose,a.exc_57f4,b.aname,to_char(vchdate,'yyyymm')";
            }

            //SQuery = "Select x.Account,x.Rep,y.Name,upper(x.purpose) as Item,upper(nvl(x.exc_57f4,'-')) as Partno,(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar)) as Totals,(sum(x.April)) as April,(sum(x.May)) as May,(sum(x.June)) as June,(sum(x.July)) as July,(sum(x.August)) as August,(sum(x.Sept)) as Sept,(sum(x.oct)) as Oct,(sum(x.Nov)) as Nov,(sum(x.Dec)) as Dec,(sum(x.Jan)) as Jan,(sum(x.Feb)) as Feb,(sum(x.Mar)) as Mar,x.Acode,x.icode from (" + SQuery + ") x left outer join (select type1,name from typegrp where id='A') y on trim(x.grp)=trim(y.type1) group by x.account,x.acode,x.icode,upper(x.purpose),upper(nvl(x.exc_57f4,'-')),x.rep,y.name ";

            SQuery = "Select x.Account,x.Rep,y.Name,upper(trim(x.purpose)) as Item,upper(trim(x.exc_57f4)) as Partno,to_char(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar)) as Totals,to_char(sum(x.April)) as April,to_char(sum(x.May)) as May,to_char(sum(x.June)) as June,to_Char(sum(x.July)) as July,to_char(sum(x.August)) as August,to_Char(sum(x.Sept)) as Sept,to_char(sum(x.oct)) as Oct,to_Char(sum(x.Nov)) as Nov,to_char(sum(x.Dec)) as Dec,to_Char(sum(x.Jan)) as Jan,to_char(sum(x.Feb)) as Feb,to_Char(sum(x.Mar)) as Mar,x.Acode,x.icode from (" + SQuery + ") x left outer join (select type1,name from typegrp where id='A') y on trim(x.grp)=trim(y.type1) group by x.account,x.acode,upper(trim(x.purpose)),x.icode,upper(trim(x.exc_57f4)),x.rep,y.name order by x.Account";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Month Wise Sales ( Qty / Value ) Analysis)", frm_qstr);
            hffield.Value = "-";
        }


        else if (hffield.Value == "MML")
        {
            SQuery = "Select type1 as fstr, Name,Type1 from Type where id='Y' and substr(type1,1,1) not in ('7','8','9') order by type1";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_mseek("-", frm_qstr);
        }
        else if (hffield.Value == "VIPL")
        {
            SQuery = "Select type1 as fstr,Name,Type1 from Type where id='Z' and substr(type1,1,2) in ('05','06') order by type1";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_mseek("-", frm_qstr);
        }
        else if (hffield.Value == "PORI")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "Select A.Orddt,B.aname as Supplier,C.iname as Item,A.Qtyord as Qty_ord,(case when round(1*(a.prate*((100-a.pdisc)/100)),4)>A.Nxtmth then 'Increase Rate' else 'Decrease Rate' END) as Status, round(nvl(a.wk3,1)*(a.prate*((100-a.pdisc)/100)),4) as Net_PO_Rate,round(nvl(a.wk3,1)*A.Nxtmth,3) as Old_Rate,a.type,a.rate_diff as Reason,A.Ordno,A.Ent_By,A.App_by from pomas a, famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.Icode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='5' and a.orddt " + PrdRange + " and  round(1*(a.prate*((100-a.pdisc)/100)),4)<>nvl(a.nxtmth,0) and nvl(a.nxtmth,0)<>0 and a.type not in ('52','53') order by a.orddt,a.ordno";
            SQuery = "Select type as fstr,Supplier as Name,Item,Orddt,Reason,Status, Net_PO_Rate,Old_Rate,Net_PO_Rate-Old_Rate as Diff,(Case when Old_Rate>0 then round(((Net_PO_Rate-Old_Rate)/Old_Rate)*100,2) else 0 end) as Percentg,Qty_ord,Ordno,type,Ent_By,App_by from (" + SQuery + ") order by Qty_ord desc,orddt,ordno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("-", frm_qstr);
        }
        else if (hffield.Value == "PR")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select a.ordno as PR_NO,a.orddt as PR_DT,b.iname as Item_Name,sum(a.prq) as Prqty,sum(a.poq) as POQty,sum(a.prq)-sum(a.poq) as Bal_prqty,round(sysdate-a.orddt,0) as Delay_Days,b.unit,max(bank) as deptt,max(tr_insur) as Ind_ref,max(a.App_by) as Appr_by,trim(a.icode) as icode,max(a.pflag)as pflag from (Select tr_insur,bank,pflag,ordno,orddt,icode,qtyord as prq,0 as poq,app_by from pomas where branchcd='" + frm_mbr + "' and type='60' and orddt " + PrdRange + " union all Select null as tr_insur,null as bank,null as pflag,pr_no,pr_dt,icode,0 as prq,qtyord as poq,null as ent_by from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt " + PrdRange + " and substr(term,1,2) not like '%CANCELLED%')a,item b where trim(A.icode)=trim(B.icode) group by b.iname,b.unit,a.ordno,a.orddt,trim(a.icode) having sum(a.prq)-sum(a.poq)>0 and max(a.pflag)<>0 order by Delay_Days desc,a.orddt,a.ordno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("PR Pending For Purchase Order " + PrdRange + " )", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "ORPR")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select a.ordno as PO_NO,a.orddt as PO_DT,b.aname as Supplier,c.iname as Item,sum(a.poq) as PO_Qty,sum(a.rcvq) as MRR_Qty,sum(a.poq)-sum(a.rcvq) as Bal_Qty,round(sysdate-a.orddt,0) as Delay_Days,max(a.app_by) as appr_by,c.unit,trim(a.icode) as icode,max(a.pflag)as pflag from (Select pflag,ordno,orddt,acode,icode,qtyord as poq,0 as rcvq,app_by from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt " + PrdRange + " union all Select null as pflag,ponum,podate,acode,icode,0 as prq,iqtyin as poq,null as app_by from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and potype like '5%' and vchdate " + PrdRange + " and store in ('Y','N') )a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(a.icode)=trim(C.icode) group by c.iname,c.unit,b.aname,a.ordno,a.orddt,trim(a.AcodE),trim(a.icode) having sum(a.poq)-sum(a.rcvq)>0 and max(a.pflag)<>1 order by Delay_Days desc,a.orddt,a.ordno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("PO Pending MRR " + PrdRange + " )", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "TAT")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "Select null as popflg,pflag,null as acode,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,ent_Dt,(Case when length(trim(nvl(App_by,'-')))>1 then app_dt else null end) as app_dt,null as jcno,null as jcdate,null as jcentdt,null as jc_app_Dt,null as invno,null as invdt,null as inv_ent,icode,qtyord,0 as poqty,0 as mrrqty,del_date from Pomas where branchcd='" + frm_mbr + "' and type like '6%' and orddt " + PrdRange + " union all Select pflag,null as pflag,acode,pr_no,to_char(pr_Dt,'dd/mm/yyyy'),null as ent_Dt,null as app_Dt,ordno as jcno,orddt as jcdate,(Case when length(trim(nvl(edt_by,'-')))>1 then edt_dt else ent_dt end) as jcentdt,app_dt,null as invno,null as invdt,null as inv_ent,icode,0 as qtyord,qtyord as poqty,0 as mrrqty,null as del_date from pomas where branchcd='" + frm_mbr + "' and type like '5%' and pr_dt " + PrdRange + " union all Select null as popflg,null as pflag,acode,prnum,to_Char(RTN_DATE,'dd/mm/yyyy'),null as ent_Dt,null as app_Dt,ponum as jcno,podate as jcdate,null as jcentdt,null as app_Dt,vchnum as invno,vchdate as invdt,ent_Dt as inv_ent,icode,0 as qtyord,0 as poqty,iqtyin as mrrqty,null as del_date from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and PODATE " + PrdRange + " and trim(nvl(prnum,'-'))!='-' ";
            SQuery = "select max(a.popflg) as popflg,max(a.pflag) as pflag,a.ordno AS PR_NO,a.orddt AS PR_DT,max(a.ent_Dt) as PR_ent_Dt,max(del_date) As Our_delv_date,max(a.app_dt) As PR_app_Dt,max(a.jcno) As PO_NO,max(a.jcdate) As PO_DT,max(a.jcentdt) As PO_ENT_DT,max(a.jc_app_Dt) as PO_APP_dT,max(a.invno) as LAST_MRR_NO,max(a.invdt) as LAST_MRR_dT,max(a.inv_ent) as LAST_MRR_ENT_DT,trim(a.icode) as ERP_code,b.Iname,sum(a.qtyord) As PR_Qty,sum(a.poqty) as Po_Qty,sum(a.mrrqty) as MRR_Qty,b.unit,max(a.acode) as Supp_cd from (" + SQuery + ")a,item b where trim(A.icode)=trim(b.icode) group by a.ordno,a.orddt,trim(A.icode),b.iname,b.unit  ";
            SQuery = "select a.PR_NO,a.PR_DT,a.PR_ent_Dt,a.Our_delv_date as Reqd_dt,a.PR_app_Dt,a.PO_NO,a.PO_DT,a.PO_ENT_DT,a.PO_APP_dT,a.LAST_MRR_NO,a.LAST_MRR_dT,a.LAST_MRR_ENT_DT,a.ERP_code,a.Iname,a.PR_Qty,a.Po_Qty,a.MRR_Qty,a.unit,a.Supp_cd,b.aname as Supplier,(Case when a.pflag=0 then 'PR Closed' else '-' end) as PR_STATUS,(Case when a.popflg=1 then 'PO Closed' else '-' end) as PO_STATUS,round(a.PO_ent_Dt-a.PR_ent_Dt,2) as PO_TAT_PR_DT,round(a.PO_ent_Dt-a.PR_app_Dt,2) as PO_TAT_PRAP_DT,round(a.LAST_MRR_dT-a.PR_app_Dt,2) as PR_TAT_PR_MR_DT from (" + SQuery + ")a left outer join famst b on trim(a.Supp_cd)=trim(b.acode) where a.PR_Qty<>0 order by a.PR_DT,a.PR_NO,a.Iname";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("PO TRACKING ( Order TO DELIVERY ) " + PrdRange + " )", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "INVARD")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            string tot_inw_line = "", tot_rej_line = "", MY_SIZE = "";

            tot_inw_line = fgen.seek_iname(frm_qstr, frm_cocd, "Select count(*) as cnt from ivoucher a where a.branchcd='" + frm_mbr + "' and substr(A.type,1,1)='0' and a.vchdate " + PrdRange + " and a.store in ('Y','N') ", "cnt");
            tot_rej_line = fgen.seek_iname(frm_qstr, frm_cocd, "Select count(*) as cnt from ivoucher a where a.branchcd='" + frm_mbr + "' and substr(A.type,1,1)='0' and a.vchdate " + PrdRange + " and a.store in ('Y','N') and a.rej_Rw>0 ", "cnt");

            MY_SIZE = "MRRREP";
            SQuery = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ to_char(a.vchdate,'DD/MM/YY') as Dated,a.Vchnum as MRR_No,b.aname as Supplier,decode(trim(a.INVNO),'-','',trim(a.INVNO)||' Dt.'||to_char(a.INVDATE,'dd/mm/yy'))||','||decode(trim(a.REFNUM),'-','','Ch.'||trim(a.REFNUM)||' Dt.'||to_char(a.REFdate,'dd/mm/yy')) as Bill_Chl,c.iname as Item_Name,c.unit,a.iqty_chl as Advised,a.iqtyin+nvl(a.rej_rw,0)+nvl(a.rej_sdp,0) as Rcvd,a.acpt_ud  as Accept,a.rej_rw as Reject,a.irate,a.ichgs as Lc,c.cpartno as Code,a.Btchno as Batchno,decode(a.segment_,1,'Y',2,'N',3,'N/a') as Exc_Doc,a.finvno,a.Type as LOT,a.no_cases as Frght_Stat),a.ponum as P_O_No,a.Genum as Gate_Entry,a.gedate as Gate_Date,a.Ent_by,a.Pname as Insp_By,a.Qcdate,a.icode,a.store,A.MTIME,a.desc_,a.mode_tpt,a.podate, a.rgpnum,a.Purpose as QARMK,a.Isize as Test_cert,A.Freight,a.QC_Date,a.t_Deptt,a.styleno as GRNO,a.stage,a.exc_Amt,a.doc_tot as Totv,c.iweight,A.BINNO,a.iqty_wt,a.EXC_TIME as rgpt,a.potype,a.vchdate,a.srno,a.exc_57f4 as Lic_ref,b.gst_no,a.invno,a.invdate,a.st_entform as EWAY_BILL,a.iexc_Addl as asitis from ivoucher a, famst b , item c where a.branchcd='" + frm_mbr + "' and substr(A.type,1,1)='0' and a.vchdate " + PrdRange + "  and TRIM(a.ICODE)=trim(c.icode) and  TRIM(A.ACODE)=TRIM(b.acode) and a.rej_rw>0 and a.store in ('Y','N') ";
            SQuery = "select x.*,y.Name as PO_type from (" + SQuery + ")x left outer join (select type1,name from type where id='M' and type1 like '5%') y on trim(x.potype)=trim(y.type1) order by x.vchdate,x.type,x.MRR_No,x.srno";

            //SQuery = "select b.iname,b.cpartno,a.sal_Qty,a.sal_amt,b.unit,b.cdrgno,a.icode,b.ent_by,b.ent_Dt from (select trim(icode)as icode,sum(bom) as gg,sum(sal_Qty) as sal_Qty,to_char(sum(sal_amt),'999,99,99,999') as sal_amt from (select icode,0 as bom,sum(iqtyout) as sal_Qty,sum(iqtyout*irate) as sal_amt from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and type!='45' and vchdate " + PrdRange + "  group by icode union all select distinct icode,1 as bom,0 as salqty,0 as amts from itemosp) group by trim(icode) having sum(bom)=0 ) a, item b where substr(a.icode,1,1) in ('7','8','9') and trim(a.icode)=trim(B.icodE) and length(Trim(b.icode))>4 order by a.sal_Qty desc,B.iname ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Material Inward Data , With Rejection" + PrdRange + " )", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "STOCK")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            string xprd1 = " between to_date('" + Convert.ToDateTime(frm_CDT1).AddYears(-1).ToShortDateString() + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(fromdt).AddDays(-1).ToShortDateString() + "','dd/mm/yyyy')";
            string xprd2 = " between to_date('" + Convert.ToDateTime(fromdt).ToShortDateString() + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(todt).ToShortDateString() + "','dd/mm/yyyy')";
            string mq0 = "", mq1 = "", mq2 = "";
            mq0 = "select b.iname,a.icode,sum(a.opening) as Opening,sum(a.cdr) as Inwards,sum(a.ccr) as Outwards,sum(opening)+sum(cdr)-sum(ccr) as closing,b.unit,b.cpartno,substr(a.icode,1,4) As sub_Grp from (Select icode, " + DateTime.Today.Year + " as opening,0 as cdr,0 as ccr,0 as clos from itembal where branchcd='" + frm_mbr + "' union all  ";
            mq1 = "select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd1 + " and store='Y' GROUP BY ICODE union all ";
            mq2 = "select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd2 + " and store='Y' GROUP BY ICODE ) a, item b where trim(A.icode)=trim(B.icode) group by b.iname,b.unit,b.cpartno,a.icode,substr(a.icode,1,4) having sum(a.opening)+sum(a.cdr)-sum(a.ccr)<0 order by substr(a.icode,1,4),b.iname";
            SQuery = mq0 + mq1 + mq2;

            //SQuery = "select b.iname,b.cpartno,a.sal_Qty,a.sal_amt,b.unit,b.cdrgno,a.icode,b.ent_by,b.ent_Dt from (select trim(icode)as icode,sum(bom) as gg,sum(sal_Qty) as sal_Qty,to_char(sum(sal_amt),'999,99,99,999') as sal_amt from (select icode,0 as bom,sum(iqtyout) as sal_Qty,sum(iqtyout*irate) as sal_amt from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and type!='45' and vchdate " + PrdRange + "  group by icode union all select distinct icode,1 as bom,0 as salqty,0 as amts from itemosp) group by trim(icode) having sum(bom)=0 ) a, item b where substr(a.icode,1,1) in ('7','8','9') and trim(a.icode)=trim(B.icodE) and length(Trim(b.icode))>4 order by a.sal_Qty desc,B.iname ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Stock Summary (Needing Attention )during" + PrdRange + " )", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "INSP" || hffield.Value == "INSPITEM" || hffield.Value == "STOCKSTATUS")
        {
            SQuery = "select TYPE1 AS FSTR,NAME,TYPE1 from type where ID='Y' ORDER BY TYPE1 ASC";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_mseek("Select TYPE ", frm_qstr);

        }
        else if (hffield.Value == "PPM")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            SQuery = "select upper(trim(b.aname)) as Vendor_Name,sum(a.iqty_chl) as Sale,0 as Rejn,a.acode from ivoucher a, famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,1)='0' and a.type not in ('04','08') and a.store in ('Y','N') group by upper(trim(b.aname)),a.acode union all select upper(trim(b.aname)) as Customer_Name,0 as Sale,sum(a.iqtyin) as Rejn,a.acode from ivoucher a, famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,1)='0' and a.type not in ('04','08') and a.store ='R' group by upper(trim(b.aname)),a.acode ";
            SQuery = "select Vendor_Name,sum(sale) as Inward_qty,sum(rejn) as Rejn_Rcv_Qty,decode(sum(sale),0,'N/a',round((sum(rejn)/sum(sale)),5)*1000000) as Rejn_PPM,decode(sum(sale),0,'N/a',round(((sum(rejn)/sum(sale)))*100,2)) as Rejn_percent,acode from (" + SQuery + ") group by Vendor_Name,acode having sum(sale)>0 Order by round((sum(rejn)/sum(sale)),5)*1000000 desc ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Vendor.Inward Qty Vs Rejection (QTY)  " + PrdRange + " , PPM , Rejn % Analysis", frm_qstr);
            hffield.Value = "-";

        }

        else if (hffield.Value == "CUSTPPM")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            string out2 = "N", Scode = "";
            SQuery = "select upper(trim(b.aname)) as Customer_Name,sum(a.iqtyout) as Sale,0 as Rejn,a.acode,sum(a.iamount) as Saleamt,0 as rej_amt from ivoucher a, famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE) and a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,1)='4' and a.type not in ('47','45') group by upper(trim(b.aname)),a.acode union all select upper(trim(b.aname)) as Customer_Name,0 as Sale,sum(a.iqty_chl) as Rejn,a.acode,0 as sale_amt,sum(a.iamount) as rej_Amt from ivoucher a, famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,2)='04' and a.store<>'R' group by upper(trim(b.aname)),a.acode ";
            SQuery = "select Customer_Name,sum(sale) as Sale_qty,sum(rejn) as Rejn_Rcv_Qty,decode(sum(sale),0,'N/a',round((sum(rejn)/sum(sale)),5)*1000000) as Rejn_PPM,decode(sum(sale),0,'N/a',round(((sum(rejn)/sum(sale)))*100,2)) as Rejn_percent,sum(Saleamt) as Saleamt,sum(rej_amt) as rej_amt,acode from (" + SQuery + ") group by Customer_Name,acode having sum(sale)>0 Order by round((sum(rejn)/sum(sale)),5)*1000000 desc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Cust.Wise Sale Vs Rejection (QTY,Value)" + PrdRange + "", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "SALESPPM")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            string out2 = "N", Scode = "";
            SQuery = "select upper(trim(b.Iname)) as Item_Name,sum(a.iqtyout) as Sale,0 as Rejn,a.icode,sum(a.iamount) as Saleamt,0 as rej_amt from ivoucher a, item b  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,1)='4' and a.type not in ('47','45') group by upper(trim(b.iname)),a.icode union all select upper(trim(b.iname)) as Customer_Name,0 as Sale,sum(a.iqty_chl) as Rejn,a.icode,0 as sale_amt,sum(a.iamount) as rej_Amt from ivoucher a, item b  WHERE TRIM(A.iCODE)=TRIM(B.iCODE)  and a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,2)='04' and a.store<>'R' group by upper(trim(b.iname)),a.icode ";
            SQuery = "select Item_Name,sum(sale) as Sale_qty,sum(rejn) as Rejn_Rcv_Qty,decode(sum(sale),0,'N/a',round((sum(rejn)/sum(sale)),5)*1000000) as Rejn_PPM,decode(sum(sale),0,'N/a',round(((sum(rejn)/sum(sale)))*100,2)) as Rejn_percent,sum(Saleamt) as Saleamt,sum(rej_amt) as rej_amt,Icode from (" + SQuery + ") group by Item_Name,icode having sum(sale)>0 Order by round((sum(rejn)/sum(sale)),5)*1000000 desc ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Item.Wise Sale Vs Rejection (QTY,Value)" + PrdRange + "", frm_qstr);
            hffield.Value = "-";
        }


        else if (hffield.Value == "CUSTORD")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            string out2 = "N", Scode = "";
            SQuery = "select b.aname,b.addr1,a.acode,b.staten,b.country,b.pay_num,b.climit,b.ent_by,b.ent_Dt from (select trim(acode)as acode,sum(bom) as gg from (select distinct acode,1 as bom from somas where branchcd='" + frm_mbr + "' and type like '4%' and orddt " + PrdRange + " union all select distinct acode,-1 as bom from famst where nvl(climit,0)>0) group by trim(acode) having sum(bom)=1 ) a, famst b where substr(a.acode,1,2) in ('16') and trim(a.acode)=trim(B.acodE) and length(Trim(b.acode))>4 order by B.aname ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Customers (orders booked) Without Credit Limit for " + frm_mbr + "", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "SALEXY")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            string xprd2 = " between to_date('" + Convert.ToDateTime(frm_CDT1).ToShortDateString() + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(frm_CDT2).ToShortDateString() + "','dd/mm/yyyy')";
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            string xprd1 = "between to_date('" + Convert.ToDateTime(frm_CDT1).ToShortDateString() + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(frm_CDT1).AddYears(-1).ToShortDateString() + "','dd/mm/yyyy')";
            string scode = "";
            if (col1.Length < 1)
            {
                scode = "";
            }
            else
            {
                scode = col1;
            }
            string mq0 = "select b.aname,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,a.acode from (Select acode, " + DateTime.Today.Year + " as opening,0 as cdr,0 as ccr,0 as clos from famstbal where branchcd!='DD' and acode like '16%' union all  ";
            string mq1 = "select acode,sum(dramt)-sum(cramt) as op,0 as cdr,0 as ccr,0 as clos from voucher where branchcd!='88' and type like '%' and vchdate " + xprd1 + " and acode like '16%' GROUP BY aCODE union all ";
            string mq2 = "select acode,0 as op,sum(dramt) as cdr,sum(cramt) as ccr,0 as clos from voucher where branchcd!='88' and type like '%' and vchdate " + xprd2 + " and acode like '16%' GROUP BY aCODE )a, famst b where trim(A.acode)=trim(B.acodE) group by b.aname,a.acode ";

            SQuery = mq0 + mq1 + mq2;

            string MY_SIZE = "TOT234567";
            int XDAYS = Convert.ToInt32((Convert.ToDateTime(todt).Month - Convert.ToDateTime(fromdt).Month) / 30);



            SQuery = "select trim(B.aname) as Customer,trim(b.staten) as Staten,sum(a.prd1) as prd1_Amt,sum(a.prd2) as prd2_amt,sum(a.prd1)-sum(a.prd2) as Diff_amt,sum(a.prd1q) as prd1_qty,sum(a.prd2q) as prd2_qty,sum(a.prd1q)-sum(a.prd2q) as Diff_Qty,sum(A.COS) AS Curr_Ac_bal,d.Name as MKT_PERSON,trim(A.acode) as Accode,(cASE WHEN sum(a.prd1)>sum(a.prd2) THEN sum(a.prd1) ELSE sum(a.prd2) END) AS MAX_vOL,(cASE WHEN sum(a.prd1)>sum(a.prd2) THEN ROUND(sum(a.prd1)/" + XDAYS + " ,0) ELSE ROUND(sum(a.prd2)/" + XDAYS + " ,0) END) AS AVG_vOL from (select acode,icode,iamount as prd1,0 as prd2,iqtyout as prd1q,0 as prd2q,0 AS COS from ivoucher where branchcd!='DD' and type like '4%' and vchdate " + xprd1 + " and type not in ('47','45') and store<>'R' and substr(Acode,1,2)!='02' union all select acode,'-' AS icode,0 as prd1,0 as prd2,0 as prd1q,0 as prd2q,CLOSING AS COS from CUST_OS UNION ALL select acode,icode,0 as prd1,iamount as prd2,0 as prd1q,iqtyout as prd2q,0 AS COS from ivoucher where branchcd!='DD' and type like '4%' and vchdate " + xprd2 + " and type not in ('47','45') and store<>'R' and substr(Acode,1,2)!='02')a,famst b, typegrp d  where d.id='A' and d.type1 like '" + scode + "%' and  trim(b.bssch)=trim(d.type1) and trim(A.acode)=trim(B.acodE)  group by trim(A.acode),trim(B.aname),trim(B.staten),d.name order by d.Name,trim(B.aname)";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Sales", frm_qstr);
            hffield.Value = "-";

        }
        else if (hffield.Value == "INVPAID")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "Select acode,invno,invdate,dramt,0 as cramt,null as pay_date from recebal where branchcd!='88' and substr(Acode,1,2) in ('16') union all Select acode,invno,invdate,dramt,0 as cramt,null as pay_date from voucher where branchcd!='88' and type like '4%' and substr(Acode,1,2) in ('16') union all Select acode,invno,invdate,0 as dramt,cramt,vchdate from voucher where branchcd!='88' and type like '1%' and substr(Acode,1,2) in ('16')";
            SQuery = "select trim(acode) as Vend_Code,trim(invno) as Inv_no,invdate,sum(cramt) as Bill_amt,sum(dramt) as pymt_Amt,max(pay_date) as Pay_date from (" + SQuery + ") group by trim(acode),trim(invno),invdate having sum(cramt)-sum(dramt)=0 and sum(Dramt)>0 ";

            SQuery = "select b.Aname as Customer_name,a.Inv_no,a.invdate,b.pay_num as Payment_Terms,a.invdate+b.pay_num as Due_Date,a.Pay_date,(b.pay_num-(a.Pay_date-a.invdate))*-1 as GAP_Days,a.Pay_date-a.invdate as Pay_Days,(Case when a.Pay_date-a.invdate<b.pay_num then 'Early' else 'Late' end) as Pay_Status,a.Vend_Code,a.Bill_amt,a.Pymt_Amt from (" + SQuery + ") a,famst b where trim(a.Vend_Code)=trim(b.acodE) and a.invdate " + PrdRange + " order by b.Aname,a.invdate";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Sales", frm_qstr);
            fgen.Fn_open_mseek("-", frm_qstr);
        }
        else if (hffield.Value == "PURINV")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "Select acode,invno,invdate,cramt,0 as dramt,null as pay_date,null as bank_date from recebal where branchcd!='88' and substr(Acode,1,2) in ('05','06') union all Select acode,invno,invdate,cramt,0 as dramt,null as pay_date,null as bank_date  from voucher where branchcd!='88' and type like '5%' and substr(Acode,1,2) in ('05','06') union all Select acode,invno,invdate,0 as cramt,dramt,vchdate,bank_date  from voucher where branchcd!='88' and type like '2%' and substr(Acode,1,2) in ('05','06')";
            SQuery = "select trim(acode) as Vend_Code,trim(invno) as Inv_no,invdate,sum(cramt) as Bill_amt,sum(dramt) as pymt_Amt,max(pay_date) as Pay_date,max(Bank_date) as Bank_date from (" + SQuery + ") group by trim(acode),trim(invno),invdate having sum(cramt)-sum(dramt)=0 and sum(cramt)>0 ";

            SQuery = "select b.Aname as Vendor_name,a.Inv_no,a.invdate,b.pay_num as Payment_Terms,a.invdate+b.pay_num as Due_Date,a.Pay_date,Bank_date as Cleared_on,b.pay_num-(a.Pay_date-a.invdate) as GAP_Days,a.Pay_date-a.invdate as Pay_Days,(Case when a.Pay_date-a.invdate<b.pay_num then 'Early' else 'Late' end) as Pay_Status,a.Vend_Code,a.Bill_amt,a.Pymt_Amt from (" + SQuery + ") a,famst b where trim(a.Vend_Code)=trim(b.acodE) and a.invdate " + PrdRange + " order by b.Aname,a.invdate";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Vendor Payment Terms Adherence)", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "PURNOTISSUE")
        {
            fgen.Fn_ValueBox("Days For Which Not Issued", frm_qstr);
        }
        else if (hffield.Value == "MRR")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select distinct 'GEMRR' as xtype,type,vchnum from ivoucherp where branchcd='" + frm_mbr + "' and type like '00' and vchdate " + PrdRange + " union all select distinct 'ALLMRR' as xtype,type,vchnum from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + PrdRange + " and store<>'R' union all select distinct 'QCMRR' as xtype,type,vchnum from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + PrdRange + " and inspected='Y' union all select distinct 'FINMRR' as xtype,type,vchnum from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + PrdRange + " and length(Trim(finvno))>2";
            SQuery = "select type,decode(xtype,'GEMRR',count(Vchnum),0) as GE_MRR,decode(xtype,'ALLMRR',count(Vchnum),0) as all_MRR,decode(xtype,'QCMRR',count(Vchnum),0) as QC_MRR,decode(xtype,'FINMRR',count(Vchnum),0) as FIN_MRR from (" + SQuery + ") group by xtype,type";

            SQuery = "select nvl(b.Name,'G.E.') as Name,sum(a.GE_MRR) as GE_DONE,sum(a.all_MRR) as MRR_Made,sum(a.QC_MRR)as QC_Done,sum(a.FIN_MRR) as Vch_Made,a.type as Type_of_MRR from (" + SQuery + ") a left outer join (Select type1,name from type where id='M' and substr(type1,1,1)='0') b on a.type=b.type1 group by b.name,a.type order by a.type";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("MRR Passing Performance  " + PrdRange + ")", frm_qstr);
        }
        else if (hffield.Value == "DOCVOL")
        {
            col1 = "";

            fgen.msg("-", "CMSG", "Do you want a Consolidated Statement?");
            hffield.Value = "DOCVOL";

        }
        else if (hffield.Value == "LATEHR")
        {
            fgen.Fn_ValueBox("Enter the time in Hrs Use 24 Hrs Format", frm_qstr);
            hffield.Value = "LATEHR";
        }

        else if (hffield.Value == "CREDIT")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            SQuery = "select a.vchnum as CrNote_No,a.vchdate as Doc_Dt,b.aname as Party,a.acode as Party_Code,a.iqty_chl as Doc_qty,a.spexc_Amt as Tot_Val,a.invno,a.invdate,a.type,A.ENT_BY  from ivoucher a , famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " AND a.type like '58%' and substr(A.acode,1,2) in ('16') and a.spexc_Amt>0 order by a.vchdate desc,a.vchnum desc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Customers (orders booked) Without Credit Limit for " + PrdRange + "", frm_qstr);
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
            //setColHeadings();

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

                    if (edmode.Value == "Y")
                    {
                        save_it = "Y";
                        // frm_vnum = txtvchnum.Value;
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
                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vchnum) AS VCH FROM enq_mast where branchcd='" + frm_mbr + "' and type='20'  order by vchdate desc ", 6, "VCH");
                                //pk_error = fgen.chk_pk(frm_qstr,frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                if (i > 20)
                                {
                                    fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vchnum) AS VCH FROM enq_mast where branchcd='" + frm_mbr + "' and type='20'  order by vchdate desc ", 6, "VCH");
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


                    if (edmode.Value == "Y")
                    {
                        cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd='" + frm_mbr + "' and type='20' and vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "' ";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);


                    }
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                    if (edmode.Value == "Y")
                    {
                        cmd_query = "delete from " + frm_tabname + " where branchcd='DD' and type='20' and  vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'  ";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        //fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");



                    }
                    else
                    {
                        if (save_it == "Y")
                        {
                            //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                            // fgen.msg("-", "AMSG", lblheader.Text + " " + " Saved Successfully ");
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
        srno = fgen.seek_iname(frm_qstr, frm_cocd, "select max(srno) as srno  from enq_mast", "srno");





    }


    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "P70106E":
                frm_vty = "CC";
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



    public void create_tab1()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;

        // Hidden Field
        sg2_dt.Columns.Add(new DataColumn("sg2_Srno", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f4", typeof(string)));

    }

    public void create_tab2()
    {
        sg3_dt = new DataTable();
        sg3_dr = null;
        // Hidden Field
        sg3_dt.Columns.Add(new DataColumn("sg3_Srno", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f3", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f4", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f5", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f6", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f7", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f8", typeof(string)));
    }

    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        // Hidden Field



        //sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(Int32)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f4", typeof(string)));

    }
    //------------------------------------------------------------------------------------

    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();

        sg2_dr["sg2_f1"] = "-";
        sg2_dr["sg2_f2"] = "-";
        sg2_dr["sg2_f3"] = "-";
        sg2_dr["sg2_f4"] = "-";

        sg2_dt.Rows.Add(sg2_dr);
    }

    public void sg3_add_blankrows()
    {
        sg3_dr = sg3_dt.NewRow();
        //sg3_dr["sg3_Srno"] = sg3_dt.Rows.Count + 1;
        sg3_dr["sg3_f1"] = "-";
        sg3_dr["sg3_f2"] = "-";
        sg3_dr["sg3_f3"] = "-";
        sg3_dr["sg3_f4"] = "-";
        sg3_dr["sg3_f5"] = "-";
        sg3_dr["sg3_f6"] = "-";
        sg3_dr["sg3_f7"] = "-";
        sg3_dr["sg3_f8"] = "-";
        sg3_dt.Rows.Add(sg3_dr);
    }


    public void sg1_add_blankrows()
    {
        sg1_dr = sg1_dt.NewRow();


        //sg1_dr["sg1_SrNo"] = sg1_dt.Rows.Count + 1;
        sg1_dr["sg1_f1"] = "-";
        sg1_dr["sg1_f2"] = "-";
        sg1_dr["sg1_f3"] = "-";
        sg1_dr["sg1_f4"] = "-";


        sg1_dt.Rows.Add(sg1_dr);
    }
    //------------------------------------------------------------------------------------

    void newCase(string vty)
    {
        #region
        vty = "20";
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        disablectrl();
        fgen.EnableForm(this.Controls);


        sg1_dt = new DataTable();
        sg2_dt = new DataTable();
        sg3_dt = new DataTable();
        create_tab();
        create_tab1();
        create_tab2();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();




        //setColHeadings();
        ViewState["sg1"] = sg1_dt;



        set_Val();
        #endregion
    }


    protected void BtnSalesData_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SALESDATA";
        fgen.msg("-", "SMSG", "Select YES for ALL Plant View and NO for FLEXIBLE DIVISION View");
    }

    protected void BtnCustWiseSale_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "CUSTWISESALE";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnSalesTrend_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SALESTREND";
        fgen.Fn_open_PartyItemDateRangeBox("", frm_qstr);
    }
    protected void BtnSalesOrdRec_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SALEORDREC";
        fgen.msg("-", "SMSG", "Select YES for ALL Plant View and NO for FLEXIBLE DIVISION View");
    }
    protected void BtnSaleGrpTrend_ServerClick(object sender, EventArgs e)
    {
        string m4 = DateTime.Today.Year + "04";
        string m5 = DateTime.Today.Year + "05";
        string m6 = DateTime.Today.Year + "06";
        string m7 = DateTime.Today.Year + "07";
        string m8 = DateTime.Today.Year + "08";
        string m9 = DateTime.Today.Year + "09";
        string m10 = DateTime.Today.Year + "10";
        string m11 = DateTime.Today.Year + "11";
        string m12 = DateTime.Today.Year + "12";
        string m1 = DateTime.Today.Year + 1 + "01";
        string m2 = DateTime.Today.Year + 1 + "02";
        string m3 = DateTime.Today.Year + 1 + "03";
        SQuery = "Select /*+ INDEX_DESC(voucher ind_VCH_DATE) */ b.Aname as Account,substr(a.acode,1,2) as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.Dramt-a.cramt),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.Dramt-a.cramt),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.Dramt-a.cramt),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.Dramt-a.cramt),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.Dramt-a.cramt),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.Dramt-a.cramt),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.Dramt-a.cramt),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.Dramt-a.cramt),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.Dramt-a.cramt),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.Dramt-a.cramt),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.Dramt-a.cramt),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.Dramt-a.cramt),0) as Mar,a.acode,b.bssch from voucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode) where a.branchcd = '" + frm_mbr + "' and a.vchdate " + DateRange + " and substr(a.acode,1,2)='16' and (substr(a.type,1,1)='4' or a.type='32' or a.type='53') group by a.acode,b.bssch,b.aname,to_char(vchdate,'yyyymm'),substr(a.acode,1,2)  ";
        SQuery = "Select x.Account,y.Name as Grp,to_char(sum(x.April+x.May+x.June+x.July+x.August+x.Sept+x.Oct+x.Nov+x.Dec+x.Jan+x.feb+x.mar)) as Totals,to_char(sum(x.April)) as April,to_char(sum(x.May)) as May,to_char(sum(x.June)) as June,to_Char(sum(x.July)) as July,to_char(sum(x.August)) as August,to_Char(sum(x.Sept)) as Sept,to_char(sum(x.oct)) as Oct,to_Char(sum(x.Nov)) as Nov,to_char(sum(x.Dec)) as Dec,to_Char(sum(x.Jan)) as Jan,to_char(sum(x.Feb)) as Feb,to_Char(sum(x.Mar)) as Mar,x.Acode,x.grp as grpx from (" + SQuery + ") x left outer join (select type1,name from typegrp where id='A') y on trim(x.bssch)=trim(y.type1) group by x.Grp,y.name,x.account,x.acode order by x.Grp,y.name,x.Account";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("Month Wise NET SALE Analysis (Sales-Dr.Note-Returns)", frm_qstr);
    }
    protected void BtnCshVouch_ServerClick(object sender, EventArgs e)
    {
        SQuery = "Select distinct a.Vchnum,a.vchdate,a.type,b.name from voucher a,(Select type1,name from type where id='V' and type1 in ('10','20')) b where a.branchcd='" + frm_mbr + "' and a.vchdate between to_date('" + frm_CDT1 + "','dd/mm/yyyy') AND to_Date('" + frm_CDT2 + "','dd/mm/yyyy') AND NVL(a.pflag,'-')<>'V' and a.type=b.type1 order by a.vchdate desc,a.type,a.vchnum desc";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("Verification of Vouchers", frm_qstr);
    }
    protected void BtnDebtAge_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "DEBTAGE";
        fgen.msg("-", "PMSG", "Select 1 for Above 90 Report 2 for 0-60 Report and 3 for All");
    }
    protected void BtnCreditAge_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "CREDITAGE";
        fgen.msg("-", "PMSG", "Select 1 for Above 90 Report 2 for 0-60 Report and 3 for All");
    }

    protected void BtnPurchTrend_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "PURCHTREND";
        fgen.Fn_open_PartyItemDateRangeBox("", frm_qstr);
    }
    protected void BtnDiRept_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "DIRPT";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnSaleAgentWise_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SALEAGENTWISE";
        fgen.msg("-", "SMSG", "Select Yes for ALL PLANT and NO for PLANT LOGGED IN");
    }
    protected void BtnSalesDespatch_ServerClick(object sender, EventArgs e)
    {
        string yymm = DateTime.Now.ToString("yyyyMM");
        SQuery = "Select branchcd,sum(amt_sale) as today,0 as mtd,0 as ytd from sale where branchcd!='DD' and type!='47' and vchdate=to_DatE('" + DateTime.Now.ToShortDateString() + "','dd/mm/yyyy') group by branchcd union all Select branchcd,0 as today,sum(amt_sale) as mtd,0 as ytd from sale where branchcd!='DD' and type!='47' and  to_char(vchdate,'yyyymm')='" + DateTime.Now.ToString("yyyyMM") + "' group by branchcd union all Select branchcd,0 as today,0 as mtd,sum(amt_sale) as ytd from sale where branchcd!='DD' and type!='47' and  vchdate " + DateRange + " group by branchcd";
        SQuery = "select b.name as Name_of_Unit,to_char(sum(today),'99,99,99,99,999') as Today,to_char(sum(mtd),'99,99,99,99,999') as MTD,to_char(sum(ytd),'99,99,99,99,999') as Ytd,a.branchcd,sum(today) as Todayd,sum(mtd) as MTDD,sum(ytd) as ytdd from (" + SQuery + ")a,type b where id='B' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            create_tab();
            sg1_dr = null;
            double ttot = 0, mtot = 0, ytot = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_f1"] = dt.Rows[i]["Name_of_Unit"].ToString().Trim();
                if (dt.Rows[i]["Today"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f2"] = fgen.make_double(dt.Rows[i]["Today"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f2"] = 0;
                }

                if (dt.Rows[i]["MTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f3"] = fgen.make_double(dt.Rows[i]["MTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f3"] = 0;
                }
                if (dt.Rows[i]["YTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["YTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f4"] = 0;
                }

                if (dt.Rows[i]["Todayd"].ToString().Trim().Length > 1)
                {
                    ttot = ttot + fgen.make_double(dt.Rows[i]["Todayd"].ToString().Trim());
                }
                else
                {
                    ttot = ttot + 0;
                }

                if (dt.Rows[i]["MTDD"].ToString().Trim().Length > 1)
                {
                    mtot = mtot + fgen.make_double(dt.Rows[i]["MTDD"].ToString().Trim());
                }
                else
                {
                    mtot = mtot + 0;
                }

                if (dt.Rows[i]["ytdd"].ToString().Trim().Length > 1)
                {
                    ytot = ytot + fgen.make_double(dt.Rows[i]["ytdd"].ToString().Trim());
                }
                else
                {
                    ytot = ytot + 0;
                }

                sg1_dt.Rows.Add(sg1_dr);
                ViewState["sg1"] = sg1_dt;
                fgen.EnableForm(this.Controls);
                disablectrl();
                setColHeadings();
            }
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f2"] = "===============";
            sg1_dr["sg1_f3"] = "===============";
            sg1_dr["sg1_f4"] = "===============";
            sg1_dt.Rows.Add(sg1_dr);

            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f1"] = "Showing Basic Sales";
            sg1_dr["sg1_f2"] = fgen.make_double(ttot, 2);
            sg1_dr["sg1_f3"] = fgen.make_double(mtot, 2);
            sg1_dr["sg1_f4"] = fgen.make_double(ytot, 2);
            sg1_dt.Rows.Add(sg1_dr);

            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            sg1_dt.Dispose();
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

    }
    protected void BtnPurchOrds_ServerClick(object sender, EventArgs e)
    {
        string yymm = DateTime.Now.ToString("yyyyMM");
        SQuery = "Select branchcd,sum(qtyord*(prate*(100-pdisc)/100)) as today,0 as mtd,0 as ytd from pomas where nvl(pflag,0)<>1 and branchcd!='DD' and type!='47' and orddt=to_DatE('" + DateTime.Now.ToShortDateString() + "','dd/mm/yyyy') group by branchcd union all Select branchcd,0 as today,sum(qtyord*(prate*(100-pdisc)/100)) as mtd,0 as ytd from pomas where nvl(pflag,0)<>1 and branchcd!='DD' and type!='47' and  to_char(orddt,'yyyymm')='" + yymm + "' group by branchcd union all Select branchcd,0 as today,0 as mtd,sum(qtyord*(prate*(100-pdisc)/100)) as ytd from pomas where nvl(pflag,0)<>1 and branchcd!='DD' and type!='47' and  orddt " + DateRange + " group by branchcd";
        SQuery = "select b.name as Name_of_Unit,to_char(sum(today)) as Today,to_char(sum(mtd)) as MTD,to_char(sum(ytd)) as Ytd,a.branchcd,sum(today) as Todayd,sum(mtd) as MTDD,sum(ytd) as ytdd from (" + SQuery + ")a,type b where id='B' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            create_tab();
            sg1_dr = null;
            double ttot = 0, mtot = 0, ytot = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_f1"] = dt.Rows[i]["Name_of_Unit"].ToString().Trim();
                if (dt.Rows[i]["Today"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f2"] = fgen.make_double(dt.Rows[i]["Today"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f2"] = 0;
                }

                if (dt.Rows[i]["MTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f3"] = fgen.make_double(dt.Rows[i]["MTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f3"] = 0;
                }
                if (dt.Rows[i]["YTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["YTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f4"] = 0;
                }

                if (dt.Rows[i]["Todayd"].ToString().Trim().Length > 1)
                {
                    ttot = ttot + fgen.make_double(dt.Rows[i]["Todayd"].ToString().Trim());
                }
                else
                {
                    ttot = ttot + 0;
                }

                if (dt.Rows[i]["MTDD"].ToString().Trim().Length > 1)
                {
                    mtot = mtot + fgen.make_double(dt.Rows[i]["MTDD"].ToString().Trim());
                }
                else
                {
                    mtot = mtot + 0;
                }

                if (dt.Rows[i]["ytdd"].ToString().Trim().Length > 1)
                {
                    ytot = ytot + fgen.make_double(dt.Rows[i]["ytdd"].ToString().Trim());
                }
                else
                {
                    ytot = ytot + 0;
                }

                sg1_dt.Rows.Add(sg1_dr);
                ViewState["sg1"] = sg1_dt;
                fgen.EnableForm(this.Controls);
                disablectrl();
                setColHeadings();
            }

            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f2"] = "===============";
            sg1_dr["sg1_f3"] = "===============";
            sg1_dr["sg1_f4"] = "===============";
            sg1_dt.Rows.Add(sg1_dr);

            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f1"] = "Showing Basic Sales";
            sg1_dr["sg1_f2"] = fgen.make_double(ttot, 2);
            sg1_dr["sg1_f3"] = fgen.make_double(mtot, 2);
            sg1_dr["sg1_f4"] = fgen.make_double(ytot, 2);
            sg1_dt.Rows.Add(sg1_dr);

            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            sg1_dt.Dispose();
        }
    }
    protected void BtnInwarVal_ServerClick(object sender, EventArgs e)
    {
        string yymm = DateTime.Now.ToString("yyyyMM");
        SQuery = "Select branchcd,sum(iqtyin*irate) as today,0 as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='0' and vchdate=to_DatE('" + DateTime.Now.ToShortDateString() + "','dd/mm/yyyy') group by branchcd union all Select branchcd,0 as today,sum(iqtyin*irate) as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='0' and  to_char(vchdate,'yyyymm')='" + yymm + "' group by branchcd union all Select branchcd,0 as today,0 as mtd,sum(iqtyin*irate) as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='0' and  vchdate " + DateRange + " group by branchcd";
        SQuery = "select b.name as Name_of_Unit,to_char(sum(today)) as Today,to_char(sum(mtd)) as MTD,to_char(sum(ytd)) as Ytd,a.branchcd,sum(today) as Todayd,sum(mtd) as MTDD,sum(ytd) as ytdd from (" + SQuery + ")a,type b where id='B' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            create_tab();
            sg1_dr = null;
            double ttot = 0, mtot = 0, ytot = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_f1"] = dt.Rows[i]["Name_of_Unit"].ToString().Trim();
                if (dt.Rows[i]["Today"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f2"] = fgen.make_double(dt.Rows[i]["Today"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f2"] = 0;
                }

                if (dt.Rows[i]["MTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f3"] = fgen.make_double(dt.Rows[i]["MTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f3"] = 0;
                }
                if (dt.Rows[i]["YTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["YTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f4"] = 0;
                }

                if (dt.Rows[i]["Todayd"].ToString().Trim().Length > 1)
                {
                    ttot = ttot + fgen.make_double(dt.Rows[i]["Todayd"].ToString().Trim());
                }
                else
                {
                    ttot = ttot + 0;
                }

                if (dt.Rows[i]["MTDD"].ToString().Trim().Length > 1)
                {
                    mtot = mtot + fgen.make_double(dt.Rows[i]["MTDD"].ToString().Trim());
                }
                else
                {
                    mtot = mtot + 0;
                }

                if (dt.Rows[i]["ytdd"].ToString().Trim().Length > 1)
                {
                    ytot = ytot + fgen.make_double(dt.Rows[i]["ytdd"].ToString().Trim());
                }
                else
                {
                    ytot = ytot + 0;
                }

                sg1_dt.Rows.Add(sg1_dr);
                ViewState["sg1"] = sg1_dt;
                fgen.EnableForm(this.Controls);
                disablectrl();
                setColHeadings();
            }

            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f2"] = "===============";
            sg1_dr["sg1_f3"] = "===============";
            sg1_dr["sg1_f4"] = "===============";
            sg1_dt.Rows.Add(sg1_dr);

            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f1"] = "Showing Basic Sales";
            sg1_dr["sg1_f2"] = fgen.make_double(ttot, 2);
            sg1_dr["sg1_f3"] = fgen.make_double(mtot, 2);
            sg1_dr["sg1_f4"] = fgen.make_double(ytot, 2);
            sg1_dt.Rows.Add(sg1_dr);

            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            sg1_dt.Dispose();
        }
    }
    protected void BtnProdGrpSale_ServerClick(object sender, EventArgs e)
    {
        string yymm = DateTime.Now.ToString("yyyyMM");
        if (frm_cocd == "ARIO" || frm_cocd == "NAVK")
        {
            SQuery = "Select substr(icode,1,2) as branchcd,sum(iamount) as today,0 as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type not in ('4G','47') and vchdate=to_DatE('" + DateTime.Now.ToShortDateString() + "','dd/mm/yyyy') group by substr(icode,1,2) union all Select substr(icode,1,2) as branchcd,0 as today,sum(iamount) as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type not in ('4G','47') and  to_char(vchdate,'yyyymm')='" + DateTime.Now.ToShortDateString() + "' group by substr(icode,1,2) union all Select substr(icode,1,2) as branchcd,0 as today,0 as mtd,sum(iamount) as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type not in ('4G','47') and  vchdate " + DateRange + " group by substr(icode,1,2)";
        }
        else
        {
            SQuery = "Select substr(icode,1,2) as branchcd,sum(iamount) as today,0 as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and vchdate=to_DatE('" + DateTime.Now.ToShortDateString() + "','dd/mm/yyyy') group by substr(icode,1,2) union all Select substr(icode,1,2) as branchcd,0 as today,sum(iamount) as mtd,0 as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and  to_char(vchdate,'yyyymm')='" + DateTime.Now.ToShortDateString() + "' group by substr(icode,1,2) union all Select substr(icode,1,2) as branchcd,0 as today,0 as mtd,sum(iamount) as ytd from ivoucher where branchcd!='DD' and substr(type,1,1)='4' and type!='47' and  vchdate " + DateRange + " group by substr(icode,1,2)";
        }
        if ((frm_cocd == "ARIO" || frm_cocd == "NAVK") || (frm_cocd == "ARCF" || frm_cocd == "HENA"))
        {
            SQuery = "select b.name as Name_of_Unit,to_char(sum(today)) as Today,to_char(sum(mtd)) as MTD,to_char(sum(ytd)) as Ytd,a.branchcd,sum(today) as Todayd,sum(mtd) as MTDD,sum(ytd) as ytdd from (" + SQuery + ")a,type b where substr(a.branchcd,1,1)='9' and b.id='Y' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";
        }
        else
        {
            SQuery = "select b.name as Name_of_Unit,to_char(sum(today)) as Today,to_char(sum(mtd)) as MTD,to_char(sum(ytd)) as Ytd,a.branchcd,sum(today) as Todayd,sum(mtd) as MTDD,sum(ytd) as ytdd from (" + SQuery + ")a,type b where b.id='Y' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";
        }

        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            create_tab();
            sg1_dr = null;
            double ttot = 0, mtot = 0, ytot = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_f1"] = dt.Rows[i]["Name_of_Unit"].ToString().Trim();
                if (dt.Rows[i]["Today"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f2"] = fgen.make_double(dt.Rows[i]["Today"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f2"] = 0;
                }

                if (dt.Rows[i]["MTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f3"] = fgen.make_double(dt.Rows[i]["MTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f3"] = 0;
                }
                if (dt.Rows[i]["YTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["YTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f4"] = 0;
                }

                if (dt.Rows[i]["Todayd"].ToString().Trim().Length > 1)
                {
                    ttot = ttot + fgen.make_double(dt.Rows[i]["Todayd"].ToString().Trim());
                }
                else
                {
                    ttot = ttot + 0;
                }

                if (dt.Rows[i]["MTDD"].ToString().Trim().Length > 1)
                {
                    mtot = mtot + fgen.make_double(dt.Rows[i]["MTDD"].ToString().Trim());
                }
                else
                {
                    mtot = mtot + 0;
                }

                if (dt.Rows[i]["ytdd"].ToString().Trim().Length > 1)
                {
                    ytot = ytot + fgen.make_double(dt.Rows[i]["ytdd"].ToString().Trim());
                }
                else
                {
                    ytot = ytot + 0;
                }

                sg1_dt.Rows.Add(sg1_dr);
                ViewState["sg1"] = sg1_dt;
                fgen.EnableForm(this.Controls);
                disablectrl();
                setColHeadings();
            }

            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f2"] = "===============";
            sg1_dr["sg1_f3"] = "===============";
            sg1_dr["sg1_f4"] = "===============";
            sg1_dt.Rows.Add(sg1_dr);

            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f1"] = "Showing Basic Sales";
            sg1_dr["sg1_f2"] = fgen.make_double(ttot, 2);
            sg1_dr["sg1_f3"] = fgen.make_double(mtot, 2);
            sg1_dr["sg1_f4"] = fgen.make_double(ytot, 2);
            sg1_dt.Rows.Add(sg1_dr);


            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            sg1_dt.Dispose();
        }
    }
    protected void BtnSalesOrd_ServerClick(object sender, EventArgs e)
    {
        string yymm = DateTime.Now.ToString("yyyyMM");
        SQuery = "Select branchcd,sum(qtyord*irate*decode(nvl(CURR_RATE,0),0,1,nvl(CURR_RATE,0))) as today,0 as mtd,0 as ytd from somas where trim(nvl(icat,'-'))<>'Y' and branchcd!='DD' and type!='47' and orddt=to_DatE('" + DateTime.Now.ToShortDateString() + "','dd/mm/yyyy') group by branchcd union all Select branchcd,0 as today,sum(qtyord*irate*decode(nvl(CURR_RATE,0),0,1,nvl(CURR_RATE,0))) as mtd,0 as ytd from somas where trim(nvl(icat,'-'))<>'Y' and branchcd!='DD' and type!='47' and  to_char(orddt,'yyyymm')='" + yymm + "' group by branchcd union all Select branchcd,0 as today,0 as mtd,sum(qtyord*irate*decode(nvl(CURR_RATE,0),0,1,nvl(CURR_RATE,0))) as ytd from somas where trim(nvl(icat,'-'))<>'Y' and branchcd!='DD' and type!='47' and  orddt " + DateRange + " group by branchcd";
        SQuery = "select b.name as Name_of_Unit,to_char(sum(today)) as Today,to_char(sum(mtd)) as MTD,to_char(sum(ytd)) as Ytd,a.branchcd,sum(today) as Todayd,sum(mtd) as MTDD,sum(ytd) as ytdd from (" + SQuery + ")a,type b where id='B' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            create_tab();
            sg1_dr = null;
            double ttot = 0, mtot = 0, ytot = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_f1"] = dt.Rows[i]["Name_of_Unit"].ToString().Trim();
                if (dt.Rows[i]["Today"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f2"] = fgen.make_double(dt.Rows[i]["Today"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f2"] = 0;
                }

                if (dt.Rows[i]["MTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f3"] = fgen.make_double(dt.Rows[i]["MTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f3"] = 0;
                }
                if (dt.Rows[i]["YTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["YTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f4"] = 0;
                }

                if (dt.Rows[i]["Todayd"].ToString().Trim().Length > 1)
                {
                    ttot = ttot + fgen.make_double(dt.Rows[i]["Todayd"].ToString().Trim());
                }
                else
                {
                    ttot = ttot + 0;
                }

                if (dt.Rows[i]["MTDD"].ToString().Trim().Length > 1)
                {
                    mtot = mtot + fgen.make_double(dt.Rows[i]["MTDD"].ToString().Trim());
                }
                else
                {
                    mtot = mtot + 0;
                }

                if (dt.Rows[i]["ytdd"].ToString().Trim().Length > 1)
                {
                    ytot = ytot + fgen.make_double(dt.Rows[i]["ytdd"].ToString().Trim());
                }
                else
                {
                    ytot = ytot + 0;
                }

                sg1_dt.Rows.Add(sg1_dr);
                ViewState["sg1"] = sg1_dt;
                fgen.EnableForm(this.Controls);
                disablectrl();
                setColHeadings();
            }

            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f2"] = "===============";
            sg1_dr["sg1_f3"] = "===============";
            sg1_dr["sg1_f4"] = "===============";
            sg1_dt.Rows.Add(sg1_dr);

            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f1"] = "Showing Basic Sales";
            sg1_dr["sg1_f2"] = fgen.make_double(ttot, 2);
            sg1_dr["sg1_f3"] = fgen.make_double(mtot, 2);
            sg1_dr["sg1_f4"] = fgen.make_double(ytot, 2);
            sg1_dt.Rows.Add(sg1_dr);

            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            sg1_dt.Dispose();
        }
    }
    protected void BtnProfitLossAc_ServerClick(object sender, EventArgs e)
    {
        string yymm = DateTime.Now.ToString("yyyyMM");
        SQuery = "Select branchcd,sum(dramt-cramt) as today,0 as mtd,0 as ytd from voucher where branchcd!='88' and substr(acode,1,1)>='2' and vchdate=to_DatE('" + DateTime.Now.ToShortDateString() + "','dd/mm/yyyy') group by branchcd union all Select branchcd,0 as today,sum(dramt-cramt) as mtd,0 as ytd from voucher where branchcd!='88' and substr(acode,1,1)>='2' and  to_char(vchdate,'yyyymm')='" + yymm + "' group by branchcd union all Select branchcd,0 as today,0 as mtd,sum(dramt-cramt) as ytd from voucher where branchcd!='88' and substr(acode,1,1)>='2' and  vchdate " + DateRange + " group by branchcd";
        SQuery = "select b.name as Name_of_Unit,to_char(sum(today)) as Today,to_char(sum(mtd)) as MTD,to_char(sum(ytd)) as Ytd,a.branchcd,sum(today) as Todayd,sum(mtd) as MTDD,sum(ytd) as ytdd from (" + SQuery + ")a,type b where id='B' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            create_tab();
            sg1_dr = null;
            double ttot = 0, mtot = 0, ytot = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_f1"] = dt.Rows[i]["Name_of_Unit"].ToString().Trim();
                if (dt.Rows[i]["Today"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f2"] = fgen.make_double(dt.Rows[i]["Today"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f2"] = 0;
                }

                if (dt.Rows[i]["MTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f3"] = fgen.make_double(dt.Rows[i]["MTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f3"] = 0;
                }
                if (dt.Rows[i]["YTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["YTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f4"] = 0;
                }

                if (dt.Rows[i]["Todayd"].ToString().Trim().Length > 1)
                {
                    ttot = ttot + fgen.make_double(dt.Rows[i]["Todayd"].ToString().Trim());
                }
                else
                {
                    ttot = ttot + 0;
                }

                if (dt.Rows[i]["MTDD"].ToString().Trim().Length > 1)
                {
                    mtot = mtot + fgen.make_double(dt.Rows[i]["MTDD"].ToString().Trim());
                }
                else
                {
                    mtot = mtot + 0;
                }

                if (dt.Rows[i]["ytdd"].ToString().Trim().Length > 1)
                {
                    ytot = ytot + fgen.make_double(dt.Rows[i]["ytdd"].ToString().Trim());
                }
                else
                {
                    ytot = ytot + 0;
                }

                sg1_dt.Rows.Add(sg1_dr);
                ViewState["sg1"] = sg1_dt;
                fgen.EnableForm(this.Controls);
                disablectrl();
                setColHeadings();
            }

            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f2"] = "===============";
            sg1_dr["sg1_f3"] = "===============";
            sg1_dr["sg1_f4"] = "===============";
            sg1_dt.Rows.Add(sg1_dr);

            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f1"] = "Showing Basic Sales";
            sg1_dr["sg1_f2"] = fgen.make_double(ttot, 2);
            sg1_dr["sg1_f3"] = fgen.make_double(mtot, 2);
            sg1_dr["sg1_f4"] = fgen.make_double(ytot, 2);
            sg1_dt.Rows.Add(sg1_dr);

            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            sg1_dt.Dispose();
        }
    }
    protected void BtnCollect_ServerClick(object sender, EventArgs e)
    {
        string yymm = DateTime.Now.ToString("yyyyMM");
        SQuery = "Select branchcd,sum(cramt-dramt) as today,0 as mtd,0 as ytd from voucher where branchcd!='88' and substr(type,1,1)='1' and substr(acode,1,2) in('16') and vchdate=to_DatE('" + DateTime.Now.ToShortDateString() + "','dd/mm/yyyy') group by branchcd union all Select branchcd,0 as today,sum(cramt-dramt) as mtd,0 as ytd from voucher where branchcd!='88' and substr(type,1,1)='1' and substr(acode,1,2) in('16') and to_char(vchdate,'yyyymm')='" + yymm + "' group by branchcd union all Select branchcd,0 as today,0 as mtd,sum(cramt-dramt) as ytd from voucher where branchcd!='88' and substr(type,1,1)='1' and substr(acode,1,2) in('16') and vchdate " + DateRange + " group by branchcd";
        SQuery = "select b.name as Name_of_Unit,to_char(sum(today)) as Today,to_char(sum(mtd)) as MTD,to_char(sum(ytd)) as Ytd,a.branchcd,sum(today) as Todayd,sum(mtd) as MTDD,sum(ytd) as ytdd from (" + SQuery + ")a,type b where id='B' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            create_tab();
            sg1_dr = null;
            double ttot = 0, mtot = 0, ytot = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_f1"] = dt.Rows[i]["Name_of_Unit"].ToString().Trim();
                if (dt.Rows[i]["Today"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f2"] = fgen.make_double(dt.Rows[i]["Today"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f2"] = 0;
                }

                if (dt.Rows[i]["MTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f3"] = fgen.make_double(dt.Rows[i]["MTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f3"] = 0;
                }
                if (dt.Rows[i]["YTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["YTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f4"] = 0;
                }

                if (dt.Rows[i]["Todayd"].ToString().Trim().Length > 1)
                {
                    ttot = ttot + fgen.make_double(dt.Rows[i]["Todayd"].ToString().Trim());
                }
                else
                {
                    ttot = ttot + 0;
                }

                if (dt.Rows[i]["MTDD"].ToString().Trim().Length > 1)
                {
                    mtot = mtot + fgen.make_double(dt.Rows[i]["MTDD"].ToString().Trim());
                }
                else
                {
                    mtot = mtot + 0;
                }

                if (dt.Rows[i]["ytdd"].ToString().Trim().Length > 1)
                {
                    ytot = ytot + fgen.make_double(dt.Rows[i]["ytdd"].ToString().Trim());
                }
                else
                {
                    ytot = ytot + 0;
                }

                sg1_dt.Rows.Add(sg1_dr);
                ViewState["sg1"] = sg1_dt;
                fgen.EnableForm(this.Controls);
                disablectrl();
                setColHeadings();
            }
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f2"] = "===============";
            sg1_dr["sg1_f3"] = "===============";
            sg1_dr["sg1_f4"] = "===============";
            sg1_dt.Rows.Add(sg1_dr);
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f1"] = "Showing Collection for Debtors";
            sg1_dr["sg1_f2"] = fgen.make_double(ttot, 2);
            sg1_dr["sg1_f3"] = fgen.make_double(mtot, 2);
            sg1_dr["sg1_f4"] = fgen.make_double(ytot, 2);
            sg1_dt.Rows.Add(sg1_dr);

            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            sg1_dt.Dispose();
        }
    }
    protected void BtnPaymnt_ServerClick(object sender, EventArgs e)
    {
        string yymm = DateTime.Now.ToString("yyyyMM");
        SQuery = "Select branchcd,sum(dramt-cramt) as today,0 as mtd,0 as ytd from voucher where branchcd!='88' and substr(type,1,1)='2' and substr(acode,1,2) in('05','06') and vchdate=to_DatE('" + DateTime.Now.ToShortDateString() + "','dd/mm/yyyy') group by branchcd union all Select branchcd,0 as today,sum(dramt-cramt) as mtd,0 as ytd from voucher where branchcd!='88' and substr(type,1,1)='2' and substr(acode,1,2) in('05','06') and to_char(vchdate,'yyyymm')='" + yymm + "' group by branchcd union all Select branchcd,0 as today,0 as mtd,sum(dramt-cramt) as ytd from voucher where branchcd!='88' and substr(type,1,1)='2' and substr(acode,1,2) in('05','06') and vchdate " + DateRange + " group by branchcd";
        SQuery = "select b.name as Name_of_Unit,to_char(sum(today)) as Today,to_char(sum(mtd)) as MTD,to_char(sum(ytd)) as Ytd,a.branchcd,sum(today) as Todayd,sum(mtd) as MTDD,sum(ytd) as ytdd from (" + SQuery + ")a,type b where id='B' and a.branchcd=b.type1 group by b.name,a.branchcd order by a.branchcd";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            create_tab();
            sg1_dr = null;
            double ttot = 0, mtot = 0, ytot = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_f1"] = dt.Rows[i]["Name_of_Unit"].ToString().Trim();
                if (dt.Rows[i]["Today"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f2"] = fgen.make_double(dt.Rows[i]["Today"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f2"] = 0;
                }

                if (dt.Rows[i]["MTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f3"] = fgen.make_double(dt.Rows[i]["MTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f3"] = 0;
                }
                if (dt.Rows[i]["YTD"].ToString().Trim().Length > 1)
                {
                    sg1_dr["sg1_f4"] = fgen.make_double(dt.Rows[i]["YTD"].ToString().Trim(), 2);
                }
                else
                {
                    sg1_dr["sg1_f4"] = 0;
                }

                if (dt.Rows[i]["Todayd"].ToString().Trim().Length > 1)
                {
                    ttot = ttot + fgen.make_double(dt.Rows[i]["Todayd"].ToString().Trim());
                }
                else
                {
                    ttot = ttot + 0;
                }

                if (dt.Rows[i]["MTDD"].ToString().Trim().Length > 1)
                {
                    mtot = mtot + fgen.make_double(dt.Rows[i]["MTDD"].ToString().Trim());
                }
                else
                {
                    mtot = mtot + 0;
                }

                if (dt.Rows[i]["ytdd"].ToString().Trim().Length > 1)
                {
                    ytot = ytot + fgen.make_double(dt.Rows[i]["ytdd"].ToString().Trim());
                }
                else
                {
                    ytot = ytot + 0;
                }

                sg1_dt.Rows.Add(sg1_dr);
                ViewState["sg1"] = sg1_dt;
                fgen.EnableForm(this.Controls);
                disablectrl();
                setColHeadings();
            }
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f2"] = "===============";
            sg1_dr["sg1_f3"] = "===============";
            sg1_dr["sg1_f4"] = "===============";
            sg1_dt.Rows.Add(sg1_dr);
            sg1_dr = sg1_dt.NewRow();
            sg1_dr["sg1_f1"] = "Showing Payments To Creditors";
            sg1_dr["sg1_f2"] = fgen.make_double(ttot, 2);
            sg1_dr["sg1_f3"] = fgen.make_double(mtot, 2);
            sg1_dr["sg1_f4"] = fgen.make_double(ytot, 2);
            sg1_dt.Rows.Add(sg1_dr);

            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            sg1_dt.Dispose();
        }
    }
    protected void BtnSaleVsCol_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SALEVSCOL";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
}