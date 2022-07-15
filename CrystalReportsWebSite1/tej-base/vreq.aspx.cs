using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web;
using System.Text;
using System.IO;
using System.Drawing;


public partial class vreq : System.Web.UI.Page
{
    string vchnum, btnmode, col1, vardate, mlvl, otp;
    string tco_cd, cdt1, cdt2, scode, sname, seek, entby, edt, edmode, headername, xmlfile, rptpath, mailmsg, mflag, rptfilepath;
    string uright, can_add, can_edit, can_del, acessuser, getemail, sendtoemail, mailpath, xmltag, mailport, subject, branchname, col3, col4;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, DateRange, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, fromdt, todt, Prg_Id, SQuery;
    string doc_is_ok = "";
    string mq1 = "", mq2 = "", mq3 = "";
    DataTable dt = new DataTable();
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
                clearcontrol();

                btnenable();
                fgen.DisableForm(this.Controls);
            }
            btncancel.Visible = false;
            //btnnew.Focus();
            //btnenable();
        }
    }
    void getColHeading()
    {
        //dtCol = new DataTable();
        //dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        //if (dtCol == null || dtCol.Rows.Count <= 0)
        //{
        //    dtCol = fgen.getdata(frm_qstr, frm_cocd, fgenMV.Fn_Get_Mvar(frm_qstr, "U_SYS_COM_QRY") + " WHERE UPPER(TRIM(FRM_NAME))='" + frm_formID + "'");
        //}
        //ViewState["d" + frm_qstr + frm_formID] = dtCol;
    }
    void diableAllBtn()
    {
        btnnew.Disabled = true;
        btnedit.Disabled = true;
        btndel.Disabled = true;
    }
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true;
    }

    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        //btncancel.Visible = false;
        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;
    }

    public void DDBind()
    {
        ddvtype.Items.Clear();

        ddvtype.Items.Add(new System.Web.UI.WebControls.ListItem("GENERAL", "GENERAL"));
        ddvtype.Items.Add(new System.Web.UI.WebControls.ListItem("VIP", "VIP"));
    }

    public void bindtextboxes()
    {
        string sb = string.Empty;

        hf1.Value = string.Empty;

        hf1.Value = fgen.bindautodata(frm_qstr, frm_cocd, "SELECT distinct NAME from typemst where ID='YM'  order by NAME");
        sb = fgen.RunListScript("ctl00_ContentPlaceHolder1_txtcomp", hf1.Value);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "GCall1", sb.ToString(), false);

        hf1.Value = string.Empty;

        hf1.Value = fgen.bindautodata(frm_qstr, frm_cocd, "SELECT distinct col16 from  scratch2 where branchcd = '" + frm_mbr + "' and type='VR' and vchdate " + DateRange + "");
        sb = fgen.RunListScript("ctl00_ContentPlaceHolder1_txtvname", hf1.Value);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "HCall1", sb.ToString(), false);

        hf1.Value = string.Empty;

        hf1.Value = fgen.bindautodata(frm_qstr, frm_cocd, "SELECT NAME from typemst where ID='LC'  order by NAME");
        sb = fgen.RunListScript("ctl00_ContentPlaceHolder1_txtloc", hf1.Value);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ICall1", sb.ToString(), false);

        hf1.Value = string.Empty;

        hf1.Value = fgen.bindautodata(frm_qstr, frm_cocd, "SELECT NAME from typemst where ID='DG'  order by NAME");
        sb = fgen.RunListScript("ctl00_ContentPlaceHolder1_txtdesig", hf1.Value);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", sb.ToString(), false);


        hf1.Value = string.Empty;
        hf1.Value = fgen.bindautodata(frm_qstr, frm_cocd, "SELECT NAME from typemst where ID='DP'  order by NAME");
        sb = fgen.RunListScript("ctl00_ContentPlaceHolder1_txtdept", hf1.Value);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "KCall1", sb.ToString(), false);

        hf1.Value = string.Empty;

        hf1.Value = fgen.bindautodata(frm_qstr, frm_cocd, "SELECT NAME from typemst where ID='PS'  order by NAME");
        sb = fgen.RunListScript("ctl00_ContentPlaceHolder1_txtrmk", hf1.Value);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "LCall1", sb.ToString(), false);

        if (frm_cocd == "JSGI" || frm_cocd == "CCEL" || frm_cocd == "STUD" || frm_cocd == "ANYG" || frm_cocd == "UKB")
        {
            // tdmobfld.Visible = true;
            //tdmobhead.Visible = true;
        }
        else
        {
            // tdmobfld.Visible = false;
            // tdmobhead.Visible = false;
        }
    }

    public void btnenable()
    {
        btnnew.Disabled = false;
        btnedit.Disabled = false;
        btndel.Disabled = false;
        btnprint.Disabled = false;
        btnsave.Disabled = true;
        btnlist.Disabled = false;
        btnexit.Disabled = false;
        btnexit.InnerHtml = "Exit";
    }
    public void btndisable()
    {
        btnnew.Disabled = true;
        btnedit.Disabled = true;
        btndel.Disabled = true;
        btnprint.Disabled = true;
        btnsave.Disabled = true;
        btnlist.Disabled = true;
        btnexit.Disabled = false;
        btnexit.InnerHtml = "Cancel";
    }

    public void disp_data()
    {
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        SQuery = "";
        btnmode = hfbtnmode.Value;
        switch (btnmode)
        {

            case "VM":
                SQuery = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,col16 as visitor_name,col15 as comp_name, vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date, to_char(docdate,'dd/mm/yyyy') as visit_date,col32 as exp_time, REMARKS as visit_purpose,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from scratch2  where branchcd ='" + frm_mbr + "' and type='VR' and  vchdate " + DateRange + " AND UPPER(TRIM(col16)) LIKE '" + txtvname.Text + "%' AND UPPER(TRIM(col15)) LIKE '" + txtcomp.Text + "%'   order by VDD desc,vchnum desc ";
                break;

            case "SURE":
                SQuery = "Select 'YES' as col1,'Yes,Please' as Text,'Record Will be Deleted' as Action from dual union all Select 'NO' as col1,'No,Do Not' as Text,'Record Will Not be Deleted' as Action from dual";
                break;

            //case "APP":
            //    query = "SELECT USERID AS FSTR, USERNAME, USERID  , DEPTT AS DEPARTMENT FROM EVAS";
            //    break;

            //case"MODI":
            //    query = "SELECT USERID AS FSTR, USERNAME, USERID  , DEPTT AS DEPARTMENT FROM EVAS";
            //    break;

            //case "VM":
            //    //condition = " AND to_datE(docdate,'dd/mm/yyyy')=to_DatE(sysdate,'dd/mm/yyyy') ";
            //    if (frm_cocd == "JSGI") cond = "";
            //    else
            //    {
            //        cond = "nvl(trim(app_by),'-') != '-' and substr(app_by,1,3)!='[R]'";
            //    }
            //    SQuery = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,col16 as visitor_name,col15 as comp_name, vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date, to_char(docdate,'dd/mm/yyyy') as visit_date,col32 as exp_time, REMARKS as visit_reason,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + "  where branchcd ='" + frm_mbr + "' and type='VR' and  vchdate " + DateRange + " AND  " + cond + " and vchnum||to_char(vchdate,'DDMMYYYY') not in (select invno||to_char(invdate,'DDMMYYYY') from scratch2 where type='VM') order by VDD desc,vchnum desc ";
            //    break;
            case "EM_OLD"://OLD LOGIC 
                //SQuery = "select branchcd||'-'||EMPCODE as FSTR,replace(name,'&','') as NAME,EMPCODE,fhname as father_name,desg_text as Designation,deptt_Text as Department,Leaving_Dt,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt from EMPMAS WHERE BRANCHCD='" + frm_mbr + "' and nvl(Leaving_Dt,'-')='-' ORDER BY EMPCODE "; //OLD QRY
                SQuery = "SELECT FSTR,NAME,EMPCODE,FATHER_NAME,DESIGNATION,DEPARTMENT,LEAVING_DT,ENT_BY,ENT_DT FROM (select branchcd||'-'||EMPCODE as FSTR,replace(name,'&','') as NAME,EMPCODE,fhname as father_name,desg_text as Designation,deptt_Text as Department,Leaving_Dt,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt from EMPMAS WHERE BRANCHCD='" + frm_mbr + "' and nvl(Leaving_Dt,'-')='-'  UNION ALL SELECT 'Z99' AS FSTR,'OTHERS' AS NAME,'-' AS EMPCODE,'-' AS father_name ,'-' AS  Designation,'-' AS  Department,'-' AS Leaving_Dt,'-' AS ENT_BY,'-' AS ENT_DT FROM DUAL)  ORDER BY EMPCODE "; //with extra row others field
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count <= 0)
                {
                    //SQuery = "select branchcd||'-'||userid as fstr, replace(username,'&','') as NAME,userid ,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt ,'-' as others    from evas where branchcd='" + frm_mbr + "'";
                    SQuery = "select  fstr,name,userid,ent_by,ent_Dt from (select   branchcd||'-'||userid as fstr, replace(username,'&','') as NAME,userid ,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt    from evas WHERE BRANCHCD='" + frm_mbr + "' union all select 'Z99' as fstr,'OTHERS' as name,'-' as userid,'-' as ent_by,'-' as ent_Dt from dual)";
                }

                if (frm_cocd == "SAGM" || frm_cocd == "BUPL")
                    SQuery = "select branchcd||'-'||EMPCODE as FSTR,replace(name,'&','') as NAME,EMPCODE,fhname as father_name,desg_text as Designation,deptt_Text as Department,Leaving_Dt,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt from EMPMAS WHERE BRANCHCD!='DD' and nvl(Leaving_Dt,'-')='-' ORDER BY EMPCODE ";
                break;

            case "EM":
                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "select ID,PARAMS  from controls where ID ='W0062'", "PARAMS");//this is control for empmas and evas option.....when params=1 then data coming from empmas and when params=2 then coming from evas
                // mq1 = "2";//this is hardcode because still no control is created..need to rmv after control creation
                if (mq1 == "1")
                {
                    SQuery = "SELECT FSTR,NAME,EMPCODE,FATHER_NAME,DESIGNATION,DEPARTMENT,LEAVING_DT,ENT_BY,ENT_DT FROM (select branchcd||'-'||EMPCODE as FSTR,replace(name,'&','') as NAME,EMPCODE,fhname as father_name,desg_text as Designation,deptt_Text as Department,Leaving_Dt,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt from EMPMAS WHERE BRANCHCD='" + frm_mbr + "' and nvl(Leaving_Dt,'-')='-'  UNION ALL SELECT 'Z99' AS FSTR,'OTHERS' AS NAME,'-' AS EMPCODE,'-' AS father_name ,'-' AS  Designation,'-' AS  Department,'-' AS Leaving_Dt,'-' AS ENT_BY,'-' AS ENT_DT FROM DUAL)  ORDER BY EMPCODE "; //with extra row others field
                }
                else
                {
                    SQuery = "select  fstr,name,userid,ent_by,ent_Dt from (select   branchcd||'-'||userid as fstr, replace(username,'&','') as NAME,userid ,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt from evas WHERE BRANCHCD='" + frm_mbr + "' union all select 'Z99' as fstr,'OTHERS' as name,'-' as userid,'-' as ent_by,'-' as ent_Dt from dual)";
                    if (frm_cocd == "SAGM" || frm_cocd == "BUPL")
                        SQuery = "select branchcd||'-'||EMPCODE as FSTR,replace(name,'&','') as NAME,EMPCODE,fhname as father_name,desg_text as Designation,deptt_Text as Department,Leaving_Dt,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt from EMPMAS WHERE BRANCHCD!='DD' and nvl(Leaving_Dt,'-')='-' ORDER BY EMPCODE ";
                }
                break;

            default:
                if (btnmode == "Edit" || btnmode == "Print" || btnmode == "Del" || btnmode == "New_E")
                    SQuery = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,(CASE WHEN trim(NVL(app_by,'-')) = '-' THeN 'PENDING' WHEN SUBSTR(trim(app_by),1,3) = '[R]' THEN 'REJECTED' else 'APPROVED' end ) as AppROVE_status,col16 as visitor_name,COL23 as mobile_no,col15 as comp_name,  to_char(docdate,'dd/mm/yyyy') as visit_date,col32 as exp_time, REMARKS as visit_purpose,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + "  where branchcd ='" + frm_mbr + "' and type='" + frm_vty + "' and  vchdate " + DateRange + " order by VDD desc,vchnum desc ";
                break;
        }
        if (SQuery == "") { }
        else
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
    public void set_Val()
    {
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "SCRATCH2";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "VR");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
    }
    public void cleardata()
    {
        fgen.ResetForm(this.Controls);
    }
    public void clearcontrol()
    {
        fgen.ResetForm(this.Controls);
    }
    protected void btnnew_Click(object sender, EventArgs e)
    {
        clearcontrol();
        hfbtnmode.Value = "New";
        fgen.msg("-", "CMSG", "Do You want to Copy from old Request");
    }
    public void sseekfunc()
    {
        disp_data();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    public void OpenPopup(string popuptype)
    {
        headername = "";
        btnmode = hfbtnmode.Value;
        switch (popuptype)
        {
            case "SSEEK":
                switch (btnmode)
                {
                    case "VM":
                        headername = "Visitor Master";
                        break;
                    case "EM":
                        headername = "Select Whom To Meet";
                        break;
                    case "New":
                        headername = "Visitor Request";
                        break;
                    case "SURE":
                        headername = "Confirmation for Deletion";
                        break;
                    case "Edit":
                        headername = "Edit Visitor Requisition  Master";
                        break;
                    case "Print":
                        headername = "Print Visitor Requisition Master";
                        break;
                    case "Del":
                        headername = "Delete Visitor Requisition Master";
                        break;
                    default:
                        break;
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('" + headername + "','SSeek.aspx','75%','82%',false);});", true);
                break;
        }
    }
    protected void btnedit_Click(object sender, EventArgs e)
    {
        clearcontrol();
        hfbtnmode.Value = "Edit";
        hfedmode.Value = "Y";
        sseekfunc();
    }


    protected void btnhideF_Click(object sender, EventArgs e)
    {
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        scode = ""; sname = ""; seek = "";

        scode = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
        sname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
        seek = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

        btnmode = hfbtnmode.Value;
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        switch (btnmode)
        {
            case "New":
                if (Request.Cookies["REPLY"].Value.ToString() == "Y")
                {
                    hfbtnmode.Value = "New_E";
                    sseekfunc();
                }
                else
                {
                    fgen.EnableForm(this.Controls);

                    hfedmode.Value = "N";

                    SQuery = "select max(vchnum) as vch from " + frm_tabname + " where branchcd = '" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "";
                    txtdocno.Text = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch");
                    hffielddt.Value = vardate;
                    txtdate.Text = hffielddt.Value;
                    btndisable();
                    btnsave.Disabled = false;
                    txtpre.Text = frm_uname;
                    txtvtime.Text = DateTime.Now.ToString("HH:mm");
                    DDBind();
                }
                break;
            case "New_E":
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select * from scratch2  where branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + scode + "'");
                if (dt.Rows.Count <= 0) return;

                txtvname.Text = dt.Rows[0]["COL16"].ToString().Trim();
                txtcomp.Text = dt.Rows[0]["COL15"].ToString().Trim();
                txtvdate.Text = dt.Rows[0]["DOCDATE"].ToString().Trim().Substring(0, 10);
                txtvtime.Text = dt.Rows[0]["COL32"].ToString().Trim();
                txtname.Text = dt.Rows[0]["COL33"].ToString().Trim();
                txtempid.Text = dt.Rows[0]["COL34"].ToString().Trim();
                txtrmk.Text = dt.Rows[0]["REMARKS"].ToString().Trim();
                txtloc.Text = dt.Rows[0]["COL17"].ToString().Trim();
                txtdept.Text = dt.Rows[0]["COL19"].ToString().Trim();
                txtdesig.Text = dt.Rows[0]["COL21"].ToString().Trim();

                txtmobile.Text = dt.Rows[0]["COL23"].ToString().Trim();

                DDBind();
                col1 = "";
                col1 = dt.Rows[0]["COL22"].ToString().Trim();
                if (col1 == "" || col1 == "-") col1 = "GENERAL";
                ddvtype.SelectedValue = col1;

                fgen.EnableForm(this.Controls);
                btndisable();
                btnsave.Disabled = false;

                hfedmode.Value = "N";

                SQuery = "select max(vchnum) as vch from scratch2 where branchcd = '" + frm_mbr + "' and type='VR' and vchdate " + DateRange + "";
                txtdocno.Text = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch");
                hffielddt.Value = vardate;
                txtdate.Text = hffielddt.Value;
                btndisable();
                txtvtime.Text = DateTime.Now.ToString("HH:mm");
                btnsave.Disabled = false;
                txtpre.Text = frm_uname;
                break;
            case "VM":
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select COL15,DOCDATE,COL32,col33,col34, REMARKS from scratch2  where branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY')='" + scode + "'");

                if (dt.Rows.Count > 0)
                {
                    txtcomp.Text = dt.Rows[0]["COL15"].ToString().Trim();
                    txtvdate.Text = dt.Rows[0]["DOCDATE"].ToString().Trim().Substring(0, 10);
                    txtvtime.Text = dt.Rows[0]["COL32"].ToString().Trim();
                    txtempid.Text = dt.Rows[0]["COL34"].ToString().Trim();
                    txtname.Text = dt.Rows[0]["COL33"].ToString().Trim();
                    txtrmk.Text = dt.Rows[0]["REMARKS"].ToString().Trim();
                    txtloc.Text = dt.Rows[0]["COL17"].ToString().Trim();
                    txtdept.Text = dt.Rows[0]["COL19"].ToString().Trim();
                    txtdesig.Text = dt.Rows[0]["COL21"].ToString().Trim();
                }
                break;

            case "EM":
                if (scode == "") return;
                txtempid.Text = scode.Split('-')[0];
                txtname.Text = sname;
                if (sname.Contains("OTHERS")) { txtname.ReadOnly = false; } else { txtname.ReadOnly = true; }

                //no need to fill on req form
                //dt = new DataTable();
                //if (hf2.Value == "EVAS")
                //{
                //    SQuery = "select desg_text,deptt_Text from EVAS where BRANCHCD||'-'||trim(userid)='" + scode + "' ";

                //}
                //else
                //{
                //    SQuery = "select desg_text,deptt_Text from empmas where BRANCHCD||'-'||trim(empcode)='" + scode + "' ";
                //}
                // if (frm_cocd == "SAGM")
                //  SQuery = "select desg_text,deptt_Text from empmas where BRANCHCD||'-'||trim(empcode)='" + scode + "' ";
                //  dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                // if (dt.Rows.Count > 0)
                //  {
                //  txtedept.Text = dt.Rows[0][1].ToString().Trim();
                //txtedesing.Text = dt.Rows[0][0].ToString().Trim();
                //  }
                break;

            case "Edit":
                if (Convert.ToDouble(frm_ulvl) > 1)
                {
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "select app_by from scratch2 where branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + scode + "' and SUBSTR(trim(app_by),1,3) != '[U]' and SUBSTR(trim(app_by),1,3) != '-'");
                    if (dt.Rows.Count > 0)
                    {
                        fgen.msg("-", "AMSG", "sorry this req. has been approved you can not edit please contact to administrator");
                        return;
                    }
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select * from scratch2  where branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + scode + "'");
                if (dt.Rows.Count <= 0) return;

                txtdocno.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                hffielddt.Value = dt.Rows[0]["vchdate"].ToString().Trim().Substring(0, 10);
                txtdate.Text = hffielddt.Value;

                txtvname.Text = dt.Rows[0]["COL16"].ToString().Trim();
                txtcomp.Text = dt.Rows[0]["COL15"].ToString().Trim();
                txtvdate.Text = dt.Rows[0]["DOCDATE"].ToString().Trim().Substring(0, 10);
                txtvtime.Text = dt.Rows[0]["COL32"].ToString().Trim();
                txtname.Text = dt.Rows[0]["COL33"].ToString().Trim();//newly add field on form
                txtempid.Text = dt.Rows[0]["COL34"].ToString().Trim();//newly add field on form
                txtrmk.Text = dt.Rows[0]["REMARKS"].ToString().Trim();
                txtloc.Text = dt.Rows[0]["COL17"].ToString().Trim();
                txtdept.Text = dt.Rows[0]["COL19"].ToString().Trim();
                txtdesig.Text = dt.Rows[0]["COL21"].ToString().Trim();
                //oporow["COL32"] = txtvtime.Text;
                txtmobile.Text = dt.Rows[0]["COL23"].ToString().Trim();
                DDBind();
                col1 = "";
                col1 = dt.Rows[0]["COL22"].ToString().Trim();
                if (col1 == "" || col1 == "-") col1 = "GENERAL";
                ddvtype.SelectedValue = col1;

                entby = dt.Rows[0]["ent_by"].ToString().Trim();
                edt = dt.Rows[0]["ent_dt"].ToString().Trim();
                txtpre.Text = entby;
                txtedit.Text = dt.Rows[0]["eDt_by"].ToString().Trim() + "  " + Convert.ToDateTime(dt.Rows[0]["edt_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                txtapp.Text = dt.Rows[0]["app_by"].ToString().Trim() + "  " + Convert.ToDateTime(dt.Rows[0]["app_dt"].ToString().Trim()).ToString("dd/MM/yyyy");

                ViewState["ENTBY"] = entby;
                ViewState["ENTDT"] = edt;

                fgen.EnableForm(this.Controls);
                btndisable();
                btnsave.Disabled = false;
                break;
            case "Del":
                ViewState["COL1"] = scode;
                ViewState["COL2"] = sname;
                hfbtnmode.Value = "SURE";
                sseekfunc();
                break;
            case "SURE":
                if (scode == "NO") { }
                else
                {
                    scode = ""; sname = "";

                    scode = (string)ViewState["COL1"];
                    sname = (string)ViewState["COL2"];
                    string dd = scode.Substring(0, 2) + scode.Substring(4, 6) + scode.Substring(10, 10);
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from scratch2  where branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + scode + "'");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl where branchcd||trim(type)||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + scode + "'");

                    fgen.save_info(frm_qstr, frm_cocd, frm_mbr, scode.Substring(4, 6), scode.Substring(10, 10), frm_uname, scode.Substring(2, 2), lblheader.Text.ToUpper());
                    fgen.msg("-", "AMSG", "Doc No. " + sname + " has been Deleted Successfully.");
                    fgen.ResetForm(this.Controls);

                    ViewState["COL1"] = null;
                    ViewState["COL2"] = null;
                }
                break;
            //case "APP":
            //    query = "SELECT  USERNAME, USERID  , DEPTT AS DEPARTMENT FROM EVAS WHERE USERID='" + scode + "' ";
            //    dt = new DataTable();
            //    fgen.getdata(frm_qstr, frm_cocd, query);

            //    break;
            //case "MODI":
            //    query = "SELECT  USERNAME, USERID  , DEPTT AS DEPARTMENT FROM EVAS WHERE USERID='" + scode + "' ";
            //    dt = new DataTable();
            //    fgen.getdata(frm_qstr, frm_cocd, query);

            //    break;
            case "Print":
                SQuery = "select * from scratch2  where branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + scode + "'";
                fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "vreq", "vreq");
                break;
            default:
                break;
        }
    }
    protected void btnhideF_S_Click(object sender, EventArgs e)
    {
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = DateTime.Now.ToString("dd/MM/yyyy");

        edmode = hfedmode.Value;

        col1 = "";
        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
        if (col1 == "Y")
        {
            if (edmode == "Y")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "update scratch2  set branchcd='DD' where branchcd ='" + frm_mbr + "' and type='VR' and vchnum='" + txtdocno.Text.Trim() + "' and to_DatE(to_char(vchdate,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + hffielddt.Value + "','dd/mm/yyyy')");
                //cmd.ExecuteNonQuery();
            }

            DataSet oDS = new DataSet();
            oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
            DataRow oporow = null;

            vchnum = string.Empty;

            if (edmode == "Y") { frm_vnum = txtdocno.Text.Trim(); }
            else
            {
                //query = "select max(vchnum) as vch from " + frm_tabname + " where branchcd = '" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "";
                //txtdocno.Text = fgen.next_no(frm_qstr, frm_cocd, query, 6, "vch");
                doc_is_ok = "";
                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtdate.Text.Trim(), frm_uname, Prg_Id);
                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }

            }
            vchnum = frm_vnum.Trim();

            oporow = oDS.Tables[0].NewRow();
            oporow["vchnum"] = vchnum.Trim();
            oporow["vchdate"] = fgen.make_def_Date(txtdate.Text, vardate); ;
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["COL16"] = txtvname.Text;
            oporow["COL15"] = txtcomp.Text;
            oporow["docdate"] = fgen.make_def_Date(txtvdate.Text, vardate);
            oporow["COL32"] = txtvtime.Text;
            oporow["col33"] = txtname.Text.Trim();
            oporow["COL34"] = txtempid.Text.Trim();

            oporow["REMARKS"] = txtrmk.Text;

            oporow["COL17"] = txtloc.Text;
            oporow["COL19"] = txtdept.Text;
            oporow["COL21"] = txtdesig.Text;
            oporow["COL22"] = ddvtype.SelectedValue;
            // Mobile No for JSGI
            oporow["COL23"] = txtmobile.Text.Trim();

            oporow["app_by"] = "-";
            if (frm_cocd == "BUPL")
            {
                oporow["app_by"] = frm_uname;
                otp = fgen.gen_otp(frm_qstr, frm_cocd);
                oporow["COL28"] = otp;
            }
            oporow["app_dt"] = vardate;

            //oporow["chk_by"] = "-";
            //oporow["chk_dt"] = presentdate;

            if (edmode == "Y")
            {
                oporow["ent_by"] = frm_uname;
                oporow["ent_dt"] = vardate;
                oporow["edt_by"] = frm_uname;
                oporow["edt_dt"] = vardate;
            }
            else
            {
                oporow["ent_by"] = frm_uname;
                oporow["ent_dt"] = vardate;
                oporow["edt_by"] = "-";
                oporow["edt_dt"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);

            fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

            if (frm_cocd == "BUPL")
                fgen.send_sms(frm_qstr, frm_cocd, txtmobile.Text, "Dear " + txtvname.Text + ", Welcome to " + frm_cocd + ", Please show this OTP " + otp + " at the Gate.", frm_uname);

            string[] ARR = { "YM", "LC", "PS", "DG", "DP" };

            foreach (string gval in ARR)
            {
                SQuery = string.Empty;
                string code = string.Empty, checkval = string.Empty;

                if (gval == "YM") checkval = txtcomp.Text;
                if (gval == "LC") checkval = txtloc.Text;
                if (gval == "PS") checkval = txtrmk.Text;
                if (gval == "DG") checkval = txtdesig.Text;
                if (gval == "DP") checkval = txtdept.Text;

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select type1 from typemst where id='" + gval + "' and upper(trim(name))='" + checkval + "'");

                if (dt.Rows.Count > 0) { }
                else
                {
                    //presentdate = Convert.ToDateTime(presentdate).ToString("dd/MM/yyyy hh:mi:ss tt");
                    string dt_Vp = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy hh:mm:ss tt");
                    SQuery = "select max(TYPE1) as vch from typemst where id='" + gval + "' ";
                    code = fgen.next_no(frm_qstr, frm_cocd, SQuery, 4, "vch");
                    try
                    {
                        fgen.execute_cmd(frm_qstr, frm_cocd, "insert into typemst (BRANCHCD,type1,NAME,ID,ent_by,ent_dt,edt_by,edt_dt)values('00','" + code + "','" + checkval + "','" + gval + "','" + frm_uname + "',TO_DATE('" + dt_Vp + "','dd/MM/yyyy HH:MI:SS AM'),'-',TO_DATE('" + dt_Vp + "','dd/MM/yyyy HH:MI:SS AM')) ");
                    }
                    catch { }
                }
            }
            // VIPIN
            if (1 == 2)
                mflag = send_mail(mflag);

            if (mflag == "Y") mailmsg = "Email has been Sent Successfully to " + frm_uname + "";
            if (mflag == "N") mailmsg = "**Alert** No Email has been Sent!";

            if (edmode == "Y")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from scratch2  where branchcd='DD' and type='VR' and vchnum='" + txtdocno.Text.Trim() + "' and to_DatE(to_char(vchdate,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + hffielddt.Value + "','dd/mm/yyyy')");
                //cmd.ExecuteNonQuery();

                fgen.msg("-", "AMSG", "Visitor Requisition No. " + txtdocno.Text + " Dated " + hffielddt.Value + " Updated Successfully");
            }
            else fgen.msg("-", "AMSG", "Visitor Requisition No. " + txtdocno.Text + " Dated " + hffielddt.Value + " Saved Successfully");

            cleardata();
            fgen.DisableForm(this.Controls);
            btnenable();
            btnsave.Disabled = true;
            ddvtype.Items.Clear();
            //txtcomp.BorderColor = Color.White;
            //txtvname.BorderColor = Color.White;
            //txtloc.BorderColor = Color.White;
            //txtrmk.BorderColor = Color.White;
            //txtdept.BorderColor = Color.White;
            //txtdesig.BorderColor = Color.White;
            //txtmobile.BorderColor = Color.White;
        }
    }

    public void setdatabase(string scode)
    {
        //ds = new DataSet();
        //da = new OracleDataAdapter("select * from scratch2  where branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY')='" + scode + "'", con);
        //da.Fill(ds, "Prepcur");

        //   ds = fgen.Type_Data(co_cd, mbr, ds, "TYPE", "");            

        xmlfile = Server.MapPath("~/xmlfile/vreq.xml");
        //ds.WriteXml(xmlfile, XmlWriteMode.WriteSchema);

        //Session["mydataset"] = ds;

        //Response.Cookies["rptfile"].Value = "~/Report/vreq.rpt";

    }
    public void crystal_rpt()
    {
        rptpath = Request.Cookies["rptfile"].Value;
        CrystalDecisions.CrystalReports.Engine.ReportDocument report;
        report = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        rptfilepath = Server.MapPath("" + rptpath + "");
        report.Load(rptfilepath);
        //report.SetDataSource(ds);
        // CRV1.ReportSource = report;
        // CRV1.DataBind();
        //oStream = (MemoryStream)report.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
    }

    public string send_mail(string mflag)
    {
        dt = new DataTable();
        /*
        da = new OracleDataAdapter("select nvl(emailID,'-') as emailID FROM evas where upper(trim(username)) in ('" + uname + "')", con);
        da.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            sendtoemail = "";
            foreach (DataRow dr in dt.Rows)
            {
                if (sendtoemail.Length > 0)
                    sendtoemail += ",";
                if (dr["emailid"].ToString().Trim().Length > 4)
                    sendtoemail += dr["emailid"].ToString().Trim();
            }

            // sendtoemail = fgen.checkemail(sendtoemail);;

            if (hfedmode.Value == "Y")
                subject = " **Edited** Tejaxo ERP: New Visitor Req No. " + txtdocno.Text + " dated " + hffielddt.Value + " has been Created. ";
            else
                subject = " Tejaxo ERP: New Visitor Req No. " + txtdocno.Text + " dated " + hffielddt.Value + " has been Created. ";

            sb = new StringBuilder();
            sb.Append("<html><body>");

            sb.Append("<br> <b>" + uname + "</b> has created a requisition no. <b>" + txtdocno.Text + "</b> dated <b>" + hffielddt.Value + "</b> that requires your Approval.<br>");
            sb.Append("<br> Some Information : ");
            sb.Append("<br> Visitor Name : <b>" + txtvname.Text + "</b> ");
            sb.Append("<br> Company Name : <b>" + txtcomp.Text + "</b> ");
            sb.Append("<br> Location : <b>" + txtloc.Text + "</b> ");
            sb.Append("<br> Designation : <b>" + txtdesig.Text + "</b> ");
            sb.Append("<br> Department : <b>" + txtdept.Text + "</b> ");
            sb.Append("<br> Visitor Type : <b>" + ddvtype.SelectedValue + "</b> ");
            sb.Append("<br> Date of Visit : <b>" + txtvdate.Text + "</b> ");
            sb.Append("<br> Purpose of Visit : <b>" + txtrmk.Text + "</b> <br>");
            sb.Append("<br>Requested you to please click link given below to login Tejaxo ERP");


            xmltag = fgen.GetXMLTag("mailip").ToUpper();
            string[] mvar = xmltag.Split('@');
            mailpath = mvar[0].ToString().Trim();
            mailport = mvar[1].ToString().Trim();

            if (co_cd == "JSGI")
                sb.Append("<br><b><a href ='http://" + mailpath + ":" + mailport + "/aspnet_client/'>Tejaxo ERP Link</b></a>");
            else if (co_cd == "DLJM" || co_cd == "SDM")
                sb.Append("<br><b><a href ='http://" + mailpath + ":" + mailport + "/visit_system/'>Tejaxo ERP Link</b></a>");

            sb.Append("<br>");
            sb.Append("<br>Thanks & Regards,");
            branchname = "";
            //    branchname = fgen.Getfirmname(co_cd, mbr);
            //  branchname = fgen.Get_Type_Data(frm_qstr, frm_cocd, mbr);
            sb.Append("<br>" + branchname + "</b>");

            //presentdate = DateTime.Parse(fgen.InserTime(vardate), AustralianDateFormat);

            col4 = DateTime.Now.ToString("dd/MM/yyyy hh:mi:ss");

            sb.Append("<br>Date/Time :  " + col3 + "");

            sb.Append("<br><br><br>");
            sb.Append("</body></html>");


            dt = new DataTable();

            setdatabase(frm_mbr + "VR" + txtdocno.Text + hffielddt.Value.Replace("/", ""));
            crystal_rpt();

            //   mflag = fgen.SendMail(co_cd, sendtoemail, sb.ToString(), subject, "req_" + txtdocno.Text, co_cd + ":Visitor Req. Creation", dt, uname, oStream, null);

        }
        else mflag = "No Email id exist in database for sending mail of Visitor Req.";
        */
        return mflag;
    }


    protected void btnsave_Click(object sender, EventArgs e)
    {
        // // fgen.RemoveTextBoxBorder(this.Controls);
        ////alermsg.Style.Add("display", "none");
        if (frm_ulvl == "2.5")
        {
            fgen.msg("-", "AMSG", "Dear  " + frm_uname + ",You Have Rights to View Only, So ERP Will Not Allow You to Modify Data !");
            return;
        }

        if (txtvname.Text.Trim() == "")
        {
            fgen.msg("-", "AMSG", "Please enter visitor name.");
            txtvname.Focus();
            //txtvname.BorderColor = Color.Red;
            return;
        }

        if (txtcomp.Text.Trim() == "")
        {
            fgen.msg("-", "AMSG", "Please enter company name.");
            txtcomp.Focus();
            //txtcomp.BorderColor = Color.Red;
            return;
        }

        if (txtloc.Text.Trim() == "")
        {
            fgen.msg("-", "AMSG", "Please enter location.");
            txtloc.Focus();
            // txtloc.BorderColor = Color.Red;
            return;
        }
        if (txtrmk.Text.Trim() == "")
        {
            fgen.msg("-", "AMSG", "Please enter purpose of visit");
            txtrmk.Focus();
            //txtrmk.BorderColor = Color.Red;
            return;
        }
        if (txtdept.Text.Trim() == "")
        {
            fgen.msg("-", "AMSG", "Please enter department");
            txtdept.Focus();
            //txtdept.BorderColor = Color.Red;
            return;
        }
        if (txtdesig.Text.Trim() == "")
        {
            fgen.msg("-", "AMSG", "Please enter designation");
            txtdesig.Focus();
            //txtdesig.BorderColor = Color.Red;
            return;
        }

        if ((frm_cocd == "JSGI" || frm_cocd == "STUD" || frm_cocd == "CCEL" || frm_cocd == "ANYG" || frm_cocd == "UKB") && txtmobile.Text.Trim() == "" || txtmobile.Text.Length < 10)
        {
            if (txtmobile.Text == "" || txtmobile.Text == "-")
            {
                fgen.msg("-", "AMSG", "Please enter Mobile No.");
            }
            else
            {
                fgen.msg("-", "AMSG", "Please enter Mobile No. in Correct Format");
            }
            txtmobile.Focus();
            //txtmobile.BorderColor = Color.Red;
            return;
        }
        if (txtvdate.Text.Trim() == "")
        {
            fgen.msg("-", "AMSG", "Please enter proposed Date of Visit!!");
            txtvdate.Focus();
            return;
        }
        else
        {
            if (Convert.ToDateTime(txtvdate.Text) < Convert.ToDateTime(txtdate.Text))
            {
                fgen.msg("-", "AMSG", "Date of Visit Should be Greater than Entry Date!!");
                txtvdate.Focus();
                return;
            }
        }
        if (frm_cocd == "DISP" && hfedmode.Value != "Y")
        {
            if (Convert.ToDateTime(txtdate.Text) < Convert.ToDateTime(vardate))
            {
                fgen.msg("-", "AMSG", "Back Date Entry Not Allowed");
                txtdate.Focus();
                return;
            }
        }
        fgen.msg("-", "SMSG", "Are You Sure!! You Want to Save!!");
    }

    protected void btnOKTarget_Click(object sender, EventArgs e)
    {
        btnhideF_S_Click(sender, e);
    }
    protected void btnCancelTarget_Click(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();
        // setColHeadings();
    }
    public void clearctrl()
    {
        // hffield.Value = "";
        //edmode.Value = "";
    }
    protected void btnlist_Click(object sender, EventArgs e)
    {
        SQuery = "";
        clearcontrol();
        hfedmode.Value = "LI";

        headername = "Visitor Requisition Report";
        SQuery = "SELECT DISTINCT vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date,(CASE WHEN trim(NVL(app_by,'-')) = '-' THeN 'PENDING' WHEN SUBSTR(trim(app_by),1,3) = '[R]' THEN 'REJECTED' else 'APPROVED' end ) as AppROVE_status,col16 as visitor_name,col15 as comp_name,COL17 as  location,COL19 as dept,COL21 as desig,col22 as visitor_type,  to_char(docdate,'dd/mm/yyyy') as visit_date,col32 as expected_time, REMARKS as visit_purpose,ent_by,to_char(ent_dt,'YYYYMMDD') as ent_dt FROM scratch2 where type='VR' and branchcd ='" + frm_mbr + "' and  vchdate " + DateRange + " order by ENTRY_NO desc";

        if (SQuery == "") { }
        else
        {

            //fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);

            // Response.Cookies["seeksql"].Value = query;
            //Response.Cookies["headername"].Value = headername;
        }
        //  ScriptManager.RegisterStartupScript(btnlist, this.GetType(), "abcd", "$(document).ready(function(){OpenPopup('" + headername + "','rptlevel2.aspx','90%','97%',false);});", true);
        fgen.Fn_open_rptlevel(headername, frm_qstr);
    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        hfedmode.Value = "D";
        hfbtnmode.Value = "Del";
        sseekfunc();
    }
    protected void btnexit_Click(object sender, EventArgs e)
    {
        if (btnexit.InnerHtml == "Exit")
        {
            Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
        }
        else
        {
            clearcontrol();

            fgen.DisableForm(this.Controls);
            btnenable();
            btnsave.Disabled = true;
            ddvtype.Items.Clear();
            ViewState["ENTBY"] = null;
            ViewState["ENTDT"] = null;
            ViewState["COL1"] = null;
            ViewState["COL2"] = null;
        }
    }
    protected void btnprint_Click(object sender, EventArgs e)
    {
        hfbtnmode.Value = "Print";
        hfedmode.Value = "P";
        sseekfunc();
    }
    protected void btndept_Click(object sender, ImageClickEventArgs e)
    {
        System.Web.UI.WebControls.ImageButton button = (System.Web.UI.WebControls.ImageButton)sender;
        switch (button.ID)
        {
            case "btnemp":
                hfbtnmode.Value = "EM";
                break;
            case "btnvisit":
                hfbtnmode.Value = "VM";
                break;
        }
        sseekfunc();
    }



    //protected void btnmodi_Click(object sender, ImageClickEventArgs e)
    //{
    //    hfbtnmode.Value = "MODI";
    //    disp_data();
    //    fgen.Fn_open_sseek("Select Modified By Name ", frm_qstr);
    //}
    //protected void btnapp_Click(object sender, ImageClickEventArgs e)
    //{
    //    hfbtnmode.Value = "APP";
    //    disp_data();
    //    fgen.Fn_open_sseek("Select Approved By Name ", frm_qstr);
    //}
}