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

public partial class smktg_reps : System.Web.UI.Page
{
    ReportDocument repDoc = new ReportDocument();
    string frm_mbr, frm_vty, frm_url, frm_qstr, frm_cocd, frm_uname, frm_myear, SQuery, frm_rptName, str, xprdRange, frm_cDt1, fpath, frm_cDt2, col1, printBar = "N", frm_PageName, frm_ulvl, xprdRange1, party_cd, part_cd;
    string frm_FileName = "", frm_formID = "", frm_UserID, fromdt, todt, header_n, cond = "", pdfView = "", data_found = "";
    string MV_CLIENT_GRP = "";
    fgenDB fgen = new fgenDB();
    private DataSet DsImages = new DataSet();
    FileStream FilStr = null; BinaryReader BinRed = null;
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
                    xprdRange1 = "between to_date('" + frm_cDt1 + "','dd/MM/yyyy') and to_Date('" + fromdt + "','dd/MM/yyyy')-1";

                    hfhcid.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "REPID");
                    hfval.Value = fgenMV.Fn_Get_Mvar(frm_qstr, "U_COL1");

                    pdfView = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PDFVIEW");
                    fgenMV.Fn_Set_Mvar(frm_qstr, "U_PDFVIEW", "-");
                }
                else Response.Redirect("~/login.aspx");
            }
            //if (!Page.IsPostBack)
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
        string chk_opt = "";
        string doc_GST = "";
        MV_CLIENT_GRP = fgenMV.Fn_Get_Mvar(frm_qstr, "U_CLIENT_GRP");
        chk_opt = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT_PW where branchcd='" + frm_mbr + "' and OPT_ID='W2027'", "fstr");
        
        
        if (chk_opt == "Y")
        //Member GCC Country
        {
            doc_GST = "GCC";
        }

        switch (iconID)
        {
            case "F1015":
                #region sale SCH
                SQuery = "select d.mthname, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN,c.gst_no, B.INAME,A.* from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum)  AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum ";
                col1 = "NO";
                if (col1 == "YES")
                {
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_, C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN,c.gst_no, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,Round((DAY1/1000),1) as  DAY1 , round((A.DAY2/1000),1) AS DAY2,round((A.DAY3/10000),1) AS DAY3,round((A.DAY4/1000),1) AS DAY4,round((A.DAY5/1000),1) AS DAY5,round((A.DAY6/1000),1)  AS DAY6,round((A.DAY7/1000),1) AS DAY7,round((A.DAY8/1000),1) AS DAY8,round((A.DAY9/1000),1) AS DAY9,round((A.DAY10/1000),1) AS DAY10,round((A.DAY11/1000),1) AS DAY11,round((A.DAY12/1000),1) AS DAY12,round((A.DAY13/1000),1) AS DAY13,round((A.DAY14/1000),1) AS DAY14,round((A.DAY15/1000),1) AS DAY15,round((A.DAY16/1000),1) AS DAY16,round((A.DAY17/1000),1) AS DAY17,round((A.DAY18/1000),1) AS DAY18,round((A.DAY19/1000),1) AS DAY19,round((A.DAY20/1000),1) AS DAY20,round((A.DAY21/1000),1) AS DAY21,round((A.DAY22/1000),1) AS DAY22,round((A.DAY23/1000),1) AS DAY23,round((A.DAY24/1000),1) AS DAY24,round((A.DAY25/1000),1) AS DAY25,round((A.DAY26/1000),1) AS DAY26,round((A.DAY27/1000),1) AS DAY27,round((A.DAY28/1000),1) AS DAY28,round((A.DAY29/1000),1) AS DAY29,round((A.DAY30/1000),1)  AS DAY30,round((A.DAY31/1000),1) AS DAY31,round((A.TOTAL/1000),1) AS TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT ,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT from schedule A,ITEM B,FAMST C,mths d where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum ";
                }
                if (col1 == "NO")
                {
                    SQuery = "select '" + col1 + "' as col1, d.mthname,to_char(a.vchdate,'YYYY') AS YEAR_,C.ANAME,C.ADDR1,C.ADDR2,C.ADDR3,C.RC_NUM AS TIN,c.gst_no, B.INAME,A.VCHNUM,TO_CHAR(A.VCHDATE,'DD/MM/YYYY') AS VCHDATE,A.ACODE,A.ICODE,DAY1,A.DAY2,A.DAY3,A.DAY4,A.DAY5,A.DAY6,A.DAY7,A.DAY8,A.DAY9,A.DAY10,A.DAY11,A.DAY12,A.DAY13,A.DAY14,A.DAY15,A.DAY16,A.DAY17,A.DAY18,A.DAY19,A.DAY20,A.DAY21,A.DAY22,A.DAY23,A.DAY24,A.DAY25,A.DAY26,A.DAY27,A.DAY28,A.DAY29,A.DAY30,A.DAY31,A.TOTAL,A.SONUM,A.SODATE,A.ENT_BY,A.ENT_DT,A.EDT_BY,A.EDT_DT,A.REMARKS,A.SCH_MON,A.PONUM,A.PODATE,A.APP_BY,A.APP_DT from schedule A,ITEM B,FAMST C,mths d  where  TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) and trim(substr(to_char(vchdate,'dd/mm/yyyy'),4,2))=trim(d.mthnum)  AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum ";
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

            case "F47101":
            case "F47106":
            case "F47106W":
            case "F49101":
            case "F45109":
            case "F49106":
            case "F1005":
            case "FB3056":
            case "FB3056Q":
            case "FB3056W":
            case "FB3056E":
            case "FB3056P":
                string so_title;
                string so_tbl;
                so_title = "Sales Order";
                so_tbl = "SOMAS";
                if (frm_cocd == "HPPI" || doc_GST == "GCC" || MV_CLIENT_GRP == "SG_TYPE") frm_rptName = "std_so_UAE";
                if (iconID == "F47101" || iconID == "F49101")
                {
                    so_title = "Master Sales Order";
                    so_tbl = "SOMASM";
                }

                if (iconID == "F45109")
                {
                    so_title = "Quotation";
                    so_tbl = "SOMASQ";
                    if (frm_cocd == "HPPI" || doc_GST == "GCC" || MV_CLIENT_GRP == "SG_TYPE") frm_rptName = "std_QA_UAE";
                }

                if (frm_rptName == "0")
                {
                    frm_rptName = "STD_SO";
                    if (frm_cocd.Trim() == "KRSM")
                    {
                        frm_rptName = "STD_SO_KRS";
                    }
                }
                if (iconID == "F47106W" || iconID == "FB3056W")
                {
                    so_title = "Work Order";
                    frm_rptName = "STD_WO";
                    if (frm_cocd.Trim() == "KRSM")
                    {
                        frm_rptName = "STD_WO_KRS";
                        so_title = "INTERNAL PURCHASE SLIP";
                    }
                }
                
                if (iconID == "F49106")
                {
                    //if (frm_cocd == "HPPI" || doc_GST == "GCC" || MV_CLIENT_GRP == "SG_TYPE")
                        frm_rptName = "std_pi_UAE";                    
                }

                if (frm_cocd == "KRS") frm_rptName = "std_sokrs";
                if (iconID == "FB3056Q")
                {
                    frm_rptName = "SOQ";
                    if (frm_cocd == "KRS") frm_rptName = "SOQ_KRS";
                }
                if (iconID == "FB3056E") frm_rptName = "SOE";
                if (iconID == "FB3056W" || iconID == "FB3056WL") frm_rptName = "SOW";
                if (iconID == "FB3056P") frm_rptName = "Praposel";
                #region S.O.
                opt = fgen.getOption(frm_qstr, frm_cocd, "W0018", "OPT_ENABLE");
                string chk_opts="";
                if (iconID == "F49106")
                {
                    SQuery = "Select a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr, G.ANAME AS CONSNAME,G.ADDR1 AS COS_ADR1,G.ADDR2 AS CONS_aDR2,G.ADDR3 AS CONS_aDR3,G.TELNUM AS CONS_TEL,G.GIRNO AS CONS_PAN,SUBSTR(G.GST_NO,0,2) AS CONS_CODE,G.EMAIL AS CSMAIL,G.TYPE AS CONS_TYPE,G.STATEN AS CONS_STATE, trim(G.GST_NO) AS CONS_GST,'SOMAS' as TAB_NAME, 'Order NO' as h1,'Order Dt' as h2, c.cpartno AS IPART, B.ADDR1,B.ADDR2,B.ADDR3,substr(b.gst_no,0,2) as statecode,b.staten,b.gst_no,b.girno as pan1,C.UNIT AS ITEM_UNIT,B.ANAME,C.ICODE AS ITEM_CODE,C.INAME AS ITEM_NAME,c.hscode, t.name as So_Type,A.* from somas a LEFT OUTER JOIN CSMST G ON TRIM(A.CSCODE)=TRIM(G.ACODE),famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(t.type1) and t.id='V' and TRIM(A.BRANCHCD)='" + frm_mbr + "'  and a.type='" + frm_vty + "' and TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY') in (" + barCode + ") order by a.ordno";
                }
                else
                {
                    SQuery = "Select a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'" + so_tbl + "' as TAB_NAME,'" + so_title + "' as so_title,'SO Number' as h1,'SO Dated' as h2,G.ANAME AS CONSNAME,G.ADDR1 AS COS_ADR1,G.ADDR2 AS CONS_aDR2,G.ADDR3 AS CONS_aDR3,G.TELNUM AS CONS_TEL,G.GIRNO AS CONS_PAN,SUBSTR(G.GST_NO,0,2) AS CONS_CODE,G.EMAIL AS CSMAIL,G.TYPE AS CONS_TYPE,G.STATEN AS CONS_STATE, G.GST_NO AS CONS_GST,'" + so_tbl + "' as TAB_NAME, 'Order NO' as h1,'Order Dt' as h2, c.cpartno AS IPART, B.ADDR1,B.ADDR2,B.ADDR3,/*substr(b.gst_no,0,2)*/ B.STAFFCD as statecode,b.staten,b.gst_no,b.girno as pan1,C.UNIT AS ITEM_UNIT,B.ANAME,C.ICODE AS ITEM_CODE,C.INAME AS ITEM_NAME,c.hscode, t.name as So_Type,A.* from " + so_tbl + " a LEFT OUTER JOIN CSMST G ON TRIM(A.CSCODE)=TRIM(G.ACODE),famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(t.type1) and t.id='V' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in (" + barCode + ") order by a.orddt,a.ordno,a.srno";
                    if (frm_cocd.Trim()=="KRSM")
                    {
                        SQuery = "Select a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr,'" + opt + "' AS btoprint,'" + so_tbl + "' as TAB_NAME,'" + so_title + "' as so_title,'SO Number' as h1,'SO Dated' as h2" +
                            ",NVL(G.ANAME,B.ANAME) AS CONSNAME,NVL(G.ADDR1,B.ADDR1) AS COS_ADR1,b.BUYCODE as oldcode,b.person as cperson,b.mobile as ctel,b.email as p_email,nvl(b.dlno,'-') as dlno,NVL(G.ADDR2,B.ADDR2) AS CONS_aDR2,NVL(G.ADDR3,B.ADDR3) AS CONS_aDR3,G.TELNUM AS CONS_TEL,NVL(G.GIRNO, b.girno) AS CONS_PAN, SUBSTR(NVL(G.GST_NO, B.GST_NO), 0, 2) AS CONS_CODE, NVL(G.EMAIL, B.EMAIL) AS CSMAIL, NVL(G.TYPE, '-') AS CONS_TYPE, NVL(G.STATEN, B.STATEN) AS CONS_STATE, NVL(G.GST_NO, B.GST_NO) AS CONS_GST" +
                            ",'" + so_tbl + "' as TAB_NAME, 'Order NO' as h1,'Order Dt' as h2, c.cpartno AS IPART, B.ADDR1,B.ADDR2,B.ADDR3,/*substr(b.gst_no,0,2)*/ SUBSTR(B.GST_NO, 0, 2) as statecode,b.staten,b.gst_no,b.girno as pan1,C.UNIT AS ITEM_UNIT,B.ANAME,C.ICODE AS ITEM_CODE,a.desc_ as iremark,C.INAME AS ITEM_NAME,c.hscode, t.name as So_Type,A.* from " + so_tbl + " a LEFT OUTER JOIN CSMST G ON TRIM(A.CSCODE)=TRIM(G.ACODE),famst b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(t.type1) and t.id='V' AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.ordno)||TO_CHAR(A.orddt,'DD/MM/YYYY') in (" + barCode + ") order by a.orddt,a.ordno,a.srno";
                    }
                }
                if (iconID == "F45109" || iconID== "F49106")
                {
                    string fam_tbl = "famst";
                    chk_opts = fgen.seek_iname(frm_qstr, frm_cocd, "select trim(upper(OPT_ENABLE)) as fstr from FIN_RSYS_OPT where OPT_ID='W0063'", "fstr");
                    if (chk_opts == "Y") { fam_tbl = "wbvu_fam_crm"; }
                    SQuery = "Select a.branchcd||a.type||Trim(A.ordno)||to_Char(A.orddt,'yyyymmdd') as fstr, G.ANAME AS CONSNAME,'" + so_title + "' as so_title,G.ADDR1 AS COS_ADR1,G.ADDR2 AS CONS_aDR2,G.ADDR3 AS CONS_aDR3,G.TELNUM AS CONS_TEL,G.GIRNO AS CONS_PAN,SUBSTR(G.GST_NO,0,2) AS CONS_CODE,G.EMAIL AS CSMAIL,G.TYPE AS CONS_TYPE,G.STATEN AS CONS_STATE, trim(G.GST_NO) AS CONS_GST,'SOMASQ' as TAB_NAME, 'Quote No.' as h1,'Quote Dt' as h2, c.cpartno AS IPART, B.ADDR1,B.ADDR2,B.ADDR3,substr(b.gst_no,0,2) as statecode,b.staten,b.gst_no,b.girno as pan1,C.UNIT AS ITEM_UNIT,B.ANAME,C.ICODE AS ITEM_CODE,C.INAME AS ITEM_NAME,c.hscode, t.name as So_Type,A.* from somasq a LEFT OUTER JOIN CSMST G ON TRIM(A.CSCODE)=TRIM(G.ACODE)," + fam_tbl +" b,item c,type t where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode)  and trim(a.type)=trim(t.type1) and t.id='V' and TRIM(A.BRANCHCD)='" + frm_mbr + "'  and a.type='" + frm_vty + "' and TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY') in (" + barCode + ") order by a.ordno";
                }
                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    //BarCode adding
                    dt = fgen.addBarCode(dt, "fstr", true);
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));

                    SQuery = "SELECT DISTINCT COL1 AS POTERMS,SRNO FROM DOCTERMS WHERE BRANCHCD='" + frm_mbr + "' AND TYPE='70' AND DOCTYPE='SO' ORDER BY SRNO";
                    dt1 = new DataTable();
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dt1.TableName = "type1";
                    mq10 = "";
                    dt3 = new DataTable();
                    mdr = null;
                    dt3.Columns.Add("poterms", typeof(string));
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        mq10 += dt1.Rows[i]["POTERMS"].ToString().Trim() + Environment.NewLine;
                    }
                    mdr = dt3.NewRow();
                    mdr["poterms"] = mq10;
                    dt3.Rows.Add(mdr);
                    dt3.TableName = "type1";
                    if (frm_cocd.Trim() == "KRSM")
                    {
                        Print_Report_BYDS(frm_cocd, frm_mbr, "std_so_krs", frm_rptName, dsRep, "S.O. Entry Report", "Y");
                    }
                    else
                    {
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_so", frm_rptName, dsRep, "S.O. Entry Report", "Y");
                    }
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F47141":
                party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTYCODE");
                part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_PARTCODE");
                party_cd = frm_ulvl == "M" ? frm_uname : party_cd;
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

            case "F47142":
                header_n = "Pending S.O. Register";
                #region
                frm_vty = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col1");
                col1 = frm_ulvl == "M" ? frm_uname : "%";
                if (col1 == "M")
                {
                    SQuery = "SELECT '" + header_n + "' AS HEADER, TO_CHAR(A.PORDDT,'DD/MM/YYYY') AS PODT,TO_CHAR(A.del_date,'DD/MM/YYYY') AS delDT,'" + fromdt + "' as frmdt,'" + todt + "' as todt,round(a.srate*a.bal_Qty,2) as Bal_Val,A.*,b.aname,c.unit FROM wbvu_pending_so A,famst b,item c where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in (" + frm_vty + ") AND A.ORDDT " + xprdRange + "  and trim(a.acode) like '" + col1 + "%' ORDER BY A.ACODE,A.ICODE";
                }
                else
                {
                    party_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col2");
                    part_cd = fgenMV.Fn_Get_Mvar(frm_qstr, "U_Col3");
                    if (party_cd == "%") SQuery = "SELECT '" + header_n + "' AS HEADER, TO_CHAR(A.PORDDT,'DD/MM/YYYY') AS PODT,TO_CHAR(A.del_date,'DD/MM/YYYY') AS delDT,'" + fromdt + "' as frmdt,'" + todt + "' as todt,round(a.srate*a.bal_Qty,2) as Bal_Val,A.*,b.aname,c.unit FROM wbvu_pending_so A,famst b,item c where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in (" + frm_vty + ") AND A.ORDDT " + xprdRange + "  and trim(a.acode) like '" + party_cd + "' and trim(a.icode) in (" + part_cd + ") ORDER BY A.ACODE,A.ICODE";
                    if (part_cd == "%") SQuery = "SELECT '" + header_n + "' AS HEADER, TO_CHAR(A.PORDDT,'DD/MM/YYYY') AS PODT,TO_CHAR(A.del_date,'DD/MM/YYYY') AS delDT,'" + fromdt + "' as frmdt,'" + todt + "' as todt,round(a.srate*a.bal_Qty,2) as Bal_Val,A.*,b.aname,c.unit FROM wbvu_pending_so A,famst b,item c where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in (" + frm_vty + ") AND A.ORDDT " + xprdRange + "  and trim(a.acode) in (" + party_cd + ") and trim(a.icode) like '" + part_cd + "' ORDER BY A.ACODE,A.ICODE";
                    if (party_cd == "%" && part_cd == "%") SQuery = "SELECT '" + header_n + "' AS HEADER, TO_CHAR(A.PORDDT,'DD/MM/YYYY') AS PODT,TO_CHAR(A.del_date,'DD/MM/YYYY') AS delDT,'" + fromdt + "' as frmdt,'" + todt + "' as todt,round(a.srate*a.bal_Qty,2) as Bal_Val,A.*,b.aname,c.unit FROM wbvu_pending_so A,famst b,item c where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in (" + frm_vty + ") AND A.ORDDT " + xprdRange + "  and trim(a.acode) like '" + party_cd + "' and trim(a.icode) like '" + part_cd + "' ORDER BY A.ACODE,A.ICODE";
                    if (party_cd != "%" && part_cd != "%") SQuery = "SELECT '" + header_n + "' AS HEADER, TO_CHAR(A.PORDDT,'DD/MM/YYYY') AS PODT,TO_CHAR(A.del_date,'DD/MM/YYYY') AS delDT,'" + fromdt + "' as frmdt,'" + todt + "' as todt,round(a.srate*a.bal_Qty,2) as Bal_Val,A.*,b.aname,c.unit FROM wbvu_pending_so A,famst b,item c where trim(a.icode)=trim(c.icode) and trim(a.acode)=trim(b.acode) and A.BRANCHCD='" + frm_mbr + "' AND A.TYPE in (" + frm_vty + ") AND A.ORDDT " + xprdRange + "  and trim(a.acode) in (" + party_cd + ") and trim(a.icode) in (" + part_cd + ") ORDER BY A.ACODE,A.ICODE";
                }
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
            case "F47222":
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

            case "F47223":
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

            case "F47226":
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

            case "F47227":
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

            case "F47228":
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

            case "F47229":
                break;

            case "F47230":
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

            case "F47231":
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

            case "F47232":
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

            case "F47233":
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
                break;

            case "F47224":
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

            case "F47225":
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

            case "F47319"://Costing Sheet print for AMAR
                #region
                dsRep = new DataSet();
                dt = new DataTable(); dt1 = new DataTable(); dt2 = new DataTable();
                //for 1 row
                dt2.Columns.Add("grid_icode", typeof(string));
                dt2.Columns.Add("grid_name", typeof(string));
                dt2.Columns.Add("Recovery", typeof(double));
                dt2.Columns.Add("req_kg", typeof(double));
                dt2.Columns.Add("grid_rate", typeof(double));
                dt2.Columns.Add("grid_cost", typeof(double));
                dt2.Columns.Add("grid_pigiron", typeof(double));
                dt2.Columns.Add("grid_return", typeof(double));
                dt2.Columns.Add("grid_req", typeof(double));
                dt2.Columns.Add("grid_diff", typeof(double));
                //for 2 row
                dt2.Columns.Add("grid_icode1", typeof(string));
                dt2.Columns.Add("grid_name1", typeof(string));
                dt2.Columns.Add("Recovery1", typeof(double));
                dt2.Columns.Add("req_kg1", typeof(double));
                dt2.Columns.Add("grid_rate1", typeof(double));
                dt2.Columns.Add("grid_cost1", typeof(double));
                dt2.Columns.Add("grid_pigiron1", typeof(double));
                dt2.Columns.Add("grid_return1", typeof(double));
                dt2.Columns.Add("grid_req1", typeof(double));
                dt2.Columns.Add("grid_diff1", typeof(double));
                // for 3 row
                dt2.Columns.Add("grid_icode2", typeof(string));
                dt2.Columns.Add("grid_name2", typeof(string));
                dt2.Columns.Add("Recovery2", typeof(double));
                dt2.Columns.Add("req_kg2", typeof(double));
                dt2.Columns.Add("grid_rate2", typeof(double));
                dt2.Columns.Add("grid_cost2", typeof(double));
                dt2.Columns.Add("grid_pigiron2", typeof(double));
                dt2.Columns.Add("grid_return2", typeof(double));
                dt2.Columns.Add("grid_req2", typeof(double));
                dt2.Columns.Add("grid_diff2", typeof(double));
                // for 4 row
                dt2.Columns.Add("grid_icode3", typeof(string));
                dt2.Columns.Add("grid_name3", typeof(string));
                dt2.Columns.Add("Recovery3", typeof(double));
                dt2.Columns.Add("req_kg3", typeof(double));
                dt2.Columns.Add("grid_rate3", typeof(double));
                dt2.Columns.Add("grid_cost3", typeof(double));
                dt2.Columns.Add("grid_pigiron3", typeof(double));
                dt2.Columns.Add("grid_return3", typeof(double));
                dt2.Columns.Add("grid_req3", typeof(double));
                dt2.Columns.Add("grid_diff3", typeof(double));
                // for 5 row
                dt2.Columns.Add("grid_icode4", typeof(string));
                dt2.Columns.Add("grid_name4", typeof(string));
                dt2.Columns.Add("Recovery4", typeof(double));
                dt2.Columns.Add("req_kg4", typeof(double));
                dt2.Columns.Add("grid_rate4", typeof(double));
                dt2.Columns.Add("grid_cost4", typeof(double));
                dt2.Columns.Add("grid_pigiron4", typeof(double));
                dt2.Columns.Add("grid_return4", typeof(double));
                dt2.Columns.Add("grid_req4", typeof(double));
                dt2.Columns.Add("grid_diff4", typeof(double));

                header_n = "Costing Sheet";
                SQuery = "select '" + header_n + "' as header,b.aname,c.iname,c.cpartno,c.unit,a.* from wb_cacost a,famst b,item c where trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and a.branchcd||trim(a.type)||trim(A.vchnum)||to_Char(a.vchdate,'dd/mm/yyyy')='" + barCode + "'";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    //for dynamic column
                    SQuery = "SELECT trim(grid_icode) as icode,grid_ferro,grid_rec,grid_reqkg,grid_rate,grid_cost,grid_pigiron,grid_contri as return,grid_req,grid_diff FROM wb_cacost where branchcd||trim(type)||trim(vchnum)||to_Char(vchdate,'dd/mm/yyyy')='" + barCode + "'";
                    dt1 = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                    dr1 = dt2.NewRow();
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            dr1["grid_icode"] = dt1.Rows[i]["icode"].ToString().Trim();
                            dr1["grid_name"] = dt1.Rows[i]["grid_ferro"].ToString().Trim();
                            dr1["Recovery"] = dt1.Rows[i]["grid_rec"].ToString().Trim();
                            dr1["req_kg"] = dt1.Rows[i]["grid_reqkg"].ToString().Trim();
                            dr1["grid_rate"] = dt1.Rows[i]["grid_rate"].ToString().Trim();
                            dr1["grid_cost"] = dt1.Rows[i]["grid_cost"].ToString().Trim();
                            dr1["grid_pigiron"] = dt1.Rows[i]["grid_pigiron"].ToString().Trim();
                            dr1["grid_return"] = dt1.Rows[i]["return"].ToString().Trim();
                            dr1["grid_req"] = dt1.Rows[i]["grid_req"].ToString().Trim();
                            dr1["grid_diff"] = dt1.Rows[i]["grid_diff"].ToString().Trim();
                        }
                        if (i == 1)
                        {
                            dr1["grid_icode1"] = dt1.Rows[i]["icode"].ToString().Trim();
                            dr1["grid_name1"] = dt1.Rows[i]["grid_ferro"].ToString().Trim();
                            dr1["Recovery1"] = dt1.Rows[i]["grid_rec"].ToString().Trim();
                            dr1["req_kg1"] = dt1.Rows[i]["grid_reqkg"].ToString().Trim();
                            dr1["grid_rate1"] = dt1.Rows[i]["grid_rate"].ToString().Trim();
                            dr1["grid_cost1"] = dt1.Rows[i]["grid_cost"].ToString().Trim();
                            dr1["grid_pigiron1"] = dt1.Rows[i]["grid_pigiron"].ToString().Trim();
                            dr1["grid_return1"] = dt1.Rows[i]["return"].ToString().Trim();
                            dr1["grid_req1"] = dt1.Rows[i]["grid_req"].ToString().Trim();
                            dr1["grid_diff1"] = dt1.Rows[i]["grid_diff"].ToString().Trim();
                        }
                        if (i == 2)
                        {
                            dr1["grid_icode2"] = dt1.Rows[i]["icode"].ToString().Trim();
                            dr1["grid_name2"] = dt1.Rows[i]["grid_ferro"].ToString().Trim();
                            dr1["Recovery2"] = dt1.Rows[i]["grid_rec"].ToString().Trim();
                            dr1["req_kg2"] = dt1.Rows[i]["grid_reqkg"].ToString().Trim();
                            dr1["grid_rate2"] = dt1.Rows[i]["grid_rate"].ToString().Trim();
                            dr1["grid_cost2"] = dt1.Rows[i]["grid_cost"].ToString().Trim();
                            dr1["grid_pigiron2"] = dt1.Rows[i]["grid_pigiron"].ToString().Trim();
                            dr1["grid_return2"] = dt1.Rows[i]["return"].ToString().Trim();
                            dr1["grid_req2"] = dt1.Rows[i]["grid_req"].ToString().Trim();
                            dr1["grid_diff2"] = dt1.Rows[i]["grid_diff"].ToString().Trim();
                        }
                        if (i == 3)
                        {
                            dr1["grid_icode3"] = dt1.Rows[i]["icode"].ToString().Trim();
                            dr1["grid_name3"] = dt1.Rows[i]["grid_ferro"].ToString().Trim();
                            dr1["Recovery3"] = dt1.Rows[i]["grid_rec"].ToString().Trim();
                            dr1["req_kg3"] = dt1.Rows[i]["grid_reqkg"].ToString().Trim();
                            dr1["grid_rate3"] = dt1.Rows[i]["grid_rate"].ToString().Trim();
                            dr1["grid_cost3"] = dt1.Rows[i]["grid_cost"].ToString().Trim();
                            dr1["grid_pigiron3"] = dt1.Rows[i]["grid_pigiron"].ToString().Trim();
                            dr1["grid_return3"] = dt1.Rows[i]["return"].ToString().Trim();
                            dr1["grid_req3"] = dt1.Rows[i]["grid_req"].ToString().Trim();
                            dr1["grid_diff3"] = dt1.Rows[i]["grid_diff"].ToString().Trim();
                        }
                        if (i == 4)
                        {
                            dr1["grid_icode4"] = dt1.Rows[i]["icode"].ToString().Trim();
                            dr1["grid_name4"] = dt1.Rows[i]["grid_ferro"].ToString().Trim();
                            dr1["Recovery4"] = dt1.Rows[i]["grid_rec"].ToString().Trim();
                            dr1["req_kg4"] = dt1.Rows[i]["grid_reqkg"].ToString().Trim();
                            dr1["grid_rate4"] = dt1.Rows[i]["grid_rate"].ToString().Trim();
                            dr1["grid_cost4"] = dt1.Rows[i]["grid_cost"].ToString().Trim();
                            dr1["grid_pigiron4"] = dt1.Rows[i]["grid_pigiron"].ToString().Trim();
                            dr1["grid_return4"] = dt1.Rows[i]["return"].ToString().Trim();
                            dr1["grid_req4"] = dt1.Rows[i]["grid_req"].ToString().Trim();
                            dr1["grid_diff4"] = dt1.Rows[i]["grid_diff"].ToString().Trim();
                        }
                    }
                    dt2.Rows.Add(dr1);
                    dt2.TableName = "Dynamic";
                    dsRep.Tables.Add(fgen.mTitle(dt2, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "CostSheet_AMAR", "CostSheet_AMAR", dsRep, "Gate Entry Report");
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;

            case "F47321": //form icon id for amar FINAL quotation form
                #region
                dt = new DataTable(); dsRep = new DataSet();
                header_n = "FINAL QUOTATION";
                //SQuery = "SELECT '" + header_n + "' as header, a.branchcd,a.type,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.acode,b.aname,a.icode,c.iname,c.cpartno,a.ciname as matl,a.invno as rfq_mc_no,to_char(a.invdate,'dd/mm/yyyy') as rfq_mc_date,a.inspchg as bop,a.qtyord as tot_tool_cost,a.qtysupp as cast_price,a.qtybal as heat_Treat,a.td as mach_price,a.cd as packaging,a.othamt1 as comp_cost,a.othamt2 as other,a.rlprc as fwd,a.class as payment,a.ord_alert as rm_base,a.pvt_mark as cast_wt,a.co_orig as quotr_validity,c.hscode as delivery,a.desc0 as rmk1,a.desc1 as rmk2,a.desc2 as rmk3,a.desc3 as rmk4,a.desc4 as rmk5,a.desc5 as rmk6,a.desc6 as rmk7,a.desc7 as rmk8,a.desc8 as rmk9,a.desc9 as rmk10,a.Ent_by,a.Ent_Dt,to_char(a.orddt,'yyyymmdd') as vdd   FROM SOMASQ A,famst b,item c WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY')='" + barCode + "'";
                SQuery = "SELECT '" + header_n + "' as header, a.branchcd,a.type,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.acode,b.aname,a.icode,c.iname,c.cpartno,a.ciname as matl,a.invno as rfq_mc_no,to_char(a.invdate,'dd/mm/yyyy') as rfq_mc_date,a.QTYORD as foundry_cost,a.QTYSUPP as mch_cost,a.CD as bop,a.qtybal as tool_cost,  a.irate as cstr_prce,a.TD as heat_trtmnt,a.DELIVERY as mch_price,a.INSPCHG as packg,a.OTHAMT3 as comp_cost,a.OTHAMT2 as paint_cost, a.OTHAMT1 as assemb_cost,a.RLPRC as forwd, a.class as pymt_term,a.ORD_ALERT as rm_base,  a.PVT_MARK as cast_weight,a.CO_ORIG as quote_val,a.HS_CODE as del_term,    a.desc0 as rmk1,a.desc1 as rmk2,a.desc2 as rmk3,a.desc3 as rmk4,a.desc4 as rmk5,a.desc5 as rmk6,a.desc6 as rmk7,a.desc7 as rmk8,a.desc8 as rmk9,a.desc9 as rmk10,a.Ent_by,a.Ent_Dt,to_char(a.orddt,'yyyymmdd') as vdd   FROM SOMASQ A,famst b,item c WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY')='" + barCode + "'";
                SQuery = "SELECT '" + header_n + "' as header, a.branchcd,a.type,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.acode,b.aname,a.icode,c.iname,c.cpartno,a.ciname as matl,a.invno as rfq_mc_no,to_char(a.invdate,'dd/mm/yyyy') as rfq_mc_date,a.QTYORD as foundry_cost,a.QTYSUPP as mch_cost,a.CD as bop,a.qtybal as tool_cost,a.irate as cstr_prce,a.TD as heat_trtmnt,a.DELIVERY as mch_price,a.INSPCHG as packg,a.OTHAMT3 as comp_cost,a.OTHAMT2 as paint_cost, a.OTHAMT1 as assemb_cost,a.RLPRC as forwd, a.class as pymt_term,a.ORD_ALERT as rm_base,  a.PVT_MARK as cast_weight,a.CO_ORIG as quote_val,a.HS_CODE as del_term,a.desc0 as rmk1,a.desc1 as rmk2,a.desc2 as rmk3,a.desc3 as rmk4,a.desc4 as rmk5,a.desc5 as rmk6,a.desc6 as rmk7,a.desc7 as rmk8,a.desc8 as rmk9,a.desc9 as rmk10,a.Ent_by,a.Ent_Dt,to_char(a.orddt,'yyyymmdd') as vdd,trim(a.busi_potent) as child,d.iname as childname,d.cpartno as childpartno,a.basic,a.excise,a.inst1 FROM famst b,item c,SOMASQ A left join item d on trim(a.busi_potent)=trim(d.icode) WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY')='" + barCode + "' order by a.srno";
                SQuery = "SELECT '" + header_n + "' as header, a.branchcd,a.type,a.ordno,to_char(a.orddt,'dd/mm/yyyy') as orddt,a.acode,b.aname,a.icode,c.iname,c.cpartno,a.ciname as matl,a.invno as rfq_mc_no,to_char(a.invdate,'dd/mm/yyyy') as rfq_mc_date,a.QTYORD as foundry_cost,a.QTYSUPP as mch_cost,a.CD as bop,a.qtybal as tool_cost,a.irate as cstr_prce,a.TD as heat_trtmnt,a.DELIVERY as mch_price,a.INSPCHG as packg,a.OTHAMT3 as comp_cost,a.OTHAMT2 as paint_cost, a.OTHAMT1 as assemb_cost,a.RLPRC as forwd, a.class as pymt_term,a.ORD_ALERT as rm_base,  a.PVT_MARK as cast_weight,a.CO_ORIG as quote_val,a.HS_CODE as del_term,a.desc0 as rmk1,a.desc1 as rmk2,a.desc2 as rmk3,a.desc3 as rmk4,a.desc4 as rmk5,a.desc5 as rmk6,a.desc6 as rmk7,a.desc7 as rmk8,a.desc8 as rmk9,a.desc9 as rmk10,a.Ent_by,a.Ent_Dt,to_char(a.orddt,'yyyymmdd') as vdd,trim(a.busi_potent) as child,d.iname as childname,d.cpartno as childpartno,a.basic,a.excise,a.inst1,a.inst2,a.inst3,a.ipack,a.packing FROM famst b,item c,SOMASQ A left join item d on trim(a.busi_potent)=trim(d.icode) WHERE trim(a.acode)=trim(b.acode) and trim(a.icode)=trim(c.icode) and TRIM(A.BRANCHCD)||TRIM(A.TYPE)||TRIM(A.ORDNO)||TO_CHAR(A.ORDDT,'DD/MM/YYYY')='" + barCode + "' order by a.srno";
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "prepcur";
                    dsRep.Tables.Add(fgen.mTitle(dt, repCount));
                    Print_Report_BYDS(frm_cocd, frm_mbr, "Quot_AMAR", "Quot_AMAR", dsRep, header_n);
                }
                else
                {
                    data_found = "N";
                }
                #endregion
                break;
            case "F47108":
                SQuery = "select a.*,b.iname,b.cpartno,b.unit,c.aname,c.acode from SCRATCH2 A,ITEM B,FAMST C where TRIM(A.ICODE)=TRIM(B.ICODE)  AND TRIM(A.ACODE)=TRIM(C.ACODE) AND A.BRANCHCD='" + frm_mbr + "' and A.TYPE ='" + frm_vty + "' and TRIM(a.vchnum)||TO_CHAR(A.vchdate,'DD/MM/YYYY') in (" + barCode + ") order by a.vchdate,a.vchnum ";

                dsRep = new DataSet();
                dt = new DataTable();
                dt = fgen.getdata(frm_qstr, frm_cocd, SQuery);
                if (dt.Rows.Count > 0)
                {
                    dt.TableName = "Prepcur";
                    dsRep.Tables.Add(dt);
                    Print_Report_BYDS(frm_cocd, frm_mbr, "std_disp_tgt", "std_disp_tgt", dsRep, "");
                }
                else
                {
                    data_found = "N";
                }
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
            if (frm_cocd == "KRSM")
            {
                try { pdfno = ds.Tables["Prepcur"].Rows[0]["fstr"].ToString(); } catch { }
                try { pdffirm = ds.Tables["Prepcur"].Rows[0]["Aname"].ToString(); } catch { }
                try { pdfdoc = ds.Tables["Prepcur"].Rows[0]["So_title"].ToString(); } catch { }
                frm_FileName = pdfdoc.Replace(' ', '_') + "__" + pdffirm.Replace(' ', '_') + "__" + pdfno;
            }

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