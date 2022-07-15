using System;
using System.Data;
using System.Data.OleDb;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.IO;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

public partial class om_mrr_edi : System.Web.UI.Page
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
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_tabname1, frm_uname, frm_PageName, fstr;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2;
    //double double_val2, double_val1;
    fgenDB fgen = new fgenDB();
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Session["mymst"] != null)
        {
            if (Session["mymst"].ToString() == "Y")
            {
                //this.Page.MasterPageFile = "~/tej-base/myNewMaster.master";
                this.Page.MasterPageFile = "~/tej-base/Fin_Master2.master";
            }
            else this.Page.MasterPageFile = "~/tej-base/Fin_Master.master";
        }
    }
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
                    frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");
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
                btnedit.Visible = false;
                btnprint.Visible = false;
                btnlist.Visible = false;
                DataTable dtW = (DataTable)ViewState["dtn"];
                if (dtW != null)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtW, "", "ContentPlaceHolder1_datadiv").ToString(), false);
                }
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

        // to hide and show to tab panel
        fgen.SetHeadingCtrl(this.Controls, dtCol);

    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true;
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
        btnTempl.Visible = false;
        if (frm_formID == "HSNCODE" || frm_formID == "BSSCH_UPL")
        {
            doc_nf.Value = "TYPE1";
            doc_df.Value = "vchdate";
        }
        else if (frm_formID == "SALEORDER")
        {
            doc_nf.Value = "ordno";
            doc_df.Value = "orddt";
        }
        else
        {
            doc_nf.Value = "vchnum";
            doc_df.Value = "vchdate";
        }

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        switch (Prg_Id)
        {
            case "F50103":
                frm_tabname = "WEB_IMPORT";
                lblheader.Text = "E-Comm Invoice Import";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "IN");
                break;
            case "F25108":
                frm_tabname = "SCRATCH2";
                lblheader.Text = "Matl. Inward Import";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "MI");
                break;
            case "HSNCODE":
                btnTempl.Visible = true;
                divRmk.Visible = false;
                btndel.Visible = false;
                frm_tabname = "TYPEGRP";
                lblheader.Text = "HSN Master Upload Routine";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "T1");
                break;
            case "BSSCH_UPL":
                btnTempl.Visible = true;
                divRmk.Visible = false;
                btndel.Visible = false;
                frm_tabname = "TYPEGRP";
                lblheader.Text = "Account Schedule Upload Routine";
                fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "A");
                break;
            case "ACOPBAL":
                btnTempl.Visible = true;
                divRmk.Visible = false;
                btndel.Visible = false;
                frm_tabname = "FAMSTBAL";
                lblheader.Text = "A/C wise Ledger Balance (Op.Bal) Master Upload Routine";
                break;
            case "BILLWISEOUTDR":
                btnTempl.Visible = true;
                divRmk.Visible = false;
                btndel.Visible = false;
                frm_tabname = "RECEBAL";
                lblheader.Text = "Bill Wise Outstandings(Drs) Master Upload Routine";
                break;
            case "BILLWISEOUTCR":
                btnTempl.Visible = true;
                divRmk.Visible = false;
                btndel.Visible = false;
                frm_tabname = "RECEBAL";
                lblheader.Text = "Bill Wise Outstandings(Crs) Master Upload Routine";
                break;
            case "ITEMWISEOPBL":
                btnTempl.Visible = true;
                divRmk.Visible = false;
                btndel.Visible = false;
                frm_tabname = "ITEMVBAL";
                lblheader.Text = "Item wise Stock Balance (Op. Bal) Master Upload Routine";
                break;
            case "WIPSTKOP":
                btnTempl.Visible = true;
                divRmk.Visible = false;
                btndel.Visible = false;
                frm_tabname = "WIPSTK";
                lblheader.Text = "Section Wise WIP Stock (Op.Bal) Master Upload Routine";
                break;
            case "STORESTKBATCHWISE":
                btnTempl.Visible = true;
                divRmk.Visible = false;
                btndel.Visible = false;
                frm_tabname = "ITEMVBAL";
                lblheader.Text = "Batch No. Wise Store Stock Master Upload Routine";
                break;
            case "REELSTOCK":
                btnTempl.Visible = true;
                divRmk.Visible = false;
                btndel.Visible = false;
                frm_tabname = "reelvch_op";
                lblheader.Text = "Reel Stock Master Upload Routine";
                break;
            case "BOM":
                btnTempl.Visible = true;
                divRmk.Visible = false;
                btndel.Visible = false;
                frm_tabname = "itemosp";
                lblheader.Text = "Bill of Materials Master Upload Routine";
                break;
            case "INWQCTEMP":
                btnTempl.Visible = true;
                divRmk.Visible = false;
                btndel.Visible = false;
                frm_tabname = "inspmst";
                lblheader.Text = "Inward Quality Templates Master Upload Routine";
                break;
            case "OUTQCTEMP":
                btnTempl.Visible = true;
                divRmk.Visible = false;
                btndel.Visible = false;
                frm_tabname = "inspmst";
                lblheader.Text = "Outward Quality Templates Master Upload Routine";
                break;
            case "SALEORDER":
                btnTempl.Visible = true;
                divRmk.Visible = false;
                btndel.Visible = false;
                frm_tabname = "somas";
                lblheader.Text = "Sales Orders Master Upload Routine";
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
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
                SQuery = "select acode,aname as customer,acode as code from famst where trim(Acode) like '16%' order by acode";
                break;
            case "TRCODE":
                SQuery = "select acode,aname as customer,acode as code from famst where trim(Acode) like '2%' order by acode";
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
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt ,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
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
            set_Val();

            if (frm_formID == "HSNCODE" || frm_formID == "BSSCH_UPL")
            {
                SQuery = "select max(" + doc_nf.Value + ") as vch from " + frm_tabname + " where branchcd='00' and ID='" + frm_vty + "' ";
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, SQuery, 3, "vch");

                txtvchnum.Text = frm_vnum;
                lbl1a.Text = frm_vty;
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            }
            else if (frm_formID == "ACOPBAL")
            {
                txtvchnum.Text = "-";
                lbl1a.Text = frm_vty;
                txtvchdate.Text = "-";
            }

            else if (frm_formID == "ITEMWISEOPBL")
            {
                SQuery = "select max(" + doc_nf.Value + ") as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='00' and " + doc_df.Value + " " + DateRange + "";
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch");

                txtvchnum.Text = frm_vnum;
                lbl1a.Text = "00";
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            }

            else if (frm_formID == "WIPSTKOP")
            {
                SQuery = "select max(" + doc_nf.Value + ") as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='50' and " + doc_df.Value + " " + DateRange + "";
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch");

                txtvchnum.Text = frm_vnum;
                lbl1a.Text = "50";
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            }

            else if (frm_formID == "STORESTKBATCHWISE")
            {
                SQuery = "select max(" + doc_nf.Value + ") as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='00' and " + doc_df.Value + " " + DateRange + "";
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch");

                txtvchnum.Text = frm_vnum;
                lbl1a.Text = "00";
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            }

            else if (frm_formID == "REELSTOCK")
            {
                SQuery = "select max(" + doc_nf.Value + ") as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='02' and " + doc_df.Value + " " + DateRange + "";
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch");

                txtvchnum.Text = frm_vnum;
                lbl1a.Text = "02";
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            }
            else if (frm_formID == "BOM")
            {
                SQuery = "select max(" + doc_nf.Value + ") as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='BM' and " + doc_df.Value + " " + DateRange + "";
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch");

                txtvchnum.Text = frm_vnum;
                lbl1a.Text = "BM";
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            }
            else if (frm_formID == "INWQCTEMP")
            {
                SQuery = "select max(" + doc_nf.Value + ") as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='20' and " + doc_df.Value + " " + DateRange + "";
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch");

                txtvchnum.Text = frm_vnum;
                lbl1a.Text = "20";
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            }
            else if (frm_formID == "OUTQCTEMP")
            {
                SQuery = "select max(" + doc_nf.Value + ") as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='10' and " + doc_df.Value + " " + DateRange + "";
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch");

                txtvchnum.Text = frm_vnum;
                lbl1a.Text = "10";
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            }
            else if (frm_formID == "SALEORDER")
            {
                SQuery = "select max(" + doc_nf.Value + ") as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "'  and " + doc_df.Value + " " + DateRange + "";
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, SQuery, 6, "vch");

                txtvchnum.Text = frm_vnum;
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            }
            else
            {
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                txtvchnum.Text = frm_vnum;
                txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            }

            disablectrl();
            fgen.EnableForm(this.Controls);
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
        string full_acode = "";
        string pad = "0";
        int code_len = 6;
        string digit7code = "N";
        digit7code = fgen.getOption(frm_qstr, frm_cocd, "W0090", "OPT_ENABLE");
        if (digit7code == "Y")
        {
            code_len = 7;
        }


        fgen.fill_dash(this.Controls);

        DataTable dtn = new DataTable();
        dtn = (DataTable)ViewState["dtn"];

        // ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);

        DataTable dtdist = new DataTable();
        switch (Prg_Id)
        {
            case "F50103":

                break;
            case "HSNCODE":
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select trim(type1) as TYPE1,UPPER(TRIM(ACREF)) AS ACREF from typegrp where id='T1' ORDER BY TYPE1");
                save_it = "Y";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[1].Text.Trim().ToUpper() == "" || gr.Cells[1].Text.Trim().ToUpper() == "-")
                    {
                        save_it = "N";
                        gr.BackColor = Color.LightPink;
                    }
                    col1 = fgen.seek_iname_dt(dt, "ACREF='" + gr.Cells[1].Text.Trim().ToUpper() + "'", "ACREF");
                    if (col1 != "0")
                    {
                        save_it = "N";
                        gr.BackColor = Color.LightPink;
                    }
                }

                if (save_it == "N")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Red Colour Rows Indicate : Repeat / Missing HSN, Please Correct these and upload file again!!");
                    return;
                }
                break;
            case "BSSCH_UPL":
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select trim(type1) as TYPE1,UPPER(TRIM(type1)) AS ACREF from typegrp where id='A' ORDER BY TYPE1");
                save_it = "Y";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[1].Text.Trim().ToUpper() == "" || gr.Cells[1].Text.Trim().ToUpper() == "-")
                    {
                        save_it = "N";
                        gr.BackColor = Color.LightPink;
                    }
                    col1 = fgen.seek_iname_dt(dt, "ACREF='" + gr.Cells[1].Text.Trim().ToUpper() + "'", "ACREF");
                    if (col1 != "0")
                    {
                        save_it = "N";
                        gr.BackColor = Color.LightPink;
                    }
                }

                if (save_it == "N")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Red Colour Rows Indicate : Repeat / Missing HSN, Please Correct these and upload file again!!");
                    return;
                }
                break;
            case "ACOPBAL":
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select TRIM(ACODE) AS ACODE,ANAME FROM FAMST");
                save_it = "Y";

                foreach (GridViewRow gr in sg1.Rows)
                {
                    full_acode = gr.Cells[1].Text.Trim().ToUpper();


                    if (gr.Cells[1].Text.Trim().ToUpper() == "" || gr.Cells[1].Text.Trim().ToUpper() == "-")
                    {
                        save_it = "N";
                        gr.BackColor = Color.LightPink;
                    }
                    col1 = fgen.seek_iname_dt(dt, "ACODE='" + gr.Cells[1].Text.Trim().ToUpper() + "'", "ACODE");
                    if (col1 == "0")
                    {
                        save_it = "N";
                        gr.BackColor = Color.LightPink;
                    }
                }
                if (save_it == "N")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Red Colour Rows Indicate : Missing / Wrong Acode, Please Correct these and upload file again!!");
                    return;
                }
                break;
            case "BILLWISEOUTCR":
            case "BILLWISEOUTDR":
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select TRIM(ACODE) AS ACODE,ANAME FROM FAMST");
                save_it = "Y";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    if (gr.Cells[1].Text.Trim().ToUpper() == "" || gr.Cells[1].Text.Trim().ToUpper() == "-")
                    {
                        save_it = "N";
                        gr.BackColor = Color.LightPink;
                    }
                    col1 = fgen.seek_iname_dt(dt, "ACODE='" + gr.Cells[1].Text.Trim().ToUpper() + "'", "ACODE");
                    if (col1 == "0")
                    {
                        save_it = "N";
                        gr.BackColor = Color.LightPink;
                    }
                }
                if (save_it == "N")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Red Colour Rows Indicate : Missing / Wrong Acode, Please Correct these and upload file again!!");
                    return;
                }
                break;


            case "SALEORDER":
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select trim(Acode)||trim(icode)||trim(pordno) as fstr from somas where branchcd='" + frm_mbr + "' order by trim(Acode)||trim(icode)||trim(pordno)");
                save_it = "Y";
                foreach (GridViewRow gr in sg1.Rows)
                {
                    fstr = gr.Cells[4].Text.Trim().ToUpper() + gr.Cells[5].Text.Trim().ToUpper() + gr.Cells[9].Text.Trim().ToUpper();
                    if (gr.Cells[1].Text.Trim().ToUpper() == "" || gr.Cells[1].Text.Trim().ToUpper() == "-")
                    {
                        save_it = "N";
                        gr.BackColor = Color.LightPink;
                    }
                    if (gr.Cells[4].Text.Trim().ToUpper() == "" || gr.Cells[4].Text.Trim().ToUpper() == "-")
                    {
                        save_it = "N";
                        gr.BackColor = Color.LightPink;
                    }
                    if (gr.Cells[5].Text.Trim().ToUpper() == "" || gr.Cells[5].Text.Trim().ToUpper() == "-")
                    {
                        save_it = "N";
                        gr.BackColor = Color.LightPink;
                    }
                    col1 = fgen.seek_iname_dt(dt, "fstr='" + fstr + "'", "fstr");
                    if (col1 != "0")
                    {
                        save_it = "N";
                        gr.BackColor = Color.LightPink;
                    }
                }
                if (save_it == "N")
                {
                    fgen.msg("-", "AMSG", "Dear " + frm_uname + ",Red Colour Rows Indicate : Missing / Duplicate / Wrong Sales Order Detail, Please Correct these and upload file again!!");
                    return;
                }
                break;
            case "WIPSTKOP":
                dtdist = dtn.DefaultView.ToTable(true, "Item Code");
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select TRIM(icode) AS ICODe,INAME FROM item order by trim(icode)");

                for (int i = 0; i < dtdist.Rows.Count; i++)
                {
                    col1 = "";
                    //SQuery = "SELECT ICODE FROM ITEM WHERE BRANCHCD='" + frm_mbr + "' AND TRIM(ICODE)='" + dtdist.Rows[i]["Item Code"].ToString().Trim() + "'";
                    col1 = fgen.seek_iname_dt(dt, "ICODE='" + dtdist.Rows[i]["Item Code"].ToString().Trim() + "'", "ICODE");
                    //col1 = fgen.seek_inameDt(frm_qstr, frm_cocd, "SELECT ICODE FROM ITEM WHERE BRANCHCD='" + frm_mbr + "' AND TRIM(ICODE)='" + dtdist.Rows[i]["Item Code"].ToString().Trim() + "' ", "");
                    if (col1 == "0")
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Item Master Missing/Incorrect in the excel file. Please correct! ");
                        return;
                    }
                }
                break;
            case "STORESTKBATCHWISE":
                dtdist = dtn.DefaultView.ToTable(true, "Item Code");

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select TRIM(icode) AS ICODe,ANAME FROM item order by trim(icode)");
                for (int i = 0; i < dtdist.Rows.Count; i++)
                {
                    col1 = "";
                    col1 = fgen.seek_iname_dt(dt, "ICODE='" + dtdist.Rows[i]["Item Code"].ToString().Trim() + "'", "ICODE");
                    //fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ICODE FROM ITEM WHERE BRANCHCD='" + frm_mbr + "' TRIM(ICODE)='" + dtdist.Rows[i]["Item Code"].ToString().Trim() + "' ", "");
                    if (col1 == "0")
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Item Master Missing/Incorrect in the excel file. Please correct! ");
                        return;
                    }
                }
                break;
            case "REELSTOCK":
                dtdist = dtn.DefaultView.ToTable(true, "Item Code");

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select TRIM(icode) AS ICODe,ANAME FROM item order by trim(icode)");
                for (int i = 0; i < dtdist.Rows.Count; i++)
                {
                    col1 = "";
                    //col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ICODE FROM ITEM WHERE BRANCHCD='" + frm_mbr + "' TRIM(ICODE)='" + dtdist.Rows[i]["Item Code"].ToString().Trim() + "' ", "");
                    col1 = fgen.seek_iname_dt(dt, "ICODE='" + dtdist.Rows[i]["Item Code"].ToString().Trim() + "'", "ICODE");
                    if (col1 == "0")
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Item Master Missing/Incorrect in the excel file. Please correct! ");
                        return;
                    }
                }
                break;
            case "BOM":
                dtdist = dtn.DefaultView.ToTable(true, "Parent Code", "Child Code");
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select TRIM(icode) AS ICODe,ANAME FROM item order by trim(icode)");

                for (int i = 0; i < dtdist.Rows.Count; i++)
                {
                    col1 = fgen.seek_iname_dt(dt, "ICODE='" + dtdist.Rows[i]["Parent Code"].ToString().Trim() + "'", "ICODE");
                    col2 = fgen.seek_iname_dt(dt, "ICODE='" + dtdist.Rows[i]["Child Code"].ToString().Trim() + "'", "ICODE");
                    //col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(Acode)||trim(po_no)||trim(PO_Line_No) as fstr,VCHNUM||'-'||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS DOCNO from impl_powork where trim(Acode)||trim(po_no)||trim(PO_Line_No) = '" + dtdist.Rows[i]["Parent Code"].ToString().Trim() + "' ", "");

                    //col2 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(Acode)||trim(po_no)||trim(PO_Line_No) as fstr,VCHNUM||'-'||TO_CHAR(VCHDATE,'DD/MM/YYYY') AS DOCNO from impl_powork where trim(Acode)||trim(po_no)||trim(PO_Line_No) = '" + dtdist.Rows[i]["Child Code"].ToString().Trim() + "' ", "");

                    if (col1 == "0" || col2 == "0")
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Master Missing/Incorrect in the excel file. Please correct! ");
                        return;
                    }


                }

                break;
            case "INWQCTEMP":
            case "OUTQCTEMP":
                dtdist = dtn.DefaultView.ToTable(true, "Parent Code", "Child Code");

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select TRIM(icode) AS ICODe,INAME FROM item order by trim(icode)");
                for (int i = 0; i < dtdist.Rows.Count; i++)
                {

                    //col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(acode) as fstr,upper(trim(aname)) as aname from famst where length(Trim(acode))>4 and trim(Acode)||trim(po_no)||trim(PO_Line_No) = '" + dtdist.Rows[i]["Parent Code"].ToString().Trim() + "' ", "");

                    //col2 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(acode) as fstr,upper(trim(aname)) as aname from famst where length(Trim(acode))>4 and trim(Acode)||trim(po_no)||trim(PO_Line_No) = '" + dtdist.Rows[i]["Child Code"].ToString().Trim() + "' ", "");
                    col1 = fgen.seek_iname_dt(dt, "ICODE='" + dtdist.Rows[i]["Icode"].ToString().Trim() + "'", "ICODE");

                    if (col1 == "0")
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Master Missing/Incorrect in the excel file. Please correct! ");
                        return;
                    }


                }

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, "select TRIM(Acode) AS ACODe,ANAME FROM famst order by trim(acode)");
                for (int i = 0; i < dtdist.Rows.Count; i++)
                {

                    //col1 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(acode) as fstr,upper(trim(aname)) as aname from famst where length(Trim(acode))>4 and trim(Acode)||trim(po_no)||trim(PO_Line_No) = '" + dtdist.Rows[i]["Parent Code"].ToString().Trim() + "' ", "");

                    //col2 = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(acode) as fstr,upper(trim(aname)) as aname from famst where length(Trim(acode))>4 and trim(Acode)||trim(po_no)||trim(PO_Line_No) = '" + dtdist.Rows[i]["Child Code"].ToString().Trim() + "' ", "");
                    col1 = fgen.seek_iname_dt(dt, "ACODE='" + dtdist.Rows[i]["Acode"].ToString().Trim() + "'", "ACODE");

                    if (col1 == "0")
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Master Missing/Incorrect in the excel file. Please correct! ");
                        return;
                    }


                }

                break;

            case "F25108":
                dtdist = dtn.DefaultView.ToTable(true, "INVNO", "INVDATE");

                for (int i = 0; i < dtdist.Rows.Count; i++)
                {
                    col1 = "";
                    col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT vchnum FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='02' AND TRIM(INVNO)||TO_CHAR(INVDATE,'DD/MM/YYYY')='" + dtdist.Rows[i]["INVNO"].ToString().Trim() + dtdist.Rows[i]["INVDATE"].ToString().Trim() + "' ", "");
                    if (col1.Length > 2)
                    {
                        fgen.msg("-", "AMSG", "Dear " + frm_uname + ", Invoice no. " + dtdist.Rows[i]["INVNO"].ToString().Trim() + " is already entered against MRR No. " + col1 + " ");
                        return;
                    }
                }
                break;

        }

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
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
            hffield.Value = "Del_E";
            make_qry_4_popup();
            fgen.Fn_open_sseek("Select " + lblheader.Text + " ", frm_qstr);
        }
        else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form !!");
    }
    //------------------------------------------------------------------------------------
    protected void btnexit_ServerClick(object sender, EventArgs e)
    {
        if (Session["mymst"] != null)
        {
            if (Session["mymst"].ToString() == "Y")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup();", true);
                Session["mymst"] = null;
            }
        }
        else
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

        //hffield.Value = "List";
        //fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {
        //SQuery = "select substr(to_Char(a.vchdate,'MONTH'),1,3) as Month_Name,sum(Dramt)-sum(Cramt) as tot_bas,sum(Dramt)-sum(cramt) as tot_qty,to_Char(a.vchdate,'YYYYMM') as mth from voucher a where a.vchdate " + DateRange + " and type like '%' and substr(acode,1,2) IN('16') group by to_Char(a.vchdate,'YYYYMM') ,substr(to_Char(a.vchdate,'MONTH'),1,3) order by to_Char(a.vchdate,'YYYYMM')";
        //fgen.Fn_FillChart(frm_cocd, frm_qstr, "Testing Chart", "line", "Main Heading", "Sub Heading", SQuery, "");
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
                switch (Prg_Id)
                {
                    case "F50103":
                        // Deleing data from Main Table                        
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from WEB_IMPORT a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                        // Deleing data from Sr Ctrl Table                                
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'  ");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (select DISTINCT a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr from IVOUCHER A WHERE A.BRANCHCD||TRIM(A.REFNUM)||TO_CHAR(A.REFDATE,'DD/MM/YYYY')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' AND A.TYPE LIKE '4%' )");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (select DISTINCT a.branchcd||a.type||trim(a.ORDNO)||to_Char(a.ORDDT,'dd/mm/yyyy') as fstr from SOMAS A WHERE A.BRANCHCD||TRIM(A.INVNO)||TO_CHAR(A.INVDATE,'DD/MM/YYYY')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' AND A.TYPE LIKE '4%' )");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from SALE a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (select DISTINCT a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr from IVOUCHER A WHERE A.BRANCHCD||TRIM(A.REFNUM)||TO_CHAR(A.REFDATE,'DD/MM/YYYY')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' AND A.TYPE LIKE '4%')");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from VOUCHER a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (select DISTINCT a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr from IVOUCHER A WHERE A.BRANCHCD||TRIM(A.REFNUM)||TO_CHAR(A.REFDATE,'DD/MM/YYYY')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' AND A.TYPE LIKE '4%')");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from SOMAS a where a.branchcd||trim(a.INVNO)||to_Char(a.INVDATE,'dd/mm/yyyy')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' AND A.TYPE LIKE '4%' ");
                        // Deleing data from ivoucher Table
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivoucher a where A.BRANCHCD||TRIM(A.REFNUM)||TO_CHAR(A.REFDATE,'DD/MM/YYYY')='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' AND A.TYPE LIKE '4%' ");
                        break;
                    default:
                        // Deleing data from Main Table
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||a.type||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                        // Deleing data from Sr Ctrl Table                                
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'  ");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') in (select DISTINCT a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr from IVOUCHER A WHERE A.BRANCHCD||TRIM(A.BTCHNO)||TRIM(A.BTCHDT)='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "')");
                        // Deleing data from ivoucher Table
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from ivoucher a where A.BRANCHCD||TRIM(A.BTCHNO)||TRIM(A.BTCHDT)='" + frm_mbr + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "' ");
                        break;
                }

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
                case "LIST_E":

                    break;
                case "New":
                    #region
                    if (col1 == "") return;
                    frm_vty = col1;
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", col1);
                    lbl1a.Text = col1;
                    if (frm_formID == "HSNCODE" || frm_formID == "BSSCH_UPL")
                    {
                        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='00' AND id='" + frm_vty + "'", 6, "VCH");
                    }
                    else
                    {
                        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(" + doc_nf.Value + ") AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND " + doc_df.Value + " " + DateRange + " ", 6, "VCH");
                    }
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
                    SQuery = "Select a.*,b.Name as TM_Name,c.Name as CL_Name,d.name as Ef_Name from " + frm_tabname + " a,type b,type c,type d where b.id2='TM' and c.id2='CL' and d.id2='TS' and trim(a.acode)=trim(b.type1) and trim(a.icode)=trim(c.type1) and trim(a.wcode)=trim(d.type1) and a.branchcd||a.type||trim(a.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + col1 + "' ORDER BY A.SRNO";
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_FSTR", col1);
                    ViewState["fstr"] = col1;
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                        txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");

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
                case "Print_E":
                    string repname = "mUpl";
                    SQuery = "select a.* from " + frm_tabname + " a where A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') order by A." + doc_nf.Value + " ";

                    SQuery = "SELECT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,B.VCHNUM AS VCH_NO,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,c.aname,c.addr1,c.addr2,c.addr3,C.EMAIL FROM SCRATCH2 A,ivoucher B,famst c WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY') and trim(A.acode)=trim(c.acode) AND A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') and a.num10>0 ORDER BY A.COL33";
                    SQuery = "SELECT distinct A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,b.branchcd||b.type||'-'||trim(B.VCHNUM) as vch_no,TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS VCH_DT,B.TYPE as vch_type,B.BRANCHCD as b_code,TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||A.COL14||A.COL17||(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16))||A.COL3||trim(a.reason) as fstr,a.reason,c.aname,c.addr1,c.addr2,c.addr3,C.EMAIL,B.VCHNUM FROM SCRATCH2 A,IVOUCHER B,FAMST C WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.reason)||trim(a.branchcd)||to_char(a.vchdate,'dd/mm/yyyy')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(B.REVIS_NO)||trim(b.branchcd)||to_char(b.vchdate,'dd/mm/yyyy') AND TRIM(a.ACODE)=TRIM(C.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') and a.num10>0  AND B.TYPE IN ('58','59') ORDER BY B.VCHNUM,A.REASON";
                    //if(frm_cocd == "YTEC") SQuery = "SELECT distinct A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,trim(B.VCHNUM) as vch_no,TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS VCH_DT,B.TYPE as vch_type,B.BRANCHCD as b_code,TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||A.COL14||A.COL17||(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16))||A.COL3||trim(a.reason) as fstr,a.reason,c.aname,c.addr1,c.addr2,c.addr3,C.EMAIL FROM SCRATCH2 A,IVOUCHER B,FAMST C WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.reason)||trim(a.branchcd)||to_char(a.vchdate,'dd/mm/yyyy')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(B.REVIS_NO)||trim(b.branchcd)||to_char(b.vchdate,'dd/mm/yyyy') AND TRIM(a.ACODE)=TRIM(C.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.BRANCHCD||A.TYPE||A." + doc_nf.Value + "||TO_CHAR(A." + doc_df.Value + ",'DD/MM/YYYY') in ('" + frm_mbr + frm_vty + col1 + "') and a.num10>0  AND B.TYPE IN ('58','59') ORDER BY A.REASON";
                    if (frm_cocd == "YTEC") repname = "mUplYTC";
                    fgen.Fn_Print_Report(frm_cocd, frm_qstr, frm_mbr, SQuery, "mUpl", repname);
                    break;
                case "TACODE":
                    txtacode.Value = col1;
                    txtAname.Value = col2;
                    break;
                case "TRCODE":
                    txtRcode.Value = col1;
                    Text2.Value = col2;
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
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
            todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
            SQuery = "SELECT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE FROM SCRATCH2 A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + DateRange + " and a.num10>0 ORDER BY A.COL33";
            SQuery = "SELECT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code FROM SCRATCH2 A,ivoucher B WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(b.location)||to_char(b.vchdate,'dd/mm/yyyy') AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and b.type in ('58','59') and a.num10>0 ORDER BY A.COL33";

            //SQuery = "SELECT distinct A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code FROM SCRATCH2 A,ivoucher B WHERE TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.col14)||trim(a.col3)||a.branchcd=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(b.location)||to_char(b.vchdate,'dd/mm/yyyy')||b.iqty_chl||trim(b.finvno)||b.branchcd AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND a.vchdate " + PrdRange + " and b.type in ('58','59') and a.num10>0 order by a.col33 ";
            //corr
            SQuery = "select DISTINCT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,a.DIFF AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,a.TOTAL AS TOTAL,A.COL29 AS HSCODE,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code from (SELECT distinct a.acode,a.vchdate,a.icode,A.COL33,A.COL34 ,A.COL35 ,A.COL2 ,A.COL1 ,A.COL3 ,A.COL12 ,A.COL22 ,A.COL13 ,A.COL14 ,A.COL16 ,A.COL26 ,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 ,A.COL18 ,A.COL19 ,A.COL20 ,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 FROM SCRATCH2 A  WHERE a.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and a.num10>0 ) a, ivoucher b where TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(b.location)||to_char(b.vchdate,'dd/mm/yyyy') and b.type in ('58','59') order by a.col33";

            //SQuery = "select DISTINCT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,a.DIFF AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,a.TOTAL AS TOTAL,A.COL29 AS HSCODE,a.col46 as vch_no,TO_CHAR(a.VCHDATE,'DD/MM/YYYY') AS VCH_DT from (SELECT distinct a.acode,a.vchdate,a.icode,A.COL33,A.COL34 ,A.COL35 ,A.COL2 ,A.COL1 ,A.COL3 ,A.COL12 ,A.COL22 ,A.COL13 ,A.COL14 ,A.COL16 ,A.COL26 ,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 ,A.COL18 ,A.COL19 ,A.COL20 ,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 FROM SCRATCH2 A  WHERE a.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and a.num10>0 ) a order by a.col33";
            SQuery = "select DISTINCT A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,a.DIFF AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,a.TOTAL AS TOTAL,A.COL29 AS HSCODE,(case when b.type='59' then B.VCHNUM else '-' end) as dr_note,(case when b.type='58' then B.VCHNUM else '-' end) as cr_note, TO_CHAR(b.VCHDATE,'DD/MM/YYYY') AS VCH_DT,b.type as vch_type,b.branchcd as b_code from (SELECT distinct a.acode,a.vchdate,a.icode,A.COL33,A.COL34 ,A.COL35 ,A.COL2 ,A.COL1 ,A.COL3 ,A.COL12 ,A.COL22 ,A.COL13 ,A.COL14 ,A.COL16 ,A.COL26 ,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 ,A.COL18 ,A.COL19 ,A.COL20 ,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 FROM SCRATCH2 A  WHERE a.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and a.num10>0 ) a, ivoucher b where TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||TRIM(A.COL14)=TRIM(B.ACODE)||TRIM(B.ICODE)||TRIM(B.INVNO)||TO_CHAR(B.INVDATE,'DD/MM/YYYY')||trim(b.location)||to_char(b.vchdate,'dd/mm/yyyy')||TRIM(B.IQTY_CHL) and b.type in ('58','59') order by a.col33";

            SQuery = "SELECT distinct A.COL33 AS BATCH_NO,A.COL34 AS SERIAL_NO,A.COL35 AS BATCH_DATE,A.COL2 AS PART_NO,A.COL1 AS PART_NAME,A.COL3 AS PO_NO,A.COL12 AS INVNO,A.COL22 AS INV_DATE,A.COL13 AS SRV_NO,A.COL14 AS QTY,A.COL16 AS OLD_RATE,A.COL26 AS NEW_RATE,(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16)) AS DIFF,A.COL17 AS BASIC_AMT,A.COL18 AS CGST,A.COL19 AS SGST,A.COL20 AS IGST,(TO_NUMBER(A.COL17)+TO_NUMBER(A.COL18)+TO_NUMBER(A.COL19)+TO_NUMBER(A.COL20)) AS TOTAL,A.COL29 AS HSCODE,'------' as dr_note,'------' as cr_note,'----------' AS VCH_DT,'--' as vch_type,'--' as b_code,TRIM(A.ACODE)||TRIM(a.ICODE)||TRIM(A.COL12)||TO_CHAR(TO_DATE(A.COL22,'DD/MM/YY'),'DD/MM/YYYY')||trim(a.col33)||to_char(a.vchdate,'dd/mm/yyyy')||A.COL14||A.COL17||(TO_NUMBER(A.COL26)-TO_NUMBER(A.COL16))||A.COL3 as fstr FROM SCRATCH2 A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='ZZ' AND A.VCHDATE " + PrdRange + " and a.num10>0 ORDER BY A.COL33";


            //fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
            //fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
            //hffield.Value = "-";
            //return;

            DataTable dtList = new DataTable();
            dtList = fgen.getdata(frm_qstr, frm_cocd, SQuery);

            dt = new DataTable();
            dt2 = new DataTable();

            if (dtList.Rows.Count <= 0) return;

            DataView dv = new DataView(dtList);
            dt = dv.ToTable(true, "BATCH_NO");
            col3 = "'-'";
            foreach (DataRow dr in dt.Rows)
            {
                col3 += ",'" + dr["batch_no"] + "'";
            }

            dt2 = fgen.getdata(frm_qstr, frm_cocd, "SELECT distinct branchcd||'~'||type||'~'||trim(vchnum)||'~'||to_char(Vchdate,'dd/mm/yyyy') as Vch,type,branchcd,TRIM(ACODE)||TRIM(ICODE)||TRIM(INVNO)||TO_CHAR(INVDATE,'DD/MM/YYYY')||trim(location)||to_char(vchdate,'dd/mm/yyyy')||iqty_chl||iamount||irate||finvno fstr FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE in ('58','59') AND VCHDATE " + DateRange + " AND STORE='N' and location in (" + col3 + ") ORDER BY vch");

            dt = new DataTable();
            dt.Columns.Add("fstr", typeof(string));
            dt.Columns.Add("col1", typeof(string));
            oporow = null;
            string mhd = "";
            col1 = "";
            foreach (DataRow dr in dtList.Rows)
            {
                col2 = dr["invno"].ToString().Trim();
                do
                {
                    mhd = fgen.seek_iname_dt(dt2, "fstr='" + dr["fstr"].ToString().Trim() + "' and vch<>'" + col1 + "'", "vch");
                    col1 = fgen.seek_iname_dt(dt, "col1='" + mhd + "'", "col1");
                }
                while (mhd == col1 && col1.Length > 2);

                {
                    col1 = mhd;
                    if (mhd.Contains("~"))
                    {
                        dr["b_code"] = mhd.Split('~')[0].ToString().Trim();
                        dr["vch_type"] = mhd.Split('~')[1].ToString().Trim();
                        if (mhd.Split('~')[1].ToString().Trim() == "58") dr["cr_note"] = mhd.Split('~')[2].ToString().Trim();
                        else dr["dr_note"] = mhd.Split('~')[2].ToString().Trim();
                        dr["vch_dt"] = mhd.Split('~')[3].ToString().Trim();

                        oporow = dt.NewRow();
                        oporow["fstr"] = dr["fstr"].ToString().Trim();
                        oporow["col1"] = col1;
                        dt.Rows.Add(oporow);
                    }
                }
            }
            if (dtList.Rows.Count > 0) dtList.Columns.Remove("fstr");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
            Session["send_dt"] = dtList;
            fgen.Fn_open_rptlevel("List of " + lblheader.Text.Trim() + " for the Period of " + fromdt + " to " + todt, frm_qstr);
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

            }

            //-----------------------------
            i = 0;
            hffield.Value = "";

            setColHeadings();

            #region Number Gen and Save to Table
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y" && Checked_ok == "Y")
            {
                //try
                {
                    if (frm_formID == "SALEORDER") uploadSaleOrderbyExcel();
                    else
                    {
                        //-
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
                            save_it = "Y";

                            if (save_it == "Y" && frm_formID != "ACOPBAL")
                            {
                                i = 0;
                                do
                                {
                                    string ho_br = "";
                                    ho_br = frm_mbr;
                                    switch (frm_formID)
                                    {
                                        case "BSSCH_UPL":
                                        case "HSNCODE":
                                            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='00' and ID='" + frm_vty + "'", 6, "vch");
                                            ho_br = "00";
                                            break;

                                        default:
                                            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                                            break;
                                    }

                                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + ho_br + frm_vty + frm_vnum + frm_CDT1, ho_br, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
                                    if (i > 10)
                                    {
                                        fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");

                                        switch (frm_formID)
                                        {
                                            case "BSSCH_UPL":
                                            case "HSNCODE":
                                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='00' and ID='" + frm_vty + "'", 6, "vch");
                                                ho_br = "00";
                                                break;

                                            default:
                                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                                                break;
                                        }


                                        //frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + 0 + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
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

                        save_fun();
                        ViewState["refNo"] = frm_vnum;


                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        if (Prg_Id == "BILLWISEOUTDR" || Prg_Id == "BILLWISEOUTCR")
                        {
                            string code_grp = "";
                            if (Prg_Id == "BILLWISEOUTDR") { code_grp = "16"; }
                            if (Prg_Id == "BILLWISEOUTCR") { code_grp = "06"; }


                            cmd_query = "delete from famstbal where branchcd='" + frm_mbr + "' and trim(Acode) in (Select trim(Acode) from recebal where branchcd='" + frm_mbr + "' and substr(acode,1,2)='" + code_grp + "')";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);


                            cmd_query = "insert into famstbal(branchcd,acode,br_acode,Yr_" + frm_myear + ")(select branchcd,trim(acode),branchcd||trim(acodE),sum(dramt)-sum(Cramt) as diff from recebal where branchcd='" + frm_mbr + "' and length(trim(acode))>=6 and substr(Acode,1,2) in ('" + code_grp + "') group by branchcd,trim(acode),branchcd||trim(acodE))";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }


                        save_fun2();

                        //--
                    }
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
                    fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl(); clearctrl();
                    sg1.DataSource = null;
                    sg1.DataBind();

                }
                //catch (Exception ex)
                //{
                //    fgen.FILL_ERR(frm_uname + " --> " + ex.Message.ToString().Trim() + " ==> " + frm_PageName + " ==> In Save Function");
                //    fgen.msg("-", "AMSG", ex.Message.ToString());
                //    col1 = "N";
                //}
                set_Val();
            #endregion
            }
        }
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
        string full_acode = "";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        vardate = fgen.seek_iname(frm_qstr, frm_cocd, "select sysdate as ldt from dual", "ldt");
        DataTable dtW = (DataTable)ViewState["dtn"];

        int code_len = 6;
        string digit7code = "N";
        digit7code = fgen.getOption(frm_qstr, frm_cocd, "W0090", "OPT_ENABLE");
        if (digit7code == "Y")
        {
            code_len = 7;
        }


        int curr_max = 0;

        int srno = 0; int vchn = 0;

        if (frm_formID == "HSNCODE" || frm_formID == "BOM" || frm_formID == "INWQCTEMP" || frm_formID == "OUTQCTEMP")
        {
            srno = 1;
            vchn = Convert.ToInt32(txtvchnum.Text.ToString());

        }
        else
        {
            srno = 0;
        }

        switch (frm_formID)
        {
            case "BSSCH_UPL":
            case "HSNCODE":
                SQuery = "select max(" + "type1" + ") as vch from " + frm_tabname + " where branchcd='00' and ID='" + frm_vty + "' ";
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, SQuery, 3, "vch");
                vchn = Convert.ToInt32(frm_vnum);
                break;
        }

        string code_vch = "";
        string code_grp = "";

        int ctr = 0;

        if (dtW != null)
        {
            DataView dvW = new DataView(dtW);
            dtW = new DataTable();
            dtW = dvW.ToTable();
            char pad = '0';

            foreach (DataRow gr1 in dtW.Rows)
            {
                switch (Prg_Id)
                {
                    case "F50103":
                        oporow = oDS.Tables[0].NewRow();
                        oporow["branchcd"] = frm_mbr;
                        oporow["TYPE"] = frm_vty;

                        oporow["UBRANCHCD"] = gr1[1].ToString().PadLeft(2, '0');
                        oporow["UTYPE"] = gr1[0].ToString().ToUpper();

                        oporow["VCHNUM"] = frm_vnum;
                        oporow["VCHDATE"] = txtvchdate.Text;

                        oporow["ORD_NO"] = (gr1[3].ToString().Trim() == null || gr1[3].ToString().Trim().Length <= 0) ? "-" : gr1[3].ToString().Trim();
                        oporow["ORD_DT"] = fgen.make_def_Date(gr1[2].ToString().Trim(), vardate);

                        full_acode = gr1[16].ToString().Trim().PadLeft(code_len, pad).ToUpper();
                        oporow["ACODE"] = full_acode;
                        oporow["ICODE"] = gr1[9].ToString().Trim();
                        oporow["ITEM_NAME"] = gr1[10].ToString().Trim();
                        oporow["QTY"] = gr1[11].ToString().Trim();
                        oporow["MRP"] = Math.Round(gr1[12].ToString().Trim().toDouble() / gr1[11].ToString().Trim().toDouble(), 2);
                        oporow["AMT"] = gr1[12].ToString().Trim();

                        oporow["INV_NO"] = gr1[13].ToString().Trim();
                        oporow["INV_DT"] = Convert.ToDateTime(gr1[14].ToString().Trim().Length < 4 ? vardate : gr1[14].ToString().Replace("&nbsp;", "").Trim()).ToString("dd/MM/yyyy");

                        oporow["BTCHNO"] = gr1[15].ToString().Replace("&nbsp;", "").Trim();

                        oporow["CUST_NAME"] = gr1[17].ToString().Replace("&nbsp;", "-").Trim();
                        oporow["CUST_MOB"] = gr1[8].ToString().Replace("&nbsp;", "-").Trim();
                        oporow["CUST_ADD1"] = gr1[7].ToString().Replace("&nbsp;", "-").Trim();
                        oporow["CUST_ADD2"] = gr1[4].ToString().Replace("&nbsp;", "-").Trim();
                        oporow["CUST_CITY"] = gr1[5].ToString().Replace("&nbsp;", "-").Trim();
                        oporow["CUST_ST"] = gr1[6].ToString().Replace("&nbsp;", "-").Trim();
                        oporow["ENT_BY"] = frm_uname;
                        oporow["ENT_DT"] = vardate;

                        oporow["SRNO"] = srno;
                        srno++;
                        oDS.Tables[0].Rows.Add(oporow);
                        break;
                    case "HSNCODE":
                        col1 = fgen.seek_iname_dt(oDS.Tables[0], "ACREF='" + gr1[1].ToString().Trim().ToUpper() + "'", "ACREF");
                        if (col1 == "0")
                        {
                            #region HSN Code Upload Saving
                            oporow = oDS.Tables[0].NewRow();
                            oporow["BRANCHCD"] = "00";
                            oporow["ID"] = frm_vty;
                            oporow["TYPE1"] = fgen.padlc(vchn, 3);

                            oporow["vchnum"] = fgen.padlc(vchn, 6);
                            oporow["vchdate"] = txtvchdate.Text.Trim();

                            oporow["acref"] = gr1[1].ToString().Trim().ToUpper();
                            oporow["Name"] = gr1[2].ToString();


                            oporow["num4"] = fgen.make_double(gr1[3].ToString());
                            oporow["num5"] = fgen.make_double(gr1[4].ToString());
                            oporow["num6"] = fgen.make_double(gr1[5].ToString());


                            oporow["acref3"] = "-";
                            oporow["acref4"] = "-";
                            oporow["acref5"] = "-";
                            oporow["acref6"] = "-";
                            oporow["NUM7"] = Convert.ToDecimal(0);
                            oporow["NUM8"] = Convert.ToDecimal(0);
                            oporow["NUM9"] = Convert.ToDecimal(0);
                            oporow["NUM10"] = Convert.ToDecimal(0);
                            oporow["EDT_DT"] = DateTime.Now.ToString();
                            oporow["EDT_BY"] = "-";

                            oporow["dpt"] = gr1[6].ToString();
                            oporow["acref2"] = gr1[7].ToString();
                            oporow["ENT_BY"] = frm_uname;
                            oporow["ENT_DT"] = vardate;

                            oporow["SRNO"] = srno;
                            srno++;
                            vchn++;
                            oDS.Tables[0].Rows.Add(oporow);
                            #endregion
                        }
                        break;
                    case "BSSCH_UPL":
                        col1 = fgen.seek_iname_dt(oDS.Tables[0], "NAME='" + gr1[1].ToString().Trim().ToUpper() + "'", "NAME");
                        if (col1 == "0")
                        {
                            #region HSN Code Upload Saving
                            oporow = oDS.Tables[0].NewRow();
                            oporow["BRANCHCD"] = "00";
                            oporow["ID"] = frm_vty;
                            oporow["TYPE1"] = gr1[1].ToString().Trim().ToUpper();

                            oporow["vchnum"] = fgen.padlc(vchn, 6);
                            oporow["vchdate"] = fgen.make_def_Date(txtvchdate.Text.Trim(), vardate);

                            oporow["Name"] = gr1[2].ToString();
                            oporow["acref"] = "-";

                            oporow["num4"] = 0;
                            oporow["num5"] = 0;
                            oporow["num6"] = 0;


                            oporow["acref3"] = "-";
                            oporow["acref4"] = "-";
                            oporow["acref5"] = "-";
                            oporow["acref6"] = "-";
                            oporow["NUM7"] = 0;
                            oporow["NUM8"] = 0;
                            oporow["NUM9"] = 0;
                            oporow["NUM10"] = 0;
                            oporow["EDT_DT"] = DateTime.Now.ToString();
                            oporow["EDT_BY"] = "-";

                            oporow["dpt"] = "-";
                            oporow["acref2"] = "-";
                            oporow["ENT_BY"] = frm_uname;
                            oporow["ENT_DT"] = vardate;

                            oporow["SRNO"] = srno;
                            srno++;
                            vchn++;
                            oDS.Tables[0].Rows.Add(oporow);
                            #endregion
                        }
                        break;
                    case "ACOPBAL":
                        string chk_item = "";

                        full_acode = gr1[1].ToString().Trim().PadLeft(code_len, pad).ToUpper();

                        chk_item = fgen.seek_iname(frm_qstr, frm_cocd, "select acode from famstbal where branchcd='" + frm_mbr + "' and trim(acode)='" + full_acode + "'", "acode");
                        if (chk_item == "0")
                        {
                            oporow = oDS.Tables[0].NewRow();
                            oporow["yr_2017"] = 0;
                            oporow["yr_2018"] = 0;
                            oporow["yr_2019"] = 0;
                            oporow["branchcd"] = frm_mbr;
                            oporow["acode"] = full_acode;
                            oporow["BR_ACODe"] = frm_mbr + full_acode;
                            oporow["Yr_" + Convert.ToDateTime(frm_CDT1).ToString("yyyy")] = fgen.make_double(gr1[4].ToString());
                            oDS.Tables[0].Rows.Add(oporow);
                        }
                        else
                        {
                            cmd_query = "update famstbal set yr_" + Convert.ToDateTime(frm_CDT1).ToString("yyyy") + " = " + gr1[4].ToString().toDouble() + " where branchcd='" + frm_mbr + "' and trim(acode)='" + full_acode + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }

                        break;
                    case "BILLWISEOUTDR":
                    case "BILLWISEOUTCR":
                        code_vch = "000001";
                        code_grp = "16";
                        if (Prg_Id == "BILLWISEOUTCR")
                        {
                            code_vch = "000002";
                            code_grp = "05";
                        }
                        full_acode = gr1[1].ToString().Trim().PadLeft(code_len, pad).ToUpper();

                        cmd_query = "delete from recebal where branchcd='" + frm_mbr + "' and substr(acode,1,2)='" + code_grp + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                        oporow = oDS.Tables[0].NewRow();
                        oporow["BRANCHCD"] = frm_mbr;
                        oporow["TYPE"] = "30";
                        oporow["VCHNUM"] = code_vch;
                        oporow["VCHDATE"] = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                        oporow["acode"] = full_acode;
                        oporow["INVNO"] = gr1[2].ToString();
                        oporow["refnum"] = "-";
                        oporow["INVDATE"] = gr1[3].ToString();
                        oporow["VCHID"] = 0;
                        oporow["dramt"] = fgen.make_double(gr1[4].ToString());
                        oporow["cramt"] = fgen.make_double(gr1[5].ToString());

                        oporow["fcdramt"] = fgen.make_double(gr1[6].ToString());
                        oporow["fccramt"] = fgen.make_double(gr1[7].ToString());

                        oporow["fc_type"] = gr1[8].ToString();
                        oporow["FCTYPE"] = gr1[8].ToString();
                        oporow["fcrate"] = fgen.make_double(gr1[9].ToString());


                        oporow["naration"] = gr1[10].ToString();

                        oporow["ent_by"] = frm_uname;
                        oporow["ent_Date"] = vardate;
                        oDS.Tables[0].Rows.Add(oporow);
                        break;

                    case "ITEMWISEOPBL":
                        ctr = ctr + 1;
                        oporow = oDS.Tables[0].NewRow();
                        oporow["BRANCHCD"] = frm_mbr;
                        oporow["Type"] = "00";
                        oporow["VCHNUM"] = txtvchnum.Text.Trim();
                        oporow["VCHDATE"] = txtvchdate.Text.Trim();

                        oporow["icode"] = gr1[1].ToString();
                        oporow["iqtyin"] = gr1[4].ToString();
                        oporow["iqtyout"] = 0;
                        oporow["Srno"] = ctr;
                        oporow["INVNO"] = gr1[5].ToString();

                        oporow["invdate"] = gr1[6].ToString();

                        //oporow["invdate"] = Format(tvchdt.Text, "dd/mm/yyyy")

                        oporow["irate"] = fgen.make_double(gr1[7].ToString());

                        full_acode = gr1[8].ToString().Trim().PadLeft(code_len, pad).ToUpper();
                        oporow["ACODE"] = full_acode;

                        oporow["ent_by"] = frm_uname;
                        oporow["ent_dt"] = vardate;

                        oDS.Tables[0].Rows.Add(oporow);
                        break;
                    case "WIPSTKOP":
                        ctr = ctr + 1;
                        oporow = oDS.Tables[0].NewRow();

                        oporow["BRANCHCD"] = frm_mbr;
                        oporow["Type"] = "50";
                        oporow["VCHNUM"] = txtvchnum.Text.Trim();
                        oporow["VCHDATE"] = txtvchdate.Text.Trim();

                        oporow["icode"] = gr1[1].ToString().PadLeft(8, '0');


                        oporow["iqtyin"] = fgen.make_double(gr1[4].ToString());


                        oporow["NGQTY"] = 0;
                        oporow["Stage"] = gr1[5].ToString();
                        oporow["wolink"] = gr1[6].ToString();
                        oporow["loc_ref"] = gr1[7].ToString();
                        oporow["Srno"] = ctr;

                        oporow["ent_by"] = frm_uname;
                        oporow["ent_dt"] = vardate;
                        oDS.Tables[0].Rows.Add(oporow);
                        break;
                    case "STORESTKBATCHWISE":
                        ctr = ctr + 1;
                        oporow = oDS.Tables[0].NewRow();
                        oporow["BRANCHCD"] = frm_mbr;
                        oporow["Type"] = "00";
                        oporow["VCHNUM"] = txtvchnum.Text.Trim();
                        oporow["VCHDATE"] = txtvchdate.Text.Trim();

                        oporow["icode"] = gr1[1].ToString().PadLeft(8, '0');

                        oporow["iqtyin"] = fgen.make_double(gr1[4].ToString());

                        oporow["iqtyout"] = 0;
                        oporow["Srno"] = ctr;
                        oporow["INVNO"] = gr1[5].ToString();
                        if (gr1[6].ToString().Length >= 1)
                        {
                            oporow["invdate"] = gr1[6].ToString();
                        }
                        else
                        {
                            oporow["invdate"] = DateTime.Now.ToShortDateString();
                        }


                        oporow["irate"] = fgen.make_double(gr1[7].ToString());

                        full_acode = gr1[6].ToString().Trim().PadLeft(code_len, pad).ToUpper();
                        oporow["ACODE"] = full_acode;


                        oporow["ent_by"] = frm_uname;
                        oporow["ent_dt"] = vardate;
                        oDS.Tables[0].Rows.Add(oporow);
                        break;
                    case "REELSTOCK":
                        ctr = ctr + 1;

                        cmd_query = "delete from reelvch_op where branchcd='" + frm_mbr + "'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                        oporow = oDS.Tables[0].NewRow();
                        oporow["BRANCHCD"] = frm_mbr;
                        oporow["Type"] = "02";
                        oporow["VCHNUM"] = gr1[0].ToString().PadLeft(6, '0');
                        oporow["VCHDATE"] = txtvchdate.Text.Trim();
                        oporow["Srno"] = ctr;
                        oporow["rec_iss"] = "D";
                        oporow["reelwout"] = 0;
                        oporow["icode"] = gr1[1].ToString().PadLeft(8, '0');
                        full_acode = gr1[2].ToString().Trim().PadLeft(code_len, pad).ToUpper();
                        oporow["ACODE"] = full_acode;

                        oporow["kclreelno"] = gr1[3].ToString().PadLeft(6, '0');
                        if (gr1[4].ToString().Trim() != "" && gr1[4].ToString().Trim() != "-")
                        {
                            oporow["coreelno"] = gr1[4].ToString();
                        }
                        else
                        {
                            oporow["coreelno"] = "OPENING";
                        }


                        oporow["reelwin"] = fgen.make_double(gr1[5].ToString());
                        oporow["irate"] = fgen.make_double(gr1[6].ToString());

                        oporow["psize"] = gr1[7].ToString();
                        oporow["gsm"] = gr1[8].ToString();
                        if (gr1[9].ToString() == "")
                        {
                            oporow["grade"] = "-";
                        }
                        else
                        {
                            oporow["grade"] = gr1[9].ToString();
                        }

                        if (gr1[10].ToString() == "")
                        {
                            oporow["rlocn"] = "-";
                        }
                        else
                        {
                            oporow["rlocn"] = gr1[10].ToString();
                        }

                        if (gr1[11].ToString() == "")
                        {
                            oporow["reelspec1"] = "-";
                        }
                        else
                        {
                            oporow["reelspec1"] = gr1[11].ToString();
                        }

                        if (gr1[12].ToString() == "")
                        {
                            oporow["reelspec2"] = "-";
                        }
                        else
                        {
                            oporow["reelspec2"] = gr1[12].ToString();
                        }

                        oporow["Unlink"] = "N";
                        oporow["posted"] = "Y";
                        oporow["RINSP_BY"] = frm_uname;
                        oporow["store_no"] = frm_mbr;
                        oDS.Tables[0].Rows.Add(oporow);

                        cmd_query = "update type set reelstkdt='" + Convert.ToDateTime(txtvchdate.Text.Trim()).ToShortDateString() + "' where type1='" + frm_mbr + "' and id='B'";
                        fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);

                        break;
                    case "BOM":

                        ctr = ctr + 1;

                        string curr = "";

                        curr = gr1[4].ToString().Trim();

                        do
                        {



                            oporow = oDS.Tables[0].NewRow();
                            oporow["BRANCHCD"] = frm_mbr;
                            oporow["Type"] = "BM";
                            oporow["VCHNUM"] = vchn;
                            oporow["VCHDATE"] = txtvchdate.Text.Trim();
                            oporow["Srno"] = ctr;
                            oporow["ibrate"] = 0;
                            oporow["icode"] = gr1[4].ToString().Trim();
                            oporow["ibcode"] = gr1[5].ToString().Trim();


                            oporow["ibdiepc"] = fgen.make_double(gr1[6].ToString());
                            oporow["sub_issue_no"] = fgen.make_double(gr1[7].ToString());

                            oporow["ibqty"] = fgen.make_double(gr1[8].ToString());
                            oporow["IBWT"] = fgen.make_double(gr1[9].ToString());
                            if (gr1[10].ToString() == "")
                            {
                                oporow["iomachine"] = "-";
                            }
                            else
                            {
                                oporow["iomachine"] = gr1[10].ToString();
                            }
                            if (gr1[11].ToString() == "")
                            {
                                oporow["ibcat"] = "0RAW";
                            }
                            else
                            {
                                oporow["ibcat"] = gr1[11].ToString();
                            }
                            oporow["ibname"] = "-";
                            oporow["unit"] = "-";
                            oporow["ioqty"] = 0;
                            oporow["irate"] = 0;
                            oporow["iorate"] = 0;
                            oporow["ICOST"] = 0;
                            oporow["iopqty"] = 0;
                            oporow["iclqty"] = 0;
                            oporow["ireceqty"] = 0;
                            oporow["iissuqty"] = 0;
                            oporow["tarrifrate"] = 0;
                            oporow["ibdiepc"] = 0;
                            oporow["cutting_no"] = 0;
                            oporow["freezing_no"] = 0;
                            oporow["MAIN_ISSUE_NO"] = 0;
                            oporow["sub_issue_no"] = 0;
                            oporow["iopr"] = "-";
                            oporow["st_type"] = "-";
                            oporow["acode"] = "-";
                            oporow["icat"] = "-";
                            oporow["istage"] = "-";
                            oporow["ostage"] = "-";
                            oporow["ioname"] = "-";
                            oporow["iname"] = "-";
                            oporow["tarrifno"] = "-";
                            oporow["naration"] = "-";

                            oporow["ent_by"] = "-";
                            oporow["edt_by"] = "-";
                            oporow["cutting_dt"] = DateTime.Now.ToShortDateString();
                            oporow["freezing_dt"] = DateTime.Now.ToShortDateString();
                            oporow["ent_dt"] = DateTime.Now.ToShortDateString();
                            oporow["edt_dt"] = DateTime.Now.ToShortDateString();
                            oDS.Tables[0].Rows.Add(oporow);
                            vchn++;
                        }
                        while (gr1[4].ToString() == curr);
                        break;
                    case "INWQCTEMP":

                        string curr_postr = "";
                        int cntrow = dtW.Rows.Count;
                        curr_postr = gr1[4].ToString() + gr1[5].ToString();

                        do
                        {
                            ctr = ctr + 1;

                            oporow = oDS.Tables[0].NewRow();
                            oporow["BRANCHCD"] = frm_mbr;
                            oporow["Type"] = gr1[1].ToString();
                            oporow["VCHNUM"] = vchn;
                            oporow["VCHDATE"] = txtvchdate.Text.Trim();
                            oporow["Srno"] = ctr;
                            full_acode = gr1[4].ToString().Trim().PadLeft(code_len, pad).ToUpper();
                            oporow["ACODE"] = full_acode;

                            oporow["icode"] = gr1[5].ToString();
                            oporow["Col1"] = gr1[6].ToString();
                            oporow["col2"] = gr1[7].ToString();
                            oporow["col3"] = gr1[8].ToString();
                            oporow["col4"] = gr1[9].ToString();
                            oporow["col5"] = gr1[10].ToString();


                            oporow["ent_by"] = frm_uname;
                            oporow["ent_dt"] = vardate;
                            oporow["edt_by"] = "-";
                            oporow["edt_dt"] = vardate;
                            oDS.Tables[0].Rows.Add(oporow);
                            vchn++;
                        }
                        while (gr1[4].ToString() + gr1[5].ToString() == curr_postr);



                        break;
                    case "OUTQCTEMP":
                        ctr = ctr + 1;
                        curr_postr = gr1[4].ToString() + gr1[5].ToString();

                        do
                        {
                            oporow = oDS.Tables[0].NewRow();
                            oporow["BRANCHCD"] = frm_mbr;
                            oporow["Type"] = gr1[1].ToString();
                            oporow["VCHNUM"] = vchn;
                            oporow["VCHDATE"] = txtvchdate.Text.Trim();
                            oporow["Srno"] = ctr;
                            full_acode = gr1[4].ToString().Trim().PadLeft(code_len, pad).ToUpper();
                            oporow["ACODE"] = full_acode;
                            oporow["icode"] = gr1[5].ToString();
                            oporow["Col1"] = gr1[6].ToString();
                            oporow["col2"] = gr1[7].ToString();
                            oporow["col3"] = gr1[8].ToString();
                            oporow["col4"] = gr1[9].ToString();
                            oporow["col5"] = gr1[10].ToString();


                            oporow["ent_by"] = frm_uname;
                            oporow["ent_dt"] = vardate;
                            oporow["edt_by"] = "-";
                            oporow["edt_dt"] = vardate;
                            oDS.Tables[0].Rows.Add(oporow);
                            vchn++;
                        }
                        while (gr1[4].ToString() + gr1[5].ToString() == curr_postr);
                        break;
                    case "F25108":
                        oporow = oDS.Tables[0].NewRow();
                        oporow["BRANCHCD"] = frm_mbr;
                        oporow["TYPE"] = frm_vty;
                        oporow["vchnum"] = frm_vnum;
                        oporow["vchdate"] = txtvchdate.Text.Trim();
                        oporow["ICODE"] = gr1["icode"].ToString().Trim();
                        oporow["ACODE"] = gr1["Acode"].ToString().Trim();

                        for (int K = 1; K < 10; K++)
                        {
                            oporow["COL" + K] = gr1[K].ToString().Trim();
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
                            oporow["edt_by"] = "-";
                            oporow["eDt_dt"] = vardate;
                        }
                        oDS.Tables[0].Rows.Add(oporow);
                        break;
                }
            }
        }



    }

    void save_fun2()
    {
        DataTable dtW = (DataTable)ViewState["dtn"];
        if (dtW != null)
        {
            switch (Prg_Id)
            {
                case "F50103":
                    #region Invoice Saving
                    {
                        string cgsg = "CG";
                        double cgst = 0, sgst = 0;
                        string hscode = "";
                        double billQty = 1;
                        string refnum = "";
                        if (ViewState["refNo"] != null) refnum = ViewState["refNo"].ToString();
                        double totAmt = 0, basicAmt = 0;
                        string frm_tab_vchr = "VOUCHER";
                        string invoiceDate = "";

                        dtW = new DataTable();
                        dtW = fgen.getdata(frm_qstr, frm_cocd, "SELECT * FROM " + frm_tabname + " WHERE BRANCHCD ='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHNUM='" + refnum + "' AND TO_CHAR(VCHDATE,'DD/MM/YYYY')='" + txtvchdate.Text.Trim() + "' order by srno ");

                        DataView dv = new DataView(dtW, "", "srno", DataViewRowState.CurrentRows);
                        DataTable dtdistDt = dv.ToTable(true, "ORD_NO", "ORD_DT", "INV_NO", "INV_DT", "ACODE", "UTYPE", "CUST_ADD2", "CUST_ADD1", "CUST_ST");

                        string chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select enable_yn as fstr from stock where id='M139'", "fstr");

                        for (int v = 0; v < dtdistDt.Rows.Count; v++)
                        {
                            frm_vty = dtdistDt.Rows[v]["UTYPE"].ToString().ToUpper();
                            invoiceDate = fgen.make_def_Date(Convert.ToDateTime(dtdistDt.Rows[v]["inv_dt"].ToString()).ToString("dd/MM/yyyy"), Convert.ToDateTime(txtvchdate.Text).ToString("dd/MM/yyyy"));

                            string rmToVSave = "Sale Inv.No " + frm_vnum;

                            string myState = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATENM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATENM");
                            string custState = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEN FROM famst WHERE trim(acode)='" + dtdistDt.Rows[v]["ACODE"].ToString().Trim() + "'", "STATEN");

                            if (myState.ToUpper().Trim() == custState.ToUpper().Trim()) cgsg = "CG";
                            else cgsg = "IG";

                            dv = new DataView(dtW, "ORD_NO='" + dtdistDt.Rows[v]["ORD_NO"].ToString().Trim() + "' AND INV_NO='" + dtdistDt.Rows[v]["INV_NO"].ToString().Trim() + "' AND INV_DT='" + invoiceDate + "' AND ACODE='" + dtdistDt.Rows[v]["ACODE"].ToString().Trim() + "'", "srno", DataViewRowState.CurrentRows);
                            i = 0;
                            string doc_is_ok = "";

                            string sonum = "sonum", sodate = vardate;
                            #region somas saving

                            // SOMAS NUMBER
                            if (dv.Count > 0)
                            {
                                frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, "SOMAS", "ORDNO", "ORDDT", frm_mbr, frm_vty, invoiceDate, frm_uname, Prg_Id);
                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                            }
                            totAmt = 0;
                            basicAmt = 0;
                            cgst = 0;
                            sgst = 0;

                            sonum = frm_vnum;

                            oDS = new DataSet();
                            oporow = null;
                            oDS = fgen.fill_schema(frm_qstr, frm_cocd, "SOMAS");
                            for (int z = 0; z < dv.Count; z++)
                            {
                                oporow = oDS.Tables[0].NewRow();
                                oporow["BRANCHCD"] = frm_mbr;
                                oporow["orignalbr"] = frm_mbr;
                                oporow["TYPE"] = frm_vty;
                                oporow["ordno"] = frm_vnum.Trim();
                                oporow["orddt"] = invoiceDate;
                                oporow["ICAT"] = "N";

                                oporow["amdt2"] = "-";

                                oporow["acode"] = dv[z]["ACODE"].ToString().Trim();
                                oporow["pordno"] = dv[z]["ord_no"].ToString().Trim();
                                oporow["porddt"] = fgen.make_def_Date(dv[z]["ord_dt"].ToString().Trim(), vardate);
                                oporow["cscode"] = "-";

                                oporow["BUSI_EXPECT"] = 0;
                                oporow["orderby"] = 0;

                                oporow["billcode"] = 0;
                                oporow["WORK_ORDNO"] = 0;

                                oporow["srno"] = i + 1;
                                oporow["icode"] = dv[z]["icode"].ToString().Trim();

                                col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT INAME FROM ITEM WHERE TRIM(ICODE)='" + dv[z]["icode"].ToString().Trim() + "'", "INAME").ToUpper();
                                if (col3 == "0")
                                    oporow["ciname"] = "-";
                                else oporow["ciname"] = col3;

                                oporow["cpartno"] = "-";

                                oporow["desc_"] = "-";
                                oporow["cu_chldt"] = vardate;
                                oporow["pvt_mark"] = "-";

                                oporow["qtyord"] = dv[z]["qty"].ToString().Trim();

                                oporow["irate"] = dv[z]["mrp"].ToString().Trim();
                                oporow["cdisc"] = 0;

                                oporow["pexc"] = 0;
                                oporow["ptax"] = 0;

                                hscode = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT HSCODE FROM ITEM WHERE TRIM(ICODE)='" + dv[z]["icode"].ToString().Trim() + "'", "HSCODE");
                                col3 = fgen.seek_iname(frm_qstr, frm_cocd, "select NUM4||'~'||NUM5||'~'||NUM6 AS  GST from TYPEGRP where TRIM(ID)='T1' AND TRIM(ACREF)='" + hscode + "'", "GST");
                                if (col3 != "0")
                                {
                                    if (cgsg == "CG")
                                    {
                                        oporow["pexc"] = col3.Split('~')[0];
                                        oporow["ptax"] = col3.Split('~')[1];
                                    }
                                    else
                                    {
                                        oporow["pexc"] = col3.Split('~')[2];
                                        oporow["ptax"] = 0;
                                    }
                                }

                                oporow["excise"] = 0;
                                oporow["cess"] = 0;

                                oporow["basic"] = (dv[z]["qty"].ToString().Trim().toDouble() * dv[z]["mrp"].ToString().Trim().toDouble()).toDouble(2);
                                if (oporow["pexc"].ToString().toDouble() > 0)
                                    oporow["excise"] = (oporow["basic"].ToString().toDouble() * (oporow["pexc"].ToString().toDouble() / 100)).toDouble(2);
                                if (oporow["ptax"].ToString().toDouble() > 0)
                                    oporow["cess"] = (oporow["basic"].ToString().toDouble() * (oporow["ptax"].ToString().toDouble() / 100)).toDouble(2);

                                oporow["total"] = Math.Round(oporow["excise"].ToString().toDouble() + oporow["ptax"].ToString().toDouble() + oporow["basic"].ToString().toDouble(), 2);

                                oporow["desc9"] = "-";

                                oporow["cdrgno"] = frm_vnum + "." + (i + 1).ToString();

                                oporow["iexc_addl"] = 0;
                                oporow["sd"] = 0;
                                oporow["ipack"] = 0;

                                oporow["qtysupp"] = 0;
                                oporow["weight"] = 0;
                                oporow["remark"] = "-";


                                oporow["currency"] = "INR";
                                oporow["amdt3"] = 0;
                                oporow["thru"] = "-";
                                //oporow["bank_cd"] = "-";

                                oporow["CURR_RATE"] = 1;

                                oporow["ST_TYPE"] = cgsg;

                                oporow["desc7"] = "-";

                                oporow["delivery"] = 0;
                                oporow["class"] = 0;
                                oporow["qtybal"] = 0;

                                oporow["taxes"] = 0;

                                oporow["invno"] = refnum;
                                oporow["invdate"] = txtvchdate.Text;

                                oporow["org_invno"] = "-";
                                oporow["org_invdt"] = vardate;
                                oporow["del_date"] = vardate;
                                oporow["delr_date"] = vardate;
                                oporow["del_wk"] = 0;
                                oporow["packing"] = 0;
                                oporow["prefix"] = "-";
                                oporow["revis_no"] = "-";

                                oporow["ms_cont"] = "-";
                                oporow["amdt1"] = "-";


                                oporow["inst1"] = 0;
                                oporow["inst2"] = 0;
                                oporow["inst3"] = 0;
                                oporow["othamt1"] = 0;
                                oporow["othamt2"] = 0;
                                oporow["othamt3"] = 0;
                                oporow["othac1"] = "-";
                                oporow["othac2"] = "-";
                                oporow["othac3"] = "-";

                                oporow["bcd"] = 0;
                                oporow["bcdr"] = 0;
                                oporow["ccess"] = 0;
                                oporow["ccessr"] = 0;
                                oporow["acvd"] = 0;
                                oporow["acvdr"] = 0;


                                oporow["shipfrom"] = "-";
                                oporow["shipto"] = "-";
                                oporow["destcount"] = "-";
                                oporow["tptdtl"] = "-";
                                oporow["predisp"] = "-";

                                oporow["packinst"] = "-";
                                oporow["shipmark"] = "-";

                                oporow["advamt"] = 0;
                                oporow["del_mth"] = 0;
                                oporow["packamt"] = 0;
                                oporow["std_pking"] = 0;
                                oporow["sheCess"] = 0;

                                oporow["Foc"] = "N";
                                oporow["promdt"] = vardate;
                                oporow["CO_ORIG"] = "-";

                                oporow["gmt_shade"] = "-";
                                oporow["gmt_size"] = "-";


                                oporow["check_by"] = "-";
                                oporow["check_dt"] = vardate;

                                oporow["desp_to"] = "New";

                                if (edmode.Value == "Y")
                                {
                                    oporow["eNt_by"] = ViewState["entby"].ToString();
                                    oporow["eNt_dt"] = ViewState["entdt"].ToString();
                                    oporow["edt_by"] = frm_uname;
                                    oporow["edt_dt"] = vardate;
                                    oporow["app_by"] = "-";
                                    oporow["app_dt"] = vardate;
                                }
                                else
                                {
                                    oporow["eNt_by"] = frm_uname;
                                    oporow["eNt_dt"] = vardate;
                                    oporow["edt_by"] = "-";
                                    oporow["eDt_dt"] = vardate;
                                    oporow["app_by"] = frm_uname;
                                    oporow["app_dt"] = vardate;
                                }
                                oDS.Tables[0].Rows.Add(oporow);
                            }
                            fgen.save_data(frm_qstr, frm_cocd, oDS, "SOMAS");
                            #endregion


                            // INVOICE NUMBER
                            //frm_vnum = fgen.Fn_next_doc_no_inv(frm_qstr, frm_cocd, "IVOUCHER", "VCHNUM", "VCHDATE", frm_mbr, frm_vty, invoiceDate, frm_uname, Prg_Id);
                            if (dv.Count > 0)
                            {
                                if (frm_cocd == "VITR")
                                {
                                    if (frm_vty == "40" || frm_vty == "4_" || frm_vty == "43" || frm_vty == "4X" || frm_vty == "4Y")
                                        frm_vnum = fgen.Fn_next_doc_no_inv(frm_qstr, frm_cocd, "IVOUCHER", "VCHNUM", "VCHDATE", frm_mbr, frm_vty, invoiceDate, frm_uname, Prg_Id, " AND TYPE IN ('40','4_','43','4X','4Y') ");
                                    else
                                        frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, "IVOUCHER", "VCHNUM", "VCHDATE", frm_mbr, frm_vty, invoiceDate, frm_uname, Prg_Id);
                                }
                                else
                                    frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, "IVOUCHER", "VCHNUM", "VCHDATE", frm_mbr, frm_vty, invoiceDate, frm_uname, Prg_Id);

                                doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                                if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                                if (chk_opt == "Y" && frm_vnum == "000001")
                                {
                                    frm_vnum = frm_mbr + frm_vnum.Substring(2, 4);
                                }
                            }
                            oDS = new DataSet();
                            oporow = null;
                            oDS = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");
                            for (int z = 0; z < dv.Count; z++)
                            {
                                #region ivoucher saving
                                {
                                    oporow = oDS.Tables[0].NewRow();
                                    oporow["BRANCHCD"] = frm_mbr;
                                    oporow["TYPE"] = frm_vty;
                                    oporow["vchnum"] = frm_vnum.Trim();
                                    oporow["vchdate"] = invoiceDate;

                                    oporow["invno"] = frm_vnum.Trim();
                                    oporow["invdate"] = invoiceDate;

                                    oporow["store"] = "Y";
                                    oporow["rec_iss"] = "C";

                                    oporow["acode"] = dv[z]["ACODE"].ToString().Trim();
                                    oporow["rcode"] = dv[z]["ACODE"].ToString().Trim();
                                    oporow["morder"] = i + 1;
                                    oporow["icode"] = dv[z]["ICODE"].ToString().Trim();

                                    col3 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT INAME FROM ITEM WHERE TRIM(ICODE)='" + dv[z]["icode"].ToString().Trim() + "'", "INAME").ToUpper();
                                    if (col3 == "0")
                                        oporow["purpose"] = "-";
                                    else oporow["purpose"] = col3;


                                    oporow["exc_57f4"] = "-";

                                    oporow["finvno"] = dv[z]["ORD_NO"].ToString().Trim();

                                    oporow["no_bdls"] = 0;
                                    oporow["btchno"] = dv[z]["btchno"].ToString().Trim();
                                    oporow["iqtyout"] = dv[z]["QTY"].ToString().Trim();

                                    oporow["irate"] = dv[z]["MRP"].ToString().Trim();
                                    oporow["ichgs"] = 0;

                                    oporow["IAMOUNT"] = Math.Round(oporow["irate"].ToString().Trim().toDouble() * oporow["iqtyout"].ToString().Trim().toDouble(), 2);
                                    hscode = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT HSCODE FROM ITEM WHERE TRIM(ICODE)='" + dv[z]["icode"].ToString().Trim() + "'", "HSCODE");
                                    col3 = fgen.seek_iname(frm_qstr, frm_cocd, "select NUM4||'~'||NUM5||'~'||NUM6 AS  GST from TYPEGRP where TRIM(ID)='T1' AND TRIM(ACREF)='" + hscode + "'", "GST");
                                    if (col3 != "0")
                                    {
                                        if (cgsg == "CG")
                                        {
                                            oporow["exc_Rate"] = col3.Split('~')[0];
                                            oporow["exc_amt"] = 0;
                                            if (oporow["exc_Rate"].ToString().toDouble() > 0)
                                                oporow["exc_amt"] = (oporow["IAMOUNT"].ToString().toDouble() * (oporow["EXC_RATE"].ToString().toDouble() / 100)).toDouble(2);

                                            oporow["cess_percent"] = col3.Split('~')[1];
                                            oporow["cess_pu"] = 0;
                                            if (oporow["cess_percent"].ToString().toDouble() > 0)
                                                oporow["cess_pu"] = (oporow["IAMOUNT"].ToString().toDouble() * (oporow["CESS_PERCENT"].ToString().toDouble() / 100)).toDouble(2);
                                        }
                                        else
                                        {
                                            oporow["exc_Rate"] = col3.Split('~')[2];
                                            oporow["exc_amt"] = 0;
                                            if (oporow["exc_Rate"].ToString().toDouble() > 0)
                                                oporow["exc_amt"] = (oporow["IAMOUNT"].ToString().toDouble() * (oporow["EXC_RATE"].ToString().toDouble() / 100)).toDouble(2);

                                            oporow["CESS_PERCENT"] = 0;
                                            oporow["CESS_PU"] = 0;
                                        }
                                    }

                                    cgst += fgen.make_double(oporow["exc_amt"].ToString().Trim(), 2);
                                    sgst += fgen.make_double(oporow["cess_pu"].ToString().Trim(), 2);
                                    basicAmt += fgen.make_double(oporow["iamount"].ToString().Trim(), 2);

                                    oporow["iopr"] = cgsg;

                                    oporow["desc_"] = "E-COMM INV";

                                    oporow["iexc_addl"] = 0;
                                    oporow["idiamtr"] = 0;
                                    oporow["ipack"] = 0;

                                    oporow["ccent"] = 0;
                                    oporow["revis_no"] = 0;

                                    oporow["ponum"] = sonum;
                                    oporow["podate"] = sodate;

                                    oporow["tc_no"] = "-";

                                    oporow["refnum"] = refnum;
                                    oporow["refdate"] = txtvchdate.Text.Trim();

                                    oporow["O_DEPTT"] = dv[z]["btchno"].ToString().Trim();

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
                                #endregion
                            }
                            fgen.save_data(frm_qstr, frm_cocd, oDS, "IVOUCHER");

                            oDS2 = new DataSet();
                            oporow2 = null;
                            oDS2 = fgen.fill_schema(frm_qstr, frm_cocd, "SALE");
                            #region sale saving
                            {
                                double Tot_Bill_qty = 0;
                                for (i = 0; i < dv.Count - 0; i++)
                                {
                                    if (dv[i]["icode"].ToString().Trim().Length > 2)
                                    {
                                        Tot_Bill_qty = Tot_Bill_qty + fgen.make_double(dv[i]["qty"].ToString().Trim());
                                    }
                                }
                                totAmt = Math.Round(basicAmt + cgst + sgst, 2);


                                //string curr_dt;                                
                                oporow2 = oDS2.Tables[0].NewRow();
                                oporow2["BRANCHCD"] = frm_mbr;
                                oporow2["TYPE"] = frm_vty;
                                oporow2["vchnum"] = frm_vnum;
                                oporow2["vchdate"] = invoiceDate;

                                oporow2["Acode"] = dtdistDt.Rows[v]["ACODE"].ToString().Trim();

                                oporow2["cscode"] = "-";

                                oporow2["invtime"] = ":";
                                oporow2["pono"] = dtdistDt.Rows[v]["ord_no"].ToString().Trim();
                                oporow2["podate"] = dtdistDt.Rows[v]["ord_dt"].ToString().Trim();

                                oporow2["destin"] = custState;
                                oporow2["st_entform"] = "-";

                                oporow2["mode_tpt"] = "-";
                                oporow2["ins_no"] = "-";
                                oporow2["freight"] = "-";
                                oporow2["insur_no"] = "-";

                                oporow2["mo_vehi"] = "-";
                                oporow2["weight"] = 0;
                                oporow2["remvdate"] = vardate;
                                oporow2["remvtime"] = "-";

                                oporow2["post"] = cgsg.Substring(0, 1);

                                oporow2[frm_vty == "4S" ? "AMT_REA" : "AMT_SALE"] = basicAmt;
                                oporow2["AMT_EXC"] = cgst;
                                oporow2["RVALUE"] = sgst;
                                oporow2["BILL_TOT"] = totAmt;

                                oporow2["BILL_qty"] = Tot_Bill_qty;

                                oporow2["naration"] = dtdistDt.Rows[v]["CUST_ADD2"].ToString().Trim() + " , " + dtdistDt.Rows[v]["CUST_ADD1"].ToString().Trim() + " , " + dtdistDt.Rows[v]["CUST_ST"].ToString().Trim();
                                oporow2["eNt_by"] = frm_uname;
                                oporow2["eNt_dt"] = vardate;

                                oporow2["DRV_NAME"] = "-";
                                oporow2["drv_mobile"] = "-";

                                oporow2["tcsamt"] = 0;

                                oporow2["ACVDRT"] = 0;
                                oporow2["TOTDISC_AMT"] = 0;

                                oporow2["GRNO"] = "-";
                                oporow2["GRDATE"] = vardate;

                                oporow2["CHLNUM"] = refnum;
                                oporow2["CHLDATE"] = txtvchdate.Text.Trim();

                                oporow2["THRU"] = dtdistDt.Rows[v]["INV_NO"].ToString().Trim();
                                if (frm_cocd == "VITR")
                                    oporow2["SPLINV_NO"] = dtdistDt.Rows[v]["INV_NO"].ToString().Trim();
                                oporow2["ST_TYPE"] = cgsg;

                                oDS2.Tables[0].Rows.Add(oporow2);
                            }
                            #endregion
                            fgen.save_data(frm_qstr, frm_cocd, oDS2, "SALE");

                            #region vsave
                            string tax_code = "", sal_code = "", tax_code2 = "", par_code = dtdistDt.Rows[v]["ACODE"].ToString().Trim();
                            string optwb = "";
                            optwb = fgen.getOption(frm_qstr, frm_cocd, "W0100", "OPT_ENABLE");
                            if (dv.Count > 0)
                            {
                                if (cgsg == "CG")
                                {
                                    if (optwb == "Y")
                                    {
                                        tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0077", "OPT_PARAM");
                                        sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0077", "OPT_PARAM2");
                                        tax_code2 = fgen.getOption(frm_qstr, frm_cocd, "W0078", "OPT_PARAM");
                                    }
                                    else
                                    {
                                        tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A77'", "PARAMS");
                                        sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A77'", "PARAMS2");
                                        tax_code2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A78'", "PARAMS");
                                    }
                                }
                                else
                                {
                                    if (optwb == "Y")
                                    {
                                        tax_code = fgen.getOption(frm_qstr, frm_cocd, "W0079", "OPT_PARAM");
                                        sal_code = fgen.getOption(frm_qstr, frm_cocd, "W0079", "OPT_PARAM2");
                                    }
                                    else
                                    {
                                        tax_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS FROM CONTROLS WHERE ID='A79'", "PARAMS");
                                        sal_code = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT PARAMS2 FROM CONTROLS WHERE ID='A79'", "PARAMS2");

                                    }
                                }
                                int srn = 50;
                                {
                                    fgen.vSave(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(invoiceDate), 1, par_code, sal_code, fgen.make_double(totAmt, 2), 0, frm_vnum, Convert.ToDateTime(invoiceDate), rmToVSave, 0, 0, 1, fgen.make_double(totAmt, 2), 0, "-", Convert.ToDateTime(invoiceDate), frm_uname, Convert.ToDateTime(vardate), cgsg, 0, billQty, "", "-", Convert.ToDateTime(vardate), "-", frm_tab_vchr, "01");
                                    srn += 1;
                                    fgen.vSave(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(invoiceDate), srn, sal_code, par_code, 0, fgen.make_double(basicAmt, 2), frm_vnum, Convert.ToDateTime(invoiceDate), rmToVSave, 0, 0, 1, 0, fgen.make_double(basicAmt, 2), "-", Convert.ToDateTime(invoiceDate), frm_uname, Convert.ToDateTime(vardate), cgsg, 0, billQty, "", "-", Convert.ToDateTime(vardate), "-", frm_tab_vchr, "01");
                                }
                                srn += 1;
                                fgen.vSave(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(invoiceDate), srn, tax_code, par_code, 0, fgen.make_double(cgst, 2), frm_vnum, Convert.ToDateTime(invoiceDate), rmToVSave, 0, 0, 1, 0, fgen.make_double(cgst, 2), "-", Convert.ToDateTime(invoiceDate), frm_uname, Convert.ToDateTime(vardate), cgsg, 0, billQty, "", "-", Convert.ToDateTime(vardate), "-", frm_tab_vchr, "01");
                                if (tax_code2.Length > 0)
                                {
                                    srn += 1;
                                    fgen.vSave(frm_qstr, frm_cocd, frm_mbr, frm_vty, frm_vnum, Convert.ToDateTime(invoiceDate), srn, tax_code2, par_code, 0, fgen.make_double(sgst, 2), frm_vnum, Convert.ToDateTime(invoiceDate), rmToVSave, 0, 0, 1, 0, fgen.make_double(sgst, 2), "-", Convert.ToDateTime(invoiceDate), frm_uname, Convert.ToDateTime(vardate), cgsg, 0, billQty, "", "-", Convert.ToDateTime(vardate), "-", frm_tab_vchr, "01");
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                    break;
                case "F25108":
                    #region MRR Saving
                    DataView distv = new DataView(dtW, "", "VCHNUM,ACODE,ICODE", DataViewRowState.CurrentRows);
                    DataTable distDT = new DataTable();
                    distDT = distv.ToTable(true, "VCHNUM", "VCHDATE", "ACODE");

                    foreach (DataRow distRow in distDT.Rows)
                    {
                        DataView dvRows = new DataView(dtW, "VCHNUM='" + distRow["VCHNUM"].ToString().Trim() + "' AND VCHDATE='" + distRow["VCHDATE"].ToString().Trim() + "' AND ACODE='" + distRow["ACODE"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        if (dvRows.Count > 0)
                        {
                            string nVty = "02";
                            string branchcd = dvRows[0]["BRANCHCD"].ToString().Trim();

                            i = 0;
                            do
                            {
                                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum)+" + i + " as vch from IVOUCHER where branchcd='" + branchcd + "' and type='" + nVty + "' and VCHDATE " + DateRange + "", 6, "vch");
                                pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + branchcd + nVty + frm_vnum + frm_CDT1, branchcd, nVty, frm_vnum, txtvchdate.Text.Trim(), distRow["ACODE"].ToString().Trim(), frm_uname);
                                if (i > 20)
                                {
                                    fgen.FILL_ERR(frm_uname + " --> Next_no Fun Prob ==> " + frm_PageName + " ==> In Save Function");
                                    frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(vchnum)+" + 0 + " as vch from IVOUCHER where branchcd='" + branchcd + "' and type='" + nVty + "' and VCHDATE " + DateRange + "", 6, "vch");
                                    pk_error = "N";
                                    i = 0;
                                }
                                i++;
                            }
                            while (pk_error == "Y");

                            oDS1 = new DataSet();
                            oporow = null;
                            oDS1 = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");

                            for (int r = 0; r < dvRows.Count; r++)
                            {
                                oporow = oDS1.Tables[0].NewRow();
                                oporow["BRANCHCD"] = branchcd;
                                oporow["TYPE"] = nVty;
                                oporow["vchnum"] = frm_vnum.Trim();
                                oporow["vchdate"] = txtvchdate.Text.Trim();
                                oporow["genum"] = dvRows[r]["INVNO"].ToString().Trim();
                                oporow["gedate"] = dvRows[r]["INVDATE"].ToString().Trim();
                                oporow["invno"] = dvRows[r]["INVNO"].ToString().Trim();
                                oporow["invdate"] = fgen.make_def_Date(dvRows[r]["INVDATE"].ToString().Trim(), vardate);
                                oporow["refnum"] = dvRows[r]["INVNO"].ToString().Trim();
                                oporow["refdate"] = fgen.make_def_Date(dvRows[r]["INVDATE"].ToString().Trim(), vardate);
                                oporow["rec_iss"] = "D";
                                oporow["lotno"] = "-";
                                oporow["location"] = dvRows[r]["BRANCHCD"].ToString().Trim();
                                oporow["revis_no"] = "-";
                                oporow["buyer"] = "-";
                                oporow["fabtype"] = "-";
                                oporow["store_no"] = frm_mbr;
                                oporow["acode"] = dvRows[r]["ACODE"].ToString().Trim();
                                oporow["vcode"] = dvRows[r]["ACODE"].ToString().Trim();
                                oporow["gst_pos"] = "0";
                                oporow["form31"] = "0";
                                oporow["mode_tpt"] = "0";
                                oporow["styleno"] = "0";
                                oporow["mtime"] = DateTime.Now.ToString("HH:mm");

                                oporow["srno"] = (r + 1);
                                {
                                    oporow["doc_tot"] = 0;
                                }
                                oporow["morder"] = i + 1;
                                oporow["icode"] = dvRows[r]["ICODE"].ToString().Trim();
                                oporow["cavity"] = 0;
                                oporow["st_entform"] = 0;
                                oporow["segment_"] = 3;
                                oporow["isize"] = 0;

                                oporow["IQTYOUT"] = 0;
                                oporow["REJ_RW"] = 0;
                                oporow["ACPT_UD"] = 0;
                                oporow["idiamtr"] = 0;
                                oporow["iweight"] = 0;
                                oporow["shots"] = 0;
                                oporow["mattype"] = "-";
                                oporow["stage"] = "-";
                                oporow["finvno"] = "-";
                                oporow["rcode"] = "-";
                                oporow["o_Deptt"] = "-";
                                oporow["freight"] = "-";
                                oporow["exc_57f4"] = "-";
                                oporow["exc_time"] = "-";
                                oporow["IQTY_CHL"] = fgen.make_double(dvRows[r]["IQTYIN"].ToString().Trim());
                                oporow["IQTY_CHLWT"] = fgen.make_double(dvRows[r]["IQTYIN"].ToString().Trim());
                                oporow["IQTYIN"] = fgen.make_double(dvRows[r]["IQTYIN"].ToString().Trim());
                                oporow["IQTY_WT"] = fgen.make_double(dvRows[r]["IQTYIN"].ToString().Trim());
                                oporow["irate"] = fgen.make_double(dvRows[r]["IRATE"].ToString().Trim());
                                oporow["ichgs"] = fgen.make_double(dvRows[r]["IRATE"].ToString().Trim());
                                oporow["iamount"] = fgen.make_double(dvRows[r]["IQTYIN"].ToString().Trim()) * fgen.make_double(dvRows[r]["IRATE"].ToString().Trim());
                                oporow["exc_Rate"] = 0;
                                oporow["cess_percent"] = 0;
                                oporow["exc_amt"] = 0;
                                oporow["cess_pu"] = 0;
                                oporow["desc_"] = "-";
                                oporow["btchno"] = (string)ViewState["refNo"];
                                oporow["btchdt"] = txtvchdate.Text;

                                oporow["ponum"] = dvRows[r]["INVNO"].ToString().Trim();
                                oporow["ordlineno"] = r;
                                oporow["podate"] = vardate;

                                {
                                    oporow["potype"] = "-";
                                    oporow["prnum"] = "-";
                                    oporow["rtn_Date"] = txtvchdate.Text.Trim();
                                }

                                oporow["rgpnum"] = "-";
                                oporow["rgpdate"] = vardate;

                                oporow["iopr"] = "-";
                                oporow["unit"] = dvRows[r]["UNIT"].ToString().Trim();
                                oporow["store"] = "Y";
                                oporow["inspected"] = "Y";
                                oporow["pname"] = frm_uname;
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
                                oporow["naration"] = txtrmk.Text.ToUpper().ToString().Trim().Replace("'", " ").Replace("\"", " ");

                                oporow["txb_punit"] = 0;
                                oporow["exp_punit"] = 0;
                                oporow["billrate"] = 0;
                                oporow["rlprc"] = 0;
                                oporow["spexc_amt"] = 0;

                                oDS1.Tables[0].Rows.Add(oporow);
                            }

                            fgen.save_data(frm_qstr, frm_cocd, oDS1, "IVOUCHER");
                        }
                    }
                    #endregion
                    break;
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
        string ext = "", filesavepath = "", excelConString = "";
        DataTable dtn = new DataTable();
        string filename = "";
        //if (txtacode.Value.Trim().Length > 2)
        {
            #region excel Format
            if (FileUpload1.HasFile)
            {
                ext = Path.GetExtension(FileUpload1.FileName).ToLower();
                if (ext == ".xls")
                {
                    filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xls";
                    FileUpload1.SaveAs(filesavepath);
                    excelConString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
                }
                else if (ext == ".csv")
                {
                    filename = "" + DateTime.Now.ToString("ddMMyyhhmmfff");
                    filesavepath = AppDomain.CurrentDomain.BaseDirectory + "tej-base\\Upload\\file" + filename + ".csv";
                    FileUpload1.SaveAs(filesavepath);
                    excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "tej-base\\Upload\\" + ";Extended Properties=\"Text;HDR=Yes;FMT=Delimited\"";
                }
                else if (ext == ".xlsx")
                {
                    filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xlsx";
                    FileUpload1.SaveAs(filesavepath);
                    excelConString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + filesavepath + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
                }
                else
                {
                    fgen.msg("-", "AMSG", "Please Select Excel File only in xls format!!");
                    return;
                }
                try
                {
                    dtn = new DataTable();
                    if (ext == ".csv")
                    {
                        var allValues = File.ReadAllText(filesavepath).Split('\n');
                        int x = 0, colN = 0;
                        dt = new DataTable();
                        DataRow myRow = null;
                        foreach (string singleRow in allValues)
                        {
                            if (singleRow != "")
                            {
                                var allCols = singleRow.Split(',');
                                colN = 0;
                                if (x != 0) myRow = dt.NewRow();
                                foreach (string cols in allCols)
                                {
                                    if (x == 0)
                                    {
                                        dt.Columns.Add(cols);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            myRow[colN] = cols;
                                        }
                                        catch { }
                                        colN++;
                                    }
                                }
                                if (x != 0) dt.Rows.Add(myRow);
                                x++;
                            }
                        }
                        dtn = dt;
                    }
                    else
                    {
                        OleDbConnection OleDbConn = new OleDbConnection(); OleDbConn.ConnectionString = excelConString;
                        OleDbConn.Open();
                        dt = OleDbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        OleDbConn.Close();
                        String[] excelSheets = new String[dt.Rows.Count];
                        int i = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[i] = row["TABLE_NAME"].ToString();
                            i++;
                        }
                        if (ext == ".csv")
                            excelSheets[0] = "file" + filename + ".csv";
                        OleDbCommand OleDbCmd = new OleDbCommand();
                        String Query = "";
                        Query = "SELECT  * FROM [" + excelSheets[0] + "]";
                        OleDbCmd.CommandText = Query;
                        OleDbCmd.Connection = OleDbConn;
                        OleDbCmd.CommandTimeout = 0;
                        OleDbDataAdapter objAdapter = new OleDbDataAdapter();
                        objAdapter.SelectCommand = OleDbCmd;
                        objAdapter.SelectCommand.CommandTimeout = 0;
                        dt = null;
                        dt = new DataTable();
                        objAdapter.Fill(dt);
                    }                    
                    switch (Prg_Id)
                    {
                        case "F25108":
                            #region MRR Import
                            dtn.Columns.Add("BRANCHCD", typeof(string));
                            dtn.Columns.Add("TYPE", typeof(string));
                            dtn.Columns.Add("VCHNUM", typeof(string));
                            dtn.Columns.Add("VCHDATE", typeof(string));
                            dtn.Columns.Add("ACODE", typeof(string));
                            dtn.Columns.Add("ANAME", typeof(string));
                            dtn.Columns.Add("ICODE", typeof(string));
                            dtn.Columns.Add("INAME", typeof(string));
                            dtn.Columns.Add("CPARTNO", typeof(string));
                            dtn.Columns.Add("UNIT", typeof(string));
                            dtn.Columns.Add("IQTYIN", typeof(string));
                            dtn.Columns.Add("IRATE", typeof(string));
                            dtn.Columns.Add("DISC", typeof(string));
                            dtn.Columns.Add("AMOUNT", typeof(string));
                            dtn.Columns.Add("CGST", typeof(string));
                            dtn.Columns.Add("SGST", typeof(string));
                            dtn.Columns.Add("IGST", typeof(string));
                            dtn.Columns.Add("INVNO", typeof(string));
                            dtn.Columns.Add("INVDATE", typeof(string));

                            DataRow drn;
                            SQuery = "SELECT DISTINCT ACODE,ANAME,BUYCODE,TRIM(ACODE)||'~'||ANAME AS FS FROM FAMST ORDER BY ACODE";
                            dt3 = new DataTable();
                            dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                            SQuery = "SELECT DISTINCT ICODE,trim(ICODE)||'~'||INAME||'~'||CPARTNO||'~'||UNIT AS VAL,cpartno,HSCODE FROM ITEM ORDER BY ICODE";
                            dt2 = new DataTable();
                            dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                            foreach (DataRow dr in dt.Rows)
                            {
                                col1 = fgen.seek_iname_dt(dt3, "BUYCODE='" + dr[0].ToString().Trim() + "'", "FS");
                                if (col1 != "0")
                                {
                                    drn = dtn.NewRow();
                                    drn["BRANCHCD"] = frm_mbr;
                                    drn["TYPE"] = "02";
                                    drn["VCHNUM"] = "-";
                                    drn["VCHDATE"] = txtvchdate.Text;

                                    drn["ACODE"] = col1.Split('~')[0].ToString();
                                    drn["ANAME"] = col1.Split('~')[1].ToString();

                                    col2 = fgen.seek_iname_dt(dt2, "cpartno='" + dr[3].ToString().Trim().ToUpper() + "'", "VAL");
                                    if (col2 != "0")
                                    {
                                        drn["ICODE"] = col2.Split('~')[0];
                                        drn["INAME"] = col2.Split('~')[1];
                                        drn["CPARTNO"] = dr[3].ToString().Trim();
                                        drn["UNIT"] = col2.Split('~')[3];

                                        drn["IQTYIN"] = dr[6].ToString().Trim();
                                        drn["IRATE"] = getDiscountedRate(col2.Split('~')[0].Substring(0, 4), dr[7].ToString().Trim(), (dr[10].ToString().Trim().toDouble() + dr[11].ToString().Trim().toDouble() + dr[12].ToString().Trim().toDouble()).ToString());

                                        drn["INVNO"] = dr[1].ToString().Trim();
                                        drn["INVDATE"] = Convert.ToDateTime(dr[2].ToString().Trim()).ToString("dd/MM/yyyy");

                                        drn["DISC"] = dr[8].ToString().Trim();
                                        drn["AMOUNT"] = dr[9].ToString().Trim();

                                        drn["CGST"] = dr[10].ToString().Trim();
                                        drn["SGST"] = dr[11].ToString().Trim();
                                        drn["IGST"] = dr[12].ToString().Trim();
                                    }
                                    dtn.Rows.Add(drn);
                                }
                            }
                            #endregion
                            break;
                        case "F50103":
                            dtn = dt;
                            break;
                        case "HSNCODE":
                            dtn = dt;
                            break;
                        case "ACOPBAL":
                            dtn = dt;
                            break;
                        case "BILLWISEOUTDR":
                            dtn = dt;
                            break;
                        case "BILLWISEOUTCR":
                            dtn = dt;
                            break;
                        case "ITEMWISEOPBL":
                            dtn = dt;
                            break;
                        case "WIPSTKOP":
                            dtn = dt;
                            break;
                        case "STORESTKBATCHWISE":
                            dtn = dt;
                            break;
                        case "REELSTOCK":
                            dtn = dt;
                            break;
                        case "BOM":
                            dtn = dt;
                            break;
                        case "INWQCTEMP":
                            dtn = dt;
                            break;
                        case "OUTQCTEMP":
                            dtn = dt;
                            break;
                        case "SALEORDER":
                            dtn = dt;
                            break;
                        case "BSSCH_UPL":
                            dtn = dt;
                            break;
                    }
                }
                catch { }
            }
            #endregion

            ViewState["dtn"] = dtn;
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);
            sg1.DataSource = dtn;
            sg1.DataBind();
            string txt_2_pad = "";
            char pad = '0';
            int code_len = 6;
            string digit7code = "N";
            digit7code = fgen.getOption(frm_qstr, frm_cocd, "W0090", "OPT_ENABLE");
            if (digit7code == "Y")
            {
                code_len = 7;
            }

            //--------
            switch (Prg_Id)
            {
                case "ACOPBAL":
                    foreach (GridViewRow gr in sg1.Rows)
                    {
                        string s1 = gr.Cells[1].Text.Trim();
                        txt_2_pad = s1.PadLeft(code_len, pad);
                        gr.Cells[1].Text = txt_2_pad;
                    }
                    break;
            }
            //-----------

            if (dtn != null)
                fgen.msg("-", "AMSG", "Total Rows Imported : " + dtn.Rows.Count.ToString());
        }
    }
    string getDiscountedRate(string ticode, string currRate, string tax)
    {
        DataTable dtdisc = new DataTable();
        if (ViewState["dtItemSub"] == null)
        {
            dtdisc = fgen.getdata(frm_qstr, frm_cocd, "SELECT TRIM(ICODE) AS ICODE,num1 as irate,num2 AS IRATE2 FROM SCRATCH2 WHERE BRANCHCD!='DD' AND TYPE='DS' ORDER BY ICODE ");
            ViewState["dtItemSub"] = dtdisc;
        }
        else
        {
            dtdisc = (DataTable)ViewState["dtItemSub"];
        }
        string rate = currRate;
        string rateDiscount = fgen.seek_iname_dt(dtdisc, "ICODE='" + ticode + "' ", "IRATE");
        if (rateDiscount.toDouble() > 0)
        {
            rate = ((rate.toDouble() - (rate.toDouble() * (rateDiscount.toDouble() / 100))).toDouble(2)).ToString();
        }
        return rate;
    }
    protected void btnAcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Supplier ", frm_qstr);
    }
    protected void btnDNCN_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "DNCN";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select D/N C/N Reaosn", frm_qstr);
    }
    protected void btnGstClass_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "GSTCLASS";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select GST Class", frm_qstr);
    }
    protected void btnRcode_Click(object sender, ImageClickEventArgs e)
    {
        hffield.Value = "TRCODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Leadger ", frm_qstr);
    }
    protected void btnTempl_Click(object sender, EventArgs e)
    {
        set_Val();
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        DataTable dtTemp = new DataTable();
        switch (Prg_Id)
        {
            case "HSNCODE":
                col1 = "HSN_Master_Uploading_Template";
                dtTemp.Columns.Add("Srno");
                dtTemp.Columns.Add("HSN Code");
                dtTemp.Columns.Add("HSN Name");
                dtTemp.Columns.Add("CGST%");
                dtTemp.Columns.Add("SGST%");
                dtTemp.Columns.Add("IGST%");
                dtTemp.Columns.Add("Taxable (Y/N)");
                dtTemp.Columns.Add("Good/Service (G/S)");
                break;
            case "BSSCH_UPL":
                col1 = "Accounts_Sch_Uploading_Template";
                dtTemp.Columns.Add("Srno");
                dtTemp.Columns.Add("Sch Code");
                dtTemp.Columns.Add("Sch Name");
                break;
            case "ACOPBAL":
                col1 = "AC_Balance_Op.Bal_Uploading_Template";
                dtTemp.Columns.Add("Srno");
                dtTemp.Columns.Add("A/C Code");
                dtTemp.Columns.Add("A/C Name");
                dtTemp.Columns.Add("Group");
                dtTemp.Columns.Add("Op.Bal(Home Currency)");
                break;
            case "BILLWISEOUTDR":
                col1 = "Bill_Wise_Outstandings(Drs)_Uploading_Template";
                dtTemp.Columns.Add("Srno");
                dtTemp.Columns.Add("Party Code");
                dtTemp.Columns.Add("Bill No.");
                dtTemp.Columns.Add("Bill Dt.");
                dtTemp.Columns.Add("Dramt");
                dtTemp.Columns.Add("Cramt");
                dtTemp.Columns.Add("FC-Dramt");
                dtTemp.Columns.Add("FC-Cramt");
                dtTemp.Columns.Add("FC-type");
                dtTemp.Columns.Add("FC-Rate");
                dtTemp.Columns.Add("Remarks");
                break;
            case "BILLWISEOUTCR":
                col1 = "Bill_Wise_Outstandings(Crs)_Uploading_Template";
                dtTemp.Columns.Add("Srno");
                dtTemp.Columns.Add("Party Code");
                dtTemp.Columns.Add("Bill No.");
                dtTemp.Columns.Add("Bill Dt.");
                dtTemp.Columns.Add("Dramt");
                dtTemp.Columns.Add("Cramt");
                dtTemp.Columns.Add("FC-Dramt");
                dtTemp.Columns.Add("FC-Cramt");
                dtTemp.Columns.Add("FC-type");
                dtTemp.Columns.Add("FC-Rate");
                dtTemp.Columns.Add("Remarks");
                break;
            case "ITEMWISEOPBL":
                col1 = "Item wise Stock Balance Op.Bal_Uploading_Template";
                dtTemp.Columns.Add("Srno");
                dtTemp.Columns.Add("Party Code");
                dtTemp.Columns.Add("Bill No.");
                dtTemp.Columns.Add("Bill Dt.");
                dtTemp.Columns.Add("Dramt");
                dtTemp.Columns.Add("Cramt");
                dtTemp.Columns.Add("FC-Dramt");
                dtTemp.Columns.Add("FC-Cramt");
                dtTemp.Columns.Add("FC-type");
                dtTemp.Columns.Add("FC-Rate");
                dtTemp.Columns.Add("Remarks");
                break;
            case "WIPSTKOP":
                col1 = "Section Wise WIP Stock Op.Bal_Uploading_Template";
                dtTemp.Columns.Add("Srno");
                dtTemp.Columns.Add("Item Code");
                dtTemp.Columns.Add("Item Name");
                dtTemp.Columns.Add("Part no");
                dtTemp.Columns.Add("Op.Bal");
                dtTemp.Columns.Add("Op.Rate");
                dtTemp.Columns.Add("Location");
                dtTemp.Columns.Add("Min.Lvl");
                dtTemp.Columns.Add("Max.Lvl");
                dtTemp.Columns.Add("Ord.Lvl");
                dtTemp.Columns.Add("C-Iname");
                dtTemp.Columns.Add("Packing 1");
                dtTemp.Columns.Add("Packing 2");
                dtTemp.Columns.Add("Ref_fld");
                break;
            case "STORESTKBATCHWISE":
                col1 = "Batch No. Wise Store Stock_Uploading_Template";
                dtTemp.Columns.Add("Srno");
                dtTemp.Columns.Add("Item Code");
                dtTemp.Columns.Add("Item Name");
                dtTemp.Columns.Add("Part no");
                dtTemp.Columns.Add("Qty");
                dtTemp.Columns.Add("Inv-no");
                dtTemp.Columns.Add("Inv-dt");
                dtTemp.Columns.Add("Rate");
                dtTemp.Columns.Add("Acode");
                break;
            case "REELSTOCK":
                col1 = "Reel Stock_Uploading_Template";
                dtTemp.Columns.Add("Srno");
                dtTemp.Columns.Add("Item Code");
                dtTemp.Columns.Add("Supp Code");
                dtTemp.Columns.Add("Our Reel No");
                dtTemp.Columns.Add("Supp Reel No");
                dtTemp.Columns.Add("Weight");
                dtTemp.Columns.Add("Rate");
                dtTemp.Columns.Add("Size");
                dtTemp.Columns.Add("GSM");
                dtTemp.Columns.Add("Grade");
                dtTemp.Columns.Add("locn");
                dtTemp.Columns.Add("REELSPEC1");
                dtTemp.Columns.Add("REELSPEC2");
                break;
            case "BOM":
                col1 = "Bill of Materials Master_Uploading_Template";
                dtTemp.Columns.Add("Srno");
                dtTemp.Columns.Add("Type");
                dtTemp.Columns.Add("Bom No");
                dtTemp.Columns.Add("Bom DT");
                dtTemp.Columns.Add("Parent Code");
                dtTemp.Columns.Add("Child Code");
                dtTemp.Columns.Add("Gross Wt");
                dtTemp.Columns.Add("Net wt");
                dtTemp.Columns.Add("Bom Qty");
                dtTemp.Columns.Add("Alt Qty");
                dtTemp.Columns.Add("Alt child");
                dtTemp.Columns.Add("RAW/COM/CON");
                break;
            case "INWQCTEMP":
                col1 = "Inward Quality Templates Master_Uploading_Template";
                dtTemp.Columns.Add("Srno");
                dtTemp.Columns.Add("Type");
                dtTemp.Columns.Add("Tmp No");
                dtTemp.Columns.Add("Tmp DT");
                dtTemp.Columns.Add("Acode");
                dtTemp.Columns.Add("Icode");
                dtTemp.Columns.Add("Parameter");
                dtTemp.Columns.Add("Specification");
                dtTemp.Columns.Add("Lower Limit");
                dtTemp.Columns.Add("Upper Limit");
                dtTemp.Columns.Add("Method");
                break;
            case "OUTQCTEMP":
                col1 = "Outward Quality Templates Master_Uploading_Template";
                dtTemp.Columns.Add("Srno");
                dtTemp.Columns.Add("Type");
                dtTemp.Columns.Add("Tmp No");
                dtTemp.Columns.Add("Tmp DT");
                dtTemp.Columns.Add("Acode");
                dtTemp.Columns.Add("Icode");
                dtTemp.Columns.Add("Parameter");
                dtTemp.Columns.Add("Specification");
                dtTemp.Columns.Add("Lower Limit");
                dtTemp.Columns.Add("Upper Limit");
                dtTemp.Columns.Add("Method");
                break;
            case "SALEORDER":
                col1 = "Sales Orders Master_Uploading_Template";
                dtTemp.Columns.Add("Srno");
                dtTemp.Columns.Add("Type");
                dtTemp.Columns.Add("S.O No");
                dtTemp.Columns.Add("S.O DT");
                dtTemp.Columns.Add("Acode");
                dtTemp.Columns.Add("Icode");
                dtTemp.Columns.Add("Customer");
                dtTemp.Columns.Add("Product");
                dtTemp.Columns.Add("Part.No");
                dtTemp.Columns.Add("Cust-PONO");
                dtTemp.Columns.Add("Cust-PODT");
                dtTemp.Columns.Add("Qty");
                dtTemp.Columns.Add("Rate");
                dtTemp.Columns.Add("Disc%");
                break;
        }

        if (dtTemp != null) fgen.exp_to_excel(dtTemp, "ms-excel", "xls", col1);
    }

    void uploadSaleOrderbyExcel()
    {
        int ctr = 0;
        DataTable dtMain = (DataTable)ViewState["dtn"];
        if (dtMain != null)
        {
            dtMain.Columns[1].ColumnName = "Type";
            dtMain.Columns[9].ColumnName = "PONO";
        }

        DataTable dtW = new DataTable();

        DataView dvW = new DataView(dtMain);
        dt = new DataTable();
        dt = dvW.ToTable(true, "Type", "PONO");

        string curr_postr = "";
        foreach (DataRow drType in dt.Rows)
        {
            frm_vty = drType["Type"].ToString();
            i = 0;
            do
            {
                frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch");
                pk_error = fgen.chk_pk(frm_qstr, frm_cocd, frm_tabname.ToUpper() + frm_mbr + frm_vty + frm_vnum + frm_CDT1, frm_mbr, frm_vty, frm_vnum, txtvchdate.Text.Trim(), "", frm_uname);
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

            dvW = new DataView(dtMain, "Type='" + drType["Type"].ToString() + "' AND PONO='" + drType["PONO"].ToString() + "'", "", DataViewRowState.CurrentRows);
            dtW = new DataTable();
            if (dvW != null)
                dtW = dvW.ToTable();

            ctr = 0;
            oDS = new DataSet();
            oporow = null;
            string cgsg = "";
           
            oDS = fgen.fill_schema(frm_qstr, frm_cocd, frm_tabname);
            foreach (DataRow gr1 in dtW.Rows)
            {
                #region Sales Order Saving
                curr_postr = gr1[4].ToString() + gr1[9].ToString();
                {
                    ctr = ctr + 1;
                    oporow = oDS.Tables[0].NewRow();

                    string myState = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATENM FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "STATENM");
                    string custState = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT STATEN FROM famst WHERE trim(acode)='" + gr1[4].ToString().Trim() + "'", "STATEN");

                    if (myState.ToUpper().Trim() == custState.ToUpper().Trim()) cgsg = "CG";
                    else cgsg = "IG";

                    oporow["BRANCHCD"] = frm_mbr;
                    oporow["Type"] = gr1[1].ToString();
                    oporow["ordno"] = frm_vnum;
                    oporow["cdrgno"] = frm_vnum + "." + ctr;
                    oporow["orddt"] = txtvchdate.Text.Trim();
                    oporow["Srno"] = ctr;
                    oporow["CSCODE"] = "-";
                    oporow["billcode"] = "-";
                    oporow["st_type"] = "-";
                    oporow["acode"] = gr1[4].ToString(); ;
                    oporow["icode"] = gr1[5].ToString();
                    oporow["ciname"] = gr1[7].ToString();
                    oporow["desc9"] = gr1[7].ToString();
                    oporow["cpartno"] = gr1[8].ToString();
                    oporow["PORDNO"] = gr1[9].ToString();
                    oporow["PORDDT"] = gr1[10].ToString();

                    oporow["qtyord"] = gr1[11].ToString();
                    oporow["irate"] = gr1[12].ToString();
                    oporow["cdisc"] = fgen.make_double(gr1[13].ToString());
                    oporow["CU_CHLDT"] = vardate;
                    oporow["qtysupp"] = 0;
                    oporow["iexc_Addl"] = 0;
                    oporow["icat"] = "N";
                    oporow["ent_by"] = frm_uname;
                    oporow["ent_dt"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["edt_dt"] = vardate;
                    oporow["check_by"] = "-";
                    oporow["check_dt"] = vardate;
                    oporow["APP_BY"] = "-";
                    oporow["APP_DT"] = vardate;
                    oporow["Delivery"] = 0;
                    oporow["Month"] = "-";
                    oporow["QD"] = 0;
                    oporow["sd"] = 0;
                    oporow["Class"] = "-";
                    oporow["zone"] = "-";
                    oporow["ms_cont"] = "-";
                    oporow["AMDT1"] = "-";
                    oporow["amdt2"] = "-";
                    oporow["amdt3"] = "-";
                    oporow["remark"] = "-";
                    oporow["desp_to"] = "-";
                    oporow["PVT_MARK"] = "-";
                    oporow["Weight"] = "-";
                    oporow["thru"] = "-";
                    oporow["qtybal"] = 0;
                    oporow["INVNO"] = "-";
                    oporow["invdate"] = vardate;
                    oporow["refdate"] = vardate;
                    oporow["org_invno"] = "-";
                    oporow["org_invdt"] = vardate;
                    oporow["CU_CHLNO"] = "-";

                    oporow["FOC"] = "-";

                    oporow["frght"] = 0;
                    oporow["iexc_Addl"] = 0;
                    oporow["del_date"] = vardate;
                    oporow["DELR_DATE"] = vardate;
                    oporow["DEL_WK"] = 0;
                    oporow["DEL_MTH"] = 0;
                    oporow["packing"] = 0;
                    oporow["Currency"] = "Rs";
                    oporow["CURR_RATE"] = 1;
                    //oporow["desc"] = "-";
                    oporow["work_ordno"] = "-";
                    oporow["prefix"] = "-";
                    oporow["revis_no"] = "-";
                    oporow["ipack"] = 0;
                    oporow["gmt_shade"] = "-";
                    oporow["gmt_size"] = "-";
                   string hscode = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT HSCODE FROM ITEM WHERE TRIM(ICODE)='" + gr1[5].ToString().Trim() + "'", "HSCODE");
                    col3 = fgen.seek_iname(frm_qstr, frm_cocd, "select NUM4||'~'||NUM5||'~'||NUM6 AS  GST from TYPEGRP where TRIM(ID)='T1' AND TRIM(ACREF)='" + hscode + "'", "GST");
                    if (col3 != "0")
                    {
                        if (cgsg == "CG")
                        {
                            oporow["pexc"] = col3.Split('~')[0];
                            oporow["ptax"] = col3.Split('~')[1];
                        }
                        else
                        {
                            oporow["pexc"] = col3.Split('~')[2];
                            oporow["ptax"] = 0;
                        }
                    }

                    oporow["orderby"] = "-";
                    oporow["BUSI_potent"] = "-";
                    oporow["BUSI_EXPECT"] = "-";
                    oporow["Amdtno"] = 0;
                    oporow["OrignalBR"] = frm_mbr;
                    oporow["rlprc"] = 0;
                    oporow["INSPCHG"] = 0;
                    oporow["td"] = 0;
                    oporow["cd"] = 0;

                    oporow["othac1"] = "-";
                    oporow["othac2"] = "-";
                    oporow["othac3"] = "-";
                    oporow["othamt1"] = 0;
                    oporow["othamt2"] = 0;
                    oporow["othamt3"] = 0;
                    oporow["shecess"] = 0;
                    oporow["desc0"] = "-";
                    oporow["desc1"] = "-";
                    oporow["desc2"] = "-";
                    oporow["desc3"] = "-";
                    oporow["desc4"] = "-";
                    oporow["desc5"] = "-";
                    oporow["desc6"] = "-";
                    oporow["desc7"] = "-";
                    oporow["desc8"] = "-";
                    oporow["desc9"] = "-";
                    oporow["BASIC"] = 0;
                    oporow["Excise"] = 0;
                    oporow["cess"] = 0;
                    oporow["taxes"] = 0;
                    oporow["TOTAL"] = 0;
                    oporow["inst1"] = 0;
                    oporow["inst2"] = 0;
                    oporow["inst3"] = 0;
                    oporow["bcd"] = 0;
                    oporow["bcdr"] = 0;
                    oporow["CCESS"] = 0;
                    oporow["CCESSR"] = 0;
                    oporow["ACVD"] = 0;
                    oporow["ACVDR"] = 0;
                    oporow["shipfrom"] = "-";
                    oporow["shipto"] = "-";
                    oporow["destcount"] = "-";
                    oporow["inspby"] = "-";
                    oporow["explic"] = "-";
                    oporow["tptdtl"] = "-";
                    oporow["predisp"] = "-";
                    oporow["packinst"] = "-";
                    oporow["shipmark"] = "-";

                    oporow["attach1"] = "-";
                    oporow["EMAIL_STATUS"] = "-";
                    oporow["othac4"] = "-";
                    oporow["othamt4"] = 0;
                    oporow["ins_per"] = 0;
                    oporow["ins_Amt"] = 0;
                    oporow["advamt"] = 0;
                    oporow["sta_amt"] = 0;
                    oporow["sta_rate"] = 0;
                    oporow["ppcdate"] = "-";
                    oporow["packamt"] = 0;
                    oporow["STD_PKING"] = 0;
                    oporow["btchno"] = "-";
                    oporow["RETN_PER"] = 0;
                    oporow["adv_rcv"] = 0;
                    oporow["adv_due"] = 0;
                    oporow["lirate"] = 0;
                    oporow["EXR_IMP"] = "-";
                    oporow["othac5"] = "-";
                    oporow["othamt5"] = 0;
                    oporow["bank_cd"] = "-";
                    oporow["INST4"] = 0;
                    oporow["PROMDT"] = vardate;
                    oporow["oinspby"] = "-";
                    oporow["MRCAL"] = "-";
                    oporow["NOTIFY"] = "-";
                    oporow["finaldst"] = "-";
                    oporow["HSCODES"] = "-";
                    oporow["SUPPBY"] = "-";
                    oporow["hs_code"] = "-";
                    oporow["oa_disc"] = 0;
                    oporow["CO_ORIG"] = "-";
                    oporow["ORD_ALERT"] = "-";
                    oporow["SO_ADD_QTY"] = 0;
                    oDS.Tables[0].Rows.Add(oporow);

                }
                #endregion
            }

            fgen.save_data(frm_qstr, frm_cocd, oDS, "SOMAS");
            save_it = "Y";
        }
    }
}