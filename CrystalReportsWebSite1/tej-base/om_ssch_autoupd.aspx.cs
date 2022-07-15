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

public partial class om_ssch_autoupd : System.Web.UI.Page
{
    string btnval, SQuery, col1, col2, col3, vardate, fromdt, todt;
    DataTable dt, dt2, dt3, dt4, dt8, dt9, dt10;
    DataRow oporow, oporow1, oporow2; DataSet oDS, oDS1, oDS2;
    int i = 0, z = 0;

    DataTable dtCol = new DataTable();
    DataTable dt1;
    string Checked_ok;
    string save_it,Co;
    double db, db1, db2, db3;
    string Prg_Id;
    string pk_error = "Y", chk_rights = "N", DateRange, xprdRange, PrdRange, cmd_query, value1, value2;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1, frm_CDT2, xprdrange;
    string mq1 = "", mq2 = "", mq3 = "", mq4 = "", mq5 = "", mq6 = "", mq7 = "", mq8 = "";
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
                    frm_CDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT1");
                    frm_CDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CDT2");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                    vardate = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
                    xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
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
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false; btnvalidate.Disabled = true;
        btnexit.Visible = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true; FileUpload1.Enabled = false;
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true; btnvalidate.Disabled = false;
        btnhideF.Enabled = true; btnhideF_s.Enabled = true; btnexit.Visible = false; btncancel.Visible = true; FileUpload1.Enabled = true;
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
        if (Prg_Id == "F50302")
        {
            lblheader.Text = "14 Day Sales Schedule";
          //  frm_tabname = "BUDGMST";
            frm_tabname = "SCHEDULE";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "46");
        }
        if (Prg_Id == "F50304")
        {
            lblheader.Text = "6 Month Sales Forecast";
            frm_tabname = "WB_SCH_UPD";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "4I");
        }      
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {
        SQuery = "";
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hfname.Value;
        switch (btnval)
        {
            case "ACODE":
                SQuery = "select trim(acode) as acode,trim(aname) as customer,trim(acode) as code from famst where trim(Acode) like '16%' order by customer";
                break;
            default:
                if (btnval == "Edit_E" || btnval == "Del_E" || btnval == "Print_E" || btnval == "COPY_OLD" || btnval == "LIST_E")
                    SQuery = "select distinct trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy') as fstr,a.vchnum as entry_no,to_char(a.vchdate,'dd/mm/yyyy') as entry_Dt,a.col33 as batchno,A.COL35 AS BATCH_DATE,a.ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as ent_dt,to_Char(a.vchdate,'yyyymmdd') as vdd from " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' AND a.vchdate " + DateRange + " order by vdd desc,a.vchnum desc";
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
            set_Val();
            frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
          //frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch"); //old logic as per saving in budgmst table
            frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "select max(" + doc_nf.Value + ")+" + i + " as vch from " + frm_tabname + " where branchcd='" + frm_mbr + "' and type='" + frm_vty + "' and " + doc_df.Value + " " + DateRange + "", 6, "vch"); //new logic as per saving in schedule table
            txtvchnum.Text = frm_vnum;
            txtvchdate.Text = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
            disablectrl();
            fgen.EnableForm(this.Controls);
            btnsave.Disabled = true;
            FileUpload1.Enabled = false;
            btnvalidate.Disabled = true;
            btnlist.Disabled = true;
            btniname.Enabled = true;
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
        fgen.fill_dash(this.Controls);
        int dhd = fgen.ChkDate(txtvchdate.Text.ToString());
        if (dhd == 0)
        { fgen.msg("-", "AMSG", "Please Select a Valid Date"); txtvchdate.Focus(); return; }
        if (Convert.ToDateTime(txtvchdate.Text) < Convert.ToDateTime(fromdt) || Convert.ToDateTime(txtvchdate.Text) > Convert.ToDateTime(todt))
        { fgen.msg("-", "AMSG", "Back Year Date is Not Allowed!!'13'Fill date for This Year Only"); txtvchdate.Focus(); return; }

        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        string crFound = "N";
        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length> 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }
        else fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        btnsave.Disabled = true;
    }
    //------------------------------------------------------------------------------------
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        //chk_rights = fgen.Fn_chk_can_del(frm_qstr, frm_cocd, frm_UserID, frm_formID);
        //if (chk_rights == "Y")
        //{
        //    clearctrl();
        //    set_Val();
        //    hffield.Value = "Del_E";
        //    make_qry_4_popup();
        //    fgen.Fn_open_sseek("Select " + lblheader.Text + " ", frm_qstr);
        //}
        //else fgen.msg("-", "AMSG", "Dear " + frm_uname + ", You Currently Do Not Have Rights To Delete Entry For This Form !!");
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
        sg1.DataSource = null;       
        sg1.DataBind();
        //ViewState["sg1_dt"] = dtn;
        clearctrl();
        enablectrl();
        setColHeadings();
    }
    //------------------------------------------------------------------------------------
    protected void btnlist_ServerClick(object sender, EventArgs e)
    {
     //   hffield.Value = "LIST_E";
       // make_qry_4_popup();
       // fgen.Fn_open_sseek("-", frm_qstr);

        hffield.Value = "List";
        fgen.Fn_open_prddmp1("Select Date for List", frm_qstr);
    }
    //------------------------------------------------------------------------------------
    protected void btnprint_ServerClick(object sender, EventArgs e)
    {

    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hfname.Value;
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
            }
        }      
        else if (hffield.Value == "SAVE")
        {
            if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y") hfCNote.Value = "Y";
            else hfCNote.Value = "N";
            DataTable dtn = new DataTable();
            dtn = (DataTable)ViewState["dtn"];
           // ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);
            fgen.msg("-", "SMSG", "Are You Sure, You Want To Save!!");
        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");
            btnval = hfname.Value;
            switch (btnval)
            {                             
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
                    break;
                case "Edit":                   
                    break;
                case "Del_E":                   
                    break;
                case "Print":                   
                    break;
                case "Edit_E":                  
                    break;
                case "Print_E":
                    break;
                case "ACODE":
                    dt3 = new DataTable();dt8=new DataTable();
                    mq1 = "SELECT distinct trim(acode) as acode FROM SOMAS WHERE branchcd='" + frm_mbr + "' and ACOde='" + col1 + "'  AND ICAT='N' ";
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, mq1);
                    mq1 = "";
                    if (dt3.Rows.Count < 1)
                    {
                        txtacode.Text = col1;
                        txtaname.Text = col2;                       
                        fgen.msg("-", "AMSG", "No any Sale Order found for this Party");
                        return;
                    }
                    else
                    {
                        txtacode.Text = col1;
                        txtaname.Text = col2;
                        //mq4="select  max(vchnum) as vchnum from schedule where BRANCHCD='" + frm_mbr + "' AND TYPE='46' AND acode='" + col1 + "' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + vardate.Substring(3, 8) + "'";
                        mq4 = "select  max(vchnum) as vch   from schedule where BRANCHCD='" + frm_mbr + "' AND TYPE='46'  AND acode='" + col1 + "' AND TO_CHAR(VCHDATE,'MM/YYYY')='" + vardate.Substring(3, 7) + "'";
                        dt8 = fgen.getdata(frm_qstr, frm_cocd,mq4 );
                        if(dt8.Rows.Count>0)
                        {
                            fgen.msg("-", "AMSG", "Schedule Already Entered for this Month,in Sheet No. " + dt8.Rows[0]["vch"].ToString().Trim() + " ,Make new Only if Reqd,Press OK to Proceed");                            
                        }
                        FileUpload1.Enabled = true;
                    }
                    break;  
            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        value1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
        value2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
        xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "List")
        {
            set_Val();
            #region
            if (Prg_Id == "F50302")
            {

                SQuery = "SELECT VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,ORG_PONO AS SCHEDULE_NO,SOLINK AS SCHEDULE_DATE,ICODE,SPLCODE AS IAIJ_CODE,SODESC1 AS FORD_CODE,SRNO  AS LINE_NO,PPORDNO AS DESP_DATE,ACTUALCOST AS QUANTITY FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + xprdRange + "";
            }
            if (Prg_Id == "F50304")
            {
                SQuery = "SELECT VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,BUY_CODE AS FORD_PLANT_CODE,PARTNO,QTY AS QUANTITY,STDATE,ICODE,LINENO,WEEK,VEND_CODE FROM FINIAIJ.WB_SCH_UPD WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' AND VCHDATE " + xprdRange + "";
            }
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
            // check directory existence if not then create
            if (!Directory.Exists(@"c:\TEJ_ERP\Upload")) //iaij
            {
                Directory.CreateDirectory(@"c:\TEJ_ERP\Upload");
            }
            string path = "Data_" + System.DateTime.Now.Date.ToString("dd_MM_yyyy").Trim() + "_" + System.DateTime.Now.ToString("HH_mm").Trim();
            string path1 = @"c:/tej_erp/Upload/" + path + ".txt";
            try
            {
                //open file
                StreamWriter wr = new StreamWriter(path1);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    wr.Write(dt.Columns[i].ToString().ToUpper() + "\t");
                }
                wr.WriteLine();
                //write rows to excel file
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            wr.Write(Convert.ToString(dt.Rows[i][j]) + "\t");
                        }
                        else
                        {
                            wr.Write("\t");
                        }
                    }
                    //go to next line
                    wr.WriteLine();
                }
                //close file
                wr.Close();
                string filePath = path;
                Session["FilePath"] = filePath + ".txt";
                Session["FileName"] = "" + path + ".txt";
                Response.Write("<script>");
                Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                Response.Write("</script>");
            }
            catch (Exception ex)
            {
                // FILL_ERR(ex.Message.ToString() + " Export To Excel");
            }
            #endregion
        }
        else
        {
            Checked_ok = "Y";
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
                            }
                        }

                        // If Vchnum becomes 000000 then Re-Save
                        if (frm_vnum == "000000") btnhideF_Click(sender, e);

                        save_fun();
                        ViewState["refNo"] = frm_vnum;
                        if (edmode.Value == "Y")
                        {
                            cmd_query = "update " + frm_tabname + " set branchcd='DD' where branchcd||type||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_FSTR") + "'";
                            fgen.execute_cmd(frm_qstr, frm_cocd, cmd_query);
                        }
                        fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                        //  save_fun2();

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
                                //fgen.send_mail(frm_cocd, "Finsys ERP", "vipin@finsys.in", "", "", "Hello", "test Mail");
                                fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully ");
                                sg1.DataSource = null;
                                sg1.DataBind();

                                btnsave.Disabled = true;
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
        if (Prg_Id == "F50302")
        {
            #region sale order nikal liya hai but save krna hai and ponum me  sale order save krna hai
            //  //SALE ORDER QUERY
            DataTable dt3 = new DataTable();
            dt3 = fgen.getdata(frm_qstr, frm_cocd, "SELECT distinct max(ordno) as ordno,to_char(orddt,'dd/mm/yyyy') as orddt,to_char(orddt,'yyyymmdd') as vdd,trim(acode) as acode FROM SOMAS WHERE branchcd='" + frm_mbr + "' and ACODE='" + txtacode.Text + "' AND ICAT='N' group by  to_char(orddt,'dd/mm/yyyy'),trim(acode),to_char(orddt,'yyyymmdd') order by ordno,vdd");

            DataTable dt6 = new DataTable(); DataTable dt5 = new DataTable();
            dt6 = fgen.getdata(frm_qstr, frm_cocd, "select trim(icode) as icode,trim(cpartno) as part_no,trim(iname) as iname from item where length(trim(icode))>=8");
            if (dtW != null)
            {
                i = 0;
                oporow = oDS.Tables[0].NewRow();
                if (dtW.Rows.Count > 0)
                {
                    DataView  view1im = new DataView(dtW);
                    DataTable dtdrsim = new DataTable();
                    dtdrsim = view1im.ToTable(true, "PART_NO"); //MAIN          
                    foreach (DataRow gr1 in dtdrsim.Rows)
                    {
                        DataView viewim = new DataView(dtW, "PART_NO='" + gr1["PART_NO"] + "'", "", DataViewRowState.CurrentRows);
                        oporow = oDS.Tables[0].NewRow();
                        DataTable dt2 = new DataTable();
                        dt2 = viewim.ToTable();
                        mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; db = 0;
                        for (i = 0; i < dt2.Rows.Count; i++)
                        {
                            #region saving in budgmnst table
                            mq2 = "";
                            oporow["BRANCHCD"] = frm_mbr;
                            oporow["type"] = frm_vty;
                            oporow["vchnum"] = frm_vnum;
                            oporow["vchdate"] = txtvchdate.Text;
                            oporow["SRNO"] = i + 1;
                            mq1 = fgen.seek_iname_dt(dt6, "part_no='" + gr1["PART_NO"].ToString().Trim() + "'", "icode");
                            if (mq1 != "0")
                            {
                                oporow["icode"] = mq1;
                            }
                            mq1 = fgen.seek_iname_dt(dt6, "part_no='" + gr1["PART_NO"].ToString().Trim() + "'", "iname");
                            oporow["partname"] = mq1;
                            oporow["partnum"] = gr1["PART_NO"].ToString().Trim();
                            oporow["acode"] = txtacode.Text.Trim();
                            oporow["REMARKS"] = dt2.Rows[i]["Schedule_No"].ToString().Trim(); //gr1["Schedule_No"].ToString().Trim() + gr1["Schedule_Date"].ToString().Trim();
                            oporow["LINE_RMK"] = fgen.make_double(dt2.Rows[i]["LINE_NO"].ToString().Trim()); //fgen.make_double(gr1["LINE_NO"].ToString().Trim());
                            ///
                            mq2 = dt2.Rows[i]["desp_date"].ToString().Trim().Substring(4, 2);  //gr1["desp_date"].ToString().Trim().Substring(4, 2);
                            if (Convert.ToInt32(mq2) < 10)
                            {
                                mq2 = mq2.Replace("0", "");
                            }
                            oporow["DAY" + mq2 + ""] = fgen.make_double(dt2.Rows[i]["QTY"].ToString().Trim());  //fgen.make_double(gr1["QTY"].ToString().Trim());
                            mq3 = "," + mq2 + "";
                            mq4 = mq4 + mq3;
                            mq5 = mq4.Replace("'", "").TrimStart(',');
                            db += fgen.make_double(dt2.Rows[i]["QTY"].ToString().Trim());  // fgen.make_double(gr1["QTY"].ToString().Trim());
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
                            #endregion
                        }
                        string[] arr = mq5.Split(',');
                        for (int j = 1; j <= 35; j++)
                        {
                            if (oporow["DAY" + j + ""].ToString() == "0" || oporow["DAY" + j + ""].ToString() == "")
                            {
                                oporow["DAY" + j + ""] = 0;
                            }
                        }
                        oporow["TOTAL"] = db;
                        oporow["PSIZE"] = 0;
                        oporow["GSM"] = 0;
                        oporow["WK1"] = 0;
                        oporow["WK2"] = 0;
                        oporow["WK3"] = 0;
                        oporow["WK4"] = 0;
                        oporow["QTYORD"] = 0;
                        oporow["IRATE"] = 0;
                        oporow["AMDTNO"] = 0;
                        oporow["orignalbr"] = 0;
                        oporow["app_by"] = "-";
                        //pic max sonum as per virender sir
                        oporow["SONUM"] = fgen.seek_iname_dt(dt3, "acode='" + txtacode.Text.Trim() + "'", "ordno");
                        oporow["SODATE"] = Convert.ToDateTime(fgen.seek_iname_dt(dt3, "acode='" + txtacode.Text.Trim() + "'", "orddt")).ToString("dd/MM/yyyy");
                        oporow["PONUM"] = "-";
                        oporow["PODATE"] = "-";
                        oDS.Tables[0].Rows.Add(oporow);
                    }
                }
            }
            #endregion
        }
        if (Prg_Id == "F50304")
        {
            #region
            DataTable dt6 = new DataTable(); DataTable dt5 = new DataTable();
            dt6 = fgen.getdata(frm_qstr, frm_cocd, "select trim(icode) as icode,trim(cpartno) as part_no from item where length(trim(icode))>=8");        
            vardate = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
            if (dtW != null)
            {                
                i = 0;
                foreach (DataRow gr1 in dtW.Rows)
                {
                    mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = ""; mq7 = ""; mq8 = "";
                    oporow = oDS.Tables[0].NewRow();
                    oporow["BRANCHCD"] = frm_mbr;
                    oporow["type"] = frm_vty;
                    oporow["vchnum"] = frm_vnum;
                    oporow["vchdate"] = vardate;
                    oporow["BUY_CODE"] = gr1["ford_plant_code"].ToString().Trim();
                    mq1 = fgen.seek_iname_dt(dt6, "part_no='" + gr1["Part_Number"].ToString().Trim() + "'", "icode");
                    if (mq1 != "0")
                    {
                        oporow["icode"] = mq1;
                    }
                    oporow["PARTNO"] = gr1["Part_Number"].ToString().Trim();
                    oporow["qty"] = gr1["Quantity"].ToString().Trim();
                    oporow["stdate"] = gr1["date"].ToString().Trim();
                    oporow["Lineno"] = gr1["Line_no"].ToString().Trim();
                    oporow["week"] = gr1["weeks"].ToString().Trim();
                    oporow["vend_code"] = gr1["IAIJ_ford_plant_code"].ToString().Trim();
                    oporow["eNt_by"] = frm_uname;
                    oporow["eNt_dt"] = vardate;
                    oporow["edt_by"] = "-";
                    oporow["eDt_dt"] = vardate;
                    oDS.Tables[0].Rows.Add(oporow);
                }
            }
             #endregion
        }       
    }
    ////=======================================
    protected void btniname_Click(object sender, EventArgs e)
    {//customer button
        hfname.Value = "ACODE";
        make_qry_4_popup();
        fgen.Fn_open_sseek("Select Customer Name", frm_qstr);
        // ScriptManager.RegisterStartupScript(btniname, this.GetType(), "abc", "$(document).ready(function(){openSSeek();});", true);
    }
    //// =================   
    void Type_Sel_query()
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        SQuery = "SELECT 'ED' AS FSTR,'Record Efforts Done' as NAME,'ED' AS CODE FROM dual";
    }
    //------------------------------------------------------------------------------------   

    protected void btnvalidate_ServerClick(object sender, EventArgs e)
    {
        int req = 0, i = 0, flag = 0; string app = "";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        DataTable dtn = new DataTable();
        dtn = (DataTable)ViewState["dtn"];
        if (Prg_Id == "F50302")  //
        {
            dt3 = new DataTable();
            dt3 = fgen.getdata(frm_qstr, frm_cocd, "select trim(icode) as icode,trim(cpartno) as part from item where length(trim(icodE))>=8 order by icode");

            dt4 = new DataTable();        
            //dt4 = fgen.getdata(frm_qstr, frm_cocd, "SELECT max(ordno) as ordno,to_char(orddt,'dd/mm/yyyy') as orddt  FROM SOMAS WHERE branchcd='" + frm_mbr + "' and ACODE='" + txtacode.Text + "' AND ICAT='N' group by  to_char(orddt,'dd/mm/yyyy') ORDER BY ORDNO ");
            dt4 = fgen.getdata(frm_qstr, frm_cocd, "SELECT DISTINCT max(ordno) as ordno,to_char(orddt,'dd/mm/yyyy') as orddt,PORDNO,ICODE,trim(acode) as acode FROM SOMAS WHERE branchcd='" + frm_mbr + "' and ACODE='" + txtacode.Text + "' AND ICAT='N' group by  to_char(orddt,'dd/mm/yyyy'),PORDNO,ICODE,trim(acode) ORDER BY ORDNO");
            for (int K = 0; K < dtn.Rows.Count; K++)
            {               
                mq1 = ""; mq2 = ""; mq3 = "";
                mq1 = fgen.seek_iname_dt(dt3, "part='" + dtn.Rows[K]["part_no"].ToString().Trim() + "'", "icode");
                if(mq1=="0")
                {
                    fgen.msg("-", "AMSG", "No item link on this Cpartno.Please check");
                    return;
                }
                else
                {
                    mq2 = fgen.seek_iname_dt(dt4, "icode='" + mq1 + "' and acode='" + txtacode.Text + "'", "ordno");
                    if (mq2 == "0")
                    {
                        fgen.msg("-", "AMSG","No Sale Order Found for this Part No '" + dtn.Rows[K]["part_no"].ToString().Trim() + "'.Please firstly link Sale Order");
                        return;
                    } ////else sale order agar hai us item and party ka then continued                    
                }
                if (dtn.Rows[K]["part_no"].ToString().Trim().Length == 1)
                {
                    fgen.msg("-", "AMSG", "Please Enter valid Part no.Empty Partno is not allowed!!");
                    return;
                }
            }
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This form is validated successfully");
            btnvalidate.Disabled = true;
            btnsave.Disabled = false;
            btniname.Enabled = false;

            return;
            #region this cmnt code for validation on fstr but abi no need becoz sch no same ho skte hai as per virender sir
            //dtn.Columns.Add("fstr", typeof(string));
            //DateTime schdt;
            //for (int K = 0; K < dtn.Rows.Count; K++)
            //{
            //    mq3 = fgen.seek_iname(frm_qstr, frm_cocd, " SELECT TO_DATE('" + dtn.Rows[K]["Schedule_Date"].ToString().Trim() + "','yymmdd') as dd from dual", "dd");
            //    mq4 = fgen.seek_iname(frm_qstr, frm_cocd, " SELECT TO_DATE('" + dtn.Rows[K]["Desp_Date"].ToString().Trim() + "','yymmdd') as dd from dual", "dd");
            //    dtn.Rows[K]["fstr"] = dtn.Rows[K]["Schedule_No"].ToString().Trim() + Convert.ToDateTime(mq3).ToString("dd/MM/yyyy") + dtn.Rows[K]["PART_NO"].ToString().Trim() + Convert.ToDateTime(mq4).ToString("dd/MM/yyyy");
            //}
            //ViewState["dtn"] = dtn;
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);
            //DataView view = new DataView(dtn);
            //DataTable distinctValues = view.ToTable(true, "FSTR");
            ////checking duplicate values in dataview
            //foreach (DataRow dr1 in distinctValues.Rows)
            //{
            //    DataView view2 = new DataView(dtn, "FSTR='" + dr1["FSTR"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
            //    dt2 = new DataTable();
            //    dt2 = view2.ToTable();
            //    if (dt2.Rows.Count == 1)
            //    {
            //    }
            //    else
            //    {
            //        for (int l = 0; l < dt2.Rows.Count; l++)
            //        {
            //            flag = 1;
            //            dtn.Rows[Convert.ToInt32(dt2.Rows[l]["dtsrno"].ToString())]["duplicate"] = dt2.Rows[l]["part_no"].ToString() + " " + "is Duplicate";
            //            //  app += "Same Schedule no/Date";
            //        }
            //    }
            //}
            //dt = new DataTable();
            //DataRow dr = null;

            #region checkexistitemname
            //dt4 = new DataTable();
            //dt4 = fgen.getdata(frm_qstr, frm_cocd, "select trim(icode) as icode,trim(iname) as iname,trim(cpartno) as part_no from item where length(trim(icode))>=8");
            //string chkname1 = "";
            //for (int i1 = 0; i1 < dtn.Rows.Count; i1++)
            //{
            //    mq4 = ""; mq6 = "";
            //    mq3 = fgen.seek_iname(frm_qstr, frm_cocd, " SELECT TO_DATE('" + dtn.Rows[i1]["Schedule_Date"].ToString().Trim() + "','yymmdd') as dd from dual", "dd");
            //    mq5 = Convert.ToDateTime(mq3).ToString("dd/MM/yyyy");
            //    string sysdt = DateTime.Now.AddDays(+15).ToString("dd/MM/yyyy");

            //    if (Convert.ToDateTime(mq5) > Convert.ToDateTime(sysdt))    //if schedule date is more than currdate+15 days..then not allowed
            //    {
            //        app += "Date more than 15 days of current date is not allowed";
            //        flag = 1;
            //        req = req + 1;
            //    }
            //    if (app != "")
            //    {
            //        dtn.Rows[i1]["reasonoffailure"] = app;
            //        app = "";
            //    }
            //}
            #endregion
           // ViewState["dtn"] = dtn;
            //dt = new DataTable();
            //DataTable dtn1 = new DataTable();
            //dtn1 = (DataTable)ViewState["dtn"];
            //dt = dtn1.Copy();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dt, "", "ContentPlaceHolder1_datadiv").ToString(), false);

            //if ((req > 0) || (flag == 1))
            //{
            //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This form is not validated successfully .Please download the excel file(See last two columns of excel file.) ");
            //    if (dtn.Rows.Count > 0)
            //    {
            //        dtn.Columns.Remove("dtsrno");
            //    }
            //    btnexptoexl.Visible = true;
            //    btnvalidate.Disabled = true;
            //    return;
            //}
            //if (flag == 0)
            //{
            //    btnsave.Disabled = false;
            //    fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This form is validated successfully");
            //    btnvalidate.Disabled = true;
            //    return;
            //}
            #endregion
        }
        if(Prg_Id=="F50304")
        {
            dt = new DataTable();
            dt = fgen.getdata(frm_qstr, frm_cocd, "select trim(icode) as icode,trim(cpartno) as part_no from item where length(trim(icode))>=8");                
            for (int K = 0; K < dtn.Rows.Count; K++)
            {
                mq1 = "";
                if (dtn.Rows[K]["part_number"] == "")
                {
                    fgen.msg("-", "AMSG", "Please Enter valid Part no.Empty Partno is not allowed!!");
                    return;
                }
                mq1 = fgen.seek_iname_dt(dt, "part_no='" + dtn.Rows[K]["Part_Number"].ToString().Trim() + "'", "icode");
                if (mq1 == "0")
                {
                    fgen.msg("-", "AMSG", "Please link Item code of the Part No. '" + dtn.Rows[K]["part_number"].ToString().Trim() + "' at Line No '" + dtn.Rows[K]["sno"].ToString().Trim() + "' !!");
                    return;
                } 
            }
            fgen.msg("-", "AMSG", "Dear " + frm_uname + ", This form is validated successfully");
            btnvalidate.Disabled = true;
            btnsave.Disabled = false;
            return;
        }
    }

    protected void btnexptoexl_Click(object sender, ImageClickEventArgs e)
    {
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", "-");
        DataTable dt1 = new DataTable();
        dt1 = (DataTable)ViewState["dtn"];
        if (dt1.Rows.Count > 0)
        {
            //fgen.exp_to_excel(dt1, "ms-excel", "xls", frm_cocd + "_" + DateTime.Now.ToString().Trim());
            //else fgen.msg("-", "AMSG", "No Data to Export");
            // dt1.Dispose();
            Session["send_dt"] = dt1;
            fgen.Fn_open_rptlevel("list of errors", frm_qstr);
        }
    }

    protected void btnupload_Click(object sender, EventArgs e)
    {
        string ext = "", filesavepath = "";
        //if (txtacode.Value.Trim().Length > 2)
        //{
        if (FileUpload1.HasFile)
        {
            ext = Path.GetExtension(FileUpload1.FileName).ToLower();
            filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".txt";
            FileUpload1.SaveAs(filesavepath);
            string[] readText = File.ReadAllLines(filesavepath);
            /////
            Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            /////
            if (Prg_Id == "F50302")
            {  //THIS IS 14 DAYS TXT FILE FORM
                //if(txtacode.Text=="" ||txtacode.Text=="")
                //{
                //    fgen.msg("-", "AMSG", "Please Select Customer code first!!");
                //    return;
                //}
                DataTable dtn = new DataTable();
                dtn.Columns.Add("Sno", typeof(string));
                dtn.Columns.Add("Schedule_No", typeof(string));
                dtn.Columns.Add("Schedule_Date", typeof(string));
                dtn.Columns.Add("Part_No", typeof(string));
                dtn.Columns.Add("IAIJ_Code", typeof(string));
                dtn.Columns.Add("Ford_Code", typeof(string));
                dtn.Columns.Add("Line_No", typeof(string));
                dtn.Columns.Add("Desp_Date", typeof(string));
                dtn.Columns.Add("Qty", typeof(string));
                string schno = "", schdt = "", cpartno = "", iaijcode = "", fordcode = "", line = "", despdt = "", qty = ""; int sno = 0; long dtsrno = 1;
                string[] u1 = null;
                string[] u2 = null;
                DataRow drn;
                string toRead = "N";
                foreach (string s in readText)
                {
                    #region
                    //if (s.Contains("F"))                   
                    //{
                    //    toRead = "N";
                    //}
                    if (!string.IsNullOrEmpty(s))
                    {
                        toRead = "Y";
                    }
                    if (toRead == "Y")
                    {
                        if (s.Contains("------------")) { }
                        else
                        {
                            if (1 == 2)
                            {
                                #region valueFill
                                schno = s.Substring(0, 5);
                                schdt = s.Substring(6, 6);
                                cpartno = s.Substring(12, 15);
                                iaijcode = s.Substring(28, 5);
                                fordcode = s.Substring(34, 5);
                                line = s.Substring(40, 1);
                                despdt = s.Substring(42, 6);
                                qty = s.Substring(48, 2);
                                #endregion
                            }
                            //string[] r1 = s.Split((char)9); //for tab
                            string[] r1 = s.Split(',');
                            int v = 0;
                            #region valueFill
                            foreach (string res in r1)
                            {
                                if (res.Length >= 1)
                                {
                                    if (v == 0) schno = res;
                                    if (v == 1) schdt = res;
                                    if (v == 2) cpartno = res;
                                    if (v == 3) iaijcode = res;
                                    if (v == 4) fordcode = res;
                                    if (v == 5) line = res;
                                    if (v == 6) despdt = res;
                                    if (v == 7) qty = res;
                                    v++; sno++;
                                }
                            }
                            v = 0;
                            #endregion
                            if (sno >= 1)
                            {
                                #region adding to table
                                drn = dtn.NewRow();
                                drn["Sno"] = dtn.Rows.Count + 1;
                                drn["Schedule_No"] = schno;
                                drn["Schedule_Date"] = schdt;
                                drn["Part_No"] = cpartno;
                                drn["IAIJ_Code"] = iaijcode;
                                drn["Ford_Code"] = fordcode;
                                drn["Line_No"] = line;
                                drn["Desp_Date"] = despdt;
                                drn["Qty"] = qty;
                                dtn.Rows.Add(drn);
                                dtsrno++;
                                #endregion
                                sno = 0;
                            }
                        }
                    }                 
                    //if (!string.IsNullOrEmpty(s))
                    //{
                    //    toRead = "Y";
                    //}
                    #endregion
                }
                sg1.DataSource = dtn;
                sg1.DataBind();
                ViewState["dtn"] = dtn;
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);
            }
            //////
            if (Prg_Id == "F50304")
            { //THIS IS 6 MONTHS
                DataTable dtn = new DataTable();
                dtn.Columns.Add("Sno", typeof(string));
                dtn.Columns.Add("ford_plant_code", typeof(string));
                dtn.Columns.Add("Part_Number", typeof(string));
                dtn.Columns.Add("Quantity", typeof(string));
                dtn.Columns.Add("Date", typeof(string));
                dtn.Columns.Add("Line_no", typeof(string));
                dtn.Columns.Add("weeks", typeof(string));
                dtn.Columns.Add("IAIJ_ford_plant_code", typeof(string));
                string part = "", qty = "", code = "", cpartno = "", date_ = "", line = "", week_ = "", iaijcode = ""; int sno = 0; long dtsrno = 1;
                string[] u1 = null;
                string[] u2 = null;
                DataRow drn;
                string toRead = "N";
                foreach (string s in readText)
                {
                    #region
                    //if (s.Contains("FORD"))
                    //{
                    //    toRead = "N";
                    //}
                    if (s.Contains("FKARE"))
                    {
                        toRead = "Y";
                    }
                    if (toRead == "Y")
                    {
                        if (s.Contains("------------")) { }
                        else
                        {
                            if (1 == 2)
                            {
                                #region valueFill
                                code = s.Substring(0, 5);
                                cpartno = s.Substring(6, 13);
                                qty = s.Substring(20, 1);
                                date_ = s.Substring(22, 10);
                                line = s.Substring(33, 1);
                                week_ = s.Substring(35, 1);
                                iaijcode = s.Substring(37, 5);
                                #endregion
                            }
                            //string[] r1 = s.Split((char)9);
                            string[] r1 = s.Split(',');
                            int v = 0;
                            #region valueFill
                            foreach (string res in r1)
                            {
                                if (res.Length >= 1)
                                {
                                    if (v == 0) code = res;
                                    if (v == 1) cpartno = res;
                                    if (v == 2) qty = res;
                                    if (v == 3) date_ = res;
                                    if (v == 4) line = res;
                                    if (v == 5) week_ = res;
                                    if (v == 6) iaijcode = res;
                                    v++; sno++;
                                }
                            }
                            v = 0;
                            #endregion

                            if (sno >= 1)
                            {
                                #region adding to table
                                drn = dtn.NewRow();
                                drn["Sno"] = dtn.Rows.Count + 1;
                                drn["ford_plant_code"] = code;
                                drn["Part_Number"] = cpartno;
                                drn["Quantity"] = qty;
                                drn["Date"] = date_;
                                drn["Line_no"] = line;
                                drn["weeks"] = week_;
                                drn["IAIJ_ford_plant_code"] = iaijcode;
                                dtn.Rows.Add(drn);
                                dtsrno++;
                                #endregion
                                sno = 0;
                            }
                        }
                    }
                    else
                    {
                        fgen.msg("-", "AMSG", "Please select valid file!!");
                        return;
                    }
                    //if (s.Contains("FKARE"))
                    //{
                    //    toRead = "Y";
                    //}
                    #endregion
                }
                btnvalidate.Disabled = false;
                btnsave.Disabled = true;
                sg1.DataSource = dtn;
                sg1.DataBind();
                ViewState["dtn"] = dtn;
                //  ScriptManager.RegisterStartupScript(this, this.GetType(), "JCall1", fgen.fill_handston(dtn, "", "ContentPlaceHolder1_datadiv").ToString(), false);
            }
            btnvalidate.Disabled = false;
        }
    }                      
}