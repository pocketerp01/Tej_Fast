using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.IO;
using System.Text;
using System.Text.RegularExpressions;

public partial class findDrCr : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4;
    DataRow oporow, oporow1, oporow2; DataSet oDS, oDS1, oDS2;
    int i = 0, z = 0;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string lvch_5859, lvch_5859_date;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

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

                    if (frm_qstr.Contains("^"))
                    {
                        if (frm_cocd != frm_qstr.Split('^')[0].ToString())
                        {
                            frm_cocd = frm_qstr.Split('^')[0].ToString();
                        }
                    }

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
                doc_addl.Value = "0";
                lvch_5859 = fgen.getOption(frm_qstr, frm_cocd, "W0131", "OPT_ENABLE");
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_lvch_5859", lvch_5859);
                lvch_5859_date = fgen.getOption(frm_qstr, frm_cocd, "W0131", "OPT_PARAM");

                if (lvch_5859 == "Y")
                {
                    if (fgen.IsDate(lvch_5859_date))
                    {
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_lvch_5859_date", lvch_5859_date);
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Please check date in control W0131.System won't allow further processing.");
                        return;
                    }
                }

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
                DataTable dtW = (DataTable)ViewState["dtn"];
                if (dtW != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtW, "", "ContentPlaceHolder1_datadiv").ToString(), false);
                }
                chktcs.Checked = true;
            }
            
            setColHeadings();
            set_Val();
            btnprint.Visible = false;
            btnedit.Visible = false;
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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;

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

        btnAcode.Enabled = false;
        btnIcode.Enabled = false;
        btnDNCN.Enabled = false;
        btnGstClass.Enabled = false;

        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;

        btnAcode.Enabled = true;
        btnIcode.Enabled = true;
        btnDNCN.Enabled = true;
        btnGstClass.Enabled = true;
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
        frm_tabname = "SCRATCH2";

        lblheader.Text = "Auto Debit Credit Note";

        if (frm_formID == "F70118B") lblheader.Text = "Bajaj Auto Debit Credit Note";

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "DC");
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
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":
                SQuery = "SELECT Type1,Name,Type1 AS CODE,id2 as Ref FROM Type WHERE id='#' and id2='CL' ORDER BY Name ";
                break;
            case "TACODE":
                SQuery = "select acode,aname as customer,acode as code from famst where substr(trim(Acode),1,2) in ('16','02') order by acode";
                if (frm_formID == "F70118B") SQuery = "select acode,aname as customer,acode as code from famst where trim(acode) like '16B%' AND upper(trim(ANAME)) LIKE '%BAJAJ%' order by acode";
                break;
            case "TICODE":
                SQuery = "select icode,iname as product,icode as code,cpartno,unit from item where length(trim(icode))>4 and trim(icode) like '9%' order by icode";
                SQuery = "select distinct a.icode,A.PURPOSE as product,a.icode as erpcode,A.EXC_57F4 as partname,a.ponum,to_Char(a.podate,'dd/mm/yyyy') as podate,a.finvno as cust_pono,a.vchnum as invno,to_char(a.vchdate,'dd/mm/yyyy') as invdate from ivoucher a  where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate between (sysdate-730) and sysdate and trim(A.acode)='" + txtacode.Value.Trim() + "' order by a.vchnum,a.icode";
                if (frm_cocd == "BONY") SQuery = "select distinct a.icode,A.PURPOSE as product,a.icode as erpcode,A.EXC_57F4 as partname,a.ponum,to_Char(a.podate,'dd/mm/yyyy') as podate,a.finvno as cust_pono,a.vchnum as invno,to_char(a.vchdate,'dd/mm/yyyy') as invdate from ivoucher a  where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate between (sysdate-1000) and sysdate and trim(A.acode)='" + txtacode.Value.Trim() + "' order by a.vchnum,a.icode";
                if (frm_formID == "F70118B") SQuery = "select distinct a.icode,A.PURPOSE as product,a.icode as erpcode,A.EXC_57F4 as partname,a.ponum,to_Char(a.podate,'dd/mm/yyyy') as podate,a.finvno as cust_pono,a.vchnum as invno,to_char(a.vchdate,'dd/mm/yyyy') as invdate,a.REFNUM AS GRNO,to_char(a.EXC_57F4DT,'DD/mm/yyyy') as grdt from ivoucher a  where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate between (sysdate-1000) and sysdate and trim(A.acode)='" + txtacode.Value.Trim() + "' and nvl(a.REFNUM,'-')!='-' order by a.vchnum,a.icode";
                if (frm_cocd == "SUNB") SQuery = "select distinct a.icode,A.PURPOSE as product,a.icode as erpcode,A.EXC_57F4 as partname,a.ponum,to_Char(a.podate,'dd/mm/yyyy') as podate,a.finvno as cust_pono from ivoucher a  where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate between (sysdate-730) and sysdate and trim(A.acode)='" + txtacode.Value.Trim() + "' order by A.PONUM,a.icode";
                break;
            case "DNCN":
                SQuery = "SELECT TYPE1,NAME AS REASON,TYPE1 AS CODE FROM TYPE WHERE ID='$' ORDER BY TYPE1";
                break;
            case "GSTCLASS":
                SQuery = "SELECT TYPE1,NAME AS REASON,TYPE1 AS CODE FROM TYPE WHERE ID='}' ORDER BY TYPE1";
                break;
            case "New":
            case "List":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD" || btnval == "LIST_E" || btnval == "LIST_E2")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.col33 as po,A.COL35 AS po_DATE,A.ACODE AS PARTY_CODE,B.ANAME AS PARTY,A.ICODE AS ERPCODE,C.INAME AS PART,C.CPARTNO AS PARTNO,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,FAMST B,ITEM C where TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(a.ICODE)=TRIM(C.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        if (frm_cocd == "MEGA")
        {
            if (frm_mbr == "00" || frm_mbr == "01") { }
            else
            {
                fgen.msg("-", "AMSG", "This Module is Activated only for plant 00 and 01");
                return;
            }
        }

        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            //hffield.Value = "New";
            //make_qry_4_popup();
            //fgen.Fn_open_sseek("-", frm_qstr);

            // else comment upper code

            frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            disablectrl();
            fgen.EnableForm(this.Controls);

            btnAcode.Focus();
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        if (frm_ulvl != "0") return;
        chk_rights = fgen.Fn_chk_can_edit(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        clearctrl();
        if (chk_rights == "Y")
        {
            hffield.Value = "Edit_E";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " ", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Edit Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Save";
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
        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only"); txtvchdate.Focus(); return; }

        if (txtacode.Value.Trim().Length < 2)
        { fgen.msg("-", "AMSG", "Please Select Customer Code!!"); txtvchdate.Focus(); return; }

        if (fgen.make_double(txtNrate.Value.Trim()) <= 0)
        { fgen.msg("-", "AMSG", "Please Enter New Rate!!"); txtNrate.Focus(); return; }

        calc();

        if (txtGstClassCode.Value.Length < 2)
        { fgen.msg("-", "AMSG", "Please Select GST Class!!"); btnGstClass.Focus(); return; }
        if (txtDnCnCode.Value.Length < 2)
        { fgen.msg("-", "AMSG", "Please Select Reason for D/N C/N !!"); btnGstClass.Focus(); return; }

        DataTable dtn = new DataTable();
        dtn = (DataTable)ViewState["dtn"];
        ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);

        DataView dv = new DataView(dtn);
        dtn = new DataTable();
        dtn = dv.ToTable(true, "sg1_f3");
        dt = new DataTable();
        dt.Columns.Add("ENTRY_NO", typeof(string));
        dt.Columns.Add("ENTRY_DT", typeof(string));
        dt.Columns.Add("BRANCH", typeof(string));
        dt.Columns.Add("Batchno", typeof(string));
        DataRow dr = null;
        foreach (DataRow drn in dtn.Rows)
        {
            dt2 = new DataTable();
            dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT distinct vchnum,vchdate,branchcd,COL33,min(num10) as num10 FROM SCRATCH2 WHERE COL33='" + drn["sg1_f3"].ToString().Trim() + "' group by vchnum,vchdate,branchcd,COL33 order by vchnum desc");
            if (dt2.Rows.Count > 0)
            {
                dr = dt.NewRow();
                dr["entry_no"] = dt2.Rows[0]["vchnum"].ToString().Trim();
                dr["entry_dt"] = dt2.Rows[0]["vchdate"].ToString().Trim();
                dr["branch"] = dt2.Rows[0]["branchcd"].ToString().Trim();
                dr["batchno"] = dt2.Rows[0]["col33"].ToString().Trim();
                dt.Rows.Add(dr);
            }
        }
        string crFound = "N";
        //if (txtAname.Value.ToString().ToUpper().Contains("MARUTI"))
        //{
        //if (dt2.Rows.Count > 0)
        //{
        //    if (dt2.Rows[0]["num10"].ToString() == "0" && dt.Rows.Count > 0)
        //    {
        //        dtn = new DataTable();
        //        dtn = (DataTable)ViewState["dtn"];
        //        foreach (DataRow drn in dtn.Rows)
        //        {
        //            if (fgen.make_double(drn["sg1_h10"].ToString().Trim()) > 0) crFound = "Y";
        //        }
        //        if (crFound == "Y")
        //        {
        //            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", These Batch is already exist!!'13'Please Upload only Credit Entries");
        //            return;
        //        }
        //    }
        //}
        //}
        //else if (dt.Rows.Count > 0)
        //{
        //    Session["send_dt"] = dt;
        //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
        //    fgen.Fn_open_rptlevel("These Batch No Already Exist!!'13'Please delete first befor uploading.", frm_qstr);
        //    return;
        //}

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
    void calc()
    {
        double oldRate = 0;
        DataTable dtn = (DataTable)ViewState["dtn"];

        foreach (GridViewRow gr in sg1.Rows)
        {
            gr.Cells[8].Text = txtNrate.Value;
            if (fgen.make_double(txtOldRate.Value) != 0)
            {
                oldRate = fgen.make_double(txtOldRate.Value);
                gr.Cells[7].Text = oldRate.ToString();
            }
            else oldRate = fgen.make_double(gr.Cells[7].Text);
            gr.Cells[9].Text = Math.Round(fgen.make_double(gr.Cells[8].Text) - oldRate, 3).ToString();

            if (fgen.make_double(txtSgst.Value.Trim()) > 0 || fgen.make_double(txtCgst.Value.Trim()) > 0)
            {
                if (fgen.make_double(txtSgst.Value.Trim()) > 0)
                {
                    gr.Cells[16].Text = txtCgst.Value;
                    gr.Cells[17].Text = txtSgst.Value;
                }
                else
                {
                    gr.Cells[16].Text = txtCgst.Value;
                    gr.Cells[17].Text = "0";
                }
            }
            gr.Cells[28].Text = Convert.ToString(fgen.make_double(((TextBox)gr.FindControl("sg1_h7")).Text) * fgen.make_double(txtNrate.Value.Trim()));
        }

        dtn = (DataTable)ViewState["dtn"];
        double d1 = 0;
        foreach (DataRow dr in dtn.Rows)
        {
            foreach (GridViewRow gr in sg1.Rows)
            {
                if (gr.Cells[10].Text.Trim() == dr["sg1_srno"].ToString().Trim()) dr["sg1_h7"] = ((TextBox)gr.FindControl("sg1_h7")).Text.Trim();
            }
            if (fgen.make_double(txtOldRate.Value) != 0)
            {
                oldRate = fgen.make_double(txtOldRate.Value);
                dr["sg1_h8"] = oldRate.ToString();
            }
            dr["sg1_h9"] = txtNrate.Value;
            dr["sg1_h10"] = Math.Round(fgen.make_double(txtNrate.Value) - fgen.make_double(dr["sg1_h8"].ToString().Trim()), 3).ToString();
            if (fgen.make_double(txtSgst.Value.Trim()) > 0 || fgen.make_double(txtCgst.Value.Trim()) > 0)
            {
                if (fgen.make_double(txtSgst.Value.Trim()) > 0)
                {
                    dr["SG1_T3"] = "CG";
                    dr["sg1_t9"] = txtCgst.Value;
                    dr["sg1_t10"] = txtSgst.Value;
                    dr["sg1_t11"] = 0;
                }
                else
                {
                    dr["SG1_T3"] = "IG";
                    dr["sg1_t9"] = 0;
                    dr["sg1_t10"] = 0;
                    dr["sg1_t11"] = txtCgst.Value;
                }
            }
            dr["sg1_t13"] = fgen.make_double(dr["sg1_h7"].ToString().Trim()) * fgen.make_double(txtNrate.Value.Trim());

            d1 += fgen.make_double(dr["sg1_h7"].ToString().Trim());
        }
        ViewState["dtn"] = dtn;

        lblRowCount.Text = "Total Rows Showing : " + sg1.Rows.Count.ToString() + " , Total Qty : " + d1;
    }
    //------------------------------------------------------------------------------------
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "Y")
        {
            clearctrl();
            set_Val();
            hffield.Value = "Del_E";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " ", frm_qstr);
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
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "LIST_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
        //fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {

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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')||trim(a.COL33)='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "'");
                // Deleing data from Sr Ctrl Table                
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.acode)='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "' and a.type in ('" + frm_vty + "','58','59') ");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'  ");
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (select DISTINCT a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr from IVOUCHER A WHERE A.BTCHNO='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "')");
                // Deleing data from voucher Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from voucher a where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (select DISTINCT a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr from IVOUCHER A WHERE A.BTCHNO='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "')");
                // Deleing data from Ivoucher Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivoucher a where A.BTCHNO='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' AND A.TYPE IN ('58','59') AND A.BRANCHCD='" + frm_mbr + "'");
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
        else if (hffield.Value == "SAVE")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y") hfCNote.Value = "Y";
            else hfCNote.Value = "N";
            DataTable dtn = new DataTable();
            dtn = (DataTable)ViewState["dtn"];
            ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);
            fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
            btnval = hffield.Value;
            switch (btnval)
            {
                case "List":
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
                    break;
                case "New":
                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    txtvchnum.Text = frm_vnum;
                    txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    disablectrl();
                    fgen.EnableForm(this.Controls);
                    // Popup asking for Copy from Older Data
                    fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Form'13'(No for make it new)");
                    hffield.Value = "NEW_E";
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
                    SQuery = "Select a.*,b.iname,b.cpartno,b.unit,b.hscode from scratch2 a,item b where trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        txtacode.Value = dt.Rows[0]["acode"].ToString().Trim();
                        txtAname.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where trim(acode)='" + dt.Rows[0]["acode"].ToString().Trim() + "'", "aname");
                        txtIcode.Value = dt.Rows[0]["icode"].ToString().Trim();
                        txtIname.Value = dt.Rows[0]["iname"].ToString().Trim();
                        txtCpartno.Value = dt.Rows[0]["cpartno"].ToString().Trim();

                        txtponum.Value = dt.Rows[0]["col33"].ToString().Trim();
                        txtPodt.Value = dt.Rows[0]["col35"].ToString().Trim();
                        txtcustPo.Text = dt.Rows[0]["col33"].ToString().Trim();

                        // ADD ON 02 MAY 2018 BY MADHVI
                        txtGstClassCode.Value = dt.Rows[0]["col47"].ToString().Trim();
                        txtGstClassName.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where trim(type1)='" + dt.Rows[0]["col47"].ToString().Trim() + "' and id='}'", "name");
                        txtDnCnCode.Value = dt.Rows[0]["col46"].ToString().Trim();
                        txtDnCnName.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where trim(type1)='" + dt.Rows[0]["col46"].ToString().Trim() + "' and id='$'", "name");
                        txtCgst.Value = dt.Rows[0]["num1"].ToString().Trim();
                        txtSgst.Value = dt.Rows[0]["num2"].ToString().Trim();
                        txtNrate.Value = dt.Rows[0]["col8"].ToString().Trim();
                        // -----------------------------

                        i = 1;
                        create_tab();
                        foreach (DataRow dr in dt.Rows)
                        {
                            sg1_dr = sg1_dt.NewRow();

                            sg1_dr["sg1_SrNo"] = i;
                            sg1_dr["sg1_h1"] = dr["branchcd"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dr["col1"].ToString().Trim();
                            sg1_dr["sg1_h3"] = dr["col2"].ToString().Trim();
                            sg1_dr["sg1_h4"] = dr["col3"].ToString().Trim();

                            sg1_dr["sg1_h5"] = dr["col4"].ToString().Trim();
                            sg1_dr["sg1_h6"] = dr["col5"].ToString().Trim();
                            //qty
                            sg1_dr["sg1_h7"] = dr["col6"].ToString().Trim();
                            sg1_dr["sg1_h8"] = dr["col7"].ToString().Trim();
                            //new rate
                            sg1_dr["sg1_h9"] = dr["col8"].ToString().Trim();
                            //diff
                            sg1_dr["sg1_h10"] = dr["col9"].ToString().Trim();

                            sg1_dr["sg1_f1"] = dr["col11"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dr["col12"].ToString().Trim();

                            sg1_dr["sg1_F3"] = dr["col13"].ToString().Trim();
                            sg1_dr["sg1_F4"] = dr["col14"].ToString().Trim();

                            sg1_dr["sg1_f5"] = dr["col15"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dr["col16"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dr["col17"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dr["col18"].ToString().Trim();

                            sg1_dr["sg1_t5"] = dr["iname"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dr["cpartno"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dr["unit"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dr["HSCODE"].ToString().Trim();

                            sg1_dr["sg1_t9"] = dr["col16"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dr["col17"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dr["col16"].ToString().Trim();

                            sg1_dr["sg1_t12"] = 0;

                            sg1_dt.Rows.Add(sg1_dr);
                            i++;
                        }
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();

                        lblRowCount.Text = "Total Rows Showing : " + sg1.Rows.Count.ToString();

                        ViewState["dtn"] = sg1_dt;

                        dt.Dispose();
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                    }
                    #endregion
                    break;
                case "LIST_E":
                    SQuery = "SELECT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.COL33 AS cust_pono,A.COL2 AS inv_no,a.col3 as inv_dt,a.acode as partycode,c.aname as party,b.icode as erpcode,d.iname as product,a.col6 as qty,a.col7 as oldrate,a.col8 as newrate,a.col9 as rate_diff,a.col13 as pono,a.col14 as pordt,a.col15 as inv_value,to_number(a.col16)+to_number(a.col17) as tax_per,a.ent_by,a.ent_dt,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,B.TYPE AS NOTE_TYPE,B.BRANCHCD AS B_CODE FROM SCRATCH2 A,ivoucher B,famst c,item d WHERE TRIM(A.ACODE)||TRIM(A.COL2)||TO_CHAR(TO_DATE(A.COL3,'DD/MM/YYYY'),'DD/MM/YYYY')||TRIM(A.COL33)||trim(a.col5)=TRIM(B.ACODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||TRIM(B.LOCATION)||trim(b.icode) and trim(A.acode)=trim(c.acode) and trim(b.icode)=trim(d.icode) AND a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' and b.type in ('58','59') ORDER BY A.COL33";
                    col3 = frm_cocd == "SAIP" ? ",b.REFNUM as grno" : "";
                    SQuery = "SELECT DISTINCT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.COL33 AS cust_pono,A.COL2 AS inv_no,a.col3 as inv_dt,a.acode as partycode,c.aname as party,b.icode as erpcode,B.PURPOSE as product,B.EXC_57F4,a.col6 as qty,a.col7 as oldrate,a.col8 as newrate,a.col9 as rate_diff,a.col13 as pono,a.col14 as pordt,to_number(a.col16)+to_number(a.col17) as tax_per,a.ent_by,a.ent_dt,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.iamount as amt_wot,b.spexc_amt as amt,B.TYPE AS NOTE_TYPE,B.BRANCHCD AS B_CODE "+col3+" FROM SCRATCH2 A,ivoucher B,famst c WHERE TRIM(A.ACODE)||TRIM(A.COL2)||TO_CHAR(TO_DATE(A.COL3,'DD/MM/YYYY'),'DD/MM/YYYY')||TRIM(A.COL33)||trim(a.col5)||trim(a.col6)=TRIM(B.ACODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||TRIM(B.LOCATION)||trim(b.icode)||b.iqty_chl AND A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(B.BTCHNO) and trim(A.acode)=trim(c.acode) AND a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' and b.type in ('58','59') ORDER BY A.COL33";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "LIST_E2":
                    SQuery = "SELECT A.BRANCHCD,A.INVNO,A.INVDATE,A.ACODE AS CODE,B.ANAME AS PARTY,A.ICODE AS ERPCODE,C.INAME AS PRODUCT,C.CPARTNO,SUM(A.AMT) AS UPLD_AMT,SUM(A.AMT2) AS IVCH_AMT,SUM(A.AMT3) AS VCH_AMT FROM (SELECT BRANCHCD,TRIM(COL11) AS INVNO,TRIM(COL12) AS INVDATE,TRIM(ACODE) AS ACODE,TRIM(ICODE) AS ICODE,SUM(IS_NUMBER(col6*col9)) AS AMT,0 AS AMT2,0 AS AMT3 FROM SCRATCH2 WHERE branchcd||type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' group by branchcd,trim(col11),trim(Col12),trim(acode),trim(icodE) UNION ALL  SELECT a.BRANCHCD,TRIM(a.INVNO),TO_CHAR(a.INVDATE,'DD/MM/YYYY') AS INVATE,TRIM(a.ACODE) AS ACODE,TRIM(a.ICODE) AS ICODE,0 AS AMT,SUM(a.IAMOUNT) AS AMT2,sum(b.dramt+b.cramt) AS AMT3 FROM IVOUCHER a ,voucher b where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) =b.branchcd||b.type||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy')||trim(b.Rcode) and a.BRANCHCD='" + frm_mbr + "' and a.type in ('58','59') and a.btchno='" + frm_mbr + col1 + "' and b.srno='50' group by a.branchcd,TRIM(a.INVNO),TO_CHAR(a.INVDATE,'DD/MM/YYYY'),TRIM(a.ACODE),TRIM(a.ICODE)) A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(b.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) GROUP BY A.BRANCHCD,A.INVNO,A.INVDATE,A.ACODE,B.ANAME,A.ICODE,C.INAME,C.CPARTNO ORDER BY A.INVNO,A.INVDATE,A.ACODE,A.ICODE";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;
                case "TACODE":
                    txtacode.Value = col1;
                    txtAname.Value = col2;
                    btnIcode.Focus();
                    break;
                case "TICODE":
                    if (col1.Length < 2) return;
                    txtIcode.Value = col1;
                    txtIname.Value = col2;

                    txtCpartno.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    txtponum.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    txtPodt.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    txtcustPo.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");
                    btnIcode.Focus();
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "DNCN":
                    txtDnCnCode.Value = col1;
                    txtDnCnName.Value = col2;
                    btnGstClass.Focus();
                    break;
                case "GSTCLASS":
                    txtGstClassCode.Value = col1;
                    txtGstClassName.Value = col2;
                    txtGstClassName.Focus();
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    void fillItems()
    {
        create_tab();
        double d1 = 0;
        dt = new DataTable();
        //PrdRange = " BETWEEN (SYSDATE-600) AND SYSDATE ";
        SQuery = "SELECT a.*,a.iqtyout as dnqty,0 as cust_rej,b.iname,b.cpartno,b.unit,B.HSCODE,a.exc_rate,a.cess_percent,a.iopr,c.num4,c.num5,c.num6,c.num7 FROM IVOUCHER a,item b,typegrp c WHERE trim(a.icode)=trim(B.icode) and trim(b.hscode)=trim(c.acref) and a.BRANCHCD='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + PrdRange + " and trim(a.acode)='" + txtacode.Value + "' and trim(a.icode)='" + txtIcode.Value + "' and trim(A.ponum)||to_char(a.podate,'dd/mm/yyyy')='" + txtponum.Value + txtPodt.Value + "' and c.id='T1' order by a.vchnum ";
        if (frm_cocd == "MEGA") SQuery = "SELECT a.*,a.iqtyout as dnqty,0 as cust_rej,A.PURPOSE AS iname,A.EXC_57F4 as cpartno,b.unit,B.HSCODE,a.exc_rate,a.cess_percent,a.iopr,c.num4,c.num5,c.num6,c.num7 FROM IVOUCHER a,item b,typegrp c WHERE trim(a.icode)=trim(B.icode) and trim(b.hscode)=trim(c.acref) and a.BRANCHCD='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + PrdRange + " and trim(a.acode)='" + txtacode.Value + "' and trim(a.icode)='" + txtIcode.Value + "' and trim(A.ponum)||to_char(a.podate,'dd/mm/yyyy')='" + txtponum.Value + txtPodt.Value + "' and c.id='T1' order by a.vchnum ";
        if (frm_cocd == "BONY") SQuery = "SELECT a.*,a.iqtyout as dnqty,0 as cust_rej,A.PURPOSE AS iname,A.EXC_57F4 AS cpartno,b.unit,B.HSCODE,a.exc_rate,a.cess_percent,a.iopr,c.num4,c.num5,c.num6,c.num7 FROM IVOUCHER a,item b,typegrp c WHERE trim(a.icode)=trim(B.icode) and trim(b.hscode)=trim(c.acref) and a.BRANCHCD='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + PrdRange + " and trim(a.acode)='" + txtacode.Value + "' and trim(a.icode)='" + txtIcode.Value + "' and trim(A.finvno)='" + txtcustPo.Text + "' AND TRIM(A.EXC_57F4)='" + txtCpartno.Value.Trim() + "' and c.id='T1' order by a.vchnum ";
        if (frm_cocd == "SAIP")
        {
            SQuery = "SELECT a.*,(Case when nvl(a.st_modv,0)>0 and nvl(a.st_modv,0)<>nvl(a.iqtyout,0) then nvl(a.st_modv,0) else a.iqtyout end ) as dnqty,nvl(et_paid,0) as cust_rej,A.PURPOSE AS iname,A.EXC_57F4 AS cpartno,b.unit,B.HSCODE,a.exc_rate,a.cess_percent,a.iopr,c.num4,c.num5,c.num6,c.num7,to_char(a.EXC_57F4DT,'YYYYMMDD') AS VDD FROM IVOUCHER a,item b,typegrp c WHERE trim(a.icode)=trim(B.icode) and trim(b.hscode)=trim(c.acref) and a.BRANCHCD='" + frm_mbr + "' and a.type like '4%' and a.EXC_57F4DT " + PrdRange + " and trim(a.acode)='" + txtacode.Value + "' and trim(a.icode)='" + txtIcode.Value + "' and c.id='T1' and nvl(a.REFNUM,'-')!='-' order by a.REFNUM,VDD ";
            SQuery = "SELECT a.*,(Case when nvl(a.st_modv,0)>0 and nvl(a.st_modv,0)<>nvl(a.iqtyout,0) then nvl(a.st_modv,0) else a.iqtyout end ) as dnqty,nvl(et_paid,0) as cust_rej,A.PURPOSE AS iname,A.EXC_57F4 AS cpartno,b.unit,B.HSCODE,a.exc_rate,a.cess_percent,a.iopr,c.num4,c.num5,c.num6,c.num7,to_char(a.EXC_57F4DT,'YYYYMMDD') AS VDD FROM IVOUCHER a,item b,typegrp c WHERE trim(a.icode)=trim(B.icode) and trim(b.hscode)=trim(c.acref) and a.BRANCHCD='" + frm_mbr + "' and a.type like '4%' and a.VCHDATE " + PrdRange + " and trim(a.acode)='" + txtacode.Value + "' and trim(a.icode)='" + txtIcode.Value + "' and c.id='T1' and nvl(a.REFNUM,'-')!='-' order by a.vchnum,VDD ";
        }
        //if(frm_cocd=="SDM")
        //{
        //    SQuery = "SELECT a.branchcd,a.type,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,'-' AS INVNO,null as invdate,trim(a.acode) as acode,trim(a.icode) as icode,a.irate,a.ponum,a.podate,a.iamount,a.iqtyout as dnqty,0 as cust_rej,b.iname,b.cpartno,b.unit,B.HSCODE,a.iexc_addl,a.exc_rate,a.cess_percent,a.iopr,c.num4,c.num5,c.num6,c.num7 FROM IVOUCHER a,item b,typegrp c WHERE trim(a.icode)=trim(B.icode) and trim(b.hscode)=trim(c.acref) and a.BRANCHCD='01' and a.type like '4%' and a.vchdate  between to_date('01/04/2020','dd/mm/yyyy') and to_date('11/02/2021','dd/mm/yyyy') and trim(a.acode)='" + txtacode.Value + "' and trim(a.icode)='" + txtIcode.Value + "' and trim(A.ponum)||to_char(a.podate,'dd/mm/yyyy')='" + txtponum.Value + txtPodt.Value + "' and c.id='T1' order by a.vchnum";
        //    SQuery = "SELECT a.branchcd,a.type,'-' as vchnum,sysdate as vchdate,'-' AS INVNO,sysdate as invdate,a.acode,a.icode,a.irate,a.ponum,a.podate,sum(a.iamount) as iamount,sum(a.iqtyout) as dnqty,0 as cust_rej,b.iname,b.cpartno,b.unit,B.HSCODE,sum(a.iexc_addl) as iexc_addl,a.exc_rate,a.cess_percent,a.iopr,c.num4,c.num5,c.num6,c.num7,a.refnum FROM IVOUCHER a,item b,typegrp c WHERE trim(a.icode)=trim(B.icode) and trim(b.hscode)=trim(c.acref) and a.BRANCHCD='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + PrdRange + " and trim(a.acode)='" + txtacode.Value + "' and trim(a.icode)='" + txtIcode.Value + "' and trim(A.ponum)||to_char(a.podate,'dd/mm/yyyy')='" + txtponum.Value + txtPodt.Value + "' and c.id='T1'  group by a.branchcd,a.type,a.acode,a.icode,a.irate,a.ponum,a.podate,b.iname,b.cpartno,b.unit,B.HSCODE,a.exc_rate,a.cess_percent,a.iopr,c.num4,c.num5,c.num6,c.num7,a.refnum";
        //}
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);      
        if (dt.Rows.Count > 0)
        {
            i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                sg1_dr = sg1_dt.NewRow();
                sg1_dr["sg1_SrNo"] = i;
                sg1_dr["sg1_h1"] = dr["branchcd"].ToString().Trim();
                sg1_dr["sg1_h2"] = dr["type"].ToString().Trim();
                sg1_dr["sg1_h3"] = dr["vchnum"].ToString().Trim();
                sg1_dr["sg1_h4"] = Convert.ToDateTime(dr["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                sg1_dr["sg1_h5"] = dr["acode"].ToString().Trim();
                sg1_dr["sg1_h6"] = dr["icode"].ToString().Trim();

                sg1_dr["sg1_h7"] = fgen.make_double(dr["dnqty"].ToString().Trim()) - fgen.make_double(dr["cust_Rej"].ToString().Trim());

                d1 += fgen.make_double(dr["dnqty"].ToString().Trim()) - fgen.make_double(dr["cust_Rej"].ToString().Trim());

                sg1_dr["sg1_h8"] = dr["irate"].ToString().Trim();
                //new rate
                sg1_dr["sg1_h9"] = 0;
                //diff
                sg1_dr["sg1_h10"] = 0;                
                    sg1_dr["sg1_f1"] = dr["invno"].ToString().Trim();
                    sg1_dr["sg1_f2"] = fgen.make_def_Date(Convert.ToDateTime(dr["invdate"].ToString().Trim()).ToString("dd/MM/yyyy"), vardate);
                
                sg1_dr["sg1_F3"] = dr["ponum"].ToString().Trim();
                sg1_dr["sg1_F4"] = fgen.make_def_Date(Convert.ToDateTime(dr["podate"].ToString().Trim()).ToString("dd/MM/yyyy"), vardate);

                sg1_dr["sg1_f5"] = dr["iamount"].ToString().Trim();

                sg1_dr["sg1_t1"] = dr["EXC_RATE"].ToString().Trim();
                sg1_dr["sg1_t2"] = dr["CESS_PERCENT"].ToString().Trim();
                sg1_dr["sg1_t3"] = dr["IOPR"].ToString().Trim();

                sg1_dr["sg1_t5"] = dr["iname"].ToString().Trim();
                sg1_dr["sg1_t6"] = dr["cpartno"].ToString().Trim();
                sg1_dr["sg1_t7"] = dr["unit"].ToString().Trim();
                sg1_dr["sg1_t8"] = dr["HSCODE"].ToString().Trim();

                if (chktOldTax.Checked)
                {
                    if (dr["num4"].ToString().Trim() == "CG")
                    {
                        sg1_dr["sg1_t9"] = dr["exc_rate"].ToString().Trim();
                        sg1_dr["sg1_t10"] = dr["cess_percent"].ToString().Trim();
                        sg1_dr["sg1_t11"] = 0;
                    }
                    else
                    {
                        sg1_dr["sg1_t9"] = 0;
                        sg1_dr["sg1_t10"] = 0;
                        sg1_dr["sg1_t11"] = dr["exc_rate"].ToString().Trim();
                    }
                }
                else
                {
                    sg1_dr["sg1_t9"] = dr["num4"].ToString().Trim();
                    sg1_dr["sg1_t10"] = dr["num5"].ToString().Trim();
                    sg1_dr["sg1_t11"] = dr["num6"].ToString().Trim();
                }
                sg1_dr["sg1_t12"] = dr["IEXC_ADDL"].ToString().Trim();

                sg1_dr["sg1_t15"] = dr["REFNUM"].ToString().Trim();
                //sg1_dr["sg1_t16"] = dr["EXC_57F4DT"].ToString().Trim();
                /////sg1_dr["sg1_t16"] = "tcs";//trying in saip
                sg1_dt.Rows.Add(sg1_dr);
                i++;
            }
        }
        sg1.DataSource = sg1_dt;
        sg1.DataBind();

        lblRowCount.Text = "Total Rows Showing : " + sg1.Rows.Count.ToString() + " , Total Qty : " + d1;

        ViewState["dtn"] = sg1_dt;

        setColHeadings();

        btnDNCN.Focus();
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "SELECT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL11 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE FROM SCRATCH2 A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + DateRange + " and a.num10>0 ORDER BY A.COL33";
            SQuery = "SELECT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT FROM SCRATCH2 A,ivoucher B WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY') AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + DateRange + " and a.num10>0 ORDER BY A.COL33";
            SQuery = "SELECT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.COL33 AS cust_pono,A.COL2 AS inv_no,a.col3 as inv_dt,a.acode as partycode,c.aname as party,a.icode as erpcode,d.iname as product,a.col6 as qty,a.col7 as oldrate,a.col8 as newrate,a.col9 as rate_diff,a.col13 as pono,a.col14 as pordt,a.col15 as inv_value,to_number(a.col16)+to_number(a.col17) as tax_per,a.ent_by,a.ent_dt,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,B.TYPE AS NOTE_TYPE,B.BRANCHCD AS B_CODE FROM SCRATCH2 A,ivoucher B,famst c,item d WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL2)||TO_CHAR(TO_DATE(A.COL3,'DD/MM/YY'),'DD/MM/YYYY')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY') and trim(A.acode)=trim(c.acode) and trim(a.icode)=trim(d.icode) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + DateRange + " and b.type in ('58','59') and a.num10>0  ORDER BY A.COL33";


            //SQuery = "SELECT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.COL33 AS cust_pono,A.COL2 AS inv_no,a.col3 as inv_dt,a.acode as partycode,c.aname as party,b.icode as erpcode,B.PURPOSE as product,B.EXC_57F4,a.col6 as qty,a.col7 as oldrate,a.col8 as newrate,a.col9 as rate_diff,a.col13 as pono,a.col14 as pordt,a.col15 as inv_value,to_number(a.col16)+to_number(a.col17) as tax_per,a.ent_by,a.ent_dt,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.iamount as amt_wot,b.spexc_amt as amt,B.TYPE AS NOTE_TYPE,B.BRANCHCD AS B_CODE FROM SCRATCH2 A,ivoucher B,famst c WHERE TRIM(A.ACODE)||TRIM(A.COL2)||TO_CHAR(TO_DATE(A.COL3,'DD/MM/YYYY'),'DD/MM/YYYY')||TRIM(A.COL33)||trim(a.col5)=TRIM(B.ACODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||TRIM(B.LOCATION)||trim(b.icode) AND A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(B.BTCHNO) and trim(A.acode)=trim(c.acode) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + DateRange + " and b.type in ('58','59') and a.num10>0 ORDER BY A.COL33";
            SQuery = "SELECT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.COL33 AS cust_pono,A.COL2 AS inv_no,a.col3 as inv_dt,a.acode as partycode,c.aname as party,b.icode as erpcode,B.PURPOSE as product,B.EXC_57F4,a.col6 as qty,a.col7 as oldrate,a.col8 as newrate,a.col9 as rate_diff,a.col13 as pono,a.col14 as pordt,a.col15 as inv_value,to_number(a.col16)+to_number(a.col17) as tax_per,a.ent_by,a.ent_dt,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.iamount as amt_wot,b.spexc_amt as amt,B.TYPE AS NOTE_TYPE,B.BRANCHCD AS B_CODE FROM SCRATCH2 A,ivoucher B,famst c WHERE TRIM(A.ACODE)||TRIM(A.COL2)||TO_CHAR(TO_DATE(A.COL3,'DD/MM/YYYY'),'DD/MM/YYYY')||TRIM(A.COL33)||trim(a.col5)||trim(a.col6)=TRIM(B.ACODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||TRIM(B.LOCATION)||trim(b.icode)||b.iqty_chl AND A.BRANCHCD||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')=TRIM(B.BTCHNO) and trim(A.acode)=trim(c.acode) and b.type in ('58','59') AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + DateRange + " ORDER BY A.COL33";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
            return;
        }
        else if (hffield.Value == "ListS")
        {
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            //SQuery = "select FSTR AS VAL1,SUM(AMT) AS UPL_AMT,SUM(AMT2) AS IVCH_AMT,SUM(AMT3) AS VCH_AMT FROM (SELECT branchcd||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') AS fstr ,SUM(IS_NUMBER(col6)*IS_NUMBER(col9)) AS AMT,0 AS AMT2,0 AS AMT3 from scratch2 where branchcd='" + frm_mbr + "' and type='DC' and vchdate " + DateRange + " group by branchcd||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') union all SELECT a.btchno,0 AS AMT,SUM(a.IAMOUNT) AS AMT2,sum(b.dramt+b.cramt) AS AMT3 FROM IVOUCHER a ,voucher b where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) =b.branchcd||b.type||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy')||trim(b.Rcode) and a.BRANCHCD='" + frm_mbr + "' and a.type in ('58','59') and a.vchdate " + DateRange + " and b.srno='50' group by a.btchno) GROUP BY FSTR ORDER BY FSTR";
            SQuery = "select FSTR AS VAL1,SUM(AMT) AS UPL_AMT,SUM(AMT2) AS IVCH_AMT,SUM(AMT3) AS VCH_AMT FROM (SELECT branchcd||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') AS fstr ,SUM(IS_NUMBER(col6)*IS_NUMBER(col9)) AS AMT,0 AS AMT2,0 AS AMT3 from scratch2 where branchcd='" + frm_mbr + "' and type='DC' and vchdate " + PrdRange + " group by branchcd||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') union all SELECT a.branchcd||trim(a.col1) as fstr,0 AS AMT,SUM(a.IAMOUNT) AS AMT2,sum(b.dramt+b.cramt) AS AMT3 FROM IVOUCHER a ,voucher b where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.acode) =b.branchcd||b.type||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy')||trim(b.Rcode) and a.BRANCHCD='" + frm_mbr + "' and a.type in ('58','59') and a.vchdate " + PrdRange + " and b.srno='50' group by a.branchcd||trim(a.col1)) GROUP BY FSTR ORDER BY FSTR";

            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
            return;
        }
        else if (hffield.Value == "TICODE")
        {
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            fillItems();
        }
        else if (hffield.Value == "ANEXX") //this is summary debit notecredit note list
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "select branchcd,type,vchnum,to_Char(vchdate,'dd/mm/yyyy') as vchdate,acode,aname as customer,FDDR1,FADDR2,faddr3,fstate,fstatecode,fgirno as pan_no,vencode,fgst_no as gst_no,iname as Material,cpartno as Material_Description,iunit,hscode,icode,iopr,sum(iqty_chl) as iqty_chl, purpose,desc_,naration,'-' AS ponum,NULL AS podate,'-' as invno,sysdate as invdate,sum(iamount) as iamount,0 as irate,'-' AS finvno,to_Char(exc_57f4dt,'dd/mm/yyyy') as exc_57f4dt,exc_rate ,sum(exc_amt) as exc_amt,refnum,iexc_addl,sum(cess_pu)  as cess_pu,cess_percent,sum(spexc_rate) as spexc_rate,sum(spexc_amt) as spexc_amt,sum(psize) as tcsamt,btchdt,form31,mfgdt,expdt from(SELECT (case when upper(A.naration) like 'SUPPL%' or substr(f.aname,1,4)='HERO' OR substr(f.aname,1,4)='TATA' then 'SUPPLEMENTARY INVOICE / DEBIT NOTE' ELSE 'DEBIT NOTE' end) as header,(CASE WHEN TRIM(NVL(F.PNAME,'-'))='-' THEN F.ANAME ELSE F.PNAME END) AS ANAME,F.ADDR1 AS FDDR1,F.ADDR2 AS FADDR2,F.ADDR3 AS FADDR3,F.STATEN AS FSTATE,SUBSTR(F.GST_NO,0,2) AS FSTATECODE,F.GIRNO AS FGIRNO,F.VENCODE,F.GST_NO AS FGST_NO,(CASE WHEN LENGTH(A.PURPOSE)>2 THEN A.PURPOSE ELSE I.INAME END) AS INAME,I.cpartno,I.UNIT AS IUNIT,I.HSCODE,A.* FROM IVOUCHER A,FAMST F ,ITEM I WHERE  TRIM(A.ACODE)=TRIM(F.ACODE) AND TRIM(A.ICODE)=TRIM(I.ICODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='59' and a.vchdate  " + PrdRange + "  order by a.vchnum ) group by header,aname,FDDR1,faddr2,faddr3,fstate,fstatecode,fgirno,vencode,fgst_no,iname,cpartno,iunit,hscode,branchcd,type,vchnum,vchdate,acode,icode,iopr, purpose,desc_,naration,exc_57f4dt,exc_rate,refnum,iexc_addl,cess_percent,btchdt,form31,mfgdt,expdt";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevelJS("Annexure of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
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
                last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and " + doc_df.Value + " " + DateRange + " ", "ldt");
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
                        //save_fun3();

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
                            save_it = "Y";

                            if (save_it == "Y")
                            {
                                i = 0;
                                do
                                {
                                    if (frm_cocd == "BONY") col3 = txtcustPo.Text.Trim();
                                    else col3 = txtponum.Value.Trim();

                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), col3, frm_uname);
                                    if (i > 20)
                                    {
                                        fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
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

                        ViewState["refNo"] = frm_vnum;
                        save_fun();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        col3 = frm_vnum + txtvchdate.Text;

                        save_fun2();

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + frm_vnum + " Updated Successfully");
                            //cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            //fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "vipin@Tejaxo.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + frm_vnum + " Saved Successfully ");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
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
        sg1_dt.Columns.Add(new DataColumn("sg1_t17", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t18", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t19", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_t20", typeof(string)));
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
        if (sg1_dt == null) create_tab();
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
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
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
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        DataTable dtW = (DataTable)ViewState["dtn"];
        if (dtW != null)
        {
            DataView dvW = new DataView(dtW);
            dvW.Sort = "SG1_H6";
            dtW = new DataTable();
            dtW = dvW.ToTable();

            foreach (DataRow gr1 in dtW.Rows)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();
                oporow["ICODE"] = gr1["SG1_H6"].ToString().Trim();
                oporow["ACODE"] = txtacode.Value.Trim();

                for (int K = 1; K < 20; K++)
                {
                    oporow["COL" + K] = gr1[K].ToString().Trim();
                }

                if (fgen.make_double(gr1["sg1_h10"].ToString().Trim()) > 0) oporow["NUM10"] = 1;
                else oporow["NUM10"] = 0;

                if (frm_cocd == "BONY") oporow["col33"] = txtcustPo.Text.Trim();
                else oporow["col33"] = txtponum.Value.Trim();

                oporow["col35"] = txtPodt.Value.Trim();

                oporow["REMARKS"] = txtrmk.Text.Trim();

                // ADD ON 02 MAY 2018 BY MADHVI
                oporow["col46"] = txtDnCnCode.Value.Trim();
                oporow["col47"] = txtGstClassCode.Value.Trim();
                oporow["num1"] = fgen.make_double(txtCgst.Value.Trim());
                oporow["num2"] = fgen.make_double(txtSgst.Value.Trim());
                // -----------------------------

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

    void save_fun2()
    {
        string sal_code = "", par_code = "", tax_code = "", tax_code2 = "", schg_code = "", iopr = "", nVty = "";
        double dVal = 0; double dVal1 = 0; double dVal2 = 0; double qty = 0; double dVal1Tot = 0; ; double dVal2Tot = 0;
        string multiinv_vnum = ""; string branchcd = ""; string newVnum = ""; string vinvno = ""; string vinvdate = "";
        string mhd = ""; double tcsrate = 0; double gstval = 0; double basic = 0; double tcsamt = 0;
        string saveTo = "Y"; string status = ""; string batchNo = "";
        string Vgstno_cntrl = "";//CONTROL FOR Long Voucher Number for 58/59 series vouchers.
        string Vgstno_paramdt = "";
        string Vgstvch_no = "";//var for saving gstvch_no in both table
        string Saving_vch_ivch = ""; string saving_inv = ""; string invRmrk = ""; string multi_invno = "";
        DataTable dtparty = new DataTable();
        dtparty = fgen.getdata(frm_qstr, frm_cocd, "select trim(Acode) as acode,nvl(status,'-') as status,nvl(CESSRATE,0) as tcsrate from famst where substr(trim(Acode),1,2)='16' order by acode asc");
        multiinv_vnum = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ENABLE_YN FROM CONTROLS WHERE ID='O43'", "ENABLE_YN");
       // multiinv_vnum = "Y";//=============CMNT THIS
        Vgstno_cntrl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_lvch_5859");
     
      //  Vgstno_cntrl = "Y";//CMNT THIS
        Vgstno_paramdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_lvch_5859_date"); //fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='O37'", "PARAMS");
        Saving_vch_ivch = fgen.seek_iname(frm_qstr, frm_cocd, "select ENABLE_YN from stock where id='M338'", "ENABLE_YN");//////////control for saving invno,gstvchno,originv_no,originv_dt in ivch and vch table
       // Saving_vch_ivch = "Y";//CMNT THIS...THIS IS FOR TESTING
        //DataTable dtSale = new DataTable();
        //dtSale = fgen.getdata(frm_qstr, frm_cocd, "SELECT distinct branchcd,TRIM(VCHNUM)||TO_cHAR(VCHDATE,'DD/MM/YYYY') AS FSTR FROM SALE WHERE BRANCHCD!='DD' AND TYPE LIKE '4%' AND VCHDATE " + DateRange + " order by fstr ");
        DataTable dtW = (DataTable)ViewState["dtn"];
        if (dtW != null)
        {
            DataView dvW = new DataView(dtW);
            dvW.Sort = "SG1_H6";
            dtW = new DataTable();
            dtW = dvW.ToTable();
            int l = 1;          

            if (frm_qstr.Contains("^"))
            {
                if (frm_cocd != frm_qstr.Split('^')[0].ToString())
                {
                    frm_cocd = frm_qstr.Split('^')[0].ToString();
                }
            }
            //=====FOR TCS
            dtparty = fgen.getdata(frm_qstr, frm_cocd, "select trim(Acode) as acode,nvl(status,'-') as status,nvl(CESSRATE,0) as tcsrate from famst where substr(trim(Acode),1,2)='16' order by acode asc");
            int multicont = 1;
            if (multiinv_vnum == "Y")
            {
                #region
                DataView dvW1 = new DataView(dtW);
                dvW1.Sort = "SG1_H3";
                DataTable dvwsort = new DataTable();
                dvwsort = dvW1.ToTable();
                oDS1 = new DataSet();
                oporow1 = null;
                oDS1 = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");
                foreach (DataRow drw in dtW.Rows)
                {
                    saveTo = "Y";
                    if (saveTo == "Y")
                    {
                        qty = fgen.make_double(drw["sg1_h7"].ToString().Trim());
                        if (qty > 0)
                        {
                            #region Complete Save Function
                            {
                                branchcd = drw["sg1_h1"].ToString().Trim();
                                invRmrk = "";                                
                                dVal = 0;
                                dVal1 = 0;
                                dVal2 = 0;
                                //basic = 0;
                                //gstval = 0;
                                //*******************
                                oporow1 = oDS1.Tables[0].NewRow();
                                oporow1["BRANCHCD"] = branchcd;

                                if (fgen.make_double(drw["sg1_h10"].ToString().Trim()) > 0) nVty = "59";
                                else nVty = "58";
                                //nVty = "59";
                                oporow1["TYPE"] = nVty;
                                if (multicont == 1) frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, "IVOUCHER", "VCHNUM", "VCHDATE", branchcd, nVty, txtvchdate.Text.Trim(), frm_uname, frm_formID);
                                else newVnum = "N";

                                oporow1["SRNO"] = multicont;
                                batchNo = drw["sg1_f3"].ToString().Trim();
                                if (frm_cocd == "BONY") batchNo = txtcustPo.Text.Trim();

                                oporow1["LOCATION"] = batchNo;

                                oporow1["vchnum"] = frm_vnum;
                                oporow1["vchdate"] = txtvchdate.Text.Trim();

                                oporow1["ACODE"] = txtacode.Value.Trim();
                                //=========
                                status = fgen.seek_iname_dt(dtparty, "acode='" + txtacode.Value.Trim() + "'", "status");
                                tcsrate = fgen.make_double(fgen.seek_iname_dt(dtparty, "acode='" + txtacode.Value.Trim() + "'", "tcsrate"));
                               
                                oporow1["VCODE"] = txtacode.Value.ToString().Trim();
                                oporow1["ICODE"] = txtIcode.Value.Trim();

                                oporow1["MATTYPE"] = txtGstClassCode.Value;
                                oporow1["POTYPE"] = txtDnCnCode.Value;

                                oporow1["GENUM"] = "S";
                                oporow1["GEDATE"] = vardate;
                                oporow1["rgpdate"] = vardate;

                                oporow1["REC_ISS"] = "C";

                                oporow1["iqtyout"] = 0;
                                oporow1["iqtyin"] = 0;
                                oporow1["IQTY_CHL"] = qty;
                                oporow1["PURPOSE"] = drw["sg1_T5"].ToString().Trim();

                                if (nVty == "59") invRmrk = "Debit Note Against PO No. :" + txtcustPo.Text;
                                else invRmrk = "Credit Note Against PO No. :" + txtcustPo.Text;

                                invRmrk += (char)13 + txtrmk.Text.Trim();

                                oporow1["NARATION"] = invRmrk;

                                oporow1["finvno"] = drw["sg1_f3"].ToString().Trim();
                                oporow1["PODATE"] = fgen.make_def_Date(Convert.ToDateTime(drw["sg1_f4"].ToString().Trim()).ToString("dd/MM/yyyy"), vardate);

                                if (Vgstno_cntrl == "Y" && Vgstno_paramdt.Trim().Length >= 10)
                                {
                                    if (Convert.ToDateTime(txtvchdate.Text.Trim()) >=Convert.ToDateTime(Vgstno_paramdt.ToString().Trim()))
                                    {
                                        //if vchdate>=Vgstno_paramdt then it will save else save '-' ......
                                        Vgstvch_no = frm_CDT1.Substring(8, 2) + frm_mbr + nVty + "-" + frm_vnum;
                                    }
                                }
                                else
                                {
                                    Vgstvch_no = frm_vnum;
                                }
                                 vinvno = fgen.padlc(Convert.ToInt32(drw["sg1_h3"].ToString().Trim()), 6);
                                 vinvdate = fgen.make_def_Date(Convert.ToDateTime(drw["sg1_h4"].ToString().Trim()).ToString("dd/MM/yyyy"), vardate);
                                
                               
                                 oporow1["INVNO"] = vinvno;
                                oporow1["INVDATE"] = vinvdate;
                                oporow1["GSTVCH_NO"] = Vgstvch_no.Trim();
                                oporow1["UNIT"] = "NOS";

                                double Rate = fgen.make_double(drw["sg1_h10"].ToString().Trim());
                                if (Rate < 0) Rate = -1 * Rate;
                                oporow1["IRATE"] = Rate;

                                //OLD RATE + " ~ " + NEW RATE
                                oporow1["PNAME"] = fgen.make_double(drw["sg1_h8"].ToString().Trim(), 2) + "~" + fgen.make_double(drw["sg1_h9"].ToString().Trim(), 2);

                                dVal = Math.Round(qty * (fgen.make_double(drw["sg1_h10"].ToString().Trim())), 2);
                                if (dVal < 0) dVal = -1 * dVal;
                                oporow1["IAMOUNT"] = dVal;
                                //------for inv wise baisc value
                                basic += dVal;
                                
                                oporow1["NO_CASES"] = drw["sg1_t8"].ToString().Trim();
                                oporow1["EXC_57F4"] = drw["sg1_t6"].ToString().Trim();

                                //oporow1["IEXC_ADDL"] = drw["sg1_t12"].ToString().Trim();
                                // change due to MEGA
                                oporow1["IEXC_ADDL"] = 0;
                                double toolAmort = fgen.make_double(drw["sg1_t12"].ToString().Trim());
                                toolAmort = 0;

                                if ((drw["SG1_T3"].ToString().Trim()) == "IG")
                                {
                                    oporow1["IOPR"] = "IG";
                                    iopr = "IG";

                                    oporow1["EXC_RATE"] = drw["sg1_t11"].ToString().Trim();
                                    dVal1 = Math.Round(dVal * (fgen.make_double(drw["sg1_t11"].ToString().Trim()) / 100), 2);
                                    dVal1 += toolAmort;
                                    oporow1["EXC_AMT"] = Math.Round(dVal1, 2);
                                }
                                else
                                {
                                    iopr = "CG";
                                    oporow1["IOPR"] = "CG";

                                    oporow1["EXC_RATE"] = drw["sg1_t9"].ToString().Trim();
                                    dVal1 = Math.Round(dVal * (fgen.make_double(drw["sg1_t9"].ToString().Trim()) / 100), 2);
                                    dVal1Tot += Math.Round(dVal1, 2);
                                    dVal1 += toolAmort;
                                    oporow1["EXC_AMT"] = Math.Round(dVal1, 2);

                                    oporow1["CESS_PERCENT"] = drw["sg1_t10"].ToString().Trim();
                                    dVal2 = Math.Round(dVal * (fgen.make_double(drw["sg1_t10"].ToString().Trim()) / 100), 2);
                                    dVal2Tot += Math.Round(dVal2, 2);
                                    dVal2 += toolAmort;
                                    oporow1["CESS_PU"] = Math.Round(dVal2, 2);
                                }
                                //---------gst total
                               // gstval += Math.Round(dVal1, 2) + Math.Round(dVal2, 2);//exc_amt+cess_pu          
                                gstval += Math.Round(dVal1, 2) + Math.Round(dVal2, 2);//exc_amt+cess_pu 

                                oporow1["STORE"] = "N";
                                oporow1["MORDER"] = 1;
                                //oporow1["SPEXC_RATE"] = dVal;
                                oporow1["SPEXC_RATE"] = 0;
                                //oporow1["SPEXC_AMT"] = dVal + dVal1 + dVal2;
                                oporow1["SPEXC_AMT"] = 0;
                                oporow1["psize"] = 0;
                                oporow1["gsm"] = 0;
                                //oporow1["col1"] = col3;

                                if (frm_cocd == "SAIP")
                                {
                                    oporow1["REFNUM"] = drw["sg1_t15"].ToString().Trim();
                                    oporow1["EXC_57F4DT"] = fgen.make_def_Date(drw["sg1_t16"].ToString().Trim(), vardate);
                                }
                                else oporow1["REFNUM"] = "-";

                                //*******************
                                par_code = txtacode.Value.Trim();
                                if (iopr == "CG")
                                {
                                    if (tax_code.Length <= 0)
                                    {
                                        tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A77'", "PARAMS");
                                        sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A77'", "PARAMS2");
                                        tax_code2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A78'", "PARAMS");
                                    }
                                }
                                else
                                {
                                    if (tax_code.Length <= 0)
                                    {
                                        tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A79'", "PARAMS");
                                        sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A79'", "PARAMS2");
                                    }
                                }
                                if (schg_code.Length <= 0)
                                    schg_code = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(params) as param from controls where id='A41'", "param");

                                //***********************

                                oporow1["RCODE"] = sal_code;

                                oporow1["btchno"] = frm_mbr + ViewState["refNo"].ToString() + txtvchdate.Text.Trim();

                                if (edmode.Value == "Y")
                                {
                                    oporow1["eNt_by"] = ViewState["entby"].ToString();
                                    oporow1["eNt_dt"] = ViewState["entdt"].ToString();
                                    oporow1["edt_by"] = frm_uname;
                                    oporow1["edt_dt"] = vardate;
                                }
                                else
                                {
                                    oporow1["eNt_by"] = frm_uname;
                                    oporow1["eNt_dt"] = vardate;
                                    oporow1["edt_by"] = "-";
                                    oporow1["eDt_dt"] = vardate;
                                }
                                //=========tcs
                                                                                                                   
                                oDS1.Tables[0].Rows.Add(oporow1);                               
                                multicont++;
                                l++;                              
                            #endregion
                            }                            
                        }
                    }
                }
                if (chktcs.Checked == true)
                {
                    tcsamt += (basic + gstval) * tcsrate / 100;
                    oDS1.Tables[0].Rows[0]["gsm"] = tcsrate;
                    oDS1.Tables[0].Rows[0]["psize"] = tcsamt;
                    oDS1.Tables[0].Rows[0]["spexc_amt"] = basic + gstval + tcsamt;
                }
                else
                {
                    tcsamt += 0;
                    oDS1.Tables[0].Rows[0]["gsm"] = 0;
                    oDS1.Tables[0].Rows[0]["psize"] = 0;
                    oDS1.Tables[0].Rows[0]["spexc_amt"] = basic + gstval;
                }  
                //tcsamt += (basic + gstval) * tcsrate / 100;     
                //oDS1.Tables[0].Rows[0]["gsm"] = tcsrate;
                //oDS1.Tables[0].Rows[0]["psize"] = tcsamt;
                //oDS1.Tables[0].Rows[0]["spexc_amt"] = basic + gstval + tcsamt;
                oDS1.Tables[0].Rows[0]["spexc_rate"] = basic;
                fgen.save_data(frm_qstr, frm_cocd, oDS1, "IVOUCHER");
                //======
                par_code = txtacode.Value.Trim();
                //=================
                string tcscode = "";
                if (status == "Y")
                {
                    tcsamt = (basic + gstval) * tcsrate / 100;//gt tot
                    tcscode = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A95'", "PARAMS");
                }
                else
                {
                    tcsamt = 0;
                    tcscode = "-";
                }
                //==============
                #region Voucher Saving
                batchNo = "W" + batchNo;
                string app_by = "-"; string vari_vch = "-";
                if (frm_cocd == "LOGW" || frm_cocd == "ROOP")
                {
                    app_by = frm_uname;
                    vari_vch = "Y";
                }
                if (branchcd.Length > 1)
                {
                    //if (Saving_vch_ivch == "Y")
                    //{
                        #region Voucher Saving ..
                        if (nVty == "58")
                        {
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, sal_code, par_code, fgen.make_double(basic, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 2, tax_code, par_code, fgen.make_double(dVal1Tot, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal1, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));

                            if (tax_code2.Length > 0 && dVal2 != 0)
                            {
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 3, tax_code2, par_code, fgen.make_double(dVal2Tot, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal2, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                            }
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, par_code, sal_code, 0, fgen.make_double(basic + dVal1Tot + dVal2Tot + tcsamt, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal + dVal1 + dVal2, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                            //----for tcs saving in voucher     
                            if (chktcs.Checked == true)
                            {
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 4, tcscode, par_code, tcsamt, 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                            }
                        }
                        else
                        {
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, sal_code, fgen.make_double(basic + dVal1Tot + dVal2Tot + tcsamt, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal + dVal1 + dVal2, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, sal_code, par_code, 0, fgen.make_double(basic, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 51, tax_code, par_code, 0, fgen.make_double(dVal1Tot, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal1, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));

                            if (tax_code2.Length > 0 && dVal2 != 0)
                            {
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 52, tax_code2, par_code, 0, fgen.make_double(dVal2Tot, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal2, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                            }
                            //----for tcs saving in voucher   
                            if (chktcs.Checked == true)
                            {
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 4, tcscode, par_code, 0, tcsamt, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                            }
                        }
                        #endregion
                    // }
                    #region below M338 control cond.
                    //else//control is N
                    //{
                    //    #region Voucher Saving ..
                    //    if (nVty == "58")
                    //    {
                    //        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, sal_code, par_code, fgen.make_double(basic, 2), 0, vinvno, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                    //        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 2, tax_code, par_code, fgen.make_double(dVal1Tot, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal1, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));

                    //        if (tax_code2.Length > 0 && dVal2 != 0)
                    //        {
                    //            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 3, tax_code2, par_code, fgen.make_double(dVal2Tot, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal2, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                    //        }
                    //        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, par_code, sal_code, 0, fgen.make_double(basic + dVal1Tot + dVal2Tot + tcsamt, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal + dVal1 + dVal2, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                    //        //----for tcs saving in voucher     
                    //        if (chktcs.Checked == true)
                    //        {
                    //            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 53, tcscode, par_code, tcsamt, 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                    //        }
                           
                    //    }
                    //    else
                    //    {
                    //        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, sal_code, fgen.make_double(basic + dVal1Tot + dVal2Tot + tcsamt, 2), 0, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal + dVal1 + dVal2, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                    //        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, sal_code, par_code, 0, fgen.make_double(basic, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                    //        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 51, tax_code, par_code, 0, fgen.make_double(dVal1Tot, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal1, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));

                    //        if (tax_code2.Length > 0 && dVal2 != 0)
                    //        {
                    //            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 52, tax_code2, par_code, 0, fgen.make_double(dVal2Tot, 2), frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal2, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                    //        }
                    //        //----for tcs saving in voucher   
                    //        if (chktcs.Checked == true)
                    //        {
                    //            fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 53, tcscode, par_code, 0, tcsamt, frm_mbr + nVty + frm_vnum, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                    //        }
                    //    }
                    //    #endregion
                    //} 
                    #endregion
                }
                #endregion
                #endregion
            }

            else
            {
                foreach (DataRow drw in dtW.Rows)
                {
                    saveTo = "Y";
                    if (saveTo == "Y")
                    {
                        qty = fgen.make_double(drw["sg1_h7"].ToString().Trim());
                        if (qty > 0)
                        {
                            #region Complete Save Function
                            {
                                branchcd = drw["sg1_h1"].ToString().Trim();
                                 invRmrk = "";
                                oDS1 = new DataSet();
                                oporow1 = null;
                                oDS1 = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");

                                gstval = 0;
                                basic = 0;
                                dVal = 0;
                                dVal1 = 0;
                                dVal2 = 0;

                                //*******************

                                oporow1 = oDS1.Tables[0].NewRow();
                                oporow1["BRANCHCD"] = branchcd;

                                if (fgen.make_double(drw["sg1_h10"].ToString().Trim()) > 0) nVty = "59";
                                else nVty = "58";

                                oporow1["TYPE"] = nVty;

                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, "IVOUCHER", "VCHNUM", "VCHDATE", branchcd, nVty, txtvchdate.Text.Trim(), frm_uname, frm_formID);

                                batchNo = drw["sg1_f3"].ToString().Trim();
                                if (frm_cocd == "BONY") batchNo = txtcustPo.Text.Trim();

                                oporow1["LOCATION"] = batchNo;

                                oporow1["vchnum"] = frm_vnum;
                                oporow1["vchdate"] = txtvchdate.Text.Trim();

                                oporow1["ACODE"] = txtacode.Value.Trim();
                                //==============
                                status = fgen.seek_iname_dt(dtparty, "acode='" + txtacode.Value.Trim() + "'", "status");
                                tcsrate = fgen.make_double(fgen.seek_iname_dt(dtparty, "acode='" + txtacode.Value.Trim() + "'", "tcsrate"));
                                //if (status == "Y")
                                //{
                                //    oporow1["gsm"] = tcsrate;
                                //}
                                //else
                                //{
                                //    oporow1["gsm"] = 0;
                                //}
                                if (chktcs.Checked == true)
                                {
                                    if (status == "Y")
                                    {
                                        oporow1["gsm"] = tcsrate;
                                    }
                                    else
                                    {
                                        oporow1["gsm"] = 0;
                                    }
                                }
                                else
                                {
                                    oporow1["gsm"] = 0;
                                }
                                //=========
                                oporow1["VCODE"] = txtacode.Value.ToString().Trim();
                                oporow1["ICODE"] = txtIcode.Value.Trim();

                                oporow1["MATTYPE"] = txtGstClassCode.Value;
                                oporow1["POTYPE"] = txtDnCnCode.Value;

                                oporow1["GENUM"] = "S";
                                oporow1["GEDATE"] = vardate;
                                oporow1["rgpdate"] = vardate;

                                oporow1["REC_ISS"] = "C";

                                oporow1["iqtyout"] = 0;
                                oporow1["iqtyin"] = 0;
                                oporow1["IQTY_CHL"] = qty;
                                oporow1["PURPOSE"] = drw["sg1_T5"].ToString().Trim();

                                if (nVty == "59") invRmrk = "Debit Note Against PO No. :" + txtcustPo.Text;
                                else invRmrk = "Credit Note Against PO No. :" + txtcustPo.Text;

                                invRmrk += (char)13 + txtrmk.Text.Trim();

                                oporow1["NARATION"] = invRmrk;

                                oporow1["finvno"] = drw["sg1_f3"].ToString().Trim();
                                oporow1["PODATE"] = fgen.make_def_Date(Convert.ToDateTime(drw["sg1_f4"].ToString().Trim()).ToString("dd/MM/yyyy"), vardate);

                                if (Vgstno_cntrl == "Y" && Vgstno_paramdt.Trim().Length >= 10)
                                {
                                    if (Convert.ToDateTime(txtvchdate.Text.Trim()) >= Convert.ToDateTime(Vgstno_paramdt.ToString().Trim()))
                                    {
                                        //if vchdate>=Vgstno_paramdt then it will save else save '-' ......
                                        Vgstvch_no = frm_CDT1.Substring(8, 2) + frm_mbr + nVty + "-" + frm_vnum;                                       
                                    }
                                }
                                else
                                {
                                    Vgstvch_no = frm_mbr + nVty + frm_vnum;
                                }
                                vinvno = fgen.padlc(Convert.ToInt32(drw["sg1_h3"].ToString().Trim()), 6);
                                vinvdate = fgen.make_def_Date(Convert.ToDateTime(drw["sg1_h4"].ToString().Trim()).ToString("dd/MM/yyyy"), vardate);
                                //only voucher me invno me long no jayega
                                 oporow1["INVNO"] = fgen.padlc(Convert.ToInt32(drw["sg1_h3"].ToString().Trim()), 6);
                                 oporow1["INVDATE"] = vinvdate;
                                 oporow1["GSTVCH_NO"] = Vgstvch_no.Trim();

                                oporow1["UNIT"] = "NOS";

                                double Rate = fgen.make_double(drw["sg1_h10"].ToString().Trim());
                                if (Rate < 0) Rate = -1 * Rate;
                                oporow1["IRATE"] = Rate;

                                //OLD RATE + " ~ " + NEW RATE
                                oporow1["PNAME"] = fgen.make_double(drw["sg1_h8"].ToString().Trim(), 2) + "~" + fgen.make_double(drw["sg1_h9"].ToString().Trim(), 2);

                                dVal = Math.Round(qty * (fgen.make_double(drw["sg1_h10"].ToString().Trim())), 2);
                                if (dVal < 0) dVal = -1 * dVal;
                                oporow1["IAMOUNT"] = dVal;
                                basic += dVal;
                                

                                oporow1["NO_CASES"] = drw["sg1_t8"].ToString().Trim();
                                oporow1["EXC_57F4"] = drw["sg1_t6"].ToString().Trim();

                                //oporow1["IEXC_ADDL"] = drw["sg1_t12"].ToString().Trim();
                                // change due to MEGA
                                oporow1["IEXC_ADDL"] = 0;
                                double toolAmort = fgen.make_double(drw["sg1_t12"].ToString().Trim());
                                toolAmort = 0;

                                if ((drw["SG1_T3"].ToString().Trim()) == "IG")
                                {
                                    oporow1["IOPR"] = "IG";
                                    iopr = "IG";

                                    oporow1["EXC_RATE"] = drw["sg1_t11"].ToString().Trim();
                                    dVal1 = Math.Round(dVal * (fgen.make_double(drw["sg1_t11"].ToString().Trim()) / 100), 2);
                                    dVal1 += toolAmort;
                                    oporow1["EXC_AMT"] = Math.Round(dVal1, 2);
                                }
                                else
                                {
                                    iopr = "CG";
                                    oporow1["IOPR"] = "CG";

                                    oporow1["EXC_RATE"] = drw["sg1_t9"].ToString().Trim();
                                    dVal1 = Math.Round(dVal * (fgen.make_double(drw["sg1_t9"].ToString().Trim()) / 100), 2);
                                    dVal1 += toolAmort;
                                    oporow1["EXC_AMT"] = Math.Round(dVal1, 2);

                                    oporow1["CESS_PERCENT"] = drw["sg1_t10"].ToString().Trim();
                                    dVal2 = Math.Round(dVal * (fgen.make_double(drw["sg1_t10"].ToString().Trim()) / 100), 2);
                                    dVal2 += toolAmort;
                                    oporow1["CESS_PU"] = Math.Round(dVal2, 2);
                                }
                                gstval += Math.Round(dVal1, 2) + Math.Round(dVal2, 2);//exc_amt+cess_pu                              
                             
                                //if (status == "Y")
                                //{
                                //    tcsamt = (basic + gstval) * tcsrate / 100;//gt tot
                                //    oporow1["PSIZE"] = tcsamt;
                                //}
                                //else
                                //{
                                //    oporow1["PSIZE"] = 0;
                                //}
                                if (chktcs.Checked == true)
                                {
                                    if (status == "Y")
                                    {
                                        tcsamt = (basic + gstval) * tcsrate / 100;//gt tot
                                        oporow1["PSIZE"] = tcsamt;
                                    }
                                    else
                                    {
                                        oporow1["PSIZE"] = 0;
                                    }
                                }
                                else
                                {
                                    oporow1["PSIZE"] = 0;
                                }
                                string tcscode = "";                            
                               tcscode = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A95'", "PARAMS");
                               
                                oporow1["STORE"] = "N";
                                oporow1["MORDER"] = 1;
                                oporow1["SPEXC_RATE"] = dVal;
                                if (chktcs.Checked == true)
                                {
                                    oporow1["SPEXC_AMT"] = dVal + dVal1 + dVal2 + tcsamt;
                                }
                                else
                                {
                                    oporow1["SPEXC_AMT"] = dVal + dVal1 + dVal2;
                                }
                                //
                                //oporow1["col1"] = col3;

                                if (frm_cocd == "SAIP")
                                {
                                    oporow1["REFNUM"] = drw["sg1_t15"].ToString().Trim();
                                    oporow1["EXC_57F4DT"] = fgen.make_def_Date(drw["sg1_t16"].ToString().Trim(), vardate);
                                }
                                else oporow1["REFNUM"] = "-";

                                //*******************
                                par_code = txtacode.Value.Trim();
                                if (iopr == "CG")
                                {
                                    if (tax_code.Length <= 0)
                                    {
                                        tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A77'", "PARAMS");
                                        sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A77'", "PARAMS2");
                                        tax_code2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A78'", "PARAMS");
                                    }
                                }
                                else
                                {
                                    if (tax_code.Length <= 0)
                                    {
                                        tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A79'", "PARAMS");
                                        sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A79'", "PARAMS2");
                                    }
                                }
                                if (schg_code.Length <= 0)
                                    schg_code = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(params) as param from controls where id='A41'", "param");

                                //***********************

                                oporow1["RCODE"] = sal_code;

                                oporow1["btchno"] = frm_mbr + ViewState["refNo"].ToString() + txtvchdate.Text.Trim();

                                if (edmode.Value == "Y")
                                {
                                    oporow1["eNt_by"] = ViewState["entby"].ToString();
                                    oporow1["eNt_dt"] = ViewState["entdt"].ToString();
                                    oporow1["edt_by"] = frm_uname;
                                    oporow1["edt_dt"] = vardate;
                                }
                                else
                                {
                                    oporow1["eNt_by"] = frm_uname;
                                    oporow1["eNt_dt"] = vardate;
                                    oporow1["edt_by"] = "-";
                                    oporow1["eDt_dt"] = vardate;
                                }                               
                                //===========
                                oDS1.Tables[0].Rows.Add(oporow1);

                                fgen.save_data(frm_qstr, frm_cocd, oDS1, "IVOUCHER");

                                #region Voucher Saving
                            //    batchNo = "W" + batchNo;
                                string app_by = "-"; string vari_vch = "-";
                                if (frm_cocd == "LOGW" || frm_cocd == "ROOP")
                                {
                                    app_by = frm_uname;
                                    vari_vch = "Y";
                                }

                                if (nVty == "58")
                                {
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, sal_code, par_code, fgen.make_double(dVal, 2), 0, Vgstvch_no, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdate));
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 2, tax_code, par_code, fgen.make_double(dVal1, 2), 0, Vgstvch_no, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal1, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdate));

                                    if (tax_code2.Length > 0 && dVal2 != 0)
                                    {
                                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 3, tax_code2, par_code, fgen.make_double(dVal2, 2), 0, Vgstvch_no, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal2, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdate));
                                    }
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, par_code, sal_code, 0, fgen.make_double(dVal + dVal1 + dVal2, 2), Vgstvch_no, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal + dVal1 + dVal2, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdate));
                                    if (chktcs.Checked == true)
                                    {
                                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 4, tcscode, par_code, tcsamt, 0, Vgstvch_no, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdate));
                                    }							
                                }
                                else
                                {
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, sal_code, fgen.make_double(dVal + dVal1 + dVal2 + tcsamt, 2), 0, Vgstvch_no, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal + dVal1 + dVal2, 2), 0, drw["sg1_t15"].ToString().Trim(), Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, vinvno,  Convert.ToDateTime(vinvdate));
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, sal_code, par_code, 0, fgen.make_double(dVal, 2), Vgstvch_no, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal, 2), drw["sg1_t15"].ToString().Trim(), Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdate));
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 51, tax_code, par_code, 0, fgen.make_double(dVal1, 2), Vgstvch_no, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal1, 2), drw["sg1_t15"].ToString().Trim(), Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdate));

                                    if (tax_code2.Length > 0 && dVal2 != 0)
                                    {
                                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 52, tax_code2, par_code, 0, fgen.make_double(dVal2, 2), Vgstvch_no, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal2, 2), drw["sg1_t15"].ToString().Trim(), Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdate));
                                    }
                                    //----for tcs saving in voucher   
                                    if (chktcs.Checked == true)
                                    {
                                        fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 4, tcscode, par_code, 0, tcsamt, Vgstvch_no, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, 0, drw["sg1_t15"].ToString().Trim(), Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, vinvno, Convert.ToDateTime(vinvdate));//ols
                                       // fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 4, tcscode, par_code, 0, tcsamt, vinvno, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), Vgstvch_no, frm_uname, Convert.ToDateTime(vardate), "-", "VOUCHER", txtGstClassCode.Value, "-", Convert.ToDateTime(txtvchdate.Text.Trim()));
                                    }
                                }
                                #endregion

                                l++;
                            }
                            #endregion
                        }
                    }
                }
            }
        }
    }

    void save_fun3()
    {

    }

    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        SQuery = "SELECT 'ED' AS FSTR,'Record Efforts Done' as NAME,'ED' AS CODE FROM dual";
    }
    //------------------------------------------------------------------------------------   
    protected void btnupload_Click(object sender, EventArgs e)
    {
        string ext = "", filesavepath = "";
        if (txtacode.Value.Trim().Length > 2)
        {
            //if (FileUpload1.HasFile)
            {
                //ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".txt";
                //FileUpload1.SaveAs(filesavepath);
                string[] readText = File.ReadAllLines(filesavepath);

                DataTable dtn = new DataTable();
                dtn.Columns.Add("ICODE", typeof(string));
                dtn.Columns.Add("Iname", typeof(string));
                dtn.Columns.Add("CPARTNO", typeof(string));
                dtn.Columns.Add("PONO", typeof(string));
                dtn.Columns.Add("OLDPONO", typeof(string));
                dtn.Columns.Add("PODT", typeof(string));
                dtn.Columns.Add("CGST", typeof(string));
                dtn.Columns.Add("SGST", typeof(string));
                dtn.Columns.Add("IGST", typeof(string));

                string icode = "", iname = "", cpartno = "", HSCODE = "", cgst = "", sgst = "", igst = "", pono = "", oldpono = "", podt = "", batchno = "", srno = "", batchdt = "";
                string sno = "", billno = "", pricdt = "", _57f2 = "", srvno = "", shp = "", acp = "", old_rate = "", bas_amt = "", cgstrate = "", sgstrate = "", igstrate = "", oldrate = "", billdt = "", srvDate = "", cum_shp = "", cum_acp = "", newRate = "", oldBas = "", Flg = "";
                string[] u1 = null;
                string[] u2 = null;

                for (int j = 1; j < 25; j++)
                {
                    dtn.Columns.Add("col" + j, typeof(string));
                }

                dtn.Columns.Add("BATCHNO", typeof(string));
                dtn.Columns.Add("SRNO", typeof(string));
                dtn.Columns.Add("BATCHDT", typeof(string));

                DataRow drn;
                string toRead = "N";
                foreach (string s in readText)
                {
                    if (s.Contains("Total For Item :"))
                    {
                        toRead = "N";
                    }
                    if (toRead == "Y")
                    {
                        if (s.Contains("------------")) { }
                        else
                        {
                            if (1 == 2)
                            {
                                #region valueFill
                                sno = s.Substring(5, 3);
                                billno = s.Substring(8, 22);
                                pricdt = s.Substring(30, 12);
                                _57f2 = s.Substring(42, 17);
                                srvno = s.Substring(59, 11);
                                shp = s.Substring(70, 5);
                                acp = s.Substring(75, 5);
                                old_rate = s.Substring(80, 7);
                                bas_amt = s.Substring(87, 10);
                                cgstrate = s.Substring(97, 8);
                                sgstrate = s.Substring(104, 8);
                                igstrate = s.Substring(114, 8);
                                oldrate = s.Substring(126, 8);
                                billdt = s.Substring(134, 11);
                                srvDate = s.Substring(145, 12);
                                cum_shp = s.Substring(157, 5);
                                cum_acp = s.Substring(163, 6);
                                newRate = s.Substring(169, 10);
                                oldBas = s.Substring(179, 6);
                                Flg = s.Substring(185, 2);
                                #endregion
                            }

                            string[] r1 = s.Split(' ');
                            int v = 0;
                            #region valueFill
                            foreach (string res in r1)
                            {
                                if (res.Length >= 1)
                                {
                                    if (v == 1) sno = res;
                                    if (v == 2) billno = res;
                                    if (v == 3) pricdt = res;
                                    if (v == 4) _57f2 = fgen.Right(billno, 6);
                                    if (v == 5) srvno = res;
                                    if (v == 6) shp = res;
                                    if (v == 7) acp = res;
                                    if (v == 8) old_rate = res;
                                    if (v == 9) bas_amt = res;
                                    if (v == 10) cgstrate = res;
                                    if (v == 11) sgstrate = res;
                                    if (v == 12) igstrate = res;
                                    if (v == 13) oldrate = res;
                                    if (v == 14) billdt = res;
                                    if (v == 15) srvDate = res;
                                    if (v == 16) cum_shp = res;
                                    if (v == 17) cum_acp = res;
                                    if (v == 18) newRate = res;
                                    if (v == 19) oldBas = res;
                                    if (v == 20) Flg = res;
                                    v++;
                                }
                            }
                            v = 0;
                            #endregion

                            if (fgen.make_double(sno) >= 1)
                            {
                                #region adding to table
                                drn = dtn.NewRow();
                                drn["icode"] = icode;
                                drn["iname"] = iname;
                                drn["cpartno"] = cpartno;
                                drn["PONO"] = pono;
                                drn["OLDPONO"] = oldpono;
                                drn["PODT"] = podt;
                                drn["CGST"] = cgst;
                                drn["SGST"] = sgst;
                                drn["IGST"] = igst;
                                drn["COL1"] = sno;
                                drn["COL2"] = billno;
                                drn["COL3"] = pricdt;
                                drn["COL4"] = _57f2;
                                drn["COL5"] = srvno;
                                drn["COL6"] = shp;
                                drn["COL7"] = acp;
                                drn["COL8"] = old_rate;
                                drn["COL9"] = bas_amt;
                                drn["COL10"] = cgstrate;
                                drn["COL11"] = sgstrate;
                                drn["COL12"] = igstrate;
                                drn["COL13"] = oldrate;
                                drn["COL14"] = billdt;
                                drn["COL15"] = srvDate;
                                drn["COL16"] = cum_shp;
                                drn["COL17"] = cum_acp;
                                drn["COL18"] = newRate;
                                drn["COL19"] = oldBas;
                                drn["COL20"] = Flg;
                                drn["COL21"] = HSCODE;
                                drn["BATCHNO"] = batchno;
                                drn["SRNO"] = srno;
                                drn["BATCHDT"] = batchdt;
                                dtn.Rows.Add(drn);
                                #endregion
                                sno = "";
                            }
                        }
                    }
                    if (s.Contains("BATCH NO  :"))
                    {
                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "BATCH NO  :");
                        u2 = Regex.Split(u1[1], "Buyer ");
                        batchno = u2[0].ToString();
                    }
                    if (s.Contains("BATCH DATE:"))
                    {
                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "BATCH DATE:");
                        batchdt = u1[1].ToString();
                    }
                    if (s.Contains("SERIAL NO :"))
                    {
                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "SERIAL NO :");
                        srno = u1[1].ToString();
                    }
                    if (s.Contains("Part No. :"))
                    {
                        #region filling header
                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "Part No. :");
                        u2 = Regex.Split(u1[1], "PO.No");
                        cpartno = u2[0].ToString();
                        if (cpartno.Trim() == "84681M68P00")
                        {

                        }
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, "sELECT TRIM(ICODE) AS ICODE,INAME,CPARTNO,HSCODE FROM item where upper(trim(CPARTNO))='" + cpartno.Trim().ToUpper() + "'");
                        if (dt.Rows.Count > 0)
                        {
                            icode = dt.Rows[0]["icode"].ToString().Trim();
                            iname = dt.Rows[0]["iname"].ToString().Trim();
                            HSCODE = dt.Rows[0]["HSCODE"].ToString().Trim();
                            dt.Dispose();
                        }

                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "PO.No");
                        u2 = Regex.Split(u1[1], "Old");
                        pono = u2[0].ToString();

                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "Old PO.No");
                        u2 = Regex.Split(u1[1], "PO Date:");
                        oldpono = u2[0].ToString();

                        u1 = null; u2 = null;
                        u1 = Regex.Split(s, "PO Date:");
                        u2 = Regex.Split(u1[1], "CGST");
                        podt = u2[0].ToString();

                        u1 = null;
                        u1 = Regex.Split(s, "CGST :");
                        cgst = u1[1].Split('%')[0].ToString();

                        u1 = null;
                        u1 = Regex.Split(s, "SGST :");
                        sgst = u1[1].Split('%')[0].ToString();

                        u1 = null;
                        u1 = Regex.Split(s, "IGST :");
                        igst = u1[1].Split('%')[0].ToString();
                        #endregion
                    }
                    if (s.Contains("S.N. BillNo"))
                    {
                        toRead = "Y";
                    }
                }

                ViewState["dtn"] = dtn;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);
            }
        }
        else
        {
            fgen.msg("-", "AMSG", "Please Select Customer First!!");
        }
    }

    protected void btnAcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer ", frm_qstr);
    }
    protected void btnIcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Product ", frm_qstr);
    }
    protected void btnDNCN_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DNCN";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select D/N C/N Reason", frm_qstr);
    }
    protected void btnGstClass_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "GSTCLASS";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select GST Class", frm_qstr);
    }
    protected void btnlist1_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "List";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    protected void btnanex_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "ANEXX";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    protected void btnlist2_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "LIST_E2";
        make_qry_4_popup();
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnlist3_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "ListS";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    protected void btnVerify_ServerClick(object sender, EventArgs e)
    {
        col1 = fgen.check_filed_name(frm_qstr, frm_cocd, "SCRATCH2", "COL46");
        if (col1 == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SCRATCH2 ADD COL46 VARCHAR(20) DEFAULT '-'");
        col1 = fgen.check_filed_name(frm_qstr, frm_cocd, "SCRATCH2", "COL47");
        if (col1 == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE SCRATCH2 ADD COL47 VARCHAR(20) DEFAULT '-'");
        //VOUCHER FIELDS 
        col1 = fgen.check_filed_name(frm_qstr, frm_cocd, "VOUCHER", "ORIGINV_NO");
        if (col1 == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE VOUCHER ADD ORIGINV_NO VARCHAR(16) DEFAULT '-'");
        col1 = fgen.check_filed_name(frm_qstr, frm_cocd, "VOUCHER", "ORIGINV_DT");
        if (col1 == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE VOUCHER ADD ORIGINV_DT DATE DEFAULT DATE");
        col1 = fgen.check_filed_name(frm_qstr, frm_cocd, "VOUCHER", "GSTVCH");
        if (col1 == "0") fgen.execute_cmd(frm_qstr, frm_cocd, "ALTER TABLE VOUCHER ADD GSTVCH_NO VARCHAr(16) DEFAULT '-'");
    }
}