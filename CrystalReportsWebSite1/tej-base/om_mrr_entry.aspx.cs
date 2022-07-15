using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_mrr_entry2 : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond, gate_link = "";
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, continueNumberSer = "N";
    string cntrlFullName = "", frm_IndType, itemCond = "";
    int kclreelno = 0;
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
            hfcocd.Value = frm_cocd;
            if (!Page.IsPostBack)
            {
                //doc_addl.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select (case when nvl(st_Sc,1)=0 then 1 else nvl(st_Sc,1) end )  as add_tx from type where id='B' and trim(upper(type1))=upper(Trim('" + frm_mbr + "'))", "add_tx");                
                string chk_opt;
                gate_link = fgen.seek_iname(frm_qstr, frm_cocd, "select enable_yn from controls where id='M52'", "enable_yn");
                if (frm_cocd == "MINV") gate_link = "Y";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_GL", gate_link);
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_MAX_MRR", "0");
                col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select PARAMS from controls where id='M50'", "PARAMS");
                if (col1.toDouble() > 0)
                {
                    chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select ENABLE_YN from STOCK where id='M035'", "ENABLE_YN");
                    if (chk_opt == "Y")
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_MAX_MRR", col1);
                }
                //chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select enable_yn from controls where id='S02'", "enable_yn");
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0051'", "fstr");
                if (chk_opt == "Y")
                {
                    continueNumberSer = "Y";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CONT", continueNumberSer);
                }
                doc_addl.Value = "-";
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0001'", "fstr");
                if (chk_opt != "Y")
                {
                    tab3.Visible = false;
                }
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0002'", "fstr");
                if (chk_opt != "Y")
                {
                    txtBarCode.Visible = false;
                    btnRead.Visible = false;
                }

                doc_GST.Value = "Y";
                //GSt india

                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2017'", "fstr");
                if (chk_opt == "N")
                {
                    doc_GST.Value = "N";

                }
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2027'", "fstr");
                if (chk_opt == "Y")
                //Member GCC Country
                {
                    doc_GST.Value = "GCC";
                }

                if (fgen.seek_iname(frm_qstr, frm_cocd, "SELECT trim(UPPER(NAME)) AS NAME FROM TYPE WHERE ID='M' AND TYPE1='07' ", "NAME") != "MRR:IMPORTED MATL")
                {
                    fgen.execute_cmd(frm_qstr, frm_cocd, "update type set name='MRR:Imported Matl' where id='M' and type1='07'");
                }
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
            btnlbl17.Visible = false;
            btnlbl18.Visible = false;
        }
        //txtTax.Text = "Y";
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
                ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Add("autocomplete", "off");

                txtlbl70.Attributes.Add("readonly", "readonly");
                txtlbl71.Attributes.Add("readonly", "readonly");
                txtlbl72.Attributes.Add("readonly", "readonly");
                txtlbl73.Attributes.Add("readonly", "readonly");

                ((TextBox)sg1.Rows[K].FindControl("sg1_t7")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t8")).Attributes.Add("readonly", "readonly");

                if (lbl1a.Text != "TC")
                {
                    if (lbl1a.Text == "05")
                    {
                        ((TextBox)sg1.Rows[K].FindControl("sg1_t5")).Attributes.Remove("readonly");
                    }
                    else
                    {
                        if (frm_cocd != "MULT") ((TextBox)sg1.Rows[K].FindControl("sg1_t5")).Attributes.Add("readonly", "readonly");
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
        if (frm_cocd == "MASS" || frm_cocd == "MAST")
        {
            if (sg2.Rows.Count <= 0) return;
            if (fgen.seek_iname(frm_qstr, frm_cocd, "select opt_enable from fin_Rsys_opt_pw where vchnum = '002033'", "") == "Y")
            {
                sg2.HeaderRow.Cells[8].Text = "Our Batch No";
                sg2.HeaderRow.Cells[13].Text = "Mill Batch";
                tab3.InnerText = "Batch/Lot Dtl";
            }
        }
        txtlbl25.Attributes.Add("readonly", "readonly");
        txtlbl27.Attributes.Add("readonly", "readonly");
        txtlbl29.Attributes.Add("readonly", "readonly");
        txtlbl31.Attributes.Add("readonly", "readonly");

        // to hide and show to tab panel        

        fgen.SetHeadingCtrl(this.Controls, dtCol);
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F25101":
                //tab2.Visible = false;
                //tab3.Visible = false;
                tab4.Visible = false;
                tab5.Visible = false;
                gate_link = fgenMV.Fn_Get_Mvar(frm_qstr, "U_GL");
                if (gate_link == "N")
                {
                    txtlbl2.Attributes.Remove("readonly");
                    txtlbl3.Attributes.Remove("readonly");
                    txtlbl5.Attributes.Remove("readonly");
                    txtlbl6.Attributes.Remove("readonly");
                    txtlbl8.Attributes.Remove("readonly");
                    txtlbl9.Attributes.Remove("readonly");
                }
                break;
        }

        if (lbl1a.Text.Trim() == "0J" || lbl1a.Text.Trim() == "09")
        {
            foreach (GridViewRow gr in sg1.Rows)
            {
                ((TextBox)gr.FindControl("sg1_t19")).Attributes.Remove("readonly");
                ((TextBox)gr.FindControl("sg1_t20")).Attributes.Remove("readonly");
            }
        }
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

        btnprint.Disabled = false; btnlist.Disabled = false; btnSticker.Disabled = false; //BY MADHVI ON 28 JULY 2018
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlbl4.Enabled = true;
        btnlbl7.Enabled = true;
        btnprint.Disabled = true; btnlist.Disabled = true; btnSticker.Disabled = true; //BY MADHVI ON 28 JULY 2018
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
            case "F25101":
                frm_tabname = "ivoucher";
                break;
        }
        // can introduce new ctrl panel 
        if (frm_cocd == "SRPF" || frm_cocd == "SVPL")
            continueNumberSer = "Y";

        continueNumberSer = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CONT");

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);

    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        gate_link = fgenMV.Fn_Get_Mvar(frm_qstr, "U_GL");
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
                SQuery = "Select Type1 as fstr,Name,type1 from type where id='A' order by name";
                break;
            case "BTN_16":
                SQuery = "select * from (select Acode,ANAME as Transporter,Acode as Code,Addr1 as Address,Addr2 as City from famst  where upper(ccode)='T' union all select 'Own' as Acode,'OWN' as Transporter,'-' as Code,'-' as Address,'-' as City from dual union all select '-' as acode,'PARTY VEHICLE' as Transporter,'-' as Code,'-' as Address,'-' as City from dual) order by  Transporter";
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
                if (gate_link == "Y")
                {
                    string prcond;
                    prcond = "-";
                    switch (lbl1a.Text)
                    {
                        case "02":
                        case "03":
                        case "07":
                            prcond = "PO";
                            break;
                        case "06":
                            prcond = "'OT'";
                            break;
                        case "05":
                            prcond = "OT";
                            break;
                        case "09":
                        case "0J":
                            prcond = "RG";
                            break;
                        case "04":
                            prcond = "BI";
                            break;
                        case "08":
                            prcond = "JO";
                            break;
                        case "0U":
                            prcond = "CH";
                            break;

                    }

                    if (frm_cocd == "SVPL")
                    {
                        if (lbl1a.Text == "01") prcond = "BI";
                        if (lbl1a.Text == "06") prcond = "JO";
                    }

                    if (!prcond.Contains("'")) prcond = "'" + prcond + "'";
                    SQuery = "SELECT DISTINCT A.vchnum||to_char(A.vchdate,'dd/mm/yyyy')||trim(a.Acode) as Fstr,A.VCHNUM AS GE_Number,to_Char(a.VCHDATE,'dd/mm/yyyy') as Ge_Date,B.Aname as Supplier,a.Invno as Inv_no,A.Refnum as Chl_no,a.Acode,b.Staten,upper(a.prnum) as prnum,to_Char(A.Invdate,'dd/mm/yyyy') as Inv_Dt,to_Char(a.vchdate,'yyyymmdd') as GE_Dt FROM IVOUCHERP a, famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.BRANCHCD='" + frm_mbr + "' AND a.VCHDATE  " + DateRange + " AND a.TYPE='00' and upper(a.prnum) in (" + prcond + ") AND (a.VCHNUM||to_char(a.vchdate,'yyyymm')) IN (SELECT VCHNUM FROM (SELECT X.VCHNUM,SUM(X.aBC) AS CNT FROM (select distinct a.vchnum||to_char(a.vchdate,'yyyymm') as vchnum,a.type,1 AS ABC from ivoucherp a  where branchcd='" + frm_mbr + "' and a.VCHDATE   " + DateRange + " AND a.type='00' and a.vchnum<>'000000' UNION ALL select distinct a.GENUM||to_char(a.gedate,'yyyymm') as genum,a.type,1 AS ABC from ivoucher a where branchcd='" + frm_mbr + "' and substr(a.type,1,1)='0' and a.VCHDATE  " + DateRange + " AND a.vchnum<>'000000' ) X GROUP BY X.VCHNUM) WHERE CNT=1) order by to_Char(a.vchdate,'yyyymmdd') desc,A.VCHNUM desc ";
                }
                else
                {
                    SQuery = "SELECT distinct a.Acode as FStr,a.Aname as Customer,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.Staten from famst a where  length(trim(nvl(a.deac_by,'-'))) <2 and substr(a.acode,1,2) in ('02','05','06','14','15') order by a.Aname ";

                    switch (lbl1a.Text)
                    {
                        case "02":
                        case "03":
                        case "07":
                            SQuery = "SELECT distinct a.Acode as FStr,b.Aname as Supplier,b.Acode,b.Addr1,b.Addr2,b.GST_No,b.Staten from pomas a,famst b where a.branchcd='" + frm_mbr + "' and a.type like '5%' and (trim(a.chk_by)!='-' or trim(a.app_by)!='-') and a.pflag!=1 and trim(A.acodE)=trim(B.acode) and length(trim(nvl(b.deac_by,'-'))) <2 order by b.Aname ";
                            break;
                        case "04":
                            SQuery = "SELECT distinct a.Acode as FStr,b.Aname as Customer,b.Acode,b.Addr1,b.Addr2,b.GST_No,b.Staten from Somas a,famst b where a.branchcd='" + frm_mbr + "' and a.type like '4%' and trim(nvl(a.app_by,'-'))!='-' and trim(A.acodE)=trim(B.acode) and length(trim(nvl(b.deac_by,'-'))) <2 order by b.Aname ";
                            break;
                        case "05":
                        case "06":
                            SQuery = "SELECT distinct a.Acode as FStr,a.Aname as Customer,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.Staten from famst a where  length(trim(nvl(a.deac_by,'-'))) <2 and substr(a.acode,1,2) in ('02','05','06','14','15')  order by a.Aname ";
                            break;
                        case "09":
                        case "0J":
                            SQuery = "SELECT distinct a.Acode as FStr,b.Aname as Supplier,b.Acode,b.Addr1,b.Addr2,b.GST_No,b.Staten from ivoucher a,famst b where a.branchcd='" + frm_mbr + "' and a.type like '2%' and trim(A.acodE)=trim(B.acode) and length(trim(nvl(b.deac_by,'-'))) <2 order by b.Aname ";
                            break;
                        case "08":
                            SQuery = "SELECT distinct a.Acode as FStr,b.Aname as Customer,b.Acode,b.Addr1,b.Addr2,b.GST_No,b.Staten from Somas a,famst b where a.branchcd='" + frm_mbr + "' and a.type like '4%' and trim(nvl(a.app_by,'-'))!='-' and trim(A.acodE)=trim(B.acode) and length(trim(nvl(b.deac_by,'-'))) <2 order by b.Aname ";
                            break;
                        case "0U":
                            SQuery = "SELECT distinct a.Acode as FStr,a.Aname as Customer,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.Staten from famst a where length(trim(nvl(a.deac_by,'-'))) <2 and substr(a.acode,1,2) in ('02')  order by a.Aname ";
                            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "sELECT ACODE FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "ACODE");
                            SQuery = "select trim(a.Fstr) as fstr,a.acode as party_code,b.aname as party,a.vchnum as chl,A.vchdate as chldt ,(a.Qtyord)-(a.Soldqty) as Balance_Qty,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty from (select fstr,acode,vchnum,vchdate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(b.ACODE)||'-'||to_ChaR(a.vchdate,'YYYYMMDD')||'-'||trim(a.vchnum) as fstr,trim(b.ACODE) as acode,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.iqtyout as Qtyord,0 as Soldqty from ivoucher a,type b where trim(a.branchcd)=trim(b.type1) and b.id='B' and a.branchcd!='" + frm_mbr + "' and a.type in ('29') and a.vchdate>=to_Date('01/04/2017','dd/mm/yyyy') union all SELECT trim(acode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||lpad(trim(refnum),6,'0') as fstr,trim(Acode) as acode,lpad(trim(refnum),6,'0') as vchnum,to_char(refdate,'dd/mm/yyyy') as vchdate,0 as Qtyord,iqty_chl as qtyord from ivoucherp where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') )  group by fstr,acode,vchnum,vchdate having sum(Qtyord)-sum(Soldqty)>0  ) a,famst b where trim(A.acode)=trim(b.acodE) order by a.vchnum,trim(a.fstr)";
                            break;
                    }
                }
                break;
            case "TICODE":
                SQuery = "SELECT distinct a.Acode as FStr,a.Aname as Customer,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.Staten from famst a where  length(trim(nvl(a.deac_by,'-'))) <2 and substr(a.acode,1,2) in ('02','05','06','14','15') order by a.Aname ";
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

                if (col1.Length <= 0) col1 = "'-'";
                SQuery = "select a.Fstr,max(a.Iname)as Item_Name,a.ERP_code,max(a.Cpartno)as Part_no,max(a.Irate) As Irate,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as PO_No,a.Fstr as SO_link,max(a.cdisc) as CDisc,max(a.iexc_Addl) as iexc_Addl,max(a.sd) as frt_pu,max(a.ipack) as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,pordno,(Case when length(trim(nvl(desc9,'-')))>1 then desc9 else ciname end) as Iname,trim(Icode) as ERP_code,Cpartno,Irate,Qtyord,0 as Soldqty,nvl(Cdisc,0) as Cdisc,nvl(iexc_addl,0) as Iexc_Addl,nvl(sd,0) as sd ,nvl(ipack,0) as ipack from somas where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "' and trim(icat)!='Y' and trim(app_by)!='-'  union all SELECT to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(Icode) as fstr,null as pordno,null as Iname,trim(Icode) as ERP_code,null as Cpartno,0 as Irate,0 as Qtyord,iqtyout as Soldqty,0 as Cdisc,0 as iexc_Addl,0 as sd,0 as ipack  from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "')a,item b where trim(a.erp_code)=trim(B.icode)  group by a.fstr,a.ERP_code,b.unit,b.hscode having (case when sum(a.Qtyord)>0 then sum(a.Qtyord)-sum(a.Soldqty) else max(a.irate) end)>0 order by Item_Name,a.fstr";
                if (gate_link == "N")
                {
                    SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode as ERP_code,a.Maker,a.cpartno as Part_no,a.cdrgno,a.unit,a.hscode,null as btchno,null as btchdt,a.irate,'-' as po_no,'-' as po_dt,0 as Ord_Qty,0 as Balance_Qty from item a where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 order by a.Iname ";
                    switch (lbl1a.Text)
                    {
                        case "02":
                        case "03":
                            SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Maker,b.Cpartno as Part_no,a.Prate as irate,(a.Qtyord)-(a.Soldqty) as Balance_Qty,b.Cdrgno,b.Unit,b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,null as btchno,null as btchdt,substr(a.fstr,19,6) as po_no,substr(a.fstr,16,2)||'/'||substr(a.fstr,14,2)||'/'||substr(a.fstr,10,4) as po_dt from (select fstr,ERP_code,max(prate) as prate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate from pomas where branchcd='" + frm_mbr + "' and type like '5%' and type!='54' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl4.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,IQTYIN as qtyord,0 as irate from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl4.Text.Trim() + "')  group by fstr,ERP_code having (case when sum(Qtyord)>0 then sum(Qtyord)-sum(Soldqty) else max(prate) end)>0  )a,item b where trim(a.erp_code)=trim(B.icode)  order by B.Iname,trim(a.fstr)";
                            break;
                        case "07":
                            SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Maker,b.Cpartno as Part_no,a.Prate as irate,(a.Qtyord)-(a.Soldqty) as Balance_Qty,b.Cdrgno,b.Unit,b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,null as btchno,null as btchdt,substr(a.fstr,19,6) as po_no,substr(a.fstr,16,2)||'/'||substr(a.fstr,14,2)||'/'||substr(a.fstr,10,4) as po_dt from (select fstr,ERP_code,max(prate) as prate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate from pomas where branchcd='" + frm_mbr + "' and type like '5%' and type='54' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl4.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,IQTYIN as qtyord,0 as irate from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl4.Text.Trim() + "')  group by fstr,ERP_code having (case when sum(Qtyord)>0 then sum(Qtyord)-sum(Soldqty) else max(prate) end)>0  )a,item b where trim(a.erp_code)=trim(B.icode)  order by B.Iname,trim(a.fstr)";
                            break;
                        case "04":
                            SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Cpartno as Part_no,a.irate,b.Cdrgno,(a.Qtyord)-(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,null as btchno,null as btchdt,substr(a.fstr,19,6) as po_no,substr(a.fstr,16,2)||'/'||substr(a.fstr,14,2)||'/'||substr(a.fstr,10,4) as po_dt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(to_Char(srno,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((irate*(100-cdisc)/100)-0) as irate from somas where branchcd='" + frm_mbr + "' and type like '4%' and trim(app_by)!='-' and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl4.Text.Trim() + "' UNION ALL SELECT trim(icode)||'-'||to_ChaR(PODATE,'YYYYMMDD')||'-'||PONUM||'-'||lpad(trim(to_Char(MORDER,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,IQTYOUT,0 as Soldqty,((irate*(100-ICHGS)/100)-0) as irate from IVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and VCHDATE>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl4.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,IQTYIN as qtyord,0 as irate from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode)  order by B.Iname,trim(a.fstr)";
                            break;
                        case "05":
                        case "06":
                            SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode as ERP_code,a.Maker,a.cpartno as Part_no,a.cdrgno,a.unit,a.hscode,null as btchno,null as btchdt,a.irate,'-' as po_no,'-' as po_dt,0 as Ord_Qty,0 as Balance_Qty from item a where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 order by a.Iname ";
                            break;
                        case "09":
                        case "0J":
                            SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Maker,b.Cpartno as Part_no,a.irate,b.Cdrgno,(a.Qtyord)-(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,null as btchno,null as btchdt,substr(a.fstr,19,6) as po_no,substr(a.fstr,16,2)||'/'||substr(a.fstr,14,2)||'/'||substr(a.fstr,10,4) as po_dt from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate from rgpmst where branchcd='" + frm_mbr + "' and type in ('21','23','26') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl4.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,IQTYIN as qtyord,0 as irate from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl4.Text.Trim() + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode)  order by B.Iname,trim(a.fstr)";
                            break;
                        case "08":
                            //SQuery = "SELECT distinct a.ordno||to_char(a.orddt,'dd/mm/yyyy')||trim(a.Icode) as FStr,b.Iname as Item_Name,a.Ordno,to_char(A.Orddt,'dd/mm/yyyy') as Ord_dtd,a.Qtyord,b.Cpartno,b.Cdrgno,b.Icode,b.Unit from somas a,item b where a.branchcd='" + frm_mbr + "' and a.type like '41%' and trim(nvl(a.app_by,'-'))!='-'  and trim(A.IcodE)=trim(B.Icode) and length(trim(nvl(b.deac_by,'-'))) <2 and trim(a.acode)='" + txtlbl7.Text + "' order by b.Iname ";
                            SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode as erp_code,a.Maker,a.cpartno as Part_no,a.cdrgno,a.unit,a.hscode,a.irate,'-' as po_no,'-' as po_dt,0 as Ord_Qty,0 as Balance_Qty from item a where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 order by a.Iname ";
                            break;
                        case "0U":
                            //SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit from item a where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 order by a.Iname ";
                            SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Maker,b.Cpartno as Part_no,a.irate,b.Cdrgno,(a.Qtyord)-(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Balance_Qty,a.btchno,a.btchdt from (select fstr,ERP_code,btchno,btchdt,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate,btchno,btchdt from ivoucher where branchcd!='" + frm_mbr + "' and type in ('29') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl4.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,IQTYIN as qtyord,0 as irate,btchno,btchdt from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl4.Text.Trim() + "')  group by fstr,ERP_code,btchno,btchdt having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode)  order by B.Iname,trim(a.fstr)";
                            SQuery = "select trim(a.Fstr) as fstr,trim(a.Fstr) as f1,(a.Qtyord)-(a.Soldqty) as Balance_Qty,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Balance_Qty,a.irate,substr(a.fstr,19,6) as po_no,substr(a.fstr,16,2)||'/'||substr(a.fstr,14,2)||'/'||substr(a.fstr,10,4) as po_dt from (select fstr,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(acode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||trim(vchnum) as fstr,iqtyout as Qtyord,0 as Soldqty, IRATE from ivoucher where branchcd!='" + frm_mbr + "' and type in ('29') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') union all SELECT trim(acode)||'-'||to_ChaR(refdate,'YYYYMMDD')||'-'||trim(refnum) as fstr,0 as Qtyord,IQTYIN as qtyord, 0 AS IRATE from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy'))  group by fstr having sum(Qtyord)-sum(Soldqty)>0  )a order by trim(a.fstr)";
                            break;
                    }
                }
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);
                break;
            case "SG1_ROW_TAX":
                SQuery = "Select Type1 as fstr,Name,Type1 as Code,nvl(Rate,0) as Rate,nvl(Excrate,0) as Schg,exc_Addr as Ref_Code from type where id='S' and length(Trim(nvl(cstno,'-')))<=1 order by name";
                break;
            case "New":
            case "Edit":
            case "Del":
            case "Print":
            case "SPrint":
                Type_Sel_query();
                break;
            case "sg1_t9":
                SQuery = "SELECT TYPE1,NAME,TYPE1 AS CODE FROM TYPE WHERE ID='Q' ORDER BY TYPE1";
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "SPrint_E")
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as MRR_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as MRR_Dt,b.Aname as Vendor,a.Invno,to_char(a.invdate,'dd/mm/yyyy') as Inv_Dt,A.refnum as Chl_no,to_char(a.refdate,'dd/mm/yyyy') as chl_Dt,a.Genum as GE_No,to_char(a.gedate,'dd/mm/yyyy') as GE_Dt,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.pname as insp_by,a.finvno as vch_ref,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd') as barcode,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a,famst b where  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " and  trim(a.acode)=trim(B.acodE) and a.store!='R' order by vdd desc,a." + doc_nf.Value + " desc";
                else if (btnval == "Sprint_E*")
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as MRR_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as MRR_Dt,b.Aname as Vendor,c.iname,a.icode as erpcode,a.Invno,to_char(a.invdate,'dd/mm/yyyy') as Inv_Dt,A.refnum as Chl_no,to_char(a.refdate,'dd/mm/yyyy') as chl_Dt,a.Genum as GE_No,to_char(a.gedate,'dd/mm/yyyy') as GE_Dt,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.pname as insp_by,a.finvno as vch_ref,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd') as barcode,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a,famst b where  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " and  trim(a.acode)=trim(B.acodE) and a.store!='R' order by vdd desc,a." + doc_nf.Value + " desc";
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
            hffield.Value = "New";
            make_qry_4_popup();
            fgen.Fn_open_sseek("select type", frm_qstr);

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
        string err_item = "", err_msg = "";
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
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }
        if ((Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt)) && frm_cocd != "KRSM")
        { fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only"); txtvchdate.Focus(); return; }

        string chk_freeze = "";
        chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1031", txtvchdate.Text.Trim());
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

        getColHeading();

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;

        //txtlbl28.Text = txtlbl28.Text.Trim().ToUpper();
        //if (txtlbl28.Text == "Y" || txtlbl28.Text == "X" || txtlbl28.Text == "N")
        //{

        //}
        //else
        //{
        //    reqd_nc = reqd_nc + 1;
        //    reqd_flds = reqd_flds + " / " + lbl28.Text;
        //}

        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }


        string chk_dupl;
        if (txtlbl2.Text != "-" && frm_cocd != "KRSM")
        {
            if (edmode.Value == "Y")
            {
                chk_dupl = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum||'-'||to_char(vchdate,'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + DateRange + " and vchnum||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + orig_vchdt + "' and trim(upper(acode))='" + txtlbl4.Text + "' and trim(upper(genum))='" + txtlbl2.Text + "'", "ldt");
            }
            else
            {
                chk_dupl = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum||'-'||to_char(vchdate,'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + DateRange + "  and trim(upper(acode))='" + txtlbl4.Text + "' and trim(upper(genum))='" + txtlbl2.Text + "'", "ldt");
            }

            if (chk_dupl == "0")
            { }
            else
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , This Gate Entry Already Entered in " + lbl1.Text + "  " + chk_dupl + ",Please Check !!");
                return;

            }
        }
        if (txtlbl5.Text != "-")
        {
            if (edmode.Value == "Y")
            {
                chk_dupl = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum||'-'||to_char(vchdate,'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + DateRange + " and vchnum||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + orig_vchdt + "' and trim(upper(acode))='" + txtlbl4.Text + "' and trim(upper(invno))='" + txtlbl5.Text + "'", "ldt");
            }
            else
            {
                chk_dupl = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum||'-'||to_char(vchdate,'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + DateRange + " and trim(upper(acode))='" + txtlbl4.Text + "' and trim(upper(invno))='" + txtlbl5.Text + "'", "ldt");
            }

            if (chk_dupl == "0")
            { }
            else
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , This Invoice No. Already Entered in " + lbl1.Text + "  " + chk_dupl + ",Please Check !!");
                return;

            }
        }

        //---------------------
        if (txtlbl4.Text.Trim().Length < 2)
        {
            Checked_ok = "N";
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Supplier Not Filled Correctly !!");
            return;
        }

        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) <= 0)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");
                i = sg1.Rows.Count;
                return;
            }
        }

        //--------------------

        string last_entdt;
        if (frm_cocd == "KRSM")
        {
            last_entdt = "0";
        }
        else
        {
            if (edmode.Value == "Y")
            {
                last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  and vchdate " + DateRange + " and vchnum||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + orig_vchdt + "' and vchdate<=to_DaTE('" + orig_vchdt + "','dd/mm/yyyy') order by vchdate desc", "ldt");
            }
            else
            {
                last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  and vchdate " + DateRange + " and vchnum||to_char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "' order by vchdate desc", "ldt");
            }
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
        //------------------

        last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
        if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt) && frm_cocd != "KRSM")
        {
            Checked_ok = "N";
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            return;
        }
        //-----------------------------
        i = 0;


        //-------------------------------------
        if (edmode.Value == "Y")
        {
            chk_dupl = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(nvl(pname,'-')) as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + " and vchnum||to_char(vchdate,'dd/mm/yyyy')='" + txtvchnum.Text + orig_vchdt + "' and trim(upper(acode))='" + txtlbl4.Text + "'", "ldt");
            if (chk_dupl == "-")
            { }
            else
            {
                if (frm_cocd != "MULT")
                {
                    if (frm_ulvl != "0")
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , This " + lbl1.Text + " Already Inspected by " + chk_dupl + ", Changes Not Allowed  !!");
                        return;
                    }
                }
            }

            chk_dupl = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(nvl(finvno,'-')) as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and vchdate " + DateRange + " and vchnum||to_char(vchdate,'dd/mm/yyyy')='" + txtvchnum.Text + orig_vchdt + "' and trim(upper(acode))='" + txtlbl4.Text + "'", "ldt");

            if (chk_dupl == "-" || chk_dupl == "0")
            { }
            else
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , This " + lbl1.Text + " Already Passed in Accounts by Vch no " + chk_dupl + ", Changes Not Allowed  !!");
                return;
            }

        }


        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "Y");
        string ok_for_save = "Y";

        if (frm_cocd == "SVPL" || frm_cocd == "MINV") splitBatch();

        // Reel Grid Checking
        if (frm_IndType == "05" || frm_IndType == "06" || frm_IndType == "12" || frm_IndType == "13")
        {
            if (sg2.Rows.Count <= 1)
            {
                for (int g = 0; g < sg1.Rows.Count; g++)
                {
                    if (sg1.Rows[g].Cells[13].Text.ToString().Trim().Substring(0, 2) == "08" || sg1.Rows[g].Cells[13].Text.ToString().Trim().Substring(0, 2) == "07" || sg1.Rows[g].Cells[13].Text.ToString().Trim().Substring(0, 2) == "09")
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Reel Not Selected, Please Fill Reels !!");
                        return;
                    }
                }
            }
        }

        if (sg2.Rows.Count > 1)
        {
            reelGridQty();
            err_msg = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_MSG");
            ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");

            if (ok_for_save == "N")
            {
                fgen.msg("-", "AMSG", err_msg);
                return;
            }
        }

        checkGridQty();

        ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
        err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");
        string err_item_name;
        err_item_name = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ERR_ITEM");

        if (ok_for_save == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' MRR Qty is Exceeding Gate Entry Qty , Please Check '13' " + err_item_name + "'13' " + err_item);
            return;
        }

        if (txtlbl13.Text.Trim() == "-" && lbl1a.Text == "07")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13'Please Entry Port code in second Tab!!");
            return;
        }

        ok_for_save = "Y";
        if (lbl1a.Text.Trim() == "09" || lbl1a.Text.Trim() == "0J")
        {
            foreach (GridViewRow gr in sg1.Rows)
            {
                if (((TextBox)gr.FindControl("sg1_t19")).Text.Trim() == "-")
                {
                    err_msg = "RGP Number Can Not be Empty'13'Please Check Serial Number " + gr.Cells[12].Text.Trim();
                    ok_for_save = "N";
                    break;
                }
            }
            if (ok_for_save == "N")
            {
                fgen.msg("-", "AMSG", err_msg);
            }
        }

        // ** Rate check for 05 type on 
        if (lbl1a.Text.Trim() == "05")
        {
            double totval = 0;
            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_MAX_MRR").toDouble() > 0)
            {
                for (int x = 0; x < sg1.Rows.Count; x++)
                {
                    totval += ((TextBox)sg1.Rows[x].FindControl("sg1_t6")).Text.toDouble();
                }
                if (totval > fgenMV.Fn_Get_Mvar(frm_qstr, "U_MAX_MRR").toDouble())
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Value of This MRR is Rs. " + totval + ", Exceeds allowed limit " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_MAX_MRR").toDouble() + ", Please Check ?");
                    return;
                }
            }
        }

        fgen.msg("-", "SMSG", "Are you sure ?'13'May I Save it ?");
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
                drQty["fstr"] = gr.Cells[13].Text.ToString().Trim() + "-" + txtlbl2.Text + "-" + txtlbl3.Text;
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
            string mquery;
            mquery = "select (a.Qtyord)-(a.Soldqty) as Bal_Qty from (select fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||vchnum||'-'||to_ChaR(vchdate,'dd/mm/yyyy') as fstr,trim(Icode) as ERP_code,iqty_chl as Qtyord,0 as Soldqty,1 as prate from ivoucherp where branchcd='" + frm_mbr + "' and type like '00%'  and trim(Acode)||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')='" + txtlbl4.Text.Trim() + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "' union all SELECT trim(icode)||'-'||genum||'-'||to_ChaR(gedate,'dd/mm/yyyy') as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate from ivoucher where branchcd='" + frm_mbr + "' and type='0%' and trim(Acode)||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')='" + txtlbl7.Text.Trim() + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "' and trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and a.fstr='" + drQty1["fstr"].ToString().Trim() + "' order by B.Iname,trim(a.fstr)";
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, mquery, "Bal_Qty");

            if (fgen.make_double(sm.ToString()) > fgen.make_double(col1) && fgen.make_double(col1) > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", drQty1["fstr"].ToString().Trim());

                string itm_name;
                itm_name = fgen.seek_iname(frm_qstr, frm_cocd, "select iname from ITEM where SUBSTR(ICODE,1,8)='" + drQty1["fstr"].ToString().Trim().Substring(0, 8) + "' ", "iname");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ERR_ITEM", itm_name + "   [ GE Qty " + col1 + " MRR Qty " + sm.ToString() + "  ]");

                break;
            }
        }
        return null;
    }
    //------------------------------------------------------------------------------------
    string reelGridQty()
    {
        DataTable dtQty = new DataTable();
        dtQty.Columns.Add(new DataColumn("icode", typeof(string)));
        dtQty.Columns.Add(new DataColumn("qty", typeof(double)));
        dtQty.Columns.Add(new DataColumn("rcount", typeof(double)));
        dtQty.Columns.Add(new DataColumn("iname", typeof(string)));
        DataRow drQty = null;
        col1 = "";
        i = 1;
        foreach (GridViewRow gr in sg1.Rows)
        {
            if (gr.Cells[13].Text.ToString().Trim().Length > 4)
            {
                if (col1 != gr.Cells[13].Text.ToString().Trim()) i = 1;
                drQty = dtQty.NewRow();
                drQty["icode"] = gr.Cells[13].Text.ToString().Trim();
                col1 = gr.Cells[13].Text.ToString().Trim();
                drQty["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t3")).Text.ToString().Trim());
                drQty["rcount"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t4")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
                i++;
            }
        }

        DataTable dtQty1 = new DataTable();
        dtQty1.Columns.Add(new DataColumn("icode", typeof(string)));
        dtQty1.Columns.Add(new DataColumn("qty", typeof(double)));
        dtQty1.Columns.Add(new DataColumn("iname", typeof(string)));
        dtQty1.Columns.Add(new DataColumn("rcount", typeof(decimal)));
        DataRow drQty1 = null;
        col1 = "";
        i = 1;
        foreach (GridViewRow gr in sg2.Rows)
        {
            if (gr.Cells[3].Text.ToString().Trim().Length > 4)
            {
                if (col1 != gr.Cells[3].Text.ToString().Trim()) i = 1;
                drQty1 = dtQty1.NewRow();
                drQty1["icode"] = gr.Cells[3].Text.ToString().Trim();
                col1 = gr.Cells[3].Text.ToString().Trim();
                drQty1["iname"] = gr.Cells[4].Text.ToString().Trim();
                drQty1["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg2_t4")).Text.ToString().Trim());
                drQty1["rcount"] = i;
                dtQty1.Rows.Add(drQty1);
                i++;
            }
        }

        object sm, sm1;

        DataView distQty = new DataView(dtQty, "", "icode", DataViewRowState.CurrentRows);
        DataTable dtQty2 = new DataTable();
        dtQty2 = distQty.ToTable(true, "icode");

        foreach (DataRow drQty2 in dtQty2.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "icode='" + drQty2["icode"].ToString().Trim() + "'");
            sm1 = dtQty1.Compute("sum(qty)", "icode='" + drQty2["icode"].ToString().Trim() + "'");

            if (fgen.make_double(sm.ToString()) != fgen.make_double(sm1.ToString()))
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_MSG", "Qty Mismatch for Item : " + fgen.seek_iname_dt(dtQty, "icode='" + drQty2["icode"].ToString().Trim() + "'", "iname") + "'13' '13'Item Grid Qty : " + sm.ToString() + "'13'Reel Grid Qty : " + sm1.ToString());
                break;
            }

            //sm = dtQty.Compute("sum(rcount)", "icode='" + drQty2["icode"].ToString().Trim() + "'");
            //sm1 = dtQty1.Compute("max(rcount)", "icode='" + drQty2["icode"].ToString().Trim() + "'");

            //if (fgen.make_double(sm.ToString()) != fgen.make_double(sm1.ToString()) && fgen.make_double(sm1.ToString()) > 0 && fgen.make_double(sm.ToString()) > 0)
            //{
            //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
            //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_MSG", "Qty Mismatch for Item : " + fgen.seek_iname_dt(dtQty, "icode='" + drQty2["icode"].ToString().Trim() + "'", "iname") + "'13' '13'Item Grid Count : " + sm.ToString() + "'13'Reel Grid Count : " + sm1.ToString());
            //    break;
            //}
        }
        return null;
    }
    //------------------------------------------------------------------------------------/
    void splitBatch()
    {
        // REEL TABLE        
        create_tab2();
        sg2_dr = null;
        i = 0;
        double fullQty = 0;
        double batchQty = 0;
        double BatchNo = 0;
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        cond = " and type='" + frm_vty + "'";
        BatchNo = fgen.make_double(frm_CDT1.Substring(8, 2) + frm_vty + fgen.next_no(frm_qstr, frm_cocd, "Select max(substr(kclreelno,5,10)) as vch from reelvch where branchcd='" + frm_mbr + "' " + cond + " and vchdate " + DateRange + "", 6, "vch"));
        foreach (GridViewRow gr1 in sg1.Rows)
        {

            batchQty = fgen.make_double(gr1.Cells[2].Text.ToString());
            fullQty = fgen.make_double(((TextBox)gr1.FindControl("sg1_t3")).Text.Trim());
            if (batchQty == 0) batchQty = fullQty;

            do
            {
                sg2_dr = sg2_dt.NewRow();

                sg2_dr["sg2_srno"] = i;
                sg2_dr["sg2_h1"] = gr1.Cells[13].Text.Trim();
                sg2_dr["sg2_h2"] = gr1.Cells[13].Text.Trim();
                sg2_dr["sg2_h3"] = "";
                sg2_dr["sg2_h4"] = "";
                sg2_dr["sg2_h5"] = "";

                sg2_dr["sg2_f1"] = gr1.Cells[13].Text.Trim();
                sg2_dr["sg2_f2"] = gr1.Cells[14].Text.Trim();
                sg2_dr["sg2_f3"] = "0";
                sg2_dr["sg2_f4"] = "0";
                sg2_dr["sg2_f5"] = "0";


                sg2_dr["sg2_t1"] = BatchNo;

                sg2_dr["sg2_t2"] = "0";
                sg2_dr["sg2_t3"] = "0";

                if (fullQty <= batchQty)
                {
                    batchQty = fullQty;
                    fullQty = fullQty - batchQty;
                }
                else fullQty = fullQty - batchQty;

                sg2_dr["sg2_t4"] = batchQty;

                sg2_dr["sg2_t5"] = 0;
                sg2_dr["sg2_t6"] = BatchNo;
                sg2_dr["sg2_t7"] = "0";
                sg2_dr["sg2_t8"] = "0";
                sg2_dr["sg2_t9"] = i.ToString();
                sg2_dr["sg2_t10"] = "0";

                sg2_dt.Rows.Add(sg2_dr);
                BatchNo += 1;
                i++;
            }
            while (fullQty != 0);
        }
        sg2_add_blankrows();
        ViewState["sg2"] = sg2_dt;
        sg2.DataSource = sg2_dt;
        sg2.DataBind();
        sg2_dt.Dispose();
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

        //chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        //if (chk_rights == "Y")
        //{
        //    hffield.Value = "PDEL";
        //    fgen.open_pwdbox("-", frm_qstr, btndel);
        //}
        //else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form !!");
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
        ViewState["kclreelno"] = null;

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
        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            hffield.Value = "Print";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Print For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
        string mgst_no;
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from REELVCH a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivchctrl a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data a where par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
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
                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    lbl1aName.Text = col2;
                    if (continueNumberSer == "Y") cond = " and type like '0%' ";
                    else cond = " and type='" + frm_vty + "'";
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' " + cond + " AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

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
                case "PDEL":
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_CONFIRM") != "1") return;
                    clearctrl();
                    set_Val();
                    hffield.Value = "Del";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
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
                    lbl1aName.Text = col2;
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
                case "SPrint":
                case "Print":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = btnval + "_E";
                    make_qry_4_popup();
                    if (frm_cocd == "RIKI" && btnval == ("Print_E"))
                    {
                        hffield.Value = "PrintR";
                        fgen.Fn_open_prddmp1("", frm_qstr);
                    }
                    else fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();

                    string mv_col;
                    mv_col = frm_mbr + frm_vty + col1;

                    SQuery = "Select a.*,to_char(A.rgpdate,'dd/mm/yyyy') as rgpdtd,to_char(A.podate,'dd/mm/yyyy') as podtd,c.Aname,c.gst_no,nvl(b.Iname,'-') As Iname,nvl(b.cpartno,'-') As Icpartno,nvl(b.cdrgno,'-') As Icdrgno,nvl(b.unit,'-') as IUnit,nvl(b.packsize,0) as ipacksize,nvl(b.iweight,0) as iiweight ,to_number(a.cavity) as cavity2 from " + frm_tabname + " a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' and a.store<>'R' ORDER BY A.morder";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txtvchnum.Text = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl2.Text = dt.Rows[i]["genum"].ToString().Trim();
                        txtlbl3.Text = Convert.ToDateTime(fgen.make_def_Date(dt.Rows[0]["gedate"].ToString().Trim(), vardate)).ToString("dd/MM/yyyy");

                        txtlbl4.Text = dt.Rows[i]["Acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();

                        if (fgen.make_double(frm_ulvl) > 2 && dt.Rows[i]["PNAME"].ToString().Trim() != "-")
                        {
                            fgen.msg("-", "AMSG", "Inspected MRR Cannot be Edited, Contact HOD/Admin");
                            return;
                        }
                        if (fgen.make_double(frm_ulvl) > 1 && dt.Rows[i]["finvno"].ToString().Trim() != "-")
                        {
                            fgen.msg("-", "AMSG", "MRR passed in Accounts , Cannot be Edited, Contact HOD/Admin");
                            return;
                        }


                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OLD_DATE", txtvchdate.Text);

                        mgst_no = dt.Rows[i]["gst_no"].ToString().Trim();

                        if (mgst_no.Length > 10 || doc_GST.Value == "GCC")
                        {
                            txtTax.Text = "Y";
                        }
                        else
                        {
                            txtTax.Text = "N";
                        }


                        txtlbl5.Text = dt.Rows[i]["invno"].ToString().Trim();
                        txtlbl6.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");


                        txtlbl8.Text = dt.Rows[i]["refnum"].ToString().Trim();
                        txtlbl9.Text = Convert.ToDateTime(dt.Rows[0]["refdate"].ToString().Trim()).ToString("dd/MM/yyyy");




                        txtlbl15.Text = dt.Rows[i]["form31"].ToString().Trim();
                        txtlbl16.Text = dt.Rows[i]["mode_tpt"].ToString().Trim();
                        txtlbl17.Text = dt.Rows[i]["styleno"].ToString().Trim();
                        txtlbl18.Text = dt.Rows[i]["mtime"].ToString().Trim();

                        txtlbl24.Text = dt.Rows[i]["cavity2"].ToString().Trim();
                        txtlbl26.Text = dt.Rows[i]["st_entform"].ToString().Trim();
                        txtlbl28.Text = dt.Rows[i]["segment_"].ToString().Trim();
                        txtlbl30.Text = dt.Rows[i]["isize"].ToString().Trim();

                        txtlbl13.Text = dt.Rows[i]["location"].ToString().Trim();

                        if (txtlbl28.Text.ToUpper() == "1")
                        {
                            txtlbl28.Text = "Y";
                        }
                        if (txtlbl28.Text.ToUpper() == "2")
                        {
                            txtlbl28.Text = "N";
                        }
                        if (txtlbl28.Text.ToUpper() == "3")
                        {
                            txtlbl28.Text = "X";
                        }


                        txtrmk.Text = dt.Rows[i]["naration"].ToString().Trim();

                        txtlbl7.Text = dt.Rows[i]["vcode"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT aname FROM famst WHERE trim(acode)='" + txtlbl7.Text.Trim() + "'", "aname");

                        txtlbl70.Text = dt.Rows[i]["gst_pos"].ToString().Trim();
                        txtlbl71.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT name FROM type WHERE id='{' and trim(type1)='" + txtlbl70.Text.Trim() + "'", "name");

                        txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");
                        txtlbl73.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM famst WHERE trim(acode)='" + txtlbl4.Text.Trim() + "'", "STATEn");

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = "-";
                            sg1_dr["sg1_h2"] = "-";
                            sg1_dr["sg1_h3"] = dt.Rows[i]["ipacksize"].ToString().Trim();
                            //fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PACKSIZE FROM ITEM WHERE TRIM(ICODe)='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "PACKSIZE");
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";


                            sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["ICpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["Icdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["IUnit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["iqty_chl"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["iqty_chlwt"].ToString().Trim();

                            sg1_dr["sg1_t3"] = dt.Rows[i]["iqtyin"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["iqty_wt"].ToString().Trim();

                            sg1_dr["sg1_t5"] = dt.Rows[i]["IRATE"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["iamount"].ToString().Trim();

                            sg1_dr["sg1_t7"] = dt.Rows[i]["exc_Rate"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["cess_percent"].ToString().Trim();

                            sg1_dr["sg1_t9"] = dt.Rows[i]["desc_"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["btchno"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["btchdt"].ToString().Trim();

                            sg1_dr["sg1_t12"] = dt.Rows[i]["mfgdt"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["expdt"].ToString().Trim();

                            sg1_dr["sg1_t14"] = dt.Rows[i]["ponum"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["ordlineno"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["podtd"].ToString().Trim();

                            sg1_dr["sg1_t17"] = dt.Rows[i]["exc_amt"].ToString().Trim();
                            sg1_dr["sg1_t18"] = dt.Rows[i]["cess_pu"].ToString().Trim();

                            sg1_dr["sg1_t19"] = dt.Rows[i]["rgpnum"].ToString().Trim();
                            sg1_dr["sg1_t20"] = dt.Rows[i]["rgpdtd"].ToString().Trim();

                            sg1_dr["sg1_t21"] = dt.Rows[i]["col1"].ToString().Trim();
                            sg1_dr["sg1_t22"] = dt.Rows[i]["iiweight"].ToString().Trim();
                            sg1_dr["sg1_t23"] = dt.Rows[i]["potype"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        if (gate_link != "Y")
                            sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        //------------------------

                        // REEL TABLE
                        SQuery = "SELECT A.*,b.iname FROM REELVCH A,item b WHERE trim(a.icodE)=trim(B.icode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' order by a.srno";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        create_tab2();
                        sg2_dr = null;
                        i = 1;
                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                sg2_dr = sg2_dt.NewRow();

                                sg2_dr["sg2_srno"] = i;
                                sg2_dr["sg2_h1"] = dr["icode"].ToString().Trim();
                                sg2_dr["sg2_h2"] = dr["icode"].ToString().Trim();
                                sg2_dr["sg2_h3"] = "";
                                sg2_dr["sg2_h4"] = "";
                                sg2_dr["sg2_h5"] = "";

                                sg2_dr["sg2_f1"] = dr["icode"].ToString().Trim();
                                sg2_dr["sg2_f2"] = dr["iname"].ToString().Trim();
                                sg2_dr["sg2_f3"] = "";
                                sg2_dr["sg2_f4"] = "";
                                sg2_dr["sg2_f5"] = "";

                                sg2_dr["sg2_t1"] = dr["kclreelno"].ToString().Trim();
                                sg2_dr["sg2_t2"] = dr["psize"].ToString().Trim();
                                sg2_dr["sg2_t3"] = dr["gsm"].ToString().Trim();
                                sg2_dr["sg2_t4"] = dr["reelwin"].ToString().Trim();
                                sg2_dr["sg2_t5"] = dr["irate"].ToString().Trim();
                                sg2_dr["sg2_t6"] = dr["coreelno"].ToString().Trim();
                                sg2_dr["sg2_t7"] = dr["reelspec1"].ToString().Trim();
                                sg2_dr["sg2_t8"] = dr["reelspec2"].ToString().Trim();
                                sg2_dr["sg2_t9"] = i.ToString(); ;
                                sg2_dr["sg2_t10"] = "";

                                sg2_dt.Rows.Add(sg2_dr);
                                i++;
                            }
                        }
                        sg2_add_blankrows();
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        dt.Dispose();
                        sg2_dt.Dispose();

                        SQuery = "SELECT A.* FROM IVCHCTRL A WHERE a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtlbl10.Text = dt.Rows[0]["pack_amt"].ToString();
                            txtlbl11.Text = dt.Rows[0]["other"].ToString();
                            txtlbl12.Text = dt.Rows[0]["frt_amt"].ToString();
                        }


                        SQuery = "Select nvl(a.udf_name,'-') as udf_name,nvl(a.udf_value,'-') as udf_value from udf_Data a where trim(a.par_tbl)='" + frm_tabname + "' and trim(a.par_fld)='" + mv_col + "' ORDER BY a.srno";
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

                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;
                case "SAVED":
                    hffield.Value = "Print_E";
                    if (frm_cocd == "SVPL" || frm_cocd == "MINV") hffield.Value = "SPrint_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;
                case "Print_E":
                case "PrintR":
                    if (col1.Length < 2) return;
                    col2 = "F1002";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", col2);
                    fgen.fin_invn_reps(frm_qstr);
                    break;
                case "SPrint_E":
                    if (col1.Length < 2) return;
                    col2 = "S1002";
                    //need to test on sticker print machien :: changed to pdf=N so that it prints like a sticker
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", "N");
                    if (frm_cocd == "SRPF") col2 += "R";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", col2);
                    fgen.fin_invn_reps(frm_qstr);
                    break;
                case "TACODE":
                    //-----------------------------
                    if (col1.Length <= 0) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;

                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;

                    SQuery = "Select b.iname,nvl(b.packsize,0) as ipacksize,nvl(b.iweight,0) as iiweight,nvl(b.cpartno,'-') as icpartno,nvl(b.cdrgno,'-') as icdrgno,nvl(b.unit,'-') as iunit,nvl(b.hscode,'-') as hscode,b.irate as masterRate,a.* from ivoucherp a,item b where trim(a.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '00%' and a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode)='" + col1 + "' ORDER BY A.srno";

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl2.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtlbl3.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl4.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst  where acode='" + txtlbl4.Text + "'", "aname");

                        mgst_no = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no from famst  where acode='" + txtlbl4.Text + "'", "gst_no");

                        if (mgst_no.Length > 10 || doc_GST.Value == "GCC")
                        {
                            txtTax.Text = "Y";
                        }
                        else
                        {
                            txtTax.Text = "N";
                        }

                        txtlbl5.Text = dt.Rows[0]["invno"].ToString().Trim();
                        if (dt.Rows[0]["invdate"].ToString().Trim().Length > 1)
                            txtlbl6.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        else txtlbl6.Text = DateTime.Now.ToString("dd/MM/yyyy");

                        txtlbl8.Text = dt.Rows[0]["refnum"].ToString().Trim();
                        txtlbl9.Text = Convert.ToDateTime(dt.Rows[0]["refdate"].ToString().Trim()).ToString("dd/MM/yyyy");


                        txtlbl15.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT br_Curren FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "br_Curren");
                        if (txtlbl15.Text.Length < 2 && doc_GST.Value != "GCC")
                        {
                            txtlbl15.Text = "INR";
                        }

                        txtlbl24.Text = "1";

                        txtlbl16.Text = dt.Rows[0]["MODE_TPT"].ToString().Trim();
                        txtlbl18.Text = dt.Rows[0]["mtime"].ToString().Trim();

                        txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");
                        txtlbl73.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM famst WHERE  acode='" + txtlbl4.Text + "'", "STATEn");

                        if (txtlbl73.Text.Length > 1)
                        {
                            txtlbl70.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT type1 FROM TYPE WHERE ID='{' AND upper(Trim(Name))=upper(Trim('" + txtlbl73.Text + "'))", "type1");
                            txtlbl71.Text = txtlbl73.Text;
                        }
                        create_tab();
                        sg1_dr = null;
                        string tcol1 = "";
                        string tcol2 = "";
                        dt4 = new DataTable();
                        dt4 = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(ACREF) as ACREF,NUM4,NUM5,NUM6 FROM TYPEGRP WHERE ID='T1' ORDER BY ACREF ");

                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = "-";
                            sg1_dr["sg1_h2"] = "-";

                            sg1_dr["sg1_h3"] = dt.Rows[i]["ipacksize"].ToString().Trim();
                            sg1_dr["sg1_h4"] = "-";

                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";

                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";
                            sg1_dr["sg1_srno"] = i + 1;
                            sg1_dr["sg1_f1"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["icpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["icdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["iunit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["iqty_chl"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["iqty_chlwt"].ToString().Trim();

                            sg1_dr["sg1_t3"] = dt.Rows[i]["iqty_chl"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["iqty_chlwt"].ToString().Trim();

                            sg1_dr["sg1_t5"] = fgen.make_double(dt.Rows[i]["irate"].ToString().Trim()) > 0 ? dt.Rows[i]["irate"].ToString().Trim() : dt.Rows[i]["masterRate"].ToString().Trim();

                            //if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                            //{
                            //    sg1_dr["sg1_t7"] = fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(num4,0) as num4 from typegrp where branchcd!='DD' and id='T1' and trim(upper(acref))=upper(Trim('" + dt.Rows[i]["hscode"].ToString().Trim() + "'))", "num4");
                            //    sg1_dr["sg1_t8"] = fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(num5,0) as num5 from typegrp where branchcd!='DD' and id='T1' and trim(upper(acref))=upper(Trim('" + dt.Rows[i]["hscode"].ToString().Trim() + "'))", "num5");
                            //}
                            //else
                            //{
                            //    sg1_dr["sg1_t7"] = fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(num6,0) as num6 from typegrp where branchcd!='DD' and id='T1' and trim(upper(acref))=upper(Trim('" + dt.Rows[i]["hscode"].ToString().Trim() + "'))", "num6");
                            //    sg1_dr["sg1_t8"] = 0;
                            //}
                            tcol1 = dt.Rows[i]["hscode"].ToString().Trim().ToUpper();
                            tcol2 = fgen.seek_iname_dt(dt4, "acref='" + tcol1 + "'", "acref");
                            if (tcol2 != "0")
                            {
                                try
                                {
                                    if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                                    {
                                        sg1_dr["sg1_t7"] = fgen.seek_iname_dt(dt4, "acref='" + tcol1 + "'", "num4");
                                        sg1_dr["sg1_t8"] = fgen.seek_iname_dt(dt4, "acref='" + tcol1 + "'", "num5");
                                    }
                                    else
                                    {
                                        sg1_dr["sg1_t7"] = fgen.seek_iname_dt(dt4, "acref='" + tcol1 + "'", "num6");
                                        sg1_dr["sg1_t8"] = "0";
                                    }
                                    if (doc_GST.Value == "GCC")
                                    {
                                        sg1_dr["sg1_t7"] = fgen.seek_iname_dt(dt4, "acref='" + tcol1 + "'", "num6");
                                        sg1_dr["sg1_t8"] = "0";
                                    }
                                }
                                catch { }
                            }

                            if (txtTax.Text == "N")
                            {
                                sg1_dr["sg1_t7"] = 0;
                                sg1_dr["sg1_t8"] = 0;
                            }

                            sg1_dr["sg1_t9"] = dt.Rows[i]["desc_"].ToString().Trim();


                            {
                                sg1_dr["sg1_t10"] = dt.Rows[i]["btchno"].ToString().Trim();
                                sg1_dr["sg1_t11"] = dt.Rows[i]["btchdt"].ToString().Trim();
                            }

                            sg1_dr["sg1_t14"] = dt.Rows[i]["ponum"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["ordlineno"].ToString().Trim();
                            sg1_dr["sg1_t16"] = Convert.ToDateTime(dt.Rows[0]["podate"].ToString().Trim()).ToString("dd/MM/yyyy");

                            sg1_dr["sg1_t19"] = dt.Rows[i]["rgpnum"].ToString().Trim();
                            sg1_dr["sg1_t20"] = dt.Rows[i]["rgpdate"].ToString().Trim();

                            sg1_dr["sg1_t21"] = fgen.padlc((i + 1), 3);
                            sg1_dr["sg1_t22"] = dt.Rows[i]["iiweight"].ToString().Trim();
                            sg1_dr["sg1_t23"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT type FROM POMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '5%' AND TRIM(ORDNO)||TO_CHAR(ORDDT,'DD/MM/YYYY')='" + sg1_dr["sg1_t14"].ToString() + sg1_dr["sg1_t16"].ToString().Trim() + "' AND TRIM(ICODE)='" + sg1_dr["sg1_f1"].ToString().Trim() + "' ", "type");
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
                    btnlbl7.Focus();

                    if (lbl1a.Text == "07")
                    {
                        hffield.Value = "CONVR";
                        fgen.Fn_ValueBox("Please Entry Rate of Exchange as per Bill", frm_qstr);
                    }
                    break;
                //-----------------------------
                case "CONVR":
                    txtlbl15.Text = "";
                    txtlbl24.Text = col1.toDouble(2).ToString();
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
                    btnlbl16.Focus();
                    break;
                case "BTN_16":
                    if (col1.Length <= 0) return;
                    txtlbl16.Text = col2;
                    btnlbl17.Focus();
                    break;
                case "BTN_17":
                    if (col1.Length <= 0) return;
                    txtlbl17.Text = col2;
                    btnlbl18.Focus();
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
                            sg1_dr["sg1_t21"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();
                            sg1_dr["sg1_t22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim();
                            sg1_dr["sg1_t23"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        dt = new DataTable();
                        String pop_qry;
                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");
                        if (col1.Trim().Length == 8) SQuery = "select a.po_no,a.fstr,a.ERP_code,a.Item_Name,a.Part_no,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                        else SQuery = "select a.po_no,a.fstr,a.ERP_code,a.Item_Name,a.Part_no,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc," +
                                "a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' " +
                                "and trim(a.fstr) in (" + col1 + ")";
                        if (gate_link == "N")
                        {
                            if (col1.Trim().Length == 8) SQuery = "select a.*,b.num4,b.num5,b.num6,b.num7,'-' as iexc_Addl,'-' as frt_pu,'-' as pkchg_pu from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                            else SQuery = "select a.*,b.num4,b.num5,b.num6,b.num7,'-' as iexc_Addl,'-' as frt_pu,'-' as pkchg_pu from (" + pop_qry + ") a,typegrp b where " +
                                    "trim(a.hscode)=trim(b.acref) and trim(b.id)='T1' and trim(a.fstr) in (" + col1 + ")";
                        }
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            if (frm_IndType == "05" || frm_IndType == "06" || frm_IndType == "12" || frm_IndType == "13")
                            {
                                itemCond = "substr(icode,1,2) not in ('08','07','09')";
                                if (frm_vty != "02" && frm_vty != "07")
                                {
                                    if (dt.Rows[d]["ERP_code"].ToString().Trim().Substring(0, 2) == "08" || dt.Rows[d]["ERP_code"].ToString().Trim().Substring(0, 2) == "07")
                                    {
                                        fgen.msg("-", "AMSG", "08, 07 is applicable only on 02 and 07 type MRR!!");
                                        return;
                                    }
                                }
                            }
                            sg1_dr["sg1_h1"] = dt.Rows[d]["ERP_code"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[d]["Item_Name"].ToString().Trim();
                            sg1_dr["sg1_h3"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PACKSIZE FROM ITEM WHERE TRIM(ICODe)='" + dt.Rows[d]["ERP_code"].ToString().Trim() + "'", "PACKSIZE");
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";

                            sg1_dr["sg1_f1"] = dt.Rows[d]["ERP_code"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["Item_Name"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["Part_no"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["po_no"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();

                            //sg1_dr["sg1_t1"] = dt.Rows[d][gate_link == "N" ? "Balance_Qty" : "Balance_Qty"].ToString().Trim();
                            //sg1_dr["sg1_t2"] = dt.Rows[d][gate_link == "N" ? "Balance_Qty" : "Balance_Qty"].ToString().Trim();
                            //sg1_dr["sg1_t3"] = dt.Rows[d][gate_link == "N" ? "Balance_Qty" : "Balance_Qty"].ToString().Trim();

                            sg1_dr["sg1_t1"] = "0";
                            sg1_dr["sg1_t2"] = "0";
                            sg1_dr["sg1_t3"] = "0";

                            sg1_dr["sg1_t5"] = dt.Rows[d]["Irate"].ToString().Trim();

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

                            sg1_dr["sg1_t9"] = "";
                            sg1_dr["sg1_t10"] = "-";
                            sg1_dr["sg1_t11"] = dt.Rows[d]["iexc_Addl"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[d]["frt_pu"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[d]["pkchg_pu"].ToString().Trim();

                            string mpo_Dt;
                            try
                            {
                                if (gate_link == "Y")
                                {
                                    mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(9, 6);
                                    sg1_dr["sg1_t14"] = mpo_Dt;
                                    sg1_dr["sg1_t15"] = "";
                                    mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(6, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(4, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 4);
                                    sg1_dr["sg1_t16"] = fgen.make_def_Date(mpo_Dt, vardate);
                                }
                                else
                                {
                                    mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(18, 6);
                                    sg1_dr["sg1_t14"] = mpo_Dt;
                                    sg1_dr["sg1_t15"] = dt.Rows[d]["fstr"].ToString().Trim().Substring(25, 4);
                                    mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(15, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(13, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(9, 4);
                                    sg1_dr["sg1_t16"] = fgen.make_def_Date(mpo_Dt, vardate);

                                    if (lbl1a.Text == "09" || lbl1a.Text == "0J")
                                    {
                                        mpo_Dt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PRATE FROM POMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '5%' AND TRIM(ORDNO)||TO_CHAR(ORDDT,'DD/MM/YYYY')='" + sg1_dr["sg1_t14"].ToString() + sg1_dr["sg1_t16"].ToString().Trim() + "' AND TRIM(ICODE)='" + sg1_dr["sg1_f1"].ToString().Trim() + "' ", "PRATE");
                                        if (mpo_Dt != "0")
                                            sg1_dr["sg1_t5"] = mpo_Dt;
                                        else
                                        {
                                            mpo_Dt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PRATE,TO_CHAR(ORDDT,'YYYYMMDD') AS VDD FROM POMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '5%' AND TRIM(ACODE)='" + txtlbl4.Text.Trim() + "' AND TRIM(ICODE)='" + sg1_dr["sg1_f1"].ToString().Trim() + "' ORDER BY VDD DESC", "PRATE");
                                            if (mpo_Dt != "0")
                                                sg1_dr["sg1_t5"] = mpo_Dt;
                                        }
                                    }
                                }

                                if (frm_vty != "02")
                                {
                                    mpo_Dt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT QTYBAL FROM POMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '5%' AND TRIM(ORDNO)||TO_CHAR(ORDDT,'DD/MM/YYYY')='" + sg1_dr["sg1_t14"].ToString() + sg1_dr["sg1_t16"].ToString().Trim() + "' AND TRIM(ICODE)='" + sg1_dr["sg1_f1"].ToString().Trim() + "' ", "QTYBAL");
                                    if (mpo_Dt != "0")
                                        sg1_dr["sg1_t22"] = mpo_Dt;
                                    mpo_Dt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT LANDCOST FROM POMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '5%' AND TRIM(ORDNO)||TO_CHAR(ORDDT,'DD/MM/YYYY')='" + sg1_dr["sg1_t14"].ToString() + sg1_dr["sg1_t16"].ToString().Trim() + "' AND TRIM(ICODE)='" + sg1_dr["sg1_f1"].ToString().Trim() + "' ", "LANDCOST");
                                    if (mpo_Dt != "0")
                                        sg1_dr["sg1_t5"] = mpo_Dt;
                                }
                            }
                            catch { }

                            if (frm_vty == "02")
                            {
                                sg1_dr["sg1_t22"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT IWEIGHT AS WT FROM ITEM WHERE TRIM(ICODE)='" + sg1_dr["sg1_f1"].ToString().Trim() + "' ", "WT");
                                if (sg1_dr["sg1_t22"].ToString().toDouble() == 0)
                                {
                                    sg1_dr["sg1_t22"] = "1";
                                }
                            }

                            // weight calc
                            var sg1t1qty = dt.Rows[d][gate_link == "N" ? "Balance_Qty" : "Balance_Qty"].ToString().Trim();
                            sg1_dr["sg1_h4"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TYPE FROM POMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '5%' AND TRIM(ORDNO)||TO_CHAR(ORDDT,'DD/MM/YYYY')='" + sg1_dr["sg1_t14"].ToString() + sg1_dr["sg1_t16"].ToString().Trim() + "' AND TRIM(ICODE)='" + sg1_dr["sg1_f1"].ToString().Trim() + "' ", "TYPE");
                            sg1_dr["sg1_t2"] = (sg1t1qty.ToString().toDouble() * sg1_dr["sg1_t22"].ToString().toDouble());
                            sg1_dr["sg1_t4"] = (sg1t1qty.ToString().toDouble() * sg1_dr["sg1_t22"].ToString().toDouble());

                            if (frm_vty == "09" || frm_vty == "0J")
                            {
                                sg1_dr["sg1_t19"] = sg1_dr["sg1_t14"];
                                sg1_dr["sg1_t20"] = sg1_dr["sg1_t16"];
                            }

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    //dt.Dispose(); 
                    sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
                    setGST();
                    break;
                case "SG2_ROW_ADD":
                    if (col1.Length < 2) return;
                    #region for gridview 2
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
                            sg2_dr["sg2_h1"] = dt.Rows[i]["sg2_h1"].ToString();
                            sg2_dr["sg2_h2"] = dt.Rows[i]["sg2_h2"].ToString();
                            sg2_dr["sg2_h3"] = dt.Rows[i]["sg2_h3"].ToString();
                            sg2_dr["sg2_h4"] = dt.Rows[i]["sg2_h4"].ToString();
                            sg2_dr["sg2_h5"] = dt.Rows[i]["sg2_h5"].ToString();

                            sg2_dr["sg2_f1"] = dt.Rows[i]["sg2_f1"].ToString();
                            sg2_dr["sg2_f2"] = dt.Rows[i]["sg2_f2"].ToString();
                            sg2_dr["sg2_f3"] = dt.Rows[i]["sg2_f3"].ToString();
                            sg2_dr["sg2_f4"] = dt.Rows[i]["sg2_f4"].ToString();
                            sg2_dr["sg2_f5"] = dt.Rows[i]["sg2_f5"].ToString();

                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                            sg2_dr["sg2_t3"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim();
                            sg2_dr["sg2_t4"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim();
                            sg2_dr["sg2_t5"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim();
                            sg2_dr["sg2_t6"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim();
                            sg2_dr["sg2_t7"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim();
                            sg2_dr["sg2_t8"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim();
                            sg2_dr["sg2_t9"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim();
                            sg2_dr["sg2_t10"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim();

                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        string reelno = "", oldreelno = "";

                        int reelnumlen = 6;
                        if (frm_cocd == "KPPL") reelnumlen = 10;

                        reelno = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT KCLREELNO AS VCH,TO_CHAR(VCHDATE,'YYYYMMDD') AS VDD FROM REELVCH WHERE BRANCHCD='" + frm_mbr + "' AND TYPE like '0%' AND VCHDATE " + DateRange + " ORDER BY VDD DESC ", "VCH");
                        oldreelno = reelno;
                        if (ViewState["kclreelno"] != null)
                            kclreelno = (int)ViewState["kclreelno"];
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_h1"] = col1;
                            sg2_dr["sg2_h2"] = col2;
                            sg2_dr["sg2_h3"] = "-";
                            sg2_dr["sg2_h4"] = "-";
                            sg2_dr["sg2_h5"] = "-";
                            sg2_dr["sg2_f1"] = col1;
                            sg2_dr["sg2_f2"] = col2.Trim();
                            sg2_dr["sg2_f3"] = "-";
                            sg2_dr["sg2_f4"] = "-";
                            sg2_dr["sg2_f5"] = "-";
                            //if (reelno == oldreelno) { reelno = (reelno.toDouble() + kclreelno + 1).ToString().PadLeft(6, '0'); }
                            //else { reelno = (reelno.toDouble() + 1).ToString().PadLeft(6, '0'); }
                            if (kclreelno == 0) kclreelno = 1;
                            reelno = (reelno.toDouble() + kclreelno).ToString().PadLeft(reelnumlen, '0');
                            sg2_dr["sg2_t1"] = reelno;
                            sg2_dr["sg2_t2"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "").Replace("&nbsp;", "");
                            sg2_dr["sg2_t3"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "").Replace("&nbsp;", "");
                            sg2_dt.Rows.Add(sg2_dr);
                            kclreelno++;
                            ViewState["kclreelno"] = kclreelno;
                        }
                    }
                    sg2_add_blankrows();

                    ViewState["sg2"] = sg2_dt;
                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    dt.Dispose(); sg2_dt.Dispose();
                    ((TextBox)sg2.Rows[z].FindControl("sg2_t1")).Focus();
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


                    //********* Saving in Hidden Field
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    //********* Saving in GridView Value
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "").Replace("&nbsp;", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "").Replace("&nbsp;", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "").Replace("&nbsp;", "");

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
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "").Replace("&nbsp;", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "").Replace("&nbsp;", "");

                    break;
                case "SG1_ROW_DT":

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

                            sg2_dr["sg2_h1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_h2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();

                            sg2_dr["sg2_f1"] = sg2.Rows[i].Cells[3].Text;
                            sg2_dr["sg2_f2"] = sg2.Rows[i].Cells[4].Text;
                            sg2_dr["sg2_f3"] = sg2.Rows[i].Cells[5].Text;
                            sg2_dr["sg2_f4"] = sg2.Rows[i].Cells[6].Text;
                            sg2_dr["sg2_f5"] = sg2.Rows[i].Cells[7].Text;

                            sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                            sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                            sg2_dr["sg2_t3"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim();
                            sg2_dr["sg2_t4"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim();
                            sg2_dr["sg2_t5"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim();
                            sg2_dr["sg2_t6"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim();
                            sg2_dr["sg2_t7"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim();
                            sg2_dr["sg2_t8"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim();
                            sg2_dr["sg2_t9"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t9")).Text.Trim();
                            sg2_dr["sg2_t10"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t10")).Text.Trim();

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
                            sg1_dr["sg1_t21"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();
                            sg1_dr["sg1_t22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim();
                            sg1_dr["sg1_t23"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim();

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
                case "sg1_t9":
                    if (col1.Length > 1)
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t9")).Text = col2;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Focus();
                    }
                    break;
                case "PONO":
                    if (col1.Length > 2)
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t14")).Text = col1.Split('-')[0];
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t16")).Text = col1.Split('-')[1];
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t5")).Text = col3;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t6")).Focus();
                    }
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

            SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, "F25126", "branchcd='" + frm_mbr + "'", "a.type like '0%' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ", "" + PrdRange);
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("MRR Entry Checklist for the Period " + fromdt + " to " + todt, frm_qstr);

            hffield.Value = "-";
        }
        else if (hffield.Value == "PrintR")
        {
            hffield.Value = "PrintR";
            fgen.Fn_open_RangeBox("", frm_qstr);
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------


            #region Number Gen and Save to Table
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "N")
            {
                btnsave.Disabled = false;
            }
            else
            {
                // double checking for GE
                string mhd = "";
                gate_link = fgen.seek_iname(frm_qstr, frm_cocd, "select enable_yn from controls where id='M52'", "enable_yn");
                if (gate_link == "Y")
                {
                    mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR FROM IVOUCHERP WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='00' AND TRIM(VCHNUM)||TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + txtlbl2.Text + txtlbl3.Text + "' ", "FSTR");
                    if (mhd == "0")
                    {
                        fgen.msg("-", "AMSG", "Gate Entry " + txtlbl2.Text + " , Dated " + txtlbl3.Text + " Not Found!!");
                        return;
                    }
                }
                //---------------

                if (Checked_ok == "Y")
                {
                    try
                    {
                        oDS = new DataSet();
                        oporow = null;
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2 = new DataSet();
                        oporow2 = null;
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "ivchctrl");

                        oDS3 = new DataSet();
                        oporow3 = null;
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "reelvch");

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
                        save_fun3();
                        //save_fun4();
                        save_fun5();

                        oDS.Dispose();
                        oporow = null;
                        oDS = new DataSet();
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "ivchctrl");

                        oDS3.Dispose();
                        oporow3 = null;
                        oDS3 = new DataSet();
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "reelvch");

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
                                if (continueNumberSer == "Y") frm_vnum = fgen.Fn_next_doc_no_inv(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                else frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }

                            }
                            txtvchnum.Text = frm_vnum;
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        save_fun2();
                        save_fun3();
                        //save_fun4();
                        save_fun5();
                        string ddl_fld1;
                        string ddl_fld2;
                        ddl_fld1 = frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                        ddl_fld2 = frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");

                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update ivchctrl set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update reelvch set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update udf_Data set branchcd='DD' where par_tbl='" + frm_tabname + "' and par_fld='" + ddl_fld1 + "'");

                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, "ivchctrl");
                        fgen.save_data(frm_qstr, frm_cocd, oDS3, "REELvch");


                        //fgen.save_data(frm_qstr, frm_cocd, oDS4, "budgmst");

                        fgen.save_data(frm_qstr, frm_cocd, oDS5, "udf_Data");


                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully'13'Do you want to see the Print Preview ?");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivchctrl where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from REELvch where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data where branchcd='DD' and par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + ddl_fld2 + "'");
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
        sg1_dt.Columns.Add(new DataColumn("sg1_t17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t20", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t21", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t22", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t23", typeof(string)));
    }
    //------------------------------------------------------------------------------------
    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        // Hidden Field
        sg2_dt.Columns.Add(new DataColumn("sg2_h1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h5", typeof(string)));

        sg2_dt.Columns.Add(new DataColumn("sg2_f1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f5", typeof(string)));

        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t6", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t7", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t8", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t9", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t10", typeof(string)));
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
        if (sg1_dt == null) create_tab();
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
        sg1_dr["sg1_t21"] = "-";
        sg1_dr["sg1_t22"] = "1";
        sg1_dr["sg1_t23"] = "-";

        sg1_dt.Rows.Add(sg1_dr);
    }
    //------------------------------------------------------------------------------------
    public void sg2_add_blankrows()
    {
        if (sg2_dt == null) create_tab2();
        sg2_dr = sg2_dt.NewRow();

        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;

        sg2_dr["sg2_h1"] = "-";
        sg2_dr["sg2_h2"] = "-";
        sg2_dr["sg2_h3"] = "-";
        sg2_dr["sg2_h4"] = "-";
        sg2_dr["sg2_h5"] = "-";

        sg2_dr["sg2_f1"] = "-";
        sg2_dr["sg2_f2"] = "-";
        sg2_dr["sg2_f3"] = "-";
        sg2_dr["sg2_f4"] = "-";
        sg2_dr["sg2_f5"] = "-";

        sg2_dr["sg2_t1"] = "-";
        sg2_dr["sg2_t2"] = "-";
        sg2_dr["sg2_t3"] = "-";
        sg2_dr["sg2_t4"] = "-";
        sg2_dr["sg2_t5"] = "-";
        sg2_dr["sg2_t6"] = "-";
        sg2_dr["sg2_t7"] = "-";
        sg2_dr["sg2_t8"] = "-";
        sg2_dr["sg2_t9"] = "-";
        sg2_dr["sg2_t10"] = "-";

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

                    fgen.Fn_open_dtbox("Select Date", frm_qstr);

                }
                break;

            case "SG1_ROW_ADD":
                if (gate_link == "Y")
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);

                    dt = new DataTable();
                    sg1_dt = new DataTable();
                    dt = (DataTable)ViewState["sg1"];
                    z = dt.Rows.Count - 1;
                    sg1_dt = dt.Clone();
                    sg1_dr = null;
                    i = 0;
                    for (i = 0; i < sg1.Rows.Count; i++)
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
                        sg1_dr["sg1_t21"] = "1";
                        sg1_dr["sg1_t22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim();
                        sg1_dr["sg1_t23"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim();

                        sg1_dt.Rows.Add(sg1_dr);
                    }


                    sg1_dr = sg1_dt.NewRow();
                    sg1_dr["sg1_srno"] = (i + 1);
                    i = index;
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
                    sg1_dr["sg1_t21"] = (i + 2);
                    sg1_dr["sg1_t22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim();
                    sg1_dr["sg1_t23"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim();

                    sg1_dt.Rows.Add(sg1_dr);

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();

                    setColHeadings();
                    set_Val();
                }
                else
                {
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
                hffield.Value = "SG2_ROW_ADD";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

                col1 = "";
                foreach (GridViewRow gr1 in sg1.Rows)
                {
                    if (col1.Length > 0) col1 += ",'" + gr1.Cells[13].Text.Trim().ToString() + "'";
                    else col1 = "'" + gr1.Cells[13].Text.Trim().ToString() + "'";
                }

                SQuery = "SELECT TRIM(ICODe) AS FSTR,INAME AS PRODUCT,ICODE AS ERPCODE,OPRATE1 AS SIZE_,OPRATE3 AS GSM,UNIT FROM ITEM WHERE TRIM(ICODE) IN (" + col1 + ") ORDER BY ICODE ";
                if (frm_IndType == "12" || frm_IndType == "13")
                    SQuery = "SELECT TRIM(ICODe) AS FSTR,INAME AS PRODUCT,ICODE AS ERPCODE,mat4 as density,mat5 as micron,UNIT FROM ITEM WHERE TRIM(ICODE) IN (" + col1 + ") ORDER BY ICODE ";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek("Select Item", frm_qstr);
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
            case "sg4_RMV":
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
            case "sg4_ROW_ADD":
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
        //fgen.Fn_open_sseek("Select Party Name", frm_qstr); // COMMENTED BY MADHVI ON 28 JULY 2018
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);// BY MADHVI ON 28 JULY 2018

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl10_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_10";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl10.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl11_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_11";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl10.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl12_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_12";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_13";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_14";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_15";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        //  fgen.Fn_open_sseek("Select Deptt ", frm_qstr); // COMMENTED BY MADHVI ON 28 JULY 2018
        fgen.Fn_open_sseek("Select " + lbl15.Text, frm_qstr); // BY MADHVI ON 28 JULY 2018
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_16";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        //  fgen.Fn_open_sseek("Select Deptt ", frm_qstr); // COMMENTED BY MADHVI ON 28 JULY 2018
        fgen.Fn_open_sseek("Select " + lbl16.Text, frm_qstr); // BY MADHVI ON 28 JULY 2018
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {
        //hffield.Value = "BTN_17";
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {
        //hffield.Value = "BTN_18";
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
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
        //fgen.Fn_open_sseek("Select Deptt", frm_qstr);  // COMMENTED BY MADHVI ON 28 JULY 2018
        fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr); // BY MADHVI ON 28 JULY 2018
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl70_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODEX";
        make_qry_4_popup();
        // fgen.Fn_open_sseek("Select Type ", frm_qstr); // COMMENTED BY MADHVI ON 28 JULY 2018
        fgen.Fn_open_sseek("Select " + lbl70.Text, frm_qstr); // BY MADHVI ON 28 JULY 2018
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
                oporow["genum"] = txtlbl2.Text.Trim();
                oporow["gedate"] = txtlbl3.Text.Trim();
                oporow["invno"] = txtlbl5.Text.Trim();
                oporow["invdate"] = fgen.make_def_Date(txtlbl6.Text.Trim(), vardate);
                oporow["refnum"] = txtlbl8.Text.Trim();
                oporow["refdate"] = fgen.make_def_Date(txtlbl9.Text.Trim(), vardate);
                oporow["rec_iss"] = "D";
                oporow["lotno"] = "-";
                oporow["location"] = txtlbl13.Text;
                oporow["revis_no"] = "-";
                oporow["buyer"] = "-";
                oporow["fabtype"] = "-";
                oporow["store_no"] = frm_mbr;
                oporow["acode"] = txtlbl4.Text.Trim();
                oporow["vcode"] = txtlbl4.Text.Trim();
                oporow["gst_pos"] = txtlbl70.Text.Trim();
                oporow["form31"] = txtlbl15.Text.Trim();
                oporow["mode_tpt"] = txtlbl16.Text.Trim();
                oporow["styleno"] = txtlbl17.Text.Trim();
                oporow["mtime"] = txtlbl18.Text.Trim();
                if (txtlbl7.Text.Trim().Length > 2)
                {
                    oporow["vcode"] = txtlbl7.Text.Trim();
                }

                oporow["srno"] = i;
                if (i == 0)
                {
                    oporow["doc_tot"] = fgen.make_double(txtlbl31.Text);
                }
                else
                {
                    oporow["doc_tot"] = 0;
                }
                oporow["morder"] = i + 1;
                oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim();
                oporow["cavity"] = fgen.make_double(txtlbl24.Text.Trim());
                oporow["st_entform"] = txtlbl26.Text;
                oporow["segment_"] = 3;
                oporow["isize"] = txtlbl30.Text;
                if (txtlbl28.Text.ToUpper() == "Y")
                {
                    oporow["segment_"] = 1;
                }
                if (txtlbl26.Text.ToUpper() == "N")
                {
                    oporow["segment_"] = 2;
                }

                //oporow["exc_57f4"] = sg1.Rows[i].Cells[15].Text.Trim();
                //oporow["finvno"] = sg1.Rows[i].Cells[16].Text.Trim();
                //txtlbl5.Text = sg1.Rows[i].Cells[16].Text.Trim();

                oporow["IQTYOUT"] = 0;
                oporow["REJ_RW"] = 0;
                oporow["ACPT_UD"] = 0;
                oporow["rej_sdp"] = 0;
                oporow["idiamtr"] = 0;
                oporow["iweight"] = 0;
                oporow["shots"] = 0;
                oporow["mattype"] = "-";
                oporow["stage"] = "-";
                oporow["finvno"] = "-";
                oporow["rcode"] = "-";
                oporow["o_Deptt"] = "-";
                oporow["freight"] = "-";
                oporow["exc_57f4"] = "-";
                oporow["exc_time"] = "-";
                oporow["IQTY_CHL"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim());
                oporow["IQTY_CHLWT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim());
                oporow["IQTYIN"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim());
                oporow["IQTY_WT"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim());
                oporow["irate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim());
                oporow["ichgs"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim());
                oporow["iamount"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim());
                oporow["exc_Rate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim());
                oporow["cess_percent"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim());
                oporow["exc_amt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim());
                oporow["cess_pu"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim());
                oporow["desc_"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                oporow["btchno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                oporow["btchdt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim() == "-")
                {
                    oporow["btchno"] = txtlbl5.Text.Trim();
                    oporow["btchdt"] = fgen.make_def_Date(txtlbl6.Text.Trim(), Convert.ToDateTime(vardate).ToString("dd/MM/yyyy"));
                }

                //oporow["mr_gdate"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                //oporow["tpt_names"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();

                oporow["mfgdt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                oporow["expdt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();

                oporow["ponum"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                oporow["ordlineno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                string po_dts;
                po_dts = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim(), vardate);
                oporow["podate"] = po_dts;

                string po_dtls;
                string po_srch;
                po_srch = txtlbl4.Text + sg1.Rows[i].Cells[13].Text.Trim() + ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim() + po_dts + ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();

                po_dtls = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(type)||'@'||trim(pr_no)||'@'||to_char(pr_dt,'dd/mm/yyyy') as fstr from pomas where branchcd='" + frm_mbr + "' and type like '5%' and trim(AcodE)||trim(icodE)||ordno||to_char(orddt,'dd/mm/yyyy')||trim(cscode)='" + po_srch + "' ", "fstr");

                if (po_dtls.Trim().Length <= 1)
                {
                    po_srch = txtlbl4.Text + sg1.Rows[i].Cells[13].Text.Trim() + ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim() + po_dts;
                    po_dtls = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(type)||'@'||trim(pr_no)||'@'||to_char(pr_dt,'dd/mm/yyyy') as fstr from pomas where branchcd='" + frm_mbr + "' and type like '5%' and trim(AcodE)||trim(icodE)||ordno||to_char(orddt,'dd/mm/yyyy')='" + po_srch + "' ", "fstr");
                }

                if (po_dtls.Trim().Length > 6)
                {
                    oporow["potype"] = po_dtls.Split('@')[0].ToString();
                    oporow["prnum"] = po_dtls.Split('@')[1].ToString();
                    oporow["rtn_Date"] = fgen.make_def_Date(po_dtls.Split('@')[2].ToString(), vardate);
                }
                else
                {
                    oporow["potype"] = "-";
                    oporow["prnum"] = "-";
                    oporow["rtn_Date"] = txtvchdate.Text.Trim();
                }
                oporow["rgpnum"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                po_dts = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim(), vardate);
                oporow["rgpdate"] = po_dts;



                if (frm_cocd != "SVPL")
                    oporow["col1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();

                oporow["iopr"] = "-";
                oporow["unit"] = lbl27.Text.Substring(0, 2);
                oporow["store"] = "N";
                oporow["inspected"] = "N";
                oporow["pname"] = "-";

                col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(NVL(OPRATE1,'0'))||'~'||TRIM(NVL(OPRATE3,'0')) AS VAL FROM ITEM WHERE ICODE='" + oporow["icode"].ToString().Trim() + "' ", "VAL");
                if (col1 != "0")
                {
                    oporow["PSIZE"] = col1.Split('~')[0];
                    oporow["GSM"] = col1.Split('~')[1];
                }

                if (frm_cocd == "MULT")
                {
                    oporow["store"] = "Y";
                    oporow["inspected"] = "Y";
                    oporow["qc_date"] = vardate;
                    oporow["qcdate"] = vardate;
                    oporow["pname"] = "-";
                    oporow["acpt_ud"] = oporow["iqtyin"];
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
                oporow["naration"] = txtrmk.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");

                // BY MADHVI ON 28 JULY 2018  ---------------
                //oporow["tpt_nameS"] = "-";
                oporow["txb_punit"] = 0;
                oporow["exp_punit"] = 0;
                oporow["billrate"] = 0;
                oporow["rlprc"] = 0;
                oporow["spexc_amt"] = 0;
                // -----------------------------------------
                oDS.Tables[0].Rows.Add(oporow);
            }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        //string curr_dt;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");


        oporow2 = oDS2.Tables[0].NewRow();
        oporow2["BRANCHCD"] = frm_mbr;
        oporow2["TYPE"] = lbl1a.Text;
        oporow2["vchnum"] = frm_vnum;
        oporow2["vchdate"] = txtvchdate.Text.Trim();

        oporow2["Acode"] = txtlbl4.Text;
        //oporow2["cscode"] = txtlbl7.Text.Trim();

        oporow2["invno"] = txtlbl5.Text;
        oporow2["invdate"] = fgen.make_def_Date(txtlbl6.Text.Trim(), vardate);


        //oporow2["post"] = lbl27.Text.Substring(0, 1);




        oporow2["Totqty"] = fgen.make_double(doc_qty.Value);
        if (sg1.Rows[0].Cells[13].Text.Length > 1)
            oporow2["inature"] = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='Y' and trim(upper(type1))=upper(Trim('" + sg1.Rows[0].Cells[13].Text.Trim().Substring(0, 2) + "'))", "name");
        oporow2["cust_amt"] = 0;
        oporow2["ed_Extra"] = "Y";
        oporow2["srno"] = 0;
        oporow2["excb_chg"] = 0;
        oporow2["ed_Rate"] = 0;
        oporow2["cess_Rate"] = 0;
        oporow2["cust_per"] = 0;
        oporow2["shvalue"] = 0;
        oporow2["cst_amt"] = 0;
        oporow2["lst_amt"] = 0;
        oporow2["s_lst"] = 0;
        oporow2["vatschg"] = 0;

        oporow2["taxcode"] = lbl27.Text.Substring(0, 2);
        oporow2["vatschg"] = 0;
        oporow2["shcess_Rate"] = 0;
        oporow2["cst_rate"] = 0;
        oporow2["lst_Rate"] = 0;

        oporow2["pack_amt"] = txtlbl10.Text.toDouble(2);
        oporow2["insu_Amt"] = 0;
        oporow2["frt_amt"] = txtlbl12.Text.toDouble(2);
        oporow2["other"] = txtlbl11.Text.toDouble(2);

        oporow2["rndcess"] = 0;
        oporow2["lessamt"] = 0;
        oporow2["matac"] = "-";

        oporow2["mainitem"] = sg1.Rows[0].Cells[14].Text.Trim();
        oporow2["mainunit"] = sg1.Rows[0].Cells[17].Text.Trim();

        oporow2["t_grno"] = "-";
        oporow2["t_grdt"] = "-";
        oporow2["t_name"] = txtlbl16.Text;
        oporow2["t_vno"] = "-";

        oporow2["finvnum"] = "-";



        oporow2["BE_refno"] = "-";
        oporow2["BE_refdt"] = "-";


        oporow2["frtpay"] = 3;
        oporow2["whname"] = "-";
        oporow2["AMT_SALE"] = fgen.make_double(txtlbl25.Text);
        oporow2["AMT_EXC"] = fgen.make_double(txtlbl27.Text);
        oporow2["RVALUE"] = fgen.make_double(txtlbl29.Text); ;
        oporow2["BILL_TOT"] = fgen.make_double(txtlbl31.Text);

        oDS2.Tables[0].Rows.Add(oporow2);
    }
    //------------------------------------------------------------------------------------
    void save_fun3()
    {
        for (i = 0; i < sg2.Rows.Count - 0; i++)
        {
            if (sg2.Rows[i].Cells[3].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim()) > 0)
            {
                oporow3 = oDS3.Tables[0].NewRow();
                oporow3["BRANCHCD"] = frm_mbr;
                oporow3["TYPE"] = lbl1a.Text;
                oporow3["vchnum"] = frm_vnum;
                oporow3["vchdate"] = txtvchdate.Text.Trim();

                oporow3["ICODE"] = sg2.Rows[i].Cells[3].Text.Trim();
                oporow3["SRNO"] = i;
                oporow3["COREELNO"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t6")).Text.Trim();
                oporow3["KCLREELNO"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                oporow3["REELWIN"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim();
                oporow3["REELWOUT"] = 0;
                oporow3["IRATE"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim().toDouble(2);
                oporow3["JOB_NO"] = "";
                oporow3["REELSPEC1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim();
                oporow3["REELSPEC2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim();
                oporow3["PSIZE"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim().toDouble();
                oporow3["GSM"] = fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim());
                oporow3["ACODE"] = txtlbl4.Text.Trim();
                oporow3["GRADE"] = "-";
                oporow3["REC_ISS"] = "D";
                oporow3["REELHIN"] = 0;
                oporow3["UNLINK"] = "N";
                oporow3["POSTED"] = "N";

                if (frm_cocd == "MASS" || frm_cocd == "MAST")
                {
                    oporow3["POSTED"] = "Y";
                }
                oporow3["JOB_DT"] = "";
                oporow3["STORE_NO"] = frm_mbr;
                oporow3["RINSP_BY"] = "-";
                oporow3["RLOCN"] = "-";
                oporow3["UINSP"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim();
                oporow3["REELMTR"] = "0";

                oDS3.Tables[0].Rows.Add(oporow3);
            }
        }
    }
    //------------------------------------------------------------------------------------
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
    //------------------------------------------------------------------------------------
    void save_fun5()
    {
        for (i = 0; i < sg4.Rows.Count - 0; i++)
        {
            if (((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim().Length > 1)
            {
                oporow5 = oDS5.Tables[0].NewRow();
                oporow5["branchcd"] = frm_mbr;
                oporow5["par_tbl"] = frm_tabname.ToUpper().Trim();
                oporow5["par_fld"] = frm_mbr + lbl1a.Text + frm_vnum + txtvchdate.Text.Trim();
                oporow5["udf_name"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                oporow5["udf_value"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
                oporow5["SRNO"] = i;

                oDS5.Tables[0].Rows.Add(oporow5);
            }
        }
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE FROM type where id='M' and type1 like '0%'  order by type1";
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
            txtlbl29.Style.Add("display", "none");
        }


    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (frm_cocd == "SRPF")
            {
                for (int z = 5; z <= 7; z++)
                {
                    sg2.HeaderRow.Cells[z].Style["display"] = "none";
                    e.Row.Cells[z].Style["display"] = "none";
                }

                {
                    sg2.HeaderRow.Cells[0].Style["display"] = "none";
                    e.Row.Cells[0].Style["display"] = "none";
                    sg2.HeaderRow.Cells[1].Style["display"] = "none";
                    e.Row.Cells[1].Style["display"] = "none";
                    sg2.HeaderRow.Cells[9].Style["display"] = "none";
                    e.Row.Cells[9].Style["display"] = "none";
                    sg2.HeaderRow.Cells[10].Style["display"] = "none";
                    e.Row.Cells[10].Style["display"] = "none";
                    sg2.HeaderRow.Cells[13].Style["display"] = "none";
                    e.Row.Cells[13].Style["display"] = "none";
                    sg2.HeaderRow.Cells[14].Style["display"] = "none";
                    e.Row.Cells[14].Style["display"] = "none";
                }

                for (int z = 10; z <= 12; z++)
                {
                    sg2.HeaderRow.Cells[z].Style["display"] = "none";
                    e.Row.Cells[z].Style["display"] = "none";
                }
                sg2.HeaderRow.Cells[8].Text = "BatchNo";
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnRead_ServerClick(object sender, EventArgs e)
    {
        dt = new DataTable();
        sg1_dt = new DataTable();
        if (frm_cocd == "BONY" || frm_cocd == "SRPF")
        {
            if (txtBarCode.Value.Trim().Length < 20) return;
            dt2 = new DataTable();
            if (frm_cocd == "SRPF")
            {
                dt2 = fgen.getdata(frm_qstr, frm_cocd, "Select a.*,c.iname,c.cpartno,c.cdrgno,c.unit,c.hscode,c.iname as erp_name,c.icode as erp_code from finprim.scratch a,finsrpf.item c where trim(A.col3)=trim(c.cpartno) and a.branchcd||a.type||a.vchnum||to_Char(a.Vchdate,'dd/mm/yyyy')='" + txtBarCode.Value.Trim().Substring(0, 20) + "'");

                create_tab2();
                sg2_dr = null;
                i = 1;
                if (dt2.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt2.Rows)
                    {
                        sg2_dr = sg2_dt.NewRow();

                        sg2_dr["sg2_srno"] = i;
                        sg2_dr["sg2_h1"] = dr["erp_code"].ToString().Trim();
                        sg2_dr["sg2_h2"] = dr["erp_code"].ToString().Trim();
                        sg2_dr["sg2_h3"] = "";
                        sg2_dr["sg2_h4"] = "";
                        sg2_dr["sg2_h5"] = "";

                        sg2_dr["sg2_f1"] = dr["erp_code"].ToString().Trim();
                        sg2_dr["sg2_f2"] = dr["erp_name"].ToString().Trim();
                        sg2_dr["sg2_f3"] = "";
                        sg2_dr["sg2_f4"] = "";
                        sg2_dr["sg2_f5"] = "";

                        sg2_dr["sg2_t1"] = dr["col2"].ToString().Trim();
                        sg2_dr["sg2_t2"] = dr["vchnum"].ToString().Trim();
                        sg2_dr["sg2_t3"] = dr["col2"].ToString().Trim();
                        sg2_dr["sg2_t4"] = dr["col4"].ToString().Trim();
                        sg2_dr["sg2_t5"] = 0;
                        sg2_dr["sg2_t6"] = "-";
                        sg2_dr["sg2_t7"] = "-";
                        sg2_dr["sg2_t8"] = dr["col4"].ToString().Trim();
                        sg2_dr["sg2_t9"] = i.ToString(); ;
                        sg2_dr["sg2_t10"] = "";


                        sg2_dt.Rows.Add(sg2_dr);
                        i++;
                    }
                }
                sg2_add_blankrows();
                ViewState["sg2"] = sg2_dt;
                sg2.DataSource = sg2_dt;
                sg2.DataBind();
                dt.Dispose();
                sg2_dt.Dispose();
                return;
            }
            else
            {
                dt2 = fgen.getdata(frm_qstr, frm_cocd, "Select a.*,b.iname,b.cpartno,b.unit,b.cdrgno,b.hscode from finprim.scratch a,finprim.item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||a.vchnum||to_Char(a.Vchdate,'dd/mm/yyyy')='" + txtBarCode.Value.Trim().Substring(0, 20) + "'");
            }

            if (dt2.Rows.Count > 0)
            {
                #region for gridview 1
                if (ViewState["sg1"] != null)
                {
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
                        sg1_dr["sg1_t21"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();

                        //sg1_dt.Rows.Add(sg1_dr);
                    }

                    for (int d = 0; d < dt2.Rows.Count; d++)
                    {
                        dt3 = new DataTable();
                        //dt3 = fgen.getdata(frm_qstr, frm_cocd, "select a.icode,a.iname,a.cpartno,a.cdrgno,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from finprim.item a,finprim.typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.icode) ='" + dt2.Rows[d]["col3"].ToString().Trim() + "' ");
                        //dt3 = fgen.getdata(frm_qstr, frm_cocd, "select a.icode,a.iname,a.cpartno,a.cdrgno,a.unit,a.hscode from finprim.item a where trim(a.icode) ='" + dt2.Rows[d]["col3"].ToString().Trim() + "' ");
                        sg1_dr = sg1_dt.NewRow();
                        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        sg1_dr["sg1_h1"] = dt2.Rows[d]["col3"].ToString().Trim();
                        sg1_dr["sg1_h2"] = col1;
                        sg1_dr["sg1_h3"] = dt2.Rows[d]["icode"].ToString().Trim();
                        sg1_dr["sg1_h4"] = dt2.Rows[d]["col2"].ToString().Trim();
                        sg1_dr["sg1_h5"] = dt2.Rows[d]["vchdate"].ToString().Trim();
                        sg1_dr["sg1_h6"] = "-";
                        sg1_dr["sg1_h7"] = "-";
                        sg1_dr["sg1_h8"] = "-";
                        sg1_dr["sg1_h9"] = "-";
                        sg1_dr["sg1_h10"] = "-";

                        sg1_dr["sg1_f1"] = dt2.Rows[d]["col3"].ToString().Trim();

                        sg1_dr["sg1_f2"] = dt2.Rows[d]["iname"].ToString().Trim();
                        sg1_dr["sg1_f3"] = dt2.Rows[d]["cpartno"].ToString().Trim();
                        sg1_dr["sg1_f4"] = dt2.Rows[d]["cdrgno"].ToString().Trim();
                        sg1_dr["sg1_f5"] = dt2.Rows[d]["unit"].ToString().Trim();

                        //if (dt3.Rows.Count > 0)
                        {
                            dt4 = new DataTable();
                            dt4 = fgen.getdata(frm_qstr, frm_cocd, "SELECT * FROM FINPRIM.TYPEGRP WHERE TRIM(acref)='" + dt2.Rows[d]["hscode"].ToString().Trim() + "' ");
                            if (dt4.Rows.Count > 0)
                            {
                                if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                                {
                                    sg1_dr["sg1_t7"] = dt4.Rows[0]["num4"].ToString().Trim();
                                    sg1_dr["sg1_t8"] = dt4.Rows[0]["num5"].ToString().Trim();
                                }
                                else
                                {
                                    sg1_dr["sg1_t7"] = dt4.Rows[0]["num6"].ToString().Trim();
                                    sg1_dr["sg1_t8"] = "0";
                                }
                            }
                        }

                        sg1_dr["sg1_t1"] = dt2.Rows[d]["col4"].ToString().Trim();
                        sg1_dr["sg1_t2"] = 0;
                        sg1_dr["sg1_t3"] = dt2.Rows[d]["col4"].ToString().Trim();
                        sg1_dr["sg1_t4"] = 0;

                        sg1_dr["sg1_t9"] = "";
                        sg1_dr["sg1_t10"] = dt2.Rows[d]["col2"].ToString().Trim(); ;
                        sg1_dr["sg1_t11"] = Convert.ToDateTime(dt2.Rows[d]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        sg1_dr["sg1_t12"] = fgen.make_def_Date(dt2.Rows[d]["col17"].ToString().Trim(), vardate);
                        sg1_dr["sg1_t13"] = fgen.make_def_Date(dt2.Rows[d]["col18"].ToString().Trim(), vardate);

                        string mpo_Dt = fgen.seek_iname(frm_qstr, frm_cocd, "select * From (Select ordno||'~'||orddt||'~'||prate from pomas where branchcd='" + frm_mbr + "' and type like '5%' and trim(Acode)='" + txtlbl4.Text.Trim() + "' and trim(icode)='" + dt2.Rows[d]["icode"].ToString().Trim() + "' and pflag<>1 order by orddt desc) where rownum<2", "");
                        if (mpo_Dt.Length > 2)
                        {
                            foreach (var v in mpo_Dt.Split('~'))
                            {
                                sg1_dr["sg1_t14"] = v[0].ToString().Trim();
                                sg1_dr["sg1_t15"] = "";
                                sg1_dr["sg1_t16"] = fgen.make_def_Date(v[1].ToString().Trim(), vardate);
                                sg1_dr["sg1_t5"] = v[2].ToString().Trim();
                            }
                        }
                        sg1_dt.Rows.Add(sg1_dr);

                        txtlbl5.Text = dt2.Rows[d]["col9"].ToString().Trim();
                        txtlbl6.Text = dt2.Rows[d]["col10"].ToString().Trim();
                    }
                    dt.Dispose();
                }
                sg1_add_blankrows();

                ViewState["sg1"] = sg1_dt;
                sg1.DataSource = sg1_dt;
                sg1.DataBind();
                sg1_dt.Dispose();
                ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                #endregion
                setColHeadings();
                setGST();
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnSticker_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SPrint";
        make_qry_4_popup();
        //  fgen.Fn_open_sseek("Select Type for Print", frm_qstr); // COMMENTED BY MADHVI ON 28 JULY 2018
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr); // BY MADHVI ON 28 JULY 2018
    }
    protected void sg1_t9_TextChanged(object sender, EventArgs e)
    {
        //fgen.msg("-", "AMSG", "H11111111111i");
    }
    protected void btnGridPop_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        if (hf1.Value.Contains("sg1_t9_"))
        {
            hffield.Value = "sg1_t9";
            hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_sg1_t9_", "");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Reason", frm_qstr);
        }
        if (hf1.Value.Contains("sg1_t5_"))
        {
            if (frm_vty == "09")
            {
                hffield.Value = "PONO";
                hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_sg1_t5_", "");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                SQuery = "SELECT trim(a.ordno)||'-'||TO_CHAR(a.orddt,'dd/mm/yyyy') as fstr,trim(a.ordno) as ponum,a.prate as rate,to_char(a.orddt,'dd/mm/yyyy') as podt,b.iname as product,b.cpartno,c.aname as party,TO_CHAR(A.ORDDT,'YYYYMMDD') AS VDD from POMAS A,ITEM B,FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '5%' AND A.ORDDT " + DateRange + " AND TRIM(A.ACODE)='" + txtlbl4.Text.Trim() + "' ORDER BY VDD DESC";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek("Select Po Number", frm_qstr);
            }
        }
    }
}