using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using MessagingToolkit.QRCode.Codec;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Drawing;
using System.Net.Mail;


public partial class om_pinv_entry2 : System.Web.UI.Page
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
    string frm_tab_vchr;
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
                doc_addl.Value = "N";
                doc_hoso.Value = "N";
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
                hf150.Value = "";
                if (fgen.getOption(frm_qstr, frm_cocd, "W0150", "OPT_ENABLE") == "Y")
                {
                    hf150.Value = fgen.getOption(frm_qstr, frm_cocd, "W0150", "OPT_PARAM");
                }

                hf151.Value = fgen.getOption(frm_qstr, frm_cocd, "W0151", "OPT_ENABLE");
                fgenMV.Fn_Set_Mvar(frm_qstr, "REQ_APP", fgen.getOption(frm_qstr, frm_cocd, "W0096", "OPT_ENABLE"));

                brPrefixWithInvNo.Value = "N";
                hfw122.Value = fgen.getOption(frm_qstr, frm_cocd, "W0122", "OPT_ENABLE");
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();

                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_OPEN_IN_EDIT") == "Y")
                    editFunction(fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"));
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
        int vv = 3;
        if (frm_formID == "F70122") vv = 8;
        for (int sR = 0; sR < sg1.Columns.Count; sR++)
        {
            string orig_name;
            double tb_Colm;
            tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
            orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

            for (int K = 0; K < sg1.Rows.Count; K++)
            {
                sg1.Columns[0].HeaderStyle.CssClass = "hidden";
                sg1.Rows[K].Cells[0].CssClass = "hidden";

                #region hide hidden columns

                for (int i = vv; i < 10; i++)
                {
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[K].Cells[i].CssClass = "hidden";
                }
                #endregion

                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

                ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");

                if (frm_formID == "F70112" || frm_formID == "F70116")
                {
                    if (hf150.Value != "")
                    {
                        if (hf150.Value.toDouble() >= frm_ulvl.toDouble())
                        {
                            ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Remove("readonly");
                        }
                        else ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Add("readonly", "readonly");
                    }
                    else ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Add("readonly", "readonly");
                }

                //// rate edit to be allow for SGRP for some time
                if ((frm_cocd == "SGRP" || frm_cocd == "UATS" || hf151.Value == "Y") && (Prg_Id == "F70112"))
                {
                    hf150.Value = "1";
                    ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Remove("readonly");
                }
                if (frm_cocd == "SGRP" || frm_cocd == "UATS")
                {
                    //if (((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Text.toDouble() == 0)
                    ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Remove("readonly");
                }
                if (frm_formID == "F70108" || frm_formID == "F70110")
                    ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Remove("readonly");
                txtlbl72.Attributes.Add("readonly", "readonly");
                txtlbl73.Attributes.Add("readonly", "readonly");
                //((TextBox)sg1.Rows[K].FindControl("sg1_t7")).Attributes.Add("readonly", "readonly");
                //((TextBox)sg1.Rows[K].FindControl("sg1_t8")).Attributes.Add("readonly", "readonly");
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
                if (sR > 10)
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



        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMFID");
        switch (Prg_Id)
        {
            case "F70108":
            case "F70110":
            case "F70112":
            case "F70116":
            case "F70122":
                tab2.Visible = false;
                tab3.Visible = false;
                //tab4.Visible = false;
                tab5.Visible = false;
                //tab6.InnerText = "Terms & Condition";
                //invDiv.Style.Add("display", "none");
                multDiv.Style.Add("display", "none");
                break;
        }

        divRcm.Visible = false;
        if (lbl1a.Text == "56")
        {
            divRcm.Visible = true;
            divRmk.Attributes.Add("class", "col-md-6");
        }
        else divRmk.Attributes.Add("class", "col-md-12");

        if (lbl1a.Text == "58" || lbl1a.Text == "59")
        {
            chkITC.Checked = false;
            chkITC.Visible = false;
            chkTCS.Visible = true;

            Label3.Visible = true;
            ImageButton1.Visible = true;
            txtbizgrp.Visible = true;
        }
        else
        {
            Label3.Visible = false;
            ImageButton1.Visible = false;
            txtbizgrp.Visible = false;
            chkTCS.Visible = false;
        }

        fgen.SetHeadingCtrl(this.Controls, dtCol);

        setGST();
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
        liBarcode.Visible = false;
        switch (Prg_Id)
        {
            case "F70116":
            case "F70122":
                frm_tab_ivch = "WB_PV_DTL";
                frm_tab_sale = "WB_PV_HEAD";
                frm_tab_vchr = "VOUCHER";
                liBarcode.Visible = true;
                break;
            case "F70108":
            case "F70110":
            case "F70112":
                frm_tab_ivch = "IVOUCHER";
                frm_tab_sale = "WB_PV_HEAD";
                frm_tab_vchr = "VOUCHER";
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TAB_IVCH", frm_tab_ivch);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TAB_SALE", frm_tab_sale);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TAB_VCHR", frm_tab_vchr);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", lbl1a.Text.Trim());

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
                SQuery = "Select Type1 as fstr,Name,type1 from type where id='A' order by name";
                break;

            case "BTN_19":
                SQuery = "SELECT '10' as fstr,'As Applicable' as NAME,'10' as Code FROM dual ";
                break;

            case "BTN_BIZ":
                SQuery = "Select Type1 as fstr,Name,type1 from typegrp where branchcd!='DD' and id='BZ' order by name";
                SQuery = "Select Type1 as fstr,Name,type1 from type where id='$' order by type1";
                break;
            case "BTN_CC1":
                SQuery = "Select Type1 as fstr,Name,type1 from typegrp where branchcd!='DD' and id='L1' order by name";
                break;
            case "BTN_TAX1":
                SQuery = "Select Type1 as fstr,Name,type1 from type where id='}' AND TYPE1<='50' order by type1";
                break;
            case "BTN_CC2":
                SQuery = "Select Type1 as fstr,Name,type1 from typegrp where branchcd!='DD' and id='L2' order by name";
                break;
            case "BTN_CC3":
                SQuery = "Select Type1 as fstr,Name,type1 from typegrp where branchcd!='DD' and id='L3' order by name";
                break;



            case "BTN_20":

                break;
            case "BTN_21":

                break;
            case "BTN_22":

                break;
            case "BTN_23":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1,rate  FROM TYPE WHERE ID='A' order by name ";
                break;
            case "TACODE":
                switch (frm_vty)
                {
                    case "31":
                    case "32":
                        SQuery = "select A.acode as fstr,replacE(ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1 and trim(nvl(a.GRP,'-'))='06' order by A.aname ";
                        SQuery = "SELECT * FROM (select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.deac_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1  union all select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.deac_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a ,(Select type1,name from type where id='Z') b,(Select trim(acode) as acode,sum(Dramt)-sum(Cramt) as tot from recdata where branchcd='" + frm_mbr + "' group by trim(AcodE) having sum(Dramt)-sum(Cramt)<>0)  c where trim(a.grp)=trim(B.type1)and trim(a.acode)=trim(c.acode) and length(Trim(nvl(a.deac_by,'-')))>1 ) WHERE trim(nvl(GRP,'-')) in ('06','02')";
                        break;
                    case "50":
                        ord_br_Str = "a.branchcd='" + frm_mbr + "'";
                        cond = " (SELECT acode FROM (SELECT x.acode,SUM(X.aBC) AS CNT FROM (select trim(a.type)||a.vchnum||to_char(a.vchdate,'yyyymm') as vchnum,a.type,1 AS ABC,trim(a.acode) as acode from ivoucher a where branchcd='" + frm_mbr + "' AND a.type like '0%' and a.type!='07' and a.vchdate " + DateRange + " and a.store='Y' UNION ALL select trim(a.fabtype)||nvl(a.tc_no,'-')||to_char(a.refdate,'yyyymm') as genum,a.type,-1 AS ABC,trim(a.acode) as acode from wb_pv_DTL a where branchcd='" + frm_mbr + "' and substr(a.type,1,1)=substr('" + frm_vty + "',1,1)  and a.vchdate " + DateRange + " ) X GROUP BY X.acode) WHERE CNT>0) X";
                        SQuery = "SELECT distinct a.ACODE AS FSTR,replacE(b.ANAME,'''','`') AS PARTY,a.ACODE AS CODE,b.Grp,b.ADDR1,b.ADDR2,b.ADDR3,b.staten as state,b.Pay_num,b.GST_no FROM IVCHCTRL a ,FAMST b, " + cond + " WHERE a.branchcd='" + frm_mbr + "' and a.type in ('02','05') and a.type!='0U' and a.type!='0S' and a.vchdate " + DateRange + " and trim(a.acode)=trim(b.acode) and trim(a.acode)=trim(x.acode) ORDER BY A.ACODE,party ";
                        break;
                    case "53":
                        ord_br_Str = "a.branchcd='" + frm_mbr + "'";
                        cond = " (SELECT acode FROM (SELECT x.acode,SUM(X.aBC) AS CNT FROM (select trim(a.type)||a.vchnum||to_char(a.vchdate,'yyyymm') as vchnum,a.type,1 AS ABC,trim(a.acode) as acode from ivoucher a where branchcd='" + frm_mbr + "' AND a.type like '0%' and a.type='04' and a.vchdate " + DateRange + " and a.store='Y' UNION ALL select trim(a.fabtype)||nvl(a.tc_no,'-')||to_char(a.refdate,'yyyymm') as genum,a.type,-1 AS ABC,trim(a.acode) as acode from wb_pv_DTL a where branchcd='" + frm_mbr + "' and substr(a.type,1,1)=substr('" + frm_vty + "',1,1)  and a.vchdate " + DateRange + " ) X GROUP BY X.acode) WHERE CNT>0) X";
                        SQuery = "SELECT distinct a.ACODE AS FSTR,replacE(b.ANAME,'''','`') AS PARTY,a.ACODE AS CODE,b.Grp,b.ADDR1,b.ADDR2,b.ADDR3,b.staten as state,b.Pay_num,b.GST_no FROM IVCHCTRL a ,FAMST b, " + cond + " WHERE a.branchcd='" + frm_mbr + "' and a.type='04' and a.vchdate " + DateRange + " and trim(a.acode)=trim(b.acode) and trim(a.acode)=trim(x.acode) ORDER BY A.ACODE,party ";
                        break;

                    case "51":
                        ord_br_Str = "a.branchcd='" + frm_mbr + "'";
                        cond = " (SELECT acode FROM (SELECT x.acode,SUM(X.aBC) AS CNT FROM (select trim(a.type)||a.vchnum||to_char(a.vchdate,'yyyymm') as vchnum,a.type,1 AS ABC,trim(a.acode) as acode from ivoucher a where branchcd='" + frm_mbr + "' AND a.type like '0%' and a.type!='07' and a.vchdate " + DateRange + " and a.store='Y' UNION ALL select trim(a.fabtype)||nvl(a.tc_no,'-')||to_char(a.refdate,'yyyymm') as genum,a.type,-1 AS ABC,trim(a.acode) as acode from wb_pv_DTL a where branchcd='" + frm_mbr + "' and substr(a.type,1,1)=substr('" + frm_vty + "',1,1) and a.vchdate " + DateRange + " ) X GROUP BY X.acode) WHERE CNT>0) X";
                        SQuery = "SELECT distinct a.ACODE AS FSTR,replacE(b.ANAME,'''','`') AS PARTY,a.ACODE AS CODE,b.Grp,b.ADDR1,b.ADDR2,b.ADDR3,b.staten as state,b.Pay_num,b.GST_no FROM IVCHCTRL a ,FAMST b, " + cond + " WHERE a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.type not in ('02','05') and a.type!='0U' and a.type!='0S' and a.vchdate " + DateRange + " and trim(a.acode)=trim(b.acode) and trim(a.acode)=trim(x.acode) ORDER BY A.ACODE,party ";
                        break;
                    case "55":
                        ord_br_Str = "a.branchcd='" + frm_mbr + "'";
                        cond = " (SELECT acode FROM (SELECT x.acode,SUM(X.aBC) AS CNT FROM (select trim(a.type)||a.vchnum||to_char(a.vchdate,'yyyymm') as vchnum,a.type,1 AS ABC,trim(a.acode) as acode from ivoucher a where branchcd='" + frm_mbr + "' AND a.type like '0%' and a.type!='07' and a.vchdate " + DateRange + " and a.store='Y' UNION ALL select trim(a.fabtype)||nvl(a.tc_no,'-')||to_char(a.refdate,'yyyymm') as genum,a.type,-1 AS ABC,trim(a.acode) as acode from wb_pv_DTL a where branchcd='" + frm_mbr + "' and substr(a.type,1,1)=substr('" + frm_vty + "',1,1) and a.vchdate " + DateRange + " ) X GROUP BY X.acode) WHERE CNT>0) X";
                        SQuery = "SELECT distinct a.ACODE AS FSTR,replacE(b.ANAME,'''','`') AS PARTY,a.ACODE AS CODE,b.Grp,b.ADDR1,b.ADDR2,b.ADDR3,b.staten as state,b.Pay_num,b.GST_no FROM IVCHCTRL a ,FAMST b, " + cond + " WHERE a.branchcd='" + frm_mbr + "' and a.type='0S' and a.vchdate " + DateRange + " and trim(a.acode)=trim(b.acode) and trim(a.acode)=trim(x.acode) ORDER BY A.ACODE,party ";
                        break;
                    case "56":
                        ord_br_Str = "a.branchcd='" + frm_mbr + "'";
                        cond = " (SELECT acode FROM (SELECT x.acode,SUM(X.aBC) AS CNT FROM (select trim(a.type)||a.vchnum||to_char(a.vchdate,'yyyymm') as vchnum,a.type,1 AS ABC,trim(a.acode) as acode from ivoucher a where branchcd='" + frm_mbr + "' AND a.type like '07%' and a.vchdate " + DateRange + " and a.store='Y' UNION ALL select trim(a.fabtype)||nvl(a.tc_no,'-')||to_char(a.refdate,'yyyymm') as genum,a.type,-1 AS ABC,trim(a.acode) as acode from wb_pv_DTL a where branchcd='" + frm_mbr + "' and substr(a.type,1,1)=substr('" + frm_vty + "',1,1) and a.vchdate " + DateRange + " ) X GROUP BY X.acode) WHERE CNT>0) X";
                        SQuery = "SELECT distinct a.ACODE AS FSTR,replacE(b.ANAME,'''','`') AS PARTY,a.ACODE AS CODE,b.Grp,b.ADDR1,b.ADDR2,b.ADDR3,b.staten as state,b.Pay_num,b.GST_no FROM IVCHCTRL a ,FAMST b, " + cond + " WHERE a.branchcd='" + frm_mbr + "' and a.type like '07%' and a.type!='0S' and a.vchdate " + DateRange + " and trim(a.acode)=trim(b.acode) and trim(a.acode)=trim(x.acode) ORDER BY A.ACODE,party ";
                        break;
                    case "58":
                    case "59":
                        //SQuery = "select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.staten,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1 and trim(nvl(a.GRP,'-')) in ('16','02') order by A.aname ";
                        SQuery = "SELECT * FROM (select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.deac_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1  union all select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.deac_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a ,(Select type1,name from type where id='Z') b,(Select trim(acode) as acode,sum(Dramt)-sum(Cramt) as tot from recdata where branchcd='" + frm_mbr + "' group by trim(AcodE) having sum(Dramt)-sum(Cramt)<>0)  c where trim(a.grp)=trim(B.type1)and trim(a.acode)=trim(c.acode) and length(Trim(nvl(a.deac_by,'-')))>1 ) WHERE trim(nvl(GRP,'-')) in ('16','02')";
                        break;
                    case "5B":
                        SQuery = "select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.staten,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1 and trim(nvl(a.GRP,'-')) in ('06','02') and trim(nvl(GstRevChg,'-'))='Y' and length(trim(nvl(gst_no,'-')))>5 order by A.aname ";
                        SQuery = "SELECT * FROM (select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.deac_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1 and trim(nvl(A.GstRevChg,'-'))='Y' and length(trim(nvl(A.gst_no,'-')))>5 union all select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.deac_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a ,(Select type1,name from type where id='Z') b,(Select trim(acode) as acode,sum(Dramt)-sum(Cramt) as tot from recdata where branchcd='" + frm_mbr + "' group by trim(AcodE) having sum(Dramt)-sum(Cramt)<>0)  c where trim(a.grp)=trim(B.type1)and trim(a.acode)=trim(c.acode) and length(Trim(nvl(a.deac_by,'-')))>1 and trim(nvl(GstRevChg,'-'))='Y' and length(trim(nvl(gst_no,'-')))>5) WHERE trim(nvl(GRP,'-')) in ('06','02')";
                        break;
                    case "5A":
                        SQuery = "select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.staten,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1 and trim(nvl(a.GRP,'-')) in ('06','02') order by A.aname ";
                        SQuery = "SELECT * FROM (select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.deac_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1 and length(trim(nvl(A.gst_no,'-')))>5 union all select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.deac_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a ,(Select type1,name from type where id='Z') b,(Select trim(acode) as acode,sum(Dramt)-sum(Cramt) as tot from recdata where branchcd='" + frm_mbr + "' group by trim(AcodE) having sum(Dramt)-sum(Cramt)<>0)  c where trim(a.grp)=trim(B.type1)and trim(a.acode)=trim(c.acode) and length(Trim(nvl(a.deac_by,'-')))>1 and length(trim(nvl(gst_no,'-')))>5) WHERE trim(nvl(GRP,'-')) in ('06','02')";
                        break;
                    case "57":
                        SQuery = "select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.staten,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1 and trim(nvl(a.GRP,'-')) in ('06','02') and trim(nvl(GstRevChg,'-'))='Y' order by A.aname ";
                        SQuery = "SELECT * FROM (select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.deac_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1 and trim(nvl(A.GstRevChg,'-'))='Y' and length(trim(nvl(A.gst_no,'-')))>5 union all select A.acode as fstr,replacE(a.ANAME,'''','`') as Account_Name,a.acode as ERP_Acode,a.Grp,A.Addr1 as Address_l1,a.addr2 as Address_l2,b.Name,a.deac_by,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a ,(Select type1,name from type where id='Z') b,(Select trim(acode) as acode,sum(Dramt)-sum(Cramt) as tot from recdata where branchcd='" + frm_mbr + "' group by trim(AcodE) having sum(Dramt)-sum(Cramt)<>0)  c where trim(a.grp)=trim(B.type1)and trim(a.acode)=trim(c.acode) and length(Trim(nvl(a.deac_by,'-')))>1 and trim(nvl(GstRevChg,'-'))='Y' and length(trim(nvl(gst_no,'-')))>5) WHERE trim(nvl(GRP,'-')) in ('06','02')";
                        break;
                    case "5P":
                        ord_br_Str = "a.branchcd='" + frm_mbr + "'";
                        cond = " (SELECT acode FROM (SELECT x.acode,SUM(X.aBC) AS CNT FROM (select trim(a.type)||a.vchnum||to_char(a.vchdate,'yyyymm') as vchnum,a.type,1 AS ABC,trim(a.acode) as acode from ivoucher a where branchcd='" + frm_mbr + "' AND a.type like '05%' and a.vchdate " + DateRange + " and a.store='Y' UNION ALL select trim(a.fabtype)||nvl(a.tc_no,'-')||to_char(a.refdate,'yyyymm') as genum,a.type,-1 AS ABC,trim(a.acode) as acode from wb_pv_DTL a where branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " ) X GROUP BY X.acode) WHERE CNT>0) X";
                        SQuery = "SELECT distinct a.ACODE AS FSTR,replacE(b.ANAME,'''','`') AS PARTY,a.ACODE AS CODE,b.Grp,b.ADDR1,b.ADDR2,b.ADDR3,b.staten as state,b.Pay_num,b.GST_no FROM IVCHCTRL a ,FAMST b, " + cond + " WHERE a.branchcd='" + frm_mbr + "' and a.type like '05%' and a.vchdate " + DateRange + " and trim(a.acode)=trim(b.acode) and trim(a.acode)=trim(x.acode) ORDER BY A.ACODE,party ";
                        break;
                    case "5S":
                        ord_br_Str = "a.branchcd='" + frm_mbr + "'";
                        cond = " (SELECT acode FROM (SELECT x.acode,SUM(X.aBC) AS CNT FROM (select trim(a.type)||a.vchnum||to_char(a.vchdate,'yyyymm') as vchnum,a.type,1 AS ABC,trim(a.acode) as acode from ivoucher a where branchcd='" + frm_mbr + "' AND a.type like '0S%' and a.vchdate " + DateRange + " and a.store='Y' UNION ALL select trim(a.fabtype)||nvl(a.tc_no,'-')||to_char(a.refdate,'yyyymm') as genum,a.type,-1 AS ABC,trim(a.acode) as acode from wb_pv_DTL a where branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " ) X GROUP BY X.acode) WHERE CNT>0) X";
                        SQuery = "SELECT distinct a.ACODE AS FSTR,replacE(b.ANAME,'''','`') AS PARTY,a.ACODE AS CODE,b.ADDR1,b.ADDR2,b.ADDR3,b.staten as state,b.Pay_num,b.GST_no FROM IVCHCTRL a ,FAMST b, " + cond + " WHERE a.branchcd='" + frm_mbr + "' and a.type like '0S%' and a.vchdate " + DateRange + " and trim(a.acode)=trim(b.acode) and trim(a.acode)=trim(x.acode) ORDER BY A.ACODE,party ";
                        break;
                }
                if (frm_formID == "F70112")
                {
                    if (frm_vty == "50" || frm_vty == "51" || frm_vty == "5A" || frm_vty == "56")
                    {
                        SQuery = "SELECT distinct b.ACODE AS FSTR,replacE(b.ANAME,'''','`') AS PARTY,b.ACODE AS CODE,b.Grp,b.ADDR1,b.ADDR2,b.ADDR3,b.staten as state,b.Pay_num,b.GST_no FROM FAMST b where length(Trim(nvl(b.deac_by,'-')))<=1 order by b.ACODE ";
                    }
                }
                break;
            case "PICK_MRR":
                cond = "0";
                string extraCond = "";
                switch (frm_vty)
                {
                    case "5P":
                        cond = "05";
                        break;
                    case "5S":
                        cond = "0S";
                        break;
                    case "50":
                        extraCond = "and a.type in ('02','05')";
                        break;
                }
                SQuery = "SELECT DISTINCT a.type||A.Vchnum||to_char(A.Vchdate,'dd/mm/yyyy')||trim(a.Acode) as Fstr,A.Vchnum AS MRR_Number,to_Char(a.Vchdate,'dd/mm/yyyy') as MRR_Date,B.Aname as Supplier,a.Invno,to_chaR(a.Invdate,'dd/mm/yyyy') as Inv_Dt,a.refnum,a.Type,to_Char(a.vchdate,'yyyymmdd') as VDD  FROM ivoucher a, famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.BRANCHCD='" + frm_mbr + "' and a.TYPE like '" + cond + "%' and a.type!='0U' and trim(a.acode) ='" + txtlbl4.Text + "' " + extraCond + " AND (a.type||a.vchnum||to_char(a.vchdate,'yyyymm')) IN (SELECT VCHNUM FROM (SELECT X.VCHNUM,SUM(X.aBC) AS CNT FROM (select trim(a.type)||a.vchnum||to_char(a.vchdate,'yyyymm') as vchnum,a.type,1 AS ABC from ivoucher a where branchcd='" + frm_mbr + "' AND a.type like '0%' and a.vchdate " + DateRange + " and trim(a.Acode)='" + txtlbl4.Text + "' and a.store='Y' " + extraCond + " UNION ALL select trim(a.fabtype)||nvl(a.tc_no,'-')||to_char(a.refdate,'yyyymm') as genum,a.type,-1 AS ABC from wb_pv_DTL a where branchcd='" + frm_mbr + "' and substr(a.type,1,1)=substr('" + frm_vty + "',1,1) and a.vchdate " + DateRange + " and trim(a.Acode)='" + txtlbl4.Text + "' ) X GROUP BY X.VCHNUM) WHERE CNT>0) order by vdd";
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

                SQuery = "select a.Fstr,max(a.Iname)as Item_Name,a.ERP_code,(case when length(trim(max(a.Cpartno)))>2 then max(a.Cpartno) else b.cpartno end) as Part_no,max(a.Irate) As Irate,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as PO_No,a.Fstr as SO_link,max(a.cdisc) as CDisc,max(a.iexc_Addl) as iexc_Addl,max(a.sd) as frt_pu,max(a.ipack) as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty,olineno,b.packsize as std_pack from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,pordno,(Case when length(trim(nvl(desc9,'-')))>1 then desc9 else ciname end) as Iname,trim(Icode) as ERP_code,Cpartno,Irate,Qtyord,0 as Soldqty,nvl(Cdisc,0) as Cdisc,nvl(iexc_addl,0) as Iexc_Addl,nvl(sd,0) as sd ,nvl(ipack,0) as ipack,trim(nvl(cdrgno,'-')) as olineno from somas where " + ord_br_Str + " and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "' and trim(icat)!='Y' and trim(app_by)!='-'  union all SELECT to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(Icode) as fstr,null as pordno,null as Iname,trim(Icode) as ERP_code,null as Cpartno,0 as Irate,0 as Qtyord,iqtyout as Soldqty,0 as Cdisc,0 as iexc_Addl,0 as sd,0 as ipack,nvl(revis_no,'-') AS linno  from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "')a,item b where trim(a.erp_code)=trim(B.icode)  group by a.olineno,a.fstr,a.ERP_code,b.unit,b.hscode,b.cpartno,b.packsize having (case when sum(a.Qtyord)>0 then sum(a.Qtyord)-sum(a.Soldqty) else max(a.irate) end)>0 order by Item_Name,a.fstr";
                cond = "";
                if (Prg_Id == "F70112")
                {
                    cond = "AND SUBSTR(A.ICODE,1,2)='59'";
                    //cond = "";
                    SQuery = "select a.ICODE AS FSTR,A.INAME as Item_Name,a.ICODE AS ERP_code,A.CPARTNO as Part_no,A.IRATE As Irate,0 as Balance_Qty,A.Unit,A.hscode,'-' as Doc_No,'-' as Doc_dt,0 as CDisc,0 as iexc_Addl,0 as frt_pu,0 as pkchg_pu,0 as Qty_Ord,0 as Sold_Qty,A.packsize as std_pack,0 AS olineno,'-' as po_no from ITEM A WHERE LENGTH(TRIM(ICODE))>4 " + cond + " and length(Trim(nvl(deac_by,'-')))<2 ORDER BY A.ICODE desc,A.INAME";
                }
                if (Prg_Id == "F70108" || Prg_Id == "F70110")
                {
                    if (lbl1a.Text.Substring(0, 1) == "3")
                    {
                        SQuery = "select a.invno||to_char(a.invdate,'dd/mm/yyyy')||a.ICODE AS FSTR,B.iname as Item_Name,a.ICODE AS ERP_code,B.cpartno as Part_no,A.IRATE As Irate,iqtyin as Qty,'-' as Unit,b.hscode,a.invno as Doc_No,to_char(a.invdate,'dd/mm/yyyy') as Doc_dt,0 as CDisc,0 as iexc_Addl,0 as frt_pu,0 as pkchg_pu,0 as Qty_Ord,0 as Sold_Qty,0 as std_pack,0 AS olineno from ivoucher A,item b WHERE trim(a.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and trim(A.acode)='" + txtlbl4.Text + "' and a.store='Y' union all select a.ICODE AS FSTR,A.INAME as Item_Name,a.ICODE AS ERP_code,A.CPARTNO as Part_no,A.IRATE As Irate,0 as Balance_Qty,A.Unit,A.hscode,'-' as PO_No,'-' as SO_link,0 as CDisc,0 as iexc_Addl,0 as frt_pu,0 as pkchg_pu,0 as Qty_Ord,0 as Sold_Qty,A.packsize as std_pack,0 AS olineno from ITEM A WHERE LENGTH(TRIM(ICODE))>4 " + cond + " and length(Trim(nvl(deac_by,'-')))<2 ";
                    }
                    else
                    {
                        SQuery = "select a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||a.ICODE AS FSTR,A.purpose as Item_Name,a.ICODE AS ERP_code,A.exc_57f4 as Part_no,A.IRATE As Irate,iqtyout as Qty,'-' as Unit,b.hscode,a.vchnum as Doc_No,to_char(a.vchdate,'dd/mm/yyyy') as Doc_dt,0 as CDisc,0 as iexc_Addl,0 as frt_pu,0 as pkchg_pu,0 as Qty_Ord,0 as Sold_Qty,0 as std_pack,0 AS olineno from ivoucher A,item b WHERE trim(a.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and trim(A.acode)='" + txtlbl4.Text + "' union all select a.ICODE AS FSTR,A.INAME as Item_Name,a.ICODE AS ERP_code,A.CPARTNO as Part_no,A.IRATE As Irate,0 as Balance_Qty,A.Unit,A.hscode,'-' as PO_No,'-' as SO_link,0 as CDisc,0 as iexc_Addl,0 as frt_pu,0 as pkchg_pu,0 as Qty_Ord,0 as Sold_Qty,A.packsize as std_pack,0 AS olineno from ITEM A WHERE LENGTH(TRIM(ICODE))>4 " + cond + " and length(Trim(nvl(deac_by,'-')))<2 ";
                    }

                }

                //SQuery = "SELECT ACODE AS FSTR,ANAME AS ACCOUNT,ACODE AS CODE FROM FAMST ORDER BY ACODE,ANAME";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);
                break;
            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                SQuery = "select A.acode as fstr,replacE(ANAME,'''','`') as Account_Name,A.Addr1 as Address_l1,a.addr2 as Address_l2,a.acode as ERP_Acode,a.Grp,b.Name,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1  order by A.aname ";
                //SQuery = "SELECT ACODE AS FSTR,ANAME AS ACCOUNT,ACODE AS CODE FROM FAMST ORDER BY ACODE,ANAME";
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
                SQuery = "select trim(upper(a.batch_no)) as Fstr,trim(upper(a.batch_no)) as Batch_no,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,sum(a.Qtyord) as Prodn,sum(a.Soldqty) as Sales from (SELECT trim(icode)||trim(btchno) as fstr,trim(btchno) as Batch_no,iqtyin as qtyord,0 as Soldqty from ivoucher where branchcd='" + frm_mbr + "' and type in ('15','16','17') and trim(store)='Y' and trim(icode)='" + row_erpcd.Trim() + "' union all SELECT trim(icode)||trim(btchno) as fstr,trim(btchno) as Batch_no,0 as qtyord,iqtyout as Soldqty from ivoucher where branchcd='" + frm_mbr + "' and type like '" + lbl1a.Text.Substring(0, 2) + "' and trim(store)='Y' and trim(icode)='" + row_erpcd.Trim() + "')a where trim(fstr) not in (" + col1 + ") group by trim(fstr),trim(upper(a.batch_no))  having  sum(a.Qtyord)-sum(a.Soldqty) >0 order by trim(upper(a.batch_no))";
                SQuery = "select A.acode as fstr,replacE(ANAME,'''','`') as Account_Name,A.Addr1 as Address_l1,a.addr2 as Address_l2,a.acode as ERP_Acode,a.Grp,b.Name,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as entry_Dt,a.Edt_by,a.edt_Dt,a.ent_Dt from famst a left outer join (Select type1,name from type where id='Z') b on trim(a.grp)=trim(B.type1) where a.branchcd!='DD' and length(Trim(nvl(a.deac_by,'-')))<=1  order by A.aname ";
                //SQuery = "SELECT ACODE AS FSTR,ANAME,ACODE FROM FAMST ORDER BY ACODE,ANAME";
                break;
            case "New":
            case "Edit":
            case "Atch":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            case "INVOICE":
                SQuery = "SELECT A.VCHNUM AS FSTR,TRIM(a.VCHNUM) AS INVNO,TO_cHAR(A.VCHDATE,'DD/MM/YYYY') AS INVDATE,B.ANAME AS CUSTOMER FROM SALE A, FAMST B WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '4%' AND A.VCHDATE " + DateRange + " AND TRIM(A.ACODE)='" + txtlbl4.Text.Trim() + "' ORDER BY A.VCHNUM DESC  ";
                if (lbl1a.Text == "31" || lbl1a.Text == "32")
                    SQuery = "SELECT DISTINCT A.INVNO AS FSTR,TRIM(a.INVNO) AS INVNO,TO_cHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,B.ANAME AS PARTY FROM VOUCHER A, FAMST B WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '5%' AND A.VCHDATE " + DateRange + " AND TRIM(A.ACODE)='" + txtlbl4.Text.Trim() + "' ORDER BY A.INVNO DESC  ";
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "Atch_E")
                    SQuery = "select distinct a.branchcd||a.type||trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Doc_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Doc_Dt,b.Aname as Supplier,b.Gst_No,a.Bill_tot,a.pono as Inv_Num,to_char(a.podate,'dd/mm/yyyy') as Inv_Date,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tab_sale + " a,famst b where  a.branchcd='" + (sdWorking ? newBranchcd : frm_mbr) + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " and  trim(a.acode)=trim(B.acodE) order by vdd desc,a." + doc_nf.Value + " desc";
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

                //string mr_time = "";
                //mr_time = fgen.seek_iname(frm_qstr, frm_cocd, "select substr(to_char(sysdate + interval '30' minute,'dd/mm/yyyy hh24:mi'),12,5) as timx from dual", "timx");
                //txtlbl30.Text = mr_time;

                hffield.Value = "New";
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Type", frm_qstr);

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

        //if (txtlbl4.Text.Trim().Length < 2)
        //{
        //    reqd_nc = reqd_nc + 1;
        //    reqd_flds = reqd_flds + " / " + lbl4.Text;
        //}

        if (txtlbl5.Text.Trim() == "-" || txtlbl5.Text.Trim() == "0" || txtlbl5.Text.Trim() == "")
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl5.Text;

        }
        if (txtlbl6.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl6.Text;

        }
        ////if (txtlbl8.Text.Trim().Length < 2)
        ////{
        ////    reqd_nc = reqd_nc + 1;
        ////    reqd_flds = reqd_flds + " / " + lbl8.Text;

        ////}

        ////if (txtlbl9.Text.Trim().Length < 2)
        ////{
        ////    reqd_nc = reqd_nc + 1;
        ////    reqd_flds = reqd_flds + " / " + lbl9.Text;

        ////}


        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }
        string chk_dupl = "";
        if (txtlbl5.Text != "-")
        {
            chk_dupl = fgen.seek_iname(frm_qstr, frm_cocd, "select Type||'-'||vchnum||'-'||to_char(vchdate,'dd/mm/yyyy') as ldt from " + frm_tab_vchr + " where branchcd='" + frm_mbr + "' and type like '5%' and vchdate " + DateRange + " and vchnum||to_char(vchdate,'dd/mm/yyyy')!='" + (string)ViewState["fstr"] + "' and trim(upper(acode))='" + txtlbl4.Text + "' and trim(upper(invno))='" + txtlbl5.Text + "' and invdate " + DateRange + "", "ldt");
            if (lbl1a.Text.Substring(0, 2) == "31" || lbl1a.Text.Substring(0, 2) == "32")
            {
                chk_dupl = "0";
            }
            if (chk_dupl == "0")
            { }
            else
            {
                Checked_ok = "N";
                // to be corrected with "old date" concept
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , This Invoice No. Already Entered in " + chk_dupl + ",Please Check !!");
                return;
            }
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
            if (frm_vty == "5A" && sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().Length < 6)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Expense A/C Code Not Filled Correctly at Line " + (i + 1) + "  !!");
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

        double vch_dr = 0;
        double vch_cr = 0;
        foreach (GridViewRow gr in sg3.Rows)
        {
            if ((((TextBox)gr.FindControl("sg3_t1")).Text.toDouble() > 0 || ((TextBox)gr.FindControl("sg3_t2")).Text.toDouble() > 0) && gr.Cells[3].Text.Trim().Length < 6)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Expense A/c Not Filled Correctly at Line " + (i + 1) + "  !!");
                i = sg1.Rows.Count;
                return;
            }

            if ((((TextBox)gr.FindControl("sg3_t1")).Text.toDouble() > 0 || ((TextBox)gr.FindControl("sg3_t2")).Text.toDouble() > 0))
            {
                vch_dr = vch_dr + ((TextBox)gr.FindControl("sg3_t1")).Text.toDouble();
                vch_cr = vch_cr + ((TextBox)gr.FindControl("sg3_t2")).Text.toDouble();
            }

        }
        vch_dr = Math.Round(vch_dr, 2);
        vch_cr = Math.Round(vch_cr, 2);
        //if (frm_vty.Substring(0, 1) == "5")
        {
            if (frm_vty == "56")
            {
                if (Math.Round(vch_dr + vch_cr, 2) == 0 || Math.Round(vch_dr, 2) != Math.Round(vch_cr, 2))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Account Voucher not Created Correctly!!'13'Press Check/Calculate and See Voucher in Tab2");
                    //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Voucher not Correctly Generated at Line (See Tab 2) !!");
                    i = sg1.Rows.Count;
                    return;
                }
            }
            else
            {
                if (txtlbl30.Text.toDouble() > 0)
                {
                    if (Math.Round(vch_dr + vch_cr, 2) == 0 || Math.Round(vch_dr, 2) != Math.Round(vch_cr, 2) || Math.Round(vch_dr, 2) != Math.Round(txtlbl28.Text.toDouble(), 2) || Math.Round(vch_cr, 2) != Math.Round(txtlbl28.Text.toDouble(), 2))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Account Voucher not Created Correctly!!'13'Press Check/Calculate and See Voucher in Tab2");
                        //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Voucher not Correctly Generated at Line (See Tab 2) !!");
                        i = sg1.Rows.Count;
                        return;
                    }
                }
                else if (Math.Round(vch_dr + vch_cr, 2) == 0 || Math.Round(vch_dr, 2) != Math.Round(vch_cr, 2) || Math.Round(vch_dr, 2) != Math.Round(txtlbl31.Text.toDouble(), 2) || Math.Round(vch_cr, 2) != Math.Round(txtlbl31.Text.toDouble(), 2))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Account Voucher not Created Correctly!!'13'Press Check/Calculate and See Voucher in Tab2");
                    //fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Voucher not Correctly Generated at Line (See Tab 2) !!");
                    i = sg1.Rows.Count;
                    return;
                }
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

        if (frm_cocd != "SGRP")
        {
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
        }

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        //if (vch_cr.ToString().toDouble(2) != txtlbl31.Text.toDouble(2))
        //{
        //    fgen.msg("-", "AMSG", "Voucher Amount and Item Grid Amount is not matching !!");
        //    return;
        //}

        if (txtcc_1.Text.Length < 2)
        {
            fgen.msg("-", "AMSG", "Voucher class not selected !!");
            return;
        }
        if (lbl1a.Text == "58" || lbl1a.Text == "59")
        {
            if (txtbizgrp.Text.Length < 2)
            {
                fgen.msg("-", "AMSG", "Reasaon not selected !!");
                return;
            }
        }
        if (fgen.make_double(txtlbl30.Text) > 1 || fgen.make_double(txtlbl30.Text) < -1)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Value of This Bill is " + txtlbl28.Text + "'13'This Voucher Value : " + txtlbl31.Text + " , Please Check ?");
            return;
        }

        if (fgen.make_double(txtlbl31.Text) <= 0)
        {
            fgen.msg("-", "AMSG", "Total Amount Can Not be Zero or Less then Zero!!");
            return;
        }

        if (txtTDSPer.Text.toDouble() > 0)
        {
            hffield.Value = "TDSQ";
            fgen.msg("-", "CMSG", "Do You Want to Deduct TDS %");
        }
        else
        {
            if (txtlbl8.Text == "Y") fgen.msg("-", "SMSG", "This Voucher will Hold due to short/excess Qty'13'Are You Sure, You Want To Save!!");
            else fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
            btnsave.Disabled = true;
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_DOC_VIEW", "N");

        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_OPEN_IN_EDIT") == "Y")
        {

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "N");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMDRILLID"));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup();", true);
        }
        else Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
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
        //CP_HF1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_HF1");
        //hf1.Value = CP_HF1;
        CP_HF1 = hf1.Value;
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
                string link_party = "";


                //

                link_party = fgen.seek_iname(frm_qstr, frm_cocd, "select branchcd||trim(Acode)||trim(vchnum)||' '||to_char(vchdate,'dd/mm/yyyy') as party from " + frm_tab_sale + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'", "party");


                dmlq = "update ivoucher set finvno='-' where branchcd||trim(Acode)||trim(finvno)='" + link_party + "' and type like '0%'";


                fgen.execute_cmd(frm_qstr, frm_cocd, dmlq);



                // Deleing data from Main Table


                dmlq = "delete from " + frm_tab_ivch + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, dmlq);
                // Deleing data from Sr Ctrl Table
                dmlq = "delete from wsr_ctrl a where a.branchcd||TRIM(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, dmlq);
                // Deleing data from sale Table
                dmlq = "delete from " + frm_tab_sale + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, dmlq);
                // Deleing data from voucher Table
                dmlq = "delete from " + frm_tab_vchr + " where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, dmlq);



                dmlq = "delete from udf_Data a where par_tbl='" + frm_tab_ivch + "' and par_fld='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, dmlq);
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                clearctrl(); fgen.ResetForm(this.Controls);
            }
        }
        else if (hffield.Value == "TDSQ")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            txtCutTDS.Text = col1;

            fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
            btnsave.Disabled = true;
        }
        else if (hffield.Value == "CALC_A")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "N") create_vch_entry("MANUAL");
            else create_vch_entry("");
        }
        else if (hffield.Value == "PRINT_E1")
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y" || frm_vty == "50")
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70203");
            }
            else if (col1 == "N" && frm_vty == "5A")
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70203");
            }
            else fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70203M");
            fgen.fin_acct_reps(frm_qstr);
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
                    qfno = "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tab_vchr + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + lbl1a.Text.Substring(0, 2) + "' AND " + doc_df.Value + " " + DateRange + " ";
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, qfno, 6, "VCH");
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
                    lbl1a.Text = col1;
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;
                case "Edit":
                case "Atch":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    hffield.Value = btnval + "_E";
                    make_qry_4_popup();
                    cond = "Edit";
                    if (btnval == "Atch") cond = "Upload File";
                    fgen.Fn_open_sseek("Select Entry to " + cond + "", frm_qstr);
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
                    lbl1a.Text = col1;
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    if (frm_formID == "F50106") fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    else fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;
                case "Atch_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1.Right(16));
                    fgen.open_fileUploadPopup("Upload File for " + lblheader.Text, frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    if (col1 == "") return;
                    clearctrl();
                    mv_col = col1;
                    editFunction(mv_col);
                    break;
                case "INVOICE":
                    txtlbl5.Text = col2;
                    txtlbl6.Text = col3;
                    break;
                case "PICK_MRR":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    double mrow_rt = 0;
                    mv_col = frm_mbr + frm_vty + col1;

                    //if (doc_GST.Value == "N")
                    //{
                    //    SQuery = "select c.unit,trim(a.acode) as acode,b.aname,b.staffcd as gst_pos,b.staten,'-' as desc_,trim(a.icode) As icode,sum(a.qtyord)-sum(a.chl_qty) as qtysupp,max(a.cdisc) as cdisc,max(a.ciname) as ciname,max(a.cpartno) As cpartno,a.ordno as ponum,to_Char(a.orddt,'dd/mm/yyyy') as podate,a.vchnum,to_chaR(a.vchdate,'dd/mm/yyyy') as Vchdated,0 as irate,A.vchnum||'-'||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||trim(a.ordline) as refr,trim(a.ordline) as ordline,0 as num4,0 as num5,0 as num6 from (select ciname,cpartno,acode,ordno,orddt,PACKNO AS vchnum,PACKDATE AS vchdate,icode,QTYSUPP as qtyord,0 as chl_qty,cdisc,ordline from DESPATCH where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(Acode)='" + txtlbl4.Text + "' union all select null as ciname,null as cpartno,acode,ponum,podate,tc_no,refdate,icode,0 as qtyord,iqtyout as chl_qty,0 as cdisc,revis_no from " + frm_tab_ivch + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(Acode)='" + txtlbl4.Text + "') a ,famst b where trim(A.acode)=trim(B.acode) and a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) in(" + col1 + ") group by c.unit,b.staffcd,b.aname,b.staten,trim(a.acode),trim(a.icode),a.ordno,a.orddt,a.vchnum,to_Char(a.vchdate,'dd/mm/yyyy'),A.vchnum||'-'||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||trim(a.ORDLINE),trim(a.ordline) having sum(a.qtyord)-sum(a.chl_qty)>0 order by a.vchnum";
                    //}
                    //else
                    //{
                    //union all select type,acode,ponum,podate,tc_no,refdate,icode,0 as qtyord,iqtyin as chl_qty,0 as cdisc,revis_no from " + frm_tab_ivch + " where branchcd='" + frm_mbr + "' and type like '0%' and trim(Acode)='" + txtlbl4.Text + "'
                    //
                    SQuery = "select a.cavity,a.type,trim(nvl(c.no_proc,'-')) as Sunit,c.unit,trim(a.acode) as acode,b.aname,trim(nvl(b.gst_no,'-')) as gst_no,b.staffcd as gst_pos,b.staten,'-' as desc_,trim(a.icode) as icode,sum(a.iqty_chl) as iqty_chl,sum(a.iqtyin) as iqtyin,sum(a.acpt_ud) as acpt_ud,sum(a.iqty_wt) as iqty_wt,c.iname,c.cpartno,a.ponum as ponum,to_Char(a.podate,'dd/mm/yyyy') as podate,a.vchnum,to_chaR(a.vchdate,'dd/mm/yyyy') as Vchdated,a.irate,A.vchnum||'-'||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||trim(a.ordline) as refr,trim(a.ordline) as ordline,d.num4,d.num5,d.num6,a.invno,to_chaR(a.invdate,'dd/mm/yyyy') as INVDATE,a.potype from (select type,acode,ponum,podate,vchnum,vchdate,icode,iqty_chl,iqtyin+nvl(rej_rw,0) as iqtyin,nvl(acpt_ud,0) as acpt_ud,nvl(iqty_wt,0) as iqty_wt,ordlineno as ordline,invno,invdate,potype,irate,nvl(cavity,1) as cavity from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and trim(Acode)='" + txtlbl4.Text + "' and store='Y' ) a ,famst b,item c,typegrp d where d.id='T1' and trim(A.acode)=trim(B.acode) and trim(c.hscode)=trim(d.acref) and trim(A.icode)=trim(c.icode) and a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) in(" + col1 + ") group by a.type,c.unit,b.staffcd,b.aname,trim(nvl(b.gst_no,'-')),b.staten,trim(a.acode),trim(a.icode),a.ponum,a.podate,a.vchnum,to_Char(a.vchdate,'dd/mm/yyyy'),a.type||A.vchnum||'-'||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||trim(a.ORDLINE),trim(a.ordline),a.irate,a.cavity,d.num4,d.num5,d.num6,c.iname,c.cpartno,trim(nvl(c.no_proc,'-')),a.invno,to_chaR(a.invdate,'dd/mm/yyyy'),a.potype order by a.vchnum";

                    SQuery = "select a.cavity,a.type,trim(nvl(c.no_proc,'-')) as Sunit,c.unit,trim(a.acode) as acode,b.aname,trim(nvl(b.gst_no,'-')) as gst_no,b.staffcd as gst_pos,b.staten,'-' as desc_,trim(a.icode) as icode,sum(a.iqty_chl) as iqty_chl,sum(a.iqtyin) as iqtyin,sum(a.acpt_ud) as acpt_ud,sum(a.iqty_wt) as iqty_wt,c.iname,c.cpartno,a.ponum as ponum,to_Char(a.podate,'dd/mm/yyyy') as podate,a.vchnum,to_chaR(a.vchdate,'dd/mm/yyyy') as Vchdated,a.irate,A.vchnum||'-'||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||trim(a.ordline) as refr,trim(a.ordline) as ordline,d.num4,d.num5,d.num6,a.invno,to_chaR(a.invdate,'dd/mm/yyyy') as INVDATE,a.potype,a.vcode,a.com_amt from (select type,acode,ponum,podate,vchnum,vchdate,icode,iqty_chl,iqtyin+nvl(rej_rw,0) as iqtyin,nvl(acpt_ud,0) as acpt_ud,nvl(iqty_wt,0) as iqty_wt,ordlineno as ordline,invno,invdate,potype,irate,nvl(cavity,1) as cavity,vcode,srno,com_amt from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and trim(Acode)='" + txtlbl4.Text + "' and store='Y' ) a ,famst b,item c,typegrp d where d.id='T1' and trim(A.acode)=trim(B.acode) and trim(c.hscode)=trim(d.acref) and trim(A.icode)=trim(c.icode) and a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) in(" + col1 + ") group by a.type,c.unit,b.staffcd,b.aname,trim(nvl(b.gst_no,'-')),b.staten,trim(a.acode),trim(a.icode),a.ponum,a.podate,a.vchnum,to_Char(a.vchdate,'dd/mm/yyyy'),a.type||A.vchnum||'-'||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||trim(a.ORDLINE),trim(a.ordline),a.irate,a.cavity,d.num4,d.num5,d.num6,c.iname,c.cpartno,trim(nvl(c.no_proc,'-')),a.invno,to_chaR(a.invdate,'dd/mm/yyyy'),a.potype,a.vcode,a.srno,a.com_amt order by a.vchnum,a.srno";
                    if (frm_vty == "51")
                        SQuery = "select a.cavity,a.type,trim(nvl(c.no_proc,'-')) as Sunit,c.unit,trim(a.acode) as acode,b.aname,trim(nvl(b.gst_no,'-')) as gst_no,b.staffcd as gst_pos,b.staten,'-' as desc_,trim(a.icode) as icode,sum(a.iqty_chl) as iqty_chl,sum(a.iqtyin) as iqtyin,sum(a.acpt_ud) as acpt_ud,sum(a.iqty_wt) as iqty_wt,c.iname,c.cpartno,a.ponum as ponum,to_Char(a.podate,'dd/mm/yyyy') as podate,a.vchnum,to_chaR(a.vchdate,'dd/mm/yyyy') as Vchdated,a.irate,A.vchnum||'-'||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||trim(a.ordline) as refr,trim(a.ordline) as ordline,d.num4,d.num5,d.num6,a.invno,to_chaR(a.invdate,'dd/mm/yyyy') as INVDATE,a.potype,a.vcode,a.com_amt from (select type,acode,ponum,podate,vchnum,vchdate,icode,iqty_chl,iqtyin+nvl(rej_rw,0) as iqtyin,nvl(acpt_ud,0) as acpt_ud,nvl(iqty_wt,0) as iqty_wt,ordlineno as ordline,invno,invdate,potype,irate,nvl(cavity,1) as cavity,vcode,srno,com_amt from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and trim(Acode)='" + txtlbl4.Text + "' and store!='R' ) a ,famst b,item c,typegrp d where d.id='T1' and trim(A.acode)=trim(B.acode) and (case when trim(c.vat_code)!='-' then trim(c.vat_code) else trim(c.hscode) end) =trim(d.acref)  and trim(A.icode)=trim(c.icode) and a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) in(" + col1 + ") group by a.type,c.unit,b.staffcd,b.aname,trim(nvl(b.gst_no,'-')),b.staten,trim(a.acode),trim(a.icode),a.ponum,a.podate,a.vchnum,to_Char(a.vchdate,'dd/mm/yyyy'),a.type||A.vchnum||'-'||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)||trim(a.ORDLINE),trim(a.ordline),a.irate,a.cavity,d.num4,d.num5,d.num6,c.iname,c.cpartno,trim(nvl(c.no_proc,'-')),a.invno,to_chaR(a.invdate,'dd/mm/yyyy'),a.potype,a.vcode,a.srno,a.com_amt order by a.vchnum,a.srno";
                    //}

                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {

                        string mgst_no = "";
                        mgst_no = dt.Rows[i]["gst_no"].ToString().Trim();

                        if (mgst_no.Length > 10 || doc_GST.Value == "GCC")
                        {
                            txtTax.Text = "Y";
                        }
                        else
                        {
                            txtTax.Text = "N";
                        }

                        txtlbl2.Text = dt.Rows[i]["vchnum"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["vchdated"].ToString().Trim();

                        txtlbl5.Text = dt.Rows[i]["INVNO"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["INVDATE"].ToString().Trim();

                        if (txtlbl5.Text.Length >= 1 && txtlbl5.Text != "-")
                        {
                            if (frm_cocd == "MPAC")
                            {
                                //&& fgen.make_double(frm_ulvl)<=2
                            }
                            else
                            {
                                txtlbl5.ReadOnly = true;
                                txtlbl6.ReadOnly = true;
                            }
                        }

                        txtlbl4.Text = dt.Rows[i]["Acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();

                        txtlbl9.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT st_entform FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '0%' AND TRIM(VCHNUM)='" + txtlbl2.Text + "' AND TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + txtlbl3.Text.Trim() + "' ", "st_entform");

                        txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");
                        txtlbl73.Text = dt.Rows[i]["staten"].ToString().Trim();

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[i]["irate"].ToString().Trim();
                            sg1_dr["sg1_h2"] = "-";
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = dt.Rows[i]["potype"].ToString().Trim();

                            sg1_dr["sg1_h5"] = dt.Rows[i]["VCODE"].ToString().Trim();
                            sg1_dr["sg1_h6"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(ACODE)='" + dt.Rows[i]["VCODE"].ToString().Trim() + "'", "ANAME");
                            sg1_dr["sg1_h7"] = dt.Rows[i]["INVNO"].ToString().Trim();
                            sg1_dr["sg1_h8"] = Convert.ToDateTime(dt.Rows[i]["INVDATE"].ToString().Trim()).ToString("dd/MM/yyyy");

                            sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["Cpartno"].ToString().Trim();

                            sg1_dr["sg1_f4"] = dt.Rows[i]["unit"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["sunit"].ToString().Trim();
                            //fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[i]["icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'") + " " + dt.Rows[i]["unit"].ToString().Trim();

                            if ((frm_cocd == "SGRP" || frm_cocd == "UATS") && (lbl1a.Text == "50" || lbl1a.Text == "51" || lbl1a.Text == "56")) { }
                            else
                            {
                                sg1_dr["sg1_t1"] = dt.Rows[i]["iqty_chl"].ToString().Trim();

                                sg1_dr["sg1_t2"] = dt.Rows[i]["iqtyin"].ToString().Trim();
                            }

                            if (dt.Rows[i]["Icode"].ToString().Trim().Substring(0, 2) == "02")
                            {
                                sg1_dr["sg1_t3"] = dt.Rows[i]["iqty_wt"].ToString().Trim();
                            }
                            else
                            {
                                if (hfw122.Value == "Y")
                                {
                                    sg1_dr["sg1_t3"] = dt.Rows[i]["iqty_chl"].ToString().Trim();
                                    if (dt.Rows[i]["iqty_chl"].ToString().Trim().toDouble() > dt.Rows[i]["acpt_ud"].ToString().Trim().toDouble())
                                    {
                                        txtlbl8.Text = "Y";
                                    }
                                    if (dt.Rows[i]["iqty_chl"].ToString().Trim().toDouble() < dt.Rows[i]["acpt_ud"].ToString().Trim().toDouble())
                                    {
                                        sg1_dr["sg1_t3"] = dt.Rows[i]["acpt_ud"].ToString().Trim();
                                    }
                                }
                                else
                                {
                                    sg1_dr["sg1_t3"] = dt.Rows[i]["acpt_ud"].ToString().Trim();
                                }

                            }

                            string col11 = "";

                            sg1_dr["sg1_t9"] = "-";

                            if (dt.Rows[i]["type"].ToString().Trim() == "09")
                            {
                                col11 = fgen.seek_iname(frm_qstr, frm_cocd, "select substr(icode,1,2) As icode from Pomas where  branchcd='" + frm_mbr + "' and trim(type) like '52%' and trim(ordno)='" + dt.Rows[i]["ponum"].ToString().Trim() + "'  and to_Char(orddt,'dd/mm/yyyy')='" + dt.Rows[i]["podate"].ToString().Trim() + "' and trim(acode)='" + txtlbl4.Text + "' and substr(trim(icode),1,2)='59'", "icode");
                                if (col11 == "59")
                                {
                                    sg1_dr["sg1_t3"] = dt.Rows[i]["iqty_wt"].ToString().Trim();
                                    sg1_dr["sg1_t9"] = "Weight Payable";
                                }
                            }

                            if (dt.Rows[i]["type"].ToString().Trim() == "04")
                            {
                                sg1_dr["sg1_t3"] = dt.Rows[i]["iqtyin"].ToString().Trim();
                            }



                            col11 = fgen.seek_iname(frm_qstr, frm_cocd, "select nvl(prate,0)||'~'||nvl(pdisc,0)||'~'||trim(pordno) as PP from Pomas where  branchcd='" + frm_mbr + "' and trim(type) like '5%' and trim(ordno)='" + dt.Rows[i]["ponum"].ToString().Trim() + "' and lpad(trim(cscode),4,'0')='" + dt.Rows[i]["ordline"].ToString().Trim() + "' and to_Char(orddt,'dd/mm/yyyy')='" + dt.Rows[i]["podate"].ToString().Trim() + "' and trim(acode)='" + txtlbl4.Text + "' and trim(icode)='" + dt.Rows[i]["Icode"].ToString().Trim() + "'", "PP");

                            if (col11.Length > 1)
                            {
                                if (dt.Rows[i]["type"].ToString().Trim() == "07")
                                {


                                    mrow_rt = Math.Round(fgen.make_double(col11.Split('~')[0].ToString()) * fgen.make_double(dt.Rows[i]["cavity"].ToString().Trim()), 4);
                                    sg1_dr["sg1_t4"] = mrow_rt;
                                    sg1_dr["sg1_t5"] = col11.Split('~')[1].ToString();

                                }
                                else
                                {
                                    sg1_dr["sg1_t4"] = col11.Split('~')[0].ToString();
                                    sg1_dr["sg1_t5"] = col11.Split('~')[1].ToString();

                                }

                            }
                            else
                            {
                                sg1_dr["sg1_t4"] = dt.Rows[i]["irate"].ToString().Trim();
                                sg1_dr["sg1_t5"] = 0;

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
                                if (dt.Rows[i]["potype"].ToString().Trim() == "54")
                                {
                                    sg1_dr["sg1_t7"] = "0";
                                    sg1_dr["sg1_t8"] = "0";

                                }
                                else
                                {
                                    sg1_dr["sg1_t7"] = dt.Rows[i]["num6"].ToString().Trim();
                                    sg1_dr["sg1_t8"] = "0";

                                }
                            }

                            if (txtTax.Text == "N")
                            {
                                sg1_dr["sg1_t7"] = 0;
                                sg1_dr["sg1_t8"] = 0;
                            }


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
                            sg1_dr["sg1_t21"] = dt.Rows[i]["type"].ToString().Trim();

                            sg1_dr["sg1_t26"] = dt.Rows[i]["COM_AMT"].ToString().Trim().toDouble();

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

                        //if (lbl1a.Text == "56")
                        //{
                        //    hffield.Value = "CONVR";
                        //    fgen.Fn_ValueBox("Please Entry Rate of Exchange as per Bill", frm_qstr);
                        //}

                        string SQueryx = "select a.* from IVCHCTRL a where  a.type||a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) in(" + col1 + ") ";
                        //}

                        dt4 = new DataTable();
                        dt4 = fgen.getdata(frm_qstr, frm_cocd, SQueryx);
                        if (dt4.Rows.Count > 0)
                        {
                            if (dt4.Rows[0]["MTOT_TCS"].ToString().toDouble() > 0)
                            {
                                txtTCSA.Text = dt4.Rows[0]["MTOT_TCS"].ToString();
                            }
                            txtlbl28.Text = dt4.Rows[0]["cst_amt"].ToString();
                            txtlbl30.Text = dt4.Rows[0]["lst_amt"].ToString();
                            txtlbl24.Text = (dt4.Rows[0]["insu_Amt"].ToString().toDouble() + dt4.Rows[0]["frt_amt"].ToString().toDouble() + dt4.Rows[0]["pack_amt"].ToString().toDouble() + dt4.Rows[0]["other"].ToString().toDouble()).ToString();

                            hfPacking.Value = dt4.Rows[0]["pack_amt"].ToString();
                            hfInsurance.Value = dt4.Rows[0]["insu_Amt"].ToString();
                            hfFrieght.Value = dt4.Rows[0]["frt_amt"].ToString();

                            hfOther.Value = dt4.Rows[0]["other"].ToString();
                        }
                        create_vch_entry("");
                    }
                    #endregion
                    break;
                case "CONVR":

                    break;
                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;
                case "Print_E":
                    if (col1.Length < 2) return;
                    //Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
                    //fgen.fin_sales_reps(frm_qstr);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "" + col1 + "");
                    hffield.Value = "PRINT_E1";
                    fgen.msg("-", "CMSG", "Press Yes to print Voucher Format,'13'No for GST Note Format");
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;

                    txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");
                    txtlbl73.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM famst WHERE trim(acode)='" + col1 + "'", "STATEn");
                    //fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");

                    btnlbl7.Focus();

                    string app_rt1;
                    app_rt1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT vchnum FROM wb_PV_HEAD WHERE branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(acode)='" + txtlbl4.Text + "' order by vchdate desc", "vchnum");
                    if (app_rt1 != "0")
                    {
                        app_rt1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT nvl(destin,'-')||'~'||nvl(mode_Tpt,'-')||'~'||nvl(ins_no,'-')||'~'||nvl(freight,'-')||'~'||nvl(insur_no,'-') as skfstr FROM wb_pv_head WHERE branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(acode)='" + txtlbl4.Text + "'  order by vchdate desc", "skfstr");
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

                    col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT tdsrate FROM FAMST WHERE ACODE='" + col1 + "'", "tdsrate");
                    if (col3 != "0") txtTDSPer.Text = col3;

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

                    string chk_opt = "";
                    if (frm_formID == "F70116" || frm_formID == "F70122")
                    {
                        chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0044'", "fstr");
                        if (chk_opt == "Y")
                        {
                            hffield.Value = "PICK_MRR";
                            make_qry_4_popup();
                            fgen.Fn_open_mseek("Select MRR", frm_qstr);
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


                case "BTN_BIZ":
                    if (col1.Length <= 0) return;
                    txtbizgrp.Text = col2 + ":" + col1;
                    break;

                case "BTN_CC1":
                case "BTN_TAX1":
                    if (col1.Length <= 0) return;
                    txtcc_1.Text = col3 + ":" + col2;
                    break;

                case "BTN_CC2":
                    if (col1.Length <= 0) return;
                    txtcc_2.Text = col3 + ":" + col2;
                    break;

                case "BTN_CC3":
                    if (col1.Length <= 0) return;
                    txtcc_3.Text = col3 + ":" + col2;
                    break;


                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    txtlbl2.Focus();
                    break;
                case "TICODEX":
                    txtlbl70.Text = col1;
                    txtlbl71.Text = col2;
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
                        if (Prg_Id == "F70112")
                        {
                            if (col1.Trim().Length == 8) SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.olineno,a.std_pack,a.doc_no,a.doc_dt from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                            else SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.olineno,a.std_pack,a.doc_no,a.doc_dt from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in (" + col1 + ")";

                        }
                        if (Prg_Id == "F70108" || Prg_Id == "F70110")
                        {
                            if (col1.Trim().Length == 8) SQuery = "select a.doc_no ,a.doc_dt ,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.qty as BALANCE_QTY,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.olineno,a.std_pack from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                            else SQuery = "select a.doc_no ,a.doc_dt ,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.qty as BALANCE_QTY,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.olineno,a.std_pack from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in (" + col1 + ")";
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
                            sg1_dr["sg1_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();

                            if (Prg_Id == "F70108" || Prg_Id == "F70110" || Prg_Id == "F70112")
                            {
                                sg1_dr["sg1_f4"] = dt.Rows[d]["doc_no"].ToString().Trim();
                                sg1_dr["sg1_f5"] = dt.Rows[d]["doc_dt"].ToString().Trim();
                                txtlbl5.Text = dt.Rows[d]["doc_no"].ToString().Trim();
                                txtlbl6.Text = dt.Rows[d]["doc_dt"].ToString().Trim();
                            }
                            else
                            {
                                sg1_dr["sg1_f4"] = dt.Rows[d]["po_no"].ToString().Trim();
                                sg1_dr["sg1_f5"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[d]["icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'") + " " + dt.Rows[d]["unit"].ToString().Trim();

                            }

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

                            if (frm_formID == "F70112")
                            {
                                if (txtlbl71.Text.Trim().Length > 3)
                                {
                                    if (txtlbl71.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                                    {
                                        sg1_dr["sg1_t7"] = dt.Rows[d]["num4"].ToString().Trim();
                                        sg1_dr["sg1_t8"] = dt.Rows[d]["num5"].ToString().Trim();
                                    }
                                    else
                                    {
                                        sg1_dr["sg1_t7"] = dt.Rows[d]["num6"].ToString().Trim();
                                        sg1_dr["sg1_t8"] = "0";
                                    }
                                }
                            }

                            if (doc_GST.Value == "GCC")
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
                                mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(9, 6);
                                sg1_dr["sg1_t14"] = mpo_Dt;
                                sg1_dr["sg1_t15"] = dt.Rows[d]["olineno"].ToString().Trim();
                                mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(6, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(4, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 4);
                                sg1_dr["sg1_t16"] = fgen.make_def_Date(mpo_Dt, vardate);
                            }
                            catch { }

                            if (Prg_Id == "F70108" || Prg_Id == "F70110")
                            {
                                sg1_dr["sg1_t14"] = "-";
                                sg1_dr["sg1_t15"] = "-";
                                sg1_dr["sg1_t16"] = "-";
                            }

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
                    if (Prg_Id == "F70108" || Prg_Id == "F70110")
                    {
                        SQuery = "select a.doc_no as po_no,a.doc_Dt,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.qty as balance_qty,a.Cdisc,a.unit,a.hscode,a.std_pack,b.num4,b.num5,b.num6,b.num7 from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                    }
                    else
                    {
                        SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,a.std_pack,b.num4,b.num5,b.num6,b.num7 from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                    }


                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    for (int d = 0; d < dt.Rows.Count; d++)
                    {

                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = "-";
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = "-";
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[2].Text = "-";
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
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[d]["icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'") + " " + dt.Rows[d]["unit"].ToString().Trim();

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
                        SQuery = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");
                        SQuery = "select * from (" + SQuery + ") where trim(fstr) ='" + col1 + "'";

                        //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        //for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg3_dr = sg3_dt.NewRow();
                            sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;

                            sg3_dr["sg3_f1"] = col1;
                            sg3_dr["sg3_f2"] = col2;
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
                case "SG3_ROW_ADD_E":
                    if (col1 == "") return;
                    sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = col1;
                    sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = col2;
                    ((TextBox)sg3.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg3_t1")).Focus();
                    break;
                case "SG1_ROW_TAX":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    break;
                case "SG1_COST":
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t22")).Text = (fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL1").ToUpper().Contains("-- SELECT --") ? "-" : fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL1").ToUpper());
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t23")).Text = (fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").ToUpper().Contains("-- SELECT --") ? "-" : fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").ToUpper());
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t24")).Text = (fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL3").ToUpper().Contains("-- SELECT --") ? "-" : fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL3").ToUpper());
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t25")).Text = (fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL4").ToUpper().Contains("-- SELECT --") ? "-" : fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL4").ToUpper());
                    }
                    break;
                case "SG1_ROW_DT":
                    {

                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = col1;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = col2;
                    }
                    //fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
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
            cond = "and a.type='" + frm_vty + "'";
            if (frm_vty == "" || frm_vty == "0")
                cond = "and a.type like '5%'";

            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "Select a.Vchnum as Inv_No,to_char(a.vchdate,'dd/mm/yyyy') as Dated,c.Aname as Supplier,a.purpose  as Item_Name,a.exc_57f4 as Part_No,a.iqtyout as sale_Qty,a.Irate,a.ichgs as Disc,b.unit,b.hscode,a.Desc_,a.icode,a.ent_by,a.ent_Dt from " + frm_tab_ivch + " a, item b,famst c where a.branchcd='" + frm_mbr + "'  " + cond + " and a." + doc_df.Value + " " + PrdRange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a." + doc_df.Value + ",a." + doc_nf.Value + ",a.morder ";
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
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tab_vchr, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
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
                        string fin_Vch_no = "";


                        fin_Vch_no = txtvchnum.Text + " " + txtvchdate.Text;

                        if (frm_formID == "F70116")
                        {
                            // updating ivoucher table LC and TAX
                            foreach (GridViewRow gr in sg1.Rows)
                            {
                                if (gr.Cells[13].Text.Length > 4)
                                {
                                    SQuery = "UPDATE IVOUCHER SET finvno='" + fin_Vch_no + "',EXC_rATE='" + ((TextBox)gr.FindControl("sg1_t7")).Text.toDouble() + "', EXC_AMT='" + ((TextBox)gr.FindControl("sg1_t17")).Text.toDouble() + "', CESS_PERCENT='" + ((TextBox)gr.FindControl("sg1_t8")).Text.toDouble() + "', CESS_PU='" + ((TextBox)gr.FindControl("sg1_t18")).Text.toDouble() + "', ICHGS='" + gr.Cells[2].Text.toDouble() + "' , EXP_PUNIT='" + gr.Cells[1].Text.toDouble() + "' WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + ((TextBox)gr.FindControl("sg1_t21")).Text + "' AND VCHNUM='" + ((TextBox)gr.FindControl("sg1_t19")).Text + "' AND TO_cHAR(VCHDATE,'DD/MM/YYYY')='" + ((TextBox)gr.FindControl("sg1_t20")).Text + "' AND TRIM(aCODE)='" + txtlbl4.Text + "' AND TRIM(ICODE)='" + gr.Cells[13].Text.Trim() + "' AND ORDLINENO='" + ((TextBox)gr.FindControl("sg1_t15")).Text + "' ";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                                    SQuery = "UPDATE IVOUCHER SET invno='" + txtlbl5.Text + "',invdate=to_DatE('" + txtlbl6.Text + "','dd/mm/yyyy') WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + ((TextBox)gr.FindControl("sg1_t21")).Text + "' AND VCHNUM='" + ((TextBox)gr.FindControl("sg1_t19")).Text + "' AND TO_cHAR(VCHDATE,'DD/MM/YYYY')='" + ((TextBox)gr.FindControl("sg1_t20")).Text + "' AND TRIM(aCODE)='" + txtlbl4.Text + "' AND TRIM(ICODE)='" + gr.Cells[13].Text.Trim() + "' AND ORDLINENO='" + ((TextBox)gr.FindControl("sg1_t15")).Text + "' and trim(invno)='-'";
                                    fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                                }
                            }
                        }

                        string ddl_fld1;
                        string ddl_fld2;
                        ddl_fld1 = frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                        ddl_fld2 = frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");

                        if (edmode.Value == "Y")
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tab_ivch + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tab_sale + " set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tab_vchr + " set branchcd='88' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "update budgmst set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update udf_Data set branchcd='DD' where par_tbl='" + frm_tab_ivch + "' and par_fld='" + ddl_fld1 + "'");

                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tab_ivch);
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, frm_tab_sale);


                        #region Voucher Saving
                        z = 0;
                        // calculation to check 
                        int dr_Srn = 1;
                        int cr_Srn = 50;
                        string matl_rev_cd = "-";
                        string debi_acode = "-";
                        string cred_acode = "-";
                        string holdYN = "N";
                        if (txtlbl8.Text == "Y")
                            holdYN = "Y";
                        foreach (GridViewRow gr in sg3.Rows)
                        {
                            if (((TextBox)gr.FindControl("sg3_t5")).Text.Trim().Length < 3)
                            {
                                ((TextBox)gr.FindControl("sg3_t5")).Text = "Bill Number : " + txtlbl5.Text.Trim() + " Dt : " + txtlbl6.Text.Trim();
                            }

                            if (z == 0)
                            {
                                matl_rev_cd = gr.Cells[3].Text.Trim();
                                //gr.Cells[3].ToString().Trim();
                            }

                            if (((TextBox)gr.FindControl("sg3_t1")).Text.toDouble() > 0)
                            {

                                debi_acode = gr.Cells[3].Text.Trim();
                                cred_acode = txtlbl4.Text.Trim();

                                fgen.vSavePV(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text), dr_Srn, debi_acode, cred_acode, ((TextBox)gr.FindControl("sg3_t1")).Text.toDouble(), ((TextBox)gr.FindControl("sg3_t2")).Text.toDouble(), txtlbl5.Text.Trim().ToUpper(), Convert.ToDateTime(txtlbl6.Text.Trim().ToUpper()), ((TextBox)gr.FindControl("sg3_t5")).Text, ((TextBox)gr.FindControl("sg3_t1")).Text.toDouble(), ((TextBox)gr.FindControl("sg3_t2")).Text.toDouble(), 0, ((TextBox)gr.FindControl("sg3_t1")).Text.toDouble(), ((TextBox)gr.FindControl("sg3_t2")).Text.toDouble(), ((TextBox)gr.FindControl("sg3_t3")).Text, Convert.ToDateTime(txtlbl6.Text.Trim().ToUpper()), frm_uname, DateTime.Now, "-", 0, 0, "-", "-", DateTime.Now, "-", "VOUCHER", txtcc_1.Text.Left(2), txtlbl2.Text, fgen.make_def_Date(txtlbl3.Text, vardate), holdYN);
                                dr_Srn = dr_Srn + 1;
                            }
                            else
                            {
                                if (((TextBox)gr.FindControl("sg3_t2")).Text.toDouble() > 0)
                                {

                                    debi_acode = gr.Cells[3].Text.Trim();
                                    cred_acode = matl_rev_cd;

                                    fgen.vSavePV(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text), cr_Srn, debi_acode, cred_acode, ((TextBox)gr.FindControl("sg3_t1")).Text.toDouble(), ((TextBox)gr.FindControl("sg3_t2")).Text.toDouble(), txtlbl5.Text.Trim().ToUpper(), Convert.ToDateTime(txtlbl6.Text.Trim().ToUpper()), ((TextBox)gr.FindControl("sg3_t5")).Text, ((TextBox)gr.FindControl("sg3_t1")).Text.toDouble(), ((TextBox)gr.FindControl("sg3_t2")).Text.toDouble(), 0, ((TextBox)gr.FindControl("sg3_t1")).Text.toDouble(), ((TextBox)gr.FindControl("sg3_t2")).Text.toDouble(), ((TextBox)gr.FindControl("sg3_t3")).Text, Convert.ToDateTime(txtlbl6.Text.Trim().ToUpper()), frm_uname, DateTime.Now, "-", 0, 0, "-", "-", DateTime.Now, "-", "VOUCHER", txtcc_1.Text.Left(2), txtlbl2.Text, fgen.make_def_Date(txtlbl3.Text, vardate), holdYN);
                                    cr_Srn = cr_Srn + 1;
                                }
                            }
                            z = z + 1;
                        }
                        if (txtTDSAmt.Text.toDouble() > 0)
                        {
                            debi_acode = fgen.getOption(frm_qstr, frm_cocd, "W0115", "OPT_PARAM");
                            cred_acode = txtlbl4.Text.Trim();
                            string mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT DRTOT FROM FAMST WHERE TRIM(ACODE)='" + cred_acode + "'", "drtot");
                            if (mhd.toDouble() != 0)
                            {
                                debi_acode = mhd.PadLeft(6, '0');
                            }

                            dr_Srn = 100;
                            fgen.vSavePV(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text), dr_Srn, cred_acode, debi_acode, txtTDSAmt.Text.toDouble(), 0, txtlbl5.Text.Trim().ToUpper(), Convert.ToDateTime(txtlbl6.Text.Trim().ToUpper()), "TDS " + txtTDSPer.Text + " % on " + txtlbl25.Text + " ", txtTDSAmt.Text.toDouble(), 0, 0, txtTDSAmt.Text.toDouble(), 0, "TDS", Convert.ToDateTime(txtlbl6.Text.Trim().ToUpper()), frm_uname, DateTime.Now, "-", 0, txtTDSPer.Text.toDouble(), "-", "-", DateTime.Now, "-", "VOUCHER", txtcc_1.Text.Left(2), txtlbl2.Text, fgen.make_def_Date(txtlbl3.Text, vardate), holdYN);
                            dr_Srn = dr_Srn + 1;

                            cr_Srn = 101;
                            fgen.vSavePV(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(txtvchdate.Text), cr_Srn, debi_acode, cred_acode, 0, txtTDSAmt.Text.toDouble(), txtlbl5.Text.Trim().ToUpper(), Convert.ToDateTime(txtlbl6.Text.Trim().ToUpper()), "TDS " + txtTDSPer.Text + " % on " + txtlbl25.Text + " ", 0, txtTDSAmt.Text.toDouble(), 0, 0, txtTDSAmt.Text.toDouble(), "TDS", Convert.ToDateTime(txtlbl6.Text.Trim().ToUpper()), frm_uname, DateTime.Now, "-", 0, txtTDSPer.Text.toDouble(), "-", "-", DateTime.Now, "-", "VOUCHER", txtcc_1.Text.Left(2), txtlbl2.Text, fgen.make_def_Date(txtlbl3.Text, vardate), holdYN);
                            cr_Srn = cr_Srn + 1;
                        }
                        //
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
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from budgmst where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data where branchcd='DD' and par_tbl='" + frm_tab_ivch + "' and par_fld='" + frm_mbr + ddl_fld2 + "'");
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "CMSG", lblheader.Text + " " + frm_vnum + " Saved Successfully'13'Do you want to see the Print Preview ?");
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

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim() + "'");
                        if (frm_cocd == "SAIA" && frm_uname != "FINTEAM")
                        {
                            if (edmode.Value != "Y")
                            {
                                try
                                {
                                    sendMail();
                                }
                                catch { }
                            }
                        }

                        if (frm_vty == "50" && frm_formID == "F70116" && txtlbl9.Text.Length > 2)
                        {
                            fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE IVOUCHER SET st_entform='" + txtlbl9.Text.Trim() + "' WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '0%' AND TRIM(VCHNUM)='" + txtlbl2.Text + "' AND TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + txtlbl3.Text.Trim() + "'");
                        }

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
    //--------------------------
    public void sdSavingInvAndMRR()
    {

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

        for (int i = 1; i < 30; i++)
        {
            sg1_dt.Columns.Add(new DataColumn("sg1_t" + i, typeof(string)));
        }
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
        sg3_dt.Columns.Add(new DataColumn("sg3_t5", typeof(string)));
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
        sg1_dr["sg1_t21"] = "-";

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
            //if (sg1.Rows.Count > 1)
            //{
            //    //if (frm_formID == "F70116")
            //    {
            //        ((ImageButton)e.Row.FindControl("sg1_btnadd")).ImageUrl = "../tej-base/images/Btn_addn.png";
            //        ((ImageButton)e.Row.FindControl("sg1_btnrmv")).ImageUrl = "../tej-base/images/Btn_addn.png";
            //    }
            //    //else
            //    //{
            //    //    ((ImageButton)e.Row.FindControl("sg1_btnadd")).ImageUrl = "~/tej-base/images/Btn_addn.png";
            //    //    ((ImageButton)e.Row.FindControl("sg1_btnrmv")).ImageUrl = "~/tej-base/images/Btn_remn.png";
            //    //}
            //}

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
            if (frm_formID == "F70122")
            {
                sg1.Columns[3].HeaderStyle.Width = 1;
                sg1.HeaderRow.Cells[4].Text = "Sec.Vendor";
                sg1.Columns[4].HeaderStyle.Width = 60;
                sg1.HeaderRow.Cells[5].Text = "Sec.Vendor";
                sg1.Columns[5].HeaderStyle.Width = 150;
                sg1.HeaderRow.Cells[6].Text = "Bill No";
                sg1.Columns[6].HeaderStyle.Width = 70;
                sg1.HeaderRow.Cells[7].Text = "Bill Dt";
                sg1.Columns[7].HeaderStyle.Width = 80;
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
                if (frm_formID == "F70116")
                {
                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT count(*) AS COL1 from POMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + sg1.Rows[rowIndex].Cells[3].Text + "' AND TRIM(ORDNO)||TO_cHAR(ORDDT,'DD/MM/YYYY')='" + ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t14")).Text.Trim() + ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t16")).Text.Trim() + "'", "COL1");
                    if (col1 != "0")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", sg1.Rows[rowIndex].Cells[3].Text);
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t14")).Text.Trim() + ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t16")).Text.Trim() + "'");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1004");
                        fgen.fin_purc_reps(frm_qstr);
                    }
                    else fgen.msg("-", "AMSG", "P.O. Not Linked!!");
                }
                else
                {
                    if (index < sg1.Rows.Count - 1)
                    {
                        hf1.Value = index.ToString();
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                        //----------------------------
                        hffield.Value = "SG1_RMV";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                        fgen.msg("-", "CMSG", "are you sure!! you want to remove this item from the list");
                    }
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
                    {
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("Select Accounts Code", frm_qstr);
                    }
                    //fgen.Fn_open_dtbox("Select Date", frm_qstr);
                }
                break;
            case "SG1_COST":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_COST";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    {
                        Session["Filled"] = "Y";
                        fgen.Fn_ValueBoxMultiple("Select Cost Center", frm_qstr, "550px", "250px");
                    }
                    //fgen.Fn_open_dtbox("Select Date", frm_qstr);
                }
                break;

            case "SG1_ROW_ADD":
                if (frm_formID == "F70116")
                {

                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT count(*) AS COL1 from POMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t21")).Text.Trim() + "' AND TRIM(ORDNO)||TO_cHAR(ORDDT,'DD/MM/YYYY')='" + ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t19")).Text.Trim() + ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t20")).Text.Trim() + "'", "COL1");
                    if (col1 != "0")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t21")).Text.Trim());
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t19")).Text.Trim() + ((TextBox)sg1.Rows[rowIndex].FindControl("sg1_t20")).Text.Trim() + "'");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1002");
                        fgen.fin_invn_reps(frm_qstr);
                    }
                    else fgen.msg("-", "AMSG", "MRR Not Linked!!");
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
                    fgen.Fn_open_sseek("Select Account", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG3_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Account", frm_qstr);
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
        fgen.Fn_open_sseek("Select Supplier ", frm_qstr);
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
        fgen.Fn_open_sseek("Select " + lbl12.Text + " ", frm_qstr);
    }
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_13";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl13.Text + " ", frm_qstr);
    }
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_14";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl14.Text + "", frm_qstr);
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

    protected void btnbiz_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_BIZ";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Reason for Dr / Cr Note ", frm_qstr);
    }

    protected void btncc1_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_TAX1";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Voucher Class", frm_qstr);
    }
    protected void btncc2_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_CC2";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Cost Centre level 2", frm_qstr);
    }
    protected void btncc3_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_CC3";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Cost Centre level 3", frm_qstr);
    }

    //------------------------------------------------------------------------------------
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl7.Text + " ", frm_qstr);
    }
    protected void btnlbl70_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODEX";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select P.O.S", frm_qstr);
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
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum.Trim();
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["invno"] = txtlbl5.Text;
                oporow["invdate"] = fgen.make_def_Date(txtlbl6.Text.Trim(), vardate);

                oporow["store"] = "N";
                oporow["rec_iss"] = "C";

                oporow["acode"] = txtlbl4.Text.Trim();
                oporow["rcode"] = txtlbl4.Text.Trim();

                oporow["morder"] = i + 1;
                oporow["BILLRATE"] = sg1.Rows[i].Cells[0].Text.Trim().toDouble();
                oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim();
                oporow["purpose"] = sg1.Rows[i].Cells[14].Text.Trim();
                oporow["exc_57f4"] = sg1.Rows[i].Cells[15].Text.Trim();
                oporow["finvno"] = sg1.Rows[i].Cells[16].Text.Trim();

                if (lbl1a.Text.Substring(0, 2) == "50" || lbl1a.Text.Substring(0, 2) == "51" || lbl1a.Text.Substring(0, 2) == "53")
                {
                    oporow["iqty_chl"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().toDouble();
                    oporow["iqtyin"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().toDouble();
                    oporow["iqty_Wt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().toDouble();
                }
                else
                {
                    oporow["rcode"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().toDouble();
                    oporow["iqty_chl"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().toDouble();

                    oporow["iqtyin"] = 0;
                    oporow["iqty_Wt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().toDouble();

                }

                oporow["iqtyout"] = 0;

                oporow["irate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim());
                oporow["ichgs"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim());

                oporow["iamount"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim());

                oporow["exc_Rate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim());
                oporow["exc_amt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim());

                oporow["cess_percent"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim());
                oporow["cess_pu"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim());

                oporow["desc_"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();

                oporow["iexc_addl"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim());
                oporow["idiamtr"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim());
                oporow["ipack"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim());

                oporow["ccent"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                oporow["revis_no"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();

                oporow["ponum"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();

                string po_dts;
                po_dts = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim(), vardate);

                if (((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim().Length > 2)
                    oporow["podate"] = po_dts;
                else oporow["podate"] = fgen.make_def_Date(txtlbl6.Text.Trim(), vardate);

                oporow["tc_no"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                po_dts = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim(), vardate);
                oporow["refdate"] = po_dts;
                oporow["fabtype"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();

                if (lbl1a.Text.Substring(0, 2) == "50" || lbl1a.Text.Substring(0, 2) == "5A" || lbl1a.Text.Substring(0, 2) == "53")
                {

                }
                else
                {
                    //txtlbl6.Text = Convert.ToDateTime(po_dts).ToString("dd/MM/yyyy");
                }


                oporow["iopr"] = lbl27.Text.Substring(0, 2);

                oporow["doc_tot"] = sg1.Rows[i].Cells[2].Text.toDouble();

                //oporow["ICHGS"] = sg1.Rows[i].Cells[1].Text.toDouble();
                oporow["exp_punit"] = sg1.Rows[i].Cells[1].Text.toDouble();
                oporow["potype"] = sg1.Rows[i].Cells[3].Text;

                if (frm_vty == "58" || frm_vty == "59")
                {
                    oporow["potype"] = txtbizgrp.Text.Right(2);
                }

                oporow["COL1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();

                if (i == 0)
                {
                    oporow["SPEXC_RATE"] = fgen.make_double(txtlbl25.Text.Trim());
                    oporow["SPEXC_AMT"] = fgen.make_double(txtlbl31.Text.Trim());

                    oporow["TXB_PUNIT"] = fgen.make_double(txtImpTaxValue.Text.Trim());
                    oporow["BILLFRT"] = fgen.make_double(txtFr.Text.Trim());
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

                if (frm_formID == "F70122")
                {
                    oporow["VCODE"] = sg1.Rows[i].Cells[4].Text;
                    oporow["REFNUM"] = sg1.Rows[i].Cells[6].Text;
                    oporow["REFDATE"] = fgen.make_def_Date(sg1.Rows[i].Cells[7].Text, vardate);
                }

                oporow["CC1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim().Left(4);
                oporow["CC2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text.Trim().Left(4);
                oporow["CC3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text.Trim().Left(4);
                oporow["PR_SEGMENT"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text.Trim().Left(4);

                oporow["COM_AMT"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text.Trim().toDouble();

                oporow["psize"] = fgen.make_double(txtTCSA.Text);

                oporow["MATTYPE"] = txtcc_1.Text.Left(2);
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().Length > 5)
                    oporow["NARATION"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                else oporow["NARATION"] = "Bill Number : " + txtlbl5.Text.Trim() + " Dt : " + txtlbl6.Text.Trim() + ", " + sg1.Rows[i].Cells[14].Text.Trim();

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
        oporow2["TYPE"] = frm_vty;
        oporow2["vchnum"] = frm_vnum;
        oporow2["vchdate"] = txtvchdate.Text.Trim();

        oporow2["Acode"] = txtlbl4.Text;
        if (sdWorking)
        {
            if (brCode != "")
                oporow2["acode"] = brCode;
        }
        oporow2["cscode"] = txtlbl7.Text.Trim();


        oporow2["remvtime"] = txtlbl2.Text;
        oporow2["remvdate"] = fgen.make_def_Date(txtlbl3.Text.Trim(), vardate);

        oporow2["pono"] = txtlbl5.Text;
        oporow2["podate"] = txtlbl6.Text;

        oporow2["destin"] = txtlbl8.Text;
        oporow2["st_entform"] = txtlbl9.Text;

        oporow2["mode_tpt"] = txtlbl15.Text;
        oporow2["ins_no"] = txtlbl16.Text;
        oporow2["freight"] = txtlbl17.Text;
        oporow2["insur_no"] = txtlbl18.Text;

        oporow2["mo_vehi"] = "-";
        oporow2["weight"] = "-";

        oporow2["fob_frt"] = fgen.make_double(txtlbl24.Text);
        oporow2["fob_ins"] = fgen.make_double(txtlbl26.Text);
        oporow2["fob_tot"] = fgen.make_double(txtlbl28.Text);
        oporow2["fob_oth"] = fgen.make_double(txtlbl30.Text);

        oporow2["post"] = lbl27.Text.Substring(0, 1);

        oporow2[frm_vty == "4S" ? "AMT_REA" : "AMT_SALE"] = fgen.make_double(txtlbl25.Text);
        oporow2["AMT_EXC"] = fgen.make_double(txtlbl27.Text);
        oporow2["RVALUE"] = fgen.make_double(txtlbl29.Text);
        oporow2["BILL_TOT"] = fgen.make_double(txtlbl31.Text);


        oporow2["FCOTH1"] = 0;
        oporow2["FCOTH2"] = 0;
        oporow2["FCOTH3"] = 0;
        oporow2["FCOTH4"] = 0;

        //if (txtbizgrp.Text.Trim().Length > 4)
        //{
        //    oporow2["FCOTH1"] = fgen.make_double(txtbizgrp.Text.Substring(0, 4));
        //}
        if (txtcc_1.Text.Trim().Length > 4)
        {
            oporow2["FCOTH2"] = fgen.make_double(txtcc_1.Text.Substring(0, 4));
        }
        if (txtcc_2.Text.Trim().Length > 4)
        {
            oporow2["FCOTH3"] = fgen.make_double(txtcc_2.Text.Substring(0, 4));
        }
        if (txtcc_3.Text.Trim().Length > 4)
        {
            oporow2["FCOTH4"] = fgen.make_double(txtcc_3.Text.Substring(0, 4));
        }

        //oporow2["FCOTH2"] = fgen.make_double(txtcc_1.Text.Substring(0, 4));
        //oporow2["FCOTH3"] = fgen.make_double(txtcc_2.Text.Substring(0, 4));
        //oporow2["FCOTH4"] = fgen.make_double(txtcc_3.Text.Substring(0, 4));



        if (frm_vty == "4S")
        {
            oporow2["BILL_TOT"] = fgen.make_double(txtlbl27.Text) + fgen.make_double(txtlbl29.Text);
        }
        oporow2["BILL_qty"] = Tot_Bill_qty;

        oporow2["naration"] = txtrmk.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
        oporow2["eNt_by"] = frm_uname;
        oporow2["eNt_dt"] = vardate;

        oporow2["DRV_NAME"] = txtlbl70.Text.Trim();
        oporow2["drv_mobile"] = (chkITC.Checked == true ? "Y" : "N");

        oporow2["tcsamt"] = fgen.make_double(txtTCSA.Text);
        oporow2["ADCAMT"] = fgen.make_double(txtTDSAmt.Text);
        oporow2["FRT_STAX"] = fgen.make_double(txtTDSPer.Text);

        //oporow2["GRNO"] = txtGrno.Text;
        //oporow2["GRDATE"] = fgen.make_def_Date(txtGrDt.Text, vardate);

        oDS2.Tables[0].Rows.Add(oporow2);
    }
    void save_fun3()
    {

    }
    void save_fun4()
    {

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

        switch (Prg_Id)
        {
            case "F70108":
                SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE,Acode AS Acctg_code FROM type where id='V' and trim(type1) in ('31','59')  order by type1";
                break;
            case "F70110":
                SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE,Acode AS Acctg_code FROM type where id='V' and trim(type1) in ('32','58')  order by type1";
                break;
            case "F70112":
                SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE,Acode AS Acctg_code FROM type where id='V' and trim(type1) in ('5A','5B','57')  order by type1";
                if (frm_cocd == "SGRP" || frm_cocd == "UATS" || hf151.Value == "Y")
                    SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE,Acode AS Acctg_code FROM type where id='V' and trim(type1) in ('5A','5B','57','50','51','56')  order by type1";
                break;
            case "F70116":
                SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE,Acode AS Acctg_code FROM type where id='V' and type1 like '5%' and type1 not in ('57','58','59','5A','5B','5S','5P') order by type1";
                break;
            case "F70122":
                SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE,Acode AS Acctg_code FROM type where id='V' and type1 like '5%' and type1 in ('5S','5P') order by type1";
                break;
        }





    }
    //void Type_Sel_querywith4f()
    //{
    //    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

    //    SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE,Acode AS Acctg_code FROM type where id='V' and type1 like '5%' and type1 not in ('58','59') order by type1";

    //}
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
        if (frm_formID == "F70112")
        {
            if (txtlbl71.Text.Trim() != "-")
            {
                if (txtlbl71.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                {
                    lbl27.Text = "CGST";
                    lbl29.Text = "SGST/UTGST";
                }
                else
                {
                    lbl27.Text = "IGST";
                    lbl29.Text = "";
                }
            }
        }
        if (doc_GST.Value == "GCC")
        {
            lbl27.Text = "VAT";
            lbl29.Text = "";

            chkITC.Visible = false;
            lbl9.Text = "Delivery_Note_No";
        }

    }
    //string getDiscountedRate(string ticode, string currRate, string tax)
    //{

    //}
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
            if (repDoc == null) return;
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
        string opt = fgen.getOption(frm_qstr, frm_cocd, "W0100", "OPT_ENABLE");
        // to check from web control panel 30/10/2019

        if (opt == "Y")
        {
            if (doc_GST.Value == "GCC")
            {
                col1 = fgen.getOption(frm_qstr, frm_cocd, "W0084", "OPT_PARAM");
                if (col1.ToString().Length < 6)
                {
                    fgen.msg("-", "AMSG", "Purchase GST Control Not Linked Correctly, Check Control No. W0084");
                    return false;
                }
            }
            else
            {
                col1 = fgen.getOption(frm_qstr, frm_cocd, "W0080", "OPT_PARAM");
                if (col1.ToString().Length < 6)
                {
                    fgen.msg("-", "AMSG", "Taxation Codes Not Linked Correctly, Check Control No. W0080, W0081, W0082");
                    return false;
                }
            }


        }
        else
        {
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A77'", "PARAMS");
            if (col1 == "0")
            {
                fgen.msg("-", "AMSG", "Taxation Codes Not Linked Correctly, Check Control No. A77, A78, A79");
                return false;
            }

            //col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A80'", "PARAMS");
            //if (col1 == "0")
            //{
            //    fgen.msg("-", "AMSG", "Purchase GST Control Not Linked Correctly, Check Control No. A80, A81, A82");
            //    return false;
            //}

        }
        return true;
    }
    protected void btncheckTax_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        if (txtlbl28.Text.toDouble() > 0)
        {
            txtlbl30.Text = Math.Round(txtlbl28.Text.toDouble() - txtlbl31.Text.toDouble(), 2).ToString();
        }
        else if (txtlbl30.Text.toDouble() != 0)
        {
            txtlbl28.Text = Math.Round(txtlbl31.Text.toDouble() - txtlbl30.Text.toDouble(), 2).ToString();
        }
        else
        {
            txtlbl28.Text = txtlbl31.Text;
        }

        if (fgen.make_double(txtlbl31.Text) <= 0)
        {
            fgen.msg("-", "AMSG", "Total Amount Can Not be Zero or Less then Zero!!");
            return;
        }
        if (frm_formID != "F70112")
        {
            foreach (GridViewRow gr in sg1.Rows)
            {
                if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper() && doc_GST.Value != "GCC")
                {
                    if (frm_vty == "51")
                        ((TextBox)gr.FindControl("sg1_t7")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT B.NUM4 FROM ITEM A,TYPEGRP B WHERE (case when trim(a.vat_code)!='-' then trim(a.vat_code) else trim(a.hscode) end) =trim(b.acref) AND B.ID='T1' AND TRIM(A.ICODE)='" + gr.Cells[13].Text.Trim() + "' ", "NUM4");
                    else
                        ((TextBox)gr.FindControl("sg1_t7")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT B.NUM4 FROM ITEM A,TYPEGRP B WHERE trim(a.hscode)=trim(b.acref) AND B.ID='T1' AND TRIM(A.ICODE)='" + gr.Cells[13].Text.Trim() + "' ", "NUM4");
                    ((TextBox)gr.FindControl("sg1_t8")).Text = ((TextBox)gr.FindControl("sg1_t7")).Text;
                }
                else
                {
                    if (lbl1a.Text == "55")
                    {

                    }
                    else
                    {
                        if (frm_vty == "51")
                            ((TextBox)gr.FindControl("sg1_t7")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT B.NUM6 FROM ITEM A,TYPEGRP B WHERE (case when trim(a.vat_code)!='-' then trim(a.vat_code) else trim(a.hscode) end) =trim(b.acref) AND B.ID='T1' AND TRIM(A.ICODE)='" + gr.Cells[13].Text.Trim() + "' ", "NUM6");
                        else
                            ((TextBox)gr.FindControl("sg1_t7")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT B.NUM6 FROM ITEM A,TYPEGRP B WHERE trim(a.hscode)=trim(b.acref) AND B.ID='T1' AND TRIM(A.ICODE)='" + gr.Cells[13].Text.Trim() + "' ", "NUM6");
                        ((TextBox)gr.FindControl("sg1_t8")).Text = "0";
                    }
                }

                if (txtTax.Text == "N")
                {
                    ((TextBox)gr.FindControl("sg1_t7")).Text = "0";
                    ((TextBox)gr.FindControl("sg1_t8")).Text = "0";
                }
            }
        }

        if ((frm_cocd == "SGRP" || frm_cocd == "UATS") && (lbl1a.Text == "50" || lbl1a.Text == "51" || lbl1a.Text == "56"))
        {
            hffield.Value = "CALC_A";
            fgen.msg("-", "CMSG", "Do You want to calculate the voucher from Item Sub Group head'13'(No for manually)");
        }
        else create_vch_entry("");
    }

    void create_vch_entry(string anyCondition)
    {
        setGST();

        if (lbl1a.Text == "58" || lbl1a.Text == "59")
        {
            if (chkTCS.Checked)
            {
                txtTCS.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT CESSRATE FROM FAMST WHERE TRIM(ACODE)='" + txtlbl4.Text.Trim() + "' ", "CESSRATE");
                if (txtTCS.Text.toDouble() > 0)
                {
                    txtTCSA.Text = (txtlbl31.Text.toDouble() * (txtTCS.Text.toDouble() / 100)).toDouble(2).ToString();
                }
            }
            else
            {
                lblTCS.Text = "";
                txtTCSA.Text = "";
            }
        }

        if (anyCondition == "MANUAL")
        {
            for (int x = 0; x < sg1.Rows.Count; x++)
            {
                if (!chkTax.Checked)
                {
                    ((TextBox)sg1.Rows[x].FindControl("sg1_t7")).Text = "0";
                    ((TextBox)sg1.Rows[x].FindControl("sg1_t8")).Text = "0";
                }
                if (sg1.Rows[x].Cells[13].Text.ToString().Trim().Length > 6)
                {
                    if (((TextBox)sg1.Rows[x].FindControl("sg1_t1")).Text.Length < 5)
                    {
                        fgen.msg("-", "Account Ledger Not Selected", "Please Check Row number " + (x + 1) + "'13'Account heads not properly linked");
                        return;
                    }
                }
            }
        }

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        string roundOff = fgen.getOption(frm_qstr, frm_cocd, "W0114", "OPT_ENABLE");
        string roundOffAcode = fgen.getOption(frm_qstr, frm_cocd, "W0114", "OPT_PARAM");
        double totval = 0;
        double lcrate = 0;
        string itemAcode = "", bill_no = "";
        foreach (GridViewRow gr in sg1.Rows)
        {
            ((TextBox)gr.FindControl("sg1_t6")).Text = (((TextBox)gr.FindControl("sg1_t3")).Text.ToString().toDouble() * gr.Cells[0].Text.toDouble()).ToString();
            totval += ((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble();
            if (frm_formID == "F70112")
            {
                // commented this condition - for MPAC - 13/07/21 - changes CGST / IGST on Place of Supply 
                //if (frm_vty != "50" && frm_vty != "51" && frm_vty != "5A" && frm_vty != "56")
                if (doc_GST.Value != "GCC")
                {
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT B.NUM4,B.NUM5,B.NUM6 FROM ITEM A,TYPEGRP B WHERE TRIM(A.HSCODE)=TRIM(B.ACREF) AND B.ID='T1' and trim(a.icode)='" + gr.Cells[13].Text.Trim() + "' ");
                    if (dt.Rows.Count > 0)
                    {
                        if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                        {
                            lbl27.Text = "CGST";
                            ((TextBox)gr.FindControl("sg1_t7")).Text = dt.Rows[0]["NUM4"].ToString().Trim();
                            ((TextBox)gr.FindControl("sg1_t8")).Text = dt.Rows[0]["NUM5"].ToString().Trim();
                        }
                        else
                        {
                            lbl27.Text = "IGST";
                            ((TextBox)gr.FindControl("sg1_t7")).Text = dt.Rows[0]["NUM6"].ToString().Trim();
                            ((TextBox)gr.FindControl("sg1_t8")).Text = "0";
                        }
                        if (txtlbl71.Text.Trim().Length > 3)
                        {
                            if (txtlbl71.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                            {
                                lbl27.Text = "CGST";
                                ((TextBox)gr.FindControl("sg1_t7")).Text = dt.Rows[0]["NUM4"].ToString().Trim();
                                ((TextBox)gr.FindControl("sg1_t8")).Text = dt.Rows[0]["NUM5"].ToString().Trim();
                            }
                            else
                            {
                                lbl27.Text = "IGST";
                                ((TextBox)gr.FindControl("sg1_t7")).Text = dt.Rows[0]["NUM6"].ToString().Trim();
                                ((TextBox)gr.FindControl("sg1_t8")).Text = "0";
                            }
                        }
                    }
                }
                if (!chkTax.Checked)
                {
                    ((TextBox)gr.FindControl("sg1_t7")).Text = "0";
                    ((TextBox)gr.FindControl("sg1_t8")).Text = "0";
                }
            }
        }
        txtlbl25.Text = totval.ToString();
        DataTable dtCode = new DataTable();
        dtCode.Columns.Add("ACODE");
        dtCode.Columns.Add("ICODE");
        dtCode.Columns.Add("BILLNO");
        dtCode.Columns.Add("BILLDT");
        dtCode.Columns.Add("VCODE");
        DataRow drCode = null;
        foreach (GridViewRow gr in sg1.Rows)
        {
            if (((TextBox)gr.FindControl("sg1_t3")).Text.ToString().toDouble() > 0)
            {
                if (txtlbl24.Text.ToString().toDouble() > 0)
                    lcrate = Math.Round(txtlbl24.Text.ToString().toDouble() / totval, 2);
                if (frm_vty == "56" && txtFr.Text.toDouble() > 0)
                {
                    lcrate = Math.Round(txtFr.Text.toDouble() / totval, 2);
                    gr.Cells[1].Text = Math.Round(lcrate * ((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble(), 2).ToString();
                    gr.Cells[2].Text = Math.Round(((TextBox)gr.FindControl("sg1_t4")).Text.ToString().toDouble() + (gr.Cells[1].Text.toDouble() / ((TextBox)gr.FindControl("sg1_t3")).Text.ToString().toDouble()), 2).ToString();
                }
                else
                {
                    gr.Cells[1].Text = Math.Round(lcrate * ((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble(), 2).ToString();
                    gr.Cells[2].Text = Math.Round(gr.Cells[0].Text.toDouble() + (gr.Cells[1].Text.toDouble() / ((TextBox)gr.FindControl("sg1_t3")).Text.ToString().toDouble()), 2).ToString();

                    //if (gr.Cells[2].Text.toDouble() > 0)
                    //    ((TextBox)gr.FindControl("sg1_t4")).Text = gr.Cells[2].Text;
                }
                //if (Prg_Id == "F70108" || Prg_Id == "F70110")
                //{

                //}
                //else
                //{                 
                //}

                if (frm_formID == "F70116" && anyCondition != "MANUAL")
                {
                    if (!itemAcode.Contains(gr.Cells[13].Text.Substring(0, 4)))
                    {
                        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(NO_PROC) AS ACODE FROM ITEM WHERE TRIM(ICODE)='" + gr.Cells[13].Text.Substring(0, 4) + "' and length(Trim(icode))=4 ", "ACODE");
                        itemAcode += "," + gr.Cells[13].Text.Substring(0, 4);
                        drCode = dtCode.NewRow();
                        drCode["ACODE"] = col1;
                        drCode["ICODE"] = gr.Cells[13].Text.Substring(0, 4);
                        dtCode.Rows.Add(drCode);
                    }
                }
                else if (frm_formID == "F70122")
                {
                    if (!itemAcode.Contains(gr.Cells[13].Text.Substring(0, 4)))
                    {
                        //if (!bill_no.Contains(gr.Cells[6].Text))
                        {
                            bill_no += "," + gr.Cells[6].Text;
                            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(NO_PROC) AS ACODE FROM ITEM WHERE TRIM(ICODE)='" + gr.Cells[13].Text.Substring(0, 4) + "' and length(Trim(icode))=4 ", "ACODE");
                            itemAcode += "," + gr.Cells[13].Text.Substring(0, 4);
                            drCode = dtCode.NewRow();
                            drCode["ACODE"] = col1;
                            drCode["ICODE"] = gr.Cells[13].Text.Substring(0, 4);
                            drCode["BILLNO"] = gr.Cells[6].Text;
                            drCode["BILLDT"] = gr.Cells[7].Text;
                            drCode["VCODE"] = gr.Cells[4].Text + ":" + gr.Cells[5].Text;
                            dtCode.Rows.Add(drCode);
                        }
                    }
                }
                else
                {
                    if (!itemAcode.Contains(((TextBox)gr.FindControl("sg1_t1")).Text.Trim()))
                    {
                        itemAcode += "," + ((TextBox)gr.FindControl("sg1_t1")).Text.Trim();
                        drCode = dtCode.NewRow();
                        drCode["ACODE"] = ((TextBox)gr.FindControl("sg1_t1")).Text.Trim();
                        drCode["ICODE"] = ((TextBox)gr.FindControl("sg1_t1")).Text.Trim();
                        dtCode.Rows.Add(drCode);
                    }
                }
            }
        }
        if (itemAcode != "") itemAcode = itemAcode.TrimStart(',');

        string sal_code = "", par_code = "", tax_code = "", tax_code2 = "";
        string tax_codeRCM = "", tax_code2RCM = "", sal_codeRCM = "";
        par_code = txtlbl4.Text.Trim();

        string optwb = "";
        optwb = fgen.getOption(frm_qstr, frm_cocd, "W0100", "OPT_ENABLE");
        if (lbl27.Text.Substring(0, 2) == "CG")
        {
            if (optwb == "Y")
            {
                if (lbl1a.Text.Substring(0, 2) == "58" || lbl1a.Text.Substring(0, 2) == "59")
                {
                    tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0077", "OPT_PARAM");
                    sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0077", "OPT_PARAM2");
                    tax_code2 = fgen.getOption(frm_qstr, frm_cocd, "W0078", "OPT_PARAM");
                }
                else
                {
                    tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0080", "OPT_PARAM");
                    sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0080", "OPT_PARAM2");
                    tax_code2 = fgen.getOption(frm_qstr, frm_cocd, "W0081", "OPT_PARAM");
                }
                if (frm_formID == "F70112" && frm_vty != "50" && frm_vty != "51" && frm_vty != "5A" && frm_vty != "56")
                {
                    tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0137", "OPT_PARAM");
                    sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0137", "OPT_PARAM2");
                    tax_code2 = fgen.getOption(frm_qstr, frm_cocd, "W0138", "OPT_PARAM");

                    tax_codeRCM = fgen.getOption(frm_qstr, frm_cocd, "W0134", "OPT_PARAM");
                    sal_codeRCM = fgen.getOption(frm_qstr, frm_cocd, "W0134", "OPT_PARAM2");
                    tax_code2RCM = fgen.getOption(frm_qstr, frm_cocd, "W0135", "OPT_PARAM");

                }
                if (frm_vty != "58" && frm_vty != "59")
                {
                    if (chkITC.Checked == false && fgen.getOption(frm_qstr, frm_cocd, "W0140", "OPT_ENABLE") == "Y")
                    {
                        tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0141", "OPT_PARAM");
                        sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0141", "OPT_PARAM2");
                        tax_code2 = fgen.getOption(frm_qstr, frm_cocd, "W0142", "OPT_PARAM");
                    }
                }
            }
            else
            {
                tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A80'", "PARAMS");
                sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A81'", "PARAMS2");
                tax_code2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A82'", "PARAMS");
            }
        }
        else
        {

            if (optwb == "Y")
            {
                tax_code2RCM = "";
                if (doc_GST.Value == "GCC")
                {
                    if (lbl1a.Text.Substring(0, 2) == "58" || lbl1a.Text.Substring(0, 2) == "59")
                    {
                        tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0083", "OPT_PARAM");
                        sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0083", "OPT_PARAM2");
                    }
                    else
                    {
                        tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0084", "OPT_PARAM");
                        sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0084", "OPT_PARAM2");

                        if (frm_formID == "F70112" && frm_vty != "5A" && frm_vty != "51")
                        {
                            tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0139", "OPT_PARAM");
                            sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0139", "OPT_PARAM2");

                            tax_codeRCM = fgen.getOption(frm_qstr, frm_cocd, "W0136", "OPT_PARAM");
                            sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0136", "OPT_PARAM2");

                        }
                    }
                    if (chkITC.Checked == false && fgen.getOption(frm_qstr, frm_cocd, "W0140", "OPT_ENABLE") == "Y")
                    {
                        tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0143", "OPT_PARAM");
                        sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0143", "OPT_PARAM2");
                    }

                }
                else
                {
                    if (lbl1a.Text.Substring(0, 2) == "58" || lbl1a.Text.Substring(0, 2) == "59")
                    {
                        tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0079", "OPT_PARAM");
                        sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0079", "OPT_PARAM2");
                    }
                    else
                    {
                        tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0082", "OPT_PARAM");
                        sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0082", "OPT_PARAM2");
                    }
                    if (frm_formID == "F70112" && frm_vty != "5A")
                    {
                        tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0139", "OPT_PARAM");
                        sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0139", "OPT_PARAM2");

                        tax_codeRCM = fgen.getOption(frm_qstr, frm_cocd, "W0136", "OPT_PARAM");
                        sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0136", "OPT_PARAM2");

                    }
                    if (chkITC.Checked == false && fgen.getOption(frm_qstr, frm_cocd, "W0140", "OPT_ENABLE") == "Y")
                    {
                        tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0143", "OPT_PARAM");
                        sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0143", "OPT_PARAM2");
                    }

                }


            }
            else
            {
                tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A80'", "PARAMS");
                sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A80'", "PARAMS2");

            }
        }

        create_tab3();

        lcrate = 0;

        double row_bas = 0;
        double row_tax1 = 0;
        double row_tax2 = 0;
        double trow_bas = 0;
        double trow_tax1 = 0;
        double trow_tax2 = 0;
        double defaultCgst = 0;
        double defaultSgst = 0;
        double highestCGST = 0, highestSGST = 0;
        string addedC = "N", addedS = "N";

        dtCode.DefaultView.Sort = "ACODE";
        dtCode = dtCode.DefaultView.ToTable();

        #region F70122
        if (frm_formID == "F70122")
        {
            DataView vdvie = new DataView(dtCode, "", "billno", DataViewRowState.CurrentRows);
            DataTable dtDistbill = vdvie.ToTable(true, "billno", "billdt", "VCODE");
            foreach (DataRow drBill in dtDistbill.Rows)
            {
                row_bas = 0;
                row_tax1 = 0;
                row_tax2 = 0;
                trow_bas = 0;
                trow_tax1 = 0;
                trow_tax2 = 0;

                foreach (DataRow drC in dtCode.Rows)
                {
                    if (drC["BILLNO"].ToString().Trim().ToUpper() == drBill["BILLNO"].ToString().Trim().ToUpper())
                    {
                        totval = 0;
                        col1 = "";
                        col1 = fgen.seek_iname_dt(sg3_dt, "sg3_f1='" + drC["acode"].ToString().Trim() + "' ", "sg3_f1");
                        foreach (GridViewRow gr in sg1.Rows)
                        {
                            if (((TextBox)gr.FindControl("sg1_t3")).Text.ToString().toDouble() > 0)
                            {
                                cond = ((TextBox)gr.FindControl("sg1_t1")).Text;
                                cond = gr.Cells[13].Text.Substring(0, 4);
                                if (cond == drC["Icode"].ToString().TrimStart())
                                {

                                    row_bas += ((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble();
                                    row_tax1 += (((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble() * ((((TextBox)gr.FindControl("sg1_t7")).Text.ToString().toDouble()) / 100));
                                    row_tax2 += (((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble() * ((((TextBox)gr.FindControl("sg1_t8")).Text.ToString().toDouble()) / 100));
                                }
                            }
                        }

                        trow_bas += row_bas;
                        trow_tax1 += row_tax1;
                        trow_tax2 += row_tax2;
                        if (col1 != drC["ACODE"].ToString().Trim())
                        {
                            sg3_dr = sg3_dt.NewRow();

                            sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                            sg3_dr["sg3_f1"] = drC["acode"].ToString().Trim();
                            sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + drC["acode"].ToString().TrimStart() + "%'", "ANAME");

                            if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                            {
                                sg3_dr["sg3_t1"] = "0";
                                sg3_dr["sg3_t2"] = Math.Round(row_bas, 3);
                            }
                            else
                            {
                                sg3_dr["sg3_t1"] = Math.Round(row_bas, 3);
                                sg3_dr["sg3_t2"] = "0";
                            }
                            {
                                sg3_dr["sg3_t3"] = drC["billno"].ToString().Trim();
                                sg3_dr["sg3_t4"] = drC["billdt"].ToString().Trim();
                                sg3_dr["sg3_t5"] = drBill["VCODE"].ToString().Trim().ToUpper().Replace("&AMP;", "&");
                            }
                            sg3_dt.Rows.Add(sg3_dr);
                        }
                        else
                        {
                            foreach (DataRow dr3 in sg3_dt.Rows)
                            {
                                if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                                {
                                    dr3["sg3_t2"] = dr3["sg3_t2"].ToString().toDouble() + row_bas;
                                }
                                else
                                {
                                    dr3["sg3_t1"] = dr3["sg3_t1"].ToString().toDouble() + row_bas;
                                }
                            }
                        }

                        row_bas = 0;
                        row_tax1 = 0;
                        row_tax2 = 0;
                    }
                }

                if (trow_tax1 > 0)
                {
                    sg3_dr = sg3_dt.NewRow();
                    sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                    sg3_dr["sg3_f1"] = tax_code;
                    sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + tax_code + "%'", "ANAME");

                    if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                    {
                        sg3_dr["sg3_t1"] = "0";
                        sg3_dr["sg3_t2"] = Math.Round(trow_tax1, 3);
                    }
                    else
                    {
                        sg3_dr["sg3_t1"] = Math.Round(trow_tax1, 3);
                        sg3_dr["sg3_t2"] = "0";
                    }

                    sg3_dr["sg3_t3"] = drBill["billno"].ToString().Trim();
                    sg3_dr["sg3_t4"] = drBill["billdt"].ToString().Trim();
                    sg3_dr["sg3_t5"] = drBill["VCODE"].ToString().ToUpper().Replace("&AMP;", "&");

                    sg3_dt.Rows.Add(sg3_dr);
                }
                if (trow_tax2 > 0)
                {
                    sg3_dr = sg3_dt.NewRow();
                    sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                    sg3_dr["sg3_f1"] = tax_code2;
                    sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + tax_code2 + "%'", "ANAME");

                    if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                    {
                        sg3_dr["sg3_t1"] = "0";
                        sg3_dr["sg3_t2"] = Math.Round(trow_tax2, 3);
                    }
                    else
                    {
                        sg3_dr["sg3_t1"] = Math.Round(trow_tax2, 3);
                        sg3_dr["sg3_t2"] = "0";
                    }

                    sg3_dr["sg3_t3"] = drBill["billno"].ToString().Trim();
                    sg3_dr["sg3_t4"] = drBill["billdt"].ToString().Trim();
                    sg3_dr["sg3_t5"] = drBill["VCODE"].ToString().Trim().ToUpper().Replace("&AMP;", "&");

                    sg3_dt.Rows.Add(sg3_dr);
                }
                {
                    sg3_dr = sg3_dt.NewRow();
                    sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                    sg3_dr["sg3_f1"] = txtlbl4.Text.Trim();
                    sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + txtlbl4.Text.Trim() + "%'", "ANAME");

                    if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                    {
                        sg3_dr["sg3_t1"] = Math.Round(trow_bas + trow_tax1 + trow_tax2, 3);
                        sg3_dr["sg3_t2"] = "0";
                    }
                    else
                    {
                        sg3_dr["sg3_t1"] = "0";
                        sg3_dr["sg3_t2"] = Math.Round(trow_bas + trow_tax1 + trow_tax2, 3);
                    }



                    sg3_dr["sg3_t3"] = drBill["billno"].ToString().Trim();
                    sg3_dr["sg3_t4"] = drBill["billdt"].ToString().Trim();
                    sg3_dr["sg3_t5"] = drBill["VCODE"].ToString().Trim().ToUpper().Replace("&AMP;", "&");

                    sg3_dt.Rows.Add(sg3_dr);
                }
            }
        }
        #endregion
        else
        {
            #region normal vc
            bool roundTax = chkRoundTax.Checked;
            int roundUpto = 2;
            if (roundTax == true)
                roundUpto = 0;
            foreach (DataRow drC in dtCode.Rows)
            {
                totval = 0;
                col1 = "";
                col1 = fgen.seek_iname_dt(sg3_dt, "sg3_f1='" + drC["acode"].ToString().Trim() + "' ", "sg3_f1");

                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (((TextBox)gr.FindControl("sg1_t3")).Text.ToString().toDouble() > 0)
                    {
                        cond = ((TextBox)gr.FindControl("sg1_t1")).Text;
                        if (frm_formID == "F70116" && anyCondition != "MANUAL")
                        {
                            cond = gr.Cells[13].Text.Substring(0, 4);
                        }
                        if (cond == drC["Icode"].ToString().TrimStart())
                        {
                            double basVal = 0, disc = 0;
                            basVal = (((TextBox)gr.FindControl("sg1_t3")).Text.ToString().toDouble() * ((TextBox)gr.FindControl("sg1_t4")).Text.ToString().toDouble());
                            if (((TextBox)gr.FindControl("sg1_t5")).Text.ToString().toDouble() > 0)
                                disc = ((((TextBox)gr.FindControl("sg1_t3")).Text.ToString().toDouble() * ((TextBox)gr.FindControl("sg1_t4")).Text.ToString().toDouble()) * (((TextBox)gr.FindControl("sg1_t5")).Text.ToString().toDouble() / 100));

                            ((TextBox)gr.FindControl("sg1_t6")).Text = (basVal - disc - ((TextBox)gr.FindControl("sg1_t26")).Text.ToString().toDouble()).ToString();

                            row_bas += ((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble();
                            if (frm_vty == "56" && ((TextBox)gr.FindControl("sg1_t10")).Text.Trim().toDouble() > 0)
                            {
                                row_tax1 += Math.Round(((TextBox)gr.FindControl("sg1_t10")).Text.ToString().toDouble() * ((((TextBox)gr.FindControl("sg1_t7")).Text.ToString().toDouble()) / 100), roundUpto);
                            }
                            else
                                row_tax1 += Math.Round(((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble() * ((((TextBox)gr.FindControl("sg1_t7")).Text.ToString().toDouble()) / 100), roundUpto, MidpointRounding.AwayFromZero);
                            row_tax2 += Math.Round(((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble() * ((((TextBox)gr.FindControl("sg1_t8")).Text.ToString().toDouble()) / 100), roundUpto, MidpointRounding.AwayFromZero);

                            defaultCgst = ((TextBox)gr.FindControl("sg1_t7")).Text.ToString().toDouble();
                            defaultSgst = ((TextBox)gr.FindControl("sg1_t8")).Text.ToString().toDouble();

                            if (defaultCgst > highestCGST) highestCGST = defaultCgst;
                            if (defaultSgst > highestSGST) highestSGST = defaultSgst;
                        }
                    }
                }

                trow_bas += row_bas;
                trow_tax1 += row_tax1;
                trow_tax2 += row_tax2;
                if (col1 != drC["ACODE"].ToString().Trim())
                {
                    sg3_dr = sg3_dt.NewRow();

                    sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                    sg3_dr["sg3_f1"] = drC["acode"].ToString().Trim();
                    sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + drC["acode"].ToString().TrimStart() + "%'", "ANAME");

                    if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                    {
                        sg3_dr["sg3_t1"] = "0";
                        sg3_dr["sg3_t2"] = Math.Round(row_bas, roundUpto, MidpointRounding.AwayFromZero);
                    }
                    else
                    {
                        sg3_dr["sg3_t1"] = Math.Round(row_bas, roundUpto, MidpointRounding.AwayFromZero);
                        sg3_dr["sg3_t2"] = "0";
                    }
                    sg3_dr["sg3_t3"] = "-";
                    sg3_dr["sg3_t4"] = "-";
                    sg3_dt.Rows.Add(sg3_dr);
                }
                else
                {
                    foreach (DataRow dr3 in sg3_dt.Rows)
                    {
                        if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                        {
                            dr3["sg3_t2"] = dr3["sg3_t2"].ToString().toDouble() + row_bas;
                        }
                        else
                        {
                            dr3["sg3_t1"] = dr3["sg3_t1"].ToString().toDouble() + row_bas;
                        }
                    }
                }

                row_bas = 0;
                row_tax1 = 0;
                row_tax2 = 0;
            }
            if (txtlbl24.Text.toDouble() > 0)
            {
                if (fgen.getOption(frm_qstr, frm_cocd, "W0128", "OPT_ENABLE") == "Y")
                {
                    string packTaxCode = fgen.getOption(frm_qstr, frm_cocd, "W0127", "OPT_PARAM");
                    string insuTaxCode = fgen.getOption(frm_qstr, frm_cocd, "W0126", "OPT_PARAM");
                    string freightTaxCode = fgen.getOption(frm_qstr, frm_cocd, "W0125", "OPT_PARAM");
                    string otherTaxCode = fgen.getOption(frm_qstr, frm_cocd, "W0129", "OPT_PARAM");


                    if (packTaxCode.Length < 5)
                    {
                        fgen.msg("Packing Tax Code not linked", "AMSG", "Please Link Control Panel W0127");
                        return;
                    }
                    if (insuTaxCode.Length < 5)
                    {
                        fgen.msg("Insurance Tax Code not linked", "AMSG", "Please Link Control Panel W0126");
                        return;
                    }
                    if (freightTaxCode.Length < 5)
                    {
                        fgen.msg("Freight Tax Code not linked", "AMSG", "Please Link Control Panel W0125");
                        return;
                    }
                    if (otherTaxCode.Length < 5)
                    {
                        fgen.msg("Other Tax Code not linked", "AMSG", "Please Link Control Panel W0129");
                        return;
                    }

                    if (hfPacking.Value.toDouble() > 0)
                    {
                        sg3_dr = sg3_dt.NewRow();
                        sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                        sg3_dr["sg3_f1"] = packTaxCode;
                        sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + packTaxCode + "%'", "ANAME");

                        if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                        {
                            sg3_dr["sg3_t1"] = "0";
                            sg3_dr["sg3_t2"] = Math.Round(hfPacking.Value.toDouble(), 2);
                        }
                        else
                        {
                            sg3_dr["sg3_t1"] = Math.Round(hfPacking.Value.toDouble(), 2);
                            sg3_dr["sg3_t2"] = "0";
                        }
                        sg3_dr["sg3_t3"] = "-";
                        sg3_dr["sg3_t4"] = "-";
                        sg3_dt.Rows.Add(sg3_dr);
                    }
                    if (hfInsurance.Value.toDouble() > 0)
                    {
                        sg3_dr = sg3_dt.NewRow();
                        sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                        sg3_dr["sg3_f1"] = insuTaxCode;
                        sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + insuTaxCode + "%'", "ANAME");

                        if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                        {
                            sg3_dr["sg3_t1"] = "0";
                            sg3_dr["sg3_t2"] = Math.Round(hfInsurance.Value.toDouble(), 2);
                        }
                        else
                        {
                            sg3_dr["sg3_t1"] = Math.Round(hfInsurance.Value.toDouble(), 2);
                            sg3_dr["sg3_t2"] = "0";
                        }
                        sg3_dr["sg3_t3"] = "-";
                        sg3_dr["sg3_t4"] = "-";
                        sg3_dt.Rows.Add(sg3_dr);
                    }
                    if (hfFrieght.Value.toDouble() > 0)
                    {
                        sg3_dr = sg3_dt.NewRow();
                        sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                        sg3_dr["sg3_f1"] = freightTaxCode;
                        sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + freightTaxCode + "%'", "ANAME");

                        if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                        {
                            sg3_dr["sg3_t1"] = "0";
                            sg3_dr["sg3_t2"] = Math.Round(hfFrieght.Value.toDouble(), 2);
                        }
                        else
                        {
                            sg3_dr["sg3_t1"] = Math.Round(hfFrieght.Value.toDouble(), 2);
                            sg3_dr["sg3_t2"] = "0";
                        }
                        sg3_dr["sg3_t3"] = "-";
                        sg3_dr["sg3_t4"] = "-";
                        sg3_dt.Rows.Add(sg3_dr);
                    }
                    if (hfOther.Value.toDouble() > 0)
                    {
                        sg3_dr = sg3_dt.NewRow();
                        sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                        sg3_dr["sg3_f1"] = otherTaxCode;
                        sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + otherTaxCode + "%'", "ANAME");

                        if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                        {
                            sg3_dr["sg3_t1"] = "0";
                            sg3_dr["sg3_t2"] = Math.Round(hfOther.Value.toDouble(), 2);
                        }
                        else
                        {
                            sg3_dr["sg3_t1"] = Math.Round(hfOther.Value.toDouble(), 2);
                            sg3_dr["sg3_t2"] = "0";
                        }
                        sg3_dr["sg3_t3"] = "-";
                        sg3_dr["sg3_t4"] = "-";
                        sg3_dt.Rows.Add(sg3_dr);
                    }
                }
                else
                {
                    if (sg3_dt.Rows.Count > 0)
                    {
                        if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                        {
                            sg3_dt.Rows[0]["sg3_t2"] = sg3_dt.Rows[0]["sg3_t2"].ToString().toDouble() + txtlbl24.Text.toDouble();
                        }
                        else
                        {
                            sg3_dt.Rows[0]["sg3_t1"] = sg3_dt.Rows[0]["sg3_t1"].ToString().toDouble() + txtlbl24.Text.toDouble();
                        }
                    }
                }
                trow_tax1 += (txtlbl24.Text.toDouble() * (defaultCgst / 100)).toDouble(roundUpto);
                trow_tax2 += (txtlbl24.Text.toDouble() * (defaultSgst / 100)).toDouble(roundUpto);
            }
            if (txtlbl24.Text.toDouble() > 0)
            {
                trow_tax1 = 0; trow_tax2 = 0;

                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (((TextBox)gr.FindControl("sg1_t3")).Text.ToString().toDouble() > 0)
                    {
                        {
                            double basVal = 0, disc = 0;

                            // row_bas += ((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble();
                            if (frm_vty == "56" && ((TextBox)gr.FindControl("sg1_t10")).Text.Trim().toDouble() > 0)
                            {
                                row_tax1 += Math.Round(((TextBox)gr.FindControl("sg1_t10")).Text.ToString().toDouble() * ((((TextBox)gr.FindControl("sg1_t7")).Text.ToString().toDouble()) / 100), 3);
                            }
                            else
                            {
                                if (highestCGST > 0)
                                {
                                    if (((TextBox)gr.FindControl("sg1_t7")).Text.ToString().toDouble() == highestCGST && addedC == "N")
                                    {
                                        row_tax1 += Math.Round((((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble() + txtlbl24.Text.toDouble()) * ((((TextBox)gr.FindControl("sg1_t7")).Text.ToString().toDouble()) / 100), 3, MidpointRounding.AwayFromZero);
                                        addedC = "Y";
                                    }
                                    else
                                        row_tax1 += Math.Round(((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble() * ((((TextBox)gr.FindControl("sg1_t7")).Text.ToString().toDouble()) / 100), 3, MidpointRounding.AwayFromZero);
                                }
                            }
                            if (highestSGST > 0)
                            {
                                if (((TextBox)gr.FindControl("sg1_t8")).Text.ToString().toDouble() == highestSGST && addedS == "N")
                                {
                                    row_tax2 += Math.Round((((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble() + txtlbl24.Text.toDouble()) * ((((TextBox)gr.FindControl("sg1_t8")).Text.ToString().toDouble()) / 100), 3, MidpointRounding.AwayFromZero);
                                    addedS = "Y";
                                }
                                else
                                    row_tax2 += Math.Round(((TextBox)gr.FindControl("sg1_t6")).Text.ToString().toDouble() * ((((TextBox)gr.FindControl("sg1_t8")).Text.ToString().toDouble()) / 100), 3, MidpointRounding.AwayFromZero);
                            }
                        }
                    }
                }
                trow_tax1 = Math.Round(row_tax1, roundUpto);
                trow_tax2 = Math.Round(row_tax2, roundUpto);
            }

            if (frm_formID == "F70112" && frm_vty != "5A" && chkITC.Checked == false && fgen.getOption(frm_qstr, frm_cocd, "W0140", "OPT_ENABLE") == "N")
            {
                sg3_dt.Rows[0]["sg3_t1"] = sg3_dt.Rows[0]["sg3_t1"].ToString().toDouble() + trow_tax1 + trow_tax2;
            }
            else
            {
                if (trow_tax1 > 0)
                {
                    sg3_dr = sg3_dt.NewRow();
                    sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                    sg3_dr["sg3_f1"] = tax_code;
                    sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + tax_code + "%'", "ANAME");

                    if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                    {
                        sg3_dr["sg3_t1"] = "0";
                        sg3_dr["sg3_t2"] = Math.Round(trow_tax1, roundUpto);
                    }
                    else
                    {
                        sg3_dr["sg3_t1"] = Math.Round(trow_tax1, roundUpto);
                        sg3_dr["sg3_t2"] = "0";
                    }

                    sg3_dr["sg3_t3"] = "-";
                    sg3_dr["sg3_t4"] = "-";

                    sg3_dt.Rows.Add(sg3_dr);
                }
                if (trow_tax2 > 0)
                {
                    sg3_dr = sg3_dt.NewRow();
                    sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                    sg3_dr["sg3_f1"] = tax_code2;
                    sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + tax_code2 + "%'", "ANAME");

                    if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                    {
                        sg3_dr["sg3_t1"] = "0";
                        sg3_dr["sg3_t2"] = Math.Round(trow_tax2, roundUpto);
                    }
                    else
                    {
                        sg3_dr["sg3_t1"] = Math.Round(trow_tax2, roundUpto);
                        sg3_dr["sg3_t2"] = "0";
                    }

                    sg3_dr["sg3_t3"] = "-";
                    sg3_dr["sg3_t4"] = "-";

                    sg3_dt.Rows.Add(sg3_dr);
                }
            }
            if (txtTCSA.Text.toDouble() > 0)
            {
                sg3_dr = sg3_dt.NewRow();
                sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                if (lbl1a.Text == "58" || lbl1a.Text == "59") sg3_dr["sg3_f1"] = fgen.getOption(frm_qstr, frm_cocd, "W0123", "OPT_PARAM");
                else sg3_dr["sg3_f1"] = fgen.getOption(frm_qstr, frm_cocd, "W0113", "OPT_PARAM");
                sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + sg3_dr["sg3_f1"].ToString().Trim() + "%'", "ANAME");

                if (lbl1a.Text == "58" || lbl1a.Text == "59")
                {
                    sg3_dr["sg3_t2"] = Math.Round(txtTCSA.Text.toDouble(), roundUpto);
                    sg3_dr["sg3_t1"] = "0";
                }
                else
                {
                    sg3_dr["sg3_t1"] = Math.Round(txtTCSA.Text.toDouble(), roundUpto);
                    sg3_dr["sg3_t2"] = "0";
                }
                sg3_dr["sg3_t3"] = "-";
                sg3_dr["sg3_t4"] = "-";

                sg3_dt.Rows.Add(sg3_dr);
            }

            if (frm_vty == "5B" || frm_vty == "57")
            {
                //****************************************
                sg3_dr = sg3_dt.NewRow();
                sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                sg3_dr["sg3_f1"] = txtlbl4.Text.Trim();
                sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + txtlbl4.Text.Trim() + "%'", "ANAME");

                sg3_dr["sg3_t1"] = "0";
                sg3_dr["sg3_t2"] = Math.Round(trow_bas, 3);

                sg3_dt.Rows.Add(sg3_dr);
                //****************************************
                sg3_dr = sg3_dt.NewRow();
                sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                sg3_dr["sg3_f1"] = tax_codeRCM;
                sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + tax_codeRCM + "%'", "ANAME");

                sg3_dr["sg3_t1"] = "0";
                sg3_dr["sg3_t2"] = Math.Round(trow_tax1, 3);

                sg3_dt.Rows.Add(sg3_dr);

                //****************************************
                if (trow_tax2 > 0)
                {
                    sg3_dr = sg3_dt.NewRow();
                    sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                    sg3_dr["sg3_f1"] = tax_code2RCM;
                    sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + tax_code2RCM + "%'", "ANAME");

                    sg3_dr["sg3_t1"] = "0";
                    sg3_dr["sg3_t2"] = Math.Round(trow_tax2, 3);

                    sg3_dt.Rows.Add(sg3_dr);
                }
            }
            else
            {
                sg3_dr = sg3_dt.NewRow();
                sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                sg3_dr["sg3_f1"] = txtlbl4.Text.Trim();
                sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + txtlbl4.Text.Trim() + "%'", "ANAME");

                if (lbl1a.Text.Substring(0, 2) == "56")
                {
                    sg3_dr["sg3_t1"] = "0";
                    sg3_dr["sg3_t2"] = Math.Round(trow_bas, 2);
                }
                else if (lbl1a.Text.Substring(0, 2) == "59" || lbl1a.Text.Substring(0, 2) == "31")
                {
                    sg3_dr["sg3_t1"] = Math.Round(trow_bas + txtlbl24.Text.toDouble() + trow_tax1 + trow_tax2 + txtTCSA.Text.toDouble(), 2);
                    sg3_dr["sg3_t2"] = "0";
                    if (roundOff == "Y")
                    {
                        sg3_dr["sg3_t1"] = txtlbl28.Text.toDouble();
                    }
                }
                else
                {
                    sg3_dr["sg3_t1"] = "0";
                    sg3_dr["sg3_t2"] = Math.Round(trow_bas + txtlbl24.Text.toDouble() + trow_tax1 + trow_tax2 + txtTCSA.Text.toDouble(), 2);
                    if (roundOff == "Y")
                        sg3_dr["sg3_t2"] = txtlbl28.Text.toDouble();
                }

                sg3_dr["sg3_t3"] = "-";
                sg3_dr["sg3_t4"] = "-";

                sg3_dt.Rows.Add(sg3_dr);
            }

            if (roundOff == "Y" && roundOffAcode.Length > 3 && txtlbl30.Text.Trim().toDouble() != 0)
            {
                sg3_dr = sg3_dt.NewRow();
                sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                sg3_dr["sg3_f1"] = roundOffAcode;
                sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + roundOffAcode.Trim() + "%'", "ANAME");

                {
                    if (txtlbl30.Text.Trim().toDouble() < 0)
                    {
                        sg3_dr["sg3_t1"] = "0";
                        sg3_dr["sg3_t2"] = Math.Abs(txtlbl30.Text.Trim().toDouble());
                    }
                    else
                    {
                        sg3_dr["sg3_t2"] = "0";
                        sg3_dr["sg3_t1"] = Math.Abs(txtlbl30.Text.Trim().toDouble());
                    }
                }

                sg3_dr["sg3_t3"] = "-";
                sg3_dr["sg3_t4"] = "-";

                sg3_dt.Rows.Add(sg3_dr);
            }

            if (frm_vty == "56" && txtImpTaxValue.Text.toDouble() > 0)
            {
                sg3_dr = sg3_dt.NewRow();
                sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                sg3_dr["sg3_f1"] = fgen.getOption(frm_qstr, frm_cocd, "W0085", "OPT_PARAM");
                sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + sg3_dr["sg3_f1"].ToString().Trim() + "%'", "ANAME");

                sg3_dr["sg3_t1"] = "0";
                sg3_dr["sg3_t2"] = Math.Round(txtImpTaxValue.Text.toDouble(), roundUpto);

                sg3_dr["sg3_t3"] = "-";
                sg3_dr["sg3_t4"] = "-";

                sg3_dt.Rows.Add(sg3_dr);
            }
            if (frm_vty == "56" && txtFr.Text.toDouble() > 0)
            {
                sg3_dr = sg3_dt.NewRow();
                sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
                sg3_dr["sg3_f1"] = fgen.getOption(frm_qstr, frm_cocd, "W0086", "OPT_PARAM");
                sg3_dr["sg3_f2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE LIKE '" + sg3_dr["sg3_f1"].ToString().Trim() + "%'", "ANAME");

                sg3_dr["sg3_t1"] = "0";
                sg3_dr["sg3_t2"] = Math.Round(txtFr.Text.toDouble(), roundUpto);

                sg3_dr["sg3_t3"] = "-";
                sg3_dr["sg3_t4"] = "-";

                sg3_dt.Rows.Add(sg3_dr);
            }


            if (frm_vty == "56" && txtImpTaxValue.Text.toDouble() > 0)
            {
                double totDr = 0;
                double totCr = 0;

                foreach (DataRow drx in sg3_dt.Rows)
                {
                    totDr += drx["sg3_t1"].ToString().toDouble();
                    totCr += drx["sg3_t2"].ToString().toDouble();
                }
                if (sg3.Rows.Count > 0)
                {
                    sg3_dt.Rows[0]["sg3_t1"] = sg3_dt.Rows[0]["sg3_t1"].ToString().toDouble() + (totCr - totDr);
                }
            }
            #endregion
        }
        if (frm_vty == "59")
        {
            DataView dvSort = new DataView(sg3_dt, "", "sg3_t1 DESC", DataViewRowState.CurrentRows);
            sg3_dt = dvSort.ToTable();

            int x = 0;
            foreach (DataRow drx in sg3_dt.Rows)
            {
                drx["sg3_SrNo"] = x + 1;
                x++;
            }
        }


        sg3_add_blankrows();
        sg3.DataSource = sg3_dt;
        sg3.DataBind();

        ViewState["sg3"] = sg3_dt;
    }

    protected void btnatch_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_DOC_VIEW", "N");
        hffield.Value = "Atch";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr);
    }
    void editFunction(string mv_col)
    {
        setColHeadings();
        set_Val();
        #region Edit Start
        SQuery = "Select a.*,to_char(A.refdate,'dd/mm/yyyy') as refdated,to_char(A.podate,'dd/mm/yyyy') as podtd,c.Aname,nvl(b.cpartno,'-') As Icpartno,nvl(b.unit,'-') as IUnit,nvl(b.no_proc,'-') as no_proc,nvl(c.gst_no,'-') as gst_no from " + frm_tab_ivch + " a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + mv_col + "' ORDER BY A.morder";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            if (frm_ulvl != "0")
            {
                if (fgen.check_filed_name(frm_qstr, frm_cocd, "VOUCHER", "AUDT_BY") != "0")
                {
                    string audtBy = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT a.AUDT_BY||'~'||TO_CHAR(a.aUDT_DT,'DD/MM/YYYY') AS COL1 FROM VOUCHER a WHERE a.branchcd||a.type||trim(a.VCHNUM)||to_Char(a.VCHDATE,'dd/mm/yyyy')='" + mv_col + "' AND (NVL(A.AUDT_BY,'-')!='-' or SUBSTR(A.AUDT_BY,1,3)='[R]') ", "COL1");
                    if (audtBy.Left(3) != "[R]")
                    {
                        if (audtBy != "0")
                        {
                            fgen.msg("-", "AMSG", "This voucher has been audited by " + audtBy.Split('~')[0] + " on " + audtBy.Split('~')[1] + ", can not be edit. Please contact to admin");
                            return;
                        }
                    }
                }
            }

            frm_vty = mv_col.Substring(2, 2);
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
            lbl1a.Text = frm_vty;
            ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
            ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

            txtvchnum.Text = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
            txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

            fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", txtvchnum.Text + txtvchdate.Text);
            ViewState["fstr"] = txtvchnum.Text + txtvchdate.Text;

            txtlbl4.Text = dt.Rows[i]["Acode"].ToString().Trim();
            txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();



            txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");
            txtlbl73.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM famst WHERE trim(acode)='" + txtlbl4.Text.Trim() + "'", "STATEn");

            string mgst_no = "";
            mgst_no = dt.Rows[i]["gst_no"].ToString().Trim();

            if (mgst_no.Length > 10)
            {
                txtTax.Text = "Y";
            }
            else
            {
                txtTax.Text = "N";
            }

            if (frm_vty == "45")
            {
                col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT CESSRATE FROM FAMST WHERE ACODE='" + txtlbl4.Text.Trim() + "'", "CESSRATE");
                if (col3 != "0") txtTCS.Text = col3;
                else txtTCS.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT nvl(params,0) as params from controls where id='D38'", "params");
            }
            else txtTCS.Text = "0";

            txtImpTaxValue.Text = dt.Rows[0]["TXB_PUNIT"].ToString().Trim();
            txtFr.Text = dt.Rows[0]["BILLFRT"].ToString().Trim();

            txtcc_1.Text = dt.Rows[0]["MATTYPE"].ToString().Trim() + ":" + fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NAME FROM TYPE WHERE ID='}' AND TYPE1='" + dt.Rows[0]["MATTYPE"].ToString().Trim() + "'", "name");

            if (frm_vty == "58" || frm_vty == "59")
            {
                txtbizgrp.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NAME FROM TYPE WHERE ID='$' AND TYPE1='" + dt.Rows[0]["potype"].ToString().Trim() + "'", "name") + ":" + dt.Rows[0]["potype"].ToString().Trim();
            }

            create_tab();
            sg1_dr = null;
            for (i = 0; i < dt.Rows.Count; i++)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                sg1_dr["sg1_h1"] = dt.Rows[i]["billrate"].ToString().Trim();
                sg1_dr["sg1_h2"] = dt.Rows[i]["EXP_PUNIT"].ToString().Trim();
                sg1_dr["sg1_h3"] = dt.Rows[i]["doc_tot"].ToString().Trim();
                sg1_dr["sg1_h4"] = dt.Rows[i]["potype"].ToString().Trim();

                if (frm_formID == "F70122")
                {
                    sg1_dr["sg1_h5"] = dt.Rows[i]["VCODE"].ToString().Trim();
                    sg1_dr["sg1_h6"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(ACODE)='" + dt.Rows[i]["VCODE"].ToString().Trim() + "'", "ANAME");
                    sg1_dr["sg1_h7"] = dt.Rows[i]["REFNUM"].ToString().Trim();
                    sg1_dr["sg1_h8"] = Convert.ToDateTime(dt.Rows[i]["refdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                }

                sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                sg1_dr["sg1_f2"] = dt.Rows[i]["purpose"].ToString().Trim();
                sg1_dr["sg1_f3"] = dt.Rows[i]["ICpartno"].ToString().Trim();
                sg1_dr["sg1_f4"] = dt.Rows[i]["Iunit"].ToString().Trim();
                sg1_dr["sg1_f5"] = dt.Rows[i]["no_proc"].ToString().Trim();


                if (lbl1a.Text.Substring(0, 2) == "50" || lbl1a.Text.Substring(0, 2) == "51" || lbl1a.Text.Substring(0, 2) == "53")
                {
                    ////oporow["iqty_chl"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().toDouble();
                    ////oporow["iqtyin"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().toDouble();
                    ////oporow["iqty_Wt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().toDouble();

                    sg1_dr["sg1_t1"] = dt.Rows[i]["iqty_chl"].ToString().Trim();
                    sg1_dr["sg1_t2"] = dt.Rows[i]["iqtyin"].ToString().Trim();
                    sg1_dr["sg1_t3"] = dt.Rows[i]["iqty_wt"].ToString().Trim();

                }
                else
                {
                    //oporow["rcode"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().toDouble();
                    //oporow["iqty_chl"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().toDouble();

                    //oporow["iqtyin"] = 0;
                    //oporow["iqty_Wt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().toDouble();

                    sg1_dr["sg1_t1"] = dt.Rows[i]["COL1"].ToString().Trim();
                    sg1_dr["sg1_t2"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE ACODE='" + dt.Rows[i]["COL1"].ToString().Trim() + "'", "ANAME");
                    sg1_dr["sg1_t3"] = dt.Rows[i]["iqty_chl"].ToString().Trim();

                }


                sg1_dr["sg1_t4"] = dt.Rows[i]["irate"].ToString().Trim();
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
                sg1_dr["sg1_t21"] = dt.Rows[i]["fabtype"].ToString().Trim();

                sg1_dr["sg1_t22"] = dt.Rows[i]["CC1"].ToString().Trim() + ":" + fgen.seek_iname(frm_qstr, frm_cocd, "select Name from typegrp where branchcd!='DD' and id='L1' and type1='" + dt.Rows[i]["CC1"].ToString().Trim() + "'", "NAME");
                sg1_dr["sg1_t23"] = dt.Rows[i]["CC2"].ToString().Trim() + ":" + fgen.seek_iname(frm_qstr, frm_cocd, "select Name from typegrp where branchcd!='DD' and id='L2' and type1='" + dt.Rows[i]["CC2"].ToString().Trim() + "'", "NAME");
                sg1_dr["sg1_t24"] = dt.Rows[i]["CC3"].ToString().Trim() + ":" + fgen.seek_iname(frm_qstr, frm_cocd, "select Name from typegrp where branchcd!='DD' and id='L3' and type1='" + dt.Rows[i]["CC3"].ToString().Trim() + "'", "NAME");
                sg1_dr["sg1_t25"] = dt.Rows[i]["PR_SEGMENT"].ToString().Trim() + ":" + fgen.seek_iname(frm_qstr, frm_cocd, "select Name from typegrp where branchcd!='DD' and id='BZ' and type1='" + dt.Rows[i]["PR_SEGMENT"].ToString().Trim() + "'", "NAME");

                sg1_dr["sg1_t26"] = dt.Rows[i]["COM_AMT"].ToString().Trim().toDouble();

                sg1_dt.Rows.Add(sg1_dr);
            }

            sg1_add_blankrows();
            ViewState["sg1"] = sg1_dt;
            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            dt.Dispose();
            sg1_dt.Dispose();
            //------------------------
            SQuery = "Select to_char(a.vchdate,'dd/mm/yyyy') as vchdt,to_char(a.remvdate,'dd/mm/yyyy') as rmvdtd,to_char(a.podate,'dd/mm/yyyy') as podtd,a.* from " + frm_tab_sale + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + mv_col + "' ";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

            create_tab2();
            sg2_dr = null;
            i = 0;
            if (dt.Rows.Count > 0)
            {
                txtlbl2.Text = dt.Rows[i]["remvtime"].ToString().Trim();
                txtlbl3.Text = dt.Rows[i]["rmvdtd"].ToString().Trim();

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

                txtlbl24.Text = dt.Rows[i]["fob_frt"].ToString().Trim();
                txtlbl26.Text = dt.Rows[i]["fob_ins"].ToString().Trim();

                txtlbl28.Text = dt.Rows[i]["fob_tot"].ToString().Trim();
                txtlbl30.Text = dt.Rows[i]["fob_oth"].ToString().Trim();

                txtrmk.Text = dt.Rows[i]["naration"].ToString().Trim();

                txtlbl25.Text = dt.Rows[i]["amt_sale"].ToString().Trim();
                txtlbl27.Text = dt.Rows[i]["amt_Exc"].ToString().Trim();
                txtlbl29.Text = dt.Rows[i]["rvalue"].ToString().Trim();
                txtlbl31.Text = dt.Rows[i]["bill_tot"].ToString().Trim();


                // ROW WISE COST CENTER GIVEN
                //if (dt.Rows[i]["FCOTH1"].ToString().Trim() != "0")
                //{
                //    txtbizgrp.Text = dt.Rows[i]["FCOTH1"].ToString().Trim() + ":" + fgen.seek_iname(frm_qstr, frm_cocd, "select Name from typegrp where branchcd!='DD' and id='BZ' and type1='" + dt.Rows[i]["FCOTH1"].ToString().Trim() + "'", "NAME");
                //}

                //if (dt.Rows[i]["FCOTH2"].ToString().Trim() != "0")
                //{
                //    txtcc_1.Text = dt.Rows[i]["FCOTH2"].ToString().Trim() + ":" + fgen.seek_iname(frm_qstr, frm_cocd, "select Name from typegrp where branchcd!='DD' and id='L1' and type1='" + dt.Rows[i]["FCOTH2"].ToString().Trim() + "'", "NAME");
                //}
                //if (dt.Rows[i]["FCOTH3"].ToString().Trim() != "0")
                //{
                //    txtcc_2.Text = dt.Rows[i]["FCOTH3"].ToString().Trim() + ":" + fgen.seek_iname(frm_qstr, frm_cocd, "select Name from typegrp where branchcd!='DD' and id='L2' and type1='" + dt.Rows[i]["FCOTH3"].ToString().Trim() + "'", "NAME");
                //}

                //if (dt.Rows[i]["FCOTH4"].ToString().Trim() != "0")
                //{
                //    txtcc_3.Text = dt.Rows[i]["FCOTH4"].ToString().Trim() + ":" + fgen.seek_iname(frm_qstr, frm_cocd, "select Name from typegrp where branchcd!='DD' and id='L3' and type1='" + dt.Rows[i]["FCOTH4"].ToString().Trim() + "'", "NAME");
                //}


                //txtGrno.Text = dt.Rows[i]["GRNO"].ToString().Trim();
                //txtGrDt.Text = dt.Rows[i]["GRDATE"].ToString().Trim();
            }
            //-----------------------
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
            SQuery = "Select a.acode,b.aname,nvl(a.tfcdr,0) as dramt,nvl(a.tfccr,0) as cramt,a.REFNUM,a.refdate,a.naration from voucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + mv_col + "' AND SUBSTR(A.NARATION,1,3)!='TDS' ORDER BY A.SRNO ";
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
                    sg3_dr["sg3_f1"] = dt.Rows[i]["acode"].ToString().Trim();
                    sg3_dr["sg3_f2"] = dt.Rows[i]["aname"].ToString().Trim();
                    sg3_dr["sg3_t1"] = dt.Rows[i]["dramt"].ToString().Trim();
                    sg3_dr["sg3_t2"] = dt.Rows[i]["cramt"].ToString().Trim();

                    sg3_dr["sg3_t3"] = dt.Rows[i]["REFNUM"].ToString().Trim();
                    sg3_dr["sg3_t4"] = Convert.ToDateTime(dt.Rows[i]["REFDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                    sg3_dr["sg3_t5"] = dt.Rows[i]["naration"].ToString().Trim();
                    sg3_dt.Rows.Add(sg3_dr);
                }
            }
            sg3_add_blankrows();
            ViewState["sg3"] = sg3_dt;
            sg3.DataSource = sg3_dt;
            sg3.DataBind();
            dt.Dispose();
            sg3_dt.Dispose();
            //------------------------

            SQuery = "Select a.* from wb_pv_head a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + mv_col + "'";
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            if (dt.Rows.Count > 0)
            {
                txtTCSA.Text = dt.Rows[0]["tcsamt"].ToString();

                txtTDSAmt.Text = dt.Rows[0]["ADCAMT"].ToString();
                txtTDSPer.Text = dt.Rows[0]["FRT_STAX"].ToString();
            }

            //-----------------------
            ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

            fgen.EnableForm(this.Controls);
            disablectrl();
            setColHeadings();
            edmode.Value = "Y";
        }
        #endregion
    }
    protected void txtBarCode_TextChanged(object sender, EventArgs e)
    {
        string barcode = "";
        barcode = txtBarCode.Text;
        if (barcode.Length > 18) barcode = barcode.Substring(0, 18);
        SQuery = "SELECT distinct a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum AS FSTR,trim(a.Vchnum) as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,b.Aname as Supplier,a.Invno,A.Refnum as chl_no,a.type,a.Ent_by,to_char(a.vchdate,'yyyymmdd') As vdd,a.inspected from ivoucher a ,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.branchcd||a.type||a.vchnum||to_char(A.vchdate,'yyyymmdd')='" + barcode + "'";
        SQuery = "SELECT DISTINCT a.type||A.Vchnum||to_char(A.Vchdate,'dd/mm/yyyy')||trim(a.Acode) as Fstr,A.Vchnum AS MRR_Number,to_Char(a.Vchdate,'dd/mm/yyyy') as MRR_Date,B.Aname as Supplier,a.Invno,to_chaR(a.Invdate,'dd/mm/yyyy') as Inv_Dt,a.refnum,to_Char(a.vchdate,'yyyymmdd') as VDD,TRIM(A.ACODE) as acode FROM ivoucher a, famst b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.BRANCHCD='" + frm_mbr + "' and a.TYPE like '0%' and a.branchcd||a.type||a.vchnum||to_char(A.vchdate,'yyyymmdd')='" + barcode + "' AND (a.type||a.vchnum||to_char(a.vchdate,'yyyymm')) IN (SELECT VCHNUM FROM (SELECT X.VCHNUM,SUM(X.aBC) AS CNT FROM (select trim(a.type)||a.vchnum||to_char(a.vchdate,'yyyymm') as vchnum,a.type,1 AS ABC from ivoucher a where branchcd='" + frm_mbr + "' AND a.type like '0%' and a.vchdate " + DateRange + " and a.store='Y' UNION ALL select trim(a.fabtype)||nvl(a.tc_no,'-')||to_char(a.refdate,'yyyymm') as genum,a.type,-1 AS ABC from wb_pv_DTL a where branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + DateRange + " ) X GROUP BY X.VCHNUM) WHERE CNT>0) order by vdd";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            hffield.Value = "PICK_MRR";
            txtlbl4.Text = dt.Rows[0]["acode"].ToString().Trim();
            txtlbl4a.Text = dt.Rows[0]["Supplier"].ToString().Trim();
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + dt.Rows[0]["fstr"].ToString().Trim() + "'");
            btnhideF_Click("", EventArgs.Empty);
        }
        else fgen.msg("Wrong Barcode!!", "AMSG", "Material Receipt Note not found !! Please scan other barcode");
        txtBarCode.Text = "";
    }
    protected void btnPO_Click(object sender, EventArgs e)
    {
        cond = "";
        if (frm_vty == "56") cond = "07";
        else cond = "02";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OVTY", lbl1a.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OFORMID", frm_formID);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "F15106");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_DOC_VIEW", "Y");
        col2 = "";
        col2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT DISTINCT TRIM(POTYPE)||'~'||TRIM(PONUM)||TO_cHAR(PODATE,'DD/MM/YYYY') AS FSTR FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '" + cond + "%' AND TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "'  and nvl(potype,'-')!='-'", "FSTR");
        if (col2.Length > 4)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col2.Split('~')[0]);

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col2.Split('~')[1]);
            fgen.open_fileUploadPopup("See the Attached File of PO ", frm_qstr);
        }
    }
    protected void btnMRR_Click(object sender, EventArgs e)
    {
        cond = "";
        if (frm_vty == "56") cond = "07";
        else cond = "02";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", cond);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OVTY", lbl1a.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OFORMID", frm_formID);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "F25101");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_DOC_VIEW", "Y");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", txtlbl2.Text.Trim() + txtlbl3.Text.Trim());
        fgen.open_fileUploadPopup("See the Attached File of MRR ", frm_qstr);
    }
    protected void btnGE_Click(object sender, EventArgs e)
    {
        cond = "";
        cond = "00";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", cond);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OVTY", lbl1a.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OFORMID", frm_formID);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", "F20101");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_DOC_VIEW", "Y");
        if (frm_vty == "56") cond = "07";
        else cond = "02";
        col2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(GENUM)||TO_cHAR(GEDATE,'DD/MM/YYYY') AS FSTR FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '" + cond + "%' AND TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "'  ", "FSTR");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col2);
        fgen.open_fileUploadPopup("See the Attached File of GE ", frm_qstr);
    }
    protected void btnPOView_Click(object sender, EventArgs e)
    {
        cond = "";
        if (frm_vty == "56") cond = "07";
        else cond = "02";

        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", cond);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OVTY", lbl1a.Text.Trim());

        //col2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT DISTINCT TRIM(POTYPE)||'~'||TRIM(PONUM)||TO_cHAR(PODATE,'DD/MM/YYYY') AS FSTR FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '" + cond + "%' AND TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "'  and nvl(potype,'-')!='-'", "FSTR");
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT DISTINCT TRIM(POTYPE) as potype,TRIM(PONUM)||TO_cHAR(PODATE,'DD/MM/YYYY') AS FSTR FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '" + cond + "%' AND TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "'  and nvl(potype,'-')!='-'");
        if (dt.Rows.Count > 0)
        {
            col3 = "";
            foreach (DataRow drp in dt.Rows)
            {
                col2 = drp["potype"].ToString().Trim();
                col3 += "," + "'" + drp["fstr"].ToString().Trim() + "'";
            }
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col2);
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", col3.TrimStart(','));
            fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1004");
            fgen.fin_purc_reps(frm_qstr);
        }
    }
    protected void btnMRRView_Click(object sender, EventArgs e)
    {
        if (frm_vty == "56") cond = "07";
        else cond = "02";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OVTY", lbl1a.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", cond);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "'");
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1002");
        fgen.fin_invn_reps(frm_qstr);
    }
    protected void btnGEView_Click(object sender, EventArgs e)
    {
        cond = "";
        if (frm_vty == "56") cond = "07";
        else cond = "02";
        cond = "0";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "00");
        col2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(GENUM)||TO_cHAR(GEDATE,'DD/MM/YYYY') AS FSTR FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '" + cond + "%' AND TRIM(VCHNUM)||TO_CHAR(vCHDATE,'DD/MM/YYYY')='" + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "'  ", "FSTR");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OVTY", lbl1a.Text.Trim());
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + col2 + "'");
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1001");
        fgen.fin_gate_reps(frm_qstr);
    }
    protected void ImageButton5_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "INVOICE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Invoice Number", frm_qstr);
    }
}