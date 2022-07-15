using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class frmdrawIss : System.Web.UI.Page
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
    string frm_tabname, frm_tabname1, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, wSeriesControl = "";
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

            wSeriesControl = "Y";
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
        lblheader.Text = "Drawing Issue Entry";
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
        //btnCamera.Disabled = true;
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
        frm_tabname = "OM_DRWG_MAKE";
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
                SQuery = "SELECT distinct icode as fstr,iname as iname,icode as code FROM item where branchcd='" + frm_mbr + "' ORDER BY iname asc ";
                col1 = "'-'"; col2 = "'-'";
                if (sg1.Rows.Count > 1)
                {
                    col1 = ""; col2 = "";
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        if (col2.Length > 0) col2 = col2 + "," + "'" + gr.Cells[3].Text.Trim() + "'";
                        else col2 = "'" + gr.Cells[3].Text.Trim() + "'";
                    }
                }
                SQuery = "SELECT A.BRANCHCD||A.TYPE||tRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS FSTR,A.VCHNUM AS DRAWING_eNTNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS DRAW_ENTDT,A.DTYPE,A.DNO as Part_No,A.RNO as Revision_No,A.TNO,A.ENT_BY,TO_CHAR(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT,A.BRANCHCD,(case when a.type='DE' THEN 'From Drawing' WHEN A.TYPE='PI' then 'From Pattern Inspection' else 'From First PC Casting' end) as file_from FROM WB_DRAWREC A WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('DE','PI','CI') AND TRIM(BRANCHCD)||TRIM(TYPE)||TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY') NOT IN (SELECT NVL(TRIM(ISSUETIME),'-') FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND TRIM(COL4)='" + TxtUserCode.Text.Trim() + "' and round(Issueenddt-sysdate)>1 ) AND A.VCHNUM not IN (" + col2 + ") ORDER BY A.VCHNUM";
                SQuery = "SELECT A.BRANCHCD||A.TYPE||tRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(F.MSGTXT) AS FSTR,A.VCHNUM AS ENT_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENT_DT,B.ANAME AS CUSTOMER,C.INAME AS PART_NAME,A.COL1 AS ECNO,f.terminal as design_type,a.t9 as drawing_stage,A.DNO as Part_No,a.acode,F.MSGTXT AS FILENAME FROM WB_DRAWREC A,FAMST B,ITEM C,ATCHVCH F WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD||A.TYPE||tRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=F.BRANCHCD||F.TYPE||tRIM(F.VCHNUM)||TO_CHAR(F.VCHDATE,'DD/MM/YYYY') AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in ('DE','PI','CI') and upper(trim(f.MSGFROM))='ACTIVATE' ORDER BY A.VCHNUM DESC,F.MSGTXT";
                if (txtreqno.Text.Length > 2)
                {
                    SQuery = "SELECT A.BRANCHCD||A.TYPE||tRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(F.MSGTXT) AS FSTR,A.VCHNUM AS ENT_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENT_DT,B.ANAME AS CUSTOMER,C.INAME AS PART_NAME,A.COL1 AS ECNO,f.terminal as design_type,a.t9 as drawing_stage,A.DNO as Part_No,a.acode,F.MSGTXT AS FILENAME FROM WB_DRAWREC A,FAMST B,ITEM C,ATCHVCH F WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND A.BRANCHCD||A.TYPE||tRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=F.BRANCHCD||F.TYPE||tRIM(F.VCHNUM)||TO_CHAR(F.VCHDATE,'DD/MM/YYYY') AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in ('DE','PI','CI') and upper(trim(f.MSGFROM))='ACTIVATE' AND TRIM(A.vchnum)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||trim(a.acode)||Trim(A.icode)||trim(F.MSGTXT) in (SELECT distinct TRIM(COL8)||trim(col9)||trim(AcodE)||trim(icode)||trim(dno) as fstr from WB_DRAW_REQ where branchcd='" + frm_mbr + "' and type='DR' AND TRIM(VCHNUM)='" + txtreqno.Text.Trim() + "' AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + txtreqdt.Text.Trim() + "' ) ORDER BY A.VCHNUM DESC,F.MSGTXT";
                    if (wSeriesControl == "Y")
                        SQuery = "SELECT A.BRANCHCD||A.TYPE||tRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(F.MSGTXT) AS FSTR,A.VCHNUM AS ENT_NO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENT_DT,B.NAME AS CUSTOMER,C.NAME AS PART_NAME,A.COL1 AS ECNO,f.terminal as design_type,a.t9 as drawing_stage,A.DNO as Part_No,a.acode,F.MSGTXT AS FILENAME FROM WB_DRAWREC A,TYPEGRP B,TYPEGRP C,ATCHVCH F WHERE TRIM(A.ACODE)=TRIM(b.TYPE1) AND B.ID='C1' AND TRIM(A.ICODE)=TRIM(C.TYPE1) AND C.ID='P1' AND A.BRANCHCD||A.TYPE||tRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=F.BRANCHCD||F.TYPE||tRIM(F.VCHNUM)||TO_CHAR(F.VCHDATE,'DD/MM/YYYY') AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in ('DE','PI','CI') and upper(trim(f.MSGFROM))='ACTIVATE' AND TRIM(A.vchnum)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||trim(a.acode)||Trim(A.icode)||trim(F.MSGTXT) in (SELECT distinct TRIM(COL8)||trim(col9)||trim(AcodE)||trim(icode)||trim(dno) as fstr from WB_DRAW_REQ where branchcd='" + frm_mbr + "' and type='DR' AND TRIM(VCHNUM)='" + txtreqno.Text.Trim() + "' AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + txtreqdt.Text.Trim() + "' ) ORDER BY A.VCHNUM DESC,F.MSGTXT";
                }
                break;
            case "New":
            case "List":
            case "Edit*":
            case "Del*":
            case "USR":
                SQuery = "SELECT USERID AS fstr,username as name,userid as code FROM EVAS where ulevel!=0 order by username";
                break;
            case "REQ":
                SQuery = "SELECT distinct a.vchnum||'-'||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as req_no,to_char(a.vchdate,'dd/mm/yyyy') as req_dt,a.ent_by,a.ent_dt,TO_CHAR(A.VCHDATE,'YYYYMMDD') AS VDD from WB_DRAW_REQ a WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='DR' AND TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY') NOT IN (SELECT distinct TRIM(INVNO)||TO_CHAR(INVDATE,'DD/MM/YYYY') AS FSTR FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ) ORDER BY VDD DESC,A.VCHNUM DESC ";
                break;
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
                    SQuery = "select distinct VCHNUM||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR,to_char(vchdate,'dd/mm/yyyy') as vchdate,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt,vchnum,type from " + frm_tabname + " where  type='IV' and branchcd='" + frm_mbr + "' and vchnum<>'000000' and vchdate " + DateRange + " order by vchnum desc";
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

            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vchnum) AS vchnum FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and substr(type,1,2)='" + frm_vty + "' and vchdate " + DateRange + " ORDER BY VCHDATE DESC  ", 6, "vchnum");
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtentdt.Text = DateTime.Now.ToString("yyyy-MM-dd");
            txtentby.Text = frm_uname;
            //TxtDate.Value = todt;

            disablectrl();
            fgen.EnableForm(this.Controls);
            txtvchnum.Focus();
            ///
            btnuser.Enabled = true;



            //ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "disableFrame('div2');", true);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    //------------------------------------------------------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        set_Val();

        if (chk_rights == "Y")
        {
            hffield.Value = "Edit";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Complaint No Edit", frm_qstr);
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

        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(upper(vchnum))='" + txtvchnum.Text + "' AND VCHDATE " + DateRange + " order by vchdate desc", "vchnum");

        if (col1 != "0" && edmode.Value != "Y")
        {
            fgen.msg("-", "AMSG", "Entry no already Exist!!");
            return;
        }

        if (txtvchnum.Text.Length < 2)
        {
            fgen.msg("-", "AMSG", "Please Enter Entry No!!");
            txtvchnum.Focus();
            return;
        }

        if (TxtUserCode.Text == "" || TxtUserCode.Text == "-")
        {
            fgen.msg("-", "AMSG", "Plese Select UserName first before Saving!!");
            txtuser.Focus();
            return;
        }

        if (txtuser.Text == frm_uname)
        {
            fgen.msg("-", "AMSG", "You Can't Issue Yourself !!Please Select another One!!");
            txtuser.Focus();
            return;
        }

        if (sg1.Rows.Count <= 1)
        {
            fgen.msg("-", "AMSG", "Please select atleast one item");
            return;
        }

        for (int k = 0; k < sg1.Rows.Count - 1; k++)
        {
            if (((TextBox)sg1.Rows[k].FindControl("sg1_t1")).Text == null || ((TextBox)sg1.Rows[k].FindControl("sg1_t1")).Text == "-" || ((TextBox)sg1.Rows[k].FindControl("sg1_t1")).Text == "")
            {
                fgen.msg("-", "AMSG", "Please Enter Start Date!!");
                return;
            }
            if (frm_cocd != "PIPL" && frm_cocd != "PPPL" && frm_cocd != "PPPF")
            {
                if (((TextBox)sg1.Rows[k].FindControl("sg1_t2")).Text == null || ((TextBox)sg1.Rows[k].FindControl("sg1_t2")).Text == "-" || ((TextBox)sg1.Rows[k].FindControl("sg1_t2")).Text == "")
                {
                    fgen.msg("-", "AMSG", "Please Enter End Date!!");
                    return;
                }
            }

            int datestart = fgen.ChkDate(((TextBox)sg1.Rows[k].FindControl("sg1_t1")).Text.ToString());
            if (datestart == 0)
            { fgen.msg("-", "AMSG", "Please Select a Valid Start Date"); return; }
            if (frm_cocd != "PIPL" && frm_cocd != "PPPL" && frm_cocd != "PPPF")
            {
                int dateend = fgen.ChkDate(((TextBox)sg1.Rows[k].FindControl("sg1_t2")).Text.ToString());
                if (dateend == 0)
                { fgen.msg("-", "AMSG", "Please Select a Valid End Date"); return; }
            }
            if (((TextBox)sg1.Rows[k].FindControl("sg1_t3")).Text == null || ((TextBox)sg1.Rows[k].FindControl("sg1_t3")).Text == "-" || ((TextBox)sg1.Rows[k].FindControl("sg1_t3")).Text == "")
            {
                fgen.msg("-", "AMSG", "Please Enter Start Time!!");
                return;
            }
            if (frm_cocd != "PIPL" && frm_cocd != "PPPL" && frm_cocd != "PPPF")
            {
                if (((TextBox)sg1.Rows[k].FindControl("sg1_t4")).Text == null || ((TextBox)sg1.Rows[k].FindControl("sg1_t4")).Text == "-" || ((TextBox)sg1.Rows[k].FindControl("sg1_t4")).Text == "")
                {
                    fgen.msg("-", "AMSG", "Please Enter End Time!!");
                    return;
                }
            }
        }


        if (edmode.Value != "Y")
        {
            for (int k = 0; k < sg1.Rows.Count - 1; k++)
            {
                if (Convert.ToDateTime(((TextBox)sg1.Rows[k].FindControl("sg1_t1")).Text) < Convert.ToDateTime(vardate))
                {

                    fgen.msg("-", "AMSG", "Issue Start Date can't be less then System Date!! ");
                    return;
                }
                //if (Convert.ToDateTime(((TextBox)sg1.Rows[k].FindControl("sg1_t2")).Text) < Convert.ToDateTime(vardate))
                //{

                //    fgen.msg("-", "AMSG", "Issue End Date can't be less then System Date!! ");
                //    return;
                //}
            }
        }



        if (edmode.Value == "Y")
        {


            if (txtentby.Text.Length < 2)
            {
                fgen.msg("-", "AMSG", "Please Enter Date of Closure!!");
                txtentby.Focus();
                return;
            }

            //int dateclos = fgen.ChkDate(txtentby.Text.ToString());
            //if (dateclos == 0)
            //{ fgen.msg("-", "AMSG", "Please Select a Valid Entry Date"); txtentby.Focus(); return; }

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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where branchcd='" + frm_mbr + "' and type='IV' and a.vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

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
                case "USR":
                    if (col1 == "") return;
                    TxtUserCode.Text = col1;
                    txtuser.Text = col2;
                    txtuser.Focus();
                    //txtMultiPlant.Value = col1;
                    //txtMultiPlant.Focus();
                    break;
                case "REQ":
                    txtreqno.Text = col2;
                    txtreqdt.Text = col3;
                    txtuser.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                    TxtUserCode.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT USERID FROM EVAS WHERE USERNAME='" + txtuser.Text.Trim() + "'", "USERID");
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
                    SQuery = "Select * from " + frm_tabname + " where BRANCHCD='" + frm_mbr + "' AND TYPE='IV' AND  VCHNUM||TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + col1 + "' and vchdate " + DateRange + " order by vchnum desc ";
                    //SQuery = "Select a.*,b.aname,c.iname from " + frm_tabname + " a,famst b,item c,wb_drawrec e,atchvch f where trim(a.mrrnum)||to_Char(a.mrrdate,'dd/mm/yyyy')||trim(a.unit)=trim(f.vchnum)||to_char(f.vchdate,'dd/mm/yyyy')||trim(f.msgtxt) and trim(a.mrrnum)||to_Char(a.mrrdate,'dd/mm/yyyy')||trim(a.icode)=trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy')||trim(e.icode) and e.type='DE' and f.type='DE' and trim(a.icode)=trim(c.icode) and trim(e.acodE)=trim(b.acode)  a.branchcd='" + frm_mbr + "' AND a.TYPE='IV' AND a.VCHNUM||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + col1 + "' and a.vchdate " + DateRange + " order by a.vchnum desc ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        string aname = "";
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtentby.Text = dt.Rows[0]["ENT_BY"].ToString().Trim();
                        txtedtby.Text = dt.Rows[0]["EDT_BY"].ToString().Trim();
                        txtentdt.Text = Convert.ToDateTime(dt.Rows[0]["ENT_DT"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtedtdt.Text = Convert.ToDateTime(dt.Rows[0]["EDT_DT"].ToString().Trim()).ToString("dd/MM/yyyy");
                        TxtUserCode.Text = dt.Rows[0]["USERCODE"].ToString().Trim();
                        txtuser.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT USERNAME FROM EVAS WHERE USERID='" + dt.Rows[0]["USERCODE"].ToString().Trim() + "'", "USERNAME");
                        txtrmk.Text = dt.Rows[0]["Remarks"].ToString().Trim();

                        txtreqno.Text = dt.Rows[0]["invno"].ToString().Trim();
                        txtreqdt.Text = dt.Rows[0]["invdate"].ToString().Trim();

                        dt.Dispose();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString()).ToString("dd/MM/yyyy");
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                        create_tab();
                        sg1_dr = null;

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_SrNo"] = i + 1;

                            sg1_dr["sg1_f1"] = dt.Rows[i]["MRRNUM"].ToString();
                            sg1_dr["sg1_f2"] = Convert.ToDateTime(dt.Rows[i]["MRRDATE"].ToString()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_f3"] = dt.Rows[i]["ICODE"].ToString();

                            if (wSeriesControl == "Y")
                                sg1_dr["sg1_f4"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NAME FROM TYPEGRP WHERE ID='P1' AND TRIM(TYPE1)='" + dt.Rows[i]["ICODE"].ToString() + "'", "NAME");
                            else sg1_dr["sg1_f4"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT INAME FROM ITEM WHERE TRIM(ICODE)='" + dt.Rows[i]["ICODE"].ToString() + "'", "INAME");
                            if (wSeriesControl == "Y")
                                sg1_dr["sg1_f5"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NAME FROM TYPEGRP WHERE TRIM(TYPE1)='" + dt.Rows[i]["ACODE"].ToString() + "'", "NAME");
                            else sg1_dr["sg1_f5"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT aNAME FROM famst WHERE TRIM(ACODE)='" + dt.Rows[i]["ACODE"].ToString() + "'", "ANAME");
                            sg1_dr["sg1_f6"] = dt.Rows[i]["unit"].ToString().Trim();
                            sg1_dr["sg1_f7"] = dt.Rows[i]["ACODE"].ToString().Trim();

                            sg1_dr["sg1_t1"] = Convert.ToDateTime(dt.Rows[i]["IssuestartDt"].ToString()).ToString("yyyy-MM-dd");
                            sg1_dr["sg1_t2"] = Convert.ToDateTime(dt.Rows[i]["Issueenddt"].ToString()).ToString("yyyy-MM-dd");
                            sg1_dr["sg1_t3"] = dt.Rows[i]["starttime"].ToString();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["endtime"].ToString();

                            sg1_dr["sg1_fstr"] = dt.Rows[i]["issuetime"].ToString();

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

                        btnuser.Enabled = false;

                        //txtentby.Text = DateTime.Now.ToString("yyyy-MM-dd");

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "disableFrame('div1');", true);
                    }
                    #endregion
                    break;
                case "DEPTT":
                case "Shift":
                    if (col1 == "") return;

                    break;
                case "MC":
                case "Incharge":

                case "Nature":

                case "Comp":

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
                        //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = dt.Rows[0]["unit"].ToString().Trim();
                        //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = "0";
                        //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = dt.Rows[0]["irate"].ToString().Trim();
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
                            sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                            sg1_dr["sg1_f6"] = dt.Rows[i]["sg1_f6"].ToString();
                            sg1_dr["sg1_f7"] = dt.Rows[i]["sg1_f7"].ToString();
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_fstr"] = ((TextBox)sg1.Rows[i].FindControl("sg1_fstr")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();

                        //if (col1.Length > 8) SQuery = "select * from item where trim(icode) in (" + col1 + ") and length(Trim(icode))>4 order by icode asc";
                        //else SQuery = "select * from item where trim(icode)=" + col1 + " and length(Trim(icode))>4 order by icode asc";
                        SQuery = "SELECT * FROM (" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL") + ") WHERE FSTR IN (" + col1 + ") ";

                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;

                            sg1_dr["sg1_f1"] = dt.Rows[d]["ENT_NO"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["ENT_DT"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["part_no"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["part_name"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["customer"].ToString().Trim();
                            sg1_dr["sg1_f6"] = dt.Rows[d]["filename"].ToString().Trim();
                            sg1_dr["sg1_f7"] = dt.Rows[d]["acode"].ToString().Trim();
                            sg1_dr["sg1_t1"] = DateTime.Now.ToString("yyyy-MM-dd");
                            sg1_dr["sg1_t2"] = "2050-01-01";
                            sg1_dr["sg1_t3"] = DateTime.Now.ToString("HH:mm");
                            sg1_dr["sg1_t4"] = DateTime.Now.ToString("HH:mm");
                            sg1_dr["sg1_fstr"] = ((TextBox)sg1.Rows[i].FindControl("sg1_fstr")).Text.Trim();
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

            SQuery = "select distinct a.VCHNUM||TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS FSTR,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,vchnum,to_char(a.issuestartdt,'dd/mm/yyyy') as issuestartdt,to_char(a.issueenddt,'dd/mm/yyyy') as issueenddt,a.starttime,a.endtime,a.Icode,b.iname,a.type from om_drwg_make a , item b where trim(a.icode)=trim(b.icode) and  a.type='IV' and a.branchcd='" + frm_mbr + "' and a.vchnum<>'000000' and a.vchdate " + PrdRange + " order by a.vchnum desc";
            SQuery = "select A.VCHNUM AS ENTRYNO,TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,b.name AS Product_name,b.acref as part_name,C.NAME AS CUSTOMER,a.mrrnum as drawing_entryno,a.unit as file_path,a.issuestartdt,a.starttime,a.ent_by,a.ent_dt from om_drwg_make a , TYPEGRP b, TYPEGRP C where trim(a.icode)=trim(b.TYPE1) AND TRIM(A.ACODE)=TRIM(c.TYPE1) and a.type='IV' AND B.ID='P1' AND C.ID='C1' and a.branchcd='" + frm_mbr + "' and a.vchnum<>'000000' and a.vchdate " + PrdRange + " order by a.vchnum desc";
            //SQuery = "select distinct VCHNUM||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR,to_char(vchdate,'dd/mm/yyyy') as vchdate,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt,vchnum,type from " + frm_tabname + " where  type='IV' and branchcd='" + frm_mbr + "' and vchnum<>'000000' and vchdate " + DateRange + " order by vchnum desc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevelJS("List of " + lblheader.Text + "", frm_qstr);
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

                    if (edmode.Value == "Y")
                    {
                        save_it = "Y";
                        frm_vnum = txtvchnum.Text;
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
                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vchnum) AS VCH FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + " order by vchdate desc ", 6, "VCH");
                                pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                if (i > 20)
                                {
                                    fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(vchnum) AS VCH FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + " order by vchdate desc ", 6, "VCH");
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
                        cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "' and vchdate " + DateRange + "";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);


                    }
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);


                    if (edmode.Value == "Y")
                    {
                        cmd_query = "delete from " + frm_tabname + " where branchcd='DD' and type='" + frm_vty + "' and  vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "' and vchdate " + DateRange + " ";

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
        srno = fgen.seek_iname(frm_qstr, frm_cocd, "select max(srno) as srno  from " + frm_tabname + "", "srno");

        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["VCHNUM"] = txtvchnum.Text.Trim();
            oporow["VCHDATE"] = txtvchdate.Text.Trim();

            oporow["SRNO"] = Convert.ToInt32(srno) + 1;
            oporow["MRRNUM"] = sg1.Rows[i].Cells[3].Text.Trim();
            oporow["MRRDATE"] = sg1.Rows[i].Cells[4].Text.Trim();
            oporow["ICODE"] = sg1.Rows[i].Cells[5].Text.Trim();  //oporow["MRATE"] = txtrate.Text.Trim();
            oporow["ACODE"] = sg1.Rows[i].Cells[9].Text.Trim();  //oporow["MRATE"] = txtrate.Text.Trim();
            oporow["UNIT"] = sg1.Rows[i].Cells[8].Text.Trim();

            oporow["USERCODE"] = TxtUserCode.Text.Trim();

            oporow["IssuestartDt"] = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text).ToString("dd/MM/yyyy");

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().Length < 3) oporow["Issueenddt"] = "01/01/2050";
            else oporow["Issueenddt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();

            oporow["starttime"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
            if (((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().Length < 3)
                oporow["endtime"] = "12:00";
            else oporow["endtime"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
            oporow["issuetime"] = ((TextBox)sg1.Rows[i].FindControl("sg1_fstr")).Text.Trim();

            if (txtrmk.Text.Length > 100) oporow["remarks"] = txtrmk.Text.Trim().Substring(0, 99);
            else oporow["remarks"] = txtrmk.Text.Trim();

            //vardate = fgen.seek_iname(co_cd, "Select to_date(to_char(sysdate,'dd/mm/yyyy HH:mi:ss'),'dd/mm/yyyy HH:mi:ss') as sysd from dual", "sysd");

            oporow["invno"] = txtreqno.Text.Trim();
            oporow["invdate"] = txtreqdt.Text.Trim();


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
                oporow["edt_dt"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);
        }


    }

    void save_fun2()
    {
        int xcount = 0;
        //xcount = sg1.Rows.Count;

        //oporow = oDs1.Tables[0].NewRow();
        //oporow["BRANCHCD"] = frm_mbr;
        //oporow["vchnum"] = frm_vnum;
        //oporow["vchdate"] = TxtDate.Value.ToString();
        //oporow["Type"] = "MN";
        //oporow["icode"] = "-";
        //oporow["Srno"] = 1;
        //oporow["col1"] = 1;
        //oporow["col2"] = "-";
        //oporow["sampqty"] = 0;
        //oporow["col3"] = "-";
        //oporow["col4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text;
        //oporow["ent_dt"] = DateTime.Today.ToShortDateString();
        //oporow["ent_by"] = frm_uname;
        //oDs1.Tables[0].Rows.Add(oporow);

        //for (i = 0; i < sg1.Rows.Count - 1; i++)
        //{
        //    //if (sg1.Rows[i].Cells[1].Text.Length > 2)
        //    //{
        //    oporow = oDs1.Tables[0].NewRow();
        //    oporow["BRANCHCD"] = frm_mbr;
        //    oporow["vchnum"] = frm_vnum;
        //    oporow["vchdate"] = TxtDate.Value.ToString();
        //    oporow["Type"] = "SS";
        //    oporow["icode"] = sg1.Rows[i].Cells[3].Text.Trim();
        //    oporow["Srno"] = sg1.Rows[i].Cells[2].Text.Trim();
        //    oporow["col1"] = sg1.Rows[i].Cells[3].Text.Trim();
        //    oporow["col2"] = sg1.Rows[i].Cells[4].Text.Trim();
        //    oporow["sampqty"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text);
        //    oporow["col3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text;
        //    oporow["col4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text;
        //    oporow["ent_dt"] = DateTime.Today.ToShortDateString();
        //    oporow["ent_by"] = frm_uname;
        //    oDs1.Tables[0].Rows.Add(oporow);
        //    //}
    }
    //}
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

            CheckBox CHK1 = (CheckBox)e.Row.FindControl("sg1_chkbox1");
            CheckBox CHK2 = (CheckBox)e.Row.FindControl("sg1_chkbox2");

            if (sg1_dt.Rows[e.Row.RowIndex][5].ToString() == "Y") CHK1.Checked = true;
            else CHK1.Checked = false;

            if (sg1_dt.Rows[e.Row.RowIndex][6].ToString() == "Y") CHK2.Checked = true;
            else CHK2.Checked = false;

            //((TextBox)e.Row.FindControl("sg1_t1")).Attributes.Add("readonly", "readonly");
            //((TextBox)e.Row.FindControl("sg1_t2")).Attributes.Add("readonly", "readonly");

            sg1.HeaderRow.Cells[2].Text = "SNo";
            sg1.HeaderRow.Cells[3].Text = "Ent No.";
            sg1.HeaderRow.Cells[4].Text = "Ent_Dt";
            sg1.HeaderRow.Cells[5].Text = "Erp Code";
            sg1.HeaderRow.Cells[5].Width = 80;
            sg1.HeaderRow.Cells[6].Text = "Part Name";
            sg1.HeaderRow.Cells[6].Width = 250;
            sg1.HeaderRow.Cells[7].Text = "Customer";
            sg1.HeaderRow.Cells[7].Width = 250;
            sg1.HeaderRow.Cells[8].Text = "FileName";
            sg1.HeaderRow.Cells[9].Text = "Code";
            sg1.HeaderRow.Cells[10].Text = "Issue";

            sg1.HeaderRow.Cells[12].Text = "Start Date";
            sg1.HeaderRow.Cells[13].Text = "End Date";
            sg1.HeaderRow.Cells[14].Text = "Start Time";
            sg1.HeaderRow.Cells[15].Text = "End Time";

            if (frm_cocd == "PIPL" || frm_cocd == "PPPL" || frm_cocd == "PPPF")
            {
                sg1.Columns[13].Visible = false;
                sg1.Columns[15].Visible = false;
            }

            //sg1.Columns[8].Visible = false;
            //sg1.Columns[9].Visible = false;
            sg1.Columns[10].Visible = false;
            sg1.Columns[11].Visible = false;
            sg1.Columns[16].Visible = false;
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

        if (txtvchnum.Text == "-")
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
        sg1_dt.Columns.Add(new DataColumn("sg1_f3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f7", typeof(string)));

        sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_fstr", typeof(string)));

    }
    //------------------------------------------------------------------------------------


    public void sg1_add_blankrows()
    {
        sg1_dr = sg1_dt.NewRow();

        sg1_dr["sg1_SrNo"] = sg1_dt.Rows.Count + 1;
        sg1_dr["sg1_f1"] = "-";
        sg1_dr["sg1_f2"] = "-";
        sg1_dr["sg1_f3"] = "-";
        sg1_dr["sg1_f4"] = "-";
        sg1_dr["sg1_t1"] = "-";
        sg1_dr["sg1_t2"] = "-";
        sg1_dr["sg1_t3"] = "-";
        sg1_dr["sg1_t4"] = "-";
        sg1_dr["sg1_fstr"] = "-";
        sg1_dt.Rows.Add(sg1_dr);
    }
    //------------------------------------------------------------------------------------

    void newCase(string vty)
    {
        #region
        if (frm_formID == "45116")
        {
            vty = "ID";
        }
        else if (frm_formID == "45117")
        {
            vty = "IV";
        }
        else
        {
            vty = "IV";
        }
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
    //protected void btnCamera_ServerClick(object sender, EventArgs e)
    //{
    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
    //    hffield.Value = "";
    //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL10", frm_mbr + frm_vty + txtvchnum.Text + Convert.ToDateTime(TxtDate.Value).ToString("dd_MM_yyyy"));
    //    fgen.open_sseek_camera("", frm_qstr);
    //}

    protected void btnuser_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "USR";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select User", frm_qstr);
    }
    protected void btnReq_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "REQ";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Request Slip No.", frm_qstr);
    }
}