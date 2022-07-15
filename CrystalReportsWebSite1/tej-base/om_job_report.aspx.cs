using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_job_report : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", xStartDt = "", Enable = "";
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
                    //frm_mbr = "01";
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

        //txtlbl8.Attributes.Add("readonly", "readonly");
        //txtlbl9.Attributes.Add("readonly", "readonly");



        // to hide and show to tab panel
        tab5.Visible = false;
        tab4.Visible = false;
        tab3.Visible = false;
        tab2.Visible = true;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        btnprint.Visible = false;

        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnlist.Disabled = false;
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
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnlist.Disabled = true;
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
        frm_tabname = "ivoucherw";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "JX");

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

            case "PARTYCODE":
                //SQuery = "SELECT distinct a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum||trim(a.icode)||trim(a.srno) AS FSTR,trim(a.Vchnum) as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,c.Iname,b.Aname as Supplier,a.Invno,A.Refnum as chl_no from ivoucher a ,famst b,item c where trim(A.icode)=trim(c.icode) and trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + DateRange + " and NVL(a.inspected,'N')='N' order by a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum||trim(a.icode)||trim(a.srno)";
                SQuery = "select trim(acode) AS FSTR, trim(aname) as party_name, trim(acode) as party_code   from famst where acode like '06%' order by acode";
                break;

            case "MACHNECODE":
                SQuery = "select trim(acode)||'/'||srno as fstr,mchname as Machine_Name,trim(acode)||'/'||srno as Machine_Code,mch_seq from pmaint where branchcd='" + frm_mbr + "' and type='10' and (upper(trim(mchname)) like '%SHEET%' or upper(trim(mchname)) like '%PAPER%CUTTING%' or upper(trim(mchname)) like '%CORR%')  order by acode,srno";
                break;

            case "TICODE":
                SQuery = "select trim(acode)||'/'||srno as fstr,mchname as Machine_Name,trim(acode)||'/'||srno as Machine_Code,mch_seq from pmaint where branchcd='" + frm_mbr + "' and type='10' order by acode,srno";
                break;

            case "TYPE":
                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                if (sg1.Rows.Count > 0)
                {
                    col1 = "";
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        if (col1.Length > 0) col1 = col1 + ",'" + frm_mbr + txtlbl4.Text + gr.Cells[13].Text.Trim() + gr.Cells[14].Text.Trim() + gr.Cells[15].Text.Trim() + "'";
                        else col1 = "'" + frm_mbr + txtlbl4.Text + gr.Cells[13].Text.Trim() + gr.Cells[14].Text.Trim() + gr.Cells[15].Text.Trim() + "'";
                    }
                    SQuery = "select b.fstr,b.tout,b.treco,b.vchnum,b.vchdate,b.icode,b.unit,b.cgst,b.sgst,b.igst,c.iname from ( select a.fstr,max(a.out) as tout,max(a.reco) as treco,a.vchnum,a.vchdate,a.icode,a.unit,max(a.ctax) as cgst,max(a.stax) as sgst,max(a.itax) as igst from (select trim(branchcd)||trim(acode)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(icode) as fstr,iqtyout as out,0 as reco,trim(vchnum) as vchnum, to_char(vchdate,'dd/mm/yyyy') as vchdate, trim(icode) as icode,trim(unit) as unit,(case when post=1 then exc_amt  else 0  end ) as ctax,(case when post=2 then exc_amt  else 0  end ) as itax, (case when post=1 then cess_pu else 0  end ) as stax from ivoucher where branchcd='" + frm_mbr + "' and type='21' and vchdate between to_date('01/07/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text + "'  union all select trim(branchcd)||trim(acode)||trim(rgpnum)||to_char(rgpdate,'dd/mm/yyyy')||trim(icode) as fstr,0 as out, iqtyout as reco,trim(rgpnum) as vchnum, to_char(rgpdate,'dd/mm/yyyy') as vchdate, trim(icode) as icode,trim(unit) as unit, 0  as ctax,0 as itax, 0 as stax from ivoucherw where branchcd='" + frm_mbr + "' and type='JX' and vchdate between to_date('01/07/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text + "' ) a group by a.fstr,a.vchnum,a.vchdate,a.icode,a.unit) b ,item c where trim(b.icode)= trim(c.icode) AND b.tout > b.treco and b.fstr not in (" + col1 + ") order by vchnum";

                }
                else
                {
                    SQuery = "select b.fstr,b.tout,b.treco,b.vchnum,b.vchdate,b.icode,b.unit,b.cgst,b.sgst,b.igst,c.iname from ( select a.fstr,max(a.out) as tout,max(a.reco) as treco,a.vchnum,a.vchdate,a.icode,a.unit,max(a.ctax) as cgst,max(a.stax) as sgst,max(a.itax) as igst from (select trim(branchcd)||trim(acode)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(icode) as fstr,iqtyout as out,0 as reco,trim(vchnum) as vchnum, to_char(vchdate,'dd/mm/yyyy') as vchdate, trim(icode) as icode,trim(unit) as unit,(case when post=1 then exc_amt  else 0  end ) as ctax,(case when post=2 then exc_amt  else 0  end ) as itax, (case when post=1 then cess_pu else 0  end ) as stax from ivoucher where branchcd='" + frm_mbr + "' and type='21' and vchdate between to_date('01/07/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text + "'  union all select trim(branchcd)||trim(acode)||trim(rgpnum)||to_char(rgpdate,'dd/mm/yyyy')||trim(icode) as fstr,0 as out, iqtyout as reco,trim(rgpnum) as vchnum, to_char(rgpdate,'dd/mm/yyyy') as vchdate, trim(icode) as icode,trim(unit) as unit, 0  as ctax,0 as itax, 0 as stax from ivoucherw where branchcd='" + frm_mbr + "' and type='JX' and vchdate between to_date('01/07/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text + "' ) a group by a.fstr,a.vchnum,a.vchdate,a.icode,a.unit) b ,item c where trim(b.icode)= trim(c.icode) AND b.tout > b.treco order by vchum";
                }
                break;

            case "SG1_ROW_ADD1":
            case "SG1_ROW_ADD_E1":
                string stage = "0";
                stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[12].Text;
                //SQuery = "select distinct  trim(a.Icode)||'.'||trim(a.vchnum) as fstr, '['||trim(a.COL16)||' Clr]'||trim(b.Iname) as Item_Name,trim(a.Icode)||'.'||trim(a.vchnum) as Item_Code,b.Cpartno as Part_No,d.aname as Customer,a.ENQDT as Delv_Dt,a.vchnum as Job_No,a.col18||'X'||a.col19 as Cut_Size from costestimate a, item b,itwstage c,famst d where trim(nvl(a.app_by,'-'))!='-' and trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(d.acode) and a.type='30' and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.status='N' and c.stagec='" + stage + "' order by trim(a.Icode)||'.'||trim(a.vchnum)";
                //SQuery = "select a.vchnum, to_date(a.vchdate,'dd/mm/yyyy') as vchdate, trim(a.icode),b.iname trim(a.unit), iqtyin from from ivoucher a, item b where branchcd='00' and type='09' and vchdate between to_date('01/07/2018','dd/mm/yyyy') and to_date('"+ todt +"','dd/mm/yyyy') and trim(acode)='"+ txtlbl4.Text +"' and trim(a.icode)=trim(b.icode) ";
                //SQuery = "select a.vchnum, to_date(a.vchdate,'dd/mm/yyyy') as vchdate, trim(a.icode),b.iname, trim(a.unit), a.iqtyin  from ivoucher a, item b where a.branchcd='00' and a.type='09' and a.vchdate between to_date('01/07/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and trim(a.acode)='" + txtlbl4.Text + "' and trim(a.icode)=trim(b.icode) ";
                SQuery = "select trim(a.type)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr, trim(a.vchnum) as vchnum,trim(a.acode) as code,to_char(a.vchdate,'dd/mm/yyyy') as vchdate, trim(a.invno) as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate ,trim(a.icode) as icode,trim(b.iname) as iname,trim(a.unit) as unit,a.iqtyin from ivoucher a, item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='09' and a.vchdate between to_date('01/07/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and trim(a.acode)='" + txtlbl4.Text + "' order by vchnum";

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
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    //SQuery = "sELECT distinct trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')as fstr,to_char(Vchdate,'dd/mm/yyyy')  as Sheet_Dt,Vchnum as Sheet_No,Job_no,Job_Dt,Ent_by,Ent_Dt FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE  " + DateRange + "   ORDER BY VCHNUM DESC";// by yogita

                    SQuery = "select distinct trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr , trim(a.vchnum) as vchnum, to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as party_code,trim(a.icode) as item_code,trim(b.iname) as item_name,trim(a.rgpnum) as challan_no from ivoucherw a , item b where trim(a.icode)=trim(b.icode) and  a.branchcd='" + frm_mbr + "' and a.type='JX' and a.vchdate between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";

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
            frm_vty = "JX";
            lbl1a.Text = frm_vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);

            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

    }

    void newCase(string vty)
    {
        #region
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' and vchdate " + DateRange + " AND TYPE='" + frm_vty + "' ", 6, "VCH");
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
        //int j;
        //for (j = i; j < 10; j++)
        //{
        //    sg1_add_blankrows();
        //}

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
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        double db = 0, db1 = 0, db2 = 0;
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }

        string chk_freeze = "";

        //if (chk_freeze == "1")
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Rolling Freeze Date !!");
        //    return;
        //}
        //if (chk_freeze == "2")
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Saving Not allowed due to Fixed Freeze Date !!");
        //    return;
        //}

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus();
            return;
        }

        if (txtlbl4.Text == "-")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill Party First!!");
            return;
        }
        if (sg1.Rows.Count <= 1)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Select Item");
            return;
        }

        //for (i = 0; i < sg1.Rows.Count - 1; i++)
        //{
        //    if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() == "-" || ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() == "" || ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim() == "-" || ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim() == "")
        //    {
        //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " Please Fill Start Time and End Time ");
        //        ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Focus();
        //        return;
        //    }
        //}       

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
        SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + DateRange + " and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
        fgen.Fn_FillChart(frm_cocd, frm_qstr, "Testing Chart", "line", "Main Heading", "Sub Heading", SQuery, "");
        //hffield.Value = "Print";
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("Select Type for Print", frm_qstr);
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
        frm_vty = "JX";
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        string cond = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                // fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");


                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History

                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + "Delete");
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
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;

                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "select a.*,to_char(a.rgpdate,'dd/mm/yyyy') as rgpdate2,to_char(a.invdate,'dd/mm/yyyy') as invdate2,to_char(a.gedate,'dd/mm/yyyy') as gedate2,trim(b.iname) as iname,trim(c.aname) as aname,b.unit as bunit,trim(d.unit) as unit3 ,trim(d.iname) as iname3 from ivoucherw a , item b,famst c,item d where trim(a.icode)=trim(b.icode) and trim(a.rcode)=trim(d.icode) and trim(a.acode)=trim(c.acode) and  a.branchcd='" + frm_mbr + "' and a.type='JX' and a.vchdate between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ";
                    //SQuery = "Select a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt,t.name,i.iname,i.unit from " + frm_tabname + " a,type t,item i where trim(a.prevcode)=trim(t.type1) and id='D' and trim(a.icode)=trim(i.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl4.Text = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["aname"].ToString().Trim();
                        txtlbl4.Text = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[i]["ent_by"].ToString().Trim();
                        txtlbl3.Text = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl5.Text = dt.Rows[i]["eDt_by"].ToString().Trim();
                        txtlbl6.Text = Convert.ToDateTime(dt.Rows[0]["eDt_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        // txtlbl4.Text = dt.Rows[i]["prevcode"].ToString().Trim();
                        //txtlbl4a.Text = dt.Rows[i]["name"].ToString().Trim();
                        //txtlbl7.Text = dt.Rows[i]["mchcode"].ToString().Trim();
                        //txtlbl7a.Text = dt.Rows[i]["ename"].ToString().Trim();

                        // doc_addl.Value = dt.Rows[0]["srno"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["naration"].ToString().Trim();
                        create_tab();
                        sg1_dr = null;
                        double db3 = 0, db4 = 0;
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
                            sg1_dr["sg1_f15"] = dt.Rows[i]["rgpnum"].ToString().Trim();
                            sg1_dr["sg1_f1"] = dt.Rows[i]["rgpdate2"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["bunit"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["iqtyout"].ToString().Trim();
                            sg1_dr["sg1_f6"] = dt.Rows[i]["exc_amt"].ToString().Trim();
                            sg1_dr["sg1_f7"] = dt.Rows[i]["cess_pu"].ToString().Trim();
                            sg1_dr["sg1_f8"] = dt.Rows[i]["exc_amt"].ToString().Trim();


                            sg1_dr["sg1_f16"] = dt.Rows[i]["genum"].ToString().Trim();
                            sg1_dr["sg1_f17"] = dt.Rows[i]["gedate2"].ToString().Trim();
                            sg1_dr["sg1_f9"] = dt.Rows[i]["invno"].ToString().Trim();
                            sg1_dr["sg1_f10"] = dt.Rows[i]["invdate2"].ToString().Trim();
                            sg1_dr["sg1_f11"] = dt.Rows[i]["rcode"].ToString().Trim();
                            sg1_dr["sg1_f12"] = dt.Rows[i]["iname3"].ToString().Trim();
                            sg1_dr["sg1_f13"] = dt.Rows[i]["unit3"].ToString().Trim();
                            sg1_dr["sg1_f14"] = dt.Rows[i]["iqty_chl"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["desc_"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["rej_rw"].ToString().Trim();


                            //sg1_dr["sg1_t1"] = dt.Rows[i]["mcstart"].ToString().Trim();
                            //sg1_dr["sg1_t2"] = dt.Rows[i]["mcstop"].ToString().Trim();
                            //sg1_dr["sg1_t3"] = dt.Rows[i]["a1"].ToString().Trim();
                            //sg1_dr["sg1_t4"] = dt.Rows[i]["a2"].ToString().Trim();
                            //sg1_dr["sg1_t5"] = dt.Rows[i]["a3"].ToString().Trim();//cutsheet
                            //db3 += fgen.make_double(dt.Rows[i]["a3"].ToString().Trim());
                            //// txtlbl8.Text = txtlbl8.Text+dt.Rows[i]["a3"].ToString().Trim();
                            //sg1_dr["sg1_t6"] = dt.Rows[i]["A4"].ToString().Trim();
                            //sg1_dr["sg1_t7"] = dt.Rows[i]["remarks2"].ToString().Trim();
                            //sg1_dr["sg1_t8"] = dt.Rows[i]["a5"].ToString().Trim();
                            //db4 += fgen.make_double(dt.Rows[i]["a5"].ToString().Trim());
                            // txtlbl9.Text += dt.Rows[i]["a5"].ToString().Trim(); //oksheet
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        txtlbl8.Text = Convert.ToString(db3);
                        txtlbl9.Text = Convert.ToString(db4);
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
                    }
                    #endregion
                    break;

                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    //SQuery = "Select b.iname,b.cpartno,b.cdrgno,b.unit,trim(a.srno) as morder1,a.*,to_chaR(a.invdate,'dd/mm/yyyy') as pinvdate,to_chaR(a.vchdate,'dd/mm/yyyy') as pvchdate from ivoucher a,item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum||trim(a.icode)||trim(a.srno)='" + col1 + "' ORDER BY A.srno";
                    SQuery = "select  EMPCODE,NAME, DEPTT_TEXT,DESG_TEXT,DTJOIN from empmas  where BRANCHCD='" + frm_mbr + "' AND LENGTH(TRIM(LEAVING_DT))<5 AND grade='" + col1 + "' order by grade";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl4.Text = col1;
                        txtlbl4a.Text = col2;
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
                case "TYPE":
                    if (col1.Length <= 0) return;
                    txtlbl7a.Text = col2;
                    txtlbl7.Text = col3;
                    // txtlbl2.Focus();
                    break;
                case "MRESULT":
                    //if (col1.Length <= 0) return;
                    //txtlbl101.Text = col1;
                    //txtlbl101a.Text = col2;
                    break;
                case "PARTYCODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    //txtlbl101.Text = col3;
                    break;

                case "MACHNECODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    break;

                case "SG1_ROW_ADD":
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
                            sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                            sg1_dr["sg1_f15"] = sg1.Rows[i].Cells[13].Text.ToString();

                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[14].Text.ToString();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[15].Text.ToString();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[16].Text.ToString();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[17].Text.ToString();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[18].Text.ToString();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[19].Text.ToString();
                            sg1_dr["sg1_f7"] = sg1.Rows[i].Cells[20].Text.ToString();
                            sg1_dr["sg1_f8"] = sg1.Rows[i].Cells[21].Text.ToString();
                            sg1_dr["sg1_f16"] = sg1.Rows[i].Cells[23].Text.ToString();
                            sg1_dr["sg1_f17"] = sg1.Rows[i].Cells[24].Text.ToString();

                            sg1_dr["sg1_f9"] = sg1.Rows[i].Cells[25].Text.ToString();
                            sg1_dr["sg1_f10"] = sg1.Rows[i].Cells[26].Text.ToString();
                            sg1_dr["sg1_f11"] = sg1.Rows[i].Cells[27].Text.ToString();
                            sg1_dr["sg1_f12"] = sg1.Rows[i].Cells[28].Text.ToString();
                            sg1_dr["sg1_f13"] = sg1.Rows[i].Cells[29].Text.ToString();
                            sg1_dr["sg1_f14"] = sg1.Rows[i].Cells[30].Text.ToString();

                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        // SQuery = "select  a.invno as Job_No,a.invdate as Job_Dt,a.vchnum as Iss_no,a.vchdate as Iss_Dt,sum(a.iss) as iqtyout,sum(a.iss)-sum(a.taken) as Balance,a.icode,trim(b.iname) as iname,b.unit,b.cpartno  from (select trim(invno) as invno,to_char(invdate,'dd/mm/yyyy') as invdate,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,iqtyout as iss,0 as taken,icode from ivoucher where branchcd='" + frm_mbr + "' and type in('30','31','32') and substr(icode,1,2) in('01','02','03','07','10','81') and vchdate>=to_datE('01/04/2009','dd/mm/yyyy') union all select trim(job_no) as job_no,trim(job_Dt)As job_dt,trim(var_code) as var_code,trim(glue_code) as glue_code,0 as iss,a1 as taken,icode from prod_sheet where branchcd='" + frm_mbr + "' and type='85' and vchdate>=to_Date('01/04/2009','dd/mm/yyyy')) a,item b  where a.invno||a.invdate='" + col1 + "' and trim(a.icode)=trim(b.icode)  group by a.invno,a.invdate,a.vchnum,a.vchdate,a.icode,trim(b.iname),b.unit,b.cpartno  order by invno desc";
                        //SQuery = "select b.tout,b.treco,b.vchnum,b.vchdate,b.icode,b.unit,b.cgst,b.sgst,b.igst,c.iname from ( select a.fstr,max(a.out) as tout,max(a.reco) as treco,a.vchnum,a.vchdate,a.icode,a.unit,max(a.ctax) as cgst,max(a.stax) as sgst,max(a.ctax) as igst from (select branchcd||acode|| trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,iqtyout as out,0 as reco,trim(vchnum) as vchnum, to_char(vchdate,'dd/mm/yyyy') as vchdate, trim(icode) as icode,trim(unit) as unit,exc_amt  as ctax, cess_pu as stax from ivoucher where branchcd='" + frm_mbr + "' and type='21' and vchdate between to_date('01/07/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text + "' union all select branchcd||acode||trim(rgpnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,0 as out, iqtyout as reco,trim(rgpnum) as vchnum, to_char(rgpdate,'dd/mm/yyyy') as vchdate, trim(icode) as icode,trim(unit) as unit, 0  as ctax, 0 as stax from ivoucherw where branchcd='" + frm_mbr + "' and type='JX' and vchdate between to_date('01/07/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text + "' ) a group by a.fstr,a.vchnum,a.vchdate,a.icode,a.unit ) b ,item c where trim(b.icode)= trim(c.icode) AND b.tout > b.treco and  ";
                        SQuery = "select b.fstr,b.tout,b.treco,b.vchnum,b.vchdate,b.icode,b.unit,b.cgst,b.sgst,b.igst,c.iname from ( select a.fstr,max(a.out) as tout,max(a.reco) as treco,a.vchnum,a.vchdate,a.icode,a.unit,max(a.ctax) as cgst,max(a.stax) as sgst,max(a.itax) as igst from (select trim(branchcd)||trim(acode)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(icode) as fstr,iqtyout as out,0 as reco,trim(vchnum) as vchnum, to_char(vchdate,'dd/mm/yyyy') as vchdate, trim(icode) as icode,trim(unit) as unit,(case when post=1 then exc_amt  else 0  end ) as ctax,(case when post=2 then exc_amt  else 0  end ) as itax, (case when post=1 then cess_pu else 0  end ) as stax from ivoucher where branchcd='" + frm_mbr + "' and type='21' and vchdate between to_date('01/07/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text + "'  union all select trim(branchcd)||trim(acode)||trim(rgpnum)||to_char(rgpdate,'dd/mm/yyyy')||trim(icode) as fstr,0 as out, iqtyout as reco,trim(rgpnum) as vchnum, to_char(rgpdate,'dd/mm/yyyy') as vchdate, trim(icode) as icode,trim(unit) as unit, 0  as ctax,0 as itax, 0 as stax from ivoucherw where branchcd='" + frm_mbr + "' and type='JX' and vchdate between to_date('01/07/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and trim(acode)='" + txtlbl4.Text + "' ) a group by a.fstr,a.vchnum,a.vchdate,a.icode,a.unit) b ,item c where trim(b.icode)= trim(c.icode) AND b.tout > b.treco and b.fstr='" + col1 + "' ";
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
                            sg1_dr["sg1_f15"] = dt.Rows[d]["vchnum"].ToString().Trim();
                            sg1_dr["sg1_f1"] = dt.Rows[d]["vchdate"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[d]["icode"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[d]["iname"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[d]["unit"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[d]["tout"].ToString().Trim();
                            sg1_dr["sg1_f6"] = dt.Rows[d]["cgst"].ToString().Trim();

                            sg1_dr["sg1_f7"] = dt.Rows[d]["sgst"].ToString().Trim();
                            sg1_dr["sg1_f8"] = dt.Rows[d]["igst"].ToString().Trim();

                            sg1_dr["sg1_f16"] = "-";
                            sg1_dr["sg1_f17"] = "-";
                            sg1_dr["sg1_f9"] = "-";
                            sg1_dr["sg1_f10"] = "-";
                            sg1_dr["sg1_f11"] = "-";
                            sg1_dr["sg1_f12"] = "-";
                            sg1_dr["sg1_f13"] = "-";
                            sg1_dr["sg1_f14"] = "-";
                            //sg1_dr["sg1_h1"] = "-";
                            //sg1_dr["sg1_h2"] = "-";
                            //sg1_dr["sg1_h3"] = "-";
                            //sg1_dr["sg1_h4"] = "-";
                            //sg1_dr["sg1_h5"] = "-";
                            //sg1_dr["sg1_h6"] = "-";
                            //sg1_dr["sg1_h7"] = "-";
                            //sg1_dr["sg1_h8"] = "-";
                            //sg1_dr["sg1_h9"] = "-";
                            //sg1_dr["sg1_h10"] = "-";
                            //sg1_dr["sg1_f1"] = dt.Rows[d]["Job_Dt"].ToString().Trim();
                            //sg1_dr["sg1_f2"] = dt.Rows[d]["Job_No"].ToString().Trim();
                            //sg1_dr["sg1_f3"] = dt.Rows[d]["icode"].ToString().Trim();
                            //sg1_dr["sg1_f4"] = dt.Rows[d]["INAME"].ToString().Trim();
                            //sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();
                            //sg1_dr["sg1_f6"] = dt.Rows[d]["Balance"].ToString().Trim();
                            //sg1_dr["sg1_t1"] = "";
                            //sg1_dr["sg1_t2"] = "";
                            //sg1_dr["sg1_t3"] = dt.Rows[d]["Balance"].ToString().Trim();
                            //sg1_dr["sg1_t4"] = "";
                            //sg1_dr["sg1_t5"] = "";
                            //sg1_dr["sg1_t6"] = "";
                            //sg1_dr["sg1_t7"] = "";
                            //sg1_dr["sg1_t8"] = "";
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
                    break;

                case "SG1_ROW_ADD_E":
                    dt = new DataTable();
                    // SQuery = "select a.*,to_chaR(a.invdate,'dd/mm/yyyy') as pent_Dt from ivoucher a where a.invno||to_char(a.invdate,'dd/mm/yyyy')='" + col1 + "' and a.branchcd='" + frm_mbr + "' and a.type in('30','31')";
                    txtlbl101.Text = col1;
                    xStartDt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='R10'", "PARAMS");
                    if (txtlbl101.Text == "No")
                    {
                        SQuery = "select invno||invdate as fstr,invno as Job_No,invdate as Job_Dt,vchnum as Iss_no,vchdate as Iss_Dt,sum(iss)-sum(taken) as Balance,a.icode,trim(b.iname) as iname,b.unit,b.cpartno   from (select trim(invno) as invno,to_char(invdate,'dd/mm/yyyy') as invdate,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,iqtyout as iss,0 as taken,icode from ivoucher where branchcd='" + frm_mbr + "' and type in('30','31','32') and substr(icode,1,2) in('01','02','03','07','10','81') and vchdate>=to_datE('" + xStartDt + "','dd/mm/yyyy') union all select trim(job_no) as job_no,trim(job_Dt)As job_dt,trim(var_code) as var_code,trim(glue_code) as glue_code,0 as iss,a1 as taken,icode from prod_sheet where branchcd='" + frm_mbr + "' and type='85' and vchdate>=to_Date('" + xStartDt + "','dd/mm/yyyy'))a,item b where trim(a.icode)=trim(b.icode) and trim(a.invno)||trim(a.invdate)='" + col1 + "' group by invno,invdate,vchnum,vchdate,a.icode,trim(b.iname),b.unit,b.cpartno  order by invno desc";
                    }
                    if (txtlbl101.Text == "Yes")
                    {
                        SQuery = "select invno||invdate as fstr,invno as Job_No,invdate as Job_Dt,vchnum as Iss_no,vchdate as Iss_Dt,sum(iss)-sum(taken) as Balance,a.icode,trim(b.iname) as iname,b.unit,b.cpartno   from (select trim(invno) as invno,to_char(invdate,'dd/mm/yyyy') as invdate,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,iqtyout as iss,0 as taken,icode from ivoucher where branchcd='" + frm_mbr + "' and type in('30','31','32') and substr(icode,1,2) in('01','02','03','07','10','81') and vchdate>=to_datE('" + xStartDt + "','dd/mm/yyyy') union all select trim(job_no) as job_no,trim(job_Dt)As job_dt,trim(var_code) as var_code,trim(glue_code) as glue_code,0 as iss,a1 as taken,icode from prod_sheet where branchcd='" + frm_mbr + "' and type='85' and vchdate>=to_Date('" + xStartDt + "','dd/mm/yyyy'))a,item b  where trim(a.icode)=trim(b.icode) and trim(a.invno)||trim(a.invdate)='" + col1 + "' group by invno,invdate,vchnum,vchdate,a.icode,trim(b.iname),b.unit,b.cpartno having sum(iss)-sum(taken) >0 order by invno desc";
                    }
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (col1.Length <= 0) return;
                    for (int d = 0; d < dt.Rows.Count; d++)
                    {
                        //********* Saving in Hidden Field 
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = dt.Rows[d]["Job_Dt"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = dt.Rows[d]["Job_No"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = dt.Rows[d]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = dt.Rows[d]["INAME"].ToString().Trim();
                        //********* Saving in GridView Value
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = dt.Rows[d]["unit"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = dt.Rows[d]["Balance"].ToString().Trim();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = dt.Rows[d]["Balance"].ToString().Trim();
                    }
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD1":
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
                            sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                            sg1_dr["sg1_f15"] = sg1.Rows[i].Cells[13].Text.ToString();

                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[14].Text.ToString();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[15].Text.ToString();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[16].Text.ToString();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[17].Text.ToString();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[18].Text.ToString();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[19].Text.ToString();
                            sg1_dr["sg1_f7"] = sg1.Rows[i].Cells[20].Text.ToString();
                            sg1_dr["sg1_f8"] = sg1.Rows[i].Cells[21].Text.ToString();
                            sg1_dr["sg1_f16"] = sg1.Rows[i].Cells[23].Text.ToString();
                            sg1_dr["sg1_f17"] = sg1.Rows[i].Cells[24].Text.ToString();

                            sg1_dr["sg1_f9"] = sg1.Rows[i].Cells[25].Text.ToString();
                            sg1_dr["sg1_f10"] = sg1.Rows[i].Cells[26].Text.ToString();
                            sg1_dr["sg1_f11"] = sg1.Rows[i].Cells[27].Text.ToString();
                            sg1_dr["sg1_f12"] = sg1.Rows[i].Cells[28].Text.ToString();
                            sg1_dr["sg1_f13"] = sg1.Rows[i].Cells[29].Text.ToString();
                            sg1_dr["sg1_f14"] = sg1.Rows[i].Cells[30].Text.ToString();

                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        string stage = "0"; string stagename = "";
                        // hf1.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL13");
                        int RowInsertAt = Convert.ToInt32(hf1.Value);
                        stage = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text + sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text;


                        dt = new DataTable();
                        SQuery = "select trim(a.type)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr, trim(a.vchnum) as vchnum,trim(a.acode) as code,to_char(a.vchdate,'dd/mm/yyyy') as vchdate, trim(a.invno) as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate ,trim(a.icode) as icode,trim(b.iname) as iname,trim(a.unit) as unit,a.iqtyin from ivoucher a, item b where trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.type='09' and a.vchdate between to_date('01/07/2017','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy') and trim(a.type)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||TRIM(a.icode) in (" + col1 + ") ";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            if (d == 0)
                            {
                            }
                            else
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
                                sg1_dr["sg1_f15"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text.ToString().Trim();
                                sg1_dr["sg1_f1"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text.ToString().Trim();
                                sg1_dr["sg1_f2"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text.ToString().Trim();
                                sg1_dr["sg1_f3"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text.ToString().Trim();
                                sg1_dr["sg1_f4"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text.ToString().Trim();
                                sg1_dr["sg1_f5"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text.ToString().Trim();
                                sg1_dr["sg1_f6"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[19].Text.ToString().Trim();

                                sg1_dr["sg1_f7"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[20].Text.ToString().Trim();
                                sg1_dr["sg1_f8"] = sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[21].Text.ToString().Trim();


                                sg1_dr["sg1_f16"] = dt.Rows[d]["vchnum"].ToString().Trim();
                                sg1_dr["sg1_f17"] = dt.Rows[d]["vchdate"].ToString().Trim();
                                sg1_dr["sg1_f9"] = dt.Rows[d]["invno"].ToString().Trim();
                                sg1_dr["sg1_f10"] = dt.Rows[d]["invdate"].ToString().Trim();
                                sg1_dr["sg1_f11"] = dt.Rows[d]["icode"].ToString().Trim();
                                sg1_dr["sg1_f12"] = dt.Rows[d]["iname"].ToString().Trim();
                                sg1_dr["sg1_f13"] = dt.Rows[d]["unit"].ToString().Trim();
                                sg1_dr["sg1_f14"] = dt.Rows[d]["iqtyin"].ToString().Trim();
                                sg1_dt.Rows.InsertAt(sg1_dr, RowInsertAt);

                                //sg1_dt.Rows.Add(sg1_dr);
                            }
                            RowInsertAt++;
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    if (dt.Rows.Count > 0)
                    {
                        int d = 0;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[23].Text = dt.Rows[d]["vchnum"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[24].Text = dt.Rows[d]["vchdate"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[25].Text = dt.Rows[d]["invno"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[26].Text = dt.Rows[d]["invdate"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[27].Text = dt.Rows[d]["icode"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[28].Text = dt.Rows[d]["iname"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[29].Text = dt.Rows[d]["unit"].ToString().Trim();
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[30].Text = dt.Rows[d]["iqtyin"].ToString().Trim();
                    }
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    setColHeadings();
                    #endregion
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
                        dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        for (i = 0; i < dt.Rows.Count - 1; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = (i + 1);
                            //sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.Trim();
                            //sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.Trim();
                            //sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.Trim();
                            //sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.Trim();
                            //sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.Trim();
                            //sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.Trim();
                            //sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.Trim();
                            //sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.Trim();
                            //sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.Trim();
                            //sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.Trim();

                            //sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.Trim();
                            //sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim();
                            //sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                            //sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                            //sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();
                            //sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[18].Text.Trim();
                            //sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            //sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            //sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            //sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            //sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            //sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            //sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            //sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            ////sg1_dr["sg1_t9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim();
                            ////sg1_dr["sg1_t10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text.Trim();
                            ////sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                            ////sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                            ////sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                            ////sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                            ////sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                            ////sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();
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
                            // sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                            sg1_dr["sg1_f15"] = sg1.Rows[i].Cells[13].Text.ToString();

                            sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[14].Text.ToString();
                            sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[15].Text.ToString();
                            sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[16].Text.ToString();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[17].Text.ToString();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[18].Text.ToString();
                            sg1_dr["sg1_f6"] = sg1.Rows[i].Cells[19].Text.ToString();
                            sg1_dr["sg1_f7"] = sg1.Rows[i].Cells[20].Text.ToString();
                            sg1_dr["sg1_f8"] = sg1.Rows[i].Cells[21].Text.ToString();
                            sg1_dr["sg1_f16"] = sg1.Rows[i].Cells[23].Text.ToString();
                            sg1_dr["sg1_f17"] = sg1.Rows[i].Cells[24].Text.ToString();

                            sg1_dr["sg1_f9"] = sg1.Rows[i].Cells[25].Text.ToString();
                            sg1_dr["sg1_f10"] = sg1.Rows[i].Cells[26].Text.ToString();
                            sg1_dr["sg1_f11"] = sg1.Rows[i].Cells[27].Text.ToString();
                            sg1_dr["sg1_f12"] = sg1.Rows[i].Cells[28].Text.ToString();
                            sg1_dr["sg1_f13"] = sg1.Rows[i].Cells[29].Text.ToString();
                            sg1_dr["sg1_f14"] = sg1.Rows[i].Cells[30].Text.ToString();

                            sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                            sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        //if (edmode.Value == "Y")
                        //{
                        //    sg1_dr["sg1_f1"] = "*" + sg1.Rows[-1 + Convert.ToInt32(hf1.Value.Trim())].Cells[13].Text.Trim();

                        //    sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        //}
                        //else
                        //{
                        //    sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                        //}

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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = "JX";
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "SELECT TRIM(VCHNUM) AS VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ICODE) AS ICODE,TRIM(B.INAME) AS INAME ,TRIM(A.RCODE) AS ICODE2,TRIM(C.INAME) AS INAME_R, TO_CHAR(A.IQTYOUT,'999,999,999.99') AS QTY_OUT,A.EXC_AMT AS CGST,A.CESS_PU AS SGST,A.EXC_AMT AS IGST , A.INVNO AS JOB_WRK_CHALLAN_NO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS JOB_WRK_CHALLAN_DT,A.IQTY_CHL AS QTY_IN,A.DESC_ AS NATURE_OF_JOBWRK, A.REJ_RW AS WASTAGE_QTY FROM IVOUCHErW A, ITEM B, ITEM C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.RCODE)=TRIM(C.ICODE) AND  A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='JX' and A.vchdate " + PrdRange + " ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "ProdRep")
        {
            DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            SQuery = "select trim(b.mchname) as Machine,trim(a.Mchcode) as Code,count(a.icode) as items,to_char(sum(a.a3),'999,999,999,999.99') as sheet_rcvd,to_char(sum(a.a4),'999,999,999,999.99') as sheet_rej,to_char(sum(a.iqtyout),'999,999,999,999.99') as total_OK from prod_sheet a,(select mchname,trim(acode)||'/'||srno as fstr from pmaint where branchcd='" + frm_mbr + "' and type='10') b where a.branchcd='" + frm_mbr + "' and substr(a.type,1,2) in('85') and vchdate " + DateRange + "  and trim(a.mchcode)=b.fstr group by b.mchname,a.Mchcode";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Paper Cutting Production Record For the Period " + fromdt + " To " + todt, frm_qstr);
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
        sg1_dt.Columns.Add(new DataColumn("sg1_f7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f8", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f9", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f10", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f11", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f12", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f13", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f14", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f15", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f16", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_f17", typeof(string)));



        sg1_dt.Columns.Add(new DataColumn("sg1_t1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t3", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t4", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t5", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t6", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t7", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t8", typeof(string)));
    }

    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
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
        sg1_dr["sg1_f6"] = "-";
        sg1_dr["sg1_f7"] = "-";
        sg1_dr["sg1_f8"] = "-";

        sg1_dr["sg1_f9"] = "-";
        sg1_dr["sg1_f10"] = "-";
        sg1_dr["sg1_f11"] = "-";
        sg1_dr["sg1_f12"] = "-";
        sg1_dr["sg1_f13"] = "-";
        sg1_dr["sg1_f14"] = "-";


        sg1_dr["sg1_t1"] = "-";
        sg1_dr["sg1_t2"] = "-";
        sg1_dr["sg1_t3"] = "-";
        sg1_dr["sg1_t4"] = "-";
        sg1_dr["sg1_t5"] = "-";
        sg1_dr["sg1_t6"] = "-";
        sg1_dr["sg1_t7"] = "-";
        sg1_dr["sg1_t8"] = "-";
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
                    fgen.Fn_open_sseek("Select Option", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Challan of Type 21", frm_qstr);
                }
                break;

            case "SG1_ROW_ADD1":
                if (sg1.Rows[Convert.ToInt32(index)].Cells[13].Text.Trim().Length > 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    hffield.Value = "SG1_ROW_ADD1";
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select item", frm_qstr);
                }
                else
                {
                    fgen.msg("-", "AMSG", "Please Select Challan First!!");
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
        hffield.Value = "PARTYCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Party", frm_qstr);
    }

    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TYPE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Result", frm_qstr);
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
        hffield.Value = "MACHNECODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr);
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
            //if (sg1.Rows[i].Cells[15].Text.Trim().Length > 1)
            //{
            oporow = oDS.Tables[0].NewRow();
            oporow["BRANCHCD"] = frm_mbr;
            oporow["TYPE"] = frm_vty;
            oporow["vchnum"] = frm_vnum;
            oporow["vchdate"] = txtvchdate.Text.Trim();

            oporow["SRNO"] = i + 1;

            oporow["acode"] = txtlbl4.Text.Trim();
            //save data into the ivucherq table of type=JX
            oporow["rgpnum"] = sg1.Rows[i].Cells[13].Text.Trim();
            oporow["rgpdate"] = sg1.Rows[i].Cells[14].Text.Trim();
            oporow["icode"] = sg1.Rows[i].Cells[15].Text.Trim();
            oporow["iqtyout"] = sg1.Rows[i].Cells[18].Text.Trim();
            oporow["iqtyin"] = "0";
            oporow["iqty_ok"] = "0";
            oporow["exc_amt"] = fgen.make_double(sg1.Rows[i].Cells[19].Text.Trim());
            oporow["cess_pu"] = fgen.make_double(sg1.Rows[i].Cells[20].Text.Trim());
            oporow["exc_amt"] = fgen.make_double(sg1.Rows[i].Cells[21].Text.Trim());

            oporow["genum"] = sg1.Rows[i].Cells[23].Text.Trim();
            oporow["gedate"] = sg1.Rows[i].Cells[24].Text.Trim();
            oporow["invno"] = sg1.Rows[i].Cells[25].Text.Trim();
            oporow["invdate"] = sg1.Rows[i].Cells[26].Text.Trim();
            oporow["rcode"] = sg1.Rows[i].Cells[27].Text.Trim();

            oporow["iqty_chl"] = fgen.make_double(sg1.Rows[i].Cells[30].Text.Trim());
            oporow["desc_"] = ((TextBox)(sg1.Rows[i].FindControl("sg1_t4"))).Text.Trim();
            oporow["rej_rw"] = fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t5"))).Text.Trim());
            oporow["naration"] = txtrmk.Text.Trim().ToUpper();


            //oporow["icode"] = sg1.Rows[i].Cells[15].Text.Trim();
            //oporow["acode"] = "-";
            //oporow["a1"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim());
            //oporow["a2"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim());
            //double q = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim()) * fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim());
            //oporow["a4"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim());
            //double q1 = q - fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim());
            //oporow["a5"] = q1;
            //oporow["a3"] = q;
            //oporow["a6"] = 0;
            //oporow["a7"] = 0;
            //oporow["total"] = 0;
            //oporow["un_melt"] = 0;
            //oporow["mlt_loss"] = 0;
            //oporow["a8"] = sg1.Rows[i].Cells[18].Text.Trim();
            //oporow["flag"] = "O";
            //oporow["subcode"] = "-";
            //oporow["prevstage"] = "-";
            //oporow["empcode"] = "-";
            //oporow["a9"] = 0;
            //oporow["a10"] = 0;
            //oporow["a11"] = 0;
            //oporow["a12"] = 0;
            //oporow["lmd"] = 0;
            //oporow["bcd"] = 0;
            //oporow["var_code"] = "-";
            //oporow["glue_code"] = txtvchdate.Text.Trim();
            //oporow["film_code"] = "-";
            //oporow["naration"] = "-";
            //oporow["num1"] = 0;
            //oporow["num2"] = 0;
            //oporow["num3"] = 0;
            //oporow["num4"] = 0;
            //oporow["num5"] = 0;
            //oporow["num6"] = 0;
            //oporow["num7"] = 0;
            //oporow["num8"] = 0;
            //oporow["num9"] = 0;
            //oporow["num10"] = 0;
            //oporow["num11"] = 0;
            //oporow["num12"] = 0;
            //oporow["mtime"] = "-";
            //oporow["exc_time"] = "-";
            //oporow["tempr"] = "-";
            //oporow["irate"] = 0;
            //oporow["mseq"] = 0;
            //oporow["a13"] = 0;
            //oporow["a14"] = 0;
            //oporow["a15"] = 0;
            //oporow["a16"] = 0;
            //oporow["a17"] = 0;
            //oporow["a18"] = 0;
            //oporow["a19"] = 0;
            //oporow["a20"] = 0;
            //oporow["fm_fact"] = 1;
            //oporow["pcpshot"] = 1;
            //oporow["pbtchno"] = "-";
            //oporow["opr_dtl"] = "-";
            //oporow["oee_r"] = 0;
            //oporow["hcut"] = 0;
            //oporow["alsttim"] = 0;
            //oporow["altctim"] = 0;
            //oporow["cust_ref"] = "-";
            //oporow["cell_ref"] = "-";
            //oporow["cell_refn"] = "-";
            //oporow["a21"] = 0;
            //oporow["a22"] = 0;
            //oporow["a23"] = 0;
            //oporow["a24"] = 0;
            //oporow["a25"] = 0;
            //oporow["a26"] = 0;
            //oporow["a27"] = 0;
            //oporow["a28"] = 0;
            //oporow["a29"] = 0;
            //oporow["a30"] = 0;
            //oporow["stage"] = "01";
            //oporow["dcode"] = "-";
            //oporow["iqtyin"] = "0";
            //oporow["iqtyout"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim());
            //oporow["mchcode"] = txtlbl7.Text.Trim();
            //oporow["ename"] = txtlbl7a.Text.Trim().ToUpper();
            //oporow["prevcode"] = txtlbl4.Text.Trim();
            ////oporow["shftcode"] = txtlbl4.Text.Trim();
            ////oporow["shftcode"] = "-";
            //oporow["PARTYCODE"] = "-";
            //oporow["noups"] = i + 1;
            //oporow["job_no"] = sg1.Rows[i].Cells[14].Text.Trim();
            //oporow["job_dt"] = sg1.Rows[i].Cells[13].Text.Trim();
            //oporow["mcstart"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
            //oporow["mcstop"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
            //oporow["TSLOT"] = Convert.ToDateTime("01/01/2010 " + ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim()) - Convert.ToDateTime("01/01/2010 " + ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim());
            //oporow["wo_no"] = "-";
            //oporow["wo_dt"] = txtvchdate.Text.Trim();
            //oporow["Remarks2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper();

            //// add rejection columns

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
            // }
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
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "JX");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }

    protected void txt_TextChanged(object sender, EventArgs e)
    {
        string dttoh = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text;
        string dttom = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text;
        string dtfromh = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text;
        string dtfromm = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text;


        DateTime dtFrom = DateTime.Parse(dtfromh + ":" + dtfromm);
        DateTime dtTo = DateTime.Parse(dttoh + ":" + dttom);

        int timeDiff = dtFrom.Subtract(dtTo).Hours;
        int timediff2 = dtFrom.Subtract(dtTo).Minutes;


        TextBox txtName = ((TextBox)sg1.Rows[i].FindControl("sg1_t5"));
        txtName.Text = timeDiff.ToString();

        TextBox txtName1 = ((TextBox)sg1.Rows[i].FindControl("sg1_t6"));
        txtName1.Text = timediff2.ToString();
    }
    //------------------------------------------------------------------------------------
    protected void btnProdReport_Click(object sender, EventArgs e)
    {
        hffield.Value = "ProdRep";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
}