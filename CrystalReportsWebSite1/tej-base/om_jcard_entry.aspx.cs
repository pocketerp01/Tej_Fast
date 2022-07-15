using System;
using System.IO;
using System.Data;
using System.Web;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_jcard_entry : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y", xprdrange1, mq2;
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    double JobQty = 0;
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2, ind_Ptype;
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
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    ind_Ptype = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IND_PTYPE");
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
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
                ((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t16")).Attributes.Add("readonly", "readonly");
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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F35101":
                tab2.Visible = false;
                tab5.Visible = false;
                tab4.Visible = false;
                break;
        }
        if (Prg_Id == "M12008")
        {
            tab5.Visible = true;
            txtlbl8.Attributes.Remove("readonly");
            txtlbl9.Attributes.Remove("readonly");
        }
        lblheader.Text = "Job Order Creation";
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false;
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
        btnprint.Disabled = true;
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
        frm_tabname = "COSTESTIMATE";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "30");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        typePopup = "N";

        if (ind_Ptype == "12" || ind_Ptype == "13")
            td_lab.Visible = true;
        else td_lab.Visible = false;

        btnlist.Visible = false;
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

                SQuery = "Select distinct a.icode as fstr,a.DESC9 as ciname,a.icode,a.cpartno,a.branchcd||a.type||a.ordno||to_char(a.orddt,'dd/mm/yyyy')||a.srno as solink,a.Qtyord,a.Soldqty as QTY_OUT,a.Bal_Qty as BAL from wbvu_pending_so A where a.branchcd='" + frm_mbr + "' order by a.DESC9 ";
                break;
            case "TICODE":
                //pop2

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
                    col1 = " and TRIM(icode) not in (" + col1 + ")";
                }

                else
                {
                    col1 = "";
                }
                SQuery = "SELECT Icode AS FSTR,Iname AS Item_Name,Cpartno,Cdrgno,unit,ent_by,Icode FROM Item where branchcd!='DD' and length(Trim(deac_by))<2  and length(Trim(icode))>4 " + col1 + " ORDER BY Iname ";
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
            case "PJOBS":
                SQuery = "select distinct trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,trim(A.vchnum) as vchnum ,to_Char(A.vchdate,'dd/MM/yyyy') as Dated,A.type,trim(B.INAME) as Item_Name,A.QTY as Qty,B.CPARTNO,a.col25 as RefNo,to_char(a.vchdate,'yyyymmdd') as vdd from costestimate A,ITEM B  WHERE a.vchdate " + DateRange + " and A.SRNO=0 AND trim(A.ICODE)=trim(B.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='30' and trim(A.icode)='" + txtlbl4.Text.Trim() + "' order by vdd desc ,trim(A.vchnum) desc";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,a.vchnum||'  '||decode(trim(nvl(a.app_by,'-')),'-','(Un Approved)','(Approved)') AS jobno,to_Char(A.vchdate,'dd/mm/yyyy') as Dated,A.type,B.INAME as Item_Name,A.QTY as Qty,a.app_by,to_char(a.app_dt,'dd/mm/yyyy') as app_dt,a.col24,A.ENT_BY,TO_CHAR(A.ENT_DT,'DD/MM/YYYY') AS ENT_DT,b.cdrgno,b.cpartno,a.icode,a.vchnum AS Vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.CONVDATE as oref,a.col25 as splno,to_char(a.vchdate,'yyyymmdd') as vdd from costestimate A,ITEM B  WHERE trim(A.ICODE)=trim(B.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='30' and A.vchnum<>'000000' and a.vchdate " + DateRange + " /*and A.SRNO=0*/ order by vdd desc,vchnum desc";
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

        string chkr01 = "", xprd1 = "";
        SQuery = "";
        chkr01 = fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='R01'", "params");
        xprd1 = "between to_date('" + chkr01 + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
        SQuery = "CREATE OR REPLACE FORCE VIEW PENDING_SO_VU" + frm_mbr + " as (select branchcd,type,max(Closed) as closed,max(ciname) as ciname,max(cpartno) as cpartno,max(pordno) as pordno,max(porddt) porddt,acode,icode,ordno,orddt,sum(qtyord) as qtyord,sum(sale) as qty_out,sum(qtyord)-sum(sale) as bal from (select branchcd, type,cu_chldt,icat as Closed,ciname,cpartno,pordno,porddt,acode,icode,ordno,orddt,qtyord,0 as sale from somas where branchcd='" + frm_mbr + "' and type like '4%' and orddt " + xprd1 + "  union all select branchcd ,type,null as cu_chldt,null as icat,null as ciname,null as cpartno,null as pordno,null as porddt,acode,icode,ponum,podate,0 as qtyord,iqtyout as sale from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + xprd1 + ")group by BRANCHCD,type,acode,icode,ordno,orddt)";
        fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);


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
            typePopup = "N";
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
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        //checks
        //-----------------------------------------------------------------------
        if (dhd == 0)
        {
            fgen.msg("-", "AMSG", "Please Select a Valid Date");
            txtvchdate.Focus();
            return;
        }

        if (txtlbl7.Text == "-" || txtlbl7.Text == "")
        {
            fgen.msg("-", "AMSG", "Please Fill Job Card Qty");
            return;
        }

        if (txtlbl4.Text == "-" || txtlbl4.Text == "")
        {
            fgen.msg("-", "AMSG", "Please Select Item");
            return;
        }

        for (int i = 0; i < sg1.Rows.Count; i++)
        {
            if (((TextBox)(sg1.Rows[i].FindControl("sg1_t5"))).Text.Trim() == "-" || ((TextBox)(sg1.Rows[i].FindControl("sg1_t5"))).Text.Trim() == "")
            {
                fgen.msg("-", "AMSG", "Please Put Atleast One Qty In Delivery Dates Grid");
                return;
            }
            JobQty += fgen.make_double(((TextBox)(sg1.Rows[i].FindControl("sg1_t5"))).Text.Trim());
        }
        if (JobQty != fgen.make_double(txtlbl7.Text))
        {
            fgen.msg("-", "AMSG", "Job Card Qty Is Not matching With This Job Card Qty Of The Delivery Dated Grid");
            return;
        }
        //string mandField = "";
        //mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        //if (mandField.Length > 1)
        //{
        //    fgen.msg("-", "AMSG", mandField);
        //    return;
        //}
        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        btnsave.Disabled = true;
    }
    //------------------------------------------------------------------------------------
    string checkGridQty()
    {
        DataTable dtQty = new DataTable();
        dtQty.Columns.Add(new DataColumn("icode", typeof(string)));
        dtQty.Columns.Add(new DataColumn("qty", typeof(double)));
        DataRow drQty = null;
        foreach (GridViewRow gr in sg1.Rows)
        {
            drQty = dtQty.NewRow();
            drQty["icode"] = gr.Cells[13].Text.ToString().Trim();
            drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t1")).Text.ToString().Trim());
            dtQty.Rows.Add(drQty);
        }
        object sm;

        DataView distQty = new DataView(dtQty, "", "icode", DataViewRowState.CurrentRows);
        DataTable dtQty1 = new DataTable();
        dtQty1 = distQty.ToTable(true, "icode");
        foreach (DataRow drQty1 in dtQty1.Rows)
        {
            sm = dtQty.Compute("sum(qty)", "icode='" + drQty1["icode"].ToString().Trim() + "'");
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
        lblPPQty.Text = "";
        lblstdWstg.Text = "";
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
        fgen.Fn_open_mseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        vty = "30";
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl2.Text = frm_uname;
        txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        txtlbl5.Text = "-";
        //txtlbl6.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();

        sg1_dt = new DataTable();
        create_tab();
        sg1_add_blankrows();

        txtlbl6.Text = "N";
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
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").Substring(0, 6), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").Substring(0, 6) + "");
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
                    //SQuery = "Select a.*,b.name,c.iname,c.cpartno as icpartno,c.cdrgno as icdrgno,c.unit as iunit,to_char(a.ent_Dt,'dd/mm/yyyy') as pent_dt,to_char(a.chk_Dt,'dd/mm/yyyy') as chkd_dt,to_char(a.app_Dt,'dd/mm/yyyy') as papp_dt from " + frm_tabname + " a,type b,item c where trim(a.acode)=trim(b.type1) and b.id='M' and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    SQuery = "SELECT a.* FROM " + frm_tabname + " a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' order by a.srno";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    string tiname = "";
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtlbl7a.Text = dt.Rows[0]["CONVDATE"].ToString().Trim();
                        doc_addl.Value = dt.Rows[0]["acode"].ToString().Trim();
                        txtlbl4.Text = dt.Rows[0]["Icode"].ToString().Trim();



                        tiname = fgen.seek_iname(frm_qstr, frm_cocd, "select iname from item where trim(icode)='" + dt.Rows[0]["Icode"].ToString().Trim() + "'", "iname");
                        txtlbl4a.Text = tiname;
                        txtlbl7.Text = dt.Rows[0]["QTY"].ToString().Trim();
                        txtUPS.Text = dt.Rows[0]["col13"].ToString().Trim();
                        txtlbl8.Text = dt.Rows[0]["col14"].ToString().Trim();
                        //txtlbl9.Text = dt.Rows[0]["col15"].ToString().Trim();
                        txtStdWstg.Text = dt.Rows[0]["col15"].ToString().Trim();
                        txtlbl5.Text = dt.Rows[0]["col16"].ToString().Trim();
                        lblPPQty.Text = dt.Rows[0]["col17"].ToString().Trim();
                        txtLen.Text = dt.Rows[0]["col18"].ToString().Trim();
                        txtwid.Text = dt.Rows[0]["col19"].ToString().Trim();
                        lblPPQty.Text = dt.Rows[0]["enr1"].ToString().Trim();
                        //lblPPQty.Text = dt.Rows[0]["enr2"].ToString().Trim();
                        lblstdWstg.Text = dt.Rows[0]["COL22"].ToString().Trim();
                        txtMktRmk.Text = dt.Rows[0]["COL12"].ToString().Trim();

                        txtlbl6.Text = dt.Rows[0]["jstatus"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[0]["ent_by"].ToString().Trim();
                        txtlbl3.Text = Convert.ToDateTime(dt.Rows[0]["ent_dt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtDirectRmk.Text = dt.Rows[0]["col25"].ToString().Trim();
                        txtMatlRmk.Text = dt.Rows[0]["comments"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["remarks"].ToString().Trim();
                        txtlbl9.Text = "0";

                        Label6.Text = fgen.seek_iname(frm_qstr, frm_cocd, "Select round((qtysupp/qtyord)*100,2) as qtysupp from somas where branchcd||type||ordno||to_Char(orddt,'dd/mm/yyyy')='" + txtlbl7a.Text.Trim().Substring(0, 20) + "' and trim(icode)='" + txtlbl4.Text.Trim() + "' and qtyord>0", "qtysupp");
                        create_tab2();
                        sg2_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg2_dr = sg2_dt.NewRow();
                            sg2_dr["sg2_srno"] = sg2_dt.Rows.Count + 1;

                            //sg2_dr["sg2_f1"] = dt.Rows[i]["col12"].ToString().Trim(); // showing wrong value
                            //sg2_dr["sg2_f2"] = dt.Rows[i]["col13"].ToString().Trim(); // showing wrong value

                            sg2_dr["sg2_f1"] = dt.Rows[i]["col2"].ToString().Trim();
                            sg2_dr["sg2_f2"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg2_dr["sg2_t1"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg2_dr["sg2_t2"] = dt.Rows[i]["col5"].ToString().Trim();
                            sg2_dr["sg2_t3"] = dt.Rows[i]["col6"].ToString().Trim();
                            sg2_dr["sg2_t4"] = dt.Rows[i]["col7"].ToString().Trim();
                            sg2_dr["sg2_t5"] = dt.Rows[i]["col8"].ToString().Trim();
                            //sg1_dr["sg2_t6"] = dt.Rows[i]["delv_item"].ToString().Trim();
                            sg2_dr["sg2_t7"] = dt.Rows[i]["col9"].ToString().Trim();
                            sg2_dr["sg2_t8"] = dt.Rows[i]["col10"].ToString().Trim();
                            sg2_dr["sg2_t9"] = dt.Rows[i]["col20"].ToString().Trim();
                            sg2_dr["sg2_t10"] = dt.Rows[i]["col21"].ToString().Trim();
                            sg2_dt.Rows.Add(sg2_dr);
                        }
                        create_tab();
                        sg1_dr = sg1_dt.NewRow();
                        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                        sg1_dr["sg1_f1"] = txtlbl4.Text;
                        sg1_dr["sg1_f2"] = tiname;

                        //sg1_dr["sg1_t1"] = dt.Rows[0]["comments"].ToString().Trim();
                        //sg1_dr["sg1_t2"] = dt.Rows[0]["QTY"].ToString().Trim();

                        sg1_dr["sg1_srno"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT SRNO FROM BUDGMST WHERE ICODE='" + txtlbl4.Text + "' AND SOLINK='" + txtlbl7a.Text.Substring(0, 20) + "'", "SRNO");
                        sg1_dr["sg1_t1"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TO_CHAR(DLV_DATE,'DD/MM/YYYY') AS DEL_DATE FROM BUDGMST WHERE ICODE='" + txtlbl4.Text + "' AND SOLINK='" + txtlbl7a.Text.Substring(0, 20) + "'", "DEL_DATE");
                        string RemarkByMktg = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TRIM(NVL(SOREMARKS,'-')) AS DESC_ FROM BUDGMST WHERE ICODE='" + txtlbl4.Text + "' AND SOLINK='" + txtlbl7a.Text.Substring(0, 20) + "'", "DESC_");
                        if (RemarkByMktg.Length == 1)
                        {
                            sg1_dr["sg1_t2"] = "-"; // SEEK_INAME RETURN 0 WHEN THERE IS NO DATA SO FOR AVOIDING 0 IN REMARKS COLUMN THIS IS DONE
                        }
                        else
                        {
                            sg1_dr["sg1_t2"] = RemarkByMktg;
                        }
                        sg1_dr["sg1_t3"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT BUDGETCOST FROM BUDGMST WHERE ICODE='" + txtlbl4.Text + "' AND SOLINK='" + txtlbl7a.Text.Substring(0, 20) + "'", "BUDGETCOST");
                        sg1_dr["sg1_t4"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACTUALCOST FROM BUDGMST WHERE ICODE='" + txtlbl4.Text + "' AND SOLINK='" + txtlbl7a.Text.Substring(0, 20) + "'", "ACTUALCOST");
                        sg1_dr["sg1_t5"] = dt.Rows[0]["QTY"].ToString().Trim();
                        sg1_dr["sg1_t6"] = "-";
                        sg1_dt.Rows.Add(sg1_dr);
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        ViewState["sg2"] = sg2_dt;
                        sg2.DataSource = sg2_dt;
                        sg2.DataBind();
                        dt.Dispose(); sg2_dt.Dispose();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                    }
                    // btnCalc_ServerClick("", EventArgs.Empty); // because of this it is showing wrong values
                    #endregion
                    break;

                case "Print_E":
                case "PJOBS":
                    if (col1.Length < 2) return;
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F35101");
                    fgen.fin_prod_reps(frm_qstr);
                    //fgen.fin_ppc_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    SQuery = "select distinct b.iname,a.icode,a.rejqty,a.maintdt,a.recalib from inspmst a,item b  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) and a.branchcd='" + frm_mbr + "' and a.type='70' and trim(a.icode)='" + col3 + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl7.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6");
                        txtlbl7a.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5");

                        doc_addl.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(A.ACODE) As Acode from SOMAS A where a.branchcd||a.type||a.ordno||to_char(a.orddt,'dd/mm/yyyy')||a.srno=upper(Trim('" + txtlbl7a.Text + "'))", "ACODE");
                        if (doc_addl.Value.Length < 3)
                        {
                            fgen.msg("-", "AMSG", "Please Select Sales Order again!!");
                            return;
                        }

                        txtlbl4.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtUPS.Text = dt.Rows[0]["rejqty"].ToString().Trim();

                        txtMktRmk.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(A.DESC_) As Acode from SOMAS A where a.branchcd||a.type||a.ordno||to_char(a.orddt,'dd/mm/yyyy')||a.srno||TRIM(A.ICODE)=upper(Trim('" + txtlbl7a.Text + txtlbl4.Text.Trim() + "'))", "ACODE");

                        txtlbl9.Text = fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[0]["icode"].ToString().Trim(), txtvchdate.Text, true, "closing_stk", "");
                        txtlbl5.Text = dt.Rows[0]["recalib"].ToString().Trim();
                        sg1_dt = new DataTable();
                        create_tab();
                        sg1_dr = sg1_dt.NewRow();

                        sg1_dr["sg1_f1"] = txtlbl4.Text.Trim();
                        sg1_dr["sg1_f2"] = txtlbl4a.Text.Trim();
                        sg1_dr["sg1_t5"] = txtlbl7.Text.Trim();

                        string get_Dtl = "";
                        get_Dtl = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT SRNO||'@'||TO_CHAR(DLV_DATE,'DD/MM/YYYY')||'@'||TRIM(NVL(SOREMARKS,'-'))||'@'||BUDGETCOST||'@'||ACTUALCOST AS FSTR FROM BUDGMST WHERE ICODE='" + txtlbl4.Text + "' AND SOLINK='" + txtlbl7a.Text.Substring(0, 20) + "'", "FSTR");
                        if (get_Dtl.Contains("@"))
                        {
                            sg1_dr["sg1_srno"] = get_Dtl.Split('@')[0].ToString().toDouble();
                            sg1_dr["sg1_t1"] = get_Dtl.Split('@')[1].ToString();
                            sg1_dr["sg1_t2"] = get_Dtl.Split('@')[2].ToString();
                            sg1_dr["sg1_t3"] = get_Dtl.Split('@')[3].ToString();
                            sg1_dr["sg1_t4"] = get_Dtl.Split('@')[4].ToString();
                        }
                        else
                        {
                            get_Dtl = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT SRNO||'@'||TO_CHAR(DEL_DATE,'DD/MM/YYYY')||'@'||TRIM(NVL(desc_,'-'))||'@'||QTYORD||'@'||QTYORD AS FSTR FROM SOMAS WHERE ICODE='" + txtlbl4.Text + "' AND BRANCHCD||TYPE||TRIM(ORDNO)||TO_CHAR(ORDDT,'DD/MM/YYYY')='" + txtlbl7a.Text.Substring(0, 20) + "'", "FSTR");
                            if (get_Dtl.Contains("@"))
                            {
                                sg1_dr["sg1_srno"] = get_Dtl.Split('@')[0].ToString().toDouble();
                                sg1_dr["sg1_t1"] = get_Dtl.Split('@')[1].ToString();
                                sg1_dr["sg1_t2"] = get_Dtl.Split('@')[2].ToString();
                                sg1_dr["sg1_t3"] = get_Dtl.Split('@')[3].ToString();
                                sg1_dr["sg1_t4"] = get_Dtl.Split('@')[4].ToString();
                            }
                        }


                        sg1_dr["sg1_t6"] = "-";
                        sg1_dt.Rows.Add(sg1_dr);
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        txtlbl7.Focus();
                        setColHeadings();

                        if (ind_Ptype == "12" || ind_Ptype == "13")
                        {
                            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select col14||'~'||col15 as gstr from inspmst where ROWNUM<2 AND trim(icode)='" + txtlbl4.Text.Trim() + "' and type='70' and branchcd<>'DD'", "gstr");
                            if (col1 != "0")
                            {
                                txtnoaccross.Text = col1.Split('~')[1];
                                txtnoofaround.Text = col1.Split('~')[0];

                                txtnoofups.Text = (txtnoaccross.Text.toDouble() * txtnoofaround.Text.toDouble()).ToString();
                            }
                        }
                    }
                    else
                    {
                        btnlbl4.Focus();
                        fgen.msg("-", "AMSG", "Process Plan Not Found For This Item " + col2 + "'13'" + col1);
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        if (col1.Length > 8) SQuery = "select * from item where trim(icode) in (" + col1 + ")";
                        else SQuery = "select * from item where trim(icode)='" + col1 + "'";
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

                            sg1_dt.Rows.Add(sg1_dr);
                        }
                    }
                    sg1_add_blankrows();

                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    dt.Dispose(); sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
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

                case "SG2_ROW_ADD_E":
                    mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                    if (mq2.Length <= 0) return;
                    string[] icode;
                    icode = mq2.Split(',');
                    //********* Saving in Hidden Field 
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    //********* Saving in GridView Value
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                    int nextRowIndex = 0;
                    nextRowIndex = Convert.ToInt32(hf1.Value);
                    for (int i = 0; i < icode.Length; i++)
                    {
                        sg2.Rows[nextRowIndex].Cells[14].Text = icode[i].ToString().Trim().Replace("'", "");
                        col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ICODE FROM ITEM WHERE UPPER(trim(INAME)) like '%" + icode[i].ToString().Trim().Replace("'", "") + "%'", "icode").Trim();

                        ((TextBox)sg2.Rows[nextRowIndex].FindControl("sg2_t7")).Text = col1;

                        dt2 = new DataTable();
                        DateRange = "BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + frm_CDT2 + "','DD/MM/YYYY')-1";
                        xprdrange1 = "BETWEEN TO_DATE('" + fromdt + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                        mq2 = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' and trim(a.icode)='" + col1 + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store='Y' and trim(icode)='" + col1 + "' GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + DateRange + " and store='Y' and trim(icode)='" + col1 + "' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE";
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);
                        if (dt2.Rows.Count > 0)
                            ((TextBox)sg2.Rows[nextRowIndex].FindControl("sg2_t5")).Text = dt2.Rows[0]["cl"].ToString();

                        nextRowIndex++;
                    }
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
            //SQuery = "select a.ordno as PR_no,to_char(a.vchdate,'dd/mm/yyyy') as PR_Dt,b.Name as Deptt_Name,c.Iname as Item_Name,C.Cpartno,a.delv_item as Reqd_by,a.qtyord as PR_Qty,c.Unit,a.Desc_ as Remarks,a.splrmk as End_use,a.doc_thr as Item_Make,a.Prate as Approx_rt,a.Icode,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.app_by,(Case when length(trim(nvl(a.app_by,'-')))<=1 then '-' else to_char(a.app_Dt,'dd/mm/yyyy') end) as app_dt,to_Char(a.vchdate,'yyyymmdd') as vdd,a.srno from " + frm_tabname + " a,type b,item c where trim(A.acode)=trim(B.type1) and b.id='M' and trim(A.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + " order by vdd ,a.ordno ,a.srno";
            SQuery = "select  distinct a.vchnum as job_no,to_char(a.vchdate,'dd/mm/yyyy') as job_dt,a.icode as item_Code,i.iname as item_name,a.acode as cust_code,f.aname as customer_name,a.qty,to_char(a.vchdate,'yyyymmdd') as vdd from costestimate a,item i, famst f where  trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + " order by vdd desc,job_no desc";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else if (hffield.Value == "REELWIP")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "";
            SQuery = "select trim(B.iname) as Item_name,trim(a.icode) as ERP_Code,sum(a.iqtyin) as Rcvd,sum(a.iqtyout) as Issued,sum(a.iqtyin)-sum(a.iqtyout) as balance,trim(a.kclreelno) as ReelNO,b.ciname,b.Cpartno as Part_no,max(a.coreelno) as co_Reelno,b.Unit from (SELECT ICODE,reelwout AS IQTYIN,0 AS IQTYOUT,kclreelno,coreelno FROM reelvch WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('31','32') AND trim(SUBSTR(ICODE,1,2))='07' and vchdate  " + PrdRange + " UNION ALL SELECT ICODE,0 AS IQTYIN,itate as IQTYOUT,col6,null as coreelno FROM costestimate WHERE branchcd='" + frm_mbr + "' and type='25' and vchdate " + PrdRange + " ) a,item b where substr(a.icode,1,2) in ('07') and trim(a.icode)=trim(B.icode) group by b.iname,b.cpartno,b.ciname,b.unit,trim(a.icode),trim(a.kclreelno) having sum(a.iqtyin)-sum(a.iqtyout)>0 order by b.iname";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("Reel In WIP", frm_qstr);
        }
        else
        {
            Checked_ok = "Y";
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
                        //oDS = new DataSet();
                        //oporow = null;
                        //oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        // This is for checking that, is it ready to save the data
                        frm_vnum = "000000";

                        //save_fun();                        
                        save_fun2();

                        oDS2.Dispose();
                        oporow2 = null;
                        oDS2 = new DataSet();
                        oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            frm_vnum = txtvchnum.Text.Trim();
                            save_it = "Y";
                        }
                        else
                        {
                            save_it = "N";
                            for (i = 0; i < sg2.Rows.Count - 0; i++)
                            {
                                if (sg2.Rows[i].Cells[13].Text.Trim().Length > 2)
                                {
                                    save_it = "Y";
                                }
                            }

                            if (save_it == "Y")
                            {
                                i = 0;
                                do
                                {
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "vch");
                                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                    if (i > 20)
                                    {
                                        fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and vchdate " + DateRange + " ", 6, "vch");
                                        pk_error = "N";
                                        i = 0;
                                    }
                                    i++;
                                }
                                while (pk_error == "Y");
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        //save_fun();
                        save_fun2();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        //fgen.save_data(frm_qstr, frm_cocd, oDS, "PROD_PLAN");
                        fgen.save_data(frm_qstr, frm_cocd, oDS2, frm_tabname);
                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                            cmd_query = "delete from PROD_PLAN where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
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
    //------------------------------------------------------------------------------------
    public void create_tab2()
    {
        sg2_dt = new DataTable();
        sg2_dr = null;
        // Hidden Field
        sg2_dt.Columns.Add(new DataColumn("sg2_SrNo", typeof(Int32)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h5", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h6", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h7", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h8", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h9", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_h10", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f1", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f2", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f3", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f4", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_f5", typeof(string)));
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
        sg2_dt.Columns.Add(new DataColumn("sg2_t11", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t12", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t13", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t14", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t15", typeof(string)));
        sg2_dt.Columns.Add(new DataColumn("sg2_t16", typeof(string)));
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
        sg1_dt.Rows.Add(sg1_dr);
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
                if (index > 0)
                {
                    if (index < sg2.Rows.Count)
                    {
                        hf1.Value = index.ToString();
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                        //----------------------------
                        hffield.Value = "SG2_ROW_ADD_E";
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                        //make_qry_4_popup();
                        //fgen.Fn_open_sseek("Select Option", frm_qstr);
                        Fn_ValueBox("-", frm_qstr);
                    }
                }
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
        fgen.Fn_open_sseek("Select Product ", frm_qstr);
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
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
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
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();

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
                oporow["chk_by"] = "-";
                oporow["chk_dt"] = vardate;
                oDS.Tables[0].Rows.Add(oporow);
            }
        }
    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {
        i = 0;
        foreach (GridViewRow gr2 in sg2.Rows)
        {
            oporow2 = oDS2.Tables[0].NewRow();
            oporow2["BRANCHCD"] = frm_mbr;
            oporow2["TYPE"] = "30";
            oporow2["VCHNUM"] = frm_vnum.Trim().ToUpper();
            oporow2["VCHDATE"] = txtvchdate.Text.Trim().ToUpper();
            //oporow2["STATUS"] = "N";
            oporow2["STATUS"] = txtlbl6.Text.Trim().ToUpper();
            oporow2["CONVDATE"] = txtlbl7a.Text.Trim().ToUpper();
            oporow2["dropdate"] = "-";
            // oporow2["comments"] = "-";
            oporow2["comments"] = txtMatlRmk.Text.Trim().ToUpper();
            oporow2["srno"] = i;
            oporow2["acode"] = doc_addl.Value.Trim();
            oporow2["Icode"] = txtlbl4.Text.Trim().ToUpper();
            oporow2["QTY"] = txtlbl7.Text.Trim().ToUpper();
            oporow2["col1"] = i + 1;
            oporow2["col2"] = gr2.Cells[13].Text.Trim().ToUpper();
            oporow2["col3"] = gr2.Cells[14].Text.Trim().ToUpper();
            oporow2["col4"] = ((TextBox)gr2.FindControl("sg2_t1")).Text.Trim().ToUpper();
            oporow2["col5"] = ((TextBox)gr2.FindControl("sg2_t2")).Text.Trim().ToUpper();
            oporow2["col6"] = ((TextBox)gr2.FindControl("sg2_t3")).Text.Trim().ToUpper();
            oporow2["col7"] = ((TextBox)gr2.FindControl("sg2_t4")).Text.Trim().ToUpper();
            oporow2["col8"] = ((TextBox)gr2.FindControl("sg2_t5")).Text.Trim().ToUpper();
            oporow2["col9"] = ((TextBox)gr2.FindControl("sg2_t7")).Text.Trim().ToUpper();
            oporow2["REMARKS"] = txtrmk.Text.Trim().ToUpper();
            oporow2["PRINTYN"] = "Y";
            //oporow2["STARTDT"] = "Y";
            oporow2["STARTDT"] = "1";
            oporow2["COL9"] = ((TextBox)gr2.FindControl("sg2_t7")).Text.Trim().ToUpper();
            oporow2["COL10"] = ((TextBox)gr2.FindControl("sg2_t8")).Text.Trim().ToUpper();
            oporow2["COL11"] = gr2.Cells[13].Text.Trim().ToUpper();
            oporow2["COL12"] = txtMktRmk.Text;
            oporow2["ENQNO"] = "-";
            oporow2["ENQDT"] = vardate;
            oporow2["COL13"] = (txtUPS.Text.toDouble() <= 0) ? "1" : txtUPS.Text.Trim().ToUpper();
            oporow2["COL14"] = txtlbl8.Text.Trim().ToUpper();
            // oporow2["COL15"] = txtlbl9.Text.Trim().ToUpper();
            oporow2["COL15"] = txtStdWstg.Text.Trim().ToUpper();
            oporow2["COL16"] = txtlbl5.Text.Trim().ToUpper();
            oporow2["COL17"] = lblPPQty.Text.Trim().ToUpper();
            oporow2["COL18"] = txtLen.Text.Trim().ToUpper();
            oporow2["COL19"] = txtwid.Text.Trim().ToUpper();
            oporow2["COL20"] = ((TextBox)gr2.FindControl("sg2_t9")).Text.Trim().ToUpper();
            if (i == 0)
            {
                oporow2["COL21"] = vardate;
                oporow2["enr1"] = lblPPQty.Text.Trim().ToUpper();
                oporow2["enr2"] = lblPPQty.Text.Trim().ToUpper();
                oporow2["col25"] = txtDirectRmk.Text.Trim().ToUpper();
                oporow2["comments5"] = "-";
            }
            else
            {
                oporow2["COL21"] = ((TextBox)gr2.FindControl("sg2_t10")).Text.Trim().ToUpper();
                oporow2["col25"] = "-";
                oporow2["enr1"] = 0;
                oporow2["enr2"] = 0;
            }

            oporow2["COL22"] = lblstdWstg.Text.Trim().ToUpper();
            // oporow2["COL23"] = gr2.Cells[3].Text.Trim();
            oporow2["COL23"] = gr2.Cells[13].Text.Trim().ToUpper(); // correct value is in column no 13
            oporow2["COL24"] = "-";
            oporow2["ITATE"] = lblPPQty.Text.Trim().ToUpper();
            oporow2["PICODE"] = txtlbl4.Text.Trim().ToUpper();
            oporow2["jstatus"] = txtlbl6.Text.ToUpper() == "Y" ? "Y" : "N";
            oporow2["irate"] = 0;
            oporow2["app_by"] = "-";
            oporow2["app_dt"] = vardate;
            oporow2["attach"] = "-";
            oporow2["attach2"] = "-";
            oporow2["attach3"] = "-";
            oporow2["comments2"] = "-";
            oporow2["comments3"] = "-";
            oporow2["az_by"] = "-";
            oporow2["az_dt"] = vardate;
            oporow2["supcl_by"] = "-";
            oporow2["comments4"] = "-";
            oporow2["splcd"] = txtnoofaround.Text.toDouble();
            //oporow2["jhold"] = "-";
            if (ind_Ptype == "12" || ind_Ptype == "13")
            { }
            else
            {
                oporow2["prc1"] = "-";
                oporow2["prc2"] = "-";
                oporow2["prc3"] = "-";
                oporow2["prc4"] = "-";
            }
            oporow2["scrp1"] = 0;
            oporow2["scrp2"] = 0;
            oporow2["time1"] = 0;
            oporow2["time2"] = 0;
            oporow2["altitem"] = "-";
            oporow2["eff_wt"] = 0;
            oporow2["NUM1"] = 0;

            if (edmode.Value == "Y")
            {
                oporow2["eNt_by"] = ViewState["entby"].ToString();
                oporow2["eNt_dt"] = ViewState["entdt"].ToString();
                oporow2["edt_by"] = frm_uname;
                oporow2["edt_dt"] = vardate;
            }
            else
            {
                oporow2["eNt_by"] = frm_uname;
                oporow2["eNt_dt"] = vardate;
                oporow2["edt_by"] = "-";
                oporow2["eDt_dt"] = vardate;
            }
            oDS2.Tables[0].Rows.Add(oporow2);
            i++;
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
        switch (Prg_Id)
        {
            case "F15101":
                SQuery = "SELECT '60' AS FSTR,'Purchase Request' as NAME,'60' AS CODE FROM dual";
                break;
        }
    }
    //------------------------------------------------------------------------------------   
    protected void sg2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (i = 0; i < 10; i++)
            {
                sg2.Columns[i].HeaderStyle.CssClass = "hidden";
                e.Row.Cells[i].CssClass = "hidden";
            }
            sg2.Columns[11].HeaderStyle.CssClass = "hidden";
            e.Row.Cells[11].CssClass = "hidden";
            for (i = 15; i < 18; i++)
            {
                sg2.Columns[i].HeaderStyle.CssClass = "hidden";
                e.Row.Cells[i].CssClass = "hidden";
            }

            for (i = 28; i < 34; i++)
            {
                sg2.Columns[i].HeaderStyle.CssClass = "hidden";
                e.Row.Cells[i].CssClass = "hidden";
            }
            sg2.Columns[23].HeaderStyle.CssClass = "hidden";
            e.Row.Cells[23].CssClass = "hidden";
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnCalc_ServerClick(object sender, EventArgs e)
    {
        double totQty = 0;
        double lab_acr1 = 0, per_mtr_lbl = 0, req_run_mtr = 0;
        double tsplcode = 0, MY_INS_CO = 0, MY_INS_DT = 0, MY_INS_CERT = 0;

        create_tab2();
        SQuery = "select acode,grade,col16,col15,col13,btchno,maintdt,BTCHDT,nvl(col1,'-') as col1,nvl(col18,'-') as col18,nvl(col2,'-') as col2,nvl(col3,'-') as col3,nvl(col4,'-') as col4,nvl(col5,'-') as col5,nvl(col10,'-') as col10,nvl(col11,'-') as col11,rejqty,recalib from inspmst where BRANCHCD='" + frm_mbr + "' AND type='70' and vchnum<>'000000' and trim(icode)='" + txtlbl4.Text.Trim() + "' order by srno ";

        string multi_lyr = "", chk_sfc = "", polyqty = "", igwt = "", stdwstg = "", polycd = "";
        // HERE TO ADD SOFTCODE FOR FLEX / LAMI / POURCH ETC
        if (ind_Ptype == "12" || ind_Ptype == "13")
        {
            //vipin
            SQuery = "select * from (select 1 as setx,A.srno,A.icode as acode,'-' AS grade,'-' AS col16,'-' AS col15,'-' AS col13,'-' AS btchno,'-' AS maintdt,'-' AS BTCHDT,a.stg_names as col1,'-' as col18,'M/c :'||a.mch_names as col2,'0' as col3,'-' as col4,A.ICODE as col5,'-' as col10,'-' as col11,1 AS rejqty,'-' AS recalib from itwstage A where A.BRANCHCD='" + frm_mbr + "' AND A.type LIKE '10%' and trim(A.icode)='" + txtlbl4.Text.Trim() + "'  union all " +
            "select 2 as setx,A.srno,A.icode as acode,'-' AS grade,'-' AS col16,'-' AS col15,'-' AS col13,'-' AS btchno,'-' AS maintdt,'-' AS BTCHDT,'Matl.'||TRIM(nvl(A.ICODE,'-')) as col1,'-' as col18,B.INAME as col2,to_Char(a.qty8/a.sampqty,'999999.999999') as col3,'-' as col4,A.ICODE as col5,'-' as col10,'-' as col11,1 AS rejqty,'-' AS recalib from inspvch A,ITEM B where TRIM(A.ICODE)=TRIM(b.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.type LIKE 'B%' and A.vchnum<>'000000' and trim(A.Acode)='" + txtlbl4.Text.Trim() + "' and a.sampqty>0 union all ";
            SQuery += "select 3 as setx,srno,acode,grade,col16,col15,col13,btchno,maintdt,BTCHDT,nvl(col1,'-') as col1,nvl(col18,'-') as col18,nvl(col2,'-') as col2,nvl(col3,'-') as col3,nvl(col4,'-') as col4,nvl(col5,'-') as col5,nvl(col10,'-') as col10,nvl(col11,'-') as col11,rejqty,recalib from inspmst where BRANCHCD='" + frm_mbr + "' AND type='70' and vchnum<>'000000' and trim(icode)='" + txtlbl4.Text.Trim() + "') order by SETX,srno ";

            //if (frm_cocd == "HPPI")
            {
                stdwstg = fgen.seek_iname(frm_qstr, frm_cocd, "select col18 from inspmst where branchcd='" + frm_mbr + "' and type='70' and trim(icode)='" + txtlbl4.Text + "' and rownum<2 ", "col18");
                igwt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT IWEIGHT FROM ITEM WHERE ICODE='" + txtlbl4.Text.Trim() + "'", "IWEIGHT");
                multi_lyr = fgen.seek_iname(frm_qstr, frm_cocd, "select col3 from inspmst where branchcd='" + frm_mbr + "' and type='70' and (trim(icode)='" + txtlbl4.Text.Trim() + "' and trim(col2)='" + txtlbl4.Text.Trim() + "') and trim(Col3)!='-' ", "COL3");
                chk_sfc = fgen.seek_iname(frm_qstr, frm_cocd, "select sampqty||'~'||acode as sampqty from inspvch where branchcd='" + frm_mbr + "' and type='B1' and trim(acode)='" + txtlbl4.Text.Trim() + "' and sampqty>0 ", "sampqty");

                if (chk_sfc == "0~0" || chk_sfc == "0")
                    chk_sfc = fgen.seek_iname(frm_qstr, frm_cocd, "select col3||'~'||col5 as col3 from inspmst where branchcd='" + frm_mbr + "' and type='70' and icode='" + txtlbl4.Text.Trim() + "' and col5 like '7%' order by srno", "col3");

                polyqty = (txtlbl7.Text.toDouble() * igwt.toDouble() * (chk_sfc.Split('~')[0].toDouble() / 1000)).ToString();
                if (multi_lyr.toDouble() > 0 && multi_lyr.toDouble() <= 100)
                {
                    polyqty = (polyqty.toDouble() * (multi_lyr.toDouble() / 100)).toDouble(0).ToString();
                }

                polyqty = (polyqty.toDouble() * ((100 + stdwstg.toDouble()) / 100)).toDouble(2).ToString();

                txtrmk.Text = "P.E. Qty = " + polyqty + " KGS";
                polycd = chk_sfc.Split('~')[1];
            }

            SQuery = "select * from (select '0' as setx,Srno,'-' as grade,'-' as col16,'-' as col15,'-' as col13,'-' as btchno,'-' as maintdt,'-' as BTCHDT,stg_names as col1,'-' as col18,'M/c :'||mch_names as col2,'-' as col3,'-' as col4,'-' as col5,'-' as col10,'-' as col11,1 as rejqty,'-' as recalib from itwstage where branchcd='" + frm_mbr + "' and trim(icode)='" + txtlbl4.Text.Trim() + "' union all select '1' as setx,00 as srno,'-' as grade,'-' as col16,'-' as col15,'-' as col13,'-' as btchno,'-' as maintdt,'-' as BTCHDT,'Extrusion Material' as col1,'-' as col18,'-' as col2,'-' as col3,'-' as col4,'-' as col5,'-' as col10,'-' as col11,1 as rejqty,'-' as recalib from dual union all " +
                     " select '2' as setx,srno,'-' as grade,'-' as col16,'-' as col15,'-' as col13,'-' as btchno,'-' as maintdt,'-' as BTCHDT,'Matl.'||trim(A.icode)||' '||upper(nvl(a.wono,''))||(case when a.obj5>0 then '  ['||nvl(a.obj5,0)||' %]' else '' end )  as col1,'-' as col18,' ['||to_char(nvl(a.qty4,0),'999.99')||' %] '||b.iname as col2,to_Char((a.qty8/a.sampqty)*(" + polyqty.toDouble() + "),'999999.99999') as col3,'-' as col4,a.icode as col5,'-' as col10,'-' as col11,1 as rejqty,'-' as recalib from inspvch a, item b where trim(A.icode)=trim(B.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.type like 'B1%' and trim(a.acode)='" + txtlbl4.Text.Trim() + "' and a.sampqty>0 " +
                     " union all select '3' as setx,srno,grade,col16,col15,col13,btchno,maintdt,BTCHDT,nvl(col1,'-') as col1,nvl(col18,'-') as col18,nvl(col2,'-') as col2,nvl(col3,'0') as col3,nvl(col4,'-') as col4,nvl(col5,'-') as col5,nvl(col10,'-') as col10,nvl(col11,'-') as col11,rejqty,recalib from inspmst where BRANCHCD='" + frm_mbr + "' AND type='70' and vchnum<>'000000' and trim(icode)='" + txtlbl4.Text.Trim() + "') ";

            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select col14||'~'||col15 as gstr from inspmst where ROWNUM<2 AND trim(icode)='" + txtlbl4.Text.Trim() + "' and type='70' and branchcd<>'DD'", "gstr");
            if (col1 != "0")
            {
                txtnoaccross.Text = col1.Split('~')[1];
                txtnoofaround.Text = col1.Split('~')[0];

                txtnoofups.Text = (txtnoaccross.Text.toDouble() * txtnoofaround.Text.toDouble()).ToString();

                MY_INS_CO = txtnoaccross.Text.toDouble();
                MY_INS_DT = txtnoofaround.Text.toDouble();
                MY_INS_CERT = MY_INS_CO * MY_INS_DT;
                txtnoofups.Text = MY_INS_CERT.ToString();
                txtUPS.Text = MY_INS_CERT.ToString();
                tsplcode = MY_INS_DT;
            }
        }


        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        i = 1;
        dt2 = new DataTable();
        DateRange = "BETWEEN TO_DATE('" + frm_CDT1 + "','DD/MM/YYYY') AND TO_DATE('" + frm_CDT2 + "','DD/MM/YYYY')-1";
        xprdrange1 = "BETWEEN TO_DATE('" + fromdt + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
        mq2 = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdrange1 + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + DateRange + " and store='Y' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE";
        dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);

        foreach (DataRow drn in dt.Rows)
        {
            txtLen.Text = dt.Rows[0]["maintdt"].ToString().Trim();
            txtwid.Text = dt.Rows[0]["BTCHDT"].ToString().Trim();
            lblstdWstg.Text = dt.Rows[0]["col18"].ToString().Trim();
            double stdWstg = fgen.make_double(lblstdWstg.Text.Replace("%", ""));

            txtlbl5.Text = dt.Rows[0]["recalib"].ToString().Trim();
            txtStdWstg.Text = Math.Round(fgen.make_double(txtlbl8.Text) * (stdWstg / 100)).ToString();
            //-----------------
            sg2_dr = sg2_dt.NewRow();
            sg2_dr["sg2_srno"] = i;

            sg2_dr["sg2_h1"] = drn["col5"].ToString().Trim();
            sg2_dr["sg2_h2"] = drn["col2"].ToString().Trim();

            if (drn["col1"].ToString().Trim().Length > 0) sg2_dr["sg2_h3"] = drn["col1"].ToString().Trim();
            else sg2_dr["sg2_h3"] = "-";

            sg2_dr["sg2_f1"] = drn["col1"].ToString().Trim();
            sg2_dr["sg2_f2"] = drn["col2"].ToString().Trim();
            double GridQty = 0;
            if (sg1.Rows.Count > 0)
            {
                GridQty = fgen.make_double(((TextBox)sg1.Rows[0].FindControl("sg1_t5")).Text);
                txtlbl7.Text = GridQty.ToString();
            }

            if (drn["col5"].ToString().Trim().Length > 4)
            {
                double papreq = 0, qtyReq = 0, extraReq = 0; string pap_gsm = "", UPS = "";
                // calculation on industry type.
                switch (ind_Ptype)
                {
                    case "12":
                    case "13":
                        UPS = MY_INS_CERT.ToString();
                        if (tsplcode > 0)
                        {
                            lab_acr1 = UPS.toDouble() / tsplcode;
                            if (drn["col13"].ToString().Trim().toDouble() > 0 && lab_acr1 > 0)
                            {
                                per_mtr_lbl = ((tsplcode * 1000) / (drn["col13"].ToString().Trim().toDouble() * 25.4));
                                req_run_mtr = (txtlbl7.Text.toDouble() / per_mtr_lbl) / lab_acr1;
                            }
                        }
                        else
                        {
                            if (drn["col3"].ToString().Trim().toDouble() > 0)
                            {
                                per_mtr_lbl = ((drn["col3"].ToString().Trim().toDouble()));
                                req_run_mtr = per_mtr_lbl;
                            }
                        }

                        qtyReq = req_run_mtr;

                        sg2_dr["sg2_t1"] = "-";
                        break;
                    default:
                        // checking on main group code
                        switch (drn["col5"].ToString().Trim().Substring(0, 2))
                        {
                            case "02":
                                if (fgen.make_double(txtUPS.Text) == 0)
                                {
                                    UPS = "1";
                                }
                                else
                                {
                                    UPS = txtUPS.Text;
                                }
                                qtyReq = fgen.make_double(txtlbl7.Text) / fgen.make_double(UPS);
                                break;
                            case "07":
                            case "81":
                                papreq = (fgen.make_double(txtLen.Text) * fgen.make_double(txtwid.Text)) / 10000;
                                pap_gsm = fgen.seek_iname(frm_qstr, frm_cocd, "select oprate3 from item where trim(icode)='" + drn["col5"].ToString().Trim() + "'", "oprate3");

                                papreq = (papreq * fgen.make_double(pap_gsm)) / 1000;
                                papreq = (papreq * fgen.make_double(txtlbl7.Text.Trim()));

                                //if (fgen.make_double(UPS) == 0) UPS = "1";
                                if (fgen.make_double(txtUPS.Text) == 0)
                                {
                                    UPS = "1";
                                }
                                else
                                {
                                    UPS = txtUPS.Text;
                                }
                                qtyReq = (papreq / fgen.make_double(UPS));
                                break;
                        }
                        sg2_dr["sg2_t1"] = "1";
                        break;
                }

                extraReq = (stdWstg / 100) * qtyReq;

                if (fgen.make_double(drn["col11"].ToString().Trim()) > 0)
                {
                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select ACREF FROM TYPEGRP WHERE BRANCHCD!='DD' AND LINENO=" + fgen.make_double(drn["col11"].ToString().Trim()) + "", "ACREF");
                    qtyReq = qtyReq * ((100 + fgen.make_double(col1)) / 100);
                }

                sg2_dr["sg2_t2"] = Math.Round(qtyReq, 4);
                sg2_dr["sg2_t3"] = Math.Round(extraReq, 4);
                sg2_dr["sg2_t4"] = Math.Round(qtyReq + extraReq, 4);
                if (dt2.Rows.Count > 0)
                {
                    sg2_dr["sg2_t5"] = fgen.seek_iname_dt(dt2, "icode='" + drn["col5"].ToString().Trim() + "'", "cl");
                }
                else
                {
                    sg2_dr["sg2_t5"] = 0;
                }
                totQty += Math.Round(qtyReq + extraReq, 4);

                sg2_dr["sg2_t7"] = drn["col5"].ToString().Trim();
                sg2_dr["sg2_t8"] = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select sum(to_number(col7)) as totals from costestimate where branchcd='" + frm_mbr + "' and type='30' and trim(col9)='" + drn["col5"].ToString() + "' and VCHDATE>=TO_dATE('" + frm_CDT1 + "','DD/MM/YYYY')", "totals"), 2);

                sg2_dr["sg2_t9"] = drn["col10"].ToString().Trim();
                sg2_dr["sg2_t10"] = drn["col11"].ToString().Trim();
            }
            else
            {
                sg2_dr["sg2_t1"] = "-";
                sg2_dr["sg2_t2"] = "-";
                sg2_dr["sg2_t3"] = "-";
                sg2_dr["sg2_t4"] = "-";
                sg2_dr["sg2_t7"] = "-";
                sg2_dr["sg2_t8"] = "-";
                sg2_dr["sg2_t9"] = "-";
                sg2_dr["sg2_t10"] = "-";
            }
            sg2_dt.Rows.Add(sg2_dr);
            i++;
        }
        sg2.DataSource = sg2_dt; sg2.DataBind();
        sg2_dt.Dispose();
        lblPPQty.Text = totQty.ToString();

        //if (ind_Ptype == "12" || ind_Ptype == "13") { }
        //else
        {
            double g_col4 = 0;
            double g_col5 = 0;

            foreach (GridViewRow gr in sg2.Rows)
            {
                if (((TextBox)gr.FindControl("sg2_t7")).Text.Trim().Length > 4 && ((TextBox)gr.FindControl("sg2_t5")).Text.toDouble() <= 0)
                {
                    gr.BackColor = Color.LightPink;
                }

                g_col4 = fgen.make_double(((TextBox)gr.FindControl("sg2_t4")).Text.ToString().Trim());
                g_col5 = fgen.make_double(((TextBox)gr.FindControl("sg2_t5")).Text.ToString().Trim());

                if (((TextBox)gr.FindControl("sg2_t7")).Text.Trim().Length > 4 && g_col4 > g_col5 && g_col4 > 0)
                {
                    gr.BackColor = Color.LightPink;
                }
            }
        }
    }
    //------------------------------------------------------------------------------------
    public void Fn_ValueBox(string titl, string QR_str)
    {
        fgenMV.Fn_Set_Mvar(QR_str, "U_BOXTYPE", "ITEM");
        if (HttpContext.Current.CurrentHandler is Page)
        {
            string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/om_ch_paper.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle1('" + fil_loc + "?STR=" + QR_str + "','1000px','610px','" + titl + "');", true);
        }

    }
    //------------------------------------------------------------------------------------
    protected void btnView_ServerClick(object sender, EventArgs e)
    {
        if (txtlbl4.Text.Trim().Length > 2)
        {
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NVL(IMAGEF,'-') AS IMAGEF FROM ITEM WHERE ICODE='" + txtlbl4.Text.Trim() + "' ", "IMAGEF");
            if (col1.Length > 2)
            {
                try
                {
                    string newPath = Server.MapPath(@"~\tej-base\upload\");
                    string filename = Path.GetFileName(col1);
                    newPath += filename;
                    File.Copy(col1, newPath, true);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filename + "','90%','90%','');", true);
                }
                catch { }
            }
            else
            {
                fgen.msg("-", "AMSG", "No File Attached!!");
            }
        }
        else
        {
            fgen.msg("-", "AMSG", "Job Card Not Selected!!");
        }

    }
    protected void btnchkstk_Click(object sender, EventArgs e)
    {
        string mq0, mq1, mq2, mq3, mq4;
        string xprd1, xprd2, CONSS;
        CONSS = "BRANCHCD='" + frm_mbr + "'";
        xprd1 = " between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_Date('" + fromdt + "','dd/mm/yyyy')-1";
        xprd2 = " between to_date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";
        mq0 = "select icode,sum(opening) as opening,sum(cdr) as CDBTS,sum(ccr) as CCDTS,sum(opening)+sum(cdr)-sum(ccr) as closing,sum(qap) as qap from (Select icode, " + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos,0 as qap from itembal where " + CONSS + "  union all  ";
        mq1 = "select icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,0 as qap from ivoucher where " + CONSS + " and type like '%' and vchdate " + xprd1 + " and store='Y' GROUP BY ICODE union all ";
        mq2 = "select icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,0 as qap from ivoucher where " + CONSS + "  and type like '%' and vchdate " + xprd2 + " and store='Y' GROUP BY ICODE UNION ALL ";
        mq3 = "select icode,0 as op,0 as cdr,0 as ccr,0 as clos,sum(iqtyin) as qap from ivoucher where " + CONSS + " and type like '0%' and vchdate " + xprd2 + " and nvl(actual_insp,'-')='-' AND STORE<>'R' GROUP BY ICODE )group by icode having sum(opening)+sum(cdr)+sum(ccr)+sum(qap)>0 ";
        mq4 = "select x.iname,x.cpartno,x.icode, nvl(y.opening,0) as opening , nvl(y.cdbts,0) as  Debits, nvl(y.ccdts,0) as Credits ,nvl(y.Closing,0) as closing ,nvl(y.qap,0) as Qa_Pend,x.IMIN,x.unit,x.cdrgno,x.mat10 as Revno,X.binno from item x,(" + mq0 + mq1 + mq2 + mq3 + " ) y where trim(x.icode)=trim(y.icode)";
        SQuery = "select '' as fstr, iname as Iname,icode as ERP_Code, to_char(opening,'9999999990.00') as opening , to_char(Debits,'9999999990.00') as  Inward, to_char(Credits,'9999999990.00') as Outward,to_char(closing,'9999999990.00') as closing,icode as acode,IMIN,Qa_Pend,substr(icode,1,2) as grpc,unit,cdrgno from (" + mq4 + ")  a where abs(debits)+abs(credits)+abs(opening)+abs(closing)!=0 and length(Trim(icode))>4 order by grpc,iname";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("Stock Status", frm_qstr);
        //fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnreelstk_Click(object sender, EventArgs e)
    {
        SQuery = "select * from (select trim(a.kclreelno)||':'||trim(a.Coreelno) as Batchno,TRIM(a.acode) as aname,b.cpartno,a.psize,a.gsm,sum(nvl(reelwin,0))-sum(nvl(reelwout,0)) as tot,TRIM(a.icode) AS ICODE,TRIM(a.Coreelno) AS Coreelno,b.iname,min(a.vchdate) as MRR_Dt from reelvch a left outer join item b on trim(a.icode)=trim(b.icode) where a.branchcd='" + frm_mbr + "' and a.posted='Y' group by b.iname,TRIM(a.acode),trim(a.kclreelno)||':'||trim(a.Coreelno),a.psize,a.gsm,TRIM(a.Coreelno),b.cpartno,TRIM(a.icode) having sum(nvl(reelwin,0))-sum(nvl(reelwout,0)) >0) order by psize,gsm,cpartno,MRR_Dt ";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("Reel In Stk", frm_qstr);
    }
    protected void btnreelwip_Click(object sender, EventArgs e)
    {
        hffield.Value = "REELWIP";
        fgen.Fn_open_prddmp1("-", frm_qstr);
    }
    protected void btnpjobs_Click(object sender, EventArgs e)
    {
        hffield.Value = "PJOBS";
        make_qry_4_popup();
        fgen.Fn_open_mseek("-", frm_qstr);
    }
    protected void btnlstiss_Click(object sender, EventArgs e)
    {
        SQuery = "";
        SQuery = "Select b.iname,b.cpartno,a.iqtyout,b.unit,a.vchdate,a.vchnum from ivoucher a, item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type like '3%' and trim(A.invno)||to_Char(A.invdate,'dd/mm/yyyy') in (select distinct trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') from costestimate where branchcd='" + frm_mbr + "' and type='30' and trim(icode)='" + txtlbl4.Text.Trim() + "') order by a.vchdate,a.vchnum";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("Last Issue Entry", frm_qstr);
    }
    protected void btndspsch_Click(object sender, EventArgs e)
    {
        SQuery = "";
        SQuery = "select b.aname,to_char(a.dlV_date,'dd/mm/yyyy') as dlv_date,budgetcost as sch_qty,disp_qty,solink from (select acode,dlv_date,budgetcost,0 as disp_qty,substr(solink,1,20) as solink from budgmst where branchcd='" + frm_mbr + "' and type='46' and dlv_Date " + DateRange + " and trim(icode)='" + txtlbl4.Text.Trim() + "' and substr(solink,1,20)='" + txtlbl7a.Text.Trim().Substring(0, 20) + "' union all select acode,vchdate,0 as actual_cost,iqtyout as disp_qty,branchcd||type||ponum||to_char(podate,'dd/mm/yyyy') from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + DateRange + " and trim(icode)='" + txtlbl4.Text.Trim() + "' and branchcd||type||ponum||to_char(podate,'dd/mm/yyyy')='" + txtlbl7a.Text.Trim().Substring(0, 20) + "') a, famst b where trim(A.acode)=trim(B.acode) order by a.dlV_date";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_rptlevel("Desp Sch", frm_qstr);
    }
    protected void btnGridPop_Click(object sender, EventArgs e)
    {
        if (hf1.Value.Contains("sg2_t1_"))
        {
            hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg2_sg2_t1_", "");
            insertRow();
        }
    }
    void insertRow()
    {
        #region Remove Row from GridView
        {
            dt = new DataTable();
            sg2_dt = new DataTable();
            dt = (DataTable)ViewState["sg2"];
            sg2_dr = null;
            create_tab2();

            if (dt == null) return;
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                if (i == hf1.Value.ToString().toDouble() + 1)
                {
                    sg2_add_blankrows();
                    sg2_dt.Rows[i]["sg2_f1"] = dt.Rows[i - 1]["sg2_f1"].ToString().Trim() + " (ALT)";
                }

                sg2_dr = sg2_dt.NewRow();
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    sg2_dr[c] = dt.Rows[i][c];
                }
                sg2_dt.Rows.Add(sg2_dr);
            }
            ViewState["sg2"] = sg2_dt;
            sg2_add_blankrows();
            sg2.DataSource = sg2_dt;
            sg2.DataBind();
            for (i = 0; i < sg2.Rows.Count; i++)
            {
                sg2.Rows[i].Cells[12].Text = (i + 1).ToString();
            }
        }
        #endregion
        setColHeadings();
    }
}
