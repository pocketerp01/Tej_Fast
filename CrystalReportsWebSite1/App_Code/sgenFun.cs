using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Win32;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Globalization;
using System.Net;
//using Sql.DataAccess.Client;
using System.Diagnostics;

using System.Xml;

using System.Dynamic;


//using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Client;
using MessagingToolkit.QRCode.Codec;

/// <summary>
/// Summary description for sgenFun
/// </summary>
/// 
public class sgenFun
{
    public string mq = "", mq0 = "", MyGuid = "";
    public string dateformat = "", timeformat = "", sqldateformat = "", sqltimeformat = "", timezone = "", sep = "", datetimeformat = "", sqldatetimeformat = "";
    public static string callbackurl = "~/main/login";
    public static string callbackmvcAct = "login";
    public static string callbackmvcCtr = "main";
    public const int duration = 3600;
    public string mhd = "", Cls_comp_code = "", val = "", icon_allow = "", firm = "", MV_CLIENT_GRP = "", pco_cd = "", mulevel, muname, opt_freez="";
    Multiton multiton;
    string urights, pk_error;
    DataTable dt_menu = new DataTable();
    public SmtpClient smtp;
    public MailMessage mail;
    public XmlDocument docxml = new XmlDocument();
    public XmlNodeList getval;
    string sender_id = "", pwd, vsmtp, xvip, xport, resultVal = "", textName = "", resultMsg = "", CCMID = "", chkActiVated;
    public string valFound = "N";
    int ssl, port;

    public sgenFun(string Myguid)
    {
        MyGuid = Myguid;
    }
    public int Make_int(string val)
    {
        int res = 0;
        try { res = Convert.ToInt32(Convert.ToDecimal(val)); }
        catch (Exception err) { }
        return res;
    }
    public Int64 Make_long(string val)
    {
        Int64 res = 0;
        try { res = Convert.ToInt64(Convert.ToDecimal(val)); }
        catch (Exception err) { }
        return res;
    }
    public double Make_double(string val)
    {
        double res = 0;
        try { res = Convert.ToDouble(val); }
        catch (Exception err) { }
        return res;
    }
    public decimal Make_decimal(string val)
    {
        decimal res = 0;
        try { res = Convert.ToDecimal(val); }
        catch (Exception err) { }
        return res;
    }
    //public void Fn_open_prddmp1(string br, string val)
    //{
    //    showDateFilter("", br);
    //}
    public decimal Make_decimal(object val)
    {
        decimal res = 0;
        try { res = Convert.ToDecimal(val); }
        catch (Exception err) { }
        return res;
    }



    public bool DateBetween(DateTime date, DateTime datePast, DateTime dateFuture)
    {

        if (datePast <= date && date <= dateFuture) return true;
        else return false;

    }
    /// <summary>
    /// opening,Rcpt,Issued,Closing_Stk,IMIN,IMAX,IORD,ALLFLD for all stock fields combine with ~
    /// </summary>
    /// <param name="co_cd"></param>
    /// <param name="mbr"></param>
    /// <param name="icode"></param>
    /// <param name="consolidate"></param>
    /// <param name="value">opening,Rcpt,Issued,Closing_Stk,IMIN,IMAX,IORD,ALLFLD for all stock fields combine with ~ </param>
    /// <returns></returns>
    public string seek_istock(string frmQstr, string co_cd, string mbr, string icode, string stockDate, bool consolidate, string valuetoShow, string condition)
    {
        string CDT2 = Multiton.Get_Mvar(frmQstr, "U_CDT2");
        string CDT1 = Multiton.Get_Mvar(frmQstr, "U_CDT1");
        string fromdt = Multiton.Get_Mvar(frmQstr, "U_MDT1");
        if (fromdt == "0") fromdt = DateTime.Now.ToString("dd/MM/yyyy");
        string year = Multiton.Get_Mvar(frmQstr, "U_YEAR");
        if (stockDate == "") stockDate = CDT2;
        //string xprdrange1 = "between to_date('" + CDT1 + "','dd/MM/yyyy') and to_date('" + CDT1 + "','dd/MM/yyyy')-1";
        string xprdrange = "between to_date('" + CDT1 + "','dd/MM/yyyy') and to_date('" + stockDate + "','dd/MM/yyyy')";
        string cond = " AND trim(icode)='" + icode + "'";
        string branch_Cd = "BRANCHCD='" + mbr + "'";
        if (consolidate) branch_Cd = "BRANCHCD not in ('DD','88')";

        string SQuery = "Select " + valuetoShow + " as retvalue from (select sum(a.opening)||'~'||sum(a.cdr)||'~'||sum(a.ccr)||'~'||(Sum(a.opening)+sum(a.cdr)-sum(a.ccr))||'~'||sum(a.imin)||'~'||sum(a.imax)||'~'||sum(a.iord) AS ALLFLD,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum(a.opening)+sum(a.cdr)-sum(a.ccr) as Closing_Stk,sum(a.imin) as imin,sum(a.imax) as imax,sum(a.iord) as iord from (Select branchcd,trim(icode) as icode,yr_" + year + " as opening,0 as cdr,0 as ccr,isnull(imin,0) as imin,isnull(imax,0) as imax,isnull(iord,0) as iord from itembal where " + branch_Cd + " " + cond + " union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr, 0 as aaa , 0 as aaa1,0 as aaa2 from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdrange + "  and store='Y' " + cond + " " + condition + " GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE) ";
        mq0 = seek_iname(frmQstr, co_cd, SQuery, "retvalue");
        return mq0;
    }
    public string makeRepQuery(string frm_qstr, string co_cd, string formName, string branchCD, string vty, string prdRange)
    {
        string retQuery = "";
        string tbl_flds = seek_iname(frm_qstr, co_cd, "select trim(date_fld)||'@'||trim(sort_fld)||'@'||trim(join_cond)||'@'||trim(table1)||'@'||trim(table2)||'@'||trim(table3)||'@'||trim(table4) as fstr from rep_config where trim(frm_name)='" + formName + "' and srno=0", "fstr");
        string datefld = "";
        string sortfld = "";
        string joinfld = "", table1 = "", table2 = "", table3 = "", table4 = "", rep_flds = "";
        if (tbl_flds.Trim().Length > 1)
        {
            datefld = tbl_flds.Split('@')[0].ToString();
            sortfld = tbl_flds.Split('@')[1].ToString();
            joinfld = tbl_flds.Split('@')[2].ToString();

            table1 = tbl_flds.Split('@')[3].ToString();
            table2 = tbl_flds.Split('@')[4].ToString();
            table3 = tbl_flds.Split('@')[5].ToString();
            table4 = tbl_flds.Split('@')[6].ToString();

            sortfld = sortfld.Replace("`", "'");
            joinfld = joinfld.Replace("`", "'");
            rep_flds = seek_iname(frm_qstr, co_cd, "select rtrim(dbms_xmlgen.convert(xmlagg(xmlelement(e," + "repfld" + "||',')).extract('//text()').getClobVal(), 1),'#,#') as fstr from(select trim(obj_name)||' as '||trim(obj_caption) as repfld from rep_config where frm_name='" + formName + "' and length(Trim(obj_name))>1 and length(Trim(obj_caption))>1  order by col_no)", "fstr");
            rep_flds = rep_flds.Replace("`", "'");
        }

        if (vty.Length > 1) vty = "and " + vty;
        retQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " where a." + branchCD + " " + vty + " and " + datefld + " " + prdRange + " and " + joinfld + "  order by " + sortfld;

        return retQuery;
    }
    public string makeRepQuery(string frm_qstr, string co_cd, string formName, string branchCD, string vty, string prdRange, string extraCond)
    {
        string retQuery = "";
        string tbl_flds = seek_iname(frm_qstr, co_cd, "select trim(date_fld)||'@'||trim(sort_fld)||'@'||trim(join_cond)||'@'||trim(table1)||'@'||trim(table2)||'@'||trim(table3)||'@'||trim(table4) as fstr from rep_config where trim(frm_name)='" + formName + "' and srno=0", "fstr");
        string datefld = "";
        string sortfld = "";
        string joinfld = "", table1 = "", table2 = "", table3 = "", table4 = "", rep_flds = "";
        if (tbl_flds.Trim().Length > 1)
        {
            datefld = tbl_flds.Split('@')[0].ToString();
            sortfld = tbl_flds.Split('@')[1].ToString();
            joinfld = tbl_flds.Split('@')[2].ToString();

            table1 = tbl_flds.Split('@')[3].ToString();
            table2 = tbl_flds.Split('@')[4].ToString();
            table3 = tbl_flds.Split('@')[5].ToString();
            table4 = tbl_flds.Split('@')[6].ToString();

            sortfld = sortfld.Replace("`", "'");
            joinfld = joinfld.Replace("`", "'");
            rep_flds = seek_iname(frm_qstr, co_cd, "select rtrim(dbms_xmlgen.convert(xmlagg(xmlelement(e," + "repfld" + "||',')).extract('//text()').getClobVal(), 1),'#,#') as fstr from(select trim(obj_name)||' as '||trim(obj_caption) as repfld from rep_config where frm_name='" + formName + "' and length(Trim(obj_name))>1 and length(Trim(obj_caption))>1  order by col_no)", "fstr");
            rep_flds = rep_flds.Replace("`", "'");
        }
        if (vty.Length > 1) vty = "and " + vty;
        retQuery = "Select " + rep_flds + " from " + table1 + " , " + table2 + ", " + table3 + " where a." + branchCD + " " + vty + " and " + datefld + " " + prdRange + " and " + joinfld + " and " + extraCond + " order by " + sortfld;

        return retQuery;
    }
    public void send_Activity_mail(string _qstr, string comp, string SName, string formId, string subject, string msg, string entby)
    {
        DataTable dtMailMgr = new DataTable();
        string mUsrcode = seek_iname(_qstr, comp, "select userid as cSource from evas where username='" + entby + "'", "cSource");

        dtMailMgr = getdata(comp, "select distinct trim(ECODE) As ecode,trim(emailid) As emailid,trim(username) as username from (SELECT a.ECODE,b.emailid,b.username FROM WB_MAIL_MGR a,EVAS B WHERE TRIM(A.ECODE)=TRIM(B.USERID) AND A.TYPE='MM' AND TRIM(a.RCODE)='" + formId + "' AND TRIM(NVL(B.EMAILID,'-'))!='-' union all SELECT b.userid as ECODE,b.emailid,b.username FROM EVAS B WHERE TRIM(b.userid)='" + mUsrcode + "' AND TRIM(NVL(B.EMAILID,'-'))!='-') ");
        foreach (DataRow dr in dtMailMgr.Rows)
        {
            send_mail(comp, SName, dr["emailid"].ToString().Trim(), "", "", subject, msg);
        }
    }
    public string save_Mailbox2(string Uniq_QSTR, string compCode, string curr_form, string cur_br, string msg_2_save, string from_Usr, string m_ed_mode)
    {
        string subj = "New : ";
        if (m_ed_mode.Trim() == "Y")
        {
            subj = "Edit : ";
        }
        string mUsrcode = seek_iname(Uniq_QSTR, compCode, "select userid as cSource from evas where username='" + from_Usr + "'", "cSource");
        string mq0;
        mq0 = GetIpAddress().ToString().ToUpper() + " ," + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
        if (mq0.Length > 29) mq0 = mq0.Substring(0, 29);

        string terminal = seek_iname(Uniq_QSTR, compCode, "select userenv('terminal')||' ,'||sysdate||' '||to_char(sysdate,'HH:MI:SS PM') as cSource from dual", "cSource");
        DataTable dtMailMgr = new DataTable();
        dtMailMgr = getdata(compCode, "select distinct Ecode from (SELECT trim(a.ECODE) as Ecode FROM WB_MAIL_MGR a WHERE A.TYPE='MM' AND TRIM(a.RCODE)='" + curr_form + "' union all Select trim('" + mUsrcode + "') as Ecode from dual) order by Ecode");
        foreach (DataRow dr in dtMailMgr.Rows)
        {
            try
            {
                string vnum = next_no(Uniq_QSTR, compCode, "select max(vchnum) as vchnum from mailbox2 where type='10'", 6, "vchnum");
                using (DataSet fgen_oDS = fill_schema(Uniq_QSTR, compCode, "mailbox2"))
                {
                    DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
                    fgen_oporow["BRANCHCD"] = cur_br;
                    fgen_oporow["TYPE"] = "10";
                    fgen_oporow["VCHNUM"] = vnum;
                    fgen_oporow["VCHDATE"] = DateTime.Now.ToString("dd/MM/yyyy");

                    fgen_oporow["msgto"] = dr["ECODE"].ToString().Trim();
                    fgen_oporow["msgfrom"] = from_Usr;
                    fgen_oporow["terminal"] = terminal;

                    fgen_oporow["msgtxt"] = subj + " " + msg_2_save + " Msg From Computer : " + terminal;
                    fgen_oporow["msgdt"] = mUsrcode;
                    fgen_oporow["msgseen"] = "N";

                    fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
                    save_data(Uniq_QSTR, compCode, fgen_oDS, "mailbox2");
                }
            }
            catch (Exception ex) { FILL_ERR("In Mailbox2 Saving :=> " + ex.Message.ToString().Trim()); }
        }
        return "";
    }

    public string save_data(string uniqStr, string Comp_Code, DataSet _oDs, string tab_name)
    {
        if (Comp_Code == "0") Comp_Code = uniqStr.Split('^')[0];
        string constr = fgenMV.Fn_Get_Mvar(uniqStr, "CONN");
        string saveSuccessed = "N";
        if (constr == "0") { constr = ConnInfo.connString(Comp_Code); }
        //cow
        try
        {
            using (OracleConnection fcon = new OracleConnection(constr))
            {
                fcon.Open();
                using (OracleDataAdapter fgen_da = new OracleDataAdapter("select * from " + tab_name + " where 1=2", fcon))
                {
                    using (OracleCommandBuilder cb = new OracleCommandBuilder(fgen_da))
                    {
                        string field_type = "";
                        for (int i = 0; i < _oDs.Tables[0].Rows.Count; i++)
                        {
                            for (int z = 0; z < _oDs.Tables[0].Columns.Count; z++)
                            {
                                field_type = _oDs.Tables[0].Columns[z].DataType.Name.ToString();
                                if (field_type.ToUpper() == "DATETIME" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                else if (field_type.ToUpper() == "DECIMAL" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                else if (field_type.ToUpper() == "INT64" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                else if (field_type.ToUpper() == "INT32" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                else if (field_type.ToUpper() == "INT16" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                else if (field_type.ToUpper() == "DOUBLE" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                else if (field_type.ToUpper() == "SINGLE" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                else if (field_type.ToUpper() == "BOOLEAN" && _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                                else _oDs.Tables[0].Rows[i][z] = _oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Replace("&nbsp;", "-").Replace("&amp;", "-").Replace(@"\", "/").Trim();


                            }
                        }
                        _oDs.Tables[0].TableName = tab_name;
                        fgen_da.Update(_oDs, tab_name);
                        _oDs.Dispose();
                        saveSuccessed = "Y";
                        cb.Dispose();
                    }
                    fgen_da.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            FILL_ERR("In Save-Data Fn " + ex.Message);
            saveSuccessed = "N";
            throw;
        }
        //cow
        return saveSuccessed;
    }

    public string send_mail(string co_cd, string name, string to, string Cc, string Bcc, string subj, string body)
    {
        string merror = ""; string[] mul;
        try
        {
            mail = new MailMessage();
            chk_email_info(co_cd, "1");
            mail.From = new MailAddress(name + "<" + sender_id + ">");
            //mail.From = new MailAddress(sender_id);

            mail.Subject = subj;
            mail.Body = body;
            mail.IsBodyHtml = true;
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
            if (Cc.Trim().Length > 2)
            {
                if (Cc.Contains(",") || Cc.Contains(";"))
                {
                    Cc = Cc.Replace(";", ",");
                    mul = Cc.Split(',');
                    foreach (string mul_id in mul)
                    {
                        mail.CC.Add(new MailAddress(mul_id));
                    }
                }
                else
                {
                    Cc = Cc.Replace(";", ""); Cc = Cc.Replace(",", "");
                    mail.CC.Add(new MailAddress(Cc));
                }
            }
            if (Bcc.Trim().Length > 2)
            {
                if (Bcc.Contains(",") || Bcc.Contains(";"))
                {
                    Bcc = Bcc.Replace(";", ",");
                    mul = Bcc.Split(',');
                    foreach (string mul_id in mul)
                    {
                        mail.Bcc.Add(new MailAddress(mul_id));
                    }
                }
                else
                {
                    Bcc = Bcc.Replace(";", ""); Bcc = Bcc.Replace(",", "");
                    mail.Bcc.Add(new MailAddress(Bcc));
                }
            }
            merror = "1";

            SendEmailInBackgroundThread(mail);

            FILL_Log("Mail has been sent to " + to.Trim().Replace("&nbsp;", "") + " (" + to.Trim() + ") ");
        }
        catch (Exception ex)
        {
            merror = "0";
            FILL_ERR(co_cd + " " + ex.Message);
            FILL_ERR("Rcv ID: " + to);
            FILL_ERR("Sender ID: " + sender_id);
        }
        return merror;
    }
    public void FILL_Log(string msg)
    {
        string ppath = @"c:\TEJ_ERP\logFile.txt";
        try
        {
            if (File.Exists(ppath))
            {
                StreamWriter w = File.AppendText(ppath);
                w.WriteLine(msg.ToString() + "-->" + DateTime.Now.ToString("ddMMyyyy hh:mm:ss tt"));
                w.WriteLine("=====================================================================");
                w.Flush();
                w.Close();
            }
            else
            {
                StreamWriter w = new StreamWriter(ppath, true);
                w.WriteLine(msg.ToString() + "-->" + DateTime.Now.ToString("ddMMyyyy hh:mm:ss tt"));
                w.WriteLine("=====================================================================");
                w.Flush();
                w.Close();
            }
        }
        catch { }
    }
    void SendEmailInBackgroundThread(MailMessage mailMessage)
    {
        //Thread bgThread = new Thread(new ParameterizedThreadStart(SendEmail));
        //bgThread.IsBackground = true;
        //bgThread.Start(mailMessage);
    }
    public void chk_email_info(string co_cd, string check_file)
    {
        string str, path;
        if (sender_id == "")
        {
            //if (co_cd.Substring(0, 1) == "A" || co_cd.Substring(0, 1) == "B" || co_cd.Substring(0, 1) == "C" || co_cd.Substring(0, 1) == "D" || co_cd.Substring(0, 1) == "E")
            //{
            //    sender_id = "erp1@pocketdriver.in";
            //    pwd = "erp_2014";
            //    vsmtp = "smtp.bizmail.yahoo.com";
            //}
            //if (co_cd.Substring(0, 1) == "F" || co_cd.Substring(0, 1) == "G" || co_cd.Substring(0, 1) == "H" || co_cd.Substring(0, 1) == "I" || co_cd.Substring(0, 1) == "J")
            //{
            //    sender_id = "erp2@pocketdriver.in";
            //    pwd = "erp_2014";
            //    vsmtp = "smtp.bizmail.yahoo.com";
            //}
            //if (co_cd.Substring(0, 1) == "K" || co_cd.Substring(0, 1) == "L" || co_cd.Substring(0, 1) == "M" || co_cd.Substring(0, 1) == "N" || co_cd.Substring(0, 1) == "O")
            //{
            //    sender_id = "erp3@pocketdriver.in";
            //    pwd = "erp_2014";
            //    vsmtp = "smtp.bizmail.yahoo.com";
            //}
            //if (co_cd.Substring(0, 1) == "P" || co_cd.Substring(0, 1) == "Q" || co_cd.Substring(0, 1) == "R" || co_cd.Substring(0, 1) == "S" || co_cd.Substring(0, 1) == "T")
            //{
            //    sender_id = "erp4@pocketdriver.in";
            //    pwd = "erp_2014";
            //    vsmtp = "smtp.bizmail.yahoo.com";
            //}
            //if (co_cd.Substring(0, 1) == "U" || co_cd.Substring(0, 1) == "V" || co_cd.Substring(0, 1) == "W" || co_cd.Substring(0, 1) == "X" || co_cd.Substring(0, 1) == "Y" || co_cd.Substring(0, 1) == "Z")
            //{
            //    sender_id = "erp4@pocketdriver.in";
            //    pwd = "erp_2014";
            //    vsmtp = "smtp.bizmail.yahoo.com";
            //}
            xvip = "1";
            xport = "465";
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
                        try
                        {
                            CCMID = str.Split('=')[1].ToString().Trim();
                        }
                        catch { CCMID = ""; }
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
                    //tw.WriteLine("CC=");
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
                        try
                        {
                            CCMID = str.Split('=')[1].ToString().Trim();
                        }
                        catch { CCMID = ""; }
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
                    //tw.WriteLine("CC=");
                    tw.Close();
                }
                ssl = Convert.ToInt32(xvip);
                port = Convert.ToInt32(xport);
            }
        }
    }
    public string make_def_Date(string mtext, string mtext2)
    {
        string result = "-";
        try
        {
            if (mtext == "") mtext = mtext2;
            result = mtext.Trim();

            if (mtext == "-") mtext = mtext2;
            result = mtext.Trim();
        }
        catch { result = "-"; }
        return result;
    }
    public string padlc(Int64 Number, int totalcharactes)
    {
        String result = "";
        Int64 temp = Number;
        for (int i = 1; i < totalcharactes; i++)
        {
            temp /= 10;
            if (temp == 0) result += "0";
        }
        result += Number.ToString();
        return result;
    }
    public string Right(string value, int length)
    {
        return value.Substring(value.Length - length);
    }
    public string server_datetime(string usercode)
    {
        return server_datetime_dt(usercode).ToString("yyyy-MM-dd HH:mm:ss");
    }
    public DateTime server_datetime_dt(string usercode)
    {
        DateTime date = new DateTime();
        try
        {
            //date = DateTime.Parse(seekval(usercode, "SELECT format(systimestamp AT TIME ZONE 'Asia/Kolkata','YYYY-MM-DD HH24:MI:SS') val FROM DUAL", "val"));
            date = DateTime.Parse(seekval(usercode, "SELECT format(GETDATE(),'yyyy-MM-dd HH:mm:ss') val FROM DUAL", "val"));
        }
        catch (Exception err)
        { }
        return date;
    }
    public DateTime datetime_Srv(string usercode)
    {
        DateTime date = new DateTime();
        try
        {
            date = DateTime.Parse(seekval(usercode, "SELECT format(systimestamp AT TIME ZONE 'Asia/Kolkata','YYYY-MM-DD HH24:MI:SS') val FROM DUAL", "val"));
        }
        catch (Exception err)
        { }
        return date;
    }
    public bool Make_date(string txtdate, out DateTime dateTime)
    {
        if (DateTime.TryParse(txtdate, out dateTime)) return true;
        else return false;
    }
    public string Make_date_S(string txtdate)
    {
        string resDate = "";
        try
        {
            resDate = DateTime.ParseExact(txtdate, Getdateformat(), CultureInfo.InvariantCulture).ToString(GetSaveDateFormat());
        }
        catch (Exception err)
        {
            resDate = "1900-01-01 00:00:00";
        }
        return resDate;
    }
    public bool IsDate(string datestr)
    {
        DateTime temp;
        if (DateTime.TryParse(datestr, out temp)) return true;
        else return false;
    }
    public string Fn_chk_doc_freeze(string Qstr, string co_cd, string ctrl_br, string ctrl_id, string doc_Dt)
    {
        opt_freez = seek_iname(Qstr, co_cd, "SELECT trim(opt_param)||'@'||trim(opt_param2) as fstr FROM FIN_RSYS_opt_PW WHERE branchcd='" + ctrl_br + "' and opt_id='" + ctrl_id + "'", "fstr");
        string roll_Days;
        string fixd_Date;
        urights = "0";
        if (opt_freez != "0")
        {
            roll_Days = opt_freez.Split('@')[0].ToString();
            fixd_Date = opt_freez.Split('@')[1].ToString();
            string mqry;
            mqry = "SELECT (case when to_datE('" + doc_Dt + "','dd/mm/yyyy')<to_datE(sysdate,'dd/mm/yyyy')-" + Make_double(roll_Days) + " then 'Y' else 'N' end)  as fstr FROM FIN_RSYS_opt_PW WHERE branchcd='" + ctrl_br + "' and opt_id='" + ctrl_id + "'";
            opt_freez = seek_iname(Qstr, co_cd, mqry, "fstr");
            if (opt_freez == "Y") urights = "1";

            if (fixd_Date.Length > 5)
            {
                mqry = "SELECT (case when to_datE('" + doc_Dt + "','dd/mm/yyyy')<to_datE('" + fixd_Date + "','yyyy-mm-dd') then 'Y' else 'N' end) as fstr FROM FIN_RSYS_opt_PW WHERE branchcd='" + ctrl_br + "' and opt_id='" + ctrl_id + "'";
                opt_freez = seek_iname(Qstr, co_cd, mqry, "fstr");
                if (opt_freez == "Y") urights = "2";
            }
        }
        return urights;

    }

    public string Fn_chk_can_del(string Qstr, string co_cd, string userid, string formid)
    {
        urights = seek_iname(Qstr, co_cd, "SELECT RCAN_del FROM FIN_MRSYS WHERE USERID='" + userid + "' and ID='" + formid + "'", "RCAN_del");
        if (urights == "N") urights = "N";
        else urights = "Y";
        return urights;
    }
    public string chk_RsysUpd(string IdNo)
    {
        string result = "0";
        result = seek_iname_dt(fgenMV.fin_rsys_upd, "ID='" + IdNo + "'", "ID");
        return result;
    }
    public string add_RsysUpd(string Qstr, string CoCD_Fgen, string IdNo, string added_by)
    {
        //to add into fin_rsys_upd and refresh memory table
        //to avoid primary key error
        string result = "0";
        execute_cmd(Qstr, CoCD_Fgen, "insert into FIN_rSYS_UPD values ('" + IdNo + "','" + added_by + "',sysdate)");
        execute_cmd(Qstr, CoCD_Fgen, "commit");

        fgenMV.fin_rsys_upd = new DataTable();
        fgenMV.fin_rsys_upd = getdata(CoCD_Fgen, "SELECT NVL(IDNO,'-') AS ID FROM FIN_RSYS_UPD ORDER BY NVL(IDNO,'-')");

        return result;
    }

    public void chk_create_tab(string Qstr, string CoCD_Fgen)
    {
        string oraSQuery = "";
        string mhd;

        //-------------------------
        mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'FIN_RSYS_UPD'", "TNAME");
        if (mq0 == "0")
        {
            oraSQuery = "create table FIN_RSYS_UPD(IDNO varchar2(6) Default '-',ent_by varchar2(10) default '-',ent_Dt date default sysdate)";
            execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
        }
        //-------------------------

        fgenMV.iconTableFull = new DataTable();
        fgenMV.iconTableFull = getdata(CoCD_Fgen, "SELECT DISTINCT NVL(ID,'-') AS ID FROM FIN_MSYS ORDER BY NVL(ID,'-')");

        fgenMV.fin_rsys_upd = new DataTable();
        fgenMV.fin_rsys_upd = getdata(CoCD_Fgen, "SELECT NVL(IDNO,'-') AS ID FROM FIN_RSYS_UPD ORDER BY NVL(IDNO,'-')");

        mhd = chk_RsysUpd("CT0001");
        if (mhd == "0" || mhd == "")
        {
            add_RsysUpd(Qstr, CoCD_Fgen, "CT0001", "DEV_A");
            //execute_cmd(Qstr, CoCD_Fgen, "insert into FIN_RSYS_UPD values ('CT0001','DEV_A',sysdate)");

            mq0 = seek_iname(Qstr, CoCD_Fgen, "select tname from tab where tname='FIN_MSYS'", "tname");
            if (mq0 == "0" || mq0 == "") execute_cmd(Qstr, CoCD_Fgen, "CREATE TABLE FIN_MSYS(ID VARCHAR2(10),MLEVEL NUMBER(1),TEXT VARCHAR2(180) default '-',ALLOW_LEVEL NUMBER(2),WEB_aCTION VARCHAR2(50) default '-',SEARCH_KEY VARCHAR2(50) default '-',submenu char(1)default 'N',submenuid char(15) default '-',form varchar2(10) default '-',param varchar2(40) default '-',imagef varchar2(50) default '-',CSS varchar2(30) default 'fa-edit',PRD varchar2(1) default '-',BRN varchar2(1) default '-',BNR varchar2(1) default '-')");

            mq0 = check_filed_name(Qstr, CoCD_Fgen, "FIN_MSYS", "VISI");
            if (mq0 == "0" || mq0 == "") execute_cmd(Qstr, CoCD_Fgen, "ALTER TABLE FIN_MSYS ADD VISI CHAR(1)");

            mq0 = seek_iname(Qstr, CoCD_Fgen, "select distinct constraint_name from user_constraints where table_name='FIN_MSYS'", "constraint_name");
            if (mq0 == "0" || mq0 == "") execute_cmd(Qstr, CoCD_Fgen, "ALTER TABLE FIN_MSYS ADD CONSTRAINT FINRSYS_PK PRIMARY KEY (ID)");

            mq0 = seek_iname(Qstr, CoCD_Fgen, "select distinct constraint_name from user_constraints where table_name='FIN_RSYS_UPD'", "constraint_name");
            if (mq0 == "0" || mq0 == "") execute_cmd(Qstr, CoCD_Fgen, "ALTER TABLE FIN_RSYS_UPD ADD CONSTRAINT FINRSYSUPD_PK PRIMARY KEY (IDNO)");

            //execute_cmd(Qstr, CoCD_Fgen, "ALTER TABLE FIN_MSYS ADD CONSTRAINT FINRSYS_PK PRIMARY KEY (ID)");
            //execute_cmd(Qstr, CoCD_Fgen, "ALTER TABLE FIN_RSYS_UPD ADD CONSTRAINT FINRSYSUPD_PK PRIMARY KEY (IDNO)");

            mq0 = seek_iname(Qstr, CoCD_Fgen, "select tname from tab where tname='FIN_MRSYS'", "tname");
            if (mq0 == "0" || mq0 == "") execute_cmd(Qstr, CoCD_Fgen, "create table FIN_MRSYS(USERID VARCHAR2(10),USERNAME VARCHAR2(30),BRANCHCD CHAR(2),ENT_BY VARCHAR2(20),ENT_DT DATE,EDT_BY VARCHAR2(20),EDT_DT DATE,ID VARCHAR2(10),MLEVEL NUMBER(1),TEXT VARCHAR2(50),ALLOW_LEVEL NUMBER(2),WEB_ACTION  VARCHAR2(50),SEARCH_KEY  vARCHAR2(50),SUBMENU  CHAR(1),SUBMENUID CHAR(15),FORM VARCHAR2(10),PARAM  VARCHAR2(10),USER_COLOR VARCHAR(10) DEFAULT '00578b',IDESC VARCHAR(50) DEFAULT '-',CSS varchar2(30) default 'fa-edit',RCAN_ADD CHAR(1) DEFAULT 'Y',RCAN_EDIT CHAR(1) DEFAULT 'Y',RCAN_DEL CHAR(1) DEFAULT 'Y',VISI CHAR(1))");

            mq0 = check_filed_name(Qstr, CoCD_Fgen, "FIN_MRSYS", "VISI");
            if (mq0 == "0" || mq0 == "") execute_cmd(Qstr, CoCD_Fgen, "ALTER TABLE FIN_MRSYS ADD VISI CHAR(1)");

            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'WSR_CTRL'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "create TABLE WSR_CTRL (FINPKFLD CHAR(40),BRANCHCD CHAR(2),TYPE CHAR(2),VCHDATE DATE,VCHNUM CHAR(6),ENT_BY CHAR(15),ENT_DT DATE,PRINTED NUMBER(1),ACODE CHAR(10),CONSTRAINT WSR_CTRL_PK PRIMARY KEY (FINPKFLD) )";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'WSR_CTRL1'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "create TABLE WSR_CTRL1 (FINPKFLD CHAR(40),BRANCHCD CHAR(2),TYPE CHAR(2),VCHDATE DATE,VCHNUM CHAR(6),ENT_BY CHAR(15),ENT_DT DATE,PRINTED NUMBER(1),ACODE CHAR(10),CONSTRAINT WSR_CTRL1_PK PRIMARY KEY (FINPKFLD) )";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'WSR_CTRL2'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "create TABLE WSR_CTRL2 (FINPKFLD CHAR(40),BRANCHCD CHAR(2),TYPE CHAR(2),VCHDATE DATE,VCHNUM CHAR(6),ENT_BY CHAR(15),ENT_DT DATE,PRINTED NUMBER(1),ACODE CHAR(10),CONSTRAINT WSR_CTRL2_PK PRIMARY KEY (FINPKFLD) )";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'WSR_CTRL3'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "create TABLE WSR_CTRL3 (FINPKFLD CHAR(40),BRANCHCD CHAR(2),TYPE CHAR(2),VCHDATE DATE,VCHNUM CHAR(6),ENT_BY CHAR(15),ENT_DT DATE,PRINTED NUMBER(1),ACODE CHAR(10),CONSTRAINT WSR_CTRL3_PK PRIMARY KEY (FINPKFLD) )";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'WSR_CTRL4'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "create TABLE WSR_CTRL4 (FINPKFLD CHAR(40),BRANCHCD CHAR(2),TYPE CHAR(2),VCHDATE DATE,VCHNUM CHAR(6),ENT_BY CHAR(15),ENT_DT DATE,PRINTED NUMBER(1),ACODE CHAR(10),CONSTRAINT WSR_CTRL4_PK PRIMARY KEY (FINPKFLD) )";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'FIN_RSYS_OPT'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "CREATE TABLE FIN_RSYS_OPT(BRANCHCD CHAR(2),TYPE CHAR(2),VCHNUM CHAR(6),VCHDATE DATE DEFAULT SYSDATE,OPT_ID VARCHAR2(6) DEFAULT '-',OPT_TEXT VARCHAR2(200) DEFAULT '-',OPT_ENABLE VARCHAR2(1) DEFAULT '-',OPT_PARAM VARCHAR2(20) DEFAULT '-',OPT_PARAM2 VARCHAR2(20) DEFAULT '-',OPT_EXCL VARCHAR2(20) DEFAULT '-',ENT_BY VARCHAR2(10) DEFAULT '-',ENT_DT DATE DEFAULT SYSDATE,EDT_BY VARCHAR2(10) DEFAULT '-',EDT_DT DATE DEFAULT SYSDATE)";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            mq0 = seek_iname(Qstr, CoCD_Fgen, "select tname from tab where tname='SYS_CONFIG'", "tname");
            if (mq0 == "0" || mq0 == "")
            {
                execute_cmd(Qstr, CoCD_Fgen, "CREATE TABLE SYS_CONFIG ( BRANCHCD  CHAR(2),  TYPE  CHAR(2),  VCHNUM    CHAR(6),  VCHDATE   DATE,  SRNO  NUMBER(4),FRM_NAME  VARCHAR2(10),FRM_TITLE CHAR(30), OBJ_NAME  CHAR(20), OBJ_CAPTION  CHAR(30), OBJ_VISIBLE  CHAR(1), OBJ_WIDTH NUMBER(5), COL_NO    NUMBER(5), ENT_ID    CHAR(6), ENT_BY    CHAR(15), ENT_DT    DATE, EDT_BY    CHAR(15), EDT_DT    DATE, FRM_HEADER   CHAR(30), OBJ_MAXLEN   NUMBER(6), OBJ_READONLY     VARCHAR2(1), OBJ_FMAND     VARCHAR2(1) DEFAULT 'N' )");
            }
            mq0 = seek_iname(Qstr, CoCD_Fgen, "select tname from tab where tname='REP_CONFIG'", "tname");
            if (mq0 == "0" || mq0 == "")
            {
                execute_cmd(Qstr, CoCD_Fgen, "CREATE TABLE REP_CONFIG (BRANCHCD  CHAR(2), TYPE CHAR(2), VCHNUM    CHAR(6), VCHDATE   DATE, SRNO      NUMBER(4), FRM_NAME  VARCHAR2(10), FRM_TITLE CHAR(30), OBJ_NAME  VARCHAR2(100), OBJ_CAPTION  VARCHAR2(40), OBJ_VISIBLE  CHAR(1), OBJ_WIDTH NUMBER(5), COL_NO    NUMBER(5), ENT_ID    CHAR(6), ENT_BY    CHAR(15), ENT_DT    DATE, EDT_BY    CHAR(15), EDT_DT    DATE, FRM_HEADER   CHAR(30), OBJ_MAXLEN   NUMBER(6), OBJ_READONLY VARCHAR2(1), TABLE1    VARCHAR2(20), TABLE2    VARCHAR2(20), TABLE3    VARCHAR2(20), TABLE4    VARCHAR2(20), DATE_FLD  VARCHAR2(20), SORT_FLD  VARCHAR2(40), JOIN_COND VARCHAR2(175))");
            }
            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'UDF_CONFIG'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "CREATE TABLE UDF_CONFIG (BRANCHCD CHAR(2),TYPE CHAR(2), VCHNUM CHAR(6), VCHDATE DATE, SRNO NUMBER(4), FRM_NAME VARCHAR2(10)  default '-', FRM_TITLE CHAR(30)  default '-', OBJ_NAME varchar2(20)  default '-', OBJ_CAPTION varchar2(50)  default '-', OBJ_VISIBLE CHAR(1)  default '-', OBJ_WIDTH NUMBER(5)  default 0, COL_NO NUMBER(5)  default 0, ENT_ID CHAR(6), ENT_BY varchar2(15)  default '-', ENT_DT DATE default sysdate, EDT_BY varchar2(15)  default '-', EDT_DT DATE default sysdate, FRM_HEADER CHAR(30) default '-', OBJ_MAXLEN NUMBER(6) default 0, OBJ_READONLY VARCHAR2(1) default '-')";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'DBD_CONFIG'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "CREATE TABLE DBD_CONFIG (BRANCHCD CHAR(2),TYPE CHAR(2), VCHNUM CHAR(6), VCHDATE DATE, SRNO NUMBER(4), FRM_NAME VARCHAR2(10)  default '-', FRM_TITLE CHAR(30)  default '-', OBJ_NAME varchar2(20)  default '-', OBJ_CAPTION varchar2(50)  default '-', OBJ_VISIBLE CHAR(1)  default '-', OBJ_WIDTH NUMBER(5)  default 0, COL_NO NUMBER(5)  default 0, ENT_ID CHAR(6), ENT_BY varchar2(15)  default '-', ENT_DT DATE default sysdate, EDT_BY varchar2(15)  default '-', EDT_DT DATE default sysdate, FRM_HEADER CHAR(30) default '-', OBJ_MAXLEN NUMBER(6) default 0, OBJ_READONLY VARCHAR2(1) default '-', OBJ_SQL VARCHAR2(1000) default '-')";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'DSK_CONFIG'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "CREATE TABLE DSK_CONFIG (BRANCHCD CHAR(2),TYPE CHAR(2), VCHNUM CHAR(6), VCHDATE DATE, SRNO NUMBER(4), FRM_NAME VARCHAR2(10)  default '-', FRM_TITLE CHAR(30)  default '-', OBJ_NAME varchar2(100)  default '-', OBJ_CAPTION varchar2(50)  default '-', OBJ_VISIBLE CHAR(1)  default '-', OBJ_WIDTH NUMBER(5)  default 0, COL_NO NUMBER(5)  default 0, ENT_ID CHAR(6), ENT_BY varchar2(15)  default '-', ENT_DT DATE default sysdate, EDT_BY varchar2(15)  default '-', EDT_DT DATE default sysdate, FRM_HEADER CHAR(30) default '-', OBJ_MAXLEN NUMBER(6) default 0, OBJ_READONLY VARCHAR2(1) default '-', OBJ_SQL VARCHAR2(1000) default '-', OBJ_SQL2 VARCHAR2(1000) default '-')";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'DSK_WCONFIG'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "CREATE TABLE DSK_WCONFIG (BRANCHCD CHAR(2),TYPE CHAR(2), VCHNUM CHAR(6), VCHDATE DATE, SRNO NUMBER(4), USERID VARCHAr(10), USERNAME VARCHAR(30),OBJ_NAME VARCHAR(100) )";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'UDF_DATA'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "CREATE TABLE UDF_DATA (BRANCHCD CHAR(2),PAR_TBL VARCHAR2(30),PAR_FLD VARCHAR2(30),UDF_NAME VARCHAR2(30),UDF_VALUE VARCHAR2(100),SRNO NUMBER(4))";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'DBD_TV_CONFIG'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "CREATE TABLE DBD_TV_CONFIG (BRANCHCD CHAR(2),TYPE CHAR(2), VCHNUM CHAR(6), VCHDATE DATE, SRNO NUMBER(4), VERT_NAME VARCHAR2(10)  default '-', FRM_TITLE CHAR(30)  default '-', FRM_NAME varchar2(50)  default '-', OBJ_NAME varchar2(20)  default '-', OBJ_CAPTION varchar2(50)  default '-', OBJ_VISIBLE CHAR(1)  default '-', OBJ_WIDTH NUMBER(5)  default 0, COL_NO NUMBER(5)  default 0, ENT_ID CHAR(6), ENT_BY varchar2(15)  default '-', ENT_DT DATE default sysdate, EDT_BY varchar2(15)  default '-', EDT_DT DATE default sysdate, FRM_HEADER CHAR(30) default '-', OBJ_MAXLEN NUMBER(6) default 0, OBJ_READONLY VARCHAR2(10) default '-', OBJ_SQL VARCHAR2(1000) default '-')";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            mq0 = check_filed_name(Qstr, CoCD_Fgen, "DBD_TV_CONFIG", "OBJ_READONLY");
            if (mq0 == "0")
            {
                oraSQuery = "alter table DBD_TV_CONFIG modify OBJ_READONLY VARCHAR2(10) default '-'";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
            mq0 = check_filed_name(Qstr, CoCD_Fgen, "DBD_TV_CONFIG", "FRM_NAME");
            if (mq0 == "0")
            {
                oraSQuery = "alter table DBD_TV_CONFIG add FRM_NAME VARCHAR2(50) default '-'";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
        }
        mhd = chk_RsysUpd("CT0002");
        if (mhd == "0" || mhd == "")
        {
            //execute_cmd(Qstr, CoCD_Fgen, "insert into FIN_RSYS_UPD values ('CT0002','DEV_A',sysdate)");
            add_RsysUpd(Qstr, CoCD_Fgen, "CT0002", "DEV_A");

            mq0 = seek_iname(Qstr, CoCD_Fgen, "SELECT TNAME FROM TAB WHERE TNAME = 'DSC_INFO'", "TNAME");
            if (mq0 == "0")
            {
                oraSQuery = "CREATE TABLE DSC_INFO (BRANCHCD CHAR(2),TYPE CHAR(2), VCHNUM CHAR(6), VCHDATE DATE, REMARKS VARCHAR(80),FILENAME VARCHAR(60), FILEPATH VARCHAR(80), ENT_BY varchar2(15)  default '-', ENT_DT DATE default sysdate)";
                execute_cmd(Qstr, CoCD_Fgen, oraSQuery);
            }
        }
    }

    public bool IsDate(string datestr, string Informat)
    {
        try
        {
            DateTime.ParseExact(datestr, Informat, CultureInfo.InvariantCulture);
            return true;
        }
        catch (Exception err) { return false; }
    }
    public bool Update_data_fast1_uncommit(string UserCode, DataTable dtform, string tab_name, string where, bool edmode, Satransaction sat)
    {
        string createddate = server_datetime(UserCode);
        //sat = new Satransaction(UserCode);
        bool result = false;
        GC.Collect();

        using (OracleConnection fCon = new OracleConnection(ConnInfo.connString(UserCode)))
        {
            fCon.Close();
            fCon.Open(); DataTable dataupdate = new DataTable();

            //    DataTable dataupdate = new DataTable();

            if (!edmode) where = " where 1=2";
            else where = " where " + where;
            if (edmode && where.Equals("")) return false;

            if (edmode) sat.Execute_cmd("delete from " + tab_name + where, "");

            bool allcolsadded = false;
            string allcolumns = "";
            string commandtext = "";
            for (int i = 0; i < dtform.Rows.Count; i++)
            {



                string updvalues = "", colname = "", coltype = "", colval = "";
                bool res = false;
                for (int k = 0; k < dtform.Columns.Count; k++)
                {

                    colname = dtform.Columns[k].ColumnName;
                    coltype = dtform.Columns[k].DataType.Name.ToString().ToUpper().Replace("&nbsp;", "").Replace("&amp;", "");



                    if (coltype.ToUpper() == "DATETIME" && !IsDate(dtform.Rows[i][colname].ToString())) dtform.Rows[i][colname] = createddate;
                    if (coltype.ToUpper() == "DECIMAL" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                    if (coltype.ToUpper() == "DOUBLE" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                    if (coltype.ToUpper() == "INT16" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                    if (coltype.ToUpper() == "INT32" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                    if (coltype.ToUpper() == "INT64" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                    if (coltype.ToUpper() == "SINGLE" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                    if (coltype.ToUpper() == "STRING" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                    if (coltype.ToUpper() == "BOOLEAN" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                    if (!coltype.ToUpper().Equals("DATETIME"))
                    {
                        dtform.Rows[i][colname] = dtform.Rows[i][colname].ToString().Replace("'", "`").Trim();
                    }
                    if (colname.ToUpper().Equals("REC_ID"))
                    {
                    }
                    else
                    {
                        if (!allcolsadded)
                        {
                            if (allcolumns == "") allcolumns = colname;
                            else allcolumns += "," + colname;
                        }
                        if (coltype.Equals("DATETIME"))
                        {
                            colval = ((DateTime)dtform.Rows[i][colname]).ToString("yyyy-MM-dd HH:mm:ss");
                            if (updvalues.Equals("")) updvalues = "to_date('" + colval + "','YYYY-MM-DD HH24:MI:SS')";
                            else updvalues = updvalues + ",to_date('" + colval + "','YYYY-MM-DD HH24:MI:SS')";
                        }
                        else if (coltype.Equals("BOOLEAN"))
                        {
                            colval = ((Boolean)dtform.Rows[i][colname]).ToString();
                            if (updvalues.Equals("")) updvalues = "" + colval + "";
                            else updvalues = updvalues + "," + colval + "";
                        }
                        else
                        {
                            colval = dtform.Rows[i][colname].ToString();
                            if (updvalues.Equals("")) updvalues = "'" + colval + "'";
                            else updvalues = updvalues + ",'" + colval + "'";
                        }
                    }
                }
                //if (!allcolsadded)
                //{
                //    commandtext = "insert into  " + tab_name + " (" + allcolumns + ") ";
                //}
                commandtext = "insert into  " + tab_name + " (" + allcolumns + ") ";
                commandtext += " SELECT " + updvalues + " FROM DUAL ";
                allcolsadded = true;
                //FILL_ERR(commandtext);
                bool done = sat.Execute_cmd(commandtext);
                if (!done)
                {
                    //sat.Rollback();
                    //showmsg(1, ress, 0);
                    return false;
                }
            }
            //string lastWord = commandtext.Split(' ').Last();
            //if (lastWord.Trim().ToUpper().Equals("UNION"))
            //{
            //    commandtext = commandtext.Remove(commandtext.LastIndexOf(' ') + 1);
            //}

            //string ress = sat.Execute_cmd(commandtext);

            //sat.Commit();
            //dataupdate.Dispose();
            return true;
        }
    }

    public bool Update_data_fast1(string UserCode, DataTable dtform, string tab_name, string where, bool edmode)
    {
        string createddate = server_datetime(UserCode);
        Satransaction sat = new Satransaction(UserCode, MyGuid);
        bool result = false;
        GC.Collect();
        //using (OracleConnection fCon = new OracleConnection(Multiton.connString(UserCode)))
        //{
        //fCon.Close();
        //fCon.Open();
        DataTable dataupdate = new DataTable();

        if (!edmode) where = " where 1=2";
        else where = " where " + where;
        if (edmode && where.Equals("")) return false;

        if (edmode) sat.Execute_cmd("delete from " + tab_name + where, "");

        bool allcolsadded = false;
        string allcolumns = "";
        string commandtext = "";
        for (int i = 0; i < dtform.Rows.Count; i++)
        {
            string updvalues = "", colname = "", coltype = "", colval = "";
            bool res = false;
            for (int k = 0; k < dtform.Columns.Count; k++)
            {

                colname = dtform.Columns[k].ColumnName;
                coltype = dtform.Columns[k].DataType.Name.ToString().ToUpper().Replace("&nbsp;", "").Replace("&amp;", "");

                if (coltype.ToUpper() == "DATETIME" && !IsDate(dtform.Rows[i][colname].ToString())) dtform.Rows[i][colname] = createddate;
                if (coltype.ToUpper() == "DECIMAL" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                if (coltype.ToUpper() == "DOUBLE" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                if (coltype.ToUpper() == "INT16" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                if (coltype.ToUpper() == "INT32" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                if (coltype.ToUpper() == "INT64" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                if (coltype.ToUpper() == "SINGLE" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                if (coltype.ToUpper() == "STRING" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = "-";
                if (coltype.ToUpper() == "BOOLEAN" && dtform.Rows[i][colname].ToString().Replace("'", "`").Trim().Length == 0) dtform.Rows[i][colname] = 0;
                if (!coltype.ToUpper().Equals("DATETIME"))
                {
                    dtform.Rows[i][colname] = dtform.Rows[i][colname].ToString().Replace("'", "`").Trim();
                }
                if (colname.ToUpper().Equals("REC_ID")) { }
                else
                {
                    if (!allcolsadded)
                    {
                        if (allcolumns == "") allcolumns = colname;
                        else allcolumns += "," + colname;
                    }
                    if (coltype.Equals("DATETIME"))
                    {
                        colval = ((DateTime)dtform.Rows[i][colname]).ToString("yyyy-MM-dd HH:mm:ss");
                        if (updvalues.Equals("")) updvalues = "to_date('" + colval + "','YYYY-MM-DD HH24:MI:SS')";
                        else updvalues = updvalues + ",to_date('" + colval + "','YYYY-MM-DD HH24:MI:SS')";
                    }
                    else if (coltype.Equals("BOOLEAN"))
                    {
                        colval = ((Boolean)dtform.Rows[i][colname]).ToString();
                        if (updvalues.Equals("")) updvalues = "" + colval + "";
                        else updvalues = updvalues + "," + colval + "";
                    }
                    else
                    {
                        colval = dtform.Rows[i][colname].ToString();
                        if (updvalues.Equals("")) updvalues = "'" + colval + "'";
                        else updvalues = updvalues + ",'" + colval + "'";
                    }
                }
            }
            if (!allcolsadded)
            {
                commandtext = "insert into  " + tab_name + " (" + allcolumns + ") ";
            }
            commandtext += " SELECT distinct " + updvalues + " FROM DUAL ";
            if (i == dtform.Rows.Count - 1)
            { }
            else
            {
                commandtext += " UNION ALL ";
            }
            allcolsadded = true;
        }
        bool done = sat.Execute_cmd(commandtext);
        if (!done)
        {
            sat.Rollback();
            return false;
        }
        else
        {
            sat.Commit();
            return true;
        }
        //}
    }

    //public void open_report_byDs_ERP(string usercode, DataSet ds, string rptname, string title, bool addlogo)
    //{
    //    Multiton multiton = Multiton.GetInstance(MyGuid);
    //    if (addlogo) ds.Tables.Add(Get_Type_Data(MyGuid, usercode, Multiton.Get_Mvar(MyGuid, "U_MBR"), "Y"));
    //    else ds.Tables.Add(Get_Type_Data(MyGuid, usercode, Multiton.Get_Mvar(MyGuid, "U_MBR")));
    //    HttpContext.Current.Session[MyGuid + "_Data"] = null;
    //    HttpContext.Current.Session[MyGuid + "_DataDS"] = ds;
    //    HttpContext.Current.Session[MyGuid + "_Report"] = rptname;
    //    HttpContext.Current.Session[MyGuid + "_title"] = title;
    //    PrintRptNew(title);
    //    //ShowRpt_xml();

    //}
    public DataTable getdata(string userCode, string DataTable_Query)
    {
        return getdata_DR(userCode, DataTable_Query);
        //GC.Collect();
        //DataTable dt = new DataTable();
        //try
        //{
        //    dt.Clear();
        //    using (OracleConnection fCon = new OracleConnection(connStringmyOracle(userCode)))
        //    {
        //        fCon.Open();
        //        using (OracleCommand cmd = new OracleCommand(DataTable_Query, fCon))
        //        {
        //            var adapter = new OracleDataAdapter(cmd);
        //            adapter.Fill(dt);
        //        }
        //    }
        //}
        //catch (Exception EX)
        //{
        //    dt.Clear();
        //    FILL_ERR(connStringmyOracle(userCode) + "===" + EX.ToString() + " ==> GetData Fun");
        //    if (EX.Message.Equals("Unable to connect to any of the specified MyOracle hosts."))
        //    {
        //        showmsg(1, "Network Connection Error!Either Check your Server or Network", 1);
        //    }
        //}

        //return dt;

    }
    public DataTable getdata_DR(string cdcode, string qu)
    {
        GC.Collect();

        DataTable dt_get = new DataTable();
        if (!qu.Trim().Equals(""))
        {
            try
            {
                dt_get.Clear();
                OracleConnection fcon = new OracleConnection (ConnInfo.connString(cdcode));

                fcon.Open();
                using (OracleCommand cmd = new OracleCommand(qu, fcon))
                {
                    using (OracleDataReader dr_reader = cmd.ExecuteReader())
                    {
                        if (dr_reader != null)
                        {
                            //if (dr_reader.HasRows)
                            dt_get.Load(dr_reader);
                            dr_reader.Close();
                            dr_reader.Dispose();
                            cmd.Dispose();
                        }
                        if (!dr_reader.IsClosed)
                        {
                            dr_reader.Close();
                            dr_reader.Dispose();
                        }
                    }
                }
                fcon.Close();
                fcon.Dispose();

                //foreach (DataColumn dc in dt_get.Columns)
                //{
                //    dc.AllowDBNull = true;
                //    dc.ReadOnly = false;
                //    dc.MaxLength = -1;
                //}
                //dt_get.AcceptChanges();
            }
            catch (Exception EX)
            {
                var LineNumber = new StackTrace(EX, true).GetFrame(0).GetFileLineNumber();
                FILL_ERR(EX.Message.ToString().Trim() + " at Line number" + LineNumber + " ==> GetData Fun");
            }
        }
        return dt_get;
    }
    public void FILL_ERR(string msg)
    {
        string ppath = @"c:\info\err.txt";
        try
        {
            if (File.Exists(ppath))
            {
                StreamWriter w = File.AppendText(ppath);
                w.WriteLine(msg.ToString() + "-->" + DateTime.Now.ToString("ddMMyyyy hh:mm:ss tt"));
                w.WriteLine("=====================================================================");
                w.Flush();
                w.Close();
            }
            else
            {
                StreamWriter w = new StreamWriter(ppath, true);
                w.WriteLine(msg.ToString() + "-->" + DateTime.Now.ToString("ddMMyyyy hh:mm:ss tt"));
                w.WriteLine("=====================================================================");
                w.Flush();
                w.Close();
            }
        }
        catch { }
    }

    public void SetSession(string MyGuid, string SessionName, object value)
    {
        HttpContext.Current.Session[MyGuid + "_" + SessionName] = value;
    }
    public object GetSession(string MyGuid, string SessionName)
    {
        return HttpContext.Current.Session[MyGuid + "_" + SessionName];
    }
    public void SetCookie(string MyGuid, string name, string value)
    {
        //Writing Multiple values in single cookie
        HttpContext.Current.Response.Cookies.Remove(MyGuid + "_" + name);
        HttpCookie hc = new HttpCookie(MyGuid + "_" + name);
        hc.Value = value;
        HttpContext.Current.Response.Cookies.Add(hc);
    }
    public string GetCookie(string MyGuid, string name)
    {
        string val = "";
        if (HttpContext.Current.Request.Cookies[MyGuid + "_" + name] != null)
        {
            val = HttpContext.Current.Request.Cookies[MyGuid + "_" + name].Value.ToString();
        }
        return val;
    }
    public string Getdateformat()
    {
        try { return HttpContext.Current.Session[MyGuid + "_dateformat"].ToString(); }
        catch (Exception err) { return GetCookie(MyGuid, "dateformat").ToString(); }

    }
    public string GetSaveDateFormat()
    {
        return "yyyy-MM-dd HH:mm:ss";

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="msgtype">values are as 1 for OK only,2 for (Yes No )Confirmation,3 for </param>
    /// <param name=""></param>
    /// <param name="alert_type">Values are 0 for Error,1 for Success and 2 for Warning</param>
    //public void showmsg(int msgtype, String msg, int alert_type)
    //{
    //    Controller controller = (Controller)HttpContext.Current.Session["TController"];
    //    controller.ViewBag.scripCall += "showmsgJS(" + msgtype + ", '" + msg.Replace("'", "") + "', " + alert_type + ");";
    //}
    //public void showFoo(string title)
    //{
    //    Controller controller = (Controller)HttpContext.Current.Session["TController"];
    //    controller.ViewBag.scripCall += "callFoo('" + title + "');";
    //}
    //public void PrintRptNew(string Title)
    //{
    //    Controller controller = (Controller)HttpContext.Current.Session["TController"];
    //    controller.ViewBag.scripCall += "showRptnew('" + Title + "');";
    //}
    //public void SetRPT(string RptID)
    //{
    //    Controller controller = (Controller)HttpContext.Current.Session["TController"];
    //    controller.ViewBag.scripCall += "sessionStorage.setItem('IconID','" + RptID + "'); sessionStorage.setItem('callbackfun', 'showreps');";
    //}
    //public void showDateFilter(string id, string br)
    //{
    //    Controller controller = (Controller)HttpContext.Current.Session["TController"];
    //    controller.ViewBag.scripCall += "datefilter('" + id + "', '" + br + "', 'showreps');";
    //}


    /// <summary>
    /// 
    /// </summary>
    /// <param name="alert_type">Values are 0 for Error,1 for Success and 2 for Warning,3 for info </param>
    /// <param name="msg"></param>
    //public void showToast(Controller controller, int alert_type, String msg)
    //{
    //    switch (alert_type)
    //    {
    //        case 0:
    //            controller.ViewBag.scripCall += "mytoast('error', 'toast-top-right', '" + msg.Replace("'", "") + "');";
    //            break;
    //        case 1:
    //            controller.ViewBag.scripCall += "mytoast('success', 'toast-top-right', '" + msg.Replace("'", "") + "');";
    //            break;
    //        case 2:
    //            controller.ViewBag.scripCall += "mytoast('warning', 'toast-top-right', '" + msg.Replace("'", "") + "');";
    //            break;
    //        case 3:
    //            controller.ViewBag.scripCall += "mytoast('info', 'toast-top-right', '" + msg.Replace("'", "") + "');";
    //            break;

    //    }

    //}

    //public string EnableForm(Controller controller1 = null)
    //{
    //    Controller controller = (Controller)HttpContext.Current.Session["TController"];
    //    controller.ViewBag.vnew = "disabled='disabled'";
    //    controller.ViewBag.vedit = "disabled='disabled'";
    //    controller.ViewBag.vsave = "";
    //    controller.ViewBag.scripCall += "enableForm();";
    //    return "Y";
    //}
    //public string DisableForm(Controller controller1 = null)
    //{
    //    Controller controller = (Controller)HttpContext.Current.Session["TController"];
    //    controller.ViewBag.vnew = "";
    //    controller.ViewBag.vedit = "";
    //    controller.ViewBag.vsave = "disabled='disabled'";
    //    controller.ViewBag.scripCall += "disableForm();";
    //    return "N";
    //}
    public DataTable Get_Type_Data(string Qstr, string pco_Cd, string mbr)
    {
        string firm = "";
        string footerGeneratedBy = "Generated By AIPL ERP";
        DataTable ds = new DataTable();
        firm = chk_co(pco_Cd);
        ds = getdata(pco_Cd, "select name as brName,addr as brAddr,addr1 as brAddr1,addr2 as brAddr2,place as brplace,tele as brTele,fax as brFax,format(rcdate,'dd/MM/yyyy') AS brRCDATE,format(cstdt,'dd/MM/yyyy') AS brcstdt,ec_code as brec_code,exc_regn as brexc_regn,exc_rang as brexc_rang,exc_div as brexc_div, RCNUM as brRCNUM, cstno as brcstno,LOWER(email) as br_email,'" + firm + "' as firm,'" + pco_Cd + "' as co_cd,'" + footerGeneratedBy + "' as footerGeneratedBy, LOWER(website) AS brwebsite,exc_tarrif as brexc_tarrif,gir_num as brgir_num,zipcode as brzipcode, bank_pf as brbank_pf, mfg_licno as brmfg_licno, est_code as brest_code, tds_num as brtds_num, exc_item as brexc_item, BANKNAME as brBANKNAME, BANKaddr as brBANKaddr, BANKaddr1 as brBANKaddr1 , BANKac as brBANKac , vat_form as brvat_form, stform as brstform, IFSC_CODE as brIFSC_CODE, RADDR as brRADDR, RADDR1 as brRADDR1, haddr as brhaddr, haddr1 as brhaddr1 , rphone as brrphone, hphone as brhphone, email1 as bremail1, email2 as bremail2, email3 as bremail3, email4 as bremail4, email5 as bremail5, co_cin as brco_cin, countrynm as brcountrynm, msme_no as brmsme_no, exc_Addr as brexc_Addr , gst_no as brgst_no , substring(a.gst_no,0,2) as brstatecode, bond_ut as brbond_ut, STATENM as brSTATENM from type a where a.type1='" + mbr + "' and upper(a.id)='B'");
        ds.TableName = "Type";
        return ds;
    }
    public string checkSpecialFirm(string pcocd, string fmbr)
    {
        switch (pcocd)
        {
            case "PRIN":
                switch (fmbr)
                {
                    case "00":
                        firm = "PREM INDUSTRIES UNIT III";
                        break;
                    case "02":
                        firm = "PREM INDUSTRIES UNIT II";
                        break;
                    case "03":
                        firm = "PREM INDUSTRIES UNIT I";
                        break;
                    case "04":
                        firm = "PREM INDUSTRIES";
                        break;
                    case "05":
                        firm = "PREM INDUSTRIES WAREHOUSE";
                        break;
                    default:
                        firm = "PREM INDUSTRIES UNIT III";
                        break;
                }
                break;
            default:
                firm = fgenCO.chk_co(pcocd);
                break;
        }
        return firm;
    }
    public DataSet Get_Type_Data(string Qstr, string pco_Cd, string mbr, DataSet ds)
    {
        string branchNameAsFirmName = "", br_name = "name";
        string footerGeneratedBy = "Generated By Tejaxo";
        if (pco_Cd == "SRIS") footerGeneratedBy = "Generated By Tejaxo";
        string printRegHeadings = fgenMV.Fn_Get_Mvar(Qstr, "U_PRINT_REG_HEADINGS");
        firm = checkSpecialFirm(pco_Cd, mbr);
        MV_CLIENT_GRP = fgenMV.Fn_Get_Mvar(Qstr, "U_CLIENT_GRP");
        if (MV_CLIENT_GRP == "SG_TYPE" || pco_Cd == "MLGI") branchNameAsFirmName = "Y";
        firm = "'" + firm + "' as firm";
        if (branchNameAsFirmName == "Y")
        {
            firm = "name as firm";
            br_name = "' '";
            if (MV_CLIENT_GRP == "SG_TYPE") br_name = "'UNIT OF SALMAN GROUP'";
        }
        if (pco_Cd == "0") pco_Cd = Qstr.Split('^')[0];
        using (OracleConnection fcon = new OracleConnection(fgenMV.Fn_Get_Mvar(Qstr, "CONN")))
        {
            fcon.Open();
            using (OracleDataAdapter fgen_da = new OracleDataAdapter("select " + br_name + " as brName,addr as brAddr,addr1 as brAddr1,addr2 as brAddr2,place as brplace,tele as brTele,fax as brFax,TO_CHAR(rcdate,'DD/MM/YYYY') AS brRCDATE,TO_CHAR(cstdt,'DD/MM/YYYY') AS brcstdt,ec_code as brec_code,exc_regn as brexc_regn,exc_rang as brexc_rang,exc_div as brexc_div, RCNUM as brRCNUM, cstno as brcstno,LOWER(email) as br_email," + firm + ",'" + pco_Cd + "' as co_cd,'" + footerGeneratedBy + "' as footerGeneratedBy, LOWER(website) AS brwebsite,exc_tarrif as brexc_tarrif,gir_num as brgir_num,zipcode as brzipcode, bank_pf as brbank_pf, mfg_licno as brmfg_licno, est_code as brest_code, tds_num as brtds_num, exc_item as brexc_item, BANKNAME as brBANKNAME, BANKaddr as brBANKaddr, BANKaddr1 as brBANKaddr1 , BANKac as brBANKac , vat_form as brvat_form, stform as brstform, IFSC_CODE as brIFSC_CODE, RADDR as brRADDR, RADDR1 as brRADDR1, haddr as brhaddr, haddr1 as brhaddr1 , rphone as brrphone, hphone as brhphone, email1 as bremail1, email2 as bremail2, email3 as bremail3, email4 as bremail4, email5 as bremail5, co_cin as brco_cin, countrynm as brcountrynm, msme_no as brmsme_no, exc_Addr as brexc_Addr , gst_no as brgst_no , substr(a.gst_no,0,2) as brstatecode, bond_ut as brbond_ut, STATENM as brSTATENM,cexc_comm,AUDIT_," + printRegHeadings + ",br_curren,exc_rang as paisa_curren,num_fmt1,num_fmt2 from type a where a.type1='" + mbr + "' and upper(a.id)='B'", fcon))
            {
                fgen_da.Fill(ds, "Type");
            }
        }
        return ds;
    }

    public DataTable Get_Type_Data(string Qstr, string pco_Cd, string mbr, string printLogo)
    {
        string firm = "";
        string footerGeneratedBy = "Generated by ERP";
        DataTable ds = new DataTable();
        firm = chk_co(pco_Cd);
        ds = getdata(pco_Cd, "select name as brName,addr as brAddr,addr1 as brAddr1,addr2 as brAddr2,place as brplace,tele as brTele,fax as brFax,format(rcdate,'dd/MM/yyyy') AS brRCDATE,format(cstdt,'dd/MM/yyyy') AS brcstdt,ec_code as brec_code,exc_regn as brexc_regn,exc_rang as brexc_rang,exc_div as brexc_div, RCNUM as brRCNUM, cstno as brcstno,LOWER(email) as br_email,'" + firm + "' as firm,'" + pco_Cd + "' as co_cd,'" + footerGeneratedBy + "' as footerGeneratedBy, LOWER(website) AS brwebsite,exc_tarrif as brexc_tarrif,gir_num as brgir_num,zipcode as brzipcode, bank_pf as brbank_pf, mfg_licno as brmfg_licno, est_code as brest_code, tds_num as brtds_num, exc_item as brexc_item, BANKNAME as brBANKNAME, BANKaddr as brBANKaddr, BANKaddr1 as brBANKaddr1 , BANKac as brBANKac , vat_form as brvat_form, stform as brstform, IFSC_CODE as brIFSC_CODE, RADDR as brRADDR, RADDR1 as brRADDR1, haddr as brhaddr, haddr1 as brhaddr1 , rphone as brrphone, hphone as brhphone, email1 as bremail1, email2 as bremail2, email3 as bremail3, email4 as bremail4, email5 as bremail5, co_cin as brco_cin, countrynm as brcountrynm, msme_no as brmsme_no, exc_Addr as brexc_Addr , gst_no as brgst_no , substring(a.gst_no,0,2) as brstatecode, bond_ut as brbond_ut, STATENM as brSTATENM from type a where a.type1='" + mbr + "' and upper(a.id)='B'");
        ds.TableName = "Type";
        if (printLogo == "Y")
        {
            ds = addLogo(pco_Cd, ds);
        }
        return ds;
    }
    DataTable addLogo(string fCocd, DataTable dataTable)
    {
        DataTable dtN = new DataTable();
        try
        {
            FileStream FilStr;
            BinaryReader BinRed;
            //string fpath = @"c:\info\logo\mlogo_" + fCocd + ".jpg";
            string fpath = HttpContext.Current.Server.MapPath(@"~\uploads\" + fCocd + "\\mlogo.png");
            if (dataTable.Rows.Count > 0)
            {
                if (!dataTable.Columns.Contains("mLogo")) dataTable.Columns.Add("mLogo", typeof(System.Byte[]));
            }
            dtN = dataTable.Clone();
            foreach (DataRow dr in dataTable.Rows)
            {
                FilStr = new FileStream(fpath, FileMode.Open);
                BinRed = new BinaryReader(FilStr);
                dr["mLogo"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);
                FilStr.Close();
                BinRed.Close();
            }
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dtN.ImportRow(dataTable.Rows[i]);
            }
            dtN.TableName = dataTable.TableName.ToString();
        }
        catch
        {
            FILL_ERR("Logo File not found in INFO folder " + @"c:\info\logo\mlogo_" + fCocd + ".jpg");
        }
        return dtN;
    }
    public string check_control(string Qstr, string pco_Cd, string control_name)
    {
        string vp = seek_iname(Qstr, pco_Cd, "Select " + control_name + " as vip from control ", "vip");
        return vp;
    }
    public string seek_iname(string Qstr, string co_Cd, string Squery, string Seek_Val1)
    {
        string ReturnVal = "";
        using (DataTable dt_Rows = getdata(co_Cd, Squery))
        {
            if (dt_Rows.Rows.Count > 0)
            {
                if (dt_Rows.Rows[0][Seek_Val1].ToString().Trim().Length > 0) ReturnVal = dt_Rows.Rows[0][Seek_Val1].ToString().Trim();
                else ReturnVal = "0";
            }
            else ReturnVal = "0";
        }
        return ReturnVal.Trim();
    }
    public bool execute_cmd(string userCode, string Execute_Query)
    {
        bool resultVal = false;
        Execute_Query = Execute_Query.Replace("&quot;", "").Replace("&nbsp;", "");
        OracleConnection fCon = new OracleConnection(ConnInfo.connString(userCode));
        {
            try
            {
                fCon.Open();
                using (OracleCommand cmd = new OracleCommand(Execute_Query, fCon))
                {
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "commit";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    resultVal = true;
                }
                fCon.Close();
                fCon.Dispose();
            }
            catch (Exception EX)
            {
                FILL_ERR(EX.Message.ToString().Trim() + " ==> Execute Command Fun");
            }
        }
        //}
        return resultVal;
    }
    public bool execute_cmd(string qstr, string userCode, string Execute_Query)
    {
        bool resultVal = false;
        Execute_Query = Execute_Query.Replace("&quot;", "").Replace("&nbsp;", "");
        OracleConnection fCon = new OracleConnection(ConnInfo.connString(userCode));
        {
            try
            {
                fCon.Open();
                using (OracleCommand cmd = new OracleCommand(Execute_Query, fCon))
                {
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "commit";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    resultVal = true;
                }
                fCon.Close();
                fCon.Dispose();
            }
            catch (Exception EX)
            {
                FILL_ERR(EX.Message.ToString().Trim() + " ==> Execute Command Fun");
            }
        }
        //}
        return resultVal;
    }
    public DataTable CheckUserDetails(string _qstr, string comp, string userName)
    {
        DataTable dtGetDetails = new DataTable();
        dtGetDetails = getdata(comp, "select a.*,a.level3pw as password from evas a where TRIM(UPPER(a.USERNAME))='" + userName + "' AND TRIM(UPPER(a.USERNAME)) LIKE '" + userName + "%'");
        return dtGetDetails;
    }
    public bool MatchPwd(string _qstr, string comp, string UserName, string UserPwd)
    {
        bool result = false;
        DataTable dtFFF = new DataTable();
        if (HttpContext.Current.Session["dtGetD"] != null) dtFFF = (DataTable)HttpContext.Current.Session["dtGetD"];
        else dtFFF = CheckUserDetails(_qstr, comp, UserName);
        if (dtFFF.Rows.Count > 0)
        {
            if (dtFFF.Rows[0]["level3pw"].ToString().Trim() == UserPwd) result = true;
            else result = false;
        }
        return result;
    }
    public string GetUserValue(string _qstr, string comp, string UserName, string fieldName)
    {
        string result = "";
        DataTable dtFFF = new DataTable();
        if (HttpContext.Current.Session["dtGetD"] != null) dtFFF = (DataTable)HttpContext.Current.Session["dtGetD"];
        else dtFFF = CheckUserDetails(_qstr, comp, UserName);
        if (dtFFF.Rows.Count > 0)
        {
            result = seek_iname_dt(dtFFF, "username='" + UserName + "'", fieldName);
        }
        return result;
    }
    public bool confirmUser(DataTable MainDT, string UserName, string UserPwd)
    {
        bool result = false;
        try
        {
            if (MainDT.Rows[0]["level3pw"].ToString() == UserPwd)
            {
                result = true;
            }
        }
        catch { }
        return result;
    }
    public string seek_iname_dt(DataTable dt_seek, string conditions, string col1)
    {
        string result = "0";
        if (dt_seek.Rows.Count > 0)
        {
            DataRow[] rows = dt_seek.Select(conditions, "", System.Data.DataViewRowState.CurrentRows);
            if (rows.Length == 0) result = "0";
            else
            {
                result = rows[0][col1].ToString().Trim();
            }
        }
        return result;
    }
    public string check_filed_name(string Qstr, string pco_Cd, string Table_Name, string Filed_Name)
    {
        string mhd = seek_iname(Qstr, pco_Cd, "SELECT upper(COLUMN_NAME) as COLUMN_NAME FROM USER_TAB_COLUMNS WHERE upper(TABLE_NAME)='" + Table_Name.Trim().ToUpper() + "' AND upper(COLUMN_NAME)='" + Filed_Name.Trim().ToUpper() + "'", "column_name").Trim();
        return mhd.Trim();
    }
    public bool MatchUser(string _qstr, string comp, string UserName, string UserPwd)
    {
        bool result = false;
        DataTable dtFFF = new DataTable();
        if (HttpContext.Current.Session["dtGetD"] != null) dtFFF = (DataTable)HttpContext.Current.Session["dtGetD"];
        else dtFFF = CheckUserDetails(_qstr, comp, UserName);
        if (dtFFF.Rows.Count > 0)
        {
            if (dtFFF.Rows[0]["username"].ToString().Trim() == UserName) result = true;
            else result = false;
        }
        return result;
    }
    public void chk_icon(string Uniq_Qstr, string comp_code)
    {
        Cls_comp_code = comp_code;
        mulevel = Multiton.Get_Mvar(Uniq_Qstr, "U_ULEVEL");
        muname = Multiton.Get_Mvar(Uniq_Qstr, "U_UNAME");

        execute_cmd(Uniq_Qstr, comp_code, "alter TABLE SR_CTRL modify FINPKFLD VARCHAR2(40)");

        mhd = seek_iname(Uniq_Qstr, comp_code, "select tname from tab where tname='ICO_TAB'", "TNAME");
        if (mhd == "0" || mhd == "") execute_cmd(Uniq_Qstr, comp_code, "CREATE TABLE ico_tab(ID VARCHAR2(10),MLEVEL NUMBER(1),TEXT VARCHAR2(50) DEFAULT '-',ALLOW_LEVEL NUMBER(2),WEB_ACTION VARCHAR2(50) DEFAULT '-',SEARCH_KEY VARCHAR2(50) DEFAULT '-',SUBMENU CHAR(1)DEFAULT 'N',SUBMENUID CHAR(15) DEFAULT '-',FORM VARCHAR2(10) DEFAULT '-',PARAM VARCHAR2(10) DEFAULT '-',IMAGEF VARCHAR2(50) DEFAULT '-',CSS VARCHAR2(10) DEFAULT '-')");
        execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE ico_tab MODIFY ID VARCHAR(10) DEFAULT '-'");

        mhd = check_filed_name(Uniq_Qstr, comp_code, "ico_tab", "IMAGEF");
        if (mhd == "0")
        {
            execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE ico_tab ADD IMAGEF VARCHAR(50) DEFAULT '-'");

            mhd = check_filed_name(Uniq_Qstr, comp_code, "ico_tab", "BRN"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE ico_tab ADD BRN CHAR(1) DEFAULT 'Y'");
            mhd = check_filed_name(Uniq_Qstr, comp_code, "ico_tab", "PRD"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE ico_tab ADD PRD CHAR(1) DEFAULT 'Y'");
            mhd = check_filed_name(Uniq_Qstr, comp_code, "ico_tab", "VISI"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE ico_tab ADD VISI CHAR(1) DEFAULT 'Y'");
            mhd = check_filed_name(Uniq_Qstr, comp_code, "ico_tab", "UPD_BY"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE ico_tab ADD UPD_BY varchar2(15) DEFAULT '-'");
            mhd = check_filed_name(Uniq_Qstr, comp_code, "ico_tab", "UPD_DT"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE ico_tab ADD UPD_DT date DEFAULT GETDATE()");

            mhd = check_filed_name(Uniq_Qstr, comp_code, "TYPE", "TBRANCHCD"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE type ADD TBRANCHCD VARCHAR2(2) DEFAULT '00'");
            mhd = check_filed_name(Uniq_Qstr, comp_code, "TYPE", "TVCHNUM"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE type ADD TVCHNUM VARCHAR2(6) DEFAULT '-'");
            mhd = check_filed_name(Uniq_Qstr, comp_code, "TYPE", "TVCHDATE"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE type ADD TVCHDATE date DEFAULT GETDATE()");

            mhd = check_filed_name(Uniq_Qstr, comp_code, "ITEM", "IVCHNUM"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE item ADD IVCHNUM VARCHAR2(6) ");
            mhd = check_filed_name(Uniq_Qstr, comp_code, "ITEM", "IVCHDATE"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE item ADD IVCHDATE date ");

            mhd = check_filed_name(Uniq_Qstr, comp_code, "ITEM", "IVCHNUM"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE item modify IVCHNUM VARCHAR2(6) default '-' ");
            mhd = check_filed_name(Uniq_Qstr, comp_code, "ITEM", "IVCHDATE"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "ALTER TABLE item modify IVCHDATE date default GETDATE()");

            mhd = check_filed_name(Uniq_Qstr, comp_code, "ico_wtab", "RCAN_ADD"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "Alter Table ico_wtab add RCAN_ADD VARCHAR2(1) default 'Y'");
            mhd = check_filed_name(Uniq_Qstr, comp_code, "ico_wtab", "RCAN_EDIT"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "Alter Table ico_wtab add RCAN_EDIT VARCHAR2(1) default 'Y'");
            mhd = check_filed_name(Uniq_Qstr, comp_code, "ico_wtab", "RCAN_DEL"); if (mhd == "0") execute_cmd(Uniq_Qstr, comp_code, "Alter Table ico_wtab add RCAN_DEL VARCHAR2(1) default 'Y'");
        }

        execute_cmd(Uniq_Qstr, comp_code, "COMMIT");

        if ((mulevel != "M" && comp_code == "LIVN") || comp_code != "FINS" || comp_code != "MLGA" || comp_code != "PKGW")
        {
            add_icon(Uniq_Qstr, "97000", 1, "System Admin", 1, "-", "-", "-", "-", "SYSAD", "-", "fa-group");
            add_icon(Uniq_Qstr, "97001", 2, "User Managment", 1, "../tej-base/frmUmst.aspx", "-", "-", "-", "SYSAD", "SYSADM", "-");
            add_icon(Uniq_Qstr, "97010", 2, "User Rights", 1, "../tej-base/urights.aspx", "-", "-", "-", "SYSAD", "SYSADM", "-");
        }
    }
    public string chk_tab(string Qstr, string sysid)
    {
        if (mulevel == "M") mhd = seek_iname(Qstr, Cls_comp_code, "Select id from ico_wtab where trim(id)='" + sysid.Trim() + "' and trim(userid)='" + muname + "'", "id");
        else mhd = seek_iname(Qstr, Cls_comp_code, "Select id from ico_tab where trim(id)='" + sysid.Trim() + "'", "id");
        if (mhd == "0") val = "N";
        else val = "Y";
        return val;
    }
    public DataSet fill_schema(string Qstr, string pco_CD, string tab_name)
    {
        DataSet fgen_oDS = new DataSet();
        using (OracleConnection fcon = new OracleConnection(ConnInfo.connString(pco_CD)))
        {
            fcon.Open();
            using (OracleDataAdapter fgen_da = new OracleDataAdapter(new OracleCommand("SELECT * FROM " + tab_name + " where 1=2 ", fcon)))
            {
                using (OracleCommandBuilder cb = new OracleCommandBuilder(fgen_da))
                {
                    fgen_da.FillSchema(fgen_oDS, SchemaType.Source);
                }
            }
        }
        return fgen_oDS;
    }
    public void add_icon(string Uniq_Qstr_AddIcon, string id, int lvl, string name, int ulvel, string webaction, string srch_key, string submenu, string submenuid, string form, string param, string CSS_NAME)
    {
        if (chk_tab(Uniq_Qstr_AddIcon, id).Trim() == "N")
        {
            DataSet oDS = new DataSet(); DataRow oporow = null;
            oDS = fill_schema(Uniq_Qstr_AddIcon, Cls_comp_code, "ico_tab");
            oporow = oDS.Tables[0].NewRow();
            oporow["ID"] = id;
            oporow["MLEVEL"] = lvl;
            oporow["TEXT"] = name;
            oporow["ALLOW_LEVEL"] = ulvel;
            oporow["WEB_aCTION"] = webaction;
            oporow["SEARCH_KEY"] = srch_key;
            oporow["SUBMENU"] = submenu;
            oporow["SUBMENUID"] = submenuid;
            oporow["FORM"] = form;
            oporow["PARAM"] = param;
            if (CSS_NAME.Length > 3) { }
            else CSS_NAME = "fa-edit";
            oporow["CSS"] = CSS_NAME;
            oporow["VISI"] = "Y";
            oDS.Tables[0].Rows.Add(oporow);
            save_data(Cls_comp_code, oDS, "ico_tab");
            oDS.Dispose();
        }
    }
    public void save_data(string Comp_Code, DataSet oDs, string tab_name)
    {
        using (OracleConnection fcon = new OracleConnection(ConnInfo.connString(Comp_Code)))
        {
            fcon.Open();
            using (OracleDataAdapter fgen_da = new OracleDataAdapter("select * from " + tab_name + " where 1=2", fcon))
            {
                using (OracleCommandBuilder cb = new OracleCommandBuilder(fgen_da))
                {
                    string field_type = "";
                    for (int i = 0; i < oDs.Tables[0].Rows.Count; i++)
                    {
                        for (int z = 0; z < oDs.Tables[0].Columns.Count; z++)
                        {
                            field_type = oDs.Tables[0].Columns[z].DataType.Name.ToString();
                            if (field_type.ToUpper() == "DATETIME" && oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                            else if (field_type.ToUpper() == "DECIMAL" && oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Trim().Length == 0) { }
                            else oDs.Tables[0].Rows[i][z] = oDs.Tables[0].Rows[i][z].ToString().Replace("'", "`").Replace("&nbsp;", "-").Replace("&amp;", "-").Replace(@"\", "/").Trim();
                        }
                    }
                    oDs.Tables[0].TableName = tab_name;
                    fgen_da.Update(oDs, tab_name);
                    oDs.Dispose();
                }
            }
        }
    }
    public void add_icon(string Uniq_Qstr_AddIcon, string id, int lvl, string name, int ulvel, string webaction, string srch_key, string submenu, string submenuid, string form, string param, string CSS_NAME, string askBranchPopup, string askPrdRange)
    {
        if (chk_tab(Uniq_Qstr_AddIcon, id).Trim() == "N")
        {
            DataSet oDS = new DataSet(); DataRow oporow = null;
            oDS = fill_schema(Uniq_Qstr_AddIcon, Cls_comp_code, "ico_tab");
            oporow = oDS.Tables[0].NewRow();
            oporow["ID"] = id;
            oporow["MLEVEL"] = lvl;
            oporow["TEXT"] = name;
            oporow["ALLOW_LEVEL"] = ulvel;
            oporow["WEB_aCTION"] = webaction;
            oporow["SEARCH_KEY"] = srch_key;
            oporow["SUBMENU"] = submenu;
            oporow["SUBMENUID"] = submenuid;
            oporow["FORM"] = form;
            oporow["PARAM"] = param;
            oporow["VISI"] = "Y";
            if (CSS_NAME.Length > 3) { }
            else CSS_NAME = "fa-edit";
            oporow["CSS"] = CSS_NAME;
            if (askBranchPopup == "N") oporow["BRN"] = "N";
            else oporow["BRN"] = "Y";
            if (askPrdRange == "N") oporow["PRD"] = "N";
            else oporow["PRD"] = "Y";
            oporow["VISI"] = "Y";
            oDS.Tables[0].Rows.Add(oporow);
            save_data(Cls_comp_code, oDS, "ico_tab");
            oDS.Dispose();
        }
    }
    public DataTable GetCompDetail(string userCode, string clientid_mst)
    {
        DataTable dt = new DataTable();
        string sessionname = "Compdtl_u_" + userCode + "_" + clientid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = getdata(userCode, @"select Company_Name,REPLACE(Company_Address,'$','') as Company_Address,Company_Country,Company_State,Company_City, Company_Pincode, Company_Email_Id
                                    ,Company_Website,Company_Contact_No,Company_Alternate_Contact_No,isnull(Company_gstin_No,'-') as Company_gstin_No,isnull(com_pan_no,'-') as com_pan_no,isnull(Company_Cin_No,'-') as Company_Cin_No from company_profile where company_profile_id='" + clientid_mst + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = getdata(userCode, @"select Company_Name,REPLACE(Company_Address,'$','') as Company_Address,Company_Country,Company_State,Company_City, Company_Pincode, Company_Email_Id
                                    ,Company_Website,Company_Contact_No,Company_Alternate_Contact_No,isnull(Company_gstin_No,'-') as Company_gstin_No,isnull(com_pan_no,'-') as com_pan_no,isnull(Company_Cin_No,'-') as Company_Cin_No from company_profile where company_profile_id='" + clientid_mst + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return dt.Copy();
    }
    public DataTable GetBranchDetail(string userCode, string unitid_mst, string clientid_mst)
    {

        DataTable dt = new DataTable();
        string sessionname = "branchdtl_u_" + userCode + "_" + unitid_mst + "";
        if ((HttpContext.Current.Application[sessionname] == null))
        {
            dt = getdata(userCode, "select distinct a.Unit_Name,REPLACE(a.Unit_Address,'$','') as Unit_Address,a.Unit_Country," +
                "cs.country_name,cs.state_name,a.Unit_State,a.Unit_City,(case when a.Unit_Pincode='000000' then '' else a.Unit_Pincode end) Unit_Pincode, a.Unit_Email,a.Unit_website, a.Unit_Contact_No," +
                "a.Unit_Alternate_Contact_No, isnull(a.septr, '-') as septr,isnull(a.Unit_GSTIN_No,'-') as Unit_GSTIN_No,isnull(a.bank,'-') as bank_name" +
                ",isnull(a.branch,'-') as bank_branch,isnull(a.acctno,'-') as bank_acc,isnull(a.ifsc,'-') as bank_ifsc from company_unit_profile a LEFT JOIN country_state cs on a.unit_city = cs.city_name " +
                "and a.unit_state = cs.state_gst_code and a.unit_country = cs.alpha_2 where cup_id = '" + unitid_mst + "' and company_profile_id = " +
                "'" + clientid_mst + "'");
            HttpContext.Current.Application[sessionname] = dt;
        }
        else
        {
            dt = ((DataTable)HttpContext.Current.Application[sessionname]);
            if (dt == null || dt.Rows.Count == 0)
            {
                dt = getdata(userCode, "select distinct a.Unit_Name,REPLACE(a.Unit_Address,'$','') as Unit_Address,a.Unit_Country," +
                "cs.country_name,cs.state_name,a.Unit_State,a.Unit_City,(case when a.Unit_Pincode='000000' then '' else a.Unit_Pincode end) Unit_Pincode, a.Unit_Email,a.Unit_website, a.Unit_Contact_No," +
                "a.Unit_Alternate_Contact_No, isnull(a.septr, '-') as septr,isnull(a.Unit_GSTIN_No,'-') as Unit_GSTIN_No from company_unit_profile a LEFT JOIN country_state cs on a.unit_city = cs.city_name " +
                "and a.unit_state = cs.state_gst_code and a.unit_country = cs.alpha_2 where cup_id = '" + unitid_mst + "' and company_profile_id = " +
                "'" + clientid_mst + "'");
                HttpContext.Current.Application[sessionname] = dt;
            }
        }
        return dt.Copy();

    }
    public byte[] Exp_to_csv_new(DataTable dataTable, string filename, string cg_com_name)
    {
        Random rnd = new Random();
        int r = rnd.Next(0, 100000);

        string ff = HttpContext.Current.Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/" +
            filename + DateTime.Now.ToString("yyyyMMddHmmss") + r);
        //string encpath = Convert_Stringto64(ff).ToString();
        // Write sample data to CSV file


        using (CsvFileWriter writer = new CsvFileWriter(ff))
        {
            CsvRow row = new CsvRow();

            foreach (DataColumn dc in dataTable.Columns)
                row.Add(String.Format(dc.ColumnName.ToString()));
            writer.WriteRow(row);
            foreach (DataRow dr in dataTable.Rows)
            {
                row = new CsvRow();
                foreach (DataColumn dc in dataTable.Columns)
                    row.Add(String.Format(dr[dc.ColumnName].ToString()));
                writer.WriteRow(row);
            }
        }

        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true; 
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + filename + ".csv");
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.ContentType = "application/text";

        byte[] data = System.IO.File.ReadAllBytes(ff);
        HttpContext.Current.Response.BinaryWrite(data);
        File.Delete(ff);
        HttpContext.Current.Response.End();
        return data;

    }

    public void Fn_Print_Report(string pco_Cd, string Uniq_QSTR, string mbr, string query, string xml, string report)
    {
        DataSet dsPrintRpt = new DataSet();
        using (DataTable dtPrintRpt = getdata(pco_Cd, query))
        {
            dtPrintRpt.TableName = "Prepcur";
            dsPrintRpt.Tables.Add(dtPrintRpt);
            dsPrintRpt = Get_Type_Data(Uniq_QSTR, pco_Cd, mbr, dsPrintRpt);
            string xfilepath = HttpContext.Current.Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
            string rptfile = "~/tej-base/REPORT/" + report.Trim() + ".rpt";

            dsPrintRpt.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
            HttpContext.Current.Session["RPTDATA"] = dsPrintRpt;
            SetCookie(MyGuid, "RPTFILE", rptfile);
        }
        if (dsPrintRpt.Tables[0].Rows.Count > 0)
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Frm_Report.aspx");
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "OpenSingle('" + fil_loc + "?STR=" + Uniq_QSTR + "','95%','95%','');", true);
            }
        }
        else
        {
            if (HttpContext.Current.CurrentHandler is Page)
            {
                Page p = (Page)HttpContext.Current.CurrentHandler;
                string fil_loc = System.Web.VirtualPathUtility.ToAbsolute("~/tej-base/Frm_Report.aspx");
                p.ClientScript.RegisterClientScriptBlock(this.GetType(), "PopUP", "openPrintOut('" + fil_loc + "?STR=" + Uniq_QSTR + "','420px','420px','Tejaxo Report Viewer');", true);
            }
        }
    }

    public MemoryStream exp_to_pdf(DataTable dt, string file_name)
    {
        //Random rnd = new Random();
        //int r = rnd.Next(0, 100000);
        //string ff = HttpContext.Current.Server.MapPath("~/Uploads/" + cg_com_name.Replace(" ", "") + "/" +
        //    file_name + DateTime.Now.ToString("yyyyMMddHmmss") + r);
        //iTextSharp.text.Document pdfDoc;

        iTextSharp.text.Document pdfDoc;
        MemoryStream stream = new System.IO.MemoryStream();
        GridView GridView2 = new GridView();
        GridView2.AllowPaging = false;
        GridView2.HeaderStyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#1797c0");
        GridView2.DataSource = dt;
        GridView2.DataBind();
        GridView2.HeaderRow.Style.Add("width", "10%");
        GridView2.HeaderRow.Style.Add("font-size", "9px");
        GridView2.Style.Add("text-decoration", "none");
        GridView2.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
        GridView2.Style.Add("font-size", "8px");

        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        GridView2.RenderControl(hw);
        StringReader sr = new StringReader(sw.ToString());
        HttpContext.Current.Response.ContentType = "application/pdf";
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + file_name + ".pdf");
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        if (dt.Columns.Count > 8) pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
        else pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 10f);
        iTextSharp.text.html.simpleparser.HTMLWorker htmlparser = new iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc);
        iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);
        iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, stream);
        pdfDoc.Open();
        htmlparser.Parse(sr);
        pdfDoc.Close();
        HttpContext.Current.Response.Write(pdfDoc);
        HttpContext.Current.Response.End();


        return stream;


    }

    public List<dynamic> Dt2DNS(DataTable dt)
    {
        var data = new List<dynamic>();
        foreach (var item in dt.AsEnumerable())
        {

            IDictionary<string, object> dn = new ExpandoObject();
            foreach (var column in dt.Columns.Cast<DataColumn>())
            {
                dn[column.ColumnName] = item[column];
            }

            data.Add(dn);
        }

        return data;
    }
    //public List<WebGridColumn> Dtcols2Webcols(DataTable dt)
    //{
    //    List<WebGridColumn> columns = new List<WebGridColumn>();

    //    foreach (DataColumn col in dt.Columns)
    //    {
    //        columns.Add(new WebGridColumn()
    //        {
    //            ColumnName = col.ColumnName,
    //            Header = col.ColumnName
    //        });
    //    }
    //    return columns;
    //}

    //public List<SG1> Dt2SG1(DataTable dt)
    //{
    //    List<SG1> grid = new List<SG1>();


    //    if (dt.Rows.Count > 0)
    //    {

    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            SG1 sg = new SG1();
    //            sg.sg1_h1 = dr["sg1_h1"].ToString();
    //            sg.sg1_h2 = dr["sg1_h2"].ToString();
    //            sg.sg1_h3 = dr["sg1_h3"].ToString();
    //            sg.sg1_h4 = dr["sg1_h4"].ToString();
    //            sg.sg1_h5 = dr["sg1_h5"].ToString();
    //            sg.sg1_h6 = dr["sg1_h6"].ToString();
    //            sg.sg1_h7 = dr["sg1_h7"].ToString();
    //            sg.sg1_h8 = dr["sg1_h8"].ToString();
    //            sg.sg1_h9 = dr["sg1_h9"].ToString();
    //            sg.sg1_h10 = dr["sg1_h10"].ToString();

    //            sg.sg1_SrNo = Make_int(dr["sg1_SrNo"].ToString());
    //            sg.sg1_f1 = dr["sg1_f1"].ToString();
    //            sg.sg1_f2 = dr["sg1_f2"].ToString();
    //            sg.sg1_f3 = dr["sg1_f3"].ToString();
    //            sg.sg1_f4 = dr["sg1_f4"].ToString();
    //            sg.sg1_f5 = dr["sg1_f5"].ToString();

    //            sg.sg1_t1 = dr["sg1_t1"].ToString();
    //            sg.sg1_t2 = dr["sg1_t2"].ToString();
    //            sg.sg1_t3 = dr["sg1_t3"].ToString();
    //            sg.sg1_t4 = dr["sg1_t4"].ToString();
    //            sg.sg1_t5 = dr["sg1_t5"].ToString();
    //            sg.sg1_t6 = dr["sg1_t6"].ToString();
    //            sg.sg1_t7 = dr["sg1_t7"].ToString();
    //            sg.sg1_t8 = dr["sg1_t8"].ToString();
    //            sg.sg1_t9 = dr["sg1_t9"].ToString();
    //            sg.sg1_t10 = dr["sg1_t10"].ToString();
    //            sg.sg1_t11 = dr["sg1_t11"].ToString();
    //            sg.sg1_t12 = dr["sg1_t12"].ToString();
    //            sg.sg1_t13 = dr["sg1_t13"].ToString();
    //            sg.sg1_t14 = dr["sg1_t14"].ToString();
    //            sg.sg1_t15 = dr["sg1_t15"].ToString();
    //            grid.Add(sg);
    //        }
    //    }

    //    return grid;
    //}

    //public List<SG2> Dt2SG2(DataTable dt)
    //{
    //    List<SG2> grid = new List<SG2>();


    //    if (dt.Rows.Count > 0)
    //    {

    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            SG2 sg = new SG2();

    //            sg.sg2_SrNo = Make_int(dr["sg2_SrNo"].ToString());

    //            sg.sg2_t1 = dr["sg2_t1"].ToString();
    //            sg.sg2_t2 = dr["sg2_t2"].ToString();
    //            sg.sg2_t3 = dr["sg2_t3"].ToString();
    //            sg.sg2_t4 = dr["sg2_t4"].ToString();
    //            grid.Add(sg);
    //        }
    //    }

    //    return grid;
    //}
    //public List<SG3> Dt2SG3(DataTable dt)
    //{
    //    List<SG3> grid = new List<SG3>();


    //    if (dt.Rows.Count > 0)
    //    {

    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            SG3 sg = new SG3();

    //            sg.sg3_SrNo = Make_int(dr["sg3_SrNo"].ToString());

    //            sg.sg3_t1 = dr["sg3_f1"].ToString();
    //            sg.sg3_t1 = dr["sg3_f2"].ToString();
    //            sg.sg3_t1 = dr["sg3_t1"].ToString();
    //            sg.sg3_t2 = dr["sg3_t2"].ToString();
    //            sg.sg3_t3 = dr["sg3_t3"].ToString();
    //            sg.sg3_t4 = dr["sg3_t4"].ToString();
    //            grid.Add(sg);
    //        }
    //    }

    //    return grid;
    //}
    //public List<SG4> Dt2SG4(DataTable dt)
    //{
    //    List<SG4> grid = new List<SG4>();


    //    if (dt.Rows.Count > 0)
    //    {

    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            SG4 sg = new SG4();

    //            sg.sg4_SrNo = Make_int(dr["sg4_SrNo"].ToString());


    //            sg.sg4_t1 = dr["sg4_t1"].ToString();
    //            sg.sg4_t2 = dr["sg4_t2"].ToString();
    //            sg.sg4_t3 = dr["sg4_t3"].ToString();
    //            sg.sg4_t4 = dr["sg4_t4"].ToString();
    //            grid.Add(sg);
    //        }
    //    }

    //    return grid;
    //}
    //public void open_grid(string title, string query, int seektype, String SRCHVAL = "", bool master = true)
    //{

    //    //string url = FindUrl("footable_v8.aspx") + "?m_id=" + EncryptDecrypt.Encrypt(MyGuid);
    //    //SetSession(MyGuid, "pageurl", url);
    //    //if (url.Equals("")) { showmsg(1, "Page Not Found", 0); return; }
    //    if (query.Trim().Length < 5)
    //    {
    //        showmsg(1, "Please Put Right Command", 2);
    //        SetSession(MyGuid, "basedtquery", "");
    //        return;
    //    }
    //    SetSession(MyGuid, "filename", title);
    //    SetSession(MyGuid, "basedtquery", query);
    //    SetSession(MyGuid, "SEEKLIMIT", 9999999999);
    //    SetSession(MyGuid, "SHOWSAVE", true);
    //    SetSession(MyGuid, "TEMPID", "-");
    //    SetSession(MyGuid, "SRCHVAL", SRCHVAL);
    //    if (seektype == 0) SetSession(MyGuid, "SEEKTYPE", 0);
    //    else SetSession(MyGuid, "SEEKTYPE", 2);
    //    SetSession(MyGuid, "CHECKTYPE", seektype);
    //    //if (HttpContext.Current.CurrentHandler is Page)
    //    //{
    //    //    Page p = (Page)HttpContext.Current.CurrentHandler;
    //    //    ScriptManager.RegisterClientScriptBlock(p, p.GetType(), "PopUP", "OpenSingle('../../../../../" + url + "','80%','800px','" + title + "');", true);
    //    //    //ScriptManager.RegisterClientScriptBlock(p, p.GetType(), "PopUP", "OpenSingle('../../../../../erp/dashboard.aspx?mid=YDCKmcdznzA=','90%','750px','" + title + "');", true);
    //    //}
    //}


    public void chkTab(string uniqQstr, string coCd)
    {
        // ------------------------------------------------------------------
        //General DML

        // ------------------------------------------------------------------
        //Company Wise DML
        string mhd = "";

        mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='ICO_TAB_UPD'", "tname");
        if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table ico_tab_UPD(IDNO varchar2(6) Default '-',ent_by varchar2(10) default '-',ent_Dt date default GETDATE())");

        mhd = seek_iname(uniqQstr, coCd, "select idno from ico_tab_UPD where trim(idno)='DM0001'", "idno");
        if (mhd == "0" || mhd == "")
        {
            execute_cmd(coCd, "insert into ico_tab_UPD values ('DM0001','DEV_A',GETDATE())");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_CSS_LOG'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_CSS_LOG(branchcd char(2),type char(2),CSSNO char(6),CSSDT date,CCODE char(10) default '-',EModule varchar2(30) default '-',EICON varchar2(30) default '-',REQ_TYPE varchar2(20) default '-',ISS_TYPE varchar2(20) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Priority number(2) default 0,remarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100),filename varchar2(60),ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");
            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_CSS_ASG'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_CSS_ASG(branchcd char(2),type char(2),DSRNO char(6),DSRDT date,CSSNO char(6),CSSDT date,CCODE char(10) default '-',eModule varchar2(30) default '-',EICON varchar2(50) default '-',ASG_ASYS varchar2(50) default '-',Priority number(3) default 0,ASG_DPT VARCHAR2(50),ASG_AGT VARCHAR2(50),remarks varchar2(150) default '-',CSS_STATUS varchar(50) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");
            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_CSS_ACT'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_CSS_ACT(branchcd char(2),type char(2),ACTNO char(6),ACTDT date,DSRNO char(6),DSRDT date,CSSNO char(6),CSSDT date,CCODE char(10) default '-',eModule varchar2(30) default '-',EICON varchar2(50) default '-',Priority number(2) default 0,ASG_DPT VARCHAR2(50),ASG_AGT VARCHAR2(50),remarks varchar2(150) default '-',ACT_STATUS varchar(50) default '-',ACT_DATE date default GETDATE(),srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");
            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_TYPE_MST'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_Type_Mst(branchcd char(2),id char(4),tmstno char(6),tmstdt date,type1 char(4) default '-',Name varchar2(80) default '-',typedpt varchar2(20) default '-',suppfld1 varchar2(50) default '-',suppfld2 varchar2(50) default '-',suppfld3 varchar2(50) default '-',orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");

            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_LOG", "LAST_ACTION");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_LOG ADD Last_Action VARCHAR2(50) DEFAULT '-'");
            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_LOG", "LAST_ACTDT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_LOG ADD Last_Actdt VARCHAR2(10) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_LOG", "FAPP_BY");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_LOG ADD FAPP_BY VARCHAR2(15) DEFAULT '-'");
            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_LOG", "FAPP_DT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_LOG ADD FAPP_DT date DEFAULT GETDATE()");

            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_LOG", "WORK_ACTION");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_LOG ADD WORK_Action VARCHAR2(50) DEFAULT '-'");


            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_ASG", "LAST_ACTION");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_ASG ADD Last_Action VARCHAR2(50) DEFAULT '-'");
            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_ASG", "LAST_ACTDT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_ASG ADD Last_Actdt VARCHAR2(10) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_ASG", "IMPL_STATUS");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_ASG ADD IMPL_status VARCHAR2(50) DEFAULT '-'");
            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_ASG", "TASK_COMPL");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_ASG ADD TASK_COMPL VARCHAR2(1) DEFAULT '-'");


            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_ACT", "TASK_COMPL");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_ACT ADD TASK_COMPL VARCHAR2(1) DEFAULT '-'");
            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_ACT", "NEXT_TGT_DATE");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_ACT ADD NEXT_TGT_DATE VARCHAR2(10) DEFAULT '-'");
            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_ACT", "FILEPATH");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_ACT ADD FILEPATH VARCHAR2(100) DEFAULT '-'");
            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_ACT", "FILENAME");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_ACT ADD FILENAME VARCHAR2(60) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_LOG", "DIR_COMP");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_LOG ADD DIR_COMP CHAR(1) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "WB_CSS_LOG", "WRKRMK");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CSS_LOG ADD WRKRMK CHAR(150) DEFAULT '-'");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_DSL_LOG'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_DSL_LOG(branchcd char(2),type char(2),DSLNO char(6),DSLDT date,DCODE char(10) default '-',CCODE char(10) default '-',EVertical varchar2(30) default '-',EModule varchar2(30) default '-',EICON varchar2(30) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',remarks varchar2(150) default '-',WRKRMK CHAR(150) DEFAULT '-',DIR_COMP CHAR(1) DEFAULT '-',Epurpose varchar2(20) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100),filename varchar2(60),ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_CAM_LOG'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_CAM_LOG(branchcd char(2),type char(2),CAMNO char(6),CAMDT date,TCODE char(10) default '-',CAM_type varchar2(50) default '-',CAM_spec varchar2(50) default '-',CAM_purpose varchar2(25) default '-',CAM_Durn varchar2(25) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',remarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100),filename varchar2(60),ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");

            execute_cmd(coCd, "ALTER TABLE WB_CSS_LOG modify filepath VARCHAR2(100) DEFAULT '-'");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_LEAD_LOG'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_Lead_LOG(branchcd char(2),type char(2),LRCNO char(6),LRCDT date,Lead_dsg char(20) default '-',LVertical varchar2(30) default '-',Ldescr varchar2(30) default '-',lgrade varchar2(30) default '-',lsubject varchar2(30) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Lremarks varchar2(150) default '-',Oremarks CHAR(150) DEFAULT '-',Lead_Mtg CHAR(1) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',last_Action varchar2(80) default '-',last_Actdt varchar2(10) default '-',ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");
            mhd = check_filed_name(uniqQstr, coCd, "WB_LEAD_LOG", "LEAD_CLOSE");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_lead_LOG ADD LEAD_CLOSE VARCHAR2(1) DEFAULT '-'");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_LEAD_ACT'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_Lead_ACT(branchcd char(2),type char(2),LACNO char(6),LACDT date,LRCNO char(6),LRCDT date,Lead_dsg char(20) default '-',LVertical varchar2(30) default '-',Ldescr varchar2(30) default '-',lgrade varchar2(30) default '-',lsubject varchar2(30) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Input_from varchar2(20) DEFAULT '-',Act_mode varchar2(10) DEFAULT '-',Next_Folo number(5) DEFAULT 0,Oremarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");

            mhd = check_filed_name(uniqQstr, coCd, "WB_LEAD_ACT", "CURR_STAT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_lead_ACT ADD CURR_STAT VARCHAR2(10) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "WB_LEAD_LOG", "CURR_STAT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_lead_LOG ADD CURR_STAT VARCHAR2(10) DEFAULT '-'");


            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_CCM_LOG'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_CCM_LOG(branchcd char(2),type char(2),CCMNO char(6),CCMDT date,Cust_NAME varchar2(80) default '-',comp_type varchar2(30) default '-',Cdescr varchar2(30) default '-',Compcatg varchar2(30) default '-',compOccr varchar2(30) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Lremarks varchar2(150) default '-',Oremarks CHAR(150) DEFAULT '-',CCM_Mtg CHAR(1) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',last_Action varchar2(80) default '-',last_Actdt varchar2(10) default '-',ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");

            mhd = check_filed_name(uniqQstr, coCd, "WB_CCM_LOG", "CCM_CLOSE");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CCM_LOG ADD CCM_CLOSE VARCHAR2(1) DEFAULT '-'");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_CCM_ACT'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_CCM_ACT(branchcd char(2),type char(2),CACNO char(6),CACDT date,CCMNO char(6),CCMDT date,Cust_NAME varchar2(80) default '-',comp_type varchar2(30) default '-',Cdescr varchar2(30) default '-',Compcatg varchar2(30) default '-',compOccr varchar2(30) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Lremarks varchar2(150) default '-',Input_from varchar2(20) DEFAULT '-',Act_mode varchar2(10) DEFAULT '-',Next_Folo number(5) DEFAULT 0,Oremarks CHAR(150) DEFAULT '-',CCM_Mtg CHAR(1) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',last_Action varchar2(80) default '-',last_Actdt varchar2(10) default '-',ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");

            mhd = check_filed_name(uniqQstr, coCd, "WB_CCM_ACT", "CURR_STAT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CCM_ACT ADD CURR_STAT VARCHAR2(10) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "WB_CCM_LOG", "CURR_STAT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_CCM_LOG ADD CURR_STAT VARCHAR2(10) DEFAULT '-'");
        }
        mhd = seek_iname(uniqQstr, coCd, "select idno from ico_tab_UPD where trim(idno)='DM0002'", "idno");
        if (mhd == "0" || mhd == "")
        {
            execute_cmd(coCd, "insert into ico_tab_UPD values ('DM0002','DEV_A',GETDATE())");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_STL_LOG'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_STL_LOG(branchcd char(2),type char(2),STLNO char(6),STLDT date,TCODE char(10) default '-',CCODE char(10) default '-',EVertical varchar2(30) default '-',EModule varchar2(30) default '-',EICON varchar2(30) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',remarks varchar2(150) default '-',WRKRMK CHAR(150) DEFAULT '-',DIR_COMP CHAR(1) DEFAULT '-',Epurpose varchar2(20) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100),filename varchar2(60),ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_OMS_LOG'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_OMS_LOG(branchcd char(2),type char(2),OPLNO char(6),OPLDT date,CCODE char(10) default '-',Month_Amt nUMBER(12,2) default 0,remarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_OMS_ACT'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_OMS_ACT(branchcd char(2),type char(2),OACNO char(6),OACDT date,CCODE char(10) default '-',Agree_Amt nUMBER(12,2) default 0,Agree_dt date default GETDATE(),remarks varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");

            mhd = check_filed_name(uniqQstr, coCd, "WB_OMS_LOG", "TCODE");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_OMS_LOG ADD TCODE VARCHAR2(10) DEFAULT '-'");
            mhd = check_filed_name(uniqQstr, coCd, "WB_OMS_LOG", "NARATION");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_OMS_LOG ADD NARATION VARCHAR2(200) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "WB_OMS_ACT", "TCODE");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_OMS_ACT ADD TCODE VARCHAR2(10) DEFAULT '-'");
            mhd = check_filed_name(uniqQstr, coCd, "WB_OMS_ACT", "NARATION");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_OMS_ACT ADD NARATION VARCHAR2(200) DEFAULT '-'");
            mhd = check_filed_name(uniqQstr, coCd, "WB_OMS_ACT", "ACT_MODE");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_OMS_ACT ADD ACT_MODE VARCHAR2(10) DEFAULT '-'");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_ALF_PLAN'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_ALF_PLAN(branchcd char(2),type char(2),ALFNO char(6),ALFDT date,TCODE char(10) default '-',CCODE char(10) default '-',VISIT_dT DATE default GETDATE(),remarks varchar2(100) default '-',NARATION varchar2(150) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");

        }

        mhd = seek_iname(uniqQstr, coCd, "select idno from ico_tab_UPD where trim(idno)='DM0003'", "idno");
        if (mhd == "0" || mhd == "")
        {
            execute_cmd(coCd, "insert into ico_tab_UPD values ('DM0003','DEV_A',GETDATE())");


            mhd = check_filed_name(uniqQstr, coCd, "EMPTRAIN", "DPT_CODE");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE EMPTRAIN ADD DPT_CODE VARCHAR2(10) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "EMPTRAIN", "DPT_NAME");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE EMPTRAIN ADD DPT_NAME VARCHAR2(40) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "EMPTRAIN", "TR_CODE");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE EMPTRAIN ADD TR_CODE VARCHAR2(10) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "EMPTRAIN", "TR_NAME");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE EMPTRAIN ADD TR_NAME VARCHAR2(40) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "EMPTRAIN", "EDT_BY");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE EMPTRAIN ADD EDT_BY VARCHAR2(20) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "EMPTRAIN", "EDT_DT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE EMPTRAIN ADD EDT_DT date DEFAULT GETDATE()");

            mhd = check_filed_name(uniqQstr, coCd, "EMPTRAIN", "chk_BY");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE EMPTRAIN ADD chk_BY VARCHAR2(20) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "EMPTRAIN", "chk_DT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE EMPTRAIN ADD chk_DT date DEFAULT GETDATE()");

            mhd = check_filed_name(uniqQstr, coCd, "EMPTRAIN", "app_BY");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE EMPTRAIN ADD app_BY VARCHAR2(20) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "EMPTRAIN", "app_DT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE EMPTRAIN ADD app_DT date DEFAULT GETDATE()");

            mhd = check_filed_name(uniqQstr, coCd, "EMPTRAIN", "NARATION");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE EMPTRAIN ADD NARATION VARCHAR2(150) DEFAULT '-'");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_LEV_REQ'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_LEV_REQ(branchcd char(2),type char(2),LRQNO char(6),LRQDT date,Empcode char(10) default '-',Lreason1 varchar2(30) default '-',Lreason2 varchar2(30) default '-',Levfrom varchar2(10) default '-',Levupto varchar2(10) default '-',Cont_Name varchar2(50) default '-',Cont_NO varchar2(20) default '-',Cont_EMAIL varchar2(30) default '-',Lremarks varchar2(150) default '-',Oremarks CHAR(150) DEFAULT '-',Resp_Shared CHAR(1) DEFAULT '-',srno number(4) default 0,orignalbr char(2),filepath varchar2(100) default '-',filename varchar2(60) default '-',last_Action varchar2(80) default '-',last_Actdt varchar2(10) default '-',ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");

        }

        mhd = seek_iname(uniqQstr, coCd, "select idno from ico_tab_UPD where trim(idno)='DM0004'", "idno");
        if (mhd == "0" || mhd == "")
        {
            execute_cmd(coCd, "insert into ico_tab_UPD values ('DM0004','DEV_A',GETDATE())");

            mhd = check_filed_name(uniqQstr, coCd, "WB_LEV_REQ", "CHK_BY");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_LEV_REQ ADD CHK_BY VARCHAR2(12) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "WB_LEV_REQ", "CHK_DT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE WB_LEV_REQ ADD CHK_DT date DEFAULT GETDATE()");

        }

        //updates on 26 Jan
        #region Gen_upds_26jan
        mhd = seek_iname(uniqQstr, coCd, "select idno from ico_tab_UPD where trim(idno)='DM0005'", "idno");
        if (mhd == "0" || mhd == "")
        {
            execute_cmd(coCd, "insert into ico_tab_UPD values ('DM0005','DEV_A',GETDATE())");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='ICO_TAB'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "CREATE TABLE ico_tab(ID VARCHAR2(10),MLEVEL NUMBER(1),TEXT VARCHAR2(50) default '-',ALLOW_LEVEL NUMBER(2),WEB_aCTION VARCHAR2(50) default '-',SEARCH_KEY VARCHAR2(50) default '-',submenu char(1)default 'N',submenuid char(15) default '-',form varchar2(10) default '-',param varchar2(10) default '-',imagef varchar2(50) default '-',CSS varchar2(30) default 'fa-edit')");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='ICO_WTAB'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table ico_wtab(USERID VARCHAR2(10),USERNAME VARCHAR2(30),BRANCHCD CHAR(2),ENT_BY VARCHAR2(20),ENT_DT DATE,EDT_BY VARCHAR2(20),EDT_DT DATE,ID VARCHAR2(10),MLEVEL NUMBER(1),TEXT VARCHAR2(50),ALLOW_LEVEL NUMBER(2),WEB_ACTION  VARCHAR2(50),SEARCH_KEY  vARCHAR2(50),SUBMENU  CHAR(1),SUBMENUID CHAR(15),FORM VARCHAR2(10),PARAM  VARCHAR2(10),USER_COLOR VARCHAR(10) DEFAULT '00578b',IDESC VARCHAR(50) DEFAULT '-',CSS varchar2(30) default 'fa-edit',RCAN_ADD CHAR(1) DEFAULT 'Y',RCAN_EDIT CHAR(1) DEFAULT 'Y',RCAN_DEL CHAR(1) DEFAULT 'Y')");

            mhd = check_filed_name(uniqQstr, coCd, "SYS_CONFIG", "OBJ_READONLY");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "ALTER TABLE SYS_CONFIG ADD OBJ_READONLY CHAR(1) DEFAULT 'N'");

            mhd = check_filed_name(uniqQstr, coCd, "ico_tab", "CSS");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE ico_tab ADD CSS VARCHAR2(30) DEFAULT 'fa-edit'");

            mhd = check_filed_name(uniqQstr, coCd, "ico_wtab", "CSS");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE ico_wtab ADD CSS VARCHAR2(30) DEFAULT 'fa-edit'");

            mhd = check_filed_name(uniqQstr, coCd, "SYS_CONFIG", "OBJ_FMAND");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE SYS_CONFIG ADD OBJ_FMAND VARCHAR2(1) DEFAULT 'N'");

            mhd = check_filed_name(uniqQstr, coCd, "MTHLYPLAN", "DCODE");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE MTHLYPLAN ADD DCODE VARCHAR2(2) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "PROD_SHEET", "DCODE");
            if (mhd == "0")
            {
                execute_cmd(coCd, "ALTER TABLE PROD_SHEET ADD DCODE VARCHAR2(2) ");
                execute_cmd(coCd, "ALTER TABLE PROD_SHEET modify DCODE VARCHAR2(2) default '-'");
            }

            mhd = check_filed_name(uniqQstr, coCd, "PROD_SHEET", "EDT_BY");
            if (mhd == "0")
            {
                execute_cmd(coCd, "ALTER TABLE PROD_SHEET ADD EDT_BY VARCHAR2(15) ");
                execute_cmd(coCd, "ALTER TABLE PROD_SHEET modify EDT_BY VARCHAR2(15) default '-'");
            }

            mhd = check_filed_name(uniqQstr, coCd, "PROD_SHEET", "EDT_DT");
            if (mhd == "0")
            {
                execute_cmd(coCd, "ALTER TABLE PROD_SHEET ADD EDT_DT date ");
                execute_cmd(coCd, "ALTER TABLE PROD_SHEET modify EDT_DT date default GETDATE()");
            }

            execute_cmd(coCd, "create or replace view wbvu_gate_po as (select a.branchcd,a.acode,a.ordno,a.orddt,trim(a.ERP_code) as icode,a.Prate,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,trim(a.Fstr) as PO_link from (select fstr,branchcd,ordno,orddt,trim(AcodE) as Acode,ERP_code,max(prate) as prate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||format(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate,acode,branchcd,ordno,orddt from pomas where branchcd!='DD' and type like '5%' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_date('01/04/2017','dd/MM/yyyy')  union all SELECT trim(icode)||'-'||format(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,0 as irate,acode,branchcd,ponum,podate from ivoucherp where branchcd!='DD' and type='00' and vchdate>=to_date('01/04/2017','dd/MM/yyyy') )  group by fstr,ERP_code,trim(acode),branchcd,ordno,orddt having sum(Qtyord)-sum(Soldqty)>0 ) a)");

            execute_cmd(coCd, "create or replace view wbvu_gate_RGP as (select a.branchcd,a.acode,a.vchnum,a.vchdate,trim(a.ERP_code) as icode,(a.Qtyord) as Sent_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,trim(a.Fstr) as RGP_link from (select fstr,branchcd,vchnum,vchdate,trim(AcodE) as Acode,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||format(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(format(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,acode,branchcd,vchnum,vchdate from rgpmst where branchcd!='DD' and type like '2%' and trim(type)!='22' and vchdate>=to_date('01/04/2017','dd/MM/yyyy')  union all SELECT trim(icode)||'-'||format(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqty_chl as qtyord,acode,branchcd,rgpnum,rgpdate from ivoucherp where branchcd!='DD' and type='00' and vchdate>=to_date('01/04/2017','dd/MM/yyyy') and prnum='RG' )  group by fstr,ERP_code,trim(acode),branchcd,vchnum,vchdate having sum(Qtyord)-sum(Soldqty)>0 ) a)");

            execute_cmd(coCd, "create or replace view wbvu_mrr_po as (select a.branchcd,a.acode,a.ordno,a.orddt,trim(a.ERP_code) as icode,a.Prate,(a.Qtyord) as Ord_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,trim(a.Fstr) as PO_link from (select fstr,branchcd,ordno,orddt,trim(AcodE) as Acode,ERP_code,max(prate) as prate,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||format(orddt,'YYYYMMDD')||'-'||ordno||'-'||lpad(trim(cscode),4,'0') as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,((prate*(100-pdisc)/100)-pdiscamt) as prate,acode,branchcd,ordno,orddt from pomas where branchcd!='DD' and type like '5%' and trim(pflag)!=1 and (trim(chk_by)!='-' or trim(app_by)!='-') and orddt>=to_date('01/04/2017','dd/MM/yyyy')  union all SELECT trim(icode)||'-'||format(podate,'YYYYMMDD')||'-'||ponum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyin+isnull(rej_rw,0) as qtyord,0 as irate,acode,branchcd,ponum,podate from ivoucher where branchcd!='DD' and type like '0%' and vchdate>=to_date('01/04/2017','dd/MM/yyyy') )  group by fstr,ERP_code,trim(acode),branchcd,ordno,orddt having sum(Qtyord)-sum(Soldqty)>0 ) a)");

            execute_cmd(coCd, "create or replace view wbvu_mrr_RGP as (select a.branchcd,a.acode,a.vchnum,a.vchdate,trim(a.ERP_code) as icode,(a.Qtyord) as Sent_Qty,(a.Soldqty) as Rcv_Qty,(a.Qtyord)-(a.Soldqty) as Bal_Qty,trim(a.Fstr) as RGP_link from (select fstr,branchcd,vchnum,vchdate,trim(AcodE) as Acode,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty  from (SELECT trim(icode)||'-'||format(vchdate,'YYYYMMDD')||'-'||vchnum||'-'||lpad(trim(format(type,'9999')),4,'0') as fstr,trim(Icode) as ERP_code,iqtyout as Qtyord,0 as Soldqty,acode,branchcd,vchnum,vchdate from rgpmst where branchcd!='DD' and type like '2%' and trim(type)!='22' and vchdate>=to_date('01/04/2017','dd/MM/yyyy')  union all SELECT trim(icode)||'-'||format(rgpdate,'YYYYMMDD')||'-'||rgpnum||'-'||trim(ordlineno) as fstr,trim(Icode) as ERP_code,0 as Qtyord,iqtyin+isnull(rej_rw,0) as qtyord,acode,branchcd,rgpnum,rgpdate from ivoucher where branchcd!='DD' and type in ('09','0J') and vchdate>=to_date('01/04/2017','dd/MM/yyyy') )  group by fstr,ERP_code,trim(acode),branchcd,vchnum,vchdate having sum(Qtyord)-sum(Soldqty)>0 ) a)");

            mhd = "create or replace view wbvu_PR_4PO as (select branchcd,fstr,ERP_code,sum(Qtyord) as Qtyord,sum(Soldqty) as Soldqty,max(bank) as Deptt,max(delv_item) As delv_item,max(desc_) as desc_ from (SELECT format(orddt,'YYYYMMDD')||'-'||ordno||'-'||trim(Icode) as fstr,trim(Icode) as ERP_code,Qtyord,0 as Soldqty,upper(isnull(bank,'-')) As bank,isnull(delv_item,'-') As delv_item,isnull(desc_,'-') as desc_,branchcd from pomas where branchcd!='DD' and type='60' and trim(pflag)!=0 and trim(app_by)!='-' and orddt>=to_date('01/04/2017','dd/MM/yyyy') union all SELECT format(pr_Dt,'YYYYMMDD')||'-'||pr_no||'-'||trim(Icode) as fstr,trim(Icode) as ERP_code,0 as Qtyord,qtyord,null as bank,null as delv_item,null as desc_,branchcd from pomas where branchcd!='DD' and type like '5%' and orddt>=to_date('01/04/2017','dd/MM/yyyy'))  group by branchcd,fstr,ERP_code having sum(Qtyord)-sum(Soldqty)>0 )  ";
            execute_cmd(coCd, mhd);

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where upper(tname)=upper('WB_MAIL_MGR')", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_mail_mgr(branchcd char(2),type char(2),vchnum char(6),vchdate date,RCODE char(10) default '-',ECODE char(10) default '-',Mail_Freq nUMBER(8,2) default 0,Mail_Sent_Dt varchar2(10) default '-',remarks varchar2(50) default '-',naration varchar2(100) default '-',srno number(4) default 0,orignalbr char(2),ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='UDF_DATA'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table UDF_DATA(branchcd char(2),PAR_TBL varchar2(30) default '-',PAR_FLD varchar2(30) default '-',udf_name varchar2(30) default '-',udf_value varchar2(100) default '-',srno number(4) default 0)");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='WB_ISS_REQ'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table WB_ISS_REQ(branchcd char(2),type char(2),vchnum char(6),vchdate date,ACODE char(10) default '-',Stage char(10) default '-',ICODE char(10) default '-',no_bdls char(10) default '-',desc_ varchar2(100) default '-',naration varchar2(100) default '-',req_qty number(12,3) default 0,req_wt number(12,3) default 0,jobno varchar2(10) default '-',jobdt date default GETDATE(),morder number(4) default 0,orignalbr char(2),closed varchar2(1) default '-',ent_by varchar2(15) default '-',ent_Dt date default GETDATE(),edt_by varchar2(15) default '-',edt_Dt date default GETDATE(),app_by varchar2(15) default '-',app_Dt date default GETDATE())");

            mhd = check_filed_name(uniqQstr, coCd, "TYPEGRP", "VCHNUM");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE typegrp ADD VCHNUM VARCHAR2(6) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "TYPEGRP", "EDT_BY");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE typegrp ADD EDT_BY VARCHAR2(12) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "TYPEGRP", "EDT_DT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE typegrp ADD EDT_DT date DEFAULT GETDATE()");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='FIN_RSYS_OPT'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table FIN_RSYS_OPT(branchcd char(2),type char(2),vchnum char(6),vchdate date default GETDATE(),OPT_ID varchar2(6) Default '-',OPT_TEXT varchar2(60) default '-',OPT_ENABLE varchar2(1) default '-',OPT_PARAM varchar2(20) default '-',OPT_PARAM2 varchar2(20) default '-',OPT_EXCL varchar2(20) default '-',ent_by varchar2(10) default '-',ent_Dt date default GETDATE(),edt_by varchar2(10) default '-',edt_Dt date default GETDATE())");

            mhd = seek_iname(uniqQstr, coCd, "select tname from tab where tname='SOMASI'", "tname");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "create table SOMASI as (select * From somas where 1=2)");
        }
        #endregion

        //updates on 18 feb
        #region Gen_upds_18feb
        mhd = seek_iname(uniqQstr, coCd, "select idno from ico_tab_UPD where trim(idno)='DM0006'", "idno");
        if (mhd == "0" || mhd == "")
        {
            execute_cmd(coCd, "insert into ico_tab_UPD values ('DM0006','DEV_A',GETDATE())");

            mhd = check_filed_name(uniqQstr, coCd, "TYPE", "TVCHNUM");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE TYPE ADD TVCHNUM VARCHAR2(6) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "TYPE", "TVCHDATE");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE type ADD TVCHDATE date DEFAULT GETDATE()");

            mhd = check_filed_name(uniqQstr, coCd, "TYPE", "MENT_BY");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE TYPE ADD MENT_BY VARCHAR2(15) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "TYPE", "MENT_DT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE type ADD MENT_DT date DEFAULT GETDATE()");

            mhd = check_filed_name(uniqQstr, coCd, "TYPE", "MEDT_BY");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE TYPE ADD MEDT_BY VARCHAR2(15) DEFAULT '-'");

            mhd = check_filed_name(uniqQstr, coCd, "TYPE", "MEDT_DT");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE type ADD MEDT_DT date DEFAULT GETDATE()");


            mhd = check_filed_name(uniqQstr, coCd, "SYS_CONFIG", "OBJ_READONLY");
            if (mhd == "0" || mhd == "") execute_cmd(coCd, "ALTER TABLE SYS_CONFIG ADD OBJ_READONLY CHAR(1) DEFAULT 'N'");
            mhd = check_filed_name(uniqQstr, coCd, "ico_tab", "CSS");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE ico_tab ADD CSS VARCHAR2(30) DEFAULT 'fa-edit'");
            mhd = check_filed_name(uniqQstr, coCd, "ico_wtab", "CSS");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE ico_wtab ADD CSS VARCHAR2(30) DEFAULT 'fa-edit'");

            mhd = check_filed_name(uniqQstr, coCd, "ico_tab", "VISI");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE ico_tab ADD VISI CHAR(1) DEFAULT 'Y'");
            mhd = check_filed_name(uniqQstr, coCd, "ico_wtab", "VISI");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE ico_wtab ADD VISI CHAR(1) DEFAULT 'Y'");

            execute_cmd(coCd, "ALTER TABLE FIN_RSYS_OPT modify opt_TExt varchar2(100) default '-'");

            execute_cmd(coCd, "ALTER TABLE ico_tab modify PARAM varchar2(30) default '-'");
            execute_cmd(coCd, "alter table DBD_TV_CONFIG modify OBJ_READONLY VARCHAR2(10) default '-'");

            mhd = check_filed_name(uniqQstr, coCd, "DBD_TV_CONFIG", "FRM_NAME");
            if (mhd == "0") execute_cmd(coCd, "ALTER TABLE DBD_TV_CONFIG ADD FRM_NAME VARCHAR2(50) DEFAULT '-'");



        }
        #endregion

        //shuru
        mhd = check_filed_name(uniqQstr, coCd, "ITEM", "DEAC_DT");
        if (mhd == "0") execute_cmd(coCd, "ALTER TABLE ITEM ADD DEAC_DT VARCHAR2(10) ");
        execute_cmd(coCd, "ALTER TABLE ITEM modify DEAC_DT VARCHAR2(10) default '-' ");

        // to update ico_tab 
        //execute_cmd( coCd, "update ico_tab set prd='N' where id='F15133'");
        //mhd = "update ico_tab set web_Action='../tej-base/om_view_sys.aspx' where id in ('F99126','F99127','F99128','F99129')";
        //execute_cmd( coCd, mhd);

    }
    public int ChkDate(string cdate)
    {
        //if (cdate.Length <= 0 || cdate == null) return 0;
        //string datestr = Convert.ToDateTime(cdate).ToString("dd/MM/yyyy");
        string format = "dd/MM/yyyy";
        //DateTime dateValue;
        //if (DateTime.TryParseExact(datestr, format, new CultureInfo("en-GB"), DateTimeStyles.None, out dateValue)) return 1;
        //else return 0;

        return IsDate(cdate, format) == true ? 1 : 0;
    }
    public void save_info(string Qstr, string pco_Cd, string mbr, string zvnum, string zvdate, string zuser, string ztype, string zremark)
    {
        using (DataSet fgen_oDS = fill_schema(Qstr, pco_Cd, "fininfo"))
        {
            DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
            fgen_oporow["BRANCHCD"] = mbr;
            fgen_oporow["TYPE"] = ztype;
            fgen_oporow["VCHNUM"] = zvnum;
            fgen_oporow["VCHDATE"] = zvdate;
            fgen_oporow["ENT_BY"] = zuser;
            fgen_oporow["ENT_DT"] = System.DateTime.Now;
            fgen_oporow["fcomment"] = zremark;
            mq0 = GetIpAddress().ToString().ToUpper() + " ," + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
            if (mq0.Length > 29) mq0 = mq0.Substring(0, 29);
            fgen_oporow["terminal"] = mq0;
            fgen_oporow["Iremarks"] = zremark;
            fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
            save_data(pco_Cd, fgen_oDS, "fininfo");
        }
    }
    public void prnt_QRbar(string pco_Cd, string bar_val, string img_name)
    {
        int s = 5;
        if (pco_Cd == "YTEC" || pco_Cd == "MANU") s = 3;
        if (pco_Cd == "ADWA" || pco_Cd == "PPAP") s = 5;
        System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();

        QRCodeEncoder encoder = new QRCodeEncoder();
        encoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
        encoder.QRCodeVersion = 0;
        encoder.QRCodeScale = s;
        Bitmap img = encoder.Encode(bar_val, System.Text.Encoding.UTF8);
        img.Save(HttpContext.Current.Server.MapPath(@"~\BarCode\" + img_name + ""), System.Drawing.Imaging.ImageFormat.Jpeg);
    }
    public DataTable addBarCode(DataTable mainDataTable, string valueforBarcodeFromTable, bool QR)
    {
        mainDataTable.Columns.Add("BarCodeDesc", typeof(string));
        mainDataTable.Columns.Add("BarCode", typeof(System.Byte[]));
        string fpath = "";
        string bValue = "";
        foreach (DataRow dr in mainDataTable.Rows)
        {
            bValue = dr[valueforBarcodeFromTable].ToString().Trim();
            fpath = HttpContext.Current.Server.MapPath(@"~\BarCode\" + bValue.Replace("*", "").Replace("/", "") + ".png");
            del_file(fpath);
            if (QR == true) prnt_QRbar("", bValue, bValue.Replace("*", "").Replace("/", "") + ".png");

            FileStream FilStr = new FileStream(fpath, FileMode.Open);
            BinaryReader BinRed = new BinaryReader(FilStr);

            dr["BarCodeDesc"] = bValue;
            dr["BarCode"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);

            FilStr.Close();
            BinRed.Close();
        }

        return mainDataTable;
    }
    public DataTable GetYearDetails(string _qstr, string comp, string year)
    {
        DataTable dtGetDetails = new DataTable();
        dtGetDetails = getdata(comp, "select code,to_char(fmdate,'yyyy')||'-'||to_char(todate,'yyyy') as fstr,to_char(fmdate,'dd/mm/yyyy') as cdt1,to_char(todate,'dd/mm/yyyy') as cdt2,branch from co where trim(code)='" + comp + year + "'");
        return dtGetDetails;
    }
    public string exp_to_xls(DataTable dt, string file_name)
    {

        GridView GridView2 = new GridView();
        GridView2.DataSource = dt;
        GridView2.DataBind();
        //GridView2.HeaderRow.Style.Add("color", "Red");
        GridView2.HeaderRow.Style.Add("width", "10%");
        GridView2.HeaderRow.Style.Add("font-size", "11px");
        GridView2.HeaderRow.Style.Add("font-weight", "800");
        GridView2.Style.Add("text-decoration", "none");
        GridView2.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
        GridView2.Style.Add("font-size", "10px");
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + file_name + ".xls");
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        string style = @"<style> .textmode { } </style>";
        HttpContext.Current.Response.Write(style);
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        GridView2.AllowPaging = false;
        GridView2.RenderControl(hw);
        HttpContext.Current.Response.Output.Write(sw.ToString());
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
        dt.Dispose();
        return sw.ToString();
    }
    public string seekval_dt(DataTable seek_dt, string conditions, string col1)
    {
        string result = "0";
        DataRow[] rows = seek_dt.Select(conditions, "", System.Data.DataViewRowState.CurrentRows);
        if (rows.Length == 0) result = "0";
        else
        {
            result = rows[0][col1].ToString().Trim().ToUpper();
        }
        return result;
    }
    public DataTable seekval_dt(DataTable seek_dt, string conditions)
    {
        string result = "0";
        DataRow[] rows = seek_dt.Select(conditions, "", System.Data.DataViewRowState.CurrentRows);
        return rows.CopyToDataTable();
    }
    public int seekval_dt_rowindex(DataTable seek_dt, string conditions)
    {
        int result = 0;
        DataRow[] rows = seek_dt.Select(conditions, "", System.Data.DataViewRowState.CurrentRows);
        if (rows.Length == 0) result = 0;
        else
        {
            result = seek_dt.Rows.IndexOf(rows[0]) + 1;
        }
        return result;
    }

    public void del_file(string Full_Path_to_del)
    {
        try
        {
            if (System.IO.File.Exists(Full_Path_to_del)) System.IO.File.Delete(Full_Path_to_del);
        }
        catch { }
    }
    public string getNumericOnly(string valueToConvert)
    {
        string output = System.Text.RegularExpressions.Regex.Replace(valueToConvert, "[^0-9]+", string.Empty);
        return output;
    }
    public string GetIpAddress()
    {
        string ip = "";
        IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress[] addr = ipEntry.AddressList;
        ip = addr[1].ToString();
        ip = ipEntry.HostName.ToString().Trim();
        return ip;
    }
    //public Select2PagedResult ItemsToSelect2Format(List<Item> Items, int totalAttendees)
    //{
    //    Select2PagedResult jsonAttendees = new Select2PagedResult();
    //    jsonAttendees.Results = new List<Select2Result>();

    //    //Loop through our attendees and translate it into a text value and an id for the select list
    //    foreach (Item a in Items)
    //    {
    //        jsonAttendees.Results.Add(new Select2Result { id = a.Icode.ToString() + "!~!" + a.hsn + "!~!" + a.uom + "!~!" + a.stock, text = a.Iname });
    //    }
    //    //Set the total count of the results from the query.
    //    jsonAttendees.Total = totalAttendees;

    //    return jsonAttendees;
    //}
    //public DataTable settable(string source)
    //{
    //    var dt = new DataTable();
    //    #region source
    //    try
    //    {

    //        bool hascols = false;

    //        JArray json = (JArray)JsonConvert.DeserializeObject(source);
    //        var rows = json;
    //        foreach (var row in rows)
    //        {
    //            List<Dictionary<string, object>> myList = new List<Dictionary<string, object>>();
    //            var objects = ((JArray)row);
    //            foreach (var obj in objects)
    //            {
    //                var c1 = ((JObject)obj);
    //                var name = ((JProperty)c1.First).Name;
    //                var value = ((JProperty)c1.First).Value;
    //                Dictionary<string, object> dic = new Dictionary<string, object>();
    //                dic.Add(name, value);
    //                myList.Add(dic);
    //            }
    //            if (!hascols)
    //            {
    //                for (int i = 0; i < myList.Count; i++)
    //                {
    //                    dt.Columns.Add(myList[i].Keys.FirstOrDefault());
    //                }
    //                hascols = true;
    //            }
    //            DataRow dataRow = dt.NewRow();
    //            foreach (Dictionary<string, object> dictionary in myList)
    //            {
    //                foreach (string column in dictionary.Keys)
    //                {
    //                    dataRow[column] = dictionary[column];
    //                }
    //            }
    //            //if (!dataRow["lat"].ToString().Trim().Equals(""))
    //            //{
    //            dt.Rows.Add(dataRow);
    //            //}
    //        }


    //    }
    //    catch (Exception err) { }
    //    #endregion
    //    return dt;

    //}
    //public DataSet setDS(string mainsource)
    //{
    //    DataSet ds = new DataSet();
    //    string[] tabs = Regex.Split(mainsource, "!~!~!~!");
    //    foreach (var source in tabs)
    //    {
    //        var dt = new DataTable();
    //        #region source
    //        try
    //        {

    //            bool hascols = false;

    //            JArray json = (JArray)JsonConvert.DeserializeObject(source);
    //            var rows = json;
    //            foreach (var row in rows)
    //            {
    //                List<Dictionary<string, object>> myList = new List<Dictionary<string, object>>();
    //                var objects = ((JArray)row);
    //                foreach (var obj in objects)
    //                {
    //                    var c1 = ((JObject)obj);
    //                    var name = ((JProperty)c1.First).Name;
    //                    var value = ((JProperty)c1.First).Value;
    //                    Dictionary<string, object> dic = new Dictionary<string, object>();
    //                    dic.Add(name, value);
    //                    myList.Add(dic);
    //                }
    //                if (!hascols)
    //                {
    //                    for (int i = 0; i < myList.Count; i++)
    //                    {
    //                        dt.Columns.Add(myList[i].Keys.FirstOrDefault());
    //                    }
    //                    hascols = true;
    //                }
    //                DataRow dataRow = dt.NewRow();
    //                foreach (Dictionary<string, object> dictionary in myList)
    //                {
    //                    foreach (string column in dictionary.Keys)
    //                    {
    //                        dataRow[column] = dictionary[column];
    //                    }
    //                }
    //                //if (!dataRow["lat"].ToString().Trim().Equals(""))
    //                //{
    //                dt.Rows.Add(dataRow);
    //                //}
    //            }


    //        }
    //        catch (Exception err) { }
    //        #endregion
    //        ds.Tables.Add(dt);
    //    }
    //    return ds;
    //}

    public string imgtobase64(string imgpath)
    {
        string base64String = "";
        using (System.Drawing.Image image = System.Drawing.Image.FromFile(imgpath))
        {
            using (MemoryStream m = new MemoryStream())
            {
                image.Save(m, image.RawFormat);
                byte[] imageBytes = m.ToArray();

                // Convert byte[] to Base64 String
                base64String = Convert.ToBase64String(imageBytes);

            }
        }
        return base64String;
    }
    public DataTable RemoveEmptyRowsFromDataTable(DataTable dt)
    {
        for (int i = dt.Rows.Count - 1; i >= 0; i--)
        {
            if (dt.Rows[i][1] == DBNull.Value)
                dt.Rows[i].Delete();
        }
        dt.AcceptChanges();
        return dt;
    }
    public string track_save(string compCode, string Uniq_QSTR, string Action, string Type, string Uname, string Pwd, string nPwd)
    {
        try
        {
            string vnum = next_no(Uniq_QSTR, compCode, "select max(vchnum) as vchnum from log_track where type='" + Type + "'", 6, "vchnum");
            string terminal = seek_iname(Uniq_QSTR, compCode, "select userenv('terminal')||' ,'||GETDATE()||' '||format(GETDATE(),'HH:mm:ss tt') as cSource from dual", "cSource");
            using (DataSet fgen_oDS = fill_schema(Uniq_QSTR, compCode, "log_track"))
            {
                DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
                fgen_oporow["BRANCHCD"] = "00";
                fgen_oporow["TYPE"] = Type;
                fgen_oporow["VCHNUM"] = vnum;
                fgen_oporow["VCHDATE"] = DateTime.Now.ToString("dd/MM/yyyy");
                fgen_oporow["FCOMMENT"] = Action;
                fgen_oporow["ENT_BY"] = Uname;
                fgen_oporow["ENT_DT"] = System.DateTime.Now;
                fgen_oporow["OPASS"] = Pwd;
                fgen_oporow["NPASS"] = nPwd;
                mq0 = GetIpAddress().ToString().ToUpper() + " ," + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                if (mq0.Length > 29) mq0 = mq0.Substring(0, 29);
                fgen_oporow["terminal"] = mq0 + " " + terminal;
                fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
                save_data(compCode, fgen_oDS, "log_track");
            }
        }
        catch (Exception ex) { FILL_ERR("In Log Track Saving :=> " + ex.Message.ToString().Trim()); }
        return "";
    }
    public void vSave(string Uniq_QSTR, string compCode, string branchcd, string voucherType, string voucherNo, DateTime voucherDt, int voucherSrno,
    string vAcode, string vRcode, double dramt, double cramt, string voucherInvno, DateTime voucherInvDate, string voucherNaration, double voucherFcrate, double voucherFcrate1,
    double voucherTfcr, double voucherTfcdr, double voucherTfccr, string voucherRefnum, DateTime voucherRefdt, string voucherEntBy, DateTime voucherEntDt, string voucherTax, double voucherStax, double qty, string gstVch_no, string tbl_name)
    {
        using (DataSet fgen_oDS = fill_schema(Uniq_QSTR, compCode, tbl_name))
        {
            DataRow oporow = fgen_oDS.Tables[0].NewRow();
            oporow["branchcd"] = branchcd;
            oporow["DEPCD"] = branchcd;

            oporow["type"] = voucherType;
            oporow["vchnum"] = voucherNo;
            oporow["vchdate"] = voucherDt;
            oporow["srno"] = voucherSrno;
            oporow["acode"] = vAcode;
            oporow["rcode"] = vRcode;
            oporow["dramt"] = dramt;
            oporow["cramt"] = cramt;
            oporow["invno"] = voucherInvno;
            oporow["invdate"] = voucherInvDate;
            oporow["naration"] = voucherNaration;
            oporow["fcrate"] = voucherFcrate;
            oporow["fcrate1"] = voucherFcrate;
            oporow["tfcr"] = voucherTfcr;
            oporow["tfcdr"] = voucherTfcdr;
            oporow["tfccr"] = voucherTfccr;
            oporow["refnum"] = voucherRefnum;
            oporow["refdate"] = voucherRefdt;
            oporow["st_entform"] = "-";
            oporow["quantity"] = qty;

            oporow["tax"] = voucherTax;
            oporow["stax"] = voucherStax;

            oporow["ent_by"] = voucherEntBy;
            oporow["ent_date"] = voucherEntDt;
            oporow["edt_by"] = "-";
            oporow["edt_date"] = voucherEntDt;
            oporow["GSTVCH_NO"] = gstVch_no;
            fgen_oDS.Tables[0].Rows.Add(oporow);
            save_data(compCode, fgen_oDS, tbl_name);
        }
    }

    public void vSave(string Uniq_QSTR, string compCode, string branchcd, string voucherType, string voucherNo, DateTime voucherDt, int voucherSrno,
    string vAcode, string vRcode, double dramt, double cramt, string voucherInvno, DateTime voucherInvDate, string voucherNaration, double voucherFcrate, double voucherFcrate1,
    double voucherTfcr, double voucherTfcdr, double voucherTfccr, string voucherRefnum, DateTime voucherRefdt, string voucherEntBy, DateTime voucherEntDt, string voucherTax, double voucherStax, double qty, string gstVch_no, string app_by, DateTime app_dt, string vari_vch, string tbl_name)
    {
        using (DataSet fgen_oDS = fill_schema(Uniq_QSTR, compCode, tbl_name))
        {
            DataRow oporow = fgen_oDS.Tables[0].NewRow();
            oporow["branchcd"] = branchcd;
            oporow["DEPCD"] = branchcd;

            oporow["type"] = voucherType;
            oporow["vchnum"] = voucherNo;
            oporow["vchdate"] = voucherDt;
            oporow["srno"] = voucherSrno;
            oporow["acode"] = vAcode;
            oporow["rcode"] = vRcode;
            oporow["dramt"] = dramt;
            oporow["cramt"] = cramt;
            oporow["invno"] = voucherInvno;
            oporow["invdate"] = voucherInvDate;
            oporow["naration"] = voucherNaration;
            oporow["fcrate"] = voucherFcrate;
            oporow["fcrate1"] = voucherFcrate;
            oporow["tfcr"] = voucherTfcr;
            oporow["tfcdr"] = voucherTfcdr;
            oporow["tfccr"] = voucherTfccr;
            oporow["refnum"] = voucherRefnum;
            oporow["refdate"] = voucherRefdt;
            oporow["st_entform"] = "-";
            oporow["quantity"] = qty;

            oporow["tax"] = voucherTax;
            oporow["stax"] = voucherStax;

            oporow["ent_by"] = voucherEntBy;
            oporow["ent_date"] = voucherEntDt;
            oporow["edt_by"] = "-";
            oporow["edt_date"] = voucherEntDt;

            oporow["app_by"] = app_by;
            oporow["app_date"] = app_dt;

            if (vari_vch == "Y") oporow["pflag"] = "V";
            else oporow["pflag"] = "-";

            oporow["GSTVCH_NO"] = gstVch_no;
            fgen_oDS.Tables[0].Rows.Add(oporow);
            save_data(compCode, fgen_oDS, tbl_name);
        }
    }
    public string exp_to_word(DataTable dt, string file_name)
    {
        GridView GridView2 = new GridView();
        GridView2.DataSource = dt;
        GridView2.DataBind();
        GridView2.HeaderRow.Style.Add("width", "10%");
        GridView2.HeaderRow.Style.Add("font-size", "11px");
        GridView2.Style.Add("text-decoration", "none");
        GridView2.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
        GridView2.Style.Add("font-size", "10px");
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + file_name + ".doc");
        HttpContext.Current.Response.Charset = "";
        HttpContext.Current.Response.ContentType = "application/vnd.ms-word ";
        StringWriter sw = new StringWriter();
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        GridView2.AllowPaging = false;
        GridView2.RenderControl(hw);
        HttpContext.Current.Response.Output.Write(sw.ToString());
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
        dt.Dispose();
        return sw.ToString();
    }
    public string genNo(string userCode, string Query, int limit, string field)
    {
        Int64 i = 0;
        string count = "", result = "";
        DataTable dtSgen = new DataTable();
        dtSgen = getdata(userCode, Query + " union all select 1 from dual");
        if (dtSgen.Rows.Count <= 2)
        {
            count = Make_int(dtSgen.Rows[0]["" + field + ""].ToString().Trim()).ToString();
        }
        else if (dtSgen.Rows.Count == 1) count = "0";
        else count = "";
        if (count.Trim() == "0") i = 1;
        else if (count.Trim() == "") { return result; }
        else
        {
            try
            {
                i = Convert.ToInt64(count);
            }
            catch { }
            i++;
        }
        result = padlc(i, limit);
        dtSgen.Dispose();
        return result;
    }

    public string genNo(string userCode, string Query, int limit, string field, int min)
    {
        Int64 i = 0;
        string count = "", result = "";
        DataTable dtSgen = new DataTable();
        dtSgen = getdata(userCode, Query);
        if (dtSgen.Rows.Count > 0) count = dtSgen.Rows[0]["" + field + ""].ToString().Trim();
        else count = "0";
        if (count.Trim() == "") i = 0;
        else
        {
            try
            {
                i = Convert.ToInt64(count);
            }
            catch { }
            i++;
        }
        //if (i < min) i = min;
        result = min.ToString() + i.ToString();
        //result = padlc(i, limit);
        dtSgen.Dispose();
        return result;
    }
    //public List<SelectListItem> dt_to_selectlist(DataTable dt)
    //{
    //    List<SelectListItem> mod1 = new List<SelectListItem>();

    //    if (dt.Rows.Count > 0)
    //    {
    //        if (dt.Columns.Count == 1)
    //        {
    //            foreach (DataRow dr in dt.Rows)
    //            {
    //                mod1.Add(new SelectListItem { Text = dr[0].ToString(), Value = dr[0].ToString() });
    //            }
    //        }
    //        else
    //        {
    //            foreach (DataRow dr in dt.Rows)
    //            {
    //                mod1.Add(new SelectListItem { Text = dr[1].ToString(), Value = dr[0].ToString() });
    //            }
    //        }
    //    }

    //    return mod1;
    //}

    public DataTable searchDataTable(string searchText, DataTable input)
    {
        if (searchText == null) searchText = "";
        DataTable output = input.Clone();
        foreach (DataColumn dc in input.Columns)
        {
            if (dc.ColumnName.ToUpper().Contains(searchText.ToUpper())) return input;
        }
        foreach (DataRow dr in input.Rows)
        {
            for (int i = 0; i < input.Columns.Count; i++)
            {
                if (dr[i].ToString().ToUpper().Contains(searchText.ToUpper()))
                {
                    DataRow drnew = output.NewRow();
                    drnew.ItemArray = dr.ItemArray;
                    output.Rows.Add(drnew);
                    break;
                }
            }
        }
        return output;
    }
    public DataTable searchDataTable(DataTable input, string colname, string value, string ftype)
    {
        if (colname == null) value = "";
        DataTable output = input.Clone();

        foreach (DataRow dr in input.Rows)
        {

            if (ftype.ToUpper().Equals("CONTAINS"))
            {
                if (dr[colname].ToString().ToUpper().Contains(value.ToUpper()))
                {
                    DataRow drnew = output.NewRow();
                    drnew.ItemArray = dr.ItemArray;
                    output.Rows.Add(drnew);

                }
            }
            else if (ftype.ToUpper().Equals("EQUALS TO"))
            {
                if (dr[colname].ToString().ToUpper().Equals(value.ToUpper()))
                {
                    DataRow drnew = output.NewRow();
                    drnew.ItemArray = dr.ItemArray;
                    output.Rows.Add(drnew);

                }
            }
            else if (ftype.ToUpper().StartsWith("SMALLER"))
            {
                if (Make_decimal(dr[colname].ToString()) < Make_decimal(value.ToUpper()))
                {
                    DataRow drnew = output.NewRow();
                    drnew.ItemArray = dr.ItemArray;
                    output.Rows.Add(drnew);

                }
            }
            else if (ftype.ToUpper().StartsWith("GREATER"))
            {
                if (Make_decimal(dr[colname].ToString()) > Make_decimal(value.ToUpper()))
                {
                    DataRow drnew = output.NewRow();
                    drnew.ItemArray = dr.ItemArray;
                    output.Rows.Add(drnew);

                }
            }

        }
        return output;
    }
    public DataTable mTitle(DataTable dataTable, int repCount)
    {
        string mtitle = "";
        DataTable dtN = new DataTable();
        if (dataTable.Rows.Count > 0)
        {
            if (!dataTable.Columns.Contains("MTITLE")) dataTable.Columns.Add("MTITLE", typeof(string));
            if (!dataTable.Columns.Contains("MTITLESRNO")) dataTable.Columns.Add("MTITLESRNO", typeof(Int32));
        }
        dtN = dataTable.Clone();
        for (int j = 0; j < repCount; j++)
        {
            foreach (DataRow dr in dataTable.Rows)
            {
                if (j == 0) mtitle = "Original for Recipients             ";
                if (j == 1) mtitle = "Duplicate for Transporter";
                if (j == 2) mtitle = "Triplicate for Aseessee";
                if (j == 3) mtitle = "Gate Copy";
                if (j == 4) mtitle = "Extra Copy";

                dr["mTITLE"] = mtitle;
                dr["MTITLESRNO"] = j;
            }
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dtN.ImportRow(dataTable.Rows[i]);
            }
        }
        dtN.TableName = dataTable.TableName.ToString();
        return dtN;
    }
    public DataTable mTitle2(DataTable dataTable, int repCount)
    {
        string mtitle = "";
        DataTable dtN = new DataTable();
        if (dataTable.Rows.Count > 0)
        {
            if (!dataTable.Columns.Contains("MTITLE")) dataTable.Columns.Add("MTITLE", typeof(string));
            if (!dataTable.Columns.Contains("MTITLESRNO")) dataTable.Columns.Add("MTITLESRNO", typeof(Int32));
        }
        dtN = dataTable.Clone();
        for (int j = 0; j < repCount; j++)
        {
            foreach (DataRow dr in dataTable.Rows)
            {
                if (j == 0) mtitle = "Original for Recipients             ";
                if (j == 1) mtitle = "Extra Copy";

                dr["mTITLE"] = mtitle;
                dr["MTITLESRNO"] = j;
            }
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dtN.ImportRow(dataTable.Rows[i]);
            }
        }
        dtN.TableName = dataTable.TableName.ToString();
        return dtN;
    }
    public static byte[] ReadFile(string filePath)
    {
        byte[] buffer;
        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        try
        {
            int length = (int)fileStream.Length;  // get file length
            buffer = new byte[length];            // create buffer
            int count;                            // actual number of bytes read
            int sum = 0;                          // total number of bytes read

            // read until Read method returns 0 (end of the stream has been reached)
            while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                sum += count;  // sum is a buffer offset for next reading
        }
        finally
        {
            fileStream.Close();
        }
        return buffer;
    }

    public double Make_double(string val, int digit)
    {
        double result = 0;
        try
        {
            if (val == "Infinity") val = "0";
            result = Convert.ToDouble(val);
        }
        catch
        {
            result = 0;
        }
        result = Math.Round(result, digit);
        return result;
    }
    public double Make_double(double val, int digit)
    {
        double result = 0;
        try
        {
            if (val.ToString() == "Infinity") val = 0;
            result = val;
        }
        catch
        {
            result = 0;
        }
        result = Math.Round(result, digit);
        return result;
    }
    public void save_SYSOPT(string Qstr, string pco_Cd, string mbr, string ztype, string zvdate, string zuser, string zopt_id, string zopt_text, string zopt_enable, string zopt_param)
    {
        string mhd;
        mhd = "N";
        mhd = seek_iname(Qstr, pco_Cd, "Select 'Y' as opt_exist from FIN_RSYS_OPT where trim(OPT_ID)='" + zopt_id.ToUpper() + "'", "opt_exist");
        if (mhd != "Y")
        {
            using (DataSet fgen_oDS = fill_schema(Qstr, pco_Cd, "FIN_RSYS_OPT"))
            {
                DataRow fgen_oporow = fgen_oDS.Tables[0].NewRow();
                fgen_oporow["BRANCHCD"] = mbr;
                fgen_oporow["TYPE"] = ztype;
                string doc_no;
                doc_no = zopt_id.Substring(1, 4);
                fgen_oporow["VCHNUM"] = doc_no.PadLeft(6, '0');
                fgen_oporow["VCHDATE"] = zvdate;

                fgen_oporow["OPT_ID"] = zopt_id.ToUpper();
                fgen_oporow["OPT_TEXT"] = zopt_text.ToUpper();
                fgen_oporow["OPT_ENABLE"] = zopt_enable.ToUpper();
                fgen_oporow["OPT_PARAM"] = zopt_param.ToUpper();
                fgen_oporow["OPT_PARAM2"] = "-";
                fgen_oporow["OPT_EXCL"] = "-";

                fgen_oporow["ENT_BY"] = zuser;
                fgen_oporow["ENT_DT"] = System.DateTime.Now;
                fgen_oporow["EDT_BY"] = "-";
                fgen_oporow["EDT_DT"] = System.DateTime.Now;

                fgen_oDS.Tables[0].Rows.Add(fgen_oporow);
                save_data(pco_Cd, fgen_oDS, "FIN_RSYS_OPT");
            }
        }
    }

    public void add(string uniqQstr, string coCd)
    {
        string mhd;
        Cls_comp_code = coCd;
        string frm_cocd = coCd;
        string frm_qstr = uniqQstr;
        // ------------------------------------------------------------------
        switch (coCd)
        {
            case "SRPF":
            case "KLAS":
            case "KCLG":
            case "PKGW":
            case "TEST":
            case "RIKI":
            case "BRPL":
            case "AGRM":
            case "ERAL":
            case "SVPL":
            case "KESR":
            case "AIPL":
            case "HIME":
                mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0020'", "idno");
                if (mhd == "0" || mhd == "")
                {
                    execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0020','DEV_A',GETDATE())");

                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0001'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0001','DEV_A',GETDATE())");
                        // ------------------------------------------------------------------
                        // Pre Sale / Lead Managament
                        // ------------------------------------------------------------------

                        add_icon(uniqQstr, "F45000", 1, "CRM Module", 3, "-", "-", "Y", "-", "fin45_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F45100", 2, "CRM Activity", 3, "-", "-", "Y", "fin45_e1", "fin45_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F45101", 3, "Lead Logging", 3, "../dir-crm/om_lead_log.aspx", "-", "-", "fin45_e1", "fin45_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F45106", 3, "Lead Followup", 3, "../dir-crm/om_lead_act.aspx", "-", "-", "fin45_e1", "fin45_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F45116", 2, "CRM Reports", 3, "-", "-", "Y", "fin45_e2", "fin45_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F45121", 3, "Lead Log List", 3, "../dir-crm-reps/om_view_crm.aspx", "-", "-", "fin45_e2", "fin45_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F45126", 3, "Lead Followup List", 3, "../dir-crm-reps/om_view_crm.aspx", "-", "-", "fin45_e2", "fin45_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F45131", 3, "Lead Status List", 3, "../dir-crm-reps/om_view_crm.aspx", "-", "-", "fin45_e2", "fin45_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F45140", 2, "CRM Dashboards", 3, "-", "-", "Y", "fin45_e3", "fin45_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F45141", 3, "Lead Mgmt Dashboard", 3, "../dir-crm-reps/om_dbd_crm.aspx", "-", "-", "fin45_e3", "fin45_a1", "-", "fa-edit");
                        ////add_icon(uniqQstr, "F45156", 1, "CRM Masters", 3, "-", "-", "Y", "fin45_e4", "fin45_a4", "-", "fa-edit");
                        ////add_icon(uniqQstr, "F45161", 2, "CRM Status Master", 3, "../tej-base/om_Typ_mst.aspx", "-", "Y", "fin45_e4", "fin45_a4", "-", "fa-edit");

                        // ------------------------------------------------------------------
                        // Customer Complaint Redressal
                        // ------------------------------------------------------------------

                        add_icon(uniqQstr, "F61000", 1, "Customer Complaint Module", 3, "-", "-", "Y", "-", "fin61_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F61100", 2, "Complaint Activity", 3, "-", "-", "Y", "fin61_e1", "fin61_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F61101", 3, "Complaint Log", 3, "../dir-css/om_ccm_log.aspx", "-", "-", "fin61_e1", "fin61_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F61106", 3, "Complaint Action", 3, "../dir-css/om_ccm_act.aspx", "-", "-", "fin61_e1", "fin61_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F61116", 2, "CCM Reports", 3, "-", "-", "Y", "fin61_e2", "fin61_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F61121", 3, "Complaint Log List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin61_e2", "fin61_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F61126", 3, "Complaint Action List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin61_e2", "fin61_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F61131", 3, "Complaint Status List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin61_e2", "fin61_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F61140", 2, "CCM Dashboards", 3, "-", "-", "Y", "fin61_e3", "fin61_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F61141", 3, "CCM Mgmt Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin61_e3", "fin61_a1", "-", "fa-edit");

                        // ------------------------------------------------------------------
                        // Customer O/s Monitoring
                        // ------------------------------------------------------------------


                        if (coCd == "TEST")
                        {
                            add_icon(uniqQstr, "F93000", 1, "Finsys OMS", 3, "-", "-", "Y", "-", "fin93_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F93100", 2, "OMS Activity", 3, "-", "-", "Y", "fin93_e1", "fin93_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F93101", 3, "OMS Plan", 3, "../dir-crm/om_oms_Plan.aspx", "-", "-", "fin93_e1", "fin93_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F93106", 3, "OMS Followup", 3, "../dir-crm/om_oms_folo.aspx", "-", "-", "fin93_e1", "fin93_a1", "-", "fa-edit");

                            add_icon(uniqQstr, "F93116", 2, "OMS Reports", 3, "-", "-", "Y", "fin93_e2", "fin93_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F93121", 3, "OMS Person Wise ", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e2", "fin93_a1", "-", "fa-edit", "N", "Y");
                            add_icon(uniqQstr, "F93126", 3, "OMS Client Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e2", "fin93_a1", "-", "fa-edit", "N", "Y");
                            add_icon(uniqQstr, "F93131", 3, "OMS Tgt VS Action", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin93_e2", "fin93_a1", "-", "fa-edit", "N", "Y");

                            add_icon(uniqQstr, "F93132", 3, "OMS Team Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin93_e2", "fin93_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F93133", 3, "OMS Client Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin93_e2", "fin93_a1", "-", "fa-edit");

                            // ------------------------------------------------------------------
                            // Customer Support System Menus
                            // ------------------------------------------------------------------

                            add_icon(uniqQstr, "F60000", 1, "Customer Support System", 3, "-", "-", "Y", "-", "fin60_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F60100", 2, "CSS Activity", 3, "-", "-", "Y", "fin60_e1", "fin60_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F60101", 3, "CSS Logging", 3, "../dir-css/om_css_log.aspx", "-", "-", "fin60_e1", "fin60_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F60106", 3, "CSS Assignment", 3, "../dir-css/om_css_asg.aspx", "-", "-", "fin60_e1", "fin60_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F60111", 3, "CSS Action", 3, "../dir-css/om_css_act.aspx", "-", "-", "fin60_e1", "fin60_a1", "-", "fa-edit");

                            add_icon(uniqQstr, "F60116", 2, "CSS Reports", 3, "-", "-", "Y", "fin60_e2", "fin60_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F60121", 3, "CSS Log List", 3, "../tej-base/rpt_DevA.aspx", "-", "Y", "fin60_e2", "fin60_a1", "-", "fa-edit", "N", "Y");
                            add_icon(uniqQstr, "F60126", 3, "CSS Assignment List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "-", "fa-edit", "N", "Y");
                            add_icon(uniqQstr, "F60131", 3, "CSS Actions List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "-", "fa-edit", "N", "Y");

                            add_icon(uniqQstr, "F60140", 2, "CSS Dashboards", 3, "-", "-", "Y", "fin60_e3", "fin60_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F60141", 3, "CSS Log Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin60_e3", "fin60_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F60146", 3, "CSS Assign Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin60_e3", "fin60_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F60151", 3, "CSS Action Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin60_e3", "fin60_a1", "-", "fa-edit");


                            add_icon(uniqQstr, "F60156", 2, "CSS Masters", 3, "-", "-", "Y", "fin60_e4", "fin60_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F60161", 3, "CSS Status Master", 3, "../tej-base/om_Typ_mst.aspx", "-", "-", "fin60_e4", "fin60_a1", "-", "fa-edit");

                            add_icon(uniqQstr, "F60171", 2, "CSS Clearance", 3, "-", "-", "Y", "fin60_e5", "fin60_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F60176", 3, "CSS Clearance (Client)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin60_e5", "fin60_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F60181", 3, "CSS Clearance (Asgnor)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin60_e5", "fin60_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F60186", 3, "Action Clearance (Asgnor)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin60_e5", "fin60_a1", "-", "fa-edit");


                            //--------------------------------
                            //ALF Monitoring System
                            //--------------------------------
                            add_icon(uniqQstr, "F92000", 1, "Finsys ALF", 3, "-", "-", "Y", "fin92_e1", "fin92_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F92100", 2, "ALF Planning", 3, "-", "-", "Y", "fin92_e1", "fin92_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F92101", 3, "Record ALF plan", 3, "../dir-css/om_alf_plan.aspx", "-", "-", "fin92_e1", "fin92_a1", "-", "fa-edit");

                            add_icon(uniqQstr, "F92116", 2, "ALF Plan Reports", 3, "-", "-", "Y", "fin92_e2", "fin92_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F92121", 3, "ALF Plan List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin92_e2", "fin92_a1", "-", "fa-edit", "N", "Y");
                            add_icon(uniqQstr, "F92126", 3, "ALF Plan Vs Actual", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin92_e2", "fin92_a1", "-", "fa-edit", "N", "Y");
                            add_icon(uniqQstr, "F92131", 3, "ALF Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin92_e2", "fin92_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F92127", 3, "ALF 31 Day Report", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin92_e2", "fin92_a1", "-", "fa-edit", "N", "Y");

                            // ------------------------------------------------------------------
                            // Software Training Guide
                            // ------------------------------------------------------------------
                            add_icon(uniqQstr, "F94000", 1, "Finsys STL", 3, "-", "-", "Y", "-", "fin94_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F94100", 2, "STL Activity", 3, "-", "-", "Y", "fin94_e1", "fin94_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F94101", 3, "Record STL", 3, "../dir-css/om_stl_log.aspx", "-", "-", "fin94_e1", "fin94_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F94106", 3, "Approve STL", 3, "../tej-base/om_appr.aspx", "-", "-", "fin94_e1", "fin94_a1", "-", "fa-edit");

                            add_icon(uniqQstr, "F94116", 2, "STL Reports", 3, "-", "-", "Y", "fin94_e2", "fin94_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F94121", 3, "Module Wise STL", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin94_e2", "fin94_a1", "-", "fa-edit", "N", "Y");
                            add_icon(uniqQstr, "F94126", 3, "Vertical Wise STL", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin94_e2", "fin94_a1", "-", "fa-edit", "N", "Y");
                            add_icon(uniqQstr, "F94131", 3, "Customer Wise STL", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin94_e3", "fin94_a1", "-", "fa-edit", "N", "Y");
                            add_icon(uniqQstr, "F94132", 3, "STL Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin94_e3", "fin94_a1", "-", "fa-edit");

                            // ------------------------------------------------------------------
                            // ERP Implementation Path
                            // ------------------------------------------------------------------

                            //add_icon(uniqQstr, "F95100", 1, "ERP Implementation Goals", 3, "-", "-", "Y", "fin95_e1", "fin95_a1", "-", "fa-edit");
                            //add_icon(uniqQstr, "F95101", 2, "ERP Module List", 3, "../tej-base/om_Typ_mst.aspx", "-", "-", "fin95_e1", "fin95_a1", "-", "fa-edit");
                            //add_icon(uniqQstr, "F95106", 2, "ERP Mile Stones", 3, "../tej-base/om_Typ_mst.aspx", "-", "-", "fin95_e1", "fin95_a1", "-", "fa-edit");
                            //add_icon(uniqQstr, "F95111", 2, "ERP Delivery Plan", 3, "../dir-css/om_erp_plan.aspx", "-", "-", "fin95_e1", "fin95_a1", "-", "fa-edit");

                            //add_icon(uniqQstr, "F95126", 1, "ERP Implementation Record", 3, "-", "-", "Y", "fin95_e2", "fin95_a2", "-", "fa-edit");
                            //add_icon(uniqQstr, "F95131", 2, "ERP Delv. Record", 3, "../dir-css/om_erp_delv.aspx", "-", "-", "fin95_e2", "fin95_a2", "-", "fa-edit");
                            //add_icon(uniqQstr, "F95132", 2, "ERP Delv. Approval(HO)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin95_e2", "fin95_a2", "-", "fa-edit");
                            //add_icon(uniqQstr, "F95136", 2, "ERP Delv. Approval(Client)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin95_e2", "fin95_a2", "-", "fa-edit");

                            // ------------------------------------------------------------------
                            // Developed Software Library
                            // ------------------------------------------------------------------

                            add_icon(uniqQstr, "F96000", 1, "Finsys DSL", 3, "-", "-", "Y", "-", "fin96_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F96100", 2, "DSL Activity", 3, "-", "-", "Y", "fin96_e1", "fin96_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F96101", 3, "Record DSL", 3, "../dir-css/om_dsl_log.aspx", "-", "-", "fin96_e1", "fin96_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F96106", 3, "Approve DSL", 3, "../tej-base/om_appr.aspx", "-", "-", "fin96_e1", "fin96_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F96107", 3, "DSL Library", 3, "../tej-base/infolib.aspx", "-", "-", "fin96_e1", "fin96_a1", "-", "fa-edit");

                            add_icon(uniqQstr, "F96116", 2, "DSL Reports", 3, "-", "-", "Y", "fin96_e2", "fin96_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F96121", 3, "Developer Wise DSL", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin96_e2", "fin96_a1", "-", "fa-edit", "N", "Y");
                            add_icon(uniqQstr, "F96126", 3, "Vertical Wise DSL", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin96_e2", "fin96_a1", "-", "fa-edit", "N", "Y");
                            add_icon(uniqQstr, "F96131", 3, "Customer Wise DSL", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin96_e2", "fin96_a1", "-", "fa-edit", "N", "Y");
                            add_icon(uniqQstr, "F96132", 3, "DSL Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin96_e2", "fin96_a1", "-", "fa-edit");

                            // ------------------------------------------------------------------
                            // Master Equipment List
                            // ------------------------------------------------------------------

                            add_icon(uniqQstr, "F97000", 1, "Finsys CAM", 3, "-", "-", "Y", "fin97_e1", "fin97_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F97100", 2, "CAM Activity", 3, "-", "-", "Y", "fin97_e1", "fin97_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F97101", 3, "Record CAM", 3, "../dir-css/om_CAM_log.aspx", "-", "-", "fin97_e1", "fin97_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F97106", 3, "Approve CAM", 3, "../tej-base/om_appr.aspx", "-", "-", "fin97_e1", "fin97_a1", "-", "fa-edit");

                            add_icon(uniqQstr, "F97116", 2, "CAM Reports", 3, "-", "-", "Y", "fin97_e2", "fin97_a1", "-", "fa-edit");
                            add_icon(uniqQstr, "F97121", 3, "CAM Log List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin97_e2", "fin97_a1", "-", "fa-edit", "N", "Y");
                            add_icon(uniqQstr, "F97132", 3, "CAM Dashboard", 3, "../tej-base/om_dboard.aspx", "-", "-", "fin97_e2", "fin97_a1", "-", "fa-edit");


                            //add_icon(uniqQstr, "P19005", 1, "PTS Admin", 3, "-", "-", "Y", "finpts_a", "finptsadm", "-", "fa-edit");
                            //add_icon(uniqQstr, "P19005A", 2, "User Rights", 3, "../tej-base/om_dboard.aspx", "-", "-", "finpts_a", "finptsadm", "-", "fa-edit");
                            //add_icon(uniqQstr, "M20016", 2, "Form Config", 3, "../tej-base/om_forms.aspx", "-", "-", "finpts_a", "finptsadm", "-", "fa-edit");

                            //add_icon(uniqQstr, "F60102", 2, "CSS Logging2", 3, "../dir-css/om_css_log2.aspx", "-", "-", "fin60_e1", "fin60_a1", "-", "fa-edit");
                            //add_icon(uniqQstr, "F60137A", 2, "CSS Assignee Status1", 3, "../tej-base/rpt_DevA.aspx", "-", "Y", "fin60_e2", "fin60_a2", "-", "fa-edit", "Y", "Y");
                        }
                    }

                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0002'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0002','DEV_A',GETDATE())");

                        // ------------------------------------------------------------------
                        // Engg Module
                        // ------------------------------------------------------------------
                        add_icon(uniqQstr, "F10000", 1, "Engg/Masters Module", 1, "-", "-", "-", "-", "fin10_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F10100", 2, "Items Masters", 3, "-", "-", "Y", "fin10_e1", "fin10_a1", "-", "fa-edit");


                        add_icon(uniqQstr, "F10106", 3, "Item Sub Groups", 3, "../dir-engg/Isub_Grp.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F10111", 3, "General Items", 3, "../dir-engg/item_gen.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F10121", 3, "Units Master", 3, "../dir-engg/Gen_type.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F10126", 3, "Process Master", 3, "../dir-engg/Gen_type.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");


                        add_icon(uniqQstr, "F10130", 2, "Production Masters", 3, "-", "-", "Y", "fin10_e2", "fin10_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F10131", 3, "Bill of Materials", 3, "../dir-engg/om_bom_ent.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10133", 3, "Process Mapping", 3, "../dir-engg/om_proc_map.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10135", 3, "Process Plan", 3, "../dir-engg/om_proc_plan.aspx", "-", "-", "fin10_e2", "fin10_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F10140", 2, "Master Approvals", 3, "-", "-", "Y", "fin10_e3", "fin10_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F10141", 3, "Item Approval", 3, "../tej-base/om_Appr.aspx", "-", "-", "fin10_e3", "fin10_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10142", 3, "BOM Approval", 3, "../tej-base/om_Appr.aspx", "-", "-", "fin10_e3", "fin10_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10143", 3, "Process Plan Approval", 3, "../tej-base/om_Appr.aspx", "-", "-", "fin10_e3", "fin10_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F10151", 2, "Masters Analysis", 3, "-", "-", "Y", "fin10_e4", "fin10_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F10156", 3, "Item Master List", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit", "N", "Y");

                        // ------------------------------------------------------------------
                        // Purchase Module
                        // ------------------------------------------------------------------


                        add_icon(uniqQstr, "F15000", 1, "Purchase Module", 1, "-", "-", "-", "-", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15100", 2, "Purchase Activity", 3, "-", "-", "Y", "fin15_e1", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15101", 3, "Purchase Request", 3, "../dir-purc/om_pur_req.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15106", 3, "Purchase Orders", 3, "../dir-purc/om_po_entry.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15111", 3, "Purchase Schedule", 3, "../dir-purc/om_pur_sch.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15116", 3, "Approved Price List", 3, "../dir-purc/om_app_vend.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F15121", 2, "Purchase Checklists", 3, "-", "-", "Y", "fin15_e2", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15126", 3, "Purchase Request Checklists", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15127", 3, "Purchase Orders Checklists", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F15128", 3, "Purchase Schedule Checklists", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15129", 3, "Approved Price Checklists", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F15131", 2, "Purchase Reports", 3, "-", "-", "Y", "fin15_e3", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15132", 3, "Purchase Requisition Register", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15133", 3, "Purchase Order Register", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15134", 3, "Purchase Schedule Report", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15135", 3, "Approved Price Register", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "-", "fa-edit", "N", "Y");


                        add_icon(uniqQstr, "F15160", 2, "Purch. Check/Approvals", 3, "-", "-", "Y", "fin15_e4", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15161", 3, "Purch Request Check", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15162", 3, "Purch Request Appr.", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15165", 3, "Purch Order Check", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15166", 3, "Purch Order Appr.", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15171", 3, "Purch Schedule Appr.", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15176", 3, "Price List Appr", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e4", "fin15_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F15151", 2, "Purchase Analysis", 3, "-", "-", "Y", "fin15_e5", "fin15_a1", "-", "fa-edit");

                        // ------------------------------------------------------------------
                        // Gate Inward, Outward
                        // ------------------------------------------------------------------
                        add_icon(uniqQstr, "F20000", 1, "Gate Module", 3, "-", "-", "Y", "-", "fin20_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F20100", 2, "Gate Activity", 3, "-", "-", "Y", "fin20_e1", "fin20_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F20101", 3, "Gate Inward Entry", 3, "../dir-gate/om_gate_inw.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");
                        //>> tag pring reqd 
                        add_icon(uniqQstr, "F20106", 3, "Gate Outward Entry", 3, "../dir-gate/om_gate_outw.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");
                        //>> Scan Option Reqd

                        add_icon(uniqQstr, "F20116", 2, "Gate Checklists", 3, "-", "-", "Y", "fin20_e2", "fin20_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F20121", 3, "Gate Inward Checklist", 3, "../dir-gate-reps/om_view_gate.aspx", "-", "-", "fin20_e2", "fin20_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F20126", 3, "Gate Outward Checklist", 3, "../dir-gate-reps/om_view_gate.aspx", "-", "-", "fin20_e2", "fin20_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F20127", 3, "Gate PO Checklist", 3, "../dir-gate-reps/om_view_gate.aspx", "-", "-", "fin20_e2", "fin20_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F20128", 3, "Gate RGP Checklist", 3, "../dir-gate-reps/om_view_gate.aspx", "-", "-", "fin20_e2", "fin20_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F20131", 2, "Gate Reports", 3, "-", "-", "Y", "fin20_e3", "fin20_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F20132", 3, "Gate Inward Register", 3, "../dir-gate-reps/om_prt_gate.aspx", "-", "-", "fin20_e3", "fin20_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F20133", 3, "Gate Outward Register", 3, "../dir-gate-reps/om_prt_gate.aspx", "-", "-", "fin20_e3", "fin20_a1", "-", "fa-edit", "N", "Y");


                        // ------------------------------------------------------------------
                        // Inventory Module
                        // ------------------------------------------------------------------
                        add_icon(uniqQstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F25100", 2, "Inventory Activity", 3, "-", "-", "Y", "fin25_e1", "fin25_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F25101", 3, "Matl Inward Entry", 3, "../dir-invn/om_mrr_entry.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F25106", 3, "Matl Outward Entry", 3, "../dir-invn/om_chl_entry.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F25111", 3, "Matl Issue Entry", 3, "../dir-invn/om_iss_entry.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F25116", 3, "Matl Return Entry", 3, "../dir-invn/om_ret_entry.aspx", "-", "-", "fin25_e1", "fin25_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F25121", 2, "Inventory Checklists", 3, "-", "-", "Y", "fin25_e2", "fin25_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F25126", 3, "Matl Inward Checklist", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25127", 3, "Matl Outward Checklist", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25128", 3, "Matl Issue Checklist", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25129", 3, "Matl Return Checklist", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e2", "fin25_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F25131", 2, "Stock Reporting", 3, "-", "-", "Y", "fin25_e3", "fin25_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F25132", 3, "Stock Ledger", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F25133", 3, "Stock Summary", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F25134", 3, "Stock Min-Max", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F25141", 3, "Matl Inward Register", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25142", 3, "Matl Outward Register", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F25143", 3, "Matl Issue Register", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25144", 3, "Matl Return Register", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F25151", 2, "OSP.Jobwork Reports", 3, "-", "-", "Y", "fin25_e5", "fin25_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F25152", 3, "Vendor Jobwork Register", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e5", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25156", 3, "Vendor Jobwork Summary", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e5", "fin25_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F25161", 2, "Cust.Jobwork Reports", 3, "-", "-", "Y", "fin25_e6", "fin25_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F25162", 3, "Cust. Jobwork Register", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e6", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25165", 3, "Cust. Jobwork Summary", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e6", "fin25_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F25171", 2, "Inventory Analysis", 3, "-", "-", "Y", "fin25_e7", "fin25_a1", "-", "fa-edit");

                        // ------------------------------------------------------------------
                        // Finsys Q.A. System
                        // ------------------------------------------------------------------
                        add_icon(uniqQstr, "F30000", 1, "Quality Module", 3, "-", "-", "-", "-", "fin30_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F30100", 2, "Quality Templates", 3, "-", "-", "Y", "fin30_e1", "fin30_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F30101", 3, "QA Inwards Template", 3, "../dir-qa/om_qa_templ.aspx", "-", "-", "fin30_e1", "fin30_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F30106", 3, "QA In-Proc Template", 3, "../dir-qa/om_qa_templ.aspx", "-", "-", "fin30_e1", "fin30_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F30111", 3, "QA Outward Template", 3, "../dir-qa/om_qa_templ.aspx", "-", "-", "fin30_e1", "fin30_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F30116", 2, "Quality Checklists", 3, "-", "-", "Y", "fin30_e2", "fin30_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F30121", 3, "QA Inwards Checklist", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F30126", 3, "QA In-Proc Checklist", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F30127", 3, "QA Outward Checklist", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F30131", 2, "Quality Reports", 3, "-", "-", "Y", "fin30_e3", "fin30_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F30132", 3, "QA Inwards Register", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F30133", 3, "QA In-Proc Register", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F30134", 3, "QA Outward Register", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F30140", 2, "Quality (Basic)", 3, "-", "-", "Y", "fin30_e4", "fin30_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F30141", 3, "Basic Inward Quality", 3, "../dir-qa/om_qa_bas.aspx", "-", "-", "fin30_e4", "fin30_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F30151", 2, "Quality Analysis", 3, "-", "-", "Y", "fin30_e5", "fin30_a1", "-", "fa-edit");

                    }

                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0003'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0003','DEV_A',GETDATE())");

                        // ------------------------------------------------------------------
                        // Sales order Management ( Dom)
                        // ------------------------------------------------------------------
                        add_icon(uniqQstr, "F47000", 1, "Domestic Sales Orders", 3, "-", "-", "Y", "-", "fin47_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F47100", 2, "Dom.Order Activity", 3, "-", "-", "Y", "fin47_e1", "fin47_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F47101", 3, "Master S.O. (Dom.)", 3, "../dir-smktg/om_so_entry.aspx", "-", "-", "fin47_e1", "fin47_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F47106", 3, "Supply S.O. (Dom.)", 3, "../dir-smktg/om_so_entry.aspx", "-", "-", "fin47_e1", "fin47_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F47111", 3, "Sales Schedule", 3, "../dir-smktg/om_sale_sch.aspx", "-", "-", "fin47_e1", "fin47_a1", "-", "fa-edit");

                        //-> to correct 
                        add_icon(uniqQstr, "F47121", 2, "Dom.Sales Approvals", 3, "-", "-", "Y", "fin47_e2", "fin47_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F47126", 3, "Check S.O. (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e2", "fin47_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F47127", 3, "Approve S.O. (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e2", "fin47_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F47128", 3, "Sales Schedule Appr.", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e2", "fin47_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F47131", 2, "Dom.Orders Checklists", 3, "-", "-", "Y", "fin47_e3", "fin47_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F47132", 3, "Sales Order Checklists(Dom.)", 3, "../dir-smktg-reps/om_view_smktg.aspx", "-", "-", "fin47_e3", "fin47_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F47133", 3, "Customer Orders(Dom.)", 3, "../dir-smktg-reps/om_view_smktg.aspx", "-", "-", "fin47_e3", "fin47_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F47134", 3, "Product. Orders(Dom.)", 3, "../dir-smktg-reps/om_view_smktg.aspx", "-", "-", "fin47_e3", "fin47_a1", "-", "fa-edit", "N", "N");

                        add_icon(uniqQstr, "F47140", 2, "Dom.Order Reports", 3, "-", "-", "Y", "fin47_e4", "fin47_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F47141", 3, "All Order Register(Dom.)", 3, "../dir-smktg-reps/om_prt_smktg.aspx", "-", "-", "fin47_e4", "fin47_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F47142", 3, "Pending Order Register(Dom.)", 3, "../dir-smktg-reps/om_prt_smktg.aspx", "-", "-", "fin47_e4", "fin47_a1", "-", "fa-edit", "N", "N");

                        add_icon(uniqQstr, "F47151", 2, "Dom.Order Analysis", 3, "-", "-", "-", "fin47_e5", "fin47_a1", "-", "fa-edit");

                        // ------------------------------------------------------------------
                        // Sales order Management (Exp)
                        // ------------------------------------------------------------------
                        add_icon(uniqQstr, "F49000", 1, "Export Sales Orders", 3, "-", "-", "Y", "-", "fin49_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F49100", 2, "Exp.Order Activity", 3, "-", "-", "Y", "fin49_e1", "fin49_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F49101", 3, "Proforma Inv. (Exp.)", 3, "../dir-emktg/om_eso_entry.aspx", "-", "-", "fin49_e1", "fin49_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F49106", 3, "Supply S.O. (Exp.)", 3, "../dir-emktg/om_eso_entry.aspx", "-", "-", "fin49_e1", "fin49_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F49111", 3, "Sales Schedule(Exp.)", 3, "../dir-emktg/om_sale_sch.aspx", "-", "-", "fin49_e1", "fin49_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F49121", 2, "Exp.Sales Approvals", 3, "-", "-", "Y", "fin49_e2", "fin49_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F49126", 3, "Check S.O. (Exp.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin49_e2", "fin49_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F49127", 3, "Approve S.O. (Exp.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin49_e2", "fin49_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F49128", 3, "Sales Schedule Appr.", 3, "../tej-base/om_appr.aspx", "-", "-", "fin49_e2", "fin49_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F49129", 3, "Check P.I. (Exp.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin49_e2", "fin49_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F49130", 3, "Approve P.I. (Exp.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin49_e2", "fin49_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F49131", 2, "Exp.Orders Checklists", 3, "-", "-", "Y", "fin49_e3", "fin49_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F49132", 3, "Sales Order Checklists(Exp.)", 3, "../dir-emktg-reps/om_view_emktg.aspx", "-", "-", "fin49_e3", "fin49_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F49133", 3, "Customer Orders(Exp.)", 3, "../dir-emktg-reps/om_view_emktg.aspx", "-", "-", "fin49_e3", "fin49_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F49134", 3, "Product. Orders(Exp.)", 3, "../dir-emktg-reps/om_view_emktg.aspx", "-", "-", "fin49_e3", "fin49_a1", "-", "fa-edit", "N", "N");

                        add_icon(uniqQstr, "F49140", 2, "Exp.Order Reports", 3, "-", "-", "Y", "fin49_e4", "fin49_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F49141", 3, "All Order Register(Exp.)", 3, "../dir-emktg-reps/om_prt_emktg.aspx", "-", "-", "fin49_e4", "fin49_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F49142", 3, "Pending Order Register(Exp.)", 3, "../dir-emktg-reps/om_prt_emktg.aspx", "-", "-", "fin49_e4", "fin49_a1", "-", "fa-edit", "N", "N");

                        add_icon(uniqQstr, "F49151", 2, "Exp.Order Analysis", 3, "-", "-", "-", "fin49_e5", "fin49_a1", "-", "fa-edit");

                        // ------------------------------------------------------------------
                        // Domestic Sales Module
                        // ------------------------------------------------------------------

                        add_icon(uniqQstr, "F50000", 1, "Domestic Sales Module", 3, "-", "-", "Y", "-", "fin50_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F50100", 2, "Dom.Sales Activity", 3, "-", "-", "Y", "fin50_e1", "fin50_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F50101", 3, "Sales Invoice (Dom.)", 3, "../dir-sales/om_inv_entry.aspx", "-", "-", "fin50_e1", "fin50_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F50106", 3, "Proforma Invoice (Dom.)", 3, "../dir-sales/om_inv_entry.aspx", "-", "-", "fin50_e1", "fin50_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F50121", 2, "Dom.Orders CheckLists", 3, "-", "-", "Y", "fin50_e2", "fin50_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F50126", 3, "Order Data Checklist", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e2", "fin50_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F50127", 3, "Pending Order Checklist", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e2", "fin50_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F50128", 3, "Pending Sch. Checklist", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e2", "fin50_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F50131", 2, "Dom.Sales Checklists", 3, "-", "-", "Y", "fin50_e3", "fin50_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F50132", 3, "Sales Data Checklists(Dom.)", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e3", "fin50_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F50133", 3, "Customer Wise Sales(Dom.)", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e3", "fin50_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F50134", 3, "Product. Wise Sales(Dom.)", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e3", "fin50_a1", "-", "fa-edit", "N", "N");

                        add_icon(uniqQstr, "F50140", 2, "Dom.Sales Reports", 3, "-", "-", "Y", "fin50_e4", "fin50_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F50141", 3, "Sales Register(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F50142", 3, "Customer Wise Reg.(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F50143", 3, "Product. Wise Reg.(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "-", "fa-edit", "N", "N");

                        add_icon(uniqQstr, "F50151", 2, "Dom.Sales Analysis", 3, "-", "-", "Y", "fin50_e5", "fin50_a1", "-", "fa-edit");


                        // ------------------------------------------------------------------
                        // Export Sales Module
                        // ------------------------------------------------------------------

                        add_icon(uniqQstr, "F55000", 1, "Export Sales Module", 3, "-", "-", "Y", "-", "fin55_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F55100", 2, "Exp.Sales Activity", 3, "-", "-", "Y", "fin55_e1", "fin55_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F55101", 3, "Sales Invoice (Exp.)", 3, "../dir-esales/om_einv_entry.aspx", "-", "-", "fin55_e1", "fin55_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F55106", 3, "Proforma Invoice (Exp.)", 3, "../dir-esales/om_einv_entry.aspx", "-", "-", "fin55_e1", "fin55_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F55121", 2, "Exp.Orders CheckLists", 3, "-", "-", "Y", "fin55_e2", "fin55_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F55126", 3, "Order Data Checklist", 3, "../dir-esales-reps/om_view_esale.aspx", "-", "-", "fin55_e2", "fin55_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F55127", 3, "Pending Order Checklist", 3, "../dir-esales-reps/om_view_esale.aspx", "-", "-", "fin55_e2", "fin55_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F55128", 3, "Pending Sch. Checklist", 3, "../dir-esales-reps/om_view_esale.aspx", "-", "-", "fin55_e2", "fin55_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F55131", 2, "Exp.Sales Checklists", 3, "-", "-", "Y", "fin55_e3", "fin55_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F55132", 3, "Sales Data Checklists(Exp.)", 3, "../dir-esales-reps/om_view_esale.aspx", "-", "-", "fin55_e3", "fin55_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F55133", 3, "Customer Wise Sales(Exp.)", 3, "../dir-esales-reps/om_view_esale.aspx", "-", "-", "fin55_e3", "fin55_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F55134", 3, "Product. Wise Sales(Exp.)", 3, "../dir-esales-reps/om_view_esale.aspx", "-", "-", "fin55_e3", "fin55_a1", "-", "fa-edit", "N", "N");

                        add_icon(uniqQstr, "F55140", 2, "Exp.Sales Reports", 3, "-", "-", "Y", "fin55_e4", "fin55_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F55141", 3, "Sales Register(Exp.)", 3, "../dir-esales-reps/om_prt_esale.aspx", "-", "-", "fin55_e4", "fin55_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F55142", 3, "Customer Wise Reg.(Exp.)", 3, "../dir-esales-reps/om_prt_esale.aspx", "-", "-", "fin55_e4", "fin55_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F55143", 3, "Product. Wise Reg.(Exp.)", 3, "../dir-esales-reps/om_prt_esale.aspx", "-", "-", "fin55_e4", "fin55_a1", "-", "fa-edit", "N", "N");

                        add_icon(uniqQstr, "F55151", 2, "Exp.Sales Analysis", 3, "-", "-", "Y", "fin55_e5", "fin55_a1", "-", "fa-edit");
                    }

                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0004'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0004','DEV_A',GETDATE())");

                        add_icon(uniqQstr, "F25171", 2, "Inventory Analysis", 3, "-", "-", "Y", "fin25_e7", "fin25_a1", "-", "fa-edit");


                        // ------------------------------------------------------------------
                        // PPC Module
                        // ------------------------------------------------------------------
                        add_icon(uniqQstr, "F35000", 1, "PPC Module", 3, "-", "-", "Y", "-", "fin35_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F35100", 2, "Prt/Pkg PPC Activity", 3, "-", "-", "Y", "fin35_e1", "fin35_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F35101", 3, "Job Order Creation", 3, "../dir-ppc/om_JCard_entry.aspx", "-", "-", "fin35_e1", "fin35_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F35106", 3, "Job Order Planning", 3, "../dir-ppc/om_JPlan_entry.aspx", "-", "-", "fin35_e1", "fin35_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F35107", 3, "Machine Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "fin35_e1", "fin35_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F35121", 2, "Prt/Pkg PPC Checklists", 3, "-", "-", "Y", "fin35_e2", "fin35_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F35126", 3, "Daily Prodn Checklist(Pt)", 3, "../dir-ppc-reps/om_view_ptppc.aspx", "-", "-", "fin35_e2", "fin35_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F35127", 3, "Mthly Prodn Checklist(Pt)", 3, "../dir-ppc-reps/om_view_ptppc.aspx", "-", "-", "fin35_e2", "fin35_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F35131", 2, "Prodn PPC Activity", 3, "-", "-", "Y", "fin35_e3", "fin35_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F35132", 3, "Sales Plan Entry", 3, "../dir-ppc/om_splan_entry.aspx", "-", "-", "fin35_e3", "fin35_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F35133", 3, "Prodn Plan Entry", 3, "../dir-ppc/om_pplan_entry.aspx", "-", "-", "fin35_e3", "fin35_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F35134", 3, "Day Wise Plan Entry", 3, "../dir-ppc/om_dplan_entry.aspx", "-", "-", "fin35_e3", "fin35_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F35135", 3, "SF Prodn Plan Entry", 3, "../dir-ppc/om_sfplan_entry.aspx", "-", "-", "fin35_e3", "fin35_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F35140", 2, "Prodn PPC Checklists", 3, "-", "-", "Y", "fin35_e4", "fin35_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F35141", 3, "Daily Prodn Checklist(Gn)", 3, "../dir-ppc-reps/om_view_gnppc.aspx", "-", "-", "fin35_e4", "fin35_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F35142", 3, "Mthly Prodn Checklist(Gn)", 3, "../dir-ppc-reps/om_view_gnppc.aspx", "-", "-", "fin35_e4", "fin35_a1", "-", "fa-edit", "N", "Y");


                        // ------------------------------------------------------------------
                        // Training Module
                        // ------------------------------------------------------------------

                        add_icon(uniqQstr, "F80000", 1, "Training Module", 3, "-", "-", "Y", "-", "fin80_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F80100", 2, "Training Activity", 3, "-", "-", "Y", "fin80_e1", "fin80_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F80101", 3, "Training Need Identify", 3, "../dir-hrm/om_train_req.aspx", "-", "-", "fin80_e1", "fin80_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F80106", 3, "Training Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin80_e1", "fin80_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F80111", 3, "Training Done Entry", 3, "../dir-hrm/om_train_done.aspx", "-", "-", "fin80_e1", "fin80_a1", "-", "fa-edit");


                        add_icon(uniqQstr, "F80121", 2, "Training Checklist", 3, "-", "-", "Y", "fin80_e2", "fin80_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F80126", 3, "Trng Rqmt Checklist", 3, "../dir-hrm-reps/om_view_hrm.aspx", "-", "-", "fin80_e2", "fin80_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F80127", 3, "Trng Done Checklist", 3, "../dir-hrm-reps/om_view_hrm.aspx", "-", "-", "fin80_e2", "fin80_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F80128", 3, "Trng Need Vs Done", 3, "../dir-hrm-reps/om_view_hrm.aspx", "-", "-", "fin80_e2", "fin80_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F80131", 2, "Training Analysis", 3, "-", "-", "Y", "fin80_e3", "fin80_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F80132", 3, "Training Dashboard", 3, "../dir-hrm-reps/om_dbd_hrm.aspx", "-", "-", "fin80_e3", "fin80_a1", "-", "fa-edit");

                        // ------------------------------------------------------------------
                        // Leave Request Module
                        // ------------------------------------------------------------------
                        add_icon(uniqQstr, "F81000", 1, "Leave Mgmt Module", 3, "-", "-", "Y", "-", "fin81_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F81100", 2, "Leave Mgmt Activity", 3, "-", "-", "Y", "fin81_e1", "fin81_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F81101", 3, "Leave Request", 3, "../dir-hrm/om_leave_req.aspx", "-", "-", "fin81_e1", "fin81_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F81106", 3, "Leave Req Checking", 3, "../tej-base/om_appr.aspx", "-", "-", "fin81_e1", "fin81_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F81111", 3, "Leave Req Approval", 3, "../tej-base/om_appr.aspx", "-", "-", "fin81_e1", "fin81_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F81121", 2, "Leaves Checklist", 3, "-", "-", "Y", "fin81_e2", "fin81_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F81126", 3, "Request Checklist", 3, "../dir-hrm-reps/om_view_hrm.aspx", "-", "-", "fin81_e2", "fin81_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F81127", 3, "Approval Checklist", 3, "../dir-hrm-reps/om_view_hrm.aspx", "-", "-", "fin81_e2", "fin81_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F81131", 2, "Leaves Analysis", 3, "-", "-", "Y", "fin81_e3", "fin81_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F81132", 3, "Leave Mgmt Dashboard", 3, "../dir-hrm-reps/om_dbd_hrm.aspx", "-", "-", "fin81_e3", "fin81_a1", "-", "fa-edit");

                    }

                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0005'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0005','DEV_A',GETDATE())");

                        // ------------------------------------------------------------------
                        // Self Sevice Docs Module
                        // ------------------------------------------------------------------
                        add_icon(uniqQstr, "F82000", 1, "Document Mgmt Module", 3, "-", "-", "Y", "-", "fin82_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F82100", 2, "Employee Docs", 3, "-", "-", "Y", "fin82_e1", "fin82_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F82101", 3, "Upload Tax Docs", 3, "../dir-hrm/om_leave_req.aspx", "-", "-", "fin82_e1", "fin82_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F82106", 3, "Approve Tax Docs", 3, "../tej-base/om_appr.aspx", "-", "-", "fin82_e1", "fin82_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F82121", 2, "Document Checklist", 3, "-", "-", "Y", "fin82_e2", "fin82_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F82126", 3, "Tax Docs Checklist", 3, "../dir-hrm-reps/om_view_crm.aspx", "-", "-", "fin82_e2", "fin82_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F82131", 2, "Document Masters", 3, "-", "-", "Y", "fin82_e3", "fin82_a1", "-", "fa-edit");



                        add_icon(uniqQstr, "F10116", 3, "FG/SFG Items", 3, "../dir-engg/item_gen.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F70100", 2, "Accounting Activity", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F70101", 3, "Receipt Vouchers", 3, "../tej-base/om_rcpt_vch.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F70111", 3, "Journal Vouchers", 3, "../tej-base/om_jour_vch.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F70116", 3, "Purchase Vouchers", 3, "../tej-base/om_purc_vch.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F70117", 3, "Bank Reco.", 3, "../tej-base/om_bank_reco.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");


                        add_icon(uniqQstr, "F70118", 3, "Auto Debit Credit Note", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");


                        add_icon(uniqQstr, "F70121", 2, "Accounts Checklists", 3, "-", "-", "Y", "fin70_e2", "fin70_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F70126", 3, "Rcpts. Checklists", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70127", 3, "Pymts. Checklists", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70128", 3, "J.V.   Checklists", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70129", 3, "Purch. Checklists", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e2", "fin70_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F70131", 2, "Accounts Registers", 3, "-", "-", "Y", "fin70_e3", "fin70_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F70132", 3, "Rcpts. Register", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70133", 3, "Pymts. Register", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70134", 3, "J.V.   Register", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70135", 3, "Purch. Register", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F70171", 2, "Acctg Master Options", 3, "-", "-", "Y", "fin70_e5", "fin70_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F70172", 3, "Accounts Master", 3, "../tej-base/acct_gen.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
                    }

                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0006'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0006','DEV_A',GETDATE())");

                        execute_cmd(coCd, "delete from ico_tab where id in ('F10101','F10121','F10126','F70173','F70176','F70174','F80116','F82132') ");

                        add_icon(uniqQstr, "F10101", 3, "Item Main Groups", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F10121", 3, "Units Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F10126", 3, "Process Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F70173", 3, "Accounts Groups", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70176", 3, "Voucher Types", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F70174", 3, "Accounts Schedules", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F80116", 3, "Training Topics", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin80_e1", "fin80_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F82132", 3, "Tax Docs Masters", 3, "../tej-base/om_tgpop_mst.aspx", "-", "-", "fin82_e3", "fin82_a1", "-", "fa-edit");

                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "24/12/2017", "DEV_A", "W0001", "Reel Grid in MRR/CHL/ISS", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "24/12/2017", "DEV_A", "W0002", "Bar Code Read Option in MRR/CHL/ISS", "N", "2");
                    }

                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0007'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0007','DEV_A',GETDATE())");

                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "26/12/2017", "DEV_A", "W0003", "Job No. Reqd in Issue System", "N", "2");

                        add_icon(uniqQstr, "F20140", 2, "Gate Analysis", 3, "-", "-", "Y", "fin20_e4", "fin20_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F25135", 3, "Store Stock Value", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25136", 3, "OSP J/wrk Stock Value", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e3", "fin25_a1", "-", "fa-edit", "N", "Y");
                    }

                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0009'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0009','DEV_A',GETDATE())");

                        add_icon(uniqQstr, "F25140", 2, "Inventory Reports", 3, "-", "-", "Y", "fin25_e4", "fin25_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F47116", 3, "Sales Projection", 3, "../dir-ppc/om_splan_entry.aspx", "-", "-", "fin47_e1", "fin47_a1", "-", "fa-edit");

                        //fin15_e3

                        add_icon(uniqQstr, "F15221", 3, "More Reports(Purch.)", 3, "../tej-base/moreReports.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F15136", 4, "Closed PR Register", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15140", 4, "Pending PR Register", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15141", 4, "PR Vs PO Register", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15142", 4, "Pending PO Register", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15143", 4, "PO Vs MRR Register", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");


                        add_icon(uniqQstr, "F15222", 4, "Sch Vs Rcpt Day Wise", 3, "../dir-purc-reps/om_prt_purc.aspx", "31 Day Tracker ", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15223", 4, "Sch Vs Rcpt Total Basis", 3, "../dir-purc-reps/om_prt_purc.aspx", "Summary of Sch Vs Rcpt", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15225", 4, "TAT PR Vs PO", 3, "../dir-purc-reps/om_view_purc.aspx", "Turn Around Time PR VS PO", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15226", 4, "TAT PO Vs MRR", 3, "../dir-purc-reps/om_view_purc.aspx", "Turn Around Time PO Vs MRR", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15227", 4, "TAT PR VS PO Vs MRR", 3, "../dir-purc-reps/om_view_purc.aspx", "Turn Around Time PR Vs PO Vs MRR", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15230", 4, "Price Comparison Chart Vendor Wise", 3, "../dir-purc-reps/om_prt_purc.aspx", "Compare Prices Vendor Wise", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15231", 4, "Price Comparison Chart Item Wise", 3, "../dir-purc-reps/om_prt_purc.aspx", "Compare Prices Item Wise", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15232", 4, "Price Comparison Chart Plant Wise", 3, "../dir-purc-reps/om_prt_purc.aspx", "Compare Prices Plant Wise,Item Wise", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");

                        //fin25_e4
                        add_icon(uniqQstr, "F25221", 3, "More Reports(Inventory)", 3, "../tej-base/moreReports.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F25222", 4, "Deptt Wise Issue Summary", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25223", 4, "Deptt Wise Issue Comparison", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25230", 4, "Rejn Stock Summary Item Wise", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25231", 4, "Rejn Stock Summary Vendor Wise", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25232", 4, "Rejn Stock Ledger", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25245", 4, "Matl. Location Report", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F25245A", 4, "FG Sticker", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F25245R", 4, "Return Sticker", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");

                        //fin47_e4
                        add_icon(uniqQstr, "F47221", 3, "More Reports(Dom.S.O.)", 3, "../tej-base/moreReports.aspx", "-", "-", "fin47_e4", "fin47_a1", "fin47_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F47222", 4, "Order Vs Dispatch", 3, "../dir-smktg-reps/om_prt_smktg.aspx", "-", "-", "fin47_e4", "fin47_a1", "fin47_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F47223", 4, "Schedule Vs Dispatch", 3, "../dir-smktg-reps/om_prt_smktg.aspx", "-", "-", "fin47_e4", "fin47_a1", "fin47_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F47224", 4, "Schedule Status (Daily)", 3, "../dir-smktg-reps/om_prt_smktg.aspx", "-", "-", "fin47_e4", "fin47_a1", "fin47_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F47225", 4, "Schedule Status (Monthly)", 3, "../dir-smktg-reps/om_prt_smktg.aspx", "-", "-", "fin47_e4", "fin47_a1", "fin47_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F47226", 4, "Rate Trend Chart Product Wise", 3, "../dir-smktg-reps/om_prt_smktg.aspx", "-", "-", "fin47_e4", "fin47_a1", "fin47_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F47227", 4, "Rate Trend Chart Customer Wise", 3, "../dir-smktg-reps/om_prt_smktg.aspx", "-", "-", "fin47_e4", "fin47_a1", "fin47_MREP", "fa-edit", "N", "Y");

                        //fin50_e4
                        add_icon(uniqQstr, "F50221", 3, "More Reports(Dom.Sales)", 3, "../tej-base/moreReports.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F50222", 4, "Party Wise Total Sales Summary(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50223", 4, "Product Wise Total Sales Summary(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50224", 4, "Party Wise 12 Month Sales Qty(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50225", 4, "Party Wise 12 Month Sales Value(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50226", 4, "Product Wise 12 Month Sales Qty(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50227", 4, "Product Wise 12 Month Sales Value(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50228", 4, "31 Day Wise Sales Value(Dom.)", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");

                        //fin60_e2

                        add_icon(uniqQstr, "F60150", 3, "More Reports(CSS)", 3, "../tej-base/moreReports.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F60132", 4, "CSS Status Report", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F60133", 4, "CSS Pending Assignment", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F60134", 4, "CSS Pending Action", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F60135", 4, "CSS Pending Closure", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F60136", 4, "CSS Action Status", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F60137", 4, "CSS Assignee Status", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F60138", 4, "CSS 31 Day Client Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F60139", 4, "CSS 12 Mth Client Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F60142", 4, "CSS 31 Day Team Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F60154", 4, "CSS Pending Team Wise", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");


                        add_icon(uniqQstr, "F10152", 3, "Masters Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F15152", 3, "Purch. Req Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin15_e5", "fin15_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15156", 3, "Purch. Ord Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin15_e5", "fin15_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F20141", 3, "Gate Inward Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin20_e4", "fin20_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F20142", 3, "Gate Outward Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin20_e4", "fin20_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F25176", 3, "Matl Inward Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin25_e7", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25181", 3, "Matl Issue Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin25_e7", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25191", 3, "Stock Data Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin25_e7", "fin25_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F30152", 3, "QA Inward Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin30_e5", "fin30_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F30156", 3, "QA Outward Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin30_e5", "fin30_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F47152", 3, "Dom.Orders Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin47_e5", "fin47_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F49152", 3, "Exp.Orders Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin49_e5", "fin49_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F50152", 3, "Dom.Sales Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin50_e5", "fin50_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F55152", 3, "Exp.Sales Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin55_e5", "fin55_a1", "-", "fa-edit");

                    }
                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0010'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0010','DEV_A',GETDATE())");

                        execute_cmd(coCd, "delete from ico_tab where id in ('F70132','F70133','F70134','F70135') ");

                        add_icon(uniqQstr, "F60153", 4, "CSS Count,Time Analysis", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "fin60_e2", "fin60_a1", "fin60_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F10221", 3, "More Reports(Engg/Devl.)", 3, "../tej-base/moreReports.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F10222", 4, "List of Items With BOM", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10223", 4, "List of Items Without BOM", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10224", 4, "Items in Multiple BOMs", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10225", 4, "BOM Where Parent/Child Match", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10226", 4, "BOM Items Without Sales Order", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");


                        add_icon(uniqQstr, "F15233", 4, "Gate Inward Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "Material Recvd on Gate", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15234", 4, "Matl Inward Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "Gate Entry -> MRR", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15235", 4, "Matl Consumption Report", 3, "../dir-purc-reps/om_prt_purc.aspx", "Review Matl Consumption", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15236", 4, "Supplier,Item Wise 12 Month P.O. Qty", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15237", 4, "Supplier,Item Wise 12 Month P.O. Value", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15238", 4, "Delivery Date Vs Rcpt Date", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15239", 4, "PO Items with Rate Inc/Decrease", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15240", 4, "PO Items with Qty. Inc/Decrease", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15241", 4, "Supplier history Card", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15242", 4, "Supplier Rating Card", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15243", 4, "Multi Plant Pending Orders", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15244", 4, "Multi Plant Rate Comparison", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F25233", 4, "Item Review Transactions", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25234", 4, "Stock summary + Analysis", 3, "../tej-base/om_stk_Asys.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25235", 4, "Short / Excess Supplies", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25236", 4, "Stock Ageing Report", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25237", 4, "Supplier,Item Wise 12 Month Purch. Qty", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25238", 4, "Group, Item Wise 12 Month Purchase Qty", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25239", 4, "Deptt,Item Wise 12 Month Consumption Qty", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25240", 4, "Group, Item Wise 12 Month Consumption Qty", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25241", 4, "Non Moving Item Report", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25242", 4, "Inward Supplies with Rejection", 3, "../tej-base/om_prt_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F30221", 3, "More Reports(Quality)", 3, "../tej-base/moreReports.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F30222", 4, "Supplier History Card", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F30223", 4, "Supplier Rating Report", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F30224", 4, "Inward Supplies with Rejection", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F30225", 4, "Suppliers 12 Month Rejn Trend", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F30226", 4, "Group,Item Wise 12 Month Rejn Trend ", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F30227", 4, "Deptt,Item Wise 12 Month Line Rejn ", 3, "../tej-base/om_prt_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F30228", 4, "Chart Showing Instances of Inward Rejn", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F30229", 4, "Chart Showing Instances of Line Rejn", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e3", "fin30_a1", "fin30_MREP", "fa-edit", "N", "Y");


                        add_icon(uniqQstr, "F70132", 3, "Rcpts. Register", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70133", 3, "Pymts. Register", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70134", 3, "J.V.   Register", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70135", 3, "Purch. Register", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "Y");


                        add_icon(uniqQstr, "F70221", 3, "More Reports(Accounts)", 3, "../tej-base/moreReports.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F70222", 4, "Cash Book", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70223", 4, "Bank Book", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70224", 4, "Sales Register", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70225", 4, "Purchase Register", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70226", 4, "Accounts Review", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70227", 4, "Net Sales Report", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70228", 4, "Expense Trend", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");

                    }
                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0012'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0012','DEV_A',GETDATE())");


                        add_icon(uniqQstr, "F70140", 2, "Accounts Reports", 3, "-", "-", "Y", "fin70_e4", "fin70_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F70141", 3, "Statement of A/c", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70142", 3, "Bills Receivable", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70143", 3, "Bills Payable", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70144", 3, "Receivable Ageing ", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70146", 3, "Payable Ageing ", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F70151", 3, "Trial Balance", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70152", 3, "P & L Account", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70156", 3, "Balance Sheet", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70161", 3, "Yearly Comparison", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "-", "fa-edit", "N", "Y");
                    }

                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0013'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0013','DEV_A',GETDATE())");

                        add_icon(uniqQstr, "F25200", 2, "Inventory Masters", 3, "-", "-", "Y", "fin25_e8", "fin25_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F25201", 3, "Inward Types Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25203", 3, "Outward Types Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25205", 3, "Issue Types Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25207", 3, "Return Types Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F25209", 3, "Department Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin25_e8", "fin25_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F15200", 2, "Purchase Masters", 3, "-", "-", "Y", "fin15_e6", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15201", 3, "P.Order Types Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin15_e6", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15203", 3, "Currency Type Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin15_e6", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15205", 3, "Price Basis Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin15_e6", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15207", 3, "Insurance Term Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin15_e6", "fin25_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15209", 3, "Freight Term Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin15_e6", "fin25_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F50200", 2, "Dom.Sales Masters", 3, "-", "-", "Y", "fin50_e6", "fin50_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F50201", 3, "Sale Inv. Types Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin50_e6", "fin50_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50203", 3, "Currency Type Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin50_e6", "fin50_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50205", 3, "Contract Terms Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin50_e6", "fin50_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50207", 3, "Payment Terms Master", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin50_e6", "fin50_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F50231", 4, "District Wise Sales Value", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50232", 4, "State Wise Sales Value", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50233", 4, "Zone Wise Sales Value", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50234", 4, "Marketing Person Wise Sales Value", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50235", 4, "Customer Group Wise Sales Value", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50236", 4, "Product Sub Group Wise Sales Value", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F50240", 4, "Schedule Vs Dispatch 31 Day", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50241", 4, "Schedule Vs Dispatch 12 Month", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50242", 4, "Schedule Vs Prodn Vs Dispatch Summary", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F50244", 4, "Schedule Vs Dispatch Cust Wise Summary", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50245", 4, "Schedule Vs Dispatch Cust Wise,Item Wise Summary", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F50250", 4, "Schedule Vs Dispatch Qty Year on Year", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50251", 4, "Schedule Vs Dispatch Value Year on Year", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F50255", 4, "Products Where Sales are Growing", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50256", 4, "Customers Where Sales are Growing ", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F50257", 4, "Products Where Sales are Falling", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50258", 4, "Customers Where Sales are Falling", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F50257", 4, "Products Where Schedule are Falling", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50258", 4, "Customers Where Schedule are Falling", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F50264", 4, "Products Wise Sales Vs Returns , PPM", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50265", 4, "Customer,Product Wise Sales Vs Returns, PPM", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "fin50_MREP", "fa-edit", "N", "Y");


                        add_icon(uniqQstr, "F15228", 4, "TAT MRR Vs MRIR", 3, "../dir-purc-reps/om_view_purc.aspx", "Turn Around Time MRR Vs MRIR", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15229", 4, "TAT PR Approval VS PR", 3, "../dir-purc-reps/om_view_purc.aspx", "Turn Around Time PR Approval Vs PR", "Y", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70106", 3, "Payment Vouchers", 3, "../tej-base/om_rcpt_vch.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");


                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "14/01/2017", "DEV_A", "W0004", "OMS Based on Finance Data", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "14/01/2017", "DEV_A", "W0005", "Line No. Based PR Vs PO", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "14/01/2017", "DEV_A", "W0006", "Line No. Based PO Vs Gate Entry", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "14/01/2017", "DEV_A", "W0007", "Line No. Based PO Vs MRR Entry", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "14/01/2017", "DEV_A", "W0008", "Line No. Based SO Vs INV Entry", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "21/01/2017", "DEV_A", "W0009", "Show 9 Series Items in P.R. Entry", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "21/01/2017", "DEV_A", "W0010", "Show 9 Series Items in Issue Entry", "N", "2");

                        // ------------------------------------------------------------------
                        // Supplier Portal
                        // ------------------------------------------------------------------

                        add_icon(uniqQstr, "F78000", 1, "Supplier Portal", 3, "-", "-", "Y", "-", "fin78_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F78100", 2, "Supplier Orders", 3, "-", "-", "Y", "fin78_e1", "fin78_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F78101", 3, "Status :Purch.Orders(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e1", "fin78_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F78106", 3, "Status :Purch.Schedule(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e1", "fin78_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F78121", 2, "Supplier Performance", 3, "-", "-", "Y", "fin78_e2", "fin78_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F78126", 3, "P.O. Dt Vs Rcpt Dt.(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e2", "fin78_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F78127", 3, "Sch. Dt Vs Rcpt Dt.(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e2", "fin78_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F78128", 3, "Rcpt Vs Accpt Qty.(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e2", "fin78_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F78131", 2, "Supplier Dues", 3, "-", "-", "Y", "fin78_e3", "fin78_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F78132", 3, "Supplier Bill Status(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin78_e3", "fin78_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F78133", 3, "Supplier Dashboard(Portal)", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin78_e3", "fin78_a1", "-", "fa-edit");


                        // ------------------------------------------------------------------
                        // Customer Portal
                        // ------------------------------------------------------------------

                        add_icon(uniqQstr, "F79000", 1, "Customer Portal", 3, "-", "-", "Y", "-", "fin79_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F79100", 2, "Customer Orders", 3, "-", "-", "Y", "fin79_e1", "fin79_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F79101", 3, "Status :Sales.Orders(Portal)", 3, "../tej-base/om_cport_reps.aspx", "-", "-", "fin79_e1", "fin79_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F79106", 3, "Status :Sales.Schedule(Portal)", 3, "../tej-base/om_cport_reps.aspx", "-", "-", "fin79_e1", "fin79_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F79121", 2, "Dispatch Performance", 3, "-", "-", "Y", "fin79_e2", "fin79_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F79126", 3, "P.O. Dt Vs Dispatch Dt.(Portal)", 3, "../tej-base/om_cport_reps.aspx", "-", "-", "fin79_e2", "fin79_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F79127", 3, "Sch. Dt Vs Dispatch Dt.(Portal)", 3, "../tej-base/om_cport_reps.aspx", "-", "-", "fin79_e2", "fin79_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F79128", 3, "Customer Dashboard.(Portal)", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin79_e2", "fin79_a1", "-", "fa-edit", "N", "Y");
                    }
                    //add_icon(uniqQstr, "F79131", 2, "Supplier Dues", 3, "-", "-", "Y", "fin79_e3", "fin79_a1", "-", "fa-edit");
                    //add_icon(uniqQstr, "F79132", 3, "Due Bill Status(Portal)", 3, "../tej-base/om_sport_reps.aspx", "-", "-", "fin79_e3", "fin79_a1", "-", "fa-edit");

                    //add_icon(uniqQstr, "F70118", 3, "Credit Dr Cr", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");



                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0014'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0014','DEV_A',GETDATE())");
                        execute_cmd(coCd, "delete from ico_tab where id in ('F99115') ");

                        add_icon(uniqQstr, "F70118", 3, "Auto Debit Credit Note", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F15210", 3, "P.R. Closure", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e6", "fin25_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F15211", 3, "P.O. Closure", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e6", "fin25_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F15245", 4, "Stock summary + Analysis", 3, "../tej-base/om_stk_Asys.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F30142", 3, "Inward QA Report", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e4", "fin30_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F30143", 3, "Inward QA Rejn Report", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e4", "fin30_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F99119", 3, "Mails Config", 3, "../tej-base/om_mail_mgr.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F99120", 3, "Notification Config", 3, "../tej-base/om_mail_mgr.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F99115", 3, "Features Config", 3, "../tej-base/om_mnu_opts.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
                    }



                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0015'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0015','DEV_A',GETDATE())");

                        add_icon(uniqQstr, "F50156", 3, "Plant Wise Sales", 3, "../tej-base/om_view_sale.aspx", "-", "-", "fin50_e5", "fin50_a1", "-", "fa-edit", "Y", "Y");

                        add_icon(uniqQstr, "F47112", 3, "Sales Budget", 3, "../dir-smktg/om_sale_budg.aspx", "-", "-", "fin47_e1", "fin47_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F10227", 4, "Items Not Used in During DTD", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10228", 4, "Boms  Not Used in During DTD", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10229", 4, "List of Deactivated Items", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10230", 4, "List of Un Approved Items", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10231", 4, "List of Items With Selected Fields", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10232", 4, "List of BOMS With Selected Fields", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F70229", 4, "P & L Trend Mthly", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70230", 4, "P & L Trend Qtrly", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F70231", 4, "Day Book:Rcpts", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F70232", 4, "Day Book:Payments", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F70233", 4, "Day Book:Journal", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F70234", 4, "Day Book:Sales", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F70235", 4, "Day Book:Purchase", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F70236", 4, "Day Book:Cash", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F70239", 4, "Day Book:Bank", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");

                        add_icon(uniqQstr, "F70237", 4, "Trial Balance 2 Col", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F70238", 4, "Trial Balance 6 Col", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "Y");
                    }


                    //1. F15222 ----------------------- ../dir-purc-reps/om_prt_purc.aspx
                    //2. F15223 ----------------------- ../dir-purc-reps/om_prt_purc.aspx
                    //3.F15238 ----------------------- ../dir-purc-reps/om_prt_purc.aspx
                    //4. F15239 ----------------------- ../dir-purc-reps/om_prt_purc.aspx

                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0016'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0016','DEV_A',GETDATE())");
                        execute_cmd(coCd, "delete from ico_tab where substring(id,1,3)  in ('F39','F40','F43')");

                        // ------------------------------------------------------------------
                        // Plastic/rubber Prodn Module
                        // ------------------------------------------------------------------

                        add_icon(uniqQstr, "F39000", 1, "Moulding Production", 3, "-", "-", "Y", "-", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39100", 2, "Prodn Activity", 3, "-", "-", "Y", "fin39_e1", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39101", 3, "Moulding Entry", 3, "../dir-prodpm/om_mldp_entry.aspx", "-", "-", "fin39_e1", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39106", 3, "Painting Entry", 3, "../dir-prodpm/om_mldp_entry.aspx", "-", "-", "fin39_e1", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39111", 3, "Assembly Entry", 3, "../dir-prodpm/om_mldp_entry.aspx", "-", "-", "fin39_e1", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39116", 3, "SF Prodn Entry", 3, "../dir-prodpm/om_prod_sffg.aspx", "-", "-", "fin39_e1", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39117", 3, "Inter Stage Tfr", 3, "../dir-prodpm/om_stg_tfr.aspx", "-", "-", "fin39_e1", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39118", 3, "FG Prodn Entry", 3, "../dir-prodpm/om_prod_sffg.aspx", "-", "-", "fin39_e1", "fin39_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F39121", 2, "Moulding Prodn Checklists", 3, "-", "-", "Y", "fin39_e2", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39126", 3, "Moulding Checklist", 3, "../dir-prodpm-reps/om_view_prodpm.aspx", "-", "-", "fin39_e2", "fin39_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F39127", 3, "Painting Checklist", 3, "../dir-prodpm-reps/om_view_prodpm.aspx", "-", "-", "fin39_e2", "fin39_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F39128", 3, "Assembly Checklist", 3, "../dir-prodpm-reps/om_view_prodpm.aspx", "-", "-", "fin39_e2", "fin39_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F39129", 3, "Down Time Checklist", 3, "../dir-prodpm-reps/om_view_prodpm.aspx", "-", "-", "fin39_e2", "fin39_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F39130", 3, "Rejection Checklist", 3, "../dir-prodpm-reps/om_view_prodpm.aspx", "-", "-", "fin39_e2", "fin39_a1", "-", "fa-edit", "N", "Y");


                        add_icon(uniqQstr, "F39140", 2, "Moulding Prodn Reports", 3, "-", "-", "Y", "fin39_e3", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39141", 3, "Moulding Register", 3, "../dir-prodpm-reps/om_prt_prodpm.aspx", "-", "-", "fin39_e3", "fin39_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F39142", 3, "Painting Register", 3, "../dir-prodpm-reps/om_prt_prodpm.aspx", "-", "-", "fin39_e3", "fin39_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F39143", 3, "Assembly Register", 3, "../dir-prodpm-reps/om_prt_prodpm.aspx", "-", "-", "fin39_e3", "fin39_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F39144", 3, "Down Time Reports(Mld)", 3, "../dir-prodpm-reps/om_prt_prodpm.aspx", "-", "-", "fin39_e3", "fin39_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F39146", 3, "Rejection Reports(Mld)", 3, "../dir-prodpm-reps/om_prt_prodpm.aspx", "-", "-", "fin39_e3", "fin39_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F39171", 2, "Moulding Prodn Analysis", 3, "-", "-", "Y", "fin39_e4", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39172", 3, "Moulding Prodn Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin39_e4", "fin39_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F39173", 3, "Painting Prodn Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin39_e4", "fin39_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F39174", 3, "Assembly Prodn Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin39_e4", "fin39_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F39176", 3, "Moulding OEE Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin39_e4", "fin39_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F39200", 2, "Material Requests", 3, "-", "-", "Y", "fin39_e9", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39201", 3, "Matl Issue Request", 3, "../dir-prod/om_prd_req.aspx", "-", "-", "fin39_e9", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39206", 3, "Matl Return Request", 3, "../dir-prod/om_prd_req.aspx", "-", "-", "fin39_e9", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39211", 3, "Matl JobWork Request", 3, "../dir-prod/om_prd_req.aspx", "-", "-", "fin39_e9", "fin39_a1", "-", "fa-edit");

                        //------------------------
                        // print/corr prodn
                        //------------------------

                        add_icon(uniqQstr, "F40000", 1, "Packaging Production", 3, "-", "-", "Y", "-", "fin40_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F40100", 2, "Prt/Pkg Activity", 3, "-", "-", "Y", "fin40_e1", "fin40_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F40101", 3, "Printing Prodn", 3, "../dir-prodpp/om_print_entry.aspx", "-", "-", "fin40_e1", "fin40_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F40106", 3, "Corrugation Prodn", 3, "../dir-prodpp/om_corr_entry.aspx", "-", "-", "fin40_e1", "fin40_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F40111", 3, "Ptg/Pkg Process Prodn", 3, "../dir-prodpp/om_prtg_entry.aspx", "-", "-", "fin40_e1", "fin40_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F40121", 2, "Prt/Pkg Prodn Checklists", 3, "-", "-", "Y", "fin40_e2", "fin40_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F40126", 3, "Daily Prodn Checklist(PP)", 3, "../dir-prodpp-reps/om_view_prodpp.aspx", "-", "-", "fin40_e2", "fin40_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F40127", 3, "Mthly Prodn Checklist(PP)", 3, "../dir-prodpp-reps/om_view_prodpp.aspx", "-", "-", "fin40_e2", "fin40_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F40128", 3, "Down Time Checklist(PP)", 3, "../dir-prodpp-reps/om_view_prodpp.aspx", "-", "-", "fin40_e2", "fin40_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F40129", 3, "Rejection Checklist(PP)", 3, "../dir-prodpp-reps/om_view_prodpp.aspx", "-", "-", "fin40_e2", "fin40_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F40131", 2, "Prt/Pkg Prodn Reports", 3, "-", "-", "Y", "fin40_e3", "fin40_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F40132", 3, "Daily Prodn Report(PP)", 3, "../dir-prodpp-reps/om_prt_prodpp.aspx", "-", "-", "fin40_e3", "fin40_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F40133", 3, "Mthly Prodn Report(PP)", 3, "../dir-prodpp-reps/om_prt_prodpp.aspx", "-", "-", "fin40_e3", "fin40_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F40134", 3, "Consumption Report(PP)", 3, "../dir-prodpp-reps/om_prt_prodpp.aspx", "-", "-", "fin40_e3", "fin40_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F40135", 3, "Wastages Report(PP)", 3, "../dir-prodpp-reps/om_prt_prodpp.aspx", "-", "-", "fin40_e3", "fin40_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F40171", 2, "Packaging Prodn Analysis", 3, "-", "-", "Y", "fin40_e4", "fin40_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F40172", 3, "Prtg. Prodn Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin40_e4", "fin39_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F40173", 3, "Corr. Prodn Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin40_e4", "fin39_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F40200", 2, "Material Requests", 3, "-", "-", "Y", "fin40_e9", "fin40_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F40201", 3, "Matl Issue Request", 3, "../dir-prod/om_prd_req.aspx", "-", "-", "fin40_e9", "fin40_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F40206", 3, "Matl Return Request", 3, "../dir-prod/om_prd_req.aspx", "-", "-", "fin40_e9", "fin40_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F40211", 3, "Matl JobWork Request", 3, "../dir-prod/om_prd_req.aspx", "-", "-", "fin40_e9", "fin40_a1", "-", "fa-edit");

                        //------------------------
                        // Sheet metal prodn
                        //------------------------
                        add_icon(uniqQstr, "F43000", 1, "Sh/Metal Production", 3, "-", "-", "Y", "-", "fin43_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F43100", 2, "Prodn Activity", 3, "-", "-", "Y", "fin43_e1", "fin43_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F43101", 3, "Press Shop Entry", 3, "../dir-prodpm/om_mldp_entry.aspx", "-", "-", "fin43_e1", "fin43_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F43106", 3, "Paint Shop Entry", 3, "../dir-prodpm/om_mldp_entry.aspx", "-", "-", "fin43_e1", "fin43_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F43111", 3, "Assy. Shop Entry", 3, "../dir-prodpm/om_mldp_entry.aspx", "-", "-", "fin43_e1", "fin43_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F43116", 3, "SF Prodn Entry", 3, "../dir-prodpm/om_prod_sffg.aspx", "-", "-", "fin43_e1", "fin43_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F43117", 3, "Inter Stage Tfr", 3, "../dir-prodpm/om_stg_tfr.aspx", "-", "-", "fin43_e1", "fin43_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F43118", 3, "FG Prodn Entry", 3, "../dir-prodpm/om_prod_sffg.aspx", "-", "-", "fin43_e1", "fin43_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F43121", 2, "Sh/Metal Prodn Checklists", 3, "-", "-", "Y", "fin43_e2", "fin43_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F43126", 3, "Press Shop Checklist(SM)", 3, "../dir-prodpm-reps/om_view_prodsm.aspx", "-", "-", "fin43_e2", "fin43_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F43127", 3, "Paint Shop Checklist(SM)", 3, "../dir-prodpm-reps/om_view_prodsm.aspx", "-", "-", "fin43_e2", "fin43_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F43129", 3, "Down Time Checklist(SM)", 3, "../dir-prodpm-reps/om_view_prodsm.aspx", "-", "-", "fin43_e2", "fin43_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F43130", 3, "Rejection Checklist(SM)", 3, "../dir-prodpm-reps/om_view_prodsm.aspx", "-", "-", "fin43_e2", "fin43_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F43140", 2, "Sh/Metal Prodn Reports", 3, "-", "-", "Y", "fin43_e3", "fin43_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F43141", 3, "Press Shop Register(SM)", 3, "../dir-prodpm-reps/om_prt_prodsm.aspx", "-", "-", "fin43_e3", "fin43_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F43142", 3, "Paint Shop Register(SM)", 3, "../dir-prodpm-reps/om_prt_prodsm.aspx", "-", "-", "fin43_e3", "fin43_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F43143", 3, "Assy. Shop Register(SM)", 3, "../dir-prodpm-reps/om_prt_prodsm.aspx", "-", "-", "fin43_e3", "fin43_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F43144", 3, "Down Time Reports(SM)", 3, "../dir-prodpm-reps/om_prt_prodsm.aspx", "-", "-", "fin43_e3", "fin43_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F43146", 3, "Rejection Reports(SM)", 3, "../dir-prodpm-reps/om_prt_prodsm.aspx", "-", "-", "fin43_e3", "fin43_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F43171", 2, "Sh/Metal Prodn Analysis", 3, "-", "-", "Y", "fin43_e4", "fin43_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F43172", 3, "Press Shop Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin43_e4", "fin43_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F43173", 3, "Paint Shop Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin43_e4", "fin43_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F43174", 3, "Assy. Shop Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin43_e4", "fin43_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F43200", 2, "Material Requests", 3, "-", "-", "Y", "fin43_e9", "fin43_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F43201", 3, "Matl Issue Request", 3, "../dir-prod/om_prd_req.aspx", "-", "-", "fin43_e9", "fin43_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F43206", 3, "Matl Return Request", 3, "../dir-prod/om_prd_req.aspx", "-", "-", "fin43_e9", "fin43_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F43211", 3, "Matl JobWork Request", 3, "../dir-prod/om_prd_req.aspx", "-", "-", "fin43_e9", "fin43_a1", "-", "fa-edit");


                    }

                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0017'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0017','DEV_A',GETDATE())");

                        //--------------------------------
                        //Mgmt MIS System
                        //--------------------------------
                        add_icon(uniqQstr, "F05000", 1, "Management MIS", 3, "-", "-", "Y", "fin05_e1", "fin05_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F05100", 2, "Sales MIS", 3, "-", "-", "Y", "fin05_e1", "fin05_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F05101", 3, "Target Vs Ach.", 3, "../dir-mis/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F05106", 3, "Schedule Vs Ach.", 3, "../dir-mis/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F05111", 3, "Plant Wise Sales", 3, "../dir-mis/om_view_mis.aspx", "-", "-", "fin05_e1", "fin05_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F05121", 2, "Acctg MIS", 3, "-", "-", "Y", "fin05_e2", "fin05_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F05126", 3, "Debtors Ageing", 3, "../dir-mis/om_view_mis.aspx", "-", "-", "fin05_e2", "fin05_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F05127", 3, "Creditor Ageing", 3, "../dir-mis/om_view_mis.aspx", "-", "-", "fin05_e2", "fin05_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F05140", 2, "Prodn MIS", 3, "-", "-", "Y", "fin05_e3", "fin05_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F05141", 3, "Production Report", 3, "../dir-mis/om_view_mis.aspx", "-", "-", "fin05_e3", "fin05_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F05142", 3, "Consumption Report", 3, "../dir-mis/om_view_mis.aspx", "-", "-", "fin05_e3", "fin05_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F05143", 3, "Downtime Report", 3, "../dir-mis/om_view_mis.aspx", "-", "-", "fin05_e3", "fin05_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F05144", 3, "Rejection Report", 3, "../dir-mis/om_view_mis.aspx", "-", "-", "fin05_e3", "fin05_a1", "-", "fa-edit", "N", "Y");


                        //--------------------------------
                        //Maint System
                        //--------------------------------
                        add_icon(uniqQstr, "F75000", 1, "Maintenance Module", 3, "-", "-", "Y", "fin75_e1", "fin75_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F75100", 2, "Maint. Activity", 3, "-", "-", "Y", "fin75_e1", "fin75_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F75101", 3, "Maint. Planning", 3, "../dir-maint/om_maint_plan.aspx", "-", "-", "fin75_e1", "fin75_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F75106", 3, "Maint. Planned Action", 3, "../dir-maint/om_maint_Act.aspx", "-", "-", "fin75_e1", "fin75_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F75111", 3, "Maint. Complaint Action", 3, "../dir-maint/om_comp_Act.aspx", "-", "-", "fin75_e1", "fin75_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F75121", 2, "Maintenance Logs", 3, "-", "-", "Y", "fin75_e2", "fin75_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F75126", 3, "Planned Maint. Logs", 3, "../dir-maint-reps/om_view_maint.aspx", "-", "-", "fin75_e2", "fin75_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F75127", 3, "Complaint Maint. Logs", 3, "../dir-maint-reps/om_view_maint.aspx", "-", "-", "fin75_e2", "fin75_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F75140", 2, "Maintenance Reports", 3, "-", "-", "Y", "fin75_e3", "fin75_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F75141", 3, "Section Wise B/Down Report", 3, "../dir-maint-reps/om_prt_maint.aspx", "-", "-", "fin75_e3", "fin75_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F75142", 3, "Depart. Wise B/Down Report", 3, "../dir-maint-reps/om_prt_maint.aspx", "-", "-", "fin75_e3", "fin75_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F75143", 3, "Machine Wise B/Down Report", 3, "../dir-maint-reps/om_prt_maint.aspx", "-", "-", "fin75_e3", "fin75_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F75144", 3, "Reason Wise B/Down Report", 3, "../dir-maint-reps/om_prt_maint.aspx", "-", "-", "fin75_e3", "fin75_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F75171", 2, "Maintenance Analysis", 3, "-", "-", "Y", "fin75_e4", "fin75_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F75172", 3, "Maintenance Dashboard", 3, "../tej-base/om_dbd_gendb.aspx", "-", "-", "fin75_e4", "fin75_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F75161", 2, "Maintenance Masters", 3, "-", "-", "Y", "fin75_e5", "fin75_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F75162", 3, "Maintenance Groups", 3, "../tej-base/om_typop_mst.aspx", "-", "-", "fin75_e5", "fin75_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F75165", 3, "Maintenance Machines", 3, "../dir-maint/om_maint_mach.aspx", "-", "-", "fin75_e5", "fin75_a1", "-", "fa-edit", "N", "Y");


                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0000", "Industry 01:Mldg/02:ShMetal/03:Casting/04:Forging/05:Prt/06:Corr/07:Paint/08:Pharma/09:Food/10:Capg.", "N", "2");

                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0011", "Print QR Code on Gate Inw", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0012", "Print QR Code on Purch Ord", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0013", "Print QR Code on Purch sch", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0014", "Print QR Code on M.R.R.", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0015", "Print QR Code on RGP/Chl", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0016", "Print QR Code on MRR.Tag", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0017", "Print QR Code on Prof.Inv", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0018", "Print QR Code on Sal.Order", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0019", "Print QR Code on Sale.Inv", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0020", "Print QR Code on Exp.PI", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0021", "Print QR Code on Exp.Ord", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0022", "Print QR Code on Exp.Inv", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0023", "Print QR Code on Payment Adv", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0024", "Print QR Code on Bal.Conf.Let", "N", "2");


                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0030", "OTP Mail Option during Login", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "07/02/2018", "DEV_A", "W0031", "OTP SMS Option during Login", "N", "2");
                    }

                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0018'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0018','DEV_A',GETDATE())");
                        add_icon(uniqQstr, "F99110", 3, "Dbd Display Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39119", 3, "Prodn Entry(Std)", 3, "../dir-prod/om_prod_bas.aspx", "-", "-", "fin39_e1", "fin39_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F39119Z", 3, "New Dashboard", 3, "../tej-base/om_dbd_gendb3.aspx", "-", "-", "fin39_e1", "fin39_a1", "-", "fa-edit");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "13/02/2018", "DEV_A", "W0032", "Request Based Issue System", "Y", "1");

                        add_icon(uniqQstr, "F15301", 3, "More Checklists(Purch.)", 3, "../tej-base/moreReports.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F15302", 4, "Pending PR Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15303", 4, "Pending PO Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15304", 4, "Pending Schedule (Day Wise) Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15305", 4, "Pending Schedule (Month Wise) Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15306", 4, "Pending Schedule (Item Wise) Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15307", 4, "Pending Schedule (Vendor Wise) Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15308", 4, "Closed PR Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15309", 4, "Closed PO Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15310", 4, "Cancelled PO Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F15311", 4, "PO Amendment History Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15312", 4, "Vendor Wise 12 Month Rates Trend Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15313", 4, "Item Wise 12 Month Rates Trend Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15314", 4, "PR vs PO Vs MRR Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15315", 4, "PO Delivery Date Vs Rcpt Date Checklist", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15316", 4, "PO Delivery Date Based Monthly Calender", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15317", 4, "Purchase Schedule Delivery Expected during DTD ", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15318", 4, "Purchase Orders Delivery Expected during DTD ", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F15319", 4, "Purchase Orders Nearing Validity Expiry ", 3, "../dir-purc-reps/om_view_purc.aspx", "-", "-", "fin15_e2", "fin15_a1", "fin15_MREPch", "fa-edit", "N", "Y");
                    }
                    //shuru
                    // made on 18.02.18 13.32 pkgg
                    mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0019'", "idno");
                    if (mhd == "0" || mhd == "")
                    {
                        execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0019','DEV_A',GETDATE())");

                        add_icon(uniqQstr, "F70118", 3, "Auto Debit Credit Note", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F15210", 3, "P.R. Closure", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e6", "fin25_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F15211", 3, "P.O. Closure", 3, "../tej-base/om_appr.aspx", "-", "-", "fin15_e6", "fin25_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F15245", 4, "Stock summary + Analysis", 3, "../tej-base/om_stk_Asys.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F30142", 3, "Inward QA Report", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e4", "fin30_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F30143", 3, "Inward QA Rejn Report", 3, "../tej-base/om_view_qa.aspx", "-", "-", "fin30_e4", "fin30_a1", "-", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F99119", 3, "Mails Config", 3, "../tej-base/om_mail_mgr.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F99120", 3, "Notification Config", 3, "../tej-base/om_mail_mgr.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F15117", 3, "Purchase Budget", 3, "../dir-purc/om_pur_budg.aspx", "-", "-", "fin15_e1", "fin15_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F70119", 3, "Expense Budget", 3, "../tej-base/om_exp_budg.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F47112", 3, "Sales Budget", 3, "../dir-smktg/om_sale_budg.aspx", "-", "-", "fin47_e1", "fin47_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F10227", 4, "Items Not Used in During DTD", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10228", 4, "Boms  Not Used in During DTD", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10229", 4, "List of Deactivated Items", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10230", 4, "List of Un Approved Items", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10231", 4, "List of Items With Selected Fields", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10232", 4, "List of BOMS With Selected Fields", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F10233", 4, "Items Without Min/Max/ROL", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10234", 4, "Min/Max/ROL of items", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10235", 4, "Similar Parent Code BOMs", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10236", 4, "Similar Child Code in same BOMs", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10237", 4, "List of FG Linked SF items without BOM", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10238", 4, "OSP BOM Search ", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10239", 4, "SF BOM Search ", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F10160", 3, "BOM Tree", 3, "../dir-engg-reps/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit", "N", "Y");

                        add_icon(uniqQstr, "F15137", 4, "Import Purchase Order Print", 3, "../dir-purc-reps/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                        add_icon(uniqQstr, "F50144", 3, "Domestic Proforma Invoice Print", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F49143", 3, "Export Proforma Invoice Print", 3, "../dir-emktg-reps/om_prt_emktg.aspx", "-", "-", "fin49_e4", "fin49_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F70136", 3, "Cheque Issue Register", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "N");
                        add_icon(uniqQstr, "F70137", 3, "Bank Reco. Print", 3, "../dir-acct-reps/om_prt_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "N");

                        // made on 18.02.18 13.32 pkgg

                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "18/02/2018", "DEV_A", "W0033", "Allow Item Repeat in BOM Entry", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "18/02/2018", "DEV_A", "W0034", "Allow Item Repeat in P.R Entry", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "18/02/2018", "DEV_A", "W0035", "Allow Item Repeat in P.O Entry", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "18/02/2018", "DEV_A", "W0036", "Allow Item Repeat in S.O Entry", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "18/02/2018", "DEV_A", "W0037", "Allow Item Repeat in Inv Entry", "N", "2");
                        save_SYSOPT(uniqQstr, coCd, "00", "OP", "18/02/2018", "DEV_A", "W0038", "Allow Item Repeat in Std Prod.", "N", "2");

                        add_icon(uniqQstr, "F39131", 3, "Prodn (Std) Checklist", 3, "../dir-prod-reps/om_view_prod.aspx", "-", "-", "fin39_e2", "fin39_a1", "-", "fa-edit", "N", "Y");


                        add_icon(uniqQstr, "F47161", 2, "Dom.Order Masters", 3, "-", "-", "-", "fin47_e6", "fin47_a1", "-", "fa-edit");
                        add_icon(uniqQstr, "F47162", 3, "S.O.Closure (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e6", "fin47_a1", "-", "fa-edit");

                        add_icon(uniqQstr, "F25245R", 4, "Return Sticker", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");
                    }
                }

                mhd = seek_iname(frm_qstr, frm_cocd, "select idno from ico_tab_UPD where trim(idno)='IC0021'", "idno");
                if (mhd == "0" || mhd == "")
                {
                    execute_cmd(frm_qstr, frm_cocd, "insert into ico_tab_UPD values ('IC0021','DEV_A',GETDATE())");

                    //------------------------------------------------------------------
                    // 04.03.18 pkgg icon for detailed qa.
                    execute_cmd(frm_qstr, frm_cocd, "update ico_tab set id='F30108',submenuid='fin30_e1' where text='QA Outward Template'");
                    add_icon(frm_qstr, "F30110", 2, "Quality Activity", 3, "-", "-", "Y", "fin30_e2", "fin30_a1", "-", "fa-edit");
                    add_icon(frm_qstr, "F30111", 3, "QA Inwards Cert.", 3, "../tej-base/om_qa_rpt.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit");
                    add_icon(frm_qstr, "F30112", 3, "QA In-Proc Cert.", 3, "../tej-base/om_qa_rpt.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit");
                    add_icon(frm_qstr, "F30113", 3, "QA Outward Cert.", 3, "../tej-base/om_qa_rpt.aspx", "-", "-", "fin30_e2", "fin30_a1", "-", "fa-edit");

                    execute_cmd(frm_qstr, frm_cocd, "update sys_config set frm_name='F30111' where frm_name='F30121'");

                    execute_cmd(frm_qstr, frm_cocd, "update ico_tab set submenuid='fin30_e3' where id in ('F30116','F30121','F30126','F30127')");
                    execute_cmd(frm_qstr, frm_cocd, "update ico_tab set submenuid='fin30_e4' where id in ('F30131','F30132','F30133','F30134')");
                    execute_cmd(frm_qstr, frm_cocd, "update ico_tab set submenuid='fin30_e5' where id in ('F30140','F30141','F30142','F30143')");
                    execute_cmd(frm_qstr, frm_cocd, "update ico_tab set submenuid='fin30_e6' where id in ('F30151','F30152','F30156')");

                    // 22.02.18 pkgg Form made by Twinkle
                    add_icon(frm_qstr, "F70179", 3, "Accounts Master", 3, "../tej-base/acc_gen1.aspx", "-", "-", "fin70_e5", "fin70_a1", "-", "fa-edit", "N", "Y");
                    ///
                    add_icon(frm_qstr, "F70240", 4, "Payment Advice", 3, "../tej-base/om_view_acct.aspx", "-", "-", "fin70_e4", "fin70_a1", "fin70_MREP", "fa-edit", "N", "N");
                    add_icon(frm_qstr, "F10233", 4, "Items Without Min/Max/ROL", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                    add_icon(frm_qstr, "F10234", 4, "Min/Max/ROL of items", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                    add_icon(frm_qstr, "F10235", 4, "Similar Parent Code BOMs", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                    add_icon(frm_qstr, "F10236", 4, "Similar Child Code in same BOMs", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                    add_icon(frm_qstr, "F10237", 4, "List of FG Linked SF items without BOM", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                    add_icon(frm_qstr, "F10238", 4, "OSP BOM Search ", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                    add_icon(frm_qstr, "F10239", 4, "SF BOM Search ", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "fin10_MREP", "fa-edit", "N", "Y");
                    add_icon(frm_qstr, "F10160", 3, "BOM Tree", 3, "../tej-base/om_view_engg.aspx", "-", "-", "fin10_e4", "fin10_a1", "-", "fa-edit", "N", "Y");

                    add_icon(frm_qstr, "F15137", 4, "Import Purchase Order Print", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");
                    add_icon(frm_qstr, "F50144", 3, "Domestic Proforma Invoice Print", 3, "../tej-base/om_prt_sale.aspx", "-", "-", "fin50_e4", "fin50_a1", "-", "fa-edit", "N", "N");
                    add_icon(frm_qstr, "F49143", 3, "Export Proforma Invoice Print", 3, "../tej-emktg-reps/om_prt_emktg.aspx", "-", "-", "fin49_e4", "fin49_a1", "-", "fa-edit", "N", "N");
                    add_icon(frm_qstr, "F70136", 3, "Cheque Issue Register", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "N");
                    add_icon(frm_qstr, "F70137", 3, "Bank Reco. Print", 3, "../tej-base/om_prt_acct.aspx", "-", "-", "fin70_e3", "fin70_a1", "-", "fa-edit", "N", "N");


                    // made on 18.02.18 13.32 pkgg
                    // 09.03.18

                    add_icon(frm_qstr, "F15246", 4, "Job No wise PR>PO>MRR", 3, "../tej-base/om_prt_purc.aspx", "-", "-", "fin15_e3", "fin15_a1", "fin15_MREP", "fa-edit", "N", "Y");


                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "18/02/2018", "DEV_A", "W0033", "Allow Item Repeat in BOM Entry", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "18/02/2018", "DEV_A", "W0034", "Allow Item Repeat in P.R Entry", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "18/02/2018", "DEV_A", "W0035", "Allow Item Repeat in P.O Entry", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "18/02/2018", "DEV_A", "W0036", "Allow Item Repeat in S.O Entry", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "18/02/2018", "DEV_A", "W0037", "Allow Item Repeat in Inv Entry", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "18/02/2018", "DEV_A", "W0038", "Allow Item Repeat in Std Prod.", "N", "2");

                    add_icon(frm_qstr, "F39131", 3, "Prodn (Std) Checklist", 3, "../tej-base/om_view_prod.aspx", "-", "-", "fin39_e2", "fin39_a1", "-", "fa-edit", "N", "Y");


                    add_icon(frm_qstr, "F47161", 2, "Dom.Order Masters", 3, "-", "-", "-", "fin47_e6", "fin47_a1", "-", "fa-edit");
                    add_icon(frm_qstr, "F47162", 3, "S.O.Closure (Dom.)", 3, "../tej-base/om_appr.aspx", "-", "-", "fin47_e6", "fin47_a1", "-", "fa-edit");


                    add_icon(frm_qstr, "F25245R", 4, "Return Sticker", 3, "../tej-base/om_view_invn.aspx", "-", "-", "fin25_e4", "fin25_a1", "fin25_MREP", "fa-edit", "N", "N");

                    execute_cmd(frm_qstr, frm_cocd, "update ico_tab set text='Day Wise Sales' where id='F05101'");
                    execute_cmd(frm_qstr, frm_cocd, "update ico_tab set text='Month Wise Sales',id='F05102' where id='F05106'");
                    execute_cmd(frm_qstr, frm_cocd, "update ico_tab set text='Plant Wise Sales',id='F05103' where id='F05111'");
                    execute_cmd(frm_qstr, frm_cocd, "update ico_tab set text='Dbd Config(TV)' where id='F99110'");
                    execute_cmd(frm_qstr, frm_cocd, "update ico_tab set text='ERP System Config' where id='F99117'");
                    add_icon(frm_qstr, "F99114", 3, "Plant Level Config", 3, "../tej-base/om_opt_mst_pw.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");

                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1001", "Rolling Freeze Days BOM", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1002", "Rolling Freeze Days Proc.Plan", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1003", "Rolling Freeze Days Stage Mapping", "N", "2");

                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1011", "Rolling Freeze Days P.R.", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1012", "Rolling Freeze Days P.O.", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1013", "Rolling Freeze Days P:Sch.", "N", "2");

                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1021", "Rolling Freeze Days G.Ent", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1022", "Rolling Freeze Days G.Out", "N", "2");

                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1031", "Rolling Freeze Days MRR", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1032", "Rolling Freeze Days CHL", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1033", "Rolling Freeze Days ISS", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1034", "Rolling Freeze Days RETU", "N", "2");

                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1041", "Rolling Freeze Days Q.A.(Basic)", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1042", "Rolling Freeze Days Q.A.(Templ)", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1043", "Rolling Freeze Days Q.A.(Report)", "N", "2");

                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1051", "Rolling Freeze Days Std.Prodn", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1052", "Rolling Freeze Days Adv.Prodn", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1053", "Rolling Freeze Days Stg.Tranfer", "N", "2");

                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1061", "Rolling Freeze Days P.I.", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1062", "Rolling Freeze Days Mst.S.O.", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1063", "Rolling Freeze Days Supply.S.O.", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1064", "Rolling Freeze Days Invoice", "N", "2");

                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1071", "Rolling Freeze Days Rcpts", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1072", "Rolling Freeze Days Pymts", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1073", "Rolling Freeze Days J.V.", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W1074", "Rolling Freeze Days P.V.", "N", "2");


                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2001", "Effective Date for Accounts", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2002", "Effective Date for Gate ", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2003", "Effective Date for Stores", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2004", "Effective Date for Reel Wise Stock", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2005", "Effective Date for Lot Wise Stock", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2006", "Effective Date for P.P.C.", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2007", "Effective Date for Production", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2015", "Currency For Branch", "N", "2");
                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W2016", "Comma Seprator[I]nd / [U]sa", "N", "2");
                    // ------------------------------------------------------------------

                    save_SYSOPT(frm_qstr, frm_cocd, "00", "OP", "04/03/2018", "DEV_A", "W0039", "Request Based Return System", "Y", "1");

                    add_icon(frm_qstr, "F05161", 2, "Stores MIS", 3, "-", "-", "Y", "fin05_e4", "fin05_a1", "-", "fa-edit");
                    add_icon(frm_qstr, "F05162", 3, "Inward Summary MIS", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e4", "fin05_a1", "-", "fa-edit", "N", "Y");
                    add_icon(frm_qstr, "F05165", 3, "Outward Summary MIS", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e4", "fin05_a1", "-", "fa-edit", "N", "Y");
                    add_icon(frm_qstr, "F05166", 3, "Issuance Summary MIS", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e4", "fin05_a1", "-", "fa-edit", "N", "Y");
                    add_icon(frm_qstr, "F05167", 3, "Returns Summary MIS", 3, "../tej-base/om_view_mis.aspx", "-", "-", "fin05_e4", "fin05_a1", "-", "fa-edit", "N", "Y");
                }

                add_icon(uniqQstr, "F41000", 1, "Production Module", 3, "-", "-", "Y", "-", "fin41_a1", "-", "fa-edit");
                add_icon(uniqQstr, "F41100", 2, "Prodn Activity", 3, "-", "-", "Y", "fin41_e1", "fin41_a1", "-", "fa-edit");
                add_icon(uniqQstr, "F41101", 3, "Prodn Entry", 3, "../dir-prod/prod_entry.aspx", "-", "-", "fin41_e1", "fin41_a1", "-", "fa-edit");

                add_icon(uniqQstr, "F35107", 3, "Machine Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "fin35_e1", "fin35_a1", "-", "fa-edit");

                add_icon(uniqQstr, "F10117", 3, "HSN Code Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");

                add_icon(frm_qstr, "F99151", 3, "Branch/Unit Master", 3, "../dir-sys-r/om_br_mst.aspx", "-", "-", "fin99_e4", "fin99_a1", "-", "fa-edit", "N", "Y");

                add_icon(frm_qstr, "F50208", 3, "Consignee Master", 3, "../dir-sales/om_csmst.aspx", "-", "-", "fin50_e6", "fin50_a1", "-", "fa-edit", "N", "Y");
                //
                add_icon(frm_qstr, "F10120", 3, "Upload Item Master", 3, "../tej-base/uplitemmst.aspx", "-", "-", "fin10_e1", "fin10_a1", "-", "fa-edit");

                break;

            case "MLGA":
            case "MSES":
            case "TEST**":

                //add_icon(uniqQstr, "P11000", 1, "PTS System", 3, "-", "-", "Y", "finpts_e", "finpts", "-", "fa-edit");
                add_icon(uniqQstr, "P11001", 1, "Project Tracking", 3, "-", "-", "Y", "finpts_e", "finptsa", "-", "fa-edit");
                add_icon(uniqQstr, "P11001A", 2, "Project Creation", 3, "../dir-mpa/om_task_mast.aspx", "-", "-", "finpts_e", "finptsa", "-", "fa-edit");
                add_icon(uniqQstr, "P11001C", 2, "Task Assignment", 3, "../dir-mpa/om_task_asgn.aspx", "-", "-", "finpts_e", "finptsa", "-", "fa-edit");
                add_icon(uniqQstr, "P11001E", 2, "Lead Managerial Hrs", 3, "../dir-mpa/frmLeadManage.aspx", "-", "-", "finpts_e", "finptsa", "-", "fa-edit");

                add_icon(uniqQstr, "P12001", 1, "Task Tracking", 3, "-", "-", "Y", "finpts_t", "finptstr", "-", "fa-edit");
                add_icon(uniqQstr, "P12001A", 2, "Time Tracking", 3, "../dir-mpa/om_task_Updt.aspx", "-", "-", "finpts_t", "finptstr", "-", "fa-edit");
                add_icon(uniqQstr, "P12001C", 2, "Task List", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_t", "finptstr", "-", "fa-edit");
                add_icon(uniqQstr, "P12001E", 2, "Leave Update", 3, "../dir-mpa/frmLeaveUpd.aspx", "-", "-", "finpts_t", "finptstr", "-", "fa-edit");

                add_icon(uniqQstr, "P13003", 1, "PTS Masters", 3, "-", "-", "Y", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003A", 2, "Business Units", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003B", 2, "Activity Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003C", 2, "Task Type Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003D", 2, "Designation Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003D1", 2, "Customer Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003E", 2, "Software Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003F", 2, "Documentation Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003F1", 2, "Documentation Status Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003G", 2, "Down Time Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003I", 2, "Status Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                //add_icon(uniqQstr, "P13003K", 2, "Assignor Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                //add_icon(uniqQstr, "P13003M", 2, "Assignee Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                //add_icon(uniqQstr, "P13003M1", 2, "Offload Assignee Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003N", 2, "Milestone Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003N1", 2, "Milestone Status Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003O", 2, "Department Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");
                add_icon(uniqQstr, "P13003Q", 2, "Proj.Category Master", 3, "../tej-base/om_typmast.aspx", "-", "-", "finpts_m", "finptsm", "-", "fa-edit");

                add_icon(uniqQstr, "P15005", 1, "PTS Reports", 3, "-", "-", "Y", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005A", 2, "Man Hour Utilization", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005B", 2, "Parameter Wise Summary Report", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005C", 2, "Billed Hours Report", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005D", 2, "Project Documentation Status", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005E", 2, "Resource Efficiency", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005G", 2, "Down Time Analysis", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005I", 2, "Project Status", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005K", 2, "Budget Vs Actual Hrs", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005M", 2, "Budget/Actual/Billed Hrs", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005M1", 2, "Milestone vs Actual Hrs", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005O", 2, "Productivity", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005Q", 2, "Profitability", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005S", 2, "Performance", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005T", 2, "Pending Activity Report", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005U", 2, "Estimate Vs Actual Hrs", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005W", 2, "Down Time Reason Analysis", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005X", 2, "Assignment Status Report", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005Y", 2, "Project budgeted/Actual Revenue", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");
                add_icon(uniqQstr, "P15005Z", 2, "Report Builder", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_r", "finptsr", "-", "fa-edit");

                add_icon(uniqQstr, "P17005", 1, "PTS MIS", 3, "-", "-", "Y", "finpts_s", "finptsmi", "-", "fa-edit");
                add_icon(uniqQstr, "P17005A", 2, "Dash Board (Client)", 3, "../tej-base/om_dboard.aspx", "-", "-", "finpts_s", "finptsmi", "-", "fa-edit");
                add_icon(uniqQstr, "P17005C", 2, "Dash Board (Overall)", 3, "../tej-base/om_dboard.aspx", "-", "-", "finpts_s", "finptsmi", "-", "fa-edit");
                add_icon(uniqQstr, "P17005E", 2, "Graph : Utilization", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_s", "finptsmi", "-", "fa-edit");
                add_icon(uniqQstr, "P17005G", 2, "Graph : Performance", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_s", "finptsmi", "-", "fa-edit");
                add_icon(uniqQstr, "P17005I", 2, "Graph : DownTime", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_s", "finptsmi", "-", "fa-edit");

                add_icon(uniqQstr, "P18005", 1, "PTS Logs", 3, "-", "-", "Y", "finpts_l", "finptsml", "-", "fa-edit");
                add_icon(uniqQstr, "P18005A", 2, "Project Log Book", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_l", "finptsml", "-", "fa-edit");
                add_icon(uniqQstr, "P18005C", 2, "Task Assigned Log Book", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_l", "finptsml", "-", "fa-edit");
                add_icon(uniqQstr, "P18005E", 2, "Task Reported Log Book", 3, "../tej-base/rpt_DevA.aspx", "-", "-", "finpts_l", "finptsml", "-", "fa-edit");


                add_icon(uniqQstr, "P19005", 1, "PTS Admin", 3, "-", "-", "Y", "finpts_a", "finptsadm", "-", "fa-edit");
                add_icon(uniqQstr, "P19005A", 2, "User Rights", 3, "../tej-base/urights.aspx", "-", "-", "finpts_a", "finptsadm", "-", "fa-edit");
                add_icon(uniqQstr, "M20016", 2, "Form Config", 3, "../tej-base/om_forms.aspx", "-", "-", "finpts_a", "finptsadm", "-", "fa-edit");
                add_icon(uniqQstr, "M20028", 2, "UDFs Config", 3, "../tej-base/om_forms.aspx", "-", "-", "finpts_a", "finptsadm", "-", "fa-edit");


                //////add_icon(uniqQstr, "M20001", 1, "System Controls", 1, "-", "-", "-", "-", "finsysmain", "-", "fa-edit");
                //////add_icon(uniqQstr, "M20016", 2, "Form Configurations", 1, "../tej-base/om_forms.aspx", "-", "-", "-", "finsysmain", "-", "fa-edit");
                //////add_icon(uniqQstr, "97000", 1, "System Admin", 1, "-", "-", "-", "-", "SYSAD", "-", "fa-group");

                //              break;

                //   case "MLGA":
                ////add_icon(uniqQstr, "M20011", 2, "OMSO", 1, "../dir-sales/om_so.aspx", "-", "-", "-", "finsysmain", "-", "fa-edit");

                ////add_icon(uniqQstr, "M20001", 1, "System Controls", 1, "-", "-", "-", "-", "finsysmain", "-", "fa-edit");
                ////add_icon(uniqQstr, "M20016", 2, "Form Configurations", 1, "../tej-base/om_forms.aspx", "-", "-", "-", "finsysmain", "-", "fa-edit");

                ////add_icon(uniqQstr, "S11001", 1, "MPA Module", 1, "-", "-", "-", "-", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S11005", 2, "MPA Entry", 3, "-", "-", "Y", "finmpa_e", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S11005A", 3, "Record Efforts", 3, "../dir-mpa/om_effort_Rec.aspx", "-", "-", "finmpa_e", "finmpa", "-", "fa-edit");

                ////add_icon(uniqQstr, "S13008", 2, "MPA Masters", 3, "-", "-", "Y", "finmpa_m", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S13008A", 3, "Customer Master", 3, "../dir-mpa/om_types.aspx", "-", "-", "finmpa_m", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S13008B", 3, "Employee Master", 3, "../dir-mpa/om_types.aspx", "-", "-", "finmpa_m", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S13008C", 3, "Efforts Master", 3, "../dir-mpa/om_types.aspx", "-", "-", "finmpa_m", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S13008D", 3, "Cust. Effort Target", 3, "../dir-mpa/om_wrk_link.aspx", "-", "-", "finmpa_m", "finmpa", "-", "fa-edit");

                ////add_icon(uniqQstr, "S15115", 2, "MPA Reports", 3, "-", "-", "Y", "finmpa_r", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S15115A", 3, "Customer Effort Summary", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S15115B", 3, "Customer Employee Effort", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S15115C", 3, "Employee Effort Summary", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S15115D", 3, "Customer Monthly Effort", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S15115D1", 3, "All Master Form", 3, "../tej-base/allMaster.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S15115E", 3, "Customer Employee Monthly Effort", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S15115F", 3, "Target Vs Actual Effort", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S15115I", 3, "D/D Sale Report", 3, "../tej-base/rpt.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");

                ////add_icon(uniqQstr, "97000", 1, "System Admin", 1, "-", "-", "-", "-", "SYSAD", "-", "fa-group");
                ////add_icon(uniqQstr, "S15115G", 3, "Client Dashboard (Client)", 3, "../tej-base/om_dboard.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");
                ////add_icon(uniqQstr, "S15115H", 3, "Dashboard (MLG)", 3, "../tej-base/om_dboard.aspx", "-", "-", "finmpa_r", "finmpa", "-", "fa-edit");

                //add_icon(uniqQstr, "97000", 1, "System Admin", 1, "-", "-", "-", "-", "SYSAD", "-", "fa-group");
                //add_icon(uniqQstr, "97010", 2, "User Rights", 1, "../tej-base/urights.aspx", "-", "-", "-", "SYSAD", "SYSADM", "-");

                break;
            case "PPAP":
            case "PTI*":
            case "TGIP":
                add_icon(uniqQstr, "P70000", 1, "Finance", 3, "-", "-", "Y", "finfina_r", "finfina_r", "-", "fa-edit");
                add_icon(uniqQstr, "P70100", 2, "Accounting Activity", 3, "-", "-", "Y", "finfina_r1", "finfina_r", "-", "fa-edit");
                add_icon(uniqQstr, "P70106C", 3, "Credit Note", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "finfina_r1", "finfina_r", "-", "fa-edit", "Y", "N");
                add_icon(uniqQstr, "P70106D", 3, "Debit Note", 3, "../dir-acct-reps/om_view_acct.aspx", "-", "-", "finfina_r1", "finfina_r", "-", "fa-edit", "Y", "N");

                add_icon(uniqQstr, "P70099", 2, "Maruti Inv File Uploading", 3, "../tej-base/fupl.aspx", "-", "Y", "finfina_r", "finfina_r", "-", "fa-edit");
                add_icon(uniqQstr, "P70099a", 2, "Toyota/All Inv File Uploading", 3, "../tej-base/fupl1.aspx", "-", "Y", "finfina_r", "finfina_r", "-", "fa-edit");

                add_icon(uniqQstr, "F50000", 1, "Sales & Marketing", 3, "-", "-", "Y", "finsmktg_s", "finsmktg", "-", "fa-edit");
                add_icon(uniqQstr, "F50035", 2, "ISD Invoice Entry", 3, "../dir-smktg/om_inv.aspx", "-", "-", "finsmktg_s", "finsmktg", "-", "fa-edit");
                add_icon(uniqQstr, "F99100", 1, "System Admin", 3, "-", "-", "Y", "fin99_e", "fin99_a", "-", "fa-edit");
                add_icon(uniqQstr, "F99101", 2, "Form Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e", "fin99_a", "-", "fa-edit");
                add_icon(uniqQstr, "F99106", 2, "UDFs Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e", "fin99_a", "-", "fa-edit");
                add_icon(uniqQstr, "F99111", 2, "Reps Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e", "fin99_a", "-", "fa-edit");
                break;
            case "ANYG":
                mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0001'", "idno");
                if (mhd == "0" || mhd == "")
                {
                    execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0001','DEV_A',GETDATE())");
                    // ------------------------------------------------------------------
                    // Gate Inward, Outward
                    // ------------------------------------------------------------------
                    add_icon(uniqQstr, "F20000", 1, "Gate Module", 3, "-", "-", "Y", "-", "fin20_a1", "-", "fa-edit");
                    add_icon(uniqQstr, "F20100", 2, "Gate Activity", 3, "-", "-", "Y", "fin20_e1", "fin20_a1", "-", "fa-edit");
                    add_icon(uniqQstr, "F20101", 3, "Gate Inward Entry", 3, "../dir-gate/om_gate_inw.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");
                    //>> tag pring reqd 
                    add_icon(uniqQstr, "F20106", 3, "Gate Outward Entry", 3, "../dir-gate/om_gate_outw.aspx", "-", "-", "fin20_e1", "fin20_a1", "-", "fa-edit");
                    //>> Scan Option Reqd

                    add_icon(uniqQstr, "F20116", 2, "Gate Checklists", 3, "-", "-", "Y", "fin20_e2", "fin20_a1", "-", "fa-edit");
                    add_icon(uniqQstr, "F20121", 3, "Gate Inward Checklist", 3, "../dir-gate-reps/om_view_gate.aspx", "-", "Y", "fin20_e2", "fin20_a1", "-", "fa-edit", "N", "Y");
                    add_icon(uniqQstr, "F20126", 3, "Gate Outward Checklist", 3, "../dir-gate-reps/om_view_gate.aspx", "-", "Y", "fin20_e2", "fin20_a1", "-", "fa-edit", "N", "Y");
                    add_icon(uniqQstr, "F20127", 3, "Gate PO Checklist", 3, "../dir-gate-reps/om_view_gate.aspx", "-", "Y", "fin20_e2", "fin20_a1", "-", "fa-edit", "N", "Y");
                    add_icon(uniqQstr, "F20128", 3, "Gate RGP Checklist", 3, "../dir-gate-reps/om_view_gate.aspx", "-", "Y", "fin20_e2", "fin20_a1", "-", "fa-edit", "N", "Y");

                    add_icon(uniqQstr, "F20131", 2, "Gate Reports", 3, "-", "-", "Y", "fin20_e3", "fin20_a1", "-", "fa-edit");
                    add_icon(uniqQstr, "F20132", 3, "Gate Inward Register", 3, "../dir-gate-reps/om_prt_gate.aspx", "-", "Y", "fin20_e3", "fin20_a1", "-", "fa-edit", "N", "Y");
                    add_icon(uniqQstr, "F20133", 3, "Gate Outward Register", 3, "../dir-gate-reps/om_prt_gate.aspx", "-", "Y", "fin20_e3", "fin20_a1", "-", "fa-edit", "N", "Y");

                }
                break;

            case "SFLG":
                add_icon(uniqQstr, "15000", 1, "Manufacturing", 1, "-", "-", "-", "-", "MANU", "-", "-", "N", "N");
                add_icon(uniqQstr, "15100", 2, "Reports", 1, "-", "-", "-", "REP", "MANU", "REPT", "-", "N", "N");
                add_icon(uniqQstr, "15192", 3, "Production Schedule Status", 1, "../tej-base/om_view_rpt_M1_reps.aspx", "-", "-", "REP", "MANU", "REPT", "-", "N", "N");
                add_icon(uniqQstr, "15193", 3, "Plan / Daily Production Report", 1, "../tej-base/om_view_rpt_M1_reps.aspx", "-", "-", "REP", "MANU", "REPT", "-", "N", "Y");
                add_icon(uniqQstr, "15194", 3, "Jobwork Pending List Item Wise", 1, "../tej-base/om_view_rpt_M1_reps.aspx", "-", "-", "REP", "MANU", "REPT", "-", "N", "N");
                add_icon(uniqQstr, "15195", 3, "Jobwork Pending Summary(Detail)", 1, "../tej-base/om_view_rpt_M1_reps.aspx", "-", "-", "REP", "MANU", "REPT", "-", "N", "N");
                add_icon(uniqQstr, "15196", 3, "Jobwork Pending Summary", 1, "../tej-base/om_view_rpt_M1_reps.aspx", "-", "-", "REP", "MANU", "REPT", "-", "N", "N");
                add_icon(uniqQstr, "15197", 3, "Jobwork Pending Summary Challan Wise", 1, "../tej-base/om_view_rpt_M1_reps.aspx", "-", "-", "REP", "MANU", "REPT", "-", "N", "N");
                break;
            case "LOGW":
            case "MEGA":
                add_icon(uniqQstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                add_icon(uniqQstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                add_icon(uniqQstr, "P70099", 3, "Maruti Inv File Uploading", 3, "../tej-base/fupl.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                add_icon(uniqQstr, "F70118", 3, "Auto Debit Credit Note", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                break;
            case "BONY":
                add_icon(uniqQstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                add_icon(uniqQstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                add_icon(uniqQstr, "F70118", 3, "Auto Debit Credit Note", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                add_icon(uniqQstr, "P70099a", 3, "Toyota/All Inv File Uploading", 3, "../tej-base/fupl1.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                break;
            case "SKHM":
                add_icon(uniqQstr, "F70000", 1, "Finance/Acctg Module", 1, "-", "-", "-", "-", "fin70_a1", "-", "fa-edit");
                add_icon(uniqQstr, "F70100", 2, "Accounting Entries", 3, "-", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                add_icon(uniqQstr, "P70099", 3, "Maruti Inv File Uploading", 3, "../tej-base/fupl.aspx", "-", "Y", "fin70_e1", "fin70_a1", "-", "fa-edit");
                add_icon(uniqQstr, "F70118", 3, "Auto Debit Credit Note", 3, "../tej-base/findDrCr.aspx", "-", "-", "fin70_e1", "fin70_a1", "-", "fa-edit");
                break;
            case "MEGH":
                // ------------------------------------------------------------------
                // Inventory Module
                // ------------------------------------------------------------------
                add_icon(uniqQstr, "F25000", 1, "Inventory Module", 3, "-", "-", "Y", "-", "fin25_a1", "-", "fa-edit");
                add_icon(uniqQstr, "F25211", 2, "Stock Reporting", 3, "-", "-", "Y", "fin25_e3", "fin25_a1", "-", "fa-edit");
                break;
        }

        // for all 
        //add_icon(uniqQstr, "F99100", 1, "System Admin", 1, "-", "-", "-", "-", "fin99_a", "-", "fa-edit");
        //add_icon(uniqQstr, "F99101", 2, "Form Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e", "fin99_a", "-", "fa-edit");
        //add_icon(uniqQstr, "F99106", 2, "UDFs Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e", "fin99_a", "-", "fa-edit");
        //add_icon(uniqQstr, "F99111", 2, "Reps Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e", "fin99_a", "-", "fa-edit");
        //add_icon(uniqQstr, "F99116", 2, "Dbd Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e", "fin99_a", "-", "fa-edit");
        //add_icon(uniqQstr, "F99117", 2, "Opts Config", 3, "../tej-base/om_opt_mst.aspx", "-", "-", "fin99_e", "fin99_a", "-", "fa-edit");
        //add_icon(uniqQstr, "F99118", 2, "View Log File", 3, "../tej-base/logView.aspx", "-", "-", "fin99_e", "fin99_a", "-", "fa-edit");

        mhd = seek_iname(uniqQstr, coCd, "select idno from ICO_TAB_UPD where trim(idno)='IC0011'", "idno");
        if (mhd == "0" || mhd == "")
        {
            execute_cmd(coCd, "insert into ICO_TAB_UPD values ('IC0011','DEV_A',GETDATE())");

            execute_cmd(coCd, "delete from ico_tab where id in ('F70106','F99100','F99101','F99106','F99111','F99116','F99117','F99118') ");
            // ------------------------------------------------------------------
            // System Admin Options
            // ------------------------------------------------------------------
            add_icon(uniqQstr, "F99000", 1, "System Admin", 3, "-", "-", "Y", "-", "fin99_a1", "-", "fa-edit");
            add_icon(uniqQstr, "F99100", 2, "System Settings", 3, "-", "-", "Y", "fin99_e1", "fin99_a1", "-", "fa-edit");
            add_icon(uniqQstr, "F99101", 3, "Form Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            add_icon(uniqQstr, "F99106", 3, "UDFs Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            add_icon(uniqQstr, "F99111", 3, "Reps Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            add_icon(uniqQstr, "F99116", 3, "Dbd Config", 3, "../tej-base/om_forms.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            add_icon(uniqQstr, "F99117", 3, "Opts Config", 3, "../tej-base/om_opt_mst.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");
            add_icon(uniqQstr, "F99118", 3, "View Logs", 3, "../tej-base/logView.aspx", "-", "-", "fin99_e1", "fin99_a1", "-", "fa-edit");

            add_icon(uniqQstr, "F99121", 2, "System Reports", 3, "-", "-", "Y", "fin99_e2", "fin99_a1", "-", "fa-edit");
            add_icon(uniqQstr, "F99126", 3, "New Items Opened", 3, "../dir-sys-reps/om_prt_sys.aspx", "-", "-", "fin99_e2", "fin99_a1", "-", "fa-edit", "N", "Y");
            add_icon(uniqQstr, "F99127", 3, "New A/cs Opened", 3, "../dir-sys-reps/om_prt_sys.aspx", "-", "-", "fin99_e2", "fin99_a1", "-", "fa-edit", "N", "Y");
            add_icon(uniqQstr, "F99128", 3, "Item Master Edited", 3, "../dir-sys-reps/om_prt_sys.aspx", "-", "-", "fin99_e2", "fin99_a1", "-", "fa-edit", "N", "Y");
            add_icon(uniqQstr, "F99129", 3, "A/c Master Edited", 3, "../dir-sys-reps/om_prt_sys.aspx", "-", "-", "fin99_e2", "fin99_a1", "-", "fa-edit", "N", "Y");
            add_icon(uniqQstr, "F99130", 3, "More Reports(System)", 3, "../tej-base/moreReports.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "N");

            add_icon(uniqQstr, "F99231", 4, "Data Entry Stats (Purchase)", 3, "../dir-sys-reps/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            add_icon(uniqQstr, "F99232", 4, "Data Entry Stats (Stores)", 3, "../dir-sys-reps/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            add_icon(uniqQstr, "F99233", 4, "Data Entry Stats (Sales)", 3, "../dir-sys-reps/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            add_icon(uniqQstr, "F99234", 4, "Data Entry Stats (Accounts)", 3, "../dir-sys-reps/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            add_icon(uniqQstr, "F99235", 4, "Data Entry Stats (Production)", 3, "../dir-sys-reps/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            add_icon(uniqQstr, "F99241", 4, "Who Did What", 3, "../dir-sys-reps/om_prt_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            add_icon(uniqQstr, "F99242", 4, "Similar Name Accounts", 3, "../dir-sys-reps/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");
            add_icon(uniqQstr, "F99243", 4, "Similar Name Items", 3, "../dir-sys-reps/om_view_sys.aspx", "-", "-", "fin99_e2", "fin99_a2", "fin99_MREP", "fa-edit", "N", "Y");

            add_icon(uniqQstr, "F99140", 2, "System Tracking", 3, "-", "-", "Y", "fin99_e3", "fin99_a1", "-", "fa-edit");
            add_icon(uniqQstr, "F99141", 3, "ERP Sessions", 3, "../dir-sys-reps/om_view_sys.aspx", "-", "-", "fin99_e3", "fin99_a1", "-", "fa-edit", "N", "Y");
            add_icon(uniqQstr, "F99142", 3, "ERP Tracking", 3, "../dir-sys-reps/om_view_sys.aspx", "-", "-", "fin99_e3", "fin99_a1", "-", "fa-edit", "N", "Y");
            add_icon(uniqQstr, "F99143", 3, "Locate Options", 3, "../dir-sys-reps/om_view_sys.aspx", "-", "-", "fin99_e3", "fin99_a1", "-", "fa-edit", "N", "Y");
        }
    }


    public string next_no(string Qstr, string co_Cd, string squery, int limit, string col1)
    {
        Int64 i = 0;
        string count = "", result = "";
        using (DataTable dt_Rows = getdata(co_Cd, squery))
        {
            if (dt_Rows.Rows.Count > 0) count = dt_Rows.Rows[0]["" + col1 + ""].ToString().Trim();
            else count = "0";
            if (count.Trim() == "" || count.Trim() == "0" || count.Trim() == "-")
            {
                i = 1;
            }
            else
            {

                try
                {
                    i = Convert.ToInt64(count);
                    i++;
                }
                catch
                {
                    i = 100001;
                }

            }
            result = padlc(i, limit);
        }
        return result;
    }

    public DataTable fill_icon_grid(string _co_Cd, string _tab_name, string _cond, string q_str)
    {
        if (dt_menu.Rows.Count > 0 && dt_menu.TableName == q_str) { }
        else
        {
            if (_cond.Length > 2) _cond = "where " + _cond;
            dt_menu = new DataTable();
            dt_menu = getdata(_co_Cd, "select * from " + _tab_name + " " + _cond + " ORDER BY ID");
            dt_menu.TableName = q_str;
        }
        return dt_menu;
    }
    public string getOption(string Qstr, string cocd, string optName, string variable)
    {
        string ReturnVal = "";
        ReturnVal = seek_iname(Qstr, cocd, "SELECT " + variable + " FROM FIN_RSYS_OPT WHERE OPT_ID='" + optName + "'", variable);
        return ReturnVal.Trim();
    }
    public string Fn_chk_can_edit(string Qstr, string co_cd, string userid, string formid)
    {
        urights = seek_iname(Qstr, co_cd, "SELECT RCAN_EDIT FROM ICO_WTAB WHERE USERID='" + userid + "' and ID='" + formid + "'", "RCAN_EDIT");
        if (urights == "N") urights = "N";
        else urights = "Y";
        return urights;
    }
    public string Fn_chk_can_add(string Qstr, string co_cd, string userid, string formid)
    {
        urights = seek_iname(Qstr, co_cd, "SELECT RCAN_add FROM ICO_WTAB WHERE USERID='" + userid + "' and ID='" + formid + "'", "RCAN_add");
        if (urights == "N") urights = "N";
        else urights = "Y";
        return urights;
    }
    //public void open_grid(string title, string query, int seektype, string mvc)
    //{

    //    //string url = FindUrl("footable_v8.aspx");
    //    string url = "/foo/footable_v2";
    //    HttpContext.Current.Session[MyGuid + "_pageurl"] = url;
    //    if (url.Equals("")) { showmsg(1, "Page Not Found", 0); return; }
    //    if (query.Trim().Length < 5)
    //    {
    //        showmsg(1, "Please Put Right Command", 2);
    //        HttpContext.Current.Session[MyGuid + "_basedtquery"] = "";
    //        return;
    //    }
    //    HttpContext.Current.Session[MyGuid + "_filename"] = title;
    //    HttpContext.Current.Session[MyGuid + "_basedtquery"] = query;
    //    HttpContext.Current.Session[MyGuid + "_SEEKLIMIT"] = 9999999999;
    //    HttpContext.Current.Session[MyGuid + "_SHOWSAVE"] = false;
    //    HttpContext.Current.Session[MyGuid + "_TEMPID"] = "-";
    //    if (seektype == 0) HttpContext.Current.Session[MyGuid + "_SEEKTYPE"] = 0;
    //    else HttpContext.Current.Session[MyGuid + "_SEEKTYPE"] = 2;
    //    HttpContext.Current.Session[MyGuid + "_CHECKTYPE"] = seektype;


    //    //if (HttpContext.Current.CurrentHandler is Page)
    //    //{
    //    Page p = (Page)HttpContext.Current.CurrentHandler;
    //    ScriptManager.RegisterClientScriptBlock(p, p.GetType(), "PopUP", "OpenSingle('../../.." + url + "','80%','800px','" + title + "');", true);
    //    //ScriptManager.RegisterClientScriptBlock(p, p.GetType(), "PopUP", "OpenSingle('../../../../../erp/dashboard.aspx?mid=YDCKmcdznzA=','90%','750px','" + title + "');", true);


    //    //}
    //}

    public string Fn_curr_dt(string Pco_Cd, string Pqstr = "")
    {
        if (Pqstr == "") Pqstr = MyGuid;
        string rdate = "";
        string xdate = seek_iname(Pqstr, Pco_Cd, "Select to_char(sysdate,'dd/MM/yyyy') as fstr from dual", "fstr");
        string xcdt2 = Multiton.Get_Mvar(Pqstr, "U_CDT2");
        try
        {
            if (Convert.ToDateTime(xdate) > Convert.ToDateTime(xcdt2))
                rdate = xcdt2;
            else
                rdate = xdate;
        }
        catch { }
        return rdate;
    }
    public string seekval(string usercode, string Squery, string Seek_Val1)
    {
        string ReturnVal = "";
        using (DataTable dt_Rows = getdata(usercode, Squery))
        {
            if (dt_Rows.Rows.Count > 0)
            {
                if (dt_Rows.Rows[0][Seek_Val1].ToString().Trim().Length > 0) ReturnVal = dt_Rows.Rows[0][Seek_Val1].ToString().Trim();
                else ReturnVal = "0";
            }
            else ReturnVal = "0";
        }
        return ReturnVal.Trim();
    }

    public string seekval_dt(string usercode, DataTable dtsearch, string searchcolumn, string filtervalue, string Seek_Val1)
    {
        string ReturnVal = "";
        var result = from r in dtsearch.AsEnumerable()
                     where r.Field<string>(searchcolumn) == filtervalue
                     //&&  r.Field<string>("Name") != ""
                     select r;
        DataTable dtResult = result.CopyToDataTable();
        ReturnVal = dtResult.Rows[0][Seek_Val1].ToString();
        return ReturnVal;


    }
    public DataSet Get_SP2Q(string userCode, String MQ1, String MQ2)
    {
        OracleConnection fCon = new OracleConnection(ConnInfo.connString(userCode));
        //fCon.Open();
        OracleCommand cmd = new OracleCommand("SP_FOO", fCon);
        //cmd.CommandType = CommandType.StoredProcedure;
        //OracleParameter mq1 = new OracleParameter();
        //mq1.ParameterName = "mq1";
        //mq1.Value = MQ1;
        //mq1.OracleDbType = OracleDbType.NVarChar;
        //mq1.Direction = ParameterDirection.Input;
        //OracleParameter mq2 = new OracleParameter();
        //mq2.ParameterName = "mq2";
        //mq2.Value = MQ2;
        //mq2.OracleDbType = OracleDbType.NVarChar;
        //mq2.Direction = ParameterDirection.Input;
        //OracleParameter Cur1 = new OracleParameter();
        //Cur1.ParameterName = "cursor1";
        //Cur1.OracleDbType = OracleDbType.RefCursor;
        //Cur1.Direction = ParameterDirection.Output;
        //OracleParameter Cur2 = new OracleParameter();
        //Cur2.ParameterName = "cursor2";
        //Cur2.OracleDbType = OracleDbType.RefCursor;
        //Cur2.Direction = ParameterDirection.Output;
        //cmd.Parameters.Add(mq1);
        //cmd.Parameters.Add(mq2);
        //cmd.Parameters.Add(Cur1);
        //cmd.Parameters.Add(Cur2);
        //Return the filled Dataset
        DataSet dataSet = new DataSet();
        var adapter = new OracleDataAdapter(cmd);
        try
        {
            adapter.Fill(dataSet);
        }
        catch (Exception err)
        {

        }
        fCon.Close();
        fCon.Dispose();
        return dataSet;
    }


    public string chk_co(string co_cd)
    {
        string fname = "";
        switch (co_cd.Trim())
        {
            case "HIME"://1                
                fname = "HIMALAYA EXPORTS";
                break;
            case "KRS"://1                
                fname = "KRSM Limited";
                break;
            case "ROTO"://1                
                fname = "SHREERAJ ROTO INDIA LTD.";
                break;
            case "SGRP"://1                
                fname = "SHREERAJ ROTO INDIA LTD.";
                break;
            default:
                fname = "XXXX";
                break;
        }
        //fname = "XXXXXXXXXXX";
        return fname;
    }

}
