using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing.Printing;
using System.IO;
using System.Net.Mail;


public partial class Frm_Report : System.Web.UI.Page
{
    string xCRFILE, co_cd, mbr, mail_chk, subj = "", xhtml_tag, HCID, firm, firmname, merrr; string[] mul; DataSet ds;
    string path = @"c:\TEJ_ERP\email_info.txt"; string str = "", xvip = "1", xport = "587", sender_id = "", pwd = "", vsmtp = "", Bcc, Cc, to = "", subject, htmbody, sQUERY;
    string frm_mbr, frm_vty, frm_vnum, frm_url, frm_qstr, frm_cocd, frm_uname, frm_tabname, frm_myear, frm_sql, frm_ulvl, frm_formID, frm_FileName, mq0, mq1, mq2, mq3, filenewname = "";
    SmtpClient smtp; MailMessage mail; int ssl = 0, port = 0;
    ReportDocument repDoc = new ReportDocument(); MemoryStream oStream;
    fgenDB fgen = new fgenDB();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.UrlReferrer == null) Response.Redirect("login.aspx");
            else
            {
                frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
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
                        frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                        co_cd = frm_cocd;
                    }
                }
                btnprint1.Focus();
                ds = new DataSet();
                ds = (DataSet)Session["RPTDATA"];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    CrystalReportViewer1.DisplayPage = true;
                    CrystalReportViewer1.DisplayGroupTree = false;
                    CrystalReportViewer1.DisplayToolbar = true;
                    CrystalReportViewer1.ReportSource = GetReportDocument();
                    CrystalReportViewer1.DataBind();
                    div1.Visible = true; div2.Visible = false; tdprint.Visible = true;

                    set_val();
                }
                else
                {
                    div1.Visible = false; div2.Visible = true; tdprint.Visible = false; set_val();
                }
                System.IO.StringWriter strw = new System.IO.StringWriter();
                HtmlTextWriter htmw = new HtmlTextWriter(strw);
            }
        }
        catch (Exception ex)
        {
            //fgen.FILL_ERR(ex.Message);
            fgen.send_cookie("Send_Mail", "N");
            div1.Visible = false; div2.Visible = true; tdprint.Visible = false; set_val();
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    { return; }
    private ReportDocument GetReportDocument()
    {
        ds = new DataSet();
        ds = (DataSet)Session["RPTDATA"];
        xCRFILE = Request.Cookies["RPTFILE"].Value.ToString();
        string repFilePath = Server.MapPath("" + xCRFILE + "");
        repDoc = new ReportDocument();
        repDoc.Load(repFilePath);
        repDoc.Refresh();
        repDoc.SetDataSource(ds);
        return repDoc;
    }
    protected void Page_UnLoad(object sender, EventArgs e)
    {
        try
        {
            repDoc.Close();
            repDoc.Dispose();
        }
        catch { }
    }
    protected override void OnUnload(EventArgs e)
    {
        try
        {
            base.OnUnload(e);
            this.Unload += new EventHandler(Report_Default_Unload);
        }
        catch { }
    }
    private ReportDocument re_fill_rpt(string qry)
    {
        mbr = frm_mbr;
        DataSet nds = new DataSet();
        nds = fgen.getDS(frm_qstr, co_cd, qry);
        nds.Tables[0].TableName = "Prepcur";
        nds = fgen.Get_Type_Data(frm_qstr, co_cd, mbr, nds);
        xCRFILE = Request.Cookies["RPTFILE"].Value.ToString();
        string repFilePath = Server.MapPath("" + xCRFILE + "");
        repDoc = new ReportDocument();
        repDoc.Load(repFilePath);
        repDoc.Refresh();
        repDoc.SetDataSource(nds);
        return repDoc;
    }
    void Report_Default_Unload(object sender, EventArgs e)
    {
        try
        {
            repDoc.Close();
            repDoc.Dispose();
        }
        catch { }
    }
    protected void CrystalReportViewer1_Unload(object sender, EventArgs e)
    {
        repDoc.Close();
        repDoc.Dispose();
    }
    protected void btnexp_Click(object sender, EventArgs e)
    {
        DataSet ds = (DataSet)Session["RPTDATA"];
        if (ds.Tables[0].Rows.Count > 0)
        {
            frm_FileName = co_cd + "_" + DateTime.Now.ToString().Trim();
            if (frm_cocd == "AVON" && frm_formID == "700025")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    frm_FileName = ds.Tables[0].Rows[0]["item_name"].ToString().Trim() + "_" + DateTime.Now.ToString().Trim();
                }
            }
            fgen.exp_to_excel(ds.Tables[0], "ms-excel", "xls", frm_FileName);
        }
    }
    protected void btnexptopdf_Click(object sender, EventArgs e)
    {
        try
        {
            frm_FileName = co_cd + "_" + DateTime.Now.ToString().Trim();
            if (frm_cocd == "AVON" && frm_formID == "700025")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    frm_FileName = ds.Tables[0].Rows[0]["item_name"].ToString().Trim() + "_" + DateTime.Now.ToString().Trim();
                }
            }
            repDoc = GetReportDocument();
            repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, frm_FileName);
        }
        catch { }
    }
    protected void btnexptoexl_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            frm_FileName = co_cd + "_" + DateTime.Now.ToString().Trim();
            if (frm_cocd == "AVON" && frm_formID == "700025")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    frm_FileName = ds.Tables[0].Rows[0]["item_name"].ToString().Trim() + "_" + DateTime.Now.ToString().Trim();
                }
            }
            repDoc = GetReportDocument();
            repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.Excel, Response, true, frm_FileName);
        }
        catch { }
    }
    protected void btnexptoword_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            frm_FileName = co_cd + "_" + DateTime.Now.ToString().Trim();
            if (frm_cocd == "AVON" && frm_formID == "700025")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    frm_FileName = ds.Tables[0].Rows[0]["item_name"].ToString().Trim() + "_" + DateTime.Now.ToString().Trim();
                }
            }
            repDoc = GetReportDocument();
            repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.WordForWindows, Response, true, frm_FileName);
        }
        catch { }
    }
    protected void btnprint1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            conv_pdf();
        }
        catch (Exception ex) { ex.Message.ToString(); }
    }
    public void conv_pdf()
    {
        repDoc = GetReportDocument();
        Stream oStream = repDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

        //iTextSharp.text.pdf.PdfDocument inpD=new iTextSharp.text.pdf.PdfDocument();

        byte[] byteArray = null;
        byteArray = new byte[oStream.Length];
        oStream.Read(byteArray, 0, Convert.ToInt32(oStream.Length - 1));
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/pdf";
        Response.BinaryWrite(byteArray);

        Response.Flush();
        Response.Close();
        repDoc.Clone();
        repDoc.Dispose();

    }
    public void set_val()
    {
        try { mail_chk = Request.Cookies["Send_Mail"].Value.ToString().Trim(); }
        catch { mail_chk = "N"; }
        ds = new DataSet();
        if (Session["RPTDATA"] != null)
        {
            ds = (DataSet)Session["RPTDATA"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (mail_chk == "Y") tremail.Visible = true;
                else tremail.Visible = false;
            }
            else { tremail.Visible = false; }
        }
    }
    /// <summary>
    /// Used to check detail for sending mail
    /// </summary>
    /// <param name="check_file">If Check File value is 2 then it will check Second email_info file
    /// else it will check First file</param>
    public void chk_email_info(string check_file)
    {
        ViewState["CCMID"] = "";
        if (co_cd.Substring(0, 1) == "A" || co_cd.Substring(0, 1) == "B" || co_cd.Substring(0, 1) == "C" || co_cd.Substring(0, 1) == "D" || co_cd.Substring(0, 1) == "E")
        {
            sender_id = "erp1@pocketdriver.in";
            pwd = "erp_2014";
            vsmtp = "smtp.bizmail.yahoo.com";
        }
        if (co_cd.Substring(0, 1) == "F" || co_cd.Substring(0, 1) == "G" || co_cd.Substring(0, 1) == "H" || co_cd.Substring(0, 1) == "I" || co_cd.Substring(0, 1) == "J")
        {
            sender_id = "erp2@pocketdriver.in";
            pwd = "erp_2014";
            vsmtp = "smtp.bizmail.yahoo.com";
        }
        if (co_cd.Substring(0, 1) == "K" || co_cd.Substring(0, 1) == "L" || co_cd.Substring(0, 1) == "M" || co_cd.Substring(0, 1) == "N" || co_cd.Substring(0, 1) == "O")
        {
            sender_id = "erp3@pocketdriver.in";
            pwd = "erp_2014";
            vsmtp = "smtp.bizmail.yahoo.com";
        }
        if (co_cd.Substring(0, 1) == "P" || co_cd.Substring(0, 1) == "Q" || co_cd.Substring(0, 1) == "R" || co_cd.Substring(0, 1) == "S" || co_cd.Substring(0, 1) == "T")
        {
            sender_id = "erp4@pocketdriver.in";
            pwd = "erp_2014";
            vsmtp = "smtp.bizmail.yahoo.com";
        }
        if (co_cd.Substring(0, 1) == "U" || co_cd.Substring(0, 1) == "V" || co_cd.Substring(0, 1) == "W" || co_cd.Substring(0, 1) == "X" || co_cd.Substring(0, 1) == "Y" || co_cd.Substring(0, 1) == "Z")
        {
            sender_id = "erp4@pocketdriver.in";
            pwd = "erp_2014";
            vsmtp = "smtp.bizmail.yahoo.com";
        }
        path = @"c:\TEJ_ERP\email_info.txt";
        if (check_file == "2")
        {
            // Checking for Second file
            path = @"c:\TEJ_ERP\email_info2.txt";
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                str = sr.ReadToEnd().Trim();
                if (str.Contains("\r")) str = str.Replace("\r", ",");
                if (str.Contains("\n")) str = str.Replace("\n", ",");
                str = str.Replace(",,", ",");
                if (str.Split(',')[0].ToString().Trim() == "Email From") { }
                else
                {
                    sender_id = str.Split(',')[0].ToString().Trim();
                    pwd = str.Split(',')[1].ToString().Trim();
                    vsmtp = str.Split(',')[2].ToString().Trim();
                    xvip = str.Split(',')[3].ToString().Trim();
                    xport = str.Split(',')[4].ToString().Trim();
                    ViewState["CCMID"] = str.Split('=')[1].ToString().Trim();
                }
            }
            else
            {
                StreamWriter tw = File.AppendText(path);
                tw.WriteLine("Email From");
                tw.WriteLine("Password");
                tw.WriteLine("SMTP");
                tw.WriteLine("SSL==> 1 if True, 0 if false");
                tw.WriteLine("PORT");
                tw.WriteLine("CC=");
                tw.Close();
            }
            ssl = Convert.ToInt32(xvip);
            port = Convert.ToInt32(xport);
        }
        else
        {
            if (File.Exists(path))
            {
                StreamReader sr = new StreamReader(path);
                str = sr.ReadToEnd().Trim();
                if (str.Contains("\r")) str = str.Replace("\r", ",");
                if (str.Contains("\n")) str = str.Replace("\n", ",");
                str = str.Replace(",,", ",");
                if (str.Split(',')[0].ToString().Trim() == "Email From") { }
                else
                {
                    sender_id = str.Split(',')[0].ToString().Trim();
                    pwd = str.Split(',')[1].ToString().Trim();
                    vsmtp = str.Split(',')[2].ToString().Trim();
                    xvip = str.Split(',')[3].ToString().Trim();
                    xport = str.Split(',')[4].ToString().Trim();
                    ViewState["CCMID"] = str.Split('=')[1].ToString().Trim();
                }
            }
            else
            {
                StreamWriter tw = File.AppendText(path);
                tw.WriteLine("Email From");
                tw.WriteLine("Password");
                tw.WriteLine("SMTP");
                tw.WriteLine("SSL==> 1 if True, 0 if false");
                tw.WriteLine("PORT");
                tw.WriteLine("CC=");
                tw.Close();
            }
            ssl = Convert.ToInt32(xvip);
            port = Convert.ToInt32(xport);
        }

        DataTable dtType = new DataTable();
        fgenDB fgen = new fgenDB();
        string mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
        dtType = fgen.getdata(frm_qstr, co_cd, "SELECT * FROM TYPE WHERE ID='B' AND TYPE1='" + mbr + "'");
        if (dtType.Rows.Count > 0)
        {
            if (dtType.Rows[0]["MAIL_FLD1"].ToString().Trim().Length > 1)
            {
                sender_id = dtType.Rows[0]["MAIL_FLD1"].ToString().Trim();
                pwd = dtType.Rows[0]["MAIL_FLD2"].ToString().Trim();
                vsmtp = dtType.Rows[0]["MAIL_FLD3"].ToString().Trim();
                xvip = dtType.Rows[0]["MAIL_FLD5"].ToString().Trim();

                if (xvip == "Y") xvip = "1";
                else xvip = "0";

                xport = dtType.Rows[0]["MAIL_FLD4"].ToString().Trim();
                ViewState["CCMID"] = dtType.Rows[0]["MAIL_FLD6"].ToString().Trim();

                ssl = Convert.ToInt32(xvip);
                port = Convert.ToInt32(xport);
            }
        }
    }
    protected void btnsendmail_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            to = "";
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("acode", typeof(string)));
            dt.Columns.Add(new DataColumn("email_info", typeof(string)));
            DataRow dr = null;
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["acode"];
            dt.PrimaryKey = keyColumns;
            DataTable mdt = new DataTable();
            // sQUERY = Request.Cookies["seekSql"].Value.ToString();
            sQUERY = fgenMV.Fn_Get_Mvar(frm_qstr, "U_SEEKSQL");
            if (frm_formID == "F15189")
            {
                mdt = fgen.getdata(frm_qstr, frm_cocd, "select distinct acode,p_email,ordno,orddt,cperson,type from ( " + sQUERY + " ) order by acode");
            }
            else
            {
                mdt = fgen.getdata(frm_qstr, co_cd, "select distinct acode,p_email from ( " + sQUERY + " ) order by acode");
            }
            foreach (DataRow dr1 in mdt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Contains(dr1["acode"].ToString())) { }
                    else
                    {
                        dr = dt.NewRow();
                        dr["acode"] = dr1["acode"];
                        dr["email_info"] = dr1["p_email"];
                        if (frm_formID == "F15189")
                        {
                            mq0 = dr1["ordno"].ToString().Trim();
                            mq1 = Convert.ToDateTime(dr1["orddt"].ToString().Trim()).ToString("dd/MM/yyyy");
                            mq2 = dr1["cperson"].ToString().Trim();
                            mq3 = dr1["type"].ToString().Trim();
                        }
                        dt.Rows.Add(dr);
                        html_body(fgen.seek_iname(frm_qstr, co_cd, "Select aname from famst where trim(acode)='" + dr1["acode"].ToString().Trim() + "'", "aname"));
                        CrystalReportViewer1.ReportSource = re_fill_rpt("SELECT * FROM (" + sQUERY + ") WHERE TRIM(ACODE)='" + dr1["ACODE"].ToString().Trim() + "' ");
                        CrystalReportViewer1.DataBind();
                        send_mail(dr1["p_email"].ToString(), subj, xhtml_tag, dr1["acode"].ToString());
                        CrystalReportViewer1.Dispose();
                    }
                }
                else
                {
                    dr = dt.NewRow();
                    dr["acode"] = dr1["acode"];
                    dr["email_info"] = dr1["p_email"];
                    if (frm_formID == "F15189")
                    {
                        mq0 = dr1["ordno"].ToString().Trim();
                        mq1 = Convert.ToDateTime(dr1["orddt"].ToString().Trim()).ToString("dd/MM/yyyy");
                        mq2 = dr1["cperson"].ToString().Trim();
                        mq3 = dr1["type"].ToString().Trim();
                    }
                    dt.Rows.Add(dr);
                    html_body(fgen.seek_iname(frm_qstr, co_cd, "Select aname from famst where trim(acode)='" + dr1["acode"].ToString().Trim() + "'", "aname"));
                    if (frm_formID == "F15189")
                    {
                        CrystalReportViewer1.ReportSource = GetReportDocument();
                    }
                    else
                    {
                        CrystalReportViewer1.ReportSource = re_fill_rpt("SELECT * FROM (" + sQUERY + ") WHERE TRIM(ACODE)='" + dr1["ACODE"].ToString().Trim() + "' ");
                    }
                    CrystalReportViewer1.DataBind();
                    send_mail(dr1["p_email"].ToString(), subj, xhtml_tag, dr1["acode"].ToString());
                    CrystalReportViewer1.Dispose();
                }
            }
            fgen.send_cookie("Send_Mail", "N");
            if (frm_formID == "F15189")
            {
                if (merrr == "Y")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnsendmail');", true);
                    Response.Write("<script>alert('Mail Has Been Send Successfully.');</script>");
                }
                else
                {
                    Response.Write("<script>alert('Mail Not Send Successfully.');</script>");
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnsendmail');", true);
            }
        }
        catch { }
    }
    public void html_body(string party_name)
    {
        // HCID = Request.Cookies["rid"].Value.ToString();
        HCID = frm_formID;
        firm = fgenCO.chk_co(co_cd); xhtml_tag = "";
        firm = firm.Replace("XXXX", co_cd);
        if (frm_cocd != "MULT")
        {
            xhtml_tag = xhtml_tag + "M/s " + firm + "<br>";
            xhtml_tag = xhtml_tag + "==================================<br>";
        }
        if (frm_formID == "F15189" && co_cd != "MULT" && co_cd != "MLAB")
        {
            firmname = fgen.seek_iname(frm_qstr, frm_cocd, "select name from type where id='B' and type1='" + frm_mbr + "'", "name");
            firm = firmname;
        }

        xhtml_tag = xhtml_tag + "To <br>";
        xhtml_tag = xhtml_tag + "<br>M/s " + party_name + "<br>";
        if (frm_cocd != "MULT")
            xhtml_tag = xhtml_tag + "Respected Sir/Mam,";
        switch (HCID)
        {
            case "34100":
                subj = "Tejaxo ERP: Statement of A/c from " + firm + "";
                xhtml_tag = xhtml_tag + "<BR>Please find attached the";
                xhtml_tag = xhtml_tag + "<br>Statement of A/c<br>";
                break;
            case "34101":
                subj = "Tejaxo ERP: Request for Clearane of dues from  " + firm + "";
                xhtml_tag = xhtml_tag + "<BR>Please find attached the";
                xhtml_tag = xhtml_tag + "<br>due bills as on date<br>";
                break;
            case "34102":
                subj = "Tejaxo ERP: A/m Sales Report from " + firm + "";
                xhtml_tag = xhtml_tag + "<BR>Please find attached the";
                xhtml_tag = xhtml_tag + "<br>despatch detials for material despatch<br>";
                break;
            case "34103":
                subj = "Tejaxo ERP: Payment Advice Report from " + firm + "";
                xhtml_tag = xhtml_tag + "<BR>Please find attached the Payment Advice";
                xhtml_tag = xhtml_tag + "<br>The bill wise detail of the bill passed for value and deductions if any are attached. Please feel free to get in touch with the accounts department for clarifications if any.";
                xhtml_tag = xhtml_tag + "<br>It is a pleasure doing business with your company.";
                break;
            case "34104":
                subj = "Tejaxo ERP: Balance Confirmation Report from " + firm + "";
                xhtml_tag = xhtml_tag + "<BR>Please find attached the";
                xhtml_tag = xhtml_tag + "<br>Balance Confirmation<br>";
                break;
            case "34105":
                subj = "Tejaxo ERP: Form Reminder Report from " + firm + "";
                xhtml_tag = xhtml_tag + "<BR>Please find attached the";
                xhtml_tag = xhtml_tag + "<br>Form Reminder<br>";
                break;
            case "34106":
                subj = "Tejaxo ERP: RTGS/NEFT Report from " + firm + "";
                xhtml_tag = xhtml_tag + "<BR>Please find attached the";
                xhtml_tag = xhtml_tag + "<br>RTGS/NEFT<br>";
                break;
            case "34107":
                subj = "Tejaxo ERP: Payment Reminder(Due Bills) Report from " + firm + "";
                xhtml_tag = xhtml_tag + "<BR>Please find attached the";
                xhtml_tag = xhtml_tag + "<br>Payment Reminder(Due Bills)<br>";
                break;
            case "34108":
                subj = "Tejaxo ERP: Debit Note Report from " + firm + "";
                xhtml_tag = xhtml_tag + "<BR>Please find attached the";
                xhtml_tag = xhtml_tag + "<br>Debit Note<br>";
                break;
            case "F15189":
                subj = "Purchase Order " + mq3 + "/" + mq0 + " from " + firm.toProper() + "";
                if (frm_cocd == "MULT")
                {
                    xhtml_tag = xhtml_tag + "Greetings from Multitec.<br>";
                    xhtml_tag = xhtml_tag + "<br>We are please to release the purchase order number : <b>" + mq0 + "</b> dated " + mq1 + ".<br>";
                    xhtml_tag = xhtml_tag + "<br>Please find the attached copy for the same<br>";
                }
                else
                {
                    xhtml_tag = xhtml_tag + "<B> Kindly Attention " + mq2 + "</B><br>";
                    xhtml_tag = xhtml_tag + "<br>We are please to release the purchase order " + mq3 + "/" + mq0 + " Dated " + mq1 + ". For your kind perusal. <br>";
                    xhtml_tag = xhtml_tag + "<br>Please find the attached Purchase Order <br>";
                }
                filenewname = "PO_" + mq3 + "-" + mq0 + "_" + firm.Replace(" ", "_");
                break;
        }
        if (frm_cocd == "MULT") xhtml_tag = xhtml_tag + "<br><b>Warm Regards,</b>";
        else xhtml_tag = xhtml_tag + "<br><b>Thanks & Regards,</b>";
        xhtml_tag = xhtml_tag + "<br><b>" + firm + "</b>";
        xhtml_tag = xhtml_tag + "<br><br><br>Note: This is an automatically generated email from Tejaxo ERP through a notification only address that cannot accept incoming email. Please do not reply to this email.";
        xhtml_tag = xhtml_tag + "</body></html>";
    }
    public void send_mail(string mail_to, string mail_subj, string mail_body, string acode)
    {
        try
        {
            if (co_cd == "INFI")
            {
                if (acode.Substring(0, 2) == "16") chk_email_info("2");
                else chk_email_info("1");
            }
            else chk_email_info("1");



            mail = new MailMessage();
            mail.From = new MailAddress(sender_id);
            mail.Subject = mail_subj;
            mail.Body = mail_body;
            mail.IsBodyHtml = true;
            to = mail_to;
            // to = "madhvi@pocketdriver.in";
            if (to.Contains(",") || to.Contains(";"))
            {
                to = to.Replace(";", ",");
                mul = to.Split(',');
                foreach (string mul_id in mul)
                {
                    mail.To.Add(new MailAddress(mul_id));
                }
            }
            else
            {
                to = to.Replace(";", ""); to = to.Replace(",", "");
                mail.To.Add(new MailAddress(to));
            }
            Cc = txtemailcc.Text.Trim().Replace("&nbsp", "");
            Cc = Cc + "," + ViewState["CCMID"].ToString().Trim().Replace("CC=", "").Replace("=", "");
            if (Cc.Trim().Length > 0)
            {
                if (Cc.Contains(",") || Cc.Contains(";"))
                {
                    Cc = Cc.Replace(";", ",");
                    mul = Cc.Split(',');
                    foreach (string mul_id in mul)
                    {
                        if (mul_id.Length > 0) mail.CC.Add(new MailAddress(mul_id));
                    }
                }
                else
                {
                    Cc = Cc.Replace(";", ""); Cc = Cc.Replace(",", "");
                    mail.CC.Add(new MailAddress(Cc));
                }
            }
            Bcc = txtemailbcc.Text.Trim().Replace("&nbsp", "");
            if (Bcc.Trim().Length > 0)
            {
                if (Bcc.Contains(",") || Bcc.Contains(";"))
                {
                    Bcc = Bcc.Replace(";", ",");
                    mul = Bcc.Split(',');
                    foreach (string mul_id in mul)
                    {
                        if (mul_id.Length > 0) mail.Bcc.Add(new MailAddress(mul_id));
                    }
                }
                else
                {
                    Bcc = Bcc.Replace(";", ""); Bcc = Bcc.Replace(",", "");
                    mail.Bcc.Add(new MailAddress(Bcc));
                }
            }
            if (frm_formID == "F15189")
            {
                if (filenewname == "") filenewname = frm_cocd + "_Purchase Order_" + mq0 + "_" + subj.Replace(" ", "_");
                Attachment atchfile = new Attachment(repDoc.ExportToStream(ExportFormatType.PortableDocFormat), filenewname + ".pdf");
                mail.Attachments.Add(atchfile);
                if (fupl.HasFile)
                {
                    mail.Attachments.Add(new Attachment(fupl.PostedFile.InputStream, fupl.FileName));  
                }
            }
            else
            {
                Attachment atchfile = new Attachment(repDoc.ExportToStream(ExportFormatType.PortableDocFormat), co_cd + "_" + subj.Replace(" ", "_") + ".pdf");
                mail.Attachments.Add(atchfile);
            }
            smtp = new SmtpClient();
            {
                smtp.Host = vsmtp;
                smtp.Port = port;
                if (ssl == 1) smtp.EnableSsl = true;
                else smtp.EnableSsl = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(sender_id, pwd);
            }
            smtp.Send(mail);

            StreamWriter tw = File.AppendText(@"c:\TEJ_ERP\email_sent.txt");
            tw.WriteLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString());
            tw.WriteLine("Mail has been sent to " + acode.Trim().Replace("&nbsp;", "") + " (" + to.Trim() + ") ");
            tw.WriteLine("==================================================================");
            tw.Close();
            merrr = "Y";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "a", "alert('Mail Not Sent!!Please check your internet connection or Email_info.txt file in c:/tej_erp folder')", true);
            StreamWriter tw = File.AppendText(@"c:\TEJ_ERP\email_not_sent.txt");
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
            tw.WriteLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToShortTimeString());
            tw.WriteLine("Mail has not been sent to " + acode.Trim().Replace("&nbsp;", "") + " (" + to.Trim() + ") " + ex.Message);
            tw.WriteLine("Line: " + trace.GetFrame(0).GetFileLineNumber());
            tw.WriteLine("==================================================================");
            tw.Close();
            merrr = "N";
        }
        mail.Dispose();
        smtp = null;
    }
    protected void btnupload_Click(object sender, EventArgs e)
    {

    }
}