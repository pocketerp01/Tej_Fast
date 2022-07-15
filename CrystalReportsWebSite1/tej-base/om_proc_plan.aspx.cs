using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


public partial class om_proc_plan : System.Web.UI.Page
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

                doc_addl.Value = fgen.getOption(frm_qstr, frm_cocd, "W0091", "OPT_ENABLE").ToUpper();

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

                if (frm_ulvl != "0")
                {
                    ((TextBox)sg1.Rows[K].FindControl("sg1_t1")).ReadOnly = true;
                }

                if (((TextBox)sg1.Rows[K].FindControl("sg1_t1")).Text == "FLUTE")
                {
                    ((TextBox)sg1.Rows[K].FindControl("sg1_t8")).ReadOnly = true;
                    ((TextBox)sg1.Rows[K].FindControl("sg1_t9")).ReadOnly = true;
                }

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
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgen.SetHeadingCtrl(this.Controls, dtCol);
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnprint.Disabled = false; btnlist.Disabled = false; btnlbl101.Enabled = false; btnlblPrefMc.Enabled = false;
        btnlblFlute.Enabled = false; btnlblDie.Enabled = false; btnlblModel.Enabled = false;
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
        btnprint.Disabled = true; btnlist.Disabled = true; btnlbl4.Enabled = true; btnlbl7.Enabled = true;
        btnlbl101.Enabled = true; btnlblPrefMc.Enabled = true;
        btnlblFlute.Enabled = true; btnlblDie.Enabled = true; btnlblModel.Enabled = true;
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
        frm_tabname = "inspmst";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "70");
        frm_vty = "70";
        lbl1a.Text = frm_vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);

        lblBend.Visible = false;
        txtlblBend.Visible = false;

        if (frm_ulvl.toDouble() < 2) chkAppr.Visible = true;
        else chkAppr.Visible = false;
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
                string HO_ORD_SYS = "";
                string BR_STR = "";
                HO_ORD_SYS = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W1100'", "fstr");
                if (HO_ORD_SYS == "Y")
                {
                    BR_STR = "a.branchcd='00'";
                }
                else
                {
                    BR_STR = "a.branchcd='" + frm_mbr + "'";
                }

                SQuery = "SELECT distinct trim(A.acode) AS FSTR,b.Aname as Customer,b.addr1,b.Addr2,b.acode from somas a ,famst b where trim(A.acode)=trim(B.acode) and " + BR_STR + " and a.type like '4%' and length(Trim(nvl(b.deac_by,'-')))<=1 order by b.aname";

                break;
            case "TICODE":
                string pquery;

                string br_Cond = "";
                br_Cond = "branchcd!='DD'";
                if (doc_addl.Value == "Y")
                {
                    br_Cond = "branchcd='" + frm_mbr + "'";
                }

                pquery = "select trim(icode) as icode,sum(cnt) as tot from (select icode,1 as cnt from item where length(trim(nvl(deac_by,'-')))<=1 and length(trim(icode))>4 union all select distinct icode,-1 as cnt from inspmst where " + br_Cond + " and type='" + lbl1a.Text + "') group by trim(icode) having sum(cnt)>0 ";
                SQuery = "SELECT a.Icode AS FSTR,trim(b.Iname) as Item_name,a.Icode,b.Cpartno,b.Cdrgno,b.unit from (" + pquery + ")a ,Item b where trim(A.icode)=trim(B.icode) and length(trim(nvl(b.deac_by,'-')))<=1 and length(trim(b.icode))>4 and substr(b.icode,1,1) in ('7','8','9') order by b.iname";
                break;
            case "TPICODE":
                SQuery = "SELECT a.Icode AS FSTR,trim(a.Iname) as Item_name,a.Icode,a.Cpartno,a.Cdrgno,a.unit from Item a where length(trim(nvl(a.deac_by,'-')))<=1 and length(trim(a.icode))>4 and substr(a.icode,1,1) in ('7','8','9') order by a.iname";
                break;
            case "sg1_t1":
                SQuery = "SELECT a.Icode AS FSTR,trim(a.Iname) as Item_name,a.Icode,a.Cpartno,a.Cdrgno,a.unit from Item a where length(trim(nvl(a.deac_by,'-')))<=1 and length(trim(a.icode))>4 order by a.iname";
                break;
            case "sg1_t7":
                SQuery = "SELECT TYPE1 AS FSTR,NAME,LINENO AS HEIGHT,ACREF AS PER_EXTRA, TYPE1 AS CODE FROM TYPEGRP WHERE ID='FU' ORDER BY SRNO ";
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

            case "DIE":
                SQuery = "select distinct branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(cpartno) as fstr,cpartno as die_id,title as die_name,col1 as specs,vchnum as rec_no,to_char(vchdate,'dd/mm/yyyy') as rec_dt from inspmst where branchcd='" + frm_mbr + "' and type='60' ORDER BY DIE_ID";
                break;

            case "FLUTE":
                SQuery = "select trim(type1) as fstr,type1,name from typegrp where id='ZF' and branchcd='" + frm_mbr + "' order by name";
                break;

            case "MODEL":
                SQuery = "select trim(type1) as fstr,type1,name from type where id='@' and type1 like '2%' order by type1";
                break;

            case "PREFMAC":
                SQuery = "select trim(mchcode) as fstr,mchcode as machine_code,mchname as machine_name from pmaint where branchcd='" + frm_mbr + "' and type='10'  order by machine_code";
                break;

            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            case "Print_E":
                SQuery = "select distinct a.branchcd||a.type||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Proc_Plan_no,to_char(a.vchdate,'dd/mm/yyyy') as Proc_Plan_Dt,b.Iname as Item_Name,b.Cpartno as Part_no,b.Icode,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' order by vdd desc,a.vchnum desc";
                break;
            case "COLOR":
                SQuery = "SELECT NAME AS FSTR,NAME,TYPE1 AS CODE FROM TYPEGRP WHERE BRANCHCD!='DD' AND ID='J1' ORDER BY TYPE1 ";
                break;
            default:
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "COPY_OLD")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.Vchnum as Proc_Plan_no,to_char(a.vchdate,'dd/mm/yyyy') as Proc_Plan_Dt,b.Iname as Item_Name,b.Cpartno as Part_no,b.Icode,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a,item b where trim(A.icode)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' order by vdd desc,a.vchnum desc";
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


            frm_vty = "70";
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
            lblAppr.Text = "";
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
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' ", 6, "VCH");
        txtvchnum.Text = frm_vnum;
        txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);

        disablectrl();
        fgen.EnableForm(this.Controls);
        btnlbl4.Focus();

        sg1_dt = new DataTable();
        create_tab();
        int j;
        for (j = i; j < 60; j++)
        {
            sg1_add_blankrows();
        }


        sg1.DataSource = sg1_dt;
        sg1.DataBind();
        setColHeadings();
        ViewState["sg1"] = sg1_dt;
        //Popup asking for Copy from Older Data

        fgen.msg("-", "CMSG", "Do You Want to Use an Existing Process Plan to Make the New Process Plan ? '13'It Will make The Process Very Fast'13'It Will Save Your Time of Re-Entry");
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
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }

        string chk_freeze = "";
        chk_freeze = fgen.Fn_chk_doc_freeze(frm_qstr, frm_cocd, frm_mbr, "W1002", txtvchdate.Text.Trim());
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
        if (txtlbl4.Text.Trim().Length < 2)
        {
            Checked_ok = "N";
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Customer Not Filled Correctly !!");
            return;
        }
        string reqd_flds;
        reqd_flds = "";
        int reqd_nc;
        reqd_nc = 0;

        if (txtlbl4.Text.Trim().Length <= 0)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl4.Text;
        }

        if (fgen.make_double(txtlbl5.Text) <= 0)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl5.Text;
        }

        //if (fgen.make_double(txtlbl6.Text) <= 0)
        //{
        //    reqd_nc = reqd_nc + 1;
        //    reqd_flds = reqd_flds + " / " + lbl6.Text;
        //}


        if (fgen.make_double(txtlbl102.Text) <= 0)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl102.Text;
        }
        if (fgen.make_double(txtlbl103.Text) <= 0)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lbl103.Text;
        }
        if (txtlblFlute.Text.Trim().Length <= 0)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + lblFlute.Text;
        }
        if (fgen.make_double(txtlblPly.Text) <= 0)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Ply";
        }
        if (fgen.make_double(txtlblFinish.Text) <= 0)
        {
            reqd_nc = reqd_nc + 1;
            reqd_flds = reqd_flds + " / " + "Finish";
        }
        if (reqd_nc > 0)
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , " + reqd_nc + " Fields Require Input '13' Please fill " + reqd_flds);
            return;
        }
        if (txtlbl7.Text.Trim() == txtlbl101.Text.Trim())
        {
            fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Parent And Child Code Cannot be Same");
            return;
        }
        //if (frm_vty == "70")
        //{
        //    if (fgen.make_double(txtlbl5.Text) <= 0)
        //    {
        //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please fill No. of Ups Correctly !!");
        //        return;
        //    }
        //    if (fgen.make_double(txtlbl6.Text) <= 0)
        //    {
        //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please fill No. of Colors Correctly !!");
        //        return;
        //    }
        //    if (fgen.make_double(txtlbl102.Text) <= 0)
        //    {
        //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please fill Sheet Width Correctly !!");
        //        return;
        //    }
        //    if (fgen.make_double(txtlbl103.Text) <= 0)
        //    {
        //        fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please fill Sheet Length Correctly !!");
        //        return;
        //    }
        //}
        //if (fgen.make_double(txtPly.Text) == 0)
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill Correct No. Of Ply");
        //    return;
        //}
        //if (fgen.make_double(txtFinish.Text) == 0)
        //{
        //    fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Please Fill Correct Finish Size");
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
        hffield.Value = "Print";
        make_qry_4_popup();
        fgen.Fn_open_mseek("Select " + lblheader.Text + " Entry To Print", frm_qstr);
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
            else
            {
                btnlbl4.Focus();
            }
        }
        else if (hffield.Value == "FLEN")
        {
            txtlbl103.Text = Request.Cookies["REPLY"].Value.ToString().Trim();
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
                    //SQuery = "Select a.*,b.text from " + frm_tabname + " a left outer join FIN_MSYS b on trim(a.frm_name)=trim(b.id) where a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    SQuery = "Select b.iname,b.cpartno,b.cdrgno,a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt from " + frm_tabname + " a,item b where trim(A.icode)=trim(B.icode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl2.Text = dt.Rows[i]["col14"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[i]["col16"].ToString().Trim();
                        txtlbl5.Text = dt.Rows[i]["rejqty"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[i]["recalib"].ToString().Trim();
                        txtlbl4.Text = dt.Rows[i]["acode"].ToString().Trim();
                        txtlbl4a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select aname from famst where trim(upper(acode))=upper(Trim('" + txtlbl4.Text + "'))", "aname");
                        txtlbl7.Text = dt.Rows[i]["icode"].ToString().Trim();
                        txtlbl7a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select iname from item where trim(upper(icode))=upper(Trim('" + txtlbl7.Text + "'))", "iname");
                        txtlbl8.Text = dt.Rows[i]["cpartno"].ToString().Trim();
                        txtlbl9.Text = dt.Rows[i]["cdrgno"].ToString().Trim();
                        txtlbl101.Text = dt.Rows[i]["picode"].ToString().Trim();
                        txtlbl101a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select iname from item where trim(upper(icode))=upper(Trim('" + txtlbl101.Text + "'))", "iname");
                        txtlbl102.Text = dt.Rows[i]["btchdt"].ToString().Trim();
                        txtlbl103.Text = dt.Rows[i]["maintdt"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["title"].ToString().Trim();
                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = i + 1;
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
                            sg1_dr["sg1_f1"] = "-";
                            sg1_dr["sg1_f2"] = "-";
                            sg1_dr["sg1_f3"] = "-";
                            sg1_dr["sg1_f4"] = "-";
                            sg1_dr["sg1_f5"] = "-";
                            sg1_dr["sg1_t1"] = dt.Rows[i]["col1"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["col2"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["col5"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["col6"].ToString().Trim();
                            //sg1_dr["sg1_t7"] = dt.Rows[i]["col7"].ToString().Trim();
                            sg1_dr["sg1_t8"] = "";
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        int j;
                        for (j = i; j < 60; j++)
                        {
                            sg1_add_blankrows();
                        }
                        ViewState["sg1"] = sg1_dt;
                        sg1_add_blankrows();
                        sg1.DataSource = sg1_dt;
                        sg1.DataBind();
                        dt.Dispose(); sg1_dt.Dispose();
                        ((TextBox)sg1.Rows[z].FindControl("sg1_t1")).Focus();
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        txtlblBend.Focus();

                        txtlbl4.Text = "";
                        txtlbl4a.Text = "";
                        txtlbl7.Text = "";
                        txtlbl7a.Text = "";
                        txtlbl101.Text = "";
                        txtlbl101a.Text = "";
                    }
                    #endregion
                    btnlbl4.Focus();
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
                case "COLOR":
                    if (col1 == "") return;
                    txtlbl6.Text = col1.ToUpper().Replace("COLOR", "").Replace("COLORS", "").Replace("COLOUR", "").Replace("COLOURS", "");
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
                    #region Edit Start
                    if (col1 == "") return;
                    clearctrl();
                    SQuery = "Select to_char(a.vchdate,'dd/mm/yyyy') as vchdt,b.iname,b.cpartno as bcpartno,b.cdrgno,a.*,to_chaR(a.ent_dt,'dd/mm/yyyy') as pent_Dt,d.aname,c.iname as piname from " + frm_tabname + " a left join item c on trim(a.picode)=trim(c.icode) ,item b,famst d where trim(A.icode)=trim(B.icode) and trim(a.acode)=trim(d.acode) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = dt.Rows[0]["vchdt"].ToString().Trim();
                        txtlbl2.Text = dt.Rows[0]["col14"].ToString().Trim();
                        txtlbl3.Text = dt.Rows[0]["col16"].ToString().Trim();
                        txtlbl5.Text = dt.Rows[0]["rejqty"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[0]["recalib"].ToString().Trim();
                        txtlbl4.Text = dt.Rows[0]["acode"].ToString().Trim();
                        txtlbl4a.Text = dt.Rows[0]["aname"].ToString().Trim();
                        txtlbl7.Text = dt.Rows[0]["icode"].ToString().Trim();
                        txtlbl7a.Text = dt.Rows[0]["iname"].ToString().Trim();
                        txtlbl8.Text = dt.Rows[0]["bcpartno"].ToString().Trim();
                        txtlbl9.Text = dt.Rows[0]["cdrgno"].ToString().Trim();
                        txtlbl101.Text = dt.Rows[0]["picode"].ToString().Trim();
                        //if (txtlbl101.Text.Length >= 8)
                        //{
                        //    txtlbl101a.Text = fgen.seek_iname(frm_qstr, frm_cocd, "select iname from item where trim(upper(icode))=upper(Trim('" + txtlbl101.Text + "'))", "iname");
                        //}
                        txtlbl101a.Text = dt.Rows[0]["piname"].ToString().Trim();
                        txtlbl102.Text = dt.Rows[0]["maintdt"].ToString().Trim();
                        txtlbl103.Text = dt.Rows[0]["btchdt"].ToString().Trim();
                        txtrmk.Text = dt.Rows[0]["title"].ToString().Trim();
                        txtlblBend.Text = dt.Rows[0]["bend_wt"].ToString().Trim();
                        txtlblPly.Text = dt.Rows[0]["col15"].ToString().Trim();
                        txtlblStdWstg.Text = dt.Rows[0]["col18"].ToString().Trim();
                        txtlblFinish.Text = dt.Rows[0]["coilnos"].ToString().Trim();
                        txtlblPrefMc.Text = dt.Rows[0]["col17"].ToString().Trim();
                        txtlblFlute.Text = dt.Rows[0]["grade"].ToString().Trim();
                        txtlblDie.Text = dt.Rows[0]["col12"].ToString().Trim();
                        txtlblModel.Text = dt.Rows[0]["col13"].ToString().Trim();

                        lblwwidth.Text = dt.Rows[0]["num2"].ToString().Trim();

                        txtlbl13.Text = dt.Rows[0]["amdcomment"].ToString().Trim();
                        txtAmdDt1.Text = dt.Rows[0]["amddt"].ToString().Trim();

                        txtlbl14.Text = dt.Rows[0]["amdcomment2"].ToString().Trim();
                        txtAmdDt2.Text = dt.Rows[0]["amddt2"].ToString().Trim();

                        txtlbl15.Text = dt.Rows[0]["amdcomment3"].ToString().Trim();
                        txtAmdDt3.Text = dt.Rows[0]["amddt3"].ToString().Trim();

                        txtlbl16.Text = dt.Rows[0]["amdcomment4"].ToString().Trim();
                        txtAmdDt4.Text = dt.Rows[0]["amddt4"].ToString().Trim();

                        txtlbl17.Text = dt.Rows[0]["amdcomment5"].ToString().Trim();
                        txtAmdDt5.Text = dt.Rows[0]["amddt5"].ToString().Trim();

                        txtlbl18.Text = dt.Rows[0]["amdcomment6"].ToString().Trim();
                        txtAmdDt6.Text = dt.Rows[0]["amddt6"].ToString().Trim();

                        lblAppr.Text = "";
                        if (dt.Rows[0]["app_by"].ToString().Trim().Length > 2)
                        {
                            lblAppr.Text = "[Last Approved By : " + dt.Rows[0]["app_by"].ToString().Trim() + " " + Convert.ToDateTime(dt.Rows[0]["app_dt"].ToString().Trim()).ToString("dd/MM/yyyy") + "]";
                        }

                        create_tab();
                        sg1_dr = null;
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            sg1_dr = sg1_dt.NewRow();
                            sg1_dr["sg1_srno"] = i + 1;
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
                            sg1_dr["sg1_f1"] = dt.Rows[i]["btchno"].ToString().Trim();
                            sg1_dr["sg1_t1"] = dt.Rows[i]["col1"].ToString().Trim();
                            sg1_dr["sg1_t2"] = dt.Rows[i]["col2"].ToString().Trim();
                            sg1_dr["sg1_t3"] = dt.Rows[i]["col3"].ToString().Trim();
                            sg1_dr["sg1_t4"] = dt.Rows[i]["col4"].ToString().Trim();
                            sg1_dr["sg1_t5"] = dt.Rows[i]["col5"].ToString().Trim();
                            sg1_dr["sg1_t6"] = dt.Rows[i]["col6"].ToString().Trim();
                            sg1_dr["sg1_t7"] = dt.Rows[i]["col9"].ToString().Trim();
                            sg1_dr["sg1_t8"] = dt.Rows[i]["col10"].ToString().Trim();
                            sg1_dr["sg1_t9"] = dt.Rows[i]["col11"].ToString().Trim();
                            sg1_dt.Rows.Add(sg1_dr);
                        }
                        int j;
                        for (j = i; j < 60; j++)
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
                        edmode.Value = "Y";
                    }
                    #endregion

                    //btnlbl7
                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ICODE FROM COSTESTIMATE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='30' AND TRIM(ICODE)='" + txtlbl7.Text.Trim() + "'", "ICODE");
                    if (col1 != "0")
                    {
                        btnlbl7.Enabled = false;
                    }
                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT VCHNUM FROM ITWSTAGE WHERE BRANCHCD!='DD' AND TRIM(ICODE)='" + txtlbl7.Text.Trim() + "' ", "VCHNUM");
                    if (col1 != "0")
                    {
                        lblStgMappingDone.Text = "Stage Mapping : Done";
                    }
                    else lblStgMappingDone.Text = "Stage Mapping : Not Done";
                    break;

                case "Print_E":
                    if (col1.Length < 2) return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F10135");
                    fgen.fin_engg_reps(frm_qstr);
                    break;

                case "TACODE":
                    if (col1.Length <= 0) return;
                    dt = new DataTable();
                    txtlbl4.Text = col1;
                    txtlbl4a.Text = col2;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_mbr, "SELECT CPARTNO,CDRGNO FROM ITEM WHERE ICODE='" + txtlbl4.Text.Trim() + "'");
                    if (dt.Rows.Count > 0)
                    {
                        txtlbl5.Text = dt.Rows[0]["cpartno"].ToString().Trim();
                        txtlbl6.Text = dt.Rows[0]["cdrgno"].ToString().Trim();
                    }
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
                    btnlbl101.Focus();
                    break;

                case "TPICODE":
                    if (col1.Length <= 0) return;
                    txtlbl101.Text = col1;
                    txtlbl101a.Text = col2;
                    txtlblBend.Focus();
                    break;

                case "SG1_ROW_ADD":
                    #region for gridview 1
                    if (col1.Length <= 0) return;
                    //if (col1.Length <= 0) return;
                    //if (ViewState["sg1"] != null)
                    //{
                    //    dt = new DataTable();
                    //    sg1_dt = new DataTable();
                    //    dt = (DataTable)ViewState["sg1"];
                    //    z = dt.Rows.Count - 1;
                    //    
                    sg1_dt = dt.Clone();
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
                    #endregion
                    setColHeadings();
                    break;
                case "sg1_t1":
                    if (col1.Length > 1)
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = col2.Replace("'", "");
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t5")).Text = col3.Replace("'", "");

                        if (((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text.ToUpper() == "TOP LAYER" || ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text.ToUpper() == "TOP PAPER")
                        {
                            dt = fgen.getdata(frm_qstr, frm_cocd, "SELECT OPRATE1,OPRATE2 FROM ITEM WHERE ICODE='" + col3.Replace("'", "") + "'");
                            if (dt.Rows.Count > 0)
                            {
                                txtlbl102.Text = dt.Rows[0]["oprate1"].ToString();
                                if (col3.Replace("'", "").Substring(0, 2) == "07")
                                {
                                    hffield.Value = "FLEN";
                                    fgen.Fn_ValueBox("Select Length", frm_qstr);
                                }
                                else
                                    txtlbl103.Text = dt.Rows[0]["oprate2"].ToString();
                            }
                        }
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t3")).Focus();
                    }
                    else ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Focus();
                    break;
                case "sg1_t7":
                    if (col1.Length > 1)
                    {
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t8")).Text = col2;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t9")).Text = col3;
                        ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t9")).Focus();
                    }
                    else ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t8")).Focus();
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
                    #endregion

                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t1")).Text = "-";
                    ((TextBox)sg1.Rows[Convert.ToInt32(hf1.Value)].FindControl("sg1_t2")).Text = "-";

                    setColHeadings();
                    break;

                case "DIE":
                    txtlblDie.Text = col2;
                    btnlblModel.Focus();
                    break;

                case "FLUTE":
                    txtlblFlute.Text = col3;
                    btnlblDie.Focus();
                    break;

                case "MODEL":
                    txtlblModel.Text = col3;
                    break;

                case "PREFMAC":
                    txtlblPrefMc.Text = col3;
                    btnlblFlute.Focus();
                    break;
                case "CTNID":
                    double c4 = 0, c5 = 0, c6 = 0, c7 = 0, c8 = 0, c9 = 0, c10 = 0, c11 = 0, c12 = 0, c13 = 0, f4 = 0, f5 = 0, f6 = 0, f7 = 0, f8 = 0, f9 = 0, f10 = 0, f11 = 0, f12 = 0;
                    c4 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL1").toDouble();
                    c5 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL2").toDouble();
                    c6 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL3").toDouble();
                    c7 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL4").toDouble();
                    c8 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL5").toDouble();
                    c9 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL6").toDouble();
                    c10 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL7").toDouble();
                    c11 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL8").toDouble();
                    c12 = fgenMV.Fn_Get_Mvar(frm_qstr, "M_COL9").toDouble();
                    f4 = c4 + c7;
                    f5 = c5 + c7;
                    f6 = c6 + c7;
                    f7 = ((f5 / 2) + c11).toDouble(2); //'flap
                    f8 = ((f5 + f6 + c11 * 2) * c12 + c8) / 10; //'calc reel size

                    f10 = ((f4 + f5) * 2 + c10 + c9) / 10; //'sheet l
                    f11 = f8 - c8 / 10; //'finish size
                    f12 = f10 * 10; //' cut lengh mm

                    txtlbl3.Text = c4 + " X " + c5 + " X " + c6;
                    txtlbl3.BackColor = System.Drawing.Color.LightGreen;
                    txtlbl2.Text = f4 + " X " + f5 + " X " + f6;
                    txtlbl2.BackColor = System.Drawing.Color.LightGreen;

                    txtlblPly.Text = c7.ToString();
                    txtlblPly.BackColor = System.Drawing.Color.LightGreen;
                    txtlbl5.Text = c12.ToString();
                    txtlbl5.BackColor = System.Drawing.Color.LightGreen;
                    txtlbl103.Text = f8.ToString();
                    txtlbl103.BackColor = System.Drawing.Color.LightGreen;
                    txtlbl10.Text = f10.ToString();
                    txtlbl10.BackColor = System.Drawing.Color.LightGreen;
                    txtlblFinish.Text = f11.ToString();
                    txtlblFinish.BackColor = System.Drawing.Color.LightGreen;

                    string auto_Fill = "";
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        if (((TextBox)gr.FindControl("sg1_t1")).Text.ToUpper().Trim().Contains("CUTTING SIZE"))
                        {
                            auto_Fill = "CUTTING SIZE " + f12;
                            ((TextBox)gr.FindControl("sg1_t2")).Text = auto_Fill;
                            ((TextBox)gr.FindControl("sg1_t2")).BackColor = System.Drawing.Color.LightGreen;
                        }
                        if (((TextBox)gr.FindControl("sg1_t1")).Text.ToUpper().Trim().Contains("FLAP"))
                        {
                            auto_Fill = auto_Fill + " FLAP " + f7;
                            ((TextBox)gr.FindControl("sg1_t2")).Text = f7.ToString();
                            ((TextBox)gr.FindControl("sg1_t2")).BackColor = System.Drawing.Color.LightGreen;
                        }
                        if (((TextBox)gr.FindControl("sg1_t1")).Text.ToUpper().Trim().Contains("DECKLE TRIM"))
                        {
                            auto_Fill = auto_Fill + "DECKLE TRIM " + c8;
                            ((TextBox)gr.FindControl("sg1_t2")).Text = c8.ToString();
                            ((TextBox)gr.FindControl("sg1_t2")).BackColor = System.Drawing.Color.LightGreen;
                        }
                        if (((TextBox)gr.FindControl("sg1_t1")).Text.ToUpper().Trim().Contains("TRIMMING"))
                        {
                            auto_Fill = auto_Fill + "TRIMMING " + c9;
                            ((TextBox)gr.FindControl("sg1_t2")).Text = c9.ToString();
                            ((TextBox)gr.FindControl("sg1_t2")).BackColor = System.Drawing.Color.LightGreen;
                        }
                    }
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
            SQuery = "select a.Vchnum as Pplan_no,to_char(a.vchdate,'dd/mm/yyyy') as Pplan_Dt,c.Aname as Customer,b.Iname,b.Cpartno,a.Col1 as Parameter,a.col2 as Standard,a.col3 ,a.col4 ,a.acode,a.icode,a.Ent_by,a.ent_Dt ,to_Char(a.vchdate,'yyyymmdd') as vdd,a.srno from " + frm_tabname + " a,item b,famst c where trim(A.acode)=trim(c.acode) and trim(A.icode)=trim(b.icode) and  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and vchdate " + PrdRange + " order by vdd desc,a.vchnum desc,a.srno";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------            
            string last_entdt;
            last_entdt = fgen.seek_iname(frm_qstr, frm_cocd, "select vchnum as Doc_no from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + lbl1a.Text + "' and trim(icode)='" + txtlbl7.Text.Trim() + "' and trim(vchnum)!='" + txtvchnum.Text.Trim() + "'", "Doc_no");
            if (last_entdt.Trim().Length < 6)
            { }
            else
            {
                Checked_ok = "N";
                fgen.msg("-", "AMSG", "Dear " + frm_uname + " , Process Plan Already Made for This Item on Doc. No. " + last_entdt + " ,Please Check !!");
            }
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
                                if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Length > 1)
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
                                fgen.msg("-", "AMSG", lblheader.Text + " " + frm_vnum + " Saved Successfully ");
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

        sg1_dt.Columns.Add(new DataColumn("sg1_SrNo", typeof(string)));
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

        sg1_dr["sg1_srno"] = sg1_dt.Rows.Count + 1;


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
        fgen.Fn_open_sseek("Select " + lbl4.Text, frm_qstr);
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
        fgen.Fn_open_sseek("Select " + lbl7.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlbl101_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TPICODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl101.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");



        for (i = 0; i < sg1.Rows.Count - 0; i++)
        {
            if (((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Length > 1)
            {
                oporow = oDS.Tables[0].NewRow();
                oporow["branchcd"] = frm_mbr;
                oporow["type"] = frm_vty;
                oporow["vchnum"] = frm_vnum.Trim().ToUpper();
                oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
                oporow["SRNO"] = i + 1;
                oporow["acode"] = txtlbl4.Text.Trim().ToUpper();
                oporow["icode"] = txtlbl7.Text.Trim().ToUpper();
                oporow["picode"] = txtlbl101.Text.Trim().ToUpper();
                oporow["cpartno"] = txtlbl8.Text.Trim().ToUpper();
                oporow["col14"] = txtlbl2.Text.Trim().ToUpper();
                oporow["col16"] = txtlbl3.Text.Trim().ToUpper();
                oporow["rejqty"] = fgen.make_double(txtlbl5.Text.Trim().ToUpper());
                oporow["recalib"] = fgen.make_double(txtlbl6.Text.Trim().ToUpper());
                oporow["btchdt"] = txtlbl103.Text.Trim().ToUpper();
                oporow["maintdt"] = txtlbl102.Text.Trim().ToUpper();
                oporow["col1"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t1")).Text.Trim().ToUpper();
                oporow["col2"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t2")).Text.Trim().ToUpper();
                oporow["col3"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t3")).Text.Trim().ToUpper();
                oporow["col4"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t4")).Text.Trim().ToUpper();
                oporow["col5"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t5")).Text.Trim().ToUpper();
                oporow["col6"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t6")).Text.Trim().ToUpper();
                oporow["amdno"] = 0;

                oporow["crefdt"] = vardate.Substring(0, 10);
                oporow["col9"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t7")).Text.Trim().ToUpper();
                oporow["col10"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t8")).Text.Trim().ToUpper();
                oporow["col11"] = ((TextBox)sg1.Rows[i].FindControl("sg1_t9")).Text.Trim().ToUpper();
                oporow["grade"] = txtlblFlute.Text.Trim().ToUpper();
                oporow["btchno"] = ((TextBox)sg1.Rows[i].FindControl("sg1_f1")).Text.Trim().ToUpper();
                oporow["col12"] = txtlblDie.Text.Trim().ToUpper();
                oporow["col13"] = txtlblModel.Text.Trim().ToUpper();
                oporow["col15"] = txtlblPly.Text.Trim().ToUpper();
                oporow["col17"] = txtlblPrefMc.Text.Trim().ToUpper();
                oporow["col18"] = txtlblStdWstg.Text.Trim().ToUpper();

                oporow["amdcomment"] = txtlbl13.Text.Trim();
                oporow["amddt"] = txtAmdDt1.Text;

                oporow["amdcomment2"] = txtlbl14.Text.Trim();
                oporow["amddt2"] = txtAmdDt2.Text;

                oporow["amdcomment3"] = txtlbl15.Text.Trim();
                oporow["amddt3"] = txtAmdDt3.Text;

                oporow["amdcomment4"] = txtlbl16.Text.Trim();
                oporow["amddt4"] = txtAmdDt4.Text;

                oporow["amdcomment5"] = txtlbl17.Text.Trim();
                oporow["amddt5"] = txtAmdDt5.Text;

                oporow["amdcomment6"] = txtlbl18.Text.Trim();
                oporow["amddt6"] = txtAmdDt6.Text;

                oporow["remark2"] = txtlbl10.Text.Trim().ToUpper();
                oporow["remark3"] = txtlbl11.Text.Trim().ToUpper();
                oporow["remark4"] = txtlbl12.Text.Trim().ToUpper();

                oporow["closeit"] = "N";
                oporow["app_by"] = "-";
                oporow["app_dt"] = vardate;
                oporow["coilnos"] = txtlblFinish.Text.Trim().ToUpper().toDouble(2);
                oporow["atch_mnt"] = "-";
                oporow["num1"] = 0;
                oporow["num2"] = lblwwidth.Text.Trim().toDouble(2);
                oporow["num3"] = 0;
                oporow["num4"] = 0;
                oporow["numwt"] = 0;
                oporow["bend_wt"] = fgen.make_double(txtlblBend.Text.Trim().ToUpper(), 2);
                oporow["block_dtl"] = "-";
                oporow["title"] = txtrmk.Text.Trim().ToUpper();
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

                if (chkAppr.Checked)
                {
                    oporow["app_by"] = frm_uname;
                    oporow["app_dt"] = vardate;
                }

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
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "70");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------   
    protected void btnlblModel_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MODEL";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblModel.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlblDie_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DIE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblDie.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlblFlute_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "FLUTE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblFlute.Text, frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnlblPrefMc_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "PREFMAC";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lblPrefMc.Text, frm_qstr);
    }
    protected void btnView_ServerClick(object sender, EventArgs e)
    {
        if (txtlbl4.Text.Trim().Length > 2)
        {
            col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NVL(IMAGEF,'-') AS IMAGEF FROM ITEM WHERE ICODE='" + txtlbl7.Text.Trim() + "' ", "IMAGEF");
            if (col1.Length > 2)
            {
                try
                {
                    string newPath = Server.MapPath(@"~\tej-base\upload\");
                    string filename = Path.GetFileName(col1);
                    newPath += filename;
                    File.Copy(col1, newPath, true);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filename + "','90%','90%','Finsys Viewer');", true);
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
    protected void btnGridPop_Click(object sender, EventArgs e)
    {
        if (hf1.Value.Contains("sg1_t1_"))
        {
            hffield.Value = "sg1_t1";
            hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_sg1_t1_", "");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            make_qry_4_popup();
            if (hf1.Value.toDouble() > 7)
                fgen.Fn_open_sseek("Select Product", frm_qstr);
            else Fn_ValueBox("Select Paper", frm_qstr);
        }
        if (hf1.Value.Contains("sg1_t7_"))
        {
            hffield.Value = "sg1_t7";
            hf1.Value = hf1.Value.Replace("ContentPlaceHolder1_sg1_sg1_t7_", "");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CMD_HF1", hf1.Value);
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select Flute", frm_qstr);
        }
    }
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
    protected void btnColor_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "COLOR";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select " + lbl6.Text, frm_qstr);
    }
    protected void btnItemPending_ServerClick(object sender, EventArgs e)
    {

        frm_vty = "70";

        string pquery;
        string br_Cond = "";

        br_Cond = "branchcd!='DD'";
        if (doc_addl.Value == "Y")
        {
            br_Cond = "branchcd='" + frm_mbr + "'";
        }

        pquery = "select trim(icode) as icode,sum(cnt) as tot from (select icode,1 as cnt from item where length(trim(nvl(deac_by,'-')))<=1 and length(trim(icode))>4 union all select distinct icode,-1 as cnt from inspmst where " + br_Cond + " and type='" + lbl1a.Text + "') group by trim(icode) having sum(cnt)>0 ";
        SQuery = "SELECT '-' as fstr,'-' as gstr,trim(b.Iname) as Item_name,a.Icode,b.Cpartno,b.Cdrgno,b.unit from (" + pquery + ")a ,Item b where trim(A.icode)=trim(B.icode) and length(trim(nvl(b.deac_by,'-')))<=1 and length(trim(b.icode))>4 and substr(b.icode,1,1) in ('7','8','9') order by b.iname";


        fgen.drillQuery(0, SQuery, frm_qstr, "1#2#", "3#4#5#6#", "400#300#100#100#");
        fgen.Fn_DrillReport("List of " + lblheader.Text.Trim() + " ", frm_qstr);


        //string doubleQuote = "\"";
        //SQuery = @"SELECT REPLACE(REPLACE(INAME,'" + doubleQuote + "',''),'''','') AS INAME,REPLACE(REPLACE(CPARTNO,'" + doubleQuote + "',''),'''','') AS CPARTNO,ICODE FROM ITEM WHERE SUBSTR(ICODE,1,1) IN ('9','7') AND  LENGTH(TRIM(ICODE))>4 AND TRIM(ICODE) NOT IN (SELECT DISTINCT TRIM(ICODE) AS ICODE FROM INSPMST WHERE BRANCHCD!='DD' AND TYPE='70') ORDER BY ICODE ";
        //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        //fgen.Fn_open_rptlevel("List of Pending Items", frm_qstr);
    }
    protected void btnCtnID_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CTNID";
        if (HttpContext.Current.CurrentHandler is Page)
        {
            string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/ival_ctnid.aspx");
            Page p = (Page)HttpContext.Current.CurrentHandler;
            p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle1('" + fil_loc + "?STR=" + frm_qstr + "','720px','350px','" + "Technical Information" + "');", true);
        }
    }
}