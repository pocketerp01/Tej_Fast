using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Drawing;
using System.Timers;

public partial class drawPrevDash : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt2, dt3, dt4;
    DataRow oporow; DataSet oDS, oDs1;
    string imgpath = "", filepath = "";
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
        //if (Request.UrlReferrer == null) Response.Redirect("login.aspx");
        //else
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
            //doc_addl.Value = "0";

            fgen.DisableForm(this.Controls);
            enablectrl();
            getColHeading();

            prevFile();
        }

        setColHeadings();
        set_Val();

        //txtPwd.Attributes.Add("type", "password");
        //txtCpwd.Attributes.Add("type", "password");

    }

    public void make_qry_4_popup()
    {
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        switch (btnval)
        {
            case "PREV":
                SQuery = "SELECT distinct VCHNUM||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR,A.issuetime AS ISSUE_TIME ,A.issuestartdt AS ISSUED_DT,A.VCHNUM ,A.mrrnum  AS Drawing_ENTRY_NO,A.mrrnum  AS DRAWING_NAME,A.ENT_BY AS ISSUED_BY ,A.userocde FROM " + frm_tabname + " A,EVAS B,DRAWREC C WHERE A.TYPE='IV' AND TRIM(A.AT3)=C.BRANCHCD||C.TYPE||TRIM(C.VCHNUM)||tO_cHAR(C.VCHDATE,'DD/MM/YYYY') AND TRIM(A.usercode)=TRIM(B.USERID) AND B.USERNAME='PUSHKAR' AND A.VCHDATE " + DateRange + "";
                break;
            case "Edit":
                SQuery = "SELECT distinct A.BRANCHCD||A.TYPE||tRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS fstr,A.Issuetime AS ISSUE_TIME ,a.endtime as endtime,to_char(A.Issuestartdt,'DD/MM/YYYY') AS ISSUED_DT,A.VCHNUM ,A.mrrnum  AS Drawing_ENTRY_NO,A.mrrnum  AS DRAWING_NAME,A.ENT_BY AS ISSUED_BY,to_char(to_date(a.endtime,'hh24:mi'),'hh24mi') as vdd,b.username,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.edt_by,to_char(a.edt_dt,'dd/mm/yyyy') as edt_dt,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt FROM " + frm_tabname + " A,EVAS B WHERE a.branchcd='" + frm_mbr + "' and A.TYPE='IV' and a.vchdate " + DateRange + "  and TRIM(A.usercode)=TRIM(B.USERID)  order by vdd";
                break;
        }
        if (SQuery.Length == 0) { }
        else
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where branchcd='" + frm_mbr + "' and type='ID' and a.vchnum||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

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
                    SQuery = "SELECT distinct A.BRANCHCD||A.TYPE||tRIM(A.VCHNUM)||TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS fstr,A.Issuetime AS ISSUE_TIME ,a.endtime as endtime,to_char(A.Issuestartdt,'DD/MM/YYYY') AS ISSUED_DT,A.VCHNUM ,A.mrrnum  AS Drawing_ENTRY_NO,A.mrrnum  AS DRAWING_NAME,A.ENT_BY AS ISSUED_BY,to_char(to_date(a.endtime,'hh24:mi'),'hh24mi') as vdd,b.username,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.edt_by,to_char(a.edt_dt,'dd/mm/yyyy') as edt_dt,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt FROM " + frm_tabname + " A,EVAS B WHERE a.branchcd='" + frm_mbr + "' and A.TYPE='IV' AND  a.branchcd||a.type||a.VCHNUM||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + col1 + "' and a.vchdate " + DateRange + "  and TRIM(A.usercode)=TRIM(B.USERID)  order by vdd";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        prevFile();
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
            }
        }
    }

    public void clearctrl()
    {
        hffield.Value = "";
        edmode.Value = "";
    }

    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        set_Val();

        if (chk_rights == "Y")
        {
            hffield.Value = "Edit";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Drawing No", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");



    }
    void prevFile()
    {
        try
        {
            filepath = ""; col3 = ""; col2 = ""; col1 = "";
            dt = new DataTable();
            SQuery = "select * from (SELECT a.branchcd||a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') AS FSTR,A.issuetime AS ISSUE_TIME ,a.endtime,A.issuestartdt AS ISSUED_DT,A.VCHNUM ,A.mrrnum  AS Drawing_ENTRY_NO,A.mrrnum  AS DRAWING_NAME,A.ENT_BY AS ISSUED_BY ,A.usercode,b.username,to_char(to_date(a.endtime,'hh24:mi'),'hh24mi') as vdd FROM om_drwg_make A,EVAS B WHERE a.branchcd='" + frm_mbr + "' and A.TYPE='IV' AND TRIM(A.usercode)=TRIM(B.USERID) and a.vchdate " + DateRange + ") where ROWNUM<7 ORDER BY vdd desc,VCHNUM desc ";
            //SQuery = "SELECT * FROM (SELECT b.username,TRIM(A.AT3) AS FSTR,A.AT1 AS ISSUE_TIME ,a.finish as endtime,A.at4 AS ISSUED_DT,A.VCHNUM ,A.COL1  AS Drawing_ENTRY_NO,A.COL2  AS DRAWING_NAME,A.ENT_BY AS ISSUED_BY ,A.COL4,to_char(to_date(a.finish,'hh24:mi'),'hh24mi') as vdd FROM "+ frm_tabname +" A,EVAS B,DRAWREC C WHERE A.TYPE='IV' AND TRIM(A.AT3)=C.BRANCHCD||C.TYPE||TRIM(C.VCHNUM)||tO_cHAR(C.VCHDATE,'DD/MM/YYYY') AND TRIM(A.COL4)=TRIM(B.USERID) and to_char(to_date(a.at4,'dd/mm/yyyy'),'dd/mm/yyyy')=to_char(sysdate,'dd/mm/yyyy') /*and to_char(to_date(a.finish,'hh24:mi'),'hh24:mi')>to_char(to_date('" + DateTime.Now.ToString("HH:mm") + "','hh24:mi'),'hh24:mi') and to_char(to_date(a.AT1,'hh24:mi'),'hh24:mi')<=to_char(to_date('" + DateTime.Now.ToString("HH:mm") + "','hh24:mi'),'hh24:mi')*/ order by vdd) WHERE ROWNUM<7";         
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add(new DataColumn("Filepath", typeof(string)));
                dt.Columns.Add(new DataColumn("hfaddr", typeof(string)));
                foreach (DataRow dr in dt.Rows)
                {
                    SQuery = "SELECT FILENAME,FILEPATH FROM FILETABLE WHERE BRANCHCD||tYPE||TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY')='" + dr["FSTR"].ToString().Trim() + "'";
                    SQuery = "SELECT FILENAME,FILEPATH FROM WB_DRAWREC WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='DE' AND TRIM(VCHNUM)='" + dr["DRAWING_ENTRY_NO"].ToString().Trim() + "'";
                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "filepath");
                    if (col1 != "0")
                    {
                        try
                        {
                            filepath = col1;
                            int i = filepath.ToUpper().IndexOf(@"UPLOAD");
                            dr["hfaddr"] = filepath.Substring(i, filepath.Length - i);
                            if (i > 0) filepath = "../tej-base/" + filepath.Substring(i, filepath.Length - i);
                            filepath = filepath + "#toolbar=0&navpanes=1&scrollbar=1&zoom=50";
                            dr["filepath"] = filepath;

                        }
                        catch { }
                    }
                    else dr["filepath"] = "";
                }
                dtList1.DataSource = dt;
                dtList1.DataBind();
            }
        }
        catch { }
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        Panel1.Update();
        prevFile();
        Panel1.Update();
    }
    protected void dtList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = dtList1.SelectedIndex;
        HiddenField dtHf1 = (HiddenField)dtList1.Items[index].FindControl("hfaddr");
        if (dtHf1.Value.Length > 0)
        {
            string filePath = dtHf1.Value.ToString().Trim();
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "var Mleft = (screen.width/2)-(1000/2);var Mtop = (screen.height/2)-(700/2);window.open( '" + url + "', null, 'height=700,width=1000,status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no,top=\'+Mtop+\', left=\'+Mleft+\'' );", true);
            //ScriptManager.RegisterStartupScript(this, typeof(string), "OPEN_WINDOW", "window.open( '" + url + "', null, 'status=yes,toolbar=no,scrollbars=yes,menubar=no,location=no');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
        }
    }
    protected void dtList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //e.Item.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(dtList1, "Select$" + e.Item.ItemIndex);
            e.Item.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(e.Item.Controls[1], string.Empty));
        }
    }

    public void set_Val()
    {
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "om_drwg_make";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        typePopup = "N";
    }

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

        ViewState["sg1"] = null;

    }

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

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "*******":

                break;
        }
        if (Prg_Id == "*******")
        {

        }
        lblheader.Text = "Drawing Preview Dashboard";
        //if (frm_cocd == "MSES") divCan.Visible = false;
        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }

    public void disablectrl()
    {
        btnedit.Disabled = true;
        btnhideF.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        //btnCamera.Disabled = false;
    }

    public void enablectrl()
    {
        btnedit.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true;

        //btnCamera.Disabled = true;



    }
}



