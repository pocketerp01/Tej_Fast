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
using System.Linq;
using System.Drawing;

public partial class sale_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, xprdrange, xprdRange1, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, cond, todt, part_cd, party_cd, mq1, mq2, mq3, mq4, mq5, pdfView = "", data_found = "", branch_Cd;
    fgenDB fgen = new fgenDB();
    private DataSet DsImages = new DataSet();
    FileStream FilStr = null; BinaryReader BinRed = null;
    DataTable ph_tbl;
    double db = 0, db1 = 0, db2 = 0, db3 = 0, db4 = 0, db5 = 0, db6 = 0, db7 = 0, db8 = 0;
    string xhtml_tag = "", firm = "", subj = "";
    string pdfdoc = "", pdffirm = "", pdfno = "", pdfdt = "";

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

                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");

                    if (fromdt == "0")
                    {
                        fromdt = frm_cDt1;
                        todt = frm_cDt2;
                    }

                    hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    pdfView = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PDFVIEW");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", "-");
                    branch_Cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_BRANCHCD");
                }
                else Response.Redirect("~/login.aspx");

            }
            //if (!Page.IsPostBack)
            {
                btnexpwithsig.Visible = false;
                btnExptoTiff.Visible = hfhcid.Value == "F50271" ? true : false;
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "USEND_MAIL") == "Y") tremail.Visible = true;
                else tremail.Visible = false;
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
        string mq10, mq1, mq0, header_n = "";
        string ded1, mq2 = "", mq3 = "", mq6 = "", mq7 = "", mq8 = "", mq9 = "", mq11 = "", mq12 = "";
        int repCount = 1;
        frm_rptName = "";
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        data_found = "Y";

        string doc_GST = "";
        string chk_opt = "";
        chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2027'", "fstr");
        if (chk_opt == "Y")
        //Member GCC Country
        {
            doc_GST = "GCC";
        }

        switch (iconID)
        {
            case "F50101":
            case "F50106":
            case "F55106":
            case "F1006":
            case "F1006A":
            case "F50271":
                #region INV
                if (iconID == "F50271")
                {
                    frm_vty = hfval.Value.Replace("'", "");
                    barCode = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                }

                string yr = "";
                mq4 = barCode.Substring(0, 6);
                string CURR = fromdt.Substring(8, 2);
                int nxt = Convert.ToInt32(CURR) + 1;
                scode = scode.Replace(";", "");
                SQuery = "select distinct A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,(a.iamount*round(a.exc_RATE/100,3)) as cgst_val,a.cess_percent as sgst,(a.iamount*round(a.cess_percent/100,3)) as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,t.type1,t1.name,C.tcsamt,a.vchdate as vdd from ivoucher a,sale c,item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by vdd,a.vchnum,a.morder ";
                SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'yyyymmdd') AS FSTR, A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.email as p_email,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,(a.iamount*round(a.exc_RATE/100,3)) as cgst_val,a.cess_percent as sgst,(a.iamount*round(a.cess_percent/100,3)) as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,t.type1,t1.name,C.tcsamt,a.vchdate as vdd,c.acvdrt,a.doc_tot from ivoucher a,sale c,item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode)  AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by vdd,a.vchnum,a.morder";

                if (iconID == "F50106" || iconID == "F55106")
                {
                    SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'yyyymmdd') AS FSTR, A.MORDER, 'N' as logo_yn, a.branchcd,to_number(a.cess_pu) cess_pu,a.type," +
                        "d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp," +
                        "a.finvno,a.exc_57f4,a.iexc_Addl,to_number(A.exc_amt) exc_amt ,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val," +
                        "to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno," +
                        "b.person as cperson,b.email as p_email,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate," +
                        "c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate," +
                        "c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as" +
                        " grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt," +
                        "c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode," +
                        "a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt," +
                        "a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,to_number(a.iamount) iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg," +
                        "a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,to_number(a.exc_RATE) as cgst," +
                        "to_number(a.exc_amt) as cgst_val,to_number(a.cess_percent) as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1," +
                        "'  PROFORMA  '||t1.name as Name,C.tcsamt,a.col1,a.col2,a.col3,a.col4,a.col5,a.col6,a.col7,a.col8,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p" +
                        ",er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p" +
                        ",er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p from ivoucherp a,salep c  left join csmst er on trim(c.cscode)=trim(er.acode)" +
                        ",item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) " +
                        "and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and" +
                        " trim(A.icode)=trim(d.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') " +
                        "in ('" + barCode.Replace("'", "") + "') order by a.vchnum,a.MORDER";
                }
                else
                {
                    //SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'yyyymmdd') AS FSTR, A.MORDER, 'N' as logo_yn, a.branchcd,to_number(a.cess_pu) CESS_PU,a.type,d.ciname," +
                    //    "d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4," +
                    //    "a.iexc_Addl,to_number(A.exc_amt) EXC_AMT,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate," +
                    //    "b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.email as p_email,b.rc_num2 as cstno," +
                    //    " b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno," +
                    //    "c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime," +
                    //    "c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no," +
                    //    "c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno," +
                    //    "a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag," +
                    //    "a.approxval as bal,a.ichgs as cdisc,to_number(a.iamount) IAMOUNT,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by," +
                    //    "to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,to_number(a.exc_RATE) as cgst,a.exc_amt as cgst_val," +
                    //    "to_number(a.cess_percent) as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,c.acvdrt,a.doc_tot,er.aname as consign_p," +
                    //    "er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p," +
                    //    "er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p from ivoucher a,sale c left join csmst er on trim(c.cscode)=trim(er.acode)," +
                    //    "item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' " +
                    //    "and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) " +
                    //    "AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by vchdate,a.vchnum,a.MORDER";

                    SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'yyyymmdd') AS FSTR, A.MORDER, 'N' as logo_yn, a.branchcd,to_number(a.cess_pu) CESS_PU,a.type,d.ciname," +
                        "d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4," +
                        "a.iexc_Addl,to_number(A.exc_amt) EXC_AMT,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate," +
                        "b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.PINCODE as p_pincode,LOWER(b.email) as p_email,b.rc_num2 as cstno," +
                        " b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(a.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno," +
                        "c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime," +
                        "c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no," +
                        "c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno," +
                        "a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag," +
                        "a.approxval as bal,a.ichgs as cdisc,to_number(a.iamount) IAMOUNT,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by," +
                        "to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,to_number(a.exc_RATE) as cgst,a.exc_amt as cgst_val," +
                        "to_number(a.cess_percent) as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,substr(NVL(b.gst_no,'-'),0,2) AS type1,t1.name,C.tcsamt,c.acvdrt,a.doc_tot,nvl(er.aname,b.aname) as consign_p," +
                        "nvl(er.addr1,b.addr1) as daddr1_p,nvl(er.addr2,b.addr2) as daddr2_p,nvl(er.addr3,b.addr3) as daddr3_p,nvl(er.addr4,b.addr4) as daddr4_p,er.pincode as d_pincode,nvl(er.telnum,b.mobile) as dtel_p, er.rc_num as dtinno_p,nvl(er.exc_num,b.exc_num) as dcstno_p,nvl(er.acode,b.acode) as mycode_p," +
                        "nvl(er.staten,b.staten) as dstaten_p,nvl(er.gst_no,b.gst_no) as dgst_no_p,nvl(er.girno,b.girno) as dpanno_p,substr(nvl(er.gst_no,b.gst_no),0,2) as dstatecode_p,to_char(a.refdate,'dd/MM/yyyy') as refdate   from ivoucher a,sale c left join csmst er on trim(c.cscode)=trim(er.acode)," +
                        "item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' " +
                        "and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) " +
                        "AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by vchdate,a.vchnum,a.MORDER";
                }

                if (frm_cocd == "STUD")
                {
                    if (frm_vty == "4F")
                    {
                        SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') AS FSTR,A.MORDER,'N' as logo_yn,C.CURREN,C.THRU,a.BRANCHCD||a.TYPE||TRIM(a.ponum)||TO_CHAr(a.podate,'DDMMYYYY') AS busiexpect,a.iweight,b.payment,nvl(a.naration,'-') as grosswt,t2.bankname,t2.bankaddr,t2.vat_form as swiftcode,t2.bankac as ac, a.branchcd,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,is_number(a.no_bdls) as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,nvl(a.st_modv,0) as cash_disc,nvl(a.st_nmodv,0) as oth_disc,f.telnum as tpt_telnum,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,h.invno AS Hinvno,TO_CHAR(h.invdate,'DD/MM/YYYY') AS Hinvdate,h.ship2,h.ship3,h.ship4,h.ship5,h.lbnetwt,h.REMARK3 AS NETWT,h.lbgrswt,h.exprmk1,h.exprmk2,h.exprmk3,h.exprmk4,h.exprmk5,h.addl1,h.addl2,h.addl3,h.addl4,h.addl5,h.tmaddl1,h.tmaddl2,h.tmaddl3,h.addl6 from ivoucher a left join hundi h on trim(a.branchcd)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(h.branchcd)||trim(h.acode)||trim(h.invno)||to_char(h.invdate,'dd/mm/yyyy'),sale c left join famst f on trim(c.tptcode)=trim(f.acode) left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,TYPE t2,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and trim(a.branchcd)=trim(t2.type1) and t2.id='B' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND a.branchcd='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by vchdate,a.vchnum,a.MORDER";
                    }
                    else
                    {
                        SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') AS FSTR,A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,is_number(a.no_bdls) as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,to_number(a.exc_RATE) as cgst,to_number(a.exc_amt) as cgst_val,to_number(a.cess_percent) as sgst,to_number(a.cess_pu) as sgst_val,a.iopr,d.hscode,to_number(b.gst_no) as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,nvl(a.st_modv,0) as cash_disc,nvl(a.st_nmodv,0) as oth_disc,B.COUNTRY,d.packsize,f.telnum as tpt_telnum,nvl(a.et_paid,0) as et_paid,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p from ivoucher a,sale c left join famst f on trim(c.tptcode)=trim(f.acode) left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND a.branchcd='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by vchdate,a.vchnum,a.MORDER";
                    }
                }

                if (frm_cocd == "SAIA") frm_rptName = "std_inv_saia";
                if (frm_cocd == "MASS" || frm_cocd == "MAST") frm_rptName = "std_inv_bank_hsn";
                if (frm_rptName.Length < 2)
                {
                    if (iconID == "F1006A" || iconID == "F50101" || iconID == "F50106")
                    {
                        if (frm_cocd == "KRSM")
                        {
                            frm_rptName = "std_inv_bank";
                        }
                        else if (frm_cocd == "TEJAXO" || frm_cocd == "DESH") frm_rptName = "std_inv_bank";
                        else
                        {
                            frm_rptName = "std_inv_bank";
                        }
                    }
                    if (frm_cocd == "AGRM") frm_rptName = "std_inv_agrm";
                }
                if (iconID == "F50106" || iconID == "F55106")
                {
                    frm_rptName = "std_Perf_inv";
                    if (frm_cocd == "KRSM")
                    {
                        frm_rptName = "std_inv_banK_PRO";
                    }
                    
                }
                if (frm_rptName.Length < 2) frm_rptName = "std_inv";

                if (frm_cocd == "HPPI" || doc_GST == "GCC") frm_rptName = "std_inv_UAE";
                if (doc_GST == "GCC" && iconID == "F50106") frm_rptName = "std_pi_UAE";
               

                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    //if (frm_vty != "4F")
                    {
                        DataTable dtHSDetails = new DataTable();
                        dtHSDetails.Columns.Add("FSTR", typeof(string));
                        dtHSDetails.Columns.Add("HSCODE_H", typeof(string));
                        dtHSDetails.Columns.Add("AMT", typeof(decimal));
                        dtHSDetails.Columns.Add("CGST_H", typeof(decimal));
                        dtHSDetails.Columns.Add("SGST_H", typeof(decimal));
                        dtHSDetails.Columns.Add("IGST_H", typeof(decimal));
                        dtHSDetails.Columns.Add("GST_H", typeof(decimal));

                        dtHSDetails = dt.AsEnumerable().GroupBy(r => new
                        {
                            fstr = r.Field<string>("FSTR"),
                            hscode = r.Field<string>("HSCODE").Trim(),
                            cgst = r.Field<decimal>("CGST"),
                            sgst = r.Field<decimal>("SGST"),
                            igst = r.Field<decimal>("CGST")
                        })
                             .Select(g =>
                                 {
                                     var row = dtHSDetails.NewRow();
                                     row["FSTR"] = g.Key.fstr;
                                     row["HSCODE_H"] = g.Key.hscode;
                                     if (g.Key.sgst > 0)
                                     {
                                         row["CGST_H"] = g.Key.cgst;
                                         row["SGST_H"] = g.Key.sgst;
                                         row["IGST_H"] = 0;
                                     }
                                     else
                                     {
                                         row["CGST_H"] = 0;
                                         row["SGST_H"] = 0;
                                         row["IGST_H"] = g.Key.igst;
                                     }
                                     row["AMT"] = g.Sum(r => r.Field<decimal>("IAMOUNT"));
                                     row["GST_H"] = g.Sum(r => r.Field<decimal>("EXC_AMT")) + g.Sum(r => r.Field<decimal>("CESS_PU"));
                                     return row;
                                 }).CopyToDataTable();

                        dtHSDetails.TableName = "dtHSDetails";
                        dsRep.Tables.Add(dtHSDetails);
                    }
                    dt.Columns.Add(new DataColumn("amtToword", typeof(string)));
                    dt.Columns.Add(new DataColumn("PkgN", typeof(double)));
                    if (frm_vty == "4F")
                    {
                        dt.Columns.Add("EXP_YR", typeof(string));
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["pkgN"] = fgen.make_double(fgen.getNumericOnly(dr["pkg"].ToString()));
                        dr["amtToword"] = fgen.ConvertNumbertoWords(dr["bill_tot"].ToString().Trim());
                        if (frm_vty == "4F")
                        {
                            yr = nxt.ToString();
                            yr = "MT/EXP/" + dr["vchnum"].ToString().Trim().Substring(2, 4) + "/" + CURR + "-" + yr + "";
                            dr["EXP_YR"] = yr;
                        }
                    }

                    dt.TableName = "Prepcur";
                    repCount = 4;
                    if (iconID == "F50106" || iconID == "F55106") repCount = 1;
                    if (frm_vty == "4F")
                    {
                        repCount = 4;
                    }
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));


                    // VIPIN                                        
                    //if (frm_cocd == "PPAP")
                    {
                        dt1 = new DataTable("barcode");
                        dt1.Columns.Add(new DataColumn("img1_desc", typeof(string)));
                        dt1.Columns.Add(new DataColumn("img1", typeof(System.Byte[])));
                        string col2 = "";
                        mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no from type where id='B' and type1='" + dt.Rows[0]["branchcd"].ToString().Trim().Replace("/", "") + "'", "gst_no");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            col1 = dt.Rows[i]["fstr"].ToString().Trim();
                            #region PPAP
                            if (frm_cocd.Equals("PPAP"))
                            {
                                col2 = col2 + "AC"
                                       + dt.Rows[i]["pordno"].ToString().Trim().Replace("/", "")
                                       + dt.Rows[i]["cpartno"].ToString().Trim().Replace("/", "")
                                       + (char)13
                                       + dt.Rows[i]["vchnum"].ToString().Trim().Replace("/", "")
                                       + (char)9
                                       + dt.Rows[i]["vchdate"].ToString().Trim().Replace("/", "")
                                       + dt.Rows[i]["qty"].ToString().Trim().Replace("/", "")
                                       + (char)9
                                       + dt.Rows[i]["bill_tot"].ToString().Trim().Replace("/", "")
                                       + (char)9
                                       + "8708.99.00"
                                       + "0.00"
                                       + (char)9
                                       + dt.Rows[i]["cess_pu"].ToString().Trim().Replace("/", "")
                                       + (char)9;
                                if (dt.Rows[i]["iopr"].ToString().Trim().Replace("/", "").ToUpper().Equals("IG"))
                                {
                                    col2 = col2 + dt.Rows[i]["exc_amt"].ToString().Trim().Replace("/", "");
                                }
                                col2 = col2 + (char)9
                                    + "0.00"
                                    + (char)9
                                     + dt.Rows[i]["irate"].ToString().Trim().Replace("/", "")
                                      + (char)9
                                      + dt.Rows[i]["iamount"].ToString().Trim().Replace("/", "")
                                      + (char)9;
                                if (dt.Rows[i]["iopr"].ToString().Trim().Replace("/", "").ToUpper().Equals("CG"))
                                {
                                    col2 = col2 + dt.Rows[i]["exc_amt"].ToString().Trim().Replace("/", "");
                                }
                                col2 = col2 + (char)9
                                     + "0.00"
                                     + (char)9
                                       + "0.00"
                                     + (char)9
                                   + dt.Rows[i]["iamount"].ToString().Trim().Replace("/", "")
                                         + (fgen.make_double(dt.Rows[i]["qty"].ToString().Trim().Replace("/", "")) * fgen.make_double(dt.Rows[i]["iexc_Addl"].ToString().Trim().Replace("/", ""))).ToString()
                                         + (char)9
                                           + "0.00"
                                     + (char)9
                                     + dt.Rows[i]["iexc_Addl"].ToString().Trim().Replace("/", "")
                                      + (char)9
                                           + "0.00"
                                     + (char)9
                                     + mq10
                                   + (char)9;
                            }
                            #endregion
                        }
                        fpath = Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "") + ".png");
                        del_file(fpath);
                        if (frm_cocd == "PPAP") fgen.prnt_QRbar(frm_cocd, col2, col1.Replace("*", "").Replace("/", "") + ".png");
                        else if (frm_cocd == "WING")
                        {
                            fpath = Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "").Replace(",", ""));
                            fgen.FILL_ERR(fpath);
                            fgen.prnt_2Dbar32bit(frm_cocd, col1, fpath);
                            fpath = fpath + ".bmp";
                            frm_rptName = "std_inv2d";
                        }
                        else fgen.prnt_QRbar(frm_cocd, col1, col1.Replace("*", "").Replace("/", "") + ".png");

                        DataRow dr = dt1.NewRow();
                        FilStr = new FileStream(fpath, FileMode.Open);
                        BinRed = new BinaryReader(FilStr);

                        dr["img1_desc"] = col1.Trim();
                        dr["img1"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);

                        dt1.Rows.Add(dr);
                        FilStr.Close();
                        BinRed.Close();

                        dsRep.Tables.Add(dt1);
                    }

                    ////csmst                
                    //SQuery = "Select distinct d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dt.Rows[0]["cscode"].ToString().Trim() + "'";
                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //if (dt.Rows.Count <= 0)
                    //{
                    //    dt = new DataTable();
                    //    SQuery = "Select 'Same as Recipient' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                    //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //}
                    //dt.TableName = "csmst";
                    //dsRep.Tables.Add(dt);

                    // inv terms
                    SQuery = "SELECT DISTINCT COL1 AS POTERMS,SRNO FROM DOCTERMS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' AND DOCTYPE='INV' ORDER BY SRNO";
                    if (iconID == "F50106" || iconID == "F55106")
                        SQuery = "SELECT DISTINCT udf_name||' '||udf_value AS POTERMS,SRNO FROM udf_data WHERE BRANCHCD='" + frm_mbr + "' AND PAR_FLD='" + frm_mbr + frm_vty + barCode.Replace("'", "") + "' ORDER BY SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    mq10 = "";
                    dt1 = new DataTable();
                    mdr = null;
                    dt1.Columns.Add("poterms", typeof(string));
                    if (frm_cocd == "STUD")
                    {
                        dt1.Columns.Add("poterms1", typeof(string));
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        mq10 += dt.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
                        if (frm_cocd == "STUD")
                        {
                            if (i > 2)
                            {
                                mq11 += dt.Rows[i]["POTERMS"].ToString().Trim();//+ Environment.NewLine
                            }
                        }
                    }
                    mdr = dt1.NewRow();
                    mdr["poterms"] = mq10;
                    if (frm_cocd == "STUD")
                    {
                        mdr["poterms1"] = mq11;
                    }
                    dt1.Rows.Add(mdr);
                    btnexpwithsig.Visible = true;
                    if (frm_cocd == "STUD")
                    {
                        if (frm_vty == "44")
                        {
                            frm_rptName = "std_inv_stud44";
                        }
                        else if (frm_vty == "43")
                        {
                            frm_rptName = "std_inv_stud43";
                        }
                        else if (frm_vty == "4F")
                        {
                            frm_rptName = "ExpInv_STUD";
                        }
                        else
                        {
                            frm_rptName = "std_inv_stud_all";
                        }
                    }
                    dt1.TableName = "INV_TERMS";
                    dsRep.Tables.Add(dt1);
                    if (frm_vty == "4F")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv_EXP", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                    }
                    else
                    {
                        if (frm_cocd == "KRSM")
                        {
                            Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv_bank", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                        }
                        else if (frm_cocd == "MASS" || frm_cocd == "MAST")
                        {
                            Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv_bank", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                        }
                        else
                        {
                            Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv_bank", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                        }
                    }
                    // btnexpwithsig_Click(null, null); // FOR DIRECT DOWNLOAD
                }
                else
                {
                    data_found = "N";
                }
                //printDefault(frm_cocd, frm_mbr, "std_invcl", "std_invcl", dsRep, "Invoice Challan");
                #endregion
                break;

            case "F50111":
                header_n = "Despatch Note";
                dt = new DataTable();
                //SQuery = "SELECT '" + header_n + "' AS HEADER,trim(a.acode) as acode,D.NAME,TRIM(B.ANAME) AS PARTY,B.ADDR1,B.ADDR2,B.ADDR3,a.packno,to_char(a.packdate,'dd/mm/yyyy') as vchdate,a.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORDDT, A.PORDNO,A.PORDDT,A.ICODE,C.INAME,C.CPARTNO,A.ORDLINE,A.QTYSUPP AS QTY,A.QTYORD AS ORD_qTY,A.IRATE,C.UNIT,nvl(a.cscode,'-') as cscode,nvl(g.ANAME,'-') AS CONSG,nvl(g.addr1,'-') as cdr1,nvl(g.addr2,'-') as cadr2,nvl(g.addr3,'-') as cadr3 FROM DESPATCH  a left outer join csmst G on trim(a.cscode)=trim(g.acode),FAMST B ,ITEM C ,TYPE D  WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(a.TYPE)=TRIM(D.TYPE1) AND D.ID='V' AND a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' and TRIM(A.PACKNO)||TO_CHAR(A.PACKDATE,'DD/MM/YYYY') in (" + barCode + ")";
                SQuery = "SELECT '" + header_n + "' AS HEADER,trim(a.acode) as acode,D.NAME,TRIM(B.ANAME) AS PARTY,B.ADDR1,B.ADDR2,B.ADDR3," +
                    "a.packno,to_char(a.packdate,'dd/mm/yyyy') as vchdate,a.ORDNO,TO_CHAR(A.ORDDT,'DD/MM/YYYY') AS ORDDT, A.PORDNO,A.PORDDT,A.ICODE,C.INAME,C.CPARTNO,A.ORDLINE" +
                    ",A.QTYSUPP AS QTY,A.QTYORD AS ORD_qTY,A.IRATE,C.UNIT,nvl(a.cscode,'-') as cscode,A.NO_BDLS AS ROLL,A.WEIGHT AS STD_PKG,nvl(g.ANAME,'-') AS CONSG,nvl(g.addr1,'-') as cdr1" +
                    ",nvl(g.addr2,'-') as cadr2,nvl(g.addr3,'-') as cadr3 FROM DESPATCH  a left outer join csmst G on trim(a.cscode)=trim(g.acode),FAMST B ,ITEM C ,TYPE D  " +
                    "WHERE TRIM(A.ACODE)=TRIM(B.ACODE) AND TRIM(A.ICODE)=TRIM(C.ICODE) AND TRIM(a.TYPE)=TRIM(D.TYPE1) AND D.ID='V' AND  a.branchcd='" + frm_mbr + "' and a.type='" + frm_vty + "' " +
                    "and TRIM(A.PACKNO)||TO_CHAR(A.PACKDATE,'DD/MM/YYYY') in (" + barCode + ")";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    frm_rptName = "std_Disp_Adv";                    
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Disp_Adv", "std_Disp_Adv", dsRep, "std_Disp_Adv");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F49212":
                #region INV
                if (xprdRange.Length == 1)
                {
                    xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                }

                cond = "and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ")";
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1").Length > 1) { cond = "and TRIM(a.vchnum)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1") + "' and a.vchdate " + xprdRange + " "; }
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1").Length > 1 && fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3").Length > 1) { cond = "and TRIM(a.vchnum) between '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR1") + "' and '" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR3") + "' and a.vchdate " + xprdRange + " "; }
                if (fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR5").Length > 1) cond = cond + " and trim(a.acode)='" + fgenMV.Fn_Get_Mvar(frm_qstr, "U_COLR5") + "' ";

                yr = "";
                CURR = frm_cDt1.Substring(8, 2);
                nxt = Convert.ToInt32(CURR) + 1;

                SQuery = "select distinct A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.email as p_email,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,c.acvdrt,a.doc_tot from ivoucher a,sale c,item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' " + cond + " order by vchdate,a.vchnum,a.MORDER";
                SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'yyyymmdd') AS FSTR, A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.email as p_email,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,c.acvdrt,a.doc_tot,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,a.acode from ivoucher a,sale c left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' " + cond + " order by vchdate,a.vchnum,a.MORDER";

                if (frm_cocd == "STUD")
                {
                    if (frm_vty == "4F")
                    {
                        SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') AS FSTR,A.MORDER,'N' as logo_yn,C.CURREN,C.THRU,a.BRANCHCD||a.TYPE||TRIM(a.ponum)||TO_CHAr(a.podate,'DDMMYYYY') AS busiexpect,a.iweight,b.payment,nvl(a.naration,'-') as grosswt,t2.bankname,t2.bankaddr,t2.vat_form as swiftcode,t2.bankac as ac, a.branchcd,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,is_number(a.no_bdls) as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,nvl(a.st_modv,0) as cash_disc,nvl(a.st_nmodv,0) as oth_disc,f.telnum as tpt_telnum,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p,h.invno AS Hinvno,TO_CHAR(h.invdate,'DD/MM/YYYY') AS Hinvdate,h.ship2,h.ship3,h.ship4,h.ship5,h.lbnetwt,h.REMARK3 AS NETWT,h.lbgrswt,h.exprmk1,h.exprmk2,h.exprmk3,h.exprmk4,h.exprmk5,h.addl1,h.addl2,h.addl3,h.addl4,h.addl5,h.tmaddl1,h.tmaddl2,h.tmaddl3,h.addl6 from ivoucher a left join hundi h on trim(a.branchcd)||trim(a.acode)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(h.branchcd)||trim(h.acode)||trim(h.invno)||to_char(h.invdate,'dd/mm/yyyy'),sale c left join famst f on trim(c.tptcode)=trim(f.acode) left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,TYPE t2,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and trim(a.branchcd)=trim(t2.type1) and t2.id='B' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND a.branchcd='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' " + cond + " order by vchdate,a.vchnum,a.MORDER";
                    }
                    else
                    {
                        SQuery = "select distinct a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY') AS FSTR,A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.ciname,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,is_number(a.no_bdls) as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,a.exc_amt as cgst_val,a.cess_percent as sgst,a.cess_pu as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,b.vencode,t.type1,t1.name,C.tcsamt,nvl(a.st_modv,0) as cash_disc,nvl(a.st_nmodv,0) as oth_disc,B.COUNTRY,d.packsize,f.telnum as tpt_telnum,nvl(a.et_paid,0) as et_paid,er.aname as consign_p,er.addr1 as daddr1_p,er.addr2 as daddr2_p,er.addr3 as daddr3_p,er.addr4 as daddr4_p,er.telnum as dtel_p, er.rc_num as dtinno_p,er.exc_num as dcstno_p,er.acode as mycode_p,er.staten as dstaten_p,er.gst_no as dgst_no_p,er.girno as dpanno_p,substr(er.gst_no,0,2) as dstatecode_p from ivoucher a,sale c left join famst f on trim(c.tptcode)=trim(f.acode) left join csmst er on trim(c.cscode)=trim(er.acode),item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) AND a.branchcd='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' " + cond + " order by vchdate,a.vchnum,a.MORDER";
                    }
                }

                if (frm_rptName.Length < 2)
                {
                    if (iconID == "F1006A" || iconID == "F50101" || iconID == "F50106") frm_rptName = "std_inv";
                    if (frm_cocd == "AGRM") frm_rptName = "std_inv_agrm";
                    if (iconID == "F50106" && frm_cocd == "MULT") frm_rptName = "std_Perf_inv";
                }
                if (frm_cocd == "SAIA") frm_rptName = "std_inv_saia";
                if (frm_rptName.Length < 2) frm_rptName = "std_inv";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    if (frm_vty != "4F")
                    {
                        DataTable dtHSDetails = new DataTable();
                        dtHSDetails.Columns.Add("FSTR", typeof(string));
                        dtHSDetails.Columns.Add("HSCODE_H", typeof(string));
                        dtHSDetails.Columns.Add("AMT", typeof(decimal));
                        dtHSDetails.Columns.Add("CGST_H", typeof(decimal));
                        dtHSDetails.Columns.Add("SGST_H", typeof(decimal));
                        dtHSDetails.Columns.Add("IGST_H", typeof(decimal));
                        dtHSDetails.Columns.Add("GST_H", typeof(decimal));

                        dtHSDetails = dt.AsEnumerable().GroupBy(r => new
                        {
                            fstr = r.Field<string>("FSTR"),
                            hscode = r.Field<string>("HSCODE").Trim(),
                            cgst = r.Field<decimal>("CGST"),
                            sgst = r.Field<decimal>("SGST"),
                            igst = r.Field<decimal>("CGST")
                        })
                             .Select(g =>
                             {
                                 var row = dtHSDetails.NewRow();
                                 row["FSTR"] = g.Key.fstr;
                                 row["HSCODE_H"] = g.Key.hscode;
                                 if (g.Key.sgst > 0)
                                 {
                                     row["CGST_H"] = g.Key.cgst;
                                     row["SGST_H"] = g.Key.sgst;
                                     row["IGST_H"] = 0;
                                 }
                                 else
                                 {
                                     row["CGST_H"] = 0;
                                     row["SGST_H"] = 0;
                                     row["IGST_H"] = g.Key.igst;
                                 }
                                 row["AMT"] = g.Sum(r => r.Field<decimal>("IAMOUNT"));
                                 row["GST_H"] = g.Sum(r => r.Field<decimal>("EXC_AMT")) + g.Sum(r => r.Field<decimal>("CESS_PU"));
                                 return row;
                             }).CopyToDataTable();

                        dtHSDetails.TableName = "dtHSDetails";
                        dsRep.Tables.Add(dtHSDetails);

                        //DataTable dtHSDetails = new DataTable();
                        //dtHSDetails.Columns.Add("FSTR", typeof(string));
                        //dtHSDetails.Columns.Add("HSCODE_H", typeof(string));
                        //dtHSDetails.Columns.Add("AMT", typeof(decimal));
                        //dtHSDetails.Columns.Add("CGST_H", typeof(decimal));
                        //dtHSDetails.Columns.Add("SGST_H", typeof(decimal));
                        //dtHSDetails.Columns.Add("IGST_H", typeof(decimal));
                        //dtHSDetails.Columns.Add("GST_H", typeof(decimal));

                        //dtHSDetails = dt.AsEnumerable().GroupBy(r => new
                        //{
                        //    fstr = r.Field<string>("FSTR"),
                        //    hscode = r.Field<string>("HSCODE").Trim(),
                        //    cgst = r.Field<decimal>("CGST"),
                        //    sgst = r.Field<decimal>("SGST"),
                        //    igst = r.Field<decimal>("CGST")
                        //})
                        //     .Select(g =>
                        //     {
                        //         var row = dtHSDetails.NewRow();
                        //         row["FSTR"] = g.Key.fstr;
                        //         row["HSCODE_H"] = g.Key.hscode;
                        //         if (g.Key.sgst > 0)
                        //         {
                        //             row["CGST_H"] = g.Key.cgst;
                        //             row["SGST_H"] = g.Key.sgst;
                        //             row["IGST_H"] = 0;
                        //         }
                        //         else
                        //         {
                        //             row["CGST_H"] = 0;
                        //             row["SGST_H"] = 0;
                        //             row["IGST_H"] = g.Key.igst;
                        //         }
                        //         row["AMT"] = g.Sum(r => r.Field<decimal>("IAMOUNT"));
                        //         row["GST_H"] = g.Sum(r => r.Field<decimal>("EXC_AMT")) + g.Sum(r => r.Field<decimal>("CESS_PU"));

                        //         return row;
                        //     }
                        //  ).CopyToDataTable();

                        //dtHSDetails.TableName = "dtHSDetails";
                        //dsRep.Tables.Add(dtHSDetails);
                    }
                    dt.Columns.Add(new DataColumn("amtToword", typeof(string)));
                    dt.Columns.Add(new DataColumn("PkgN", typeof(double)));
                    if (frm_vty == "4F")
                    {
                        dt.Columns.Add("EXP_YR", typeof(string));
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["pkgN"] = fgen.make_double(fgen.getNumericOnly(dr["pkg"].ToString()));
                        dr["amtToword"] = fgen.ConvertNumbertoWords(dr["bill_tot"].ToString().Trim());
                        if (frm_vty == "4F")
                        {
                            yr = nxt.ToString();
                            yr = "EXP/" + dr["vchnum"].ToString().Trim().Substring(2, 4) + "/" + CURR + "-" + yr + "";
                            dr["EXP_YR"] = yr;
                        }
                    }

                    dt.TableName = "Prepcur";
                    repCount = 1;
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));

                    // VIPIN                                        
                    //if (frm_cocd == "PPAP")
                    {
                        dt1 = new DataTable("barcode");
                        dt1.Columns.Add(new DataColumn("img1_desc", typeof(string)));
                        dt1.Columns.Add(new DataColumn("img1", typeof(System.Byte[])));
                        string col2 = "";
                        mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no from type where id='B' and type1='" + dt.Rows[0]["branchcd"].ToString().Trim().Replace("/", "") + "'", "gst_no");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            col1 = dt.Rows[i]["branchcd"].ToString().Trim().Replace("/", "") + "," + dt.Rows[i]["vchnum"].ToString().Trim().Replace("/", "");
                        }
                        fpath = Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "") + ".png");
                        del_file(fpath);
                        if (frm_cocd == "PPAP") fgen.prnt_QRbar(frm_cocd, col2, col1.Replace("*", "").Replace("/", "") + ".png");
                        else if (frm_cocd == "WING")
                        {
                            fpath = Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "").Replace(",", ""));
                            fgen.prnt_2Dbar32bit(frm_cocd, col1, fpath);
                            fpath = fpath + ".bmp";
                            frm_rptName = "std_inv2d";
                        }
                        else fgen.prnt_QRbar(frm_cocd, col1, col1.Replace("*", "").Replace("/", "") + ".png");

                        DataRow dr = dt1.NewRow();
                        FilStr = new FileStream(fpath, FileMode.Open);
                        BinRed = new BinaryReader(FilStr);

                        dr["img1_desc"] = col1.Trim();
                        dr["img1"] = BinRed.ReadBytes((int)BinRed.BaseStream.Length);

                        dt1.Rows.Add(dr);
                        FilStr.Close();
                        BinRed.Close();

                        dsRep.Tables.Add(dt1);
                    }

                    //csmst                
                    //SQuery = "Select distinct d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dt.Rows[0]["cscode"].ToString().Trim() + "'";
                    //dt = new DataTable();
                    //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //if (dt.Rows.Count <= 0)
                    //{
                    //    dt = new DataTable();
                    //    SQuery = "Select 'Same as Recipient' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                    //    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    //}
                    //dt.TableName = "csmst";
                    //dsRep.Tables.Add(dt);

                    // inv terms
                    SQuery = "SELECT DISTINCT COL1 AS POTERMS,SRNO FROM DOCTERMS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' AND DOCTYPE='INV' ORDER BY SRNO";
                    if (iconID == "F50106")
                        SQuery = "SELECT DISTINCT udf_name||' '||udf_value AS POTERMS,SRNO FROM udf_data WHERE BRANCHCD='" + frm_mbr + "' AND PAR_FLD='" + frm_mbr + frm_vty + barCode.Replace("'", "") + "' ORDER BY SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    mq10 = "";
                    dt1 = new DataTable();
                    mdr = null;
                    dt1.Columns.Add("poterms", typeof(string));
                    if (frm_cocd == "STUD")
                    {
                        dt1.Columns.Add("poterms1", typeof(string));
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        mq10 += dt.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
                        if (frm_cocd == "STUD")
                        {
                            if (i > 2)
                            {
                                mq11 += dt.Rows[i]["POTERMS"].ToString().Trim();//+ Environment.NewLine
                            }
                        }
                    }
                    mdr = dt1.NewRow();
                    mdr["poterms"] = mq10;
                    if (frm_cocd == "STUD")
                    {
                        mdr["poterms1"] = mq11;
                    }
                    dt1.Rows.Add(mdr);
                    btnexpwithsig.Visible = true;

                    dt1.TableName = "INV_TERMS";
                    dsRep.Tables.Add(dt1);

                    if (frm_cocd == "STUD")
                    {
                        if (frm_vty == "44")
                        {
                            frm_rptName = "std_inv_stud44";
                        }
                        else if (frm_vty == "43")
                        {
                            frm_rptName = "std_inv_stud43";
                        }
                        else if (frm_vty == "4F")
                        {
                            frm_rptName = "ExpInv_STUD";
                        }
                        else
                        {
                            frm_rptName = "std_inv_stud_all";
                        }
                    }
                    if (frm_vty == "4F")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv_EXP", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                    }
                    else
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                    }
                    // btnexpwithsig_Click(null, null); // FOR DIRECT DOWNLOAD                                       
                }
                else
                {
                    data_found = "N";
                }
                //printDefault(frm_cocd, frm_mbr, "std_invcl", "std_invcl", dsRep, "Invoice Challan");
                #endregion
                break;
            // ------------ MERGE BY MADHVI ON 13TH JAN 2018 , MADE BY YOGITA ---------- //

            case "F50266": //new code 14 feb 2019...yet not merged only pdf is send to client for testing           
                #region Material Lying with Godown  (invoice-summary)

                //for financial year
                int CurrentYear = DateTime.Today.Year;
                int PreviousYear = DateTime.Today.Year - 1;
                int NextYear = DateTime.Today.Year + 1;
                string PreYear = PreviousYear.ToString();
                string NexYear = NextYear.ToString();
                string CurYear = CurrentYear.ToString();
                string FinYear = null;

                if (DateTime.Today.Month > 3)
                    FinYear = CurYear + "-" + NexYear;
                else
                    FinYear = PreYear + "-" + CurYear;
                mq6 = FinYear.Substring(2, 2);
                ///////
                ph_tbl = new DataTable();
                header_n = "Material Lying with Godown(Invoice Wise-Detail)";
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("days", typeof(string));
                ph_tbl.Columns.Add("sno", typeof(string));
                ph_tbl.Columns.Add("acode", typeof(string));
                ph_tbl.Columns.Add("aname", typeof(string));
                ph_tbl.Columns.Add("icode", typeof(string));
                ph_tbl.Columns.Add("iname", typeof(string));
                ph_tbl.Columns.Add("part", typeof(string));
                ph_tbl.Columns.Add("invno", typeof(string));
                ph_tbl.Columns.Add("invdate", typeof(string));
                ph_tbl.Columns.Add("mat_lift_dt", typeof(string));
                ph_tbl.Columns.Add("inv_qty", typeof(double));
                ph_tbl.Columns.Add("lifted_qty", typeof(double));
                ph_tbl.Columns.Add("bal_qty", typeof(double));
                ph_tbl.Columns.Add("rate", typeof(double));
                ph_tbl.Columns.Add("amount", typeof(double));
                mq5 = ""; mq11 = "";
                mq5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col4");
                mq11 = System.DateTime.Now.Date.ToString("dd/MM/yyyy");

                DateTime tim = Convert.ToDateTime(mq11).AddDays(-Convert.ToInt32(mq5));
                xprdRange = " between to_date('05/07/2018','dd/mm/yyyy') and to_date('" + tim.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";
                xprdRange = " between to_date('01/01/2018','dd/mm/yyyy') and to_date('" + tim.ToString("dd/MM/yyyy") + "','dd/mm/yyyy')";  //NEW


                // SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.picked) As picked,sum(a.op)+sum(a.inv)-sum(a.picked) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select trim(vchnum) as vchnum,LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,0 as picked,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,0 as picked,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE " + xprdRange + " and store_no='Y'  union all  select trim(vchnum) as vchnum,null AS VCHDATE,TRIM(icode) AS ICODE,trim(acode) as acode,0 as op,0 as inv,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4,0 AS RATE from multivch where branchcd='" + frm_mbr + "' and type='WH' and vchdate " + xprdRange + " and  trim(acode) ='" + mq1 + "' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink"; //as per bansal sir...rate comes from warehouse master

                SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.op)+sum(a.inv) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE  " + xprdRange + " and store_no='Y' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //main dt

                SQuery = "select icode,acode,sum(picked) as picked,col4 from (select TRIM(icode) AS ICODE,trim(acode) as acode,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH'  and  trim(acode) ='" + mq1 + "') group by acode,icode,col4";
                dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);//qry for lifted qty

                SQuery = "select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD/MM/YYYY') AS VCHDATE,to_char(vchdate,'yyyymmdd') as vdd,TRIM(icode) as icode,trim(acode) as acode,SUM(iqtyout) as inv,0 as picked,vchnum AS wolink,sum(IQTY_CHLWT*IQTYOUT)/sum(iqtyout) AS RATE_VAL from ivoucher where branchcd='" + frm_mbr + "' and type='4F'  and trim(acode) ='" + mq1 + "' and vchdate  " + xprdRange + " and store='Y' GROUP BY TRIM(vchnum),TO_CHAR(vchdate,'DD/MM/YYYY'),to_char(vchdate,'yyyymmdd'),TRIM(icode) ,trim(acode),IQTY_CHLWT,vchnum "; //transaction table
                dt1 = new DataTable();//
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                mq0 = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate"; //old
                mq0 = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate"; //new 4feb19
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; mq2 = ""; mq3 = ""; mq4 = "";
                    dr1 = ph_tbl.NewRow();
                    dr1["header"] = header_n;
                    dr1["days"] = mq5;
                    dr1["sno"] = i + 1;
                    dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                    dr1["aname"] = dt.Rows[i]["aname"].ToString().Trim();
                    dr1["icode"] = dt.Rows[i]["erp_code"].ToString().Trim();
                    dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                    dr1["part"] = dt.Rows[i]["cpartno"].ToString().Trim();
                    mq3 = dt.Rows[i]["inv_link"].ToString().Trim();
                    dr1["invno"] = dt.Rows[i]["inv_link"].ToString().Trim();
                    dr1["invdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                    dr1["mat_lift_dt"] = fgen.seek_iname_dt(dt2, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "' and icode='" + dt.Rows[i]["erp_code"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "vchdate");
                    //dr1["lifted_qty"] = fgen.make_double(dt.Rows[i]["picked"].ToString().Trim());
                    dr1["lifted_qty"] = fgen.make_double(fgen.seek_iname_dt(dt3, "acode='" + dr1["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "picked"));
                    db3 = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                    db4 = fgen.make_double(dt.Rows[i]["rate"].ToString().Trim());
                    dr1["rate"] = db4;
                    dr1["inv_qty"] = db3;
                    dr1["bal_qty"] = fgen.make_double(dr1["inv_qty"].ToString().Trim()) - fgen.make_double(dr1["lifted_qty"].ToString().Trim());
                    db = fgen.make_double(dr1["bal_qty"].ToString().Trim());
                    db2 = db * db1;
                    dr1["amount"] = fgen.make_double(dr1["rate"].ToString().Trim()) * db;
                    if (db != 0)
                    {
                        ph_tbl.Rows.Add(dr1);
                    }
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(ph_tbl, repCount));
                    Print_Report_BYDS_pdf(frm_cocd, frm_mbr, "Mat_Lying_wid_Godown_ERAL_InvWise", "Mat_Lying_wid_Godown_ERAL_InvWise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50267":
                #region
                ////for financial year
                //CurrentYear = DateTime.Today.Year;
                //PreviousYear = DateTime.Today.Year - 1;
                //NextYear = DateTime.Today.Year + 1;
                //PreYear = PreviousYear.ToString();
                //NexYear = NextYear.ToString();
                //CurYear = CurrentYear.ToString();
                //FinYear = null;
                //if (DateTime.Today.Month > 3)
                //    FinYear = CurYear + "-" + NexYear;
                //else
                //    FinYear = PreYear + "-" + CurYear;
                //mq6 = FinYear.Substring(2, 2);
                ////Material Lying with Godown  (item wise-DETAIL)
                //ph_tbl = new DataTable();
                //header_n = "Material Lying with Godown(Item Wise-Detail)";
                //ph_tbl.Columns.Add("header", typeof(string));
                //ph_tbl.Columns.Add("days", typeof(string));
                //ph_tbl.Columns.Add("sno", typeof(string));
                //ph_tbl.Columns.Add("acode", typeof(string));
                //ph_tbl.Columns.Add("aname", typeof(string));
                //ph_tbl.Columns.Add("icode", typeof(string));
                //ph_tbl.Columns.Add("iname", typeof(string));
                //ph_tbl.Columns.Add("part", typeof(string));
                //ph_tbl.Columns.Add("invno", typeof(string));
                //ph_tbl.Columns.Add("invdate", typeof(string));
                //ph_tbl.Columns.Add("mat_lift_dt", typeof(string));
                //ph_tbl.Columns.Add("inv_qty", typeof(double));
                //ph_tbl.Columns.Add("lifted_qty", typeof(double));
                //ph_tbl.Columns.Add("bal_qty", typeof(double));
                //ph_tbl.Columns.Add("rate", typeof(double));
                //ph_tbl.Columns.Add("amount", typeof(double));
                //mq5 = "";
                ////mq5 = fgen.seek_iname(frm_qstr, frm_cDt1, "select to_date('" + fromdt + "','dd/mm/yyyy')-to_date('" + todt + "','dd/mm/yyyy') as days from dual", "days");
                //mq5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                //mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");


                //SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.op)+sum(a.inv) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE  " + xprdRange + " and store_no='Y' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink";
                //dt = new DataTable();
                //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //main dt

                //SQuery = "select icode,acode,sum(picked) as picked,col4 from (select TRIM(icode) AS ICODE,trim(acode) as acode,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH'  and  trim(acode) ='" + mq1 + "') group by acode,icode,col4";
                //dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);//qry for lifted qty

                //mq0 = "select  vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE ='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate";
                //dt2 = new DataTable();
                //dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; mq2 = ""; mq3 = "";
                //    dr1 = ph_tbl.NewRow();
                //    dr1["header"] = header_n;
                //    dr1["sno"] = i + 1;
                //    dr1["days"] = mq5;
                //    dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                //    dr1["aname"] = dt.Rows[i]["aname"].ToString().Trim();
                //    dr1["icode"] = dt.Rows[i]["erp_code"].ToString().Trim();
                //    dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                //    dr1["part"] = dt.Rows[i]["cpartno"].ToString().Trim();
                //    dr1["invno"] = dt.Rows[i]["inv_link"].ToString().Trim();
                //    dr1["invdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                //    dr1["mat_lift_dt"] = fgen.seek_iname_dt(dt2, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "vchdate");
                //    dr1["inv_qty"] = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                //    //   dr1["lifted_qty"] = fgen.make_double(dt.Rows[i]["picked"].ToString().Trim());
                //    dr1["lifted_qty"] = fgen.make_double(fgen.seek_iname_dt(dt3, "acode='" + dr1["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "picked"));

                //    db3 = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                //    db4 = fgen.make_double(dt.Rows[i]["rate"].ToString().Trim());
                //    dr1["rate"] = db4;
                //    dr1["inv_qty"] = db3;
                //    dr1["bal_qty"] = fgen.make_double(dr1["inv_qty"].ToString().Trim()) - fgen.make_double(dr1["lifted_qty"].ToString().Trim());
                //    db = fgen.make_double(dr1["bal_qty"].ToString().Trim());
                //    db2 = db * db1;
                //    dr1["amount"] = fgen.make_double(dr1["rate"].ToString().Trim()) * db;
                //    if (db != 0)
                //    {
                //        ph_tbl.Rows.Add(dr1);
                //    }
                //}
                //if (ph_tbl.Rows.Count > 0)
                //{
                //    ph_tbl.TableName = "Prepcur";
                //    dsRep.Tables.Add(fgen.mTitle(ph_tbl, repCount));
                //    Print_Report_BYDS_pdf(frm_cocd, frm_mbr, "Mat_Lying_wid_Godown_ERAL", "Mat_Lying_wid_Godown_ERAL", dsRep, header_n);
                //}
                //else
                //{
                //    data_found = "N";
                //}
                #endregion
                #region Material Lying with Godown  (item wise-Detail)

                ph_tbl = new DataTable();
                header_n = "Material Lying with Godown(item wise-Detail)";
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("days", typeof(string));
                ph_tbl.Columns.Add("sno", typeof(string));
                ph_tbl.Columns.Add("acode", typeof(string));
                ph_tbl.Columns.Add("aname", typeof(string));
                ph_tbl.Columns.Add("icode", typeof(string));
                ph_tbl.Columns.Add("iname", typeof(string));
                ph_tbl.Columns.Add("part", typeof(string));
                ph_tbl.Columns.Add("invno", typeof(string));
                ph_tbl.Columns.Add("invdate", typeof(string));
                ph_tbl.Columns.Add("mat_lift_dt", typeof(string));
                ph_tbl.Columns.Add("inv_qty", typeof(double));
                ph_tbl.Columns.Add("lifted_qty", typeof(double));
                ph_tbl.Columns.Add("bal_qty", typeof(double));
                ph_tbl.Columns.Add("rate", typeof(double));
                ph_tbl.Columns.Add("amount", typeof(double));
                ph_tbl.Columns.Add("rep_dt", typeof(string));
                mq5 = ""; mq11 = ""; mq1 = ""; mq10 = "";
                mq5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                mq10 = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                mq11 = fgen.seek_iname(frm_qstr, frm_cDt1, "select to_date('" + mq10 + "','dd/mm/yyyy')-to_date('" + todt + "','dd/mm/yyyy') as days from dual", "days");

                xprdRange = " between to_date('01/01/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";  //NEW


                // SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.picked) As picked,sum(a.op)+sum(a.inv)-sum(a.picked) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select trim(vchnum) as vchnum,LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,0 as picked,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,0 as picked,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE " + xprdRange + " and store_no='Y'  union all  select trim(vchnum) as vchnum,null AS VCHDATE,TRIM(icode) AS ICODE,trim(acode) as acode,0 as op,0 as inv,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4,0 AS RATE from multivch where branchcd='" + frm_mbr + "' and type='WH' and vchdate " + xprdRange + " and  trim(acode) ='" + mq1 + "' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink"; //as per bansal sir...rate comes from warehouse master

                SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.op)+sum(a.inv) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE  " + xprdRange + " and store_no='Y' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //main dt

                SQuery = "select icode,acode,sum(picked) as picked,col4 from (select TRIM(icode) AS ICODE,trim(acode) as acode,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH'  and  trim(acode) ='" + mq1 + "') group by acode,icode,col4";
                dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);//qry for lifted qty

                SQuery = "select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD/MM/YYYY') AS VCHDATE,to_char(vchdate,'yyyymmdd') as vdd,TRIM(icode) as icode,trim(acode) as acode,SUM(iqtyout) as inv,0 as picked,vchnum AS wolink,sum(IQTY_CHLWT*IQTYOUT)/sum(iqtyout) AS RATE_VAL from ivoucher where branchcd='" + frm_mbr + "' and type='4F'  and trim(acode) ='" + mq1 + "' and vchdate  " + xprdRange + " and store='Y' GROUP BY TRIM(vchnum),TO_CHAR(vchdate,'DD/MM/YYYY'),to_char(vchdate,'yyyymmdd'),TRIM(icode) ,trim(acode),IQTY_CHLWT,vchnum "; //transaction table
                dt1 = new DataTable();//
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                mq0 = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate"; //old
                mq0 = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate"; //new 4feb19
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; mq2 = ""; mq3 = ""; mq4 = "";
                    dr1 = ph_tbl.NewRow();
                    dr1["header"] = header_n;
                    dr1["days"] = mq11;
                    dr1["sno"] = i + 1;
                    dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                    dr1["aname"] = dt.Rows[i]["aname"].ToString().Trim();
                    dr1["icode"] = dt.Rows[i]["erp_code"].ToString().Trim();
                    dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                    dr1["part"] = dt.Rows[i]["cpartno"].ToString().Trim();
                    mq3 = dt.Rows[i]["inv_link"].ToString().Trim();
                    dr1["invno"] = dt.Rows[i]["inv_link"].ToString().Trim();
                    dr1["invdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                    dr1["mat_lift_dt"] = fgen.seek_iname_dt(dt2, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "' and icode='" + dt.Rows[i]["erp_code"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "vchdate");
                    //dr1["lifted_qty"] = fgen.make_double(dt.Rows[i]["picked"].ToString().Trim());
                    dr1["lifted_qty"] = fgen.make_double(fgen.seek_iname_dt(dt3, "acode='" + dr1["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "picked"));
                    db3 = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                    db4 = fgen.make_double(dt.Rows[i]["rate"].ToString().Trim());
                    dr1["rate"] = db4;
                    dr1["inv_qty"] = db3;
                    dr1["bal_qty"] = fgen.make_double(dr1["inv_qty"].ToString().Trim()) - fgen.make_double(dr1["lifted_qty"].ToString().Trim());
                    db = fgen.make_double(dr1["bal_qty"].ToString().Trim());
                    db2 = db * db1;
                    dr1["amount"] = fgen.make_double(dr1["rate"].ToString().Trim()) * db;
                    dr1["rep_dt"] = Convert.ToString(todt);
                    if (db != 0)
                    {
                        ph_tbl.Rows.Add(dr1);
                    }
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(ph_tbl, repCount));
                    Print_Report_BYDS_pdf(frm_cocd, frm_mbr, "Mat_Lying_wid_Godown_ERAL", "Mat_Lying_wid_Godown_ERAL", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50268":
                #region this table for detail
                //ph_tbl = new DataTable();
                //header_n = "Material Lying with Godown(Item Wise-Concise)";
                //ph_tbl.Columns.Add("header", typeof(string));
                //ph_tbl.Columns.Add("days", typeof(string));
                //ph_tbl.Columns.Add("sno", typeof(string));
                //ph_tbl.Columns.Add("acode", typeof(string));
                //ph_tbl.Columns.Add("aname", typeof(string));
                //ph_tbl.Columns.Add("icode", typeof(string));
                //ph_tbl.Columns.Add("iname", typeof(string));
                //ph_tbl.Columns.Add("part", typeof(string));
                //ph_tbl.Columns.Add("invno", typeof(string));
                //ph_tbl.Columns.Add("invdate", typeof(string));
                //ph_tbl.Columns.Add("mat_lift_dt", typeof(string));
                //ph_tbl.Columns.Add("inv_qty", typeof(double));
                //ph_tbl.Columns.Add("lifted_qty", typeof(double));
                //ph_tbl.Columns.Add("bal_qty", typeof(double));
                //ph_tbl.Columns.Add("rate", typeof(double));
                //ph_tbl.Columns.Add("amount", typeof(double));
                /////// dtm table using for sumamry
                //dtm = new DataTable();
                //dtm.Columns.Add("header", typeof(string));
                //dtm.Columns.Add("days", typeof(string));
                //dtm.Columns.Add("sno", typeof(string));
                //dtm.Columns.Add("acode", typeof(string));
                //dtm.Columns.Add("aname", typeof(string));
                //dtm.Columns.Add("icode", typeof(string));
                //dtm.Columns.Add("iname", typeof(string));
                //dtm.Columns.Add("part", typeof(string));
                //dtm.Columns.Add("invno", typeof(string));
                //dtm.Columns.Add("invdate", typeof(string));
                //dtm.Columns.Add("mat_lift_dt", typeof(string));
                //dtm.Columns.Add("inv_qty", typeof(double));
                //dtm.Columns.Add("lifted_qty", typeof(double));
                //dtm.Columns.Add("bal_qty", typeof(double));
                //dtm.Columns.Add("rate", typeof(double));
                //dtm.Columns.Add("amount", typeof(double));
                /////
                //mq5 = "";
                //mq5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                //mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                ////SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.picked) As picked,sum(a.op)+sum(a.inv)-sum(a.picked) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,max(A.vchdate) as vchdate,max(a.vdd) as vdd1,max(a.rate_val) as rate,trim(a.acode) as acode,trim(c.aname) as aname from (select trim(vchnum) as vchnum,LOC_REF AS VCHDATE, LOC_REF as vdd, trim(icode) as icode,trim(maincode) as acode,sum(iqtyin) as op,0 as inv,0 as picked,wolink,sum(ngqty*IQTYIN)/sum(iqtyin) AS RATE_VAL from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' group by trim(vchnum) ,LOC_REF ,trim(icode) ,trim(maincode) ,wolink union all  select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD/MM/YYYY') AS VCHDATE,to_char(vchdate,'yyyymmdd') as vdd,TRIM(icode) as icode,trim(acode) as acode,0 as op,SUM(iqtyout) as inv,0 as picked,vchnum AS wolink,sum(IQTY_CHLWT*IQTYOUT)/sum(iqtyout) AS RATE_VAL from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and vchdate " + xprdRange + " and trim(acode) ='" + mq1 + "' and store_no='Y' GROUP BY TRIM(vchnum),TO_CHAR(vchdate,'DD/MM/YYYY'),to_char(vchdate,'yyyymmdd'),TRIM(icode) ,trim(acode),IQTY_CHLWT,vchnum  union all   select trim(vchnum) as vchnum,null AS VCHDATE, null as vdd,TRIM(icode) AS ICODE,trim(acode) as acode,0 as op,0 as inv,qty1 as picked,col4 as col4,0 AS RATE_VAL from multivch where branchcd='" + frm_mbr + "' and type='WH' and vchdate  " + xprdRange + "  and  trim(acode) ='" + mq1 + "') a,item b,famst c  where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vdd1,wolink ";               
                ////new 14 FEB 2019
                //SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.op)+sum(a.inv) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE  " + xprdRange + " and store_no='Y' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink";
                //dt = new DataTable();
                //dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //main dt

                //SQuery = "select icode,acode,sum(picked) as picked,col4 from (select TRIM(icode) AS ICODE,trim(acode) as acode,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH'  and  trim(acode) ='" + mq1 + "') group by acode,icode,col4";
                //dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);//qry for lifted qty

                //mq0 = "select  vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE ='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate";
                //dt2 = new DataTable();
                //dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; mq2 = ""; mq3 = "";
                //    dr1 = ph_tbl.NewRow();
                //    dr1["header"] = header_n;
                //    dr1["sno"] = i + 1;
                //    dr1["days"] = mq5;
                //    dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                //    dr1["aname"] = dt.Rows[i]["aname"].ToString().Trim();
                //    dr1["icode"] = dt.Rows[i]["erp_code"].ToString().Trim();
                //    dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                //    dr1["part"] = dt.Rows[i]["cpartno"].ToString().Trim();
                //    dr1["invno"] = dt.Rows[i]["inv_link"].ToString().Trim();
                //    dr1["invdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                //    dr1["mat_lift_dt"] = fgen.seek_iname_dt(dt2, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "vchdate");
                //    dr1["inv_qty"] = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                //    //dr1["lifted_qty"] = fgen.make_double(dt.Rows[i]["picked"].ToString().Trim());
                //    dr1["lifted_qty"] = fgen.make_double(fgen.seek_iname_dt(dt3, "acode='" + dr1["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "picked"));
                //    db3 = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                //    db4 = fgen.make_double(dt.Rows[i]["rate"].ToString().Trim());
                //    dr1["rate"] = db4;
                //    dr1["inv_qty"] = db3;
                //    dr1["bal_qty"] = fgen.make_double(dr1["inv_qty"].ToString().Trim()) - fgen.make_double(dr1["lifted_qty"].ToString().Trim());
                //    db = fgen.make_double(dr1["bal_qty"].ToString().Trim());
                //    db2 = db * db1;
                //    dr1["amount"] = fgen.make_double(dr1["rate"].ToString().Trim()) * db;
                //    if (db != 0)
                //    {
                //        ph_tbl.Rows.Add(dr1);
                //    }
                //}

                //if (ph_tbl.Rows.Count > 0)
                //{
                //    DataView view1im = new DataView(ph_tbl);
                //    DataTable dtdrsim = new DataTable();
                //    dtdrsim = view1im.ToTable(true, "ACODE", "icode"); //MAIN                  
                //    foreach (DataRow dr0 in dtdrsim.Rows)
                //    {
                //        DataView view2 = new DataView(ph_tbl, "acode='" + dr0["acode"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                //        dt5 = new DataTable();
                //        dt5 = view2.ToTable();
                //        db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; mq2 = ""; mq3 = "";
                //        dr1 = dtm.NewRow();
                //        for (int i = 0; i < dt5.Rows.Count; i++)
                //        {
                //            dr1["header"] = header_n;
                //            dr1["days"] = dt5.Rows[i]["days"].ToString().Trim();
                //            dr1["sno"] = i + 1;
                //            dr1["acode"] = dt5.Rows[i]["acode"].ToString().Trim();
                //            dr1["aname"] = dt5.Rows[i]["aname"].ToString().Trim();
                //            dr1["icode"] = dt5.Rows[i]["icode"].ToString().Trim();
                //            dr1["iname"] = dt5.Rows[i]["iname"].ToString().Trim();
                //            dr1["part"] = dt5.Rows[i]["part"].ToString().Trim();
                //            dr1["mat_lift_dt"] = dt5.Rows[i]["mat_lift_dt"].ToString().Trim();
                //            db3 += fgen.make_double(dt5.Rows[i]["inv_qty"].ToString().Trim());
                //            dr1["inv_qty"] = db3;
                //            db4 += fgen.make_double(dt5.Rows[i]["lifted_qty"].ToString().Trim());
                //            dr1["lifted_qty"] = db4;
                //            db5 += fgen.make_double(dt5.Rows[i]["bal_qty"].ToString().Trim());
                //            dr1["bal_qty"] = db5;
                //            dr1["rate"] = fgen.make_double(dt5.Rows[i]["rate"].ToString().Trim());
                //            db = fgen.make_double(dt5.Rows[i]["bal_qty"].ToString().Trim());
                //            db1 = fgen.make_double(dt5.Rows[i]["rate"].ToString().Trim());
                //            db2 += db * db1;
                //            dr1["amount"] = db2;
                //        }
                //        if (db != 0)
                //        {
                //            dtm.Rows.Add(dr1);
                //        }
                //    }
                //}
                //if (dtm.Rows.Count > 0)
                //{
                //    dtm.TableName = "Prepcur";
                //    dsRep.Tables.Add(fgen.mTitle(dtm, repCount));
                //    Print_Report_BYDS_pdf(frm_cocd, frm_mbr, "Mat_Lying_wid_Godown_ERAL_ItemWise", "Mat_Lying_wid_Godown_ERAL_ItemWise", dsRep, header_n);
                //}
                //else
                //{
                //    data_found = "N";
                //}
                #endregion
                #region Material Lying with Godown  (Item Wise-Summary)

                ph_tbl = new DataTable();
                header_n = "Material Lying with Godown(Item Wise-Summary)";
                ph_tbl.Columns.Add("header", typeof(string));
                ph_tbl.Columns.Add("days", typeof(string));
                ph_tbl.Columns.Add("sno", typeof(string));
                ph_tbl.Columns.Add("acode", typeof(string));
                ph_tbl.Columns.Add("aname", typeof(string));
                ph_tbl.Columns.Add("icode", typeof(string));
                ph_tbl.Columns.Add("iname", typeof(string));
                ph_tbl.Columns.Add("part", typeof(string));
                ph_tbl.Columns.Add("invno", typeof(string));
                ph_tbl.Columns.Add("invdate", typeof(string));
                ph_tbl.Columns.Add("mat_lift_dt", typeof(string));
                ph_tbl.Columns.Add("inv_qty", typeof(double));
                ph_tbl.Columns.Add("lifted_qty", typeof(double));
                ph_tbl.Columns.Add("bal_qty", typeof(double));
                ph_tbl.Columns.Add("rate", typeof(double));
                ph_tbl.Columns.Add("amount", typeof(double));
                ph_tbl.Columns.Add("rep_dt", typeof(string));
                ///// dtm table using for sumamry
                dtm = new DataTable();
                dtm.Columns.Add("header", typeof(string));
                dtm.Columns.Add("days", typeof(string));
                dtm.Columns.Add("sno", typeof(string));
                dtm.Columns.Add("acode", typeof(string));
                dtm.Columns.Add("aname", typeof(string));
                dtm.Columns.Add("icode", typeof(string));
                dtm.Columns.Add("iname", typeof(string));
                dtm.Columns.Add("part", typeof(string));
                dtm.Columns.Add("invno", typeof(string));
                dtm.Columns.Add("invdate", typeof(string));
                dtm.Columns.Add("mat_lift_dt", typeof(string));
                dtm.Columns.Add("inv_qty", typeof(double));
                dtm.Columns.Add("lifted_qty", typeof(double));
                dtm.Columns.Add("bal_qty", typeof(double));
                dtm.Columns.Add("rate", typeof(double));
                dtm.Columns.Add("amount", typeof(double));
                dtm.Columns.Add("rep_dt", typeof(string));
                ///
                mq5 = ""; mq11 = ""; mq1 = ""; mq10 = "";
                mq5 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL3");
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                mq10 = System.DateTime.Now.Date.ToString("dd/MM/yyyy");
                mq11 = fgen.seek_iname(frm_qstr, frm_cDt1, "select to_date('" + mq10 + "','dd/mm/yyyy')-to_date('" + todt + "','dd/mm/yyyy') as days from dual", "days");

                xprdRange = " between to_date('01/01/2018','dd/mm/yyyy') and to_date('" + todt + "','dd/mm/yyyy')";  //NEW


                // SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.picked) As picked,sum(a.op)+sum(a.inv)-sum(a.picked) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select trim(vchnum) as vchnum,LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,0 as picked,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,0 as picked,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE " + xprdRange + " and store_no='Y'  union all  select trim(vchnum) as vchnum,null AS VCHDATE,TRIM(icode) AS ICODE,trim(acode) as acode,0 as op,0 as inv,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4,0 AS RATE from multivch where branchcd='" + frm_mbr + "' and type='WH' and vchdate " + xprdRange + " and  trim(acode) ='" + mq1 + "' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink"; //as per bansal sir...rate comes from warehouse master

                SQuery = "select trim(b.iname) as iname,trim(b.cpartno) as cpartno,sum(a.op) as Opening,sum(a.op)+sum(a.inv) as Inv_Qty,sum(a.op)+sum(a.inv) as Bal_Qty,a.wolink as Inv_link,trim(a.icode) as ERp_code,min(A.vchdate) as vchdate,max(A.RATE) as rate,trim(a.acode) as acode,trim(c.aname) as aname from ( select LOC_REF AS VCHDATE,trim(icode) as icode,trim(maincode) as acode,iqtyin as op,0 as inv,(case when  wolink LIKE 'ERAL%' then SUBSTR(WOLINK,6,2)||'0'||SUBSTR(WOLINK,12,3) else wolink end ) as wolink,(ngqty*IQTYIN)/(iqtyin) AS RATE from wipstk where branchcd='" + frm_mbr + "' and type='WH' and trim(maincode) ='" + mq1 + "' union all  select TO_CHAR(vchdate,'DD.MM.YYYY') AS VCHDATE,TRIM(icode) as icode,trim(acode) as acode,0 as op,iqtyout as inv,vchnum AS wolink,(IQTY_CHLWT*IQTYOUT)/(iqtyout) AS RATE from ivoucher where branchcd='" + frm_mbr + "' and type='4F' and trim(acode) ='" + mq1 + "' AND VCHDATE  " + xprdRange + " and store_no='Y' )a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode)  group by trim(b.iname),trim(b.cpartno),a.wolink ,trim(a.icode) ,trim(a.acode),trim(c.aname) order by iname,vchdate,wolink";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery); //main dt

                SQuery = "select icode,acode,sum(picked) as picked,col4 from (select TRIM(icode) AS ICODE,trim(acode) as acode,qty1 as picked,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH'  and  trim(acode) ='" + mq1 + "') group by acode,icode,col4";
                dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery);//qry for lifted qty

                SQuery = "select TRIM(vchnum) AS VCHNUM,TO_CHAR(vchdate,'DD/MM/YYYY') AS VCHDATE,to_char(vchdate,'yyyymmdd') as vdd,TRIM(icode) as icode,trim(acode) as acode,SUM(iqtyout) as inv,0 as picked,vchnum AS wolink,sum(IQTY_CHLWT*IQTYOUT)/sum(iqtyout) AS RATE_VAL from ivoucher where branchcd='" + frm_mbr + "' and type='4F'  and trim(acode) ='" + mq1 + "' and vchdate  " + xprdRange + " and store='Y' GROUP BY TRIM(vchnum),TO_CHAR(vchdate,'DD/MM/YYYY'),to_char(vchdate,'yyyymmdd'),TRIM(icode) ,trim(acode),IQTY_CHLWT,vchnum "; //transaction table
                dt1 = new DataTable();//
                dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                mq0 = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate"; //old
                mq0 = "select vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,acode,icode,(case when  col4 LIKE 'ERAL%' then SUBSTR(col4,6,2)||'0'||SUBSTR(col4,12,3) else col4 end ) AS col4  from multivch where branchcd='" + frm_mbr + "' and type='WH' AND ACODE='" + mq1 + "' and vchdate " + xprdRange + " order by vchdate"; //new 4feb19
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; mq2 = ""; mq3 = ""; mq4 = "";
                    dr1 = ph_tbl.NewRow();
                    dr1["header"] = header_n;
                    dr1["days"] = mq11;
                    dr1["sno"] = i + 1;
                    dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                    dr1["aname"] = dt.Rows[i]["aname"].ToString().Trim();
                    dr1["icode"] = dt.Rows[i]["erp_code"].ToString().Trim();
                    dr1["iname"] = dt.Rows[i]["iname"].ToString().Trim();
                    dr1["part"] = dt.Rows[i]["cpartno"].ToString().Trim();
                    mq3 = dt.Rows[i]["inv_link"].ToString().Trim();
                    dr1["invno"] = dt.Rows[i]["inv_link"].ToString().Trim();
                    dr1["invdate"] = dt.Rows[i]["vchdate"].ToString().Trim();
                    dr1["mat_lift_dt"] = fgen.seek_iname_dt(dt2, "acode='" + dt.Rows[i]["acode"].ToString().Trim() + "' and icode='" + dt.Rows[i]["erp_code"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "vchdate");
                    //dr1["lifted_qty"] = fgen.make_double(dt.Rows[i]["picked"].ToString().Trim());
                    dr1["lifted_qty"] = fgen.make_double(fgen.seek_iname_dt(dt3, "acode='" + dr1["acode"].ToString().Trim() + "' and icode='" + dr1["icode"].ToString().Trim() + "' and col4='" + dr1["invno"].ToString().Trim() + "'", "picked"));
                    db3 = fgen.make_double(dt.Rows[i]["inv_qty"].ToString().Trim());
                    db4 = fgen.make_double(dt.Rows[i]["rate"].ToString().Trim());
                    dr1["rate"] = db4;
                    dr1["inv_qty"] = db3;
                    dr1["bal_qty"] = fgen.make_double(dr1["inv_qty"].ToString().Trim()) - fgen.make_double(dr1["lifted_qty"].ToString().Trim());
                    db = fgen.make_double(dr1["bal_qty"].ToString().Trim());
                    db2 = db * db1;
                    dr1["amount"] = fgen.make_double(dr1["rate"].ToString().Trim()) * db;
                    dr1["rep_dt"] = Convert.ToString(todt);
                    if (db != 0)
                    {
                        ph_tbl.Rows.Add(dr1);
                    }
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    DataView view1im = new DataView(ph_tbl);
                    DataTable dtdrsim = new DataTable();
                    dtdrsim = view1im.ToTable(true, "ACODE", "icode"); //MAIN                  
                    foreach (DataRow dr0 in dtdrsim.Rows)
                    {
                        DataView view2 = new DataView(ph_tbl, "acode='" + dr0["acode"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt5 = new DataTable();
                        dt5 = view2.ToTable();
                        db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; mq2 = ""; mq3 = "";
                        dr1 = dtm.NewRow();
                        for (int i = 0; i < dt5.Rows.Count; i++)
                        {
                            dr1["header"] = header_n;
                            dr1["days"] = dt5.Rows[i]["days"].ToString().Trim();
                            dr1["sno"] = i + 1;
                            dr1["acode"] = dt5.Rows[i]["acode"].ToString().Trim();
                            dr1["aname"] = dt5.Rows[i]["aname"].ToString().Trim();
                            dr1["icode"] = dt5.Rows[i]["icode"].ToString().Trim();
                            dr1["iname"] = dt5.Rows[i]["iname"].ToString().Trim();
                            dr1["part"] = dt5.Rows[i]["part"].ToString().Trim();
                            dr1["mat_lift_dt"] = dt5.Rows[i]["mat_lift_dt"].ToString().Trim();
                            db3 += fgen.make_double(dt5.Rows[i]["inv_qty"].ToString().Trim());
                            dr1["inv_qty"] = db3;
                            db4 += fgen.make_double(dt5.Rows[i]["lifted_qty"].ToString().Trim());
                            dr1["lifted_qty"] = db4;
                            db5 += fgen.make_double(dt5.Rows[i]["bal_qty"].ToString().Trim());
                            dr1["bal_qty"] = db5;
                            dr1["rate"] = fgen.make_double(dt5.Rows[i]["rate"].ToString().Trim());
                            db = fgen.make_double(dt5.Rows[i]["bal_qty"].ToString().Trim());
                            db1 = fgen.make_double(dt5.Rows[i]["rate"].ToString().Trim());
                            db2 += db * db1;
                            dr1["amount"] = db2;
                            dr1["rep_dt"] = dt5.Rows[i]["rep_dt"].ToString().Trim();
                        }
                        if (db != 0)
                        {
                            dtm.Rows.Add(dr1);
                        }
                    }
                }
                if (dtm.Rows.Count > 0)
                {
                    dtm.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dtm, repCount));
                    Print_Report_BYDS_pdf(frm_cocd, frm_mbr, "Mat_Lying_wid_Godown_ERAL_ItemWise", "Mat_Lying_wid_Godown_ERAL_ItemWise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50141":
                // Sales Register (Dom.)
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "select DISTINCT '" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,(a.exc_amt+a.cess_pu) as tax_val,a.*, TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum)||TRIM(A.TYPE) AS GRP ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,i.iname,i.cpartno as c_cpartno,i.unit as iunit1,c.exc_not_no,c.no_bdls as Cno_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,C.tcsamt,c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate  from ivoucher a,sale c,famst b,item i where trim(a.BRANCHCD)||trim(a.TYPE)||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= trim(c.BRANCHCD)||trim(c.TYPE)||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(i.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type IN (" + frm_vty + ")  AND A.vchdate " + xprdRange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ORDER BY A.morder";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sale_REG", "std_Sale_REG", dsRep, "Sales Register (Dom.)");
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50142":
                #region
                // Cust. Wise Register (Dom.)
                //mq12 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                //if (mq12.Length > 0)
                //{
                // SQuery = "select DISTINCT '" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,'Customer Wise Sales Report (Dom.)' as header, a.*, trim(a.acode) AS GRP,TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum) as vdd,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,i.iname,i.cpartno as c_cpartno,i.unit as iunit1  from ivoucher a,famst b,item i where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(i.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type='" + frm_vty + "' AND A.vchdate " + xprdRange + " and a.acode in (" + mq12 + ") ORDER BY vdd,A.morder";
                SQuery = "select DISTINCT '" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,(a.exc_amt+a.cess_pu) as tax_val,'Customer Wise Sales Report (Dom.)' as header, a.*, trim(a.acode) AS GRP,TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum) as vdd ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,i.iname,i.cpartno as c_cpartno,i.unit as iunit1,c.exc_not_no,c.no_bdls as Cno_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,C.tcsamt,c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate  from ivoucher a,sale c,famst b,item i where trim(a.BRANCHCD)||trim(a.TYPE)||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= trim(c.BRANCHCD)||trim(c.TYPE)||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(i.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type in (" + frm_vty + ") AND A.vchdate " + xprdRange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ORDER BY A.morder";
                //}
                //else
                //{
                //    SQuery = "select DISTINCT '" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,(a.exc_amt+a.cess_pu) as tax_val,'Customer Wise Sales Report (Dom.)' as header, a.*, trim(a.acode) AS GRP,TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum) as vdd ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,i.iname,i.cpartno as c_cpartno,i.unit as iunit1,c.exc_not_no,c.no_bdls as Cno_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,C.tcsamt,c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate  from ivoucher a,sale c,famst b,item i where trim(a.BRANCHCD)||trim(a.TYPE)||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= trim(c.BRANCHCD)||trim(c.TYPE)||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(i.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type in (" + frm_vty + ") AND A.vchdate " + xprdRange + " and a.acode like '%' ORDER BY A.morder";
                //}
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Party_Sale_REG", "std_Party_Sale_REG", dsRep, "Party Wise Sales Register (Dom.)");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50143":
                #region
                // Product Wise Register (Dom.)
                // mq12 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                //if (mq12.Length > 0)
                //{
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "select '" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,(a.exc_amt+a.cess_pu) as tax_val,'Product Wise Sales Report (Dom.)' as header, a.*, trim(a.icode) AS GRP,TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum) as vdd ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,i.iname,i.cpartno as c_cpartno,i.unit as iunit1,c.exc_not_no,c.no_bdls as Cno_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,C.tcsamt,c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate  from ivoucher a,sale c,famst b,item i where trim(a.BRANCHCD)||trim(a.TYPE)||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= trim(c.BRANCHCD)||trim(c.TYPE)||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(i.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type in (" + frm_vty + ") AND A.vchdate " + xprdRange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'  ORDER BY A.icode,a.vchdate,a.vchnum";
                //}
                //else
                //{
                //    SQuery = "select DISTINCT '" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,(a.exc_amt+a.cess_pu) as tax_val,'Product Wise Sales Report (Dom.)' as header, a.*, trim(a.icode) AS GRP,TO_CHAR(A.vchdate,'YYYYMMDD')||TRIM(A.vchnum) as vdd ,b.aname,b.ADDR1 AS ADRES1,b.ADDR2 AS CADDRES,b.ADDR3 AS CADRES3,i.iname,i.cpartno as c_cpartno,i.unit as iunit1,c.exc_not_no,c.no_bdls as Cno_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,C.tcsamt,c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate  from ivoucher a,sale c,famst b,item i where trim(a.BRANCHCD)||trim(a.TYPE)||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= trim(c.BRANCHCD)||trim(c.TYPE)||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(i.icode) and A.BRANCHCD='" + frm_mbr + "'  AND a.type in (" + frm_vty + ") AND A.vchdate " + xprdRange + " and a.icode like '%' and nvl(a.iqtyout,0)>0 ORDER BY A.morder";
                //}
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Item_Sale_REG", "std_Item_Sale_REG", dsRep, "Item Wise Sales Register (Dom.)");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50144":
                #region
                // Domestic Proforma Invoice Print
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                SQuery = "select distinct A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,trim(a.desc_) as desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,(a.iamount*round(a.exc_RATE/100,3)) as cgst_val,a.cess_percent as sgst,(a.iamount*round(a.cess_percent/100,3)) as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,t.type1,t1.name,a.vchdate as vdd from ivoucherp a,item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' AND trim(a.icode)=trim(d.icode)  AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY') in (" + mq1 + ") order by vdd,a.vchnum,a.morder";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add(new DataColumn("PkgN", typeof(double)));
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    SQuery = "select c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as  remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,'0' as tcsamt  from salep c where  TRIM(c.BRANCHCD)||TRIM(c.TYPE)||TRIM(c.VCHNUM)||TO_CHAR(c.VCHDATE,'DD/MM/YYYY') in (" + mq1 + ") ";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt.TableName = "SALES_P";
                    dsRep.Tables.Add(dt);
                    //-----------------------------------------------csmst      
                    SQuery = "Select distinct d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dsRep.Tables[1].Rows[0]["cscode"].ToString().Trim() + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count <= 0)
                    {
                        dt = new DataTable();
                        SQuery = "Select 'Same as Recipient' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
                        SQuery = "SELECT ANAME AS consign ,ADDR1 as daddr1,ADDR2 as daddr2,ADDR3 as daddr3,ADDR4 daddr4,'-' as dtel,'-' as dtinno,'-' as dcstno,acode as mycode,staten as dstaten,gst_no as dgst_no,girno as dpanno,substr(gst_no,0,2) as dstatecode FROM FAMST WHERE ACODE='" + dsRep.Tables[0].Rows[0]["acode"].ToString().Trim() + "'";
                        dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    }
                    dt.TableName = "csmst";
                    dsRep.Tables.Add(dt);
                    // inv terms
                    SQuery = "SELECT DISTINCT COL1 AS POTERMS,SRNO FROM DOCTERMS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' AND DOCTYPE='INV' ORDER BY SRNO";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                    mq10 = "";
                    dt1 = new DataTable();
                    DataRow dr = null;
                    dt1.Columns.Add("poterms", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        mq10 += dt.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
                    }
                    dr = dt1.NewRow();
                    dr["poterms"] = mq10;
                    dt1.Rows.Add(dr);
                    dt1.TableName = "INV_TERMS";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv_PI", "std_inv_PI", dsRep, header_n);
                    Session["mydataset"] = dsRep;
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            // ------------ MERGE BY MADHVI ON 11TH JAN 2018 , MADE BY YOGITA ---------- //

            case "F50222":
                header_n = "Party Wise Total Sale (DOM)";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, B.NAME AS SALESNAME,A.TYPE,A.ACODE,C.ANAME,SUM(BASIC) AS BASIC,SUM(SGST) AS SGST,SUM(IGST) AS IGST,SUM(CGST) AS CGST  FROM(SELECT  A.type,a.acode , A.AMT_SALE AS BASIC,(Case when POST='I' then A.AMT_EXC else 0 end) as IGST,(Case when POST='C' then A.AMT_EXC else 0 end) as CGST,(Case when POST='C' then A.AMT_EXC else 0 end) as SGST FROM SALE A WHERE A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '4%' AND A.VCHDATE " + xprdRange + " and a.acode like '" + party_cd + "%') A ,FAMST C,TYPE B WHERE  TRIM(A.ACODE)=TRIM(C.ACODE) AND TRIM(A.TYPE)=TRIM(B.TYPE1) AND B.ID='V' group by b.name,a.type,a.acode,C.ANAME ORDER BY A.ACODE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Party_Sale_DOM", "std_Party_Sale_DOM", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50223":
                header_n = "Product Wise Total Sale (DOM)";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                // EARLIER IT WAS ROUND(SUM(A.IAMOUNT)/SUM(A.IQTYOUT),2) AS IRATE BUT CHANGED TO (CASE WHEN SUM(IQTYOUT)> 0 THEN ROUND(SUM(A.IAMOUNT)/SUM(A.IQTYOUT),2) ELSE 0 END)AS IRATE BECAUSE IT IS GIVING DIVISOR IS EQUAL TO ZERO ERROR BY MADHVI ON 18 MAY 2018 
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, C.INAME AS SUBGNAME, SUBSTR(TRIM(A.ICODE),1,4) AS SUBGRP,B.INAME,B.CPARTNO, A.ICODE,SUM(A.IQTYOUT) AS QTY,(CASE WHEN SUM(IQTYOUT)> 0 THEN ROUND(SUM(A.IAMOUNT)/SUM(A.IQTYOUT),2) ELSE 0 END)AS IRATE from ivoucher a,item b,ITEM C  where trim(a.icode)=trim(b.icode) and SUBSTR(TRIM(A.ICODE),1,4)=TRIM(C.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type like '4%' and  a.VCHDATE " + xprdRange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' and length(Trim(C.icode))=4 GROUP BY C.INAME, SUBSTR(TRIM(A.ICODE),1,4),b.iname,b.cpartno,a.icode order by b.iname";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, C.INAME AS SUBGNAME, SUBSTR(TRIM(A.ICODE),1,4) AS SUBGRP,B.INAME,B.CPARTNO, A.ICODE,SUM(A.IQTYOUT) AS QTY,(CASE WHEN SUM(IQTYOUT)> 0 THEN ROUND(SUM(A.IAMOUNT)/SUM(A.IQTYOUT),2) ELSE 0 END)AS IRATE from ivoucher a,item b,ITEM C  where trim(a.icode)=trim(b.icode) and SUBSTR(TRIM(A.ICODE),1,4)=TRIM(C.ICODE) AND a.branchcd='" + frm_mbr + "' and a.type like '4%' and  a.VCHDATE " + xprdRange + " and a.icode like '" + party_cd + "%'  and length(Trim(C.icode))=4 GROUP BY C.INAME, SUBSTR(TRIM(A.ICODE),1,4),b.iname,b.cpartno,a.icode order by b.iname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(frm_cocd, dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Prod_Sale_DOM", "std_Prod_Sale_DOM", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50224":
                header_n = "Party Wise Total Qty(DOM)";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (party_cd.Length > 2)
                {
                    cond = " and trim(a.icode) in (" + party_cd + ") and trim(a.icode) in (" + party_cd + ")";
                }
                else
                {
                    cond = " and trim(a.icode) like '%' and trim(a.icode) like '%'";
                }
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.morder as store,A.ACODE,C.ANAME,b.INAME,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,a.icode as item_code,b.cpartno,b.hscode  from ( select ACODE,icode,(Case when to_char(vchdate,'mm')='04' then IQTYOUT   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then IQTYOUT   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then IQTYOUT   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then IQTYOUT   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then IQTYOUT   else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then IQTYOUT   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then IQTYOUT   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then IQTYOUT   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then IQTYOUT   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then IQTYOUT   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then IQTYOUT   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then IQTYOUT   else 0 end) as mar,morder from IVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + xprdRange + ") a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) " + cond + " group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,a.morder order by store";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Party_Prod_wise", "std_Party_Prod_wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50228":
                header_n = "31 Day Sales Report With Quantity";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                // WRITTEN ON PRT_SALE PAGE AS IT IS NOT GIVING ALERT MSG 
                //DateTime date1 = Convert.ToDateTime(fromdt);
                //DateTime date2 = Convert.ToDateTime(todt);
                //TimeSpan days = date2 - date1;
                //if (days.TotalDays > 31)
                //{
                //    fgen.msg("-", "AMSG", "Please Select 31 Days Only"); return;
                //}
                //else
                //{
                #region 31 Days
                SQuery = "SELECT ICODE,TO_CHAR(VCHDATE,'DD/MM/YYYY') AS day,IQTYOUT,TO_CHAR(VCHDATE,'yyyymmdd') AS VCH FROM IVOUCHER WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '4%' AND VCHDATE " + xprdRange + " and acode like '" + party_cd + "%' and icode like '" + part_cd + "%' ORDER BY VCH";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt1 = new DataTable();
                dt1 = dt.Clone();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < 31; i++)
                    {
                        DataRow ft = dt1.NewRow();
                        ft["day"] = fgen.padlc(i + 1, 2);
                        ded1 = ft["day"].ToString();
                        dt1.Rows.Add(ft);
                    }
                }
                mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq6 = ""; mq7 = ""; mq8 = ""; mq9 = ""; mq10 = ""; mq11 = ""; mq12 = "";
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    if (mq0.Length > 0)
                    {
                        mq0 = mq0 + ",decode(TO_CHAR(VCHDATE,'DD'),'" + dt1.Rows[j]["day"].ToString().Trim() + "',iqtyout,0) as DAY_" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq0 = "decode(TO_CHAR(VCHDATE,'DD'),'" + dt1.Rows[j]["day"].ToString().Trim() + "',iqtyout,0) as DAY_" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    if (mq7.Length > 0)
                    {
                        mq7 = mq7 + ",A.DAY_" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq7 = "A.DAY_" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    //FOR SUM 
                    if (mq11.Length > 0)
                    {
                        mq11 = mq11 + ",sum(DAY_" + dt1.Rows[j]["day"].ToString().Trim() + ") as DAY_" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                    else
                    {
                        mq11 = "sum(DAY_" + dt1.Rows[j]["day"].ToString().Trim() + ") as DAY_" + dt1.Rows[j]["day"].ToString().Trim() + "";
                    }
                }
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header," + mq7 + ",a.icode,b.iname,d.aname,A.ACODE,c.mthname,a.vch1 from (SELECT " + mq11 + ",ICODE,ACODE,vchdate,vch1 FROM (SELECT to_char(vchdate,'MM') as vchdate,to_char(vchdate,'yyyy') as vch1, ICODE,ACODE,BRANCHCD," + mq0 + " from ivoucher  where branchcd='" + frm_mbr + "' and type like '4%' AND  vchdate " + xprdRange + ") GROUP BY ICODE,vchdate,vch1,acode) a,item b,famst D,mths c WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(D.ACODE) and trim(a.vchdate)=trim(c.mthnum) order by a.acode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Daywise31_Sale", "Daywise31_Sale", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50229":
                header_n = "Party Wise Total Value(DOM)";
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, A.morder as STORE, A.ACODE,C.ANAME,b.INAME,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,a.icode as item_code,b.cpartno,b.hscode  from ( select ACODE,icode,(Case when to_char(vchdate,'mm')='04' then nvl(iamount,'0')   else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then nvl(iamount,'0')   else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then nvl(iamount,'0')   else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then nvl(iamount,'0')   else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then nvl(iamount,'0') else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then nvl(iamount,'0')   else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then nvl(iamount,'0')   else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then nvl(iamount,'0')   else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then nvl(iamount,'0')   else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then nvl(iamount,'0')   else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then nvl(iamount,'0')   else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then nvl(iamount,'0')   else 0 end) as mar,morder from IVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + xprdRange + " and nvl(iqtyout,0)>0) a,ITEM b,FAMST C where trim(a.icode)=trim(b.icode) AND TRIM(A.ACODE)=TRIM(C.ACODE) group by a.icode,b.iname,b.cpartno,b.hscode,A.ACODE,C.ANAME,A.morder order by STORE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Party_Prod_wise", "std_Party_Prod_wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;
            // ------------ MERGE BY MADHVI ON 22ND JAN 2018 , MADE BY YOGITA ON 20TH JAN 2018 ---------- //

            case "F50240":
                header_n = "Schedule Vs Dispatch 31 Day";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                party_cd = frm_ulvl == "M" ? frm_uname : party_cd;
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit,sum(a.day1) as Day_01,sum(a.day2) as day_02,sum(a.day3) as day_03,sum(a.day4) as day_04,sum(a.day5) as day_05,sum(a.day6) as day_06,sum(a.day7) as day_07,sum(a.day8) as day_08,sum(a.day9) as day_09,sum(a.day10) as day_10,sum(a.day11) as day_11,sum(a.day12) as day_12,sum(a.day13) as day_13,sum(a.day14) as day_14,sum(a.day15) as day_15,sum(a.day16) as day_16,sum(a.day17) as day_17,sum(a.day18) as day_18,sum(a.day19) as day_19,sum(a.day20) as day_20,sum(a.day21) as day_21,sum(a.day22) as day_22,sum(a.day23) as day_23,sum(a.day24) as day_24,sum(a.day25) as day_25,sum(a.day26) as day_26,sum(a.day27) as day_27,sum(a.day28) as day_28,sum(a.day29) as day_29,sum(a.day30) as day_30,sum(a.day31) as day_31,sum(A.Rday1) as Rday1,sum(A.Rday2) as Rday2,sum(A.Rday3) as Rday3,sum(A.Rday4) as Rday4,sum(A.Rday5) as Rday5,sum(A.Rday6) as Rday6,sum(A.Rday7) as Rday7,sum(A.Rday8) as Rday8,sum(A.Rday9) as Rday9, sum(A.Rday10) as Rday10,sum(A.Rday11) as Rday11,sum(A.Rday12) as Rday12,sum(A.Rday13) as Rday13,sum(A.Rday14) as Rday14,sum(A.Rday15) as Rday15,sum(A.Rday16) as Rday16,sum(A.Rday17) as Rday17,sum(A.Rday18) as Rday18,sum(A.Rday19) as Rday19,sum(A.Rday20) as Rday20,sum(A.Rday21) as Rday21,sum(A.Rday22) as Rday22,sum(A.Rday23) as Rday23,sum(A.Rday24) as Rday24,sum(A.Rday25) as Rday25,sum(A.Rday26) as Rday26,sum(A.Rday27) as Rday27,sum(A.Rday28) as Rday28,sum(A.Rday29) as Rday29,sum(A.Rday30) as Rday30,sum(A.Rday31) as Rday31 from (SELECT Acode,icode,DAY1,DAY2,DAY3,day4,day5,day6,day7,day8,day9,day10, Day11,day12,day13,day14,day15,day16,day17 ,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31,0 AS Rday1,0 AS Rday2,0 AS Rday3,0 AS Rday4,0 AS Rday5,0 AS Rday6,0 AS Rday7,0 AS Rday8,0 AS Rday9,0 AS Rday10,0 AS Rday11,0 AS Rday12,0 AS Rday13,0 Rday14,0 AS Rday15,0 AS Rday16,0 AS Rday17,0 AS Rday18,0 AS Rday19,0 AS Rday20,0 AS Rday21,0 AS Rday22,0 AS Rday23,0 AS Rday24,0 AS Rday25,0 AS Rday26,0 AS Rday27,0 AS Rday28,0 AS Rday29,0 AS Rday30,0 AS Rday31 FROM SCHEDULE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='46' and vchdate " + xprdRange + " UNION ALL SELECT acode,icode,0 as DAY1,0 as day2,0 as day3,0 as day4,0 as day5,0 as day6,0 as day7,0 as day8,0 as day9,0 as day10 ,0 as day11,0 as day12, 0 as day13,0 as day14,0 as day15,0 as day16,0 as day17,0 as day18,0 as day19,0 as day20,0 as day21,0 as day22,0 as day23,0 as day24,0 as day25,0 as day26,0 as day27,0 as day28,0 as day29,0 as day30,0 as day31,(Case when to_char(vchdate,'dd')='01' then iqtyout else 0 end) as Rday1,(Case when to_char(vchdate,'dd')='02' then iqtyout else 0 end) as Rday2,(Case when to_char(vchdate,'dd')='03' then iqtyout else 0 end) as Rday3,(Case when to_char(vchdate,'dd')='04' then iqtyout else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyout else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyout else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyout else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyout else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyout else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyout else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyout else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyout else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyout else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyout else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyout else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyout else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyout else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyout else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyout else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyout  else 0 end) as Rday20,(Case when to_char(vchdate,'dd')='21' then iqtyout else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyout  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyout else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyout  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyout  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyout else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyout else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyout  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyout  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyout  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyout  else 0 end) as Rday31 from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + xprdRange + " and nvl(iqtyout,0)>0)  a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' group by a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit order by a.icode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sch_Vs_Desp_DayWise", "std_Sch_Vs_Desp_DayWise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50241":
                header_n = "Schedule Vs Dispatch 12 Month";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                party_cd = frm_ulvl == "M" ? frm_uname : party_cd;
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.acode,b.aname as party,a.icode,c.iname,c.cpartno,c.unit,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as stot,sum(a.djan+a.dfeb+a.dmar+a.dapr+a.dmay+a.djun+a.djul+a.daug+a.dsep+a.doct+a.dnov+a.ddec+a.djan+a.dfeb+a.dmar) as dtot,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug ,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.dapr) as dapr,sum(a.dmay) as dmay,sum(a.djun) as djun,sum(a.djul) as djul,sum(a.daug) as daug,sum(a.dsep) as dsep,sum(a.doct) as doct,sum(a.dnov) as dnov,sum(a.ddec) as ddec,sum(a.djan) as djan,sum(a.dfeb) as dfeb,sum(a.dmar) as dmar from (select acode,icode,(case when to_char(vchdate,'mm')='04' then total else 0 end) as apr,(case when to_char(vchdate,'mm')='05' then total else 0 end) as may,(case when to_char(vchdate,'mm')='06' then total else 0 end) as jun,(case when to_char(vchdate,'mm')='07' then total else 0 end) as jul,(case when to_char(vchdate,'mm')='08' then total else 0 end) as aug,(case when to_char(vchdate,'mm')='09' then total else 0 end) as sep,(case when to_char(vchdate,'mm')='10' then total else 0 end) as oct,(case when to_char(vchdate,'mm')='11' then total else 0 end) as nov,(case when to_char(vchdate,'mm')='12' then total else 0 end) as dec,(case when to_char(vchdate,'mm')='01' then total else 0 end) as jan,(case when to_char(vchdate,'mm')='02' then total else 0 end) as feb,(case when to_char(vchdate,'mm')='03' then total else 0 end) as mar ,0 as dapr,0 as dmay,0 as djun,0 as djul,0 as daug,0 as dsep,0 as doct,0 as dnov,0 as ddec,0 as djan,0 as dfeb,0 as dmar  from schedule where branchcd='" + frm_mbr + "' and type='46' and vchdate " + xprdRange + " union all select acode ,icode,0 as apr,0 as may,0 as jun,0 as jul,0 as aug,0 as sep,0 as oct,0 as nov,0 as dec,0 as jan,0 as feb,0 as mar,(Case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as Dapr,(Case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as Dmay,(Case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as Djun,(Case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as Djul,(Case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as Daug,(Case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as Dsep,(Case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as Doct,(Case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as Dnov,(Case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as Ddec,(Case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as Djan,(Case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as Dfeb,(Case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as Dmar from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + xprdRange + " and nvl(iqtyout,0)>0) a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' group by a.acode,b.aname,a.icode,c.iname,c.cpartno,c.unit";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sch_Vs_Desp_Mth", "std_Sch_Vs_Desp_Mth", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            //MAKE AND MERGED BY YOGITA

            case "F50386": //ITEM WISE WISE
            case "F50388"://SUBGROUP WISE
            case "F50390"://MAIN GROUP WISE
                #region
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                if (iconID == "F50386")
                {
                    header_n = "Item Wise Sales Report";
                    if (part_cd.Length < 1 && party_cd.Length < 1)
                    {
                        cond = " and acode like '%' and icode like '%' ";
                    }
                    else if (party_cd.Length > 1 && part_cd.Length > 1)
                    {
                        cond = " and acode in (" + party_cd + ") and icode in (" + part_cd + ") ";
                    }
                    else if (party_cd.Length > 1 && part_cd.Length < 1)
                    {
                        cond = " and acode in (" + party_cd + ") and icode like '%'";
                    }
                    else if (party_cd.Length < 1 && part_cd.Length > 1)
                    {
                        cond = " and acode like '%' and icode in (" + part_cd + ") ";
                    }
                    SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, a.acode,trim(f.aname) as party,a.icode,trim(b.iname) as iname,substr(trim(a.icode),1,2) as mcode,trim(c.name) as mname,substr(trim(a.icode),1,4) as scode,trim(d.iname) as sname,sum(a.ord_qty) as ord_qty,sum(a.so_qty) as so_qty,sum(a.inv_qty) as inv_qty from (Select type,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,nvl(qtyord,0) as ord_qty,0 as so_qty,0 as inv_qty from somasq where " + branch_Cd + " and type like '4%' and orddt " + xprdRange + "  " + cond + " union all select type,org_invno as ordno,to_char(org_invdt,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,0 as ord_qty,nvl(qtyord,0) as so_qty,0 as inv_qty from somas where " + branch_Cd + " and type like '4%' and orddt " + xprdRange + " " + cond + "  union all select type,ponum as ordno,to_char(podate,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,0 as ord_qty,0 as so_qty,nvl(iqtyout,0) as inv_qty from ivoucher where " + branch_Cd + " and type like '4%' and type not in ('45','47') and vchdate " + xprdRange + " " + cond + " ) a,item b,type c,item d,famst f where trim(a.icode)=trim(b.icode) and substr(trim(a.icode),1,2)=trim(c.type1) and c.id='Y' and  substr(trim(a.icode),1,4)=trim(d.icode) and length(trim(d.icode))='4' and trim(a.acode)=trim(f.acode) group by a.acode,a.icode,trim(b.iname),substr(trim(a.icode),1,2),trim(c.name),substr(trim(a.icode),1,4),trim(d.iname),trim(f.aname)";
                    frm_rptName = "ITEM_WISE_STUD";
                }
                if (iconID == "F50388")
                {
                    header_n = "Sub Group Wise Sales Report";
                    if (part_cd.Length < 1 && party_cd.Length < 1)
                    {
                        cond = " and acode like '%' and substr(trim(icode),1,4) like '%' ";
                    }
                    else if (party_cd.Length > 1 && part_cd.Length > 1)
                    {
                        cond = " and acode in (" + party_cd + ") and substr(trim(icode),1,4) in (" + part_cd + ") ";
                    }
                    else if (party_cd.Length > 1 && part_cd.Length < 1)
                    {
                        cond = " and acode in (" + party_cd + ") and substr(trim(icode),1,4) like '%'";
                    }
                    else if (party_cd.Length < 1 && part_cd.Length > 1)
                    {
                        cond = " and acode like '%' and substr(trim(icode),1,4) in (" + part_cd + ") ";
                    }
                    SQuery = "select  '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, a.acode,trim(f.aname) as party,substr(trim(a.icode),1,2) as mcode,trim(c.name) as mname,substr(trim(a.icode),1,4) as scode,trim(d.iname) as sname,sum(a.ord_qty) as ord_qty,sum(a.so_qty) as so_qty,sum(a.inv_qty) as inv_qty from (Select type,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,nvl(qtyord,0) as ord_qty,0 as so_qty,0 as inv_qty from somasq where " + branch_Cd + " and type like '4%' and orddt " + xprdRange + " " + cond + " union all select type,org_invno as ordno,to_char(org_invdt,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,0 as ord_qty,nvl(qtyord,0) as so_qty,0 as inv_qty from somas where " + branch_Cd + " and type like '4%' and orddt " + xprdRange + " " + cond + "  union all select type,ponum as ordno,to_char(podate,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,0 as ord_qty,0 as so_qty,nvl(iqtyout,0) as inv_qty from ivoucher where " + branch_Cd + " and type like '4%' and type not in ('45','47') and vchdate " + xprdRange + " " + cond + " ) a,item d,famst f,TYPE C where  substr(trim(a.icode),1,4)=trim(d.icode) and length(trim(d.icode))='4' and trim(a.acode)=trim(f.acode) AND  substr(trim(a.icode),1,2)=trim(c.type1) and c.id='Y' group by a.acode,substr(trim(a.icode),1,4),trim(d.iname),trim(f.aname),substr(trim(a.icode),1,2),trim(c.name) order by mcode";
                    frm_rptName = "SG_WISE_STUD";
                }
                if (iconID == "F50390")
                {
                    header_n = "Main Group Wise Sales Report";
                    if (part_cd.Length < 1 && party_cd.Length < 1)
                    {
                        cond = " and acode like '%' and substr(trim(icode),1,2) like '%' ";
                    }
                    else if (party_cd.Length > 1 && part_cd.Length > 1)
                    {
                        cond = " and acode in (" + party_cd + ") and substr(trim(icode),1,2) in (" + part_cd + ") ";
                    }
                    else if (party_cd.Length > 1 && part_cd.Length < 1)
                    {
                        cond = " and acode in (" + party_cd + ") and substr(trim(icode),1,2) like '%'";
                    }
                    else if (party_cd.Length < 1 && part_cd.Length > 1)
                    {
                        cond = " and acode like '%' and substr(trim(icode),1,2) in (" + part_cd + ") ";
                    }
                    SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, a.acode,trim(f.aname) as party,substr(trim(a.icode),1,2) as mcode,trim(c.name) as mname,sum(a.ord_qty) as ord_qty,sum(a.so_qty) as so_qty,sum(a.inv_qty) as inv_qty from (Select type,ordno,to_char(orddt,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,nvl(qtyord,0) as ord_qty,0 as so_qty,0 as inv_qty from somasq where " + branch_Cd + " and type like '4%' and orddt " + xprdRange + " " + cond + " union all select type,org_invno as ordno,to_char(org_invdt,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,0 as ord_qty,nvl(qtyord,0) as so_qty,0 as inv_qty from somas where " + branch_Cd + " and type like '4%' and orddt " + xprdRange + " " + cond + "  union all select type,ponum as ordno,to_char(podate,'dd/mm/yyyy') as orddt,trim(acode) as acode,trim(icode) as icode,0 as ord_qty,0 as so_qty,nvl(iqtyout,0) as inv_qty from ivoucher where " + branch_Cd + " and type like '4%' and type not in ('45','47') and vchdate " + xprdRange + " " + cond + " ) a,type c,famst f where  substr(trim(a.icode),1,2)=trim(c.type1) and c.id='Y' and trim(a.acode)=trim(f.acode) group by a.acode,substr(trim(a.icode),1,2),trim(c.name),trim(f.aname)";
                    frm_rptName = "MG_WISE_STUD";
                }
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            //made by akshay

            case "F50380":
                header_n = "Order Main Group Wise";
                dt = new DataTable();
                //SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,B.ICODE as scode,B.INAME as sname,C.TYPE1 as mcode,C.NAME as mname,SUM(A.QTYORD) as qty FROM SOMASQ A ,ITEM B ,TYPE C  WHERE TRIM(SUBSTR(A.ICODE,1,4))=TRIM(B.ICODE) AND TRIM(SUBSTR(A.ICODE,1,2))= TRIM(C.TYPE1)  AND C.ID='Y' AND  A.BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '4%' AND A.ORDDT " + xprdRange + " GROUP BY B.ICODE,B.INAME,C.TYPE1,C.NAME ORDER BY B.ICODE";
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,C.TYPE1 as mcode,C.NAME as mname,SUM(A.QTYORD) as qty FROM SOMASQ A ,TYPE C  WHERE  TRIM(SUBSTR(A.ICODE,1,2))= TRIM(C.TYPE1)  AND C.ID='Y' AND  A." + branch_Cd + " AND TYPE LIKE '4%' AND A.ORDDT " + xprdRange + " GROUP BY C.TYPE1,C.NAME ORDER BY c.type1";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Ord_Main_Grp_Wise", "Ord_Main_Grp_Wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50382":
                header_n = "Order Sub Group Wise";
                dt = new DataTable();
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,B.ICODE as scode,B.INAME as sname,C.TYPE1,C.NAME,SUM(A.QTYORD) as qty FROM SOMASQ A ,ITEM B ,TYPE C  WHERE TRIM(SUBSTR(A.ICODE,1,4))=TRIM(B.ICODE) AND TRIM(SUBSTR(A.ICODE,1,2))= TRIM(C.TYPE1)  AND C.ID='Y' AND  A." + branch_Cd + " AND TYPE LIKE '4%' AND A.ORDDT " + xprdRange + " GROUP BY B.ICODE,B.INAME,C.TYPE1,C.NAME ORDER BY B.ICODE";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Ord_Sub_Grp_Wise", "Ord_Sub_Grp_Wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50384":
                header_n = "Order Item Wise";
                dt = new DataTable();
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,B.ICODE AS SCODE,B.INAME AS SNAME,C.TYPE1 AS MCODE,C.NAME AS MNAME,A.ICODE,D.INAME ,SUM(A.QTYORD) as qty FROM SOMASQ A ,ITEM B ,TYPE C,ITEM D  WHERE TRIM(SUBSTR(A.ICODE,1,4))=TRIM(B.ICODE) AND TRIM(SUBSTR(A.ICODE,1,2))= TRIM(C.TYPE1) AND TRIM(A.ICODE)=TRIM(D.ICODE)  AND C.ID='Y' AND  A." + branch_Cd + " AND TYPE LIKE '4%' AND A.ORDDT " + xprdRange + " GROUP BY B.ICODE,B.INAME,C.TYPE1,C.NAME,A.ICODE,D.INAME  ORDER BY B.ICODE";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Ord_Item_Wise", "Ord_Item_Wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50311":
                #region
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                cond = " and A.acode like '" + party_cd + "%' and A.icode like '" + part_cd + "%' ";
                header_n = "Sale Register With Item Details";
                SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,a.vdd,trim(a.vchnum)||a.vchdate||trim(a.acode) as fstr,a.vchnum as invno,a.vchdate as invdt,trim(a.acode) as acode,trim(a.icode) as icode,trim(c.iname) as item_name,trim(b.aname) as party,sum(a.iqtyout) as qty ,sum(a.basis) as basis,sum(a.cgst) as cgst,sum(a.sgst) as sgst,sum(a.igst) as igst,a.disc,a.irate,a.bill_tot,a.mode_tpt from (select distinct  a.VCHNUM,to_char(a.vchdate,'dd/mm/yyyy') as VCHDATE,to_char(a.vchdate,'yyyymmdd') as vdd,a.ACODE,a.icode,a.iqtyout,B.AMT_SALE as basis,a.irate, (case when b.st_type='CG' THEN b.aMT_exc else 0 end) AS CGST,(case when b.st_Type='IG' THEN b.aMT_exc else 0 end) AS IGST,rvalue as sgst,b.totdisc_Amt as disc,b.bill_tot,b.mode_tpt  from ivoucher a,sale b where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and  a.branchcd='" + frm_mbr + "' and a.type like '4%' " + cond + " and a.vchdate " + xprdRange + " ) a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) group by a.vchnum,a.vchdate,trim(a.acode),trim(b.aname),a.disc,trim(a.icode),trim(c.iname),a.irate,a.bill_tot,a.mode_tpt,a.vdd  order by fstr,vdd asc";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "SaleReg_11Col", "SaleReg_11Col", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50312":
                #region
                header_n = "Sale Register 10 Col";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,a.vchnum as invno,a.vchdate as invdt,trim(a.acode) as acode,trim(b.aname) as party,sum(a.basis) as basis,sum(a.cgst) as cgst,sum(a.sgst) as sgst,a.disc from (select distinct  a.VCHNUM,to_char(a.vchdate,'dd/mm/yyyy') as VCHDATE,a.ACODE,a.IAMOUNT as basis, a.EXC_aMT AS CGSt,a.CESS_PU as sgst,b.totdisc_Amt as disc  from ivoucher a,sale b where trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and  a.branchcd='" + frm_mbr + "' and a.type in (" + mq0 + ")  and a.vchdate " + xprdRange + " ) a,famst b where trim(a.acode)=trim(b.acode) group by a.vchnum,a.vchdate,trim(a.acode),trim(b.aname),a.disc order by invno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "SaleReg_10Col", "SaleReg_10Col", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50313":
                #region
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                cond = " and a.acode like '" + party_cd + "%'";
                header_n = "Sale Register Party Wise(Gross Total Amount)";
                SQuery = "select distinct '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, A.VCHNUM,TO_char(a.vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'yyyyMMdd') as vdd,trim(a.acode) as acode,trim(c.aname) as party,a.grno,to_char(a.grdate,'dd/mm/yyyy') as grdate,a.mode_tpt,a.bill_tot from SALE a,famst c where trim(a.acode)=trim(c.acode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + xprdRange + " " + cond + "  order by vchnum,vdd asc";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "SaleReg_2Col", "SaleReg_2Col", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50314":
                #region
                header_n = "Sale Register 5 Col";
                SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,a.vchnum as invno,a.vchdate as invdt,trim(a.acode) as acode,trim(b.aname) as party,sum(a.basis) as basis,sum(a.cgst) as cgst,sum(a.sgst) as sgst  from (select distinct  VCHNUM,to_char(vchdate,'dd/mm/yyyy') as VCHDATE,ACODE,IAMOUNT as basis, EXC_aMT AS CGSt,CESS_PU as sgst  from ivoucher where branchcd='" + frm_mbr + "' and type like '4%'  and vchdate " + xprdRange + " ) a,famst b where trim(a.acode)=trim(b.acode) group by a.vchnum,a.vchdate,trim(a.acode),trim(b.aname) order by invno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "SaleReg_5Col", "SaleReg_5Col", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50315":
                #region
                dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable();
                dt2.Columns.Add("header", typeof(string));
                dt2.Columns.Add("fromdt", typeof(string));
                dt2.Columns.Add("todt", typeof(string));
                dt2.Columns.Add("icode", typeof(string));
                dt2.Columns.Add("Iname", typeof(string));
                dt2.Columns.Add("tot", typeof(double));
                dt2.Columns.Add("qty", typeof(double));
                dt2.Columns.Add("basic", typeof(double));
                //   dr1 = new DataRow();
                header_n = "Items Covering 80% Value " + fromdt + " To " + todt + "";
                mq0 = "";
                mq0 = "SELECT SUM(bill_tot) AS TOT FROM SALE  WHERE BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '4%' AND VCHDATE " + xprdRange + "";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0); //tot sale from sale
                ////////////
                SQuery = "select distinct trim(a.icode) as icode,b.iname,sum(a.iqtyout) as qty,sum(a.exc_amt)+sum(a.cess_pu)+sum(a.iamount) as basic from ivoucher a,item b  where  trim(a.icode)=trim(b.icode) and a.branchcd='" + frm_mbr + "' and a.TYPE LIKE '4%' AND a.VCHDATE " + xprdRange + " and a.icode like '9%' group by trim(a.icode),b.iname";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    db = 0; db2 = 0;
                    db = fgen.make_double(dt1.Rows[0]["TOT"].ToString().Trim()) * 80 / 100;
                    db2 = Math.Round(db, 2);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        db1 = 0;
                        dr1 = dt2.NewRow();
                        dr1["header"] = header_n;
                        dr1["fromdt"] = fromdt;
                        dr1["todt"] = todt;
                        dr1["tot"] = fgen.make_double(dt1.Rows[0]["TOT"].ToString().Trim());
                        dr1["icode"] = dt.Rows[i]["icode"].ToString().Trim();
                        dr1["Iname"] = dt.Rows[i]["iname"].ToString().Trim().ToUpper();
                        dr1["qty"] = fgen.make_double(dt.Rows[i]["qty"].ToString().Trim());
                        dr1["basic"] = fgen.make_double(dt.Rows[i]["basic"].ToString().Trim());
                        db1 = fgen.make_double(dt.Rows[i]["basic"].ToString().Trim());
                        if (db1 <= db2) //if basic is less than or eql to tot then row will be add
                        {
                            dt2.Rows.Add(dr1);
                        }
                    }
                }
                if (dt2.Rows.Count > 0)
                {
                    dt2.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt2, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "ItemCovering_80per", "ItemCovering_80per", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50316":
                #region
                header_n = "Country Wise Sales";
                mq0 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                cond = " and A.acode like '" + party_cd + "%' and A.icode like '" + part_cd + "%' ";
                dt = new DataTable();
                SQuery = "SELECT '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + mq0 + "' as country, trim(a.vchnum)||' '||to_char(a.vchdate,'dd/mm/yyyy') as bill_details,a.vchnum,to_char(a.vchdate,'yyyyMMdd') as vdd,a.acode,b.aname,a.icode,c.iname,sum(a.iqtyout) as qty,a.irate,sum(a.iamount) as amt ,0 as disc FROM IVOUCHER a,famst b,item c WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE LIKE '4%' AND a.VCHDATE " + xprdRange + " " + cond + " and b.country='" + mq0 + "'  group by  trim(a.vchnum)||' '||to_char(a.vchdate,'dd/mm/yyyy'),a.vchnum,to_char(a.vchdate,'yyyyMMdd'),a.acode,b.aname,a.icode,c.iname,a.irate  order by aname";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Country_wise_sale", "Country_wise_sale", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            //made and merged by yogita on 02 feb 2019
            case "ITEM_GR_WT":
                #region
                dt = new DataTable();
                header_n = "Item Wise GrWt/Nwt";
                SQuery = "select distinct '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, A.ACODE,A.ICODE,SUM(A.IQTYOUT) AS QTY,SUM(A.IAMOUNT) AS AMT,b.iname,b.cpartno,c.aname as party  from ivoucher A,item b,famst c where  trim(a.icode)=trim(b.icode) and trim(a.acode)=trim(c.acode) and A.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + xprdRange + " GROUP BY a.ACODE,a.ICODE,b.iname,c.aname,b.cpartno order by party,b.iname";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "item_wise_GrWt_Nwt", "item_wise_GrWt_Nwt", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "MTH_WEEK": //monthly week wise analysis
                #region
                dt = new DataTable(); string myear = "";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL4");
                if (Convert.ToInt32(mq1) > 3)
                {
                    myear = frm_myear;
                }
                else
                {
                    int d = Convert.ToInt32(frm_myear) + 1;
                    myear = Convert.ToString(d);
                }
                mq2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ADD_MONTHS(TO_DATE('" + mq1 + "/" + myear + "','MM/yyyy') , 1 ) - TO_DATE('" + mq1 + "/" + myear + "','MM/yyyy') as DAYS  FROM DUAL", "DAYS");
                mq3 = fgen.seek_iname(frm_qstr, frm_cocd, "select mthname from mths where mthnum='" + mq1 + "'", "mthname");

                header_n = "Weeky Sales Analysis for the Month " + mq3 + "/" + myear + "";

                SQuery = "select '" + header_n + "' as header,'" + mq1 + "/" + myear + "' as mthyear,'" + mq3 + "' as mthname, trim(a.acode) as acode,trim(b.aname) as party,trim(a.subitem) as itemcode,sum(a.week1+a.week2+a.week3+a.week4) as tot,sum(week1) as week1,sum(week2) as week2,sum(week3) as week3,sum(week4) as week4 from (select distinct  a.acode,substr(trim(a.icode),1,4) as subitem ,a.iqtyout as week1,0 as week2,0 as week3,0 as week4   from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate between to_date('01/" + mq1 + "/" + myear + "','dd/mm/yyyy') and to_Date('07/" + mq1 + "/" + myear + "','dd/mm/yyyy')  union all  select distinct  a.acode,substr(trim(a.icode),1,4) as subitem,0 as week1 ,a.iqtyout as week2 ,0 as week3 ,0 as week4  from ivoucher a where  a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate between to_date('08/" + mq1 + "/" + myear + "','dd/mm/yyyy') and to_Date('14/" + mq1 + "/" + myear + "','dd/mm/yyyy')  union all  select distinct  a.acode,substr(trim(a.icode),1,4) as subitem,0 as week1,0 as week2 ,a.iqtyout as week3,0 as week4 from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate between to_date('15/" + mq1 + "/" + myear + "','dd/mm/yyyy') and to_Date('21/" + mq1 + "/" + myear + "','dd/mm/yyyy') union all  select distinct  a.acode,substr(trim(a.icode),1,4) as subitem,0 as week1,0 as week2,0 as week3 ,a.iqtyout as week4   from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate between to_date('22/" + mq1 + "/" + myear + "','dd/mm/yyyy') and to_Date('" + mq2 + "/" + mq1 + "/" + myear + "','dd/mm/yyyy') ) a,famst b where trim(a.acode)=trim(b.acode)  group by trim(a.acode) ,trim(b.aname),trim(a.subitem) order by itemcode";

                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Mthly_week_analysis", "Mthly_week_analysis", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "SALE_REJ":///made on 1 feb19
                #region
                dt2 = new DataTable(); dt3 = new DataTable();
                dt2.Columns.Add("header", typeof(string));
                dt2.Columns.Add("fromdt", typeof(string));
                dt2.Columns.Add("sale_mthname", typeof(string));
                dt2.Columns.Add("basic_mthname", typeof(string));
                dt2.Columns.Add("invno", typeof(string));
                dt2.Columns.Add("party", typeof(string));
                dt2.Columns.Add("acode", typeof(string));
                dt2.Columns.Add("join", typeof(string));
                dt2.Columns.Add("qty", typeof(double));
                dt2.Columns.Add("value", typeof(double));
                dt2.Columns.Add("totsale", typeof(double));
                dt2.Columns.Add("basicsale", typeof(double));

                dt = new DataTable(); dt1 = new DataTable(); mq0 = ""; mq1 = ""; mq2 = ""; mq3 = ""; mq4 = ""; mq5 = ""; mq6 = "";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");//SELECTED DATE
                mq2 = mq1.Substring(3, 7);
                mq4 = Convert.ToDateTime(fgen.seek_iname(frm_qstr, frm_cocd, "select to_date('" + mq2 + "','MM/yyyy')-1 as lastdt from dual", "lastdt")).ToString("dd/MM/yyyy");
                mq3 = mq4.Substring(3, 7);//last month as per select month
                mq5 = fgen.seek_iname(frm_qstr, frm_cocd, "select mthname as mth from mths where mthnum='" + mq1.Substring(3, 2) + "'", "mth");
                mq6 = fgen.seek_iname(frm_qstr, frm_cocd, "select mthname as mth from mths where mthnum='" + mq4.Substring(3, 2) + "'", "mth");

                header_n = "Sales & Rejection Summary (Basic Value) " + mq1 + " To " + mq1 + "";
                SQuery = "select a.vchnum,a.acode,b.aname,sum(a.amt_sale) as value from sale a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "'  and a.type like '4%' and to_char(a.vchdate,'dd/mm/yyyy')='" + mq1 + "' group by a.vchnum,a.acode,b.aname";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                ///////               
                mq0 = "select sum(a.amt_sale) as totsale from sale a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "'  and a.type like '4%' and to_char(a.vchdate,'mm/yyyy')='" + mq3 + "' ";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                mq0 = "select sum(a.amt_sale) as basicsale from sale a,famst b where  trim(a.acode)=trim(b.acode) and a.branchcd='" + frm_mbr + "'  and a.type like '4%' and to_char(a.vchdate,'mm/yyyy')='" + mq2 + "' ";
                dt3 = fgen.getdata(frm_qstr, frm_cocd, mq0);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = dt2.NewRow();
                        dr1["header"] = header_n;
                        dr1["fromdt"] = fromdt;
                        dr1["sale_mthname"] = mq6;
                        dr1["basic_mthname"] = mq5;
                        dr1["invno"] = dt.Rows[i]["vchnum"].ToString().Trim();
                        dr1["party"] = dt.Rows[i]["aname"].ToString().Trim().ToUpper();
                        dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                        dr1["join"] = dr1["invno"].ToString().Trim() + "   " + dr1["party"].ToString().Trim().ToUpper();
                        dr1["qty"] = 0;
                        dr1["value"] = fgen.make_double(dt.Rows[i]["value"].ToString().Trim());
                        dr1["totsale"] = fgen.make_double(dt1.Rows[0]["totsale"].ToString().Trim());
                        dr1["basicsale"] = fgen.make_double(dt3.Rows[0]["basicsale"].ToString().Trim());
                        dt2.Rows.Add(dr1);
                    }
                }
                if (dt2.Rows.Count > 0)
                {
                    dt2.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt2, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "sales_rejection_smry", "sales_rejection_smry", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "TARIF":///made on 1 feb19
                #region
                header_n = "Tarrif Wise Analysis";
                dt = new DataTable();
                //SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, tarrifno,type,name,cgst_rate_unit,igst_rate_unit,sum(qty) as qty,sum(igst) as igst,sum(sgst) as sgst,sum(basic) as basic from  (select b.tarrifno,a.type,c.name,(case when a.iopr='CG' then c.num4 else 0 end) as cgst_rate_unit,(case when a.iopr='IG' then  c.num6 else 0 end) as igst_rate_unit,a.iqtyout as qty,a.exc_amt as igst,a.cess_pu as sgst,a.iamount as basic from ivoucher a,item b,TYPEGRP C where trim(a.icode)=trim(b.icode) AND trim(b.tarrifno)=trim(c.acref) and c.id='T1' and a.branchcd='" + frm_mbr + "' and a.type like '4%'  and a.vchdate " + xprdRange + " ) group by tarrifno,type,name,cgst_rate_unit,igst_rate_unit order by type";
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,tarrifno,tarrifno||cgst_rate_unit as grp,type,typname,name,cgst_rate_unit,sum(qty) as qty,sum(igst) as igst,sum(sgst) as sgst,sum(basic) as basic from  (select b.tarrifno,a.type,d.name as typname,c.name,(case when a.iopr='CG' then c.num4 else c.num6 end) as cgst_rate_unit,a.iqtyout as qty,a.exc_amt as igst,a.cess_pu as sgst,a.iamount as basic from ivoucher a,item b,TYPEGRP C,type d where trim(a.icode)=trim(b.icode) AND trim(b.tarrifno)=trim(c.acref) and c.id='T1' and trim(a.type)=trim(d.type1) and d.id='V' and a.branchcd='" + frm_mbr + "' and a.type like '4%'  and a.vchdate " + xprdRange + " ) group by tarrifno,type,name,cgst_rate_unit,typname order by type";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "tarrif_wise_analysis", "tarrif_wise_analysis", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "TARIF_BILL":///made on 1 feb19.done
                #region mrp vaue and adi amt in rpt file is pending to set in rpt
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                dt = new DataTable();
                header_n = "Tarrif wise Invoice wise Report";
                // SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, vchnum,vchdate,vdd,acode,aname,sum(cgst) as cgst,sum(igst) as igst,sum(sgst) as sgst,sum(basic) as basic,exc_tarrif,hs_name,sum(total) as tot,sum(bill_qty) as qty  from (select a.vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'yyyymmdd') as vdd,a.acode,b.aname,(case when a.st_type='CG' then a.amt_exc else 0 end) as cgst,(case when a.st_Type='IG'  then a.amt_exc else 0 end) as igst,a.rvalue as sgst,a.amt_sale as basic,a.exc_tarrif,a.exc_item as hs_name,a.bill_tot as total,a.bill_qty from sale a,famst b where trim(a.acode)=trim(b.acode) and  a.branchcd='" + frm_mbr + "' and a.type in (" + mq1 + ") and a.vchdate " + xprdRange + " ) group by vchnum,vchdate,vdd,acode,aname,exc_tarrif,hs_name  order by vchnum";// in this qry igst,cgst,sgst are diff
                SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, vchnum,vchdate,vdd,acode,aname,sum(cgst) as cgst,sum(igst) as igst,sum(sgst) as sgst,sum(basic) as basic,exc_tarrif,hs_name,sum(total) as tot,sum(bill_qty) as qty  from (select a.vchnum,to_char(vchdate,'dd/mm/yyyy') as vchdate,to_char(a.vchdate,'yyyymmdd') as vdd,a.acode,b.aname,a.amt_exc as cgst,0 as igst,a.rvalue as sgst,a.amt_sale as basic,a.exc_tarrif,a.exc_item as hs_name,a.bill_tot as total,a.bill_qty from sale a,famst b where trim(a.acode)=trim(b.acode) and  a.branchcd='" + frm_mbr + "' and a.type in (" + mq1 + ") and a.vchdate " + xprdRange + " ) group by vchnum,vchdate,vdd,acode,aname,exc_tarrif,hs_name  order by vchnum,vdd"; //in this cgst/igst are same
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "tarrif_bill_wise", "tarrif_bill_wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "A3":
                header_n = "SOP";
                SQuery = "SELECT A.*,I.INAME,B.INAME AS BINAME FROM INSPMST A,ITEM I,ITEM B WHERE TRIM(A.COL1)=TRIM(I.ICODE) AND TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.VCHNUM)||TO_CHAR(A.VCHDATE,'DD/MM/YYYY')||TRIM(A.COL1)||TRIM(A.ICODE)='" + barCode + "' AND LENGTH(TRIM(I.ICODE))=4 ORDER BY A.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "crpt_NeopSOP", "crpt_NeopSOP", dsRep, "");
                }
                break;

            case "A2":
                header_n = "Sales Projection Sheet";
                SQuery = "Select a.*,b.iname,b.cpartno,c.aname from mthlyplan a,item b,famst c where trim(a.icode)=trim(b.icode) and trim(a.CUST)=trim(c.acode) and a.branchcd||a.type||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')='" + barCode + "' order by a.srno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "soproj", "soproj", dsRep, "");
                }
                break;

            case "F50321":
                #region
                ph_tbl = new DataTable();
                #region
                ph_tbl.Columns.Add("HEADER", typeof(string));
                ph_tbl.Columns.Add("FSTR", typeof(string));
                ph_tbl.Columns.Add("FROMDT", typeof(string));
                ph_tbl.Columns.Add("TODT", typeof(string));
                ph_tbl.Columns.Add("Our_Order_No", typeof(string));
                ph_tbl.Columns.Add("Date", typeof(string));
                ph_tbl.Columns.Add("ACODE", typeof(string));
                ph_tbl.Columns.Add("Party", typeof(string));
                ph_tbl.Columns.Add("Item", typeof(string));
                ph_tbl.Columns.Add("ErpCode", typeof(string));
                ph_tbl.Columns.Add("Customer_Order_No", typeof(string));
                ph_tbl.Columns.Add("Customer_Order_Date", typeof(string));
                ph_tbl.Columns.Add("Order_Line_No", typeof(string));
                ph_tbl.Columns.Add("Order_Qty", typeof(double));
                ph_tbl.Columns.Add("Tolerance_Qty", typeof(double));
                ph_tbl.Columns.Add("Sale_Qty", typeof(double));
                ph_tbl.Columns.Add("Invoice_No", typeof(string));
                ph_tbl.Columns.Add("Invoice_Date", typeof(string));
                ph_tbl.Columns.Add("Balance_Order_Qty", typeof(double));
                ph_tbl.Columns.Add("Rate", typeof(double));
                ph_tbl.Columns.Add("Bsr_Stock", typeof(double));
                ph_tbl.Columns.Add("Bal_Order_Req_To_Desp_Bsr_Qty", typeof(double));
                #endregion
                dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable(); dt5 = new DataTable(); dt6 = new DataTable();
                mq0 = ""; mq1 = ""; mq2 = "";
                header_n = "Pending Order Register";
                xprdRange1 = "between to_Date('01/04/" + frm_myear + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1"; //for one fetching day closing
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                string cond1 = "", cond2 = "";
                if (party_cd.Length > 2)
                {
                    cond = "and trim(a.icode) in (" + party_cd + ")";
                    cond2 = "and trim(icode) in (" + party_cd + ")";
                }
                else
                {
                    cond = "and trim(a.icode) like '%'";
                    cond2 = "and trim(icode) like '%'";
                }

                if (part_cd.Length > 2)
                {
                    cond1 = "and trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') in (" + part_cd + ")";
                }
                else
                { }
                mq3 = "";
                mq3 = fgenMV.Fn_Get_Mvar(frm_qstr, "COL4");
                mq0 = "select trim(a.branchcd)||trim(a.type)||trim(a.acode)||trim(a.icode)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy')||trim(a.cdrgno) as fstr,a.branchcd,a.type, a.ordno as ordno,to_char(a.orddt,'dd/mm/yyyy') as ord_date,to_char(a.orddt,'yyyymmdd') as vdd,a.cdrgno,trim(a.acode) as acode,trim(a.icode) as icode,b.aname as party,trim(c.iname) as item,a.weight as ord_line_no,a.pordno as cust_ordno,to_Char(a.porddt,'dd/mm/yyyy') as cust_ordt,sum(a.qtyord) as order_Qty,sum(a.qtysupp) as Tolerance_Qty, nvl(a.irate,0) as irate,sum(nvl(a.qtyord,0)*nvl(a.irate,0)) as order_nal from somas a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and substr(trim(a.type),1,1)='4' and a.type!='47' " + cond + " and a.acode in (" + mq3 + ") and trim(a.branchcd)||trim(a.type)||trim(a.ordno)||to_char(a.orddt,'dd/mm/yyyy') in (" + part_cd + ") group by a.ordno,to_char(a.orddt,'dd/mm/yyyy'),trim(a.acode),trim(a.icode),b.aname,trim(c.iname) ,a.pordno,to_Char(a.porddt,'dd/mm/yyyy'),a.weight,a.irate,to_char(a.orddt,'yyyymmdd'),a.branchcd,a.type,a.cdrgno order by vdd,ordno,icode asc";
                dt = fgen.getdata(frm_qstr, frm_cocd, mq0);//main dt 

                mq0 = "";
                mq0 = fgen.seek_iname(frm_qstr, frm_cocd, "select  to_char(to_date('" + frm_cDt1 + "','dd/mm/yyyy')+600,'dd/MM/yyyy') as dd from dual", "dd");//add 600 days in date for invoice ...inv next year b ban skta hai

                xprdrange = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_date('" + mq0 + "','dd/MM/yyyy')";
                mq1 = "select a.branchcd,a.type,A.vchnum as invno,to_char(a.vchdate,'dd/mm/yyyy') as invdt ,trim(a.acode) as acode,trim(a.icode) as icode,a.prnum,sum(a.iqtyout) as sale_qty,a.binno as lineno,a.irate,sum(a.iamount) as sale_val,a.finvno,a.ponum,to_char(a.podate,'dd/mm/yyyy') as podate,a.prnum,b.mo_vehi  from ivoucher a,sale b where trim(a.branchcd)||trim(a.type)||trim(A.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.vchdate " + xprdrange + " " + cond + " and a.acode in (" + mq3 + ") group by a.vchnum ,to_char(a.vchdate,'dd/mm/yyyy'),trim(a.acode),trim(a.icode) ,a.finvno,a.ponum,to_char(a.podate,'dd/mm/yyyy'),a.prnum,b.mo_vehi,a.irate,a.branchcd,a.type,a.binno,a.prnum order by invno,invdt asc";
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq1);

                mq2 = "select trim(a.icode) as icode ,trim(b.iname) as iname,b.irate,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,trim(icode) as icode,yr_" + frm_myear + "  as opening,0 as cdr,0 as ccr from itembal where branchcd='" + frm_mbr + "'  and length(trim(icode))>4  " + cond2 + "  union all select branchcd,trim(icode) as icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + frm_mbr + "' AND VCHDATE " + xprdRange1 + "  and store='Y' " + cond2 + " GROUP BY trim(icode),branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + frm_mbr + "' AND vchdate " + xprdrange + " and store='Y' " + cond2 + " and substr(trim(icode),1,1)='9' GROUP BY trim(icode) ,branchcd) a,item b where trim(a.icode)=trim(b.icode) GROUP BY A.ICODE,trim(b.iname),b.irate having sum(a.opening)+sum(a.cdr)+sum(a.ccr)<>0 order by icode";
                dt2 = fgen.getdata(frm_qstr, frm_cocd, mq2);//stock dt
                header_n = "Pending SO BSR Qty";
                if (dt.Rows.Count > 0)
                {
                    DataView view1im = new DataView(dt);
                    DataTable dtdrsim = new DataTable();
                    dtdrsim = view1im.ToTable(true, "branchcd", "type", "acode", "icode", "ordno", "ord_date", "cdrgno"); //MAIN                  
                    foreach (DataRow dr0 in dtdrsim.Rows)
                    {
                        dt3 = new DataTable(); dt4 = new DataTable();
                        DataView viewim = new DataView(dt, "branchcd='" + dr0["branchcd"].ToString().Trim() + "' and type='" + dr0["type"].ToString().Trim() + "' and acode='" + dr0["acode"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "' and ordno='" + dr0["ordno"].ToString().Trim() + "' and ord_date='" + dr0["ord_date"].ToString().Trim() + "' and cdrgno='" + dr0["cdrgno"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                        dt3 = viewim.ToTable();//somas view
                        dr1 = ph_tbl.NewRow();
                        ////invoice view
                        if (dt1.Rows.Count > 0)
                        {
                            DataView viewim1 = new DataView(dt1, "branchcd='" + dr0["branchcd"].ToString().Trim() + "' and type='" + dr0["type"].ToString().Trim() + "' and acode='" + dr0["acode"].ToString().Trim() + "' and icode='" + dr0["icode"].ToString().Trim() + "' and ponum='" + dr0["ordno"].ToString().Trim() + "' and podate='" + dr0["ord_date"].ToString().Trim() + "' and prnum='" + dr0["cdrgno"].ToString().Trim() + "'", "", DataViewRowState.CurrentRows);
                            dt4 = viewim1.ToTable();
                        }
                        db6 = 0;//for bal order qty
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            #region order details
                            db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0;
                            dr1 = ph_tbl.NewRow();
                            dr1["FSTR"] = dt3.Rows[i]["fstr"].ToString().Trim();
                            dr1["header"] = header_n;
                            dr1["fromdt"] = fromdt;
                            dr1["todt"] = todt;
                            dr1["Our_Order_No"] = dt3.Rows[i]["ordno"].ToString().Trim();
                            dr1["Date"] = dt3.Rows[i]["ord_date"].ToString().Trim();
                            dr1["Acode"] = dt3.Rows[i]["acode"].ToString().Trim();
                            dr1["Party"] = dt3.Rows[i]["party"].ToString().Trim();
                            dr1["Item"] = dt3.Rows[i]["item"].ToString().Trim();
                            dr1["ErpCode"] = dt3.Rows[i]["icode"].ToString().Trim();
                            dr1["Customer_Order_No"] = dt3.Rows[i]["cust_ordno"].ToString().Trim();
                            dr1["Customer_Order_Date"] = dt3.Rows[i]["cust_ordt"].ToString().Trim();
                            dr1["Order_Line_No"] = dt3.Rows[i]["ord_line_no"].ToString().Trim();
                            dr1["Order_Qty"] = dt3.Rows[i]["order_Qty"].ToString().Trim();
                            db1 = fgen.make_double(dr1["Order_Qty"].ToString().Trim());
                            //=================                                  
                            for (int j = 0; j < dt4.Rows.Count; j++)
                            {
                                #region filling invoice details on basis of order
                                if (j != 0)
                                {
                                    dr1 = ph_tbl.NewRow();   /// for invoice
                                    dr1["FSTR"] = dt3.Rows[i]["fstr"].ToString().Trim();
                                    dr1["header"] = header_n;
                                    dr1["fromdt"] = fromdt;
                                    dr1["todt"] = todt;
                                    dr1["Our_Order_No"] = dt3.Rows[i]["ordno"].ToString().Trim();
                                    dr1["Date"] = dt3.Rows[i]["ord_date"].ToString().Trim();
                                    dr1["Acode"] = dt3.Rows[i]["acode"].ToString().Trim();
                                    dr1["Party"] = dt3.Rows[i]["party"].ToString().Trim();
                                    dr1["Item"] = dt3.Rows[i]["item"].ToString().Trim();
                                    dr1["ErpCode"] = dt3.Rows[i]["icode"].ToString().Trim();
                                    dr1["Customer_Order_No"] = dt3.Rows[i]["cust_ordno"].ToString().Trim();
                                    dr1["Customer_Order_Date"] = dt3.Rows[i]["cust_ordt"].ToString().Trim();
                                    dr1["Order_Line_No"] = dt3.Rows[i]["ord_line_no"].ToString().Trim();
                                    dr1["Order_Qty"] = dt3.Rows[i]["order_Qty"].ToString().Trim();
                                    db1 = fgen.make_double(dr1["Order_Qty"].ToString().Trim());
                                }
                                dr1["Sale_Qty"] = dt4.Rows[j]["sale_qty"].ToString().Trim();
                                db2 = fgen.make_double(dr1["Sale_Qty"].ToString().Trim());
                                db3 = db1 - db2;//bal order qty
                                if (db3 > 0)
                                {
                                    db4 = fgen.make_double(dt3.Rows[i]["Tolerance_Qty"].ToString().Trim().Split('.')[0].ToString());
                                    dr1["Tolerance_Qty"] = db4;
                                }
                                else
                                {
                                    dr1["Tolerance_Qty"] = 0;
                                }
                                dr1["Invoice_No"] = dt4.Rows[j]["invno"].ToString().Trim();
                                dr1["Invoice_Date"] = dt4.Rows[j]["invdt"].ToString().Trim();
                                if (j == 0)
                                {
                                    dr1["Balance_Order_Qty"] = db3;
                                    db6 = db3;
                                }
                                else
                                {
                                    dr1["Balance_Order_Qty"] = db6 - db2;
                                    db6 = fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim());
                                }
                                dr1["Rate"] = fgen.make_double(dt4.Rows[j]["irate"].ToString().Trim());
                                dr1["Bsr_Stock"] = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dt4.Rows[j]["icode"].ToString().Trim() + "'", "Closing_Stk"));
                                db8 = fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim());
                                db5 = fgen.make_double(dr1["Bsr_Stock"].ToString().Trim()) - fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim());
                                if (db8 >= 0)
                                {
                                    if (db5 > 0)
                                    {
                                        dr1["Bal_Order_Req_To_Desp_Bsr_Qty"] = db5;
                                    }
                                    else
                                    {
                                        dr1["Bal_Order_Req_To_Desp_Bsr_Qty"] = fgen.make_double(dr1["Bsr_Stock"].ToString().Trim());
                                    }
                                }
                                #endregion
                                ph_tbl.Rows.Add(dr1);
                            }
                            if (dt4.Rows.Count == 0)
                            {
                                db2 = fgen.make_double(dr1["Sale_Qty"].ToString().Trim());
                                db3 = db1 - db2;//bal order qty
                                if (db3 > 0)
                                {
                                    db4 = fgen.make_double(dt3.Rows[i]["Tolerance_Qty"].ToString().Trim().Split('.')[0].ToString());
                                    dr1["Tolerance_Qty"] = db4;
                                }
                                else
                                {
                                    dr1["Tolerance_Qty"] = 0;
                                }
                                dr1["Bsr_Stock"] = fgen.make_double(fgen.seek_iname_dt(dt2, "icode='" + dt3.Rows[i]["icode"].ToString().Trim() + "'", "Closing_Stk"));
                                dr1["Balance_Order_Qty"] = db3;
                                db8 = fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim());
                                db5 = fgen.make_double(dr1["Bsr_Stock"].ToString().Trim()) - fgen.make_double(dr1["Balance_Order_Qty"].ToString().Trim());
                                if (db8 >= 0)
                                {
                                    if (db5 > 0)
                                    {
                                        dr1["Bal_Order_Req_To_Desp_Bsr_Qty"] = db5;
                                    }
                                    else
                                    {
                                        dr1["Bal_Order_Req_To_Desp_Bsr_Qty"] = fgen.make_double(dr1["Bsr_Stock"].ToString().Trim());
                                    }
                                }
                                ph_tbl.Rows.Add(dr1);
                            }
                            #endregion
                        }
                    }
                }
                if (ph_tbl.Rows.Count > 0)
                {
                    ph_tbl.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(ph_tbl, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Pending_SO_BSR_QTY", "Pending_SO_BSR_QTY", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50275":
                header_n = "Main Group,Sub Group,Party Wise Sale Qty Report";
                mq10 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                mq9 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL2");
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (mq10.Length <= 1)
                {
                    mq2 = "and trim(a.acode) like '%'";
                }
                else
                {
                    mq2 = "and trim(a.acode) in (" + mq10 + ")";
                }
                //-------------------------
                if (mq9.Length <= 1)
                {
                    mq3 = "and trim(c.staten) like '%'";
                }
                else
                {
                    mq3 = "and trim(c.staten) in (" + mq9 + ")";
                }
                //----------------------------------
                if (party_cd.Length <= 1)
                {
                    mq1 = "and trim(d.type1) like '%'";
                }
                else
                {
                    mq1 = "and trim(d.type1) = '" + party_cd + "'";
                }

                if (part_cd.Length <= 1)
                {
                    mq0 = "and substr(trim(a.icode),1,4) like '%'";
                }
                else
                {
                    mq0 = "and substr(trim(a.icode),1,4) in (" + part_cd + ")";
                }

                dt = new DataTable();
                SQuery = "select '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,'" + header_n + "' as header, trim(a.acode) as acode,c.aname as party,c.staten as state,substr(trim(a.icode),1,4) as sub_grp,trim(b.iname) as sub_nm,substr(trim(a.icode),1,2) as main_grp,trim(d.name) as mgname,sum(a.iqtyout) as iqtyout,sum(a.iamount) as iamount ,sum(a.return) as ret_qty,sum(a.ret_value) as ret_value ,sum(a.avg_qty) as avg_qty,sum(a.avg_amt) as avg_amt from (select a.acode,a.icode , a.iqtyout,a.iamount ,0 as return,0 as ret_value,0 as avg_qty,0 as avg_amt from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='47' and a.vchdate " + xprdRange + " and nvl(a.iqtyout,0)>0 union all select a.acode,a.icode ,0 as  iqtyout,0 as iamount ,a.iqtyin as return,a.iamount as ret_value,0 as avg_qty,0 as avg_amt from ivoucher a where a.branchcd='" + frm_mbr + "' and a.type ='04' and a.vchdate " + xprdRange + ") a , item b ,famst c,type d where trim(a.acode)=trim(c.acode) and substr(trim(a.icode),1,4)=trim(b.icode) and substr(trim(a.icode),1,2)=trim(d.type1) and d.id='Y' and length(trim(b.icode))=4  " + mq0 + " " + mq1 + " " + mq2 + " " + mq3 + " group by trim(a.acode),c.aname ,substr(trim(a.icode),1,4) ,trim(b.iname),substr(trim(a.icode),1,2) ,trim(d.name),c.staten order by main_grp,sub_grp,acode ";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                TimeSpan t = Convert.ToDateTime(todt) - Convert.ToDateTime(fromdt).AddDays(1);

                foreach (DataRow dr in dt.Rows)
                {
                    dr["avg_qty"] = (fgen.make_double(dr["iqtyout"].ToString().Trim()) - fgen.make_double(dr["ret_qty"].ToString().Trim())) / t.TotalDays;
                    dr["avg_amt"] = (fgen.make_double(dr["iamount"].ToString().Trim()) - fgen.make_double(dr["ret_value"].ToString().Trim())) / t.TotalDays;
                }
                if (dt.Rows.Count > 0)
                {

                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Sale_Qty_Himt", "Sale_Qty_Himt", dsRep, header_n);
                }
                break;

            case "F50278":
                #region
                header_n = "Monthly State Wise,Group Wise,Sales Trend";
                DataTable dt10 = new DataTable();
                dt10.Columns.Add("Header", typeof(string));
                dt10.Columns.Add("fromdt", typeof(string));
                dt10.Columns.Add("todt", typeof(string));
                dt10.Columns.Add("State", typeof(string));
                dt10.Columns.Add("MCODE", typeof(string));
                dt10.Columns.Add("MNAME", typeof(string));
                dt10.Columns.Add("SUBCODE", typeof(string));
                dt10.Columns.Add("SUBNAME", typeof(string));
                dt10.Columns.Add("icode", typeof(string));
                dt10.Columns.Add("iname", typeof(string));
                dt10.Columns.Add("apr_qty", typeof(double));
                dt10.Columns.Add("apr_val", typeof(double));
                dt10.Columns.Add("may_qty", typeof(double));
                dt10.Columns.Add("may_val", typeof(double));
                dt10.Columns.Add("june_qty", typeof(double));
                dt10.Columns.Add("june_val", typeof(double));
                dt10.Columns.Add("july_qty", typeof(double));
                dt10.Columns.Add("july_val", typeof(double));
                dt10.Columns.Add("aug_qty", typeof(double));
                dt10.Columns.Add("aug_val", typeof(double));
                dt10.Columns.Add("sep_qty", typeof(double));
                dt10.Columns.Add("sep_val", typeof(double));
                dt10.Columns.Add("oct_qty", typeof(double));
                dt10.Columns.Add("oct_val", typeof(double));
                dt10.Columns.Add("nov_qty", typeof(double));
                dt10.Columns.Add("nov_val", typeof(double));
                dt10.Columns.Add("dec_qty", typeof(double));
                dt10.Columns.Add("dec_val", typeof(double));
                dt10.Columns.Add("jan_qty", typeof(double));
                dt10.Columns.Add("jan_val", typeof(double));
                dt10.Columns.Add("feb_qty", typeof(double));
                dt10.Columns.Add("feb_val", typeof(double));
                dt10.Columns.Add("mar_qty", typeof(double));
                dt10.Columns.Add("mar_val", typeof(double));
                dt10.Columns.Add("total_qty", typeof(double));
                dt10.Columns.Add("total_val", typeof(double));
                mq3 = ""; mq4 = "";
                mq4 = fgenMV.Fn_Get_Mvar(frm_qstr, "COL4");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                if (mq4.Length > 1)
                {
                    mq3 = "and f.staten in (" + mq4 + ")";
                }
                else
                {
                    mq3 = "and f.staten like '%'";
                }
                if (party_cd.Length <= 1)
                {
                    party_cd = "and A.MGCODE like '%'";
                }
                else
                {
                    party_cd = "and A.MGCODE='" + party_cd + "'";
                }
                if (part_cd.Length <= 1)
                {
                    part_cd = "and A.SUBCODE like '%'";
                }
                else
                {
                    part_cd = " and A.SUBCODE in (" + part_cd + ")";
                }
                //SQuery = "select  a.MGCODE,t.name as mgname,a.subcode,d.iname as subname,trim(f.staten) as state,sum(a.apr_qty) as apr_qty,sum(a.apr_val) as apr_val,sum(a.may_qty) as may_qty,sum(a.may_val) as may_val,sum(a.june_qty) as june_qty,sum(a.june_val) as june_val,sum(a.july_qty) as july_qty,sum(a.july_val) as july_val,sum(a.aug_qty) as aug_qty,sum(a.aug_val) as aug_val,sum(a.sep_qty) as sep_qty,sum(a.sep_val) as sep_val,sum(a.oct_qty) as oct_qty,sum(a.oct_val) as oct_val,sum(a.nov_qty) as nov_qty,sum(a.nov_val) as nov_val,sum(a.dec_qty) as dec_qty,sum(a.dec_val) as dec_val,sum(a.jan_qty) as jan_qty,sum(a.jan_val) as jan_val,sum(a.feb_qty) as feb_qty,sum(a.feb_val) as feb_val,sum(a.mar_qty) as mar_qty,sum(a.mar_val) as mar_val,sum(a.apr_qty)+sum(a.may_qty)+sum(a.june_qty)+sum(a.july_qty)+sum(a.aug_qty)+sum(a.sep_qty)+sum(a.oct_qty)+sum(a.nov_qty)+sum(a.dec_qty)+sum(a.jan_qty)+sum(a.feb_qty)+sum(a.mar_qty) as total_qty,sum(a.apr_val)+sum(a.may_val)+sum(a.june_val)+sum(a.july_val)+sum(a.aug_val)+sum(a.sep_val)+sum(a.oct_val)+sum(a.nov_val)+sum(a.dec_val)+sum(a.jan_val)+sum(a.feb_val)+sum(a.mar_val) as total_val from (select substr(trim(icode),1,2) AS MGCODE,substr(trim(icode),1,4) as SUBCODE,trim(acode) as acode, (case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as apr_qty,(case when to_char(vchdate,'mm')='04' then iamount else 0 end) as apr_val,(case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as may_qty,(case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may_val,(case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as june_qty,(case when to_char(vchdate,'mm')='06' then iamount else 0 end) as june_val,(case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as july_qty,(case when to_char(vchdate,'mm')='07' then iamount else 0 end) as july_val,(case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as aug_qty,(case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug_val,(case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as sep_qty,(case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep_val,(case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as oct_qty,(case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct_val,(case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as nov_qty,(case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov_val,(case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as dec_qty,(case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec_val,(case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as jan_qty,(case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan_val,(case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as feb_qty,(case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb_val,(case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as mar_qty,(case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar_val from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " and nvl(iqtyout,0)>0) a, famst f,type t,ITEM D where trim(a.acode)=trim(f.acode) " + mq3 + " " + part_cd + " " + party_cd + " and trim(a.mgcode)=trim(t.type1) and t.id='Y' and trim(a.subcode)=trim(d.icode) and length(trim(d.icode))=4 group by trim(f.staten),a.MGCODE,a.subcode,t.name,d.iname order by state,a.MGCODE,a.subcode";
                SQuery = "select  a.MGCODE,t.name as mgname,a.subcode,d.iname as subname,a.icode,e.iname,trim(f.staten) as state,sum(a.apr_qty) as apr_qty,sum(a.apr_val) as apr_val,sum(a.may_qty) as may_qty,sum(a.may_val) as may_val,sum(a.june_qty) as june_qty,sum(a.june_val) as june_val,sum(a.july_qty) as july_qty,sum(a.july_val) as july_val,sum(a.aug_qty) as aug_qty,sum(a.aug_val) as aug_val,sum(a.sep_qty) as sep_qty,sum(a.sep_val) as sep_val,sum(a.oct_qty) as oct_qty,sum(a.oct_val) as oct_val,sum(a.nov_qty) as nov_qty,sum(a.nov_val) as nov_val,sum(a.dec_qty) as dec_qty,sum(a.dec_val) as dec_val,sum(a.jan_qty) as jan_qty,sum(a.jan_val) as jan_val,sum(a.feb_qty) as feb_qty,sum(a.feb_val) as feb_val,sum(a.mar_qty) as mar_qty,sum(a.mar_val) as mar_val,sum(a.apr_qty)+sum(a.may_qty)+sum(a.june_qty)+sum(a.july_qty)+sum(a.aug_qty)+sum(a.sep_qty)+sum(a.oct_qty)+sum(a.nov_qty)+sum(a.dec_qty)+sum(a.jan_qty)+sum(a.feb_qty)+sum(a.mar_qty) as total_qty,sum(a.apr_val)+sum(a.may_val)+sum(a.june_val)+sum(a.july_val)+sum(a.aug_val)+sum(a.sep_val)+sum(a.oct_val)+sum(a.nov_val)+sum(a.dec_val)+sum(a.jan_val)+sum(a.feb_val)+sum(a.mar_val) as total_val from (select substr(trim(icode),1,2) AS MGCODE,substr(trim(icode),1,4) as SUBCODE,trim(icode) as icode,trim(acode) as acode, (case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as apr_qty,(case when to_char(vchdate,'mm')='04' then iamount else 0 end) as apr_val,(case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as may_qty,(case when to_char(vchdate,'mm')='05' then iamount else 0 end) as may_val,(case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as june_qty,(case when to_char(vchdate,'mm')='06' then iamount else 0 end) as june_val,(case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as july_qty,(case when to_char(vchdate,'mm')='07' then iamount else 0 end) as july_val,(case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as aug_qty,(case when to_char(vchdate,'mm')='08' then iamount else 0 end) as aug_val,(case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as sep_qty,(case when to_char(vchdate,'mm')='09' then iamount else 0 end) as sep_val,(case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as oct_qty,(case when to_char(vchdate,'mm')='10' then iamount else 0 end) as oct_val,(case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as nov_qty,(case when to_char(vchdate,'mm')='11' then iamount else 0 end) as nov_val,(case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as dec_qty,(case when to_char(vchdate,'mm')='12' then iamount else 0 end) as dec_val,(case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as jan_qty,(case when to_char(vchdate,'mm')='01' then iamount else 0 end) as jan_val,(case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as feb_qty,(case when to_char(vchdate,'mm')='02' then iamount else 0 end) as feb_val,(case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as mar_qty,(case when to_char(vchdate,'mm')='03' then iamount else 0 end) as mar_val from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " and nvl(iqtyout,0)>0) a, famst f,type t,ITEM D,item e where trim(a.acode)=trim(f.acode) " + mq3 + " " + part_cd + " " + party_cd + " and trim(a.mgcode)=trim(t.type1) and t.id='Y' and trim(a.subcode)=trim(d.icode) and length(trim(d.icode))=4  and trim(a.icode)=trim(e.icode) group by trim(f.staten),a.MGCODE,a.subcode,t.name,d.iname,a.icode,e.iname order by state,a.MGCODE,a.subcode";
                dt5 = new DataTable();

                dt5 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt5.Rows.Count > 0)
                {
                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        #region
                        dr1 = dt10.NewRow();
                        dr1["header"] = header_n;
                        dr1["fromdt"] = fromdt;
                        dr1["todt"] = todt;
                        dr1["State"] = dt5.Rows[i]["state"].ToString().Trim();
                        dr1["MCODE"] = dt5.Rows[i]["mgcode"].ToString().Trim();
                        dr1["MNAME"] = dt5.Rows[i]["mgname"].ToString().Trim();
                        dr1["SUBCODE"] = dt5.Rows[i]["subcode"].ToString().Trim();
                        dr1["SUBNAME"] = dt5.Rows[i]["subname"].ToString().Trim();
                        dr1["icode"] = dt5.Rows[i]["icode"].ToString().Trim();
                        dr1["iname"] = dt5.Rows[i]["iname"].ToString().Trim();
                        dr1["apr_qty"] = fgen.make_double(dt5.Rows[i]["apr_qty"].ToString().Trim());
                        dr1["may_qty"] = fgen.make_double(dt5.Rows[i]["may_qty"].ToString().Trim());
                        dr1["june_qty"] = fgen.make_double(dt5.Rows[i]["june_qty"].ToString().Trim());
                        dr1["july_qty"] = fgen.make_double(dt5.Rows[i]["july_qty"].ToString().Trim());
                        dr1["aug_qty"] = fgen.make_double(dt5.Rows[i]["aug_qty"].ToString().Trim());
                        dr1["sep_qty"] = fgen.make_double(dt5.Rows[i]["sep_qty"].ToString().Trim());
                        dr1["oct_qty"] = fgen.make_double(dt5.Rows[i]["oct_qty"].ToString().Trim());
                        dr1["nov_qty"] = fgen.make_double(dt5.Rows[i]["nov_qty"].ToString().Trim());
                        dr1["dec_qty"] = fgen.make_double(dt5.Rows[i]["dec_qty"].ToString().Trim());
                        dr1["jan_qty"] = fgen.make_double(dt5.Rows[i]["jan_qty"].ToString().Trim());
                        dr1["feb_qty"] = fgen.make_double(dt5.Rows[i]["feb_qty"].ToString().Trim());
                        dr1["mar_qty"] = fgen.make_double(dt5.Rows[i]["mar_qty"].ToString().Trim());
                        dr1["apr_val"] = fgen.make_double(dt5.Rows[i]["apr_val"].ToString().Trim());
                        dr1["may_val"] = fgen.make_double(dt5.Rows[i]["may_val"].ToString().Trim());
                        dr1["june_val"] = fgen.make_double(dt5.Rows[i]["june_val"].ToString().Trim());
                        dr1["july_val"] = fgen.make_double(dt5.Rows[i]["july_val"].ToString().Trim());
                        dr1["aug_val"] = fgen.make_double(dt5.Rows[i]["aug_val"].ToString().Trim());
                        dr1["sep_val"] = fgen.make_double(dt5.Rows[i]["sep_val"].ToString().Trim());
                        dr1["oct_val"] = fgen.make_double(dt5.Rows[i]["oct_val"].ToString().Trim());
                        dr1["nov_val"] = fgen.make_double(dt5.Rows[i]["nov_val"].ToString().Trim());
                        dr1["dec_val"] = fgen.make_double(dt5.Rows[i]["dec_val"].ToString().Trim());
                        dr1["jan_val"] = fgen.make_double(dt5.Rows[i]["jan_val"].ToString().Trim());
                        dr1["feb_val"] = fgen.make_double(dt5.Rows[i]["feb_val"].ToString().Trim());
                        dr1["mar_val"] = fgen.make_double(dt5.Rows[i]["mar_val"].ToString().Trim());
                        dr1["total_qty"] = fgen.make_double(dt5.Rows[i]["total_qty"].ToString().Trim());
                        dr1["total_Val"] = fgen.make_double(dt5.Rows[i]["total_val"].ToString().Trim());
                        dt10.Rows.Add(dr1);
                        #endregion
                    }
                }
                if (dt10.Rows.Count > 0)
                {
                    dt10.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt10, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Mthly_Grp_State_Wise_Sales_trnd", "Mthly_Grp_State_Wise_Sales_trnd", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50279":
                #region
                dtm = new DataTable();//add dummy column in this
                dtm.Columns.Add("col_1", typeof(string));
                dtm.Columns.Add("col_2", typeof(string));
                dtm.Columns.Add("col_3", typeof(string));
                dtm.Columns.Add("col_4", typeof(string));
                dtm.Columns.Add("col_5", typeof(string));
                dtm.Columns.Add("col_6", typeof(string));
                dtm.Columns.Add("col_7", typeof(string));
                dtm.Columns.Add("col_8", typeof(string));
                dt4 = dtm.Clone();
                dt5 = dtm.Clone();
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                cond1 = ""; cond2 = ""; string cond3 = "";
                if (party_cd.Length < 2)
                {
                    cond = "and substr(trim(icode),1,2) like '%'";
                    cond2 = "and substr(trim(a.icode),1,2) like '%'";
                }
                else
                {
                    cond = "and substr(trim(icode),1,2) in (" + party_cd + ") ";
                    cond2 = "and substr(trim(a.icode),1,2) in (" + party_cd + ") ";
                }
                if (part_cd.Length < 2)
                {
                    cond1 = "and substr(trim(icode),1,4) like '%'";
                    cond3 = "and substr(trim(a.icode),1,4) like '%'";
                }
                else
                {
                    cond1 = "and substr(trim(icode),1,4) in (" + part_cd + ")";
                    cond3 = "and substr(trim(a.icode),1,4) in (" + part_cd + ")";
                }
                dsRep = new DataSet();
                header_n = "Sales Trend Statement";
                int cnt = 0, cnt1 = 0;
                string footer_n = "";
                //SQuery = "select a.acode,b.aname,a.subcode,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from (select trim(acode) as acode,substr(trim(icode),1,4) as subcode,(Case when to_char(vchdate,'mm')='04' then  (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as mar from iVOUCHER where type like '4%' and type!='47' and vchdate " + xprdrange + " )  a,famst b,item c  where trim(a.acode)=trim(b.acode) and trim(a.subcode)=trim(c.icode) and length(trim(c.icode))>4  group by a.acode,b.aname,a.subcode order by acode";
                //SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + footer_n + "' as footer, a.acode,b.aname,a.mcode,D.NAME AS MNAME,a.subcode as type,C.IName as name,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as total,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from (select trim(acode) as acode,substr(trim(icode),1,2) as mcode,substr(trim(icode),1,4) as subcode,(Case when to_char(vchdate,'mm')='04' then  (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as mar from iVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " " + cond + " " + cond1 + ") a,famst b,item c ,type d where trim(a.acode)=trim(b.acode) and trim(a.subcode)=trim(c.icode) and length(trim(c.icode))=4 and a.mcode=trim(d.type1) and d.id='Y' group by a.acode,b.aname,a.subcode,c.iname,a.mcode,d.name order by type";
                //SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + footer_n + "' as footer, a.acode,b.aname,a.mcode,D.NAME AS MNAME,a.subcode as type,C.IName as name,round(sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar)/100000,2) as total,round(sum(a.apr)/100000,2) as apr,round(sum(a.may)/100000,2) as may,round(sum(a.jun)/100000,2) as jun,round(sum(a.jul)/100000,2) as jul,round(sum(a.aug)/100000,2) as aug,round(sum(a.sep)/100000,2) as sep,round(sum(a.oct)/100000,2) as oct,round(sum(a.nov)/100000,2) as nov,round(sum(a.dec)/100000,2) as dec,round(sum(a.jan)/100000,2) as jan,round(sum(a.feb)/100000,2) as feb,round(sum(a.mar)/100000,2) as mar from (select trim(acode) as acode,substr(trim(icode),1,2) as mcode,substr(trim(icode),1,4) as subcode,(Case when to_char(vchdate,'mm')='04' then  (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as mar from iVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " " + cond + " " + cond1 + ") a,famst b,item c ,type d where trim(a.acode)=trim(b.acode) and trim(a.subcode)=trim(c.icode) and length(trim(c.icode))=4 and a.mcode=trim(d.type1) and d.id='Y' group by a.acode,b.aname,a.subcode,c.iname,a.mcode,d.name order by type";
                SQuery = "select '" + header_n + "' AS header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,'" + footer_n + "' as footer,a.mcode,D.NAME AS MNAME,a.subcode as type,C.IName as name,round(sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar)/100000,2) as total,round(sum(a.apr)/100000,2) as apr,round(sum(a.may)/100000,2) as may,round(sum(a.jun)/100000,2) as jun,round(sum(a.jul)/100000,2) as jul,round(sum(a.aug)/100000,2) as aug,round(sum(a.sep)/100000,2) as sep,round(sum(a.oct)/100000,2) as oct,round(sum(a.nov)/100000,2) as nov,round(sum(a.dec)/100000,2) as dec,round(sum(a.jan)/100000,2) as jan,round(sum(a.feb)/100000,2) as feb,round(sum(a.mar)/100000,2) as mar from (select trim(acode) as acode,substr(trim(icode),1,2) as mcode,substr(trim(icode),1,4) as subcode,(Case when to_char(vchdate,'mm')='04' then  (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as mar from iVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " " + cond + " " + cond1 + ") a,item c ,type d where trim(a.subcode)=trim(c.icode) and length(trim(c.icode))=4 and a.mcode=trim(d.type1) and d.id='Y' group by a.subcode,c.iname,a.mcode,d.name order by total asc";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    #region
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    SQuery = "select count (mth) as mth from (select to_char(vchdate,'mm') as mth ,sum(bill_tot/100000) as salee from sale where  branchcd='" + frm_mbr + "' and  type like '4%'  and vchdate " + xprdRange + " group by to_char(vchdate,'mm')) where salee>0";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "Mth";
                        dsRep.Tables.Add(dt);
                    }
                    //////////
                    SQuery = "select sum(total) as basic_tot from (select a.acode,b.aname,a.mcode,D.NAME AS MNAME,a.subcode as type,C.IName as name,round(sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar)/100000,2) as total,round(sum(a.apr)/100000,2) as apr,round(sum(a.may)/100000,2) as may,round(sum(a.jun)/100000,2) as jun,round(sum(a.jul)/100000,2) as jul,round(sum(a.aug)/100000,2) as aug,round(sum(a.sep)/100000,2) as sep,round(sum(a.oct)/100000,2) as oct,round(sum(a.nov)/100000,2) as nov,round(sum(a.dec)/100000,2) as dec,round(sum(a.jan)/100000,2) as jan,round(sum(a.feb)/100000,2) as feb,round(sum(a.mar)/100000,2) as mar from (select trim(acode) as acode,substr(trim(icode),1,2) as mcode,substr(trim(icode),1,4) as subcode,(Case when to_char(vchdate,'mm')='04' then  (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as Apr,(Case when to_char(vchdate,'mm')='05' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as may,(Case when to_char(vchdate,'mm')='06' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jun,(Case when to_char(vchdate,'mm')='07' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jul,(Case when to_char(vchdate,'mm')='08' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as aug,(Case when to_char(vchdate,'mm')='09' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as sep,(Case when to_char(vchdate,'mm')='10' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as oct,(Case when to_char(vchdate,'mm')='11' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as nov,(Case when to_char(vchdate,'mm')='12' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as dec,(Case when to_char(vchdate,'mm')='01' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as jan,(Case when to_char(vchdate,'mm')='02' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0)) else 0 end) as feb,(Case when to_char(vchdate,'mm')='03' then (iamount+nvl(exc_amt,0)+nvl(Cess_pu,0))  else 0 end) as mar from iVOUCHER where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " " + cond + " " + cond1 + ") a,famst b,item c ,type d where trim(a.acode)=trim(b.acode) and trim(a.subcode)=trim(c.icode) and length(trim(c.icode))=4 and a.mcode=trim(d.type1) and d.id='Y' group by a.acode,b.aname,a.subcode,c.iname,a.mcode,d.name order by type)";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "basic_tot";
                        dsRep.Tables.Add(dt);
                    }
                    ////////////////////////////====================
                    header_n = "Our Top 10 Customer(In Order of Turnover)";
                    //SQuery = "select * from (select '" + header_n + "' as headerr, a.acode,b.aname,ROUND(sum(a.iqtyout*a.irate/100000),2) as salee_ FROM IVOUCHER a,famst b where   trim(a.acode)=trim(b.acode)  and SUBSTR(TRIM(A.ACODE),1,2) IN ('02','16') and  a.branchcd='" + frm_mbr + "' and a.type like '4%'  and  a.vchdate " + xprdRange + "  group by a.acode,b.aname order by  salee_ desc) where rownum<='10'"; //all customer without using any selection                   
                    //SQuery = "select * from (select '" + header_n + "' as headerr, a.acode,b.aname,ROUND(sum(a.bill_tot/100000),2) as salee_ FROM sale a,famst b where   trim(a.acode)=trim(b.acode)  and SUBSTR(TRIM(A.ACODE),1,2) IN ('02','16') and  a.branchcd='" + frm_mbr + "' and a.type like '4%'  and  a.vchdate " + xprdRange + " " + cond + " " + cond1 + "  group by a.acode,b.aname order by  salee_ desc) where rownum<='10'"; //customer as per selection group or subgroup
                    SQuery = "select * from (select '" + header_n + "' as headerr, a.acode,b.aname,ROUND(sum(a.iqtyout*a.irate/100000),2) as salee_ FROM IVOUCHER a,famst b where   trim(a.acode)=trim(b.acode)  and SUBSTR(TRIM(A.ACODE),1,2) IN ('02','16') and  a.branchcd='" + frm_mbr + "' and a.type like '4%' AND A.TYPE!='47' and  a.vchdate " + xprdRange + " " + cond + " " + cond1 + "  group by a.acode,b.aname order by  salee_ desc) where rownum<='10'"; //customer as per selection group or subgroup
                    dt2 = new DataTable();
                    dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //party dt
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            dr1 = dt4.NewRow();
                            dr1["col_1"] = dt2.Rows[i]["acode"].ToString().Trim();
                            dr1["col_2"] = dt2.Rows[i]["aname"].ToString().Trim();
                            dr1["col_3"] = dt2.Rows[i]["salee_"].ToString().Trim();
                            dr1["col_4"] = dt2.Rows[i]["headerr"].ToString().Trim();
                            dt4.Rows.Add(dr1);
                        }
                    }
                    //======================================
                    header_n = "Our Top 10 Sale Items(In Order of Turnover)";
                    //SQuery = "select  distinct *  from (select '" + header_n + "' as header, a.icode,b.iname,ROUND(sum(a.iqtyout*a.irate/100000),2)  as salee from ivoucher a,item b where   trim(a.icode)=trim(b.icode) and a.icode like '9%'   and  a.branchcd='" + frm_mbr + "' and a.type like '4%' AND A.TYPE!='47' and  a.vchdate  " + xprdRange + "  group by a.icode,b.iname order by  salee desc) where rownum<='10'";
                    // SQuery = "select  distinct *  from (select '" + header_n + "' as header, a.icode,b.iname,ROUND(sum(a.iamount/100000),2) as salee from ivoucher a,item b where   trim(a.icode)=trim(b.icode) and a.icode like '9%'   and  a.branchcd='" + frm_mbr + "' and a.type like '4%'  and  a.vchdate  " + xprdRange + " and substr(trim(a.icode),1,2) like '" + party_cd + "%' and substr(trim(a.icode),1,4) like '" + part_cd + "%' group by a.icode,b.iname order by  salee desc) where rownum<='10'";
                    SQuery = "select  distinct *  from (select '" + header_n + "' as header, a.icode,b.iname,ROUND(sum(a.iqtyout*a.irate/100000),2)  as salee from ivoucher a,item b where  trim(a.icode)=trim(b.icode) and  a.branchcd='" + frm_mbr + "' and a.type like '4%' AND A.TYPE!='47' and  a.vchdate  " + xprdRange + " " + cond2 + " " + cond3 + "  group by a.icode,b.iname order by  salee desc) where rownum<='10'";// as per se;ection                                    
                    dt3 = new DataTable();
                    dt3 = fgen.getdata(frm_qstr, frm_cocd, SQuery); //item dt
                    //if (dt3.Rows.Count > 0)
                    //{
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        dr1 = dt5.NewRow();
                        dr1["col_5"] = dt3.Rows[i]["icode"].ToString().Trim();
                        dr1["col_6"] = dt3.Rows[i]["iname"].ToString().Trim();
                        dr1["col_7"] = dt3.Rows[i]["salee"].ToString().Trim();
                        dr1["col_8"] = dt3.Rows[i]["header"].ToString().Trim();
                        dt5.Rows.Add(dr1);
                    }
                    // }
                    cnt = dt4.Rows.Count;
                    cnt1 = dt5.Rows.Count;

                    if (dt5.Rows.Count > 0)
                    {//// if there no any row in top sale item dt
                        if (cnt == cnt1)
                        {//if dt5 had rows equal to dt4 and dt5 less than dt4
                            for (int i = 0; i < dt4.Rows.Count; i++)
                            {
                                dt4.Rows[i]["col_5"] = dt5.Rows[i]["col_5"].ToString().Trim();
                                dt4.Rows[i]["col_6"] = dt5.Rows[i]["col_6"].ToString().Trim();
                                dt4.Rows[i]["col_7"] = dt5.Rows[i]["col_7"].ToString().Trim();
                                dt4.Rows[i]["col_8"] = dt5.Rows[i]["col_8"].ToString().Trim();
                            }
                        }
                        if (cnt > cnt1)
                        {
                            for (int i = 0; i < cnt1; i++)
                            {
                                dt4.Rows[i]["col_5"] = dt5.Rows[i]["col_5"].ToString().Trim();
                                dt4.Rows[i]["col_6"] = dt5.Rows[i]["col_6"].ToString().Trim();
                                dt4.Rows[i]["col_7"] = dt5.Rows[i]["col_7"].ToString().Trim();
                                dt4.Rows[i]["col_8"] = dt5.Rows[i]["col_8"].ToString().Trim();
                            }
                        }
                        if (cnt < cnt1)
                        {//dt4 less than dt5
                            for (int i = 0; i < cnt; i++)
                            {
                                dt4.Rows[i]["col_5"] = dt5.Rows[i]["col_5"].ToString().Trim();
                                dt4.Rows[i]["col_6"] = dt5.Rows[i]["col_6"].ToString().Trim();
                                dt4.Rows[i]["col_7"] = dt5.Rows[i]["col_7"].ToString().Trim();
                                dt4.Rows[i]["col_8"] = dt5.Rows[i]["col_8"].ToString().Trim();
                            }
                            for (int i = cnt; i < cnt1; i++)
                            {
                                dr1 = dt4.NewRow();
                                dr1["col_5"] = dt5.Rows[i]["col_5"].ToString().Trim();
                                dr1["col_6"] = dt5.Rows[i]["col_6"].ToString().Trim();
                                dr1["col_7"] = dt5.Rows[i]["col_7"].ToString().Trim();
                                dr1["col_8"] = dt5.Rows[i]["col_8"].ToString().Trim();
                                dt4.Rows.Add(dr1);
                            }
                        }
                    }
                    dt4.TableName = "subrpt";
                    dsRep.Tables.Add(dt4);
                    //================
                    //  SQuery = "select sum(amt_sale/100000) as amt_Sale,sum(amt_exc/100000) as cgst,sum(rvalue/100000) as sgst  from sale where  branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and  vchdate " + xprdRange + ""; //old qry
                    SQuery = "select sum(b.amt_sale/100000) as amt_Sale,sum(b.amt_exc/100000) as cgst,sum(b.rvalue/100000) as sgst  from ivoucher a,sale b where  trim(a.branchcd)||trim(a.type)||trim(a.vchnum)||to_char(a.vchdate,'dd/mm/yyyy')=trim(b.branchcd)||trim(b.type)||trim(b.vchnum)||to_char(b.vchdate,'dd/mm/yyyy') and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.type!='47' and a.vchdate " + xprdRange + " " + cond2 + " " + cond3 + ""; //new qry ...data cominmg as per grp selection
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "mid";
                        dsRep.Tables.Add(dt);
                    }
                    #endregion
                    Print_Report_BYDS(frm_cocd, frm_mbr, "HIMT_Sales_Trend", "HIMT_Sales_Trend", dsRep, header_n);//Sales_Trend_HIMT
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F47111D":
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                cond1 = "";
                if (party_cd.Length < 2)
                {
                    cond = "and trim(acode) like '%'";
                }
                else
                {
                    cond = "and trim(acode) ='" + party_cd + "'";
                }
                if (part_cd.Length < 2)
                {
                    cond1 = "and trim(icode) like '%'";
                }
                else
                {
                    cond1 = "and trim(icode)='" + part_cd + "'";
                }
                mq0 = ""; mq1 = "";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");//CHOICE
                dt = new DataTable();
                mq0 = "select substr(solink,1,20) as solink,ACODE,icode,BUDGETCOST as qty,actualCOST as tentqty,000000000.00 AS job,000000000.00 AS SALE from budgmst  where  BRANCHCD='" + frm_mbr + "' AND type='46' and to_Date(desc_,'dd/mm/yyyy') " + xprdRange + " " + cond + " " + cond1 + " and BUDGETCOST+actualCOST>0 union all select convdate AS SOLINK,ACODE,icode,000000000.00 AS QTY ,000000000.00 AS tentqty,qty as job,000000000.00 AS SALE  from costestimate where BRANCHCD='" + frm_mbr + "' AND SUBSTR(type,1,2)='30' and VCHDATE " + xprdRange + " " + cond + " " + cond1 + " and srno=1 union all  select BRANCHCD||TYPE||PONUM||TO_CHAR(PODATE,'DD/MM/YYYY') AS SOLINK,ACODE,icode,000000000.00 AS qty,000000000.00 AS tentqty,000000000.00 as job,IQTYOUT as sale from IVOUCHER where  BRANCHCD='" + frm_mbr + "' AND SUBSTR(type,1,1)='4' and VCHDATE " + xprdRange + " " + cond + " " + cond1 + " and IQTYOUT>0";
                if (mq1 == "Y")//SALE ORDER WISE
                {
                    header_n = "Delivery Monitoring Report";
                    SQuery = "select '" + fromdt + "' AS FROMDT,'" + todt + "' AS TODT,'" + header_n + "'  AS HEADER,substr(a.solink,1,20) as solink,trim(B.ANAME) as aname,trim(C.INAME) AS CINAME,trim(C.CPARTNO) as cpartno,trim(A.ACODE) as acode,trim(A.ICODE) as icode,SUM(A.QTY) AS QTY,SUM(A.tentqty) AS tQTY,SUM(A.job) AS jQTY,SUM(A.SALE) AS SALEs  FROM ( " + mq0 + " )  A,FAMST B , ITEM C WHERE trim(A.ACODE)=trim(B.ACODE) AND trim(A.ICODE)=trim(C.ICODE) GROUP BY substr(a.solink,1,20),trim(A.ACODE),trim(B.ANAME),trim(C.INAME),trim(A.ICODE),trim(C.CPARTNO)  ORDER BY ACODE,CPARTNO,solink"; //this qry for sale order wise
                    frm_rptName = "Sales_Monitoring_Rep_SO"; //rpt name
                }
                else
                { //ITEM WISE
                    header_n = "Delivery Monitoring Report";
                    SQuery = "select '" + fromdt + "' AS FROMDT,'" + todt + "' AS TODT,'" + header_n + "' AS HEADER, trim(B.ANAME) as aname,trim(C.INAME) AS CINAME,trim(C.CPARTNO) AS CPARTNO,trim(A.ACODE) as acode,trim(A.ICODE) as icode,SUM(A.QTY) AS QTY,SUM(A.tentqty) AS tQTY,SUM(A.job) AS jQTY,SUM(A.SALE) AS SALEs  FROM (" + mq0 + ") A,FAMST B , ITEM C WHERE trim(A.ACODE)=trim(B.ACODE) AND trim(A.ICODE)=trim(C.ICODE) GROUP BY trim(B.ANAME),trim(C.INAME),trim(C.CPARTNO),trim(A.ACODE),trim(A.ICODE)  ORDER BY acode,cpartno";
                    frm_rptName = "Sales_Monitoring_Rep";//rpt name
                }
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, frm_rptName, frm_rptName, dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F50328":
                #region
                header_n = "Schedule Vs Rcpt Vs Despatch Detail Report";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (party_cd.Length < 2)
                {
                    party_cd = "16";
                }
                if (part_cd.Length < 2)
                {
                    part_cd = "9";
                }

                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,A.ident,a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit,sum(a.day1) as Day_01,sum(a.day2) as day_02,sum(a.day3) as day_03,sum(a.day4) as day_04,sum(a.day5) as day_05,sum(a.day6) as day_06,sum(a.day7) as day_07,sum(a.day8) as day_08,sum(a.day9) as day_09,sum(a.day10) as day_10,sum(a.day11) as day_11,sum(a.day12) as day_12,sum(a.day13) as day_13,sum(a.day14) as day_14,sum(a.day15) as day_15,sum(a.day16) as day_16,sum(a.day17) as day_17,sum(a.day18) as day_18,sum(a.day19) as day_19,sum(a.day20) as day_20,sum(a.day21) as day_21,sum(a.day22) as day_22,sum(a.day23) as day_23,sum(a.day24) as day_24,sum(a.day25) as day_25,sum(a.day26) as day_26,sum(a.day27) as day_27,sum(a.day28) as day_28,sum(a.day29) as day_29,sum(a.day30) as day_30,sum(a.day31) as day_31 from (SELECT Acode,icode,'1)S' as ident,DAY1,DAY2,DAY3,day4,day5,day6,day7,day8,day9,day10, Day11,day12,day13,day14,day15,day16,day17 ,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31 FROM SCHEDULE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='46' and VCHDATE " + xprdRange + " UNION ALL  SELECT acode,icode,'2)R' as ident,(Case when to_char(vchdate,'dd')='01' then iqtyin else 0 end) as DAY1,(Case when to_char(vchdate,'dd')='02' then iqtyin else 0 end) as DAY2,(Case when to_char(vchdate,'dd')='03' then iqtyin else 0 end) as DAY3,(Case when to_char(vchdate,'dd')='04' then iqtyin else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyin else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyin else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyin else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyin else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyin else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyin else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyin else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyin else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyin else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyin else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyin else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyin else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyin else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyin else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyin else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyin  else 0 end) as DAY20,(Case when to_char(vchdate,'dd')='21' then iqtyin else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyin  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyin else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyin  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyin  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyin else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyin else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyin  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyin  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyin  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyin  else 0 end) as Rday31 from ivoucher where branchcd='" + frm_mbr + "' and type='08' and store='Y' and VCHDATE " + xprdRange + " and nvl(iqtyin,0)>0 union all SELECT acode,icode,'3)D' as ident,(Case when to_char(vchdate,'dd')='01' then iqtyout else 0 end) as DAY1,(Case when to_char(vchdate,'dd')='02' then iqtyout else 0 end) as DAY2,(Case when to_char(vchdate,'dd')='03' then iqtyout else 0 end) as DAY3,(Case when to_char(vchdate,'dd')='04' then iqtyout else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyout else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyout else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyout else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyout else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyout else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyout else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyout else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyout else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyout else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyout else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyout else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyout else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyout else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyout else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyout else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyout  else 0 end) as Rday20,(Case when to_char(vchdate,'dd')='21' then iqtyout else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyout  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyout else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyout  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyout  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyout else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyout else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyout  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyout  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyout  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyout  else 0 end) as Rday31 from ivoucher where branchcd='" + frm_mbr + "' and SUBSTR(TYPE,1,1) IN ('2','4') AND STORE='Y' and VCHDATE " + xprdRange + " and nvl(iqtyout,0)>0) a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%'group by A.ident,a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit order by a.icode,A.ident";//with sum
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sch_Vs_Rcpt_Vs_Desp_DayWise", "std_Sch_Vs_Rcpt_Vs_Desp_DayWise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F50329":
                #region
                header_n = "Schedule Vs Rcpt Vs Despatch Summary Report";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                if (party_cd.Length < 2)
                {
                    party_cd = "16";
                }
                if (part_cd.Length < 2)
                {
                    part_cd = "9";
                }
                mq0 = fromdt;
                mq1 = todt;
                mq2 = "";
                double days = DateTime.DaysInMonth(fgen.make_int(mq0.Substring(6, 4)), fgen.make_int(mq0.Substring(3, 2)));
                double d1 = fgen.make_double(todt.Substring(0, 2));
                xprdRange1 = "between to_Date('" + frm_cDt1 + "','dd/mm/yyyy') and to_date('" + fromdt + "','dd/mm/yyyy')-1";
                //mq2 = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as IOPQTY,nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' union all select branchcd,icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " " + cond + " and store='Y'  GROUP BY ICODE,branchcd union all select branchcd,trim(icode) as icode,0 as op,sum(nvl(iqtyin,0)) as cdr,sum(nvl(iqtyout,0)) as ccr,0 as clos from IVOUCHER where branchcd='" + frm_mbr + "' and type like '%'  and vchdate " + xprdRange + " " + cond + " and store='Y' GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE"; //op bal and clos bal dt
                mq2 = "select branchcd,trim(icode) as icode,nvl(sum(opening),0) as op from (Select A.branchcd,A.icode, a.yr_" + frm_myear + " as opening,0 as cdr,0 as ccr,0 as clos from itembal a where a.branchcd='" + frm_mbr + "' union all select branchcd,icode,sum(nvl(iqtyin,0))-sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr,0 as clos FROM IVOUCHER where branchcd='" + frm_mbr + "' and type like '%' and vchdate " + xprdRange1 + " " + cond + " and store='Y'  GROUP BY ICODE,branchcd ) where LENGTH(tRIM(ICODE))>=8 group by branchcd,trim(icode) ORDER BY ICODE";//only op dt//nvl(sum(cdr),0) as qtyin,nvl(sum(ccr),0) as qtyout,sum(opening)+sum(cdr)-sum(ccr) as cl
                dt1 = new DataTable();
                dt1 = fgen.getdata(frm_qstr, frm_cocd, mq2);//stock dt
                //====================
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,'" + days + "' as days,'" + d1 + "' as d1,ACODE,ICODE,ANAME,INAME,CPARTNO,UNIT,SUM(SCH_QTY) AS SCH_QTY,SUM(RCPT_qTY) AS RCPT_qTY,SUM(DESP_QTY) AS DESP_QTY FROM(select acode,icode,aname,iname,cpartno,unit,decode(ident,'1)S',TOT,0) as sch_qty,decode(ident,'2)R',TOT,0) AS RCPT_qTY,decode(ident,'3)D',TOT,0) AS DESP_QTY FROM (select a.acode,a.icode, A.ident,trim(a.acode)||trim(a.icode) as fstr,b.aname,c.iname,c.cpartno,c.unit,(a.day1+a.day2+a.day3+a.day4+a.day5+a.day6 +a.day7+a.day8+a.day9+a.day10+a.day11+a.day12+a.day13+a.day14+a.day15+a.day16+a.day17+a.day18+a.day19+a.day20+a.day21+a.day22+a.day23+a.day24+a.day25+a.day26+a.day27+a.day28+a.day29 +a.day30+a.day31) AS TOT  from (SELECT Acode,icode,'1)S' as ident,DAY1,DAY2,DAY3,day4,day5,day6,day7,day8,day9,day10, Day11,day12,day13,day14,day15,day16,day17 ,day18,day19,day20,day21,day22,day23,day24,day25,day26,day27,day28,day29,day30,day31 FROM SCHEDULE WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='46' and VCHDATE " + xprdRange + " UNION ALL  SELECT acode,icode,'2)R' as ident,(Case when to_char(vchdate,'dd')='01' then iqtyin else 0 end) as DAY1,(Case when to_char(vchdate,'dd')='02' then iqtyin else 0 end) as DAY2,(Case when to_char(vchdate,'dd')='03' then iqtyin else 0 end) as DAY3,(Case when to_char(vchdate,'dd')='04' then iqtyin else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyin else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyin else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyin else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyin else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyin else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyin else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyin else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyin else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyin else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyin else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyin else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyin else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyin else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyin else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyin else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyin  else 0 end) as DAY20,(Case when to_char(vchdate,'dd')='21' then iqtyin else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyin  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyin else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyin  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyin  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyin else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyin else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyin  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyin  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyin  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyin  else 0 end) as Rday31 from ivoucher where branchcd='" + frm_mbr + "' and type='08' and store='Y' and VCHDATE " + xprdRange + " and nvl(iqtyin,0)>0 union all SELECT acode,icode,'3)D' as ident,(Case when to_char(vchdate,'dd')='01' then iqtyout else 0 end) as DAY1,(Case when to_char(vchdate,'dd')='02' then iqtyout else 0 end) as DAY2,(Case when to_char(vchdate,'dd')='03' then iqtyout else 0 end) as DAY3,(Case when to_char(vchdate,'dd')='04' then iqtyout else 0 end) as Rday4,(Case when to_char(vchdate,'dd')='05' then iqtyout else 0 end) as Rday5,(Case when to_char(vchdate,'dd')='06' then iqtyout else 0 end) as Rday6 ,(Case when to_char(vchdate,'dd')='07' then iqtyout else 0 end) as Rday7,(Case when to_char(vchdate,'dd')='08' then iqtyout else 0 end) as Rday8,(Case when to_char(vchdate,'dd')='09' then iqtyout else 0 end) as Rday9,(Case when to_char(vchdate,'dd')='10' then iqtyout else 0 end) as Rday10,(Case when to_char(vchdate,'dd')='11' then iqtyout else 0 end) as Rday11,(Case when to_char(vchdate,'dd')='12' then iqtyout else 0 end) as Rday12,(Case when to_char(vchdate,'dd')='13' then iqtyout else 0 end) as Rday13,(Case when to_char(vchdate,'dd')='14' then iqtyout else 0 end) as Rday14,(Case when to_char(vchdate,'dd')='15' then iqtyout else 0 end) as Rday15,(Case when to_char(vchdate,'dd')='16' then iqtyout else 0 end) as Rday16,(Case when to_char(vchdate,'dd')='17' then iqtyout else 0 end) as Rday17,(Case when to_char(vchdate,'dd')='18' then iqtyout else 0 end) as Rday18,(Case when to_char(vchdate,'dd')='19' then iqtyout else 0 end) as Rday19,(Case when to_char(vchdate,'dd')='20' then iqtyout  else 0 end) as Rday20,(Case when to_char(vchdate,'dd')='21' then iqtyout else 0 end) as Rday21,(Case when to_char(vchdate,'dd')='22' then iqtyout  else 0 end) as Rday22,(Case when to_char(vchdate,'dd')='23' then iqtyout else 0 end) as Rday23,(Case when to_char(vchdate,'dd')='24' then iqtyout  else 0 end) as Rday24,(Case when to_char(vchdate,'dd')='25' then iqtyout  else 0 end) as Rday25,(Case when to_char(vchdate,'dd')='26' then iqtyout else 0 end) as Rday26,(Case when to_char(vchdate,'dd')='27' then iqtyout else 0 end) as Rday27,(Case when to_char(vchdate,'dd')='28'  then iqtyout  else 0 end) as Rday28,(Case when to_char(vchdate,'dd')='29'  then iqtyout  else 0 end) as Rday29,(Case when to_char(vchdate,'dd')='30'  then iqtyout  else 0 end) as Rday30,(Case when to_char(vchdate,'dd')='31'  then iqtyout  else 0 end) as Rday31 from ivoucher where branchcd='" + frm_mbr + "' and SUBSTR(TYPE,1,1) IN ('2','4') AND STORE='Y'  and VCHDATE " + xprdRange + " and nvl(iqtyout,0)>0) a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' order by a.icode,A.ident) ) GROUP BY ACODE,ICODE,ANAME,INAME,CPARTNO,UNIT order by iname";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                dt.Columns.Add("sch_expted", typeof(double));
                dt.Columns.Add("rcpt_expted", typeof(double));
                dt.Columns.Add("desp_expted", typeof(double));
                dt.Columns.Add("rcpt_compliance", typeof(double));
                dt.Columns.Add("desp_compliance", typeof(double));
                dt.Columns.Add("op", typeof(double));
                dr1 = dt.NewRow();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //expected formula====
                        db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; double db7 = 0;
                        dt.Rows[i]["op"] = fgen.make_double(fgen.seek_iname_dt(dt1, "icode='" + dt.Rows[i]["icode"].ToString().Trim() + "'", "op"));
                        db = (d1 / days);
                        dt.Rows[i]["sch_expted"] = Math.Round(db * fgen.make_double(dt.Rows[i]["SCH_QTY"].ToString().Trim().Replace("NaN", "0")), 2);
                        dt.Rows[i]["rcpt_expted"] = Math.Round(db * fgen.make_double(dt.Rows[i]["RCPT_qTY"].ToString().Trim().Replace("NaN", "0")), 2);//as per client no need to show in report
                        dt.Rows[i]["desp_expted"] = Math.Round(db * fgen.make_double(dt.Rows[i]["DESP_QTY"].ToString().Trim().Replace("NaN", "0")), 2);//as per client no need to show in report                 
                        db1 = fgen.make_double(dt.Rows[i]["RCPT_qTY"].ToString().Trim().Replace("NaN", "0"));
                        db2 = fgen.make_double(dt.Rows[i]["rcpt_expted"].ToString().Trim().Replace("NaN", "0"));
                        db7 = fgen.make_double(dt.Rows[i]["sch_expted"].ToString().Trim().Replace("NaN", "0"));
                        if (db1 != 0 && db7 != 0)
                        {
                            db3 = Math.Round((db1 / db7) * 100, 2);//as per client                        
                        }
                        else
                        {
                            db3 = 0;
                        }
                        dt.Rows[i]["rcpt_compliance"] = db3;
                        db4 = fgen.make_double(dt.Rows[i]["DESP_QTY"].ToString().Trim().Replace("NaN", "0"));
                        if (db4 != 0 && db7 != 0)
                        {
                            db6 = Math.Round((db4 / db7) * 100, 2);
                        }
                        else
                        {
                            db6 = 0;
                        }
                        dt.Rows[i]["desp_compliance"] = db6;
                    }
                    ///============================
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Std_sch_vs_rcpt_desp_smry", "Std_sch_vs_rcpt_desp_smry", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
        }
    }

    public void Print_Report_BYDS(string frm_cocd, string frm_mbr, string xml, string report, DataSet data_set, string title)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/Report/" + report.Trim() + ".rpt";
        data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, frm_cocd, frm_mbr));
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
            if (pdfView == "Y")
                conv_pdf(data_set, rptfile);
        }
        else
        {
        }
        data_set.Dispose();
    }

    public void Print_Report_BYDS_pdf(string frm_cocd, string frm_mbr, string xml, string report, DataSet data_set, string title)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/Report/" + report.Trim() + ".rpt";
        data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, frm_cocd, frm_mbr));
        data_set.WriteXml(xfilepath, XmlWriteMode.WriteSchema);
        if (data_set.Tables[0].Rows.Count > 0)
        {
            Session["data_set"] = data_set;
            Session["rptfile"] = rptfile;
            conv_pdf(data_set, rptfile);
            //if try catch uncommented the pdf file will be downloaded directly. comment the above conv_pdf(data_set, rptfile) line only. otherwise get pdf in pdf viewer. 
            //try 
            //{
            //    frm_FileName = frm_cocd + "_" + DateTime.Now.ToString().Trim();
            //    DataSet ds = (DataSet)Session["data_set"];
            //    string rpt = (string)Session["rptfile"];
            //    repDoc = GetReportDocument(ds, rpt);
            //    repDoc.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Response, true, frm_FileName);
            //}
            //catch { }
        }
        else
        {
        }
        data_set.Dispose();
    }

    public void Print_Report_BYDS(string frm_cocd, string frm_mbr, string xml, string report, DataSet data_set, string title, string addlogo)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/Report/" + report.Trim() + ".rpt";

        if (addlogo == "Y") data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, frm_cocd, frm_mbr, "Y"));
        else data_set.Tables.Add(fgen.Get_Type_Data(frm_qstr, frm_cocd, frm_mbr));

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
            if (pdfView == "Y")
                conv_pdf(data_set, rptfile);
        }
        else
        {
        }
        data_set.Dispose();
    }
    public void Print_Report_BYDS2(string frm_cocd, string frm_mbr, string xml, string report, DataSet data_set, string title, string addlogo)
    {
        string xfilepath = Server.MapPath("~/tej-base/XMLFILE/" + xml.Trim() + ".xml");
        string rptfile = "~/tej-base/Report/" + report.Trim() + ".rpt";

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
            if (hfhcid.Value == "F49212" && (frm_cocd == "MLGI" || frm_cocd == "ELEC" || frm_cocd == "WING" || frm_cocd == "STUD"))
            {
                Session["rptfile"] = rptfile;
                Session["data_set"] = data_set;
                printDsc(data_set, rptfile);
            }
            else if (pdfView == "Y")
                conv_pdf(data_set, rptfile);
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
            if (frm_cocd == "KRSM")
            {
                try { pdfno = ds.Tables["Prepcur"].Rows[0]["fstr"].ToString(); } catch { }
                try { pdffirm = ds.Tables["Prepcur"].Rows[0]["Aname"].ToString(); } catch { }
                if (hfhcid.Value == "F50106" || hfhcid.Value == "F55106") { pdfdoc = "Proforma"; }
                //else { try { pdfdoc = ds.Tables["Prepcur"].Rows[0]["name"].ToString(); } catch { } }
                else { pdfdoc = "Invoice"; }
                frm_FileName = pdfdoc.Replace(' ', '_') + "__" + pdffirm.Replace(' ', '_') + "__" + pdfno;
            }
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

    protected void btnExptoTiff_Click(object sender, ImageClickEventArgs e)
    {
        expToTiff();

        string tiffPath = @"c:\TEJ_ERP\tiff";
        DirectoryInfo di = new DirectoryInfo(tiffPath);
        string[] allFiles = (string[])ViewState["frm_pdfname"];
        var files = di.GetFiles();
        for (int i = 0; i < allFiles.Length; i++)
        {
            var filesToDownload = files.Where(r => r.Name.Contains(allFiles[i].ToString().Substring(0, 20)));
            foreach (FileInfo file in filesToDownload)
            {
                Session["FilePath"] = tiffPath;
                Session["FileName"] = file.Name.ToString();
                Response.Write("<script>");
                Response.Write("window.open('../tej-base/dwnlodFile.aspx','_blank')");
                Response.Write("</script>");
            }
        }
    }

    void expToTiff()
    {
        frm_FileName = frm_cocd + "_" + DateTime.Now.ToString("dd_MM_yy").Trim();
        DataSet ds = (DataSet)Session["data_set"];
        string rpt = (string)Session["rptfile"];
        DataTable dtDistEntryNo = new DataTable();
        DataView dv = new DataView(ds.Tables[0], "", "", DataViewRowState.CurrentRows);
        dtDistEntryNo = dv.ToTable(true, "VCHNUM");
        string frm_pdfName = "";
        string[] allFiles = new string[dtDistEntryNo.Rows.Count];
        int a = 0;
        foreach (DataRow dr in dtDistEntryNo.Rows)
        {
            DataTable newDt = new DataTable();
            DataSet newDs = new DataSet();
            DataView dvN = new DataView(ds.Tables[0], "VCHNUM='" + dr["vchnum"].ToString().Trim() + "' AND MTITLESRNO='0'", "", DataViewRowState.CurrentRows);
            newDt = dvN.ToTable();
            newDs.Tables.Add(newDt);
            repDoc = GetReportDocument(newDs, rpt);
            ExportOptions expOpt;
            DiskFileDestinationOptions crDiskFileDest = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions crFormatType = new PdfRtfWordFormatOptions();
            frm_pdfName = "Invoice_No." + dr["vchnum"].ToString().Trim() + ".pdf";
            frm_FileName = "c:\\TEJ_erp\\PDF\\" + frm_pdfName;
            crDiskFileDest.DiskFileName = frm_FileName;

            expOpt = repDoc.ExportOptions;
            {
                expOpt.ExportDestinationType = ExportDestinationType.DiskFile;
                expOpt.ExportFormatType = ExportFormatType.PortableDocFormat;
                expOpt.DestinationOptions = crDiskFileDest;
                expOpt.FormatOptions = crFormatType;
            }
            repDoc.Export();

            fgen.convertPdfToTiff(frm_FileName, "c:\\TEJ_erp\\tiff");

            allFiles[a] = frm_pdfName;
            a++;
        }
        ViewState["frm_pdfname"] = allFiles;
    }

    protected void btnexpwithsig_Click(object sender, ImageClickEventArgs e)
    {
        frm_FileName = frm_cocd + "_" + DateTime.Now.ToString("dd_MM_yy").Trim();
        DataSet ds = (DataSet)Session["data_set"];
        string rpt = (string)Session["rptfile"];

        rpt = "std_inv_stud_all";

        Print_Report_BYDS2(frm_cocd, frm_mbr, rpt, rpt, ds, "", "Y");
    }

    protected void btnsendmail_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string aname1 = "", mq1 = "", mq10 = "";
            frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
            DataTable dt = new DataTable();
            DataTable mdt = new DataTable();
            DataTable fdt = new DataTable();
            DataSet data_set = new DataSet();
            data_set = (DataSet)Session["data_set"];
            DataView dv = new DataView(data_set.Tables["Prepcur"], "", "acode", DataViewRowState.CurrentRows);

            fdt = data_set.Tables["Prepcur"];
            mdt = dv.ToTable(true, "acode", "p_email");
            DataSet dsRep = new DataSet();
            DataRow dr;
            dt = fdt.Clone();

            foreach (DataRow dr1 in mdt.Rows)
            {
                if (dr1["p_email"].ToString().Length > 2)
                {
                    dsRep = new DataSet();// FOR REMOVING DATATABLE ALREADY BELONGS TO ANOTHER DATASET
                    dt = new DataTable();// FOR REMOVING DATATABLE ALREADY BELONGS TO THIS DATASET
                    dt = fdt.Clone();
                    DataTable dt1 = new DataTable();
                    dv = new DataView(fdt, "acode='" + dr1["acode"].ToString().Trim() + "'", "acode", DataViewRowState.CurrentRows);
                    dt1 = dv.ToTable();
                    foreach (DataRow drdt1 in dt1.Rows)
                    {
                        dr = dt.NewRow();
                        aname1 = drdt1["aname"].ToString().Trim();
                        mq1 = drdt1["vchnum"].ToString().Trim() + " Dated " + drdt1["vchdate"].ToString().Trim() + ", Rs " + drdt1["bill_tot"].ToString().Trim();
                        foreach (DataColumn dcdt in dt.Columns)
                        {
                            if (drdt1[dcdt.ColumnName] == null) dr[dcdt.ColumnName] = 0;
                            else dr[dcdt.ColumnName] = drdt1[dcdt.ColumnName];
                        }
                        dt.Rows.Add(dr);
                    }

                    string repname = "";
                    frm_formID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_FORMID");
                    if (frm_formID == "F49212")
                    {
                        repname = "std_inv";
                        //if (frm_cocd == "MLGI" || frm_cocd == "WING" || frm_cocd == "STUD")
                        //    repname = "std_inv_DSC";
                    }

                    xhtml_tag = "";
                    html_body(aname1, mq1);
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);

                    for (int i = 0; i < data_set.Tables.Count; i++)
                    {
                        if (data_set.Tables[i].TableName != "Prepcur")
                        {
                            DataTable newDt = new DataTable();
                            DataRow newDR;
                            newDt = data_set.Tables[i].Clone();
                            foreach (DataRow drdt1 in data_set.Tables[i].Rows)
                            {
                                newDR = newDt.NewRow();
                                foreach (DataColumn dcdt in data_set.Tables[i].Columns)
                                {
                                    if (drdt1[dcdt.ColumnName] == null) newDR[dcdt.ColumnName] = 0;
                                    else newDR[dcdt.ColumnName] = drdt1[dcdt.ColumnName];
                                }
                                newDt.Rows.Add(newDR);
                            }
                            newDt.TableName = data_set.Tables[i].TableName;
                            dsRep.Tables.Add(newDt);
                        }
                    }

                    Print_Report_BYDS2(frm_cocd, frm_mbr, repname, repname, dsRep, "", "Y");
                    Attachment atchfile = null;
                    if (ViewState["frm_pdfname"] != null)
                    {
                        atchfile = new Attachment("c:\\TEJ_erp\\tiff\\" + ViewState["frm_pdfname"].ToString());
                    }
                    ViewState["frm_pdfname"] = null;

                    fgen.send_mail(frm_qstr, frm_cocd, (frm_cocd == "KLAS" ? "" : "Tejaxo ERP"), dr1["p_email"].ToString().Trim(), txtemailcc.Text, txtemailbcc.Text, subj, xhtml_tag, atchfile, "2");
                    repDoc.Close(); repDoc.Dispose(); CrystalReportViewer1.Dispose();
                }
            }
            fgen.send_cookie("Send_Mail", "N");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "abc", "closePopup('#btnsendmail');", true);
        }
        catch (Exception ex)
        {
        }
    }

    public void html_body(string party_name, string oth_var1)
    {
        firm = fgenCO.chk_co(frm_cocd);
        firm = firm.Replace("XXXX", frm_cocd);

        if (frm_formID == "F49212" && frm_cocd == "MLGI")
        {
            xhtml_tag = xhtml_tag + "<br>M/s " + firm + " <br>";
            xhtml_tag = xhtml_tag + "" + fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ADDR AS FSTR FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "FSTR") + "<br>";
            xhtml_tag = xhtml_tag + "" + fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ADDR1 AS FSTR FROM TYPE WHERE ID='B' AND TYPE1='" + frm_mbr + "'", "FSTR") + "<br>";
            xhtml_tag = xhtml_tag + "===============================<br>";
        }
        xhtml_tag = xhtml_tag + "<br>To<br>";
        xhtml_tag = xhtml_tag + "M/s " + party_name + "<br>";
        xhtml_tag = xhtml_tag + "<br>Dear Sir/Madam,<br>";

        switch (frm_formID)
        {
            case "F49212":
                subj = "Tejaxo ERP: Invoice Number " + oth_var1 + " (" + party_name + ")";
                xhtml_tag = xhtml_tag + "<br>Please find attached the Invoice Number " + oth_var1 + "";
                xhtml_tag = xhtml_tag + "<br>This is digitally Signed Invoice, Kindly Print the Invoice and book in your accounts.";
                break;
        }
        xhtml_tag = xhtml_tag + "<br><br><b>Thanks & Regards,</b>";
        xhtml_tag = xhtml_tag + "<br><b>" + firm + "</b>";
        if (frm_cocd != "MLGI")
            xhtml_tag = xhtml_tag + "<br><br><br>Note: This is an automatically generated email from Tejaxo ERP, Please do not reply";
        xhtml_tag = xhtml_tag + "</body></html>";
    }

    void printDsc(DataSet dataSet, string rptFile)
    {
        //string frm_FileName = frm_cocd + "_" + DateTime.Now.ToString("dd_MM_yy").Trim();
        //DataSet ds = dataSet;
        //string rpt = rptFile;
        //DataTable dtDistEntryNo = new DataTable();
        //DataView dv = new DataView(ds.Tables["Prepcur"], "", "", DataViewRowState.CurrentRows);
        //dtDistEntryNo = dv.ToTable(true, "VCHNUM");
        //string frm_pdfName = "";
        //string[] allFiles = new string[dtDistEntryNo.Rows.Count];
        //string tiffPath = @"c:\TEJ_ERP\tiff\";
        //int k = 0;

        //col1 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT NUM4||'~'||NUM5 AS FSTR FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + 1006 + "' ", "FSTR");

        ////left pad
        //int a = 710;
        //// bottom pad
        //int b = 50;
        ////width

        //if (col1.Split('~')[0].ToString().toDouble() > 0)
        //{
        //    a = fgen.make_int(col1.Split('~')[0].ToString());
        //    b = fgen.make_int(col1.Split('~')[1].ToString());
        //}

        //int c = a + 100;
        ////height
        //int d = b + 40;

        //string dscPanNo = "", dscAuthName = "", dscNametoPrint = "";

        //dscAuthName = fgen.dscAuthName(frm_qstr, frm_cocd, frm_mbr, frm_uname);
        //dscNametoPrint = fgen.dscNametoPrint(frm_qstr, frm_cocd, frm_mbr, frm_uname);
        //dscPanNo = fgen.dscPanNo(frm_qstr, frm_cocd, frm_mbr, frm_uname);

        //foreach (DataRow dr in dtDistEntryNo.Rows)
        //{
        //    try
        //    {
        //        DataTable newDt = new DataTable();
        //        DataSet newDs = new DataSet();
        //        DataView dvN = new DataView(ds.Tables["Prepcur"], "VCHNUM='" + dr["vchnum"].ToString().Trim() + "' ", "", DataViewRowState.CurrentRows);
        //        newDt = dvN.ToTable();
        //        newDs.Tables.Add(newDt);
        //        frm_pdfName = "Invoice_No." + dr["vchnum"].ToString().Trim() + ".pdf";
        //        frm_FileName = Server.MapPath(@"~\tej-base\xmlfile\" + frm_pdfName);

        //        repDoc.Refresh();
        //        repDoc.ExportToDisk(ExportFormatType.PortableDocFormat, frm_FileName);

        //        repDoc.Dispose();

        //        string path1 = Server.MapPath(@"~\tej-base\xmlfile\" + frm_pdfName);
        //        path1 = Server.MapPath(@"~\tej-base\xmlfile\" + frm_pdfName);

        //        FileInfo fi = new FileInfo(path1);
        //        BinaryReader br = new BinaryReader(fi.OpenRead());

        //        Webtel_e_Sign.Res rr = new Webtel_e_Sign.Res();

        //        Webtel_e_Sign.ESign aa = new Webtel_e_Sign.ESign(ConnInfo.IP, "FIN" + frm_cocd, ConnInfo.nPwd, ConnInfo.srv, "1521", "2");

        //        //-2 for last page
        //        //-1 for every page

        //        rr = aa.SignPDF(br.ReadBytes((int)fi.Length), dscAuthName, dscNametoPrint, a, b, c, d, "", frm_pdfName, -1, "", -1);

        //        string path2 = tiffPath + frm_pdfName;
        //        string path3 = "c:\\TEJ_erp\\pdf\\" + frm_pdfName;

        //        if (rr.Error_Detail != "")
        //        {
        //            fgen.FILL_ERR(rr.Error_Detail);
        //        }

        //        File.WriteAllBytes(path2, rr.OutputFile);

        //        allFiles[k] = frm_pdfName;
        //        br.Close();
        //        br.Dispose();
        //        k++;
        //        br.Dispose();

        //        //convertPdfToDSC(v);
        //    }
        //    catch (Exception ex)
        //    {
        //        fgen.FILL_ERR("DSC Conv : " + ex.Message + " " + frm_pdfName);
        //    }
        //    ViewState["frm_pdfname"] = frm_pdfName;
        //}
    }
}