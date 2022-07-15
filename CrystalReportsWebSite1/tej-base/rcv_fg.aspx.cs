using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class rcv_fg : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string html_body = "";
    string Prg_Id, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
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

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            frm_vty = "15";

            if (!Page.IsPostBack)
            {
                doc_addl.Value = "1";

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();

            DateRange = " between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";

            //if (frm_ulvl != "0") btndel.Visible = false;

            btnprint.Visible = false;
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

            for (int K = 0; K < sg1.Rows.Count; K++)
            {
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));
                ((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("readonly", "readonly");
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

        //txtlbl2.Attributes.Add("readonly", "readonly");
        //txtlbl3.Attributes.Add("readonly", "readonly");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");


        //txtlbl5.Attributes.Add("readonly", "readonly");
        //txtlbl6.Attributes.Add("readonly", "readonly");

        //my_Tabs
        //txtlbl2.Attributes["required"] = "true";
        //txtlbl2.BackColor = System.Drawing.ColorTranslator.FromHtml("#E0FF00");
        // to hide and show to tab panel

        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {

            case "M12008":
                tab3.Visible = false;
                tab4.Visible = false;
                break;
            case "F60161":
                //AllTabs.Visible = false;
                break;
        }
        tab1.Visible = true;
        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;

        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false;
        btnprint.Disabled = false;

        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();

        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();
        sg4_add_blankrows();



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
        btnnew.Disabled = true;
        btnedit.Disabled = true;
        btnsave.Disabled = false;
        btnlist.Disabled = true;
        btnprint.Disabled = true;
        btndel.Disabled = true;
        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;
        btnexit.Visible = false;
        btncancel.Visible = true;
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
        string tbl_id;
        tbl_id = "";
        typePopup = "N";

        doc_nf.Value = "VCHNUM";
        doc_df.Value = "VCHDATE";
        frm_tabname = "IVOUCHER";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        tbl_id = "15";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F10101":
                tbl_id = "Y";
                //item group
                break;
            case "F10121":
                tbl_id = "U";
                //uom
                break;
            case "F10126":
                tbl_id = "K";
                //process
                break;
            case "F70173":
                tbl_id = "Z";
                //ac grp
                break;
            case "F70176":
                tbl_id = "V";
                //vch type
                break;
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", tbl_id);

        dt = new DataTable();
        //dt = fgen.getdata(frm_qstr, frm_cocd, "Select ID,Type1,Name,ment_by,to_char(ment_dt,'dd/mm/yyyy') as ent_Dt,mEDT_BY,mEDT_DT from " + frm_tabname + " where TBRANCHCD='00' AND id='"+ tbl_id +"' order by type1 ");

        sg5.DataSource = null;
        sg5.DataBind();

        sg4.DataSource = null;
        sg4.DataBind();

        lblheader.Text = "Assembly Entry";
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {

        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        if (CSR.Length > 1) cond = " and trim(a.ccode)='" + cond + "'";
        switch (btnval)
        {
            case "BTN_23":
                break;
            case "TACODE":
                //pop1
                Acode_Sel_query();
                break;
            case "TICODE":
                //pop1
                Icode_Sel_query();
                break;
            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col2 = "SELECT ICODE,SUM(BAL) AS qty FROM (SELECT TRIM(ICODE) AS ICODE,TRIM(PURPOSE) AS PURPOSE,-1*IQTYOUT AS BAL FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='39' AND VCHDATE>=TO_dATE('01/04/2019','DD/MM/YYYY') UNION ALL SELECT TRIM(ICODE) AS ICODE,TRIM(PURPOSE) AS PURPOSE,IQTYOUT AS BAL FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '3%' AND TYPE!='39' AND STORE='Y' AND VCHDATE>=TO_dATE('01/04/2019','DD/MM/YYYY') ) GROUP BY ICODE HAVING SUM(BAL)>0";

                col1 = "";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT TRIM(IBCODE) AS ICODE FROM ITEMOSP WHERE TRIM(ICODE)='" + txtIcode.Value.Trim() + "'");
                foreach (DataRow dr in dt.Rows)
                {
                    if (col1.Length > 1) col1 += "," + "'" + dr["icode"].ToString().Trim() + "'";
                    else col1 = "'" + dr["icode"].ToString().Trim() + "'";
                }

                col2 = "SELECT ICODE,TRIM(PURPOSE) AS PURPOSE,TRIM(BTCHNO) AS BTCHNO,TRIM(TC_NO) AS TC_NO,SUM(BAL) AS qty FROM (SELECT TRIM(ICODE) AS ICODE,TRIM(PURPOSE) AS PURPOSE,TRIM(BTCHNO) AS BTCHNO,TRIM(TC_NO) AS TC_NO,-1*IQTYOUT AS BAL FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='39' AND VCHDATE>=TO_dATE('01/04/2019','DD/MM/YYYY') UNION ALL SELECT TRIM(ICODE) AS ICODE,TRIM(PURPOSE) AS PURPOSE,TRIM(BTCHNO) AS BTCHNO,TRIM(TC_NO) AS TC_NO,IQTYOUT AS BAL FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '3%' AND TYPE!='39' AND STORE='Y' AND VCHDATE>=TO_dATE('01/04/2019','DD/MM/YYYY')) WHERE ICODE IN (" + col1 + ") GROUP BY ICODE,TRIM(BTCHNO),TRIM(Tc_NO),TRIM(PURPOSE) HAVING SUM(BAL)>0";
                //SQuery = "SELECT distinct trim(a.Icode)||'~'||trim(b.purpose)||'~'||trim(b.btchno)||'~'||Trim(b.tc_no) as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit,b.purpose AS CTRLNO,b.btchno,b.tc_no AS IDNO,b.ponum as pono,b.finvno,b.invno from item a,ivoucher b,itemosp c,(" + col2 + ") d where trim(a.icode)=trim(b.icode) and trim(a.icode)=trim(c.ibcode) and trim(b.icode)=trim(d.icode) AND B.BRANCHCD='" + frm_mbr + "' and b.type like '3%' and b.type!='39' and length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8  and trim(c.icode)='" + txtIcode.Value.Trim() + "' order by a.Iname ";
                //SQuery = "SELECT distinct trim(a.Icode)||'~'||trim(D.purpose)||'~'||trim(D.btchno)||'~'||Trim(D.tc_no) as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit,D.purpose AS CTRLNO,D.btchno,D.tc_no AS IDNO,D.QTY AS BAL from item a,itemosp c,(" + col2 + ") d where trim(a.icode)=trim(c.ibcode) and length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8  and trim(c.icode)='" + txtIcode.Value.Trim() + "' order by a.Iname ";
                SQuery = "SELECT distinct trim(a.Icode)||'~'||trim(D.purpose)||'~'||trim(D.btchno)||'~'||Trim(D.tc_no) as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit,D.purpose AS CTRLNO,D.btchno,D.tc_no AS IDNO,D.QTY AS BAL from item a,(" + col2 + ") d where length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8  AND TRIM(A.ICODE)=TRIM(D.ICODE) order by a.Iname ";
                break;
            case "SG1_ROW_TAX":
                break;
            case "New":
                Type_Sel_query();
                break;
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            case "JOBNO":
                SQuery = "SELECT A.FSTR,A.VCHNUM AS JOBNO,A.VCHDATE AS JOBDT,B.INAME AS PRODUCT,A.ICODE AS ITEM,SUM(A.qTY) AS QTY, TO_CHAR(TO_DATE(A.VCHDATE,'DD/MM/YYYY'),'YYYYMMDD') AS VDD FROM (SELECT BRANCHCD||TYPE||TRIM(vCHNUM)||TO_cHAR(vCHDATE,'DD/MM/YYYY') AS FSTR,TRIM(VCHNUM) AS VCHNUM,TO_CHAr(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,QTY FROM COSTESTIMATE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='30' AND VCHDATE " + DateRange + " UNION ALL SELECT BRANCHCD||'30'||TRIM(PLANNO)||TRIM(PLANDT) AS FSTR,TRIM(PLANNO) AS VCHNUM,TRIM(PLANDT) AS VCHDATE,TRIM(ICODE) AS ICODE,-1*QTY AS QTY FROM EXTRUSION WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='TB' AND VCHDATE " + DateRange + " ) A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) GROUP BY A.FSTR,A.VCHNUM,A.VCHDATE,A.ICODE,B.INAME HAVING SUM(A.qTY)>0 ORDER BY VDD DESC,A.VCHNUM DESC";
                break;
            case "STAGE":
                SQuery = "SELECT TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='1' and type1 LIKE '6G%' ORDER BY TYPE1";
                break;
            case "MACHINE":
                SQuery = "SELECT TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM typegrp WHERE branchcd='" + frm_mbr + "' and type='MN' ORDER BY TYPE1";
                break;
            case "ERPCODE":
                SQuery = "SELECT ICODE AS FSTR,INAME AS PRODUCT,CPARTNO,ICODE AS ERPCODE FROM ITEM WHERE length(trim(icode))>6 and ICODE LIKE '9%' ORDER BY ICODE";
                SQuery = "SELECT A.VCHNUM||A.VCHDATE||A.ICODE AS FSTR,A.REVIS_NO, A.VCHNUM AS JONO,A.VCHDATE AS JODT,B.INAME AS PRODUCT,A.ICODE ERPCODE,SUM(A.JCQTY) AS JC_QTY,SUM(A.PRODQTY) AS " +
                    "PROD_QTY,SUM(A.JCQTY)-SUM(A.PRODQTY) AS BAL FROM (SELECT TRIM(ORDNO) AS VCHNUM,TO_cHAR(ORDDT,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,QTYORD AS JCQTY,0 AS " +
                    "PRODQTY,ORDNO||TO_CHAR(ORDDT,'DD/MM/YYYY')||ICODE AS REVIS_NO FROM SOMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '4%' AND ORDDT " + DateRange + " UNION ALL SELECT TRIM(INVNO),TO_cHAR(INVDATE,'DD/MM/YYYY')" +
                    ",TRIM(ICODE),0 AS QTY,IQTYIN,REVIS_NO FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " AND " +
                    "TRIM(STAGE)='" + txtStage.Value.Trim().Split('~')[0] + "') A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) GROUP BY A.VCHNUM||A.VCHDATE||A.ICODE,A.VCHNUM,A.VCHDATE,B.INAME" +
                    ",A.ICODE,A.REVIS_NO HAVING SUM(A.JCQTY)-SUM(A.PRODQTY)>0 ORDER BY A.VCHNUM,A.VCHDATE";
                if (txtStage.Value.Length > 2)
                {
                    string stage = txtStage.Value.Substring(0, 2);
                    if (stage == "61" || stage == "6G") { }
                    else
                    {
                        SQuery = "SELECT A.VCHNUM||A.VCHDATE||A.ICODE AS FSTR, A.VCHNUM AS JONO,A.VCHDATE AS JODT,B.INAME AS PRODUCT,A.ICODE ERPCODE,SUM(A.JCQTY) AS JC_QTY,SUM(A.PRODQTY) AS PROD_QTY,SUM(A.JCQTY)-SUM(A.PRODQTY) AS BAL FROM (SELECT TRIM(VCHNUM) AS VCHNUM,TO_cHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,balqty AS JCQTY,0 AS PRODQTY FROM TBJC WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='JC' AND VCHDATE " + DateRange + " AND SUBSTR(ICODE,1,1)='9' UNION ALL SELECT TRIM(INVNO),TO_cHAR(INVDATE,'DD/MM/YYYY'),TRIM(ICODE),0 AS QTY,IQTYIN FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + ") A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) GROUP BY A.VCHNUM||A.VCHDATE||A.ICODE,A.VCHNUM,A.VCHDATE,B.INAME,A.ICODE HAVING SUM(A.JCQTY)-SUM(A.PRODQTY)>0 ORDER BY A.VCHNUM,A.VCHDATE";
                        //SQuery = "SELECT A.VCHNUM||A.VCHDATE||A.ICODE AS FSTR, A.VCHNUM AS JONO,A.VCHDATE AS JODT,B.INAME AS PRODUCT,A.ICODE ERPCODE,SUM(A.JCQTY) AS TFR_QTY,SUM(A.PRODQTY) AS PROD_QTY,SUM(A.JCQTY)-SUM(A.PRODQTY) AS BAL,A.revis_no as Tracking_No FROM (SELECT TRIM(invno) AS VCHNUM,TO_cHAR(invdate,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,IQTYIN AS JCQTY,0 AS PRODQTY,revis_no FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='3A' AND VCHDATE " + DateRange + " AND STAGE='" + stage + "' UNION ALL SELECT TRIM(INVNO),TO_cHAR(INVDATE,'DD/MM/YYYY'),TRIM(ICODE),0 AS QTY,IQTYIN,revis_no FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='15' AND VCHDATE " + DateRange + " AND STAGE='" + stage + "') A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) GROUP BY A.VCHNUM||A.VCHDATE||A.ICODE,A.VCHNUM,A.VCHDATE,B.INAME,A.revis_no,A.ICODE HAVING SUM(A.JCQTY)-SUM(A.PRODQTY)>0 ORDER BY A.VCHNUM,A.VCHDATE";
                        SQuery = "SELECT A.VCHNUM||A.VCHDATE||A.ICODE AS FSTR, A.VCHNUM AS JONO,A.VCHDATE AS JODT,B.INAME AS PRODUCT,A.ICODE ERPCODE,SUM(A.JCQTY) AS TFR_QTY,SUM(A.PRODQTY) AS PROD_QTY,SUM(A.JCQTY)-SUM(A.PRODQTY) AS BAL,A.revis_no as Tracking_No FROM (SELECT TRIM(invno) AS VCHNUM,TO_cHAR(invdate,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,IQTYOUT AS JCQTY,0 AS PRODQTY,revis_no FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='3A' AND VCHDATE " + DateRange + " AND IOPR='" + stage + "' UNION ALL SELECT TRIM(INVNO),TO_cHAR(INVDATE,'DD/MM/YYYY'),TRIM(ICODE),0 AS QTY,IQTYIN,revis_no FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='15' AND VCHDATE " + DateRange + " AND STAGE='" + stage + "') A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) GROUP BY A.VCHNUM||A.VCHDATE||A.ICODE,A.VCHNUM,A.VCHDATE,B.INAME,A.revis_no,A.ICODE HAVING SUM(A.JCQTY)-SUM(A.PRODQTY)>0 ORDER BY A.VCHNUM,A.VCHDATE";


                        SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit,a.hsname,b.revis_no,b.bal as Stock_Qty from item a inner join (" + fgen.WIPSTKQry(frm_cocd, frm_qstr, frm_mbr, fromdt, todt) + ") b on trim(a.icode)=trim(b.icode) and length(trim(nvl(b.revis_no,'-'))) >6 " +
                            "where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 and trim(b.stage)='" + txtStage.Value.Split('~')[0].ToString() + "' order by a.Iname ";


                    }
                    //if (stage == "69") SQuery = "SELECT A.VCHNUM||A.VCHDATE||A.ICODE AS FSTR, A.VCHNUM AS JONO,A.VCHDATE AS JODT,B.INAME AS PRODUCT,A.ICODE ERPCODE,SUM(A.JCQTY) AS JC_QTY,SUM(A.PRODQTY) AS PROD_QTY,SUM(A.JCQTY)-SUM(A.PRODQTY) AS BAL FROM (SELECT TRIM(ORDNO) AS VCHNUM,TO_cHAR(ORDDT,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,QTYORD AS JCQTY,0 AS PRODQTY FROM SOMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '4%' AND ORDDT " + DateRange + " UNION ALL SELECT TRIM(INVNO),TO_cHAR(INVDATE,'DD/MM/YYYY'),TRIM(ICODE),0 AS QTY,IQTYIN FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " AND TRIM(STAGE)='" + txtStage.Value.Trim() + "') A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) GROUP BY A.VCHNUM||A.VCHDATE||A.ICODE,A.VCHNUM,A.VCHDATE,B.INAME,A.ICODE HAVING SUM(A.JCQTY)-SUM(A.PRODQTY)>0 ORDER BY A.VCHNUM,A.VCHDATE";
                }
                break;
            case "2ND":
                SQuery = "SELECT A.ibcode AS FSTR,B.INAME,A.ibcode FROM ITEMOSP A,ITEM B WHERE TRIM(A.ibcode)=TRIM(B.icode) AND TRIM(A.ICODE)='" + hf1.Value + "' and SUBSTR(A.IBCODE,1,1) IN ('9','7') ORDER BY A.IBCODE ";
                break;
            case "VALIDATEBOM":
                SQuery = "Select TRIM(A.ICODE) as fstr,a.vchnum as bomno,to_char(A.vchdate,'dd/mm/yyyy') as bomdt,a.ibcode as erpcode,b.iname as product,a.ibqty as bomqty from itemosp a,item b where trim(a.ibcode)=trim(b.icode) and trim(a.icode)='" + txtIcode.Value.Trim() + "' order by a.ibcode";
                break;
            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    SQuery = "select distinct TRIM(a.VCHNUM)||TO_cHAR(a.VCHDATE,'DD/MM/YYYY') as fstr,a." + doc_nf.Value + " as Prod_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Prod_Dt,b.iName as product,a.icode as erpCode,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,c.name as stage,a.stage as code from " + frm_tabname + " a,item b,type c where trim(A.icode)=trim(B.icode) and trim(a.stage)=trim(c.type1) and c.id='1' and a.branchcd='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "'  and a.vchdate " + DateRange + " order by vdd desc,a." + doc_nf.Value + " desc";
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
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
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

            if (CSR.Length > 1)
            {
                //txtlbl4.Value = CSR;
                //txtlbl4.Disabled = true;
            }
            btnStage.Focus();
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }
    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");

        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND type='" + frm_vty + "'  AND VCHDATE " + DateRange + "", 6, "VCH");

        txtvchnum.Value = frm_vnum;
        txtvchdate.Value = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        //txtlbl2.Text = frm_uname;
        //txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

        disablectrl();
        fgen.EnableForm(this.Controls);


        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();


        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;

        sg2_dt = new DataTable();
        create_tab2();
        sg2_add_blankrows();
        sg2_add_blankrows();
        sg2_add_blankrows();
        sg2_add_blankrows();
        sg2_add_blankrows();
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        setColHeadings();
        ViewState["sg2"] = sg2_dt;

        sg3_dt = new DataTable();
        create_tab3();
        sg3_add_blankrows();
        sg3.DataSource = sg3_dt;
        sg3.DataBind();
        setColHeadings();
        ViewState["sg3"] = sg3_dt;

        //-------------------------------------------
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        SQuery = "Select nvl(a.obj_name,'-') as udf_name from udf_config a where trim(a.frm_name)='" + Prg_Id + "' ORDER BY a.srno";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

        create_tab4();
        sg4_dr = null;
        if (dt.Rows.Count > 0)
        {
            for (i = 0; i < dt.Rows.Count; i++)
            {
                sg4_dr = sg4_dt.NewRow();
                sg4_dr["sg4_srno"] = sg4_dt.Rows.Count + 1;

                sg4_dr["sg4_t1"] = dt.Rows[i]["udf_name"].ToString().Trim();
                sg4_dt.Rows.Add(sg4_dr);
            }
        }
        sg4_add_blankrows();
        ViewState["sg4"] = sg4_dt;
        sg4.DataSource = sg4_dt;
        sg4.DataBind();
        dt.Dispose();
        sg4_dt.Dispose();

        //--------------------------------
        ////sg4_dt = new DataTable();
        ////create_tab4();
        ////sg4_add_blankrows();
        ////sg4_add_blankrows();
        ////sg4_add_blankrows();
        ////sg4_add_blankrows();
        ////sg4_add_blankrows();
        ////sg4.DataSource = sg4_dt;
        ////sg4.DataBind();
        ////setColHeadings();
        ////ViewState["sg4"] = sg4_dt;        
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
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

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Value.ToString());
        if (dhd == 0) { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }


        //if (fgen.make_double(txtlbl51.Text) > (fgen.make_double(txtlbl40.Text) + (fgen.make_double(txtlbl40.Text) * 0.3)))
        //{
        //    fgen.msg("-", "AMSG", "Cannot Enter more then Job Qty!!");
        //    return;
        //}

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        double totqty = 0;
        foreach (GridViewRow gr1 in sg1.Rows)
        {
            totqty += fgen.make_double(((TextBox)gr1.FindControl("sg1_t1")).Text);
        }
        if (totqty <= 0)
        {
            fgen.msg("-", "AMSG", "Please Fill Production Qty!!");
            return;
        }

        if (txtStage.Value.Trim().ToString().Length > 1)
        {
            {
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[13].Text.Trim().Length > 2)
                    {
                        //vipin
                        string icode = gr.Cells[13].Text.Trim();
                        //col1 = fgen.seek_iname(frm_qstr, frm_cocd, wipquery(icode, txtlbl51v.Text), "BAL");
                        col1 = fgen.SeekWipStock(frm_cocd, frm_qstr, frm_mbr, icode, txtStage.Value.Split('~')[0], txtlbl51v.Text.Trim(), fromdt, todt, " where REVIS_NO='" + txtlbl51v.Text + "' and type || trim(vchnum) || to_Char(vchdate, 'dd/mm/yyyy') != '" + frm_vty + txtvchnum.Value.Trim() + txtvchdate.Value.Trim() + "'");
                        if (fgen.make_double(col1) < fgen.make_double(((TextBox)gr.FindControl("sg1_t1")).Text.Trim()))
                        {
                            fgen.msg("-", "AMSG", "Total Balance Material " + col1 + "'13'Consuming Material " + ((TextBox)gr.FindControl("sg1_t1")).Text.Trim() + "'13'Can Not Exceed from Stock!!");
                            return;
                        }
                    }
                }
            }
        }

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
    }
    //------------------------------------------------------------------------------------
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        chk_rights = "Y";
        if (chk_rights == "Y")
        {
            clearctrl();
            set_Val();
            hffield.Value = "Del";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr, false);

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
        sg4_dt = new DataTable();

        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();

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

        sg4_add_blankrows();
        sg4.DataSource = sg4_dt;
        sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();


        ViewState["sg1"] = null;
        ViewState["sg2"] = null;
        ViewState["sg3"] = null;
        ViewState["sg4"] = null;
        setColHeadings();

        imgItem.ImageUrl = null;
        //lblInfo.InnerText = "";
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
        //fgen.Fn_FillChart(frm_cocd, frm_qstr, "Testing Chart", "line", "Main Heading", "Sub Heading", SQuery);
        //hffield.Value = "Print";
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
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
            if (CP_BTN.Trim().Substring(0, 3) == "BTN" || CP_BTN.Trim().Substring(0, 3) == "SG1" || CP_BTN.Trim().Substring(0, 3) == "SG2" || CP_BTN.Trim().Substring(0, 3) == "SG3" || CP_BTN.Trim().Substring(0, 3) == "SG4")
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||trim(a.type)||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||trim(a.type)||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + "39" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + "Delete");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                clearctrl(); fgen.ResetForm(this.Controls);
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
                case "Del":
                    if (col1 == "") return;
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);

                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry No to Delete", frm_qstr);
                    break;
                case "Edit":
                    if (col1 == "") return;
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);

                    hffield.Value = "Edit_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry No to Edit", frm_qstr);
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
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry No to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "" || col1 == "-") return;
                    clearctrl();
                    frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");

                    string mv_col;
                    mv_col = frm_mbr + frm_vty + col1;
                    SQuery = "Select a.*,to_Char(a.ent_Dt,'dd/mm/yyyy') As ent_date,to_Char(a.INVDATE,'dd/mm/yyyy') As INVDAT,b.iname,b.cpartno from " + frm_tabname + " a,item b where trim(a.icode)=trim(B.icode) and a.branchcd||a.type||trim(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + mv_col + "' ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txtvchnum.Value = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Value = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl51v.Text = dt.Rows[0]["revis_no"].ToString();
                        txtjobno.Value = dt.Rows[0]["INVNO"].ToString() + "-" + dt.Rows[0]["INVDAT"].ToString();
                        txtOBSODmin.Text = dt.Rows[0]["BILLRATE"].ToString();
                        txtOBSODmax.Text = dt.Rows[0]["SHE_CESS"].ToString();

                        txtOBSthikmin.Text = dt.Rows[0]["BILLAMT"].ToString();
                        txtOBSthikmax.Text = dt.Rows[0]["PAPCESS"].ToString();

                        ddEdgePre.SelectedItem.Text = dt.Rows[0]["DOCSRNO"].ToString();
                        ddDimension.SelectedItem.Text = dt.Rows[0]["COL1"].ToString();

                        txtlbl51.Text = dt.Rows[0]["iqtyin"].ToString().Trim();

                        txtBtchno.Value = dt.Rows[0]["btchno"].ToString().Trim();
                        txtControl.Value = dt.Rows[0]["purpose"].ToString().Trim();
                        txtIDNo.Value = dt.Rows[0]["tc_no"].ToString().Trim();

                        txtIcode.Value = dt.Rows[0]["icode"].ToString().Trim();
                        txtiname.Value = dt.Rows[0]["iname"].ToString().Trim();

                        txtMachine.Value = dt.Rows[0]["location"].ToString().Trim();
                        txtStage.Value = dt.Rows[0]["stage"].ToString().Trim() + "~" + fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TYPE1 AS FSTR,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='1' and type1='" + dt.Rows[0]["stage"].ToString().Trim() + "'", "name");


                        dt2 = new DataTable();
                        SQuery = "Select a.*,to_Char(a.ent_Dt,'dd/mm/yyyy') As ent_date,b.iname,b.cpartno from " + frm_tabname + " a,item b where trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a.VCHNUM)||TO_CHAR(a.VCHDATE,'DD/MM/YYYY')='" + frm_mbr + "39" + col1 + "' ";
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        i = 0;
                        foreach (DataRow dr2 in dt2.Rows)
                        {
                            create_tab();
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = i + 1;
                            sg1_dr["sg1_f1"] = dr2["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dr2["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dr2["cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = txtControl.Value;

                            sg1_dr["sg1_t1"] = dr2["iqtyout"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dr2["REJ_RW"].ToString().Trim();
                            sg1_dr["sg1_t3"] = txtControl.Value;
                            sg1_dr["sg1_t4"] = txtBtchno.Value;
                            sg1_dr["sg1_t5"] = txtIDNo.Value;
                            sg1_dt.Rows.Add(sg1_dr);

                            ViewState["sg1"] = sg1_dt;
                            sg1.DataSource = sg1_dt;
                            sg1.DataBind();
                            sg1_dt.Dispose();
                        }

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                        txtvchnum.Disabled = true;
                        txtvchdate.Disabled = true;
                    }
                    #endregion
                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||trim(A.TYPE)||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    //txtlbl4.Text = col1;
                    //txtlbl4a.Text = col2;

                    //txtlbl5.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    //txtlbl6.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");

                    //btnlbl7.Focus();
                    break;
                case "TICODE":
                    if (col1.Length <= 0) return;
                    //txtlbl7.Text = col1;
                    //txtlbl7a.Text = col2;
                    //txtlbl2.Focus();
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

                            sg1_dt.Rows.Add(sg1_dr);
                        }



                        //create_tab();

                        sg1_dr = sg1_dt.NewRow();
                        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        sg1_dr["sg1_h1"] = col3;
                        sg1_dr["sg1_h2"] = col2;
                        sg1_dr["sg1_h3"] = "-";
                        sg1_dr["sg1_h4"] = "-";
                        sg1_dr["sg1_h5"] = "-";
                        sg1_dr["sg1_h6"] = "-";
                        sg1_dr["sg1_h7"] = "-";
                        sg1_dr["sg1_h8"] = "-";
                        sg1_dr["sg1_h9"] = "-";
                        sg1_dr["sg1_h10"] = "-";

                        sg1_dr["sg1_f1"] = col3;
                        sg1_dr["sg1_f2"] = col2;
                        sg1_dr["sg1_f3"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                        sg1_dr["sg1_f4"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6");
                        sg1_dr["sg1_f5"] = col1.Split('~')[1];

                        sg1_dr["sg1_t1"] = "";
                        sg1_dr["sg1_t2"] = "";
                        sg1_dr["sg1_t3"] = col1.Split('~')[1];
                        sg1_dr["sg1_t4"] = col1.Split('~')[2];
                        sg1_dr["sg1_t5"] = col1.Split('~')[3];
                        sg1_dr["sg1_t6"] = "0";
                        sg1_dr["sg1_t7"] = "0";
                        sg1_dr["sg1_t8"] = "0";
                        sg1_dr["sg1_t9"] = "0";
                        sg1_dr["sg1_t10"] = "";
                        sg1_dr["sg1_t11"] = "";
                        sg1_dr["sg1_t12"] = "";
                        sg1_dr["sg1_t13"] = "";
                        sg1_dr["sg1_t14"] = "";
                        sg1_dr["sg1_t15"] = "";
                        sg1_dr["sg1_t16"] = "";

                        sg1_dt.Rows.Add(sg1_dr);

                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    break;
                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    if (edmode.Value == "Y")
                    {
                        //return;
                    }

                    create_tab();

                    sg1_dr = sg1_dt.NewRow();
                    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                    sg1_dr["sg1_h1"] = col3;
                    sg1_dr["sg1_h2"] = col2;
                    sg1_dr["sg1_h3"] = "-";
                    sg1_dr["sg1_h4"] = "-";
                    sg1_dr["sg1_h5"] = "-";
                    sg1_dr["sg1_h6"] = "-";
                    sg1_dr["sg1_h7"] = "-";
                    sg1_dr["sg1_h8"] = "-";
                    sg1_dr["sg1_h9"] = "-";
                    sg1_dr["sg1_h10"] = "-";

                    sg1_dr["sg1_f1"] = col3;
                    sg1_dr["sg1_f2"] = col2;
                    sg1_dr["sg1_f3"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                    sg1_dr["sg1_f4"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6");
                    sg1_dr["sg1_f5"] = col1.Split('~')[1];

                    sg1_dr["sg1_t1"] = "";
                    sg1_dr["sg1_t2"] = "";
                    sg1_dr["sg1_t3"] = col1.Split('~')[1];
                    sg1_dr["sg1_t4"] = col1.Split('~')[2];
                    sg1_dr["sg1_t5"] = col1.Split('~')[3];
                    sg1_dr["sg1_t6"] = "0";
                    sg1_dr["sg1_t7"] = "0";
                    sg1_dr["sg1_t8"] = "0";
                    sg1_dr["sg1_t9"] = "0";
                    sg1_dr["sg1_t10"] = "";
                    sg1_dr["sg1_t11"] = "";
                    sg1_dr["sg1_t12"] = "";
                    sg1_dr["sg1_t13"] = "";
                    sg1_dr["sg1_t14"] = "";
                    sg1_dr["sg1_t15"] = "";
                    sg1_dr["sg1_t16"] = "";

                    sg1_dt.Rows.Add(sg1_dr);


                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();


                    //********* Saving in Hidden Field                     
                    /*
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col3;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6");

                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = col1.Split('~')[1];
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = col1.Split('~')[2];
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t5")).Text = col1.Split('~')[3];
                    */

                    txtControl.Value = col1.Split('~')[1];
                    txtBtchno.Value = col1.Split('~')[2];
                    txtIDNo.Value = col1.Split('~')[3];

                    //VIPIN
                    col2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ibqty FROM ITEMOSP WHERE TRIM(ICODE)='" + txtIcode.Value.Trim() + "' AND TRIM(IBCODE)='" + col3 + "' and branchcd='" + frm_mbr + "'", "IBQTY");
                    col3 = Convert.ToString(fgen.make_double(col2) * fgen.make_double(txtlbl51.Text.Trim()));

                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = col3;


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
                case "SG1_ROW_TAX":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t16")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t12")).Focus();
                    break;
                case "SG1_ROW_DT":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    break;

                //case "sg1_Row_Tax_E":
                //    if (col1.Length <= 0) return;
                //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[27].Text = col1;
                //    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[28].Text = col2;
                //    setColHeadings();
                //    break;
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

                            sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                            sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();


                            sg4_dt.Rows.Add(sg4_dr);
                        }

                        sg4_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg4_add_blankrows();

                        ViewState["sg4"] = sg4_dt;
                        sg4.DataSource = sg4_dt;
                        sg4.DataBind();
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

                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.Trim();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();

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

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        if (edmode.Value == "Y")
                        {
                            //sg1_dr["sg1_f1"] = "*" + sg1.Rows[-1 + Convert.ToInt32(hf1.Value.Trim())].Cells[13].Text.Trim();

                            sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        }
                        else
                        {
                            sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        }

                        sg1_add_blankrows();

                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                    }
                    #endregion
                    setColHeadings();
                    break;
                case "STAGE":
                    if (col1.Length > 1)
                    {
                        txtStage.Value = col1 + "~" + col2;

                        create_tab();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();

                        btnMachine.Focus();
                    }
                    else btnStage.Focus();
                    break;
                case "MACHINE":
                    if (col1.Length > 1)
                    {
                        txtMachine.Value = col1 + "~" + col2;
                        btnIcode.Focus();
                    }
                    else btnMachine.Focus();
                    break;
                case "ERPCODE":
                    hf1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    var trcno = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
                    if (txtStage.Value.Substring(0, 2) == "61")
                    {
                        hf1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                        txtjobno.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "") + "-" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                        trcno = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "") + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "") + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                        txtlbl51v.Text = trcno;
                        hffield.Value = "2ND";
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("-", frm_qstr);
                    }
                    else
                    {
                        {
                            //hf1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                            //txtjobno.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "") + "-" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                            //trcno = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "") + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "") + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                            //txtlbl51v.Text = trcno;

                            txtlbl51v.Text = trcno;
                            txtjobno.Value = trcno.Substring(0, 6) + "-" + trcno.Substring(6, 10);
                            txtIcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                            txtiname.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

                            if (txtStage.Value.Length > 2)
                            {
                                string stage = txtStage.Value.Substring(0, 2);
                                txtlbl51.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9").ToString().Trim().Replace("&amp", "");
                                create_tab();
                                if (stage == "6G")
                                {
                                    calc(txtIcode.Value);
                                }
                                else
                                {
                                    txtlbl51v.Text = trcno;
                                    sg1_dr = sg1_dt.NewRow();
                                    sg1_dr["sg1_srno"] = i + 1;
                                    sg1_dr["sg1_f1"] = txtIcode.Value.Trim();
                                    sg1_dr["sg1_f2"] = txtiname.Value.Trim();
                                    sg1_dr["sg1_f3"] = "-";
                                    sg1_dr["sg1_f4"] = "-";
                                    sg1_dr["sg1_f5"] = fgen.SeekWipStock(frm_cocd, frm_qstr, frm_mbr, txtIcode.Value.Trim(), txtStage.Value.Split('~')[0], txtlbl51v.Text.Trim(), fromdt, todt, " where REVIS_NO='" + txtlbl51v.Text + "' and type || trim(vchnum) || to_Char(vchdate, 'dd/mm/yyyy') != '" + frm_vty + txtvchnum.Value.Trim() + txtvchdate.Value.Trim() + "'");
                                    sg1_dr["sg1_t1"] = fgen.make_double(txtlbl51.Text.Trim());
                                    sg1_dr["sg1_t3"] = txtControl.Value;
                                    sg1_dr["sg1_t4"] = txtBtchno.Value;
                                    sg1_dr["sg1_t5"] = txtIDNo.Value;
                                    sg1_dt.Rows.Add(sg1_dr);
                                }
                                ViewState["sg1"] = sg1_dt;
                                sg1.DataSource = sg1_dt;
                                sg1.DataBind();
                                sg1_dt.Dispose();

                                txtlbl51.AutoPostBack = false;
                            }
                        }
                    }
                    break;
                case "2ND":
                    if (col1.Length > 1)
                    {
                        txtIcode.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                        txtiname.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");

                        if (txtStage.Value.Length > 2)
                        {
                            string stage = txtStage.Value.Substring(0, 2);
                            if (stage != "61" && stage != "6G")
                            {
                                txtlbl51.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8").ToString().Trim().Replace("&amp", "");

                                create_tab();
                                sg1_dr = sg1_dt.NewRow();
                                sg1_dr["sg1_srno"] = i + 1;
                                sg1_dr["sg1_f1"] = txtIcode.Value.Trim();
                                sg1_dr["sg1_f2"] = txtiname.Value.Trim();
                                sg1_dr["sg1_f3"] = "-";
                                sg1_dr["sg1_f4"] = "-";
                                sg1_dr["sg1_f5"] = fgen.seek_iname(frm_qstr, frm_cocd, wipquery(txtIcode.Value.ToString().Trim(), txtlbl51v.Text.Trim()), "BAL");

                                sg1_dr["sg1_t1"] = fgen.make_double(txtlbl51.Text.Trim());
                                sg1_dr["sg1_t3"] = txtControl.Value;
                                sg1_dr["sg1_t4"] = txtBtchno.Value;
                                sg1_dr["sg1_t5"] = txtIDNo.Value;
                                sg1_dt.Rows.Add(sg1_dr);

                                ViewState["sg1"] = sg1_dt;
                                sg1.DataSource = sg1_dt;
                                sg1.DataBind();
                                sg1_dt.Dispose();

                                txtlbl51.AutoPostBack = false;
                            }
                        }
                    }
                    break;
                case "VALIDATEBOM":
                    if (col1 == "") return;
                    if (!col1.Contains("'")) col1 = "'" + col1 + "'";
                    calc(col1);
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

            SQuery = "select distinct a." + doc_nf.Value + " as Prod_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Prod_Dt,b.iName as product,b.cpartno,A.LOCATION AS MACHINE,a.iqtyin as qty,a.rej_rw as rej_Qty,a.stage,C.NAME AS STAGENAME,a.icode as erpCode,a.purpose as ctrlno,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a,item b,TYPE C where trim(A.icode)=trim(B.icode) AND TRIM(a.STAGE)=TRIM(C.TYPE1) AND C.ID='1' and a.branchcd='" + frm_mbr + "' and trim(a.type)='" + frm_vty + "'  and a.vchdate " + PrdRange + " order by vdd desc,a." + doc_nf.Value + "";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------

            i = 0;
            hffield.Value = "";

            #region Number Gen and Save to Table
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y" && Checked_ok == "Y")
            {
                try
                {
                    oDS = new DataSet();
                    oporow = null;
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                    // This is for checking that, is it ready to save the data
                    frm_vnum = "000000";
                    save_fun();

                    oDS.Dispose();
                    oporow = null;
                    oDS = new DataSet();
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                    if (edmode.Value == "Y")
                    {
                        frm_vnum = txtvchnum.Value.Trim();
                        save_it = "Y";
                    }
                    else
                    {
                        save_it = "Y";
                        //for (i = 0; i < sg1.Rows.Count - 0; i++)
                        //{
                        //    if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2)
                        //    {
                        //        save_it = "Y";
                        //    }
                        //}

                        if (save_it == "Y")
                        {

                            i = 0;
                            do
                            {
                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "'  AND VCHDATE " + DateRange + " ", 6, "vch");
                                pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Value.Trim(), "", frm_uname);
                                if (i > 20)
                                {
                                    fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 1 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "'  AND VCHDATE " + DateRange + "", 6, "vch");
                                    pk_error = "N";
                                    i = 0;
                                }
                                i++;
                            }
                            while (pk_error == "Y");
                        }
                    }

                    if (frm_vnum == "000000") btnhideF_Click(sender, e);

                    save_fun();

                    string ddl_fld1;
                    string ddl_fld2;
                    ddl_fld1 = frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    ddl_fld2 = frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    if (edmode.Value == "Y")
                    {
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||trim(type)||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + ddl_fld1 + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "' and type='39'");
                    }
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                    if (edmode.Value == "Y")
                    {
                        fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||trim(type)||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + ddl_fld2 + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||trim(type)||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD39" + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                    }
                    else
                    {
                        if (save_it == "Y")
                        {
                            //html_body = html_body + "Please note your SRF No : " + frm_vnum + "<br>";
                            //html_body = html_body + "ERP team will contact you in case of any further clarification required within next 3 working days. You can track your service request through SRF status also.<br>";
                            //html_body = html_body + "Always at your service, <br>";
                            //html_body = html_body + "ERP support <br>";

                            //fgen.send_mail(frm_cocd, "ERP ERP", txtlbl5.Value, "", "", "SRF : Query has been logged " + frm_vnum, html_body);

                            //fgen.msg("-", "AMSG", "SRF No " + frm_vnum + "'13'ERP team will contact you in case of any further clarification required within next 3 working days. You can track your service request through SRF status also.");
                            fgen.msg("-", "AMSG", "Data Saved");

                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Data Not Saved");
                        }
                    }
                    imgItem.ImageUrl = null;
                    //lblInfo.InnerText = "";
                    set_Val();
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

    }
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

    public void create_tab4()
    {
        sg4_dt = new DataTable();
        sg4_dr = null;
        // Hidden Field

        sg4_dt.Columns.Add(new DataColumn("sg4_SrNo", typeof(Int32)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t1", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t2", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t3", typeof(string)));
        sg4_dt.Columns.Add(new DataColumn("sg4_t4", typeof(string)));

    }

    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
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

        sg1_dr["sg1_t1"] = "-";
        sg1_dr["sg1_t2"] = "-";
        sg1_dr["sg1_t3"] = "0";
        sg1_dr["sg1_t4"] = "0";
        sg1_dr["sg1_t5"] = "0";
        sg1_dr["sg1_t6"] = "0";
        sg1_dr["sg1_t7"] = "0";
        sg1_dr["sg1_t8"] = "0";
        sg1_dr["sg1_t9"] = "-";
        sg1_dr["sg1_t10"] = "-";
        sg1_dr["sg1_t11"] = "-";
        sg1_dr["sg1_t12"] = "-";
        sg1_dr["sg1_t13"] = "-";
        sg1_dr["sg1_t14"] = "-";
        sg1_dr["sg1_t15"] = "-";
        sg1_dr["sg1_t16"] = "-";

        sg1_dt.Rows.Add(sg1_dr);
    }
    public void sg2_add_blankrows()
    {
        sg2_dr = sg2_dt.NewRow();


        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
        sg2_dr["sg2_t1"] = "-";
        sg2_dr["sg2_t2"] = "-";
        sg2_dt.Rows.Add(sg2_dr);
    }
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

    public void sg4_add_blankrows()
    {
        sg4_dr = sg4_dt.NewRow();


        sg4_dr["sg4_SrNo"] = sg4_dt.Rows.Count + 1;
        sg4_dr["sg4_t1"] = "-";
        sg4_dr["sg4_t2"] = "-";
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
                    if (sg1.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
                    {
                        sg1.Rows[sg1r].Cells[j].Text = sg1.Rows[sg1r].Cells[j].Text.Substring(0, 35);
                    }
                }
            }

            sg1.Columns[10].HeaderStyle.CssClass = "hidden";
            e.Row.Cells[10].CssClass = "hidden";
            sg1.Columns[11].HeaderStyle.CssClass = "hidden";
            e.Row.Cells[11].CssClass = "hidden";

            sg1.HeaderRow.Cells[12].Text = "Srno";
            sg1.HeaderRow.Cells[13].Text = "Erp Code";
            sg1.HeaderRow.Cells[14].Text = "Product";
            sg1.HeaderRow.Cells[15].Text = "Part No";
            sg1.HeaderRow.Cells[16].Text = "Unit";
            sg1.HeaderRow.Cells[17].Text = "WIP.Stk";

            sg1.HeaderRow.Cells[18].Text = "Qty Consume";


            sg1.Columns[19].HeaderStyle.CssClass = "hidden";
            e.Row.Cells[19].CssClass = "hidden";

            sg1.HeaderRow.Cells[20].Text = "Rejection";
            if (frm_mbr == "01" && frm_cocd == "HIME")
                sg1.HeaderRow.Cells[20].Text = "Scrap";

            sg1.HeaderRow.Cells[21].Text = "Control.No";
            sg1.HeaderRow.Cells[22].Text = "Heat.No";
            sg1.HeaderRow.Cells[23].Text = "Id.No";

            i = 24;
            do
            {
                sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                e.Row.Cells[i].CssClass = "hidden";
                i++;
            }
            while (i != 36);
        }
    }

    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG1_RMV":

                break;
            case "SG1_ROW_TAX":

                break;
            case "SG1_ROW_DT":

                break;

            case "SG1_ROW_ADD":
                hf1.Value = index.ToString();
                if (index < sg1.Rows.Count - 1)
                {
                    hffield.Value = "SG1_ROW_ADD_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("-", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("-", frm_qstr);
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

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG2_RMV":

                break;
            case "SG2_ROW_ADD":

                break;
        }
    }

    //------------------------------------------------------------------------------------
    protected void sg3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg3.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG3_RMV":

                break;
            case "SG3_ROW_ADD":

                break;
        }
    }
    protected void sg4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg4.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "sg4_RMV":

                break;
            case "sg4_ROW_ADD":

                break;
        }
    }

    //------------------------------------------------------------------------------------


    //------------------------------------------------------------------------------------

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
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        double rejqty = 0;
        int i = 0;
        // type in 39
        foreach (GridViewRow gr in sg1.Rows)
        {
            i++;
            if (fgen.make_double(((TextBox)gr.FindControl("sg1_t1")).Text) > 0)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = "39";
                oporow["MORDER"] = i;
                oporow[doc_nf.Value] = frm_vnum;
                oporow[doc_df.Value] = txtvchdate.Value.Trim();

                oporow["icode"] = gr.Cells[13].Text.Trim();

                oporow["iqtyin"] = "0";
                oporow["iqtyout"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t1")).Text);
                oporow["REJ_RW"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t2")).Text);
                rejqty += fgen.make_double(((TextBox)gr.FindControl("sg1_t2")).Text);

                oporow["stage"] = txtStage.Value.Trim().Split('~')[0].ToString();
                oporow["store"] = "W";
                oporow["revis_no"] = txtlbl51v.Text;

                oporow["BILLRATE"] = fgen.make_double(txtOBSODmin.Text);
                oporow["SHE_CESS"] = fgen.make_double(txtOBSODmax.Text);

                oporow["BILLAMT"] = fgen.make_double(txtOBSthikmin.Text);
                oporow["PAPCESS"] = fgen.make_double(txtOBSthikmax.Text);

                oporow["DOCSRNO"] = ddEdgePre.SelectedItem.Text;
                oporow["COL1"] = ddDimension.SelectedItem.Text;

                oporow["btchno"] = ((TextBox)gr.FindControl("sg1_t4")).Text.Trim();
                oporow["purpose"] = ((TextBox)gr.FindControl("sg1_t3")).Text.Trim();
                oporow["tc_no"] = ((TextBox)gr.FindControl("sg1_t5")).Text.Trim();

                oporow["INVNO"] = txtjobno.Value.Split('-')[0];
                oporow["INVDATE"] = txtjobno.Value.Split('-')[1];

                oporow["location"] = txtMachine.Value;

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

        // type in 15
        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = frm_mbr;
        oporow["TYPE"] = frm_vty;
        oporow[doc_nf.Value] = frm_vnum;
        oporow[doc_df.Value] = txtvchdate.Value.Trim();
        oporow["MORDER"] = 0;
        oporow["icode"] = txtIcode.Value.Trim();
        oporow["iqtyin"] = fgen.make_double(txtlbl51.Text.Trim()) - rejqty;
        oporow["rej_rw"] = rejqty;
        oporow["iqtyout"] = "0";
        oporow["stage"] = txtStage.Value.Trim().Split('~')[0].ToString();
        oporow["store"] = "W";
        oporow["BILLRATE"] = fgen.make_double(txtOBSODmin.Text);
        oporow["SHE_CESS"] = fgen.make_double(txtOBSODmax.Text);
        oporow["revis_no"] = txtlbl51v.Text;

        oporow["BILLAMT"] = fgen.make_double(txtOBSthikmin.Text);
        oporow["PAPCESS"] = fgen.make_double(txtOBSthikmax.Text);

        oporow["DOCSRNO"] = ddEdgePre.SelectedItem.Text;
        oporow["COL1"] = ddDimension.SelectedItem.Text;

        oporow["btchno"] = txtBtchno.Value;
        oporow["purpose"] = txtControl.Value;
        oporow["tc_no"] = txtIDNo.Value;

        oporow["location"] = txtMachine.Value;
        oporow["INVNO"] = txtjobno.Value.Split('-')[0];
        oporow["INVDATE"] = txtjobno.Value.Split('-')[1];

        if (txtStage.Value.Split('~')[0].ToString() == "6G")
        {
            oporow["store"] = "W";
        }
        oporow["INSPECTED"] = "N";
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

        //if (rejqty > 0 && fgcode.Length > 0)
        //{
        //    // type in 15
        //    {
        //        {
        //            oporow = oDS.Tables[0].NewRow();
        //            oporow["BRANCHCD"] = frm_mbr;
        //            oporow["TYPE"] = frm_vty;
        //            oporow[doc_nf.Value] = frm_vnum;
        //            oporow[doc_df.Value] = txtvchdate.Value.Trim();

        //            oporow["icode"] = fgcode;
        //            oporow["iqtyin"] = rejqty;
        //            oporow["rej_rw"] = 0;
        //            oporow["iqtyout"] = "0";

        //            oporow["stage"] = txtStage.Value.Trim().Split('~')[0].ToString();
        //            oporow["store"] = "Y";

        //            oporow["BILLRATE"] = fgen.make_double(txtOBSODmin.Text);
        //            oporow["SHE_CESS"] = fgen.make_double(txtOBSODmax.Text);

        //            oporow["BILLAMT"] = fgen.make_double(txtOBSthikmin.Text);
        //            oporow["PAPCESS"] = fgen.make_double(txtOBSthikmax.Text);

        //            oporow["DOCSRNO"] = ddEdgePre.SelectedItem.Text;
        //            oporow["COL1"] = ddDimension.SelectedItem.Text;

        //            oporow["btchno"] = txtBtchno.Value;
        //            oporow["purpose"] = txtControl.Value;
        //            oporow["tc_no"] = txtIDNo.Value;

        //            oporow["location"] = txtMachine.Value;

        //            if (txtStage.Value.Split('~')[0].ToString() == "69")
        //            {
        //                //oporow["store"] = "Y";
        //            }
        //            oporow["INSPECTED"] = "Y";
        //            if (edmode.Value == "Y")
        //            {
        //                oporow["eNt_by"] = ViewState["entby"].ToString();
        //                oporow["eNt_dt"] = ViewState["entdt"].ToString();
        //                oporow["edt_by"] = frm_uname;
        //                oporow["edt_dt"] = vardate;
        //            }
        //            else
        //            {
        //                oporow["eNt_by"] = frm_uname;
        //                oporow["eNt_dt"] = vardate;
        //                oporow["edt_by"] = "-";
        //                oporow["edt_dt"] = vardate;
        //            }
        //            oDS.Tables[0].Rows.Add(oporow);
        //        }
        //    }
        //}
    }
    void Acode_Sel_query()
    {

    }
    void Icode_Sel_query()
    {

    }

    void Type_Sel_query()
    {


    }

    //------------------------------------------------------------------------------------   
    protected void sg4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg1r = 0; sg1r < sg4.Rows.Count; sg1r++)
            {
                for (int j = 0; j < sg4.Columns.Count; j++)
                {
                    sg4.Rows[sg1r].Cells[j].ToolTip = sg4.Rows[sg1r].Cells[j].Text;
                    if (sg4.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
                    {
                        sg4.Rows[sg1r].Cells[j].Text = sg4.Rows[sg1r].Cells[j].Text.Substring(0, 35);
                    }
                }
            }
            e.Row.Cells[0].Style["display"] = "none";
            sg4.HeaderRow.Cells[0].Style["display"] = "none";
            e.Row.Cells[1].Style["display"] = "none";
            sg4.HeaderRow.Cells[1].Style["display"] = "none";
        }
    }


    protected void btnJobNo_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "JOBNO";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnIcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ERPCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnStage_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "STAGE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void txtlbl51_TextChanged(object sender, EventArgs e)
    {
        hffield.Value = "VALIDATEBOM";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    void calc(string bomno)
    {
        if (txtStage.Value.Trim().Length > 1)
        {
            if (txtStage.Value.Trim().Split('~')[0] == "61" || txtStage.Value.Trim().Split('~')[0] == "6G")
            {
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT A.IBCODE,B.INAME,B.CPARTNO,B.UNIT,a.ibqty FROM ITEMOSP A,ITEM B where TRIM(A.IBCODE)=TRIM(B.ICODE) AND TRIM(A.ICODE)='" + txtIcode.Value.Trim() + "' and a.branchcd='" + frm_mbr + "' and trim(a.icode) in (" + bomno + ") order by a.ibcode ");
                if (dt.Rows.Count > 0)
                {
                    i = 0;
                    create_tab();
                    foreach (DataRow dr in dt.Rows)
                    {
                        sg1_dr = sg1_dt.NewRow();
                        sg1_dr["sg1_srno"] = i + 1;
                        sg1_dr["sg1_f1"] = dr["ibcode"].ToString().Trim();
                        sg1_dr["sg1_f2"] = dr["iname"].ToString().Trim();
                        sg1_dr["sg1_f3"] = dr["cpartno"].ToString().Trim();
                        sg1_dr["sg1_f4"] = dr["unit"].ToString().Trim();
                        //sg1_dr["sg1_f5"] = fgen.seek_iname(frm_qstr, frm_cocd, wipquery(dr["ibcode"].ToString().Trim(), txtlbl51v.Text.Trim()), "BAL");
                        sg1_dr["sg1_f5"] = fgen.SeekWipStock(frm_cocd, frm_qstr, frm_mbr, dr["ibcode"].ToString().Trim(), txtStage.Value.Split('~')[0], txtlbl51v.Text.Trim(), fromdt, todt, " where REVIS_NO='" + txtlbl51v.Text + "' and type || trim(vchnum) || to_Char(vchdate, 'dd/mm/yyyy') != '" + frm_vty + txtvchnum.Value.Trim() + txtvchdate.Value.Trim() + "'");

                        sg1_dr["sg1_t1"] = fgen.make_double(dr["ibqty"].ToString().Trim()) * fgen.make_double(txtlbl51.Text.Trim());
                        sg1_dt.Rows.Add(sg1_dr);

                        i++;
                    }
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    sg1_dt.Dispose();
                }
                else
                {
                    fgen.msg("-", "AMSG", "BOM not entered for " + txtiname.Value);
                    return;
                }
            }
            else
            {
                i = 0;
                create_tab();
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_srno"] = i + 1;
                sg1_dr["sg1_f1"] = txtIcode.Value.Trim();
                sg1_dr["sg1_f2"] = txtiname.Value.Trim();
                sg1_dr["sg1_f3"] = "-";
                sg1_dr["sg1_f4"] = "-";
                sg1_dr["sg1_f5"] = "-";

                sg1_dr["sg1_t1"] = fgen.make_double(txtlbl51.Text.Trim());
                sg1_dr["sg1_t3"] = txtControl.Value;
                sg1_dr["sg1_t4"] = txtBtchno.Value;
                sg1_dr["sg1_t5"] = txtIDNo.Value;
                sg1_dt.Rows.Add(sg1_dr);

                ViewState["sg1"] = sg1_dt;
                sg1.DataSource = sg1_dt;
                sg1.DataBind();
                sg1_dt.Dispose();
            }
        }
        else
        {
            fgen.msg("-", "AMSG", "Please Select Stage First");
        }
    }
    protected void btnMachine_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MACHINE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    string wipquery(string icode, string trackno)
    {
        //SQuery = "SELECT ICODE,SUM(IQTYIN) AS QTYIN,SUM(IQTYOUT) AS QTYOUT,SUM(IQTYIN)-SUM(IQTYOUT) AS BAL FROM (" +
        //    "SELECT TRIM(ICODE) ICODE,IQTYOUT AS IQTYIN,0 AS IQTYOUT FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '3%' AND TYPE!='39' AND VCHDATE " + DateRange + " AND " +
        //    "TRIM(ICODE)='" + icode + "' UNION ALL " +
        //    "SELECT TRIM(ICODE) ICODE,0 AS IQTYIN,IQTYIN AS IQTYOUT FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '1%' " +
        //    "AND TYPE<'15' AND VCHDATE " + DateRange + " AND TRIM(ICODE)='" + icode + "' " +
        //    "UNION ALL SELECT TRIM(ICODE),IQTYIN,0 AS IQTYIN FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '1*5%' AND " +
        //    "VCHDATE " + DateRange + " AND TRIM(ICODE)='" + icode + "' and trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Value + txtvchdate.Value + "' UNION ALL" +
        //    " SELECT TRIM(ICODE),0 AS IQTYIN,IQTYOUT FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '39%' " +
        //    "AND VCHDATE " + DateRange + " AND TRIM(ICODE)='" + icode + "' and " +
        //    "trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Value + txtvchdate.Value + "' ) GROUP BY ICODE ";
        SQuery = "SELECT ICODE,SUM(IQTYIN) AS QTYIN,SUM(IQTYOUT) AS QTYOUT,SUM(IQTYIN)-SUM(IQTYOUT) AS BAL FROM (" +
            "SELECT TRIM(ICODE) ICODE,IQTYOUT AS IQTYIN,0 AS IQTYOUT FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '3%' AND TYPE!='39' AND VCHDATE " + DateRange + " AND " +
            "TRIM(ICODE)='" + icode + "' AND REVIS_NO = '" + trackno + "' AND STORE='Y' UNION ALL " +
            "SELECT TRIM(ICODE) ICODE,0 AS IQTYIN,IQTYIN AS IQTYOUT FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '1%' " +
            "AND TYPE<'15' AND VCHDATE " + DateRange + " AND TRIM(ICODE)='" + icode + "'  AND REVIS_NO = '" + trackno + "' AND STORE='Y' " +
            "UNION ALL SELECT TRIM(ICODE),IQTYIN,0 AS IQTYIN FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '1*5%' AND " +
            "VCHDATE " + DateRange + " AND TRIM(ICODE)='" + icode + "' and trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Value + txtvchdate.Value + "' AND REVIS_NO = '" + trackno + "' AND STORE='Y' UNION ALL" +
            " SELECT TRIM(ICODE),0 AS IQTYIN,IQTYOUT FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '39%' " +
            "AND VCHDATE " + DateRange + " AND TRIM(ICODE)='" + icode + "' and " +
            "trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Value + txtvchdate.Value + "' AND REVIS_NO = '" + trackno + "' AND STORE='W') GROUP BY ICODE ";
        //if (txtStage.Value.Length > 2)
        //{
        //    if (txtStage.Value.Substring(0, 2) != "61")
        //    {
        //        SQuery = "SELECT ICODE,SUM(IQTYIN) AS QTYIN,SUM(IQTYOUT) AS QTYOUT,SUM(IQTYIN)-SUM(IQTYOUT) AS BAL FROM (SELECT TRIM(ICODE) ICODE,IQTYOUT AS IQTYIN,0 AS IQTYOUT FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '3%' AND TYPE!='39' AND VCHDATE " + DateRange + " AND TRIM(ICODE)='" + icode + "' UNION ALL SELECT TRIM(ICODE) ICODE,0 AS IQTYIN,IQTYIN AS IQTYOUT FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '1%' AND TYPE<15 AND VCHDATE " + DateRange + " AND TRIM(ICODE)='" + icode + "' UNION ALL SELECT TRIM(ICODE),IQTYIN,0 AS IQTYIN FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '15%' AND VCHDATE " + DateRange + " AND TRIM(ICODE)='" + icode + "' and trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Value + txtvchdate.Value + "' UNION ALL SELECT TRIM(ICODE),0 AS IQTYIN,IQTYOUT FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '39%' AND VCHDATE " + DateRange + " AND TRIM(ICODE)='" + icode + "' and trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Value + txtvchdate.Value + "' ) GROUP BY ICODE ";
        //    }
        //}
        return SQuery;
    }
}