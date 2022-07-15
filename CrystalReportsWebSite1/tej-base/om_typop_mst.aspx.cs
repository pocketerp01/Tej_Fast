using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_typop_mst : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;

    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;
    DataTable sg4_dt; DataRow sg4_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string html_body = "";
    string Prg_Id, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_fchar, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
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
                    CSR = fgenMV.Fn_Get_Mvar(frm_qstr, "C_S_R");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                }
                else Response.Redirect("~/login.aspx");
            }



            if (!Page.IsPostBack)
            {
                doc_addl.Value = "1";
                fgen.execute_cmd(frm_qstr, frm_cocd, "update type set tvchnum=lpad(Trim(type1),6,'0') where trim(nvl(tvchnum,'-'))='-'");
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();

            if (frm_ulvl != "0") btndel.Visible = false;

            //btnprint.Visible = false;
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
                //if (fgen.make_double(mcol_width) > 0)
                //{
                //    sg1.Columns[sR].HeaderStyle.Width = Convert.ToInt32(mcol_width);
                //    sg1.Rows[0].Cells[sR].Width = Convert.ToInt32(mcol_width);
                //}
            }
        }

        //txtlbl2.Attributes.Add("readonly", "readonly");
        //txtlbl3.Attributes.Add("readonly", "readonly");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");



        //txtlbl6.Attributes.Add("readonly", "readonly");

        //my_Tabs
        //txtlbl2.Attributes["required"] = "true";
        //txtlbl2.BackColor = System.Drawing.ColorTranslator.FromHtml("#E0FF00");
        // to hide and show to tab panel

        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;
        tab7.Visible = false;
        if (frm_formID == "F70176")
        {
            //
            tab8.Visible = true;
        }
        else
        {
            tab7.Visible = false;
            tab8.Visible = false;
        }


        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {

            case "M12008":
                tab3.Visible = false;
                tab4.Visible = false;
                break;
            case "F60161":
                //AllTabs.Visible = false;
                break;
        }
        tab1.Visible = true;
        tab2.Visible = false;
        tab3.Visible = false;
        tab4.Visible = false;
        tab5.Visible = false;
        tab6.Visible = false;
        tab7.Visible = false;
        if (frm_formID == "F70176")
        {
            //tab7.Visible = true;
            tab8.Visible = true;
        }
        else
        {
            tab7.Visible = false;
            tab8.Visible = false;
        }

        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false;
        btnprint.Disabled = false;
        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();

        sg1_add_blankrows();
        sg2_add_blankrows();
        sg3_add_blankrows();
        sg4_add_blankrows();



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
        btnnew.Disabled = true;
        btnedit.Disabled = true;
        btnsave.Disabled = false;
        btnlist.Disabled = true;
        btnprint.Disabled = true;
        btndel.Disabled = true;
        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;
        btnexit.Visible = false;
        btncancel.Visible = true;

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
        string tbl_id;
        tbl_id = "";
        string tbl_typ;
        tbl_typ = "%";
        string tbl_cond;
        tbl_cond = "1=1";

        typePopup = "N";

        doc_nf.Value = "TVCHNUM";
        doc_df.Value = "TVCHDATE";
        frm_tabname = "TYPE";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        d3.Visible = false;
        d4.Visible = false;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        BTN_Itclass.Visible = false;
        switch (Prg_Id)
        {

            case "F10101":
                tbl_id = "Y";
                lblheader.Text = "Item Group Master";
                BTN_Itclass.Visible = true ;
                txtlbl2.MaxLength = 40;
                break;
            case "F10121":
                tbl_id = "U";
                lblheader.Text = "Units Master";
                break;
            case "F10126":
                tbl_id = "K";
                lblheader.Text = "Processes Master";
                d3.Visible = true;
                d4.Visible = true;
                break;
            case "F70173":
                tbl_id = "Z";
                lblheader.Text = "Account Groups Master";
                break;
            case "F70176":
                tbl_id = "V";
                lblheader.Text = "Voucher Types Master";
                break;
            case "F70192":
                tbl_id = "{";
                lblheader.Text = "States Master";
                btnPersonName.Visible = false;
                ImageButton1.Visible = false;

                break;
            case "F70195":
                tbl_id = "N";
                //tbl_typ = "1";
                lblheader.Text = "Standard Naration Master";
                ImageButton1.Visible = false;
                
                break;
            case "F75162":
                tbl_id = ":";
                tbl_typ = "1";
                lblheader.Text = "Machinery Groups";
                break;
            case "F25201":
                tbl_id = "M";
                tbl_typ = "0";
                lblheader.Text = "Inward Type Master";
                break;
            case "F99154":
                tbl_id = "M";
                tbl_typ = "1";
                tbl_cond = "substr(a.type1,1,2)>'14'";
                lblheader.Text = "Production Type Master";
                break;

            case "F25203":
                tbl_id = "M";
                tbl_typ = "2";
                lblheader.Text = "Outward Type Master";
                break;
            case "F25205":
                tbl_id = "M";
                tbl_typ = "3";
                lblheader.Text = "Issue Type Master";
                break;
            case "FB3058":
                tbl_id = "1";
                tbl_typ = "6";
                lblheader.Text = "WIP Stage Master";
                break;
            case "F25207":
                tbl_id = "M";
                tbl_typ = "1";
                lblheader.Text = "Return Type Master";
                break;
            case "F25209":
                tbl_id = "M";
                tbl_typ = "6";
                lblheader.Text = "Department Master";
                break;

            case "F15201":
                tbl_id = "M";
                tbl_typ = "5";
                lblheader.Text = "Purch Order Type Master";
                break;

            case "F15203":
                tbl_id = "A";
                lblheader.Text = "Currency Type Master";
                break;
            case "F15205":
                tbl_id = "G";
                tbl_typ = "1";
                lblheader.Text = "Price Basis Master";
                break;
            case "F15207":
                tbl_id = "H";
                tbl_typ = "0";
                lblheader.Text = "Insurance Terms Master";
                break;
            case "F15209":
                tbl_id = "H";
                tbl_typ = "1";
                lblheader.Text = "Freight Terms Master";
                break;
            case "F50201":
                tbl_id = "V";
                tbl_typ = "4";
                lblheader.Text = "Sales Type Master";
                break;
            case "F47163":
            case "F50203":
                tbl_id = "A";
                lblheader.Text = "Currency Type Master";
                break;
            case "F47164":
            case "F50205":
                tbl_id = "<";
                tbl_typ = "0";
                lblheader.Text = "Contract Terms Master";
                break;
            case "F47165":
            case "F50207":
                tbl_id = "G";
                tbl_typ = "5";
                lblheader.Text = "Payment Terms Master";
                break;
            case "F50209":
                tbl_id = "G";
                tbl_typ = "2";
                lblheader.Text = "Mode of Transport Master";
                break;
            case "F99153":
                tbl_id = "D";
                tbl_typ = "1";
                lblheader.Text = "Shifts Master";
                break;

            case "F85235":
                tbl_id = "I";
                // tbl_typ = "SUBSTR(TRIM(TYPE1),1,1)<'2'";
                lblheader.Text = "Employee Grade Master";
                lbl4.Visible = false;
                ImageButton1.Visible = false;
                txtlbl4.Visible = false;
                LBL_Itclass.Visible = false; 
                Label4.Visible = false;
                TXT_Itclass.Visible = false;
                btnprint.Visible = false;
                break;
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", tbl_id);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FCHAR", tbl_typ);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_FTY_COND", tbl_cond);

        dt = new DataTable();
        string cmd_qry;


        cmd_qry = "Select a.ID,a.Type1 as Type_Code,a.Name as Type_Name,a.ment_by as Entry_by,to_char(a.ment_dt,'dd/mm/yyyy') as Entry_Dt,a.mEDT_BY as Edit_by,a.mEDT_DT as Edit_Dt,a.Acode as Actg_Code,b.aname as Actg_Name from " + frm_tabname + " a left outer join famst b on trim(A.acode)=trim(B.acode)  where a.TBRANCHCD='00' AND a.id ='" + tbl_id + "' AND a.type1 like '" + tbl_typ + "%' order by a.type1 ";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F10101":
                cmd_qry = "Select a.ID,a.Type1 as Type_Code,a.Name as Type_Name,a.ment_by as Entry_by,to_char(a.ment_dt,'dd/mm/yyyy') as Entry_Dt,a.mEDT_BY as Edit_by,a.mEDT_DT as Edit_Dt,a.Acode as Actg_Code,b.aname as Actg_Name,a.Stform as Item_Class from " + frm_tabname + " a left outer join famst b on trim(A.acode)=trim(B.acode)  where a.TBRANCHCD='00' AND a.id ='" + tbl_id + "' AND a.type1 like '" + tbl_typ + "%' order by a.type1 ";
                break;
            case "F70173":
                cmd_qry = "Select a.ID,a.Type1 as Type_Code,a.Name as Type_Name,a.ment_by as Entry_by,to_char(a.ment_dt,'dd/mm/yyyy') as Entry_Dt,a.mEDT_BY as Edit_by,a.mEDT_DT as Edit_Dt,a.Acode as Actg_Code,b.aname as Actg_Name from " + frm_tabname + " a left outer join famst b on trim(A.acode)=trim(B.acode)  where a.TBRANCHCD='00' AND a.id ='" + tbl_id + "' AND a.type1 like '" + tbl_typ + "%' order by a.type1 ";
                break;

            case "F99153":
                cmd_qry = "Select a.ID,a.Type1 as Type_Code,a.Name as Type_Name,a.ment_by as Entry_by,to_char(a.ment_dt,'dd/mm/yyyy') as Entry_Dt,a.mEDT_BY as Edit_by,a.mEDT_DT as Edit_Dt,a.Acode as Actg_Code from " + frm_tabname + " a where a.TBRANCHCD='00' AND a.id ='" + tbl_id + "' AND a.type1 like '" + tbl_typ + "%' order by a.type1 ";
                break;
            case "F99154":
                cmd_qry = "Select a.ID,a.Type1 as Type_Code,a.Name as Type_Name,a.ment_by as Entry_by,to_char(a.ment_dt,'dd/mm/yyyy') as Entry_Dt,a.mEDT_BY as Edit_by,a.mEDT_DT as Edit_Dt,a.Acode as Actg_Code from " + frm_tabname + " a where a.TBRANCHCD='00' AND a.id ='" + tbl_id + "' AND a.type1 like '" + tbl_typ + "%' and " + tbl_cond + " order by a.type1 ";
                break;

            case "F85235":
                cmd_qry = "Select a.ID,a.Type1 as Type_Code,a.Name as Type_Name,a.ment_by as Entry_by,to_char(a.ment_dt,'dd/mm/yyyy') as Entry_Dt,a.mEDT_BY as Edit_by from " + frm_tabname + " a where a.TBRANCHCD='00' AND a.id ='" + tbl_id + "' AND SUBSTR(TRIM(TYPE1),1,1)<'2' order by a.type1 ";
                break;
        }

        dt = fgen.getdata(frm_qstr, frm_cocd, cmd_qry);

        sg5.DataSource = dt;
        sg5.DataBind();

        sg4.DataSource = null;
        sg4.DataBind();
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        string frm_tcond = "";
        string comb_char;
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");

        comb_char = frm_vty;

        if (frm_fchar != "%")
        {
            comb_char = frm_vty + frm_fchar;
        }
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        if (CSR.Length > 1) cond = " and trim(a.ccode)='" + cond + "'";
        switch (btnval)
        {
            case "ITM_CLASS":
                SQuery = "SELECT trim(type1)||':'||trim(name) AS FSTR,Name AS NAME,Type1 AS CODE FROM typegrp where branchcd!='DD' and id='YY' order by Type1";
                break;
            case "TYPECODE":

                SQuery = "select sum(Valu) as fstr,coded as Code_No_Available,max(name) as Code_Name,(case when sum(Valu)>0 then 'Code Available' else 'Code Already Used' end) as Code_Status from (select lpad(trim(to_char(rownum,'99')),2,'0') as coded,1 as Valu,null as name from (select rowid,rownum from FIN_MSYS order by id) where rownum<100 union all select type1,-1 as coded,name from type where id='" + frm_vty + "') group by coded  order by Coded";
                if (comb_char.Length > 1)
                {
                    SQuery = "select * from (select sum(Valu) as fstr,coded as Code_No_Available,max(name) as Code_Name,(case when sum(Valu)>0 then 'Code Available' else 'Code Already Used' end) as Code_Status from (select lpad(trim(to_char(rownum,'99')),2,'0') as coded,1 as Valu,null as name from (select rowid,rownum from FIN_MSYS order by id) where rownum<100 union all select type1,-1 as coded,name from type where id='" + frm_vty + "' and type1 like '" + frm_fchar + "%') group by coded ) where Code_No_Available like '" + frm_fchar + "%' order by Code_No_Available ";
                }
                if (frm_formID == "F85235")
                {
                    SQuery = "select * from (select sum(Valu) as fstr,coded as Code_No_Available,max(name) as Code_Name,(case when sum(Valu)>0 then 'Code Available' else 'Code Already Used' end) as Code_Status from (select lpad(trim(to_char(rownum,'99')),2,'0') as coded,1 as Valu,null as name from (select rowid,rownum from FIN_MSYS order by id) where rownum<100 union all select type1,-1 as coded,name from type where id='" + frm_vty + "' and SUBSTR(TRIM(TYPE1),1,1)<'2') where substr(coded,1,1)<2 group by coded) order by Code_No_Available";
                }


                break;
            case "ACTGCODE":
                switch (comb_char)
                {
                    case "Y":
                        SQuery = "select trim(Acode) as fstr,Aname as Account_Name,Acode as Code,substr(acode,1,2) as grps from famst where length(trim(nvl(deac_by,'-'))) <2 and (substr(acode,1,1)>='2' or substr(acode,1,2)='10') order by grps,Aname";
                        break;
                    case "V4":
                        SQuery = "select trim(Acode) as fstr,Aname as Account_Name,Acode as Code,substr(acode,1,2) as grps from famst where length(trim(nvl(deac_by,'-'))) <2 and substr(acode,1,1) in ('2','3') order by grps,Aname";
                        break;
                    case "V1":
                    case "V2":
                        SQuery = "select trim(Acode) as fstr,Aname as Account_Name,Acode as Code,substr(acode,1,2) as grps from famst where length(trim(nvl(deac_by,'-'))) <2 and substr(acode,1,2) in ('03','12') order by grps,Aname";
                        break;
                    default:
                        SQuery = "select trim(Acode) as fstr,Aname as Account_Name,Acode as Code,substr(acode,1,2) as grps from famst where length(trim(nvl(deac_by,'-'))) <2 and (substr(acode,1,2) in ('03','12') or substr(acode,1,1) in ('2'))  order by grps,Aname";
                        break;
                }


                break;
            case "ACCODE":
                SQuery = "SELECT ACODE AS FSTR,ANAME AS NAME,ACODE AS CODE,ENT_BY,ENT_DT FROM FAMST WHERE SUBSTR(ACODE,1,2) IN('01','02','20','03','10','12','21') ORDER BY ACODE";
                break;
            case "USER":
                SQuery = "SELECT USERID AS FSTR,USERNAME AS NAME,USERID AS CODE,DEPTT FROM EVAS ORDER BY USERNAME";
                break;
            case "BTN_23":

                break;
            case "TACODE":
                //pop1
                Acode_Sel_query();
                break;
            case "TICODE":
                //pop1
                Icode_Sel_query();
                break;
            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":

                break;

            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":

                break;
            case "SG1_ROW_TAX":
                break;

            case "New":
                Type_Sel_query();
                break;
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")
                    frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");
                frm_tcond = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FTY_COND");

                SQuery = "select distinct a.type1 as fstr,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt,a.Name,a.type1 as Code,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where  a.tbranchcd='00' and trim(a.ID)='" + frm_vty + "' and trim(a.type1) like '" + frm_fchar + "%'  order by a.type1";

                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                switch (Prg_Id)
                {
                    case "F99154":
                        SQuery = "select distinct a.type1 as fstr,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt,a.Name,a.type1 as Code,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where  a.tbranchcd='00' and trim(a.ID)='" + frm_vty + "' and trim(a.type1) like '" + frm_fchar + "%' and " + frm_tcond + "  order by a.type1";
                        break;

                    case "F85235":
                        SQuery = "select distinct a.type1 as fstr,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt,a.Name,a.type1 as Code,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where  a.tbranchcd='00' and trim(a.ID)='" + frm_vty + "' and SUBSTR(TRIM(a.TYPE1),1,1)<'2'  order by a.type1";
                        break;
                    case "F60161":
                        //AllTabs.Visible = false;
                        break;
                }
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
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";
            txtlbl2.Focus();
            if (typePopup == "N") newCase(frm_vty);
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }

            if (CSR.Length > 1)
            {
                //txtlbl4.Value = CSR;
                //txtlbl4.Disabled = true;
            }
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");

        if (frm_mbr != "00")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please operate this Option from Plant Code 00 !!");
            return;
        }

    }
    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");
        if (frm_formID == "F85235")
        {
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE tBRANCHCD='00' AND ID='" + frm_vty + "' AND SUBSTR(TRIM(TYPE1),1,1)<'2'", 6, "VCH");
        }
        else
        {
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE tBRANCHCD='00' AND ID='" + frm_vty + "' AND type1 like '" + frm_fchar + "%' ", 6, "VCH");
        }
        txtvchnum.Value = frm_vnum;
        txtvchdate.Value = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        //txtlbl2.Text = frm_uname;
        //txtlbl3.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        if (frm_formID.Trim() == "F70192")
        {
            txtlbl3.Value = frm_vnum.Substring(frm_vnum.Trim().Length - 2, 2);
        }
        disablectrl();
        fgen.EnableForm(this.Controls);


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

        //--------------------------------
        ////sg4_dt = new DataTable();
        ////create_tab4();
        ////sg4_add_blankrows();
        ////sg4_add_blankrows();
        ////sg4_add_blankrows();
        ////sg4_add_blankrows();
        ////sg4_add_blankrows();
        ////sg4.DataSource = sg4_dt;
        ////sg4.DataBind();
        ////setColHeadings();
        ////ViewState["sg4"] = sg4_dt;        
        #endregion
    }
    //------------------------------------------------------------------------------------
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        if (frm_mbr != "00")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please operate this Option from Plant Code 00 !!");
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
        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }
        if (frm_mbr != "00")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Please operate this Option from Plant Code 00 !!");
            return;
        }

        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Value.ToString());
        if (dhd == 0) { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;

        if (txtlbl2.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / Master Name ";
        }

        if (txtlbl3.Value.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / Master Code ";
        }

        if (txtlbl4.Value.Trim().Length < 2 && frm_vty == "V")
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / Account Code ";
        }

        if (TXT_Itclass.Value.Trim().Length < 2 && frm_vty == "Y")
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / Item Classification ";
        }

        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }
        string chk_exist = "";
        chk_exist = fgen.seek_iname(frm_qstr, frm_cocd, "select type1||'-'||name as fstr from type where id='" + frm_vty + "' and type1!='" + txtlbl3.Value.Trim() + "' and upper(trim(name))='" + txtlbl2.Value.Trim().ToUpper() + "'", "fstr");
        if (chk_exist.ToString().Length > 5)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " Name already Open , See '13' " + chk_exist + " '13' Please Re Check " + reqd_flds);
            return;
        }

        fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
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

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");

        PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        SQuery = "select distinct a.type1 as Type_Code,a.Name,a.Acode,a.type1 as Code,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt from " + frm_tabname + " a where  a.tbranchcd='00' and trim(a.ID)='" + frm_vty + "' and trim(a.type1) like '" + frm_fchar + "%'  order by a.type1";
        switch (Prg_Id)
        {
            case "F10101":
                SQuery = "select a.Name as Item_Grp_Name,a.type1 as Item_Grp_Code,a.Stform as Item_class,b.aname as Acctg_Name,a.Acode as Acctg_Code,a.MEnt_by as Ent_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,a.MEdt_by as Edit_by,to_char(a.Medt_Dt,'dd/mm/yyyy') as Edt_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt from " + frm_tabname + " a left outer join famst b on trim(A.acode)=trim(B.acode) where a.tbranchcd='00' and trim(a.ID)='" + frm_vty + "' and trim(a.type1) like '" + frm_fchar + "%'  order by a.type1";
                break;
            case "F70173":
                SQuery = "select b.Name as Nature_of_Account,a.Name as Actg_Grp_Name,a.type1 as Type_Code,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,a.MEdt_by,to_char(a.Medt_Dt,'dd/mm/yyyy') as Edt_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd  from " + frm_tabname + " a left outer join (select type1,name from type where id='#') b on substr(trim(A.type1),1,1)=trim(B.type1) where a.tbranchcd='00' and trim(a.ID)='" + frm_vty + "' and trim(a.type1) like '" + frm_fchar + "%'  order by a.type1";
                break;
            case "F70176":
                SQuery = "select a.Name as Actg_Grp_Name,a.type1 as Actg_Grp_Code,a.Acode as Acctg_Code,a.type1 as Type_Code,b.aname as Acctg_Name,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,a.MEdt_by,to_char(a.Medt_Dt,'dd/mm/yyyy') as Edt_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt from " + frm_tabname + " a left outer join famst b on trim(A.acode)=trim(B.acode) where a.tbranchcd='00' and trim(a.ID)='" + frm_vty + "' and trim(a.type1) like '" + frm_fchar + "%'  order by a.type1";
                break;
            default:
                SQuery = "select a.Name as Type_Name,a.type1 as Type_Code,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,a.MEdt_by,to_char(a.Medt_Dt,'dd/mm/yyyy') as Edt_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt from " + frm_tabname + " a where a.tbranchcd='00' and trim(a.ID)='" + frm_vty + "' and trim(a.type1) like '" + frm_fchar + "%'  order by a.type1";
                break;
        }

        if (Prg_Id == "F70173")
        {
            fgen.drillQuery(0, SQuery, frm_qstr, "1#", "3#4#5#6#", "250#350#100#100#");
        }
        else
        {
            fgen.drillQuery(0, SQuery, frm_qstr, "1#", "3#4#5#6#", "350#100#100#100#");
        }
        
        fgen.Fn_DrillReport("List of " + lblheader.Text.Trim(), frm_qstr);
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        //fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
        hffield.Value = "-";


    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F10101");
        string header_n = "Item Group List";

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");


        if (frm_vty == "Y")
        {
            SQuery = "SELECT '" + header_n + "' as header, A.TYPE1 AS FSTR,A.TYPE1 AS CODE,A.NAME as mgname,A.ACODE AS LINK_aC ,B.ANAME AS LINK_AC_NAME  FROM TYPE A LEFT OUTER JOIN FAMST B ON TRIM(A.ACODE)=TRIM(B.ACODE) WHERE   A. ID='Y' ORDER BY A.TYPE1";
        }
        else
        {
            SQuery = "SELECT '" + header_n + "' as header, A.TYPE1 AS FSTR,A.TYPE1 AS CODE,A.NAME as mgname,'-' AS LINK_aC ,'-' AS LINK_AC_NAME  FROM TYPE A  WHERE trim(a.ID)='" + frm_vty + "' and trim(a.type1) like '" + frm_fchar + "%' ORDER BY A.TYPE1";

        }

        fgen.Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "MainGrpMaster", "MainGrpMaster");
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

        frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {
            string mqry = "";
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (frm_vty == "Y")
            {
                mqry = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as fstr from item where substr(icode,1,2)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'", "fstr");
                if (fgen.make_double(mqry.ToString()) > 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", " + mqry + " Items Opened under this Group , Deletion not Permitted !!");
                    return;
                }

            }
            if (frm_vty == "Z")
            {
                mqry = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as fstr from famst where substr(acode,1,2)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'", "fstr");
                if (fgen.make_double(mqry.ToString()) > 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", " + mqry + " Accounts Opened under this Group , Deletion not Permitted !!");
                    return;
                }

            }
            if (frm_vty == "U")
            {
                mqry = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as fstr from item where trim(upper(unit))='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "'", "fstr");
                if (fgen.make_double(mqry.ToString()) > 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", " + mqry + " Items Opened under this Unit , Deletion not Permitted !!");
                    return;
                }

            }
            if (frm_vty == "K")
            {
                mqry = fgen.seek_iname(frm_qstr, frm_cocd, "select count(*) as fstr from itwstage where trim(upper(stagec))='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4") + "'", "fstr");
                if (fgen.make_double(mqry.ToString()) > 0)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", " + mqry + " Stage Routing With this Unit , Deletion not Permitted !!");
                    return;
                }

            }

            if (col1 == "Y")
            {
                // Deleing data from Main Table

                mqry = "delete from " + frm_tabname + " a where trim(a.ID)||trim(a.type1)='" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, mqry);
                // Deleing data from Sr Ctrl Table
                mqry = "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'";
                fgen.execute_cmd(frm_qstr, frm_cocd, mqry);

                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");
                clearctrl(); fgen.ResetForm(this.Controls);
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
                case "Del":
                    if (col1 == "") return;
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);

                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;
                case "Edit":
                    if (col1 == "") return;
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);

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
                    //fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Print_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Print", frm_qstr);
                    break;
                case "Edit_E":
                    //edit_Click
                    #region Edit Start
                    if (col1 == "" || col1 == "-") return;
                    clearctrl();
                    frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                    frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");
                    string mv_col;
                    mv_col = frm_mbr + frm_vty + col1;
                    SQuery = "Select a.*,to_Char(a.ent_Dt,'dd/mm/yyyy') As ment_date from " + frm_tabname + " a where a.tbranchcd||trim(a.ID)||trim(a.type1)='" + mv_col + "' ";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "fstr", col1);
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();

                        txtvchnum.Value = dt.Rows[0]["" + doc_nf.Value + ""].ToString().Trim();
                        txtvchdate.Value = Convert.ToDateTime(dt.Rows[0]["" + doc_df.Value + ""].ToString().Trim()).ToString("dd/MM/yyyy");

                        txtlbl2.Value = dt.Rows[i]["NAME"].ToString().Trim();
                        txtlbl3.Value = dt.Rows[i]["type1"].ToString().Trim();

                        txtlbl4.Value = dt.Rows[i]["acode"].ToString().Trim();
                        TXT_Itclass.Value = dt.Rows[i]["stform"].ToString().Trim();

                        if (frm_formID == "F10126")
                        {
                            //rate = make ready time 
                            //excrate = prd.time
                            //exc_tarrif = m/c code
                            //rcnum = main grp
                            //balop = sheet_ctn

                            Text1.Value = dt.Rows[i]["addr2"].ToString().Trim();
                            Text2.Value = dt.Rows[i]["rate"].ToString().Trim();
                            Text3.Value = dt.Rows[i]["excrate"].ToString().Trim();
                            Text4.Value = dt.Rows[i]["exc_tarrif"].ToString().Trim();
                            Text5.Value = dt.Rows[i]["rcnum"].ToString().Trim();
                            Text6.Value = dt.Rows[i]["balop"].ToString().Trim();
                        }

                        if (frm_formID == "F70176")
                        {
                            // TxtCode.Value = dt.Rows[i]["TYPE1"].ToString().ToUpper().Trim();
                            //TxtName.Value = dt.Rows[i]["NAME"].ToString().ToUpper().Trim();
                            //txtact.Value = dt.Rows[i]["ACODE"].ToString().ToUpper().Trim(); 
                            txtchk.Value = dt.Rows[i]["RCNUM"].ToString().ToUpper().Trim();
                            txtUser.Value = dt.Rows[i]["ADDR"].ToString().ToUpper().Trim();
                            txtUser.Value = dt.Rows[i]["ADDR1"].ToString().ToUpper().Trim();
                            Text7.Value = dt.Rows[i]["ADDR2"].ToString().ToUpper().Trim();
                            txtchqno.Value = dt.Rows[i]["place"].ToString().ToUpper().Trim();
                            txtunit.Value = dt.Rows[i]["EXC_DIV"].ToString().ToUpper().Trim();
                            txtGstSub.Value = dt.Rows[i]["EXC_ITEM"].ToString().ToUpper().Trim();
                            txtacpaytop.Value = dt.Rows[i]["ZIPCODE"].ToString().ToUpper().Trim();
                            txtleftac.Value = dt.Rows[i]["EMAIL"].ToString().ToUpper().Trim();
                            TxtDatetop.Value = dt.Rows[i]["WEBSITE"].ToString().ToUpper().Trim();
                            TxtLeftDt.Value = dt.Rows[i]["TCS_NUM"].ToString().ToUpper().Trim(); ;
                            TxtPrtyNme.Value = dt.Rows[i]["VAT_FORM"].ToString().ToUpper().Trim();
                            TxtPrtyNamelft.Value = dt.Rows[i]["CSTNO"].ToString().ToUpper().Trim();
                            TxtAmttop.Value = dt.Rows[i]["NOTIFICATION"].ToString().ToUpper().Trim();
                            TxtAmtLft.Value = dt.Rows[i]["RADDR"].ToString().ToUpper().Trim();
                            TxtAmtFigtop.Value = dt.Rows[i]["RADDR1"].ToString().ToUpper().Trim();
                            TxtAmtfglft.Value = dt.Rows[i]["RPHONE"].ToString().ToUpper().Trim();
                            TxtFrmName.Value = dt.Rows[i]["HADDR"].ToString().ToUpper().Trim();
                            TxtFrmLft.Value = dt.Rows[i]["HADDR1"].ToString().ToUpper().Trim();
                            TxtAuthSign.Value = dt.Rows[i]["HPHONE"].ToString().ToUpper().Trim();
                            TxtAuthSiglft.Value = dt.Rows[i]["BANKNAME"].ToString().ToUpper().Trim();
                            TxtAcNo.Value = dt.Rows[i]["BANKADDR"].ToString().ToUpper().Trim();
                            TxtAcNoLft.Value = dt.Rows[i]["BANKAC"].ToString().ToUpper().Trim();
                            TxtCap.Value = dt.Rows[i]["bond_ut"].ToString().ToUpper().Trim();
                        }

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;


                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        sg1_dt.Dispose();
                        //------------------------

                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";

                        txtvchnum.Disabled = true;
                        txtvchdate.Disabled = true;


                    }
                    #endregion
                    break;
                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.tBRANCHCD||trim(A.ID)||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
                    break;
                case "ITM_CLASS":
                    TXT_Itclass.Value = col1;
                    break;

                case "TYPECODE":
                    //if (fgen.make_double(col1) <= 0)
                    //{
                    //    fgen.msg("-", "AMSG", "Please Choose Code Which is Available !!");
                    //    return;
                    //}
                    //else
                    {
                        txtlbl3.Value = col2;
                    }

                    break;
                case "ACTGCODE":
                    txtlbl4.Value = col1;
                    break;
                case "ACCODE":
                    txtact.Value = col1;
                    break;
                case "USER":
                    txtUser.Value = col1;
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    //txtlbl4.Text = col1;
                    //txtlbl4a.Text = col2;

                    //txtlbl5.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                    //txtlbl6.Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL7").ToString().Trim().Replace("&amp", "");

                    //btnlbl7.Focus();
                    break;


                case "TICODE":
                    if (col1.Length <= 0) return;
                    //txtlbl7.Text = col1;
                    //txtlbl7a.Text = col2;
                    //txtlbl2.Focus();
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
                            sg1_dr["sg1_t3"] = "0";
                            sg1_dr["sg1_t4"] = "0";
                            sg1_dr["sg1_t5"] = "0";
                            sg1_dr["sg1_t6"] = "0";
                            sg1_dr["sg1_t7"] = "0";
                            sg1_dr["sg1_t8"] = "0";
                            sg1_dr["sg1_t9"] = "0";
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
                    sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    #endregion
                    setColHeadings();
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
                    //((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
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

                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t12")).Focus();
                    break;
                case "SG1_ROW_DT":
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1").ToString().Trim().Replace("&amp", "");
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
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {

        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------

            i = 0;
            hffield.Value = "";



            #region Number Gen and Save to Table
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
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
                        frm_vnum = txtvchnum.Value.Trim();
                        save_it = "Y";
                    }

                    else
                    {
                        save_it = "Y";


                        if (save_it == "Y")
                        {

                            i = 0;


                            do
                            {
                                if (frm_formID == "F85235")
                                {
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where tbranchcd='" + frm_mbr + "' and ID='" + frm_vty + "' and SUBSTR(TRIM(TYPE1),1,1)<'2'", 6, "vch");
                                }
                                else
                                {
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where tbranchcd='" + frm_mbr + "' and ID='" + frm_vty + "' ", 6, "vch");
                                }
                                pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Value.Trim(), "", frm_uname);
                                if (i > 20)
                                {
                                    fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where tbranchcd='" + frm_mbr + "' and ID='" + frm_vty + "' ", 6, "vch");
                                    pk_error = "N";
                                    i = 0;
                                }
                                i++;
                            }
                            while (pk_error == "Y");
                        }
                    }


                    if (frm_vnum == "000000") btnhideF_Click(sender, e);

                    save_fun();

                    string ddl_fld1;
                    string ddl_fld2;
                    string cmd_qry;
                    ddl_fld1 = frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    ddl_fld2 = frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    if (edmode.Value == "Y")
                    {
                        cmd_qry = "update " + frm_tabname + " set tbranchcd='DD' where tbranchcd||trim(ID)||trim(type1)='" + ddl_fld1 + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_qry);
                    }
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                    if (edmode.Value == "Y")
                    {
                        fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");
                        cmd_qry = "delete from " + frm_tabname + " where tbranchcd||trim(ID)||trim(type1)='DD" + ddl_fld2 + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_qry);
                    }
                    else
                    {
                        if (save_it == "Y")
                        {
                            fgen.msg("-", "AMSG", "Data Saved");

                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Data Not Saved");
                        }
                    }

                    #region Email Sending Function
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    //html started                            
                    sb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
                    sb.Append("<h3>" + fgenCO.chk_co(frm_cocd) + "</h3>");
                    sb.Append("<br>Dear Sir/Mam,<br> This is to advise that the following " + lblheader.Text + " has been saved by " + frm_uname + ".<br><br>");

                    //table structure
                    sb.Append("<table border=1 cellspacing=1 cellpadding=1 style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; color: #474646'>");

                    sb.Append("<tr style='color: #FFFFFF; background-color: #0099FF; font-weight: 700; font-family: Arial, Helvetica, sans-serif'>" +
                    "<td><b>Master Code</b></td><td><b>Master Name</b></td><td><b>User Name</b></td><td><b>Activity Date</b></td><td><b>ID</b></td>");
                    //vipin
                    //foreach (GridViewRow gr in sg1.Rows)
                    //{
                    //    if (gr.Cells[13].Text.Trim().Length > 4)
                    //    {

                    sb.Append("<tr>");
                    sb.Append("<td>");
                    sb.Append(txtlbl3.Value.Trim());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(txtlbl2.Value.Trim());
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(frm_uname);
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(vardate);
                    sb.Append("</td>");
                    sb.Append("<td>");
                    sb.Append(Prg_Id);
                    sb.Append("</td>");
                    sb.Append("</tr>");
                    //    }
                    //}
                    sb.Append("</table></br>");

                    sb.Append("Thanks & Regards");
                    sb.Append("<h5>Note: This is an Auto generated Mail from Tejaxo ERP. The above details are to the best of information <br> and data available to the ERP System. For any discrepancy/ clarification kindly get in touch with the concerned official. </h5>");
                    sb.Append("</body></html>");

                    //send mail
                    string subj = "";
                    if (edmode.Value == "Y") subj = "Edited : ";
                    else subj = "New Entry : ";

                    fgen.send_Activity_mail(frm_qstr, frm_cocd, "Tejaxo ERP", frm_formID, subj + lblheader.Text + " #" + frm_vnum, sb.ToString(), frm_uname);

                    sb.Clear();
                    #endregion

                    fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + frm_vnum, frm_uname, edmode.Value);

                    set_Val();
                    fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                }
                catch (Exception ex)
                {


                    fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                    fgen.msg("-", "AMSG", ex.Message.ToString());
                    col1 = "N";
                }
            #endregion
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
        sg1_dr["sg1_t3"] = "0";
        sg1_dr["sg1_t4"] = "0";
        sg1_dr["sg1_t5"] = "0";
        sg1_dr["sg1_t6"] = "0";
        sg1_dr["sg1_t7"] = "0";
        sg1_dr["sg1_t8"] = "0";
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
        { }
    }

    //------------------------------------------------------------------------------------
    protected void sg1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg1.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG1_RMV":

                break;
            case "SG1_ROW_TAX":

                break;
            case "SG1_ROW_DT":

                break;

            case "SG1_ROW_ADD":


                break;
        }
    }
    //------------------------------------------------------------------------------------
    protected void sg2_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg2.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG2_RMV":

                break;
            case "SG2_ROW_ADD":

                break;
        }
    }

    //------------------------------------------------------------------------------------
    protected void sg3_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg3.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "SG3_RMV":

                break;
            case "SG3_ROW_ADD":

                break;
        }
    }
    protected void sg4_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string var = e.CommandName.ToString();
        int rowIndex = ((GridViewRow)((ImageButton)e.CommandSource).NamingContainer).RowIndex;
        int index = Convert.ToInt32(sg4.Rows[rowIndex].RowIndex);

        if (txtvchnum.Value == "-")
        {
            fgen.msg("-", "AMSG", "Doc No. not correct");
            return;
        }
        switch (var)
        {
            case "sg4_RMV":

                break;
            case "sg4_ROW_ADD":

                break;
        }
    }

    //------------------------------------------------------------------------------------


    //------------------------------------------------------------------------------------

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
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        oporow = oDS.Tables[0].NewRow();

        oporow["tBRANCHCD"] = frm_mbr;
        oporow["ID"] = frm_vty;
        oporow[doc_nf.Value] = frm_vnum;
        oporow[doc_df.Value] = txtvchdate.Value.Trim();


        oporow["Name"] = txtlbl2.Value.ToUpper().Trim();
        oporow["type1"] = txtlbl3.Value.ToUpper().Trim();
        oporow["acode"] = txtlbl4.Value.ToUpper().Trim();
        oporow["stform"] = TXT_Itclass.Value.ToUpper().Trim();

        if (frm_formID == "F70176")
        {
            //oporow["TYPE1"]     = TxtCode.Value.ToUpper().Trim();
            //oporow["NAME"]      = TxtName.Value.ToUpper().Trim();
            //oporow["ACODE"]     = txtact.Value.ToUpper().Trim();
            oporow["RCNUM"] = txtchk.Value.ToUpper().Trim();
            oporow["ADDR"] = txtUser.Value.ToUpper().Trim();
            oporow["ADDR1"] = txtUser.Value.ToUpper().Trim().Replace("'", "");
            oporow["ADDR2"] = Text7.Value.ToUpper().Trim();
            oporow["PLACE"] = txtchqno.Value.ToUpper().Trim();
            oporow["EXC_DIV"] = txtunit.Value.Trim();
            oporow["EXC_ITEM"] = txtGstSub.Value.ToUpper().Trim();
            oporow["ZIPCODE"] = txtacpaytop.Value.Trim();
            oporow["EMAIL"] = txtleftac.Value.Trim();
            oporow["website"] = TxtDatetop.Value.Trim();
            oporow["tcs_num"] = TxtLeftDt.Value.Trim();
            oporow["vat_foRm"] = TxtPrtyNme.Value.Trim();
            oporow["cstno"] = TxtPrtyNamelft.Value.Trim();
            oporow["notification"] = TxtAmttop.Value.Trim();
            oporow["Raddr"] = TxtAmtLft.Value.Trim();
            oporow["Raddr1"] = TxtAmtFigtop.Value.Trim();
            oporow["Rphone"] = TxtAmtfglft.Value.Trim();
            oporow["haddr"] = TxtFrmName.Value.Trim();
            oporow["haddr1"] = TxtFrmLft.Value.Trim();
            oporow["hphone"] = TxtAuthSign.Value.Trim();
            oporow["bankname"] = TxtAuthSiglft.Value.Trim();
            oporow["bankaddr"] = TxtAcNo.Value.Trim();
            oporow["bankac"] = TxtAcNoLft.Value.Trim();
            oporow["bond_ut"] = TxtCap.Value.Trim();
        }

        if (frm_formID == "F10126")
        {
            //rate = make ready time 
            //excrate = prd.time
            //exc_tarrif = m/c code
            //rcnum = main grp
            //balop = sheet_ctn

            oporow["addr2"] = Text1.Value.ToUpper().Trim();
            oporow["rate"] = Text2.Value.ToUpper().Trim().toDouble();
            oporow["excrate"] = Text3.Value.ToUpper().Trim().toDouble();
            oporow["exc_tarrif"] = Text4.Value.ToUpper().Trim().toDouble();
            oporow["rcnum"] = Text5.Value.ToUpper().Trim();
            oporow["balop"] = Text6.Value.ToUpper().Trim().toDouble();
        }

        if (edmode.Value == "Y")
        {

            if ((string)ViewState["entdt"] == null || (string)ViewState["entdt"] == "")
            {
                oporow["meNt_by"] = frm_uname;
                oporow["meNt_dt"] = vardate;
            }
            else
            {
                oporow["meNt_by"] = ViewState["entby"].ToString();
                oporow["meNt_dt"] = ViewState["entdt"].ToString();
            }

            oporow["medt_by"] = frm_uname;
            oporow["medt_dt"] = vardate;
        }
        else
        {
            oporow["meNt_by"] = frm_uname;
            oporow["meNt_dt"] = vardate;
            oporow["medt_by"] = "-";
            oporow["meDt_dt"] = vardate;
        }
        oDS.Tables[0].Rows.Add(oporow);

    }

    void Acode_Sel_query()
    {

    }
    void Icode_Sel_query()
    {

    }

    void Type_Sel_query()
    {
        //string comb_char;
        //SQuery = "";
        //string tbl_cond="";
        //frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        //frm_fchar = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FCHAR");
        //comb_char = frm_vty;

        //if (tbl_cond == "")
        //{
        //    tbl_cond = "1=1";
        //}


        //Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        //SQuery = "select distinct a.type1 as fstr,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt,a.Name,a.type1 as Code,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where  a.tbranchcd='00' and trim(a.ID)='" + frm_vty + "' and SUBSTR(TRIM(a.TYPE1),1,1)='" + frm_fchar + "'  order by a.type1";
        //switch (Prg_Id)
        //{
        //    case "F99154":
        //        SQuery = "select distinct a.type1 as fstr,a." + doc_nf.Value + " as Opt_No,to_char(a." + doc_df.Value + ",'dd/mm/yyyy') as Opt_Dt,a.Name,a.type1 as Code,a.MEnt_by,to_char(a.Ment_Dt,'dd/mm/yyyy') as Ent_dt,to_Char(a." + doc_df.Value + ",'yyyymmdd') as vdd from " + frm_tabname + " a where  a.tbranchcd='00' and trim(a.ID)='" + frm_vty + "' and SUBSTR(TRIM(a.TYPE1),1,1)='" + frm_fchar + "' and " + tbl_cond + "  order by a.type1";
        //        break;

        //}

    }

    //------------------------------------------------------------------------------------   
    protected void sg4_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int sg1r = 0; sg1r < sg4.Rows.Count; sg1r++)
            {
                for (int j = 0; j < sg4.Columns.Count; j++)
                {
                    sg4.Rows[sg1r].Cells[j].ToolTip = sg4.Rows[sg1r].Cells[j].Text;
                    if (sg4.Rows[sg1r].Cells[j].Text.Trim().Length > 35)
                    {
                        sg4.Rows[sg1r].Cells[j].Text = sg4.Rows[sg1r].Cells[j].Text.Substring(0, 35);
                    }
                }
            }
            e.Row.Cells[0].Style["display"] = "none";
            sg4.HeaderRow.Cells[0].Style["display"] = "none";
            e.Row.Cells[1].Style["display"] = "none";
            sg4.HeaderRow.Cells[1].Style["display"] = "none";
        }
    }

    protected void btntype_Click(object sender, ImageClickEventArgs e)
    {
        if (edmode.Value == "Y")
        {
            fgen.msg("-", "AMSG", "Code Change not Allowed in Edit mode !!");

            return;
        }
        else
        {
            hffield.Value = "TYPECODE";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Type Code", frm_qstr);
        }
    }
    protected void btnactg_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ACTGCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Account Code", frm_qstr);
    }
    protected void btnAcCode_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void BtnUser_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void btn_ITC_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ITM_CLASS";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Item Class", frm_qstr);
    }

    protected void BtnAct_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "ACCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Account Code", frm_qstr);
    }
    protected void BtnUser_Click1(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "USER";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select USER", frm_qstr);
    }
}