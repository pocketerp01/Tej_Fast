using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Web.UI.WebControls.WebParts;



public partial class om_payments : System.Web.UI.Page
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
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_PageName;
    string frm_tabname, frm_myear, frm_ulvl, frm_formID, frm_UserID, frm_CDT1;
    //double double_val2, double_val1;
    fgenDB fgen = new fgenDB();

    public string action1 = string.Empty;
    public string hash1 = string.Empty;
    public string txnid1 = string.Empty;
    string sUrl = "";
    string fUrl = "";

    protected void Page_Load(object sender, EventArgs e)
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

        }
        if (!Page.IsPostBack)
        {
            fgen.DisableForm(this.Controls);
            enablectrl();


        }
        frm_cocd = "TEST";
        frm_tabname = "GST";
        frm_vty = "AP";
        vardate = DateTime.Now.ToString("dd/MM/yyyy");
        if (ViewState["QSTR"] == null)
        {
            if (frm_qstr == "" || frm_qstr == null)
                frm_qstr = Guid.NewGuid().ToString().Substring(0, 20).ToUpper();

            ViewState["QSTR"] = frm_qstr;
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CDT1", "01/01/" + DateTime.Now.Year);
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_CDT2", "31/12/" + DateTime.Now.Year);
            string constr = ConnInfo.connString(frm_cocd);
            fgenMV.Fn_Set_Mvar(frm_qstr, "CONN", constr);
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_TABNAME", frm_tabname);
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_VTY", frm_vty);
        }
        frm_qstr = ViewState["QSTR"].ToString();
        doc_nf.Value = "VCHNUM";
        doc_df.Value = "VCHDATE";

        key.Value = ConfigurationManager.AppSettings["MERCHANT_KEY"];
        sUrl = ConfigurationManager.AppSettings["SUCCESS_URL"];
        fUrl = ConfigurationManager.AppSettings["FAIL_URL"];
    }
    //------------------------------------------------------------------------------------
    public void enablectrl()
    {
        btnnew.Disabled = false; btnsave.Disabled = true; btncancel.Visible = false; btnhideF.Enabled = true; btnhideF_s.Enabled = true;
        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();
    }
    //------------------------------------------------------------------------------------
    public void disablectrl()
    {
        btnnew.Disabled = true;
        btnsave.Disabled = false;
        btnhideF.Enabled = true;
        btnhideF_s.Enabled = true;
        btncancel.Visible = true;
    }
    //------------------------------------------------------------------------------------
    public void clearctrl()
    {
        hffield.Value = "";
        edmode.Value = "";
    }
    //------------------------------------------------------------------------------------
    public void make_qry_4_popup()
    {

        SQuery = "";
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        btnval = hffield.Value;
        if (CSR.Length > 1) cond = " and trim(a.ccode)='" + cond + "'";
        switch (btnval)
        {

            case "New":
                Type_Sel_query();
                break;
        }
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
        fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
    }
    //------------------------------------------------------------------------------------
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {        
        clearctrl();
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        hffield.Value = "New";
        newCase(frm_vty);
    }

    void newCase(string vty)
    {
        if (col1 == "") return;
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        disablectrl();
        fgen.EnableForm(this.Controls);
        //-------------------------------------------
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
    }
    //------------------------------------------------------------------------------------
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");

        fgen.fill_dash(this.Controls);

        if (txtcname.Value.Trim().Length < 4 || txtcname.Value.Trim() == "-")
        {
            fgen.msg("-", "AMSG", " Please Enter Valid Company Code !!");
            return;
        }
        if (txtcperson.Value.Trim().Length < 1 || txtcperson.Value.Trim() == "-")
        {
            fgen.msg("-", "AMSG", " Please Enter Valid Contact Details !!");
            return;
        }
        if (txtcname.Value.Trim().Length < 1 || txtcname.Value.Trim() == "-")
        {
            fgen.msg("-", "AMSG", " Please Enter Valis Company Code !!");
            return;
        }

        string mandField = "";
        mandField = fgen.checkMandatoryFields(this.Controls, dtCol);
        if (mandField.Length > 1)
        {
            fgen.msg("-", "AMSG", mandField);
            return;
        }
        fgen.msg("-", "SMSG", "Are You Sure, You Want To Proceed to Pay!!");
    }

    protected void txtccode_textchanged(object sender, EventArgs e)
    {
        if (txtccode.Text != "")
        {
            txtcname.Value = fgen.seek_iname(frm_qstr, frm_cocd, "select full_name from evas where upper(trim(USERNAME)) = '" + txtccode.Text.Trim().ToUpper() + "'", "full_name");
            if (txtcname.Value == "0") txtcname.Value = "";
            txtcperson.Focus();
        }
        else
        {
            txtcname.Value = "";
        }
    }
    //------------------------------------------------------------------------------------
    protected void btncancel_ServerClick(object sender, EventArgs e)
    {
        fgen.ResetForm(this.Controls);
        fgen.DisableForm(this.Controls);
        clearctrl();
        enablectrl();

        create_tab();
        create_tab2();
        create_tab3();
        create_tab4();

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
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                // Deleing data from Main Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " a where a.branchcd||trim(a.ID)||trim(a." + doc_nf.Value + ")||to_char(a." + doc_df.Value + ",'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");
                // Deleing data from Sr Ctrl Table
                fgen.execute_cmd(frm_qstr, frm_cocd, "delete from wsr_ctrl a where a.branchcd||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1") + "'");

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

            }
        }
    }
    //------------------------------------------------------------------------------------
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {

        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        frm_tabname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_TABNAME");
        frm_vty = "AP";
        frm_tabname = "GST";
        if (hffield.Value == "List")
        {
            PrdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");

            SQuery = "SELECT a.Id,a.Type1,a.Name,a.Typedpt,a.Ent_by,to_char(a.ent_Dt,'dd/mm/yyyy') as Ent_Dtd,a.Edt_by,to_char(a.edt_Dt,'dd/mm/yyyy') as Edt_Dtd FROM " + frm_tabname + " a where a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "'  order by a.type1 ";
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

                    save_it = "Y";
                    if (save_it == "Y")
                    {
                        string doc_is_ok = "";
                        frm_vnum = fgen.Fn_next_doc_no(frm_qstr, frm_cocd, frm_tabname, doc_nf.Value, doc_df.Value, frm_mbr, frm_vty, vardate, frm_uname, Prg_Id);
                        doc_is_ok = fgenMV.Fn_Get_Mvar(frm_qstr, "U_NUM_OK");
                        if (doc_is_ok == "N") { fgen.msg("-", "AMSG", "Server is Busy , Please Re-Save the Document"); return; }
                    }

                    // If Vchnum becomes 000000 then Re-Save
                    if (frm_vnum == "000000") btnhideF_Click(sender, e);

                    save_fun();

                    string ddl_fld1;
                    string ddl_fld2;
                    ddl_fld1 = frm_mbr + frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    ddl_fld2 = frm_vty + fgenMV.Fn_Get_Mvar(frm_qstr, "fstr");
                    if (edmode.Value == "Y")
                    {

                        fgen.execute_cmd(frm_qstr, frm_cocd, "update " + frm_tabname + " set branchcd='DD' where branchcd||trim(ID)||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='" + ddl_fld1 + "'");


                    }
                    fgen.save_data(frm_qstr, frm_cocd, oDS, frm_tabname);

                    if (edmode.Value == "Y")
                    {
                        fgen.msg("-", "AMSG", lblheader.Text + " " + " Updated Successfully");
                        fgen.execute_cmd(frm_qstr, frm_cocd, "delete from " + frm_tabname + " where branchcd||trim(ID)||trim(" + doc_nf.Value + ")||to_char(" + doc_df.Value + ",'dd/mm/yyyy')='DD" + ddl_fld2 + "'");
                    }
                    else
                    {
                        if (save_it == "Y")
                        {
                            submitPayment();

                            fgen.msg("-", "AMSG", "Data Saved");

                        }
                        else
                        {
                            fgen.msg("-", "AMSG", "Data Not Saved");
                        }
                    }


                    #region Email Sending Function
                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    ////html started                            
                    //sb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
                    //sb.Append("<h3>" + fgenCO.chk_co(frm_cocd) + "</h3>");
                    //sb.Append("<br>Dear Sir/Mam,<br> This is to advise that the following " + lblheader.Text + " has been saved by " + frm_uname + ".<br><br>");

                    ////table structure
                    //sb.Append("<table border=1 cellspacing=1 cellpadding=1 style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; color: #474646'>");

                    //sb.Append("<tr style='color: #FFFFFF; background-color: #0099FF; font-weight: 700; font-family: Arial, Helvetica, sans-serif'>" +
                    //"<td><b>SubGrp Code</b></td><td><b>SubGrp Name</b></td><td><b>User Name</b></td><td><b>Activity Date</b></td><td><b>ID</b></td>");
                    ////vipin
                    ////foreach (GridViewRow gr in sg1.Rows)
                    ////{
                    ////    if (gr.Cells[13].Text.Trim().Length > 4)
                    ////    {

                    //sb.Append("<tr>");
                    //sb.Append("<td>");
                    //sb.Append(txtemail.Value.Trim());
                    //sb.Append("</td>");
                    //sb.Append("<td>");
                    //sb.Append(txtcname.Value.Trim());
                    //sb.Append("</td>");
                    //sb.Append("<td>");
                    //sb.Append(frm_uname);
                    //sb.Append("</td>");
                    //sb.Append("<td>");
                    //sb.Append(vardate);
                    //sb.Append("</td>");
                    //sb.Append("<td>");
                    //sb.Append(Prg_Id);
                    //sb.Append("</td>");
                    //sb.Append("</tr>");
                    ////    }
                    ////}
                    //sb.Append("</table></br>");

                    //sb.Append("Thanks & Regards");
                    //sb.Append("<h5>Note: This is an Auto generated Mail from Tejaxo ERP. The above details are to the best of information <br> and data available to the ERP System. For any discrepancy/ clarification kindly get in touch with the concerned official. </h5>");
                    //sb.Append("</body></html>");

                    ////send mail
                    //string subj = "";
                    //if (edmode.Value == "Y") subj = "Edited : ";
                    //else subj = "New Entry : ";
                    //fgen.send_Activity_mail(frm_qstr, frm_cocd, "Tejaxo ERP", frm_formID, subj + lblheader.Text + " #" + frm_vnum, sb.ToString(), frm_uname);

                    ////fgen.send_Activity_msg(frm_qstr, frm_cocd, frm_formID, subj + lblheader.Text + " #" + frm_vnum + " by " + frm_uname, frm_uname);

                    //sb.Clear();
                    #endregion


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


    public string Generatehash512(string text)
    {

        byte[] message = Encoding.UTF8.GetBytes(text);

        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] hashValue;
        SHA512Managed hashString = new SHA512Managed();
        string hex = "";
        hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
        {
            hex += String.Format("{0:x2}", x);
        }
        return hex;

    }
    void submitPayment()
    {
        try
        {
            string id = "";
            string amt = txtlbl7.Value;
            string pinfo = "DD";
            string fname = txtcname.Value;
            string emailID = txtemail.Value;
            string udf1 = "-";
            string udf2 = "-";
            string udf3 = "-";
            string udf4 = "-";
            string udf5 = "-";
            string SALT = "-";
            string phn = txtcnumber.Value;

            string[] hashVarsSeq;
            string hash_string = string.Empty;

            if (string.IsNullOrEmpty(Request.Form["txnid"])) // generating txnid
            {
                Random rnd = new Random();
                string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
                txnid1 = strHash.ToString().Substring(0, 20);
            }
            else
            {
                txnid1 = Request.Form["txnid"];
            }
            if (string.IsNullOrEmpty(Request.Form["hash"])) // generating hash value
            {
                if (
                    string.IsNullOrEmpty(ConfigurationManager.AppSettings["MERCHANT_KEY"]) ||
                    string.IsNullOrEmpty(txnid1) ||
                    string.IsNullOrEmpty(amt) ||
                    string.IsNullOrEmpty(fname) ||
                    string.IsNullOrEmpty(emailID) ||
                    string.IsNullOrEmpty(phn)
                    )
                {
                    //error                    
                    return;
                }

                else
                {
                    hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|'); // spliting hash sequence from config
                    hash_string = "";
                    foreach (string hash_var in hashVarsSeq)
                    {
                        if (hash_var == "key")
                        {
                            hash_string = hash_string + ConfigurationManager.AppSettings["MERCHANT_KEY"];
                            hash_string = hash_string + '|';
                        }
                        else if (hash_var == "txnid")
                        {
                            hash_string = hash_string + txnid1;
                            hash_string = hash_string + '|';
                        }
                        else if (hash_var == "amount")
                        {
                            hash_string = hash_string + Convert.ToDecimal(amt).ToString("g29");
                            hash_string = hash_string + '|';
                        }
                        else if (hash_var == "productinfo")
                        {
                            hash_string = hash_string + pinfo;
                            hash_string = hash_string + '|';
                        }
                        else if (hash_var == "firstname")
                        {
                            hash_string = hash_string + fname;
                            hash_string = hash_string + '|';
                        }
                        else if (hash_var == "email")
                        {
                            hash_string = hash_string + emailID;
                            hash_string = hash_string + '|';
                        }
                        else if (hash_var == "udf1")
                        {
                            hash_string = hash_string + udf1;
                            hash_string = hash_string + '|';
                        }
                        else if (hash_var == "udf2")
                        {
                            hash_string = hash_string + udf2;
                            hash_string = hash_string + '|';
                        }
                        else if (hash_var == "udf3")
                        {
                            hash_string = hash_string + udf3;
                            hash_string = hash_string + '|';
                        }
                        else if (hash_var == "udf4")
                        {
                            hash_string = hash_string + udf4;
                            hash_string = hash_string + '|';
                        }
                        else if (hash_var == "udf5")
                        {
                            hash_string = hash_string + udf5;
                            hash_string = hash_string + '|';
                        }
                        else
                        {
                            hash_string = hash_string + (Request.Form[hash_var] != null ? Request.Form[hash_var] : "");// isset if else
                            hash_string = hash_string + '|';
                        }
                    }

                    hash_string += ConfigurationManager.AppSettings["SALT"];// appending SALT

                    hash1 = Generatehash512(hash_string).ToLower();         //generating hash
                    action1 = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";// setting URL
                }
            }
            else if (!string.IsNullOrEmpty(Request.Form["hash"]))
            {
                hash1 = Request.Form["hash"];
                action1 = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";
            }

            sUrl = ConfigurationManager.AppSettings["SUCCESS_URL"];
            fUrl = ConfigurationManager.AppSettings["FAIL_URL"];

            if (!string.IsNullOrEmpty(hash1))
            {
                hash.Value = hash1;
                txnid.Value = txnid1;

                System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                data.Add("hash", hash.Value);
                data.Add("txnid", txnid.Value);
                data.Add("key", key.Value);
                string AmountForm = Convert.ToDecimal(amt).ToString("g29");// eliminating trailing zeros
                txtlbl7.Value = AmountForm;
                data.Add("amount", AmountForm);
                fgen.send_cookie("vNmxxRQM", EncryptDecrypt.Encrypt(AmountForm));
                fgen.send_cookie("tNmxxRQM", EncryptDecrypt.Encrypt(txnid.Value));
                data.Add("firstname", fname);
                data.Add("email", emailID);
                data.Add("phone", phn);
                data.Add("productinfo", pinfo);
                data.Add("surl", sUrl);
                data.Add("furl", fUrl);
                data.Add("lastname", "-");
                data.Add("curl", "-");
                data.Add("address1", "-");
                data.Add("address2", "-");
                data.Add("city", "-");
                data.Add("state", "-");
                data.Add("country", "-");
                data.Add("zipcode", "-");
                data.Add("udf1", udf1);
                data.Add("udf2", udf2);
                data.Add("udf3", udf3);
                data.Add("udf4", udf4);
                data.Add("udf5", udf5);
                data.Add("pg", "-");


                string strForm = PreparePOSTForm(action1, data);
                Page.Controls.Add(new LiteralControl(strForm));

            }

            else
            {
                //no hash

            }

        }

        catch (Exception ex)
        {
            Response.Write("<span style='color:red'>" + ex.Message + "</span>");

        }

    }
    private string PreparePOSTForm(string url, System.Collections.Hashtable data)      // post form
    {
        //Set a name for the form
        string formID = "PostForm";
        //Build the form using the specified data to be posted.
        StringBuilder strForm = new StringBuilder();
        strForm.Append("<form id=\"" + formID + "\" name=\"" +
                       formID + "\" action=\"" + url +
                       "\" method=\"POST\">");

        foreach (System.Collections.DictionaryEntry key in data)
        {

            strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                           "\" value=\"" + key.Value + "\">");
        }


        strForm.Append("</form>");
        //Build the JavaScript which will do the Posting operation.
        StringBuilder strScript = new StringBuilder();
        strScript.Append("<script language='javascript'>");
        strScript.Append("var v" + formID + " = document." +
                         formID + ";");
        strScript.Append("v" + formID + ".submit();");
        strScript.Append("</script>");
        //Return the form and the script concatenated.
        //(The order is important, Form then JavaScript)
        return strForm.ToString() + strScript.ToString();
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
    void save_fun()
    {
        //string curr_dt;
        Prg_Id = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
        frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
        oporow = oDS.Tables[0].NewRow();

        oporow["BRANCHCD"] = frm_mbr;
        oporow["TYPE"] = frm_vty;
        oporow["vchnum"] = frm_vnum.Trim();
        oporow["vchdate"] = DateTime.Now.ToString("dd/MM/yyyy");

        fgen.send_cookie("sTTxlwww", EncryptDecrypt.Encrypt(frm_mbr + frm_vty + frm_vnum.Trim() + oporow["vchdate"].ToString().Trim()));

        oporow["srno"] = i + 1;
        if (txtccode.Text == "-") txtccode.Text = "FINS";
        oporow["co_cd"] = txtccode.Text.Trim().ToUpper();
        oporow["col2"] = "1";
        oporow["col3"] = txtlbl7.Value.Trim().ToUpper();
        oporow["col4"] = oporow["vchdate"];
        oporow["col5"] = txtlbl8.Value.Trim().ToUpper();
        oporow["col6"] = txtinv.Value.Trim().ToUpper();
        oporow["col7"] = txtcperson.Value.Trim().ToUpper();
        oporow["col8"] = txtemail.Value.Trim().ToUpper();
        oporow["col9"] = txtcnumber.Value.Trim().ToUpper();
        //oporow["col11"] = cmbPay.SelectedItem.Text;
        oporow["num4"] = fgen.make_double(txtlbl7.Value.Trim());
        oporow["num5"] = fgen.make_double(txtlbl7.Value.Trim());

        oporow["txnid"] = txnid.Value;

        if (edmode.Value == "Y")
        {
            oporow["ent_by"] = ViewState["entby"].ToString();
            oporow["ent_dt"] = ViewState["entdt"].ToString();
        }
        else
        {
            oporow["ent_by"] = txtccode.Text.Trim();
            oporow["ent_dt"] = vardate;
            //  oporow["edt_by"] = "-";
            //  oporow["edt_dt"] = vardate;
        }

        oDS.Tables[0].Rows.Add(oporow);

    }

    void Type_Sel_query()
    {


    }
}