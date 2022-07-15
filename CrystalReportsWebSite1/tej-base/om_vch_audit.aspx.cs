using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_vch_audit : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, typePopup = "Y";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4;
    int i = 0, z = 0;
    string hscode = "", msg = "";
    DataTable sg1_dt; DataRow sg1_dr;
    DataTable sg2_dt; DataRow sg2_dr;
    DataTable sg3_dt; DataRow sg3_dr;

    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cmd_query;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
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
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                doc_addl.Value = "-";
                string chk_opt = "";
                chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0009'", "fstr");
                if (chk_opt == "Y")
                {
                    doc_addl.Value = "Y";
                }
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();

                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_OPEN_IN_EDIT") == "Y")
                    editFunction(fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"));
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

                ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Attributes.Add("autocomplete", "off");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t2")).Attributes.Add("autocomplete", "off");

                ((TextBox)sg1.Rows[K].FindControl("sg1_t10")).Attributes.Add("readonly", "readonly");
                ((TextBox)sg1.Rows[K].FindControl("sg1_t11")).Attributes.Add("readonly", "readonly");

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

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F70122C":
                lbl4.Text = "Account";
                break;
            default:
                break;

        }


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
        frm_tabname = "MULTIVCH";

        switch (Prg_Id)
        {
            case "F70122C":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "3Z");
                break;
            default:
                lblheader.Text = "Voucher Audit";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "AU");
                break;
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        typePopup = "Y";

        btnedit.Visible = false;
        btndel.Visible = false;
        btnprint.Visible = false;
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {

        SQuery = "";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
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
                switch (Prg_Id)
                {
                    case "F70122C":
                        SQuery = "Select Acode as fstr,Aname,Acode,Addr1,Addr2 from famst where substr(Acode,1,2) in ('02','06','16') order by Aname";
                        break;
                    default:
                        SQuery = "Select type1 as Code,Name,type1 from typegrp where branchcd!='DD' and id='JV' order by type1";
                        break;
                }
                break;

            case "TICODE":
                //pop2
                SQuery = "SELECT Type1 AS FSTR,NAME AS Deptt,Type1 AS CODE FROM type where id='M' and type1 like '6%' order by Name";
                break;
            case "SG1_ROW_ADD":
            case "SG1_ROW_ADD_E":

                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {

                    if (gr.Cells[13].Text.Trim().Length > 2)
                    {
                        if (col1.Length > 0) col1 = col1 + "," + "'" + ((TextBox)gr.FindControl("sg1_t3")).Text.Trim() + "'";
                        else col1 = "'" + ((TextBox)gr.FindControl("sg1_t3")).Text.Trim() + "'";
                    }
                }
                if (col1.Length > 0)
                {
                    col1 = " TRIM(type1) not in (" + col1 + ")";
                }
                else
                {
                    col1 = "1=1";
                }

                SQuery = "select TRIM(type1) as fstr,name,type1 as code from type where id='V' AND " + col1 + "  ORDER BY code";

                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);

                break;


            case "SG1_ROW_ADD2":
            case "SG1_ROW_ADD2_E":
                if (Prg_Id == "F70122C")
                {
                    SQuery = "";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);

                    return;

                }

                switch (Prg_Id)
                {
                    case "F70122C":
                        SQuery = "select to_char(invdate,'yyyymmdd')||trim(acode)||trim(invno) As fstr,trim(invno) as Inv_no,to_char(invdate,'dd/mm/yyyy') as Inv_Date,sum(dramt) as dramt,sum(cramt) as cramt,sum(dramt)-sum(cramt) as Net_Amt,to_char(invdate,'yyyymmdd') as Invd,trim(acode) As Acc_code from (select acode,invno,invdate,dramt,cramt from recebal where branchcd='" + frm_mbr + "' and acode='" + txtlbl4.Text.Trim() + "' union all select acode,invno,invdate,dramt,cramt from voucher where branchcd='" + frm_mbr + "' and acode='" + txtlbl4.Text.Trim() + "') group by trim(acode),trim(invno),to_char(invdate,'dd/mm/yyyy'),to_char(invdate,'yyyymmdd')||trim(acode)||trim(invno),to_char(invdate,'yyyymmdd')  having sum(dramt)-sum(cramt) <>0 order by to_char(invdate,'yyyymmdd'),trim(invno) ";
                        break;
                    default:
                        SQuery = "select a.Acode as fstr,a.ANAME as Account_Name,a.Acode as Ac_Code,a.Addr1 as Address,a.Addr2 as City,a.grp,b.Name,a.Payment from famst a left outer join ( select name,type1 from type where id='Z') b on trim(a.grp)=trim(b.type1) where a.branchcd='00' AND length(Trim(nvl(a.deac_by,'-')))<=1 order by a.aname ";

                        break;
                }
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_ITEMQRY", SQuery);

                break;
            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
                break;
            case "sg1_t2":
                SQuery = "select a.Acode as fstr,a.ANAME as Account_Name,a.Acode as Ac_Code,a.Addr1 as Address,a.Addr2 as City,a.grp,b.Name,a.Payment from famst a left outer join ( select name,type1 from type where id='Z') b on trim(a.grp)=trim(b.type1) where a.branchcd='00' AND length(Trim(nvl(a.deac_by,'-')))<=1 order by a.aname ";
                break;
            case "sg1_t6":
            case "sg1_t7":
            case "sg1_t8":
            case "sg1_t9":
            case "sg1_t10":
                SQuery = "select Acode as fstr,ANAME as Party,Acode as Code,Addr1 as Address,Addr2 as City,Payment from famst where branchcd='00' AND length(Trim(nvl(deac_by,'-')))<=1 order by aname ";
                break;
            case "SG1_ROW_INV_E":
            case "SG1_ROW_INV":
                if (Prg_Id == "F70122C")
                {
                    return;

                }

                col1 = "";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (((TextBox)gr.FindControl("sg1_t3")).Text.Trim().Length > 1)
                    {
                        if (col1.Length > 0) col1 = col1 + ",'" + ((TextBox)gr.FindControl("sg1_t3")).Text.Trim() + ((TextBox)gr.FindControl("sg1_t4")).Text.Trim() + "'";
                        else col1 = "'" + ((TextBox)gr.FindControl("sg1_t3")).Text.Trim() + ((TextBox)gr.FindControl("sg1_t4")).Text.Trim() + "'";
                    }
                }
                string cond = "";
                if (col1.Length > 2) cond = " and trim(a.invno)||to_char(a.invdate,'dd/mm/yyyy') not in (" + col1 + ") ";
                SQuery = "SELECT TRIM(A.INVNO) AS FSTR,TRIM(A.INVNO) AS INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,A.DRAMT AS DR,A.CRAMT AS CR,NET AS BALANCE,B.ANAME AS ACCOUNT,A.ACODE AS CODE FROM RECDATA A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODe) AND A.BRANCHCD='" + frm_mbr + "' AND trim(A.acode)='" + sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text + "' AND A.NET!=0 " + cond + " ORDER BY A.INVNO,A.INVDATE ";
                break;
            case "New":
            case "Edit":
            case "Del":
                Type_Sel_query();
                break;

            case "Print":
                SQuery = "SELECT type1 AS FSTR,name as NAME,type1 AS CODE FROM TYPE WHERE ID='V' AND TYPE1 in ('30','33','37','38') ORDER BY TYPE1 ";
                break;

            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as JV_no,to_char(a.vchdate,'dd/mm/yyyy') as Vch_Dt,b.aname as Account,a.ent_by,to_char(a.ent_Date,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                if (lbl1a.Text == "38" && btnval == "COPY_OLD")
                {
                    SQuery = "select distinct trim(A.branchcd)||trim(A.type)||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as JV_no,to_char(a.vchdate,'dd/mm/yyyy') as Vch_Dt,a.type,b.aname as Account,a.ent_by,to_char(a.ent_Date,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type like '%' and vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
                }
                if (btnval == "Print_E")
                    SQuery = "select distinct a.branchcd||a.type||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as JV_no,to_char(a.vchdate,'dd/mm/yyyy') as Vch_Dt,b.aname as Account,a.ent_by,to_char(a.ent_Date,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
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
            typePopup = "N";
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
        string orig_vchdt;
        orig_vchdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_OLD_DATE");
        if (edmode.Value != "Y")
        {
            orig_vchdt = txtvchdate.Text;
        }

        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }

        fgen.msg("-", "SMSG", "You are going to audit all the documents of the selected dates. This will auto audit all the vouchers during the date and can not be edited or deleted after done'13'Are You Sure, You Want To Save!!");
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
            fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr);// REMOVE TYPE FROM THE LINE BY MADHVI ON 28 JULY 2018
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_OPEN_IN_EDIT") == "Y")
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OPEN_IN_EDIT", "N");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FORMID", fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMDRILLID"));
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup();", true);
        }
        else Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
        //Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr);
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
        //fgen.Fn_open_Act_itm_prd("-", frm_qstr);
        fgen.Fn_open_prddmp1("", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {

        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr); // BY MADHVI ON 28 JULY 2018
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        frm_vty = vty;
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
        if (sg1.Rows.Count > 0)
            ((ImageButton)sg1.Rows[0].FindControl("sg1_btnadd")).Focus();
        // Popup asking for Copy from Older Data
        if (frm_cocd == "SGRP" || frm_cocd == "UATS")
            fgen.msg("-", "CMSG", "Do You Want to Copy From Existing Document'13'(No for make it new)");
        hffield.Value = "NEW_E";
        #endregion
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
        //--
        string CP_BTN;
        string CP_HF1;
        CP_BTN = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CMD_FROM");
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
                //string chk_po_made;
                //chk_po_made = fgen.seek_iname(frm_qstr, frm_cocd, "select ordno||' Dt.'||to_char(orddt,'dd/mm/yyyy') As fstr from pomas where branchcd||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' and type like '5%'", "fstr");
                //if (chk_po_made.Length > 6)
                //{
                //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Purchase Order no ." + chk_po_made + " Already Made Against This P.R. , Deletion is Not Allowed !!");
                //    return;
                //}
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                // Showing Confirmation
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
                    SQuery = "Select a.*,nvl(a.ent_by,'-') as endt_by,to_char(a.ent_Date,'dd/mm/yyyy') as entd_dt,to_char(a.edt_Date,'dd/mm/yyyy') as edtd_dt,'-' as app_by,b.aname,c.aname as rname from " + frm_tabname + " a,famst b,famst c where trim(a.acode)=trim(b.acode) and trim(a.rcode)=trim(c.acode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    SQuery = "Select a.*,nvl(a.ent_by,'-') as endt_by,to_char(a.ent_Date,'dd/mm/yyyy') as entd_dt,to_char(a.edt_Date,'dd/mm/yyyy') as edtd_dt,'-' as app_by,b.aname from " + frm_tabname + " a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    if (lbl1a.Text == "38" && btnval == "COPY_OLD")
                    {
                        SQuery = "Select a.*,nvl(a.ent_by,'-') as endt_by,to_char(a.ent_Date,'dd/mm/yyyy') as entd_dt,to_char(a.edt_Date,'dd/mm/yyyy') as edtd_dt,'-' as app_by,b.aname from " + frm_tabname + " a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' ORDER BY A.SRNO";
                    }
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        if (lbl1a.Text == "38" && btnval == "COPY_OLD")
                        {

                        }
                        else
                        {
                            lbl1a.Text = frm_vty;
                        }


                        txtlbl4.Text = dt.Rows[i]["depcd"].ToString().Trim();
                        //txtlbl4a.Text = dt.Rows[i]["kindattn"].ToString().Trim();

                        txtRmk.Text = dt.Rows[0]["naration"].ToString().Trim();

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
                            if (lbl1a.Text == "38" && btnval == "COPY_OLD")
                            {
                                sg1_dr["sg1_f1"] = dt.Rows[i]["acode"].ToString().Trim();
                                sg1_dr["sg1_f2"] = dt.Rows[i]["aname"].ToString().Trim();
                                sg1_dr["sg1_f3"] = "-";

                                sg1_dr["sg1_f4"] = dt.Rows[i]["rcode"].ToString().Trim();
                                sg1_dr["sg1_f5"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(aCODE)='" + dt.Rows[i]["rcode"].ToString().Trim() + "'", "ANAME");

                                sg1_dr["sg1_t2"] = dt.Rows[i]["dramt"].ToString().Trim();
                                sg1_dr["sg1_t1"] = dt.Rows[i]["cramt"].ToString().Trim();

                            }
                            else
                            {
                                sg1_dr["sg1_f1"] = dt.Rows[i]["acode"].ToString().Trim();
                                sg1_dr["sg1_f2"] = dt.Rows[i]["aname"].ToString().Trim();
                                sg1_dr["sg1_f3"] = "-";

                                sg1_dr["sg1_f4"] = dt.Rows[i]["rcode"].ToString().Trim();
                                sg1_dr["sg1_f5"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(aCODE)='" + dt.Rows[i]["rcode"].ToString().Trim() + "'", "ANAME");

                                sg1_dr["sg1_t1"] = dt.Rows[i]["dramt"].ToString().Trim();
                                sg1_dr["sg1_t2"] = dt.Rows[i]["cramt"].ToString().Trim();
                            }
                            sg1_dr["sg1_t3"] = dt.Rows[i]["invno"].ToString().Trim();
                            sg1_dr["sg1_t4"] = Convert.ToDateTime(dt.Rows[i]["invdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                            sg1_dr["sg1_t5"] = dt.Rows[i]["naration"].ToString().Trim();

                            //if (dt.Rows[i]["delv_item"].ToString().Trim().Length == 10)
                            //{
                            //    sg1_dr["sg1_t6"] = Convert.ToDateTime(dt.Rows[i]["delv_item"].ToString().Trim()).ToString("yyy-MM-dd"); // ADD Convert.ToDateTime IN THE LINE SO THAT WHEN DATE IS SAVED FROM MAIN IT WILL SHOW IN THE WEB BY MADHVI ON 23 JULY 2018
                            //}
                            //sg1_dr["sg1_t7"] = dt.Rows[i]["psize"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        sg1_add_blankrows();
                        ViewState["sg1"] = sg1_dt;
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
                    if (col1 == "") return;
                    editFunction(frm_mbr + frm_vty + col1);
                    break;
                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;
                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "" + col1 + "");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F70203");
                    fgen.fin_acct_reps(frm_qstr);
                    break;
                case "TACODE":
                    if (col1.Length <= 0) return;
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    string sal_Grd = "01";

                    switch (txtlbl4.Text)
                    {
                        case "001":
                            //salry_vch
                            # region
                            string code_pf_pyb = fgen.seek_iname(frm_qstr, frm_cocd, "Select opt_param from fin_rsys_opt where opt_id='W0068'", "opt_param");
                            string code_sal_pyb = fgen.seek_iname(frm_qstr, frm_cocd, "Select opt_param from fin_rsys_opt where opt_id='W0069'", "opt_param");
                            string code_tot_sal = fgen.seek_iname(frm_qstr, frm_cocd, "Select opt_param from fin_rsys_opt where opt_id='W0070'", "opt_param");

                            string sal_qry = "";
                            string pop_qry = "";
                            string amt_er1 = "", amt_er2 = "", amt_er3 = "", amt_er4 = "", amt_er5 = "", amt_er6 = "", amt_er7 = "", amt_er8 = "", amt_er9 = "", amt_er10 = "";
                            string amt_Ded1 = "", amt_Ded2 = "", amt_Ded3 = "", amt_Ded4 = "", amt_Ded5 = "", amt_Ded6 = "", amt_Ded7 = "", amt_Ded8 = "", amt_Ded9 = "", amt_Ded10 = "";
                            string pf_amt_comp = "";
                            string totern = "", netSlry = "";



                            pop_qry = "SELECT sum(nvl(Er1,0))||'~'||sum(nvl(Er2,0))||'~'||sum(nvl(Er3,0))||'~'||sum(nvl(Er4,0))||'~'||sum(nvl(Er5,0))||'~'||sum(nvl(Er6,0))||'~'||sum(nvl(Er7,0))||'~'||sum(nvl(Er8,0))||'~'||sum(nvl(Er9,0))||'~'||sum(nvl(Er10,0))||'~'||sum(nvl(totern,0))||'~'||sum(nvl(netslry,0)) AS PP FROM pay WHERE branchcd='" + frm_mbr + "' and grade='" + sal_Grd + "'  and to_char(date_,'yyyymm')=to_char(to_Date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy'),'yyyymm')";
                            sal_qry = fgen.seek_iname(frm_qstr, frm_cocd, pop_qry, "PP");
                            if (sal_qry.Length > 1)
                            {
                                amt_er1 = sal_qry.Split('~')[0].ToString();
                                amt_er2 = sal_qry.Split('~')[1].ToString();
                                amt_er3 = sal_qry.Split('~')[2].ToString();
                                amt_er4 = sal_qry.Split('~')[3].ToString();
                                amt_er5 = sal_qry.Split('~')[4].ToString();
                                amt_er6 = sal_qry.Split('~')[5].ToString();
                                amt_er7 = sal_qry.Split('~')[6].ToString();
                                amt_er8 = sal_qry.Split('~')[7].ToString();
                                amt_er9 = sal_qry.Split('~')[8].ToString();
                                amt_er10 = sal_qry.Split('~')[9].ToString();
                                totern = sal_qry.Split('~')[10].ToString();
                                netSlry = sal_qry.Split('~')[11].ToString();
                            }


                            pop_qry = "SELECT sum(nvl(ded1,0))||'~'||sum(nvl(ded2,0))||'~'||sum(nvl(ded3,0))||'~'||sum(nvl(ded4,0))||'~'||sum(nvl(ded5,0))||'~'||sum(nvl(ded6,0))||'~'||sum(nvl(ded7,0))||'~'||sum(nvl(ded8,0))||'~'||sum(nvl(ded9,0))||'~'||sum(nvl(ded10,0))||'~'||sum(nvl(pf_amt_cs,0)) AS PP FROM pay WHERE branchcd='" + frm_mbr + "' and grade='" + sal_Grd + "'  and  to_char(date_,'yyyymm')=to_char(to_Date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy'),'yyyymm')";
                            sal_qry = fgen.seek_iname(frm_qstr, frm_cocd, pop_qry, "PP");
                            if (sal_qry.Length > 1)
                            {
                                amt_Ded1 = sal_qry.Split('~')[0].ToString();
                                amt_Ded2 = sal_qry.Split('~')[1].ToString();
                                amt_Ded3 = sal_qry.Split('~')[2].ToString();
                                amt_Ded4 = sal_qry.Split('~')[3].ToString();
                                amt_Ded5 = sal_qry.Split('~')[4].ToString();
                                amt_Ded6 = sal_qry.Split('~')[5].ToString();
                                amt_Ded7 = sal_qry.Split('~')[6].ToString();
                                amt_Ded8 = sal_qry.Split('~')[7].ToString();
                                amt_Ded9 = sal_qry.Split('~')[8].ToString();
                                amt_Ded10 = sal_qry.Split('~')[9].ToString();
                                pf_amt_comp = sal_qry.Split('~')[10].ToString();
                            }



                            SQuery = "select distinct ed_fld,aname,ed_ff,substr(mo_Acode,3,10) as aacode from (Select distinct b.aname,a.ed_fld,substr(a.ed_fld,1,1) as ed_ff,lpad(trim(to_char(a.morder,'99')),2,'0')||trim(a.fa_code) as MO_ACode from wb_selmast a,famst b where a.branchcd='" + frm_mbr + "' and trim(a.fa_code)=trim(b.acode) and a.grade='" + sal_Grd + "' ORDER BY substr(a.ed_fld,1,1) desc,lpad(trim(to_char(a.morder,'99')),2,'0')||trim(a.fa_code) ) order by ed_ff desc";

                            DataTable dtm = new DataTable();
                            dtm.Columns.Add("aacode");
                            dtm.Columns.Add("aname");
                            dtm.Columns.Add("ed_ff");
                            dtm.Columns.Add("acd_damt", typeof(double));
                            dtm.Columns.Add("acd_camt", typeof(double));
                            dtm.Columns.Add("cost_Cent");
                            DataRow drm;

                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                            ViewState["fstr"] = col1;
                            dt = new DataTable();
                            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                            if (dt.Rows.Count > 0)
                            {
                                DataTable dtDistA = new DataTable();
                                DataView dvdis = new DataView(dt, "ED_FF='D'", "AACODE", DataViewRowState.CurrentRows);
                                dtDistA = dvdis.ToTable(true, "AACODE", "aname", "ed_ff");

                                double totAmt = 0;





                                pop_qry = "SELECT cc1,sum(nvl(totern,0)) as totern,sum(nvl(netslry,0)) AS netslry FROM pay WHERE branchcd='" + frm_mbr + "' and grade='" + sal_Grd + "'  and to_char(date_,'yyyymm')=to_char(to_Date('" + txtvchdate.Text.Trim() + "','dd/mm/yyyy'),'yyyymm') group by cc1 having sum(nvl(totern,0))>0 ";
                                DataTable sdt;
                                sdt = new DataTable();
                                sdt = fgen.getdata(frm_qstr, frm_cocd, pop_qry);
                                if (sdt.Rows.Count > 0)
                                {

                                    for (i = 0; i < sdt.Rows.Count; i++)
                                    {
                                        drm = dtm.NewRow();
                                        drm["aacode"] = code_sal_pyb;
                                        drm["aname"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(ACODE)='" + drm["aacode"].ToString().Trim() + "'", "aname");
                                        drm["ed_ff"] = "";
                                        drm["acd_damt"] = sdt.Rows[i]["totern"].ToString().Trim();
                                        drm["cost_Cent"] = sdt.Rows[i]["cc1"].ToString().Trim();
                                        dtm.Rows.Add(drm);

                                    }
                                }


                                drm = dtm.NewRow();
                                drm["aacode"] = code_pf_pyb;
                                drm["aname"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(ACODE)='" + drm["aacode"].ToString().Trim() + "'", "aname");
                                drm["ed_ff"] = "";
                                drm["acd_damt"] = pf_amt_comp.toDouble();
                                dtm.Rows.Add(drm);

                                foreach (DataRow dr in dtDistA.Rows)
                                {
                                    //0400001
                                    dvdis = new DataView(dt, "AACODE='" + dr["AACODE"].ToString() + "'", "", DataViewRowState.CurrentRows);
                                    for (int x = 0; x < dvdis.Count; x++)
                                    {
                                        totAmt = 0;
                                        switch (dvdis[x].Row["ed_fld"].ToString().ToUpper().Trim())
                                        {
                                            case "DED1":
                                                totAmt += amt_Ded1.toDouble() + pf_amt_comp.toDouble();
                                                break;
                                            case "DED2":
                                                totAmt += amt_Ded2.toDouble();
                                                break;
                                            case "DED3":
                                                totAmt += amt_Ded3.toDouble();
                                                break;
                                            case "DED4":
                                                totAmt += amt_Ded4.toDouble();
                                                break;
                                            case "DED5":
                                                totAmt += amt_Ded5.toDouble();
                                                break;
                                            case "DED6":
                                                totAmt += amt_Ded6.toDouble();
                                                break;
                                            case "DED7":
                                                totAmt += amt_Ded7.toDouble();
                                                break;
                                            case "DED8":
                                                totAmt += amt_Ded8.toDouble();
                                                break;
                                            case "DED9":
                                                totAmt += amt_Ded9.toDouble();
                                                break;
                                            case "DED10":
                                                totAmt += amt_Ded10.toDouble();
                                                break;
                                        }
                                    }

                                    drm = dtm.NewRow();
                                    drm["aacode"] = dr["aacode"].ToString();
                                    drm["aname"] = dr["aname"].ToString();
                                    drm["ed_ff"] = dr["ed_ff"].ToString();
                                    drm["acd_camt"] = totAmt;
                                    dtm.Rows.Add(drm);
                                }

                                drm = dtm.NewRow();
                                drm["aacode"] = code_tot_sal;
                                drm["aname"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(ACODE)='" + drm["aacode"].ToString().Trim() + "'", "aname");
                                drm["ed_ff"] = "";

                                drm["acd_camt"] = fgen.make_double(netSlry);

                                dtm.Rows.Add(drm);







                                if (dtm.Rows.Count > 0)
                                {
                                    txtRmk.Text = "Salary for the Month ending " + txtvchdate.Text.Trim();

                                    create_tab();
                                    sg1_dr = null;
                                    for (i = 0; i < dtm.Rows.Count; i++)
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
                                        sg1_dr["sg1_f1"] = dtm.Rows[i]["aacode"].ToString().Trim();
                                        sg1_dr["sg1_f2"] = dtm.Rows[i]["aname"].ToString().Trim();
                                        sg1_dr["sg1_f3"] = "-";

                                        sg1_dr["sg1_t1"] = dtm.Rows[i]["acd_damt"].ToString().Trim();
                                        sg1_dr["sg1_t2"] = dtm.Rows[i]["acd_camt"].ToString().Trim();
                                        sg1_dr["sg1_t3"] = dtm.Rows[i]["cost_Cent"].ToString().Trim();


                                        if (dtm.Rows[i]["cost_Cent"].ToString().Trim().Length > 1)
                                        {
                                            sg1_dr["sg1_t5"] = fgen.seek_iname(frm_qstr, frm_cocd, "select Name from typegrp where branchcd!='DD' and id='L1' and type1='" + dtm.Rows[i]["cost_Cent"].ToString().Trim() + "'", "name");
                                        }
                                        sg1_dt.Rows.Add(sg1_dr);
                                    }

                                    sg1_add_blankrows();
                                    ViewState["sg1"] = sg1_dt;
                                    sg1.DataSource = sg1_dt;
                                    sg1.DataBind();
                                    dt.Dispose(); sg1_dt.Dispose();
                                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                                    fgen.EnableForm(this.Controls);
                                    disablectrl();
                                    setColHeadings();
                                }
                            }
                            #endregion
                            break;
                        case "002":
                            break;
                        case "003":
                        case "004":
                            //amtorz vch
                            # region

                            string vtype_rmk = "";
                            if (txtlbl4.Text == "004")
                            {
                                SQuery = "Select to_char(a.amtz_date,'dd/mm/yyyy') as amtz_date,to_chaR(a.vchdate,'yyyy')||a.type||a.vchnum as ref_doc,to_Char(a.amtz_date,'yyyy Mon') as exp_mth,a.amtz_ppcode as amtz_xpcode,a.amtz_xpcode as amtz_ppcode,b.aname as amtz_xpname,c.aname as amtz_ppname,sum(nvl(a.amtz_amt,0)) as amtz_amt from WB_PPVCH_DTL a,famst b,famst c where trim(a.amtz_ppcode)=trim(b.acode) and trim(a.amtz_xpcode)=trim(c.acode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='1' and to_Char(a.amtz_date,'yyyymm')=to_char(to_Date('" + txtvchdate.Text + "','dd/mm/yyyy'),'yyyymm') and to_chaR(a.vchdate,'yyyy')||a.type||a.vchnum||to_char(a.amtz_date,'dd/mm/yyyy') not in (Select trim(invno)||to_char(invdate,'dd/mm/yyyy') from voucher where branchcd='" + frm_mbr + "' and type='30' and vchdate " + DateRange + ") group by to_chaR(a.vchdate,'yyyy')||a.type||a.vchnum,to_Char(a.amtz_date,'yyyy Mon'),a.amtz_ppcode,a.amtz_xpcode,b.aname,c.aname,to_char(a.amtz_date,'dd/mm/yyyy') order by to_chaR(a.vchdate,'yyyy')||a.type||a.vchnum";
                                vtype_rmk = " Installment";
                            }
                            else
                            {
                                SQuery = "Select to_char(a.amtz_date,'dd/mm/yyyy') as amtz_date,to_chaR(a.vchdate,'yyyy')||a.type||a.vchnum as ref_doc,to_Char(a.amtz_date,'yyyy Mon') as exp_mth,a.amtz_ppcode,a.amtz_xpcode,b.aname as amtz_ppname,c.aname as amtz_xpname,sum(nvl(a.amtz_amt,0)) as amtz_amt from WB_PPVCH_DTL a,famst b,famst c where trim(a.amtz_ppcode)=trim(b.acode) and trim(a.amtz_xpcode)=trim(c.acode) and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='2' and to_Char(a.amtz_date,'yyyymm')=to_char(to_Date('" + txtvchdate.Text + "','dd/mm/yyyy'),'yyyymm') and to_chaR(a.vchdate,'yyyy')||a.type||a.vchnum||to_char(a.amtz_date,'dd/mm/yyyy') not in (Select trim(invno)||to_char(invdate,'dd/mm/yyyy') from voucher where branchcd='" + frm_mbr + "' and type='30' and vchdate " + DateRange + ") group by to_chaR(a.vchdate,'yyyy')||a.type||a.vchnum,to_Char(a.amtz_date,'yyyy Mon'),a.amtz_ppcode,a.amtz_xpcode,b.aname,c.aname,to_char(a.amtz_date,'dd/mm/yyyy') order by to_chaR(a.vchdate,'yyyy')||a.type||a.vchnum";
                                vtype_rmk = " Amortization";

                            }

                            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                            ViewState["fstr"] = col1;
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
                                    sg1_dr["sg1_f1"] = dt.Rows[i]["amtz_xpcode"].ToString().Trim();
                                    sg1_dr["sg1_f2"] = dt.Rows[i]["amtz_xpname"].ToString().Trim();
                                    sg1_dr["sg1_f3"] = "-";
                                    sg1_dr["sg1_f4"] = dt.Rows[i]["amtz_ppcode"].ToString().Trim();
                                    sg1_dr["sg1_f5"] = dt.Rows[i]["amtz_ppname"].ToString().Trim();

                                    sg1_dr["sg1_t1"] = dt.Rows[i]["amtz_amt"].ToString().Trim();


                                    sg1_dr["sg1_t2"] = 0;
                                    sg1_dr["sg1_t3"] = dt.Rows[i]["ref_Doc"].ToString().Trim();
                                    sg1_dr["sg1_t4"] = dt.Rows[i]["amtz_date"].ToString().Trim();
                                    sg1_dr["sg1_t5"] = dt.Rows[i]["exp_mth"].ToString().Trim() + " " + vtype_rmk;
                                    sg1_dt.Rows.Add(sg1_dr);
                                }

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
                                    sg1_dr["sg1_f1"] = dt.Rows[i]["amtz_ppcode"].ToString().Trim();
                                    sg1_dr["sg1_f2"] = dt.Rows[i]["amtz_ppname"].ToString().Trim();
                                    sg1_dr["sg1_f3"] = "-";
                                    sg1_dr["sg1_f4"] = dt.Rows[i]["amtz_xpcode"].ToString().Trim();
                                    sg1_dr["sg1_f5"] = dt.Rows[i]["amtz_xpname"].ToString().Trim();

                                    sg1_dr["sg1_t1"] = 0;
                                    sg1_dr["sg1_t2"] = dt.Rows[i]["amtz_amt"].ToString().Trim();
                                    sg1_dr["sg1_t3"] = dt.Rows[i]["ref_Doc"].ToString().Trim();
                                    sg1_dr["sg1_t4"] = dt.Rows[i]["amtz_date"].ToString().Trim();
                                    sg1_dr["sg1_t5"] = dt.Rows[i]["exp_mth"].ToString().Trim() + " " + vtype_rmk;
                                    sg1_dt.Rows.Add(sg1_dr);
                                }


                                sg1_add_blankrows();
                                ViewState["sg1"] = sg1_dt;
                                sg1.DataSource = sg1_dt;
                                sg1.DataBind();
                                dt.Dispose(); sg1_dt.Dispose();
                                ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                                fgen.EnableForm(this.Controls);
                                disablectrl();
                                setColHeadings();
                                edmode.Value = "N";
                            }

                            #endregion

                            break;
                    }

                    btnlbl7.Focus();
                    break;
                case "sg1_t6":
                    if (col1.Length <= 1) return;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl(hf2.Value)).Text = col1;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t7")).Focus();
                    break;
                case "sg1_t7":
                    if (col1.Length <= 1) return;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl(hf2.Value)).Text = col1;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t8")).Focus();
                    break;
                case "sg1_t8":
                    if (col1.Length <= 1) return;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl(hf2.Value)).Text = col1;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t9")).Focus();
                    break;
                case "sg1_t9":
                    if (col1.Length <= 1) return;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl(hf2.Value)).Text = col1;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t10")).Focus();
                    break;
                case "sg1_t10":
                    if (col1.Length <= 1) return;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl(hf2.Value)).Text = col1;
                    ((ImageButton)sg1.Rows[Convert.ToInt32(hf1.Value) + 1].FindControl("sg1_btnadd")).Focus();
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
                        string last_acd = "";
                        string last_anm = "";
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

                            if (i == 0)
                            {
                                last_acd = dt.Rows[i]["sg1_f1"].ToString();
                                last_anm = dt.Rows[i]["sg1_f2"].ToString();
                            }
                            sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["sg1_f4"].ToString();
                            //sg1.Rows[i].Cells[17].Text;
                            sg1_dr["sg1_f5"] = dt.Rows[i]["sg1_f5"].ToString();
                            //sg1.Rows[i].Cells[18].Text;
                            sg1_dr["sg1_t1"] = (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim());
                            sg1_dr["sg1_t2"] = (((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim());
                            sg1_dr["sg1_t3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                            sg1_dr["sg1_t4"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim(), DateTime.Now.ToString("dd/MM/yyyy"));
                            sg1_dr["sg1_t5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim();
                            sg1_dr["sg1_t6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim();
                            sg1_dr["sg1_t7"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim();
                            sg1_dr["sg1_t8"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        hscode = ""; msg = "";
                        string pop_qry = "";
                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                        switch (Prg_Id)
                        {
                            case "F70122C":
                                SQuery = "select b.aname,b.acode,a.* from (" + pop_qry + ") a,famst b where trim(A.acc_code)=trim(B.acode) and trim(a.fstr) in (" + col1 + ") order by a.fstr";
                                break;
                            default:
                                SQuery = "select a.* from (" + pop_qry + ") a where trim(a.fstr) in (" + col1 + ") order by a.FSTR";
                                break;
                        }


                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;

                            if (1 == 1)
                            {
                                sg1_dr["sg1_h1"] = dt.Rows[d]["code"].ToString().Trim();
                                sg1_dr["sg1_h2"] = dt.Rows[d]["name"].ToString().Trim();
                                sg1_dr["sg1_h3"] = "-";
                                sg1_dr["sg1_h4"] = "-";
                                sg1_dr["sg1_h5"] = "-";
                                sg1_dr["sg1_h6"] = "-";
                                sg1_dr["sg1_h7"] = "-";
                                sg1_dr["sg1_h8"] = "-";
                                sg1_dr["sg1_h9"] = "-";
                                sg1_dr["sg1_h10"] = "-";
                                sg1_dr["sg1_f1"] = dt.Rows[d]["code"].ToString().Trim();
                                sg1_dr["sg1_f2"] = dt.Rows[d]["name"].ToString().Trim();
                                sg1_dr["sg1_f3"] = "-";
                                sg1_dr["sg1_t1"] = Convert.ToDateTime(frm_CDT1).ToString("yyyy-MM-dd");
                                sg1_dr["sg1_t2"] = Convert.ToDateTime(frm_CDT2).ToString("yyyy-MM-dd");
                                sg1_dr["sg1_t3"] = "";

                                sg1_dr["sg1_t6"] = "";
                                sg1_dr["sg1_t7"] = "";
                                sg1_dr["sg1_t8"] = "";
                                sg1_dr["sg1_t9"] = "";
                                sg1_dr["sg1_t10"] = "";
                                sg1_dr["sg1_t11"] = "";
                                sg1_dr["sg1_t12"] = "";

                                sg1_dt.Rows.Add(sg1_dr);
                            }
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    sg1_dt.Dispose();
                    ((ImageButton)sg1.Rows[z].FindControl("sg1_btnrcd")).Focus();

                    #endregion
                    setColHeadings();
                    break;

                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    //********* Saving in Hidden Field
                    hscode = ""; msg = "";
                    {
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[0].Text = col1;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[1].Text = col2;
                        //********* Saving in GridView Value
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text = col1;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[14].Text = col2;
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[15].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[16].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4").ToString().Trim().Replace("&amp", "");
                        sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL5").ToString().Trim().Replace("&amp", "");
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                        if (frm_cocd == "MULT")
                            ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = fgen.make_double(fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, col1, txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'")).ToString();
                    }

                    setColHeadings();
                    break;
                case "SG1_ROW_ADD2":
                case "SG1_ROW_ADD2_E":
                    if (col1.Length <= 0) return;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[17].Text = col1;
                    sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[18].Text = col2;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Focus();
                    setColHeadings();
                    break;
                case "SG1_ROW_INV":
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
                        string last_acd = "";
                        string last_anm = "";
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

                            if (i == 0)
                            {
                                last_acd = dt.Rows[i]["sg1_f1"].ToString();
                                last_anm = dt.Rows[i]["sg1_f2"].ToString();
                            }
                            sg1_dr["sg1_f3"] = dt.Rows[i]["sg1_f3"].ToString();
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[17].Text;
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[18].Text;
                            //sg1.Rows[i].Cells[18].Text;
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
                        hscode = ""; msg = "";
                        string pop_qry = "";
                        pop_qry = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ITEMQRY");
                        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

                        SQuery = "select * from (SELECT TRIM(A.INVNO) AS FSTR,TRIM(A.INVNO) AS INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,A.DRAMT AS DR,A.CRAMT AS CR,NET AS BALANCE,B.ANAME,A.ACODE,abs(NET) AS abs_BALANCE FROM RECDATA A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODe) AND A.BRANCHCD='" + frm_mbr + "' AND trim(A.acode)='" + sg1.Rows[Convert.ToInt32(hf1.Value)].Cells[13].Text + "' AND A.NET!=0 ORDER BY A.INVNO,A.INVDATE) where fstr in (" + col1 + ") ";

                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            if (d == 0)
                            {
                                sg1_dt.Rows[Convert.ToInt16(hf1.Value)]["sg1_h1"] = dt.Rows[d]["acode"].ToString().Trim();
                                sg1_dt.Rows[Convert.ToInt16(hf1.Value)]["sg1_h2"] = dt.Rows[d]["aname"].ToString().Trim();

                                sg1_dt.Rows[Convert.ToInt16(hf1.Value)]["sg1_f1"] = dt.Rows[d]["acode"].ToString().Trim();
                                sg1_dt.Rows[Convert.ToInt16(hf1.Value)]["sg1_f2"] = dt.Rows[d]["aname"].ToString().Trim();

                                sg1_dt.Rows[Convert.ToInt16(hf1.Value)]["sg1_t2"] = (dt.Rows[d]["balance"].ToString().Trim().toDouble() > 0 ? dt.Rows[d]["abs_BALANCE"].ToString().Trim().toDouble() : 0);
                                sg1_dt.Rows[Convert.ToInt16(hf1.Value)]["sg1_t1"] = (dt.Rows[d]["balance"].ToString().Trim().toDouble() > 0 ? 0 : dt.Rows[d]["abs_BALANCE"].ToString().Trim().toDouble());


                                sg1_dt.Rows[Convert.ToInt16(hf1.Value)]["sg1_t3"] = dt.Rows[d]["invno"].ToString().Trim();
                                sg1_dt.Rows[Convert.ToInt16(hf1.Value)]["sg1_t4"] = dt.Rows[d]["invdate"].ToString().Trim();
                            }
                            else
                            {
                                sg1_dr = sg1_dt.NewRow();
                                sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                                sg1_dr["sg1_h1"] = dt.Rows[d]["acode"].ToString().Trim();
                                sg1_dr["sg1_h2"] = dt.Rows[d]["aname"].ToString().Trim();
                                sg1_dr["sg1_h3"] = "-";
                                sg1_dr["sg1_h4"] = "-";
                                sg1_dr["sg1_h5"] = "-";
                                sg1_dr["sg1_h6"] = "-";
                                sg1_dr["sg1_h7"] = "-";
                                sg1_dr["sg1_h8"] = "-";
                                sg1_dr["sg1_h9"] = "-";
                                sg1_dr["sg1_h10"] = "-";
                                sg1_dr["sg1_f1"] = dt.Rows[d]["acode"].ToString().Trim();
                                sg1_dr["sg1_f2"] = dt.Rows[d]["aname"].ToString().Trim();
                                sg1_dr["sg1_f3"] = "-";

                                sg1_dr["sg1_t2"] = (dt.Rows[d]["balance"].ToString().Trim().toDouble() > 0 ? dt.Rows[d]["abs_BALANCE"].ToString().Trim().toDouble() : 0);
                                sg1_dr["sg1_t1"] = (dt.Rows[d]["balance"].ToString().Trim().toDouble() > 0 ? 0 : dt.Rows[d]["abs_BALANCE"].ToString().Trim().toDouble());

                                sg1_dr["sg1_t3"] = dt.Rows[d]["invno"].ToString().Trim();
                                sg1_dr["sg1_t4"] = dt.Rows[d]["invdate"].ToString().Trim();
                                sg1_dr["sg1_t5"] = "";


                                sg1_dr["sg1_f4"] = sg1_dt.Rows[Convert.ToInt16(hf1.Value)]["sg1_f4"];
                                sg1_dr["sg1_f5"] = sg1_dt.Rows[Convert.ToInt16(hf1.Value)]["sg1_f5"];


                                sg1_dr["sg1_t6"] = "";
                                sg1_dr["sg1_t7"] = "";
                                sg1_dr["sg1_t8"] = "";
                                sg1_dr["sg1_t9"] = "";
                                sg1_dr["sg1_t10"] = "";
                                sg1_dr["sg1_t11"] = "";
                                sg1_dr["sg1_t12"] = "";

                                sg1_dt.Rows.Add(sg1_dr);
                            }
                        }
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    sg1_dt.Dispose();
                    ((ImageButton)sg1.Rows[z].FindControl("sg1_btnrcd")).Focus();

                    #endregion
                    setColHeadings();
                    break;
                case "SG1_ROW_INV_E":
                    if (col1.Length <= 0) return;
                    if (sg1.Rows.Count <= 0) return;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = col1;
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = col3;
                    if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").toDouble() > 0)
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = Math.Abs(fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").toDouble()).ToString();
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = "0";
                    }
                    else
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = "0";
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = Math.Abs(fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").toDouble()).ToString();
                    }
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Focus();
                    setColHeadings();
                    break;
                case "sg1_INV_ADD":
                    if (col1.Length > 1)
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Text = col1;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t4")).Text = col2;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Focus();
                    }
                    else ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Focus();
                    break;

                case "SG3_ROW_ADD":

                    break;
                case "SG2_RMV":

                    break;
                case "SG3_RMV":

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
                            sg1_dr["sg1_f4"] = sg1.Rows[i].Cells[17].Text.Trim();
                            sg1_dr["sg1_f5"] = sg1.Rows[i].Cells[18].Text.Trim();

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
                case "sg1_t2":
                    if (col1.Length > 1)
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t7")).Text = col1;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = "#F.O. No: " + col1;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Focus();
                    }
                    else ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Focus();
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
            SQuery = "select a.vchnum as vOUCHER_NO,to_char(a.vchdate,'dd/mm/yyyy') as VCH_Dt,b.aname as accounts,a.dramt,a.cramt,a.srno,A.TYPE,to_Char(a.vchdate,'yyyymmdd') as vdd from voucher a,famst b where trim(A.acode)=trim(B.acode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + " order by vdd desc,a.vchnum desc,a.srno";
            //SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, "F15126", "branchcd='" + frm_mbr + "'", "a.type='60' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'", PrdRange);
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel(lblheader.Text + " Checklist for the Period " + fromdt + " to " + todt, frm_qstr);

            hffield.Value = "-";
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
                                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 1)
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

                        //fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                        //fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");

                        if (edmode.Value == "Y")
                        {

                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                            fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully");

                        }
                        else
                        {

                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "vipin@Tejaxo.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully");
                            }
                            else
                            {
                                fgen.msg("-", "AMSG", "Data Not Saved");
                            }
                        }
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_COL1", "'" + frm_mbr + frm_vty + frm_vnum + txtvchdate.Text.Trim() + "'");

                        foreach (GridViewRow gr in sg1.Rows)
                        {
                            if (gr.Cells[13].Text.Trim().Length > 1)
                            {
                                fgen.execute_cmd(frm_qstr, frm_cocd, "UPDATE VOUCHER SET AUDT_BY='" + frm_uname + "' ,AUDT_DT =SYSDATE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + gr.Cells[13].Text.Trim() + "' AND VCHDATE BETWEEN TO_DATE('" + Convert.ToDateTime(((TextBox)gr.FindControl("sg1_t1")).Text.Trim()).ToString("dd/MM/yyyy") + "','DD/MM/YYYY') AND TO_DATE('" + Convert.ToDateTime(((TextBox)gr.FindControl("sg1_t2")).Text.Trim()).ToString("dd/MM/yyyy") + "','DD/MM/YYYY') AND TRIM(NVL(AUDT_BY,'-'))='-' ");
                            }
                        }

                        fgen.save_Mailbox2(frm_qstr, frm_cocd, frm_formID, frm_mbr, lblheader.Text + " # " + txtvchnum.Text + " " + txtvchdate.Text.Trim(), frm_uname, edmode.Value);

                        fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                        hffield.Value = "SAVED";
                        ViewState["sg1"] = null;
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


    }
    //------------------------------------------------------------------------------------
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
        if (sg1_dr == null) create_tab();
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

        sg1_dr["sg1_t1"] = 0;
        sg1_dr["sg1_t2"] = 0;
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
            for (int sg1r = 0; sg1r < sg1.Rows.Count; sg1r++)
            {
                for (int j = 0; j < sg1.Columns.Count; j++)
                {
                    sg1.Rows[sg1r].Cells[j].ToolTip = sg1.Rows[sg1r].Cells[j].Text;
                    if (sg1.Rows[sg1r].Cells[j].Text.Trim().Length > 50)
                    {
                        sg1.Rows[sg1r].Cells[j].Text = sg1.Rows[sg1r].Cells[j].Text.Substring(0, 50);
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
                    fgen.Fn_open_sseek("Select Account", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Account", frm_qstr);

                }
                break;
            case "SG1_ROW_ADD2":
                if (index < sg1.Rows.Count - 1)
                {
                    hf1.Value = index.ToString();
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                    //----------------------------
                    hffield.Value = "SG1_ROW_ADD2_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Account", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD2";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Account", frm_qstr);
                }
                break;
            case "SG1_ROW_INV":
                hf1.Value = index.ToString();
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
                //----------------------------
                if (index == sg1.Rows.Count - 2)
                {
                    hffield.Value = "SG1_ROW_INV";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Invoice", frm_qstr);

                }
                else
                {
                    hffield.Value = "SG1_ROW_INV_E";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Invoice", frm_qstr);
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

        if (txtvchnum.Text == "-")
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
    //------------------------------------------------------------------------------------
    protected void btnlbl4_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);  // BY MADHVI ON 23 JULY 2018
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl10_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl11_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl12_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl13_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl14_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl15_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl16_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl17_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl18_Click(object sender, ImageClickEventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnlbl19_Click(object sender, ImageClickEventArgs e)
    {

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
        hffield.Value = "TICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Deptt ", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        string vardate;
        string my_nars;

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        //grid saving
        int srno = 0;
        for (i = 0; i <= sg1.Rows.Count - 1; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Length > 1)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["BRANCHCD"] = frm_mbr;
                oporow["TYPE"] = frm_vty;
                oporow["vchnum"] = frm_vnum;
                oporow["vchdate"] = txtvchdate.Text.Trim();
                oporow["srno"] = srno;

                oporow["ICODE"] = sg1.Rows[i].Cells[13].Text.Trim();
                oporow["TITLE"] = sg1.Rows[i].Cells[14].Text.Trim();

                oporow["OMIN"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();
                oporow["OMAX"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim();

                oporow["COL1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim();
                oporow["COL2"] = txtRmk.Text.Trim();

                if (edmode.Value == "Y")
                {
                    oporow["ent_by"] = ViewState["entby"];
                    oporow["ent_Dt"] = ViewState["entdt"];

                    oporow["REL_BY"] = frm_uname;
                    oporow["REL_DT"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                }
                else
                {
                    oporow["ent_by"] = frm_uname;
                    oporow["ent_dt"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                    oporow["REL_BY"] = "-";
                    oporow["REL_DT"] = Convert.ToDateTime(vardate).ToString("dd/MM/yyyy");
                }
                oDS.Tables[0].Rows.Add(oporow);
                srno++;
            }
        }


    }
    //------------------------------------------------------------------------------------
    void save_fun2()
    {

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
            case "F70122C":
                SQuery = "SELECT type1 AS FSTR,name as NAME,type1 AS CODE FROM TYPE WHERE ID='V' AND TYPE1 in ('3Z') ORDER BY TYPE1 ";
                lbl4.Text = "Account";
                break;

            default:
                SQuery = "SELECT type1 AS FSTR,name as NAME,type1 AS CODE FROM TYPE WHERE ID='V' AND TYPE1 in ('30','37','38') ORDER BY TYPE1 ";
                break;

        }
    }
    //------------------------------------------------------------------------------------   
    protected void sg1_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes["onkeydown"] = "if (event.keyCode != 13) { javascript:return SelectSibling(event); }";
            //e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }
    protected void btnGridPop_Click(object sender, EventArgs e)
    {
        {
            col1 = hf1.Value.Replace("ContentPlaceHolder1_sg1_sg1_", "");
            hf1.Value = col1.Split('_')[1];
            hf2.Value = "sg1_" + col1.Split('_')[0];
        }
        hffield.Value = hf2.Value;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select F.O. / W.O. Number", frm_qstr);
    }
    void editFunction(string fstr)
    {
        col1 = fstr;
        #region Edit Start
        clearctrl();
        set_Val();
        SQuery = "Select a.*,nvl(a.ent_by,'-') as endt_by,to_char(a.ent_Date,'dd/mm/yyyy') as entd_dt,to_char(a.edt_Date,'dd/mm/yyyy') as edtd_dt,'-' as app_by,b.aname,c.aname as rname from " + frm_tabname + " a,famst b,famst c where trim(a.acode)=trim(b.acode) and trim(a.rcode)=trim(c.acode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' ORDER BY A.SRNO";
        SQuery = "Select trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.*,nvl(a.ent_by,'-') as endt_by,to_char(a.ent_Date,'dd/mm/yyyy') as entd_dt,to_char(a.edt_Date,'dd/mm/yyyy') as edtd_dt,'-' as app_by,b.aname from " + frm_tabname + " a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + col1 + "' ORDER BY A.SRNO";

        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        if (dt.Rows.Count > 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", dt.Rows[0]["fstr"].ToString().Trim());
            ViewState["fstr"] = dt.Rows[0]["fstr"].ToString().Trim();
            txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
            txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
            lbl1a.Text = frm_vty;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_OLD_DATE", txtvchdate.Text);

            txtlbl2.Text = dt.Rows[i]["endt_by"].ToString().Trim();
            txtlbl3.Text = dt.Rows[i]["entd_Dt"].ToString().Trim();

            ViewState["entby"] = dt.Rows[0]["endt_by"].ToString();
            ViewState["entdt"] = dt.Rows[0]["entd_dt"].ToString();


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
                sg1_dr["sg1_f1"] = dt.Rows[i]["acode"].ToString().Trim();
                sg1_dr["sg1_f2"] = dt.Rows[i]["aname"].ToString().Trim();
                sg1_dr["sg1_f3"] = "-";

                sg1_dr["sg1_f4"] = dt.Rows[i]["rcode"].ToString().Trim();
                sg1_dr["sg1_f5"] = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ANAME FROM FAMST WHERE TRIM(aCODE)='" + dt.Rows[i]["rcode"].ToString().Trim() + "'", "ANAME");

                sg1_dr["sg1_t1"] = dt.Rows[i]["dramt"].ToString().Trim();
                sg1_dr["sg1_t2"] = dt.Rows[i]["cramt"].ToString().Trim();
                sg1_dr["sg1_t3"] = dt.Rows[i]["invno"].ToString().Trim();
                sg1_dr["sg1_t4"] = Convert.ToDateTime(fgen.make_def_Date(dt.Rows[i]["invdate"].ToString().Trim(), vardate)).ToString("dd/MM/yyyy");
                sg1_dr["sg1_t5"] = dt.Rows[i]["naration"].ToString().Trim();

                //if (dt.Rows[i]["delv_item"].ToString().Trim().Length == 10)
                //{
                //    sg1_dr["sg1_t6"] = Convert.ToDateTime(dt.Rows[i]["delv_item"].ToString().Trim()).ToString("yyy-MM-dd"); // ADD Convert.ToDateTime IN THE LINE SO THAT WHEN DATE IS SAVED FROM MAIN IT WILL SHOW IN THE WEB BY MADHVI ON 23 JULY 2018
                //}
                //sg1_dr["sg1_t7"] = dt.Rows[i]["psize"].ToString().Trim();
                sg1_dt.Rows.Add(sg1_dr);
            }

            sg1_add_blankrows();
            ViewState["sg1"] = sg1_dt;
            sg1.DataSource = sg1_dt;
            sg1.DataBind();
            dt.Dispose(); sg1_dt.Dispose();
            ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
            fgen.EnableForm(this.Controls);
            disablectrl();
            setColHeadings();
            edmode.Value = "Y";
        }
        #endregion
    }
}