using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Data.OleDb;

public partial class om_po_entry : System.Web.UI.Page
{

    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok, save_it, Prg_Id;
    string frm_famtbl = "";
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond = "", itemRepeat = "";
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    string mgst_no = "", runningVchnum = "N", PR_lin_itm_sys = "N";
    string PR_buttn = "", frm_IndType = "";
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
                    vardate = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VARDATE");

                    frm_IndType = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                itemRepeat = fgen.seek_iname(frm_qstr, frm_cocd, "select enable_yn from controls where id='P27'", "enable_yn");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMREPEAT", itemRepeat);

                //doc_addl.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select (case when nvl(st_Sc,1)=0 then 1 else nvl(st_Sc,1) end )  as add_tx from type where id='B' and trim(upper(type1))=upper(Trim('" + frm_mbr + "'))", "add_tx");
                doc_addl.Value = "-";

                runningVchnum = fgen.getOption(frm_qstr, frm_cocd, "W0053", "OPT_ENABLE");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_RUNNO", (runningVchnum == "0" ? "N" : runningVchnum));

                doc_GST.Value = "Y";
                //GSt india
                string chk_opt = "";
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2017'", "fstr");
                if (chk_opt == "N")
                {
                    doc_GST.Value = "N";

                }
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2027'", "fstr");
                if (chk_opt == "Y")
                {
                    doc_GST.Value = "GCC";
                }
                PR_lin_itm_sys = fgen.getOption(frm_qstr, frm_cocd, "W0005", "OPT_ENABLE");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_PR_LIN_SYS", PR_lin_itm_sys);
                PR_buttn = fgen.getOption(frm_qstr, frm_cocd, "W0066", "OPT_ENABLE");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_PR_BUTTN", PR_buttn);
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

        // to hide and show to tab panel



        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F50101":
                tab2.Visible = false;
                tab3.Visible = false;
                tab4.Visible = false;
                tab5.Visible = false;
                tab6.Visible = false;
                break;

            case "F15106":

                btnImport.Visible = false;
                tab2.Visible = false;
                if (frm_cocd == "SAIA")
                {
                    btnImport.Visible = true;
                }
                tab2.Visible = false;
                tab5.Visible = false;
                break;

            case "F15601":
                btnImport.Visible = false;
                tab2.Visible = false;
                tab4.Visible = false;
                tab5.Visible = false;
                break;
        }
        if (Prg_Id == "M12008")
        {
            tab5.Visible = true;
            txtlbl8.Attributes.Remove("readonly");
            txtlbl9.Attributes.Remove("readonly");
        }
        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false; //BY MADHVI ON 23 JULY 2018
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
        btnprint.Disabled = true; btnlist.Disabled = true; //BY MADHVI ON 23 JULY 2018
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

            case "F15106*":
                frm_tabname = "POMAS";
                break;
            case "F15106":
                frm_tabname = "POMAS";
                break;
            case "F15601":
                frm_tabname = "WB_PORFQ";
                break;
        }
        frm_famtbl = "FAMST";
        if (Prg_Id == "F15601")
        {
            frm_famtbl = "wbvu_fam_vend";
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        btnPayDays.Visible = false;
        if (frm_ulvl == "M")
        {
            if (txtlbl4.Text.Trim().Length < 3)
            {
                txtlbl4.Text = frm_UserID;
                txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM " + frm_famtbl + " WHERE ACODE='" + txtlbl4.Text.Trim() + "'", "ANAME");
                setVendorDetails();
            }
            btnlbl4.Visible = false;
        }

        PR_buttn = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PR_BUTTN");
        if (PR_buttn == "N")
        {
            btnPR.Visible = false;
        }
        else btnPR.Visible = true;

        // date lock
        CalendarExtender2.StartDate = DateTime.Now;
        CalendarExtender2.EndDate = DateTime.Now.AddYears(2);
        CalendarExtender3.StartDate = DateTime.Now;
        CalendarExtender3.EndDate = DateTime.Now.AddYears(2);
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
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  as Code FROM TYPE WHERE ID='G' and substr(type1,1,1)='1'";
                break;
            case "BTN_11":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  as Code FROM TYPE WHERE ID='G' and substr(type1,1,1)='2'";
                break;
            case "BTN_12":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  as Code FROM TYPE WHERE ID='G' and substr(type1,1,1)='3'";
                break;
            case "BTN_13":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  as Code FROM TYPE WHERE ID='G' and substr(type1,1,1)='4'";
                break;
            case "BTN_14":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  as Code FROM TYPE WHERE ID='H' and substr(type1,1,1)='1'";
                break;
            case "BTN_15":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  as Code FROM TYPE WHERE ID='A'   order by type1";
                break;
            case "BTN_16":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  as Code FROM TYPE WHERE ID='G' and substr(type1,1,1)='1' order by name";
                break;
            case "BTN_17":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  as Code FROM TYPE WHERE ID='H' and substr(type1,1,1)='0' order by name";
                break;
            case "BTN_18":
                SQuery = "SELECT type1 as fstr,NAME,TYPE1  as Code FROM TYPE WHERE ID='H' and substr(type1,1,1)='1' order by name";
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
                string acodeCond = "trim(GRP) in ('02','05','06') and ";
                SQuery =  "SELECT ACODE AS FSTR,ANAME AS PARTY,ACODE AS CODE,ADDR1,ADDR2,staten as state,Pay_num,trim(nvl(gst_no,'-')) as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + " FROM " + frm_famtbl + " where " + acodeCond + "  length(Trim(nvl(deac_by,'-')))<=1 and length(trim(nvl(staten,'-')))>1 and (case when length(nvl(showinbr,'-'))>1 then instr(nvl(showinbr,'-'),'" + frm_mbr + "') else 1 end)>0  ORDER BY aname ";
                if (frm_vty == "54")
                {
                    SQuery = "SELECT ACODE AS FSTR,ANAME AS PARTY,ACODE AS CODE,ADDR1,ADDR2,staten as state,Pay_num,trim(nvl(gst_no,'-')) as " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + " FROM " + frm_famtbl + " where " + acodeCond + "  length(Trim(nvl(deac_by,'-')))<=1 and (case when length(nvl(showinbr,'-'))>1 then instr(nvl(showinbr,'-'),'" + frm_mbr + "') else 1 end)>0  ORDER BY aname ";
                }

                break;
            case "TICODE":
                //pop2
                SQuery = "SELECT ACODE AS FSTR,ANAME AS PARTY,ACODE AS CODE,ADDR1,ADDR2 FROM CSMST where branchcd!='DD' ORDER BY aname ";
                //SQuery = "SELECT ICODE AS FSTR,INAME AS PRODUCT,ICODE AS CODE,UNIT,CPARTNO AS PARTNO FROM ITEM WHERE LENGTH(tRIM(ICODE))>4 ";
                break;
            case "TICODEX":
                SQuery = "select type1,NAME as Department,type1 as Code from type where id='M' and substr(type1,1,1) in('6') order by name ";
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

                itemRepeat = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMREPEAT");
                if (col1.Length <= 0) col1 = "'-'";
                if (itemRepeat == "Y") col1 = "'-'";
                if (frm_cocd == "MULT") col1 = "'-'";
                if (frm_formID == "F15601")
                {
                    SQuery = "SELECT trim(Icode) AS FSTR,Iname AS Item_Name,trim(Icode) AS erp_code,Maker,Cpartno AS Part_no,Irate,Cdrgno AS Drg_no,unit,hscode,nvl(iweight,1) as iweight FROM Item where branchcd!='DD' and length(Trim(nvl(deac_by,'-')))<2  and length(Trim(icode))>4 and trim(icode) not in (" + col1 + ") and substr(icode,1,1)!='9' ORDER BY Iname";
                }
                else
                {
                    cond = "";
                    if (lbl1a.Text == "51" && frm_IndType == "06")
                    {
                        cond = " and substr(icode,1,2)!='02'";
                    }
                    if (lbl1a.Text == "53")
                    {
                        SQuery = "SELECT Icode as fstr,Iname as Item_Name,icode as ERP_Code,Maker,Cpartno AS Part_no,Irate,Cdrgno AS Drg_no,Unit,hscode,nvl(iweight,1) as iweight,'-' AS Remarks,'-' as Reqd_Dt,0 as bal_qty FROM Item WHERE length(Trim(icode))>4 and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 and length(Trim(nvl(hscode,'-')))>1 ORDER BY Iname  ";
                    }
                    else
                    {

                        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_PR_LIN_SYS") == "Y")
                        {
                            //Non line no. query
                            SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,nvl(b.iweight,1) as iweight ,trim(a.ERP_code) as ERP_code,b.Maker,b.Cpartno as Part_no,b.Irate,b.Cdrgno,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,b.hscode,substr(a.Fstr,10,6) as PR_No,(a.bank) as Deptt,(a.delv_item) as Reqd_Dt,(a.desc_) as Remarks,trim(a.Fstr) as PR_link,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Req_Qty from (select fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,max(bank) as bank,max(delv_item) As delv_item,max(desc_) as desc_ from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode)||'-'||trim(pr_srn) as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,nvl(bank,'-') As bank,nvl(delv_item,'-') As delv_item,nvl(desc_,'-') as desc_ from pomas where branchcd='" + frm_mbr + "' and type='60' and nvl(pflag,0)!=0 and trim(app_by)!='-' and orddt>=to_Date('01/04/2017','dd/mm/yyyy') " + cond + " union all SELECT to_ChaR(pr_Dt,'YYYYMMDD')||'-'||pr_no||'-'||trim(Icode)||'-'||trim(pr_srn) as fstr,trim(Icode) as ERP_code,0 as Qtyord,qtyord,null as bank,null as delv_item,null as desc_ from pomas where branchcd='" + frm_mbr + "' and type like '" + lbl1a.Text.Substring(0, 1) + "%' and orddt>=to_Date('01/04/2017','dd/mm/yyyy'))  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and trim(a.erp_code) not in (" + col1 + ") order by B.Iname,trim(a.fstr)";
                        }
                        else
                        {
                            SQuery = "SELECT Icode as fstr,Iname as Item_Name,icode as ERP_Code,Maker,Cpartno AS Part_no,Irate,Cdrgno AS Drg_no,Unit,hscode,nvl(iweight,1) as iweight,'-' AS Remarks,'-' as Reqd_Dt,0 as bal_qty FROM Item WHERE length(Trim(icode))>4  and trim(icode) not in (" + col1 + ") " + cond + " and length(Trim(nvl(deac_by,'-')))<=1 and length(Trim(nvl(hscode,'-')))>1 ORDER BY Iname  ";
                        }

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
                Type_Sel_query();
                break;
            case "BTNDELREQ":
                SQuery = "SELECT NAME as fstr,NAME,TYPE1  as Code FROM TYPE WHERE ID='G' and substr(type1,1,1)='3' order by name";
                break;
            case "BTNDELMODE":
                SQuery = "SELECT NAME as fstr,NAME,TYPE1  as Code FROM TYPE WHERE ID='G' and substr(type1,1,1)='2' order by name";
                break;
            case "BTNPAYMODE":
            case "BTNPAYDAYS":
                SQuery = "SELECT NAME as fstr,NAME,TYPE1  as Code FROM TYPE WHERE ID='G' and substr(type1,1,1)='4' order by name";
                break;
            case "SG1_ROW_DT":
            case "sg1_t3":
                string curr_itm = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text.Trim();
                SQuery = "Select a.ordno||to_Char(a.orddt,'dd/mm/yyyy') as FStr,b.iname,b.Maker,a.prate,a.pdisc,a.qtyord,a.app_by,to_char(a.app_dt,'dd/mm/yyyy') as app_dt,a.ent_by,a.ordno as po_no,to_char(a.orddt,'dd/mm/yyyy') as po_dt  from pomas a, item b where trim(a.icode)=trim(B.icode) and a.type like '5%' and a.icode ='" + curr_itm + "' order by a.orddt desc ";
                break;
            default:
                cond = frm_ulvl == "M" ? " and trim(a.acode)='" + frm_uname + "'" : "";
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                {
                    if (frm_formID == "F15601")
                    {
                        SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Doc_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Doc_Dt,b.Aname as Vendor,b.Staten,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.Chk_by,a.App_by,(case when nvl(a.pflag,0)=1 then 'Closed' else 'Active' end) as RFQ_Stat,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a," + frm_famtbl + " b where  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " and  trim(a.acode)=trim(B.acodE) " + cond + " order by vdd desc,a." + doc_nf.Value + " desc";
                    }
                    else
                    {
                        SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Doc_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Doc_Dt,b.Aname as Vendor,b.Staten,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.Chk_by,a.App_by,(case when nvl(a.pflag,0)=1 then 'Closed' else 'Active' end) as Po_Stat,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a," + frm_famtbl + " b where  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " and  trim(a.acode)=trim(B.acodE) " + cond + " order by vdd desc,a." + doc_nf.Value + " desc";
                    }
                    if (btnval == "COPY_OLD")
                        SQuery = "select distinct trim(A." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy') as fstr,a." + doc_nf.Value + " as Doc_no,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Doc_Dt,b.Aname as Vendor,b.Staten,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_dt,a.Chk_by,a.App_by,(case when nvl(a.pflag,0)=1 then 'Closed' else 'Active' end) as Po_Stat,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a," + frm_famtbl + " b where  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a." + doc_df.Value + " " + DateRange + " and  trim(a.acode)=trim(B.acodE) " + cond + " order by vdd desc,a." + doc_nf.Value + " desc";
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
            hffield.Value = "New";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Type", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");


        string chk_curren = "";
        chk_curren = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT br_Curren FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "br_Curren");
        if (chk_curren.Length < 2)
        {
            fgen.msg("-", "AMSG", "Please update Currency in Branch Master!!");
            return;
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
        string orig_vchdt;
        orig_vchdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OLD_DATE");
        if (edmode.Value != "Y")
        {
            orig_vchdt = txtvchdate.Text;
        }
        if (frm_formID != "F15601")
        {

            string chk_freeze = "";
            chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1012", txtvchdate.Text.Trim());
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
            if (frm_formID == "F15601")
            {
                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) <= 0)
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");
                    i = sg1.Rows.Count;
                    return;
                }
            }
            else
            {
                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) <= 0 && (lbl1a.Text == "51" || lbl1a.Text == "54"))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");
                    i = sg1.Rows.Count;
                    return;

                }
            }
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text) <= 0)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Rate Not Filled Correctly at Line " + (i + 1) + "  !!");
                i = sg1.Rows.Count;
                return;

            }

            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Length < 10)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Date of Delivery Not Filled Correctly at Line " + (i + 1) + "  !!");
                i = sg1.Rows.Count;
                return;
            }
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Length > 5)
            {
                if (Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text) < Convert.ToDateTime(txtvchdate.Text.Trim()))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Date of Delivery Can Not be less then P.O. Date at Line " + (i + 1) + " !!");
                    i = sg1.Rows.Count;
                    return;
                }
            }

            if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text) >= 101)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Discount Percentage can not be 100% or more then 100% at Line " + (i + 1) + "  !!");
                i = sg1.Rows.Count;
                return;
            }
        }

        string last_entdt;
        //checks

        if (edmode.Value == "Y")
        {
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  and orddt " + DateRange + " and ordno||to_char(orddt,'dd/mm/yyyy')!='" + txtvchnum.Text + orig_vchdt + "' and orddt<=to_DaTE('" + orig_vchdt + "','dd/mm/yyyy') order by orddt desc", "ldt");
        }
        else
        {
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  and orddt " + DateRange + " and ordno||to_char(orddt,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "' order by orddt desc", "ldt");
        }

        if (last_entdt == "0")
        { }
        else
        {
            if (frm_cocd == "KRSM") { }
            else
            {
                if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                    return;

                }
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
        checkGridQty();

        string ok_for_save;
        ok_for_save = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_CHK");
        string err_item;
        err_item = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OQTY_ITM");

        // is this to be here 
        if (frm_formID != "F15601")
        {
            if (frm_cocd != "MINV")
            {
                if (ok_for_save == "N")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' PO Qty is Exceeding PR Qty , Please Check item '13' " + err_item);
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
        if (frm_formID != "F15601")
        {
            string chk_mrr_made;
            chk_mrr_made = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum||' Dt.'||to_char(vchdate,'dd/mm/yyyy') As fstr from ivoucher where branchcd||potype||trim(ponum)||to_char(podate,'dd/mm/yyyy')='" + frm_mbr + lbl1a.Text + txtvchnum.Text + orig_vchdt + "'", "fstr");
            if (chk_mrr_made.Length > 6)
            {
                hffield.Value = "CHKUSER";
                fgen.msg("-", "SMSG", "Dear " + frm_uname + ", MRR/GRN no ." + chk_mrr_made + " Already Made Against This P.O. , Editing is Not Allowed !!");
                return;
            }
        }
        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        btnsave.Disabled = true;
    }
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
            string mqry;
            mqry = "select (a.Qtyord)-(a.Soldqty) as Bal_Qty from (select fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,0 as prate from pomas where branchcd='" + frm_mbr + "' and type like '60%' and trim(pflag)!=0  and orddt>=to_Date('01/04/2017','dd/mm/yyyy')  union all SELECT trim(icode)||'-'||to_ChaR(orddt,'YYYYMMDD')||'-'||ordno as fstr,trim(Icode) as ERP_code,0 as Qtyord,qtyord ,0 as irate from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(ordno)||to_Char(orddt,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and a.fstr='" + drQty1["fstr"].ToString().Trim() + "' order by B.Iname,trim(a.fstr)";
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, mqry, "Bal_Qty");

            if (fgen.make_double(sm.ToString()) > fgen.make_double(col1) && fgen.make_double(col1) > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", drQty1["fstr"].ToString().Trim());
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
        //  fgen.Fn_open_sseek("Select Type for Print", frm_qstr);// COMMENTED BY MADHVI ON 28 JULY 2018
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Type", frm_qstr); // BY MADHVI ON 28 JULY 2018
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

                string chk_mrr_made;
                chk_mrr_made = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum||' Dt.'||to_char(vchdate,'dd/mm/yyyy') As fstr from ivoucher where branchcd||potype||trim(ponum)||to_char(podate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'", "fstr");
                if (chk_mrr_made.Length > 6)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", MRR/GRN no ." + chk_mrr_made + " Already Made Against This P.O. , Deletion is Not Allowed !!");
                    return;
                }

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
                    txtvty.Text = col1;
                    cond = (fgenMV.Fn_Get_Mvar(frm_qstr, "U_RUNNO") == "Y") ? "AND SUBSTR(TYPE,1,1)='" + frm_vty.Substring(0, 1) + "' " : "AND TYPE='" + frm_vty + "'";
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' " + cond + " AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");

                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

                    txtlbl15.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT br_Curren FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "br_Curren");

                    if (txtlbl15.Text.Length < 2 && doc_GST.Value != "GCC")
                    {
                        txtlbl15.Text = "INR";
                    }

                    col1 = "";
                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT Type1||'~'||NAME AS Deptt,Type1 AS CODE FROM type where id='M' and trim(Type1) in (select trim(erpdeptt) as fstr from EVAS WHERE USERNAME='" + frm_uname + "' ) ", "Deptt");
                    if (col1.Length > 5)
                    {
                        txtlbl70.Text = col1.Split('~')[0];
                        txtlbl71.Text = col1.Split('~')[1];
                    }

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
                    var ordno = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT FSTR FROM (select to_Char(A.orddt,'yyyymmdd')||Trim(A.ordno) fstr from" +
                        " pomas a ORDER BY 1 DESC) K WHERE ROWNUM=1  ", "fstr");
                    var dttt = fgen.getdata(frm_qstr, frm_cocd, "select TERMS ,CONDI  from POTERM where TO_CHAR(VCHDATE,'yyyymmdd')||TRIM(VCHNUM) ='" + ordno + "' ");
                    foreach (DataRow dr in dttt.Rows)
                    {

                        sg2_dr = sg2_dt.NewRow();


                        sg2_dr["sg2_SrNo"] = sg2_dt.Rows.Count + 1;
                        sg2_dr["sg2_t1"] = dr["TERMS"].ToString();
                        sg2_dr["sg2_t2"] = dr["CONDI"].ToString();
                        sg2_dt.Rows.Add(sg2_dr);

                    }
                    sg2_add_blankrows();
                    //sg2_add_blankrows();
                    //sg2_add_blankrows();
                    //sg2_add_blankrows();
                    //sg2_add_blankrows();
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
                    string copy_fac = "N";
                    copy_fac = fgen.getOption(frm_qstr, frm_cocd, "W0065", "OPT_ENABLE");
                    if (copy_fac == "Y")
                    {
                        fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
                        hffield.Value = "NEW_E";
                    }

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
                    txtvty.Text = col1;
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
                    fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();

                    string mv_col;
                    mv_col = frm_mbr + frm_vty + col1;

                    SQuery = "Select a.*,to_Char(nvl(a.pr_Dt,a.orddt),'yyyymmdd')||'-'||trim(A.pr_no)||'-'||trim(a.pr_srn) As pr_Dt1,to_Char(nvl(a.del_date,a.orddt),'dd/mm/yyyy') As del_date1,to_Char(nvl(a.porddt,a.orddt),'dd/mm/yyyy') As porddt1,to_Char(a.effdate,'dd/mm/yyyy') As eff_Dt1,to_Char(a.validupto,'dd/mm/yyyy') As val_Dt1,b.iname,c.Aname,trim(nvl(c.gst_no,'-')) as Gst_no,nvl(b.cpartno,'-') As Icpartno,nvl(b.cdrgno,'-') as Icdrgno,nvl(b.unit,'-') as Unit from " + frm_tabname + " a,item b," + frm_famtbl + " c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        hfapby.Value = dt.Rows[0]["app_by"].ToString();

                        if (fgen.make_double(frm_ulvl) > 2 && dt.Rows[i]["app_by"].ToString().Trim() != "-")
                        {
                            fgen.msg("-", "AMSG", "Approved Order Cannot be Edited, Contact HOD/Admin");
                            return;
                        }

                        txtvchnum.Text = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OLD_DATE", txtvchdate.Text);

                        mgst_no = dt.Rows[i]["gst_no"].ToString().Trim();

                        if (mgst_no.Length > 10)
                        {
                            txtTax.Text = "Y";
                        }
                        else
                        {
                            txtTax.Text = "N";
                        }


                        txtAmd.Text = dt.Rows[i]["amdtno"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[i]["pordno"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["porddt1"].ToString().Trim();

                        txtlbl4.Text = dt.Rows[i]["Acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();

                        txtlbl5.Text = dt.Rows[i]["delv_term"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["mode_tpt"].ToString().Trim();

                        txtlbl7.Text = dt.Rows[0]["cscode1"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname  from csmst where trim(upper(acode))=upper(Trim('" + txtlbl7.Text + "'))", "aname");

                        txtlbl71.Text = dt.Rows[i]["INST"].ToString().Trim();
                        txtlbl70.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT type1 FROM TYPE WHERE ID='M' AND Name='" + txtlbl71.Text + "'", "type1");
                        txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");
                        txtlbl73.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM " + frm_famtbl + " WHERE trim(acode)='" + txtlbl4.Text.Trim() + "'", "STATEn");

                        txtlbl8.Text = dt.Rows[i]["doc_thr"].ToString().Trim();
                        txtlbl9.Text = dt.Rows[i]["payment"].ToString().Trim();

                        txtrmk.Text = dt.Rows[i]["term"].ToString().Trim();



                        txtlbl15.Text = dt.Rows[i]["currency"].ToString().Trim();
                        txtlbl16.Text = dt.Rows[i]["pbasis"].ToString().Trim();
                        txtlbl17.Text = dt.Rows[i]["tr_insur"].ToString().Trim();
                        txtlbl18.Text = dt.Rows[i]["freight"].ToString().Trim();

                        txtlbl24.Text = dt.Rows[i]["wk3"].ToString().Trim();
                        //txtlbl26.Text = dt.Rows[i]["basic"].ToString().Trim();
                        txtlbl28.Text = dt.Rows[i]["eff_Dt1"].ToString().Trim();
                        txtlbl30.Text = dt.Rows[i]["val_Dt1"].ToString().Trim();


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

                            sg1_dr["sg1_f4"] = dt.Rows[i]["pr_Dt1"].ToString().Trim();

                            //sg1_dr["sg1_f5"] = dt.Rows[i]["Unit"].ToString().Trim();
                            sg1_dr["sg1_f5"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[i]["icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'") + " / " + dt.Rows[i]["unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["desc_"].ToString().Trim();
                            //sg1_dr["sg1_t2"] = dt.Rows[0]["del_Date1"].ToString().Trim();
                            sg1_dr["sg1_t2"] = Convert.ToDateTime(fgen.make_def_Date(dt.Rows[i]["delv_item"].ToString().Trim(), vardate)).ToString("yyyy-MM-dd");

                            sg1_dr["sg1_t3"] = dt.Rows[i]["qtyord"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["prate"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["pdisc"].ToString().Trim();

                            sg1_dr["sg1_t7"] = dt.Rows[i]["pexc"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["pcess"].ToString().Trim();

                            sg1_dr["sg1_t9"] = dt.Rows[i]["ciname"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["splrmk"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["rate_comm"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["cscode"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["del_Sch"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["po_tolr"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["wk1"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["qtybal"].ToString().Trim();
                            sg1_dr["sg1_t17"] = dt.Rows[i]["issue_no"].ToString().Trim();
                            string app_rt;
                            app_rt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT irate||'#'||disc as skfstr FROM appvendvch WHERE branchcd='" + frm_mbr + "' and trim(acode)='" + txtlbl4.Text + "' and trim(Icode)='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "skfstr");
                            if (app_rt != "0")
                            {
                                sg1_dr["sg1_h3"] = "APL";
                            }

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
                        edmode.Value = "Y";
                        if (fgen.make_double(frm_ulvl) >= 1) btnlbl4.Enabled = false;
                    }
                    #endregion
                    break;
                case "COPY_OLD":
                    #region Copy from old
                    if (col1 == "") return;
                    clearctrl();

                    mv_col = "";
                    mv_col = frm_mbr + frm_vty + col1;

                    SQuery = "Select a.*,to_Char(nvl(a.pr_Dt,a.orddt),'yyyymmdd')||'-'||trim(A.pr_no) As pr_Dt1,to_Char(nvl(a.del_date,a.orddt),'dd/mm/yyyy') As del_date1,to_Char(nvl(a.porddt,a.orddt),'dd/mm/yyyy') As porddt1,to_Char(a.effdate,'dd/mm/yyyy') As eff_Dt1,to_Char(a.validupto,'dd/mm/yyyy') As val_Dt1,b.iname,c.Aname,trim(nvl(c.gst_no,'-')) as Gst_no,nvl(b.cpartno,'-') As Icpartno,nvl(b.cdrgno,'-') as Icdrgno,nvl(b.unit,'-') as Unit from " + frm_tabname + " a,item b," + frm_famtbl + " c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_Char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        hfapby.Value = "-";

                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OLD_DATE", txtvchdate.Text);

                        mgst_no = dt.Rows[i]["gst_no"].ToString().Trim();

                        if (mgst_no.Length > 10)
                        {
                            txtTax.Text = "Y";
                        }
                        else
                        {
                            txtTax.Text = "N";
                        }


                        txtAmd.Text = dt.Rows[i]["amdtno"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[i]["pordno"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["porddt1"].ToString().Trim();

                        txtlbl4.Text = dt.Rows[i]["Acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();

                        txtlbl5.Text = dt.Rows[i]["delv_term"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["mode_tpt"].ToString().Trim();

                        txtlbl7.Text = dt.Rows[0]["cscode1"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname  from csmst where trim(upper(acode))=upper(Trim('" + txtlbl7.Text + "'))", "aname");

                        txtlbl71.Text = dt.Rows[i]["INST"].ToString().Trim();
                        txtlbl70.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT type1 FROM TYPE WHERE ID='M' AND Name='" + txtlbl71.Text + "'", "type1");
                        txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");
                        txtlbl73.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEn FROM " + frm_famtbl + " WHERE trim(acode)='" + txtlbl4.Text.Trim() + "'", "STATEn");

                        txtlbl8.Text = dt.Rows[i]["doc_thr"].ToString().Trim();
                        txtlbl9.Text = dt.Rows[i]["payment"].ToString().Trim();

                        txtrmk.Text = dt.Rows[i]["term"].ToString().Trim();



                        txtlbl15.Text = dt.Rows[i]["currency"].ToString().Trim();
                        txtlbl16.Text = dt.Rows[i]["pbasis"].ToString().Trim();
                        txtlbl17.Text = dt.Rows[i]["tr_insur"].ToString().Trim();
                        txtlbl18.Text = dt.Rows[i]["freight"].ToString().Trim();

                        txtlbl24.Text = dt.Rows[i]["wk3"].ToString().Trim();
                        //txtlbl26.Text = dt.Rows[i]["basic"].ToString().Trim();
                        txtlbl28.Text = dt.Rows[i]["eff_Dt1"].ToString().Trim();
                        txtlbl30.Text = dt.Rows[i]["val_Dt1"].ToString().Trim();


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
                            sg1_dr["sg1_f4"] = dt.Rows[i]["pr_Dt1"].ToString().Trim();
                            sg1_dr["sg1_f5"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[i]["icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'") + " / " + dt.Rows[i]["unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["desc_"].ToString().Trim();
                            //sg1_dr["sg1_t2"] = dt.Rows[0]["del_Date1"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["delv_item"].ToString().Trim();

                            sg1_dr["sg1_t3"] = dt.Rows[i]["qtyord"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["prate"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["pdisc"].ToString().Trim();

                            sg1_dr["sg1_t7"] = dt.Rows[i]["pexc"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["pcess"].ToString().Trim();

                            sg1_dr["sg1_t9"] = dt.Rows[i]["ciname"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["splrmk"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["rate_comm"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["cscode"].ToString().Trim();
                            sg1_dr["sg1_t13"] = dt.Rows[i]["del_Sch"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["po_tolr"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["wk1"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["qtybal"].ToString().Trim();
                            sg1_dr["sg1_t17"] = dt.Rows[i]["issue_no"].ToString().Trim();

                            string app_rt;
                            app_rt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT irate||'#'||disc as skfstr FROM appvendvch WHERE branchcd='" + frm_mbr + "' and trim(acode)='" + txtlbl4.Text + "' and trim(Icode)='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "skfstr");
                            if (app_rt != "0")
                            {
                                sg1_dr["sg1_h3"] = "APL";
                            }

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

                        if (fgen.make_double(frm_ulvl) >= 1) btnlbl4.Enabled = false;
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
                    if (frm_formID == "F15601")
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F15601");
                    }
                    else
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1004");
                        //if (frm_cocd == "DREM" || frm_cocd == "PPCL") 
                        fgenMV.Fn_Set_Mvar(frm_qstr, "USEND_MAIL", "Y");
                    }
                    fgen.fin_purc_reps(frm_qstr);
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;

                    lblGSTinfo.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TAXVAR") + " : " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL8");
                    setVendorDetails();
                    if (sg1.Rows.Count > 0)
                    {
                        dt4 = new DataTable();
                        dt4 = fgen.getdata(frm_qstr, frm_cocd, "SELECT trim(ACREF) as ACREF,NUM4,NUM5,NUM6 FROM TYPEGRP WHERE ID='T1' ORDER BY ACREF ");
                        foreach (GridViewRow gr1 in sg1.Rows)
                        {
                            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT HSCODE FROM ITEM WHERE TRIM(ICODE)='" + gr1.Cells[13].Text.Trim().ToUpper() + "'", "HSCODE");

                            col2 = fgen.seek_iname_dt(dt4, "acref='" + col1 + "'", "acref");
                            if (col2 != "0")
                            {
                                try
                                {
                                    if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                                    {
                                        ((TextBox)gr1.FindControl("sg1_t7")).Text = fgen.seek_iname_dt(dt4, "acref='" + col1 + "'", "num4");
                                        ((TextBox)gr1.FindControl("sg1_t8")).Text = fgen.seek_iname_dt(dt4, "acref='" + col1 + "'", "num5");
                                    }
                                    else
                                    {
                                        ((TextBox)gr1.FindControl("sg1_t7")).Text = fgen.seek_iname_dt(dt4, "acref='" + col1 + "'", "num6");
                                        ((TextBox)gr1.FindControl("sg1_t8")).Text = "0";
                                    }
                                    if (doc_GST.Value == "GCC")
                                    {
                                        ((TextBox)gr1.FindControl("sg1_t7")).Text = fgen.seek_iname_dt(dt4, "acref='" + col1 + "'", "num6");
                                        ((TextBox)gr1.FindControl("sg1_t8")).Text = "0";
                                    }
                                }
                                catch { }
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
                    string repItem = "";
                    string repItemAl = "";
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

                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();

                        String pop_qry;
                        string balfield = "0";
                        balfield = "a.bal_Qty";
                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");
                        if (frm_formID == "F15601")
                        {
                            SQuery = "select a.fstr,a.ERP_code as icode,a.iweight,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,'-' as fo_no from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in (" + col1 + ") order by a.Item_Name";
                        }
                        else
                        {
                            if (lbl1a.Text == "52" || lbl1a.Text == "53")
                            {
                                if (col1.Trim().Length == 8) SQuery = "select distinct 1 as iweight,a.icode,a.iname,a.cpartno,a.cdrgno,a.unit,'-' as remarks,'-' as Reqd_Dt,0 as bal_Qty,a.hscode,b.num4,b.num5,b.num6,b.num7,'-' as fo_no from item a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.icode) in ('" + col1 + "')";
                                else SQuery = "select distinct 1 as iweight,a.cdrgno as fstr,a.icode,a.iname,a.cpartno,a.cdrgno,a.unit,'-' as remarks,'-' as Reqd_Dt,0 as bal_Qty,a.hscode,b.num4,b.num5,b.num6,b.num7,'-' as fo_no from item a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.icode) in (" + col1 + ") order by a.iname";
                            }
                            else
                            {
                                if (col1.Trim().Length == 8) SQuery = "select a.fstr,a.ERP_code as icode,a.iweight,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno," + balfield + " as bal_qty,Reqd_Dt,a.Remarks,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,'-' as fo_no from (" + pop_qry + ") a left outer join typegrp b on trim(a.hscode)=trim(b.acref) and b.id='T1' where trim(a.fstr) in ('" + col1 + "')";
                                else SQuery = "select a.fstr,a.ERP_code as icode,a.iweight,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno," + balfield + " as bal_qty,Reqd_Dt,a.Remarks,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,'-' as fo_no from (" + pop_qry + ") a left outer join typegrp b on trim(a.hscode)=trim(b.acref) and b.id='T1' where trim(a.fstr) in (" + col1 + ") order by a.Item_Name";
                            }
                            if (hfpr.Value == "PR")
                            {
                                if (col1.Trim().Length == 8) SQuery = "select a.fstr,a.ERP_code as icode,a.iweight,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno," + balfield + " as bal_qty,Reqd_Dt,a.Remarks,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.fo_no from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in ('" + col1 + "')";
                                else SQuery = "select a.fstr,a.ERP_code as icode,a.iweight,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno," + balfield + " as bal_qty,Reqd_Dt,a.Remarks,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.fo_no from (" + pop_qry + ") a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.fstr) in (" + col1 + ") order by a.Item_Name";
                            }
                        }
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        if (dt.Rows.Count <= 0)
                        {
                            if (col1.Trim().Length == 8) SQuery = "select a.fstr,a.ERP_code as icode,a.iweight,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,0 as bal_qty,null as Reqd_Dt,a.Remarks,a.unit,a.hscode,0 as num4,0 as num5,0 as num6,0 as num7,'-' as fo_no from (" + pop_qry + ") a where trim(a.fstr) in ('" + col1 + "') order by a.Item_Name";
                            else SQuery = "select a.fstr,a.ERP_code as icode,a.iweight,a.Item_Name as iname,a.Part_no as cpartno,'-' as cdrgno,a.Bal_Qty as bal_qty,a.Reqd_Dt as Reqd_Dt,a.Remarks,a.unit,a.hscode,0 as num4,0 as num5,0 as num6,0 as num7,'-' as fo_no from (" + pop_qry + ") a where trim(a.fstr) in (" + col1 + ") order by a.Item_Name";

                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            if (dt.Rows.Count <= 0)
                            {
                                fgen.msg("-", "AMSG", "Please Check HSN Code of ERP Code : " + col3.Replace("'", ""));
                                return;
                            }
                        }
                        col3 = "";
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            repItemAl = "Y";
                            if (lbl1a.Text == "52" || lbl1a.Text == "53")
                            {
                                if (dt.Rows[d]["fstr"].ToString().Trim().Length > 16)
                                    col3 = fgen.seek_iname_dt(sg1_dt, "sg1_f4='" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 16) + "'", "sg1_f4");
                            }
                            else
                            {
                                if (dt.Rows[d]["fstr"].ToString().Trim().Length > 27)
                                {
                                    col3 = fgen.seek_iname_dt(sg1_dt, "sg1_f4='" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 16) + dt.Rows[d]["fstr"].ToString().Trim().Substring(25, 3) + "'", "sg1_f4");
                                }
                                else
                                {
                                    if (dt.Rows[d]["fstr"].ToString().Trim().Length > 15)
                                        col3 = fgen.seek_iname_dt(sg1_dt, "sg1_f4='" + dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 16) + "'", "sg1_f4");
                                }
                            }
                            if (col3 != "0" && col3 != "")
                            {
                                repItem = col3;
                                repItemAl = "N";
                            }
                            if (frm_cocd == "MULT")
                                repItemAl = "Y";
                            if (repItemAl == "Y")
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

                                sg1_dr["sg1_f5"] = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[d]["icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'") + " / " + dt.Rows[d]["unit"].ToString().Trim();

                                sg1_dr["sg1_t1"] = "-";
                                if (frm_formID == "F15601")
                                {
                                    sg1_dr["sg1_t2"] = "-";
                                    sg1_dr["sg1_t3"] = "-";
                                    sg1_dr["sg1_t10"] = "-";
                                    sg1_dr["sg1_t15"] = "-";
                                    sg1_dr["sg1_t4"] = "-";
                                    sg1_dr["sg1_t5"] = "-";
                                    sg1_dr["sg1_t16"] = "1";
                                    sg1_dr["sg1_t17"] = "-";
                                    sg1_dr["sg1_f4"] = "-";
                                    if (lbl1a.Text == "55")
                                    {
                                        sg1_dr["sg1_h6"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NO_PROC FROM ITEM WHERE ICODE='" + dt.Rows[d]["icode"].ToString().Trim() + "'", "NO_PROC");
                                        sg1_dr["sg1_t16"] = dt.Rows[d]["iweight"].ToString().Trim();
                                    }
                                }
                                else
                                {
                                    if (dt.Rows[d]["reqd_Dt"].ToString().Trim().Length > 5)
                                        sg1_dr["sg1_t2"] = Convert.ToDateTime(dt.Rows[d]["reqd_Dt"].ToString().Trim()).ToString("yyyy-MM-dd");
                                    sg1_dr["sg1_t3"] = dt.Rows[d]["bal_Qty"].ToString().Trim();
                                    sg1_dr["sg1_t10"] = dt.Rows[d]["remarks"].ToString().Trim();
                                    sg1_dr["sg1_t15"] = dt.Rows[d]["bal_Qty"].ToString().Trim();
                                    sg1_dr["sg1_t4"] = "";
                                    sg1_dr["sg1_t5"] = "";
                                    sg1_dr["sg1_t16"] = "1";

                                    if (lbl1a.Text == "52" || lbl1a.Text == "53")
                                    {
                                        sg1_dr["sg1_f4"] = dt.Rows[d]["fstr"].ToString().Trim();
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (dt.Rows[d]["fstr"].ToString().Trim().Length > 6)
                                                sg1_dr["sg1_f4"] = dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 15);
                                            if (dt.Rows[d]["fstr"].ToString().Trim().Length > 18)
                                                sg1_dr["sg1_f4"] = dt.Rows[d]["fstr"].ToString().Trim().Substring(0, 16) + dt.Rows[d]["fstr"].ToString().Trim().Substring(25, 3);
                                        }
                                        catch { }
                                    }

                                    if (hfpr.Value == "PR")
                                    {
                                        sg1_dr["sg1_f4"] = dt.Rows[d]["fstr"].ToString().Trim();
                                    }

                                    if (lbl1a.Text == "55")
                                    {
                                        sg1_dr["sg1_h6"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NO_PROC FROM ITEM WHERE ICODE='" + dt.Rows[d]["icode"].ToString().Trim() + "'", "NO_PROC");
                                        sg1_dr["sg1_t16"] = dt.Rows[d]["iweight"].ToString().Trim();
                                    }

                                    string app_rt;
                                    app_rt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT irate||'#'||disc as skfstr FROM appvendvch WHERE branchcd='" + frm_mbr + "' and trim(acode)='" + txtlbl4.Text + "' and trim(Icode)='" + dt.Rows[d]["icode"].ToString().Trim() + "'", "skfstr");
                                    if (app_rt != "0")
                                    {
                                        sg1_dr["sg1_t4"] = app_rt.Split('#')[0].ToString();
                                        sg1_dr["sg1_t5"] = app_rt.Split('#')[1].ToString();
                                        sg1_dr["sg1_t17"] = "APL";
                                        sg1_dr["sg1_h3"] = "APL";
                                    }
                                }
                                try
                                {
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
                                catch { }
                                if (lbl1a.Text == "54")
                                {
                                    sg1_dr["sg1_t7"] = "0";
                                    sg1_dr["sg1_t8"] = "0";

                                }
                                sg1_dr["sg1_t9"] = "";
                                sg1_dr["sg1_t10"] = "";
                                sg1_dr["sg1_t11"] = "";
                                sg1_dr["sg1_t12"] = "";
                                sg1_dr["sg1_t13"] = frm_cocd == "MULT" ? dt.Rows[d]["fo_no"].ToString().Trim() : "";
                                sg1_dr["sg1_t14"] = "";
                                sg1_dt.Rows.Add(sg1_dr);
                            }
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    hfpr.Value = "";
                    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();                    
                    #endregion
                    setColHeadings();
                    setGST();
                    if (repItem.Length > 0)
                    {
                        if (frm_cocd != "MULT")
                            fgen.msg("-", "AMSG", "Duplicate ERP Code Found!! " + repItem);
                    }
                    hfpr.Value = "";
                    break;
                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    if (edmode.Value == "Y")
                    {
                        //return;
                    }


                    //********* Saving in Hidden Field 
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    //********* Saving in GridView Value
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    if (lbl1a.Text == "52" || lbl1a.Text == "53")
                    {
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = col1;
                    }
                    else
                    {
                        try
                        {
                            if (col1.Trim().Length > 6)
                                sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = col1.Trim().Substring(0, 15);
                            if (col1.Trim().Length > 18)
                                sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = col1.Trim().Substring(0, 16) + col1.Trim().Substring(25, 3);
                        }
                        catch { }
                    }

                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", ""), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'") + " / " + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");

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
                    break;
                case "SG1_ROW_DT":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
                    break;
                case "sg1_t3":
                    if (col1.Length > 1)
                    {
                        txtlbl2.Text = col1.Substring(0, 6);
                        txtlbl3.Text = col1.Substring(6, 10);
                    }
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = col3;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Focus();
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
                case "BTNDELREQ":
                    txtlbl5.Text = col1;
                    break;
                case "BTNDELMODE":
                    txtlbl6.Text = col1;
                    break;
                case "BTNPAYMODE":
                    txtlbl8.Text = col1;
                    break;
                case "BTNPAYDAYS":
                    txtlbl9.Text = col1;
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", txtvty.Text);
        frm_vty = txtvty.Text;
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List" || hffield.Value == "PENDPR" || hffield.Value == "PENDPO" || hffield.Value == "STKINH" || hffield.Value == "APPVENL")
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

            switch (hffield.Value)
            {
                case "List":
                    SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, "F15126", "branchcd='" + frm_mbr + "'", "a.type='60' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'", PrdRange);
                    headerN = "Purchase Request Checklist for the Period " + fromdt + " to " + todt;
                    break;
                case "PENDPR":
                    xprd1 = " BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')";
                    SQuery = "select '-' as fstr,'-' as gstr,c.iname as Item_Name,c.cpartno,sum(a.poq) as PR_Qty,sum(a.rcvq) as PO_Qty,sum(a.poq)-sum(a.rcvq) as Bal_Qty,(Case when sum(a.poq)>0 then round(((sum(a.poq)-sum(a.rcvq))/sum(a.poq))*100,2) else 0 end) as bal_per,round(sysdate-a.orddt,0) as Pend_Days,c.unit,a.ordno,a.orddt,trim(a.icode) as ERP_Code,max(a.pordno)as Indentor,max(a.pflag)as pflag,sum(a.mrr_Qty) as mrr_Qty,a.branchcd from (Select branchcd,pflag,ordno,orddt,icode,qtyord as poq,0 as rcvq,0 as rej_Qty,pordno,psize,0 as mrr_qty from pomas where branchcd='" + frm_mbr + "' and type='60' and orddt " + xprd1 + " and icode like '" + party_cd + "%' union all Select branchcd,null as pflag,pr_no,pr_Dt,icode,0 as prq,qtyord as poq,0 as rej_rw,null as pordno,del_Sch,0 as mrr_Qty from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt " + xprd1 + "   and icode like '" + party_cd + "%' union all Select branchcd,null as pflag,prnum,rtn_date,icode,0 as prq,0 as poq,0 as rej_rw,null as pordno,null as del_Sch,iqty_chl from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + xprd1 + " and icode like '" + party_cd + "%' and store in('Y','N'))a,item c where trim(a.icode)=trim(C.icode) group by c.cpartno,c.iname,c.unit,a.branchcd,a.ordno,a.orddt,trim(a.icode) having sum(a.poq)-sum(a.rcvq)>0 and max(a.pflag)<>0 order by a.orddt,a.ordno";

                    fgen.drillQuery(0, SQuery, frm_qstr, "5#6#7#8#", "3#4#5#6#7#8#9#", "350#100#100#100#100#100#100#");
                    headerN = "Pending Purchase Requests (" + fromdt + " to " + todt + ")";
                    break;
                case "PENDPO":
                    xprd1 = " BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')";
                    string mq1 = "";
                    string mq2 = "";

                    mq1 = "select  '-' as fstr,'-' as gstr,c.iname as Item_Name,sum(a.poq) as PO_Qty,sum(a.rcvq) as MRR_Qty,sum(a.poq)-sum(a.rcvq) as Bal_Qty,sum(a.rej_Qty) as Rejn_Qty,(Case when sum(a.poq)>0 then round(((sum(a.poq)-sum(a.rcvq))/sum(a.poq))*100,2) else 0 end) as bal_per,round(sysdate-a.orddt,0) as Pend_Days,c.unit,a.branchcd,a.ordno,a.orddt,a.type,trim(a.icode) as ERP_Code,trim(a.acode) as Act_code,max(a.pflag)as pflag,max(a.del_Sch) as wo_no from (Select branchcd,pflag,ordno,orddt,acode,icode,qtyord as poq,0 as rcvq,0 as rej_Qty,type,del_Sch from pomas where branchcd='" + frm_mbr + "' and substr(type,1,1) ='5' and orddt " + xprd1 + " and  acode like '" + party_cd + "%' and icode like '" + part_cd + "%' union all Select branchcd,null as pflag,ponum,podate,acode,icode,0 as prq,iqtyin as poq,rej_rw,potype,null as del_Sch from ivoucher where branchcd='" + frm_mbr + "' and substr(type,1,1)='0' and potype like '5%' and vchdate " + xprd1 + " and store in ('Y','N')  ";
                    mq2 = "and  acode like '" + party_cd + "' and icode like '" + part_cd + "%'  )a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(a.icode)=trim(C.icode) group by c.iname,c.unit,b.aname,a.branchcd,a.type,a.ordno,a.orddt,trim(a.AcodE),trim(a.icode) having sum(a.poq)-sum(a.rcvq)>0 and max(a.pflag)!=1 order by b.aname,a.orddt,a.ordno";

                    SQuery = mq1 + mq2;
                    fgen.drillQuery(0, SQuery, frm_qstr, "4#5#6#7#8#", "3#4#5#6#7#8#", "350#100#100#100#100#100#");
                    headerN = "Pending Purchase Orders (" + fromdt + " to " + todt + ")";
                    break;
                case "STKINH":
                    SQuery = "select '-' as fstr,'-' as gstr,b.Iname,b.Cpartno,sum(a.opening) as Opening_Stock,sum(a.cdr) as Receipts,sum(a.ccr) as Issues,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stock,b.Unit,TRIM(A.ICODE) AS ERP_Code,max(a.imin) as Min_lvl,max(a.imax) as Max_lvl,max(a.iord) as ReOrder_lvl  from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where BRANCHCD='" + frm_mbr + "' and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,(nvl(iqtyin,0))-(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr,0 as imin,0 as imax,0 as iord FROM IVOUCHER where BRANCHCD='" + frm_mbr + "' and TYPE LIKE '%' AND VCHDATE " + xprd1 + " and store='Y' union all select branchcd,trim(icode) as icode,0 as op,(nvl(iqtyin,0)) as cdr,(nvl(iqtyout,0)) as ccr,0 as imin,0 as imax,0 as iord from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%'  AND VCHDATE " + xprd2 + " and store='Y') a,item b where trim(A.icode)=trim(B.icode) and a.icode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' and substr(A.icode,1,1)<'8'  GROUP BY b.Iname,b.Cpartno,b.Unit,TRIM(A.ICODE) ORDER BY trim(a.ICODE)";
                    headerN = "Stock Summary Report (" + fromdt + " to " + todt + ")";
                    fgen.drillQuery(0, SQuery, frm_qstr, "5#6#7#8#", "3#4#5#6#7#8#", "350#100#100#100#100#100#");
                    break;
                case "APPVENL":

                    SQuery = "select '-' as fstr,'-' as gstr,b.aName as Supplier,c.Iname as Item_Name,C.Cpartno,a.irate as APL_Rate,a.disc as APL_Disc,c.Unit,c.no_proc as Sec_Unit,a.row_text as Remarks,a.Icode,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.app_by,(Case when length(trim(nvl(a.app_by,'-')))<=1 then '-' else to_char(a.app_Dt,'dd/mm/yyyy') end) as app_dt,to_Char(a.vchdate,'yyyymmdd') as vdd,a.srno,a.Vchnum as APL_no,to_char(a.vchdate,'dd/mm/yyyy') as APL_Dt from Appvendvch a,famst b,item c where trim(A.acode)=trim(B.acode)  and trim(A.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='10' and a.acode like '" + party_cd + "' and a.icode like '" + part_cd + "%'   order by vdd ,a.vchnum ,a.srno";
                    headerN = "Approved Vendor List ";
                    fgen.drillQuery(0, SQuery, frm_qstr, "6#7#", "3#4#5#6#7#8#", "350#200#100#100#100#100#");
                    break;

            }
            fgen.Fn_DrillReport(headerN, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            if (hffield.Value == "CHKUSER")
            {
                if (frm_ulvl.Trim() == "0")
                {

                }
                else
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Only Owner level user can update the order, Please Check !!");
                    return;
                }
             
            }
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
                        oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oDS2 = new DataSet();
                        oporow2 = null;
                        //oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "ivchctrl");

                        oDS3 = new DataSet();
                        oporow3 = null;
                        oDS3 = fgen.fill_schema(frm_qstr, frm_cocd, "POTERM");

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
                                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_RUNNO") == "Y") frm_vnum = fgen.Fn_next_doc_no_inv(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, txtvchdate.Text.Trim(), frm_uname, Prg_Id);
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
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update poterm set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            //fgen.execute_cmd(frm_qstr, frm_cocd, "update budgmst set branchcd='DD' where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                            fgen.execute_cmd(frm_qstr, frm_cocd, "update udf_Data set branchcd='DD' where par_tbl='" + frm_tabname + "' and par_fld='" + ddl_fld1 + "'");

                        }

                        if (fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname) == "Y")
                        {
                            //fgen.save_data(frm_qstr, frm_cocd, oDS2, "ivchctrl");
                            fgen.save_data(frm_qstr, frm_cocd, oDS3, "poterm");
                            //fgen.save_data(frm_qstr, frm_cocd, oDS4, "budgmst");
                            fgen.save_data(frm_qstr, frm_cocd, oDS5, "udf_Data");

                            if (edmode.Value == "Y")
                            {

                                fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully'13'Do you want to see the Print Preview ?");
                                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivchctrl where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from poterm where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from budgmst where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr") + "'");
                                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from udf_Data where branchcd='DD' and par_tbl='" + frm_tabname + "' and par_fld='" + frm_mbr + ddl_fld2 + "'");
                            }
                            else
                            {
                                fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully'13'Do you want to see the Print Preview ?");
                            }
                            if (save_it == "Y")
                            {
                                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtvchdate.Text.Trim() + "'");


                                #region Email Sending Function
                                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                //html started                            
                                sb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
                                sb.Append("<h3>" + fgenCO.chk_co(frm_cocd) + "</h3>");
                                sb.Append("<br>Dear Sir/Mam,<br> This is to inform you that the following " + lblheader.Text + " has been saved by " + frm_uname + ".<br><br>");
                                sb.Append("<br>" + lbl1.Text.Trim() + " - Date : " + frm_vnum + " - " + txtvchdate.Text + "");
                                sb.Append("<br>Total Value of order : " + txtlbl31.Text + " <br><br>");
                                //table structure
                                sb.Append("<table border=1 cellspacing=1 cellpadding=1 style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; color: #474646'>");

                                sb.Append("<tr style='color: #FFFFFF; background-color: #0099FF; font-weight: 700; font-family: Arial, Helvetica, sans-serif'>" +
                                "<td><b>Srno</b></td><td><b>Item Code</b></td><td><b>Item Name</b></td><td><b>Quantity</b></td><td><b>Discount</b></td><td><b>Rate</b></td><td><b>Amount</b></td>");
                                i = 0;
                                foreach (GridViewRow gr in sg1.Rows)
                                {
                                    sb.Append("<tr>");
                                    sb.Append("<td>");
                                    sb.Append(i + 1);
                                    sb.Append("</td>");
                                    sb.Append("<td>");
                                    sb.Append(gr.Cells[13].Text);
                                    sb.Append("</td>");
                                    sb.Append("<td>");
                                    sb.Append(gr.Cells[14].Text);
                                    sb.Append("</td>");
                                    sb.Append("<td>");
                                    sb.Append(((TextBox)gr.FindControl("sg1_t3")).Text.Trim() + " " + gr.Cells[17].Text);
                                    sb.Append("</td>");
                                    sb.Append("<td>");
                                    sb.Append(((TextBox)gr.FindControl("sg1_t4")).Text.Trim());
                                    sb.Append("</td>");
                                    sb.Append("<td>");
                                    sb.Append(((TextBox)gr.FindControl("sg1_t5")).Text.Trim());
                                    sb.Append("</td>");
                                    sb.Append("<td>");
                                    sb.Append(((TextBox)gr.FindControl("sg1_t6")).Text.Trim());
                                    sb.Append("</td>");
                                    sb.Append("</tr>");
                                    i++;
                                }
                                sb.Append("</table></br></br>");

                                sb.Append("Thanks & Regards");
                                sb.Append("<h5>Note: This is an Auto generated Mail from Tejaxo ERP. The above details are to the best of information <br> and data available to the ERP System. For any discrepancy/ clarification kindly get in touch with the concerned official. </h5>");
                                sb.Append("</body></html>");

                                //send mail
                                string subj = "";
                                if (edmode.Value == "Y") subj = "Edited : ";
                                else subj = "New Entry : ";
                                fgen.send_Activity_mail(frm_qstr, frm_cocd, "Tejaxo ERP", frm_formID, subj + lblheader.Text + " # " + frm_vnum, sb.ToString(), frm_uname);
                                sb.Clear();
                                #endregion

                                fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                                fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                                hffield.Value = "SAVED";
                                setColHeadings();
                            }
                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Something is wrong in the Entry, Please re-check and Save!!");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        btnsave.Disabled = false;
                        fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N";
                        btnsave.Disabled = false;
                        return;
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
        sg1_dr["sg1_t2"] = "";
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
                sg1.HeaderRow.Cells[25].Text = "CGST";
                sg1.HeaderRow.Cells[26].Text = "SGST/UTGST";
            }
            else
            {
                sg1.HeaderRow.Cells[25].Text = "IGST";
                sg1.HeaderRow.Cells[26].Text = "-";
            }

            if (doc_GST.Value == "GCC")
            {
                sg1.HeaderRow.Cells[25].Text = "VAT";
                sg1.HeaderRow.Cells[26].Text = "-";
            }

            if (e.Row.Cells[2].Text.Trim().ToUpper() == "APL")
            {
                ((TextBox)e.Row.FindControl("sg1_t4")).Attributes.Add("readonly", "readonly");
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

                    //fgen.Fn_open_dtbox("Select Date", frm_qstr);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("", frm_qstr);

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
        fgen.Fn_open_sseek("Select Currency ", frm_qstr);
    }
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_16";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Price Basis ", frm_qstr);
    }
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_17";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Insurance Terms ", frm_qstr);
    }
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTN_18";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Freight Terms ", frm_qstr);
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
        fgen.Fn_open_sseek("Select Consignee", frm_qstr);
    }
    protected void btnlbl70_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODEX";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Department ", frm_qstr);
    }

    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        if (fgen.make_double(txtlbl24.Text) <= 0)
        {
            txtlbl15.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT br_Curren FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "br_Curren");

            if (txtlbl15.Text.Length < 2 && doc_GST.Value != "GCC")
            {
                txtlbl15.Text = "INR";
            }

            //txtlbl15.Text = "INR";
            txtlbl24.Text = "1";
        }

        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        //string mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select app_by from "+ frm_tabname +" where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and ordno='" + frm_vnum + "' and to_char(orddt,'dd/mm/yyyy')='" + txtvchdate.Text.Trim() + "'", "app_by");

        //if (mq0.Trim().Length > 1)
        //{
        //    for (i = 0; i < sg1.Rows.Count - 0; i++)
        //    {
        //        if (sg1.Rows[i].Cells[13].Text.Length > 2)
        //        {
        //            oporow = oDS.Tables[0].NewRow();
        //            oporow["BRANCHCD"] = "AM";
        //            oporow["orignalbr"] = frm_mbr;
        //            oporow["TYPE"] = frm_vty;
        //            oporow["ordno"] = frm_vnum;
        //            oporow["orddt"] = txtvchdate.Text.Trim();

        //            oporow["SRNO"] = i;
        //            oporow["pordno"] = txtlbl2.Text;
        //            oporow["porddt"] = fgen.make_def_Date(txtlbl3.Text, vardate);

        //            oporow["delv_term"] = txtlbl5.Text;
        //            oporow["mode_tpt"] = txtlbl6.Text;

        //            oporow["acode"] = txtlbl4.Text;
        //            oporow["cscode1"] = txtlbl7.Text;

        //            oporow["doc_thr"] = txtlbl8.Text;
        //            oporow["payment"] = txtlbl9.Text;

        //            oporow["pdays"] = fgen.make_double(txtlbl9.Text);

        //            oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim();
        //            oporow["unit"] = sg1.Rows[i].Cells[17].Text.Trim();
        //            oporow["desc_"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text;
        //            oporow["del_Date"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim(), vardate);
        //            oporow["delv_item"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text;


        //            oporow["qtyord"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
        //            oporow["prate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text);
        //            oporow["pdisc"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);

        //            oporow["landcost"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text);

        //            oporow["o_qty"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
        //            oporow["o_prate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text);



        //            oporow["pexc"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
        //            oporow["pcess"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);

        //            oporow["ciname"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text;
        //            if (((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().Length < 2)
        //            {
        //                oporow["ciname"] = sg1.Rows[i].Cells[14].Text.Trim();
        //            }

        //            oporow["splRmk"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text;

        //            oporow["rate_comm"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text);
        //            double ord_lin;
        //            ord_lin = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text);

        //            if (ord_lin <= 0)
        //            {
        //                ord_lin = (i + 1) * 10;
        //                oporow["cscode"] = ord_lin.ToString().Trim();
        //            }
        //            else
        //            {
        //                oporow["cscode"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text;
        //            }

        //            oporow["del_sch"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text;
        //            oporow["po_tolr"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text);
        //            oporow["wk1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text);

        //            oporow["tax"] = lbl27.Text.Substring(0, 2);

        //            oporow["term"] = txtrmk.Text.ToString().Trim().Replace("'", " ").Replace("\"", " ");
        //            oporow["remark"] = "-";
        //            oporow["test"] = "-";
        //            oporow["inst"] = txtlbl71.Text;

        //            if (sg1.Rows[i].Cells[16].Text.Length == 15)
        //            {
        //                string mpr_dtl;
        //                mpr_dtl = sg1.Rows[i].Cells[16].Text.Trim().Substring(9, 6);
        //                oporow["pr_no"] = mpr_dtl;

        //                mpr_dtl = sg1.Rows[i].Cells[16].Text.Trim().Substring(6, 2) + "/" + sg1.Rows[i].Cells[16].Text.Trim().Substring(4, 2) + "/" + sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 4);
        //                oporow["pr_Dt"] = fgen.make_def_Date(mpr_dtl, vardate);
        //            }
        //            else
        //            {
        //                oporow["pr_no"] = "-";
        //                oporow["pr_Dt"] = txtvchdate.Text.Trim();
        //            }


        //            oporow["packing"] = "-";

        //            oporow["transporter"] = "-";

        //            oporow["currency"] = txtlbl15.Text;
        //            oporow["pbasis"] = txtlbl16.Text;
        //            oporow["tr_insur"] = txtlbl17.Text;
        //            oporow["freight"] = txtlbl18.Text;



        //            oporow["pflag"] = 0;

        //            oporow["effdate"] = fgen.make_def_Date(txtlbl28.Text, vardate);
        //            oporow["validupto"] = fgen.make_def_Date(txtlbl30.Text, vardate);

        //            oporow["pdiscamt"] = 0;
        //            oporow["pexcamt"] = 0;

        //            double fd_rate;
        //            fd_rate = 0;
        //            switch (lbl1a.Text)
        //            {
        //                case "50":
        //                case "51":
        //                    fd_rate = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select ((a.prate*(100-a.pdisc)/100))-a.pdiscamt as fstr from pomas a WHERE a.branchcd='" + frm_mbr + "' and substr(a.type,1,2) in ('50','51') and trim(a.icode)='" + sg1.Rows[i].Cells[13].Text.Trim() + "' AND NVL(a.pflag,0)<>1 and a.app_by!='-' order by a.orddt desc ,a.prate desc", "fstr"));
        //                    break;
        //                default:
        //                    fd_rate = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select ((a.prate*(100-a.pdisc)/100))-a.pdiscamt as fstr from pomas a WHERE a.branchcd='" + frm_mbr + "' and substr(a.type,1,2) in ('" + lbl1a.Text + "') and trim(a.icode)='" + sg1.Rows[i].Cells[13].Text.Trim() + "' AND NVL(a.pflag,0)<>1 and a.app_by!='-' order by a.orddt desc ,a.prate desc", "fstr"));
        //                    break;
        //            }
        //            oporow["nxtmth"] = fd_rate;

        //            oporow["nxtmth2"] = 0;

        //            oporow["othamt1"] = fgen.make_double(txtlbl25.Text);
        //            oporow["othamt2"] = fgen.make_double(txtlbl27.Text);
        //            oporow["othamt3"] = fgen.make_double(txtlbl29.Text);
        //            oporow["rate_cd"] = fgen.make_double(txtlbl31.Text);
        //            oporow["wk3"] = fgen.make_double(txtlbl24.Text);
        //            oporow["othac1"] = "-";
        //            oporow["othac2"] = "-";
        //            oporow["othac3"] = "-";


        //            oporow["D18no"] = "-";
        //            oporow["st31no"] = "-";
        //            oporow["st38no"] = "-";
        //            oporow["psize"] = "-";
        //            oporow["bank"] = "-";

        //            oporow["prefsource"] = "-";
        //            oporow["qtysupp"] = 0;
        //            oporow["rate_ok"] = 0;
        //            string mq1 = fgen.next_no(frm_qstr, frm_cocd, "select max(amdtno) as " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and ordno='" + frm_vnum + "' and to_char(orddt,'dd/mm/yyyy')='" + txtvchdate.Text.Trim() + "'", 1, "vch");
        //            if (mq1 == "1")
        //            {
        //                mq1 = "0";
        //            }
        //            oporow["amdtno"] = mq1;
        //            oporow["gsm"] = 0;
        //            oporow["rate_rej"] = 0;
        //            oporow["qtybal"] = 1;
        //            oporow["ptax"] = 0;
        //            oporow["pamt"] = 0;

        //            oporow["issue_no"] = 0;
        //            oporow["invno"] = "Y";
        //            oporow["invdate"] = txtvchdate.Text.Trim();
        //            oporow["Delivery"] = 0;
        //            oporow["DEL_MTH"] = 0;
        //            oporow["DEL_wk"] = 0;



        //            oporow["refdate"] = txtvchdate.Text.Trim();
        //            oporow["STORE_NO"] = "SA";
        //            oporow["desp_to"] = "-";
        //            oporow["chl_ref"] = "-";
        //            oporow["rate_ok"] = 0;

        //            oporow["stax"] = "-";
        //            oporow["exc"] = "-";
        //            oporow["iopr"] = "-";
        //            oporow["amd_no"] = "-";

        //            oporow["rate_diff"] = "-";
        //            oporow["poprefix"] = "-";
        //            oporow["kindattn"] = "-";
        //            oporow["billcode"] = "-";

        //            oporow["tdisc_amt"] = 0;
        //            oporow["vend_wt"] = 0;
        //            oporow["wk2"] = 0;

        //            oporow["wk4"] = 0;

        //            oporow["ed_serv"] = "-";

        //            oporow["pdiscamt2"] = 0;
        //            oporow["txb_frt"] = 0;
        //            oporow["atch1"] = "-";
        //            oporow["atch2"] = "-";
        //            oporow["atch3"] = "-";


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
        //                oporow["eDt_dt"] = vardate;
        //            }
        //            oporow["chk_by"] = "-";
        //            oporow["chk_dt"] = vardate;

        //            oporow["app_by"] = "-";
        //            oporow["app_dt"] = vardate;

        //            oDS.Tables[0].Rows.Add(oporow);
        //        }
        //    }
        //}
        //else
        //{

        if (hfapby.Value.Length > 1 && frm_vnum != "000000")
        {
            double new_amd_no = 0;

            new_amd_no = fgen.make_double(txtAmd.Text.ToString());

            txtAmd.Text = (new_amd_no + 1).ToString();
        }


        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {

            if (sg1.Rows[i].Cells[13].Text.Length > 2)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["orignalbr"] = frm_mbr;
                oporow["TYPE"] = lbl1a.Text;
                oporow["ordno"] = frm_vnum;
                oporow["orddt"] = txtvchdate.Text.Trim();

                oporow["SRNO"] = i;
                oporow["pordno"] = txtlbl2.Text;
                oporow["porddt"] = fgen.make_def_Date(txtlbl3.Text, vardate);

                oporow["delv_term"] = txtlbl5.Text;
                oporow["mode_tpt"] = txtlbl6.Text;

                oporow["acode"] = txtlbl4.Text;
                oporow["cscode1"] = txtlbl7.Text;

                oporow["doc_thr"] = txtlbl8.Text;
                oporow["payment"] = txtlbl9.Text;

                oporow["pdays"] = fgen.make_double(txtlbl9.Text);

                oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim();
                oporow["unit"] = (sg1.Rows[i].Cells[17].Text.Trim().Contains("/") ? sg1.Rows[i].Cells[17].Text.Trim().Split('/')[1] : sg1.Rows[i].Cells[17].Text.Trim());

                if (lbl1a.Text == "55")
                {
                    double tot_Wt = 0;
                    tot_Wt = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) * fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text);
                    string mno_proc = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT nvl(no_proc,'-') as no_proc FROM item WHERE icode='" + sg1.Rows[i].Cells[13].Text.Trim() + "'", "no_proc"); ;

                    oporow["desc_"] = "Tot : " + fgen.make_double(tot_Wt, 2) + " " + mno_proc;

                    oporow["landcost"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text) / fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                }
                else
                {
                    oporow["desc_"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text;
                    oporow["landcost"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text);
                }



                oporow["del_Date"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim(), vardate);
                oporow["delv_item"] = fgen.make_def_Date(Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text).ToString("dd/MM/yyyy"), vardate);


                oporow["qtyord"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                oporow["prate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text);
                oporow["pdisc"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);


                oporow["o_qty"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                oporow["o_prate"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text);

                oporow["pexc"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                oporow["pcess"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);

                oporow["ciname"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text;
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().Length < 2)
                {
                    oporow["ciname"] = sg1.Rows[i].Cells[14].Text.Trim();
                }

                oporow["splRmk"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text;

                oporow["rate_comm"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text);
                double ord_lin;
                ord_lin = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text);

                if (ord_lin <= 0)
                {
                    ord_lin = (i + 1) * 10;
                    oporow["cscode"] = ord_lin.ToString().Trim();
                }
                else
                {
                    oporow["cscode"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text;
                }

                oporow["del_sch"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text;
                oporow["po_tolr"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text);
                oporow["wk1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text);
                oporow["qtybal"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text);
                oporow["tax"] = lbl27.Text.Substring(0, 2);

                oporow["term"] = txtrmk.Text.ToString().Trim().Replace("'", " ").Replace("\"", " ");
                oporow["remark"] = "-";
                oporow["test"] = "-";
                oporow["inst"] = txtlbl71.Text;

                if (sg1.Rows[i].Cells[16].Text.Length == 24)
                {
                    string mpr_dtl;
                    mpr_dtl = sg1.Rows[i].Cells[16].Text.Trim().Substring(9, 6);
                    oporow["pr_no"] = mpr_dtl;

                    mpr_dtl = sg1.Rows[i].Cells[16].Text.Trim().Substring(6, 2) + "/" + sg1.Rows[i].Cells[16].Text.Trim().Substring(4, 2) + "/" + sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 4);
                    oporow["pr_Dt"] = fgen.make_def_Date(mpr_dtl, vardate);

                    try
                    {
                        oporow["pr_srn"] = sg1.Rows[i].Cells[16].Text.Trim().Substring(16, 3);
                    }
                    catch { }
                }
                else if (sg1.Rows[i].Cells[16].Text.Length > 18)
                {
                    string mpr_dtl;
                    mpr_dtl = sg1.Rows[i].Cells[16].Text.Trim().Substring(9, 6);
                    oporow["pr_no"] = mpr_dtl;

                    mpr_dtl = sg1.Rows[i].Cells[16].Text.Trim().Substring(6, 2) + "/" + sg1.Rows[i].Cells[16].Text.Trim().Substring(4, 2) + "/" + sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 4);
                    oporow["pr_Dt"] = fgen.make_def_Date(mpr_dtl, vardate);

                    try
                    {
                        oporow["pr_srn"] = sg1.Rows[i].Cells[16].Text.Trim().Substring(16, 3);
                    }
                    catch { }
                }
                else
                {
                    oporow["pr_no"] = "-";
                    oporow["pr_Dt"] = txtvchdate.Text.Trim();

                    oporow["pr_srn"] = "";
                }

                oporow["packing"] = "-";

                oporow["transporter"] = "-";

                oporow["currency"] = txtlbl15.Text;
                oporow["pbasis"] = txtlbl16.Text;
                oporow["tr_insur"] = txtlbl17.Text;
                oporow["freight"] = txtlbl18.Text;



                oporow["pflag"] = 0;

                oporow["effdate"] = fgen.make_def_Date(txtlbl28.Text, vardate);
                oporow["validupto"] = fgen.make_def_Date(txtlbl30.Text, vardate);

                oporow["pdiscamt"] = 0;
                oporow["pexcamt"] = 0;

                double fd_rate;
                fd_rate = 0;
                switch (lbl1a.Text)
                {
                    case "50":
                    case "51":
                        fd_rate = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select ((a.prate*(100-a.pdisc)/100))-a.pdiscamt as fstr from " + frm_tabname + " a WHERE a.branchcd='" + frm_mbr + "' and substr(a.type,1,2) in ('50','51') and trim(a.icode)='" + sg1.Rows[i].Cells[13].Text.Trim() + "' AND NVL(a.pflag,0)<>1 and a.app_by!='-' order by a.orddt desc ,a.prate desc", "fstr"));
                        break;
                    default:
                        fd_rate = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select ((a.prate*(100-a.pdisc)/100))-a.pdiscamt as fstr from " + frm_tabname + " a WHERE a.branchcd='" + frm_mbr + "' and substr(a.type,1,2) in ('" + lbl1a.Text + "') and trim(a.icode)='" + sg1.Rows[i].Cells[13].Text.Trim() + "' AND NVL(a.pflag,0)<>1 and a.app_by!='-' order by a.orddt desc ,a.prate desc", "fstr"));
                        break;
                }
                oporow["nxtmth"] = fd_rate;

                oporow["nxtmth2"] = 0;
                oporow["amdtno"] = fgen.make_double(txtAmd.Text);
                oporow["othamt1"] = fgen.make_double(txtlbl25.Text);
                oporow["othamt2"] = fgen.make_double(txtlbl27.Text);
                oporow["othamt3"] = fgen.make_double(txtlbl29.Text);

                oporow["rate_cd"] = fgen.make_double(txtlbl31.Text);

                oporow["wk3"] = fgen.make_double(txtlbl24.Text);
                oporow["othac1"] = "-";
                oporow["othac2"] = "-";
                oporow["othac3"] = "-";


                oporow["D18no"] = "-";
                oporow["st31no"] = "-";
                oporow["st38no"] = "-";
                oporow["psize"] = "-";
                oporow["bank"] = "-";

                oporow["prefsource"] = "-";
                oporow["qtysupp"] = 0;
                oporow["rate_ok"] = 0;

                oporow["gsm"] = 0;
                oporow["rate_rej"] = 0;

                oporow["ptax"] = 0;
                oporow["pamt"] = 0;

                if ((((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text) == "APL")
                {
                    oporow["issue_no"] = 1;
                }
                else
                {
                    oporow["issue_no"] = 0;
                }
                oporow["invno"] = "Y";
                oporow["invdate"] = txtvchdate.Text.Trim();
                oporow["Delivery"] = 0;
                oporow["DEL_MTH"] = 0;
                oporow["DEL_wk"] = 0;



                oporow["refdate"] = txtvchdate.Text.Trim();
                oporow["STORE_NO"] = "SA";
                oporow["desp_to"] = "-";
                oporow["chl_ref"] = "-";
                oporow["rate_ok"] = 0;

                oporow["stax"] = "-";
                oporow["exc"] = "-";
                oporow["iopr"] = "-";
                oporow["amd_no"] = "-";

                oporow["rate_diff"] = "-";
                oporow["poprefix"] = "-";
                oporow["kindattn"] = "-";
                oporow["billcode"] = "-";

                oporow["tdisc_amt"] = 0;
                oporow["vend_wt"] = 0;




                oporow["wk2"] = 0;

                oporow["wk4"] = 0;


                oporow["ed_serv"] = "-";

                oporow["pdiscamt2"] = 0;
                oporow["txb_frt"] = 0;
                oporow["atch1"] = "-";
                oporow["atch2"] = "-";
                oporow["atch3"] = "-";

                oporow["psize"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text;

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
                oporow["chk_by"] = "-";
                oporow["chk_dt"] = vardate;

                oporow["app_by"] = "-";
                oporow["app_dt"] = vardate;

                oDS.Tables[0].Rows.Add(oporow);
            }
        }
    }

    void save_fun2()
    {

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

        SQuery = "SELECT type1 AS FSTR,NAME,type1 AS CODE FROM type where id='M' and type1 like '5%' order by type1";
        btnval = hffield.Value;
        if (btnval != "New")
        {
            SQuery = "select Fstr,Document_Name,Document_Series,count(*) as Document_Count from (SELECT distinct a.type as Fstr,b.NAME as Document_Name,a.TYPE as Document_Series,a.ordno FROM " + frm_tabname + " a ,TYPE b WHERE a.branchcd='" + frm_mbr + "' and a.orddt " + DateRange + " and trim(A.type)=trim(B.type1) and b.ID='M' and a.type like '5%') group by Fstr,Document_Name,Document_Series order by Document_Series ";
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
            txtlbl29.Style.Add("display", "none");
        }

    }
    void setVendorDetails()
    {
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT PAY_NUM,GST_NO,STATEN FROM " + frm_famtbl + " WHERE ACODE='" + txtlbl4.Text.Trim() + "'");
        if (dt.Rows.Count > 0)
        {
            txtlbl9.Text = dt.Rows[0]["pay_num"].ToString().Trim();
            mgst_no = dt.Rows[0]["GST_NO"].ToString().Trim();
            txtlbl73.Text = dt.Rows[0]["STATEN"].ToString().Trim();
        }

        if (mgst_no.Length > 10)
        {
            txtTax.Text = "Y";
        }
        else
        {
            txtTax.Text = "N";
        }

        txtlbl72.Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEnM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATEnM");

        string old_terms = "";
        old_terms = fgen.seek_iname(frm_qstr, frm_cocd, "select fstr from (SELECT trim(pordno)||'^'||to_char(porddt,'dd/mm/yyyy')||'^'||trim(delv_Term)||'^'||trim(mode_tpt)||'^'||trim(doc_thr)||'^'||trim(payment)||'^'||trim(currency)||'^'||trim(pbasis)||'^'||trim(tr_insur)||'^'||trim(freight) as fstr,orddt FROM " + frm_tabname + " WHERE branchcd='" + frm_mbr + "' and trim(acode)='" + txtlbl4.Text + "' order by orddt desc) where rownum<2 ", "fstr");
        if (old_terms.Contains("^"))
        {

            txtlbl2.Text = old_terms.Split('^')[0].ToString();
            txtlbl3.Text = old_terms.Split('^')[1].ToString();

            txtlbl5.Text = old_terms.Split('^')[2].ToString();
            txtlbl6.Text = old_terms.Split('^')[3].ToString();

            txtlbl8.Text = old_terms.Split('^')[4].ToString();
            txtlbl9.Text = old_terms.Split('^')[5].ToString();


            txtlbl15.Text = old_terms.Split('^')[6].ToString();
            txtlbl16.Text = old_terms.Split('^')[7].ToString();
            txtlbl17.Text = old_terms.Split('^')[8].ToString();
            txtlbl18.Text = old_terms.Split('^')[9].ToString();
        }

        btnlbl7.Focus();
    }
    protected void btnGridPop_Click(object sender, EventArgs e)
    {
        if (hf1.Value.Contains("sg1_t3_"))
        {
            hffield.Value = "sg1_t3";
            hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_sg1_t3_", "");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            make_qry_4_popup();
            fgen.Fn_open_sseek("Rate History", frm_qstr);
        }
    }
    protected void btnPR_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "SG1_ROW_ADD";
        hfpr.Value = "PR";
        col1 = "";
        if (btnval != "SG3_ROW_ADD" && btnval != "SG3_ROW_ADD_E")
        {
            foreach (GridViewRow gr in sg1.Rows)
            {
                if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[13].Text.Trim() + "'";
                else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
            }
        }

        itemRepeat = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMREPEAT");
        if (col1.Length <= 0) col1 = "'-'";
        if (itemRepeat == "Y") col1 = "'-'";
        if (frm_cocd == "MULT") col1 = "'-'";
        SQuery = "select trim(a.Fstr) as fstr,b.Iname as Item_Name,b.Maker,substr(a.fstr,10,6) as prno,substr(a.fstr,7,2)||'/'||substr(a.fstr,5,2)||'/'||substr(a.fstr,1,4) as prdt ,trim(a.ERP_code) as ERP_code,b.Cpartno as Part_no,b.Irate,b.Cdrgno,(a.Qtyord)-(a.Soldqty) as Bal_Qty,b.Unit,(a.desc_) as Remarks,b.hscode,(a.bank) as Deptt,(a.delv_item) as Reqd_Dt,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Req_Qty,trim(a.Fstr) as PR_link,a.psize as fo_no,nvl(b.iweight,1) as iweight from (select fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,max(bank) as bank,max(delv_item) As delv_item,max(desc_) as desc_,max(psize) as psize from (SELECT to_ChaR(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,nvl(bank,'-') As bank,nvl(delv_item,'-') As delv_item,nvl(desc_,'-') as desc_,psize from pomas where branchcd='" + frm_mbr + "' and type='60' and trim(pflag)!=0 and trim(app_by)!='-' and orddt>=to_Date('01/04/2017','dd/mm/yyyy') union all SELECT to_ChaR(pr_Dt,'YYYYMMDD')||'-'||pr_no||'-'||trim(Icode) as fstr,trim(Icode) as ERP_code,0 as Qtyord,qtyord,null as bank,null as delv_item,null as desc_,psize from pomas where branchcd='" + frm_mbr + "' and type like '5%' and orddt>=to_Date('01/04/2017','dd/mm/yyyy'))  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and trim(a.erp_code) not in (" + col1 + ") order by B.Iname,trim(a.fstr)";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_mseek("-", frm_qstr);
    }

    protected void btnupload_ServerClick(object sender, EventArgs e)
    {
        string ext = "", filesavepath = "";
        string excelConString = "";
        if (fileImport.HasFile)
        {
            ext = System.IO.Path.GetExtension(fileImport.FileName).ToLower();
            if (ext == ".xls")
            {
                filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xls";
                fileImport.SaveAs(filesavepath);
                excelConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
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
            dtm.Columns.Add("MAT2", typeof(string));
            dtm.Columns.Add("qty", typeof(string));

            DataRow drn;
            foreach (DataRow dr in DTEX.Rows)
            {
                SQuery = "select distinct a.icode,a.iname,a.cpartno,a.cdrgno,a.irate,a.unit,a.hscode,b.num4,b.num5,b.num6,b.num7,a.mat2 from item a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and trim(a.CPARTNO) in ('" + dr[0].ToString().Trim() + "')";
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
                    drn["MAT2"] = dt2.Rows[0]["MAT2"];
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
                    sg1_dr["sg1_t17"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text.Trim();

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

                    sg1_dr["sg1_t9"] = dt.Rows[d]["iname"].ToString().Trim();
                    sg1_dr["sg1_t10"] = dt.Rows[d]["cpartno"].ToString().Trim();
                    sg1_dr["sg1_t11"] = "0";
                    sg1_dr["sg1_t12"] = "";
                    sg1_dr["sg1_t13"] = "";
                    sg1_dr["sg1_t14"] = "";
                    sg1_dr["sg1_t15"] = dt.Rows[d]["mat2"].ToString().Trim();
                    sg1_dr["sg1_t16"] = "";
                    sg1_dr["sg1_t17"] = "";

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
    protected void btnCheckTax_ServerClick(object sender, EventArgs e)
    {
        foreach (GridViewRow gr in sg1.Rows)
        {
            if (gr.Cells[13].Text.Trim().Length > 5)
            {
                if (((TextBox)gr.FindControl("sg1_t7")).Text.toDouble() <= 0)
                {
                    if (txtlbl72.Text.Trim().ToUpper() == txtlbl73.Text.Trim().ToUpper())
                    {
                        ((TextBox)gr.FindControl("sg1_t7")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT B.NUM4 FROM TYPEGRP B,ITEM A WHERE TRIM(A.HSCODE)=TRIM(B.ACREF) AND TRIM(A.ICODe)='" + gr.Cells[13].Text.Trim() + "' ", "num4");
                        ((TextBox)gr.FindControl("sg1_t8")).Text = ((TextBox)gr.FindControl("sg1_t7")).Text;
                    }
                    else
                    {
                        ((TextBox)gr.FindControl("sg1_t7")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT B.NUM6 FROM TYPEGRP B,ITEM A WHERE TRIM(A.HSCODE)=TRIM(B.ACREF) AND TRIM(A.ICODe)='" + gr.Cells[13].Text.Trim() + "' ", "num6");
                        ((TextBox)gr.FindControl("sg1_t8")).Text = "0";
                    }
                }
            }
        }
    }
    [System.Web.Services.WebMethod]
    public static void calc()
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        hffield.Value = "PENDPR";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMREPID", frm_formID + "_1");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        hffield.Value = "PENDPO";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMREPID", frm_formID + "_2");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        hffield.Value = "STKINH";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMREPID", frm_formID + "_3");

        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        hffield.Value = "APPVENL";
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    protected void btnDelReq_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTNDELREQ";
        make_qry_4_popup();
        fgen.Fn_open_sseek("select " + lbl5.Text + "", frm_qstr);
    }
    protected void btnDelMode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTNDELMODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("select " + lbl6.Text + "", frm_qstr);
    }
    protected void btnPayMode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BTNPAYMODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("select " + lbl8.Text + "", frm_qstr);
    }
    protected void btnPayDays_Click(object sender, ImageClickEventArgs e)
    {
        return;
        //hffield.Value = "BTNPAYDAYS";
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("select " + lbl9.Text + "", frm_qstr);
    }
}