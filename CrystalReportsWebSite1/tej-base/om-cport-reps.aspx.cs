using System;
using System.Data;
using System.Web;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing.Printing;
using System.IO;
using System.Net.Mail;

using MessagingToolkit.QRCode.Codec;
using System.Drawing;

public partial class om_cport_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, xprdRange1, DateRange, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, xprdrange1, xprd2, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, todt, header_n, footer_n;
    string branch_Cd = "", xprd1 = "", firm, xhtml_tag, subj, party_cd, part_cd, cust_cd, cond1, pdfView = "", data_found = "", year = "";
    int i1 = 0;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, cond = " ", eff_Dt;
    DataRow dr;
    fgenDB fgen = new fgenDB();
    private DataSet DsImages = new DataSet();
    FileStream FilStr = null; BinaryReader BinRed = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            frm_url = HttpContext.Current.Request.Url.AbsoluteUri;
            frm_PageName = System.IO.Path.GetFileName(Request.Url.AbsoluteUri);
            No_Data_Found.Visible = false;
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
                    frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                    xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");
                    year = fgenMV.Fn_Get_Mvar(frm_qstr, "U_YEAR");
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                    xprdRange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";

                    hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    branch_Cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BRANCH_CD");
                }
                else Response.Redirect("~/login.aspx");

            }

            if (!Page.IsPostBack)
            {
                printCrpt(hfhcid.Value);

                if (data_found == "N")
                {
                    No_Data_Found.Visible = true;
                    divReportViewer.Visible = false;
                }
                else
                {
                    divReportViewer.Visible = true;
                    CrystalReportViewer1.RefreshReport();
                    CrystalReportViewer1.Focus();
                }
            }
        }
        catch (Exception ex)
        {
            fgen.FILL_ERR(ex.Message);
        }
    }

    void printCrpt(string iconID)
    {
        DataTable dt, dt1, dt2, dt3, dt4, dt5, dt6, dtm;
        DataRow mdr, dr1;
        DataSet dsRep = new DataSet();
        string barCode = hfval.Value;
        string scode = barCode;
        string sname = "";
        string mq10, mq1, mq0;
        int repCount = 1;
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        string opt = "";
        data_found = "Y";

        switch (iconID)
        {
            case "F79101"://...ICON NOT FOUND
                #region new report made by yogita...set prd='N'
                header_n = "Status :Sales.Orders(Portal)";
                cond = "and trim(a.acode) = '" + frm_uname + "'";
                SQuery = "select '" + frm_cDt1 + "' as frmdt,'" + frm_cDt2 + "' as todt,'" + header_n + "' as header,a.acode,b.aname as party,a.icode,c.iname,c.cpartno,c.unit,sum(a.djan+a.dfeb+a.dmar+a.dapr+a.dmay+a.djun+a.djul+a.daug+a.dsep+a.doct+a.dnov+a.ddec+a.djan+a.dfeb+a.dmar) as dtot,sum(a.dapr) as dapr,sum(a.dmay) as dmay,sum(a.djun) as djun,sum(a.djul) as djul,sum(a.daug) as daug,sum(a.dsep) as dsep,sum(a.doct) as doct,sum(a.dnov) as dnov,sum(a.ddec) as ddec,sum(a.djan) as djan,sum(a.dfeb) as dfeb,sum(a.dmar) as dmar,a.branchcd,t.name as branch_name,t.addr,t.addr1,t.place,t.tele from (select branchcd, acode ,icode,(Case when to_char(orddt,'mm')='04' then qtyord else 0 end) as Dapr,(Case when to_char(orddt,'mm')='05' then qtyord else 0 end) as Dmay,(Case when to_char(orddt,'mm')='06' then qtyord else 0 end) as Djun,(Case when to_char(orddt,'mm')='07' then qtyord else 0 end) as Djul,(Case when to_char(orddt,'mm')='08' then qtyord else 0 end) as Daug,(Case when to_char(orddt,'mm')='09' then qtyord else 0 end) as Dsep,(Case when to_char(orddt,'mm')='10' then qtyord else 0 end) as Doct,(Case when to_char(orddt,'mm')='11' then qtyord else 0 end) as Dnov,(Case when to_char(orddt,'mm')='12' then qtyord else 0 end) as Ddec,(Case when to_char(orddt,'mm')='01' then qtyord else 0 end) as Djan,(Case when to_char(orddt,'mm')='02' then qtyord else 0 end) as Dfeb,(Case when to_char(orddt,'mm')='03' then qtyord else 0 end) as Dmar from somas where branchcd!='DD' and type LIKE '4%' and orddt " + DateRange + " ) a,famst b,item c,TYPE T where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) AND trim(a.branchcd)=trim(t.type1) and t.id='B' " + cond + " group by a.acode,b.aname,a.icode,c.iname,c.cpartno,c.unit,a.branchcd,t.name,t.addr,t.addr1,t.place,t.tele";
                dt = new DataTable(); dt1 = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                SQuery = "SELECT TYPE1 AS MBR,NAME AS MBR_NAME,ADDR AS MBR_aDDR,ADDR1 AS MBR_ADDR1,TELE as mbr_tele,FAX FROM TYPE WHERE TYPE1='00' AND ID='B'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    dt1.TableName = "mbr_detail";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Desp_mth_Trend_CPortal", "Desp_mth_Trend_CPortal", dsRep, header_n); //for cust portal
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F79106"://...ICON NOT FOUND
                #region new report made by yogita...set prd='N'
                header_n = "Status :Sales.Schedule(Portal)";
                cond = "and trim(a.acode) = '" + frm_uname + "'";
                SQuery = "select '" + frm_cDt1 + "' as frmdt,'" + frm_cDt2 + "' as todt,'" + header_n + "' as header,a.acode,b.aname as party,a.icode,c.iname,c.cpartno,c.unit,sum(a.djan+a.dfeb+a.dmar+a.dapr+a.dmay+a.djun+a.djul+a.daug+a.dsep+a.doct+a.dnov+a.ddec+a.djan+a.dfeb+a.dmar) as dtot,sum(a.dapr) as dapr,sum(a.dmay) as dmay,sum(a.djun) as djun,sum(a.djul) as djul,sum(a.daug) as daug,sum(a.dsep) as dsep,sum(a.doct) as doct,sum(a.dnov) as dnov,sum(a.ddec) as ddec,sum(a.djan) as djan,sum(a.dfeb) as dfeb,sum(a.dmar) as dmar,a.branchcd,t.name as branch_name,t.addr,t.addr1,t.place,t.tele from (select branchcd, trim(acode) as acode,trim(icode) as icode,(Case when to_char(vchdate,'mm')='04' then total else 0 end) as Dapr,(Case when to_char(vchdate,'mm')='05' then total else 0 end) as Dmay,(Case when to_char(vchdate,'mm')='06' then total else 0 end) as Djun,(Case when to_char(vchdate,'mm')='07' then total else 0 end) as Djul,(Case when to_char(vchdate,'mm')='08' then total else 0 end) as Daug,(Case when to_char(vchdate,'mm')='09' then total else 0 end) as Dsep,(Case when to_char(vchdate,'mm')='10' then total else 0 end) as Doct,(Case when to_char(vchdate,'mm')='11' then total else 0 end) as Dnov,(Case when to_char(vchdate,'mm')='12' then total else 0 end) as Ddec,(Case when to_char(vchdate,'mm')='01' then total else 0 end) as Djan,(Case when to_char(vchdate,'mm')='02' then total else 0 end) as Dfeb,(Case when to_char(vchdate,'mm')='03' then total else 0 end) as Dmar from schedule where branchcd!='DD' and type ='46' and vchdate " + DateRange + " ) a,famst b,item c,TYPE T where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) AND trim(a.branchcd)=trim(t.type1) and t.id='B' " + cond + " group by a.acode,b.aname,a.icode,c.iname,c.cpartno,c.unit,a.branchcd,t.name,t.addr,t.addr1,t.place,t.tele";
                dt = new DataTable(); dt1 = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                SQuery = "SELECT TYPE1 AS MBR,NAME AS MBR_NAME,ADDR AS MBR_aDDR,ADDR1 AS MBR_ADDR1,TELE as mbr_tele,FAX FROM TYPE WHERE TYPE1='00' AND ID='B'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    dt1.TableName = "mbr_detail";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Desp_mth_Trend_CPortal", "Desp_mth_Trend_CPortal", dsRep, header_n); //for cust portal
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F79111":
                #region Despatch Trend (Portal)
                header_n = "Despatch Trend (Qty)";
                SQuery = "select '" + frm_cDt1 + "' as frmdt,'" + frm_cDt2 + "' as todt,'" + header_n + "' as header,a.acode,b.aname as party,a.icode,a.ciname as iname,c.cpartno,c.unit,sum(a.djan+a.dfeb+a.dmar+a.dapr+a.dmay+a.djun+a.djul+a.daug+a.dsep+a.doct+a.dnov+a.ddec+a.djan+a.dfeb+a.dmar) as dtot,sum(a.dapr) as dapr,sum(a.dmay) as dmay,sum(a.djun) as djun,sum(a.djul) as djul,sum(a.daug) as daug,sum(a.dsep) as dsep,sum(a.doct) as doct,sum(a.dnov) as dnov,sum(a.ddec) as ddec,sum(a.djan) as djan,sum(a.dfeb) as dfeb,sum(a.dmar) as dmar,a.branchcd,t.name as branch_name,t.addr,t.addr1,t.place,t.tele from (select branchcd,trim(acode) as acode,trim(icode) as icode,trim(purpose) as ciname,(Case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as Dapr,(Case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as Dmay,(Case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as Djun,(Case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as Djul,(Case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as Daug,(Case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as Dsep,(Case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as Doct,(Case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as Dnov,(Case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as Ddec,(Case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as Djan,(Case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as Dfeb,(Case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as Dmar from ivoucher where branchcd!='DD' and substr(trim(type),1,1)='4' and type!='47' and vchdate " + DateRange + " and trim(acode)='" + frm_uname + "') a,famst b,item c,TYPE T where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) AND trim(a.branchcd)=trim(t.type1) and t.id='B' group by a.acode,b.aname,a.icode,a.ciname,c.cpartno,c.unit,a.branchcd,t.name,t.addr,t.addr1,t.place,t.tele";
                dt = new DataTable(); dt1 = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                mq0 = "SELECT TYPE1 AS MBR,NAME AS MBR_NAME,ADDR AS MBR_aDDR,ADDR1 AS MBR_ADDR1,TELE as mbr_tele,FAX FROM TYPE WHERE TYPE1='00' AND ID='B'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    dt1.TableName = "mbr_detail";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Desp_mth_Trend_CPortal", "Desp_mth_Trend_CPortal", dsRep, header_n); //for cust portal
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F79126":
                #region Order Vs Despatch Monthly Trend
                header_n = "Order Dt. Vs Despatch Dt.";
                dsRep = new DataSet();
                SQuery = "select '" + frm_cDt1 + "' as frmdt,'" + frm_cDt2 + "' as todt,'" + header_n + "' as header,a.acode,b.aname as party,a.icode,trim(a.ciname) as iname,c.cpartno,c.unit,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as stot,sum(a.djan+a.dfeb+a.dmar+a.dapr+a.dmay+a.djun+a.djul+a.daug+a.dsep+a.doct+a.dnov+a.ddec+a.djan+a.dfeb+a.dmar) as dtot,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug ,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.dapr) as dapr,sum(a.dmay) as dmay,sum(a.djun) as djun,sum(a.djul) as djul,sum(a.daug) as daug,sum(a.dsep) as dsep,sum(a.doct) as doct,sum(a.dnov) as dnov,sum(a.ddec) as ddec,sum(a.djan) as djan,sum(a.dfeb) as dfeb,sum(a.dmar) as dmar,a.branchcd,t.name as branch_name,t.addr,t.addr1,t.place,t.tele from (select branchcd,trim(acode) as acode,trim(icode) as icode,trim(ciname) as ciname,(case when to_char(porddt,'mm')='04' then QTYORD else 0 end) as apr,(case when to_char(porddt,'mm')='05' then QTYORD else 0 end) as may,(case when to_char(porddt,'mm')='06' then QTYORD else 0 end) as jun,(case when to_char(porddt,'mm')='07' then QTYORD else 0 end) as jul,(case when to_char(porddt,'mm')='08' then QTYORD else 0 end) as aug,(case when to_char(porddt,'mm')='09' then QTYORD else 0 end) as sep,(case when to_char(porddt,'mm')='10' then QTYORD else 0 end) as oct,(case when to_char(porddt,'mm')='11' then QTYORD else 0 end) as nov,(case when to_char(porddt,'mm')='12' then QTYORD else 0 end) as dec,(case when to_char(porddt,'mm')='01' then QTYORD else 0 end) as jan,(case when to_char(porddt,'mm')='02' then QTYORD else 0 end) as feb,(case when to_char(porddt,'mm')='03' then QTYORD else 0 end) as mar ,0 as dapr,0 as dmay,0 as djun,0 as djul,0 as daug,0 as dsep,0 as doct,0 as dnov,0 as ddec,0 as djan,0 as dfeb,0 as dmar from sOMAS where branchcd!='DD' and substr(trim(type),1,1)='4' and type!='47' and porddt " + DateRange + " and trim(acode)='" + frm_uname + "' union all select branchcd,trim(acode) as acode ,trim(icode) as icode,trim(purpose) as purpose,0 as apr,0 as may,0 as jun,0 as jul,0 as aug,0 as sep,0 as oct,0 as nov,0 as dec,0 as jan,0 as feb,0 as mar,(Case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as Dapr,(Case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as Dmay,(Case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as Djun,(Case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as Djul,(Case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as Daug,(Case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as Dsep,(Case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as Doct,(Case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as Dnov,(Case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as Ddec,(Case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as Djan,(Case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as Dfeb,(Case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as Dmar from ivoucher where branchcd!='DD' and substr(trim(type),1,1)='4' and type!='47' and vchdate " + DateRange + " and trim(acode)='" + frm_uname + "') a,famst b,item c,TYPE T where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) AND trim(a.branchcd)=trim(t.type1) and t.id='B' group by a.acode,b.aname,a.icode,trim(a.ciname),c.cpartno,c.unit,a.branchcd,t.name,t.addr,t.addr1,t.place,t.tele";
                dt = new DataTable(); dt1 = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                mq0 = "SELECT TYPE1 AS MBR,NAME AS MBR_NAME,ADDR AS MBR_aDDR,ADDR1 AS MBR_ADDR1,TELE as mbr_tele,FAX FROM TYPE WHERE TYPE1='00' AND ID='B'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    dt1.TableName = "mbr_detail";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Ord_vs_desp_dated_CPortal", "std_Ord_vs_desp_dated_CPortal", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F79132":
                #region Statement of Account
                header_n = "Statement of Account";
                year = fromdt.Substring(6, 4);
                xprd1 = "between to_date('01/04/" + year + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                SQuery = "select g.*,t.name,substr(g.type,1,1) as cond from (select '" + fromdt + "' as frmdate,'" + todt + "' as todate,i.Acode,i.iname,nvl(i.cpartno,'-') as cpartno,nvl(i.issu_uom,'-') as issu_uom,nvl(i.unit,0) as unit,nvl(i.binno,'-') as binno,i.iopqty,i.obal,nvl(v.TYPE,'-') as type,nvl(v.VCHNUM,'-') as vchnum,v.VCHDATE,v.RCODE,nvl(v.iqtyin,0) as iqtyin,nvl(v.iqtyout,0) as iqtyout,nvl(v.rej_rw,0) as rej_rw,trim(v.naration) as naration,v.invno,v.invdate,nvl(v.Rname,'-') as aname,i.p_email, substr(nvl(v.name,'-'),1,4) as Tname,v.branchcd as bcode,'" + fromdt + "' as cdt1,'" + todt + "' as cdt2 from (Select s.branchcd,r.Acode,r.iname,r.cpartno,r.unit,r.email as p_email,r.issu_uom,r.binno,r.iopqty,nvl(s.obal,0) as obal from (select a.ACODE ,a.email,a.ANAME AS iname,a.ADDR1 AS cpartno,a.ADDR2 AS issu_uom,a.PERSON AS unit,a.GIRNO AS binno,NVL(b.opb,0) as iopqty from FAMST a left outer join (select trim(acode) as acode,sum(yr_" + frm_myear + ") as opb from famstbal where branchcd!='DD' and TRIM(acode) ='" + frm_uname + "' group by acode) b on trim(a.acode)=trim(B.acode) WHERE TRIM(A.acode) ='" + frm_uname + "' order by a.Acode) r left outer join (select branchcd,ACODE,sum(nvl(DRAMT,0))-sum(nvl(CRAMT,0)) as obal from voucher where BRANCHCD!='DD' AND BRANCHCD!='88' and VCHDATE " + xprdRange1 + "  and TRIM(acode) ='" + frm_uname + "' GROUP BY ACODE,branchcd) s on r.Acode=s.Acode ) i left outer join (Select X.BRANCHCD,x.TYPE,x.VCHNUM,x.VCHDATE,x.ACODE,X.RCODE,nvl(x.IQTYIN,0) as iqtyin,nvl(x.IQTYOUT,0) as iqtyout,nvl(x.rej_rw,0) as rej_rw,x.naration,x.invno,x.invdate,x.Rname,nvl(y.name,'-') as name,x.p_email from (select A.BRANCHCD,A.TYPE,A.VCHNUM,A.VCHDATE,a.ACODE,(case when a.type like '4%' then 'Sale Bill No.'||a.vchnum||' '||a.naration else'Chq.No.'||max(a.invno)||' Dt.'||to_char(A.vchdate,'DD/MM/YYYY')||' '||a.naration end) as naration,nvl(b.aname,'-') Rname,b.email as p_email,A.RCODE,0 AS REJ_RW,(case when sum(A.DRAMT)-sum(A.CRAMT)>0 then ABS(sum(A.DRAMT)-sum(A.CRAMT)) else 0 end) AS IQTYIN,(case when sum(A.DRAMT)-sum(A.CRAMT)>0 then 0 else abs(sum(A.DRAMT)-sum(A.CRAMT)) end) AS IQTYOUT,max(a.invno) as invno,max(a.invdate) as invdate from voucher A ,FAMST B where a.Rcode=b.acode and a.branchcd!='DD' AND a.branchcd!='88' and trim(a.acode)='" + frm_uname + "' and A.VCHDATE " + xprdRange + " group by A.BRANCHCD,A.TYPE,A.VCHNUM,A.VCHDATE,a.ACODE,a.naration,nvl(b.aname,'-'),b.email,A.RCODE,to_char(A.vchdate,'DD/MM/YYYY') )x left outer join (select type1,name,addr2 from type where id='V') y on x.type=y.type1) v on i.Acode=v.Acode order by i.Acode,v.vchdate,v.type,v.vchnum) g,type t where trim(g.bcode)=trim(t.type1) and t.id='B' and g.acode ='" + frm_uname + "' order by g.Acode,g.vchdate,g.type,g.vchnum";
                dt = new DataTable(); dt1 = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                mq0 = "SELECT TYPE1 AS MBR,NAME AS MBR_NAME,ADDR AS MBR_aDDR,ADDR1 AS MBR_ADDR1,TELE as mbr_tele,FAX FROM TYPE WHERE TYPE1='00' AND ID='B'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    dt1.TableName = "mbr_detail";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "stac_Cportal", "stac_Cportal", dsRep, header_n); //cust rep
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F79133":
                #region Ageing Detailed
                SQuery = ""; cond = "";
                cond = "and trim(acode) ='" + frm_uname.Trim() + "'";
                m1 = fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='R01'", "params");
                eff_Dt = " vchdate>= to_date('" + m1.Trim() + "','dd/mm/yyyy') and vchdate<= to_date('" + todt + "','dd/mm/yyyy')";
                fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view recdataw as (select TRIM(ACODE) AS ACODE,TRIM(INVNO) AS INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM VOUCHER WHERE BRANCHCD!='88' AND BRANCHCD!='DD' AND " + eff_Dt + "  and  SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY ACODE,INVNO,INVDATE UNION ALL SELECT ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM RECEBAL WHERE BRANCHCD!='DD' AND SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY ACODE,INVNO,INVDATE ) c  GROUP BY TRIM(ACODE),TRIM(INVNO),INVDATE HAVING SUM(dRAMT)-SUM(CRAMT)<>0) ORDER BY ACODE,INVDATE,INVNO");
                SQuery = "select 'Debtor Ageing As on Date " + todt + " with Transaction summary' as header,m.aname as Party,n.invno,to_char(n.invdate,'dd/mm/yyyy') as invdate,to_char(n.invdate,'yyyymmdd') as vdd,sum(n.total) as Total_Outstanding,sum(n.slab1) as Current_Os,sum(n.slab2) as OVER_30_60,sum(n.slab3) as OVER_61_90,sum(n.slab4) as OVER_90_180,sum(n.slab5) as OVER_181,n.acode,m.ADDR1 as Address,m.Payment as P_days,m.Climit  as Cr_limit from (SELECT acode,invno,invdate,dramt-cramt as total,(CASE WHEN (TO_DATE('" + todt + "','DD/MM/YYYY')-invdate BETWEEN 0 AND 30) THEN dramt-cramt END) as slab1  ,(CASE WHEN (TO_DATE('" + todt + "','DD/MM/YYYY')-invdate BETWEEN 31 AND 60) THEN dramt-cramt END) as slab2,(CASE WHEN (TO_DATE('" + todt + "','DD/MM/YYYY')-invdate BETWEEN 61 AND 90) THEN dramt-cramt END) as slab3,(CASE WHEN (TO_DATE('" + todt + "','DD/MM/YYYY')-invdate BETWEEN 91 AND 180) THEN dramt-cramt END) as slab4,(CASE WHEN (TO_DATE('" + todt + "','DD/MM/YYYY')-invdate > 181) THEN dramt-cramt END) as slab5 from  recdataw where acode like '16%' " + cond + ") n ,famst m where trim(n.acode)=trim(m.acode) group by m.aname,n.invno,m.addr1,m.climit,m.payment,n.acode,m.zcode,n.invdate having sum(n.total)>0 order by m.aname,vdd,n.invno";
                dt = new DataTable(); dt1 = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                mq0 = "SELECT TYPE1 AS MBR,NAME AS MBR_NAME,ADDR AS MBR_aDDR,ADDR1 AS MBR_ADDR1,TELE as mbr_tele,FAX FROM TYPE WHERE TYPE1='00' AND ID='B'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    dt1.TableName = "mbr_detail";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "deb_age_d_CPortal", "deb_age_d_CPortal", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F79134":
                #region  Ageing Summary
                header_n = "Debtors Ageing Summary Report";
                cond = "";
                cond = "and trim(acode)='" + frm_uname + "'";
                branch_Cd = "branchcd not in ('DD','88')";
                m1 = fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='R01'", "params");
                eff_Dt = " vchdate>= to_date('" + m1.Trim() + "','dd/mm/yyyy') and vchdate<= to_date('" + todt + "','dd/mm/yyyy')";
                fgen.execute_cmd(frm_qstr, frm_cocd, "create or replace view recdataw as(select branchcd,TRIM(ACODE) AS ACODE,TRIM(INVNO) AS INVNO,INVDATE,SUM(DRAMT) AS DRAMT,SUM(CRAMT) AS CRAMT,SUM(DRAMT)-SUM(cRAMT) AS NET from (SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM VOUCHER WHERE BRANCHCD!='88' AND BRANCHCD!='DD' AND " + eff_Dt + "  and  SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE UNION ALL SELECT branchcd,ACODE,INVNO,INVDATE ,SUM(nvl(DRAMT,0)) AS DRAMT,SUM(nvl(CRAMT,0)) AS CRAMT ,sum(nvl(dramt,0))-sum(nvl(cramt,0)) as net FROM RECEBAL WHERE SUBSTR(ACODE,1,2)IN('02','05','06','16') GROUP BY branchcd,ACODE,INVNO,INVDATE ) c  GROUP BY branchcd,TRIM(ACODE),TRIM(INVNO),INVDATE HAVING SUM(dRAMT)-SUM(CRAMT)<>0)  ORDER BY branchcd,ACODE,INVDATE,INVNO");
                xprdrange1 = "between to_Date('01/" + todt.Substring(3, 7).Trim() + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                xprd1 = " between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                mq0 = "Select 0 as Total_Outstanding, 0 as Current_Os,0 as OVER_30_60,0 as OVER_61_90,0 as OVER_90_180,0 as OVER_181,TRIM(acode) AS ACODE,0 AS CL,0 AS BILL_TOT,sum(cramt-dramt) as mtd from voucher where " + branch_Cd + " and SUBSTR(TYPE,1,1) in ('1','3') and substr(acode,1,2) in ('16') and VCHDATE " + xprdrange1 + " group by TRIM(acode) UNION ALL ";
                mq1 = "select 0 as Total_Outstanding, 0 as Current_Os,0 as OVER_30_60,0 as OVER_61_90,0 as OVER_90_180,0 as OVER_181,TRIM(acode) AS ACODE,0 AS CL,sum(bill_tot) as bill_tot,0 AS MTD from sale where " + branch_Cd + " and type like '4%' and type<>'47' and vchdate " + xprdrange1 + " and substr(acode,1,2)='16'  group by TRIM(acode) UNION ALL ";
                mq2 = "select sum(n.total) as Total_Outstanding, sum(n.slab1) as Current_Os, sum(n.slab2) as OVER_30_60, sum(n.slab3) as OVER_61_90, sum(n.slab4) as OVER_90_180, sum(n.slab5) as OVER_181,TRIM(n.acode) AS ACODE,0 AS CL,0 AS BILL_TOT,0 AS MTD from (SELECT TRIM(acode) AS ACODE,dramt-cramt as total,(CASE WHEN (TO_DATE('" + todt + "','DD/MM/YYYY')-invdate BETWEEN 0 AND 30) THEN dramt-cramt END) as slab1  ,(CASE WHEN (TO_DATE('" + todt + "','DD/MM/YYYY')-invdate BETWEEN 31 AND 60) THEN dramt-cramt END) as slab2,(CASE WHEN (TO_DATE('" + todt + "','DD/MM/YYYY')-invdate BETWEEN 61 AND 90) THEN dramt-cramt END) as slab3,(CASE WHEN (TO_DATE('" + todt + "','DD/MM/YYYY')-invdate BETWEEN 91 AND 180) THEN dramt-cramt END) as slab4,(CASE WHEN (TO_DATE('" + todt + "','DD/MM/YYYY')-invdate > 181) THEN dramt-cramt END) as slab5 from  recdataw where " + branch_Cd + ") n where substr(n.acode,1,2) in ('16') and n.total!=0 group by TRIM(n.acode) UNION ALL ";
                mq3 = "select 0 as Total_Outstanding, 0 as Current_Os,0 as OVER_30_60,0 as OVER_61_90,0 as OVER_90_180,0 as OVER_181,TRIM(acode) AS ACODE,sum(opb)+sum(curr) as CL,0 as bill_tot,0 AS MTD from (select TRIM(acode) AS ACODE, yr_" + year + " as opb, 0 as curr from famstbal where " + branch_Cd + " union all select TRIM(acode) AS ACODE, 0 as opb, sum(dramt)-sum(cramt) as curr from voucher where " + branch_Cd + " and vchdate " + xprd1 + " group by TRIM(acode)) group by TRIM(acode) ";
                SQuery = "SELECT * FROM (SELECT 'Debtor Ageing As on Date " + todt + " with Transaction summary' as header,x.pay_num,x.aname as party,x.Climit,sum(Z.TOTAL_OUTSTANDING) as TOTAL_OUTSTANDING,sum(Z.CURRENT_OS) as CURRENT_OS,sum(Z.OVER_30_60) as OVER_30_60,sum(Z.OVER_61_90) as OVER_61_90,sum(Z.OVER_90_180) as OVER_90_180,sum(Z.OVER_181) as OVER_181,TRIM(Z.ACODE) AS ACODE,sum(Z.CL) as cl,sum(Z.BILL_TOT) as BILL_TOT,sum(Z.MTD) as mtd FROM ( " + mq0 + mq1 + mq2 + mq3 + ") Z,famst x where trim(z.acode)=trim(x.acode) and substr(x.acode,1,2)='16' and trim(z.acode)='" + frm_uname + "' group by trim(z.acode),x.aname,x.Climit,x.pay_num) WHERE (TOTAL_OUTSTANDING+CURRENT_OS+OVER_30_60+OVER_61_90+OVER_90_180+OVER_181+CL+BILL_TOT+MTD)!=0 order by party";
                dt = new DataTable(); dt1 = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                mq0 = "SELECT TYPE1 AS MBR,NAME AS MBR_NAME,ADDR AS MBR_aDDR,ADDR1 AS MBR_ADDR1,TELE as mbr_tele,FAX FROM TYPE WHERE TYPE1='00' AND ID='B'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    dt1.TableName = "mbr_detail";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "ageing_deb_Cportal", "ageing_deb_Cportal", dsRep, header_n); //for customer portal
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F79136":
                #region Order Vs Dispatch 12 Month
                header_n = "Order Vs Dispatch 12 Month";
                SQuery = "select '" + frm_cDt1 + "' as frmdt,'" + frm_cDt2 + "' as todt,'" + header_n + "' as header,TRIM(a.acode) AS ACODE,b.aname as party,TRIM(a.icode) AS ICODE,TRIM(A.Ciname) AS CINAME,c.cpartno,c.unit,a.branchcd,t.name as branch,trim(t.addr1) as branch_addr1,trim(t.addr2) as branch_addr2,t.tele as branch_tele,t.place as branch_place,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as stot,sum(a.djan+a.dfeb+a.dmar+a.dapr+a.dmay+a.djun+a.djul+a.daug+a.dsep+a.doct+a.dnov+a.ddec+a.djan+a.dfeb+a.dmar) as dtot,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug ,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.dapr) as dapr,sum(a.dmay) as dmay,sum(a.djun) as djun,sum(a.djul) as djul,sum(a.daug) as daug,sum(a.dsep) as dsep,sum(a.doct) as doct,sum(a.dnov) as dnov,sum(a.ddec) as ddec,sum(a.djan) as djan,sum(a.dfeb) as dfeb,sum(a.dmar) as dmar from (select branchcd,acode,icode,CINAME,(case when to_char(ORDDT,'mm')='04' then QTYORD else 0 end) as apr,(case when to_char(ORDDT,'mm')='05' then QTYORD else 0 end) as may,(case when to_char(ORDDT,'mm')='06' then QTYORD else 0 end) as jun,(case when to_char(ORDDT,'mm')='07' then QTYORD else 0 end) as jul,(case when to_char(ORDDT,'mm')='08' then QTYORD else 0 end) as aug,(case when to_char(ORDDT,'mm')='09' then QTYORD else 0 end) as sep,(case when to_char(ORDDT,'mm')='10' then QTYORD else 0 end) as oct,(case when to_char(ORDDT,'mm')='11' then QTYORD else 0 end) as nov,(case when to_char(ORDDT,'mm')='12' then QTYORD else 0 end) as dec,(case when to_char(ORDDT,'mm')='01' then QTYORD else 0 end) as jan,(case when to_char(ORDDT,'mm')='02' then QTYORD else 0 end) as feb,(case when to_char(ORDDT,'mm')='03' then QTYORD else 0 end) as mar ,0 as dapr,0 as dmay,0 as djun,0 as djul,0 as daug,0 as dsep,0 as doct,0 as dnov,0 as ddec,0 as djan,0 as dfeb,0 as dmar from sOMAS where branchcd!='DD' and type LIKE '4%' and ORDDT " + DateRange + " and trim(acode)='" + frm_uname + "' union all select branchcd,acode ,icode,PURPOSE,0 as apr,0 as may,0 as jun,0 as jul,0 as aug,0 as sep,0 as oct,0 as nov,0 as dec,0 as jan,0 as feb,0 as mar,(Case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as Dapr,(Case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as Dmay,(Case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as Djun,(Case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as Djul,(Case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as Daug,(Case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as Dsep,(Case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as Doct,(Case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as Dnov,(Case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as Ddec,(Case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as Djan,(Case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as Dfeb,(Case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as Dmar from ivoucher where branchcd!='DD' and type like '4%' and vchdate " + DateRange + " and trim(acode)='" + frm_uname + "') a,famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' group by TRIM(a.acode),b.aname,TRIM(a.icode),TRIM(A.Ciname),c.cpartno,c.unit,a.branchcd,t.name,trim(t.addr1),trim(t.addr2),t.tele,t.place";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                mq0 = "SELECT TYPE1 AS MBR,NAME AS MBR_NAME,ADDR AS MBR_aDDR,ADDR1 AS MBR_ADDR1,TELE as mbr_tele,FAX FROM TYPE WHERE TYPE1='00' AND ID='B'";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    dt1.TableName = "Mbr_Detail";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Ord_vs_desp_mth_CPortal", "std_Ord_vs_desp_mth_CPortal", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F79138":
                #region Schedule Vs Despatch Daily Trend
                mq1 = ""; mq2 = "";
                header_n = "Schedule Vs Despatch Daily Trend (Qty)";
                cond = "and trim(a.acode)='" + frm_uname + "'";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                if (Convert.ToInt32(mq1) < 3)
                {
                    i1 = Convert.ToInt16(year) + 1;
                }
                else
                {
                    i1 = Convert.ToInt16(year);
                }

                mq2 = mq1 + "/" + i1;
                // schedule
                SQuery = "select '" + frm_cDt1 + "' as frmdt,'" + frm_cDt1 + "' as todt,'" + header_n + "' as header,'" + mq2 + "' AS MTH,a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit,a.branchcd,t.name as branch_name,t.addr,t.addr1,t.place,t.tele,sum(a.day1) as Day_01,sum(a.day2) as day_02,sum(a.day3) as day_03,sum(a.day4) as day_04,sum(a.day5) as day_05,sum(a.day6) as day_06,sum(a.day7) as day_07,sum(a.day8) as day_08,sum(a.day9) as day_09,sum(a.day10) as day_10,sum(a.day11) as day_11,sum(a.day12) as day_12,sum(a.day13) as day_13,sum(a.day14) as day_14,sum(a.day15) as day_15,sum(a.day16) as day_16,sum(a.day17) as day_17,sum(a.day18) as day_18,sum(a.day19) as day_19,sum(a.day20) as day_20,sum(a.day21) as day_21,sum(a.day22) as day_22,sum(a.day23) as day_23,sum(a.day24) as day_24,sum(a.day25) as day_25,sum(a.day26) as day_26,sum(a.day27) as day_27,sum(a.day28) as day_28,sum(a.day29) as day_29,sum(a.day30) as day_30,sum(a.day31) as day_31,sum(A.Rday1) as Rday1,sum(A.Rday2) as Rday2,sum(A.Rday3) as Rday3,sum(A.Rday4) as Rday4,sum(A.Rday5) as Rday5,sum(A.Rday6) as Rday6,sum(A.Rday7) as Rday7,sum(A.Rday8) as Rday8,sum(A.Rday9) as Rday9, sum(A.Rday10) as Rday10,sum(A.Rday11) as Rday11,sum(A.Rday12) as Rday12,sum(A.Rday13) as Rday13,sum(A.Rday14) as Rday14,sum(A.Rday15) as Rday15,sum(A.Rday16) as Rday16,sum(A.Rday17) as Rday17,sum(A.Rday18) as Rday18,sum(A.Rday19) as Rday19,sum(A.Rday20) as Rday20,sum(A.Rday21) as Rday21,sum(A.Rday22) as Rday22,sum(A.Rday23) as Rday23,sum(A.Rday24) as Rday24,sum(A.Rday25) as Rday25,sum(A.Rday26) as Rday26,sum(A.Rday27) as Rday27,sum(A.Rday28) as Rday28,sum(A.Rday29) as Rday29,sum(A.Rday30) as Rday30,sum(A.Rday31) as Rday31 from (SELECT branchcd, trim(Acode) as acode,trim(icode) as icode,DAY1,DAY2,DAY3,day4,day5,day6,day7,day8,day9,day10, Day11,day12,day13,day14,day15,day16,day17 ,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31,0 AS Rday1,0 AS Rday2,0 AS Rday3,0 AS Rday4,0 AS Rday5,0 AS Rday6,0 AS Rday7,0 AS Rday8,0 AS Rday9,0 AS Rday10,0 AS Rday11,0 AS Rday12,0 AS Rday13,0 Rday14,0 AS Rday15,0 AS Rday16,0 AS Rday17,0 AS Rday18,0 AS Rday19,0 AS Rday20,0 AS Rday21,0 AS Rday22,0 AS Rday23,0 AS Rday24,0 AS Rday25,0 AS Rday26,0 AS Rday27,0 AS Rday28,0 AS Rday29,0 AS Rday30,0 AS Rday31 FROM SCHEDULE WHERE BRANCHCd!='DD' AND TYPE='46' and to_char(vchdate,'mm/yyyy')='" + mq2 + "' UNION ALL SELECT  branchcd,trim(acode) as acode,trim(icode) as icode,0 as DAY1,0 as day2,0 as day3,0 as day4,0 as day5,0 as day6,0 as day7,0 as day8,0 as day9,0 as day10 ,0 as day11,0 as day12, 0 as day13,0 as day14,0 as day15,0 as day16,0 as day17,0 as day18,0 as day19,0 as day20,0 as day21,0 as day22,0 as day23,0 as day24,0 as day25,0 as day26,0 as day27,0 as day28,0 as day29,0 as day30,0 as day31,(Case when to_char(vchdate,'dd')='01' then iqtyout else 0 end) as Rday1,(Case when to_char(vchdate,'dd')='02' then iqtyout else 0 end) as Rday2,(Case when to_char(vchdate,'dd')='03' then iqtyout else 0 end) as Rday3,(Case when to_char(vchdate,'dd')='04' then iqtyout else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyout else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyout else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyout else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyout else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyout else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyout else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyout else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyout else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyout else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyout else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyout else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyout else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyout else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyout else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyout else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyout  else 0 end) as Rday20,(Case when to_char(vchdate,'dd')='21' then iqtyout else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyout  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyout else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyout  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyout  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyout else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyout else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyout  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyout  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyout  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyout  else 0 end) as Rday31 from ivoucher where branchcd!='DD' and substr(trim(type),1,1)='4' and to_char(vchdate,'mm/yyyy')='" + mq2 + "' and nvl(iqtyout,0)>0)  a,famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' " + cond + " group by a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit,a.branchcd,t.name,t.addr,t.addr1,t.place,t.tele order by a.icode";
                // budgmst
                SQuery = "select '" + frm_cDt1 + "' as frmdt,'" + frm_cDt1 + "' as todt,'" + header_n + "' as header,'" + mq2 + "' AS MTH,trim(a.acode) as acode,trim(a.icode) as icode,trim(b.aname) as aname,trim(a.ciname) as iname,trim(c.cpartno) as cpartno,trim(c.unit) as unit,a.branchcd,t.name as branch_name,t.addr,t.addr1,t.place,t.tele,sum(a.day1) as Day_01,sum(a.day2) as day_02,sum(a.day3) as day_03,sum(a.day4) as day_04,sum(a.day5) as day_05,sum(a.day6) as day_06,sum(a.day7) as day_07,sum(a.day8) as day_08,sum(a.day9) as day_09,sum(a.day10) as day_10,sum(a.day11) as day_11,sum(a.day12) as day_12,sum(a.day13) as day_13,sum(a.day14) as day_14,sum(a.day15) as day_15,sum(a.day16) as day_16,sum(a.day17) as day_17,sum(a.day18) as day_18,sum(a.day19) as day_19,sum(a.day20) as day_20,sum(a.day21) as day_21,sum(a.day22) as day_22,sum(a.day23) as day_23,sum(a.day24) as day_24,sum(a.day25) as day_25,sum(a.day26) as day_26,sum(a.day27) as day_27,sum(a.day28) as day_28,sum(a.day29) as day_29,sum(a.day30) as day_30,sum(a.day31) as day_31,sum(A.Rday1) as Rday1,sum(A.Rday2) as Rday2,sum(A.Rday3) as Rday3,sum(A.Rday4) as Rday4,sum(A.Rday5) as Rday5,sum(A.Rday6) as Rday6,sum(A.Rday7) as Rday7,sum(A.Rday8) as Rday8,sum(A.Rday9) as Rday9, sum(A.Rday10) as Rday10,sum(A.Rday11) as Rday11,sum(A.Rday12) as Rday12,sum(A.Rday13) as Rday13,sum(A.Rday14) as Rday14,sum(A.Rday15) as Rday15,sum(A.Rday16) as Rday16,sum(A.Rday17) as Rday17,sum(A.Rday18) as Rday18,sum(A.Rday19) as Rday19,sum(A.Rday20) as Rday20,sum(A.Rday21) as Rday21,sum(A.Rday22) as Rday22,sum(A.Rday23) as Rday23,sum(A.Rday24) as Rday24,sum(A.Rday25) as Rday25,sum(A.Rday26) as Rday26,sum(A.Rday27) as Rday27,sum(A.Rday28) as Rday28,sum(A.Rday29) as Rday29,sum(A.Rday30) as Rday30,sum(A.Rday31) as Rday31 from (SELECT a.branchcd, trim(a.Acode) as acode,trim(a.icode) as icode,trim(s.ciname) as ciname,(case when to_char(a.dlv_date,'dd')='01' then budgetcost else 0 end) as day1,(case when to_char(a.dlv_date,'dd')='02' then budgetcost else 0 end) as day2,(case when to_char(a.dlv_date,'dd')='03' then budgetcost else 0 end) as day3,(case when to_char(a.dlv_date,'dd')='04' then budgetcost else 0 end) as day4,(case when to_char(a.dlv_date,'dd')='05' then budgetcost else 0 end) as day5,(case when to_char(a.dlv_date,'dd')='06' then budgetcost else 0 end) as day6,(case when to_char(a.dlv_date,'dd')='07' then budgetcost else 0 end) as day7,(case when to_char(a.dlv_date,'dd')='08' then budgetcost else 0 end) as day8,(case when to_char(a.dlv_date,'dd')='09' then budgetcost else 0 end) as day9,(case when to_char(a.dlv_date,'dd')='10' then budgetcost else 0 end) as day10,(case when to_char(a.dlv_date,'dd')='11' then budgetcost else 0 end) as day11,(case when to_char(a.dlv_date,'dd')='12' then budgetcost else 0 end) as day12,(case when to_char(a.dlv_date,'dd')='13' then budgetcost else 0 end) as day13,(case when to_char(a.dlv_date,'dd')='14' then budgetcost else 0 end) as day14,(case when to_char(a.dlv_date,'dd')='15' then budgetcost else 0 end) as day15,(case when to_char(a.dlv_date,'dd')='16' then budgetcost else 0 end) as day16,(case when to_char(a.dlv_date,'dd')='17' then budgetcost else 0 end) as day17,(case when to_char(a.dlv_date,'dd')='18' then budgetcost else 0 end) as day18,(case when to_char(a.dlv_date,'dd')='19' then budgetcost else 0 end) as day19,(case when to_char(a.dlv_date,'dd')='20' then budgetcost else 0 end) as day20,(case when to_char(a.dlv_date,'dd')='21' then budgetcost else 0 end) as day21,(case when to_char(a.dlv_date,'dd')='22' then budgetcost else 0 end) as day22,(case when to_char(a.dlv_date,'dd')='23' then budgetcost else 0 end) as day23,(case when to_char(a.dlv_date,'dd')='24' then budgetcost else 0 end) as day24,(case when to_char(a.dlv_date,'dd')='25' then budgetcost else 0 end) as day25,(case when to_char(a.dlv_date,'dd')='26' then budgetcost else 0 end) as day26,(case when to_char(a.dlv_date,'dd')='27' then budgetcost else 0 end) as day27,(case when to_char(a.dlv_date,'dd')='28' then budgetcost else 0 end) as day28,(case when to_char(a.dlv_date,'dd')='29' then budgetcost else 0 end) as day29,(case when to_char(a.dlv_date,'dd')='30' then budgetcost else 0 end) as day30,(case when to_char(a.dlv_date,'dd')='31' then budgetcost else 0 end) as day31,0 AS Rday1,0 AS Rday2,0 AS Rday3,0 AS Rday4,0 AS Rday5,0 AS Rday6,0 AS Rday7,0 AS Rday8,0 AS Rday9,0 AS Rday10,0 AS Rday11,0 AS Rday12,0 AS Rday13,0 Rday14,0 AS Rday15,0 AS Rday16,0 AS Rday17,0 AS Rday18,0 AS Rday19,0 AS Rday20,0 AS Rday21,0 AS Rday22,0 AS Rday23,0 AS Rday24,0 AS Rday25,0 AS Rday26,0 AS Rday27,0 AS Rday28,0 AS Rday29,0 AS Rday30,0 AS Rday31 FROM budgmst a,somas s where trim(a.solink)||trim(a.icode)||trim(a.acode)=trim(s.branchcd)||trim(s.type)||trim(s.ordno)||to_char(s.orddt,'dd/mm/yyyy')||trim(s.icode)||trim(s.acode) and a.BRANCHCd!='DD' AND a.TYPE='46' and to_char(a.dlv_date,'mm/yyyy')='" + mq2 + "' UNION ALL SELECT  branchcd,trim(acode) as acode,trim(icode) as icode,trim(purpose) as purpose,0 as DAY1,0 as day2,0 as day3,0 as day4,0 as day5,0 as day6,0 as day7,0 as day8,0 as day9,0 as day10 ,0 as day11,0 as day12, 0 as day13,0 as day14,0 as day15,0 as day16,0 as day17,0 as day18,0 as day19,0 as day20,0 as day21,0 as day22,0 as day23,0 as day24,0 as day25,0 as day26,0 as day27,0 as day28,0 as day29,0 as day30,0 as day31,(Case when to_char(vchdate,'dd')='01' then iqtyout else 0 end) as Rday1,(Case when to_char(vchdate,'dd')='02' then iqtyout else 0 end) as Rday2,(Case when to_char(vchdate,'dd')='03' then iqtyout else 0 end) as Rday3,(Case when to_char(vchdate,'dd')='04' then iqtyout else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyout else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyout else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyout else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyout else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyout else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyout else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyout else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyout else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyout else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyout else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyout else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyout else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyout else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyout else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyout else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyout  else 0 end) as Rday20,(Case when to_char(vchdate,'dd')='21' then iqtyout else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyout  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyout else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyout  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyout  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyout else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyout else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyout  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyout  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyout  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyout  else 0 end) as Rday31 from ivoucher where branchcd!='DD' and substr(trim(type),1,1)='4' and to_char(vchdate,'mm/yyyy')='" + mq2 + "' and nvl(iqtyout,0)>0)  a,famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' " + cond + " group by trim(a.acode),trim(a.icode),trim(b.aname),trim(a.ciname),trim(c.cpartno),trim(c.unit),a.branchcd,t.name,t.addr,t.addr1,t.place,t.tele order by icode";
                dt = new DataTable(); dt1 = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                mq0 = "SELECT TYPE1 AS MBR,NAME AS MBR_NAME,ADDR AS MBR_aDDR,ADDR1 AS MBR_ADDR1,TELE as mbr_tele,FAX FROM TYPE WHERE TYPE1='00' AND ID='B'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    dt1.TableName = "mbr_detail";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sch_Vs_Desp_DayWise_CPortal", "std_Sch_Vs_Desp_DayWise_CPortal", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F79139":
                #region Sch Vs Desp 12 Months
                header_n = "Schedule Vs Despatch 12 Month (Qty)";
                // schedule
                SQuery = "select '" + frm_cDt1 + "' as frmdt,'" + frm_cDt2 + "' as todt,'" + header_n + "' as header,a.acode,b.aname as party,a.icode,c.iname,c.cpartno,c.unit,a.branchcd,t.name as branch_name,t.addr,t.addr1,t.place,t.tele,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as stot,sum(a.djan+a.dfeb+a.dmar+a.dapr+a.dmay+a.djun+a.djul+a.daug+a.dsep+a.doct+a.dnov+a.ddec+a.djan+a.dfeb+a.dmar) as dtot,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug ,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.dapr) as dapr,sum(a.dmay) as dmay,sum(a.djun) as djun,sum(a.djul) as djul,sum(a.daug) as daug,sum(a.dsep) as dsep,sum(a.doct) as doct,sum(a.dnov) as dnov,sum(a.ddec) as ddec,sum(a.djan) as djan,sum(a.dfeb) as dfeb,sum(a.dmar) as dmar from (select branchcd,trim(acode) as acode,trim(icode) as icode,(case when to_char(vchdate,'mm')='04' then total else 0 end) as apr,(case when to_char(vchdate,'mm')='05' then total else 0 end) as may,(case when to_char(vchdate,'mm')='06' then total else 0 end) as jun,(case when to_char(vchdate,'mm')='07' then total else 0 end) as jul,(case when to_char(vchdate,'mm')='08' then total else 0 end) as aug,(case when to_char(vchdate,'mm')='09' then total else 0 end) as sep,(case when to_char(vchdate,'mm')='10' then total else 0 end) as oct,(case when to_char(vchdate,'mm')='11' then total else 0 end) as nov,(case when to_char(vchdate,'mm')='12' then total else 0 end) as dec,(case when to_char(vchdate,'mm')='01' then total else 0 end) as jan,(case when to_char(vchdate,'mm')='02' then total else 0 end) as feb,(case when to_char(vchdate,'mm')='03' then total else 0 end) as mar ,0 as dapr,0 as dmay,0 as djun,0 as djul,0 as daug,0 as dsep,0 as doct,0 as dnov,0 as ddec,0 as djan,0 as dfeb,0 as dmar from schedule where branchcd!='DD' and type='46' and vchdate " + DateRange + " and trim(acode)='" + frm_uname + "' union all select branchcd,trim(acode) as acode,trim(icode) as icode,0 as apr,0 as may,0 as jun,0 as jul,0 as aug,0 as sep,0 as oct,0 as nov,0 as dec,0 as jan,0 as feb,0 as mar,(Case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as Dapr,(Case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as Dmay,(Case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as Djun,(Case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as Djul,(Case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as Daug,(Case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as Dsep,(Case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as Doct,(Case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as Dnov,(Case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as Ddec,(Case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as Djan,(Case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as Dfeb,(Case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as Dmar from ivoucher where branchcd!='DD' and substr(trim(type),1,1)='4' and type!='47' and vchdate " + DateRange + " and trim(acode)='" + frm_uname + "') a,famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' group by a.acode,b.aname,a.icode,c.iname,c.cpartno,c.unit,a.branchcd,t.name,t.addr,t.addr1,t.place,t.tele";
                //budgmst
                SQuery = "select '" + frm_cDt1 + "' as frmdt,'" + frm_cDt2 + "' as todt,'" + header_n + "' as header,trim(a.acode) as acode,trim(b.aname) as party,trim(a.icode) as icode,trim(a.ciname) as iname,trim(c.cpartno) as cpartno,trim(c.unit) as unit,a.branchcd,t.name as branch_name,t.addr,t.addr1,t.place,t.tele,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as stot,sum(a.djan+a.dfeb+a.dmar+a.dapr+a.dmay+a.djun+a.djul+a.daug+a.dsep+a.doct+a.dnov+a.ddec+a.djan+a.dfeb+a.dmar) as dtot,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug ,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.dapr) as dapr,sum(a.dmay) as dmay,sum(a.djun) as djun,sum(a.djul) as djul,sum(a.daug) as daug,sum(a.dsep) as dsep,sum(a.doct) as doct,sum(a.dnov) as dnov,sum(a.ddec) as ddec,sum(a.djan) as djan,sum(a.dfeb) as dfeb,sum(a.dmar) as dmar from (select a.branchcd,trim(a.acode) as acode,trim(a.icode) as icode,trim(s.ciname) as ciname,(case when to_char(a.dlv_date,'mm')='04' then budgetcost else 0 end) as apr,(case when to_char(a.dlv_date,'mm')='05' then budgetcost else 0 end) as may,(case when to_char(a.dlv_date,'mm')='06' then budgetcost else 0 end) as jun,(case when to_char(a.dlv_date,'mm')='07' then budgetcost else 0 end) as jul,(case when to_char(a.dlv_date,'mm')='08' then budgetcost else 0 end) as aug,(case when to_char(a.dlv_date,'mm')='09' then budgetcost else 0 end) as sep,(case when to_char(a.dlv_date,'mm')='10' then budgetcost else 0 end) as oct,(case when to_char(a.dlv_date,'mm')='11' then budgetcost else 0 end) as nov,(case when to_char(a.dlv_date,'mm')='12' then budgetcost else 0 end) as dec,(case when to_char(dlv_date,'mm')='01' then budgetcost else 0 end) as jan,(case when to_char(dlv_date,'mm')='02' then budgetcost else 0 end) as feb,(case when to_char(dlv_date,'mm')='03' then budgetcost else 0 end) as mar ,0 as dapr,0 as dmay,0 as djun,0 as djul,0 as daug,0 as dsep,0 as doct,0 as dnov,0 as ddec,0 as djan,0 as dfeb,0 as dmar from budgmst a,somas s where trim(a.solink)||trim(a.icode)||trim(a.acode)=trim(s.branchcd)||trim(s.type)||trim(s.ordno)||to_char(s.orddt,'dd/mm/yyyy')||trim(s.icode)||trim(s.acode) and a.branchcd!='DD' and a.type='46' and a.dlv_date " + DateRange + " and trim(a.acode)='" + frm_uname + "' union all select branchcd,trim(acode) as acode,trim(icode) as icode,trim(purpose) as purpose,0 as apr,0 as may,0 as jun,0 as jul,0 as aug,0 as sep,0 as oct,0 as nov,0 as dec,0 as jan,0 as feb,0 as mar,(Case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as Dapr,(Case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as Dmay,(Case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as Djun,(Case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as Djul,(Case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as Daug,(Case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as Dsep,(Case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as Doct,(Case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as Dnov,(Case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as Ddec,(Case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as Djan,(Case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as Dfeb,(Case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as Dmar from ivoucher where branchcd!='DD' and substr(trim(type),1,1)='4' and type!='47' and vchdate " + DateRange + " and trim(acode)='" + frm_uname + "') a,famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and trim(a.branchcd)=trim(t.type1) and t.id='B' group by trim(a.acode),trim(b.aname),trim(a.icode),trim(a.ciname),trim(c.cpartno),trim(c.unit),a.branchcd,t.name,t.addr,t.addr1,t.place,t.tele order by icode";
                dt = new DataTable(); dt1 = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                mq0 = "SELECT TYPE1 AS MBR,NAME AS MBR_NAME,ADDR AS MBR_aDDR,ADDR1 AS MBR_ADDR1,TELE as mbr_tele,FAX FROM TYPE WHERE TYPE1='00' AND ID='B'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    ///for cust portal 00 branchcd is fixed 
                    dt1.TableName = "mbr_detail";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sch_vs_desp_mth_CPortal", "std_Sch_vs_desp_mth_CPortal", dsRep, header_n); //for cust portal
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
        }
    }

    public void Print_Report_BYDS(string co_Cd, string mbr, string xml, string report, DataSet data_set, string title)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/Report/" + report.Trim() + ".rpt";
        data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr));
        data_set.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
        if (data_set.Tables[0].Rows.Count > 0)
        {
            CrystalReportViewer1.DisplayPage = true;
            CrystalReportViewer1.DisplayToolbar = true;
            CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = GetReportDocument(data_set, rptfile);
            CrystalReportViewer1.DataBind();
            Session["data_set"] = data_set;
            Session["rptfile"] = rptfile;
            //conv_pdf(data_set, rptfile);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup2();", true);
        }
        data_set.Dispose();
    }

    public void Print_Report_BYDS(string co_Cd, string mbr, string xml, string report, DataSet data_set, string title, string addlogo)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/Report/" + report.Trim() + ".rpt";

        if (addlogo == "Y") data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr, "Y"));
        else data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr));

        data_set.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
        if (data_set.Tables[0].Rows.Count > 0)
        {
            CrystalReportViewer1.DisplayPage = true;
            CrystalReportViewer1.DisplayToolbar = true;
            CrystalReportViewer1.DisplayGroupTree = false;
            CrystalReportViewer1.ReportSource = GetReportDocument(data_set, rptfile);
            CrystalReportViewer1.DataBind();
            Session["data_set"] = data_set;
            Session["rptfile"] = rptfile;
            //conv_pdf(data_set, rptfile);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup2();", true);
        }
        data_set.Dispose();
    }

    public override void VerifyRenderingInServerForm(Control control)
    { return; }

    private ReportDocument GetReportDocument(DataSet rptDS, string rptFileName)
    {
        string repFilePath = Server.MapPath("" + rptFileName + "");
        repDoc = new ReportDocument();
        repDoc.Load(repFilePath, OpenReportMethod.OpenReportByDefault);
        repDoc.Refresh();
        repDoc.SetDataSource(rptDS);
        rptDS.Dispose();
        return repDoc;
    }

    protected void Page_UnLoad(object sender, EventArgs e)
    {
        try
        {
            repDoc.Close();
            repDoc.Dispose();
        }
        catch (Exception ex) { fgen.FILL_ERR(ex.Message.ToString().Trim() + "==> dprint ==> At the Time of Page UnLoad."); }
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
        if (data_found == "N")
        {
            return;
        }
        else
        {
            if (repDoc == null) return;
            else
            {
                repDoc.Close();
                repDoc.Dispose();
            }
        }
    }

    public void conv_pdf(DataSet dataSet, string rptFile)
    {
        //if (1 == 2)
        {
            repDoc = GetReportDocument(dataSet, rptFile);
            Stream oStream = repDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
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
    }

    public void del_file(string path)
    {
        try
        {
            fpath = Server.MapPath(path);
            if (System.IO.File.Exists(fpath)) System.IO.File.Delete(fpath);
        }
        catch { }
    }

    protected void btnexp_Click(object sender, EventArgs e)
    {
        DataSet ds = (DataSet)Session["data_set"];
        if (ds.Tables[0].Rows.Count > 0)
        {
            frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            fgen.exp_to_excel(ds.Tables[0], "ms-excel", "xls", frm_FileName);
        }
    }

    protected void btnexptopdf_Click(object sender, EventArgs e)
    {
        try
        {
            frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            DataSet ds = (DataSet)Session["data_set"];
            string rpt = (string)Session["rptfile"];
            repDoc = GetReportDocument(ds, rpt);
            repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, frm_FileName);
        }
        catch { }
    }

    protected void btnexptoexl_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            DataSet ds = (DataSet)Session["data_set"];
            string rpt = (string)Session["rptfile"];
            repDoc = GetReportDocument(ds, rpt);
            repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.Excel, Response, true, frm_FileName);
        }
        catch { }
    }

    protected void btnexptoword_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            DataSet ds = (DataSet)Session["data_set"];
            string rpt = (string)Session["rptfile"];
            repDoc = GetReportDocument(ds, rpt);
            repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.WordForWindows, Response, true, frm_FileName);
        }
        catch { }
    }

    protected void btnprint1_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DataSet ds = (DataSet)Session["data_set"];
            string rpt = (string)Session["rptfile"];
            conv_pdf(ds, rpt);
        }
        catch (Exception ex) { ex.Message.ToString(); }
    }

}