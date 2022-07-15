using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Linq;

public partial class om_Da_entry : System.Web.UI.Page
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
    string pop_qry = "";
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tab_ivch, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, cond = "";
    string mv_col;
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
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    hfcocd.Value = frm_cocd;
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                //---------------------------------
                string chk_opt = "";
                string chk_opt_yn = "";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_BATCH_INV", "N");

                doc_GST.Value = "Y";
                doc_hosopw.Value = "N";
                SQuery = "select opt_id,trim(upper(OPT_ENABLE)) as OPT_ENABLE from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID in ('W1100','W2017','W2027','W2019') order by OPT_ID";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        chk_opt = dt.Rows[i]["OPT_ID"].ToString().Trim();
                        chk_opt_yn = dt.Rows[i]["OPT_ENABLE"].ToString().Trim();
                        switch (chk_opt)
                        {
                            case "W1100"://branch wise HO SO system
                                if (chk_opt_yn == "Y") { doc_hosopw.Value = "Y"; }
                                if (frm_formID == "F56010")
                                    doc_hosopw.Value = "N";
                                break;

                            case "W2017"://INDIA GST
                                if (chk_opt_yn == "N") { doc_GST.Value = "N"; }
                                break;

                            case "W2027"://Member GCC Country
                                if (chk_opt_yn == "Y") { doc_GST.Value = "GCC"; }
                                break;

                            case "W2019"://Batch Wise
                                if (chk_opt_yn == "Y")
                                {
                                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_BATCH_INV", chk_opt_yn);
                                }
                                break;
                        }
                    }
                }

                hfW18.Value = fgen.getOption(frm_qstr, frm_cocd, "W0118", "OPT_ENABLE");
                hfW120.Value = fgen.getOption(frm_qstr, frm_cocd, "W0120", "OPT_ENABLE");
                dt.Dispose();

                doc_hoso.Value = "N";
                //SQuery = "select opt_id,trim(upper(OPT_ENABLE)) as OPT_ENABLE from FIN_RSYS_OPT where OPT_ID in ('W0052') order by OPT_ID";
                //dt = new DataTable();
                //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //if (dt.Rows.Count > 0)
                //{
                //    for (i = 0; i < dt.Rows.Count; i++)
                //    {
                //        chk_opt = dt.Rows[i]["OPT_ID"].ToString().Trim();
                //        chk_opt_yn = dt.Rows[i]["OPT_ENABLE"].ToString().Trim();
                //        switch (chk_opt)
                //        {
                //            case "W0052":   //HO Based SO 
                //                if (chk_opt_yn == "Y") { doc_hoso.Value = "Y"; }
                //                break;
                //        }
                //    }
                //}
                dt.Dispose();

                //---------------------------------
                //if (frm_cocd == "SGRP" && (frm_formID == "F55111"||frm_formID=="F50111"))
                //{
                //    netwt.Visible = true;
                //    groswt.Visible = true;
                //}
                //else
                //{
                //    netwt.Visible = false;
                //    groswt.Visible = false;
                //}

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

            for (int K = 0; K < sg1.Rows.Count; K++)
            {

                #region hide hidden columns
                for (int i = 0; i < 10; i++)
                {
                    if (frm_formID == "F50111" && frm_cocd == "AERO" && i == 8)
                    {
                        sg1.HeaderRow.Cells[sR].Text = "Product Weight";
                    }
                    else
                    {
                        sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                        sg1.Rows[K].Cells[i].CssClass = "hidden";
                    }
                }
                #endregion

                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

                ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");


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
                    //sg1.Columns[sR].Visible = false;
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


            //if (sR == tb_Colm)
            //{
            //    // hidding column
            //    if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
            //    {
            //        sg1.Columns[sR].Visible = false;
            //    }
            //    // Setting Heading Name
            //    sg1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
            //    // Setting Col Width
            //    string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
            //    if (fgen.make_double(mcol_width) > 0)
            //    {
            //        sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
            //        sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
            //    }
            //}
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
            case "F50111":
            case "F55111":
            case "F50116":
                tab2.Visible = false;
                tab3.Visible = false;
                tab4.Visible = false;
                tab5.Visible = false;
                //tab6.Visible = false;
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
        doc_nf.Value = "packno";
        doc_df.Value = "packdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F50111":
            case "F55111":
            case "F56010":
                frm_tab_ivch = "DESPATCH";
                break;

        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TAB_IVCH", frm_tab_ivch);
        btnItemSelection.Visible = false;
        txtscanbarcode.AutoPostBack = false;
        if (frm_cocd == "RWPL")
        {
            txtscanbarcode.AutoPostBack = true;
            btnItemSelection.Visible = true;
            txtscanbarcode.TextMode = TextBoxMode.SingleLine;
            Button1.Visible = false;
            Button2.Visible = false;
            Button3.Visible = false;
            Button4.Visible = false;
        }
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        cond = " TYPE='" + frm_vty + "'";
        if (frm_cocd == "AGRM" || frm_cocd == "KESR") cond = " TYPE like '4%'";

        if (frm_vty != "4F")
            btnlbl18.Enabled = false;
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        string ord_br_Str = "";
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tab_ivch = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_IVCH");

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

            case "BTN_20":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM FAMST where trim(nvl(GRP,'-')) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_21":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM FAMST where trim(nvl(GRP,'-')) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_22":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM FAMST where trim(nvl(GRP,'-')) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_23":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1,rate  FROM TYPE WHERE ID='A' order by name ";
                break;
            case "TACODE":
                //pop1
                //
                ord_br_Str = "a.branchcd='" + frm_mbr + "'";
                if (doc_hosopw.Value == "Y")
                {
                    ord_br_Str = "a.branchcd='00'";
                }
                // check for SAIA 
                ord_br_Str = "a.branchcd='" + frm_mbr + "'";
                if (doc_hosopw.Value == "Y")
                {
                    ord_br_Str = "a.branchcd='00' and trim(nvl(a.mfginbr,'-'))='" + frm_mbr + "'";
                }

                SQuery = "SELECT distinct a.ACODE AS FSTR,b.ANAME AS PARTY,a.ACODE AS CODE,b.ADDR1,b.ADDR2,b.staten as state,Pay_num,b.Grp FROM somas a, FAMST b where " + ord_br_Str + " and a.type='" + frm_vty + "' and trim(nvl(a.ICAT,'-'))!='Y'  and trim(nvl(a.app_by,'-'))!='-' and trim(A.acode)=trim(B.acode) and length(Trim(nvl(b.deac_by,'-')))<=1 ORDER BY aname ";
                if (lbl1a.Text == "47")
                {
                    SQuery = "SELECT distinct a.ACODE AS FSTR,a.ANAME AS PARTY,a.ACODE AS CODE,a.ADDR1,a.ADDR2,a.staten as state,a.Pay_num,a.Grp FROM FAMST a where SUBSTR(a.ACODE,1,2)='06' and length(Trim(nvl(a.deac_by,'-')))<=1 ORDER BY a.aname ";
                }
                if (frm_cocd == "HEXP")
                {
                    SQuery = "SELECT distinct a.ACODE AS FSTR,b.ANAME AS PARTY,a.ACODE AS CODE,trim(a.ordno) as ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,b.staten as state,b.ADDR1,b.ADDR2,b.Pay_num,b.Grp FROM somas a, FAMST b where " + ord_br_Str + " and a.type='" + frm_vty + "' and trim(nvl(a.ICAT,'-'))!='Y' and trim(nvl(a.app_by,'-'))!='-' and trim(A.acode)=trim(B.acode) and length(Trim(nvl(b.deac_by,'-')))<=1 and to_char(a.orddt,'yyyymmdd')||'-'||trim(a.ordno)||'-'||trim(a.icode) in (select trim(fstr) as acode from (select a.Fstr,max(a.Iname)as Item_Name,a.ERP_code,max(a.Cpartno)as Part_no,max(a.Irate) As Irate,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as PO_No,a.Fstr as SO_link,max(a.cdisc) as CDisc,max(a.iexc_Addl) as iexc_Addl,max(a.sd) as frt_pu,max(a.ipack) as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty,trim(A.cdrgno) As line_no,B.PACKSIZE AS STD_PACK,max(a.currency) as currency,trim(a.acode) as acode from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,pordno,(Case when length(trim(nvl(desc9,'-')))>1 then desc9 else ciname end) as Iname,trim(Icode) as ERP_code,Cpartno,Irate,Qtyord,0 as Soldqty,nvl(Cdisc,0) as Cdisc,nvl(iexc_addl,0) as Iexc_Addl,nvl(sd,0) as sd ,nvl(ipack,0) as ipack,ordno cdrgno,currency,trim(acode) as acode from somas where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(icat)!='Y' and trim(nvl(app_by,'-'))!='-' and 1=1 union all SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,null as pordno,null as Iname,trim(Icode) as ERP_code,null as Cpartno,0 as Irate,0 as Qtyord,qtysupp as Soldqty,0 as Cdisc,0 as iexc_Addl,0 as sd,0 as ipack,ordno ordline,null as currency,trim(acode) as acode  from despatch where branchcd='" + frm_mbr + "' and type='" + frm_vty + "')a,item b where trim(a.erp_code)=trim(B.icode)  group by a.fstr,trim(A.cdrgno),a.ERP_code,b.unit,b.hscode,B.PACKSIZE,trim(a.acode) having (case when sum(a.Qtyord)>0 then sum(a.Qtyord)-sum(a.Soldqty) else max(a.irate) end)>0 ) ) ORDER BY aname ";
                }

                SQuery = "Select a.*,b.Name from (" + SQuery + ") a left outer join (select Name,type1 from type where id='Z') b on trim(A.grp)=trim(B.type1) order by Party";

                break;
            case "TICODE":
                //pop2
                SQuery = "SELECT ACODE AS FSTR,ANAME AS PARTY,ACODE AS CODE,ADDR1,ADDR2 FROM CSMST where branchcd!='DD' and trim(tcsnum)='" + txtlbl4.Text + "' ORDER BY aname ";
                //SQuery = "SELECT ICODE AS FSTR,INAME AS PRODUCT,ICODE AS CODE,UNIT,CPARTNO AS PARTNO FROM ITEM WHERE LENGTH(tRIM(ICODE))>4 ";
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

                col1 = "";
                if (col1.Length <= 0) col1 = "'-'";
                //pop1
                ord_br_Str = "branchcd='" + frm_mbr + "'";
                if (doc_hosopw.Value == "Y")
                {
                    ord_br_Str = "branchcd='00'";
                }
                // vipin chk_below_command
                ord_br_Str = "branchcd='" + frm_mbr + "'";
                string more_Cond = "";
                more_Cond = "1=1";
                if (frm_cocd == "SAGM")
                {
                    more_Cond = "trim(weight)='" + frm_mbr + "'";
                }

                if (doc_hosopw.Value == "Y")
                {
                    ord_br_Str = "branchcd='00' and trim(nvl(mfginbr,'-'))='" + frm_mbr + "'";
                }

                SQuery = "select a.Fstr,max(a.Iname)as Item_Name,a.ERP_code,max(a.Cpartno)as Part_no,max(a.Irate) As Irate,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as PO_No,a.Fstr as SO_link,max(a.cdisc) as CDisc,max(a.iexc_Addl) as iexc_Addl,max(a.sd) as frt_pu,max(a.ipack) as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty,trim(A.cdrgno) As line_no,B.PACKSIZE AS STD_PACK,max(a.currency) as currency from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,pordno,(Case when length(trim(nvl(desc9,'-')))>1 then desc9 else ciname end) as Iname,trim(Icode) as ERP_code,Cpartno,Irate,Qtyord,0 as Soldqty,nvl(Cdisc,0) as Cdisc,nvl(iexc_addl,0) as Iexc_Addl,nvl(sd,0) as sd ,nvl(ipack,0) as ipack,cdrgno,currency from somas where " + ord_br_Str + " and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "' and trim(icat)!='Y' and trim(nvl(app_by,'-'))!='-' and " + more_Cond + " union all SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,null as pordno,null as Iname,trim(Icode) as ERP_code,null as Cpartno,0 as Irate,0 as Qtyord,qtysupp as Soldqty,0 as Cdisc,0 as iexc_Addl,0 as sd,0 as ipack,ordline,null as currency  from despatch where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "')a,item b where trim(a.erp_code)=trim(B.icode)  group by a.fstr,trim(A.cdrgno),a.ERP_code,b.unit,b.hscode,B.PACKSIZE having (case when sum(a.Qtyord)>0 then sum(a.Qtyord)-sum(a.Soldqty) else max(a.irate) end)>0 order by Item_Name,a.fstr";
                if (lbl1a.Text == "47")
                {
                    SQuery = "select a.Fstr,b.Iname as Item_Name,a.ERP_code,b.Cpartno as Part_no,max(a.Irate) As Irate,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as Inv_No,a.Fstr as MRR_link,0 as CDisc,0 as iexc_Addl,0 as frt_pu,0 as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty,'-' As line_no,B.PACKSIZE AS STD_PACK,'-' as currency from (SELECT to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(Icode) as fstr,invno as pordno,trim(Icode) as ERP_code,Irate,nvl(rej_rw,0) as Qtyord,0 as Soldqty,'-' as currency from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and trim(acode)='" + txtlbl4.Text + "' and trim(nvl(store,'-'))='Y' and rej_rw>0 union all SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,'-' as pordno,trim(Icode) as ERP_code,Irate,0 as Qtyord,qtysupp as Soldqty,null as currency from despatch where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "')a,item b where trim(a.erp_code)=trim(B.icode)  group by a.Fstr,b.Iname,a.ERP_code,b.Cpartno,b.Unit,b.hscode,a.Fstr,B.PACKSIZE order by b.iname,a.fstr";
                }

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);

                break;
            case "SG1_ROW_TAX":

                SQuery = "Select Type1 as fstr,Name,Type1 as Code,nvl(Rate,0) as Rate,nvl(Excrate,0) as Schg,exc_Addr as Ref_Code from type where id='S' and length(Trim(nvl(cstno,'-')))<=1 order by name";
                break;
            case "BATCH":
                col1 = "";
                if (btnval != "SG3_ROW_ADD" && btnval != "SG3_ROW_ADD_E")
                {
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        if (((TextBox)gr.FindControl("sg1_t2")).Text.Trim().Length > 2)
                        {
                            if (col1.Length > 0) col1 = col1 + ",'" + ((TextBox)gr.FindControl("sg1_t2")).Text.Trim() + "'";
                            else col1 = "'" + ((TextBox)gr.FindControl("sg1_t2")).Text.Trim() + "'";
                        }
                    }
                }
                //col1 = "";
                string xprd1 = " BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                string xprd2 = " BETWEEN TO_DATE('" + fromdt + "','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')-1";
                if (col1 != "") col1 = " and a.btchno not in (" + col1 + ")";
                string mq = "SELECT * FROM (select b.iname,b.no_proc,b.unit,b.bfactor,b.oprate1 as psize,b.oprate3 as gsm,b.oprate1,b.oprate2,b.oprate3,trim(a.kclreelno)as My_reel,min(vchdate) as Vchdate,max(trim(upper(a.coreelno))) as Co_reel,trim(a.icode) as Icode,sum(a.opening) as op,sum(pdr) as pwd,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+SUM(A.PDR)+sum(a.cdr)-sum(a.ccr) as closing,MAX(ACODE) AS ACODE,substr(a.icode,1,4) as Igrp,max(insp_done) as Insp_done,max(origwt) as origwt,max(rlocn) as rlocn,max(reel_mill) as reel_mill from (Select null as vchdate,kclreelno,null as coreelno,icode, reelwin as opening,0 as pdr,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE,null as insp_done,0 as origwt,rlocn,'-' as reel_mill from reelvch where branchcd='" + frm_mbr + "'  and substr(nvl(rinsp_by,'-'),1,6)='REELOP' and 1=1 union all  select min(vchdate) As vchdate,kclreelno,coreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as pdr,0 as cdr,0 as ccr,0 as clos,max(ACODE) As ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' as reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,sum(reelwin) as pdr,0 as cdr,0 as ccr,0 as clos,MAX(aCODE) AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt ,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, sum(reelwin) as cdr,0 as ccr,0 as clos,max(ACODE) as acode,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '1%' and vchdate " + xprd2 + " and posted='Y' GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, 0 as cdr,sum(reelwout) as ccr,0 as clos,max(ACODE) as acode,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE )a,item b where trim(a.icode)=trim(B.icode) and nvl(b.oprate1,0) like '%' and nvl(b.oprate3,0) like '%' and nvl(b.bfactor,0) like '%'  group by b.iname,b.no_proc,b.unit,b.bfactor,b.oprate1,b.oprate2,b.oprate3,trim(a.icode),substr(a.icode,1,4),trim(a.kclreelno) )m where 1=1 and nvl(m.aCODE,'%') like '%'";
                if (frm_formID == "F50111")
                {
                    mq = "select trim(icodE) as icode,trim(btchno) as My_reel,iqtyin as closing,0 as iqtyout,nvl(iqty_wt,0) as iqty_wt from ivoucher where branchcd='" + frm_mbr + "' and (TYPE='3A' or type='15' OR TYPE='16') and STAGE='69' and vchdate " + DateRange + " ";
                    if (frm_cocd == "AEROx")
                        mq = "select trim(icodE) as icode,trim(invno) as My_reel,iqtyin as closing,0 as iqtyout,iqty_wt from ivoucher where branchcd='" + frm_mbr + "' and (TYPE='3A' or type='15' OR TYPE='16') and vchdate " + DateRange + " AND STORE='Y' ";
                }
                SQuery = "select a.btchno||'~'||sum(a.iqtyin-a.iqtyout) as fstr,b.iname as product,a.btchno as batch_no,sum(a.iqtyin-a.iqtyout) as bal,a.icode as erpcode,sum(a.iqty_wt) as weight from (select trim(icodE) as icode,trim(My_reel) as btchno,closing iqtyin,0 as iqtyout,0 iqty_wt from (" + mq + ") where trim(icode) in (" + hf2.Value + ") union all select trim(icode) as icode,trim(no_bdls) as batchno,0 as iqtyin,qtysupp,0 as iqtywt from despatch where branchcd='" + frm_mbr + "' and type like '4%' and packdate " + DateRange + " and trim(icode) in (" + hf2.Value + ") and trim(packno)||to_Char(packdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "' ) a,item b where trim(a.icode)=trim(B.icode) " + col1 + " group by a.btchno,a.icode,b.iname having sum(a.iqtyin-a.iqtyout)>0 order by a.btchno";
                if (frm_cocd == "AERO")
                    SQuery = "select a.btchno||'~'||sum(a.iqtyin-a.iqtyout) as fstr,b.iname as product,a.btchno as batch_no,sum(a.iqtyin-a.iqtyout) as bal,a.icode as erpcode,sum(a.iqty_wt) as weight from (select trim(icodE) as icode,trim(My_reel) as btchno,closing iqtyin,0 as iqtyout,iqty_wt from (" + mq + ") where trim(icode) in (" + hf2.Value + ") union all select trim(icode) as icode,trim(naration) as batchno,0 as iqtyin,qtysupp,0 as iqtywt from despatch where branchcd='" + frm_mbr + "' and type like '4%' and packdate " + DateRange + " and trim(icode) in (" + hf2.Value + ") and trim(packno)||to_Char(packdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "' ) a,item b where trim(a.icode)=trim(B.icode) " + col1 + " group by a.btchno,a.icode,b.iname having sum(a.iqtyin-a.iqtyout)>0 order by a.btchno";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "Atch_E")
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Doc_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Doc_Dt,b.Aname as Customer,b.addr1,b.Grp,b.Staten,B.Country,a.Acode,a.Ent_by,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tab_ivch + " a,famst b where  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " and  trim(a.acode)=trim(B.acodE) order by vdd desc,a." + doc_nf.Value + " desc";
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
            txtlbl2.Text = DateTime.Now.ToString("HH:mm").ToString();
            hffield.Value = "New";
            make_qry_4_popup();
            fgen.Fn_open_sseek("select Type", frm_qstr);
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
        if (frm_cocd != "RWPL")
        {
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

            //if (txtlbl24.Text.Trim().Length < 2)
            //{
            //    reqd_nc = reqd_nc + 1;
            //    reqd_flds = reqd_flds + " / " + lbl24.Text;

            //}
            if (txtlbl27.Text.Trim().Length < 2)
            {
                reqd_nc = reqd_nc + 1;
                reqd_flds = reqd_flds + " / " + lbl27.Text;

            }

            //if (txtlbl15.Text.Trim().Length < 2)
            //{
            //    reqd_nc = reqd_nc + 1;
            //    reqd_flds = reqd_flds + " / " + lbl15.Text;

            //}
            //if (txtlbl16.Text.Trim().Length < 2)
            //{
            //    reqd_nc = reqd_nc + 1;
            //    reqd_flds = reqd_flds + " / " + lbl16.Text;

            //}
            //if (txtlbl17.Text.Trim().Length < 2)
            //{
            //    reqd_nc = reqd_nc + 1;
            //    reqd_flds = reqd_flds + " / " + lbl17.Text;

            //}



            if (reqd_nc > 0)
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
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

            //if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2)
            //{
            //    if (sg1.Rows[i].Cells[3].Text.Trim().toDouble() < fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text))
            //    {
            //        Checked_ok = "N";
            //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Cannot be more then Order Qty'13'Order Qty : " + sg1.Rows[i].Cells[3].Text.Trim().toDouble() + "'13'DA Qty : " + ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text + "'13'Correctly at Line " + (i + 1) + "  !!");
            //        i = sg1.Rows.Count;
            //        return;

            //    }
            //}
            {   //allow zero rate from item master condition once - 19/05/21 - made for sgrp
                if (frm_cocd != "SGRP")
                {
                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NVL(WKFLG,'-') AS WKFLG FROM ITEM WHERE TRIM(ICODE)='" + sg1.Rows[i].Cells[13].Text.Trim() + "' ", "WKFLG");
                    if (col1 != "Y")
                    {
                        if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text) <= 0 && sg1.Rows[i].Cells[4].Text.Trim() != "ACCS-C")
                        {
                            Checked_ok = "N";
                            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Rate Not Filled Correctly at Line " + (i + 1) + "  !!");
                            i = sg1.Rows.Count;
                            return;
                        }
                    }
                }
            }
        }

        string last_entdt;
        //checks
        if (edmode.Value == "Y")
        {
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tab_ivch + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  and packdate " + DateRange + " and packno||to_char(packdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "' and packdate<=to_DaTE('" + txtvchdate.Text + "','dd/mm/yyyy') order by packdate desc", "ldt");
        }
        else
        {
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tab_ivch + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  and packdate " + DateRange + " and packno||to_char(packdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "' order by packdate desc", "ldt");
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

        if (frm_cocd != "MPAC")
        {
            if (ok_for_save == "N")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' Dispatch Qty is Exceeding Order Qty , Please Check '13' " + err_item_name + "'13' " + err_item);
                return;
            }
        }

        //**************** Stock Check
        if (Prg_Id == "F50106")
        {

        }
        else
        {
            // BYPASSING THE LOCK FOR FEW DAYS  - 10/06/2021
            if (frm_cocd != "SGRP")
                checkStockQty();
        }

        ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
        err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");

        if (frm_cocd != "MPAC")
        {
            if (ok_for_save == "N")
            {
                fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' Cannot Make DA more then Stock Qty , Please Check item : " + err_item);
                return;
            }
        }

        // for some time. 02/04/2021
        // BYPASSING THE LOCK FOR FEW DAYS  - 10/06/2021 - SGRP
        if (frm_cocd != "SGRP" && frm_cocd != "AERO")
        {
            if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_BATCH_INV") == "Y")
            {
                check_btch_StockQty();

                ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
                err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");

                if (ok_for_save == "N")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' Cannot Despatch more the Batch Stock Qty , Please Check item : " + err_item);
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

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        //btnsave.Disabled = true;
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

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "Y");
        DataView distQty = new DataView(dtQty, "", "fstr", DataViewRowState.CurrentRows);
        DataTable dtQty1 = new DataTable();
        dtQty1 = distQty.ToTable(true, "fstr");
        foreach (DataRow drQty1 in dtQty1.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "fstr='" + drQty1["fstr"].ToString().Trim() + "'");

            SQuery = "select trim(upper(a.batch_no)) as Fstr,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty from (SELECT trim(icode)||trim(btchno) as fstr,trim(btchno) as Batch_no,iqtyin as qtyord,0 as Soldqty from ivoucher where branchcd='" + frm_mbr + "' and type in ('3A','16','15','17') and trim(store) in ('W','Y') and stage='69'  and trim(icode)||'-'||trim(btchno)='" + drQty1["fstr"].ToString().Trim() + "' union all SELECT trim(icode)||trim(no_bdls) as fstr,trim(no_bdls) as Batch_no,0 as qtyord,QTYSUPP as Soldqty from despatch where branchcd='" + frm_mbr + "' and type like '4%' and trim(icode)||'-'||trim(no_bdls)='" + drQty1["fstr"].ToString().Trim() + "' and type||trim(packno)||to_Char(packdate,'dd/mm/yyyy') !='" + lbl1a.Text + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "')a  group by trim(fstr),trim(upper(a.batch_no))  having  sum(a.Qtyord)-sum(a.Soldqty) >0 order by trim(upper(a.batch_no))";

            if (frm_formID != "F50111")
            {
                string xprd1 = " BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                string xprd2 = " BETWEEN TO_DATE('" + fromdt + "','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')-1";
                col1 = "  ";
                string mq = "SELECT * FROM (select b.iname,b.no_proc,b.unit,b.bfactor,b.oprate1 as psize,b.oprate3 as gsm,b.oprate1,b.oprate2,b.oprate3,trim(a.kclreelno)as My_reel,min(vchdate) as Vchdate,max(trim(upper(a.coreelno))) as Co_reel,trim(a.icode) as Icode,sum(a.opening) as op,sum(pdr) as pwd,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+SUM(A.PDR)+sum(a.cdr)-sum(a.ccr) as closing,MAX(ACODE) AS ACODE,substr(a.icode,1,4) as Igrp,max(insp_done) as Insp_done,max(origwt) as origwt,max(rlocn) as rlocn,max(reel_mill) as reel_mill from (Select null as vchdate,kclreelno,null as coreelno,icode, reelwin as opening,0 as pdr,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE,null as insp_done,0 as origwt,rlocn,'-' as reel_mill from reelvch where branchcd='" + frm_mbr + "'  and substr(nvl(rinsp_by,'-'),1,6)='REELOP' and 1=1 union all  select min(vchdate) As vchdate,kclreelno,coreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as pdr,0 as cdr,0 as ccr,0 as clos,max(ACODE) As ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' as reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,sum(reelwin) as pdr,0 as cdr,0 as ccr,0 as clos,MAX(aCODE) AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt ,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, sum(reelwin) as cdr,0 as ccr,0 as clos,max(ACODE) as acode,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '1%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, 0 as cdr,sum(reelwout) as ccr,0 as clos,max(ACODE) as acode,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE )a,item b where trim(a.icode)=trim(B.icode) and nvl(b.oprate1,0) like '%' and nvl(b.oprate3,0) like '%' and nvl(b.bfactor,0) like '%'  group by b.iname,b.no_proc,b.unit,b.bfactor,b.oprate1,b.oprate2,b.oprate3,trim(a.icode),substr(a.icode,1,4),trim(a.kclreelno) )m where 1=1 and nvl(m.aCODE,'%') like '%'";
                SQuery = "select a.btchno||'~'||sum(a.iqtyin-a.iqtyout) as fstr,b.iname as product,a.btchno as batch_no,sum(a.iqtyin-a.iqtyout) as Balance_qty,a.icode as erpcode from (select trim(icodE) as icode,trim(My_reel) as btchno,closing iqtyin,0 as iqtyout from (" + mq + ") where trim(icode)||'-'||trim(my_Reel)='" + drQty1["fstr"].ToString().Trim() + "' union all select trim(icode) as icode,trim(no_bdls) as batchno,0 as iqtyin,qtysupp from despatch where branchcd='" + frm_mbr + "' and type like '4%' and packdate " + DateRange + " and trim(icode) in (" + hf2.Value + ") and type||trim(packno)||to_Char(packdate,'dd/mm/yyyy')!='" + lbl1a.Text + txtvchnum.Text + txtvchdate.Text + "' and trim(icode)||'-'||trim(no_bdls)='" + drQty1["fstr"].ToString().Trim() + "') a,item b where trim(a.icode)=trim(B.icode) " + col1 + " group by a.btchno,a.icode,b.iname having sum(a.iqtyin-a.iqtyout)>0 order by a.btchno";
            }
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
                drQty["fstr"] = gr.Cells[13].Text.ToString().Trim() + "-" + ((TextBox)gr.FindControl("sg1_t14")).Text.ToString().Trim() + "-" + ((TextBox)gr.FindControl("sg1_t16")).Text.ToString().Trim();
                drQty["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t3")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
            }
        }
        object sm;
        string ord_br_Str = "";
        ord_br_Str = "branchcd='" + frm_mbr + "'";
        if (doc_hoso.Value == "Y")
        {
            ord_br_Str = "branchcd='00'";
        }
        if (doc_hosopw.Value == "Y")
        {
            ord_br_Str = "branchcd='00' and trim(nvl(mfginbr,'-'))='" + frm_mbr + "'";
        }

        DataView distQty = new DataView(dtQty, "", "fstr", DataViewRowState.CurrentRows);
        DataTable dtQty1 = new DataTable();
        dtQty1 = distQty.ToTable(true, "fstr");
        foreach (DataRow drQty1 in dtQty1.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "fstr='" + drQty1["fstr"].ToString().Trim() + "'");
            string chk_itm;
            chk_itm = drQty1["fstr"].ToString().Trim().Substring(0, 8);
            string mqry;
            mqry = "select (a.Qtyord)-(a.Soldqty) as Bal_Qty from (select fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(Icode)||'-'||ordno||'-'||to_char(orddt,'dd/mm/yyyy') as fstr,trim(Icode) as ERP_code,Qtyord+(Qtyord*(nvl(qtysupp,0)/100)) as qtyord,0 as Soldqty,0 as prate from Somas where " + ord_br_Str + " and type like '" + lbl1a.Text + "%' and trim(icat)!='Y'  and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text.Trim() + "' union all SELECT trim(Icode)||'-'||trim(ordno)||'-'||to_char(orddt,'dd/mm/yyyy') as fstr,trim(Icode) as ERP_code,0 as Qtyordx,qtysupp as qtyord ,0 as irate from despatch where branchcd='" + frm_mbr + "' and type like '" + lbl1a.Text + "%' and packdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text.Trim() + "' and trim(packno)||to_Char(packdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and a.fstr='" + drQty1["fstr"].ToString().Trim() + "' order by B.Iname,trim(a.fstr)";
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, mqry, "Bal_Qty");

            if (fgen.make_double(sm.ToString()) > fgen.make_double(col1))
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
        hfsonum.Value = "";
        lblSODetail.Text = "";
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

        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tab_ivch + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data a where par_tbl='" + frm_tab_ivch + "' and par_fld='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                clearctrl(); fgen.ResetForm(this.Controls);
            }
        }
        else if (hffield.Value == "PRINT_ASK")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            if (col1 == "N")
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
            }
            else fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id + "P");
            fgen.fin_sales_reps(frm_qstr);
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

                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tab_ivch + " WHERE BRANCHCD='" + frm_mbr + "' AND " + cond + " AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
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
                    col3 = "";
                    mv_col = frm_mbr + frm_vty + col1;
                    col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(vCHNUM)||' / '||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR FROM IVOUCHER WHERE BRANCHCD||TYPE||TRIM(TC_NO)||TO_cHAR(refdate,'DD/MM/YYYY')='" + mv_col + "' ", "FSTR");
                    if (col3.Length > 5 && frm_ulvl != "0")
                    {
                        fgen.msg("-", "AMSG", "Invoice has been already made against the DA : " + col1.Substring(0, 6) + " '13'Invoice No. / Date : " + col3 + "'13'Can not Delete this!!");
                        return;
                    }
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;
                case "Print":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();


                    mv_col = frm_mbr + frm_vty + col1;
                    col3 = "";
                    col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(vCHNUM)||' / '||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS FSTR FROM IVOUCHER WHERE BRANCHCD||TYPE||TRIM(TC_NO)||TO_cHAR(refdate,'DD/MM/YYYY')='" + mv_col + "' ", "FSTR");
                    if (col3.Length > 5 && frm_ulvl != "0")
                    {
                        fgen.msg("-", "AMSG", "Invoice has been already made against the DA : " + col1.Substring(0, 6) + " '13'Invoice No. / Date : " + col3 + "'13'Can not Edit this");
                        return;
                    }

                    SQuery = "Select a.*,to_char(A.porddt,'dd/mm/yyyy') as podtd,to_char(A.orddt,'dd/mm/yyyy') as ordtd,c.Aname,nvl(b.cpartno,'-') As Icpartno,nvl(b.unit,'-') as IUnit,b.packsize from " + frm_tab_ivch + " a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        //ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txtvchnum.Text = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl4.Text = dt.Rows[i]["Acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();

                        //                        txtlbl70.Text = dt.Rows[i]["gst_pos"].ToString().Trim();
                        txtlbl71.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM famst WHERE trim(acode)='" + txtlbl70.Text.Trim() + "'", "STATEn");

                        txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");
                        txtlbl73.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM famst WHERE trim(acode)='" + txtlbl4.Text.Trim() + "'", "STATEn");

                        txtlbl7.Text = dt.Rows[0]["cscode"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname  from csmst where trim(upper(acode))=upper(Trim('" + txtlbl7.Text + "'))", "aname");

                        txtlbl15.Text = dt.Rows[0]["mode_tpt"].ToString().Trim();
                        txtlbl16.Text = dt.Rows[0]["thru"].ToString().Trim();
                        txtlbl17.Text = dt.Rows[0]["freight"].ToString().Trim();
                        txtlbl18.Text = dt.Rows[0]["currency"].ToString().Trim();

                        txtlbl8.Text = dt.Rows[0]["desp_to"].ToString().Trim();
                        txtlbl9.Text = dt.Rows[0]["PVT_MARK"].ToString().Trim();

                        txtlbl24.Text = dt.Rows[i]["mo_Vehi"].ToString().Trim();
                        txtlbl26.Text = dt.Rows[i]["weight"].ToString().Trim();
                        txtlbl28.Text = dt.Rows[i]["QTY_PKG"].ToString().Trim();//by yogita
                        txtlbl30.Text = dt.Rows[i]["amdt1"].ToString().Trim();
                        //for prep date and time            
                        txtlbl3.Text = Convert.ToDateTime(dt.Rows[i]["REMVDATE"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl2.Text = dt.Rows[i]["REMVTIME"].ToString().Trim();
                        //=============for sgrp net wt and gross wt editing...yogita....24/06/2021
                        txtgroswt.Text = dt.Rows[i]["NOF_BOX"].ToString().Trim();
                        txtnetwt.Text = dt.Rows[i]["QTY_BOX"].ToString().Trim();
                        //====remark   
                        txtrmk.Text = dt.Rows[i]["remark"].ToString().Trim();
                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = "-";
                            sg1_dr["sg1_h2"] = "-";
                            sg1_dr["sg1_h3"] = dt.Rows[i]["packsize"].ToString().Trim();
                            sg1_dr["sg1_h4"] = fgen.seek_iname(frm_qstr, frm_cocd, "select balance_qty from (select a.Fstr,max(a.Iname)as Item_Name,a.ERP_code,max(a.Cpartno)as Part_no,max(a.Irate) As Irate,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as PO_No,a.Fstr as SO_link,max(a.cdisc) as CDisc,max(a.iexc_Addl) as iexc_Addl,max(a.sd) as frt_pu,max(a.ipack) as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty,trim(A.cdrgno) As line_no,B.PACKSIZE AS STD_PACK from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,pordno,(Case when length(trim(nvl(desc9,'-')))>1 then desc9 else ciname end) as Iname,trim(Icode) as ERP_code,Cpartno,Irate,Qtyord,0 as Soldqty,nvl(Cdisc,0) as Cdisc,nvl(iexc_addl,0) as Iexc_Addl,nvl(sd,0) as sd ,nvl(ipack,0) as ipack,cdrgno from somas where branchcd!='DD' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "' and trim(icat)!='Y' and trim(nvl(app_by,'-'))!='-' and trim(ordno)='" + dt.Rows[i]["ordno"].ToString().Trim() + "' and to_char(orddt,'dd/mm/yyyy')='" + dt.Rows[i]["ordtd"].ToString().Trim() + "' and trim(icodE)='" + dt.Rows[i]["icode"].ToString().Trim() + "' union all SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,null as pordno,null as Iname,trim(Icode) as ERP_code,null as Cpartno,0 as Irate,0 as Qtyord,qtysupp as Soldqty,0 as Cdisc,0 as iexc_Addl,0 as sd,0 as ipack,ordline  from despatch where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "' and trim(ordno)='" + dt.Rows[i]["ordno"].ToString().Trim() + "' and to_char(orddt,'dd/mm/yyyy')='" + dt.Rows[i]["ordtd"].ToString().Trim() + "' and trim(icodE)='" + dt.Rows[i]["icode"].ToString().Trim() + "')a,item b where trim(a.erp_code)=trim(B.icode)  group by a.fstr,trim(A.cdrgno),a.ERP_code,b.unit,b.hscode,B.PACKSIZE having (case when sum(a.Qtyord)>0 then sum(a.Qtyord)-sum(a.Soldqty) else max(a.irate) end)>0 ) ", "balance_qty");
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";

                            sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["ciname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["ICpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["pordno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["IUnit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["grno"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["no_bdls"].ToString().Trim();

                            sg1_dr["sg1_t3"] = dt.Rows[i]["qtysupp"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["irate"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["cdisc"].ToString().Trim();

                            sg1_dr["sg1_t7"] = dt.Rows[i]["gtax1"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["gtax2"].ToString().Trim();

                            sg1_dr["sg1_t9"] = dt.Rows[i]["naration"].ToString().Trim();
                            if (frm_cocd == "AERO")
                            {
                                sg1_dr["sg1_t10"] = dt.Rows[i]["SDBQTY"].ToString().Trim() + "-" + dt.Rows[i]["SDAVAILED"].ToString().Trim();
                            }
                            else
                                sg1_dr["sg1_t10"] = dt.Rows[i]["SDBQTY"].ToString().Trim();
                            //sg1_dr["sg1_t11"] = dt.Rows[i]["iexc_Addl"].ToString().Trim();

                            //sg1_dr["sg1_t12"] = dt.Rows[i]["idiamtr"].ToString().Trim();
                            //sg1_dr["sg1_t13"] = dt.Rows[i]["ipack"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["ordno"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["ORDLINE"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["ordtd"].ToString().Trim();

                            sg1_dr["sg1_t20"] = dt.Rows[i]["opr_name"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
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
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;
                case "Atch_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    fgen.open_fileUploadPopup("Upload File for " + lblheader.Text, frm_qstr);
                    break;

                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;
                case "Print_E":
                    if (col1.Length < 2) return;
                    if (frm_cocd == "AERO")
                    {
                        hffield.Value = "PRINT_ASK";
                        fgen.msg("-", "CMSG", "Do You want to print Paper bag Packing list'13'No fo flexible style");
                    }
                    else
                    {
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
                        fgen.fin_sales_reps(frm_qstr);
                    }
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    string chk_party_bl = "";
                    chk_party_bl = "SELECT ctrlno||'-'||ctrldt||'-'||app_By as fstr FROM wb_tran_Ctrl WHERE branchcd!='DD' and type='BL' and TRIM(ACODE)='" + col1.Trim().ToUpper() + "' and trim(nvl(app_by,'-'))!='-' and trim(nvl(close_by,'-'))='-' and substr(app_by,1,3)!='[R]'";
                    chk_party_bl = fgen.seek_iname(frm_qstr, frm_cocd, chk_party_bl, "fstr");

                    if (chk_party_bl.Length > 10)
                    {
                        fgen.msg("-", "AMSG", "Transaction With " + col2 + " are Blocked '13' See Doc.No. " + chk_party_bl + " !!, Transaction Not allowed");
                        txtlbl4.Text = "-";
                        txtlbl4a.Text = "-";
                        return;
                    }

                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;

                    if (frm_cocd == "HEXP")
                    {
                        txtlbl5.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                        txtlbl6.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    }

                    txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATENM");
                    txtlbl73.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");

                    txtlbl70.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT type1 FROM TYPE WHERE ID='{' AND upper(Trim(Name))=upper(Trim('" + txtlbl73.Text + "'))", "type1");
                    txtlbl71.Text = txtlbl73.Text;
                    btnlbl7.Focus();

                    if (frm_vty == "45")
                    {
                        col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT CESSRATE FROM FAMST WHERE ACODE='" + col1 + "'", "CESSRATE");
                        if (col3 != "0") txtTCS.Text = col3;
                        else txtTCS.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT nvl(params,0) as params from controls where id='D38'", "params");
                    }
                    else txtTCS.Text = "0";

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

                    setColHeadings();

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
                case "BATCH":
                    //SQuery = "select a.btchno as fstr,b.iname as product,a.btchno as batch_no,sum(a.iqtyin-a.iqtyout) as bal,a.icode as erpcode from (select trim(icodE) as icode,trim(btchno) as btchno,iqtyin,0 as iqtyout from ivoucher where branchcd='" + frm_mbr + "' and type='3A' and stage='69' and vchdate " + DateRange + " and trim(icode) in (" + hf2.Value + ") union all select trim(icode) as icode,trim(no_bdls) as batchno,0 as iqtyin,qtysupp from despatch where branchcd='" + frm_mbr + "' and type like '4%' and orddt " + DateRange + " and trim(icode) in (" + hf2.Value + ")) a,item b where trim(a.icode)=trim(B.icode) group by a.btchno,a.icode,b.iname order by a.btchno";
                    if (col1 == "") return;
                    hf1.Value = (sg1.Rows.Count - 2).ToString();
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
                            if (hf1.Value.toInt() == i)
                            {
                                int d = hf1.Value.toInt();
                                z = 1;
                                int indx = 0;
                                foreach (string batchNo in col1.Split(','))
                                {
                                    sg1_dr = sg1_dt.NewRow();
                                    sg1_dr["sg1_srno"] = i + z;
                                    sg1_dr["sg1_h1"] = dt.Rows[d]["sg1_h1"].ToString();
                                    sg1_dr["sg1_h2"] = dt.Rows[d]["sg1_h2"].ToString();
                                    sg1_dr["sg1_h3"] = dt.Rows[d]["sg1_h3"].ToString();
                                    sg1_dr["sg1_h4"] = dt.Rows[d]["sg1_h4"].ToString();
                                    sg1_dr["sg1_h5"] = dt.Rows[d]["sg1_h5"].ToString();
                                    sg1_dr["sg1_h6"] = dt.Rows[d]["sg1_h6"].ToString();
                                    sg1_dr["sg1_h7"] = dt.Rows[d]["sg1_h7"].ToString();
                                    sg1_dr["sg1_h8"] = dt.Rows[d]["sg1_h8"].ToString();
                                    sg1_dr["sg1_h9"] = dt.Rows[d]["sg1_h9"].ToString();
                                    sg1_dr["sg1_h10"] = dt.Rows[d]["sg1_h10"].ToString();

                                    sg1_dr["sg1_f1"] = dt.Rows[d]["sg1_f1"].ToString();
                                    sg1_dr["sg1_f2"] = dt.Rows[d]["sg1_f2"].ToString();
                                    sg1_dr["sg1_f3"] = dt.Rows[d]["sg1_f3"].ToString();
                                    sg1_dr["sg1_f4"] = dt.Rows[d]["sg1_f4"].ToString();
                                    sg1_dr["sg1_f5"] = dt.Rows[d]["sg1_f5"].ToString();
                                    sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t1")).Text.Trim();
                                    if (frm_cocd != "AERO")
                                        sg1_dr["sg1_t2"] = batchNo.Split('~')[0].Replace("'", "");
                                    sg1_dr["sg1_t3"] = batchNo.Split('~')[1].Replace("'", "");
                                    sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t4")).Text.Trim();
                                    sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t5")).Text.Trim();
                                    sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t6")).Text.Trim();
                                    sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t7")).Text.Trim();
                                    sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t8")).Text.Trim();
                                    sg1_dr["sg1_t9"] = batchNo.Split('~')[0].Replace("'", "");
                                    sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t10")).Text.Trim();
                                    sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t11")).Text.Trim();
                                    sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t12")).Text.Trim();
                                    sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t13")).Text.Trim();
                                    sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t14")).Text.Trim();
                                    sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t15")).Text.Trim();
                                    sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t16")).Text.Trim();
                                    sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t17")).Text.Trim();
                                    sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[d].FindControl("sg1_t18")).Text.Trim();
                                    sg1_dr["sg1_t19"] = fgen.seek_iname(frm_qstr, frm_cocd, "sELECT RLPRC FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='17' AND TRIM(BTCHNO)='" + batchNo.Split('~')[0].Replace("'", "") + "'", "RLPRC");
                                    sg1_dr["sg1_t20"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim();
                                    //sg1_dr["sg1_h9"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").Replace("'", "").Split(',')[indx];

                                    sg1_dt.Rows.Add(sg1_dr);
                                    z++;
                                    indx++;
                                }
                            }
                            else
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

                                sg1_dt.Rows.Add(sg1_dr);
                            }
                        }
                    }

                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();

                    setColHeadings();
                    setGST();
                    break;
                case "ERPCODEX":
                    if (col1 == "") return;
                    lblSODetail.Text = col3 + ":" + col2 + "\n Balance Qty : " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                    txtlbl5.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7");
                    string mpo_Dtx = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8").Left(8).Substring(6, 2) + "/" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8").Left(8).Substring(4, 2) + "/" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8").Left(8).Trim().Substring(0, 4);
                    txtlbl6.Text = mpo_Dtx;
                    hfOrderQty.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                    hfIrate.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9");
                    hfsonum.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL10");

                    txtscanbarcode.Focus();
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();

                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");
                        if (!col1.Contains("'")) col1 = "'" + col1 + "'";

                        if (doc_GST.Value == "N")
                        {
                            if (col1.Trim().Length == 8) SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,0 as num4,0 as num5,0 as num6,0 as num7,a.line_no,A.std_pack,a.currency from (" + pop_qry + ") a where trim(a.fstr) in ('" + col1 + "')";
                            else SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,0 as num4,0 as num5,0 as num6,0 as num7,a.line_no,A.std_pack,a.currency from (" + pop_qry + ") a trim(a.fstr) in (" + col1 + ")";
                        }
                        else
                        {
                            if (col1.Trim().Length == 8) SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.line_no,A.std_pack,a.currency from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                            else SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.line_no,A.std_pack,a.currency from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in (" + col1 + ")";

                        }
                        if (lbl1a.Text == "47")
                        {
                            if (col1.Trim().Length == 8) SQuery = "select a.Inv_No as po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.line_no,A.std_pack,a.currency from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                            else SQuery = "select a.Inv_No as po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.line_no,A.std_pack,a.currency from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in (" + col1 + ")";

                            //SQuery = "SELECT distinct a.ACODE AS FSTR,a.ANAME AS PARTY,a.ACODE AS CODE,a.ADDR1,a.ADDR2,a.staten as state,a.Pay_num FROM FAMST a where SUBSTR(a.ACODE,1,2)='06' and length(Trim(nvl(a.deac_by,'-')))<=1 ORDER BY a.aname ";
                        }

                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        col3 = "";
                        string accBom = "N";
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            if (d == 0)
                            {
                                txtlbl18.Text = dt.Rows[0]["currency"].ToString().Trim();
                            }
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            if (!col3.Contains(dt.Rows[d]["icode"].ToString().Trim()))
                                col3 += ",'" + dt.Rows[d]["icode"].ToString().Trim() + "'";
                            sg1_dr["sg1_h1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_h3"] = dt.Rows[d]["std_pack"].ToString().Trim();
                            sg1_dr["sg1_h4"] = dt.Rows[d]["Balance_Qty"].ToString().Trim();
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
                            sg1_dr["sg1_t3"] = dt.Rows[d]["Balance_Qty"].ToString().Trim();
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
                            if (frm_vty == "4F")
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
                            mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(9, 6);
                            sg1_dr["sg1_t14"] = mpo_Dt;
                            sg1_dr["sg1_t15"] = dt.Rows[d]["line_no"].ToString().Trim();
                            mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(6, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(4, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 4);
                            sg1_dr["sg1_t16"] = fgen.make_def_Date(mpo_Dt, vardate);

                            if (txtlbl5.Text.Trim().Length <= 1)
                            {
                                txtlbl5.Text = dt.Rows[d]["po_no"].ToString().Trim();
                                txtlbl6.Text = mpo_Dt;
                            }

                            if (txtlbl7.Text.Trim().Length <= 1)
                            {
                                txtlbl7.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT CSCODE FROM SOMAS WHERE TYPE='" + frm_vty + "' AND ORDNO='" + txtlbl5.Text + "' AND TO_CHAR(ORDDT,'DD/MM/YYYY')='" + txtlbl6.Text + "'", "cscode");
                                txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname  from csmst where trim(upper(acode))=upper(Trim('" + txtlbl7.Text + "'))", "aname");
                            }

                            sg1_dr["sg1_t19"] = fgen.seek_iname(frm_qstr, frm_cocd, "sELECT MAX(RLPRC) AS RLPRC FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='17' AND TRIM(ICODE)='" + dt.Rows[d]["icode"].ToString().Trim() + "'", "RLPRC");

                            accBom = "N";
                            if (hfW120.Value == "Y")
                            {
                                col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(icode)||'~'||IBQTY AS VAL FROM ITEMOSP2 WHERE TRIM(ICODE)='" + dt.Rows[d]["icode"].ToString().Trim() + "' ", "VAL");
                                if (col3 != "0")
                                {
                                    sg1_dr["sg1_t3"] = "";
                                    sg1_dr["sg1_h5"] = "ACCS";
                                    sg1_dr["sg1_t22"] = col3;
                                    accBom = "Y";
                                }
                            }

                            sg1_dt.Rows.Add(sg1_dr);

                            if (accBom == "Y" && col3.Contains("~"))
                            {
                                DataTable dtaccs = new DataTable();
                                dtaccs = fgen.getdata(frm_qstr, frm_cocd, "SELECT TRIM(a.IBCODE) as ibcode,a.IBQTY AS VAL,b.iname,b.packsize,b.non_stk,b.cpartno,b.unit FROM ITEMOSP2 A,ITEM B WHERE TRIM(A.ibcode)=TRIM(b.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND TRIM(A.ICODE)='" + dt.Rows[d]["icode"].ToString().Trim() + "' ");
                                foreach (DataRow dracc in dtaccs.Rows)
                                {
                                    col2 = dracc["ibcode"].ToString().Trim();
                                    sg1_dr = sg1_dt.NewRow();
                                    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;

                                    sg1_dr["sg1_h1"] = dt.Rows[d]["icode"].ToString().Trim();
                                    sg1_dr["sg1_h2"] = dracc["iname"].ToString().Trim();
                                    sg1_dr["sg1_h3"] = dracc["packsize"].ToString().Trim();
                                    sg1_dr["sg1_h4"] = dracc["non_stk"].ToString().Trim();
                                    sg1_dr["sg1_h5"] = "ACCS-C";
                                    sg1_dr["sg1_h6"] = dracc["VAL"].ToString().Trim();
                                    sg1_dr["sg1_h7"] = "-";
                                    sg1_dr["sg1_h8"] = "-";
                                    sg1_dr["sg1_h9"] = "-";
                                    sg1_dr["sg1_h10"] = "-";

                                    sg1_dr["sg1_f1"] = col2;
                                    sg1_dr["sg1_f2"] = dracc["iname"].ToString().Trim();
                                    sg1_dr["sg1_f3"] = dracc["cpartno"].ToString().Trim();
                                    sg1_dr["sg1_f4"] = "-";
                                    sg1_dr["sg1_f5"] = dracc["unit"].ToString().Trim();

                                    sg1_dr["sg1_t1"] = "";
                                    sg1_dr["sg1_t2"] = "";

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
                                    if (frm_vty == "4F")
                                    {
                                        sg1_dr["sg1_t7"] = "0";
                                        sg1_dr["sg1_t8"] = "0";
                                    }

                                    sg1_dr["sg1_t9"] = "";
                                    sg1_dr["sg1_t10"] = "-";

                                    mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(9, 6);
                                    sg1_dr["sg1_t14"] = mpo_Dt;
                                    sg1_dr["sg1_t15"] = dt.Rows[d]["line_no"].ToString().Trim();
                                    mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(6, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(4, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 4);
                                    sg1_dr["sg1_t16"] = fgen.make_def_Date(mpo_Dt, vardate);

                                    if (txtlbl5.Text.Trim().Length <= 1)
                                    {
                                        txtlbl5.Text = dt.Rows[d]["po_no"].ToString().Trim();
                                        txtlbl6.Text = mpo_Dt;
                                    }

                                    if (txtlbl7.Text.Trim().Length <= 1)
                                    {
                                        txtlbl7.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT CSCODE FROM SOMAS WHERE TYPE='" + frm_vty + "' AND ORDNO='" + txtlbl5.Text + "' AND TO_CHAR(ORDDT,'DD/MM/YYYY')='" + txtlbl6.Text + "'", "cscode");
                                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname  from csmst where trim(upper(acode))=upper(Trim('" + txtlbl7.Text + "'))", "aname");
                                    }

                                    sg1_dr["sg1_t19"] = fgen.seek_iname(frm_qstr, frm_cocd, "sELECT MAX(RLPRC) AS RLPRC FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='17' AND TRIM(ICODE)='" + dt.Rows[d]["icode"].ToString().Trim() + "'", "RLPRC");

                                    sg1_dr["sg1_t22"] = dt.Rows[d]["icode"].ToString().Trim() + "~" + dracc["val"].ToString().Trim();
                                    sg1_dt.Rows.Add(sg1_dr);
                                }
                            }
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    //20/7/2020 :: comment due to RTE
                    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_BATCH_INV") == "Y" && lbl1a.Text != "47")
                    {
                        hf2.Value = col3.TrimStart(',');
                        hffield.Value = "BATCH";
                        make_qry_4_popup();
                        fgen.Fn_open_mseek("Select Batch No.", frm_qstr);
                    }

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
                    SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                    //else SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in (" + col1 + ")";

                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    for (int d = 0; d < dt.Rows.Count; d++)
                    {



                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = dt.Rows[d]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = dt.Rows[d]["iname"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[2].Text = "-";
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = dt.Rows[d]["Balance_Qty"].ToString().Trim();
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
                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t3")).Text = dt.Rows[d]["Balance_Qty"].ToString().Trim();
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

                        string mpo_Dt;
                        mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(9, 6);
                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t14")).Text = mpo_Dt;

                        mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(6, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(4, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 4);

                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t15")).Text = dt.Rows[d]["ORDLINE"].ToString().Trim();

                        ((TextBox)(sg1.Rows[Convert.ToInt32(hf1.Value)]).FindControl("sg1_t16")).Text = fgen.make_def_Date(mpo_Dt, vardate);



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
                            sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();
                            sg1_dr["sg1_t18"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim();
                            sg1_dr["sg1_t19"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim();
                            sg1_dr["sg1_t20"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim();

                            sg1_dr["sg1_t21"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim();
                            sg1_dr["sg1_t22"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim();
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
                    if (sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text.Left(2) == "KG")
                    {
                        if (fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").toDouble() > ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text.toDouble())
                        {
                            fgen.msg("-", "AMSG", "Tare Weight Cannot be more then Product Gross Weight'13'Product Gross Weight : " + ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text.toDouble() + "'13'Tare Weigth : " + fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").toDouble() + " ");
                            return;
                        }
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Text = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text + "-" + fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2");
                        if (Convert.ToInt32(hf1.Value) == 0)
                        {
                            for (int xx = 0; xx < sg1.Rows.Count - 1; xx++)
                            {
                                if (fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").toDouble() > ((TextBox)sg1.Rows[xx].FindControl("sg1_t3")).Text.toDouble())
                                {
                                    fgen.msg("-", "AMSG", "Tare Weight Cannot be more then Product Gross Weight'13'Product Gross Weight : " + ((TextBox)sg1.Rows[xx].FindControl("sg1_t3")).Text.toDouble() + "'13'Tare Weigth : " + fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").toDouble() + " ");
                                    return;
                                }
                                ((TextBox)sg1.Rows[xx].FindControl("sg1_t10")).Text = ((TextBox)sg1.Rows[xx].FindControl("sg1_t3")).Text + "-" + fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2");
                            }
                        }
                    }
                    else
                    {
                        if (fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").toDouble() > sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[8].Text.toDouble())
                        {
                            fgen.msg("-", "AMSG", "Tare Weight Cannot be more then Product Gross Weight'13'Product Gross Weight : " + sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[8].Text.toDouble() + "'13'Tare Weigth : " + fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").toDouble() + " ");
                            return;
                        }
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Text = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[8].Text + "-" + fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2");
                        if (Convert.ToInt32(hf1.Value) == 0)
                        {
                            for (int xx = 0; xx < sg1.Rows.Count - 1; xx++)
                            {
                                if (fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").toDouble() > sg1.Rows[xx].Cells[8].Text.toDouble())
                                {
                                    fgen.msg("-", "AMSG", "Tare Weight Cannot be more then Product Gross Weight'13'Product Gross Weight : " + sg1.Rows[xx].Cells[8].Text.toDouble() + "'13'Tare Weigth : " + fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").toDouble() + " ");
                                    return;
                                }
                                ((TextBox)sg1.Rows[xx].FindControl("sg1_t10")).Text = sg1.Rows[xx].Cells[8].Text + "-" + fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2");
                            }
                        }
                    }
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Focus();
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_vty = lbl1a.Text.Trim();
        frm_tab_ivch = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAB_IVCH");

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
            SQuery = "Select a.packno as DA_No,to_char(a.packdate,'dd/mm/yyyy') as Dated,c.Aname as Customer,a.ciname as Item_Name,a.cpartno as Part_No,a.qtysupp as DA_Qty,a.Irate as rate,a.no_bdls as batch_no,b.unit,b.hscode,'-' as Desc_,a.icode,a.ent_by,a.ent_Dt from " + frm_tab_ivch + " a, item b,famst c where a.branchcd='" + frm_mbr + "'  and a.type='" + frm_vty + "' and a." + doc_df.Value + " " + PrdRange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a." + doc_df.Value + ",a." + doc_nf.Value + ",a.srno ";
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
                        //oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tab_sale);

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
                        //save_fun2();
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
                        //oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "sale");

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


                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        //save_fun2();
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

                            fgen.execute_cmd(frm_qstr, frm_cocd, "update udf_Data set branchcd='DD' where par_tbl='" + frm_tab_ivch + "' and par_fld='" + ddl_fld1 + "'");

                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tab_ivch);
                        //fgen.save_data(frm_qstr, frm_cocd, oDS2, "sale");

                        //fgen.save_data(frm_qstr, frm_cocd, oDS3, "poterm");
                        //fgen.save_data(frm_qstr, frm_cocd, oDS4, "budgmst");
                        fgen.save_data(frm_qstr, frm_cocd, oDS5, "udf_Data");

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully'13'Do you want to see the Print Preview ?");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tab_ivch + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");

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

                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtvchdate.Text.Trim() + "'");
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                        lblSODetail.Text = "";
                        hfsonum.Value = "";
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
        sg1_dr["sg1_t17"] = "-";
        sg1_dr["sg1_t18"] = "-";

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
                if (frm_cocd == "RWPL")
                    return;

                if (frm_cocd == "ROYL")
                {
                    if (sg1.Rows.Count >= 10)
                    {
                        fgen.msg("-", "AMSG", "More then 9 items are not allowed in DA!!");
                        return;
                    }
                }

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
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_BATCH_INV") == "Y")
                        fgen.Fn_open_sseek("Select Item", frm_qstr);
                    else fgen.Fn_open_mseek("Select Item", frm_qstr);
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

        if (txtvchnum.Text == "-")
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
        fgen.Fn_open_sseek("Select " + lbl12.Text.Trim() + " ", frm_qstr);
    }
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_13";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl13.Text.Trim() + " ", frm_qstr);
    }
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_14";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl14.Text.Trim() + " ", frm_qstr);
    }
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_15";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl15.Text.Trim() + " ", frm_qstr);
    }
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_16";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl15.Text.Trim() + " ", frm_qstr);
    }
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_17";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl17.Text.Trim() + " ", frm_qstr);
    }
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_18";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl18.Text.Trim() + " ", frm_qstr);
    }
    protected void btnlbl19_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_19";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("", frm_qstr);
    }



    //------------------------------------------------------------------------------------
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl7.Text + "", frm_qstr);
    }
    protected void btnlbl70_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODEX";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl70.Text + " ", frm_qstr);
    }
    protected void btnAtch_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Atch_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr);
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
                oporow["packno"] = frm_vnum.Trim();
                oporow["packdate"] = txtvchdate.Text.Trim();

                oporow["cscode"] = (txtlbl7.Text == "") ? "-" : txtlbl7.Text;
                oporow["desp_to"] = txtlbl8.Text.Trim();
                oporow["PVT_MARK"] = txtlbl9.Text;

                oporow["org_invno"] = "-";
                oporow["org_invdt"] = txtvchdate.Text.Trim();
                oporow["refdate"] = txtvchdate.Text.Trim();

                oporow["CU_CHLDT"] = txtvchdate.Text.Trim();
                oporow["exc_57f4"] = "-";
                oporow["class"] = 0;


                oporow["acode"] = txtlbl4.Text.Trim();
                oporow["srno"] = i + 1;

                oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim();
                oporow["ciname"] = sg1.Rows[i].Cells[14].Text.Trim();
                oporow["cpartno"] = sg1.Rows[i].Cells[15].Text.Trim();
                oporow["pordno"] = sg1.Rows[i].Cells[16].Text.Trim();


                oporow["no_bdls"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                oporow["grno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                //oporow["btchno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                oporow["qtysupp"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim());
                oporow["qtyord"] = 0;
                oporow["cu_chlno"] = (((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim());
                oporow["naration"] = (((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim());

                oporow["irate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim());
                oporow["RLPRC"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim());
                oporow["cdisc"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim());
                oporow["amt_sale"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim());

                oporow["gtax1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim());
                oporow["gtax2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim());


                oporow["ordno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();

                oporow["ORDLINE"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();

                string po_dts;
                po_dts = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim(), vardate);

                oporow["orddt"] = po_dts;
                oporow["porddt"] = po_dts;

                oporow["billcode"] = "-";
                oporow["st_type"] = "-";
                oporow["remark"] = txtrmk.Text.Trim();

                oporow["ipack"] = 0;
                oporow["cdisc"] = 0;
                oporow["amdt2"] = sg1.Rows[i].Cells[4].Text.Trim(); //LOCCODE.text 
                oporow["amdt3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text.Trim(); //LOCCODE.text 
                oporow["Delivery"] = 0;
                oporow["QD"] = 0;
                oporow["sd"] = 0;

                //FOR SAVING NET WT AND GROSS WT===yogita
                //if (frm_cocd == "SGRP" && (frm_formID == "F55111" || frm_formID == "F50111"))
                //{
                oporow["QTY_BOX"] = fgen.make_double(txtnetwt.Text.Trim());
                oporow["NOF_BOX"] = fgen.make_double(txtgroswt.Text.Trim());
                //}
                //else
                //{
                // oporow["QTY_BOX"] = 0;
                //  oporow["NOF_BOX"] = 0;
                //}

                oporow["icat"] = "-";
                oporow["AvgpcWt"] = 0;

                oporow["mode_tpt"] = txtlbl15.Text.Trim();
                oporow["thru"] = txtlbl16.Text.Trim();
                oporow["freight"] = txtlbl17.Text.Trim();
                oporow["Currency"] = txtlbl18.Text.Trim();

                oporow["mo_vehi"] = txtlbl24.Text;
                oporow["weight"] = txtlbl26.Text;
                oporow["QTY_PKG"] = txtlbl28.Text.toDouble();//by yogita..tota packet was not saving 
                oporow["amdt1"] = txtlbl30.Text.Trim(); // sh_ref

                oporow["qtybal"] = 0;
                oporow["Post"] = "-";

                oporow["amt_Sale"] = 0;
                oporow["AMT_FRT"] = 0;
                oporow["AMT_SD"] = 0;
                oporow["amt_Exc"] = 0;

                //rstmp!Currency = tcurr.text

                oporow["frght"] = 0;
                oporow["PACK"] = 0;
                oporow["fdue"] = "-";
                oporow["ms_Cont"] = "-";

                if (((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().Contains("-"))
                {
                    oporow["SDBQTY"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().Split('-')[0].toDouble();
                    oporow["SDAVAILED"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().Split('-')[1].toDouble();
                }
                else
                {
                    oporow["SDBQTY"] = 0;
                    oporow["SDAVAILED"] = 0;
                }
                oporow["QDAVAILED"] = 0;

                oporow["OPR_NAME"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim();

                oporow["foc"] = 0;
                oporow["zone"] = "-";
                oporow["invno"] = "-";
                //oporow["mscont"] = "-";                


                oporow["invdate"] = txtvchdate.Text.Trim();
                oporow["grdate"] = txtvchdate.Text.Trim();
                ///for prep date and time
                oporow["REMVDATE"] = txtlbl3.Text.Trim();
                oporow["REMVTIME"] = fgen.make_def_Date(txtlbl2.Text.Trim(), vardate);

                //oporow["REMVDATE"] = txtvchdate.Text.Trim();
                //oporow["REMVTIME"] = "-";

                oporow["INVTIME"] = "N"; // dnote close option

                if (edmode.Value == "Y")
                {
                    oporow["eNt_by"] = ViewState["entby"].ToString();
                    //  oporow["eNt_dt"] = ViewState["entdt"].ToString();
                    //oporow["edt_by"] = frm_uname;
                    //oporow["edt_dt"] = vardate;

                }
                else
                {


                    oporow["eNt_by"] = frm_uname;
                    // oporow["eNt_dt"] = vardate;
                    //oporow["edt_by"] = "-";
                    //oporow["eDt_dt"] = vardate;

                }


                //If cd = "MVIN" Then
                //    rstmp!QDAVAILED = Val(sg.text(i, -13))
                //Else
                //    rstmp!QDAVAILED = 0
                //End If

                //    If cd = "PAIL" Then
                //    rstmp!SDAVAILED = Val(sg.text(i, 12))
                //Else
                //    rstmp!SDAVAILED = 0
                //End If
                //rstmp!SDBQTY = 0
                //If edmode = "Y" Then
                //    rstmp!SDBQTY = Val(amd_no.text) + 1
                //End If

                //If cd = "KLAS" Then
                //        popsql = "update somas set packinst='Packing List Made' where branchcd='" & mbr & "' and type='" & vty & "' and ordno='" & Trim(rstmp!ordno) & "' and orddt=to_Date('" & Format(rstmp!orddt, "dd/mm/yyyy") & "','dd/mm/yyyy') and trim(icode)='" & Trim(sg.text(i, 1)) & "' and trim(acode)='" & Trim(tacode.text) & "'"
                //        consql.Execute (popsql)
                //End If

                //If cd = "MMC" Then

                //    Dim zz As String
                //    zz = seek_iname3("select CU_CHLDT,prefix,desc9 from SOMAS where BRANCHCD='" & mbr & "' AND TYPE='" & vty & "' AND trim(Acode)='" & Trim(tacode.text) & "' and trim(icode)='" & Trim(sg.text(i, 1)) & "' AND TO_CHAR(ORDDT,'DD/MM/YYYY')='" & Trim(sg.text(i, -5)) & "' AND trim(ORDNO)='" & Trim(sg.text(i, -4)) & "'", "CU_CHLDT", "prefix", "desc9")
                //    If IsDate(seekn1) Then
                //        rstmp!refdate = Format(seekn1, "dd/mm/yyyy")
                //        rstmp!fdue = Trim(SeekN2)
                //        rstmp!naration = Trim(SeekN3)
                //    End If

                //    rstmp!ms_cont = Left(seek_iname("select trim(nvl(remark,'-')) as remark from appvendvch where BRANCHCD!='DD' AND trim(Acode)='" & Trim(tacode.text) & "' and trim(icode)='" & Trim(sg.text(i, 1)) & "'", "remark"), 100)
                //End If

                //If batch_w_da = "Y" Then
                //    rstmp!grno = IIf(Trim(sg.text(i, -9)) = "", "-", Trim(sg.text(i, -9)))
                //    rstmp!fdue = IIf(Trim(sg.text(i, -10)) = "", "-", Trim(sg.text(i, -10)))
                //    rstmp!INVNO = IIf(Trim(sg.text(i, -11)) = "", "-", Trim(sg.text(i, -11)))
                //End If

                oDS.Tables[0].Rows.Add(oporow);


            }
        }
    }
    void save_fun2()
    {

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
            if (((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim().Length > 1)
            {
                oporow5 = oDS5.Tables[0].NewRow();
                oporow5["branchcd"] = frm_mbr;
                oporow5["par_tbl"] = frm_tab_ivch.ToUpper().Trim();
                oporow5["par_fld"] = frm_mbr + lbl1a.Text + frm_vnum + txtvchdate.Text.Trim();
                oporow5["udf_name"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t1")).Text.Trim();
                oporow5["udf_value"] = ((TextBox)sg4.Rows[i].FindControl("sg4_t2")).Text.Trim();
                oporow5["SRNO"] = i;

                oDS5.Tables[0].Rows.Add(oporow5);
            }
        }
    }

    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        if (Prg_Id == "F55111")
        {
            SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE FROM type where id='V' and type1 like '4%' and type1 in ('4F','4E') order by type1";
        }
        else if (Prg_Id == "F56010") SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE FROM type where id='V' and type1 like '4%' and type1 in ('4A') order by type1";
        else
        {
            SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE FROM type where id='V' and type1 like '4%' and type1 not in ('4F','4E') order by type1";
            if (frm_cocd == "AERO")
                SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE FROM type where id='V' and type1 like '4%' order by type1";
        }

        btnval = hffield.Value;
        if (btnval != "New")
        {
            SQuery = "select Fstr,Document_Name,Document_Series,count(*) as Document_Count from (SELECT distinct a.type as Fstr,b.NAME as Document_Name,a.TYPE as Document_Series,a.packno FROM " + frm_tab_ivch + " a ,TYPE b WHERE a.branchcd='" + frm_mbr + "' and a.packdate " + DateRange + " and trim(A.type)=trim(B.type1) and b.ID='V' and a.type like '4%' /*and a.type not in ('4F','4E')*/) group by Fstr,Document_Name,Document_Series order by Document_Series ";
            if (Prg_Id == "F55111")
            {
                SQuery = "select Fstr,Document_Name,Document_Series,count(*) as Document_Count from (SELECT distinct a.type as Fstr,b.NAME as Document_Name,a.TYPE as Document_Series,a.packno FROM " + frm_tab_ivch + " a ,TYPE b WHERE a.branchcd='" + frm_mbr + "' and a.packdate " + DateRange + " and trim(A.type)=trim(B.type1) and b.ID='V' and a.type like '4%' and a.type in ('4F','4E')) group by Fstr,Document_Name,Document_Series order by Document_Series ";
            }

        }

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
    protected void Button1_Click(object sender, EventArgs e)
    { }
    protected void Button2_Click(object sender, EventArgs e)
    {
        //
    }
    protected void Button3_Click(object sender, EventArgs e)
    {

    }
    protected void Button4_Click(object sender, EventArgs e)
    {

    }
    protected void txtscanbarcode_TextChanged(object sender, EventArgs e)
    {
        #region for gridview 1
        //if (col1.Length <= 0) return;
        //col1 = "";
        //foreach (GridViewRow gr in sg1.Rows)
        //{
        //    col1 += "," + ((TextBox)gr.FindControl("sg1_t2")).Text.Trim();
        //}

        //if (col1.Contains(txtscanbarcode.Text.Trim()))
        //{
        //    fgen.msg("-", "AMSG", "Barcode : " + txtscanbarcode.Text + " is alread scanned!! ");

        //    return;
        //}

        if (ViewState["sg1"] != null)
        {
            dt = new DataTable();

            sg1_dt = new DataTable();
            dt = (DataTable)ViewState["sg1"];
            z = dt.Rows.Count;
            sg1_dt = dt.Clone();
            sg1_dr = null;
            for (i = 0; i < dt.Rows.Count; i++)
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

            string[] allBarCode = txtscanbarcode.Text.Split('\n');
            var distinctBarcode = allBarCode.Distinct();

            foreach (string singlBarcode in distinctBarcode)
            {
                dt = new DataTable();
                if (frm_cocd == "HEXP")
                {
                    cond = "";
                    SQuery = "select distinct b.TAGNO  from despatch a , packrec b where trim(a.no_bdls)=trim(b.tagno) and a.branchcd='" + frm_mbr + "' and a.type!='" + frm_vty + "' AND TRIM(A.PACKNO)||TO_CHAR(A.packdate,'DD/MM/YYYY')!='" + txtvchnum.Text + txtvchdate.Text + "' ";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    col1 = "";
                    if (dt2.Rows.Count > 0 || sg1_dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            if (col1.Length > 0) col1 = col1 + ",'" + dt2.Rows[i]["TAGNO"].ToString().Trim() + "'";
                            else col1 = "'" + dt2.Rows[i]["TAGNO"].ToString().Trim() + "'";
                        }
                        for (int i = 0; i < sg1_dt.Rows.Count; i++)
                        {
                            if (col1.Length > 0) col1 = col1 + ",'" + sg1_dt.Rows[i]["sg1_t2"].ToString().Trim() + "'";
                            else col1 = "'" + sg1_dt.Rows[i]["sg1_t2"].ToString().Trim() + "'";
                        }
                        cond = " and TRIM(a.TAGNO) not in (" + col1 + ")";
                    }
                    else
                    {
                        cond = "";
                    }

                    string mq = "select distinct trim(erp_code) as erp_code from (select a.Fstr,max(a.Iname)as Item_Name,a.ERP_code,max(a.Cpartno)as Part_no,max(a.Irate) As Irate,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as PO_No,a.Fstr as SO_link,max(a.cdisc) as CDisc,max(a.iexc_Addl) as iexc_Addl,max(a.sd) as frt_pu,max(a.ipack) as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty,trim(A.cdrgno) As line_no,B.PACKSIZE AS STD_PACK,max(a.currency) as currency,trim(a.acode) as acode from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,pordno,(Case when length(trim(nvl(desc9,'-')))>1 then desc9 else ciname end) as Iname,trim(Icode) as ERP_code,Cpartno,Irate,Qtyord,0 as Soldqty,nvl(Cdisc,0) as Cdisc,nvl(iexc_addl,0) as Iexc_Addl,nvl(sd,0) as sd ,nvl(ipack,0) as ipack,cdrgno,currency,trim(acode) as acode from somas where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(icat)!='Y' and trim(nvl(app_by,'-'))!='-' and 1=1 and trim(acode)='" + txtlbl4.Text.Trim() + "' and ordno='" + txtlbl5.Text.Trim() + "' and to_char(orddt,'dd/mm/yyyy')='" + txtlbl6.Text.Trim() + "' union all SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,null as pordno,null as Iname,trim(Icode) as ERP_code,null as Cpartno,0 as Irate,0 as Qtyord,qtysupp as Soldqty,0 as Cdisc,0 as iexc_Addl,0 as sd,0 as ipack,ordline,null as currency,trim(acode) as acode  from despatch where branchcd='" + frm_mbr + "' and type='" + frm_vty + "')a,item b where trim(a.erp_code)=trim(B.icode)  group by a.fstr,trim(A.cdrgno),a.ERP_code,b.unit,b.hscode,B.PACKSIZE,trim(a.acode) having (case when sum(a.Qtyord)>0 then sum(a.Qtyord)-sum(a.Soldqty) else max(a.irate) end)>0 )";
                    SQuery = "SELECT TRIM(a.ICODE)||TRIM(a.VCHNUM)||trim(a.TAGNO) AS fstr,a.VCHNUM AS COL2,a.VCHDATE COL3,1 AS COL4,a.COL1 AS COL5,b.icode,'-' as currency,b.iname,b.packsize as std_pack,1 as balance_qty,b.cpartno,'-' as po_no,b.unit,b.irate,0 as cdisc,c.num4,c.num5,c.num6,c.num7,'-' as iexc_Addl,'' as frt_pu,'' as pkchg_pu,0 as line_no,a.TAGNO as tag FROM PACKREC a,ITEM B,TYPEGRP C WHERE trim(b.hscode)=trim(c.acref) and c.id='T1' and TRIM(A.ICODE)=TRIM(b.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='PT' " + cond + " AND TRIM(a.TAGNO)='" + singlBarcode.Replace("\r", "").Trim() + "' and trim(a.icode) in (" + mq + ") AND (UPPER(TRIM(A.COL2))='OUTER' OR UPPER(TRIM(A.COL2))='MASTER CARTON') ";
                }
                else
                {
                    if (txtscanbarcode.Text.Contains("__"))
                    {
                        string newQRcode = txtscanbarcode.Text.Split('_')[0];
                        if (newQRcode.Contains("/"))
                            newQRcode = newQRcode.Split('/')[0];

                        string mqx = "select distinct trim(erp_code) as erp_code from (select a.Fstr,max(a.Iname)as Item_Name,a.ERP_code,max(a.Cpartno)as Part_no,max(a.Irate) As Irate,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as PO_No,a.Fstr as SO_link,max(a.cdisc) as CDisc,max(a.iexc_Addl) as iexc_Addl,max(a.sd) as frt_pu,max(a.ipack) as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty,trim(A.cdrgno) As line_no,B.PACKSIZE AS STD_PACK,max(a.currency) as currency,trim(a.acode) as acode from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,pordno,(Case when length(trim(nvl(desc9,'-')))>1 then desc9 else ciname end) as Iname,trim(Icode) as ERP_code,Cpartno,Irate,Qtyord,0 as Soldqty,nvl(Cdisc,0) as Cdisc,nvl(iexc_addl,0) as Iexc_Addl,nvl(sd,0) as sd ,nvl(ipack,0) as ipack,cdrgno,currency,trim(acode) as acode from somas where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(icat)!='Y' and trim(nvl(app_by,'-'))!='-' and 1=1 and trim(acode)='" + txtlbl4.Text.Trim() + "' and ordno='" + hfsonum.Value.Split('.')[0].Trim() + "' and to_char(orddt,'dd/mm/yyyy')='" + txtlbl6.Text.Trim() + "' union all SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,null as pordno,null as Iname,trim(Icode) as ERP_code,null as Cpartno,0 as Irate,0 as Qtyord,qtysupp as Soldqty,0 as Cdisc,0 as iexc_Addl,0 as sd,0 as ipack,ordline,null as currency,trim(acode) as acode  from despatch where branchcd='" + frm_mbr + "' and type='" + frm_vty + "')a,item b where trim(a.erp_code)=trim(B.icode) and trim(A.cdrgno)='" + hfsonum.Value + "' and a.erp_code='" + lblSODetail.Text.Trim().Left(8) + "' group by a.fstr,trim(A.cdrgno),a.ERP_code,b.unit,b.hscode,B.PACKSIZE,trim(a.acode) having (case when sum(a.Qtyord)>0 then sum(a.Qtyord)-sum(a.Soldqty) else max(a.irate) end)>0 )";
                        SQuery = "SELECT TRIM(a.ICODE) as icode,B.INAME,B.UNIT,b.PACKSIZE AS STD_PACK,'" + txtscanbarcode.Text.Split('_')[txtscanbarcode.Text.Split('_').Length - 1] + "' AS BALANCE_qTY,B.CPARTNO,A.INVNO AS TAG,'" + hfIrate.Value + "' as irate,0 as cdisc,'" + txtlbl5.Text.Trim() + "' as po_no,C.NUM4,C.NUM5,C.NUM6,0 AS iexc_Addl,0 AS frt_pu,0 AS pkchg_pu FROM IVOUCHER A,ITEM B,TYPEGRP C WHERE TRIM(A.ICODE)=TRIM(b.ICODE) AND TRIM(B.HSCODE)=TRIM(c.ACREf) AND C.ID='T1' AND A.BRANCHCD||A.TYPE||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'YYYYMMDD')||TRIM(A.ICODE)||TRIM(A.INVNO)='" + newQRcode + "' and trim(a.icode) in (" + mqx + ") ";
                    }
                }
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                col3 = "";
                if (dt == null) return;
                for (int d = 0; d < dt.Rows.Count; d++)
                {
                    sg1_dr = sg1_dt.NewRow();
                    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                    sg1_dr["sg1_h1"] = dt.Rows[d]["icode"].ToString().Trim();
                    sg1_dr["sg1_h2"] = dt.Rows[d]["iname"].ToString().Trim();
                    sg1_dr["sg1_h3"] = dt.Rows[d]["std_pack"].ToString().Trim();
                    sg1_dr["sg1_h4"] = dt.Rows[d]["Balance_Qty"].ToString().Trim();
                    sg1_dr["sg1_h5"] = "-";
                    sg1_dr["sg1_h6"] = "-";
                    sg1_dr["sg1_h7"] = "-";
                    sg1_dr["sg1_h8"] = "-";
                    sg1_dr["sg1_h9"] = "-";
                    sg1_dr["sg1_h10"] = "-";

                    sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                    sg1_dr["sg1_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                    sg1_dr["sg1_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();
                    sg1_dr["sg1_f4"] = dt.Rows[d]["po_no"].ToString().Trim();//-
                    sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();

                    sg1_dr["sg1_t1"] = "";
                    sg1_dr["sg1_t2"] = dt.Rows[d]["tag"].ToString().Trim();
                    sg1_dr["sg1_t3"] = dt.Rows[d]["Balance_Qty"].ToString().Trim();
                    sg1_dr["sg1_t4"] = dt.Rows[d]["Irate"].ToString().Trim();
                    sg1_dr["sg1_t5"] = dt.Rows[d]["cDisc"].ToString().Trim();//0

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
                    if (frm_vty == "4F")
                    {
                        sg1_dr["sg1_t7"] = "0";
                        sg1_dr["sg1_t8"] = "0";
                    }

                    sg1_dr["sg1_t9"] = "";
                    sg1_dr["sg1_t10"] = "-";
                    sg1_dr["sg1_t11"] = dt.Rows[d]["iexc_Addl"].ToString().Trim();
                    sg1_dr["sg1_t12"] = dt.Rows[d]["frt_pu"].ToString().Trim();
                    sg1_dr["sg1_t13"] = dt.Rows[d]["pkchg_pu"].ToString().Trim();

                    if (hfsonum.Value.Contains("."))
                    {
                        sg1_dr["sg1_t14"] = hfsonum.Value.Split('.')[0];
                        sg1_dr["sg1_t15"] = hfsonum.Value;
                        sg1_dr["sg1_t16"] = txtlbl6.Text;
                    }
                    else
                    {
                        sg1_dr["sg1_t14"] = txtlbl5.Text;
                        sg1_dr["sg1_t15"] = "1";
                        sg1_dr["sg1_t16"] = txtlbl6.Text;
                    }

                    sg1_dr["sg1_t16"] = txtlbl6.Text;
                    //sg1_dr["sg1_t19"] = dt.Rows[d]["RLPRC"].ToString().Trim();

                    sg1_dt.Rows.Add(sg1_dr);
                    if (d == 0)
                    {
                        try
                        {
                            txtlbl18.Text = dt.Rows[0]["currency"].ToString().Trim();
                        }
                        catch
                        {
                            txtlbl18.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT CURRENCY FROM SOMAS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND TRIM(ORDNO)='" + sg1_dr["sg1_t14"].ToString().Trim() + "' AND TO_CHAR(ORDDT,'DD/MM/YYYY')='" + sg1_dr["sg1_t16"].ToString().Trim() + "' ", "CURRENCY");
                        }
                    }
                }
            }
        }

        DataView dv = sg1_dt.DefaultView;
        dv.Sort = "sg1_srno desc";
        DataTable sortedDT = dv.ToTable();
        //sg1_add_blankrows();

        ViewState["sg1"] = sortedDT;
        sg1.DataSource = sortedDT;
        sg1.DataBind();


        if (sg1.Rows.Count > 1)
            ((TextBox)sg1.Rows[sg1.Rows.Count - 1].FindControl("sg1_t1")).Focus();

        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_BATCH_INV") == "Y" && lbl1a.Text != "47")
        {
            hf2.Value = col3.TrimStart(',');
            hffield.Value = "BATCH";
            make_qry_4_popup();
            fgen.Fn_open_mseek("Select Batch No.", frm_qstr);
        }

        #endregion
        setColHeadings();
        setGST();
        txtscanbarcode.Text = "";
        txtscanbarcode.Focus();
    }
    protected void btnIcode_Click(object sender, EventArgs e)
    {
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
        //pop1

        string ord_br_Str = "branchcd='" + frm_mbr + "'";
        if (doc_hosopw.Value == "Y")
        {
            ord_br_Str = "branchcd='00'";
        }
        // vipin chk_below_command
        ord_br_Str = "branchcd='" + frm_mbr + "'";
        string more_Cond = "";
        more_Cond = "1=1";
        if (frm_cocd == "SAGM")
        {
            more_Cond = "trim(weight)='" + frm_mbr + "'";
        }

        if (doc_hosopw.Value == "Y")
        {
            ord_br_Str = "branchcd='00' and trim(nvl(mfginbr,'-'))='" + frm_mbr + "'";
        }

        SQuery = "select a.Fstr,max(a.Iname)as Item_Name,a.ERP_code,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as PO_No,a.Fstr as SO_link,max(a.Irate) As Irate,trim(A.cdrgno) As line_no,max(a.cdisc) as CDisc,max(a.iexc_Addl) as iexc_Addl,max(a.sd) as frt_pu,max(a.ipack) as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty,B.PACKSIZE AS STD_PACK,max(a.currency) as currency,max(a.Cpartno)as Part_no from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,pordno,(Case when length(trim(nvl(desc9,'-')))>1 then desc9 else ciname end) as Iname,trim(Icode) as ERP_code,Cpartno,Irate,Qtyord,0 as Soldqty,nvl(Cdisc,0) as Cdisc,nvl(iexc_addl,0) as Iexc_Addl,nvl(sd,0) as sd ,nvl(ipack,0) as ipack,cdrgno,currency from somas where " + ord_br_Str + " and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "' and trim(icat)!='Y' and trim(nvl(app_by,'-'))!='-' and " + more_Cond + " union all SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,null as pordno,null as Iname,trim(Icode) as ERP_code,null as Cpartno,0 as Irate,0 as Qtyord,qtysupp as Soldqty,0 as Cdisc,0 as iexc_Addl,0 as sd,0 as ipack,ordline,null as currency  from despatch where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "')a,item b where trim(a.erp_code)=trim(B.icode)  group by a.fstr,trim(A.cdrgno),a.ERP_code,b.unit,b.hscode,B.PACKSIZE having (case when sum(a.Qtyord)>0 then sum(a.Qtyord)-sum(a.Soldqty) else max(a.irate) end)>0 order by Item_Name,a.fstr";
        if (lbl1a.Text == "47")
        {
            SQuery = "select a.Fstr,b.Iname as Item_Name,a.ERP_code,b.Cpartno as Part_no,max(a.Irate) As Irate,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as Inv_No,a.Fstr as MRR_link,0 as CDisc,0 as iexc_Addl,0 as frt_pu,0 as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty,'-' As line_no,B.PACKSIZE AS STD_PACK,'-' as currency from (SELECT to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||trim(Icode) as fstr,invno as pordno,trim(Icode) as ERP_code,Irate,nvl(rej_rw,0) as Qtyord,0 as Soldqty,'-' as currency from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and trim(acode)='" + txtlbl4.Text + "' and trim(nvl(store,'-'))='Y' and rej_rw>0 union all SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,'-' as pordno,trim(Icode) as ERP_code,Irate,0 as Qtyord,qtysupp as Soldqty,null as currency from despatch where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "')a,item b where trim(a.erp_code)=trim(B.icode)  group by a.Fstr,b.Iname,a.ERP_code,b.Cpartno,b.Unit,b.hscode,a.Fstr,B.PACKSIZE order by b.iname,a.fstr";
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);

        hffield.Value = "ERPCODEX";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_sseek("Select Item", frm_qstr);
    }
    protected void btnGridPop_Click(object sender, EventArgs e)
    {
        if (hf1.Value.Contains("sg1_t9_"))
        {
            hffield.Value = "sg1_t9";
            hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_sg1_t9_", "");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            fgen.Fn_ValueBoxMultiple("Enter Gross Weight / Tare Weight", frm_qstr, "350px", "250px");
        }
    }
}