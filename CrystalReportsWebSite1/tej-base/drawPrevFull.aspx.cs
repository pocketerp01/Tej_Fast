using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class drawPrevFull : System.Web.UI.Page
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
    string frm_tabname = "OM_DRWG_MAKE", frm_tabname1, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    //double double_val2, double_val1;
    fgenDB fgen = new fgenDB();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
        else
        {

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
                prevFile();
            }

            //txtPwd.Attributes.Add("type", "password");
            //txtCpwd.Attributes.Add("type", "password");
        }
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        switch (hffield.Value)
        {
            case "Prev":
                SQuery = "SELECT TRIM(A.AT3) AS FSTR,A.AT1 AS ISSUE_TIME ,a.finish as endtime,A.at4 AS ISSUED_DT,A.VCHNUM ,A.COL1  AS Drawing_ENTRY_NO,A.COL2  AS DRAWING_NAME,A.ENT_BY AS ISSUED_BY ,A.COL4,to_char(to_date(a.finish,'hh24:mi'),'hh24mi') as vdd FROM Multivch A,EVAS B,DRAWREC C WHERE A.TYPE='IV' AND TRIM(A.AT3)=C.BRANCHCD||C.TYPE||TRIM(C.VCHNUM)||tO_cHAR(C.VCHDATE,'DD/MM/YYYY') AND TRIM(A.COL4)=TRIM(B.USERID) AND B.USERNAME='" + frm_uname + "' AND to_char(to_date(a.at4,'dd/mm/yyyy'),'dd/mm/yyyy')=to_char(sysdate,'dd/mm/yyyy') and to_char(to_date(a.finish,'hh24:mi'),'hh24:mi')>to_char(to_date('" + DateTime.Now.ToString("HH:mm") + "','hh24:mi'),'hh24:mi') and to_char(to_date(a.AT1,'hh24:mi'),'hh24:mi')<=to_char(to_date('" + DateTime.Now.ToString("HH:mm") + "','hh24:mi'),'hh24:mi') AND TRIM(A.AT3)='" + col1 + "' order by vdd";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    SQuery = "SELECT FILENAME,FILEPATH FROM FILETABLE WHERE BRANCHCD||tYPE||TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY')||TRIM(FILENAME)='" + dt.Rows[0]["FSTR"].ToString().Trim() + col2 + "'";
                    col1 = dt.Rows[0]["ISSUED_DT"].ToString().Trim();
                    col2 = dt.Rows[0]["ISSUE_TIME"].ToString().Trim();
                    col3 = dt.Rows[0]["endtime"].ToString().Trim();
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        hf1.Value = "";
                        hf2.Value = "";
                        filepath = dt.Rows[0]["filepath"].ToString().Trim();
                        hf1.Value = "MANUAL";
                        hf2.Value = filepath;
                        prevFile(filepath);
                    }
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    { }
    //------------------------------------------------------------------------------------
    void prevFile()
    {
        try
        {
            hf1.Value = "";
            //hf2.Value = "";
            filepath = ""; col3 = ""; col2 = ""; col1 = "";
            dt = new DataTable();
            //SQuery = "SELECT TRIM(A.AT3) AS FSTR,A.AT1 AS ISSUE_TIME ,a.finish as endtime,A.at4 AS ISSUED_DT,A.VCHNUM ,A.COL1  AS Drawing_ENTRY_NO,A.COL2  AS DRAWING_NAME,A.ENT_BY AS ISSUED_BY ,A.COL4 FROM Multivch A,EVAS B,DRAWREC C WHERE A.TYPE='IV' AND TRIM(A.AT3)=C.BRANCHCD||C.TYPE||TRIM(C.VCHNUM)||tO_cHAR(C.VCHDATE,'DD/MM/YYYY') AND TRIM(A.COL4)=TRIM(B.USERID) AND B.USERNAME='" + uname + "' AND A.VCHDATE " + daterange + " and to_date(A.at4,'dd/mm/yyyy')=sysdate and to_date(a.finish,'hh24:mi')<" + System.DateTime.Now.ToString("HH:mm") + " order by a.vchnum";
            SQuery = "SELECT distinct VCHNUM||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR,A.issuetime AS ISSUE_TIME ,a.endtime as endtime,A.starttime,TO_CHAR(A.issuestartdt,'DD/MM/YYYY') AS ISSUED_DT,A.VCHNUM ,A.mrrnum  AS Drawing_ENTRY_NO,A.mrrnum  AS DRAWING_NAME,A.ENT_BY AS ISSUED_BY,to_char(to_date(a.endtime,'hh24:mi'),'hh24mi') as vdd,b.username FROM " + frm_tabname + " a , evas b where trim(a.usercode)=trim(b.userid) and  a.branchcd='" + frm_mbr + "' and a.type='IV' and a.vchnum<>'000000' and a.vchdate " + DateRange + " order by a.vchnum desc ";
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count > 0)
            {
                col1 = dt.Rows[0]["ISSUED_DT"].ToString().Trim();
                col2 = dt.Rows[0]["starttime"].ToString().Trim();
                col3 = dt.Rows[0]["ENDTIME"].ToString().Trim();
                SQuery = "SELECT FILENAME,FILEPATH FROM WB_DRAWREC WHERE TRIM(VCHNUM)='" + dt.Rows[0]["DRAWING_ENTRY_NO"].ToString().Trim() + "'";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    filepath = dt2.Rows[0]["filepath"].ToString().Trim();
                }
            }
            //filepath = @"Uploads\MMC_000215_4341-MC.pdf";
            if (col3.Length > 0 && col2.Length > 0 && col1.Length > 0)
            {
                DateTime dtime1 = Convert.ToDateTime(col1 + " " + col2);
                DateTime dtime2 = Convert.ToDateTime(col1 + " " + col3);
                TimeSpan tsp = DateTime.Now - dtime1;
                TimeSpan tsp1 = dtime2 - DateTime.Now;
                if (Convert.ToInt32(tsp.Minutes) >= 0 && Convert.ToInt32(tsp1.Minutes) >= 0)
                {
                    int i = filepath.ToUpper().IndexOf("UPLOAD");
                    filepath = "../tej-base/" + filepath.Substring(i, filepath.Length - i);
                    filepath = filepath + "#toolbar=0&navpanes=1&scrollbar=1&zoom=80";
                    Iframe1.Attributes.Add("src", filepath);
                    Iframe1.Visible = true;
                }
            }
            else Iframe1.Visible = false;
        }
        catch { }
    }
    void make_qry_4_popup()
    {
        btnval = hffield.Value;
        switch (btnval)
        {
            case "PREV":
                SQuery = "SELECT TRIM(A.AT3) AS FSTR,D.FILENAME AS FILENAME,A.AT1 AS ISSUE_TIME ,A.at4 AS ISSUED_DT,A.VCHNUM ,A.COL1  AS Drawing_ENTRY_NO,A.COL2  AS DRAWING_NAME,A.ENT_BY AS ISSUED_BY ,A.COL4 FROM Multivch A,EVAS B,DRAWREC C,FILETABLE D WHERE A.TYPE='IV' AND TRIM(A.AT3)=C.BRANCHCD||C.TYPE||TRIM(C.VCHNUM)||tO_cHAR(C.VCHDATE,'DD/MM/YYYY') AND TRIM(A.AT3)=D.BRANCHCD||D.TYPE||TRIM(D.VCHNUM)||tO_cHAR(D.VCHDATE,'DD/MM/YYYY') AND TRIM(A.COL4)=TRIM(B.USERID) AND B.USERNAME='" + frm_uname + "' AND to_char(to_date(a.at4,'dd/mm/yyyy'),'dd/mm/yyyy')=to_char(sysdate,'dd/mm/yyyy') and to_char(to_date(a.finish,'hh24:mi'),'hh24:mi')>to_char(to_date('" + DateTime.Now.ToString("HH:mm") + "','hh24:mi'),'hh24:mi') and to_char(to_date(a.AT1,'hh24:mi'),'hh24:mi')<=to_char(to_date('" + DateTime.Now.ToString("HH:mm") + "','hh24:mi'),'hh24:mi')";
                SQuery = "SELECT * FROM (SELECT (case when to_char(to_date(a.finish,'hh24:mi'),'hh24:mi')>to_char(to_date('" + DateTime.Now.ToString("HH:mm") + "','hh24:mi'),'hh24:mi') then 'TRUE' else 'FALSE' END) as time1, (case when to_char(to_date(a.AT1,'hh24:mi'),'hh24:mi')<=to_char(to_date('" + DateTime.Now.ToString("HH:mm") + "','hh24:mi'),'hh24:mi') then 'TRUE' ELSE 'FALSE' END) as time2, TRIM(A.AT3) AS FSTR,D.FILENAME AS FILENAME,A.AT1 AS ISSUE_TIME ,A.at4 AS ISSUED_DT,A.VCHNUM ,A.COL1  AS Drawing_ENTRY_NO,A.COL2  AS DRAWING_NAME,A.ENT_BY AS ISSUED_BY ,A.COL4 FROM Multivch A,EVAS B,DRAWREC C,FILETABLE D WHERE A.TYPE='IV' AND TRIM(A.AT3)=C.BRANCHCD||C.TYPE||TRIM(C.VCHNUM)||tO_cHAR(C.VCHDATE,'DD/MM/YYYY') AND TRIM(A.AT3)=D.BRANCHCD||D.TYPE||TRIM(D.VCHNUM)||tO_cHAR(D.VCHDATE,'DD/MM/YYYY') AND TRIM(A.COL4)=TRIM(B.USERID) AND B.USERNAME='" + frm_uname + "' AND to_char(to_date(a.at4,'dd/mm/yyyy'),'dd/mm/yyyy')=to_char(sysdate,'dd/mm/yyyy') ) WHERE TIME1='TRUE' AND TIME2='TRUE' ";
                break;
        }
        if (SQuery.Length == 0) { }
        else
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
    protected void btnpopup_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
        //hffield.Value = "PREV";
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("PREV", frm_qstr);
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        Panel1.Update();
        Label1.Text = "Drawing refreshed at: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        prevFile();
    }

    void prevFile(string FilePath)
    {
        try
        {
            filepath = FilePath;
            if (col3.Length > 0 && col2.Length > 0 && col1.Length > 0)
            {
                DateTime dtime1 = Convert.ToDateTime(col1 + " " + col2);
                DateTime dtime2 = Convert.ToDateTime(col1 + " " + col3);
                TimeSpan tsp = DateTime.Now - dtime1;
                TimeSpan tsp1 = dtime2 - DateTime.Now;
                if (Convert.ToInt32(tsp.Minutes) > 0 && Convert.ToInt32(tsp1.Minutes) >= 0)
                {
                    int i = filepath.IndexOf(@"Uploads\");
                    filepath = filepath.Substring(i, filepath.Length - i);
                    filepath = filepath + "#toolbar=0&navpanes=1&scrollbar=1&zoom=80";
                    Iframe1.Attributes.Add("src", filepath);
                    Iframe1.Visible = true;
                }
            }
            else Iframe1.Visible = false;
        }
        catch { }
    }


    public string filepath { get; set; }
}
