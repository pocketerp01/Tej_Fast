using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.IO;
using System.Data.OleDb;
using System.Text;
using Oracle.ManagedDataAccess.Client;

using System.Web.Script.Serialization;
using System.Xml;
//using Newtonsoft.Json.Linq;
using System.Collections.Generic;
//using Newtonsoft.Json;
using System.Collections;
using System.Runtime.InteropServices;
//using System.Data.XmlReadMode;

//JSON_XML local id on sgrp backup
public partial class Om_Json_form : System.Web.UI.Page
{

    DataTable dtTable = null;
    List<KeyValuePair<string, string>>
    kvpList = new List<KeyValuePair<string, string>>();
    List<KeyValuePair<string, int>>
    kvpLevelList = new List<KeyValuePair<string, int>>();
    List<KeyValuePair<string, bool>>
    kvpLevelList1 = new List<KeyValuePair<string, bool>>();
    List<KeyValuePair<string, int>>
    kvpLevelListforparent = new List<KeyValuePair<string, int>>();
    static XmlNodeList ParentnodeList = null;
    static int Level = 1;
    static int parentid = 0;
    static string nodeparent = String.Empty;

    DataRow dr, dr1;
    string frm_url, frm_PageName, frm_qstr, frm_cocd, frm_mbr, lbl1a_Text, frm_uname, fromdt, frm_myear, CSR, frm_ulvl, frm_UserID, typePopup = "Y";
    string mhd = "", HCID, cond, vty, vchnum = "", col1, col2, col3, SQuery, Bfromdt, todt, DateRange; int x = 0;
    DataTable dt, dt1, pTable; DataRow oporow; DataSet oDS; OracleDataAdapter da; OracleCommandBuilder cb;
    string mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, vardate, rcode, pk_error, tempicode = "";
    string signedQRCode, signedInvoice, Irn, IrnQrCodeValue;
    string frm_vty, frm_vnum;
    string frm_tabname, frm_formID, frm_CDT1;
    string path = "";
    DataTable sg1_dt; DataRow sg1_dr;
    fgenDB fgen = new fgenDB();
    OracleConnection con;
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
                    DateRange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";
                    fromdt = "01/04/" + frm_myear;
                    todt = "31/03/" + Convert.ToString(Convert.ToInt32(frm_myear) + 1);
                }
                else Response.Redirect("~/login.aspx");
            }

            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl(); btndel.Visible = false;
                if (HCID == "15005")
                {
                    // dd1.Visible = false;
                    tr1.Visible = true;
                    if (frm_cocd == "DLJM")
                    {
                        txtdate.Visible = true; txtdate.ReadOnly = true;
                        txtdate_CalendarExtender.Enabled = false;
                        MaskedEditExtender1.Enabled = false;
                    }
                    else txtdate.Visible = false;
                }
                else if (HCID == "25599" || HCID == "10598" || HCID == "10597" || HCID == "25578" || HCID == "29051" || HCID == "10596")
                {
                    tr1.Visible = false; txtdate.Visible = false;// dd1.Visible = false; 
                }
                else if (HCID == "75011") { tr1.Visible = false; txtdate.Visible = true; txtdate.Text = System.DateTime.Now.ToShortDateString(); }
                else
                {
                    fill_drp(); tr1.Visible = false; txtdate.Visible = false;
                    if (HCID == "75010" || HCID == "15505a") btndel.Visible = true;
                    if (HCID == "15507a")
                    {
                        btnsave.Visible = false;
                        tr1.Visible = true;
                        lblAcode.Text = "Satge Code";
                    }
                }
                set_Val();
            }
            btnupload.Attributes.Add("Style", "Display:none");
        }

        System.IO.StringWriter strw = new System.IO.StringWriter();
        HtmlTextWriter htmw = new HtmlTextWriter(strw);
    }
    public override void VerifyRenderingInServerForm(Control control)
    { return; }
    public void enablectrl()
    {
        btnnew.Disabled = false; btnsave.Disabled = true; FileUpload1.Enabled = false; btndel.Disabled = false;
        btnext.Text = " Exit "; btnext.Enabled = true; btnhideF_s.Enabled = true; btnext.AccessKey = "X";
    }
    public void disablectrl()
    {
        btnnew.Disabled = true; btnsave.Disabled = false; FileUpload1.Enabled = true; btndel.Disabled = true;
        btnext.Text = "Cancel"; btnext.Enabled = true; btnhideF_s.Enabled = true; btnext.AccessKey = "C";
    }
    public void fill_drp()
    {
        HCID = frm_formID;
        SQuery = "";
        switch (HCID)
        {
            case "22561":
            case "22562":
            case "22563":
            case "22565":
            case "22566A":
            case "22566B":
            case "22566":
                SQuery = "Select type1 as fstr,type1||' '||name as name from type where id='Y' order by type1";
                break;
            case "75010":
                SQuery = "select * from (sELECT rownum as srno,TO_CHAR(LAST_DAY(TO_DATE(MON,'MM/YYYY')),'DD/MM/YYYY') AS FSTR,MONTH||' ('||TO_CHAR(LAST_DAY(TO_DATE(MON,'MM/YYYY')),'DD/MM/YYYY')||')' AS NAME FROM (select to_char(date '" + (Convert.ToInt32(frm_myear) - 0) + "-12-01' + numtoyminterval(level,'month'),'MONTH') as month,to_char(date '" + (Convert.ToInt32(frm_myear) - 1) + "-12-01' + numtoyminterval(level,'month'),'mm/YYYY') as mon from dual connect by level <= 15)) where srno>3";
                break;
            case "15506a":
            case "15507a":
                SQuery = "SELECT DISTINCT A.ICODE AS FSTR,B.INAME AS NAME FROM IVOUCHER A,ITEM B WHERE TRIM(A.ICODE)=TRIM(b.ICODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '15%' AND A.STORE='Y' ORDER BY A.ICODE";
                break;
            case "15505a":
                SQuery = "SELECT '0' AS FSTR,'DIRECT CONSUMED' AS NAME FROM DUAL UNION ALL SELECT '1' AS FSTR,'BOM CONSUMED' AS NAME FROM DUAL";
                break;
            case "22085":
                SQuery = "select '1' as fstr,'Mahindra File' as name from dual union all select '2' as fstr,'Bajaj File' as name from dual ";
                break;
        }
        dt = new DataTable();
        //if (SQuery.Length > 0)
        //{           
        //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
        //    dd1.DataSource = dt;
        //    dd1.DataTextField = "name";
        //    dd1.DataValueField = "fstr";
        //    dd1.DataBind();
        //    dt.Dispose();
        //    if (HCID != "75010" && HCID != "15505a" && HCID != "22085") dd1.Items.Insert(0, "All Items");
        //    dd1.Visible = true;
        //}
    }
    //------------------------------------------------------------------------------------
    public void set_Val()
    {

        frm_tabname = "OM_WB_JSON";
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
        if (frm_formID == "JSON_XML" || frm_formID == "F50118")
        {
            lblheader.Text = "XML UPLOAD FORM";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "XM");
            Label1.InnerText = "Xml_File_No";
        }
        else
        {
            lblheader.Text = "JSON UPLOAD FORM";
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", "JS");
            Label1.InnerText = "Json_File_No";
        }

        lbl1a.Text = frm_vty;
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        set_Val();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        fgen.EnableForm(this.Controls);
        disablectrl();
        FileUpload1.Focus();
        frm_vnum = fgen.next_no(frm_qstr, frm_cocd, "SELECT MAX(VCHNUM) AS VCH FROM " + frm_tabname + " WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='" + frm_vty + "' and VCHDATE " + DateRange + "", 6, "VCH");
        txtvchnum.Value = frm_vnum;
        txtvchdate.Value = fgen.Fn_curr_dt(frm_cocd, frm_qstr);
        fgen.EnableForm(this.Controls);
    }
    //------------------------------------------------------------------------------------
    public void clearctrl()
    {
        // hffield.Value = "";
        edmode.Value = "";
    }

    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        HCID = frm_formID;
        if (sg3.Rows.Count > 0)
        {
            fgen.msg("-", "SMSG", "Are you Sure You want to Update");
        }
        else fgen.msg("-", "AMSG", "No File Uploaded");
    }
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        hffild.Value = "Del";
        HCID = frm_formID;
        switch (HCID)
        {
            case "75010":
                SQuery = "Select distinct branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum as entry_no,to_Char(vchdate,'dd/mm/yyyy') as entry_Date,ent_by,to_ChaR(ent_dt,'dd/mm/yyyy') as ent_dt from attnp where branchcd='" + frm_mbr + "' and vchdate " + DateRange + "";
                break;

            case "JSON_XML":

                break;
        }

        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);

        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btnext_Click(object sender, EventArgs e)
    {
        if (btnext.Text == " Exit ")
        { Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr); }
        else
        {
            fgen.ResetForm(this.Controls);
            fgen.DisableForm(this.Controls);
            enablectrl();
            sg1.DataSource = null;
            sg1.DataBind();
            sg1.Visible = false;
            sg3.DataSource = null;
            sg3.DataBind();
            // sg3.Visible = false;
            btnexp.Visible = false;
        }
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {

        HCID = frm_formID;
        if (hffild.Value.Trim() == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                switch (HCID)
                {
                    case "75010":
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from attnp a where a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + edmode.Value + "'");
                        break;
                }
                fgen.msg("-", "AMSG", "Details are deleted for order " + edmode.Value.Substring(4, 6) + "");
                fgen.ResetForm(this.Controls); edmode.Value = "";
            }
        }
        else if (hffild.Value == "TFRTOREJ")
        {
            hffild.Value = "";
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();

            if (col1 == "Y")
            {
                int i = 0;
                do
                {
                    vty = "3A";
                    vchnum = fgen.next_no(frm_qstr, frm_cocd, "select max(nvl(vchnum,0))+" + i + " as vch from IVOUCHER where branchcd='" + frm_mbr + "' and type='" + vty + "' and vchdate " + DateRange + "", 6, "vch");
                    pk_error = fgen.chk_pk(frm_qstr, frm_cocd, "IVOUCHER" + frm_mbr + vty + vchnum + System.DateTime.Now.ToString("dd/MM/yyyy"), frm_mbr, vty, vchnum, DateTime.Now.ToString("dd/MM/yyyy"), "", frm_uname);
                    i++;
                }
                while (pk_error == "Y");

                oDS = new DataSet();
                oDS = fgen.fill_schema(frm_qstr, frm_cocd, "IVOUCHER");

                for (i = 0; i < sg1.Rows.Count; i++)
                {
                    if (sg1.Rows[i].Cells[3].Text.Trim().Length > 4 && sg1.Rows[i].Cells[8].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "") == "N")
                    {
                        oporow = oDS.Tables[0].NewRow();
                        oporow["BRANCHCD"] = frm_mbr;
                        oporow["TYPE"] = vty;
                        oporow["vchnum"] = vchnum;
                        oporow["vchdate"] = DateTime.Now.ToString("dd/MM/yyyy");

                        oporow["acode"] = txtacode.Text.Trim();
                        oporow["stage"] = txtacode.Text.Trim();
                        oporow["srno"] = (i + 1);
                        oporow["morder"] = (i + 1);
                        oporow["RCODE"] = sg1.Rows[i].Cells[2].Text.Trim();
                        oporow["ICODE"] = sg1.Rows[i].Cells[2].Text.Trim();
                        oporow["iqtyin"] = 0;
                        oporow["iqty_chl"] = 0;

                        oporow["iqtyout"] = sg1.Rows[i].Cells[7].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                        oporow["PURPOSE"] = sg1.Rows[i].Cells[6].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                        oporow["btchno"] = sg1.Rows[i].Cells[4].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                        oporow["binno"] = sg1.Rows[i].Cells[5].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");

                        oporow["rec_iss"] = "C";
                        oporow["iopr"] = "6R";
                        oporow["INSPECTED"] = "N";

                        oporow["desc_"] = "Tfr to Rejection from " + txtaname.Text + "";

                        oporow["naration"] = "Tfr from EDI Process";
                        oporow["rej_rw"] = 0;
                        oporow["acpt_ud"] = 0;
                        oporow["store"] = "W";

                        oporow["ent_by"] = frm_uname;
                        oporow["ent_dt"] = System.DateTime.Now;
                        oporow["edt_by"] = "-";
                        oporow["edt_dt"] = System.DateTime.Now;

                        oDS.Tables[0].Rows.Add(oporow);
                        //************************
                        oporow = oDS.Tables[0].NewRow();
                        oporow["BRANCHCD"] = frm_mbr;
                        oporow["TYPE"] = vty;
                        oporow["vchnum"] = vchnum;
                        oporow["vchdate"] = DateTime.Now.ToString("dd/MM/yyyy");

                        oporow["acode"] = "6R";
                        oporow["stage"] = "6R";
                        oporow["srno"] = (i + 1);
                        oporow["morder"] = (i + 1);
                        oporow["RCODE"] = sg1.Rows[i].Cells[2].Text.Trim();
                        oporow["ICODE"] = sg1.Rows[i].Cells[2].Text.Trim();

                        oporow["iqty_chl"] = 0;
                        oporow["iqtyout"] = 0;
                        oporow["rec_iss"] = "D";
                        oporow["iopr"] = "-";

                        oporow["iqtyin"] = sg1.Rows[i].Cells[7].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                        oporow["PURPOSE"] = sg1.Rows[i].Cells[6].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                        oporow["btchno"] = sg1.Rows[i].Cells[4].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");
                        oporow["binno"] = sg1.Rows[i].Cells[5].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");

                        oporow["acpt_ud"] = sg1.Rows[i].Cells[7].Text.Trim().Replace("-", "").Replace("&nbsp;", "").Replace("&amp;", "");

                        oporow["desc_"] = "Tfr from " + txtaname.Text + " to Rejection";

                        oporow["naration"] = "Tfr from EDI Process";
                        oporow["rej_rw"] = 0;
                        oporow["store"] = "W";
                        oporow["INSPECTED"] = "N";

                        oporow["ent_by"] = frm_uname;
                        oporow["ent_dt"] = System.DateTime.Now;
                        oporow["edt_by"] = "-";
                        oporow["edt_dt"] = System.DateTime.Now;

                        oDS.Tables[0].Rows.Add(oporow);
                    }
                }
                if (oDS.Tables[0].Rows.Count > 0) fgen.save_data(frm_qstr, frm_cocd, oDS, "IVOUCHER");

                fgen.msg("-", "AMSG", "Transferred to Rejection Stage'13'Doc No. " + vchnum);
            }
        }
        else
        {
            col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
            col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
            col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

            switch (hffild.Value.Trim())
            {
                case "tacode":
                    txtacode.Text = col1;
                    txtaname.Text = col2;
                    break;
                case "Del":
                    edmode.Value = "";
                    edmode.Value = col1;
                    fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete");
                    hffild.Value = "D";
                    break;
            }
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        string confirmValue = Request.Form["confirm_value"];
        string hfd = "";
        hfd = hffild.Value;

        if (Request.Cookies["REPLY"].Value.ToString().Trim() == "Y")
        {
            vchnum = "";
            HCID = frm_formID;
            if (HCID == "JSON_XML" || frm_formID == "F50118") //FOR XML SAVING
            {
                #region
                hfxml.Value = "";
                oDS = new DataSet();
                oDS = fgen.fill_schema(frm_qstr, frm_cocd, "Om_wb_Json");
                foreach (GridViewRow gr1 in sg3.Rows)
                {
                    oporow = oDS.Tables[0].NewRow();
                    oporow["branchcd"] = frm_mbr;
                    oporow["type"] = "XM";
                    oporow["vchnum"] = txtvchnum.Value.Trim();
                    vchnum = txtvchnum.Value.Trim();
                    oporow["vchdate"] = System.DateTime.Now.ToString();
                    oporow["srno"] = (gr1.RowIndex + 1);
                    oporow["acode"] = txtacode.Text.Trim();
                    oporow["icode"] = mhd.Trim();
                    oporow["COL1"] = gr1.Cells[0].Text.Trim().Replace("&nbsp;", "-");
                    oporow["COL2"] = gr1.Cells[1].Text.Trim().Replace("&nbsp;", "-");
                    oporow["COL3"] = gr1.Cells[2].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL4"] = gr1.Cells[3].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL5"] = gr1.Cells[5].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL6"] = gr1.Cells[6].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL7"] = gr1.Cells[7].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL8"] = gr1.Cells[8].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL9"] = gr1.Cells[9].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL10"] = gr1.Cells[10].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL11"] = gr1.Cells[11].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL12"] = gr1.Cells[12].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL13"] = gr1.Cells[13].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL14"] = gr1.Cells[14].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL15"] = gr1.Cells[15].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL16"] = gr1.Cells[16].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL17"] = gr1.Cells[17].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL18"] = gr1.Cells[18].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL19"] = gr1.Cells[19].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL20"] = gr1.Cells[20].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL21"] = gr1.Cells[21].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL22"] = gr1.Cells[22].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL23"] = gr1.Cells[23].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL24"] = gr1.Cells[24].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL25"] = gr1.Cells[25].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL26"] = gr1.Cells[26].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL27"] = gr1.Cells[27].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL28"] = gr1.Cells[28].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL29"] = gr1.Cells[29].Text.Trim().Replace("&nbsp;", "0");
                    oporow["COL30"] = gr1.Cells[30].Text.Trim().Replace("&nbsp;", "0");
                    oporow["ent_by"] = frm_uname;
                    oporow["ent_dt"] = System.DateTime.Now.ToString();
                    oDS.Tables[0].Rows.Add(oporow);
                }
                fgen.save_data(frm_qstr, frm_cocd, oDS, "Om_wb_Json");
                #endregion
            }
            if (vchnum.Length > 0) fgen.msg("-", "AMSG", "Updation Successfully Completed'13'Entry No " + vchnum + "");
            else fgen.msg("-", "AMSG", "Updation Successfully Completed");
            fgen.DisableForm(this.Controls); enablectrl(); fgen.ResetForm(this.Controls);
            sg3.DataSource = null;
            sg3.DataBind();
        }
    }
    protected void btnupload_Click(object sender, EventArgs e)
    {
        sg1.DataSource = null; sg1.DataBind();
        sg3.DataSource = null; sg3.DataBind();
        string ext = "", excelConString = "", filesavepath = "";
        HCID = frm_formID;
        if (FileUpload1.HasFile)
        {
            ext = Path.GetExtension(FileUpload1.FileName).ToLower();
            if (ext == ".json" || ext == ".txt")
            {
                path = @"c:\TEJ_ERP\WTEWAYBILL.json";//atual json file
                path = @"c:\TEJ_ERP\EWAYBILL.json";
                if (ext == ".json")
                {
                    filesavepath = AppDomain.CurrentDomain.BaseDirectory + "Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".json";
                    FileUpload1.SaveAs(filesavepath);
                    excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filesavepath + ";Extended Properties=Text;";
                }
                sg1_dt = new DataTable();
                create_tab();
                sg1_add_blankrows();
                //   readjsonfile1(path); //this fun for that json file which is provided by pkg sir
                readjsonfile(path); //this fun for finsys generated fun
            }
            else if (ext == ".xml")
            {
                txtAttch.Text = FileUpload1.FileName;
                filesavepath = AppDomain.CurrentDomain.BaseDirectory + "\\tej-base\\Upload\\file" + DateTime.Now.ToString("ddMMyyyyhhmmfff") + ".xml";
                FileUpload1.SaveAs(filesavepath);
                path = filesavepath;
                readxmlfile(path);
                hfxml.Value = "XMLSAVE";
            }
            else fgen.msg("-", "AMSG", "File is not in valid format'13'Be sure this file is XML(.xml)");
        }
    }
    public void exp()
    {
        dt = new DataTable();
        dt = (DataTable)ViewState["SG2"];
        if (dt.Rows.Count > 0)
        {
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename= " + frm_cocd + "_" + DateTime.Now.ToString().Trim() + ".xls");
            Response.ContentType = "application/ms-excel";
            string str = string.Empty;
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write(str + dtcol.ColumnName);
                str = "\t";
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }
    }
    protected void sg1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        HCID = frm_formID;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblsr = (Label)e.Row.FindControl("lblsr");
            lblsr.Text = ((sg1.PageIndex * sg1.PageSize) + e.Row.RowIndex + 1).ToString();

            if ((HCID == "75010" || HCID == "75011") && frm_cocd == "YTEC")
            {
                e.Row.Cells[0].Style["display"] = "none";
                sg1.HeaderRow.Cells[0].Style["display"] = "none";
            }
        }
    }
    protected void btnexp_Click(object sender, EventArgs e)
    {
        exp();
    }
    protected void btnacode_Click(object sender, ImageClickEventArgs e)
    {
        hffild.Value = "tacode";
        //fgen.send_cookie("xid", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        SQuery = "Select acode as fstr,aname as party_name,acode as code from famst where trim(acode) like '16%' order by aname";
        if (HCID == "15507a") SQuery = "SELECT TYPE1 AS FSTR,NAME AS STG_NAME,TYPE1 AS CODE FROM TYPE WHERE ID='1' ORDER BY TYPE1";
        fgen.send_cookie("srchSql", SQuery);
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        fgen.Fn_open_sseek("-", frm_qstr);
    }
    protected void btntfr_Click(object sender, EventArgs e)
    {
        if (frm_cocd == "MANU")
        {
            hffild.Value = "TFRTOREJ";
            fgen.msg("-", "CMSG", "Are Sure you want to Tfr from " + txtaname.Text + " to Rejection Stage");
        }
    }
    public void OnConfirm(object sender, EventArgs e)
    {
        string confirmValue = Request.Form["confirm_value"];
        if (confirmValue == "Yes")
        {
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked YES!')", true);
        }
        else
        {
            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('You clicked NO!')", true);
        }
    }
    //public object Get()
    //{
    //    string allText = System.IO.File.ReadAllText(@"c:\data.json");

    //    object jsonObject = JsonConvert.DeserializeObject(allText);
    //    return jsonObject;
    //}

    public DataTable jsonDataDiplay(string jsonfilepath)
    {
        StreamReader sr = new StreamReader(jsonfilepath);
        string json = sr.ReadToEnd();
        //dynamic table = JsonConvert.DeserializeObject(json);
        DataTable newTable = new DataTable();
        newTable.Columns.Add("Line", typeof(string));
        newTable.Columns.Add("Direction", typeof(string));
        newTable.Columns.Add("Stations", typeof(string));
        newTable.Columns.Add("MRTShuttleDirection", typeof(string));

        //foreach (var row in table.value.AffectedSegments)
        //{
        //    newTable.Rows.Add(row.Line, row.Direction, row.Stations, row.MRTShuttleDirection);
        //}
        return newTable;
    }

    public void readxml(string xmlpath)
    {
        DataSet ds = new DataSet();
        //   ds.ReadXml(Server.MapPath("~/Parts.Xml"));
        ds.ReadXml(xmlpath);
        sg3.DataSource = ds;
        sg3.DataBind();
    }

    public void readxmlfile(string xmlfilepath)
    {
        #region
        DataSet dataSet = new DataSet();
        dt = new DataTable();
        dataSet.ReadXml(xmlfilepath, XmlReadMode.InferSchema);//here read xml
        dt = new DataTable();
        dt1 = new DataTable();
        dt1.Columns.Add("col0", typeof(string));
        dt1.Columns.Add("col1", typeof(string));
        dt1.Columns.Add("col2", typeof(string));
        dt1.Columns.Add("col3", typeof(string));
        dt1.Columns.Add("col4", typeof(string));
        dt1.Columns.Add("col5", typeof(string));
        dt1.Columns.Add("col6", typeof(string));
        dt1.Columns.Add("col7", typeof(string));
        dt1.Columns.Add("col8", typeof(string));
        dt1.Columns.Add("col9", typeof(string));
        dt1.Columns.Add("col10", typeof(string));
        dt1.Columns.Add("col11", typeof(string));
        dt1.Columns.Add("col12", typeof(string));
        dt1.Columns.Add("col13", typeof(string));
        dt1.Columns.Add("col14", typeof(string));
        dt1.Columns.Add("col15", typeof(string));
        dt1.Columns.Add("col16", typeof(string));
        dt1.Columns.Add("col17", typeof(string));
        dt1.Columns.Add("col18", typeof(string));
        dt1.Columns.Add("col19", typeof(string));
        dt1.Columns.Add("col20", typeof(string));
        dt1.Columns.Add("col21", typeof(string));
        dt1.Columns.Add("col22", typeof(string));
        dt1.Columns.Add("col23", typeof(string));
        dt1.Columns.Add("col24", typeof(string));
        dt1.Columns.Add("col25", typeof(string));
        dt1.Columns.Add("col26", typeof(string));
        dt1.Columns.Add("col27", typeof(string));
        dt1.Columns.Add("col28", typeof(string));
        dt1.Columns.Add("col29", typeof(string));
        dt1.Columns.Add("col30", typeof(string));
        //==============
        foreach (DataTable table in dataSet.Tables)
        {
            int cnt = table.Columns.Count;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                dr = dt1.NewRow();
                for (int j = 0; j < cnt; j++)
                {
                    dr["col" + j + ""] = table.Rows[i][j].ToString().Trim();
                }
                dt1.Rows.Add(dr);
            }
        }
        sg3.DataSource = dt1;
        sg3.DataBind();
        #endregion


        #region another code...by this function only one row is showing in grid
        //if (FileUpload1.HasFile)
        //{
        //    string fileExt = System.IO.Path.GetExtension(FileUpload1.FileName);
        //    if (fileExt == ".xml")
        //    {
        //        using (MemoryStream stream = new MemoryStream(FileUpload1.FileBytes))     // Using FileBytes because I can’t save file to server
        //        {
        //            XmlDocument document = new XmlDocument();
        //            document.Load(stream);
        //            //stream.Position = 1;
        //            string xmlFile;
        //            using (StreamReader inputStreamReader = new StreamReader(FileUpload1.PostedFile.InputStream))
        //            {
        //                xmlFile = inputStreamReader.ReadToEnd();
        //            }
        //            XmlTextReader reader = new XmlTextReader(new StringReader(xmlFile));

        //            DataSet dataSet = new DataSet();
        //            dataSet.ReadXml(reader);                        
        //            sg3.DataSource = dataSet.Tables[0];
        //            sg3.DataBind();
        //        }
        //    }
        //}
        #endregion
    }

    public void readjsonfile(string jsonfilepath)
    { //this fun for finsys generated json file
        // string jsonpath = "c:\\TEJ_erp\\WTEWAYBILL_g.json";

        bool hascols = false;
        using (StreamReader r = new StreamReader(path))
        {
            string json = r.ReadToEnd();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var items = jss.Deserialize<object>(json);
            var ds1 = jss.Deserialize<Dictionary<string, object>>(json);
            var ds = new KeyValuePair<string, object>();
            int k = 0;
            foreach (var d in ds1)
            {
                if (k == 1) ds = d;
                k++;
            }

            int a = 0;

            var dsvals = ((ArrayList)ds.Value);

            DataTable dt = new DataTable();

            var dic = new Dictionary<string, object>();
            foreach (var v in dsvals)
            {
                dic = ((Dictionary<string, object>)v);
                break;
            }

            if (!hascols)
            {
                //dt.Columns.Add(k1.Key);
                //dt.Columns.Add(k2.Key);
                //dt.Columns.Add(k3.Key);
                //dt.Columns.Add(k4.Key);
                foreach (var d in dic)
                {
                    dt.Columns.Add(d.Key.ToString());
                }
                hascols = true;
            }

            foreach (var v in dsvals)
            {
                dic = ((Dictionary<string, object>)v);
                DataRow dr = dt.NewRow();
                //dr[k1.Key] = k1.Value;
                //dr[k2.Key] = k2.Value;
                //dr[k3.Key] = k3.Value;
                //dr[k4.Key] = k4.Value;
                foreach (var d in dic)
                {
                    dr[d.Key.ToString()] = d.Value.ToString();
                }
                dt.Rows.Add(dr);
            }
            ///Code End
            ///
            sg3.DataSource = dt;
            sg3.DataBind();
        }

    }

    public void readjsonfile1(string jsonfilepath)
    {// this code for json file which is given by pkg sir
        // string jsonpath = "c:\\TEJ_erp\\WTEWAYBILL.json";
        bool hascols = false;
        using (StreamReader r = new StreamReader(path))
        {
            string json = r.ReadToEnd();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var items = jss.Deserialize<object>(json);
            var ds = jss.Deserialize<Dictionary<string, object>>(json);
            var ls = new KeyValuePair<string, object>();
            var k1 = new KeyValuePair<string, object>();
            var k2 = new KeyValuePair<string, object>();
            var k3 = new KeyValuePair<string, object>();
            var k4 = new KeyValuePair<string, object>();
            var k5 = new KeyValuePair<string, object>();
            int a = 0;
            foreach (var d in ds)
            {
                if (a == 0) ls = d;
                if (a == 1) k1 = d;
                if (a == 2) k2 = d;
                if (a == 3) k3 = d;
                if (a == 4) k4 = d;
                if (a == 5) k5 = d;
                a++;

            }
            DataTable dt = new DataTable();


            var vals = ((ArrayList)ls.Value);
            var dic = new Dictionary<string, object>();
            foreach (var v in vals)
            {
                dic = ((Dictionary<string, object>)v);
                break;
            }

            if (!hascols)
            {
                dt.Columns.Add(k1.Key);
                dt.Columns.Add(k2.Key);
                dt.Columns.Add(k3.Key);
                dt.Columns.Add(k4.Key);
                foreach (var d in dic)
                {
                    dt.Columns.Add(d.Key.ToString());
                }
                hascols = true;
            }

            foreach (var v in vals)
            {
                dic = ((Dictionary<string, object>)v);
                DataRow dr = dt.NewRow();
                dr[k1.Key] = k1.Value;
                dr[k2.Key] = k2.Value;
                dr[k3.Key] = k3.Value;
                dr[k4.Key] = k4.Value;
                foreach (var d in dic)
                {
                    dr[d.Key.ToString()] = d.Value.ToString();
                }
                dt.Rows.Add(dr);
            }
            ///Code End
            ///
            sg3.DataSource = dt;
            sg3.DataBind();
        }

    }

    public void readjson(string jsonfilepath)
    {//this is not working
        if (File.Exists(jsonfilepath))
        {
            using (StreamReader r = new StreamReader(jsonfilepath))
            {
                string json = r.ReadToEnd();
                JavaScriptSerializer jss = new JavaScriptSerializer();
                var items = jss.Deserialize<invFieldJSON[]>(json);//real            ////for old format of json
                //     // Model.MyType obj = JSON.Deserialize<Model.MyType>(data);
                //  var items = jss.Deserialize<object>(json); //==============isme fill hotu json...but library ni add hori
                //  List<object> collection = (List<object>)items;  
                //var jsonData = JsonConvert.SerializeObject(orderViewModel);
                //   var ff = jss.Deserialize<JPropertyDescriptor>(json);
                #region
                //foreach (var o in collection)
                //{
                //    if (o.key == "Push_Data_List")
                //    {
                //    }
                //    else if (o.key == "Year")
                //    {

                //    }
                //    else if (o.key == "Month")
                //    {

                //    }
                //    else if (o.key == "EFUserName")
                //    {

                //    }
                //    else if (o.key == "EFPassword")
                //    {

                //    }
                //    else if (o.key == "CDKey")
                //    {

                //    }
                //}
                #endregion
                #region old code
                //foreach (var item in items)
                //{
                //    sg1_dr = sg1_dt.NewRow();
                //    if (item != null)
                //    {
                //        signedInvoice = item.SignedInvoice;
                //        signedQRCode = item.SignedQRCode;
                //        Irn = item.Irn;
                //    }
                //    sg1_dr["sg1_h1"] = signedInvoice;
                //    sg1_dr["sg1_h2"] = signedQRCode;
                //    sg1_dr["sg1_h3"] = Irn;
                //    sg1_dt.Rows.Add(sg1_dr);
                //}
                #endregion
                sg1.DataSource = sg1_dt;
                sg1.DataBind();
            }
        }
    }

    public void create_tab()
    {
        sg1_dt = new DataTable();
        sg1_dr = null;
        sg1_dt.Columns.Add(new DataColumn("sg1_h1", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h2", typeof(string)));
        sg1_dt.Columns.Add(new DataColumn("sg1_h3", typeof(string)));
    }
    public void sg1_add_blankrows()
    {
        sg1_dr = sg1_dt.NewRow();
        sg1_dr["sg1_h1"] = "-";
        sg1_dr["sg1_h2"] = "-";
        sg1_dr["sg1_h3"] = "-";
        sg1_dt.Rows.Add(sg1_dr);
    }
}
class invFieldJSON
{
    public string ErrorMessage { get; set; }
    public string ErrorCode { get; set; }
    public string Status { get; set; }
    public string GSTIN { get; set; }
    public string DocNo { get; set; }
    public string DocType { get; set; }
    public string DocDate { get; set; }
    public string Irn { get; set; }
    public string AckDate { get; set; }
    public string AckNo { get; set; }
    public string SignedInvoice { get; set; }
    public string SignedQRCode { get; set; }
    public string IrnStatus { get; set; }
}

public class IEnumerable
{
    public string RootObject { get; set; }
}
public class AffectedSegment
{
    public string Line { get; set; }
    public string Direction { get; set; }
    public string Stations { get; set; }
    public string MRTShuttleDirection { get; set; }
}
public class Message
{
    public string Content { get; set; }
    public string CreatedDate { get; set; }
}

public class Value
{
    public int Status { get; set; }
    public List<AffectedSegment> AffectedSegments { get; set; }
    public List<Message> Message { get; set; }
}

public class RootObject
{
    public Value value { get; set; }
}