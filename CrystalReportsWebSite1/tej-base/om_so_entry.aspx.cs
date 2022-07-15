using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Data.OleDb;

public partial class om_so_entry : System.Web.UI.Page
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
    string mhd = "";
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond = "";
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_famtbl = "", frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    string rateDiscount; string curr_itm;
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
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                string chk_opt = "";
                string chk_opt_yn = "";

                doc_addl.Value = "-";
                doc_GST.Value = "Y";
                doc_bom.Value = "N";
                doc_hosopw.Value = "N";
                SQuery = "select opt_id,trim(upper(OPT_ENABLE)) as OPT_ENABLE from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID in ('W1100','W2017','W2018','W2020','W2027') order by OPT_ID";
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
                            case "W1100":
                                //branch wise HO SO system
                                if (chk_opt_yn == "Y") { doc_hosopw.Value = "Y"; }
                                break;

                            case "W2017":
                                //INDIA GST
                                if (chk_opt_yn == "N") { doc_GST.Value = "N"; }
                                break;
                            case "W2018":
                                //all seris in sales order
                                if (chk_opt_yn == "Y") { doc_addl.Value = "Y"; }
                                break;
                            case "W2020":
                                //bom compulsary for so
                                if (chk_opt_yn == "Y") { doc_bom.Value = "Y"; }
                                break;
                            case "W2027":
                                //Member GCC Country
                                if (chk_opt_yn == "Y") { doc_GST.Value = "GCC"; }
                                break;
                        }
                    }
                }
                dt.Dispose();

                doc_hoso.Value = "N";
                doc_fview.Value = "N";
                SQuery = "select opt_id,trim(upper(OPT_ENABLE)) as OPT_ENABLE from FIN_RSYS_OPT where OPT_ID in ('W0052','W0063') order by OPT_ID";
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
                            case "W0052":
                                //HO Based SO 
                                if (chk_opt_yn == "Y") { doc_hoso.Value = "Y"; }
                                break;
                            case "W0063":
                                //Fam + Crm mst View
                                if (chk_opt_yn == "Y" && Prg_Id == "F45109") { doc_fview.Value = "Y"; }
                                break;
                        }
                    }
                }
                dt.Dispose();




                //---------------------
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
                    //sg1.HeaderRow.Cells[i].Style["display"] = "none";
                    //sg1.Rows[K].Cells[i].Style["display"] = "none";
                    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[K].Cells[i].CssClass = "hidden";
                }
                #endregion
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

                ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t4")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t5")).Attributes.Add("autocomplete", "off");


                txtlbl70.Attributes.Add("readonly", "readonly");
                txtlbl71.Attributes.Add("readonly", "readonly");
                txtlbl72.Attributes.Add("readonly", "readonly");
                txtlbl8.Attributes.Add("readonly", "readonly");
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
            case "F45109":
            case "F47101":
                tab2.Visible = false;
                tab3.Visible = false;
                tab4.Visible = false;
                tab5.Visible = false;
                tab6.Visible = false;

                btnImport.Visible = false;
                btnPI.Visible = false;
                break;
            case "F49106":
            case "F47106":
                btnImport.Visible = false;
                tab2.Visible = false;
                if (frm_cocd == "SAIA")
                {
                    btnImport.Visible = true;
                }
                btnPI.Visible = false;
                if (frm_cocd == "SAIA" || frm_cocd == "MULT")
                {
                    btnPI.Visible = true;
                }

                //tab3.Visible = false;
                //tab4.Visible = false;
                tab5.Visible = false;
                break;

            case "F50116":
                tab2.Visible = false;
                //tab3.Visible = false;
                //tab4.Visible = false;
                tab5.Visible = false;
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

        btnImport.Disabled = true;
        btnPI.Disabled = true;

        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        sg2.DataSource = sg2_dt; sg2.DataBind();
        if (sg2.Rows.Count > 0) sg2.Rows[0].Visible = false; sg2_dt.Dispose();
        sg3.DataSource = sg3_dt; sg3.DataBind();
        if (sg3.Rows.Count > 0) sg3.Rows[0].Visible = false; sg3_dt.Dispose();
        sg4.DataSource = sg4_dt; sg4.DataBind();
        if (sg4.Rows.Count > 0) sg4.Rows[0].Visible = false; sg4_dt.Dispose();
        btnprint.Disabled = false; btnlist.Disabled = false;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlbl4.Enabled = true;
        btnlbl7.Enabled = true;
        btnprint.Disabled = true; btnlist.Disabled = true;

        btnImport.Disabled = false;
        btnPI.Disabled = false;
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
        doc_nf.Value = "ordno";
        doc_df.Value = "orddt";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {

            case "F47101":
                frm_tabname = "SOMASM";
                btnWO.Visible = false;
                btnWO.Visible = false;
                break;
            case "F47106":
                frm_tabname = "SOMAS";
                break;
            case "F45109":
                frm_tabname = "SOMASQ";
                btnWO.Visible = false;
                break;
            default:
                frm_tabname = "SOMAS";
                break;
        }
        frm_famtbl = "FAMST";
        if (doc_fview.Value == "Y" && Prg_Id == "F45109")
        {
            frm_famtbl = "wbvu_fam_crm";
        }
        if (frm_ulvl == "M")
        {
            btnlbl4.Visible = false;
            txtlbl4.Text = frm_UserID;
            txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM " + frm_famtbl + " WHERE UPPER(tRIM(aCODE))='" + frm_UserID + "'", "ANAME");

            if (txtlbl72.Text.Trim().Length < 2)
                setAcodSelection();
        }

        if (frm_ulvl == "M")
        {
            btnlbl18.Visible = false;
            txtlbl17.Enabled = false;
            btnlbl17.Visible = false;
        }
        if (txtlbl72.Text.ToString().Trim().Length < 3)
            txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");

        btnlbl17.Visible = false;

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
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='A'   order by type1";
                break;
            case "BTN_16":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='<' and substr(type1,1,1)='0'  order by type1";
                break;
            case "BTN_17":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  FROM TYPE WHERE ID='G' and substr(type1,1,1)='5' order by type1";
                break;
            case "BTN_18":
                SQuery = "SELECT ACODE AS FSTR,Acode,replacE(ANAME,'''','`') AS Account,Addr1,Addr2 FROM " + frm_famtbl + " where substr(acode,1,2) in ('03','12') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;

            case "BTN_19":
                SQuery = "SELECT '10' as fstr,'As Applicable' as NAME,'10' as Code FROM dual ";
                break;

            case "BTN_20":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM " + frm_famtbl + " where substr(acode,1,2) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_21":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM " + frm_famtbl + " where substr(acode,1,2) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_22":
                SQuery = "SELECT ACODE AS FSTR,replacE(ANAME,'''','`') AS Account,ACODE AS CODE FROM " + frm_famtbl + " where substr(acode,1,2) in ('21') and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY aname ";
                break;
            case "BTN_23":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1,rate  FROM TYPE WHERE ID='A' order by name ";
                break;
            case "TACODE":
                //pop1
                string acodeCond = "trim(GRP) in ('02','16') and ";
                SQuery = "SELECT ACODE AS FSTR,ANAME AS PARTY,ACODE AS CODE,ADDR1,ADDR2,staten as state,Pay_num FROM " + frm_famtbl + " where " + acodeCond + " length(Trim(nvl(deac_by,'-')))<=1 and length(Trim(nvl(STATEN,'-')))>1 and (case when length(nvl(showinbr,'-'))>1 then instr(nvl(showinbr,'-'),'" + frm_mbr + "') else 1 end)>0  ORDER BY aname ";

                if (frm_cocd == "SAIA")
                {
                    if (frm_ulvl != "0")
                    {
                        SQuery = "SELECT ACODE AS FSTR,ANAME AS PARTY,ACODE AS CODE,ADDR1,ADDR2,ADDR3,staten as state,Pay_num FROM " + frm_famtbl + " where substr(acode,1,2)in ('16') and length(Trim(nvl(deac_by,'-')))<=1 and length(Trim(nvl(STATEN,'-')))>1 and trim(upper(STATEN))='" + txtlbl72.Text.Trim().ToUpper() + "' ORDER BY aname ";
                        if (frm_vty == "4B")
                            SQuery = "SELECT ACODE AS FSTR,ANAME AS PARTY,ACODE AS CODE,ADDR1,ADDR2,ADDR3,staten as state,Pay_num FROM " + frm_famtbl + " where substr(acode,1,2)in ('02') and length(Trim(nvl(deac_by,'-')))<=1 and length(Trim(nvl(STATEN,'-')))>1  ORDER BY aname ";
                    }
                }
                break;
            case "PICK_SAGI":
                SQuery = "SELECT DISTINCT A.ordno||to_char(A.orddt,'dd/mm/yyyy')||trim(a.Acode) as Fstr,A.Ordno AS SO_Number,to_Char(a.orddt,'dd/mm/yyyy') as SO_Date,B.Aname as Customer,a.Pordno,to_Char(a.orddt,'yyyymmdd') as VDD  FROM somas a, " + frm_famtbl + " b  WHERE TRIM(A.ACODE)=TRIM(B.ACODE)  and a.BRANCHCD='00' and a.TYPE='4C' and substr(a.acode,1,2) ='02' AND (a.ordno||to_char(a.orddt,'yyyymm')) IN (SELECT VCHNUM FROM (SELECT X.VCHNUM,SUM(X.aBC) AS CNT FROM (select distinct a.ordno||to_char(a.orddt,'yyyymm') as vchnum,a.type,1 AS ABC from finsagi.somas a  where branchcd='00' AND a.type='4C' UNION ALL select distinct nvl(a.invno,'-')||to_char(a.invdate,'yyyymm') as genum,a.type,1 AS ABC from somas a where branchcd='00' and a.type='4F' and substr(a.Acode,1,2)='02' ) X GROUP BY X.VCHNUM) WHERE CNT=1) ";
                break;
            case "TICODE":
                //pop2
                cond = frm_ulvl == "M" ? " AND TCNUM='" + frm_uname + "'" : "";
                SQuery = "SELECT ACODE AS FSTR,ANAME AS PARTY,ACODE AS CODE,ADDR1,ADDR2 FROM CSMST where branchcd!='DD' " + cond + " ORDER BY aname ";
                break;
            case "TICODEX":
                SQuery = "Select Type1,Name,Type1 as Code from type where id='@' and substr(type1,1,1)<'2' order by name";
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
                if (doc_addl.Value == "Y")
                {
                    SQuery = "SELECT Icode,Iname as product,icode as erpcode,Maker,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,hscode,irate as MRP,ciname,madeinbr FROM Item WHERE length(Trim(icode))>4 and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 and length(Trim(nvl(hscode,'-')))>1 and (case when length(nvl(showinbr,'-'))>1 then instr(nvl(showinbr,'-'),'" + frm_mbr + "') else 1 end)>0 ORDER BY Iname  ";
                }
                else
                {
                    SQuery = "SELECT Icode,Iname as product,icode as erpcode,Maker,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,hscode,irate as MRP,ciname,madeinbr FROM Item WHERE length(Trim(icode))>4 and icode like '9%' and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 and length(Trim(nvl(hscode,'-')))>1 and (case when length(nvl(showinbr,'-'))>1 then instr(nvl(showinbr,'-'),'" + frm_mbr + "') else 1 end)>0 ORDER BY Iname  ";
                }

                if (doc_bom.Value == "Y")
                {
                    SQuery = "SELECT distinct a.Icode,a.Iname as product,a.icode as erpcode,a.Maker,a.Cpartno AS Part_no,a.Cdrgno AS Drg_no,a.Unit,a.hscode,a.irate as MRP,a.ciname,a.madeinbr FROM Item a,itemosp b WHERE trim(A.icodE)=trim(b.icode) and a.icode like '9%' and trim(a.icode) not in (" + col1 + ") and length(Trim(nvl(a.deac_by,'-')))<=1 and length(Trim(nvl(a.hscode,'-')))>1 and (case when length(nvl(showinbr,'-'))>1 then instr(nvl(showinbr,'-'),'" + frm_mbr + "') else 1 end)>0 ORDER BY a.Iname  ";
                }

                if (frm_ulvl == "M" || doc_somasm.Value == "Y")
                    SQuery = "SELECT DISTINCT A.ICODE,B.INAME AS PRODUCT,A.ICODE AS ERPCODE,B.Maker,B.CPARTNO AS PART_NO,B.CDRGNO AS DRG_NO,B.UNIT,B.HSCODE,b.ciname,b.madeinbr FROM SOMASM A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '4%' AND TRIM(A.ACODE)='" + txtlbl4.Text.Trim() + "' and trim(A.icode) not in (" + col1 + ") and (case when length(nvl(showinbr,'-'))>1 then instr(nvl(showinbr,'-'),'" + frm_mbr + "') else 1 end)>0 ORDER BY A.ICODE ";
                break;
            case "SG1_ROW_QU":
                curr_itm = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CURR_ITEM");
                cond = " and trim(a.acode)='" + txtlbl4.Text.Trim().ToUpper() + "' AND TRIM(A.ICODE)='" + curr_itm + "'";
                SQuery = "select distinct trim(A." + doc_nf.Value + ")||'-'||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Doc_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Doc_Dt,b.Aname as Customer,c.iname product,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.App_by,(Case when a.icat='Y' then 'Closed' else 'Active' end) as SO_Status,to_char(a.App_Dt,'dd/mm/yyyy') as App_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + "SOMASQ" + "" + (btnval == "PI_E" ? "Q" : "") + " a," + frm_famtbl + " b,ITEM C where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " and  trim(a.acode)=trim(B.acodE) AND TRIM(A.ICODE)=TRIM(C.ICODE) " + cond + " order by vdd desc,a." + doc_nf.Value + " desc";
                if (Prg_Id == "F45109")
                    SQuery = "select trim(A.LRCNO)||'-'||to_Char(a.LRCDT,'dd/mm/yyyy') as fstr,a.LRCNO as lead_no,to_char(a.LRCdt,'dd/mm/yyyy') as lead_Dt,a.Ldescr as company,a.Lvertical,a.cont_name as person,a.cont_no as contact_no ,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt from " + "WB_LEAD_LOG" + " a where a.branchcd='" + frm_mbr + "' and a.type='LR' AND a.LRCDT " + DateRange + " order by a.LRCDT desc,a.LRCNO desc";
                break;
            case "SG1_ROW_TAX":

                SQuery = "Select Type1 as fstr,Name,Type1 as Code,nvl(Rate,0) as Rate,nvl(Excrate,0) as Schg,exc_Addr as Ref_Code from type where id='S' and length(Trim(nvl(cstno,'-')))<=1 order by name";
                break;
            case "SG1_ROW_DT":
                curr_itm = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CURR_ITEM");

                if (Prg_Id == "F45109" && doc_fview.Value == "Y")
                {
                    SQuery = "select distinct a.branchcd||'-'||trim(A.lrcno)||'-'||to_Char(a.lrcdt,'dd/mm/yyyy') as fstr,a.lrcno as Lead_No,to_char(a.lrcdt,'dd/mm/yyyy') as Lead_Dt,a.Ldescr as company,a.Lvertical,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a.lrcdt,'yyyymmdd') as vdd from wb_lead_log a where  a.branchcd='" + frm_mbr + "' and a.type='LR' and trim(nvl(a.app_by,'-'))!='-' order by vdd desc,a.lrcno desc";
                }
                else
                {
                    SQuery = "SELECT a.Icode,c.Aname as Customer,a.icode as erpcode,a.irate,b.Cpartno AS Part_no,b.Cdrgno AS Drg_no,to_char(a.orddt,'dd/mm/yyyy') as Ord_Dt,a.ordno,to_char(a.orddt,'yyyymmdd') as VDD FROM " + frm_tabname + "  a, item b ," + frm_famtbl + " c WHERE trim(A.Acode)=trim(c.acode) and trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.icat<>'Y' and trim(a.icode) = '" + curr_itm + "' and length(Trim(nvl(b.deac_by,'-')))<=1 and length(Trim(nvl(b.hscode,'-')))>1  ORDER BY to_char(a.orddt,'yyyymmdd') desc  ";
                }
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
            case "WO":
            case "PI":
                Type_Sel_query();
                break;
            case "PI_E":
                // from somasq
                SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Doc_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Doc_Dt,b.Aname as Customer,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.App_by,(Case when a.icat='Y' then 'Closed' else 'Active' end) as SO_Status,to_char(a.App_Dt,'dd/mm/yyyy') as App_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + "" + (btnval == "PI_E" ? "Q" : "") + " a," + frm_famtbl + " b where  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " and  trim(a.acode)=trim(B.acodE) " + cond + " and trim(a.ORDNO)||to_char(a.ORDDT,'dd/mm/yyyy') not in (SELECT trim(a.pordno)||to_char(a.porddt,'dd/mm/yyyy') AS FSTR FROM " + frm_tabname + " A WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "') order by vdd desc,a." + doc_nf.Value + " desc";
                // from salep
                SQuery = "select distinct trim(A.VCHNUM)||to_Char(a.VCHDATE,'dd/mm/yyyy') as fstr,a.VCHNUM as Doc_no,to_char(a.VCHDATE,'dd/mm/yyyy') as Doc_Dt,b.Aname as Customer,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a.VCHDATE,'yyyymmdd') as vdd from SALEP a," + frm_famtbl + " b where  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a.VCHDATE " + DateRange + " and  trim(a.acode)=trim(B.acodE) " + cond + " and trim(a.VCHNUM)||to_char(a.VCHDATE,'dd/mm/yyyy') not in (SELECT trim(a.pordno)||to_char(a.porddt,'dd/mm/yyyy') AS FSTR FROM " + frm_tabname + " A WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "') order by vdd desc,a.VCHNUM desc";
                break;
            case "SALESMAN":
                SQuery = "SELECT trim(Type1)||':'||trim(Name) as Fstr,Name,Type1 as Code,Acref as ASM_Name,Acref2 as RSM_Name from typegrp where branchcd!='DD' and id='EM' order by type1";
                break;
            default:
                cond = frm_ulvl == "M" ? " and trim(a.acode)='" + frm_uname + "'" : "";
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "WO_E" || btnval == "Atch_E")
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Doc_no," +
                        "to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Doc_Dt,b.Aname as Customer,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.App_by" +
                        ",a.pordno as Cust_PO,to_char(a.porddt,'dd/mm/yyyy') as Cust_PO_DT ," +
                        "(Case when a.icat='Y' then 'Closed' else 'Active' end) as SO_Status,to_char(a.App_Dt,'dd/mm/yyyy') as App_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd" +
                        "" +
                        " from " + frm_tabname + "" + (btnval == "PI_E" ? "Q" : "") + " a," + frm_famtbl + " b where  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " and  trim(a.acode)=trim(B.acodE) " + cond + " order by vdd desc,a." + doc_nf.Value + " desc";
                if (btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Doc_no," +
                        "to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Doc_Dt,b.Aname as Customer,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt" +
                        ",a.App_by,a.pordno as Cust_PO,to_char(a.porddt,'dd/mm/yyyy') as Cust_PO_DT ," +
                        "(Case when a.icat='Y' then 'Closed' else 'Active' end) as SO_Status,to_char(a.App_Dt,'dd/mm/yyyy') as App_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd" +
                        "  from " + frm_tabname + " a," + frm_famtbl + " b where  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and trim(a.icat)<>'Y' and  trim(a.acode)=trim(B.acodE) " + cond + " order by vdd desc,a." + doc_nf.Value + " desc";
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        if (frm_cocd == "SAIA")
        {
            if (frm_mbr.toDouble() > 50)
            {
                fgen.msg("-", "AMSG", "S.O. Creation Not Allowed in Depo Branch!!");
                return;
            }
        }

        if ((doc_hoso.Value == "Y" && frm_mbr != "00") || (doc_hosopw.Value == "Y" && frm_mbr != "00"))
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , As per System Settings, Sale Order Activity Done from HO(00) Only.");
            return;
        }

        string chk_curren = "";
        chk_curren = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT br_Curren FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "br_Curren");
        if (chk_curren.Length < 2)
        {
            fgen.msg("-", "AMSG", "Please update Currency in Branch Master!!");
            return;
        }

        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Type", frm_qstr);

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
        if ((doc_hoso.Value == "Y" && frm_mbr != "00") || (doc_hosopw.Value == "Y" && frm_mbr != "00"))
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , As per System Settings, Sale Order Activity Done from HO(00) Only..");
            return;
        }

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
        if (frm_cocd == "SAIA")
        {
            if (frm_mbr.toDouble() > 50)
            {
                fgen.msg("-", "AMSG", "S.O. Saving Not Allowed in Depo Branch!!");
                return;
            }
        }
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }

        string chk_freeze = "";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F47101":
                chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1062", txtvchdate.Text.Trim());
                break;
            case "F47106":
                chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1063", txtvchdate.Text.Trim());
                break;
            default:
                chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1063", txtvchdate.Text.Trim());
                break;
        }

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

        string chk_alent;

        if (txtlbl5.Text.Trim().ToUpper() != "NA" && txtlbl5.Text.Trim().ToUpper() != "N/A")
        {
            chk_alent = fgen.seek_iname(frm_qstr, frm_cocd, "select ordno||'-'||to_char(orddt,'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type like '4%' and ordno||to_char(orddt,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "' and trim(upper(icat))<>'Y' and trim(upper(acode))='" + txtlbl4.Text + "' and trim(upper(pordno))='" + txtlbl5.Text.ToUpper().Trim() + "' and orddt " + DateRange + "", "ldt");
            if (chk_alent == "0")
            { }
            else
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , This Customer's Order No. Already Entered in Doc No." + chk_alent + ",Please Check, Repeat Entry not Allowed !!");
                return;
            }
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
        if (txtlbl70.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl70.Text;
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
        if (txtlbl17.Text.Trim() == "" || txtlbl17.Text.Trim() == "-")
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl17.Text;

        }



        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }


        string delvdate = "";

        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) < 0)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");
                i = sg1.Rows.Count;
                return;

            }

            if (lbl1a.Text != "4S")
            {
                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text) <= 0)
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Rate Not Filled Correctly at Line " + (i + 1) + "  !!");
                    i = sg1.Rows.Count;
                    return;
                }
            }

            if (Prg_Id == "F47101")
            { }
            else
            {
                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Length < 10)
                {
                    if (delvdate != "")
                    {
                        ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text = delvdate;
                    }
                    else
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Date of Delivery Not Filled Correctly at Line " + (i + 1) + "  !!");
                        i = sg1.Rows.Count;
                        return;
                    }
                }
                delvdate = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text;
                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2)
                {
                    if (Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text) < Convert.ToDateTime(txtvchdate.Text))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Date of Delivery Can Not less then Order Date at Line " + (i + 1) + "  !!");
                        i = sg1.Rows.Count;
                        return;
                    }
                }
            }

            if (Prg_Id == "F47106" && (frm_cocd == "SAGM" || doc_hosopw.Value == "Y"))
            {
                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim().Length < 2)
                {
                    Checked_ok = "N";

                    fgen.msg("-", "AMSG", "Dear " + frm_uname + "Please check Item Master '13' Manufacturing Locn Not Filled Correctly at Line " + (i + 1) + "  !!");
                    i = sg1.Rows.Count;
                    return;

                }
            }


        }

        string last_entdt;
        //checks
        if (edmode.Value == "Y")
        {
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  and orddt " + DateRange + " and ordno||to_char(orddt,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "' and orddt<=to_DaTE('" + txtvchdate.Text + "','dd/mm/yyyy') order by orddt desc", "ldt");
        }
        else
        {
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  and orddt " + DateRange + " and ordno||to_char(orddt,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "' order by orddt desc", "ldt");
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
        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        for (int l = 0; l < sg3.Rows.Count - 1; l++)
        {
            if (sg3.Rows[l].Cells.ToString().Length > 4)
            {
                dhd = fgen.ChkDate(((TextBox)sg3.Rows[l].FindControl("sg3_t1")).Text.Trim());
                if (dhd == 0)
                {
                    fgen.msg("-", "AMSG", "Please Select a Valid Date in Schedule Grid!!");
                    txtvchdate.Focus();
                    return;
                }
            }
        }

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

        btnval = hffield.Value;
        //--
        string mv_col;
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from poterm a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from budgmst a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data a where par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

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
                    if (frm_cocd == "MULT") cond = "like '4%'";
                    else cond = "='" + frm_vty + "'";
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE " + cond + " AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);


                    disablectrl();
                    fgen.EnableForm(this.Controls);
                    btnlbl4.Focus();


                    if (lbl1a.Text != "4F" && lbl1a.Text != "4S")
                    {
                        btnlbl15.Visible = false;

                        txtlbl15.Enabled = false;
                        txtlbl24.Enabled = false;
                    }

                    txtlbl15.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT br_Curren FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "br_Curren");
                    if (txtlbl15.Text.Length < 2 && doc_GST.Value != "GCC")
                    {
                        txtlbl15.Text = "INR";
                    }

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
                    if (frm_cocd == "SAIA")
                    {
                        SQuery = "Select Type1,Name,Type1 as Code from type where id='@' and substr(type1,1,1)<'2' and type1='01' order by name";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count > 0)
                        {
                            txtlbl70.Text = dt.Rows[0][0].ToString();
                            txtlbl71.Text = dt.Rows[0][1].ToString();
                        }
                    }
                    //-------------------------------------------
                    // Popup asking for Copy from Older Data
                    if (frm_cocd != "SAIA")
                    {
                        fgen.msg("-", "CMSG", "Do You Want to Copy From Existing " + lblheader.Text + "'13'(No for make it new)");
                        hffield.Value = "NEW_E";
                    }
                    break;
                    #endregion
                case "COPY_OLD":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();


                    mv_col = frm_mbr + frm_vty + col1;

                    SQuery = "Select a.*,to_Char(nvl(a.invdate,a.orddt),'dd/mm/yyyy') As invdated,to_Char(nvl(a.cu_chldt,a.orddt),'dd/mm/yyyy') As cu_chldt1,to_Char(nvl(a.porddt,a.orddt),'dd/mm/yyyy') As porddt1,b.iname,c.Aname,nvl(b.cpartno,'-') As Icpartno,nvl(b.cdrgno,'-') as Icdrgno,nvl(b.unit,'-') as Unit from " + frm_tabname + " a,item b," + frm_famtbl + " c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {


                        txtlbl2.Text = dt.Rows[i]["invno"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["invdated"].ToString().Trim();


                        txtlbl4.Text = dt.Rows[i]["Acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();

                        txtlbl73.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM " + frm_famtbl + " WHERE trim(acode)='" + txtlbl4.Text.Trim() + "'", "STATEn");

                        txtlbl5.Text = dt.Rows[i]["pordno"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["porddt1"].ToString().Trim();



                        txtlbl7.Text = dt.Rows[0]["cscode"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname  from csmst where trim(upper(acode))=upper(Trim('" + txtlbl7.Text + "'))", "aname");

                        txtlbl70.Text = dt.Rows[0]["billcode"].ToString().Trim();
                        txtlbl71.Text = dt.Rows[0]["work_ordno"].ToString().Trim();


                        txtlbl8.Text = dt.Rows[0]["BUSI_EXPECT"].ToString().Trim();
                        txtlbl9.Text = dt.Rows[0]["orderby"].ToString().Trim();
                        txtrmk.Text = dt.Rows[i]["remark"].ToString().Trim();

                        //txtlbl10.Text = dt.Rows[i]["gmt_shade"].ToString().Trim();
                        //txtlbl11.Text = dt.Rows[i]["busi_Expect"].ToString().Trim();
                        //txtlbl12.Text = dt.Rows[i]["inspby"].ToString().Trim();
                        //txtlbl13.Text = dt.Rows[i]["amdt3"].ToString().Trim();
                        //txtlbl14.Text = dt.Rows[i]["gmt_size"].ToString().Trim();



                        txtlbl15.Text = dt.Rows[i]["currency"].ToString().Trim();
                        txtlbl16.Text = dt.Rows[i]["amdt3"].ToString().Trim();
                        txtlbl17.Text = dt.Rows[i]["thru"].ToString().Trim();
                        txtlbl18.Text = dt.Rows[i]["bank_cd"].ToString().Trim();




                        txtlbl24.Text = dt.Rows[i]["curr_rate"].ToString().Trim();
                        txtlbl25.Text = dt.Rows[i]["basic"].ToString().Trim();
                        txtlbl27.Text = dt.Rows[i]["excise"].ToString().Trim();
                        txtlbl29.Text = dt.Rows[i]["cess"].ToString().Trim();
                        txtlbl31.Text = dt.Rows[i]["TOTAL"].ToString().Trim();

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

                            //sg1_dr["sg1_h7"] = dt.Rows[i]["pr_no"].ToString().Trim();
                            //sg1_dr["sg1_h8"] = dt.Rows[i]["pr_dt"].ToString().Trim();
                            //sg1_dr["sg1_h9"] = dt.Rows[i]["o_prate"].ToString().Trim();
                            //sg1_dr["sg1_h10"] = dt.Rows[i]["o_Qty"].ToString().Trim();

                            sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["Iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["ICpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["ICdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["Unit"].ToString().Trim();



                            sg1_dr["sg1_t1"] = dt.Rows[i]["desc_"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["pvt_mark"].ToString().Trim();

                            sg1_dr["sg1_t3"] = dt.Rows[i]["qtyord"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["irate"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["cdisc"].ToString().Trim();

                            sg1_dr["sg1_t7"] = dt.Rows[i]["pexc"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["ptax"].ToString().Trim();

                            sg1_dr["sg1_t9"] = dt.Rows[i]["desc9"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["iexc_Addl"].ToString().Trim();

                            sg1_dr["sg1_t12"] = dt.Rows[i]["sd"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["ipack"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["qtysupp"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["MFGINBR"].ToString().Trim();


                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        //------------------------
                        SQuery = "Select nvl(a.terms,'-') as terms,nvl(a.condi,'-') as condi from poterm a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY a.sno";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        create_tab2();
                        sg2_dr = null;
                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {

                                sg2_dr = sg2_dt.NewRow();
                                sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;

                                sg2_dr["sg2_t1"] = dt.Rows[i]["terms"].ToString().Trim();
                                sg2_dr["sg2_t2"] = dt.Rows[i]["condi"].ToString().Trim();

                                sg2_dt.Rows.Add(sg2_dr);
                            }
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

                        //------------------------
                        SQuery = "Select a.icode,to_chaR(a.dlv_Date,'dd/mm/yyyy') As dlv_Date,nvl(a.budgetcost,0) as budgetcost,nvl(a.actualcost,0) as actualcost,a.jobcardrqd,b.iname,nvl(b.cpartno,'-') As cpartno,nvl(b.cdrgno,'-') as cdrgno,nvl(b.unit,'-') as Unit from budgmst a,item b where trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO ";
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

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "N";
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
                case "Print":
                case "WO":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    if (hffield.Value == "WO") hffield.Value = "WO_E";
                    else hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;
                case "PI":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "PI_E";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select P.I. No to Copy", frm_qstr);
                    break;
                case "PI_E":
                    if (col1 == "") return;
                    clearctrl();


                    mv_col = frm_mbr + frm_vty + col1;

                    SQuery = "Select a.*,b.iname,c.Aname,nvl(b.cpartno,'-') As Icpartno,nvl(b.cdrgno,'-') as Icdrgno,nvl(b.unit,'-') as Unit from IVOUCHERP a,item b," + frm_famtbl + " c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a.VCHNUM)||to_Char(a.VCHDATE,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {

                        if (btnval == "PI_E")
                        {
                            txtlbl2.Text = dt.Rows[0]["VCHNUM"].ToString().Trim();
                            txtlbl3.Text = Convert.ToDateTime(dt.Rows[0]["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyyy");

                            txtlbl5.Text = dt.Rows[0]["VCHNUM"].ToString().Trim();
                            txtlbl6.Text = Convert.ToDateTime(dt.Rows[0]["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyyy");

                            if (frm_cocd == "MULT") cond = "like '4%'";
                            else cond = "='" + frm_vty + "'";

                            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE " + cond + " AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                            txtvchnum.Text = frm_vnum;
                            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                        }


                        txtlbl4.Text = dt.Rows[i]["Acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();

                        txtlbl73.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM " + frm_famtbl + " WHERE trim(acode)='" + txtlbl4.Text.Trim() + "'", "STATEn");


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

                            //sg1_dr["sg1_h7"] = dt.Rows[i]["pr_no"].ToString().Trim();
                            //sg1_dr["sg1_h8"] = dt.Rows[i]["pr_dt"].ToString().Trim();
                            //sg1_dr["sg1_h9"] = dt.Rows[i]["o_prate"].ToString().Trim();
                            //sg1_dr["sg1_h10"] = dt.Rows[i]["o_Qty"].ToString().Trim();

                            sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["Iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["ICpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["ICdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["Unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["desc_"].ToString().Trim();
                            sg1_dr["sg1_t2"] = "";

                            sg1_dr["sg1_t3"] = dt.Rows[i]["IQTYOUT"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["irate"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["ichgs"].ToString().Trim();

                            string app_rt;
                            app_rt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT irate as skfstr FROM somasm WHERE branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and trim(acode)='" + txtlbl4.Text + "' and trim(Icode)='" + dt.Rows[i]["icode"].ToString().Trim() + "' order by orddt desc", "skfstr");
                            if (app_rt != "0" && frm_tabname != "SOMASM")
                            {
                                app_rt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT irate||'~'||cdisc as skfstr FROM somasm WHERE branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and trim(acode)='" + txtlbl4.Text + "' and trim(Icode)='" + dt.Rows[i]["icode"].ToString().Trim() + "' order by orddt desc", "skfstr");
                                if (app_rt.Contains("~"))
                                {
                                    sg1_dr["sg1_h3"] = "SOMASM";
                                }
                            }

                            sg1_dr["sg1_t7"] = dt.Rows[i]["EXC_RATE"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["CESS_PERCENT"].ToString().Trim();

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        //------------------------
                        SQuery = "Select nvl(a.terms,'-') as terms,nvl(a.condi,'-') as condi from poterm a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY a.sno";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        create_tab2();
                        sg2_dr = null;
                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {

                                sg2_dr = sg2_dt.NewRow();
                                sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;

                                sg2_dr["sg2_t1"] = dt.Rows[i]["terms"].ToString().Trim();
                                sg2_dr["sg2_t2"] = dt.Rows[i]["condi"].ToString().Trim();

                                sg2_dt.Rows.Add(sg2_dr);
                            }
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

                        //------------------------
                        SQuery = "Select a.icode,to_chaR(a.dlv_Date,'dd/mm/yyyy') As dlv_Date,nvl(a.budgetcost,0) as budgetcost,nvl(a.actualcost,0) as actualcost,a.jobcardrqd,b.iname,nvl(b.cpartno,'-') As cpartno,nvl(b.cdrgno,'-') as cdrgno,nvl(b.unit,'-') as Unit from budgmst a,item b where trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO ";
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

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();

                    }
                    break;
                case "Atch_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    fgen.open_fileUploadPopup("Upload File for " + lblheader.Text, frm_qstr);
                    break;

                case "Edit_E":
                case "PI_EXXXX":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();


                    mv_col = frm_mbr + frm_vty + col1;

                    SQuery = "Select a.*,to_Char(nvl(a.cu_chldt,a.orddt),'dd/mm/yyyy') As cu_chldt1,to_Char(nvl(a.porddt,a.orddt),'dd/mm/yyyy') As porddt1,b.iname,c.Aname,nvl(b.cpartno,'-') As Icpartno,nvl(b.cdrgno,'-') as Icdrgno,nvl(b.unit,'-') as Unit from " + frm_tabname + "" + (btnval == "PI_E" ? "Q" : "") + " a,item b," + frm_famtbl + " c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        if (frm_tabname != "SOMASM")
                        {
                            if (dt.Rows[0]["app_by"].ToString().Trim().Length > 2 && frm_ulvl.toDouble() > 1)
                            {
                                fgen.msg("-", "AMSG", "Sales Order is already approved, Can not Edit this'13'Contact to admin!!");
                                return;
                            }
                        }

                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txtvchnum.Text = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl2.Text = dt.Rows[i]["amdt2"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["porddt1"].ToString().Trim();
                        txtlbl5.Text = dt.Rows[i]["pordno"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["porddt1"].ToString().Trim();


                        if (btnval == "PI_E")
                        {
                            txtlbl2.Text = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                            txtlbl3.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

                            txtlbl5.Text = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                            txtlbl6.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

                            if (frm_cocd == "MULT") cond = "like '4%'";
                            else cond = "='" + frm_vty + "'";

                            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE " + cond + " AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                            txtvchnum.Text = frm_vnum;
                            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                        }


                        txtlbl4.Text = dt.Rows[i]["Acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();

                        txtlbl73.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM " + frm_famtbl + " WHERE trim(acode)='" + txtlbl4.Text.Trim() + "'", "STATEn");


                        txtlbl7.Text = dt.Rows[0]["cscode"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname  from csmst where trim(upper(acode))=upper(Trim('" + txtlbl7.Text + "'))", "aname");

                        txtlbl70.Text = dt.Rows[0]["billcode"].ToString().Trim();
                        txtlbl71.Text = dt.Rows[0]["work_ordno"].ToString().Trim();


                        txtlbl8.Text = dt.Rows[0]["BUSI_EXPECT"].ToString().Trim();
                        txtlbl9.Text = dt.Rows[0]["orderby"].ToString().Trim();
                        txtrmk.Text = dt.Rows[i]["remark"].ToString().Trim();

                        //txtlbl10.Text = dt.Rows[i]["gmt_shade"].ToString().Trim();
                        //txtlbl11.Text = dt.Rows[i]["busi_Expect"].ToString().Trim();
                        //txtlbl12.Text = dt.Rows[i]["inspby"].ToString().Trim();
                        //txtlbl13.Text = dt.Rows[i]["amdt3"].ToString().Trim();
                        //txtlbl14.Text = dt.Rows[i]["gmt_size"].ToString().Trim();

                        txtlbl15.Text = dt.Rows[i]["currency"].ToString().Trim();
                        txtlbl16.Text = dt.Rows[i]["amdt3"].ToString().Trim();
                        txtlbl17.Text = dt.Rows[i]["thru"].ToString().Trim();
                        txtlbl18.Text = dt.Rows[i]["bank_cd"].ToString().Trim();

                        txtlbl24.Text = dt.Rows[i]["curr_rate"].ToString().Trim();
                        txtlbl25.Text = dt.Rows[i]["basic"].ToString().Trim();
                        txtlbl27.Text = dt.Rows[i]["excise"].ToString().Trim();
                        txtlbl29.Text = dt.Rows[i]["cess"].ToString().Trim();
                        txtlbl31.Text = dt.Rows[i]["TOTAL"].ToString().Trim();

                        txtlbl28.Text = dt.Rows[i]["advamt"].ToString().Trim();
                        txtlbl30.Text = dt.Rows[i]["del_mth"].ToString().Trim();

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

                            //sg1_dr["sg1_h7"] = dt.Rows[i]["pr_no"].ToString().Trim();
                            //sg1_dr["sg1_h8"] = dt.Rows[i]["pr_dt"].ToString().Trim();
                            //sg1_dr["sg1_h9"] = dt.Rows[i]["o_prate"].ToString().Trim();
                            //sg1_dr["sg1_h10"] = dt.Rows[i]["o_Qty"].ToString().Trim();

                            sg1_dr["sg1_f1"] = dt.Rows[i]["Icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["Iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["ICpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["ICdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["Unit"].ToString().Trim();
                            if (doc_fview.Value == "Y" && Prg_Id == "F45109")
                            {
                                sg1_dr["sg1_f4"] = dt.Rows[i]["polink"].ToString().Trim();

                            }
                            sg1_dr["sg1_t1"] = dt.Rows[i]["desc_"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["pvt_mark"].ToString().Trim();

                            sg1_dr["sg1_t3"] = dt.Rows[i]["qtyord"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["irate"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["cdisc"].ToString().Trim();

                            string app_rt;
                            app_rt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT irate as skfstr FROM somasm WHERE branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and trim(acode)='" + txtlbl4.Text + "' and trim(Icode)='" + dt.Rows[i]["icode"].ToString().Trim() + "' order by orddt desc", "skfstr");
                            if (app_rt != "0")
                            {
                                app_rt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT irate||'~'||cdisc as skfstr FROM somasm WHERE branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and trim(acode)='" + txtlbl4.Text + "' and trim(Icode)='" + dt.Rows[i]["icode"].ToString().Trim() + "' order by orddt desc", "skfstr");
                                if (app_rt.Contains("~"))
                                {
                                    sg1_dr["sg1_h3"] = "SOMASM";
                                }
                            }

                            sg1_dr["sg1_t7"] = dt.Rows[i]["pexc"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["ptax"].ToString().Trim();

                            sg1_dr["sg1_t9"] = dt.Rows[i]["desc9"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["iexc_Addl"].ToString().Trim();

                            sg1_dr["sg1_t12"] = dt.Rows[i]["sd"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["ipack"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["qtysupp"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["MFGINBR"].ToString().Trim();

                            sg1_dr["sg1_t16"] = dt.Rows[i]["invno"].ToString().Trim() + "-" + Convert.ToDateTime(dt.Rows[i]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();
                        //------------------------
                        SQuery = "Select nvl(a.terms,'-') as terms,nvl(a.condi,'-') as condi from poterm a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY a.sno";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        create_tab2();
                        sg2_dr = null;
                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {

                                sg2_dr = sg2_dt.NewRow();
                                sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;

                                sg2_dr["sg2_t1"] = dt.Rows[i]["terms"].ToString().Trim();
                                sg2_dr["sg2_t2"] = dt.Rows[i]["condi"].ToString().Trim();

                                sg2_dt.Rows.Add(sg2_dr);
                            }
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

                        //------------------------
                        SQuery = "Select a.icode,to_chaR(a.dlv_Date,'dd/mm/yyyy') As dlv_Date,nvl(a.budgetcost,0) as budgetcost,nvl(a.actualcost,0) as actualcost,a.jobcardrqd,b.iname,nvl(b.cpartno,'-') As cpartno,nvl(b.cdrgno,'-') as cdrgno,nvl(b.unit,'-') as Unit from budgmst a,item b where trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO ";
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

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        if (btnval != "PI_E")
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
                case "WO_E":
                    if (col1.Length < 2) return;
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    if (hffield.Value == "WO_E") fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id + "W");
                    else fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
                    fgen.fin_smktg_reps(frm_qstr);
                    break;
                case "PICK_SAGI":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();


                    mv_col = frm_mbr + frm_vty + col1;

                    SQuery = "Select a.*,to_Char(nvl(a.orddt,a.orddt),'dd/mm/yyyy') As orddated,to_Char(nvl(a.cu_chldt,a.orddt),'dd/mm/yyyy') As cu_chldt1,to_Char(nvl(a.porddt,a.orddt),'dd/mm/yyyy') As porddt1,b.iname,c.Aname,nvl(b.cpartno,'-') As Icpartno,nvl(b.cdrgno,'-') as Icdrgno,nvl(b.unit,'-') as Unit from finsagi.somas a,item b," + frm_famtbl + " c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')||trim(a.Acode)='" + frm_mbr + "4C" + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {


                        txtlbl2.Text = dt.Rows[i]["ordno"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["orddated"].ToString().Trim();

                        txtlbl4.Text = dt.Rows[i]["Acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();

                        txtlbl73.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM " + frm_famtbl + " WHERE trim(acode)='" + txtlbl4.Text.Trim() + "'", "STATEn");

                        txtlbl5.Text = dt.Rows[i]["pordno"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["porddt1"].ToString().Trim();

                        txtlbl7.Text = dt.Rows[0]["cscode"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname  from csmst where trim(upper(acode))=upper(Trim('" + txtlbl7.Text + "'))", "aname");

                        txtlbl70.Text = dt.Rows[0]["billcode"].ToString().Trim();
                        txtlbl71.Text = dt.Rows[0]["work_ordno"].ToString().Trim();

                        txtlbl8.Text = dt.Rows[0]["BUSI_EXPECT"].ToString().Trim();
                        txtlbl9.Text = dt.Rows[0]["orderby"].ToString().Trim();
                        txtrmk.Text = dt.Rows[i]["remark"].ToString().Trim();


                        txtlbl15.Text = dt.Rows[i]["currency"].ToString().Trim();
                        txtlbl16.Text = dt.Rows[i]["amdt3"].ToString().Trim();
                        txtlbl17.Text = dt.Rows[i]["thru"].ToString().Trim();
                        txtlbl18.Text = dt.Rows[i]["bank_cd"].ToString().Trim();

                        txtlbl24.Text = dt.Rows[i]["curr_rate"].ToString().Trim();
                        txtlbl25.Text = dt.Rows[i]["basic"].ToString().Trim();
                        txtlbl27.Text = dt.Rows[i]["excise"].ToString().Trim();
                        txtlbl29.Text = dt.Rows[i]["cess"].ToString().Trim();
                        txtlbl31.Text = dt.Rows[i]["TOTAL"].ToString().Trim();

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
                            sg1_dr["sg1_f2"] = dt.Rows[i]["Iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["ICpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["ICdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["Unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["desc_"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["pvt_mark"].ToString().Trim();

                            sg1_dr["sg1_t3"] = dt.Rows[i]["qtyord"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["irate"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["cdisc"].ToString().Trim();


                            sg1_dr["sg1_t7"] = dt.Rows[i]["pexc"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["ptax"].ToString().Trim();

                            sg1_dr["sg1_t9"] = dt.Rows[i]["desc9"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["iexc_Addl"].ToString().Trim();

                            sg1_dr["sg1_t12"] = dt.Rows[i]["sd"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["ipack"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["qtysupp"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["MFGINBR"].ToString().Trim();


                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose();
                        sg1_dt.Dispose();

                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "N";
                    }
                    #endregion
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    doc_somasm.Value = "";
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;

                    setAcodSelection();
                    if (frm_cocd == "SAGM" && lbl1a.Text == "4F" && txtlbl4.Text == "02S007")
                    {
                        hffield.Value = "PICK_SAGI";
                        make_qry_4_popup();
                        fgen.Fn_open_sseek("Select Type", frm_qstr);

                    }
                    if (frm_formID == "F47101")
                    {
                        mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(ORDNO)||'-'||TO_CHAR(ORDDT,'DD/MM/YYYY') AS FSTR FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND TRIM(ACODE)='" + txtlbl4.Text.Trim() + "' ", "fstr");
                        if (mhd != "0")
                        {
                            txtlbl4.Text = "";
                            txtlbl4a.Text = "";
                            fgen.msg("-", "AMSG", "Master S.O. has already entered for Selected Party. Order No - Date " + mhd);
                            return;
                        }
                    }
                    else
                    {
                        if (frm_tabname == "SOMAS")
                        {
                            mhd = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(ORDNO)||'-'||TO_CHAR(ORDDT,'DD/MM/YYYY') AS FSTR FROM " + frm_tabname + "M WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND TRIM(ACODE)='" + txtlbl4.Text.Trim() + "' ", "fstr");
                            if (mhd != "0")
                            {
                                doc_somasm.Value = "Y";
                            }
                        }
                    }
                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select enable_yn from controls where id='F46'", "enable_yn");
                    if (col1 == "Y" && txtlbl4.Text.Substring(0, 2) != "02")
                    {
                        string grc_Dt = "";
                        //col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(min(invdate),'dd/mm/yyyy') as mind from recdata where trim(acode)='" + txtlbl4.Text.Trim() + "' and NET > 10", "mind");
                        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TO_cHAR(INVDATE,'DD/MM/YYYY') AS MIND,NET FROM (SELECT ACODE,INVNO,INVDATE,SUM(NET) AS NET FROM RECDATA WHERE TRIM(ACODE)='" + txtlbl4.Text.Trim() + "' GROUP BY ACODE,INVNO,INVDATE) WHERE NET>10", "mind");
                        col2 = fgen.seek_iname(frm_qstr, frm_cocd, "select (payment+balop) as blop from " + frm_famtbl + " where trim(Acode)='" + txtlbl4.Text.Trim() + "'", "blop");
                        int a = 0;
                        if (col1.Length > 2) a = Convert.ToInt32(fgen.seek_iname(frm_qstr, frm_cocd, "Select to_date(to_char(sysdate,'dd/mm/yyyy'),'dd/mm/yyyy')-to_Date('" + col1 + "','dd/mm/yyyy') as dy from dual", "dy"));
                        if (a > 0)
                        {
                            if ((a > Convert.ToInt32(col2)) && col1 != "0")
                            {
                                try
                                {
                                    grc_Dt = fgen.seek_iname(frm_qstr, frm_cocd, "select grace_dt from " + frm_famtbl + " where trim(Acode)='" + txtlbl4.Text.ToString().Trim() + "' AND grace_dt!='-'", "grace_dt");
                                    if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(grc_Dt)) fgen.msg("-", "AMSG", "Allowed Upto " + Convert.ToDateTime(grc_Dt) + "");
                                    else
                                    {
                                        fgen.msg("-", "AMSG", "This Party is Allowed Credit " + col2 + " Days'13'However Some Bills are Pending for " + (a + 1) + " Days'13'New Sales Order , Please Contact to Customer ");
                                        txtlbl4.Text = "";
                                        txtlbl4a.Text = "";
                                    }
                                }
                                catch
                                {
                                    fgen.msg("-", "AMSG", "This Party is Allowed Credit " + col2 + " Days'13'However Some Bills are Pending for " + (a + 1) + " Days'13'New Sales Order , Please Contact to Customer ");
                                    txtlbl4.Text = "";
                                    txtlbl4a.Text = "";
                                }
                            }
                        }
                    }

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

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        if (doc_GST.Value == "N" || frm_vty == "4F")
                        {
                            if (col1.Trim().Length == 8) SQuery = "select distinct a.icode,a.iname,a.cpartno,a.cdrgno,a.unit,a.hscode,0 as num4,0 as num5,0 as num6,0 as num7,a.mat2,a.irate,nvl(a.madeinbr,'-') as madeinbr from item a where trim(a.icode) in ('" + col1 + "')";
                            else SQuery = "select distinct a.icode,a.iname,a.cpartno,a.cdrgno,a.unit,a.hscode,0 as num4,0 as num5,0 as num6,0 as num7,a.mat2,a.irate,nvl(a.madeinbr,'-') as madeinbr from item a where trim(a.icode) in (" + col1 + ")";
                        }
                        else
                        {
                            if (col1.Trim().Length == 8) SQuery = "select distinct a.icode,a.iname,a.cpartno,a.cdrgno,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.mat2,a.irate,nvl(a.madeinbr,'-') as madeinbr from item a left outer join typegrp b on trim(a.hscode)=trim(b.acref) and b.id='T1' where trim(a.icode) in ('" + col1 + "')";
                            else SQuery = "select distinct a.icode,a.iname,a.cpartno,a.cdrgno,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.mat2,a.irate,nvl(a.madeinbr,'-') as madeinbr from item a left outer join typegrp b on trim(a.hscode)=trim(b.acref) and b.id='T1' where trim(a.icode) in (" + col1 + ")";
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

                            sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = "";
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = dt.Rows[d]["irate"].ToString().Trim();
                            sg1_dr["sg1_t5"] = "";
                            string app_rt;
                            app_rt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT irate as skfstr FROM somasm WHERE branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(acode)='" + txtlbl4.Text + "' and trim(Icode)='" + dt.Rows[d]["icode"].ToString().Trim() + "' order by orddt desc", "skfstr");
                            if (app_rt != "0")
                            {
                                app_rt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT irate||'~'||cdisc as skfstr FROM somasm WHERE branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(acode)='" + txtlbl4.Text + "' and trim(Icode)='" + dt.Rows[d]["icode"].ToString().Trim() + "' order by orddt desc", "skfstr");
                                if (app_rt.Contains("~"))
                                {
                                    sg1_dr["sg1_t4"] = app_rt.Split('~')[0].ToString();
                                    sg1_dr["sg1_t5"] = app_rt.Split('~')[1].ToString();
                                    sg1_dr["sg1_h3"] = "SOMASM";
                                }
                            }
                            else
                            {
                                app_rt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT irate||'~'||nvl(cdisc,0)||'~'||trim(nvl(Desc9,'-'))||'~'||trim(cpartno) as skfstr FROM somas WHERE branchcd='" + frm_mbr + "' and trim(acode)='" + txtlbl4.Text + "' and trim(Icode)='" + dt.Rows[d]["icode"].ToString().Trim() + "' order by orddt desc", "skfstr");
                                if (app_rt.Contains("~"))
                                {
                                    if (frm_cocd != "SAIA")
                                        sg1_dr["sg1_t4"] = app_rt.Split('~')[0].ToString();
                                    sg1_dr["sg1_t5"] = app_rt.Split('~')[1].ToString();
                                    sg1_dr["sg1_t9"] = app_rt.Split('~')[2].ToString();
                                    sg1_dr["sg1_t10"] = app_rt.Split('~')[3].ToString();

                                    //((TextBox)sg1.Rows[d].FindControl("sg1_t4")).Attributes.Add("readonly", "readonly");
                                    //((TextBox)sg1.Rows[d].FindControl("sg1_t5")).Attributes.Add("readonly", "readonly");
                                }
                            }
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


                            if (frm_cocd == "SAIA" && frm_formID == "F47106")
                            {
                                sg1_dr["sg1_t4"] = getDiscountedRate(dt.Rows[d]["icode"].ToString().Trim(), sg1_dr["sg1_t4"].ToString(), (sg1_dr["sg1_t7"].ToString().toDouble() + sg1_dr["sg1_t8"].ToString().toDouble()).ToString());
                                sg1_dr["sg1_f4"] = rateDiscount;
                            }

                            sg1_dr["sg1_t9"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[d]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_t11"] = "0";
                            sg1_dr["sg1_t12"] = "";
                            sg1_dr["sg1_t13"] = "";
                            sg1_dr["sg1_t14"] = "";
                            sg1_dr["sg1_t15"] = dt.Rows[d]["MADEINBR"].ToString().Trim();
                            sg1_dr["sg1_t16"] = "";

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    if (sg1_dt != null)
                    {
                        sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    }
                    #endregion
                    setColHeadings();
                    setGST();
                    btnlbl4.Enabled = false;
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

                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t9")).Text = col2;

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
                    //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t16")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    break;
                case "SG1_ROW_DT":
                    if (Prg_Id == "F45109" && doc_fview.Value == "Y")
                    {
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = col1;
                    }


                    //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    break;
                case "SG1_ROW_QU":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t16")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                    if (col1.Length > 8)
                    {
                        txtlbl2.Text = col1.Split('-')[0];
                        txtlbl3.Text = col1.Split('-')[1];
                    }
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
                case "SALESMAN":
                    txtlbl8.Text = col1;
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

            cond = frm_ulvl == "M" ? " trim(a.acode)='" + frm_uname + "'" : "trim(a.acode) like '" + party_cd + "%'";

            SQuery = "Select a.ORDNO as SO_No,to_char(a.oRDDT,'dd/mm/yyyy') as Dated,decode(substr(trim(a.app_by),1,1),'-','(Un Approved)','(','(Cancel/Rej)','(Approved)') as Doc_Status,c.Aname as Customer,b.iname as Item_Name,b.cpartno as Part_No,a.qtyord as Req_Qty,a.Irate,a.Cdisc,b.unit,a.Desc_,a.icode,a.ent_by,a.ent_Dt,a.app_by,a.app_dt  from " + frm_tabname + " a, item b," + frm_famtbl + " c where a.branchcd='" + frm_mbr + "'  /*and a.type='" + frm_vty + "'*/ and a." + doc_df.Value + " " + PrdRange + " and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and " + cond + " and a.icode like '" + part_cd + "%' order by a." + doc_df.Value + ",a." + doc_nf.Value + ",a.srno ";
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

            frm_vty = lbl1a.Text;
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
                        //oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "ivchctrl");

                        oDS3 = new DataSet();
                        oporow3 = null;
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "poterm");

                        oDS4 = new DataSet();
                        oporow4 = null;
                        //oDS4 = fgen.fill_schema(frm_qstr, frm_cocd, "budgmst");

                        oDS5 = new DataSet();
                        oporow5 = null;
                        //oDS5 = fgen.fill_schema(frm_qstr, frm_cocd, "udf_data");

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";
                        save_fun();
                        //save_fun2();
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
                        //oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "ivchctrl");

                        oDS3.Dispose();
                        oporow3 = null;
                        oDS3 = new DataSet();
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "poterm");

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
                                if (frm_cocd == "MULT") frm_vnum = fgen.Fn_next_doc_no_inv(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                else frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }

                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        //save_fun2();
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
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "update ivchctrl set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "update poterm set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "update budgmst set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "update udf_Data set branchcd='DD' where par_tbl='" + frm_tabname + "' and par_fld='" + ddl_fld1 + "'");

                        }

                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        //fgen.save_data(frm_qstr, frm_cocd, oDS2, "ivchctrl");
                        //fgen.save_data(frm_qstr, frm_cocd, oDS3, "poterm");
                        //fgen.save_data(frm_qstr, frm_cocd, oDS4, "budgmst");
                        //fgen.save_data(frm_qstr, frm_cocd, oDS5, "udf_Data");

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully'13'Do you want to see the Print Preview ?");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivchctrl where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from poterm where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from budgmst where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data where branchcd='DD' and par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + ddl_fld2 + "'");
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

                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr"), frm_uname, edmode.Value);

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtvchdate.Text.Trim() + "'");
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                        hffield.Value = "SAVED";

                        setColHeadings();
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
        sg1_dr["sg1_t11"] = "0";
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

            if (e.Row.Cells[2].Text.Trim().ToUpper() == "SOMASM" && frm_tabname != "SOMASM")
            {
                ((TextBox)e.Row.FindControl("sg1_t4")).Attributes.Add("readonly", "readonly");
            }

            int z = 0;
            int i = 13;
            //for (int i = z; i < e.Row.Cells.Count - 1; i++)
            if (e.Row.Cells.Count > 13)
            {
                if (e.Row.Cells[13].Text.Trim().Length >= 8)
                {
                    TableCell cell = e.Row.Cells[i];
                    cell.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='underline';";
                    cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                    cell.ToolTip = "You can click this cell to Check Rate history";
                    cell.Attributes["ondblclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}", SelectedGridCellIndex.ClientID, i, Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
                    cell.BackColor = System.Drawing.Color.GreenYellow;
                }
            }
        }
    }

    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
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
                case "SG1_ROW_QU":
                    if (index < sg1.Rows.Count - 1)
                    {
                        hf1.Value = index.ToString();
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CURR_ITEM", sg1.Rows[index].Cells[13].Text);

                        //----------------------------
                        hffield.Value = "SG1_ROW_QU";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                        make_qry_4_popup();
                        if (Prg_Id == "F45109") fgen.Fn_open_sseek("Select Lead Number", frm_qstr);
                        else fgen.Fn_open_sseek("Select Quotation Number", frm_qstr);
                    }
                    break;

                case "SG1_ROW_ADD":
                    if (txtlbl4.Text.Trim().Length < 5)
                    {
                        fgen.msg("-", "AMSG", "Please Select Party First!!");
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
        catch { }
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
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
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
        fgen.Fn_open_sseek("Select " + lbl12.Text + " ", frm_qstr);
    }
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_13";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl13.Text + "", frm_qstr);
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
        fgen.Fn_open_sseek("Select Currency ", frm_qstr);
    }
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_16";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Contract Terms ", frm_qstr);
    }
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_17";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Payment Terms ", frm_qstr);
    }
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_18";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Bank A/c ", frm_qstr);
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
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl7.Text + " ", frm_qstr);
    }
    protected void btnlbl70_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODEX";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);

        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type ", frm_qstr);
    }

    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        if (fgen.make_double(txtlbl24.Text) <= 0)
        {
            //txtlbl15.Text = "INR";
            txtlbl24.Text = "1";
        }
        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Length > 2)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["orignalbr"] = frm_mbr;
                oporow["TYPE"] = lbl1a.Text.Substring(0, 2);
                oporow["ordno"] = frm_vnum.Trim();
                oporow["orddt"] = txtvchdate.Text.Trim();
                oporow["ICAT"] = "N";

                oporow["amdt2"] = txtlbl2.Text;

                oporow["acode"] = txtlbl4.Text.Trim();
                oporow["pordno"] = txtlbl5.Text.Trim().ToUpper();
                oporow["porddt"] = fgen.make_def_Date(txtlbl6.Text.Trim(), vardate);
                oporow["cscode"] = txtlbl7.Text.Trim();

                oporow["BUSI_EXPECT"] = txtlbl8.Text.Trim().ToUpper();
                oporow["SALE_REP"] = txtlbl8.Text.Trim().ToUpper();

                oporow["orderby"] = txtlbl9.Text.Trim().ToUpper();

                oporow["billcode"] = txtlbl70.Text.Trim().ToUpper();
                oporow["WORK_ORDNO"] = txtlbl71.Text.Trim().ToUpper();

                oporow["srno"] = i + 1;
                oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim();

                if (doc_fview.Value == "Y" && Prg_Id == "F45109")
                {
                    oporow["polink"] = sg1.Rows[i].Cells[16].Text.Trim();
                }

                oporow["ciname"] = sg1.Rows[i].Cells[14].Text.Trim();

                oporow["desc_"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                oporow["cu_chldt"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim(), vardate);
                oporow["pvt_mark"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();

                oporow["qtyord"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim());

                oporow["irate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim());
                oporow["cdisc"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim());

                oporow["pexc"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim());
                oporow["ptax"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim());

                oporow["desc9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();

                if (((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().Length < 2)
                {
                    oporow["desc9"] = sg1.Rows[i].Cells[14].Text.Trim();
                }


                oporow["cpartno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                oporow["cdrgno"] = frm_vnum + "." + (i + 1).ToString();

                oporow["iexc_addl"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim());
                oporow["sd"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim());
                oporow["ipack"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim());

                oporow["qtysupp"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim());
                oporow["MFGINBR"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                oporow["weight"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                oporow["remark"] = txtrmk.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");


                oporow["currency"] = txtlbl15.Text.Trim();
                oporow["amdt3"] = txtlbl16.Text.Trim();
                oporow["thru"] = txtlbl17.Text.Trim();
                oporow["bank_cd"] = txtlbl18.Text.Trim();

                oporow["CURR_RATE"] = fgen.make_double(txtlbl24.Text.Trim());

                oporow["ST_TYPE"] = lbl27.Text.Substring(0, 2);
                oporow["basic"] = txtlbl25.Text.Trim();
                oporow["excise"] = txtlbl27.Text;
                oporow["cess"] = txtlbl29.Text;
                oporow["total"] = txtlbl31.Text.ToString().Trim();

                if (frm_tabname == "SOMAS") oporow["desc7"] = "-";
                else oporow["desc7"] = txtlbl4a.Text.Trim().Replace("'", "`");


                oporow["delivery"] = 0;
                oporow["class"] = 0;
                oporow["qtybal"] = 0;

                oporow["taxes"] = 0;

                if (((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().Length > 10)
                {
                    oporow["invno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().Split('-')[0];
                    oporow["invdate"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().Split('-')[((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim().Split('-').Length - 1];
                }
                else
                {
                    oporow["invno"] = "-";
                    oporow["invdate"] = vardate;
                }

                oporow["org_invno"] = "-";
                oporow["org_invdt"] = vardate;
                oporow["del_date"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim(), vardate);
                oporow["delr_date"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim(), vardate);
                oporow["del_wk"] = 0;
                oporow["packing"] = 0;
                oporow["prefix"] = "-";
                oporow["revis_no"] = "-";

                oporow["ms_cont"] = "-";
                oporow["amdt1"] = "-";



                oporow["inst1"] = 0;
                oporow["inst2"] = 0;
                oporow["inst3"] = 0;
                oporow["othamt1"] = 0;
                oporow["othamt2"] = 0;
                oporow["othamt3"] = 0;
                oporow["othac1"] = "-";
                oporow["othac2"] = "-";
                oporow["othac3"] = "-";

                oporow["bcd"] = 0;
                oporow["bcdr"] = 0;
                oporow["ccess"] = 0;
                oporow["ccessr"] = 0;
                oporow["acvd"] = 0;
                oporow["acvdr"] = 0;


                oporow["shipfrom"] = "-";
                oporow["shipto"] = "-";
                oporow["destcount"] = "-";
                oporow["tptdtl"] = "-";
                oporow["predisp"] = "-";

                oporow["packinst"] = "-";
                oporow["shipmark"] = "-";

                oporow["advamt"] = txtlbl28.Text.toDouble();
                oporow["del_mth"] = txtlbl30.Text.toDouble();

                oporow["packamt"] = 0;
                oporow["std_pking"] = 0;
                oporow["sheCess"] = 0;


                oporow["Foc"] = "N";
                oporow["promdt"] = vardate;
                oporow["CO_ORIG"] = "-";



                //oporow["gmt_size"] = TextBox8.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");
                //oporow["desc4"] = txttransname.Text.Trim();
                //oporow["desc5"] = txtbookingplace.Text.Trim();
                //oporow["desc6"] = dddlv.SelectedValue.ToString().Trim();

                //oporow["desc8"] = txtfrghtrmk.Text.Trim();

                //oporow["orderby"] = txtpersonname.Text.Trim();
                //oporow["explic"] = txtnego.Text.Trim();
                oporow["gmt_shade"] = "-";
                oporow["gmt_size"] = "-";


                oporow["check_by"] = "-";
                oporow["check_dt"] = vardate;

                if (edmode.Value == "Y") oporow["desp_to"] = "Edited";
                else oporow["desp_to"] = "New";

                if (edmode.Value == "Y")
                {
                    oporow["eNt_by"] = ViewState["entby"].ToString();
                    oporow["eNt_dt"] = ViewState["entdt"].ToString();
                    oporow["edt_by"] = frm_uname;
                    oporow["edt_dt"] = vardate;
                    oporow["app_by"] = "-";
                    oporow["app_dt"] = vardate;
                }
                else
                {
                    oporow["eNt_by"] = frm_uname;
                    oporow["eNt_dt"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["eDt_dt"] = vardate;
                    oporow["app_by"] = "-";
                    oporow["app_dt"] = vardate;
                }
                oDS.Tables[0].Rows.Add(oporow);
            }
        }
    }
    void save_fun2()
    {
        ////string curr_dt;
        //vardate = fgen.seek_iname(frm_qstr,frm_cocd, "select sysdate as ldt from dual", "ldt");


        //oporow2 = oDS2.Tables[0].NewRow();
        //oporow2["BRANCHCD"] = frm_mbr;
        //oporow2["TYPE"] = lbl1a.Text;
        //oporow2["vchnum"] = frm_vnum;
        //oporow2["vchdate"] = txtvchdate.Text.Trim();
        //oporow2["invdate"] = txtvchdate.Text.Trim();


        //oporow2["Acode"] = txtlbl4.Text;
        //oporow2["inature"] = "-";
        //oporow2["invno"] = "-";

        //oporow2["srno"] = 0;

        //oporow2["TOTQTY"] = 0;
        //oporow2["AMT_SALE"] = fgen.make_double(txtlbl24.Text);
        //oporow2["AMT_EXC"] = fgen.make_double(txtlbl26.Text);
        //oporow2["RVALUE"] = fgen.make_double(txtlbl27.Text); ;
        //oporow2["CST_AMT"] = fgen.make_double(txtlbl28.Text); ;
        //oporow2["OTHER"] = fgen.make_double(txtlbl30.Text);
        //oporow2["BILL_TOT"] = fgen.make_double(txtlbl31.Text);
        //oporow2["VATSCHG"] = fgen.make_double(txtlbl29.Text); ;
        //oporow2["PACK_AMT"] = fgen.make_double(txtlbl25.Text);

        //oporow2["MATAC"] = "-";
        //oporow2["TAXCODE"] = "-";
        //oporow2["TAXRATE"] = 0;
        //oporow2["LESSAMT"] = 0;
        //oporow2["TAX_FRT"] = 0;

        //oporow2["FRT_AMT"] = 0;

        //oporow2["SHVALUE"] = 0;
        //oporow2["RNDCESS"] = 0;
        //oporow2["LST_AMT"] = 0;

        //oporow2["S_LST"] = 0;
        //oporow2["COND1"] = "-";
        //oporow2["COND2"] = "-";
        //oporow2["COND3"] = "-";
        //oporow2["COND4"] = "-";
        //oporow2["COND5"] = "-";
        //oporow2["COND6"] = "-";
        //oporow2["COND7"] = "-";
        //oporow2["COND8"] = "-";
        //oporow2["COND9"] = "-";
        //oporow2["EXCB_CHG"] = 0;
        //oporow2["FINVNUM"] = "-";
        //oporow2["ED_EXTRA"] = "-";
        //oporow2["WHNAME"] = "-";
        //oporow2["ATCH1"] = "-";
        //oporow2["ATCH2"] = "-";
        //oporow2["T_GRNO"] = "-";
        //oporow2["T_GRDT"] = "-";
        //oporow2["T_NAME"] = "-";
        //oporow2["T_VNO"] = "-";
        //oporow2["CUST_AMT"] = 0;
        //oporow2["AC_HOVER"] = "-";
        //oDS2.Tables[0].Rows.Add(oporow2);
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

        if (frm_cocd == "SAGI")
        {
            SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE FROM type where id='V' and type1 like '4%' and type1 in ('4C','4F') order by type1";
        }
        else
        {
            if (Prg_Id == "F47106" || Prg_Id == "F47101" || Prg_Id == "F45109")
            {
                SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE FROM type where id='V' and type1 like '4%' and type1!='4F' order by type1";
            }
            else
            {
                SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE FROM type where id='V' and type1 like '4%' order by type1";
            }

        }
        btnval = hffield.Value;
        if (btnval != "New")
        {
            SQuery = "select Fstr,Document_Name,Document_Series,count(*) as Document_Count from (SELECT distinct a.type as Fstr,b.NAME as Document_Name,a.TYPE as Document_Series,a.ordno FROM " + frm_tabname + " a ,TYPE b WHERE a.branchcd='" + frm_mbr + "' and a.orddt " + DateRange + " and trim(A.type)=trim(B.type1) and b.ID='V' and a.type like '4%') group by Fstr,Document_Name,Document_Series order by Document_Series ";
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
            txtlbl29.Width = 1;
        }
    }

    void setAcodSelection()
    {
        txtlbl73.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT staten FROM " + frm_famtbl + " WHERE ACODE='" + txtlbl4.Text.Trim() + "' ", "staten");

        string app_rt1;
        app_rt1 = fgen.seek_iname(frm_qstr, frm_cocd, "select * from (SELECT ordno FROM somasm WHERE branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(acode)='" + txtlbl4.Text + "' order by orddt desc) where rownum<2", "ordno");
        if (app_rt1 != "0")
        {
            app_rt1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT nvl(currency,'-')||'~'||nvl(amdt3,'-')||'~'||nvl(thru,'-')||'~'||nvl(bank_cd,'-')||'~'||nvl(busi_Expect,'-')||'~'||nvl(orderby,'-') as skfstr FROM somasm WHERE branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and trim(acode)='" + txtlbl4.Text + "'  order by orddt desc", "skfstr");
            if (app_rt1.Contains("~"))
            {
                txtlbl15.Text = app_rt1.Split('~')[0].ToString();
                txtlbl16.Text = app_rt1.Split('~')[1].ToString();
                txtlbl17.Text = app_rt1.Split('~')[2].ToString();
                txtlbl18.Text = app_rt1.Split('~')[3].ToString();
                txtlbl8.Text = app_rt1.Split('~')[4].ToString();
                txtlbl9.Text = app_rt1.Split('~')[5].ToString();
            }

        }
        if (txtlbl17.Text.Trim() == "" || txtlbl17.Text.Trim() == "-")
        {
            txtlbl17.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT pay_num FROM " + frm_famtbl + " WHERE ACODE='" + txtlbl4.Text.Trim() + "'", "pay_num");
        }
        btnlbl7.Focus();
    }
    protected void btnCons_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "C_FROM", "SOMAS");
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('../tej-base/om_csmst.aspx?STR=" + frm_qstr + "','90%','95%','Tejaxo');", true);

    }
    protected void btnupload_ServerClick(object sender, EventArgs e)
    {
        string ext = "", filesavepath = "";
        string excelConString = "";
        string filename = "";
        if (fileImport.HasFile)
        {
            ext = System.IO.Path.GetExtension(fileImport.FileName).ToLower();
            if (ext == ".xls")
            {
                filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xls";
                fileImport.SaveAs(filesavepath);
                excelConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
            }
            else if (ext == ".csv")
            {
                filename = "" + DateTime.Now.ToString("ddMMyyhhmmfff");
                filesavepath = AppDomain.CurrentDomain.BaseDirectory + "tej-base\\Upload\\file" + filename + ".csv";
                fileImport.SaveAs(filesavepath);
                excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "tej-base\\Upload\\" + ";Extended Properties=\"Text;HDR=Yes;FMT=Delimited\"";
            }
            else
            {
                fgen.msg("-", "AMSG", "Please Select Excel File only in xls format!!");
                return;
            }
        }

        try
        {
            OleDbConnection OleDbConn = new OleDbConnection(); OleDbConn.ConnectionString = excelConString;
            OleDbConn.Open();
            DataTable DTEXdt = OleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            OleDbConn.Close();
            String[] excelSheets = new String[DTEXdt.Rows.Count];
            int i = 0;
            foreach (DataRow row in DTEXdt.Rows)
            {
                excelSheets[i] = row["TABLE_NAME"].ToString();
                i++;
            }
            if (ext == ".csv")
                excelSheets[0] = "file" + filename + ".csv";
            OleDbCommand OleDbCmd = new OleDbCommand();
            String Query = "";
            Query = "SELECT  * FROM [" + excelSheets[0] + "]";
            OleDbCmd.CommandText = Query;
            OleDbCmd.Connection = OleDbConn;
            OleDbCmd.CommandTimeout = 0;
            OleDbDataAdapter objAdapter = new OleDbDataAdapter();
            objAdapter.SelectCommand = OleDbCmd;
            objAdapter.SelectCommand.CommandTimeout = 0;
            DataTable DTEX = new DataTable();
            objAdapter.Fill(DTEX);
            DataTable dtm = new DataTable();
            dtm.Columns.Add("ICODE", typeof(string));
            dtm.Columns.Add("INAME", typeof(string));
            dtm.Columns.Add("CPARTNO", typeof(string));
            dtm.Columns.Add("CDRGNO", typeof(string));
            dtm.Columns.Add("UNIT", typeof(string));
            dtm.Columns.Add("irate", typeof(string));
            dtm.Columns.Add("HSCODE", typeof(string));
            dtm.Columns.Add("NUM4", typeof(string));
            dtm.Columns.Add("NUM5", typeof(string));
            dtm.Columns.Add("NUM6", typeof(string));
            dtm.Columns.Add("NUM7", typeof(string));
            dtm.Columns.Add("MADEINBR", typeof(string));
            dtm.Columns.Add("qty", typeof(string));

            DataRow drn;
            foreach (DataRow dr in DTEX.Rows)
            {
                SQuery = "select distinct a.icode,a.iname,a.cpartno,a.cdrgno,a.irate,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.mat2,nvl(a.madeinbr,'-') as madeinbr from item a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.CPARTNO) in ('" + dr[0].ToString().Trim() + "')";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                if (dt2.Rows.Count > 0)
                {
                    drn = dtm.NewRow();
                    drn["ICODE"] = dt2.Rows[0]["ICODE"];
                    drn["INAME"] = dt2.Rows[0]["INAME"];
                    drn["CPARTNO"] = dt2.Rows[0]["CPARTNO"];
                    drn["CDRGNO"] = dt2.Rows[0]["CDRGNO"];
                    drn["UNIT"] = dt2.Rows[0]["UNIT"];
                    drn["irate"] = dt2.Rows[0]["irate"];
                    drn["HSCODE"] = dt2.Rows[0]["HSCODE"];
                    drn["NUM4"] = dt2.Rows[0]["NUM4"];
                    drn["NUM5"] = dt2.Rows[0]["NUM5"];
                    drn["NUM6"] = dt2.Rows[0]["NUM6"];
                    drn["NUM7"] = dt2.Rows[0]["NUM7"];
                    drn["MADEINBR"] = dt2.Rows[0]["MADEINBR"];
                    drn["qty"] = dr[1];
                    dtm.Rows.Add(drn);
                }
            }

            #region for gridview 1
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
                dt = dtm;
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

                    sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                    sg1_dr["sg1_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                    sg1_dr["sg1_f3"] = dt.Rows[d]["cpartno"].ToString().Trim();
                    sg1_dr["sg1_f4"] = dt.Rows[d]["cdrgno"].ToString().Trim();
                    sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();

                    sg1_dr["sg1_t1"] = "";
                    sg1_dr["sg1_t2"] = "";
                    sg1_dr["sg1_t3"] = dt.Rows[d]["qty"].ToString().Trim();
                    sg1_dr["sg1_t4"] = dt.Rows[d]["irate"].ToString().Trim();
                    sg1_dr["sg1_t5"] = "";
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
                    if (frm_cocd == "SAIA" && frm_formID == "F47106")
                    {
                        sg1_dr["sg1_t4"] = getDiscountedRate(dt.Rows[d]["icode"].ToString().Trim(), sg1_dr["sg1_t4"].ToString(), (sg1_dr["sg1_t7"].ToString().toDouble() + sg1_dr["sg1_t8"].ToString().toDouble()).ToString());
                        sg1_dr["sg1_f4"] = rateDiscount;
                    }
                    sg1_dr["sg1_t9"] = dt.Rows[d]["iname"].ToString().Trim();
                    sg1_dr["sg1_t10"] = dt.Rows[d]["cpartno"].ToString().Trim();
                    sg1_dr["sg1_t11"] = "0";
                    sg1_dr["sg1_t12"] = "";
                    sg1_dr["sg1_t13"] = "";
                    sg1_dr["sg1_t14"] = "";
                    sg1_dr["sg1_t15"] = dt.Rows[d]["MADEINBR"].ToString().Trim();
                    sg1_dr["sg1_t16"] = "";

                    sg1_dt.Rows.Add(sg1_dr);
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
        catch { fgen.msg("-", "AMSG", "Please Select Excel File only in .xls format!!"); }
    }

    string getDiscountedRate(string ticode, string currRate, string tax)
    {
        DataTable dtdisc = new DataTable();
        if (ViewState["dtItemSub"] == null)
        {
            cond = "BRANCHCD='00'";
            SQuery = "SELECT TRIM(ICODE) AS ICODE,num1 as irate,num2 AS IRATE2,to_char(vchdate,'yyyymmdd') as vdd FROM SCRATCH2 WHERE " + cond + " AND TYPE='DS' ORDER BY icode,vdd desc ";
            if (frm_mbr == "08" || frm_mbr == "09")
            {
            }
            else
            {
                dtdisc = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                ViewState["dtItemSub"] = dtdisc;
            }
        }
        else
        {
            dtdisc = (DataTable)ViewState["dtItemSub"];
        }
        if (frm_mbr == "08" || frm_mbr == "09")
        {
            col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT BANK_ACNO AS COL1 FROM " + frm_famtbl + " WHERE TRIM(ACODE)='" + txtlbl4.Text.Trim() + "'", "COL1");
            SQuery = "SELECT TRIM(b.ICODE) AS ICODE,a.num1 as irate,a.num2 AS IRATE2,to_char(a.vchdate,'yyyymmdd') as vdd FROM SCRATCH2 a,item b WHERE a.branchcd in ('08','09') AND a.TYPE='DS' and trim(B.abc_class)=trim(A.icode) AND TRIM(B.MAT1)=TRIM(A.COL1) AND TRIM(A.ACODe)='" + col3 + "' ORDER BY a.icode,vdd desc ";
            dtdisc = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            ViewState["dtItemSub"] = dtdisc;
        }

        string rate = currRate;
        rateDiscount = fgen.seek_iname_dt(dtdisc, "ICODE='" + ticode + "' ", "IRATE2", "vdd desc");
        if (rateDiscount.toDouble() > 0)
        {
            rate = ((rate.toDouble() - (rate.toDouble() * (rateDiscount.toDouble() / 100))).toDouble(2)).ToString();
        }
        return rate;
    }
    protected void btnWO_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "WO";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Type for Work Order Print", frm_qstr);
    }
    protected void btnPI_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "PI_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select P.I.", frm_qstr);
    }

    protected void btnAtch_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Atch_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr);
    }


    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    protected void Button2_Click(object sender, EventArgs e)
    {

    }
    protected void Button3_Click(object sender, EventArgs e)
    {

    }
    protected void Button4_Click(object sender, EventArgs e)
    {

    }
    protected void btnOurReps_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "SALESMAN";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Our Representative / Sales Person ", frm_qstr);
    }
    protected void sg1_SelectedIndexChanged(object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow row = sg1.SelectedRow;
        int rowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        if (selectedCellIndex < 0) selectedCellIndex = 0;
        string mq0 = sg1.HeaderRow.Cells[selectedCellIndex].Text.Replace("<br/>", " "); // dynamic heading        
        if (selectedCellIndex > 0) selectedCellIndex -= 1;

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CURR_ITEM", sg1.Rows[rowIndex].Cells[13].Text);

        //----------------------------
        hffield.Value = "SG1_ROW_DT";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Rate History", frm_qstr);
    }
}