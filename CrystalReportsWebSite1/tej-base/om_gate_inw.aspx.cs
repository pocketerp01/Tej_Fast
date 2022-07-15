using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;


public partial class om_gate_inw : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
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
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, chk_opt, frm_IndType;

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
                    frm_IndType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }
            hfQstr.Value = frm_qstr;
            if (!Page.IsPostBack)
            {
                doc_addl.Value = "1";
                lbl1a.Text = "00";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_MAX_MRR", "0");
                col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select PARAMS from controls where id='M50'", "PARAMS");
                if (col1.toDouble() > 0)
                {
                    chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select ENABLE_YN from STOCK where id='M035'", "ENABLE_YN");
                    if (chk_opt == "Y")
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_MAX_MRR", col1);
                }

                string wb_cash_lim = "";
                wb_cash_lim = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(OPT_PARAM) As OPT_PARAM from fin_Rsys_opt where opt_enable='Y' and OPT_id='W0067'", "OPT_PARAM");
                if (fgen.make_double(wb_cash_lim) > 0)
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_MAX_MRR", wb_cash_lim);
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

        if (dtCol == null) return;
        if (sg1.Rows.Count <= 0) return;
        for (int sR = 0; sR < sg1.Columns.Count; sR++)
        {
            string orig_name;
            double tb_Colm;
            tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
            orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();

            //txtlbl8.BackColor = System.Drawing.ColorTranslator.FromHtml("#DAF7A6");
            //txtlbl9.BackColor = System.Drawing.ColorTranslator.FromHtml("#DAF7A6");
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

                ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t5")).Attributes.Add("autocomplete", "off");

                if (txtlbl4.Text == "PO" && (frm_cocd != "MASS" && frm_cocd != "MAST"))
                {
                    ((TextBox)sg1.Rows[K].FindControl("sg1_t5")).Attributes.Add("readonly", "readonly");
                }

                ((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("readonly", "readonly");

                if (sg1.Rows[K].Cells[13].Text.Trim().Length > 4)
                {
                    if (sg1.Rows[K].Cells[13].Text.Trim().Substring(0, 2) == "07" || sg1.Rows[K].Cells[13].Text.Trim().Substring(0, 2) == "08" || sg1.Rows[K].Cells[13].Text.Trim().Substring(0, 2) == "09")
                    {
                        sg1.HeaderRow.Cells[20].Text = "Rcv(REEL)";
                    }
                }
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
                sg1.HeaderRow.Cells[sR].Text = sg1.HeaderRow.Cells[sR].Text.Replace(" /n ", "<br/>");
                // Setting Col Width
                string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
                if (fgen.make_double(mcol_width) > 0)
                {
                    //sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
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
        tab2.Visible = false;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        //switch (Prg_Id)
        //{
        //    case "M09024":
        //    case "M10003":
        //    case "M11003":
        //    case "M10012":
        //    case "M11012":
        //    case "M12008":
        //        tab3.Visible = false;
        //        tab4.Visible = false;
        //        break;
        //}
        //if (Prg_Id == "M12008")
        //{
        //    tab5.Visible = true;
        //    txtlbl8.Attributes.Remove("readonly");
        //    txtlbl9.Attributes.Remove("readonly");
        //}
        fgen.SetHeadingCtrl(this.Controls, dtCol);


        switch (txtlbl4.Text.Trim().ToUpper())
        {
            case "PO":
                sg1.HeaderRow.Cells[18].Text = "P.O.Qty";
                sg1.HeaderRow.Cells[19].Text = "Chl.Qty";
                break;
            case "OT":
                sg1.HeaderRow.Cells[18].Text = "OT.Qty";
                sg1.HeaderRow.Cells[19].Text = "Chl.Qty";
                break;
            case "BI":
                sg1.HeaderRow.Cells[18].Text = "BI.Qty";
                sg1.HeaderRow.Cells[19].Text = "Chl.Qty";
                break;
            case "RG":
                sg1.HeaderRow.Cells[18].Text = "RGP.Qty";
                sg1.HeaderRow.Cells[19].Text = "Chl.Qty";
                break;
            case "JO":
                sg1.HeaderRow.Cells[18].Text = "JOB.Qty";
                sg1.HeaderRow.Cells[19].Text = "Chl.Qty";
                break;
            case "CH":
                sg1.HeaderRow.Cells[18].Text = "CH.Qty";
                sg1.HeaderRow.Cells[19].Text = "Chl.Qty";
                break;
        }
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false; //BY MADHVI ON 30 JULY 2018
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
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
        btnCamera.Disabled = true;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnprint.Disabled = true; btnlist.Disabled = true; //BY MADHVI ON 30 JULY 2018
        btnlbl4.Enabled = true;
        btnlbl7.Enabled = true; btnCamera.Disabled = false;
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
        frm_tabname = "ivoucherp";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "00");

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

            case "TACODE":
                //pop1
                SQuery = "SELECT 'PO' as Fstr,'PO Based' as Name,'PO' as Code from dual union all SELECT 'BI' as Fstr,'Customer Return' as Name,'BI' as Code from dual union all SELECT 'OT' as Fstr,'Non PO Purchase' as Name,'OT' as Code from dual union all SELECT 'RG' as Fstr,'RGP (After Job Work)' as Name,'RG' as Code from dual union all SELECT 'JO' as Fstr,'Material (For Job Work)' as Name,'JO' as Code from dual union all SELECT 'CH' as Fstr,'Inter Unit Challan' as Name,'CH' as Code from dual";
                break;
            case "TICODE":
                //pop2
                switch (txtlbl4.Text)
                {
                    case "PO":
                        SQuery = "SELECT distinct a.Acode as FStr,b.Aname as Supplier,b.Acode,b.Addr1,b.Addr2,b.GST_No AS " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.Staten from pomas a,famst b where a.branchcd='" + frm_mbr + "' and a.type like '5%' and (trim(a.chk_by)!='-' or trim(a.app_by)!='-') and a.pflag!=1 and trim(A.acodE)=trim(B.acode) and length(trim(nvl(b.deac_by,'-'))) <2 order by b.Aname ";
                        break;
                    case "BI":
                        SQuery = "SELECT distinct a.Acode as FStr,b.Aname as Customer,b.Acode,b.Addr1,b.Addr2,b.GST_No AS " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.Staten from ivoucher a,famst b where a.branchcd='" + frm_mbr + "' and a.type like '4%'  and trim(A.acodE)=trim(B.acode) and length(trim(nvl(b.deac_by,'-'))) <2 order by b.Aname ";
                        break;
                    case "OT":
                        SQuery = "SELECT distinct a.Acode as FStr,a.Aname as Customer,a.Acode,a.Addr1,a.Addr2,a.GST_No AS " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.Staten from famst a where  length(trim(nvl(a.deac_by,'-'))) <2 and substr(a.acode,1,2) in ('02','05','06','14','15')  order by a.Aname ";
                        break;
                    case "RG":
                        SQuery = "SELECT distinct a.Acode as FStr,b.Aname as Supplier,b.Acode,b.Addr1,b.Addr2,b.GST_No AS " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.Staten from ivoucher a,famst b where a.branchcd='" + frm_mbr + "' and a.type like '2%' and trim(A.acodE)=trim(B.acode) and length(trim(nvl(b.deac_by,'-'))) <2 order by b.Aname ";
                        break;
                    case "JO":
                        //SQuery = "SELECT distinct a.Acode as FStr,b.Aname as Customer,b.Acode,b.Addr1,b.Addr2,b.GST_No AS " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",b.Staten from Somas a,famst b where a.branchcd='" + frm_mbr + "' and a.type like '4%' and trim(nvl(a.app_by,'-'))!='-' and trim(A.acodE)=trim(B.acode) and length(trim(nvl(b.deac_by,'-'))) <2 order by b.Aname ";
                        SQuery = "SELECT distinct a.Acode as FStr,a.Aname as Customer,a.Acode,a.Addr1,a.Addr2,a.GST_No AS " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.Staten from famst a where length(trim(nvl(a.deac_by,'-'))) <2  and substr(a.acode,1,2) in ('16','14','15') order by a.Aname ";
                        break;
                    case "CH":
                        SQuery = "SELECT distinct a.Acode as FStr,a.Aname as Customer,a.Acode,a.Addr1,a.Addr2,a.GST_No AS " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + ",a.Staten from famst a where length(trim(nvl(a.deac_by,'-'))) <2 and substr(a.acode,1,2) in ('02')  order by a.Aname ";
                        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "sELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "ACODE");
                        SQuery = "select trim(a.Fstr) as fstr,a.acode as party_code,b.aname as party,a.vchnum as chl,A.vchdate as chldt ,(a.Qtyord)-(a.Soldqty) as Bal_Qty,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty from (select fstr,acode,vchnum,vchdate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(b.ACODE)||'-'||to_ChaR(a.vchdate,'YYYYMMDD')||'-'||trim(a.vchnum) as fstr,trim(b.ACODE) as acode,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.iqtyout as Qtyord,0 as Soldqty from ivoucher a,type b where trim(a.branchcd)=trim(b.type1) and b.id='B' and a.branchcd!='" + frm_mbr + "' and a.type in ('29') and a.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') union all SELECT trim(acode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||lpad(trim(refnum),6,'0') as fstr,trim(Acode) as acode,lpad(trim(refnum),6,'0') as vchnum,to_char(refdate,'dd/mm/yyyy') as vchdate,0 as Qtyord,iqty_chl as qtyord from ivoucherp where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') )  group by fstr,acode,vchnum,vchdate having sum(Qtyord)-sum(Soldqty)>0  ) a,famst b where trim(A.acode)=trim(b.acodE) order by a.vchnum,trim(a.fstr)";
                        break;
                }
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                //foreach (GridViewRow gr in sg1.Rows)
                //{
                //    if (gr.Cells[13].Text.Trim().Length > 2)
                //    {
                //        if (col1.Length > 0) col1 = col1 + "," + "'" + gr.Cells[13].Text.Trim() + "'";
                //        else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                //    }
                //}
                //if (col1.Length > 0) 
                //{
                //    col1 = " and TRIM(a.icode) not in (" + col1 + ")";
                //}    

                //else
                //{
                //    col1 = "";
                //}
                switch (txtlbl4.Text)
                {
                    case "PO":
                        SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Maker,b.Cpartno as Part_no,(a.Qtyord)-(a.Soldqty) as Bal_Qty,a.Prate,b.Cdrgno,b.Unit,substr(a.fstr,19,6) as PO_NO" +
                            ",b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty,null as btchno,null as btchdt from (select fstr,ERP_code,max(prate) as prate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty " +
                            "from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate " +
                            "from pomas where branchcd='" + frm_mbr + "' and type like '5%' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and substr(app_by,1,2)!='(R' and orddt>=to_Date('01/04/2017','dd/mm/yyyy') " +
                            "and trim(Acode)='" + txtlbl7.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as " +
                            "ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate from ivoucherp where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and" +
                            " trim(Acode)='" + txtlbl7.Text.Trim() + "' UNION ALL SELECT trim(icode) as fstr,trim(Icode) as ERP_code,1 as Qtyord,0 as Soldqty,0 as irate" +
                            " from ITEM where branchcd = '00' and length(trim(icode))>5 AND SUBSTR(ICODE,1,2)= '59')  group by fstr,ERP_code having (case when sum(Qtyord)>0 then sum(Qtyord)-sum(Soldqty) else max(prate) end)>0  )a,item b where" +
                            " trim(a.erp_code)=trim(B.icode) and length(trim(b.icode))>5 order by B.Iname,trim(a.fstr)";
                        break;
                    case "BI":
                        SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Maker,b.Cpartno as Part_no,a.irate as Prate,b.Cdrgno,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty,null as btchno,null as btchdt from " +
                            "(select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (" +
                            "SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(to_Char(srno,'9999')),4,'0') as fstr,trim(Icode) as ERP_code" +
                            ",Qtyord,0 as Soldqty,((irate*(100-cdisc)/100)-0) as irate from somas where branchcd='" + frm_mbr + "' and type like '4%' and trim(app_by)!='-' and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' UNION ALL SELECT trim(icode)||'-'||to_ChaR(PODATE,'YYYYMMDD')||'-'||PONUM||'-'||lpad(trim(to_Char(MORDER,'9999')),4,'0') as" +
                            " fstr,trim(Icode) as ERP_code,IQTYOUT,0 as Soldqty,((irate*(100-ICHGS)/100)-0) as irate from IVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate from ivoucherp where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "'" +
                            " UNION ALL SELECT trim(icode) as fstr,trim(Icode) as ERP_code,1 as Qtyord,0 as Soldqty,0 as irate" +
                            " from ITEM where branchcd = '00' and length(trim(icode))> 4 AND SUBSTR(ICODE,1,2)= '59')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and length(trim(b.icode))>5 order by B.Iname,trim(a.fstr)";
                        break;
                    case "OT":
                        SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Maker,a.cpartno,a.cdrgno,a.unit,null as btchno,null as btchdt from item a where " +
                            "length(trim(nvl(a.deac_by,'-'))) <2 /*AND substr(a.ICODE,1,1) in ('8','5','9')*/ order by a.Iname ";
                        break;
                    case "RG":
                        SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Maker,b.Cpartno as Part_no,a.irate as Prate,b.Cdrgno,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,substr(trim(a.Fstr),19,6) as RGP_no,b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty,null as btchno,null as btchdt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from " +
                            "(SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate from rgpmst where branchcd='" + frm_mbr + "' and type in ('21','23','26') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr," +
                            "trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate from ivoucherp where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' UNION ALL SELECT trim(icode) as fstr,trim(Icode) as ERP_code,1 as Qtyord,0 as Soldqty,0 as irate" +
                            " from ITEM where branchcd = '00' and length(trim(icode))> 4 AND SUBSTR(ICODE,1,2)= '59')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and length(trim(b.icode))>5 order by B.Iname,trim(a.fstr)";
                        break;
                    case "JO":
                        //SQuery = "SELECT distinct a.ordno||to_char(a.orddt,'dd/mm/yyyy')||trim(a.Icode) as FStr,b.Iname as Item_Name,a.Ordno,to_char(A.Orddt,'dd/mm/yyyy') as Ord_dtd,a.Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit from somas a,item b where a.branchcd='" + frm_mbr + "' and a.type like '41%' and trim(nvl(a.app_by,'-'))!='-'  and trim(A.IcodE)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2 and trim(a.acode)='" + txtlbl7.Text + "' order by b.Iname ";
                        SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Maker,a.cpartno,a.cdrgno,a.unit from item a " +
                            "where  length(trim(nvl(a.deac_by,'-'))) <2 /*AND substr(a.ICODE,1,1) in ('8','5','9')*/ order by a.Iname ";
                        break;
                    case "CH":
                        //SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit from item a where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 order by a.Iname ";
                        SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Maker,b.Cpartno as Part_no,a.irate as Prate,b.Cdrgno,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty,a.btchno,a.btchdt from " +
                            "(select fstr,ERP_code,btchno,btchdt,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate,btchno,btchdt from ivoucher where " +
                            "branchcd!='" + frm_mbr + "' and type in ('29') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord" +
                            ",iqty_chl as qtyord,0 as irate,btchno,btchdt from ivoucherp where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' UNION ALL SELECT trim(icode) as fstr,trim(Icode) as ERP_code,1 as Qtyord,0 as Soldqty,0 as irate,'0' as btchno,'0' as btchdt" +
                            " from ITEM where branchcd = '00' and length(trim(icode))> 4 AND SUBSTR(ICODE,1,2)= '59')  group by fstr,ERP_code,btchno,btchdt having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode)  order by B.Iname,trim(a.fstr)";
                        SQuery = "select trim(a.Fstr) as fstr,trim(a.Fstr) as f1,(a.Qtyord)-(a.Soldqty) as Bal_Qty,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty from (" +
                            "select fstr,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (" +
                            "SELECT trim(acode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||trim(vchnum) as fstr,iqtyout as Qtyord,0 as Soldqty, IRATE from ivoucher where branchcd!='" + frm_mbr + "' and type in ('29') and " +
                            "vchdate>=to_Date('01/04/2017','dd/mm/yyyy') union all SELECT trim(acode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(refnum) as fstr,0 as Qtyord,iqty_chl as qtyord, 0 AS IRATE from ivoucherp" +
                            " where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') UNION ALL SELECT trim(icode) as fstr,trim(Icode) as ERP_code,1 as Qtyord,0 as Soldqty,0 as irate" +
                            " from ITEM where branchcd = '00' and length(trim(icode))> 4 AND SUBSTR(ICODE,1,2)= '59')  group by fstr having sum(Qtyord)-sum(Soldqty)>0  )a order by trim(a.fstr)";
                        break;
                }
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);
                break;
            case "SG1_ROW_ADD_ALL":
            case "SG1_ROW_ADD_E_ALL":
                SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.Maker,a.cpartno,a.cdrgno,a.unit,null as btchno,null as btchdt from item a where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 order by a.Iname ";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);
                hffield.Value = btnval.Replace("_ALL", "");
                break;
            case "AFCH":
                SQuery = "select trim(a.Fstr) as fstr,trim(a.Fstr) as f1,(a.Qtyord)-(a.Soldqty) as Bal_Qty,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty from (select fstr,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT to_ChaR(vchdate,'YYYYMMDD')||'-'||trim(vchnum) as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate,btchno,btchdt from ivoucher where branchcd!='" + frm_mbr + "' and type in ('29') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' union all SELECT to_ChaR(invdate,'YYYYMMDD')||'-'||trim(invno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate,btchno,btchdt from ivoucherp where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "')  group by fstr having sum(Qtyord)-sum(Soldqty)>0  )a order by trim(a.fstr)";
                //fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);
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

                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(trim(icode))>5 and icode like '9%' and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname  ";

                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(trim(icode))>5 and icode like '9%' and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname  ";
                break;
            case "sg1_t2":
                SQuery = "SELECT DISTINCT TO_cHAR(A.ORDDT,'YYYYMMDD')||'-'||TRIM(A.ORDNO)||'-'||A.BRANCHCD||A.TYPE AS FSTR,A.ORDNO AS PONO,TO_CHAr(A.ORDDT,'DD/MM/YYYYY') AS PODT,B.ANAME AS PARTY,A.ACODE,TO_cHAR(A.ORDDT,'YYYYMMDD') AS VDD FROM POMAS A,FAMST B WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '53%' AND A.ORDDT " + DateRange + " AND TRIM(A.ACODE)='" + txtlbl7.Text.Trim() + "' order by vdd desc ";
                // ADD BY MADHVI ON 11/05/2019 .... QUERY GIVEN BY MAYURI MAM
                SQuery = "Select A.BRANCHCD||A.TYPE||TRIM(A.ORDNO)||TO_cHAR(A.ORDDT,'YYYYMMDD')||TRIM(A.ICODE) AS FSTR,a.Ordno as Order_No,to_char(a.Orddt,'dd/mm/yyyy') as Dated,b.iname,a.prate,a.desc_,a.qtyord,a.icode,a.Ent_by ,a.App_by,b.hscode from pomas a,item b where trim(a.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='5' and a.type>'51' and trim(a.AcodE)='" + txtlbl7.Text.Trim() + "'  and a.pflag<>1 and (trim(nvl(a.App_by,'-'))<>'-' or trim(nvl(a.chk_by,'-'))<>'-') order by a.orddt desc,a.ordno desc";
                break;
            case "New":
            case "Edit":
            case "Del":
            case "SPrint":
            case "Print":
            case "Atch":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "Atch_E" || btnval == "SPrint_E")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as GE_no,to_char(a.vchdate,'dd/mm/yyyy') as GE_Dt,b.aName as Supplier,a.Invno,a.Refnum,a.prnum as GE_type,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type='00' AND A.VCHDATE " + DateRange + " order by vdd desc,a.vchnum desc";
                break;
        }
        if (typePopup == "N" && (btnval == "Edit" || btnval == "Del" | btnval == "Print" | btnval == "Atch" | btnval == "SPrint"))
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

            txtlbl8.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select substr(to_char(sysdate,'dd/mm/yyyy hh24:mi'),12,5) as timx from dual", "timx");


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
            fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr);// REMOVE TYPE FROM THE LINE BY MADHVI ON 30 JULY 2018
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {

        string orig_vchdt;
        orig_vchdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OLD_DATE");
        if (edmode.Value != "Y")
        {
            orig_vchdt = txtvchdate.Text;
        }


        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date");
            txtvchdate.Focus();
            return;
        }

        dhd = fgen.ChkDate(txtlbl3.Text.ToString());
        if (dhd == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Bill Date");
            txtlbl3.Focus();
            return;
        }

        if (Convert.ToDateTime(txtvchdate.Text) > DateTime.Now)
        {
            fgen.msg("-", "AMSG", "Entry Date Can Not be Greater then Current Date!!");
            txtvchdate.Focus();
            return;
        }

        if (Convert.ToDateTime(txtlbl3.Text) > DateTime.Now)
        {
            fgen.msg("-", "AMSG", "Bill Date Can Not be Greater then Current Date!!");
            txtvchdate.Focus();
            return;
        }

        if (Convert.ToDateTime(txtlbl3.Text) < DateTime.Now.AddDays(-120))
        {
            fgen.msg("-", "AMSG", "Bill Date Can Not be Less then 120 Days from Current Date!!");
            txtvchdate.Focus();
            return;
        }

        if (Convert.ToDateTime(txtlbl6.Text) > DateTime.Now)
        {
            fgen.msg("-", "AMSG", "Challan Date Can Not be Greater then Current Date!!");
            txtvchdate.Focus();
            return;
        }

        if (Convert.ToDateTime(txtlbl6.Text) < DateTime.Now.AddDays(-120))
        {
            fgen.msg("-", "AMSG", "Bill Date Can Not be Less then 120 Days from Current Date!!");
            txtvchdate.Focus();
            return;
        }

        string chk_alent;
        if (edmode.Value == "Y")
        {
            chk_alent = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum||'-'||to_char(vchdate,'dd/mm/yyyy') as ldt from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + DateRange + " and genum||to_char(gedate,'dd/mm/yyyy')='" + txtvchnum.Text + orig_vchdt + "' and trim(upper(acode))='" + txtlbl7.Text + "'", "ldt");
            if (chk_alent == "0")
            { }
            else
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , This Gate Entry Already Entered in MRR No." + chk_alent + ",Please Check, Edit/Save not Allowed !!");
                return;
            }
        }
        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;

        string chk_freeze = "";
        chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1021", txtvchdate.Text.Trim());
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

        if (txtlbl2.Text.Trim() == "-" && txtlbl5.Text.Trim() == "-")
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl2.Text + " / " + lbl5.Text;
        }

        if (txtlbl4.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl4.Text;
        }

        if (txtlbl7.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl7.Text;
        }

        if (txtlbl8.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl8.Text;
        }

        if (txtlbl9.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl9.Text;
        }

        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }

        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {

            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text) <= 0)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");
                return;
            }


            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text) < 0)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Rate Not Filled Correctly at Line " + (i + 1) + "  !!");
                return;
            }

            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && txtlbl4.Text == "RG" && sg1.Rows[i].Cells[16].Text.Trim().Length < 16)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , RGP Not linked Correctly at Line " + (i + 1) + "  !!");
                return;
            }


        }

        string last_entdt;
        //checks

        last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate<=to_DatE('" + orig_vchdt + "','dd/mm/yyyy') and vchdate " + DateRange + "", "ldt");
        if (last_entdt == "0")
        { }
        else
        {
            if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                return;

            }
        }
        if (txtlbl2.Text != "-")
        {
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum||'-'||to_char(vchdate,'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + " and vchnum||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + orig_vchdt + "' and trim(upper(acode))='" + txtlbl7.Text + "' and trim(upper(invno))='" + txtlbl2.Text + "'", "ldt");
            if (last_entdt == "0")
            { }
            else
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , This Invoice No. Already Entered in G.E. No. " + last_entdt + ",Please Check !!");
                return;

            }
        }
        if (txtlbl5.Text != "-")
        {
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum||'-'||to_char(vchdate,'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + " and vchnum||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + orig_vchdt + "' and trim(upper(acode))='" + txtlbl7.Text + "' and trim(upper(refnum))='" + txtlbl5.Text + "'", "ldt");
            if (last_entdt == "0")
            { }
            else
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , This Challan No. Already Entered in G.E. No. " + last_entdt + ",Please Check !!");
                return;

            }
        }


        last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
        if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
        {
            Checked_ok = "N";
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            return;

        }


        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "Y");

        switch (txtlbl4.Text)
        {
            case "PO":
            case "RG":
                checkGridQty();
                break;
        }

        string ok_for_save;
        ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
        string err_item;
        err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");
        string err_item_name;
        err_item_name = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ERR_ITEM");

        if (ok_for_save == "N")
        {
            switch (txtlbl4.Text)
            {
                case "PO":
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' Gate Entry Qty is Exceeding Order Qty , Please Check '13' " + err_item_name + "'13' " + err_item);
                    break;
                case "RG":
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' Gate Entry Qty is Exceeding RGP Qty , Please Check '13' " + err_item_name + "'13' " + err_item);
                    break;
            }
            return;
        }

        if (txtlbl4.Text.Trim() == "OT")
        {
            double totval = 0;
            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_MAX_MRR").toDouble() > 0)
            {
                for (int x = 0; x < sg1.Rows.Count; x++)
                {
                    totval += (((TextBox)sg1.Rows[x].FindControl("sg1_t2")).Text.toDouble() * ((TextBox)sg1.Rows[x].FindControl("sg1_t5")).Text.toDouble());
                }
                if (totval > fgenMV.Fn_Get_Mvar(frm_qstr, "U_MAX_MRR").toDouble())
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Value of This Gate Entry is Rs. " + totval + ", Exceeds allowed limit " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_MAX_MRR").toDouble() + ", Please Check ?");
                    return;
                }
            }
        }

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        if (frm_IndType == "05" || frm_IndType == "06")
        {
            foreach (GridViewRow row in sg1.Rows)
            {
                if (row.Cells[13].Text.Trim().Length > 5)
                {
                    if (row.Cells[13].Text.Trim().Substring(0, 2) == "02" && ((TextBox)row.FindControl("sg1_t3")).Text.ToString().toDouble() <= 0)
                    {
                        fgen.msg("-", "AMSG", "Weight not entered for Sr.No " + row.Cells[12].Text.Trim() + " ,Product : " + row.Cells[14].Text.Trim());
                        return;
                    }
                }
            }
        }

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        btnsave.Disabled = true;
    }
    //------------------------------------------------------------------------------------
    string checkGridQty()
    {
        DataTable dtQty = new DataTable();
        dtQty.Columns.Add(new DataColumn("fstr", typeof(string)));
        dtQty.Columns.Add(new DataColumn("qty", typeof(double)));
        dtQty.Columns.Add(new DataColumn("iname", typeof(string)));
        DataRow drQty = null;
        foreach (GridViewRow gr in sg1.Rows)
        {
            if (gr.Cells[13].Text.ToString().Trim().Length > 4)
            {
                drQty = dtQty.NewRow();
                drQty["fstr"] = gr.Cells[13].Text.ToString().Trim() + "-" + gr.Cells[16].Text.ToString().Trim();
                drQty["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t2")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
            }
        }
        object sm;
        string tole = "";
        DataView distQty = new DataView(dtQty, "", "fstr", DataViewRowState.CurrentRows);
        DataTable dtQty1 = new DataTable();
        dtQty1 = distQty.ToTable(true, "fstr");
        foreach (DataRow drQty1 in dtQty1.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "fstr='" + drQty1["fstr"].ToString().Trim() + "'");



            switch (txtlbl4.Text)
            {
                case "PO":
                    string xicode = "";
                    xicode = drQty1["fstr"].ToString();
                    xicode = xicode.Substring(0, 4);
                    tole = fgen.seek_iname(frm_qstr, frm_cocd, "select mqty1 from ITEM where LENGTH(tRIM(ICODE))=4 AND  SUBSTR(ICODE,1,4)='" + xicode + "' ", "mqty1");
                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select (a.Qtyord)-(a.Soldqty) as Bal_Qty from (select fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,round(Qtyord*(" + ((100 + fgen.make_double(tole)) / 100) + "),2) as qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate from pomas where branchcd='" + frm_mbr + "' and type like '5%' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate from ivoucherp where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' and trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and a.fstr='" + drQty1["fstr"].ToString().Trim() + "' order by B.Iname,trim(a.fstr)", "Bal_Qty");
                    break;
                case "RG":
                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select (a.Qtyord)-(a.Soldqty) as Bal_Qty from (select fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(type),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate as prate from rgpmst where branchcd='" + frm_mbr + "' and type in ('21','23') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate from ivoucherp where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' and trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and a.fstr='" + drQty1["fstr"].ToString().Trim() + "' order by B.Iname,trim(a.fstr)", "Bal_Qty");
                    break;
            }

            if (fgen.make_double(sm.ToString()) > fgen.make_double(col1) && fgen.make_double(col1) > 0)
            {

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", drQty1["fstr"].ToString().Trim());
                string itm_name;
                itm_name = fgen.seek_iname(frm_qstr, frm_cocd, "select iname from ITEM where SUBSTR(ICODE,1,8)='" + drQty1["fstr"].ToString().Trim().Substring(0, 8) + "' ", "iname");

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ERR_ITEM", itm_name + "  [ PO Bal " + col1 + " Entry Qty " + sm.ToString() + " ]");

                break;
            }
        }
        return null;
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
            fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr); ;// REMOVE TYPE FROM THE LINE BY MADHVI ON 30 JULY 2018
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        Server.ClearError();
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
        hffield.Value = "L1";
        fgen.msg("-", "CMSG", "Do You want to check List with Images'13'(No for without Image)");
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        // fgen.Fn_open_mseek("Select Type for Print", frm_qstr);// COMMENTED BY MADHVI ON 30 JULY 2018
        fgen.Fn_open_mseek("Select " + lblheader.Text, frm_qstr); // BY MADHVI ON 30 JULY 2018
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region

        lbl1a.Text = "00";
        vty = lbl1a.Text;
        frm_vty = lbl1a.Text;

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

        txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

        txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
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
            if (hffield.Value == "TICODE")
            {
                btnval = hffield.Value;
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

                string chk_alent;
                if (edmode.Value == "Y")
                {
                    chk_alent = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum||'-'||to_char(vchdate,'dd/mm/yyyy') as ldt from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + DateRange + " and genum||to_char(gedate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'", "ldt");
                    if (chk_alent == "0")
                    { }
                    else
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , This Gate Entry Already Entered in MRR No." + chk_alent + ",Please Check, Deletion not Allowed !!");
                        return;
                    }
                }

                // Deleing data from Main Table

                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");


                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
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
        else if (hffield.Value == "L1")
        {
            hffield.Value = "List";
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                hffield.Value = "List1";
            }
            fgen.Fn_open_Act_itm_prd("-", frm_qstr);
        }
        else if (hffield.Value == "BI")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y") hffield.Value = CP_BTN;
            else hffield.Value = CP_BTN + "_ALL";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM1", hffield.Value);
            make_qry_4_popup();
            if (hffield.Value == "SG1_ROW_ADD_ALL" || hffield.Value == "SG1_ROW_ADD") fgen.Fn_open_mseek("-", frm_qstr);
            else fgen.Fn_open_sseek("-", frm_qstr);
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

                    string chk_alent;
                    chk_alent = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum||'-'||to_char(vchdate,'dd/mm/yyyy') as ldt from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + DateRange + " and genum||to_char(gedate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'", "ldt");
                    if (chk_alent == "0")
                    { }
                    else
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , This Gate Entry Already Entered in MRR No." + chk_alent + ",Please Check, Deletion not Allowed !!");
                        return;
                    }

                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;
                case "SPrint":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "SPrint_E";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;

                case "Print":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;
                case "sg1_t2":
                    if (col1.Length > 1)
                    {
                        //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = col1; // BRANCHCD,TYPE,VCHNUM,VCHDATE OF FIRST POPUP IS SAVED IN IT.
                        // ADD BY MADHVI ON 11/05/2019
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[9].Text = col1.Substring(4, 14);
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t5")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select prate from pomas where trim(branchcd)||trim(type)||trim(ordno)||to_char(orddt,'yyyymmdd')||trim(icode)='" + col1 + "'", "prate");
                        //-----------------
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Focus();
                    }
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();

                    SQuery = "Select a.*,b.aname,c.iname,c.cpartno,c.cdrgno,c.unit AS iunit,to_Char(a.podate,'yyyymmdd')||'-'||trim(A.ponum)||'-'||trim(A.ordlineno) As link_Str,to_Char(a.rgpdate,'yyyymmdd')||'-'||trim(A.rgpnum)||'-'||trim(A.ordlineno) As link_Str2,to_char(a.invdate,'dd/mm/yyyy') as pinv_Dt,to_char(a.refdate,'dd/mm/yyyy') as pref_Dt,trim(A.ponum)||to_Char(a.podate,'yyyymmdd') as podetails,tpt_names from " + frm_tabname + " a,famst b,item c where trim(a.acode)=trim(b.acode)  and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OLD_DATE", txtvchdate.Text);

                        txtlbl2.Text = dt.Rows[i]["INVNO"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["pinv_Dt"].ToString().Trim();

                        txtlbl7.Text = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["aname"].ToString().Trim();

                        txtlbl5.Text = dt.Rows[i]["REFNUM"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["pref_Dt"].ToString().Trim();


                        txtlbl4.Text = dt.Rows[0]["PRNUM"].ToString().Trim();
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from (SELECT 'PO' as Fstr,'PO Based' as Name,'PO' as Code from dual union all SELECT 'BI' as Fstr,'Customer Return' as Name,'BI' as Code from dual union all SELECT 'OT' as Fstr,'Non PO Purchase' as Name,'OT' as Code from dual union all SELECT 'RG' as Fstr,'RGP (After Job Work)' as Name,'RG' as Code from dual union all SELECT 'JO' as Fstr,'Material (For Job Work)' as Name,'JO' as Code from dual union all SELECT 'CH' as Fstr,'Inter Unit Challan' as Name,'CH' as Code from dual) where code='" + txtlbl4.Text + "'", "name");

                        txtDriverName.Text = dt.Rows[0]["THRU"].ToString().Trim();

                        txtlbl8.Text = dt.Rows[0]["MTIME"].ToString().Trim();
                        txtlbl9.Text = dt.Rows[0]["MODE_TPT"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["NARATION"].ToString().Trim();

                        lbl1a.Text = dt.Rows[0]["type"].ToString().Trim();
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CAM1", dt.Rows[0]["TPT_NAMES"].ToString().Trim());
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
                            sg1_dr["sg1_h10"] = "";

                            sg1_dr["sg1_f1"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["iunit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["rej_sdp"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["IQTY_CHL"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["IQTY_CHLWT"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["IQTY_wT"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["IRATE"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["DESC_"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["spexc_Amt"].ToString().Trim();

                            switch (txtlbl4.Text)
                            {
                                case "PO":
                                case "BI":
                                    sg1_dr["sg1_f4"] = dt.Rows[i]["link_Str"].ToString().Trim();
                                    break;
                                case "RG":
                                    sg1_dr["sg1_f4"] = dt.Rows[i]["link_Str2"].ToString().Trim();
                                    sg1_dr["sg1_h10"] = dt.Rows[i]["podetails"].ToString().Trim();// ADD BY MADHVI ON 13/05/2019
                                    break;
                            }

                            //sg1_dr["sg1_t7"] = dt.Rows[i]["pexc"].ToString().Trim();
                            //sg1_dr["sg1_t8"] = dt.Rows[i]["pcess"].ToString().Trim();
                            //sg1_dr["sg1_t9"] = dt.Rows[i]["ptax"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                        btnlbl4.Enabled = false;
                    }
                    #endregion
                    break;
                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;
                case "SPrint_E":
                    if (col1.Length < 2) return;
                    col2 = "S1002G";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", col2);
                    fgen.fin_invn_reps(frm_qstr);
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1001");
                    fgen.fin_gate_reps(frm_qstr);
                    break;
                case "Atch_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    fgen.open_fileUploadPopup("Upload File for " + lblheader.Text, frm_qstr);
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    hffield.Value = "";
                    btnlbl7.Focus();

                    txtlbl7.Text = "";
                    txtlbl7a.Text = "";
                    {
                        create_tab();
                        sg1_add_blankrows();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        setColHeadings();
                    }

                    ViewState["sg1"] = sg1_dt;
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
                    txtlbl2.Focus();

                    if (txtlbl4.Text == "CH")
                    {
                        fillChallanItem();
                    }
                    else
                    {
                        create_tab();
                        sg1_add_blankrows();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        setColHeadings();
                    }

                    ViewState["sg1"] = sg1_dt;
                    break;
                case "SG1_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0)
                    {
                        hffield.Value = "";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", "");
                        return;
                    }
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        string pop_qry;
                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");


                        dt = new DataTable();
                        if (col1.Length > 8) SQuery = "select icode,iname as item_name,cpartno,cdrgno,unit,0 as Qtyord from item where trim(icode) in (" + col1 + ")";
                        else SQuery = "select icode,iname as item_name,cpartno,cdrgno,unit,0 as Qtyord from item where trim(icode)='" + col1 + "'";

                        switch (txtlbl4.Text)
                        {
                            case "PO":
                                if (col1.Contains("'")) SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link in (" + col1 + ") order by b.Iname ";
                                else SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link='" + col1 + "' order by b.Iname ";
                                break;
                            case "BI":
                                if (col1.Length > 25) SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link in (" + col1 + ") order by b.Iname ";
                                else SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link=" + col1 + " order by b.Iname ";
                                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_FROM1").Contains("_ALL"))
                                {
                                    if (col1.Length > 8) SQuery = "select icode,iname as item_name,'00-00-00-00' as po_link,cpartno,cdrgno,unit,0 as Qtyord,irate as prate from item where trim(icode) in (" + col1 + ")";
                                    else SQuery = "select icode,iname as item_name,'00-00-00-00' as po_link,cpartno,cdrgno,unit,0 as Qtyord,irate as prate from item where trim(icode)='" + col1 + "'";
                                }
                                break;
                            case "RG":
                                if (col1.Length > 25) SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate,'-' as btchno,'-' as btchdt from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link in (" + col1 + ") order by b.Iname ";
                                else SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate,'-' as btchno,'-' as btchdt from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link=" + col1 + " order by b.Iname ";
                                break;

                            case "CH*":
                                if (col1.Length > 25) SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate,a.btchno,a.btchdt from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link in (" + col1 + ") order by b.Iname ";
                                else SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate,a.btchno,a.btchdt from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link=" + col1 + " order by b.Iname ";
                                break;
                        }


                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

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

                            sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["item_name"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();

                            sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[d]["Qtyord"].ToString().Trim();
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = "";
                            sg1_dr["sg1_t5"] = "";
                            sg1_dr["sg1_t6"] = "";
                            sg1_dr["sg1_t7"] = "";
                            if (!dt.Rows[d]["icode"].ToString().Trim().Substring(0, 2).Equals("59"))
                            {
                                switch (txtlbl4.Text)
                                {
                                    case "PO":
                                    case "BI":
                                    case "RG":
                                        string po_linkg;
                                        string po_linkg2;
                                        po_linkg = dt.Rows[d]["po_link"].ToString().Trim();
                                        po_linkg2 = po_linkg.Split('-')[1].ToString() + "-" + po_linkg.Split('-')[2].ToString() + "-" + po_linkg.Split('-')[3].ToString();
                                        sg1_dr["sg1_f4"] = po_linkg2;
                                        sg1_dr["sg1_t5"] = dt.Rows[d]["prate"].ToString().Trim();
                                        sg1_dr["sg1_t7"] = dt.Rows[d]["Qtyord"].ToString().Trim();
                                        break;
                                    case "CH":
                                        po_linkg = dt.Rows[d]["po_link"].ToString().Trim();
                                        po_linkg2 = po_linkg.Split('-')[1].ToString() + "-" + po_linkg.Split('-')[2].ToString() + "-" + po_linkg.Split('-')[3].ToString();
                                        sg1_dr["sg1_f4"] = po_linkg2;
                                        sg1_dr["sg1_t5"] = dt.Rows[d]["prate"].ToString().Trim();
                                        sg1_dr["sg1_t7"] = dt.Rows[d]["Qtyord"].ToString().Trim();

                                        sg1_dr["sg1_t6"] = dt.Rows[d]["btchno"].ToString().Trim();
                                        break;
                                }
                            }
                            sg1_dr["sg1_t8"] = "";
                            sg1_dr["sg1_t9"] = "";
                            sg1_dr["sg1_t10"] = "";
                            sg1_dr["sg1_t11"] = "";
                            sg1_dr["sg1_t12"] = "";
                            sg1_dr["sg1_t13"] = "";
                            sg1_dr["sg1_t14"] = "";
                            sg1_dr["sg1_t15"] = "";
                            sg1_dr["sg1_t16"] = "";

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    if (sg1.Rows.Count > 0)
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    break;
                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0)
                    {
                        hffield.Value = "";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", "");
                        return;
                    }
                    col1 = "'" + col1 + "'";
                    {
                        string pop_qry;
                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");


                        dt = new DataTable();
                        if (col1.Length > 8) SQuery = "select icode,iname as item_name,cpartno,cdrgno,unit from item where trim(icode) in (" + col1 + ")";
                        else SQuery = "select icode,iname as item_name,cpartno,cdrgno,unit from item where trim(icode)='" + col1 + "'";

                        switch (txtlbl4.Text)
                        {
                            case "PO":
                                if (col1.Length > 25) SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link in (" + col1 + ") order by b.Iname ";
                                else SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link=" + col1 + " order by b.Iname ";
                                break;
                            case "BI":
                                if (col1.Length > 25) SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link in (" + col1 + ") order by b.Iname ";
                                else SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link=" + col1 + " order by b.Iname ";
                                break;
                            case "RG":
                                if (col1.Length > 25) SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link in (" + col1 + ") order by b.Iname ";
                                else SQuery = "SELECT a.po_link as FStr,b.Iname as Item_Name,a.po_link,a.Bal_qty as Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit,a.prate from (" + pop_qry + ") a,item b where trim(A.erp_Code)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2  and a.po_link=" + col1 + " order by b.Iname ";
                                break;
                        }


                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        //********* Saving in Hidden Field 
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = dt.Rows[0]["icode"].ToString().Trim(); ;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = dt.Rows[0]["item_name"].ToString().Trim();
                        //********* Saving in GridView Value
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[0]["icode"].ToString().Trim(); ;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[0]["item_name"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[0]["cpartno"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[0]["unit"].ToString().Trim();

                        //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");

                        switch (txtlbl4.Text)
                        {
                            case "PO":
                            case "BI":
                            case "RG":
                                string po_linkg;
                                string po_linkg2;
                                po_linkg = dt.Rows[0]["po_link"].ToString().Trim();
                                po_linkg2 = po_linkg.Split('-')[1].ToString() + "-" + po_linkg.Split('-')[2].ToString() + "-" + po_linkg.Split('-')[3].ToString();
                                sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = po_linkg2;
                                ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t5")).Text = dt.Rows[0]["prate"].ToString().Trim();
                                ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t7")).Text = dt.Rows[0]["Qtyord"].ToString().Trim();
                                break;
                        }
                    }

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
            }
        }
    }
    //------------------------------------------------------------------------------------
    void fillChallanItem()
    {
        if (col1.Length < 2) return;
        SQuery = "SELECT a.*,b.iname,b.cpartno,b.unit FROM IVOUCHER a,item b,TYPE C WHERE trim(a.icode)=trim(b.icode) AND TRIM(A.BRANCHCD)=TRIM(C.TYPE1) AND C.ID='B' and a.BRANCHCD!='" + frm_mbr + "' AND a.TYPE='29' AND trim(C.acode)||'-'||TO_CHAR(a.VCHDATE,'YYYYMMDD')||'-'||TRIM(a.VCHNUM)='" + col1 + "' ";
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count <= 0) return;

        txtlbl7.Text = fgen.seek_iname(frm_qstr, frm_cocd, "sELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + dt.Rows[0]["branchcd"].ToString().Trim() + "'", "ACODE");
        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "sELECT aname FROM famst WHERE trim(Acode)='" + txtlbl7.Text.Trim() + "'", "aname");

        txtlbl5.Text = dt.Rows[0]["vchnum"].ToString().Trim();
        txtlbl6.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

        create_tab();
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

            sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
            sg1_dr["sg1_f2"] = dt.Rows[d]["iname"].ToString().Trim();
            sg1_dr["sg1_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();

            sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();

            sg1_dr["sg1_t1"] = dt.Rows[d]["iqtyout"].ToString().Trim();
            sg1_dr["sg1_t2"] = dt.Rows[d]["iqtyout"].ToString().Trim();
            sg1_dr["sg1_t3"] = "";
            sg1_dr["sg1_t4"] = "";
            sg1_dr["sg1_t5"] = "";
            sg1_dr["sg1_t6"] = "";
            sg1_dr["sg1_t7"] = "";

            sg1_dr["sg1_t5"] = dt.Rows[d]["irate"].ToString().Trim();
            sg1_dr["sg1_t7"] = dt.Rows[d]["iqtyout"].ToString().Trim();

            sg1_dr["sg1_t6"] = dt.Rows[d]["btchno"].ToString().Trim() + "~" + dt.Rows[d]["btchdt"].ToString().Trim();

            sg1_dr["sg1_t8"] = "";
            sg1_dr["sg1_t9"] = "";
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
        dt.Dispose(); //sg1_dt.Dispose();
        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");


        if (hffield.Value == "List" || hffield.Value == "List1" || hffield.Value == "PENDPO" || hffield.Value == "PENDRGP" || hffield.Value == "PENDMRR")
        {
            string party_cd = "";
            string part_cd = "";
            party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
            part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
            if (party_cd.Trim().Length <= 1)
            {
                party_cd = "%";
            }
            if (part_cd.Trim().Length <= 1)
            {
                part_cd = "%";
            }

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            string headerN = "";
            string xprd1 = "";
            string xprd2 = "";
            xprd1 = " BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
            xprd2 = " BETWEEN TO_DATE('" + fromdt + "','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')-1";
            string mq1 = "";
            string mq2 = "";

            switch (hffield.Value)
            {
                case "List":
                    SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, "F20121", "branchcd='" + frm_mbr + "'", "a.type='00' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ", PrdRange);
                    headerN = lblheader.Text + " Checklist for the Period " + fromdt + " to " + todt;
                    break;
                case "List1":
                    SQuery = "SELECT distinct a.vchnum as GE_no,to_char(a.VCHDATE,'dd/mm/yyyy') as GE_dt,b.aname||'('||trim(a.acode)||')' as party,a.ent_by,a.ent_dt,a.type,a.acode,a.branchcd,a.tpt_nameS as img_src from " + frm_tabname + " A, FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODe) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='00' AND A.VCHDATE " + DateRange + "  ORDER BY A.VCHNUM DESC ";
                    headerN = lblheader.Text + " Checklist for the Period " + fromdt + " to " + todt;
                    break;
                case "PENDPO":
                    xprd1 = " BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')";

                    mq1 = "select  '-' as fstr,'-' as gstr,b.aname,c.iname as Item_Name,sum(a.poq) as PO_Qty,sum(a.rcvq) as Gate_Qty,sum(a.poq)-sum(a.rcvq) as Bal_Qty,(Case when sum(a.poq)>0 then round(((sum(a.poq)-sum(a.rcvq))/sum(a.poq))*100,2) else 0 end) as bal_per,round(sysdate-a.orddt,0) as Pend_Days,c.unit,a.branchcd,a.ordno,a.orddt,trim(a.icode) as ERP_Code,trim(a.acode) as Act_code,max(a.pflag)as pflag,max(a.del_Sch) as wo_no from (Select branchcd,pflag,ordno,orddt,acode,icode,qtyord as poq,0 as rcvq,0 as rej_Qty,del_Sch from pomas where branchcd='" + frm_mbr + "' and substr(type,1,1) ='5' and orddt " + xprd1 + " and  acode like '" + party_cd + "%' and icode like '" + part_cd + "%' union all Select branchcd,null as pflag,ponum,podate,acode,icode,0 as prq,iqty_chl as poq,0 as rej_rw,null as del_Sch from ivoucherp where branchcd='" + frm_mbr + "' and substr(type,1,1)='0' and vchdate " + xprd1 + "  ";
                    mq2 = "and  acode like '" + party_cd + "' and icode like '" + part_cd + "%'  )a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(a.icode)=trim(C.icode) group by c.iname,c.unit,b.aname,a.branchcd,a.ordno,a.orddt,trim(a.AcodE),trim(a.icode) having sum(a.poq)-sum(a.rcvq)>0 and max(a.pflag)!=1 order by b.aname,a.orddt,a.ordno";

                    SQuery = mq1 + mq2;
                    fgen.drillQuery(0, SQuery, frm_qstr, "5#6#7#8#", "3#4#5#6#7#8#", "350#250#100#100#100#100#");
                    headerN = "Pending Purchase Orders at GATE (" + fromdt + " to " + todt + ")";
                    break;
                case "PENDRGP":
                    xprd1 = " BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')";

                    mq1 = "select  '-' as fstr,'-' as gstr,b.aname,c.iname as Item_Name,sum(a.poq) as RGP_Qty,sum(a.rcvq) as Gate_Qty,sum(a.poq)-sum(a.rcvq) as Bal_Qty,(Case when sum(a.poq)>0 then round(((sum(a.poq)-sum(a.rcvq))/sum(a.poq))*100,2) else 0 end) as bal_per,round(sysdate-a.vchdate,0) as Pend_Days,c.unit,a.branchcd,a.vchnum,a.vchdate,trim(a.icode) as ERP_Code,trim(a.acode) as Act_code from (Select branchcd,vchnum,vchdate,acode,icode,iqtyout as poq,0 as rcvq from ivoucher where branchcd='" + frm_mbr + "' and (substr(type,1,2) ='21' or substr(type,1,2) ='23') and vchdate " + xprd1 + " and  acode like '" + party_cd + "%' and icode like '" + part_cd + "%' union all Select branchcd,rgpnum,rgpdate,acode,icode,0 as prq,iqty_chl as poq from ivoucherp where branchcd='" + frm_mbr + "' and substr(type,1,1)='0' and vchdate " + xprd1 + " and PRNUM='RG' ";
                    mq2 = "and  acode like '" + party_cd + "' and icode like '" + part_cd + "%'  )a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(a.icode)=trim(C.icode) group by c.iname,c.unit,b.aname,a.branchcd,a.vchnum,a.vchdate,trim(a.AcodE),trim(a.icode) having sum(a.poq)-sum(a.rcvq)>0 order by b.aname,a.vchdate,a.vchnum";

                    SQuery = mq1 + mq2;
                    fgen.drillQuery(0, SQuery, frm_qstr, "5#6#7#8#", "3#4#5#6#7#8#", "350#250#100#100#100#100#");
                    headerN = "Pending Returnable Material at GATE (" + fromdt + " to " + todt + ")";
                    break;

                case "PENDMRR":
                    xprd1 = " BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')";

                    mq1 = "select  '-' as fstr,'-' as gstr,b.aname,c.iname as Item_Name,sum(a.poq) as Gate_Qty,sum(a.rcvq) as MRR_Qty,sum(a.poq)-sum(a.rcvq) as Bal_Qty,(Case when sum(a.poq)>0 then round(((sum(a.poq)-sum(a.rcvq))/sum(a.poq))*100,2) else 0 end) as bal_per,round(sysdate-a.vchdate,0) as Pend_Days,c.unit,a.branchcd,a.vchnum,a.vchdate,trim(a.icode) as ERP_Code,trim(a.acode) as Act_code from (Select branchcd,vchnum,vchdate,acode,icode,iqty_chl as poq,0 as rcvq from ivoucherp where branchcd='" + frm_mbr + "' and substr(type,1,1) ='0' and vchdate " + xprd1 + " and  acode like '" + party_cd + "%' and icode like '" + part_cd + "%' union all Select branchcd,genum,gedate,acode,icode,0 as prq,iqty_chl as poq from ivoucher where branchcd='" + frm_mbr + "' and substr(type,1,1)='0' and vchdate " + xprd1 + " and (store='Y' or store='N') ";
                    mq2 = "and  acode like '" + party_cd + "' and icode like '" + part_cd + "%'  )a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(a.icode)=trim(C.icode) group by c.iname,c.unit,b.aname,a.branchcd,a.vchnum,a.vchdate,trim(a.AcodE),trim(a.icode) having sum(a.poq)-sum(a.rcvq)>0 order by b.aname,a.vchdate,a.vchnum";

                    SQuery = mq1 + mq2;
                    fgen.drillQuery(0, SQuery, frm_qstr, "5#6#7#8#", "3#4#5#6#7#8#", "350#250#100#100#100#100#");
                    headerN = "Gate Entry Pending for MRR at Store (" + fromdt + " to " + todt + ")";
                    break;
            }
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            if (hffield.Value == "List1") fgen.Fn_open_rptlevelIMG(lblheader.Text + " Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
            else fgen.Fn_open_rptlevel(headerN, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------


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



                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();
                        //save_fun2();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

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
                                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2)
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
                        //save_fun2();

                        if (edmode.Value == "Y")
                        {


                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);


                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);


                        if (edmode.Value == "Y")
                        {
                            //fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            fgen.msg("-", "CMSG", lblheader.Text + " " + frm_vnum + " Updated Successfully'13'Do you want to see the Print Preview ?");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                                //fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
                                fgen.msg("-", "CMSG", lblheader.Text + " " + frm_vnum + " Saved Successfully'13'Do you want to see the Print Preview ?");
                                //updateMessage();
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }


                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtvchdate.Text.Trim() + "'");
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
            //for (int sg1r = 0; sg1r < sg1.Rows.Count; sg1r++)
            //{
            //    for (int j = 0; j < sg1.Columns.Count; j++)
            //    {
            //        sg1.Rows[sg1r].Cells[j].ToolTip = sg1.Rows[sg1r].Cells[j].Text;
            //        if (sg1.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
            //        {
            //            sg1.Rows[sg1r].Cells[j].Text = sg1.Rows[sg1r].Cells[j].Text.Substring(0, 35);
            //        }
            //    }
            //}
            switch (txtlbl4.Text.Trim().ToUpper())
            {
                case "PO":
                    ((TextBox)e.Row.FindControl("sg1_t1")).ReadOnly = true;
                    break;
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
                //if (txtlbl7.Text.Trim().Length < 2)
                //{
                //    fgen.msg("-", "AMSG", "Please Choose Supplier");
                //    return;
                //}
                if (txtlbl4.Text.Trim().ToUpper() == "CH") return;
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_ADD_E";
                }
                else hffield.Value = "SG1_ROW_ADD";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

                if (txtlbl4.Text.Trim().ToUpper() == "BI")
                {
                    hffield.Value = "BI";
                    fgen.msg("-", "CMSG", "Do You want to Show Billed Data ?(No for All Data)");
                }
                else
                {
                    make_qry_4_popup();
                    if (hffield.Value == "SG1_ROW_ADD_E") fgen.Fn_open_sseek("Select Item", frm_qstr);
                    fgen.Fn_open_mseek("Select Item", frm_qstr);
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
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select G.E.Type ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl10_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl11_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl12_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {

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
        fgen.Fn_open_sseek("Select Party ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Length > 2)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy");
                oporow["rec_iss"] = "W";
                oporow["INVNO"] = txtlbl2.Text.Replace(" ", "");
                oporow["INVDATE"] = txtlbl3.Text;
                oporow["REFNUM"] = txtlbl5.Text.Replace(" ", "");
                oporow["REFDATE"] = txtlbl6.Text;
                oporow["PRNUM"] = txtlbl4.Text.Replace(" ", "");
                oporow["acode"] = txtlbl7.Text;
                oporow["MTIME"] = txtlbl8.Text;
                oporow["mode_tpt"] = txtlbl9.Text;
                oporow["SRNO"] = i.ToString();
                oporow["morder"] = i;
                oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim();
                oporow["rej_sdp"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
                oporow["IQTY_CHL"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text);
                oporow["iqty_chlwt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                oporow["iqty_WT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text);
                oporow["IRATE"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                oporow["DESC_"] = (((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text);
                oporow["spexc_Amt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                oporow["iqtyin"] = 0;
                oporow["iweight"] = 0;
                oporow["iqtyout"] = 0;
                oporow["iamount"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text) * fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                oporow["segment_"] = 0;
                oporow["tc_no"] = "-";
                oporow["pname"] = "-";
                oporow["purpose"] = "-";
                oporow["store"] = "N";
                oporow["thru"] = txtDriverName.Text;
                oporow["finvno"] = "-";
                oporow["PONUM"] = "-";
                oporow["PODATE"] = txtvchdate.Text.Trim();
                oporow["RGPNUM"] = "-";
                oporow["RGPdate"] = txtvchdate.Text.Trim();

                if (txtlbl4.Text == "CH")
                {
                    if ((((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text).Contains("~"))
                    {
                        oporow["btchno"] = (((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text).Split('~')[0].ToString().Trim();
                        oporow["btchdt"] = (((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text).Split('~')[1].ToString().Trim();
                    }
                }
                if (!sg1.Rows[i].Cells[13].Text.Trim().Substring(0, 2).Equals("59"))
                {
                    string mpr_dtl;
                    switch (txtlbl4.Text)
                    {
                        case "PO":
                        case "BI":
                            if (sg1.Rows[i].Cells[16].Text.Trim().Length >= 16)
                            {
                                try
                                {
                                    mpr_dtl = sg1.Rows[i].Cells[16].Text.Trim().Substring(9, 6);
                                    oporow["ponum"] = mpr_dtl;
                                    mpr_dtl = sg1.Rows[i].Cells[16].Text.Trim().Substring(6, 2) + "/" + sg1.Rows[i].Cells[16].Text.Trim().Substring(4, 2) + "/" + sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 4);
                                    oporow["podate"] = fgen.make_def_Date(mpr_dtl, vardate);
                                    if (sg1.Rows[i].Cells[16].Text.Trim().Length == 19)
                                    {
                                        mpr_dtl = sg1.Rows[i].Cells[16].Text.Trim().Substring(16, 3);
                                        oporow["ordlineno"] = mpr_dtl;
                                    }
                                    else
                                    {
                                        mpr_dtl = sg1.Rows[i].Cells[16].Text.Trim().Substring(16, 4);
                                        oporow["ordlineno"] = mpr_dtl;
                                    }
                                }
                                catch
                                {

                                }
                            }
                            break;
                        case "RG":
                            if (sg1.Rows[i].Cells[16].Text.Trim().Length >= 16)
                            {
                                mpr_dtl = sg1.Rows[i].Cells[16].Text.Trim().Substring(9, 6);
                                oporow["rgpnum"] = mpr_dtl;
                                mpr_dtl = sg1.Rows[i].Cells[16].Text.Trim().Substring(16, 4);
                                oporow["ordlineno"] = mpr_dtl;
                                mpr_dtl = sg1.Rows[i].Cells[16].Text.Trim().Substring(6, 2) + "/" + sg1.Rows[i].Cells[16].Text.Trim().Substring(4, 2) + "/" + sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 4);
                                oporow["rgpdate"] = fgen.make_def_Date(mpr_dtl, vardate);
                            }
                            if (sg1.Rows[i].Cells[9].Text.Trim().Length > 10)
                            {
                                // ADD BY MADHVI ON 13/05/2019
                                mpr_dtl = sg1.Rows[i].Cells[9].Text.Trim().Substring(0, 6);
                                oporow["ponum"] = mpr_dtl;
                                mpr_dtl = sg1.Rows[i].Cells[9].Text.Trim().Substring(12, 2) + "/" + sg1.Rows[i].Cells[9].Text.Trim().Substring(10, 2) + "/" + sg1.Rows[i].Cells[9].Text.Trim().Substring(6, 4);
                                oporow["podate"] = fgen.make_def_Date(mpr_dtl, vardate);
                            }
                            break;
                    }
                }
                switch (txtlbl4.Text)
                {
                    case "PO":
                        oporow["cess_percent"] = 1;
                        break;
                    case "BI":
                        oporow["cess_percent"] = 2;
                        break;
                    case "OT":
                        oporow["cess_percent"] = 3;
                        break;
                    case "RG":
                        oporow["cess_percent"] = 4;
                        break;
                    case "JO":
                        oporow["cess_percent"] = 5;
                        break;
                    case "CH":
                        oporow["cess_percent"] = 6;
                        break;
                }


                oporow["naration"] = txtrmk.Text.Trim();

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
                // BY MADHVI ON 30 JULY 2018  ---------------
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_CAM1").Trim().Length > 1)
                {
                    string cam_img = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CAM1").Trim().Substring(fgenMV.Fn_Get_Mvar(frm_qstr, "U_CAM1").Trim().ToUpper().IndexOf("UPLOAD"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_CAM1").Trim().Length - fgenMV.Fn_Get_Mvar(frm_qstr, "U_CAM1").Trim().ToUpper().IndexOf("UPLOAD"));
                    oporow["tpt_nameS"] = cam_img;
                }
                else
                {
                    oporow["tpt_nameS"] = "-";
                }
                oporow["doc_tot"] = 0;
                oporow["gst_pos"] = "-";
                oporow["she_cess"] = 0;
                //oporow["mr_rdate"] = "-";
                //oporow["mr_gdate"] = "-";
                oporow["potype"] = "-";
                oporow["mattype"] = "L";
                oporow["ccent"] = "-";
                oporow["exc_57f4"] = "-";
                oporow["o_deptt"] = "-";
                //txtDriverName
                // -----------------------------------------
                oDS.Tables[0].Rows.Add(oporow);

            }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {

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

        SQuery = "SELECT '00' AS FSTR,'Gate Inward Entry' as NAME,'00' AS CODE FROM dual";

    }
    protected void btnSticker_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SPrint";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select " + lblheader.Text + " for TAG Printing", frm_qstr); // BY MADHVI ON 28 JULY 2018
    }
    //------------------------------------------------------------------------------------
    public void updateMessage()
    {
        string str;
        string v = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(sno) AS VCH FROM message", 1, "VCH");

        //str = "insert into message(sno,message,flag,PHNO)values('" + v + "','you have open a new branch','N','" + txt_phone_no.Value + "')";
        // we can put dynamic phone number here ,as for now am taking phone number from branch master.
        // but right now in tele field there is no mobile number all are telephones numbers.

        str = "insert into message(sno,message,flag,PHNO)values('" + v + "',' Dear " + frm_uname + " Material received GE No. " + txtvchnum.Text + " from " + txtlbl7a.Text + " vide Invoice No." + txtlbl2.Text + "','N','919311278885')";

        fgen.execute_cmd(frm_qstr, frm_cocd, str);

    }
    //------------------------------------------------------------------------------------   
    protected void btnGridPop_Click(object sender, EventArgs e)
    {
        if (hf1.Value.Contains("sg1_t2_"))
        {
            hffield.Value = "sg1_t2";
            hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_sg1_t2_", "");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            make_qry_4_popup();
            fgen.Fn_open_sseek("Please Link P.O. No.", frm_qstr);
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnCamera_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        hffield.Value = "";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL10", frm_mbr + frm_vty + txtvchnum.Text + Convert.ToDateTime(txtvchdate.Text).ToString("dd_MM_yyyy"));
        fgen.open_sseek_camera("", frm_qstr);
    }
    //------------------------------------------------------------------------------------    
    protected void btnAtch_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Atch_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr);
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        hffield.Value = "PENDPO";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMREPID", frm_formID + "_1");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        hffield.Value = "PENDRGP";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMREPID", frm_formID + "_2");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        hffield.Value = "PENDMRR";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMREPID", frm_formID + "_3");

        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void btnConvsheetToWt_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow sgr in sg1.Rows)
        {
            if (sgr.Cells[13].Text.Trim().Length > 5)
            {
                if (sgr.Cells[13].Text.Trim().Substring(0, 2) == "02")
                {
                    if (((TextBox)sgr.FindControl("sg1_t3")).Text.ToString().toDouble() <= 0)
                    {
                        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select iweight from item where trim(icode)='" + sgr.Cells[13].Text.Trim() + "'", "IWEIGHT");
                        ((TextBox)sgr.FindControl("sg1_t3")).Text = (((TextBox)sgr.FindControl("sg1_t2")).Text.toDouble() * col1.toDouble()).toDouble(3).ToString();
                    }
                }
            }
        }
    }
}