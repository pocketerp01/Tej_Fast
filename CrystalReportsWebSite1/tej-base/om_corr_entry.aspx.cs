using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_corr_entry : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, Squery2, col1, col2, col3, col4, col7, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5; DataRow oporow6; DataSet oDS6;
    int i = 0, z = 0; string WIPStDt;
    DateTime date1, date2; TimeSpan Diff; double Min;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, ind_Ptype;
    double InputQty = 0, OutputQty = 0, RejectionQty = 0, DowTimeQty = 0, QtyReq = 0, Qty = 0, rm_Val;
    string frm_tabname1, frm_tabname2, frm_tabname6, mchcode, msg, xprdrange1, xprdrange; double GridInputTot = 0, GridOutputTot = 0;
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
                    // variables declare to call 2 tables
                    ind_Ptype = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL8", "");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL9", "");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL10", "");
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                doc_addl.Value = "-";
                string chk_opt = "";
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0092'", "fstr");
                if (chk_opt == "Y")
                {
                    doc_addl.Value = "Y";
                }


                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
        }
    }
    //------------------------------------------------------------------------------------
    void getColHeading()
    {
        dtCol = new DataTable();
        dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
        if (dtCol == null || dtCol.Rows.Count <= 0)
        {
            dtCol = fgen.getdata(frm_qstr, frm_cocd, fgenMV.Fn_Get_Mvar(frm_qstr, "U_SYS_COM_QRY") + " WHERE UPPER(TRIM(FRM_NAME))='" + frm_formID + "'");
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
        #region hide hidden columns
        sg1.Columns[0].Visible = false;
        sg1.Columns[1].Visible = false;
        sg1.Columns[2].Visible = false;
        sg1.Columns[3].Visible = false;
        sg1.Columns[4].Visible = false;
        sg1.Columns[5].Visible = false;
        sg1.Columns[6].Visible = false;
        sg1.Columns[7].Visible = false;
        sg1.Columns[8].Visible = false;
        sg1.Columns[9].Visible = false;
        #endregion
        if (dtCol == null) return;
        if (sg1.Rows.Count <= 0) return;
        for (int sR = 0; sR < sg1.Columns.Count; sR++)
        {
            string orig_name;
            double tb_Colm;
            tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
            orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();

            //for (int K = 0; K < sg1.Rows.Count; K++)
            //{
            //    if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

            //    ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");

            //    ((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("readonly", "readonly");
            //    ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("readonly", "readonly");
            //    ((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("readonly", "readonly");


            //}
            orig_name = orig_name.ToUpper();
            //if (sg1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_NAME"))
            if (sR == tb_Colm)
            {
                // hidding column
                if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
                {
                    sg1.Columns[sR].Visible = false;
                }
                // Setting Heading Name
                sg1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
                // Setting Col Width
                string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
                if (fgen.make_double(mcol_width) > 0)
                {
                    sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
                    sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                }
            }
        }

        //txtlbl8.Attributes.Add("readonly", "readonly");
        //txtlbl9.Attributes.Add("readonly", "readonly");
        // to hide and show to tab panel
        //tab5.Visible = false;
        //tab4.Visible = false;
        //tab3.Visible = false;
        //tab2.Visible = false;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false;
        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();
        sg4_add_blankrows();

        btnlbl4.Enabled = false;
        btnlbl7.Enabled = false;

        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
        sg4.DataSource = sg4_dt; sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlbl4.Enabled = true;
        btnlbl7.Enabled = true;
        btnprint.Disabled = true; btnlist.Disabled = true;
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
        doc_nf.Value = "VCHNUM";
        doc_df.Value = "VCHDATE";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "PROD_SHEET";
        frm_tabname1 = "COSTESTIMATE";
        frm_tabname2 = "INSPVCH";
        frm_tabname6 = "IVOUCHER";
        frm_vty = "88";

        switch (frm_formID)
        {
            case "F39102":
                lblheader.Text = "Production Entry";
                frm_tabname = "PROD_SHEETK";
                frm_tabname1 = "COSTESTIMATEK";
                frm_tabname2 = "INSPVCHK";
                frm_vty = "86";

                lbl5.Text = "RR_Code";
                lbl6.Text = "RR_Qty";
                txtCoreCal.Visible = false;
                lbl8.Style.Add("display", "none");
                txtlbl8.Style.Add("display", "none");
                lbl9.Style.Add("display", "none");
                txtlbl9.Style.Add("display", "none");
                break;
            case "F40107":
                lblheader.Text = "Label Production";
                break;
            default:
                lblheader.Text = "Corrugation Production";
                break;
        }
        // to put by icon id
        if (ind_Ptype == "12" || ind_Ptype == "13")
        {
            trCorr1.Visible = false;
            trCorr2.Visible = false;

            trPoly1.Visible = true;
            trPoly2.Visible = true;

            frm_vty = "86";
            frm_tabname = "PROD_SHEETK";
            frm_tabname1 = "COSTESTIMATEK";
            frm_tabname2 = "INSPVCHK";
        }
        else
        {
            trCorr1.Visible = true;
            trCorr2.Visible = true;

            trPoly1.Visible = false;
            trPoly2.Visible = false;
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL8", frm_tabname1);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL9", frm_tabname2);
        typePopup = "N";
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        btnval = hffield.Value;
        switch (btnval)
        {
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
            case "Mach":
                SQuery = "select distinct mchcode as fstr,ename as machine_name ,mchcode  from " + frm_tabname + " where branchcd='" + frm_mbr + "' and TYPE IN('86','88') and vchdate " + PrdRange + " order by ename";
                break;
            case "Section":
                SQuery = "";
                break;
            case "Empl":
                SQuery = "select distinct trim(opr_dtl) as fstr,trim(opr_dtl) asopr_name ,trim(opr_dtl) as names from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type in ('88','86') and vchdate " + PrdRange + " and length(trim(opr_dtl))>1";
                break;
            case "ChkStage":
                SQuery = "select '' as fstr,trim(b.name) as stagename,trim(a.stagec) as stagec,a.srno,a.ent_by from itwstage a,type b where trim(a.stagec)=trim(b.type1) and a.branchcd='" + frm_mbr + "' and a.type='10' and trim(a.icode)='" + txtlbl47.Text.Trim() + "' and b.id='K' order by srno";
                break;

            case "TACODE":
                if (doc_addl.Value == "Y")
                {
                    SQuery = "select trim(Acref) as fstr, NAME,trim(Acref) as Code from typegrp where branchcd='" + frm_mbr + "' and id='WI' and substr(acref,1,1)='6' order by trim(Acref)";
                }
                else
                {
                    SQuery = "select type1 as fstr, NAME,type1 from type where id='1' and substr(type1,1,1)='6' order by type1";
                }


                break;
            case "TICODE":
                SQuery = "select type1 as fstr,NAME,type1 from type where id='D' and substr(type1,1,1)='1' order by name";
                break;
            case "MACHINE":
                SQuery = "select Trim(acode)||'/'||srno AS FSTR, mchname as Machine_Name,trim(acode)||'/'||srno as Machine_Code,mchcode,spec2 as Tcode from pmaint where branchcd='" + frm_mbr + "' and type='10' /*and acode='18' and trim(nvl(spec2,'-'))!='-'*/ order by acode,srno";
                break;
            case "TEAMLEAD":
                SQuery = "Select e.EMPCODE as FSTR, e.NAME AS Emp_Name,e.EMPCODE as Emp_code,E.FHNAME as Father_Name,e.deptt_text,E.desg_TExt FROM EMPMAS e where e.branchcd='" + frm_mbr + "' and length(Trim(e.leaving_Dt))<5 order by e.name";
                break;
            case "PLANNO":
                SQuery = "select trim(s.job_no)||to_char(s.job_dt,'dd/mm/yyyy') as fstr, i.iname AS Item_Name,s.icode as Item_Code,nvl(s.PLAN,0) as Planned,nvl(r.PROD,0) as Produced,I.Cpartno,to_char(s.vchdate,'dd/mm/yyyy') as vchdate,s.vchnum,s.job_no,to_Char(s.job_Dt,'dd/mm/yyyy') as job_dt,S.ENAME AS Process,to_char(s.vchdate,'yyyymmdd') as vdd from item i,(select a.job_no as vchnum,to_DaTE(a.job_Dt,'dd/mm/yyyy') as vchdate,a.icode,SUM(a.iqtyout) as PLAN,a.job_no,to_DatE(a.job_Dt,'dd/mm/yyyy') as job_dt,'-' as ENAME from " + frm_tabname + " a where branchcd='" + frm_mbr + "' and a.type='90' and A.VCHDATE " + DateRange + "  GROUP BY a.job_no,to_DaTE(a.job_Dt,'dd/mm/yyyy'),a.icode,a.job_no,to_DatE(a.job_Dt,'dd/mm/yyyy')) s left outer join (select JOB_NO ,icode,'-' as ENAME,sum(iqtyin) as PROD from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and VCHDATE " + DateRange + "  and acode='" + txtlbl4.Text.Trim() + "' group by JOB_NO ,job_dt,ENAME,icode) r on trim(s.JOB_NO)||trim(s.icode)=trim(r.JOB_NO )||trim(r.icode) where trim(i.icode)=trim(s.icode) and nvl(s.PLAN,0)-nvl(r.PROD,0) > 0 order by vdd desc ,vchnum desc";
                if (ind_Ptype == "12" || ind_Ptype == "13")
                    SQuery = " SELECT A.VCHNUM||A.VCHDATE AS FSTR,B.INAME AS ITEM_NAME,A.ICODE AS ITEM_CODE,SUM(A.PLAN) AS PLANNED,SUM(A.PROD) AS PROD,B.CPARTNO,A.VCHDATE AS JOBDt,A.VCHNUM AS JOB_NO FROM (SELECT TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,QTY AS PLAN,0 AS PROD FROM COSTESTIMATE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='30' AND VCHDATE " + DateRange + " AND SRNO=0 UNION ALL SELECT TRIM(JOB_NO),TRIM(JOB_DT),TRIM(ICODE) AS ICODE,0 AS PLAN,IQTYIN AS PROD FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " AND trim(ACODE)='" + txtlbl4.Text.Trim() + "' ) A, ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) GROUP BY A.VCHNUM||A.VCHDATE,B.INAME,A.ICODE,B.CPARTNO,A.VCHDATE,A.VCHNUM HAVING (SUM(A.PLAN)-SUM(A.PROD))>0 ORDER BY A.VCHNUM DESC ";
                if (ind_Ptype == "01")
                    SQuery = " SELECT A.VCHNUM||A.VCHDATE AS FSTR,B.INAME AS ITEM_NAME,A.ICODE AS ITEM_CODE,SUM(A.PLAN) AS PLANNED,SUM(A.PROD) AS PROD,B.CPARTNO,A.VCHDATE AS JOBDt,A.VCHNUM AS JOB_NO FROM (SELECT TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,a1 AS PLAN,0 AS PROD FROM prod_sheet WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='11' AND VCHDATE " + DateRange + " AND trim(ACODE)='" + txtlbl4.Text.Trim() + "' UNION ALL SELECT TRIM(JOB_NO),TRIM(JOB_DT),TRIM(ICODE) AS ICODE,0 AS PLAN,IQTYIN AS PROD FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " AND trim(ACODE)='" + txtlbl4.Text.Trim() + "' ) A, ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) GROUP BY A.VCHNUM||A.VCHDATE,B.INAME,A.ICODE,B.CPARTNO,A.VCHDATE,A.VCHNUM HAVING (SUM(A.PLAN)-SUM(A.PROD))>0 ORDER BY B.INAME,A.VCHNUM,A.VCHDATE ";
                break;
            case "JOBNO":
                SQuery = "select type1 as fstr,NAME,type1 from type where id='D' and substr(type1,1,1)='1' order by name";
                break;
            case "OPR1":
                SQuery = "Select trim(e.EMPCODE) as fstr,trim(e.NAME) AS Emp_Name,e.EMPCODE as Emp_code,E.FHNAME as Father_Name,e.deptt_text,E.desg_TExt FROM EMPMAS e where e.branchcd='" + frm_mbr + "' and length(Trim(e.leaving_Dt))<5 order by e.name";
                break;
            case "OPR2":
                SQuery = "Select trim(e.EMPCODE) as fstr,trim(e.NAME) AS Emp_Name,e.EMPCODE as Emp_code,E.FHNAME as Father_Name,e.deptt_text,E.desg_TExt FROM EMPMAS e where e.branchcd='" + frm_mbr + "' and length(Trim(e.leaving_Dt))<5 order by e.name";
                break;
            case "OPR3":
                SQuery = "Select trim(e.EMPCODE) as fstr,trim(e.NAME) AS Emp_Name,e.EMPCODE as Emp_code,E.FHNAME as Father_Name,e.deptt_text,E.desg_TExt FROM EMPMAS e where e.branchcd='" + frm_mbr + "' and length(Trim(e.leaving_Dt))<5 order by e.name";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[20].Text.Trim().Length > 2)
                    {
                        if (col1.Length > 0) col1 = col1 + "," + "'" + gr.Cells[20].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[20].Text.Trim() + "'";
                    }
                }
                if (col1.Length > 0)
                {
                    col1 = " and TRIM(kclreelno) not in (" + col1 + ")";
                }
                else
                {
                    col1 = "";
                }
                WIPStDt = fgen.seek_iname(frm_qstr, frm_mbr, "select trim(wipstdt) as wipstdt from type where id='B' and type1='" + frm_mbr + "'", "wipstdt");
                if (WIPStDt.Length == 1)
                {
                    WIPStDt = fgen.seek_iname(frm_qstr, frm_mbr, "select params from controls where id='R10'", "params");
                }

                SQuery = "select trim(a.icode)||trim(a.kclreelno) as fstr, B.iname as Item_Name,trim(a.icode) as ERP_Code,sum(a.iqtyin)-sum(a.iqtyout) as Bal,b.Unit,trim(a.kclreelno) as BATCH_NO,b.Cpartno as Part_no,b.ciname,max(A.coreelno) as coreel,sum(a.iqtyin) as Rcvd,sum(a.iqtyout) as Used from (SELECT ICODE,reelwout AS IQTYIN,REELWIN AS IQTYOUT,kclreelno,coreelno FROM reelvch WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('31','32','11') AND trim(SUBSTR(ICODE,1,2)) IN ('07','08','09','80','81') and NOT (TYPE='32' AND LENGTH(TRIM(ACODE))>=6) AND vchdate between to_Date('" + WIPStDt + "','dd/mm/yyyy') and to_date('" + txtvchdate.Text + "','dd/mm/yyyy')  AND TRIM(NVL(RINSP_BY,'-'))!='REELOP*' UNION ALL SELECT ICODE,IQTYIN,0 AS IQTYOUT,wolink,col1t as coreelno FROM wipstk WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='50' AND trim(SUBSTR(ICODE,1,2)) IN ('07','08','09','80','81') and vchdate>=to_Date('" + WIPStDt + "','dd/mm/yyyy') and trim(Stage)='" + txtlbl4.Text + "' UNION ALL SELECT ICODE,iqtyout as IQTYIN,iqtyin AS IQTYOUT,'-' as wolink,'-' as coreelno FROM ivoucher WHERE BRANCHCD='" + frm_mbr + "' AND substr(TYPE,1,2) in ('30','31','11') AND trim(SUBSTR(ICODE,1,2)) not IN ('07','08','09','80','81') and vchdate>=to_Date('" + WIPStDt + "','dd/mm/yyyy') and trim(Stage)='" + txtlbl4.Text + "' and store='Y' union all SELECT ICODE,0 AS IQTYIN,itate as IQTYOUT,col6,null as coreelno FROM " + frm_tabname1 + " WHERE branchcd='" + frm_mbr + "' and type='25' and vchdate  between to_Date('" + WIPStDt + "','dd/mm/yyyy') and to_date('" + txtvchdate.Text + "','dd/mm/yyyy') and trim(col21)='" + txtlbl4.Text + "') a,item b where trim(a.icode)=trim(B.icode) " + col1 + " group by b.iname,b.cpartno,b.ciname,b.unit,trim(a.icode),trim(a.kclreelno) having sum(a.iqtyin)-sum(a.iqtyout)>0 order by B.iname";

                //SQuery = "select trim(a.icode)||trim(a.kclreelno) as fstr, B.iname as Item_name,trim(a.icode) as ERP_Code,sum(a.iqtyin)-sum(a.iqtyout) as balance,b.Unit,trim(a.kclreelno) as ReelNO,b.Cpartno as Part_no,b.ciname,max(A.coreelno) as coreel,sum(a.iqtyin) as Rcvd,sum(a.iqtyout) as Used from (SELECT ICODE,reelwout AS IQTYIN,REELWIN AS IQTYOUT,kclreelno,coreelno FROM reelvch WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('31','32','11') AND trim(SUBSTR(ICODE,1,2)) IN ('07','80','81') and vchdate " + DateRange + " AND TRIM(NVL(RINSP_BY,'-'))!='REELOP*' AND 1=1  UNION ALL SELECT ICODE,IQTYIN,0 AS IQTYOUT,wolink,col1t as coreelno FROM wipstk WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('50') AND trim(SUBSTR(ICODE,1,2)) IN ('07','80','81') and vchdate>=to_Date('31/12/2016','dd/mm/yyyy')  union all SELECT ICODE,0 AS IQTYIN,itate as IQTYOUT,col6,null as coreelno FROM costestimate WHERE branchcd='" + frm_mbr + "' and type='25' and vchdate " + DateRange + "   ) a,item b where trim(a.icode)=trim(B.icode) group by b.iname,b.cpartno,b.ciname,b.unit,trim(a.icode),trim(a.kclreelno) having sum(a.iqtyin)-sum(a.iqtyout)>0  " + col1 + " order by B.iname"; //by yogita
                if (txtlbl4.Text != "61")
                    SQuery = " SELECT trim(a.icode)||trim(a.BTCHNO) AS FSTR,B.INAME AS Item_Name,A.ICODE AS ITEM_CODE,SUM(A.iqtyin)-SUM(A.iqtyout) AS Bal,b.unit,a.BTCHNO as BATCH_NO,B.CPARTNO,b.ciname,'-' as coreelno,sum(a.iqtyin) as rcvd,sum(a.iqtyout) as used,'-' AS coreel,A.ICODE AS ERP_Code,A.BTCHNO AS reelno FROM (SELECT TRIM(ICODE) AS ICODE,IQTYIN,0 AS IQTYOUT,trim(btchno) as btchno FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='3A' AND VCHDATE " + DateRange + " AND TRIM(aCODe)='" + txtlbl4.Text + "' UNION ALL SELECT TRIM(ICODE) AS ICODE,0 AS PLAN,IQTYIN AS PROD,trim(remarks2) as btchno FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " AND trim(ACODE)='" + txtlbl4.Text.Trim() + "' AND TRIM(JOB_NO)||TRIM(JOB_DT)='" + txtlbl43.Text + txtlbl14.Text + "' ) A , ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) GROUP BY B.INAME,A.ICODE,B.CPARTNO,trim(a.icode)||trim(a.BTCHNO),A.BTCHNO,B.CINAME,B.UNIT HAVING (SUM(A.iqtyin)-SUM(A.iqtyout))>0 ";
                if (ind_Ptype == "01")
                {
                    string R10 = "";
                    R10 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT WIPSTDT FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "WIPSTDT");
                    if (R10 == "0") R10 = "01/01/1950";
                    SQuery = "SELECT TRIM(A.ICODE)||TRIM(A.BTCHNO) AS FSTR,B.INAME AS Item_Name,B.CPARTNO AS PARTNO,B.UNIT,A.BTCHNO AS BATCH_NO,sum(A.QTY) AS BAL,a.icode as ERP_Code FROM (select ICODE,BTCHNO,SUM(QTY) AS QTY FROM (SELECT TRIM(ICODE) AS ICODE,TRIM(BTCHNO) AS BTCHNO,(IQTYOUT) AS QTY FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND (SUBSTR(TYPE,1,1)='3' or SUBSTR(TYPE,1,1)='1') AND TYPE!='39' AND VCHDATE>=TO_dATE('" + R10 + "','DD/MM/YYYY') AND STORE='Y' UNION ALL SELECT TRIM(ICODE) AS ICODE,TRIM(WOLINK) AS BTCHNO,(IQTYIN) AS QTY FROM WIPSTK WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='50' AND VCHDATE>=TO_dATE('" + R10 + "','DD/MM/YYYY') UNION ALL SELECT TRIM(ICODE) AS ICODE,TRIM(BTCHNO) AS BTCHNO,(-1*iqtyout) AS QTY FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='39' AND VCHDATE>=TO_dATE('" + R10 + "','DD/MM/YYYY') AND STORE='W') GROUP BY ICODE,BTCHNO ) A,ITEM B,ITEMOSP C WHERE TRIM(a.ICODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)=TRIM(C.IBCODe) AND TRIM(C.ICODE)='" + txtlbl47.Text.Trim() + "' group by a.icode,b.iname,b.cpartno,b.unit,a.btchno having sum(a.qty)>0  ORDER BY B.INAME";
                }
                if (ind_Ptype == "12")
                {
                    SQuery = "SELECT TRIM(A.ICODE)||'10001' AS FSTR,a.INAME AS Item_Name,a.CPARTNO AS PARTNO,a.UNIT,'10001' AS BATCH_NO,'100' AS BAL,a.icode as ERP_Code FROM item a where length(Trim(a.icode))>4 order by a.icode";
                    if (txtlbl4.Text != "60")
                        SQuery = " SELECT trim(a.icode)||trim(a.BTCHNO) AS FSTR,B.INAME AS Item_Name,A.ICODE AS ITEM_CODE,SUM(A.iqtyin)-SUM(A.iqtyout) AS Bal,b.unit,a.BTCHNO as BATCH_NO,B.CPARTNO,b.ciname,'-' as coreelno,sum(a.iqtyin) as rcvd,sum(a.iqtyout) as used,'-' AS coreel,A.ICODE AS ERP_Code,A.BTCHNO AS reelno FROM (SELECT TRIM(ICODE) AS ICODE,IQTYIN,0 AS IQTYOUT,trim(btchno) as btchno FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='3A' AND VCHDATE " + DateRange + " AND TRIM(aCODe)='" + txtlbl4.Text + "' UNION ALL SELECT TRIM(ICODE) AS ICODE,0 AS PLAN,IQTYIN AS PROD,trim(remarks2) as btchno FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " AND trim(ACODE)='" + txtlbl4.Text.Trim() + "' AND TRIM(JOB_NO)||TRIM(JOB_DT)='" + txtlbl43.Text + txtlbl14.Text + "' ) A , ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) GROUP BY B.INAME,A.ICODE,B.CPARTNO,trim(a.icode)||trim(a.BTCHNO),A.BTCHNO,B.CINAME,B.UNIT HAVING (SUM(A.iqtyin)-SUM(A.iqtyout))>0 ";
                }
                break;

            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                //pop3
                // to avoid repeat of item
                col1 = "";
                if (btnval != "SG3_ROW_ADD" && btnval != "SG3_ROW_ADD_E")
                {
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[13].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                    }
                }
                if (col1.Length <= 0) col1 = "'-'";
                SQuery = "sELECT distinct a.icode as fstr,b.Iname,a.Icode,b.Cpartno,b.No_proc as Sec_Unit,a.rejqty as GVT,b.unit From inspmst a, item b where a.branchcd!='DD' and a.type='70' and trim(A.icodE)=trim(B.icode) and trim(a.icode)='" + txtlbl47.Text + "'order by b.iname";
                if (ind_Ptype == "01")
                    SQuery = "sELECT distinct a.icode as fstr,a.Iname,a.Icode,a.Cpartno,a.No_proc as Sec_Unit,A.unit From item a where a.branchcd!='DD' and trim(a.icode)='" + txtlbl47.Text + "'order by a.iname";
                break;

            case "SG2_ROW_ADD":
            case "SG2_ROW_ADD_E":
                col1 = "";
                //foreach (GridViewRow gr in sg2.Rows)
                //{
                //    if (gr.Cells[4].Text.Trim().Length > 2)
                //    {
                //        if (col1.Length > 0) col1 = col1 + "," + "'" + gr.Cells[4].Text.Trim() + "'";
                //        else col1 = "'" + gr.Cells[4].Text.Trim() + "'";
                //    }
                //}
                //if (col1.Length > 0)
                //{
                //    col1 = " and TRIM(type1) not in (" + col1 + ")";
                //}
                //else
                //{
                //    col1 = "";
                //}
                //if (col1 == "")
                //{
                //    SQuery = "Select type1 as fstr, Name,type1,branchcd from typewip where branchcd!='DD' and id='RJC61' " + col1 + " order by type1";
                //}
                //else
                //{
                //    SQuery = "Select type1 as fstr, Name,type1,branchcd from typewip where branchcd!='DD' and id='RJC61' " + col1 + " order by type1";
                //}
                SQuery = "Select type1 as fstr, Name,type1,branchcd from typewip where branchcd!='DD' and id='RJC61' order by type1";
                break;

            case "SG4_ROW_ADD":
            case "SG4_ROW_ADD_E":
                col1 = "";
                SQuery = "Select  type1 as fstr,Name,type1,branchcd from typewip where branchcd!='DD' and id='DTC61' order by type1";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
            case "SPrint":
                Type_Sel_query();
                break;
            case "Print_E":
            case "SPrint_E":
                SQuery = "select TRIM(A.BRANCHCD)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.JOB_NO)||TRIM(A.JOB_DT)||TRIM(A.ICODE) as fstr,B.iname,TO_CHAR(a.vchdate,'DD/MM/YYYY') as Batch_Dt,a.vchnum as Batch_No,a.ename as Machine,sum(a.iqtyin) as Batch_Qty,a.prevcode as ShiftName,a.mchcode,a.glue_code as plan_Cd,a.icode,substr(a.remarks2,1,9) as Refno,TO_CHAR(a.vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + " a,item b  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and  a.type='" + frm_vty + "' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " group by B.iname,a.vchdate,a.vchnum,a.ename,a.mchcode,a.prevcode,a.glue_code,a.icode,substr(a.remarks2,1,9),A.BRANCHCD,A.JOB_NO,TRIM(A.JOB_DT) order by VDD desc ,a.vchnum desc";
                SQuery = "select TRIM(A.BRANCHCD)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.ICODE) as fstr,B.iname,TO_CHAR(a.vchdate,'DD/MM/YYYY') as Batch_Dt,a.vchnum as Batch_No,a.ename as Machine,sum(a.iqtyin) as Batch_Qty,a.prevcode as ShiftName,a.mchcode,a.glue_code as plan_Cd,a.icode,substr(a.remarks2,1,9) as Refno,TO_CHAR(a.vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + " a,item b  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and  a.type='" + frm_vty + "' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " group by B.iname,a.vchdate,a.vchnum,a.ename,a.mchcode,a.prevcode,a.glue_code,a.icode,substr(a.remarks2,1,9),A.BRANCHCD,A.JOB_NO,TRIM(A.JOB_DT) order by VDD desc ,a.vchnum desc";
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "select a.vchnum||to_char(a.vchdate,'dd/MM/yyyy')as fstr,B.iname,TO_CHAR(a.vchdate,'DD/MM/YYYY') as Batch_Dt,a.vchnum as Batch_No,a.ename as Machine,sum(a.iqtyin) as Batch_Qty,a.prevcode as ShiftName,a.mchcode,a.glue_code as plan_Cd,a.icode,substr(a.remarks2,1,9) as Refno,TO_CHAR(a.vchdate,'YYYYMMDD') AS VDD from " + frm_tabname + " a,item b  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and  a.type='" + frm_vty + "' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " group by B.iname,a.vchdate,a.vchnum,a.ename,a.mchcode,a.prevcode,a.glue_code,a.icode,substr(a.remarks2,1,9) order by VDD desc ,a.vchnum desc";
                break;
        }
        if (typePopup == "N" && (btnval == "Edit" || btnval == "Del" || btnval == "Print" || btnval == "SPrint"))
        {
            btnval = btnval + "_E";
            hffield.Value = btnval;
            make_qry_4_popup();
        }
        else
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            

            hffield.Value = "New";

            if (typePopup == "N") newCase(frm_vty);

            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }

            // else comment upper code

            //frm_vnum = fgen.next_no(frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
            //txtvchnum.Text = frm_vnum;
            //txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            //disablectrl();
            //fgen.EnableForm(this.Controls);/// why this has been commented  
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Edit", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());

        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }

        if (txtlbl4a.Text == "" || txtlbl4a.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Select Process !!");
            return;
        }
        if (txtlbl7a.Text == "" || txtlbl7a.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Select Shift !!");
            return;
        }
        if (txtlbl50.Text == "" || txtlbl50.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Enter Start Time !!");
            return;
        }
        if (txtlbl51.Text == "" || txtlbl51.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please Enter Stop Time !!");
            return;
        }
        if (sg1.Rows.Count <= 1)
        {
            fgen.msg("-", "AMSG", "Please Select Atleast One in Input Tab !!");
            return;
        }
        if (sg2.Rows.Count <= 1)
        {
            fgen.msg("-", "AMSG", "Please Select Atleast One in Rejection Tab !!");
            return;
        }
        if (sg3.Rows.Count == 0)
        {
            fgen.msg("-", "AMSG", "Please Select Atleast One in Output Tab !!");
            return;
        }
        if (sg4.Rows.Count <= 1)
        {
            fgen.msg("-", "AMSG", "Please Select Atleast One in Downtime Tab !!");
            return;
        }

        double consumeQty = 0, rejQty = 0, qtytot = 0, totProd = 0, Difference = 0, JobCardWt = 0, ScrapWt = 0, OutputKGS = 0;
        //consumption grid
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            consumeQty += ((TextBox)sg1.Rows[i].FindControl("sg1_f7")).Text.toDouble();
        }
        // rejection grid
        for (int i = 0; i < sg2.Rows.Count; i++)
        {
            rejQty += ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.toDouble();
        }
        //production grid
        for (int i = 0; i < sg3.Rows.Count; i++)
        {
            qtytot += ((TextBox)sg3.Rows[i].FindControl("sg3_f5")).Text.toDouble();
        }

        string save = "N";

        if ((ind_Ptype == "12" || ind_Ptype == "13"))
        {
            JobCardWt = 1;
            totProd = ((rejQty + qtytot) * JobCardWt) + ScrapWt;
            OutputKGS = totProd - ScrapWt;
            Difference = (totProd - consumeQty).toDouble(2);

            if (Difference > 1 || Difference < -1)
            {
                msg = "Please See Total Input " + space(50) + " = <b>" + consumeQty + "</b> Kgs <br/>";
                msg += "_____________________________________________________________<br/>";
                msg += "Output Qty <br/>";
                msg += "" + space(20) + " a) Ok " + space(60) + " <b>" + qtytot + "</b> <br/>";
                msg += "" + space(20) + " b) Rejected " + space(50) + " <b>" + rejQty + "</b> <br/>";
                msg += "" + space(85) + "___________<br/>";
                msg += "" + space(91) + "<b>" + (rejQty + qtytot) + "</b><br/><br/>";

                {
                    //msg += "Std Wt/Pc (as per Job Card Paper Wt.) " + space(24) + " = <b>" + JobCardWt + "</b><br/>";
                    //msg += "Scrap Wt " + space(71) + " = <b>" + ScrapWt + "</b><br/>";
                    //msg += "Output Kgs " + space(68) + " = <b>" + OutputKGS + "</b><br/>";
                }

                msg += "_____________________________________________________________<br/>";
                msg += "Diff " + space(80) + " = <b>" + Difference + "</b><br/><br/>";
                msg += "Allowed within Tolerance of <b>1 kg</b><br/>";
                msg += "Please Fill Data & Reconcile.<br/>";
                msg += "Correct Data will help you in CAPA <br/>Corrective Action & Preventive Action Plan.<br/><br/>";
                save = "N";
                //fgen.msgBig(frm_qstr, "-", "AMSG", msg);

                save = "Y";
            }
            else save = "Y";
        }
        else
        {
            SQuery = "select a.qty,b.iweight,b.iname,a.col9,a.col7,a.col15,A.col13 from costestimate a , item b where trim(A.col9)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchnum='" + txtlbl43.Text.Trim() + "' and a.vchdate=to_DaTE('" + txtlbl14.Text.Trim() + "','dd/mm/yyyy') and substr(a.col9,1,2) in ('01','02','07','08','09','80','81') ";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            JobCardWt = 0;
            double papgiven = 0, jcqty1 = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (jcqty1 <= 0)
                    jcqty1 = dt.Rows[i]["qty"].ToString().toDouble() + dt.Rows[i]["col15"].ToString().toDouble() * dt.Rows[i]["col13"].ToString().toDouble();
                if (dt.Rows[i]["col9"].ToString().Substring(0, 2) == "07" || dt.Rows[i]["col9"].ToString().Substring(0, 2) == "80" || dt.Rows[i]["col9"].ToString().Substring(0, 2) == "81")
                    papgiven += dt.Rows[i]["col7"].ToString().toDouble();
                else papgiven += dt.Rows[i]["col7"].ToString().toDouble() * dt.Rows[i]["iweight"].ToString().toDouble();
            }

            JobCardWt = (papgiven / jcqty1).toDouble(3);
            ScrapWt = txtlbl5.Text.Trim().toDouble() + txtlbl6.Text.Trim().toDouble() + txtlbl8.Text.Trim().toDouble() + txtlbl9.Text.Trim().toDouble();

            if (ind_Ptype == "12" || ind_Ptype == "13")
            {
                papgiven = 1;
                JobCardWt = 1;
            }
            totProd = ((rejQty + qtytot) * JobCardWt) + ScrapWt;
            OutputKGS = totProd - ScrapWt;
            Difference = (totProd - consumeQty).toDouble(2);

            if (Difference > 1 || Difference < -1)
            {
                msg = "Please See Total Input " + space(50) + " = <b>" + consumeQty + "</b> Kgs <br/>";
                msg += "_____________________________________________________________<br/>";
                msg += "Output Qty <br/>";
                msg += "" + space(20) + " a) Ok " + space(60) + " <b>" + qtytot + "</b> <br/>";
                msg += "" + space(20) + " b) Rejected " + space(50) + " <b>" + rejQty + "</b> <br/>";
                msg += "" + space(85) + "___________<br/>";
                msg += "" + space(91) + "<b>" + (rejQty + qtytot) + "</b><br/><br/>";

                if (ind_Ptype == "12" || ind_Ptype == "13")
                {

                }
                else
                {
                    msg += "Std Wt/Pc (as per Job Card Paper Wt.) " + space(24) + " = <b>" + JobCardWt + "</b><br/>";
                    msg += "Scrap Wt " + space(71) + " = <b>" + ScrapWt + "</b><br/>";
                    msg += "Output Kgs " + space(68) + " = <b>" + OutputKGS + "</b><br/>";
                }
                msg += "_____________________________________________________________<br/>";
                msg += "Diff " + space(80) + " = <b>" + Difference + "</b><br/><br/>";
                msg += "Allowed within Tolerance of <b>1 kg</b><br/>";
                msg += "Please Fill Data & Reconcile.<br/>";
                msg += "Correct Data will help you in CAPA <br/>Corrective Action & Preventive Action Plan.<br/><br/>";
                save = "N";
                fgen.msgBig(frm_qstr, "-", "AMSG", msg);
            }
            else save = "Y";
        }

        //string chk_freeze="";
        //chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1011",txtvchdate.Text.Trim());
        //if (chk_freeze == "1")
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Rolling Freeze Date !!");
        //    return;
        //}
        //if (chk_freeze == "2")
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Fixed Freeze Date !!");
        //    return;
        //}
        //checks
        //-----------------------------------------------------------------------
        #region comment
        //string reqd_flds;
        //reqd_flds = "";
        //int reqd_nc;
        //reqd_nc = 0;

        //if (txtlbl50.Text.Trim().Length < 2)
        //{
        //    reqd_nc = reqd_nc + 1;
        //    reqd_flds = reqd_flds + " /  start time";
        //}

        //if (txtlbl51.Text.Trim().Length < 2)
        //{
        //    reqd_nc = reqd_nc + 1;
        //    reqd_flds = reqd_flds + " / end time ";
        //}
        //if (reqd_nc > 0)
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
        //    return;
        //}

        //string wo_reqd = "";// 
        //wo_reqd = fgen.seek_iname(frm_qstr, frm_cocd, "select upper(enable_yn) as fstr from stock where id='M011'", "fstr");


        //int i;
        //for (i = 0; i < sg1.Rows.Count - 0; i++)
        //{
        //    if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text) <= 0)
        //    {
        //        Checked_ok = "N";
        //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");

        //        i = sg1.Rows.Count;
        //        return; 
        //    }

        //    if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Length < 2 && wo_reqd=="Y")
        //    {
        //        Checked_ok = "N";
        //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , W.O. No. Not Filled Correctly at Line " + (i + 1) + "  !!");

        //        i = sg1.Rows.Count;
        //        return;
        //    }

        //    if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Length < 10)
        //    {
        //        Checked_ok = "N";
        //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Date Not Filled Correctly at Line " + (i + 1) + "  !!");

        //        i = sg1.Rows.Count;
        //        return; 
        //    }
        //    else
        //    {
        //        string curr_dt;
        //        string reqd_bydt;
        //        if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2)
        //        {
        //            curr_dt = Convert.ToDateTime(txtvchdate.Text).ToString("dd/MM/yyyy");
        //            reqd_bydt = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text).ToString("dd/MM/yyyy");

        //            if (Convert.ToDateTime(curr_dt) > Convert.ToDateTime(reqd_bydt))
        //            {
        //                Checked_ok = "N";
        //                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Required by Date Not Less Than Current Date, See line No. " + (i + 1) + "  !!");
        //                i = sg1.Rows.Count;
        //                return; 
        //            }
        //        }
        //    }
        //}

        #endregion
        //-----------------------------------------------------------------------        
        CalculateManPowerCost();
        if (save == "Y")
        {
            btnsave.Disabled = true;
            fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        }
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Delete", frm_qstr);
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
        lblStkval.Text = "";

        sg1_dt = new DataTable();
        sg2_dt = new DataTable();
        sg3_dt = new DataTable();
        create_tab();
        create_tab2();
        create_tab3();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();

        sg2_add_blankrows();
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();

        sg3_add_blankrows();
        sg3.DataSource = sg3_dt;
        sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();


        ViewState["sg1"] = null;
        ViewState["sg2"] = null;
        ViewState["sg3"] = null;
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vty = frm_vty;
        lbl1a.Text = vty;

        //frm_vnum = fgen.next_number(frm_qstr, frm_cocd, frm_tabname, frm_mbr, frm_vty, doc_nf.Value, doc_df.Value, DateRange,frm_CDT1,frm_uname,"Y" );

        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");

        txtvchnum.Text = frm_vnum;

        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

        todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");

        if (Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        {
            txtvchdate.Text = todt;
        }

        txtlbl2.Text = frm_uname;
        txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);


        //txtlbl5.Text = "-";
        //txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();

        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();


        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;

        sg3_dt = new DataTable();
        create_tab3();
        sg3_add_blankrows();


        sg3.DataSource = sg3_dt;
        sg3.DataBind();
        setColHeadings();
        ViewState["sg3"] = sg3_dt;



        sg2_dt = new DataTable();
        create_tab2();
        sg2_add_blankrows();


        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        setColHeadings();
        ViewState["sg2"] = sg2_dt;




        sg4_dt = new DataTable();
        create_tab4();
        sg4_add_blankrows();


        sg4.DataSource = sg4_dt;
        sg4.DataBind();
        setColHeadings();
        ViewState["sg4"] = sg4_dt;
        // Popup asking for Copy from Older Data
        //fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        //hffield.Value = "NEW_E";
        #endregion
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
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        frm_tabname2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                string chk_po_made;
                chk_po_made = fgen.seek_iname(frm_qstr, frm_cocd, "select ordno||' Dt.'||to_char(orddt,'dd/mm/yyyy') As fstr from pomas where branchcd||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'", "fstr");
                if (chk_po_made.Length > 6)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Purchase Order no ." + chk_po_made + " Already Made Against This P.R. , Deletion is Not Allowed !!");
                    return;
                }
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname1 + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "25" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname1 + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "40" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname2 + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "45" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname2 + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "55" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname6 + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "15" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + "25" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + "40" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + "45" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + "55" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + "15" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(6, 10), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                // Showing Confirmation
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6) + "");
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
            col4 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
            col7 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");

            switch (btnval)
            {
                case "New":
                    newCase(col1);
                    break;

                case "COPY_OLD":
                    #region Copy from Old Temp
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,b.text from " + frm_tabname + " a left outer join FIN_MSYS b on trim(a.frm_name)=trim(b.id) where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {

                        txtlbl4.Text = dt.Rows[i]["frm_name"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["text"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[i]["frm_header"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["ent_id"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["text"].ToString().Trim();
                            sg1_dr["sg1_h3"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";

                            sg1_dr["sg1_f1"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["text"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";


                            sg1_dr["sg1_t7"] = "";

                            if (frm_tabname.ToUpper() == "SYS_CONFIG")
                            {
                                sg1_dr["sg1_t7"] = dt.Rows[i]["OBJ_READONLY"].ToString().Trim();
                            }

                            sg1_dr["sg1_t8"] = "";

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1_add_blankrows();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                    }
                    #endregion
                    break;

                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;

                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Edit", frm_qstr);
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
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;
                case "SPrint":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "SPrint_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "select a.prevcode,a.branchcd,a.type,a.vchnum,a.vchdate,a.acode,a.ent_by,a.ent_dt,a.icode,i.iname,a.a1,a.a2,a.a4,a.iqtyin,a.iqtyout,a.mchcode,a.empcode,a.shftcode,a.job_no,a.job_dt,a.mcstart,a.mcstop,ename,a.remarks2 ,a.opr_dtl from " + frm_tabname + " a,item i  where trim(a.icode)=trim(i.icode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY SRNO";
                    SQuery1 = "select col5,scrp1,scrp2,time1,time2, branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,convdate,dropdate,col1,col2,col3,col4,col6,col13,comments2,comments3,comments4,substr(comments2,1,6) as comments2a,substr(comments2,8,length(comments2)) as comments2b,substr(comments3,1,6) as comments3a,substr(comments3,8,length(comments3)) as comments3b,substr(comments4,1,6) as comments4a,substr(comments4,8,length(comments4)) as comments4b from " + frm_tabname1 + " where branchcd||trim(vchnum)||to_char(vchdate,'dd/MM/yyyy')='" + frm_mbr + col1 + "' and type in ('25','40') ORDER BY SRNO";
                    Squery2 = "select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,col1,col2,col3,col4,col5,matl,finish from " + frm_tabname2 + " where branchcd||trim(vchnum)||to_char(vchdate,'dd/MM/yyyy')='" + frm_mbr + col1 + "' and type in ('55','45') ORDER BY SRNO";
                    //SQuery = "Select a.*,b.name,c.iname,c.cpartno as icpartno,c.cdrgno as icdrgno,c.unit as iunit,to_char(a.ent_Dt,'dd/mm/yyyy') as pent_dt,to_char(a.chk_Dt,'dd/mm/yyyy') as chkd_dt,to_char(a.app_Dt,'dd/mm/yyyy') as papp_dt from " + frm_tabname + " a,type b,item c where trim(a.acode)=trim(b.type1) and b.id='M' and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);//prod_sheet dt

                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery1); //costestimate dt

                    dt4 = new DataTable();
                    dt4 = fgen.getdata(frm_qstr, frm_cocd, Squery2); //inspvch dt
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl2.Text = dt.Rows[i]["ent_by"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["ent_Dt"].ToString().Trim();
                        txtlbl4.Text = dt.Rows[i]["acode"].ToString().Trim();
                        // txtlbl4a.Text = "CORRUGATION";//need to correct
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='1' and type1='" + dt.Rows[i]["acode"].ToString().Trim() + "'", "name");
                        txtlbl50.Text = dt.Rows[i]["mcstart"].ToString().Trim();
                        txtlbl51.Text = dt.Rows[i]["mcstop"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["shftcode"].ToString().Trim();
                        // txtlbl7a.Text = dt.Rows[0]["prevcode"].ToString().Trim(); // IT IS COMMENTED BECAUSE WHEN ENTRY SAVED THROUGH MAIN FINSYS IS PICKED THIS FIELD IS BLANK.
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_mbr, "select NAME from type where id='D' and type1='" + txtlbl7.Text.Trim() + "'", "name");
                        //txtlbl8.Text = dt.Rows[i]["app_by"].ToString().Trim();
                        //txtlbl9.Text = dt.Rows[i]["papp_Dt"].ToString().Trim();
                        txtlbl40.Text = dt.Rows[i]["mchcode"].ToString().Trim();
                        txtlbl40a.Text = dt.Rows[i]["ename"].ToString().Trim();//need to correct
                        txtlbl41.Text = dt.Rows[i]["empcode"].ToString().Trim();
                        txtlbl41a.Text = dt.Rows[i]["opr_dtl"].ToString().Trim();
                        txtlbl43.Text = dt.Rows[0]["job_no"].ToString().Trim();
                        txtlbl42.Text = dt.Rows[0]["job_no"].ToString().Trim();
                        txtlbl49.Text = dt.Rows[0]["job_dt"].ToString().Trim();//                       
                        txtlbl14.Text = dt.Rows[0]["job_dt"].ToString().Trim();//                       
                        txtlbl47.Text = dt.Rows[0]["icode"].ToString().Trim();
                        QtyReq = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select sum(is_number(col5)) as col5 from " + frm_tabname1 + " where branchcd='" + frm_mbr + "' and type='30' and trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + txtlbl43.Text + txtlbl49.Text + "'", "col5"));
                        Qty = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select distinct qty from " + frm_tabname1 + " where branchcd='" + frm_mbr + "' and type='30' and trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + txtlbl43.Text + txtlbl49.Text + "' and rownum=1", "qty"));
                        if (Qty > 0)
                        {
                            txtJobCardWt.Text = (QtyReq / Qty).ToString();
                        }
                        txtitemname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtlbl48.Text = dt.Rows[0]["iqtyout"].ToString().Trim();//                                      
                        create_tab();
                        sg1_dr = null;
                        create_tab3();
                        for (i = 0; i < dt2.Rows.Count; i++)
                        {
                            if (dt2.Rows[i]["type"].ToString().Trim() == "25")
                            {
                                sg1_dr = sg1_dt.NewRow();
                                sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                                sg1_dr["sg1_h1"] = "-";
                                sg1_dr["sg1_h2"] = "-";
                                sg1_dr["sg1_h3"] = "-";
                                sg1_dr["sg1_h4"] = "-";
                                sg1_dr["sg1_h5"] = "-";
                                sg1_dr["sg1_h6"] = "-";
                                sg1_dr["sg1_h7"] = "-";
                                sg1_dr["sg1_h8"] = "-";
                                sg1_dr["sg1_h9"] = "-";
                                sg1_dr["sg1_h10"] = "-";
                                sg1_dr["sg1_f3"] = dt2.Rows[i]["col13"].ToString().Trim();
                                sg1_dr["sg1_f4"] = dt2.Rows[i]["col1"].ToString().Trim();
                                sg1_dr["sg1_f5"] = dt2.Rows[i]["col2"].ToString().Trim();
                                sg1_dr["sg1_f6"] = dt2.Rows[i]["col3"].ToString().Trim();
                                sg1_dr["sg1_f7"] = dt2.Rows[i]["col4"].ToString().Trim();
                                sg1_dr["sg1_f8"] = dt2.Rows[i]["col6"].ToString().Trim();
                                GridInputTot += fgen.make_double(dt2.Rows[i]["col4"].ToString().Trim());
                                sg1_dt.Rows.Add(sg1_dr);
                            }
                            else
                            {
                                txtlbl44.Text = dt2.Rows[i]["comments2a"].ToString().Trim();
                                txtlbl45.Text = dt2.Rows[i]["comments3a"].ToString().Trim();
                                txtlbl46.Text = dt2.Rows[i]["comments4a"].ToString().Trim();
                                txtlbl44a.Text = dt2.Rows[i]["comments2b"].ToString().Trim();
                                txtlbl45a.Text = dt2.Rows[i]["comments3b"].ToString().Trim();
                                txtlbl46a.Text = dt2.Rows[i]["comments4b"].ToString().Trim();
                                sg3_dr = null;
                                sg3_dr = sg3_dt.NewRow();
                                sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;
                                sg3_dr["sg3_f1"] = dt2.Rows[i]["col1"].ToString().Trim();
                                sg3_dr["sg3_f2"] = dt2.Rows[i]["col2"].ToString().Trim();
                                sg3_dr["sg3_f4"] = dt2.Rows[i]["col4"].ToString().Trim();
                                sg3_dr["sg3_t1"] = dt2.Rows[i]["col3"].ToString().Trim();
                                sg3_dr["sg3_f5"] = dt2.Rows[i]["col5"].ToString().Trim();
                                sg3_dr["sg3_f6"] = dt2.Rows[i]["col6"].ToString().Trim();
                                txtlbl5.Text = dt2.Rows[i]["scrp1"].ToString().Trim();
                                txtlbl6.Text = dt2.Rows[i]["scrp2"].ToString().Trim();
                                txtlbl8.Text = dt2.Rows[i]["time1"].ToString().Trim();
                                txtlbl9.Text = dt2.Rows[i]["time2"].ToString().Trim();
                                GridOutputTot += fgen.make_double(dt2.Rows[i]["col3"].ToString().Trim());
                                sg3_dt.Rows.Add(sg3_dr);
                            }
                        }
                        //txtTotInput.Text = GridInputTot.ToString();
                        //txtTotOutput.Text = GridOutputTot.ToString();
                        create_tab2();
                        sg2_dr = null;
                        create_tab4();
                        sg4_dr = null;
                        for (i = 0; i < dt4.Rows.Count; i++)
                        {
                            if (dt4.Rows[i]["type"].ToString().Trim() == "45")
                            {
                                sg2_dr = sg2_dt.NewRow();
                                sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                                sg2_dr["sg2_f1"] = dt4.Rows[i]["col1"].ToString().Trim();
                                sg2_dr["sg2_f2"] = dt4.Rows[i]["col2"].ToString().Trim();
                                sg2_dr["sg2_t1"] = dt4.Rows[i]["col3"].ToString().Trim();
                                sg2_dt.Rows.Add(sg2_dr);
                            }
                            else
                            {
                                sg4_dr = sg4_dt.NewRow();
                                sg4_dr["sg4_srno"] = sg4_dt.Rows.Count + 1;
                                sg4_dr["sg4_f1"] = dt4.Rows[i]["col1"].ToString().Trim();
                                sg4_dr["sg4_f2"] = dt4.Rows[i]["col2"].ToString().Trim();
                                sg4_dr["sg4_t1"] = dt4.Rows[i]["col3"].ToString().Trim();
                                sg4_dr["sg4_t2"] = dt4.Rows[i]["col4"].ToString().Trim();
                                sg4_dr["sg4_t3"] = dt4.Rows[i]["col5"].ToString().Trim();
                                sg4_dr["sg4_t4"] = dt4.Rows[i]["matl"].ToString().Trim();
                                sg4_dr["sg4_t5"] = dt4.Rows[i]["finish"].ToString().Trim();
                                sg4_dt.Rows.Add(sg4_dr);
                            }
                        }
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1_add_blankrows();
                        sg1.DataBind();
                        sg1_dt.Dispose();
                        //////
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2_add_blankrows();
                        sg2.DataBind();
                        sg2_dt.Dispose();
                        ///////////////////////
                        if (sg3_dt != null)
                        {
                            ViewState["sg3"] = sg3_dt;
                            sg3_add_blankrows();
                            sg3.DataSource = sg3_dt;
                            sg3.DataBind();
                            sg3_dt.Dispose();
                        }
                        ///////
                        ViewState["sg4"] = sg4_dt;
                        sg4.DataSource = sg4_dt;
                        sg4_add_blankrows();
                        sg4.DataBind();
                        sg4_dt.Dispose();
                        ////sg1_add_blankrows();

                        //sg2_add_blankrows();
                        ////sg3_add_blankrows();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                        fgen.fin_prodpp_reps(frm_qstr);
                    }
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;

                case "SPrint_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID + "S");
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    btnlbl7.Focus();
                    break;

                case "MACHINE":
                    if (col1.Length <= 0) return;
                    txtlbl40a.Text = col2;
                    txtlbl40.Text = col1;
                    mchcode = col4;
                    ImageButton8.Focus();
                    break;

                case "PLANNO":
                    if (col1.Length <= 0) return;
                    txtlbl42.Text = col1.Substring(0, 6);
                    txtlbl47.Text = col3;
                    txtlbl43.Text = col1.Substring(0, 6);
                    txtlbl48.Text = col4;
                    txtlbl49.Text = col7;
                    txtlbl14.Text = col7;
                    lblitem.Text = col2;
                    txtitemname.Text = col2;

                    xprdrange1 = "BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    xprdrange = "BETWEEN TO_DATE('" + fromdt + "','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')";
                    dt3 = new DataTable();
                    string mq3 = "select TRIM(A.ICODE) AS ICODE,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + " as opening,0 as cdr,0 as ccr from itembal where BRANCHCD='" + frm_mbr + "' and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where BRANCHCD='" + frm_mbr + "' and TYPE LIKE '%' AND VCHDATE " + xprdrange1 + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%'  AND VCHDATE " + xprdrange + " and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY TRIM(A.ICODE) ORDER BY ICODE";
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, mq3);
                    string mq4 = fgen.seek_iname_dt(dt3, "ICODE='" + txtlbl47.Text + "'", "Closing_Stk");
                    lblStkval.Text = mq4 + "_" + fgen.seek_iname(frm_qstr, frm_cocd, "SELECT UNIT FROM ITEM WHERE ICODE='" + txtlbl47.Text + "'", "UNIT");
                    if (ind_Ptype == "01")
                    {
                    }
                    else
                    {
                        QtyReq = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select sum(is_number(col5)) as col5 from costestimate where branchcd='" + frm_mbr + "' and type='30' and trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'", "col5"));
                        Qty = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select distinct qty from costestimate where branchcd='" + frm_mbr + "' and type='30' and trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' and rownum=1", "qty"));
                    }
                    if (Qty > 0)
                    {
                        txtJobCardWt.Text = (QtyReq / Qty).ToString();
                    }
                    ImageButton5.Focus();
                    break;

                case "JOBNO":
                    if (col1.Length <= 0) return;
                    txtlbl43.Text = txtlbl42.Text;
                    break;

                case "TEAMLEAD":
                    if (col1.Length <= 0) return;
                    txtlbl41a.Text = col2;
                    txtlbl41.Text = col1;
                    ImageButton2.Focus();
                    break;

                case "OPR1":
                    if (col1.Length <= 0) return;
                    txtlbl44.Text = col1;
                    txtlbl44a.Text = col2;
                    ImageButton6.Focus();
                    break;

                case "OPR2":
                    if (col1.Length <= 0) return;
                    txtlbl45.Text = col1;
                    txtlbl45a.Text = col2;
                    ImageButton7.Focus();
                    break;

                case "OPR3":
                    if (col1.Length <= 0) return;
                    txtlbl46.Text = col1;
                    txtlbl46a.Text = col2;
                    txtlbl50.Focus();
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

                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    txtlbl5.Focus();
                    if (ind_Ptype == "01")
                        ImageButton1.Focus();
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    GridInputTot = 0;
                    //txtTotInput.Text = "0";
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
                            sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["sg1_h2"].ToString();
                            sg1_dr["sg1_h3"] = dt.Rows[i]["sg1_h3"].ToString();
                            sg1_dr["sg1_h4"] = dt.Rows[i]["sg1_h4"].ToString();
                            sg1_dr["sg1_h5"] = dt.Rows[i]["sg1_h5"].ToString();
                            sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                            sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                            sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                            sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                            sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();
                            sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                            sg1_dr["sg1_f6"] = dt.Rows[i]["sg1_f6"].ToString();
                            sg1_dr["sg1_f7"] = dt.Rows[i]["sg1_f7"].ToString();
                            sg1_dr["sg1_f8"] = dt.Rows[i]["sg1_f8"].ToString();
                            sg1_dr["sg1_f9"] = dt.Rows[i]["sg1_f9"].ToString();
                            GridInputTot += fgen.make_double(dt.Rows[i]["sg1_f7"].ToString());
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        dt = new DataTable();
                        WIPStDt = fgen.seek_iname(frm_qstr, frm_mbr, "select trim(wipstdt) as wipstdt from type where id='B' and type1='" + frm_mbr + "'", "wipstdt");
                        if (WIPStDt.Length == 1)
                        {
                            WIPStDt = fgen.seek_iname(frm_qstr, frm_mbr, "select params from controls where id='R10'", "params");
                        }

                        SQuery = "select trim(a.icode)||trim(a.kclreelno) as fstr, B.iname as Item_name,trim(a.icode) as ERP_Code,sum(a.iqtyin)-sum(a.iqtyout) as balance,b.Unit,trim(a.kclreelno) as ReelNO,b.Cpartno as Part_no,b.ciname,max(A.coreelno) as coreel,sum(a.iqtyin) as Rcvd,sum(a.iqtyout) as Used from (SELECT ICODE,reelwout AS IQTYIN,REELWIN AS IQTYOUT,kclreelno,coreelno FROM reelvch WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('31','32','11') AND trim(SUBSTR(ICODE,1,2)) IN ('07','08','09','80','81') and NOT (TYPE='32' AND LENGTH(TRIM(ACODE))>=6) AND vchdate between to_Date('" + WIPStDt + "','dd/mm/yyyy') and to_date('" + txtvchdate.Text + "','dd/mm/yyyy')  AND TRIM(NVL(RINSP_BY,'-'))!='REELOP*' AND 1=1  UNION ALL SELECT ICODE,IQTYIN,0 AS IQTYOUT,wolink,col1t as coreelno FROM wipstk WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('50') and trim(Stage)='" + txtlbl4.Text + "' AND trim(SUBSTR(ICODE,1,2)) IN ('07','08','09','80','81') and vchdate>=to_Date('" + WIPStDt + "','dd/mm/yyyy') union all UNION ALL SELECT ICODE,iqtyout as IQTYIN,iqtyin AS IQTYOUT,'-' as wolink,'-' as coreelno FROM ivoucher WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('30','31','11') and trim(Stage)='" + txtlbl4.Text + "' and  trim(SUBSTR(ICODE,1,2)) not IN ('07','08','09','80','81') and vchdate>=to_Date('" + WIPStDt + "','dd/mm/yyyy')  union all SELECT ICODE,0 AS IQTYIN,itate as IQTYOUT,col6,null as coreelno FROM costestimate WHERE branchcd='" + frm_mbr + "' and type='25' and vchdate  between to_Date('" + WIPStDt + "','dd/mm/yyyy') and to_date('" + txtvchdate.Text + "','dd/mm/yyyy') and trim(col21)='" + txtlbl4.Text + "' ) a,item b where trim(a.icode)=trim(B.icode) and trim(a.icode)||trim(a.kclreelno) in (" + col1 + ") group by b.iname,b.cpartno,b.ciname,b.unit,trim(a.icode),trim(a.kclreelno) having sum(a.iqtyin)-sum(a.iqtyout)>0 order by B.iname";
                        if (txtlbl4.Text != "61")
                            SQuery = " SELECT trim(a.icode)||trim(a.BTCHNO) AS FSTR,B.INAME AS ITEM_NAME,A.ICODE AS ITEM_CODE,SUM(A.iqtyin)-SUM(A.iqtyout) AS balance,b.unit,a.BTCHNO as btchno,B.CPARTNO,b.ciname,'-' as coreelno,sum(a.iqtyin) as rcvd,sum(a.iqtyout) as used,'-' AS coreel,A.ICODE AS erp_code,A.BTCHNO AS reelno FROM (SELECT TRIM(ICODE) AS ICODE,IQTYIN,0 AS IQTYOUT,trim(btchno) as btchno FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='3A' AND VCHDATE " + DateRange + " AND TRIM(aCODe)='" + txtlbl4.Text + "' UNION ALL SELECT TRIM(ICODE) AS ICODE,0 AS PLAN,IQTYIN AS PROD,trim(remarks2) as btchno FROM PROD_SHEET WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " AND trim(ACODE)='" + txtlbl4.Text.Trim() + "' AND TRIM(JOB_NO)||TRIM(JOB_DT)='" + txtlbl43.Text + txtlbl14.Text + "' ) A , ITEM B WHERE TRIM(A.ICODE)=TRIM(B.ICODE) GROUP BY B.INAME,A.ICODE,B.CPARTNO,trim(a.icode)||trim(a.BTCHNO),A.BTCHNO,B.CINAME,B.UNIT HAVING (SUM(A.iqtyin)-SUM(A.iqtyout))>0 and trim(a.icode)||trim(a.BTCHNO) in (" + col1 + ")";
                        if (ind_Ptype == "01")
                        {
                            SQuery = "SELECT X.*,0 AS USED,X.BATCH_NO AS COREEL,X.ITEM_NAME,X.ERP_CODE,BAL AS BALANCE,X.BATCH_NO AS REELNO FROM (" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL") + ") X WHERE TRIM(X.FSTR) IN (" + col1 + ") ";
                        }
                        else
                            SQuery = "SELECT X.*,0 AS USED,X.BATCH_NO AS COREEL,X.ITEM_NAME,X.ERP_CODE,BAL AS BALANCE,X.BATCH_NO AS REELNO FROM (" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL") + ") X WHERE TRIM(X.FSTR) IN (" + col1 + ") ";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        //raw_mat
                        //batch_no
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = "-";
                            sg1_dr["sg1_h2"] = "-";
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";
                            sg1_dr["sg1_f1"] = dt.Rows[d]["used"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["coreel"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["unit"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["erp_code"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["item_name"].ToString().Trim();
                            sg1_dr["sg1_f6"] = "1";
                            sg1_dr["sg1_f7"] = dt.Rows[d]["balance"].ToString().Trim();
                            sg1_dr["sg1_f8"] = dt.Rows[d]["reelno"].ToString().Trim();
                            sg1_dr["sg1_f9"] = "-";
                            GridInputTot += fgen.make_double(dt.Rows[d]["balance"].ToString().Trim());
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    //txtTotInput.Text = GridInputTot.ToString();
                    if (sg1_dt == null) return;
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1_add_blankrows();
                    sg1.DataBind();
                    dt.Dispose();
                    sg1_dt.Dispose();
                    #endregion
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    //********* Saving in Hidden Field 
                    //********* Saving in GridView Value
                    WIPStDt = fgen.seek_iname(frm_qstr, frm_mbr, "select trim(wipstdt) as wipstdt from type where id='B' and type1='" + frm_mbr + "'", "wipstdt");
                    if (WIPStDt.Length == 1)
                    {
                        WIPStDt = fgen.seek_iname(frm_qstr, frm_mbr, "select params from controls where id='R10'", "params");
                    }
                    SQuery = "select trim(a.icode)||trim(a.kclreelno) as fstr, B.iname as Item_name,trim(a.icode) as ERP_Code,sum(a.iqtyin)-sum(a.iqtyout) as balance,b.Unit,trim(a.kclreelno) as ReelNO,b.Cpartno as Part_no,b.ciname,max(A.coreelno) as coreel,sum(a.iqtyin) as Rcvd,sum(a.iqtyout) as Used from (SELECT ICODE,reelwout AS IQTYIN,REELWIN AS IQTYOUT,kclreelno,coreelno FROM reelvch WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('31','32','11') AND trim(SUBSTR(ICODE,1,2)) IN ('07','08','09','80','81') and NOT (TYPE='32' AND LENGTH(TRIM(ACODE))=6) AND vchdate between to_Date('" + WIPStDt + "','dd/mm/yyyy') and to_date('" + txtvchdate.Text + "','dd/mm/yyyy')  AND TRIM(NVL(RINSP_BY,'-'))!='REELOP*' AND 1=1  UNION ALL SELECT ICODE,IQTYIN,0 AS IQTYOUT,wolink,col1t as coreelno FROM wipstk WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('50') AND trim(SUBSTR(ICODE,1,2)) IN ('07','08','09','80','81') and vchdate>=to_Date('" + WIPStDt + "','dd/mm/yyyy')  union all SELECT ICODE,0 AS IQTYIN,itate as IQTYOUT,col6,null as coreelno FROM costestimate WHERE branchcd='" + frm_mbr + "' and type='25' and vchdate  between to_Date('" + WIPStDt + "','dd/mm/yyyy') and to_date('" + txtvchdate.Text + "','dd/mm/yyyy')) a,item b where trim(a.icode)=trim(B.icode) and trim(a.icode)||trim(a.kclreelno)= '" + col1 + "' group by b.iname,b.cpartno,b.ciname,b.unit,trim(a.icode),trim(a.kclreelno) having sum(a.iqtyin)-sum(a.iqtyout)>0 order by B.iname";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        //sg1_dr["sg1_srno"] = dt.Rows.Count + 1;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = hf1.Value;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = dt.Rows[0]["used"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = dt.Rows[0]["coreel"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[5].Text = dt.Rows[0]["unit"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[6].Text = dt.Rows[0]["erp_code"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[7].Text = dt.Rows[0]["item_name"].ToString().Trim();
                        (((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_f7")))).Text = dt.Rows[0]["balance"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[20].Text = dt.Rows[0]["reelno"].ToString().Trim();
                        dt.Dispose();
                        GridInputTot = 0;
                        for (int i = 0; i < sg1.Rows.Count - 1; i++)
                        {
                            GridInputTot += fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_f7"))).Text);
                        }
                        //txtTotInput.Text = GridInputTot.ToString();
                    }
                    break;

                case "SG3_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    if (ViewState["sg3"] != null)
                    {
                        dt = new DataTable();
                        sg3_dt = new DataTable();
                        dt = (DataTable)ViewState["sg3"];
                        z = dt.Rows.Count - 1;
                        sg3_dt = dt.Clone();
                        sg3_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = Convert.ToInt32(dt.Rows[i]["sg3_srno"].ToString());
                            sg3_dr["sg3_f1"] = dt.Rows[i]["sg3_f1"].ToString();
                            sg3_dr["sg3_f2"] = dt.Rows[i]["sg3_f2"].ToString();
                            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                            sg3_dr["sg3_f4"] = dt.Rows[i]["sg3_f4"].ToString();
                            sg3_dr["sg3_f5"] = ((TextBox)sg3.Rows[i].FindControl("sg3_f5")).Text.Trim();
                            sg3_dr["sg3_f6"] = dt.Rows[i]["sg3_f6"].ToString();
                            sg3_dt.Rows.Add(sg3_dr);
                        }
                        dt = new DataTable();
                        SQuery = "sELECT distinct b.Iname,a.Icode,b.Cpartno,b.No_proc as Sec_Unit,a.rejqty as GVT,b.unit From inspmst a, item b where a.branchcd!='DD' and a.type='70' and trim(A.icodE)=trim(B.icode) and trim(a.icode)='" + txtlbl47.Text + "' order by b.iname";
                        if (ind_Ptype == "01")
                        {
                            SQuery = "select x.*,1 as gvt from (" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL") + ") x where trim(x.fstr)='" + txtlbl47.Text + "'";
                        }
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        i = 1; mchcode = txtlbl40.Text.Trim();
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;
                            sg3_dr["sg3_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg3_dr["sg3_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                            if (ind_Ptype == "12" || ind_Ptype == "13") sg3_dr["sg3_f4"] = "1";
                            else sg3_dr["sg3_f4"] = dt.Rows[d]["gvt"].ToString().Trim();

                            sg3_dr["sg3_f6"] = txtvchnum.Text + "/" + txtlbl4.Text + "-" + mchcode + "/" + (sg3.Rows.Count).ToString().PadLeft(3, '0');
                            sg3_dr["sg3_t1"] = "";
                            sg3_dt.Rows.Add(sg3_dr);
                            i++;
                        }
                    }
                    // sg3_add_blankrows();
                    ViewState["sg3"] = sg3_dt;
                    sg3_add_blankrows();
                    sg3.DataSource = sg3_dt;
                    sg3.DataBind();
                    if (sg3_dt != null) sg3_dt.Dispose();
                    ((TextBox)sg3.Rows[z].FindControl("sg3_t1")).Focus();
                    #endregion
                    break;
                case "QTYBOX":
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL1").toDouble() > 0)
                    {
                        if (ViewState["sg3"] != null)
                        {

                            i = 1; mchcode = txtlbl40.Text.Trim();

                            sg3_dt = new DataTable();
                            dt = (DataTable)ViewState["sg3"];
                            z = dt.Rows.Count - 1;
                            sg3_dt = dt.Clone();
                            sg3_dr = null;
                            for (i = 0; i < dt.Rows.Count - 1; i++)
                            {
                                sg3_dr = sg3_dt.NewRow();
                                sg3_dr["sg3_srno"] = Convert.ToInt32(dt.Rows[i]["sg3_srno"].ToString());
                                sg3_dr["sg3_f1"] = dt.Rows[i]["sg3_f1"].ToString();
                                sg3_dr["sg3_f2"] = dt.Rows[i]["sg3_f2"].ToString();
                                sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                                sg3_dr["sg3_f4"] = dt.Rows[i]["sg3_f4"].ToString();
                                sg3_dr["sg3_f5"] = ((TextBox)sg3.Rows[i].FindControl("sg3_f5")).Text.Trim();
                                sg3_dr["sg3_f6"] = dt.Rows[i]["sg3_f6"].ToString();
                                sg3_dt.Rows.Add(sg3_dr);
                            }

                            double fullQty = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL1").toDouble();
                            double batchQty = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").toDouble();
                            double totnumrows = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL3").toDouble();
                            double totprodroll = 0;
                            dt = new DataTable();
                            SQuery = "sELECT distinct b.Iname,a.Icode,b.Cpartno,b.No_proc as Sec_Unit,a.rejqty as GVT,b.unit From inspmst a, item b where a.branchcd!='DD' and a.type='70' and trim(A.icodE)=trim(B.icode) and trim(a.icode)='" + txtlbl47.Text + "'order by b.iname";
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            do
                            {
                                sg3_dr = sg3_dt.NewRow();
                                sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;
                                sg3_dr["sg3_f1"] = dt.Rows[0]["icode"].ToString().Trim();
                                sg3_dr["sg3_f2"] = dt.Rows[0]["iname"].ToString().Trim();
                                if (ind_Ptype == "12" || ind_Ptype == "13") sg3_dr["sg3_f4"] = "1";
                                else sg3_dr["sg3_f4"] = dt.Rows[0]["gvt"].ToString().Trim();

                                sg3_dr["sg3_f6"] = txtvchnum.Text + "/" + txtlbl4.Text + "-" + mchcode + "/" + (sg3_dt.Rows.Count + 1).ToString().PadLeft(3, '0');

                                if (fullQty <= batchQty)
                                {
                                    batchQty = fullQty;
                                    fullQty = fullQty - batchQty;
                                }
                                else fullQty = fullQty - batchQty;

                                sg3_dr["sg3_t1"] = batchQty;
                                sg3_dt.Rows.Add(sg3_dr);
                                i++;
                            }
                            while (fullQty != 0);
                        }
                    }
                    // sg3_add_blankrows();
                    ViewState["sg3"] = sg3_dt;
                    sg3_add_blankrows();
                    sg3.DataSource = sg3_dt;
                    sg3.DataBind();
                    if (sg3_dt != null) sg3_dt.Dispose();
                    //((TextBox)sg3.Rows[z].FindControl("sg3_t1")).Focus();
                    break;
                case "SG3_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    SQuery = "SELECT distinct b.Iname,a.Icode,b.Cpartno,b.No_proc as Sec_Unit,a.rejqty as GVT,b.unit From inspmst a, item b where a.branchcd!='DD' and a.type='70' and trim(A.icodE)=trim(B.icode) and trim(a.icode)='" + txtlbl47.Text + "'order by b.iname";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        mchcode = txtlbl40.Text.Trim();
                        int j = 0;
                        j = Convert.ToInt32(hf1.Value + 1);
                        sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = dt.Rows[0]["iname"].ToString().Trim();
                        sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[6].Text = dt.Rows[0]["gvt"].ToString().Trim();
                        sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[8].Text = txtvchnum.Text.Trim() + "/" + txtlbl4.Text.Trim() + "-" + mchcode + "/0" + j; //by yogita
                        sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = col1.Trim();
                    }
                    break;

                case "SG2_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg2_dt = new DataTable();
                        dt = (DataTable)ViewState["sg2"];
                        z = dt.Rows.Count - 1;
                        sg2_dt = dt.Clone();
                        sg2_dr = null;
                        i = 0;
                        for (i = 0; i < sg2.Rows.Count - 1; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = (i + 1);
                            sg2_dr["sg2_f1"] = sg2.Rows[i].Cells[3].Text.Trim().Replace("&nbsp;", "-");
                            sg2_dr["sg2_f2"] = sg2.Rows[i].Cells[4].Text.Trim().Replace("&nbsp;", "-");
                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim().Replace("&nbsp;", "-");
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim().Replace("&nbsp;", "-");
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        sg2_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg2_add_blankrows();
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        for (i = 0; i < sg2.Rows.Count; i++)
                        {
                            sg2.Rows[i].Cells[2].Text = (i + 1).ToString();
                        }
                    }
                    #endregion
                    setColHeadings();
                    break;

                case "SG3_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg3_dt = new DataTable();
                        dt = (DataTable)ViewState["sg3"];
                        z = dt.Rows.Count - 1;
                        sg3_dt = dt.Clone();
                        sg3_dr = null;
                        i = 0;
                        for (i = 0; i < sg3.Rows.Count; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = (i + 1);
                            sg3_dr["sg3_f1"] = sg3.Rows[i].Cells[3].Text.Trim().Replace("&nbsp;", "-");
                            sg3_dr["sg3_f2"] = sg3.Rows[i].Cells[4].Text.Trim().Replace("&nbsp;", "-");
                            sg3_dr["sg3_f4"] = sg3.Rows[i].Cells[6].Text.Trim().Replace("&nbsp;", "-");
                            sg3_dr["sg3_f6"] = sg3.Rows[i].Cells[8].Text.Trim().Replace("&nbsp;", "-");
                            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim().Replace("&nbsp;", "-");
                            sg3_dr["sg3_f5"] = ((TextBox)sg3.Rows[i].FindControl("sg3_f5")).Text.Trim().Replace("&nbsp;", "-");
                            sg3_dt.Rows.Add(sg3_dr);
                        }
                        sg3_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        // sg3_add_blankrows();
                        ViewState["sg3"] = sg3_dt;
                        sg3_add_blankrows();
                        sg3.DataSource = sg3_dt;
                        sg3.DataBind();
                        for (i = 0; i < sg3.Rows.Count; i++)
                        {
                            sg3.Rows[i].Cells[2].Text = (i + 1).ToString();
                        }
                    }
                    #endregion
                    setColHeadings();
                    break;

                case "SG4_RMV":
                    #region Remove Row from GridView
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    {
                        dt = new DataTable();
                        sg4_dt = new DataTable();
                        dt = (DataTable)ViewState["sg4"];
                        z = dt.Rows.Count - 1;
                        sg4_dt = dt.Clone();
                        sg4_dr = null;
                        i = 0;
                        for (i = 0; i < sg4.Rows.Count - 1; i++)
                        {
                            sg4_dr = sg4_dt.NewRow();
                            sg4_dr["sg4_srno"] = (i + 1);
                            sg4_dr["sg4_f1"] = sg4.Rows[i].Cells[3].Text.Trim().Replace("&nbsp;", "-").Replace("&amp;", "&");
                            sg4_dr["sg4_f2"] = sg4.Rows[i].Cells[4].Text.Trim().Replace("&nbsp;", "-").Replace("&amp;", "&");
                            sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim().Replace("&nbsp;", "-");
                            sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim().Replace("&nbsp;", "-");
                            sg4_dr["sg4_t3"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t3")).Text.Trim().Replace("&nbsp;", "-");
                            sg4_dr["sg4_t4"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t4")).Text.Trim().Replace("&nbsp;", "-");
                            sg4_dr["sg4_t5"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t5")).Text.Trim().Replace("&nbsp;", "-");
                            sg4_dt.Rows.Add(sg4_dr);
                        }
                        sg4_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg4_add_blankrows();
                        ViewState["sg4"] = sg4_dt;
                        sg4.DataSource = sg4_dt;
                        sg4.DataBind();
                        for (i = 0; i < sg4.Rows.Count; i++)
                        {
                            sg4.Rows[i].Cells[2].Text = (i + 1).ToString();
                        }
                    }
                    #endregion
                    setColHeadings();
                    break;

                case "SG1_RMV":
                    #region Remove Row from GridView
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
                            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.Trim();
                            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.Trim();
                            sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.Trim();
                            sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.Trim();
                            sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.Trim();
                            sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.Trim();
                            sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.Trim();
                            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.Trim();
                            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.Trim();
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.Trim().Replace("&nbsp;", "-");
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim().Replace("&nbsp;", "-");
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim().Replace("&nbsp;", "-");
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim().Replace("&nbsp;", "-");
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim().Replace("&nbsp;", "-");
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.Trim().Replace("&nbsp;", "-");
                            sg1_dr["sg1_f8"] = sg1.Rows[i].Cells[20].Text.Trim().Replace("&amp;nbsp;", "-").Replace("&nbsp;", "-");
                            sg1_dr["sg1_f9"] = sg1.Rows[i].Cells[21].Text.Trim().Replace("&amp;nbsp;", "-").Replace("&nbsp;", "-");
                            sg1_dr["sg1_f7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f7")).Text.Trim().Replace("&nbsp;", "-");
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        for (i = 0; i < sg1.Rows.Count; i++)
                        {
                            sg1.Rows[i].Cells[12].Text = (i + 1).ToString();
                        }
                    }
                    #endregion
                    setColHeadings();
                    break;

                case "SG2_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    if (ViewState["sg2"] != null)
                    {
                        dt = new DataTable();
                        sg2_dt = new DataTable();
                        dt = (DataTable)ViewState["sg2"];
                        z = dt.Rows.Count - 1;
                        sg2_dt = dt.Clone();
                        sg2_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = Convert.ToInt32(dt.Rows[i]["sg2_srno"].ToString());
                            sg2_dr["sg2_f1"] = dt.Rows[i]["sg2_f1"].ToString();
                            sg2_dr["sg2_f2"] = dt.Rows[i]["sg2_f2"].ToString();
                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                            sg2_dt.Rows.Add(sg2_dr);
                        }

                        dt = new DataTable();
                        SQuery = "Select Name,type1,branchcd from typewip where branchcd!='DD' and id='RJC61' and trim(type1) in (" + col1 + ") order by type1";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_f1"] = dt.Rows[d]["name"].ToString().Trim();
                            sg2_dr["sg2_f2"] = dt.Rows[d]["type1"].ToString().Trim();
                            sg2_dr["sg2_t1"] = "";
                            sg2_dr["sg2_t2"] = "";
                            sg2_dr["sg2_t3"] = "";
                            sg2_dr["sg2_t4"] = "";
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                    }
                    ViewState["sg2"] = sg2_dt;
                    sg2.DataSource = sg2_dt;
                    sg2_add_blankrows();
                    sg2.DataBind();
                    sg2_dt.Dispose();
                    ((TextBox)sg2.Rows[z].FindControl("sg2_t1")).Focus();
                    #endregion
                    break;

                case "SG2_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    SQuery = "Select Name,type1,branchcd from typewip where branchcd!='DD' and id='RJC61' and trim(type1) = '" + col1 + "' order by type1";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        sg2.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = col1;
                        sg2.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = col2;
                    }
                    break;

                case "SG4_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    if (ViewState["sg4"] != null)
                    {
                        dt = new DataTable();
                        sg4_dt = new DataTable();
                        dt = (DataTable)ViewState["sg4"];
                        z = dt.Rows.Count - 1;
                        sg4_dt = dt.Clone();
                        sg4_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg4_dr = sg4_dt.NewRow();
                            sg4_dr["sg4_srno"] = Convert.ToInt32(dt.Rows[i]["sg4_srno"].ToString());
                            sg4_dr["sg4_f1"] = dt.Rows[i]["sg4_f1"].ToString();
                            sg4_dr["sg4_f2"] = dt.Rows[i]["sg4_f2"].ToString();
                            sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                            sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
                            sg4_dr["sg4_t3"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t3")).Text.Trim();
                            sg4_dr["sg4_t4"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t4")).Text.Trim();
                            sg4_dt.Rows.Add(sg4_dr);
                        }

                        dt = new DataTable();
                        SQuery = "Select type1 as fstr,Name,type1,branchcd from typewip where branchcd!='DD' and id='DTC61' and trim(type1) in (" + col1 + ")order by type1";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg4_dr = sg4_dt.NewRow();
                            sg4_dr["sg4_srno"] = sg4_dt.Rows.Count + 1;
                            sg4_dr["sg4_f1"] = dt.Rows[d]["name"].ToString().Trim();
                            sg4_dr["sg4_f2"] = dt.Rows[d]["type1"].ToString().Trim();
                            sg4_dr["sg4_t1"] = "";
                            sg4_dr["sg4_t2"] = "";
                            sg4_dr["sg4_t3"] = "";
                            sg4_dr["sg4_t4"] = "";
                            sg4_dt.Rows.Add(sg4_dr);
                        }
                    }
                    sg4_add_blankrows();
                    ViewState["sg4"] = sg4_dt;
                    sg4.DataSource = sg4_dt;
                    sg4.DataBind();
                    dt.Dispose(); sg4_dt.Dispose();
                    ((TextBox)sg4.Rows[z].FindControl("sg4_t1")).Focus();
                    #endregion
                    break;

                case "SG4_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    SQuery = "Select  type1 as fstr,Name,type1,branchcd from typewip where branchcd!='DD' and id='DTC61' and trim(type1) = '" + col1 + "' order by type1";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        sg4.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = dt.Rows[0]["Name"].ToString().Trim();
                        sg4.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = dt.Rows[0]["type1"].ToString().Trim();
                    }
                    break;
                case "ProdnRep":
                    if (col1 == "1")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40132");
                        fgen.fin_prodpp_reps(frm_qstr);
                    }
                    else if (col1 == "2")
                    {
                        SQuery = "";
                    }
                    else if (col1 == "3")
                    {
                        hffield.Value = "Mach";
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("-", frm_qstr);
                    }
                    else
                    {
                        hffield.Value = "Empl";
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("-", frm_qstr);
                    }
                    hf2.Value = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    break;
                case "Mach":
                case "Empl":
                case "Section":
                    if (col1.Length <= 0) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", hf2.Value);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL2", col1);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40132");
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;

            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        frm_tabname2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "select a.vchnum as Batch_No,TO_CHAR(a.vchdate,'DD/MM/YYYY') as Batch_Dt,B.iname as item_Name,a.ename as Machine,sum(a.iqtyin) as Batch_Qty,a.prevcode as ShiftName,a.mchcode,a.glue_code as plan_Cd,a.icode,substr(a.remarks2,1,9) as Refno,TO_CHAR(a.vchdate,'YYYYMMDD') AS VDD from prod_sheet a,item b  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " group by B.iname,a.vchdate,a.vchnum,a.ename,a.mchcode,a.prevcode,a.glue_code,a.icode,substr(a.remarks2,1,9) order by VDD desc ,a.vchnum desc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Corrugation Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "ProdnRep")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "select '1' as fstr, 'All' as opt,'1' as sno from dual union all select '2' as fstr, 'Section' as opt,'2' as sno from dual union all select '3' as fstr, 'Machine' as opt,'3' as sno from dual union all select '4' as fstr, 'Empl' as opt,'4' as sno from dual";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_sseek("Select Scope", frm_qstr);
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------
            i = 0;
            hffield.Value = "";
            setColHeadings();

            #region Number Gen and Save to Table
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "N")
            {
                btnsave.Disabled = false;
            }
            else
            {
                btnsave.Disabled = true;
                if (Checked_ok == "Y")
                {
                    try
                    {
                        oDS = new DataSet();
                        oporow = null;
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS5 = new DataSet();
                        oporow5 = null;
                        oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname2);//for rejection an ddown time same table only type is different..thats why2 ods is maked for saving data into same table but on diff type

                        oDS4 = new DataSet();
                        oporow4 = null;
                        oDS4 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname2);

                        oDS2 = new DataSet();
                        oporow2 = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1); //costestimate table

                        oDS3 = new DataSet();
                        oporow3 = null;
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);

                        oDS6 = new DataSet();
                        oporow6 = null;
                        oDS6 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname6);

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();
                        save_fun2();
                        save_fun3();
                        save_fun6();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS5.Dispose();
                        oporow5 = null;
                        oDS5 = new DataSet();
                        oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname2);

                        oDS4.Dispose();
                        oporow4 = null;
                        oDS4 = new DataSet();
                        oDS4 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname2);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);


                        oDS3.Dispose();
                        oporow3 = null;
                        oDS3 = new DataSet();
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);

                        oDS6.Dispose();
                        oDS6 = new DataSet();
                        oporow6 = null;
                        oDS6 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname6);


                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnum.Text.Trim();
                            save_it = "Y";
                        }

                        else
                        {
                            save_it = "N";
                            for (i = 0; i < sg1.Rows.Count - 0; i++)
                            {
                                //if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2)
                                {
                                    save_it = "Y";
                                }
                            }

                            if (save_it == "Y")
                            {
                                string doc_is_ok = "";
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        save_fun2();
                        save_fun3();
                        save_fun6();
                        if (edmode.Value == "Y")
                        {
                            // prod_sheet
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            //costestimate type 25
                            cmd_query = "update " + frm_tabname1 + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "25" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            // cost estimate tpe 40
                            cmd_query = "update " + frm_tabname1 + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "40" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            // inspvch type 45
                            cmd_query = "update " + frm_tabname2 + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "45" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            // inspvch type 55
                            cmd_query = "update " + frm_tabname2 + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "55" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            // ivoucher type 15
                            cmd_query = "update " + frm_tabname6 + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "15" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS5, frm_tabname2);
                        fgen.save_data(frm_qstr, frm_cocd, oDS4, frm_tabname2);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, frm_tabname1);
                        fgen.save_data(frm_qstr, frm_cocd, oDS3, frm_tabname1);

                        fgen.save_data(frm_qstr, frm_cocd, oDS6, frm_tabname6);

                        //fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                        //fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully'13'Do you want to see the Print Preview ?");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";

                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "delete from " + frm_tabname1 + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + "25" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";

                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "delete from " + frm_tabname1 + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + "40" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";

                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "delete from " + frm_tabname2 + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + "45" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";

                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "delete from " + frm_tabname2 + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + "55" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                            cmd_query = "delete from " + frm_tabname6 + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + "15" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "CMSG", lblheader.Text + " " + frm_vnum + " Saved Successfully'13'Do you want to see the Print Preview ?");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);

                        string isManPowerCostCalculate = "Y";
                        // control panel to add 
                        if (isManPowerCostCalculate == "Y")
                        {
                            CalculateManPowerCost();
                        }

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_mbr + frm_vnum.Trim() + txtvchdate.Text.Trim() + txtlbl47.Text.Trim() + "'");
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                        hffield.Value = "SAVED";
                    }
                    catch (Exception ex)
                    {
                        btnsave.Disabled = false;
                        fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N";
                    }
            #endregion
                }
            }
        }
    }
    //------------------------------------------------------------------------------------
    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        // Hidden Field
        sg1_dt.Columns.Add(new DataColumn("sg1_h1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h10", typeof(string)));

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



        //sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t5", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t6", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t7", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t8", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t9", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t10", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t11", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t12", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t13", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t14", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t15", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t16", typeof(string)));

    }
    //------------------------------------------------------------------------------------
    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        // Hidden Field

        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t4", typeof(string)));

    }
    //------------------------------------------------------------------------------------
    public void create_tab3()
    {
        sg3_dt = new DataTable();
        sg3_dr = null;
        // Hidden Field
        sg3_dt.Columns.Add(new DataColumn("sg3_srno", typeof(Int32)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f4", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f5", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f6", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t1", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void create_tab4()
    {
        sg4_dt = new DataTable();
        sg4_dr = null;
        // Hidden Field
        sg4_dt.Columns.Add(new DataColumn("sg4_SrNo", typeof(Int32)));
        sg4_dt.Columns.Add(new DataColumn("sg4_f1", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_f2", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t1", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t2", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t3", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t4", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t5", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        if (sg1_dt == null) return;
        sg1_dr = sg1_dt.NewRow();
        sg1_dr["sg1_h1"] = "-";
        sg1_dr["sg1_h2"] = "-";
        sg1_dr["sg1_h3"] = "-";
        sg1_dr["sg1_h4"] = "-";
        sg1_dr["sg1_h5"] = "-";
        sg1_dr["sg1_h6"] = "-";
        sg1_dr["sg1_h7"] = "-";
        sg1_dr["sg1_h8"] = "-";
        sg1_dr["sg1_h9"] = "-";
        sg1_dr["sg1_h10"] = "-";
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
        sg1_dt.Rows.Add(sg1_dr);
    }
    //------------------------------------------------------------------------------------
    public void sg2_add_blankrows()
    {
        if (sg2_dt == null) return;
        sg2_dr = sg2_dt.NewRow();
        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
        sg2_dr["sg2_t1"] = "-";
        sg2_dr["sg2_t2"] = "-";
        sg2_dt.Rows.Add(sg2_dr);
    }
    //------------------------------------------------------------------------------------
    public void sg3_add_blankrows()
    {
        if (sg3_dt == null) return;
        sg3_dr = sg3_dt.NewRow();
        sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;
        sg3_dr["sg3_f1"] = "-";
        sg3_dr["sg3_f2"] = "-";
        sg3_dr["sg3_t1"] = "-";
        sg3_dr["sg3_f4"] = "-";
        sg3_dr["sg3_f5"] = "-";
        sg3_dr["sg3_f6"] = "-";
        sg3_dt.Rows.Add(sg3_dr);
    }
    //------------------------------------------------------------------------------------
    public void sg4_add_blankrows()
    {
        if (sg4_dt == null) return;
        sg4_dr = sg4_dt.NewRow();
        sg4_dr["sg4_SrNo"] = sg4_dt.Rows.Count + 1;
        sg4_dr["sg4_f1"] = "-";
        sg4_dr["sg4_f2"] = "-";
        sg4_dr["sg4_t1"] = "-";
        sg4_dr["sg4_t2"] = "-";
        sg4_dr["sg4_t3"] = "-";
        sg4_dr["sg4_t4"] = "-";
        sg4_dr["sg4_t5"] = "-";
        sg4_dt.Rows.Add(sg4_dr);
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg1r = 0; sg1r < sg1.Rows.Count; sg1r++)
            {
                for (int j = 0; j < sg1.Columns.Count; j++)
                {
                    sg1.Rows[sg1r].Cells[j].ToolTip = sg1.Rows[sg1r].Cells[j].Text;
                    if (sg1.Rows[sg1r].Cells[j].Text.Trim().Length > 30)
                    {
                        sg1.Rows[sg1r].Cells[j].Text = sg1.Rows[sg1r].Cells[j].Text.Substring(0, 30);
                    }
                }
            }
            sg1.HeaderRow.Cells[10].Width = 40;
            sg1.HeaderRow.Cells[11].Width = 40;
            sg1.HeaderRow.Cells[12].Width = 40;
            sg1.HeaderRow.Cells[14].Width = 40;
            sg1.HeaderRow.Cells[2].Text = "slg";
            sg1.HeaderRow.Cells[3].Text = "Mill.Reel";
            sg1.HeaderRow.Cells[3].Width = 40;
            sg1.HeaderRow.Cells[4].Text = "Unit";
            sg1.HeaderRow.Cells[5].Text = "S.No.";
            sg1.HeaderRow.Cells[6].Text = "ItemCode";
            sg1.HeaderRow.Cells[7].Text = "InputName";
            sg1.HeaderRow.Cells[8].Text = "No.OfPkg";
            sg1.HeaderRow.Cells[9].Text = "Qty";
            sg1.HeaderRow.Cells[20].Text = "ReelNo.";
            if (ind_Ptype == "01")
            {
                sg1.HeaderRow.Cells[14].Text = "Batch.No";
                sg1.HeaderRow.Cells[20].Text = "Batch.No.";
            }
        }
    }
    //------------------------------------------------------------------------------------
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
                    fgen.Fn_open_mseek("Select Item", frm_qstr);
                    //fgen.Fn_open_mseek("Select Item", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg2.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
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
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Rejection Reason From The List");
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
                    fgen.Fn_open_sseek("Select Rejection Reason", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG2_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Rejection Reasons", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg3.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG3_RMV":
                if (index < sg3.Rows.Count)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG3_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "SG3_ROW_ADD":
                if (txtlbl40.Text.Trim().Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Select Machine First");
                    return;
                }
                else
                {
                    if ((ind_Ptype == "12" || ind_Ptype == "13") && txtlbl4.Text == "65")
                    {
                        SQuery = "";
                        Fn_ValueBox("-", frm_qstr);
                    }
                    else
                    {
                        if (index < sg3.Rows.Count - 1)
                        {
                            hf1.Value = index.ToString();
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                            //----------------------------
                            hffield.Value = "SG3_ROW_ADD_E";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                            make_qry_4_popup();
                            fgen.Fn_open_sseek("Select Item", frm_qstr);
                        }
                        else
                        {
                            hffield.Value = "SG3_ROW_ADD";
                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                            make_qry_4_popup();
                            fgen.Fn_open_sseek("Select Item", frm_qstr);
                        }
                    }
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }
    //------------------------------------------------------------------------------------
    protected void sg4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg4.Rows[rowIndex].RowIndex);

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG4_RMV":
                if (index < sg4.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG4_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This DownTime Reason From The List");
                }
                break;


            case "SG4_ROW_ADD":
                if (index < sg4.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG4_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select DownTime Reason", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG4_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select DownTime Reasons", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Process ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl10_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MACHINE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Machine ", frm_qstr);

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl11_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TEAMLEAD";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Team Leader", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl12_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PLANNO";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Plan No. ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "JOBNO";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Job Code", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "OPR1";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Operator", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "OPR2";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Operator ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "OPR3";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Operator ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl19_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl20_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl21_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl22_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl23_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select shift ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
        frm_tabname2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9");

        double sum = sumrejection();
        i = 0;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (i = 0; i < sg3.Rows.Count - 1; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty.Trim().ToUpper();
            oporow["vchnum"] = txtvchnum.Text.Trim().ToUpper();
            oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            //prod_sheet table
            oporow["acode"] = txtlbl4.Text.Trim();
            oporow["icode"] = txtlbl47.Text.Trim();//got from plan number
            oporow["a1"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim().ToUpper());
            oporow["a2"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_f5")).Text.Trim().ToUpper());
            oporow["a3"] = 0;
            oporow["a4"] = sum;// add rejection qunatity
            oporow["a5"] = 0;
            oporow["a6"] = 0;
            oporow["a7"] = 0;
            oporow["a8"] = 0;
            oporow["total"] = 0;
            oporow["un_melt"] = 0;
            oporow["mlt_loss"] = sum;// add rejection quantity
            oporow["remarks2"] = sg3.Rows[i].Cells[8].Text.Trim().ToUpper();
            oporow["flag"] = 1;
            oporow["srno"] = i;
            oporow["remarks"] = "-";
            oporow["stage"] = txtlbl4.Text.Trim();
            if (frm_formID == "F40106") oporow["stage"] = "02";
            oporow["iqtyin"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_f5")).Text.Trim().ToUpper());
            oporow["iqtyout"] = txtlbl48.Text.Trim();
            oporow["subcode"] = fgen.seek_iname(frm_qstr, frm_cocd, "select unit from item where icode='" + txtlbl47.Text.Trim().ToUpper() + "'", "unit");
            oporow["mchcode"] = txtlbl40.Text.Trim().ToUpper();
            oporow["prevstage"] = "-";
            oporow["prevcode"] = txtlbl7a.Text.Trim().ToUpper();
            oporow["empcode"] = txtlbl41.Text.Trim().ToUpper();
            oporow["shftcode"] = txtlbl7.Text.Trim().ToUpper();
            oporow["noups"] = 1;
            oporow["job_no"] = txtlbl43.Text.Trim().ToUpper();
            oporow["job_dt"] = txtlbl49.Text.Trim().ToUpper();
            oporow["mcstart"] = txtlbl50.Text.Trim().ToUpper();
            oporow["mcstop"] = txtlbl51.Text.Trim().ToUpper();
            date1 = Convert.ToDateTime(txtlbl51.Text.Trim().ToUpper());
            date2 = Convert.ToDateTime(txtlbl50.Text.Trim().ToUpper());
            Diff = date1 - date2;
            oporow["TSLOT"] = Diff.TotalMinutes.ToString();
            oporow["ename"] = txtlbl40a.Text.Trim().ToUpper();
            oporow["glue_code"] = txtlbl42.Text.Trim().ToUpper();
            oporow["wo_no"] = txtlbl42.Text.Trim().ToUpper();
            oporow["wo_dt"] = fgen.make_def_Date(txtlbl49.Text.Trim().ToUpper(), vardate);
            oporow["opr_dtl"] = txtlbl41a.Text.Trim().ToUpper();
            oporow["a9"] = 0;
            oporow["a10"] = 0;
            oporow["a11"] = 0;
            oporow["a12"] = 0;
            oporow["lmd"] = 0;
            oporow["bcd"] = 0;
            oporow["var_code"] = "-";
            oporow["film_code"] = "-";
            oporow["naration"] = "-";
            oporow["num1"] = 0;
            oporow["num2"] = 0;
            oporow["num3"] = 0;
            oporow["num4"] = 0;
            oporow["num5"] = 0;
            oporow["num6"] = 0;
            oporow["num7"] = 0;
            oporow["num8"] = 0;
            oporow["num9"] = 0;
            oporow["num10"] = 0;
            oporow["num11"] = 0;
            oporow["num12"] = 0;
            oporow["mtime"] = "-";
            oporow["exc_time"] = "-";
            oporow["tempr"] = "-";
            oporow["irate"] = 0;
            oporow["mseq"] = 0;
            oporow["a13"] = 0;
            oporow["a14"] = 0;
            oporow["a15"] = 0;
            oporow["a16"] = 0;
            oporow["a17"] = 0;
            oporow["a18"] = 0;
            oporow["a19"] = 0;
            oporow["a20"] = 0;
            if (ind_Ptype == "12" || ind_Ptype == "13")
            {
                oporow["a19"] = txtByProdInk.Text.toDouble();
                oporow["a20"] = txtByProdThin.Text.toDouble();
            }
            oporow["fm_fact"] = 1;
            oporow["pcpshot"] = 1;
            oporow["PBTCHNO"] = "-";
            oporow["OEE_R"] = 0;
            oporow["HCUT"] = 0;
            oporow["ALSTTIM"] = 0;
            oporow["ALTCTIM"] = 0;
            oporow["CUST_REF"] = 0;
            oporow["CELL_REF"] = "-";
            oporow["CELL_REFN"] = "-";
            if (ind_Ptype != "12" && ind_Ptype != "13")
                oporow["dcode"] = "-";
            if (ind_Ptype != "01" && ind_Ptype != "05" && ind_Ptype != "12" && ind_Ptype != "13")
            {
                oporow["a21"] = 0;
                oporow["a22"] = 0;
                oporow["a23"] = 0;
                oporow["a24"] = 0;
                oporow["a25"] = 0;
                oporow["a26"] = 0;
                oporow["a27"] = 0;
                oporow["a28"] = 0;
                oporow["a29"] = 0;
                oporow["a30"] = 0;
            }
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
                oporow["eDt_dt"] = vardate;
            }
            

            oDS.Tables[0].Rows.Add(oporow);
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        for (i = 0; i < sg2.Rows.Count - 1; i++)
        {
            oporow5 = oDS5.Tables[0].NewRow();
            oporow5["branchcd"] = frm_mbr;
            oporow5["type"] = "45";
            oporow5["vchnum"] = txtvchnum.Text.Trim().ToUpper();
            oporow5["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow5["title"] = txtlbl40a.Text.Trim().ToUpper();
            oporow5["btchno"] = txtvchnum.Text.Trim().ToUpper();
            oporow5["acode"] = txtlbl4.Text.Trim().ToUpper();
            oporow5["icode"] = txtlbl47.Text.Trim().ToUpper();
            oporow5["grade"] = "-";// value not found so after discussion with Mayuri mam leaving blank
            oporow5["cpartno"] = "-";
            oporow5["srno"] = i + 1;
            oporow5["col1"] = sg2.Rows[i].Cells[3].Text.Trim().ToUpper();
            oporow5["col2"] = sg2.Rows[i].Cells[4].Text.Trim().ToUpper();
            oporow5["col3"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim().ToUpper());
            oporow5["btchdt"] = txtvchdate.Text.Trim().ToUpper();
            oporow5["obsv15"] = txtlbl7a.Text.Trim().ToUpper();
            oporow5["obsv16"] = txtlbl41a.Text.Trim().ToUpper();
            oporow5["mrrnum"] = frm_vty.Trim().ToUpper();
            oporow5["mrrdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow5["qty8"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim().ToUpper());
            oporow5["col4"] = "-";
            oporow5["col5"] = "-";
            oporow5["col6"] = "-";
            oporow5["result"] = "-";
            oporow5["obsv1"] = "-";
            oporow5["obsv2"] = "-";
            oporow5["obsv3"] = "-";
            oporow5["obsv4"] = "-";
            oporow5["obsv5"] = "-";
            oporow5["obsv6"] = "-";
            oporow5["obsv7"] = "-";
            oporow5["obsv8"] = "-";
            oporow5["obsv9"] = "-";
            oporow5["obsv10"] = "-";
            oporow5["obsv11"] = "-";
            oporow5["obsv12"] = "-";
            oporow5["obsv13"] = "-";
            oporow5["obsv14"] = "-";
            oporow5["contplan"] = "-";
            oporow5["wono"] = "-";
            oporow5["matl"] = "-";
            oporow5["finish"] = "-";
            oporow5["omax"] = "-";
            oporow5["omin"] = "-";
            oporow5["rejqty"] = 0;
            oporow5["obj1"] = 0;
            oporow5["obj2"] = 0;
            oporow5["obj3"] = 0;
            oporow5["obj4"] = 0;
            oporow5["obj5"] = 0;
            oporow5["obj6"] = 0;
            oporow5["qty1"] = 0;
            oporow5["qty2"] = 0;
            oporow5["qty3"] = 0;
            oporow5["qty4"] = 0;
            oporow5["mrsrno"] = "-";
            oporow5["qty5"] = 0;
            oporow5["qty6"] = 0;
            oporow5["qty7"] = 0;
            oporow5["lsrno"] = "-";
            oporow5["amdtno"] = 0;
            oporow5["matlrdt"] = "-";
            oporow5["rel_by"] = "-";
            oporow5["rel_dt"] = "-";
            oporow5["spr_ref"] = "-";
            oporow5["spr_lot"] = "-";

            if (edmode.Value == "Y")
            {
                oporow5["eNt_by"] = ViewState["entby"].ToString();
                oporow5["eNt_dt"] = ViewState["entdt"].ToString();
            }
            else
            {
                oporow5["eNt_by"] = frm_uname;
                oporow5["eNt_dt"] = vardate;
            }
            hffield.Value = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
            hffield.Value = hffield.Value;
            if (i == 0)
            {
                oporow5["sampqty"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_f5")).Text.Trim());
            }
            else
            {
                oporow5["sampqty"] = "0";
            }
            oDS5.Tables[0].Rows.Add(oporow5);
        }
        for (i = 0; i < sg4.Rows.Count - 1; i++)
        {
            //if (i == 0)
            //{
            oporow4 = oDS4.Tables[0].NewRow();
            oporow4["branchcd"] = frm_mbr;
            oporow4["type"] = "55";
            oporow4["vchnum"] = txtvchnum.Text.Trim().ToUpper();
            oporow4["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow4["title"] = txtlbl40a.Text.Trim().ToUpper();
            oporow4["btchno"] = txtvchnum.Text.Trim().ToUpper();
            oporow4["acode"] = txtlbl4.Text.Trim().ToUpper();
            oporow4["icode"] = txtlbl47.Text.Trim().ToUpper();
            oporow4["grade"] = "-";// value not found so after discussion with Mayuri mam leaving blank
            oporow4["cpartno"] = "-";
            oporow4["srno"] = i + 1;
            oporow4["col1"] = sg4.Rows[i].Cells[3].Text.Trim().ToUpper();
            oporow4["col2"] = sg4.Rows[i].Cells[4].Text.Trim().ToUpper();
            date1 = Convert.ToDateTime(((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim());
            date2 = Convert.ToDateTime(((TextBox)sg4.Rows[i].FindControl("sg4_t3")).Text.Trim());
            Diff = date2 - date1;
            Min = Diff.TotalMinutes;
            // oporow4["col3"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim().ToUpper();
            oporow4["col3"] = Min.ToString();
            oporow4["col4"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim().ToUpper();
            oporow4["col5"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t3")).Text.Trim().ToUpper();
            //oporow4["col6"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t4")).Text.Trim().ToUpper();
            oporow4["col6"] = "-";
            oporow4["btchdt"] = txtvchdate.Text.Trim().ToUpper();
            oporow4["obsv15"] = txtlbl7a.Text.Trim().ToUpper();
            oporow4["obsv16"] = txtlbl41a.Text.Trim().ToUpper();
            oporow4["mrrnum"] = frm_vty.Trim().ToUpper();
            oporow4["mrrdate"] = txtvchdate.Text.Trim().ToUpper();
            // oporow4["matl"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t5")).Text.Trim().ToUpper();//no confirmed
            oporow4["matl"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t4")).Text.Trim().ToUpper();
            oporow4["finish"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t5")).Text.Trim().ToUpper();
            oporow4["result"] = "-";
            oporow4["obsv1"] = "-";
            oporow4["obsv2"] = "-";
            oporow4["obsv3"] = "-";
            oporow4["obsv4"] = "-";
            oporow4["obsv5"] = "-";
            oporow4["obsv6"] = "-";
            oporow4["obsv7"] = "-";
            oporow4["obsv8"] = "-";
            oporow4["obsv9"] = "-";
            oporow4["obsv10"] = "-";
            oporow4["obsv11"] = "-";
            oporow4["obsv12"] = "-";
            oporow4["obsv13"] = "-";
            oporow4["obsv14"] = "-";
            oporow4["contplan"] = "-";
            oporow4["wono"] = "-";
            oporow4["omax"] = "-";
            oporow4["omin"] = "-";
            oporow4["rejqty"] = 0;
            oporow4["obj1"] = 0;
            oporow4["obj2"] = 0;
            oporow4["obj3"] = 0;
            oporow4["obj4"] = 0;
            oporow4["obj5"] = 0;
            oporow4["obj6"] = 0;
            oporow4["qty1"] = 0;
            oporow4["qty2"] = 0;
            oporow4["qty3"] = 0;
            oporow4["qty4"] = 0;
            oporow4["mrsrno"] = "-";
            oporow4["qty5"] = 0;
            oporow4["qty6"] = 0;
            oporow4["qty7"] = 0;
            oporow4["lsrno"] = "-";
            oporow4["amdtno"] = 0;
            oporow4["matlrdt"] = "-";
            oporow4["rel_by"] = "-";
            oporow4["rel_dt"] = "-";
            oporow4["spr_ref"] = "-";
            oporow4["spr_lot"] = "-";
            if (edmode.Value == "Y")
            {
                oporow4["eNt_by"] = ViewState["entby"].ToString();
                oporow4["eNt_dt"] = ViewState["entdt"].ToString();
            }
            else
            {
                oporow4["eNt_by"] = frm_uname;
                oporow4["eNt_dt"] = vardate;
            }
            hffield.Value = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
            hffield.Value = hffield.Value;
            if (i == 0)
            {
                oporow4["sampqty"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_f5")).Text.Trim());
            }
            else
            {
                oporow4["sampqty"] = "0";
            }
            oDS4.Tables[0].Rows.Add(oporow4);
            //}
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun3()
    {
        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            oporow2 = oDS2.Tables[0].NewRow();
            oporow2["branchcd"] = frm_mbr;
            oporow2["type"] = "25";
            oporow2["vchnum"] = txtvchnum.Text.Trim().ToUpper();
            oporow2["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow2["status"] = "-";
            oporow2["convdate"] = txtlbl48.Text.Trim().ToUpper();
            oporow2["comments"] = "-";
            oporow2["srno"] = i + 1;
            // oporow2["dropdate"] = txtlbl48.Text.Trim().ToUpper();
            oporow2["dropdate"] = "-";// value not found so after discussion with Mayuri mam leaving blank
            oporow2["acode"] = txtlbl47.Text.Trim().ToUpper();
            oporow2["icode"] = sg1.Rows[i].Cells[16].Text.Trim().ToUpper();
            oporow2["col1"] = sg1.Rows[i].Cells[16].Text.Trim().ToUpper();
            oporow2["col2"] = sg1.Rows[i].Cells[17].Text.Trim().ToUpper();
            oporow2["col3"] = sg1.Rows[i].Cells[18].Text.Trim().ToUpper();
            oporow2["col4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_f7")).Text.Trim().ToUpper());
            oporow2["itate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_f7")).Text.Trim().ToUpper());
            oporow2["col5"] = "-";
            oporow2["col6"] = sg1.Rows[i].Cells[20].Text.Trim().ToUpper();
            // oporow2["qty"] = txtlbl48.Text; //THIS IS SAVED IN 4 DIFF FIELDS 2 FIELD IS DATE DATATYPE AND 1 IS CHAR AND 1 IS NUMBER.....WHICH IS RIGHT?????NEED HELP
            oporow2["qty"] = 0;
            oporow2["printyn"] = "Y";
            oporow2["col11"] = "SF";
            oporow2["col12"] = "-";// value not found so after discussion with Mayuri mam leaving blank
            oporow2["enqno"] = txtlbl42.Text.Trim().ToUpper();
            oporow2["enqdt"] = txtlbl49.Text.Trim().ToUpper();
            oporow2["col21"] = txtlbl4.Text.Trim().ToUpper();
            oporow2["col23"] = txtlbl7a.Text.Trim().ToUpper();
            oporow2["col24"] = txtlbl50.Text.Trim().ToUpper(); //THIS FIELD SAVED IN TWO DIFF FILEDS ???
            oporow2["col25"] = txtlbl51.Text.Trim().ToUpper();
            oporow2["startdt"] = "-";// value not found so after discussion with Mayuri mam leaving blank
            oporow2["col13"] = sg1.Rows[i].Cells[15].Text.Trim().ToUpper();
            oporow2["jstatus"] = "-";
            oporow2["supcl_by"] = txtlbl40a.Text.Trim().Length > 30 ? txtlbl40a.Text.Trim().Substring(0, 29).ToUpper() : txtlbl40a.Text.Trim().ToUpper();
            double input = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_f7")).Text.Trim().ToUpper());
            input = input + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_f7")).Text.Trim().ToUpper());
            //oporow2["itate"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim(); 
            oporow2["app_dt"] = vardate;
            oporow2["az_dt"] = vardate;
            oporow2["picode"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE,QTY FROM (select acode,QTY from costestimate where branchcd='" + frm_mbr + "' and type='30' and trim(Vchnum)='" + txtlbl42.Text.Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + txtlbl49.Text.Trim() + "' and icode='" + txtlbl47.Text.Trim() + "') WHERE ROWNUM<2", "ACODE");
            oporow2["jstatus"] = "N";
            oporow2["col7"] = "-";
            oporow2["col8"] = "-";
            oporow2["remarks"] = txtrmk.Text.Trim().ToUpper();
            oporow2["col9"] = "-";
            oporow2["col10"] = "-";
            oporow2["col14"] = "-";
            oporow2["col15"] = "-";
            oporow2["col16"] = "-";
            oporow2["col17"] = "-";
            oporow2["col18"] = "-";
            oporow2["col19"] = "-";
            oporow2["col20"] = "-";
            oporow2["col22"] = 0;
            oporow2["irate"] = 0;
            oporow2["app_by"] = "-";
            oporow2["attach"] = "-";
            oporow2["attach2"] = "-";
            oporow2["attach3"] = "-";
            oporow2["AZ_by"] = "-";
            oporow2["comments2"] = "-";
            oporow2["comments3"] = "-";
            oporow2["clo_dt"] = vardate;
            oporow2["comments4"] = "-";
            oporow2["comments5"] = "-";
            oporow2["splcd"] = "-";
            oporow2["jhold"] = "-";
            oporow2["prc1"] = "-";
            oporow2["prc2"] = "-";
            oporow2["prc3"] = "-";
            oporow2["prc4"] = "-";
            oporow2["num1"] = 0;
            oporow2["enr1"] = 0;
            oporow2["enr2"] = 0;
            oporow2["altitem"] = "-";
            oporow2["eff_wt"] = 0;
            oporow2["scrp1"] = txtRcylScrap.Text.toDouble();
            oporow2["scrp2"] = txtNonRcylScrap.Text.toDouble();
            oporow2["time1"] = 0;
            oporow2["time2"] = 0;
            if (edmode.Value == "Y")
            {
                oporow2["eNt_by"] = ViewState["entby"].ToString();
                oporow2["eNt_dt"] = ViewState["entdt"].ToString();
                oporow2["edt_by"] = frm_uname;
                oporow2["edt_dt"] = vardate;
            }
            else
            {
                oporow2["eNt_by"] = frm_uname;
                oporow2["eNt_dt"] = vardate;
                oporow2["APP_DT"] = vardate;
                oporow2["AZ_dt"] = vardate;
                oporow2["edt_by"] = "-";
                oporow2["edt_dt"] = vardate;
            }

            if (ind_Ptype != "05")
            {
                oporow2["manpwr"] = txtNumOfOperator.Text.toDouble();
                oporow2["chgovers"] = txtNumOfHelper.Text.toDouble();
            }

            oDS2.Tables[0].Rows.Add(oporow2);
            lblinput.Text = input.ToString();
        }
        for (i = 0; i < sg3.Rows.Count - 1; i++)
        {
            //if (i == 0)
            {
                oporow3 = oDS3.Tables[0].NewRow();
                oporow3["branchcd"] = frm_mbr;
                oporow3["type"] = "40";
                oporow3["vchnum"] = txtvchnum.Text.Trim().ToUpper();
                oporow3["vchdate"] = txtvchdate.Text.Trim().ToUpper();
                oporow3["srno"] = i;
                oporow3["acode"] = txtlbl47.Text.Trim().ToUpper();
                oporow3["icode"] = txtlbl47.Text.Trim().ToUpper();
                oporow3["col1"] = txtlbl47.Text.Trim().ToUpper();
                oporow3["qty"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim().ToUpper());
                oporow3["col1"] = txtlbl47.Text.Trim().ToUpper();
                oporow3["col2"] = sg3.Rows[i].Cells[4].Text.Trim().ToUpper();
                oporow3["col3"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim().ToUpper());
                oporow3["col4"] = sg3.Rows[i].Cells[6].Text.Trim().ToUpper();
                oporow3["col5"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_f5")).Text.Trim().ToUpper());
                oporow3["col6"] = sg3.Rows[i].Cells[8].Text.Trim().ToUpper();
                oporow3["ent_by"] = frm_uname;
                oporow3["ent_dt"] = vardate;
                oporow3["printyn"] = "Y";
                oporow3["col11"] = "FG";
                oporow3["col12"] = txtlbl40a.Text.Trim().ToUpper();// value not found so after discussion with Mayuri mam saving machine name
                oporow3["enqno"] = txtlbl42.Text.Trim().ToUpper();
                oporow3["enqdt"] = txtlbl49.Text.Trim().ToUpper();
                oporow3["supcl_by"] = txtlbl40a.Text.Trim().Length > 30 ? txtlbl40a.Text.Trim().Substring(0, 29).ToUpper() : txtlbl40a.Text.Trim().ToUpper();
                oporow3["col13"] = sg3.Rows[i].Cells[6].Text.Trim().ToUpper();
                oporow3["col21"] = txtlbl4.Text.Trim().ToUpper();
                oporow3["col23"] = txtlbl7a.Text.Trim().ToUpper();
                oporow3["col24"] = txtlbl41a.Text.Trim().ToUpper();
                oporow3["col25"] = txtlbl40a.Text.Trim().ToUpper();
                oporow3["col22"] = txtlbl4a.Text.Trim().ToUpper();
                oporow3["scrp1"] = fgen.make_double(txtlbl5.Text.Trim().ToUpper());
                oporow3["scrp2"] = fgen.make_double(txtlbl6.Text.Trim().ToUpper());
                oporow3["time1"] = fgen.make_double(txtlbl8.Text.Trim().ToUpper());
                oporow3["time2"] = fgen.make_double(txtlbl9.Text.Trim().ToUpper());
                oporow3["prc1"] = "-";//value not found so after discussion with Mayuri mam leaving blank
                oporow3["prc2"] = "-";//value not found so after discussion with Mayuri mam leaving blank
                oporow3["prc3"] = "-";//value not found so after discussion with Mayuri mam leaving blank
                oporow3["prc4"] = "-";
                //correc
                oporow3["num1"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim().ToUpper());//value not found
                oporow3["comments2"] = txtlbl44.Text.Trim().ToUpper() + "  " + txtlbl44a.Text.Trim().ToUpper();
                oporow3["comments3"] = txtlbl45.Text.Trim().ToUpper() + "  " + txtlbl45a.Text.Trim().ToUpper(); ;
                oporow3["comments4"] = txtlbl46.Text.Trim().ToUpper() + "  " + txtlbl46a.Text.Trim().ToUpper(); ;
                oporow3["status"] = "-";
                oporow3["convdate"] = "-";
                oporow3["dropdate"] = "-";
                oporow3["comments"] = "-";
                oporow3["col7"] = "-";
                oporow3["col8"] = "-";
                oporow3["remarks"] = "-";
                oporow3["startdt"] = "-";
                oporow3["col9"] = "-";
                oporow3["col10"] = "-";
                oporow3["col14"] = "-";
                oporow3["col15"] = "-";
                oporow3["col16"] = "-";// value not found so after discussion with Mayuri mam leaving blank
                oporow3["col17"] = "-";
                oporow3["col18"] = "-";
                oporow3["col19"] = "-";
                oporow3["col20"] = "-";
                oporow3["itate"] = 0;
                oporow3["irate"] = 0;
                oporow3["app_by"] = "-";
                oporow3["attach"] = "-";
                oporow3["attach2"] = "-";
                oporow3["attach3"] = "-";
                oporow3["AZ_by"] = "-";
                oporow3["picode"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE,QTY FROM (select acode,QTY from costestimate where branchcd='" + frm_mbr + "' and type='30' and trim(Vchnum)='" + txtlbl42.Text.Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + txtlbl49.Text.Trim() + "' and icode='" + txtlbl47.Text.Trim() + "') WHERE ROWNUM<2", "ACODE");
                oporow3["jstatus"] = "N";
                oporow3["clo_dt"] = vardate;
                oporow3["comments5"] = "-";
                oporow3["splcd"] = "-";
                oporow3["jhold"] = "-";
                oporow3["prc4"] = "-";
                oporow3["enr1"] = 0;
                oporow3["enr2"] = 0;
                oporow3["altitem"] = "-";
                oporow3["eff_wt"] = 0;

                if (edmode.Value == "Y")
                {
                    oporow3["eNt_by"] = ViewState["entby"].ToString();
                    oporow3["eNt_dt"] = ViewState["entdt"].ToString();
                    oporow3["edt_by"] = frm_uname;
                    oporow3["edt_dt"] = vardate;
                }
                else
                {
                    oporow3["eNt_by"] = frm_uname;
                    oporow3["eNt_dt"] = vardate;
                    oporow3["APP_DT"] = vardate;
                    oporow3["AZ_dt"] = vardate;
                    oporow3["edt_by"] = "-";
                    oporow3["edt_dt"] = vardate;
                }

                if (ind_Ptype != "05")
                {
                    oporow3["manpwr"] = txtNumOfOperator.Text.toDouble();
                    oporow3["chgovers"] = txtNumOfHelper.Text.toDouble();
                }
                oDS3.Tables[0].Rows.Add(oporow3);
            }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun4()
    {

    }
    void save_fun6()
    {
        for (i = 0; i < sg3.Rows.Count - 1; i++)
        {
            oporow6 = oDS6.Tables[0].NewRow();


            oporow6["BRANCHCD"] = frm_mbr;
            oporow6["TYPE"] = "15";
            oporow6["vchnum"] = txtvchnum.Text.Trim().ToUpper();
            oporow6["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow6["Acode"] = txtlbl4.Text.Trim();

            oporow6["stage"] = txtlbl4.Text.Trim();
            oporow6["IRATE"] = 0;
            oporow6["REC_ISS"] = "D";
            oporow6["RCODE"] = "-";

            oporow6["FREIGHT"] = txtvchnum.Text;

            oporow6["BTCHNO"] = sg3.Rows[i].Cells[8].Text.Trim().ToUpper();
            oporow6["BTCHDT"] = txtvchdate.Text.Trim().ToUpper();

            oporow6["INVNO"] = txtlbl42.Text.Trim().ToUpper();
            oporow6["INVDATE"] = txtlbl49.Text.Trim().ToUpper();

            oporow6["REFNUM"] = txtlbl42.Text.Trim().ToUpper();
            oporow6["REFDATE"] = txtlbl49.Text.Trim().ToUpper();

            oporow6["DESC_"] = "-";

            oporow6["icode"] = txtlbl47.Text.Trim();//got from plan number

            oporow6["IQTYOUT"] = 0;
            oporow6["IQTYIN"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_f5")).Text.Trim().ToUpper()); ;

            oporow6["STORE"] = "W";
            oporow6["MORDER"] = i;

            if (edmode.Value == "Y")
            {
                oporow6["eNt_by"] = ViewState["entby"].ToString();
                oporow6["eNt_dt"] = ViewState["entdt"].ToString();
                oporow6["edt_by"] = frm_uname;
                oporow6["edt_dt"] = vardate;
            }
            else
            {
                oporow6["eNt_by"] = frm_uname;
                oporow6["eNt_dt"] = vardate;
                oporow6["edt_by"] = "-";
                oporow6["edt_dt"] = vardate;
            }

            oDS6.Tables[0].Rows.Add(oporow6);
        }
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F15101":
                SQuery = "SELECT '60' AS FSTR,'Purchase Request' as NAME,'60' AS CODE FROM dual";
                break;
        }
    }
    //------------------------------------------------------------------------------------   
    protected void sg3_t1_TextChanged(object sender, EventArgs e)
    {
        //fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
        // made logic to get working hours and working minutes
        string quantity = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text;
        string pkt = sg3.Rows[i].Cells[5].Text;
        long tot = Convert.ToInt64(quantity) * Convert.ToInt64(pkt);
        sg3.Rows[i].Cells[6].Text = tot.ToString();
    }
    //------------------------------------------------------------------------------------
    public double sumrejection()
    {
        double sum = 0;
        for (i = 0; i < sg2.Rows.Count - 1; i++)
        {
            sum += fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text);
        }
        return sum;
    }
    //------------------------------------------------------------------------------------
    protected void btnCal_Click(object sender, EventArgs e)
    {
        // CORE Cal
        double range, range1;
        range = fgen.make_double(txtCoreCal.Text);
        range1 = fgen.make_double(fgen.seek_iname(frm_qstr, frm_formID, "select oprate1 from item where trim(icode)='" + sg1.Rows[0].Cells[16].Text.Trim() + "'", "oprate1"));
        if (range == 0)
        {
            range = 1;
        }
        txtlbl9.Text = (Math.Round((range * 3.5 * range1) / 100, 2)).ToString();

        // TRIM Cal
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, "Select maintdt,btchdt,coilnos from inspmst where branchcd='" + frm_mbr + "' and type='70' and trim(icodE)='" + txtlbl47.Text + "'");
        double bd_size, cl_size, fin_size, tot_Gsm, extper1, grssht_wt, netsht_wt, trim_wt;
        if (dt.Rows.Count > 0)
        {
            //pp_Flds = seek_iname3("Select maintdt,btchdt,coilnos from inspmst where branchcd='" & mbr & "' and type='70' and trim(icodE)='" & ticode.text & "'", "maintdt", "btchdt", "coilnos")
            // Dim bd_size As Double
            //  bd_size = Val(SeekN2) / 100
            //Dim cl_size As Double
            //cl_size = Val(seekn1) / 100
            //Dim fin_size As Double
            // fin_size = Val(SeekN3) / 100
            // Dim tot_Gsm As Double
            //Dim rs11 As ADODB.Recordset
            //Set rs11 = New ADODB.Recordset
            //If rs11.RecordCount <= 0 Then
            //Else
            //    Do While rs11.EOF = False
            // If Trim(checknullc(rs11!COL20)) <> "-" Then
            // extper1 = Val(seek_iname("select ACREF FROM TYPEGRP WHERE BRANCHCD!='DD' AND LINENO=" & Val(checknullc(rs11!col21)) & "", "ACREF"))
            //extper=val(Seek_iname("select xx from typegrp where branchcd!='DD' and type1
            // tot_Gsm = tot_Gsm + (Val(rs11!oprate3) * ((100 + extper1) / 100));
            // Else
            // tot_Gsm = tot_Gsm + Val(rs11!oprate3);
            //  End If
            // rs11.MoveNext
            //  Loop
            //End If
            // rs11.close
            //Dim grssht_wt As Double
            //Dim netsht_wt As Double
            //Dim trim_wt As Double
            //b1t6.text = Round(trim_wt * Val(sg.text(0, 3)), 3)

            bd_size = fgen.make_double(dt.Rows[0]["btchdt"].ToString()) / 100;
            cl_size = fgen.make_double(dt.Rows[0]["maintdt"].ToString()) / 100;
            fin_size = fgen.make_double(dt.Rows[0]["coilnos"].ToString()) / 100;
            tot_Gsm = 0;
            dt2 = new DataTable();
            dt2 = fgen.getdata(frm_qstr, frm_cocd, "select a.col9,b.oprate3,nvl(a.col20,'0') as col20,a.col21 from costestimate a, item b where trim(A.col9)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.icode='" + txtlbl47.Text + "' and a.vchnum='" + txtlbl42.Text.Trim() + "' and to_char(a.vchdate,'dd/mm/yyyy')='" + txtlbl49.Text.Trim() + "' and col9 like '07%' order by a.srno");
            if (dt2.Rows.Count > 0)
            {
                if (fgen.make_double(dt2.Rows[0]["col20"].ToString()) != 0)
                {
                    extper1 = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select ACREF FROM TYPEGRP WHERE BRANCHCD!='DD' AND LINENO=" + dt2.Rows[0]["col21"].ToString().Trim() + "'", "ACREF"));
                    tot_Gsm = tot_Gsm + fgen.make_double(dt2.Rows[0]["oprate3"].ToString()) * ((100 + extper1) / 100);
                }
                else
                {
                    tot_Gsm = tot_Gsm + fgen.make_double(dt2.Rows[0]["oprate3"].ToString());
                }
            }
            tot_Gsm = tot_Gsm / 1000;
            grssht_wt = (bd_size * cl_size) * tot_Gsm;
            netsht_wt = (bd_size * fin_size) * tot_Gsm;
            trim_wt = Math.Round(grssht_wt - netsht_wt, 6);
            txtlbl6.Text = (Math.Round(trim_wt * fgen.make_double(((TextBox)(sg3.Rows[0].FindControl("sg3_t1"))).Text), 3)).ToString();
        }
    }
    string space(int count)
    {
        string rs = "";
        for (int i = 0; i < count; i++)
        {
            rs += "&nbsp;";
        }
        return rs;
    }
    protected void btnShowMaterial_Click(object sender, EventArgs e)
    {
        SQuery = "SELECT * FROM (select trim(a.icode) as icode,trim(a.coreelno) as coreelno,trim(a.kclreelno) AS reelno,B.INAME,b.unit ,(sum(a.iqtyin)-sum(a.iqtyout)) AS balance from (SELECT ICODE,SUM(IQTYIN) AS IQTYIN,SUM(IQTYOUT) AS IQTYOUT,KCLREELNO,COREELNO FROM ( SELECT trim(ICODE) as icode,reelwout AS IQTYIN,REELWIN AS IQTYOUT,trim(kclreelno) as kclreelno,'-' as coreelno FROM reelvch WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('31','32','11') AND trim(SUBSTR(ICODE,1,2)) IN ('07','08','09','80','81') AND TRIM(NVL(RINSP_BY,'-'))!='REELOP*' AND 1=1  and trim(job_no)||trim(job_dt)='" + txtlbl43.Text.Trim() + txtlbl14.Text.Trim() + "' UNION ALL SELECT trim(ICODE) as icode,0 AS IQTYIN,is_number(col4) as IQTYOUT,col6,'-' as coreelno FROM costestimate WHERE BRANCHCD='" + frm_mbr + "' and type='25' and trim(enqno)||to_char(enqdt,'dd/mm/yyyy')='" + txtlbl43.Text.Trim() + txtlbl14.Text.Trim() + "') GROUP BY ICODE,KCLREELNO,COREELNO HAVING SUM(IQTYIN)-SUM(IQTYOUT)>0) a,item b where trim(a.icode)=trim(B.icode) group by b.iname,b.cpartno,b.ciname,b.unit,trim(a.icode),trim(a.kclreelno),trim(a.coreelno),b.unit having sum(a.iqtyin)-sum(a.iqtyout)>0  ) ";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            #region for gridview 1
            GridInputTot = 0;
            //txtTotInput.Text = "0";
            if (ViewState["sg1"] != null)
            {
                sg1_dt = new DataTable();
                dt2 = (DataTable)ViewState["sg1"];
                z = dt.Rows.Count - 1;
                sg1_dt = dt2.Clone();
                sg1_dr = null;
                for (int d = 0; d < dt.Rows.Count; d++)
                {
                    sg1_dr = sg1_dt.NewRow();
                    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                    sg1_dr["sg1_h1"] = "-";
                    sg1_dr["sg1_h2"] = "-";
                    sg1_dr["sg1_h3"] = "-";
                    sg1_dr["sg1_h4"] = "-";
                    sg1_dr["sg1_h5"] = "-";
                    sg1_dr["sg1_h6"] = "-";
                    sg1_dr["sg1_h7"] = "-";
                    sg1_dr["sg1_h8"] = "-";
                    sg1_dr["sg1_h9"] = "-";
                    sg1_dr["sg1_h10"] = "-";
                    sg1_dr["sg1_f1"] = "-";
                    sg1_dr["sg1_f2"] = dt.Rows[d]["coreelno"].ToString().Trim();
                    sg1_dr["sg1_f3"] = dt.Rows[d]["unit"].ToString().Trim();
                    sg1_dr["sg1_f4"] = dt.Rows[d]["icode"].ToString().Trim();
                    sg1_dr["sg1_f5"] = dt.Rows[d]["iname"].ToString().Trim();
                    sg1_dr["sg1_f6"] = "1";
                    sg1_dr["sg1_f7"] = dt.Rows[d]["balance"].ToString().Trim();
                    sg1_dr["sg1_f8"] = dt.Rows[d]["reelno"].ToString().Trim();
                    sg1_dr["sg1_f9"] = "-";
                    GridInputTot += fgen.make_double(dt.Rows[d]["balance"].ToString().Trim());
                    sg1_dt.Rows.Add(sg1_dr);
                }
            }
            if (sg1_dt == null) return;
            ViewState["sg1"] = sg1_dt;
            sg1.DataSource = sg1_dt;
            sg1_add_blankrows();
            sg1.DataBind();
            dt.Dispose();
            sg1_dt.Dispose();
            #endregion
            setColHeadings();
        }
    }
    protected void btnprdnrep_Click(object sender, EventArgs e)
    {
        hffield.Value = "ProdnRep";
        fgen.Fn_open_prddmp1("-", frm_qstr);
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("Select shift ", frm_qstr);
    }
    protected void btnchkstage_Click(object sender, EventArgs e)
    {
        hffield.Value = "ChkStage";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Check Stage", frm_qstr);
    }
    protected void btnsprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SPrint";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
    }
    public void Fn_ValueBox(string titl, string QR_str)
    {
        hffield.Value = "QTYBOX";
        fgenMV.Fn_Set_Mvar(QR_str, "U_BOXTYPE", "ITEM");
        if (HttpContext.Current.CurrentHandler is Page)
        {
            string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/ival_prod.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle1('" + fil_loc + "?STR=" + QR_str + "','250px','200px','Enter Production Rolls');", true);
        }
    }
    double CalculateRMCcost()
    {
        rm_Val = 0;
        string mrt1 = "";
        xprdrange = "BETWEEN TO_DATE('" + fromdt + "','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')";
        OutputQty = 0;
        foreach (GridViewRow gr3 in sg3.Rows)
        {
            if (gr3.Cells[3].Text.Trim().Substring(0, 1) == "9")
                mrt1 = fgen.seek_iname(frm_qstr, frm_cocd, "select irate from " + frm_tabname1 + " where branchcd='" + frm_mbr + "' and type='40' and trim(icode)='" + gr3.Cells[3].Text.Trim() + "' and trim(col6)='" + gr3.Cells[8].Text.Trim() + "'", "irate");
            else
                mrt1 = fgen.seek_iname(frm_qstr, frm_cocd, "select * From (select (Case when nvl(a.ichgs,a.irate)=0 then nvl(b.iqd,0) else nvl(a.ichgs,a.irate) end )  as irate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.vchdate<=to_DaTE('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy') and a.vchdate " + xprdrange + " and a.irate>0 and a.store='Y' and substr(a.type,1,1)='0' and trim(a.icode)='" + gr3.Cells[3].Text.Trim() + "' order by a.vchdate desc) where rownum<2 ", "irate");

            if (mrt1.toDouble() == 0)
                mrt1 = fgen.seek_iname(frm_qstr, frm_cocd, "select (Case when nvl(iqd,irate)=0 then irate else iqd end) as irate from item where trim(icode)='" + gr3.Cells[3].Text.Trim() + "'", "irate");

            OutputQty += ((TextBox)gr3.FindControl("sg3_f5")).Text.toDouble();


            //if (mrt1.toDouble() == 0)
            //    gr3.BackColor = System.Drawing.Color.LightPink;

            //rstmp!irate = Val(mrt1)
            //If megh_box = "Y" And Left(Trim(sg2.text(i, 1)), 2) = "02" Then
            //    Dim unit_wt1 As String
            //    unit_wt1 = seek_iname("select iweight from item where trim(icode)='" & Trim(sg2.text(i, 1)) & "'", "iweight")
            //    rm_Val = rm_Val + (rstmp!irate * (rstmp!itate * Val(unit_wt1)))
            //Else
            rm_Val = rm_Val + (mrt1.toDouble() * 1);
        }
        return rm_Val;
    }
    double do_fetch_by_prod_Val()
    {
        string scrprod1, by_prod1, by_prod2;
        double scrprod_val = 0, by_prod1_val = 0, by_prod2_val = 0, doubleResult = 0;

        if (txtRcylScrap.Text.toDouble() > 0)
        {
            scrprod1 = fgen.seek_iname(frm_qstr, frm_cocd, "select scrp_rcy from item where trim(icode)='" + txtlbl47.Text.Trim() + "'", "scrp_rcy");
            scrprod1 = fgen.seek_iname(frm_qstr, frm_cocd, "select irate from item where trim(icode)='" + scrprod1 + "'", "irate");
            scrprod_val = txtRcylScrap.Text.toDouble() * scrprod1.toDouble();
        }

        if (txtByProdInk.Text.toDouble() > 0)
        {
            by_prod1 = fgen.seek_iname(frm_qstr, frm_cocd, "Select params from controls where id='M84'", "params");
            by_prod1 = fgen.seek_iname(frm_qstr, frm_cocd, "select irate from item where trim(icode)='" + by_prod1 + "'", "irate");
            by_prod1_val = txtByProdInk.Text.toDouble() * by_prod1.toDouble();
        }

        if (txtByProdThin.Text.toDouble() > 0)
        {
            by_prod2 = fgen.seek_iname(frm_qstr, frm_cocd, "Select params from controls where id='M85'", "params");
            by_prod2 = fgen.seek_iname(frm_qstr, frm_cocd, "select irate from item where trim(icode)='" + by_prod2 + "'", "irate");
            by_prod2_val = txtByProdThin.Text.toDouble() * by_prod2.toDouble();
        }

        doubleResult = Math.Round(scrprod_val + by_prod1_val + by_prod2_val, 2);
        return doubleResult;
    }
    void CalculateManPowerCost()
    {
        string pickmrc = "", timeDiff = "";
        double mchCost = 0, ovhCost = 0, oprCost = 0, hlpCost = 0, rmCostHR = 0, ohCostHR = 0, opr_hr_rt = 0, hlpr_hr_rt = 0, rmc_Cost = 0, by_prod_val = 0, icodeVal = 0;
        timeDiff = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT round(TO_dATE('" + txtvchdate.Text + " " + txtlbl51.Text.Trim() + "','dd/mm/yyyy HH24:mi')-TO_dATE('" + txtvchdate.Text + " " + txtlbl50.Text.Trim() + "','dd/mm/yyyy HH24:mi'),2)*24*60 as cal from dual ", "cal");

        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "Select rcost_hr||'~'||ohcost_hr as fstr from pmaint where branchcd='" + frm_mbr + "' and type='10' and trim(acode)||'/'||srno='" + txtlbl40.Text.Trim() + "'", "fstr");
        if (col1 != "0" && col1 != "~")
        {
            rmCostHR = col1.Split('~')[0].toDouble();
            ohCostHR = col1.Split('~')[1].toDouble();
        }

        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NUM5||'~'||NUM6 AS FSTR FROM TYPEGRP WHERE BRANCHCD='" + frm_mbr + "' AND ID='WI' AND TRIM(ACREF)='" + txtlbl4.Text.Trim() + "' ", "FSTR");
        if (col1 != "0" && col1 != "~")
        {
            opr_hr_rt = col1.Split('~')[0].toDouble();
            hlpr_hr_rt = col1.Split('~')[1].toDouble();
        }

        rmc_Cost = CalculateRMCcost();

        mchCost = Math.Round((timeDiff.toDouble() / 60) * rmCostHR, 6);
        ovhCost = Math.Round((timeDiff.toDouble() / 60) * ohCostHR, 6);

        oprCost = Math.Round((timeDiff.toDouble() / 60) * opr_hr_rt, 6);
        hlpCost = Math.Round((timeDiff.toDouble() / 60) * hlpr_hr_rt, 6);

        rm_Val = Math.Round(rmc_Cost + mchCost + ovhCost + (oprCost * txtNumOfOperator.Text.toDouble()) + (hlpCost * txtNumOfHelper.Text.toDouble()), 2);

        by_prod_val = do_fetch_by_prod_Val();

        rm_Val = rm_Val - by_prod_val;

        icodeVal = (rm_Val / OutputQty);

        fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE ITEM SET IQD='" + icodeVal + "' WHERE TRIM(ICODE)='" + txtlbl47.Text.Trim() + "' ");

        SQuery = "update " + frm_tabname1 + " set irate=" + icodeVal + ",num1=0,num2=0,num3=0,num4=0,num5=0,time1=0 where branchcd='" + frm_mbr + "' and trim(col21)='" + txtlbl4.Text.Trim() + "' and vchnum='" + txtvchnum.Text.Trim() + "' and vchdate=to_date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy') and type='40'";
        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
        SQuery = "update " + frm_tabname1 + " set num1=" + mchCost + ",num2=" + ovhCost + ",num3=" + oprCost + ",num4=" + hlpCost + ",num5=" + rmc_Cost + ",time1=" + timeDiff + ",num6=" + by_prod_val + " where branchcd='" + frm_mbr + "' and trim(col21)='" + txtlbl4.Text.Trim() + "' and vchnum='" + txtvchnum.Text + "' and vchdate=to_date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy') and type='40' and srno=0";
        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
    }
}