using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_ca_cost : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", mq0;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0; double rate = 0; int gridcount = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it, mq1, mq2, mq3, mq4, mq8, mq9;
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
            btnlist.Visible = false;
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
                // ONLY FOR THIS FORM
                //for (int i = 0; i < 10; i++)
                //{
                //    sg1.Columns[i].HeaderStyle.CssClass = "hidden";
                //    sg1.Rows[K].Cells[i].CssClass = "hidden";
                //}
                #endregion
                if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));
            }
            orig_name = orig_name.ToUpper();
            if (sR == tb_Colm)
            {
                // hidding column
                if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
                {
                    sg1.Columns[sR].Visible = false;
                }
                // Setting Heading Name
                // ONLY FOR THIS FORM
                //sg1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false; btnlbl4.Enabled = false; btnCal.Disabled = true;
        ImageButton1.Enabled = false; ImageButton2.Enabled = false; ImageButton3.Enabled = false; ImageButton4.Enabled = false; btnRFQ.Enabled = false; ImageButton6.Enabled = false;
        create_tab(); Img_Carburiser.Enabled = false; Img_Steel.Enabled = false;
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = true; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlbl4.Enabled = true; btnprint.Disabled = true; btnlist.Disabled = true; btnCal.Disabled = false; Img_Carburiser.Enabled = true; Img_Steel.Enabled = true;
        ImageButton1.Enabled = true; ImageButton2.Enabled = true; ImageButton3.Enabled = true; ImageButton4.Enabled = true; btnRFQ.Enabled = true; ImageButton6.Enabled = true;
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
        lblheader.Text = "Costing Sheet";
        doc_nf.Value = "vchnum";
        doc_df.Value = "vchdate";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "wb_cacost";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CA01");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
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
            case "RFQ":
                SQuery = "SELECT trim(a.fstr) as fstr,trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,TRIM(a.icode) AS CODE,TRIM(i.iname) AS ITEM_NAME,to_char(a.orddt,'yyyymmdd') as vdd  from  (select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,1 AS QTY from wb_sorfq where branchcd='" + frm_mbr + "' and type ='MC' and nvl(trim(app_by),'-')!='C' union all select distinct branchcd||'MC'||trim(INVNO)||to_char(INVDATE,'dd/mm/yyyy') as fstr,INVNO,INVDATE,icode,-1 AS QTY from wb_cacost where branchcd='" + frm_mbr + "' and type='CA01')a,item i where trim(a.icode)=trim(i.icode) GROUP BY trim(a.fstr),trim(a.ordno),to_char(a.orddt,'dd/mm/yyyy'),TRIM(a.icode),TRIM(i.iname),to_char(a.orddt,'yyyymmdd') HAVING SUM(QTY)>0 ORDER BY FSTR";
                SQuery = "SELECT trim(a.fstr)||trim(a.delv_item) as fstr,trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,TRIM(a.icode) AS CODE,TRIM(i.iname) AS ITEM_NAME,trim(a.amd_no) as child,to_char(a.orddt,'yyyymmdd') as vdd  from  (select distinct branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy') as fstr,ordno,orddt,icode,amd_no,delv_item,1 AS QTY from wb_sorfq where branchcd='" + frm_mbr + "' and type ='RF' and nvl(trim(app_by),'-')!='C' union all select distinct trim(pordno) as fstr,INVNO,INVDATE,icode,childcode,parentchild,-1 AS QTY from wb_cacost where branchcd='" + frm_mbr + "' and type='CA01')a,item i where trim(a.icode)=trim(i.icode) GROUP BY trim(a.fstr)||trim(a.delv_item),trim(a.ordno),to_char(a.orddt,'dd/mm/yyyy'),TRIM(a.icode),TRIM(i.iname),to_char(a.orddt,'yyyymmdd'),trim(a.amd_no) HAVING SUM(QTY)>0 ORDER BY FSTR";
                break;

            case "TACODE":
                SQuery = "select acode as fstr,trim(aname) as customer_name,acode as code,acode,addr1,addr2 from famst where length(trim(nvl(deac_by,'-')))<=1 and branchcd!='DD' and substr(Acode,1,2) ='16' order by aname";
                break;

            case "ICODE":
                SQuery = "select icode as fstr,trim(iname) as item_name,icode as code,cpartno,unit from item where length(trim(icode))>4 and substr(icode,1,2)>='7' order by item_name";
                break;

            case "BOX":
                SQuery = "select branchcd||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,name as boxname,num1 as lenght,num2 as breadth,num3 as height,to_char(vchdate,'yyyymmdd') as vdd from wb_master where id='CP19' order by vdd desc,entry_no desc";
                break;

            case "POWER":
                SQuery = "select branchcd||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,num1 as electricity_rate,to_char(vchdate,'yyyymmdd') as vdd from wb_master where id='CP17' order by vdd desc,entry_no desc";
                break;

            case "CONVERSION":
                SQuery = "select branchcd||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,num4 as loabour_cost,to_char(vchdate,'yyyymmdd') as vdd from wb_master where id='CP17' order by vdd desc,entry_no desc";
                break;

            case "METALLIC":
                SQuery = "select branchcd||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,name,to_char(vchdate,'yyyymmdd') as vdd from wb_master where id='CP18' order by vdd desc,entry_no desc";
                break;

            case "CORE":
                SQuery = "select branchcd||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,name,to_char(vchdate,'yyyymmdd') as vdd from wb_master where id='CP20' order by vdd desc,entry_no desc";
                break;

            case "STEEL":
                SQuery = "Select trim(icode) as fstr,trim(icode) as code,iname as name,unit,hscode from item where icode in ('14020003','14020005') order by code";
                break;

            case "CARBURISER":
                SQuery = "Select trim(icode) as fstr,trim(icode) as code,iname as name,unit,hscode from item where icode in ('10020046','10020017') order by code";
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (col1.Length > 0) col1 = col1 + ",'" + gr.Cells[3].Text.Trim() + "'";
                    else col1 = "'" + gr.Cells[3].Text.Trim() + "'";
                }
                if (col1.Length <= 0) col1 = "'-'";
                SQuery = "Select trim(icode) as fstr,trim(icode) as code,iname as name,unit,hscode from item where icode NOT IN (" + col1 + ") and length(trim(icode))>4 and substr(trim(icode),1,1)<2 order by code";
                break;

            case "New":
                SQuery = "Select 'Line1' as fstr,'Line 1' as line,'L1' as code from dual union all Select 'Line2' as fstr,'Line 2' as line,'L2' as code from dual";
                break;

            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "Print_E":
                SQuery = "select distinct a.branchcd||trim(a.type)||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Entry_no,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Dt,a.acode as code,f.aname as cust_name,a.childcode,a.icode as item_code,i.INAME as item_name,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst f,item i where trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' order by vdd desc,a.vchnum desc";
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Entry_no,to_char(a.vchdate,'dd/mm/yyyy') as Entry_Dt,a.acode as code,f.aname as cust_name,a.childcode,a.icode as item_code,i.INAME as item_name,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst f,item i where trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' order by vdd desc,a.vchnum desc";
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
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            frm_vty = "CA01";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
            typePopup = "N";
            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Line", frm_qstr);
            }
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
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "'", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl2.Text = frm_uname;
        txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl5.Text = "-";
        txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        SQuery = "select vchnum,vchdate,irate,trim(icode) as icode,to_char(vchdate,'yyyymmdd') as vdd from ivoucher where branchcd='" + frm_mbr + "'  and type like '0%' and icode in ('14010001','14020006','14020003','14020005') AND vchdate>(SYSDATE-600) order by vdd desc";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            txtFRate.Text = fgen.seek_iname_dt(dt, "icode='14010001'", "irate");
            txtPRate.Text = fgen.seek_iname_dt(dt, "icode='14020006'", "irate");//10010004
            //txtSRate.Text = fgen.seek_iname_dt(dt, "icode='14020003'", "irate");
            //txtCRate.Text = fgen.seek_iname_dt(dt, "icode='14020005'", "irate");// ACTUAL ICODE NOT KNOW
        }
        // WHEN EITHER THERE IS NO MRR IN PAST 600 DAYS OR RATE IS ZERO IN MRR
        mq0 = "select irate,trim(icode) as icode from item where icode in ('14010001','14020006','14020003','14020005') order by icode";
        dt2 = new DataTable();
        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
        if (txtFRate.Text == "0" || txtFRate.Text == "")
        {
            txtFRate.Text = fgen.seek_iname_dt(dt2, "icode='14010001'", "irate");
        }
        if (txtPRate.Text == "0" || txtPRate.Text == "")
        {
            txtPRate.Text = fgen.seek_iname_dt(dt2, "icode='14020006'", "irate");
        }
        //if (txtSRate.Text == "0" || txtSRate.Text == "")
        //{
        //    txtSRate.Text = fgen.seek_iname_dt(dt2, "icode='14020003'", "irate");
        //}
        //if (txtCRate.Text == "0" || txtCRate.Text == "")
        //{
        //    txtCRate.Text = fgen.seek_iname_dt(dt2, "icode='14020005'", "irate");
        //}
        create_tab();
        sg1_add_blankrows();
        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnRFQ.Focus();
        setColHeadings();
        // Popup asking for Copy from Older Data
        fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
        hffield.Value = "NEW_E";
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
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
        Cal();

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }
        if (txtRFQ.Text.Length <= 1)
        {
            fgen.msg("-", frm_qstr, "Please Select RFQ No.");
            btnRFQ.Focus(); return;
        }
        if (txtlbl4a.Text.Length <= 1)
        {
            fgen.msg("-", frm_qstr, "Please Select Customer");
            return;
        }
        if (txtIcode.Text.Length <= 1)
        {
            fgen.msg("-", frm_qstr, "Please Select Item");
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
        fgen.Fn_open_sseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
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
                mq1 = "select trim(pordno) as pordno from somasq where branchcd='" + frm_mbr + "' and type='FQ' and trim(pordno)='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                mq2 = fgen.seek_iname(frm_qstr, frm_cocd, mq1, "pordno");
                if (mq2 == "0")
                {
                    // for deleting test flag field from last table i.e type RF
                    string mq5, mq6, mq7, mq8;
                    mq4 = "select trim(a.pordno) as pordno,trim(a.pbasis) as pbasis from " + frm_tabname + " a where a.branchcd||trim(a.type)||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                    mq5 = fgen.seek_iname(frm_qstr, frm_cocd, mq4, "pordno");
                    mq6 = "update wb_sorfq set test='-' where branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + mq5 + "'"; ;
                    fgen.execute_cmd(frm_qstr, frm_cocd, mq6);

                    // for deleting test flag field from first table i.e type EC or ER
                    mq7 = fgen.seek_iname(frm_qstr, frm_cocd, mq4, "pbasis");
                    mq8 = "update wb_sorfq set test='R' where branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + mq7 + "'";
                    fgen.execute_cmd(frm_qstr, frm_cocd, mq8);

                    // Deleing data from Main Table
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                    // Deleing data from WSr Ctrl Table
                    fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                    // Saving Deleting History
                    fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty.Substring(2, 2), lblheader.Text.Trim() + " " + frm_vty + " Deleted");
                    fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                    clearctrl(); fgen.ResetForm(this.Controls);
                }
                else
                {
                    fgen.msg("-", "AMSG", "Quotation Entry Is Done.'13'Entry Cannot be deleted"); clearctrl(); fgen.ResetForm(this.Controls);
                }
            }
        }
        else if (hffield.Value == "NEW_E")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
            {
                hffield.Value = "COPY_OLD";
                make_qry_4_popup();
                fgen.Fn_open_sseek(lblheader.Text + " Entry For Copy", frm_qstr);
            }
            else
            {
                hffield.Value = "New";
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select Line", frm_qstr);
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
                    //newCase(fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY"));
                    hfLine.Value = col1;
                    btnRFQ.Focus();
                    break;

                case "COPY_OLD":
                    #region
                    SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt,to_chaR(a.edt_dt,'dd/mm/yyyy') as pedt_Dt,f.aname,i.iname,i.cpartno from " + frm_tabname + " a,famst f,item i where trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        //txtlbl4.Text = dt.Rows[0]["acode"].ToString().Trim();
                        //txtlbl4a.Text = dt.Rows[0]["aname"].ToString().Trim();
                        //txtIcode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        //txtIname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        //txtCpart.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                        //txtlbl2.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        //txtlbl3.Text = dt.Rows[0]["pent_Dt"].ToString().Trim();
                        //txtlbl5.Text = dt.Rows[0]["edt_by"].ToString().Trim();
                        //txtlbl6.Text = dt.Rows[0]["pedt_Dt"].ToString().Trim();
                        //txtRFQ.Text = dt.Rows[0]["invno"].ToString().Trim();
                        //txtRFQDt.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtMaterial.Text = dt.Rows[0]["MATERIAL"].ToString().Trim();
                        txtLength.Text = dt.Rows[0]["LENGTH"].ToString().Trim();
                        txtWidth.Text = dt.Rows[0]["WIDTH"].ToString().Trim();
                        txtHeight.Text = dt.Rows[0]["HEIGHT"].ToString().Trim();
                        //txtCast.Text = dt.Rows[0]["CAST"].ToString().Trim();
                        //txtCast_No.Text = dt.Rows[0]["CAST_MOULD"].ToString().Trim();
                        //txtBunch.Text = dt.Rows[0]["BUNCH"].ToString().Trim();
                        txtActual.Text = dt.Rows[0]["ACTUAL"].ToString().Trim();
                        txtPattern.Text = dt.Rows[0]["PATTERN"].ToString().Trim();
                        //txtRej.Text = dt.Rows[0]["REJ"].ToString().Trim();
                        txtNet.Text = dt.Rows[0]["NET_EFF"].ToString().Trim();
                        txtMixer.Text = dt.Rows[0]["MIXER"].ToString().Trim();
                        txtMould_Rt.Text = dt.Rows[0]["MOULDING_RATE"].ToString().Trim();
                        txtLabour.Text = dt.Rows[0]["LABOUR"].ToString().Trim();
                        txtMaint.Text = dt.Rows[0]["MAINT"].ToString().Trim();
                        txtFettling.Text = dt.Rows[0]["FETTLING"].ToString().Trim();
                        txtInterest.Text = dt.Rows[0]["INTEREST"].ToString().Trim();
                        txtDepr.Text = dt.Rows[0]["DEPR"].ToString().Trim();
                        txtOther.Text = dt.Rows[0]["OTHERS"].ToString().Trim();
                        txtSubTotal.Text = dt.Rows[0]["STOTAL"].ToString().Trim();
                        txtGrandTot.Text = dt.Rows[0]["GTOTAL"].ToString().Trim();
                        txtCast_Rt.Text = dt.Rows[0]["CAST_RT"].ToString().Trim();
                        txtElect.Text = dt.Rows[0]["ELECTRICITY"].ToString().Trim();
                        txtAux.Text = dt.Rows[0]["AUXULARY"].ToString().Trim();
                        txtMetling.Text = dt.Rows[0]["MELTING"].ToString().Trim();
                        txtPower.Text = dt.Rows[0]["POWER"].ToString().Trim();
                        txtCore_Wt.Text = dt.Rows[0]["CORE_WT"].ToString().Trim();
                        txtCore_Rt.Text = dt.Rows[0]["CORE_RT"].ToString().Trim();
                        txtCore_Rej.Text = dt.Rows[0]["CORE_REJ"].ToString().Trim();
                        txtCore_Cost.Text = dt.Rows[0]["CORE_COST"].ToString().Trim();
                        txtFCons.Text = dt.Rows[0]["FCONS"].ToString().Trim();
                        txtFContri.Text = dt.Rows[0]["FCONTRI"].ToString().Trim();
                        txtFRate.Text = dt.Rows[0]["FRATE"].ToString().Trim();
                        txtFWt.Text = dt.Rows[0]["FWT"].ToString().Trim();
                        txtFSi.Text = dt.Rows[0]["FSI"].ToString().Trim();
                        txtFMn.Text = dt.Rows[0]["FMN"].ToString().Trim();
                        txtFC.Text = dt.Rows[0]["FC"].ToString().Trim();
                        txtFMoly.Text = dt.Rows[0]["FMOLY"].ToString().Trim();
                        txtPCons.Text = dt.Rows[0]["PCONS"].ToString().Trim();
                        txtPContri.Text = dt.Rows[0]["PCONTRI"].ToString().Trim();
                        txtPRate.Text = dt.Rows[0]["PRATE"].ToString().Trim();
                        txtPWt.Text = dt.Rows[0]["PWT"].ToString().Trim();
                        txtPSi.Text = dt.Rows[0]["PSI"].ToString().Trim();
                        txtPMn.Text = dt.Rows[0]["PMN"].ToString().Trim();
                        txtPC.Text = dt.Rows[0]["PC"].ToString().Trim();
                        txtPMoly.Text = dt.Rows[0]["PMOLY"].ToString().Trim();
                        txtSCons.Text = dt.Rows[0]["SCONS"].ToString().Trim();
                        txtSContri.Text = dt.Rows[0]["SCONTRI"].ToString().Trim();
                        txtSRate.Text = dt.Rows[0]["SRATE"].ToString().Trim();
                        txtSWt.Text = dt.Rows[0]["SWT"].ToString().Trim();
                        txtSSi.Text = dt.Rows[0]["SSI"].ToString().Trim();
                        txtSMn.Text = dt.Rows[0]["SMN"].ToString().Trim();
                        txtSC.Text = dt.Rows[0]["SC"].ToString().Trim();
                        txtSMoly.Text = dt.Rows[0]["SMOLY"].ToString().Trim();
                        txtCCons.Text = dt.Rows[0]["CCONS"].ToString().Trim();
                        txtCContri.Text = dt.Rows[0]["CCONTRI"].ToString().Trim();
                        txtCRate.Text = dt.Rows[0]["CRATE"].ToString().Trim();
                        txtCWt.Text = dt.Rows[0]["CWT"].ToString().Trim();
                        txtCSi.Text = dt.Rows[0]["CSI"].ToString().Trim();
                        txtCMn.Text = dt.Rows[0]["CMN"].ToString().Trim();
                        txtCC.Text = dt.Rows[0]["CC"].ToString().Trim();
                        txtCMoly.Text = dt.Rows[0]["CMOLY"].ToString().Trim();
                        txtSubCons.Text = dt.Rows[0]["TOTCONS"].ToString().Trim();
                        txtSubContri.Text = dt.Rows[0]["TOTCONTRI"].ToString().Trim();
                        txtSubWt.Text = dt.Rows[0]["TOTWT"].ToString().Trim();
                        txtSubSi.Text = dt.Rows[0]["TOTSI"].ToString().Trim();
                        txtSubMn.Text = dt.Rows[0]["TOTMN"].ToString().Trim();
                        txtSubC.Text = dt.Rows[0]["TOTC"].ToString().Trim();
                        txtSubMoly.Text = dt.Rows[0]["TOTMOLY"].ToString().Trim();
                        txtReqSi.Text = dt.Rows[0]["RSI"].ToString().Trim();
                        txtReqMn.Text = dt.Rows[0]["RMN"].ToString().Trim();
                        txtReqC.Text = dt.Rows[0]["RC"].ToString().Trim();
                        txtReqMoly.Text = dt.Rows[0]["RMOLY"].ToString().Trim();
                        txtDiffSi.Text = dt.Rows[0]["DSI"].ToString().Trim();
                        txtDiffMn.Text = dt.Rows[0]["DMN"].ToString().Trim();
                        txtDiffC.Text = dt.Rows[0]["DC"].ToString().Trim();
                        txtDiffMoly.Text = dt.Rows[0]["DMOLY"].ToString().Trim();
                        txtMetContri.Text = dt.Rows[0]["MET_CONTRI"].ToString().Trim();
                        txtFeSiRec.Text = dt.Rows[0]["FESI_REC"].ToString().Trim();
                        txtFeSiReq.Text = dt.Rows[0]["FESI_REQ"].ToString().Trim();
                        txtFeSiRate.Text = dt.Rows[0]["FESI_RT"].ToString().Trim();
                        txtFeSiCost.Text = dt.Rows[0]["FESI_COST"].ToString().Trim();
                        txtFeMnRec.Text = dt.Rows[0]["FEMN_REC"].ToString().Trim();
                        txtFeMnReq.Text = dt.Rows[0]["FEMN_REQ"].ToString().Trim();
                        txtFeMnRate.Text = dt.Rows[0]["FEMN_RT"].ToString().Trim();
                        txtFeMnCost.Text = dt.Rows[0]["FEMN_COST"].ToString().Trim();
                        txtCSCRec.Text = dt.Rows[0]["CSC_REC"].ToString().Trim();
                        txtCSCReq.Text = dt.Rows[0]["CSC_REQ"].ToString().Trim();
                        txtCSCRate.Text = dt.Rows[0]["CSC_RT"].ToString().Trim();
                        txtCSCCost.Text = dt.Rows[0]["CSC_COST"].ToString().Trim();
                        txtMolyRec.Text = dt.Rows[0]["MOLY_REC"].ToString().Trim();
                        txtMolyReq.Text = dt.Rows[0]["MOLY_REQ"].ToString().Trim();
                        txtMolyRate.Text = dt.Rows[0]["MOLY_RT"].ToString().Trim();
                        txtMolyCost.Text = dt.Rows[0]["MOLY_COST"].ToString().Trim();
                        txtFeSiMGRec.Text = dt.Rows[0]["FESIMG_REC"].ToString().Trim();
                        txtFeSiMGReq.Text = dt.Rows[0]["FESIMG_REQ"].ToString().Trim();
                        txtFeSiMGRate.Text = dt.Rows[0]["FESIMG_RT"].ToString().Trim();
                        txtFeSiMGCost.Text = dt.Rows[0]["FESIMG_COST"].ToString().Trim();
                        txtFerroTot.Text = dt.Rows[0]["FERRO_TOT"].ToString().Trim();
                        txtMetTot.Text = dt.Rows[0]["META_TOT"].ToString().Trim();
                        txtStgWt.Text = dt.Rows[0]["STAGE_WT"].ToString().Trim();
                        txtMelting_Loss.Text = dt.Rows[0]["MELTING_LOSS"].ToString().Trim();
                        txtMelting_Loss_Wt.Text = dt.Rows[0]["MELTING_STAGE_WT"].ToString().Trim();
                        txtMasterAlloy1.Text = dt.Rows[0]["MAS_ALLOY1"].ToString().Trim();
                        txtMasterAlloy2.Text = dt.Rows[0]["MAS_ALLOY2"].ToString().Trim();
                        txtMasterAlloy.Text = dt.Rows[0]["MAS_ALLOY3"].ToString().Trim();
                        txtMasterAlloy_Wt.Text = dt.Rows[0]["MAS_ALLOY_WT"].ToString().Trim();
                        txtInnoculation1.Text = dt.Rows[0]["INNOCULATION1"].ToString().Trim();
                        txtInnoculation2.Text = dt.Rows[0]["INNOCULATION2"].ToString().Trim();
                        txtInnoculation3.Text = dt.Rows[0]["INNOCULATION3"].ToString().Trim();
                        txtInnoculationWt.Text = dt.Rows[0]["INNOCULATION_WT"].ToString().Trim();
                        txtNetYield.Text = dt.Rows[0]["YIELD_RET"].ToString().Trim();
                        txtTotMetRate.Text = dt.Rows[0]["TOT_METALLIC_RT"].ToString().Trim();
                        txtProfit1.Text = dt.Rows[0]["PROFIT1"].ToString().Trim();
                        txtProfit2.Text = dt.Rows[0]["PROFIT2"].ToString().Trim();
                        txtOver1.Text = dt.Rows[0]["OVER_HEAD1"].ToString().Trim();
                        txtOver2.Text = dt.Rows[0]["OVER_HEAD2"].ToString().Trim();
                        txtCastTot_OH.Text = dt.Rows[0]["TOT_CAST_RT_OH"].ToString().Trim();
                        txtCast_Rs.Text = dt.Rows[0]["TOT_CAST_RT"].ToString().Trim();
                        txtTrans.Text = dt.Rows[0]["TRANS_COST"].ToString().Trim();
                        txtTool.Text = dt.Rows[0]["TOOL"].ToString().Trim();
                        txtTotMach.Text = dt.Rows[0]["TOT_MACH_COST"].ToString().Trim();
                        txtPacking.Text = dt.Rows[0]["PACKING"].ToString().Trim();
                        txtHeat.Text = dt.Rows[0]["HEAT"].ToString().Trim();
                        txtFinal.Text = dt.Rows[0]["FINAL"].ToString().Trim();
                        txtInterestPer.Text = dt.Rows[0]["INTEREST_PER"].ToString().Trim();
                        txtVendor_Cost.Text = dt.Rows[0]["VENDOR"].ToString().Trim();
                        //txtFstr.Text = dt.Rows[0]["PORDNO"].ToString().Trim();
                        //txtFstr2.Text = dt.Rows[0]["PBASIS"].ToString().Trim();
                        txtTest.Text = dt.Rows[0]["TEST"].ToString().Trim();
                        txtChaplet.Text = dt.Rows[0]["CHAPLET"].ToString().Trim();
                        txtMould_Heating.Text = dt.Rows[0]["HEATING"].ToString().Trim();
                        txtMouldingOther.Text = dt.Rows[0]["MLD_OTHER"].ToString().Trim();
                        txtSleeve.Text = dt.Rows[0]["SLEEVE"].ToString().Trim();
                        txtSand.Text = dt.Rows[0]["SAND"].ToString().Trim();
                        txtPainting.Text = dt.Rows[0]["PAINTING"].ToString().Trim();
                        txtConver_Other1.Text = dt.Rows[0]["CONV_OTHER1"].ToString().Trim();
                        txtConver_Other2.Text = dt.Rows[0]["CONV_OTHER2"].ToString().Trim();
                        txtInterestPer2.Text = dt.Rows[0]["INTEREST_PER2"].ToString().Trim();
                        hfLine.Value = dt.Rows[0]["LINE"].ToString().Trim();
                        txtCore_Type.Text = dt.Rows[0]["CORE_TYPE"].ToString().Trim();
                        create_tab();
                        sg1_dr = null;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_f1"] = dt.Rows[i]["GRID_ICODE"].ToString().Trim();
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_t1"] = dt.Rows[i]["GRID_FERRO"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["GRID_REC"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["GRID_REQKG"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["GRID_RATE"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["GRID_COST"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["GRID_PIGIRON"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["GRID_CONTRI"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["GRID_REQ"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["GRID_DIFF"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        if (dt.Rows[0]["GRID_ICODE"].ToString().Trim().Length > 4 && dt.Rows.Count < 5)
                        {
                            sg1_add_blankrows();
                        }
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        btnRFQ.Focus();
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
                    //if (frm_ulvl != "0")
                    //{
                    //    fgen.msg("-", "AMSG", "Deleting Rigths Allowed To Owner Only");
                    //    return;
                    //}
                    edmode.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffield.Value = "D";
                    break;

                case "Print":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    //if (frm_ulvl != "0")
                    //{
                    //    fgen.msg("-", "AMSG", "Editing Rigths Allowed To Owner Only");
                    //    return;
                    //}
                    SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt,to_chaR(a.edt_dt,'dd/mm/yyyy') as pedt_Dt,f.aname,i.iname,i.cpartno from " + frm_tabname + " a,famst f,item i where trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "'";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["aname"].ToString().Trim();
                        txtIcode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtIname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtCpart.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[0]["pent_Dt"].ToString().Trim();
                        txtlbl5.Text = dt.Rows[0]["edt_by"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[0]["pedt_Dt"].ToString().Trim();
                        txtRFQ.Text = dt.Rows[0]["invno"].ToString().Trim();
                        txtRFQDt.Text = Convert.ToDateTime(dt.Rows[0]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtMaterial.Text = dt.Rows[0]["MATERIAL"].ToString().Trim();
                        txtLength.Text = dt.Rows[0]["LENGTH"].ToString().Trim();
                        txtWidth.Text = dt.Rows[0]["WIDTH"].ToString().Trim();
                        txtHeight.Text = dt.Rows[0]["HEIGHT"].ToString().Trim();
                        txtCast.Text = dt.Rows[0]["CAST"].ToString().Trim();
                        txtCast_No.Text = dt.Rows[0]["CAST_MOULD"].ToString().Trim();
                        txtBunch.Text = dt.Rows[0]["BUNCH"].ToString().Trim();
                        txtActual.Text = dt.Rows[0]["ACTUAL"].ToString().Trim();
                        txtPattern.Text = dt.Rows[0]["PATTERN"].ToString().Trim();
                        txtRej.Text = dt.Rows[0]["REJ"].ToString().Trim();
                        txtNet.Text = dt.Rows[0]["NET_EFF"].ToString().Trim();
                        txtMixer.Text = dt.Rows[0]["MIXER"].ToString().Trim();
                        txtMould_Rt.Text = dt.Rows[0]["MOULDING_RATE"].ToString().Trim();
                        txtLabour.Text = dt.Rows[0]["LABOUR"].ToString().Trim();
                        txtMaint.Text = dt.Rows[0]["MAINT"].ToString().Trim();
                        txtFettling.Text = dt.Rows[0]["FETTLING"].ToString().Trim();
                        txtInterest.Text = dt.Rows[0]["INTEREST"].ToString().Trim();
                        txtDepr.Text = dt.Rows[0]["DEPR"].ToString().Trim();
                        txtOther.Text = dt.Rows[0]["OTHERS"].ToString().Trim();
                        txtSubTotal.Text = dt.Rows[0]["STOTAL"].ToString().Trim();
                        txtGrandTot.Text = dt.Rows[0]["GTOTAL"].ToString().Trim();
                        txtCast_Rt.Text = dt.Rows[0]["CAST_RT"].ToString().Trim();
                        txtElect.Text = dt.Rows[0]["ELECTRICITY"].ToString().Trim();
                        txtAux.Text = dt.Rows[0]["AUXULARY"].ToString().Trim();
                        txtMetling.Text = dt.Rows[0]["MELTING"].ToString().Trim();
                        txtPower.Text = dt.Rows[0]["POWER"].ToString().Trim();
                        txtCore_Wt.Text = dt.Rows[0]["CORE_WT"].ToString().Trim();
                        txtCore_Rt.Text = dt.Rows[0]["CORE_RT"].ToString().Trim();
                        txtCore_Rej.Text = dt.Rows[0]["CORE_REJ"].ToString().Trim();
                        txtCore_Cost.Text = dt.Rows[0]["CORE_COST"].ToString().Trim();
                        txtFCons.Text = dt.Rows[0]["FCONS"].ToString().Trim();
                        txtFContri.Text = dt.Rows[0]["FCONTRI"].ToString().Trim();
                        txtFRate.Text = dt.Rows[0]["FRATE"].ToString().Trim();
                        txtFWt.Text = dt.Rows[0]["FWT"].ToString().Trim();
                        txtFSi.Text = dt.Rows[0]["FSI"].ToString().Trim();
                        txtFMn.Text = dt.Rows[0]["FMN"].ToString().Trim();
                        txtFC.Text = dt.Rows[0]["FC"].ToString().Trim();
                        txtFMoly.Text = dt.Rows[0]["FMOLY"].ToString().Trim();
                        txtPCons.Text = dt.Rows[0]["PCONS"].ToString().Trim();
                        txtPContri.Text = dt.Rows[0]["PCONTRI"].ToString().Trim();
                        txtPRate.Text = dt.Rows[0]["PRATE"].ToString().Trim();
                        txtPWt.Text = dt.Rows[0]["PWT"].ToString().Trim();
                        txtPSi.Text = dt.Rows[0]["PSI"].ToString().Trim();
                        txtPMn.Text = dt.Rows[0]["PMN"].ToString().Trim();
                        txtPC.Text = dt.Rows[0]["PC"].ToString().Trim();
                        txtPMoly.Text = dt.Rows[0]["PMOLY"].ToString().Trim();
                        txtSCons.Text = dt.Rows[0]["SCONS"].ToString().Trim();
                        txtSContri.Text = dt.Rows[0]["SCONTRI"].ToString().Trim();
                        txtSRate.Text = dt.Rows[0]["SRATE"].ToString().Trim();
                        txtSWt.Text = dt.Rows[0]["SWT"].ToString().Trim();
                        txtSSi.Text = dt.Rows[0]["SSI"].ToString().Trim();
                        txtSMn.Text = dt.Rows[0]["SMN"].ToString().Trim();
                        txtSC.Text = dt.Rows[0]["SC"].ToString().Trim();
                        txtSMoly.Text = dt.Rows[0]["SMOLY"].ToString().Trim();
                        txtCCons.Text = dt.Rows[0]["CCONS"].ToString().Trim();
                        txtCContri.Text = dt.Rows[0]["CCONTRI"].ToString().Trim();
                        txtCRate.Text = dt.Rows[0]["CRATE"].ToString().Trim();
                        txtCWt.Text = dt.Rows[0]["CWT"].ToString().Trim();
                        txtCSi.Text = dt.Rows[0]["CSI"].ToString().Trim();
                        txtCMn.Text = dt.Rows[0]["CMN"].ToString().Trim();
                        txtCC.Text = dt.Rows[0]["CC"].ToString().Trim();
                        txtCMoly.Text = dt.Rows[0]["CMOLY"].ToString().Trim();
                        txtSubCons.Text = dt.Rows[0]["TOTCONS"].ToString().Trim();
                        txtSubContri.Text = dt.Rows[0]["TOTCONTRI"].ToString().Trim();
                        txtSubWt.Text = dt.Rows[0]["TOTWT"].ToString().Trim();
                        txtSubSi.Text = dt.Rows[0]["TOTSI"].ToString().Trim();
                        txtSubMn.Text = dt.Rows[0]["TOTMN"].ToString().Trim();
                        txtSubC.Text = dt.Rows[0]["TOTC"].ToString().Trim();
                        txtSubMoly.Text = dt.Rows[0]["TOTMOLY"].ToString().Trim();
                        txtReqSi.Text = dt.Rows[0]["RSI"].ToString().Trim();
                        txtReqMn.Text = dt.Rows[0]["RMN"].ToString().Trim();
                        txtReqC.Text = dt.Rows[0]["RC"].ToString().Trim();
                        txtReqMoly.Text = dt.Rows[0]["RMOLY"].ToString().Trim();
                        txtDiffSi.Text = dt.Rows[0]["DSI"].ToString().Trim();
                        txtDiffMn.Text = dt.Rows[0]["DMN"].ToString().Trim();
                        txtDiffC.Text = dt.Rows[0]["DC"].ToString().Trim();
                        txtDiffMoly.Text = dt.Rows[0]["DMOLY"].ToString().Trim();
                        txtMetContri.Text = dt.Rows[0]["MET_CONTRI"].ToString().Trim();
                        txtFeSiRec.Text = dt.Rows[0]["FESI_REC"].ToString().Trim();
                        txtFeSiReq.Text = dt.Rows[0]["FESI_REQ"].ToString().Trim();
                        txtFeSiRate.Text = dt.Rows[0]["FESI_RT"].ToString().Trim();
                        txtFeSiCost.Text = dt.Rows[0]["FESI_COST"].ToString().Trim();
                        txtFeMnRec.Text = dt.Rows[0]["FEMN_REC"].ToString().Trim();
                        txtFeMnReq.Text = dt.Rows[0]["FEMN_REQ"].ToString().Trim();
                        txtFeMnRate.Text = dt.Rows[0]["FEMN_RT"].ToString().Trim();
                        txtFeMnCost.Text = dt.Rows[0]["FEMN_COST"].ToString().Trim();
                        txtCSCRec.Text = dt.Rows[0]["CSC_REC"].ToString().Trim();
                        txtCSCReq.Text = dt.Rows[0]["CSC_REQ"].ToString().Trim();
                        txtCSCRate.Text = dt.Rows[0]["CSC_RT"].ToString().Trim();
                        txtCSCCost.Text = dt.Rows[0]["CSC_COST"].ToString().Trim();
                        txtMolyRec.Text = dt.Rows[0]["MOLY_REC"].ToString().Trim();
                        txtMolyReq.Text = dt.Rows[0]["MOLY_REQ"].ToString().Trim();
                        txtMolyRate.Text = dt.Rows[0]["MOLY_RT"].ToString().Trim();
                        txtMolyCost.Text = dt.Rows[0]["MOLY_COST"].ToString().Trim();
                        txtFeSiMGRec.Text = dt.Rows[0]["FESIMG_REC"].ToString().Trim();
                        txtFeSiMGReq.Text = dt.Rows[0]["FESIMG_REQ"].ToString().Trim();
                        txtFeSiMGRate.Text = dt.Rows[0]["FESIMG_RT"].ToString().Trim();
                        txtFeSiMGCost.Text = dt.Rows[0]["FESIMG_COST"].ToString().Trim();
                        txtFerroTot.Text = dt.Rows[0]["FERRO_TOT"].ToString().Trim();
                        txtMetTot.Text = dt.Rows[0]["META_TOT"].ToString().Trim();
                        txtStgWt.Text = dt.Rows[0]["STAGE_WT"].ToString().Trim();
                        txtMelting_Loss.Text = dt.Rows[0]["MELTING_LOSS"].ToString().Trim();
                        txtMelting_Loss_Wt.Text = dt.Rows[0]["MELTING_STAGE_WT"].ToString().Trim();
                        txtMasterAlloy1.Text = dt.Rows[0]["MAS_ALLOY1"].ToString().Trim();
                        txtMasterAlloy2.Text = dt.Rows[0]["MAS_ALLOY2"].ToString().Trim();
                        txtMasterAlloy.Text = dt.Rows[0]["MAS_ALLOY3"].ToString().Trim();
                        txtMasterAlloy_Wt.Text = dt.Rows[0]["MAS_ALLOY_WT"].ToString().Trim();
                        txtInnoculation1.Text = dt.Rows[0]["INNOCULATION1"].ToString().Trim();
                        txtInnoculation2.Text = dt.Rows[0]["INNOCULATION2"].ToString().Trim();
                        txtInnoculation3.Text = dt.Rows[0]["INNOCULATION3"].ToString().Trim();
                        txtInnoculationWt.Text = dt.Rows[0]["INNOCULATION_WT"].ToString().Trim();
                        txtNetYield.Text = dt.Rows[0]["YIELD_RET"].ToString().Trim();
                        txtTotMetRate.Text = dt.Rows[0]["TOT_METALLIC_RT"].ToString().Trim();
                        txtProfit1.Text = dt.Rows[0]["PROFIT1"].ToString().Trim();
                        txtProfit2.Text = dt.Rows[0]["PROFIT2"].ToString().Trim();
                        txtOver1.Text = dt.Rows[0]["OVER_HEAD1"].ToString().Trim();
                        txtOver2.Text = dt.Rows[0]["OVER_HEAD2"].ToString().Trim();
                        txtCastTot_OH.Text = dt.Rows[0]["TOT_CAST_RT_OH"].ToString().Trim();
                        txtCast_Rs.Text = dt.Rows[0]["TOT_CAST_RT"].ToString().Trim();
                        txtTrans.Text = dt.Rows[0]["TRANS_COST"].ToString().Trim();
                        txtTool.Text = dt.Rows[0]["TOOL"].ToString().Trim();
                        txtTotMach.Text = dt.Rows[0]["TOT_MACH_COST"].ToString().Trim();
                        txtPacking.Text = dt.Rows[0]["PACKING"].ToString().Trim();
                        txtHeat.Text = dt.Rows[0]["HEAT"].ToString().Trim();
                        txtFinal.Text = dt.Rows[0]["FINAL"].ToString().Trim();
                        txtInterestPer.Text = dt.Rows[0]["INTEREST_PER"].ToString().Trim();
                        txtVendor_Cost.Text = dt.Rows[0]["VENDOR"].ToString().Trim();
                        txtFstr.Text = dt.Rows[0]["PORDNO"].ToString().Trim();
                        txtFstr2.Text = dt.Rows[0]["PBASIS"].ToString().Trim();
                        txtTest.Text = dt.Rows[0]["TEST"].ToString().Trim();
                        txtChaplet.Text = dt.Rows[0]["CHAPLET"].ToString().Trim();
                        txtMould_Heating.Text = dt.Rows[0]["HEATING"].ToString().Trim();
                        txtMouldingOther.Text = dt.Rows[0]["MLD_OTHER"].ToString().Trim();
                        txtSleeve.Text = dt.Rows[0]["SLEEVE"].ToString().Trim();
                        txtSand.Text = dt.Rows[0]["SAND"].ToString().Trim();
                        txtPainting.Text = dt.Rows[0]["PAINTING"].ToString().Trim();
                        txtConver_Other1.Text = dt.Rows[0]["CONV_OTHER1"].ToString().Trim();
                        txtConver_Other2.Text = dt.Rows[0]["CONV_OTHER2"].ToString().Trim();
                        txtInterestPer2.Text = dt.Rows[0]["INTEREST_PER2"].ToString().Trim();
                        hfLine.Value = dt.Rows[0]["LINE"].ToString().Trim();
                        txtCore_Type.Text = dt.Rows[0]["CORE_TYPE"].ToString().Trim();
                        txtParentChild.Text = dt.Rows[0]["PARENTCHILD"].ToString().Trim();
                        txtChildCode.Text = dt.Rows[0]["CHILDCODE"].ToString().Trim();
                        txtChildName.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(iname) as iname from item where icode='" + txtChildCode.Text.Trim() + "'", "iname");

                        create_tab();
                        sg1_dr = null;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_f1"] = dt.Rows[i]["GRID_ICODE"].ToString().Trim();
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_t1"] = dt.Rows[i]["GRID_FERRO"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["GRID_REC"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["GRID_REQKG"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["GRID_RATE"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["GRID_COST"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["GRID_PIGIRON"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["GRID_CONTRI"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["GRID_REQ"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["GRID_DIFF"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        if (dt.Rows[0]["GRID_ICODE"].ToString().Trim().Length > 4 && dt.Rows.Count < 5)
                        {
                            sg1_add_blankrows();
                        }
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
                        btnRFQ.Enabled = false;
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;

                case "Print_E":
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", Prg_Id);
                    fgen.fin_smktg_reps(frm_qstr);
                    break;

                case "RFQ":
                    if (col1.Length <= 0) return;
                    SQuery = "select distinct a.branchcd||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') as fstr,trim(a.ordno) as rfq_no,to_char(a.orddt,'dd/mm/yyyy') as rfq_date,a.icode,i.iname,i.cpartno,i.cdrgno,a.acode,f.aname,a.pordno,a.pbasis,a.amd_no,a.delv_item from wb_sorfq a,item i,famst f where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(f.acode) and a.branchcd||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(delv_item)='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtRFQ.Text = dt.Rows[0]["rfq_no"].ToString().Trim();
                        txtRFQDt.Text = dt.Rows[0]["rfq_date"].ToString().Trim();
                        txtIcode.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtIname.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtCpart.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                        txtlbl4.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["aname"].ToString().Trim();
                        txtFstr.Text = dt.Rows[0]["fstr"].ToString().Trim();
                        //txtFstr2.Text = dt.Rows[0]["pbasis"].ToString().Trim(); // BRANCHCD||TYPE||ORDNO||ORDDT OF ER OR EC
                        txtFstr2.Text = dt.Rows[0]["pordno"].ToString().Trim();
                        txtChildCode.Text = dt.Rows[0]["amd_no"].ToString().Trim();
                        txtChildName.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(iname) as iname from item where icode='" + dt.Rows[0]["amd_no"].ToString().Trim() + "'", "iname");
                        txtParentChild.Text = dt.Rows[0]["delv_item"].ToString().Trim();
                        mq8 = dt.Rows[0]["pordno"].ToString().Trim();
                        //mq9 = "select WK1 as cast_wt,WK2 as bunch_wt,WK3 as cavity,IOPR as rej_perc from wb_sorfq where branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy')='" + mq8 + "'";
                        mq9 = "select WK1 as cast_wt,WK2 as bunch_wt,WK3 as cavity,IOPR as rej_perc from wb_sorfq where branchcd||trim(type)||trim(ordno)||to_char(orddt,'dd/mm/yyyy')||trim(delv_item)='" + col1.Trim() + "'";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq9);
                        if (dt2.Rows.Count > 0)
                        {
                            txtCast.Text = dt2.Rows[0]["cast_wt"].ToString().Trim();
                            txtBunch.Text = dt2.Rows[0]["bunch_wt"].ToString().Trim();
                            txtCast_No.Text = dt2.Rows[0]["cavity"].ToString().Trim();
                            txtRej.Text = dt2.Rows[0]["rej_perc"].ToString().Trim();
                        }
                        Cal();
                    }
                    ImageButton4.Focus();
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    break;

                case "ICODE":
                    if (col1.Length <= 0) return;
                    txtIcode.Text = col1;
                    txtIname.Text = col2;
                    ImageButton4.Focus();
                    break;

                case "BOX":
                    if (col1.Length <= 0) return;
                    SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,num1,num2,num3 from wb_master where branchcd||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtLength.Text = dt.Rows[0]["num1"].ToString().Trim();
                        txtWidth.Text = dt.Rows[0]["num2"].ToString().Trim();
                        txtHeight.Text = dt.Rows[0]["num3"].ToString().Trim();
                        Cal();
                    }
                    txtMaterial.Focus();
                    break;

                case "CONVERSION":
                    if (col1.Length <= 0) return;
                    SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,num4,num5,num6,num7,num8,num9 from wb_master where branchcd||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtLabour.Text = dt.Rows[0]["num4"].ToString().Trim();
                        txtMaint.Text = dt.Rows[0]["num5"].ToString().Trim();
                        txtFettling.Text = dt.Rows[0]["num6"].ToString().Trim();
                        txtInterest.Text = dt.Rows[0]["num7"].ToString().Trim();
                        txtDepr.Text = dt.Rows[0]["num8"].ToString().Trim();
                        txtOther.Text = dt.Rows[0]["num9"].ToString().Trim();
                        Cal();
                    }
                    txtPainting.Focus();
                    break;

                case "POWER":
                    if (col1.Length <= 0) return;
                    SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,num1,num2,num3 from wb_master where branchcd||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtElect.Text = dt.Rows[0]["num1"].ToString().Trim();
                        txtAux.Text = dt.Rows[0]["num2"].ToString().Trim();
                        txtMetling.Text = dt.Rows[0]["num3"].ToString().Trim();
                        Cal();
                    }
                    txtCore_Wt.Focus();
                    break;

                case "METALLIC":
                    if (col1.Length <= 0) return;
                    SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,num1,num2,num3,num4,num5,num6 from wb_master where branchcd||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtFeSiRec.Text = dt.Rows[0]["num1"].ToString().Trim();
                        txtFeMnRec.Text = dt.Rows[0]["num2"].ToString().Trim();
                        txtCSCRec.Text = dt.Rows[0]["num3"].ToString().Trim();
                        txtMolyRec.Text = dt.Rows[0]["num4"].ToString().Trim();
                        if (hfLine.Value == "Line1")
                        {
                            txtFeSiMGRec.Text = dt.Rows[0]["num5"].ToString().Trim();
                        }
                        else
                        {
                            txtFeSiMGRec.Text = dt.Rows[0]["num6"].ToString().Trim();
                        }
                        mq0 = "select vchnum,vchdate,irate,trim(icode) as icode,to_char(vchdate,'yyyymmdd') as vdd from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and icode in ('10020002','10020001','10020042','10020023','10040008') AND vchdate>(SYSDATE-600) order by vdd desc";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        txtFeSiRate.Text = fgen.seek_iname_dt(dt2, "icode='10020002'", "irate");
                        txtFeMnRate.Text = fgen.seek_iname_dt(dt2, "icode='10020001'", "irate");
                        //txtCSCRate.Text = fgen.seek_iname_dt(dt2, "icode='10020017'", "irate");
                        txtMolyRate.Text = fgen.seek_iname_dt(dt2, "icode='10020023'", "irate");
                        txtFeSiMGRate.Text = fgen.seek_iname_dt(dt2, "icode='10040008'", "irate");

                        // WHEN EITHER THERE IS NO MRR IN PAST 600 DAYS OR RATE IS ZERO IN MRR
                        mq0 = "select irate,trim(icode) as icode from item where icode in ('10020002','10020001','10020042','10020023','10040008') order by icode";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        if (txtFeSiRate.Text == "0")
                        {
                            txtFeSiRate.Text = fgen.seek_iname_dt(dt2, "icode='10020002'", "irate");
                        }
                        if (txtFeMnRate.Text == "0")
                        {
                            txtFeMnRate.Text = fgen.seek_iname_dt(dt2, "icode='10020001'", "irate");
                        }
                        //if (txtCSCRate.Text == "0")
                        //{
                        //    txtCSCRate.Text = fgen.seek_iname_dt(dt2, "icode='10020017'", "irate");
                        //}
                        if (txtMolyRate.Text == "0")
                        {
                            txtMolyRate.Text = fgen.seek_iname_dt(dt2, "icode='10020023'", "irate");
                        }
                        if (txtFeSiMGRate.Text == "0")
                        {
                            txtFeSiMGRate.Text = fgen.seek_iname_dt(dt2, "icode='10040008'", "irate");
                        }
                        Cal();
                    }
                    Img_Carburiser.Focus();
                    break;

                case "CORE":
                    if (col1.Length <= 0) return;
                    SQuery = "select vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_dt,name from wb_master where branchcd||trim(id)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtCore_Type.Text = dt.Rows[0]["name"].ToString().Trim();
                        Cal();
                    }
                    txtCore_Rt.Focus();
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    SQuery = "Select icode,iname,irate from item where icode ='" + col1.Trim() + "' order by icode";
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    mq0 = "select vchnum,vchdate,irate,trim(icode) as icode,to_char(vchdate,'yyyymmdd') as vdd from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and icode in (" + col1.Trim() + ") AND vchdate>(SYSDATE-600) order by vdd desc";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in Hidden Field
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[3].Text = dt.Rows[d]["icode"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = dt.Rows[d]["iname"].ToString().Trim();
                        rate = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[d]["icode"].ToString().Trim() + "'", "irate"));
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[d]["icode"].ToString().Trim() + "'", "irate");
                        if (rate == 0)
                        {
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = dt.Rows[d]["irate"].ToString().Trim();
                        }
                    }
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
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
                            sg1_dr["sg1_srno"] = Convert.ToInt32(sg1.Rows[i].Cells[2].Text);
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[3].Text.Trim();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[4].Text.Trim();
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
                        SQuery = "Select icode,iname,irate from item where icode in (" + col1.Trim() + ") order by icode";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        mq0 = "select vchnum,vchdate,irate,trim(icode) as icode,to_char(vchdate,'yyyymmdd') as vdd from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and icode in (" + col1.Trim() + ") AND vchdate>(SYSDATE-600) order by vdd desc";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                        gridcount = sg1_dt.Rows.Count + dt.Rows.Count;
                        if (gridcount > 5)
                        {
                            fgen.msg("-", "AMSG", "Please Select 5 Items Max");
                            return;
                        }
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            rate = 0;
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_t1"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_t2"] = "";
                            sg1_dr["sg1_t3"] = "";
                            rate = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[d]["icode"].ToString().Trim() + "'", "irate"));
                            sg1_dr["sg1_t4"] = fgen.seek_iname_dt(dt2, "icode='" + dt.Rows[d]["icode"].ToString().Trim() + "'", "irate");
                            if (rate == 0)
                            {
                                sg1_dr["sg1_t4"] = dt.Rows[d]["irate"].ToString().Trim();
                            }
                            sg1_dr["sg1_t5"] = "";
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    if (gridcount < 5)
                    {
                        sg1_add_blankrows();
                    }
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    setColHeadings();
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
                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[3].Text.Trim();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[4].Text.Trim();
                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        if (gridcount < 5)
                        {
                            sg1_add_blankrows();
                        }
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        for (i = 0; i < sg1.Rows.Count; i++)
                        {
                            sg1.Rows[i].Cells[2].Text = (i + 1).ToString();
                        }
                    }
                    #endregion
                    setColHeadings();
                    break;

                case "STEEL":
                    SQuery = "select vchnum,vchdate,irate,trim(icode) as icode,to_char(vchdate,'yyyymmdd') as vdd from ivoucher where branchcd='" + frm_mbr + "'  and type like '0%' and icode ='" + col1 + "' AND vchdate>(SYSDATE-600) order by vdd desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtSRate.Text = dt.Rows[0]["irate"].ToString().Trim();
                    }
                    // WHEN EITHER THERE IS NO MRR IN PAST 600 DAYS OR RATE IS ZERO IN MRR
                    mq0 = "select irate,trim(icode) as icode from item where icode ='" + col1 + "' order by icode";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    if (txtSRate.Text == "0" || txtSRate.Text == "")
                    {
                        txtSRate.Text = dt2.Rows[0]["irate"].ToString().Trim();
                    }
                    Cal();
                    break;

                case "CARBURISER":
                    SQuery = "select vchnum,vchdate,irate,trim(icode) as icode,to_char(vchdate,'yyyymmdd') as vdd from ivoucher where branchcd='" + frm_mbr + "'  and type like '0%' and icode ='" + col1 + "' AND vchdate>(SYSDATE-600) order by vdd desc";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtCSCRate.Text = dt.Rows[0]["irate"].ToString().Trim();
                    }
                    // WHEN EITHER THERE IS NO MRR IN PAST 600 DAYS OR RATE IS ZERO IN MRR
                    mq0 = "select irate,trim(icode) as icode from item where icode ='" + col1 + "' order by icode";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    if (txtCSCRate.Text == "0" || txtCSCRate.Text == "")
                    {
                        txtCSCRate.Text = dt2.Rows[0]["irate"].ToString().Trim();
                    }
                    Cal();
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY"); ;
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            string last_entdt;
            //checks
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'", "ldt");
            if (last_entdt == "0" || edmode.Value == "Y")
            {
            }
            else
            {
                if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
                {
                    Checked_ok = "N";
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + ",Please Check !!");
                }
            }
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
            if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("dd/MM/yyyy") + " ,Please Check !!");
            }
            // -----------------------------
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
                        if (sg1.Rows.Count <= 1)
                        {
                            save_fun();
                        }
                        else
                        {
                            save_fun2();
                        }

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
                            save_it = "Y";
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

                        if (sg1.Rows.Count <= 1)
                        {
                            save_fun();
                        }
                        else
                        {
                            save_fun2();
                        }

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);
                        if (edmode.Value == "Y")
                        {
                            mq3 = "update " + frm_tabname + " set test='" + txtTest.Text.Trim() + "' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, mq3);
                        }
                        string mycmd4 = ""; // SAVING FLAG IN ER ENTRY
                        mycmd4 = "update WB_SORFQ set TEST='C' where branchcd||type||trim(ordno)||to_char(orddt,'dd/MM/yyyy')='" + txtFstr2.Text.Trim() + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, mycmd4);

                        string mycmd3 = ""; // SAVING FLAG IN RF ENTRY
                        mycmd3 = "update WB_SORFQ set TEST='C' where branchcd||type||trim(ordno)||to_char(orddt,'dd/MM/yyyy')='" + txtFstr.Text.Trim() + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, mycmd3);

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
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);
                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
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
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton5_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "RFQ";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select RFQ Details ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton42_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Item ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        i = 0;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();
        oporow["BRANCHCD"] = frm_mbr;
        oporow["TYPE"] = frm_vty;
        oporow["vchnum"] = frm_vnum;
        oporow["vchdate"] = txtvchdate.Text.Trim();
        oporow["acode"] = txtlbl4.Text.Trim().ToUpper();
        oporow["icode"] = txtIcode.Text.Trim().ToUpper();
        oporow["invno"] = txtRFQ.Text.Trim().Trim().ToUpper();
        oporow["invdate"] = txtRFQDt.Text.Trim().Trim().ToUpper();
        oporow["MATERIAL"] = txtMaterial.Text.Trim().Trim().ToUpper();
        oporow["LENGTH"] = fgen.make_double(txtLength.Text.Trim().Trim().ToUpper());
        oporow["WIDTH"] = fgen.make_double(txtWidth.Text.Trim().Trim().ToUpper());
        oporow["HEIGHT"] = fgen.make_double(txtHeight.Text.Trim().Trim().ToUpper());
        oporow["CAST"] = fgen.make_double(txtCast.Text.Trim().Trim().ToUpper());
        oporow["CAST_MOULD"] = fgen.make_double(txtCast_No.Text.Trim().Trim().ToUpper());
        oporow["BUNCH"] = fgen.make_double(txtBunch.Text.Trim().Trim().ToUpper());
        oporow["ACTUAL"] = fgen.make_double(txtActual.Text.Trim().Trim().ToUpper());
        oporow["PATTERN"] = fgen.make_double(txtPattern.Text.Trim().Trim().ToUpper());
        oporow["REJ"] = fgen.make_double(txtRej.Text.Trim().Trim().ToUpper());
        oporow["NET_EFF"] = fgen.make_double(txtNet.Text.Trim().Trim().ToUpper());
        oporow["MIXER"] = fgen.make_double(txtMixer.Text.Trim().Trim().ToUpper());
        oporow["MOULDING_RATE"] = fgen.make_double(txtMould_Rt.Text.Trim().ToUpper());
        oporow["LABOUR"] = fgen.make_double(txtLabour.Text.Trim().Trim().ToUpper());
        oporow["MAINT"] = fgen.make_double(txtMaint.Text.Trim().Trim().ToUpper());
        oporow["FETTLING"] = fgen.make_double(txtFettling.Text.Trim().Trim().ToUpper());
        oporow["INTEREST"] = fgen.make_double(txtInterest.Text.Trim().Trim().ToUpper());
        oporow["DEPR"] = fgen.make_double(txtDepr.Text.Trim().Trim().ToUpper());
        oporow["OTHERS"] = fgen.make_double(txtOther.Text.Trim().Trim().ToUpper());
        oporow["STOTAL"] = fgen.make_double(txtSubTotal.Text.Trim().Trim().ToUpper());
        oporow["GTOTAL"] = fgen.make_double(txtGrandTot.Text.Trim().Trim().ToUpper());
        oporow["CAST_RT"] = fgen.make_double(txtCast_Rt.Text.Trim().Trim().ToUpper());
        oporow["ELECTRICITY"] = fgen.make_double(txtElect.Text.Trim().Trim().ToUpper());
        oporow["AUXULARY"] = fgen.make_double(txtAux.Text.Trim().Trim().ToUpper());
        oporow["MELTING"] = fgen.make_double(txtMetling.Text.Trim().Trim().ToUpper());
        oporow["POWER"] = fgen.make_double(txtPower.Text.Trim().Trim().ToUpper());
        oporow["CORE_WT"] = fgen.make_double(txtCore_Wt.Text.Trim().Trim().ToUpper());
        oporow["CORE_RT"] = fgen.make_double(txtCore_Rt.Text.Trim().Trim().ToUpper());
        oporow["CORE_REJ"] = fgen.make_double(txtCore_Rej.Text.Trim().Trim().ToUpper());
        oporow["CORE_COST"] = fgen.make_double(txtCore_Cost.Text.Trim().Trim().ToUpper());
        oporow["FCONS"] = fgen.make_double(txtFCons.Text.Trim().Trim().ToUpper());
        oporow["FCONTRI"] = fgen.make_double(txtFContri.Text.Trim().Trim().ToUpper());
        oporow["FRATE"] = fgen.make_double(txtFRate.Text.Trim().Trim().ToUpper());
        oporow["FWT"] = fgen.make_double(txtFWt.Text.Trim().Trim().ToUpper());
        oporow["FSI"] = fgen.make_double(txtFSi.Text.Trim().Trim().ToUpper());
        oporow["FMN"] = fgen.make_double(txtFMn.Text.Trim().Trim().ToUpper());
        oporow["FC"] = fgen.make_double(txtFC.Text.Trim().Trim().ToUpper());
        oporow["FMOLY"] = fgen.make_double(txtFMoly.Text.Trim().Trim().ToUpper());
        oporow["PCONS"] = fgen.make_double(txtPCons.Text.Trim().Trim().ToUpper());
        oporow["PCONTRI"] = fgen.make_double(txtPContri.Text.Trim().Trim().ToUpper());
        oporow["PRATE"] = fgen.make_double(txtPRate.Text.Trim().Trim().ToUpper());
        oporow["PWT"] = fgen.make_double(txtPWt.Text.Trim().Trim().ToUpper());
        oporow["PSI"] = fgen.make_double(txtPSi.Text.Trim().Trim().ToUpper());
        oporow["PMN"] = fgen.make_double(txtPMn.Text.Trim().Trim().ToUpper());
        oporow["PC"] = fgen.make_double(txtPC.Text.Trim().Trim().ToUpper());
        oporow["PMOLY"] = fgen.make_double(txtPMoly.Text.Trim().Trim().ToUpper());
        oporow["SCONS"] = fgen.make_double(txtSCons.Text.Trim().Trim().ToUpper());
        oporow["SCONTRI"] = fgen.make_double(txtSContri.Text.Trim().Trim().ToUpper());
        oporow["SRATE"] = fgen.make_double(txtSRate.Text.Trim().Trim().ToUpper());
        oporow["SWT"] = fgen.make_double(txtSWt.Text.Trim().Trim().ToUpper());
        oporow["SSI"] = fgen.make_double(txtSSi.Text.Trim().Trim().ToUpper());
        oporow["SMN"] = fgen.make_double(txtSMn.Text.Trim().Trim().ToUpper());
        oporow["SC"] = fgen.make_double(txtSC.Text.Trim().Trim().ToUpper());
        oporow["SMOLY"] = fgen.make_double(txtSMoly.Text.Trim().Trim().ToUpper());
        oporow["CCONS"] = fgen.make_double(txtCCons.Text.Trim().Trim().ToUpper());
        oporow["CCONTRI"] = fgen.make_double(txtCContri.Text.Trim().Trim().ToUpper());
        oporow["CRATE"] = fgen.make_double(txtCRate.Text.Trim().Trim().ToUpper());
        oporow["CWT"] = fgen.make_double(txtCWt.Text.Trim().Trim().ToUpper());
        oporow["CSI"] = fgen.make_double(txtCSi.Text.Trim().Trim().ToUpper());
        oporow["CMN"] = fgen.make_double(txtCMn.Text.Trim().Trim().ToUpper());
        oporow["CC"] = fgen.make_double(txtCC.Text.Trim().Trim().ToUpper());
        oporow["CMOLY"] = fgen.make_double(txtCMoly.Text.Trim().Trim().ToUpper());
        oporow["TOTCONS"] = fgen.make_double(txtSubCons.Text.Trim().Trim().ToUpper());
        oporow["TOTCONTRI"] = fgen.make_double(txtSubContri.Text.Trim().Trim().ToUpper());
        oporow["TOTWT"] = fgen.make_double(txtSubWt.Text.Trim().Trim().ToUpper());
        oporow["TOTSI"] = fgen.make_double(txtSubSi.Text.Trim().Trim().ToUpper());
        oporow["TOTMN"] = fgen.make_double(txtSubMn.Text.Trim().Trim().ToUpper());
        oporow["TOTC"] = fgen.make_double(txtSubC.Text.Trim().Trim().ToUpper());
        oporow["TOTMOLY"] = fgen.make_double(txtSubMoly.Text.Trim().Trim().ToUpper());
        oporow["RSI"] = fgen.make_double(txtReqSi.Text.Trim().Trim().ToUpper());
        oporow["RMN"] = fgen.make_double(txtReqMn.Text.Trim().Trim().ToUpper());
        oporow["RC"] = fgen.make_double(txtReqC.Text.Trim().Trim().ToUpper());
        oporow["RMOLY"] = fgen.make_double(txtReqMoly.Text.Trim().Trim().ToUpper());
        oporow["DSI"] = fgen.make_double(txtDiffSi.Text.Trim().Trim().ToUpper());
        oporow["DMN"] = fgen.make_double(txtDiffMn.Text.Trim().Trim().ToUpper());
        oporow["DC"] = fgen.make_double(txtDiffC.Text.Trim().Trim().ToUpper());
        oporow["DMOLY"] = fgen.make_double(txtDiffMoly.Text.Trim().Trim().ToUpper());
        oporow["MET_CONTRI"] = fgen.make_double(txtMetContri.Text.Trim().Trim().ToUpper());
        oporow["FESI_REC"] = fgen.make_double(txtFeSiRec.Text.Trim().Trim().ToUpper());
        oporow["FESI_REQ"] = fgen.make_double(txtFeSiReq.Text.Trim().Trim().ToUpper());
        oporow["FESI_RT"] = fgen.make_double(txtFeSiRate.Text.Trim().Trim().ToUpper());
        oporow["FESI_COST"] = fgen.make_double(txtFeSiCost.Text.Trim().Trim().ToUpper());
        oporow["FEMN_REC"] = fgen.make_double(txtFeMnRec.Text.Trim().Trim().ToUpper());
        oporow["FEMN_REQ"] = fgen.make_double(txtFeMnReq.Text.Trim().Trim().ToUpper());
        oporow["FEMN_RT"] = fgen.make_double(txtFeMnRate.Text.Trim().Trim().ToUpper());
        oporow["FEMN_COST"] = fgen.make_double(txtFeMnCost.Text.Trim().Trim().ToUpper());
        oporow["CSC_REC"] = fgen.make_double(txtCSCRec.Text.Trim().Trim().ToUpper());
        oporow["CSC_REQ"] = fgen.make_double(txtCSCReq.Text.Trim().Trim().ToUpper());
        oporow["CSC_RT"] = fgen.make_double(txtCSCRate.Text.Trim().Trim().ToUpper());
        oporow["CSC_COST"] = fgen.make_double(txtCSCCost.Text.Trim().Trim().ToUpper());
        oporow["MOLY_REC"] = fgen.make_double(txtMolyRec.Text.Trim().Trim().ToUpper());
        oporow["MOLY_REQ"] = fgen.make_double(txtMolyReq.Text.Trim().Trim().ToUpper());
        oporow["MOLY_RT"] = fgen.make_double(txtMolyRate.Text.Trim().Trim().ToUpper());
        oporow["MOLY_COST"] = fgen.make_double(txtMolyCost.Text.Trim().Trim().ToUpper());
        oporow["FESIMG_REC"] = fgen.make_double(txtFeSiMGRec.Text.Trim().Trim().ToUpper());
        oporow["FESIMG_REQ"] = fgen.make_double(txtFeSiMGReq.Text.Trim().Trim().ToUpper());
        oporow["FESIMG_RT"] = fgen.make_double(txtFeSiMGRate.Text.Trim().Trim().ToUpper());
        oporow["FESIMG_COST"] = fgen.make_double(txtFeSiMGCost.Text.Trim().Trim().ToUpper());
        oporow["FERRO_TOT"] = fgen.make_double(txtFerroTot.Text.Trim().Trim().ToUpper());
        oporow["META_TOT"] = fgen.make_double(txtMetTot.Text.Trim().Trim().ToUpper());
        oporow["STAGE_WT"] = fgen.make_double(txtStgWt.Text.Trim().Trim().ToUpper());
        oporow["MELTING_LOSS"] = fgen.make_double(txtMelting_Loss.Text.Trim().Trim().ToUpper());
        oporow["MELTING_STAGE_WT"] = fgen.make_double(txtMelting_Loss_Wt.Text.Trim().Trim().ToUpper());
        oporow["MAS_ALLOY1"] = fgen.make_double(txtMasterAlloy1.Text.Trim().Trim().ToUpper());
        oporow["MAS_ALLOY2"] = fgen.make_double(txtMasterAlloy2.Text.Trim().Trim().ToUpper());
        oporow["MAS_ALLOY3"] = fgen.make_double(txtMasterAlloy.Text.Trim().Trim().ToUpper());
        oporow["MAS_ALLOY_WT"] = fgen.make_double(txtMasterAlloy_Wt.Text.Trim().Trim().ToUpper());
        oporow["INNOCULATION1"] = fgen.make_double(txtInnoculation1.Text.Trim().Trim().ToUpper());
        oporow["INNOCULATION2"] = fgen.make_double(txtInnoculation2.Text.Trim().Trim().ToUpper());
        oporow["INNOCULATION3"] = fgen.make_double(txtInnoculation3.Text.Trim().Trim().ToUpper());
        oporow["INNOCULATION_WT"] = fgen.make_double(txtInnoculationWt.Text.Trim().Trim().ToUpper());
        oporow["YIELD_RET"] = fgen.make_double(txtNetYield.Text.Trim().Trim().ToUpper());
        oporow["TOT_METALLIC_RT"] = fgen.make_double(txtTotMetRate.Text.Trim().Trim().ToUpper());
        oporow["PROFIT1"] = fgen.make_double(txtProfit1.Text.Trim().Trim().ToUpper());
        oporow["PROFIT2"] = fgen.make_double(txtProfit2.Text.Trim().Trim().ToUpper());
        oporow["OVER_HEAD1"] = fgen.make_double(txtOver1.Text.Trim().Trim().ToUpper());
        oporow["OVER_HEAD2"] = fgen.make_double(txtOver2.Text.Trim().Trim().ToUpper());
        oporow["TOT_CAST_RT_OH"] = fgen.make_double(txtCastTot_OH.Text.Trim().Trim().ToUpper());
        oporow["TOT_CAST_RT"] = fgen.make_double(txtCast_Rs.Text.Trim().Trim().ToUpper());
        oporow["TRANS_COST"] = fgen.make_double(txtTrans.Text.Trim().Trim().ToUpper());
        oporow["TOOL"] = fgen.make_double(txtTool.Text.Trim().Trim().ToUpper());
        oporow["TOT_MACH_COST"] = fgen.make_double(txtTotMach.Text.Trim().Trim().ToUpper());
        oporow["PACKING"] = fgen.make_double(txtPacking.Text.Trim().Trim().ToUpper());
        oporow["HEAT"] = fgen.make_double(txtHeat.Text.Trim().Trim().ToUpper());
        oporow["FINAL"] = fgen.make_double(txtFinal.Text.Trim().Trim().ToUpper());
        oporow["INTEREST_PER"] = fgen.make_double(txtInterestPer.Text.Trim().Trim().ToUpper());
        oporow["VENDOR"] = fgen.make_double(txtVendor_Cost.Text.Trim().Trim().ToUpper());

        oporow["PORDNO"] = txtFstr.Text.Trim().ToUpper();
        oporow["PBASIS"] = txtFstr2.Text.Trim().ToUpper();
        oporow["app_by"] = "-";
        oporow["app_dt"] = vardate;

        oporow["CHAPLET"] = fgen.make_double(txtChaplet.Text.Trim().Trim().ToUpper());
        oporow["HEATING"] = fgen.make_double(txtMould_Heating.Text.Trim().Trim().ToUpper());
        oporow["MLD_OTHER"] = fgen.make_double(txtMouldingOther.Text.Trim().Trim().ToUpper());
        oporow["SLEEVE"] = fgen.make_double(txtSleeve.Text.Trim().Trim().ToUpper());
        oporow["SAND"] = fgen.make_double(txtSand.Text.Trim().Trim().ToUpper());
        oporow["PAINTING"] = fgen.make_double(txtPainting.Text.Trim().Trim().ToUpper());
        oporow["CONV_OTHER1"] = fgen.make_double(txtConver_Other1.Text.Trim().Trim().ToUpper());
        oporow["CONV_OTHER2"] = fgen.make_double(txtConver_Other2.Text.Trim().Trim().ToUpper());

        oporow["GRID_ICODE"] = sg1.Rows[i].Cells[3].Text.Trim().ToUpper();
        if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper().Length > 20)
        {
            oporow["GRID_FERRO"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper().Substring(0, 20);
        }
        else
        {
            oporow["GRID_FERRO"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
        }
        oporow["GRID_REC"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper());
        oporow["GRID_REQKG"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper());
        oporow["GRID_RATE"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper());
        oporow["GRID_COST"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper());
        oporow["GRID_PIGIRON"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper());
        oporow["GRID_CONTRI"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper());
        oporow["GRID_REQ"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper());
        oporow["GRID_DIFF"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper());
        oporow["LINE"] = hfLine.Value.Trim().ToUpper();
        oporow["CORE_TYPE"] = txtCore_Type.Text.Trim().ToUpper();
        oporow["CHILDCODE"] = txtChildCode.Text.Trim().ToUpper();
        oporow["PARENTCHILD"] = txtParentChild.Text.Trim().ToUpper();
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
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            if (sg1.Rows[i].Cells[3].Text.Trim().Length > 1)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();
                oporow["acode"] = txtlbl4.Text.Trim().ToUpper();
                oporow["icode"] = txtIcode.Text.Trim().ToUpper();
                oporow["invno"] = txtRFQ.Text.Trim().Trim().ToUpper();
                oporow["invdate"] = txtRFQDt.Text.Trim().Trim().ToUpper();
                oporow["MATERIAL"] = txtMaterial.Text.Trim().Trim().ToUpper();
                oporow["LENGTH"] = fgen.make_double(txtLength.Text.Trim().Trim().ToUpper());
                oporow["WIDTH"] = fgen.make_double(txtWidth.Text.Trim().Trim().ToUpper());
                oporow["HEIGHT"] = fgen.make_double(txtHeight.Text.Trim().Trim().ToUpper());
                oporow["CAST"] = fgen.make_double(txtCast.Text.Trim().Trim().ToUpper());
                oporow["CAST_MOULD"] = fgen.make_double(txtCast_No.Text.Trim().Trim().ToUpper());
                oporow["BUNCH"] = fgen.make_double(txtBunch.Text.Trim().Trim().ToUpper());
                oporow["ACTUAL"] = fgen.make_double(txtActual.Text.Trim().Trim().ToUpper());
                oporow["PATTERN"] = fgen.make_double(txtPattern.Text.Trim().Trim().ToUpper());
                oporow["REJ"] = fgen.make_double(txtRej.Text.Trim().Trim().ToUpper());
                oporow["NET_EFF"] = fgen.make_double(txtNet.Text.Trim().Trim().ToUpper());
                oporow["MIXER"] = fgen.make_double(txtMixer.Text.Trim().Trim().ToUpper());
                oporow["MOULDING_RATE"] = fgen.make_double(txtMould_Rt.Text.Trim().ToUpper());
                oporow["LABOUR"] = fgen.make_double(txtLabour.Text.Trim().Trim().ToUpper());
                oporow["MAINT"] = fgen.make_double(txtMaint.Text.Trim().Trim().ToUpper());
                oporow["FETTLING"] = fgen.make_double(txtFettling.Text.Trim().Trim().ToUpper());
                oporow["INTEREST"] = fgen.make_double(txtInterest.Text.Trim().Trim().ToUpper());
                oporow["DEPR"] = fgen.make_double(txtDepr.Text.Trim().Trim().ToUpper());
                oporow["OTHERS"] = fgen.make_double(txtOther.Text.Trim().Trim().ToUpper());
                oporow["STOTAL"] = fgen.make_double(txtSubTotal.Text.Trim().Trim().ToUpper());
                oporow["GTOTAL"] = fgen.make_double(txtGrandTot.Text.Trim().Trim().ToUpper());
                oporow["CAST_RT"] = fgen.make_double(txtCast_Rt.Text.Trim().Trim().ToUpper());
                oporow["ELECTRICITY"] = fgen.make_double(txtElect.Text.Trim().Trim().ToUpper());
                oporow["AUXULARY"] = fgen.make_double(txtAux.Text.Trim().Trim().ToUpper());
                oporow["MELTING"] = fgen.make_double(txtMetling.Text.Trim().Trim().ToUpper());
                oporow["POWER"] = fgen.make_double(txtPower.Text.Trim().Trim().ToUpper());
                oporow["CORE_WT"] = fgen.make_double(txtCore_Wt.Text.Trim().Trim().ToUpper());
                oporow["CORE_RT"] = fgen.make_double(txtCore_Rt.Text.Trim().Trim().ToUpper());
                oporow["CORE_REJ"] = fgen.make_double(txtCore_Rej.Text.Trim().Trim().ToUpper());
                oporow["CORE_COST"] = fgen.make_double(txtCore_Cost.Text.Trim().Trim().ToUpper());
                oporow["FCONS"] = fgen.make_double(txtFCons.Text.Trim().Trim().ToUpper());
                oporow["FCONTRI"] = fgen.make_double(txtFContri.Text.Trim().Trim().ToUpper());
                oporow["FRATE"] = fgen.make_double(txtFRate.Text.Trim().Trim().ToUpper());
                oporow["FWT"] = fgen.make_double(txtFWt.Text.Trim().Trim().ToUpper());
                oporow["FSI"] = fgen.make_double(txtFSi.Text.Trim().Trim().ToUpper());
                oporow["FMN"] = fgen.make_double(txtFMn.Text.Trim().Trim().ToUpper());
                oporow["FC"] = fgen.make_double(txtFC.Text.Trim().Trim().ToUpper());
                oporow["FMOLY"] = fgen.make_double(txtFMoly.Text.Trim().Trim().ToUpper());
                oporow["PCONS"] = fgen.make_double(txtPCons.Text.Trim().Trim().ToUpper());
                oporow["PCONTRI"] = fgen.make_double(txtPContri.Text.Trim().Trim().ToUpper());
                oporow["PRATE"] = fgen.make_double(txtPRate.Text.Trim().Trim().ToUpper());
                oporow["PWT"] = fgen.make_double(txtPWt.Text.Trim().Trim().ToUpper());
                oporow["PSI"] = fgen.make_double(txtPSi.Text.Trim().Trim().ToUpper());
                oporow["PMN"] = fgen.make_double(txtPMn.Text.Trim().Trim().ToUpper());
                oporow["PC"] = fgen.make_double(txtPC.Text.Trim().Trim().ToUpper());
                oporow["PMOLY"] = fgen.make_double(txtPMoly.Text.Trim().Trim().ToUpper());
                oporow["SCONS"] = fgen.make_double(txtSCons.Text.Trim().Trim().ToUpper());
                oporow["SCONTRI"] = fgen.make_double(txtSContri.Text.Trim().Trim().ToUpper());
                oporow["SRATE"] = fgen.make_double(txtSRate.Text.Trim().Trim().ToUpper());
                oporow["SWT"] = fgen.make_double(txtSWt.Text.Trim().Trim().ToUpper());
                oporow["SSI"] = fgen.make_double(txtSSi.Text.Trim().Trim().ToUpper());
                oporow["SMN"] = fgen.make_double(txtSMn.Text.Trim().Trim().ToUpper());
                oporow["SC"] = fgen.make_double(txtSC.Text.Trim().Trim().ToUpper());
                oporow["SMOLY"] = fgen.make_double(txtSMoly.Text.Trim().Trim().ToUpper());
                oporow["CCONS"] = fgen.make_double(txtCCons.Text.Trim().Trim().ToUpper());
                oporow["CCONTRI"] = fgen.make_double(txtCContri.Text.Trim().Trim().ToUpper());
                oporow["CRATE"] = fgen.make_double(txtCRate.Text.Trim().Trim().ToUpper());
                oporow["CWT"] = fgen.make_double(txtCWt.Text.Trim().Trim().ToUpper());
                oporow["CSI"] = fgen.make_double(txtCSi.Text.Trim().Trim().ToUpper());
                oporow["CMN"] = fgen.make_double(txtCMn.Text.Trim().Trim().ToUpper());
                oporow["CC"] = fgen.make_double(txtCC.Text.Trim().Trim().ToUpper());
                oporow["CMOLY"] = fgen.make_double(txtCMoly.Text.Trim().Trim().ToUpper());
                oporow["TOTCONS"] = fgen.make_double(txtSubCons.Text.Trim().Trim().ToUpper());
                oporow["TOTCONTRI"] = fgen.make_double(txtSubContri.Text.Trim().Trim().ToUpper());
                oporow["TOTWT"] = fgen.make_double(txtSubWt.Text.Trim().Trim().ToUpper());
                oporow["TOTSI"] = fgen.make_double(txtSubSi.Text.Trim().Trim().ToUpper());
                oporow["TOTMN"] = fgen.make_double(txtSubMn.Text.Trim().Trim().ToUpper());
                oporow["TOTC"] = fgen.make_double(txtSubC.Text.Trim().Trim().ToUpper());
                oporow["TOTMOLY"] = fgen.make_double(txtSubMoly.Text.Trim().Trim().ToUpper());
                oporow["RSI"] = fgen.make_double(txtReqSi.Text.Trim().Trim().ToUpper());
                oporow["RMN"] = fgen.make_double(txtReqMn.Text.Trim().Trim().ToUpper());
                oporow["RC"] = fgen.make_double(txtReqC.Text.Trim().Trim().ToUpper());
                oporow["RMOLY"] = fgen.make_double(txtReqMoly.Text.Trim().Trim().ToUpper());
                oporow["DSI"] = fgen.make_double(txtDiffSi.Text.Trim().Trim().ToUpper());
                oporow["DMN"] = fgen.make_double(txtDiffMn.Text.Trim().Trim().ToUpper());
                oporow["DC"] = fgen.make_double(txtDiffC.Text.Trim().Trim().ToUpper());
                oporow["DMOLY"] = fgen.make_double(txtDiffMoly.Text.Trim().Trim().ToUpper());
                oporow["MET_CONTRI"] = fgen.make_double(txtMetContri.Text.Trim().Trim().ToUpper());
                oporow["FESI_REC"] = fgen.make_double(txtFeSiRec.Text.Trim().Trim().ToUpper());
                oporow["FESI_REQ"] = fgen.make_double(txtFeSiReq.Text.Trim().Trim().ToUpper());
                oporow["FESI_RT"] = fgen.make_double(txtFeSiRate.Text.Trim().Trim().ToUpper());
                oporow["FESI_COST"] = fgen.make_double(txtFeSiCost.Text.Trim().Trim().ToUpper());
                oporow["FEMN_REC"] = fgen.make_double(txtFeMnRec.Text.Trim().Trim().ToUpper());
                oporow["FEMN_REQ"] = fgen.make_double(txtFeMnReq.Text.Trim().Trim().ToUpper());
                oporow["FEMN_RT"] = fgen.make_double(txtFeMnRate.Text.Trim().Trim().ToUpper());
                oporow["FEMN_COST"] = fgen.make_double(txtFeMnCost.Text.Trim().Trim().ToUpper());
                oporow["CSC_REC"] = fgen.make_double(txtCSCRec.Text.Trim().Trim().ToUpper());
                oporow["CSC_REQ"] = fgen.make_double(txtCSCReq.Text.Trim().Trim().ToUpper());
                oporow["CSC_RT"] = fgen.make_double(txtCSCRate.Text.Trim().Trim().ToUpper());
                oporow["CSC_COST"] = fgen.make_double(txtCSCCost.Text.Trim().Trim().ToUpper());
                oporow["MOLY_REC"] = fgen.make_double(txtMolyRec.Text.Trim().Trim().ToUpper());
                oporow["MOLY_REQ"] = fgen.make_double(txtMolyReq.Text.Trim().Trim().ToUpper());
                oporow["MOLY_RT"] = fgen.make_double(txtMolyRate.Text.Trim().Trim().ToUpper());
                oporow["MOLY_COST"] = fgen.make_double(txtMolyCost.Text.Trim().Trim().ToUpper());
                oporow["FESIMG_REC"] = fgen.make_double(txtFeSiMGRec.Text.Trim().Trim().ToUpper());
                oporow["FESIMG_REQ"] = fgen.make_double(txtFeSiMGReq.Text.Trim().Trim().ToUpper());
                oporow["FESIMG_RT"] = fgen.make_double(txtFeSiMGRate.Text.Trim().Trim().ToUpper());
                oporow["FESIMG_COST"] = fgen.make_double(txtFeSiMGCost.Text.Trim().Trim().ToUpper());
                oporow["FERRO_TOT"] = fgen.make_double(txtFerroTot.Text.Trim().Trim().ToUpper());
                oporow["META_TOT"] = fgen.make_double(txtMetTot.Text.Trim().Trim().ToUpper());
                oporow["STAGE_WT"] = fgen.make_double(txtStgWt.Text.Trim().Trim().ToUpper());
                oporow["MELTING_LOSS"] = fgen.make_double(txtMelting_Loss.Text.Trim().Trim().ToUpper());
                oporow["MELTING_STAGE_WT"] = fgen.make_double(txtMelting_Loss_Wt.Text.Trim().Trim().ToUpper());
                oporow["MAS_ALLOY1"] = fgen.make_double(txtMasterAlloy1.Text.Trim().Trim().ToUpper());
                oporow["MAS_ALLOY2"] = fgen.make_double(txtMasterAlloy2.Text.Trim().Trim().ToUpper());
                oporow["MAS_ALLOY3"] = fgen.make_double(txtMasterAlloy.Text.Trim().Trim().ToUpper());
                oporow["MAS_ALLOY_WT"] = fgen.make_double(txtMasterAlloy_Wt.Text.Trim().Trim().ToUpper());
                oporow["INNOCULATION1"] = fgen.make_double(txtInnoculation1.Text.Trim().Trim().ToUpper());
                oporow["INNOCULATION2"] = fgen.make_double(txtInnoculation2.Text.Trim().Trim().ToUpper());
                oporow["INNOCULATION3"] = fgen.make_double(txtInnoculation3.Text.Trim().Trim().ToUpper());
                oporow["INNOCULATION_WT"] = fgen.make_double(txtInnoculationWt.Text.Trim().Trim().ToUpper());
                oporow["YIELD_RET"] = fgen.make_double(txtNetYield.Text.Trim().Trim().ToUpper());
                oporow["TOT_METALLIC_RT"] = fgen.make_double(txtTotMetRate.Text.Trim().Trim().ToUpper());
                oporow["PROFIT1"] = fgen.make_double(txtProfit1.Text.Trim().Trim().ToUpper());
                oporow["PROFIT2"] = fgen.make_double(txtProfit2.Text.Trim().Trim().ToUpper());
                oporow["OVER_HEAD1"] = fgen.make_double(txtOver1.Text.Trim().Trim().ToUpper());
                oporow["OVER_HEAD2"] = fgen.make_double(txtOver2.Text.Trim().Trim().ToUpper());
                oporow["TOT_CAST_RT_OH"] = fgen.make_double(txtCastTot_OH.Text.Trim().Trim().ToUpper());
                oporow["TOT_CAST_RT"] = fgen.make_double(txtCast_Rs.Text.Trim().Trim().ToUpper());
                oporow["TRANS_COST"] = fgen.make_double(txtTrans.Text.Trim().Trim().ToUpper());
                oporow["TOOL"] = fgen.make_double(txtTool.Text.Trim().Trim().ToUpper());
                oporow["TOT_MACH_COST"] = fgen.make_double(txtTotMach.Text.Trim().Trim().ToUpper());
                oporow["PACKING"] = fgen.make_double(txtPacking.Text.Trim().Trim().ToUpper());
                oporow["HEAT"] = fgen.make_double(txtHeat.Text.Trim().Trim().ToUpper());
                oporow["FINAL"] = fgen.make_double(txtFinal.Text.Trim().Trim().ToUpper());
                oporow["INTEREST_PER"] = fgen.make_double(txtInterestPer.Text.Trim().Trim().ToUpper());
                oporow["VENDOR"] = fgen.make_double(txtVendor_Cost.Text.Trim().Trim().ToUpper());
                oporow["PORDNO"] = txtFstr.Text.Trim();
                oporow["PBASIS"] = txtFstr2.Text.Trim();
                oporow["app_by"] = "-";
                oporow["app_dt"] = vardate;
                oporow["CHAPLET"] = fgen.make_double(txtChaplet.Text.Trim().Trim().ToUpper());
                oporow["HEATING"] = fgen.make_double(txtMould_Heating.Text.Trim().Trim().ToUpper());
                oporow["MLD_OTHER"] = fgen.make_double(txtMouldingOther.Text.Trim().Trim().ToUpper());
                oporow["SLEEVE"] = fgen.make_double(txtSleeve.Text.Trim().Trim().ToUpper());
                oporow["SAND"] = fgen.make_double(txtSand.Text.Trim().Trim().ToUpper());
                oporow["PAINTING"] = fgen.make_double(txtPainting.Text.Trim().Trim().ToUpper());
                oporow["CONV_OTHER1"] = fgen.make_double(txtConver_Other1.Text.Trim().Trim().ToUpper());
                oporow["CONV_OTHER2"] = fgen.make_double(txtConver_Other2.Text.Trim().Trim().ToUpper());
                oporow["GRID_ICODE"] = sg1.Rows[i].Cells[3].Text.Trim().ToUpper();
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper().Length > 20)
                {
                    oporow["GRID_FERRO"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper().Substring(0, 20);
                }
                else
                {
                    oporow["GRID_FERRO"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
                }
                oporow["GRID_REC"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper());
                oporow["GRID_REQKG"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper());
                oporow["GRID_RATE"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper());
                oporow["GRID_COST"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper());
                oporow["GRID_PIGIRON"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper());
                oporow["GRID_CONTRI"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper());
                oporow["GRID_REQ"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper());
                oporow["GRID_DIFF"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper());
                oporow["INTEREST_PER2"] = fgen.make_double(txtInterestPer2.Text.Trim().Trim().ToUpper());
                oporow["LINE"] = hfLine.Value.Trim().ToUpper();
                oporow["CORE_TYPE"] = txtCore_Type.Text.Trim().ToUpper();
                oporow["CHILDCODE"] = txtChildCode.Text.Trim().ToUpper();
                oporow["PARENTCHILD"] = txtParentChild.Text.Trim().ToUpper();
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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "CA01");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
    }
    //------------------------------------------------------------------------------------   
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CONVERSION";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Conversion Cost", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "POWER";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Power Cost", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "METALLIC";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Metallic Rate/Ton", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BOX";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Box Size", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void ImageButton6_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CORE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Core Type", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    private void Cal()
    {
        double Actual = 0, Pattern = 0, Rejection = 0, Net = 0, Power = 0, PowerTot = 0, Core_cost = 0, Subtot = 0, Fwt = 0, f, Fsi = 0, Fmn = 0, Fc = 0, Fmoly = 0, Foundry_cost = 0, Pig_cost = 0, Steel_cost = 0, Cast_cost = 0, Cast_Rt = 0, Mould_Rt = 0;
        double Stg_Wt = 0, Fcontri = 0, Pcontri = 0, Scontri = 0, Ccontri = 0, FeSiReq = 0, FeMnReq = 0, CSCReq = 0, MolyReq = 0, MasterAlloy = 0, MasterAlloyWt = 0, Innoculation = 0, InnoculationWt = 0, Melting_Loss_Wt = 0, MetTot = 0, NetYield = 0, TotMetRate = 0, Profit = 0, Over = 0, CastTot_OH = 0;
        double grid_cost1 = 0, grid_cost2 = 0, grid_cost3 = 0, grid_cost4 = 0, grid_cost5 = 0, grid_reqkg1 = 0, grid_reqkg2 = 0, grid_reqkg3 = 0, grid_reqkg4 = 0, grid_reqkg5 = 0;
        double grid_ret1 = 0, grid_ret2 = 0, grid_ret3 = 0, grid_ret4 = 0, grid_ret5 = 0, grid_diff1 = 0, grid_diff2 = 0, grid_diff3 = 0, grid_diff4 = 0, grid_diff5 = 0;

        Actual = fgen.make_double(txtCast.Text) * fgen.make_double(txtCast_No.Text);
        txtActual.Text = Math.Round(Actual, 2).ToString();

        Pattern = (Actual / fgen.make_double(txtBunch.Text) * 100);
        txtPattern.Text = Math.Round(Pattern, 2).ToString();

        Rejection = 1 - (fgen.make_double(txtRej.Text) / 100);
        Net = Pattern * Rejection;
        txtNet.Text = Math.Round(Net, 2).ToString();

        //Power = ((fgen.make_double(txtElect.Text) * fgen.make_double(txtAux.Text)) + (fgen.make_double(txtElect.Text) * fgen.make_double(txtMetling.Text))) / fgen.make_double(txtSubWt.Text);
        // PowerTot = (Power / Net / 2) * 100;
        Power = ((fgen.make_double(txtElect.Text) * fgen.make_double(txtAux.Text)) + (fgen.make_double(txtElect.Text) * fgen.make_double(txtMetling.Text)));
        PowerTot = (Power / 1000 / Net) * 100;
        txtPower.Text = Math.Round(PowerTot, 2).ToString();

        Core_cost = (fgen.make_double(txtCore_Rt.Text) * fgen.make_double(txtCore_Wt.Text)) * 1.05 / fgen.make_double(txtCast.Text);
        txtCore_Cost.Text = Math.Round(Core_cost, 2).ToString();

        Mould_Rt = (fgen.make_double(txtChaplet.Text) + fgen.make_double(txtMould_Heating.Text) + fgen.make_double(txtMouldingOther.Text) + fgen.make_double(txtSleeve.Text) + fgen.make_double(txtSand.Text)) / Actual / fgen.make_double(txtCast_No.Text);
        txtMould_Rt.Text = Math.Round(Mould_Rt, 2).ToString();

        Subtot = fgen.make_double(txtLabour.Text) + fgen.make_double(txtMaint.Text) + fgen.make_double(txtFettling.Text) + fgen.make_double(txtInterest.Text) + fgen.make_double(txtDepr.Text) + fgen.make_double(txtOther.Text) + fgen.make_double(txtConver_Other1.Text) + fgen.make_double(txtPainting.Text) + fgen.make_double(txtConver_Other1.Text) + fgen.make_double(txtConver_Other2.Text);
        txtSubTotal.Text = Math.Round(Subtot, 2).ToString();
        txtGrandTot.Text = Math.Round(PowerTot + Mould_Rt + Core_cost + Subtot, 2).ToString();

        Fcontri = fgen.make_double(txtFContri.Text);

        //Fcontri = 100 - Net;
        Fwt = (Fcontri * 500) / 100;
        // txtFContri.Text = Math.Round(Fcontri, 2).ToString();
        txtFWt.Text = Math.Round(Fwt, 2).ToString();

        Fsi = (fgen.make_double(txtReqSi.Text) * Fcontri) / 100;
        Fmn = (fgen.make_double(txtReqMn.Text) * Fcontri) / 100;
        Fc = (fgen.make_double(txtReqC.Text) * Fcontri) / 100;
        Fmoly = (fgen.make_double(txtReqMoly.Text) * Fcontri) / 100;

        txtFSi.Text = Math.Round(Fsi, 4).ToString();
        txtFMn.Text = Math.Round(Fmn, 4).ToString();
        txtFC.Text = Math.Round(Fc, 4).ToString();
        txtFMoly.Text = Math.Round(Fmoly, 4).ToString();

        Pcontri = ((100 - Fcontri) * fgen.make_double(txtPCons.Text)) / 100;
        Scontri = ((100 - Fcontri) * fgen.make_double(txtSCons.Text)) / 100;
        Ccontri = ((100 - Fcontri) * fgen.make_double(txtCCons.Text)) / 100;

        txtPContri.Text = Math.Round(Pcontri, 2).ToString();
        txtPWt.Text = Math.Round((Pcontri * 1000) / 100, 2).ToString();

        txtSContri.Text = Math.Round(Scontri, 2).ToString();
        txtSWt.Text = Math.Round((Scontri * 500) / 100, 2).ToString();

        txtCContri.Text = Math.Round(Ccontri, 2).ToString();
        txtCWt.Text = Math.Round((Ccontri * 1000) / 100, 2).ToString();

        txtSubCons.Text = Math.Round(fgen.make_double(txtPCons.Text) + fgen.make_double(txtSCons.Text) + fgen.make_double(txtCCons.Text), 2).ToString();
        txtSubContri.Text = Math.Round(Fcontri + Pcontri + Scontri + Ccontri, 2).ToString();

        txtDiffSi.Text = Math.Round(fgen.make_double(txtReqSi.Text) - Fsi, 4).ToString();
        txtDiffMn.Text = Math.Round(fgen.make_double(txtReqMn.Text) - Fmn, 4).ToString();
        txtDiffC.Text = Math.Round(fgen.make_double(txtReqC.Text) - Fc, 4).ToString();
        txtDiffMoly.Text = Math.Round(fgen.make_double(txtReqMoly.Text) - Fmoly, 4).ToString();

        Foundry_cost = (Fcontri * fgen.make_double(txtFRate.Text)) / 100;
        Pig_cost = (Pcontri * fgen.make_double(txtPRate.Text)) / 100;
        Steel_cost = (Scontri * fgen.make_double(txtSRate.Text)) / 100;
        Cast_cost = (Ccontri * fgen.make_double(txtCRate.Text)) / 100;
        txtMetContri.Text = Math.Round((Foundry_cost + Pig_cost + Steel_cost + Cast_cost) * 500, 2).ToString();

        //FeSiReq = ((fgen.make_double(txtSubWt.Text) * 0.016) / fgen.make_double(txtFeSiRec.Text)) * 100;
        //FeMnReq = ((fgen.make_double(txtSubWt.Text) * 0.0034) / fgen.make_double(txtFeMnRec.Text)) * 100;
        //CSCReq = ((fgen.make_double(txtSubWt.Text) * 0.0217) / fgen.make_double(txtCSCRec.Text)) * 100;
        FeSiReq = ((fgen.make_double(txtSubWt.Text) * fgen.make_double(txtDiffSi.Text)) / fgen.make_double(txtFeSiRec.Text));
        FeMnReq = ((fgen.make_double(txtSubWt.Text) * fgen.make_double(txtDiffMn.Text)) / fgen.make_double(txtFeMnRec.Text));
        CSCReq = ((fgen.make_double(txtSubWt.Text) * fgen.make_double(txtDiffC.Text)) / fgen.make_double(txtCSCRec.Text));
        MolyReq = ((fgen.make_double(txtSubWt.Text) * fgen.make_double(txtDiffMoly.Text)) / fgen.make_double(txtMolyRec.Text));

        txtFeSiReq.Text = Math.Round(FeSiReq, 2).ToString();
        txtFeMnReq.Text = Math.Round(FeMnReq, 2).ToString();
        txtCSCReq.Text = Math.Round(CSCReq, 2).ToString();
        txtMolyReq.Text = Math.Round(MolyReq, 2).ToString();

        txtFeSiCost.Text = Math.Round(FeSiReq * fgen.make_double(txtFeSiRate.Text), 2).ToString();
        txtFeMnCost.Text = Math.Round(FeMnReq * fgen.make_double(txtFeMnRate.Text), 2).ToString();
        txtCSCCost.Text = Math.Round(CSCReq * fgen.make_double(txtCSCRate.Text), 2).ToString();
        txtMolyCost.Text = Math.Round(MolyReq * fgen.make_double(txtMolyRate.Text), 2).ToString();
        txtFeSiMGCost.Text = Math.Round(fgen.make_double(txtFeSiMGReq.Text) * fgen.make_double(txtFeSiMGRate.Text), 2).ToString();

        #region Count =5
        if (sg1.Rows.Count == 5)
        {
            grid_ret1 = (fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;
            grid_ret2 = (fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;
            grid_ret3 = (fgen.make_double(((TextBox)sg1.Rows[2].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;
            grid_ret4 = (fgen.make_double(((TextBox)sg1.Rows[3].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;
            grid_ret5 = (fgen.make_double(((TextBox)sg1.Rows[4].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;

            grid_diff1 = fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t8")).Text) - grid_ret1;
            grid_diff2 = fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t8")).Text) - grid_ret2;
            grid_diff3 = fgen.make_double(((TextBox)sg1.Rows[2].FindControl("sg1_t8")).Text) - grid_ret3;
            grid_diff4 = fgen.make_double(((TextBox)sg1.Rows[3].FindControl("sg1_t8")).Text) - grid_ret4;
            grid_diff5 = fgen.make_double(((TextBox)sg1.Rows[4].FindControl("sg1_t8")).Text) - grid_ret5;

            grid_reqkg1 = (fgen.make_double(txtSubWt.Text) * grid_diff1) / fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t2")).Text);
            grid_reqkg2 = (fgen.make_double(txtSubWt.Text) * grid_diff2) / fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t2")).Text);
            grid_reqkg3 = (fgen.make_double(txtSubWt.Text) * grid_diff3) / fgen.make_double(((TextBox)sg1.Rows[2].FindControl("sg1_t2")).Text);
            grid_reqkg4 = (fgen.make_double(txtSubWt.Text) * grid_diff4) / fgen.make_double(((TextBox)sg1.Rows[3].FindControl("sg1_t2")).Text);
            grid_reqkg5 = (fgen.make_double(txtSubWt.Text) * grid_diff5) / fgen.make_double(((TextBox)sg1.Rows[4].FindControl("sg1_t2")).Text);

            grid_cost1 = grid_reqkg1 * fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t4")).Text);
            grid_cost2 = grid_reqkg2 * fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t4")).Text);
            grid_cost3 = grid_reqkg3 * fgen.make_double(((TextBox)sg1.Rows[2].FindControl("sg1_t4")).Text);
            grid_cost4 = grid_reqkg4 * fgen.make_double(((TextBox)sg1.Rows[3].FindControl("sg1_t4")).Text);
            grid_cost5 = grid_reqkg5 * fgen.make_double(((TextBox)sg1.Rows[4].FindControl("sg1_t4")).Text);

            ((TextBox)sg1.Rows[0].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg2, 2).ToString();
            ((TextBox)sg1.Rows[2].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg3, 2).ToString();
            ((TextBox)sg1.Rows[3].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg4, 2).ToString();
            ((TextBox)sg1.Rows[4].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg5, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t7")).Text = Math.Round(grid_ret1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t7")).Text = Math.Round(grid_ret2, 2).ToString();
            ((TextBox)sg1.Rows[2].FindControl("sg1_t7")).Text = Math.Round(grid_ret3, 2).ToString();
            ((TextBox)sg1.Rows[3].FindControl("sg1_t7")).Text = Math.Round(grid_ret4, 2).ToString();
            ((TextBox)sg1.Rows[4].FindControl("sg1_t7")).Text = Math.Round(grid_ret5, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t9")).Text = Math.Round(grid_diff1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t9")).Text = Math.Round(grid_diff2, 2).ToString();
            ((TextBox)sg1.Rows[2].FindControl("sg1_t9")).Text = Math.Round(grid_diff3, 2).ToString();
            ((TextBox)sg1.Rows[3].FindControl("sg1_t9")).Text = Math.Round(grid_diff4, 2).ToString();
            ((TextBox)sg1.Rows[4].FindControl("sg1_t9")).Text = Math.Round(grid_diff5, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t5")).Text = Math.Round(grid_cost1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t5")).Text = Math.Round(grid_cost2, 2).ToString();
            ((TextBox)sg1.Rows[2].FindControl("sg1_t5")).Text = Math.Round(grid_cost3, 2).ToString();
            ((TextBox)sg1.Rows[3].FindControl("sg1_t5")).Text = Math.Round(grid_cost4, 2).ToString();
            ((TextBox)sg1.Rows[4].FindControl("sg1_t5")).Text = Math.Round(grid_cost5, 2).ToString();
        }
        #endregion

        #region Count =4
        else if (sg1.Rows.Count == 4)
        {
            grid_ret1 = (fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;
            grid_ret2 = (fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;
            grid_ret3 = (fgen.make_double(((TextBox)sg1.Rows[2].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;
            grid_ret4 = (fgen.make_double(((TextBox)sg1.Rows[3].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;

            grid_diff1 = fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t8")).Text) - grid_ret1;
            grid_diff2 = fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t8")).Text) - grid_ret2;
            grid_diff3 = fgen.make_double(((TextBox)sg1.Rows[2].FindControl("sg1_t8")).Text) - grid_ret3;
            grid_diff4 = fgen.make_double(((TextBox)sg1.Rows[3].FindControl("sg1_t8")).Text) - grid_ret4;

            grid_reqkg1 = (fgen.make_double(txtSubWt.Text) * grid_diff1) / fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t2")).Text);
            grid_reqkg2 = (fgen.make_double(txtSubWt.Text) * grid_diff2) / fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t2")).Text);
            grid_reqkg3 = (fgen.make_double(txtSubWt.Text) * grid_diff3) / fgen.make_double(((TextBox)sg1.Rows[2].FindControl("sg1_t2")).Text);
            grid_reqkg4 = (fgen.make_double(txtSubWt.Text) * grid_diff4) / fgen.make_double(((TextBox)sg1.Rows[3].FindControl("sg1_t2")).Text);

            grid_cost1 = grid_reqkg1 * fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t4")).Text);
            grid_cost2 = grid_reqkg2 * fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t4")).Text);
            grid_cost3 = grid_reqkg3 * fgen.make_double(((TextBox)sg1.Rows[2].FindControl("sg1_t4")).Text);
            grid_cost4 = grid_reqkg4 * fgen.make_double(((TextBox)sg1.Rows[3].FindControl("sg1_t4")).Text);

            ((TextBox)sg1.Rows[0].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg2, 2).ToString();
            ((TextBox)sg1.Rows[2].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg3, 2).ToString();
            ((TextBox)sg1.Rows[3].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg4, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t7")).Text = Math.Round(grid_ret1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t7")).Text = Math.Round(grid_ret2, 2).ToString();
            ((TextBox)sg1.Rows[2].FindControl("sg1_t7")).Text = Math.Round(grid_ret3, 2).ToString();
            ((TextBox)sg1.Rows[3].FindControl("sg1_t7")).Text = Math.Round(grid_ret4, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t9")).Text = Math.Round(grid_diff1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t9")).Text = Math.Round(grid_diff2, 2).ToString();
            ((TextBox)sg1.Rows[2].FindControl("sg1_t9")).Text = Math.Round(grid_diff3, 2).ToString();
            ((TextBox)sg1.Rows[3].FindControl("sg1_t9")).Text = Math.Round(grid_diff4, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t5")).Text = Math.Round(grid_cost1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t5")).Text = Math.Round(grid_cost2, 2).ToString();
            ((TextBox)sg1.Rows[2].FindControl("sg1_t5")).Text = Math.Round(grid_cost3, 2).ToString();
            ((TextBox)sg1.Rows[3].FindControl("sg1_t5")).Text = Math.Round(grid_cost4, 2).ToString();
        }
        #endregion

        #region Count =3
        else if (sg1.Rows.Count == 3)
        {
            grid_ret1 = (fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;
            grid_ret2 = (fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;
            grid_ret3 = (fgen.make_double(((TextBox)sg1.Rows[2].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;

            grid_diff1 = fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t8")).Text) - grid_ret1;
            grid_diff2 = fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t8")).Text) - grid_ret2;
            grid_diff3 = fgen.make_double(((TextBox)sg1.Rows[2].FindControl("sg1_t8")).Text) - grid_ret3;

            grid_reqkg1 = (fgen.make_double(txtSubWt.Text) * grid_diff1) / fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t2")).Text);
            grid_reqkg2 = (fgen.make_double(txtSubWt.Text) * grid_diff2) / fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t2")).Text);
            grid_reqkg3 = (fgen.make_double(txtSubWt.Text) * grid_diff3) / fgen.make_double(((TextBox)sg1.Rows[2].FindControl("sg1_t2")).Text);

            grid_cost1 = grid_reqkg1 * fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t4")).Text);
            grid_cost2 = grid_reqkg2 * fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t4")).Text);
            grid_cost3 = grid_reqkg3 * fgen.make_double(((TextBox)sg1.Rows[2].FindControl("sg1_t4")).Text);

            ((TextBox)sg1.Rows[0].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg2, 2).ToString();
            ((TextBox)sg1.Rows[2].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg3, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t7")).Text = Math.Round(grid_ret1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t7")).Text = Math.Round(grid_ret2, 2).ToString();
            ((TextBox)sg1.Rows[2].FindControl("sg1_t7")).Text = Math.Round(grid_ret3, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t9")).Text = Math.Round(grid_diff1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t9")).Text = Math.Round(grid_diff2, 2).ToString();
            ((TextBox)sg1.Rows[2].FindControl("sg1_t9")).Text = Math.Round(grid_diff3, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t5")).Text = Math.Round(grid_cost1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t5")).Text = Math.Round(grid_cost2, 2).ToString();
            ((TextBox)sg1.Rows[2].FindControl("sg1_t5")).Text = Math.Round(grid_cost3, 2).ToString();
        }
        #endregion

        #region Count =2
        else if (sg1.Rows.Count == 2)
        {
            grid_ret1 = (fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;
            grid_ret2 = (fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;

            grid_diff1 = fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t8")).Text) - grid_ret1;
            grid_diff2 = fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t8")).Text) - grid_ret2;

            grid_reqkg1 = (fgen.make_double(txtSubWt.Text) * grid_diff1) / fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t2")).Text);
            grid_reqkg2 = (fgen.make_double(txtSubWt.Text) * grid_diff2) / fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t2")).Text);

            grid_cost1 = grid_reqkg1 * fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t4")).Text);
            grid_cost2 = grid_reqkg2 * fgen.make_double(((TextBox)sg1.Rows[1].FindControl("sg1_t4")).Text);

            ((TextBox)sg1.Rows[0].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg2, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t7")).Text = Math.Round(grid_ret1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t7")).Text = Math.Round(grid_ret2, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t9")).Text = Math.Round(grid_diff1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t9")).Text = Math.Round(grid_diff2, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t5")).Text = Math.Round(grid_cost1, 2).ToString();
            ((TextBox)sg1.Rows[1].FindControl("sg1_t5")).Text = Math.Round(grid_cost2, 2).ToString();
        }
        #endregion

        #region Count =1
        else if (sg1.Rows.Count == 1)
        {
            grid_ret1 = (fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t8")).Text) * fgen.make_double(txtFContri.Text)) / 100;

            grid_diff1 = fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t8")).Text) - grid_ret1;

            grid_reqkg1 = (fgen.make_double(txtSubWt.Text) * grid_diff1) / fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t2")).Text);

            grid_cost1 = grid_reqkg1 * fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t4")).Text);

            ((TextBox)sg1.Rows[0].FindControl("sg1_t3")).Text = Math.Round(grid_reqkg1, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t7")).Text = Math.Round(grid_ret1, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t9")).Text = Math.Round(grid_diff1, 2).ToString();

            ((TextBox)sg1.Rows[0].FindControl("sg1_t5")).Text = Math.Round(grid_cost1, 2).ToString();
        }
        #endregion
        txtFerroTot.Text = Math.Round(fgen.make_double(txtFeSiCost.Text) + fgen.make_double(txtFeMnCost.Text) + fgen.make_double(txtCSCCost.Text) + fgen.make_double(txtMolyCost.Text) + fgen.make_double(txtFeSiMGCost.Text) + Math.Round(fgen.make_double(grid_cost1.ToString().Replace("NaN", "0").Replace("Infinity", "0")), 2) + Math.Round(fgen.make_double(grid_cost2.ToString().Replace("NaN", "0").Replace("Infinity", "0")), 2) + Math.Round(fgen.make_double(grid_cost3.ToString().Replace("NaN", "0").Replace("Infinity", "0")), 2) + Math.Round(fgen.make_double(grid_cost4.ToString().Replace("NaN", "0").Replace("Infinity", "0")), 2) + Math.Round(fgen.make_double(grid_cost5.ToString().Replace("NaN", "0").Replace("Infinity", "0")), 2), 2).ToString();
        MetTot = fgen.make_double(txtFerroTot.Text) + fgen.make_double(txtMetContri.Text);
        txtMetTot.Text = Math.Round(MetTot, 2).ToString();
        Stg_Wt = fgen.make_double(txtSubWt.Text) + FeSiReq + FeMnReq + CSCReq + MolyReq + fgen.make_double(txtFeSiMGReq.Text) + fgen.make_double(grid_reqkg1.ToString().Replace("NaN", "0").Replace("Infinity", "0")) + fgen.make_double(grid_reqkg2.ToString().Replace("NaN", "0").Replace("Infinity", "0")) + fgen.make_double(grid_reqkg3.ToString().Replace("NaN", "0").Replace("Infinity", "0"));
        txtStgWt.Text = Math.Round(Stg_Wt, 2).ToString();
        Melting_Loss_Wt = Stg_Wt - ((fgen.make_double(txtMelting_Loss.Text) * Stg_Wt) / 100);
        txtMelting_Loss_Wt.Text = Math.Round(Melting_Loss_Wt, 2).ToString();

        MasterAlloy = fgen.make_double(txtMasterAlloy1.Text) * fgen.make_double(txtMasterAlloy2.Text) / 100;
        MasterAlloyWt = fgen.make_double(txtMasterAlloy1.Text) * Melting_Loss_Wt / 100;
        txtMasterAlloy.Text = Math.Round(MasterAlloy * Melting_Loss_Wt, 2).ToString();
        txtMasterAlloy_Wt.Text = Math.Round(Melting_Loss_Wt + MasterAlloyWt, 2).ToString();

        Innoculation = fgen.make_double(txtInnoculation1.Text) * fgen.make_double(txtInnoculation2.Text) / 100;
        InnoculationWt = fgen.make_double(txtInnoculation1.Text) * (Melting_Loss_Wt + MasterAlloyWt) / 100;
        txtInnoculation3.Text = Math.Round(fgen.make_double(txtSubWt.Text) * (Innoculation), 2).ToString();
        txtInnoculationWt.Text = Math.Round((Melting_Loss_Wt + MasterAlloyWt) + (InnoculationWt), 2).ToString();
        NetYield = ((Melting_Loss_Wt + MasterAlloyWt) + (InnoculationWt)) - Fwt;
        txtNetYield.Text = Math.Round(NetYield, 2).ToString();
        TotMetRate = (MetTot - (fgen.make_double(txtFRate.Text) * Fwt)) / NetYield;
        txtTotMetRate.Text = Math.Round(TotMetRate, 2).ToString();

        Cast_Rt = fgen.make_double(txtGrandTot.Text) + TotMetRate;
        txtCast_Rt.Text = Math.Round(Cast_Rt, 2).ToString();

        Profit = (fgen.make_double(txtProfit1.Text) * Cast_Rt) / 100;
        Over = (fgen.make_double(txtOver1.Text) * Cast_Rt) / 100;
        CastTot_OH = Profit + Over + Cast_Rt;
        txtProfit2.Text = Math.Round(Profit, 2).ToString();
        txtOver2.Text = Math.Round(Over, 2).ToString();

        txtCastTot_OH.Text = Math.Round(CastTot_OH, 2).ToString();
        txtCast_Rs.Text = Math.Round(CastTot_OH * fgen.make_double(txtCast.Text), 2).ToString();
        txtFinal.Text = Math.Round(fgen.make_double(txtCast_Rs.Text) + fgen.make_double(txtTrans.Text) + fgen.make_double(txtTool.Text) + fgen.make_double(txtPacking.Text) + fgen.make_double(txtHeat.Text), 2).ToString();
        txtInterestPer.Text = Math.Round((fgen.make_double(txtFinal.Text) * fgen.make_double(txtInterestPer2.Text) / 100), 2).ToString();
        txtVendor_Cost.Text = Math.Round(fgen.make_double(txtFinal.Text) + fgen.make_double(txtInterestPer.Text), 2).ToString();
    }
    //------------------------------------------------------------------------------------
    protected void btnCal_ServerClick(object sender, EventArgs e)
    {
        Cal();
        btnsave.Disabled = false;
    }
    //------------------------------------------------------------------------------------
    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        // Hidden Field
        sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(Int32)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f2", typeof(string)));
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
            sg1_dr["sg1_SrNo"] = sg1_dt.Rows.Count + 1;
            sg1_dr["sg1_f1"] = "-";
            sg1_dr["sg1_f2"] = "-";
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

            sg1.Columns[0].HeaderStyle.Width = 30;
            sg1.Columns[1].HeaderStyle.Width = 30;
            sg1.Columns[2].HeaderStyle.Width = 50;
            sg1.Columns[3].HeaderStyle.Width = 80;
            sg1.Columns[4].HeaderStyle.Width = 30;
            sg1.Columns[5].HeaderStyle.Width = 250;
            sg1.Columns[6].HeaderStyle.Width = 100;
            sg1.Columns[7].HeaderStyle.Width = 100;
            sg1.Columns[8].HeaderStyle.Width = 100;
            sg1.Columns[9].HeaderStyle.Width = 100;
            sg1.Columns[10].HeaderStyle.Width = 100;
            sg1.Columns[11].HeaderStyle.Width = 100;
            sg1.Columns[12].HeaderStyle.Width = 100;
            sg1.Columns[13].HeaderStyle.Width = 100;
            sg1.Columns[14].HeaderStyle.Width = 100;
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
                }
                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void Img_Steel_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "STEEL";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Item", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void Img_Carburiser_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CARBURISER";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Item", frm_qstr);
    }
    //------------------------------------------------------------------------------------
}