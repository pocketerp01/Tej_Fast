using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Oracle.ManagedDataAccess.Client;
using System.Net.Mail;
using System.IO;

//ACTION  LOCAL ID ON WPPL BKUP FOR RUNNIG

public partial class Dak1_task : System.Web.UI.Page
{
    string btnval, SQuery, co_cd, uname, col1, col2, col3, mbr, cstr, vchnum, vardate, fromdt, todt, DateRange, year, ulvl, merr = "0";
    string path = @"c:\TEJ_ERP\email_info.txt"; string str = "", xvip = "1", xport = "587", sender_id = "", pwd = "", vsmtp = "", Bcc, Cc, to = "", subject, htmbody, sQUERY;
    SmtpClient smtp; MailMessage mail; int ssl = 0, port = 0;
    string query, headername, acoder, vty, HCID, m1, m2, ulevel, mlvl, tabname, pk_error, cdt1, cdt2, tco_cd;
    string[] mul;
    string val, value1, value2, value3, xprdrange, branch_Cd, xprd1, cldt, cDT1, cDT2, xprdrange1, xprd2, sysdat, currdate;
    DataTable dt, dt1; DataRow oporow;
    fgenDB fgen = new fgenDB();
    string frm_url, frm_qstr, frm_formID;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null) Response.Redirect("~/login.aspx");
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
                    co_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COCD");
                    uname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    ulvl = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    ulevel = fgenMV.Fn_Get_Mvar(frm_qstr, "U_ULEVEL");
                    mbr = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MBR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                }
                else Response.Redirect("~/login.aspx");
            }
            if (!Page.IsPostBack)
            {
                fgen.DisableForm(this.Controls);
                enablectrl(); btnnew.Focus();
            }
            vardate = DateTime.Now.ToString("dd/MM/yyyy");
        }
    }
    public void enablectrl()
    {
        btnnew.Disabled = false; btnedit.Disabled = false; btnsave.Disabled = true; btndel.Disabled = false;//btnList.Disabled = false;
        btnext.Text = " Exit "; btnext.Enabled = true; btnhideF.Enabled = true; btnhideF_s.Enabled = true; //imguserid.Enabled = false;
    }
    public void disablectrl()
    {
        btnnew.Disabled = true; btnedit.Disabled = true; btnsave.Disabled = false; btndel.Disabled = true;//btnList.Disabled = true;
        btnext.Text = "Cancel"; btnext.Enabled = true; btnhideF.Enabled = true; btnhideF_s.Enabled = true; //imguserid.Enabled = true;
    }
    public void clearctrl()
    {
        hffield.Value = ""; edmode.Value = "";
    }
    protected void btnnew_ServerClick(object sender, EventArgs e)
    {
        col1 = fgen.check_filed_name(frm_qstr, co_cd, "SCRATCH2", "COL48");
        if (col1 == "0")
        {
            fgen.execute_cmd(frm_qstr, co_cd, "ALTER TABLE SCRATCH2 ADD COL48 DATE DEFAULT SYSDATE");
        }
        clearctrl();
        hffield.Value = "New";
        disp_data();
        fgen.Fn_open_sseek("Select Your Task for Take Action", frm_qstr);
    }
    protected void btnedit_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Edit";
        disp_data();
        fgen.Fn_open_sseek("Edit Your Task", frm_qstr);
    }
    protected void btnsave_ServerClick(object sender, EventArgs e)
    {
        if (txtreason.Text == "" || txtreason.Text == "-")
        {
            fgen.msg("-", "AMSG", "Please Enter Action against the Task First!!"); return;
        }
        fgen.msg("-", "SMSG", "Are you sure, you want to Save!!");
        return;
    }
    protected void btndel_ServerClick(object sender, EventArgs e)
    {
        clearctrl();
        hffield.Value = "Del";
        disp_data();
        fgen.Fn_open_sseek("Delete Your Task", frm_qstr);
    }
    protected void btnext_Click(object sender, EventArgs e)
    {
        if (btnext.Text == " Exit ")
        { Response.Redirect("~/tej-base/desktop.aspx?STR=" + frm_qstr); }
        else
        {
            fgen.ResetForm(this.Controls);
            fgen.DisableForm(this.Controls);
            clearctrl();
            enablectrl();
        }
    }
    protected void btnhideF_Click(object sender, EventArgs e)
    {
        btnval = hffield.Value;
        if (hffield.Value == "D")
        {
            col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
            if (col1 == "Y")
            {
                fgen.execute_cmd(frm_qstr, co_cd, "delete from scratch2 where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + edmode.Value + "' AND ENT_BY='" + uname + "'");
                fgen.msg("-", "AMSG", "Details are deleted for Task No. " + edmode.Value.Substring(4, 6) + "");
                clearctrl(); fgen.ResetForm(this.Controls);                
            }
        }
        else
        {
            //if (Request.Cookies["Value1"].Value != null || Request.Cookies["Value2"].Value != null || Request.Cookies["Value3"].Value != null)
            {
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
                col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

                switch (btnval)
                {
                    #region
                    case "New":
                        if (col1.Length <= 1) return;
                        query = "select branchcd,VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE, ACODE,COL1 AS USERNAME,COL2 AS MAIL,COL4 AS PRIORITY,col5 as cc,COL14 AS SUBJECT,ENT_BY,TO_CHAR(ENT_DT,'DD/MM/YYYY') AS ENT_DT,REMARKS AS TEXT,to_char(DOCDATE,'dd/MM/yyyy') AS TASKDATE,TO_CHAR(EDT_DT,'DD/MM/YYYY') AS EDT_DT,EDT_BY FROM SCRATCH  where   branchcd='" + mbr + "' and type='DK' /*and ent_by='" + uname.Trim() + "'*/ and  branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(acode)='" + col1 + "'";
                        dt = fgen.getdata(frm_qstr, co_cd, query);
                        fgen.EnableForm(this.Controls); disablectrl();
                        txtvchnew.Text = fgen.next_no(frm_qstr, co_cd, "select max(vchnum) as vch from scratch2 where type='TA' and branchcd='" + mbr + "'", 6, "vch");
                        txtdate.Text = vardate; //txttskdate.Text = vardate;
                        txtredate.Text = vardate;
                        if (dt.Rows.Count > 0)
                        {
                            txtvchdate.Text = dt.Rows[0]["vchdate"].ToString().Trim();
                            txtvchnum.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                            txtuserid.Text = dt.Rows[0]["ACODE"].ToString().Trim();
                            txtsubject.Text = dt.Rows[0]["subject"].ToString().Trim();
                            txtemailcc.Text = dt.Rows[0]["cc"].ToString().Trim();
                            txtmsg.Text = dt.Rows[0]["TEXT"].ToString().Trim();
                            txttskdate.Text = dt.Rows[0]["TASKDATE"].ToString().Trim();
                            txtdrop.Text = dt.Rows[0]["PRIORITY"].ToString().Trim();
                            txtAssign.Text = dt.Rows[0]["ENT_by"].ToString().Trim();
                            txtAssignDt.Text = dt.Rows[0]["ENT_DT"].ToString().Trim();
                        }
                        break;
                    #endregion
                    case "Edit":
                        if (col1.Length <= 1) return;
                        clearctrl();
                        dt = new DataTable();
                        m2 = "";
                        m2 = "select * from scratch2 where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' and ent_by='" + uname.Trim() + "'";
                        dt = fgen.getdata(frm_qstr, co_cd, "select * from scratch2 where branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')='" + col1 + "' and ent_by='" + uname.Trim() + "'");
                        if (dt.Rows.Count > 0)
                        {
                            txtvchnew.Text = dt.Rows[0]["vchnum"].ToString().Trim();
                            txtdate.Text = Convert.ToDateTime(dt.Rows[0]["vchdate"].ToString()).ToString("dd/MM/yyyy").Trim();
                            txtvchnum.Text = dt.Rows[0]["col6"].ToString().Trim();
                            txtvchdate.Text = Convert.ToDateTime(dt.Rows[0]["col48"].ToString()).ToString("dd/MM/yyyy").Trim();
                            txtuserid.Text = dt.Rows[0]["col2"].ToString().Trim();
                            txttskdate.Text = Convert.ToDateTime(dt.Rows[0]["DOCDATE"].ToString()).ToString("dd/MM/yyyy").Trim();
                            txtsubject.Text = dt.Rows[0]["COL14"].ToString().Trim();
                            txtmsg.Text = dt.Rows[0]["REMARKS"].ToString().Trim();
                            txtreason.Text = dt.Rows[0]["col5"].ToString().Trim();
                            txtdrop.Text = dt.Rows[0]["col4"].ToString().Trim();
                            txtredate.Text = dt.Rows[0]["col7"].ToString().Trim();
                            txtreason.Text = dt.Rows[0]["reason"].ToString().Trim();
                            txtemailcc.Text = dt.Rows[0]["col1"].ToString().Trim();//add by yogita
                            txtAssign.Text = dt.Rows[0]["COL28"].ToString().Trim();
                            txtAssignDt.Text = dt.Rows[0]["COL29"].ToString().Trim();

                            edmode.Value = "Y"; ViewState["entby"] = dt.Rows[0]["ent_by"].ToString(); ViewState["entdt"] = dt.Rows[0]["ent_dt"].ToString();
                            fgen.EnableForm(this.Controls); disablectrl();
                        }
                        break;
                    case "Del":
                        clearctrl();
                        edmode.Value = col1.Trim();
                        fgen.msg("-", "CMSG", "Are You Sure!! You Want to Delete Task No. " + col1.Substring(4, 6) + "");
                        hffield.Value = "D";
                        break;
                }
            }
        }
    }
    protected void btnhideF_s_Click(object sender, EventArgs e)
    {
        col1 = "";
        if (hffield.Value == "List")
        {
            //if (Request.Cookies["Value1"].Value.Length > 0 || Request.Cookies["Value2"].Value.Length > 0 || Request.Cookies["Value3"].Value.Length > 0)
            {
                col1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1").ToString().Trim().Replace("&amp", "");
                col2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2").ToString().Trim().Replace("&amp", "");
                col3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3").ToString().Trim().Replace("&amp", "");

                fromdt = value1;
                todt = value2;
                cldt = value3;
                hffromdt.Value = fromdt;
                hftodt.Value = todt;
                cDT1 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(fmdate,'dd/mm/yyyy') as fromdt from co where code='" + co_cd + year + "'", "fromdt");
                cDT2 = fgen.seek_iname(frm_qstr, co_cd, "select to_char(todate,'dd/mm/yyyy') as todate from co where code='" + co_cd + year + "'", "todate");
                xprdrange = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                sysdat = System.DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        col1 = Request.Cookies["REPLY"].Value.ToString().Trim();
        if (col1 != "Y")
        { }
        else
        {
            if (edmode.Value == "Y") fgen.execute_cmd(frm_qstr, co_cd, "update scratch2 set branchcd='DD' where branchcd='" + mbr + "' and type='TA' and trim(vchnum)='" + txtvchnew.Text.Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + txtdate.Text.Trim() + "'");

            DataSet oDS = new DataSet();
            oDS = fgen.fill_schema(frm_qstr, co_cd, "scratch2");
            if (edmode.Value == "Y") vchnum = txtvchnew.Text;
            else vchnum = fgen.next_no(frm_qstr, co_cd, "select max(vchnum) as vch from scratch2 where  branchcd='" + mbr + "' and type='TA'", 6, "vch");
            //*******************
            //*************************SAVING ****************************************************//           
            // if(txtredate.Text!="-" ){ 
            oporow = oDS.Tables[0].NewRow();
            oporow["branchcd"] = mbr;
            oporow["TYPE"] = "TA";// "DK";
            oporow["vchnum"] = txtvchnew.Text.Trim();
            oporow["vchdate"] = txtdate.Text.Trim();
            oporow["col2"] = txtuserid.Text.Trim();//assigned to in task assign form
            oporow["col7"] = txtredate.Text.Trim();// action date
            oporow["col3"] = "-";//
            oporow["col1"] = txtemailcc.Text.Trim(); //cc value
            oporow["col4"] = txtdrop.Text.Trim(); //ddl1.SelectedItem.ToString();
            oporow["col14"] = txtsubject.Text.Trim();// task mail subject
            oporow["remarks"] = txtmsg.Text.Trim();// task email msg
            oporow["docdate"] = txttskdate.Text.Trim();//date by whcih task dhould be done
            oporow["ReASON"] = txtreason.Text.Trim();// remarks
            oporow["col6"] = txtvchnum.Text.Trim();//
            oporow["col48"] = txtvchdate.Text.Trim();

            oporow["col28"] = txtAssign.Text.Trim();
            oporow["col29"] = txtAssignDt.Text.Trim();
            if (edmode.Value == "Y")
            {
                oporow["eNt_by"] = ViewState["entby"].ToString();
                oporow["eNt_dt"] = ViewState["entdt"];
                oporow["edt_by"] = uname;
                oporow["edt_dt"] = vardate;
            }
            else
            {
                oporow["eNt_by"] = uname;
                oporow["eNt_dt"] = vardate;
                oporow["edt_by"] = "-";
                oporow["eDt_dt"] = vardate;
            }
            oDS.Tables[0].Rows.Add(oporow);

            fgen.save_data(frm_qstr, co_cd, oDS, "scratch2");
            #region
            if (txtAssign.Text.Length > 1)
            {
                string eID = fgen.seek_iname(frm_qstr, co_cd, "select emailid from evas where username='" + txtAssign.Text + "'", "emailid");
                System.Text.StringBuilder msb = new System.Text.StringBuilder();
                msb.Append("<html><body style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; font-style: italic; color: #474646'>");
                msb.Append("Dear " + txtAssign.Text.ToString().Trim() + ",<br/><br/>");
                msb.Append("for your kind information action has been taken on below task assign by you to " + uname + "<br/>");
                msb.Append("action taken on this.<br/><br/>");
                msb.Append("<table border=1 cellspacing=2 cellpadding=2 style='font-family: Arial, Helvetica, sans-serif; font-weight: 700; font-size: 13px; color: #474646'>");
                msb.Append("<tr style='color: #FFFFFF; background-color: #0099FF; font-weight: 700; font-family: Arial, Helvetica, sans-serif'><td><b>Assign by</b></td><td><b>Assign No./Date</b></td><td><b>Subject</b></td><td><b>Completion Date</b></td><td><b>Priority</b></td></tr>");
                msb.Append("<td>");
                msb.Append("Mr/Ms " + txtAssign.Text);
                msb.Append("</td>");
                msb.Append("<td>");
                msb.Append(txtvchnum.Text + "/" + txttskdate.Text);
                msb.Append("</td>");
                msb.Append("<td style='width:150px;'>");
                msb.Append(txtsubject.Text.Trim());
                msb.Append("</td>");
                msb.Append("<td>");
                msb.Append("" + txttskdate.Text.Trim() + "");
                msb.Append("</td>");
                msb.Append("<td>");
                msb.Append("" + txtdrop.Text.Trim() + "");
                msb.Append("</td>");
                msb.Append("</tr>");
                msb.Append("<tr>");
                msb.Append("<td>Details: ");
                msb.Append("</td>");
                msb.Append("<td colspan='5'>");
                msb.Append(txtmsg.Text.Trim().ToString() + "");
                msb.Append("</td>");
                msb.Append("</tr>");

                msb.Append("<tr>");
                msb.Append("<td>Action Taken: ");
                msb.Append("</td>");
                msb.Append("<td colspan='5'>");
                msb.Append(txtreason.Text.Trim().ToString() + "");
                msb.Append("</td>");
                msb.Append("</tr>");

                msb.Append("</table><br/><br/>");
                msb.Append("<br>===========================================================<br>");
                msb.Append("<br>This Report is Auto generated from the Tejaxo ERP.");
                msb.Append("<br>The above details are to be best of information and data available to the ERP system.");
                msb.Append("<br>Errors or Omissions if any are regretted.");
                msb.Append("Thanks and Regards,<br/>");
                msb.Append("" + fgenCO.chk_co(co_cd) + "");
                msb.Append("</body></html>");
                //string cc = "skg@pocketdriver.in,pkg@pocketdriver.in,gm@pocketdriver.in";
                //if (co_cd == "VITR") merr = fgen.send_mail("Tejaxo ERP", txtuserid.Text.Trim(), "", "", txtsubject.Text.Trim(), msb.ToString(), "115.249.131.196", 25, 0, "jpr.erp@vitromed.co.in", "vitro");
                //else
                //{

                // merr = send_mail("Tejaxo ERP", txtuserid.Text.Trim(), "", "");
                //  merr = send_mail("Tejaxo ERP", txtuserid.Text.Trim(), "", "", txtsubject.Text.Trim(), msb.ToString(), "smtp.gmail.com", 587, 1, "rrrbaghel@gmail.com", "finsyserp123");
                string cc = "";
                if (txtemailcc.Text.Trim().Length > 2) cc = txtemailcc.Text.Trim();
                string subje = "Action Taken : [" + txtvchnum.Text + "] on " + txtdate.Text.Trim() + ", " + txtsubject.Text.Trim() + " (" + txtdrop.Text.Trim() + ")";
                merr = fgen.send_mail(co_cd, "Tejaxo ERP", eID, cc, "", subje, msb.ToString());
            }
            #endregion
            if (edmode.Value == "Y")
            {
                //fgen.msg("-", "AMSG", "Data Updated Successfully!!");
                if (merr == "0")
                {
                    fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully & Mail Not Sent");
                }
                else
                {
                    fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Updated Successfully & Mail Sent");
                }
                fgen.execute_cmd(frm_qstr, co_cd, "delete from scratch2 where branchcd='DD' and type='TA' and trim(vchnum)='" + txtvchnew.Text.Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + txtdate.Text.Trim() + "'");
            }
            else
            {
                // fgen.msg("-", "AMSG", "Data Saved Successfully!!");
                if (merr == "0")
                {
                    fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully & Mail Not Sent");
                }
                else
                {
                    fgen.msg("-", "AMSG", lblheader.Text + " " + txtvchnum.Text + " Saved Successfully & Mail Sent");
                }
            }
            fgen.ResetForm(this.Controls); fgen.DisableForm(this.Controls); enablectrl();
        }
    }
    public void disp_data()
    {
        btnval = hffield.Value.Trim();
        switch (btnval)
        {
            case "List":
                SQuery = "SELECT 'action taken' AS FSTR,'action taken' AS Task FROM DUAL UNION ALL SELECT 'no action taken' AS FSTR,'no action taken'  AS Task FROM DUAL";
                break;
            case "New":
                // COMMENTED BY MADHVI ON 02 MAY 2018 AS IT IS SHOWING ALL THOSE TASK WHICH ARE HAVING APPROVED ACTION TAKEN
                // SQuery = "select distinct branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(acode) as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_Date,col1 as user_name,ent_by as assign_by,to_char(ent_Dt,'dd/mm/yyyy') as assign_Dt,col14 as Subject from scratch where branchcd='" + mbr + "' and type='DK' and upper(trim(col1))='" + uname.Trim() + "' and SUBSTR(TRIM(col3),1,3)='[A]' order by vchnum desc"; //real
                col1 = "";
                if (ulevel != "0") col1 = " and upper(trim(b.col1))='" + uname.Trim() + "'";
                SQuery = "SELECT TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY')||TRIM(B.ACODE) AS FSTR,A.VCHNUM AS ENTRY_NO,A.VCHDATE AS ENTRY_DT,B.col1 as user_name,B.ent_by as assign_by,to_char(B.ent_Dt,'dd/mm/yyyy') as assign_Dt,B.col14 as Subject FROM (select distinct BRANCHCD,vchnum,to_char(vchdate,'dd/mm/yyyy') as VCHDATE,TRIM(ACODE) AS ACODE,1 AS QTY from scratch where branchcd='" + mbr + "' and type='DK' /*and SUBSTR(TRIM(col3),1,3)='[A]'*/ UNION ALL SELECT DISTINCT BRANCHCD,COL6 AS VCHNUM,TO_CHAR(COL48,'DD/MM/YYYY') AS VCHDATE,TRIM(COL2) AS ACODE, -1 AS QTY FROM scratch2 where BRANCHCD='" + mbr + "' AND type='TA' AND SUBSTR(TRIM(APP_BY),1,3)='[A]' )A, SCRATCH B Where TRIM(A.BRANCHCD)||TRIM(A.VCHNUM)||trim(A.VCHDATE)=TRIM(B.BRANCHCD)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY') and B.type='DK' " + col1 + " GROUP BY TRIM(B.BRANCHCD)||TRIM(B.TYPE)||TRIM(B.VCHNUM)||TO_CHAR(B.VCHDATE,'DD/MM/YYYY')||TRIM(B.ACODE),A.VCHNUM ,A.VCHDATE,B.col1,B.ent_by,to_char(B.ent_Dt,'dd/mm/yyyy'),B.col14  HAVING SUM(QTY)>0 order by ENTRY_NO desc";
                break;
            default:
                if (btnval == "Del" || btnval == "Edit")
                {
                    col1 = "";
                    if (ulevel != "0") col1 = " and upper(trim(ent_by))='" + uname.Trim() + "'";
                    SQuery = "select distinct branchcd||type||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy') as fstr,vchnum as entry_no,to_char(vchdate,'dd/mm/yyyy') as entry_Date,col6 as task_no,to_char(col48,'dd/mm/yyyy') as task_dt,col2 as user_id,ent_by as assign_by,to_char(ent_Dt,'dd/mm/yyyy') as assign_Dt,col14 as Subject from scratch2 where branchcd='" + mbr + "' and type='TA' " + col1 + " order by vchnum desc";
                }
                break;
        }
        if (SQuery.Length > 0)
        {
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_XID", "Tejaxo");
            fgenMV.Fn_Set_Mvar(frm_qstr, "U_SEEKSQL", SQuery);
        }
    }
    /// <summary>    

}


//SQL> ALTER TABLE FINPPCL.SCRATCH2 ADD REASON VARCHAR2(200);

//Table altered.