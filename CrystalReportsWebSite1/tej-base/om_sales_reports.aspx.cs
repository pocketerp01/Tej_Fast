using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;

using System.IO;

public partial class om_sales_reports : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", Tax_GCC = "", OVER_SEAS = "", iscons = "";
    DataTable dt, dt2, dt3, dt4;
    DataRow oporow; DataSet oDS, oDs1;
    int i = 0, z = 0;
    int ADDER = 0;

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

        //sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();



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
            case "GROSSRPT":
                SQuery = "Select 'Total by Customer ' as fstr,'Total by Customer ' as Grouping,'B.Aname as Grp_by,' as Fieldn,'B.aname,' as Grp_fld from dual union all Select 'Total by State ' as fstr,'Total by State ' as Grouping,'B.Staten as Grp_by,' as Fieldn,'B.staten,' as Grp_fld from dual union all Select 'Total by Country ' as fstr,'Total by Country ' as Grouping,'B.Country as Grp_by,' as Fieldn,'B.Country,' as Grp_fld from dual";
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

        else if (hffield.Value == "ITEMQTY")
        {
            fgen.msg("-", "SMSG", "Do you want to see Consolidated Report?");
            hffield.Value = "ITEMQTYY";

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

        else if (hffield.Value == "SEARCHMTH")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            string Prt_RAV = "Y";
            string mthchar = Convert.ToDateTime(fromdt).Year.ToString().Trim();
            if (Convert.ToDateTime(fromdt).Month.ToString().Trim().Length == 1)
            {
                mthchar = mthchar + "0" + Convert.ToDateTime(fromdt).Month.ToString().Trim();
            }
            else
            {
                mthchar = mthchar + Convert.ToDateTime(fromdt).Month.ToString().Trim();
            }
            if (Prt_RAV == "Y")
            {
                SQuery = "select x.acode,x.icode,x.sch,nvl(y.desp,0) as desp from(select a.acode,a.icode,sum(a.budgetcost) as sch from budgmst a where trim(a.acode)='" + col1.Trim() + "' and a.branchcd='" + frm_mbr + "' and a.type='46' and to_char(a.dlv_Date,'yyyymm')='" + mthchar + "' group by a.acode,a.icode) x left outer join (select b.acode,b.icode,sum(b.iqtyout) as desp from ivoucher b where trim(b.acode)='" + col1.Trim() + "' and b.store='Y' and b.branchcd='" + frm_mbr + "' and b.type like ('4%') and to_char(b.vchdate,'yyyymm')='" + mthchar + "' group by b.acode,b.icode ) y on trim(x.acode)||trim(x.icode)=trim(y.acode)||trim(y.icode)";
            }
            else
            {
                SQuery = "select x.acode,x.icode,x.sch,nvl(y.desp,0) as desp from(select a.acode,a.icode,sum(a.total) as sch from schedule a where trim(a.acode)='" + col1.Trim() + "' and a.branchcd='" + frm_mbr + "' and a.type='46' and to_char(a.vchdate,'yyyymm')='" + mthchar + "' group by a.acode,a.icode) x left outer join (select b.acode,b.icode,sum(b.iqtyout) as desp from ivoucher b where trim(b.acode)='" + col1.Trim() + "' and b.store='Y' and b.branchcd='" + frm_mbr + "' and b.type like ('4%') and to_char(b.vchdate,'yyyymm')='" + mthchar + "' group by b.acode,b.icode ) y on trim(x.acode)||trim(x.icode)=trim(y.acode)||trim(y.icode)";
            }

            SQuery = "Select n.icode as fstr, M.iname as Item,m.Cpartno as Drwg_No,sum(n.sch) as Schedule,sum(n.desp) as Despatch,decode(sum(n.sch),0,'N/A',round(sum(n.desp)/sum(n.sch)*100,2)) as Desp_Percent,sum(n.sch)-sum(n.Desp) as Bal_Qty from (" + SQuery + ") n,Item m where trim(m.icode)=trim(n.icode) group by n.icode,m.iname,m.cpartno order by m.iname";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("Item Wise Despatch Details", frm_qstr);
            hffield.Value = "SEARCHMTHPART";

        }

        else if (hffield.Value == "SEARCHMTHPART")
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            string Prt_RAV = "Y";
            string mthchar = Convert.ToDateTime(fromdt).Year.ToString().Trim();
            if (Convert.ToDateTime(fromdt).Month.ToString().Trim().Length == 1)
            {
                mthchar = mthchar + "0" + Convert.ToDateTime(fromdt).Month.ToString().Trim();
            }
            else
            {
                mthchar = mthchar + Convert.ToDateTime(fromdt).Month.ToString().Trim();
            }

            SQuery = "select X.CPARTNO,x.opening as opening , nvl(y.cdbts,0) as  Receipts, nvl(y.ccdts,0) as Issues,x.opening+nvl(y.cdbts,0)-nvl(y.ccdts,0) as Closing from (select a.icode,a.iname,A.CPARTNO,A.unit,A.imax,A.imin,A.iord,a.opening_bal+nvl(b.newop,0) opening,a.grp from  (select substr(f.icode,1,2) as grp,f.icode,f.iname,F.CPARTNO,F.UNIT,F.imax,F.imin,F.iord,sum(nvl(fb." + DateTime.Today.Year + ",0)) as opening_bal from item f left outer join (select icode,Yr_" + DateTime.Today.Year + " from itembal where trim(icode)='" + col1.Trim() + "' and branchcd='" + frm_mbr + "') fb on trim(f.icode)=trim(fb.icode) where trim(f.icode)='" + col1.Trim() + "' and f.branchcd <>'DD' group by substr(f.icode,1,2),f.icode,f.iname,F.CPARTNO,F.UNIT,F.imax,F.imin,F.iord) a left outer join  (select v.icode,nvl(sum(v.iqtyin),0)-nvl(sum(v.iqtyout),0) newop from ivoucher v where trim(v.icode)='" + col1.Trim() + "' and v.store='Y' and v.branchcd ='" + frm_mbr + "' and v.vchdate between TO_DATE('" + fromdt + "','DD/MM/YYYY') and TO_DATE('" + Convert.ToDateTime(fromdt).AddDays(-1).ToShortDateString() + "','DD/MM/YYYY') group by v.icode  ) b on trim(a.icode)=trim(b.icode) ) x left outer join (select v.icode,sum(v.iqtyin) cdbts,sum(v.iqtyout)ccdts from ivoucher v where trim(v.icode)='" + col1.Trim() + "' and v.store='Y' and v.branchcd ='" + frm_mbr + "' and v.vchdate between TO_DATE('" + fromdt + "','DD/MM/YYYY') and TO_DATE('" + todt + "','DD/MM/YYYY') group by v.icode) y on trim(x.icode)=trim(y.icode) ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Stock Statement for This Item " + PrdRange + "", frm_qstr);
            hffield.Value = "-";

        }

        else if (hffield.Value == "PARTYWISEQTY")
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
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            SQuery = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname)||' '||trim(b.cpartno) as Item,substr(a.icode,1,4) as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iqtyout),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iqtyout),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iqtyout),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iqtyout),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iqtyout),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iqtyout),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iqtyout),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iqtyout),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iqtyout),0) as Dec,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iqtyout),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iqtyout),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iqtyout),0) as Mar,a.icode,b.unit from ivoucher a left outer join item b on a.icode=b.icode where a.branchcd = '" + frm_mbr + "' and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' and a.type!='47' and trim(a.acode)='" + col1.Trim() + "' group by a.icode,b.unit,substr(a.icode,1,4),trim(b.Iname)||' '||trim(b.cpartno),to_char(vchdate,'yyyymm')  ";
            string Cons_mbr = "MTHCONS1";
            string MY_SIZE = "MTHLYREPI";
            SQuery = "Select Item,unit,to_char(sum(April),'99,99,99,999.99') as April,to_char(sum(May),'99,99,99,999.99') as May,to_char(sum(June),'99,99,99,999.99') as June,to_Char(sum(July),'99,99,99,999.99') as July,to_char(sum(August),'99,99,99,999.99') as August,to_Char(sum(Sept),'99,99,99,999.99') as Sept,to_char(sum(oct),'99,99,99,999.99') as Oct,to_Char(sum(Nov),'99,99,99,999.99') as Nov,to_char(sum(Dec),'99,99,99,999.99') as Dec,to_Char(sum(Jan),'99,99,99,999.99') as Jan,to_char(sum(Feb),'99,99,99,999.99') as Feb,to_Char(sum(Mar),'99,99,99,999.99') as Mar,grp,icode from (" + SQuery + ") group by item,grp,icode,unit order by grp,item";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Month Wise Sales ( Qty ) For " + frm_mbr + " " + PrdRange + "", frm_qstr);
            hffield.Value = "-";

        }

        else if (hffield.Value == "QUACOMP")
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
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");

            SQuery = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname)||' '||trim(b.cpartno) as Item,' ' as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iqtyout),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iqtyout),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iqtyout),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iqtyout),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iqtyout),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iqtyout),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iqtyout),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iqtyout),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iqtyout),0) as Dec,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iqtyout),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iqtyout),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iqtyout),0) as Mar,a.icode from ivoucher a left outer join item b on a.icode=b.icode where a.branchcd = '" + frm_mbr + "' and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' and trim(a.acode)='" + col1.Trim() + "' group by a.icode,trim(b.Iname)||' '||trim(b.cpartno ),to_char(vchdate,'yyyymm')  ";
            string MY_SIZE = "JOBREPP";
            SQuery = "Select Item,to_char(sum(April+may+june),'99,99,99,999.99') as Qtr1,to_char(sum(July+August+sept),'99,99,99,999.99') as Qtr2,to_char(sum(oct+nov+Dec),'99,99,99,999.99') as Qtr3,to_Char(sum(Jan+feb+mar),'99,99,99,999.99') as Qtr4,icode from (" + SQuery + ") group by item,grp,icode order by item";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Qtr Wise Sales ( Qty ) For " + frm_mbr + " " + PrdRange + "", frm_qstr);
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

        else if (hffield.Value == "MTHSCH")
        {

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select distinct to_char(a.vchdate,'yyyy MONTH') as Month_name,acode as sch_Val,null as Sale_val,to_Char(A.vchdate,'yyyymm') as mthyr from schedule a where a.branchcd='" + frm_mbr + "' and a.type='46' and a.vchdate " + PrdRange + " union all select distinct to_char(a.vchdate,'yyyy MONTH') as Month_name,null as sch_Val,acode as Sale_val,to_Char(A.vchdate,'yyyymm') as mthyr from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type not in ('45','47') and a.vchdate " + PrdRange + "";
            SQuery = "select Month_name,to_Char(count(sch_Val),'999,99,99,999') as sch_Customer,to_Char(count(sale_Val),'999,99,99,999') as Sale_Customers,(Case when count(sch_val)>0 then round((count(sale_val)/count(sch_val))*100,2) else 0 end) as Perc_ach,mthyr from (" + SQuery + ") group by Month_name,mthyr order by Mthyr";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Schedule Vs Sale customer Count between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(todt).AddDays(Convert.ToInt32(col1)).ToShortDateString() + "','dd/mm/yyyy')", frm_qstr);
            hffield.Value = "-";
        }



        else if (hffield.Value == "GROSSRPT")
        {
            fgen.msg("-", "SMSG", "Do You Want Consolidated Report?");
            hffield.Value = "GROSSRPT";
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
            //fgen.Fn_open_rptlevel("", frm_qstr);
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
            fgen.Fn_open_rptlevel("", frm_qstr);
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
        else if (hffield.Value == "LASTVSCURR")
        {
            hf2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fgen.Fn_open_prddmp1("", frm_qstr);
            hffield.Value = "LASTVSCURRR";
        }
        else if (hffield.Value == "SCHVL")
        {
            hf2.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fgen.Fn_open_prddmp1("", frm_qstr);
            hffield.Value = "SCHVLL";
        }
        else if (hffield.Value == "SCHVLL")
        {
            col1 = hf2.Value;
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "Select trim(a.acode) as Acode,Sum(a.total*a.irate) as Prd1,0 as prd2 from schedule a where a.branchcd='" + frm_mbr + "' and a.vchdate " + col1 + " group by trim(a.acode) union all  Select trim(a.acode) as Acode,0 as prd1,Sum(a.total*a.irate) as prd2 from schedule a where a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " group by trim(a.acode)";
            SQuery = "Select b.Aname as Customer,to_char(Sum(a.Prd1),'999,99,99,999.99') as Period_1,to_char(sum(a.prd2),'999,99,99,999.99')as Period_2,to_char((Sum(a.Prd1)-sum(a.prd2))*-1,'999,99,99,999.99') as Difference,(Case when Sum(a.Prd1)-sum(a.prd2)<0 then 'Increase' else 'Decrease' end) as Sch_Position from (Select trim(a.acode) as Acode,Sum(a.total*a.irate) as Prd1,0 as prd2 from schedule a where a.branchcd='" + frm_mbr + "' and a.type='46' and a.vchdate " + col1 + " group by trim(a.acode) union all  Select trim(a.acode) as Acode,0 as prd1,Sum(a.total*a.irate) as prd2 from schedule a where a.branchcd='" + frm_mbr + "' and a.type='46' and a.vchdate " + PrdRange + " group by trim(a.acode)) a, famst b where trim(A.acode)=trim(b.acode) group by b.Aname,trim(A.acode) Order by B.aname";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Schedule Qty Comparison", frm_qstr);
        }
        else if (hffield.Value == "LASTVSCURRR")
        {
            col1 = hf2.Value;
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "Select trim(a.acode) as Acode,Sum(a.total) as Prd1,0 as prd2 from schedule a where a.branchcd='" + frm_mbr + "' and a.vchdate " + col1 + " group by trim(a.acode) union all  Select trim(a.acode) as Acode,0 as prd1,Sum(a.total) as prd2 from schedule a where a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " group by trim(a.acode)";
            SQuery = "Select b.Aname as Customer,Sum(a.Prd1) as Period_1,sum(a.prd2)as Period_2,(Sum(a.Prd1)-sum(a.prd2))*-1 as Difference,(Case when Sum(a.Prd1)-sum(a.prd2)<0 then 'Increase' else 'Decrease' end) as Sch_Position from (Select trim(a.acode) as Acode,Sum(a.total) as Prd1,0 as prd2 from schedule a where a.branchcd='" + frm_mbr + "' and a.type='46' and a.vchdate " + col1 + " group by trim(a.acode) union all  Select trim(a.acode) as Acode,0 as prd1,Sum(a.total) as prd2 from schedule a where a.branchcd='" + frm_mbr + "' and a.type='46' and a.vchdate " + PrdRange + " group by trim(a.acode)) a, famst b where trim(A.acode)=trim(b.acode) group by b.Aname,trim(A.acode) Order by B.aname";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Schedule Qty Comparison", frm_qstr);

        }
        else if (hffield.Value == "EWAY")
        {

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "Select nvl(e.place,'-') As Doc_type,nvl(e.exc_item,'-') As Subtype,a.branchcd,A.vchnum,a.vchdate,b.pincode,b.aname,b.addr1,b.addr2,b.addr3,b.gst_no,b.staten,b.staffcd as St_code,c.iname,c.hscode,a.iqtyout,a.irate,a.iamount,round(a.iqtyout*a.iexc_Addl,2) as Tool_Cost,round(a.iqtyout*a.ipack,2) as pack_Cost,round(a.iqtyout*a.idiamtr,2) as frt_Cost,(Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.iopr)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.iopr)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.iopr)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.iopr)='IG' then a.exc_amt else 0 end) as IGST_amt,a.Invno,A.Invdate,a.type,c.unit,d.brdist_kms from  ivoucher a, famst b , item c,famstbal d,type e where a.type=e.type1 and e.id='V' and d.branchcd='" + frm_mbr + "' and trim(a.acode)=trim(b.acode) and trim(a.acode)=trim(d.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + PrdRange + " ";
            SQuery = "Select 'Outward' as Supply_type,a.Subtype,a.Doc_type,a.vchnum as Doc_No,a.vchdate as Doc_Date,a.aname as To_other_party,a.gst_no as To_GSTIN,a.addr1 as to_Address1,a.addr2 as to_Address2,a.addr3 as to_Place,a.pincode as To_Pin_Code,a.staten as to_state,b.exc_item as Product,b.exc_item as Descriptions,a.HSCODE,a.unit,sum(a.iqtyout)as Qty_tot,sum(a.iamount)+sum(a.Tool_Cost)+sum(a.pack_Cost)+sum(a.frt_Cost) as Taxable_Val ,a.CGST_RT||'-'||a.SGST_Rate||'-'||a.IGST_Rt||'-'||0 as Tax_Rate,sum(a.CGST_amt) as CGST_amt,sum(a.SGST_amt) as SGST_amt,sum(a.IGST_amt) as IGST_amt,0 as Cess_Amt, b.mode_tpt,a.brdist_kms as Distance,b.ins_no as Trans_Name,'-' as Trans_Id,b.grno as Trans_docno,b.Grdate as Trans_Date,replace(replace(b.mo_Vehi,'-',''),' ','') as Vehicle_no from (" + SQuery + ") a, sale b where trim(a.branchcd)||a.type||a.vchnum||to_Char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||b.type||b.vchnum||to_Char(b.vchdate,'dd/mm/yyyy') group by a.vchnum,a.vchdate,a.aname,a.gst_no,a.addr1,a.addr2,a.addr3,a.pincode,a.staten,b.exc_item,b.exc_item,a.HSCODE,a.unit,a.CGST_RT,a.SGST_Rate,a.IGST_Rt ,b.mode_tpt,a.brdist_kms,b.ins_no,b.grno,b.Grdate,replace(replace(b.mo_Vehi,'-',''),' ',''),a.Subtype,a.Doc_type order by a.vchnum,a.vchdate";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Invoice Search " + PrdRange + "", frm_qstr);

        }
        else if (hffield.Value == "MASTWT")
        {

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            SQuery = "select E.Aname as customer,a.purpose as Description,a.exc_57f4 as Part_code,a.finvno as ORder_no,A.vCHNUM AS Bill_No,a.vchdate as Bill_Dt,sum(a.iqtyout) as Quantity,sum(a.iqtyout*b.iweight) as Weight,a.acode from ivoucher a,item b,sale c,type d,famst e where trim(A.acode)=trim(E.acode) and d.id='V' and a.type=d.type1 and a.branchcd||a.type||a.vchnum||to_char(A.vchdate,'dd/mm/yyyy')=c.branchcd||c.type||c.vchnum||to_char(c.vchdate,'dd/mm/yyyy') and trim(a.icode)=trim(B.icodE) and a.branchcd ='" + frm_mbr + "'  and a.type like '4%' and a.vchdate " + PrdRange + " group by E.Aname,a.acode,a.purpose,a.exc_57f4,a.finvno,A.vCHNUM,a.vchdate order by a.vchdate,a.vchnum ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Invoice Wise Report " + PrdRange + "", frm_qstr);

        }
        else if (hffield.Value == "COMVSTAX")
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


            if (Tax_GCC == "Y" || OVER_SEAS == "Y") ADDER = 0;
            string m4 = DateTime.Today.Year + "04";
            string m5 = DateTime.Today.Year + "05";
            string m6 = DateTime.Today.Year + "06";
            string m7 = DateTime.Today.Year + "07";
            string m8 = DateTime.Today.Year + "08";
            string m9 = DateTime.Today.Year + "09";
            string m10 = DateTime.Today.Year + "10";
            string m11 = DateTime.Today.Year + "11";
            string m12 = DateTime.Today.Year + "12";
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";

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

            SQuery = "select b.aname,c.iname,a.ordno,a.orddt,sum(a.qtyord) As qtyord,sum(a.da) As da,sum(a.ci) As ci ,sum(a.ti) As ti,sum(a.qtyord)-sum(a.ci) As Bal_Ci,sum(a.qtyord)-sum(a.ti) As Bal_ti,trim(a.acode) as acode,trim(a.icode) As icode,max(a.pordno) As pordno,max(a.porddt) as porddt  from (Select pordno,porddt,ordno,orddt,acode,icode,qtyord,0 as da,0 as ci ,0 as ti from somas where branchcd='00' and  acode " + party_cd + " and icode " + part_cd + " union all Select null as pordno,null as porddt,ordno,orddt,acode,icode,0 as qtyord,qtysupp as da,0 as ci ,0 as ti from despatch where branchcd!='DD' and  acode " + party_cd + " and icode " + part_cd + " union all Select null as pordno,null as porddt,ponum,podate,acode,icode,0 as qtyord,0 as da,iqtyout as ci ,0 as ti from ivoucherp where branchcd!='DD' and type like '4%' and acode " + party_cd + " and icode " + part_cd + " union all Select null as pordno,null as porddt,ponum,podate,acode,icode,0 as qtyord,0 as da,0 as ci ,iqtyout as ti from ivoucher where branchcd!='DD' and type like '4%' and  acode " + party_cd + " and icode " + part_cd + ")a,famst b,item c where trim(A.acode)=trim(b.acode) and trim(A.icode)=trim(c.icode) group by b.aname,c.iname,a.ordno,a.orddt,trim(a.acode),trim(a.icode) ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Comm inv Vs Tax Invoice" + PrdRange + " ", frm_qstr);
        }
        else if (hffield.Value == "INVDEBT")
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

            SQuery = "Select a.vchnum as Bill_no,a.vchdate as Bill_dt,b.aname as Customer,a.purpose as Item_name,a.exc_57f4 as Part_no,sum(A.iqtyout) as tot_qty,a.irate,a.icode,a.acode from ivoucher a, famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + PrdRange + " and a.acode " + party_cd + " and a.icode " + part_cd + " group by a.vchnum,a.vchdate,b.aname,a.purpose,a.exc_57f4,a.icode,a.acode,a.irate order by a.vchdate,a.vchnum";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Invoice Level Summary" + PrdRange + " ", frm_qstr);
        }
        else if (hffield.Value == "DATEWISESALE")
        {

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select a.vchdate as Dated,to_char(sum(a.bill_qty),'99,99,99,999.99') as Quantity,to_char(sum(a.amt_sale),'99,99,99,999.99') as Basic_Value,to_char(sum(a.Bill_tot),'99,99,99,999.99') as Gross_Value,to_char(sum(a.amt_exc),'99,99,99,999.99') as Taxes1,to_char(sum(a.rvalue),'99,99,99,999.99') as Taxes2 from sale a where a.vchdate " + PrdRange + " and a.branchcd ='" + frm_mbr + "'  and a.type<>'47' group by a.vchdate order by a.vchdate ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Day Wise Sales : Quantity , Basic , Gross ", frm_qstr);

        }
        else if (hffield.Value == "TARIFFCHECK")
        {

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select vchnum,vchdate,count(pname) as cnt from (Select distinct vchnum,vchdate,pname from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + PrdRange + ") group by vchnum,vchdate having count(pname)>1 order by vchdate,vchnum";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Inv With More than 1 tarrif " + PrdRange + "", frm_qstr);

        }
        else if (hffield.Value == "MAINGRPSALE")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                col2 = "a.branchcd != 'DD'";
            }
            else
            {
                col2 = "a.branchcd = '" + frm_mbr + "'";
            }

            string m4 = DateTime.Today.Year + "04";
            string m5 = DateTime.Today.Year + "05";
            string m6 = DateTime.Today.Year + "06";
            string m7 = DateTime.Today.Year + "07";
            string m8 = DateTime.Today.Year + "08";
            string m9 = DateTime.Today.Year + "09";
            string m10 = DateTime.Today.Year + "10";
            string m11 = DateTime.Today.Year + "11";
            string m12 = DateTime.Today.Year + "12";
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";

            SQuery = "Select a.branchcd,'Qty' as Rep,substr(a.icode,1,4) as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iqtyout),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iqtyout),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iqtyout),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iqtyout),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iqtyout),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iqtyout),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iqtyout),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iqtyout),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iqtyout),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iqtyout),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iqtyout),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iqtyout),0) as Mar from ivoucher a where " + col2 + " and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' group by a.branchcd,substr(a.icode,1,4),to_char(vchdate,'yyyymm')  union all Select a.branchcd,'Value' as Rep,substr(a.icode,1,4) as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iamount),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iamount),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iamount),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iamount),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iamount),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iamount),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iamount),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iamount),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iamount),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iamount),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iamount),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iamount),0) as Mar from ivoucher a where " + col2 + " and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' group by a.branchcd,substr(a.icode,1,4),to_char(vchdate,'yyyymm')";
            SQuery = "Select Y.Iname as Sub_Grp,Y.NAme as Main_Grp,x.Rep,to_char(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar),'99,99,99,999.99') as Totals,to_char(sum(x.April),'99,99,99,999.99') as April,to_char(sum(x.May),'99,99,99,999.99') as May,to_char(sum(x.June),'99,99,99,999.99') as June,to_Char(sum(x.July),'99,99,99,999.99') as July,to_char(sum(x.August),'99,99,99,999.99') as August,to_Char(sum(x.Sept),'99,99,99,999.99') as Sept,to_char(sum(x.oct),'99,99,99,999.99') as Oct,to_Char(sum(x.Nov),'99,99,99,999.99') as Nov,to_char(sum(x.Dec),'99,99,99,999.99') as Dec,to_Char(sum(x.Jan),'99,99,99,999.99') as Jan,to_char(sum(x.Feb),'99,99,99,999.99') as Feb,to_Char(sum(x.Mar),'99,99,99,999.99') as Mar,x.grp,y.type1 as mg_code  from (" + SQuery + ") x left outer join (select b.type1,b.NAme,a.Icode,a.iname from item a,type b where b.id='Y' and substr(a.icode,1,2)=trim(B.type1) and length(Trim(a.icode))=4) y on trim(x.grp)=trim(y.icode) where 1=1 group by y.type1,y.NAme,Y.Iname,x.grp,x.Rep order by x.Grp,x.Rep";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Main Grp,Sub Group Wise Month Wise Sales ( Qty / Value ) Analysis ", frm_qstr);

        }
        else if (hffield.Value == "MAINGRPORD")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                col2 = "a.branchcd != 'DD'";
            }
            else
            {
                col2 = "a.branchcd = '" + frm_mbr + "'";
            }

            string m4 = DateTime.Today.Year + "04";
            string m5 = DateTime.Today.Year + "05";
            string m6 = DateTime.Today.Year + "06";
            string m7 = DateTime.Today.Year + "07";
            string m8 = DateTime.Today.Year + "08";
            string m9 = DateTime.Today.Year + "09";
            string m10 = DateTime.Today.Year + "10";
            string m11 = DateTime.Today.Year + "11";
            string m12 = DateTime.Today.Year + "12";
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";

            string amt_fld = "(QTYORD*(a.irate*((100-a.cdisc)/100))*decode(a.CURR_RATE,0,1,curr_Rate))";

            SQuery = "Select a.branchcd,'Qty' as Rep,substr(a.icode,1,4) as grp,decode(to_chaR(orddt,'yyyymm')," + m4 + ",sum(a.qtyord),0) as April,decode(to_chaR(orddt,'yyyymm')," + m5 + ",sum(a.qtyord),0) as May,decode(to_chaR(orddt,'yyyymm')," + m6 + ",sum(a.qtyord),0) as June,decode(to_chaR(orddt,'yyyymm')," + m7 + ",sum(a.qtyord),0) as July,decode(to_chaR(orddt,'yyyymm')," + m8 + ",sum(a.qtyord),0) as August,decode(to_chaR(orddt,'yyyymm')," + m9 + ",sum(a.qtyord),0) as Sept,decode(to_chaR(orddt,'yyyymm')," + m10 + ",sum(a.qtyord),0) as Oct,decode(to_chaR(orddt,'yyyymm')," + m11 + ",sum(a.qtyord),0) as Nov,decode(to_chaR(orddt,'yyyymm')," + m12 + ",sum(a.qtyord),0) as Dec,decode(to_chaR(orddt,'yyyymm')," + m1 + ",sum(a.qtyord),0) as Jan,decode(to_chaR(orddt,'yyyymm')," + m2 + ",sum(a.qtyord),0) as Feb,decode(to_chaR(orddt,'yyyymm')," + m3 + ",sum(a.qtyord),0) as Mar from somas a where " + col2 + " and a.orddt " + DateRange + " and substr(a.type,1,1)='4' group by a.branchcd,substr(a.icode,1,4),to_char(orddt,'yyyymm')  union all Select a.branchcd,'Value' as Rep,substr(a.icode,1,4) as grp,decode(to_chaR(orddt,'yyyymm')," + m4 + ",sum(" + amt_fld + "),0) as April,decode(to_chaR(orddt,'yyyymm')," + m5 + ",sum(" + amt_fld + "),0) as May,decode(to_chaR(orddt,'yyyymm')," + m6 + ",sum(" + amt_fld + "),0) as June,decode(to_chaR(orddt,'yyyymm')," + m7 + ",sum(" + amt_fld + "),0) as July,decode(to_chaR(orddt,'yyyymm')," + m8 + ",sum(" + amt_fld + "),0) as August,decode(to_chaR(orddt,'yyyymm')," + m9 + ",sum(" + amt_fld + "),0) as Sept,decode(to_chaR(orddt,'yyyymm')," + m10 + ",sum(" + amt_fld + "),0) as Oct,decode(to_chaR(orddt,'yyyymm')," + m11 + ",sum(" + amt_fld + "),0) as Nov,decode(to_chaR(orddt,'yyyymm')," + m12 + ",sum(" + amt_fld + "),0) as Dec ,decode(to_chaR(orddt,'yyyymm')," + m1 + ",sum(" + amt_fld + "),0) as Jan,decode(to_chaR(orddt,'yyyymm')," + m2 + ",sum(" + amt_fld + "),0) as Feb,decode(to_chaR(orddt,'yyyymm')," + m3 + ",sum(" + amt_fld + "),0) as Mar from somas a where " + col2 + " and a.orddt " + DateRange + " and substr(a.type,1,1)='4' group by a.branchcd,substr(a.icode,1,4),to_char(orddt,'yyyymm')";
            SQuery = "Select Y.Iname as Sub_Grp,Y.NAme as Main_Grp,x.Rep,to_char(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar),'99,99,99,999.99') as Totals,to_char(sum(x.April),'99,99,99,999.99') as April,to_char(sum(x.May),'99,99,99,999.99') as May,to_char(sum(x.June),'99,99,99,999.99') as June,to_Char(sum(x.July),'99,99,99,999.99') as July,to_char(sum(x.August),'99,99,99,999.99') as August,to_Char(sum(x.Sept),'99,99,99,999.99') as Sept,to_char(sum(x.oct),'99,99,99,999.99') as Oct,to_Char(sum(x.Nov),'99,99,99,999.99') as Nov,to_char(sum(x.Dec),'99,99,99,999.99') as Dec,to_Char(sum(x.Jan),'99,99,99,999.99') as Jan,to_char(sum(x.Feb),'99,99,99,999.99') as Feb,to_Char(sum(x.Mar),'99,99,99,999.99') as Mar,x.grp,y.type1 as mg_code  from (" + SQuery + ") x left outer join (select b.type1,b.NAme,a.Icode,a.iname from item a,type b where b.id='Y' and substr(a.icode,1,2)=trim(B.type1) and length(Trim(a.icode))=4) y on trim(x.grp)=trim(y.icode) where 1=1 group by y.type1,y.NAme,Y.Iname,x.grp,x.Rep order by x.Grp,x.Rep";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Main Grp,Sub Group Wise Month Wise Order ( Qty / Value ) Analysis ", frm_qstr);

        }
        else if (hffield.Value == "INVWISERPT")
        {

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select A.vCHNUM AS Bill_No,a.vchdate as Bill_Dt,E.Aname as customer,d.Name as Sal_type,c.exc_item,sum(a.iqtyout) as Quantity,sum(a.iamount) as Basic_Value,sum((a.iamount+(a.iqtyout*a.iexc_Addl))) as Asb_Value,sum(a.exc_amt) as Duties,sum(a.iamount+a.exc_amt)  as Taxable_Value,c.st_Rate as Tax_Rate,sum(round((a.iamount+a.exc_amt)*(c.st_rate/100),2))  as Tax_amt,sum(a.iamount+a.exc_amt+round((a.iamount+a.exc_amt)*(c.st_rate/100),2))  as Item_Total,max(c.tcsamt) as TCS_Amt from ivoucher a,item b,sale c,type d,famst e where trim(A.acode)=trim(E.acode) and d.id='V' and a.type=d.type1 and a.branchcd||a.type||a.vchnum||to_char(A.vchdate,'dd/mm/yyyy')=c.branchcd||c.type||c.vchnum||to_char(c.vchdate,'dd/mm/yyyy') and trim(a.icode)=trim(B.icodE) and a.branchcd ='" + frm_mbr + "'  and a.type like '4%' and a.vchdate " + PrdRange + " group by A.vCHNUM,a.vchdate,E.Aname,d.Name,c.exc_item,c.st_Rate order by a.vchdate,a.vchnum ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Invoice Wise Sales : Quantity , Basic , Taxable  ", frm_qstr);

        }
        else if (hffield.Value == "STATEQTY")
        {

            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            string br_Str = "";
            if (col1 == "Y")
            {
                br_Str = "a.branchcd != 'DD'";
            }
            else
            {
                br_Str = "a.branchcd = '" + frm_mbr + "'";
            }
            string m4 = DateTime.Today.Year + "04";
            string m5 = DateTime.Today.Year + "05";
            string m6 = DateTime.Today.Year + "06";
            string m7 = DateTime.Today.Year + "07";
            string m8 = DateTime.Today.Year + "08";
            string m9 = DateTime.Today.Year + "09";
            string m10 = DateTime.Today.Year + "10";
            string m11 = DateTime.Today.Year + "11";
            string m12 = DateTime.Today.Year + "12";
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";

            SQuery = "Select a.branchcd,'Qty' as Rep,substr(a.acode,1,7) as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iqtyout),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iqtyout),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iqtyout),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iqtyout),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iqtyout),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iqtyout),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iqtyout),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iqtyout),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iqtyout),0) as Dec,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iqtyout),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iqtyout),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iqtyout),0) as Mar from ivoucher a where " + br_Str + " and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' group by a.branchcd,substr(a.acode,1,7),to_char(vchdate,'yyyymm')  union all Select a.branchcd,'Value' as Rep,substr(a.acode,1,7) as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iamount),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iamount),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iamount),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iamount),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iamount),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iamount),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iamount),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iamount),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iamount),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iamount),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iamount),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iamount),0) as Mar from ivoucher a where " + br_Str + " and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' group by a.branchcd,substr(a.acode,1,7),to_char(vchdate,'yyyymm')";


            string MY_SIZE = "MTHLYREPP";

            SQuery = "Select Y.Staten as State_NAme,x.Rep,to_char(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar),'99,99,99,999.99') as Totals,to_char(sum(x.April),'99,99,99,999.99') as April,to_char(sum(x.May),'99,99,99,999.99') as May,to_char(sum(x.June),'99,99,99,999.99') as June,to_Char(sum(x.July),'99,99,99,999.99') as July,to_char(sum(x.August),'99,99,99,999.99') as August,to_Char(sum(x.Sept),'99,99,99,999.99') as Sept,to_char(sum(x.oct),'99,99,99,999.99') as Oct,to_Char(sum(x.Nov),'99,99,99,999.99') as Nov,to_char(sum(x.Dec),'99,99,99,999.99') as Dec,to_Char(sum(x.Jan),'99,99,99,999.99') as Jan,to_char(sum(x.Feb),'99,99,99,999.99') as Feb,to_Char(sum(x.Mar),'99,99,99,999.99') as Mar  from (" + SQuery + ") x left outer join famst y on trim(x.grp)=trim(y.acode) group by Y.staten,x.Rep order by y.staten,x.Rep";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("State Wise Month Wise Sales ( Qty / Value ) Analysis ", frm_qstr);

        }
        else if (hffield.Value == "STATEQTYGRP")
        {

            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            string br_Str = "";
            if (col1 == "Y")
            {
                br_Str = "a.branchcd != 'DD'";
            }
            else
            {
                br_Str = "a.branchcd = '" + frm_mbr + "'";
            }
            string m4 = DateTime.Today.Year + "04";
            string m5 = DateTime.Today.Year + "05";
            string m6 = DateTime.Today.Year + "06";
            string m7 = DateTime.Today.Year + "07";
            string m8 = DateTime.Today.Year + "08";
            string m9 = DateTime.Today.Year + "09";
            string m10 = DateTime.Today.Year + "10";
            string m11 = DateTime.Today.Year + "11";
            string m12 = DateTime.Today.Year + "12";
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";

            SQuery = "Select a.branchcd,'Qty' as Rep,trim(a.acode) As acode,substr(a.icode,1,4) as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iqtyout),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iqtyout),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iqtyout),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iqtyout),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iqtyout),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iqtyout),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iqtyout),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iqtyout),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iqtyout),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iqtyout),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iqtyout),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iqtyout),0) as Mar from ivoucher a where " + br_Str + " and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' group by a.branchcd,trim(a.acode),substr(a.icode,1,4),to_char(vchdate,'yyyymm')  union all Select a.branchcd,'Value' as Rep,trim(a.acode) As acode,substr(a.icode,1,4) as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iamount),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iamount),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ", sum(a.iamount),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iamount),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iamount),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iamount),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iamount),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iamount),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iamount),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iamount),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iamount),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iamount),0) as Mar from ivoucher a  where " + br_Str + " and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' group by a.branchcd,trim(a.acode),substr(a.icode,1,4),to_char(vchdate,'yyyymm')";

            SQuery = "Select z.Staten,Y.NAme as Main_Grp,Y.Iname as Sub_Grp,x.Rep,to_char(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar),'99,99,99,999.99') as Totals,to_char(sum(x.April),'99,99,99,999.99') as April,to_char(sum(x.May),'99,99,99,999.99') as May,to_char(sum(x.June),'99,99,99,999.99') as June,to_Char(sum(x.July),'99,99,99,999.99') as July,to_char(sum(x.August),'99,99,99,999.99') as August,to_Char(sum(x.Sept),'99,99,99,999.99') as Sept,to_char(sum(x.oct),'99,99,99,999.99') as Oct,to_Char(sum(x.Nov),'99,99,99,999.99') as Nov,to_char(sum(x.Dec),'99,99,99,999.99') as Dec,to_Char(sum(x.Jan),'99,99,99,999.99') as Jan,to_char(sum(x.Feb),'99,99,99,999.99') as Feb,to_Char(sum(x.Mar),'99,99,99,999.99') as Mar,x.grp,y.type1 as mg_code  from famst z,(" + SQuery + ") x left outer join (select b.type1,b.NAme,a.Icode,a.iname from item a,type b where b.id='Y' and substr(a.icode,1,2)=trim(B.type1) and length(Trim(a.icode))=4) y on trim(x.grp)=trim(y.icode) where trim(Z.acode)=trim(X.acode) group by z.staten,y.type1,y.NAme,Y.Iname,x.grp,x.Rep order by z.staten,x.Grp,x.Rep";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Main Grp,Sub Group Wise Month Wise Sales ( Qty / Value ) Analysis ", frm_qstr);

        }
        else if (hffield.Value == "INVWISERPTTOT")
        {

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select A.vCHNUM AS Bill_No,a.vchdate as Bill_Dt,E.Aname as customer,d.Name as Sal_type,a.exc_item,a.amt_sale as Basic_Value,a.amt_Sale+a.amt_Extexc as Asb_Value,a.amt_Exc as Duties,a.amt_sale+a.amt_exc as Taxable_Value,a.st_Rate as Tax_Rate,a.st_Amt as Tax_amt,  a.tcsamt,a.amt_rea as Frt_amt,a.bill_tot  as Item_Total from sale a,type d,famst e where trim(A.acode)=trim(E.acode) and d.id='V' and a.type=d.type1  and a.branchcd ='" + frm_mbr + "'  and a.type like '4%' and a.vchdate " + PrdRange + " order by a.vchdate,a.vchnum ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Invoice Wise Sales : Quantity , Basic , Taxable , Totals  ", frm_qstr);

        }

        else if (hffield.Value == "MTHSCH")
        {

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select to_char(a.vchdate,'yyyy MONTH') as Month_name,(a.total*a.irate) as sch_Val,0 as Sale_val,to_Char(A.vchdate,'yyyymm') as mthyr from schedule a where a.branchcd='" + frm_mbr + "' and a.type='46' and a.vchdate " + PrdRange + " union all select to_char(a.vchdate,'yyyy MONTH') as Month_name,0 as sch_Val,(a.iamount) as Sale_val,to_Char(A.vchdate,'yyyymm') as mthyr from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type not in ('45','47') and a.vchdate " + PrdRange + "";
            SQuery = "select Month_name as fstr, Month_name,to_Char(sum(sch_Val),'999,99,99,999') as sch_Val,to_Char(sum(sale_Val),'999,99,99,999') as Sale_val,(Case when sum(sch_val)>0 then round((sum(sale_val)/sum(sch_val))*100,2) else 0 end) as Perc_ach,mthyr from (" + SQuery + ") group by Month_name,mthyr order by Mthyr";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("Schedule Vs Sale Achieved", frm_qstr);

        }
        else if (hffield.Value == "YRWISESALE")
        {
            int fld_cnt = 1;
            if (fld_cnt == 1)
            {
                col1 = "round(sum(a.Amt_sale)/10000000,2) ";
            }
            else if (fld_cnt == 2)
            {
                col1 = "round(sum(a.Amt_sale)/10000000,2) ";
            }
            else if (fld_cnt == 3)
            {
                col1 = "round(sum(a.Amt_sale)/10000000,2) ";
            }
            else
            {
                col1 = "0";
            }

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            string xstr = "";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, "Select fmdate,todate from co where substr(code,1,length(trim(code))-4) like '" + frm_cocd + "' and fmdate " + PrdRange + " order by fmdate");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string kyrstr = "between to_date('" + Convert.ToDateTime(dt.Rows[i]["fmdate"]).ToShortDateString().ToString() + "','dd/mm/yyyy') and to_date('" + Convert.ToDateTime(dt.Rows[i]["todate"]).ToShortDateString().ToString() + "','dd/mm/yyyy')";
                    xstr = xstr + "SELECT to_Char(a.vchdate,'MM') as Year_Name,to_Char(a.vchdate,'MONTH') as Month_Name," + col1 + " as tot_bas1," + col1 + " as tot_bas2, " + col1 + " as tot_bas3 from sale a where a.branchcd='" + frm_mbr + "' and a.type!='47' and a.type like '%%' and substr(a.acode,1,2)='16' and a.vchdate " + kyrstr + " group by to_Char(a.vchdate,'MONTH'),to_Char(a.vchdate,'MM') union all ";
                }
            }

            SQuery = xstr + " SELECT '-' as yrstr1,'-' as yrstr,0 as Bas_tot1,0 as Bas_tot2,0 as Bas_tot3 from sale where 1=2 ";
            SQuery = "Select Month_name,sum(tot_bas1) as year_1,sum(tot_bas2) as year_2,sum(tot_bas3) as year_3,null as mthsno,year_name from (" + SQuery + ") group by month_name,year_name union all select null as mthname,0 as y1,0 as y2,0 as y3,mthsno,mthnum from mths ";
            SQuery = "select * from (Select max(Month_name) As Month_name,sum(year_1) as year_1,sum(year_2) as year_2,sum(year_3) as year_3,max(mthsno) as mthsno,year_name as mthsrn from (" + SQuery + ") group by year_name) order by mthsno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Month Wise Year Wise Summary " + PrdRange + "  ", frm_qstr);

        }
        else if (hffield.Value == "PRICESUMM")
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


            if (Tax_GCC == "Y" || OVER_SEAS == "Y") ADDER = 0;
            string m4 = DateTime.Today.Year + "04";
            string m5 = DateTime.Today.Year + "05";
            string m6 = DateTime.Today.Year + "06";
            string m7 = DateTime.Today.Year + "07";
            string m8 = DateTime.Today.Year + "08";
            string m9 = DateTime.Today.Year + "09";
            string m10 = DateTime.Today.Year + "10";
            string m11 = DateTime.Today.Year + "11";
            string m12 = DateTime.Today.Year + "12";
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";

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

            SQuery = "WITH pivot_data AS (SELECT to_Char(a.Vchdate,'mm') as  deptno, trim(a.icode) as Product, max(a.irate)  as sal,trim(a.exc_57f4) as Part_no_inv FROM ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + PrdRange + " and a.icode like '9%' and a.acode " + party_cd + " and a.icode " + part_cd + " group by to_Char(a.Vchdate,'mm'),trim(a.icode),trim(a.exc_57f4) )SELECT * From pivot_data PIVOT ( max(sal) FOR deptno IN  ('04' as April,'05' as May,'06' as June,'07' as July,'08' as August,'09' as Sept,'10' as Oct,'11' as Nov,'12' as Dec,'01' as Jan,'02' as Feb,'03' as Mar))";
            SQuery = "Select b.iname,a.* from (" + SQuery + ")a,item b where trim(A.product)=trim(B.icode) order by b.Iname";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Price Summary " + PrdRange + " ", frm_qstr);

        }
        else if (hffield.Value == "SCHDISPNEW")
        {
            fgen.msg("-", "SMSG", "Choose YES for WithOut SO Del Date and NO for With SO Del Date");
            hffield.Value = "SCHDISPNEWW";

        }
        else if (hffield.Value == "SCHDISPNEWW")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
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


            if (Tax_GCC == "Y" || OVER_SEAS == "Y") ADDER = 0;
            string m4 = DateTime.Today.Year + "04";
            string m5 = DateTime.Today.Year + "05";
            string m6 = DateTime.Today.Year + "06";
            string m7 = DateTime.Today.Year + "07";
            string m8 = DateTime.Today.Year + "08";
            string m9 = DateTime.Today.Year + "09";
            string m10 = DateTime.Today.Year + "10";
            string m11 = DateTime.Today.Year + "11";
            string m12 = DateTime.Today.Year + "12";
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";

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

            if (col1 == "Y")
            {
                string popsql = "Select a.Branchcd as Plant_Cd,to_char(a.vchdate,'Month YYYY') as Sal_mth,B.Aname as Cust_Name,a.Vchnum as Invno,a.vchdate as Invdate,a.Acode,b.zoname as Zone_,B.addr1,b.addr2,b.addr3,c.Icode,c.purpose as Cust_Part_Name,c.Exc_57f4 as Cust_Part_code,'-' as Category,c.iqtyout,c.irate,c.iamount,C.exc_RATE as c_igst_Rate,(C.iamount*round(C.exc_RATE/100,3)) as c_igst_val,C.cess_percent as sgst_Rate,(C.iamount*round(C.cess_percent/100,3)) as sgst_val,a.grno,a.grdate,a.Cmrr_no,a.cmrr_Dt,b.Pname as Cust_rep,a.ruleno,a.chlnum as exp_inv_no,a.chldate as exp_inv_dt ,B.GST_NO,t.name as TYPE_NAME,A.DLV_TERMS from sale a, famst b, ivoucher c,TYPE T  ";
                string mseeksql = "where trim(a.acode)=trim(B.acode) and a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')=c.branchcd||c.type||c.vchnum||to_char(c.vchdate,'dd/mm/yyyy')  AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='V' and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + PrdRange + " and a.acode " + party_cd + " and c.icode " + part_cd + " order by a.vchdate,a.vchnum";
                SQuery = popsql + mseeksql;
            }
            else
            {
                SQuery = "Select a.Branchcd as Plant_Cd,to_char(a.vchdate,'Month YYYY') as Sal_mth,B.Aname as Cust_Name,a.Vchnum as Invno,a.vchdate as Invdate,a.Acode,b.zoname as Zone_,B.addr1,b.addr2,b.addr3,c.Icode,c.purpose as Cust_Part_Name,c.Exc_57f4 as Cust_Part_code,'-' as Category,c.iqtyout,c.irate,c.iamount,C.exc_RATE as c_igst_Rate,(C.iamount*round(C.exc_RATE/100,3)) as c_igst_val,C.cess_percent as sgst_Rate,(C.iamount*round(C.cess_percent/100,3)) as sgst_val,a.grno,a.grdate,a.Cmrr_no,a.cmrr_Dt,b.Pname as Cust_rep,a.ruleno,a.chlnum as exp_inv_no,a.chldate as exp_inv_dt ,B.GST_NO,d.del_date,t.name as TYPE_NAME,A.DLV_TERMS from sale a, famst b, ivoucher c,SOMAS D,TYPE T where trim(a.acode)=trim(B.acode) and a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')=c.branchcd||c.type||c.vchnum||to_char(c.vchdate,'dd/mm/yyyy') and trim(c.type)||trim(c.acode)||trim(c.icode)||trim(c.revis_no)||to_char(c.podate,'dd/mm/yyyy')=trim(d.type)||trim(d.acode)||trim(d.icode)||trim(d.cdrgno)||to_char(d.orddt,'dd/mm/yyyy') AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='V' and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + PrdRange + " and a.acode " + party_cd + " and c.icode " + part_cd + " order by a.vchdate,a.vchnum";
            }


            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Sales Report" + PrdRange + " ", frm_qstr);
        }
        else if (hffield.Value == "DATEWISESALECUST")
        {

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select b.Aname as Customer,a.vchdate as Dated,to_char(sum(a.bill_qty),'99,99,99,999.99') as Quantity,to_char(sum(a.amt_sale),'99,99,99,999.99') as Basic_Value,to_char(sum(a.Bill_tot),'99,99,99,999.99') as Gross_Value,to_char(sum(a.amt_exc),'99,99,99,999.99') as Duties from sale a,famst b where trim(a.acode)=trim(B.acodE) and a.vchdate " + PrdRange + " and a.branchcd ='" + frm_mbr + "'  and a.type<>'47' group by b.aname,a.vchdate order by a.vchdate,b.aname ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Day Wise Sales : Quantity , Basic , Gross ", frm_qstr);

        }
        else if (hffield.Value == "DATEWISESALEITEM")
        {

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select b.Iname as Item,a.vchdate as Dated,to_char(sum(a.iqtyout),'99,99,99,999.99') as Quantity,to_char(sum(a.iamount),'99,99,99,999.99') as Basic_Value,to_char(sum(a.exc_amt),'99,99,99,999.99') as Duties,to_char(sum(a.iamount+a.exc_amt),'99,99,99,999.99') as BasicPlusED,b.cpartno from ivoucher a,item b where trim(a.icode)=trim(B.icodE) and a.vchdate " + PrdRange + " and a.branchcd ='" + frm_mbr + "'  and a.type like '4%' and a.type<>'47' group by b.iname,a.vchdate,b.cpartno order by a.vchdate,b.iname ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Day Wise Sales : Quantity , Basic , Gross ", frm_qstr);

        }
        else if (hffield.Value == "DATEWISESALELINE")
        {

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select b.Maker as Line_name,a.vchdate as Dated,to_char(sum(a.iqtyout),'99,99,99,999.99') as Quantity,to_char(sum(a.iamount),'99,99,99,999.99') as Basic_Value,to_char(sum(a.exc_amt),'99,99,99,999.99') as Duties,to_char(sum(a.iamount+a.exc_amt),'99,99,99,999.99') as BasicPlusED from ivoucher a,item b where trim(a.icode)=trim(B.icodE) and a.vchdate " + PrdRange + " and a.branchcd ='" + frm_mbr + "'  and a.type like '4%' and a.type<>'47' group by b.Maker,a.vchdate order by a.vchdate,b.Maker ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Day Wise Sales : Quantity , Basic , Gross ", frm_qstr);

        }
        else if (hffield.Value == "ITEMINVLINE")
        {
            fgen.msg("-", "SMSG", "Select YES for PRE GST View and NO for GST FMT View");
            hffield.Value = "ITEMINVLINEE";
        }
        else if (hffield.Value == "ITEMINVLINEE")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                SQuery = "select A.vCHNUM AS Bill_No,a.vchdate as Bill_Dt,E.Aname as customer,d.Name as Sal_type,c.exc_item,b.Tarrifno,b.Iname as Item,a.Icode,a.btchno,b.Cpartno as Partno,b.Maker as Prod_line,a.iqtyout as Quantity,a.irate as Basic_Rate,a.iamount as Basic_Value,(a.iamount+(a.iqtyout*a.iexc_Addl)) as Asb_Value,(a.exc_amt) as Duties,(a.iamount+a.exc_amt)  as Taxable_Value,c.st_Rate as Tax_Rate,round((a.iamount+a.exc_amt)*(c.st_rate/100),2)  as Tax_amt,  a.iamount+a.exc_amt+round((a.iamount+a.exc_amt)*(c.st_rate/100),2)  as Item_Total,a.Location,c.tcsamt from ivoucher a,item b,sale c,type d,famst e where trim(A.acode)=trim(E.acode) and d.id='V' and a.type=d.type1 and a.branchcd||a.type||a.vchnum||to_char(A.vchdate,'dd/mm/yyyy')=c.branchcd||c.type||c.vchnum||to_char(c.vchdate,'dd/mm/yyyy') and trim(a.icode)=trim(B.icodE) and a.branchcd ='" + frm_mbr + "'  and a.type like '4%' and a.vchdate " + PrdRange + " order by a.vchdate,a.vchnum,b.iname ";
            }
            else
            {
                SQuery = "select A.vCHNUM AS Bill_No,a.vchdate as Bill_Dt,E.Aname as customer,e.gst_no,d.Name as Sal_type,b.HSCODE,b.Iname as Item,a.Icode,a.btchno,b.Cpartno as Partno,b.Maker as Prod_line,a.iqtyout as Quantity,a.irate as Basic_Rate,a.iamount as Basic_Value,(a.iamount+(a.iqtyout*a.iexc_Addl)) as Asb_Value,(a.exc_amt+a.cess_pu) as Total_GST, a.iamount+a.exc_amt+a.cesS_pu  as Item_Total,a.iopr,a.Location,a.finvno,c.tcsamt,c.cscode,c.full_invno,a.exc_Rate ,a.cess_percent,a.exc_amt ,a.cess_pu  from ivoucher a,item b,sale c,type d,famst e where trim(A.acode)=trim(E.acode) and d.id='V' and a.type=d.type1 and a.branchcd||a.type||a.vchnum||to_char(A.vchdate,'dd/mm/yyyy')=c.branchcd||c.type||c.vchnum||to_char(c.vchdate,'dd/mm/yyyy') and trim(a.icode)=trim(B.icodE) and a.branchcd ='" + frm_mbr + "'  and a.type like '4%' and a.vchdate " + PrdRange + " order by a.vchdate,a.vchnum,b.iname ";
                SQuery = "select a.Bill_No,a.Bill_Dt,a.customer,a.gst_no,b.aname as cons_name,a.Sal_type,a.HSCODE,a.Item,a.Icode,a.finvno as po_no,a.btchno,a.Partno,a.Prod_line,a.Quantity,a.Basic_Rate,a.Basic_Value,a.Asb_Value,a.Total_GST, (Case when trim(A.iopr)='CG' then a.exc_Rate else 0 end) as CGST_RT,(Case when trim(A.iopr)='CG' then a.exc_Amt else 0 end) as CGST_amt,(Case when trim(A.iopr)='CG' then a.cess_percent else 0 end) as SGST_Rate,(Case when trim(A.iopr)='CG' then a.cess_pu else 0 end) as SGST_amt,(Case when trim(A.iopr)='IG' then a.exc_rate else 0 end) as IGST_Rt,(Case when trim(A.iopr)='IG' then a.exc_amt else 0 end) as IGST_amt,a.Item_Total,a.Location,a.tcsamt,a.full_invno from (" + SQuery + ") a left outer join csmst b on trim(A.cscode)=trim(b.acode) order by a.Bill_Dt,a.Bill_no,a.Item ";
            }

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Invoice Wise Sales : Quantity , Basic , Gross ", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "SCHQTYCOMP")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            hf2.Value = col1;
            fgen.msg("-", "SMSG", "Please select YES for ITEM Lvl View and NO for Grp Lvl");
            hffield.Value = "SCHQTYCOMPP";
        }
        else if (hffield.Value == "SCHVALCOMP")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            hf2.Value = col1;
            fgen.msg("-", "SMSG", "Please select YES for ITEM Lvl View and NO for Grp Lvl");
            hffield.Value = "SCHVALCOMPP";
        }
        else if (hffield.Value == "SCHQTYCOMPP")
        {
            col1 = hf2.Value;
            col2 = Request.Cookies["REPLY"].Value.ToString().Trim();
            string br_fld;
            string m4 = DateTime.Today.Year + "04";
            string m5 = DateTime.Today.Year + "05";
            string m6 = DateTime.Today.Year + "06";
            string m7 = DateTime.Today.Year + "07";
            string m8 = DateTime.Today.Year + "08";
            string m9 = DateTime.Today.Year + "09";
            string m10 = DateTime.Today.Year + "10";
            string m11 = DateTime.Today.Year + "11";
            string m12 = DateTime.Today.Year + "12";
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";
            if (col1 == "Y")
            {
                br_fld = "a.branchcd!='DD'";

            }
            else
            {
                br_fld = "a.branchcd='" + frm_mbr + "'";
            }

            string ord = "";
            if (col2 == "Y")
            {
                ord = "item";
            }
            else
            {
                ord = "grp";
            }
            if (col1 == "Y")
            {

                SQuery = "Select trim(b.Name) as Item,substr(a.icode,1,2) as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.total*a.irate),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.total*a.irate),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.total*a.irate),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.total*a.irate),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.total*a.irate),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.total*a.irate),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.total*a.irate),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.total*a.irate),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.total*a.irate),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.total*a.irate),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.total*a.irate),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.total*a.irate),0) as Mar,substr(a.icode,1,2) as ICode from schedule  a ,type b where substr(a.icode,1,2)=b.type1 and " + br_fld + " and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' and a.type!='47' and b.id='Y' and substr(a.icode,1,1)='9' group by substr(a.icode,1,2),trim(b.name),to_char(vchdate,'yyyymm')  ";
                SQuery = "Select Item,grp,to_char(sum(April),'99,99,99,999.99') as April,to_char(sum(May),'99,99,99,999.99') as May,to_char(sum(June),'99,99,99,999.99') as June,to_Char(sum(July),'99,99,99,999.99') as July,to_char(sum(August),'99,99,99,999.99') as August,to_Char(sum(Sept),'99,99,99,999.99') as Sept,to_char(sum(oct),'99,99,99,999.99') as Oct,to_Char(sum(Nov),'99,99,99,999.99') as Nov,to_char(sum(Dec),'99,99,99,999.99') as Dec,to_Char(sum(Jan),'99,99,99,999.99') as Jan,to_char(sum(Feb),'99,99,99,999.99') as Feb,to_Char(sum(Mar),'99,99,99,999.99') as Mar,icode from (" + SQuery + ") group by item,grp,icode order by " + ord + "";
            }
            else
            {
                SQuery = "Select trim(b.Iname)||' '||trim(b.cpartno) as Item,a.acode,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.total*a.irate),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.total*a.irate),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.total*a.irate),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.total*a.irate),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.total*a.irate),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.total*a.irate),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.total*a.irate),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.total*a.irate),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.total*a.irate),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.total*a.irate),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.total*a.irate),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.total*a.irate),0) as Mar,a.icode from schedule a left outer join item b on a.icode=b.icode where " + br_fld + " and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' and a.type!='47' group by a.acode,a.icode,trim(b.Iname)||' '||trim(b.cpartno),to_char(vchdate,'yyyymm')  ";
                SQuery = "Select m.Item,n.aname ,to_char(sum(m.April),'99,99,99,999.99') as April,to_char(sum(m.May),'99,99,99,999.99') as May,to_char(sum(m.June),'99,99,99,999.99') as June,to_Char(sum(m.July),'99,99,99,999.99') as July,to_char(sum(m.August),'99,99,99,999.99') as August,to_Char(sum(m.Sept),'99,99,99,999.99') as Sept,to_char(sum(m.oct),'99,99,99,999.99') as Oct,to_Char(sum(m.Nov),'99,99,99,999.99') as Nov,to_char(sum(m.Dec),'99,99,99,999.99') as Dec,to_Char(sum(m.Jan),'99,99,99,999.99') as Jan,to_char(sum(m.Feb),'99,99,99,999.99') as Feb,to_Char(sum(m.Mar),'99,99,99,999.99') as Mar,icode from (" + SQuery + ")m,famst n where trim(m.acodE)=trim(n.acode) group by m.item,n.aname,m.icode order by " + ord + "";
            }

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            if (col1 == "Y")
            {
                fgen.Fn_open_rptlevel("Consolidated Report", frm_qstr);
            }
            else
            {
                fgen.Fn_open_rptlevel("Plant Wise Report", frm_qstr);
            }

            hffield.Value = "-";
            hf2.Value = "";
        }
        else if (hffield.Value == "SCHVALCOMPP")
        {
            col1 = hf2.Value;
            col2 = Request.Cookies["REPLY"].Value.ToString().Trim();
            string br_fld;
            string m4 = DateTime.Today.Year + "04";
            string m5 = DateTime.Today.Year + "05";
            string m6 = DateTime.Today.Year + "06";
            string m7 = DateTime.Today.Year + "07";
            string m8 = DateTime.Today.Year + "08";
            string m9 = DateTime.Today.Year + "09";
            string m10 = DateTime.Today.Year + "10";
            string m11 = DateTime.Today.Year + "11";
            string m12 = DateTime.Today.Year + "12";
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";
            if (col1 == "Y")
            {
                br_fld = "a.branchcd!='DD'";
                if ((frm_cocd == "KCLG" || frm_cocd == "KHEM") && frm_mbr == "06")
                {
                    br_fld = "a.branchcd in ('02','06')";
                }
            }
            else
            {
                br_fld = "a.branchcd='" + frm_mbr + "'";
            }

            string ord = "";
            if (col2 == "Y")
            {
                ord = "item";
            }
            else
            {
                ord = "grp";
            }
            if (col1 == "Y")
            {

                SQuery = "Select trim(b.Name) as Item,substr(a.icode,1,2) as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.total),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.total),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.total),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.total),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.total),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.total),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.total),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.total),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.total),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.total),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.total),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.total),0) as Mar,substr(a.icode,1,2) as ICode from schedule  a ,type b where substr(a.icode,1,2)=b.type1 and " + br_fld + " and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' and a.type!='47' and b.id='Y' and substr(a.icode,1,1)='9' group by substr(a.icode,1,2),trim(b.name),to_char(vchdate,'yyyymm')  ";
                SQuery = "Select Item,grp,to_char(sum(April),'99,99,99,999.99') as April,to_char(sum(May),'99,99,99,999.99') as May,to_char(sum(June),'99,99,99,999.99') as June,to_Char(sum(July),'99,99,99,999.99') as July,to_char(sum(August),'99,99,99,999.99') as August,to_Char(sum(Sept),'99,99,99,999.99') as Sept,to_char(sum(oct),'99,99,99,999.99') as Oct,to_Char(sum(Nov),'99,99,99,999.99') as Nov,to_char(sum(Dec),'99,99,99,999.99') as Dec,to_Char(sum(Jan),'99,99,99,999.99') as Jan,to_char(sum(Feb),'99,99,99,999.99') as Feb,to_Char(sum(Mar),'99,99,99,999.99') as Mar,icode from (" + SQuery + ") group by item,grp,icode order by " + ord + "";
            }
            else
            {
                SQuery = "Select trim(b.Iname) as Item,a.acode,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.total),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.total),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.total),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.total),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.total),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.total),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.total),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.total),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.total),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.total),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.total),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.total),0) as Mar,a.icode,trim(b.cpartno) as partno,max(a.irate) as max_rate from schedule a left outer join item b on a.icode=b.icode where " + br_fld + " and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' and a.type!='47' group by a.acode,a.icode,trim(b.Iname),trim(b.cpartno),to_char(vchdate,'yyyymm')  ";
                SQuery = "Select m.Item,n.aname ,to_char(sum(m.April),'99,99,99,999.99') as April,to_char(sum(m.May),'99,99,99,999.99') as May,to_char(sum(m.June),'99,99,99,999.99') as June,to_Char(sum(m.July),'99,99,99,999.99') as July,to_char(sum(m.August),'99,99,99,999.99') as August,to_Char(sum(m.Sept),'99,99,99,999.99') as Sept,to_char(sum(m.oct),'99,99,99,999.99') as Oct,to_Char(sum(m.Nov),'99,99,99,999.99') as Nov,to_char(sum(m.Dec),'99,99,99,999.99') as Dec,to_Char(sum(m.Jan),'99,99,99,999.99') as Jan,to_char(sum(m.Feb),'99,99,99,999.99') as Feb,to_Char(sum(m.Mar),'99,99,99,999.99') as Mar,icode,partno,max_rate from (" + SQuery + ")m,famst n where trim(m.acodE)=trim(n.acode) group by m.item,n.aname,m.icode,m.partno,m.max_rate order by " + ord + "";
            }

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            if (col1 == "Y")
            {
                fgen.Fn_open_rptlevel("Consolidated Report", frm_qstr);
            }
            else
            {
                fgen.Fn_open_rptlevel("Plant Wise Report", frm_qstr);
            }

            hffield.Value = "-";
            hf2.Value = "";
        }
        else if (hffield.Value == "ITEMQTY")
        {
            SQuery = "select trim(icode) as fstr,iname as name,trim(icode) from item where length(trim(icode))=4 and substr(icode,1,1)='9' order by iname asc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("-", frm_qstr);
            hffield.Value = "ITEMQTY";

        }
        else if (hffield.Value == "SHEDISP")
        {
            hffield.Value = "SHEDISPP";
            fgen.Fn_open_prddmp1("", frm_qstr);
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            hf2.Value = PrdRange;

        }
        else if (hffield.Value == "SHEDISPP")
        {
            //PrdRange = hf2.Value;
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            string prdrange1 = hf2.Value;
            SQuery = "select trim(B.aname) as Customer,trim(C.iname) as Item,sum(a.total) as Sch_Qty,sum(a.sale) as Sal_Qty,sum(a.total*c.wt_net)/1000 as Sch_wt,sum(a.sale*c.wt_net)/1000 as Sal_wt,sum(a.saleamt) as Saleamt,sum(a.total)-sum(a.sale) as Diff_Qty,c.wt_net,trim(C.cpartno) as Partno,trim(A.acode) as Accode,trim(a.icode) as Itcode from (select acode,icode,total,0 as sale,0 as saleamt from schedule  where branchcd='" + frm_mbr + "' and type='46' and vchdate " + PrdRange + " union all select acode,icode,0 as total,iqtyout as sale,iamount as saleamt from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + PrdRange + " and type not in ('47') and store<>'R')a,famst b, item c where c.iname not like '%SCRAP%' and trim(A.acode)=trim(B.acodE) and trim(A.icode)=trim(c.icode) group by trim(A.acode),trim(a.icode),trim(B.aname),trim(C.iname),trim(C.cpartno),c.wt_net order by trim(B.aname),trim(C.iname)";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Sch vs dispatch " + PrdRange + "", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "CUSTTREND")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            SQuery = "Select B.aname,to_Char(a.vchdate,'YYYY MONTH') as Mths,sum(a.wt_num) as Wt,sum(a.amt_sale) as Basic,sum(a.insp_Amt) as FC_amt,to_Char(a.vchdate,'YYYYMM') as Mthnum,a.acode from sale a, famst b where trim(A.acode)=trim(B.acode) and a.branchcd!='DD' and a.type like '4%' and a.vchdate " + PrdRange + " group by b.aname,to_Char(a.vchdate,'YYYY MONTH'),to_Char(a.vchdate,'YYYYMM'),a.acode order by b.aname,to_Char(a.vchdate,'YYYYMM')";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Customer Wise Wt Report", frm_qstr);
            hffield.Value = "-";

        }
        else if (hffield.Value == "CLOSEDSO")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            string MYVTY = "upper(trim(nvl(a.icat,'N')))= 'Y'";
            string MY_SIZE = "ODRR";
            SQuery = "Select a.refdate as CLosed_On,to_char(a.orddt,'DD/MM/YYYY') as Dated,a.ordno as Ord_No,b.aname as Customer,B.MKTGGRP AS Grp,b.addr3,a.cpartno as Part_No,a.ciname as Part_Name,a.qtyord as Qty_Ord,a.shipmark as Close_rmk,a.thru as PymtTerms,a.Pordno,a.Ent_by,a.icode as Code,decode(a.ent_dt,a.edt_dt,'1','2') as Stat,a.gmt_size as NRemarks,a.type,a.edt_by,a.edt_Dt from somas a, famst b where a.branchcd='" + frm_mbr + "' and substr(A.type,1,1) like '4%' and a.refdate " + PrdRange + "  and  TRIM(A.ACODE)=TRIM(b.acode) and " + MYVTY + " order by a.refdate,a.orddt,a.ordno,a.srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Customer Wise Wt Report", frm_qstr);
            hffield.Value = "-";

        }
        else if (hffield.Value == "ITEMQTYY")
        {
            col1 = Request.Cookies["reply"].Value.ToString();
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");

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


            if (Tax_GCC == "Y" || OVER_SEAS == "Y") ADDER = 0;
            string m4 = DateTime.Today.Year + "04";
            string m5 = DateTime.Today.Year + "05";
            string m6 = DateTime.Today.Year + "06";
            string m7 = DateTime.Today.Year + "07";
            string m8 = DateTime.Today.Year + "08";
            string m9 = DateTime.Today.Year + "09";
            string m10 = DateTime.Today.Year + "10";
            string m11 = DateTime.Today.Year + "11";
            string m12 = DateTime.Today.Year + "12";
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";

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

            if (col1 == "Y")
            {
                SQuery = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Qty' as Rep,b.bssch as grp,a.purpose,a.exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iqtyout),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iqtyout),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iqtyout),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iqtyout),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iqtyout),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iqtyout),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iqtyout),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iqtyout),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iqtyout),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iqtyout),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iqtyout),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iqtyout),0) as Mar,a.acode,a.icode from ivoucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode) where a.branchcd != 'DD' and substr(a.type,1,1)='4' and a.vchdate " + PrdRange + " and trim(a.acode)  " + party_cd + " and trim(a.icode) " + part_cd + " and trim(substr(a.icode,1,4)) like '" + col2 + "%' group by a.acode,b.bssch,a.purpose,a.exc_57f4,a.icode,b.aname,to_char(vchdate,'yyyymm')  union all Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Value' as Rep,b.bssch as grp,a.purpose,a.exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iamount),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iamount),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iamount),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iamount),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iamount),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iamount),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iamount),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iamount),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iamount),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iamount),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iamount),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iamount),0) as Mar,a.acode,a.icode from ivoucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode)  where a.branchcd != 'DD' and substr(a.type,1,1)='4' and a.vchdate " + PrdRange + "  and trim(a.acode) " + party_cd + " and trim(a.icode) " + part_cd + " and trim(substr(a.icode,1,4)) like '" + col2 + "%' group by a.acode,b.bssch,a.icode,a.purpose,a.exc_57f4,b.aname,to_char(vchdate,'yyyymm')";
            }
            else
            {
                SQuery = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Qty' as Rep,b.bssch as grp,a.purpose,a.exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iqtyout),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iqtyout),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iqtyout),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iqtyout),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iqtyout),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iqtyout),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iqtyout),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iqtyout),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iqtyout),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iqtyout),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iqtyout),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iqtyout),0) as Mar,a.acode,a.icode from ivoucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode) where a.branchcd ='" + frm_mbr + "' and substr(a.type,1,1)='4' and a.vchdate " + PrdRange + " and trim(a.acode) " + party_cd + " and trim(a.icode) " + part_cd + " and trim(substr(a.icode,1,4)) like '" + col2 + "%' group by a.acode,b.bssch,a.purpose,a.exc_57f4,a.icode,b.aname,to_char(vchdate,'yyyymm')  union all Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Value' as Rep,b.bssch as grp,a.purpose,a.exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iamount),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iamount),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ", sum(a.iamount),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iamount),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iamount),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iamount),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iamount),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iamount),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iamount),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iamount),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iamount),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iamount),0) as Mar,a.acode,a.icode from ivoucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode)  where a.branchcd = '" + frm_mbr + "' and substr(a.type,1,1)='4' and a.vchdate " + PrdRange + "  and trim(a.acode) " + party_cd + " and trim(a.icode) " + part_cd + " and trim(substr(a.icode,1,4)) like '" + col2 + "%' group by a.acode,b.bssch,a.icode,a.purpose,a.exc_57f4,b.aname,to_char(vchdate,'yyyymm')";
            }

            string mseeksql = "Select x.Rep,upper(x.purpose) as Item,upper(x.exc_57f4) as Partno,(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar)) as Totals,(sum(x.April)) as April,(sum(x.May)) as May,(sum(x.June)) as June,(sum(x.July)) as July,(sum(x.August)) as August,(sum(x.Sept)) as Sept,(sum(x.oct)) as Oct,(sum(x.Nov)) as Nov,(sum(x.Dec)) as Dec,(sum(x.Jan)) as Jan,(sum(x.Feb)) as Feb,(sum(x.Mar)) as Mar,x.icode from (" + SQuery + ") x  group by x.icode,upper(x.purpose),upper(x.exc_57f4),x.rep ";

            SQuery = "Select upper(x.purpose) as Item,x.exc_57f4 as Partno,x.Rep,to_char(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar),'99,99,99,999.99') as Totals,to_char(sum(x.April),'99,99,99,999.99') as April,to_char(sum(x.May),'99,99,99,999.99') as May,to_char(sum(x.June),'99,99,99,999.99') as June,to_Char(sum(x.July),'99,99,99,999.99') as July,to_char(sum(x.August),'99,99,99,999.99') as August,to_Char(sum(x.Sept),'99,99,99,999.99') as Sept,to_char(sum(x.oct),'99,99,99,999.99') as Oct,to_Char(sum(x.Nov),'99,99,99,999.99') as Nov,to_char(sum(x.Dec),'99,99,99,999.99') as Dec,to_Char(sum(x.Jan),'99,99,99,999.99') as Jan,to_char(sum(x.Feb),'99,99,99,999.99') as Feb,to_Char(sum(x.Mar),'99,99,99,999.99') as Mar,x.icode,y.siname from (" + SQuery + ") x,item y where trim(X.icodE)=trim(y.icode) group by upper(x.purpose),x.exc_57f4,x.icode,x.rep,y.siname order by upper(x.purpose)";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Month Wise Sales ( Qty / Value ) Analysis ", frm_qstr);
            hffield.Value = "-";

        }
        else if (hffield.Value == "PARTYQTYVAL")
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
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                SQuery = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ a.branchcd,b.Aname as Account,'Qty' as Rep,b.bssch as grp,a.purpose,a.exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iqtyout),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iqtyout),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iqtyout),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iqtyout),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iqtyout),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iqtyout),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iqtyout),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iqtyout),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iqtyout),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iqtyout),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iqtyout),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iqtyout),0) as Mar,a.acode from ivoucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode) where a.branchcd != 'DD' and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' group by a.branchcd,a.acode,b.bssch,a.purpose,a.exc_57f4,b.aname,to_char(vchdate,'yyyymm')  union all Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ a.branchcd,b.Aname as Account,'Value' as Rep,b.bssch as grp,a.purpose,a.exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iamount),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iamount),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iamount),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iamount),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iamount),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iamount),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iamount),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iamount),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iamount),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iamount),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iamount),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iamount),0) as Mar,a.acode from ivoucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode) where a.branchcd != 'DD' and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' group by a.branchcd,a.acode,b.bssch,a.purpose,a.exc_57f4,b.aname,to_char(vchdate,'yyyymm')";
            }
            else
            {
                SQuery = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Qty' as Rep,b.bssch as grp,a.purpose,a.exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iqtyout),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iqtyout),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iqtyout),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iqtyout),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iqtyout),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iqtyout),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iqtyout),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iqtyout),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iqtyout),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iqtyout),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iqtyout),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iqtyout),0) as Mar,a.acode from ivoucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode) where a.branchcd = '" + frm_mbr + "' and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' group by a.acode,b.bssch,a.purpose,a.exc_57f4,b.aname,to_char(vchdate,'yyyymm')  union all Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ b.Aname as Account,'Value' as Rep,b.bssch as grp,a.purpose,a.exc_57f4,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iamount),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iamount),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ", sum(a.iamount),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iamount),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iamount),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iamount),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iamount),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iamount),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iamount),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iamount),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iamount),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iamount),0) as Mar,a.acode from ivoucher a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode) where a.branchcd = '" + frm_mbr + "' and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' group by a.acode,b.bssch,a.purpose,a.exc_57f4,b.aname,to_char(vchdate,'yyyymm')";
            }

            SQuery = "Select x.Account,x.Rep,y.Name,upper(x.purpose) as Item,upper(x.exc_57f4) as Partno,to_char(sum(x.April)+sum(x.May)+sum(x.June)+sum(x.July)+sum(x.August)+sum(x.Sept)+sum(x.Oct)+sum(x.Nov)+sum(x.Dec)+sum(x.Jan)+sum(x.Feb)+sum(x.Mar),'99,99,99,999.99') as Totals,to_char(sum(x.April),'99,99,99,999.99') as April,to_char(sum(x.May),'99,99,99,999.99') as May,to_char(sum(x.June),'99,99,99,999.99') as June,to_Char(sum(x.July),'99,99,99,999.99') as July,to_char(sum(x.August),'99,99,99,999.99') as August,to_Char(sum(x.Sept),'99,99,99,999.99') as Sept,to_char(sum(x.oct),'99,99,99,999.99') as Oct,to_Char(sum(x.Nov),'99,99,99,999.99') as Nov,to_char(sum(x.Dec),'99,99,99,999.99') as Dec,to_Char(sum(x.Jan),'99,99,99,999.99') as Jan,to_char(sum(x.Feb),'99,99,99,999.99') as Feb,to_Char(sum(x.Mar),'99,99,99,999.99') as Mar,x.Acode from (" + SQuery + ") x left outer join (select type1,name from typegrp where id='A') y on trim(x.grp)=trim(y.type1) group by x.account,x.acode,upper(x.purpose),upper(x.exc_57f4),x.rep,y.name order by x.Account";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Month Wise Sales ( Qty / Value ) Analysis ", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "DISTSALE")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select b.District,to_char(sum(a.iqtyout),'999,99,99,999') as Quantity,to_char(sum(a.iamount),'999,99,99,999') as Amount,to_char(sum(a.IQTY_CHLWT),'99,99,99,999') as TWeight from ivoucher a, famst b where a.branchcd!='DD' and a.vchdate " + PrdRange + " and substr(a.type,1,1)='4' and trim(a.acode)=trim(b.acode) group by b.District order by sum(a.iamount) desc,b.District ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("District Wise Sales " + PrdRange + "", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "SEARCHMTH")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            string xmth = Convert.ToDateTime(fromdt).Month.ToString();
            if (xmth.Length == 1)
            {
                xmth = "0" + xmth;
            }
            string Prt_RAV = "Y";
            if (Prt_RAV == "Y")
            {
                SQuery = "select x.acode,x.sch,nvl(y.desp,0) as Desp from(select a.acode,a.icode,sum(a.budgetcost) as sch from budgmst a where a.branchcd='" + frm_mbr + "' and a.type='46' and to_char(a.dlv_Date,'yyyymm')='" + Convert.ToDateTime(fromdt).Year.ToString().Trim() + xmth.Trim() + "' group by a.acode,a.icode) x left outer join (select b.acode,b.icode,sum(b.iqtyout) as Desp from ivoucher b where b.store='Y' and b.branchcd='" + frm_mbr + "' and b.type like ('4%') and to_char(b.vchdate,'yyyymm')='" + Convert.ToDateTime(fromdt).Year.ToString().Trim() + xmth.Trim() + "' group by b.acode,b.icode ) y on trim(x.acode)||trim(x.icode)=trim(y.acode)||trim(y.icode)";
            }

            else
            {
                SQuery = "select x.acode,x.sch,nvl(y.desp,0) as Desp from(select a.acode,a.icode,sum(a.total) as sch from schedule a where a.branchcd='" + frm_mbr + "' and a.type='46' and to_char(a.vchdate,'yyyymm')='" + Convert.ToDateTime(fromdt).Year.ToString().Trim() + xmth.Trim() + " ' group by a.acode,a.icode) x left outer join (select b.acode,b.icode,sum(b.iqtyout) as Desp from ivoucher b where b.store='Y' and b.branchcd='" + frm_mbr + "' and b.type like ('4%') and to_char(b.vchdate,'yyyymm')='" + Convert.ToDateTime(fromdt).Year.ToString().Trim() + xmth.Trim() + "' group by b.acode,b.icode ) y on trim(x.acode)||trim(x.icode)=trim(y.acode)||trim(y.icode)";
            }


            SQuery = "Select n.acode as fstr, M.aname as Customer,n.acode as Code,sum(n.sch) as Schedule,sum(n.Desp) as Despatch,sum(n.sch)-sum(n.Desp) as Bal_Qty,decode(sum(n.sch),0,'N/A',round(sum(n.desp)/sum(n.sch)*100,2)) as Desp_Percent from (" + SQuery + ") n,famst m where trim(m.acode)=trim(n.acode) group by m.aname,n.acode order by m.aname";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("Customers With Schedules During the Selected Period", frm_qstr);
            hffield.Value = "SEARCHMTH";
        }
        else if (hffield.Value == "GROSSRPT")
        {
            string br_fld = "";
            string m4 = DateTime.Today.Year + "04";
            string m5 = DateTime.Today.Year + "05";
            string m6 = DateTime.Today.Year + "06";
            string m7 = DateTime.Today.Year + "07";
            string m8 = DateTime.Today.Year + "08";
            string m9 = DateTime.Today.Year + "09";
            string m10 = DateTime.Today.Year + "10";
            string m11 = DateTime.Today.Year + "11";
            string m12 = DateTime.Today.Year + "12";
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";
            string amt_fld = "(a.bill_tot-nvl(a.st_amt,0)-nvl(a.sta_amt,0)-nvl(a.amt_Exc,0)-nvl(a.amt_job,0)-nvl(a.rvalue,0))";
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            if (col1 == "Y")
            {
                br_fld = "a.branchcd!='DD'";
            }
            else
            {
                br_fld = "a.branchcd='" + frm_mbr + "'";
            }
            SQuery = "Select decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(" + amt_fld + "),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(" + amt_fld + "),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(" + amt_fld + "),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(" + amt_fld + "),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(" + amt_fld + "),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(" + amt_fld + "),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(" + amt_fld + "),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(" + amt_fld + "),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(" + amt_fld + "),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(" + amt_fld + "),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(" + amt_fld + "),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(" + amt_fld + "),0) as Mar from sale a left outer join famst b on  TRIM(A.ACODE)=TRIM(b.acode) where " + br_fld + " and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' group by maddr1 to_char(vchdate,'yyyymm')  ";

            string MY_SIZE = "MTHLYREPP";

            SQuery = "Select x.Grp_by,to_char(sum(x.April+x.may+x.June+x.July+x.August+x.Sept+x.oct+x.nov+x.dec+x.Jan+x.feb+x.mar),'99,99,99,999') as Totals,to_char(sum(x.April),'99,99,99,999') as April,to_char(sum(x.May),'99,99,99,999') as May,to_char(sum(x.June),'99,99,99,999') as June,to_Char(sum(x.July),'99,99,99,999') as July,to_char(sum(x.August),'99,99,99,999') as August,to_Char(sum(x.Sept),'99,99,99,999') as Sept,to_char(sum(x.oct),'99,99,99,999') as Oct,to_Char(sum(x.Nov),'99,99,99,999') as Nov,to_char(sum(x.Dec),'99,99,99,999') as Dec,to_Char(sum(x.Jan),'99,99,99,999') as Jan,to_char(sum(x.Feb),'99,99,99,999') as Feb,to_Char(sum(x.Mar),'99,99,99,999') as Mar from (" + SQuery + ") x group by x.grp_by order by x.grp_by";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("District Wise Sales " + PrdRange + "", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "ZONEDET")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select b.addr3 as Zone,sum(a.amt_Sale) as Basic,sum(a.bill_tot) as Gross from sale a, famst b where a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,1)='4' and trim(a.acode)=trim(b.acode) group by b.addr3";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Zone Wise Sales" + PrdRange + "", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "SALESGRPRPT")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select b.mktggrp as Sale_Grp,sum(a.amt_Sale) as Basic,sum(a.bill_tot) as Gross from sale a, famst b where a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,1)='4' and trim(a.acode)=trim(b.acode) group by b.mktggrp order by b.mktggrp desc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Group Wise Sales" + PrdRange + "", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "CONTDET")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select b.continent as Continent,to_chaR(sum(a.iqtyout*a.irate),'999,99,99,999.99') as value from ivoucher a, famst b where a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,1)='4' and a.type!='47' and trim(a.acode)=trim(b.acode) group by b.continent";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Zone Wise Sales" + PrdRange + "", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "STATEWISESALE")
        {

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            if (frm_cocd == "ARCF")
            {
                SQuery = "select b.STATEN as State,to_chaR(sum(a.Dramt),'999,99,99,999.99') as value from voucher a, famst b where a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,1)='4' and a.type!='47' and trim(a.acode)=trim(b.acode) and substr(a.acode,1,2)='16' group by b.STATEN";
            }
            else
            {
                if (frm_cocd == "BUPL")
                {
                    SQuery = "select Zone,sum(svalue) as svalue,sum(rtvalue) as rtvalue,sum(svalue)-sum(rtvalue) as Netvalue from (select b.STATEN as Zone,sum(a.bill_tot) as svalue,0 as rtvalue from sale a, famst b where a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,1)='4' and a.type in ('40','46') and trim(a.acode)=trim(b.acode) group by b.STATEN union all select b.STATEN as Zone,0 as svalue,sum(a.iqtyin*a.irate) as rtvalue from ivoucher a, famst b where a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,2)='04' and trim(a.acode)=trim(b.acode) and trim(a.Acode) in (select distinct trim(acode) from sale where type in ('40','46')) group by b.STATEN) group by zone";
                }
                else
                {
                    SQuery = "select b.STATEN as Zone,to_chaR(sum(a.iqtyout*a.irate),'999,99,99,999.99') as value from ivoucher a, famst b where a.branchcd='" + frm_mbr + "' and a.vchdate " + PrdRange + " and substr(a.type,1,1)='4' and a.type!='47' and trim(a.acode)=trim(b.acode) group by b.STATEN";
                }
            }

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("State Wise Sales" + PrdRange + "", frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "HSNWISESALE")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select Name as HS_NAme,HSCODE,to_Char(qty_total,'999,99,99,999') as Qty_Total,to_Char(BAsic_value,'999,99,99,999') as Basic_Val from (Select c.NAme,b.HSCODE,sum(a.iqtyout) as qty_total,sum(a.iamount) As BAsic_value from ivoucher a,item b,typegrp c where trim(a.icode)=trim(B.icode) and trim(b.hscode)=trim(c.acref) and c.id='T1' and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + PrdRange + " group by c.NAme,b.HSCODE) order by BAsic_value desc ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("HS Code wise Report " + PrdRange + "", frm_qstr);
            hffield.Value = "-";
        }


        else if (hffield.Value == "ITEMMONTHC")
        {
            fgen.msg("-", "SMSG", "Please select YES for Item Lvl View and NO for Grp Lvl");
            hffield.Value = "ITEMMONTHG";
        }
        else if (hffield.Value == "ITEMMONTHG")
        {
            col1 = Request.Cookies["reply"].Value.ToString();
            col2 = hf2.Value;
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            string br_fld = "";
            if (col2 == "Y")
            {

                br_fld = "a.branchcd!='DD'";
            }
            else
            {
                br_fld = "a.branchcd='" + frm_mbr + "'";
            }

            string m4 = DateTime.Today.Year + "04";
            string m5 = DateTime.Today.Year + "05";
            string m6 = DateTime.Today.Year + "06";
            string m7 = DateTime.Today.Year + "07";
            string m8 = DateTime.Today.Year + "08";
            string m9 = DateTime.Today.Year + "09";
            string m10 = DateTime.Today.Year + "10";
            string m11 = DateTime.Today.Year + "11";
            string m12 = DateTime.Today.Year + "12";
            string m1 = DateTime.Today.Year + ADDER + "01";
            string m2 = DateTime.Today.Year + ADDER + "02";
            string m3 = DateTime.Today.Year + ADDER + "03";

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            SQuery = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Iname)||' '||trim(b.cpartno) as Item,' ' as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iamount),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iamount),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iamount),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iamount),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iamount),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iamount),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iamount),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iamount),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iamount),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iamount),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iamount),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iamount),0) as Mar,a.icode from ivoucher a left outer join item b on a.icode=b.icode where " + br_fld + " and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' and a.type!='47' group by a.icode,trim(b.Iname)||' '||trim(b.cpartno),to_char(vchdate,'yyyymm')  ";
            if (col1 == "N")
            {
                SQuery = "Select /*+ INDEX_DESC(ivoucher ind_IVCH_DATE) */ trim(b.Name) as Item,substr(a.icode,1,2) as grp,decode(to_chaR(vchdate,'yyyymm')," + m4 + ",sum(a.iamount),0) as April,decode(to_chaR(vchdate,'yyyymm')," + m5 + ",sum(a.iamount),0) as May,decode(to_chaR(vchdate,'yyyymm')," + m6 + ",sum(a.iamount),0) as June,decode(to_chaR(vchdate,'yyyymm')," + m7 + ",sum(a.iamount),0) as July,decode(to_chaR(vchdate,'yyyymm')," + m8 + ",sum(a.iamount),0) as August,decode(to_chaR(vchdate,'yyyymm')," + m9 + ",sum(a.iamount),0) as Sept,decode(to_chaR(vchdate,'yyyymm')," + m10 + ",sum(a.iamount),0) as Oct,decode(to_chaR(vchdate,'yyyymm')," + m11 + ",sum(a.iamount),0) as Nov,decode(to_chaR(vchdate,'yyyymm')," + m12 + ",sum(a.iamount),0) as Dec ,decode(to_chaR(vchdate,'yyyymm')," + m1 + ",sum(a.iamount),0) as Jan,decode(to_chaR(vchdate,'yyyymm')," + m2 + ",sum(a.iamount),0) as Feb,decode(to_chaR(vchdate,'yyyymm')," + m3 + ",sum(a.iamount),0) as Mar,substr(a.icode,1,2) as ICode from ivoucher a ,type b where substr(a.icode,1,2)=b.type1 and " + br_fld + " and a.vchdate " + DateRange + " and substr(a.type,1,1)='4' and a.type!='47' and b.id='Y' and substr(a.icode,1,1)='9' group by substr(a.icode,1,2),trim(b.name),to_char(vchdate,'yyyymm')  ";
            }
            SQuery = "Select Item,grp,to_char(sum(April),'99,99,99,999.99') as April,to_char(sum(May),'99,99,99,999.99') as May,to_char(sum(June),'99,99,99,999.99') as June,to_Char(sum(July),'99,99,99,999.99') as July,to_char(sum(August),'99,99,99,999.99') as August,to_Char(sum(Sept),'99,99,99,999.99') as Sept,to_char(sum(oct),'99,99,99,999.99') as Oct,to_Char(sum(Nov),'99,99,99,999.99') as Nov,to_char(sum(Dec),'99,99,99,999.99') as Dec,to_Char(sum(Jan),'99,99,99,999.99') as Jan,to_char(sum(Feb),'99,99,99,999.99') as Feb,to_Char(sum(Mar),'99,99,99,999.99') as Mar,icode from (" + SQuery + ") group by item,grp,icode order by item"; //+ IIf(Msgresult = 1, "item", "grp")
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            if (col1 == "Y")
            {
                fgen.Fn_open_rptlevel("Consolidated Report", frm_qstr);
            }
            else
            {
                fgen.Fn_open_rptlevel("Plant Wise Report", frm_qstr);
            }

            hffield.Value = "";
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
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("+amp", "");
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




    protected void btnGet_ServerClick(object sender, EventArgs e)
    {
        //Get the button that raised the event
        Button btn = (Button)sender;

        //Get the row that contains this button
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
    }

    protected void btnJobOrdStat_ServerClick(object sender, EventArgs e)
    {

    }

    protected void btnPorpr_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "ORPR";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnTatDays_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "TAT";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnChallans_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "CHALLAN";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnStoreStock_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "STOCK";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnLatehrs_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "LATEHR";
        fgen.Fn_open_PartyItemDateRangeBox("-", frm_qstr);
    }
    protected void rep1_ServerClick(object sender, EventArgs e)
    {

    }
    protected void BtnItemSold_ServerClick(object sender, EventArgs e)
    {

    }
    protected void BtnItemSoldMML_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "MML";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnVendorsIssuePriceList_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "VIPL";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnPurchaseOrder_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "PORI";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnPurReq_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "PR";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnWithotBOMCreation_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "BOM";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnInv_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "INVARD";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnInsp_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "INSP";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnItemInsp_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "INSPITEM";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnPPMData_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "PPM";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnStockStatus_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "STOCKSTATUS";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnCusPPM_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "CUSTPPM";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnSaleppm_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SALESPPM";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnCustOrd_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "CUSTORD";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnCustComp_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "CUSTORD";

    }
    protected void BtnShip_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "CUSTSHIP";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnXYPr_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SALEXY";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void BtnDebt_ServerClick(object sender, EventArgs e)
    {

    }
    protected void BtnCredit_ServerClick(object sender, EventArgs e)
    {

    }
    protected void BtnInvPaid_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "INVPAID";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnPurInv_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "PURINV";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnCredOut_ServerClick(object sender, EventArgs e)
    {

    }
    protected void BtnFundCol_ServerClick(object sender, EventArgs e)
    {

    }
    protected void BtnPurNtIss_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "PURNOTISSUE";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BMrrPen_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "MRR";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnDocVol_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "DOCVOL";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }
    protected void BtnUserlist_ServerClick(object sender, EventArgs e)
    {
        SQuery = "SELECT b.Name,a.username,a.deptt,decode(trim(a.ulevel),'0','0:TOP LEVEL','1','Administrator','2','Department Head','2.5','2:View Rights','Operator') Rights,a.Can_ADD,a.Can_edit,a.Can_del,a.CAN_MST,a.CAN_PY,a.CAN_CON,a.Can_Apprv,a.CAN_CHPYV,a.Can_Adm,a.allowbr as Br_allowed,a.mdeptt as Multi_deptt,a.branchcd,a.userid,a.ent_by,a.ent_Dt,a.edt_by,a.edt_Dt,a.close_by,a.close_dt  FROM evas a, type b where a.branchcd=b.type1 and b.id='B' and a.branchcd!='DD' and length(Trim(nvl(a.close_by,'-')))<=2 and a.level3pw!='CLOSEDU' order by a.username";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("User Statistics in ERP ", frm_qstr);
        hffield.Value = "-";
    }

    protected void BtnCredit1_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "CREDIT";
        fgen.Fn_open_prddmp1("Select Period", frm_qstr);
    }

    protected void BtnGrossAmt_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "ITEMQTY";
        fgen.Fn_open_PartyItemDateRangeBox("-", frm_qstr);
    }
    protected void BtnUnBillChl_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "UNBILLCHL";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnDisSale_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "DISTSALE";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnState_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "STATEWISESALE";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnHSNSale_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "HSNWISESALE";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnItemMonthDesp_ServerClick(object sender, EventArgs e)
    {

        fgen.msg("-", "SMSG", "Do You Want Consolidated Report?");
        hffield.Value = "ITEMMONTHC";
        hf2.Value = Request.Cookies["REPLY"].Value.ToString().Trim();

    }
    protected void BtnCustDesp_ServerClick(object sender, EventArgs e)
    {

        SQuery = "select distinct a.acode as fstr, nvl(b.aname,'-') as Name,b.addr1 as Address,b.addr2 as City from sale a ,famst b where  a.acode=b.acode and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " order by nvl(b.aname,'-') ";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_sseek("Select Parties", frm_qstr);
        hffield.Value = "PARTYWISEQTY";

    }
    protected void BtnQtyComp_ServerClick(object sender, EventArgs e)
    {
        SQuery = "select distinct a.acode as fstr, nvl(b.aname,'-') as Name,b.addr1 as Address,b.addr2 as City from sale a ,famst b where  a.acode=b.acode and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " order by nvl(b.aname,'-') ";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_sseek("Select Parties", frm_qstr);
        hffield.Value = "QUACOMP";
    }

    protected void BtnPartyItem_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "PARTYQTYVAL";
        fgen.msg("-", "SMSG", "Do You Want Consolidated Report?");
    }
    protected void BtnZoneDet_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "ZONEDET";
        fgen.Fn_open_prddmp1("", frm_qstr);
    }
    protected void BtnCont_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "CONTDET";
        fgen.Fn_open_prddmp1("", frm_qstr);
    }
    protected void BtnSalesGrpRpt_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SALESGRPRPT";
        fgen.Fn_open_prddmp1("", frm_qstr);
    }
    protected void BtnGrossRpt_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "GROSSRPT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("", frm_qstr);
    }
    protected void BtnSerchMth_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SEARCHMTH";
        fgen.Fn_open_prddmp1("", frm_qstr);
    }
    protected void BtnCusTren_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "CUSTTREND";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnShedDisp_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SHEDISP";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnClosedSO_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "CLOSEDSO";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }

    protected void BtnQtyComp2_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SCHQTYCOMP";
        fgen.msg("-", "SMSG", "Do You Want Consolidated Report");
    }
    protected void BtnSchValComp_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SCHVALCOMP";
        fgen.msg("-", "SMSG", "Do You Want Consolidated Report");
    }
    protected void BtnLastvsCurr_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "LASTVSCURR";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnSchVal_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SCHVL";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnDayWiseSale_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "DATEWISESALE";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnCustWiseSale_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "DATEWISESALECUST";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnDateWiseItem_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "DATEWISESALEITEM";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnDateWiseLine_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "DATEWISESALELINE";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnItemInv_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "ITEMINVLINE";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnInvWiseRpt_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "INVWISERPT";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnInvWiseRptTot_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "INVWISERPTTOT";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnPriceSumm_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "PRICESUMM";
        fgen.Fn_open_PartyItemDateRangeBox("", frm_qstr);
    }
    protected void BtnSchvsDispNew_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SCHDISPNEW";
        fgen.Fn_open_PartyItemDateRangeBox("", frm_qstr);
    }
    protected void BtnTarrifChk_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "TARIFFCHECK";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void BtnMainGrpSale_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "MAINGRPSALE";
        fgen.msg("-", "SMSG", "Do You want Consolidated Report?");
    }
    protected void BtnOrdValue_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "MAINGRPORD";
        fgen.msg("-", "SMSG", "Do You want Consolidated Report?");
    }
    protected void BtnStateQty_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "STATEQTY";
        fgen.msg("-", "SMSG", "Do You want Consolidated Report?");
    }
    protected void BtnStateQtyGrp_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "STATEQTYGRP";
        fgen.msg("-", "SMSG", "Do You want Consolidated Report?");
    }
    protected void BtnYrWiseSale_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "YRWISESALE";
        fgen.Fn_open_prddmp1("", frm_qstr);
    }
    protected void BtnMthSchedule_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "MTHSCH";
        fgen.Fn_open_prddmp1("", frm_qstr);
    }
    protected void BtnComVsTax_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "COMVSTAX";
        fgen.Fn_open_PartyItemDateRangeBox("", frm_qstr);
    }
    protected void BtnSummInvDbt_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "INVDEBT";
        fgen.Fn_open_PartyItemDateRangeBox("", frm_qstr);
    }
    protected void BtnEwayBill_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "EWAY";
        fgen.Fn_open_prddmp1("", frm_qstr);
    }
    protected void BtnMastWT_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "MASTWT";
        fgen.Fn_open_prddmp1("", frm_qstr);
    }
}