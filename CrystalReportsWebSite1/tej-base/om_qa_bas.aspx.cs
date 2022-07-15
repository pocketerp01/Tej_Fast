using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_qa_bas : System.Web.UI.Page
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
                doc_addl.Value = "1";

                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();
            btndel.Visible = false;
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
        
        //#region hide hidden columns
        //sg1.Columns[0].Visible = false;
        //sg1.Columns[1].Visible = false;
        //sg1.Columns[2].Visible = false;
        //sg1.Columns[3].Visible = false;
        //sg1.Columns[4].Visible = false;
        //sg1.Columns[5].Visible = false;
        //sg1.Columns[6].Visible = false;
        //sg1.Columns[7].Visible = false;
        //sg1.Columns[8].Visible = false;
        //sg1.Columns[9].Visible = false;
        //#endregion

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



                //((TextBox)sg1.Rows[K].FindControl("sg1_t7")).Attributes.Add("readonly", "readonly");
                //((TextBox)sg1.Rows[K].FindControl("sg1_t8")).Attributes.Add("readonly", "readonly");
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
                if (sR > 10)
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

        //for (int sR = 0; sR < sg1.Columns.Count; sR++)
        //{
        //    string orig_name;
        //    double tb_Colm;

        //    #region hide hidden columns
        //    for (int i = 2; i < 10; i++)
        //    {
        //        sg1.Columns[i].HeaderStyle.CssClass = "hidden";
        //        sg1.Rows[sR].Cells[i].CssClass = "hidden";
        //    }
        //    #endregion

        //    tb_Colm = fgen.make_double(fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "COL_NO"));
        //    orig_name = sg1.HeaderRow.Cells[sR].Text.Trim();

        //    for (int K = 0; K < sg1.Rows.Count; K++)
        //    {
        //        if (orig_name.ToLower().Contains("sg1_t")) ((TextBox)sg1.Rows[K].FindControl(orig_name.ToLower())).MaxLength = fgen.make_int(fgen.seek_iname_dt(dtCol, "OBJ_NAME='" + orig_name + "'", "OBJ_MAXLEN"));

        //    }
        //    orig_name = orig_name.ToUpper();
        //    //if (sg1.HeaderRow.Cells[sR].Text.Trim().ToUpper() == fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_NAME"))
        //    if (sR == tb_Colm)
        //    {
        //        // hidding column
        //        if (fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_VISIBLE") == "N")
        //        {
        //            sg1.Columns[sR].Visible = false;
        //        }
        //        // Setting Heading Name
        //        sg1.HeaderRow.Cells[sR].Text = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_CAPTION");
        //        // Setting Col Width
        //        string mcol_width = fgen.seek_iname_dt(dtCol, "COL_NO=" + sR + "", "OBJ_WIDTH");
        //        if (fgen.make_double(mcol_width) > 0)
        //        {
        //            sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
        //            sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
        //        }
        //    }
        //}

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
        frm_tabname = "ivoucher";

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

            case "TACODE":
                SQuery = "SELECT distinct a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum AS FSTR,trim(a.Vchnum) as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,b.Aname as Supplier,a.Invno,A.Refnum as chl_no,a.type,a.Ent_by,to_char(a.vchdate,'yyyymmdd') As vdd from ivoucher a ,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + DateRange + " and NVL(a.inspected,'N')='N' order by vdd desc ,a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum ";
                break;
            case "TICODE":
                //Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                //pquery = "select trim(icode) as icode,sum(cnt) as tot from (select icode,1 as cnt from item where length(trim(nvl(deac_by,'-')))<=1 and length(trim(icode))>4 union all select distinct icode,-1 as cnt from inspmst where branchcd!='DD' and type='" + lbl1a.Text + "' and trim(Acode)='" + txtlbl4.Text.Trim() + "') group by trim(icode) having sum(cnt)>0 ";
                //SQuery = "SELECT a.Icode AS FSTR,trim(b.Iname) as Item_name,a.Icode,b.Cpartno,b.Cdrgno,b.unit from ("+ pquery +")a ,Item b where trim(A.icode)=trim(B.icode) and length(trim(nvl(b.deac_by,'-')))<=1 and length(trim(b.icode))>4 and substr(b.icode,1,1) in ('7','8','9') order by b.iname";
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":

                //SQuery = "SELECT userid AS FSTR,Full_Name AS Client_Name,username as CCode FROM evas where branchcd!='DD' and username!='-' and userid>'000052' and trim(userid) not in (select trim(Ccode) from wb_oms_log where branchcd!='DD' and to_char(opldt,'yyyymm')=to_char(to_DaTE('" + txtvchdate.Text  + "','dd/mm/yyyy'),'yyyymm')) order by Username";
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
            case "FGQ":
                if (frm_vty == "16")
                {
                    SQuery = "select a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')as fstr, a.vchnum||'  '||decode(trim(nvl(a.INSPECTED,'Q')),'N','(After QC)','(QC Pend)') as Slip_No ,a.vchdate , a.type,B.INAME,B.CPARTNO,A.INVNO AS JOB_NO from ivoucher a ,ITEM B where TRIM(A.ICODE)=TRIM(B.ICODE) AND a.type='" + col1 + "' and a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " and a.inspected!='Y' AND A.inspected!='X' order by a.vchdate desc,a.vchnum desC";
                }
                else
                {
                    SQuery = "select distinct a.vchnum||to_char(a.vchdate,'dd/mm/yyyy')as fstr, a.vchnum as Prod_Doc_No,to_char(a.vchdate,'dd/mm/yyyy') as Prod_Doc_Dt,a.type,a.qcDate,a.Ent_by,to_char(a.vchdate,'yyyymmdd') as Vdd from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type='" + col1 + "' and  a.vchdate " + DateRange + " and a.Store!='Y' order by vdd desc,a.vchnum desc";
                }
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD")
                    SQuery = "SELECT distinct a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum AS FSTR,trim(a.Vchnum) as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,b.Aname as Supplier,a.Invno,A.Refnum as chl_no,a.ent_by,a.pname as insp_by,to_char(A.vchdate,'yyyymmdd') as vdd from ivoucher a ,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + DateRange + " and NVL(a.inspected,'N')='Y' and a.store<>'R' order by vdd desc,a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum DESC ";
                //SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Templ_no,to_char(a.vchdate,'dd/mm/yyyy') as Templ_Dt,b.Iname,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' order by vdd desc,a.vchnum desc";
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


            frm_vty = "10";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);

            if (frm_formID == "F30144") typePopup = "Y";
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
    void newCase(string vty)
    {
        #region
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();

        sg1_dt = new DataTable();
        create_tab();
        int j;
        for (j = i; j < 10; j++)
        {
            sg1_add_blankrows();
        }


        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;

        if (frm_formID == "F30144")
        {
            hffield.Value = "FGQ";
            make_qry_4_popup();
            fgen.Fn_open_sseek("-", frm_qstr);
        }

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

        string chk_freeze = "";
        chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1041", txtvchdate.Text.Trim());
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

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        foreach (GridViewRow gr1 in sg1.Rows)
        {
            TextBox t1 = (TextBox)gr1.FindControl("sg1_t1");
            TextBox t2 = (TextBox)gr1.FindControl("sg1_t2");
            TextBox t3 = (TextBox)gr1.FindControl("sg1_t3");
            if (fgen.make_double(t2.Text) == 0 && fgen.make_double(t3.Text) == 0)
            {
                fgen.msg("-", "AMSG", "Both Value Can not be Zero!!'13'Check Item : " + gr1.Cells[13].Text.Trim());
                return;
            }
            if (fgen.make_double(t2.Text) > fgen.make_double(t1.Text))
            {
                fgen.msg("-", "AMSG", "Accepted Qty Can not be greater then incoming Qty!!'13'Check Item : " + gr1.Cells[13].Text.Trim());
                return;
            }
            if (fgen.make_double(t3.Text) > fgen.make_double(t1.Text))
            {
                fgen.msg("-", "AMSG", "Rejection Qty Can not be greater then incoming Qty!!'13'Check Item : " + gr1.Cells[13].Text.Trim());
                return;
            }
        }

        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) > 0)
            {
                ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text = (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text) - fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text)).ToString();
            }

            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) < 0)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Rejn Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");
                return;
            }
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text) < 0)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Accepted Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");
                return;
            }
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) > 0 && ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Length < 2)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , In case of Rejn , Remarks Not Filled Correctly at Line " + (i + 1) + "  !!");
                return;
            }

            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text, 3) + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text, 3), 3) != fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text, 3))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Acpt Qty + Rejn Quantity Not Equal to Rcvd Qty , Please Check at Line " + (i + 1) + "  !!");
                return;
            }
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
        fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {

        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select MRR for Print", frm_qstr);

        //SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,round((sum(rej_rw)/sum(iqty_chl))*100,2) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from ivoucher a where a.branchcd='" + frm_mbr + "' and a.vchdate " + DateRange + " and type like '0%' and a.store='Y' group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
        //fgen.Fn_FillChart(frm_cocd, frm_qstr, "Inward Quality Rejn Trend", "line", "Month Wise", "Incoming Rejection %", SQuery, "");

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
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                //// Deleing data from Sr Ctrl Table
                //fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");


                //// Saving Deleting History
                //fgen.save_info(frm_qstr,frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                //fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                //clearctrl(); fgen.ResetForm(this.Controls);
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
                    hffield.Value = btnval + "_E";
                    make_qry_4_popup();

                    fgen.Fn_open_mseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_click
                    if (col1.Length <= 0) return;

                    SQuery = "Select b.iname,b.cpartno,b.cdrgno,b.unit,a.morder,a.*,to_chaR(a.invdate,'dd/mm/yyyy') as pinvdate,to_chaR(a.refdate,'dd/mm/yyyy') as prefdate from " + frm_tabname + " a,item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||to_Char(a.vchdate,'yyyymmdd')||trim(a.vchnum)='" + col1 + "' and a.store<>'R' ORDER BY A.morder";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl2.Text = dt.Rows[i]["invno"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["pinvdate"].ToString().Trim();

                        txtlbl5.Text = dt.Rows[i]["refnum"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["prefdate"].ToString().Trim();

                        txtlbl4.Text = dt.Rows[i]["type"].ToString().Trim();
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='M' and trim(upper(type1))=upper(Trim('" + txtlbl4.Text + "'))", "name");

                        txtlbl7.Text = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where trim(upper(acode))=upper(Trim('" + txtlbl7.Text + "'))", "aname");

                        txtlbl8.Text = frm_uname;
                        txtlbl9.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[i]["ponum"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["podate"].ToString().Trim();

                            sg1_dr["sg1_h3"] = dt.Rows[i]["rgpnum"].ToString().Trim();
                            sg1_dr["sg1_h4"] = dt.Rows[i]["rgpdate"].ToString().Trim();

                            sg1_dr["sg1_h5"] = dt.Rows[i]["irate"].ToString().Trim();
                            sg1_dr["sg1_h6"] = dt.Rows[i]["potype"].ToString().Trim();

                            sg1_dr["sg1_h7"] = dt.Rows[i]["genum"].ToString().Trim();
                            sg1_dr["sg1_h8"] = dt.Rows[i]["gedate"].ToString().Trim();
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";

                            sg1_dr["sg1_f1"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = fgen.make_double(dt.Rows[i]["iqtyin"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["rej_rw"].ToString().Trim());
                            sg1_dr["sg1_t2"] = dt.Rows[i]["iqtyin"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["rej_rw"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["rej_sdp"].ToString().Trim();

                            sg1_dr["sg1_t5"] = dt.Rows[i]["rej_sdv"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["iexc_addl"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["purpose"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["btchno"].ToString().Trim();

                            sg1_dr["sg1_t9"] = dt.Rows[i]["btchdt"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["tc_no"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["morder1"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
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
                        edmode.Value = "Y";
                    }
                    break;
                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;
                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1002");
                    fgen.fin_invn_reps(frm_qstr);
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F30141");
                    //fgen.fin_qa_reps(frm_qstr);
                    break;
                case "FGQ":
                    if (col1.Length <= 0) return;

                    SQuery = "Select b.iname,b.cpartno,b.cdrgno,b.unit,a.morder,a.*,to_chaR(a.invdate,'dd/mm/yyyy') as pinvdate,to_chaR(a.refdate,'dd/mm/yyyy') as prefdate from " + frm_tabname + " a,item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.morder";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", frm_mbr + frm_vty + col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl2.Text = dt.Rows[i]["invno"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["pinvdate"].ToString().Trim();

                        txtlbl5.Text = dt.Rows[i]["refnum"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["prefdate"].ToString().Trim();

                        txtlbl4.Text = dt.Rows[i]["type"].ToString().Trim();
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='M' and trim(upper(type1))=upper(Trim('" + txtlbl4.Text + "'))", "name");

                        txtlbl7.Text = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where trim(upper(acode))=upper(Trim('" + txtlbl7.Text + "'))", "aname");

                        txtlbl8.Text = frm_uname;
                        txtlbl9.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[i]["ponum"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["podate"].ToString().Trim();

                            sg1_dr["sg1_h3"] = dt.Rows[i]["rgpnum"].ToString().Trim();
                            sg1_dr["sg1_h4"] = dt.Rows[i]["rgpdate"].ToString().Trim();

                            sg1_dr["sg1_h5"] = dt.Rows[i]["irate"].ToString().Trim();
                            sg1_dr["sg1_h6"] = dt.Rows[i]["potype"].ToString().Trim();

                            sg1_dr["sg1_h7"] = dt.Rows[i]["genum"].ToString().Trim();
                            sg1_dr["sg1_h8"] = dt.Rows[i]["gedate"].ToString().Trim();
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";

                            sg1_dr["sg1_f1"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["iqtyin"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["iqtyin"].ToString().Trim();

                            sg1_dr["sg1_t3"] = 0;
                            sg1_dr["sg1_t4"] = 0;
                            sg1_dr["sg1_t5"] = 0;
                            sg1_dr["sg1_t6"] = 0;
                            sg1_dr["sg1_t7"] = dt.Rows[i]["purpose"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["btchno"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["btchdt"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["tc_no"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["morder1"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
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
                        //edmode.Value = "Y";
                    }
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;

                    SQuery = "Select b.iname,b.cpartno,b.cdrgno,b.unit,a.morder,a.*,to_chaR(a.invdate,'dd/mm/yyyy') as pinvdate,to_chaR(a.refdate,'dd/mm/yyyy') as prefdate from " + frm_tabname + " a,item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||to_Char(a.vchdate,'yyyymmdd')||trim(a.vchnum)='" + col1 + "' and a.store<>'R' ORDER BY A.morder";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl2.Text = dt.Rows[i]["invno"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["pinvdate"].ToString().Trim();

                        txtlbl5.Text = dt.Rows[i]["refnum"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["prefdate"].ToString().Trim();

                        txtlbl4.Text = dt.Rows[i]["type"].ToString().Trim();
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='M' and trim(upper(type1))=upper(Trim('" + txtlbl4.Text + "'))", "name");

                        txtlbl7.Text = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where trim(upper(acode))=upper(Trim('" + txtlbl7.Text + "'))", "aname");

                        txtlbl8.Text = frm_uname;
                        txtlbl9.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            sg1_dr["sg1_h1"] = dt.Rows[i]["ponum"].ToString().Trim();
                            sg1_dr["sg1_h2"] = dt.Rows[i]["podate"].ToString().Trim();

                            sg1_dr["sg1_h3"] = dt.Rows[i]["rgpnum"].ToString().Trim();
                            sg1_dr["sg1_h4"] = dt.Rows[i]["rgpdate"].ToString().Trim();

                            sg1_dr["sg1_h5"] = dt.Rows[i]["irate"].ToString().Trim();
                            sg1_dr["sg1_h6"] = dt.Rows[i]["potype"].ToString().Trim();

                            sg1_dr["sg1_h7"] = dt.Rows[i]["genum"].ToString().Trim();
                            sg1_dr["sg1_h8"] = dt.Rows[i]["gedate"].ToString().Trim();
                            sg1_dr["sg1_h9"] = "-";
                            sg1_dr["sg1_h10"] = "-";

                            sg1_dr["sg1_f1"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["cdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["unit"].ToString().Trim();

                            sg1_dr["sg1_t1"] = dt.Rows[i]["iqtyin"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["iqtyin"].ToString().Trim();

                            sg1_dr["sg1_t3"] = 0;
                            sg1_dr["sg1_t4"] = 0;
                            sg1_dr["sg1_t5"] = 0;
                            sg1_dr["sg1_t6"] = 0;
                            sg1_dr["sg1_t7"] = dt.Rows[i]["purpose"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["btchno"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["btchdt"].ToString().Trim();
                            sg1_dr["sg1_t10"] = dt.Rows[i]["tc_no"].ToString().Trim();
                            sg1_dr["sg1_t11"] = dt.Rows[i]["morder1"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
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

                case "TICODE":
                    if (col1.Length <= 0) return;
                    txtlbl7.Text = col1;
                    txtlbl7a.Text = col2;
                    txtlbl2.Focus();
                    break;
                case "SG1_ROW_ADD":
                    //#region for gridview 1
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
                    //        sg1_dr["sg1_srno"] = Convert.ToInt32(dt.Rows[i]["sg1_srno"].ToString());
                    //        sg1_dr["sg1_h1"] = dt.Rows[i]["sg1_h1"].ToString();
                    //        sg1_dr["sg1_h2"] = dt.Rows[i]["sg1_h2"].ToString();
                    //        sg1_dr["sg1_h3"] = dt.Rows[i]["sg1_h3"].ToString();
                    //        sg1_dr["sg1_h4"] = dt.Rows[i]["sg1_h4"].ToString();
                    //        sg1_dr["sg1_h5"] = dt.Rows[i]["sg1_h5"].ToString();
                    //        sg1_dr["sg1_h6"] = dt.Rows[i]["sg1_h6"].ToString();
                    //        sg1_dr["sg1_h7"] = dt.Rows[i]["sg1_h7"].ToString();
                    //        sg1_dr["sg1_h8"] = dt.Rows[i]["sg1_h8"].ToString();
                    //        sg1_dr["sg1_h9"] = dt.Rows[i]["sg1_h9"].ToString();
                    //        sg1_dr["sg1_h10"] = dt.Rows[i]["sg1_h10"].ToString();

                    //        sg1_dr["sg1_f1"] = dt.Rows[i]["sg1_f1"].ToString();
                    //        sg1_dr["sg1_f2"] = dt.Rows[i]["sg1_f2"].ToString();
                    //        sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                    //        sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                    //        sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                    //        sg1_dr["sg1_t1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                    //        sg1_dr["sg1_t2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim();
                    //        sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                    //        sg1_dr["sg1_t4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim();
                    //        sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                    //        sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                    //        sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                    //        sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                    //        sg1_dt.Rows.Add(sg1_dr);
                    //    }

                    //    dt = new DataTable();
                    //    if (col1.Length > 6) SQuery = "select * from evas where trim(userid) in (" + col1 + ")";
                    //    else SQuery = "select * from evas where trim(userid)='" + col1 + "'";
                    //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    //    for (int d = 0; d < dt.Rows.Count; d++)
                    //    {
                    //        sg1_dr = sg1_dt.NewRow();
                    //        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                    //        sg1_dr["sg1_h1"] = dt.Rows[d]["userid"].ToString().Trim();
                    //        sg1_dr["sg1_h2"] = dt.Rows[d]["username"].ToString().Trim();
                    //        sg1_dr["sg1_h3"] = "-";
                    //        sg1_dr["sg1_h4"] = "-";
                    //        sg1_dr["sg1_h5"] = "-";
                    //        sg1_dr["sg1_h6"] = "-";
                    //        sg1_dr["sg1_h7"] = "-";
                    //        sg1_dr["sg1_h8"] = "-";
                    //        sg1_dr["sg1_h9"] = "-";
                    //        sg1_dr["sg1_h10"] = "-";

                    //        sg1_dr["sg1_f1"] = dt.Rows[d]["USERID"].ToString().Trim();
                    //        sg1_dr["sg1_f2"] = dt.Rows[d]["full_Name"].ToString().Trim();
                    //        sg1_dr["sg1_f3"] = dt.Rows[d]["username"].ToString().Trim();
                    //        sg1_dr["sg1_f4"] = dt.Rows[d]["contactno"].ToString().Trim();
                    //        sg1_dr["sg1_f5"] = dt.Rows[d]["emailid"].ToString().Trim();

                    //        sg1_dr["sg1_t1"] = "";
                    //        sg1_dr["sg1_t2"] = "";
                    //        sg1_dr["sg1_t3"] = "";
                    //        sg1_dr["sg1_t4"] = "";
                    //        sg1_dr["sg1_t5"] = "";
                    //        sg1_dr["sg1_t6"] = "";
                    //        sg1_dr["sg1_t7"] = "";
                    //        sg1_dr["sg1_t8"] = "";
                    //        sg1_dr["sg1_t9"] = "";
                    //        sg1_dr["sg1_t10"] = "";
                    //        sg1_dr["sg1_t11"] = "";
                    //        sg1_dr["sg1_t12"] = "";
                    //        sg1_dr["sg1_t13"] = "";
                    //        sg1_dr["sg1_t14"] = "";
                    //        sg1_dr["sg1_t15"] = "";
                    //        sg1_dr["sg1_t16"] = "";

                    //        sg1_dt.Rows.Add(sg1_dr);
                    //    }
                    //}
                    //sg1_add_blankrows();

                    //ViewState["sg1"] = sg1_dt;
                    //sg1.DataSource = sg1_dt;
                    //sg1.DataBind();
                    //dt.Dispose(); sg1_dt.Dispose();
                    //((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    //#endregion
                    setColHeadings();
                    break;
                case "SG1_ROW_ADD_E":
                    //if (col1.Length <= 0) return;
                    ////********* Saving in Hidden Field 
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                    ////********* Saving in GridView Value
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                    //sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                    //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    setColHeadings();
                    break;
                case "SG3_ROW_ADD":
                    //#region for gridview 1
                    //if (col1.Length <= 0) return;
                    //if (ViewState["sg3"] != null)
                    //{
                    //    dt = new DataTable();
                    //    sg3_dt = new DataTable();
                    //    dt = (DataTable)ViewState["sg3"];
                    //    z = dt.Rows.Count - 1;
                    //    sg3_dt = dt.Clone();
                    //    sg3_dr = null;
                    //    for (i = 0; i < dt.Rows.Count - 1; i++)
                    //    {
                    //        sg3_dr = sg3_dt.NewRow();
                    //        sg3_dr["sg3_srno"] = Convert.ToInt32(dt.Rows[i]["sg3_srno"].ToString());
                    //        sg3_dr["sg3_f1"] = dt.Rows[i]["sg3_f1"].ToString();
                    //        sg3_dr["sg3_f2"] = dt.Rows[i]["sg3_f2"].ToString();
                    //        sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                    //        sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                    //        sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                    //        sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();
                    //        sg3_dt.Rows.Add(sg3_dr);
                    //    }

                    //    dt = new DataTable();
                    //    if (col1.Length > 8) SQuery = "select * from item where trim(icode) in (" + col1 + ")";
                    //    else SQuery = "select * from item where trim(icode)='" + col1 + "'";
                    //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    //    for (int d = 0; d < dt.Rows.Count; d++)
                    //    {
                    //        sg3_dr = sg3_dt.NewRow();
                    //        sg3_dr["sg3_srno"] = sg3_dt.Rows.Count + 1;

                    //        sg3_dr["sg3_f1"] = dt.Rows[d]["icode"].ToString().Trim();
                    //        sg3_dr["sg3_f2"] = dt.Rows[d]["iname"].ToString().Trim();
                    //        sg3_dr["sg3_t1"] = "";
                    //        sg3_dr["sg3_t2"] = "";
                    //        sg3_dr["sg3_t3"] = "";
                    //        sg3_dr["sg3_t4"] = "";
                    //        sg3_dt.Rows.Add(sg3_dr);
                    //    }
                    //}
                    //sg3_add_blankrows();

                    //ViewState["sg3"] = sg3_dt;
                    //sg3.DataSource = sg3_dt;
                    //sg3.DataBind();
                    //dt.Dispose(); sg3_dt.Dispose();
                    //((TextBox)sg3.Rows[z].FindControl("sg3_t1")).Focus();
                    //#endregion
                    break;


                case "SG2_RMV":
                    //#region Remove Row from GridView
                    //if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    //{
                    //    dt = new DataTable();
                    //    sg2_dt = new DataTable();
                    //    dt = (DataTable)ViewState["sg2"];
                    //    z = dt.Rows.Count - 1;
                    //    sg2_dt = dt.Clone();
                    //    sg2_dr = null;
                    //    i = 0;
                    //    for (i = 0; i < sg2.Rows.Count - 1; i++)
                    //    {
                    //        sg2_dr = sg2_dt.NewRow();
                    //        sg2_dr["sg2_srno"] = (i + 1);

                    //        sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                    //        sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();


                    //        sg2_dt.Rows.Add(sg2_dr);
                    //    }

                    //    sg2_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                    //    sg2_add_blankrows();

                    //    ViewState["sg2"] = sg2_dt;
                    //    sg2.DataSource = sg2_dt;
                    //    sg2.DataBind();
                    //}
                    //#endregion
                    //setColHeadings();
                    break;
                case "SG3_RMV":
                    //#region Remove Row from GridView
                    //if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    //{
                    //    dt = new DataTable();
                    //    sg3_dt = new DataTable();
                    //    dt = (DataTable)ViewState["sg3"];
                    //    z = dt.Rows.Count - 1;
                    //    sg3_dt = dt.Clone();
                    //    sg3_dr = null;
                    //    i = 0;
                    //    for (i = 0; i < sg3.Rows.Count - 1; i++)
                    //    {
                    //        sg3_dr = sg3_dt.NewRow();
                    //        sg3_dr["sg3_srno"] = (i + 1);
                    //        sg3_dr["sg3_f1"] = sg3.Rows[i].Cells[3].Text.Trim();
                    //        sg3_dr["sg3_f2"] = sg3.Rows[i].Cells[4].Text.Trim();

                    //        sg3_dr["sg3_t1"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t1")).Text.Trim();
                    //        sg3_dr["sg3_t2"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t2")).Text.Trim();
                    //        sg3_dr["sg3_t3"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t3")).Text.Trim();
                    //        sg3_dr["sg3_t4"] = ((TextBox)sg3.Rows[i].FindControl("sg3_t4")).Text.Trim();

                    //        sg3_dt.Rows.Add(sg3_dr);
                    //    }

                    //    sg3_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                    //    sg3_add_blankrows();

                    //    ViewState["sg3"] = sg3_dt;
                    //    sg3.DataSource = sg3_dt;
                    //    sg3.DataBind();
                    //}
                    //#endregion
                    //setColHeadings();
                    break;

                case "SG1_RMV":
                    //#region Remove Row from GridView
                    //if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                    //{
                    //    dt = new DataTable();
                    //    sg1_dt = new DataTable();
                    //    dt = (DataTable)ViewState["sg1"];
                    //    z = dt.Rows.Count - 1;
                    //    sg1_dt = dt.Clone();
                    //    sg1_dr = null;
                    //    i = 0;
                    //    for (i = 0; i < sg1.Rows.Count - 1; i++)
                    //    {
                    //        sg1_dr = sg1_dt.NewRow();
                    //        sg1_dr["sg1_srno"] = (i + 1);
                    //        sg1_dr["sg1_h1"] = sg1.Rows[i].Cells[0].Text.Trim();
                    //        sg1_dr["sg1_h2"] = sg1.Rows[i].Cells[1].Text.Trim();
                    //        sg1_dr["sg1_h3"] = sg1.Rows[i].Cells[2].Text.Trim();
                    //        sg1_dr["sg1_h4"] = sg1.Rows[i].Cells[3].Text.Trim();
                    //        sg1_dr["sg1_h5"] = sg1.Rows[i].Cells[4].Text.Trim();
                    //        sg1_dr["sg1_h6"] = sg1.Rows[i].Cells[5].Text.Trim();
                    //        sg1_dr["sg1_h7"] = sg1.Rows[i].Cells[6].Text.Trim();
                    //        sg1_dr["sg1_h8"] = sg1.Rows[i].Cells[7].Text.Trim();
                    //        sg1_dr["sg1_h9"] = sg1.Rows[i].Cells[8].Text.Trim();
                    //        sg1_dr["sg1_h10"] = sg1.Rows[i].Cells[9].Text.Trim();

                    //        sg1_dr["sg1_f1"] = sg1.Rows[i].Cells[13].Text.Trim();
                    //        sg1_dr["sg1_f2"] = sg1.Rows[i].Cells[14].Text.Trim();
                    //        sg1_dr["sg1_f3"] = sg1.Rows[i].Cells[15].Text.Trim();
                    //        sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[16].Text.Trim();
                    //        sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[17].Text.Trim();

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
                    //        sg1_dr["sg1_t11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text.Trim();
                    //        sg1_dr["sg1_t12"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t12")).Text.Trim();
                    //        sg1_dr["sg1_t13"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t13")).Text.Trim();
                    //        sg1_dr["sg1_t14"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t14")).Text.Trim();
                    //        sg1_dr["sg1_t15"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t15")).Text.Trim();
                    //        sg1_dr["sg1_t16"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t16")).Text.Trim();

                    //        sg1_dt.Rows.Add(sg1_dr);
                    //    }

                    //    if (edmode.Value == "Y")
                    //    {
                    //        //sg1_dr["sg1_f1"] = "*" + sg1.Rows[-1 + Convert.ToInt32(hf1.Value.Trim())].Cells[13].Text.Trim();

                    //        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                    //    }
                    //    else
                    //    {
                    //        sg1_dt.Rows[Convert.ToInt32(hf1.Value.Trim())].Delete();
                    //    }

                    //    sg1_add_blankrows();

                    //    ViewState["sg1"] = sg1_dt;
                    //    sg1.DataSource = sg1_dt;
                    //    sg1.DataBind();
                    //}
                    //#endregion
                    //setColHeadings();
                    break;
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {

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
            SQuery = "SELECT trim(a.Vchnum) as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,b.Aname as Supplier,a.Invno,A.Refnum as chl_no,a.Iqty_chl as Supp_Qty,a.iqtyin+nvl(a.rej_rw,0) as Rcv_qty,a.acpt_ud as Acpt_qty,a.rej_Rw as Rejn,round((a.rej_Rw/a.iqty_chl)*100,2) as Rejn_percent,round((a.rej_Rw/a.iqty_chl)*1000000,2) as Rejn_PPM,a.ent_by,a.pname as insp_by,a.qcdate,a.purpose as rej_rmk,a.tc_no,a.btchno,to_char(A.vchdate,'yyyymmdd') as vdd from ivoucher a ,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + PrdRange + " and NVL(a.inspected,'N')='Y' and a.store<>'R' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by vdd desc,a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum  ";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------
            if (txtlbl4.Text.Trim().Length < 2)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , MRR Type Not Filled Correctly !!");
                return;
            }

            string last_entdt;

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
                        for (i = 0; i < sg1.Rows.Count - 0; i++)
                        {
                            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text) + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) == fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text))
                            {
                                cmd_query = "update " + frm_tabname + " set qc_date=sysdate,qcdate=to_datE(sysdate,'dd/mm/yyyy'),inspected='Y',store='Y',pname='" + frm_uname + "',iqtyin=" + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text) + ",rej_rw=" + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) + ",acpt_ud=" + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text) + ",rej_sdp=" + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text) + ",rej_sdv=" + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text) + ",purpose='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text + "',tc_no='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text + "' where branchcd||type||to_char(" + doc_df.Value + ",'yyyymmdd')||trim(" + doc_nf.Value + ")||trim(icode)||trim(morder)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + sg1.Rows[i].Cells[13].Text.Trim() + (((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text) + "' and store<>'R' ";
                                if (frm_formID == "F30144") cmd_query = "update " + frm_tabname + " set qc_date=sysdate,qcdate=to_datE(sysdate,'dd/mm/yyyy'),inspected='N',store='Q',pname='" + frm_uname + "',iqtyin=" + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text) + ",rej_rw=" + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) + ",acpt_ud=" + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text) + ",rej_sdp=" + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text) + ",rej_sdv=" + fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text) + ",purpose='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text + "',tc_no='" + ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text + "' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')||trim(icode)||trim(morder)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + sg1.Rows[i].Cells[13].Text.Trim() + (((TextBox)sg1.Rows[i].FindControl("sg1_t11")).Text) + "' and store<>'R' ";
                                fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                            }
                        }

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

                        frm_vnum = txtvchnum.Text.Trim();
                        save_it = "Y";



                        if (frm_vnum == "000000") btnhideF_Click(sender, e);
                        if (edmode.Value == "Y")
                        {
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||to_char(" + doc_df.Value + ",'yyyymmdd')||trim(" + doc_nf.Value + ")='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "' and store='R'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }

                        save_fun();


                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);


                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully'13'Do you want to see the Print Preview ?");

                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully'13'Do you want to see the Print Preview ?");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Data Not Saved");
                            }
                        }


                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_mbr + txtlbl4.Text.Trim() + Convert.ToDateTime(txtvchdate.Text.Trim()).ToString("yyyyMMdd") + txtvchnum.Text + "'");
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
            //for (int sg1r = 0; sg1r < sg1.Rows.Count; sg1r++)
            //{
            //    for (int j = 0; j < sg1.Columns.Count; j++)
            //    {
            //        sg1.Rows[sg1r].Cells[j].ToolTip = sg1.Rows[sg1r].Cells[j].Text;
            //        if (sg1.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
            //        {
            //            sg1.Rows[sg1r].Cells[j].Text = sg1.Rows[sg1r].Cells[j].Text.Substring(0, 35);
            //        }
            //    }
            //}

            //sg1.HeaderRow.Cells[10].Style.Add("style", "none");
            //e.Row.Cells[10].Style.Add("style", "none");
            //sg1.HeaderRow.Cells[11].Style.Add("style", "none");
            //e.Row.Cells[11].Style.Add("style", "none");
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
                //if (index < sg1.Rows.Count - 1)
                //{
                //    hf1.Value = index.ToString();
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                //    //----------------------------
                //    hffield.Value = "SG1_RMV";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                //    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Remove This Item From The List");
                //}
                break;


            case "SG1_ROW_ADD":

                //if (index < sg1.Rows.Count - 1)
                //{
                //    hf1.Value = index.ToString();
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                //    //----------------------------
                //    hffield.Value = "SG1_ROW_ADD_E";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                //    make_qry_4_popup();
                //    fgen.Fn_open_sseek("Select Item", frm_qstr);
                //}
                //else
                //{
                //    hffield.Value = "SG1_ROW_ADD";
                //    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                //    make_qry_4_popup();
                //    fgen.Fn_open_mseek("Select Item", frm_qstr);
                //    //fgen.Fn_open_mseek("Select Item", frm_qstr);
                //}
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
                //dt = new DataTable();
                //sg2_dt = new DataTable();
                //dt = (DataTable)ViewState["sg2"];
                //z = dt.Rows.Count - 1;
                //sg2_dt = dt.Clone();
                //sg2_dr = null;
                //i = 0;
                //for (i = 0; i < sg2.Rows.Count; i++)
                //{
                //    sg2_dr = sg2_dt.NewRow();
                //    sg2_dr["sg2_srno"] = (i + 1);
                //    sg2_dr["sg2_t1"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t1")).Text.Trim();
                //    sg2_dr["sg2_t2"] = ((TextBox)sg2.Rows[i].FindControl("sg2_t2")).Text.Trim();
                //    sg2_dt.Rows.Add(sg2_dr);
                //}
                //sg2_add_blankrows();
                //ViewState["sg2"] = sg2_dt;
                //sg2.DataSource = sg2_dt;
                //sg2.DataBind();
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
        fgen.Fn_open_sseek("Select MRR Details / Supplier ", frm_qstr);
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
        string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        curr_dt = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        //if (frm_vnum != "000000")
        //{
        //    cmd_query = "update ivoucher set pname='" + frm_uname + "',tc_no='" + txtvchnum.Text + "',qc_date=sysdate,qcdate=to_datE(sysdate,'dd/mm/yyyy'),ACTUAL_INSP='Y',store='Y',inspected='Y',desc_=DECODE(Trim(desc_),'-','',Trim(desc_))||'QA.No.'||'" + txtvchnum.Text + "',IQTYIN=" + (fgen.make_double(txtlbl11.Text) - fgen.make_double(txtlbl13.Text)) + ",ACPT_UD =" + (fgen.make_double(txtlbl11.Text) - fgen.make_double(txtlbl13.Text)) + ",REJ_RW=" + fgen.make_double(txtlbl13.Text) + ",IEXC_aDDL =" + fgen.make_double(txtlbl14.Text) + " where branchcd='" + frm_mbr + "' and type like '04%' and vchnum ='" + txtvchnum.Text + "' and vchdate=to_Date('" + txtvchdate.Text + "','dd/mm/yyyy') and acode ='" + txtlbl7.Text + "' and store<>'R'";
        //    fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
        //    if (txtlbl4.Text == "04")
        //    {
        //        cmd_query = "delete from ivoucher where branchcd='" + frm_mbr + "' and type like '04%' and vchnum ='" + txtvchnum.Text + "' and vchdate=to_Date('" + txtvchdate.Text + "','dd/mm/yyyy') and acode ='" + txtlbl7.Text + "' and store='R'";
        //        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

        //        cmd_query = "insert into ivoucher(vcode,iamount,btchno,btchdt,tc_no,pname,inspected,actual_insp,qcdate,qc_date,srno,branchcd,type,vchnum,vchdate,acode,icode,store,iqty_chl,iqtyin,iqtyout,acpt_ud,rej_rw,ponum,podate,rgpnum,rgpdate,invno,invdate,genum,gedate,rec_iss,ent_by,ent_dt,edt_by,edt_dt)(select acode,rej_rw*irate,btchno,btchdt,'" + txtvchnum.Text + "',pname,inspected,actual_insp,qcdate,qc_date,srno,branchcd,type,vchnum,vchdate,acode,icode,'R',iqty_chl," + fgen.make_double(txtlbl13.Text) + " as iqtyin,0 as iqtyout,0 as acpt_ud,0 as rej_rw,ponum,podate,rgpnum,rgpdate,invno,invdate,genum,gedate,rec_iss,ent_by,ent_dt,edt_by,edt_dt from ivoucher where branchcd='" + frm_mbr + "' and type like '04%' and vchnum ='" + txtvchnum.Text + "' and vchdate=to_Date('" + txtvchdate.Text + "','dd/mm/yyyy') and store<>'R' and acode ='" + txtlbl7.Text + "' )";
        //        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

        //    }

        //}

        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text) > 0)
            {


                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = txtlbl4.Text;
                oporow["vchnum"] = txtvchnum.Text.Trim();
                oporow["vchdate"] = txtvchdate.Text.Trim();

                oporow["SRNO"] = i;
                oporow["MORDER"] = i;
                oporow["store"] = "R";
                oporow["inspected"] = "Y";
                oporow["actual_insp"] = "Y";
                oporow["pname"] = frm_uname;

                oporow["invno"] = txtlbl2.Text; ;
                oporow["invdate"] = txtlbl3.Text; ;
                oporow["refnum"] = txtlbl5.Text; ;
                oporow["refdate"] = txtlbl6.Text; ;

                oporow["acode"] = txtlbl7.Text;
                oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim();
                oporow["iqty_chl"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
                oporow["rej_rw"] = 0;


                oporow["qc_Date"] = curr_dt;
                oporow["qcdate"] = curr_dt;

                //following are not saving.
                ////sg1_dr["sg1_h1"] = dt.Rows[i]["ponum"].ToString().Trim();
                ////sg1_dr["sg1_h2"] = dt.Rows[i]["podate"].ToString().Trim();

                ////sg1_dr["sg1_h3"] = dt.Rows[i]["rgpnum"].ToString().Trim();
                ////sg1_dr["sg1_h4"] = dt.Rows[i]["rgpdate"].ToString().Trim();

                ////sg1_dr["sg1_h5"] = dt.Rows[i]["irate"].ToString().Trim();
                ////sg1_dr["sg1_h6"] = dt.Rows[i]["potype"].ToString().Trim();

                ////sg1_dr["sg1_h7"] = dt.Rows[i]["genum"].ToString().Trim();
                ////sg1_dr["sg1_h8"] = dt.Rows[i]["gedate"].ToString().Trim();
 
                
                oporow["ponum"] = sg1.Rows[i].Cells[0].Text.Trim();
                oporow["podate"] = fgen.make_def_Date(sg1.Rows[i].Cells[1].Text.Trim(), vardate);

                oporow["rgpnum"] = sg1.Rows[i].Cells[2].Text.Trim();
                oporow["rgpdate"] = fgen.make_def_Date(sg1.Rows[i].Cells[3].Text.Trim(), vardate);

                oporow["irate"] = fgen.make_double(sg1.Rows[i].Cells[4].Text.Trim());
                oporow["potype"] = sg1.Rows[i].Cells[5].Text.Trim();

                oporow["genum"] = fgen.make_double(sg1.Rows[i].Cells[6].Text.Trim());
                oporow["gedate"] = fgen.make_def_Date(sg1.Rows[i].Cells[7].Text.Trim(), vardate);

                oporow["rec_iss"] = "D";
                oporow["iqtyin"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text);
                oporow["idiamtr"] = 0;
                oporow["iweight"] = 0;
                oporow["iamount"] = 0;
                oporow["iqtyout"] = 0;
                oporow["iqty_ok"] = 0;
                oporow["acpt_ud"] = 0;
                oporow["rej_sdp"] = 0;
                oporow["rej_Sdv"] = 0;
                oporow["desc_"] = "-";
                oporow["iqty_wt"] = 0;



                oporow["purpose"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text;
                oporow["btchno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text;
                oporow["btchdt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text;
                oporow["tc_no"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t10")).Text;

                oporow["naration"] = txtrmk.Text.Trim();

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

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "10");
        if (frm_formID == "F30144")
        {
            SQuery = "select type1 as fstr, name,type1 from type where id='M' and substr(type1,1,2)>'14' and substr(type1,1,1)<'19' order by type1";
        }
    }
    //------------------------------------------------------------------------------------   
    protected void txtBarCode_TextChanged(object sender, EventArgs e)
    {
        string barcode = "";
        barcode = txtBarCode.Text;
        if (barcode.Length > 18) barcode = barcode.Substring(0, 18);
        SQuery = "SELECT distinct a.branchcd||a.type||to_char(A.vchdate,'yyyymmdd')||a.vchnum AS FSTR,trim(a.Vchnum) as MRR_No,to_char(a.vchdate,'dd/mm/yyyy') as MRR_Dt,b.Aname as Supplier,a.Invno,A.Refnum as chl_no,a.type,a.Ent_by,to_char(a.vchdate,'yyyymmdd') As vdd,a.inspected from ivoucher a ,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.branchcd||a.type||a.vchnum||to_char(A.vchdate,'yyyymmdd')='" + barcode + "'";
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["inspected"].ToString().Trim() != "Y")
            {
                hffield.Value = "TACODE";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", dt.Rows[0]["fstr"].ToString().Trim());
                btnhideF_Click("", EventArgs.Empty);
            }
            else fgen.msg("Already QC Passed", "AMSG", "Material has been already quality passed !! Please scan other Material Receipt");
        }
        else fgen.msg("Wrong Barcode!!", "AMSG", "Material Receipt Note not found !! Please scan other barcode");
        txtBarCode.Text = "";
    }
}