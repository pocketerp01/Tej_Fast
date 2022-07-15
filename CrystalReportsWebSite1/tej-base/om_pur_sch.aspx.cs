using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_pur_sch : System.Web.UI.Page
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
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

                // hidding column
                if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
                {
                    sg1.Columns[sR].HeaderStyle.CssClass = "hidden";
                    sg1.Rows[K].Cells[sR].CssClass = "hidden";
                    //sg1.Columns[sR].Visible = false;
                }
            }
            orig_name = orig_name.ToUpper();
            //if (sg1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_NAME"))
            if (sR == tb_Colm)
            {
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
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "schedule";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "66");

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
                SQuery = "SELECT DISTINCT a.Acode as FStr,a.Aname as Customer,a.Acode,a.Addr1,a.Addr2,a.GST_No,a.Staten from famst a,POMAS b where trim(A.acodE)=trim(B.acode) AND b.BRANCHCD='" + frm_mbr + "' AND B.TYPE LIKE '5%'  and length(trim(nvl(a.deac_by,'-'))) <2 AND (B.pflag)!=1 AND TRIM(NVL(B.APP_BY,'-'))!='-' order by a.Aname ";
                break;
            case "TICODE":
                //pop2
                SQuery = "SELECT Type1 AS FSTR,NAME AS Deptt,Type1 AS CODE FROM type where id='M' and type1 like '5%' order by type1";
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[13].Text.Trim().Length > 2)
                    {
                        if (col1.Length > 0) col1 = col1 + "," + "'" + gr.Cells[13].Text.Trim() + "'";
                        else col1 = "'" + gr.Cells[13].Text.Trim() + "'";
                    }
                }
                if (col1.Length > 0)
                {
                    col1 = " and TRIM(a.icode) not in (" + col1 + ")";
                }
                else
                {
                    col1 = "";
                }

                SQuery = "SELECT DISTINCT trim(A.Icode)||a.ordno||'-'||to_char(a.orddt,'dd/mm/yyyy') AS FSTR,B.Iname AS Item_Name,B.Cpartno,A.Icode,a.Ordno,to_char(a.Orddt,'dd/mm/yyyy') as Ord_dt,b.Cdrgno,a.qtyord||' '||b.unit,to_char(a.Orddt,'yyyymmdd') as Ordt FROM POMAS a , Item B where TRIM(a.ICODE)=TRIM(B.ICODE) AND a.branchcd='" + frm_mbr + "' AND a.acode='" + txtlbl4.Text + "' and length(Trim(b.deac_by))<2  and length(Trim(b.icode))>4 " + col1 + " and trim(nvl(a.app_by,'-'))!='-' and (a.pflag)!=1 ORDER BY b.Iname,Ordt,a.ordno ";
                //SQuery = "";
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

                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(Trim(icode))>4 and icode like '9%' and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname  ";
                break;


            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as Sch_no,to_char(a.vchdate,'dd/mm/yyyy') as Sch_Dt,b.aName as Supplier,b.Addr1 as Address,b.Staten,b.Gst_no,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b where trim(A.acode)=trim(B.acode)  and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
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
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            hffield.Value = "Edit";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr);// REMOVE TYPE FROM THE LINE BY MADHVI ON 28 JULY 2018
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
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }


        string chk_freeze = "";
        chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1013", txtvchdate.Text.Trim());
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
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", '13' Schedule Qty is Exceeding Order Qty , , Please Check '13' " + err_item_name + "'13' " + err_item);
            return;
        }

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
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
                drQty["fstr"] = gr.Cells[13].Text.ToString().Trim() + gr.Cells[16].Text.ToString().Trim();
                drQty["iname"] = gr.Cells[14].Text.ToString().Trim();
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t2")).Text.ToString().Trim());
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
            string chk_itm;
            chk_itm = drQty1["fstr"].ToString().Trim().Substring(0, 8);
            mqry = "select (a.Qtyord)-(a.Soldqty) as Bal_Qty from (select fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(Icode)||ordno||'-'||to_char(orddt,'dd/mm/yyyy') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,0 as prate from pomas where branchcd='" + frm_mbr + "' and type like '5%' and trim(pflag)!=1  and orddt>=to_Date('01/04/2017','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text.Trim() + "' union all SELECT trim(Icode)||ponum||'-'||trim(podate) as fstr,trim(Icode) as ERP_code,0 as Qtyord,total as qtyord ,0 as irate from schedule where branchcd='" + frm_mbr + "' and type like '66%' and vchdate>=to_Date('01/04/2017','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text.Trim() + "' and trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "')  group by fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0  )a,item b where trim(a.erp_code)=trim(B.icode) and a.fstr='" + drQty1["fstr"].ToString().Trim() + "' order by B.Iname,trim(a.fstr)";
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, mqry, "Bal_Qty");

            if (fgen.make_double(sm.ToString()) > fgen.make_double(col1) && fgen.make_double(col1) > 0)
            {
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_CHK", "N");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_OQTY_ITM", drQty1["fstr"].ToString().Trim());

                string itm_name;
                itm_name = fgen.seek_iname(frm_qstr, frm_cocd, "select iname from ITEM where SUBSTR(ICODE,1,8)='" + chk_itm + "' ", "iname") + " PO Qty " + fgen.make_double(col1);
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ERR_ITEM", itm_name);

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
            fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr); // REMOVE TYPE FROM THE LINE BY MADHVI ON 28 JULY 2018
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
        //fgen.Fn_open_mseek("Select Type for Print", frm_qstr);//// COMMENTED BY MADHVI ON 28 JULY 2018
        fgen.Fn_open_mseek("Select " + lblheader.Text, frm_qstr); // BY MADHVI ON 28 JULY 2018
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        vty = "66";
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
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
                    fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.*,b.aname,c.iname,c.cpartno,c.cdrgno,c.unit,trim(a.podate) as podated,to_char(a.ent_Dt,'dd/mm/yyyy') as pent_dt,to_char(a.app_Dt,'dd/mm/yyyy') as papp_dt from " + frm_tabname + " a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");


                        txtlbl2.Text = dt.Rows[i]["ent_by"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["pent_Dt"].ToString().Trim();

                        txtlbl4.Text = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();

                        txtlbl5.Text = dt.Rows[i]["app_by"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["papp_Dt"].ToString().Trim();


                        //txtlbl7.Text = dt.Rows[0]["acode"].ToString().Trim();
                        //txtlbl7a.Text = dt.Rows[0]["name"].ToString().Trim();

                        //txtlbl8.Text = dt.Rows[0]["mode_tpt"].ToString().Trim();
                        //txtlbl9.Text = dt.Rows[0]["pflag"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["remarks"].ToString().Trim();


                        //oporow["ponum"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text;
                        //oporow["podate"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim(), vardate);
                        //oporow["rgpnum"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text;
                        //oporow["rgpdate"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim(), vardate);


                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            // filling value in sg1_h1
                            // saving icode in this field
                            sg1_dr["sg1_h1"] = dt.Rows[i]["irate"].ToString().Trim();

                            sg1_dr["sg1_h2"] = "-";
                            sg1_dr["sg1_h3"] = "-";
                            sg1_dr["sg1_h4"] = "-";
                            sg1_dr["sg1_h5"] = "-";
                            sg1_dr["sg1_h6"] = "-";
                            sg1_dr["sg1_h7"] = "-";
                            sg1_dr["sg1_h8"] = "-";
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";

                            sg1_dr["sg1_f1"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["ponum"].ToString().Trim() + "-" + dt.Rows[i]["podated"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["line_rmk"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["total"].ToString().Trim();


                            sg1_dr["sg1_t3"] = dt.Rows[i]["day1"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["day2"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["day3"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["day4"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["day5"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["day6"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["day7"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["day8"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["day9"].ToString().Trim();
                            sg1_dr["sg1_t12"] = dt.Rows[i]["day10"].ToString().Trim();

                            sg1_dr["sg1_t13"] = dt.Rows[i]["day11"].ToString().Trim();
                            sg1_dr["sg1_t14"] = dt.Rows[i]["day12"].ToString().Trim();
                            sg1_dr["sg1_t15"] = dt.Rows[i]["day13"].ToString().Trim();
                            sg1_dr["sg1_t16"] = dt.Rows[i]["day14"].ToString().Trim();
                            sg1_dr["sg1_t17"] = dt.Rows[i]["day15"].ToString().Trim();
                            sg1_dr["sg1_t18"] = dt.Rows[i]["day16"].ToString().Trim();
                            sg1_dr["sg1_t19"] = dt.Rows[i]["day17"].ToString().Trim();
                            sg1_dr["sg1_t20"] = dt.Rows[i]["day18"].ToString().Trim();
                            sg1_dr["sg1_t21"] = dt.Rows[i]["day19"].ToString().Trim();
                            sg1_dr["sg1_t22"] = dt.Rows[i]["day20"].ToString().Trim();

                            sg1_dr["sg1_t23"] = dt.Rows[i]["day21"].ToString().Trim();
                            sg1_dr["sg1_t24"] = dt.Rows[i]["day22"].ToString().Trim();
                            sg1_dr["sg1_t25"] = dt.Rows[i]["day23"].ToString().Trim();
                            sg1_dr["sg1_t26"] = dt.Rows[i]["day24"].ToString().Trim();
                            sg1_dr["sg1_t27"] = dt.Rows[i]["day25"].ToString().Trim();
                            sg1_dr["sg1_t28"] = dt.Rows[i]["day26"].ToString().Trim();
                            sg1_dr["sg1_t29"] = dt.Rows[i]["day27"].ToString().Trim();
                            sg1_dr["sg1_t30"] = dt.Rows[i]["day28"].ToString().Trim();
                            sg1_dr["sg1_t31"] = dt.Rows[i]["day29"].ToString().Trim();
                            sg1_dr["sg1_t32"] = dt.Rows[i]["day30"].ToString().Trim();
                            sg1_dr["sg1_t33"] = dt.Rows[i]["day31"].ToString().Trim();

                            sg1_dr["sg1_t34"] = dt.Rows[i]["wk1"].ToString().Trim();
                            sg1_dr["sg1_t35"] = dt.Rows[i]["wk2"].ToString().Trim();

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
                        edmode.Value = "Y";

                        if (fgen.make_double(frm_ulvl) <= 1) btnlbl4.Enabled = false;
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
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1014");
                    fgen.fin_purc_reps(frm_qstr);
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;

                    btnlbl7.Focus();
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
                            sg1_dr["sg1_t24"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text.Trim();
                            sg1_dr["sg1_t25"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text.Trim();
                            sg1_dr["sg1_t26"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text.Trim();
                            sg1_dr["sg1_t27"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t27")).Text.Trim();
                            sg1_dr["sg1_t28"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t28")).Text.Trim();
                            sg1_dr["sg1_t29"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t29")).Text.Trim();
                            sg1_dr["sg1_t30"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t30")).Text.Trim();

                            sg1_dr["sg1_t31"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t31")).Text.Trim();
                            sg1_dr["sg1_t32"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t32")).Text.Trim();
                            sg1_dr["sg1_t33"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t33")).Text.Trim();
                            sg1_dr["sg1_t34"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t34")).Text.Trim();
                            sg1_dr["sg1_t35"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t35")).Text.Trim();


                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();



                        if (col1.Length > 30) SQuery = "SELECT DISTINCT A.Icode AS FSTR,B.Iname AS Item_Name,A.Icode,a.prate,a.Ordno,a.Orddt,B.Cpartno,b.Cdrgno,b.unit,to_char(a.orddt,'dd/mm/yyyy') as orddtd,a.type,a.qtyord FROM POMAS a , Item B where TRIM(a.ICODE)=TRIM(B.ICODE) AND a.branchcd='" + frm_mbr + "' AND a.acode='" + txtlbl4.Text + "' and length(Trim(b.deac_by))<2  and length(Trim(b.icode))>4  and trim(nvl(a.app_by,'-'))!='-' and (a.pflag)!=1 and trim(A.Icode)||a.ordno||'-'||to_char(a.orddt,'dd/mm/yyyy') in (" + col1 + ") ORDER BY b.Iname,a.orddt ";
                        else SQuery = SQuery = "SELECT DISTINCT A.Icode AS FSTR,B.Iname AS Item_Name,A.Icode,a.prate,a.Ordno,a.Orddt,B.Cpartno,b.Cdrgno,b.unit,to_char(a.orddt,'dd/mm/yyyy') as orddtd,a.type,a.qtyord FROM POMAS a , Item B where TRIM(a.ICODE)=TRIM(B.ICODE) AND a.branchcd='" + frm_mbr + "' AND a.acode='" + txtlbl4.Text + "' and length(Trim(b.deac_by))<2  and length(Trim(b.icode))>4  and trim(nvl(a.app_by,'-'))!='-' and (a.pflag)!=1 and trim(A.Icode)||a.ordno||'-'||to_char(a.orddt,'dd/mm/yyyy')=" + col1 + " ORDER BY b.Iname,a.orddt ";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            // filling value in sg1_h1
                            // saving icode in this field
                            sg1_dr["sg1_h1"] = dt.Rows[d]["prate"].ToString().Trim();

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
                            sg1_dr["sg1_f4"] = dt.Rows[d]["ordno"].ToString().Trim() + "-" + dt.Rows[d]["orddtd"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["qtyord"].ToString().Trim() + " " + dt.Rows[d]["unit"].ToString().Trim();
                            //fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                            sg1_dr["sg1_t1"] = "";
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = "";
                            sg1_dr["sg1_t4"] = "";
                            sg1_dr["sg1_t5"] = "";
                            sg1_dr["sg1_t6"] = "";
                            sg1_dr["sg1_t7"] = "";
                            sg1_dr["sg1_t8"] = "";
                            sg1_dr["sg1_t9"] = "";
                            sg1_dr["sg1_t10"] = "";

                            sg1_dr["sg1_t11"] = "";
                            sg1_dr["sg1_t12"] = "";
                            sg1_dr["sg1_t13"] = "";
                            sg1_dr["sg1_t14"] = "";
                            sg1_dr["sg1_t15"] = "";
                            sg1_dr["sg1_t16"] = "";
                            sg1_dr["sg1_t17"] = "";
                            sg1_dr["sg1_t18"] = "";
                            sg1_dr["sg1_t19"] = "";
                            sg1_dr["sg1_t20"] = "";

                            sg1_dr["sg1_t21"] = "";
                            sg1_dr["sg1_t22"] = "";
                            sg1_dr["sg1_t23"] = "";
                            sg1_dr["sg1_t24"] = "";
                            sg1_dr["sg1_t25"] = "";
                            sg1_dr["sg1_t26"] = "";
                            sg1_dr["sg1_t27"] = "";
                            sg1_dr["sg1_t28"] = "";
                            sg1_dr["sg1_t29"] = "";
                            sg1_dr["sg1_t30"] = "";

                            sg1_dr["sg1_t31"] = "";
                            sg1_dr["sg1_t32"] = "";
                            sg1_dr["sg1_t33"] = "";
                            sg1_dr["sg1_t34"] = "";
                            sg1_dr["sg1_t35"] = "";

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    btnlbl4.Enabled = false; btnlbl7.Enabled = false; // BY MADHVI ON 23 JULY 2018
                    #endregion
                    setColHeadings();
                    break;
                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    //********* Saving in Hidden Field 
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    //********* Saving in GridView Value
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
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
                            sg1_dr["sg1_t28"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t28")).Text.Trim();
                            sg1_dr["sg1_t29"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t29")).Text.Trim();
                            sg1_dr["sg1_t30"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t30")).Text.Trim();

                            sg1_dr["sg1_t31"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t31")).Text.Trim();
                            sg1_dr["sg1_t32"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t32")).Text.Trim();
                            sg1_dr["sg1_t33"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t33")).Text.Trim();
                            sg1_dr["sg1_t34"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t34")).Text.Trim();
                            sg1_dr["sg1_t35"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t35")).Text.Trim();

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

        if (hffield.Value == "List" || hffield.Value == "PENDPR" || hffield.Value == "PENDPO" || hffield.Value == "STKINH" || hffield.Value == "PENDSCH")
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
                    PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

                    SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, "F15128", "branchcd='" + frm_mbr + "'", "a.type='66'", PrdRange);

                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);

                    fgen.Fn_open_rptlevel("Purchase Schedule Checklist for the Period " + fromdt + " to " + todt, frm_qstr);
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

                    mq1 = "select  '-' as fstr,'-' as gstr,b.Aname,c.iname as Item_Name,sum(a.poq) as PO_Qty,sum(a.rcvq) as MRR_Qty,sum(a.poq)-sum(a.rcvq) as Bal_Qty,sum(a.rej_Qty) as Rejn_Qty,(Case when sum(a.poq)>0 then round(((sum(a.poq)-sum(a.rcvq))/sum(a.poq))*100,2) else 0 end) as bal_per,round(sysdate-a.orddt,0) as Pend_Days,c.unit,a.branchcd,a.ordno,a.orddt,a.type,trim(a.icode) as ERP_Code,trim(a.acode) as Act_code,max(a.pflag)as pflag,max(a.del_Sch) as wo_no from (Select branchcd,pflag,ordno,orddt,acode,icode,qtyord as poq,0 as rcvq,0 as rej_Qty,type,del_Sch from pomas where branchcd='" + frm_mbr + "' and substr(type,1,1) ='5' and orddt " + xprd1 + " and  acode like '" + party_cd + "%' and icode like '" + part_cd + "%' union all Select branchcd,null as pflag,ponum,podate,acode,icode,0 as prq,iqtyin as poq,rej_rw,potype,null as del_Sch from ivoucher where branchcd='" + frm_mbr + "' and substr(type,1,1)='0' and potype like '5%' and vchdate " + xprd1 + " and store in ('Y','N')  ";
                    mq2 = "and  acode like '" + party_cd + "' and icode like '" + part_cd + "%'  )a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(a.icode)=trim(C.icode) group by c.iname,c.unit,b.aname,a.branchcd,a.type,a.ordno,a.orddt,trim(a.AcodE),trim(a.icode) having sum(a.poq)-sum(a.rcvq)>0 and max(a.pflag)!=1 order by b.aname,a.orddt,a.ordno";

                    SQuery = mq1 + mq2;
                    fgen.drillQuery(0, SQuery, frm_qstr, "4#5#6#7#8#", "3#4#5#6#7#8#", "350#100#100#100#100#100#");
                    headerN = "Pending Purchase Orders (" + fromdt + " to " + todt + ")";
                    break;
                case "STKINH":
                    SQuery = "select '-' as fstr,'-' as gstr,b.Iname,b.Cpartno,sum(a.opening) as Opening_Stock,sum(a.cdr) as Receipts,sum(a.ccr) as Issues,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stock,b.Unit,TRIM(A.ICODE) AS ERP_Code,max(a.imin) as Min_lvl,max(a.imax) as Max_lvl,max(a.iord) as ReOrder_lvl  from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as imin,nvl(imax,0) as imax,nvl(iord,0) as iord from itembal where BRANCHCD='" + frm_mbr + "' and length(trim(icode))>4 union all select branchcd,trim(icode) as icode,(nvl(iqtyin,0))-(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr,0 as imin,0 as imax,0 as iord FROM IVOUCHER where BRANCHCD='" + frm_mbr + "' and TYPE LIKE '%' AND VCHDATE " + xprd1 + " and store='Y' union all select branchcd,trim(icode) as icode,0 as op,(nvl(iqtyin,0)) as cdr,(nvl(iqtyout,0)) as ccr,0 as imin,0 as imax,0 as iord from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%'  AND VCHDATE " + xprd2 + " and store='Y') a,item b where trim(A.icode)=trim(B.icode) and a.icode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' and substr(A.icode,1,1)<'8' GROUP BY b.Iname,b.Cpartno,b.Unit,TRIM(A.ICODE) ORDER BY trim(a.ICODE)";
                    headerN = "Stock Summary Report (" + fromdt + " to " + todt + ")";
                    fgen.drillQuery(0, SQuery, frm_qstr, "5#6#7#8#", "3#4#5#6#7#8#", "350#100#100#100#100#100#");
                    break;
                case "PENDSCH":
                    xprd1 = " BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + todt + "','DD/MM/YYYY')";

                    mq1 = "select  '-' as fstr,'-' as gstr,b.Aname,c.iname as Item_Name,sum(a.poq) as Sch_Qty,sum(a.rcvq) as Rcvd_Qty,sum(a.poq)-sum(a.rcvq) as Bal_Qty,sum(a.rej_Qty) as Rejn_Qty,(Case when sum(a.poq)>0 then round(((sum(a.poq)-sum(a.rcvq))/sum(a.poq))*100,2) else 0 end) as bal_per,c.unit,a.branchcd,a.sch_Dt,trim(a.icode) as ERP_Code,trim(a.acode) as Act_code from (Select branchcd,to_char(vchdate,'yyyymm') as Sch_Dt,acode,icode,total as poq,0 as rcvq,0 as rej_qty from schedule where branchcd='" + frm_mbr + "' and substr(type,1,2) ='66' and vchdate " + xprd1 + " and  acode like '" + party_cd + "%' and icode like '" + part_cd + "%' union all Select branchcd,to_char(vchdate,'yyyymm') as Sch_Dt,acode,icode,0 as poq,iqtyin as rcvq,nvl(Rej_rw,0) as rejn from ivoucher where branchcd='" + frm_mbr + "' and substr(type,1,1)='0' and vchdate " + xprd1 + " and store in ('Y','N')  ";
                    mq2 = "and  acode like '" + party_cd + "' and icode like '" + part_cd + "%'  )a,famst b,item c where trim(A.acode)=trim(B.acode) and trim(a.icode)=trim(C.icode) group by c.iname,c.unit,b.aname,a.branchcd,a.sch_Dt,trim(a.AcodE),trim(a.icode) having sum(a.poq)-sum(a.rcvq)>0 order by b.aname,a.sch_Dt";

                    SQuery = mq1 + mq2;
                    fgen.drillQuery(0, SQuery, frm_qstr, "4#5#6#7#8#", "3#4#5#6#7#8#", "350#100#100#100#100#100#");
                    headerN = "Pending Purchase Schedule (" + fromdt + " to " + todt + ")";
                    break;

            }
            fgen.Fn_DrillReport(headerN, frm_qstr);
            hffield.Value = "-";
        }

        
        
        else
        {
            Checked_ok = "Y";
            //-----------------------------
            if (txtlbl4.Text.Trim().Length < 2)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Department Not Filled Correctly !!"); // COMMENTED BY MADHVI ON 23 JULY 2018 AS THERE IS NO DEPARTMENT SELECTION ON THE FORM
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + lbl4.Text + " Not Filled Correctly !!"); // BY MADHVI ON 23 JULY 2018
                return; // BY MADHVI ON 23 JULY 2018 AS WHEN NO SUPPLIER IS SELECTED , IT IS NOT COMING OUT OF THE FUN AND GO TO THE NEXT LINE
            }

            //for (i = 0; i < sg1.Rows.Count - 0; i++)
            //{
            //    if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) <= 0)
            //    {
            //        Checked_ok = "N";
            //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");
            //        i = sg1.Rows.Count;
            //    }
            //}

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
                        // fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!"); // ORIGINAL ... BY MADHVI ON 23 JULY 2018 AS ENTRY WORD COMES WRITTEN TWO TIMES 
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Date " + last_entdt + " , This " + lblheader.Text + " Date " + txtvchdate.Text.ToString() + ",Please Check !!"); // BY MADHVI ON 23 JULY 2018

                    }
                }
            }

            //last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            //if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            //{
            //    Checked_ok = "N";
            //    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            //}
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

                            fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully'13'Do you want to see the Print Preview ?");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);


                        }
                        else
                        {

                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully'13'Do you want to see the Print Preview ?");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_vnum + txtvchdate.Text.Trim() + "'");

                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);

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
        sg1_dt.Columns.Add(new DataColumn("sg1_t24", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t25", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t26", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t27", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t28", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t29", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t30", typeof(string)));

        sg1_dt.Columns.Add(new DataColumn("sg1_t31", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t32", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t33", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t34", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t35", typeof(string)));

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
        sg1_dr["sg1_t28"] = "-";
        sg1_dr["sg1_t29"] = "-";
        sg1_dr["sg1_t30"] = "-";

        sg1_dr["sg1_t31"] = "-";
        sg1_dr["sg1_t32"] = "-";
        sg1_dr["sg1_t33"] = "-";
        sg1_dr["sg1_t34"] = "-";
        sg1_dr["sg1_t35"] = "-";

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
                    fgen.Fn_open_mseek("Select Item", frm_qstr);
                    //fgen.Fn_open_mseek("Select Item", frm_qstr);
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
        // fgen.Fn_open_sseek("Select Customer ", frm_qstr); // COMMENTED BY MADHVI ON 23 JULY 2018
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr); // BY MADHVI ON 23 JULY 2018
    }
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
    protected void btnlbl7_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        // fgen.Fn_open_sseek("Select Deptt ", frm_qstr);  // COMMENTED BY MADHVI ON 23 JULY 2018
        fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr);  // BY MADHVI ON 23 JULY 2018
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
                oporow["orignalbr"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["SRNO"] = i;
                oporow["acode"] = txtlbl4.Text;

                oporow["irate"] = sg1.Rows[i].Cells[0].Text.Trim();

                //fgen.make_double(((HiddenField)sg1.Rows[i].FindControl("sg1_h1")).Value);

                oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim();

                oporow["partname"] = sg1.Rows[i].Cells[14].Text.Trim();
                oporow["partnum"] = sg1.Rows[i].Cells[15].Text.Trim();

                oporow["ponum"] = sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 6);
                oporow["podate"] = sg1.Rows[i].Cells[16].Text.Trim().Substring(7, 10);

                string popsql = "";
                popsql = "select type from pomas where trim(upper(branchcd))='" + frm_mbr + "' and trim(ordno)||'-'||to_char(orddt,'dd/mm/yyyy')='" + sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 17) + "' and trim(upper(acode))='" + txtlbl4.Text + "' and trim(upper(icode))='" + sg1.Rows[i].Cells[13].Text.Trim() + "'";
                oporow["sonum"] = fgen.seek_iname(frm_qstr, frm_cocd, popsql, "type");
                //oporow["sonum"] = sg1.Rows[i].Cells[16].Text.Trim().Substring(0, 6);
                oporow["sodate"] = vardate;

                oporow["line_rmk"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text;
                oporow["total"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text);

                oporow["day1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                oporow["day2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text);
                oporow["day3"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                oporow["day4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text);
                oporow["day5"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text);
                oporow["day6"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text);
                oporow["day7"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text);
                oporow["day8"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text);
                oporow["day9"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text);
                oporow["day10"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text);

                oporow["day11"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text);
                oporow["day12"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text);
                oporow["day13"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text);
                oporow["day14"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text);
                oporow["day15"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t17")).Text);
                oporow["day16"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t18")).Text);
                oporow["day17"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t19")).Text);
                oporow["day18"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t20")).Text);
                oporow["day19"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t21")).Text);
                oporow["day20"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t22")).Text);

                oporow["day21"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t23")).Text);
                oporow["day22"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t24")).Text);
                oporow["day23"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t25")).Text);
                oporow["day24"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t26")).Text);
                oporow["day25"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t27")).Text);
                oporow["day26"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t28")).Text);
                oporow["day27"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t29")).Text);
                oporow["day28"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t30")).Text);
                oporow["day29"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t31")).Text);
                oporow["day30"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t32")).Text);
                oporow["day31"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t33")).Text);
                oporow["wk1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t34")).Text);
                oporow["wk2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t35")).Text);
                oporow["remarks"] = txtrmk.Text.ToString().Trim().Replace("'", " ").Replace("\"", " ");
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
                oporow["app_by"] = "-";
                oporow["app_dt"] = vardate;

                // BY MADHVI ON 23 JULY 2018 ---------------
                oporow["Psize"] = 0;
                oporow["gsm"] = 0;
                oporow["wk3"] = 0;
                oporow["wk4"] = 0;
                oporow["amdtno"] = 0;
                oporow["qtyord"] = 0;
                oporow["addl_rmk"] = "-";
                //oporow["pr_dtl"] = "-";
                // -----------------------------------------
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
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F50111":
                SQuery = "SELECT '66' AS FSTR,'Purchase Schedule' as NAME,'66' AS CODE FROM dual";
                break;

        }
    }
    //------------------------------------------------------------------------------------   
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
        hffield.Value = "PENDSCH";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMREPID", frm_formID + "_3");
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
}