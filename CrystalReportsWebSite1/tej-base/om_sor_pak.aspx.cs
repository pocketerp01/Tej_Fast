using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_sor_pak : System.Web.UI.Page
{
    string btnval, SQuery, SQuery1, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", xprdrange1, mq2, PDateRange;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    double checkOkQty = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_tabname1, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2, party_cd, part_cd;
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
                    //frm_mbr = "01";
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                doc_addl.Value = "1";
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();

            typePopup = "N";
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


        if (dtCol == null) return;
        if (sg1.Rows.Count <= 0) return;
        for (int sR = 0; sR < sg1.Columns.Count; sR++)
        {
            string orig_name;
            double tb_Colm;
            tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
            orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();

            for (int K = 0; K < sg1.Rows.Count; K++)
            {
                #region hide hidden columns
                for (int i = 0; i < 10; i++)
                {
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[K].Cells[i].CssClass = "hidden";
                }
                #endregion
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

            }
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
        tab5.Visible = false;
        tab4.Visible = false;
        tab3.Visible = false;
        tab2.Visible = true;

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
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();

        btnlbl4.Enabled = false;
        btnlbl7.Enabled = false;

        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        fetch_col_earn();
        // fetch_col_downtime();
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
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
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "costestimate";
        frm_tabname1 = "ivoucher";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "60");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL5", frm_tabname1);
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
            case "SHFTCODE":
                SQuery = "select  type1 as fstr,NAME,place,type1 from type where id='D' and substr(type1,1,1)='1' order by name";
                break;
            case "DEPTTCODE":
                SQuery = "select type1 as fstr, NAME as Deptt,Type1 as Code from type where id='M' and substr(type1,1,1)='6' and (type1='64' or name like '%PROD%' or name like '%CORR%') order by name ";
                break;
            case "TICODE":

                //Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                //string pquery;
                //switch (Prg_Id)
                //{
                //    case "F30101":
                //        pquery = "select trim(icode) as icode,sum(cnt) as tot from (select icode,1 as cnt from item where length(trim(nvl(deac_by,'-')))<=1 and length(trim(icode))>4 union all select distinct icode,-1 as cnt from inspmst where branchcd!='DD' and type='" + lbl1a.Text + "') group by trim(icode) having sum(cnt)>0 ";
                //        SQuery = "SELECT a.Icode AS FSTR,trim(b.Iname) as Item_name,a.Icode,b.Cpartno,b.Cdrgno,b.unit from ("+ pquery +")a ,Item b where trim(A.icode)=trim(B.icode) and length(trim(nvl(b.deac_by,'-')))<=1 and length(trim(b.icode))>4 and substr(b.icode,1,1) < ('9') order by b.iname";
                //        break;
                //    case "F30106":
                //        pquery = "select trim(icode) as icode,sum(cnt) as tot from (select icode,1 as cnt from item where length(trim(nvl(deac_by,'-')))<=1 and length(trim(icode))>4 union all select distinct icode,-1 as cnt from inspmst where branchcd!='DD' and type='" + lbl1a.Text + "') group by trim(icode) having sum(cnt)>0 ";
                //        SQuery = "SELECT a.Icode AS FSTR,trim(b.Iname) as Item_name,a.Icode,b.Cpartno,b.Cdrgno,b.unit from ("+ pquery +")a ,Item b where trim(A.icode)=trim(B.icode) and length(trim(nvl(b.deac_by,'-')))<=1 and length(trim(b.icode))>4 and substr(b.icode,1,1) in ('7','9') order by b.iname";
                //        break;
                //    case "F30111":
                //        pquery = "select trim(icode) as icode,sum(cnt) as tot from (select icode,1 as cnt from item where length(trim(nvl(deac_by,'-')))<=1 and length(trim(icode))>4 union all select distinct icode,-1 as cnt from inspmst where branchcd!='DD' and type='" + lbl1a.Text + "' and trim(Acode)='" + txtlbl4.Text.Trim() + "') group by trim(icode) having sum(cnt)>0 ";
                //        SQuery = "SELECT a.Icode AS FSTR,trim(b.Iname) as Item_name,a.Icode,b.Cpartno,b.Cdrgno,b.unit from ("+ pquery +")a ,Item b where trim(A.icode)=trim(B.icode) and length(trim(nvl(b.deac_by,'-')))<=1 and length(trim(b.icode))>4 and substr(b.icode,1,1) in ('7','8','9') order by b.iname";
                //        break;
                //}
                SQuery = "select trim(acode)||'/'||srno as fstr,mchname as Machine_Name,trim(acode)||'/'||srno as Machine_Code,mch_seq from pmaint where branchcd='05' and type='10' order by acode,srno";
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                //SQuery = "select a.type1 as fstr,A.NAME,A.type1,B.CNT AS ITEMS from type A,(select DISTINCT stagec,count(icode) AS CNT from itwstage  GROUP BY STAGEC) B where A.id='K' AND A.TYPE1=B.STAGEC order by A.TYPE1";
                //original  SQuery = "select distinct a.vchnum as fstr,B.INAME as Item_Name,b.cpartno as Part_No,a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,a.acode,a.status,a.JSTATUS from costestimate A,ITEM B  WHERE a.vchdate  " + DateRange + "  and A.SRNO=0 AND trim(A.ICODE)=trim(B.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='30' and trim(nvl(a.app_by,'-'))<>'-' and nvl(a.status,'N')<>'Y' and nvl(a.jstatus,'N')<>'Y' and trim(nvl(a.enqno,'N'))<>'Y' and b.pageno=1 order by A.vchdate desc ,A.vchnum desc";
                // SQuery = "select distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,B.INAME as Item_Name,b.cpartno as Part_No,a.icode as ERP_Code,A.vchnum AS Job_No,to_char(A.vchdate,'dd/mm/yyyy') as Dated,A.QTY as Job_Qty,a.acode,a.status,a.JSTATUS from costestimate A,ITEM B  WHERE  trim(A.ICODE)=trim(B.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='30' and trim(nvl(a.app_by,'-'))<>'-' and nvl(a.status,'N')<>'Y' and nvl(a.jstatus,'N')<>'Y' and trim(nvl(a.enqno,'N'))<>'Y' and b.pageno=1 AND A.SRNO=0 order by dated desc ,A.vchnum desc";
                PDateRange = "between to_date('" + frm_CDT1 + "','dd/mm/yyyy')-200 and to_date('" + frm_CDT2 + "','dd/mm/yyyy')";
                if (sg1.Rows.Count > 1)
                {
                    col1 = "";
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        if (col1.Length > 0) col1 = col1 + ",'" + ((TextBox)(gr.FindControl("sg1_t1"))).Text.Trim() + gr.Cells[16].Text.Trim() + gr.Cells[17].Text.Trim() + "'";
                        else col1 = "'" + ((TextBox)(gr.FindControl("sg1_t1"))).Text.Trim() + gr.Cells[16].Text.Trim() + gr.Cells[17].Text.Trim() + "'";
                    }
                    SQuery = "select trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy') as fstr, b.iname,b.cpartno,a.erp_Code,a.job_no,to_char(a.Dated,'dd/mm/yyyy') as dated,sum(a.Job_Qty) as Job_qty,a.acode,sum(a.prodn) as Prodn_qty,MAX(A.PRODDT) AS PROD_DT from (select a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,0 as prodn,a.acode,null as proddt from costestimate A WHERE  a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + PDateRange + " and trim(a.icode)||trim(a.vchnum)||to_char(A.vchdate,'dd/mm/yyyy') not in (" + col1 + ") and A.SRNO=0 AND trim(nvl(a.status,'N'))<>'Y' union all select a.icode as ERP_Code,A.enqno AS Job_No,A.enqdt as Dated,0 as Job_Qty,to_number(a.col4) as prodn,a.acode,A.VCHDATE from costestimate A WHERE a.branchcd='" + frm_mbr + "' and a.type='60' and a.vchdate  " + PDateRange + " and trim(a.icode)||trim(a.vchnum)||to_char(A.vchdate,'dd/mm/yyyy') not in (" + col1 + ") )a, item b where trim(A.erp_Code)=trim(B.icode) group by b.iname,b.cpartno,a.erp_Code,a.job_no,a.dated,a.acode having sum(a.Job_Qty)-sum(a.prodn)>0 order by b.iname";
                }
                else
                {
                    SQuery = "select trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy') as fstr, b.iname,b.cpartno,a.erp_Code,a.job_no,to_char(a.Dated,'dd/mm/yyyy') as dated,sum(a.Job_Qty) as Job_qty,a.acode,sum(a.prodn) as Prodn_qty,MAX(A.PRODDT) AS PROD_DT from (select a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,0 as prodn,a.acode,null as proddt from costestimate A WHERE  a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + PDateRange + " and A.SRNO=0 AND trim(nvl(a.status,'N'))<>'Y' union all select a.icode as ERP_Code,A.enqno AS Job_No,A.enqdt as Dated,0 as Job_Qty,to_number(a.col4) as prodn,a.acode,A.VCHDATE from costestimate A WHERE a.branchcd='" + frm_mbr + "' and a.type='60' and a.vchdate  " + PDateRange + " )a, item b where trim(A.erp_Code)=trim(B.icode) group by b.iname,b.cpartno,a.erp_Code,a.job_no,a.dated,a.acode having sum(a.Job_Qty)-sum(a.prodn)>0 order by b.iname";

                }
                break;

            case "SG1_ROW_ADD1":
            case "SG1_ROW_ADD_E1":
                string stage = "0";
                stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text;
                SQuery = "select distinct trim(a.Icode)||'.'||trim(a.vchnum) as fstr, '['||trim(a.COL16)||' Clr]'||trim(b.Iname) as Item_Name,trim(a.Icode)||'.'||trim(a.vchnum) as Item_Code,b.Cpartno as Part_No,d.aname as Customer,a.ENQDT as Delv_Dt,a.vchnum as Job_No,a.col18||'X'||a.col19 as Cut_Size from costestimate a, item b,itwstage c,famst d where trim(nvl(a.app_by,'-'))!='-' and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(d.acode) and a.type='30' and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.status='N' and c.stagec='" + stage + "' order by trim(a.Icode)||'.'||trim(a.vchnum)";
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
                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(Trim(icode))>4 and icode like '9%' and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname  ";
                break;


            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            case "Print_E":
            case "IDTags":
                // SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Report_no,to_char(a.vchdate,'dd/mm/yyyy') as Report_Dt,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,B.ANAME,to_Char(a.vchdate,'yyyymmdd') as vdd  from " + frm_tabname + " a ,FAMST B  where TRIM(A.ACODE)=TRIM(B.ACODE) AND a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' order by vdd desc,a.vchnum desc";
                SQuery = "select distinct TRIM(A.BRANCHCD)||TRIM(A.TYPE)||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Report_no,to_char(a.vchdate,'dd/mm/yyyy') as Report_Dt,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,B.ANAME,to_Char(a.vchdate,'yyyymmdd') as vdd  from costestimate a ,FAMST B  where TRIM(A.ACODE)=TRIM(B.ACODE) AND a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                break;

            case "ProdRep":
                SQuery = "SELECT TYPE1 AS FSTR,TRIM(NAME) AS SERIES_NAME ,TRIM(TYPE1)  AS CODE from type where ID='M' AND  TYPE1 > '14' AND TYPE1 <'20' order by type1";
                break;
            case "SOrd":
                SQuery = "select '1'as fstr ,'Item Name' as ord , '1' as sno from dual union all select '2'as fstr ,'Rej%' as ord , '2' as sno from dual union all select '3'as fstr ,'Rej Cost' as ord , '3' as sno from dual";
                break;
            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                {
                    // SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Report_no,to_char(a.vchdate,'dd/mm/yyyy') as Report_Dt,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,B.ANAME,to_Char(a.vchdate,'yyyymmdd') as vdd  from " + frm_tabname + " a ,FAMST B  where TRIM(A.ACODE)=TRIM(B.ACODE) AND a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.type,B.ANAME,a.convdate,a.ent_by,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a ,FAMST B  where TRIM(A.ACODE)=TRIM(B.ACODE) AND a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                }
                break;
        }
        if (typePopup == "N" && (btnval == "Edit" || btnval == "Del" | btnval == "Print"))
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

            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

            frm_vty = "60";
            lbl1a.Text = frm_vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);

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
            //fgen.EnableForm(this.Controls);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        // frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");
        frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl2.Text = frm_uname;
        txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl5.Text = "-";
        txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();

        sg1_dt = new DataTable();
        create_tab();
        int j;
        //for (j = i; j < 10; j++)
        //{
        //    sg1_add_blankrows();
        //}

        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        fetch_col_earn();
        //  fetch_col_downtime();
        ViewState["sg1"] = sg1_dt;
        // Popup asking for Copy from Older Data
        //fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        //hffield.Value = "NEW_E";        

        #endregion
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
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
        string chk_freeze = "";
        //chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1043", txtvchdate.Text.Trim());
        if (chk_freeze == "1")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Rolling Freeze Date !!");
            return;
        }
        if (chk_freeze == "2")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Fixed Freeze Date !!");
            return;
        }

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }


        if (txtlbl4.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill " + lbl4.Text);
            return;
        }

        if (txtlbl7.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill " + lbl7.Text);
            return;
        }

        if (sg1.Rows.Count <= 1)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Select Item");
            return;
        }

        for (int i = 0; i < sg1.Rows.Count - 1; i++)
        {
            checkOkQty = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t4"))).Text.Trim());
            if (checkOkQty == 0)
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill Ok Qty At Line No. " + sg1.Rows[i].Cells[13].Text + "");
                return;
            }
        }

        //string mandField = "";
        //mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        //if (mandField.Length > 1)
        //{
        //    fgen.msg("-", "AMSG", mandField);
        //    return;
        //}

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        btnsave.Disabled = true;
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
        fetch_col_earn();
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
        //SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + DateRange + " and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
        //fgen.Fn_FillChart(frm_cocd, frm_qstr, "Testing Chart", "line", "Main Heading", "Sub Heading", SQuery, "");
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
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
        //frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_vty = "60";
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");


                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + "Delete");
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

                            sg1_dr["sg1_t1"] = dt.Rows[i]["OBJ_NAME"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["OBJ_CAPTION"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["OBJ_WIDTH"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["OBJ_VISIBLE"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["col_no"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["obj_maxlen"].ToString().Trim();
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
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

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

                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt,to_char(a.enqdt,'dd/mm/yyyy') as enq_dt from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    SQuery1 = "Select a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.mtime,a.binno from ivoucher a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + "16" + col1 + "' ORDER BY A.SRNO";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery1);

                    fromdt = vardate;
                    DateRange = "BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + frm_CDT2 + "','DD/MM/YYYY')-1";
                    xprdrange1 = "BETWEEN TO_DATE('" + fromdt + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    mq2 = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' and icode like '9%' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store='Y' and icode like '9%' GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + DateRange + " and store='Y' and icode like '9%' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8  group by branchcd,trim(icode) ORDER BY ICODE";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);

                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl2.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        txtlbl3.Text = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl5.Text = dt.Rows[0]["eDt_by"].ToString().Trim();
                        txtlbl6.Text = Convert.ToDateTime(fgen.make_def_Date(dt.Rows[0]["edt_dt"].ToString().Trim(), vardate)).ToString("dd/MM/yyyy");
                        txtlbl4.Text = dt.Rows[0]["attach3"].ToString().Trim();
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select NAME from type where id='D' and substr(type1,1,1)='1' AND TYPE1='" + dt.Rows[i]["attach3"].ToString().Trim() + "' order by name", "NAME");// get it from seekname
                        txtlbl7.Text = dt.Rows[0]["dropdate"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select NAME from type where id='M'  and type1 like '6%' AND TYPE1='" + dt.Rows[i]["dropdate"].ToString().Trim() + "' order by name", "NAME");// get it from seekname
                        txtrmk.Text = dt.Rows[0]["remarks"].ToString().Trim();
                        doc_addl.Value = dt.Rows[0]["srno"].ToString().Trim();

                        if (dt3.Rows.Count > 0)
                        {
                            txtSort.Text = dt3.Rows[0]["mtime"].ToString().Trim();
                            txtPack.Text = dt3.Rows[0]["binno"].ToString().Trim();
                        }
                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
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
                            sg1_dr["sg1_f1"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "CL");
                            sg1_dr["sg1_f2"] = dt.Rows[i]["comments"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["enqno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["enq_dt"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["col2"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["col5"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["itate"].ToString().Trim();
                            //  sg1_dr["sg1_t7"] = dt.Rows[i]["re"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["col6"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["col7"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["col8"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["col9"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["col10"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["col11"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["col12"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["col13"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["col14"].ToString().Trim();
                            sg1_dr["sg1_t17"] = dt.Rows[i]["col15"].ToString().Trim();

                            sg1_dr["sg1_t18"] = dt.Rows[i]["col16"].ToString().Trim();
                            sg1_dr["sg1_t19"] = dt.Rows[i]["col17"].ToString().Trim();
                            sg1_dr["sg1_t20"] = dt.Rows[i]["col18"].ToString().Trim();
                            sg1_dr["sg1_t21"] = dt.Rows[i]["col19"].ToString().Trim();
                            sg1_dr["sg1_t22"] = dt.Rows[i]["col20"].ToString().Trim();
                            sg1_dr["sg1_t23"] = dt.Rows[i]["col21"].ToString().Trim();
                            sg1_dr["sg1_t24"] = dt.Rows[i]["col22"].ToString().Trim();
                            sg1_dr["sg1_t25"] = dt.Rows[i]["col23"].ToString().Trim();
                            sg1_dr["sg1_t26"] = dt.Rows[i]["col24"].ToString().Trim();
                            sg1_dr["sg1_t27"] = dt.Rows[i]["col25"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        fetch_col_earn();
                        //  fetch_col_downtime();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    //SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    //fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40112");
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;

                case "IDTags":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F40112A");
                    fgen.fin_prodpp_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    //SQuery = "Select b.iname,b.cpartno,b.cdrgno,b.unit,trim(a.srno) as morder1,a.*,to_chaR(a.invdate,'dd/mm/yyyy') as pinvdate,to_chaR(a.vchdate,'dd/mm/yyyy') as pvchdate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum||trim(a.icode)||trim(a.srno)='" + col1 + "' ORDER BY A.srno";
                    SQuery = "select  EMPCODE,NAME, DEPTT_TEXT,DESG_TEXT,DTJOIN from empmas  where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + col1 + "' order by grade";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl4.Text = col1;
                        txtlbl4a.Text = col2;
                    }
                    dt.Dispose();
                    // SQuery = "Select * from inspmst a where a.branchcd='" + frm_mbr + "' and a.icode='" + txtlbl7.Text + "' ORDER BY A.srno";
                    SQuery = "select  EMPCODE AS COL1,NAME AS COL2, DEPTT_TEXT AS COL3,DESG_TEXT AS COL4,TO_CHAR(DTJOIN,'dd/MM/yyyy') AS COL6,ENT_DT,ENT_BY from empmas  where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + col1 + "' order by grade";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
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
                            sg1_dr["sg1_srno"] = i + 1;
                            sg1_dr["sg1_f1"] = dt.Rows[i]["col1"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["col2"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["col6"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }


                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        //edmode.Value = "Y";
                    }
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

                    //if (col1.Length <= 0) return;
                    //txtlbl7.Text = col1;
                    //txtlbl7a.Text = col2;
                    //txtlbl2.Focus();
                    break;
                case "MRESULT":

                    if (col1.Length <= 0) return;
                    txtlbl101.Text = col1;
                    txtlbl101a.Text = col2;
                    break;

                case "SHFTCODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    txtlbl101.Text = col3;
                    btnlbl7.Focus();
                    break;

                case "DEPTTCODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
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
                            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.ToString();
                            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.ToString();
                            sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.ToString();
                            sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.ToString();
                            sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.ToString();
                            sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.ToString();
                            sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.ToString();
                            sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.ToString();
                            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.ToString();
                            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.ToString();
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[14].Text.ToString();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[15].Text.ToString();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[16].Text.ToString();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[17].Text.ToString();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[18].Text.ToString();
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                            sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                            sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                            sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();
                            sg1_dr["sg1_t19"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                            sg1_dr["sg1_t20"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim();
                            sg1_dr["sg1_t21"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();
                            sg1_dr["sg1_t22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim();
                            sg1_dr["sg1_t23"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim();
                            sg1_dr["sg1_t24"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text.Trim();
                            sg1_dr["sg1_t25"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text.Trim();
                            sg1_dr["sg1_t26"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text.Trim();
                            sg1_dr["sg1_t27"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t27")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        PDateRange = "between to_date('" + frm_CDT1 + "','dd/mm/yyyy')-200  and to_date('" + frm_CDT2 + "','dd/mm/yyyy')";
                        dt = new DataTable();
                        // SQuery = "select distinct a.vchnum as fstr,B.INAME as Item_Name,b.cpartno as Part_No,a.icode as ERP_Code,A.vchnum AS Job_No,to_char(A.vchdate,'dd/mm/yyyy') as Dated,A.QTY as Job_Qty,a.acode,a.status,a.JSTATUS from costestimate A,ITEM B  WHERE trim(A.ICODE)=trim(B.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='30' and trim(nvl(a.app_by,'-'))<>'-' and nvl(a.status,'N')<>'Y' and nvl(a.jstatus,'N')<>'Y' and trim(nvl(a.enqno,'N'))<>'Y' and b.pageno=1 and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' and A.SRNO=0  order by Dated desc ,A.vchnum desc"; ;
                        SQuery = "select trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy') as fstr, b.iname as Item_Name,b.cpartno as Part_No,a.erp_Code,a.job_no,to_char(a.Dated,'dd/mm/yyyy') as dated,sum(a.Job_Qty) as Job_qty,a.acode,sum(a.prodn) as Prodn_qty,MAX(A.PRODDT) AS PROD_DT from (select a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,0 as prodn,a.acode,null as proddt from costestimate A WHERE  a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + PDateRange + " and A.SRNO=0 AND trim(nvl(a.status,'N'))<>'Y' union all select a.icode as ERP_Code,A.enqno AS Job_No,A.enqdt as Dated,0 as Job_Qty,to_number(a.col4) as prodn,a.acode,A.VCHDATE from costestimate A WHERE a.branchcd='" + frm_mbr + "' and a.type='60' and a.vchdate  " + PDateRange + " )a, item b where  trim(A.erp_Code)=trim(B.icode) and trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy')='" + col1 + "' group by b.iname,b.cpartno,a.erp_Code,a.job_no,a.dated,a.acode having sum(a.Job_Qty)-sum(a.prodn)>0 order by b.iname";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        fromdt = vardate;
                        DateRange = "BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + frm_CDT2 + "','DD/MM/YYYY')-1";
                        xprdrange1 = "BETWEEN TO_DATE('" + fromdt + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                        mq2 = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' and icode like '9%' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store='Y' and icode like '9%' GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + DateRange + " and store='Y' and icode like '9%' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8  group by branchcd,trim(icode) ORDER BY ICODE";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";
                            sg1_dr["sg1_f1"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[d]["erp_code"].ToString().Trim() + "'", "CL");
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = dt.Rows[d]["Job_No"].ToString().Trim();
                            sg1_dr["sg1_f4"] = Convert.ToDateTime(dt.Rows[d]["dated"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_t1"] = dt.Rows[d]["ERP_Code"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[d]["Item_Name"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[d]["Part_No"].ToString().Trim();
                            string acode = fgen.seek_iname(frm_qstr, frm_cocd, "select * from (select acode,max(irate) as irt from somas where branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and icat<>'Y' and trim(icode)='" + dt.Rows[d]["ERP_Code"].ToString().Trim() + "' group by acode) order by irt desc", "acode");
                            sg1_dr["sg1_t6"] = fgen.seek_iname(frm_qstr, frm_cocd, "select * from (select acode,max(irate) as irt from somas where branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and icat<>'Y' and trim(icode)='" + dt.Rows[d]["ERP_Code"].ToString().Trim() + "' group by acode) order by irt desc", "irt");
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t4")).Focus();
                    #endregion
                    setColHeadings();
                    fetch_col_earn();
                    //fetch_col_downtime();
                    break;
                case "SG1_ROW_ADD_E":
                    dt = new DataTable();
                    PDateRange = "between to_date('" + frm_CDT1 + "','dd/mm/yyyy')-200 and to_date('" + frm_CDT2 + "','dd/mm/yyyy')";
                    //SQuery = "select distinct a.vchnum as fstr,B.INAME as Item_Name,b.cpartno as Part_No,a.icode as ERP_Code,A.vchnum AS Job_No,to_char(A.vchdate,'dd/mm/yyyy') as Dated,A.QTY as Job_Qty,a.acode,a.status,a.JSTATUS from costestimate A,ITEM B  WHERE trim(A.ICODE)=trim(B.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='30' and trim(nvl(a.app_by,'-'))<>'-' and nvl(a.status,'N')<>'Y' and nvl(a.jstatus,'N')<>'Y' and trim(nvl(a.enqno,'N'))<>'Y' and b.pageno=1 and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' and A.SRNO=0  order by Dated desc ,A.vchnum desc";
                    SQuery = "select trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy') as fstr,a.ERP_Code, b.iname as Item_Name,b.cpartno,a.erp_Code as Part_No,a.job_no,to_char(a.Dated,'dd/mm/yyyy') as dated,sum(a.Job_Qty) as Job_qty,a.acode,sum(a.prodn) as Prodn_qty,MAX(A.PRODDT) AS PROD_DT from (select a.icode as ERP_Code,A.vchnum AS Job_No,A.vchdate as Dated,A.QTY as Job_Qty,0 as prodn,a.acode,null as proddt from costestimate A WHERE  a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + PDateRange + " and A.SRNO=0 AND trim(nvl(a.status,'N'))<>'Y' union all select a.icode as ERP_Code,A.enqno AS Job_No,A.enqdt as Dated,0 as Job_Qty,to_number(a.col4) as prodn,a.acode,A.VCHDATE from costestimate A WHERE a.branchcd='" + frm_mbr + "' and a.type='60' and a.vchdate  " + PDateRange + " )a, item b where  trim(A.erp_Code)=trim(B.icode) and trim(a.job_no)||to_char(a.Dated,'dd/mm/yyyy')='" + col1 + "' group by b.iname,b.cpartno,a.erp_Code,a.job_no,a.dated,a.acode having sum(a.Job_Qty)-sum(a.prodn)>0 order by b.iname";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    fromdt = vardate;
                    DateRange = "BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + frm_CDT2 + "','DD/MM/YYYY')-1";
                    xprdrange1 = "BETWEEN TO_DATE('" + fromdt + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                    mq2 = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' and icode like '9%' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store='Y' and icode like '9%' GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + DateRange + " and store='Y' and icode like '9%' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8  group by branchcd,trim(icode) ORDER BY ICODE";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                    if (dt.Rows.Count > 0)
                    {
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[0]["Job_No"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[0]["dated"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = dt.Rows[0]["ERP_Code"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = dt.Rows[0]["Item_Name"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = dt.Rows[0]["Part_No"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t6")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select * from (select acode,max(irate) as irt from somas where branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and icat<>'Y' and trim(icode)='" + dt.Rows[0]["ERP_Code"].ToString().Trim() + "' group by acode) order by irt desc", "irt");
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[0]["erp_code"].ToString().Trim() + "'", "CL");
                        setColHeadings();
                        //fetch_col_earn();
                    }
                    break;

                case "SG1_ROW_ADD1":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    //if (ViewState["sg1"] != null)
                    //{
                    //    dt = new DataTable();
                    //    sg1_dt = new DataTable();
                    //    dt = (DataTable)ViewState["sg1"];
                    //    z = dt.Rows.Count - 1;
                    //    sg1_dt = dt.Clone();
                    //    sg1_dr = null;
                    //    for (i = 0; i < dt.Rows.Count - 1; i++)
                    //    {
                    //        sg1_dr = sg1_dt.NewRow();
                    //        sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                    //        sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
                    //        sg1_dr["sg1_h2"] = dt.Rows[i]["sg1_h2"].ToString();
                    //        sg1_dr["sg1_h3"] = dt.Rows[i]["sg1_h3"].ToString();
                    //        sg1_dr["sg1_h4"] = dt.Rows[i]["sg1_h4"].ToString();
                    //        sg1_dr["sg1_h5"] = dt.Rows[i]["sg1_h5"].ToString();
                    //        sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                    //        sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                    //        sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                    //        sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                    //        sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();

                    //        sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                    //        sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                    //        sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                    //        sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                    //        sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                    //        sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                    //        sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                    //        sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                    //        sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                    //        sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                    //        sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                    //        sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                    //        sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                    //        sg1_dt.Rows.Add(sg1_dr);
                    //}
                    string stage = "0";
                    stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text;
                    dt = new DataTable();
                    //if (col1.Length > 6) SQuery = "select * from evas where trim(userid) in (" + col1 + ")";
                    //else SQuery = "select * from evas where trim(userid)='" + col1 + "'";
                    SQuery = "select distinct b.Iname as iname,a.Icode as iCode,b.Cpartno,a.vchnum,a.qty,to_char(a.vchdate,'dd/mm/yyyy')as vchdate,trim(a.Icode)||'.'||trim(a.vchnum) as fstr,a.col17 from costestimate a, item b,itwstage c where trim(a.icode)=trim(b.icode) and a.type='30' and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.status='N' and c.stagec='" + stage + "' and trim(a.Icode)||'.'||trim(a.vchnum) in (" + col1 + ") order by trim(a.Icode)||'.'||trim(a.vchnum)";
                    //SQuery = "select  a.type1 as fstr,A.NAME,A.type1,B.CNT AS ITEMS from type A,(select DISTINCT stagec,count(icode) AS CNT from itwstage  GROUP BY STAGEC) B where A.id='K' AND A.TYPE1=B.STAGEC and a.type1 in("+col1 +") order by A.TYPE1";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        sg1.Rows[d].Cells[18].Text = dt.Rows[d]["cpartno"].ToString().Trim();

                        //sg1_dr["sg1_f6"] = dt.Rows[d]["cpartno"].ToString().Trim();
                        sg1.Rows[d].Cells[17].Text = dt.Rows[d]["vchnum"].ToString().Trim();
                        sg1.Rows[d].Cells[16].Text = dt.Rows[d]["vchdate"].ToString().Trim();

                        ((TextBox)sg1.Rows[d].FindControl("sg1_t3")).Text = dt.Rows[d]["iname"].ToString().Trim();
                        sg1.Rows[d].Cells[18].Text = dt.Rows[d]["Cpartno"].ToString().Trim();
                        sg1.Rows[d].Cells[22].Width = 70;
                        // sg1_dr["sg1_t2"] = "";
                        //sg1_dr["sg1_t3"] = "";
                        ((TextBox)sg1.Rows[d].FindControl("sg1_t6")).Text = dt.Rows[d]["qty"].ToString().Trim();
                        ((TextBox)sg1.Rows[d].FindControl("sg1_t21")).Text = dt.Rows[d]["iCode"].ToString().Trim();
                        ((TextBox)sg1.Rows[d].FindControl("sg1_t4")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select rate from type where id='K' and type1='" + stage + "'", "rate");
                        ((TextBox)sg1.Rows[d].FindControl("sg1_t5")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select excrate from type where id='K' and type1='" + stage + "'", "excrate");


                        string stagename = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STAGEC FROM ITWSTAGE WHERE SRNO>(SELECT SRNO FROM ITWSTAGE WHERE ICODE='90020488' AND STAGEC='" + stage + "' AND ROWNUM<=1)AND ROWNUM<=1 ORDER BY SRNO", "stagec");

                        ((TextBox)sg1.Rows[d].FindControl("sg1_t36")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='K' and type1='" + stagename + " '", "name");
                        //sg1_dr["sg1_t4"] = dt.Rows[d]["Type1"].ToString().Trim();
                        //sg1_dr["sg1_t5"] = "";
                        //sg1_dr["sg1_t7"] = "";
                        // ((TextBox)sg1.Rows[d].FindControl("sg1_t1")).Text = dt.Rows[d]["qty"].ToString().Trim();
                        //sg1_dr["sg1_t8"] = "";
                        //sg1_dr["sg1_t9"] = "";
                        //sg1_dr["sg1_t10"] = "";
                        //  sg1_dr["sg1_t11"] = dt.Rows[d]["icode"].ToString().Trim();
                        //sg1_dr["sg1_t12"] = "";
                        //sg1_dr["sg1_t13"] = "";
                        //sg1_dr["sg1_t14"] = "";
                        //sg1_dr["sg1_t15"] = "";
                        //sg1_dr["sg1_t16"] = "";

                        // sg1_dt.Rows.Add(sg1_dr);
                    }

                    //sg1_add_blankrows();

                    //ViewState["sg1"] = sg1_dt;
                    //sg1.DataSource = sg1_dt;
                    //sg1.DataBind();
                    //dt.Dispose(); sg1_dt.Dispose();
                    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();

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
                            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();
                            sg3_dt.Rows.Add(sg3_dr);
                        }

                        dt = new DataTable();
                        if (col1.Length > 8) SQuery = "select * from item where trim(icode) in (" + col1 + ")";
                        else SQuery = "select * from item where trim(icode)='" + col1 + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;

                            sg3_dr["sg3_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg3_dr["sg3_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg3_dr["sg3_t1"] = "";
                            sg3_dr["sg3_t2"] = "";
                            sg3_dr["sg3_t3"] = "";
                            sg3_dr["sg3_t4"] = "";
                            sg3_dt.Rows.Add(sg3_dr);
                        }
                    }
                    sg3_add_blankrows();

                    ViewState["sg3"] = sg3_dt;
                    sg3.DataSource = sg3_dt;
                    sg3.DataBind();
                    dt.Dispose(); sg3_dt.Dispose();
                    ((TextBox)sg3.Rows[z].FindControl("sg3_t1")).Focus();
                    #endregion
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

                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();


                            sg2_dt.Rows.Add(sg2_dr);
                        }

                        sg2_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg2_add_blankrows();

                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
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
                        for (i = 0; i < sg3.Rows.Count - 1; i++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = (i + 1);
                            sg3_dr["sg3_f1"] = sg3.Rows[i].Cells[3].Text.Trim();
                            sg3_dr["sg3_f2"] = sg3.Rows[i].Cells[4].Text.Trim();

                            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();

                            sg3_dt.Rows.Add(sg3_dr);
                        }

                        sg3_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg3_add_blankrows();

                        ViewState["sg3"] = sg3_dt;
                        sg3.DataSource = sg3_dt;
                        sg3.DataBind();
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
                            sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.Trim();
                            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.Trim();
                            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.Trim();
                            sg1_dr["sg1_srno"] = sg1.Rows[i].Cells[13].Text.Trim();
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[14].Text.Trim();
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[17].Text.Trim();
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                            sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                            sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
                            sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();
                            sg1_dr["sg1_t19"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                            sg1_dr["sg1_t20"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim();
                            sg1_dr["sg1_t21"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();
                            sg1_dr["sg1_t22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim();
                            sg1_dr["sg1_t23"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim();
                            sg1_dr["sg1_t24"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text.Trim();
                            sg1_dr["sg1_t25"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text.Trim();
                            sg1_dr["sg1_t26"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text.Trim();
                            sg1_dr["sg1_t27"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t27")).Text.Trim();
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
                        setColHeadings();
                        fetch_col_earn();
                    }
                    #endregion
                    break;

                case "ProdRep":
                    hffield.Value = "ProdRep";
                    Prodrep.Value = col1;
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = "60";
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            //SQuery = "select a.Vchnum as Templ_no,to_char(a.vchdate,'dd/mm/yyyy') as Templ_Dt,c.Aname as Supplier,b.Iname,b.Cpartno,a.Col1 as Parameter,a.col2 as Standard,a.col3 as Lower_lmt,a.col4 as Upper_limit,a.acode,a.icode,a.Ent_by,a.ent_Dt ,to_Char(a.vchdate,'yyyymmdd') as vdd,a.srno from " + frm_tabname + " a,item b,famst c where trim(A.acode)=trim(c.acode) and trim(A.icode)=trim(b.icode) and  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + " order by vdd ,a.vchnum ,a.srno";
            SQuery = "SELECT a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode as cust_code,c.aname as cust_name,a.icode as item_code,b.iname as item_name,a.enqno as jobno,to_char(A.enqdt,'dd/mm/yyyy') as job_dt from costestimate a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + " order by entry_no";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "ProdRep")
        {
            DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "Select  to_char(a.vchdate,'DD-MON') as Dated,a.Vchnum as Prod_No,b.Iname as Item,b.tarrifno,a.icode as Code,b.Cpartno as Part_No,a.iqtyin as Prodn,a.desc_ as Specs,a.invno,a.invdate,a.Ent_by,a.ent_dt,a.edt_by as QCBY,a.edt_Dt as QCDT,a.store from ivoucher a, item b where a.branchcd='" + frm_mbr + "' and substr(A.type,1,1) like '1%' and a.vchdate  " + DateRange + "   and a.icode=b.icode and a.type='" + Prodrep.Value + "' order by a.vchdate,a.type,a.vchnum,a.srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Production Record (DTD) For the Period " + fromdt + " To " + todt, frm_qstr);
        }
        else if (hffield.Value == "SummReport")
        {
            DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
            part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
            hffield.Value = "SOrd";
            make_qry_4_popup();
            fgen.Fn_open_sseek("-", frm_qstr);
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------

            string last_entdt;
            //checks
            if (edmode.Value == "Y")
            {
            }
            else
            {
                last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  ", "ldt");
                if (last_entdt == "0")
                { }
                else
                {
                    if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                    }
                }
            }

            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            }
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
                if (Checked_ok == "Y")
                {
                    try
                    {
                        oDS = new DataSet();
                        oporow = null;
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2 = new DataSet();
                        oporow2 = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);


                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();
                        save_fun2();


                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname1);

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

                                //if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
                                //{
                                save_it = "Y";
                                // }
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
                        updatetype30();
                        save_fun2();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "update " + frm_tabname1 + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + 16 + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, frm_tabname1);


                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            cmd_query = "delete from " + frm_tabname1 + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + 16 + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + frm_vnum + " Saved Successfully ");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);

                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
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
        sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t10", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t11", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t13", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t14", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t16", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t20", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t21", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t22", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t23", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t24", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t25", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t26", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t27", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t28", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t29", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t30", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t31", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t32", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t33", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t34", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t35", typeof(string)));
        //sg1_dt.Columns.Add(new DataColumn("sg1_t36", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        // Hidden Field
        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
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
        sg3_dt.Columns.Add(new DataColumn("sg3_SrNo", typeof(Int32)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t3", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t4", typeof(string)));
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
        sg1_dr["sg1_t1"] = "-";
        sg1_dr["sg1_t2"] = "-";
        sg1_dr["sg1_t3"] = "-";
        sg1_dr["sg1_t4"] = "-";
        sg1_dr["sg1_t5"] = "-";
        sg1_dr["sg1_t6"] = "-";
        sg1_dr["sg1_t7"] = "-";
        sg1_dr["sg1_t8"] = "-";
        sg1_dr["sg1_t9"] = "-";
        sg1_dr["sg1_t10"] = "-";
        sg1_dr["sg1_t11"] = "-";
        sg1_dr["sg1_t12"] = "-";
        sg1_dr["sg1_t13"] = "-";
        sg1_dr["sg1_t14"] = "-";
        sg1_dr["sg1_t15"] = "-";
        sg1_dr["sg1_t16"] = "-";
        sg1_dr["sg1_t17"] = "-";
        sg1_dr["sg1_t18"] = "-";
        sg1_dr["sg1_t19"] = "-";
        sg1_dr["sg1_t20"] = "-";
        sg1_dr["sg1_t21"] = "-";
        sg1_dr["sg1_t22"] = "-";
        sg1_dr["sg1_t23"] = "-";
        sg1_dr["sg1_t24"] = "-";
        sg1_dr["sg1_t25"] = "-";
        sg1_dr["sg1_t26"] = "-";
        sg1_dr["sg1_t27"] = "-";
        //sg1_dr["sg1_t28"] = "-";
        //sg1_dr["sg1_t29"] = "-";
        //sg1_dr["sg1_t30"] = "-";
        //sg1_dr["sg1_t31"] = "-";
        //sg1_dr["sg1_t32"] = "-";
        //sg1_dr["sg1_t33"] = "-";
        //sg1_dr["sg1_t34"] = "-";
        //sg1_dr["sg1_t35"] = "-";
        //sg1_dr["sg1_t36"] = "-";
        sg1_dt.Rows.Add(sg1_dr);
    }
    //------------------------------------------------------------------------------------
    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();
        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
        sg2_dr["sg2_t1"] = "-";
        sg2_dr["sg2_t2"] = "-";
        sg2_dt.Rows.Add(sg2_dr);
    }
    //------------------------------------------------------------------------------------
    public void sg3_add_blankrows()
    {
        sg3_dr = sg3_dt.NewRow();
        sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
        sg3_dr["sg3_f1"] = "-";
        sg3_dr["sg3_f2"] = "-";
        sg3_dr["sg3_t1"] = "-";
        sg3_dr["sg3_t2"] = "-";
        sg3_dr["sg3_t3"] = "-";
        sg3_dr["sg3_t4"] = "-";

        sg3_dt.Rows.Add(sg3_dr);
    }
    //------------------------------------------------------------------------------------
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg1r = 0; sg1r < sg1.Rows.Count; sg1r++)
            {
                ((TextBox)(sg1.Rows[sg1r].FindControl("sg1_t5"))).Attributes.Add("readonly", "readonly");
                //sg1.HeaderRow.Cells[23].Width = 100;
                //sg1.HeaderRow.Cells[24].Width = 100;
                //sg1.HeaderRow.Cells[25].Width = 100;
                //sg1.HeaderRow.Cells[26].Width = 100;
                //sg1.HeaderRow.Cells[27].Width = 100;
                //sg1.HeaderRow.Cells[28].Width = 100;
                //sg1.HeaderRow.Cells[29].Width = 100;
                //sg1.HeaderRow.Cells[30].Width = 100;
                //sg1.HeaderRow.Cells[31].Width = 100;
                //sg1.HeaderRow.Cells[32].Width = 100;
                //sg1.HeaderRow.Cells[33].Width = 100;
                //sg1.HeaderRow.Cells[34].Width = 100;
                //sg1.HeaderRow.Cells[35].Width = 100;
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
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                    //fgen.Fn_open_mseek("Select Item", frm_qstr);
                }
                break;

            case "SG1_ROW_ADD1":
                if (sg1.Rows[Convert.ToInt32(index)].Cells[13].Text.Trim().Length > 1)
                {
                    hf1.Value = index.ToString();
                    hffield.Value = "SG1_ROW_ADD1";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select item", frm_qstr);
                }
                else
                {
                    fgen.msg("-", "AMSG", "Please Select Stage First!!");
                    return;
                }
                break;
        }
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
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "SG2_ROW_ADD":
                dt = new DataTable();
                sg2_dt = new DataTable();
                dt = (DataTable)ViewState["sg2"];
                z = dt.Rows.Count - 1;
                sg2_dt = dt.Clone();
                sg2_dr = null;
                i = 0;
                for (i = 0; i < sg2.Rows.Count; i++)
                {
                    sg2_dr = sg2_dt.NewRow();
                    sg2_dr["sg2_srno"] = (i + 1);
                    sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                    sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                    sg2_dt.Rows.Add(sg2_dr);
                }
                sg2_add_blankrows();
                ViewState["sg2"] = sg2_dt;
                sg2.DataSource = sg2_dt;
                sg2.DataBind();
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
                if (index < sg3.Rows.Count - 1)
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
                    fgen.Fn_open_mseek("Select Item", frm_qstr);
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SHFTCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Result", frm_qstr);
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
        hffield.Value = "DEPTTCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum.Trim().ToUpper();
            oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow["SRNO"] = i + 1;
            //save data into the costestimate table
            oporow["icode"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();

            // oporow["COMMENTS"] = sg1.Rows[i].Cells[15].Text.Trim();
            oporow["COMMENTS"] = PackingSizeCal(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim(), ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim());
            oporow["COL1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
            oporow["COL2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper();
            oporow["COL3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper();
            oporow["COL4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper();
            oporow["COL5"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper());
            oporow["convdate"] = "SNP";
            oporow["dropdate"] = txtlbl7.Text.Trim().ToUpper();
            oporow["acode"] = fgen.seek_iname(frm_qstr, frm_cocd, "select * from (select acode,max(irate) as irt from somas where branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and icat<>'Y' and trim(icode)='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper() + "' group by acode) order by irt desc", "acode");
            oporow["qty"] = 0;
            oporow["col6"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper());
            oporow["col7"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper());
            oporow["col8"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().ToUpper());
            oporow["col9"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim().ToUpper());
            oporow["col10"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim().ToUpper());
            oporow["col11"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim().ToUpper());
            oporow["col12"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim().ToUpper());
            oporow["enqno"] = sg1.Rows[i].Cells[16].Text.Trim().ToUpper();
            oporow["enqdt"] = sg1.Rows[i].Cells[17].Text.Trim().ToUpper();
            oporow["ATTACH3"] = txtlbl4.Text.Trim().ToUpper();
            oporow["col13"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim().ToUpper());
            oporow["col14"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().ToUpper());
            oporow["itate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper());
            oporow["col15"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim().ToUpper());
            oporow["col16"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim().ToUpper());
            oporow["col17"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim().ToUpper());
            oporow["col18"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim().ToUpper());
            oporow["col19"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim().ToUpper());
            oporow["col20"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim().ToUpper());
            oporow["col21"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim().ToUpper());
            oporow["col22"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text.Trim().ToUpper());
            oporow["col23"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text.Trim().ToUpper());
            oporow["col24"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text.Trim().ToUpper());
            oporow["col25"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t27")).Text.Trim().ToUpper());
            oporow["jstatus"] = "N";
            oporow["PRINTYN"] = "Y";
            oporow["STARTDT"] = "0";
            oporow["remarks"] = txtrmk.Text.Trim().ToUpper();

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
                oporow["APP_DT"] = vardate;
                oporow["AZ_dt"] = vardate;
                oporow["edt_by"] = "-";
                oporow["edt_dt"] = vardate;
            }

            oDS.Tables[0].Rows.Add(oporow);
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5"); ;
        // frm_vty = "16";
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            oporow2 = oDS2.Tables[0].NewRow();
            oporow2["BRANCHCD"] = frm_mbr;
            oporow2["TYPE"] = "16";
            oporow2["vchnum"] = frm_vnum.Trim().ToUpper();
            oporow2["vchdate"] = txtvchdate.Text.Trim().ToUpper();
            oporow2["SRNO"] = i + 1;
            oporow2["morder"] = i + 1;
            oporow2["rgpnum"] = "000000";
            oporow2["iopr"] = "16";
            oporow2["acode"] = txtlbl4.Text.Trim().ToUpper();
            oporow2["icode"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
            oporow2["rec_iss"] = "D";
            oporow2["iqtyin"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper());
            oporow2["iqtyout"] = 0;
            oporow2["iqty_chl"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper());
            oporow2["rej_rw"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper());
            //oporow2["desc_"] = sg1.Rows[i].Cells[15].Text.Trim().ToUpper();
            oporow2["desc_"] = PackingSizeCal(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper(), ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper());
            oporow2["rcode"] = "-";
            oporow2["prnum"] = "SNP";
            oporow2["naration"] = frm_uname + " " + vardate;
            oporow2["irate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper());
            oporow2["approxval"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper());
            oporow2["thru"] = "-";
            oporow2["pname"] = "-";
            oporow2["no_cases"] = frm_uname;
            oporow2["mtime"] = txtSort.Text.Trim().ToUpper();
            oporow2["binno"] = txtPack.Text.Trim().ToUpper();
            oporow2["o_deptt"] = txtlbl4a.Text.Trim().ToUpper();
            oporow2["iqty_wt"] = 0;
            oporow2["iweight"] = 0;
            oporow2["isize"] = "-";
            oporow2["tc_no"] = "-";
            oporow2["styleno"] = "-";
            oporow2["btchno"] = "-";
            oporow2["vcode"] = txtlbl7.Text.Trim().ToUpper();
            oporow2["invno"] = sg1.Rows[i].Cells[16].Text.Trim().ToUpper();
            oporow2["invdate"] = sg1.Rows[i].Cells[17].Text.Trim().ToUpper();
            oporow2["inspected"] = "Q";
            oporow2["store"] = "N";
            oporow2["unit"] = "-";
            oporow2["exp_punit"] = 0;
            oporow2["txb_punit"] = 0;
            oporow2["gst_pos"] = "-";
            oporow2["doc_tot"] = 0;
            oporow2["tpt_names"] = "-";
            oporow2["col1"] = "-";

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
                oporow2["edt_by"] = "-";
                oporow2["eDt_dt"] = vardate;
            }
            oDS2.Tables[0].Rows.Add(oporow2);
        }

    }
    //------------------------------------------------------------------------------------
    void save_fun3()
    {

    }
    //------------------------------------------------------------------------------------
    void save_fun4()
    {

    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "60");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------
    protected void txt_TextChanged(object sender, EventArgs e)
    {
        //fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
        // made logic to get working hours and working minutes
        string dttoh = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text;
        string dttom = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text;
        string dtfromh = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text;
        string dtfromm = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text;

        DateTime dtFrom = DateTime.Parse(dtfromh + ":" + dtfromm);
        DateTime dtTo = DateTime.Parse(dttoh + ":" + dttom);

        int timeDiff = dtFrom.Subtract(dtTo).Hours;
        int timediff2 = dtFrom.Subtract(dtTo).Minutes;

        TextBox txtName = ((TextBox)sg1.Rows[i].FindControl("sg1_t5"));
        txtName.Text = timeDiff.ToString();

        TextBox txtName1 = ((TextBox)sg1.Rows[i].FindControl("sg1_t6"));
        txtName1.Text = timediff2.ToString();
    }
    //------------------------------------------------------------------------------------
    public void fetch_col_earn()
    {
        dt2 = new DataTable();
        SQuery = "select INITCAP(substr(Name,1,10)) as Name from(Select  Name,type1,branchcd from typewip where branchcd!='DD' and id='RJC61'  order by type1) where rownum<=20";
        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg1_dr = sg1_dt.NewRow();

        i = 0;
        int d = 25;
        if (dt2.Rows.Count > 0)
        {
            do
            {
                sg1.HeaderRow.Cells[d].Text = dt2.Rows[i]["Name"].ToString().Trim();
                d = d + 1;
                i = i + 1;

            } while (i < dt2.Rows.Count);

        }
    }
    //------------------------------------------------------------------------------------
    public void fetch_col_downtime()
    {
        dt2 = new DataTable();
        SQuery = "select INITCAP(substr(Name,1,10)) as Name from(Select  Name,type1,branchcd from typewip where branchcd!='DD' and id='DTC61' order by type1) where rownum<=10";
        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        sg1_dr = sg1_dt.NewRow();
        i = 0;
        int d = 35;
        if (dt2.Rows.Count > 0)
        {
            if (sg1.Rows.Count > 0)
            {
                do
                {
                    sg1.HeaderRow.Cells[d].Text = dt2.Rows[i]["Name"].ToString().Trim();
                    d = d + 1;
                    i = i + 1;

                } while (i < dt2.Rows.Count);
            }
        }
    }
    //------------------------------------------------------------------------------------
    public void updatetype30()
    {
        for (i = 0; i < sg1.Rows.Count; i++)
        {
            CheckBox chk = (CheckBox)sg1.Rows[i].FindControl("sg1_chk");
            if (chk != null & chk.Checked)
            {

                cmd_query = "update " + frm_tabname + " set jstatus='Y',supcl_by='" + frm_uname + vardate + "' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + 30 + sg1.Rows[i].Cells[16].Text.Trim() + Convert.ToDateTime(sg1.Rows[i].Cells[17].Text.Trim()).ToString("dd/MM/yyyy") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
            }

        }
    }
    //------------------------------------------------------------------------------------
    public string PackingSizeCal(string Qty, string Icode)
    {
        string FinalPacking = "";
        double OkQty = fgen.make_double(Qty);
        double StdPackSize = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select (Case when nvl(packsize,0)=0 then 1 else nvl(packsize,0) end) as packsize from item where icode='" + Icode + "'", "packsize"));
        double PackingSize = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select round(" + Qty + "/" + StdPackSize.ToString() + ",2) as packsize from dual", "packsize"));
        //OkQty / StdPackSize;

        string[] Packing = PackingSize.ToString().Split('.');
        //if (Packing.Length > 1)
        if (PackingSize > 0)
        {
            double RoundOff = Math.Round(fgen.make_double(Packing[0].ToString()), 1);
            //   int a = fgen.make_int(RoundOff.ToString());

            //FinalPacking = StdPackSize + "X" + Packing[0].ToString() + " , " + (RoundOff.ToString()).ToString() + "X1";
            FinalPacking = StdPackSize + "X" + Packing[0].ToString() + " , " + (OkQty - (StdPackSize * RoundOff)) + " X 1";
            //(RoundOff.ToString()).ToString() + "X1";
        }
        else
        {
            FinalPacking = StdPackSize + "X" + Packing[0].ToString();
        }
        return FinalPacking;
    }
    //------------------------------------------------------------------------------------
    protected void btnProdReport_Click(object sender, EventArgs e)
    {
        hffield.Value = "ProdRep";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Series", frm_qstr);
    }
    //------------------------------------------------------------------------------------      
    protected void btnidtag_Click(object sender, EventArgs e)
    {
        hffield.Value = "IDTags";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Series", frm_qstr);
    }
    protected void btnsummrep_Click(object sender, EventArgs e)
    {
        hffield.Value = "SummReport";
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);        
    }
}