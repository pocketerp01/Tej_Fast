using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;

using System.IO;

public partial class om_RoadMap : System.Web.UI.Page
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

        //sg1_add_blankrows();
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
        dt = new DataTable();
        SQuery = "Select distinct Mile_stone from Impl_road order by Mile_stone";
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0) {
            create_tab();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sg1_dr = sg1_dt.NewRow();
                //sg1_dr["sg1_Srno"] = sg1_dt.Rows.Count + 1;
                sg1_dr["sg1_f1"] = dt.Rows[i]["Mile_stone"].ToString().Trim();
                sg1_dt.Rows.Add(sg1_dr);
                fgen.EnableForm(this.Controls);
                disablectrl();
                //setColHeadings();

                ViewState["sg1"] = sg1_dt;

            }

            //sg1_add_blankrows();
            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            sg1_dt.Dispose();
        }
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
        else if (hffield.Value == "Show") {

            col2 = Request.Cookies["reply"].Value.ToString();
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", col2);

            

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
                    //fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);                    
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
            fgen.msg("-", "CMSG", "Select Yes for Open Jobs and No for Pend. SO.!");
            
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

            sg1.Columns[0].HeaderStyle.Width = 300;
           
            
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

            //sg2.Columns[0].HeaderStyle.Width = 400;
            //sg2.Columns[1].HeaderStyle.Width = 200;
            //sg2.Columns[2].HeaderStyle.Width = 100;
            //sg2.Columns[3].HeaderStyle.Width = 100;
            //sg2.Columns[4].HeaderStyle.Width = 200;
            //sg2.Columns[5].HeaderStyle.Width = 450;

            //TableCell cell = e.Row.Cells[1];
            //cell.BackColor = Color.GreenYellow;

            TableCell cell = e.Row.Cells[3];
            
            double quantity = fgen.make_double(cell.Text.Replace("%",""));
            
            if (quantity<=50)
            {
                cell.BackColor = Color.Yellow;
            }
            else if (quantity > 50 && quantity < 90)
            {
                cell.BackColor = Color.Cyan;
            }
            else if (quantity > 90)
            {
                cell.BackColor = Color.YellowGreen;
            }
            
            
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

        string Milestone = ""; string tblqry1 = ""; string tblqry2 = "";

        Milestone = sg1.SelectedRow.Cells[0].Text;

        SQuery = "select * from impl_road where trim((Mile_Stone))='" + Milestone.ToString().Trim() + "' order by srno";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            create_tab1();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sg2_dr = sg2_dt.NewRow();
                sg2_dr["sg2_f1"] = dt.Rows[i]["Sub_Stone"].ToString().Trim();
                tblqry1 = dt.Rows[i]["Comp_Std"].ToString().Trim();
                tblqry1 = tblqry1.Replace("FULLYEAR", "" + DateRange + "");
                tblqry1 = tblqry1.Replace("`", "'");
                tblqry1 = tblqry1.Replace("CURRYEAR", "YR_"+ DateTime.Now.Year.ToString() +"");
                tblqry1 = tblqry1.Replace("DATEFLD", "" + DateRange + "");
                tblqry2 = dt.Rows[i]["Comp_act"].ToString().Trim();
                tblqry2 = tblqry2.Replace("DATEFLD", "" + DateRange + "");
                tblqry2 = tblqry2.Replace("FULLYEAR", "" + DateRange + "");
                tblqry2 = tblqry2.Replace("`", "'");
                sg2_dr["sg2_f2"] = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "" + tblqry1 + "", "cnt"));
                sg2_dr["sg2_f3"] = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "" + tblqry2 + "", "cnt"));

                if (fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "" + tblqry1 + "", "cnt")) > 0 && fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "" + tblqry2 + "", "cnt")) > 0)
                {
                    sg2_dr["sg2_f4"] = Math.Round((fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "" + tblqry2 + "", "cnt")) / fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "" + tblqry1 + "", "cnt"))) * 100,2) + "%";
                }

                sg2_dt.Rows.Add(sg2_dr);
                fgen.EnableForm(this.Controls);
                disablectrl();              
                ViewState["sg2"] = sg2_dt;
           
            }

            sg2.DataSource = sg2_dt;
            sg2.DataBind();
            sg2_dt.Dispose();
        }
           
            //ViewState["sg2"] = sg2_dt;
            //sg2.DataSource = sg2_dt;
            //sg2.DataBind();
            //dt.Dispose(); 
    }
}