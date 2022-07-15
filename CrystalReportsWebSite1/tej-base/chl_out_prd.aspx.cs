using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class chl_out_prd : System.Web.UI.Page
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
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange;
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
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                //doc_addl.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select (case when nvl(st_Sc,1)=0 then 1 else nvl(st_Sc,1) end )  as add_tx from type where id='B' and trim(upper(type1))=upper(Trim('" + frm_mbr + "'))", "add_tx");
                doc_addl.Value = "-";
                string chk_opt;
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0001'", "fstr");
                if (chk_opt != "Y")
                {
                    tab2.Visible = false;
                    btnPost.Visible = false;
                }
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0002'", "fstr");
                if (chk_opt != "Y")
                {
                    txtBarCode.Visible = false;
                    btnRead.Visible = false;
                }


                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
            setGST();

            if (lbl1a.Text == "27")
            {
                txtlbl2.ReadOnly = false;
                txtlbl2.Attributes.Remove("readonly");
                txtlbl3.Attributes.Remove("readonly");
            }

            if (Prg_Id == "FB1057")
            {
                lbl4.Visible = false;
                btnlbl4.Visible = false;
                txtlbl4.Visible = false;
                txtlbl4a.Visible = false;
                btngetchl.Visible = false;
                //txtlbl2.ReadOnly = false;
                //txtlbl2.Attributes.Remove("readonly");
                //txtlbl3.Attributes.Remove("readonly");
            }
            else
            {
                btngetchl.Visible = true;
            }
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
                    //sg1.HeaderRow.Cells[i].Style["display"] = "none";
                    //sg1.Rows[K].Cells[i].Style["display"] = "none";
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[K].Cells[i].CssClass = "hidden";
                }
                #endregion
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

                ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");

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
                    sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
                    sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                }
            }
        }
        txtlbl25.Attributes.Add("readonly", "readonly");
        txtlbl27.Attributes.Add("readonly", "readonly");
        txtlbl29.Attributes.Add("readonly", "readonly");
        txtlbl31.Attributes.Add("readonly", "readonly");
        //to hide and show to tab panel
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F25106":
            case "FB1052":
            case "FB1057":
                tab2.Visible = false;
                tab3.Visible = false;
                tab4.Visible = false;
                tab5.Visible = false;
                break;
        }
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //void setColHeadings()
    //{
    //    dtCol = new DataTable();
    //    dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
    //    if (dtCol == null || dtCol.Rows.Count <= 0)
    //    {
    //        getColHeading();
    //    }
    //    dtCol = new DataTable();
    //    dtCol = (DataTable)ViewState["d" + frm_qstr + frm_formID];
    //    #region hide hidden columns
    //    sg1.Columns[0].Visible = false;
    //    sg1.Columns[1].Visible = false;
    //    sg1.Columns[2].Visible = false;
    //    sg1.Columns[3].Visible = false;
    //    sg1.Columns[4].Visible = false;
    //    sg1.Columns[5].Visible = false;
    //    sg1.Columns[6].Visible = false;
    //    sg1.Columns[7].Visible = false;
    //    sg1.Columns[8].Visible = false;
    //    sg1.Columns[9].Visible = false;
    //    #endregion
    //    if (dtCol == null) return;
    //    if (sg1.Rows.Count <= 0) return;
    //    for (int sR = 0; sR < sg1.Columns.Count; sR++)
    //    {
    //        string orig_name;
    //        double tb_Colm;
    //        tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
    //        orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();

    //        for (int K = 0; K < sg1.Rows.Count; K++)
    //        {
    //            if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

    //            ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");
    //            ((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");

    //            txtlbl70.Attributes.Add("readonly", "readonly");
    //            txtlbl71.Attributes.Add("readonly", "readonly");
    //            txtlbl72.Attributes.Add("readonly", "readonly");
    //            txtlbl73.Attributes.Add("readonly", "readonly");
    //            ((TextBox)sg1.Rows[K].FindControl("sg1_t7")).Attributes.Add("readonly", "readonly");
    //            ((TextBox)sg1.Rows[K].FindControl("sg1_t8")).Attributes.Add("readonly", "readonly");
    //        }
    //        orig_name = orig_name.ToUpper();
    //        //if (sg1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_NAME"))
    //        if (sR == tb_Colm)
    //        {
    //            // hidding column
    //            if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
    //            {
    //                sg1.Columns[sR].Visible = false;
    //            }
    //            // Setting Heading Name
    //            sg1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
    //            // Setting Col Width
    //            string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
    //            if (fgen.make_double(mcol_width) > 0)
    //            {
    //                sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
    //                sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
    //            }
    //        }
    //    }

    //    txtlbl25.Attributes.Add("readonly", "readonly");
    //    txtlbl27.Attributes.Add("readonly", "readonly");
    //    txtlbl29.Attributes.Add("readonly", "readonly");
    //    txtlbl31.Attributes.Add("readonly", "readonly");

    //    // to hide and show to tab panel



    //    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
    //    switch (Prg_Id)
    //    {
    //        case "F25106":
    //        case "FB1052":
    //        case "FB1057":
    //            tab2.Visible = false;
    //            tab3.Visible = false;
    //            tab4.Visible = false;
    //            tab5.Visible = false;
    //            break;
    //    }

    //    fgen.SetHeadingCtrl(this.Controls, dtCol);
    //}
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btngetchl.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
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
        btnnew.Disabled = true; btngetchl.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
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
        //Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        //switch (Prg_Id)
        //{

        //    case "F25106":
        frm_tabname = "ivoucher";
        //        break;
        //}
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
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
            case "Row_Add":
            case "Row_Edit":
                if (sg1.Rows.Count > 1)
                {
                    col1 = ""; col2 = "";
                    foreach (GridViewRow r1 in sg1.Rows)
                    {
                        if (frm_cocd == "MANU")
                        {
                            if (col2.Length > 0) col2 = col2 + "," + "'" + r1.Cells[9].Text.Trim() + "'";
                            else col2 = "'" + r1.Cells[9].Text.Trim() + "'";
                        }
                        else
                        {
                            if (col2.Length > 0) col2 = col2 + "," + "'" + r1.Cells[3].Text.Trim() + "'";
                            else col2 = "'" + r1.Cells[3].Text.Trim() + "'";
                        }
                    }
                    col2 = "(" + col2 + ")";
                }
                else col2 = " ('A')";
                //SQuery = "select trim(a.icode)||trim(a.btchno)||a.invno||to_Char(a.invdate,'dd/mm/yyyy') as fstr,c.iname as item_name,a.icode as item_code,c.cpartno,c.unit,a.btchno as Batch,a.binno as ABC,sum(a.iqtyin)-sum(a.iqtyout) as balance,a.invno as job_no,to_char(a.invdate,'dd/mm/yyyy') as job_dt,trim(a.icode)||trim(A.btchno) as bcode,revis_no as Tracking_No from (select trim(icode) as icode,btchno AS btchno,iqtyin,0 as iqtyout,binno,invno,invdate,revis_no from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and " +
                //    "stage='" + txtlbl70B.Text.Trim() + "'  and store='W' union all select trim(icode) as icode,btchno AS btchno,0 as iqtyin,iqtyout,binno,invno,invdate,revis_no from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and stage='" + txtlbl70D.Text.Trim() + "'  and store='W') a,item c where trim(a.icode)=trim(c.icode) group by c.iname,revis_no,a.icode,c.cpartno,c.unit,a.btchno,a.binno,trim(a.icode)||trim(A.btchno),a.invno,to_Char(a.invdate,'dd/mm/yyyy'),trim(a.icode)||trim(a.btchno)||a.invno||to_Char(a.invdate,'dd/mm/yyyy') having sum(a.iqtyin)-sum(a.iqtyout)>0 order by a.icode";
                break;

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
                SQuery = "select * from (select Acode,ANAME as Transporter,Acode as Code,Addr1 as Address,Addr2 as City from famst  where upper(ccode)='T' union all select 'Own' as Acode,'OWN' as Transporter,'-' as Code,'-' as Address,'-' as City from dual union all select 'PARTY VEHICLE' as Transporter,'-' as Code,'-' as Address,'-' as City from dual) order by  Transporter";
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
                if (lbl1a.Text == "29")
                {
                    SQuery = "SELECT distinct a.Acode as FStr,a.Aname as Party_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.Staten from famst a where a.branchcd!='DD' and length(trim(nvl(a.deac_by,'-'))) <2 and substr(a.acode,1,2) in ('02') and length(Trim(A.staten))>2 order by a.Aname ";
                }
                else if (lbl1a.Text == "27")
                    SQuery = "SELECT distinct a.Acode as FStr,a.Aname as Party_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.Staten from famst a where a.branchcd!='DD' and length(trim(nvl(a.deac_by,'-'))) <2 and length(Trim(A.staten))>2 order by a.Aname ";
                else
                {
                    SQuery = "SELECT distinct a.Acode as FStr,a.Aname as Party_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.Staten from famst a where a.branchcd!='DD' and length(trim(nvl(a.deac_by,'-'))) <2 and substr(a.acode,1,2) in ('02','05','06','14','15') and length(Trim(A.staten))>2 order by a.Aname ";
                }
                if (frm_cocd == "DREW" || frm_cocd == "PROD" || frm_cocd == "MASS" || frm_cocd == "MAST")
                    SQuery = "SELECT distinct a.Acode as FStr,a.Aname as Party_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.Staten from famst a where a.branchcd!='DD' and length(trim(nvl(a.deac_by,'-'))) <2 and length(Trim(A.staten))>2 order by a.Aname ";

                break;
            case "TICODE":
                //pop2
                //SQuery = "SELECT distinct a.Acode as FStr,a.Aname as Party_Name,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.Staten from famst a where a.branchcd!='DD' and length(trim(nvl(a.deac_by,'-'))) <2 and substr(a.acode,1,2) in ('02','05','06','14','15') order by a.Aname ";
                SQuery = "SELECT ACODE AS FSTR,ANAME AS PARTY,ACODE AS CODE,ADDR1,ADDR2 FROM CSMST where branchcd!='DD' and trim(tcsnum)='" + txtlbl4.Text + "' ORDER BY aname ";
                //SQuery = "SELECT ICODE AS FSTR,INAME AS PRODUCT,ICODE AS CODE,UNIT,CPARTNO AS PARTNO FROM ITEM WHERE LENGTH(tRIM(ICODE))>4 ";
                break;
            case "TICODEX":
                SQuery = "select type1,name as State ,type1 as code from type where id='{' order by Name";
                break;
            case "SFROM":
                SQuery = "Select type1 as fstr,name as stage,type1 as code,'WIP' as stg from type where id='1' and type1 not in ('6R','6F','6G','6H') order by type1";
                break;
            case "SFOR":
                if (sg1.Rows.Count > 0)
                {
                    try
                    {
                        col2 = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t19")).Text.Trim().Split('~')[0];
                    }
                    catch
                    {
                        col2 = "0";
                    }
                }
                else col2 = "0";
                SQuery = "Select type1 as fstr,name as stage,type1 as code,'WIP' as stg from type where id='1' and type1 not in ('6R','6F','6G','6H') and type1" +
                    " not in ('" + col2 + "','%') order by type1";
                break;
            case "STO":
                if (sg1.Rows.Count > 0)
                {
                    try
                    {
                        col2 = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t19")).Text.Trim().Split('~')[0];
                        col3 = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t20")).Text.Trim().Split('~')[0];
                    }
                    catch
                    {
                        col2 = col3 = "0";
                    }
                }
                else col2 = "0";
                SQuery = "Select type1 as fstr,name as stage,type1 as code,'WIP' as stg from type where id='1' and type1 not in ('6R','6G','6H') and type1 not in ('" + col2 + "','" + col3 + "','%') order by type1";
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                //pop3
                // to avoid repeat of item
                if (((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t19")).Text.Trim().Length < 1 || ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t20")).Text.Trim().Length < 1 || ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t21")).Text.Trim().Length < 1)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill All Stages Before Item Selection");
                    return;
                }
                col1 = "";
                if (btnval != "SG3_ROW_ADD" && btnval != "SG3_ROW_ADD_E")
                {
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[19].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[19].Text.Trim() + "'";
                    }
                }

                if (col1.Length <= 0) col1 = "'-'";
                if (frm_cocd == "HIME" && lbl1a.Text == "22") col1 = "'-'";
                string stagefrom = "", stagefor = "", stageto = "";
                try
                {
                    col2 = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t19")).Text.Trim().Split('~')[0];
                    stagefrom = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t19")).Text.Trim();
                    stagefor = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t20")).Text.Trim();
                    stageto = ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t21")).Text.Trim();
                }
                catch
                {
                    col2 = "0";
                }
                switch (lbl1a.Text)
                {
                    case "22":
                    case "23":
                    case "29":
                        SQuery = "SELECT distinct b.REVIS_NO||a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit,a.hsname,b.revis_no,b.bal as Stock_Qty from item a inner join (" + fgen.WIPSTKQry(frm_cocd, frm_qstr, frm_mbr, fromdt, todt) + ") b on trim(a.icode)=trim(b.icode) and length(trim(nvl(b.revis_no,'-'))) >6 " +
                            "where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 and trim(A.icode) not in (" + col1 + ") and trim(b.stage)='" + col2 + "' order by a.Iname ";
                        break;
                    case "21":
                        SQuery = "SELECT distinct b.revis_no||a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit,a.hsname,b.revis_no,b.bal as Stock_Qty,'" + stagefrom + "' as Stage_From,'" + stagefor + "' as Stage_For,'" + stageto + "' as Stage_To from item a inner join (" + fgen.WIPSTKQry(frm_cocd, frm_qstr, frm_mbr, fromdt, todt) + ") b on trim(a.icode)=trim(b.icode) and length(trim(nvl(b.revis_no,'-'))) >6 " +
                            "where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 and trim(A.icode) not in (" + col1 + ") and trim(b.stage)='" + col2 + "' order by a.Iname ";
                        break;
                    case "24":
                        SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Cpartno as Part_no,a.irate as Prate,b.Cdrgno,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty,b.hsname from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate from rgpmst where branchcd='" + frm_mbr + "' and type in ('21','23') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate from ivoucherp where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode)  order by B.Iname,trim(a.fstr)";
                        break;
                    case "25":
                        SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Cpartno as Part_no,a.irate as Prate,b.Cdrgno,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty,b.hsname from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate from rgpmst where branchcd='" + frm_mbr + "' and type in ('21','23') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate from ivoucherp where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode)  order by B.Iname,trim(a.fstr)";
                        break;
                    case "27":
                        SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit,a.hsname from item a where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 and trim(A.icode) not in (" + col1 + ") order by a.Iname ";
                        break;
                }
                //SQuery = "select a.Fstr,max(a.Iname)as Item_Name,a.ERP_code,max(a.Cpartno)as Part_no,max(a.Irate) As Irate,sum(a.Qtyord)-sum(a.Soldqty) as Balance_Qty,b.Unit,b.hscode,max(a.pordno) as PO_No,a.Fstr as SO_link,max(a.cdisc) as CDisc,max(a.iexc_Addl) as iexc_Addl,max(a.sd) as frt_pu,max(a.ipack) as pkchg_pu,sum(a.Qtyord) as Qty_Ord,sum(a.Soldqty) as Sold_Qty from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,pordno,(Case when length(trim(nvl(desc9,'-')))>1 then desc9 else ciname end) as Iname,trim(Icode) as ERP_code,Cpartno,Irate,Qtyord,0 as Soldqty,nvl(Cdisc,0) as Cdisc,nvl(iexc_addl,0) as Iexc_Addl,nvl(sd,0) as sd ,nvl(ipack,0) as ipack from somas where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "' and trim(icat)!='Y' and trim(app_by)!='-'  union all SELECT to_ChaR(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(Icode) as fstr,null as pordno,null as Iname,trim(Icode) as ERP_code,null as Cpartno,0 as Irate,0 as Qtyord,iqtyout as Soldqty,0 as Cdisc,0 as iexc_Addl,0 as sd,0 as ipack  from ivoucher where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text.Substring(0, 2) + "' and trim(acode)='" + txtlbl4.Text + "')a,item b where trim(a.erp_code)=trim(B.icode)  group by a.fstr,a.ERP_code,b.unit,b.hscode having (case when sum(a.Qtyord)>0 then sum(a.Qtyord)-sum(a.Soldqty) else max(a.irate) end)>0 order by Item_Name,a.fstr";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);
                break;
            case "SG1_ROW_TAX":

                SQuery = "Select Type1 as fstr,Name,Type1 as Code,nvl(Rate,0) as Rate,nvl(Excrate,0) as Schg,exc_Addr as Ref_Code from type where id='S' and length(Trim(nvl(cstno,'-')))<=1 order by name";
                break;

            case "New":
            case "Edit":
            case "CHL":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (Prg_Id != "FB1057")
                {
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Chl_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Chl_Dt,b.Aname as Party_Name,a.acode,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a,famst b where trim(A.acode)=trim(b.acode) and  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " order by vdd desc,a." + doc_nf.Value + " desc";
                else if(btnval == "CHL_E")
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Chl_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Chl_Dt" +
                            ",'-' as Party_Name,a.acode,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " and a.acode = '-' order by vdd desc,a." + doc_nf.Value + " desc";
                }
                else
                {
                    if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                        SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Chl_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Chl_Dt" +
                            ",a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and" +
                            " a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " and a.acode = '-'  order by vdd desc,a." + doc_nf.Value + " desc";

                }
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
            txtlbl26.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select substr(to_char(sysdate,'dd/mm/yyyy hh24:mi'),12,5) as timx from dual", "timx");
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
    protected void btnchl_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            hffield.Value = "CHL";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Add Entry For This Form !!");
    }
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
        //--------------------------
        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;
        if (Prg_Id != "FB1057")
        {
            if (txtlbl4.Text.Trim().Length < 2)
            {
                reqd_nc = reqd_nc + 1;
                reqd_flds = reqd_flds + " / " + lbl4.Text;
            }

        }
        if (txtlbl5.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl5.Text;
        }
        if (txtlbl6.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl6.Text;
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
        if (txtlbl16.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl16.Text;
        }
        if (txtlbl18.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl18.Text;
        }


        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }

        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[19].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text) <= 0)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");
                i = sg1.Rows.Count;
            }
        }

        string last_entdt;
        //checks
        last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and " + doc_df.Value + " " + DateRange + " ", "ldt");
        if (last_entdt == "0") { }
        else if (edmode.Value != "Y")
        {
            if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
            }
        }
        last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
        if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
        {
            Checked_ok = "N";
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
        }


        //--------------------------
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }
        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only"); txtvchdate.Focus(); return; }


        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "Y");
        string ok_for_save = "Y"; string err_item, err_msg;

        //if (sg2.Rows.Count > 1)
        //{
        //    reelGridQty();
        //    err_msg = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_MSG");
        //    ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");

        //    if (ok_for_save == "N")
        //    {
        //        fgen.msg("-", "AMSG", err_msg);
        //        return;
        //    }
        //}

        //checkGridQty();

        //ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
        //err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");

        //if (ok_for_save == "N")
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' MRR Qty is Exceeding Gate Entry Qty , Please Check item '13' " + err_item);
        //    return;
        //}

        //**************** Stock Check
        string chkstock = "Y";
        if (frm_cocd == "HIME")
        {
            if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime("01/04/2019"))
                chkstock = "N";
        }
        if (chkstock == "Y")
            checkStockQty();

        ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
        err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");

        if (frm_cocd == "HIME") ok_for_save = "Y";

        if (ok_for_save == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' Cannot issue more then Stock Qty , Please Check item : " + err_item);
            return;
        }

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        if (frm_vty == "23" || frm_vty == "21")
        {
            //JWStockQty();
            //ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
            //err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_MSG");

            if (ok_for_save == "N")
            {
                //fgen.msg("-", "AMSG", err_item);
                //return;
            }
        }

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }
        //fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", fgenMV.Fn_Get_Mvar(frm_qstr, "fstr"));
        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
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
            if (gr.Cells[19].Text.ToString().Trim().Length > 4)
            {
                drQty = dtQty.NewRow();
                drQty["fstr"] = gr.Cells[19].Text.ToString().Trim() + "-" + txtlbl2.Text + "-" + txtlbl3.Text;
                drQty["iname"] = gr.Cells[20].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t1")).Text.ToString().Trim());
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


            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select (a.Qtyord)-(a.Soldqty) as Bal_Qty from (select fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||vchnum||'-'||to_ChaR(vchdate,'dd/mm/yyyy') as fstr,trim(Icode) as ERP_code,iqty_chl as Qtyord,0 as Soldqty,1 as prate from ivoucherp where branchcd='" + frm_mbr + "' and type like '00%'  and trim(Acode)||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')='" + txtlbl7.Text.Trim() + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "' union all SELECT trim(icode)||'-'||genum||'-'||to_ChaR(gedate,'dd/mm/yyyy') as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate from ivoucher where branchcd='" + frm_mbr + "' and type='0%' and trim(Acode)||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')='" + txtlbl7.Text.Trim() + txtlbl2.Text.Trim() + txtlbl3.Text.Trim() + "' and trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and a.fstr='" + drQty1["fstr"].ToString().Trim() + "' order by B.Iname,trim(a.fstr)", "Bal_Qty");

            if (fgen.make_double(sm.ToString()) > fgen.make_double(col1) && fgen.make_double(col1) > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", drQty1["fstr"].ToString().Trim());
                break;
            }
        }
        return null;
    }

    string reelGridQty()
    {
        DataTable dtQty = new DataTable();
        dtQty.Columns.Add(new DataColumn("icode", typeof(string)));
        dtQty.Columns.Add(new DataColumn("qty", typeof(double)));
        dtQty.Columns.Add(new DataColumn("rcount", typeof(double)));
        dtQty.Columns.Add(new DataColumn("iname", typeof(string)));
        DataRow drQty = null;
        foreach (GridViewRow gr in sg1.Rows)
        {
            if (gr.Cells[13].Text.ToString().Trim().Length > 4)
            {
                drQty = dtQty.NewRow();
                drQty["icode"] = gr.Cells[13].Text.ToString().Trim();
                drQty["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t2")).Text.ToString().Trim());
                drQty["rcount"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t4")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
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
            if (gr.Cells[3].Text.ToString().Trim().Length > 4 && fgen.make_double(((TextBox)gr.FindControl("sg2_t4")).Text.ToString().Trim()) > 0)
            {
                if (col1 != gr.Cells[3].Text.ToString().Trim()) i = 1;
                drQty1 = dtQty1.NewRow();
                drQty1["icode"] = gr.Cells[3].Text.ToString().Trim();
                col1 = gr.Cells[3].Text.ToString().Trim();
                drQty1["iname"] = gr.Cells[14].Text.ToString().Trim();
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

            if (fgen.make_double(sm.ToString()) != fgen.make_double(sm1.ToString()) && fgen.make_double(sm1.ToString()) > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_MSG", "Qty Mismatch for Item " + fgen.seek_iname_dt(dtQty, "icode='" + drQty2["icode"].ToString().Trim() + "'", "iname") + "'13' Grid 1 Qty : " + sm.ToString() + "'13'Grid 2 Qty : " + sm1.ToString());
                break;
            }

            sm = dtQty1.Compute("max(rcount)", "icode='" + drQty2["icode"].ToString().Trim() + "'");
            sm1 = dtQty.Compute("sum(rcount)", "icode='" + drQty2["icode"].ToString().Trim() + "'");

            if (fgen.make_double(sm.ToString()) != fgen.make_double(sm1.ToString()) && fgen.make_double(sm1.ToString()) > 0 && fgen.make_double(sm.ToString()) > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_MSG", "Qty Mismatch for Item " + fgen.seek_iname_dt(dtQty, "icode='" + drQty2["icode"].ToString().Trim() + "'", "iname") + "'13' Grid 1 Count : " + sm.ToString() + "'13'Grid 2 Count : " + sm1.ToString());
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
                drQty["fstr"] = gr.Cells[19].Text.ToString().Trim();
                drQty["iname"] = gr.Cells[20].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t1")).Text.ToString().Trim());
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
            // stock check

            frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
            //col1 = fgen.SeekWipStock(frm_cocd, frm_qstr, frm_mbr, drQty1["fstr"].ToString().Trim(), txtlbl70B.Text, "", fromdt, todt, " where type || trim(vchnum) || to_Char(vchdate, 'dd/mm/yyyy') != '" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");

            //col1 = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, drQty1["fstr"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");

            if (fgen.make_double(col1) == 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", drQty1["fstr"].ToString().Trim());
                break;
            }

            if (fgen.make_double(sm.ToString()) > fgen.make_double(col1))
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", drQty1["fstr"].ToString().Trim());
                break;
            }
        }
        return null;
    }

    string JWStockQty()
    {
        DataTable dtQty = new DataTable();
        dtQty.Columns.Add(new DataColumn("icode", typeof(string)));
        dtQty.Columns.Add(new DataColumn("qty", typeof(double)));
        DataRow drQty = null;
        foreach (GridViewRow gr in sg1.Rows)
        {
            if (gr.Cells[13].Text.ToString().Trim().Length > 4)
            {
                drQty = dtQty.NewRow();
                drQty["icode"] = gr.Cells[13].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t1")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
            }
        }

        if (sg3.Rows.Count < 1)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_MSG", "Select Item to Receive First!!");
            return null;
        }

        DataTable dtQty1 = new DataTable();
        dtQty1.Columns.Add(new DataColumn("icode", typeof(string)));
        dtQty1.Columns.Add(new DataColumn("qty", typeof(double)));
        DataRow drQty1 = null;
        col1 = "";
        i = 1;
        foreach (GridViewRow gr in sg3.Rows)
        {
            if (gr.Cells[3].Text.ToString().Trim().Length > 4 && fgen.make_double(((TextBox)gr.FindControl("sg3_t1")).Text.ToString().Trim()) > 0)
            {
                if (col1 != gr.Cells[3].Text.ToString().Trim()) i = 1;
                drQty1 = dtQty1.NewRow();
                drQty1["icode"] = gr.Cells[3].Text.ToString().Trim();
                col1 = gr.Cells[3].Text.ToString().Trim();
                drQty1["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg3_t1")).Text.ToString().Trim());
                dtQty1.Rows.Add(drQty1);
            }
        }

        object sm, sm1;

        DataView distQty = new DataView(dtQty, "", "icode", DataViewRowState.CurrentRows);
        DataTable dtQty2 = new DataTable();
        dtQty2 = distQty.ToTable(true, "icode");

        foreach (DataRow drQty2 in dtQty2.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "icode='" + drQty2["icode"].ToString().Trim() + "'");
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "sELECT jwq_ctrl FROM ITEM WHERE TRIM(ICODE)='" + drQty2["icode"].ToString().Trim() + "'", "jwq_ctrl");
            if (col1 == "Y")
            {
                sm1 = dtQty1.Compute("sum(qty)", "icode='" + drQty2["icode"].ToString().Trim() + "'");

                if (fgen.make_double(sm.ToString()) != fgen.make_double(sm1.ToString()))
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_MSG", "Qty Mismatch for Item " + fgen.seek_iname(frm_qstr, frm_cocd, "Select iname from item where trim(icode)='" + drQty2["icode"].ToString().Trim() + "'", "iname") + "'13' Grid 1 Qty : " + sm.ToString() + "'13'Grid 2 Qty : " + sm1.ToString());
                    break;
                }
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
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from reelvch a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from rgpmst a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data a where par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

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
                case "SFROM":
                    if (col1.ToString().Length < 2) return;
                    //if (col1.ToString().Length < 2) return;
                    //txtlbl70B.Text = col1; txtlbl71B.Text = col2;
                    //txtlbl70D.Focus();
                    //// Opening STO Popup
                    //hffield.Value = "STO";
                    //make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Stage To", frm_qstr);
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t19")).Text = col1 + "~" + col2;
                    break;
                case "SFOR":
                    if (col1.ToString().Length < 2) return;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t20")).Text = col1 + "~" + col2;
                    break;
                case "STO":
                    if (col1.ToString().Length < 2) return;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t21")).Text = col1 + "~" + col2;

                    //txtlbl70D.Text = col1; txtlbl71D.Text = col2;
                    //create_tab();
                    //sg1_add_blankrows();
                    //sg1.DataSource = sg1_dt; sg1.DataBind(); ViewState["sg1"] = sg1_dt;
                    //((ImageButton)sg1.Rows[0].FindControl("btnadd")).Focus();
                    // Opening Item Popup
                    //hf1.Value = "1";
                    //hffield.Value = "Row_Add";
                    //make_qry_4_popup();
                    //fgen.Fn_open_sseek("Select Your Product", frm_qstr);
                    break;

                case "New":
                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    string cond = "and type='" + frm_vty + "'";
                    if (lbl1a.Text == "27" || lbl1a.Text == "22") cond = "and type in ('22','27') ";
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
                    sg4_dt.Dispose();
                    //-------------------------------------------
                    break;
                #endregion
                case "Del":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry No to Delete", frm_qstr);
                    break;
                case "CHL":
                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    hffield.Value = "Edit_E";
                    if (Prg_Id != "FB1057" && btnval == "CHL")
                    {
                        hffield.Value = "CHL_E";
                    }
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Request to Update", frm_qstr);
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
                    fgen.Fn_open_mseek("Select Entry No to Print", frm_qstr);
                    break;
                case "Edit_E":
                case "CHL_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    string mv_col;
                    mv_col = frm_mbr + frm_vty + col1;
                    SQuery = "Select a.*,to_char(A.podate,'dd/mm/yyyy') as podtd,c.aname,nvl(b.Iname,'-') As Iname,nvl(b.cpartno,'-') As Icpartno,nvl(b.cdrgno,'-') As Icdrgno,nvl(b.unit,'-') as IUnit " +
                    "from " + frm_tabname + " a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) " +
                    "and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "'  ORDER BY A.morder";
                    if (btnval == "CHL_E")
                    {
                        SQuery = "Select a.*,to_char(A.podate,'dd/mm/yyyy') as podtd,'-' aname,nvl(b.Iname,'-') As Iname,nvl(b.cpartno,'-') As Icpartno,nvl(b.cdrgno,'-') As Icdrgno,nvl(b.unit,'-') as IUnit " +
                 "from " + frm_tabname + " a,item b where trim(a.icode)=trim(b.icode) " +
                 "and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "'  ORDER BY A.morder";
                    }
                    if (Prg_Id == "FB1057")
                    {
                    SQuery = "Select a.*,to_char(A.podate,'dd/mm/yyyy') as podtd,'-' aname,nvl(b.Iname,'-') As Iname,nvl(b.cpartno,'-') As Icpartno,nvl(b.cdrgno,'-') As Icdrgno,nvl(b.unit,'-') as IUnit " +
                        "from " + frm_tabname + " a,item b where trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "'  ORDER BY A.morder";
                    }
                    else
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
                        //txtlbl70B.Text = dt.Rows[i]["stage"].ToString().Trim();
                        //txtlbl71B.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='1' and type1 = '" + txtlbl70B.Text.Trim() + "'", "name");
                        //txtlbl70D.Text = dt.Rows[i]["iopr"].ToString().Trim();
                        //txtlbl71D.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='1' and type1 = '" + txtlbl70D.Text.Trim() + "'", "name");
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();
                        txtlbl5.Text = dt.Rows[i]["prnum"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["EXC_TIME"].ToString().Trim();
                        txtlbl8.Text = dt.Rows[i]["pname"].ToString().Trim();
                        txtlbl9.Text = dt.Rows[i]["o_deptt"].ToString().Trim();
                        txtlbl15.Text = dt.Rows[i]["st_entform"].ToString().Trim();
                        txtlbl16.Text = dt.Rows[i]["thru"].ToString().Trim();
                        txtlbl17.Text = dt.Rows[i]["Location"].ToString().Trim();
                        txtlbl18.Text = dt.Rows[i]["freight"].ToString().Trim();
                        txtlbl24.Text = dt.Rows[i]["binno"].ToString().Trim();
                        txtlbl26.Text = dt.Rows[i]["mtime"].ToString().Trim();
                        txtlbl28.Text = dt.Rows[i]["iqty_chlwt"].ToString().Trim();
                        txtlbl30.Text = dt.Rows[i]["no_Cases"].ToString().Trim();
                        txtrmk.Text = dt.Rows[i]["naration"].ToString().Trim();

                        txtlbl7.Text = dt.Rows[i]["vcode"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT aname FROM famst WHERE trim(acode)='" + txtlbl7.Text.Trim() + "'", "aname");

                        txtlbl70.Text = dt.Rows[i]["gst_pos"].ToString().Trim();
                        txtlbl71.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT Name FROM Type WHERE id='{' and trim(type1)='" + txtlbl70.Text.Trim() + "'", "name");

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
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";




                            sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["ICpartno"].ToString().Trim();
                            //sg1_dr["sg1_f4"] = dt.Rows[i]["Icdrgno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = fgen.SeekWipStock(frm_cocd, frm_qstr, frm_mbr, dt.Rows[i]["Icode"].ToString().Trim(), dt.Rows[i]["stage"].ToString(), dt.Rows[i]["revis_no"].ToString().Trim(), fromdt, todt, " where type || trim(vchnum) || to_Char(vchdate, 'dd/mm/yyyy') != '" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                            sg1_dr["sg1_f5"] = dt.Rows[i]["IUnit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["iqtyout"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["iqty_Wt"].ToString().Trim();
                            sg1_dr["sg1_t19"] = dt.Rows[i]["stage"].ToString().Trim() + "~" + fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='1' and trim(type1) = '" + dt.Rows[i]["stage"].ToString().Trim() + "'", "name"); ;
                            sg1_dr["sg1_t20"] = dt.Rows[i]["iopr"].ToString().Trim() + "~" + fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='1' and trim(type1) = '" + dt.Rows[i]["iopr"].ToString().Trim() + "'", "name"); ;
                            sg1_dr["sg1_t21"] = dt.Rows[i]["stagef"].ToString().Trim() + "~" + fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='1' and trim(type1) = '" + dt.Rows[i]["stagef"].ToString().Trim() + "'", "name"); ;

                            sg1_dr["sg1_t3"] = dt.Rows[i]["no_bdls"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["desc_"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["irate"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["iamount"].ToString().Trim();

                            sg1_dr["sg1_t7"] = dt.Rows[i]["exc_Rate"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["cess_percent"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["desc_"].ToString().Trim();
                            if (frm_vty == "21")
                            {
                                sg1_dr["sg1_t9"] = dt.Rows[i]["revis_no"].ToString().Trim();
                            }
                            sg1_dr["sg1_t10"] = dt.Rows[i]["btchno"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["btchdt"].ToString().Trim();

                            //sg1_dr["sg1_t12"] = dt.Rows[i]["mfgdt"].ToString().Trim();
                            //sg1_dr["sg1_t13"] = dt.Rows[i]["expdt"].ToString().Trim();

                            sg1_dr["sg1_t14"] = dt.Rows[i]["ponum"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["ordlineno"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["podtd"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }


                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        sg1_dt.Dispose();
                        //------------------------

                        // REEL TABLE
                        //SQuery = "SELECT A.*,b.iname FROM REELVCH A,item b WHERE trim(a.icodE)=trim(B.icode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' order by a.srno";
                        SQuery = "SELECT sysdate from dual where 1=2";
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
                        sg2_dt.Dispose();

                        //-----------------------
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
                        sg4_dt.Dispose();
                        //------------------------

                        //------------------------
                        SQuery = "Select a.*,to_char(A.podate,'dd/mm/yyyy') as podtd,c.aname,nvl(b.Iname,'-') As Iname,nvl(b.cpartno,'-') As Icpartno,nvl(b.cdrgno,'-') As Icdrgno,nvl(b.unit,'-') as IUnit from rgpmst a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "'  ORDER BY A.segment_";

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

                                sg3_dr["sg3_f3"] = dt.Rows[i]["icpartno"].ToString().Trim();
                                sg3_dr["sg3_f4"] = dt.Rows[i]["icdrgno"].ToString().Trim();
                                sg3_dr["sg3_f5"] = dt.Rows[i]["iunit"].ToString().Trim();

                                sg3_dr["sg3_t1"] = dt.Rows[i]["iqtyout"].ToString().Trim();
                                sg3_dr["sg3_t2"] = dt.Rows[i]["iqty_wt"].ToString().Trim();
                                sg3_dr["sg3_t3"] = dt.Rows[i]["no_bdls"].ToString().Trim();
                                sg3_dr["sg3_t4"] = dt.Rows[i]["desc_"].ToString().Trim();
                                sg3_dr["sg3_t5"] = dt.Rows[i]["irate"].ToString().Trim();
                                sg3_dr["sg3_t6"] = dt.Rows[i]["iamount"].ToString().Trim();



                                sg3_dt.Rows.Add(sg3_dr);
                            }
                        }
                        sg3_add_blankrows();
                        ViewState["sg3"] = sg3_dt;
                        sg3.DataSource = sg3_dt;
                        sg3.DataBind();
                        sg3_dt.Dispose();

                        //-----------------------
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
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;
                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1007");
                    if (Prg_Id == "FB1057")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1008");
                    }
                    fgen.fin_invn_reps(frm_qstr);
                    break;

                case "TACODE":
                    //-----------------------------
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");
                    txtlbl73.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");

                    btnlbl7.Focus();
                    break;
                //-----------------------------
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

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();


                        String pop_qry;

                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");

                        switch (lbl1a.Text)
                        {
                            case "22":
                            case "23":
                            case "29":
                                if (col1.Trim().Length < 8) SQuery = "select '-' as po_no,'-' as fstr,a.Icode,a.iname,a.cpartno,a.cdrgno,a.irate,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from item a,typegrp b where trim(a.hscode)=trim(b.acref) and trim(a.hsname)=trim(b.name) and b.id='T1' and trim(a.icode) in (" + col1 + ")";
                                else SQuery = "select '-' as po_no,'-' as fstr,a.Icode,a.iname,a.cpartno,a.cdrgno,a.irate,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7  from item a,typegrp b where trim(a.hscode)=trim(b.acref) and trim(a.hsname)=trim(b.name) and b.id='T1' and trim(a.icode) in (" + col1 + ")";
                                //SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit from item a where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 and trim(A.icode) not in (" + col1 + ") order by a.Iname ";
                                break;
                            case "21":
                                if (col1.Trim().Length < 8) SQuery = "select '-' as po_no,'-' as fstr,a.Icode,a.iname,a.cpartno,a.cdrgno,a.irate,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from item a,typegrp b where trim(a.hscode)=trim(b.acref) and trim(a.hsname)=trim(b.name) and b.id='T1' and trim(a.icode) in (" + col1 + ")";
                                else
                                {
                                    SQuery = "select distinct '-' as po_no,'-' as fstr,c.revis_no as trackno,a.Icode,a.iname,a.cpartno,a.cdrgno,a.irate,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,c.Stage_From,c.Stage_For,c.Stage_To " +
                                   "from item a,typegrp b,(" + pop_qry + ") c where trim(a.hscode)=trim(b.acref) and trim(a.hsname)=trim(b.name) " +
                                   "and b.id='T1' and trim(a.icode)=trim(c.icode) and trim(c.fstr) in ('" + col1 + "')";

                                    //SQuery = "SELECT a.* from (" + pop_qry + ") a where a.fstr in (" + col1 + ")";
                                }

                                //      SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit,a.hsname,b.revis_no,b.bal as Stock_Qty from item a inner join (" + fgen.WIPSTKQry(frm_cocd, frm_qstr, frm_mbr, "", "") + ") b on trim(a.icode)=trim(b.icode) and length(trim(nvl(b.revis_no,'-'))) >6 " +
                                //"where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 and trim(A.icode) not in (" + col1 + ") and trim(b.stage)='" + ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t19")).Text.Trim().Split('~')[0] + "' order by a.Iname ";



                                //      SQuery = "select '-' as po_no,'-' as fstr,a.Icode,a.iname,a.cpartno,a.cdrgno,a.irate,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 " +
                                //          "from item a,typegrp b inner join () where trim(a.hscode)=trim(b.acref) and trim(a.hsname)=trim(b.name) " +
                                //          "and b.id='T1' and trim(a.icode) in (" + col1 + ")";

                                //SQuery = "SELECT distinct a.Icode as FStr,a.Iname as Item_Name,a.Icode,a.cpartno,a.cdrgno,a.unit from item a where  length(trim(nvl(a.deac_by,'-'))) <2 AND LENGTH(tRIM(a.ICODE))>=8 and trim(A.icode) not in (" + col1 + ") order by a.Iname ";
                                break;
                            case "24":
                                SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,trim(a.ERP_code) as ERP_code,b.Cpartno as Part_no,a.irate as Prate,b.Cdrgno,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,trim(a.Fstr) as PO_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty from (select fstr,ERP_code,max(irate) as irate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(to_Char(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,irate from rgpmst where branchcd='" + frm_mbr + "' and type in ('21','23') and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "' union all SELECT trim(icode)||'-'||to_ChaR(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate from ivoucherp where branchcd='" + frm_mbr + "' and type='00' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(Acode)='" + txtlbl7.Text.Trim() + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode)  order by B.Iname,trim(a.fstr)";
                                break;
                            case "25":
                                if (col1.Trim().Length == 8) SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and trim(a.hsname)=trim(B.name) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                                else SQuery = "select a.po_no,a.fstr,a.ERP_code as icode,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.irate,a.iexc_addl,a.frt_pu,a.pkchg_pu,a.balance_qty,a.Cdisc,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and trim(a.hsname)=trim(B.name) and b.id='T1' and trim(a.fstr) in (" + col1 + ")";
                                break;
                            case "27":
                                if (col1.Trim().Length < 8) SQuery = "select '-' as po_no,'-' as fstr,a.Icode,a.iname,a.cpartno,a.cdrgno,a.irate,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from item a,typegrp b where trim(a.hscode)=trim(b.acref) and trim(a.hsname)=trim(B.name) and b.id='T1' and trim(a.icode) in (" + col1 + ")";
                                else SQuery = "select '-' as po_no,'-' as fstr,a.Icode,a.iname,a.cpartno,a.cdrgno,a.irate,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7  from item a,typegrp b where trim(a.hscode)=trim(b.acref) and trim(a.hsname)=trim(B.name) and b.id='T1' and trim(a.icode) in (" + col1 + ")";
                                break;
                        }


                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";

                            sg1_dr["sg1_t19"] = dt.Rows[d]["Stage_From"].ToString().Trim();
                            sg1_dr["sg1_t20"] = dt.Rows[d]["Stage_For"].ToString().Trim();
                            sg1_dr["sg1_t21"] = dt.Rows[d]["Stage_To"].ToString().Trim();
                            sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = fgen.SeekWipStock(frm_cocd, frm_qstr, frm_mbr, dt.Rows[d]["Icode"].ToString().Trim(), dt.Rows[d]["Stage_From"].ToString().Trim().Split('~')[0], dt.Rows[d]["trackno"].ToString().Trim(), fromdt, todt, " where type || trim(vchnum) || to_Char(vchdate, 'dd/mm/yyyy') != '" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                            //sg1_dr["sg1_f4"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[d]["Icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'");
                            sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = "0";
                            sg1_dr["sg1_t2"] = "0";
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = "";
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
                            if (frm_vty == "21")
                            {
                                sg1_dr["sg1_t9"] = dt.Rows[d]["trackno"].ToString().Trim();
                            }
                            sg1_dr["sg1_t10"] = "-";
                            sg1_dr["sg1_t11"] = "0";
                            sg1_dr["sg1_t12"] = "0";
                            sg1_dr["sg1_t13"] = "0";

                            //string mpo_Dt;
                            //mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(9, 6);
                            //sg1_dr["sg1_t14"] = mpo_Dt;
                            //sg1_dr["sg1_t15"] = "";
                            //mpo_Dt = dt.Rows[d]["fstr"].ToString().Trim().Substring(6, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(4, 2) + "/" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 4);
                            //sg1_dr["sg1_t16"] = fgen.make_def_Date(mpo_Dt, vardate);


                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
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

                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;
                            sg2_dr["sg2_h1"] = col1;
                            sg2_dr["sg2_h2"] = col2;
                            sg2_dr["sg2_h3"] = "-";
                            sg2_dr["sg2_h4"] = "-";
                            sg2_dr["sg2_h5"] = "-";

                            sg2_dr["sg2_f1"] = col1;
                            sg2_dr["sg2_f2"] = col2;
                            sg2_dr["sg2_f3"] = "-";
                            sg2_dr["sg2_f4"] = "-";
                            sg2_dr["sg2_f5"] = "-";

                            sg2_dr["sg2_t1"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                            sg2_dr["sg2_t2"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                            sg2_dr["sg2_t3"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");
                            sg2_dr["sg2_t4"] = "0";
                            sg2_dr["sg2_t5"] = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL9").ToString().Trim().Replace("&amp", "");

                            sg2_dt.Rows.Add(sg2_dr);
                        }
                    }
                    sg2_add_blankrows();

                    ViewState["sg2"] = sg2_dt;
                    sg2.DataSource = sg2_dt;
                    sg2.DataBind();
                    sg2_dt.Dispose();
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
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

                    setColHeadings();
                    break;
                case "BTNRCV":
                    col1 = Request.Cookies["REPLY"].Value;
                    hffield.Value = "";
                    if (col1 == "N") return;
                    rcvMatl();
                    break;
                case "SG3_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    if (edmode.Value == "Y")
                    {
                        //return;
                    }

                    //********* Saving in Hidden Field
                    //sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                    //sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    //********* Saving in GridView Value
                    sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = col1;
                    sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[4].Text = col2;
                    sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[5].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[6].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    sg3.Rows[Convert.ToInt32(hf1.Value)].Cells[7].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");

                    setColHeadings();
                    break;
                case "SG2_ROW_JOB":
                    if (col1.Length <= 0) return;
                    ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t6")).Text = col2;
                    ((TextBox)sg2.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg2_t7")).Text = col3;
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
                            sg3_dr["sg3_f3"] = dt.Rows[i]["sg3_f3"].ToString();
                            sg3_dr["sg3_f4"] = dt.Rows[i]["sg3_f4"].ToString();
                            sg3_dr["sg3_f5"] = dt.Rows[i]["sg3_f5"].ToString();

                            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();

                            sg3_dr["sg3_t5"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t5")).Text.Trim();
                            sg3_dr["sg3_t6"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t6")).Text.Trim();
                            sg3_dr["sg3_t7"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t7")).Text.Trim();
                            sg3_dr["sg3_t8"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t8")).Text.Trim();

                            sg3_dr["sg3_t9"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t9")).Text.Trim();
                            sg3_dr["sg3_t10"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t10")).Text.Trim();


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

                            sg3_dr["sg3_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();
                            sg3_dr["sg3_f4"] = dt.Rows[d]["cdrgno"].ToString().Trim();
                            sg3_dr["sg3_f5"] = dt.Rows[d]["unit"].ToString().Trim();

                            sg3_dr["sg3_t1"] = "";
                            sg3_dr["sg3_t2"] = "";
                            sg3_dr["sg3_t3"] = "";
                            sg3_dr["sg3_t4"] = "";
                            sg3_dr["sg3_t5"] = "";
                            sg3_dr["sg3_t6"] = "";
                            sg3_dr["sg3_t7"] = "";
                            sg3_dr["sg3_t8"] = "";
                            sg3_dr["sg3_t9"] = "";
                            sg3_dr["sg3_t10"] = "";

                            sg3_dt.Rows.Add(sg3_dr);
                        }
                    }
                    sg3_add_blankrows();

                    ViewState["sg3"] = sg3_dt;
                    sg3.DataSource = sg3_dt;
                    sg3.DataBind();
                    sg3_dt.Dispose();
                    #endregion
                    break;
                case "SG1_ROW_TAX":
                    //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t11")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");

                    break;
                case "SG1_ROW_DT":
                    //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
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
                            sg3_dr["sg3_f3"] = sg3.Rows[i].Cells[5].Text.Trim();
                            sg3_dr["sg3_f4"] = sg3.Rows[i].Cells[6].Text.Trim();
                            sg3_dr["sg3_f5"] = sg3.Rows[i].Cells[7].Text.Trim();

                            sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                            sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                            sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                            sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();

                            sg3_dr["sg3_t5"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t5")).Text.Trim();
                            sg3_dr["sg3_t6"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t6")).Text.Trim();
                            sg3_dr["sg3_t7"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t7")).Text.Trim();
                            sg3_dr["sg3_t8"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t8")).Text.Trim();

                            sg3_dr["sg3_t9"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t9")).Text.Trim();
                            sg3_dr["sg3_t10"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t10")).Text.Trim();


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
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "Select a.Vchnum as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as Dated,c.Aname as Supplier,b.Iname as Item_Name,b.cpartno as Part_No,a.iqtyin as MRR_Qty,a.Irate,b.unit,b.hscode,a.Desc_,a.icode,a.ent_by,a.ent_Dt from " + frm_tabname + " a, item b,famst c where a.branchcd='" + frm_mbr + "'  and a.type='" + frm_vty + "' and a." + doc_df.Value + " " + PrdRange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) order by a." + doc_df.Value + ",a." + doc_nf.Value + ",a.morder ";
            if (Prg_Id == "FB1057")
            {
                SQuery = "Select a.Vchnum as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as Dated,'-' as Supplier,b.Iname as Item_Name,b.cpartno as Part_No,a.iqtyin as MRR_Qty,a.Irate,b.unit,b.hscode,a.Desc_," +
                    "a.icode,a.ent_by,a.ent_Dt from " + frm_tabname + " a, item b where a.branchcd='" + frm_mbr + "'  and " +
                    "a.type='" + frm_vty + "' and a." + doc_df.Value + " " + PrdRange + " and trim(a.icode)=trim(b.icode) AND a.acode='-' order by " +
                    "a." + doc_df.Value + ",a." + doc_nf.Value + ",a.morder ";
    
            }
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
            if (col1 == "Y" && Checked_ok == "Y")
            {
                try
                {
                    oDS = new DataSet();
                    oporow = null;
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                    //oDS2 = new DataSet();
                    //oporow2 = null;
                    //oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "ivchctrl");

                    //oDS3 = new DataSet();
                    //oporow3 = null;
                    //oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "reelvch");

                    oDS4 = new DataSet();
                    oporow4 = null;
                    oDS4 = fgen.fill_schema(frm_qstr, frm_cocd, "rgpmst");

                    //oDS5 = new DataSet();
                    //oporow5 = null;
                    //oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");

                    // This is for checking that, is it ready to save the data
                    frm_vnum = "000000";

                    save_ivch();

                    //save_rgp();
                    //save_reelv();

                    //save_fun5();

                    oDS.Dispose();
                    oporow = null;
                    oDS = new DataSet();
                    oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                    //oDS2.Dispose();
                    //oporow2 = null;
                    //oDS2 = new DataSet();
                    //oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "ivchctrl");

                    //oDS3.Dispose();
                    //oporow3 = null;
                    //oDS3 = new DataSet();
                    //oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "reelvch");

                    oporow4 = null;
                    oDS4 = new DataSet();
                    oDS4 = fgen.fill_schema(frm_qstr, frm_cocd, "rgpmst");

                    //oDS5.Dispose();
                    //oporow5 = null;
                    //oDS5 = new DataSet();
                    //oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");


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
                            if (sg1.Rows[i].Cells[19].Text.Trim().Length > 2)
                            {
                                save_it = "Y";
                            }
                        }

                        if (save_it == "Y")
                        {
                            i = 0;

                            string next_vnum;
                            string last_vnum;
                            string cond = "and type='" + frm_vty + "'";
                            if (lbl1a.Text == "27" || lbl1a.Text == "22") cond = "and type in ('22','27') ";
                            do
                            {
                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' " + cond + " and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                                pk_error = "N";
                                //next_vnum = frm_vnum;
                                //last_vnum = fgen.seek_iname(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ") as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' " + cond + " and " + doc_df.Value + " " + DateRange + "", "vch");

                                //pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);

                                //if ((fgen.make_double(next_vnum) - 1) != fgen.make_double(last_vnum) && fgen.make_double(next_vnum) > 1) pk_error = "Y";



                                //if (i > 20)
                                //{

                                //    //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_Ctrl a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy') not in (;

                                //    fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                //    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 1 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' " + cond + " and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                                //    pk_error = "N";
                                //    i = 0;
                                //}
                                //i++;
                            }
                            while (pk_error == "Y");
                        }
                    }

                    // If Vchnum becomes 000000 then Re-Save
                    if (frm_vnum == "000000") btnhideF_Click(sender, e);


                    save_ivch();
                    save_rgp();
                    //save_reelv();

                    //save_fun5();


                    string ddl_fld1;
                    string ddl_fld2;
                    ddl_fld1 = frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    ddl_fld2 = frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");

                    if (edmode.Value == "Y")
                    {
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "update reelvch set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "update rgpmst set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "update udf_Data set branchcd='DD' where par_tbl='" + frm_tabname + "' and par_fld='" + ddl_fld1 + "'");

                    }

                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                    //fgen.save_data(frm_cocd, oDS2, "ivchctrl");
                    //fgen.save_data(frm_cocd, oDS3, "REELvch");
                    fgen.save_data(frm_qstr, frm_cocd, oDS4, "rgpmst");
                    //fgen.save_data(frm_cocd, oDS5, "udf_Data");

                    if (edmode.Value == "Y")
                    {
                        fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully'13'Do You want to see Print Out");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from REELvch where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from rgpmst where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                        //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data where branchcd='DD' and par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + ddl_fld2 + "'");
                    }
                    else
                    {
                        if (save_it == "Y")
                        {
                            fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully'13'Do You want to see Print Out");
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Data Not Saved");
                        }
                    }


                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtvchdate.Text.Trim() + "'");



                    fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();


                    hffield.Value = "SAVED";
                }
                catch (Exception ex)
                {
                    fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                    fgen.msg("-", "AMSG", ex.Message.ToString());
                    col1 = "N";
                }
            }
            #endregion
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

    }
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

    public void create_tab3()
    {


        sg3_dt = new DataTable();
        sg3_dr = null;
        // Hidden Field

        sg3_dt.Columns.Add(new DataColumn("sg3_SrNo", typeof(Int32)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f3", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f4", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_f5", typeof(string)));

        sg3_dt.Columns.Add(new DataColumn("sg3_t1", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t2", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t3", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t4", typeof(string)));

        sg3_dt.Columns.Add(new DataColumn("sg3_t5", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t6", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t7", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t8", typeof(string)));

        sg3_dt.Columns.Add(new DataColumn("sg3_t9", typeof(string)));
        sg3_dt.Columns.Add(new DataColumn("sg3_t10", typeof(string)));


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
        sg1_dr["sg1_t1"] = "0";
        sg1_dr["sg1_t2"] = "-";
        sg1_dr["sg1_t3"] = "-";
        sg1_dr["sg1_t4"] = "-";
        sg1_dr["sg1_t5"] = "0";
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
    public void sg3_add_blankrows()
    {
        sg3_dr = sg3_dt.NewRow();

        sg3_dr["sg3_SrNo"] = sg3_dt.Rows.Count + 1;
        sg3_dr["sg3_f1"] = "-";
        sg3_dr["sg3_f2"] = "-";
        sg3_dr["sg3_f3"] = "-";
        sg3_dr["sg3_f4"] = "-";
        sg3_dr["sg3_f5"] = "-";

        sg3_dr["sg3_t1"] = "-";
        sg3_dr["sg3_t2"] = "-";
        sg3_dr["sg3_t3"] = "-";
        sg3_dr["sg3_t4"] = "-";

        sg3_dr["sg3_t5"] = "-";
        sg3_dr["sg3_t6"] = "-";
        sg3_dr["sg3_t7"] = "-";
        sg3_dr["sg3_t8"] = "-";

        sg3_dr["sg3_t9"] = "-";
        sg3_dr["sg3_t10"] = "-";


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

            setGST();
            //if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
            //{
            //    sg1.HeaderRow.Cells[24].Text = "CGST";
            //    sg1.HeaderRow.Cells[25].Text = "SGST/UTGST";
            //}
            //else
            //{
            //    sg1.HeaderRow.Cells[24].Text = "IGST";
            //    sg1.HeaderRow.Cells[25].Text = "-";
            //}
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg3r = 0; sg3r < sg3.Rows.Count; sg3r++)
            {
                for (int j = 0; j < sg3.Columns.Count; j++)
                {
                    sg3.Rows[sg3r].Cells[j].ToolTip = sg3.Rows[sg3r].Cells[j].Text;
                    if (sg3.Rows[sg3r].Cells[j].Text.Trim().Length > 35)
                    {
                        sg3.Rows[sg3r].Cells[j].Text = sg3.Rows[sg3r].Cells[j].Text.Substring(0, 35);

                    }
                }
            }

        }
    }
    void cal()
    {
        double vp = 0, vp1 = 0;
        for (int zk = 0; zk < sg1.Rows.Count - 1; zk++)
        {
            //vp1 = Convert.ToDouble(((TextBox)sg1.Rows[zk].FindControl("txtfld1")).Text.Trim());
            vp += vp1;
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
            case "Rmv":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString(); hffield.Value = "Rmv";
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove this item from list");
                }
                break;
            case "SG1_ROW_STGFRM":
                hf1.Value = index.ToString();
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                if (index < sg1.Rows.Count - 1)
                {
                    hffield.Value = "SFROM";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select From Stage", frm_qstr);
                }
                else
                {
                    hffield.Value = "SFROM";
                    make_qry_4_popup();
                    //fgen.open_mseek("Select Your Product(s)");
                    fgen.Fn_open_sseek("Select From Stage", frm_qstr);
                }
                this.cal();
                break;
            case "SG1_ROW_STGFOR":

                hf1.Value = index.ToString();
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                if (index < sg1.Rows.Count - 1)
                {
                    hffield.Value = "SFOR";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Stage For", frm_qstr);
                }
                else
                {
                    hffield.Value = "SFOR";
                    make_qry_4_popup();
                    //fgen.open_mseek("Select Your Product(s)");
                    fgen.Fn_open_sseek("Select From Stage", frm_qstr);
                }
                this.cal();
                break;
            case "SG1_ROW_STGTO":

                hf1.Value = index.ToString();
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                if (index < sg1.Rows.Count - 1)
                {
                    hffield.Value = "STO";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Stage For", frm_qstr);
                }
                else
                {
                    hffield.Value = "STO";
                    make_qry_4_popup();
                    //fgen.open_mseek("Select Your Product(s)");
                    fgen.Fn_open_sseek("Select From Stage", frm_qstr);
                }
                this.cal();
                break;
            case "Row_Add":
                //if (txtlbl70B.Text == "" || txtlbl70B.Text == "0") fgen.msg("-", "AMSG", "First Please Select From Stage!!");
                //else if (txtlbl70D.Text == "" || txtlbl70D.Text == "0") fgen.msg("-", "AMSG", "First Please Select To Stage!!");
                //else
                //{
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    hffield.Value = "Row_Edit";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Your Product", frm_qstr);
                }
                else
                {
                    hffield.Value = "Row_Add";
                    make_qry_4_popup();
                    //fgen.open_mseek("Select Your Product(s)");
                    fgen.Fn_open_sseek("Select Your Product", frm_qstr);
                }
                this.cal();
                //}
                break;

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
                //if (txtlbl70B.Text == "" || txtlbl70B.Text == "0") fgen.msg("-", "AMSG", "First Please Select From Stage!!");
                //else if (txtlbl70D.Text == "" || txtlbl70D.Text == "0") fgen.msg("-", "AMSG", "First Please Select To Stage!!");
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
                foreach (GridViewRow gr2 in sg2.Rows)
                {
                    if (col1.Length > 0) col1 += ",'" + gr2.Cells[3].Text.Trim().ToString() + ((TextBox)gr2.FindControl("sg2_t1")).Text.Trim().ToString() + "'";
                    else col1 = "'" + gr2.Cells[3].Text.Trim().ToString() + ((TextBox)gr2.FindControl("sg2_t1")).Text.Trim().ToString() + "'";
                }

                SQuery = "SELECT TRIM(ICODe) AS FSTR,INAME AS PRODUCT,ICODE AS ERPCODE,OPRATE1 AS SIZE_,OPRATE3 AS GSM,UNIT FROM ITEM WHERE TRIM(ICODE) LIKE '7%' ORDER BY ICODE ";
                SQuery = "SELECT TRIM(A.ICODE) AS FSTR,B.INAME AS PRODUCT,A.ICODE AS ERPCODE,A.KCLREELNO,A.COREELNO,B.OPRATE1,B.OPRATE3,B.UNIT,a.irate FROM REELVCH A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) and trim(a.icode)||trim(a.kclreelno) not in (" + col1 + ") ";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek("Select Item", frm_qstr);
                break;
            case "SG2_ROW_JOB":
                hffield.Value = "SG2_ROW_JOB";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

                SQuery = "Select distinct a.vchnum||a.vchdate as fstr,trim(a.Vchnum) as Job_no,to_Char(a.vchdate,'dd/mm/yyyy') as job_Dt,a.type,b.iname as item_name from costestimate a,item b where trim(a.icode)=trim(b.icodE) and a.branchcd='" + frm_mbr + "' and a.type='30' and a.vchdate " + DateRange + " order by trim(a.vchnum)  ";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                fgen.Fn_open_sseek("Select Job No", frm_qstr);
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
        fgen.Fn_open_sseek("Select Vendor ", frm_qstr);
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
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_16";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_17";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_18";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
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
    void save_ivch()
    {
        //string curr_dt;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[19].Text.Length > 2)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = lbl1a.Text.Substring(0, 2);
                oporow["vchnum"] = frm_vnum.Trim();
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["genum"] = txtlbl2.Text.Trim();
                oporow["gedate"] = txtlbl3.Text.Trim();


                oporow["prnum"] = txtlbl5.Text.Trim();
                oporow["exc_time"] = fgen.make_def_Date(txtlbl6.Text.Trim(), vardate);

                oporow["pname"] = txtlbl8.Text.Trim();
                oporow["o_Deptt"] = txtlbl9.Text.Trim();

                oporow["acode"] = txtlbl4.Text.Trim();
                oporow["vcode"] = txtlbl4.Text.Trim();
                oporow["REC_ISS"] = "C";
                oporow["store"] = "Y";
                oporow["inspected"] = "Y";
                if (Prg_Id == "FB1057")
                {
                    oporow["acode"] = "-";
                    oporow["vcode"] = "-";
                    oporow["REC_ISS"] = "-";
                    oporow["store"] = "-";
                    oporow["inspected"] = "-";
                }
                oporow["gst_pos"] = txtlbl70.Text.Trim();

                oporow["st_entform"] = txtlbl15.Text.Trim();
                oporow["thru"] = txtlbl16.Text.Trim();
                oporow["location"] = txtlbl17.Text.Trim();
                oporow["freight"] = txtlbl18.Text.Trim();

                if (txtlbl7.Text.Trim().Length > 2)
                {
                    oporow["vcode"] = txtlbl7.Text.Trim();
                }

                oporow["morder"] = i + 1;
                oporow["icode"] = sg1.Rows[i].Cells[19].Text.Trim();

                oporow["binno"] = txtlbl24.Text;
                oporow["mtime"] = txtlbl26.Text;
                oporow["iqty_chlwt"] = fgen.make_double(txtlbl28.Text);
                oporow["no_Cases"] = txtlbl30.Text;
                oporow["iqtyout"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim());
                oporow["iqty_Wt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim());
                oporow["no_bdls"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                oporow["desc_"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                oporow["irate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim());
                oporow["iamount"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim());

                oporow["exc_Rate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim());
                oporow["cess_percent"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim());

                oporow["stage"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text.Trim().Split('~')[0];
                oporow["iopr"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text.Trim().Split('~')[0];
                oporow["stagef"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text.Trim().Split('~')[0];

                oporow["exc_amt"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim());
                oporow["cess_pu"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text.Trim());


                oporow["revis_no"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                oporow["btchno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                oporow["btchdt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();

                //oporow["mfgdt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                //oporow["expdt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();

                oporow["ponum"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim(); ;
                oporow["ordlineno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                string po_dts;
                po_dts = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim(), vardate);

                oporow["podate"] = po_dts;
                if (lbl27.Text.Substring(0, 2) == "IG")
                {
                    oporow["POST"] = 2;
                }
                else
                {
                    oporow["POST"] = 1;
                }
                //oporow["iopr"] = lbl27.Text.Substring(0, 2);
              

                oporow["rej_sdv"] = 0;
                oporow["IQTYIN"] = 0;
                oporow["REJ_RW"] = 0;
                oporow["ACPT_UD"] = 0;

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
                oDS.Tables[0].Rows.Add(oporow);
            }
        }
    }



    void save_reelv()
    {
        for (i = 0; i < sg2.Rows.Count - 0; i++)
        {
            if (sg2.Rows[i].Cells[3].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg2.Rows[i].FindControl("sg2_t4")).Text.Trim()) > 1)
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
                oporow3["IRATE"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t5")).Text.Trim();
                oporow3["JOB_NO"] = "";
                oporow3["REELSPEC1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t7")).Text.Trim();
                oporow3["REELSPEC2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t8")).Text.Trim();
                oporow3["PSIZE"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                oporow3["GSM"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t3")).Text.Trim();
                oporow3["ACODE"] = txtlbl4.Text.Trim();
                oporow3["GRADE"] = "-";
                oporow3["REC_ISS"] = "C";
                oporow3["REELHIN"] = 0;
                oporow3["UNLINK"] = "N";
                oporow3["POSTED"] = "Y";
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
    void save_rgp()
    {
        //string curr_dt;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (i = 0; i < sg3.Rows.Count - 0; i++)
        {
            if (sg3.Rows[i].Cells[3].Text.Length > 2)
            {
                oporow4 = oDS4.Tables[0].NewRow();
                oporow4["BRANCHCD"] = frm_mbr;
                oporow4["TYPE"] = lbl1a.Text.Substring(0, 2);
                oporow4["vchnum"] = frm_vnum.Trim();
                oporow4["vchdate"] = txtvchdate.Text.Trim();
                oporow4["acode"] = txtlbl4.Text.Trim();
                if (Prg_Id == "FB1057")
                {
                    oporow["acode"] = "-";
                }

                //oporow4["vcode"] = txtlbl4.Text.Trim();

                oporow4["segment_"] = i + 1;
                oporow4["icode"] = sg3.Rows[i].Cells[3].Text.Trim();

                oporow4["iqtyout"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim());
                oporow4["iqty_Wt"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim());
                oporow4["no_bdls"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                oporow4["desc_"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();
                oporow4["irate"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t5")).Text.Trim());
                oporow4["iamount"] = fgen.make_double(((TextBox)sg3.Rows[i].FindControl("sg3_t6")).Text.Trim());

                //oporow4["btchno"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t10")).Text.Trim();
                //oporow["btchdt"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t11")).Text.Trim();

                //oporow["mfgdt"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t12")).Text.Trim();
                //oporow["expdt"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t13")).Text.Trim();

                //oporow["ponum"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t14")).Text.Trim(); ;
                //oporow["ordlineno"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t15")).Text.Trim();
                //string po_dts;
                //po_dts = fgen.make_def_Date(((TextBox)sg3.Rows[i].FindControl("sg3_t16")).Text.Trim(), vardate);

                //oporow["podate"] = po_dts;

                oporow4["iopr"] = lbl27.Text.Substring(0, 2);

                oporow4["REC_ISS"] = "C";
                oporow4["store"] = "N";
                //oporow4["inspected"] = "Y";
                oporow4["pname"] = "-";
                oporow4["rej_sdv"] = 0;
                oporow4["IQTYIN"] = 0;
                oporow4["REJ_RW"] = 0;
                oporow4["ACPT_UD"] = 0;

                if (edmode.Value == "Y")
                {


                    oporow4["eNt_by"] = ViewState["entby"].ToString();
                    oporow4["eNt_dt"] = ViewState["entdt"].ToString();
                    oporow4["edt_by"] = frm_uname;
                    oporow4["edt_dt"] = vardate;

                }
                else
                {


                    oporow4["eNt_by"] = frm_uname;
                    oporow4["eNt_dt"] = vardate;
                    oporow4["edt_by"] = "-";
                    oporow4["eDt_dt"] = vardate;

                }
                oporow4["naration"] = txtrmk.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oDS4.Tables[0].Rows.Add(oporow4);
            }
        }

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
                oporow5["par_tbl"] = frm_tabname.ToUpper().Trim();
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

        SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE FROM type where id='M' and type1 like '2%'  order by type1";
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
    }
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int z = 3; z <= 7; z++)
            {
                sg2.HeaderRow.Cells[z].Style["display"] = "none";
                e.Row.Cells[z].Style["display"] = "none";
            }

            for (int z = 10; z <= 12; z++)
            {
                sg2.HeaderRow.Cells[z].Style["display"] = "none";
                e.Row.Cells[z].Style["display"] = "none";
            }
        }
    }
    protected void btnPost_ServerClick(object sender, EventArgs e)
    {
        dt = new DataTable();
        sg2_dt = new DataTable();
        create_tab2();
        sg2_dr = null;
        for (i = 0; i < sg2.Rows.Count - 1; i++)
        {
            sg2_dr = sg2_dt.NewRow();
            sg2_dr["sg2_srno"] = (i + 1);
            sg2_dr["sg2_h1"] = sg2.Rows[i].Cells[0].Text.Trim();
            sg2_dr["sg2_h2"] = sg2.Rows[i].Cells[1].Text.Trim();
            sg2_dr["sg2_h3"] = sg2.Rows[i].Cells[2].Text.Trim();
            sg2_dr["sg2_h4"] = sg2.Rows[i].Cells[3].Text.Trim();
            sg2_dr["sg2_h5"] = sg2.Rows[i].Cells[4].Text.Trim();

            sg2_dr["sg2_f1"] = sg2.Rows[i].Cells[3].Text.Trim();
            sg2_dr["sg2_f2"] = sg2.Rows[i].Cells[4].Text.Trim();
            sg2_dr["sg2_f3"] = sg2.Rows[i].Cells[5].Text.Trim();
            sg2_dr["sg2_f4"] = sg2.Rows[i].Cells[6].Text.Trim();
            sg2_dr["sg2_f5"] = sg2.Rows[i].Cells[7].Text.Trim();

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
        if (sg2_dt.Rows.Count <= 0) return;
        #region for gridview 1
        if (ViewState["sg1"] != null)
        {
            dt = new DataTable();
            sg1_dt = new DataTable();
            create_tab();
            ViewState["sg1"] = sg1_dt;
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
            DataView dv = new DataView(sg2_dt);
            dt = dv.ToTable(true, "sg2_F1");
            for (int d = 0; d < dt.Rows.Count; d++)
            {
                if (dt.Rows[d]["sg2_F1"].ToString().Length > 1)
                {
                    sg1_dr = sg1_dt.NewRow();
                    sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                    sg1_dr["sg1_h1"] = fgen.seek_iname_dt(sg2_dt, "sg2_F1='" + dt.Rows[d][0].ToString().Trim() + "'", "sg2_F1");
                    sg1_dr["sg1_h2"] = fgen.seek_iname_dt(sg2_dt, "sg2_F1='" + dt.Rows[d][0].ToString().Trim() + "'", "sg2_F2");
                    sg1_dr["sg1_h3"] = "-";
                    sg1_dr["sg1_h4"] = "-";
                    sg1_dr["sg1_h5"] = "-";
                    sg1_dr["sg1_h6"] = "-";
                    sg1_dr["sg1_h7"] = "-";
                    sg1_dr["sg1_h8"] = "-";
                    sg1_dr["sg1_h9"] = "-";
                    sg1_dr["sg1_h10"] = "-";

                    sg1_dr["sg1_f1"] = fgen.seek_iname_dt(sg2_dt, "sg2_F1='" + dt.Rows[d][0].ToString().Trim() + "'", "sg2_F1");
                    sg1_dr["sg1_f2"] = fgen.seek_iname_dt(sg2_dt, "sg2_F1='" + dt.Rows[d][0].ToString().Trim() + "'", "sg2_F2");
                    sg1_dr["sg1_f3"] = fgen.seek_iname_dt(sg2_dt, "sg2_F1='" + dt.Rows[d][0].ToString().Trim() + "'", "sg2_F3");
                    sg1_dr["sg1_f4"] = "-";
                    sg1_dr["sg1_f5"] = "-";

                    sg1_dr["sg1_t1"] = "0";
                    double dval = 0;
                    foreach (DataRow sgdr2 in sg2_dt.Rows)
                    {
                        if (sgdr2["sg2_f1"].ToString().Trim().Length > 2 && sgdr2["sg2_f1"].ToString().Trim() == dt.Rows[d][0].ToString().Trim())
                        {
                            dval += fgen.make_double(sgdr2["sg2_t4"].ToString().Trim());
                        }
                    }
                    sg1_dr["sg1_t1"] = dval.ToString();
                    sg1_dr["sg1_t3"] = "-";
                    sg1_dr["sg1_t4"] = "-";
                    sg1_dr["sg1_t5"] = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select (Case when nvl(iqd,0)=0 then irate else iqd end) as irate from item where trim(upper(icode))=Trim('" + sg1_dr["sg1_f1"] + "')", "irate"));

                    sg1_dr["sg1_t9"] = "";
                    sg1_dr["sg1_t10"] = "-";
                    sg1_dr["sg1_t11"] = "-";
                    sg1_dr["sg1_t12"] = "-";
                    sg1_dr["sg1_t13"] = "-";

                    sg1_dr["sg1_t14"] = "-";
                    sg1_dr["sg1_t15"] = "";
                    sg1_dr["sg1_t16"] = "-";

                    sg1_dt.Rows.Add(sg1_dr);
                }
            }
        }
        //sg1_add_blankrows();

        ViewState["sg1"] = sg1_dt;
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        sg1_dt.Dispose();
        //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
        #endregion
        setColHeadings();
        setGST();
    }
    protected void btnRead_ServerClick(object sender, EventArgs e)
    {
        dt = new DataTable();
        sg1_dt = new DataTable();
        if (frm_cocd == "BONY" || frm_cocd == "SRPF")
        {
            if (txtBarCode.Value.Trim().Length < 21) return;
            string str = txtBarCode.Value.Trim();
            if (str.Contains("\r")) str = str.Replace("\r", "$");
            if (str.Contains("\n")) str = str.Replace("\n", "$");
            if (str.Contains("$$")) str = str.Replace("$$", "$");
            string[] sp = str.Split('$');
            col1 = "";
            foreach (string s in sp)
            {
                if (s.Length > 1)
                {
                    if (col1.Length > 0) col1 = col1 + "," + "'" + s.ToString() + "'";
                    else col1 = "'" + s.ToString() + "'";
                }
            }
            if (col1.Length < 2) return;
            dt2 = new DataTable();
            dt2 = fgen.getdata(frm_qstr, frm_cocd, "Select a.*,b.iname,b.cpartno,b.unit,b.cdrgno from finprim.scratch a,finprim.item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||a.vchnum||to_Char(a.Vchdate,'dd/mm/yyyy')='" + col1 + "'");
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
                        dt3 = fgen.getdata(frm_qstr, frm_cocd, "select a.icode,a.iname,a.cpartno,a.cdrgno,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7 from item a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.icode) ='" + dt2.Rows[d]["col3"].ToString().Trim() + "' ");
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

                        if (dt3.Rows.Count > 0)
                        {
                            sg1_dr["sg1_f2"] = dt3.Rows[0]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt3.Rows[0]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt3.Rows[0]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt3.Rows[0]["unit"].ToString().Trim();

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
                        }

                        sg1_dr["sg1_t1"] = dt2.Rows[d]["col4"].ToString().Trim();
                        sg1_dr["sg1_t2"] = dt2.Rows[d]["col4"].ToString().Trim();
                        sg1_dr["sg1_t3"] = 0;
                        sg1_dr["sg1_t4"] = 0;

                        sg1_dr["sg1_t9"] = "";
                        sg1_dr["sg1_t10"] = dt2.Rows[d]["col2"].ToString().Trim(); ;
                        //sg1_dr["sg1_t11"] = dt.Rows[d]["iexc_Addl"].ToString().Trim();
                        //sg1_dr["sg1_t12"] = dt.Rows[d]["frt_pu"].ToString().Trim();
                        //sg1_dr["sg1_t13"] = dt.Rows[d]["pkchg_pu"].ToString().Trim();

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
    protected void btnRcv_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "BTNRCV";
        fgen.msg("-", "CMSG", "Are You Sure, You Want Copy from Sent Matl!!");
    }
    void rcvMatl()
    {
        create_tab3();
        dt = new DataTable();
        sg3_dr = null;
        i = 1;

        foreach (GridViewRow gr1 in sg1.Rows)
        {
            if (gr1.Cells[13].Text.Length > 1)
            {
                sg3_dr = sg3_dt.NewRow();
                sg3_dr["sg3_srno"] = i;
                sg3_dr["sg3_f1"] = gr1.Cells[13].Text;
                sg3_dr["sg3_f2"] = gr1.Cells[14].Text;
                sg3_dr["sg3_f3"] = gr1.Cells[15].Text;
                sg3_dr["sg3_f4"] = gr1.Cells[16].Text;
                sg3_dr["sg3_f5"] = gr1.Cells[17].Text;

                sg3_dr["sg3_t1"] = ((TextBox)gr1.FindControl("sg1_t1")).Text;
                sg3_dr["sg3_t2"] = ((TextBox)gr1.FindControl("sg1_t2")).Text;
                sg3_dr["sg3_t3"] = ((TextBox)gr1.FindControl("sg1_t3")).Text;
                sg3_dr["sg3_t4"] = ((TextBox)gr1.FindControl("sg1_t4")).Text;
                sg3_dr["sg3_t5"] = ((TextBox)gr1.FindControl("sg1_t5")).Text;

                sg3_dr["sg3_t6"] = ((TextBox)gr1.FindControl("sg1_t6")).Text;
                sg3_dr["sg3_t7"] = ((TextBox)gr1.FindControl("sg1_t7")).Text;
                sg3_dr["sg3_t8"] = ((TextBox)gr1.FindControl("sg1_t8")).Text;
                sg3_dr["sg3_t9"] = ((TextBox)gr1.FindControl("sg1_t9")).Text;
                sg3_dr["sg3_t10"] = ((TextBox)gr1.FindControl("sg1_t10")).Text;

                sg3_dt.Rows.Add(sg3_dr);

                i++;
            }
        }
        sg3_add_blankrows();
        ViewState["sg3"] = sg3_dt;
        sg3.DataSource = sg3_dt;
        sg3.DataBind();
    }
}