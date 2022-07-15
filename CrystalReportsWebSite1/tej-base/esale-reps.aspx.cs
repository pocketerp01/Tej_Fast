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

public partial class esale_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, pdfView = "", fromdt, todt, data_found = "", header_n, party_cd, part_cd, cond1;
    string mq0 = "", mq1 = "", mq2 = "", mq3 = "", cond = "", val = "", mq4 = "", mq5 = "", mq6 = "", mq7 = "", mq8 = "", mq9 = "", mq10 = "", mq11 = "", yr = "";
    fgenDB fgen = new fgenDB();
    double db1, db2, db3, db4, db5;
    int i0 = 0;
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
                    xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    pdfView = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PDFVIEW");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", "-");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
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
        string mq10, mq1, mq0, mq2, mq3, mq4;
        int repCount = 1;
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        data_found = "Y";
        frm_formID = iconID;
        switch (iconID)
        {
            case "F50101":
            case "F1006":
            case "F1006A":
            case "F55101":
            case "F55106":
                {
                    string yrst = "";
                    yrst = frm_cDt1.Substring(8, 2) + "-" + frm_cDt2.Substring(8, 2);
                    mq2 = barCode;
                    SQuery = "select distinct '             -            ' as busi_expect, A.MORDER,'EXP'||'/'||a.vchnum||'/'||'" + yrst + "' as EXP_YR, 'N' as logo_yn,C.CURREN,C.THRU,a.BRANCHCD||a.TYPE||TRIM(a.ponum)||TO_CHAr(a.podate,'DDMMYYYY') AS busiexpect, (case when c.cscode='-' then 'Same as Consignee' else c1.aname end) as consign,c1.addr1 as daddr1,c1.addr2 as daddr2,c1.addr3 as daddr3,c1.addr4 as daddr4,c1.telnum as dtel, c1.rc_num as dtinno,c1.exc_num as dcstno,c1.acode as mycode,c1.staten as dstaten,c1.gst_no as dgst_no,c1.girno as dpanno,substr(c1.gst_no,0,2) as dstatecode,  a.iweight,b.payment,nvl(H1.naration,'-') as grosswt,t2.bankname,t2.bankaddr,t2.vat_form as swiftcode,t2.bankac as ac, a.branchcd,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,is_number(a.no_bdls) as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,nvl(a.st_modv,0) as cash_disc,nvl(a.st_nmodv,0) as oth_disc, h1.invno AS Hinvno,TO_CHAR(h1.invdate,'DD/MM/YYYY') AS Hinvdate,h1.lbnetwt,h1.REMARK3 AS NETWT,h1.lbgrswt,h1.exprmk1,h1.exprmk2,h1.exprmk3,h1.exprmk4,h1.exprmk5,h1.TMADDL1,h1.TMADDL2,a.iqty_chlwt from ivoucher a left outer join hundi h1 on trim(a.branchcd)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(h1.branchcd)||trim(h1.acode)||trim(h1.invno)||to_char(h1.invdate,'dd/mm/yyyy') ,sale c left outer join csmst c1 on trim(c.cscode)=trim(c1.acode),item d,type t1,TYPE t2,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and trim(a.branchcd)=trim(t2.type1) and t2.id='B' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND trim(a.branchcd)='" + frm_mbr + "' and a.type='4F' AND TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + mq2 + ") order by vchdate,a.vchnum,a.MORDER";
                    if (frm_formID == "F55106")
                        SQuery = "select distinct '             -            ' as busi_expect, A.MORDER,'EXP'||'/'||a.vchnum||'/'||'" + yrst + "' as EXP_YR, 'N' as logo_yn,C.CURREN,C.THRU,a.BRANCHCD||a.TYPE||TRIM(a.ponum)||TO_CHAr(a.podate,'DDMMYYYY') AS busiexpect, (case when c.cscode='-' then 'Same as Consignee' else c1.aname end) as consign,c1.addr1 as daddr1,c1.addr2 as daddr2,c1.addr3 as daddr3,c1.addr4 as daddr4,c1.telnum as dtel, c1.rc_num as dtinno,c1.exc_num as dcstno,c1.acode as mycode,c1.staten as dstaten,c1.gst_no as dgst_no,c1.girno as dpanno,substr(c1.gst_no,0,2) as dstatecode,  a.iweight,b.payment,nvl(H1.naration,'-') as grosswt,t2.bankname,t2.bankaddr,t2.vat_form as swiftcode,t2.bankac as ac, a.branchcd,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,is_number(a.no_bdls) as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,0 as cash_disc,0 as oth_disc, h1.invno AS Hinvno,TO_CHAR(h1.invdate,'DD/MM/YYYY') AS Hinvdate,h1.lbnetwt,h1.REMARK3 AS NETWT,h1.lbgrswt,h1.exprmk1,h1.exprmk2,h1.exprmk3,h1.exprmk4,h1.exprmk5,h1.TMADDL1,h1.TMADDL2,a.iqty_chlwt from ivoucherp a left outer join hundip h1 on trim(a.branchcd)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(h1.branchcd)||trim(h1.acode)||trim(h1.invno)||to_char(h1.invdate,'dd/mm/yyyy') ,salep c left outer join csmst c1 on trim(c.cscode)=trim(c1.acode),item d,type t1,TYPE t2,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and trim(a.branchcd)=trim(t2.type1) and t2.id='B' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND trim(a.branchcd)='" + frm_mbr + "' and a.type='4F' AND TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + mq2 + ") order by vchdate,a.vchnum,a.MORDER";
                    dt = new DataTable(); dt1 = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    foreach (DataColumn dc in dt.Columns)
                    {
                        dc.ReadOnly = false;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        DataView view1im = new DataView(dt);
                        DataTable dtdrsim = new DataTable();
                        dtdrsim = view1im.ToTable(true, "branchcd", "ordno", "orddt", "acode"); //MAIN    
                        int m = 0; dt6 = new DataTable();
                        foreach (DataRow dr0 in dtdrsim.Rows)
                        {
                            DataView view2 = new DataView(dt, "branchcd='" + dr0["branchcd"].ToString().Trim() + "' AND ordno='" + dr0["ordno"].ToString().Trim() + "' AND orddt='" + dr0["orddt"].ToString().Trim() + "' AND acode='" + dr0["acode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            DataTable dticode = new DataTable();
                            dticode = view2.ToTable();
                            for (int i = 0; i < dticode.Rows.Count; i++)
                            {
                                mq1 = "";
                                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "select  busi_expect from somas where branchcd||type||ordno||to_char(orddt,'dd/MM/yyyy')='" + dticode.Rows[i]["busiexpect"].ToString().Trim() + "'", "busi_expect");
                                if (mq1.Length > 2) { }
                                else { mq1 = "-"; }
                                dt.Rows[i]["busi_expect"] = mq1;
                            }
                        }
                        dt.TableName = "Prepcur";
                        dsRep.Tables.Add(dt);


                        SQuery = "SELECT DISTINCT udf_name||' '||udf_value AS POTERMS,SRNO FROM udf_data WHERE BRANCHCD='" + frm_mbr + "' AND PAR_FLD='" + frm_mbr + frm_vty + barCode.Replace("'", "") + "' ORDER BY SRNO";
                        dt = new DataTable();
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                        mq10 = "";
                        dt1 = new DataTable();
                        mdr = null;
                        dt1.Columns.Add("poterms", typeof(string));
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            mq10 += dt.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
                        }
                        mdr = dt1.NewRow();
                        mdr["poterms"] = mq10;
                        dt1.Rows.Add(mdr);

                        if (dt1.Rows.Count > 0)
                        {
                            dt1.TableName = "INV_TERMS";
                            dsRep.Tables.Add(dt1);
                        }


                        frm_rptName = "ExpInv_STUD";
                        if (frm_formID == "F55106") frm_rptName = "ExpProfInv";

                        if (dsRep.Tables[0].Rows.Count > 0)
                        {
                            Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, header_n);
                        }
                    }
                }
                break;

            case "F55141":
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                mq0 = "select trim( to_char(to_date('" + fromdt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + fromdt.Substring(6, 4) + "','yyyy'),'yyyy')) as FRMDATE  from dual";
                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "FRMDATE");
                mq2 = "select trim( to_char(to_date('" + todt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + todt.Substring(6, 4) + "','yyyy'),'yyyy')) as TODATE  from dual";
                mq3 = fgen.seek_iname(frm_qstr, frm_cocd, mq2, "TODATE");
                SQuery = "select DISTINCT  '" + mq1 + "' as FRMDATE,'" + mq3 + "' AS TODATE,'" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,to_char(a.vchdate,'dd/mm/yyyy') as vch,to_char(a.podate,'dd/mm/yyyy') as podt, a.*, TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum)||TRIM(A.TYPE) AS GRP ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,c.iname,c.cpartno  from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type='" + frm_vty + "' AND A.vchdate " + xprdRange + "  ORDER BY A.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep = new DataSet();
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "STD_ESALE_REG", "std_Sale_REG", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F55142":
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                sname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq0 = "select trim( to_char(to_date('" + fromdt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + fromdt.Substring(6, 4) + "','yyyy'),'yyyy')) as FRMDATE  from dual";
                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "FRMDATE");
                mq2 = "select trim( to_char(to_date('" + todt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + todt.Substring(6, 4) + "','yyyy'),'yyyy')) as TODATE  from dual";
                mq3 = fgen.seek_iname(frm_qstr, frm_cocd, mq2, "TODATE");
                if (sname.Length > 0)
                {
                    SQuery = "select DISTINCT  '" + mq1 + "' as FRMDATE,'" + mq3 + "' AS TODATE,'" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,to_char(a.vchdate,'dd/mm/yyyy') as vch,to_char(a.podate,'dd/mm/yyyy') as podt, a.*, TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum)||TRIM(A.TYPE) AS GRP ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,c.iname,c.cpartno  from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type='" + frm_vty + "' AND A.vchdate " + xprdRange + "AND A.ACODE in (" + sname + ")  ORDER BY A.SRNO";
                }
                else
                {
                    SQuery = "select DISTINCT  '" + mq1 + "' as FRMDATE,'" + mq3 + "' AS TODATE,'" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,to_char(a.vchdate,'dd/mm/yyyy') as vch,to_char(a.podate,'dd/mm/yyyy') as podt, a.*, TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum)||TRIM(A.TYPE) AS GRP ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,c.iname,c.cpartno  from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type='" + frm_vty + "' AND A.vchdate " + xprdRange + "AND A.ACODE like '%'  ORDER BY A.SRNO";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep = new DataSet();
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "STD_ESALE_REG", "std_Sale_REG", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F55143":
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                sname = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq0 = "select trim( to_char(to_date('" + fromdt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + fromdt.Substring(6, 4) + "','yyyy'),'yyyy')) as FRMDATE  from dual";
                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "FRMDATE");
                mq2 = "select trim( to_char(to_date('" + todt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + todt.Substring(6, 4) + "','yyyy'),'yyyy')) as TODATE  from dual";
                mq3 = fgen.seek_iname(frm_qstr, frm_cocd, mq2, "TODATE");
                if (sname.Length > 0)
                {
                    SQuery = "select DISTINCT  '" + mq1 + "' as FRMDATE,'" + mq3 + "' AS TODATE,'" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,to_char(a.vchdate,'dd/mm/yyyy') as vch,to_char(a.podate,'dd/mm/yyyy') as podt, a.*, TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum)||TRIM(A.TYPE) AS GRP ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,c.iname,c.cpartno  from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type='" + frm_vty + "' AND A.vchdate " + xprdRange + "AND A.ICODE in (" + sname + ")  ORDER BY A.SRNO";
                }
                else
                {
                    SQuery = "select DISTINCT  '" + mq1 + "' as FRMDATE,'" + mq3 + "' AS TODATE,'" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,to_char(a.vchdate,'dd/mm/yyyy') as vch,to_char(a.podate,'dd/mm/yyyy') as podt, a.*, TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum)||TRIM(A.TYPE) AS GRP ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,c.iname,c.cpartno  from ivoucher a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type='" + frm_vty + "' AND A.vchdate " + xprdRange + "AND A.ICODE like '%'  ORDER BY A.SRNO";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep = new DataSet();
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "STD_ESALE_REG", "std_Sale_REG", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F55145":
                header_n = "EXPORT INVOICE";
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "select a.vchnum as invno,TO_CHAR(a.vchdate,'DD/MM/YYYY') AS INVDATE,A.ACODE AS ACODE,A.ICODE,b.aname,c.iname,a.iqtyout as qty,a.iQTY_CHLWT as rate,a.iamount,trim(c.cpartno) as partno,b.payterm,b.payment,A.FaBTYPE,a.finvno,a.btchno,a.iweight from ivoucher a , famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||TRIM(A.vchnum)||TO_CHAR(A.vchDATE,'DD/MM/YYYY') = '" + mq2 + "'";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                SQuery = "SELECT ANAME AS consign ,ADDR1 as daddr1,ADDR2 as daddr2,ADDR3 as daddr3,ADDR4 as daddr4,email,telnum,fax,'-' as dcstno,acode as mycode,staten as dstaten,gst_no as dgst_no,girno as dpanno,substr(gst_no,0,2) as dstatecode FROM FAMST WHERE ACODE='" + dt.Rows[0]["ACODE"].ToString().Trim() + "'";
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);

                    dt1.TableName = "csmst";
                    dsRep.Tables.Add(dt1);

                    SQuery = "select invno AS Hinvno,TO_CHAR(invdate,'DD/MM/YYYY') AS Hinvdate,ship2,ship3,ship4,ship5,lbnetwt,lbgrswt,exprmk1,exprmk2,exprmk3,exprmk4,exprmk5,addl1,addl2,addl3, addl4, addl5,tmaddl1,tmaddl2,tmaddl3,addl6 from hundi where branchcd='" + frm_mbr + "' and type='IV' and acode='" + dt.Rows[0]["ACODE"].ToString().Trim() + "' and trim(invno)||to_char(invdate,'dd/mm/yyyy')='" + mq2.Substring(4, 16) + "'";
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    SQuery = "select pono,mo_vehi,no_bdls,stform_no,ins_co,curren,cscode,acode from sale where branchcd||type||TRIM(vchnum)||TO_CHAR(vchDATE,'DD/MM/YYYY') = '" + mq2 + "'";
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt3.TableName = "sale";
                    dsRep.Tables.Add(dt3);

                    if (dt2.Rows.Count < 1)
                    {
                        SQuery = "select '-' as Hinvno,'-' as Hinvdate,'-' as ship2,'-' as ship3,'-' as ship4,'-' as ship5,'-' as lbnetwt,'-' as lbgrswt,'-' as exprmk1,'-' as exprmk2,'-' as exprmk3,'-' as exprmk4,'-' as exprmk5,'-' as addl1,'-' as addl2,'-' as addl3,'-' as addl4,'-' as addl5,'-' as tmaddl1,'-' as tmaddl2,'-' as tmaddl3,'-' as addl6 from dual";
                        dt2 = new DataTable();
                        dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    }
                    dt2.TableName = "hundi";
                    dsRep.Tables.Add(dt2);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "ExpInv_SAGE", "ExpInv_SAGE", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F55146":
                #region Packing List
                header_n = "Packing List";
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "select '" + header_n + "' as header,b.aname,B.ADDR1,B.ADDR2,b.addr3,b.telnum,b.fax,b.email,a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.acode) as acode,trim(a.icode) as icode,a.srno,is_number(a.col1) as box_no,a.col2,is_number(a.col3) as qty_number,is_number(a.col4) as grs_wt,a.col9,a.col10 from scratch a,FAMST B where TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.BRANCHCD)||trim(a.type)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') ='" + mq2 + "'";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    mq4 = "select distinct finvno from ivoucher where branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and type!='4F' AND vchnum='" + dt.Rows[0]["col9"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + dt.Rows[0]["col10"].ToString().Trim() + "'";
                    mq3 = fgen.seek_iname(frm_qstr, frm_cocd, mq4, "finvno");

                    dt.Columns.Add(new DataColumn("PkgN", typeof(double)));
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    //sale table                  
                    SQuery = "select '" + mq3 + "' as ponodt, acode,vchnum as svch,to_char(vchdate,'dd/mm/yyyy') as svchdt,cscode from sale where branchcd='" + frm_mbr + "' and vchnum='" + dsRep.Tables[0].Rows[0]["col9"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + dsRep.Tables[0].Rows[0]["col10"].ToString().Trim() + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt.TableName = "SALES_P";
                    dsRep.Tables.Add(dt);
                    ///////////					
                    SQuery = "Select distinct d.tdsnum,d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dsRep.Tables[1].Rows[0]["cscode"].ToString().Trim() + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count <= 0)
                    {
                        dt = new DataTable();
                        SQuery = "SELECT ANAME AS consign ,ADDR1 as daddr1,ADDR2 as daddr2,ADDR3 as daddr3,ADDR4 daddr4,'-' as dtel,'-' as dtinno,'-' as dcstno,acode as mycode,staten as dstaten,gst_no as dgst_no,girno as dpanno,substr(gst_no,0,2) as dstatecode FROM FAMST WHERE ACODE='" + dsRep.Tables[0].Rows[0]["ACODE"].ToString().Trim() + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    }
                    dt.TableName = "csmst";
                    dsRep.Tables.Add(dt);
                    pdfView = "Y";
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_PackingList_SAGE", "std_PackingList_SAGE", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F55161":
                #region THIS IS FOR ONLY CMPL
                header_n = "Packing List";//for cmpl new format
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "select '" + header_n + "' as header, a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,trim(a.icode) as icode,i.iname ,trim(a.acode) as partycode,a.SRNO,is_number(a.COL1) AS carton_wt,a.COL2 AS DESC_,is_number(a.COL3) AS QTY_PR_pallet,is_number(a.col4) as pr_pallet_wt,is_number(a.col5) as tot_wt,is_number(a.col11) as pallet_wt,a.col6 as pallet_dimen,is_number(a.col7) as no_of_pallet ,is_number(a.col3)*is_number(a.col7) as tot_qty,a.col13 as pallet_no,I.TARRIFNO, B.GST_NO,B.TELNUM ,B.PAYMENT,B.EMAIL,b.aname,b.addr1,b.addr2,b.addr3,b.addr4,B.COUNTRY,a.col9,a.col10  from scratch  a ,item i,famst b where trim(a.icode)=trim(i.icode) and trim(a.acode)=trim(b.acode) and TRIM(A.BRANCHCD)||trim(a.type)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')='" + mq1 + "'  order by a.srno";
                dsRep = new DataSet(); dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                fgen.send_cookie("seekSql", SQuery);
                if (dt.Rows.Count > 0)
                {
                    mq4 = "select distinct finvno from ivoucher where branchcd='" + frm_mbr + "' and substr(type,1,1)='4' and type!='4F' AND vchnum='" + dt.Rows[0]["col9"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + dt.Rows[0]["col10"].ToString().Trim() + "'";
                    mq3 = fgen.seek_iname(frm_qstr, frm_cocd, mq4, "finvno");
                    ///////                
                    dt.Columns.Add(new DataColumn("PkgN", typeof(double)));
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    //sale table                  
                    SQuery = "select '" + mq3 + "' as ponodt, acode,vchnum as svch,to_char(vchdate,'dd/mm/yyyy') as svchdt,cscode from sale where branchcd='" + frm_mbr + "' and vchnum='" + dsRep.Tables[0].Rows[0]["col9"].ToString().Trim() + "' and to_char(vchdate,'dd/mm/yyyy')='" + dsRep.Tables[0].Rows[0]["col10"].ToString().Trim() + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt.TableName = "SALES_P";
                    dsRep.Tables.Add(dt);
                    ///////////					
                    SQuery = "Select distinct d.tdsnum, d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dsRep.Tables[1].Rows[0]["cscode"].ToString().Trim() + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count <= 0)
                    {
                        dt = new DataTable();
                        SQuery = "Select 'Consignee' as tdsnum,'Same as Buyer' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                        SQuery = "SELECT ANAME AS consign,ADDR1 as daddr1,ADDR2 as daddr2,ADDR3 as daddr3,ADDR4 daddr4,'-' as dtel,'-' as dtinno,'-' as dcstno,acode as mycode,staten as dstaten,gst_no as dgst_no,girno as dpanno,substr(gst_no,0,2) as dstatecode FROM FAMST WHERE ACODE='" + dsRep.Tables[0].Rows[0]["partycode"].ToString().Trim() + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    }
                    dt.TableName = "csmst";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_PackingList_New", "std_PackingList_New", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F55514":
                #region Import License Report
                header_n = "Import License Report";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col2");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col3");
                dt1 = new DataTable();
                dt1.Columns.Add("Licence_No", typeof(string));
                dt1.Columns.Add("Licence_Date", typeof(string));
                dt1.Columns.Add("Item_Desc", typeof(string));
                dt1.Columns.Add("Quantity", typeof(string));
                dt1.Columns.Add("CIF_VALUE", typeof(string));
                dt1.Columns.Add("Bill_no", typeof(string));
                dt1.Columns.Add("Bill_date", typeof(string));
                dt1.Columns.Add("Qty_kgs", typeof(double));
                dt1.Columns.Add("CIF_Value_USD", typeof(double));
                dt1.Columns.Add("CIF_Value_BE", typeof(double));
                dt1.Columns.Add("Balnc_Qty", typeof(double));
                dt1.Columns.Add("Balnc_value", typeof(double));
                dt1.Columns.Add("duty_save", typeof(double));
                dt1.Columns.Add("file", typeof(string));
                dt1.Columns.Add("impvalid", typeof(string));
                dt1.Columns.Add("BALANCE_QTY", typeof(string));
                dt1.Columns.Add("BALANCE_VALUE", typeof(string));

                i0 = 1;
                SQuery = "select a.vchnum,trim(a.licno) as licno,to_char(a.licdt,'dd/mm/yyyy') as licdt,trim(A.ciname) as ciname,a.dgft_file,to_char(b.impvalid,'dd/mm/yyyy') as impvalid,a.billno,to_char(a.bill_dt,'dd/mm/yyyy') as bill_dt,a.qtyin ,a.iamount,a.fob_val,a.balqty as balnc_qty,a.val_usd as duty_save,a.num1,a.num2,a.srno,a.imp_qty,a.imp_val from wb_licrec a , wb_licrec b where trim(a.licno)=trim(b.licno) and  a.branchcd='" + frm_mbr + "' and a.type='20'and b.type='10' and b.flag='IM' and a.vchdate between to_date('" + mq2 + "','dd/mm/yyyy') and to_date('" + mq1 + "','dd/mm/yyyy') and trim(a.licno)||to_char(a.licdt,'dd/mm/yyyy')||trim(a.ciname) ='" + mq0 + "' group by a.vchnum,trim(a.licno) ,to_char(a.licdt,'dd/mm/yyyy') ,trim(A.ciname) ,a.dgft_file ,to_char(b.impvalid,'dd/mm/yyyy') ,a.billno,to_char(a.bill_dt,'dd/mm/yyyy') ,a.qtyin ,a.iamount,a.fob_val,a.balqty ,a.val_usd,a.num1,a.num2,a.srno ,a.imp_qty,a.imp_val order by a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = dt1.NewRow();
                        dr1["Licence_No"] = dt.Rows[i]["licno"].ToString().Trim();
                        dr1["Licence_Date"] = dt.Rows[i]["licdt"].ToString().Trim();
                        dr1["Item_Desc"] = dt.Rows[i]["ciname"].ToString().Trim();
                        dr1["Quantity"] = dt.Rows[i]["imp_qty"].ToString().Trim();
                        dr1["CIF_VALUE"] = dt.Rows[i]["imp_val"].ToString().Trim();
                        dr1["Bill_no"] = dt.Rows[i]["billno"].ToString().Trim();
                        dr1["Bill_date"] = dt.Rows[i]["bill_dt"].ToString().Trim();
                        dr1["Qty_kgs"] = fgen.make_double(dt.Rows[i]["qtyin"].ToString().Trim());
                        dr1["CIF_Value_USD"] = fgen.make_double(dt.Rows[i]["iamount"].ToString().Trim());
                        dr1["CIF_Value_BE"] = fgen.make_double(dt.Rows[i]["fob_val"].ToString().Trim());
                        dr1["duty_save"] = fgen.make_double(dt.Rows[i]["duty_save"].ToString().Trim());
                        dr1["file"] = dt.Rows[i]["dgft_file"].ToString().Trim();
                        dr1["impvalid"] = dt.Rows[i]["impvalid"].ToString().Trim();
                        dr1["BALANCE_QTY"] = dt.Rows[i]["num1"].ToString().Trim();
                        dr1["BALANCE_VALUE"] = dt.Rows[i]["num2"].ToString().Trim();
                        dt1.Rows.Add(dr1);
                    }
                }
                if (dt1.Rows.Count > 0)
                {
                    dt1.TableName = "prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt1, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Import_License", "Import_License", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F55515":
                #region Export License Report
                header_n = "Export License Report";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                dt1 = new DataTable();
                dt1.Columns.Add("Licence_No", typeof(string));
                dt1.Columns.Add("Licence_Date", typeof(string));
                dt1.Columns.Add("Item_Desc", typeof(string));
                dt1.Columns.Add("Quantity", typeof(string));
                dt1.Columns.Add("value", typeof(string));
                dt1.Columns.Add("Bill_no", typeof(string));
                dt1.Columns.Add("Bill_date", typeof(string));
                dt1.Columns.Add("Invoice_no", typeof(string));
                dt1.Columns.Add("Invoice_date", typeof(string));
                dt1.Columns.Add("RM_Item", typeof(string));
                dt1.Columns.Add("wast_perc", typeof(double));
                dt1.Columns.Add("Without_Wast", typeof(double));
                dt1.Columns.Add("With_Wast", typeof(double));
                dt1.Columns.Add("FOB_Value_rs", typeof(double));
                dt1.Columns.Add("FOB_Value_US$", typeof(double));
                dt1.Columns.Add("FOB_Value_BRC", typeof(double));
                dt1.Columns.Add("FOB_Value_BRC_US$", typeof(double));
                dt1.Columns.Add("file", typeof(string));
                dt1.Columns.Add("expvalid", typeof(string));
                dt1.Columns.Add("desc_", typeof(string));
                dt1.Columns.Add("val_add", typeof(double));

                //SQuery = "select trim(a.licno) as licno,to_char(a.licdt,'dd/mm/yyyy') as licdt,trim(a.icode) as icode,trim(a.ciname) as ciname,c.wast_perc,a.qtyout,a.balqty,trim(a.billno) as billno,to_char(a.bill_dt) as bill_dt,trim(a.invno) as invno,to_char(a.invdate,'dd/mm/yyyy') as invdt,trim(a.dgft_file) as filee,a.val_add ,a.exp_qty,a.exp_val ,trim(b.iname) as iname from wb_licrec a,item b ,wb_licrec c  where trim(a.icode) =trim(b.icode) and trim(a.ciname)=trim(c.ciname) and a.branchcd='"+frm_mbr+"' and a.type='30' and c.type='10' and c.flag='IM' and a.vchnum='000001'";
                //SQuery = "select trim(a.licno) as licno,to_char(a.licdt,'dd/mm/yyyy') as licdt,trim(a.ciname) as ciname,trim(a.term) as exp_name,c.wast_perc,trim(a.desc_) as desc_,sum(a.qtyout) as qtyout,trim(a.billno) as billno,to_char(a.bill_dt,'dd/mm/yyyy') as bill_dt,trim(a.invno) as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,trim(a.dgft_file) as filee,a.exp_qty,a.exp_val,to_char(a.expvalid,'dd/mm/yyyy') as expvalid,a.val_add,a.srno  from wb_licrec a,wb_licrec c where trim(a.licno)||trim(a.ciname)=trim(c.licno)||trim(c.ciname) and a.branchcd='" + frm_mbr + "' and a.type='30' and c.type='10' and c.flag='IM' and a.vchdate between to_date('" + mq2 + "','dd/mm/yyyy') and to_date('" + mq1 + "','dd/mm/yyyy') and trim(a.licno)||to_char(a.licdt,'dd/mm/yyyy')||trim(a.term)='" + mq0 + "' group by  trim(a.licno),to_char(a.licdt,'dd/mm/yyyy'),trim(a.ciname),c.wast_perc,trim(a.billno),to_char(a.bill_dt,'dd/mm/yyyy'),trim(a.invno),to_char(a.invdate,'dd/mm/yyyy'),trim(a.dgft_file),a.exp_qty,a.exp_val,trim(a.term),to_char(a.expvalid,'dd/mm/yyyy'),a.desc_,a.val_add,a.srno order by a.srno ,billno";
                SQuery = "select trim(a.licno) as licno,to_char(a.licdt,'dd/mm/yyyy') as licdt,trim(a.ciname) as ciname,trim(a.term) as exp_name,c.wast_perc,trim(a.desc_) as desc_,sum(a.qtyout) as qtyout,trim(a.billno) as billno,to_char(a.bill_dt,'dd/mm/yyyy') as bill_dt,trim(a.invno) as invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,trim(a.dgft_file) as filee,a.exp_qty,a.exp_val,to_char(a.expvalid,'dd/mm/yyyy') as expvalid,a.val_add,a.srno  from wb_licrec a,wb_licrec c where trim(a.licno)||trim(a.ciname)=trim(c.licno)||trim(c.ciname) and a.branchcd='" + frm_mbr + "' and a.type='30' and c.type='10' and c.flag='IM' and a.vchdate between to_date('" + mq2 + "','dd/mm/yyyy') and to_date('" + mq1 + "','dd/mm/yyyy') and trim(a.licno)||to_char(a.licdt,'dd/mm/yyyy')||trim(a.term)='" + mq0 + "' group by  trim(a.licno),to_char(a.licdt,'dd/mm/yyyy'),trim(a.ciname),c.wast_perc,trim(a.billno),to_char(a.bill_dt,'dd/mm/yyyy'),trim(a.invno),to_char(a.invdate,'dd/mm/yyyy'),trim(a.dgft_file),a.exp_qty,a.exp_val,trim(a.term),to_char(a.expvalid,'dd/mm/yyyy'),a.desc_,a.val_add,a.srno order by ciname desc, srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    DataView view3 = new DataView(dt);
                    DataTable dt7 = new DataTable();
                    dt7 = view3.ToTable(true, "invno", "invdate", "billno", "bill_dt");
                    foreach (DataRow dr3 in dt7.Rows)
                    {
                        DataView view4 = new DataView(dt, "invno='" + dr3["invno"].ToString().Trim() + "' and invdate='" + dr3["invdate"].ToString().Trim() + "' and billno='" + dr3["billno"].ToString().Trim() + "' and bill_dt='" + dr3["bill_dt"].ToString().Trim() + "' ", "", DataViewRowState.CurrentRows);
                        DataTable dt8 = new DataTable();
                        dt8 = view4.ToTable();

                        for (int i = 0; i < dt8.Rows.Count; i++)
                        {
                            dr1 = dt1.NewRow();
                            dr1["Licence_No"] = dt8.Rows[i]["licno"].ToString().Trim();
                            dr1["Licence_Date"] = dt8.Rows[i]["licdt"].ToString().Trim();
                            dr1["Item_Desc"] = dt8.Rows[i]["exp_name"].ToString().Trim();
                            dr1["Quantity"] = dt8.Rows[i]["exp_qty"].ToString().Trim();
                            dr1["value"] = dt8.Rows[i]["exp_val"].ToString().Trim();
                            dr1["Bill_no"] = dt8.Rows[i]["billno"].ToString().Trim();
                            dr1["Bill_date"] = dt8.Rows[i]["bill_dt"].ToString().Trim();
                            dr1["Invoice_no"] = dt8.Rows[i]["invno"].ToString().Trim();
                            dr1["Invoice_date"] = dt8.Rows[i]["invdate"].ToString().Trim();
                            dr1["file"] = dt8.Rows[i]["filee"].ToString().Trim();
                            dr1["RM_Item"] = dt8.Rows[i]["ciname"].ToString().Trim();
                            dr1["desc_"] = dt8.Rows[i]["desc_"].ToString().Trim();
                            dr1["Without_Wast"] = dt8.Rows[i]["qtyout"].ToString().Trim();
                            dr1["wast_perc"] = dt.Rows[i]["wast_perc"].ToString().Trim();
                            dr1["expvalid"] = dt8.Rows[i]["expvalid"].ToString().Trim();
                            db1 = fgen.make_double(dt8.Rows[i]["qtyout"].ToString().Trim()) * fgen.make_double(dt8.Rows[i]["wast_perc"].ToString().Trim()) / 100;
                            dr1["With_Wast"] = fgen.make_double(dt8.Rows[i]["qtyout"].ToString().Trim()) + db1;
                            mq1 = "SELECT trim(invno) as invno,TO_CHAR(ENTRY_DT_BILL,'DD/MM/YYYY') as invdate,TRIM(ENTRY_NO_BILL) as bill_no,TO_CHAR(ENTRY_DT_BILL,'DD/MM/YYYY') as bill_date,FOB,DUTY,EXHG_BRC,FOB_INR,FOB_FOREIGN,FREIGHT_INR_SL FROM WB_EXP_IMP WHERE TYPE='EX' AND BRANCHCD='" + frm_mbr + "' AND TRIM(ENTRY_NO_BILL)||TO_CHAR(ENTRY_DT_BILL,'DD/MM/YYYY')='" + dr1["Invoice_no"] + dr1["Invoice_date"] + "'";
                            if (i == 0)
                            {
                                //dt6 = fgen.getdata(frm_qstr, frm_cocd, mq1);
                                dr1["FOB_Value_rs"] = fgen.seek_iname(frm_qstr, frm_cocd, mq1, "FOB");
                                dr1["FOB_Value_US$"] = fgen.seek_iname(frm_qstr, frm_cocd, mq1, "DUTY");
                                dr1["FOB_Value_BRC"] = fgen.seek_iname(frm_qstr, frm_cocd, mq1, "FOB_INR");
                                dr1["FOB_Value_BRC_US$"] = fgen.seek_iname(frm_qstr, frm_cocd, mq1, "FOB_FOREIGN");
                                dr1["val_add"] = dt.Rows[i]["val_add"].ToString().Trim();
                            }
                            else
                            {
                                dr1["FOB_Value_rs"] = 0;
                                dr1["FOB_Value_US$"] = 0;
                                dr1["FOB_Value_BRC"] = 0;
                                dr1["FOB_Value_BRC_US$"] = 0;
                            }
                            dt1.Rows.Add(dr1);
                        }
                    }
                }

                if (dt1.Rows.Count > 0)
                {
                    dt1.TableName = "Prepcur";
                    dsRep.Tables.Add(dt1);
                    dt4 = new DataTable();
                    dt4.Columns.Add("RM_Item_summ", typeof(string));
                    dt4.Columns.Add("Without_Wast_summ", typeof(double));
                    dt4.Columns.Add("With_Wast_summ", typeof(double));
                    dt4.Columns.Add("Wastage_Perc_summ", typeof(double));
                    dt4.Columns.Add("Import_Adj", typeof(double));
                    dt4.Columns.Add("total_brc", typeof(double));
                    dt4.Columns.Add("val_add", typeof(double));
                    dt4.Columns.Add("Excess", typeof(double));
                    dr1 = null;
                    mq10 = "select TRIM(CINAME) AS CINAME,WAST_PERC,BRANCHCD from wb_licrec where branchcd='" + frm_mbr + "' and type='10' and flAG='IM'";
                    dt5 = new DataTable();
                    dt5 = fgen.getdata(frm_qstr, frm_cocd, mq10);
                    dt6 = new DataTable();
                    mq8 = "select a.licno,a.ciname,sum(a.qtyin) as qty from wb_licrec a where a.branchcd='" + frm_mbr + "' and a.type='20' group by a.licno,a.ciname ";
                    dt6 = fgen.getdata(frm_qstr, frm_cocd, mq8);

                    DataView view1 = new DataView(dt1);
                    dt2 = new DataTable();
                    dt2 = view1.ToTable(true, "rm_item");
                    db5 = 0;
                    foreach (DataRow dr2 in dt2.Rows)
                    {
                        dt2 = view1.ToTable(true, "rm_item");
                        DataView view2 = new DataView(dt1, "rm_item='" + dr2["rm_item"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt3 = new DataTable();
                        dt3 = view2.ToTable();
                        dr1 = dt4.NewRow();
                        db1 = 0; db3 = 0; db4 = 0;
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            db1 += fgen.make_double(dt3.Rows[i]["Without_Wast"].ToString().Trim());
                            db3 += fgen.make_double(dt3.Rows[i]["With_Wast"].ToString().Trim());
                            db4 = fgen.make_double(fgen.seek_iname_dt(dt6, "licno='" + dt3.Rows[i]["licence_no"].ToString().Trim() + "' and ciname='" + dt3.Rows[i]["rm_item"].ToString().Trim() + "'", "qty"));
                            db5 += fgen.make_double(dt3.Rows[i]["FOB_Value_BRC_US$"].ToString().Trim());
                            dr1["val_add"] = fgen.make_double(dt3.Rows[i]["val_add"].ToString().Trim());
                        }
                        dr1["RM_Item_summ"] = dr2["rm_item"].ToString().Trim();
                        dr1["Without_Wast_summ"] = db1;
                        dr1["With_Wast_summ"] = db3;
                        dr1["Import_Adj"] = db4;
                        dr1["total_brc"] = db5;
                        dr1["Wastage_Perc_summ"] = fgen.seek_iname_dt(dt5, "ciname='" + dr2["rm_item"].ToString().Trim() + "'", "WAST_PERC");
                        dt4.Rows.Add(dr1);
                    }
                    dt4.TableName = "Summ";
                    dsRep.Tables.Add(dt4);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Export_License", "Export_License", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F55518":
                SQuery = "select a.vchnum,a.vchdate,trim(a.icode) as icode,trim(a.licno) as licno,to_char(a.licdt,'dd/mm/yyyy') as licdt,a.obsv1 as unit,a.qtyin,a.iamount,a.balqty,a.fob_val,trim(a.ciname) as ciname,trim(a.billno) as billno,to_char(a.bill_dt,'dd/mm/yyyy') as bill_dt,a.val_usd,a.imp_qty,a.imp_val,a.num1,a.num2,a.num4,a.remark,to_char(b.expvalid,'dd/mm/yyyy') as last_date,to_char(b.impvalid,'dd/mm/yyyy') as validty,b.num3 as avg_exp,b.num1 as exp_obli,b.num2 as no_times  from wb_licrec a, wb_licrec b  where trim(a.licno)||to_char(a.licdt,'dd/mm/yyyy')||trim(a.ciname)=trim(b.licno)||to_char(b.licdt,'dd/mm/yyyy')||trim(b.ciname) and b.type='11' and a.branchcd||a.type||trim(a.licno)||to_char(a.licdt,'dd/mm/yyyy')='" + barCode + "'";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Epcg_Imp_License", "Epcg_Imp_License", dsRep, header_n);
                }
                break;
            case "F55519":
                header_n = "EPCG Export Adjustment";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                dt = new DataTable();
                SQuery = "SELECT '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,a.vchnum,a.vchdate,a.licno,to_char(a.licdt,'dd/mm/yyyy') as licdt,a.qtyout,a.cif_val,a.balqty,a.ciname,a.refdate,a.billno,to_char(a.bill_dt,'dd/mm/yyyy') as bill_dt,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.term,a.dgft_file,a.val_add,a.exp_qty,a.exp_val,a.num1,a.num2,a.num3,a.num4,is_number(a.obsv2) as obsv2,to_char(b.expvalid,'dd/mm/yyyy') as last_date,to_char(b.impvalid,'dd/mm/yyyy') as validty,b.num3 as avg_exp,b.num1 as exp_obli,b.num2 as no_times FROM WB_LICREC a, wb_licrec b  where trim(a.licno)||to_char(a.licdt,'dd/mm/yyyy')||trim(a.term)=trim(b.licno)||to_char(b.licdt,'dd/mm/yyyy')||trim(b.ciname) and b.type='11' and a.BRANCHCD||a.TYPE||TRIM(a.LICNO)||TO_CHAR(a.LICDT,'DD/MM/YYYY')||TRIM(a.TERM) ='" + barCode + "'";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt1 = new DataTable();
                mq0 = "select val_usd from wb_licrec where  branchcd='" + frm_mbr + "' and type='21' and TRIM(LICNO)||TO_CHAR(LICDT,'DD/MM/YYYY')||TRIM(ciname) ='" + barCode.Substring(4) + "'";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(dt);
                    dt1.TableName = "prep2";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Epcg_Exp", "Epcg_Exp", dsRep, header_n);
                }
                break;

            case "F55520":
                SQuery = "select a.vchnum,to_char(a.vchdate,'dd/mm/yyyy') as vchdate ,a.icode,b.iname,a.hscode,c.name,a.licno,to_char(a.licdt,'dd/mm/yyyy') as licdt,a.qtyout as qty_nos,a.iamount as rate_usd,a.cif_val as value_USD,a.balqty as Qty_kgs,a.invno,to_char(a.invdate,'dd/mm/yyyy') as invdate,a.val_add as scheme_code,a.wast_perc,a.num1 as item_rate,a.num2 as value_usd2,a.num3 as present_market_value from wb_licrec a , item b , typegrp c where trim(a.hscode)=trim(c.acref) and trim(a.icode)=trim(b.icode) and c.id='T1' and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "'";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Annex_Cust", "Annex_Cust", dsRep, header_n);
                }
                break;

            case "F55521":
                #region
                header_n = "Advance Cargo Declaration (Non-Hazardous Cargo)";
                mq0 = "";
                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, "select est_code from type where id='B' and type1='" + frm_mbr + "'", "est_code");

                SQuery = "select '" + mq1 + "' as iec_code,b.name as hsname,substr(trim(A.INVNO),3,4)||'/'||trim(a.num1)||'  TO  '||substr(trim(a.invno),3,4)||'/'||trim(a.num2) as seal_no,a.* from wb_exp_frt a,typegrp b where trim(a.hscode)=trim(b.acref) and b.id='T1' and a.branchcd='" + frm_mbr + "' and a.type='10' and trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' order by seal_no asc";
                dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt3 = new DataTable(); dt4 = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(dt);
                    //for branch
                    mq0 = "select  distinct type1, trim(name) as mbr_name,trim(addr)||trim(addr1) as mbr_Adr,place,tele  from typE where id='B' AND TYPE1='" + dt.Rows[0]["obsv10"].ToString().Trim() + "'";
                    dt4 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                    dt4.TableName = "branch_detail";
                    dsRep.Tables.Add(dt4);
                    ///////for consignee block
                    SQuery = "SELECT  ACODE AS CSCODE,ANAME AS CNAME,ADDR1 AS CADDR1,ADDR2 AS CADDR2,ADDR3 AS CADDR3,TELNUM AS CTELNUM,FAX AS CFAX  FROM CSMST WHERE ACODE='" + dt.Rows[0]["cscode"].ToString().Trim() + "'";
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt1.TableName = "Consg";
                    dsRep.Tables.Add(dt1);
                    ////////// for notify block
                    SQuery = "SELECT  ACODE AS NCODE,ANAME AS NNAME,ADDR1 AS NADDR1,ADDR2 AS NADDR2,ADDR3 AS NADDR3,TELNUM AS NTELNUM,FAX AS NFAX  FROM CSMST WHERE ACODE='" + dt.Rows[0]["vcode"].ToString().Trim() + "'";
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt2.TableName = "Notify_blk";
                    dsRep.Tables.Add(dt2);
                    ////FOR NOTIFY 2ND BLOK
                    SQuery = "SELECT  ACODE AS NCODE1,ANAME AS NNAME1,ADDR1 AS NADDR1_,ADDR2 AS NADDR2_,ADDR3 AS NADDR3_,TELNUM AS NTELNUM1,FAX AS NFAX1  FROM CSMST WHERE ACODE='" + dt.Rows[0]["vcode2"].ToString().Trim() + "'";
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt3.TableName = "Notify_blk_II";
                    dsRep.Tables.Add(dt3);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Adv_Cargo", "Adv_Cargo", dsRep, header_n);
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
            if (pdfView == "Y") conv_pdf(data_set, rptfile);
        }
        else
        {
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
            if (pdfView == "Y") conv_pdf(data_set, rptfile);
        }
        else
        {
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