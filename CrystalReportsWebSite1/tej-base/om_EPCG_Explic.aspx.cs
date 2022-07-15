using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_EPCG_Explic : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", xStartDt = "", Enable = "", ivch_tbl = "", sale_tbl = "", sec_tbl = "", cond = "", ship_bill_no_dt = "";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it, mq0, mq1, mq2, mq3, mq4;
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
                doc_addl.Value = "1";

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();

            typePopup = "N";
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

                ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t3")).Attributes.Add("autocomplete", "off");

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

        // to hide and show to tab panel      
        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnlist.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnprint.Disabled = false;
        create_tab();
        sg1_add_blankrows();
        btnlbl4.Enabled = false;
        btnlbl7.Enabled = false;
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; btnprint.Disabled = true;
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
        frm_tabname = "WB_LICREC";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "31");
        lblheader.Text = "EPCG Export Licence Adj";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
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

            case "LICNO":
                SQuery = "Select Distinct a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.licno)||trim(a.ciname)||TRIM(SRNO) as fstr,a.licno as licence_no,to_char(licdt,'dd/mm/yyyy') as licence_dt, trim(a.vchnum) as doc_no,to_char(a.vchdate,'dd/mm/yyyy') as doc_dt,a.ciname as description ,TO_CHAR(a.impvalid,'DD/MM/YYYY') as validity,a.imp_val as import_value,a.imp_qty as import_qty,to_char(vchdate,'yyyymmdd') as vdd from wb_licrec a where a.branchcd='" + frm_mbr + "' and a.type='11' order by vdd";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[17].Text.ToString() + gr.Cells[18].Text.ToString() + "'";
                    else col1 = "'" + gr.Cells[17].Text.ToString() + gr.Cells[18].Text.ToString() + "'";
                }
                if (col1.Length <= 0) col1 = "'-'";
                //SQuery = "select a.billno||a.bill_dt||TRIM(A.INVNO)||TO_CHAR(A.INVDATE,'DD/MM/YYYY')||trim(a.icode) as fstr,TRIM(A.INVNO) AS INV_NO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,a.billno,a.bill_dt,a.icode, b.iname from (select distinct A.VCHNUM AS INVNO,A.VCHDATE AS INVDATE,a.ponum as billno,to_char(a.podate,'dd/mm/yyyy') as bill_dt ,b.icode as icode,1 as qty from ivoucherp a,matl_spec b where trim(a.tc_no)||to_char(a.refdate,'dd/mm/yyyy') =trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and  a.branchcd='" + frm_mbr + "' and a.type='4F' and a.vchdate " + DateRange + " union all select distinct invno,invdate,billno,to_char(bill_dt,'dd/mm/yyyy') as bill_dt ,icode,-1 as qty from wb_licrec where branchcd='" + frm_mbr + "' and type='30' and vchdate " + DateRange + ") a , item b where trim(a.icode)=trim(b.icode)  having sum(qty)>0  group by TRIM(A.INVNO) ,TO_CHAR(A.invdate,'DD/MM/YYYY'),a.billno,a.bill_dt,a.icode,a.invno,a.invdate,b.iname order by billno";
                SQuery = "select trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')||a.billno||to_char(a.bill_dt,'dd/mm/yyyy') as fstr, a.invno ,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.billno,to_char(a.bill_dt,'dd/mm/yyyy') as bill_dt from (select distinct a.vchnum as invno,a.vchdate as invdate,b.ship_billno as billno,to_date(b.ship_billdt,'dd/mm/yyyy') as bill_dt,1 as qty from ivoucherp a ,wb_exp_imp b  where trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.entry_no_bill)||to_char(b.entry_dt_bill,'dd/mm/yyyy') and  a.type='4F' and a.branchcd='" + frm_mbr + "' and b.type='EX' and a.vchdate BETWEEN to_date('" + txtlicdt.Text.Trim() + "','DD/MM/YYYY') AND to_date('" + txtlicvalid.Text.Trim() + "','DD/MM/YYYY') union all select distinct invno,invdate,billno,bill_dt,-1 as qty from wb_licrec where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' )  a  where  trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy') not in (" + col1 + ") group by a.invno ,to_char(a.invdate,'dd/mm/yyyy'),a.billno,to_char(a.bill_dt,'dd/mm/yyyy') having sum(qty)>0 order by fstr"; //old
                // ORIGINAL   SQuery = "select trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')||a.billno||to_char(a.bill_dt,'dd/mm/yyyy') as fstr, a.invno ,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.billno,to_char(a.bill_dt,'dd/mm/yyyy') as bill_dt,a.fob,a.duty,a.fob_inr,a.fob_foreign from (select distinct a.vchnum as invno,a.vchdate as invdate,b.ship_billno as billno,to_date(b.ship_billdt,'dd/mm/yyyy') as bill_dt,B.FOB,B.DUTY,B.FOB_INR,B.FOB_FOREIGN,1 as qty from ivoucherp a ,wb_exp_imp b  where trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.entry_no_bill)||to_char(b.entry_dt_bill,'dd/mm/yyyy') and  a.type='4F' and a.branchcd='" + frm_mbr + "' and b.type='EX' and a.vchdate BETWEEN to_date('" + txtlicdt.Text.Trim() + "','DD/MM/YYYY') AND to_date('" + txtlicvalid.Text.Trim() + "','DD/MM/YYYY') union all select distinct invno,invdate,billno,bill_dt,0 AS FOB,'-' AS DUTY,0 AS FOB_INR,0 AS FOB_FOREIGN,-1 as qty from wb_licrec where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' )  a  where  trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy') not in (" + col1 + ") group by a.invno ,to_char(a.invdate,'dd/mm/yyyy'),a.billno,to_char(a.bill_dt,'dd/mm/yyyy'),a.fob,a.duty,a.fob_inr,a.fob_foreign  having sum(qty)>0 order by fstr";//by yogita

                mq0 = "select count(*) as cnt from salep where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + DateRange + "";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count > 0)
                {
                    if (fgen.make_double(dt.Rows[0]["cnt"].ToString()) > 0)
                    {
                        ivch_tbl = "Ivoucherp";
                        ship_bill_no_dt = "b.ship_billno as billno,to_date(b.ship_billdt,'dd/mm/yyyy') as bill_dt,B.FOB,B.DUTY,B.FOB_INR,B.FOB_FOREIGN";
                        sec_tbl = "wb_exp_imp b";
                        cond = " trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.entry_no_bill)||to_char(b.entry_dt_bill,'dd/mm/yyyy') and b.type='EX' ";
                    }
                    else
                    {
                        ivch_tbl = "Ivoucher";
                        ship_bill_no_dt = "b.pvt_mark as billno,b.tptbill_dt as bill_dt,0 as fob,'-' as duty,0 as fob_inr,0 as fob_foreign";
                        sec_tbl = "sale b";
                        cond = " a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=b.branchcd||b.type||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and nvl(trim(b.pvt_mark),'-')!='-' ";
                    }
                }
                SQuery = "select trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')||a.billno||to_char(a.bill_dt,'dd/mm/yyyy') as fstr, a.invno ,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.billno,to_char(a.bill_dt,'dd/mm/yyyy') as bill_dt,a.fob,a.duty,a.fob_inr,a.fob_foreign from (select distinct a.vchnum as invno,a.vchdate as invdate," + ship_bill_no_dt + ",1 as qty from " + ivch_tbl + " a ," + sec_tbl + "  where " + cond + " and a.type='4F' and a.branchcd='" + frm_mbr + "' and a.vchdate BETWEEN to_date('" + txtlicdt.Text.Trim() + "','DD/MM/YYYY') AND to_date('" + txtlicvalid.Text.Trim() + "','DD/MM/YYYY') union all select distinct invno,invdate,billno,bill_dt,0 AS FOB,'-' AS DUTY,0 AS FOB_INR,0 AS FOB_FOREIGN,-1 as qty from wb_licrec where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' )  a  where  trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy') not in (" + col1 + ") group by a.invno ,to_char(a.invdate,'dd/mm/yyyy'),a.billno,to_char(a.bill_dt,'dd/mm/yyyy'),a.fob,a.duty,a.fob_inr,a.fob_foreign  having sum(qty)>0 order by fstr";
                break;

            case "SG1_ROW_ADD1":
            case "SG1_ROW_ADD_E1":
                string stage = "0";
                stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text;
                SQuery = "";
                break;
            case "SG1_ROW_ITEM":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (((TextBox)gr.FindControl("sg1_t10")).Text.Trim().Length > 1)
                    {
                        if (col1.Length > 0) col1 = col1 + ",'" + ((TextBox)gr.FindControl("sg1_t10")).Text.Trim() + "'";
                        else col1 = "'" + ((TextBox)gr.FindControl("sg1_t10")).Text.Trim() + "'";
                    }
                }
                if (col1.Length <= 0) col1 = "'-'";
                SQuery = "select trim(ciname) as fstr, trim(ciname) as item_Description from wb_licrec where branchcd='" + frm_mbr + "' and type='11' and trim(ciname) not in (" + col1 + ")";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "Print_E":
                SQuery = "select distinct a.branchcd||a.type||trim(a.licno)||to_char(a.licdt,'dd/mm/yyyy')||trim(a.term) as fstr,a.licno as licence_no,to_char(a.licdt,'dd/mm/yyyy') as licence_dt,trim(a.term) as item_name,to_char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a WHERE  a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' and vchdate " + DateRange + " ORDER BY vdd DESC";
                break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "SELECT distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_dt,A.LICNO,TO_CHAR(A.LICDT,'DD/MM/YYYY') AS LIC_DT,A.DGFT_FILE,a.ENT_BY ,to_char(a.ENT_dt,'dd/mm/yyyy') as ENT_dt,to_char(a.vchdate,'yyyymmdd') as vdd FROM " + frm_tabname + " A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE  " + DateRange + " ORDER BY vdd DESC,entry_no DESC";
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
        // DDBind();
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            frm_vty = "31";
            lbl1a.Text = frm_vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);

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
    void newCase(string vty)
    {
        #region
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + "", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtent_by.Text = frm_uname;
        txtent_dt.Text = vardate;
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
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            hffield.Value = "Edit";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Edit", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        Cal();
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
            fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus();
            return;
        }

        if (txtlbl4.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Select " + lbl4.Text);
            txtlbl4.Focus();
            return;
        }
        //if (fgen.make_double(txtlbl3.Text.Trim()) >fgen.make_double(txtbalqty.Text.Trim())) 
        //{
        //    fgen.msg("-", "AMSG", "Please Check Your Value!! '13' Balance Value Cannot be Less than Selected Value "); return;
        //}
        if (sg1.Rows.Count <= 1)
        {
            fgen.msg("-", "AMSG", "Please Select Invoices");
            return;
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
            fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Delete", frm_qstr);
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
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
        ViewState["sg1"] = null;
        setColHeadings();
        //DDClear();
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
        fgen.Fn_open_sseek("Select Licence for Print", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
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
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' AND FINPKFLD LIKE '" + frm_tabname + "%'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(6, 10), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                //fgen.save_info(frm_qstr,frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0,6),vardate, frm_uname, frm_vty, lblheader.Text.Trim() + "Delete");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6) + "");
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
                        //txtlbl4a.Text = dt.Rows[i]["text"].ToString().Trim();
                        //txtlbl2.Text = dt.Rows[i]["frm_header"].ToString().Trim();
                        //txtlbl7.Text = dt.Rows[0]["ent_id"].ToString().Trim();
                        //txtlbl7a.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[i]["frm_name"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["text"].ToString().Trim();
                            sg1_dr["sg1_h3"] = dt.Rows[i]["frm_name"].ToString().Trim();
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
                    fgen.Fn_open_sseek("Select Licence No to Print", frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select a.* from " + frm_tabname + " a   where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4.Text = dt.Rows[0]["licno"].ToString().Trim();
                        txtlicdt.Text = Convert.ToDateTime(dt.Rows[0]["licdt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl7.Text = dt.Rows[0]["DGFT_FILE"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["VAL_ADD"].ToString().Trim();
                        txtcurrqty.Text = dt.Rows[0]["EXP_QTY"].ToString().Trim();
                        txtcurrval.Text = dt.Rows[0]["EXP_VAL"].ToString().Trim();
                        txtbalqty.Text = dt.Rows[0]["balqty"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                        txtent_by.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        txtent_dt.Text = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtitemdesc.Text = dt.Rows[0]["term"].ToString().Trim().ToUpper();
                        txt_exp_oblig_imp.Text = dt.Rows[0]["qtyout"].ToString().Trim();
                        txtlicvalid.Text = Convert.ToDateTime(dt.Rows[0]["refdate"].ToString().Trim().ToUpper()).ToString("dd/MM/yyyy");
                        txtlbl3.Text = dt.Rows[0]["num3"].ToString().Trim();
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
                            sg1_dr["sg1_f1"] = dt.Rows[i]["billno"].ToString().Trim();
                            sg1_dr["sg1_f2"] = Convert.ToDateTime(dt.Rows[i]["bill_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_f3"] = dt.Rows[i]["invno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = Convert.ToDateTime(dt.Rows[i]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_f6"] = "-";
                            sg1_dr["sg1_t1"] = dt.Rows[i]["cif_val"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["num1"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["obsv2"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["num2"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["num4"].ToString().Trim();
                            sg1_dr["sg1_t6"] = "";
                            sg1_dr["sg1_t10"] = dt.Rows[i]["ciname"].ToString().Trim(); ;
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
                        btnlbl4.Enabled = false;
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", frm_formID);
                    fgen.fin_esales_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    #region
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    //SQuery = "Select b.iname,b.cpartno,b.cdrgno,b.unit,trim(a.srno) as morder1,a.*,to_chaR(a.invdate,'dd/mm/yyyy') as pinvdate,to_chaR(a.vchdate,'dd/mm/yyyy') as pvchdate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum||trim(a.icode)||trim(a.srno)='" + col1 + "' ORDER BY A.srno";
                    SQuery = "select  EMPCODE,NAME, DEPTT_TEXT,DESG_TEXT,DTJOIN from empmas  where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + col1 + "' order by grade";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl4.Text = col1;
                        //txtlbl4a.Text = col2;
                    }
                    dt.Dispose();
                    // SQuery = "Select * from inspmst a where a.branchcd='" + frm_mbr + "' and a.icode='" + txtlbl7.Text + "' ORDER BY A.srno";
                    SQuery = "select  EMPCODE AS COL1,NAME AS COL2, DEPTT_TEXT AS COL3,DESG_TEXT AS COL4,TO_CHAR(DTJOIN,'dd/MM/yyyy') AS COL6,ENT_DT,ENT_BY from empmas  where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + col1 + "' order by grade";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
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
                            sg1_dr["sg1_srno"] = i + 1;
                            sg1_dr["sg1_f1"] = dt.Rows[i]["col1"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["col2"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["col6"].ToString().Trim();
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
                    #endregion
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

                case "LICNO":
                    if (col1.Length <= 0) return;
                    //SQuery = "Select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.licno) as fstr,a.licno as lic_no,to_char(a.licdt,'dd/mm/yyyy') as lic_dt,a.impqty ,a.impval,a.cif_val as val,to_char(a.vchdate,'yyymmdd') as vdd from wb_licrec a b.id='T1' and a.flag='IM' and  a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.licno)='" + col1 + "'order by vdd";
                    SQuery = "Select a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.licno)||TRIM(SRNO) as fstr,trim(a.ciname) as ciname,a.num1,a.num2,a.VAL_ADD,a.licno as lic_no,to_char(a.licdt,'dd/mm/yyyy') as lic_dt,a.qtyin ,a.imp_val,a.cif_val as val,to_char(a.IMPVALID,'dd/mm/yyyy') as IMPVALID, to_char(a.vchdate,'yyymmdd') as vdd from wb_licrec a where  a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.licno)||trim(a.ciname)||TRIM(SRNO)='" + col1 + "' order  by vdd";

                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl4.Text = dt.Rows[0]["lic_no"].ToString().Trim();
                        txtlicdt.Text = dt.Rows[0]["lic_dt"].ToString().Trim();
                        txtcurrqty.Text = dt.Rows[0]["qtyin"].ToString().Trim();
                        txtcurrval.Text = dt.Rows[0]["imp_val"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[i]["num1"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[i]["num2"].ToString().Trim();
                        txtitemdesc.Text = dt.Rows[i]["ciname"].ToString().Trim();
                        txtlicvalid.Text = dt.Rows[i]["IMPVALID"].ToString().Trim();

                        //mq0 = "select balqty from wb_licrec where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and licno='" + txtlbl4.Text + "'  AND LICDT=TO_DATE('" + txtlicdt.Text + "','DD/MM/YYYY') AND CINAME ='" + txtitemdesc.Text.Trim() + "'";
                        //mq1 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "balqty");
                        //if (mq1 != "0")
                        //{
                        //    txtbalqty.Text = mq1;
                        //}
                        //else
                        //{
                        //    txtbalqty.Text = dt.Rows[0]["qtyin"].ToString().Trim();
                        //}
                        mq1 = "select sum(qtyout) as exp_imp from WB_LICREC where branchcd='" + frm_mbr + "' and type='21' and licno='" + txtlbl4.Text + "' and to_char(licdt,'dd/mm/yyyy')='" + txtlicdt.Text + "'";
                        mq2 = fgen.seek_iname(frm_qstr, frm_cocd, mq1, "exp_imp");
                        if (mq2 != "0")
                        {
                            txt_exp_oblig_imp.Text = mq2;
                        }
                        else
                        {
                            txt_exp_oblig_imp.Text = "0";
                        }
                    }
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    SQuery = "select distinct a.ponum as billno,to_char(a.podate,'dd/mm/yyyy') as bill_dt ,trim(b.icode) as icode,a.invno as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,SUM(a.iqtyout) AS IQTYOUT,c.iname,sum(a.iqtyout*a.iqty_chlwt) as value  from ivoucherp a,matl_spec b,item c where trim(a.tc_no)||to_char(a.refdate,'dd/mm/yyyy') =trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and trim(b.icode)=trim(c.icode) and  a.branchcd='" + frm_mbr + "' and a.type='4F' and  a.ponum||to_char(a.podate,'dd/mm/yyyy')||a.invno||to_char(a.invdate,'dd/mm/yyyy')||trim(b.icode) ='" + col1 + "' GROUP BY a.ponum ,to_char(a.podate,'dd/mm/yyyy') ,trim(b.icode) ,a.invno ,a.iqty_chlwt,to_char(a.invdate,'dd/mm/yyyy'),c.iname ";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (col1.Length <= 0) return;
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in Hidden Field 
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[d]["billno"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[d]["bill_dt"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[d]["invno"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[d]["invdate"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[d]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["iname"].ToString().Trim();

                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = (fgen.make_double(dt.Rows[d]["iqtyout"].ToString().Trim())).ToString();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = (fgen.make_double(dt.Rows[d]["value"].ToString().Trim())).ToString();
                    }
                    setColHeadings();
                    break;

                case "SG1_ROW_ITEM":
                    #region for gridview 1
                    //if (col1.Length <= 0) return;
                    //if (ViewState["sg1"] != null)
                    //{
                    //    dt = new DataTable();
                    //    sg1_dt = new DataTable();
                    //    dt = (DataTable)ViewState["sg1"];
                    //    z = dt.Rows.Count - 1;
                    //    sg1_dt = dt.Clone();
                    //    sg1_dr = null;
                    //    for (i = 0; i < dt.Rows.Count - 1; i++)
                    //    {
                    //        sg1_dr = sg1_dt.NewRow();
                    //        sg1_dr["sg1_srno"] = Convert.ToInt32(sg1.Rows[i].Cells[14].Text);
                    //        sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.ToString();
                    //        sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.ToString();
                    //        sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.ToString();
                    //        sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.ToString();
                    //        sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.ToString();
                    //        sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.ToString();
                    //        sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.ToString();
                    //        sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.ToString();
                    //        sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.ToString();
                    //        sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.ToString();
                    //        sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.ToString();
                    //        sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.ToString();
                    //        sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.ToString();
                    //        sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.ToString();
                    //        sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.ToString();
                    //        sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.ToString();
                    //        sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                    //        sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                    //        sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                    //        sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                    //        sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                    //        sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                    //        sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                    //        sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                    //        sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                    //        sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                    //        sg1_dt.Rows.Add(sg1_dr);
                    //    }
                    //    dt = new DataTable();

                    //    SQuery = "select trim(ciname) as ciname from wb_licrec where branchcd!='DD' and type='10' and flag='IM' and trim(ciname)= '" + col1 + "'";

                    //    string mq1;
                    //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //    for (int d = 0; d < dt.Rows.Count; d++)
                    //    {
                    //        sg1_dr = sg1_dt.NewRow();
                    //        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                    //        sg1_dr["sg1_h1"] = "-";
                    //        sg1_dr["sg1_h2"] = "-";
                    //        sg1_dr["sg1_h3"] = "-";
                    //        sg1_dr["sg1_h4"] = "-";
                    //        sg1_dr["sg1_h5"] = "-";
                    //        sg1_dr["sg1_h6"] = "-";
                    //        sg1_dr["sg1_h7"] = "-";
                    //        sg1_dr["sg1_h8"] = "-";
                    //        sg1_dr["sg1_h9"] = "-";
                    //        sg1_dr["sg1_h10"] = "-";
                    //        sg1_dr["sg1_t10"] = dt.Rows[d]["ciname"].ToString().Trim();
                    //        sg1_dt.Rows.Add(sg1_dr);
                    //    }
                    //}
                    // sg1_add_blankrows();
                    //ViewState["sg1"] = sg1_dt;
                    //sg1.DataSource = sg1_dt;
                    //sg1.DataBind();
                    //dt.Dispose(); sg1_dt.Dispose();
                    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    //setColHeadings();
                    //DDBind();
                    #endregion
                    if (col1.Length <= 0) return;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Text = col1;
                    hf2.Value = col1;
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    if (ViewState["sg1"] != null)
                    {
                        sg1_dt = new DataTable();
                        dt = (DataTable)ViewState["sg1"];
                        z = dt.Rows.Count - 1;
                        sg1_dt = dt.Clone();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = Convert.ToInt32(sg1.Rows[i].Cells[14].Text);
                            sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.ToString();
                            sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.ToString();
                            sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.ToString();
                            sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.ToString();
                            sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.ToString();
                            sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.ToString();
                            sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.ToString();
                            sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.ToString();
                            sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.ToString();
                            sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.ToString();
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[15].Text.ToString();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[16].Text.ToString();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[17].Text.ToString();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[18].Text.ToString();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[19].Text.ToString();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[20].Text.ToString();
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        //SQuery = "select distinct a.ponum as billno,to_char(a.podate,'dd/mm/yyyy') as bill_dt ,trim(b.icode) as icode,a.invno as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,SUM(a.iqtyout) AS IQTYOUT,c.iname from ivoucherp a,matl_spec b,item c where trim(a.tc_no)||to_char(a.refdate,'dd/mm/yyyy') =trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and trim(b.icode)=trim(c.icode) and  a.branchcd='" + frm_mbr + "' and a.type='4F' and  a.ponum||to_char(a.podate,'dd/mm/yyyy')||a.invno||to_char(a.invdate,'dd/mm/yyyy')||trim(b.icode) in (" + col1 + ") GROUP BY a.ponum ,to_char(a.podate,'dd/mm/yyyy') ,trim(b.icode) ,a.invno ,to_char(a.invdate,'dd/mm/yyyy'),c.iname ";
                        SQuery = "select trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')||a.billno||to_char(a.bill_dt,'dd/mm/yyyy') as fstr, a.invno ,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.billno,to_char(a.bill_dt,'dd/mm/yyyy') as bill_dt,sum(a.foreign_val) as foreign_val from (select distinct a.vchnum as invno,a.vchdate as invdate,b.ship_billno as billno,to_date(b.ship_billdt,'dd/mm/yyyy') as bill_dt,b.foreign_val as foreign_val,1 as qty from ivoucherp a ,wb_exp_imp b where trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.entry_no_bill)||to_char(b.entry_dt_bill,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and  a.type='4F' and b.type='EX' union all select distinct invno,invdate,billno,bill_dt,cif_val as foreign_val,-1 as qty from wb_licrec where branchcd='" + frm_mbr + "' and type='31' )  a where trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')||a.billno||to_char(a.bill_dt,'dd/mm/yyyy') in (" + col1 + ")  having sum(qty)>0 group by a.invno ,to_char(a.invdate,'dd/mm/yyyy'),a.billno,to_char(a.bill_dt,'dd/mm/yyyy') "; //old
                        SQuery = "select trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')||a.billno||to_char(a.bill_dt,'dd/mm/yyyy') as fstr, a.invno ,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.billno,to_char(a.bill_dt,'dd/mm/yyyy') as bill_dt,sum(a.foreign_val) as foreign_val,sum(a.fob) as fob,a.duty,sum(a.fob_inr) as fob_inr,sum(a.fob_foreign) as fob_foreign from (select distinct a.vchnum as invno,a.vchdate as invdate,b.ship_billno as billno,to_date(b.ship_billdt,'dd/mm/yyyy') as bill_dt,B.FOB,B.DUTY,B.FOB_INR,B.FOB_FOREIGN,b.foreign_val as foreign_val,1 as qty from ivoucherp a ,wb_exp_imp b where trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.entry_no_bill)||to_char(b.entry_dt_bill,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and  a.type='4F' and b.type='EX' union all select distinct invno,invdate,billno,bill_dt,0 AS FOB,'-' AS DUTY,0 AS FOB_INR,0 AS FOB_FOREIGN,cif_val as foreign_val,-1 as qty from wb_licrec where branchcd='" + frm_mbr + "' and type='31' )  a where trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')||a.billno||to_char(a.bill_dt,'dd/mm/yyyy') in (" + col1 + ")  having sum(qty)>0 group by a.invno ,to_char(a.invdate,'dd/mm/yyyy'),a.billno,to_char(a.bill_dt,'dd/mm/yyyy'),a.duty ";//yogita

                        mq0 = "select count(*) as cnt from salep where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + DateRange + "";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        if (dt.Rows.Count > 0)
                        {
                            if (fgen.make_double(dt.Rows[0]["cnt"].ToString()) > 0)
                            {
                                ivch_tbl = "Ivoucherp";
                                ship_bill_no_dt = "b.ship_billno as billno,to_date(b.ship_billdt,'dd/mm/yyyy') as bill_dt,B.FOB,B.DUTY,B.FOB_INR,B.FOB_FOREIGN,b.foreign_val as foreign_val";
                                sec_tbl = "wb_exp_imp b";
                                sale_tbl = "salep";
                                cond = " trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.entry_no_bill)||to_char(b.entry_dt_bill,'dd/mm/yyyy') and b.type='EX' ";
                            }
                            else
                            {
                                ivch_tbl = "Ivoucher";
                                ship_bill_no_dt = "b.pvt_mark as billno,b.tptbill_dt as bill_dt,0 as fob,'-' as duty,0 as fob_inr,0 as fob_foreign,0 as foreign_val";
                                sec_tbl = "sale b";
                                sale_tbl = "sale";
                                cond = " a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=b.branchcd||b.type||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and nvl(trim(b.pvt_mark),'-')!='-' ";
                            }
                        }
                        SQuery = "select trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')||a.billno||to_char(a.bill_dt,'dd/mm/yyyy') as fstr, a.invno ,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.billno,to_char(a.bill_dt,'dd/mm/yyyy') as bill_dt,sum(a.foreign_val) as foreign_val,sum(a.fob) as fob,a.duty,sum(a.fob_inr) as fob_inr,sum(a.fob_foreign) as fob_foreign from (select distinct a.vchnum as invno,a.vchdate as invdate," + ship_bill_no_dt + ",1 as qty from " + ivch_tbl + " a ," + sec_tbl + "  where " + cond + " and a.type='4F' and a.branchcd='" + frm_mbr + "' and a.vchdate BETWEEN to_date('" + txtlicdt.Text.Trim() + "','DD/MM/YYYY') AND to_date('" + txtlicvalid.Text.Trim() + "','DD/MM/YYYY') union all select distinct invno,invdate,billno,bill_dt,0 AS FOB,'-' AS DUTY,0 AS FOB_INR,0 AS FOB_FOREIGN,0 as foreign_val,-1 as qty from wb_licrec where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' )  a where trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy')||a.billno||to_char(a.bill_dt,'dd/mm/yyyy') in (" + col1 + ") group by a.invno ,to_char(a.invdate,'dd/mm/yyyy'),a.billno,to_char(a.bill_dt,'dd/mm/yyyy'),a.duty having sum(qty)>0 order by fstr";

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
                            sg1_dr["sg1_f1"] = dt.Rows[d]["billno"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["bill_dt"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["invno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["invdate"].ToString().Trim();
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_f6"] = "-";
                            sg1_dr["sg1_t1"] = fgen.make_double(dt.Rows[d]["foreign_val"].ToString().Trim());
                            sg1_dr["sg1_t2"] = fgen.make_double(dt.Rows[d]["fob"].ToString().Trim());
                            sg1_dr["sg1_t3"] = dt.Rows[d]["duty"].ToString().Trim();
                            sg1_dr["sg1_t4"] = fgen.make_double(dt.Rows[d]["fob_inr"].ToString().Trim());
                            sg1_dr["sg1_t5"] = fgen.make_double(dt.Rows[d]["fob_foreign"].ToString().Trim());
                            sg1_dr["sg1_t6"] = "";
                            sg1_dr["sg1_t7"] = "";
                            sg1_dr["sg1_t8"] = "";
                            mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(exc_item) as exc_item from " + sale_tbl + " where branchcd='" + frm_mbr + "' and type='4F' and trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + dt.Rows[d]["invno"].ToString().Trim() + dt.Rows[d]["invdate"].ToString().Trim() + "' ", "exc_item");
                            sg1_dr["sg1_t10"] = mq0;
                            //sg1_dr["sg1_t10"] = hf2.Value.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    setColHeadings();
                    Cal();
                    #endregion
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
                        for (i = 0; i < dt.Rows.Count - 1; i++)
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
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[15].Text.Trim();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[16].Text.Trim();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[17].Text.Trim();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[18].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[19].Text.Trim();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[20].Text.Trim();
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        for (i = 0; i < sg1.Rows.Count; i++)
                        {
                            sg1.Rows[i].Cells[14].Text = (i + 1).ToString();
                        }
                    }
                    #endregion
                    setColHeadings();
                    Cal();
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "sELECT distinct trim(a.Vchnum) as Entry_no,to_char(a.Vchdate,'dd/mm/yyyy') as Entry_Dt,a.billno,TO_CHAR(a.bill_Dt,'DD/MM/YYYY') AS BILL_dT,A.INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,trim(a.ciname) as item_desc,a.cif_val ,a.licno as licence_no,to_char(licdt,'dd/mm/yyyy') as licence_dt,A.DGFT_FILE,A.VAL_ADD,a.Ent_by,a.Ent_Dt,to_char(a.vchdate,'yyyymmdd') as vdd FROM " + frm_tabname + " a  WHERE a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='" + frm_vty + "' AND a.VCHDATE  " + PrdRange + " ORDER BY vdd DESC,entry_No DESC";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------            
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
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                    }
                }
            }

            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            }
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
                            frm_vnum = txtvchnum.Text.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            save_it = "N";
                            for (i = 0; i < sg1.Rows.Count - 0; i++)
                            {
                                save_it = "Y";
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

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl(); //DDClear();
                    }
                    catch (Exception ex)
                    {
                        fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N"; btnsave.Disabled = false;
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
        sg1_dt.Columns.Add(new DataColumn("sg1_f6", typeof(string)));
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
    }
    //------------------------------------------------------------------------------------
    public void sg1_add_blankrows()
    {
        if (sg1_dt != null)
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
            sg1_dr["sg1_f6"] = "-";
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

            sg1_dt.Rows.Add(sg1_dr);
        }
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
                    if (frm_cocd == "SAGM")
                    {
                        ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t1")).ReadOnly = true;
                        ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t2")).ReadOnly = true;
                        ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t3")).ReadOnly = true;
                        ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t4")).ReadOnly = true;
                        ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t5")).ReadOnly = true;
                        ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t6")).ReadOnly = true;
                    }
                    else
                    {
                        ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t1")).ReadOnly = false;
                        ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t2")).ReadOnly = false;
                        ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t3")).ReadOnly = false;
                        ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t4")).ReadOnly = false;
                        ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t5")).ReadOnly = false;
                        ((TextBox)sg1.Rows[sg1r].FindControl("sg1_t6")).ReadOnly = false;
                    }
                }
            }
            sg1.Columns[10].HeaderStyle.Width = 40;
            sg1.Columns[11].HeaderStyle.Width = 200;
            sg1.Columns[12].HeaderStyle.Width = 40;
            sg1.Columns[13].HeaderStyle.Width = 40;
            sg1.Columns[14].HeaderStyle.Width = 70;
            sg1.Columns[15].HeaderStyle.Width = 130;
            sg1.Columns[16].HeaderStyle.Width = 130;
            sg1.Columns[17].HeaderStyle.Width = 130;
            sg1.Columns[18].HeaderStyle.Width = 130;
            sg1.Columns[19].HeaderStyle.Width = 150;
            sg1.Columns[20].HeaderStyle.Width = 230;
            sg1.Columns[21].HeaderStyle.Width = 160;
            sg1.Columns[22].HeaderStyle.Width = 160;
            sg1.Columns[23].HeaderStyle.Width = 160;
            sg1.Columns[24].HeaderStyle.Width = 160;
            sg1.Columns[25].HeaderStyle.Width = 160;
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
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Invoice From The List");
                }
                break;

            case "SG1_ROW_ADD":
                if (txtlbl4.Text.Trim().Length <= 1)
                {
                    fgen.msg("-", "AMSG", "Please Select Licence Details");
                    return;
                }
                //if (sg1.Rows.Count > 0)
                //{
                //    mq0 = ((TextBox)sg1.Rows[Convert.ToInt32(index)].FindControl("sg1_t10")).Text;
                //    if (mq0.Trim().Length <= 1)
                //    {
                //        fgen.msg("-", "AMSG", "Please Select Item Description");
                //        return;
                //    }
                //}
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Invoice Detail", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Bill Of Entries", frm_qstr);
                }
                break;
            case "SG1_ROW_ITEM":
                //if (index < sg1.Rows.Count - 1)
                //{
                hf1.Value = index.ToString();
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                //----------------------------

                hffield.Value = "SG1_ROW_ITEM";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Item Description", frm_qstr);
                //}
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", "-");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", "-");
        hffield.Value = "LICNO";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TYPE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Result", frm_qstr);
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

    }
    //------------------------------------------------------------------------------------   
    void save_fun()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        for (i = 0; i < sg1.Rows.Count - 1; i++)
        {
            if (sg1.Rows[i].Cells[15].Text.Trim().Length > 1)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;  //div 1
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum.Trim().ToUpper();
                oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
                oporow["SRNO"] = i + 1;
                oporow["licno"] = txtlbl4.Text.Trim().ToUpper();//licno
                oporow["licdt"] = txtlicdt.Text.Trim().ToUpper();//licdt
                oporow["DGFT_FILE"] = txtlbl7.Text.Trim().ToUpper();
                oporow["VAL_ADD"] = fgen.make_double(txtlbl7a.Text.Trim().ToUpper());
                oporow["EXP_QTY"] = fgen.make_double(txtcurrqty.Text.Trim().ToUpper());
                oporow["EXP_VAL"] = fgen.make_double(txtcurrval.Text.Trim().ToUpper());
                oporow["acode"] = "-";
                oporow["QTYIN"] = "0";
                oporow["QTYOUT"] = fgen.make_double(txt_exp_oblig_imp.Text.Trim()); //fgen.make_double(txtlbl7a.Text.Trim());
                oporow["term"] = txtitemdesc.Text.Trim().ToUpper();
                //oporow["balqty"] = fgen.make_double(txtbalqty.Text.Trim()) - fgen.make_double(txtlbl3.Text.Trim());// balqty...old
                //  oporow["balqty"] = fgen.make_double(txtlbl7.Text.Trim()) - fgen.make_double(txtlbl3.Text.Trim());
                oporow["balqty"] = fgen.make_double(txt_exp_oblig_imp.Text.Trim()) - fgen.make_double(txtlbl3.Text.Trim());
                oporow["ciname"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim().ToUpper();
                oporow["billno"] = sg1.Rows[i].Cells[15].Text.Trim().ToUpper();
                oporow["bill_dt"] = sg1.Rows[i].Cells[16].Text.Trim().ToUpper();
                oporow["invno"] = sg1.Rows[i].Cells[17].Text.Trim().ToUpper();
                oporow["invdate"] = Convert.ToDateTime(sg1.Rows[i].Cells[18].Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
                oporow["icode"] = "-";
                oporow["cif_val"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper()); // qty(kgs)             
                oporow["num1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper());
                oporow["obsv2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper();
                oporow["num2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper());
                oporow["num4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper());
                oporow["refdate"] = Convert.ToDateTime(txtlicvalid.Text.Trim().ToUpper()).ToString("dd/MM/yyyy");
                oporow["num3"] = txtlbl3.Text.Trim();

                if (txtrmk.Text.Trim().Length > 300)
                {
                    oporow["REMARK"] = txtrmk.Text.Trim().ToUpper().Substring(0, 299);
                }
                else
                {
                    oporow["REMARK"] = txtrmk.Text.Trim().ToUpper();
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
                    oporow["edt_dt"] = vardate;
                }
                oDS.Tables[0].Rows.Add(oporow);
            }
        }
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "31");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------ 
    public void Cal()
    {
        double coltot = 0; double min = 0;
        for (int sg1r = 0; sg1r < sg1.Rows.Count - 1; sg1r++)
        {
            coltot += fgen.make_double(((TextBox)sg1.Rows[sg1r].FindControl("sg1_t1")).Text.Trim());
            txtlbl3.Text = coltot.ToString().Trim();
        }
        min = fgen.make_double(txt_exp_oblig_imp.Text) - coltot;
        txtbalqty.Text = min.ToString();
    }
    //------------------------------------------------------------------------------------
}