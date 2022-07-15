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

public partial class invn_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, xprdRange1, xprd1, xprd2, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, todt, branch_Cd, header_n, footer_n, footer_erp;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10, cond = " ", party_cd, part_cd, pdfView = "", data_found = "";
    double fullQty = 0, db1, db7, db8, db, db2, db6;
    double batchQty = 0;
    int srno = 1, z = 0;
    int i = 0;
    string icodecond = "";
    DataRow dr;
    fgenDB fgen = new fgenDB();
    private DataSet DsImages = new DataSet();
    FileStream FilStr = null; BinaryReader BinRed = null;

    protected void Page_Load(object sender, EventArgs e)
    { }
    protected void Page_Init(object sender, EventArgs e)
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

                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                    xprdRange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";

                    pdfView = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PDFVIEW");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", "-");

                    hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    branch_Cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BRANCH_CD");
                    footer_erp = "Generated on Finsys ERP Web";
                }
                else Response.Redirect("~/login.aspx");

            }
            if (!Page.IsPostBack)
            {
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "USEND_MAIL") == "Y") pdfView = "N";

                printCrpt(hfhcid.Value);
                CrystalReportViewer1.Focus();
                if (data_found == "N")
                {
                    No_Data_Found.Visible = true;
                    divReportViewer.Visible = false;
                }
                else
                {
                    divReportViewer.Visible = true;
                }
            }
            else
            {
                try
                {
                    CrystalReportViewer1.ReportSource = GetReportDocument((DataSet)Session["data_set"], (string)Session["rptfile"]);
                }
                catch { }
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
        string opt = "", chk_opt = "";
        data_found = "Y";
        string doc_GST = "";
        chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2027'", "fstr");
        //Member GCC Country
        if (chk_opt == "Y")
        {
            doc_GST = "GCC";
        }
        switch (iconID)
        {
            //MRR
            case "F1002":
                #region M.R.R.
                dt = new DataTable();
                cond = "and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")";
                string footerName = fgen.getOptionPW(frm_qstr, frm_cocd, "W2036", "OPT_PARAM", frm_mbr);
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1").Length > 1) { cond = "and TRIM(a.vchnum)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1") + "' and a.vchdate " + xprdRange + " "; }
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) { cond = "and TRIM(a.vchnum) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and a.vchdate " + xprdRange + " "; }

                opt = fgen.getOption(frm_qstr, frm_cocd, "W0014", "OPT_ENABLE");
                if (frm_vty == "10")
                {
                    cond = "and a.branchcd||a.type||TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(a.vchnum) in (" + barCode + ")";
                    SQuery = "select a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,f.addr1 as caddr1,f.addr2 as caddr2,f.addr3 as caddr3,f.addr4 as caddr4,f.mobile as ctel,f.aname,f.gst_no as cgst_no,f.email as cemail,t.name as mrrtype,i.unit as iunit,i.iname,i.cpartno as icpartno,b.amt_sale as totamt,b.bill_tot as grandtot, b.amt_exc as cgst_val,b.rvalue as sgst_val,B.EXCB_CHG AS TXBL,a.*,i.no_proc as pur_uom,i.hscode,'" + footerName + "' as footerName from ivoucher a,item i,famst f,type t,ivchctrl b  where trim(a.branchcd)||trim(a.type)||TRIM(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||TRIM(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and trim(a.type)=trim(t.type1) and t.id='M' " + cond + " and a.store<>'R' ORDER BY a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd'),A.MORDER";
                }
                else
                {
                    SQuery = "select a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,f.addr1 as caddr1,f.addr2 as caddr2,f.addr3 as caddr3,f.addr4 as caddr4,f.mobile as ctel,f.aname,f.gst_no as cgst_no,f.email as cemail,t.name as mrrtype,i.unit as iunit,i.iname,i.cpartno as icpartno,b.amt_sale as totamt,b.bill_tot as grandtot, b.amt_exc as cgst_val,b.rvalue as sgst_val,B.EXCB_CHG AS TXBL,a.*,i.no_proc as pur_uom,i.hscode,'" + footerName + "' as footerName from ivoucher a,item i,famst f,type t,ivchctrl b  where trim(a.branchcd)||trim(a.type)||TRIM(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||TRIM(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and trim(a.type)=trim(t.type1) and t.id='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' " + cond + " and a.store<>'R' ORDER BY a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd'),A.MORDER";
                    if (frm_vty == "0")
                        SQuery = "select a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,f.addr1 as caddr1,f.addr2 as caddr2,f.addr3 as caddr3,f.addr4 as caddr4,f.mobile as ctel,f.aname,f.gst_no as cgst_no,f.email as cemail,t.name as mrrtype,i.unit as iunit,i.iname,i.cpartno as icpartno,b.amt_sale as totamt,b.bill_tot as grandtot, b.amt_exc as cgst_val,b.rvalue as sgst_val,B.EXCB_CHG AS TXBL,a.*,i.no_proc as pur_uom,i.hscode,'" + footerName + "' as footerName from ivoucher a,item i,famst f,type t,ivchctrl b  where trim(a.branchcd)||trim(a.type)||TRIM(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||TRIM(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and trim(a.acode)=trim(f.acode) and trim(a.icode)=trim(i.icode) and trim(a.type)=trim(t.type1) and t.id='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE LIKE '0%' " + cond + " and a.store<>'R' ORDER BY a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd'),A.MORDER";
                }
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                dt2 = new DataTable();
                SQuery = "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr_sub,a.vchnum as vchnum_sub,a.vchdate as vchdate_sub,A.icode as icode_sub,a.srno as srno_sub,a.coreelno as coreelno_sub,a.kclreelno as kclreelno_sub,a.reelwin as reelwin_sub,a.reelwout as reelwout_sub,a.irate as irate_sub,a.reelspec1 as reelspec1_sub,a.reelspec2 as reelspec2_sub,a.psize as psize_sub,a.gsm as gsm_sub,a.uinsp as uinsp_sub,a.reelmtr as reelmtr_sub,b.iname,b.hscode,'" + footerName + "' as footerName FROM REELVCH A,item b WHERE trim(a.icodE)=trim(B.icode) and a.branchcd='" + frm_mbr + "' and A.TYPE LIKE '0%' " + cond + " order by a.srno";
                dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    //BarCode adding
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dt2.TableName = "subrept";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    dsRep.Tables.Add(dt2);

                    if (frm_rptName == "0") frm_rptName = "std_mrr";
                    opt = fgen.getOption(frm_qstr, frm_cocd, "W0097", "OPT_ENABLE");
                    if (opt == "Y") frm_rptName = "std_mrr_batch";
                    if (doc_GST == "GCC") frm_rptName = "std_mrr_intl";

                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_mrr", frm_rptName, dsRep, "M.R.R Report", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            //MRR Sticker
            case "S1002":
                #region MRR Sticker
                //old qry
                //SQuery = "Select a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd')||trim(a.icode)||trim(a.btchno) as fstr,A.MORDER,d.name as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,b.aname,a.btchno,a.iqtyin,A.IQTY_WT,a.invno,a.invdate,a.col1,c.packsize from ivoucher a,famst b ,item c,type d where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.type=d.type1 and d.id='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") and a.store<>'R' ORDER BY VDATE,a.vchnum,A.MORDER";//old qry
                //if (frm_cocd == "ROYL") SQuery = "Select a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd')||trim(a.icode)||trim(a.btchno) as fstr,A.MORDER,d.name as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,c.unit,b.aname,a.btchno,a.iqtyin,A.IQTY_WT,a.invno,a.invdate,a.col1,c.packsize,a.no_bdls as no_of_pkt from ivoucher a,famst b ,item c,type d where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.type=d.type1 and d.id='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") and a.store<>'R' ORDER BY VDATE,a.vchnum,A.MORDER";////old qry

                //this is for royl and others...yogita
                SQuery = "Select a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd')||trim(a.icode)||trim(a.btchno) as fstr,A.MORDER,d.name as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,c.unit,b.aname,a.btchno,a.iqtyin,A.IQTY_WT,a.invno,a.invdate,a.col1,c.packsize,a.no_bdls as no_of_pkt from ivoucher a,famst b ,item c,type d where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.type=d.type1 and d.id='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") and a.store<>'R' ORDER BY VDATE,a.vchnum,A.MORDER";
                if (frm_cocd == "SVPL" || frm_cocd == "MINV") SQuery = "Select a.branchcd,trim(e.icode)||trim(e.kclreelno) as fstr,(E.SRNO+1) as srno,e.reelwin as PACKSIZE,A.MORDER,d.name as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'MON-YY') as vchmnth,trim(a.icode) as icode,trim(a.acode) as acode,trim(c.iname) as iname,(CASE WHEN LENGTH(NVL(e.RLOCN,'-'))>2 THEN A.LOCATION ELSE c.binno END) as locn,trim(b.aname) as aname,a.btchno,nvl(a.iqtyin,0) as iqtyin,nvl(A.IQTY_WT,0) as iqty_wt,a.invno,a.invdate,a.col1,c.packsize AS PACKSIZE2,a.ent_by,a.ent_dt,A.NARATION,c.packsize from ivoucher a,famst b ,item c,type d,reelvch e where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)=e.branchcd||e.type||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy')||trim(e.icode) and a.type=d.type1 and d.id='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") and a.store!='R' ORDER BY VDATE,a.vchnum,E.SRNO";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (frm_cocd == "SVPL")
                {
                    dt1.TableName = "barcode";
                    dt1 = fgen.addBarCode(dt1, "fstr", true);
                    dsRep.Tables.Add(dt1);

                    frm_rptName = "mrr_stk";
                    if (frm_cocd == "SVPL" || frm_cocd == "MINV") frm_rptName = "mrr_stk_svpl";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "mrr_stk", frm_rptName, dsRep, "Sticker", "Y");
                }
                else
                {
                    if (dt1.Rows.Count > 0)
                    {
                        dr = null;
                        fullQty = 0;
                        batchQty = 0;
                        srno = 1;

                        dt = new DataTable();
                        dt = dt1.Clone();
                        dt.Columns.Add("vchnum", typeof(string));
                        dt.Columns.Add("binqty", typeof(double));
                        dt.Columns.Add("header", typeof(string));
                        dt.Columns.Add("srno", typeof(string));
                        dr1 = null;
                        foreach (DataRow dtr1 in dt1.Rows)
                        {
                            fullQty = fgen.make_double(dtr1["iqtyin"].ToString());
                            batchQty = fgen.make_double(dtr1["packsize"].ToString());
                            //if (fullQty == batchQty && z == 0) break;
                            if (batchQty == 0) batchQty = fullQty;
                            do
                            {
                                dr1 = dt.NewRow();
                                foreach (DataColumn dc in dt1.Columns)
                                {
                                    dr1[dc.ColumnName] = dtr1[dc.ColumnName].ToString().Trim();
                                }
                                if (fullQty <= batchQty)
                                {
                                    batchQty = fullQty;
                                    fullQty = fullQty - batchQty;
                                }
                                else fullQty = fullQty - batchQty;
                                dr1["binqty"] = batchQty;
                                dr1["IQTYIN"] = batchQty;
                                //dr1["fstr"] = dtr1["fstr"].ToString() + "~" + srno.ToString() + "~" + batchQty.ToString();
                                dr1["header"] = "";
                                dr1["vchnum"] = dr1["fstr"].ToString();
                                dt.Rows.Add(dr1);
                                srno++;
                            }
                            while (fullQty != 0);
                            z++;
                        }

                        dt.TableName = "barcode";
                        dt = fgen.addBarCode(dt, "fstr", true);
                        dsRep.Tables.Add(fgen.mTitle(dt, 1));

                        //dt1.TableName = "Prepcur";
                        //dsRep.Tables.Add(dt1);


                        //dt1.TableName = "barcode";
                        //dt1 = fgen.addBarCode(dt1, "fstr", true);
                        //dsRep.Tables.Add(dt1);

                        frm_rptName = "mrr_stk";
                        if (frm_cocd == "ROYL") frm_rptName = "mrr_stk_ROYL";//by yogita....set sticker left and right side with 50mm size
                        if (frm_cocd == "SVPL" || frm_cocd == "MINV") frm_rptName = "mrr_stk_svpl";
                        Print_Report_BYDS(frm_cocd, frm_mbr, "mrr_stk", frm_rptName, dsRep, "Sticker", "Y");
                    }
                }
                #endregion
                break;

            case "S1002G":
                #region Gate Sticker
                if (frm_cocd == "ROYL")
                {
                    // SQuery = "Select a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd') as fstr,'Gate Entry' as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.acode,b.aname,sum(a.iqty_chl) as iqtyin,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,'-' as col1,count(a.icode) as icodes,a.mode_tpt,a.mtime,a.ponum,to_char(a.podate,'dd/mm/yyyy') as podate  from ivoucherp a,famst b ,item c where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") group by a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd'),a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD'),to_char(a.vchdate,'DD/MM/YYYY'),a.acode,b.aname,a.invno,to_char(a.invdate,'dd/mm/yyyy'),a.mode_tpt,a.mtime,a.ponum,to_char(a.podate,'dd/mm/yyyy') ORDER BY VDATE,a.vchnum";
                    SQuery = "Select a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd') as fstr,'Gate Entry' as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.acode,b.aname,sum(a.iqty_chl) as iqtyin,(case when length(trim(a.invno))<2 then a.refnum else a.invno end) as INVNO,to_char(a.invdate,'dd/mm/yyyy') as invdate,'-' as col1,count(a.icode) as icodes,a.mode_tpt,a.mtime,a.ponum,to_char(a.podate,'dd/mm/yyyy') as podate  from ivoucherp a,famst b ,item c where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") group by a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd'),a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD'),to_char(a.vchdate,'DD/MM/YYYY'),a.acode,b.aname,a.invno,to_char(a.invdate,'dd/mm/yyyy'),a.mode_tpt,a.mtime,a.ponum,to_char(a.podate,'dd/mm/yyyy'),(case when length(trim(a.invno))<2 then a.refnum else a.invno end) ORDER BY VDATE,a.vchnum";//yogita...as per ashok sir
                }
                else
                {
                    SQuery = "Select a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd')||trim(a.icode)||trim(a.btchno) as fstr,A.MORDER,'Gate Entry' as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,b.aname,a.btchno,a.iqty_chl as iqtyin,A.IQTY_WT,a.invno,a.invdate,'-' as col1 from ivoucherp a,famst b ,item c where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') ='" + barCode + "'  ORDER BY VDATE,a.vchnum,A.MORDER";
                    SQuery = "Select a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd') as fstr,'Gate Entry' as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.acode,b.aname,sum(a.iqty_chl) as iqtyin,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,'-' as col1,count(a.icode) as icodes,a.mode_tpt,a.mtime from ivoucherp a,famst b ,item c where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") group by a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd'),a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD'),to_char(a.vchdate,'DD/MM/YYYY'),a.acode,b.aname,a.invno,to_char(a.invdate,'dd/mm/yyyy'),a.mode_tpt,a.mtime ORDER BY VDATE,a.vchnum";
                }
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt1.Rows.Count > 0)
                {
                    dt1.TableName = "barcode";
                    dt1 = fgen.addBarCode(dt1, "fstr", true);

                    if (frm_cocd == "ROYL")
                    {
                        dsRep.Tables.Add(fgen.mTitle(dt1, 2));
                        frm_rptName = "mrr_stk_gt_ROYL";
                    }
                    else
                    {
                        dsRep.Tables.Add(dt1);
                        frm_rptName = "mrr_stk_gt";
                    }
                    Print_Report_BYDS(frm_cocd, frm_mbr, "mrr_stk_gt", frm_rptName, dsRep, "Sticker", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;


            //REEL Sticker
            case "S1002R":
            case "F25198A":
            case "F25397":
            case "F25396":
            case "F25395":
                #region MRR Sticker
                if (frm_cocd == "SRPF")
                    SQuery = "Select s.col18,s.col17,a.branchcd,trim(e.kclreelno) as fstr,e.kclreelno,e.REELWIN,A.MORDER,d.name as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,c.cpartno,b.aname,a.btchno,a.iqtyin,A.IQTY_WT,a.invno,a.invdate,a.col1,a.tpt_names,a.mr_gdate,c.unit,e.coreelno,c.oprate1 psize,c.oprate3 gsm from ivoucher a,famst b ,item c,type d,reelvch e,finprim.scratch s where trim(e.kclreelno)=trim(s.col2) and trim(c.cpartno)=trim(s.icode) and trim(a.icode)=trim(c.icode) and trim(e.icode)=trim(c.icodE) and trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_chaR(a.vchdate,'dd/mm/yyyy')=e.branchcd||e.type||trim(e.vchnum)||to_chaR(e.vchdate,'dd/mm/yyyy') and a.type=d.type1 and d.id='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  ORDER BY VDATE,a.vchnum,A.MORDER";
                else
                    SQuery = "Select a.branchcd,trim(e.kclreelno) as fstr,e.kclreelno,e.REELWIN,A.MORDER,d.name as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,c.cpartno,b.aname,a.btchno,a.iqtyin,A.IQTY_WT,a.invno,a.invdate,a.col1,a.tpt_names,a.mr_gdate,c.unit,e.coreelno,c.oprate1 psize,c.oprate3 gsm from ivoucher a,famst b ,item c,type d,reelvch e where trim(a.icode)=trim(c.icode) and trim(e.icode)=trim(c.icodE) and trim(a.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_chaR(a.vchdate,'dd/mm/yyyy')=e.branchcd||e.type||trim(e.vchnum)||to_chaR(e.vchdate,'dd/mm/yyyy') and a.type=d.type1 and d.id='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  ORDER BY VDATE,a.vchnum,A.MORDER";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt1.Rows.Count > 0)
                {
                    dt1.TableName = "barcode";
                    dt1 = fgen.addBarCode(dt1, "fstr", true);
                    dsRep.Tables.Add(dt1);
                    frm_rptName = "reel_stk";
                    switch (iconID)
                    {
                        case "F25395":
                            frm_rptName = "reel_stka4";
                            break;
                        case "F25396":
                            frm_rptName = "reel_stka5";
                            break;
                    }
                    if (frm_cocd == "SRPF")
                        Print_Report_BYDS(frm_cocd, frm_mbr, "reel_stk_SRPF", "reel_stk_SRPF", dsRep, "Sticker", "Y");
                    else
                        Print_Report_BYDS(frm_cocd, frm_mbr, "reel_stk", frm_rptName, dsRep, "Sticker", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F25198C":
                #region MRR Sticker
                SQuery = "Select a.branchcd,trim(e.kclreelno) as fstr,e.kclreelno,e.REELWIN,A.MORDER,d.name as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,c.cpartno,b.aname,a.btchno,a.iqtyin,A.IQTY_WT,a.invno,a.invdate,a.col1,a.tpt_names,a.mr_gdate,c.unit,e.coreelno,c.oprate1 psize,c.oprate3 gsm from ivoucher a,famst b ,item c,type d,reelvch e where trim(a.icode)=trim(c.icode) and trim(e.icode)=trim(c.icodE) and trim(e.acode)=trim(b.acode) and a.branchcd||a.type||trim(a.vchnum)||to_chaR(a.vchdate,'dd/mm/yyyy')=e.branchcd||e.type||trim(e.vchnum)||to_chaR(e.vchdate,'dd/mm/yyyy') and a.type=d.type1 and d.id='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  ORDER BY VDATE,a.vchnum,A.MORDER";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt1.Rows.Count > 0)
                {
                    dt1.TableName = "barcode";
                    dt1 = fgen.addBarCode(dt1, "fstr", true);
                    dsRep.Tables.Add(dt1);
                    frm_rptName = "reel_stk";
                    switch (iconID)
                    {
                        case "F25395":
                            frm_rptName = "reel_stka4";
                            break;
                        case "F25396":
                            frm_rptName = "reel_stka5";
                            break;
                    }
                    Print_Report_BYDS(frm_cocd, frm_mbr, "reel_stk", frm_rptName, dsRep, "Sticker", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F25198B":
                string cDT1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                string cDT2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                fromdt = cDT1;
                todt = cDT2;
                string xprdrange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                xprd1 = "between to_date('" + cDT1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                xprd2 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                xprdrange = xprd2;
                string xprd3 = " between to_date('" + cDT1 + "','dd/mm/yyyy') and to_Date('" + todt + "','dd/mm/yyyy')";
                string typstring = "'07','08','09'";
                string icodecond = "and substr(icode,1,2) in (" + typstring + ") ";
                string reel_V_tbl = "reelvch";
                icodecond = "";

                mq0 = "select b.iname,b.cpartno,b.pur_uom,b.bfactor,b.oprate1 as psize,b.oprate3 as gsm,b.oprate1,b.oprate2,b.oprate3,trim(a.kclreelno)as My_reel,min(vchdate) as Vchdate,max(trim(upper(a.coreelno))) as Co_reel,trim(a.icode) as Icode,sum(a.opening) as op,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as closing,MAX(aCODE) AS ACODE,substr(a.icode,1,4) as Igrp,max(insp_done) as Insp_done,max(origwt) as origwt,max(rlocn) as rlocn,max(reel_mill) as reel_mill from (Select null as vchdate,kclreelno,null as coreelno,icode, reelwin as opening,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE,null as insp_done,0 as origwt,rlocn,'-' as reel_mill from " + reel_V_tbl + " where branchcd='" + frm_mbr + "' " + icodecond + " and substr(nvl(rinsp_by,'-'),1,6)='REELOP' and 1=2 union all  ";
                mq1 = "select min(vchdate) As vchdate,kclreelno,coreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as cdr,0 as ccr,0 as clos,MAX(ACODE) AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' as reel_mill from " + reel_V_tbl + " where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y' " + icodecond + " GROUP BY type,kclreelno,coreelno,ICODE union all ";
                mq2 = "select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,sum(reelwin) as cdr,sum(reelwout) as ccr,0 as clos,MAX(aCODE) AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from " + reel_V_tbl + " where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y' " + icodecond + " GROUP BY type,kclreelno,coreelno,ICODE )a,item b where trim(a.icode)=trim(B.icode) and nvl(b.oprate1,0) like '%' and nvl(b.oprate3,0) like '%' and nvl(b.bfactor,0) like '%'  group by b.iname,b.cpartno,b.pur_uom,b.bfactor,b.oprate1,b.oprate2,b.oprate3,trim(a.icode),substr(a.icode,1,4),trim(a.kclreelno)  ";
                SQuery = "create or replace view REEL_DSTK_" + frm_mbr + " as(SELECT * FROM (" + mq0 + mq1 + mq2 + ")m where 1=1 and nvl(m.aCODE,'%') like '%' )";
                fgen.execute_cmd(frm_qstr, frm_cocd, SQuery);

                SQuery = "Select '" + frm_mbr + "' as branchcd,trim(e.my_reel) as fstr,e.my_reel kclreelno,e.closing REELWIN,1 as MORDER,null as header,null as type,'-' as vchnum,sysdate as vdate,sysdate as vchdate,e.icode,E.acode,c.iname,c.cpartno,b.aname,null btchno,0 as iqtyin,0 as IQTY_WT,null as invno,sysdate as invdate,null as col1,null as tpt_names,sysdate as mr_gdate,c.unit,e.co_reel coreelno,c.oprate1 psize,c.oprate3 gsm from item c,reel_dstk_" + frm_mbr + " e left outer join famst b on trim(b.acode)=trim(e.acode) where trim(e.icode)=trim(c.icode) and TRIM(e.my_reel) in ('" + barCode + "')  ";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt1.Rows.Count > 0)
                {
                    dt1.TableName = "barcode";
                    dt1 = fgen.addBarCode(dt1, "fstr", true);
                    dsRep.Tables.Add(dt1);
                    frm_rptName = "reel_stk";
                    if (frm_cocd == "SVPL" || frm_cocd == "MINV") frm_rptName = "mrr_stk_svpl";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "mrr_stk", frm_rptName, dsRep, "Sticker", "Y");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25245R":
                #region Return Sticker
                if (!barCode.Contains("'")) barCode = "'" + barCode + "'";
                SQuery = "Select a.branchcd,e.kclreelno as fstr,e.reelwin as iqtyin,A.MORDER,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,a.btchno,a.iqtyin as ivchin,A.IQTY_WT,a.invno,a.invdate,a.col1,c.packsize from ivoucher a,item c,reelvch e where trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)=e.branchcd||e.type||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy')||trim(e.icode) AND a.branchcd||a.type||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY VDATE,a.vchnum,A.MORDER";
                SQuery = "Select a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr, a.branchcd,A.MORDER,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,a.btchno,a.IQTY_CHL as iqtyin,a.IQTY_CHL as ivchin,nvl(A.IQTY_WT,0) as IQTY_WT,a.invno,nvl(a.invdate,sysdate) as INVDATE,nvl(a.col1,0) as COL1,c.packsize,a.ent_by,a.ent_dt,to_char(a.vchdate,'MON-YY') as vchmnth from ivoucher a,item c where trim(a.icode)=trim(c.icode) AND a.branchcd||a.type||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY VDATE,a.vchnum,A.MORDER";
                if (frm_cocd == "SVPL*") SQuery = "Select a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr, a.branchcd,A.MORDER,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,a.btchno,a.IQTY_CHL as iqtyin,a.IQTY_CHL as ivchin,nvl(A.IQTY_WT,0) as IQTY_WT,a.invno,nvl(a.invdate,sysdate) as INVDATE,nvl(a.col1,0) as COL1,c.packsize,a.ent_by,a.ent_dt from ivoucher a,item c where trim(a.icode)=trim(c.icode) AND a.branchcd||a.type||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY VDATE,a.vchnum,A.MORDER";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                dr = null;
                dt = new DataTable();
                dt = dt1.Clone();
                dt.Columns.Add("binqty", typeof(double));
                dt.Columns.Add("header", typeof(string));
                fullQty = 0;
                batchQty = 0;
                srno = 1;
                //remove as per ravi sir's request
                if (1 == 2)
                {
                    foreach (DataRow dtr1 in dt1.Rows)
                    {
                        dr = dt.NewRow();
                        foreach (DataColumn dc in dt1.Columns)
                        {
                            dr[dc.ColumnName] = dtr1[dc.ColumnName].ToString().Trim();
                        }
                        dr["binqty"] = fgen.make_double(dtr1["iqtyin"].ToString());
                        dr["fstr"] = dtr1["fstr"].ToString() + "~" + srno.ToString() + "~" + dtr1["iqtyin"].ToString();
                        dr["header"] = "Main Sticker";
                        dt.Rows.Add(dr);
                        srno++;
                    }
                }
                z = 0;
                foreach (DataRow dtr1 in dt1.Rows)
                {
                    fullQty = fgen.make_double(dtr1["iqtyin"].ToString());
                    batchQty = fgen.make_double(dtr1["packsize"].ToString());
                    //if (fullQty == batchQty && z == 0) break;
                    if (batchQty == 0) batchQty = fullQty;
                    do
                    {
                        dr = dt.NewRow();
                        foreach (DataColumn dc in dt1.Columns)
                        {
                            dr[dc.ColumnName] = dtr1[dc.ColumnName].ToString().Trim();
                        }
                        if (fullQty <= batchQty)
                        {
                            batchQty = fullQty;
                            fullQty = fullQty - batchQty;
                        }
                        else fullQty = fullQty - batchQty;
                        dr["binqty"] = batchQty;
                        dr["fstr"] = dtr1["fstr"].ToString() + "~" + srno.ToString() + "~" + batchQty.ToString();
                        dr["header"] = "";
                        dt.Rows.Add(dr);
                        srno++;
                    }
                    while (fullQty != 0);
                    z++;
                }
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "barcode";
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dsRep.Tables.Add(fgen.mTitle(dt, 1));
                    frm_rptName = "ret_stk_svpl";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "ret_stk", frm_rptName, dsRep, "Sticker", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F25194":
                double totqty; double packqty; double fillqty;
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                header_n = "WIP Transfer Material Tags";
                SQuery = "select '" + header_n + "' as header , 0 as tagqty, A.vchnum ,to_char(A.vchdate,'dd/mm/yyyy') as vchdate , A.IQTYOUT, B.NAME, T.NAME AS WipNAME , C.ICODE , C.INAME , C.BINNO , C.PACKSIZE , d.ANAME  from ivoucher a, type b,TYPE T ,ITEM C LEFT JOIN FAMST D ON TRIM(C.AC_ACODE)=TRIM(D.ACODE)  where TRIM(A.ACODE)=TRIM(B.TYPE1) AND TRIM(A.IOPR)=TRIM(T.TYPE1)  AND TRIM(A.ICODE)=TRIM(C.ICODE)  AND A.branchcd||A.type||A.vchnum||to_char(A.vchdate,'dd/mm/yyyy') in (" + mq0 + ") AND B.ID='1' AND T.ID='1' ORDER BY INAME ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt.Columns.Add("grp", typeof(string));
                dt1 = new DataTable();
                dt1 = dt.Clone();
                dr1 = null;
                int srn = 0;
                foreach (DataRow drwIP in dt.Rows)
                {
                    totqty = fgen.make_double(drwIP["iqtyout"].ToString().Trim());
                    packqty = fgen.make_double(drwIP["packsize"].ToString().Trim());
                    if (packqty == 0) packqty = totqty;
                    fillqty = 0;
                    do
                    {
                        if (totqty > packqty)
                        {
                            fillqty = packqty;
                            totqty = totqty - packqty;
                        }
                        else
                        {
                            fillqty = totqty;
                            totqty = totqty - fillqty;
                        }
                        dr1 = dt1.NewRow();
                        dr1["tagqty"] = fillqty;
                        dr1["grp"] = drwIP["icode"].ToString().Trim() + fillqty + srn;
                        dr1["vchnum"] = drwIP["vchnum"].ToString().Trim();
                        dr1["vchdate"] = Convert.ToDateTime(drwIP["vchdate"].ToString().Trim()).ToString("dd/MM/yyyy");
                        dr1["iname"] = drwIP["iname"].ToString().Trim();
                        dr1["aname"] = drwIP["aname"].ToString().Trim();
                        dr1["name"] = drwIP["name"].ToString().Trim();
                        dr1["wipname"] = drwIP["wipname"].ToString().Trim();
                        dr1["icode"] = drwIP["icode"].ToString().Trim();
                        dr1["packsize"] = drwIP["packsize"].ToString().Trim();
                        dr1["binno"] = drwIP["binno"].ToString().Trim();
                        dr1["iqtyout"] = drwIP["iqtyout"].ToString().Trim();
                        dt1.Rows.Add(dr1);
                        srn++;
                    }
                    while (totqty != 0);
                }
                if (dt1.Rows.Count > 0)
                {
                    dt1.TableName = "Prepcur";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "AmarWipStk", "AmarWipStk", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25245RA":
                #region Return Sticker
                if (!barCode.Contains("'")) barCode = "'" + barCode + "'";
                SQuery = "Select a.branchcd,e.kclreelno as fstr,e.reelwin as iqtyin,A.MORDER,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,a.btchno,a.iqtyin as ivchin,A.IQTY_WT,a.invno,a.invdate,a.col1,c.packsize from ivoucher a,item c,reelvch e where trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)=e.branchcd||e.type||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy')||trim(e.icode) AND a.branchcd||a.type||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY VDATE,a.vchnum,A.MORDER";
                SQuery = "Select a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr, a.branchcd,A.MORDER,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,a.btchno,a.IQTY_CHL as iqtyin,a.IQTY_CHL as ivchin,nvl(A.IQTY_WT,0) as IQTY_WT,a.invno,nvl(a.invdate,sysdate) as INVDATE,nvl(a.col1,0) as COL1,c.packsize,a.ent_by,a.ent_dt from ivoucher a,item c where trim(a.icode)=trim(c.icode) AND a.branchcd||a.type||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY VDATE,a.vchnum,A.MORDER";
                //if (frm_cocd == "SVPL*") SQuery = "Select a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr, a.branchcd,A.MORDER,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,a.btchno,a.IQTY_CHL as iqtyin,a.IQTY_CHL as ivchin,nvl(A.IQTY_WT,0) as IQTY_WT,a.invno,nvl(a.invdate,sysdate) as INVDATE,nvl(a.col1,0) as COL1,c.packsize,a.ent_by,a.ent_dt from ivoucher a,item c where trim(a.icode)=trim(c.icode) AND a.branchcd||a.type||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY VDATE,a.vchnum,A.MORDER";
                SQuery = "Select a.branchcd,e.kclreelno as fstr1,e.reelwin as PACKSIZE,A.MORDER,d.name as header,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'MON-YY') as vchmnth,a.icode,a.acode,c.iname,(CASE WHEN LENGTH(NVL(e.RLOCN,'-'))>2 THEN A.LOCATION ELSE c.binno END) as locn,a.btchno,a.iqtyin,nvl(A.IQTY_WT,0) as IQTY_WT,c.packsize AS PACKSIZE2,a.ent_by,a.ent_dt from ivoucher a,item c,type d,reelvch e where trim(a.icode)=trim(c.icode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode)=e.branchcd||e.type||trim(e.vchnum)||to_char(e.vchdate,'dd/mm/yyyy')||trim(e.icode) and a.type=d.type1 and d.id='M' AND a.branchcd||a.type||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  ORDER BY VDATE,a.vchnum,A.MORDER";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                dr = null;
                dt = new DataTable();
                dt = dt1.Clone();
                dt.Columns.Add("binqty", typeof(double));
                dt.Columns.Add("header", typeof(string));

                DataColumn dcNew = new DataColumn();
                dcNew.ColumnName = "fstr";
                dcNew.MaxLength = 100;
                dt.Columns.Add(dcNew);

                fullQty = 0;
                batchQty = 0;
                srno = 1;
                //remove as per ravi sir's request
                if (1 == 2)
                {
                    foreach (DataRow dtr1 in dt1.Rows)
                    {
                        dr = dt.NewRow();
                        foreach (DataColumn dc in dt1.Columns)
                        {
                            dr[dc.ColumnName] = dtr1[dc.ColumnName].ToString().Trim();
                        }
                        dr["binqty"] = fgen.make_double(dtr1["iqtyin"].ToString());
                        dr["fstr"] = dtr1["fstr1"].ToString().Trim();
                        dr["header"] = "Main Sticker";
                        dt.Rows.Add(dr);
                        srno++;
                    }
                }
                z = 0;
                foreach (DataRow dtr1 in dt1.Rows)
                {
                    fullQty = fgen.make_double(dtr1["iqtyin"].ToString());
                    batchQty = fgen.make_double(dtr1["packsize"].ToString());
                    //if (fullQty == batchQty && z == 0) break;
                    if (batchQty == 0) batchQty = fullQty;
                    do
                    {
                        dr = dt.NewRow();
                        foreach (DataColumn dc in dt1.Columns)
                        {
                            dr[dc.ColumnName] = dtr1[dc.ColumnName].ToString().Trim();
                        }
                        if (fullQty <= batchQty)
                        {
                            batchQty = fullQty;
                            fullQty = fullQty - batchQty;
                        }
                        else fullQty = fullQty - batchQty;
                        dr["binqty"] = batchQty;
                        dr["fstr"] = dtr1["fstr1"].ToString().Trim();
                        dr["header"] = "";
                        dt.Rows.Add(dr);
                        srno++;
                    }
                    while (fullQty != 0);
                    z++;
                }
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "barcode";
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dsRep.Tables.Add(fgen.mTitle(dt, 1));
                    frm_rptName = "ret_stk_svplR";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "ret_stk", frm_rptName, dsRep, "Sticker", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F40234":
                SQuery = "Select a.branchcd,trim(a.binno) as fstr,A.MORDER,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,c.cpartno,C.MAT5,a.btchno,c.packsize,a.iqtyin,a.iqtyin as binqty ,nvl(A.IQTY_WT,0) as IQTY_WT,a.invno,a.invdate,a.ent_by,a.ent_dt,to_char(a.vchdate,'MON-YY') as vchmnth from ivoucher a,item c where trim(a.icode)=trim(c.icode) AND a.branchcd||a.type||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in ('" + barCode + "') /*and a.store!='W'*/ and a.iqtyin>0 ORDER BY VDATE,a.vchnum,A.MORDER";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "barcode";
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dsRep.Tables.Add(fgen.mTitle(dt, 1));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "fg_stk", "fg_stk_vigp", dsRep, "Sticker", "Y");
                }
                else
                {
                    data_found = "N";
                }
                break;
            //FG Sticker
            case "F25245A":
                #region FG Sticker
                if (!barCode.Contains("'")) barCode = "'" + barCode + "'";
                if (frm_cocd == "SACL")
                {
                    SQuery = "Select distinct a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd')||trim(a.icode) as fstr,A.MORDER,a.type,a.vchnum as docno,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,c.cpartno,C.MAT5,a.btchno,(case when a.iqtyin>0 then a.iqtyin else a.iqty_chl end) as iqtyin,A.iqtyin as IQTY_WT,a.invno,a.invdate,c.packsize from ivoucher a,item c where trim(a.icode)=trim(c.icode) AND a.branchcd||a.type||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  ORDER BY VDATE,a.vchnum,A.MORDER";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    //**** BATCH QTY FORM                
                    dt = new DataTable();
                    dt = dt1.Clone();
                    dt.Columns.Add("binqty", typeof(double));
                    dt.Columns.Add("header", typeof(string));
                    dt.Columns.Add("srno", typeof(string));
                    dt.Columns.Add("vchnum", typeof(string));
                    dr1 = null;
                    foreach (DataRow dtr1 in dt1.Rows)
                    {
                        fullQty = fgen.make_double(dtr1["iqtyin"].ToString());
                        batchQty = fgen.make_double(dtr1["packsize"].ToString());
                        //if (fullQty == batchQty && z == 0) break;
                        if (batchQty == 0) batchQty = fullQty;
                        do
                        {
                            dr1 = dt.NewRow();
                            foreach (DataColumn dc in dt1.Columns)
                            {
                                dr1[dc.ColumnName] = dtr1[dc.ColumnName].ToString().Trim();
                            }
                            if (fullQty <= batchQty)
                            {
                                batchQty = fullQty;
                                fullQty = fullQty - batchQty;
                            }
                            else fullQty = fullQty - batchQty;
                            dr1["binqty"] = batchQty;
                            //dr1["fstr"] = dtr1["fstr"].ToString() + "~" + srno.ToString() + "~" + batchQty.ToString();
                            dr1["header"] = "";
                            dr1["vchnum"] = dr1["docno"].ToString() + "" + srno.ToString();
                            dt.Rows.Add(dr1);
                            srno++;
                        }
                        while (fullQty != 0);
                        z++;
                    }

                    dt.TableName = "barcode";
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dsRep.Tables.Add(fgen.mTitle(dt, 1));


                    Print_Report_BYDS(frm_cocd, frm_mbr, "fg_stk", "prod_stkSACL", dsRep, "Sticker", "Y");
                }
                else
                {
                    SQuery = "Select distinct a.branchcd,a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'yyyymmdd')||trim(a.icode) as fstr,A.MORDER,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,c.cpartno,C.MAT5,a.btchno,c.packsize,a.iqtyin ,nvl(A.IQTY_WT,0) as IQTY_WT,a.invno,a.invdate,a.ent_by,a.ent_dt,to_char(a.vchdate,'MON-YY') as vchmnth from ivoucher a,item c where trim(a.icode)=trim(c.icode) AND a.branchcd||a.type||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") /*and a.store!='W'*/ ORDER BY VDATE,a.vchnum,A.MORDER";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dr = null;
                    dt = new DataTable();
                    dt = dt1.Clone();
                    dt.Columns.Add("binqty", typeof(double));
                    dt.Columns.Add("header", typeof(string));
                    fullQty = 0;
                    batchQty = 0;
                    srno = 1;
                    //remove as per ravi sir's request
                    if (1 == 2)
                    {
                        foreach (DataRow dtr1 in dt1.Rows)
                        {
                            dr = dt.NewRow();
                            foreach (DataColumn dc in dt1.Columns)
                            {
                                dr[dc.ColumnName] = dtr1[dc.ColumnName].ToString().Trim();
                            }
                            dr["binqty"] = fgen.make_double(dtr1["iqtyin"].ToString());
                            dr["fstr"] = dtr1["fstr"].ToString() + "~" + srno.ToString();
                            dr["header"] = "Main Sticker";
                            dt.Rows.Add(dr);
                            srno++;
                        }
                    }
                    z = 0;
                    foreach (DataRow dtr1 in dt1.Rows)
                    {
                        fullQty = fgen.make_double(dtr1["iqtyin"].ToString());
                        batchQty = fgen.make_double(dtr1["packsize"].ToString());
                        //if (fullQty == batchQty && z == 0) break;
                        if (batchQty == 0) batchQty = fullQty;
                        do
                        {
                            dr = dt.NewRow();
                            foreach (DataColumn dc in dt1.Columns)
                            {
                                dr[dc.ColumnName] = dtr1[dc.ColumnName].ToString().Trim();
                            }
                            if (fullQty <= batchQty)
                            {
                                batchQty = fullQty;
                                fullQty = fullQty - batchQty;
                            }
                            else fullQty = fullQty - batchQty;
                            dr["binqty"] = batchQty;
                            dr["fstr"] = dtr1["fstr"].ToString() + "~" + batchQty + "~" + srno.ToString();
                            dr["header"] = "";
                            dt.Rows.Add(dr);
                            srno++;
                        }
                        while (fullQty != 0);
                        z++;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "barcode";
                        dt = fgen.addBarCode(dt, "fstr", true);
                        dsRep.Tables.Add(fgen.mTitle(dt, 1));
                        Print_Report_BYDS(frm_cocd, frm_mbr, "fg_stk", "fg_stk", dsRep, "Sticker", "Y");
                    }
                    else
                    {
                        data_found = "N";
                    }
                }
                #endregion
                break;

            case "F25245S":
                SQuery = "Select a.branchcd||a.type||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')||trim(a.icode) as fstr, a.branchcd,A.MORDER,a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,trim(a.icode) as icode,trim(a.acode) as acode,trim(c.iname) as iname,a.btchno,nvl(a.iqtyin,0) as iqtyin,nvl(a.IQTY_CHL,0) as ivchin,nvl(A.IQTY_WT,0) as iqty_wt,a.invno,a.invdate,a.col1,c.packsize from ivoucherW a,item c where trim(a.icode)=trim(c.icode) AND a.branchcd||a.type||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY VDATE,a.vchnum,A.MORDER";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt1.Rows.Count > 0)
                {
                    dt1.TableName = "barcode";
                    dt1 = fgen.addBarCode(dt1, "fstr", true);
                    dsRep.Tables.Add(dt1);
                    frm_rptName = "mrr_stk";
                    if (frm_cocd == "SVPL") frm_rptName = "fg_rcv_stk_svpl";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "ret_stk", frm_rptName, dsRep, "Sticker", "Y");
                }
                else
                {
                    data_found = "N";
                }
                break;

            //CHL
            case "F1007":
                #region CHL
                opt = fgen.getOption(frm_qstr, frm_cocd, "W0015", "OPT_ENABLE");
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");

                if (mq0 == "YES")
                {
                    SQuery = "SELECT A.RTN_DATE,c.EMAIL AS CEMAIL,c.WEBSITE AS CWEBSITE,C.STATEN AS CSTATEN,A.THRU,D.NAME AS CHALLAN_TYPE,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO,B.INAME,B.UNIT AS UNIT1,b.hscode,B.CPARTNO AS APART,C.ANAME AS PARTY,C.ADDR1 AS PADRES1,C.ADDR2 AS PADRES2,C.ADDR3 ASPADR3,C.ADDR4 AS DIVISION ,C.TELNUM,C.GST_NO AS CGSTNO ,C.RC_NUM AS PARTY_TIN,C.GIRNO AS CGIRNO,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY_SENT,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A. IAMOUNT AS IAMT,A.IRATE AS ARATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS TIME_,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL,a.btchno,a.btchdt,a.freight,C.VENCODE,a.Location,nvl(a.GSTVCH_NO,A.VCHNUM) as GSTVCH_NO,NVL(a.ccent,'-') AS ccent,a.t_Deptt FROM IVOUCHER A,ITEM B,FAMST C ,TYPE D WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and TRIM(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ") AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) AND D.ID='M' ORDER BY A.ICODE";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt.TableName = "Prepcur";
                    repCount = 1;
                    if (frm_vty == "25")
                        repCount = 4;
                    dsRep = new DataSet();
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));

                    mq0 = "SELECT B.INAME,B.UNIT AS UNIT2,B.CPARTNO, A.BRANCHCD AS MBR,A.TYPE AS BTYPE,A.VCHNUM AS BVCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS BVCHDATE1,A.ICODE AS BICODE,A.ACODE AS BACODE,A.IQTYOUT AS BQTY,A.IQTY_WT AS WT_REC FROM RGPMST A,ITEM B  WHERE A.BRANCHCD='" + frm_mbr + "' AND TRIM(A.ICODE)=TRIM(B.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and TRIM(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ") ORDER BY b.cpartno";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    dt1.TableName = "FAMST";
                    dsRep.Tables.Add(dt1);

                    if (dsRep.Tables[0].Rows.Count > 0)
                    {

                        if (frm_vty == "25")
                        {
                            Print_Report_BYDS(frm_cocd, frm_mbr, "std_Challan_basic", "std_Challan_cjw25", dsRep, "challan", "Y");
                        }
                        else
                        {
                            Print_Report_BYDS(frm_cocd, frm_mbr, "std_Challan_basic", "std_Challan_basic", dsRep, "challan", "Y");
                        }

                    }
                    else
                    {
                        data_found = "N";
                    }
                }
                else
                {
                    SQuery = "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,D.NAME AS CHALLAN_TYPE,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO,B.INAME,B.UNIT AS UNIT1,B.CPARTNO AS APART,C.ANAME AS PARTY,C.ADDR1 AS PADRES1,C.ADDR2 AS PADRES2,C.ADDR3 ASPADR3,C.ADDR4 AS DIVISION ,C.TELNUM ,C.RC_NUM AS PARTY_TIN,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY_SENT,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A. IAMOUNT AS IAMT,A.IRATE AS ARATE,A.EXC_57F4,A.EXC_57F4DT,nvl(A.IQTY_WT,0) AS QTY_WT_SENT,A.MTIME AS TIME_,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL FROM IVOUCHER A,ITEM B,FAMST C ,TYPE D WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) AND D.ID='M' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY A.ICODE";

                    SQuery = "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,D.NAME,A.PRNUM,A.NARATION,A.APPROXVAL,A.ST_ENTFORM AS ST_38,'" + opt + "' AS btoprint,A.LOCATION AS DOCKET_NO,A.EXC_TIME,A.IAMOUNT,A.BINNO as mo_vehi,B.INAME,B.UNIT,B.CPARTNO AS APART,b.hscode,C.ANAME,C.ADDR1 as caddr1,C.ADDR2 as caddr2,C.ADDR3 as caddr3,C.ADDR4 as caddr4,c.staten,t.type1,c.gst_no as cgst_no,c.girno,C.TELNUM ,C.RC_NUM AS PARTY_TIN,C.EXC_NUM AS PARTY_ECC,C.MOBILE AS MB,C.PINCODE AS PARTY_PINCODE ,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.PRNUM AS DAYS_,A.ICODE,A.ACODE,A.IQTYOUT AS QTY,A.DESC_ AS RMK,A.PONUM AS PO_NO,A.PODATE AS PO_DATE,A.approxval AS IAMT,a.post,A.IRATE,A.EXC_57F4,A.EXC_57F4DT,A.IQTY_WT AS QTY_WT_SENT,A.MTIME AS remvtime,a.thru as ins_no,a.rtn_date as remv_date ,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,C.EMAIL,a.exc_rate as cgst,a.exc_amt as cgst_val,a.cess_pu as sgst_val,a.cess_percent as sgst,a.cess_pu+a.exc_amt as taxval,a.freight,C.VENCODE,a.Location,nvl(a.GSTVCH_NO,A.VCHNUM) as GSTVCH_NO,NVL(a.ccent,'-') AS ccent FROM IVOUCHER A,ITEM B,TYPE D ,FAMST C left join type t on trim(c.staten)=trim(t.name) and t.id='{' WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(D.TYPE1) and a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and TRIM(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + barCode + ") AND D.ID='M' ORDER BY A.ICODE";

                    dsRep = new DataSet();
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "Prepcur";
                        //BarCode adding
                        dt = fgen.addBarCode(dt, "fstr", true);
                        repCount = 3;
                        dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));

                        frm_rptName = "std_chl_gst";
                        if (doc_GST == "GCC") frm_rptName = "std_chl_intl";
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_chl", frm_rptName, dsRep, "Challan Report", "Y");
                    }
                    else
                    {
                        data_found = "N";
                    }
                }
                #endregion
                break;
            case "F1007A":
                #region CHL2
                frm_mbr = scode.Substring(0, 2);
                frm_vty = scode.Substring(2, 2);
                sname = scode.Substring(4, 6);
                sname = "'" + sname + "'" + " and " + "'" + scode.Substring(20, 6) + "'";

                SQuery = "SELECT B.INAME,B.UNIT AS UNIT2, A.BRANCHCD AS MBR,A.TYPE AS BTYPE,A.VCHNUM AS BVCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS BVCHDATE1,A.ICODE AS BICODE,A.ACODE AS BACODE,A.IQTYOUT AS BQTY,A.IQTY_WT AS WT_REC FROM RGPMST A,ITEM B  WHERE a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and a.VCHNUM BETWEEN " + sname + " and a.VCHDATE " + xprdRange + " AND TRIM(A.ICODE)=TRIM(B.ICODE)";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_chl2", frm_rptName, dsRep, "Challan Report");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F25111":
            case "F2511B":
            case "FB1055":
            case "FB1054":
                #region Issue
                SQuery = "select E.NAME as header,'Material Issue Slip' as h1,'Issue Agst Job Card' as h2, trim(C.NAME) AS DPT_NAME,trim(I.INAME) as iname,trim(I.CPARTNO) as cpartno,trim(I.UNIT) AS IUNIT,I.BINNO AS ITEMBIN,A.*,'-' AS MCHNAME FROM IVOUCHER A, ITEM I ,TYPE C,TYPE E WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.TYPE1) AND trim(C.ID)='M' AND E.ID='M' AND TRIM(A.TYPE)=TRIM(E.TYPE1) AND trim(A.BRANCHCD)='" + frm_mbr + "' and trim(A.TYPE) ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum,A.MORDER";//old
                SQuery = "select E.NAME as header,'Material Issue Slip' as h1,'Issue Agst Job Card' as h2, trim(C.NAME) AS DPT_NAME,trim(I.INAME) as iname,trim(I.CPARTNO) as cpartno,trim(I.UNIT) AS IUNIT,I.BINNO AS ITEMBIN,A.*,'-' AS MCHNAME,d.ciname,PORDNO,TO_CHAR(PORDDT,'dd/MM/yyyy')  as Porddt,f.aname FROM IVOUCHER A, ITEM I ,TYPE C,TYPE E ,somas d,famst f WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.TYPE1) AND trim(C.ID)='M' AND E.ID='M' AND TRIM(A.TYPE)=TRIM(E.TYPE1) AND trim(A.BRANCHCD)='" + frm_mbr + "' and trim(A.TYPE) ='" + frm_vty + "' and trim(d.ordno)||TO_CHAR(D.ORDDT,'DD/MM/YYYY')||TRIM(D.ICODE)=TRIM(A.revis_no) and trim(d.acode)=trim(f.acode) and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum,A.MORDER";
                //SQuery = "select 'Material Issue Slip' as header,'Material Issue Slip' as h1,'Issue Agst Job Card' as h2,nvl(trim(r.vchnum),'-') as reelvch, trim(C.NAME) AS DPT_NAME,trim(I.INAME) as iname,trim(I.CPARTNO) as cpartno,trim(I.UNIT) AS IUNIT,I.BINNO AS ITEMBIN,A.* FROM IVOUCHER A left outer join reelvch R on trim(a.branchcd)||trim(a.type)||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY')||trim(a.icode)=trim(r.branchcd)||trim(r.type)||TRIM(R.vchnum)||TO_CHAR(R.vchdate,'DD/MM/YYYY')||trim(r.icode) , ITEM I ,TYPE C WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.TYPE1) AND trim(C.ID)='M' AND trim(A.BRANCHCD)='" + frm_mbr + "' and trim(A.TYPE) ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum,A.MORDER";
                
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                mq0 = "";
                mq0 = "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr_sub,a.vchnum as vchnum_sub,a.vchdate as vchdate_sub,A.icode as icode_sub,a.srno as srno_sub,a.coreelno as coreelno_sub,a.kclreelno as kclreelno_sub,a.reelwin as reelwin_sub,a.reelwout as reelwout_sub,a.irate as irate_sub,a.reelspec1 as reelspec1_sub,a.reelspec2 as reelspec2_sub,a.psize as psize_sub,a.gsm as gsm_sub,a.uinsp as uinsp_sub,a.reelmtr as reelmtr_sub,b.iname FROM REELVCH A,item b WHERE trim(a.icodE)=trim(B.icode) and trim(A.BRANCHCD)='" + frm_mbr + "' and trim(A.TYPE) ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  order by a.srno";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt1.Rows.Count <= 0)
                {
                    dt1 = new DataTable();
                    SQuery = "Select '-' as fstr_sub,'-' as vchnum_sub,null as vchdate_sub,'-' as icode_sub,0 as srno_sub,'-' as coreelno_sub, '-' as kclreelno_sub,'-' as reelwin_sub,0 as reelwout_sub,0 as irate_sub,'-' as reelspec1_sub,'-' as reelspec2_sub,'-' as psize_sub,'-' as gsm_sub,'-' as uinsp_sub,'-' as reelmtr_sub,'-' as iname from dual";
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                }
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    dt1.TableName = "Sub";
                    dsRep.Tables.Add(dt1);
                    if (frm_cocd == "MASS" || frm_cocd == "MAST")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_iss_mass", "std_iss_mass", dsRep, "Store Issue Note");
                    }
                    else
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_iss", "std_iss", dsRep, "Store Issue Note");

                    }
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F2521B":
                #region Issue request
                SQuery = "select E.NAME as header,'Material Issue Request' as h1,'Issue Agst Job Card' as h2, trim(C.NAME) AS DPT_NAME,trim(I.INAME) as iname,trim(I.CPARTNO) as cpartno,trim(I.UNIT) AS IUNIT,I.BINNO AS ITEMBIN,A.*,'-' AS MCHNAME FROM IVOUCHERC A, ITEM I ,TYPE C,TYPE E WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.TYPE1) AND trim(C.ID)='M' AND E.ID='M' AND TRIM(A.TYPE)=TRIM(E.TYPE1) AND trim(A.BRANCHCD)='" + frm_mbr + "' and trim(A.TYPE) ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum,A.MORDER";//old
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                mq0 = "";
                mq0 = "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr_sub,a.vchnum as vchnum_sub,a.vchdate as vchdate_sub,A.icode as icode_sub,a.srno as srno_sub,a.coreelno as coreelno_sub,a.kclreelno as kclreelno_sub,a.reelwin as reelwin_sub,a.reelwout as reelwout_sub,a.irate as irate_sub,a.reelspec1 as reelspec1_sub,a.reelspec2 as reelspec2_sub,a.psize as psize_sub,a.gsm as gsm_sub,a.uinsp as uinsp_sub,a.reelmtr as reelmtr_sub,b.iname FROM REELVCH A,item b WHERE trim(a.icodE)=trim(B.icode) and trim(A.BRANCHCD)='" + frm_mbr + "' and trim(A.TYPE) ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  order by a.srno";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt1.Rows.Count <= 0)
                {
                    dt1 = new DataTable();
                    SQuery = "Select '-' as fstr_sub,'-' as vchnum_sub,null as vchdate_sub,'-' as icode_sub,0 as srno_sub,'-' as coreelno_sub, '-' as kclreelno_sub,'-' as reelwin_sub,0 as reelwout_sub,0 as irate_sub,'-' as reelspec1_sub,'-' as reelspec2_sub,'-' as psize_sub,'-' as gsm_sub,'-' as uinsp_sub,'-' as reelmtr_sub,'-' as iname from dual";
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                }
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    dt1.TableName = "Sub";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_iss_reqbom", "std_iss_reqbom", dsRep, "Store Issue Request");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F25113":
            case "F25116":
                #region store return
                header_n = "Material Return Note";
                if (iconID == "F25113") header_n = "Material Conversion Note";
                SQuery = "select '" + header_n + "' as header,'" + header_n + "' as h1,'" + header_n + "' as h2, nvl(trim(r.vchnum),'-') as reelvch,trim(C.NAME) AS DPT_NAME,trim(I.INAME) as iname,trim(I.CPARTNO) as cpartno,trim(I.UNIT) AS IUNIT,I.BINNO AS ITEMBIN,A.*  FROM IVOUCHER A left outer join reelvch R on trim(a.branchcd)||trim(a.type)||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY')||trim(a.icode)=trim(r.branchcd)||trim(r.type)||TRIM(R.vchnum)||TO_CHAR(R.vchdate,'DD/MM/YYYY')||trim(r.icode) , ITEM I ,TYPE C WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.TYPE1) AND trim(C.ID)='M' AND trim(A.BRANCHCD)='" + frm_mbr + "' and trim(A.TYPE) ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum,A.MORDER";

                SQuery = "select '" + header_n + "' as header,'" + header_n + "' as h1,'" + header_n + "' as h2, a.vchnum as reelvch,trim(C.NAME) AS DPT_NAME,trim(I.INAME) as iname,trim(I.CPARTNO) as cpartno,trim(I.UNIT) AS IUNIT,I.BINNO AS ITEMBIN,A.*  FROM IVOUCHER A , ITEM I ,TYPE C WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.TYPE1) AND trim(C.ID)='M' AND trim(A.BRANCHCD)='" + frm_mbr + "' and trim(A.TYPE) ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum,A.MORDER";

                if (frm_cocd == "ROYL" && iconID == "F25116")
                {
                    SQuery = "select '" + header_n + "' as header,'" + header_n + "' as h1,'" + header_n + "' as h2, a.vchnum as reelvch,trim(C.NAME) AS DPT_NAME,D.NAME AS TYPE_NAME,trim(I.INAME) as iname,trim(I.CPARTNO) as cpartno,trim(I.UNIT) AS IUNIT,I.BINNO AS ITEMBIN,A.*  FROM IVOUCHER A , ITEM I ,TYPE C,TYPE D WHERE TRIM(I.ICODE)=TRIM(A.ICODE) AND TRIM(A.ACODE)=TRIM(C.TYPE1) AND TRIM(A.TYPE)=TRIM(D.TYPE1) AND D.id='M'  AND trim(C.ID)='M' AND trim(A.BRANCHCD)='" + frm_mbr + "' and trim(A.TYPE) ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum,A.MORDER";
                }
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                mq0 = "";
                mq0 = "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr_sub,a.vchnum as vchnum_sub,a.vchdate as vchdate_sub,A.icode as icode_sub,a.srno as srno_sub,a.coreelno as coreelno_sub,a.kclreelno as kclreelno_sub,a.reelwin as reelwin_sub,a.reelwin as reelwout_sub,a.irate as irate_sub,a.reelspec1 as reelspec1_sub,a.reelspec2 as reelspec2_sub,a.psize as psize_sub,a.gsm as gsm_sub,a.uinsp as uinsp_sub,a.reelmtr as reelmtr_sub,b.iname FROM REELVCH A,item b WHERE trim(a.icodE)=trim(B.icode) and trim(A.BRANCHCD)='" + frm_mbr + "' and trim(A.TYPE) ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  order by a.srno";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt1.Rows.Count <= 0)
                {
                    dt1 = new DataTable();
                    SQuery = "Select '-' as fstr_sub,'-' as vchnum_sub,null as vchdate_sub,'-' as icode_sub,0 as srno_sub,'-' as coreelno_sub, '-' as kclreelno_sub,'-' as reelwin_sub,0 as reelwout_sub,0 as irate_sub,'-' as reelspec1_sub,'-' as reelspec2_sub,'-' as psize_sub,'-' as gsm_sub,'-' as uinsp_sub,'-' as reelmtr_sub,'-' as iname from dual";
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                }
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    dt1.TableName = "Sub";
                    dsRep.Tables.Add(dt1);
                    if (frm_cocd == "ROYL" && iconID == "F25116")
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_ret_ROYL", "std_ret_ROYL", dsRep, "Store Return Note");
                    else
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_ret", "std_ret", dsRep, "Store Return Note");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F25132":
            case "F15166":
                #region this is new after changes OLD
                //DataTable ph_tbl = new DataTable();
                //ph_tbl.Columns.Add("fromdt", typeof(string));
                //ph_tbl.Columns.Add("todt", typeof(string));
                //ph_tbl.Columns.Add("header", typeof(string));
                //ph_tbl.Columns.Add("mcode", typeof(string));
                //ph_tbl.Columns.Add("scode", typeof(string));
                //ph_tbl.Columns.Add("branchcd", typeof(string));
                //ph_tbl.Columns.Add("type", typeof(string));
                //ph_tbl.Columns.Add("vchnum", typeof(string));
                //ph_tbl.Columns.Add("vchd", typeof(DateTime));
                //ph_tbl.Columns.Add("erpcode", typeof(string));
                //ph_tbl.Columns.Add("iopqty", typeof(double));
                //ph_tbl.Columns.Add("iqtyin", typeof(double));
                //ph_tbl.Columns.Add("iqtyout", typeof(double));
                //ph_tbl.Columns.Add("iname", typeof(string));
                //ph_tbl.Columns.Add("icode", typeof(string));
                //ph_tbl.Columns.Add("cpartno", typeof(string));
                //ph_tbl.Columns.Add("Location", typeof(string));
                //ph_tbl.Columns.Add("desc_", typeof(string));
                //ph_tbl.Columns.Add("unit", typeof(string));
                //ph_tbl.Columns.Add("cdrgno", typeof(string));
                //ph_tbl.Columns.Add("iord", typeof(double));
                //ph_tbl.Columns.Add("imin", typeof(double));
                //ph_tbl.Columns.Add("imax", typeof(double));
                //ph_tbl.Columns.Add("iweight", typeof(double));
                //ph_tbl.Columns.Add("sname", typeof(string));
                //ph_tbl.Columns.Add("mname", typeof(string));
                //ph_tbl.Columns.Add("deptt", typeof(string));
                //ph_tbl.Columns.Add("vchdate", typeof(string));
                //ph_tbl.Columns.Add("vdd", typeof(string));
                //ph_tbl.Columns.Add("month_", typeof(string));
                //header_n = "Stock Ledger";
                //party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                //xprdRange1 = "between to_Date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')-1";
                //cond = "";
                ////if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) cond = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                ////if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1) cond = " and trim(icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";

                //if (party_cd.Trim().Length <= 1)
                //{
                //    party_cd = "%";
                //}
                //if (part_cd.Trim().Length <= 1)
                //{
                //    part_cd = "%";
                //}
                ////  cond = " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' ";
                //cond = " and icode like '" + part_cd + "%' ";

                //// original SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + header_n + "' as header, a.*,b.iname,b.location,a.erpcode as icode,b.cpartno,b.unit,b.cdrgno,b.iord,b.imin,b.imax,b.iweight,c.iname as sname,d.name as mname,e.name as deptt,to_Char(A.vchd,'dd-Mon') as vchdate,to_Char(a.vchd,'yyyymmdd') as vdd,to_char(a.vchd,'MM') as month_  from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.branchcd,a.type,a.vchnum,a.vchdate as vchd,a.icode as erpcode,sum(a.cdr) as iqtyin,sum(a.ccr) as iqtyout,a.desc_ from (select branchcd,type,vchnum,vchdate,trim(icode) as icode,(iqtyin)-(iqtyout) as op,0 as cdr,0 as ccr ,desc_ FROM IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdRange1 + " " + cond + "  union all select branchcd,type,vchnum,vchdate,trim(icode) as icode,0 as op,(iqtyin) as cdr,(iqtyout) as ccr,desc_ from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE  " + xprdRange + " " + cond + " ) a where (a.cdr+a.ccr)>0 group by substr(a.icode,1,2),substr(a.icode,1,4),a.branchcd,a.type,a.vchnum,a.vchdate,a.icode,a.desc_) a,item b,item c,type d,type e where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and trim(A.type)=trim(e.type1) and d.id='Y' and e.id='M' order by a.erpcode,vdd,a.vchnum";
                //SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + header_n + "' as header, a.*,b.iname,b.location,a.erpcode as icode,b.cpartno,b.unit,b.cdrgno,b.iord,b.imin,b.imax,b.iweight,c.iname as sname,d.name as mname,to_Char(A.vchd,'dd-Mon') as vchdate,to_Char(a.vchd,'yyyymmdd') as vdd,to_char(a.vchd,'MM') as month_  from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.branchcd,a.type,a.vchnum,a.vchdate as vchd,a.icode as erpcode,sum(a.cdr) as iqtyin,sum(a.ccr) as iqtyout,a.desc_ from (select branchcd,type,vchnum,vchdate,trim(icode) as icode,(iqtyin)-(iqtyout) as op,0 as cdr,0 as ccr ,desc_ FROM IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdRange1 + " " + cond + "  union all select branchcd,type,vchnum,vchdate,trim(icode) as icode,0 as op,(iqtyin) as cdr,(iqtyout) as ccr,desc_ from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE  " + xprdRange + " " + cond + " ) a where (a.cdr+a.ccr)>0 group by substr(a.icode,1,2),substr(a.icode,1,4),a.branchcd,a.type,a.vchnum,a.vchdate,a.icode,a.desc_) a,item b,item c,type d,type e where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and d.id='Y' order by a.erpcode,vdd,a.vchnum";
                //dt = new DataTable();
                //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //SQuery = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as IOPQTY,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " " + cond + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + xprdRange + " " + cond + " and store='Y' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE";
                //dt1 = new DataTable();
                //dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //foreach (DataRow dr2 in dt.Rows)
                //{
                //    dr1 = ph_tbl.NewRow();
                //    dr1["fromdt"] = fromdt;
                //    dr1["todt"] = todt;
                //    dr1["header"] = header_n;
                //    dr1["mcode"] = dr2["mcode"].ToString().Trim();
                //    dr1["scode"] = dr2["scode"].ToString().Trim();
                //    dr1["branchcd"] = dr2["branchcd"].ToString().Trim();
                //    dr1["type"] = dr2["type"].ToString().Trim();
                //    dr1["vchnum"] = dr2["vchnum"].ToString().Trim();
                //    dr1["vchd"] = Convert.ToDateTime(dr2["vchd"].ToString().Trim()).ToString("dd/MM/yyyy");
                //    dr1["erpcode"] = dr2["erpcode"].ToString().Trim();
                //    dr1["iopqty"] = fgen.seek_iname_dt(dt1, "icode='" + dr2["erpcode"].ToString().Trim() + "'", "IOPQTY"); //opening
                //    dr1["iqtyin"] = fgen.make_double(dr2["iqtyin"].ToString().Trim());
                //    dr1["iqtyout"] = fgen.make_double(dr2["iqtyout"].ToString().Trim());
                //    dr1["iname"] = dr2["iname"].ToString().Trim();
                //    dr1["icode"] = dr2["icode"].ToString().Trim();
                //    dr1["Location"] = dr2["location"].ToString().Trim();
                //    dr1["desc_"] = dr2["desc_"].ToString().Trim();
                //    dr1["cpartno"] = dr2["cpartno"].ToString().Trim();
                //    dr1["unit"] = dr2["unit"].ToString().Trim();
                //    dr1["cdrgno"] = dr2["cdrgno"].ToString().Trim();
                //    dr1["imin"] = fgen.make_double(dr2["imin"].ToString().Trim());
                //    dr1["imax"] = fgen.make_double(dr2["imax"].ToString().Trim());
                //    dr1["iweight"] = fgen.make_double(dr2["iweight"].ToString().Trim());
                //    dr1["sname"] = dr2["sname"].ToString().Trim();
                //    dr1["mname"] = dr2["mname"].ToString().Trim();
                //    // dr1["deptt"] = dr2["deptt"].ToString().Trim();
                //    dr1["vchdate"] = dr2["vchdate"].ToString().Trim();
                //    dr1["vdd"] = dr2["vdd"].ToString().Trim();
                //    dr1["month_"] = dr2["month_"].ToString().Trim();
                //    ph_tbl.Rows.Add(dr1);
                //}
                //dsRep = new DataSet();

                //ph_tbl.TableName = "Prepcur";
                //dsRep.Tables.Add(ph_tbl);
                //Print_Report_BYDS(frm_cocd, frm_mbr, "stklgr", "stklgr", dsRep, header_n);
                #endregion
                #region this is new after changes
                DataTable ph_tbl = new DataTable();
                ph_tbl.Columns.Add("fromdt", typeof(string));
                ph_tbl.Columns.Add("todt", typeof(string));
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("mcode", typeof(string));
                ph_tbl.Columns.Add("scode", typeof(string));
                ph_tbl.Columns.Add("branchcd", typeof(string));
                ph_tbl.Columns.Add("type", typeof(string));
                ph_tbl.Columns.Add("vchnum", typeof(string));
                ph_tbl.Columns.Add("vchd", typeof(DateTime));
                ph_tbl.Columns.Add("erpcode", typeof(string));
                ph_tbl.Columns.Add("iopqty", typeof(double));
                ph_tbl.Columns.Add("iqtyin", typeof(double));
                ph_tbl.Columns.Add("iqtyout", typeof(double));
                ph_tbl.Columns.Add("iname", typeof(string));
                ph_tbl.Columns.Add("aname", typeof(string));
                ph_tbl.Columns.Add("icode", typeof(string));
                ph_tbl.Columns.Add("cpartno", typeof(string));
                ph_tbl.Columns.Add("Location", typeof(string));
                ph_tbl.Columns.Add("desc_", typeof(string));
                ph_tbl.Columns.Add("unit", typeof(string));
                ph_tbl.Columns.Add("closing", typeof(double));
                ph_tbl.Columns.Add("cl_tot", typeof(double));
                ph_tbl.Columns.Add("cdrgno", typeof(string));
                ph_tbl.Columns.Add("iord", typeof(double));
                ph_tbl.Columns.Add("imin", typeof(double));
                ph_tbl.Columns.Add("imax", typeof(double));
                ph_tbl.Columns.Add("iweight", typeof(double));
                ph_tbl.Columns.Add("sname", typeof(string));
                ph_tbl.Columns.Add("mname", typeof(string));
                ph_tbl.Columns.Add("deptt", typeof(string));
                ph_tbl.Columns.Add("vchdate", typeof(string));
                ph_tbl.Columns.Add("vdd", typeof(string));
                ph_tbl.Columns.Add("month_", typeof(string));
                header_n = "Stock Ledger";
                //party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                xprdRange1 = "between to_Date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
                cond = "";
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) cond = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1) cond = " and trim(icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";

                //if (party_cd.Trim().Length <= 1)
                //{
                //    party_cd = "%";
                //}
                //if (part_cd.Trim().Length <= 1)
                //{
                //    part_cd = "%";
                //}
                //  cond = " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' ";
                //cond = " and icode like '" + party_cd + "%' ";
                // original SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + header_n + "' as header, a.*,b.iname,b.location,a.erpcode as icode,b.cpartno,b.unit,b.cdrgno,b.iord,b.imin,b.imax,b.iweight,c.iname as sname,d.name as mname,e.name as deptt,to_Char(A.vchd,'dd-Mon') as vchdate,to_Char(a.vchd,'yyyymmdd') as vdd,to_char(a.vchd,'MM') as month_  from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.branchcd,a.type,a.vchnum,a.vchdate as vchd,a.icode as erpcode,sum(a.cdr) as iqtyin,sum(a.ccr) as iqtyout,a.desc_ from (select branchcd,type,vchnum,vchdate,trim(icode) as icode,(iqtyin)-(iqtyout) as op,0 as cdr,0 as ccr ,desc_ FROM IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdRange1 + " " + cond + "  union all select branchcd,type,vchnum,vchdate,trim(icode) as icode,0 as op,(iqtyin) as cdr,(iqtyout) as ccr,desc_ from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE  " + xprdRange + " " + cond + " ) a where (a.cdr+a.ccr)>0 group by substr(a.icode,1,2),substr(a.icode,1,4),a.branchcd,a.type,a.vchnum,a.vchdate,a.icode,a.desc_) a,item b,item c,type d,type e where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and trim(A.type)=trim(e.type1) and d.id='Y' and e.id='M' order by a.erpcode,vdd,a.vchnum";
                //SQuery = "select DISTINCT '" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + header_n + "' as header, a.*,b.iname,b.location,a.erpcode as icode,b.cpartno,b.unit,b.cdrgno,b.iord,b.imin,b.imax,b.iweight,c.iname as sname,d.name as mname,to_Char(A.vchd,'dd-Mon') as vchdate,to_Char(a.vchd,'yyyymmdd') as vdd,to_char(a.vchd,'MM') as month_  from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.branchcd,a.type,a.vchnum,a.vchdate as vchd,a.icode as erpcode,sum(a.cdr) as iqtyin,sum(a.ccr) as iqtyout,a.desc_ from (select branchcd,type,vchnum,vchdate,trim(icode) as icode,(iqtyin)-(iqtyout) as op,0 as cdr,0 as ccr ,desc_ FROM IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdRange1 + " " + cond + "  union all select branchcd,type,vchnum,vchdate,trim(icode) as icode,0 as op,(iqtyin) as cdr,(iqtyout) as ccr,desc_ from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE  " + xprdRange + " " + cond + " ) a where (a.cdr+a.ccr)>0 group by substr(a.icode,1,2),substr(a.icode,1,4),a.branchcd,a.type,a.vchnum,a.vchdate,a.icode,a.desc_) a,item b,item c,type d where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and d.id='Y' order by a.erpcode,vdd,a.vchnum";
                //SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt, '" + header_n + "' as header,F.ANAME,(case  when TRIM(nvl(f.aname,'-'))!='-'  then f.aname  else a.o_deptt end) as dept, a.*,b.iname,b.location,a.erpcode as icode,b.cpartno,b.unit,b.cdrgno,b.iord,b.imin,b.imax,b.iweight,c.iname as sname,d.name as mname,to_Char(A.vchd,'dd-Mon') as vchdate,to_Char(a.vchd,'yyyymmdd') as vdd,to_char(a.vchd,'MM') as month_  from  (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,A.ACODE,A.O_DEPTT,a.branchcd,a.type,a.vchnum,a.vchdate as vchd,a.icode as erpcode,sum(a.cdr) as iqtyin,sum(a.ccr) as iqtyout,a.desc_ from  (select branchcd,type,vchnum,vchdate,trim(icode) as icode,TRIM(ACODE) AS ACODE,(iqtyin)-(iqtyout) as op,0 as cdr,0 as ccr ,desc_,O_DEPTT FROM IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdRange1 + " " + cond + " union all select branchcd,type,vchnum,vchdate,trim(icode) as icode,TRIM(ACODE) AS ACODE,0 as op,(iqtyin) as cdr,(iqtyout) as ccr,desc_,O_DEPTT from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdRange + " " + cond + ")  a where (a.cdr+a.ccr)>0 group by substr(a.icode,1,2),substr(a.icode,1,4),a.branchcd,a.type,a.vchnum,a.vchdate,a.icode,a.desc_,A.ACODE,A.O_DEPTT )  a LEFT OUTER JOIN FAMST F ON TRIM(A.ACODE)=TRIM(F.ACODE),item b,item c,type d where  trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and d.id='Y' order by a.erpcode,vdd,a.vchnum desc";
                SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt, '" + header_n + "' as header,F.ANAME,(case  when TRIM(nvl(f.aname,'-'))!='-'  then f.aname  else a.o_deptt end) as dept, a.*,trim(b.iname) as iname,b.location,trim(a.erpcode) as icode,trim(b.cpartno) as cpartno,b.unit,b.cdrgno,e.iord,nvl(e.imin,0) as imin,nvl(e.imax,0) as imax,nvl(b.iweight,0) as iweight,trim(c.iname) as sname,trim(d.name) as mname,to_Char(A.vchd,'dd-Mon') as vchdate,to_Char(a.vchd,'yyyymmdd') as vdd,to_char(a.vchd,'MM') as month_  from  (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,trim(A.ACODE) as acode,trim(A.O_DEPTT) as o_deptt,a.branchcd,a.type,a.vchnum,a.vchdate as vchd,trim(a.icode) as erpcode,sum(op) as op,sum(a.cdr) as iqtyin,sum(a.ccr) as iqtyout,a.desc_,a.invno,a.invdate from  (select '" + frm_mbr + "' as branchcd,'-' as type,'OpBal.' as vchnum,to_DaTE('" + fromdt + "','dd/mm/yyyy') as vchdate,trim(icode) as icode,'-' AS ACODE,sum(YR_" + frm_myear + ") as op,0 as cdr,0 as ccr ,'-' as desc_,'-' as O_DEPTT,'-' as invno,'-' as invdate  FROM ITEMBAL where branchcd='" + frm_mbr + "' " + cond + " group by trim(icode) union all select '" + frm_mbr + "' as branchcd,'-' as type,'OpBal.' as vchnum,to_DaTE('" + fromdt + "','dd/mm/yyyy') as vchdate,trim(icode) as icode,'-' AS ACODE,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr ,'-' as desc_,'-' as O_DEPTT,'-' as invno,'-' as invdate  FROM IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdRange1 + " " + cond + " group by trim(icode) union all select branchcd,type,vchnum,vchdate,trim(icode) as icode,TRIM(ACODE) AS ACODE,0 as op,nvl(iqtyin,0) as cdr,nvl(iqtyout,0) as ccr,desc_,O_DEPTT,invno,to_char(invdate,'dd/mm/yyyy') as invdate from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdRange + " " + cond + ")  a where (abs(a.op)+abs(a.cdr)+abs(a.ccr))>0 group by substr(a.icode,1,2),substr(a.icode,1,4),a.branchcd,a.type,a.vchnum,a.vchdate,a.icode,a.desc_,A.ACODE,A.O_DEPTT,a.invno,a.invdate)  a LEFT OUTER JOIN FAMST F ON TRIM(A.ACODE)=TRIM(F.ACODE),item b,item c,type d,itembal e where  trim(a.erpcode)=trim(b.icode) and trim(a.erpcode)=trim(e.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and d.id='Y' AND E.BRANCHCD='" + frm_mbr + "' order by a.erpcode,vdd,a.type,a.vchnum ";

                SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt, '" + header_n + "' as header,F.ANAME,(case  when TRIM(nvl(f.aname,'-'))!='-'  then f.aname  else a.o_deptt end) as dept, a.*,trim(b.iname) as iname,b.location,trim(a.erpcode) as icode,trim(b.cpartno) as cpartno,b.unit,b.cdrgno,e.iord,nvl(e.imin,0) as imin,nvl(e.imax,0) as imax,nvl(b.iweight,0) as iweight,trim(c.iname) as sname,trim(d.name) as mname,to_Char(A.vchd,'dd-Mon') as vchdate,to_Char(a.vchd,'yyyymmdd') as vdd,to_char(a.vchd,'MM') as month_  from  (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,trim(A.ACODE) as acode,trim(A.O_DEPTT) as o_deptt,a.branchcd,a.type,a.vchnum,a.vchdate as vchd,trim(a.icode) as erpcode,sum(op) as op,sum(a.cdr) as iqtyin,sum(a.ccr) as iqtyout,a.desc_,a.invno,a.invdate from  (select '" + frm_mbr + "' as branchcd,'-' as type,'OpBal.' as vchnum,to_DaTE('" + fromdt + "','dd/mm/yyyy') as vchdate,trim(icode) as icode,'-' AS ACODE,sum(YR_" + frm_myear + ") as op,0 as cdr,0 as ccr ,'-' as desc_,'-' as O_DEPTT,'-' as invno,'-' as invdate  FROM ITEMBAL where branchcd='" + frm_mbr + "' " + cond + " group by trim(icode) union all select '" + frm_mbr + "' as branchcd,'-' as type,'OpBal.' as vchnum,to_DaTE('" + fromdt + "','dd/mm/yyyy') as vchdate,trim(icode) as icode,'-' AS ACODE,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr ,'-' as desc_,'-' as O_DEPTT,'-' as invno,'-' as invdate  FROM IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdRange1 + " " + cond + " group by trim(icode) union all select branchcd,type,vchnum,vchdate,trim(icode) as icode,TRIM(ACODE) AS ACODE,0 as op,nvl(iqtyin,0) as cdr,nvl(iqtyout,0) as ccr,desc_,O_DEPTT,invno,to_char(invdate,'dd/mm/yyyy') as invdate from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '%' and store='Y' AND VCHDATE " + xprdRange + " " + cond + ")  a where (abs(a.op)+abs(a.cdr)+abs(a.ccr))>0 group by substr(a.icode,1,2),substr(a.icode,1,4),a.branchcd,a.type,a.vchnum,a.vchdate,a.icode,a.desc_,A.ACODE,A.O_DEPTT,a.invno,a.invdate)  a LEFT OUTER JOIN FAMST F ON TRIM(A.ACODE)=TRIM(F.ACODE) left outer join itembal e on trim(a.erpcode)=trim(e.icode) AND E.BRANCHCD='" + frm_mbr + "' ,item b,item c,type d where  trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and d.id='Y'  order by a.erpcode,vdd,a.type,a.vchnum ";



                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //SQuery = "-";
                SQuery = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as IOPQTY,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' " + cond + " union all select branchcd,icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " " + cond + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + xprdRange + " " + cond + " and store='Y' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                dt3 = new DataTable();
                dt3 = fgen.getdata(frm_qstr, frm_cocd, "select name,trim(type1) as type1 from type where id='M' order by type1");

                dt4 = new DataTable();
                dt4 = fgen.getdata(frm_qstr, frm_cocd, "select trim(icode) as icode,imin,imax,iord from itembal where branchcd='" + frm_mbr + "' " + cond + "");

                if (dt.Rows.Count > 0)
                {
                    DataView view1 = new DataView(dt);
                    DataTable dtdrsim = new DataTable();
                    dtdrsim = view1.ToTable(true, "ERPCODE"); //MAIN       
                    foreach (DataRow dr2 in dtdrsim.Rows)
                    {
                        //  DataView viewim = new DataView(dt, "erpcode='" + dr2["erpcode"] + "' and VCHNUM='" + dr2["VCHNUM"] + "'", "", DataViewRowState.CurrentRows);
                        DataView viewim = new DataView(dt, "erpcode='" + dr2["erpcode"] + "'", "", DataViewRowState.CurrentRows);
                        dr1 = ph_tbl.NewRow();
                        dt1 = new DataTable();
                        dt1 = viewim.ToTable();
                        db1 = 0; mq1 = ""; db = 0; db2 = 0; db6 = 0; db7 = 0; int m = 0;
                        for (i = 0; i < dt1.Rows.Count; i++)
                        {
                            m = dt1.Rows.Count - 1;
                            #region
                            dr1 = ph_tbl.NewRow();
                            dr1["fromdt"] = fromdt;
                            dr1["todt"] = todt;
                            dr1["header"] = header_n;
                            dr1["mcode"] = dt1.Rows[i]["mcode"].ToString().Trim();  //dr2["mcode"].ToString().Trim();
                            dr1["scode"] = dt1.Rows[i]["scode"].ToString().Trim();   // dr2["scode"].ToString().Trim();
                            dr1["branchcd"] = dt1.Rows[i]["branchcd"].ToString().Trim();    //dr2["branchcd"].ToString().Trim();
                            dr1["type"] = dt1.Rows[i]["type"].ToString().Trim();// dr2["type"].ToString().Trim();
                            dr1["vchnum"] = dt1.Rows[i]["vchnum"].ToString().Trim();   //dr2["vchnum"].ToString().Trim();
                            dr1["vchd"] = Convert.ToDateTime(dt1.Rows[i]["vchd"].ToString().Trim()).ToString("dd/MM/yyyy");
                            dr1["erpcode"] = dt1.Rows[i]["erpcode"].ToString().Trim();//dr2["erpcode"].ToString().Trim();
                            dr1["iopqty"] = fgen.seek_iname_dt(dt2, "icode='" + dt1.Rows[i]["erpcode"].ToString().Trim() + "'", "IOPQTY"); //opening
                            dr1["iqtyin"] = fgen.make_double(dt1.Rows[i]["iqtyin"].ToString().Trim());
                            dr1["iqtyout"] = fgen.make_double(dt1.Rows[i]["iqtyout"].ToString().Trim());

                            if (i == 0)
                            {
                                if (dt1.Rows.Count > 0)
                                {
                                    db1 = fgen.make_double(dr1["IOPQTY"].ToString().Trim()); //opening
                                }
                                dr1["iopqty"] = db1;
                                db2 = db1;
                                db2 = db2 + fgen.make_double(dr1["iqtyin"].ToString()) - fgen.make_double(dr1["iqtyout"].ToString());
                            }
                            else
                            {
                                db2 = db2 + fgen.make_double(dr1["iqtyin"].ToString()) - fgen.make_double(dr1["iqtyout"].ToString());
                            }
                            if (i != 0)
                            {
                                dr1["iopqty"] = db1;
                            }
                            if (i == m)
                            {
                                dr1["cl_tot"] = db2;
                            }
                            dr1["closing"] = db2;
                            dr1["iname"] = dt1.Rows[i]["iname"].ToString().Trim();//dr2["iname"].ToString().Trim();
                            dr1["aname"] = dt1.Rows[i]["aname"].ToString().Trim();
                            dr1["icode"] = dt1.Rows[i]["icode"].ToString().Trim(); //dr2["icode"].ToString().Trim();
                            dr1["Location"] = dt1.Rows[i]["location"].ToString().Trim(); //dr2["location"].ToString().Trim();
                            if (dt1.Rows[i]["desc_"].ToString().Trim().Length > 1)
                            {
                                dr1["desc_"] = dt1.Rows[i]["desc_"].ToString().Trim();  //dr2["desc_"].ToString().Trim();
                            }
                            else
                            {
                                if (dt1.Rows[i]["invno"].ToString().Trim().Length > 1)
                                {
                                    dr1["desc_"] = "Doc No. " + dt1.Rows[i]["invno"].ToString().Trim() + " Dt." + dt1.Rows[i]["invdate"].ToString().Trim() + "";
                                }
                                else
                                {
                                    dr1["desc_"] = "";
                                }
                            }
                            dr1["cpartno"] = dt1.Rows[i]["cpartno"].ToString().Trim();  //dr2["cpartno"].ToString().Trim();
                            dr1["unit"] = dt1.Rows[i]["unit"].ToString().Trim();  //dr2["unit"].ToString().Trim();
                            dr1["cdrgno"] = dt1.Rows[i]["cdrgno"].ToString().Trim();   //dr2["cdrgno"].ToString().Trim();
                            // dr1["imin"] = fgen.make_double(dt1.Rows[i]["imin"].ToString().Trim());//old
                            //dr1["imax"] = fgen.make_double(dt1.Rows[i]["imax"].ToString().Trim());//old

                            dr1["imin"] = fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dr1["erpcode"].ToString().Trim() + "'", "imin"));//new
                            dr1["imax"] = fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dr1["erpcode"].ToString().Trim() + "'", "imax"));//new
                            dr1["iord"] = fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dr1["erpcode"].ToString().Trim() + "'", "iord"));//new

                            dr1["iweight"] = fgen.make_double(dt1.Rows[i]["iweight"].ToString().Trim());
                            dr1["sname"] = dt1.Rows[i]["sname"].ToString().Trim();
                            dr1["mname"] = dt1.Rows[i]["mname"].ToString().Trim();
                            if (dt1.Rows[i]["DEPT"].ToString().Trim().Length > 1)
                            {
                                dr1["deptt"] = dt1.Rows[i]["DEPT"].ToString().Trim();
                            }
                            else
                            {
                                dr1["deptt"] = fgen.seek_iname_dt(dt3, "type1='" + dt1.Rows[i]["type"].ToString().Trim() + "'", "name");
                            }
                            dr1["vchdate"] = dt1.Rows[i]["vchdate"].ToString().Trim();
                            dr1["vdd"] = dt1.Rows[i]["vdd"].ToString().Trim();
                            dr1["month_"] = dt1.Rows[i]["month_"].ToString().Trim();
                            #endregion
                            ph_tbl.Rows.Add(dr1);
                        }

                    }
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(ph_tbl);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "stklgr", "stklgr", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F25133":
                #region Stock Summary Report  OLD  COMMENTED ON 30 JULY 2018 BY MADHVI....THIS REPORT GETS USER INPUTS FROM THIS FN_OPEN_PARTYITEMDATERANGEBOX AND THIS BOX GIVES DATA BASED ON U_COLR3 NOT ON U_PARTCODE
                //xprdRange1 = "between to_Date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
                //cond = "";
                //party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                ////if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) cond = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                ////if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1) cond = " and trim(icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";

                //if (party_cd.Trim().Length <= 1)
                //{
                //    party_cd = "%";
                //}
                //if (part_cd.Trim().Length <= 1)
                //{
                //    part_cd = "%";
                //}

                //cond = " and icode like '" + party_cd + "%' and icode like '" + part_cd + "%' ";

                //SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Group Wise Stock (All Items)' as header, a.*,b.iname,b.cpartno,b.unit,b.cdrgno,c.iname as sname,d.name as mname from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + " as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + " and length(trim(icode))>4 " + cond + " union all select branchcd,trim(icode) as icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdRange1 + " " + cond + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%'  AND VCHDATE " + xprdRange + " " + cond + " and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE,substr(a.icode,1,2),substr(a.icode,1,4) having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0) a,item b,item c,type d where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and d.id='Y' order by a.erpcode,b.iname";

                //dt = new DataTable();
                //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //dt.TableName = "Prepcur";
                //dsRep.Tables.Add(dt);
                //Print_Report_BYDS(frm_cocd, frm_mbr, "stksumm", "stksumm", dsRep, "Stock Summary Report");
                #endregion
                #region Stock Summary Report New
                xprdRange1 = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
                xprdRange1 = "between to_Date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
                cond = "";
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) cond = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1) cond = " and trim(icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Group Wise Stock (All Items)' as header, a.*,trim(b.iname) as iname,b.cpartno,b.maker,b.unit,b.cdrgno,trim(c.iname) as sname,d.name as mname from " +
                    "(select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.icode as erpcode,sum(a.opening) as opening" +
                    ",sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from " +
                    "(Select branchcd,trim(icode) as icode,yr_" + frm_myear + "  as opening,0 as cdr,0 as ccr from itembal where " + branch_Cd + "" +
                    " and length(trim(icode))>4 " + cond + " union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0))" +
                    " as op,0 as cdr,0 as ccr FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdRange1 + " " + cond + " " +
                    "and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr" +
                    ",sum(nvl(iqtyout,0)) as ccr from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%'  AND VCHDATE " + xprdRange + " " +
                    "" + cond + " and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE,substr(a.icode,1,2),substr(a.icode,1,4) " +
                    "having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0) a,item b,item c,type d where trim(a.erpcode)=trim(b.icode)" +
                    " and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and d.id='Y' order by a.erpcode,b.iname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "stksummbd", "stksummbd", dsRep, "Stock Summary Report");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F25134":
                xprdRange1 = "between to_Date('" + fromdt + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
                cond = "";
                // OLD COMMENTED ON 30 JULY 2018 BY MADHVI (THIS REPORT GETS USER INPUTS FROM THIS FN_OPEN_PARTYITEMDATERANGEBOX AND THIS BOX GIVES DATA BASED ON U_COLR3 NOT ON U_PARTCODE)
                // -------------------------- (START)
                //party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                //part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //if (party_cd.Trim().Length <= 1)
                //{
                //    party_cd = "%";
                //}
                //if (part_cd.Trim().Length <= 1)
                //{
                //    part_cd = "%";
                //}
                //cond = " and icode like '" + party_cd + "%' and icode like '" + part_cd + "%' ";
                // ------------------------- (END)
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) cond = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1) cond = " and trim(icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'Group Wise Stock (All Items)' as header, a.*,trim(b.iname) as iname,b.cpartno,b.unit,b.cdrgno,trim(c.iname) as sname,trim(d.name) as mname from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,trim(a.icode) as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk,sum(a.Imin) as Min_level,sum(a.Imax) as Max_level,sum(a.Iord) as RO_level from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,nvl(imin,0) as Imin,nvl(imax,0) as Imax,nvl(Iord,0) as Iord from itembal where " + branch_Cd + " and length(trim(icode))>4 " + cond + " union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr,0 as Imin,0 as Imax,0 as Iord FROM IVOUCHER where " + branch_Cd + " and TYPE LIKE '%' AND VCHDATE " + xprdRange1 + " " + cond + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr,0 as Imin,0 as Imax,0 as Iord from IVOUCHER where " + branch_Cd + " and TYPE LIKE '%'  AND VCHDATE " + xprdRange + " " + cond + " and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE,substr(a.icode,1,2),substr(a.icode,1,4) having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0) a,item b,item c,type d where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and d.id='Y' order by a.erpcode,b.iname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "stksumm_lvl", "stksumm_lvl", dsRep, "Stock Summary Report With Min/Max Level");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25141":
            case "RPT1_1"://finbase form
                // MRR REG
                //7may..observation is done
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "SELECT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(B.ADDR1)||TRIM(B.ADDR2) AS ADRES, A.O_DEPTT,A.VCHNUM AS MRRNO,A.TYPE,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDATE,trim(A.REFNUM) AS CHALLANO,A.REFDATE AS CHALDATE,A.GENUM AS GENO,A.GEDATE AS GEDT,trim(A.ICODE) as icode ,trim(A.ACODE) as acode,trim(B.ANAME) as aname,nvl(A.IQTYIN,0) AS ACPT_QTY, A.INVNO,A.INVDATE,A.PONUM,A.PODATE,nvl(A.IRATE,0) as irate,nvl(A.IAMOUNT,0) as iamount,A.NARATION,'RGP:'||A.RGPNUM AS RGPNUM,TO_CHAR(A.RGPDATE,'DD/MM/YYYY') AS RGPDATE,A.DESC_,trim(C.INAME) as iname,C.UNIT AS CUNIT,C.CPARTNO,A.FINVNO,A.MODE_TPT AS VECH FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE)  AND  A.BRANCHCD='" + frm_mbr + "' and A.TYPE IN (" + frm_vty + ") AND A.VCHDATE " + xprdRange + " and a.store not in ('R','W') and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' ORDER BY A.MORDER";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_MRR_REG", "std_MRR_REG", dsRep, "Matl. Inward Report");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "RPT4"://form in fin-base .. om_web_rpt_royl
                header_n = "MRR Passing Report";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                if (mq1 == "Y")
                {
                    SQuery = "SELECT '" + header_n + "' as header,'" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(B.ADDR1)||TRIM(B.ADDR2) AS ADRES, A.O_DEPTT,A.VCHNUM AS MRRNO,A.TYPE,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDATE ,trim(A.ACODE) as acode,trim(B.ANAME) as aname, A.INVNO,A.INVDATE,A.DESC_,A.FINVNO,(case when nvl(trim(a.inspected),'-')='Y' then '(Insp.'||trim(a.pname)||')'||trim(A.finvno) else '-' end) as inspby FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE IN (" + frm_vty + ") AND A.VCHDATE " + xprdRange + " and a.store not in ('R','W') and a.srno='1' ORDER BY A.MORDer,a.vchnum";
                }
                else
                {
                    SQuery = "SELECT '" + header_n + "' as header,'" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(B.ADDR1)||TRIM(B.ADDR2) AS ADRES, A.O_DEPTT,A.VCHNUM AS MRRNO,A.TYPE,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDATE ,trim(A.ACODE) as acode,trim(B.ANAME) as aname, A.INVNO,A.INVDATE,A.DESC_,A.FINVNO,(case when nvl(trim(a.inspected),'-')='Y' then '(Insp.'||trim(a.pname)||')'||trim(A.finvno) else '-' end) as inspby FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE IN (" + frm_vty + ") AND A.VCHDATE " + xprdRange + " and a.store not in ('R','W') and a.srno='1' ORDER BY A.MORDer,a.vchnum";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_MRR_Paasing", "std_MRR_Paasing", dsRep, "-");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25142":
                // CHALLAN REG
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1").Length > 1) cond = " and trim(a.acode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1") + "'";
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR2").Length > 1) cond = " and trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR2") + "'";
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR2").Length > 1) cond = " and trim(a.acode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1") + "' and trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR2") + "' ";

                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE"); // ADDED BY MADHVI ON 9TH APR 2018
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "SELECT  '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,'Dpt.W.O.' as header,trim(C.ANAME) as aname,C.ADDR1 AS ADRES1,C.ADDR2 AS ADRES2,C.ADDR3 AS ADRES3,trim(B.INAME) as iname,A.BRANCHCD,A.DESC_,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,trim(A.ICODE) as icode,nvl(A.IRATE,0) as irate,nvl(A.IAMOUNT,0) as iamount,TRIM(A.TYPE)||'/'||TRIM(A.VCHNUM) AS RGP_NO,trim(A.ACODE) as acode,nvl(A.IQTYOUT,0) as iqtyout,A.PONUM,A.PODATE,B.UNIT AS BUNIT,nvl(A.APPROXVAL,0) as APPROXVAL,A.MORDER AS SRNO,B.CPARTNO AS PARTNO,A.O_DEPTT FROM IVOUCHER A,ITEM B,FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND  A.TYPE IN (" + frm_vty + ") AND A.VCHDATE " + xprdRange + "  " + cond + " and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' ORDER BY a.type,a.vchdate,a.vchnum,A.MORDER";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Challan_REG", "std_Challan_REG", dsRep, "Matl. Outward Report");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25143":
                // ISSUE REG
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                string icode = fgenMV.Fn_Get_Mvar(frm_qstr, "U_III");
                string jobno = fgenMV.Fn_Get_Mvar(frm_qstr, "U_IV");
                string jobno1 = "";
                if (icode.Length < 3) icode = "";
                else icode = " AND TRIM(a.ICODE)='" + icode + "'";
                if (jobno.Length < 3) jobno = "";
                else
                {
                    jobno1 = " AND TRIM(jobno)||to_Char(jobdt,'dd/mm/yyyy')='" + jobno + "'";
                    jobno = " AND TRIM(a.invno)||to_Char(a.invdate,'dd/mm/yyyy')='" + jobno + "'";
                }
                SQuery = "SELECT DISTINCT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE ,TO_CHAR(A.VCHDATE,'yyyymmdd')||TRIM(A.VCHNUM) AS GROUPING,trim(B.NAME) as dept_name, trim(C.INAME) as iname,C.UNIT AS CUNIT,C.CPARTNO AS PARTNO,(case when nvl(a.IRATE,0)>0 then nvl(a.IRATE,0) WHEN NVL(C.IQD,0)>0 THEN NVL(C.IQD,0) else nvl(C.IRATE,0) end) AS RATE,ROUND(A.IQTYOUT * (case when nvl(a.IRATE,0)>0 then nvl(a.IRATE,0) WHEN NVL(C.IQD,0)>0 THEN NVL(C.IQD,0) else nvl(C.IRATE,0) end), 2) AS IAMOUNTVAL,trim(T.NAME) AS ISSUETYPE,A.* FROM IVOUCHER A,TYPE B,ITEM C,TYPE T WHERE TRIM(A.ACODE)=TRIM(B.TYPE1) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(A.TYPE)=TRIM(T.TYPE1) AND T.ID='M' AND B.ID='M' AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE IN (" + frm_vty + ") AND A.VCHDATE " + xprdRange + " and a.store='Y' and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' " + icode + " " + jobno + " ORDER BY A.MORDER";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Issue_REG", "std_Issue_REG", dsRep, "Matl. Issue Report");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25144":
                // RETURN REG
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE"); // ADDED BY MADHVI ON 9TH APR 2018
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "select trim(b.iname) as iname,b.unit as bunit,b.cpartno,'Store Return Register' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,to_char(a.vchdate,'yyyymmdd') as vdd,trim(t.name) as deptt,to_char(a.vchdate,'yyyymmdd')||trim(a.vchnum) as grp ,a.* from ivoucher a,item b,type t where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(t.type1) and t.id='M' and a.branchcd='" + frm_mbr + "' and a.type IN (" + frm_vty + ") and a.vchdate " + xprdRange + " and a.store in ('Y','R') and trim(a.acode) like '" + party_cd + "%' and trim(a.icode) like '" + part_cd + "%' order by vdd,a.vchnum,a.morder";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Return_REG", "std_Return_REG", dsRep, "Matl. Return Report");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25235":
                // SHORT / EXCESS SUPPLIES
                //SQuery = "SELECT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(A.VCHNUM) AS VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ICODE) AS ICODE,A.SRNO,TRIM(A.ACODE) AS ACODE,(CASE WHEN A.PONUM='-' THEN '000000' ELSE A.PONUM END) AS PONUM,TO_CHAR(A.PODATE,'DD/MM/YYYY') AS PODATE,A.INVNO,TO_CHAR(A.INVDATE,'DD/MM/YYYY') AS INVDATE,a.type as grp,A.REFNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS REFDATE,nvl(A.IQTY_CHL,0) as iqty_chl,A.NARATION,trim(I.INAME) as iname,I.UNIT,I.CPARTNO AS PARTNO,trim(F.ANAME) as aname,TRIM(F.ADDR1)||TRIM(F.ADDR2) AS ADDRESS,A.MODE_TPT,A.DESC_,A.SPEXC_AMT FROM IVOUCHERP A,ITEM I,FAMST F WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND TRIM(A.ACODE)=TRIM(F.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='00' AND A.VCHDATE " + xprdRange + " AND nvl(A.SPEXC_AMT,0)!=nvl(A.IQTY_CHL,0) ORDER BY A.SRNO";
                string new_sh_Ex = "N", autoshiftrej = "Y", chk_fld = "";
                mq0 = "Select enable_yn from controls where trim(id)='O99' and enable_yn='Y'";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt1.Rows.Count > 0) new_sh_Ex = "Y";
                chk_fld = "a.iqty_chl";
                if (autoshiftrej == "Y")
                {
                    if (new_sh_Ex == "Y") col1 = "round(a.iqty_chl,3)!=round(nvl(a.iqty_ok,0),3)and nvl(a.iqty_ok,0)>0 ";
                    else col1 = "round(a.iqty_chl,3)!=round((a.iqtyin+rej_rw),3) ";
                }
                else
                {
                    col1 = "round(a.iqty_chl,3)!=round(a.iqtyin,3)";
                }
                if (new_sh_Ex == "Y") chk_fld = "nvl(a.iqty_ok,0)";

                SQuery = "SELECT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,a.genum||to_char(a.gedate,'dd/mm/yyyy') as fstr,a.vchnum||to_char(a.vchdate,'dd/mm/yyyy') as fstr1,A.RGPNUM,A.RGPDATE,a.branchcd,a.purpose,a.type,a.icode,a.genum as f1,a.gedate as f2,a.o_deptt,trim(a.invno) as f3,a.REFNUM,a.invdate as f4,b.aname as f5,c.iname as f6,c.cpartno,c.unit as f7,a.mode_tpt,a.t_deptt,a.rej_rw,a.acpt_ud,a.iqty_chl," + chk_fld + " as f8,a.vchnum as f9,a.vchdate as f10 , a.refdate as REFDATE,a.irate,a.finvno,nvl(a.idiamtr,0) as poqty,a.irate*(" + chk_fld + "-(a.iqtyin)) as amt,a.iqtyin as f11," + chk_fld + " -(a.iqtyin) as f12,a.podate,a.ponum ,b.addr1 as f14,b.addr2 as f15,a.desc_ as f16,a.NARATION as f17 from ivoucher a ,item c ,famst b where TRIM(a.acode)=TRIM(b.acode) and trim(a.icode)=trim(c.icode)and a.branchcd='" + frm_mbr + "' and substr(a.type,1,1)='0' AND a.VCHDATE " + xprdRange + " and " + col1 + " order by vchdate,vchnum,srno,gedate,genum";//and trim(a.Acode) like '%'
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Short_Excess_Supp", "std_Short_Excess_Supp", dsRep, "Short / Excess Supplies");
                }
                else
                {
                    data_found = "N";
                }
                break;
            case "F25238":
                // GORUP ITEM WISE PO RATES
                header_n = "Group,Item Wise 12 Month Purchase Qty";
                footer_n = "footer";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,'" + footer_n + "' as footer, A.ACODE,trim(C.ANAME) as aname,a.icode as item_code,trim(b.INAME) as iname,substr(trim(a.icode),1,2) as mg,trim(D.NAME) AS GRPNAME,b.cpartno,b.hscode,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar  from ( select trim(ACODE) as acode,trim(icode) as icode,(Case when to_char(ORDDT,'mm')='04' then nvl(QTYORD,0) else 0 end) as Apr,(Case when to_char(ORDDT,'mm')='05' then nvl(QTYORD,0) else 0 end) as may,(Case when to_char(ORDDT,'mm')='06' then nvl(QTYORD,0)  else 0 end) as jun,(Case when to_char(ORDDT,'mm')='07' then nvl(QTYORD,0) else 0 end) as jul,(Case when to_char(ORDDT,'mm')='08' then nvl(QTYORD,0) else 0 end) as aug,(Case when to_char(ORDDT,'mm')='09' then nvl(QTYORD,0) else 0 end) as sep,(Case when to_char(ORDDT,'mm')='10' then nvl(QTYORD,0) else 0 end) as oct,(Case when to_char(ORDDT,'mm')='11' then nvl(QTYORD,0) else 0 end) as nov,(Case when to_char(ORDDT,'mm')='12' then nvl(QTYORD,0) else 0 end) as dec,(Case when to_char(ORDDT,'mm')='01' then nvl(QTYORD,0) else 0 end) as jan,(Case when to_char(ORDDT,'mm')='02' then nvl(QTYORD,0)  else 0 end) as feb,(Case when to_char(ORDDT,'mm')='03' then nvl(QTYORD,0) else 0 end) as mar  from POMAS where branchcd='" + frm_mbr + "' and type like '5%' and ORDDT " + xprdRange + ") a,ITEM b,FAMST C,type d where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND substr(trim(a.icode),1,2)=trim(D.type1) and D.id='Y' group by a.icode,trim(b.iname),b.cpartno,b.hscode,A.ACODE,trim(C.ANAME),substr(trim(a.icode),1,2),trim(D.NAME),D.TYPE1 ORDER BY A.ACODE,MG";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Grp_Itm_PO_qty_n", "std_Grp_Itm_PO_qty_n", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25237":
                // Supplier,Item Wise 12 Month Purch. Qty
                header_n = "Supplier,Item Wise 12 Month Purch. Qty";
                footer_n = "footer";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,'" + footer_n + "' as footer,A.ACODE,trim(C.name) as dept,a.icode,TRIM(b.INAME) AS INAME,trim(b.cpartno) as cpartno,b.unit,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from (select trim(ACODE) as acode,trim(icode) as icode,(Case when to_char(vchdate,'mm')='04' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then nvl(iqtyout,0)-nvl(iqtyin,0)  else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then nvl(iqtyout,0)-nvl(iqtyin,0)  else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then nvl(iqtyout,0)-nvl(iqtyin,0)  else 0 end) as mar  from ivoucher where branchcd='" + frm_mbr + "' and substr(type,0,1) in ('1','3') and type not in ('15','16','17','18','19','36') and vchdate " + xprdRange + " and store='Y') a,ITEM b,type C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.Type1) and c.id='M'  group by a.icode,trim(b.iname),trim(b.cpartno),b.unit,a.acode,trim(c.NAME) ORDER BY A.iCODE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_dept_consum_qty", "std_dept_consum_qty", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25239":
                // Deptt, ItEM WISE consumption report
                header_n = "Deptt,Item Wise 12 Month Consumption Qty";
                footer_n = "footer";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,'" + footer_n + "' as footer,A.ACODE,trim(C.name) as dept,a.icode,TRIM(b.INAME) AS INAME,b.cpartno,b.unit,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from (select trim(ACODE) as acode,trim(icode) as icode,(Case when to_char(vchdate,'mm')='04' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then nvl(iqtyout,0)-nvl(iqtyin,0)  else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then nvl(iqtyout,0)-nvl(iqtyin,0)  else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then nvl(iqtyout,0)-nvl(iqtyin,0)  else 0 end) as mar  from ivoucher where branchcd='" + frm_mbr + "' and substr(type,1,1) in ('1','3') and type not in ('15','16','17','18','19','36') and vchdate " + xprdRange + " and store='Y') a,ITEM b,type C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.Type1) and c.id='M'  group by a.icode,trim(b.iname),b.cpartno,b.unit,a.acode,trim(c.NAME) ORDER BY A.iCODE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_dept_consum_qty", "std_dept_consum_qty", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25240":
                // group ItEM WISE consumption report
                header_n = "Group,Item Wise 12 Month Consumption Qty";
                footer_n = "footer";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,'" + footer_n + "' as footer,substr(trim(a.icode),1,2) as mg,trim(d.name) as grpname,a.icode,trim(b.INAME) as iname,b.cpartno,b.unit ,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from (select trim(icode) as icode,(Case when to_char(vchdate,'mm')='04' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then nvl(iqtyout,0)-nvl(iqtyin,0)  else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then nvl(iqtyout,0)-nvl(iqtyin,0)  else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then nvl(iqtyout,0)-nvl(iqtyin,0)  else 0 end) as mar  from ivoucher where branchcd='" + frm_mbr + "' and substr(type,1,1) in ('1','3') and type not in ('15','16','17','18','19','36') and vchdate " + xprdRange + " and store='Y') a,ITEM b,type d where trim(a.icode)=trim(b.icode) AND substr(trim(a.icode),1,2)=trim(D.type1) and D.id='Y' group by a.icode,trim(b.iname),b.cpartno,b.unit,substr(trim(a.icode),1,2),trim(D.NAME) ORDER BY A.iCODE,MG";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_grp_cons_qty", "std_grp_cons_qty", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25223":
                //Department wise Issue Comparison
                header_n = "Department wise Issue Comparison";
                footer_n = "Note:The Report is Based on the Entries done.Net Issue Qty Department Wise,Item Wise=Total Qty Issued- Total Qty Rec During the Period";
                //SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.acode,a.icode,b.iname,b.cpartno,b.unit,c.name as dept_name,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from (select ACODE,icode,(Case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then iqtyout  else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as mar  from ivoucher where branchcd='" + frm_mbr + "' AND TYPE LIKE '3%' and vchdate " + xprdRange + " and store='Y') a,item b,type c where trim(a.icode)=trim(b.icode)  and trim(a.acode)=trim(c.type1) and c.id='M' group by a.acode,a.icode,b.iname,b.cpartno,b.unit,c.name order by a.icode";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,'" + footer_n + "' as footer,a.acode,a.icode,trim(b.iname) as iname,b.cpartno,b.unit,trim(c.name) as dept_name,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from (select trim(ACODE) as acode,trim(icode) as icode,(Case when to_char(vchdate,'mm')='04' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then nvl(iqtyout,0)-nvl(iqtyin,0)  else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then nvl(iqtyout,0)-nvl(iqtyin,0) else 0 end) as mar from ivoucher where branchcd='" + frm_mbr + "' AND substr(trim(TYPE),1,1) in ('1','3') and type not in ('15','16','17','18','19','36') and vchdate " + xprdRange + " and store='Y') a,item b,type c where trim(a.icode)=trim(b.icode)  and trim(a.acode)=trim(c.type1) and c.id='M' group by a.acode,a.icode,trim(b.iname),b.cpartno,b.unit,trim(c.name) order by a.icode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_dept_isue_comp", "std_dept_isue_comp", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25242":
                // INWARD SUPPLIES WITH REJECTION
                header_n = "Inward Supplies With Rejection";
                footer_n = "";
                SQuery = "SELECT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,'" + header_n + "' as header,'" + footer_n + "' as footer, TRIM(A.BRANCHCD) AS BRANCHCD,TRIM(B.ADDR1)||TRIM(B.ADDR2) AS ADRES, A.O_DEPTT,A.VCHNUM AS MRRNO,A.TYPE,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDATE,trim(A.REFNUM) AS CHALLANO,A.REFDATE AS CHALDATE,A.GENUM AS GENO,A.GEDATE AS GEDT,trim(A.ICODE) as icode,trim(A.ACODE) as acode,trim(B.ANAME) as aname,nvl(A.IQTYIN,0)+NVL(A.REJ_RW,0) AS ACPT_QTY, A.INVNO,A.INVDATE,A.PONUM,A.PODATE,nvl(A.IRATE,0) as irate,a.iamount ,A.NARATION,'RGP:'||A.RGPNUM AS RGPNUM,TO_CHAR(A.RGPDATE,'DD/MM/YYYY') AS RGPDATE,A.DESC_,trim(C.INAME) as iname,C.UNIT AS CUNIT,trim(C.CPARTNO) as cpartno,A.FINVNO,nvl(A.REJ_RW,0) as rej_rw FROM IVOUCHER A,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE)  AND  A.BRANCHCD='" + frm_mbr + "' and A.TYPE in (" + frm_vty + ") AND A.VCHDATE " + xprdRange + " and a.store not in ('R','W') and nvl(rej_rw,0)>0 ORDER BY A.MORDER";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Inw_Supp_With_Rej", "std_Inw_Supp_With_Rej", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25222": ///////////by yogita...5.5.18
                #region
                header_n = "Department Wise Issue Summary";
                dsRep = new DataSet();
                //  SQuery = "Select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, to_char(a.vchdate,'dd/mm/yyyy') as Doc_Dt,a.vchnum as Doc_no,a.acode as Deptt_Code,b.Name as Deptt_name,a.icode as Item_Code,c.iname as Item_name,c.unit as UOM,c.cpartno as Part,a.iqtyout as Qty_rcv,a.invno as Inv_no,to_char(a.invdate,'dd/mm/yyyy') as Inv_Dt,a.Desc_ as Remarks,a.Ent_by as Entry_by,to_char(a.ent_dt,'dd/mm/yyyy') as Entry_Dt,a.Edt_by as Edit_by,(case when nvl(a.edt_by,'-')!='-' then to_char(a.edt_dt,'dd/mm/yyyy') else '-' end) as Edit_Dt,to_char(a.vchdate,'yyyymmdd') as VDD from ivoucher a , type b, item c where a.branchcd='" + frm_mbr + "'  and a.type LIKE '3%' and a.vchdate  " + xprdRange + " and trim(A.acode)=trim(B.type1) and trim(A.icode)=trim(C.icode) and b.id='M'  order by vdd,a.vchnum,a.morder"; //this query for Dept wise issue detail
                SQuery = "Select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, trim(c.cpartno) as cpartno, trim(a.acode) as Deptt_Code,trim(b.Name) as Deptt_name,trim(a.icode) as Item_Code,trim(c.iname) as Item_name,c.unit as UOM,sum(nvl(a.iqtyout,0)) as Qty_rcv from ivoucher a , type b, item c where a.branchcd='" + frm_mbr + "'  and a.type='30' and a.vchdate  " + xprdRange + " and trim(A.acode)=trim(B.type1) and trim(A.icode)=trim(C.icode) and b.id='M' group by trim(c.cpartno),trim(a.acode),trim(b.Name),trim(a.icode),trim(c.iname),c.unit order by Item_Code"; //this for dept wise issue sumry 
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_dept_isue_smry", "std_dept_isue_smry", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F25230":  //BY YOGITA ...5.5.18
                #region
                header_n = "Rejection Stock Summary";
                //  SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, b.icode as subgrpcode,c.type1 as mgcode, b.iname as sub_grp,c.name as mgname, a.branchcd,trim(a.icode) as icode,d.iname as itemname,d.unit as dunit,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as qtyin,nvl(sum(a.ccr),0) as qtyout,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl from (Select A.branchcd,A.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " and store='R' GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + xprdRange + " and store='R' GROUP BY ICODE,branchcd) a,item b,type c ,item d where LENGTH(tRIM(a.ICODE))>=8 and substr(TRIM(A.icode),1,4)=trim(b.icode) and length(TRIM(b.icode))=4 and c.id='Y' and trim(a.icode)=trim(d.icode) group by a.branchcd,trim(a.icode),b.iname,c.name ,c.type1,b.icode,d.iname,d.unit  having (sum(a.opening)+sum(a.cdr)-sum(a.ccr))>0  ORDER BY B.ICODE,C.TYPE1,icode"; //distinct
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, trim(b.icode) as subgrpcode,trim(c.type1) as mgcode, trim(b.iname) as sub_grp,trim(c.name) as mgname, a.branchcd,trim(a.icode) as icode,trim(d.iname) as itemname,d.unit as dunit,nvl(sum(a.opening),0) as opening,nvl(sum(a.cdr),0) as qtyin,nvl(sum(a.ccr),0) as qtyout,sum(a.opening)+sum(a.cdr)-sum(a.ccr) as cl from (Select A.branchcd,A.icode, 0 as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "X' union all select branchcd,icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " and store='R' GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + xprdRange + " and store='R' GROUP BY ICODE,branchcd) a,item b,type c ,item d where LENGTH(tRIM(a.ICODE))>=8 and substr(TRIM(A.icode),1,4)=trim(b.icode) and length(TRIM(b.icode))=4 and c.id='Y' AND SUBSTR(TRIM(A.ICODE),1,2)=TRIM(C.TYPE1) and trim(a.icode)=trim(d.icode) group by a.branchcd,trim(a.icode),trim(b.iname),trim(c.name) ,trim(c.type1),trim(b.icode),trim(d.iname),d.unit /*having (sum(a.opening)+sum(a.cdr)-sum(a.ccr))>0*/ ORDER BY subgrpcode,mgcode,icode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Rejection_Store_Stock", "Rejection_Store_Stock", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F25149": // MADE BY YOGITA
                header_n = "FG Valuation";
                #region
                ph_tbl = new DataTable();//add dummy column in \ xthis
                ph_tbl.Columns.Add("fromdt", typeof(string));
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("todt", typeof(string));
                ph_tbl.Columns.Add("icode", typeof(string));
                ph_tbl.Columns.Add("iname", typeof(string));
                ph_tbl.Columns.Add("cpart", typeof(string));
                ph_tbl.Columns.Add("cdrgno", typeof(string));
                ph_tbl.Columns.Add("unit", typeof(string));

                ph_tbl.Columns.Add("qty", typeof(double)); //isme closing bal aayega item ka
                ph_tbl.Columns.Add("so_rate", typeof(double));
                ph_tbl.Columns.Add("so_val", typeof(double));
                ph_tbl.Columns.Add("inv_rate", typeof(double));
                ph_tbl.Columns.Add("inv_val", typeof(double));
                ph_tbl.Columns.Add("item_rate", typeof(double));
                ph_tbl.Columns.Add("item_val", typeof(double));
                ph_tbl.Columns.Add("HSCODE", typeof(string));

                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL11"); // MAIN GRP
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); // ORDER BY
                SQuery = "select * from (select branchcd,trim(icode) as icode,nvl(sum(opening),0) as IOPQTY,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + xprdRange + " and store='Y' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE) where SUBSTR(TRIM(icode),1,2) in (" + mq2 + ")";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //STOCK DT

                SQuery = "select to_char(vchdate,'yyyymmdd') as vdd ,trim(icode) as icode,nvl(irate,0) as irate from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' /*and vchdate " + xprdRange + "*/ and vchdate>(sysdate-800) and substr(trim(icode),1,2) in (" + mq2 + ") order by vdd desc";
                dt4 = new DataTable();
                dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //invoice rate

                SQuery = "select to_char(orddt,'yyyymmdd') as vdd ,trim(icode) as icode,nvl(irate,0) as irate from somas where branchcd='" + frm_mbr + "' and type like '4%' /*and orddt " + xprdRange + "*/ and orddt>(sysdate-800)  and substr(trim(icode),1,2) in (" + mq2 + ") order by vdd desc";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //sale order rate

                SQuery = "select distinct trim(icode) as icode,nvl(irate,0) as irate,trim(iname) as iname,nvl(cpartno,'-') as cpartno,nvl(HSCODe,'-') as hscode,nvl(cdrgno,'-') as cdrgno,unit from item where substr(trim(icode),1,2) in (" + mq2 + ") and  length(trim(icode))>=8 order by icode /*" + mq2 + "*/";
                dt3 = new DataTable();
                dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery); // item rate
                ////////
                if (dt3.Rows.Count > 0)
                {
                    DataView view1im = new DataView(dt3); //VIEW OF MAIN DT
                    dt1 = new DataTable();
                    dt1 = view1im.ToTable(true, "ICODE");
                    foreach (DataRow dr0 in dt1.Rows) //view wali main dt
                    {
                        DataView viewim = new DataView(dt3, "ICODE='" + dr0["ICODE"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        DataTable dticode = new DataTable();
                        dticode = viewim.ToTable();
                        dr1 = ph_tbl.NewRow();
                        for (i = 0; i < dticode.Rows.Count; i++)
                        {
                            dr1["fromdt"] = fromdt;
                            dr1["todt"] = todt;
                            dr1["header"] = header_n;
                            dr1["icode"] = dticode.Rows[i]["icode"].ToString().Trim();
                            dr1["cdrgno"] = dticode.Rows[i]["cdrgno"].ToString().Trim();
                            dr1["iname"] = dticode.Rows[i]["iname"].ToString().Trim();
                            dr1["unit"] = dticode.Rows[i]["unit"].ToString().Trim();
                            dr1["cpart"] = dticode.Rows[i]["cpartno"].ToString().Trim();
                            dr1["HSCODE"] = dticode.Rows[i]["HSCODE"].ToString().Trim();
                            dr1["item_rate"] = fgen.make_double(dticode.Rows[i]["irate"].ToString().Trim());
                            dr1["so_rate"] = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "irate"));
                            dr1["inv_rate"] = fgen.make_double(fgen.seek_iname_dt(dt4, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "irate"));
                            dr1["qty"] = fgen.make_double(fgen.seek_iname_dt(dt, "icode='" + dticode.Rows[i]["icode"].ToString().Trim() + "'", "cl"));
                            dr1["inv_val"] = fgen.make_double(dr1["inv_rate"].ToString().Trim()) * fgen.make_double(dr1["qty"].ToString().Trim());
                            dr1["item_val"] = fgen.make_double(dr1["item_rate"].ToString().Trim()) * fgen.make_double(dr1["qty"].ToString().Trim());
                            dr1["so_val"] = fgen.make_double(dr1["so_rate"].ToString().Trim()) * fgen.make_double(dr1["qty"].ToString().Trim());
                        }
                        ph_tbl.Rows.Add(dr1);
                    }
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(ph_tbl);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "FG_Valuation", "FG_Valuation", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F25139": // data is not matching with main finsys. already informed to yogita mam
                header_n = "Pending (Qty & Value) RGP Wise";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                cond = " and TRIM(acode) like '" + party_cd + "%' and TRIM(icode) like '" + part_cd + "%' ";
                SQuery = "SELECT '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(A.ICODE) AS ICODE,TRIM(A.ACODE) AS ACODE,A.RGP_QTY,A.MRR_qTY,(A.RGP_QTY- A.MRR_qTY) AS BAL,(A.RGP_QTY- A.MRR_qTY)*A.IRATE AS VAL,TRIM(B.ANAME) AS ANAME,TRIM(B.ADDR1) AS ADDR1,TRIM(B.ADDR2) AS ADDR2,TRIM(C.INAME) AS INAME,TRIM(C.UNIT) AS UNIT,TRIM(C.CPARTNO) AS CPARTNO,TO_CHAR(A.VCHDATE,'YYYYMMDD')||TRIM(A.VCHNUM) AS VDD FROM (SELECT VCHNUM,VCHDATE,TRIM(ICODE) AS ICODE,TRIM(ACODE) AS ACODE,SUM(RGP_QTY) AS RGP_QTY, SUM (MRR_qTY) AS MRR_qTY,MAX(IRATE) AS IRATE FROM (SELECT VCHNUM,VCHDATE,TRIM(ICODE) AS ICODE,TRIM(ACODE) AS ACODE,IQTYOUT AS RGP_QTY,0 AS MRR_qTY ,IRATE FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='23' AND VCHDATE " + xprdRange + cond + " UNION ALL SELECT RGPNUM ,RGPDATE,TRIM(ICODE) AS ICODE,TRIM(ACODE) AS ACODE,0 AS RGP_QTY,IQTYIN AS mrr_qty,0 AS IRATE  from ivoucher where branchcd='" + frm_mbr + "' and type='0J'  AND RGPDATE " + xprdRange + cond + ") GROUP BY VCHNUM,VCHDATE,TRIM(ICODE),TRIM(ACODE) ) A ,FAMST B,ITEM C WHERE  TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND (A.RGP_QTY- A.MRR_QTY >0) ORDER BY VDD,A.VCHNUM";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Only_pend_qty_val_rgp_wise", "Only_pend_qty_val_rgp_wise", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25198":
                #region Extusion Sticker
                if (frm_cocd == "SACL")
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_DPRINT", "Y");
                    fgen.dPrint(frm_cocd, frm_mbr, frm_UserID, "F25245A", frm_cDt1, barCode.Replace("'", ""));
                }
                else
                {
                    SQuery = "Select a.branchcd,trim(a.icode)||trim(a.btchno) as fstr,A.MORDER, a.type,a.vchnum,to_char(a.vchdate,'YYYYMMDD') as vdate,to_char(a.vchdate,'DD/MM/YYYY') as vchdate,a.icode,a.acode,c.iname,a.btchno,a.iqtyin,A.IQTY_WT,a.invno,a.invdate,a.col1 from ivoucher a ,item c where trim(a.icode)=trim(c.icode)  AND A.BRANCHCD||A.TYPE||TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")  ORDER BY VDATE,a.vchnum,A.MORDER";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt1.Rows.Count > 0)
                    {
                        dt1.TableName = "barcode";
                        dt1 = fgen.addBarCode(dt1, "fstr", true);
                        dsRep.Tables.Add(dt1);
                        frm_rptName = "prod_stk";
                        Print_Report_BYDS(frm_cocd, frm_mbr, "prod_stk", frm_rptName, dsRep, "Sticker", "Y");
                    }
                    else
                    {
                        data_found = "N";
                    }
                }
                #endregion
                break;

            case "F25232":
                header_n = "Rejection Stock Ledger";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (party_cd == "0") party_cd = "";
                if (part_cd == "0") part_cd = "";
                mq3 = fgen.seek_iname(frm_qstr, frm_cocd, "select params from controls where id='R24'", "params");
                if (mq3.Length <= 1)
                {
                    mq3 = frm_cDt1;
                }
                xprdRange1 = " between to_date('" + mq3 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')";
                SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + header_n + "' as header, a.type,a.vchnum,a.vchdate,a.invno,a.invdate,a.desc_,trim(a.icode) as icode,b.iname,a.acode,f.aname,nvl(sum(a.opening),0) as op,nvl(sum(a.cdr),0) as qtyin,nvl(sum(a.ccr),0) as qtyout,0 as cl,a.vdd from (select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,icode,invno,to_char(invdate,'dd/mm/yyyy') as invdate,desc_,to_char(vchdate,'yyyymmdd') as vdd,acode,sum(iqtyin)-sum(iqtyout) as opening,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " and store='R' and substr(trim(icode),1,2) like '" + party_cd + "%' and substr(trim(icode),1,4) like '" + part_cd + "%' GROUP BY branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy'),icode,invno,to_char(invdate,'dd/mm/yyyy'),desc_,to_char(vchdate,'yyyymmdd'),acode union all select branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,icode,invno,to_char(invdate,'dd/mm/yyyy') as invdate,desc_,to_char(vchdate,'yyyymmdd') as vdd,acode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange + " and store='R' and substr(trim(icode),1,2) like '" + party_cd + "%' and substr(trim(icode),1,4) like '" + part_cd + "%' GROUP BY branchcd,type,vchnum,to_char(vchdate,'dd/mm/yyyy'),icode,invno,to_char(invdate,'dd/mm/yyyy'),desc_,to_char(vchdate,'yyyymmdd'),acode) a,item b,famst f where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(f.acode) and LENGTH(tRIM(a.ICODE))>=8  group by a.type,a.vchnum,a.vchdate,a.invno,a.invdate,a.desc_,trim(a.icode),b.iname,a.vdd,a.acode,f.aname ORDER BY a.vdd,vchnum,icode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dr = null;
                dt3 = new DataTable();
                dt3 = dt.Clone();
                if (dt.Rows.Count > 0)
                {
                    DataView viewIcode = new DataView(dt);
                    dt1 = new DataTable();
                    dt1 = viewIcode.ToTable(true, "icode");

                    foreach (DataRow drn in dt1.Rows)
                    {
                        DataView viewDt = new DataView(dt, "icode='" + drn["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt2 = new DataTable();
                        dt2 = viewDt.ToTable();
                        db1 = 0; db2 = 0; db6 = 0; db7 = 0;

                        for (i = 0; i < dt2.Rows.Count; i++)
                        {
                            dr = dt3.NewRow();
                            if (i == 0)
                            {
                                db2 = fgen.make_double(dt2.Rows[i]["qtyin"].ToString().Trim()) - fgen.make_double(dt2.Rows[i]["qtyout"].ToString().Trim());
                            }
                            else
                            {
                                db2 = (db2 + fgen.make_double(dt2.Rows[i]["qtyin"].ToString().Trim())) - fgen.make_double(dt2.Rows[i]["qtyout"].ToString().Trim());
                            }

                            dr["fromdt"] = dt2.Rows[i]["fromdt"].ToString().Trim();
                            dr["todt"] = dt2.Rows[i]["todt"].ToString().Trim();
                            dr["header"] = dt2.Rows[i]["header"].ToString().Trim();
                            dr["type"] = dt2.Rows[i]["type"].ToString().Trim();
                            dr["vchnum"] = dt2.Rows[i]["vchnum"].ToString().Trim();
                            dr["vchdate"] = dt2.Rows[i]["vchdate"].ToString().Trim();
                            dr["invno"] = dt2.Rows[i]["invno"].ToString().Trim();
                            dr["invdate"] = dt2.Rows[i]["invdate"].ToString().Trim();
                            dr["icode"] = dt2.Rows[i]["icode"].ToString().Trim();
                            dr["iname"] = dt2.Rows[i]["iname"].ToString().Trim();
                            dr["desc_"] = dt2.Rows[i]["desc_"].ToString().Trim();
                            dr["qtyin"] = dt2.Rows[i]["qtyin"].ToString().Trim();
                            dr["qtyout"] = dt2.Rows[i]["qtyout"].ToString().Trim();
                            dr["vdd"] = dt2.Rows[i]["vdd"].ToString().Trim();
                            dr["acode"] = dt2.Rows[i]["acode"].ToString().Trim();
                            dr["aname"] = dt2.Rows[i]["aname"].ToString().Trim();
                            dr["cl"] = db2;
                            dt3.Rows.Add(dr);
                        }
                    }
                }
                if (dt3.Rows.Count > 0)
                {
                    dt3.TableName = "Prepcur";
                    dsRep = new DataSet();
                    dsRep.Tables.Add(dt3);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Rejection_Stock_Ledger", "Rejection_Stock_Ledger", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25241":
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL12"); //for date selected
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL11"); // for selected days for non moving
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1"); // for selected type 
                if (mq1 == "0")
                    mq1 = todt;
                mq4 = Convert.ToDateTime(fgen.seek_iname(frm_qstr, frm_cocd, "SELECT TO_DATE('" + mq1 + "','DD/MM/YYYY')-" + mq2 + " AS TOTL_DYS FROM DUAL", "TOTL_DYS")).ToString("dd/MM/yyyy");
                //mq4 is for date selected - no of days  fr non moving
                header_n = "Items Non Moving for " + mq2 + " Days";
                SQuery = "SELECT '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,'" + header_n + "' as header,a.mcode,a.scode,a.erpcode,a.opening,a.opening*b.iqd as value,trim(b.iname) as iname,B.IQD,b.cpartno,trim(c.iname) as sname,d.name as mname from (select substr(a.icode,1,2) as mcode,substr(a.icode,1,4) as scode,a.icode as erpcode,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + " as opening,0 as cdr,0 as ccr from itembal where BRANCHCD='" + frm_mbr + "' and length(trim(icode))>4  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where BRANCHCD='" + frm_mbr + "' and TYPE LIKE '%' AND VCHDATE " + xprdRange1 + " and store='Y' GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where BRANCHCD='" + frm_mbr + "' and TYPE LIKE '%'  AND VCHDATE between to_date('" + mq1 + "','dd/mm/yyyy') and to_Date('" + mq1 + "','dd/mm/yyyy') and store='Y' GROUP BY trim(icode) ,branchcd) a GROUP BY A.ICODE,substr(a.icode,1,2),substr(a.icode,1,4) having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0) a,item b,item c,type d where trim(a.erpcode)=trim(b.icode) and trim(A.scode)=trim(c.icode) and trim(a.mcode)=trim(d.type1) and d.id='Y' /*and a.mcode='" + mq3 + "'*/ AND A.OPENING>0 order by a.erpcode,b.iname";
                dt = new DataTable();  // for stock values  // iqd for rate 
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                // as suggested by puneet sir-->
                mq0 = "select distinct icode from ivoucher where BRANCHCD='" + frm_mbr + "' and TYPE LIKE '%' /*and substr(icode,1,2)='" + mq3 + "'*/ AND vchdate between to_date('" + mq4 + "','dd/mm/yyyy') and to_date('" + mq1 + "','dd/mm/yyyy')";
                dt1 = new DataTable(); // for comparing non moving items for entred days
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                //----------------------------------
                if (dt.Rows.Count > 0)
                {
                    dt2 = new DataTable();
                    dt2 = dt.Clone();
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        mq0 = fgen.seek_iname_dt(dt1, "icode='" + dt.Rows[i]["erpcode"].ToString().Trim() + "'", "icode");
                        if (mq0.Length <= 1)
                        {
                            dr = dt2.NewRow();
                            dr["header"] = dt.Rows[i]["header"].ToString().Trim();
                            dr["FRMDATE"] = dt.Rows[i]["FRMDATE"].ToString().Trim();
                            dr["TODATE"] = dt.Rows[i]["TODATE"].ToString().Trim();
                            dr["MCODE"] = dt.Rows[i]["MCODE"].ToString().Trim();
                            dr["SCODE"] = dt.Rows[i]["SCODE"].ToString().Trim();
                            dr["ERPCODE"] = dt.Rows[i]["ERPCODE"].ToString().Trim();
                            dr["OPENING"] = fgen.make_double(dt.Rows[i]["OPENING"].ToString().Trim());
                            dr["value"] = fgen.make_double(dt.Rows[i]["value"].ToString().Trim());
                            dr["INAME"] = dt.Rows[i]["INAME"].ToString().Trim();
                            dr["IQD"] = fgen.make_double(dt.Rows[i]["IQD"].ToString().Trim());
                            dr["cpartno"] = dt.Rows[i]["cpartno"].ToString().Trim();
                            dr["SNAME"] = dt.Rows[i]["SNAME"].ToString().Trim();
                            dr["MNAME"] = dt.Rows[i]["MNAME"].ToString().Trim();
                            dt2.Rows.Add(dr);
                        }
                    }

                    dsRep = new DataSet();
                    dt2.TableName = "Prepcur";
                    dsRep.Tables.Add(dt2);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Non_Moving_Items", "Non_Moving_Items", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F25157":
                #region
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (party_cd.Trim().Length <= 1)
                {
                    party_cd = "%";
                }
                if (part_cd.Trim().Length <= 1)
                {
                    part_cd = "%";
                }
                cond = " and TRIM(acode) like '" + party_cd + "%' and TRIM(icode) like '" + part_cd + "%' ";
                mq0 = "";
                header_n = "Job Work Register";
                mq0 = "SELECT '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, A.VCHNUM,A.VCHDATE,A.ICODE,A.ACODE,A.RGP_QTY,A.MRR_QTY,A.MRRNO,A.MRRDT,A.WONO,A.WODT, B.ANAME,B.ADDR1,B.ADDR2,C.INAME,C.CPARTNO,a.desc_  FROM (SELECT VCHNUM,VCHDATE,ICODE,ACODE,SUM(RGP_QTY) AS RGP_QTY,SUM(MRR_QTY) AS MRR_QTY,MRRNO,MRRDT,WODT,WONO,DESC_ FROM (SELECT VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,TRIM(ACODE) AS ACODE,IQTYOUT AS RGP_QTY,0 AS MRR_qTY,NULL AS MRRNO,TO_CHAR(VCHDATE,'DD/MM/YYYY')  AS MRRDT,NULL AS WONO,NULL AS WODT,desc_ FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='21' AND VCHDATE " + xprdRange + "  " + cond + "  UNION ALL SELECT RGPNUM ,TO_CHAR(RGPDATE,'DD/MM/YYYY') AS RGPDATE,TRIM(ICODE) AS ICODE,TRIM(ACODE) AS ACODE,0 AS RGP_QTY,IQTYIN AS mrr_qty,vchnum as mrrno,to_char(VCHDATE,'dd/mm/yyyy') as mrrdt,INVNO AS WONO,TO_CHAR(INVDATE,'DD/MM/YYYY') AS WODT,desc_ from ivoucher where branchcd='" + frm_mbr + "' AND TYPE='09' AND VCHDATE " + xprdRange + " " + cond + " ) GROUP BY VCHNUM,VCHDATE,ICODE,ACODE,MRRNO,MRRDT,WODT,WONO,DESC_) A ,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) ORDER BY a.rgp_qty desc"; //A.VCHNUM,A.ACODE,A.ICODE,A.MRRDT              

                // 09-07-2021 -- CHANGED TABLE IVOUCHER TO RGPMST FOR OMEG
                mq0 = "SELECT '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, A.VCHNUM,A.VCHDATE,A.ICODE,A.ACODE,A.RGP_QTY,A.MRR_QTY,A.MRRNO,A.MRRDT,A.WONO,A.WODT, B.ANAME,B.ADDR1,B.ADDR2,C.INAME,C.CPARTNO,a.desc_  FROM (SELECT VCHNUM,VCHDATE,ICODE,ACODE,SUM(RGP_QTY) AS RGP_QTY,SUM(MRR_QTY) AS MRR_QTY,MRRNO,MRRDT,WODT,WONO,DESC_ FROM (SELECT VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,TRIM(ACODE) AS ACODE,IQTYOUT AS RGP_QTY,0 AS MRR_qTY,NULL AS MRRNO,TO_CHAR(VCHDATE,'DD/MM/YYYY')  AS MRRDT,NULL AS WONO,NULL AS WODT,desc_ FROM RGPMST WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='21' AND VCHDATE " + xprdRange + "  " + cond + "  UNION ALL SELECT RGPNUM ,TO_CHAR(RGPDATE,'DD/MM/YYYY') AS RGPDATE,TRIM(ICODE) AS ICODE,TRIM(ACODE) AS ACODE,0 AS RGP_QTY,IQTYIN AS mrr_qty,vchnum as mrrno,to_char(VCHDATE,'dd/mm/yyyy') as mrrdt,INVNO AS WONO,TO_CHAR(INVDATE,'DD/MM/YYYY') AS WODT,desc_ from ivoucher where branchcd='" + frm_mbr + "' AND TYPE='09' AND VCHDATE " + xprdRange + " " + cond + " ) GROUP BY VCHNUM,VCHDATE,ICODE,ACODE,MRRNO,MRRDT,WODT,WONO,DESC_) A ,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) ORDER BY a.rgp_qty desc"; //A.VCHNUM,A.ACODE,A.ICODE,A.MRRDT              
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "JobWorkReg", "JobWorkReg", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F25158": //RGP VS MRR
                #region
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (party_cd.Trim().Length <= 1)
                {
                    party_cd = "%";
                }
                if (part_cd.Trim().Length <= 1)
                {
                    part_cd = "%";
                }
                cond = " and TRIM(acode) like '" + party_cd + "%' and TRIM(icode) like '" + part_cd + "%' ";
                mq0 = "";
                header_n = "Rgp Vs Mrr Report";
                mq0 = "SELECT '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, A.VCHNUM,A.VCHDATE,A.ICODE,A.ACODE,A.RGP_QTY,A.MRR_QTY,A.MRRNO,A.MRRDT,A.WONO,A.WODT, B.ANAME,B.ADDR1,B.ADDR2,C.INAME,C.CPARTNO,a.desc_  FROM (SELECT VCHNUM,VCHDATE,ICODE,ACODE,SUM(RGP_QTY) AS RGP_QTY,SUM(MRR_QTY) AS MRR_QTY,MRRNO,MRRDT,WODT,WONO,DESC_ FROM (SELECT VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,TRIM(ACODE) AS ACODE,IQTYOUT AS RGP_QTY,0 AS MRR_qTY,NULL AS MRRNO,TO_CHAR(VCHDATE,'DD/MM/YYYY')  AS MRRDT,NULL AS WONO,NULL AS WODT,desc_ FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='23' AND VCHDATE " + xprdRange + "  " + cond + "  UNION ALL SELECT RGPNUM ,TO_CHAR(RGPDATE,'DD/MM/YYYY') AS RGPDATE,TRIM(ICODE) AS ICODE,TRIM(ACODE) AS ACODE,0 AS RGP_QTY,IQTYIN AS mrr_qty,vchnum as mrrno,to_char(VCHDATE,'dd/mm/yyyy') as mrrdt,INVNO AS WONO,TO_CHAR(INVDATE,'DD/MM/YYYY') AS WODT,desc_ from ivoucher where branchcd='" + frm_mbr + "' AND TYPE='0J' AND VCHDATE " + xprdRange + " " + cond + " ) GROUP BY VCHNUM,VCHDATE,ICODE,ACODE,MRRNO,MRRDT,WODT,WONO,DESC_) A ,FAMST B,ITEM C WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) ORDER BY A.VCHNUM,A.ACODE,A.ICODE,A.MRRDT";

                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "RGP_Vs_MRR", "RGP_Vs_MRR", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F25271":
                #region
                dt2 = new DataTable(); dt = new DataTable(); dt1 = new DataTable();
                dt4 = new DataTable(); dt5 = new DataTable();
                header_n = "MRR-J/w Tie(08-27) up Report";
                dt2.Columns.Add("header", typeof(string));
                dt2.Columns.Add("fromdt", typeof(string));
                dt2.Columns.Add("todt", typeof(string));
                dt2.Columns.Add("acode", typeof(string));
                dt2.Columns.Add("party", typeof(string));
                dt2.Columns.Add("mrrno", typeof(string));
                dt2.Columns.Add("mrrdt", typeof(string));
                dt2.Columns.Add("mrrqty", typeof(double));
                dt2.Columns.Add("chlno", typeof(string));
                dt2.Columns.Add("chldt", typeof(string));
                dt2.Columns.Add("chlqty", typeof(double));
                dt2.Columns.Add("Invno", typeof(string));
                dt2.Columns.Add("Invdt", typeof(string));
                dt2.Columns.Add("excise_chlno", typeof(string));

                SQuery = "SELECT A.VCHNUM AS MRRNO,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS MRRDT,SUM(NVL(A.IQTYIN,0)) AS MRQTY,TRIM(A.ACODE) AS ACODE,B.ANAME,A.REFNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY') AS REFDATE  FROM IVOUCHER A,FAMST B WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND  A.BRANCHCD='" + frm_mbr + "' AND A.TYPE='08' AND  A.VCHDATE " + xprdRange + "  GROUP BY  A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ,TRIM(A.ACODE),A.REFNUM,TO_CHAR(A.REFDATE,'DD/MM/YYYY'),B.ANAME order by MRRNO";//and a.vchnum='000137' for testing
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //MRRDT

                SQuery = "SELECT VCHNUM AS CHLNO,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS chldt,SUM(NVL(IQTYout,0)) AS CHLQTY,TRIM(ACODE) AS ACODE,TC_NO,TO_CHAR(MR_GDATE,'DD/MM/YYYY') AS REFDATE ,ponum as invoice_no,to_char(podate,'dd/mm/yyyy') as invdate,naration as pono  FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='27' AND  VCHDATE " + xprdRange + " GROUP BY  VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') ,TRIM(ACODE),TC_NO,TO_CHAR(MR_GDATE,'DD/MM/YYYY'),trim(acode),ponum , to_char(podate,'dd/mm/yyyy'),naration order by CHLNO";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //CHLDT
                if (dt.Rows.Count > 0)
                {
                    DataView view1im = new DataView(dt);
                    DataTable dtdrsim = new DataTable();
                    dtdrsim = view1im.ToTable(true, "acode", "REFNUM", "refdate"); //MAIN        
                    foreach (DataRow dr0 in dtdrsim.Rows)
                    {
                        DataView viewim = new DataView(dt, "acode='" + dr0["acode"] + "' and REFNUM='" + dr0["REFNUM"] + "' and refdate='" + dr0["refdate"] + "'", "", DataViewRowState.CurrentRows);
                        dt4 = new DataTable();
                        dt4 = viewim.ToTable();
                        mq5 = "";
                        if (dt1.Rows.Count > 0)
                        {
                            DataView viewim1 = new DataView(dt1, "acode='" + dr0["acode"].ToString().Trim() + "' and tc_no='" + dr0["REFNUM"].ToString().Trim() + "' and REFDATE='" + dr0["refdate"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt5 = viewim1.ToTable();
                        }
                        for (int j = 0; j < dt5.Rows.Count; j++)
                        {
                            dr1 = dt2.NewRow();
                            dr1["header"] = header_n;
                            dr1["fromdt"] = fromdt;
                            dr1["todt"] = todt;
                            dr1["acode"] = dr0["acode"].ToString().Trim();
                            dr1["party"] = dt4.Rows[0]["ANAME"].ToString().Trim();
                            dr1["mrrno"] = dt4.Rows[0]["MRRNO"].ToString().Trim();
                            dr1["mrrdt"] = dt4.Rows[0]["MRRDT"].ToString().Trim();
                            dr1["mrrqty"] = fgen.make_double(dt4.Rows[0]["MRQTY"].ToString().Trim());
                            dr1["chlno"] = dt5.Rows[j]["CHLNO"].ToString().Trim();
                            dr1["chldt"] = dt5.Rows[j]["chldt"].ToString().Trim();
                            dr1["chlqty"] = fgen.make_double(dt5.Rows[j]["CHLQTY"].ToString().Trim());
                            dr1["Invno"] = dt5.Rows[j]["invoice_no"].ToString().Trim();
                            dr1["Invdt"] = dt5.Rows[j]["invdate"].ToString().Trim();
                            dt2.Rows.Add(dr1);
                        }
                    }
                }
                if (dt2.Rows.Count > 0)
                {
                    dt2.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt2, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_PRAG", "std_PRAG", dsRep, header_n);
                }
                #endregion
                else
                {
                    data_found = "N";
                }
                break;

            case "F25264":  //OPENING BAL
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                xprdRange1 = "BETWEEN TO_DATE('" + frm_cDt1 + "','DD/MM/YYYY') AND TO_DATE('" + fromdt + "','DD/MM/YYYY')-1";
                mq0 = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl,batch AS NO_BDLS from (Select A.branchcd,A.icode, a.IQTY as opening,0 as cdr,0 as ccr,0 as clos,trim(no_bdls) as batch from excvch a where A." + branch_Cd + " AND VCHDATE <=to_date('" + todt + "','dd/mm/yyyy') union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,trim(btchno) as batch FROM IVOUCHER where " + branch_Cd + " and type like '4%' and vchdate " + xprdRange1 + " and store='Y' GROUP BY ICODE,branchcd,trim(btchno) ,type union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,trim(btchno) as batch from IVOUCHER where " + branch_Cd + " and type like '4%' and vchdate " + xprdRange + " and store='Y' GROUP BY ICODE,branchcd,trim(btchno),type) where LENGTH(tRIM(ICODE))>=8   group by branchcd,trim(icode),batch ORDER BY ICODE";
                //mq0 = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as opening,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl,batch AS NO_BDLS from (Select A.branchcd,A.icode, a.IQTY as opening,0 as cdr,0 as ccr,0 as clos,trim(no_bdls) as batch from excvch a where A." + branch_Cd + " union all select branchcd,icode,sum(iqtyin)-sum(iqtyout) as op,0 as cdr,0 as ccr,0 as clos,trim(btchno) as batch FROM IVOUCHER where " + branch_Cd + " and type like '4%' and vchdate<=TO_DATE('" + todt + "','DD/MM/YYYY') and store='Y' GROUP BY ICODE,branchcd,trim(btchno) ,type union all select branchcd,icode,0 as op,sum(iqtyin) as cdr,sum(iqtyout) as ccr,0 as clos,trim(btchno) as batch from IVOUCHER where " + branch_Cd + " and type like '4%' and vchdate <=TO_DATE('" + todt + "','DD/MM/YYYY') and store='Y' GROUP BY ICODE,branchcd,trim(btchno),type) where LENGTH(tRIM(ICODE))>=8  group by branchcd,trim(icode),batch ORDER BY ICODE";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                // ALL DETAILS
                SQuery = "SELECT 'Stock Statement Report as on " + todt + "' as HEADER,A.VCHNUM,a.VCHDATE,A.ICODE,A.NO_BDLS,A.MFGDT1,A.EXPDT1,SUM(A.IQTY) AS IQTY,SUM(A.OQTY) AS OQTY,sum(a.iqty)-sum(a.oqty) as bal,I.INAME,B.INAME AS BINAME,B.ICODE AS BICODE,A.VDD,0 as opening,0 as closing,'" + mq3 + "' as catg FROM(select DISTINCT VCHNUM,to_char(VCHDATE,'dd/mm/yyyy') as vchdate,TRIM(ICODE) AS ICODE,TRIM(NO_BDLS) AS NO_BDLS,MFGDT1,EXPDT1,IQTY,0 AS OQTY,TO_CHAR(VCHDATE,'YYYYMMDD') AS VDD from excvch where " + branch_Cd + " and vchdate<=to_date('" + todt + "','dd/mm/yyyy') UNION ALL select VCHNUM,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS VCHDATE,TRIM(ICODE) AS ICODE,TRIM(BTCHNO) AS BTCHNO,MFGDT,EXPDT,0 AS IQTY,IQTYOUT,TO_CHAR(VCHDATE,'YYYYMMDD') AS VDD from ivoucher where " + branch_Cd + " and type like '4%' and vchdate " + xprdRange + ") A,ITEM I,ITEM B WHERE TRIM(A.ICODE)=TRIM(I.ICODE) AND SUBSTR(A.ICODE,0,4)=TRIM(B.ICODE) AND LENGTH(TRIM(B.ICODE))=4 GROUP BY A.ICODE,A.NO_BDLS,I.INAME,B.INAME,B.ICODE,A.MFGDT1,A.EXPDT1,A.VCHNUM,A.VCHDATE,A.vdd order by A.VDD,a.vchnum";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dtm = new DataTable();
                dtm = dt.Clone();
                dt2 = new DataTable();
                dt2.Columns.Add("Icode", typeof(string));
                dt2.Columns.Add("Batches", typeof(string));
                dr1 = null;
                // FOR FINDING ALL BATCHES EITHER WITH BAL OR NIL QTY
                if (dt1.Rows.Count > 0)
                {
                    DataView view1 = new DataView(dt1);
                    DataTable dtdrsim = new DataTable();
                    dtdrsim = view1.ToTable(true, "icode", "no_bdls");
                    foreach (DataRow drm in dtdrsim.Rows)
                    {
                        dt.CaseSensitive = true;
                        DataView view1im = new DataView(dt, "icode='" + drm["icode"].ToString().Trim() + "' and trim(no_bdls)='" + drm["no_bdls"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        DataTable dticode = new DataTable();
                        dticode = view1im.ToTable();
                        db1 = 0; db2 = 0;
                        for (i = 0; i < dticode.Rows.Count; i++)
                        {
                            dr1 = dt2.NewRow();
                            db1 += fgen.make_double(dticode.Rows[i]["iqty"].ToString().Trim());
                            db2 += fgen.make_double(dticode.Rows[i]["oqty"].ToString().Trim());
                        }
                        if (mq3 == "Bal")
                        {
                            if (db1 - db2 > 0)
                            {
                                dr1["icode"] = drm["icode"].ToString().Trim();
                                dr1["batches"] = drm["no_bdls"].ToString().Trim();
                                dt2.Rows.Add(dr1);
                            }
                        }
                        else
                        {
                            if (db1 - db2 == 0)
                            {
                                dr1["icode"] = drm["icode"].ToString().Trim();
                                dr1["batches"] = drm["no_bdls"].ToString().Trim();
                                dt2.Rows.Add(dr1);
                            }
                        }
                    }
                }
                // MERGING THOSE BATCHES IN TO A DATATABLE THAT SATISFY THE SELECTED CONDITION
                foreach (DataRow da in dt2.Rows)
                {
                    DataView view1im1 = new DataView(dt, "icode='" + da["icode"].ToString().Trim() + "' and no_bdls='" + da["batches"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                    DataTable dticode2 = new DataTable();
                    dticode2 = view1im1.ToTable();
                    for (i = 0; i < dticode2.Rows.Count; i++)
                    {
                        dr1 = dtm.NewRow();
                        dr1["header"] = dticode2.Rows[i]["header"].ToString().Trim();
                        dr1["icode"] = dticode2.Rows[i]["icode"].ToString().Trim();
                        dr1["iname"] = dticode2.Rows[i]["iname"].ToString().Trim();
                        dr1["no_bdls"] = dticode2.Rows[i]["no_bdls"].ToString().Trim();
                        dr1["vchnum"] = dticode2.Rows[i]["vchnum"].ToString().Trim();
                        dr1["vchdate"] = dticode2.Rows[i]["vchdate"].ToString().Trim();
                        dr1["vdd"] = dticode2.Rows[i]["vdd"].ToString().Trim();
                        dr1["bicode"] = dticode2.Rows[i]["bicode"].ToString().Trim();
                        dr1["biname"] = dticode2.Rows[i]["biname"].ToString().Trim();
                        dr1["iqty"] = fgen.make_double(dticode2.Rows[i]["iqty"].ToString().Trim());
                        dr1["oqty"] = fgen.make_double(dticode2.Rows[i]["oqty"].ToString().Trim());
                        dr1["mfgdt1"] = dticode2.Rows[i]["mfgdt1"].ToString().Trim();
                        dr1["expdt1"] = dticode2.Rows[i]["expdt1"].ToString().Trim();
                        dr1["catg"] = dticode2.Rows[i]["catg"].ToString().Trim();
                        dtm.Rows.Add(dr1);
                    }
                }
                //ASSIGNING THE OPENING AND CLOSING BAL
                foreach (DataRow dr3 in dtm.Rows)
                {
                    dr3["opening"] = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr3["icode"].ToString().Trim() + "' and no_bdls='" + dr3["no_bdls"].ToString().Trim() + "'", "opening"));
                    dr3["closing"] = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dr3["icode"].ToString().Trim() + "' and no_bdls='" + dr3["no_bdls"].ToString().Trim() + "'", "cl"));
                }
                //fgen.Print_Report_BYDT(co_cd, mbr, "crpt_TMIStockStatement", "crpt_TMIStockStatement", dtm);
                dsRep = new DataSet();
                dtm.TableName = "Prepcur";
                dsRep.Tables.Add(dtm);
                Print_Report_BYDS(frm_cocd, frm_mbr, "crpt_TMIStockStatement", "crpt_TMIStockStatement", dsRep, header_n);
                break;

            case "F25262":
                #region matl issue Sticker SEL
                SQuery = "";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //barCode = "00055319/04/2019";
                SQuery = "select trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') as fstr,a.branchcd,a.type,trim(a.vchnum) as vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vch_date,trim(a.acode) as acode,trim(a.icode) as icode , substr(trim(b.iname),1,28) as item_name,trim(a.iopr) as iopr,sum(a.iqty_chl) as iqty_chl,sum(a.iqtyout) as iqtyout ,trim(a.freight) as wo_no,trim(c.Iname) as mname,A.RCODE from ivoucher a ,item b , ITEM c  where trim(a.RCODE)=trim(c.ICODE) and trim(a.icode)=trim(b.icode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy') in (" + party_cd + ") group by trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy'),a.branchcd,a.type,trim(a.vchnum),to_char(a.vchdate,'dd/mm/yyyy'),trim(a.acode),trim(a.icode) , substr(trim(b.iname),1,28),trim(a.iopr),trim(a.freight),trim(c.Iname),A.RCODE order by vchnum";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt1.Rows.Count > 0)
                {
                    dt1.TableName = "barcode";
                    //dt1 = fgen.addBarCode(dt1, "fstr", true);
                    dsRep.Tables.Add(dt1);
                    frm_rptName = "iss_Sel_stk";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "iss_Sel_stk", frm_rptName, dsRep, "", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F25118":
                #region CR
                sname = "";
                header_n = "Customer Rejection";
                SQuery = "SELECT a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'dd/mm/yyyy') as fstr,'-' AS btoprint,d.ANAME,TRIM(D.ANAME) AS CUST,TRIM(D.ADDR1) AS ADRES1," +
                "TRIM(D.ADDR2) AS ADRES2,TRIM(D.ADDR3) AS ADRES3,TRIM(D.GIRNO) AS CUSTPAN,TRIM(D.STAFFCD) AS STAFFCD,TRIM(D.PERSON) AS CPERSON,TRIM(D.EMAIL) AS CMAIL,TRIM(D.TELNUM) AS CONT,TRIM(D.STATEN) AS CSTATE, TRIM(D.GST_NO) AS C_GST,SUBSTR(TRIM(D.GST_NO),1,2) AS STAT_CODE,TRIM(C.INAME) AS INAME,TRIM(C.CPARTNO) AS  PARTNO,TRIM(C.PUR_UOM) AS CMT,TRIM(C.NO_PROC) AS Sunit,TRIM(C.UNIT) AS CUNIT,TRIM(C.HSCODE) AS HSCODE,A.*,'Customer Rejection' AS CASE,nvl(d.email,'-') as p_email FROM WB_CUST_REJ A,ITEM C,FAMST D WHERE TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(A.ACODE)=TRIM(D.ACODE) AND " +
                "A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='CR' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") ORDER BY a.vchdate,a.vchnum";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    pdfView = "N";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "cust_rej_lrfp", "cust_rej_lrfp", dsRep, header_n, "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F25144B":
                header_n = "Crate Ledger";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                xprdRange1 = "between to_Date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt1 + "','dd/mm/yyyy')-1";
                string xprdRange2 = "between to_Date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy')";
                if (party_cd.Trim().Length <= 1)
                {
                    party_cd = "%";
                }
                if (part_cd.Trim().Length <= 1)
                {
                    part_cd = "%";
                }

                ph_tbl = new DataTable();
                ph_tbl.Columns.Add("fromdt", typeof(string));
                ph_tbl.Columns.Add("todt", typeof(string));
                ph_tbl.Columns.Add("acode", typeof(string));
                ph_tbl.Columns.Add("aname", typeof(string));
                ph_tbl.Columns.Add("addr1", typeof(string));
                ph_tbl.Columns.Add("addr2", typeof(string));
                ph_tbl.Columns.Add("vchnum", typeof(string));
                ph_tbl.Columns.Add("vchdate", typeof(DateTime));
                ph_tbl.Columns.Add("op", typeof(string));
                ph_tbl.Columns.Add("cdr", typeof(double));
                ph_tbl.Columns.Add("ccr", typeof(double));
                ph_tbl.Columns.Add("clos", typeof(double));
                ph_tbl.Columns.Add("type", typeof(string));
                //ph_tbl.Columns.Add("totop", typeof(double));


                DataSet dsreplrfp1;
                DataTable dtlrfp1 = new DataTable();

                SQuery = "Select trim(params) as param from controls where id='I88'";
                dtlrfp1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dtlrfp1.Rows.Count > 0)
                {
                    dsreplrfp1 = new DataSet();
                    dtlrfp1.TableName = "Controls";
                    dsreplrfp1.Tables.Add(fgen.mTitle(dtlrfp1, repCount));

                }
                string user_acode = "";
                user_acode = fgenMV.Fn_Get_Mvar(frm_qstr, "U_UNAME");
                DataTable dtlrfp3 = new DataTable();
                //DataSet dsReplrfp;
                user_acode = "16S011";
                cond = "and acode='" + user_acode + "'";
                SQuery = "Select '" + frm_cDt1 + "' as FromDate,'" + frm_cDt2 + "' as ToDate, acode,aname,addr1,addr2 from famst where 1=1 " + cond + "";
                dtlrfp3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);


                //DataSet dsreplrfp2;
                DataTable dtlrfp2 = new DataTable();
                int xop = 0;

                SQuery = "SELECT sum(a.op) AS OP,a.type,a.vchnum,a.vchdate as vchdate,A.ACODE,SUM(a.cdr) AS CDR,SUM(a.ccr) AS CCR,SUM(a.clos) AS CLOS FROM (select 'Yop' as type,'000000' as vchnum,'" + frm_cDt1 + "' as Vchdate,clqty as op,0 as cdr,0 as ccr,0 as clos,ACODE from crate_bal where branchcd='" + frm_mbr + "' and acode='" + user_acode + "' and icode like '" + part_cd + "%' union all select 'Pop' as type,'000000' as vchnum,'" + frm_cDt1 + "' as Vchdate,nvl(sum(nvl(iqtyout,0))-sum(nvl(iqtyin,0)),0)  as op,0 as cdr,0 as ccr,0 as clos,ACODE from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate between to_Date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt1 + "','dd/mm/yyyy')-1 " + cond + " and icode like '" + part_cd + "%' and store='Y' group by ACODE union all select type,vchnum,to_Char(vchdate,'dd/mm/yyyy') as vchdate,0 as op,iqtyin as cdr,iqtyout as ccr,0 as clos,ACODE from ivoucher where branchcd='" + frm_mbr + "' and type like '%' and vchdate between to_Date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + frm_cDt2 + "','dd/mm/yyyy') " + cond + " and icode like '" + part_cd + "%' and store='Y')  A group by a.type,a.vchnum,a.vchdate,ACODE";
                dtlrfp2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dtlrfp2.Rows.Count > 0)
                {
                    DataView view1 = new DataView(dtlrfp2);
                    DataTable dtdrsim = new DataTable();
                    dtdrsim = view1.ToTable(true, "ACODE"); //MAIN
                    foreach (DataRow dr2 in dtdrsim.Rows)
                    {
                        DataView viewim = new DataView(dtlrfp2, "ACODE='" + dr2["ACODE"] + "'", "", DataViewRowState.CurrentRows);
                        dr1 = ph_tbl.NewRow();
                        dt1 = new DataTable();
                        dt1 = viewim.ToTable();
                        db1 = 0; mq1 = ""; db = 0; db2 = 0; db6 = 0; db7 = 0; int m = 0;
                        for (i = 0; i < dt1.Rows.Count; i++)
                        {
                            //mq1 = "TYPE";
                            //switch(mq1)
                            //{
                            //    case"YOP":

                            //        break;
                            //}
                            m = dt1.Rows.Count - 1;
                            xop = Convert.ToInt32(xop) + Convert.ToInt32(dt1.Rows[i]["op"]);
                            dr1 = ph_tbl.NewRow();
                            dr1["fromdt"] = frm_cDt1;
                            dr1["todt"] = frm_cDt2;
                            //dr1["header"] = header_n;
                            dr1["acode"] = dt1.Rows[i]["acode"].ToString().Trim();
                            //  dr1["aname"] = dt1.Rows[i]["aname"].ToString().Trim();
                            dr1["aname"] = fgen.seek_iname_dt(dtlrfp3, "acode='" + dt1.Rows[i]["acode"].ToString().Trim() + "'", "aname");
                            dr1["addr1"] = fgen.seek_iname_dt(dtlrfp3, "acode='" + dt1.Rows[i]["acode"].ToString().Trim() + "'", "addr1");
                            dr1["addr2"] = fgen.seek_iname_dt(dtlrfp3, "acode='" + dt1.Rows[i]["acode"].ToString().Trim() + "'", "addr2");
                            dr1["vchnum"] = dt1.Rows[i]["vchnum"].ToString().Trim();
                            dr1["vchdate"] = dt1.Rows[i]["vchdate"].ToString().Trim();
                            dr1["op"] = fgen.make_double(dt1.Rows[i]["op"].ToString().Trim());
                            dr1["cdr"] = fgen.make_double(dt1.Rows[i]["cdr"].ToString().Trim());
                            dr1["ccr"] = fgen.make_double(dt1.Rows[i]["ccr"].ToString().Trim());
                            dr1["clos"] = fgen.make_double(dt1.Rows[i]["clos"].ToString().Trim());
                            dr1["type"] = dt1.Rows[i]["type"].ToString().Trim();
                            ///dr1["totop"] = fgen.make_double(dt1.Rows[i]["op"].ToString().Trim());
                            ph_tbl.Rows.Add(dr1);
                        }


                    }

                }

                dsRep = new DataSet();
                ph_tbl.TableName = "Prepcur";
                dsRep.Tables.Add(ph_tbl);
                Print_Report_BYDS(frm_cocd, frm_mbr, "Crateledger", "Crateledger", dsRep, header_n);
                break;
            case "F25244":
                #region Reel Ledger

                frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, " U_MDT1");
                todt = fgenMV.Fn_Get_Mvar(frm_qstr, " U_MDT2");
                xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                xprd1 = "between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                xprd2 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                if (party_cd.Length <= 1)
                {
                    mq2 = " ";
                }
                else
                {
                    mq2 = " and substr(d.icode,1,2)='" + party_cd + "'";
                }

                if (part_cd.Length <= 1)
                {
                    mq3 = " ";
                }
                else
                {
                    mq3 = " and substr(d.icode,1,4)='" + part_cd + "'";
                }
                icodecond = "" + mq2 + " " + mq3 + " ";

                header_n = "Reel Ledger";
                xprdRange1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                cond = "";

                //icodecond = " and d.icode='03030002' and d.my_reel='00000020' ";

                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) cond = " and trim(icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1) cond = " and trim(icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";
                //SQuery = "SELECT * FROM (select b.iname,b.pur_uom,b.bfactor,b.oprate1 as psize,b.oprate3 as gsm,b.oprate1,b.oprate2,b.oprate3,trim(a.kclreelno)as My_reel,min(vchdate) as Vchdate,max(trim(upper(a.coreelno))) as Co_reel,trim(a.icode) as Icode,sum(a.opening) as op,sum(pdr) as pwd,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+SUM(A.PDR)+sum(a.cdr)-sum(a.ccr) as closing,MAX(aCODE) AS ACODE,substr(a.icode,1,4) as Igrp,max(insp_done) as Insp_done,max(origwt) as origwt,max(rlocn) as rlocn,max(reel_mill) as reel_mill from (Select null as vchdate,kclreelno,null as coreelno,icode, reelwin as opening,0 as pdr,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE,null as insp_done,0 as origwt,rlocn,'-' as reel_mill from reelvch where branchcd='" + frm_mbr + "'  and substr(nvl(rinsp_by,'-'),1,6)='REELOP' and 1=2 union all  select min(vchdate) As vchdate,kclreelno,coreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as pdr,0 as cdr,0 as ccr,0 as clos,null AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' as reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,sum(reelwin) as pdr,0 as cdr,0 as ccr,0 as clos,MAX(aCODE) AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt ,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + xprdRange1 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, sum(reelwin) as cdr,0 as ccr,0 as clos,null AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '1%' and vchdate " + xprdRange1 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, 0 as cdr,sum(reelwout) as ccr,0 as clos,null AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE )a,item b where trim(a.icode)=trim(B.icode) and nvl(b.oprate1,0) like '%' and nvl(b.oprate3,0) like '%' and nvl(b.bfactor,0) like '%'  group by b.iname,b.pur_uom,b.bfactor,b.oprate1,b.oprate2,b.oprate3,trim(a.icode),substr(a.icode,1,4),trim(a.kclreelno) )m where 1=1 and nvl(m.aCODE,'%') like '%' ";

                SQuery = "SELECT * FROM (select b.iname,b.pur_uom,b.bfactor,b.oprate1 as psize,b.oprate3 as gsm,b.oprate1,b.oprate2,b.oprate3,trim(a.kclreelno)as My_reel,vchnum as vchnum,vchdate as Vchdate,job_no as job_no,job_dt as job_dt,trim(upper(a.coreelno)) as Co_reel,trim(a.icode) as Icode,a.opening as op,pdr as pwd,a.cdr as inwd,a.ccr as outw,a.opening+A.PDR+a.cdr-a.ccr as closing,aCODE AS ACODE,substr(a.icode,1,4) as Igrp,insp_done as Insp_done,origwt as origwt,rlocn as rlocn,reel_mill as reel_mill,b.unit as unit,b.cpartno as cpartno,type from (Select null as vchnum,null as vchdate,job_no as job_no,job_dt as job_dt,type,kclreelno,null as coreelno,icode, reelwin as opening,0 as pdr,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE,null as insp_done,0 as origwt,rlocn as rlocn,'-' as reel_mill from reelvch where branchcd='" + frm_mbr + "'  and substr(nvl(rinsp_by,'-'),1,6)='REELOP' and 1=2 union all  select null as vchnum,null As vchdate,null as job_no,null as job_dt,'OP' as type,kclreelno,coreelno,icode,sum(reelwin-reelwout) as op,0 as pdr,0 as cdr,0 as ccr,0 as clos,null AS ACODE,null AS insp_done,0 as origwt,null As rlocn,'-' as reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate  " + xprd1 + " and posted='Y' group by kclreelno,coreelno,icode union all select vchnum as vchnum,vchdate As vchdate,job_no as job_no,job_dt as job_dt,type,kclreelno,coreelno,icode,0 as op,reelwin as pdr,0 as cdr,0 as ccr,0 as clos,aCODE AS ACODE,rpapinsp AS insp_done,(Case when type in ('02','05','0U','07','70') then reelwin else 0 end) as origwt ,rlocn As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '0%' and vchdate  " + xprd2 + " and posted='Y'  union all select vchnum as vchnum,vchdate As vchdate,job_no as job_no,job_dt as job_dt,type,kclreelno,coreelno,icode,0 as op,0 as pdr, reelwin as cdr,0 as ccr,0 as clos,null AS ACODE,rpapinsp AS insp_done,(Case when type in ('02','05','0U','07','70') then reelwin else 0 end) as origwt,rlocn As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '1%' and vchdate " + xprd2 + " and posted='Y'  union all select vchnum as vchnum,vchdate As vchdate,job_no as job_no,job_dt as job_dt,type,kclreelno,coreelno,icode,0 as op,0 as pdr, 0 as cdr,reelwout as ccr,0 as clos,null AS ACODE,rpapinsp AS insp_done,(Case when type in ('02','05','0U','07','70') then reelwin else 0 end) as origwt,rlocn As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate  " + xprd2 + " and posted='Y' and length(reelwout)>1  )a,item b where trim(a.icode)=trim(B.icode) and nvl(b.oprate1,0) like '%' and nvl(b.oprate3,0) like '%' and nvl(b.bfactor,0) like '%'   )m where 1=1 and nvl(m.aCODE,'%') like '%'";
                //SQuery = "SELECT * FROM (select b.iname,b.pur_uom,b.bfactor,b.oprate1 as psize,b.oprate3 as gsm,b.oprate1,b.oprate2,b.oprate3,trim(a.kclreelno)as My_reel,vchnum as vchnum,vchdate as Vchdate,job_no as job_no,job_dt as job_dt,trim(upper(a.coreelno)) as Co_reel,trim(a.icode) as Icode,nvl(x.op,0) as op,pdr as pwd,a.cdr as inwd,a.ccr as outw,nvl(x.op,0)+A.PDR+a.cdr-a.ccr as closing,aCODE AS ACODE,substr(a.icode,1,4) as Igrp,insp_done as Insp_done,origwt as origwt,rlocn as rlocn,reel_mill as reel_mill,b.unit as unit,b.cpartno as cpartno,type from (select vchnum as vchnum,vchdate As vchdate,job_no as job_no,job_dt as job_dt,type,kclreelno,coreelno,icode,0 as op,reelwin as pdr,0 as cdr,0 as ccr,0 as clos,aCODE AS ACODE,rpapinsp AS insp_done,(Case when type in ('02','05','0U','07','70') then reelwin else 0 end) as origwt ,rlocn As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '0%' and vchdate  " + xprd2 + " and posted='Y'  union all select vchnum as vchnum,vchdate As vchdate,job_no as job_no,job_dt as job_dt,type,kclreelno,coreelno,icode,0 as op,0 as pdr, reelwin as cdr,0 as ccr,0 as clos,null AS ACODE,rpapinsp AS insp_done,(Case when type in ('02','05','0U','07','70') then reelwin else 0 end) as origwt,rlocn As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '1%' and vchdate " + xprd2 + " and posted='Y'  union all select vchnum as vchnum,vchdate As vchdate,job_no as job_no,job_dt as job_dt,type,kclreelno,coreelno,icode,0 as op,0 as pdr, 0 as cdr,reelwout as ccr,0 as clos,null AS ACODE,rpapinsp AS insp_done,(Case when type in ('02','05','0U','07','70') then reelwin else 0 end) as origwt,rlocn As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate  " + xprd2 + " and posted='Y' and length(reelwout)>1  )a  left outer join (select kclreelno,coreelno,icode,sum(reelwin-reelwout) as op from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate  " + xprd1 + " and posted='Y' group by kclreelno,coreelno,icode) x on trim(a.icode)||trim(a.kclreelno)=trim(x.icode)||trim(x.kclreelno) ,item b where trim(a.icode)=trim(B.icode) and nvl(b.oprate1,0) like '%' and nvl(b.oprate3,0) like '%' and nvl(b.bfactor,0) like '%'   )m where 1=1 and nvl(m.aCODE,'%') like '%'";
                SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + header_n + "' as header,d.icode as item,d.icode,d.Iname,d.my_reel as Lot_No,d.op as iopqty,d.pwd as Pur_Qty,d.inwd as iqtyin,d.outw as iqtyout,d.rlocn as Location,null as weight,null as imin,null as imax,d.unit,null as deptt,d.cpartno,d.job_no,d.job_dt,d.vchnum,d.vchdate,d.type,d.acode,b.aname from (" + SQuery + ") d left outer join famst b on trim(d.acode)=trim(b.acode) where 1=1 " + icodecond + " order by d.op,d.type,d.my_reel,d.icode ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "stkreel", "stkreel", dsRep, header_n);

                }
                #endregion
                break;
            case "F25236V":
            case "F25236":
                #region Stock Ageing Report
                frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");
                fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, " U_MDT1");
                todt = fgenMV.Fn_Get_Mvar(frm_qstr, " U_MDT2");
                xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, " U_PRDRANGE");
                xprd1 = "between to_date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy') -1";
                xprd2 = "between to_date('" + fromdt + "','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                if (party_cd.Length <= 1)
                {
                    mq2 = " ";
                }
                else
                {
                    mq2 = " and substr(d.icode,1,2)='" + party_cd + "'";
                }

                if (part_cd.Length <= 1)
                {
                    mq3 = " ";
                }
                else
                {
                    mq3 = " and substr(d.icode,1,4)='" + part_cd + "'";
                }
                icodecond = "" + mq2 + " " + mq3 + " ";
                if (iconID == "F25236")
                {
                    SQuery = "SELECT * FROM (select b.iname,b.pur_uom,b.bfactor,b.oprate1 as psize,b.oprate3 as gsm,b.oprate1,b.oprate2,b.oprate3,trim(a.kclreelno)as My_reel,min(vchdate) as Vchdate,max(trim(upper(a.coreelno))) as Co_reel,trim(a.icode) as Icode,sum(a.opening) as op,sum(pdr) as pwd,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+SUM(A.PDR)+sum(a.cdr)-sum(a.ccr) as closing,MAX(ACODE) AS ACODE,substr(a.icode,1,4) as Igrp,max(insp_done) as Insp_done,max(origwt) as origwt,max(rlocn) as rlocn,max(reel_mill) as reel_mill from (Select null as vchdate,kclreelno,null as coreelno,icode, reelwin as opening,0 as pdr,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE,null as insp_done,0 as origwt,rlocn,'-' as reel_mill from reelvch where branchcd='" + frm_mbr + "'  and substr(nvl(rinsp_by,'-'),1,6)='REELOP' and 1=2 union all  select min(vchdate) As vchdate,kclreelno,coreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as pdr,0 as cdr,0 as ccr,0 as clos,null AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' as reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,sum(reelwin) as pdr,0 as cdr,0 as ccr,0 as clos,MAX(aCODE) AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt ,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, sum(reelwin) as cdr,0 as ccr,0 as clos,null AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '1%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, 0 as cdr,sum(reelwout) as ccr,0 as clos,null AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE )a,item b where trim(a.icode)=trim(B.icode) and nvl(b.oprate1,0) like '%' and nvl(b.oprate3,0) like '%' and nvl(b.bfactor,0) like '%'  group by b.iname,b.pur_uom,b.bfactor,b.oprate1,b.oprate2,b.oprate3,trim(a.icode),substr(a.icode,1,4),trim(a.kclreelno) )m where 1=1 and nvl(m.aCODE,'%') like '%' ";
                    SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,e.aname as Party,d.Iname as Item_Name,d.my_reel as Lot_No,d.op as Opening_qty,d.pwd as Purchase_Qty,d.outw as Issue_Qty,d.inwd as return_Qty,d.closing as Closing_Qty,d.co_Reel as comp_batch,d.Icode,d.insp_done,d.rlocn from (" + SQuery + ") d left join famst e on trim(d.acode)=trim(e.acode) where 1=1 " + icodecond + "  order by d.icode,d.my_reel ";
                }
                else
                {
                    SQuery = "SELECT * FROM (select b.iname,b.pur_uom,b.bfactor,b.oprate1 as psize,b.oprate3 as gsm,b.oprate1,b.oprate2,b.oprate3,trim(a.kclreelno)as My_reel,min(vchdate) as Vchdate,max(trim(upper(a.coreelno))) as Co_reel,trim(a.icode) as Icode,sum(a.opening) as op,sum(pdr) as pwd,sum(a.cdr) as inwd,sum(a.ccr) as outw,sum(a.opening)+SUM(A.PDR)+sum(a.cdr)-sum(a.ccr) as closing,MAX(ACODE) AS ACODE,substr(a.icode,1,4) as Igrp,max(insp_done) as Insp_done,max(origwt) as origwt,max(rlocn) as rlocn,max(reel_mill) as reel_mill,max(a.irate) as irate from (Select null as vchdate,kclreelno,null as coreelno,icode, reelwin as opening,0 as pdr,0 as cdr,0 as ccr,0 as clos,NULL AS ACODE,null as insp_done,0 as origwt,rlocn,'-' as reel_mill,0 as irate from reelvch where branchcd='" + frm_mbr + "'  and substr(nvl(rinsp_by,'-'),1,6)='REELOP' and 1=2 union all  select min(vchdate) As vchdate,kclreelno,coreelno,icode,sum(reelwin)-sum(reelwout) as op,0 as pdr,0 as cdr,0 as ccr,0 as clos,null AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' as reel_mill,max(irate) as irate from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd1 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,sum(reelwin) as pdr,0 as cdr,0 as ccr,0 as clos,MAX(aCODE) AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt ,max(rlocn) As rlocn,'-' As reel_mill,max(irate) as irate from reelvch where branchcd='" + frm_mbr + "' and type like '0%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, sum(reelwin) as cdr,0 as ccr,0 as clos,null AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill,max(irate) as irate from reelvch where branchcd='" + frm_mbr + "' and type like '1%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE union all select min(vchdate) As vchdate,kclreelno,coreelno,icode,0 as op,0 as pdr, 0 as cdr,sum(reelwout) as ccr,0 as clos,null AS ACODE,MAX(rpapinsp) AS insp_done,(Case when type in ('02','05','0U','07','70') then sum(reelwin) else 0 end) as origwt,max(rlocn) As rlocn,'-' As reel_mill,max(irate) as irate from reelvch where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprd2 + " and posted='Y'  GROUP BY type,kclreelno,coreelno,ICODE )a,item b where trim(a.icode)=trim(B.icode) and nvl(b.oprate1,0) like '%' and nvl(b.oprate3,0) like '%' and nvl(b.bfactor,0) like '%'  group by b.iname,b.pur_uom,b.bfactor,b.oprate1,b.oprate2,b.oprate3,trim(a.icode),substr(a.icode,1,4),trim(a.kclreelno) )m where 1=1 and nvl(m.aCODE,'%') like '%' ";
                    SQuery = "select '" + fromdt + "' as fromdt,'" + todt + "' as todt,e.aname as Party,d.Iname as Item_Name,d.my_reel as Lot_No,d.op as Opening_qty,d.pwd as Purchase_Qty,d.outw  as Issue_Qty,d.inwd as return_Qty,d.closing as Closing_Qty,d.co_Reel as comp_batch,d.Icode,d.insp_done,d.rlocn,d.irate from (" + SQuery + ") d left join famst e on trim(d.acode)=trim(e.acode) where 1=1 " + icodecond + "  order by d.icode,d.my_reel ";
                }

                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                dt2 = new DataTable();
                dt2.Columns.Add("fromdt", typeof(string));
                dt2.Columns.Add("todt", typeof(string));
                dt2.Columns.Add("Party", typeof(string));
                dt2.Columns.Add("Item_Name", typeof(string));
                dt2.Columns.Add("Lot_No", typeof(string));
                dt2.Columns.Add("Opening_qty", typeof(double));
                dt2.Columns.Add("Purchase_Qty", typeof(double));
                dt2.Columns.Add("Closing_Qty", typeof(double));
                dt2.Columns.Add("comp_batch", typeof(string));
                dt2.Columns.Add("Icode", typeof(string));
                dt2.Columns.Add("insp_done", typeof(string));
                dt2.Columns.Add("rlocn", typeof(string));
                dt2.Columns.Add("irate", typeof(double));

                //dr1 = new DataRow();
                dr1 = null;
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = dt2.NewRow();
                        dr1["fromdt"] = fromdt;
                        dr1["todt"] = todt;
                        dr1["Party"] = dt.Rows[i]["Party"].ToString().Trim();
                        dr1["Item_Name"] = dt.Rows[i]["Item_Name"].ToString().Trim();
                        dr1["Lot_No"] = dt.Rows[i]["Lot_No"].ToString().Trim();
                        dr1["Opening_qty"] = fgen.make_double(dt.Rows[i]["Opening_qty"].ToString().Trim());
                        dr1["Purchase_Qty"] = fgen.make_double(dt.Rows[i]["Purchase_Qty"].ToString().Trim());
                        dr1["Closing_Qty"] = fgen.make_double(dt.Rows[i]["Closing_Qty"].ToString().Trim());
                        dr1["comp_batch"] = dt.Rows[i]["comp_batch"].ToString().Trim();
                        dr1["Icode"] = dt.Rows[i]["Icode"].ToString().Trim();
                        dr1["insp_done"] = dt.Rows[i]["insp_done"].ToString().Trim();
                        dr1["rlocn"] = dt.Rows[i]["rlocn"].ToString().Trim();
                        if (iconID == "F25236")
                        {
                            dr1["irate"] = 0;
                        }
                        else
                        {
                            dr1["irate"] = fgen.make_double(dt.Rows[i]["irate"].ToString().Trim());
                        }
                        dt2.Rows.Add(dr1);
                    }
                }

                string[] slab1 = new string[] { "0_to_30", "31_to_60", "61_to_90", "91_to_120", "121_to_180", "181_to_360" };
                string allSlab = "";
                string sumallSlab = "";
                string todaysDt = "to_date('" + todt + "','dd/mm/yyyy')";
                mq0 = "";
                for (int s = 0; s < slab1.Length; s++)
                {
                    mq0 = "R_" + slab1[s];
                    dt2.Columns.Add(mq0, typeof(double));
                    dt2.Columns.Add(mq0 + "_V", typeof(double));
                    allSlab += "," + "(case when (" + todaysDt + " - VCHDATE BETWEEN " + slab1[s].Replace("_", " ").Replace("to", "and") + ") THEN QTY END) as " + mq0;
                    sumallSlab += ", " + "sum(" + mq0 + ") as " + mq0;
                }
                dt2.Columns.Add("Others");
                dt2.Columns.Add("Others_V");
                if (allSlab != "")
                {
                    allSlab = allSlab.TrimStart(',');
                    sumallSlab = sumallSlab.TrimStart(',');
                }
                DataTable dtMRR = new DataTable();
                SQuery = "SELECT ICODE ," + sumallSlab + " FROM (SELECT ICODE, " + allSlab + " FROM (SELECT TRIM(ICODE) AS ICODE, VCHDATE, reelwin AS QTY FROM reelvch WHERE branchcd='" + frm_mbr + "' AND (TYPE LIKE '%') AND VCHDATE BETWEEN TO_dATE('" + frm_cDt1 + "','dd/mm/yyyy') AND TO_dATE('" + todt + "','dd/mm/yyyy') AND posted='Y' UNION ALL SELECT TRIM(ICODE) AS ICODE, VCHDATE, reelwin AS QTY FROM reelvch WHERE branchcd='" + frm_mbr + "' AND TYPE LIKE '1%'  AND VCHDATE BETWEEN TO_dATE('" + frm_cDt1 + "','dd/mm/yyyy') AND TO_dATE('" + todt + "','dd/mm/yyyy') AND posted='Y' ) ) group by ICODE order by icode   ";
                dtMRR = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                fullQty = 0;
                SQuery = "";
                foreach (DataRow drr in dt2.Rows)
                {
                    if (drr["Closing_Qty"].ToString().toDouble() > 0)
                    {
                        fullQty = drr["Closing_Qty"].ToString().toDouble();

                        col1 = "";
                        if (dtMRR.Rows.Count > 0)
                        {
                            col1 = fgen.seek_iname_dt(dtMRR, "ICODE='" + drr["icode"].ToString().TrimStart() + "'", "icode");
                        }
                        //if (col1.Length > 4)
                        {
                            col1 = "0";
                            for (int s = 0; s < slab1.Length; s++)
                            {

                                mq0 = "R_" + slab1[s];
                                col1 = fgen.seek_iname_dt(dtMRR, "ICODE='" + drr["icode"].ToString().TrimStart() + "'", mq0);
                                if (fullQty < col1.toDouble()) col1 = fullQty.toDouble(4).ToString();
                                if (col1.Length >= 1)
                                {
                                    drr[mq0] = col1;
                                }
                                else
                                {
                                    drr[mq0] = 0;
                                }
                                if (iconID == "F25236V")
                                {
                                    if (col1.Length >= 1)
                                    {
                                        drr[mq0 + "_V"] = (col1.toDouble() * drr["irate"].ToString().toDouble()).toDouble(4).ToString();
                                    }
                                    else
                                    {
                                        drr[mq0 + "_V"] = 0;
                                    }

                                }
                                else
                                {
                                    drr[mq0 + "_V"] = 0;
                                }


                                fullQty = (fullQty - col1.toDouble()).toDouble(4);
                                if (fullQty == 0) break;
                            }
                            if (fullQty > 0)
                            {
                                drr["OTHERS"] = fullQty.toDouble(4);
                                if (iconID == "F25236V")
                                {
                                    drr["OTHERS_V"] = (fullQty.toDouble(4) * drr["irate"].ToString().toDouble()).toDouble(4).ToString();
                                }
                                else
                                {
                                    drr["OTHERS_V"] = "";
                                }

                            }
                        }

                    }
                }
                dt2.TableName = "Prepcur";
                dsRep.Tables.Add(dt2);
                if (iconID == "F25236")
                {
                    Print_Report_BYDS(frm_cocd, frm_mbr, "stock_ageing", "stock_ageing", dsRep, header_n);
                }
                else
                {
                    Print_Report_BYDS(frm_cocd, frm_mbr, "stock_ageing_val", "stock_ageing_val", dsRep, header_n);
                }

                #endregion
                break;

            case "RPT19":// mrr passing form report
                #region
                dtm = new DataTable();
                dtm.Columns.Add("header", typeof(string));
                dtm.Columns.Add("fromdt", typeof(string));
                dtm.Columns.Add("todt", typeof(string));
                dtm.Columns.Add("type", typeof(string));
                dtm.Columns.Add("mrrno", typeof(string));
                dtm.Columns.Add("mrrdt", typeof(string));
                dtm.Columns.Add("supplier", typeof(string));
                dtm.Columns.Add("geno", typeof(string));
                dtm.Columns.Add("gedate", typeof(string));
                dtm.Columns.Add("qcdate", typeof(string));
                dtm.Columns.Add("qcdays", typeof(Int32));
                dtm.Columns.Add("acref", typeof(string));
                dtm.Columns.Add("acdays", typeof(Int32));
                header_n = "MRR,G.E,Q.C,A/c Linking Register";
                SQuery = "select a.type,a.vchnum as mrrno,to_char(a.vchdate,'dd/mm/yyyy') as mrrdt,a.genum,to_char(a.gedate,'dd/mm/yyyy') as gedate,a.qcdate,a.acode,to_char(to_date(a.qcdate,'dd/mm/yyyy'),'dd/mm/yyyy') as qcdate1,a.finvno,substr(trim(a.finvno),8,10) as accref,b.aname from ivoucher a,famst b where trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "' and a.type like '0%' and a.vchdate " + xprdRange + " and nvl(trim(a.finvno),'-')!='-' and nvl(a.inspected,'-')='Y' ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dr1 = dtm.NewRow();
                DateTime dd;
                TimeSpan diff, diff1;
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    mq1 = ""; mq2 = "";
                    dr1 = dtm.NewRow();
                    dr1["header"] = header_n;
                    dr1["fromdt"] = fromdt;
                    dr1["todt"] = todt;
                    dr1["type"] = dt.Rows[i]["type"].ToString().Trim();
                    dr1["mrrno"] = dt.Rows[i]["mrrno"].ToString().Trim();
                    dr1["mrrdt"] = dt.Rows[i]["mrrdt"].ToString().Trim();
                    dr1["supplier"] = dt.Rows[i]["aname"].ToString().Trim();
                    dr1["geno"] = dt.Rows[i]["genum"].ToString().Trim();
                    dr1["gedate"] = dt.Rows[i]["gedate"].ToString().Trim();
                    dr1["qcdate"] = dt.Rows[i]["qcdate"].ToString().Trim();
                    diff1 = Convert.ToDateTime(dr1["mrrdt"].ToString().Trim()) - Convert.ToDateTime(dr1["gedate"].ToString().Trim());
                    dr1["qcdays"] = diff1.Days;
                    dr1["acref"] = dt.Rows[i]["finvno"].ToString().Trim();
                    dd = Convert.ToDateTime(dt.Rows[i]["accref"].ToString().Trim());
                    //acdays==acref-gedate
                    diff = dd - Convert.ToDateTime(dt.Rows[i]["gedate"].ToString().Trim());
                    dr1["acdays"] = diff.Days;
                    if (diff1.Days == 0)
                    {
                        dtm.Rows.Add(dr1);
                    }
                }
                if (dtm.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dtm.TableName = "Prepcur";
                    dsRep.Tables.Add(dtm);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "time_Tracking_GE", "time_Tracking_GE", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            //========Stock ledger=======
            case "F25244L":  //ledger        
            case "F25244S":  //Summary    
            case "F25244T":  //Summary Main Group
            case "F25244U":  //Summary Sub Main Group
                #region
                dtm = new DataTable();
                dtm.Columns.Add("header", typeof(string));
                dtm.Columns.Add("fromdt", typeof(string));
                dtm.Columns.Add("todt", typeof(string));
                dtm.Columns.Add("MG_CODE", typeof(string));
                dtm.Columns.Add("MG_NAME", typeof(string));
                dtm.Columns.Add("SUBG_CODE", typeof(string));
                dtm.Columns.Add("SUBG_NAME", typeof(string));
                dtm.Columns.Add("icode", typeof(string));
                dtm.Columns.Add("item_name", typeof(string));
                dtm.Columns.Add("cpartno", typeof(string));
                dtm.Columns.Add("unit", typeof(string));
                dtm.Columns.Add("op_bal_Qty", typeof(double));
                dtm.Columns.Add("op_Value", typeof(double));
                dtm.Columns.Add("Inward_Qty", typeof(double));
                dtm.Columns.Add("Inward_Value", typeof(double));
                dtm.Columns.Add("job_work_mtl_val", typeof(double));
                dtm.Columns.Add("outward_Qty", typeof(double));
                dtm.Columns.Add("outward_Val", typeof(double));
                dtm.Columns.Add("Consumption_Qty", typeof(double));
                dtm.Columns.Add("Consumption_Val", typeof(double));
                dtm.Columns.Add("Clos_qty", typeof(double));
                dtm.Columns.Add("Clos_Val", typeof(double));
                dtm.Columns.Add("Avg_rate", typeof(double));

                dtm.Columns.Add("IQTYIN", typeof(double));
                dtm.Columns.Add("IQTYOUT", typeof(double));
                dtm.Columns.Add("BAL", typeof(double));

                dtm.Columns.Add("VTY", typeof(string));
                dtm.Columns.Add("VCHNUM", typeof(string));
                dtm.Columns.Add("VCHDATE", typeof(string));
                dtm.Columns.Add("RATE", typeof(double));

                switch (iconID)
                {
                    case "F25244S":
                        header_n = "Stock Summary Value with weighted average rate";
                        break;
                    case "F25244L":
                        header_n = "Stock Ledger Value with weighted average rate";
                        break;
                    case "F25244U":
                        header_n = "Stock Summary Main Group Value with weighted average rate";
                        break;
                    case "F25244T":
                        header_n = "Stock Summary Sub Group Value with weighted average rate";
                        break;
                }

                string startDt = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT INVN_STDT FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "INVN_STDT");
                if (startDt == "0") startDt = frm_cDt1;

                if (Convert.ToDateTime(fromdt) > Convert.ToDateTime(startDt))
                    startDt = fromdt;

                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) cond = " and trim(a.icode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "'";
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4").Length > 1) cond = " and trim(a.icode) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR4") + "'";

                DataTable dtItems = new DataTable();
                dtItems = fgen.getdata(frm_qstr, frm_cocd, "Select a.icode,a.iname,a.unit,a.irate from item a where length(trim(a.icode))>4 " + cond + "");

                dt = new DataTable();
                SQuery = "Select a.type,a.vchnum,a.vchdate,a.icode,nvl(a.iqtyin,0) as iqtyin,nvl(a.iqtyout,0) as iqtyout,nvl(a.iqtyin,0)-nvl(a.iqtyout,0) as BAL,((nvl(a.iqtyin,0)-nvl(a.iqtyout,0)) *nvl(a.ichgs,0)) as bal_val,nvl(a.ichgs,0) as ichgs,nvl(a.iamount,0) as iamount,nvl(a.rlprc,0) as rlprc,a.invno,a.invdate,a.acode from ivoucher a where a.branchcd='" + frm_mbr + "' and a.vchdate>=to_DatE('" + frm_cDt1 + "','dd/mm/yyyy') and a.vchdate<=to_DatE('" + todt + "','dd/mm/yyyy') and a.store='Y' " + cond + " order by a.icode,a.vchdate,a.type,a.vchnum";//trim(icode)='02020001' and
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                dt1 = new DataTable();
                mq0 = "Select a.icode,nvl(b.YR_" + frm_myear + ",0) as opbal,NVL(a.irate,0) AS IRATE,a.iname as item_name,trim(a.cpartno) as partno,trim(a.unit) as unit ,substr(trim(a.icode),1,2) as mgcode,trim(c.name) as mg_name,X.ICODE AS SUBG_CODE,X.INAME AS SUBG_NAME from item a left join itembal b on trim(A.icode)=trim(B.icode)   and b.branchcd='" + frm_mbr + "' and b.YR_" + frm_myear + " <>0 ,type c,ITEM X where  substr(trim(a.icode),1,2)=trim(c.type1) and c.id='Y' AND length(trim(a.icode))>=8  AND SUBSTR(A.ICODE,1,4)=TRIM(X.ICODE) AND LENGTH(TRIM(X.ICODE))=4 " + cond + " order by a.icode";//trim(b.icode)='02020001' and
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                string type = "";
                string opDt = startDt;
                string mcode = "";
                string oldIcode = "";
                double ipq = 0, ipv = 0, sbq = 0, SBV = 0, upq = 0, upv = 0, opq = 0, opv = 0, cloq = 0, clov = 0, avgrate = 0, my_op_rt = 0;
                string mgcode = "", MG_NAME = "", subgName = "", subgCode = "", icodee = "", item_name = "", cpartno = "", unit = "";
                double irate = 0, rep_amt = 0, ext_amt = 0, op_bal_qty = 0, op_value = 0;
                string hasDtl = "N";
                string lastIcode = "";
                double closQty = 0, closValue = 0;
                double lastAvgPrice = 0;
                i = 0;
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        mcode = dt.Rows[i]["ICODE"].ToString().Trim();
                        irate = fgen.make_double(fgen.seek_iname_dt(dt1, "trim(icode)='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "IRATE"));
                        hasDtl = "N";
                        if (oldIcode == mcode)
                        {
                            op_bal_qty = 0;
                            op_value = 0;
                            hasDtl = "Y";
                        }
                        else
                        {
                            mgcode = fgen.seek_iname_dt(dt1, "trim(icode)='" + mcode + "'", "mgcode");
                            MG_NAME = fgen.seek_iname_dt(dt1, "trim(icode)='" + mcode + "'", "mg_name");

                            subgCode = fgen.seek_iname_dt(dt1, "trim(icode)='" + mcode + "'", "subg_CODE");
                            subgName = fgen.seek_iname_dt(dt1, "trim(icode)='" + mcode + "'", "subg_name");

                            cpartno = fgen.seek_iname_dt(dt1, "trim(icode)='" + mcode + "'", "partno");
                            unit = fgen.seek_iname_dt(dt1, "trim(icode)='" + mcode + "'", "unit");
                            item_name = fgen.seek_iname_dt(dt1, "trim(icode)='" + mcode + "'", "item_name");
                        }
                        if (hasDtl == "N")
                        {
                            op_bal_qty = fgen.make_double(fgen.seek_iname_dt(dt1, "trim(icode)='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "opbal"));
                            op_value = op_bal_qty * irate;

                            if (Convert.ToDateTime(fromdt) > Convert.ToDateTime(startDt))
                                opDt = fromdt;

                            op_bal_qty += dt.Compute("SUM(BAL)", "icode='" + mcode + "' AND VCHDATE<'" + Convert.ToDateTime(opDt) + "' ").ToString().toDouble();
                            if (dt.Compute("SUM(BAL)", "icode='" + mcode + "' AND VCHDATE<'" + Convert.ToDateTime(opDt) + "' ").ToString().toDouble() > 0)
                                op_value += dt.Compute("SUM(bal_val)", "icode='" + mcode + "' AND VCHDATE<'" + Convert.ToDateTime(opDt) + "' ").ToString().toDouble();
                            if (op_value == 0 && op_bal_qty > 0)
                                op_value = op_bal_qty * irate;

                            //if (opDt != startDt)
                            {
                                if (op_value > 0 && op_bal_qty > 0)
                                    irate = op_value / op_bal_qty;
                            }
                        }

                        oldIcode = mcode;
                        type = dt.Rows[i]["type"].ToString().Trim();
                        //if (Convert.ToDateTime(dt.Rows[i]["VCHDATE"].ToString().Trim()) < Convert.ToDateTime(startDt))
                        //{
                        //    rep_amt = (dt.Rows[i]["IQTYIN"].ToString().Trim().toDouble() - dt.Rows[i]["IQTYOUT"].ToString().Trim().toDouble()) * fgen.seek_iname_dt(dtItems, "ICODE='" + mcode + "'", "IRATE").toDouble();
                        //}
                        //else
                        {
                            switch (type.Left(1))
                            {
                                case "0":
                                    rep_amt = fgen.make_double(dt.Rows[i]["IQTYIN"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["ichgs"].ToString().Trim());
                                    ext_amt = fgen.make_double(dt.Rows[i]["IQTYIN"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["RLPRC"].ToString().Trim());
                                    break;
                                case "1":
                                    rep_amt = fgen.make_double(dt.Rows[i]["IQTYIN"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["ichgs"].ToString().Trim());
                                    break;
                                case "2":
                                    rep_amt = fgen.make_double(dt.Rows[i]["iqtyout"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["ichgs"].ToString().Trim());
                                    break;
                                case "3":
                                    if (fgen.make_double(dt.Rows[i]["iqtyin"].ToString().Trim()) > 0)
                                    {
                                        rep_amt = fgen.make_double(dt.Rows[i]["IQTYIN"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["ichgs"].ToString().Trim());
                                    }
                                    else
                                    {
                                        rep_amt = (fgen.make_double(dt.Rows[i]["iqtyout"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["ichgs"].ToString().Trim())) * -1;
                                    }
                                    break;
                                case "4":
                                    rep_amt = (fgen.make_double(dt.Rows[i]["iqtyout"].ToString().Trim()) * fgen.make_double(dt.Rows[i]["rlprc"].ToString().Trim()));
                                    break;
                                default:

                                    break;
                            }
                        }

                        if (hasDtl == "N" && op_bal_qty != 0)
                        {
                            dr1 = dtm.NewRow();
                            dr1["header"] = header_n;
                            dr1["fromdt"] = fromdt;
                            dr1["todt"] = todt;

                            dr1["MG_CODE"] = mgcode;
                            dr1["MG_NAME"] = MG_NAME;

                            dr1["subg_CODE"] = subgCode;
                            dr1["subg_NAME"] = subgName;

                            dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                            dr1["item_name"] = item_name;
                            dr1["cpartno"] = cpartno;
                            dr1["unit"] = unit;

                            dr1["op_bal_Qty"] = op_bal_qty;
                            dr1["op_Value"] = (op_bal_qty > 0 ? op_value : 0);
                            dr1["Clos_qty"] = 0;
                            dr1["Clos_Val"] = 0;

                            dr1["Inward_Qty"] = 0;
                            dr1["Inward_Value"] = 0;
                            dr1["job_work_mtl_val"] = 0;
                            dr1["outward_Qty"] = 0;
                            dr1["outward_Val"] = 0;
                            dr1["Consumption_Qty"] = 0;
                            dr1["Consumption_Val"] = 0;

                            dr1["IQTYIN"] = 0;
                            dr1["IQTYOUT"] = 0;
                            dr1["BAL"] = 0;

                            dr1["Avg_rate"] = 0;

                            dr1["VTY"] = "OP";
                            dr1["VCHNUM"] = "Op.Bal";
                            dr1["VCHDATE"] = Convert.ToDateTime(opDt).AddDays(0).ToString("dd/MM/yyy");
                            dr1["RATE"] = irate;
                            dtm.Rows.Add(dr1);

                            hasDtl = "Y";
                        }
                        if (Convert.ToDateTime(dt.Rows[i]["VCHDATE"].ToString().Trim()) < Convert.ToDateTime(startDt))
                        {

                        }
                        else
                        {
                            if (dt.Rows[i]["iqtyIN"].ToString().Trim().toDouble() > 0 || dt.Rows[i]["iqtyout"].ToString().Trim().toDouble() > 0)
                            {
                                dr1 = dtm.NewRow();
                                dr1["header"] = header_n;
                                dr1["fromdt"] = fromdt;
                                dr1["todt"] = todt;

                                dr1["MG_CODE"] = mgcode;
                                dr1["MG_NAME"] = MG_NAME;

                                dr1["subg_CODE"] = subgCode;
                                dr1["subg_NAME"] = subgName;

                                dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                                dr1["item_name"] = item_name;
                                dr1["cpartno"] = cpartno;
                                dr1["unit"] = unit;

                                dr1["op_bal_Qty"] = 0;
                                dr1["op_Value"] = 0;
                                switch (type.Left(1))
                                {
                                    case "0":
                                        dr1["Inward_Qty"] = fgen.make_double(dt.Rows[i]["IQTYIN"].ToString().Trim());
                                        dr1["Inward_Value"] = (dr1["Inward_Qty"].ToString().toDouble() > 0 ? rep_amt : 0);
                                        dr1["job_work_mtl_val"] = 0;
                                        dr1["outward_Qty"] = 0;
                                        dr1["outward_Val"] = 0;
                                        dr1["Consumption_Qty"] = 0;
                                        dr1["Consumption_Val"] = 0;
                                        break;

                                    case "2":
                                    case "4":
                                        dr1["Inward_Qty"] = 0;
                                        dr1["Inward_Value"] = 0;
                                        dr1["job_work_mtl_val"] = 0;
                                        dr1["outward_Qty"] = fgen.make_double(dt.Rows[i]["iqtyout"].ToString().Trim());
                                        dr1["outward_Val"] = rep_amt;
                                        dr1["Consumption_Qty"] = 0;
                                        dr1["Consumption_Val"] = 0;
                                        break;
                                    default:
                                        dr1["Inward_Qty"] = 0;
                                        dr1["Inward_Value"] = 0;
                                        dr1["job_work_mtl_val"] = 0;
                                        dr1["outward_Qty"] = 0;
                                        dr1["outward_Val"] = 0;
                                        dr1["Consumption_Qty"] = fgen.make_double(dt.Rows[i]["iqtyout"].ToString().Trim()) - fgen.make_double(dt.Rows[i]["iqtyin"].ToString().Trim());
                                        dr1["Consumption_Val"] = rep_amt * -1;
                                        break;
                                }
                                dr1["Clos_qty"] = 0;
                                dr1["Clos_Val"] = 0;

                                dr1["IQTYIN"] = dt.Rows[i]["iqtyIN"].ToString().Trim().toDouble();
                                dr1["IQTYOUT"] = dt.Rows[i]["iqtyout"].ToString().Trim().toDouble();
                                dr1["BAL"] = dt.Rows[i]["BAL"].ToString().Trim().toDouble();
                                dr1["Avg_rate"] = ((dr1["Inward_Value"].ToString().toDouble() + dr1["op_Value"].ToString().toDouble() + dr1["outward_Val"].ToString().toDouble() + dr1["Consumption_Val"].ToString().toDouble()) / (fgen.make_double(dt.Rows[i]["iqtyin"].ToString().Trim()) + fgen.make_double(dt.Rows[i]["iqtyout"].ToString().Trim())));

                                dr1["VTY"] = dt.Rows[i]["TYPE"].ToString().Trim();
                                dr1["VCHNUM"] = dt.Rows[i]["VCHNUM"].ToString().Trim();
                                dr1["VCHDATE"] = Convert.ToDateTime(dt.Rows[i]["VCHDATE"].ToString().Trim()).ToString("dd/MM/yyy");
                                dr1["RATE"] = (type.Left(1) == "4" ? dt.Rows[i]["rlprc"].ToString().Trim() : dt.Rows[i]["ichgs"].ToString().Trim());
                                dtm.Rows.Add(dr1);
                            }
                        }
                    }
                    if (iconID == "F25244L")
                    {
                        if (dtm.Rows.Count > 0)
                        {
                            DataView dvsor = new DataView(dtm);
                            dvsor.Sort = "ICODE";
                            closQty = 0; closValue = 0;
                            mcode = "";
                            lastIcode = "";
                            lastAvgPrice = 0;
                            for (i = 0; i < dtm.Rows.Count; i++)
                            {
                                if (mcode != dtm.Rows[i]["ICODE"].ToString().Trim())
                                {
                                    closQty = fgen.make_double(dtm.Rows[i]["op_bal_Qty"].ToString().Trim()) + fgen.make_double(dtm.Rows[i]["Inward_Qty"].ToString().Trim()) - fgen.make_double(dtm.Rows[i]["outward_Qty"].ToString().Trim()) - fgen.make_double(dtm.Rows[i]["Consumption_Qty"].ToString().Trim());
                                    closValue = fgen.make_double(dtm.Rows[i]["op_Value"].ToString().Trim()) + fgen.make_double(dtm.Rows[i]["Inward_Value"].ToString().Trim()) - fgen.make_double(dtm.Rows[i]["outward_Val"].ToString().Trim()) - fgen.make_double(dtm.Rows[i]["Consumption_Val"].ToString().Trim());
                                    dtm.Rows[i]["clos_qty"] = closQty;
                                    if (closQty > 0)
                                        dtm.Rows[i]["Clos_Val"] = closValue;
                                }
                                else
                                {
                                    dtm.Rows[i]["clos_qty"] = (dtm.Rows[i - 1]["clos_qty"].ToString().toDouble() + dtm.Rows[i]["Inward_Qty"].ToString().Trim().toDouble()) - (fgen.make_double(dtm.Rows[i]["outward_Qty"].ToString().Trim()) + fgen.make_double(dtm.Rows[i]["Consumption_Qty"].ToString().Trim()));
                                    if (dtm.Rows[i]["clos_qty"].ToString().toDouble() != 0)
                                        dtm.Rows[i]["Clos_Val"] = (dtm.Rows[i - 1]["Clos_Val"].ToString().toDouble()) + fgen.make_double(dtm.Rows[i]["Inward_Value"].ToString().Trim()) - fgen.make_double(dtm.Rows[i]["outward_Val"].ToString().Trim()) - fgen.make_double(dtm.Rows[i]["Consumption_Val"].ToString().Trim());
                                }

                                {
                                    if (dtm.Rows[i]["Clos_Val"].ToString().toDouble() != 0)
                                        dtm.Rows[i]["Avg_rate"] = dtm.Rows[i]["Clos_Val"].ToString().toDouble() / dtm.Rows[i]["clos_qty"].ToString().toDouble();
                                    else
                                        dtm.Rows[i]["Avg_rate"] = (dtm.Rows[i]["op_Value"].ToString().Trim().toDouble() + dtm.Rows[i]["Inward_Value"].ToString().Trim().toDouble() - dtm.Rows[i]["outward_Val"].ToString().Trim().toDouble() - dtm.Rows[i]["Consumption_Val"].ToString().Trim().toDouble()) / (dtm.Rows[i]["op_bal_Qty"].ToString().Trim().toDouble() + dtm.Rows[i]["Inward_Qty"].ToString().Trim().toDouble() - dtm.Rows[i]["outward_Qty"].ToString().Trim().toDouble() - dtm.Rows[i]["Consumption_Qty"].ToString().Trim().toDouble());
                                    //else dtm.Rows[i]["Avg_rate"] = 0;
                                    lastIcode = dtm.Rows[i]["ICODE"].ToString().Trim();
                                }
                                mcode = dtm.Rows[i]["ICODE"].ToString().Trim();

                            }
                            dsRep = new DataSet();
                            dtm.TableName = "Prepcur";
                            dsRep.Tables.Add(dtm);
                            //pdfView = "N";
                            Print_Report_BYDS(frm_cocd, frm_mbr, "stk_Report_legr", "stk_Report_legr_Detailed", dsRep, header_n);
                        }
                    }
                    else
                    {
                        dt2 = new DataTable();
                        dt3 = new DataTable();
                        dt3 = dtm.Clone();
                        DataTable myNewtable = dtm;
                        DataTable dtDistinctRow = new DataTable();
                        DataView dv = new DataView(myNewtable, "", "", DataViewRowState.CurrentRows);
                        dtDistinctRow = dv.ToTable(true, "icode");

                        foreach (DataRow drdistinct in dtDistinctRow.Rows)
                        {
                            ipq = 0; ipv = 0; sbq = 0; SBV = 0; upq = 0; upv = 0; opq = 0; opv = 0; cloq = 0; clov = 0; avgrate = 0; my_op_rt = 0;
                            mgcode = ""; MG_NAME = ""; icodee = ""; item_name = ""; cpartno = ""; unit = "";
                            hasDtl = "N";
                            my_op_rt = fgen.seek_iname_dt(dtItems, "ICODE='" + drdistinct["icode"].ToString().Trim() + "'", "irate").toDouble();
                            dv = new DataView(myNewtable, "icode='" + drdistinct["icode"].ToString() + "'", "", DataViewRowState.CurrentRows);
                            dt2 = dv.ToTable();
                            for (i = 0; i < dt2.Rows.Count; i++)
                            {
                                mgcode = dt2.Rows[i]["MG_CODE"].ToString().Trim();
                                MG_NAME = dt2.Rows[i]["MG_NAME"].ToString().Trim();
                                icodee = dt2.Rows[i]["icode"].ToString().Trim();
                                mcode = dt2.Rows[i]["icode"].ToString().Trim();
                                item_name = dt2.Rows[i]["item_name"].ToString().Trim();
                                cpartno = dt2.Rows[i]["cpartno"].ToString().Trim();
                                unit = dt2.Rows[i]["unit"].ToString().Trim();

                                subgCode = dt2.Rows[i]["subg_CODE"].ToString().Trim();
                                subgName = dt2.Rows[i]["subg_name"].ToString().Trim();

                                if (Convert.ToDateTime(dt.Rows[i]["VCHDATE"].ToString().Trim()) < Convert.ToDateTime(startDt))
                                {
                                    if (hasDtl == "N")
                                    {
                                        if (Convert.ToDateTime(fromdt) > Convert.ToDateTime(startDt))
                                            opDt = fromdt;
                                        opq += dt.Compute("SUM(BAL)", "icode='" + mcode + "' AND VCHDATE<'" + Convert.ToDateTime(opDt) + "' ").ToString().toDouble();
                                        opv += dt.Compute("SUM(bal_val)", "icode='" + mcode + "' AND VCHDATE<'" + Convert.ToDateTime(opDt) + "' ").ToString().toDouble();
                                        if (opv == 0 && op_bal_qty > 0)
                                            opv = opq * my_op_rt;

                                        hasDtl = "Y";
                                    }
                                }

                                {

                                    ipq = ipq + fgen.make_double(dt2.Rows[i]["Inward_Qty"].ToString().Trim());
                                    ipv = ipv + fgen.make_double(dt2.Rows[i]["Inward_Value"].ToString().Trim());
                                    sbq = sbq + fgen.make_double(dt2.Rows[i]["outward_Qty"].ToString().Trim());
                                    SBV = SBV + fgen.make_double(dt2.Rows[i]["outward_Val"].ToString().Trim());
                                    upq = upq + fgen.make_double(dt2.Rows[i]["Consumption_Qty"].ToString().Trim());
                                    upv = upv + fgen.make_double(dt2.Rows[i]["Consumption_Val"].ToString().Trim());
                                    cloq = opq + ipq - sbq - upq;
                                    clov = opv + ipv - SBV - upv;
                                    avgrate = fgen.make_double(dt2.Rows[i]["Avg_rate"].ToString().Trim());
                                }
                            }
                            dr1 = dt3.NewRow();
                            dr1["header"] = header_n;
                            dr1["fromdt"] = fromdt;
                            dr1["todt"] = todt;
                            dr1["MG_CODE"] = mgcode;
                            dr1["MG_NAME"] = MG_NAME;

                            dr1["SUBG_CODE"] = subgCode;
                            dr1["SUBG_name"] = subgName;

                            dr1["icode"] = icodee;
                            dr1["item_name"] = item_name;
                            dr1["cpartno"] = cpartno;
                            dr1["unit"] = unit;
                            dr1["op_bal_Qty"] = opq;
                            dr1["op_Value"] = opv;
                            dr1["Inward_Qty"] = ipq;
                            dr1["Inward_Value"] = ipv;
                            dr1["job_work_mtl_val"] = 0;
                            dr1["outward_Qty"] = sbq;
                            dr1["outward_Val"] = SBV;
                            dr1["Consumption_Qty"] = upq;
                            dr1["Consumption_Val"] = upv;
                            dr1["Clos_qty"] = cloq;
                            dr1["Clos_Val"] = clov;
                            if ((opq + ipq - sbq - upq) != 0 && (opv + ipv - SBV - upv) != 0)
                                dr1["Avg_rate"] = (opv + ipv - SBV - upv) / (opq + ipq - sbq - upq);
                            else dr1["Avg_rate"] = 0;
                            dt3.Rows.Add(dr1);
                        }

                        if (dt3.Rows.Count > 0)
                        {
                            if (iconID == "F25244T" || iconID == "F25244U")
                            {
                                mgcode = "SUBG_CODE";
                                MG_NAME = "SUBG_NAME";
                                frm_rptName = "stk_Report_val_summr_subg";
                                if (iconID == "F25244T")
                                {
                                    mgcode = "MG_CODE";
                                    MG_NAME = "MG_NAME";
                                    frm_rptName = "stk_Report_val_summr_mg";
                                }

                                DataView dvSu = new DataView(dt3);
                                DataTable dtx = dvSu.ToTable(true, "MG_CODE", "MG_NAME", mgcode, MG_NAME);
                                DataTable dtSumm = dt3.Clone();
                                DataRow drSumm = null;
                                foreach (DataRow drx in dtx.Rows)
                                {
                                    drSumm = dtSumm.NewRow();
                                    drSumm["header"] = header_n;
                                    drSumm["fromdt"] = fromdt;
                                    drSumm["todt"] = todt;
                                    drSumm["MG_CODE"] = drx["MG_CODE"].ToString();
                                    drSumm["MG_NAME"] = drx["MG_NAME"].ToString();

                                    drSumm["SUBG_CODE"] = drx[mgcode].ToString();
                                    drSumm["SUBG_name"] = drx[MG_NAME].ToString();

                                    drSumm["icode"] = drx[mgcode].ToString();
                                    drSumm["item_name"] = drx[MG_NAME].ToString();
                                    drSumm["cpartno"] = "";
                                    drSumm["unit"] = "";
                                    drSumm["op_bal_Qty"] = dt3.Compute("sum(op_bal_Qty)", "" + mgcode + "='" + drx[mgcode].ToString() + "'");
                                    drSumm["op_Value"] = dt3.Compute("sum(op_Value)", "" + mgcode + "='" + drx[mgcode].ToString() + "'");
                                    drSumm["Inward_Qty"] = dt3.Compute("sum(Inward_Qty)", "" + mgcode + "='" + drx[mgcode].ToString() + "'");
                                    drSumm["Inward_Value"] = dt3.Compute("sum(Inward_Value)", "" + mgcode + "='" + drx[mgcode].ToString() + "'");
                                    drSumm["job_work_mtl_val"] = 0;
                                    drSumm["outward_Qty"] = dt3.Compute("sum(outward_Qty)", "" + mgcode + "='" + drx[mgcode].ToString() + "'");
                                    drSumm["outward_Val"] = dt3.Compute("sum(outward_Val)", "" + mgcode + "='" + drx[mgcode].ToString() + "'");
                                    drSumm["Consumption_Qty"] = dt3.Compute("sum(Consumption_Qty)", "" + mgcode + "='" + drx[mgcode].ToString() + "'");
                                    drSumm["Consumption_Val"] = dt3.Compute("sum(Consumption_Val)", "" + mgcode + "='" + drx[mgcode].ToString() + "'");
                                    drSumm["Clos_qty"] = drSumm["op_bal_Qty"].ToString().toDouble() + drSumm["Inward_Qty"].ToString().toDouble() - drSumm["outward_Qty"].ToString().toDouble() - drSumm["Consumption_Qty"].ToString().toDouble();
                                    drSumm["Clos_Val"] = drSumm["op_Value"].ToString().toDouble() + drSumm["Inward_Value"].ToString().toDouble() - drSumm["outward_Val"].ToString().toDouble() - drSumm["Consumption_Val"].ToString().toDouble();
                                    drSumm["Avg_rate"] = 0;
                                    dtSumm.Rows.Add(drSumm);
                                }

                                dsRep = new DataSet();
                                dtSumm.TableName = "Prepcur";
                                dsRep.Tables.Add(dtSumm);
                                //pdfView = "N";
                                Print_Report_BYDS(frm_cocd, frm_mbr, "stk_Report_legr", frm_rptName, dsRep, header_n);
                            }
                            else
                            {
                                dsRep = new DataSet();
                                dt3.TableName = "Prepcur";
                                dsRep.Tables.Add(dt3);
                                //pdfView = "N";
                                Print_Report_BYDS(frm_cocd, frm_mbr, "stk_Report_legr", "stk_Report_val_summr", dsRep, header_n);
                            }
                        }
                    }
                }

                //stk_Report
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
            pdfView = "N";
            if (pdfView == "Y")
                conv_pdf(data_set, rptfile);
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
        string addTypeFile = "Y";
        if (data_set.Tables.Count > 0)
        {
            for (int k = 0; k < data_set.Tables.Count; k++)
            {
                if (data_set.Tables[k].TableName.ToUpper() == "TYPE") addTypeFile = "N";
            }
        }
        if (addTypeFile == "Y")
        {
            if (addlogo == "Y") data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr, "Y"));
            else data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, co_Cd, mbr));
        }
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
            pdfView = "N";
            if (pdfView == "Y" || frm_cocd == "MULT")
                conv_pdf(data_set, rptfile);
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
        repDoc.Load(repFilePath);
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

            CrystalReportViewer1.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
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
            repDoc.Close();
            repDoc.Dispose();
        }
    }

    public void conv_pdf(DataSet dataSet, string rptFile)
    {
        //if (1 == 2)
        {
            if (!repDoc.IsLoaded)
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
            //repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.Excel, Response, true, frm_FileName);
            ExportOptions exportOption = repDoc.ExportOptions;
            {
                exportOption.ExportFormatType = ExportFormatType.Excel;
                exportOption.FormatOptions = new ExcelFormatOptions();
            }
            repDoc.ExportToHttpResponse(exportOption, Response, true, frm_FileName);
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

    protected void btnhide_Click(object sender, EventArgs e)
    {
        DataSet dsRep;
        switch (hfhcid.Value)
        {
            case "F25198A":
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopupmy('#ContentPlaceHolder1_btnhideF_s');", true);                
                dsRep = new DataSet();
                if (Session["data_set"] != null)
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "F25198A5");
                    dsRep = (DataSet)Session["data_set"];
                    Print_Report_BYDS(frm_cocd, frm_mbr, "reel_stk", "reel_stka5", dsRep, "Sticker", "Y");
                }
                break;
            case "F25198A5":
                dsRep = new DataSet();
                if (Session["data_set"] != null)
                {
                    fgenMV.Fn_Set_Mvar(frm_qstr, "REPID", "-");
                    dsRep = (DataSet)Session["data_set"];
                    Print_Report_BYDS(frm_cocd, frm_mbr, "reel_stk", "reel_stka4", dsRep, "Sticker", "Y");
                }
                else ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup();", true);
                break;
            default:
                ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup();", true);
                break;
        }
    }
    protected void btnPurExcel_Click(object sender, ImageClickEventArgs e)
    {
        DataSet ds = (DataSet)Session["data_set"];
        if (ds.Tables[0].Rows.Count > 0)
        {
            frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            fgen.exp_to_excel(ds.Tables[0], "ms-excel", "xls", frm_FileName);
        }
    }
}