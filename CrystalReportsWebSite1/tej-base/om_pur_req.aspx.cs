using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_pur_req : System.Web.UI.Page
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
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
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
                    vardate = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VARDATE");
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
        doc_nf.Value = "ordno";
        doc_df.Value = "orddt";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = "pomas";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "60");

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
                //SQuery = "SELECT ID,TEXT,ID AS CODE FROM FIN_MSYS WHERE NVL(TRIM(WEB_ACTION),'-')!='-' ORDER BY ID  ";
                SQuery = "SELECT 'MT','Purchase (Materials)' as PR_Type,'MT' AS CODE FROM dual union all SELECT 'JW','Job Work' as PR_Type,'JW' AS CODE FROM dual ";
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
                if (frm_cocd == "MULT") col1 = "";
                if (doc_addl.Value == "Y")
                {
                    SQuery = "SELECT Icode AS FSTR,Iname AS Item_Name,Cpartno,Cdrgno,unit,hscode,maker,ent_by,Icode FROM Item where branchcd!='DD' and length(Trim(nvl(deac_by,'-')))<2  and length(Trim(icode))>4 " + col1 + " and (case when length(nvl(showinbr,'-'))>1 then instr(nvl(showinbr,'-'),'" + frm_mbr + "') else 1 end)>0 ORDER BY Iname ";
                }
                else
                {
                    SQuery = "SELECT Icode AS FSTR,Iname AS Item_Name,Cpartno,Cdrgno,unit,hscode,maker,ent_by,Icode FROM Item where branchcd!='DD' and length(Trim(nvl(deac_by,'-')))<2  and length(Trim(icode))>4 " + col1 + " and substr(icode,1,1)!='9' and  (case when length(nvl(showinbr,'-'))>1 then instr(nvl(showinbr,'-'),'" + frm_mbr + "') else 1 end)>0 ORDER BY Iname ";
                }
                break;
            case "SG3_ROW_ADD":
            case "SG3_ROW_ADD_E":
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
                if (frm_cocd == "MULT") col1 = "'-'";

                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(Trim(icode))>4 and icode like '9%' and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname  ";

                SQuery = "SELECT Icode,Iname,Cpartno AS Part_no,Cdrgno AS Drg_no,Unit,Icode as ERP_Code FROM Item WHERE length(Trim(icode))>4 and icode like '9%' and trim(icode) not in (" + col1 + ") and length(Trim(nvl(deac_by,'-')))<=1 ORDER BY Iname  ";
                break;
            case "sg1_t2":
                SQuery = "SELECT trim(a.ordno) fstr,B.aname as customer,a.ordno,to_Char(a.orddt,'dd/mm/yyyy') as orddt,a.icode as erpcode,c.iname as product from somas a,famst b, item c where trim(a.acode)=trim(b.acodE) and trim(a.icode)=trim(C.icodE) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.orddt " + DateRange + " order by a.ordno,orddt";
                break;
            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD" || btnval == "Atch_E")
                    SQuery = "select distinct trim(A.ordno)||to_Char(a.orddt,'dd/mm/yyyy') as fstr,a.ordno as PR_no,to_char(a.orddt,'dd/mm/yyyy') as PR_Dt,b.Name as Deptt,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,a.Chk_by,a.app_by,(case when nvl(a.pflag,0)=0 then 'Closed' else 'Active' end) as PR_Stat,to_Char(a.orddt,'yyyymmdd') as vdd from " + frm_tabname + " a,type b where trim(A.acode)=trim(B.type1) and b.id='M' and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and orddt " + DateRange + " order by vdd desc,a.ordno desc";
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
            col1 = "";
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT Type1||'~'||NAME AS Deptt,Type1 AS CODE FROM type where id='M' and trim(Type1) in (select trim(erpdeptt) as fstr from EVAS WHERE USERNAME='" + frm_uname + "' ) ", "Deptt");
            if (col1.Length > 5)
            {
                txtlbl7.Text = col1.Split('~')[0];
                txtlbl7a.Text = col1.Split('~')[1];
            }
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



        string chk_po_made;
        chk_po_made = fgen.seek_iname(frm_qstr, frm_cocd, "select ordno||' Dt.'||to_char(orddt,'dd/mm/yyyy') As fstr from pomas where branchcd='" + frm_mbr + "' and type like '5%' and trim(pr_no)||to_char(pr_Dt,'dd/mm/yyyy')='" + txtvchnum.Text + orig_vchdt + "'", "fstr");
        if (chk_po_made.Length > 6)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Purchase Order no ." + chk_po_made + " Already Made Against This P.R. , Editing is Not Allowed !!");
            return;
        }

        chk_rights = fgen.Fn_chk_can_add(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        if (chk_rights == "N")
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Save Entry For This Form !!");
            return;
        }

        string chk_freeze = "";
        chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1011", txtvchdate.Text.Trim());
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

        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        {
            fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only");
            txtvchdate.Focus();
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

        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;

        if (txtlbl4.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl4.Text;
        }

        if (txtlbl7.Text.Trim().Length < 2)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl7.Text;
        }


        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }

        string wo_reqd = "";
        wo_reqd = fgen.seek_iname(frm_qstr, frm_cocd, "select upper(enable_yn) as fstr from stock where id='M011'", "fstr");

        string lastDt = "";
        int i;
        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text) <= 0)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Quantity Not Filled Correctly at Line " + (i + 1) + "  !!");

                i = sg1.Rows.Count;
                return;
            }

            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Length < 2 && wo_reqd == "Y")
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , W.O. No. Not Filled Correctly at Line " + (i + 1) + "  !!");

                i = sg1.Rows.Count;
                return;
            }

            if (lastDt == "")
                lastDt = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text;
            if (lastDt != "")
            {
                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2)
                    ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text = lastDt;
            }
            if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2 && ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Length < 10)
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Date Not Filled Correctly at Line " + (i + 1) + "  !!");

                i = sg1.Rows.Count;
                return;
            }
            else
            {
                string curr_dt;
                string reqd_bydt;
                if (sg1.Rows[i].Cells[13].Text.Trim().Length > 2)
                {
                    curr_dt = Convert.ToDateTime(txtvchdate.Text).ToString("dd/MM/yyyy");
                    reqd_bydt = Convert.ToDateTime(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text).ToString("dd/MM/yyyy");

                    if (Convert.ToDateTime(curr_dt) > Convert.ToDateTime(reqd_bydt))
                    {
                        Checked_ok = "N";
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Required by Date Cannot be Less Than Current Date, See line No. " + (i + 1) + "  !!");
                        i = sg1.Rows.Count;
                        return;
                    }
                }
            }
        }
        //-----------------------------------------------------------------------

        string last_entdt;
        //checks

        if (edmode.Value == "Y")
        {
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  and orddt " + DateRange + " and ordno||to_char(orddt,'dd/mm/yyyy')!='" + txtvchnum.Text + orig_vchdt + "' and orddt<=to_DaTE('" + orig_vchdt + "','dd/mm/yyyy') order by orddt desc", "ldt");
        }
        else
        {
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(max(" + doc_df.Value + "),'dd/mm/yyyy') as ldt from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "'  and orddt " + DateRange + " and ordno||to_char(orddt,'dd/mm/yyyy')!='" + txtvchnum.Text + txtvchdate.Text + "' order by orddt desc", "ldt");
        }

        if (last_entdt == "0")
        { }
        else
        {
            if (Convert.ToDateTime(last_entdt) > Convert.ToDateTime(txtvchdate.Text.ToString()))
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Last " + lblheader.Text + " Entry Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + ",Please Check !!");
                return;

            }
        }
        last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select to_char(sysdate,'dd/mm/yyyy') as ldt from dual", "ldt");
        if (Convert.ToDateTime(txtvchdate.Text.ToString()) > Convert.ToDateTime(last_entdt))
        {
            Checked_ok = "N";
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Server Date " + last_entdt + " , This " + lblheader.Text + " Entry Date " + txtvchdate.Text.ToString() + " ,Please Check !!");
            return;

        }


        //-----------------------------------------------------------------------

        checkGridQty();

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }

        Checked_ok = "";
        if (checkPurchBudg() == "Y")
        {
            fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
            btnsave.Disabled = true;
        }
        else fgen.msg("-", "AMSG", Checked_ok);
    }

    string checkPurchBudg()
    {
        string resultPur = "Y";
        DataTable dtQty = new DataTable();
        dtQty.Columns.Add(new DataColumn("icode", typeof(string)));
        dtQty.Columns.Add(new DataColumn("qty", typeof(double)));
        dtQty.Columns.Add(new DataColumn("rate", typeof(double)));
        DataRow drQty = null;
        foreach (GridViewRow gr in sg1.Rows)
        {
            if (gr.Cells[13].Text.ToString().Length > 4)
            {
                drQty = dtQty.NewRow();
                drQty["icode"] = gr.Cells[13].Text.ToString().Trim().Substring(0, 4);
                drQty["qty"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t1")).Text.ToString().Trim());
                drQty["rate"] = fgen.make_double(((TextBox)gr.FindControl("sg1_t5")).Text.ToString().Trim());
                dtQty.Rows.Add(drQty);
            }
        }
        DataTable dtNeU = new DataTable();
        dtNeU = dtQty.Clone();
        DataRow drNeU;
        DataView dv = new DataView(dtQty);
        dt4 = new DataTable();
        dt4 = dv.ToTable(true, "ICODE");
        double dbr = 0;
        for (int i = 0; i < dt4.Rows.Count; i++)
        {
            drNeU = dtNeU.NewRow();
            drNeU["ICODE"] = dt4.Rows[i]["ICODE"].ToString().Trim();
            dbr = Convert.ToDouble(dtQty.Compute("SUM(QTY)*SUM(RATE)", "ICODE='" + dt4.Rows[i]["ICODE"].ToString().Trim() + "'"));
            drNeU["QTY"] = dbr;
            dtNeU.Rows.Add(drNeU);
        }

        int myMnth = DateTime.Now.Month;
        if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_CLIENT_GRP") != "SG_TYPE") myMnth = myMnth - 3;
        dt = new DataTable();
        dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT TRIM(ICODE) AS ICODE,SUM(day" + myMnth + ") AS QTY FROM SCHEDULE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='PB' AND TRIM(ACODE)='" + txtlbl7.Text.Trim() + "' GROUP BY TRIM(ICODE) ");
        foreach (DataRow drx in dtNeU.Rows)
        {
            col1 = fgen.seek_iname_dt(dt, "ICODE='" + drx["ICODE"].ToString().Trim() + "'", "QTY");
            if (col1.toDouble() > 0)
            {
                if (drx["QTY"].ToString().toDouble() > col1.toDouble())
                {
                    resultPur = "N";
                    Checked_ok = "'13'Value is exceeding from the Purchase Budget for Your Department.'13'Allowed Budget value is : " + col1 + " for subgroup Code : " + drx["ICODE"].ToString().Trim() + ".'13'Value in P.R. for Same group code : " + drx["QTY"] + " ";
                    break;
                }
            }
        }


        return resultPur;
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
        //SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + DateRange + " and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
        //fgen.Fn_FillChart(frm_cocd, frm_qstr, "Testing Chart", "line", "Main Heading", "Sub Heading", SQuery, "");
        hffield.Value = "Print";
        make_qry_4_popup();
        //   fgen.Fn_open_mseek("Select Type for Print", frm_qstr); // COMMENTED BY MADHVI ON 28 JULY 2018
        fgen.Fn_open_mseek("Select " + lblheader.Text, frm_qstr); // BY MADHVI ON 28 JULY 2018
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        vty = "60";
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;

        //frm_vnum = fgen.next_number(frm_qstr, frm_cocd, frm_tabname, frm_mbr, frm_vty, doc_nf.Value, doc_df.Value, DateRange,frm_CDT1,frm_uname,"Y" );

        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and orddt " + DateRange + " ", 6, "VCH");
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
                string chk_po_made;
                chk_po_made = fgen.seek_iname(frm_qstr, frm_cocd, "select ordno||' Dt.'||to_char(orddt,'dd/mm/yyyy') As fstr from pomas where branchcd||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' and type like '5%'", "fstr");
                if (chk_po_made.Length > 6)
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Purchase Order no ." + chk_po_made + " Already Made Against This P.R. , Deletion is Not Allowed !!");
                    return;
                }
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
                    SQuery = "Select a.*,b.name,c.iname,c.cpartno as icpartno,c.cdrgno as icdrgno,c.unit as iunit,to_char(a.ent_Dt,'dd/mm/yyyy') as pent_dt,to_char(a.chk_Dt,'dd/mm/yyyy') as chkd_dt,to_char(a.app_Dt,'dd/mm/yyyy') as papp_dt from " + frm_tabname + " a,type b,item c where trim(a.acode)=trim(b.type1) and b.id='M' and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(a.ordno)||to_Char(a.orddt,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["ordno"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["orddt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        lbl1a.Text = frm_vty;
                        fgenMV.Fn_Set_Mvar(frm_qstr, "U_OLD_DATE", txtvchdate.Text);

                        if (fgen.make_double(frm_ulvl) > 2 && dt.Rows[i]["app_by"].ToString().Trim() != "-")
                        {
                            fgen.msg("-", "AMSG", "Approved Order Cannot be Edited, Contact HOD/Admin");
                            return;
                        }
                        txtlbl2.Text = dt.Rows[i]["ent_by"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["pent_Dt"].ToString().Trim();
                        txtlbl4.Text = dt.Rows[i]["TAX"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[i]["kindattn"].ToString().Trim();
                        txtlbl5.Text = dt.Rows[i]["chk_by"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["chkd_Dt"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["name"].ToString().Trim();
                        txtlbl8.Text = dt.Rows[i]["app_by"].ToString().Trim();
                        txtlbl9.Text = dt.Rows[i]["papp_Dt"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["remark"].ToString().Trim();

                        txtIndRef.Text = dt.Rows[0]["pordno"].ToString().Trim();

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
                            sg1_dr["sg1_f1"] = dt.Rows[i]["icode"].ToString().Trim();
                            sg1_dr["sg1_f2"] = dt.Rows[i]["iname"].ToString().Trim();
                            sg1_dr["sg1_f3"] = dt.Rows[i]["icpartno"].ToString().Trim();
                            sg1_dr["sg1_f4"] = dt.Rows[i]["icdrgno"].ToString().Trim();
                            sg1_dr["sg1_f5"] = dt.Rows[i]["iunit"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["qtyord"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["desc_"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["splrmk"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["DOC_THR"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["prate"].ToString().Trim();
                            if (dt.Rows[i]["delv_item"].ToString().Trim().Length == 10)
                            {
                                sg1_dr["sg1_t6"] = Convert.ToDateTime(dt.Rows[i]["delv_item"].ToString().Trim()).ToString("yyy-MM-dd"); // ADD Convert.ToDateTime IN THE LINE SO THAT WHEN DATE IS SAVED FROM MAIN IT WILL SHOW IN THE WEB BY MADHVI ON 23 JULY 2018
                            }
                            sg1_dr["sg1_t7"] = dt.Rows[i]["psize"].ToString().Trim();
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
                    }
                    #endregion
                    break;
                case "SAVED":
                    hffield.Value = "Print_E";
                    if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
                        btnhideF_Click(sender, e);
                    break;
                case "Atch_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    fgen.open_fileUploadPopup("Upload File for " + lblheader.Text, frm_qstr);
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F1003");
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
                            sg1_dt.Rows.Add(sg1_dr);
                        }

                        dt = new DataTable();
                        hscode = ""; msg = "";
                        if (col1.Length > 8) SQuery = "select * from item where trim(icode) in (" + col1 + ") order by iname";
                        else SQuery = "select * from item where trim(icode)='" + col1 + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                        for (int d = 0; d < dt.Rows.Count; d++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;
                            hscode = fgen.seek_iname_dt(dt, "icode='" + dt.Rows[d]["icode"].ToString().Trim() + "'", "hscode");
                            if (hscode.Trim().Length <= 1)
                            {
                                msg += "," + dt.Rows[d]["icode"].ToString().Trim();
                            }
                            if (hscode.Trim().Length > 1)
                            {
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
                                if (frm_cocd == "MULT")
                                {
                                    sg1_dr["sg1_f4"] = fgen.make_double(fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, dt.Rows[d]["icode"].ToString().Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'"));
                                }
                                sg1_dr["sg1_f5"] = dt.Rows[d]["unit"].ToString().Trim();
                                //fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL6").ToString().Trim().Replace("&amp", "");
                                sg1_dr["sg1_t1"] = "";
                                sg1_dr["sg1_t2"] = "";
                                sg1_dr["sg1_t3"] = "";
                                sg1_dr["sg1_t4"] = dt.Rows[d]["maker"].ToString().Trim();
                                sg1_dr["sg1_t5"] = dt.Rows[d]["irate"].ToString().Trim();
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
                    }
                    sg1_add_blankrows();
                    ViewState["sg1"] = sg1_dt;
                    sg1.DataSource = sg1_dt;
                    sg1.DataBind();
                    sg1_dt.Dispose();
                    ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                    if (msg.Length > 5)
                    {
                        fgen.msg("-", "AMSG", "HS Code For Selected Item " + msg.TrimStart(',') + " Not Linked. '13' Hence Not Filled in the Grid.");
                    }
                    #endregion
                    setColHeadings();
                    break;
                case "SG1_ROW_ADD_E":
                    if (col1.Length <= 0) return;
                    //********* Saving in Hidden Field
                    hscode = ""; msg = "";
                    hscode = fgen.seek_iname(frm_qstr, frm_cocd, "select hscode from item where icode='" + col1 + "'", "hscode");
                    if (hscode.Trim().Length <= 1)
                    {
                        msg += "," + col1;
                    }
                    if (hscode.Trim().Length > 1)
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
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t5")).Text = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT IRATE FROM ITEM WHERE ICODE='" + col1 + "'", "IRATE");
                    }
                    if (msg.Length > 5)
                    {
                        fgen.msg("-", "AMSG", "HS Code For Selected Item " + msg.TrimStart(',') + " Not Linked. '13' Hence Not Filled in the Grid.");
                    }
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

        if (hffield.Value == "List" || hffield.Value == "PENDPR" || hffield.Value == "PENDPO" || hffield.Value == "STKINH")
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
                    SQuery = fgen.makeRepQuery(frm_qstr, frm_cocd, "F15126", "branchcd='" + frm_mbr + "'", "a.type='60' and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'", PrdRange);
                    headerN = "Purchase Request Checklist for the Period " + fromdt + " to " + todt;
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

                    mq1 = "select  '-' as fstr,'-' as gstr,c.iname as Item_Name,sum(a.poq) as PO_Qty,sum(a.rcvq) as MRR_Qty,sum(a.poq)-sum(a.rcvq) as Bal_Qty,sum(a.rej_Qty) as Rejn_Qty,(Case when sum(a.poq)>0 then round(((sum(a.poq)-sum(a.rcvq))/sum(a.poq))*100,2) else 0 end) as bal_per,round(sysdate-a.orddt,0) as Pend_Days,c.unit,a.branchcd,a.ordno,a.orddt,a.type,trim(a.icode) as ERP_Code,trim(a.acode) as Act_code,max(a.pflag)as pflag,max(a.del_Sch) as wo_no from (Select branchcd,pflag,ordno,orddt,acode,icode,qtyord as poq,0 as rcvq,0 as rej_Qty,type,del_Sch from pomas where branchcd='" + frm_mbr + "' and substr(type,1,1) ='5' and orddt " + xprd1 + " and  acode like '" + party_cd + "%' and icode like '" + part_cd + "%' union all Select branchcd,null as pflag,ponum,podate,acode,icode,0 as prq,iqtyin as poq,rej_rw,potype,null as del_Sch from ivoucher where branchcd='" + frm_mbr + "' and substr(type,1,1)='0' and potype like '5%' and vchdate " + xprd1 + " and store in ('Y','N')  ";
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
            }
            fgen.Fn_DrillReport(headerN, frm_qstr);
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

                        //fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2"), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3"), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                        //fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2") + "");

                        if (edmode.Value == "Y")
                        {

                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                            fgen.msg("-", "CMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully'13'Do you want to see the Print Preview ?");

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

                        #region Email Sending Function
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        //html started                            
                        sb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
                        sb.Append("<h3>" + fgenCO.chk_co(frm_cocd) + "</h3>");
                        sb.Append("<br>Dear Sir/Mam,<br> This is to inform you that the following " + lblheader.Text + " has been saved by " + frm_uname + ".<br><br>");
                        sb.Append("<br>" + lbl1.Text.Trim() + " - Date : " + frm_vnum + " - " + txtvchdate.Text + "");                        
                        //table structure
                        sb.Append("<table border=1 cellspacing=1 cellpadding=1 style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; color: #474646'>");

                        sb.Append("<tr style='color: #FFFFFF; background-color: #0099FF; font-weight: 700; font-family: Arial, Helvetica, sans-serif'>" +
                        "<td><b>Srno</b></td><td><b>Item Code</b></td><td><b>Item Name</b></td><td><b>PR Quantity</b></td><td><b>Description</b></td>");
                        i = 0;
                        foreach (GridViewRow gr in sg1.Rows)
                        {
                            sb.Append("<tr>");
                            sb.Append("<td>");
                            sb.Append(i + 1);
                            sb.Append("</td>");
                            sb.Append("<td>");
                            sb.Append(gr.Cells[13].Text);
                            sb.Append("</td>");
                            sb.Append("<td>");
                            sb.Append(gr.Cells[14].Text);
                            sb.Append("</td>");
                            sb.Append("<td>");
                            sb.Append(((TextBox)gr.FindControl("sg1_t1")).Text.Trim() + " " + gr.Cells[17].Text);
                            sb.Append("</td>");
                            sb.Append("<td>");
                            sb.Append(((TextBox)gr.FindControl("sg1_t2")).Text.Trim());
                            sb.Append("</td>");                            
                            sb.Append("</tr>");
                            i++;
                        }
                        sb.Append("</table></br></br>");

                        sb.Append("Thanks & Regards");
                        sb.Append("<h5>Note: This is an Auto generated Mail from Tejaxo ERP. The above details are to the best of information <br> and data available to the ERP System. For any discrepancy/ clarification kindly get in touch with the concerned official. </h5>");
                        sb.Append("</body></html>");

                        //send mail
                        string subj = "";
                        if (edmode.Value == "Y") subj = "Edited : ";
                        else subj = "New Entry : ";
                        fgen.send_Activity_mail(frm_qstr, frm_cocd, "Tejaxo ERP", frm_formID, subj + lblheader.Text + " # " + frm_vnum, sb.ToString(), frm_uname);
                        sb.Clear();
                        #endregion

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
                    fgen.Fn_open_sseek("Select Item", frm_qstr);
                }
                else
                {
                    hffield.Value = "SG1_ROW_ADD";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_FROM", hffield.Value);
                    make_qry_4_popup();
                    fgen.Fn_open_mseek("Select Items", frm_qstr); // CHANGE ITEM TO ITEMS BY MADHVI ON 23 JULY 2018
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
        // fgen.Fn_open_sseek("Select Supplier ", frm_qstr);  // COMMENTED BY MADHVI ON 23 JULY 2018
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
                oporow["ordno"] = frm_vnum;
                oporow["orddt"] = txtvchdate.Text.Trim();
                oporow["SRNO"] = i;
                oporow["TAX"] = txtlbl4.Text;
                oporow["kindattn"] = txtlbl4a.Text;
                oporow["acode"] = txtlbl7.Text;
                oporow["icode"] = sg1.Rows[i].Cells[13].Text.Trim();
                oporow["qtyord"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
                oporow["desc_"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text;
                oporow["splrmk"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text;
                oporow["DOC_THR"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text;
                double fd_rate;
                fd_rate = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text);
                if (fd_rate == 0)
                {
                    fd_rate = fgen.make_double(fgen.seek_iname(frm_qstr, frm_cocd, "select (Case when nvl(iqd,0)=0 then irate else iqd end) as irate from item where trim(upper(icode))=Trim('" + sg1.Rows[i].Cells[13].Text.Trim() + "')", "irate"));
                }
                oporow["prate"] = fd_rate;

                // oporow["delv_item"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text; // ORIGINAL .... COMMENTED BY MADHVI ON 23 JULY 2018 TO SOLVE DATE FORMAT ISSUE
                // oporow["delv_term"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text; // ORIGINAL .... COMMENTED BY MADHVI ON 23 JULY 2018 TO SOLVE DATE FORMAT ISSUE

                oporow["delv_item"] = Convert.ToDateTime(fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text, vardate).ToString()).ToString("dd/MM/yyyy"); // WRITTEN BY MADHVI ON 23 JULY 2018
                oporow["delv_term"] = Convert.ToDateTime(fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text, vardate).ToString()).ToString("dd/MM/yyyy"); // WRITTEN BY MADHVI ON 23 JULY 2018

                oporow["psize"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text;

                oporow["unit"] = sg1.Rows[i].Cells[17].Text.Trim(); // EARLIER IT WAS SAVING '-' SO CHANGE BY MADHVI ON 23 JULY 2018

                oporow["pordno"] = txtIndRef.Text.Trim().ToUpper();

                oporow["porddt"] = txtvchdate.Text.Trim();
                oporow["bank"] = txtlbl7a.Text;
                oporow["mode_tpt"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text;
                oporow["tr_insur"] = "-";
                oporow["freight"] = "-";
                oporow["remark"] = txtrmk.Text.Trim();
                oporow["prefsource"] = "-";
                oporow["app_by"] = (frm_cocd == "MULT") ? frm_uname : "-";
                oporow["app_dt"] = txtvchdate.Text.Trim();
                oporow["qtysupp"] = fgen.make_double(fgen.seek_istock(frm_qstr, frm_cocd, frm_mbr, sg1.Rows[i].Cells[13].Text.Trim(), txtvchdate.Text.Trim(), false, "closing_stk", " and type||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy') !='" + frm_vty + txtvchnum.Text.Trim() + txtvchdate.Text.Trim() + "'"));
                oporow["qtybal"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text); // EARLIER IT WAS SAVING 0 SO CHANGE BY MADHVI ON 23 JULY 2018
                oporow["pflag"] = 1;
                oporow["pdisc"] = 0;
                oporow["ptax"] = 0;
                oporow["pexc"] = 0;
                oporow["pamt"] = 0;
                oporow["issue_no"] = 0;
                oporow["invno"] = "-";
                oporow["invdate"] = txtvchdate.Text.Trim();
                oporow["Delivery"] = 0;
                oporow["DEL_MTH"] = 0;
                oporow["DEL_wk"] = 0;

                // oporow["del_Date"] = fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text, vardate); // ORIGINAL .... COMMENTED BY MADHVI ON 23 JULY 2018 TO SOLVE DATE FORMAT ISSUE
                oporow["del_Date"] = Convert.ToDateTime(fgen.make_def_Date(((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text, vardate).ToString()).ToString("dd/MM/yyyy"); // WRITTEN BY MADHVI ON 23 JULY 2018
                oporow["inst"] = "-";
                oporow["term"] = "-";
                oporow["refdate"] = txtvchdate.Text.Trim();
                oporow["STORE_NO"] = "SA";

                string lastin = "";
                string lastpp = "";
                lastin = fgen.seek_iname(frm_qstr, frm_cocd, "select fstr from (select 'Last Ind No.'||ordno||' Dt.'||to_char(orddt,'dd/mm/yyyy')||' By '||trim(Bank)||' Qty='||qtyord||' '||Unit as fstr from pomas where branchcd='" + frm_mbr + "' and type='60' and trim(icode)='" + sg1.Rows[i].Cells[13].Text.Trim() + "' order by orddt desc) where rownum<2", "fstr");
                lastpp = fgen.seek_iname(frm_qstr, frm_cocd, "select fstr from (select 'Last Inw No.'||a.vchnum||' Dt.'||to_char(a.vchdate,'dd/mm/yyyy')||' From '||trim(b.aname)||' @'||irate as fstr from ivoucher a, famst b WHERE a.branchcd='" + frm_mbr + "' and substr(type,1,1)='0' and a.type not in ('08','09','0J') and trim(a.icode)='" + sg1.Rows[i].Cells[13].Text.Trim() + "' and TRIM(A.ACODE)=TRIM(B.ACODE) order by a.vchdate desc) where rownum<2", "fstr");
                oporow["desp_to"] = lastin + " | " + lastpp;

                oporow["packing"] = 0;
                oporow["payment"] = fgen.seek_iname(frm_qstr, frm_cocd, "select iname from item where length(Trim(icode))=4 and trim(icode)='" + sg1.Rows[i].Cells[13].Text.Substring(0, 4).Trim() + "'", "iname");
                oporow["stax"] = "-";
                oporow["exc"] = "-";
                oporow["iopr"] = "-";
                oporow["pr_no"] = "-";
                oporow["pr_Dt"] = txtvchdate.Text.Trim();
                oporow["amd_no"] = "-";
                oporow["del_Sch"] = "-";
                oporow["st31no"] = "-";
                oporow["vend_wt"] = 1; // EARLIER IT WAS SAVING 0 SO CHANGE BY MADHVI ON 23 JULY 2018
                oporow["wk1"] = 0;
                oporow["wk2"] = 0;
                oporow["wk3"] = 0;
                oporow["wk4"] = 0;
                oporow["pbasis"] = "-";
                if (edmode.Value == "Y")
                {
                    oporow["eNt_by"] = ViewState["entby"].ToString();
                    oporow["eNt_dt"] = ViewState["entdt"].ToString();
                    oporow["edt_by"] = frm_uname;
                    oporow["edt_dt"] = vardate;
                    oporow["pordno"] = ViewState["entby"].ToString(); // BY MADHVI ON 23 JULY 2018
                }
                else
                {
                    oporow["eNt_by"] = frm_uname;
                    oporow["eNt_dt"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["eDt_dt"] = vardate;
                    oporow["pordno"] = frm_uname; // BY MADHVI ON 23 JULY 2018
                }

                if (txtIndRef.Text.Trim().ToUpper().Length > 1)
                {
                    oporow["pordno"] = txtIndRef.Text.Trim().ToUpper();
                }



                oporow["app_dt"] = vardate;
                oporow["chk_by"] = "-";
                oporow["chk_dt"] = vardate;

                // BY MADHVI ON 23 JULY 2018 ---------------
                oporow["rate_cd"] = 0;
                oporow["amdtno"] = 0;
                oporow["o_qty"] = fgen.make_double(((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text);
                oporow["ciname"] = "-";
                oporow["cscode1"] = "-";
                oporow["ed_serv"] = "-";
                oporow["atch1"] = "-";
                oporow["pdiscamt2"] = 0;
                oporow["txb_frt"] = 0;
                oporow["atch2"] = "-";
                oporow["atch3"] = "-";
                //oporow["link_cd"] = "-";
                oporow["po_tolr"] = 0;

                oporow["PR_SRN"] = (i + 1).ToString().PadLeft(3, '0');
                // -----------------------------------------
                oDS.Tables[0].Rows.Add(oporow);
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
            case "F15101":
                SQuery = "SELECT '60' AS FSTR,'Purchase Request' as NAME,'60' AS CODE FROM dual";
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
        if (hf1.Value.Contains("sg1_t2_"))
        {
            hffield.Value = "sg1_t2";
            hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_sg1_t2_", "");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select F.O. / W.O. Number", frm_qstr);
        }
    }
    protected void btnAtch_ServerClick(object sender, EventArgs e)
    {
        hffield.Value = "Atch_E";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblheader.Text, frm_qstr);
    }
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
    //protected void Button4_Click(object sender, EventArgs e)
    //{
    //    hffield.Value = "List";
    //    fgen.Fn_open_Act_itm_prd("-", frm_qstr);
    //}
}