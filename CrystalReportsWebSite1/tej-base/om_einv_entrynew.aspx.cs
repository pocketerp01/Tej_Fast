using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Net.Mail;


public partial class om_einv_entrynew : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS, sdOds; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok, xhtml_tag;
    string save_it;
    string pop_qry = "";
    string newBranchcd = "", brCode = "";
    bool sdWorking = false;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tab_ivch, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, cond = "";
    string frm_tab_sale;
    string frm_tab_vchr, frm_tab_hundi;
    //double double_val2, double_val1;
    fgenDB fgen = new fgenDB();
    ReportDocument repDoc;

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
            hfcocd.Value = frm_cocd;
            if (!Page.IsPostBack)
            {

                string chk_opt = "";
                //batch_Stock
                doc_addl.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_pw where branchcd='" + frm_mbr + "' and OPT_ID='W2019'", "fstr");

                doc_hoso.Value = "N";
                //order from ho only
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0052'", "fstr");
                if (chk_opt == "Y")
                {
                    doc_hoso.Value = "Y";
                }

                doc_GST.Value = "Y";
                //GSt india
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2017'", "fstr");
                if (chk_opt == "N")
                {
                    doc_GST.Value = "N";

                    chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2027'", "fstr");
                    if (chk_opt == "Y")
                    //Member GCC Country
                    {
                        doc_GST.Value = "GCC";
                    }
                }

                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select enable_yn as fstr from stock where id='M139'", "fstr");
                if (chk_opt == "Y")
                {
                    brPrefixWithInvNo.Value = "Y";
                }

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
            //fgen.Fn_open_prddmp1("-", frm_qstr);
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

                ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");

                txtCashDiscValue.Attributes.Add("readonly", "readonly");
                txtlbl70.Attributes.Add("readonly", "readonly");
                txtlbl71.Attributes.Add("readonly", "readonly");
                txtlbl72.Attributes.Add("readonly", "readonly");
                txtlbl73.Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t7")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t8")).Attributes.Add("readonly", "readonly");
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
                    //sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
                    sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
                    sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                }
            }
        }

        txtlbl25.Attributes.Add("readonly", "readonly");
        txtlbl27.Attributes.Add("readonly", "readonly");
        txtlbl29.Attributes.Add("readonly", "readonly");
        txtlbl31.Attributes.Add("readonly", "readonly");

        // to hide and show to tab panel



        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F50101":
            case "F50116":
            case "F55101":
                tab2.Visible = false;
                tab3.Visible = false;
                tab4.Visible = false;
                //tab6.Visible = false;
                multDiv.Style.Add("display", "none");
                break;
            case "F55106":
                tab2.Visible = false;
                tab3.Visible = false;
                tab4.Visible = false;
                multDiv.Style.Add("display", "none");
                chkFOC.Visible = false;

                multDiv.Style.Add("display", "none");

                break;

        }

        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
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
        switch (Prg_Id)
        {
            case "F50101":
            case "F55101":
                frm_tab_ivch = "IVOUCHER";
                frm_tab_sale = "SALE";
                frm_tab_vchr = "VOUCHER";
                frm_tab_hundi = "HUNDI";

                if (frm_cocd == "SAIA") sdWorking = false;
                if (brCode == "")
                {
                    newBranchcd = frm_mbr;
                    if (frm_mbr.toDouble() < 50)
                    {
                        newBranchcd = (fgen.make_double(frm_mbr) + 40).ToString();
                        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT LINK_BR FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "' ", "LINK_BR");
                        if (col1 != "0") newBranchcd = col1;
                    }
                    brCode = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + newBranchcd + "'", "ACODE");
                }
                break;
            case "F55106":
                frm_tab_ivch = "IVOUCHERP";
                frm_tab_sale = "SALEP";
                frm_tab_vchr = "VOUCHERP";
                frm_tab_hundi = "HUNDIP";
                break;

        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TAB_IVCH", frm_tab_ivch);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TAB_SALE", frm_tab_sale);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TAB_VCHR", frm_tab_vchr);

        //cond = " TYPE='" + frm_vty + "'";
        //if (frm_cocd == "AGRM" || frm_cocd == "KESR") cond = " TYPE like '4%'";
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {

        SQuery = "";
        string ord_br_Str = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tab_ivch = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_IVCH");
        frm_tab_sale = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_SALE");
        frm_tab_vchr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_VCHR");
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
                SQuery = "Select Type1 as fstr,Name,Type1 as Code,Addr1 as Owner,vchnum as Veh_type from type where id='G' and substr(type1,1,1)='2'  order by name,addr1";
                break;
            case "BTN_16":
                SQuery = "select * from (select Acode,ANAME as Transporter,Acode as Code,Addr1 as Address,Addr2 as City from famst  where upper(ccode)='T' union all select 'Own' as Acode,'OWN' as Transporter,'-' as Code,'-' as Address,'-' as City from dual union all select 'party' as acode,'PARTY VEHICLE' as Transporter,'-' as Code,'-' as Address,'-' as City from dual) order by  Transporter";
                break;
            case "BTN_17":
                SQuery = "Select Type1 as fstr,Name,type1 from type where id='>' order by name";
                break;
            case "BTN_18":
                SQuery = "Select Type1 as fstr,Name,type1 from type where id='<' order by name";
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
                ord_br_Str = "a.branchcd='" + frm_mbr + "'";
                if (doc_hoso.Value == "Y")
                {
                    ord_br_Str = "a.branchcd='00'";
                }

                SQuery = "SELECT distinct a.ACODE AS FSTR,b.ANAME AS PARTY,a.ACODE AS CODE,b.ADDR1,b.ADDR2,b.staten as state,b.Pay_num,b.GST_no FROM somas a, FAMST b where " + ord_br_Str + " and a.type='" + frm_vty + "' and trim(nvl(a.ICAT,'-'))!='Y'  and trim(nvl(a.app_by,'-'))!='-' and trim(A.acode)=trim(B.acode) and length(Trim(nvl(b.deac_by,'-')))<=1 ORDER BY aname ";
                if (Prg_Id == "F55106")
                {
                    SQuery = "SELECT distinct a.ACODE AS FSTR,A.ANAME AS PARTY,a.ACODE AS CODE,A.ADDR1,A.ADDR2,A.staten as state,A.Pay_num,A.GST_no FROM FAMST A WHERE SUBSTR(ACODE,1,2)='16' ORDER BY A.ACODE,A.aname ";
                }
                break;
            case "PICK_DA":
                SQuery = "SELECT DISTINCT A.packno||to_char(A.packdate,'dd/mm/yyyy')||trim(a.Acode) as Fstr,A.packno AS DA_Number,to_Char(a.packdate,'dd/mm/yyyy') as DA_Date,B.Aname as Customer,a.Pordno,to_Char(a.packdate,'yyyymmdd') as VDD  FROM despatch a, famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.BRANCHCD='" + frm_mbr + "' and a.TYPE='" + frm_vty + "' and trim(a.acode) ='" + txtlbl4.Text + "' AND (a.packno||to_char(a.packdate,'yyyymm')) IN (SELECT VCHNUM FROM (SELECT X.VCHNUM,SUM(X.aBC) AS CNT FROM (select a.packno||to_char(a.packdate,'yyyymm') as vchnum,a.type,Qtysupp AS ABC from despatch a  where branchcd='" + frm_mbr + "' AND a.type='" + frm_vty + "' and trim(a.Acode)='" + txtlbl4.Text + "' UNION ALL select nvl(a.tc_no,'-')||to_char(a.refdate,'yyyymm') as genum,a.type,a.iqtyout*-1 AS ABC from ivoucher a where branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and trim(a.Acode)='" + txtlbl4.Text + "' ) X GROUP BY X.VCHNUM) WHERE CNT>0) order by vdd";
                break;

            case "TICODE":
                //pop2
                SQuery = "SELECT ACODE AS FSTR,ANAME AS PARTY,ACODE AS CODE,ADDR1,ADDR2 FROM CSMST where branchcd!='DD' ORDER BY aname ";
                break;
            case "TICODEX":
                SQuery = "select type1,name as State ,type1 as code from type where id='{' order by Name";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
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
                ord_br_Str = "branchcd='" + frm_mbr + "'";
                if (doc_hoso.Value == "Y")
                {
                    ord_br_Str = "branchcd='00'";
                }

                if (col1.Length <= 0) col1 = "'-'";
                SQuery = "select a.Fstr,max(a.Iname)as Item_Name,a.ERP_code,max(a.Cpartno)as Part_no,max(a.Irate) As Irate,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as PO_No,a.Fstr as SO_link,max(a.cdisc) as CDisc,max(a.iexc_Addl) as iexc_Addl,max(a.sd) as frt_pu,max(a.ipack) as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty,olineno from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,pordno,(Case when length(trim(nvl(desc9,'-')))>1 then desc9 else ciname end) as Iname,trim(Icode) as ERP_code,Cpartno,Irate,Qtyord,0 as Soldqty,nvl(Cdisc,0) as Cdisc,nvl(iexc_addl,0) as Iexc_Addl,nvl(sd,0) as sd ,nvl(ipack,0) as ipack,trim(nvl(cdrgno,'-')) as olineno from somas where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "' and trim(icat)!='Y' and trim(app_by)!='-'  union all SELECT to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(Icode) as fstr,null as pordno,null as Iname,trim(Icode) as ERP_code,null as Cpartno,0 as Irate,0 as Qtyord,iqtyout as Soldqty,0 as Cdisc,0 as iexc_Addl,0 as sd,0 as ipack,nvl(revis_no,'-') AS linno  from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "')a,item b where trim(a.erp_code)=trim(B.icode)  group by a.olineno,a.fstr,a.ERP_code,b.unit,b.hscode having (case when sum(a.Qtyord)>0 then sum(a.Qtyord)-sum(a.Soldqty) else max(a.irate) end)>0 order by Item_Name,a.fstr";

                SQuery = "select a.Fstr,max(b.Iname)as Item_Name,a.ERP_code,(case when length(trim(max(a.Cpartno)))>2 then max(a.Cpartno) else b.cpartno end) as Part_no,max(a.Irate) As Irate,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as PO_No,a.Fstr as SO_link,max(a.cdisc) as CDisc,max(a.iexc_Addl) as iexc_Addl,max(a.sd) as frt_pu,max(a.ipack) as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty,olineno,b.packsize as std_pack from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,pordno,(Case when length(trim(nvl(desc9,'-')))>1 then desc9 else ciname end) as Iname,trim(Icode) as ERP_code,Cpartno,Irate,Qtyord,0 as Soldqty,nvl(Cdisc,0) as Cdisc,nvl(iexc_addl,0) as Iexc_Addl,nvl(sd,0) as sd ,nvl(ipack,0) as ipack,trim(nvl(cdrgno,'-')) as olineno from somas where " + ord_br_Str + " and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "' and trim(icat)!='Y' and trim(app_by)!='-'  union all SELECT to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(Icode) as fstr,null as pordno,null as Iname,trim(Icode) as ERP_code,null as Cpartno,0 as Irate,0 as Qtyord,iqtyout as Soldqty,0 as Cdisc,0 as iexc_Addl,0 as sd,0 as ipack,nvl(revis_no,'-') AS linno  from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "')a,item b where trim(a.erp_code)=trim(B.icode)  group by a.olineno,a.fstr,a.ERP_code,b.unit,b.hscode,b.cpartno,b.packsize having (case when sum(a.Qtyord)>0 then sum(a.Qtyord)-sum(a.Soldqty) else max(a.irate) end)>0 order by Item_Name,a.fstr";
                if (Prg_Id == "F55106")
                {
                    cond = "AND (SUBSTR(A.ICODE,1,1)='9' OR SUBSTR(A.ICODE,1,2)='59')";
                    if (frm_vty == "4B") cond = "";
                    if (frm_cocd == "MULT") cond = "";
                    SQuery = "select a.ICODE AS FSTR,A.INAME as Item_Name,a.ICODE AS ERP_code,A.CPARTNO as Part_no,A.IRATE As Irate,0 as Balance_Qty,A.Unit,A.hscode,'-' as PO_No,'-' as SO_link,0 as CDisc,0 as iexc_Addl,0 as frt_pu,0 as pkchg_pu,0 as Qty_Ord,0 as Sold_Qty,A.packsize as std_pack,0 AS olineno from ITEM A WHERE LENGTH(TRIM(ICODE))>4 " + cond + " ORDER BY A.ICODE desc,A.INAME";
                }
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);
                break;
            case "SG1_ROW_TAX":

                SQuery = "Select Type1 as fstr,Name,Type1 as Code,nvl(Rate,0) as Rate,nvl(Excrate,0) as Schg,exc_Addr as Ref_Code from type where id='S' and length(Trim(nvl(cstno,'-')))<=1 order by name";
                break;
            case "SG1_ROW_DT":
                col1 = "";
                int cnt = 0;
                foreach (GridViewRow gr in sg1.Rows)
                {

                    if (gr.Cells[13].Text.Trim().Length > 1 && ((TextBox)sg1.Rows[cnt].FindControl("sg1_t2")).Text.Trim().Length > 1)
                    {
                        if (col1.Length > 0)
                        {
                            col1 = col1 + ",'" + gr.Cells[13].Text.Trim() + ((TextBox)sg1.Rows[cnt].FindControl("sg1_t2")).Text.Trim() + "'";
                        }
                        else
                        {
                            col1 = "'" + gr.Cells[13].Text.Trim() + ((TextBox)sg1.Rows[cnt].FindControl("sg1_t2")).Text.Trim() + "'";
                        }
                    }
                    cnt = cnt + 1;
                }
                if (col1.Length <= 0) col1 = "'-'";
                string row_erpcd = "";
                row_erpcd = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text.Trim();
                SQuery = "select trim(upper(a.batch_no)) as Fstr,trim(upper(a.batch_no)) as Batch_no,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,sum(a.Qtyord) as Prodn,sum(a.Soldqty) as Sales from (SELECT trim(icode)||trim(btchno) as fstr,trim(btchno) as Batch_no,iqtyin as qtyord,0 as Soldqty from ivoucher where branchcd='" + frm_mbr + "' and type in ('15','16','17') and trim(store)='Y' and trim(icode)='" + row_erpcd.Trim() + "' union all SELECT trim(icode)||trim(btchno) as fstr,trim(btchno) as Batch_no,0 as qtyord,iqtyout as Soldqty from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and trim(store)='Y' and trim(icode)='" + row_erpcd.Trim() + "')a where trim(fstr) not in (" + col1 + ") group by trim(fstr),trim(upper(a.batch_no))  having  sum(a.Qtyord)-sum(a.Soldqty) >0 order by trim(upper(a.batch_no))";

                break;
            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Doc_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Doc_Dt,b.Aname as Customer,b.Gst_No,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tab_ivch + " a,famst b where  a.branchcd='" + (sdWorking ? newBranchcd : frm_mbr) + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " and  trim(a.acode)=trim(B.acodE) order by vdd desc,a." + doc_nf.Value + " desc";
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        if (checkControlsLinkedCorrectly())
        {
            chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
            clearctrl();
            if (chk_rights == "Y")
            {
                // if want to ask popup at the time of new
                txtlbl2.Text = DateTime.Now.ToString("HH:mm").ToString();
                string mr_time = "";
                mr_time = fgen.seek_iname(frm_qstr, frm_cocd, "select substr(to_char(sysdate + interval '30' minute,'dd/mm/yyyy hh24:mi'),12,5) as timx from dual", "timx");
                txtlbl30.Text = mr_time;

                hffield.Value = "New";
                make_qry_4_popup();
                fgen.Fn_open_sseek("select type", frm_qstr);

                // else comment upper code

                //frm_vnum = fgen.next_no(frm_vnum, "SELECT MAX(vCHNUM) AS VCH FROM " + frm_tab_ivch + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + DateRange + " ", 6, "VCH");
                //txtvchnum.Text = frm_vnum;
                //txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                //disablectrl();
                //fgen.EnableForm(this.Controls);
            }
            else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
        }

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
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }

        string chk_freeze = "";
        chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1064", txtvchdate.Text.Trim());
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
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date");
            txtvchdate.Focus();
            return;
        }
        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        {
            fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only");
            txtvchdate.Focus();
            return;
        }



        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;

        if (txtlbl4.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl4.Text;
        }

        //if (txtlbl5.Text.Trim().Length < 2)
        //{
        //    reqd_nc = reqd_nc + 1;
        //    reqd_flds = reqd_flds + " / " + lbl5.Text;

        //}
        //if (txtlbl6.Text.Trim().Length < 2)
        //{
        //    reqd_nc = reqd_nc + 1;
        //    reqd_flds = reqd_flds + " / " + lbl6.Text;

        //}
        //if (txtlbl8.Text.Trim().Length < 2)
        //{
        //    reqd_nc = reqd_nc + 1;
        //    reqd_flds = reqd_flds + " / " + lbl8.Text;

        //}

        //if (txtlbl9.Text.Trim().Length < 2)
        //{
        //    reqd_nc = reqd_nc + 1;
        //    reqd_flds = reqd_flds + " / " + lbl9.Text;

        //}

        if (txtlbl24.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl24.Text;

        } if (frm_formID != "F55106")
        {
            if (txtlbl27.Text.Trim().Length < 2)
            {
                reqd_nc = reqd_nc + 1;
                reqd_flds = reqd_flds + " / " + lbl27.Text;

            }
            if (txtlbl15.Text.Trim().Length < 2)
            {
                reqd_nc = reqd_nc + 1;
                reqd_flds = reqd_flds + " / " + lbl15.Text;

            }
            if (txtlbl16.Text.Trim().Length < 2)
            {
                reqd_nc = reqd_nc + 1;
                reqd_flds = reqd_flds + " / " + lbl16.Text;

            }
            if (txtlbl17.Text.Trim().Length < 2)
            {
                reqd_nc = reqd_nc + 1;
                reqd_flds = reqd_flds + " / " + lbl17.Text;

            }
        }


        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }



        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) < 0)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");
                i = sg1.Rows.Count;
                return;

            }
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text) <= 0)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Rate Not Filled Correctly at Line " + (i + 1) + "  !!");
                i = sg1.Rows.Count;
                return;

            }


        }

        string last_entdt;
        //checks
        if (edmode.Value == "Y")
        {
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tab_sale + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  and vchdate " + DateRange + " and vchnum||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "' and vchdate<=to_DaTE('" + txtvchdate.Text + "','dd/mm/yyyy') order by vchdate desc", "ldt");
        }
        else
        {
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tab_sale + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  and vchdate " + DateRange + " and vchnum||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "' order by vchdate desc", "ldt");
        }

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
        last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
        if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt) && edmode.Value == "N")
        {
            Checked_ok = "N";
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            return;

        }





        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "Y");
        checkGridQty();

        string ok_for_save;
        ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
        string err_item;
        err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");
        string err_item_name;
        err_item_name = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ERR_ITEM");

        if (ok_for_save == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' Invoice Qty is Exceeding Order Qty , Please Check '13' " + err_item_name + "'13' " + err_item);
            return;
        }

        //**************** Stock Check
        if (Prg_Id == "F55106")
        {

        }
        else
        {
            checkStockQty();
        }

        ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
        err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");

        if (ok_for_save == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' Cannot Despatch more the Stock Qty , Please Check item : " + err_item);
            return;
        }

        //**************** Batch Stock Qty check 
        if (doc_addl.Value == "Y")
        {
            if (Prg_Id == "F55106")
            {

            }
            else
            {
                check_btch_StockQty();

            }

            ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
            err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");

            if (ok_for_save == "N")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' Cannot Despatch more the Batch Stock Qty , Please Check item : " + err_item);
                return;
            }
        }


        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }
        if (fgen.make_double(txtlbl31.Text) <= 0)
        {
            fgen.msg("-", "AMSG", "Total Amount Can Not be Zero or Less then Zero!!");
            return;
        }

        if (hfCalcGST.Value == "" || hfCalcGST.Value == null)
        {
            hffield.Value = "DUTY";
            fgen.msg("-", "CMSG", "Do You Want to Calculate Taxes'13'(No for save without Taxes)");
            return;
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
                drQty["fstr"] = gr.Cells[13].Text.ToString().Trim() + "-" + ((TextBox)gr.FindControl("sg1_t14")).Text.ToString().Trim() + "-" + ((TextBox)gr.FindControl("sg1_t16")).Text.ToString().Trim() + "-" + ((TextBox)gr.FindControl("sg1_t15")).Text.ToString().Trim();
                drQty["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t3")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
            }
        }
        object sm;

        DataView distQty = new DataView(dtQty, "", "fstr", DataViewRowState.CurrentRows);
        DataTable dtQty1 = new DataTable();
        dtQty1 = distQty.ToTable(true, "fstr");
        foreach (DataRow drQty1 in dtQty1.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "fstr='" + drQty1["fstr"].ToString().Trim() + "'");
            string chk_itm;
            chk_itm = drQty1["fstr"].ToString().Trim().Substring(0, 8);
            string mqry;
            mqry = "select (a.Qtyord)-(a.Soldqty) as Bal_Qty from (select fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(Icode)||'-'||ordno||'-'||to_char(orddt,'dd/mm/yyyy')||'-'||trim(cdrgno) as fstr,trim(Icode) as ERP_code,Qtyord+nvl(qtysupp,0) as qtyord,0 as Soldqty,0 as prate from Somas where branchcd='" + frm_mbr + "' and type like '" + lbl1a.Text + "%' and trim(icat)!='Y'  and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text.Trim() + "' union all SELECT trim(Icode)||'-'||ponum||'-'||to_char(podate,'dd/mm/yyyy')||'-'||trim(revis_no) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyout as qtyord ,0 as irate from ivoucher where branchcd='" + frm_mbr + "' and type like '" + lbl1a.Text + "%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text.Trim() + "' and trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and a.fstr='" + drQty1["fstr"].ToString().Trim() + "' order by B.Iname,trim(a.fstr)";
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, mqry, "Bal_Qty");

            if (fgen.make_double(sm.ToString()) > fgen.make_double(col1) && fgen.make_double(col1) > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", drQty1["fstr"].ToString().Trim());

                string itm_name;
                itm_name = fgen.seek_iname(frm_qstr, frm_cocd, "select iname from ITEM where SUBSTR(ICODE,1,8)='" + chk_itm + "' ", "iname");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ERR_ITEM", itm_name + " SO Qty " + col1);

                break;

            }
        }
        return null;
    }
    string checkStockQty()
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
                drQty["fstr"] = gr.Cells[13].Text.ToString().Trim();
                drQty["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t3")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
            }
        }
        object sm;

        DataView distQty = new DataView(dtQty, "", "fstr", DataViewRowState.CurrentRows);
        DataTable dtQty1 = new DataTable();
        dtQty1 = distQty.ToTable(true, "fstr");
        foreach (DataRow drQty1 in dtQty1.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "fstr='" + drQty1["fstr"].ToString().Trim() + "'");

            col1 = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, drQty1["fstr"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");

            if (fgen.make_double(sm.ToString()) > fgen.make_double(col1) && !drQty1["fstr"].ToString().Trim().Substring(0,2).Equals("98"))
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", drQty1["fstr"].ToString().Trim() + " Stock Qty : " + col1);
                break;
            }
        }
        return null;
    }

    string check_btch_StockQty()
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
                drQty["fstr"] = gr.Cells[13].Text.ToString().Trim() + "-" + ((TextBox)gr.FindControl("sg1_t2")).Text.ToString().Trim();
                drQty["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t3")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
            }
        }
        object sm;

        DataView distQty = new DataView(dtQty, "", "fstr", DataViewRowState.CurrentRows);
        DataTable dtQty1 = new DataTable();
        dtQty1 = distQty.ToTable(true, "fstr");
        foreach (DataRow drQty1 in dtQty1.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "fstr='" + drQty1["fstr"].ToString().Trim() + "'");

            SQuery = "select trim(upper(a.batch_no)) as Fstr,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty from (SELECT trim(icode)||trim(btchno) as fstr,trim(btchno) as Batch_no,iqtyin as qtyord,0 as Soldqty from ivoucher where branchcd='" + frm_mbr + "' and type in ('15','16','17') and trim(store)='Y' and trim(icode)||'-'||trim(btchno)='" + drQty1["fstr"].ToString().Trim() + "' union all SELECT trim(icode)||trim(btchno) as fstr,trim(btchno) as Batch_no,0 as qtyord,iqtyout as Soldqty from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and trim(store)='Y' and trim(icode)||'-'||trim(btchno)='" + drQty1["fstr"].ToString().Trim() + "' and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + lbl1a.Text + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "')a  group by trim(fstr),trim(upper(a.batch_no))  having  sum(a.Qtyord)-sum(a.Soldqty) >0 order by trim(upper(a.batch_no))";
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, SQuery, "Balance_qty");

            if (fgen.make_double(sm.ToString()) > fgen.make_double(col1))
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", drQty1["fstr"].ToString().Trim() + " Stock Qty : " + col1);
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
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
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_Click(object sender, EventArgs e)
    {

        string mv_col;
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
        frm_tab_ivch = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_IVCH");
        frm_tab_sale = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_SALE");
        frm_tab_vchr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_VCHR");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                string dmlq = "";
                // Deleing data from Main Table
                dmlq = "delete from " + frm_tab_ivch + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, dmlq);
                // Deleing data from Sr Ctrl Table
                dmlq = "delete from wsr_ctrl a where a.branchcd||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, dmlq);
                // Deleing data from sale Table
                dmlq = "delete from " + frm_tab_sale + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + "" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, dmlq);
                // Deleing data from voucher Table
                dmlq = "delete from " + frm_tab_vchr + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + "" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, dmlq);
                // Deleing data from hundi Table
                dmlq = "delete from " + frm_tab_hundi + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + "" + "IV" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, dmlq);
                //

                if (sdWorking)
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM IVOUCHER WHERE BRANCHCD='" + newBranchcd + "' AND TYPE IN ('02','" + frm_vty + "') AND TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM VOUCHER WHERE BRANCHCD='" + newBranchcd + "' AND TYPE IN ('" + frm_vty + "') AND TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                    fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM SALE WHERE BRANCHCD='" + newBranchcd + "' AND TYPE IN ('" + frm_vty + "') AND TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                }

                dmlq = "delete from udf_Data a where par_tbl='" + frm_tab_ivch + "' and par_fld='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, dmlq);
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                clearctrl(); fgen.ResetForm(this.Controls);
            }
        }
        else if (hffield.Value == "DUTY")
        {
            hfCalcGST.Value = Request.Cookies["REPLY"].Value.ToString().Trim();
            fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

            switch (btnval)
            {
                case "New":
                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    string qfno = "";
                    qfno = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tab_ivch + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE like '4%' AND " + doc_df.Value + " " + DateRange + " ";
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, qfno, 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    txtlbl28.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);


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
                    //-------------------------------------------


                    break;
                    #endregion
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
                    if (frm_formID == "F55106") fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    else fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();


                    mv_col = frm_mbr + frm_vty + col1;

                    SQuery = "Select a.*,to_char(A.refdate,'dd/mm/yyyy') as refdated,to_char(A.podate,'dd/mm/yyyy') as podtd,c.Aname,nvl(b.cpartno,'-') As Icpartno,nvl(b.unit,'-') as IUnit from " + frm_tab_ivch + " a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + (sdWorking ? newBranchcd : frm_mbr) + frm_vty + col1 + "' ORDER BY A.morder";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txtvchnum.Text = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl4.Text = dt.Rows[i]["Acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();

                        txtPayTerms.Text = dt.Rows[i]["thru"].ToString().Trim();

                        txtlbl70.Text = dt.Rows[i]["gst_pos"].ToString().Trim();
                        txtlbl71.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM famst WHERE trim(acode)='" + txtlbl70.Text.Trim() + "'", "STATEn");

                        txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");
                        txtlbl73.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM famst WHERE trim(acode)='" + txtlbl4.Text.Trim() + "'", "STATEn");

                        if (frm_vty == "45")
                        {
                            col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT CESSRATE FROM FAMST WHERE ACODE='" + txtlbl4.Text.Trim() + "'", "CESSRATE");
                            if (col3 != "0") txtTCS.Text = col3;
                            else txtTCS.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT nvl(params,0) as params from controls where id='D38'", "params");
                        }
                        else txtTCS.Text = "0";


                        if (frm_formID == "F55106" && frm_cocd == "MULT")
                        {                            
                            txtInsuCharge.Text = dt.Rows[0]["COL1"].ToString();
                            txtOldBalance.Text = dt.Rows[0]["COL2"].ToString();
                            txtOthChrg.Text = dt.Rows[0]["COL3"].ToString();
                            txtAdvRcvd.Text = dt.Rows[0]["COL4"].ToString();

                            chkOldBal.Checked = (dt.Rows[0]["col6"].ToString().toDouble() == 1) ? true : false;
                            chkOthChrg.Checked = (dt.Rows[0]["col7"].ToString().toDouble() == 1) ? true : false;
                            chkAdvRcvd.Checked = (dt.Rows[0]["col8"].ToString().toDouble() == 1) ? true : false;
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


                            sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["purpose"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["ICpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["finvno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["IUnit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["no_bdls"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["btchno"].ToString().Trim();

                            sg1_dr["sg1_t3"] = dt.Rows[i]["iqtyout"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["iqty_chlwt"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["ichgs"].ToString().Trim();

                            sg1_dr["sg1_t7"] = dt.Rows[i]["exc_Rate"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["cess_percent"].ToString().Trim();

                            sg1_dr["sg1_t9"] = dt.Rows[i]["desc_"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["ccent"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["iexc_Addl"].ToString().Trim();

                            sg1_dr["sg1_t12"] = dt.Rows[i]["idiamtr"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["ipack"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["ponum"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["revis_no"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["podtd"].ToString().Trim();

                            sg1_dr["sg1_t17"] = dt.Rows[i]["exc_amt"].ToString().Trim();
                            sg1_dr["sg1_t18"] = dt.Rows[i]["cess_pu"].ToString().Trim();

                            sg1_dr["sg1_t19"] = dt.Rows[i]["tc_no"].ToString().Trim();
                            sg1_dr["sg1_t20"] = dt.Rows[i]["refdated"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        //------------------------
                        SQuery = "Select to_char(a.vchdate,'dd/mm/yyyy') as vchdt,to_char(a.remvdate,'dd/mm/yyyy') as rmvdtd,to_char(a.podate,'dd/mm/yyyy') as podtd,a.* from " + frm_tab_sale + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        create_tab2();
                        sg2_dr = null;
                        i = 0;
                        if (dt.Rows.Count > 0)
                        {
                            txtlbl2.Text = dt.Rows[i]["invtime"].ToString().Trim();
                            txtlbl3.Text = dt.Rows[i]["vchdt"].ToString().Trim();
                            txtlbl5.Text = dt.Rows[i]["pono"].ToString().Trim();
                            txtlbl6.Text = dt.Rows[i]["podtd"].ToString().Trim();

                            txtlbl7.Text = dt.Rows[0]["cscode"].ToString().Trim();
                            txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname  from csmst where trim(upper(acode))=upper(Trim('" + txtlbl7.Text + "'))", "aname");

                            txtlbl8.Text = dt.Rows[i]["destin"].ToString().Trim();
                            txtlbl9.Text = dt.Rows[i]["st_entform"].ToString().Trim();

                            txtlbl15.Text = dt.Rows[i]["mode_tpt"].ToString().Trim();
                            txtlbl16.Text = dt.Rows[i]["ins_no"].ToString().Trim();
                            txtlbl17.Text = dt.Rows[i]["freight"].ToString().Trim();
                            txtlbl18.Text = dt.Rows[i]["insur_no"].ToString().Trim();

                            txtlbl24.Text = dt.Rows[i]["CURREN"].ToString().Trim();
                            txtContTerms.Text = dt.Rows[i]["ins_cert"].ToString().Trim();
                            txtlbl26.Text = dt.Rows[i]["weight"].ToString().Trim();
                            txtlbl28.Text = dt.Rows[i]["rmvdtd"].ToString().Trim();
                            txtlbl30.Text = dt.Rows[i]["remvtime"].ToString().Trim();
                            txtrmk.Text = dt.Rows[i]["naration"].ToString().Trim();

                            txtlbl25.Text = dt.Rows[i]["amt_sale"].ToString().Trim();
                            txtlbl27.Text = dt.Rows[i]["amt_Exc"].ToString().Trim();
                            txtlbl29.Text = dt.Rows[i]["rvalue"].ToString().Trim();
                            txtlbl31.Text = dt.Rows[i]["bill_tot"].ToString().Trim();

                            txtDrvName.Text = dt.Rows[i]["DRV_NAME"].ToString().Trim();
                            txtDrvMobile.Text = dt.Rows[i]["drv_mobile"].ToString().Trim();
                            if (frm_formID != "F55106")
                            {
                                txtCashDisc.Text = dt.Rows[i]["ACVDRT"].ToString().Trim();
                                txtCashDiscValue.Text = dt.Rows[i]["TOTDISC_AMT"].ToString().Trim();
                            }

                            txtGrno.Text = dt.Rows[i]["GRNO"].ToString().Trim();
                            txtGrDt.Text = dt.Rows[i]["GRDATE"].ToString().Trim();
                        }
                        sg2_add_blankrows();
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        dt.Dispose();
                        sg2_dt.Dispose();
                        //-----------------------
                        sg2_add_blankrows();
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        dt.Dispose();
                        sg2_dt.Dispose();
                        //------------------------
                        SQuery = "Select nvl(a.udf_name,'-') as udf_name,nvl(a.udf_value,'-') as udf_value from udf_Data a where trim(a.par_tbl)='" + frm_tab_ivch + "' and trim(a.par_fld)='" + mv_col + "' ORDER BY a.srno";
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
                                sg4_dr["sg4_t2"] = dt.Rows[i]["udf_value"].ToString().Trim();

                                sg4_dt.Rows.Add(sg4_dr);
                            }
                        }
                        sg4_add_blankrows();
                        ViewState["sg4"] = sg4_dt;
                        sg4.DataSource = sg4_dt;
                        sg4.DataBind();
                        dt.Dispose();
                        sg4_dt.Dispose();
                        //------------------------

                        //------------------------
                        SQuery = "Select a.icode,to_chaR(a.dlv_Date,'dd/mm/yyyy') As dlv_Date,nvl(a.budgetcost,0) as budgetcost,nvl(a.actualcost,0) as actualcost,a.jobcardrqd,b.iname,nvl(b.cpartno,'-') As cpartno,nvl(b.cdrgno,'-') as cdrgno,nvl(b.unit,'-') as Unit from budgmst a,item b where trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' and 1=2 ORDER BY A.SRNO ";
                        //union all Select '-' as icode,to_DatE(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy') as dlv_Date,0 as budgetcost,'-' as iname,'-' As cpartno,'-' as cdrgno,nvl(b.unit,'-') as Unit from dual                         

                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        create_tab3();
                        sg3_dr = null;
                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                sg3_dr = sg3_dt.NewRow();
                                sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;
                                sg3_dr["sg3_f1"] = dt.Rows[i]["icode"].ToString().Trim();
                                sg3_dr["sg3_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                                sg3_dr["sg3_t1"] = dt.Rows[i]["dlv_Date"].ToString().Trim();
                                sg3_dr["sg3_t2"] = dt.Rows[i]["budgetcost"].ToString().Trim();
                                sg3_dr["sg3_t3"] = dt.Rows[i]["actualcost"].ToString().Trim();
                                sg3_dr["sg3_t4"] = dt.Rows[i]["jobcardrqd"].ToString().Trim();
                                sg3_dt.Rows.Add(sg3_dr);
                            }
                        }
                        sg3_add_blankrows();
                        ViewState["sg3"] = sg3_dt;
                        sg3.DataSource = sg3_dt;
                        sg3.DataBind();
                        dt.Dispose();
                        sg3_dt.Dispose();

                        //-----------------------
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "sELECT * FROM " + frm_tab_hundi + " WHERE BRANCHCD||TYPE||TRIM(vCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + frm_mbr + "IV" + col1 + "' ");
                        if (dt.Rows.Count > 0)
                        {
                            txtlbl40.Text = dt.Rows[0]["exprmk1"].ToString().Trim();
                            txtlbl41.Text = dt.Rows[0]["exprmk2"].ToString().Trim();
                            txtlbl46.Text = dt.Rows[0]["exprmk3"].ToString().Trim();
                            txtlbl47.Text = dt.Rows[0]["exprmk4"].ToString().Trim();
                            txtlbl48.Text = dt.Rows[0]["exprmk5"].ToString().Trim();

                            txtlbl43.Text = dt.Rows[0]["TMADDL1"].ToString().Trim();
                            txtlbl49.Text = dt.Rows[0]["TMADDL2"].ToString().Trim();

                            txtGrWt.Text = dt.Rows[0]["naration"].ToString().Trim();
                            txtNetWt.Text = dt.Rows[0]["remark3"].ToString().Trim();
                        }

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
                    hfCalcGST.Value = "";
                    break;
                case "PICK_DA":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();


                    mv_col = frm_mbr + frm_vty + col1;

                    if (doc_GST.Value == "N")
                    {
                        SQuery = "select a.acode,b.aname,b.staffcd as gst_pos,b.staten,'-' as desc_,a.icode,sum(a.qtyord)-sum(a.chl_qty) as qtysupp,max(a.cdisc) as cdisc,max(a.ciname) as ciname,max(a.cpartno) As cpartno,a.ordno as ponum,to_Char(a.orddt,'dd/mm/yyyy') as podate,a.vchnum,to_chaR(a.vchdate,'dd/mm/yyyy') as Vchdated,0 as irate,A.vchnum||'-'||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||trim(a.ordline) as refr,trim(a.ordline) as ordline,0 as num4,0 as num5,0 as num6 from (select ciname,cpartno,acode,ordno,orddt,PACKNO AS vchnum,PACKDATE AS vchdate,icode,QTYSUPP as qtyord,0 as chl_qty,cdisc,ordline from DESPATCH where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(Acode)='" + txtlbl4.Text + "' union all select null as ciname,null as cpartno,acode,ponum,podate,tc_no,refdate,icode,0 as qtyord,iqtyout as chl_qty,0 as cdisc,revis_no from " + frm_tab_ivch + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(Acode)='" + txtlbl4.Text + "') a ,famst b where trim(A.acode)=trim(B.acode) and a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) in(" + col1 + ") group by a.acode,b.staffcd,b.aname,b.staten,a.acode,a.icode,a.ordno,a.orddt,a.vchnum,to_Char(a.vchdate,'dd/mm/yyyy'),A.vchnum||'-'||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||trim(a.ORDLINE),trim(a.ordline) having sum(a.qtyord)-sum(a.chl_qty)>0 order by a.vchnum";
                    }
                    else
                    {
                        SQuery = "select a.acode,b.aname,b.staffcd as gst_pos,b.staten,'-' as desc_,a.icode,sum(a.qtyord)-sum(a.chl_qty) as qtysupp,max(a.cdisc) as cdisc,max(a.ciname) as ciname,max(a.cpartno) As cpartno,a.ordno as ponum,to_Char(a.orddt,'dd/mm/yyyy') as podate,a.vchnum,to_chaR(a.vchdate,'dd/mm/yyyy') as Vchdated,0 as irate,A.vchnum||'-'||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||trim(a.ordline) as refr,trim(a.ordline) as ordline,d.num4,d.num5,d.num6 from (select ciname,cpartno,acode,ordno,orddt,PACKNO AS vchnum,PACKDATE AS vchdate,icode,QTYSUPP as qtyord,0 as chl_qty,cdisc,ordline from DESPATCH where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(Acode)='" + txtlbl4.Text + "' union all select null as ciname,null as cpartno,acode,ponum,podate,tc_no,refdate,icode,0 as qtyord,iqtyout as chl_qty,0 as cdisc,revis_no from " + frm_tab_ivch + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(Acode)='" + txtlbl4.Text + "') a ,famst b,item c,typegrp d where d.id='T1' and trim(A.acode)=trim(B.acode) and trim(c.hscode)=trim(d.acref) and trim(A.icode)=trim(c.icode) and a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) in(" + col1 + ") group by a.acode,b.staffcd,b.aname,b.staten,a.acode,a.icode,a.ordno,a.orddt,a.vchnum,to_Char(a.vchdate,'dd/mm/yyyy'),A.vchnum||'-'||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||trim(a.ORDLINE),trim(a.ordline),d.num4,d.num5,d.num6 having sum(a.qtyord)-sum(a.chl_qty)>0 order by a.vchnum";
                    }

                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {

                        txtlbl4.Text = dt.Rows[i]["Acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();

                        txtlbl70.Text = dt.Rows[i]["gst_pos"].ToString().Trim();
                        txtlbl71.Text = dt.Rows[i]["aname"].ToString().Trim();

                        txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");
                        txtlbl73.Text = dt.Rows[i]["staten"].ToString().Trim();


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


                            sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["ciname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["Cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";

                            sg1_dr["sg1_t1"] = "-";
                            sg1_dr["sg1_t2"] = "-";

                            sg1_dr["sg1_t3"] = dt.Rows[i]["qtysupp"].ToString().Trim();

                            string col11 = "";
                            col11 = fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(irate,0)||'~'||nvl(cdisc,0)||'~'||trim(pordno) as PP  from somas where  branchcd='" + frm_mbr + "' and trim(type)='" + frm_vty + "' and trim(ordno)='" + dt.Rows[i]["ponum"].ToString().Trim() + "' and trim(cdrgno)='" + dt.Rows[i]["ordline"].ToString().Trim() + "' and to_Char(orddt,'dd/mm/yyyy')='" + dt.Rows[i]["podate"].ToString().Trim() + "' and trim(acode)='" + txtlbl4.Text + "' and trim(icode)='" + dt.Rows[i]["Icode"].ToString().Trim() + "'", "PP");

                            if (col11.Length > 1)
                            {
                                sg1_dr["sg1_t4"] = col11.Split('~')[0].ToString();
                                sg1_dr["sg1_t5"] = col11.Split('~')[1].ToString();

                                sg1_dr["sg1_f4"] = col11.Split('~')[2].ToString();

                                if (txtlbl5.Text == "" || txtlbl5.Text == "-" || txtlbl5.Text == "0")
                                    txtlbl5.Text = col11.Split('~')[2].ToString();

                            }
                            else
                            {
                                sg1_dr["sg1_t4"] = dt.Rows[i]["irate"].ToString().Trim();
                                sg1_dr["sg1_t5"] = dt.Rows[i]["cdisc"].ToString().Trim();

                            }
                            if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                            {
                                sg1_dr["sg1_t7"] = dt.Rows[i]["num4"].ToString().Trim();
                                sg1_dr["sg1_t8"] = dt.Rows[i]["num5"].ToString().Trim();
                            }
                            else
                            {
                                sg1_dr["sg1_t7"] = dt.Rows[i]["num6"].ToString().Trim();
                                sg1_dr["sg1_t8"] = "0";
                            }


                            if (doc_GST.Value == "GCC")
                            {
                                sg1_dr["sg1_t7"] = dt.Rows[i]["num6"].ToString().Trim();
                                sg1_dr["sg1_t8"] = "0";
                            }
                            if (doc_GST.Value == "GCC" || frm_vty == "4F" || frm_vty == "4T")
                            {
                                sg1_dr["sg1_t7"] = "0";
                                sg1_dr["sg1_t8"] = "0";
                            }

                            sg1_dr["sg1_t9"] = "-";
                            sg1_dr["sg1_t10"] = "-";
                            sg1_dr["sg1_t11"] = 0;
                            //dt.Rows[i]["iexc_Addl"].ToString().Trim();

                            sg1_dr["sg1_t12"] = 0;
                            sg1_dr["sg1_t13"] = 0;
                            sg1_dr["sg1_t14"] = dt.Rows[i]["ponum"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["ordline"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["podate"].ToString().Trim();

                            sg1_dr["sg1_t19"] = dt.Rows[i]["vchnum"].ToString().Trim();
                            sg1_dr["sg1_t20"] = dt.Rows[i]["vchdated"].ToString().Trim();
                            //cow





                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();

                        //-----------------------
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "N";
                    }
                    #endregion
                    break;

                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;
                case "Print_E":
                    if (col1.Length < 2) return;
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
                    fgen.fin_esales_reps(frm_qstr);
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;

                    txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");
                    txtlbl73.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");

                    txtlbl70.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT type1 FROM TYPE WHERE ID='{' AND upper(Trim(Name))=upper(Trim('" + txtlbl73.Text + "'))", "type1");
                    txtlbl71.Text = txtlbl73.Text;
                    btnlbl7.Focus();


                    string app_rt1;
                    app_rt1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT vchnum FROM sale WHERE branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(acode)='" + txtlbl4.Text + "' order by vchdate desc", "vchnum");
                    if (app_rt1 != "0")
                    {
                        app_rt1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT nvl(destin,'-')||'~'||nvl(mode_Tpt,'-')||'~'||nvl(ins_no,'-')||'~'||nvl(freight,'-')||'~'||nvl(insur_no,'-') as skfstr FROM sale WHERE branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(acode)='" + txtlbl4.Text + "'  order by vchdate desc", "skfstr");
                        if (app_rt1.Contains("~"))
                        {

                            txtlbl8.Text = app_rt1.Split('~')[0].ToString();
                            txtlbl15.Text = app_rt1.Split('~')[1].ToString();
                            txtlbl16.Text = app_rt1.Split('~')[2].ToString();
                            txtlbl17.Text = app_rt1.Split('~')[3].ToString();
                            txtlbl18.Text = app_rt1.Split('~')[4].ToString();

                        }
                    }


                    if (frm_vty == "45")
                    {
                        col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT CESSRATE FROM FAMST WHERE ACODE='" + col1 + "'", "CESSRATE");
                        if (col3 != "0") txtTCS.Text = col3;
                        else txtTCS.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT nvl(params,0) as params from controls where id='D38'", "params");
                    }
                    else txtTCS.Text = "0";

                    string chk_opt = "";
                    if (frm_formID != "F55106")
                    {
                        chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0044'", "fstr");
                        if (chk_opt == "Y")
                        {
                            hffield.Value = "PICK_DA";
                            make_qry_4_popup();
                            fgen.Fn_open_mseek("Select Type", frm_qstr);
                        }
                    }
                    col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT MAX(A.par_fld) AS PAR_FLD FROM udf_Data a where trim(a.par_tbl)='" + frm_tab_ivch + "'", "PAR_FLD");
                    SQuery = "Select nvl(a.udf_name,'-') as udf_name,nvl(a.udf_value,'-') as udf_value from udf_Data a where trim(a.par_tbl)='" + frm_tab_ivch + "' AND TRIM(A.PAR_FLD)='" + col3.Trim() + "' ORDER BY a.srno";
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
                            sg4_dr["sg4_t2"] = dt.Rows[i]["udf_value"].ToString().Trim();

                            sg4_dt.Rows.Add(sg4_dr);
                        }
                    }
                    sg4_add_blankrows();
                    ViewState["sg4"] = sg4_dt;
                    sg4.DataSource = sg4_dt;
                    sg4.DataBind();
                    dt.Dispose();
                    sg4_dt.Dispose();
                    break;
                case "BTN_10":
                    if (col1.Length <= 0) return;
                    txtlbl10.Text = col2;
                    btnlbl11.Focus();
                    break;
                case "BTN_11":
                    if (col1.Length <= 0) return;
                    txtlbl11.Text = col2;
                    btnlbl12.Focus();
                    break;
                case "BTN_12":
                    if (col1.Length <= 0) return;
                    txtlbl12.Text = col2;
                    btnlbl13.Focus();
                    break;
                case "BTN_13":
                    if (col1.Length <= 0) return;
                    txtlbl13.Text = col2;
                    btnlbl14.Focus();
                    break;
                case "BTN_14":
                    if (col1.Length <= 0) return;
                    txtlbl14.Text = col2;
                    btnlbl15.Focus();
                    break;
                case "BTN_15":
                    if (col1.Length <= 0) return;
                    txtlbl15.Text = col2;
                    //btnlbl16.Focus();
                    break;
                case "BTN_16":
                    if (col1.Length <= 0) return;
                    txtlbl16.Text = col2;
                    //btnlbl17.Focus();
                    break;
                case "BTN_17":
                    if (col1.Length <= 0) return;
                    txtlbl17.Text = col2;
                    //btnlbl18.Focus();
                    break;
                case "BTN_18":
                    if (col1.Length <= 0) return;
                    txtlbl18.Text = col2;
                    break;


                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    txtlbl2.Focus();
                    break;
                case "TICODEX":
                    if (col1.Length <= 0) return;
                    txtlbl70.Text = col1;
                    txtlbl71.Text = col2;
                    txtlbl2.Focus();
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
                            sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();
                            sg1_dr["sg1_t19"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                            sg1_dr["sg1_t20"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();


                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");
                        if (doc_GST.Value == "N")
                        {
                            if (col1.Trim().Length == 8) SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,0 as num4,0 as num5,0 as num6,0 as num7,a.olineno,a.std_pack from (" + pop_qry + ") a where trim(a.fstr) in ('" + col1 + "')";
                            else SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,0 as num4,0 as num5,0 as num6,0 as num7,a.olineno,a.std_pack from (" + pop_qry + ") a where trim(a.fstr) in (" + col1 + ")";

                        }
                        else
                        {
                            if (col1.Trim().Length == 8) SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.olineno,a.std_pack from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                            else SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.olineno,a.std_pack from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in (" + col1 + ")";

                        }
                        if (Prg_Id == "F55106")
                        {
                            if (col1.Trim().Length == 8) SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.olineno,a.std_pack from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                            else SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.olineno,a.std_pack from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in (" + col1 + ")";
                        }

                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_h3"] = dt.Rows[d]["std_pack"].ToString().Trim();
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
                            sg1_dr["sg1_f4"] = dt.Rows[d]["po_no"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = "";
                            sg1_dr["sg1_t2"] = "";
                            if (fgen.make_double(dt.Rows[d]["Balance_Qty"].ToString().Trim()) < 0)
                            {
                                sg1_dr["sg1_t3"] = "0";
                            }
                            else
                            {
                                sg1_dr["sg1_t3"] = dt.Rows[d]["Balance_Qty"].ToString().Trim();
                            }
                            sg1_dr["sg1_t4"] = dt.Rows[d]["Irate"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[d]["cDisc"].ToString().Trim();



                            if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                            {
                                sg1_dr["sg1_t7"] = dt.Rows[d]["num4"].ToString().Trim();
                                sg1_dr["sg1_t8"] = dt.Rows[d]["num5"].ToString().Trim();
                            }
                            else
                            {
                                sg1_dr["sg1_t7"] = dt.Rows[d]["num6"].ToString().Trim();
                                sg1_dr["sg1_t8"] = "0";
                            }

                            if (doc_GST.Value == "GCC")
                            {
                                sg1_dr["sg1_t7"] = dt.Rows[d]["num6"].ToString().Trim();
                                sg1_dr["sg1_t8"] = "0";
                            }

                            if (doc_GST.Value == "GCC" || frm_vty == "4F" || frm_vty == "4T")
                            {
                                sg1_dr["sg1_t7"] = "0";
                                sg1_dr["sg1_t8"] = "0";
                            }

                            sg1_dr["sg1_t9"] = "";
                            sg1_dr["sg1_t10"] = "-";
                            sg1_dr["sg1_t11"] = dt.Rows[d]["iexc_Addl"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[d]["frt_pu"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[d]["pkchg_pu"].ToString().Trim();

                            string mpo_Dt;
                            try
                            {
                                mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(9, 6);
                                sg1_dr["sg1_t14"] = mpo_Dt;
                                col1 = mpo_Dt;
                                sg1_dr["sg1_t15"] = dt.Rows[d]["olineno"].ToString().Trim();
                                mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(6, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(4, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 4);
                                sg1_dr["sg1_t16"] = fgen.make_def_Date(mpo_Dt, vardate);

                                if (txtlbl24.Text == "")
                                {
                                    txtlbl24.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT CURRENCY FROM SOMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('4F','4T') AND TRIM(ORDNO)||TO_CHAR(ORDDT,'DD/MM/YYYY')||TRIM(ICODE)='" + sg1_dr["sg1_t14"].ToString() + sg1_dr["sg1_t16"].ToString() + dt.Rows[d]["icode"].ToString().Trim() + "'", "CURRENCY");
                                    txtlbl26.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT CURR_RATE FROM SOMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('4F','4T') AND TRIM(ORDNO)||TO_CHAR(ORDDT,'DD/MM/YYYY')||TRIM(ICODE)='" + sg1_dr["sg1_t14"].ToString() + sg1_dr["sg1_t16"].ToString() + dt.Rows[d]["icode"].ToString().Trim() + "'", "CURR_RATE");
                                }

                                if (txtlbl5.Text.Trim().Length <= 1)
                                {
                                    txtlbl5.Text = dt.Rows[d]["po_no"].ToString().Trim();
                                    txtlbl6.Text = mpo_Dt;
                                }
                            }
                            catch { }
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
                    setGST();
                    break;
                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    if (edmode.Value == "Y")
                    {
                        //return;
                    }

                    pop_qry = "";
                    pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");
                    SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,a.std_pack,b.num4,b.num5,b.num6,b.num7 from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                    //else SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in (" + col1 + ")";

                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    for (int d = 0; d < dt.Rows.Count; d++)
                    {





                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = dt.Rows[d]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = dt.Rows[d]["iname"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[2].Text = dt.Rows[d]["std_pack"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = "-";
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = "-";
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[5].Text = "-";
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[6].Text = "-";
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[7].Text = "-";
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[8].Text = "-";
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[9].Text = "-";

                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[d]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[d]["iname"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[d]["cpartno"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[d]["po_no"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[d]["unit"].ToString().Trim();

                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t1")).Text = "";
                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t2")).Text = "";
                        if (fgen.make_double(dt.Rows[d]["Balance_Qty"].ToString().Trim()) < 0)
                        {
                            ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t3")).Text = "0";
                        }
                        else
                        {
                            ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t3")).Text = dt.Rows[d]["Balance_Qty"].ToString().Trim();
                        }
                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t4")).Text = dt.Rows[d]["Irate"].ToString().Trim();
                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t5")).Text = dt.Rows[d]["cDisc"].ToString().Trim();



                        if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                        {
                            ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t7")).Text = dt.Rows[d]["num4"].ToString().Trim();
                            ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t8")).Text = dt.Rows[d]["num5"].ToString().Trim();
                        }
                        else
                        {
                            ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t7")).Text = dt.Rows[d]["num6"].ToString().Trim();
                            ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t8")).Text = "0";
                        }

                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t9")).Text = "";
                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t10")).Text = "-";
                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t11")).Text = dt.Rows[d]["iexc_Addl"].ToString().Trim();
                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t12")).Text = dt.Rows[d]["frt_pu"].ToString().Trim();
                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t13")).Text = dt.Rows[d]["pkchg_pu"].ToString().Trim();

                        string mpo_Dt = "";
                        if (dt.Rows[d]["fstr"].ToString().Trim().Length > 9)
                        {
                            mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(9, 6);
                            ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t14")).Text = mpo_Dt;
                            ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t15")).Text = "";
                            mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(6, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(4, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 4);
                            ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t16")).Text = fgen.make_def_Date(mpo_Dt, vardate);
                        }
                        if (txtlbl5.Text.Trim().Length <= 1)
                        {
                            txtlbl5.Text = dt.Rows[d]["po_no"].ToString().Trim();
                            txtlbl6.Text = mpo_Dt;
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
                case "SG1_ROW_TAX":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");

                    break;
                case "SG1_ROW_DT":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = col1;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = col3;
                    //fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
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
                            sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();
                            sg1_dr["sg1_t19"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                            sg1_dr["sg1_t20"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim();

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
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tab_ivch = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_IVCH");
        frm_tab_sale = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_SALE");
        frm_tab_vchr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_VCHR");
        if (hffield.Value == "List")
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
            SQuery = "Select a.Vchnum as Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Dated,c.Aname as Customer,a.purpose  as Item_Name,a.exc_57f4 as Part_No,a.iqtyout as sale_Qty,a.Irate,a.ichgs as Disc,b.unit,b.hscode,a.Desc_,a.icode,a.ent_by,a.ent_Dt from " + frm_tab_ivch + " a, item b,famst c where a.branchcd='" + frm_mbr + "'  and a.type='" + frm_vty + "' and a." + doc_df.Value + " " + PrdRange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a." + doc_df.Value + ",a." + doc_nf.Value + ",a.morder ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);

            hffield.Value = "-";

        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------

            //-----------------------------
            i = 0;
            hffield.Value = "";

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
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tab_ivch);

                        oDS2 = new DataSet();
                        oporow2 = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tab_sale);

                        oDS3 = new DataSet();
                        oporow3 = null;
                        //oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "poterm");

                        oDS4 = new DataSet();
                        oporow4 = null;
                        //oDS4 = fgen.fill_schema(frm_qstr, frm_cocd, "budgmst");

                        oDS5 = new DataSet();
                        oporow5 = null;
                        oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();
                        save_fun2();
                        //save_fun3();
                        //save_fun4();
                        save_fun5();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tab_ivch);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tab_sale);

                        oDS3.Dispose();
                        oporow3 = null;
                        oDS3 = new DataSet();
                        //oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "poterm");

                        oDS4.Dispose();
                        oporow4 = null;
                        oDS4 = new DataSet();
                        //oDS4 = fgen.fill_schema(frm_qstr, frm_cocd, "budgmst");

                        oDS5.Dispose();
                        oporow5 = null;
                        oDS5 = new DataSet();
                        oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");


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
                                frm_vnum = fgen.Fn_next_doc_no_inv(frm_qstr, frm_cocd, frm_tab_ivch, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                                if (brPrefixWithInvNo.Value == "Y" && frm_vnum == "000001")
                                {
                                    frm_vnum = frm_mbr + frm_vnum.Substring(2, 4);
                                }
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        save_fun2();
                        //save_fun3();
                        //save_fun4();
                        save_fun5();
                        string ddl_fld1;
                        string ddl_fld2;
                        ddl_fld1 = frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                        ddl_fld2 = frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");

                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tab_ivch + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tab_sale + " set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tab_vchr + " set branchcd='88' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tab_hundi + " set branchcd='88' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + "IV" + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "update budgmst set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update udf_Data set branchcd='DD' where par_tbl='" + frm_tab_ivch + "' and par_fld='" + ddl_fld1 + "'");
                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tab_ivch);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, frm_tab_sale);


                        #region Hundi Saving
                        SQuery = "INSERT INTO " + frm_tab_hundi + " (BRANCHCD,TYPE,VCHNUM,VCHDATE,INVNO,INVDATE,ACODE,EXPRMK1,EXPRMK2,EXPRMK3,EXPRMK4,EXPRMK5,DUE_DATE,REFNUM,REFDATE,HUNDIAMT,PAY_DATE,RCODE,INVAMT,BANK,TMADDL1,TMADDL2,NARATION,REMARK3) VALUES " +
                        "('" + frm_mbr + "','IV','" + frm_vnum + "',to_date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy'),'" + frm_vnum + "',to_date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy'),'" + txtlbl4.Text.Trim() + "','" + txtlbl40.Text.Trim() + "','" + txtlbl41.Text.Trim() + "','" + txtlbl46.Text.Trim() + "','" + txtlbl47.Text.Trim() + "','" + txtlbl48.Text.Trim() + "',SYSDATE" +
                        ",'-',SYSDATE,'0',SYSDATE,'-','0','-','" + txtlbl43.Text.Trim().ToUpper() + "','" + txtlbl49.Text.Trim().ToUpper() + "','" + txtGrWt.Text.Trim() + "','" + txtNetWt.Text.Trim() + "') ";
                        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        #endregion

                        #region Voucher Saving
                        string sal_code = "", par_code = "", tax_code = "", tax_code2 = "", schg_code;
                        par_code = txtlbl4.Text.Trim();

                        string optwb = "";
                        optwb = fgen.getOption(frm_qstr, frm_cocd, "W0100", "OPT_ENABLE");
                        if (lbl27.Text.Substring(0, 2) == "CG")
                        {

                            if (optwb == "Y")
                            {
                                tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0077", "OPT_PARAM");
                                sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0077", "OPT_PARAM2");
                                tax_code2 = fgen.getOption(frm_qstr, frm_cocd, "W0078", "OPT_PARAM");
                            }
                            else
                            {
                                tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A77'", "PARAMS");
                                sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A77'", "PARAMS2");
                                tax_code2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A78'", "PARAMS");
                            }
                        }
                        else
                        {

                            if (optwb == "Y")
                            {
                                tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0079", "OPT_PARAM");
                                sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0079", "OPT_PARAM2");
                            }
                            else
                            {
                                tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A79'", "PARAMS");
                                sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A79'", "PARAMS2");

                            }


                        }


                        schg_code = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(params) as param from controls where id='A41'", "param");
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                        sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE FROM TYPE WHERE ID='V' AND TYPE1='" + frm_vty + "'", "ACODE");


                        string rmToVSave = "Sale Inv.No " + frm_vnum;
                        //if (Prg_Id == "F50035")
                        //{
                        //    rmToVSave = txtrmk.Text.Trim() + " " + frm_vnum;
                        //    fgen.vSave(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, tax_code, (fgen.make_double(txtlbl27.Text.Trim()) + fgen.make_double(txtlbl29.Text.Trim())).ToString(), "0", frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, "0", "0", "1", "0", "0", "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), "0", billQty.ToString());
                        //}
                        //else
                        //{
                        //}                                        

                        double currRate = 0;
                        currRate = txtlbl26.Text.Trim().toDouble();

                        if (currRate <= 0) currRate = 1;

                        double billQty;
                        billQty = 1;
                        frm_vty = lbl1a.Text.Substring(0, 2);
                        //working here                        
                        int srn = 50;
                        if (chkFOC.Checked == true)
                        {
                            string code4s = fgen.check_control(frm_qstr, frm_cocd, "D80");
                            if (hfCalcGST.Value == "Y")
                            {
                                fgen.vSave(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, sal_code, Math.Round((fgen.make_double(txtlbl27.Text.Trim(), 2) + fgen.make_double(txtlbl29.Text.Trim(), 2)) * currRate, 2), 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, Math.Round((fgen.make_double(txtlbl27.Text.Trim(), 2) + fgen.make_double(txtlbl29.Text.Trim(), 2)) * currRate, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
                            }

                            if (code4s != "0")
                            {
                                srn += 1;
                                fgen.vSave(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), srn, sal_code, code4s, Math.Round(fgen.make_double(txtlbl25.Text, 2) * currRate, 2), 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, Math.Round(fgen.make_double(txtlbl25.Text, 2) * currRate, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
                                srn += 1;
                                fgen.vSave(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), srn, code4s, sal_code, 0, Math.Round(fgen.make_double(txtlbl25.Text, 2) * currRate, 2), frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, 0, Math.Round(fgen.make_double(txtlbl25.Text, 2) * currRate, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
                            }
                        }
                        else
                        {
                            fgen.vSave(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, sal_code, Math.Round(fgen.make_double(txtlbl31.Text.Trim(), 2) * currRate, 2), 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, Math.Round(fgen.make_double(txtlbl31.Text.Trim(), 2) * currRate, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
                            srn += 1;
                            fgen.vSave(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), srn, sal_code, par_code, 0, Math.Round(fgen.make_double(txtlbl25.Text, 2) * currRate, 2), frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, 0, Math.Round(fgen.make_double(txtlbl25.Text, 2) * currRate, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
                        }
                        if (hfCalcGST.Value == "Y")
                        {
                            srn += 1;
                            fgen.vSave(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), srn, tax_code, par_code, 0, Math.Round(fgen.make_double(txtlbl27.Text, 2) * currRate, 2), frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, 0, Math.Round(fgen.make_double(txtlbl27.Text, 2) * currRate, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);

                            if (tax_code2.Length > 0)
                            {
                                srn += 1;
                                fgen.vSave(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), srn, tax_code2, par_code, 0, Math.Round(fgen.make_double(txtlbl29.Text, 2) * currRate, 2), frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, 0, Math.Round(fgen.make_double(txtlbl29.Text, 2) * currRate, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
                            }
                        }
                        if (frm_vty == "45" && fgen.make_double(txtTCSA.Text) > 0)
                        {
                            srn += 1;
                            tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='D37'", "params");
                            fgen.vSave(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), srn, tax_code, par_code, 0, fgen.make_double(txtTCSA.Text, 2), frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, 0, fgen.make_double(txtTCSA.Text, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
                        }
                        #endregion
                        //fgen.save_data(frm_qstr, frm_cocd, oDS3, "poterm");
                        //fgen.save_data(frm_qstr, frm_cocd, oDS4, "budgmst");
                        fgen.save_data(frm_qstr, frm_cocd, oDS5, "udf_Data");

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully'13'Do you want to see the Print Preview ?");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tab_ivch + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tab_sale + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tab_vchr + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='88" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tab_hundi + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='88" + "IV" + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from budgmst where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data where branchcd='DD' and par_tbl='" + frm_tab_ivch + "' and par_fld='" + frm_mbr + ddl_fld2 + "'");
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully'13'Do you want to see the Print Preview ?");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }

                        if (sdWorking)
                        {
                            //sdSavingInvAndMRR();
                        }

                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtvchdate.Text.Trim() + "'");
                        if (frm_cocd == "SAIA")
                            sendMail();
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                        hffield.Value = "SAVED";
                        hfCalcGST.Value = "";
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
    //--------------------------
    public void sdSavingInvAndMRR()
    {
        fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM IVOUCHER WHERE BRANCHCD='" + newBranchcd + "' AND TYPE IN ('02','" + frm_vty + "') AND TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY')='" + frm_vnum + txtvchdate.Text + "'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM VOUCHER WHERE BRANCHCD='" + newBranchcd + "' AND TYPE IN ('" + frm_vty + "') AND TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY')='" + frm_vnum + txtvchdate.Text + "'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM VOUCHER WHERE BRANCHCD='" + newBranchcd + "' AND TYPE IN ('" + "50" + "') AND TRIM(INVNO)||TO_cHAR(INVDATE,'DD/MM/YYYY')='" + frm_vnum + txtvchdate.Text + "'");
        fgen.execute_cmd(frm_qstr, frm_cocd, "DELETE FROM SALE WHERE BRANCHCD='" + newBranchcd + "' AND TYPE IN ('" + frm_vty + "') AND TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY')='" + frm_vnum + txtvchdate.Text + "'");

        DataSet OLDDS = new DataSet();
        OLDDS = oDS;

        #region ivoucher MRR
        OLDDS = new DataSet();
        OLDDS = oDS;
        sdOds = new DataSet();
        oporow2 = null;
        sdOds = fgen.fill_schema(frm_qstr, frm_cocd, frm_tab_ivch);
        double sdTotalValue = 0, sdTotalCGST = 0, sdTotalSGST = 0, sdTotalBasic = 0;
        // saving mrr in another branch
        foreach (DataRow dr in OLDDS.Tables[0].Rows)
        {
            oporow2 = sdOds.Tables[0].NewRow();
            for (int i = 0; i < OLDDS.Tables[0].Columns.Count; i++)
            {
                oporow2[i] = dr[i];
            }
            oporow2["BRANCHCD"] = newBranchcd;
            oporow2["TYPE"] = "02";
            oporow2["ACODE"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "ACODE");
            oporow2["IQTYIN"] = dr["IQTYOUT"];
            oporow2["IQTY_CHL"] = dr["IQTYOUT"];
            oporow2["IQTY_OK"] = dr["IQTYOUT"];
            oporow2["ACPT_UD"] = dr["IQTYOUT"];
            oporow2["REJ_RW"] = "0";
            oporow2["IQTYOUT"] = "0";
            oporow2["IRATE"] = getDiscountedRate(dr["icode"].ToString().Trim(), dr["irate"].ToString(), "0");
            oporow2["IAMOUNT"] = Math.Round(oporow2["IQTYIN"].ToString().toDouble() * oporow2["IRATE"].ToString().toDouble(), 2);

            if (oporow2["EXC_RATE"].ToString().Trim().toDouble() > 0)
                oporow2["EXC_AMT"] = Math.Round(oporow2["IAMOUNT"].ToString().toDouble() * (oporow2["EXC_RATE"].ToString().toDouble() / 100), 2);
            if (oporow2["CESS_PERCENT"].ToString().Trim().toDouble() > 0)
                oporow2["CESS_PU"] = Math.Round(oporow2["IAMOUNT"].ToString().toDouble() * (oporow2["CESS_PERCENT"].ToString().toDouble() / 100), 2);

            sdTotalBasic += oporow2["IAMOUNT"].ToString().toDouble();
            sdTotalCGST += oporow2["exc_amt"].ToString().toDouble();
            sdTotalSGST += oporow2["cess_pu"].ToString().toDouble();

            oporow2["INSPECTED"] = "Y";
            sdOds.Tables[0].Rows.Add(oporow2);
        }
        sdTotalBasic = Math.Round(sdTotalBasic, 2);
        sdTotalCGST = Math.Round(sdTotalCGST, 2);
        sdTotalSGST = Math.Round(sdTotalSGST, 2);
        sdTotalValue = Math.Round(sdTotalBasic + sdTotalCGST + sdTotalSGST, 2);
        fgen.save_data(frm_qstr, frm_cocd, sdOds, frm_tab_ivch);
        #endregion

        #region ivoucher saving
        sdOds = new DataSet();
        oporow2 = null;
        sdOds = fgen.fill_schema(frm_qstr, frm_cocd, frm_tab_ivch);
        // saving invoice in another branch
        foreach (DataRow dr in OLDDS.Tables[0].Rows)
        {
            oporow2 = sdOds.Tables[0].NewRow();
            for (int i = 0; i < OLDDS.Tables[0].Columns.Count; i++)
            {
                oporow2[i] = dr[i];
            }
            oporow2["BRANCHCD"] = newBranchcd;
            oporow2["ACODE"] = txtlbl4.Text;
            sdOds.Tables[0].Rows.Add(oporow2);
        }
        fgen.save_data(frm_qstr, frm_cocd, sdOds, frm_tab_ivch);
        #endregion

        #region sale saving
        sdOds = new DataSet();
        OLDDS = oDS2;
        oporow2 = null;
        sdOds = fgen.fill_schema(frm_qstr, frm_cocd, frm_tab_sale);
        // saving invoice in another branch
        foreach (DataRow dr in OLDDS.Tables[0].Rows)
        {
            oporow2 = sdOds.Tables[0].NewRow();
            for (int i = 0; i < OLDDS.Tables[0].Columns.Count; i++)
            {
                oporow2[i] = dr[i];
            }
            oporow2["BRANCHCD"] = newBranchcd;
            oporow2["ACODE"] = txtlbl4.Text;
            sdOds.Tables[0].Rows.Add(oporow2);
        }
        fgen.save_data(frm_qstr, frm_cocd, sdOds, frm_tab_sale);
        #endregion

        #region Voucher Saving
        string sal_code = "", par_code = "", tax_code = "", tax_code2 = "", schg_code;
        par_code = txtlbl4.Text.Trim();


        string optwb = "";
        optwb = fgen.getOption(frm_qstr, frm_cocd, "W0100", "OPT_ENABLE");
        if (lbl27.Text.Substring(0, 2) == "CG")
        {

            if (optwb == "Y")
            {
                tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0077", "OPT_PARAM");
                sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0077", "OPT_PARAM2");
                tax_code2 = fgen.getOption(frm_qstr, frm_cocd, "W0078", "OPT_PARAM");
            }
            else
            {
                tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A77'", "PARAMS");
                sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A77'", "PARAMS2");
                tax_code2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A78'", "PARAMS");
            }
        }
        else
        {

            if (optwb == "Y")
            {
                tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0079", "OPT_PARAM");
                sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0079", "OPT_PARAM2");
            }
            else
            {
                tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A79'", "PARAMS");
                sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A79'", "PARAMS2");

            }
        }

        schg_code = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(params) as param from controls where id='A41'", "param");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");


        string rmToVSave = "Sale Inv.No " + frm_vnum;
        double billQty;
        billQty = 1;
        frm_vty = lbl1a.Text.Substring(0, 2);
        //working here
        fgen.vSave(frm_qstr, frm_cocd, newBranchcd, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, sal_code, fgen.make_double(txtlbl31.Text.Trim(), 2), 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, fgen.make_double(txtlbl31.Text.Trim(), 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
        int srn = 50;
        fgen.vSave(frm_qstr, frm_cocd, newBranchcd, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), srn, sal_code, par_code, 0, fgen.make_double(txtlbl25.Text, 2), frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, 0, fgen.make_double(txtlbl25.Text, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
        srn += 1;
        fgen.vSave(frm_qstr, frm_cocd, newBranchcd, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), srn, tax_code, par_code, 0, fgen.make_double(txtlbl27.Text, 2), frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, 0, fgen.make_double(txtlbl27.Text, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
        if (tax_code2.Length > 0)
        {
            srn += 1;
            fgen.vSave(frm_qstr, frm_cocd, newBranchcd, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), srn, tax_code2, par_code, 0, fgen.make_double(txtlbl29.Text, 2), frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, 0, fgen.make_double(txtlbl29.Text, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
        }
        if (frm_vty == "45" && fgen.make_double(txtTCSA.Text) > 0)
        {
            srn += 1;
            tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='D37'", "params");
            fgen.vSave(frm_qstr, frm_cocd, newBranchcd, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), srn, tax_code, par_code, 0, fgen.make_double(txtTCSA.Text, 2), frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, 0, fgen.make_double(txtTCSA.Text, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
        }
        #endregion

        #region Purchase Side Voucher Saving
        col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "ACODE");
        string frm_vnum1 = "";
        if (lbl27.Text.Substring(0, 2) == "CG")
        {
            tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A80'", "PARAMS");
            sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A80'", "PARAMS2");
            tax_code2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A81'", "PARAMS");
        }
        else
        {
            tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A82'", "PARAMS");
            sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A82'", "PARAMS2");
        }
        SQuery = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM VOUCHER WHERE BRANCHCD='" + newBranchcd + "' AND TYPE='50' AND " + doc_df.Value + " " + DateRange + " ";
        frm_vnum1 = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "VCH");

        fgen.vSave(frm_qstr, frm_cocd, newBranchcd, "50", frm_vnum1, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, sal_code, col3, sdTotalBasic, 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, sdTotalBasic, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
        fgen.vSave(frm_qstr, frm_cocd, newBranchcd, "50", frm_vnum1, Convert.ToDateTime(txtvchdate.Text.Trim()), 2, tax_code, col3, sdTotalCGST, 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, sdTotalCGST, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
        if (tax_code2.Length > 0)
        {
            fgen.vSave(frm_qstr, frm_cocd, newBranchcd, "50", frm_vnum1, Convert.ToDateTime(txtvchdate.Text.Trim()), 3, tax_code2, col3, sdTotalSGST, 0, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, sdTotalSGST, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
        }
        fgen.vSave(frm_qstr, frm_cocd, newBranchcd, "50", frm_vnum1, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, col3, sal_code, 0, sdTotalValue, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), rmToVSave, 0, 0, 1, 0, sdTotalValue, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), lbl27.Text.Substring(0, 2), 0, billQty, "", frm_tab_vchr);
        #endregion
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
        sg1_dt.Columns.Add(new DataColumn("sg1_t17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t20", typeof(string)));

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
        if (sg1_dt == null)
        { return; }
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
        sg1_dr["sg1_t17"] = "-";
        sg1_dr["sg1_t18"] = "-";
        sg1_dr["sg1_t19"] = "-";
        sg1_dr["sg1_t20"] = "-";

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
                }
            }

            setGST();
            if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
            {
                sg1.HeaderRow.Cells[24].Text = "CGST";
                sg1.HeaderRow.Cells[25].Text = "SGST/UTGST";
            }
            else
            {
                sg1.HeaderRow.Cells[24].Text = "IGST";
                sg1.HeaderRow.Cells[25].Text = "-";
            }

            if (doc_GST.Value == "GCC")
            {
                sg1.HeaderRow.Cells[24].Text = "VAT";
                sg1.HeaderRow.Cells[25].Text = "-";

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
            case "SG1_ROW_TAX":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_TAX";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                break;
            case "SG1_ROW_DT":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_DT";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Item", frm_qstr);

                    //fgen.Fn_open_dtbox("Select Date", frm_qstr);

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
                    hffield.Value = "sg4_RMV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                }
                break;
            case "SG4_ROW_ADD":
                dt = new DataTable();
                sg4_dt = new DataTable();
                dt = (DataTable)ViewState["sg4"];
                z = dt.Rows.Count - 1;
                sg4_dt = dt.Clone();
                sg4_dr = null;
                i = 0;
                for (i = 0; i < sg4.Rows.Count; i++)
                {
                    sg4_dr = sg4_dt.NewRow();
                    sg4_dr["sg4_srno"] = (i + 1);
                    sg4_dr["sg4_t1"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                    sg4_dr["sg4_t2"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
                    sg4_dt.Rows.Add(sg4_dr);
                }
                sg4_add_blankrows();
                ViewState["sg4"] = sg4_dt;
                sg4.DataSource = sg4_dt;
                sg4.DataBind();
                break;
        }
    }

    //------------------------------------------------------------------------------------

    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer ", frm_qstr);
    }
    protected void btnlbl10_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_10";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl10.Text, frm_qstr);
    }
    protected void btnlbl11_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_11";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl10.Text, frm_qstr);
    }
    protected void btnlbl12_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_12";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_13";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_14";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_15";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Mode of Transport, ex : By Road, By Air, By Ship etc ", frm_qstr);
    }
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_16";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Transporter ", frm_qstr);
    }
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_17";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Freight Terms ", frm_qstr);
    }
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_18";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Insurance ", frm_qstr);
    }
    protected void btnlbl19_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_19";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }



    //------------------------------------------------------------------------------------
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl70_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODEX";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type ", frm_qstr);
    }

    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");


        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Length > 2)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = lbl1a.Text.Substring(0, 2);
                oporow["vchnum"] = frm_vnum.Trim();
                oporow["vchdate"] = txtvchdate.Text.Trim();


                oporow["invno"] = frm_vnum.Trim();
                oporow["invdate"] = txtvchdate.Text.Trim();

                oporow["store"] = "Y";
                oporow["rec_iss"] = "C";

                oporow["acode"] = txtlbl4.Text.Trim();
                oporow["rcode"] = txtlbl4.Text.Trim();
                if (sdWorking)
                {
                    if (brCode != "")
                        oporow["acode"] = brCode;
                }
                oporow["morder"] = i + 1;
                oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim();
                oporow["purpose"] = sg1.Rows[i].Cells[14].Text.Trim();
                oporow["exc_57f4"] = sg1.Rows[i].Cells[15].Text.Trim();
                oporow["finvno"] = sg1.Rows[i].Cells[16].Text.Trim();
                txtlbl5.Text = sg1.Rows[i].Cells[16].Text.Trim();

                oporow["no_bdls"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                oporow["btchno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                oporow["iqtyout"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim());

                oporow["FABTYPE"] = txtlbl24.Text;
                oporow["ACPT_UD"] = fgen.make_double(txtlbl26.Text);

                oporow["irate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim()) * txtlbl26.Text.toDouble(4) > 0 ? Math.Round(fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim(), 4) * txtlbl26.Text.toDouble(4), 4) : 1;
                oporow["iqty_chlwt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim());
                oporow["ichgs"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim());

                oporow["iamount"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim());

                oporow["exc_Rate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim());
                oporow["exc_amt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim());

                oporow["cess_percent"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim());
                oporow["cess_pu"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim());

                if (hfCalcGST.Value != "Y")
                {
                    oporow["exc_Rate"] = 0;
                    oporow["exc_amt"] = 0;

                    oporow["cess_percent"] = 0;
                    oporow["cess_pu"] = 0;
                }

                oporow["desc_"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();


                oporow["iexc_addl"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim());
                oporow["idiamtr"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim());
                oporow["ipack"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim());


                oporow["ccent"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                oporow["revis_no"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                oporow["ponum"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();

                string po_dts;
                po_dts = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim(), vardate);

                oporow["podate"] = po_dts;


                oporow["tc_no"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                po_dts = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim(), vardate);
                oporow["refdate"] = po_dts;

                txtlbl6.Text = Convert.ToDateTime(po_dts).ToString("dd/MM/yyyy");

                oporow["iopr"] = lbl27.Text.Substring(0, 2);

                oporow["doc_tot"] = sg1.Rows[i].Cells[2].Text.toDouble();

                if (frm_formID == "F55106" && frm_cocd == "MULT")
                {
                    oporow["col1"] = 0;
                    oporow["col2"] = txtOldBalance.Text.toDouble();
                    oporow["col3"] = txtOthChrg.Text.toDouble();
                    oporow["col4"] = txtAdvRcvd.Text.toDouble();

                    oporow["col6"] = (chkOldBal.Checked) ? 1 : 0;
                    oporow["col7"] = (chkOthChrg.Checked) ? 1 : 0;
                    oporow["col8"] = (chkAdvRcvd.Checked) ? 1 : 0;
                }

                oporow["THRU"] = txtPayTerms.Text;

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
    }
    void save_fun2()
    {
        double Tot_Bill_qty = 0;
        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Length > 2)
            {
                Tot_Bill_qty = Tot_Bill_qty + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim());
            }
        }

        //string curr_dt;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow2 = oDS2.Tables[0].NewRow();
        oporow2["BRANCHCD"] = frm_mbr;
        oporow2["TYPE"] = lbl1a.Text;
        oporow2["vchnum"] = frm_vnum;
        oporow2["vchdate"] = txtvchdate.Text.Trim();

        oporow2["Acode"] = txtlbl4.Text;
        if (sdWorking)
        {
            if (brCode != "")
                oporow2["acode"] = brCode;
        }
        oporow2["cscode"] = txtlbl7.Text.Trim();

        oporow2["invtime"] = txtlbl2.Text;
        oporow2["pono"] = txtlbl5.Text;
        oporow2["podate"] = txtlbl6.Text;

        oporow2["destin"] = txtlbl8.Text;
        oporow2["st_entform"] = txtlbl9.Text;

        oporow2["mode_tpt"] = txtlbl15.Text;
        oporow2["ins_no"] = txtlbl16.Text;
        oporow2["freight"] = txtlbl17.Text;
        oporow2["insur_no"] = txtlbl18.Text;

        oporow2["mo_vehi"] = "-";
        oporow2["weight"] = txtlbl26.Text;
        oporow2["remvdate"] = fgen.make_def_Date(txtlbl28.Text.Trim(), vardate);
        oporow2["remvtime"] = txtlbl30.Text;
        oporow2["post"] = lbl27.Text.Substring(0, 1);

        oporow2[chkFOC.Checked == true ? "AMT_REA" : "AMT_SALE"] = fgen.make_double(txtlbl25.Text);

        if (hfCalcGST.Value == "Y")
        {
            oporow2["AMT_EXC"] = fgen.make_double(txtlbl27.Text);
            oporow2["RVALUE"] = fgen.make_double(txtlbl29.Text);
            oporow2["BILL_TOT"] = fgen.make_double(txtlbl31.Text);
        }
        else
        {
            oporow2["AMT_EXC"] = "0";
            oporow2["RVALUE"] = "0";
            oporow2["BILL_TOT"] = fgen.make_double(txtlbl31.Text) - (fgen.make_double(txtlbl27.Text) + fgen.make_double(txtlbl29.Text));
        }

        if (chkFOC.Checked == true)
        {
            if (hfCalcGST.Value == "Y")
            {
                oporow2["BILL_TOT"] = fgen.make_double(txtlbl27.Text) + fgen.make_double(txtlbl29.Text);
            }
            else oporow2["BILL_TOT"] = "0";
        }

        oporow2["BILL_qty"] = Tot_Bill_qty;

        oporow2["naration"] = txtrmk.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
        oporow2["eNt_by"] = frm_uname;
        oporow2["eNt_dt"] = vardate;

        oporow2["DRV_NAME"] = txtDrvName.Text.Trim();
        oporow2["drv_mobile"] = txtDrvMobile.Text.Trim();

        oporow2["tcsamt"] = fgen.make_double(txtTCSA.Text);

        oporow2["THRU"] = txtPayTerms.Text;

        if (frm_formID != "F55106")
        {
            oporow2["ACVDRT"] = txtCashDisc.Text.toDouble();
            oporow2["TOTDISC_AMT"] = txtCashDiscValue.Text.toDouble();
        }

        oporow2["GRNO"] = txtGrno.Text;
        oporow2["GRDATE"] = fgen.make_def_Date(txtGrDt.Text, vardate);

        oporow2["CURREN"] = txtlbl24.Text;

        oporow2["ins_cert"] = txtContTerms.Text;

        oDS2.Tables[0].Rows.Add(oporow2);
    }
    void save_fun3()
    {
        for (i = 0; i < sg2.Rows.Count - 0; i++)
        {
            if (((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim().Length > 1)
            {
                oporow3 = oDS3.Tables[0].NewRow();
                oporow3["BRANCHCD"] = frm_mbr;

                oporow3["TYPE"] = lbl1a.Text;
                oporow3["vchnum"] = frm_vnum;
                oporow3["vchdate"] = txtvchdate.Text.Trim();
                oporow3["SNO"] = i;
                oporow3["terms"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                oporow3["condi"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                oDS3.Tables[0].Rows.Add(oporow3);
            }
        }
    }
    void save_fun4()
    {
        for (i = 0; i < sg3.Rows.Count - 0; i++)
        {
            if (((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim().Length > 1)
            {
                oporow4 = oDS4.Tables[0].NewRow();
                oporow4["BRANCHCD"] = frm_mbr;

                oporow4["TYPE"] = lbl1a.Text;
                oporow4["vchnum"] = frm_vnum;
                oporow4["vchdate"] = txtvchdate.Text.Trim();
                oporow4["SRNO"] = i;
                oporow4["icode"] = sg3.Rows[i].Cells[3].Text.Trim();
                oporow4["dlv_Date"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.ToString();
                oporow4["budgetcost"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text);
                oporow4["actualcost"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text);
                oporow4["jobcardrqd"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text);
                oDS4.Tables[0].Rows.Add(oporow4);
            }
        }
    }

    void save_fun5()
    {
        for (i = 0; i < sg4.Rows.Count - 0; i++)
        {
            if (((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim().Length > 0 && ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim() != "-")
            {
                oporow5 = oDS5.Tables[0].NewRow();
                oporow5["branchcd"] = frm_mbr;
                oporow5["par_tbl"] = frm_tab_ivch.ToUpper().Trim();
                oporow5["par_fld"] = frm_mbr + lbl1a.Text + frm_vnum + txtvchdate.Text.Trim();
                oporow5["udf_name"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim() == "-" ? "" : ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                oporow5["udf_value"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim() == "-" ? "" : ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
                oporow5["SRNO"] = i;

                oDS5.Tables[0].Rows.Add(oporow5);
            }
        }
    }

    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE,Acode AS Acctg_code FROM type where id='V' and type1 like '4%' and type1 in ('4F','4T') order by type1";


    }
    //------------------------------------------------------------------------------------   
    void setGST()
    {
        lbl25.Text = "Taxbl_Total";
        lbl31.Text = "Grand_Total";
        if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
        {
            lbl27.Text = "CGST";
            lbl29.Text = "SGST/UTGST";
        }
        else
        {
            lbl27.Text = "IGST";
            lbl29.Text = "";
        }
        if (doc_GST.Value == "GCC")
        {
            lbl27.Text = "VAT";
            lbl29.Text = "";
        }

    }
    string getDiscountedRate(string ticode, string currRate, string tax)
    {
        DataTable dtdisc = new DataTable();
        if (ViewState["dtItemSub"] == null)
        {
            dtdisc = fgen.getdata(frm_qstr, frm_cocd, "SELECT TRIM(ICODE) AS ICODE,num1 as irate,num2 AS IRATE2 FROM SCRATCH2 WHERE BRANCHCD!='DD' AND TYPE='DS' ORDER BY ICODE ");
            ViewState["dtItemSub"] = dtdisc;
        }
        else
        {
            dtdisc = (DataTable)ViewState["dtItemSub"];
        }
        string rate = currRate;
        string rateDiscount = fgen.seek_iname_dt(dtdisc, "ICODE='" + ticode + "' ", "IRATE2");
        if (rateDiscount.toDouble() > 0)
        {
            rate = ((rate.toDouble() - (rate.toDouble() * (rateDiscount.toDouble() / 100))).toDouble(2)).ToString();
        }
        return rate;
    }
    void sendMail()
    {
        DataSet dsRep = new DataSet();
        SQuery = "select distinct A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,c.acvdrt,a.doc_tot from ivoucher a,sale c,item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + ") order by vchdate,a.vchnum,a.MORDER";
        string frm_rptName = "std_inv_std";
        if (frm_cocd == "SAIA") frm_rptName = "std_inv_saia";
        string mq10 = "";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            dt.Columns.Add(new DataColumn("amtToword", typeof(string)));
            dt.Columns.Add(new DataColumn("PkgN", typeof(double)));
            foreach (DataRow dr in dt.Rows)
            {
                dr["pkgN"] = fgen.make_double(fgen.getNumericOnly(dr["pkg"].ToString()));

                dr["amtToword"] = fgen.ConvertNumbertoWords(dr["bill_tot"].ToString().Trim());
            }

            dt.TableName = "Prepcur";
            int repCount = 4;
            dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));

            DataTable dt1 = new DataTable("barcode");
            dt1.Columns.Add(new DataColumn("img1_desc", typeof(string)));
            dt1.Columns.Add(new DataColumn("img1", typeof(System.Byte[])));
            string col2 = "";
            mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no from type where id='B' and type1='" + dt.Rows[0]["branchcd"].ToString().Trim().Replace("/", "") + "'", "gst_no");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                col1 = dt.Rows[i]["branchcd"].ToString().Trim().Replace("/", "") + "," + dt.Rows[i]["vchnum"].ToString().Trim().Replace("/", "");
            }
            string fpath = Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "") + ".png");
            del_file(fpath);
            if (frm_cocd == "PPAP") fgen.prnt_QRbar(frm_cocd, col2, col1.Replace("*", "").Replace("/", "") + ".png");
            else fgen.prnt_QRbar(frm_cocd, col1, col1.Replace("*", "").Replace("/", "") + ".png");

            DataRow drBcode = dt1.NewRow();
            FileStream FilStr = new FileStream(fpath, FileMode.Open);
            BinaryReader BinRed = new BinaryReader(FilStr);

            drBcode["img1_desc"] = col1.Trim();
            drBcode["img1"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);

            dt1.Rows.Add(drBcode);
            FilStr.Close();
            BinRed.Close();

            dsRep.Tables.Add(dt1);

            //csmst                
            SQuery = "Select distinct d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dt.Rows[0]["cscode"].ToString().Trim() + "'";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count <= 0)
            {
                dt = new DataTable();
                SQuery = "Select 'Same as Recipient' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            }
            dt.TableName = "csmst";
            dsRep.Tables.Add(dt);

            // inv terms
            SQuery = "SELECT DISTINCT COL1 AS POTERMS,SRNO FROM DOCTERMS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' AND DOCTYPE='INV' ORDER BY SRNO";

            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

            dt1 = new DataTable();
            DataRow mdr = null;
            dt1.Columns.Add("poterms", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                mq10 += dt.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
            }
            mdr = dt1.NewRow();
            mdr["poterms"] = mq10;
            dt1.Rows.Add(mdr);
            if (dt1.Rows.Count > 0)
            {
                dt1.TableName = "INV_TERMS";
                dsRep.Tables.Add(dt1);
                Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv", frm_rptName, dsRep, "Invoice Entry Report", "Y");

                string subj = "Invoice No. " + frm_vnum;
                html_body();
                Attachment atchfile = new Attachment(repDoc.ExportToStream(ExportFormatType.PortableDocFormat), frm_cocd + "_" + subj.Replace(" ", "_") + ".pdf");
                if (frm_cocd == "SAIA")
                {
                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT EMAIL FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "EMAIL");
                    if (col1 != "0")
                        fgen.send_mail(frm_qstr, frm_cocd, "Tejaxo ERP", col1, "", "", subj, xhtml_tag, atchfile, "1");

                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT EMAIL FROM FAMST WHERE ACODE='" + txtlbl4.Text.Trim().ToUpper() + "'", "EMAIL");
                    if (col1 != "0")
                        fgen.send_mail(frm_qstr, frm_cocd, "Tejaxo ERP", col1, "", "", subj, xhtml_tag, atchfile, "1");
                }
            }
        }
    }
    public void html_body()
    {
        xhtml_tag = xhtml_tag + "<h4><B> Respected Sir, </B></h4><br>";
        xhtml_tag = xhtml_tag + "Please find the attached file of shipment<br><br>";


        xhtml_tag = xhtml_tag + "<br><br><b>Thanks & Regards,</b>";
        xhtml_tag = xhtml_tag + "<br><b>" + fgenCO.chk_co(frm_cocd) + "</b>";
        xhtml_tag = xhtml_tag + "<br><br><br>Note: This is an automatically generated email from Tejaxo ERP, Please do not reply";
        xhtml_tag = xhtml_tag + "</body></html>";
    }
    public void Print_Report_BYDS(string co_Cd, string mbr, string xml, string report, DataSet data_set, string title, string addlogo)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/Report/" + report.Trim() + ".rpt";

        if (addlogo == "Y") data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr, "Y"));
        else data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr));

        data_set.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
        if (data_set.Tables[0].Rows.Count > 0)
        {
            CrystalReportViewer1.ReportSource = GetReportDocument(data_set, rptfile);
            CrystalReportViewer1.DataBind();
        }
        else
        {
        }
        data_set.Dispose();
    }
    private ReportDocument GetReportDocument(DataSet rptDS, string rptFileName)
    {
        string repFilePath = Server.MapPath("" + rptFileName + "");
        repDoc = new ReportDocument();
        repDoc.Load(repFilePath);
        repDoc.Refresh();
        repDoc.SetDataSource(rptDS);
        return repDoc;
    }
    void Report_Default_Unload(object sender, EventArgs e)
    {
        try
        {
            repDoc.Close();
            repDoc.Dispose();
        }
        catch { }
    }

    protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
    {
        try
        {
            repDoc.Close();
            repDoc.Dispose();
        }
        catch { }
    }
    public void del_file(string path)
    {
        try
        {
            string fpath = Server.MapPath(path);
            if (System.IO.File.Exists(fpath)) System.IO.File.Delete(fpath);
        }
        catch { }
    }
    bool checkControlsLinkedCorrectly()
    {
        string optwb = "";
        optwb = fgen.getOption(frm_qstr, frm_cocd, "W0100", "OPT_ENABLE");
        if (optwb == "Y")
        {
            col1 = fgen.getOption(frm_qstr, frm_cocd, "W0077", "OPT_PARAM");
            if (col1 == "0")
            {
                fgen.msg("-", "AMSG", "Sales Tax Control Not Linked Correctly, Check Control No. W0077");
                return false;
            }
            col1 = fgen.getOption(frm_qstr, frm_cocd, "W0078", "OPT_PARAM");
            if (col1 == "0")
            {
                fgen.msg("-", "AMSG", "Sales Tax Control Not Linked Correctly, Check Control No. W0077");
                return false;
            }
        }
        else
        {
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A77'", "PARAMS");
            if (col1 == "0")
            {
                fgen.msg("-", "AMSG", "Sales GST Control Not Linked Correctly, Check Control No. A77, A78, A79");
                return false;
            }
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A80'", "PARAMS");
            if (col1 == "0")
            {
                fgen.msg("-", "AMSG", "Purchase GST Control Not Linked Correctly, Check Control No. A80, A81, A82");
                return false;
            }
        }
        return true;
    }
}