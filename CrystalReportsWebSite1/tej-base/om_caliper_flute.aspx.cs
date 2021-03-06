using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Diagnostics;

public partial class om_caliper_flute : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt, mq0, typePopup = "N";
    DataTable dt, dt2, dt3, dt4; DataRow oporow; DataSet oDS; DataRow oporow2; DataSet oDS2; DataRow oporow3; DataSet oDS3; DataRow oporow4; DataSet oDS4; DataRow oporow5; DataSet oDS5;
    int i = 0, z = 0;
    DataTable dtCol = new DataTable();
    string Checked_ok;
    string save_it;
    string cmd_query = "";
    string Prg_Id, lbl1a_Text, CSR;
    string pk_error = "Y", chk_rights = "N", DateRange, PrdRange, cond;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    string a, b, c;
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
                    lbl1a_Text = "CS";
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
                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                switch (Prg_Id)
                {
                    case "F10146":
                        lblheader.Text = "Paper Flute, Caliper Index Master";
                        break;
                    case "F10147":
                        lblheader.Text = "Box Master";
                        break;
                    //by akshay
                    case "F10148":
                        lblheader.Text = "Paper Index Rate Master";
                        break;
                }
                fgen.DisableForm(this.Controls);
                enablectrl();
                getColHeading();
            }
            setColHeadings();
            set_Val();

            if (frm_ulvl != "0")
            {
                btndel.Visible = false;
            }

            if (lblUpload.Text.Length > 1)
            {
                btnView1.Visible = true;
                btnDwnld1.Visible = true;
            }
            btnprint.Visible = false;
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

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        btnlist.Disabled = false;
        btnprint.Disabled = false; Attch.Enabled = false;
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
        Attch.Enabled = true;
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
        Attch.Enabled = false;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        switch (Prg_Id)
        {
            case "F10146":
                frm_tabname = "wb_corrcst_flutem"; // flute master table.....REAL TABLE   
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "^4");
                Label11.InnerText = "Flute";
                Label12.InnerText = "Caliper";
                Label14.Visible = false; txt_param3.Visible = false;
                Label13.InnerText = "Remarks";
                Label15.InnerText = "Index1";
                Label16.InnerText = "Index2";
                Label17.Visible = false; txt_param7.Visible = false;

                Attch.Visible = false;
                txtAttch.Visible = false;
                txtAttchPath.Visible = false;
                btnAtt.Visible = false;
                lblShow.Visible = false;
                lblUpload.Visible = false;
                btnView1.Visible = false;
                btnDwnld1.Visible = false;
                tab_upload.Visible = false;
                // Label27.Visible = false; by akshay
                break;

            case "F10147":
                frm_tabname = "wb_corrcst_flutem"; // flute master table.....box type  
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "^7");
                Label11.InnerText = "Deccal_Formula";
                Label12.InnerText = "Length_Formula";
                Label14.InnerText = "Name";
                Label13.InnerText = "Remark";
                Label16.InnerText = "Area/piece";
                Label17.InnerText = "Area_Formula";
                Label15.InnerText = "Box_Code";
                txt_param5.MaxLength = 20;
                txt_param7.MaxLength = 8;
                break;

            case "F10148": //by akshay
                frm_tabname = "wb_corrcst_rctm"; // flute master table.....REAL TABLE  
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "^8");
                Label11.InnerText = "BF";
                Label12.InnerText = "GSM";
                txt_param1.MaxLength = 10;
                txt_param2.MaxLength = 10;
                txt_param3.MaxLength = 15;
                txt_param4.MaxLength = 10;
                txt_param5.MaxLength = 50;
                txt_param6.MaxLength = 10;
                txt_param7.MaxLength = 10;
                //Label12.Visible = false;
                tab_upload.Visible = false;
                break;

        }
        typePopup = "N";
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
            case "New":
            case "Edit":
            case "Del":
            case "Print":
                Type_Sel_query();
                break;

            default:
                Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E")

                    switch (Prg_Id)
                    {
                        case "F10146":
                            SQuery = "SELECT trim(vchnum)||to_char(vchdate,'dd/MM/yyyy') as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,flute,caliper as gsm,ent_by, to_char(ent_dt,'dd/mm/yyyy') as ent_date,type,to_char(vchdate,'yyyymmdd') as vdd FROM  " + frm_tabname + " WHERE branchcd='" + frm_mbr + "' and  TYPE='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,vchnum desc"; ///BY YOGITA  edit by akshay
                            SQuery = "SELECT trim(vchnum)||to_char(vchdate,'dd/MM/yyyy') as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,flute,caliper as gsm,ent_by, to_char(ent_dt,'dd/mm/yyyy') as ent_date,type,to_char(vchdate,'yyyymmdd') as vdd FROM  " + frm_tabname + " WHERE branchcd='" + frm_mbr + "' and  TYPE='" + frm_vty + "' order by vdd desc,vchnum desc"; ///BY YOGITA  edit by akshay
                            break;

                        case "F10147":
                            SQuery = "SELECT trim(vchnum)||to_char(vchdate,'dd/MM/yyyy') as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,flute as Deccal_Formula,caliper as Length_Formula,name,ent_by, to_char(ent_dt,'dd/mm/yyyy') as ent_date,type,to_char(vchdate,'yyyymmdd') as vdd FROM  " + frm_tabname + " WHERE branchcd='" + frm_mbr + "' and  TYPE='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,vchnum desc"; ///BY YOGITA edit by akshay
                            SQuery = "SELECT trim(vchnum)||to_char(vchdate,'dd/MM/yyyy') as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,flute as Deccal_Formula,caliper as Length_Formula,name,ent_by, to_char(ent_dt,'dd/mm/yyyy') as ent_date,type,to_char(vchdate,'yyyymmdd') as vdd FROM  " + frm_tabname + " WHERE branchcd='" + frm_mbr + "' and  TYPE='" + frm_vty + "'  order by vdd desc,vchnum desc"; ///BY YOGITA edit by akshay
                            break;

                        case "F10148":
                            SQuery = "select trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy')as vchdate,bf,gsm,code as FEFCO_code,ent_by, to_char(ent_dt,'dd/mm/yyyy') as ent_date,type,to_char(vchdate,'yyyymmdd') as vdd from " + frm_tabname + " where branchcd=" + frm_mbr + " and type='" + frm_vty + "' and vchdate " + DateRange + " order by vdd desc,vchnum desc";
                            SQuery = "select trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum,to_char(vchdate,'dd/mm/yyyy')as vchdate,bf,gsm,code as FEFCO_code,ent_by, to_char(ent_dt,'dd/mm/yyyy') as ent_date,type,to_char(vchdate,'yyyymmdd') as vdd from " + frm_tabname + " where branchcd=" + frm_mbr + " and type='" + frm_vty + "' order by vdd desc,vchnum desc";
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
        if (chk_rights == "Y")
        {
            // if want to ask popup at the time of new            
            hffield.Value = "New";
            if (typePopup == "N")
            {
                newCase(frm_vty);
            }
            else
            {
                make_qry_4_popup();
                fgen.Fn_open_sseek("-", frm_qstr);
            }
            Attch.Enabled = true;
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + " , You Currently Do Not Have Rights To Add New Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    void newCase(string vty)
    {
        #region
        if (col1 == "") return;
        frm_vty = vty;
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", vty);
        lbl1a.Text = vty;
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " where branchcd='" + frm_mbr + "' and  type='" + frm_vty + "' and vchdate " + DateRange + "", 6, "VCH");
        txt_code.Value = frm_vnum;
        txtvchdate.Text = vardate;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        disablectrl();
        fgen.EnableForm(this.Controls);
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
                // Deleing data from WSr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Saving Deleting History
                fgen.save_info(frm_qstr, frm_cocd, frm_mbr, fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6), fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(6, 10), frm_uname, frm_vty, lblheader.Text.Trim() + " Deleted");
                fgen.msg("-", "AMSG", "Entry Deleted For " + lblheader.Text + " No." + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").Substring(0, 6) + "");
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
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    hffield.Value = "Del_E";
                    make_qry_4_popup();
                    fgen.Fn_open_sseek("Select Entry to Delete", frm_qstr);
                    break;

                case "Edit":
                    if (col1 == "") return;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
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
                    Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    SQuery = "Select a.* from " + frm_tabname + " a where trim(branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/MM/yyyy')='" + frm_mbr + frm_vty + col1 + "'";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        i = 0;
                        ViewState["entby"] = dt.Rows[0]["ent_by"].ToString();
                        ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                        switch (Prg_Id)
                        {
                            case "F10146":
                                txt_code.Value = dt.Rows[i]["vchnum"].ToString();
                                txtvchdate.Text = Convert.ToDateTime(dt.Rows[i]["vchdate"].ToString()).ToString("dd/MM/yyyy");
                                txt_param1.Value = dt.Rows[i]["flute"].ToString();
                                txt_param2.Value = dt.Rows[i]["caliper"].ToString();
                                txt_param4.Value = dt.Rows[i]["rem"].ToString();
                                txt_param5.Value = dt.Rows[i]["ind1"].ToString();
                                txt_param6.Value = dt.Rows[i]["ind2"].ToString();

                                if ((txt_param1.Value.Trim() == "B") || (txt_param1.Value.Trim() == "BC") || (txt_param1.Value.Trim() == "C"))
                                {
                                    txt_param2.Disabled = true;
                                }
                                break;

                            case "F10147":
                                txt_code.Value = dt.Rows[i]["vchnum"].ToString();
                                txtvchdate.Text = Convert.ToDateTime(dt.Rows[i]["vchdate"].ToString()).ToString("dd/MM/yyyy");
                                txtAttchPath.Text = dt.Rows[i]["Imagepath"].ToString();
                                txtAttch.Text = dt.Rows[i]["image"].ToString();
                                txt_param1.Value = dt.Rows[i]["flute"].ToString();
                                txt_param2.Value = dt.Rows[i]["caliper"].ToString();
                                txt_param3.Value = dt.Rows[i]["name"].ToString();
                                txt_param4.Value = dt.Rows[i]["rem"].ToString();
                                txt_param5.Value = dt.Rows[i]["boxtypecode"].ToString();
                                txt_param6.Value = dt.Rows[i]["ind2"].ToString();
                                txt_param7.Value = dt.Rows[i]["area"].ToString();
                                break;

                            case "F10148":
                                txt_code.Value = dt.Rows[i]["vchnum"].ToString();
                                txtvchdate.Text = Convert.ToDateTime(dt.Rows[i]["vchdate"].ToString()).ToString("dd/MM/yyyy");
                                // txtAttchPath.Text = dt.Rows[i]["Imagepath"].ToString();
                                // txtAttch.Text = dt.Rows[i]["image"].ToString();
                                txt_param1.Value = dt.Rows[i]["BF"].ToString();
                                txt_param2.Value = dt.Rows[i]["gsm"].ToString();
                                txt_param3.Value = dt.Rows[i]["code"].ToString();
                                txt_param4.Value = dt.Rows[i]["hrctrt"].ToString();
                                txt_param5.Value = dt.Rows[i]["rem"].ToString();
                                txt_param6.Value = dt.Rows[i]["nrcti"].ToString();
                                txt_param7.Value = dt.Rows[i]["hrcti"].ToString();
                                break;

                        }
                        fgen.EnableForm(this.Controls);
                        disablectrl();
                        setColHeadings();
                        edmode.Value = "Y";
                        if (lblUpload.Text.Length > 1) btnDwnld1.Visible = true;
                    }
                    #endregion
                    break;

                case "Print_E":
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "pr1", "pr1");
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
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            switch (Prg_Id)
            {
                case "F10146":
                    SQuery = "SELECT trim(vchnum) as vchnum, to_char(vchdate,'dd/mm/yyyy') as vchdate,flute,caliper as gsm,rem as remarks,ind1 as index1,ind2 as index2,ent_by ,to_char(ent_dt,'dd/mm/yyyy') as ent_dt,to_char(vchdate,'yyyymmdd') as vdd FROM " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + " ORDER BY vdd DESC,vchnum DESC";
                    break;

                case "F10147":
                    SQuery = "SELECT trim(vchnum) as vchnum, to_char(vchdate,'dd/mm/yyyy') as vchdate,flute as Deccal_Formula,caliper as Length_Formula,name,rem,boxtypecode as area_Formula,ind2 as index2,area,imagepath,image as image_name,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt,to_char(vchdate,'yyyymmdd') as vdd FROM " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + " ORDER BY vdd DESC,vchnum DESC";
                    break;

                case "F10148":
                    SQuery = "SELECT trim(vchnum) as vchnum, to_char(vchdate,'dd/mm/yyyy') as vchdate, bf,gsm,code as fefco_code,hrctrt as caliper,rem as remarks,hrcti as index2,nrcti as area,ent_by,to_char(ent_dt,'dd/mm/yyyy') as ent_dt,to_char(vchdate,'yyyymmdd') as vdd FROM " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.vchdate " + PrdRange + " ORDER BY vdd DESC,vchnum DESC";
                    break;
            }
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            hffield.Value = "-";
        }
        else
        {
            Checked_ok = "Y";
            //-----------------------------
            i = 0;
            hffield.Value = "";

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
                            frm_vnum = txt_code.Value.Trim();
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

                        save_fun();

                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        if (edmode.Value == "Y")
                        {
                            fgen.msg("-", "AMSG", lblheader.Text + " " + txt_code.Value + " Updated Successfully");
                            cmd_query = "delete from " + frm_tabname + " where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        else
                        {
                            if (save_it == "Y")
                            {
                                //fgen.send_mail(frm_cocd, "Tejaxo ERP", "info@pocketdriver.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txt_code.Value + " Saved Successfully ");
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
                        fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                        fgen.msg("-", "AMSG", ex.Message.ToString());
                        col1 = "N"; btnsave.Disabled = false; lblUpload.Text = "";
                    }
                }
            #endregion
            }
        }
    }
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
    protected void btn_mgr_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "MGRBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Account Code", frm_qstr);
    }
    protected void btn_costcent_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "COSTBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Cost center code", frm_qstr);
    }
    protected void btn_stat_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "STATBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select State", frm_qstr);
    }
    protected void btn_bnkacct_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "BNKACTBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select  Bank account", frm_qstr);
    }
    protected void btn_ctry_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "CTRYBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Country", frm_qstr);
    }
    protected void btn_ivl_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "IVLBUT";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select level cost center cost", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    void save_fun()
    {
        set_Val();
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");

        oporow = oDS.Tables[0].NewRow();
        oporow["branchcd"] = frm_mbr;
        oporow["vchnum"] = frm_vnum.Trim().ToUpper();
        oporow["vchdate"] = txtvchdate.Text.Trim().ToUpper();
        switch (Prg_Id)
        {
            case "F10146":
                oporow["TYPE"] = frm_vty; //FLUTE MASTER
                oporow["FLUTE"] = txt_param1.Value.ToUpper().Trim();
                oporow["CALIPER"] = txt_param2.Value.ToUpper().Trim();
                oporow["ind1"] = fgen.make_double(txt_param5.Value.ToString().Trim());
                //oporow["ind2"] = fgen.make_double(txt_param7.Value.ToString().Trim());
                oporow["ind2"] = fgen.make_double(txt_param6.Value.ToString().Trim());
                oporow["REM"] = txt_param4.Value.ToUpper().Trim();
                oporow["area"] = "-";
                oporow["Boxtypecode"] = "-";
                oporow["name"] = "-";
                oporow["image"] = "-";
                break;

            case "F10147":
                oporow["TYPE"] = frm_vty;//BOX MASTER
                oporow["boxtypecode"] = txt_param5.Value.ToString().Trim();
                oporow["area"] = txt_param7.Value.ToString().Trim();
                oporow["ind2"] = fgen.make_double(txt_param6.Value.ToString().Trim());
                // txt_param5.Value.ToString().ToUpper().Trim();
                oporow["REM"] = txt_param4.Value.ToUpper().Trim();
                oporow["name"] = txt_param3.Value.ToString().ToUpper().Trim();
                oporow["caliper"] = txt_param2.Value.ToString().ToUpper().Trim();
                oporow["flute"] = txt_param1.Value.ToString().ToUpper().Trim();
                if (txtAttch.Text.Length > 1)
                {
                    oporow["IMAGE"] = txtAttch.Text.ToUpper().Trim();
                    oporow["IMAGEPATH"] = txtAttchPath.Text.ToUpper().Trim();
                }
                break;

            case "F10148":
                oporow["TYPE"] = frm_vty;//PAPER MASTER
                oporow["code"] = txt_param3.Value.ToString().ToUpper().Trim();
                oporow["gsm"] = fgen.make_double(txt_param2.Value.ToString().ToUpper().Trim());
                oporow["BF"] = fgen.make_double(txt_param1.Value.ToString().ToUpper().Trim());
                oporow["hrctrt"] = fgen.make_double(txt_param4.Value.ToString().ToUpper().Trim());
                oporow["rem"] = txt_param5.Value.ToString().Trim();
                oporow["nrcti"] = fgen.make_double(txt_param6.Value.ToString().Trim());
                oporow["hrcti"] = fgen.make_double(txt_param7.Value.ToString().Trim());
                break;
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
            oporow["eNt_dt"] = vardate;
            oporow["edt_by"] = frm_uname;
            oporow["edt_dt"] = vardate;
        }
        oDS.Tables[0].Rows.Add(oporow);
    }
    //------------------------------------------------------------------------------------
    void save_fun5()
    {
    }
    //------------------------------------------------------------------------------------
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F10146":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "^4");
                break;

            case "F10147":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "^7");
                break;

            case "F10148":
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "^8");
                break;
        }
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------      
    protected void btnAtt_Click(object sender, EventArgs e)
    {
        string filepath = @"c:\TEJ_ERP\UPLOAD\";      //Server.MapPath("~/tej-base/UPLOAD/");
        Attch.Visible = true;
        if (Attch.HasFile)
        {
            txtAttch.Text = Attch.FileName;
            filepath = filepath + vardate.Replace(@"/", "_") + "~" + Attch.FileName;
            txtAttchPath.Text = Server.MapPath("~/tej-base/Upload/") + Attch.FileName;

            Attch.PostedFile.SaveAs(filepath + Attch.FileName);
            Attch.PostedFile.SaveAs(Server.MapPath("~/tej-base/Upload/") + Attch.FileName);


            lblUpload.Text = filepath;
            btnView1.Visible = true;
            btnDwnld1.Visible = true;
        }
        else
        {
            lblUpload.Text = "";
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnView1_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = txtAttchPath.Text.Substring(txtAttchPath.Text.ToUpper().IndexOf("UPLOAD"), txtAttchPath.Text.Length - txtAttchPath.Text.ToUpper().IndexOf("UPLOAD"));
        ScriptManager.RegisterStartupScript(this, this.GetType(), "PopUP", "OpenSingle('" + "../tej-base/Upload/" + filePath.Replace("\\", "/").Replace("UPLOAD", "") + "','90%','90%','Finsys Viewer');", true);
    }
    //------------------------------------------------------------------------------------
    protected void btnDwnld1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string filePath = lblUpload.Text.Substring(lblUpload.Text.ToUpper().IndexOf("UPLOAD"), lblUpload.Text.Length - lblUpload.Text.ToUpper().IndexOf("UPLOAD"));
            Session["FilePath"] = filePath.ToUpper().Replace("\\", "/").Replace("UPLOAD", "");
            Session["FileName"] = txtAttch.Text;
            Response.Write("<script>");
            Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
            Response.Write("</script>");
        }
        catch { }
    }
    //------------------------------------------------------------------------------------
}