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

public partial class emktg_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, DateRange, cond, frm_uname, frm_myear, part_cd, party_cd, SQuery, frm_rptName, str, xprdRange, pdfView, xprdRange1, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl;
    string frm_FileName = "", frm_formID = "", frm_UserID, data_found = "", header_n, fromdt, todt;
    double db, db1, db2, db3, db4, db5, db6, db7, db8, db9, db0;
    string m1, mq0, mq1, mq2, mq3, mq4, mq5, mq6, mq7, mq8, mq9, mq10;
    DataTable dticode, dticode1, dticode2;
    int i0;

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
                    DateRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_DATERANGE");
                    xprdRange = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PRDRANGE");
                    frm_UserID = fgenMV.Fn_Get_Mvar(frm_qstr, "U_USERID");

                    frm_cDt1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt1");
                    frm_cDt2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Cdt2");

                    fromdt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT1");
                    todt = fgenMV.Fn_Get_Mvar(frm_qstr, "U_MDT2");
                    xprdRange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";

                    hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    pdfView = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PDFVIEW");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", "-");
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
        string mq10, mq1, mq0, mq2, mq3;
        int repCount = 1;
        frm_rptName = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ACREF FROM TYPEGRP WHERE ID='X1' AND TYPE1='" + iconID.Replace("F", "") + "' ", "ACREF");
        string opt = "";
        data_found = "Y";

        switch (iconID)
        {
            case "F1015":
                #region sale SCH
                SQuery = "select d.mthname, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.* from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum)  AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum ";
                col1 = "NO";
                if (col1 == "YES")
                {
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,Round((DAY1/1000),1) as  DAY1 , round((A.DAY2/1000),1) AS DAY2,round((A.DAY3/10000),1) AS DAY3,round((A.DAY4/1000),1) AS DAY4,round((A.DAY5/1000),1) AS DAY5,round((A.DAY6/1000),1)  AS DAY6,round((A.DAY7/1000),1) AS DAY7,round((A.DAY8/1000),1) AS DAY8,round((A.DAY9/1000),1) AS DAY9,round((A.DAY10/1000),1) AS DAY10,round((A.DAY11/1000),1) AS DAY11,round((A.DAY12/1000),1) AS DAY12,round((A.DAY13/1000),1) AS DAY13,round((A.DAY14/1000),1) AS DAY14,round((A.DAY15/1000),1) AS DAY15,round((A.DAY16/1000),1) AS DAY16,round((A.DAY17/1000),1) AS DAY17,round((A.DAY18/1000),1) AS DAY18,round((A.DAY19/1000),1) AS DAY19,round((A.DAY20/1000),1) AS DAY20,round((A.DAY21/1000),1) AS DAY21,round((A.DAY22/1000),1) AS DAY22,round((A.DAY23/1000),1) AS DAY23,round((A.DAY24/1000),1) AS DAY24,round((A.DAY25/1000),1) AS DAY25,round((A.DAY26/1000),1) AS DAY26,round((A.DAY27/1000),1) AS DAY27,round((A.DAY28/1000),1) AS DAY28,round((A.DAY29/1000),1) AS DAY29,round((A.DAY30/1000),1)  AS DAY30,round((A.DAY31/1000),1) AS DAY31,round((A.TOTAL/1000),1) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT ,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum ";
                }
                if (col1 == "NO")
                {
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_,C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,DAY1,A.DAY2,A.DAY3,A.DAY4,A.DAY5,A.DAY6,A.DAY7,A.DAY8,A.DAY9,A.DAY10,A.DAY11,A.DAY12,A.DAY13,A.DAY14,A.DAY15,A.DAY16,A.DAY17,A.DAY18,A.DAY19,A.DAY20,A.DAY21,A.DAY22,A.DAY23,A.DAY24,A.DAY25,A.DAY26,A.DAY27,A.DAY28,A.DAY29,A.DAY30,A.DAY31,A.TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum)  AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum ";
                }

                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_so_schedule", "std_so_schedule", dsRep, "SALES SCHD RPT");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            //P.O
            //INV
            case "F1006":
            case "F1006A":
                #region INV
                scode = scode.Replace(";", "");
                opt = fgen.getOption(frm_qstr, frm_cocd, "W0019", "OPT_ENABLE");
                SQuery = "select distinct A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,(a.iamount*round(a.exc_RATE/100,3)) as cgst_val,a.cess_percent as sgst,(a.iamount*round(a.cess_percent/100,3)) as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,t.type1,t1.name,C.tcsamt from ivoucher a,sale c,item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY')='" + scode + "' order by vchdate,a.vchnum,a.MORDER";
                SQuery = "select distinct a.branchcd||a.type||Trim(A.vchnum)||to_Char(A.vchdate,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,A.MORDER, 'N' as logo_yn, a.branchcd,a.cess_pu,a.type,d.cpartno as dpartno,to_char(a.podate,'Mon yyyy') as po_month,a.iexc_addl,trim(a.finvno)||' Dt.'||to_char(a.podate,'dd/mm/yyyy') as po,a.idiamtr as mrp,a.finvno,a.exc_57f4,a.iexc_Addl,A.exc_amt,a.vchnum,a.o_deptt,a.exc_rate,to_char(a.vchdate,'dd/mm/yyyy') as vchdate,a.acode,c.chlnum,'-' as sd_val,to_char(c.chldate,'dd/mm/yyyy') as chldate,b.addr1 as caddr1,b.addr2 as caddr2,b.addr3 as caddr3,b.addr4 as caddr4,b.mobile as ctel,nvl(b.dlno,'-') as dlno,b.person as cperson,b.rc_num2 as cstno, b.rc_num as tinno,b.exc_num as pcstno, c.pono,to_char(c.podate,'dd/mm/yyyy') as podate,c.exc_not_no,c.no_bdls,c.mode_tpt,c.mo_vehi,c.insur_no,c.st_entform,c.ins_cert,c.grno,c.stform_no,c.mcomment,to_char(c.remvdate,'dd/mm/yyyy') as remvdate,c.remvtime,c.bill_qty,c.naration,c.st_type,c.st_rate,c.drv_name,c.drv_mobile,c.freight,c.weight,c.invtime,c.st31_form,c.ins_co,to_char(c.grdate,'dd/mm/yyyy') as grdate,to_char(c.stform_dt,'dd/mm/yyyy') as stform_dt,c.cscode,c.act_tpt_amt,c.ins_no,c.destin,c.pack_rate,c.tptbill_no,c.sta_rate,c.sta_amt,c.totdisc_amt,c.tsubs_amt,c.retention,c.bill_tot,c.amt_sttt,c.amt_stsc,c.amt_sale,c.amt_exc,c.rvalue,c.amt_job,c.st_amt,c.amt_rea,b.aname, a.location,a.srno,a.icode,a.purpose as iname,a.exc_57f4 as cpartno,a.irate,a.revis_no as cdrgno,a.finvno as pordno,a.ponum as ordno,to_char(a.podate,'dd/mm/yyyy') as orddt,a.pname as nsp_flag,a.approxval as bal,a.ichgs as cdisc,a.iamount,a.iqtyout as qty,a.desc_,a.fabtype as strt,a.mode_tpt as stcd,ipack as stk,a.no_bdls as pkg,a.ent_by,to_char(a.ent_dt,'dd/mm/yyyy') as ent_dt,a.btchno,to_char(a.rtn_date,'mm-yy') as expdt,to_char(a.rGPdate,'mm-yy') as mfgdt,d.unit,a.exc_RATE as cgst,(a.iamount*round(a.exc_RATE/100,3)) as cgst_val,a.cess_percent as sgst,(a.iamount*round(a.cess_percent/100,3)) as sgst_val,a.iopr,d.hscode,b.gst_no as cgst_no,b.girno,b.staten,t.type1,t1.name,C.tcsamt from ivoucher a,sale c,item d,type t1,famst b left join type t on trim(b.staten)=trim(t.name) and t.id='{' where trim(a.acode)=trim(b.acode) and trim(a.type)=trim(t1.type1) and t1.id='V' and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DDMMYYYY')= c.BRANCHCD||c.TYPE||TRIM(c.vchnum)||TO_CHAr(c.vchdate,'DDMMYYYY') and trim(A.icode)=trim(d.icode) and a.BRANCHCD||a.TYPE||TRIM(a.vchnum)||TO_CHAr(a.vchdate,'DD/MM/YYYY')='" + scode + "' order by vchdate,a.vchnum,a.MORDER";

                if (iconID == "F1006A") frm_rptName = "std_inv";

                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add(new DataColumn("PkgN", typeof(double)));
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["pkgN"] = fgen.make_double(fgen.getNumericOnly(dr["pkg"].ToString()));
                    }

                    dt.TableName = "Prepcur";

                    //BarCode adding
                    dt = fgen.addBarCode(dt, "fstr", true);

                    repCount = 4;
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));

                    // VIPIN                                        
                    if (frm_cocd == "PPAP")
                    {
                        dt1 = new DataTable("barcode");
                        dt1.Columns.Add(new DataColumn("img1_desc", typeof(string)));
                        dt1.Columns.Add(new DataColumn("img1", typeof(System.Byte[])));
                        string col2 = "";
                        mq10 = fgen.seek_iname(frm_qstr, frm_cocd, "select gst_no from type where id='B' and type1='" + dt.Rows[0]["branchcd"].ToString().Trim().Replace("/", "") + "'", "gst_no");
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            col1 = dt.Rows[i]["branchcd"].ToString().Trim().Replace("/", "") + "," + dt.Rows[i]["vchnum"].ToString().Trim().Replace("/", "");

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
                        }
                        fpath = Server.MapPath(@"~\tej-base\BarCode\" + col1.Trim().Replace("*", "").Replace("/", "") + ".png");
                        del_file(fpath);
                        if (frm_cocd == "PPAP") fgen.prnt_QRbar(frm_cocd, col2, col1.Replace("*", "").Replace("/", "") + ".png");
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
                    SQuery = "Select distinct d.aname as consign,d.addr1 as daddr1,d.addr2 as daddr2,d.addr3 as daddr3,d.addr4 as daddr4,d.telnum as dtel, d.rc_num as dtinno,d.exc_num as dcstno,d.acode as mycode,d.staten as dstaten,d.gst_no as dgst_no,d.girno as dpanno,substr(d.gst_no,0,2) as dstatecode from csmst d where trim(d.acode)= '" + dt.Rows[0]["cscode"].ToString().Trim() + "'";
                    dt = new DataTable();
                    dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    if (dt.Rows.Count <= 0)
                    {
                        dt = new DataTable();
                        SQuery = "Select 'Same as Recipient' as consign,'-' as daddr1,'-' as daddr2,'-' as daddr3,'-' as daddr4,'-' as dtel, '-' as dtinno,'-' as dcstno,'-' as mycode,'-' as dstaten,'-' as dgst_no,'-' as dpanno,'-' as dstatecode from dual";
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
                    mdr = null;
                    dt1.Columns.Add("poterms", typeof(string));
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        mq10 += dt.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
                    }
                    mdr = dt1.NewRow();
                    mdr["poterms"] = mq10;
                    dt1.Rows.Add(mdr);
                    dt1.TableName = "INV_TERMS";
                    dsRep.Tables.Add(dt1);

                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_inv", frm_rptName, dsRep, "Invoice Entry Report", "Y");
                }
                else
                {
                    data_found = "N";
                }
                //printDefault(frm_cocd, frm_mbr, "std_invcl", "std_invcl", dsRep, "Invoice Challan");
                #endregion
                break;

            case "F49101":
            case "F49106":
            case "F1005":
                string so_title;
                string so_tbl;
                so_title = "Sales Order";
                so_tbl = "SOMAS";
                if (iconID == "F49101" || iconID == "F49101")
                {
                    so_title = "Master Sales Order";
                    so_tbl = "SOMASM";
                }
                if (frm_rptName == "0")
                {
                    frm_rptName = "STD_SO";
                }
                #region S.O.
                opt = fgen.getOption(frm_qstr, frm_cocd, "W0018", "OPT_ENABLE");
                SQuery = "Select a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'" + so_tbl + "' as TAB_NAME,'" + so_title + "' as so_title,'SO Number' as h1,'SO Dated' as h2,G.ANAME AS CONSNAME,G.ADDR1 AS COS_ADR1,G.ADDR2 AS CONS_aDR2,G.ADDR3 AS CONS_aDR3,G.TELNUM AS CONS_TEL,G.GIRNO AS CONS_PAN,SUBSTR(G.GST_NO,0,2) AS CONS_CODE,G.EMAIL AS CSMAIL,G.TYPE AS CONS_TYPE,G.STATEN AS CONS_STATE, G.GST_NO AS CONS_GST,'" + so_tbl + "' as TAB_NAME, 'Order NO' as h1,'Order Dt' as h2, c.cpartno AS IPART, B.ADDR1,B.ADDR2,B.ADDR3,/*substr(b.gst_no,0,2)*/ B.STAFFCD as statecode,b.staten,b.gst_no,b.girno as pan1,C.UNIT AS ITEM_UNIT,B.ANAME,C.ICODE AS ITEM_CODE,C.INAME AS ITEM_NAME,c.hscode, t.name as So_Type,A.* from " + so_tbl + " a LEFT OUTER JOIN CSMST G ON TRIM(A.CSCODE)=TRIM(G.ACODE),famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(t.type1) and t.id='V' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in (" + barCode + ") order by a.orddt,a.ordno,a.srno";
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    //BarCode adding
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_so", frm_rptName, dsRep, "S.O. Entry Report", "Y");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F49141":
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                mq10 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_VTY");
                mq0 = "select trim(to_char(to_date('" + fromdt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + fromdt.Substring(6, 4) + "','yyyy'),'yyyy')) as FRMDATE  from dual";
                mq1 = fgen.seek_iname(frm_qstr, frm_cocd, mq0, "FRMDATE");
                mq2 = "select trim( to_char(to_date('" + todt.Substring(0, 5) + "','dd/mm'),'dd Month'))||' '||trim(to_char(to_date('" + todt.Substring(6, 4) + "','yyyy'),'yyyy')) as TODATE  from dual";
                mq3 = fgen.seek_iname(frm_qstr, frm_cocd, mq2, "TODATE");
                SQuery = "SELECT DISTINCT  '" + mq1 + "' as FRMDATE,'" + mq3 + "' AS TODATE,'" + fromdt + "' AS FRMDATE1,'" + todt + "' AS TODATE1,D.ANAME AS DNAME,B.INAME,C.ANAME,C.ADDR1 AS ADRES1,C.ADDR2 AS CADDRES,C.ADDR3 AS CADRES3,D.ADDR1 AS DADRES1,D.ADDR2 AS DADRES2,D.ADDR3 AS DADRES3,TO_CHAR(A.ORDDT,'YYYYMMDD')||TRIM(A.ORDNO)||TRIM(A.TYPE) AS GRP,  A.* FROM SOMAS A  LEFT OUTER JOIN CSMST D ON TRIM(A.CSCODE)=TRIM(D.ACODE), ITEM B,FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE)=TRIM(C.ACODE) AND A.BRANCHCD='" + frm_mbr + "'  AND A.TYPE in (" + mq10 + ") AND A.ORDDT " + xprdRange + " and a.acode like '" + party_cd + "%' and a.icode like '" + part_cd + "%' ORDER BY a.orddt,a.type,a.ordno,A.SRNO";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_SO_REG", "std_SO_REG", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F49142":
                header_n = "Pending S.O. Register";
                #region
                // type variable not in query //SQuery = "SELECT TO_CHAR(A.PORDDT,'DD/MM/YYYY') AS PODT,'" + fromdt + "' as frmdt,'" + todt + "' as todt, A.* FROM wbvu_pend_so A where A.BRANCHCD='" + mbr + "' AND A.TYPE='" + hfcode.Value + "' AND A.ORDDT " + xprdrange + "  ORDER BY A.ACODE,A.ICODE";                                                 
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                // GIVING ERROR TABLE OR VIEW DOES NOT EXIST
                // SQuery = "SELECT TO_CHAR(A.PORDDT,'DD/MM/YYYY') AS PODT,'" + fromdt + "' as frmdt,'" + todt + "' as todt, A.* FROM wbvu_pend_so A where A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in (" + frm_vty + ") AND A.ORDDT " + xprdRange + "  ORDER BY A.ACODE,A.ICODE";
                // ADD HEADER IN THE QUERY
                SQuery = "SELECT '" + header_n + "' AS HEADER, TO_CHAR(A.PORDDT,'DD/MM/YYYY') AS PODT,'" + fromdt + "' as frmdt,'" + todt + "' as todt,round(a.srate*a.bal_Qty,2) as Bal_Val,A.* FROM wbvu_pending_so A where A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in (" + frm_vty + ") AND A.ORDDT " + xprdRange + "  ORDER BY A.ACODE,A.ICODE";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Pend_Order_Register", "std_Pend_Order_Register", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;
                #endregion

            ////BY YOGITA 5/5/2018
            case "F49222":
                #region Order Vs Dispatch 12 Month
                header_n = "Order Vs Dispatch 12 Month";
                dsRep = new DataSet();
                cond = frm_ulvl == "M" ? "and trim(a.acode)='" + frm_uname + "'" : "";
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.acode,b.aname as party,a.icode,c.iname,c.cpartno,c.unit,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as stot,sum(a.djan+a.dfeb+a.dmar+a.dapr+a.dmay+a.djun+a.djul+a.daug+a.dsep+a.doct+a.dnov+a.ddec+a.djan+a.dfeb+a.dmar) as dtot,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug ,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.dapr) as dapr,sum(a.dmay) as dmay,sum(a.djun) as djun,sum(a.djul) as djul,sum(a.daug) as daug,sum(a.dsep) as dsep,sum(a.doct) as doct,sum(a.dnov) as dnov,sum(a.ddec) as ddec,sum(a.djan) as djan,sum(a.dfeb) as dfeb,sum(a.dmar) as dmar from (select acode,icode,(case when to_char(ORDDT,'mm')='04' then QTYORD else 0 end) as apr,(case when to_char(ORDDT,'mm')='05' then QTYORD else 0 end) as may,(case when to_char(ORDDT,'mm')='06' then QTYORD else 0 end) as jun,(case when to_char(ORDDT,'mm')='07' then QTYORD else 0 end) as jul,(case when to_char(ORDDT,'mm')='08' then QTYORD else 0 end) as aug,(case when to_char(ORDDT,'mm')='09' then QTYORD else 0 end) as sep,(case when to_char(ORDDT,'mm')='10' then QTYORD else 0 end) as oct,(case when to_char(ORDDT,'mm')='11' then QTYORD else 0 end) as nov,(case when to_char(ORDDT,'mm')='12' then QTYORD else 0 end) as dec,(case when to_char(ORDDT,'mm')='01' then QTYORD else 0 end) as jan,(case when to_char(ORDDT,'mm')='02' then QTYORD else 0 end) as feb,(case when to_char(ORDDT,'mm')='03' then QTYORD else 0 end) as mar ,0 as dapr,0 as dmay,0 as djun,0 as djul,0 as daug,0 as dsep,0 as doct,0 as dnov,0 as ddec,0 as djan,0 as dfeb,0 as dmar  from sOMAS where branchcd='" + frm_mbr + "' and type LIKE '4%' and ORDDT " + xprdRange + " union all select acode ,icode,0 as apr,0 as may,0 as jun,0 as jul,0 as aug,0 as sep,0 as oct,0 as nov,0 as dec,0 as jan,0 as feb,0 as mar,(Case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as Dapr,(Case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as Dmay,(Case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as Djun,(Case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as Djul,(Case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as Daug,(Case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as Dsep,(Case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as Doct,(Case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as Dnov,(Case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as Ddec,(Case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as Djan,(Case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as Dfeb,(Case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as Dmar from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and vchdate  " + xprdRange + " ) a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) " + cond + " group by a.acode,b.aname,a.icode,c.iname,c.cpartno,c.unit";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Ord_vs_desp_mth", "std_Ord_vs_desp_mth", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F49223":
                #region Schedule Vs Dispatch 12 Month
                header_n = "Schedule Vs Dispatch 12 Month";
                dsRep = new DataSet();
                cond = frm_ulvl == "M" ? "and trim(a.acode)='" + frm_uname + "'" : "";
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.acode,b.aname as party,a.icode,c.iname,c.cpartno,c.unit,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as stot,sum(a.djan+a.dfeb+a.dmar+a.dapr+a.dmay+a.djun+a.djul+a.daug+a.dsep+a.doct+a.dnov+a.ddec+a.djan+a.dfeb+a.dmar) as dtot,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug ,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar,sum(a.dapr) as dapr,sum(a.dmay) as dmay,sum(a.djun) as djun,sum(a.djul) as djul,sum(a.daug) as daug,sum(a.dsep) as dsep,sum(a.doct) as doct,sum(a.dnov) as dnov,sum(a.ddec) as ddec,sum(a.djan) as djan,sum(a.dfeb) as dfeb,sum(a.dmar) as dmar from (select acode,icode,(case when to_char(vchdate,'mm')='04' then total else 0 end) as apr,(case when to_char(vchdate,'mm')='05' then total else 0 end) as may,(case when to_char(vchdate,'mm')='06' then total else 0 end) as jun,(case when to_char(vchdate,'mm')='07' then total else 0 end) as jul,(case when to_char(vchdate,'mm')='08' then total else 0 end) as aug,(case when to_char(vchdate,'mm')='09' then total else 0 end) as sep,(case when to_char(vchdate,'mm')='10' then total else 0 end) as oct,(case when to_char(vchdate,'mm')='11' then total else 0 end) as nov,(case when to_char(vchdate,'mm')='12' then total else 0 end) as dec,(case when to_char(vchdate,'mm')='01' then total else 0 end) as jan,(case when to_char(vchdate,'mm')='02' then total else 0 end) as feb,(case when to_char(vchdate,'mm')='03' then total else 0 end) as mar ,0 as dapr,0 as dmay,0 as djun,0 as djul,0 as daug,0 as dsep,0 as doct,0 as dnov,0 as ddec,0 as djan,0 as dfeb,0 as dmar  from schedule where branchcd='" + frm_mbr + "' and type='46' and vchdate " + xprdRange + " union all select acode ,icode,0 as apr,0 as may,0 as jun,0 as jul,0 as aug,0 as sep,0 as oct,0 as nov,0 as dec,0 as jan,0 as feb,0 as mar,(Case when to_char(vchdate,'mm')='04' then iqtyout else 0 end) as Dapr,(Case when to_char(vchdate,'mm')='05' then iqtyout else 0 end) as Dmay,(Case when to_char(vchdate,'mm')='06' then iqtyout else 0 end) as Djun,(Case when to_char(vchdate,'mm')='07' then iqtyout else 0 end) as Djul,(Case when to_char(vchdate,'mm')='08' then iqtyout else 0 end) as Daug,(Case when to_char(vchdate,'mm')='09' then iqtyout else 0 end) as Dsep,(Case when to_char(vchdate,'mm')='10' then iqtyout else 0 end) as Doct,(Case when to_char(vchdate,'mm')='11' then iqtyout else 0 end) as Dnov,(Case when to_char(vchdate,'mm')='12' then iqtyout else 0 end) as Ddec,(Case when to_char(vchdate,'mm')='01' then iqtyout else 0 end) as Djan,(Case when to_char(vchdate,'mm')='02' then iqtyout else 0 end) as Dfeb,(Case when to_char(vchdate,'mm')='03' then iqtyout else 0 end) as Dmar from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and vchdate " + xprdRange + " ) a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) " + cond + " group by a.acode,b.aname,a.icode,c.iname,c.cpartno,c.unit";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_Sch_vs_desp_mth", "std_Sch_vs_desp_mth", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

                break;

            case "F49224":
                header_n = "Schedule Status Daily";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                SQuery = "SELECT '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header, A.Acode,b.aname,A.icode,c.iname,c.cpartno,c.unit,SUM(A.TOTAL) AS TOTAL,SUM(A.DAY1) AS Day_01,sum(a.DAY2) as day_02,sum(a.day3) as day_03,sum(a.day4) as day_04,sum(a.day5) as day_05,sum(a.day6) as day_06,sum(a.day7) as day_07,sum(a.day8) as day_08,sum(a.day9) as day_09,sum(a.day10) as day_10,sum(a.day11) as day_11,sum(a.day12) as day_12,sum(a.day13) as day_13,sum(a.day14) as day_14,sum(a.day15) as day_15,sum(a.day16) as day_16,sum(a.day17) as day_17,sum(a.day18) as day_18,sum(a.day19) as day_19,sum(a.day20) as day_20,sum(a.day21) as day_21,sum(a.day22) as day_22,sum(a.day23) as day_23,sum(a.day24) as day_24,sum(a.day25) as day_25,sum(a.day26) as day_26,sum(a.day27) as day_27,sum(a.day28) as day_28,sum(a.day29) as day_29,sum(a.day30) as day_30,sum(a.day31) as day_31 FROM SCHEDULE a,famst b ,ITEM C WHERE trim(a.acode)=trim(b.acode) AND TRIM(A.ICODE)=TRIM(C.ICODE) and a.BRANCHCD='" + frm_mbr + "' AND a.TYPE='46' and a.vchdate  " + xprdRange + "  " + cond + " group by a.acode,a.icode,b.aname,c.iname,c.cpartno,c.unit";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "sch_status_daily", "sch_status_daily", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F49225":
                header_n = "Schedule Status Monthly";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");

                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,a.acode,b.aname as party,a.icode,c.iname,c.cpartno,c.unit,sum(a.apr+a.may+a.jun+a.jul+a.aug+a.sep+a.oct+a.nov+a.dec+a.jan+a.feb+a.mar) as stot,sum(a.apr) as apr,sum(a.may) as may,sum(a.jun) as jun,sum(a.jul) as jul,sum(a.aug) as aug ,sum(a.sep) as sep,sum(a.oct) as oct,sum(a.nov) as nov,sum(a.dec) as dec,sum(a.jan) as jan,sum(a.feb) as feb,sum(a.mar) as mar from (select acode,icode,(case when to_char(vchdate,'mm')='04' then total else 0 end) as apr,(case when to_char(vchdate,'mm')='05' then total else 0 end) as may,(case when to_char(vchdate,'mm')='06' then total else 0 end) as jun,(case when to_char(vchdate,'mm')='07' then total else 0 end) as jul,(case when to_char(vchdate,'mm')='08' then total else 0 end) as aug,(case when to_char(vchdate,'mm')='09' then total else 0 end) as sep,(case when to_char(vchdate,'mm')='10' then total else 0 end) as oct,(case when to_char(vchdate,'mm')='11' then total else 0 end) as nov,(case when to_char(vchdate,'mm')='12' then total else 0 end) as dec,(case when to_char(vchdate,'mm')='01' then total else 0 end) as jan,(case when to_char(vchdate,'mm')='02' then total else 0 end) as feb,(case when to_char(vchdate,'mm')='03' then total else 0 end) as mar from schedule where branchcd='" + frm_mbr + "' and type='46' and vchdate " + xprdRange + " ) a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) " + cond + " group by a.acode,b.aname,a.icode,c.iname,c.cpartno,c.unit";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "sch_status_mthly", "sch_status_mthly", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F49226":
                //party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                header_n = "Rate Trend Chart Product Wise";
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,trim(a.icode) as icode,trim(i.iname) as iname,i.cpartno,i.unit,sum(a.apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar from (select icode,(case when to_char(vchdate,'mm')='04'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as apr,(case when to_char(vchdate,'mm')='05'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as may,(case when to_char(vchdate,'mm')='06'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as jun,(case when to_char(vchdate,'mm')='07'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as jul,(case when to_char(vchdate,'mm')='08'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as aug,(case when to_char(vchdate,'mm')='09'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as sep,(case when to_char(vchdate,'mm')='10'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as oct,(case when to_char(vchdate,'mm')='11'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as nov,(case when to_char(vchdate,'mm')='12'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as dec,(case when to_char(vchdate,'mm')='01'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as jan,(case when to_char(vchdate,'mm')='02'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as feb,(case when to_char(vchdate,'mm')='03'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as mar from ivoucher where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " and nvl(trim(iqtyout),0)!=0 group by icode,to_char(vchdate,'mm'))a,item i where trim(a.icode)=trim(i.icode) AND A.ICODE LIKE '" + party_cd + "%' group by trim(a.icode),trim(i.iname),i.cpartno,i.unit order by iname,icode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Rate_Trnd_Chrt_Prod", "Rate_Trnd_Chrt_Prod", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F49227":
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                header_n = "Rate Trend Chart Customer Wise";
                SQuery = "select '" + fromdt + "' as frmdt,'" + todt + "' as todt,'" + header_n + "' as header,trim(a.acode) as acode,trim(f.aname) as aname,sum(a.apr) as apr,sum(may) as may,sum(jun) as jun,sum(jul) as jul,sum(aug) as aug,sum(sep) as sep,sum(oct) as oct,sum(nov) as nov,sum(dec) as dec,sum(jan) as jan,sum(feb) as feb,sum(mar) as mar from (select acode,(case when to_char(vchdate,'mm')='04'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as apr,(case when to_char(vchdate,'mm')='05'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as may,(case when to_char(vchdate,'mm')='06'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as jun,(case when to_char(vchdate,'mm')='07'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as jul,(case when to_char(vchdate,'mm')='08'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as aug,(case when to_char(vchdate,'mm')='09'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as sep,(case when to_char(vchdate,'mm')='10'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as oct,(case when to_char(vchdate,'mm')='11'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as nov,(case when to_char(vchdate,'mm')='12'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as dec,(case when to_char(vchdate,'mm')='01'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as jan,(case when to_char(vchdate,'mm')='02'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as feb,(case when to_char(vchdate,'mm')='03'  then  round(sum(iamount)/sum(iqtyout),2) else 0 end) as mar from ivoucher a where branchcd='" + frm_mbr + "' and type like '4%' and type!='47' and vchdate " + xprdRange + " group by acode,to_char(vchdate,'mm'))a,famst f where trim(a.acode)=trim(f.acode) AND A.ACODE LIKE '" + party_cd + "%' group by trim(a.acode),trim(f.aname) order by aname,acode";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Rate_Trnd_Chrt_Cust", "Rate_Trnd_Chrt_Cust", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F49228":
            case "F70256": //FOR ACC REP
                header_n = "Bill Wise Month Wise Sale Report";
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, B.INAME,C.ANAME,C.MKTGGRP,b.cpartno,A.BRANCHCD,A.TYPE,A.VCHNUM,TO_CHAR(A.VCHDATE,'MONTH') AS VCHDATE,to_char(a.vchdate,'dd/mm/yyyy') as vhdate ,A.ACODE,A.ICODE,A.IQTYOUT, A.IAMOUNT,A.IRATE,trim(substr(A.FINVNO,1,13)) AS PONO FROM IVOUCHER A,ITEM B ,FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE) = TRIM(C.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '4%' AND A.VCHDATE " + xprdRange + " ORDER BY C.ANAME,A.VCHDATE ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Bill_Wise_Salerep", "Bill_Wise_Salerep", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F49210":
                #region
                dt2 = new DataTable(); dt = new DataTable();
                dt2.Columns.Add("header", typeof(string));
                dt2.Columns.Add("fromdt", typeof(string));
                dt2.Columns.Add("todt", typeof(string));
                dt2.Columns.Add("acode", typeof(string));
                dt2.Columns.Add("customer", typeof(string));
                dt2.Columns.Add("pono", typeof(string));
                dt2.Columns.Add("podt", typeof(string));
                dt2.Columns.Add("part", typeof(string));
                dt2.Columns.Add("due_date", typeof(string));
                dt2.Columns.Add("lt", typeof(string));
                dt2.Columns.Add("ship_dt_plnt", typeof(string));
                dt2.Columns.Add("sage_delv_dt", typeof(string));
                dt2.Columns.Add("unit", typeof(string));
                dt2.Columns.Add("Remarks", typeof(string));

                header_n = "Estimated Delivery Schedule";

                //somas me 00 krna hai ya mbr pas krna hai..need to ask

                SQuery = "SELECT trim(a.acode) as acode,trim(b.aname) as customer,trim(a.icode) as icode,A.DESC_,c.cpartno as part,c.unit,trim(a.pordno) as po,to_char(a.porddt,'dd/mm/yyyy') as podt,nvl(a.qtyord,0) as qtyord,to_char(a.cu_chldt,'dd/mm/yyyy') as due_date,a.irate FROM  somas a,famst b,item c  where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd='" + frm_mbr + "' and a.type like '4%' and a.orddt " + xprdRange + "";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dr1 = dt2.NewRow();
                        dr1["header"] = header_n;
                        dr1["fromdt"] = fromdt;
                        dr1["todt"] = todt;
                        dr1["acode"] = dt.Rows[i]["acode"].ToString().Trim();
                        dr1["customer"] = dt.Rows[i]["customer"].ToString().Trim();
                        dr1["pono"] = dt.Rows[i]["po"].ToString().Trim();
                        dr1["podt"] = dt.Rows[i]["podt"].ToString().Trim();
                        dr1["part"] = dt.Rows[i]["part"].ToString().Trim();
                        dr1["due_date"] = dt.Rows[i]["due_date"].ToString().Trim();
                        dr1["lt"] = "";
                        dr1["ship_dt_plnt"] = "";
                        dr1["sage_delv_dt"] = "";
                        dr1["unit"] = dt.Rows[i]["unit"].ToString().Trim();
                        dr1["Remarks"] = dt.Rows[i]["DESC_"].ToString().Trim();
                        dt2.Rows.Add(dr1);
                    }
                }
                if (dt2.Rows.Count > 0)
                {
                    dt2.TableName = "prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt2, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "SAGE_9", "SAGE_9", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F49229":
                break;

            case "F49230":
            case "F70257": //MONTH WISE SALES SUMMARY....MADE BY Akshay
                header_n = "Cust,Item,Bill Wise Sale Report";
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, c.acode,c.aname,b.iname,trim(b.cpartno) as partno,A.BRANCHCD,A.TYPE,count(a.vchnum) as vch,TO_CHAR(A.VCHDATE,'MONTH') AS VCHDATE,TO_CHAR(A.VCHDATE,'yyyyMM') AS DAT ,sum(A.IQTYOUT) AS QTY , sum(A.IAMOUNT) AS BASIC FROM IVOUCHER A,ITEM B ,FAMST C WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE) = TRIM(C.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '4%' AND A.VCHDATE " + xprdRange + "  group by A.BRANCHCD,A.TYPE,TO_CHAR(A.VCHDATE,'MONTH') ,c.aname,b.iname,c.acode,b.cpartno,TO_CHAR(A.VCHDATE,'yyyyMM')  ORDER BY ANAME,INAME ASC,DAT";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Month_Item_Wise_Sale", "Month_Item_Wise_Sale", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F49231":
            // case "F70277": ///THIS REPORT ALSO CREATED VIEW PAGE
            case "F70258":
                header_n = "Customer Wise Sale Report";
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, C.ANAME,C.MKTGGRP ,sum(A.IQTYOUT) as qty, sum(A.IAMOUNT) as basic FROM IVOUCHER A ,FAMST C WHERE TRIM(A.ACODE) = TRIM(C.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '4%' AND A.VCHDATE " + xprdRange + " group by c.aname ,C.MKTGGRP ORDER BY C.ANAME ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Customer_Wise_Sale", "Customer_Wise_Sale", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F49232":
                header_n = "Cust part Wise Sale Report";
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt, C.MKTGGRP ,B.INAME,C.ANAME,B.UNIT,B.CPARTNO ,SUM(A.IQTYOUT) AS QTY ,SUM(A.IAMOUNT) AS BASIC FROM IVOUCHER A, ITEM B,FAMST C  WHERE TRIM(A.ICODE)=TRIM(B.ICODE) AND TRIM(A.ACODE) = TRIM(C.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND TYPE LIKE '4%' AND A.VCHDATE " + xprdRange + " GROUP BY B.INAME,C.ANAME,B.UNIT,B.CPARTNO,C.MKTGGRP  ORDER BY C.ANAME ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Cust_Part_wise", "Cust_Part_wise", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                break;

            case "F49206": // order Acceptance
                header_n = "Order Acceptance";
                mq1 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                SQuery = "select '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,'" + header_n + "' as header, a.branchcd,a.type,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.acode,trim(b.aname) as aname,b.addr1,b.addr2,b.addr3,b.staten,b.country,b.gst_no,b.girno,trim(a.icode) as icode,trim(c.iname) as iname,trim(a.cpartno) as partno,a.ciname,a.pordno,to_char(a.porddt,'dd/mm/yyyy') as porddt,a.thru,a.qtyord,a.irate,a.qtyord * a.irate  as basic,to_char(a.cu_chldt,'dd/mm/yyyy') as cu_chldt,A.GMT_SHADE,A.REMARK,A.INSPBY,a.currency,a.curr_rate,to_char(a.promdt,'dd/mm/yyyy') as promdt,c.unit from somas a , famst b , item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd||a.type||a.ordno||to_char(a.orddt,'dd/mm/yyyy')='" + mq1 + "' order by ordno";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Ord_Acceptance", "Ord_Acceptance", dsRep, header_n);
                }
                break;

            case "F49209":    //SET TYPE=4F    
                #region
                //  mq1 = "4F";  
                // mq1="SUBSTR(TYPE,1,1)=4";
                dt2 = new DataTable(); dt = new DataTable(); dt1 = new DataTable(); dt4 = new DataTable();
                dt2.Columns.Add("header", typeof(string));
                dt2.Columns.Add("fromdt", typeof(string));
                dt2.Columns.Add("todt", typeof(string));
                dt2.Columns.Add("acode", typeof(string));
                dt2.Columns.Add("customer", typeof(string));
                //  dt2.Columns.Add("icode", typeof(string));
                //  dt2.Columns.Add("item", typeof(string));
                dt2.Columns.Add("unit", typeof(string));
                dt2.Columns.Add("opbal", typeof(double));
                dt2.Columns.Add("order_rcv", typeof(double));
                dt2.Columns.Add("order_shipped", typeof(double));
                dt2.Columns.Add("closbal", typeof(double));
                dt2.Columns.Add("BWL", typeof(double)); //08 BRNCHCD
                dt2.Columns.Add("SBD", typeof(double)); //01
                dt2.Columns.Add("FBD", typeof(double)); //04

                dt3 = fgen.getdata(frm_qstr, frm_cocd, "select TYPE1,NAME  from type where id='B'");
                //for (int j = 0; j < dt3.Rows.Count;j++ ) //FOR DYNAMIC COLUMN...........
                //{
                //    dt2.Columns.Add("" + dt3.Rows[j]["name"].ToString().Trim() + "", typeof(string));
                //}

                mq2 = "";
                mq2 = fgen.seek_iname(frm_qstr, frm_cocd, "SELECT ID,PARAMS  FROM  CONTROLS WHERE ID='R01'", "PARAMS");  //date from control as per financial start

                xprdRange1 = "between to_date('" + mq2 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";
                header_n = "Export New Order Rec/Shipment Dispatched";
                // SQuery = "select a.acode as acode,trim(b.aname) as cust,a.icode,trim(c.iname) as item,c.unit,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,null AS ICODE,trim(acode) as acode,0 as opening,0 as cdr,0 as ccr from famstbal where branchcd='" + frm_mbr + "' and 1=2  union all select branchcd,TRIM(ICODE) AS ICODE,trim(acode) as acode,sum(nvl(qtyord,0))  as op,0 as cdr,0 as ccr FROM somas where branchcd='" + frm_mbr + "' and TYPE LIKE '4%' AND orddt " + xprdRange1 + "  GROUP BY trim(acode),branchcd,trim(icode)  union all  select branchcd,TRIM(ICODE) AS ICODE,trim(acode) as acode,-1*sum(nvl(iqtyout,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '4%' AND VCHDATE " + xprdRange1 + "   and store='Y' GROUP BY trim(acode),branchcd,trim(icode) union all select branchcd,TRIM(ICODE) AS ICODE,trim(acode) as acode,0 as op,sum(nvl(qtyord,0)) as cdr,0 as ccr from somas where branchcd='" + frm_mbr + "' and TYPE LIKE '4%'  AND orddt " + xprdRange + " GROUP BY trim(acode) ,branchcd,trim(icode) union all select branchcd,TRIM(ICODE) AS ICODE,trim(acode) as acode,0 as op,0 as cdr,sum(nvl(iqtyout,0)) as ccr from IVOUCHER where branchcd='" + frm_mbr + "' and TYPE LIKE '4%'  AND VCHDATE " + xprdRange + " and store='Y' GROUP BY trim(acode),branchcd,trim(icode)) a,famst b,item c where  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and substr(trim(a.acode),1,2) in ('16','02') GROUP BY A.aCODE,a.icode,trim(b.aname),trim(c.iname),c.unit";
                SQuery = "select a.acode as acode,trim(b.aname) as cust,a.icode,trim(c.iname) as item,c.unit,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,null AS ICODE,trim(acode) as acode,0 as opening,0 as cdr,0 as ccr from famstbal where branchcd!='DD' and 1=2  union all select (case when length(trim(weight))=2 then weight else branchcd end ) as branchcd,TRIM(ICODE) AS ICODE,trim(acode) as acode,sum(nvl(qtyord,0)*nvl(irate,0)) as op,0 as cdr,0 as ccr FROM somas where branchcd='00' and TYPE LIKE '4%' AND orddt " + xprdRange1 + "  GROUP BY trim(acode),(case when length(trim(weight))=2 then weight else branchcd end ),trim(icode)  union all  select branchcd,TRIM(ICODE) AS ICODE,trim(acode) as acode,-1*sum(nvl(iqtyout,0)*nvl(iqty_chlwt,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd!='DD' and TYPE LIKE '4%' AND VCHDATE " + xprdRange1 + " and store='Y' GROUP BY trim(acode),branchcd,trim(icode) union all select (case when length(trim(weight))=2 then weight else branchcd end ) as branchcd,TRIM(ICODE) AS ICODE,trim(acode) as acode,0 as op,sum(nvl(qtyord,0)*nvl(irate,0)) as cdr,0 as ccr from somas where branchcd='00' and TYPE LIKE '4%'  AND orddt " + xprdRange + " GROUP BY trim(acode) ,(case when length(trim(weight))=2 then weight else branchcd end ),trim(icode) union all select branchcd,TRIM(ICODE) AS ICODE,trim(acode) as acode,0 as op,0 as cdr,sum(nvl(iqtyout,0)*nvl(iqty_chlwt,0)) as ccr from IVOUCHER where branchcd!='DD' and TYPE LIKE '4%'  AND VCHDATE " + xprdRange + " and store='Y' GROUP BY trim(acode),branchcd,trim(icode)) a,famst b,item c where  trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and substr(trim(a.acode),1,2) in ('16','02') GROUP BY A.aCODE,a.icode,trim(b.aname),trim(c.iname),c.unit ORDER BY A.acode";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                SQuery = "select a.branchcd,a.acode as acode,trim(b.aname) as cust,sum(a.opening) as opening,sum(a.cdr) as Rcpt,sum(a.ccr) as Issued,Sum((a.opening+a.cdr)-a.ccr) as Closing_Stk from (Select branchcd,null AS ICODE,trim(acode) as acode,0 as opening,0 as cdr,0 as ccr from famstbal where branchcd!='DD' and 1=2  union all select (case when length(trim(weight))=2 then weight else branchcd end ) as branchcd,TRIM(ICODE) AS ICODE,trim(acode) as acode,sum(nvl(qtyord,0)*nvl(irate,0)) as op,0 as cdr,0 as ccr FROM somas where branchcd='00' and TYPE LIKE '4%' AND orddt " + xprdRange1 + "  GROUP BY trim(acode),(case when length(trim(weight))=2 then weight else branchcd end ),trim(icode)  union all  select branchcd,TRIM(ICODE) AS ICODE,trim(acode) as acode,-1*sum(nvl(iqtyout,0)*nvl(iqty_chlwt,0)) as op,0 as cdr,0 as ccr FROM IVOUCHER where branchcd!='DD' and TYPE LIKE '4%' AND VCHDATE " + xprdRange1 + " and store='Y' GROUP BY trim(acode),branchcd,trim(icode) union all select (case when length(trim(weight))=2 then weight else branchcd end ) as branchcd,TRIM(ICODE) AS ICODE,trim(acode) as acode,0 as op,sum(nvl(qtyord,0)*nvl(irate,0)) as cdr,0 as ccr from somas where branchcd='00' and TYPE LIKE '4%'  AND orddt " + xprdRange + " GROUP BY trim(acode) ,(case when length(trim(weight))=2 then weight else branchcd end ),trim(icode) union all select branchcd,TRIM(ICODE) AS ICODE,trim(acode) as acode,0 as op,0 as cdr,sum(nvl(iqtyout,0)*nvl(iqty_chlwt,0)) as ccr from IVOUCHER where branchcd!='DD' and TYPE LIKE '4%'  AND VCHDATE " + xprdRange + " and store='Y' GROUP BY trim(acode),branchcd,trim(icode)) a,famst b where  trim(a.acode)=trim(b.acode) and substr(trim(a.acode),1,2) in ('16','02') GROUP BY A.aCODE,trim(b.aname),a.branchcd ORDER BY A.BRANCHCD,A.ACODE";
                dt4 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                //===================================================
                if (dt.Rows.Count > 0)
                {
                    DataView view1im = new DataView(dt);
                    DataTable dtdrsim = new DataTable();
                    dtdrsim = view1im.ToTable(true, "ACODE"); //MAIN                  
                    foreach (DataRow dr0 in dtdrsim.Rows)
                    {
                        DataView viewim = new DataView(dt, "ACODE='" + dr0["ACODE"] + "'", "", DataViewRowState.CurrentRows);
                        dr1 = dt2.NewRow();
                        dt1 = new DataTable();
                        dt1 = viewim.ToTable();
                        db = 0; db1 = 0; db2 = 0; db3 = 0; db4 = 0; db5 = 0; db6 = 0; mq3 = "";
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            #region
                            dr1["header"] = header_n;
                            dr1["fromdt"] = fromdt;
                            dr1["todt"] = todt;
                            dr1["acode"] = dt1.Rows[i]["acode"].ToString().Trim();
                            dr1["customer"] = dt1.Rows[i]["cust"].ToString().Trim();
                            //   dr1["icode"] = dt1.Rows[i]["icode"].ToString().Trim();
                            // dr1["item"] = dt1.Rows[i]["item"].ToString().Trim();                                                       
                            db += fgen.make_double(dt1.Rows[i]["opening"].ToString().Trim());
                            dr1["opbal"] = db;
                            db1 += fgen.make_double(dt1.Rows[i]["Rcpt"].ToString().Trim());
                            dr1["order_rcv"] = db1;
                            db2 += fgen.make_double(dt1.Rows[i]["Issued"].ToString().Trim());
                            dr1["order_shipped"] = db2;
                            db3 += fgen.make_double(dt1.Rows[i]["Closing_Stk"].ToString().Trim());
                            dr1["closbal"] = db3;
                            //////////////for branchwise field
                            db4 += fgen.make_double(fgen.seek_iname_dt(dt4, "acode='" + dr1["acode"].ToString().Trim() + "' and branchcd='01'", "Closing_Stk"));
                            dr1["SBD"] = db4;
                            db5 += fgen.make_double(fgen.seek_iname_dt(dt4, "acode='" + dr1["acode"].ToString().Trim() + "' and branchcd='04'", "Closing_Stk"));
                            dr1["FBD"] = db5;
                            db6 += fgen.make_double(fgen.seek_iname_dt(dt4, "acode='" + dr1["acode"].ToString().Trim() + "' and branchcd='08'", "Closing_Stk"));
                            dr1["BWL"] = db6;
                            #endregion
                        }
                        if (db4 != 0)
                        {
                            mq3 = fgen.seek_iname_dt(dt3, "type1='01'", "name"); //01 update krna hai when doing merge
                        }
                        if (db5 != 0)
                        {
                            mq3 = mq3 + "/" + fgen.seek_iname_dt(dt3, "type1='04'", "name"); //04 krna hai yaha
                        }
                        if (db6 != 0)
                        {
                            mq3 = mq3 + "/" + fgen.seek_iname_dt(dt3, "type1='08'", "name");
                        }
                        dr1["unit"] = mq3;
                        dt2.Rows.Add(dr1);
                    }
                }
                if (dt2.Rows.Count > 0)
                {
                    dt2.TableName = "prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt2, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Exp_Ord_Rcv_Shpmnt", "Exp_Ord_Rcv_Shpmnt", dsRep, header_n);
                }
                break;

            case "F49233":
                header_n = "Item Wise Sale Report";
                SQuery = "select '" + header_n + "' as header,'" + fromdt + "' as fromdt,'" + todt + "' as todt,B.MKTGGRP ,c.iname,sum(A.IQTYOUT) as qty, sum(A.IAMOUNT) as basic FROM IVOUCHER A ,item C,famst b WHERE TRIM(A.iCODE) = TRIM(C.iCODE) AND TRIM(A.ACODE)=TRIM(B.ACODE) AND A.BRANCHCD='" + frm_mbr + "' AND A.TYPE LIKE '4%' AND A.VCHDATE " + xprdRange + " AND B.MKTGGRP='-' group by c.iname ,B.MKTGGRP ORDER BY C.INAME ";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Item_Wise_Salerep", "Item_Wise_Salerep", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F49204":
                header_n = "Qty Wise Export Order Analysis";
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                mq0 = party_cd;
                if (mq0.Length <= 1)
                {
                    cond = " and trim(a.acode) like '%'";
                }
                else
                {
                    cond = " and trim(a.acode) in (" + party_cd + ")";
                }
                SQuery = "select '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,'" + header_n + "' as header, party_name,sum(april) as april,sum(may) as may,sum(june) as june,sum(july) as july, sum(august) as aug,sum(sept) as sept,sum(october) as oct,sum(november) as nov,sum(december) as dec,sum(jan) as jan,sum(feb) as feb,sum(march) as mar from (SELECT b.aname as party_name ,decode(to_char(a.VCHDATE,'mm'),'04',a.IQTYOUT,'0') as APRIL,decode(to_char(a.VCHDATE,'mm'),'05',a.IQTYOUT,'0') as MAY,decode(to_char(a.VCHDATE,'mm'),'06',a.IQTYOUT,'0') as JUNE,decode(to_char(a.VCHDATE,'mm'),'07',a.IQTYOUT,'0') as JULY,decode(to_char(a.VCHDATE,'mm'),'08',a.IQTYOUT,'0') as AUGUST,decode(to_char(a.VCHDATE,'mm'),'09',a.IQTYOUT,'0') as SEPT,decode(to_char(a.VCHDATE,'mm'),'10',a.IQTYOUT,'0') as OCTOBER,decode(to_char(a.VCHDATE,'mm'),'11',a.IQTYOUT,'0') as NOVEMBER,decode(to_char(a.VCHDATE,'mm'),'12',a.IQTYOUT,'0') as DECEMBER,decode(to_char(a.VCHDATE,'mm'),'01',a.IQTYOUT,'0') as JAN,decode(to_char(a.VCHDATE,'mm'),'02',a.IQTYOUT,'0') as FEB,decode(to_char(a.VCHDATE,'mm'),'03',a.IQTYOUT,'0') as MARCH FROM IVOUCHER a ,famst b   WHERE trim(a.acode)=trim(b.acode) and  a.BRANCHCD='" + frm_mbr + "' AND a.TYPE in (" + mq2 + ") " + cond + " and b.country like '" + part_cd + "%'  AND a.VCHDATE " + xprdRange + " ) group by party_name order by party_name";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Party_Qty_Exp_Analysis", "Party_Qty_Exp_Analysis", dsRep, header_n);
                }
                break;

            case "F49205":
                header_n = "Value Wise Export Order Analysis";
                mq2 = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                mq0 = party_cd;
                if (mq0.Length <= 1)
                {
                    cond = " and trim(a.acode) like '%'";
                }
                else
                {
                    cond = " and trim(a.acode) in (" + party_cd + ")";
                }
                SQuery = "select '" + fromdt + "' as FRMDATE,'" + todt + "' AS TODATE,'" + header_n + "' as header, party_name,sum(april) as april, sum(may) as may,sum(june) as june ,sum(july) as july,sum(august) as aug,sum(sept) as sept ,sum(october) as oct,sum(november) as nov,sum(december) as dec,sum(jan) as jan,sum(feb) as feb,sum(march) as mar from (SELECT b.aname as party_name ,decode(to_char(a.VCHDATE,'mm'),'04',(a.IQTYOUT*a.iqty_chlwt),'0') as APRIL,decode(to_char(a.VCHDATE,'mm'),'05',(a.IQTYOUT*a.iqty_chlwt),'0') as MAY,decode(to_char(a.VCHDATE,'mm'),'06',(a.IQTYOUT*a.iqty_chlwt),'0') as JUNE,decode(to_char(a.VCHDATE,'mm'),'07',(a.IQTYOUT*a.iqty_chlwt),'0') as JULY,decode(to_char(a.VCHDATE,'mm'),'08',(a.IQTYOUT*a.iqty_chlwt),'0') as AUGUST,decode(to_char(a.VCHDATE,'mm'),'09',(a.IQTYOUT*a.iqty_chlwt),'0') as SEPT,decode(to_char(a.VCHDATE,'mm'),'10',(a.IQTYOUT*a.iqty_chlwt),'0') as OCTOBER,decode(to_char(a.VCHDATE,'mm'),'11',(a.IQTYOUT*a.iqty_chlwt),'0') as NOVEMBER,decode(to_char(a.VCHDATE,'mm'),'12',(a.IQTYOUT*a.iqty_chlwt),'0') as DECEMBER,decode(to_char(a.VCHDATE,'mm'),'01',(a.IQTYOUT*a.iqty_chlwt),'0') as JAN,decode(to_char(a.VCHDATE,'mm'),'02',(a.IQTYOUT*a.iqty_chlwt),'0') as FEB,decode(to_char(a.VCHDATE,'mm'),'03',(a.IQTYOUT*a.iqty_chlwt),'0') as MARCH FROM IVOUCHER a ,famst b   WHERE trim(a.acode)=trim(b.acode) and  a.BRANCHCD='" + frm_mbr + "' AND a.TYPE in (" + mq2 + ")  " + cond + " and b.country like '" + part_cd + "%' AND a.VCHDATE " + xprdRange + " ) group by party_name order by party_name";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dsRep = new DataSet();
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Party_Value_Exp_Analysis", "Party_Value_Exp_Analysis", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F49149":
                header_n = "Sale Summary For R/M Details";
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                mq6 = part_cd;
                mq7 = party_cd;
                string cond1 = "";
                #region
                if (mq6.Length <= 1)
                {
                    cond = " and trim(b.vchnum) like '%'";
                }
                else
                {
                    cond = " and trim(b.vchnum) in (" + part_cd + ")";
                }
                if (mq7.Length <= 1)
                {
                    cond1 = " and trim(b.acode) like '%'";
                }
                else
                {
                    cond1 = " and trim(b.acode) in (" + party_cd + ")";
                }
                #region
                mq0 = "";
                mq0 = "select rm_name,qty ,rownum from (select rm_name,sum(mat_qty) as qty from matl_spec where branchcd='" + frm_mbr + "' and type='4F' and vchdate " + xprdRange + " group by rm_name order by qty desc ) where rownum<=20";
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, mq0);
                dt.Columns.Add("header", typeof(string));
                dt.Columns.Add("column", typeof(string));
                dt1 = new DataTable();
                dt1.Columns.Add("fromdt", typeof(string));
                dt1.Columns.Add("TODATE", typeof(string));

                dt1.Columns.Add("INV_NO", typeof(string));
                dt1.Columns.Add("CUSTOMER", typeof(string));
                dt1.Columns.Add("CONSIGNEE", typeof(string));
                dt1.Columns.Add("VFC", typeof(double));
                dt1.Columns.Add("RATE", typeof(double));
                dt1.Columns.Add("INR(CIF)", typeof(double));
                dt1.Columns.Add("QTY", typeof(double));
                i0 = 1;
                for (int i = 0; i < 20; i++)
                {
                    dt1.Columns.Add("hd" + i0, typeof(string));
                    dt1.Columns.Add("num" + i0, typeof(double));
                    i0++;
                }

                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    try
                    {
                        dt.Rows[k]["header"] = "hd".Trim() + dt.Rows[k]["rownum"].ToString().Trim();
                        dt.Rows[k]["column"] = "num".Trim() + dt.Rows[k]["rownum"].ToString().Trim();
                    }
                    catch
                    {
                    }
                }
                SQuery = "SELECT trim(b.branchcd)||trim(b.type)||trim(b.tc_no)||to_char(b.refdate,'dd/mm/yyyy')||trim(b.acode) as fstr,TRIM(b.vchnum) as invno,TO_CHAR(B.VCHDATE,'DD/MM/YYYY') AS INVDATE,TRIM(b.tc_no) as vchnum,to_char(b.refdate,'dd/mm/yyyy') as ddate,TRIM(b.acode) AS ACODE,sum(b.iqtyout) as iqtyout,TRIM(C.ANAME) AS ANAME,b.acpt_ud  as rate,sum(b.iqtyout*b.iqty_chlwt) as vfc,sum(b.iqtyout*b.iqty_chlwt*b.acpt_ud) as inrc FROM IVOUCHERP B, FAMST C WHERE  TRIM(b.ACODE)=TRIM(C.ACODE) AND b.BRANCHCD='" + frm_mbr + "' AND b.TYPE='4F' AND b.refDATE " + xprdRange + " " + cond1 + "  " + cond + " GROUP BY TO_CHAR(B.VCHDATE,'DD/MM/YYYY'),TRIM(b.vchnum),TRIM(b.tc_no),to_char(b.refdate,'dd/mm/yyyy'),TRIM(b.acode),TRIM(C.ANAME),b.acpt_ud, trim(b.branchcd)||trim(b.type)||trim(b.tc_no)||to_char(b.refdate,'dd/mm/yyyy')||trim(b.acode)  order by invno";
                dt2 = new DataTable();
                dt2 = fgen.getdata(frm_qstr, frm_cocd, SQuery);

                mq4 = "select trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(acode) as fstr,rm_name,sum(mat_qty) as mat_qty from matl_spec where trim(branchcd)='" + frm_mbr + "' and trim(type)='4F' and vchdate " + xprdRange + " group by trim(branchcd)||trim(type)||trim(vchnum)||to_char(vchdate,'dd/mm/yyyy')||trim(acode),rm_name order by mat_qty";
                dt3 = new DataTable();
                dt3 = fgen.getdata(frm_qstr, frm_cocd, mq4);
                mq5 = "SELECT TRIM(b.VCHNUM) AS VCHNUM,TO_CHAR(b.VCHDATE,'DD/MM/YYYY') as VCHDATE,TRIM(b.ACODE) AS ACODE,TRIM(c.ACODE) AS CCODE,TRIM(c.ANAME) AS CONSIGNEE_NAME  FROM SALEP b,CSMST c WHERE TRIM(b.CSCODE) =TRIM(c.ACODE) and b.branchcd='" + frm_mbr + "' and b.type='4F' AND B.VCHDATE  " + xprdRange + "  " + cond + "";
                dt4 = new DataTable();
                dt4 = fgen.getdata(frm_qstr, frm_cocd, mq5);
                mq6 = "select vchnum,vchdate,acode,iqty_chlwt from ivoucherp where branchcd='" + frm_mbr + "' and type='4F' and  icode like '5%' AND VCHDATE " + xprdRange + "";
                dt5 = new DataTable();
                dt5 = fgen.getdata(frm_qstr, frm_cocd, mq6);

                DataRow drrow1;
                if (dt2.Rows.Count > 0)
                {
                    DataView view1im = new DataView(dt2);
                    dt6 = new DataTable();
                    dt6 = view1im.ToTable(true, "fstr");
                    foreach (DataRow dr0 in dt6.Rows)
                    {
                        DataView view1 = new DataView(dt2, "fstr='" + dr0["fstr"] + "'", "", DataViewRowState.CurrentRows);
                        dticode = new DataTable();
                        dticode = view1.ToTable();

                        DataView view2 = new DataView(dt3, "fstr='" + dr0["fstr"] + "'", "", DataViewRowState.CurrentRows);
                        dticode2 = new DataTable();
                        dticode2 = view2.ToTable();
                        for (int i = 0; i < dticode.Rows.Count; i++)
                        {
                            drrow1 = dt1.NewRow();

                            drrow1["fromdt"] = fromdt;
                            drrow1["TODATE"] = todt;
                            drrow1["INV_NO"] = dticode.Rows[i]["invno"].ToString().Trim();
                            drrow1["CUSTOMER"] = dticode.Rows[i]["ANAME"].ToString().Trim() + "/" + " " + fgen.seek_iname_dt(dt4, "VCHNUM='" + dticode.Rows[i]["invno"].ToString().Trim() + "' AND VCHDATE='" + dticode.Rows[i]["INVDATE"].ToString().Trim() + "' AND ACODE='" + dticode.Rows[i]["ACODE"].ToString().Trim() + "'", "CONSIGNEE_NAME");
                            //drrow1["CONSIGNEE"] = fgen.seek_iname_dt(dt4, "VCHNUM='" + dticode.Rows[i]["invno"].ToString().Trim() + "' AND VCHDATE='" + dticode.Rows[i]["INVDATE"].ToString().Trim() + "' AND ACODE='" + dticode.Rows[i]["ACODE"].ToString().Trim() + "'", "CONSIGNEE_NAME");
                            db9 = fgen.make_double(fgen.seek_iname_dt(dt5, "VCHNUM='" + dticode.Rows[i]["invno"].ToString().Trim() + "' AND VCHDATE='" + dticode.Rows[i]["INVDATE"].ToString().Trim() + "'AND ACODE='" + dticode.Rows[i]["ACODE"].ToString().Trim() + "'", "IQTY_CHLWT"));
                            drrow1["VFC"] = fgen.make_double(dticode.Rows[i]["vfc"].ToString().Trim()) + db9;
                            drrow1["RATE"] = fgen.make_double(dticode.Rows[i]["rate"].ToString().Trim());
                            //drrow1["INR(CIF)"] = fgen.make_double(dticode.Rows[i]["inrc"].ToString().Trim());                            
                            drrow1["INR(CIF)"] = fgen.make_double(drrow1["VFC"].ToString()) * fgen.make_double(drrow1["RATE"].ToString());
                            drrow1["QTY"] = fgen.make_double(dticode.Rows[i]["iqtyout"].ToString().Trim());

                            for (int k = 0; k < dticode2.Rows.Count; k++)
                            {
                                mq8 = dticode2.Rows[k]["rm_name"].ToString().Trim();
                                mq9 = fgen.seek_iname_dt(dt, "rm_name='" + mq8 + "'", "column");
                                mq10 = fgen.seek_iname_dt(dt, "rm_name='" + mq8 + "'", "header");
                                drrow1[mq9] = fgen.make_double(dticode2.Rows[k]["mat_qty"].ToString().Trim());
                            }
                            i0 = 1;
                            for (int l = 0; l < 20; l++)
                            {
                                try
                                {
                                    drrow1["hd" + i0] = dt.Rows[l]["rm_name"].ToString().Trim();
                                    i0++;
                                }
                                catch { }
                            }
                            dt1.Rows.Add(drrow1);
                        }
                    }
                #endregion
                    dsRep = new DataSet();
                    dt1.TableName = "Prepcur";
                    dsRep.Tables.Add(dt1);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Rm_Detailed", "Rm_Detailed", dsRep, header_n);
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