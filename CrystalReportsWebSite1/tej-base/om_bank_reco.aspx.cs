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

public partial class om_bank_reco : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4;
    DataRow oporow, oporow1, oporow2; DataSet oDS, oDS1, oDS2;
    int i = 0, z = 0;

    DataTable dtCol = new DataTable();
    string Checked_ok;
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

    //variables declared to implement bank reconcilation form
    string autoflag = "0";
    string ctrl_r_dt = "N";
    string ERP_M131_reco_upd_vch = "";
    string ERP_W1102_reco_upd_vch = "";
    string unclear_entries = "N";

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
                doc_addl.Value = "0";

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
                DataTable dtW = (DataTable)ViewState["dtn"];
                if (dtW != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtW, "", "ContentPlaceHolder1_datadiv").ToString(), false);
                }
                ERP_M131_reco_upd_vch = fgen.seek_iname(frm_qstr, frm_cocd, "select enable_yn from stock where id='M131'", "enable_yn");
                ERP_W1102_reco_upd_vch = fgen.seek_iname(frm_qstr, frm_cocd, "select opt_enable from FIN_RSYS_OPT where trim(opt_id)='W1102'", "opt_enable");
            }
            setColHeadings();
            set_Val();
            btnprint.Visible = true;
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
        btnlist.Disabled = false; btnlist1.Disabled = true; btnautofilldt.Disabled = true; btnupdate.Disabled = true;
        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();
        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();
        sg4_add_blankrows();

        btnAcode.Enabled = false;
        //btnIcode.Enabled = false;
        // btnDNCN.Enabled = false;
        //btnGstClass.Enabled = false;
        ViewState["dtn"] = null;
        sg1.DataSource = sg1_dt; sg1.DataBind();
        if (sg1.Rows.Count > 0) sg1.Rows[0].Visible = false; sg1_dt.Dispose();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
        btnlist.Disabled = true; btnlist1.Disabled = false; btnautofilldt.Disabled = false; btnupdate.Disabled = false;
        btnAcode.Enabled = true;
        //btnIcode.Enabled = true;
        //btnDNCN.Enabled = true;
        // btnGstClass.Enabled = true;
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

        lblheader.Text = "Bank Reconcilation";

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "50");
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
                SQuery = "Select c.acode as fstr, C.AName as Account_Name,b.acode as Account_Code,count(a.vchnum) as Entries from (Select distinct vchnum||to_char(vchdate,'dd/mm/yyyy') as vchnum,type from voucher where branchcd ='" + frm_mbr + "' and vchdate like '%' and acode!='120000' union all Select distinct a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') as vchnum,b.type1 as type from voucherop a, type b where trim(a.acode)=trim(b.acode) and b.id='V' and a.branchcd ='" + frm_mbr + "' and a.vchdate like '%' and a.acode!='120000' ) a ,type b,famst c where b.id='V' and trim(b.acode)=trim(C.acode) and a.type=b.type1 and (substr(c.grp,1,2)='12' OR substr(c.grp,1,2)='03') group by c.acode,C.Aname,b.acode order by C.Aname";
                break;
            case "TICODE":
                SQuery = "select icode,iname as product,icode as code,cpartno,unit from item where length(trim(icode))>4 and trim(icode) like '9%' order by icode";
                SQuery = "select distinct a.icode,A.PURPOSE as product,a.icode as erpcode,A.EXC_57F4 as partname,a.ponum,to_Char(a.podate,'dd/mm/yyyy') as podate,a.finvno as cust_pono,a.vchnum as invno,to_char(a.vchdate,'dd/mm/yyyy') as invdate from ivoucher a  where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + DateRange + " and trim(A.acode)='" + txtbankcode.Value.Trim() + "' order by a.vchnum,a.icode";
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
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD" || btnval == "LIST_E")
                    //SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.col33 as po,A.COL35 AS po_DATE,A.ACODE AS PARTY_CODE,B.ANAME AS PARTY,A.ICODE AS ERPCODE,C.INAME AS PART,C.CPARTNO AS PARTNO,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,FAMST B,ITEM C where TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(a.ICODE)=TRIM(C.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr , A.vchnum,to_char(A.vchdate,'dd/mm/yyyy') as vchdate,A.type,a.ent_by from costestimate A WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and A.vchnum<>'000000' AND A.VCHDATE " + DateRange + " order by vchdate desc ,A.vchnum desc";
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
            //hffield.Value = "New";
            //make_qry_4_popup();
            //fgen.Fn_open_sseek("-", frm_qstr);

            // else comment upper code

            frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            txtrecondt.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            disablectrl();
            create_tab();
            sg1_add_blankrows();


            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            setColHeadings();
            ViewState["sg1"] = sg1_dt;
            fgen.EnableForm(this.Controls);

            btnAcode.Focus();
            btnsave.Disabled = true;
        }

        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        //if (frm_ulvl != "0") return;
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

        if (txtbankcode.Value.Trim().Length < 2)
        { fgen.msg("-", "AMSG", "Please Select Customer Code!!"); txtvchdate.Focus(); return; }

        //if (fgen.make_double(txtcalbank.Value.Trim()) <= 0)
        //{ fgen.msg("-", "AMSG", "Please Enter New Rate!!"); txtcalbank.Focus(); return; }



        //if (txtGstClassCode.Value.Length < 2)
        //{ fgen.msg("-", "AMSG", "Please Select GST Class!!"); btnGstClass.Focus(); return; }
        //if (txtDnCnCode.Value.Length < 2)
        //{ fgen.msg("-", "AMSG", "Please Select Reason for D/N C/N !!"); btnGstClass.Focus(); return; }

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
        //if (txtbankname.Value.ToString().ToUpper().Contains("MARUTI"))
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

        if (sg1.Rows.Count < 0)
        {





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
            //  gr.Cells[8].Text = txtNrate.Value;
            if (fgen.make_double(txtcalbank.Value) != 0)
            {
                oldRate = fgen.make_double(txtcalbank.Value);
                gr.Cells[7].Text = oldRate.ToString();
            }
            else oldRate = fgen.make_double(gr.Cells[7].Text);
            gr.Cells[9].Text = Math.Round(fgen.make_double(gr.Cells[8].Text) - oldRate, 3).ToString();

            //if (fgen.make_double(txtSgst.Value.Trim()) > 0 || fgen.make_double(txtdiff.Value.Trim()) > 0)
            //{
            //    if (fgen.make_double(txtSgst.Value.Trim()) > 0)
            //    {
            //        gr.Cells[16].Text = txtdiff.Value;
            //        gr.Cells[17].Text = txtSgst.Value;
            //    }
            //    else
            //    {
            //        gr.Cells[16].Text = txtdiff.Value;
            //        gr.Cells[17].Text = "0";
            //    }
            //}
            //gr.Cells[28].Text = Convert.ToString(fgen.make_double(((TextBox)gr.FindControl("sg1_h7")).Text) * fgen.make_double(txtNrate.Value.Trim()));
        }

        dtn = (DataTable)ViewState["dtn"];
        double d1 = 0;
        foreach (DataRow dr in dtn.Rows)
        {
            foreach (GridViewRow gr in sg1.Rows)
            {
                if (gr.Cells[10].Text.Trim() == dr["sg1_srno"].ToString().Trim()) dr["sg1_h7"] = ((TextBox)gr.FindControl("sg1_h7")).Text.Trim();
            }
            if (fgen.make_double(txtcalbank.Value) != 0)
            {
                oldRate = fgen.make_double(txtcalbank.Value);
                dr["sg1_h8"] = oldRate.ToString();
            }
            dr["sg1_h9"] = txtcalbank.Value;
            dr["sg1_h10"] = Math.Round(fgen.make_double(txtcalbank.Value) - fgen.make_double(dr["sg1_h8"].ToString().Trim()), 3).ToString();
            if (fgen.make_double(txtcalbank.Value.Trim()) > 0 || fgen.make_double(txtdiff.Value.Trim()) > 0)
            {
                if (fgen.make_double(txtcalbank.Value.Trim()) > 0)
                {
                    dr["SG1_T3"] = "CG";
                    dr["sg1_t9"] = txtdiff.Value;
                    //dr["sg1_t10"] = txtSgst.Value;
                    dr["sg1_t11"] = 0;
                }
                else
                {
                    dr["SG1_T3"] = "IG";
                    dr["sg1_t9"] = 0;
                    dr["sg1_t10"] = 0;
                    dr["sg1_t11"] = txtdiff.Value;
                }
            }
            dr["sg1_t13"] = fgen.make_double(dr["sg1_h7"].ToString().Trim()) * fgen.make_double(txtcalbank.Value.Trim());

            d1 += fgen.make_double(dr["sg1_h7"].ToString().Trim());
        }
        ViewState["dtn"] = dtn;

        lblRowCount.Text = "Total Rows Showing : " + sg1.Rows.Count.ToString() + " ";
    }
    //------------------------------------------------------------------------------------
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (frm_ulvl == "0" || frm_ulvl == "1")
        {
            if (chk_rights == "Y")
            {
                clearctrl();
                set_Val();
                hffield.Value = "Del_E";
                make_qry_4_popup();
                fgen.Fn_open_sseek("Select " + lblheader.Text + " to Delete ", frm_qstr);
            }
            else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form !!");
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form, Entry deletion allowed for Admin and above level only !!");
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
        //hffield.Value = "LIST_E";
        //make_qry_4_popup();
        //fgen.Fn_open_sseek("-", frm_qstr);
        hffield.Value = "LIST";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        // SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + DateRange + " and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
        // fgen.Fn_FillChart(frm_cocd, frm_qstr, "Testing Chart", "line", "Main Heading", "Sub Heading", SQuery, "");
        hffield.Value = "Print_E";
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
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' and a.branchcd='" + frm_mbr + "' and a.type='50' ");
                // Deleing data from Sr Ctrl Table                
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.acode)='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "' and a.type in ('" + frm_vty + "','58','59') ");
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'  ");
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
                    fgen.msg("-", "CMSG", "You Have Entered the deletion Module. This is a Sensitive Area. An Entry deleted while never be Re-Instated.Are You Sure!! You Want to Delete");
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
                        txtbankcode.Value = dt.Rows[0]["acode"].ToString().Trim();
                        txtbankname.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where trim(acode)='" + dt.Rows[0]["acode"].ToString().Trim() + "'", "aname");
                        txtrecondt.Text = dt.Rows[0]["icode"].ToString().Trim();
                        //txtIname.Value = dt.Rows[0]["iname"].ToString().Trim();
                        txtbalance.Value = dt.Rows[0]["cpartno"].ToString().Trim();

                        txtissnotclr.Value = dt.Rows[0]["col33"].ToString().Trim();
                        //txtPodt.Value = dt.Rows[0]["col35"].ToString().Trim();
                        txtcustPo.Text = dt.Rows[0]["col33"].ToString().Trim();

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
                    SQuery = "SELECT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.COL33 AS cust_pono,A.COL2 AS inv_no,a.col3 as inv_dt,a.acode as partycode,c.aname as party,b.icode as erpcode,B.PURPOSE as product,B.EXC_57F4,a.col6 as qty,a.col7 as oldrate,a.col8 as newrate,a.col9 as rate_diff,a.col13 as pono,a.col14 as pordt,a.col15 as inv_value,to_number(a.col16)+to_number(a.col17) as tax_per,a.ent_by,a.ent_dt,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.iamount as amt_wot,b.spexc_amt as amt,B.TYPE AS NOTE_TYPE,B.BRANCHCD AS B_CODE FROM SCRATCH2 A,ivoucher B,famst c WHERE TRIM(A.ACODE)||TRIM(A.COL2)||TO_CHAR(TO_DATE(A.COL3,'DD/MM/YYYY'),'DD/MM/YYYY')||TRIM(A.COL33)||trim(a.col5)||trim(a.col6)=TRIM(B.ACODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||TRIM(B.LOCATION)||trim(b.icode)||b.iqty_chl and trim(A.acode)=trim(c.acode) AND a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' and b.type in ('58','59') ORDER BY A.COL33";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
                    fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
                    break;
                case "Print_E":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", frm_mbr + frm_vty + col1);//for grade                           
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70137");
                    fgen.fin_acct_reps(frm_qstr);
                    break;
                case "TACODE":
                    txtbankcode.Value = col1;
                    txtbankname.Value = col2;
                    //btnIcode.Focus();
                    break;
                case "TICODE":
                    if (col1.Length < 2) return;
                    txtrecondt.Text = col1;
                    // txtIname.Value = col2
                    txtbalance.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    txtissnotclr.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    //txtPodt.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    txtcustPo.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");
                    //btnIcode.Focus();
                    fgen.Fn_open_prddmp1("-", frm_qstr);
                    break;
                case "DNCN":
                    //txtDnCnCode.Value = col1;
                    txtledgerbal.Value = col2;
                    //btnGstClass.Focus();
                    break;
                case "GSTCLASS":
                    //txtGstClassCode.Value = col1;
                    txtdepnotclr.Value = col2;
                    txtdepnotclr.Focus();
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
        //SQuery = "SELECT a.*,b.iname,b.cpartno,b.unit,B.HSCODE,a.exc_rate,a.cess_percent,a.iopr,c.num4,c.num5,c.num6,c.num7 FROM IVOUCHER a,item b,typegrp c WHERE trim(a.icode)=trim(B.icode) and trim(b.hscode)=trim(c.acref) and a.BRANCHCD='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + PrdRange + " and trim(a.acode)='" + txtbankcode.Value + "' and trim(a.icode)='" + txtrecondt.Text + "' and trim(A.ponum)||to_char(a.podate,'dd/mm/yyyy')='" + txtissnotclr.Value + txtPodt.Value + "' and c.id='T1' order by a.vchnum ";
        if (frm_cocd == "BONY") SQuery = "SELECT a.*,A.PURPOSE AS iname,A.EXC_57F4 AS cpartno,b.unit,B.HSCODE,a.exc_rate,a.cess_percent,a.iopr,c.num4,c.num5,c.num6,c.num7 FROM IVOUCHER a,item b,typegrp c WHERE trim(a.icode)=trim(B.icode) and trim(b.hscode)=trim(c.acref) and a.BRANCHCD='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + PrdRange + " and trim(a.acode)='" + txtbankcode.Value + "' and trim(a.icode)='" + txtrecondt.Text + "' and trim(A.finvno)='" + txtcustPo.Text + "' and c.id='T1' order by a.vchnum ";
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

                sg1_dr["sg1_h7"] = dr["iqtyout"].ToString().Trim();
                d1 += fgen.make_double(dr["iqtyout"].ToString().Trim());

                sg1_dr["sg1_h8"] = dr["irate"].ToString().Trim();
                //new rate
                sg1_dr["sg1_h9"] = 0;
                //diff
                sg1_dr["sg1_h10"] = 0;

                sg1_dr["sg1_f1"] = dr["invno"].ToString().Trim();
                sg1_dr["sg1_f2"] = Convert.ToDateTime(dr["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                sg1_dr["sg1_F3"] = dr["ponum"].ToString().Trim();
                sg1_dr["sg1_F4"] = Convert.ToDateTime(dr["podate"].ToString().Trim()).ToString("dd/MM/yyyy");

                sg1_dr["sg1_f5"] = dr["iamount"].ToString().Trim();

                sg1_dr["sg1_t1"] = dr["EXC_RATE"].ToString().Trim();
                sg1_dr["sg1_t2"] = dr["CESS_PERCENT"].ToString().Trim();
                sg1_dr["sg1_t3"] = dr["IOPR"].ToString().Trim();

                sg1_dr["sg1_t5"] = dr["iname"].ToString().Trim();
                sg1_dr["sg1_t6"] = dr["cpartno"].ToString().Trim();
                sg1_dr["sg1_t7"] = dr["unit"].ToString().Trim();
                sg1_dr["sg1_t8"] = dr["HSCODE"].ToString().Trim();

                if (chkdispclear.Checked)
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
                sg1_dr["sg1_t16"] = dr["EXC_57F4DT"].ToString().Trim();

                sg1_dt.Rows.Add(sg1_dr);
                i++;
            }
        }
        sg1.DataSource = sg1_dt;
        sg1.DataBind();

        lblRowCount.Text = "Total Rows Showing : " + sg1.Rows.Count.ToString() + " , Total Qty : " + d1;

        ViewState["dtn"] = sg1_dt;

        setColHeadings();

        //btnDNCN.Focus();
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");

        unclear_entries = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL11");
        if (unclear_entries == "Y")
        {
            gridfill();
            return;
        }


        if (hffield.Value == "LIST")
        {
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "SELECT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL11 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE FROM SCRATCH2 A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + DateRange + " and a.num10>0 ORDER BY A.COL33";
            SQuery = "SELECT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT FROM SCRATCH2 A,ivoucher B WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY') AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + DateRange + " and a.num10>0 ORDER BY A.COL33";
            SQuery = "SELECT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.COL33 AS cust_pono,A.COL2 AS inv_no,a.col3 as inv_dt,a.acode as partycode,c.aname as party,a.icode as erpcode,d.iname as product,a.col6 as qty,a.col7 as oldrate,a.col8 as newrate,a.col9 as rate_diff,a.col13 as pono,a.col14 as pordt,a.col15 as inv_value,to_number(a.col16)+to_number(a.col17) as tax_per,a.ent_by,a.ent_dt,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,B.TYPE AS NOTE_TYPE,B.BRANCHCD AS B_CODE FROM SCRATCH2 A,ivoucher B,famst c,item d WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL2)||TO_CHAR(TO_DATE(A.COL3,'DD/MM/YY'),'DD/MM/YYYY')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY') and trim(A.acode)=trim(c.acode) and trim(a.icode)=trim(d.icode) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + DateRange + " and b.type in ('58','59') and a.num10>0  ORDER BY A.COL33";

            SQuery = "SELECT A.VCHNUM AS ENTRYNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS ENTRYDT,A.COL33 AS cust_pono,A.COL2 AS inv_no,a.col3 as inv_dt,a.acode as partycode,c.aname as party,b.icode as erpcode,B.PURPOSE as product,B.EXC_57F4,a.col6 as qty,a.col7 as oldrate,a.col8 as newrate,a.col9 as rate_diff,a.col13 as pono,a.col14 as pordt,a.col15 as inv_value,to_number(a.col16)+to_number(a.col17) as tax_per,a.ent_by,a.ent_dt,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.iamount as amt_wot,b.spexc_amt as amt,B.TYPE AS NOTE_TYPE,B.BRANCHCD AS B_CODE FROM SCRATCH2 A,ivoucher B,famst c WHERE TRIM(A.ACODE)||TRIM(A.COL2)||TO_CHAR(TO_DATE(A.COL3,'DD/MM/YYYY'),'DD/MM/YYYY')||TRIM(A.COL33)||trim(a.col5)=TRIM(B.ACODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||TRIM(B.LOCATION)||trim(b.icode) and trim(A.acode)=trim(c.acode) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='" + frm_vty + "' AND A.VCHDATE " + DateRange + " and b.type in ('58','59') and a.num10>0 ORDER BY A.COL33";
            SQuery = "select  a.vchnum as Entry_No,to_char(a.vchdate,'dd/mm/yyyy') as entry_date,startdt as Reconcilation_Date,Remarks as Bank_name,COL12 as Ledger_Bal,NUM1 as Bank_Calculation,COL11 as Balance,col1 as vchnum,col2 as vchdate,col3 as chq_no,col4 as Debit_Amt,col5 as credit_amt,col7 as Party_Name from costestimate A WHERE a.branchcd='" + frm_mbr + "' and a.type='50' and A.vchnum<>'000000' AND A.VCHDATE  " + PrdRange + " order by vchdate desc ,A.vchnum desc";
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
        else
        {
            Checked_ok = "Y";
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();

            autoflag = fgenMV.Fn_Get_Mvar(frm_qstr, "COL12");

            if (col1 == "Y" && autoflag == "1")
            {
                for (int i2 = 0; i2 < sg1.Rows.Count; i2++)
                {

                    if (((TextBox)sg1.Rows[i2].FindControl("sg1_t1")).Text == "")
                    {
                        ((TextBox)sg1.Rows[i2].FindControl("sg1_t1")).Text = Convert.ToDateTime(txtrecondt.Text).ToString("yyy-MM-dd").Trim();

                        ((TextBox)sg1.Rows[i2].FindControl("sg1_t6")).Text = "Y";

                    }
                }

                autoflag = "0";
                fgenMV.Fn_Set_Mvar(frm_qstr, "COL12", "0");
                return;

            }
            //-----------------------------

            //checks
            if (edmode.Value == "Y")
            {
            }
            else
            {
            }



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
                                    else col3 = txtissnotclr.Value.Trim();

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

                        //save_fun2();----------------commented after seeing there is no saving in ivoucher table while the form entry

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
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        //if (frm_vnum != "000000" && frm_vty == "20")
        //{
        //    cmd_query = "update ivoucher set pname='" + frm_uname + "',tc_no='" + txtvchnum.Text + "',qc_date=sysdate,qcdate=to_datE(sysdate,'dd/mm/yyyy'),ACTUAL_INSP='Y',store='Y',inspected='Y',desc_=DECODE(Trim(desc_),'-','',Trim(desc_))||'QA.No.'||'" + txtvchnum.Text + "',IQTYIN=" + (fgen.make_double(txtlbl11.Text) - fgen.make_double(txtlbl13.Text)) + ",ACPT_UD =" + (fgen.make_double(txtlbl11.Text) - fgen.make_double(txtlbl13.Text)) + ",REJ_RW=" + fgen.make_double(txtlbl13.Text) + ",IEXC_aDDL =" + fgen.make_double(txtlbl14.Text) + " where branchcd='" + frm_mbr + "' and type like '0%' and vchnum ='" + txtlbl2.Text + "' and vchdate=to_Date('" + txtlbl3.Text + "','dd/mm/yyyy') and srno='" + doc_addl.Value.Trim() + "' and acode ='" + txtlbl4.Text + "' and icode ='" + txtlbl7.Text + "' and store<>'R'";
        //    fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
        //    if (fgen.make_double(txtlbl13.Text) > 0)
        //    {
        //        cmd_query = "delete from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchnum ='" + txtlbl2.Text + "' and vchdate=to_Date('" + txtlbl3.Text + "','dd/mm/yyyy') and srno='" + doc_addl.Value.Trim() + "' and acode ='" + txtlbl4.Text + "' and icode ='" + txtlbl7.Text + "' and store='R'";
        //        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

        //        cmd_query = "insert into ivoucher(vcode,iamount,btchno,btchdt,tc_no,pname,inspected,actual_insp,qcdate,qc_date,srno,branchcd,type,vchnum,vchdate,acode,icode,store,iqty_chl,iqtyin,iqtyout,acpt_ud,rej_rw,ponum,podate,rgpnum,rgpdate,invno,invdate,genum,gedate,rec_iss,ent_by,ent_dt,edt_by,edt_dt)(select acode,rej_rw*irate,btchno,btchdt,'" + txtvchnum.Text + "',pname,inspected,actual_insp,qcdate,qc_date,srno,branchcd,type,vchnum,vchdate,acode,icode,'R',iqty_chl," + fgen.make_double(txtlbl13.Text) + " as iqtyin,0 as iqtyout,0 as acpt_ud,0 as rej_rw,ponum,podate,rgpnum,rgpdate,invno,invdate,genum,gedate,rec_iss,ent_by,ent_dt,edt_by,edt_dt from ivoucher where branchcd='" + frm_mbr + "' and type like '0%' and vchnum ='" + txtlbl2.Text + "' and vchdate=to_Date('" + txtlbl3.Text + "','dd/mm/yyyy') and srno='" + doc_addl.Value.Trim() + "' and store<>'R' and acode ='" + txtlbl4.Text + "' and icode ='" + txtlbl7.Text + "')";
        //        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

        //    }

        //}

        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = txtvchnum.Text.Trim();
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["SRNO"] = i + 1;
                oporow["CONVDATE"] = "-";
                oporow["DROPDATE"] = "-";
                oporow["comments"] = "-";

                //  oporow["COMMENTS"] = sg1.Rows[i].Cells[16].Text.Trim();
                oporow["ACODE"] = "-";
                oporow["ICODE"] = txtbankcode.Value.Trim();
                oporow["COL1"] = sg1.Rows[i].Cells[13].Text.Trim();
                oporow["COL2"] = sg1.Rows[i].Cells[14].Text.Trim();
                oporow["COL3"] = sg1.Rows[i].Cells[15].Text.Trim();
                oporow["COL4"] = sg1.Rows[i].Cells[16].Text.Trim();
                oporow["COL5"] = sg1.Rows[i].Cells[17].Text.Trim();
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Length > 4) //bank date
                {
                    oporow["COL6"] = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text).ToString("dd/MM/yyyy");
                }
                else
                {
                    oporow["COL6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text;
                }
                oporow["COL7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text;
                oporow["COL8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text;
                oporow["REMARKS"] = txtbankname.Value.Trim();
                oporow["PRINTYN"] = "Y";
                oporow["STARTDT"] = txtrecondt.Text;
                //oporow["COL9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text;
                oporow["COL9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text;
                oporow["COL10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text;
                oporow["COL11"] = txtbalance.Value;
                oporow["COL12"] = txtledgerbal.Value;

                oporow["col13"] = txtvchdate.Text;
                oporow["col14"] = "-";
                oporow["col15"] = "-";
                oporow["col16"] = "-";
                oporow["col17"] = "-";
                oporow["col18"] = "-";
                oporow["col19"] = "-";
                oporow["col20"] = "-";
                oporow["col21"] = "-";
                oporow["col22"] = "-";
                oporow["col23"] = "-";
                oporow["col24"] = "-";
                oporow["col25"] = "-";
                oporow["itate"] = 0;
                oporow["irate"] = 0;
                oporow["app_by"] = "-";
                oporow["app_dt"] = vardate;
                oporow["attach"] = "-";
                oporow["attach2"] = "-";
                oporow["attach3"] = "-";
                oporow["comments2"] = "-";
                oporow["comments3"] = "-";
                oporow["az_by"] = "-";
                //  oporow["az_dt"] = vardate;//OLD
                oporow["az_dt"] = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text).ToString("dd/MM/yyyy");
                oporow["picode"] = "-";
                oporow["jstatus"] = "N";
                oporow["supcl_by"] = "-";
                oporow["comments4"] = "-";
                oporow["comments5"] = "-";
                oporow["splcd"] = "-";
                oporow["jhold"] = "-";
                oporow["prc1"] = "-";
                oporow["prc2"] = "-";
                oporow["prc3"] = "-";
                oporow["prc4"] = "-";
                oporow["scrp1"] = 0;
                oporow["scrp2"] = 0;
                oporow["time1"] = 0;
                oporow["time2"] = 0;
                oporow["enr1"] = 0;
                oporow["enr2"] = 0;
                oporow["altitem"] = "-";
                oporow["eff_wt"] = 0;
                oporow["NUM1"] = txtcalbank.Value;
                oporow["ENQDT"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");

                if (Check1.Checked == true)
                {
                    oporow["ENQNO"] = "OK";

                }
                else
                {
                    oporow["ENQNO"] = "-";
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
                    oporow["eNt_dt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
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
        double dVal = 0; double dVal1 = 0; double dVal2 = 0; double qty = 0;
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
            string mhd = "";
            string saveTo = "Y";
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
                            string branchcd = drw["sg1_h1"].ToString().Trim();
                            string invRmrk = "";
                            oDS1 = new DataSet();
                            oporow1 = null;
                            oDS1 = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");

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

                            string batchNo = drw["sg1_f3"].ToString().Trim();
                            if (frm_cocd == "BONY") batchNo = txtcustPo.Text.Trim();

                            oporow1["LOCATION"] = batchNo;

                            oporow1["vchnum"] = frm_vnum;
                            oporow1["vchdate"] = txtvchdate.Text.Trim();

                            oporow1["ACODE"] = txtbankcode.Value.Trim();
                            oporow1["VCODE"] = txtbankcode.Value.ToString().Trim();
                            oporow1["ICODE"] = txtrecondt.Text.Trim();

                            //oporow1["MATTYPE"] = txtGstClassCode.Value;
                            //oporow1["POTYPE"] = txtDnCnCode.Value;

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
                            oporow1["PODATE"] = Convert.ToDateTime(drw["sg1_f4"].ToString().Trim()).ToString("dd/MM/yyyy");

                            string vinvno = fgen.padlc(Convert.ToInt32(drw["sg1_h3"].ToString().Trim()), 6);
                            string vinvdate = Convert.ToDateTime(drw["sg1_h4"].ToString().Trim()).ToString("dd/MM/yyyy");

                            oporow1["INVNO"] = vinvno;
                            oporow1["INVDATE"] = vinvdate;

                            oporow1["UNIT"] = "NOS";

                            double Rate = fgen.make_double(drw["sg1_h10"].ToString().Trim());
                            if (Rate < 0) Rate = -1 * Rate;
                            oporow1["IRATE"] = Rate;

                            dVal = Math.Round(qty * (fgen.make_double(drw["sg1_h10"].ToString().Trim())), 2);
                            if (dVal < 0) dVal = -1 * dVal;
                            oporow1["IAMOUNT"] = dVal;

                            oporow1["NO_CASES"] = drw["sg1_t8"].ToString().Trim();
                            oporow1["EXC_57F4"] = drw["sg1_t8"].ToString().Trim();

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

                            oporow1["STORE"] = "N";
                            oporow1["MORDER"] = 1;
                            oporow1["SPEXC_RATE"] = dVal;
                            oporow1["SPEXC_AMT"] = dVal + dVal1 + dVal2;
                            //
                            oporow1["col1"] = col3;

                            oporow1["REFNUM"] = drw["sg1_t15"].ToString().Trim();
                            oporow1["EXC_57F4DT"] = drw["sg1_t16"].ToString().Trim();

                            //*******************
                            par_code = txtbankcode.Value.Trim();
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

                            oDS1.Tables[0].Rows.Add(oporow1);

                            fgen.save_data(frm_qstr, frm_cocd, oDS1, "IVOUCHER");

                            #region Voucher Saving
                            batchNo = "W" + batchNo;
                            string app_by = "-"; string vari_vch = "-";
                            if (frm_cocd == "LOGW" || frm_cocd == "ROOP")
                            {
                                app_by = frm_uname;
                                vari_vch = "Y";
                            }

                            if (nVty == "58")
                            {
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, sal_code, par_code, fgen.make_double(dVal, 2), 0, vinvno, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER");
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 2, tax_code, par_code, fgen.make_double(dVal1, 2), 0, vinvno, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal1, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER");

                                if (tax_code2.Length > 0 && dVal2 != 0)
                                {
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 3, tax_code2, par_code, fgen.make_double(dVal2, 2), 0, vinvno, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal2, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER");
                                }
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, par_code, sal_code, 0, fgen.make_double(dVal + dVal1 + dVal2, 2), vinvno, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal + dVal1 + dVal2, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER");
                            }
                            else
                            {
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 1, par_code, sal_code, fgen.make_double(dVal + dVal1 + dVal2, 2), 0, vinvno, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, fgen.make_double(dVal + dVal1 + dVal2, 2), 0, "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER");
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 50, sal_code, par_code, 0, fgen.make_double(dVal, 2), vinvno, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER");
                                fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 51, tax_code, par_code, 0, fgen.make_double(dVal1, 2), vinvno, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal1, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER");

                                if (tax_code2.Length > 0 && dVal2 != 0)
                                {
                                    fgen.vSave(frm_qstr, frm_cocd, branchcd, nVty, frm_vnum, Convert.ToDateTime(txtvchdate.Text.Trim()), 52, tax_code2, par_code, 0, fgen.make_double(dVal2, 2), vinvno, Convert.ToDateTime(vinvdate), invRmrk, 0, 0, 1, 0, fgen.make_double(dVal2, 2), "-", Convert.ToDateTime(txtvchdate.Text.Trim()), frm_uname, Convert.ToDateTime(vardate), iopr, 0, fgen.make_double(qty, 2), batchNo, app_by, Convert.ToDateTime(vardate), vari_vch, "VOUCHER");
                                }
                            }

                            //lblprogress.Text = "No.Of Vouchers updated" + l;
                            #endregion

                            l++;
                        }
                        #endregion
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
        if (txtbankcode.Value.Trim().Length > 2)
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
        fgen.Fn_open_sseek("Select Bank ", frm_qstr);
    }

    protected void btnlist1_ServerClick(object sender, EventArgs e)
    {
        if (txtbalance.Value.toDouble() == 0)
        {
            fgen.msg("-", "AMSG", "Please fill bank balance before fetching the data.");
            return;
        }
        unclear_entries = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL11");
        if ((txtbankcode.Value == "-") || (txtbankcode.Value == ""))
        {
            fgen.msg("-", "AMSG", "No Bank is selected for the reconcilation...");
            txtbankcode.Focus();
            return;
        }
        if (txtrecondt.Text.Length <= 0 || txtrecondt.Text == "" || txtrecondt.Text == "-" || txtrecondt.Text == "0")
        {
            fgen.msg("-", "AMSG", "Reconcilation date is not entered !!");
            txtbankcode.Focus();
            return;
        }
        if (fgen.getOption(frm_qstr, frm_cocd, "W0096", "OPT_ENABLE") == "Y")
        {
            string totCount = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT COUNT(DISTINCT VCHNUM) AS CC FROM VOUCHER WHERE NVL(APP_BY,'-')='-' AND SUBSTR(TYPE,1,1) IN ('1','2') AND TRIM(ACODE)='" + txtbankcode.Value.Trim() + "' ", "CC");
            if (totCount.toDouble() > 0)
            {
                fgen.msg("-", "AMSG", "ERP has found, few of the unapproved vouchers'13'Please approve them before doing the Bank Reconciliation !!");
                return;
            }
        }


        txtledgerbal.Value = acBal(txtbankcode.Value.Trim());
        //if ((unclear_entries == "Y") && (chkdispclear.Checked == true))
        //{
        //    fgen.Fn_open_dtbox("Enter Date", frm_qstr);
        //    return;
        //}

        if (chkdispclear.Checked == true)
        {
            fgen.Fn_open_dtbox("Enter Date", frm_qstr);
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL11", "Y");
            return;
        }

        //if ((chkdispclear.Checked == true))
        //{

        //    fgen.msg("-", "AMSG", "Please Enter Date from which you wish to see the Cleared Entries");
        //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL11", "Y");
        //    // return;
        //}

        else
        {
            gridfill();


            //SQuery = "select * from (select a.vchnum,a.vchdate,a.refnum CHEQUE_NO,to_char(a.dramt,'999999999990.00') Debits,to_char(a.cramt,'9999999990.00') Credits,a.bank_date,A.NARATION,A.TYPE,A.rCODE,' ' as Touched,to_number(to_char(a.vchdate,'yyyymmdd')) as numdt,a.costcd,a.branchcd,a.refdate,nvl(a.tfcdr,0) as tfcdr,nvl(a.tfccr,0) as tfccr,a.naration as Remarks from voucher a where trim(a.acode)='120010' and a.branchcd<>'88' and a.vchdate between to_date('31/12/2016','dd/mm/yyyy') and to_date ('08/05/2018','dd/mm/yyyy')  and (bank_date is null OR BANK_dATE >TO_DATE('08/05/2018','DD/MM/YYYY')) union all select substr(x.invno,1,6) as invno,x.invdate,x.refnum CHEQUE_NO,to_char(x.dramt,'999999999990.00') Debits,to_char(x.cramt,'9999999990.00') Credits,x.bank_date,x.NARATION,x.TYPE,x.rCODE,' ' as Touched,to_number(to_char(x.vchdate,'yyyymmdd')) as numdt,'OPE' as costcd,x.branchcd,x.vchdate as refdate,0 as tfcdr,0 as tfccr,'-' as Remarks from voucherop x where x.branchcd<>'DD' and x.type='99' and trim(x.acode)='120010' and (x.bank_date is null OR x.BANK_dATE >TO_DATE('08/05/2018','DD/MM/YYYY')) ) order by vchdate,vchnum,CHEQUE_NO,branchcd";
            //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

            ////code to insert data into gridview

            //if (dt.Rows.Count > 0)
            //{
            //    //txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
            //    //txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
            //    //txtbankcode.Value = dt.Rows[0]["acode"].ToString().Trim();
            //    //txtbankname.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where trim(acode)='" + dt.Rows[0]["acode"].ToString().Trim() + "'", "aname");
            //    //txtrecondt.Text = dt.Rows[0]["icode"].ToString().Trim();
            //    ////txtIname.Value = dt.Rows[0]["iname"].ToString().Trim();
            //    //txtbalance.Value = dt.Rows[0]["cpartno"].ToString().Trim();

            //    //txtissnotclr.Value = dt.Rows[0]["col33"].ToString().Trim();
            //    ////txtPodt.Value = dt.Rows[0]["col35"].ToString().Trim();
            //    //txtcustPo.Text = dt.Rows[0]["col33"].ToString().Trim();

            //    i = 1;
            //    create_tab();
            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        sg1_dr = sg1_dt.NewRow();

            //        sg1_dr["sg1_SrNo"] = i;
            //        //sg1_dr["sg1_h1"] = dr["branchcd"].ToString().Trim();
            //        //sg1_dr["sg1_h2"] = dr["col1"].ToString().Trim();
            //        //sg1_dr["sg1_h3"] = dr["col2"].ToString().Trim();
            //        //sg1_dr["sg1_h4"] = dr["col3"].ToString().Trim();

            //        //sg1_dr["sg1_h5"] = dr["col4"].ToString().Trim();
            //        //sg1_dr["sg1_h6"] = dr["col5"].ToString().Trim();
            //        ////qty
            //        //sg1_dr["sg1_h7"] = dr["col6"].ToString().Trim();
            //        //sg1_dr["sg1_h8"] = dr["col7"].ToString().Trim();
            //        ////new rate
            //        //sg1_dr["sg1_h9"] = dr["col8"].ToString().Trim();
            //        ////diff
            //        //sg1_dr["sg1_h10"] = dr["col9"].ToString().Trim();

            //        sg1_dr["sg1_f1"] = dr["vchnum"].ToString().Trim();
            //        sg1_dr["sg1_f2"] = Convert.ToDateTime(dr["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

            //        sg1_dr["sg1_F3"] = dr["cheque_no"].ToString().Trim();
            //        sg1_dr["sg1_F4"] = dr["debits"].ToString().Trim();

            //        sg1_dr["sg1_f5"] = dr["credits"].ToString().Trim();

            //        sg1_dr["sg1_t1"] = dr["bank_date"].ToString().Trim();
            //        sg1_dr["sg1_t2"] = dr["naration"].ToString().Trim();
            //        sg1_dr["sg1_t3"] = dr["type"].ToString().Trim();

            //        sg1_dr["sg1_t5"] = dr["rcode"].ToString().Trim();
            //        sg1_dr["sg1_t6"] = dr["touched"].ToString().Trim();
            //        sg1_dr["sg1_t7"] = dr["numdt"].ToString().Trim();
            //        sg1_dr["sg1_t8"] = dr["costcd"].ToString().Trim();

            //        sg1_dr["sg1_t9"] = dr["branchcd"].ToString().Trim();
            //        sg1_dr["sg1_t10"] = dr["refdate"].ToString().Trim();
            //        sg1_dr["sg1_t11"] = dr["tfcdr"].ToString().Trim();

            //        sg1_dr["sg1_t12"] = dr["tfccr"].ToString().Trim();
            //        sg1_dr["sg1_t13"] = dr["remarks"].ToString().Trim();

            //        sg1_dt.Rows.Add(sg1_dr);
            //        i++;
            //    }
            //    sg1.DataSource = sg1_dt;
            //    sg1.DataBind();
            //    gridfill();

            //    lblRowCount.Text = "Total Rows Showing : " + sg1.Rows.Count.ToString();

            //    ViewState["dtn"] = sg1_dt;

            //    dt.Dispose();
            //    //ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
            //    //ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
            //    fgen.EnableForm(this.Controls);
            //    disablectrl();
            //    setColHeadings();
            // edmode.Value = "Y";
            // }


        }
    }

    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        bool chkDt = fgen.CheckIsDate(txtrecondt.Text);
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //sg1.Rows[sg1r].Cells[8].Attributes.Add("readonly", "false");
            sg1.Columns[0].Visible = false;
            sg1.Columns[1].Visible = false;
            sg1.Columns[2].Visible = false;
            sg1.Columns[3].Visible = false;
            sg1.Columns[4].Visible = false;
            sg1.Columns[5].Visible = false;
            sg1.Columns[6].Visible = false;
            sg1.Columns[7].Visible = false;
            sg1.Columns[8].Visible = false;
            sg1.Columns[9].Visible = false;
            sg1.Columns[10].Visible = false;
            sg1.Columns[11].Visible = false;
            sg1.Columns[21].Visible = false;
            sg1.Columns[31].Visible = false;
            sg1.Columns[32].Visible = false;
            sg1.Columns[33].Visible = false;
            updatetouched();


            if (chkDt)
                ((TextBox)e.Row.FindControl("sg1_t1")).Attributes["max"] = Convert.ToDateTime(txtrecondt.Text).ToString("yyyy-MM-dd");

        }

    }

    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();

        int index = 0;

        if (txtvchnum.Text == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG1_RMV":
            //if (index < sg1.Rows.Count - 1)
            //{
            //    hf1.Value = index.ToString();
            //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            //    //----------------------------
            //    hffield.Value = "SG1_RMV";
            //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
            //    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
            //}
            //break;


            case "SG1_ROW_ADD":

                if (index < sg1.Rows.Count)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_ADD_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select place", frm_qstr);
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

            case "SG1_ROW_ADD1":

                if (index < sg1.Rows.Count)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_ADD1_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Transporter", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD1";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Item", frm_qstr);
                    //fgen.Fn_open_mseek("Select Item", frm_qstr);
                }
                break;








        }
    }

    private void gridfill()
    {
        //variables declare for queries
        string popsql, seekSql = "", prtstring, MDT1 = "";
        double drtot;
        double crtot;
        int i;

        string query;
        string mq0;
        if ((txtbankcode.Value == "-") || (txtbankcode.Value == ""))
        {
            fgen.msg("-", "AMSG", "No Bank is selected for the reconcilation...");
            return;
        }


        if ((frm_cocd == "MANU*"))
        {
            //ac_datebal;
            //tacode.text.Trim();
            //(DateTime.Parse(Format(DateTime.Parse(tRdate), "dd/mm/yyyy")) + 1);
        }
        else
        {
            //Cons_ac_datebal;
            //tacode.text.Trim();
            //(DateTime.Parse(Format(DateTime.Parse(tRdate), "dd/mm/yyyy")) + 1);
        }



        string trandt = "";

        mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select enable_yn from controls where id='R06'", "enable_yn");
        if (mq0 == "Y")
        {
            trandt = fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='R06'", "params");
        }


        //check_control("R01");
        //if (((control_allowed == "Y")
        //            && IsDate(control_param)))
        //{
        //    trandt = control_param;
        //}

        //check_control("R06");
        //if (((control_allowed == "Y")
        //            && IsDate(control_param)))
        //{
        //    trandt = control_param;
        //}

        trandt = Convert.ToDateTime(trandt.Trim()).ToString("dd/MM/yyyy");
        if ((chkdispclear.Checked == true))
        {

            MDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");

            if ((MDT1.Trim() == ""))
            {
                prtstring = ("between to_date(\'"
                            + (trandt + ("\',\'dd/mm/yyyy\') and to_date (\'"
                            + (txtrecondt.Text.Trim() + "\',\'dd/mm/yyyy\')"))));
            }
            else
            {
                prtstring = ("between to_date(\'"
                            + (MDT1 + ("\',\'dd/mm/yyyy\') and to_date (\'"
                            + (txtrecondt.Text.Trim() + "\',\'dd/mm/yyyy\')"))));
            }

        }
        else
        {
            prtstring = ("between to_date(\'"
                        + (trandt + ("\',\'dd/mm/yyyy\') and to_date (\'"
                        + (txtrecondt.Text.Trim() + "\',\'dd/mm/yyyy\')"))));
        }

      if ((chkdispclear.Checked == true))
        {
            popsql = (@"select a.vchnum,a.vchdate,a.refnum CHEQUE_NO,to_char(a.dramt,'999999999990.00') Debits,to_char(a.cramt,'9999999990.00') Credits,a.bank_date,A.NARATION,A.TYPE,A.rCODE,' ' as Touched,to_number(to_char(a.vchdate,'yyyymmdd')) as numdt,a.costcd,a.branchcd,nvl(a.refdate,a.vchdate) as refdate,nvl(a.tfcdr,0) as tfcdr,nvl(a.tfccr,0) as tfccr,a.naration as Remarks from voucher a where trim(a.acode)='"
                        + (txtbankcode.Value.Trim() + ("\' and a.branchcd<>\'88\' and a.bank_Date "
                        + (prtstring + " "))));
        }
        else
        {
            popsql = (@"select a.vchnum,a.vchdate,a.refnum CHEQUE_NO,to_char(a.dramt,'999999999990.00') Debits,to_char(a.cramt,'9999999990.00') Credits,a.bank_date,A.NARATION,A.TYPE,A.rCODE,' ' as Touched,to_number(to_char(a.vchdate,'yyyymmdd')) as numdt,a.costcd,a.branchcd,nvl(a.refdate,a.vchdate) as refdate,nvl(a.tfcdr,0) as tfcdr,nvl(a.tfccr,0) as tfccr,a.naration as Remarks from voucher a where trim(a.acode)='"
                        + (txtbankcode.Value.Trim() + ("\' and a.branchcd<>\'88\' and a.vchdate "
                        + (prtstring + " "))));
        }

        if ((chkdispclear.Checked == false))
        {
            popsql = (popsql + (" and (bank_date is null OR BANK_dATE >TO_DATE(\'"
                        + (Convert.ToDateTime(txtrecondt.Text).ToString("dd/MM/yyyy") + "\',\'DD/MM/YYYY\'))")));
            seekSql = (@"select substr(x.invno,1,6) as invno,x.invdate,x.refnum CHEQUE_NO,to_char(x.dramt,'999999999990.00') Debits,to_char(x.cramt,'9999999990.00') Credits,x.bank_date,x.NARATION,x.TYPE,x.rCODE,' ' as Touched,to_number(to_char(x.vchdate,'yyyymmdd')) as numdt,'OPE' as costcd,x.branchcd,x.vchdate as refdate,0 as tfcdr,0 as tfccr,'-' as Remarks from voucherop x where x.branchcd<>'DD' and x.type='99' and trim(x.acode)='"
                        + (txtbankcode.Value.Trim() + ("\' and (x.bank_date is null OR x.BANK_dATE >TO_DATE(\'"
                        + (Convert.ToDateTime(txtrecondt.Text).ToString("dd/MM/yyyy") + "\',\'DD/MM/YYYY\'))"))));
        }

        if ((chkdispclear.Checked == true))
        {
            popsql = (popsql + (" and (BANK_dATE IS NULL OR TO_CHAR(bank_date,'dd/MM/yyyy') <=\'"
                        + (Convert.ToDateTime(txtrecondt.Text).ToString("dd/MM/yyyy") + ("\' OR TO_CHAR(VCHDATE,\'YYYYMM\') =\'"
                        + (Convert.ToDateTime(txtrecondt.Text).ToString("dd/MM/yyyy") + "\')")))));
            seekSql = (@"select substr(x.invno,1,6) as invno,x.invdate,x.refnum CHEQUE_NO,to_char(x.dramt,'999999999990.00') Debits,to_char(x.cramt,'9999999990.00') Credits,x.bank_date,x.NARATION,x.TYPE,x.rCODE,' ' as Touched,to_number(to_char(x.vchdate,'yyyymmdd')) as numdt,'OPE' as costcd,x.branchcd,x.vchdate as refdate,0 as tfcdr,0 as tfccr,'-' as Remarks from voucherop x where x.branchcd<>'DD' and x.type='99' and trim(x.acode)='"
                        + (txtbankcode.Value.Trim() + ("\' and (x.BANK_dATE IS NULL OR TO_CHAR(x.bank_date,'dd/MM/yyyy') =\'"
                        + (Convert.ToDateTime(txtrecondt.Text).ToString("dd/MM/yyyy") + ("\' OR TO_CHAR(x.invdate,'dd/MM/yyyy') =\'"
                        + (Convert.ToDateTime(txtrecondt.Text).ToString("dd/MM/yyyy") + "\')"))))));
        }

        string mhd;
        string mhd1;
        mhd1 = (popsql + (" union all "
                    + (seekSql + " ")));
        mhd = ("select * from ("
                    + (mhd1 + ") order by vchdate,vchnum,CHEQUE_NO,branchcd"));
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, mhd);
        if (dt.Rows.Count <= 0)
        {
            fgen.msg("", "ASMG", "No Uncleared Entries Found.");
            return;
        }

        if (dt.Rows.Count > 0)
        {
            i = 1;
            create_tab();
            foreach (DataRow dr in dt.Rows)
            {
                sg1_dr = sg1_dt.NewRow();

                sg1_dr["sg1_SrNo"] = i;
                sg1_dr["sg1_f1"] = dr["vchnum"].ToString().Trim();
                sg1_dr["sg1_f2"] = Convert.ToDateTime(dr["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                sg1_dr["sg1_F3"] = dr["cheque_no"].ToString().Trim();
                sg1_dr["sg1_F4"] = dr["debits"].ToString().Trim();

                sg1_dr["sg1_f5"] = dr["credits"].ToString().Trim();

                sg1_dr["sg1_t1"] = dr["bank_date"].ToString().Trim();
                sg1_dr["sg1_t2"] = dr["naration"].ToString().Trim();
                sg1_dr["sg1_t3"] = dr["type"].ToString().Trim();

                sg1_dr["sg1_t5"] = dr["rcode"].ToString().Trim();
                sg1_dr["sg1_t6"] = dr["touched"].ToString().Trim();
                sg1_dr["sg1_t7"] = dr["numdt"].ToString().Trim();
                sg1_dr["sg1_t8"] = dr["costcd"].ToString().Trim();

                sg1_dr["sg1_t9"] = dr["branchcd"].ToString().Trim();
                sg1_dr["sg1_t10"] = Convert.ToDateTime(dr["refdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                sg1_dr["sg1_t11"] = dr["tfcdr"].ToString().Trim();

                sg1_dr["sg1_t12"] = dr["tfccr"].ToString().Trim();
                sg1_dr["sg1_t13"] = dr["remarks"].ToString().Trim();

                sg1_dt.Rows.Add(sg1_dr);
                i++;
            }
            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            lblRowCount.Text = "Total Rows Showing : " + sg1.Rows.Count.ToString();

            ViewState["dtn"] = sg1_dt;

            dt.Dispose();
            fgen.EnableForm(this.Controls);
            disablectrl();
            setColHeadings();
            // edmode.Value = "Y";
        }

        drtot = 0;
        crtot = 0;
        for (i = 0; i <= sg1.Rows.Count - 1; i++)
        {
            //sg.text(i, -1) = (i + 1);

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text != "" && ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text != "-" && ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text != "OPE")
            {
                string name2prt;
                name2prt = "";
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text != "-")
                {
                    if ((((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().Trim().Length <= 2))
                    {
                        name2prt = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from type where id='C' and type1='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim() + "'", "name");
                    }
                    else
                    {
                        name2prt = fgen.seek_iname(frm_qstr, frm_cocd, "Select name from typegrp where id='AC' and type1='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim() + "'", "name");
                    }

                }

                ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text = name2prt;
                // 'seek_iname("select name from type where id='C' and type1='" & Trim(SG.Text(i, 11)) & "'", "name")
                ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).BackColor = System.Drawing.Color.Cyan;
            }
            else
            {
                //sg.text(i, 6) = seek_fam(sg.text(i, 8).Trim());
                ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where acode='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text + "'", "aname");

                if (((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text == "")
                {
                    //((TextBox)sg1.Rows[i].FindControl("sg1_t6")).BackColor = System.Drawing.Color.Red;
                    sg1.Rows[i].BackColor = System.Drawing.Color.Red;

                }
                else
                {
                    //((TextBox)sg1.Rows[i].FindControl("sg1_t6")).BackColor = System.Drawing.Color.White;
                    sg1.Rows[i].BackColor = System.Drawing.Color.White;
                }

            }

            if ((Check1.Checked == true))
            {
                if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() != "")
                {

                }
                else
                {
                    drtot = (drtot + double.Parse(sg1.Rows[i].Cells[17].Text.Trim()));
                    crtot = (crtot + double.Parse(sg1.Rows[i].Cells[16].Text.Trim()));
                }

            }
            else
            {
                drtot = (drtot + double.Parse(sg1.Rows[i].Cells[17].Text.Trim()));
                crtot = (crtot + double.Parse(sg1.Rows[i].Cells[16].Text.Trim()));
            }
        }

        // txtbalance.Value = "250000.00";
        // filledrows = i;
        // tpdeposit.text = Format(crtot, "0.00");

        if ((txtbalance.Value == "") || (txtbalance.Value == "-"))
        {

            txtbalance.Value = "0.00";

        }
        txtdepnotclr.Value = crtot.ToString();
        //tpwithdrawls.text = Format(drtot, "0.00");
        txtissnotclr.Value = drtot.ToString();
        // txtcalbank.Value = ((double.Parse(txtledgerbal.Value) - double.Parse(txtdepnotclr.Value))
        // + double.Parse(txtissnotclr.Value));

        txtcalbank.Value = Math.Round(((double.Parse(txtledgerbal.Value) - double.Parse(txtdepnotclr.Value))
                    + double.Parse(txtissnotclr.Value)), 2).ToString();

        txtdiff.Value = Math.Round((double.Parse(txtbalance.Value) - double.Parse(txtcalbank.Value)), 2).ToString();
        // tCbankbal.text = Format(tCbankbal.text, "0.00");
        // tdiff.text = Format(tdiff.text, "0.00");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL11", "N");
        btnsave.Disabled = false;


    }

    protected void btnautofilldt_ServerClick(object sender, EventArgs e)
    {
        fgen.msg("-", "SMSG", "Auto Fill All Bank Dates With Reconcilliation Date ?");

        autoflag = "1";
        fgenMV.Fn_Set_Mvar(frm_qstr, "COL12", "1");

    }

    // sorting the gridview on the basis of column clicked, if asc execute descending else ascending.
    protected void sg1_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable dtn = new DataTable();
        dtn = (DataTable)ViewState["dtn"];
        if (dtn != null)
        {
            string sortingDirection = string.Empty;

            if (dir == SortDirection.Ascending)
            {
                dir = SortDirection.Descending;
                sortingDirection = "Desc";

            }
            else
            {
                dir = SortDirection.Ascending;

                sortingDirection = "Asc";
            }

            DataView sortedView = new DataView(dtn);

            sortedView.Sort = e.SortExpression + " " + sortingDirection;

            sg1.DataSource = sortedView;

            sg1.DataBind();
        }

    }

    public SortDirection dir
    {
        get
        {
            if (ViewState["dirState"] == null)
            {

                ViewState["dirState"] = SortDirection.Ascending;

            }

            return (SortDirection)ViewState["dirState"];
        }
        set
        {
            ViewState["dirState"] = value;

        }

    }

    protected void btnupdate_ServerClick(object sender, EventArgs e)
    {
        if ((txtbankcode.Value == "-") || (txtbankcode.Value == ""))
        {
            fgen.msg("-", "AMSG", "No Bank is selected for the reconcilation...");
            txtbankcode.Focus();
            return;
        }

        string message = "";
        if ((frm_cocd == "NITP") || (frm_cocd == "NITC") || (frm_cocd == "MMSK") || (frm_cocd == "MMC") || (frm_cocd == "FIND") || (frm_cocd == "NITP") || (frm_cocd == "NITC"))
        {
            ctrl_r_dt = "Y";

        }
        ctrl_r_dt = "Y";
        for (int i = 0; i <= sg1.Rows.Count - 1; i++)
        {
            if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().Length > 1)
            {

                if (fgen.ChkDate(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text) == 1)
                {
                    if (((DateTime.Parse(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text) < DateTime.Parse(sg1.Rows[i].Cells[14].Text.Trim())) && (ctrl_r_dt == "Y")))
                    {

                        fgen.msg("", "ASMG", "Please check Date at Line No. " + ((i + 1) + ('\r' + ("Bank Date has been put Less than Voucher Date" + ('\r' + "Please correct !!")))));

                        return;


                    }
                    if (((DateTime.Parse(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text) < DateTime.Parse(((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text)) && (frm_cocd == "MEGH")))
                    {
                        fgen.msg("", "ASMG", "Please check Date at Line No. " + ((i + 1) + ('\r' + ("Bank Date has been put Less than Chq Date" + ('\r' + "Please correct !!")))));
                        return;

                    }
                    if (((DateTime.Parse(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text) > DateTime.Parse(txtrecondt.Text)) && (ctrl_r_dt == "Y")))
                    {

                        fgen.msg("", "ASMG", "Please check Date at Line No. " + ((i + 1) + ('\r' + ("Bank Date has been put More than Reco. Date" + ('\r' + "Please correct !!")))));
                        return;
                    }

                }

            }

        }

        for (int i = 0; i <= sg1.Rows.Count - 1; i++)
        {

            if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().Length > 2 || ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim() == "Y")
            {
                if ((1 == 1))
                {
                    if ((((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() == "") || (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim() == "-"))
                    {
                        if ((((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim() == "OPE"))
                        {
                            SQuery = "update voucherop set bank_date=null where branchcd='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim() + "' and invno='" + sg1.Rows[i].Cells[13].Text.Trim() + "' and invdate=to_date('" + sg1.Rows[i].Cells[14].Text.Trim() + "','dd/mm/yyyy')and type='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() + "' ";
                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        }
                        else
                        {
                            SQuery = "update voucher set bank_date=null where branchcd='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim() + "' and vchnum='" + sg1.Rows[i].Cells[13].Text.Trim() + "' and vchdate=to_date('" + sg1.Rows[i].Cells[14].Text.Trim() + "','dd/mm/yyyy') and type='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() + "' and trim(acode)='" + txtbankcode.Value.Trim() + "' and refnum='" + sg1.Rows[i].Cells[15].Text.Trim() + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        }

                    }
                    else
                    {
                        SQuery = "";
                        if (fgen.ChkDate(Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text).ToString("dd/MM/yyyy")) != 0)
                        {
                            if ((((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim() == "OPE"))
                            {
                                if (((sg1.Rows[i].Cells[14].Text.Trim() == "") || (sg1.Rows[i].Cells[14].Text.Trim() == "-")))
                                {
                                    SQuery = "update voucherop set bank_date=to_date('" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy') where branchcd='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim() + "' and invno='" + sg1.Rows[i].Cells[13].Text.Trim() + "' and invdate=to_date('" + sg1.Rows[i].Cells[14].Text.Trim() + "','dd/mm/yyyy') and type= '" + ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() + "' and rcode='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim() + "' and trim(acode)='" + txtbankcode.Value.Trim() + "'";
                                }
                                else
                                {
                                    SQuery = "update voucherop set bank_date=to_date('" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy') where branchcd='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim() + "' and invno= '" + sg1.Rows[i].Cells[13].Text.Trim() + "' and invdate=to_date('" + sg1.Rows[i].Cells[14].Text.Trim() + "','dd/mm/yyyy') and type='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() + "' and rcode='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim() + "' and trim(acode)='" + txtbankcode.Value.Trim() + "' and refnum='" + sg1.Rows[i].Cells[15].Text.Trim() + "'";
                                }

                            }
                            else if (((sg1.Rows[i].Cells[14].Text.Trim() == "") || (sg1.Rows[i].Cells[14].Text.Trim() == "-")))
                            {
                                SQuery = "update voucher set bank_date=to_date('" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy') where branchcd='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim() + "' and vchnum='" + sg1.Rows[i].Cells[13].Text.Trim() + "' and vchdate=to_date('" + sg1.Rows[i].Cells[14].Text.Trim() + "','dd/mm/yyyy') and type= '" + ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() + "'  and rcode= '" + ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim() + "' and trim(acode)='" + txtbankcode.Value.Trim() + "'";
                            }

                            else if (((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Length > 3 && ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.ToUpper() != "&NBSP;")
                            {
                                if (((ERP_M131_reco_upd_vch == "Y" || ERP_W1102_reco_upd_vch == "Y") && (((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().Substring(0, 2) == "16") || (((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().Substring(0, 2) == "06") || (((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().Substring(0, 2) == "05") || ((((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().Substring(0, 2) == "02") || (((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().Substring(0, 2) == "14"))))
                                {
                                    SQuery = "update voucher set bank_date=to_date('" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy') where branchcd='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim() + "' and vchnum='" + sg1.Rows[i].Cells[13].Text.Trim() + "' and vchdate=to_date('" + sg1.Rows[i].Cells[14].Text.Trim() + "','dd/mm/yyyy') and type='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() + "' and trim(refnum)= '" + sg1.Rows[i].Cells[15].Text.Trim() + "'";
                                }
                                else
                                    SQuery = "update voucher set bank_date=to_date('" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy') where branchcd='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim() + "' and vchnum= '" + sg1.Rows[i].Cells[13].Text.Trim() + "' and vchdate=to_date('" + sg1.Rows[i].Cells[14].Text.Trim() + "','dd/mm/yyyy') and type='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() + "' and trim(rcode)= '" + ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim() + "' and trim(acode)='" + txtbankcode.Value.Trim() + "' and trim(refnum)='" + sg1.Rows[i].Cells[15].Text.Trim() + "'";
                            }
                            else
                            {
                                SQuery = "update voucher set bank_date=to_date('" + Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim()).ToString("dd/MM/yyyy") + "','dd/MM/yyyy') where branchcd='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim() + "' and vchnum= '" + sg1.Rows[i].Cells[13].Text.Trim() + "' and vchdate=to_date('" + sg1.Rows[i].Cells[14].Text.Trim() + "','dd/mm/yyyy') and type='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim() + "' and trim(rcode)= '" + ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim() + "' and trim(acode)='" + txtbankcode.Value.Trim() + "' and trim(refnum)='" + sg1.Rows[i].Cells[15].Text.Trim() + "'";
                            }

                            if (SQuery != "")
                                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);
                        }

                    }

                }

            }

        }
        gridfill();
    }

    public void updatetouched()
    {
        for (int j = 0; j < sg1.Rows.Count; j++)
        {
            if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Length > 2)
            {
                ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text = "Y";
            }
        }
    }
    string acBal(string selecAcode)
    {
        string xprd1 = "between to_date('" + frm_CDT1 + "','dd/mm/yyyy') and to_date('" + txtrecondt.Text + "','dd/mm/yyyy') ";


        //string SQueryx = "select sum(opb)+sum(inbal)-sum(outbal) as bal from (select sum(yr_" + frm_myear + ") as opb,0 as inbal,0 as outbal from famstbal where branchcd IN ('" + frm_mbr + "') and acode  in ('" + selecAcode + "') group by acode union all select sum(nvl(DRAMT,0))-sum(nvl(CRAMT,0)) as obal,0 as inbal,0 as outbal from voucher where branchcd IN ('" + frm_mbr + "') and VCHDATE " + xprd1 + " and acode  in ('" + selecAcode + "') union all select 0 as opbal,(case when sum(A.DRAMT)-sum(A.CRAMT)>0 then ABS(sum(A.DRAMT)-sum(A.CRAMT)) else 0 end) AS IQTYIN,(case when sum(A.DRAMT)-sum(A.CRAMT)>0 then 0 else abs(sum(A.DRAMT)-sum(A.CRAMT)) end) AS IQTYOUT from voucher A where a.branchcd IN ('" + frm_mbr + "') and A.VCHDATE " + DateRange + " AND A.ACODE  IN ('" + selecAcode + "') )";
        string SQueryx = "select sum(opb)+sum(inbal)-sum(outbal) as bal from (select nvl(yr_" + frm_myear + ",0) as opb,0 as inbal,0 as outbal from famstbal where branchcd!='DD' and acode='" + selecAcode + "' union all select 0 as opbal,dramt,cramt from voucher A where a.branchcd!='88' and a.branchcd!='DD' and A.VCHDATE " + xprd1 + " AND A.ACODE='" + selecAcode + "' )";
        
        return fgen.seek_iname(frm_qstr, frm_cocd, SQueryx, "BAL");
    }
}