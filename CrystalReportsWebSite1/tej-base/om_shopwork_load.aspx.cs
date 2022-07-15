using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;

using System.IO;

public partial class om_shopwork_load : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
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
    string frm_tabname, frm_tabname1, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
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
                //getColHeading();
            }
            //setColHeadings();
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
        btnlist.Visible = true;
        btnTraExc.Visible = true;
        //if (frm_cocd == "MSES") divCan.Visible = false;
        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {

        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false; btnTraExc.Disabled = false;

        create_tab();
        create_tab1();
        create_tab2();

        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();

        //btnlbl4.Enabled = false;
        //btnlbl7.Enabled = false;

        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();

        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();

    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {

        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
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
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();


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

            string rel1 = ""; string rel2 = ""; int colmcount = 0; string mfld1 = "";

            rel1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim();
            rel2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim();
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            if (rel2 == "Y")
            {
                mfld1 = "load_1";
                showcap.Text = "Hrs Shown as (8.5 Hr = 1 Day)";
            }
            else if (rel2 == "N")
            {
                mfld1 = "load_2";
                showcap.Text = "Hrs Shown as (11 Hr = 1 Day)";
            }
            else if (rel2 == "A")
            {
                mfld1 = "load_3";
                showcap.Text = "Hrs Shown as (22.5 Hr = 1 Day)";
            }
            else
            {
                showcap.Text = "";
            }

            double mdivider = 1;

            if (rel1 == "Y")
            {
                SQuery = "select * from ( select max(srno) as srnos,Name,Stagec,round(sum(Job_Stg_Mins)/60,0) as Load_1,round((sum(Job_Stg_Mins)/60)/11,0) as Load_2,round((sum(Job_Stg_Mins)/60)/22.5,0) as Load_3 from ( select b.srno,a.icode,a.qty,c.name,b.stagec,c.rate as Mk_Ready,c.excrate as Prodn_1K,(c.excrate/1000)*a.qty as Prodn_Time,c.rate+((c.excrate)*(a.qty/1000)) as Job_Stg_Mins,b.mtime,b.mtime1,b.hcut from costestimate a, itwstage b,type c where c.id='K' and trim(b.stagec)=trim(c.type1) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + PrdRange + " and trim(A.icode)=trim(B.icode) and a.srno=1 and a.status!='Y' and trim(nvl(a.app_by,'-'))!='-' ) Group by Name,Stagec ) order by srnos";
            }
            else if (rel1 == "N")
            {
                SQuery = "select * from (select max(srno) as srnos,Name,Stagec,round(sum(Job_Stg_Mins)/60,0) as Load_1,round((sum(Job_Stg_Mins)/60)/11,0) as Load_2,round((sum(Job_Stg_Mins)/60)/22.5,0) as Load_3 from ( select b.srno,a.icode,a.qty,c.name,b.stagec,c.rate as Mk_Ready,c.excrate as Prodn_1K,(c.excrate/1000)*a.qty as Prodn_Time,c.rate+((c.excrate)*(a.qty/1000)) as Job_Stg_Mins,b.mtime,b.mtime1,b.hcut from ( select trim(icode) as icode,sum(Bal_qty) as qty from ( select trim(a.Order_No) as Order_No,a.Orddt,sum(a.qtyord)as Ord_qty,sum(a.salqty) as Sal_Qty,sum(a.qtyord)-sum(a.salqty) as Bal_qty,trim(a.acode) as Acode,trim(a.icode)As Icode from ( select type,pordno,porddt,0 as stk,a.cu_chldt,a.ordno as Order_No,a.orddt as Orddt,a.acode,a.icode,a.qtyord,0 as sch_qty,0 as prd_qty,0 as jcqty,0 as salqty,null as jcdt,null as invdt,icat,notify,BUSI_EXPECT,null as jcno from somas a where a.branchcd='" + frm_mbr + "' and a.type!='45' and A.orddt  " + PrdRange + " and trim(a.acode) like '%' and trim(a.icode) like '%' union all select null as type,null as pordno,null as porddt,0 as stk,null as cu_Chldt,a.vchnum as Order_No,a.vchdate as Orddt,a.acode,a.icode,0 as qty_ord,a.budgetcost as Qty1,a.actualcost as Qty2,0 as jcqty,0 as salqty,null as jcdt,null as invdt,null as icat,null as notify,null as notify1,null as jcno   from budgmst a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and A.vchdate " + PrdRange + " and trim(a.acode) like '%' and trim(a.icode) like '%' union all select null as type,null as pordno,null as porddt,0 as stk,null as cu_Chldt,substr(a.convdate,5,6) as Order_No,to_DaTE(substr(a.convdate,11,10),'dd/mm/yyyy') as Orddt,a.acode,a.icode,0 as qty_ord,0 as Qty1,0 as Qty2,qty as jcqty,0 as salqty ,vchdate as jcdt,null as invdt,null as icat,null as notify,null as notify1,vchnum as jcno  from costestimate a where a.branchcd='" + frm_mbr + "' and a.type like '30%' and A.vchdate " + PrdRange + " and trim(a.acode) like '%' and trim(a.srno)=0 and trim(a.icode) like '%' union all select  null as type,null as pordno,null as porddt,0 as stk,null as cu_Chldt,a.ponum as Order_No,a.podate as Orddt,a.acode,a.icode,0 as qtyord,0 as sch_qty,0 as prd_qty,0 as jcqty,iqtyout as salqty,null as jcdt,vchdate as invdt,null as icat,null as notify,null as notify1,null as jcno   from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%'and A.vchdate " + PrdRange + " and trim(a.acode) like '%' and trim(a.icode) like '%') a group by a.Order_No,a.Orddt,trim(a.acode),trim(a.icode) having sum(a.qtyord)-sum(a.salqty)>0 and max(a.icat)<>'Y'  ) group by trim(icode)) a, itwstage b,type c where c.id='K' and trim(b.stagec)=trim(c.type1) and trim(A.icode)=trim(B.icode) ) Group by Name,Stagec ) order by srnos ";
            }
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

            colmcount = Convert.ToInt32(fgen.seek_iname(frm_qstr, frm_cocd, "select max(" + mfld1 + ") as Colcnt from (" + SQuery + ")", "Colcnt"));

            if (colmcount > 20)
            {
                if (rel2 == "Y")
                {
                    mdivider = 8.5;
                }
                else
                {
                    mdivider = 2;
                }
            }

            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                fgen.EnableForm(this.Controls);
                disablectrl();
                edmode.Value = "Y";
                create_tab();
                sg1_dr = null;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sg1_dr = sg1_dt.NewRow();
                    double loop_lim = 0;
                    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                    sg1_dr["sg1_f1"] = dt.Rows[i]["Name"].ToString().Trim();
                    sg1_dr["sg1_f2"] = dt.Rows[i]["stagec"].ToString().Trim();

                    if (rel2 == "Y")
                    {
                        sg1_dr["sg1_f3"] = dt.Rows[i]["Load_1"].ToString().Trim();
                        loop_lim = (fgen.make_double(dt.Rows[i]["Load_1"].ToString().Trim()) / mdivider);
                    }
                    else if (rel2 == "N")
                    {
                        sg1_dr["sg1_f3"] = dt.Rows[i]["Load_2"].ToString().Trim();
                        loop_lim = (fgen.make_double(dt.Rows[i]["Load_2"].ToString().Trim()) / mdivider);
                    }
                    else if (rel2 == "A")
                    {
                        sg1_dr["sg1_f3"] = dt.Rows[i]["Load_3"].ToString().Trim();
                        loop_lim = (fgen.make_double(dt.Rows[i]["Load_3"].ToString().Trim()) / mdivider);
                    }

                    if (loop_lim > 75)
                    {
                        loop_lim = 75;
                    }

                    //int ctr = 4;

                    for (int j = 0; j < loop_lim; j++)
                    {

                        sg1_dr["sg1_f" + (4 + j) + ""] = "Y";

                    }
                    sg1_dt.Rows.Add(sg1_dr);
                    fgen.EnableForm(this.Controls);
                    disablectrl();
                    //setColHeadings();

                    ViewState["sg1"] = sg1_dt;
                }





            }

            sg1_add_blankrows();
            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            sg1_dt.Dispose();


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

                    fgen.msg("-", "PMSG", "Select Report Data'13'1 = for Hours'13'2 = for One Shift'13'3 = for Two Shift");
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



                        sg1_add_blankrows();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        sg1_dt.Dispose();


                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

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
                            sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        if (col1.Length > 8) SQuery = "select * from item where trim(icode)='" + col1 + "' ";
                        else SQuery = "select * from item where trim(icode)='" + col1 + "' ";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);



                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;

                            sg1_dr["sg1_f1"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_t1"] = "0";
                            sg1_dr["sg1_t2"] = dt.Rows[d]["irate"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);



                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    // ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

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

                    //setColHeadings();
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
            fgen.msg("-", "CMSG", "Please Select Report Data'13''13'Yes = for Open Jobs'13'No = for Pending SO.");

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
                    xcountrows = sg1.Rows.Count;

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

        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {




            oDS.Tables[0].Rows.Add(oporow);

        }




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

    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(sg1, "Select$" + e.Row.RowIndex);
            e.Row.Attributes["style"] = "cursor:pointer";

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

            sg1.Columns[0].HeaderStyle.Width = 50;
            sg1.Columns[1].HeaderStyle.Width = 220;
            sg1.Columns[2].HeaderStyle.Width = 220;
            sg1.Columns[3].HeaderStyle.Width = 100;
            sg1.Columns[4].HeaderStyle.Width = 80;
            sg1.Columns[5].HeaderStyle.Width = 80;
            sg1.Columns[6].HeaderStyle.Width = 70;
            sg1.Columns[7].HeaderStyle.Width = 70;
            sg1.Columns[8].HeaderStyle.Width = 70;
            sg1.Columns[9].HeaderStyle.Width = 70;
            sg1.Columns[10].HeaderStyle.Width = 70;
            sg1.Columns[11].HeaderStyle.Width = 60;
            sg1.Columns[12].HeaderStyle.Width = 70;
            sg1.Columns[13].HeaderStyle.Width = 70;



            for (int i = 4; i <= 75; i++)
            {
                TableCell cell = e.Row.Cells[i];
                string quantity = cell.Text;
                if (quantity == "Y")
                {
                    cell.BackColor = Color.GreenYellow;
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
        int rowIndex = 0;
        // int rowIndex = ((GridViewRow)((Button)e.CommandSource).NamingContainer).RowIndex;
        //int index = Convert.ToInt32(sg1.Rows[rowIndex]);


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
        sg2_dt.Columns.Add(new DataColumn("sg2_f5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f6", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f7", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f8", typeof(string)));

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
        sg1_dt.Columns.Add(new DataColumn("sg1_f21", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f22", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f23", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f24", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f25", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f26", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f27", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f28", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f29", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f30", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f31", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f32", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f33", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f34", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f35", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f36", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f37", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f38", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f39", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f40", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f41", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f42", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f43", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f44", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f45", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f46", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f47", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f48", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f49", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f50", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f51", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f52", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f53", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f54", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f55", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f56", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f57", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f58", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f59", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f60", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f61", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f62", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f63", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f64", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f65", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f66", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f67", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f68", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f69", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f70", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f71", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f72", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f73", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f74", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f75", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f76", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f77", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f78", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f79", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f80", typeof(string)));
    }
    //------------------------------------------------------------------------------------

    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();
        sg2_dr["sg2_Srno"] = sg2_dt.Rows.Count + 1;
        sg2_dr["sg2_f1"] = "-";
        sg2_dr["sg2_f2"] = "-";
        sg2_dr["sg2_f3"] = "-";
        sg2_dr["sg2_f4"] = "-";
        sg2_dr["sg2_f5"] = "-";
        sg2_dr["sg2_f6"] = "-";
        sg2_dr["sg2_f7"] = "-";
        sg2_dr["sg2_f8"] = "-";

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
        sg1_dr["sg1_f21"] = "-";
        sg1_dr["sg1_f22"] = "-";
        sg1_dr["sg1_f23"] = "-";
        sg1_dr["sg1_f24"] = "-";
        sg1_dr["sg1_f25"] = "-";
        sg1_dr["sg1_f26"] = "-";
        sg1_dr["sg1_f27"] = "-";
        sg1_dr["sg1_f28"] = "-";
        sg1_dr["sg1_f29"] = "-";
        sg1_dr["sg1_f30"] = "-";
        sg1_dr["sg1_f31"] = "-";
        sg1_dr["sg1_f32"] = "-";
        sg1_dr["sg1_f33"] = "-";
        sg1_dr["sg1_f34"] = "-";
        sg1_dr["sg1_f35"] = "-";
        sg1_dr["sg1_f36"] = "-";
        sg1_dr["sg1_f37"] = "-";
        sg1_dr["sg1_f38"] = "-";
        sg1_dr["sg1_f39"] = "-";
        sg1_dr["sg1_f40"] = "-";
        sg1_dr["sg1_f41"] = "-";
        sg1_dr["sg1_f42"] = "-";
        sg1_dr["sg1_f43"] = "-";
        sg1_dr["sg1_f44"] = "-";
        sg1_dr["sg1_f45"] = "-";
        sg1_dr["sg1_f46"] = "-";
        sg1_dr["sg1_f47"] = "-";
        sg1_dr["sg1_f48"] = "-";
        sg1_dr["sg1_f49"] = "-";
        sg1_dr["sg1_f50"] = "-";
        sg1_dr["sg1_f51"] = "-";
        sg1_dr["sg1_f52"] = "-";
        sg1_dr["sg1_f53"] = "-";
        sg1_dr["sg1_f54"] = "-";
        sg1_dr["sg1_f55"] = "-";
        sg1_dr["sg1_f56"] = "-";
        sg1_dr["sg1_f57"] = "-";
        sg1_dr["sg1_f58"] = "-";
        sg1_dr["sg1_f59"] = "-";
        sg1_dr["sg1_f60"] = "-";
        sg1_dr["sg1_f61"] = "-";
        sg1_dr["sg1_f62"] = "-";
        sg1_dr["sg1_f63"] = "-";
        sg1_dr["sg1_f64"] = "-";
        sg1_dr["sg1_f65"] = "-";
        sg1_dr["sg1_f66"] = "-";
        sg1_dr["sg1_f67"] = "-";
        sg1_dr["sg1_f68"] = "-";

        sg1_dr["sg1_f69"] = "-";
        sg1_dr["sg1_f70"] = "-";
        sg1_dr["sg1_f71"] = "-";
        sg1_dr["sg1_f72"] = "-";
        sg1_dr["sg1_f73"] = "-";
        sg1_dr["sg1_f74"] = "-";
        sg1_dr["sg1_f75"] = "-";
        sg1_dr["sg1_f76"] = "-";
        sg1_dr["sg1_f77"] = "-";
        sg1_dr["sg1_f78"] = "-"; sg1_dr["sg1_f79"] = "-"; sg1_dr["sg1_f80"] = "-";

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



        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        //setColHeadings();
        ViewState["sg1"] = sg1_dt;



        set_Val();
        #endregion
    }

    // added 22/04/2020 :: VV
    //protected void btnCamera_ServerClick(object sender, EventArgs e)
    //{
    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
    //    hffield.Value = "";
    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL10", frm_mbr + frm_vty + txtvchnum.Value + Convert.ToDateTime(txtvchdate.Value).ToString("dd_MM_yyyy"));
    //    fgen.open_sseek_camera("", frm_qstr);
    //}

    protected void btnComp_Click1(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CompTyp";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Complaint Type", frm_qstr);
    }
    protected void sg1_btnadd_Click(object sender, ImageClickEventArgs e)
    {
    }



    protected void BtnEnqTyp_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Enq";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Enquiry Type", frm_qstr);
    }
    protected void BtnPriorit_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Priority";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Priority", frm_qstr);
    }
    protected void BtnDom_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Dom";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Domestic/Export", frm_qstr);
    }
    protected void BtnZone_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Zone";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Domestic/Export", frm_qstr);
    }
    protected void BtnRef_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void BtnPers_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Person";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Our Person", frm_qstr);
    }



    protected void btnClient_Click1(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "Client";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer", frm_qstr);
    }



    protected void btnTraExc_ServerClick1(object sender, EventArgs e)
    {
        if (sg1.Rows.Count > 0)
            fgen.ExportGridToExcel(sg1, frm_cocd + "_Delivery_Details_" + DateTime.Now.ToString().Trim() + ".xls");
        else fgen.msg("-", "AMSG", "No Data To Export");
        //dt = new DataTable();
        //dt = (DataTable)ViewState["sg1"];
        //if (dt.Rows.Count > 0) fgen.exp_to_excel(dt, "ms-excel", "xls", frm_cocd + "_" + DateTime.Now.ToString().Trim());
        //else fgen.msg("-", "AMSG", "No Data To Export");
        //dt.Dispose();
    }
    protected void btnGet_ServerClick(object sender, EventArgs e)
    {
        //Get the button that raised the event
        Button btn = (Button)sender;

        //Get the row that contains this button
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
    }
    protected void btntrsexc2_ServerClick(object sender, EventArgs e)
    {
        if (sg2.Rows.Count > 0)
            fgen.ExportGridToExcel(sg2, frm_cocd + "_Information_" + DateTime.Now.ToString().Trim() + ".xls");
        else fgen.msg("-", "AMSG", "No Data To Export");

        //dt = new DataTable();
        //dt = (DataTable)ViewState["sg2"];
        //if (dt.Rows.Count > 0) fgen.exp_to_excel(dt, "ms-excel", "xls", frm_cocd + "_" + DateTime.Now.ToString().Trim());
        //else fgen.msg("-", "AMSG", "No Data To Export");
        //dt.Dispose();
    }
    protected void btnJobOrdStat_ServerClick(object sender, EventArgs e)
    {

    }
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            for (int sg1r = 0; sg1r < sg2.Rows.Count; sg1r++)
            {
                for (int j = 0; j < sg2.Columns.Count; j++)
                {
                    sg2.Rows[sg1r].Cells[j].ToolTip = sg2.Rows[sg1r].Cells[j].Text;
                    if (sg2.Rows[sg1r].Cells[j].Text.Trim().Length > 50)
                    {
                        sg2.Rows[sg1r].Cells[j].Text = sg2.Rows[sg1r].Cells[j].Text.Substring(0, 50);
                    }
                }


            }

            sg2.Columns[0].HeaderStyle.Width = 40;
            sg2.Columns[1].HeaderStyle.Width = 300;
            sg2.Columns[2].HeaderStyle.Width = 100;
            sg2.Columns[3].HeaderStyle.Width = 200;
            sg2.Columns[4].HeaderStyle.Width = 200;
            sg2.Columns[5].HeaderStyle.Width = 450;

            TableCell cell = e.Row.Cells[1];
            cell.BackColor = Color.GreenYellow;


        }
    }
    protected void sg2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg2.Rows[rowIndex].RowIndex);


        switch (var)
        {
            case "SG2_RMV":
                if (index < sg2.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG2_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;


            case "SG2_ROW_ADD":


                if (index < sg2.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG2_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG2_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Customer Name", frm_qstr); // CHANGE ITEM TO ITEMS BY MADHVI ON 23 JULY 2018
                    //fgen.Fn_open_mseek("Select Item", frm_qstr);
                }
                break;
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        //required to avoid the run time error "  
        //Control 'GridView1' of type 'Grid View' must be placed inside a form tag with runat=server."  
    }
    protected void sg1_SelectedIndexChanged(object sender, EventArgs e)
    {

        string customername = "", iname = "", col4, col5, col6;

        iname = sg1.SelectedRow.Cells[2].Text;

        string rel3 = ""; string rel4 = "";

        rel3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim();
        rel4 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim();
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

        if (rel3 == "Y")
        {
            SQuery = "select * from (select enqdt,vchnum,vchdate,Iname,Icode,max(qty) as Qty,nvl(Cpartno,'-') as Cpartno,round(sum(Job_Stg_Mins)/60,2) as Load_1,round((sum(Job_Stg_Mins)/60)/11,2) as Load_2,round((sum(Job_Stg_Mins)/60)/22.5,2) as Load_3 from (select a.enqdt,a.vchnum,a.vchdate,d.iname,d.cpartno,b.srno,a.icode,a.qty,c.name,b.stagec,c.rate as Mk_Ready,c.excrate as Prodn_1K,(c.excrate/1000)*a.qty as Prodn_Time,c.rate+((c.excrate)*(a.qty/1000)) as Job_Stg_Mins,b.mtime,b.mtime1,b.hcut from costestimate a, itwstage b,type c,item d where c.type1='" + iname + "' and c.id='K' and trim(b.stagec)=trim(c.type1) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + PrdRange + " and trim(A.icode)=trim(d.icode) and trim(A.icode)=trim(B.icode) and a.srno=1 and a.status!='Y' and trim(nvl(a.app_by,'-'))!='-' ) Group by Iname,Icode,nvl(Cpartno,'-'),vchnum,vchdate,enqdt ) order by vchdate";
        }
        else if (rel3 == "N")
        {
            SQuery = "select * from (select enqdt,vchnum,vchdate,Iname,Icode,max(qty) as Qty,nvl(Cpartno,'-') as Cpartno,round(sum(Job_Stg_Mins)/60,2) as Load_1,round((sum(Job_Stg_Mins)/60)/11,2) as Load_2,round((sum(Job_Stg_Mins)/60)/22.5,2) as Load_3 from (select a.cu_chldt as enqdt,a.order_no as vchnum,a.orddt vchdate,d.iname,d.cpartno,b.srno,a.icode,a.qty,c.name,b.stagec,c.rate as Mk_Ready,c.excrate as Prodn_1K,(c.excrate/1000)*a.qty as Prodn_Time,c.rate+((c.excrate)*(a.qty/1000)) as Job_Stg_Mins,b.mtime,b.mtime1,b.hcut from (select max(cu_chldt)as cu_chldt,order_no,orddt,trim(icode) as icode,sum(Bal_qty) as qty from (select max(a.cu_chldt) as cu_chldt,trim(a.Order_No) as Order_No,a.Orddt,sum(a.qtyord)as Ord_qty,sum(a.salqty) as Sal_Qty,sum(a.qtyord)-sum(a.salqty) as Bal_qty,         trim(a.acode) as Acode,trim(a.icode)As Icode from (select type,pordno,porddt,0 as stk,a.cu_chldt,a.ordno as Order_No,a.orddt as Orddt,a.acode,a.icode,a.qtyord,0 as sch_qty,0 as prd_qty,0 as jcqty,0 as salqty,null as jcdt,null as invdt,icat,notify,BUSI_EXPECT,null as jcno from somas a where a.branchcd='" + frm_mbr + "' and a.type!='45' and A.orddt " + PrdRange + " and trim(a.acode) like '%' and trim(a.icode) like '%' union all select null as type,null as pordno,null as porddt,0 as stk,null as cu_Chldt,a.vchnum as Order_No,a.vchdate as Orddt,a.acode,a.icode,0 as qty_ord,a.budgetcost as Qty1,a.actualcost as Qty2,0 as jcqty,0 as salqty,null as jcdt,null as invdt,null as icat,null as notify,null as notify1,null as jcno   from budgmst a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and A.vchdate " + PrdRange + " and trim(a.acode) like '%' and trim(a.icode) like '%'           union all select null as type,null as pordno,null as porddt,0 as stk,null as cu_Chldt,substr(a.convdate,5,6) as Order_No,to_DaTE(substr(a.convdate,11,10),'dd/mm/yyyy') as Orddt,a.acode,a.icode,0 as qty_ord,0 as Qty1,0 as Qty2,qty as jcqty,0 as salqty ,vchdate as jcdt,null as invdt,null as icat,null as notify,null as notify1,vchnum as jcno  from costestimate a where a.branchcd='" + frm_mbr + "' and a.type like '30%' and A.vchdate " + PrdRange + " and trim(a.acode) like '%' and trim(a.srno)=0 and trim(a.icode) like '%'            union all select  null as type,null as pordno,null as porddt,0 as stk,null as cu_Chldt,a.ponum as Order_No,a.podate as Orddt,a.acode,a.icode,0 as qtyord,0 as sch_qty,0 as prd_qty,0 as jcqty,iqtyout as salqty,null as jcdt,vchdate as invdt,null as icat,null as notify,null as notify1,null as jcno   from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%'and A.vchdate " + PrdRange + " and trim(a.acode) like '%' and trim(a.icode) like '%') a group by a.Order_No,a.Orddt,trim(a.acode),trim(a.icode) having sum(a.qtyord)-sum(a.salqty)>0 and max(a.icat)<>'Y') group by order_no,orddt,trim(icode)) a, itwstage b,type c,item d where c.type1='" + iname + "' and c.id='K' and trim(b.stagec)=trim(c.type1) and trim(A.icode)=trim(d.icode) and trim(A.icode)=trim(B.icode) ) Group by Iname,Icode,nvl(Cpartno,'-'),vchnum,vchdate,enqdt ) order by vchdate";
        }


        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);


        if (dt.Rows.Count > 0)
        {
            create_tab1();


            for (int i = 0; i < dt.Rows.Count; i++)
            {

                sg2_dr = sg2_dt.NewRow();
                sg2_dr["sg2_Srno"] = sg2_dt.Rows.Count + 1;
                sg2_dr["sg2_f1"] = dt.Rows[i]["iname"].ToString().Trim();
                sg2_dr["sg2_f2"] = dt.Rows[i]["icode"].ToString().Trim();
                sg2_dr["sg2_f3"] = dt.Rows[i]["cpartno"].ToString().Trim();

                sg2_dr["sg2_f4"] = dt.Rows[i]["vchnum"].ToString().Trim();
                sg2_dr["sg2_f5"] = Convert.ToDateTime(dt.Rows[i]["vchdate"].ToString().Trim()).ToShortDateString();
                sg2_dr["sg2_f6"] = dt.Rows[i]["qty"].ToString().Trim();

                if (rel4 == "Y")
                {
                    sg2_dr["sg2_f7"] = dt.Rows[i]["Load_1"].ToString().Trim();
                }
                else if (rel4 == "N")
                {
                    sg2_dr["sg2_f7"] = dt.Rows[i]["Load_2"].ToString().Trim();
                }
                else if (rel4 == "A")
                {
                    sg2_dr["sg2_f7"] = dt.Rows[i]["Load_3"].ToString().Trim();
                }
                sg2_dr["sg2_f8"] = Convert.ToDateTime(dt.Rows[i]["enqdt"].ToString().Trim()).ToShortDateString();
                sg2_dt.Rows.Add(sg2_dr);
            }
        }


        ViewState["sg2"] = sg2_dt;
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        dt.Dispose();
    }
}