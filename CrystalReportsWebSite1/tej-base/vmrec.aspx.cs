using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using System.IO;
using System.Drawing;

public partial class vmrec : System.Web.UI.Page
{
    DataTable dt, pTable;
    IFormatProvider AustralianDateFormat;
    string vchdate, presentdate, entdate, invdate;
    string frm_qstr, mq1 = "", mq2 = "", mq3 = "";
    string vchnum, btnmode, vardate, mhd;
    string cdt1, cdt2, scode, sname, seek, entby, edt, edmode, headername, xmlfile;
    string uright, can_add, can_edit, can_del, acessuser, fileName, condition;
    string fName, fpath;

    // by akshay
    string btnval, SQuery, col1, col2, col3, cstr, fromdt, todt, year, cond = "", vip = "";
    string pk_error = "Y", chk_rights = "N", DateRange;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_cocd, frm_uname, frm_tabname, Prg_Id, frm_myear, frm_sql, frm_ulvl, frm_formID, frm_UserID;
    // DataTable dt;
    string doc_is_ok = "";
    DataRow oporow;
    fgenDB fgen = new fgenDB();
    DataSet oDS, dsRep = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
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
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    AustralianDateFormat = System.Globalization.CultureInfo.CreateSpecificCulture("en-AU").DateTimeFormat;

                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                r1.Visible = false;
                clearcontrol();
                if (Request.Cookies["U_RIGHT"] != null)
                {
                    uright = Request.Cookies["U_RIGHT"].Value.ToString();
                    can_add = uright.Substring(0, 1);
                    can_edit = uright.Substring(1, 1);
                    can_del = uright.Substring(2, 1);
                }
                if (can_add == "N") btnnew.Visible = false;
                else btnnew.Visible = true;
                if (can_edit == "N") btnedit.Visible = false;
                else btnedit.Visible = true;
                if (can_del == "N") btndelete.Visible = false;
                else btndelete.Visible = true;
                btnenable();
                btnsave.Disabled = true;
                //if (frm_cocd == "CCEL" || frm_cocd == "STUD" || frm_cocd == "ANYG" || frm_cocd == "UKB") lblOtp.Text = "Mobile No.";
                {
                    divOtp.Visible = false;
                    //   txtsrno.Height = 60;//AS PER MAYURI MAM
                }
                if (frm_cocd == "BUPL" || frm_cocd == "DISP" || frm_cocd == "OMP") divOtp.Visible = true;
                fgen.DisableForm(this.Controls);
                btnnew.Focus();
            }
        }
    }

    public void diableAllBtn()
    {
        btnnew.Disabled = true;
        btnedit.Disabled = true;
        btndelete.Disabled = true;
    }

    public void DDBind()
    {
        ddid.Items.Clear();
        ddcarry.Items.Clear();
        ddvtype.Items.Clear();

        ddid.Items.Add(new System.Web.UI.WebControls.ListItem("NO", "NO"));
        ddid.Items.Add(new System.Web.UI.WebControls.ListItem("YES", "YES"));

        ddcarry.Items.Add(new System.Web.UI.WebControls.ListItem("NO", "NO"));
        ddcarry.Items.Add(new System.Web.UI.WebControls.ListItem("YES", "YES"));

        ddvtype.Items.Add(new System.Web.UI.WebControls.ListItem("GENERAL", "GENERAL"));
        ddvtype.Items.Add(new System.Web.UI.WebControls.ListItem("VIP", "VIP"));
    }

    public void btnenable()
    {
        btnnew.Disabled = false;
        btnedit.Disabled = false;
        btndelete.Disabled = false;
        btnprint.Disabled = false;
        btnsave.Disabled = false;
        btnlist.Disabled = false;
        btnexit.Disabled = false;
        btnexit.InnerHtml = "Exit";
    }
    public void btndisable()
    {
        btnnew.Disabled = true;
        btnedit.Disabled = true;
        btndelete.Disabled = true;
        btnprint.Disabled = true;
        btnsave.Disabled = true;
        btnlist.Disabled = true;
        btnexit.Disabled = false;
        btnexit.InnerHtml = "Cancel";
    }
    public void set_Val()
    {
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "SCRATCH2";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "VM");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);

        if (frm_cocd == "DISP" || frm_cocd == "OMP")
        {
            txtempid.ReadOnly = false;
            txtname.ReadOnly = false;
        }
        else
        {
            txtedept.ReadOnly = true;
            txtdesig.ReadOnly = true;
        }
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
                //condition = " AND to_datE(docdate,'dd/mm/yyyy')=to_DatE(sysdate,'dd/mm/yyyy') ";
                if (frm_cocd == "JSGI") cond = "";
                else
                {
                    cond = "nvl(trim(app_by),'-') != '-' and substr(app_by,1,3)!='[R]'";
                }
                SQuery = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY') as fstr,col16 as visitor_name,col15 as comp_name, vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date, to_char(docdate,'dd/mm/yyyy') as visit_date,col32 as exp_time, REMARKS as visit_reason,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + "  where branchcd ='" + frm_mbr + "' and type='VR' and  vchdate " + DateRange + " AND  " + cond + " and vchnum||to_char(vchdate,'DDMMYYYY') not in (select invno||to_char(invdate,'DDMMYYYY') from scratch2 where type='VM') order by VDD desc,vchnum desc ";
                break;
            //case "EM_old":
            //    SQuery = "select branchcd||'-'||EMPCODE as FSTR,replace(name,'&','') as NAME,EMPCODE,fhname as father_name,desg_text as Designation,deptt_Text as Department,Leaving_Dt,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt from EMPMAS WHERE BRANCHCD='" + frm_mbr + "' and nvl(Leaving_Dt,'-')='-' ORDER BY EMPCODE ";
            //    if (frm_cocd == "SAGM" || frm_cocd == "BUPL")
            //        SQuery = "select branchcd||'-'||EMPCODE as FSTR,replace(name,'&','') as NAME,EMPCODE,fhname as father_name,desg_text as Designation,deptt_Text as Department,Leaving_Dt,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt from EMPMAS WHERE BRANCHCD!='DD' and nvl(Leaving_Dt,'-')='-' ORDER BY EMPCODE ";
            //    break;

            case "EM_OLD":
                SQuery = "select branchcd||'-'||EMPCODE as FSTR,replace(name,'&','') as NAME,EMPCODE,fhname as father_name,desg_text as Designation,deptt_Text as Department,Leaving_Dt,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt from EMPMAS WHERE BRANCHCD='" + frm_mbr + "' and nvl(Leaving_Dt,'-')='-' ORDER BY EMPCODE ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count <= 0)
                {
                    hf2.Value = "EVAS";
                    SQuery = "select branchcd||'-'||userid as fstr, replace(username,'&','') as NAME,userid ,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt ,'-' as others    from evas where branchcd='" + frm_mbr + "' ORDER BY USERID";
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
                    hf2.Value = "EVAS";
                    SQuery = "select  fstr,name,userid,ent_by,ent_Dt from (select   branchcd||'-'||userid as fstr, replace(username,'&','') as NAME,userid ,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt    from evas WHERE BRANCHCD='" + frm_mbr + "' union all select 'Z99' as fstr,'OTHERS' as name,'-' as userid,'-' as ent_by,'-' as ent_Dt from dual)";
                }
                if (frm_cocd == "SAGM" || frm_cocd == "BUPL")
                    SQuery = "select branchcd||'-'||EMPCODE as FSTR,replace(name,'&','') as NAME,EMPCODE,fhname as father_name,desg_text as Designation,deptt_Text as Department,Leaving_Dt,Ent_by,to_char(Ent_dt,'dd/mm/yyyy') as ent_dt from EMPMAS WHERE BRANCHCD!='DD' and nvl(Leaving_Dt,'-')='-' ORDER BY EMPCODE ";
                break;

            case "SURE":
                SQuery = "Select 'YES' as col1,'Yes,Please' as Text,'Record Will be Deleted' as Action from dual union all Select 'NO' as col1,'No,Do Not' as Text,'Record Will Not be Deleted' as Action from dual";
                break;
            default:
                if (btnmode == "Edit" || btnmode == "Print" || btnmode == "Del" || btnmode == "Tag")
                    SQuery = "SELECT distinct branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_date, col16 as visitor_name,col15 as comp_name,col17 as location,col12 as purpose,col19 as department,col21 as designation,to_char(docdate,'dd/mm/yyyy') as last_visited_on,col23 as mobile,col26 as mfg,col29 as serial_no,acode as empid,col1 as name,COL7 as emp_dept,COL8 as emp_desig,COL22 AS REQ_BY,REMARKS,ent_by,ent_dt, to_char(vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + "  where branchcd ='" + frm_mbr + "' and type='" + frm_vty + "' and  vchdate " + DateRange + " order by VDD desc,vchnum desc";
                break;
        }
        if (SQuery == "") { }
        else
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
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
        hfedmode.Value = "New";
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");

        SQuery = "select max(vchnum) as vch from scratch2 where branchcd = '" + frm_mbr + "' and type='VM' and vchdate " + DateRange + "";

        txtdocno.Text = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch");
        hffielddt.Value = vardate;
        txtdate.Text = hffielddt.Value;
        btndisable();
        btnsave.Disabled = false;
        // txttimein.Value = fgen.InserTime(vardate).Substring(11, 5);
        txttimein.Text = fgen.Fn_curr_dt_time(frm_qstr, (vardate)).Substring(11, 5);
        txtpre.Text = frm_uname;
        DDBind();
        fgen.EnableForm(this.Controls);
        if (frm_cocd == "AMSG" || frm_cocd == "CCEL" || frm_cocd == "STUD" || frm_cocd == "BUPL" || frm_cocd == "DISP") txtotp.Focus();
    }
    public void sseekfunc()
    {
        disp_data();
        OpenPopup("SSEEK");
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
                        headername = "Visitor Master(only approved and current date req. display here)";
                        break;
                    case "EM":
                        headername = "Select Whom To Meet";
                        break;
                    case "SURE":
                        headername = "Confirmation for Deletion";
                        break;
                    case "Edit":
                        headername = "Edit Visitor Movement Master";
                        break;
                    case "Print":
                        headername = "Print Visitor Movement Master";
                        break;
                    case "Del":
                        headername = "Delete Visitor Movement Master";
                        break;
                    case "Tag":
                        headername = "Select Entry To Print ID Tag";
                        break;

                    default:
                        break;
                }
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "$(document).ready(function(){OpenPopup('" + headername + "','SSeek.aspx','75%','82%',false);});", true);
                fgen.Fn_open_sseek(headername, frm_qstr);
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

    public Image byteArrayToImage(byte[] byteArrayIn)
    {
        Stream ms = new MemoryStream(byteArrayIn);
        Image returnImage = Image.FromStream(ms);
        return returnImage;
    }

    protected void btnhideF_Click(object sender, EventArgs e)
    {
        scode = ""; sname = ""; seek = "";
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");

        btnmode = hfbtnmode.Value;

        if (hfbtnmode.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();

            if (col1 == "Y")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from scratch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + scode + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from WSR_CTRL a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + scode + "'");
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, scode.Substring(4, 6), scode.Substring(10, 10), frm_uname, scode.Substring(2, 2), lblheader.Text.ToUpper());
                fgen.msg("-", "AMSG", "Details are deleted for " + lblheader.Text + " No. " + scode.Substring(4, 6) + "");
                fgen.ResetForm(this.Controls);
            }
        }
        else
        {
            scode = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            sname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            seek = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
            {
                switch (btnmode)
                {
                    case "VM":
                        dt = new DataTable();
                        SQuery = "select ent_by, COL15,COL16,col17,col19,col21,col22,col23,COL33,col34,REMARKS,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate from scratch2  where branchcd||type||vchnum||to_char(vchdate,'DDMMYYYY')='" + scode + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        if (dt.Rows.Count > 0)
                        {
                            txtvname.Text = dt.Rows[0]["COL16"].ToString().Trim();
                            txtcomp.Text = dt.Rows[0]["COL15"].ToString().Trim();
                            txtloc.Text = dt.Rows[0]["COL17"].ToString().Trim();
                            txtdept.Text = dt.Rows[0]["COL19"].ToString().Trim();
                            txtdesig.Text = dt.Rows[0]["COL21"].ToString().Trim();
                            txtvtype.Text = dt.Rows[0]["COL22"].ToString().Trim();

                            txtpurpose.Text = dt.Rows[0]["REMARKS"].ToString().Trim();
                            txtrid.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                            txtrdt.Text = dt.Rows[0]["vchdate"].ToString().Trim();
                            txtapp.Text = dt.Rows[0]["ent_By"].ToString().Trim();
                            txtmob.Text = dt.Rows[0]["col23"].ToString().Trim();
                            txtempid.Text = dt.Rows[0]["col34"].ToString().Trim();
                            txtname.Text = dt.Rows[0]["col33"].ToString().Trim();
                            txtloc.Focus();
                        }
                        break;

                    case "EM":
                        if (scode == "") return;
                        txtempid.Text = scode.Split('-')[0];
                        txtname.Text = sname;
                        if (sname.Contains("OTHERS")) { txtname.ReadOnly = false; } else { txtname.ReadOnly = true; }

                        dt = new DataTable();
                        SQuery = "select desg_text,deptt_Text from empmas where BRANCHCD||'-'||trim(empcode)='" + scode + "' ";
                        if (frm_cocd == "SAGM")
                            SQuery = "select desg_text,deptt_Text from empmas where BRANCHCD||'-'||trim(empcode)='" + scode + "' ";

                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        //da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            txtedept.Text = dt.Rows[0][1].ToString().Trim();
                            txtedesing.Text = dt.Rows[0][0].ToString().Trim();
                        }
                        break;

                    case "Edit":
                        #region
                        col1 = scode;
                        if (col1 == "" || col1 == "-" || col1 == null) return;
                        dt = new DataTable();
                        SQuery = "select * from scratch2 where branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + scode + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        // da.Fill(dt);

                        txtdocno.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        hffielddt.Value = dt.Rows[0]["vchdate"].ToString().Trim().Substring(0, 10);
                        txtdate.Text = hffielddt.Value;
                        txtvname.Text = dt.Rows[0]["COL16"].ToString().Trim();
                        txtcomp.Text = dt.Rows[0]["COL15"].ToString().Trim();
                        txtloc.Text = dt.Rows[0]["COL17"].ToString().Trim();
                        txtpurpose.Text = dt.Rows[0]["COL12"].ToString().Trim();
                        txtdept.Text = dt.Rows[0]["COL19"].ToString().Trim();
                        txtdesig.Text = dt.Rows[0]["COL21"].ToString().Trim();
                        txtvdate.Text = dt.Rows[0]["COL30"].ToString().Trim();
                        txtmob.Text = dt.Rows[0]["COL23"].ToString().Trim();
                        txtmfg.Text = dt.Rows[0]["COL26"].ToString().Trim();
                        try
                        {
                            rd_done.SelectedValue = dt.Rows[0]["COL27"].ToString().Trim();
                        }
                        catch { }
                        txtsrno.Text = dt.Rows[0]["COL29"].ToString().Trim();
                        txtempid.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtname.Text = dt.Rows[0]["COL1"].ToString().Trim();
                        txtedept.Text = dt.Rows[0]["COL7"].ToString().Trim();
                        txtedesing.Text = dt.Rows[0]["COL8"].ToString().Trim();
                        txttimein.Text = dt.Rows[0]["COL37"].ToString().Trim();
                        txttimeout.Text = dt.Rows[0]["COL38"].ToString().Trim();
                        txtAlotMin.Text = dt.Rows[0]["COL39"].ToString().Trim();
                        DDBind();
                        col1 = "";
                        col1 = dt.Rows[0]["COL31"].ToString().Trim();
                        if (col1 == "" || col1 == "-") col1 = "NO";
                        ddid.SelectedValue = col1;

                        col1 = "";
                        col1 = dt.Rows[0]["COL32"].ToString().Trim();
                        if (col1 == "" || col1 == "-") col1 = "NO";
                        ddcarry.SelectedValue = col1;

                        txtivalue.Text = dt.Rows[0]["COL35"].ToString().Trim();
                        txtiname.Text = dt.Rows[0]["COL36"].ToString().Trim();

                        txtrmk.Text = dt.Rows[0]["REMARKS"].ToString().Trim();
                        fName = ""; fpath = "";
                        fpath = dt.Rows[0]["col14"].ToString().Trim();
                        fName = getpath(fpath, fName);
                        empImage.ImageUrl = "~" + fName;

                        entby = dt.Rows[0]["ent_by"].ToString().Trim();
                        edt = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtpre.Text = entby + "  " + edt;
                        txtedit.Text = dt.Rows[0]["eDt_by"].ToString().Trim() + "  " + Convert.ToDateTime(dt.Rows[0]["edt_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtapp.Text = dt.Rows[0]["COL22"].ToString().Trim();

                        txtapp.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");

                        if (txttimein.Text == "-") txttimein.Text = edt.Substring(11, 5);

                        ViewState["ENTBY"] = entby;
                        ViewState["ENTDT"] = edt;

                        try
                        {
                            txtrid.Text = dt.Rows[0]["invno"].ToString().Trim();
                            txtrdt.Text = dt.Rows[0]["invdate"].ToString().Trim().Substring(0, 10);
                        }
                        catch
                        {

                            dt = new DataTable();

                            SQuery = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate from scratch2  where UPPER(TRIM(COL16))='" + txtvname.Text + "' AND UPPER(TRIM(COL15))='" + txtcomp.Text + "' AND UPPER(TRIM(REMARKS))='" + txtrmk.Text + "' AND TYPE='VR' ";
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            //da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                txtrid.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                                txtrdt.Text = dt.Rows[0]["vchdate"].ToString().Trim();
                            }
                        }

                        //fgen.EnableForm(this.Page);
                        fgen.EnableForm(this.Controls);
                        btndisable();
                        btnsave.Disabled = false;
                        #endregion
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

                            // by akshay
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from scratch2  where branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + scode + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + scode + "'");
                            fgen.save_info(frm_qstr, frm_cocd, frm_mbr, scode.Substring(4, 6), scode.Substring(10, 10), frm_uname, scode.Substring(2, 2), lblheader.Text.ToUpper());
                            fgen.msg("-", "AMSG", "Doc No. " + sname + " has been Deleted Successfully.");
                            fgen.ResetForm(this.Controls);

                            ViewState["COL1"] = null;
                            ViewState["COL2"] = null;
                        }
                        break;
                    case "Print":
                        #region
                        SQuery = "select * from scratch2  where branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + scode + "'";
                        DataSet dsn = new DataSet();
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        dt.Columns.Add("pic", typeof(System.Byte[]));
                        dt.Columns.Add("img1", typeof(System.Byte[]));
                        dt.Columns.Add("img1_desc", typeof(string));
                        fpath = "";
                        string bValue = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            try
                            {
                                col1 = scode;

                                fpath = Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "") + ".png");
                                del_file(fpath);

                                fgen.prnt_QRbar(frm_cocd, col1, col1.Replace("*", "").Replace("/", "") + ".png");

                                FileStream FilStr;
                                BinaryReader BinRed;

                                FilStr = new FileStream(fpath, FileMode.Open);
                                BinRed = new BinaryReader(FilStr);

                                dr["img1_desc"] = col1.Trim();
                                dr["img1"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);

                                FilStr.Close();
                                BinRed.Close();

                                fpath = dr["COL14"].ToString().Trim();

                                FilStr = new FileStream(fpath, FileMode.Open);
                                BinRed = new BinaryReader(FilStr);

                                dr["pic"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                                FilStr.Close();
                                BinRed.Close();
                            }
                            catch { }
                        }
                        dt.TableName = "Prepcure";
                        dsn.Tables.Add(dt);
                        fgen.Print_Report_BYDS(frm_cocd, frm_qstr, frm_mbr, "vmrec", "vmrec", dsn, "");
                        break;
                        #endregion
                    case "Tag":
                        #region
                        SQuery = "select * from scratch2  where branchcd||type||vchnum||to_char(vchdate,'DD/MM/YYYY')='" + scode + "'";

                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        dt.Columns.Add("pic", typeof(System.Byte[]));
                        dt.Columns.Add("img1", typeof(System.Byte[]));
                        dt.Columns.Add("img1_desc", typeof(string));
                        fpath = "";
                        foreach (DataRow dr in dt.Rows)
                        {
                            try
                            {
                                col1 = scode;

                                fpath = Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "") + ".png");
                                del_file(fpath);

                                fgen.prnt_QRbar(frm_cocd, col1, col1.Replace("*", "").Replace("/", "") + ".png");

                                FileStream FilStr;
                                BinaryReader BinRed;

                                FilStr = new FileStream(fpath, FileMode.Open);
                                BinRed = new BinaryReader(FilStr);

                                dr["img1_desc"] = col1.Trim();
                                dr["img1"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);

                                FilStr.Close();
                                BinRed.Close();

                                fpath = dr["COL14"].ToString().Trim();
                                FilStr = new FileStream(fpath, FileMode.Open);
                                BinRed = new BinaryReader(FilStr);
                                dr["pic"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                                FilStr.Close();
                                BinRed.Close();
                            }
                            catch { }
                        }
                        dt.TableName = "Prepcure";
                        dsRep.Tables.Add(dt);
                        fgen.Print_Report_BYDS(frm_cocd, frm_qstr, frm_mbr, "vmrec_tag", "vmrec_tag", dsRep, "");
                        #endregion
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public string getpath(string fpath, string fName)
    {
        try
        {
            if (fpath == "" || fpath == "-") { }
            else
            {
                empImage.ImageUrl = "~/images/myImage.jpg?" + DateTime.Now.Ticks.ToString();
                int i = 0;
                i = fpath.IndexOf(@"~/tej-base/Upload");
                fName = fpath.Substring(i, fpath.Length - i);
            }
        }
        catch { }
        return fName;
    }

    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");

        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        edmode = hfedmode.Value;

        col1 = "";
        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();

        if (col1 == "N") { }
        else
        {
            if (edmode == "Y")
            {
                entdate = edt;
                fgen.execute_cmd(frm_qstr, frm_cocd, "update scratch2  set branchcd='DD' where branchcd ='" + frm_mbr + "' and type='VM' and vchnum='" + txtdocno.Text.Trim() + "' and to_DatE(to_char(vchdate,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + hffielddt.Value + "','dd/mm/yyyy')");
            }

            oDS = new DataSet();

            oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

            vchnum = string.Empty;

            if (edmode == "Y") { frm_vnum = txtdocno.Text.Trim(); }
            else
            {
                //SQuery = "select max(vchnum) as vch from scratch2 where branchcd = '" + frm_mbr + "' and type='VM' and vchdate " + DateRange + "";
                //txtdocno.Text = fgen.Gen_No(frm_cocd, SQuery, "vch", 6);
                //txtdocno.Text = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch"); // by akshay
                doc_is_ok = "";
                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtdate.Text.Trim(), frm_uname, Prg_Id);
                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
            }
            //vchnum = txtdocno.Text.Trim();
            vchnum = frm_vnum.Trim();
            oporow = oDS.Tables[0].NewRow();
            oporow["vchnum"] = vchnum.Trim();
            oporow["vchdate"] = fgen.make_def_Date(txtdate.Text.Trim(), vardate);
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["COL16"] = txtvname.Text.ToUpper().Trim();
            oporow["COL15"] = txtcomp.Text.ToUpper().Trim();
            oporow["COL17"] = txtloc.Text.ToUpper().Trim();
            oporow["COL12"] = txtpurpose.Text.ToUpper().Trim();
            oporow["COL19"] = txtdept.Text.ToUpper().Trim();
            oporow["COL21"] = txtdesig.Text.ToUpper().Trim();
            oporow["COL30"] = txtvdate.Text.ToUpper().Trim();
            oporow["COL23"] = txtmob.Text.ToUpper().Trim();
            oporow["COL26"] = txtmfg.Text.ToUpper().Trim();
            oporow["COL29"] = txtsrno.Text.ToUpper().Trim();
            oporow["acode"] = txtempid.Text.ToUpper().Trim();
            oporow["COL1"] = txtname.Text.ToUpper().Trim();
            oporow["COL7"] = txtedept.Text.ToUpper().Trim();
            oporow["COL8"] = txtedesing.Text.ToUpper().Trim();
            oporow["COL31"] = ddid.SelectedValue;
            oporow["COL32"] = ddcarry.SelectedValue;
            oporow["COL35"] = txtivalue.Text.ToUpper().Trim();
            oporow["COL36"] = txtiname.Text.ToUpper().Trim();
            oporow["COL37"] = txttimein.Text.ToUpper().Trim();
            oporow["COL38"] = txttimeout.Text.ToUpper().Trim();
            oporow["COL39"] = txtAlotMin.Text.ToUpper().Trim();
            oporow["INVNO"] = txtrid.Text;
            if (txtrdt.Text.Trim().Length < 2)
            {
                oporow["INVdate"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
            }
            else
            {
                oporow["INVdate"] = Convert.ToDateTime(txtrdt.Text.ToUpper().Trim()).ToString("dd/MM/yyyy");
            }
            oporow["COL22"] = txtapp.Text.ToUpper().Trim();
            oporow["COL28"] = txtotp.Text.Trim();
            oporow["REMARKS"] = txtrmk.Text.ToUpper().Trim();
            oporow["COL27"] = rd_done.SelectedValue.ToString().Trim();//DIRECT RB VALUE

            fpath = ""; fName = "";

            if (imgData.Value == null | imgData.Value == "") { fpath = "-"; }
            else
            {
                string imgStr = imgData.Value;
                byte[] bytes = Convert.FromBase64String(imgStr);
                Image img = byteArrayToImage(bytes);

                fName = frm_mbr + frm_vty + vchnum + Convert.ToDateTime(oporow["vchdate"].ToString()).ToString("ddMMyyyy") + ".png";
                fpath = Server.MapPath(@"~/tej-base/Upload") + "\\" + fName;
                img.Save(fpath, System.Drawing.Imaging.ImageFormat.Png);
            }

            oporow["COL14"] = fpath;
            oporow["app_by"] = "-";
            oporow["app_dt"] = vardate;

            if (edmode == "Y")
            {
                oporow["eNt_by"] = ViewState["ENTBY"].ToString();
                oporow["eNt_dt"] = ViewState["ENTDT"].ToString();
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

            fgen.save_data(frm_qstr, frm_cocd, oDS, "scratch2");

            if (edmode == "Y")
            {
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from scratch2  where branchcd='DD' and type='VM' and vchnum='" + txtdocno.Text.Trim() + "' and to_DatE(to_char(vchdate,'dd/mm/yyyy'),'dd/mm/yyyy')=to_Date('" + hffielddt.Value + "','dd/mm/yyyy')");

                fgen.msg("-", "AMSG", "Visitor Movement No. " + txtdocno.Text + " Dated " + hffielddt.Value + " Updated Successfully.");
            }
            else
                fgen.msg("-", "AMSG", "Visitor Movement No. " + txtdocno.Text + " Dated " + hffielddt.Value + " Saved Successfully.");

            cleardata();
            empImage.ImageUrl = string.Empty;
            //fgen.DisableForm(this.Page);
            fgen.DisableForm(this.Controls);
            btnenable();
            btnsave.Disabled = true;

            //con.Close();
            ddcarry.Items.Clear();
            ddid.Items.Clear();
            txtapp.BackColor = System.Drawing.Color.Empty;
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        //fgen.RemoveTextBoxBorder(this.Page);

        //alermsg.Style.Add("display", "none");
        if (frm_ulvl == "2.5")
        {
            //fgen.msg("AMSG", "Dear  " + uname + ",You Have Rights to View Only, So ERP Will Not Allow You to Modify Data !");
            fgen.msg("-", "AMSG", "Dear  " + frm_uname + ",You Have Rights to View Only, So ERP Will Not Allow You to Modify Data !");
            return;
        }

        if (txtvname.Text.Trim() == "")
        {
            fgen.msg("-", "AMSG", "Please enter visitor name.");

            //  txtvname.BorderColor = Color.Red;
            return;
        }
        ////if visitor 
        if (rd_done.SelectedValue != "0")
        {
            if (txtmob.Text.Trim() == "")
            {
                fgen.msg("-", "AMSG", "Please enter mobile no.");
                //  txtmob.BorderColor = Color.Red;            
                return;
            }
            if (frm_cocd != "JSGI")
            {
                if (txtempid.Text.Trim() == "")
                {
                    fgen.msg("-", "AMSG", "Please enter Whom to Meet");
                    // txtempid.BorderColor = Color.Red;
                    return;
                }
            }
            if (ddid.SelectedValue == "YES")
            {
                if (txtiname.Text.Trim() == "" || txtiname.Text.Trim() == "-")
                {
                    fgen.msg("-", "AMSG", "Please enter ID Name.");
                    //  txtiname.BorderColor = Color.Red;
                    return;
                }
                if (txtivalue.Text.Trim() == "" || txtivalue.Text.Trim() == "-")
                {
                    fgen.msg("-", "AMSG", "Please enter ID Value.");
                    // txtivalue.BorderColor = Color.Red;
                    return;
                }
            }
            if (ddcarry.SelectedValue == "YES")
            {
                if (txtmfg.Text.Trim() == "" || txtmfg.Text.Trim() == "-")
                {
                    fgen.msg("-", "AMSG", "Please enter Laptop Manufacture Name.");
                    // txtmfg.BorderColor = Color.Red;
                    return;
                }
                if (txtsrno.Text.Trim() == "" || txtsrno.Text.Trim() == "-")
                {
                    fgen.msg("-", "AMSG", "Please enter Laptop Serial No.");
                    // txtsrno.BorderColor = Color.Red;
                    return;
                }
            }
            if ((frm_cocd == "JSGI" || frm_cocd == "BUPL" || frm_cocd == "DISP") && txtotp.Text.Trim() == "")
            {
                if (txtvtype.Text.Contains("VIP"))
                { }
                else
                {
                    fgen.msg("-", "AMSG", "Please enter OTP No.");
                    // txtotp.BorderColor = Color.Red;
                    return;
                }
            }
            if ((frm_cocd == "CCEL" || frm_cocd == "STUD") && txtotp.Text.Trim() == "")
            {
                fgen.msg("-", "AMSG", "Please enter Mobile No.");
                // txtotp.BorderColor = Color.Red;
                return;
            }
        }//close bracket of radio btn
        hfbtnmode.Value = "SURE_S";
        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "confirm", "<script>$(document).ready(function(){ MyConfirm2('Are you sure you want save Visitor Movement Record?'); });</script>", false);
        fgen.msg("-", "SMSG", "Are You Sure!! You Want to Save!!");
    }
    protected void btnOKTarget_Click(object sender, EventArgs e)
    {
        fgen.send_cookie("Column1", "Yes");
        btnhideF_s_Click(sender, e);
    }
    protected void btnCancelTarget_Click(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        btnenable();
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

        headername = "Visitor Movement Report";
        SQuery = "SELECT DISTINCT col16 as visitor_name,col15 as comp_name,col17 as location,col12 as purpose,col19 as department,col21 as designation,to_char(docdate,'dd/mm/yyyy') as last_visited_on,col23 as mobile,col26 as mfg,col29 as serial_no,acode as empid,col1 as name,COL7 as emp_dept,COL8 as emp_desig,COL22 AS REQ_BY, REMARKS,ent_by,ent_dt,COL37 as timein,COL38 as timeout,reason as outRemarks, to_char(vchdate,'YYYYMMDD') AS VDD FROM scratch2 where type='VM' and branchcd ='" + frm_mbr + "' and  vchdate " + DateRange + " order by VDD desc";

        if (SQuery == "") { }
        else
        {
            //fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);

            //Response.Cookies["seeksql"].Value = query;
            //Response.Cookies["headername"].Value = headername;
        }
        // ScriptManager.RegisterStartupScript(btnlist, this.GetType(), "abcd", "$(document).ready(function(){OpenPopup('" + headername + "','rptlevel2.aspx','90%','97%',false);});", true);
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
            empImage.ImageUrl = string.Empty;
            //fgen.DisableForm(this.Page);
            fgen.DisableForm(this.Controls);
            btnenable();
            btnsave.Disabled = true;
            ddcarry.Items.Clear();
            ddid.Items.Clear();
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

    // ******************* Otp Checking
    protected void txtotp_TextChanged(object sender, EventArgs e)
    {
        // fgen.RemoveTextBoxBorder(this.Page);
        //alermsg.Style.Add("display", "none");

        mhd = "";
        string cond = "", mhd1 = ""; ;
        if (frm_cocd == "CCEL" || frm_cocd == "STUD" || frm_cocd == "ANYG" || frm_cocd == "UKB") cond = "and trim(upper(col23))='" + txtotp.Text.Trim() + "'";
        else cond = "and trim(upper(col28))='" + txtotp.Text.Trim() + "'";
        if (frm_cocd == "BUPL") cond = "AND (trim(upper(col23))='" + txtotp.Text.Trim() + "' or trim(upper(col28))='" + txtotp.Text.Trim() + "')";
        if (frm_cocd == "CCEL" || frm_cocd == "STUD" || frm_cocd == "ANYG" || frm_cocd == "UKB") mhd = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(upper(COL23)) as otp from scratch2 where BRANCHCD='" + frm_mbr + "' AND type='VR' AND VCHDATE " + DateRange + " " + cond + "", "OTP");
        else
        {
            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(upper(COL28))||'~'||trim(upper(COL23)) as otp from scratch2 where BRANCHCD='" + frm_mbr + "' AND type='VR' AND VCHDATE " + DateRange + " " + cond + "", "OTP");
            if (mhd != "0")
            {
                mhd1 = mhd.Split('~')[1];
                mhd = mhd.Split('~')[0];
            }
        }
        if (mhd == txtotp.Text.Trim().ToUpper() || mhd1 == txtotp.Text.Trim().ToUpper())
        {
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, "select a.*,to_char(a.vchdate,'yyyymmdd') as vdd from SCRATCH2 a WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='VR' AND a.VCHDATE " + DateRange + " " + cond + " order by vdd desc, vchnum desc");
            if (frm_cocd == "CCEL" || frm_cocd == "STUD" || frm_cocd == "ANYG" || frm_cocd == "UKB") mhd = "";
            else mhd = fgen.seek_iname(frm_qstr, frm_cocd, "Select trim(upper(COL28)) as otp from scratch2 where BRANCHCD='" + frm_mbr + "' AND type='VM' AND VCHDATE " + DateRange + " " + cond + " ", "OTP");
            if (dt.Rows.Count > 0 && (mhd != txtotp.Text.Trim().ToUpper()))
            {
                txtvname.Text = dt.Rows[0]["COL16"].ToString().Trim();
                txtcomp.Text = dt.Rows[0]["COL15"].ToString().Trim();
                txtloc.Text = dt.Rows[0]["COL17"].ToString().Trim();
                txtdept.Text = dt.Rows[0]["COL19"].ToString().Trim();
                txtdesig.Text = dt.Rows[0]["COL21"].ToString().Trim();
                txtvtype.Text = dt.Rows[0]["COL22"].ToString().Trim();
                txtpurpose.Text = dt.Rows[0]["REMARKS"].ToString().Trim();
                txtrid.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                txtrdt.Text = dt.Rows[0]["vchdate"].ToString().Trim();
                txtapp.Text = dt.Rows[0]["ent_By"].ToString().Trim();
                txtmob.Text = dt.Rows[0]["col23"].ToString().Trim();

                txtloc.Focus();
            }
            else
            {
                txtotp.Text = "";
                txtvname.Text = "";
                txtcomp.Text = "";
                txtloc.Text = "";
                txtdept.Text = "";
                txtdesig.Text = "";
                txtvtype.Text = "";
                txtpurpose.Text = "";
                txtrid.Text = "";
                txtrdt.Text = "";
                txtapp.Text = "";
                txtmob.Text = "";

                if (frm_cocd == "CCEL" || frm_cocd == "STUD") fgen.msg("-", "AMSG", "Wrong Mobile No Entered!!");
                else fgen.msg("-", "AMSG", "OTP Already Used!!");
                //  txtotp.BorderColor = Color.Red;
                txtotp.Focus();
                return;
            }
        }
        else
        {
            txtotp.Text = "";

            txtvname.Text = "";
            txtcomp.Text = "";
            txtloc.Text = "";
            txtdept.Text = "";
            txtdesig.Text = "";
            txtvtype.Text = "";
            txtpurpose.Text = "";
            txtrid.Text = "";
            txtrdt.Text = "";
            txtapp.Text = "";
            txtmob.Text = "";
            if (frm_cocd == "CCEL" || frm_cocd == "STUD" || frm_cocd == "ANYG" || frm_cocd == "UKB") fgen.msg("-", "AMSG", "Wrong Mobile No Entered!!");
            else fgen.msg("-", "AMSG", "Wrong OTP Entered!!");
            // txtotp.BorderColor = Color.Red;
            txtotp.Focus();
            return;
        }
    }
    protected void btntag_ServerClick(object sender, EventArgs e)
    {
        hfbtnmode.Value = "Tag";
        hfedmode.Value = "T";
        sseekfunc();
    }
    public void del_file(string path)
    {
        try
        {
            if (System.IO.File.Exists(fpath)) System.IO.File.Delete(fpath);
        }
        catch { }
    }
    protected void rd_done_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rd_done.SelectedValue == "0")
        {
            btnvisit.Enabled = false; btnemp.Enabled = false;
            txtvname.ReadOnly = false;
            txtcomp.ReadOnly = false; txtloc.ReadOnly = false; txtpurpose.ReadOnly = false;
            txtdept.ReadOnly = false; txttimein.ReadOnly = false; txttimeout.ReadOnly = false;
            txtdesig.ReadOnly = false; txtvdate.ReadOnly = false; txtvtype.ReadOnly = false;
            txtedept.ReadOnly = false; txtedit.ReadOnly = false; txtpre.ReadOnly = false;
            txtapp.ReadOnly = false; txtempid.ReadOnly = false; txtname.ReadOnly = false;
            //==========
            txtcomp.Text = ""; txtloc.Text = ""; txtpurpose.Text = ""; txtdept.Text = ""; txttimein.Text = ""; txttimeout.Text = ""; txtempid.Text = ""; txtmob.Text = "";
            txtdesig.Text = ""; txtvdate.Text = ""; txtvtype.Text = ""; txtedept.Text = ""; txtedit.Text = ""; txtpre.Text = ""; txtapp.Text = ""; txtname.Text = "";
        }
        else
        {
            btnvisit.Enabled = true; btnemp.Enabled = true;
            txtvname.ReadOnly = true;
            txtcomp.ReadOnly = true; txtloc.ReadOnly = true; txtpurpose.ReadOnly = true;
            txtdept.ReadOnly = true; txttimein.ReadOnly = true; txttimeout.ReadOnly = true;
            txtdesig.ReadOnly = true; txtvdate.ReadOnly = true; txtvtype.ReadOnly = true;
            txtedept.ReadOnly = true; txtedit.ReadOnly = true; txtpre.ReadOnly = true;
            txtapp.ReadOnly = true; txtempid.ReadOnly = true; txtname.ReadOnly = true;
            //===============
            txtcomp.Text = ""; txtloc.Text = ""; txtpurpose.Text = ""; txtdept.Text = ""; txttimein.Text = ""; txttimeout.Text = ""; txtempid.Text = ""; txtmob.Text = "";
            txtdesig.Text = ""; txtvdate.Text = ""; txtvtype.Text = ""; txtedept.Text = ""; txtedit.Text = ""; txtpre.Text = ""; txtapp.Text = ""; txtname.Text = "";
        }
    }
}